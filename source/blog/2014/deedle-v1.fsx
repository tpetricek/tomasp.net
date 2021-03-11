(**

New features and improvements in Deedle v1.0
============================================

 - date: 2014-05-27T15:41:01.7294268+01:00
 - description: As Howard Mansell already announced we have officially released the '1.0' version of Deedle. In this blog post, I'll have a quick look at a couple of new features in Deedle. Howard's announcement has a more detailed list, but I just want to give a couple of examples and briefly comment on performance improvements we did.
 - layout: article
 - image: http://tomasp.net/blog/2014/deedle-v1/mpg-per-cyl.png
 - tags: f#,deedle,data science
 - title: New features and improvements in Deedle v1.0
 - url: 2014/deedle-v1

--------------------------------------------------------------------------------
 - standalone


As Howard Mansell already [announced on the BlueMountain Tech blog](http://techblog.bluemountaincapital.com/2014/05/21/deedle-v1-0-release/),
we have officially released the "1.0" version of Deedle. In case you have not 
heard of Deedle yet, it is a .NET library for interactive data analysis and 
exploration. Deedle works great with both C# and F#. It provides two main data
structures: _series_ for working with data and time series and _frame_ for working
with collections of series (think CSV files, data tables etc.)

The great thing about Deedle is that it has been becoming a foundational library
that makes it possible to integrate a wide range of diverse data-science components.
For example, the [R type provider](http://bluemountaincapital.github.io/FSharpRProvider/)
works well with Deedle and so does [F# Charting](http://fsharp.github.io/FSharp.Charting/).
We've been also working on integrating all of these into a single package called 
[FsLab](https://github.com/tpetricek/FsLab), but more about that next time! 

In this blog post, I'll have a quick look at a couple of new features in Deedle 
(and corresponding R type provider release). Howard's announcement has a 
[more detailed list](http://techblog.bluemountaincapital.com/2014/05/21/deedle-v1-0-release/), 
but I just want to give a couple of examples and briefly comment on performance
improvements we did.


--------------------------------------------------------------------------------
*)
(*** hide ***)
#nowarn "1189"
#I "../packages"
#load "FSharp.Charting.0.84/FSharp.Charting.fsx"
System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
open System
open FSharp.Charting
let shouldEqual a b = ()
type TestAttribute() = 
  inherit System.Attribute()
type PerfTestAttribute(Iterations:int) = 
  inherit System.Attribute()
(**

As Howard Mansell already [announced on the BlueMountain Tech blog](http://techblog.bluemountaincapital.com/2014/05/21/deedle-v1-0-release/),
we have officially released the "1.0" version of Deedle. In case you have not 
heard of Deedle yet, it is a .NET library for interactive data analysis and 
exploration. Deedle works great with both C# and F#. It provides two main data
structures: _series_ for working with data and time series and _frame_ for working
with collections of series (think CSV files, data tables etc.)

The great thing about Deedle is that it has been becoming a foundational library
that makes it possible to integrate a wide range of diverse data-science components.
For example, the [R type provider](http://bluemountaincapital.github.io/FSharpRProvider/)
works well with Deedle and so does [F# Charting](http://fsharp.github.io/FSharp.Charting/).
We've been also working on integrating all of these into a single package called 
[FsLab](https://github.com/tpetricek/FsLab), but more about that next time! 

In this blog post, I'll have a quick look at a couple of new features in Deedle 
(and corresponding R type provider release). Howard's announcement has a 
[more detailed list](http://techblog.bluemountaincapital.com/2014/05/21/deedle-v1-0-release/), 
but I just want to give a couple of examples and briefly comment on performance
improvemens we did.

What's new in Deedle?
---------------------

Perhaps the most visible difference in the new version is that many of the functions
are renamed. We thought that before v1.0, we had a unique chance to get the naming 
right, so we did a lot of renamings to make sure that everything is consistent. For 
example, some functions used _series_ and some _column_, some used _sort_ and others
_order_ and so on. This should now be cleaned up. Similarly, we fixed a number of 
mismatches between `Series` and `Frame` modules.

### Additions to Deedle API

Aside from renaming, we also added a couple of useful functions. For example, the 
[homepage sample](http://bluemountaincapital.github.io/Deedle/) compares survival
ration for different passenger classes. This can now be done even more easily using
`PivotTable`:

*)
#load "Deedle.1.0.0/Deedle.fsx"
open Deedle

let titanic = Frame.ReadCsv("../data/titanic.csv")

// Pivot operation using "Sex" as row 
// and "Survived" as a new column
titanic 
|> Frame.pivotTable 
    (fun _ row -> row.GetAs<string>("Sex"))
    (fun _ row -> row.GetAs<bool>("Survived"))
    Frame.countRows

// The same operation using method notation
titanic.PivotTable<string, bool, _>
  ("Sex", "Survived", Frame.countRows)
(**
The operation groups the rows according to the two keys and then performs aggregation
using the specified function (here `Frame.countRows`). This is a common operation and 
so we wanted to make it as simple as possible. We also continue to expose operations
both as F# functions in modules and as C#-friendly methods.

Another example where we did lot of improvements is statistics:
*)
let msft = Frame.ReadCsv<DateTime>("../data/msft.csv", "Date")
msft?Open |> Stats.movingVariance 100
msft?Open |> Stats.expandingMean
msft?Open |> Stats.kurt
(**
The first improvement is that you can now specify key column when loading data from a CSV
file (again, this is very common). The same feature is available when loading data from 
a sequence of .NET objects using `Frame.ofRows`.

The next new thing is the `Stats` module. This is the new place for all functions related
to statistics and numerical computations. We found that adding more functions to `Series`
and `Frame` modules was a bit confusing, so we moved all statistical functions in one place.
This is even more important now that we added more functions (kurtosis, skewness, variance)
and we added more ways to calculate them (moving and expanding windows). For  more information
see the [frame and series statistics](http://bluemountaincapital.github.io/Deedle/stats.html) page.

### Improved documentation

Finally, one of the strong points of Deedle is that it has an excellent documentation. This 
is now even more the case, because we polished the documentation automatically generated from
Markdown comments in the source code. In particular, for the three core modules:

 - [**Series** module](http://bluemountaincapital.github.io/Deedle/reference/deedle-seriesmodule.html)
   provides functions for working with individual data series and time-series values. This includes
   operations such as sampling, transformations, data access and more.
 - [**Frame** module](http://bluemountaincapital.github.io/Deedle/reference/deedle-framemodule.html)
  `provides functions that are similar to those in the <code>Series</code> module, but operate 
   on entire data frames. You can transform, align and join frames, perform various re-indexing
   operations etc.
 - [**Stats** module](http://bluemountaincapital.github.io/Deedle/reference/deedle-stats.html)
   implements standard statistical functions (mean, variance, kurtosis, skewness, etc.) over 
   series, moving windows, expanding windows and a lot more. The module contains functions 
   for both series and frames.


What's new in the R provider?
-----------------------------

Together with a new release of Deedle, we also updated the R type provider. There
are a couple of improvements that make it work a lot better:

 * The installation from NuGet does no longer rely on PowerShell installation 
   script, so it can work on Mono and when using the "Restore Packages" feature.
 * The type provider communicates with R via a separate process, so it is more 
   stable and it will also let us call 64bit version of R.

These are technical, but very important improvements. However, we also added one
nice new feature that makes it even easier to mix R and F#!

### RData type provider

In R, you can save workspaces (environments) into `*.rdata` files. This is useful
if you want to archive results of some interactive analysis done in the R environment.
But, wouldn't it be nice if you could do some data analysis in R and then save the
data to a file and load it easily from F# in a type-safe way?

This is exactly what you get with the `RData` type provider! Let's say that I have
`cars.rdata` file containing the `mtcars` data set (saved under the name `cars`) 
together with a list `mpg` and a value `mpgMean`. I can write:
*)
#load "RProvider.1.0.9/RProvider.fsx"
open RProvider

let file = new RData<"../data/cars.rdata">()

// Calculate mean in R and in F#
let mean1 = file.mpg |> Array.average
let mean2 = file.mpgMean.[0]

// Average mpg based on cylinder count
file.cars
|> Frame.groupRowsByInt "cyl"
|> Frame.getCol "mpg"
|> Stats.levelMean fst
|> Series.observations
(**

If you look at the types, you'll see that `file.mpg` is of type `float[]` and
`file.cars` is of type `Frame<string, string>`. The R type provider uses the installed
plugins (like the Deedle plugin) to find the most appropriate F# type for exposing
the data and so the R data frame `cars` is automatically exposed as Deedle frame.

This lets us quickly group the values by "cyl" (number of cylinders) and then 
calculate average miles per gallon "mpg" for each of the groups. Using F# Charting,
the result looks like this:

<div style="text-align:center;margin-right:50px;">
<img src="mpg-per-cyl.png" style="width:450px" />
</div>

Deedle performance improvements
-------------------------------
*)
(*** hide ***)
let r1 = series [1 => 1.0]
let r2 = series [1 => 1.0]
let r3 = series [1 => 1.0]
(**
In this release of Deedle, we spent some time on improving the performance. The first
version was designed with performance in mind and the internals make it possible to 
implement operations efficiently (e.g. in F#, it is quite easy to write code so that 
the data is stored in continuous memory blocks). However, there were a number of places
where some Deedle function just used the "simplest stupid way to get things done".

This was nice, because it let us quickly build a sophisticated and easy to use API,
but there were cases where things were just too slow. So, improving performance is an 
ongoing effort and if you find a use case where Deedle is slow, please
[submit an issue](https://github.com/BlueMountainCapital/Deedle/issues)!

### Measuring performance

To make sure we can monitor the performance, I created a fairly simple tool that lets
us measure performance automatically. This is currently available in 
[my branch](https://github.com/tpetricek/Deedle/tree/tpetricek.PerfTests/tests).
The tool is started [via a FAKE script](https://github.com/tpetricek/Deedle/blob/tpetricek.PerfTests/tests/PerformanceTools/performance.fsx)
and it measures the performance of all tests in [a specified file](https://github.com/tpetricek/Deedle/blob/tpetricek.PerfTests/tests/Deedle.PerfTests/Performance.fs).
The tests also serve as unit tests. For example:
*)
[<Test;PerfTest(Iterations=10)>]
let ``Merge 3 unordered 300k long series (repeating Merge)`` () =
  r1.Merge(r2).Merge(r3).KeyCount |> shouldEqual 900000
(**
The `PerfTest` attribute specifies that the function is a performance test and it
also lets us specify number of iterations (so that we run quick tests repeatedly, but
slow tests only a few times). 

### Absolute performance 

I did two simple analyses of the performance. The first chart compares the new version
of Deedle with the previous version available on NuGet:

<p style="padding-left:15px">
<strong style="color:#1f77b4">&#8226; v0.9.12 (November 2013)</strong><br />
<strong style="color:#68C433">&#8226; v1.0.0 (May 2014)</strong>
</p>

The numbers represent the total number of milliseconds needed to run the test. Note
that the X axis is limited to 10 seconds, but some of the tests actually take longer
using the old version. Also, some tests only have value when using the new version - 
this is because they are using function that is new in v1.0.

<iframe src="output-abs.html" style="width:640px;height:630px;border-style:none;">
</iframe>

A couple of points worth mentioning:

 * Some of the notable improvements are when merging series - this also applies to 
   joining of frames (e.g. when applying numerical operations). We also added 
   overload of `Merge` on frames that can merge multiple series at once, which is
   significantly faster (and lets you merge e.g. 1000 frames, which was previously
   too slow).

 * There is a number of improvements in `Resample` operations. Again, this is just
   an example of a more general speedup (that also affects windowing and chunking
   functions).

### Relative performance

In the previous chart, it is a bit difficult to see what is the greatest performance
improvement. In the following chart, the tests are scaled so that the performance
using original version (0.9.12) is used as 100% and the performance using the new 
version is shown as a percentage (so cutting 10sec down to 5sec shows as 50%)

<iframe src="output-rel.html" style="width:640px;height:560px;border-style:none;">
</iframe>

Again, you can see a number of interesting things:

 * The biggest speedup is on "Accessing float series via object series". This is 
   the case when you access a column on a frame using `df.Columns` (which returns
   a series of `ObjectSeries<'K>` values). Because we do not know the type of 
   individual columns, we return them as series containing `obj` values. In the 
   new version, this does not actually box the values and so converting the series
   back to `Series<'K, float>` is essentially no-op.

 * We also did some work on improving grouping (and related) operations, so, for 
   example the [homepage sample](http://bluemountaincapital.github.io/Deedle/) is now
   about twice as fast. There is still (a lot of) room for improvement, but as you
   can see, we're working hard on this!

 * The joining and merging operations are about 6x faster, but for `Merge` this is
   even more significant when you're merging multiple frames.

The tests that I included here are by no means comprehensive. They simply represent
a couple of test cases that I was working on. However, with the performance measurements
in place, we should be able to use this more and more often! So, if you have an 
interesting use case, submit a pull request [adding a performance test](https://github.com/tpetricek/Deedle/blob/tpetricek.PerfTests/tests/Deedle.PerfTests/Performance.fs)!

Summary
-------

The "1.0" release of Deedle is an important milestone. Although Deedle has been 
around since November (and it has been used internally by BlueMountain), the 
"1.0" release means that the library is becoming more stable and ready for others
to adopt.

Of course, there is always room for improvement. There are operations that could
be faster (please report them!), there are functions that should be added 
(please suggest them!) and there are likely a few remaining bugs. I marked some
issues as [up-for-grabs](https://github.com/BlueMountainCapital/Deedle/issues?labels=up-for-grabs&page=1&state=open)
in case you wanted to contribute directly.

Another important thing about Deedle is that it is a foundational component
around which we can build an awesome .NET data science stack. If you're interested,
register at [www.fslab.org](http://fslab.org/) and follow this blog for more
information.

There are many people who contributed to Deedle (and R provider), but the projects
wouldn't exist without [Howard Mansell](https://github.com/hmansell) and
[Adam Klein](https://github.com/adamklein) at BlueMountain. A lot of the R provider
work has been done by [David Charboneau](https://github.com/dcharbon).  **Thanks!**
*)
