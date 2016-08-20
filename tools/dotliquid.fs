// (Mostly adapted from Suave.DotLiquid)
module FsBlog.DotLiquid

open System
open System.IO
open System.Text.RegularExpressions
open System.Reflection
open System.Collections.Generic
open System.Collections.Concurrent

open DotLiquid
open DotLiquid.NamingConventions
open Microsoft.FSharp.Reflection

// Use the C# naming convenl tion by default
do Template.NamingConvention <- CSharpNamingConvention()

/// Represents a local file system relative to the specified 'root'
let private localFileSystem root =
  { new DotLiquid.FileSystems.IFileSystem with
      member this.ReadTemplateFile(context, templateName) =
        let templatePath = context.[templateName] :?> string
        let fullPath = Path.Combine(root, templatePath)
        if not (File.Exists(fullPath)) then failwithf "File not found: %s" fullPath
        File.ReadAllText(fullPath) }

/// Given a type which is an F# record containing seq<_>, list<_>, array<_>, option and 
/// other records, register the type with DotLiquid so that its fields are accessible
let private tryRegisterTypeTree ty =
  let registered = Dictionary<_, _>()
  let rec loop ty =
    if not (registered.ContainsKey ty) then
      if FSharpType.IsRecord ty then
        let fields = FSharpType.GetRecordFields ty
        Template.RegisterSafeType(ty, [| for f in fields -> f.Name |])
        for f in fields do loop f.PropertyType
      elif ty.IsGenericType then
        let t = ty.GetGenericTypeDefinition()
        if t = typedefof<seq<_>> || t = typedefof<list<_>>  then
          loop (ty.GetGenericArguments().[0])          
        elif t = typedefof<option<_>> then
          Template.RegisterSafeType(ty, [|"Value"|])
          loop (ty.GetGenericArguments().[0])            
      elif ty.IsArray then          
        loop (ty.GetElementType())
      registered.[ty] <- true
  loop ty

type private Renderer<'T> = string -> 'T-> string

/// Parse the specified template & register the type that we want to use as "model"
let private parseTemplate template typ : Renderer<'M> =
  tryRegisterTypeTree typ
  let t = Template.Parse template
  fun k v -> dict [k, box v] |> Hash.FromDictionary |> t.Render

/// Loads a template & remembers the last write time
/// (so that we can automatically reload the template when file changes)
let private fileTemplate (typ, fileName) = 
  let writeTime = File.GetLastWriteTime fileName
  use file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
  use reader = new StreamReader(file)
  let dotLiquidTemplate = reader.ReadToEnd()
  writeTime, parseTemplate dotLiquidTemplate typ

/// Memoize a function. An item is recomputed when `isValid` returns `false`
let private memoize isValid f =
  let cache = ConcurrentDictionary<_ , _>()
  fun x -> 
    match cache.TryGetValue x with
    | true, res when isValid x res -> res
    | _ ->
      let res = f x
      cache.[x] <- res
      res

/// Load template & memoize & automatically reload when the file changes
let private fileTemplateMemoized =
  fileTemplate
  |> memoize (fun (_, templatePath) (lastWrite, _) ->
    File.GetLastWriteTime templatePath <= lastWrite)

/// Root direcotry where DotLiquid is looking for templates.
let mutable private templatesDir = None

/// Set the root directory where DotLiquid is looking for templates. 
let setTemplatesDir dir =
  if templatesDir <> Some dir then
    templatesDir <- Some dir
    Template.FileSystem <- localFileSystem dir

/// Render a page using DotLiquid template. 
let render fileName (model:'M) =
  let fullPath =
    match templatesDir with
    | None -> failwith "Templates path not set. Call setTemplatesDir first."
    | Some root -> Path.Combine(root, fileName)

  let writeTime, renderer = fileTemplateMemoized (typeof<'M>, fullPath)
  renderer "model" (box model)

/// Transform file and write results to a file
let transform target source model = 
  File.WriteAllText(target, render source model)

/// Register functions from a module as filters available in DotLiquid templates.
let registerFiltersByName name =
  let asm = Assembly.GetExecutingAssembly()
  let typ = 
    asm.GetTypes()
    |> Array.filter (fun t -> t.FullName.EndsWith(name) && not(t.FullName.Contains("<StartupCode")))
    |> Seq.last
  Template.RegisterFilter typ

/// Filters that can be used in DotLiquid files
module Filters = 
  let private html = Regex("\<[^\>]*\>")

  let tagUrl (s:string) = 
    s.Replace(".", "dot").Replace("#", "sharp").Replace(" ", "-")

  let urlEncode (url:string) =
    System.Web.HttpUtility.UrlEncode(url)

  let mailEncode (url:string) =
    urlEncode(url).Replace("+", "%20")

  let dateAsIso (d:DateTime) = 
    d.ToString("o")

  let dateNice (d:DateTime) = 
    d.ToString("dddd, d MMMM yyyy, h:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-GB"))

  let trimHtml (s:string) = 
    html.Replace(s, "")

  let breakColons (s:string) = 
    match s.Split(':') |> List.ofSeq with
    | [s] -> s
    | s1::ss -> sprintf "<span class='hm'>%s</span><br /><span class='hs'>%s</span>" s1 (String.concat ":" ss)
    | [] -> ""

/// Initialize DotLiquid settings
let initialize (cfg:SiteConfig) = 
  registerFiltersByName "Filters"
  setTemplatesDir cfg.Layouts
