(**

FnuPlot: Cross-platform charting with gnuplot
=============================================

 - date: 2015-01-15T16:58:17.9508797+00:00
 - description: There is a bunch of visualization and charting libraries for F#. Sadly, perhaps the most advanced ones do not work particularly well outside of Windows. This blog post introduces a new work-in-progress library called FnuPlot which is a lightweight and cross-platform wrapper for gnuplot.
 - layout: post
 - image: http://fsprojects.github.io/FnuPlot/img/logo.png
 - tags: f#,fslab,data science
 - title: FnuPlot: Cross-platform charting with gnuplot
 - url: 2015/fnuplot

--------------------------------------------------------------------------------
 - standalone

<img src="http://fsprojects.github.io/FnuPlot/img/logo.png" style="width:120px;float:right;margin:10px" />

There is a bunch of visualization and charting libraries for F#. Sadly, perhaps the most
advanced one, [F# Charting](http://fsharp.github.io/FSharp.Charting/), does not work 
particularly well outside of Windows at the moment. There are also some work-in-progress
libraries based on HTML like [Foogle Charts](http://fsprojects.github.io/Foogle.Charts/) and
[FsPlot](http://tahahachana.github.io/FsPlot/), which are cross-platform, but not quite
ready yet. 

Before Christmas, I got a [notification from GitHub](https://github.com/fsprojects/FnuPlot/pull/2)
about a pull request for a simple gnuplot wrapper that I wrote a long time ago (and which used
to be carefully hidden [on CodePlex](http://fsxplat.codeplex.com)).

The library is incomplete and I don't expect to dedicate too much time to maintaining it,
but it works quite nicely for basic charts and so I though I'd add the 
[ProjectScaffold](http://fsprojects.github.io/ProjectScaffold/) structure, do a few tweaks
and make it available as a modern F# project.


--------------------------------------------------------------------------------
*)
(*** hide ***)
#I "../packages"
#r "FnuPlot.0.0.5-beta/lib/net40/FnuPlot.dll"
#r "FSharp.Data.2.1.1/lib/net40/FSharp.Data.dll"
let path = @"C:\Programs\Data\gnuplot\bin\gnuplot.exe"
(**

<img src="http://fsprojects.github.io/FnuPlot/img/logo.png" style="width:120px;float:right;margin:10px" />

There is a bunch of visualization and charting libraries for F#. Sadly, perhaps the most
advanced one, [F# Charting](http://fsharp.github.io/FSharp.Charting/), does not work 
particularly well outside of Windows at the moment. There are also some work-in-progress
libraries based on HTML like [Foogle Charts](http://fsprojects.github.io/Foogle.Charts/) and
[FsPlot](http://tahahachana.github.io/FsPlot/), which are cross-platform, but not quite
ready yet. 

Before Christmas, I got a [notification from GitHub](https://github.com/fsprojects/FnuPlot/pull/2)
about a pull request for a simple gnuplot wrapper that I wrote a long time ago (and which used
to be carefully hidden [on CodePlex](http://fsxplat.codeplex.com)).

The library is incomplete and I don't expect to dedicate too much time to maintaining it,
but it works quite nicely for basic charts and so I though I'd add the 
[ProjectScaffold](http://fsprojects.github.io/ProjectScaffold/) structure, do a few tweaks
and make it available as a modern F# project.

 * [FnuPlot documentation & web site](http://fsprojects.github.io/FnuPlot/)
 * Get [FnuPlot from Nuget](http://www.nuget.org/packages/FnuPlot) or use [Paket GitHub dependency](http://fsprojects.github.io/FnuPlot/tutorial.html#Installing-and-configuring-FnuPlot)
 * [FnuPlot source code on GitHub](https://github.com/fsprojects/FnuPlot)

Introducing FnuPlot
-------------------

FnuPlot is a simple DSL for composing charts. In some ways, it is similar to
[F# Charting](http://fsharp.github.io/FSharp.Charting/), but it has a few specific
aspects that are designed based on how gnuplot works.

Assuming you already have FnuPlot referenced from NuGet, you can start by 
creating a new instance of `GnuPlot` (this is `IDisposable` and the `Dispose` method
stops the underlying `gnuplot` process). The constructor takes a full path to 
`gnuplot` as an argument, in case this is not available in your `PATH`:
*)
open FnuPlot
open System.Drawing

let path_win = "C:\Program Files\gnuplot\bin\Wgnuplot.exe"
let path_nix = "/usr/local/bin/gnuplot"
let gp = new GnuPlot(path)
(**
To create charts, you can now use `gp.Plot`. This has a couple of overloads. The
most basic one just takes a string with the function you want to plot:
*)
gp.Plot("sin(x)")
(**
![](sin.png)

If you want to create charts based on data calculated in F#, then you'll need to
use a type called `Series`. This provides static methods for creating various kinds
of series (lines, histograms, ...). The following creates a line series from X and Y
values and a function series with additional configuration:
*)
// Line created from X and Y values
Series.XY [for x in 0.0 .. 0.5 .. 10.0 -> x, sin x]
|> gp.Plot

// Function with specified title & line color
Series.Function
  ( "sin(x)", title="sin", 
    lineColor=Color.BurlyWood )
|> gp.Plot
(**

Here, we're using an overload of `gp.Plot` that takes a single series. You can also
call it with a sequence of series, to combine multiple lines into a single chart.

The following combines the simple function chart with a (not very smooth) line generated
using an F# list comprehension:
*)
[ Series.XY
    ( [for x in 0.0 .. 0.5 .. 10.0 -> x, sin x], 
      title="sin", lineColor=Color.BurlyWood, weight=2 )
  Series.Function
    ( "cos(x)", title="cos", 
      lineColor=Color.DodgerBlue, weight=2) ]
|> gp.Plot
(**
![](sincos.png)

Here, we're calling `gp.Plot` with the pipeline operator and we only specified the data.
However, the `gp.Plot` method has a number of other optional parameters that can be used
to configure how the chart looks. You can, for example, specify the range:
*)
let series = (***[omit:(Same as above)]***)
  [ Series.XY
      ( [for x in 0.0 .. 0.5 .. 10.0 -> x, sin x], 
        title="sin", lineColor=Color.BurlyWood, weight=2 )
    Series.Function
      ( "cos(x)", title="cos", 
        lineColor=Color.DodgerBlue, weight=2) ](***[/omit]***)

gp.Plot( series, range = Range.[0.0 .. 3.14, -1.5 .. 1.5] )
(**
The DSL for specifying ranges is using F# range expressions, which is a nice trick (it
does not actually generate a range!) and you can read more about it [in the 
documentation](http://fsprojects.github.io/FnuPlot/tutorial.html#Configuring-ranges-and-styles).

Visualizing WorldBank data
--------------------------

To look at a larger example, I'm going to use the usual WorldBank type provider 
from [F# Data](http://fsharp.github.io/FSharp.Data/) and create a chart showing 
inequality using the [GINI index](http://en.wikipedia.org/wiki/Gini_coefficient) for
the countries of the Eurozone (paying with Euro).

First, let's generate some colours for the lines of the chart. The following snippet
uses a couple of pre-defined colours and than adds a darker version to the palette:
*)
/// Base colors converted from HTML format
let coreColors = 
  [ "#5DA5DA"; "#FAA43A"; "#60BD68"; "#F17CB0"; 
    "#B2912F"; "#B276B2"; "#DECF3F"; "#F15854" ]
  |> Seq.map ColorTranslator.FromHtml

/// Infinite sequence with core colors followed by
/// a darker version and then repeated recursively
let rec allColors = seq {
  yield! coreColors
  for c in coreColors do 
    yield Color.FromArgb(int c.R/2, int c.G/2, int c.B/2) 
  yield! allColors }
(**
If you're a gnuplot expert, you can configure the palete directly. The `gp` object provides
a method `gp.SendCommand` where you can send arbitrary command to gnuplot. Here, we're 
going to specify colours explicitly using the `lineColor` parameter.

Now, we want to iterate over all countries in a specified region, get the GINI index
values and construct a list of `Series.XY` charts that can then be passed to `gp.Plot`.
The whole snippet looks as follows:
*)
open FSharp.Data

// Get EURO area countries from WorldBank
let wb = WorldBankData.GetDataContext()
let euro = wb.Regions.``Euro area``.Countries

// Specify chart range (to get space for legend)
gp.Set(range=RangeX.[.. 2020.0])

//
[ for color, country in Seq.zip allColors euro ->
    let values = 
      country.Indicators.``GINI index``
      |> Seq.map (fun (y, v) -> float y, v)
    Series.XY(values, title=country.Name, weight=2, lineColor=color) ]
|> gp.Plot

(**
![](countries.png)

One last interesting thing demonstrated by the above snippet is the `gp.Set` function.
You can use this to configure a number of properties globally, for all subsequent charts.

Looking for contributors!
-------------------------

This article shows a couple of things that you can do with the current FnuPlot library.
However, as I mentioned before, the library is really a fairly simple prototype that
I implemented a long time ago. I think using gnuplot is a good way to get nice cross-platform
charts, so I did a bit more work and turned it into a proper F# project under the
fsprojects GitHub organization. But there is a lot that needs to be done if we want to 
support all of gnuplot. So if you're interested, [start discussions & send pull requests
for FnuPlot on GitHub](https://github.com/fsprojects/FnuPlot)!

*)
