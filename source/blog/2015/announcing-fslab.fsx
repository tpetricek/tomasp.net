(**

Announcing FsLab: Data science package
======================================

 - date: 2015-05-05T15:55:01.9000642+01:00
 - description: After over a year of working on FsLab and talking about it at conferences, it is finally time for an official announcement. So, today, I'm excited to announce FsLab a cross-platform package for doing data science with .NET and Mono.
 - layout: article
 - image: http://tomasp.net/blog/2015/announcing-fslab/fslab.png
 - tags: f#,fslab,data science
 - title: Announcing FsLab: Data science package for Mono and .NET
 - url: 2015/announcing-fslab

--------------------------------------------------------------------------------
 - standalone


<img src="http://tomasp.net/blog/2015/announcing-fslab/fslab.png" style="width:120px;float:right;margin:10px" />

After over a year of working on FsLab and talking about it at conferences, it is finally time 
for an official announcement. So, today, I'm excited to announce [FsLab](http://www.fslab.org) -
a cross-platform package for doing data science with .NET and Mono.

It is probably not necessary to explain why data science is an important area. We live 
surrounded by information, but extracting useful knowledge from the vast amounts of data is 
not an easy task. You have to access data in different formats (JSON-based REST services, XML, 
CSV files or even HTML tables), you need to deal with missing values, combine and align data 
from multiple sources and then build visualizations (or reports) to tell the right story.

The goal of FsLab is to make this process easier. FsLab combines the power of F# type providers,
the efficiency and robustness of Mono and .NET and the high quality engineering of the 
open-source ecosystem around F# and C#.


--------------------------------------------------------------------------------
*)
(*** hide ***)
#I "../packages/2015/"
(**

<img src="http://tomasp.net/blog/2015/announcing-fslab/fslab.png" style="width:120px;float:right;margin:10px" />

After over a year of working on FsLab and talking about it at conferences, it is finally time 
for an official announcement. So, today, I'm excited to announce [FsLab](http://www.fslab.org) -
a cross-platform package for doing data science with .NET and Mono.

It is probably not necessary to explain why data science is an important area. We live 
surrounded by information, but extracting useful knowledge from the vast amounts of data is 
not an easy task. You have to access data in different formats (JSON-based REST services, XML, 
CSV files or even HTML tables), you need to deal with missing values, combine and align data 
from multiple sources and then build visualizations (or reports) to tell the right story.

The goal of FsLab is to make this process easier. FsLab combines the power of F# type providers,
the efficiency and robustness of Mono and .NET and the high quality engineering of the 
open-source ecosystem around F# and C#.

FsLab links and resources
-------------------------

 - You can find more information about FsLab at our [recently launched web site](http://fslab.org/).
   The web site is [hosted on GitHub](https://github.com/fslaborg/fslaborg.github.io), so please
   send corrections and improvements as pull requests!

 - To get started, check out the [downloads page](http://fslab.org/download/). The easiest option
   is to use one of the two templates [also hosted on GitHub](https://github.com/fslaborg/FsLab.Templates).
   You can get some inspiration from the [getting started tutorial](http://fslab.org/getting-started/).

 - If you want to help us shape the future of FsLab, then please [join the F# Data 
   Science group](https://groups.google.com/group/fsharp-data-science) on Google. For GitHub-related
   topics (projects, etc.) there is also [FsLab admin repository](https://github.com/fslaborg/fslaborg.admin/issues).

FsLab questions and answers
---------------------------

Rather than writing a long introduction about FsLab, the following tries to answer the most 
important questions that you might have about FsLab using the Q &amp; A format.

**Why should I choose FsLab over X?**  
There is a couple of things that FsLab does exceptionally well. With [F# Data type 
providers](http://fsharp.github.io/FSharp.Data/), you get type-safe access to a wide range
of external data sources with tooling that no other data science package can offer. FsLab also
runs on Mono and .NET and so it is extremely easy to turn your experiments into production-quality
code. For many other tasks, you can easily call other tools such as R using the [R type 
provider](http://bluemountaincapital.github.io/FSharpRProvider/).

**Is FsLab only for F#?**  
No. Some of the libraries that are a part of FsLab have excellent C# support - most importantly, 
Deedle, which is the core library for working with data frames and data series has 
[an excellent C# support](http://bluemountaincapital.github.io/Deedle/csharpintro.html). The 
libraries that rely on type providers are F#-only, but you can use them and then expose the 
functionality to C#, Visual Basic .NET or any other .NET language.

**Who is behind FsLab?**  
FsLab is a community effort with a large number of contributors - both individuals and companies.
[BlueMountain Capital](https://www.bluemountaincapital.com) is funding the development of R type
provider and Deedle, F# Data is maintained by [Gustavo Guerra](http://twitter.com/ovatsus) and
contributors, Math.NET is maintained by [Christoph Rüegg](https://twitter.com/cdrnet) and contributors.
Finally, I'm the maintainer of the FsLab package. Commercial support and training for FsLab is 
available from [fsharpWorks](http://www.fsharpworks.com/).

**What is the FsLab roadmap?**  
There is no official roadmap yet. Please help us shape it by joining the discussion! However,
there are a couple of things that are coming to FsLab very soon:

 - We're integrating FsLab with [XPlot](http://tahahachana.github.io/XPlot/) to 
   provide cross-platform HTML5 charting.
 - We're working on [FsLab Journal template](https://github.com/fslaborg/FsLab.Templates/tree/journal) which
   lets you generate reports from scripts.
 - We're integrating FsLab with [M-Brace](http://www.m-brace.net/), which lets you
   scale your scripts to the cloud.
 - We're working on [BigDeedle](https://twitter.com/tomaspetricek/status/575349159726870528), a new
   backend for Deedle that makes it possible to treat big data as ordinary frames and series.

Demonstrating the FsLab approach
--------------------------------

I don't want to turn this announcement into a technical post about FsLab, but since FsLab
is very much about technology, I'll give you at least a quick demo. The demo illustrates
the 2 key ideas that FsLab follows:

<img src="cc.png" style="float:right;margin:0px 0px 10px 10px; width:200px" />

 - **Access, analyze, visualize cycle** - when doing data science, you typically follow this
   cycle a number of times. You get some data, try to explore it, visualize the results and
   then repeat. FsLab gives you great tools for all three steps.

 - **Integrate with leading technologies** - FsLab has some great libraries and excells in 
   some areas (like data access). For other tasks, it can integrate with other technologies - 
   it lets you call R packages and visualize data using Google Charts.

To start with FsLab, you need to [download FsLab package or a template](http://fslab.org/download/).
Then you can write an F# script file that references FsLab and opens all necessary namespaces:
*)
#load "packages/FsLab/FsLab.fsx"
open Deedle
open FSharp.Data
open XPlot.GoogleCharts
open XPlot.GoogleCharts.Deedle
(**
The example uses [F# Data](http://fsharp.github.io/FSharp.Data) for data access, 
[Deedle](http://bluemountaincapital.github.io/Deedle/) for working with time series and
[XPlot](http://tahahachana.github.io/XPlot/) for producing Google Charts.

We'll use the [World Bank type provider](http://fsharp.github.io/FSharp.Data/library/WorldBank.html)
to get the population in the largest city of Czech Republic as a time series. When writing
the code in F#-enabled editor, you'll get auto-completion offering all countries of the world
and thousands of indicators:
*)
let wb = WorldBankData.GetDataContext()
let pop = 
  wb.Countries.``Czech Republic``
   .Indicators.``Population in largest city``
  |> series
(**
The `|>` operator passes the data from World Bank to the `series` function to create a 
Deedle series that gives you a nice way to explore the data.
When you run the code in F# REPL, you'll see a printout showing the first few years and 
last few years of the time series (Prague had 1,000,830 inhabitants in 1960 and 1,302,883
inhabitants in 2014).

Next, we'll use the [R type provider](http://bluemountaincapital.github.io/FSharpRProvider) to
call the R `stats` package to calculate linear regression:
*)
open RProvider
open RProvider.stats

let df = frame [ "pop" => pop ]
df?years <- pop.Keys
df?predict <- R.predict(R.lm("pop~years", df)).GetValue<float[]>()
(**
The first two lines reference the R type provider. Again, thanks to the type provider mechanism,
you get auto-completion on `RProvider.` (with all installed R packages) and on `R.` (with all
available R functions). 

The code then creates a Deedle data frame `df` with columns `pop` (from World Bank data), 
`years` (with the keys of the `pop` series) and then it uses `R.lm` and `R.predict` to 
calculate linear regression model and use it to predict values for the current range of years.

With three more lines of code, we can build a Google Charts chart comparing the actual data
with the data predicted by the linear regression model:

*)
[ df?predict; df?pop ] 
|> Chart.Line
|> Chart.WithOptions (Options(title="Prague Population"))
(**
I embedded the chart below by hand, but you can also use the [FsLab Journal 
template](http://fslab.org/download/), which produces the HTML automatically from 
your F# script:

<div id="83a61d92-2bd7-4635-9840-4daaea0603fc" style="width: 600px; height: 350px;"></div>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript">
google.load("visualization", "1", {packages:["corechart"]})
google.setOnLoadCallback(drawChart);
function drawChart() {
  var data = new google.visualization.DataTable({"cols": [{"type": "string" ,"id": "Column 1" ,"label": "Column 1" }, {"type": "number" ,"id": "Column 2" ,"label": "Column 2" }, {"type": "number" ,"id": "Column 3" ,"label": "Column 3" }], "rows" : [{"c" : [{"v": "1960"}, {"v": 1044195.95064935}, {"v": 1000830}]}, {"c" : [{"v": "1961"}, {"v": 1048583.40634921}, {"v": 1007834}]}, {"c" : [{"v": "1962"}, {"v": 1052970.86204906}, {"v": 1015213}]}, {"c" : [{"v": "1963"}, {"v": 1057358.31774892}, {"v": 1022647}]}, {"c" : [{"v": "1964"}, {"v": 1061745.77344877}, {"v": 1030146}]}, {"c" : [{"v": "1965"}, {"v": 1066133.22914863}, {"v": 1037678}]}, {"c" : [{"v": "1966"}, {"v": 1070520.68484848}, {"v": 1045276}]}, {"c" : [{"v": "1967"}, {"v": 1074908.14054834}, {"v": 1052930}]}, {"c" : [{"v": "1968"}, {"v": 1079295.5962482}, {"v": 1060651}]}, {"c" : [{"v": "1969"}, {"v": 1083683.05194805}, {"v": 1068406}]}, {"c" : [{"v": "1970"}, {"v": 1088070.50764791}, {"v": 1076230}]}, {"c" : [{"v": "1971"}, {"v": 1092457.96334776}, {"v": 1085284}]}, {"c" : [{"v": "1972"}, {"v": 1096845.41904762}, {"v": 1095284}]}, {"c" : [{"v": "1973"}, {"v": 1101232.87474747}, {"v": 1105348}]}, {"c" : [{"v": "1974"}, {"v": 1105620.33044733}, {"v": 1115519}]}, {"c" : [{"v": "1975"}, {"v": 1110007.78614719}, {"v": 1125783}]}, {"c" : [{"v": "1976"}, {"v": 1114395.24184704}, {"v": 1136156}]}, {"c" : [{"v": "1977"}, {"v": 1118782.6975469}, {"v": 1146595}]}, {"c" : [{"v": "1978"}, {"v": 1123170.15324675}, {"v": 1157146}]}, {"c" : [{"v": "1979"}, {"v": 1127557.60894661}, {"v": 1167793}]}, {"c" : [{"v": "1980"}, {"v": 1131945.06464646}, {"v": 1178553}]}, {"c" : [{"v": "1981"}, {"v": 1136332.52034632}, {"v": 1184211}]}, {"c" : [{"v": "1982"}, {"v": 1140719.97604618}, {"v": 1187275}]}, {"c" : [{"v": "1983"}, {"v": 1145107.43174603}, {"v": 1190346}]}, {"c" : [{"v": "1984"}, {"v": 1149494.88744589}, {"v": 1193430}]}, {"c" : [{"v": "1985"}, {"v": 1153882.34314574}, {"v": 1196513}]}, {"c" : [{"v": "1986"}, {"v": 1158269.7988456}, {"v": 1199608}]}, {"c" : [{"v": "1987"}, {"v": 1162657.25454545}, {"v": 1202712}]}, {"c" : [{"v": "1988"}, {"v": 1167044.71024531}, {"v": 1205828}]}, {"c" : [{"v": "1989"}, {"v": 1171432.16594517}, {"v": 1208943}]}, {"c" : [{"v": "1990"}, {"v": 1175819.62164502}, {"v": 1212070}]}, {"c" : [{"v": "1991"}, {"v": 1180207.07734488}, {"v": 1212664}]}, {"c" : [{"v": "1992"}, {"v": 1184594.53304473}, {"v": 1208077}]}, {"c" : [{"v": "1993"}, {"v": 1188981.98874459}, {"v": 1203520}]}, {"c" : [{"v": "1994"}, {"v": 1193369.44444444}, {"v": 1198974}]}, {"c" : [{"v": "1995"}, {"v": 1197756.9001443}, {"v": 1194445}]}, {"c" : [{"v": "1996"}, {"v": 1202144.35584416}, {"v": 1189927}]}, {"c" : [{"v": "1997"}, {"v": 1206531.81154401}, {"v": 1185438}]}, {"c" : [{"v": "1998"}, {"v": 1210919.26724387}, {"v": 1180960}]}, {"c" : [{"v": "1999"}, {"v": 1215306.72294372}, {"v": 1176499}]}, {"c" : [{"v": "2000"}, {"v": 1219694.17864358}, {"v": 1172049}]}, {"c" : [{"v": "2001"}, {"v": 1224081.63434343}, {"v": 1172285}]}, {"c" : [{"v": "2002"}, {"v": 1228469.09004329}, {"v": 1181849}]}, {"c" : [{"v": "2003"}, {"v": 1232856.54574315}, {"v": 1191491}]}, {"c" : [{"v": "2004"}, {"v": 1237244.001443}, {"v": 1201224}]}, {"c" : [{"v": "2005"}, {"v": 1241631.45714286}, {"v": 1211011}]}, {"c" : [{"v": "2006"}, {"v": 1246018.91284271}, {"v": 1220890}]}, {"c" : [{"v": "2007"}, {"v": 1250406.36854257}, {"v": 1230850}]}, {"c" : [{"v": "2008"}, {"v": 1254793.82424242}, {"v": 1240906}]}, {"c" : [{"v": "2009"}, {"v": 1259181.27994228}, {"v": 1251015}]}, {"c" : [{"v": "2010"}, {"v": 1263568.73564214}, {"v": 1261221}]}, {"c" : [{"v": "2011"}, {"v": 1267956.19134199}, {"v": 1271510}]}, {"c" : [{"v": "2012"}, {"v": 1272343.64704185}, {"v": 1281883}]}, {"c" : [{"v": "2013"}, {"v": 1276731.1027417}, {"v": 1292340}]}, {"c" : [{"v": "2014"}, {"v": 1281118.55844156}, {"v": 1302883}]}]});
  var options = {"legend":{"position":"none"},"title":"Prague Population"} 
  var chart = new google.visualization.LineChart(document.getElementById('83a61d92-2bd7-4635-9840-4daaea0603fc'));
  chart.draw(data, options);
}
</script>

Summary
-------

FsLab is a collection of high quality libraries for doing data science on Mono and .NET.
It combines the power of F# type providers for data access, it lets you easily explore ideas,
while writing code for a robust platform that is easy to deploy.

Many of the libraries that are included in FsLab have been around for some time, have been
used in production and have a large number of contributors, both from the open-source community
and from commercial companies.

Even the FsLab package itself existed for some time - but with this announcement, the project
reaches a new milestone. We've done a lot of work on making FsLab stable, well documented and
truly cross-platform over the last few months and many more things are coming in the near future.
So stay tuned, [send us feedback](https://groups.google.com/group/fsharp-data-science), 
[contribute](https://github.com/fslaborg/fslaborg.admin/blob/master/CONTRIBUTING.md) and 
[try FsLab now](http://www.fslab.org)!


*)
