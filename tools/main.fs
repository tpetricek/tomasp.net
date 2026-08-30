module FsBlog.Main

open System
open System.IO
open System.Net
open System.Net.WebSockets
open System.Text
open System.Threading
open FsBlog

let private (</>) a b = Path.Combine(a, b)

// --------------------------------------------------------------------------------------
// Blog configuration
// --------------------------------------------------------------------------------------

/// The website folder holds both 'layouts' and 'source'. Finding it by walking out of the
/// binary's folder keeps the tool runnable from any directory.
let rec private findWebsiteRoot (dir:DirectoryInfo) =
  if isNull (box dir) then
    failwith "Could not locate the website folder (expected 'layouts' and 'source' inside it)"
  elif Directory.Exists(dir.FullName </> "layouts") && Directory.Exists(dir.FullName </> "source") then
    dir.FullName
  else findWebsiteRoot dir.Parent

let private website = findWebsiteRoot (DirectoryInfo(AppContext.BaseDirectory))
let private outside p = Path.GetFullPath(website </> ".." </> p)

let config =
  { // Where the site is hosted (without trailing '/')
    Root = "http://tomasp.net"
    // Directory with DotLiquid templates
    Layouts = website </> "layouts"

    // Output directory (outside of the repo)
    Output = outside "output"

    // Files from source are transformed/copied to the output
    // Blog & Academic are also parsed and available in DotLiquid templates
    Source = website </> "source"
    Blog = website </> "source" </> "blog"
    Academic = website </> "source" </> "academic"
    // Source with photos for the calendar
    Calendar = outside "calendar"
    Website = website
    // Calendar photos are served from Cloudflare R2
    CalendarRoot = "https://img.tomasp.net/calendar" }

// --------------------------------------------------------------------------------------
// Generating and updating site
// --------------------------------------------------------------------------------------

DotLiquid.initialize config

let private loadSite () =
  let posts, papers = Blog.groupArticles config
  let archives = Blog.archives posts
  { Posts = posts; Papers = papers; Archives = archives; PostsTitle = ""
    ImageRoot = config.CalendarRoot }

let mutable private site = loadSite ()

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

/// Regenerate site - clean the output folder & regenerate everything
let regenerateSite () =
  printfn "Regenerating site from scratch"
  for dir in Directory.GetDirectories(config.Output) do
    if not (dir.EndsWith(".git")) then
      Directory.Delete(dir, true)
  for f in Directory.GetFiles(config.Output) do File.Delete f
  updateSite true None

// --------------------------------------------------------------------------------------
// Local web server for debugging
// --------------------------------------------------------------------------------------

