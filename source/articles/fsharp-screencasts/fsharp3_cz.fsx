#r "FSharp.PowerPack.dll"

open System
open System.Net
open System.IO
open System.Xml
open Microsoft.FSharp.Control

let feeds = 
  [ "http://blogs.msdn.com/MainFeed.aspx?Type=AllBlogs"
    "http://msmvps.com/blogs/MainFeed.aspx"
    "http://weblogs.asp.net/MainFeed.aspx" ]

let downloadUrl(url:string) = async {
  let req = HttpWebRequest.Create(url)
  let! rsp = req.AsyncGetResponse()
  use rst = rsp.GetResponseStream()
  use reader = new StreamReader(rst)
  let str = reader.ReadToEnd()
  return str }

let searchItems(feed:string) = async {
  let! xml = downloadUrl(feed)
  let doc = new XmlDocument()
  doc.LoadXml(xml)

  let items = 
    [ for nd in doc.SelectNodes("rss/channel/item") do
        let title = nd.SelectSingleNode("title").InnerText
        let descr = nd.SelectSingleNode("description").InnerText
        yield (title, descr) ]

  let keywords = "c# azure asp.net"
  let keywordsArray = keywords.Split(' ')
  let result = 
    items
      |> List.filter (fun (title, descr) ->
          keywordsArray |> Array.exists (fun keyword ->
            title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) > 0
            || descr.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) > 0))
  return result }

let res = 
  Async.Run
    (Async.Parallel([ for feed in feeds do
                        yield searchItems(feed) ])) |> List.concat

  
