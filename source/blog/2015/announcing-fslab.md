Announcing FsLab: Data science package
======================================

 - date: 2015-05-05T15:55:01.9000642+01:00
 - description: After over a year of working on FsLab and talking about it at conferences, it is finally time for an official announcement. So, today, I'm excited to announce FsLab a cross-platform package for doing data science with .NET and Mono.
 - layout: article
 - image: http://tomasp.net/blog/2015/announcing-fslab/fslab.png
 - tags: f#,fslab,data science
 - title: Announcing FsLab: Data science package for Mono and .NET
 - url: 2015/announcing-fslab
 - format: bakedin

--------------------------------------------------------------------------------
<img src="http://tomasp.net/blog/2015/announcing-fslab/fslab.png" style="width:120px;float:right;margin:10px" />
<p>After over a year of working on FsLab and talking about it at conferences, it is finally time
for an official announcement. So, today, I'm excited to announce <a href="http://www.fslab.org">FsLab</a> -
a cross-platform package for doing data science with .NET and Mono.</p>
<p>It is probably not necessary to explain why data science is an important area. We live
surrounded by information, but extracting useful knowledge from the vast amounts of data is
not an easy task. You have to access data in different formats (JSON-based REST services, XML,
CSV files or even HTML tables), you need to deal with missing values, combine and align data
from multiple sources and then build visualizations (or reports) to tell the right story.</p>
<p>The goal of FsLab is to make this process easier. FsLab combines the power of F# type providers,
the efficiency and robustness of Mono and .NET and the high quality engineering of the
open-source ecosystem around F# and C#.</p>


