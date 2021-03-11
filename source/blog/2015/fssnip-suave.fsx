(**

Creating web sites with Suave: How to contribute to F# Snippets
===============================================================

 - date: 2015-09-15T23:26:01.8511959+01:00
 - description: The core of many web sites and web APIs is very simple. Given an HTTP request, produce a HTTP response. Sounds pretty simple, so why are there so many evil frameworks that make simple web programming difficult? In this blog post, I'll write about Suave -a nice composable library for web programming with F#. The blog post also shows a few interesting samples from the new version of F# Snippets and you are welcome to contribute!
 - layout: article
 - image: http://tomasp.net/blog/2015/fssnip-suave/logo.png
 - tags: f#,web
 - title: Creating web sites with Suave: How to contribute to F# Snippets
 - url: 2015/fssnip-suave

--------------------------------------------------------------------------------
 - standalone

<img src="http://tomasp.net/blog/2015/fssnip-suave/logo.png" style="width:130px;float:right;margin-left:10px" />

The core of many web sites and web APIs is very simple. Given an HTTP request,
produce a HTTP response. In F#, we can represent this as a function with type
`Request -> Response`. To make our server scalable, we should make the function
_asynchronous_ to avoid unnecessary blocking of threads. In F#, this can be 
captured as `Request -> Async<Response>`. Sounds pretty simple, right? So why 
are there so many [evil frameworks](http://tomasp.net/blog/2015/library-frameworks/)
that make simple web programming difficult?  

Fortunately, there is a nice F# library called [Suave.io](http://suave.io) that 
is based exactly on the above idea:

> Suave is a simple web development F# library providing a lightweight web server 
> and a set of combinators to manipulate route flow and task composition.

I recently decided to start a new version of the [F# Snippets](http://www.fssnip.net)
web site and I wanted to keep the implementation functional, simple, 
cross-platform and easy to contrbute to. I wrote a [first prototype of the 
implementation](https://github.com/tpetricek/FsSnip.Website/) using Suave and 
already received a few contributions via pull requests! In this blog post, I'll
share a few interesting aspects of the implementation and I'll give you some
good pointers where you can learn more about Suave. _There is no excuse for not
contributing to F# Snippets v2 after reading this blog post_!

--------------------------------------------------------------------------------
*)
(*** hide ***)
#r "../packages/2015/packages/Suave/lib/net40/Suave.dll"
#r "../packages/2015/packages/FSharp.Data/lib/net40/FSharp.Data.dll"
open System.IO
open Suave.Web
open Suave.Http
open Suave.Http.Files
open Suave.Http.Applicatives
open Suave.Http.Writers
(**


<img src="http://tomasp.net/blog/2015/fssnip-suave/logo.png" style="width:130px;float:right;margin-left:10px" />

The core of many web sites and web APIs is very simple. Given an HTTP request,
produce a HTTP response. In F#, we can represent this as a function with type
`Request -> Response`. To make our server scalable, we should make the function
_asynchronous_ to avoid unnecessary blocking of threads. In F#, this can be 
captured as `Request -> Async<Response>`. Sounds pretty simple, right? So why 
are there so many [evil frameworks](http://tomasp.net/blog/2015/library-frameworks/)
that make simple web programming difficult?  

Fortunately, there is a nice F# library called [Suave.io](http://suave.io) that 
is based exactly on the above idea:

> Suave is a simple web development F# library providing a lightweight web server 
> and a set of combinators to manipulate route flow and task composition.

I recently decided to start a new version of the [F# Snippets](http://www.fssnip.net)
web site and I wanted to keep the implementation functional, simple, 
cross-platform and easy to contrbute to. I wrote a [first prototype of the 
implementation](https://github.com/tpetricek/FsSnip.Website/) using Suave and 
already received a few contributions via pull requests! In this blog post, I'll
share a few interesting aspects of the implementation and I'll give you some
good pointers where you can learn more about Suave. _There is no excuse for not
contributing to F# Snippets v2 after reading this blog post_!

Getting started with Suave
--------------------------

I recently did a couple of talks about Suave at user groups and conferences and
many of them have been recorded. There are also a couple of nice examples online
and some good documentation on the official web site. So if you want to learn 
more about Suave, here are some links for you:

 * **Channel 9 Interview.** [Seth Juarez](https://twitter.com/sethjuarez) did 
   an interview with me when I was in Redmond and I did a quick 20 minute demo
   showing how to [Deploy an F# Web Application with Suave to 
   Azure](https://channel9.msdn.com/Blogs/Seth-Juarez/Deploying-an-F-Web-Application-with-Suave).
   This is the last part of a mini series, so you might also want to check out
   [Making the Case for using F#](http://channel9.msdn.com/Blogs/Seth-Juarez/Making-the-Case-for-using-F-with-Tomas-Petricek)
   if you are new to F#, [Domain Modeling in F#](http://channel9.msdn.com/Blogs/Seth-Juarez/Domain-Modeling-in-F-with-Tomas-Petricek)
   and [Type Providers in F#](http://channel9.msdn.com/Blogs/Seth-Juarez/Type-Providers-in-F-with-Tomas-Petricek).

 * **NDC Oslo Talk.** Next, I talked about Suave at NDC in Oslo. The talk shows
   two demo application - a web portal showing weather and news and a simple chat
   written using agents. The talk
   [End-to-end Functional Web Development](https://vimeo.com/131641270) has been 
   recorded and you can also get [the full source code](https://github.com/tpetricek/Talks/tree/master/2015/end-to-end-web/ndc/code-done).
   and [slides](http://tpetricek.github.io/Talks/2015/end-to-end-web/ndc/).
   It is also worth noting that I'm using the [awesome F# Atom plugin](https://github.com/fsprojects/atom-fsharp/)
   in the talk, together with some custom [FAKE build scripts](http://fsharp.github.io/FAKE/)
   to get a nice live reloading when developing the web sites. 
   
 * **Community for F# Talk.** [Henrik Feldt](https://twitter.com/henrikfeldt) who is one of
   the Suave contributors did a nice talk [Suave from Scratch](https://www.youtube.com/watch?v=ujxwW6fFXOc)
   on the Community for F# channel. This shows many more Suave features and so it is a
   great follow-up to the above. Also, Henrik is showing Suave [on Mac using Xamarin Studio](http://fsharp.org/use/mac/),
   so you can see that it truly is cross-platform.

 * **Web Site and Dojo.** For more information, there is a bunch of examples and 
   documentation on the [official Suave web site](http://suave.io/). This includes various
   ways of deploying Suave applications too. If you then want to get some hands-on
   experience, try completing [the simple Suave Dojo that I put together!](https://github.com/tpetricek/Dojo-Suave-FsHome)

<img src="video.png" style="width:75%;margin-left:12%;margin-bottom:20px" title="It is big. Really big. You just won't believe how vastly, hugely, mind-bogglingly big it is. I mean, you may think it's a long way down the road to the chemist, but that's just peanuts to space." />

Introducing F# Snippets v2
--------------------------

As already mentioned, I started using Suave for the new version of the F# Snippets web
site. The web site is basically a pastebin for F# code snippets. The nice thing is that
it uses [F# Formatting](http://tpetricek.github.io/FSharp.Formatting/) for formatting the
code snippets and generating tool tips. I never released the source code for the old
version, because it was simoply too ugly. The new version fixes this!

 * The [source code is on GitHub](https://github.com/tpetricek/FsSnip.Website/) - 
   to run it locally, you'll need to download sample data as discussed in the README.
 * The [prototype runs on Azure](http://fssnip.azurewebsites.net/) - this is
   automatically deployed from the `master` branch in the GitHub project and it
   runs as Azure Website.
 * And [here is a list of remaining issues before it can replace the old 
   version](https://github.com/tpetricek/FsSnip.Website/labels/status-priority) - 
   the project is quite simple, so this is a great place where you can contribute!
   
The previous version of F# Snippets stored all data in an SQL database. When creating the new
one, I was wondering what is the best option given the size of the web site. It turns out
that the meta-data about all the snippets is small enough to fit in memory (about 1MB
in JSON format) and so the new version is a lot simpler.

It keeps the meta-data in memory. The formatted snippets are stored in local file system
(when testing things locally) or in Azure blob storage (when running on Azure) - though
you can also use Azure storage during development. When the meta-data change, it is also
saved to a JSON file in the blob storage (so that it can be reloaded if the application 
is shut down).

You can find more details in the [project architecture section](https://github.com/tpetricek/FsSnip.Website/#project-architecture--structure)
of the project README document.

Interesting Suave snippets
--------------------------

There is a number of things that make Suave really nice to use. As you can have a look 
at the materials above to learn everything about it, I want to give you just a few
examples based on my experience with F# Snippets. 

The first nice thing about Suave is that it is a library rather than a framework. This
means that you are in control of starting and running the server. This makes it easy
to deploy it to Azure, Heroku or anywhere else. In F# Snippets, we have one entry-point
in the `app.fsx` file. This composes the server from individual components.

### Composing server from web parts

The [following code snippet](https://github.com/tpetricek/FsSnip.Website/blob/f857d7b84ba5603708db036a509bd3ca9141f0ca/app.fsx#L70)
shows how the server is composed. As you can see, we have functionality for showing
the home page, displaying snippets, inserting new snippets, listing snippets and the
RSS feed:
*)
let app = 
  choose 
    [ // When accessing '/' we display the homepage
      path "/" >>= Home.showHome 
      
      // Display snippet (latest, specific version and raw source)
      pathWithId "/%s" (fun id -> Snippet.showSnippet id Latest) 
      pathWithId "/raw/%s" (fun id -> Snippet.showRawSnippet id Latest) 
      pathScan "/%s/%d" 
        (fun (id, r) -> Snippet.showSnippet id (Revision r)) 
      pathScan "/raw/%s/%d"  
        (fun (id, r) -> Snippet.showRawSnippet id (Revision r)) 
      
      // Insert page, with simple REST API to check snippet for errors
      path "/pages/insert" >>= Insert.insertSnippet 
      path "/pages/insert/check" >>= Insert.checkSnippet 
      
      // Listing of snippets by author and by tag
      path "/authors/" >>= Author.showAll 
      pathScan "/authors/%s" Author.showSnippets 
      path "/tags/" >>= Tag.showAll 
      pathScan "/tags/%s" Tag.showSnippets 
      
      // Display RSS feed (allowing number of different path formats)
      ( path "/rss/" <|> path "/rss" <|> 
        path "/pages/Rss" <|> path "/pages/Rss/" ) >>= Rss.getRss 
      
      // Otherwise, try to process the request as a static file
      // (this handles all the CSS and JS files as well as images)
      browseStaticFiles ] 
(**
The `choose` combinator takes a list of _web parts_ and composes them. A Suave web
part is essentially one of those functions from the introduction - web parts can handle
requests and produce response. Here, we are building a single web part that goes through
the web parts in the list and uses the first one that can handle an incoming request.
The `path` combinator is used to restrict what requests a web part handles - so for 
example `path "/" >>= Home.showHome` means that we should display the home page if the
request is for the path `/`. A very nice function is `pathScan` - it takes an F# 
_format string_ and builds a web part that recognizes requests to URL with the specified
pattern. We can, for example, say `pathScan "/raw/%s/%d"` to detect URLs such as
`/raw/cJ/5`.

### Displaying snippets with DotLiquid

The Suave library does not force you to use any specific templating engine and I actually
used Suave for some time with just string concatenation or `str.Replace`. But if you want
to use some templating library, it is really easy to add support for it. To see just how
easy, look at my [pull request adding support for DotLiquid](https://github.com/SuaveIO/suave/pull/267).
We're using [DotLiquid](http://dotliquidmarkup.org/) in F# snippets, so here is how the
code looks.

The [code sample below](https://github.com/tpetricek/FsSnip.Website/blob/f857d7b84ba5603708db036a509bd3ca9141f0ca/code/pages/snippet.fs#L16)
shows how we handle request to display a snippet. We get the snippet ID, get information about
it from the meta-data and read the file from storage. If everything succeeds, we create
a record `FormattedSnippet` and pass it to the template loaded from `snippet.html`:
*)
type FormattedSnippet =
  { Html : string
    Details : Data.Snippet
    Revision : int }

let showSnippet id r =
  let id' = demangleId id
  let snippetOpt = 
    publicSnippets
    |> Seq.tryFind (fun s -> s.ID = id') 
  match snippetOpt with
  | Some snippetInfo -> 
      match Data.loadSnippet id r with
      | Some snippet ->
          { Html = snippet
            Details =
              Data.snippets 
              |> Seq.find (fun s -> s.ID = demangleId id)
            Revision =
              match r with 
              | Latest -> snippetInfo.Versions - 1 
              | Revision r -> r }
          |> DotLiquid.page<FormattedSnippet> "snippet.html"
      | None -> invalidSnippetId id
  | None -> invalidSnippetId id
(**
You can find the [full template on GitHub](https://github.com/tpetricek/FsSnip.Website/blob/f857d7b84ba5603708db036a509bd3ca9141f0ca/templates/snippet.html).
The value of the record is exposed as `model` and we can access its properties in the
template. For example, the heading is generated by `<h1>{{ model.Details.Title }}</h1>`.

### Checking F# code during insertion

The new F# Snippets web site reports all the errors in your F# code on the fly when 
you are inserting the snippet. Go to the [insert snippet page](http://fssnip.azurewebsites.net/pages/insert),
type some invalid F#, wait a second and you should see the compiler errors and warnings!

The implementation of this uses a simple JavaScript with timer and it calls the `/insert/check`
API end-point implemented by the server. This then returns a simple JSON with a list of
the errors and warning. 

This is another elegant piece of F# code that uses Suave composable web parts and 
the JSON type provider from [F# Data](http://fsharp.github.io/FSharp.Data/) to generate
the JSON response. Check out [the following snippet](https://github.com/tpetricek/FsSnip.Website/blob/f857d7b84ba5603708db036a509bd3ca9141f0ca/code/pages/insert.fs#L72):

*)
open FSharp.Data
type Errors = JsonProvider<"""
  [ {"location":[1,1,10,10], "error":true, "message":"sth"} ]""">

let noCache = 
  setHeader "Cache-Control" "no-cache, no-store, must-revalidate"
  >>= setHeader "Pragma" "no-cache"
  >>= setHeader "Expires" "0"
  >>= setMimeType "application/json"

let checkSnippet ctx = async {
  use sr = new StreamReader(new MemoryStream(ctx.request.rawForm))
  let request = sr.ReadToEnd()
  let doc = 
    Literate.ParseScriptString
      (request, "/temp/Snippet.fsx", formatAgent)
  let json = 
    JsonValue.Array
      [| for SourceError((l1,c1),(l2,c2),kind,msg) in doc.Errors ->
         Errors.Root
           ( [| l1; c1; l2; c2 |], 
             (kind = ErrorKind.Error), msg).JsonValue |]
             
  return! ctx |> (noCache >>= Successful.OK(json.ToString()) ) }
(**
There are a few nice things worth mentioning:

 * The example shows an interesting use of the JSON type provider. We give it a sample
   JSON (list with one error), but we're not using it to _read_ data but instead to 
   _generate_ response. As you can ee on line 20, we can then use the provided type
   `Errors.Root` to easily build a JSON value representing the error or warning.
   
 * We need to disable all caching in the HTTP response. To do this, we use _composition
   of web parts_. We define `noCache` which sets all the different HTTP headers
   required for this (lines 6-9) and then we use it when producing the result
   on line 24.

The compositional nature of Suave means that you can really easily define reusable
components and structure your code in the way that works for you. For F# Snippets, 
I wanted to make the project easy to contribute to, and so there is a fairly large
number of small independent files implementing the different components.

Summary
-------

This blog post had two purposes. First, I wanted to share some of the resources
that you might find useful if you want to learn about web development with F# using
Suave. There are many more information available on the internet, including [blog
post from Scott Hanselman](http://www.hanselman.com/blog/RunningSuaveioAndFWithFAKEInAzureWebAppsWithGitAndTheDeployButton.aspx) and
[a cool series by Claus Sørensen](http://blog.geist.no/suave-io-introduction-and-example-part-1-intro/),
so my list is just scratching the surface!

My second secret goal was to convince you to contribute to the [new F# Snippets 
project](https://github.com/tpetricek/FsSnip.Website/). Writing the prototype was a lot of
fun and I think you'd have fun contributing too. There is also a very large number of features
that people asked about (commenting, search, clustering, suggesting tags, etc.), so
I think anyone will find something interesting. To start with, there are [a few high-priority
issues](https://github.com/tpetricek/FsSnip.Website/labels/status-priority) that need
to be resolved before we can replace the old version.
*)
