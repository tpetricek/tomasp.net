module FsBlog.Blog

open System.IO
open System.Xml.Linq
open FsBlog
open FsBlog.Document
open FsBlog.Helpers

// For pretty name formatting
let private enGb = System.Globalization.CultureInfo.GetCultureInfo("en-GB")
let private (</>) a b = Path.Combine(a, b)
let private ensureDirectory d = 
  if not (Directory.Exists(d)) then Directory.CreateDirectory(d) |> ignore

/// Get all files in 'root' that are not in a directory containing
/// either `.ignore` or another explicitly given ignore file
let rec private listFiles blocker root = seq {
  if not (File.Exists(root </> ".ignore") || File.Exists(root </> blocker)) then
    yield! Directory.GetFiles(root)
    for d in Directory.GetDirectories(root) do
      yield! listFiles blocker d }

let private (|Extension|) f = Path.GetExtension(f)
let private (|EndsWith|_|) s (f:string) = if f.EndsWith(s) then Some() else None

/// Read given articles (and cache transformed Article<string> objects)
let private readArticles cfg files = 
  files 
  |> Seq.choose (function
    | Extension ".md" as f -> 
        try Some(transformMarkdown cfg f) 
        with e -> 
          printfn "Error when processing Markdown file: %s" f
          printfn "%A" e
          None
    | Extension ".fsx" as f -> 
        try Some(transformFsScript cfg f) 
        with e -> 
          printfn "Error when processing Markdown file: %s" f
          printfn "%A" e
          None
    | EndsWith ".aspx.html" as f -> 
        try Some(transformLegacyHtml cfg f) 
        with e -> 
          printfn "Error when processing Markdown file: %s" f
          printfn "%A" e
          None
    | _ -> None ) 
  |> Seq.filter (fun p -> not (p.Title.Contains("[DRAFT]")))
  |> Seq.sortByDescending (fun p -> p.Date) 
  |> Seq.toArray  


/// Read articles in the 'blog' folder and in the 'academic' folder
let groupArticles cfg = 
  let posts = 
    readArticles cfg (listFiles ".no-transform" cfg.Blog)
  let papers = 
    readArticles cfg (listFiles ".no-transform" cfg.Academic)
    |> Seq.filter (fun p -> p.HasDate && p.Tags |> Seq.contains "publication")
    |> Seq.sortByDescending (fun d -> d.Date)
  posts, papers 

/// Transform all Markdown, F# Script and legacy HTML files in the source folder
/// Returns 'true' when any file was updated, 'false' if nothing has changed.
let processFiles cfg archives changes = 
  let layoutFiles = Directory.GetFiles(cfg.Layouts)
  let sources = listFiles ".no-transform" cfg.Source
  let mutable anyChange = false
  for f in sources do
    let outf = Path.ChangeExtension(f.Replace(cfg.Source, cfg.Output), "").TrimEnd('.') </> "index.html"
    let forlay = Seq.append [f] layoutFiles
    match f, changes with
    | f, Some changes when not (Set.contains f changes) -> ()
    | EndsWith ".aspx.html", _ ->
        if Helpers.sourceChangedSeq forlay outf then
          printfn "Processing file: %s" (f.Replace(cfg.Source, ""))
          ensureDirectory (Path.GetDirectoryName outf)
          let article = transformLegacyHtml cfg f
          let layout = defaultArg article.Layout "post"
          let model = { Article = article; Archives = archives }
          File.WriteAllText(outf, DotLiquid.render (layout + ".html") model)
          anyChange <- true

    | Let transformFsScript (transform, Extension ".fsx"), _
    | Let transformMarkdown (transform, Extension ".md"), _-> 
        if sourceChangedSeq forlay outf then
          printfn "Processing file: %s" (f.Replace(cfg.Source, ""))
          ensureDirectory (Path.GetDirectoryName outf)
          let article = transform cfg f
          let layout = defaultArg article.Layout "post"
          let model = { Article = article; Archives = archives }
          File.WriteAllText(outf, DotLiquid.render (layout + ".html") model)
          anyChange <- true
    | _ -> () 

  anyChange