--------------------------------------------------------------------------------
<h1><span class="hm">Announcing FsLab</span><span class="hs"> Data science package</span></h1>
<img src="http://tomasp.net/blog/2015/announcing-fslab/fslab.png" style="width:120px;float:right;margin:10px" />
<p>After over a year of working on FsLab and talking about it at conferences, it is finally time
for an official announcement. So, today, I'm excited to announce <a href="http://www.fslab.org">FsLab</a> -
a cross-platform package for doing data science with .NET and Mono.</p>
<p>It is probably not necessary to explain why data science is an important area. We live
surrounded by information, but extracting useful knowledge from the vast amounts of data is
not an easy task. You have to access data in different formats (JSON-based REST services, XML,
CSV files or even HTML tables), you need to deal with missing values, combine and align data
from multiple sources and then build visualizations (or reports) to tell the right story.</p>
<p>The goal of FsLab is to make this process easier. FsLab combines the power of F# type providers,
the efficiency and robustness of Mono and .NET and the high quality engineering of the
open-source ecosystem around F# and C#.</p>
<h2>FsLab links and resources</h2>
<ul>
<li>
<p>You can find more information about FsLab at our <a href="http://fslab.org/">recently launched web site</a>.
The web site is <a href="https://github.com/fslaborg/fslaborg.github.io">hosted on GitHub</a>, so please
send corrections and improvements as pull requests!</p>
</li>
<li>
<p>To get started, check out the <a href="http://fslab.org/download/">downloads page</a>. The easiest option
is to use one of the two templates <a href="https://github.com/fslaborg/FsLab.Templates">also hosted on GitHub</a>.
You can get some inspiration from the <a href="http://fslab.org/getting-started/">getting started tutorial</a>.</p>
</li>
<li>
<p>If you want to help us shape the future of FsLab, then please <a href="https://groups.google.com/group/fsharp-data-science">join the F# Data
Science group</a> on Google. For GitHub-related
topics (projects, etc.) there is also <a href="https://github.com/fslaborg/fslaborg.admin/issues">FsLab admin repository</a>.</p>
</li>
</ul>
<h2>FsLab questions and answers</h2>
<p>Rather than writing a long introduction about FsLab, the following tries to answer the most
important questions that you might have about FsLab using the Q &amp; A format.</p>
<p><strong>Why should I choose FsLab over X?</strong><br />
There is a couple of things that FsLab does exceptionally well. With <a href="http://fsharp.github.io/FSharp.Data/">F# Data type
providers</a>, you get type-safe access to a wide range
of external data sources with tooling that no other data science package can offer. FsLab also
runs on Mono and .NET and so it is extremely easy to turn your experiments into production-quality
code. For many other tasks, you can easily call other tools such as R using the <a href="http://bluemountaincapital.github.io/FSharpRProvider/">R type
provider</a>.</p>
<p><strong>Is FsLab only for F#?</strong><br />
No. Some of the libraries that are a part of FsLab have excellent C# support - most importantly,
Deedle, which is the core library for working with data frames and data series has
<a href="http://bluemountaincapital.github.io/Deedle/csharpintro.html">an excellent C# support</a>. The
libraries that rely on type providers are F#-only, but you can use them and then expose the
functionality to C#, Visual Basic .NET or any other .NET language.</p>
<p><strong>Who is behind FsLab?</strong><br />
FsLab is a community effort with a large number of contributors - both individuals and companies.
<a href="https://www.bluemountaincapital.com">BlueMountain Capital</a> is funding the development of R type
provider and Deedle, F# Data is maintained by <a href="http://twitter.com/ovatsus">Gustavo Guerra</a> and
contributors, Math.NET is maintained by <a href="https://twitter.com/cdrnet">Christoph Rüegg</a> and contributors.
Finally, I'm the maintainer of the FsLab package. Commercial support and training for FsLab is
available from <a href="http://www.fsharpworks.com/">fsharpWorks</a>.</p>
<p><strong>What is the FsLab roadmap?</strong><br />
There is no official roadmap yet. Please help us shape it by joining the discussion! However,
there are a couple of things that are coming to FsLab very soon:</p>
<ul>
<li>
We're integrating FsLab with <a href="http://tahahachana.github.io/XPlot/">XPlot</a> to 
provide cross-platform HTML5 charting.
</li>
<li>
We're working on <a href="https://github.com/fslaborg/FsLab.Templates/tree/journal">FsLab Journal template</a> which
lets you generate reports from scripts.
</li>
<li>
We're integrating FsLab with <a href="http://www.m-brace.net/">M-Brace</a>, which lets you
scale your scripts to the cloud.
</li>
<li>
We're working on <a href="https://twitter.com/tomaspetricek/status/575349159726870528">BigDeedle</a>, a new
backend for Deedle that makes it possible to treat big data as ordinary frames and series.
</li>
</ul>
<h2>Demonstrating the FsLab approach</h2>
<p>I don't want to turn this announcement into a technical post about FsLab, but since FsLab
is very much about technology, I'll give you at least a quick demo. The demo illustrates
the 2 key ideas that FsLab follows:</p>
<img src="cc.png" style="float:right;margin:0px 0px 10px 10px; width:200px" />
<ul>
<li>
<p><strong>Access, analyze, visualize cycle</strong> - when doing data science, you typically follow this
cycle a number of times. You get some data, try to explore it, visualize the results and
then repeat. FsLab gives you great tools for all three steps.</p>
</li>
<li>
<p><strong>Integrate with leading technologies</strong> - FsLab has some great libraries and excells in
some areas (like data access). For other tasks, it can integrate with other technologies -
it lets you call R packages and visualize data using Google Charts.</p>
</li>
</ul>
<p>To start with FsLab, you need to <a href="http://fslab.org/download/">download FsLab package or a template</a>.
Then you can write an F# script file that references FsLab and opens all necessary namespaces:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="prep">#load</span> <span class="s">&quot;packages/FsLab/FsLab.fsx&quot;</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="i">Deedle</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="i">FSharp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="i">Data</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs4', 4)" onmouseover="showTip(event, 'fs4', 4)" class="i">XPlot</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs5', 5)" onmouseover="showTip(event, 'fs5', 5)" class="i">GoogleCharts</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs4', 6)" onmouseover="showTip(event, 'fs4', 6)" class="i">XPlot</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs5', 7)" onmouseover="showTip(event, 'fs5', 7)" class="i">GoogleCharts</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs6', 8)" onmouseover="showTip(event, 'fs6', 8)" class="i">Deedle</span>
</code></pre></td>
</tr>
</table>
<p>The example uses <a href="http://fsharp.github.io/FSharp.Data">F# Data</a> for data access,
<a href="http://bluemountaincapital.github.io/Deedle/">Deedle</a> for working with time series and
<a href="http://tahahachana.github.io/XPlot/">XPlot</a> for producing Google Charts.</p>
<p>We'll use the <a href="http://fsharp.github.io/FSharp.Data/library/WorldBank.html">World Bank type provider</a>
to get the population in the largest city of Czech Republic as a time series. When writing
the code in F#-enabled editor, you'll get auto-completion offering all countries of the world
and thousands of indicators:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs7', 9)" onmouseover="showTip(event, 'fs7', 9)" class="i">wb</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs8', 10)" onmouseover="showTip(event, 'fs8', 10)" class="t">WorldBankData</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs9', 11)" onmouseover="showTip(event, 'fs9', 11)" class="f">GetDataContext</span>()
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs10', 12)" onmouseover="showTip(event, 'fs10', 12)" class="i">pop</span> <span class="o">=</span> 
  <span onmouseout="hideTip(event, 'fs7', 13)" onmouseover="showTip(event, 'fs7', 13)" class="i">wb</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 14)" onmouseover="showTip(event, 'fs11', 14)" class="i">Countries</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 15)" onmouseover="showTip(event, 'fs11', 15)" class="i">``Czech Republic``</span>
   <span class="o">.</span><span class="i">Indicators</span><span class="o">.</span><span class="i">``Population in largest city``</span>
  <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs12', 16)" onmouseover="showTip(event, 'fs12', 16)" class="f">series</span>
