#r "System.Xml.Linq.dll"
#r "../packages/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll"
#r "../packages/DotLiquid/lib/NET45/DotLiquid.dll"
#r "../packages/WindowsAzure.Storage/lib/net40/Microsoft.WindowsAzure.Storage.dll"
#load "../packages/FSharp.Formatting/FSharp.Formatting.fsx"
#load "config.fs"
#load "domain.fs"
#load "helpers.fs"
#load "dotliquid.fs"
#load "document.fs"
#load "calendar.fs"
#load "blog.fs"
open System
open System.IO
open FsBlog

printfn "%A" (Environment.GetCommandLineArgs())
// --------------------------------------------------------------------------------------
// Blog configuration
// --------------------------------------------------------------------------------------

let (</>) a b = Path.Combine(a, b)
let fullPath p = Path.GetFullPath(__SOURCE_DIRECTORY__ </> p)

let config =
  { // Where the site is hosted (without trailing '/')
    Root = "http://tomasp.net"
    // Directory with DotLiquid templates
    Layouts = fullPath "../layouts"

    // Cache and outptu directory (can be outside of the repo)
    Cache = fullPath "../../cache"
    Output = fullPath "../../output"

    // Files from source are transformed/copied to the output
    // Blog & Academic are also parsed and available in DotLiquid templates
    Source = fullPath "../source"
    Blog = fullPath "../source/blog"
    Academic = fullPath "../source/academic"
    // Source with photos for the calendar
    Calendar = fullPath "../../calendar" }

// --------------------------------------------------------------------------------------
// Generating and updating site
// --------------------------------------------------------------------------------------

DotLiquid.initialize config

let loadSite () =
  let posts, papers = Blog.groupArticles config
  let archives = Blog.archives posts
  { Posts = posts; Papers = papers; Archives = archives; PostsTitle = "" }

let mutable site = loadSite ()

/// Update site - generate all output files if they need to be refreshed
/// When `full = true`, also update archives & calendar pages
let updateSite full changes =
  printfn "Updating site"
  printfn "Copying static files"
  Blog.copyFiles config changes
  printfn "Processing site source"
  if Blog.processFiles config site.Archives changes then
    site <- loadSite()

  printfn "Processing special files"
  let specialFiles =
    [ "404.html", "404.html", site
      "index.html", "index.html", site
      "academic/index.html", "papers.html", site
      "blog/index.html", "listing.html",
        { site with Posts = Seq.take 20 site.Posts } ]
  for target, layout, model in specialFiles do
    DotLiquid.transform (config.Output </> target) (config.Layouts </> layout) model

  if full then
    printfn "Generating RSS feed"
    Blog.generateRss (config.Output </> "rss.xml") config
      "Tomas Petricek - Languages and tools, open-source, philosophy of science and F# coding"
      ( "Tomas is a computer scientist, open-source developer and an occasional philosopher of " +
        "science. I'm working on tools for data-driven storytelling, contribute to a number of F# " +
        "projects and I run trainings and offer consulting via fsharpWorks." )
      site.Posts
    printfn "Generating archives"
    Blog.generateBlogArchives config site
    Blog.generateTagArchives config site
    Calendar.generateCalendarSite site.Archives config


/// Regenerate site - clean the output folder & regenerate (does not clean cache)
let regenerateSite () =
  printfn "Regenerating site from scratch"
  for dir in Directory.GetDirectories(config.Output) do
    if not (dir.EndsWith(".git")) then
      Directory.Delete(dir, true)
  for f in Directory.GetFiles(config.Output) do File.Delete f
  updateSite true None

/// Run some operation based on command line argument
let mutable cmd = ""
while cmd <> null do
  cmd <- Console.ReadLine()
  if cmd <> null then
    let args = cmd.Split([|';'|], StringSplitOptions.RemoveEmptyEntries) |> List.ofSeq 
    printfn "Running command: %A" args
    match args with
    | ["calendar"] -> Calendar.uploadCalendarFiles config
    | ["regenerate"] -> regenerateSite ()
    | ["updateall"] -> updateSite false None
    | "update"::changes -> updateSite false (Some(set changes))
    | _ -> printfn "Unrecognized command"
    printfn "DONE"
