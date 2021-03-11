module FsBlog.Calendar

open System
open System.IO
open System.Drawing
open System.Drawing.Imaging
open Microsoft.WindowsAzure.Storage
open FsBlog
open FsBlog.Helpers

let private (</>) a b = Path.Combine(a, b)
let private ensureDirectory d = 
  if not (Directory.Exists(d)) then Directory.CreateDirectory(d) |> ignore

// Get objects needed for JPEG encoding
let private jpegCodec = ImageCodecInfo.GetImageEncoders() |> Seq.find (fun c -> c.FormatID = ImageFormat.Jpeg.Guid)
let private jpegEncoder = Encoder.Quality
let private qualityParam = new EncoderParameters(Param = [| new EncoderParameter(jpegEncoder, 95L) |])

let private enGb = System.Globalization.CultureInfo.GetCultureInfo("en-GB")

// Get the 'calendar' container from Azure storaget
let private container = Lazy.Create (fun () ->
  let account = CloudStorageAccount.Parse(Config.CalendarStorage)
  let container = account.CreateCloudBlobClient().GetContainerReference("calendar")
  if not (container.Exists()) then failwith "container 'calendar' not found" 
  else container)

/// Check if file exists
let private calendarFileExists name = 
  container.Value.GetBlobReference(name).Exists()

/// Write file to Azure container
let private writeCalendarImage name path = 
  let blob = container.Value.GetBlockBlobReference(name)
  blob.UploadFromFile(path)

/// Write file (bytes) to Azure container
let private writeCalendarBytes name bytes = 
  let blob = container.Value.GetBlockBlobReference(name)
  blob.UploadFromByteArray(bytes, 0, bytes.Length)

/// Resize file so that both width & height are smaller than 'maxSize'
let private resizeFile maxSize source (target:string) = 
  use bmp = Bitmap.FromFile(source)
  let scale = max ((float bmp.Width) / (float maxSize)) ((float bmp.Height) / (float maxSize))
  use nbmp = new Bitmap(int (float bmp.Width / scale), int (float bmp.Height / scale))
  ( use gr = Graphics.FromImage(nbmp)
    gr.DrawImage(bmp, 0, 0, nbmp.Width, nbmp.Height) )
  nbmp.Save(target, jpegCodec, qualityParam)


/// Make sure all local files are uploaded to Azure storage
/// (creates files named '2016/august.jpg', '2016/august-orig.jpg', '2016/august-preview.jpg'
/// with various sizes of image and also '2016/august.non-na' if the source was not NA file)
let uploadCalendarFiles (cfg:SiteConfig) = 
  for dir in Directory.GetDirectories(cfg.Calendar) do
    let year = int (Path.GetFileNameWithoutExtension(dir))
    printfn "Checking calendar files for: %d" year
    for month in 1 .. 12 do 
      let monthName = enGb.DateTimeFormat.GetMonthName(month).ToLower()
      let blob suffix = string year + "/" + monthName + suffix
      if not (calendarFileExists (blob ".non-na")) then 
        let source = cfg.Calendar </> (blob ".jpg")
        let source, na = if File.Exists(source) then source, false else cfg.Calendar </> "na.png", true
        let writeFile size suffix = 
          printfn "Uploading calendar: %s" (blob suffix)
          use target = DisposableFile.CreateTemp()
          let file =
            if size = -1 then source 
            else resizeFile size source target.FileName; target.FileName
          writeCalendarImage (blob suffix) file          
        writeFile -1 "-original.jpg"
        writeFile 700 ".jpg"
        writeFile 240 "-preview.jpg"
        if not na then writeCalendarBytes (blob ".non-na") [||]


/// Generate page for a given calendar year
let private generateCalendarIndex archives (cfg:SiteConfig) year file =
  ensureDirectory (Path.GetDirectoryName(file))
  let months = 
    [ for m in 1 .. 12 ->
        let name = enGb.DateTimeFormat.GetMonthName(m)
        { Name = name; Link = name.ToLower() } ]
  let model = { Archives = archives; Year = string year; Months = months }
  File.WriteAllText(file, DotLiquid.render (cfg.Layouts </> "calendar.html") model)


/// Generate all calendar pages
let generateCalendarSite archives (cfg:SiteConfig) =
  // Current year index page
  let calendarFile = cfg.Output </> "calendar" </> "index.html"
  generateCalendarIndex archives cfg DateTime.Now.Year calendarFile

  for dir in Directory.GetDirectories(cfg.Calendar) do
    // Year index page
    let year = int (Path.GetFileNameWithoutExtension(dir))
    printfn "Generating calendar pages for: %d" year
    let yearFile = cfg.Output </> "calendar" </> string year </> "index.html"
    generateCalendarIndex archives cfg year yearFile

(*
// Individual month pages
    for month in 1 .. 12 do 
      let monthName = enGb.DateTimeFormat.GetMonthName(month)
      let monthFile = cfg.Output </> "calendar" </> string year </> monthName.ToLower() </> "index.html"
      ensureDirectory (Path.GetDirectoryName(monthFile))
      let model = 
        { Archives = archives; 
          Title = sprintf "%s %d" monthName year; Link = sprintf "%d/%s" year (monthName.ToLower())
          Days = [ for i in 1 .. enGb.Calendar.GetDaysInMonth(year, month) ->
                    { Day = i; Highlighted = enGb.Calendar.GetDayOfWeek(DateTime(year, month, i)) = DayOfWeek.Sunday } ] }
      File.WriteAllText(monthFile, DotLiquid.render (cfg.Layouts </> "cal-month.html") model)
*)