</code></pre></td>
</tr>
</table>
<p>The <code>|&gt;</code> operator passes the data from World Bank to the <code>series</code> function to create a
Deedle series that gives you a nice way to explore the data.
When you run the code in F# REPL, you'll see a printout showing the first few years and
last few years of the time series (Prague had 1,000,830 inhabitants in 1960 and 1,302,883
inhabitants in 2014).</p>
<p>Next, we'll use the <a href="http://bluemountaincapital.github.io/FSharpRProvider">R type provider</a> to
call the R <code>stats</code> package to calculate linear regression:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">open</span> <span onmouseout="hideTip(event, 'fs13', 17)" onmouseover="showTip(event, 'fs13', 17)" class="i">RProvider</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs13', 18)" onmouseover="showTip(event, 'fs13', 18)" class="i">RProvider</span><span class="o">.</span><span class="i">stats</span>

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs14', 19)" onmouseover="showTip(event, 'fs14', 19)" class="i">df</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs15', 20)" onmouseover="showTip(event, 'fs15', 20)" class="f">frame</span> [ <span class="s">&quot;pop&quot;</span> <span class="o">=&gt;</span> <span onmouseout="hideTip(event, 'fs10', 21)" onmouseover="showTip(event, 'fs10', 21)" class="i">pop</span> ]
<span onmouseout="hideTip(event, 'fs14', 22)" onmouseover="showTip(event, 'fs14', 22)" class="i">df</span><span class="o">?</span><span class="i">years</span> <span class="o">&lt;-</span> <span onmouseout="hideTip(event, 'fs10', 23)" onmouseover="showTip(event, 'fs10', 23)" class="i">pop</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs16', 24)" onmouseover="showTip(event, 'fs16', 24)" class="i">Keys</span>
<span onmouseout="hideTip(event, 'fs14', 25)" onmouseover="showTip(event, 'fs14', 25)" class="i">df</span><span class="o">?</span><span class="i">predict</span> <span class="o">&lt;-</span> <span class="i">R</span><span class="o">.</span><span class="i">predict</span>(<span class="i">R</span><span class="o">.</span><span class="i">lm</span>(<span class="s">&quot;pop~years&quot;</span>, <span onmouseout="hideTip(event, 'fs14', 26)" onmouseover="showTip(event, 'fs14', 26)" class="i">df</span>))<span class="o">.</span><span class="i">GetValue</span><span class="o">&lt;</span><span onmouseout="hideTip(event, 'fs17', 27)" onmouseover="showTip(event, 'fs17', 27)" class="i">float</span>[]<span class="o">&gt;</span>()
</code></pre></td>
</tr>
</table>
<p>The first two lines reference the R type provider. Again, thanks to the type provider mechanism,
you get auto-completion on <code>RProvider.</code> (with all installed R packages) and on <code>R.</code> (with all
available R functions).</p>
<p>The code then creates a Deedle data frame <code>df</code> with columns <code>pop</code> (from World Bank data),
<code>years</code> (with the keys of the <code>pop</code> series) and then it uses <code>R.lm</code> and <code>R.predict</code> to
calculate linear regression model and use it to predict values for the current range of years.</p>
<p>With three more lines of code, we can build a Google Charts chart comparing the actual data
with the data predicted by the linear regression model:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp">[ <span onmouseout="hideTip(event, 'fs14', 28)" onmouseover="showTip(event, 'fs14', 28)" class="i">df</span><span class="o">?</span><span class="i">predict</span>; <span onmouseout="hideTip(event, 'fs14', 29)" onmouseover="showTip(event, 'fs14', 29)" class="i">df</span><span class="o">?</span><span onmouseout="hideTip(event, 'fs18', 30)" onmouseover="showTip(event, 'fs18', 30)" class="i">pop</span> ] 
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs19', 31)" onmouseover="showTip(event, 'fs19', 31)" class="t">Chart</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs20', 32)" onmouseover="showTip(event, 'fs20', 32)" class="f">Line</span>
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs19', 33)" onmouseover="showTip(event, 'fs19', 33)" class="t">Chart</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs21', 34)" onmouseover="showTip(event, 'fs21', 34)" class="f">WithOptions</span> (<span onmouseout="hideTip(event, 'fs22', 35)" onmouseover="showTip(event, 'fs22', 35)" class="t">Options</span>(<span class="i">title</span><span class="o">=</span><span class="s">&quot;Prague Population&quot;</span>))
</code></pre></td>
</tr>
</table>
<p>I embedded the chart below by hand, but you can also use the <a href="http://fslab.org/download/">FsLab Journal
template</a>, which produces the HTML automatically from
your F# script:</p>
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
<h2>Summary</h2>
<p>FsLab is a collection of high quality libraries for doing data science on Mono and .NET.
It combines the power of F# type providers for data access, it lets you easily explore ideas,
while writing code for a robust platform that is easy to deploy.</p>
<p>Many of the libraries that are included in FsLab have been around for some time, have been
used in production and have a large number of contributors, both from the open-source community
and from commercial companies.</p>
<p>Even the FsLab package itself existed for some time - but with this announcement, the project
reaches a new milestone. We've done a lot of work on making FsLab stable, well documented and
truly cross-platform over the last few months and many more things are coming in the near future.
So stay tuned, <a href="https://groups.google.com/group/fsharp-data-science">send us feedback</a>,
<a href="https://github.com/fslaborg/fslaborg.admin/blob/master/CONTRIBUTING.md">contribute</a> and
<a href="http://www.fslab.org">try FsLab now</a>!</p>


