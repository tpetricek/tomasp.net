#r "System.Xml.Linq.dll"
#r "../packages/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll"
#r "../packages/Suave/lib/net40/Suave.dll"
#r "../packages/FAKE/tools/FakeLib.dll"
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
open Fake
open System
open System.IO
open FsBlog

// --------------------------------------------------------------------------------------
// Blog configuration
// --------------------------------------------------------------------------------------

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
  { Posts = posts; Papers = papers; Archives = archives }

let mutable site = loadSite ()

/// Update site - generate all output files if they need to be refreshed
/// When `full = true`, also update archives & calendar pages
let updateSite full changes =
  trace "Updating site"
  traceImportant "Copying static files"
  Blog.copyFiles config changes
  traceImportant "Processing site source"
  if Blog.processFiles config site.Archives changes then 
    site <- loadSite()
    
  traceImportant "Processing special files"
  let specialFiles = 
    [ "404.html", "404.html"
      "index.html", "hp-main.html"
      "academic/index.html", "hp-academic.html"
      "blog/index.html", "listing.html" ]
  for target, layout in specialFiles do
    DotLiquid.transform (config.Output </> target) (config.Layouts </> layout) site

  if full then
    traceImportant "Generating RSS feed"
    Blog.generateRss (config.Output </> "rss.xml") config
      "Tomas Petricek - Languages and tools, open-source, philosophy of science and F# coding"
      "Tomas is a computer scienctist, open-source developer and an occasional philosopher of science. I'm working on tools for data-driven storytelling, contribute to a number of F# projects and I run trainings and offer consulting via fsharpWorks."
      site.Posts 
    traceImportant "Generating archives"
    Blog.generateBlogArchives config site
    Blog.generateTagArchives config site
    Calendar.generateCalendarSite site.Archives config


/// Regenerate site - clean the output folder & regenerate (does not clean cache)
let regenerateSite () = 
  trace "Regenerating site from scratch"
  for dir in Directory.GetDirectories(config.Output) do
    if not (dir.EndsWith(".git")) then 
      CleanDir dir; Directory.Delete dir
  for f in Directory.GetFiles(config.Output) do File.Delete f
  updateSite true None

// --------------------------------------------------------------------------------------
// Local Suave server for debugging
// --------------------------------------------------------------------------------------

open Suave
open Suave.Filters
open Suave.Operators
open Suave.WebSocket
open Suave.Sockets.Control

let port = 11112
let refreshEvent = new Event<unit>()

/// JavaScript to reload the site when page is updated
let wsRefresh = """
  <script language="javascript" type="text/javascript">
    function init() {
      try {
        websocket = new WebSocket("ws://" + window.location.hostname + ":" + window.location.port + "/websocket");
        websocket.onmessage = function(evt) { location.reload(); };
      } catch (e) { /* silently ignore lack of websockets */ }
    }
    window.addEventListener("load", init, false);
  </script>"""

// All generated content is index files in directories. When serving
// them, we replace absolute links & inject websocket code for refresh
let handleDir dir = 
  let html = File.ReadAllText(config.Output </> dir </> "index.html")
  html.Replace(config.Root, sprintf "http://localhost:%d" port)
      .Replace("</body", wsRefresh + "</body")
  |> Successful.OK

let app = 
  choose [
    path "/websocket" >=> handShake (fun ws ctx -> async {
      let msg = System.Text.Encoding.UTF8.GetBytes "refreshed"
      while true do
        do! refreshEvent.Publish |> Control.Async.AwaitEvent
        do! ws.send Text msg true |> Async.Ignore
      return Choice1Of2 () }) 
    path "/" >=> request (fun _ -> handleDir "")
    pathScan "/%s/" handleDir
    Files.browseHome ]

let serverConfig =
  { Web.defaultConfig with
      homeFolder = Some config.Output
      logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Warn
      bindings = [ HttpBinding.mkSimple HTTP "127.0.0.1" port ] }

let startServer () =
  let _, start = Web.startWebServerAsync serverConfig app
  let cts = new System.Threading.CancellationTokenSource()
  Async.Start(start, cts.Token)

// --------------------------------------------------------------------------------------
// FAKE build targets
// --------------------------------------------------------------------------------------

Target "run" (fun () ->
  updateSite false None
  let all = __SOURCE_DIRECTORY__ </> ".."  |> Path.GetFullPath
  use _watcher = 
    !! (all </> "**/*.*") -- (all </> ".ionide.debug") 
    |> WatchChanges (fun e ->
      printfn "Changed files"
      for f in e do printfn " - %s" f.Name
      let changes = 
        if e |> Seq.exists (fun f -> f.FullPath.StartsWith(config.Layouts)) then None
        else Some (set [ for f in e -> f.FullPath ])
      try 
        updateSite false changes 
        refreshEvent.Trigger ()
        trace "Site updated successfully..."
      with e ->
        traceError "Updating site failed!"
        traceException e )
  
  startServer ()
  Diagnostics.Process.Start(sprintf "http://localhost:%d" port) |> ignore
  trace "Waiting for changes, press Enter to stop...."
  Console.ReadLine () |> ignore 
)

Target "publish" (fun () ->
  regenerateSite ()
)

RunTargetOrDefault "run"
