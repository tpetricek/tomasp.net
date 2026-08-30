F# Data: New type provider library
==================================

 - date: 2013-03-28T03:23:41.0000000
 - description: F# Data is a new library that gives you all you need to access data in F# 3.0. It implements type providers for WorldBank, Freebase and structured document formats (CSV, JSON and XML) as well as other helpers. This article introduces the library and gives a quick overview of its features.
 - layout: article
 - tags: open source,f#,f# data,type providers
 - title: F# Data: New type provider library
 - url: fsharp-data.aspx
 - rawbody: true

--------------------------------------------------------------------------------
<img src="https://raw.github.com/fsharp/FSharp.Data/master/misc/logo.png" class="rdecor" style="width:120px;height:120px;" />
<p>When F# 3.0 type providers were still in beta version, I wrote a couple of type
providers as examples for talks. These included the WorldBank type provider
(now available <a href="http://www.tryfsharp.org">on Try F#</a>) and also type provider for
XML that infered the structure from sample.<br />
For some time, these were hosted as part of <a href="https://github.com/fsharp/fsharpx/">FSharpX</a>
and the authors of FSharpX also added a number of great features.</p>
<p>When I found some more time earlier this year, I decided to start a new library
that would be fully focused on data access in F# and on type providers and
I started working on <strong>F# Data</strong>. The library has now reached a stable state
and <a href="http://www.navision-blog.de/blog/2013/03/27/fsharpx-1-8-removes-support-for-document-type-provider/">Steffen also announced</a>
that the document type providers (JSON, XML and CSV) are not going to be available
in FSharpX since the next version.</p>
<p>This means that if you're interested in accessing data using F# type providers,
you should now go to F# Data. Here are the most important links:</p>
<ul>
<li><a href="https://github.com/fsharp/FSharp.Data">F# Data source code on GitHub</a></li>
<li><a href="http://fsharp.github.com/FSharp.Data/">F# Data documentation &amp; tutorials</a></li>
<li><a href="http://nuget.org/packages/FSharp.Data">F# Data on NuGet</a></li>
</ul>
<p>Before looking at the details, I would like to thank to <a href="https://github.com/ovatsus">Gustavo Guerra</a>
who made some amazing contributions to the library! (More contributors are always welcome,
so continue reading if you're interested...)</p>
--------------------------------------------------------------------------------
<h1><span class="hm">F# Data</span><span class="hs"> New type provider library</span></h1>
<img src="https://raw.github.com/fsharp/FSharp.Data/master/misc/logo.png" class="rdecor" style="width:120px;height:120px;" />
<p>When F# 3.0 type providers were still in beta version, I wrote a couple of type
providers as examples for talks. These included the WorldBank type provider
(now available <a href="http://www.tryfsharp.org">on Try F#</a>) and also type provider for
XML that infered the structure from sample.<br />
For some time, these were hosted as part of <a href="https://github.com/fsharp/fsharpx/">FSharpX</a>
and the authors of FSharpX also added a number of great features.</p>
<p>When I found some more time earlier this year, I decided to start a new library
that would be fully focused on data access in F# and on type providers and
I started working on <strong>F# Data</strong>. The library has now reached a stable state
and <a href="http://www.navision-blog.de/blog/2013/03/27/fsharpx-1-8-removes-support-for-document-type-provider/">Steffen also announced</a>
that the document type providers (JSON, XML and CSV) are not going to be available
in FSharpX since the next version.</p>
<p>This means that if you're interested in accessing data using F# type providers,
you should now go to F# Data. Here are the most important links:</p>
<ul>
<li><a href="https://github.com/fsharp/FSharp.Data">F# Data source code on GitHub</a></li>
<li><a href="http://fsharp.github.com/FSharp.Data/">F# Data documentation &amp; tutorials</a></li>
<li><a href="http://nuget.org/packages/FSharp.Data">F# Data on NuGet</a></li>
</ul>
<p>Before looking at the details, I would like to thank to <a href="https://github.com/ovatsus">Gustavo Guerra</a>
who made some amazing contributions to the library! (More contributors are always welcome,
so continue reading if you're interested...)</p>
<h2>F# Data Overview</h2>
<p>The library contains several type providers, a couple of helper functions and it also
comes with comprehensive documentation. Here is a quick summary of the key features:</p>
<ul>
<li>
<p><strong>Document type providers</strong> are providers for JSON, XML and CSV that
infer the structure of a file from a provided example and give you a typed
access to other data in the same format.</p>
</li>
<li>
<p><strong>WorldBank and Freebase</strong> are also hosted as part of the library. They give
you access to <a href="http://data.worldbank.org">WorldBank</a> indicators (information about countries) and to the
<a href="http://freebase.com">Freebase graph database</a> (this was originally written as
a sample by the F# team).</p>
</li>
<li>
<p><strong>Comprehensive documentation</strong> the library is using my other project,
<a href="https://github.com/tpetricek/FSharp.Formatting">F# Formatting</a> to automatically
generate a <a href="http://fsharp.github.com/FSharp.Data/">nice documentation</a> from
<code>*.fsx</code> script files with examples.</p>
</li>
<li>
<p><strong>HTTP utility</strong> the library also contains a very easy to use type for making
HTTP requests with just a single line (look for <code>Http.Request</code> in the <a href="http://fsharp.github.com/FSharp.Data/library/Http.html">documentation</a>.
This is something that I've been missing a lot when working with REST APIs from F#.</p>
</li>
</ul>
<h2>F# Data Code Samples</h2>
<p>I do not want to spend too much time demonstrating all the awesome features of the F# Data
library, but let me include just a few code snippets to demonstrate some interesting features.
(You can find more in the <a href="http://fsharp.github.com/FSharp.Data/">documentation</a>).</p>
<p>All the samples assume that we're using an F# Script file, so we start by referencing the
F# Data library using <code>#r</code> (in a project file, you would add reference as usual). I also
open two namespaces - <code>FSharp.Data</code> with the data-related API and <code>FSharp.Net</code> with a
helper type <code>Http</code> for making HTTP requests:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="pp">#r</span> <span class="s">&quot;FSharp.Data.dll&quot;</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="id">FSharp</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="id">Data</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs1', 3)" onmouseover="showTip(event, 'fs1', 3)" class="id">FSharp</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs3', 4)" onmouseover="showTip(event, 'fs3', 4)" class="id">Net</span>
</code></pre></td>
</tr>
</table>
<p>Now, let's quickly look at a number of examples that demonstrate the F# Data library.
You cannot quite see that in static code sample on a blog, but note that all data access
is done in a typed way. When you type <code>.</code>, you get a completion and if you make a typo,
you'll get an instantaneous feedback about the error.</p>
<h3>Geting government debt from WorldBank</h3>
<p>The <code>WorldBankData</code> type gives you access to the <a href="http://worldbank.org">World Bank</a> data
set. For example, we can look at "Czech Republic" and get the government debt for the
most recent year (using <code>Seq.maxBy</code> to get value for the most recent year available):</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs4', 5)" onmouseover="showTip(event, 'fs4', 5)" class="id">wb</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs5', 6)" onmouseover="showTip(event, 'fs5', 6)" class="rt">WorldBankData</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs6', 7)" onmouseover="showTip(event, 'fs6', 7)" class="id">GetDataContext</span><span class="pn">(</span><span class="pn">)</span>
<span onmouseout="hideTip(event, 'fs4', 8)" onmouseover="showTip(event, 'fs4', 8)" class="id">wb</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs7', 9)" onmouseover="showTip(event, 'fs7', 9)" class="id">Countries</span><span class="pn">.</span><span class="id">``Czech Republic``</span><span class="pn">.</span><span class="id">Indicators</span><span class="pn">.</span><span class="id">``Central government debt, total (% of GDP)``</span>
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs8', 10)" onmouseover="showTip(event, 'fs8', 10)" class="m">Seq</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs9', 11)" onmouseover="showTip(event, 'fs9', 11)" class="id">maxBy</span> <span onmouseout="hideTip(event, 'fs10', 12)" onmouseover="showTip(event, 'fs10', 12)" class="fn">fst</span>
</code></pre></td>
</tr>
</table>
<h3>Geting religion list from Freebase</h3>
<p>The <code>FreebaseData</code> type gives you access to <a href="http://freebase.com">Freebase</a>. You can just
type <code>.</code> and explore the data sources available - for example, to look at a list of
religions and print first 10:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs11', 13)" onmouseover="showTip(event, 'fs11', 13)" class="id">fb</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs12', 14)" onmouseover="showTip(event, 'fs12', 14)" class="rt">FreebaseData</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs13', 15)" onmouseover="showTip(event, 'fs13', 15)" class="id">GetDataContext</span><span class="pn">(</span><span class="pn">)</span>
<span class="k">for</span> <span onmouseout="hideTip(event, 'fs14', 16)" onmouseover="showTip(event, 'fs14', 16)" class="id">rel</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs11', 17)" onmouseover="showTip(event, 'fs11', 17)" class="id">fb</span><span class="pn">.</span><span class="id">Society</span><span class="pn">.</span><span class="id">Religion</span><span class="pn">.</span><span class="id">Religions</span> <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs8', 18)" onmouseover="showTip(event, 'fs8', 18)" class="m">Seq</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs15', 19)" onmouseover="showTip(event, 'fs15', 19)" class="id">take</span> <span class="n">10</span> <span class="k">do</span>
  <span onmouseout="hideTip(event, 'fs16', 20)" onmouseover="showTip(event, 'fs16', 20)" class="fn">printfn</span> <span class="s">&quot;</span><span class="pf">%s</span><span class="s">&quot;</span> <span onmouseout="hideTip(event, 'fs14', 21)" onmouseover="showTip(event, 'fs14', 21)" class="id">rel</span><span class="pn">.</span><span class="id">Name</span>
</code></pre></td>
</tr>
</table>
<h3>Parsing RSS news feed from BBC</h3>
<p>If you want to get news using RSS, you can use <code>XmlProvider</code>. All you need is a sample
file or a string (marked as <code>Literal</code>) with the RSS data. Then you can pass this string
or file to the provider as a static parameter and you'll get nice types for working with
RSS feeds. Here, we get news from BBC using the <code>Http.Request</code> helper:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
<span class="l">7: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span class="pn">[&lt;</span><span onmouseout="hideTip(event, 'fs17', 22)" onmouseover="showTip(event, 'fs17', 22)" class="rt">Literal</span><span class="pn">&gt;]</span> <span onmouseout="hideTip(event, 'fs18', 23)" onmouseover="showTip(event, 'fs18', 23)" class="id">RssSample</span> <span class="o">=</span> <span id="fst19" onmouseout="hideTip(event, 'fs19', 24)" onmouseover="showTip(event, 'fs19', 24, document.getElementById('fst19'))" class="omitted">(Sample RSS feed omitted)</span>
<span class="k">type</span> <span onmouseout="hideTip(event, 'fs20', 25)" onmouseover="showTip(event, 'fs20', 25)" class="rt">Rss</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs21', 26)" onmouseover="showTip(event, 'fs21', 26)" class="rt">XmlProvider</span><span class="pn">&lt;</span><span onmouseout="hideTip(event, 'fs18', 27)" onmouseover="showTip(event, 'fs18', 27)" class="id">RssSample</span><span class="pn">&gt;</span>

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs22', 28)" onmouseover="showTip(event, 'fs22', 28)" class="id">feed</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs20', 29)" onmouseover="showTip(event, 'fs20', 29)" class="rt">Rss</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs23', 30)" onmouseover="showTip(event, 'fs23', 30)" class="id">Parse</span><span class="pn">(</span><span onmouseout="hideTip(event, 'fs24', 31)" onmouseover="showTip(event, 'fs24', 31)" class="rt">Http</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs25', 32)" onmouseover="showTip(event, 'fs25', 32)" class="id">Request</span><span class="pn">(</span><span class="s">&quot;http://feeds.bbci.co.uk/news/rss.xml&quot;</span><span class="pn">)</span><span class="pn">)</span>
<span onmouseout="hideTip(event, 'fs16', 33)" onmouseover="showTip(event, 'fs16', 33)" class="fn">printfn</span>  <span class="s">&quot;</span><span class="pf">%s</span><span class="s">&quot;</span> <span onmouseout="hideTip(event, 'fs22', 34)" onmouseover="showTip(event, 'fs22', 34)" class="id">feed</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs26', 35)" onmouseover="showTip(event, 'fs26', 35)" class="id">Channel</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs27', 36)" onmouseover="showTip(event, 'fs27', 36)" class="id">Title</span>
<span class="k">for</span> <span onmouseout="hideTip(event, 'fs28', 37)" onmouseover="showTip(event, 'fs28', 37)" class="id">item</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs22', 38)" onmouseover="showTip(event, 'fs22', 38)" class="fn">feed</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs26', 39)" onmouseover="showTip(event, 'fs26', 39)" class="id">Channel</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs29', 40)" onmouseover="showTip(event, 'fs29', 40)" class="id">GetItems</span><span class="pn">(</span><span class="pn">)</span> <span class="k">do</span>
  <span onmouseout="hideTip(event, 'fs16', 41)" onmouseover="showTip(event, 'fs16', 41)" class="fn">printfn</span> <span class="s">&quot; - </span><span class="pf">%s</span><span class="s">&quot;</span> <span onmouseout="hideTip(event, 'fs28', 42)" onmouseover="showTip(event, 'fs28', 42)" class="id">item</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs30', 43)" onmouseover="showTip(event, 'fs30', 43)" class="id">Title</span>