<div class="tip" id="fs1">namespace Deedle</div>
<div class="tip" id="fs2">Multiple items<br />namespace FSharp<br /><br />--------------------<br />namespace Microsoft.FSharp</div>
<div class="tip" id="fs3">Multiple items<br />namespace FSharp.Data<br /><br />--------------------<br />namespace Microsoft.FSharp.Data</div>
<div class="tip" id="fs4">namespace XPlot</div>
<div class="tip" id="fs5">namespace XPlot.GoogleCharts</div>
<div class="tip" id="fs6">module Deedle<br /><br />from XPlot.GoogleCharts</div>
<div class="tip" id="fs7">val wb : WorldBankData.ServiceTypes.WorldBankDataService<br /><br />Full name: Announcing-fslab.wb</div>
<div class="tip" id="fs8">type WorldBankData =<br />&#160;&#160;static member GetDataContext : unit -&gt; WorldBankDataService<br />&#160;&#160;nested type ServiceTypes<br /><br />Full name: FSharp.Data.WorldBankData</div>
<div class="tip" id="fs9">WorldBankData.GetDataContext() : WorldBankData.ServiceTypes.WorldBankDataService</div>
<div class="tip" id="fs10">val pop : Series&lt;key,&#39;a&gt;<br /><br />Full name: Announcing-fslab.pop</div>
<div class="tip" id="fs11"></div>
<div class="tip" id="fs12">val series : observations:seq&lt;&#39;a * &#39;b&gt; -&gt; Series&lt;&#39;a,&#39;b&gt; (requires equality)<br /><br />Full name: Deedle.FSharpSeriesExtensions.series</div>
<div class="tip" id="fs13">namespace RProvider</div>
<div class="tip" id="fs14">val df : Frame&lt;key,string&gt;<br /><br />Full name: Announcing-fslab.df</div>
<div class="tip" id="fs15">val frame : columns:seq&lt;&#39;a * #ISeries&lt;&#39;c&gt;&gt; -&gt; Frame&lt;&#39;c,&#39;a&gt; (requires equality and equality)<br /><br />Full name: Deedle.FSharpFrameExtensions.frame</div>
<div class="tip" id="fs16">property Series.Keys: seq&lt;key&gt;</div>
<div class="tip" id="fs17">Multiple items<br />val float : value:&#39;T -&gt; float (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.float<br /><br />--------------------<br />type float = System.Double<br /><br />Full name: Microsoft.FSharp.Core.float<br /><br />--------------------<br />type float&lt;&#39;Measure&gt; = float<br /><br />Full name: Microsoft.FSharp.Core.float&lt;_&gt;</div>
<div class="tip" id="fs18">val pop : Series&lt;key,Frame&lt;&#39;a,&#39;b&gt;&gt; (requires equality and equality)<br /><br />Full name: Announcing-fslab.pop</div>
<div class="tip" id="fs19">type Chart =<br />&#160;&#160;static member Annotation : data:seq&lt;#seq&lt;DateTime * &#39;V * string * string&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires &#39;V :&gt; value)<br />&#160;&#160;static member Annotation : data:seq&lt;DateTime * #value * string * string&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Area : data:seq&lt;#seq&lt;&#39;K * &#39;V&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires &#39;K :&gt; key and &#39;V :&gt; value)<br />&#160;&#160;static member Area : data:seq&lt;#key * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Bar : data:seq&lt;#seq&lt;&#39;K * &#39;V&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires &#39;K :&gt; key and &#39;V :&gt; value)<br />&#160;&#160;static member Bar : data:seq&lt;#key * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Bubble : data:seq&lt;string * #value * #value * #value * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Bubble : data:seq&lt;string * #value * #value * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Bubble : data:seq&lt;string * #value * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Calendar : data:seq&lt;DateTime * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;...<br /><br />Full name: XPlot.GoogleCharts.Chart</div>
<div class="tip" id="fs20">static member Chart.Line : data:Frame&lt;&#39;K,&#39;V&gt; * ?Options:Options -&gt; GoogleChart (requires equality and equality)<br />static member Chart.Line : data:Series&lt;&#39;K,#value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires equality and &#39;K :&gt; key)<br />static member Chart.Line : data:seq&lt;Series&lt;&#39;K,#value&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires equality and &#39;K :&gt; key)<br />static member Chart.Line : data:seq&lt;#seq&lt;&#39;K * &#39;V&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires &#39;K :&gt; key and &#39;V :&gt; value)<br />static member Chart.Line : data:seq&lt;#key * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart</div>
<div class="tip" id="fs21">static member Chart.WithOptions : options:Options -&gt; chart:GoogleChart -&gt; GoogleChart</div>
<div class="tip" id="fs22">Multiple items<br />type Options =<br />&#160;&#160;new : unit -&gt; Options<br />&#160;&#160;member ShouldSerializeaggregationTarget : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeallValuesSuffix : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeallowHtml : unit -&gt; bool<br />&#160;&#160;member ShouldSerializealternatingRowStyle : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeanimation : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeannotations : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeannotationsWidth : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeareaOpacity : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeavoidOverlappingGridLines : unit -&gt; bool<br />&#160;&#160;...<br /><br />Full name: XPlot.GoogleCharts.Configuration.Options<br /><br />--------------------<br />new : unit -&gt; Options</div>