module private Server =
  let port = 11112
  let private root1 = "http://tomasp.net"
  let private root2 = "https://tomasp.net"
  let private local = sprintf "http://localhost:%d" port

  /// JavaScript to reload the site when page is updated
  let private wsRefresh = """
  <script language="javascript" type="text/javascript">
    function init() {
      try {
        websocket = new WebSocket("ws://" + window.location.hostname + ":" + window.location.port + "/websocket");
        websocket.onmessage = function(evt) { location.reload(); };
      } catch (e) { /* silently ignore lack of websockets */ }
    }
    window.addEventListener("load", init, false);
  </script>"""

  let private contentTypes =
    dict [ ".html", "text/html; charset=utf-8"; ".css", "text/css"; ".js", "text/javascript"
           ".json", "application/json"; ".xml", "application/xml"; ".svg", "image/svg+xml"
           ".png", "image/png"; ".jpg", "image/jpeg"; ".jpeg", "image/jpeg"; ".gif", "image/gif"
           ".ico", "image/x-icon"; ".woff", "font/woff"; ".woff2", "font/woff2"; ".ttf", "font/ttf"
           ".pdf", "application/pdf"; ".zip", "application/zip"; ".txt", "text/plain" ]

  let private sockets = ResizeArray<WebSocket>()

  /// Tell connected browsers to reload
  let refresh () =
    lock sockets (fun () ->
      let msg = ArraySegment(Encoding.UTF8.GetBytes "refreshed")
      for ws in List.ofSeq sockets do
        try
          if ws.State = WebSocketState.Open then
            ws.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None).Wait()
        with _ -> sockets.Remove(ws) |> ignore)

  // All generated content is index files in directories. When serving
  // them, we replace absolute links & inject websocket code for refresh
  let private serveDirectory (res:HttpListenerResponse) (dir:string) =
    let file = config.Output </> dir </> "index.html"
    if not (File.Exists file) then
      res.StatusCode <- 404
      res.Close()
    else
      let html =
        File.ReadAllText(file).Replace(root1, local).Replace(root2, local)
            .Replace("</body", wsRefresh + "</body")
      let bytes = Encoding.UTF8.GetBytes html
      res.ContentType <- "text/html; charset=utf-8"
      res.ContentLength64 <- int64 bytes.Length
      res.OutputStream.Write(bytes, 0, bytes.Length)
      res.Close()

  let private serveFile (res:HttpListenerResponse) (path:string) =
    let file = config.Output </> path.Replace('/', Path.DirectorySeparatorChar)
    if not (File.Exists file) then
      res.StatusCode <- 404
      res.Close()
    else
      let ext = Path.GetExtension(file).ToLowerInvariant()
      res.ContentType <- if contentTypes.ContainsKey ext then contentTypes.[ext] else "application/octet-stream"
      use fs = File.OpenRead file
      res.ContentLength64 <- fs.Length
      fs.CopyTo(res.OutputStream)
      res.Close()

  let private handle (ctx:HttpListenerContext) = async {
    let path = Uri.UnescapeDataString(ctx.Request.Url.AbsolutePath)
    if path = "/websocket" then
      if ctx.Request.IsWebSocketRequest then
        let! ws = ctx.AcceptWebSocketAsync(null) |> Async.AwaitTask
        lock sockets (fun () -> sockets.Add ws.WebSocket)
      else ctx.Response.StatusCode <- 400; ctx.Response.Close()
    elif path.EndsWith "/" then
      serveDirectory ctx.Response (path.Trim('/'))
    else
      serveFile ctx.Response (path.TrimStart('/')) }

  let start () =
    let listener = new HttpListener()
    listener.Prefixes.Add(sprintf "http://localhost:%d/" port)
    listener.Start()
    let rec loop () = async {
      let! ctx = listener.GetContextAsync() |> Async.AwaitTask
      Async.Start(async { try do! handle ctx with e -> eprintfn "Request failed: %s" e.Message })
      return! loop () }
    Async.Start(loop ())

// --------------------------------------------------------------------------------------
// Watching for changes
// --------------------------------------------------------------------------------------

/// Rebuild on change. A layout affects every page, so that triggers a full pass; anything
/// else refreshes only what changed.
let private watch () =
  let pending = System.Collections.Generic.HashSet<string>()
  let mutable timer : Timer = null

  let rebuild () =
    let changes = lock pending (fun () -> let c = Set.ofSeq pending in pending.Clear(); c)
    if not (Set.isEmpty changes) then
      printfn "Changed files"
      for f in changes do printfn " - %s" (Path.GetFileName f)
      try
        if changes |> Seq.exists (fun f -> f.StartsWith(config.Layouts)) then updateSite false None
        else updateSite false (Some changes)
        Server.refresh ()
        printfn "Site updated successfully..."
      with e ->
        eprintfn "Updating site failed: %s" e.Message

  let onChange (e:FileSystemEventArgs) =
    if not (e.FullPath.Contains(@"\bin\") || e.FullPath.Contains(@"\obj\")) then
      lock pending (fun () -> pending.Add e.FullPath |> ignore)
      // debounce - editors write a file several times in a row
      if not (isNull timer) then timer.Dispose()
      timer <- new Timer((fun _ -> rebuild ()), null, 500, Timeout.Infinite)

  let watchers =
    [ for dir in [ config.Source; config.Layouts ] ->
        let w = new FileSystemWatcher(dir, IncludeSubdirectories = true, EnableRaisingEvents = true)
        w.Changed.Add onChange
        w.Created.Add onChange
        w.Renamed.Add(fun e -> onChange (e :> FileSystemEventArgs))
        w ]
  watchers

// --------------------------------------------------------------------------------------
// Entry point
// --------------------------------------------------------------------------------------

[<EntryPoint>]
let main argv =
  match (if argv.Length = 0 then "run" else argv.[0].ToLowerInvariant()) with
  | "build" ->
      regenerateSite ()
      0
  | "calendar" ->
      Calendar.uploadCalendarFiles config
      0
  | "run" ->
      updateSite false None
      let _watchers = watch ()
      Server.start ()
      let url = sprintf "http://localhost:%d" Server.port
      try Diagnostics.Process.Start(Diagnostics.ProcessStartInfo(url, UseShellExecute = true)) |> ignore
      with _ -> ()
      printfn "Serving %s - press Enter to stop..." url
      Console.ReadLine () |> ignore
      0
  | cmd ->
      eprintfn "Unknown command '%s' (expected 'run', 'build' or 'calendar')" cmd
      1
