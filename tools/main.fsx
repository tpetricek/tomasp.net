#r "../packages/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll"
#r "../packages/Suave/lib/net40/Suave.dll"
#r "../packages/FAKE/tools/FakeLib.dll"
#r "../packages/DotLiquid/lib/NET45/DotLiquid.dll"
#load "../packages/FSharp.Formatting/FSharp.Formatting.fsx"
open Fake
open System
open System.IO
open System.Collections.Generic
open FSharp.Literate
open FSharp.Markdown
open FSharp.Markdown.Html
open DotLiquid
open DotLiquid.NamingConventions

let serializer = Newtonsoft.Json.JsonSerializer.Create()

let toJson value =
  let sb = System.Text.StringBuilder()
  use tw = new System.IO.StringWriter(sb)
  serializer.Serialize(tw, value)
  sb.ToString()

let fromJson<'R> str : 'R = 
  use tr = new System.IO.StringReader(str)
  serializer.Deserialize(tr, typeof<'R>) :?> 'R

let (|Let|) p v = p, v

// --------------------------------------------------------------------------------------
// Various file & directory helpers
// --------------------------------------------------------------------------------------
module FileHelpers =

  /// Returns a file name in the TEMP folder and deletes it when disposed
  type DisposableFile(file, deletes) =
    static member Create(file) =
      new DisposableFile(file, [file])
    static member CreateTemp(?extension) = 
      let temp = Path.GetTempFileName()
      let file = match extension with Some ext -> temp + ext | _ -> temp
      new DisposableFile(file, [temp; file])
    member x.FileName = file
    interface System.IDisposable with
      member x.Dispose() = 
        for delete in deletes do
          if File.Exists(delete) then File.Delete(delete)

  let sourceChanged source target = 
    if File.Exists target then 
      let sourceTime = File.GetLastWriteTime source
      let targetTime = File.GetLastWriteTime target
      sourceTime > targetTime 
    else true

  let sourceChangedSeq sources target = 
    if File.Exists target then 
      let sourceTime = sources |> Seq.map File.GetLastWriteTime |> Seq.max
      let targetTime = File.GetLastWriteTime target
      sourceTime > targetTime 
    else true


// --------------------------------------------------------------------------------------
// DotLiquid
// --------------------------------------------------------------------------------------

/// A module for rendering DotLiquid template with Suave
module DotLiquid =

  open System.Reflection
  open System.Collections.Generic
  open System.Collections.Concurrent
  open System.IO
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

  /// Register functions from a module as filters available in DotLiquid templates.
  let registerFiltersByName name =
    let asm = Assembly.GetExecutingAssembly()
    let typ = 
      asm.GetTypes()
      |> Array.filter (fun t -> t.FullName.EndsWith(name) && not(t.FullName.Contains("<StartupCode")))
      |> Seq.last
    Template.RegisterFilter typ

// --------------------------------------------------------------------------------------
// Document transformations
// --------------------------------------------------------------------------------------

module Transforms = 
  
  let (|ColonSeparatedSpans|_|) spans =
    let rec loop before spans = 
      match spans with
      | Literal(text=s)::rest when s.Contains(":") ->
          let s1, s2 = s.Substring(0, s.IndexOf(':')).Trim(), s.Substring(s.IndexOf(':')+1)
          let before = List.rev before
          let before = if String.IsNullOrWhiteSpace(s1) then before else Literal(s1, None)::before
          let rest = if String.IsNullOrWhiteSpace(s2) then rest else Literal(s2, None)::rest
          Some(before, rest)
      | [] -> None
      | x::xs -> loop (x::before) xs
    loop [] spans

  let createFormattingContext writer = 
    { Writer = writer
      Links = dict []
      Newline = "\n"
      LineBreak = ignore
      WrapCodeSnippets = false
      GenerateHeaderAnchors = true
      UniqueNameGenerator = new UniqueNameGenerator()
      ParagraphIndent = ignore }

  let formatSpans spans = 
    let sb = Text.StringBuilder()
    ( use wr = new StringWriter(sb)
      let fc = createFormattingContext wr
      Html.formatSpans fc spans )
    sb.ToString()

  let formatPlainSpans spans = 
    let sb = Text.StringBuilder()
    let rec loop spans = 
      for span in spans do
        match span with
        | DirectLink(body=body) -> loop body
        | Literal(text=t) -> sb.Append(t) |> ignore
        | _ -> failwithf "Unsupported span: %A" span
    loop spans
    sb.ToString()

  let generateSubheadings = function
    | Heading(size=1; body=ColonSeparatedSpans(before, after)) -> 
          InlineBlock
            (sprintf "<h1><span class=\"hm\">%s</span><span class=\"hs\">%s</span></h1>" 
              (formatSpans before) (formatSpans after), None)
    | p -> p

// --------------------------------------------------------------------------------------
// 
// --------------------------------------------------------------------------------------

open Transforms 
open FileHelpers

let root = "http://tomasp.net"
let all = __SOURCE_DIRECTORY__ </> ".." |> Path.GetFullPath
let source = __SOURCE_DIRECTORY__ </> "../source" |> Path.GetFullPath
let calendar = __SOURCE_DIRECTORY__ </> "../calendar" |> Path.GetFullPath
let cache = __SOURCE_DIRECTORY__ </> "../../cache" |> Path.GetFullPath
let blog = __SOURCE_DIRECTORY__ </> "../source/blog" |> Path.GetFullPath
let academic = __SOURCE_DIRECTORY__ </> "../source/academic" |> Path.GetFullPath
let output = __SOURCE_DIRECTORY__ </> "../../output2" |> Path.GetFullPath
let layouts = __SOURCE_DIRECTORY__ </> "../layouts" |> Path.GetFullPath

DotLiquid.setTemplatesDir layouts

let readProperty = function
  | [Span(body=ColonSeparatedSpans(before, after))] ->
      match formatPlainSpans before with
      | "description" -> "description", formatSpans after
      | s -> s, (formatPlainSpans after).Trim()
  | p -> failwithf "Failed to read property: %A" p

let (|Properties|) = function
  | ListBlock(kind=MarkdownListKind.Unordered; items=props)::rest ->
      props |> List.map readProperty |> dict, rest
  | rest -> dict [], rest

let (|Abstract|) = function
  | HorizontalRule(_)::ListBlock(kind=MarkdownListKind.Unordered; items=props)::rest 
  | HorizontalRule(_)::Let [] (props, rest) ->
      let rec split acc = function
        | HorizontalRule _ :: rest -> List.rev acc, rest
        | p :: rest -> split (p::acc) rest
        | _ -> failwith "Parsing abstract failed"
      let standalone = props |> Seq.exists(function [Span(body=[Literal(text="standalone")])] -> true | _ -> false)
      let abs, rest = split [] rest
      Some(standalone, abs), rest
  | rest -> None, rest

let readMetadata (pars:MarkdownParagraphs) = 
  match pars with
  | Heading(size=1; body=title)::Properties(props, Abstract(abs, rest)) -> title, props, abs, rest
  | f -> failwithf "No metadata: %A" f

type Article<'T> =
  { Title : string
    Subtitle : string
    Description : string
    Image : string
    LargeImage : bool
    Tags : seq<string>
    Date : DateTime
    HasDate : bool
    Layout : string option
    Abstract : 'T
    Body : 'T 
    Url : string }
  member x.With(abs, body) = 
    { Subtitle = x.Subtitle; Title = x.Title; Description = x.Description; Image = x.Image
      LargeImage = x.LargeImage; Tags = x.Tags; Date = x.Date; Url = x.Url
      HasDate = x.HasDate; Layout = x.Layout; Abstract = abs; Body = body }

let tryFind k (props:IDictionary<string, string>) = 
  if props.ContainsKey k then Some(props.[k]) else None

let doc = Markdown.Parse(File.ReadAllText @"C:\Tomas\Web\TomaspNet.New\website\source\blog\2016\philopl-questions.md")
let pars = doc.Paragraphs

let parseMetadata (file:string) (title, props, abstractOpt, body) =
  let abs, body =   
    match abstractOpt with
    | Some(true, abs) -> abs, Heading(1, title, None)::body
    | Some(false, abs) -> abs, Heading(1, title, None)::(abs @ body)
    | None -> [], Heading(1, title, None)::body
  let date = tryFind "date" props |> Option.map DateTime.Parse

  { Title = formatSpans title
    Subtitle = defaultArg (tryFind "subtitle" props) ""
    Description = defaultArg (tryFind "description" props) ""
    Image = match tryFind "image" props, tryFind "image-large" props with Some i, _ | _, Some i -> i | _ -> ""
    LargeImage = (tryFind "image-large" props).IsSome
    Tags = 
      (defaultArg (tryFind "tags" props) "").Split([| ',' |], StringSplitOptions.RemoveEmptyEntries) 
      |> Seq.map (fun s -> s.Trim()) |> List.ofSeq
    Date = defaultArg date DateTime.MinValue 
    HasDate = date.IsSome
    Url = root + (Path.ChangeExtension(file.Substring(source.Length), "").TrimEnd('.')).Replace('\\', '/')  + "/"
    Layout = tryFind "layout" props 
    Abstract = abs; Body = body }
    

let transformMarkdown plain (inf:string) =
  let cached = Path.ChangeExtension(cache </> inf.Substring(source.Length+1), ".json")
  if not (sourceChanged inf cached) then 
    fromJson (File.ReadAllText cached)
  else
    printfn "Transforming: %s" inf
    let document =
      if plain then Literate.ParseMarkdownFile(inf)
      else Literate.ParseScriptFile(inf)

    let article = parseMetadata inf (readMetadata document.Paragraphs)
    let body = document.With(List.map Transforms.generateSubheadings article.Body)
    let abs = document.With(article.Abstract)
  
    use tmpAbs = DisposableFile.CreateTemp(".html")
    use tmpDoc = DisposableFile.CreateTemp(".html")
    Literate.ProcessDocument(body, tmpDoc.FileName)
    Literate.ProcessDocument(abs, tmpAbs.FileName)

    let res = article.With(File.ReadAllText tmpAbs.FileName, File.ReadAllText tmpDoc.FileName)
    ensureDirectory (Path.GetDirectoryName cached)
    File.WriteAllText(cached, toJson res)
    res

open System.Text.RegularExpressions

let legacyRegex = 
  Regex
    ("\<!-- \[info\](.*)\[/info\] --\>.*" +
     "\<!-- \[abstract\](.*)\[/abstract\] --\>.*" +
     "\<h1\>(.*)\</h1\>(.*)", RegexOptions.Singleline)

let transformLegacyHtml (inf:string) = 
  let cached = Path.ChangeExtension(cache </> inf.Substring(source.Length+1), ".json")
  if not (sourceChanged inf cached) then 
    fromJson (File.ReadAllText cached)
  else
    printfn "Transforming: %s" inf
    let html = File.ReadAllText(inf)
    let rmatch = legacyRegex.Match(html)
    if not rmatch.Success then failwithf "Failed to parse legacy html: %s" inf

    let props = rmatch.Groups.[1].Value
    let abstr = rmatch.Groups.[2].Value
    let title = rmatch.Groups.[3].Value
    let body = rmatch.Groups.[4].Value
    
    let article = parseMetadata inf (readMetadata (Heading(1, [Literal(title, None)], None)::Markdown.Parse(props).Paragraphs))

    let res = article.With(abstr, "<h1>" + title + "</h1>" + body)
    ensureDirectory (Path.GetDirectoryName cached)
    File.WriteAllText(cached, toJson res)
    res

module Filters = 
  let html = Regex("\<[^\>]*\>")
  let tagUrl (s:string) = 
    s.Replace(".", "dot").Replace("#", "sharp").Replace(" ", "-")

  let urlEncode (url:string) =
    System.Web.HttpUtility.UrlEncode(url)
  let mailEncode (url:string) =
    urlEncode(url).Replace("+", "%20")
  let dateAsIso (d:DateTime) = d.ToString("o")
  let dateNice (d:DateTime) = d.ToString("dddd, d MMMM yyyy, h:mm tt", System.Globalization.CultureInfo.GetCultureInfo("en-GB"))
  let trimHtml (s:string) = 
    html.Replace(s, "")

  let breakColons (s:string) = 
    match s.Split(':') |> List.ofSeq with
    | [s] -> s
    | s1::ss -> sprintf "<span class='hm'>%s</span><br /><span class='hs'>%s</span>" s1 (String.concat ":" ss)
    | [] -> ""

DotLiquid.registerFiltersByName "Filters"
(*

  let processed = File.ReadAllText(html.FileName)
  File.WriteAllText(html.FileName, header + processed)
          
  
  *)
let (|Extension|) f = Path.GetExtension(f)
let (|EndsWith|_|) s (f:string) = if f.EndsWith(s) then Some() else None


let rec listFiles blocker root = seq {
  if not (File.Exists(root </> ".ignore") || File.Exists(root </> blocker)) then
    yield! Directory.GetFiles(root)
    for d in Directory.GetDirectories(root) do
      yield! listFiles blocker d }

let copyFiles () =
  let sources = listFiles ".no-copy" source
  for f in sources do
    match f with
    | Extension ".md" | EndsWith ".aspx.html" | EndsWith ".dot.html" | Extension ".fsx" -> ()
    | _ -> 
      let outf = f.Replace(source, output)
      if sourceChanged f outf then
        printfn "%s" f
        ensureDirectory (Path.GetDirectoryName(outf))
        File.Copy(f, outf, true)

type Category = 
  { Name : string
    Url : string
    Count : int }

type Archives = 
  { Tags : seq<Category>
    History : seq<Category> }

type Site = 
  { Posts : seq<Article<string>> 
    Archives : Archives
    Papers : seq<Article<string>> } 

type ArticleModel = 
  { Article : Article<string>
    Archives : Archives }

let readArticles files = 
  seq {
    for f in files do
      match f with
      | Extension ".md" -> yield try transformMarkdown true f with e -> failwithf "Error when processing: %s\n%A" f e 
      | Extension ".fsx" -> yield try transformMarkdown false f with e -> failwithf "Error when processing: %s\n%A" f e 
      | EndsWith ".aspx.html" -> yield try transformLegacyHtml f with e -> failwithf "Error when processing: %s\n%A" f e 
      | _ -> () } |> Seq.sortByDescending (fun p -> p.Date) |> Seq.toArray  

let processFiles archives = 
  let layoutFiles = !! (layouts </> "*.*")  |> List.ofSeq
  let sources = listFiles ".no-transform" source
  seq { for f in sources do
          let outf = Path.ChangeExtension(f.Replace(source, output), "").TrimEnd('.') </> "index.html"
          let forlay = Seq.append [f] layoutFiles
          match f with
          | EndsWith ".aspx.html" ->
              if sourceChangedSeq forlay outf then
                printfn "%s -> \n  %s" f outf
                ensureDirectory (Path.GetDirectoryName outf)
                let article = transformLegacyHtml f
                let layout = defaultArg article.Layout "post"
                File.WriteAllText(outf, DotLiquid.render (layout + ".html") { Article = article; Archives = archives })
                yield f
          | Let false (plain, Extension ".fsx")
          | Let true (plain, Extension ".md") -> 
              if sourceChangedSeq forlay outf then
                printfn "%s -> \n  %s" f outf
                ensureDirectory (Path.GetDirectoryName outf)
                let article = transformMarkdown plain f
                let layout = defaultArg article.Layout "post"
                File.WriteAllText(outf, DotLiquid.render (layout + ".html") { Article = article; Archives = archives })
                yield f
          | _ -> () } |> Seq.toArray |> Seq.isEmpty |> not // printfn "Skipping %s" f

let enGb = System.Globalization.CultureInfo.GetCultureInfo("en-GB")
let posts = readArticles (listFiles ".no-transform" blog)

let archives =
  { Tags = 
      posts |> Seq.collect (fun p -> p.Tags) |> Seq.distinct |> Seq.map (fun t ->
        { Name = t; Url = t; Count = posts |> Seq.filter (fun p -> p.Tags |> Seq.contains t) |> Seq.length })
    History = 
      posts |> Seq.countBy (fun p -> p.Date.Year, p.Date.Month) |> Seq.map (fun ((y, m), c) ->
        { Name = sprintf "%s %d" (enGb.DateTimeFormat.GetMonthName m) y
          Url = sprintf "%s_%d" (enGb.DateTimeFormat.GetMonthName(m).ToLower()) y
          Count = c }) }

let filtered = set ["f#"; "c#"; "functional"]
let fposts = posts |> Seq.filter (fun p -> 0 = Seq.length (Set.intersect filtered (set p.Tags)))
let byTag t = fposts |> Seq.filter (fun p -> p.Tags |> Seq.contains t)
let tags = fposts |> Seq.collect (fun p -> p.Tags) |> Seq.distinct |> List.ofSeq

tags |> List.map (fun t -> 
  t, 
  byTag t |> Seq.length,
  () //byTag t |> Seq.map (fun a -> a.Title) |> String.concat "\n"
  ) |> List.sortByDescending (fun (_, n, _) -> n)

// data-journalism
// philosophy
// fsharp, csharp, functional

posts |> Seq.length

#r "System.Xml.Linq.dll"
open System.Xml.Linq

let generateRss root title description posts target = 
  let (!) name = XName.Get(name)
  let items = 
    [| for item in posts |> Seq.sortByDescending (fun p -> p.Date) |> Seq.take 20 ->
        XElement
          ( !"item", 
            XElement(!"title", item.Title),
            XElement(!"guid", root + "/blog/" + item.Url),
            XElement(!"link", root + "/blog/" + item.Url + "/index.html"),
            XElement(!"pubDate", item.Date.ToUniversalTime().ToString("r")),
            XElement(!"description", (item.Abstract:string)) ) |]
  let channel = 
    XElement
      ( !"channel",
        XElement(!"title", (title:string)),
        XElement(!"link", (root:string)),
        XElement(!"description", (description:string)),
        items )
  let doc = XDocument(XElement(!"rss", XAttribute(!"version", "2.0"), channel))
  File.WriteAllText(target, doc.ToString())

let reloadSite () = 
  let posts = readArticles (listFiles ".no-transform" blog)
  let papers = 
    readArticles (listFiles ".no-transform" academic)
    |> Seq.filter (fun p -> p.HasDate && p.Tags |> Seq.contains "publication")
    |> Seq.sortByDescending (fun d -> d.Date)
  { Archives = archives; Posts = posts; Papers = papers }

let mutable site = reloadSite()

let generateBlogArchives () = 
  let pages = site.Posts |> Seq.groupBy (fun p -> p.Date.Year, p.Date.Month) 
  for (y, m), posts in pages do
    let url = sprintf "%s_%d" (enGb.DateTimeFormat.GetMonthName(m).ToLower()) y
    let outf = output </> "blog" </> "archive" </> url </> "index.html"
    ensureDirectory (output </> "blog" </> "archive" </> url)
    printfn "Updating archive: %s" outf
    let tag = { site with Posts = posts }
    File.WriteAllText(outf, DotLiquid.render (blog </> "index.dot.html") tag)

let generateTagArchives () = 
  let tags = site.Posts |> Seq.collect (fun p -> p.Tags) |> Seq.distinct 
  let pages = tags |> Seq.map (fun t ->
    t, site.Posts |> Seq.filter (fun p -> p.Tags |> Seq.contains t))
  for t, posts in pages do
    let url = Filters.tagUrl t
    let outf = output </> "blog" </> "tag" </> url </> "index.html"
    printfn "Updating archive: %s" outf
    ensureDirectory (output </> "blog" </> "tag" </> url)
    let tag = { site with Posts = posts }
    File.WriteAllText(outf, DotLiquid.render (layouts </> "listing.html") tag)

!! (all  </> "**/*.*") |> WatchChanges (fun e ->
  try
    printfn "Changes..."
    copyFiles ()
    if processFiles archives then 
      site <- reloadSite()
    
    File.WriteAllText(output </> "index.html", DotLiquid.render (source </> "index.dot.html") site)
    File.WriteAllText(output </> "blog/index.html", DotLiquid.render (layouts </> "listing.html") site)
    File.WriteAllText(output </> "academic/index.html", DotLiquid.render (academic </> "index.dot.html") site)
    generateRss "http://tomasp.net" "Tomas Petricek - Languages and tools, open-source, philosophy of science and F# coding"
      "Tomas is a computer scienctist, open-source developer and an occasional philosopher of science. I'm working on tools for data-driven storytelling, contribute to a number of F# projects and I run trainings and offer consulting via fsharpWorks."
      posts (output </> "rss.xml")
    printfn "All updated...."
  with e ->
    printfn "Update failed: %A" e
)


open Suave
open Suave.Filters
open Suave.Operators

let port = 11112

let app = 
  choose [
    path "/" >=> request (fun _ ->
        let html = File.ReadAllText(output </> "index.html")
        html.Replace("http://tomasp.net", sprintf "http://localhost:%d" port)
        |> Successful.OK)
    pathScan "/%s/"  (fun dir -> 
        let html = File.ReadAllText(output </> dir </> "index.html")
        html.Replace("http://tomasp.net", sprintf "http://localhost:%d" port)
        |> Successful.OK)
    Files.browseHome ]

let serverConfig =
  { Web.defaultConfig with
      homeFolder = Some output
      logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Warn
      bindings = [ HttpBinding.mkSimple HTTP "127.0.0.1" port ] }
let a, b = Web.startWebServerAsync serverConfig app
let cts = new System.Threading.CancellationTokenSource()
Async.Start(b, cts.Token)
Diagnostics.Process.Start(sprintf "http://localhost:%d" port)

// cts.Cancel()

// --------------------------------------------------------------------------------------
// Calendar
// --------------------------------------------------------------------------------------

#r @"..\packages\WindowsAzure.Storage\lib\net40\Microsoft.WindowsAzure.Storage.dll"
#load "config.fs"
open Microsoft.WindowsAzure.Storage

module Calendar = 
  let createCloudBlobClient() = 
    let account = CloudStorageAccount.Parse(Config.CalendarStorage)
    account.CreateCloudBlobClient()

  let container = createCloudBlobClient().GetContainerReference("calendar")
  if not (container.Exists()) then failwith "container 'calendar' not found" 

  let calendarFileExists name = 
    container.GetBlobReference(name).Exists()

  let writeCalendarImage name path = 
    let blob = container.GetBlockBlobReference(name)
    blob.UploadFromFile(path)

  let writeCalendarBytes name bytes = 
    let blob = container.GetBlockBlobReference(name)
    blob.UploadFromByteArray(bytes, 0, bytes.Length)

  open System.Drawing
  open System.Drawing.Imaging

  // Get objects needed for JPEG encoding
  let jpegCodec = 
    ImageCodecInfo.GetImageEncoders() 
    |> Seq.find (fun c -> c.FormatID = ImageFormat.Jpeg.Guid)
  let jpegEncoder = Encoder.Quality
  let qualityParam = new EncoderParameters(Param = [| new EncoderParameter(jpegEncoder, 95L) |])

  /// Resize file so that both width & height are smaller than 'maxSize'
  let resizeFile maxSize source (target:string) = 
    use bmp = Bitmap.FromFile(source)
    let scale = max ((float bmp.Width) / (float maxSize)) ((float bmp.Height) / (float maxSize))
    use nbmp = new Bitmap(int (float bmp.Width / scale), int (float bmp.Height / scale))
    ( use gr = Graphics.FromImage(nbmp)
      gr.DrawImage(bmp, 0, 0, nbmp.Width, nbmp.Height) )
    nbmp.Save(target, jpegCodec, qualityParam)

  let uploadCalendarFiles () = 
    for dir in Directory.GetDirectories(calendar) do
      let year = int (Path.GetFileNameWithoutExtension(dir))
      printfn "Checking calendar files for: %d" year
      for month in 1 .. 12 do 
        let monthName = enGb.DateTimeFormat.GetMonthName(month).ToLower()
        let blob suffix = string year + "/" + monthName + suffix
        if not (calendarFileExists (blob ".non-na")) then 
          let source = calendar </> (blob ".jpg")
          let source, na = if File.Exists(source) then source, false else calendar </> "na.png", true
          let writeFile size suffix = 
            printfn "Uploading calendar: %s" (blob suffix)
            use target = DisposableFile.CreateTemp()
            resizeFile size source target.FileName
            writeCalendarImage (blob suffix) target.FileName          
          writeFile 2400 "-original.jpg"
          writeFile 700 ".jpg"
          writeFile 240 "-preview.jpg"
          if not na then writeCalendarBytes (blob ".non-na") [||]


  type Day = 
    { Day : int
      Highlighted : bool }

  type Month = 
    { Name : string
      Link : string }

  type CalendarYear =
    { Year : string
      Months : seq<Month> 
      Archives : Archives }

  type CalendarMonth =
    { Title : string
      Link : string
      Days : seq<Day>
      Archives : Archives }

  let generateCalendarIndex year file =
    ensureDirectory (Path.GetDirectoryName(file))
    let model = 
      { Archives = archives; Year = string year
        Months = [ for m in 1 .. 12 ->
                    let name = enGb.DateTimeFormat.GetMonthName(m)
                    { Name = name; Link = name.ToLower() } ] }
    File.WriteAllText(file, DotLiquid.render (calendar </> "year.dot.html") model)

  let calendarFile = output </> "calendar" </> "index.html"
  generateCalendarIndex 2016 calendarFile

  for dir in Directory.GetDirectories(calendar) do
    let year = int (Path.GetFileNameWithoutExtension(dir))
    printfn "Generating calendar pages for: %d" year
    let yearFile = output </> "calendar" </> string year </> "index.html"
    generateCalendarIndex year yearFile

    for month in 1 .. 12 do 
      let monthName = enGb.DateTimeFormat.GetMonthName(month)
      let monthFile = output </> "calendar" </> string year </> monthName.ToLower() </> "index.html"
      ensureDirectory (Path.GetDirectoryName(monthFile))
      let model = 
        { Archives = archives; 
          Title = sprintf "%s %d" monthName year; Link = sprintf "%d/%s" year (monthName.ToLower())
          Days = [ for i in 1 .. enGb.Calendar.GetDaysInMonth(year, month) ->
                    { Day = i; Highlighted = enGb.Calendar.GetDayOfWeek(DateTime(year, month, i)) = DayOfWeek.Sunday } ] }
      File.WriteAllText(monthFile, DotLiquid.render (calendar </> "month.dot.html") model)




for d in Directory.GetDirectories(output) do
  printfn "%s" d








(*
#load "lib/posts.fs"
#load "lib/blog.fs"
#load "lib/calendar.fs"
open System
open System.IO
open FsBlogLib.FileHelpers
open FsBlogLib.BlogPosts
open FsBlogLib.Blog
open FsBlogLib.Calendar
open FSharp.Http

// --------------------------------------------------------------------------------------
// Configuration
// --------------------------------------------------------------------------------------

// Root URL for the generated HTML & other basic information
let root = "http://tomasp.net" 
let title = "Tomas Petricek's blog"
let description = 
   "Writing about software development in F# and .NET, sharing materials from " +
   "my F# trainings and talks, writing about programming language research and theory."

// Information about source directory, blog subdirectory, layouts & content
let source = __SOURCE_DIRECTORY__ ++ "../source"
let blog = __SOURCE_DIRECTORY__ ++ "../source/blog"
let blogIndex = __SOURCE_DIRECTORY__ ++ "../source/blog/index.cshtml"
let layouts = __SOURCE_DIRECTORY__ ++ "../layouts"
let parts = __SOURCE_DIRECTORY__ ++ "../layouts/parts"
let content = __SOURCE_DIRECTORY__ ++ "../content"
let template = __SOURCE_DIRECTORY__ ++ "empty-template.html"
let calendar = __SOURCE_DIRECTORY__ ++ "../calendar"
let calendarMonth = __SOURCE_DIRECTORY__ ++ "../source/calendar/month.cshtml"
let calendarIndex = __SOURCE_DIRECTORY__ ++ "../source/calendar/index.cshtml"

// F# code generation - skip 'exclude' directory & add 'references'
let exclude = 
  [ yield __SOURCE_DIRECTORY__ ++ "../source/blog/packages"
    yield __SOURCE_DIRECTORY__ ++ "../source/blog/abstracts"
    yield __SOURCE_DIRECTORY__ ++ "../source/calendar"
    for y in 2013 .. 2025 do
      yield __SOURCE_DIRECTORY__ ++ "../source/blog" ++ (string y) ++ "abstracts"  ]

let references = []

let special =
  [ source ++ "index.cshtml"
    source ++ "blog" ++ "index.cshtml" ]

// Output directory (gh-pages branch)
let output = __SOURCE_DIRECTORY__ ++ "../../output"

// Dependencies - if any of these files change, then we must regenerate all
let dependencies = 
  [ yield! Directory.GetFiles(layouts) 
    yield! Directory.GetFiles(parts) 
    yield calendarMonth 
    yield calendarIndex ]

// --------------------------------------------------------------------------------------
// Clean, build 
// --------------------------------------------------------------------------------------

let tagRenames = 
  [ ("F# language", "f#"); ("Functional Programming in .NET", "functional");
    ("Materials & Links", "links"); ("C# language", "c#"); (".NET General", ".net") ] |> dict

let clean() = 
  for dir in Directory.GetDirectories(output) do
    if not (dir.EndsWith(".git")) then SafeDeleteDir dir true
  for file in Directory.GetFiles(output) do
    File.Delete(file)

let buildSite (updateTagArchive) =
  let noModel = { Model.Root = root; MonthlyPosts = [||]; Posts = [||]; TaglyPosts = [||]; GenerateAll = true }
  let razor = FsBlogLib.Razor(layouts, Model = noModel)
  let model = LoadModel(tagRenames, TransformAsTemp (template, source) razor, root, blog)

  // Generate RSS feed
  GenerateRss root title description model (output ++ "rss.xml")
  GenerateCalendar root layouts output dependencies calendar calendarMonth calendarIndex model

  let uk = System.Globalization.CultureInfo.GetCultureInfo("en-GB")
  GeneratePostListing 
    layouts template blogIndex model model.MonthlyPosts 
    (fun (y, m, _) -> output ++ "blog" ++ "archive" ++ (m.ToLower() + "-" + (string y)) ++ "index.html")
    (fun (y, m, _) -> y = DateTime.Now.Year && m = uk.DateTimeFormat.GetMonthName(DateTime.Now.Month))
    (fun (y, m, _) -> sprintf "%d %s" y m)
    (fun (_, _, p) -> p)

  if updateTagArchive then
    GeneratePostListing 
      layouts template blogIndex model model.TaglyPosts
      (fun (_, u, _) -> output ++ "blog" ++ "tag" ++ u ++ "index.html")
      (fun (_, _, _) -> true)
      (fun (t, _, _) -> t)
      (fun (_, _, p) -> p)

  let filesToProcess = 
    GetSourceFiles source output
    |> SkipExcludedFiles exclude
    |> TransformOutputFiles output
    |> FilterChangedFiles dependencies special
    
  let razor = FsBlogLib.Razor(layouts, Model = model)
  for current, target in filesToProcess do
    EnsureDirectory(Path.GetDirectoryName(target))
    printfn "Processing file: %s" (current.Substring(source.Length + 1))
    TransformFile template true razor None current target

  CopyFiles content output 
  CopyFiles calendar (output ++ "calendar")

// --------------------------------------------------------------------------------------
// Test using local HTTP server
// --------------------------------------------------------------------------------------

let server : ref<option<HttpServer>> = ref None
let stop () =
  server.Value |> Option.iter (fun v -> v.Stop())
let run() =
  let url = "http://localhost:8080/" 
  stop ()
  server := Some(HttpServer.Start(url, output, Replacements = ["http://tomasp.net/", url]))
  printfn "Starting web server at %s" url
  System.Diagnostics.Process.Start(url) |> ignore

// --------------------------------------------------------------------------------------
// Commands
// --------------------------------------------------------------------------------------

let commands () =
  run ()
  stop ()

  buildSite (true) // true - update tag archives
  buildSite (false)

  clean()
  *)