</code></pre></td>
</tr>
</table>
<h3>Geting stock prices from Yahoo CSV</h3>
<p>Working with CSV files is similar. The <code>CsvProvider</code> takes static parameter with sample
data (either as a file name or as actual data). Here, we use a file and we also specify
that we only want to use first 10 rows for the inference (for performance reasons). The
provider infers column names and types. Here is how you calculate the average MSFT stock
price over the entire history:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs31', 44)" onmouseover="showTip(event, 'fs31', 44)" class="d">Stocks</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs32', 45)" onmouseover="showTip(event, 'fs32', 45)" class="rt">CsvProvider</span><span class="pn">&lt;</span><span class="s">&quot;data/fsharp-data/MSFT.csv&quot;</span><span class="pn">,</span> <span class="id">InferRows</span><span class="o">=</span><span class="n">10</span><span class="pn">&gt;</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs33', 46)" onmouseover="showTip(event, 'fs33', 46)" class="id">msft</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs31', 47)" onmouseover="showTip(event, 'fs31', 47)" class="d">Stocks</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs34', 48)" onmouseover="showTip(event, 'fs34', 48)" class="id">Load</span><span class="pn">(</span><span class="s">&quot;http://ichart.finance.yahoo.com/table.csv?s=MSFT&quot;</span><span class="pn">)</span>
<span onmouseout="hideTip(event, 'fs33', 49)" onmouseover="showTip(event, 'fs33', 49)" class="id">msft</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs35', 50)" onmouseover="showTip(event, 'fs35', 50)" class="id">Data</span> <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs8', 51)" onmouseover="showTip(event, 'fs8', 51)" class="m">Seq</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs36', 52)" onmouseover="showTip(event, 'fs36', 52)" class="id">averageBy</span> <span class="pn">(</span><span class="k">fun</span> <span onmouseout="hideTip(event, 'fs37', 53)" onmouseover="showTip(event, 'fs37', 53)" class="id">row</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs37', 54)" onmouseover="showTip(event, 'fs37', 54)" class="id">row</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs38', 55)" onmouseover="showTip(event, 'fs38', 55)" class="id">Open</span><span class="pn">)</span>
</code></pre></td>
</tr>
</table>
<h3>Geting list of F# snippets using REST API</h3>
<p>For our last example, we'll use REST API provided by <a href="http://fssnip.net">F# Snippets</a>. The
API returns a JSON data set containing information about snippets. We can easily use it by
defining a string <code>Literal</code> with sample JSON and passing it to <code>JsonProvider</code>. To get the
data, we use <code>Http.Request</code>, but this time we specify <code>Content-Type</code> header. Working
with the results is, again, done in a nice typed way:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l"> 1: </span>
<span class="l"> 2: </span>
<span class="l"> 3: </span>
<span class="l"> 4: </span>
<span class="l"> 5: </span>
<span class="l"> 6: </span>
<span class="l"> 7: </span>
<span class="l"> 8: </span>
<span class="l"> 9: </span>
<span class="l">10: </span>
<span class="l">11: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span class="pn">[&lt;</span><span onmouseout="hideTip(event, 'fs17', 56)" onmouseover="showTip(event, 'fs17', 56)" class="rt">Literal</span><span class="pn">&gt;]</span> <span onmouseout="hideTip(event, 'fs39', 57)" onmouseover="showTip(event, 'fs39', 57)" class="id">FsSnipNewsSample</span> <span class="o">=</span> <span id="fst40" onmouseout="hideTip(event, 'fs40', 58)" onmouseover="showTip(event, 'fs40', 58, document.getElementById('fst40'))" class="omitted">(Sample JSON with snippet data omitted)</span>
<span class="k">type</span> <span onmouseout="hideTip(event, 'fs41', 59)" onmouseover="showTip(event, 'fs41', 59)" class="rt">FsSnipNews</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs42', 60)" onmouseover="showTip(event, 'fs42', 60)" class="rt">JsonProvider</span><span class="pn">&lt;</span><span onmouseout="hideTip(event, 'fs39', 61)" onmouseover="showTip(event, 'fs39', 61)" class="id">FsSnipNewsSample</span><span class="pn">&gt;</span>

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs43', 62)" onmouseover="showTip(event, 'fs43', 62)" class="id">data</span> <span class="o">=</span> 
  <span onmouseout="hideTip(event, 'fs24', 63)" onmouseover="showTip(event, 'fs24', 63)" class="rt">Http</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs25', 64)" onmouseover="showTip(event, 'fs25', 64)" class="id">Request</span>
    <span class="pn">(</span> <span class="s">&quot;http://api.fssnip.net/1/snippet&quot;</span><span class="pn">,</span> 
      <span class="id">headers</span><span class="o">=</span><span class="pn">[</span><span class="s">&quot;content-type&quot;</span><span class="pn">,</span> <span class="s">&quot;application/json&quot;</span><span class="pn">]</span> <span class="pn">)</span>

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs44', 65)" onmouseover="showTip(event, 'fs44', 65)" class="id">res</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs41', 66)" onmouseover="showTip(event, 'fs41', 66)" class="rt">FsSnipNews</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs45', 67)" onmouseover="showTip(event, 'fs45', 67)" class="id">Parse</span><span class="pn">(</span><span onmouseout="hideTip(event, 'fs43', 68)" onmouseover="showTip(event, 'fs43', 68)" class="id">data</span><span class="pn">)</span>
<span class="k">for</span> <span onmouseout="hideTip(event, 'fs46', 69)" onmouseover="showTip(event, 'fs46', 69)" class="id">snippet</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs44', 70)" onmouseover="showTip(event, 'fs44', 70)" class="id">res</span> <span class="k">do</span>
  <span onmouseout="hideTip(event, 'fs16', 71)" onmouseover="showTip(event, 'fs16', 71)" class="fn">printfn</span> <span class="s">&quot; - </span><span class="pf">%s</span><span class="s">&quot;</span> <span onmouseout="hideTip(event, 'fs46', 72)" onmouseover="showTip(event, 'fs46', 72)" class="id">snippet</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs47', 73)" onmouseover="showTip(event, 'fs47', 73)" class="id">Title</span>
