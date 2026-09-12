module FsBlog.Helpers
open System
open System.IO
open System.Text.RegularExpressions

/// Helper pattern that lets you write things like:
///
///    match stuff with
///    | Let true (isCase1, Case1 str) 
///    | Let false (isCase1, Case2 str) -> (...)
///
let (|Let|) p v = p, v

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

/// GIven `target` generated from `source`, returns
/// `true` if `target` needs to be regenerated
let sourceChanged (source:string) (target:string) = 
  if File.Exists target then 
    let sourceTime = File.GetLastWriteTime source
    let targetTime = File.GetLastWriteTime target
    sourceTime > targetTime 
  else true

/// Given `target` generated from multiple `sources`, 
/// returns `true` if the file needs to be regenarted
let sourceChangedSeq (sources:seq<string>) (target:string) = 
  if File.Exists target then 
    let sourceTime = sources |> Seq.map File.GetLastWriteTime |> Seq.max
    let targetTime = File.GetLastWriteTime target
    sourceTime > targetTime 
  else true

// --------------------------------------------------------------------------------------
// Web analytics
// --------------------------------------------------------------------------------------

/// Medama analytics tag. This is not in any layout - it is injected into the `<head>` of
/// every HTML page the generator writes, so that hand-written pages get it too.
let trackingTag = """<script defer src="https://med.tomasp.net/script.js"></script>"""

/// Matches the opening `<head>` tag, but not `<header>`
let private headRegex = Regex(@"<head(\s[^>]*)?>", RegexOptions.IgnoreCase)

/// Insert the tracking tag right after the opening `<head>` tag. Pages without a head
/// (fragments, redirects) and pages that already have the tag are returned unchanged.
let injectTracking (html:string) =
  let head = headRegex.Match(html)
  if not head.Success || html.Contains(trackingTag) then html
  else
    // Keep whatever line endings the page already uses
    let newline = if html.Contains("\r\n") then "\r\n" else "\n"
    html.Insert(head.Index + head.Length, newline + "  " + trackingTag)

/// Copy a hand-written HTML page, adding the tracking tag. The byte order mark is kept if
/// the source has one; a file that is not valid UTF-8 is copied byte-for-byte instead.
let copyHtmlFile (source:string) (target:string) =
  let bytes = File.ReadAllBytes source
  let hasBom = bytes.Length >= 3 && bytes.[0] = 0xEFuy && bytes.[1] = 0xBBuy && bytes.[2] = 0xBFuy
  let encoding = Text.UTF8Encoding(hasBom, true)
  try
    let text = encoding.GetString(bytes, (if hasBom then 3 else 0), bytes.Length - (if hasBom then 3 else 0))
    File.WriteAllText(target, injectTracking text, encoding)
  with :? Text.DecoderFallbackException ->
    printfn "Not a UTF-8 page, copying as it is: %s" source
    File.WriteAllBytes(target, bytes)
