module FsBlog.Helpers
open System.IO

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
let sourceChanged source target = 
  if File.Exists target then 
    let sourceTime = File.GetLastWriteTime source
    let targetTime = File.GetLastWriteTime target
    sourceTime > targetTime 
  else true

/// Given `target` generated from multiple `sources`, 
/// returns `true` if the file needs to be regenarted
let sourceChangedSeq sources target = 
  if File.Exists target then 
    let sourceTime = sources |> Seq.map File.GetLastWriteTime |> Seq.max
    let targetTime = File.GetLastWriteTime target
    sourceTime > targetTime 
  else true

/// JSON serialization helpers
module Json = 
  let private serializer = Newtonsoft.Json.JsonSerializer.Create()

  /// Serializes value to JSON using Newtonsoft
  let toJson value =
    let sb = System.Text.StringBuilder()
    use tw = new System.IO.StringWriter(sb)
    serializer.Serialize(tw, value)
    sb.ToString()

  /// Deserializes value from JSON using Newtonsoft
  let fromJson<'R> str : 'R = 
    use tr = new System.IO.StringReader(str)
    serializer.Deserialize(tr, typeof<'R>) :?> 'R