/// Copy files from source directory to output. Ignore files that are to be transformed
/// and directories with either '.ignore' or '.no-copy' file.
let copyFiles (cfg:SiteConfig) changes =
  let sources = listFiles ".no-copy" cfg.Source
  for f in sources do
    match f, changes with
    | (Extension ".md" | EndsWith ".aspx.html" | Extension ".fsx"), _ -> ()
    | f, Some changes when not (Set.contains f changes) -> ()
    | _ -> 
        let outf = f.Replace(cfg.Source, cfg.Output)
        if sourceChanged f outf then
          printfn "Copying file: %s" (f.Replace(cfg.Source, ""))
          ensureDirectory (Path.GetDirectoryName(outf))
          File.Copy(f, outf, true)

/// Generate tag and history archives from given blog posts
let archives (posts:seq<Article<_>>) =
  { Tags = 
      posts 
      |> Seq.collect (fun p -> p.Tags) |> Seq.distinct 
      |> Seq.map (fun t ->
        { Name = t; Url = t; Count = posts |> Seq.filter (fun p -> p.Tags |> Seq.contains t) |> Seq.length })
      |> Seq.sortByDescending (fun t -> t.Count)
    History = 
      posts 
      |> Seq.countBy (fun p -> p.Date.Year, p.Date.Month) 
      |> Seq.map (fun ((y, m), c) ->
        { Name = sprintf "%s %d" (enGb.DateTimeFormat.GetMonthName m) y
          Url = sprintf "%s_%d" (enGb.DateTimeFormat.GetMonthName(m).ToLower()) y
          Count = c }) }

/// Generates archives by month. Stored in `blog/archive/august_2016/index.html`
let generateBlogArchives cfg site = 
  let pages = site.Posts |> Seq.groupBy (fun p -> p.Date.Year, p.Date.Month) 
  for (y, m), posts in pages do
    let url = sprintf "%s_%d" (enGb.DateTimeFormat.GetMonthName(m).ToLower()) y
    let outf = cfg.Output </> "blog" </> "archive" </> url </> "index.html"
    ensureDirectory (cfg.Output </> "blog" </> "archive" </> url)
    printfn "Generating history archive: %s" url
    let tag = { site with Posts = posts; PostsTitle = enGb.DateTimeFormat.GetMonthName(m) + " " + string y }
    File.WriteAllText(outf, DotLiquid.render (cfg.Layouts </> "listing.html") tag)

/// Generates archives by tag. Stored in `blog/tag/philosophy/index.html`
let generateTagArchives cfg site = 
  let tags = site.Posts |> Seq.collect (fun p -> p.Tags) |> Seq.distinct 
  let pages = tags |> Seq.map (fun t ->
    t, site.Posts |> Seq.filter (fun p -> p.Tags |> Seq.contains t))
  for t, posts in pages do
    let url = DotLiquid.Filters.tagUrl t
    let outf = cfg.Output </> "blog" </> "tag" </> url </> "index.html"
    printfn "Generating tag archive: %s" url
    ensureDirectory (cfg.Output </> "blog" </> "tag" </> url)
    let tag = { site with Posts = posts; PostsTitle = "Tagged " + t }
    File.WriteAllText(outf, DotLiquid.render (cfg.Layouts </> "listing.html") tag)

/// Generate RSS feed from a collection of articles
let generateRss target (cfg:SiteConfig) title description posts = 
  let (!) name = XName.Get(name)
  let items = 
    [| for item in posts |> Seq.sortByDescending (fun p -> p.Date) |> Seq.take 20 ->
        XElement
          ( !"item", 
            XElement(!"title", item.Title),
            XElement(!"guid", item.Url),
            XElement(!"link", item.Url),
            XElement(!"pubDate", item.Date.ToUniversalTime().ToString("r")),
            XElement(!"description", (item.Abstract:string)) ) |]
  let channel = 
    XElement
      ( !"channel",
        XElement(!"title", (title:string)),
        XElement(!"link", (cfg.Root:string)),
        XElement(!"description", (description:string)),
        items )
  let doc = XDocument(XElement(!"rss", XAttribute(!"version", "2.0"), channel))
  File.WriteAllText(target, doc.ToString())
