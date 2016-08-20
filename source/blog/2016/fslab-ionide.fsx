(**

Better F# data science with FsLab and Ionide
============================================

 - date: 2016-07-06T16:03:23.8677625+01:00
 - description: The most recent version of Ionide comes with a completely revamped version of F# Interactive which makes it possible to format the results of running F# code as HTML. This blog post provides some of the details about how this works and it also introduces an Ionide integration with FsLab, which gives you new powerful tools for doing data science with F#.
 - layout: post
 - image: http://tomasp.net/blog/2016/fslab-ionide/prices.png
 - tags: f#,fslab,data science
 - title: Better F# data science with FsLab and Ionide
 - url: 2016/fslab-ionide

--------------------------------------------------------------------------------
 - standalone


At [NDC Oslo 2016](http://ndcoslo.com/), I did a talk about some of the recent new F# projects
that are making data science with F# even nicer than it used to be. The talk covered a wider range
of topics, but one of the nice new thing I showed was the improved F# Interactive in the [Ionide
plugin for Atom](http://www.ionide.io/) and the integration with FsLab libraries that it provides.

In particular, with the latest version of [Ionide](http://ionide.io) for [Atom](http://atom.io)
and the latest version of [FsLab package](http://www.fslab.org), you can run code in F# Interactive
and you'll see resulting time series, data frames, matrices, vectors and charts as nicely pretty
printed HTML objects, right in the editor. The following shows some of the features (click on it
for a bigger version):

<a href="http://tomasp.net/blog/2016/fslab-ionide/prices.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/prices.png" style="margin:15px 2% 25px 2%;width:96%" />
</a>

In this post, I'll write about how the new Ionide and FsLab integration works, how you can use
it with your own libraries and also about some of the future plans. You can also learn more by
getting the FsLab package, or watching the NDC talk..

--------------------------------------------------------------------------------
*)
(*** hide ***)
#I "../packages/2016/"
(**

At [NDC Oslo 2016](http://ndcoslo.com/), I did a talk about some of the recent new F# projects
that are making data science with F# even nicer than it used to be. The talk covered a wider range
of topics, but one of the nice new thing I showed was the improved F# Interactive in the [Ionide
plugin for Atom](http://www.ionide.io/) and the integration with FsLab libraries that it provides.

In particular, with the latest version of [Ionide](http://ionide.io) for [Atom](http://atom.io)
and the latest version of [FsLab package](http://www.fslab.org), you can run code in F# Interactive
and you'll see resulting time series, data frames, matrices, vectors and charts as nicely pretty
printed HTML objects, right in the editor. The following shows some of the features (click on it
for a bigger version):

<a href="http://tomasp.net/blog/2016/fslab-ionide/prices.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/prices.png" style="margin:0px 5% 15px 5%;width:90%" />
</a>

In this post, I'll write about how the new Ionide and FsLab integration works, how you can use
it with your own libraries and also about some of the future plans. You can also learn more by
getting the FsLab package, or watching the NDC talk:

 * [Analysing Big Time-series Data in the Cloud](https://vimeo.com/171317247) is my NDC Oslo 2016
   talk. It shows the new Ionide + FsLab integration, but also uses [BigDeedle](https://github.com/BlueMountainCapital/Deedle.BigDemo)
   and [MBrace](http://www.mbrace.io) to interactively process large data in the cloud.

 * [FsLab downloads page](http://fslab.org/download/) has templates that you can download to get
   started. Just install [Atom](http://atom.io) with [Ionide](http://ionide.io), download the FsLab
   basic template and you're good to go!

 * For more background on FsLab as well as additional examples, check out my [FsLab announcement
   from last year](http://tomasp.net/blog/2015/announcing-fslab/). This explains what is (and is
   not) FsLab, how you can contribute and much more.

## FsLab formatters for Ionide

[FsLab is just a NuGet package](http://www.nuget.org/packages/FsLab) that references a number
of other F# packages for doing data science with F#. The one thing that it adds is an easy to
use load script that you can use to load all the packages from F# interactive. This means
that when you download the template, the sample script file starts with something like this:
*)
#load "packages/FsLab/Themes/AtomChester.fsx"
#load "packages/FsLab/FsLab.fsx"

open Deedle
open FSharp.Data
open XPlot.GoogleCharts
open XPlot.GoogleCharts.Deedle
(**
The first line loads a default _theme_ that configures how embedded charts and tables
will be formatted. It sets things like float formatting options, colours, fonts etc.
You can find and contribute themes in the [FsLab.Formatters repository](https://github.com/fslaborg/FsLab.Formatters/tree/master/src/Themes) -
the current choice covers only one white and one dark theme for Atom. The second line
is the more important one, which loads the FsLab dependencies.

The basic template comes with a minimal example that downloads two time series
from the World Bank and finds the years when they were the most different:
*)
let wb = WorldBankData.GetDataContext()

let cz = wb.Countries.``Czech Republic``.Indicators
let eu = wb.Countries.``European Union``.Indicators

let czschool = series cz.``Gross enrolment ratio, tertiary, both sexes (%)``
let euschool = series eu.``Gross enrolment ratio, tertiary, both sexes (%)``

// Get 5 years with the largest difference between EU and CZ
abs (czschool - euschool)
|> Series.sort
|> Series.rev
|> Series.take 5
(**
When you run the code in Atom, a formatter for Deedle series should make it easy
to see the result of the last expression - make sure to run the last 4 lines of
the snippet as a _separate interaction_. Ionide will only show the formatted object
if the formattable object is the _result_ of the snippet. Alternatively, you can also
select `czschool` or `euschool` and run Alt+Enter to see one of the source series:

<a href="http://tomasp.net/blog/2016/fslab-ionide/series.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/series.png" style="margin:0px 5% 15px 5%;width:90%" />
</a>

Aside from Deedle series, the FsLab package registers formatters for the charting libraries
that it comes with. This includes [F# Charting](http://www.fslab.org/FSharp.Charting) (Windows-only),
[XPlot Google charts](https://tahahachana.github.io/XPlot/google-charts.html) and also
[XPlot Plotly charts](https://tahahachana.github.io/XPlot/plotly.html). The following example
plots the two time-series using the XPlot wrapper for Google charts:
*)
[ czschool.[1975 .. 2010]; euschool.[1975 .. 2010] ]
|> Chart.Line
|> Chart.WithOptions (Options(legend=Legend(position="bottom")))
|> Chart.WithLabels ["CZ"; "EU"]
(**

The Google chart is formatted according to the theme that we loaded on the first line of the script,
so it looks nicely integrated with the F# Interactive window (but as I mentioned, we need your help
with adding more than just the [two standard Atom themes](https://github.com/fslaborg/FsLab.Formatters/tree/master/src/Themes)).

<a href="http://tomasp.net/blog/2016/fslab-ionide/chart.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/chart.png" style="margin:0px 5% 15px 5%;width:90%" />
</a>

One of the nice aspects of how the FsLab and Ionide integration works is that it is not ad-hoc
integration for just a couple of selected libraries - quite the opposite! All the FsLab formatters
live in a [separate repository](https://github.com/fslaborg/FsLab.Formatters) from Ionide and
you can create your own formatters that will work in exactly the same way. The following section
has more details about the underlying mechanism behind all this.

## Creating custom HTML formatters

<img src="http://ionide.io/FsInteractiveService/img/logo.png" class="rdecor" style="width:120px" />

The latest release of [ionide-fsi](https://atom.io/packages/ionide-fsi), which is the F# Interactive
plugin for Atom no longer runs `fsi.exe` in the background (like Visual Studio or all other editors),
but instead it is based on the brand new [FsInteractiveService](http://ionide.io/FsInteractiveService/).
This is a light-weight server that wraps the F# Interactive functionality. It can be consumed by any
editor via HTTP and it exposes API for [evaluating F# code](http://ionide.io/FsInteractiveService/http.html)
but also for [getting autocompletion and other hints](http://ionide.io/FsInteractiveService/intelli.html).

The FsInteractiveService extends the standard F# Interactive functionality with the ability to [format
objects as HTML](http://ionide.io/FsInteractiveService/htmlprinter.html). The idea is very simple. You
call `fsi.AddHtmlPrinter` and specify a function that turns your object into an HTML string! When you
evaluate an expression that returns a value that has a registered formatter, Ionide will then display
it using your provided HTML formatter.

### Creating HTML formatter for tables

As a basic example, say you have a type that represents a table:
*)
type Table = Table of string[,]
(**
Now, we want to create a HTML formatter that will render the table as a `<table>` element. To do
this, all you need is to call `fsi.AddHtmlPrinter`. The FsInteractiveService also defines a
symbol `HAS_FSI_ADDHTMLPRINTER` and so it is a good idea to wrap the following code in a big
`#if HAS_FSI_ADDHTMLPRINTER` block - this way, the code will be compatible with F# Interactive in
Visual Studio and other editors that do not support HTML formatters (yet).

    fsi.AddHtmlPrinter(fun (Table t) ->
      let body =
        [ yield "<table>"
          for i in 0 .. t.GetLength(0)-1 do
            yield "<tr>"
            for j in 0 .. t.GetLength(1)-1 do
              yield "<td>" + t.[i,j] + "</td>"
            yield "</tr>"
          yield "</table>" ]
        |> String.concat ""
      seq [ "style", "<style>table { background:#f0f0f0; }</style>" ],
      body )

The result of the formatter function is actually `seq<string * string> * string`. The tuple consists
of two things:

 - The second element is the HTML body that represents the formatted value.
  Typically, editors will embed this into HTML output.

 - A sequence of key value pairs that represents additional styles and sripts used that are
   required by the body. The keys can be `style` or `script` (or other custom keys supported by
   editors) and can be treated in a special way by the editors (e.g. loading JavaScript
   dynamically in Atom requires placing the HTML content in an `<iframe>``).

You can now define a table as follows:
*)
let table =
  [ [ "Test"; "More"]
    [ "1234"; "5678"] ]
  |> array2D |> Table
(**
In the current version, the value is only formatted when `Table` is returned as a direct result
of an expression. This means that you need to evaluate an expression of type `Table` rather than,
for example, a value binding as above:
*)
table
(**
When you run the above in Atom, you will see a table formatted as HTML `<table>` element. (Some
more styling is needed to actually make this pretty, but this is a good start. Oh and did you
know that Atom supports the `<marquee>` tag?!)

### Themes, parameters and servers

In practice, there are a few other concerns that make formatting objects as HTML harder. For
example, some of the HTML formatters can implement lazy loading where they use a simple web server
running in the background to provide data to the view (which calls the server using JavaScript).
Also, it is nice if all the HTML formatters can share the same visual theme. To make these
possible, the FsInteractiveService also defines `fsi.HtmlPrinterParameters` which is a global
value of type `IDictionary<string, obj>` that can be used for storing various shared configuration.

For example, the `html-standalone-output` parameter specifies whether the generated HTML code
should be stand-alone, or whether it is allowed to use JavaScript to load data lazily (the latter
is used for Deedle frames in the talk and it means you can scroll through the data, but you need
to hava a server running in the background):

    #if HAS_FSI_ADDHTMLPRINTER
    let standaloneHtmlOutput =
      fsi.HtmlPrinterParameters.["html-standalone-output"] :?> bool
    #endif

There are a couple of examples of how this dictionary can be used in the standard FsLab formatters:

 * [The `DefaultWhite.fsx` file](https://github.com/fslaborg/FsLab.Formatters/blob/8dc5a1c372afa05025b5244c83494be83bfab3d9/src/Themes/DefaultWhite.fs)
   shows the different kind of parameters that you can specify for default FsLab formatters. You can
   copy & edit it to create new visual styles for FsLab (and send a PR to [FsLab.Formatters](https://github.com/fslaborg/FsLab.Formatters)
   if they correspond to a common Atom theme!)

 * [The XPlot formatter in `XPlot.fs`](https://github.com/fslaborg/FsLab.Formatters/blob/8dc5a1c372afa05025b5244c83494be83bfab3d9/src/Html/XPlot.fs#L79)
   is a good example of a formatter that reads the above visual styles and uses it to customize the look
   of the HTML it generates.

 * [The Deedle formatter in `Deedle.fs`](https://github.com/fslaborg/FsLab.Formatters/blob/8dc5a1c372afa05025b5244c83494be83bfab3d9/src/Html/Deedle.fs#L298)
   uses a lightweight Suave server running in the background to load data from a frame or series on demand.
   This is a good example of a more sophisticated formatter.

## FsLab Journal and looking ahead

### Formatting in FsLab journals

The [FsLab downloads page](http://fslab.org/download/) also lets you download a [FsLab Journal
template](https://github.com/fslaborg/FsLab.Templates/archive/journal.zip). This is something that
has been available in FsLab for longer time, but I never wrote much about it. The summary is:

> FsLab Journal lets you turn your F# scripts consisting of F# code snippets and Markdown
> formatted comments into a nice HTML report.

When you download the template, you can just run `build run` and your script will be turned into
a HTML report in the background. When you change your script, the background runner will upadate
and reload your report. If you want to produce stand-alone HTML (that does not require background
server), you can run `build html`. The following is an opened journal, running on my machine.

<a href="http://tomasp.net/blog/2016/fslab-ionide/journal.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/journal.png" style="margin:0px 5% 15px 5%;width:90%" />
</a>

In the latest version of FsLab, the formatting for journals is based on
the same `fsi.AddHtmlPrinter` formatters. This means we get to reuse the code for it, but most
importantly, when _your_ write your own formatter, it will work with both Ionide and also with
FsLab journals.

### Formatting in Jupyter notebooks

One of the related projects in the F# and data science space is the [F# bindings for Jupyter
Notebooks](https://github.com/fsprojects/IfSharp). This does not yet use the same model for
registering HTML formatters via `fsi.AddHtmlPrinter`. Instead, it has its own mechanism for
[registering printers](https://github.com/fsprojects/IfSharp/blob/jupyter/src/IfSharp.Kernel/Printers.fs),
but I expect that it will be possible to merge the two so that you can just write `fsi.AddHtmlPrinter`
once and use it in Ionide, FsLab Journals as well as Jupyter.
























*)