</code></pre></td>
</tr>
</table>
<h2>Summary</h2>
<p>Although I started working on F# Data around christmas, this is the first blog
post about it. The library had some time to develop and we fixed some of the most
important bugs, so if you're interested in data access in F#, <a href="https://github.com/fsharp/FSharp.Data">F# Data</a>
is the right tool for you!</p>
<p>I included a quick overview of some of the type providers that are available in the
library - including those for WorldBank, Freebase, CSV, XML and JSON. Of course, I did
not cover all the features of the library. You can find more information in the
<a href="http://fsharp.github.com/FSharp.Data/">detailed documentation</a>.</p>
<h3>Contribute to F# Data</h3>
<p>As I mentioned already, the library already had some great contributors. Gustavo Guerra did
a great job on making it work in Portable profile and on Silverlight. However, there
is always work to be done :-) and contributors are very welcome. If you're interested,
check out the <a href="https://github.com/fsharp/FSharp.Data/issues">list of issues</a>. I also
wrote a page on <a href="http://fsharp.github.com/FSharp.Data/contributing.html">contributing to F# Data</a>
with basic information about the library structure.</p>
<div class="tip" id="fs1">Multiple items<br />namespace FSharp<br /><br />--------------------<br />namespace Microsoft.FSharp</div>
<div class="tip" id="fs2">Multiple items<br />namespace FSharp.Data<br /><br />--------------------<br />namespace Microsoft.FSharp.Data</div>
<div class="tip" id="fs3">namespace FSharp.Net</div>
<div class="tip" id="fs4">val wb : WorldBankData.ServiceTypes.WorldBankDataService</div>
<div class="tip" id="fs5">type WorldBankData =<br />&#160;&#160;static member GetDataContext : unit -&gt; WorldBankDataService<br />&#160;&#160;nested type ServiceTypes</div>
<div class="tip" id="fs6">WorldBankData.GetDataContext() : WorldBankData.ServiceTypes.WorldBankDataService</div>
<div class="tip" id="fs7">property WorldBankData.ServiceTypes.WorldBankDataService.Countries: WorldBankData.ServiceTypes.Countries</div>
<div class="tip" id="fs8">module Seq<br /><br />from Microsoft.FSharp.Collections</div>
<div class="tip" id="fs9">val maxBy : projection:(&#39;T -&gt; &#39;U) -&gt; source:seq&lt;&#39;T&gt; -&gt; &#39;T (requires comparison)</div>
<div class="tip" id="fs10">val fst : tuple:(&#39;T1 * &#39;T2) -&gt; &#39;T1</div>
<div class="tip" id="fs11">val fb : FreebaseData.ServiceTypes.FreebaseService</div>
<div class="tip" id="fs12">type FreebaseData =<br />&#160;&#160;static member GetDataContext : unit -&gt; FreebaseService<br />&#160;&#160;nested type ServiceTypes<br /><em><br /><br />Contains data and types drawn from the web data store. See www.freebase.com for terms and conditions.</em></div>
<div class="tip" id="fs13">FreebaseData.GetDataContext() : FreebaseData.ServiceTypes.FreebaseService</div>
<div class="tip" id="fs14">val rel : obj</div>
<div class="tip" id="fs15">val take : count:int -&gt; source:seq&lt;&#39;T&gt; -&gt; seq&lt;&#39;T&gt;</div>
<div class="tip" id="fs16">val printfn : format:Printf.TextWriterFormat&lt;&#39;T&gt; -&gt; &#39;T</div>
<div class="tip" id="fs17">Multiple items<br />type LiteralAttribute =<br />&#160;&#160;inherit Attribute<br />&#160;&#160;new : unit -&gt; LiteralAttribute<br /><br />--------------------<br />new : unit -&gt; LiteralAttribute</div>
<div class="tip" id="fs18">val RssSample : string</div>
<div class="tip" id="fs19">&quot;&quot;&quot;&lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;<br />&#160;&#160;&lt;rss version=&quot;2.0&quot;&gt;  <br />&#160;&#160;&#160;&#160;&lt;channel&gt; <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;title&gt;BBC News - Home&lt;/title&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;link&gt;http://www.bbc.co.uk/news/#sa-ns_mchannel=rss&amp;amp;ns_source=PublicRSS20-sa&lt;/link&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;description&gt;The latest stories from the Home section of the BBC News web site.&lt;/description&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;lastBuildDate&gt;Thu, 28 Mar 2013 01:10:12 GMT&lt;/lastBuildDate&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;item&gt; <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;title&gt;Government loses Abu Qatada appeal&lt;/title&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;description&gt;Home Secretary Theresa May loses her appeal against a ruling preventing the deportation of radical cleric Abu Qatada.&lt;/description&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;link&gt;http://www.bbc.co.uk/news/uk-21955844#sa-ns_mchannel=rss&amp;amp;ns_source=PublicRSS20-sa&lt;/link&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;guid isPermaLink=&quot;false&quot;&gt;http://www.bbc.co.uk/news/uk-21955844&lt;/guid&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;pubDate&gt;Wed, 27 Mar 2013 17:37:35 GMT&lt;/pubDate&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;/item&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;item&gt; <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;title&gt;Synchrotron yields &#39;safer&#39; vaccine&lt;/title&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;description&gt;British scientists develop a new way to create an entirely synthetic vaccine which does not rely on using live infectious virus, meaning it is much safer.&lt;/description&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;link&gt;http://www.bbc.co.uk/news/health-21958361#sa-ns_mchannel=rss&amp;amp;ns_source=PublicRSS20-sa&lt;/link&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;guid isPermaLink=&quot;false&quot;&gt;http://www.bbc.co.uk/news/health-21958361&lt;/guid&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;pubDate&gt;Wed, 27 Mar 2013 22:00:04 GMT&lt;/pubDate&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;/item&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;item&gt; <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;title&gt;Oil firms invest $500m in huge field&lt;/title&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;description&gt;Major oil companies announce plans which they hope will boost production from the UK&#39;s biggest oilfield.&lt;/description&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;link&gt;http://www.bbc.co.uk/news/uk-scotland-scotland-business-21955536#sa-ns_mchannel=rss&amp;amp;ns_source=PublicRSS20-sa&lt;/link&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;guid isPermaLink=&quot;false&quot;&gt;http://www.bbc.co.uk/news/uk-scotland-scotland-business-21955536&lt;/guid&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;pubDate&gt;Thu, 28 Mar 2013 00:31:05 GMT&lt;/pubDate&gt;  <br />&#160;&#160;&#160;&#160;&#160;&#160;&lt;/item&gt;  <br />&#160;&#160;&#160;&#160;&lt;/channel&gt; <br />&#160;&#160;&lt;/rss&gt;<br />&#160;&#160;&quot;&quot;&quot;</div>
<div class="tip" id="fs20">type Rss = XmlProvider&lt;...&gt;</div>
<div class="tip" id="fs21">type XmlProvider<br /><em><br /><br />&lt;summary&gt;Typed representation of a XML file&lt;/summary&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Sample&#39;&gt;Location of a XML sample file or a string containing a sample XML document&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Global&#39;&gt;If true, the inference unifies all XML elements with the same name&lt;/param&gt;                     <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Culture&#39;&gt;The culture used for parsing numbers and dates.&lt;/param&gt;                     <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;SampleList&#39;&gt;If true, the children of the root in the sample document represent individual samples for the inference.&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;ResolutionFolder&#39;&gt;A directory that is used when resolving relative file references (at design time and in hosted execution)&lt;/param&gt;</em></div>
<div class="tip" id="fs22">val feed : XmlProvider&lt;...&gt;.DomainTypes.Rss</div>
<div class="tip" id="fs23">XmlProvider&lt;...&gt;.Parse(text: string) : XmlProvider&lt;...&gt;.DomainTypes.Rss</div>
<div class="tip" id="fs24">type Http =<br />&#160;&#160;private new : unit -&gt; Http<br />&#160;&#160;static member AsyncRequest : url:string -&gt; Async&lt;string&gt;<br />&#160;&#160;static member AsyncRequest : url:string * ?query:(string * string) list * ?headers:(string * string) list * ?meth:string * ?body:string -&gt; Async&lt;string&gt;<br />&#160;&#160;static member Request : url:string -&gt; string<br />&#160;&#160;static member Request : url:string * ?query:(string * string) list * ?headers:(string * string) list * ?meth:string * ?body:string -&gt; string</div>
<div class="tip" id="fs25">static member Http.Request : url:string -&gt; string<br />static member Http.Request : url:string * ?query:(string * string) list * ?headers:(string * string) list * ?meth:string * ?body:string -&gt; string</div>
<div class="tip" id="fs26">property XmlProvider&lt;...&gt;.DomainTypes.Rss.Channel: XmlProvider&lt;...&gt;.DomainTypes.Channel</div>
<div class="tip" id="fs27">property XmlProvider&lt;...&gt;.DomainTypes.Channel.Title: string</div>
<div class="tip" id="fs28">val item : XmlProvider&lt;...&gt;.DomainTypes.Item</div>
<div class="tip" id="fs29">XmlProvider&lt;...&gt;.DomainTypes.Channel.GetItems() : XmlProvider&lt;...&gt;.DomainTypes.Item []</div>
<div class="tip" id="fs30">property XmlProvider&lt;...&gt;.DomainTypes.Item.Title: string</div>
<div class="tip" id="fs31">type Stocks = CsvProvider&lt;...&gt;</div>
<div class="tip" id="fs32">type CsvProvider<br /><em><br /><br />&lt;summary&gt;Typed representation of a CSV file&lt;/summary&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Sample&#39;&gt;Location of a CSV sample file or a string containing a sample CSV document&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Separator&#39;&gt;Column delimiter&lt;/param&gt;                     <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Culture&#39;&gt;The culture used for parsing numbers and dates.&lt;/param&gt;                     <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;InferRows&#39;&gt;Number of rows to use for inference. Defaults to 1000. If this is zero, all rows are used.&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;ResolutionFolder&#39;&gt;A directory that is used when resolving relative file references (at design time and in hosted execution)&lt;/param&gt;</em></div>
<div class="tip" id="fs33">val msft : CsvProvider&lt;...&gt;</div>
<div class="tip" id="fs34">CsvProvider&lt;...&gt;.Load(uri: string) : CsvProvider&lt;...&gt;<br />CsvProvider&lt;...&gt;.Load(stream: System.IO.Stream) : CsvProvider&lt;...&gt;</div>
<div class="tip" id="fs35">property CsvProvider&lt;...&gt;.Data: System.Collections.Generic.IEnumerable&lt;CsvProvider&lt;...&gt;.Row&gt;</div>
<div class="tip" id="fs36">val averageBy : projection:(&#39;T -&gt; &#39;U) -&gt; source:seq&lt;&#39;T&gt; -&gt; &#39;U (requires member ( + ) and member DivideByInt and member get_Zero)</div>
<div class="tip" id="fs37">val row : CsvProvider&lt;...&gt;.Row</div>
<div class="tip" id="fs38">property CsvProvider&lt;...&gt;.Row.Open: decimal</div>
<div class="tip" id="fs39">val FsSnipNewsSample : string</div>
<div class="tip" id="fs40">&quot;&quot;&quot;[ { &quot;author&quot;: &quot;Kit Eason&quot;,<br />&#160;&#160;&#160;&#160;&quot;title&quot;: &quot;Eurovision - Some(points)&quot;,<br />&#160;&#160;&#160;&#160;&quot;description&quot;: &quot;The Eurovision final scoring system using records and some higher order functions. (...)&quot;,<br />&#160;&#160;&#160;&#160;&quot;likes&quot;: 1,<br />&#160;&#160;&#160;&#160;&quot;link&quot;: &quot;http://fssnip.net/cg&quot;,<br />&#160;&#160;&#160;&#160;&quot;published&quot;: &quot;5 months ago&quot;},<br />&#160;&#160;{ &quot;author&quot;: &quot;Eirik Tsarpalis&quot;,<br />&#160;&#160;&#160;&#160;&quot;title&quot;: &quot;Codomains through Reflection&quot;,<br />&#160;&#160;&#160;&#160;&quot;description&quot;: &quot;Any type signature has the form of a curried chain T0 -&gt; T1 -&gt; .... -&gt; Tn, where Tn is not a function type. (...)&quot;,<br />&#160;&#160;&#160;&#160;&quot;likes&quot;: 2,<br />&#160;&#160;&#160;&#160;&quot;link&quot;: &quot;http://fssnip.net/cf&quot;,<br />&#160;&#160;&#160;&#160;&quot;published&quot;: &quot;5 months ago&quot; } ]&quot;&quot;&quot;</div>
<div class="tip" id="fs41">type FsSnipNews = JsonProvider&lt;...&gt;</div>
<div class="tip" id="fs42">type JsonProvider<br /><em><br /><br />&lt;summary&gt;Typed representation of a JSON document&lt;/summary&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Sample&#39;&gt;Location of a JSON sample file or a string containing a sample JSON document&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;SampleList&#39;&gt;If true, sample should be a list of individual samples for the inference.&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Culture&#39;&gt;The culture used for parsing numbers and dates.&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;ResolutionFolder&#39;&gt;A directory that is used when resolving relative file references (at design time and in hosted execution)&lt;/param&gt;</em></div>
<div class="tip" id="fs43">val data : string</div>
<div class="tip" id="fs44">val res : JsonProvider&lt;...&gt;.DomainTypes.Entity []</div>
<div class="tip" id="fs45">JsonProvider&lt;...&gt;.Parse(text: string) : JsonProvider&lt;...&gt;.DomainTypes.Entity []</div>
<div class="tip" id="fs46">val snippet : JsonProvider&lt;...&gt;.DomainTypes.Entity</div>
<div class="tip" id="fs47">property JsonProvider&lt;...&gt;.DomainTypes.Entity.Title: string</div>
