open System
open System.Net
open System.IO
open System.Xml

let feed = "http://msmvps.com/blogs/MainFeed.aspx"

let downloadUrl(url:string) = 
  let req = HttpWebRequest.Create(url)
  let rsp = req.GetResponse()
  let rst = rsp.GetResponseStream()
  let reader = new StreamReader(rst)
  let str = reader.ReadToEnd()
  str
  
let xml = downloadUrl(feed)
let doc = new XmlDocument()
doc.LoadXml(xml)

let items = 
  [ for nd in doc.SelectNodes("rss/channel/item") do
      let title = nd.SelectSingleNode("title").InnerText
      let descr = nd.SelectSingleNode("description").InnerText
      yield (title, descr) ]

let keyword = "windows"
let filtered = 
  items
    |> List.filter (fun (title, descr) ->
        title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) > 0
        || descr.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) > 0)
  
  
