#r "../packages/Suave/lib/net40/Suave.dll"
#r "../packages/FAKE/tools/FakeLib.dll"
open Fake
open System
open System.IO
open Fake.Git.CommandHelper
open Suave
open Suave.Filters
open Suave.Operators
open Suave.WebSocket
open System.Diagnostics

// --------------------------------------------------------------------------------------
// Local Suave server for debugging 
// --------------------------------------------------------------------------------------

let fullPath p = Path.GetFullPath(__SOURCE_DIRECTORY__ </> p)
let root = "http://tomasp.net"
let output = fullPath "../../output"
let layouts = fullPath "../layouts"

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
  let html = File.ReadAllText(output </> dir </> "index.html")
  html.Replace(root, sprintf "http://localhost:%d" port)
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
      homeFolder = Some output
      logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Warn
      bindings = [ HttpBinding.mkSimple HTTP "127.0.0.1" port ] }

let startServer () =
  let _, start = Web.startWebServerAsync serverConfig app
  let cts = new System.Threading.CancellationTokenSource()
  Async.Start(start, cts.Token)

let worker = 
  let psi = 
    ProcessStartInfo
      ( FileName = (__SOURCE_DIRECTORY__  </> "../packages/FSharp.Compiler.Tools/tools/fsi.exe"), 
        Arguments = "tools/update.fsx", UseShellExecute = false, 
        RedirectStandardOutput = true, RedirectStandardInput = true)  
  Process.Start(psi)

let invokeUpdate cmd changes = 
  worker.StandardInput.WriteLine(cmd + " " + String.concat " " changes)
  let mutable s = ""
  while (s <- worker.StandardOutput.ReadLine(); s <> "DONE") do printfn "%s" s

// --------------------------------------------------------------------------------------
// FAKE build targets
// --------------------------------------------------------------------------------------

Target "run" (fun () ->
  invokeUpdate "updateall" []
  let all = __SOURCE_DIRECTORY__ </> ".."  |> Path.GetFullPath
  use _watcher = 
    !! (all </> "**/*.*") -- (all </> ".ionide.debug") -- (all </> "packages" </> "**/*.*") 
    |> WatchChanges (fun e ->
      printfn "Changed files"
      for f in e do printfn " - %s" f.Name
      let cmd, changes = 
        if e |> Seq.exists (fun f -> f.FullPath.StartsWith(layouts)) then "updateall", set []
        else "update", (set [ for f in e -> f.FullPath ])
      try 
        invokeUpdate cmd changes 
        refreshEvent.Trigger ()
        trace "Site updated successfully..."
      with e ->
        traceException e 
        traceError "Updating site failed!" )
  
  startServer ()
  Diagnostics.Process.Start(sprintf "http://localhost:%d" port) |> ignore
  trace "Waiting for changes, press Enter to stop...."
  Console.ReadLine () |> ignore 
)

Target "update" (fun () ->
  invokeUpdate "regenerate" []
  invokeUpdate "calendar" []
)

Target "publish" (fun () ->
  runGitCommand output "add ." |> ignore
  runGitCommand output (sprintf "commit -a -m \"Updating site (%s)\"" (DateTime.Now.ToString("f"))) |> ignore
  Git.Branches.push output
)

"update" ==> "publish"
RunTargetOrDefault "run"
