namespace FSharp

module Dynamic = 
  open System.Data 
  open System.Data.SqlClient
  open Microsoft.FSharp.Reflection

  module Internal =
    let createCommand name (args:'T) connection = 
      let cmd = new SqlCommand(name, connection)
      cmd.CommandType <- CommandType.StoredProcedure
      SqlCommandBuilder.DeriveParameters(cmd)
      let parameters = 
        [| for (p:SqlParameter) in cmd.Parameters do
             if p.Direction = ParameterDirection.Input then
               yield p |]
      let arguments = 
        if typeof<'T> = typeof<unit> then [| |]
        elif FSharpType.IsTuple(typeof<'T>) then FSharpValue.GetTupleFields(args)
        else [| args |]
      if arguments.Length <> parameters.Length then 
        failwith "Incorrect number of arguments!"
      for (par, arg) in Seq.zip parameters arguments do 
        par.Value <- arg
      cmd

  type DatabaseNonQuery(connectionString:string) = 
    member private x.ConnectionString = connectionString
    static member (?) (x:DatabaseNonQuery, name) = fun (args:'T) -> 
      use cn = new SqlConnection(x.ConnectionString)
      cn.Open()
      let cmd = Internal.createCommand name args cn
      cmd.ExecuteNonQuery() |> ignore

  type Row(reader:SqlDataReader) = 
    member private x.Reader = reader
    static member (?) (x:Row, name:string) : 'R = 
      x.Reader.[name] :?> 'R

  type DatabaseQuery(connectionString:string) = 
    member private x.ConnectionString = connectionString
    static member (?) (x:DatabaseQuery, name) = fun (args:'T) -> seq {
      use cn = new SqlConnection(x.ConnectionString)
      cn.Open()
      let cmd = Internal.createCommand name args cn
      let reader = cmd.ExecuteReader()
      while reader.Read() do
        yield Row(reader) }

  type DynamicDatabase(connectionString:string) =
    member x.Query = DatabaseQuery(connectionString)
    member x.NonQuery = DatabaseNonQuery(connectionString)

open Dynamic


// [snippet:record]
/// Data returned from the model to a view
type PollOption =
  { ID : int
    Votes : int
    Percentage : float
    Title : string }
// [/snippet]

module DynamicDemo = 
  let connectionString = 
    "Data Source=.\SQLEXPRESS;AttachDbFilename=\"|DataDirectory|" +
    "PollData.mdf\";Integrated Security=True;User Instance=True"
  
  // [snippet:F# dynamic db]
  /// Call 'GetOptions' procedure to load collection of options
  let load() = 
    let db = new DynamicDatabase(connectionString)
    let options = db.Query?GetOptions()
    // Loop over the result set and create 'PollOption' values
    [ for row in options do
        yield { ID = row?ID; Votes = row?Votes;
                Title = row?Title; Percentage = row?Percentage } ]
  // [/snippet]


#load @"C:\Tomas\Materials\Public\Talks 2011\Data Access (GOTO Copenhagen)\GotoDemos (All)\PollWebStructural\FSharpWeb.Core\DynamicDatabase.fs"
open FSharpWeb.Core

module StructuralDemo =
  let connectionString = 
    "Data Source=.\SQLEXPRESS;AttachDbFilename=\"|DataDirectory|" +
    "PollData.mdf\";Integrated Security=True;User Instance=True"

  // [snippet:Structural]
  /// Call 'GetOptions' and automtically convert result to a collection
  let load() : seq<PollOption> = 
    let db = new DynamicDatabase(connectionString)
    db?GetOptions()

  /// Call 'Vote' procedure without returning any value
  let vote(id:int) : unit = 
    let db = new DynamicDatabase(connectionString)
    db?Vote(id)
  // [/snippet]

#r "System.Xml.Linq.dll"
#load @"C:\Tomas\Materials\Public\Talks 2011\Data Access (GOTO Copenhagen)\GotoDemos (All)\XmlStructural\FSharpWeb.Core\StructuralXml.fs"


// [snippet:record]
/// Represents a collection of news (title, 
/// description, url) with a media name and a link
type Listing = 
  { Name : string
    Link : string
    Items : seq<string * string * string> }
// [/snippet]

module Model =
  open System.Text.RegularExpressions

  /// Helper function that strips HTML from a 
  /// string and takes first 200 characters
  let stripHtml (html:string) = 
    let res = Regex.Replace(html, "<.*?>", "")
    if res.Length > 200 then res.Substring(0, 200) + "..." else res

  // --------------------------------------------------------------------------
  // Example #1 - Parsing RSS feeds

  // Specifies the structure of the source XML document
  // A union case name corresponds to elemnets in XML document, so for example
  //   type Title = Title of string
  //
  // will match a XML node like:
  //   <title>hello world</title>
  //

  // [snippet:Xml structure]
  // Specifies the expected structure of XML document
  type Title = Title of string
  type Link = Link of string
  type Description = Description of string

  /// Item contains title, link and description
  type Item = Item of Title * Link * Description
  /// Channel contains information and a list of (nested) items
  type Channel = Channel of Title * Link * Description * list<Item>
  /// Root element is 'rss' containing a channel
  type Rss = Rss of Channel
  // [/snippet]
  
  // [snippet:loading xml]
  /// Loads news from the Guardian using RSS feed
  let loadGuardian() =
    // Download data and convert them to the 'Rss' structure
    let url = "http://feeds.guardian.co.uk/theguardian/world/rss"
    let doc = StructuralXml.Load(url, LowerCase = true)
    let (Rss(Channel(Title title, Link link, _, items))) = doc.Root 

    // Create F# 'Listing' type from the parsed XML
    let items = seq { 
      for (Item(Title title, Link link, Description descr)) in items do
        yield title, link, stripHtml descr }
    { Name = title; Link = link; Items = items }
  // [/snippet]


#r @"C:\Tomas\Materials\Public\Talks 2011\Data Access (GOTO Copenhagen)\GotoDemos (All)\Freebase\Samples.DataStore.Freebase.dll"
