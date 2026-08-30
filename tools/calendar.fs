module FsBlog.Calendar

open System
open System.IO
open System.Drawing
open System.Drawing.Imaging
open FsBlog
open FsBlog.Helpers

let private (</>) a b = Path.Combine(a, b)
let private ensureDirectory d = 
  if not (Directory.Exists(d)) then Directory.CreateDirectory(d) |> ignore

// Lazy because System.Drawing is Windows-only - generating the calendar *pages* has to
// work anywhere, only resizing needs the imaging stack.
let private jpegCodec =
  lazy (ImageCodecInfo.GetImageEncoders() |> Seq.find (fun c -> c.FormatID = ImageFormat.Jpeg.Guid))
let private qualityParam =
  lazy (new EncoderParameters(Param = [| new EncoderParameter(Encoder.Quality, 95L) |]))

let private enGb = System.Globalization.CultureInfo.GetCultureInfo("en-GB")

/// Which months of which years are uploaded. Committed, so CI can generate the calendar
/// pages without the photo library.
let private manifestFile (cfg:SiteConfig) = cfg.Website </> "calendar.txt"

let readManifest (cfg:SiteConfig) : Map<int, Set<string>> =
  let f = manifestFile cfg
  if not (File.Exists f) then Map.empty
  else
    // One "<year>: <month>,<month>,..." line per year
    File.ReadAllLines f
    |> Seq.choose (fun line ->
        match line.Split(':') with
        | [| y; months |] when not (String.IsNullOrWhiteSpace y) ->
            Some(int (y.Trim()),
                 months.Split(',') |> Seq.map (fun m -> m.Trim())
                                   |> Seq.filter (fun m -> m <> "") |> Set.ofSeq)
        | _ -> None)
    |> Map.ofSeq

let private writeManifest (cfg:SiteConfig) (m:Map<int, Set<string>>) =
  let lines =
    m |> Map.toSeq
      |> Seq.map (fun (y, months) -> sprintf "%d: %s" y (String.concat "," (Set.toSeq months)))
  File.WriteAllLines(manifestFile cfg, lines)

/// Resize file so that both width & height are smaller than 'maxSize'
let private resizeFile maxSize source (target:string) =
  use bmp = Bitmap.FromFile(source)
  let scale = max ((float bmp.Width) / (float maxSize)) ((float bmp.Height) / (float maxSize))
  use nbmp = new Bitmap(int (float bmp.Width / scale), int (float bmp.Height / scale))
  ( use gr = Graphics.FromImage(nbmp)
    gr.DrawImage(bmp, 0, 0, nbmp.Width, nbmp.Height) )
  nbmp.Save(target, jpegCodec.Value, qualityParam.Value)

/// Upload every photo not yet in the manifest, then record it. Months with no photo get
/// the 'na' placeholder and stay out of the manifest, so they are retried later.
let uploadCalendarFiles (cfg:SiteConfig) =
  let cred = R2.credentialsFromEnvironment ()
  let mutable manifest = readManifest cfg
  for dir in Directory.GetDirectories(cfg.Calendar) do
    let year = int (Path.GetFileNameWithoutExtension(dir))
    printfn "Checking calendar files for: %d" year
    for month in 1 .. 12 do
      let monthName = enGb.DateTimeFormat.GetMonthName(month).ToLower()
      // Re-read per month - the manifest grows as we go
      let known = defaultArg (Map.tryFind year manifest) Set.empty
      if not (known.Contains monthName) then
        let source = cfg.Calendar </> string year </> (monthName + ".jpg")
        let source, na = if File.Exists(source) then source, false else cfg.Calendar </> "na.png", true
        // A month's photo never changes once set, but a placeholder is replaced as soon
        // as the real photo arrives - and the URL stays the same, so it must expire quickly.
        let cacheControl = if na then "max-age=300" else "max-age=31536000"
        let uploadFile suffix file =
          let key = sprintf "calendar/%d/%s%s" year monthName suffix
          printfn "Uploading calendar: %s" key
          R2.putFile cred key "image/jpeg" cacheControl file
        let uploadResized size suffix =
          use target = DisposableFile.CreateTemp(".jpg")
          resizeFile size source target.FileName
          uploadFile suffix target.FileName
        // Full-size photo goes up untouched; a placeholder month has none, so the 700px
        // version stands in rather than leaving a dead link.
        if na then uploadResized 700 "-original.jpg"
        else uploadFile "-original.jpg" source
        uploadResized 700 ".jpg"
        uploadResized 240 "-preview.jpg"
        if not na then
          manifest <- Map.add year (known.Add monthName) manifest
          writeManifest cfg manifest

/// Generate page for a given calendar year
let private generateCalendarIndex archives (cfg:SiteConfig) year (file:string) =
  ensureDirectory (Path.GetDirectoryName(file))
  let months = 
    [ for m in 1 .. 12 ->
        let name = enGb.DateTimeFormat.GetMonthName(m)
        { Name = name; Link = name.ToLower() } ]
  let model = { Archives = archives; Year = string year; Months = months; ImageRoot = cfg.CalendarRoot }
  File.WriteAllText(file, DotLiquid.render (cfg.Layouts </> "calendar.html") model)


/// Generate all calendar pages
let generateCalendarSite archives (cfg:SiteConfig) =
  // Current year index page
  let calendarFile = cfg.Output </> "calendar" </> "index.html"
  generateCalendarIndex archives cfg DateTime.Now.Year calendarFile

  // Years come from the manifest, so this works without the photo library
  for year in readManifest cfg |> Map.toSeq |> Seq.map fst do
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