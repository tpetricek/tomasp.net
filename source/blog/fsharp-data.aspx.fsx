(**
F# Data: New type provider library
==================================

 - date: 2013-03-28T03:23:41.0000000
 - description: F# Data is a new library that gives you all you need to access data in F# 3.0. It implements type providers for WorldBank, Freebase and structured document formats (CSV, JSON and XML) as well as other helpers. This article introduces the library and gives a quick overview of its features.
 - layout: article
 - tags: open source,f#,f# data,type providers
 - title: F# Data: New type provider library
 - url: fsharp-data.aspx

--------------------------------------------------------------------------------
 - standalone


<img src="https://raw.github.com/fsharp/FSharp.Data/master/misc/logo.png" class="rdecor" style="width:120px;height:120px;" />

When F# 3.0 type providers were still in beta version, I wrote a couple of type 
providers as examples for talks. These included the WorldBank type provider
(now available [on Try F#](http://www.tryfsharp.org)) and also type provider for
XML that infered the structure from sample.   
For some time, these were hosted as part of [FSharpX](https://github.com/fsharp/fsharpx/) 
and the authors of FSharpX also added a number of great features.

When I found some more time earlier this year, I decided to start a new library
that would be fully focused on data access in F# and on type providers and
I started working on **F# Data**. The library has now reached a stable state
and [Steffen also announced](http://www.navision-blog.de/blog/2013/03/27/fsharpx-1-8-removes-support-for-document-type-provider/) 
that the document type providers (JSON, XML and CSV) are not going to be available
in FSharpX since the next version. 

This means that if you're interested in accessing data using F# type providers, 
you should now go to F# Data. Here are the most important links:

 * [F# Data source code on GitHub](https://github.com/fsharp/FSharp.Data)
 * [F# Data documentation & tutorials](http://fsharp.github.com/FSharp.Data/)
 * [F# Data on NuGet](http://nuget.org/packages/FSharp.Data)

Before looking at the details, I would like to thank to [Gustavo Guerra](https://github.com/ovatsus)
who made some amazing contributions to the library! (More contributors are always welcome,
so continue reading if you're interested...)


--------------------------------------------------------------------------------


<img src="https://raw.github.com/fsharp/FSharp.Data/master/misc/logo.png" class="rdecor" style="width:120px;height:120px;" />

When F# 3.0 type providers were still in beta version, I wrote a couple of type 
providers as examples for talks. These included the WorldBank type provider
(now available [on Try F#](http://www.tryfsharp.org)) and also type provider for
XML that infered the structure from sample.   
For some time, these were hosted as part of [FSharpX](https://github.com/fsharp/fsharpx/) 
and the authors of FSharpX also added a number of great features.

When I found some more time earlier this year, I decided to start a new library
that would be fully focused on data access in F# and on type providers and
I started working on **F# Data**. The library has now reached a stable state
and [Steffen also announced](http://www.navision-blog.de/blog/2013/03/27/fsharpx-1-8-removes-support-for-document-type-provider/) 
that the document type providers (JSON, XML and CSV) are not going to be available
in FSharpX since the next version. 

This means that if you're interested in accessing data using F# type providers, 
you should now go to F# Data. Here are the most important links:

 * [F# Data source code on GitHub](https://github.com/fsharp/FSharp.Data)
 * [F# Data documentation & tutorials](http://fsharp.github.com/FSharp.Data/)
 * [F# Data on NuGet](http://nuget.org/packages/FSharp.Data)

Before looking at the details, I would like to thank to [Gustavo Guerra](https://github.com/ovatsus)
who made some amazing contributions to the library! (More contributors are always welcome,
so continue reading if you're interested...)

F# Data Overview
----------------

The library contains several type providers, a couple of helper functions and it also
comes with comprehensive documentation. Here is a quick summary of the key features:

 * **Document type providers** are providers for JSON, XML and CSV that 
   infer the structure of a file from a provided example and give you a typed
   access to other data in the same format.

 * **WorldBank and Freebase** are also hosted as part of the library. They give 
   you access to [WorldBank](http://data.worldbank.org) indicators (information about countries) and to the
   [Freebase graph database](http://freebase.com) (this was originally written as
   a sample by the F# team).

 * **Comprehensive documentation** the library is using my other project,
   [F# Formatting](https://github.com/tpetricek/FSharp.Formatting) to automatically
   generate a [nice documentation](http://fsharp.github.com/FSharp.Data/) from 
   `*.fsx` script files with examples.

 * **HTTP utility** the library also contains a very easy to use type for making
   HTTP requests with just a single line (look for `Http.Request` in the [documentation](http://fsharp.github.com/FSharp.Data/library/Http.html).
   This is something that I've been missing a lot when working with REST APIs from F#.

F# Data Code Samples
--------------------
*)

(*** hide ***)
#r "System.Xml.Linq.dll"
#I "packages\FSharp.Data.1.1.0.0"

(**
I do not want to spend too much time demonstrating all the awesome features of the F# Data 
library, but let me include just a few code snippets to demonstrate some interesting features.
(You can find more in the [documentation](http://fsharp.github.com/FSharp.Data/)).

All the samples assume that we're using an F# Script file, so we start by referencing the 
F# Data library using `#r` (in a project file, you would add reference as usual). I also 
open two namespaces - `FSharp.Data` with the data-related API and `FSharp.Net` with a 
helper type `Http` for making HTTP requests:
*)

#r "FSharp.Data.dll"
open FSharp.Data
open FSharp.Net

(**
Now, let's quickly look at a number of examples that demonstrate the F# Data library.
You cannot quite see that in static code sample on a blog, but note that all data access
is done in a typed way. When you type `.`, you get a completion and if you make a typo,
you'll get an instantaneous feedback about the error.

### Geting government debt from WorldBank

The `WorldBankData` type gives you access to the [World Bank](http://worldbank.org) data
set. For example, we can look at "Czech Republic" and get the government debt for the
most recent year (using `Seq.maxBy` to get value for the most recent year available):

*)
let wb = WorldBankData.GetDataContext()
wb.Countries.``Czech Republic``.Indicators.``Central government debt, total (% of GDP)``
|> Seq.maxBy fst

(**
### Geting religion list from Freebase

The `FreebaseData` type gives you access to [Freebase](http://freebase.com). You can just
type `.` and explore the data sources available - for example, to look at a list of 
religions and print first 10:

*)
let fb = FreebaseData.GetDataContext()
for rel in fb.Society.Religion.Religions |> Seq.take 10 do
  printfn "%s" rel.Name

(**
### Parsing RSS news feed from BBC

If you want to get news using RSS, you can use `XmlProvider`. All you need is a sample
file or a string (marked as `Literal`) with the RSS data. Then you can pass this string
or file to the provider as a static parameter and you'll get nice types for working with 
RSS feeds. Here, we get news from BBC using the `Http.Request` helper:

*)
let [<Literal>] RssSample = (*[omit:(Sample RSS feed omitted)]*)
  """<?xml version="1.0" encoding="UTF-8"?>
  <rss version="2.0">  
    <channel> 
      <title>BBC News - Home</title>  
      <link>http://www.bbc.co.uk/news/#sa-ns_mchannel=rss&amp;ns_source=PublicRSS20-sa</link>  
      <description>The latest stories from the Home section of the BBC News web site.</description>  
      <lastBuildDate>Thu, 28 Mar 2013 01:10:12 GMT</lastBuildDate>  
      <item> 
        <title>Government loses Abu Qatada appeal</title>  
        <description>Home Secretary Theresa May loses her appeal against a ruling preventing the deportation of radical cleric Abu Qatada.</description>  
        <link>http://www.bbc.co.uk/news/uk-21955844#sa-ns_mchannel=rss&amp;ns_source=PublicRSS20-sa</link>  
        <guid isPermaLink="false">http://www.bbc.co.uk/news/uk-21955844</guid>  
        <pubDate>Wed, 27 Mar 2013 17:37:35 GMT</pubDate>  
      </item>  
      <item> 
        <title>Synchrotron yields 'safer' vaccine</title>  
        <description>British scientists develop a new way to create an entirely synthetic vaccine which does not rely on using live infectious virus, meaning it is much safer.</description>  
        <link>http://www.bbc.co.uk/news/health-21958361#sa-ns_mchannel=rss&amp;ns_source=PublicRSS20-sa</link>  
        <guid isPermaLink="false">http://www.bbc.co.uk/news/health-21958361</guid>  
        <pubDate>Wed, 27 Mar 2013 22:00:04 GMT</pubDate>  
      </item>  
      <item> 
        <title>Oil firms invest $500m in huge field</title>  
        <description>Major oil companies announce plans which they hope will boost production from the UK's biggest oilfield.</description>  
        <link>http://www.bbc.co.uk/news/uk-scotland-scotland-business-21955536#sa-ns_mchannel=rss&amp;ns_source=PublicRSS20-sa</link>  
        <guid isPermaLink="false">http://www.bbc.co.uk/news/uk-scotland-scotland-business-21955536</guid>  
        <pubDate>Thu, 28 Mar 2013 00:31:05 GMT</pubDate>  
      </item>  
    </channel> 
  </rss>
  """(*[/omit]*)
type Rss = XmlProvider<RssSample>

let feed = Rss.Parse(Http.Request("http://feeds.bbci.co.uk/news/rss.xml"))
printfn  "%s" feed.Channel.Title
for item in feed.Channel.GetItems() do
  printfn " - %s" item.Title

(**
### Geting stock prices from Yahoo CSV

Working with CSV files is similar. The `CsvProvider` takes static parameter with sample 
data (either as a file name or as actual data). Here, we use a file and we also specify 
that we only want to use first 10 rows for the inference (for performance reasons). The
provider infers column names and types. Here is how you calculate the average MSFT stock 
price over the entire history:

*)
type Stocks = CsvProvider<"data/fsharp-data/MSFT.csv", InferRows=10>
let msft = Stocks.Load("http://ichart.finance.yahoo.com/table.csv?s=MSFT")
msft.Data |> Seq.averageBy (fun row -> row.Open)

(**
### Geting list of F# snippets using REST API

For our last example, we'll use REST API provided by [F# Snippets](http://fssnip.net). The
API returns a JSON data set containing information about snippets. We can easily use it by
defining a string `Literal` with sample JSON and passing it to `JsonProvider`. To get the
data, we use `Http.Request`, but this time we specify `Content-Type` header. Working
with the results is, again, done in a nice typed way: 

*)
let [<Literal>] FsSnipNewsSample = (*[omit:(Sample JSON with snippet data omitted)]*)"""[ { "author": "Kit Eason",
    "title": "Eurovision - Some(points)",
    "description": "The Eurovision final scoring system using records and some higher order functions. (...)",
    "likes": 1,
    "link": "http://fssnip.net/cg",
    "published": "5 months ago"},
  { "author": "Eirik Tsarpalis",
    "title": "Codomains through Reflection",
    "description": "Any type signature has the form of a curried chain T0 -> T1 -> .... -> Tn, where Tn is not a function type. (...)",
    "likes": 2,
    "link": "http://fssnip.net/cf",
    "published": "5 months ago" } ]"""(*[/omit]*)
type FsSnipNews = JsonProvider<FsSnipNewsSample>

let data = 
  Http.Request
    ( "http://api.fssnip.net/1/snippet", 
      headers=["content-type", "application/json"] )

let res = FsSnipNews.Parse(data)
for snippet in res do
  printfn " - %s" snippet.Title

(**

Summary
-------

Although I started working on F# Data around christmas, this is the first blog
post about it. The library had some time to develop and we fixed some of the most
important bugs, so if you're interested in data access in F#, [F# Data](https://github.com/fsharp/FSharp.Data)
is the right tool for you!

I included a quick overview of some of the type providers that are available in the 
library - including those for WorldBank, Freebase, CSV, XML and JSON. Of course, I did
not cover all the features of the library. You can find more information in the
[detailed documentation](http://fsharp.github.com/FSharp.Data/).

### Contribute to F# Data

As I mentioned already, the library already had some great contributors. Gustavo Guerra did
a great job on making it work in Portable profile and on Silverlight. However, there
is always work to be done :-) and contributors are very welcome. If you're interested, 
check out the [list of issues](https://github.com/fsharp/FSharp.Data/issues). I also
wrote a page on [contributing to F# Data](http://fsharp.github.com/FSharp.Data/contributing.html)
with basic information about the library structure. 
*)