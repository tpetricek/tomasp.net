Better F# data science with FsLab and Ionide
============================================

 - date: 2016-07-06T16:03:23.8677625+01:00
 - description: The most recent version of Ionide comes with a completely revamped version of F# Interactive which makes it possible to format the results of running F# code as HTML. This blog post provides some of the details about how this works and it also introduces an Ionide integration with FsLab, which gives you new powerful tools for doing data science with F#.
 - layout: article
 - image: http://tomasp.net/blog/2016/fslab-ionide/prices.png
 - tags: f#,fslab,data science
 - title: Better F# data science with FsLab and Ionide
 - url: 2016/fslab-ionide
 - format: bakedin

--------------------------------------------------------------------------------
<p>At <a href="http://ndcoslo.com/">NDC Oslo 2016</a>, I did a talk about some of the recent new F# projects
that are making data science with F# even nicer than it used to be. The talk covered a wider range
of topics, but one of the nice new thing I showed was the improved F# Interactive in the <a href="http://www.ionide.io/">Ionide
plugin for Atom</a> and the integration with FsLab libraries that it provides.</p>
<p>In particular, with the latest version of <a href="http://ionide.io">Ionide</a> for <a href="http://atom.io">Atom</a>
and the latest version of <a href="http://www.fslab.org">FsLab package</a>, you can run code in F# Interactive
and you'll see resulting time series, data frames, matrices, vectors and charts as nicely pretty
printed HTML objects, right in the editor. The following shows some of the features (click on it
for a bigger version):</p>
<a href="http://tomasp.net/blog/2016/fslab-ionide/prices.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/prices.png" style="margin:15px 2% 25px 2%;width:96%" />
</a>
<p>In this post, I'll write about how the new Ionide and FsLab integration works, how you can use
it with your own libraries and also about some of the future plans. You can also learn more by
getting the FsLab package, or watching the NDC talk..</p>


--------------------------------------------------------------------------------
<h1>Better F# data science with FsLab and Ionide</h1>
<p>At <a href="http://ndcoslo.com/">NDC Oslo 2016</a>, I did a talk about some of the recent new F# projects
that are making data science with F# even nicer than it used to be. The talk covered a wider range
of topics, but one of the nice new thing I showed was the improved F# Interactive in the <a href="http://www.ionide.io/">Ionide
plugin for Atom</a> and the integration with FsLab libraries that it provides.</p>
<p>In particular, with the latest version of <a href="http://ionide.io">Ionide</a> for <a href="http://atom.io">Atom</a>
and the latest version of <a href="http://www.fslab.org">FsLab package</a>, you can run code in F# Interactive
and you'll see resulting time series, data frames, matrices, vectors and charts as nicely pretty
printed HTML objects, right in the editor. The following shows some of the features (click on it
for a bigger version):</p>
<a href="http://tomasp.net/blog/2016/fslab-ionide/prices.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/prices.png" style="margin:0px 5% 15px 5%;width:90%" />
</a>
<p>In this post, I'll write about how the new Ionide and FsLab integration works, how you can use
it with your own libraries and also about some of the future plans. You can also learn more by
getting the FsLab package, or watching the NDC talk:</p>
<ul>
<li>
<p><a href="https://vimeo.com/171317247">Analysing Big Time-series Data in the Cloud</a> is my NDC Oslo 2016
talk. It shows the new Ionide + FsLab integration, but also uses <a href="https://github.com/BlueMountainCapital/Deedle.BigDemo">BigDeedle</a>
and <a href="http://www.mbrace.io">MBrace</a> to interactively process large data in the cloud.</p>
</li>
<li>
<p><a href="http://fslab.org/download/">FsLab downloads page</a> has templates that you can download to get
started. Just install <a href="http://atom.io">Atom</a> with <a href="http://ionide.io">Ionide</a>, download the FsLab
basic template and you're good to go!</p>
</li>
<li>
<p>For more background on FsLab as well as additional examples, check out my <a href="http://tomasp.net/blog/2015/announcing-fslab/">FsLab announcement
from last year</a>. This explains what is (and is
not) FsLab, how you can contribute and much more.</p>
</li>
</ul>
<h2>FsLab formatters for Ionide</h2>
<p><a href="http://www.nuget.org/packages/FsLab">FsLab is just a NuGet package</a> that references a number
of other F# packages for doing data science with F#. The one thing that it adds is an easy to
use load script that you can use to load all the packages from F# interactive. This means
that when you download the template, the sample script file starts with something like this:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
<span class="l">7: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="prep">#load</span> <span class="s">&quot;packages/FsLab/Themes/AtomChester.fsx&quot;</span>
<span class="prep">#load</span> <span class="s">&quot;packages/FsLab/FsLab.fsx&quot;</span>

<span class="k">open</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="i">Deedle</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="i">FSharp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="i">Data</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs4', 4)" onmouseover="showTip(event, 'fs4', 4)" class="i">XPlot</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs5', 5)" onmouseover="showTip(event, 'fs5', 5)" class="i">GoogleCharts</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs4', 6)" onmouseover="showTip(event, 'fs4', 6)" class="i">XPlot</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs5', 7)" onmouseover="showTip(event, 'fs5', 7)" class="i">GoogleCharts</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs6', 8)" onmouseover="showTip(event, 'fs6', 8)" class="i">Deedle</span>
</code></pre></td>
</tr>
</table>
<p>The first line loads a default <em>theme</em> that configures how embedded charts and tables
will be formatted. It sets things like float formatting options, colours, fonts etc.
You can find and contribute themes in the <a href="https://github.com/fslaborg/FsLab.Formatters/tree/master/src/Themes">FsLab.Formatters repository</a> -
the current choice covers only one white and one dark theme for Atom. The second line
is the more important one, which loads the FsLab dependencies.</p>
<p>The basic template comes with a minimal example that downloads two time series
from the World Bank and finds the years when they were the most different:</p>
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
<span class="l">12: </span>
<span class="l">13: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs7', 9)" onmouseover="showTip(event, 'fs7', 9)" class="i">wb</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs8', 10)" onmouseover="showTip(event, 'fs8', 10)" class="t">WorldBankData</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs9', 11)" onmouseover="showTip(event, 'fs9', 11)" class="f">GetDataContext</span>()

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs10', 12)" onmouseover="showTip(event, 'fs10', 12)" class="i">cz</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs7', 13)" onmouseover="showTip(event, 'fs7', 13)" class="i">wb</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 14)" onmouseover="showTip(event, 'fs11', 14)" class="i">Countries</span><span class="o">.</span><span class="i">``Czech Republic``</span><span class="o">.</span><span class="i">Indicators</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs12', 15)" onmouseover="showTip(event, 'fs12', 15)" class="i">eu</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs7', 16)" onmouseover="showTip(event, 'fs7', 16)" class="i">wb</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 17)" onmouseover="showTip(event, 'fs11', 17)" class="i">Countries</span><span class="o">.</span><span class="i">``European Union``</span><span class="o">.</span><span class="i">Indicators</span>

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs13', 18)" onmouseover="showTip(event, 'fs13', 18)" class="i">czschool</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs14', 19)" onmouseover="showTip(event, 'fs14', 19)" class="f">series</span> <span onmouseout="hideTip(event, 'fs10', 20)" onmouseover="showTip(event, 'fs10', 20)" class="i">cz</span><span class="o">.</span><span class="i">``Gross enrolment ratio, tertiary, both sexes (%)``</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs15', 21)" onmouseover="showTip(event, 'fs15', 21)" class="i">euschool</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs14', 22)" onmouseover="showTip(event, 'fs14', 22)" class="f">series</span> <span onmouseout="hideTip(event, 'fs12', 23)" onmouseover="showTip(event, 'fs12', 23)" class="i">eu</span><span class="o">.</span><span class="i">``Gross enrolment ratio, tertiary, both sexes (%)``</span>

<span class="c">// Get 5 years with the largest difference between EU and CZ</span>
<span onmouseout="hideTip(event, 'fs16', 24)" onmouseover="showTip(event, 'fs16', 24)" class="f">abs</span> (<span onmouseout="hideTip(event, 'fs13', 25)" onmouseover="showTip(event, 'fs13', 25)" class="i">czschool</span> <span class="o">-</span> <span onmouseout="hideTip(event, 'fs15', 26)" onmouseover="showTip(event, 'fs15', 26)" class="i">euschool</span>)
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs17', 27)" onmouseover="showTip(event, 'fs17', 27)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs18', 28)" onmouseover="showTip(event, 'fs18', 28)" class="f">sort</span>
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs17', 29)" onmouseover="showTip(event, 'fs17', 29)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs19', 30)" onmouseover="showTip(event, 'fs19', 30)" class="f">rev</span>
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs17', 31)" onmouseover="showTip(event, 'fs17', 31)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs20', 32)" onmouseover="showTip(event, 'fs20', 32)" class="f">take</span> <span class="n">5</span>
</code></pre></td>
</tr>
</table>
<p>When you run the code in Atom, a formatter for Deedle series should make it easy
to see the result of the last expression - make sure to run the last 4 lines of
the snippet as a <em>separate interaction</em>. Ionide will only show the formatted object
if the formattable object is the <em>result</em> of the snippet. Alternatively, you can also
select <code>czschool</code> or <code>euschool</code> and run Alt+Enter to see one of the source series:</p>
<a href="http://tomasp.net/blog/2016/fslab-ionide/series.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/series.png" style="margin:0px 5% 15px 5%;width:90%" />
</a>
<p>Aside from Deedle series, the FsLab package registers formatters for the charting libraries
that it comes with. This includes <a href="http://www.fslab.org/FSharp.Charting">F# Charting</a> (Windows-only),
<a href="https://tahahachana.github.io/XPlot/google-charts.html">XPlot Google charts</a> and also
<a href="https://tahahachana.github.io/XPlot/plotly.html">XPlot Plotly charts</a>. The following example
plots the two time-series using the XPlot wrapper for Google charts:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp">[ <span onmouseout="hideTip(event, 'fs13', 33)" onmouseover="showTip(event, 'fs13', 33)" class="i">czschool</span><span class="o">.</span>[<span class="n">1975</span> <span class="o">..</span> <span class="n">2010</span>]; <span onmouseout="hideTip(event, 'fs15', 34)" onmouseover="showTip(event, 'fs15', 34)" class="i">euschool</span><span class="o">.</span>[<span class="n">1975</span> <span class="o">..</span> <span class="n">2010</span>] ]
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs21', 35)" onmouseover="showTip(event, 'fs21', 35)" class="t">Chart</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs22', 36)" onmouseover="showTip(event, 'fs22', 36)" class="f">Line</span>
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs21', 37)" onmouseover="showTip(event, 'fs21', 37)" class="t">Chart</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs23', 38)" onmouseover="showTip(event, 'fs23', 38)" class="f">WithOptions</span> (<span onmouseout="hideTip(event, 'fs24', 39)" onmouseover="showTip(event, 'fs24', 39)" class="t">Options</span>(<span class="i">legend</span><span class="o">=</span><span onmouseout="hideTip(event, 'fs25', 40)" onmouseover="showTip(event, 'fs25', 40)" class="t">Legend</span>(<span class="i">position</span><span class="o">=</span><span class="s">&quot;bottom&quot;</span>)))
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs21', 41)" onmouseover="showTip(event, 'fs21', 41)" class="t">Chart</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs26', 42)" onmouseover="showTip(event, 'fs26', 42)" class="f">WithLabels</span> [<span class="s">&quot;CZ&quot;</span>; <span class="s">&quot;EU&quot;</span>]
</code></pre></td>
</tr>
</table>
<p>The Google chart is formatted according to the theme that we loaded on the first line of the script,
so it looks nicely integrated with the F# Interactive window (but as I mentioned, we need your help
with adding more than just the <a href="https://github.com/fslaborg/FsLab.Formatters/tree/master/src/Themes">two standard Atom themes</a>).</p>
<a href="http://tomasp.net/blog/2016/fslab-ionide/chart.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/chart.png" style="margin:0px 5% 15px 5%;width:90%" />
</a>
<p>One of the nice aspects of how the FsLab and Ionide integration works is that it is not ad-hoc
integration for just a couple of selected libraries - quite the opposite! All the FsLab formatters
live in a <a href="https://github.com/fslaborg/FsLab.Formatters">separate repository</a> from Ionide and
you can create your own formatters that will work in exactly the same way. The following section
has more details about the underlying mechanism behind all this.</p>
<h2>Creating custom HTML formatters</h2>
<img src="http://ionide.io/FsInteractiveService/img/logo.png" class="rdecor" style="width:120px" />
<p>The latest release of <a href="https://atom.io/packages/ionide-fsi">ionide-fsi</a>, which is the F# Interactive
plugin for Atom no longer runs <code>fsi.exe</code> in the background (like Visual Studio or all other editors),
but instead it is based on the brand new <a href="http://ionide.io/FsInteractiveService/">FsInteractiveService</a>.
This is a light-weight server that wraps the F# Interactive functionality. It can be consumed by any
editor via HTTP and it exposes API for <a href="http://ionide.io/FsInteractiveService/http.html">evaluating F# code</a>
but also for <a href="http://ionide.io/FsInteractiveService/intelli.html">getting autocompletion and other hints</a>.</p>
<p>The FsInteractiveService extends the standard F# Interactive functionality with the ability to <a href="http://ionide.io/FsInteractiveService/htmlprinter.html">format
objects as HTML</a>. The idea is very simple. You
call <code>fsi.AddHtmlPrinter</code> and specify a function that turns your object into an HTML string! When you
evaluate an expression that returns a value that has a registered formatter, Ionide will then display
it using your provided HTML formatter.</p>
<h3>Creating HTML formatter for tables</h3>
<p>As a basic example, say you have a type that represents a table:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs27', 43)" onmouseover="showTip(event, 'fs27', 43)" class="t">Table</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs27', 44)" onmouseover="showTip(event, 'fs27', 44)" class="p">Table</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs28', 45)" onmouseover="showTip(event, 'fs28', 45)" class="t">string</span>[,]
</code></pre></td>
</tr>
</table>
<p>Now, we want to create a HTML formatter that will render the table as a <code>&lt;table&gt;</code> element. To do
this, all you need is to call <code>fsi.AddHtmlPrinter</code>. The FsInteractiveService also defines a
symbol <code>HAS_FSI_ADDHTMLPRINTER</code> and so it is a good idea to wrap the following code in a big
<code>#if HAS_FSI_ADDHTMLPRINTER</code> block - this way, the code will be compatible with F# Interactive in
Visual Studio and other editors that do not support HTML formatters (yet).</p>
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
<span class="l">12: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span onmouseout="hideTip(event, 'fs29', 46)" onmouseover="showTip(event, 'fs29', 46)" class="i">fsi</span><span class="o">.</span><span class="i">AddHtmlPrinter</span>(<span class="k">fun</span> (<span class="i">Table</span> <span class="i">t</span>) <span class="k">-&gt;</span>
  <span class="k">let</span> <span class="i">body</span> <span class="o">=</span>
    [ <span class="k">yield</span> <span class="s">&quot;&lt;table&gt;&quot;</span>
      <span class="k">for</span> <span class="i">i</span> <span class="k">in</span> <span class="n">0</span> <span class="o">..</span> <span class="i">t</span><span class="o">.</span><span class="i">GetLength</span>(<span class="n">0</span>)<span class="o">-</span><span class="n">1</span> <span class="k">do</span>
        <span class="k">yield</span> <span class="s">&quot;&lt;tr&gt;&quot;</span>
        <span class="k">for</span> <span class="i">j</span> <span class="k">in</span> <span class="n">0</span> <span class="o">..</span> <span class="i">t</span><span class="o">.</span><span class="i">GetLength</span>(<span class="n">1</span>)<span class="o">-</span><span class="n">1</span> <span class="k">do</span>
          <span class="k">yield</span> <span class="s">&quot;&lt;td&gt;&quot;</span> <span class="o">+</span> <span class="i">t</span><span class="o">.</span>[<span class="i">i</span>,<span class="i">j</span>] <span class="o">+</span> <span class="s">&quot;&lt;/td&gt;&quot;</span>
        <span class="k">yield</span> <span class="s">&quot;&lt;/tr&gt;&quot;</span>
      <span class="k">yield</span> <span class="s">&quot;&lt;/table&gt;&quot;</span> ]
    <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs30', 47)" onmouseover="showTip(event, 'fs30', 47)" class="i">String</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs31', 48)" onmouseover="showTip(event, 'fs31', 48)" class="i">concat</span> <span class="s">&quot;&quot;</span>
  <span onmouseout="hideTip(event, 'fs32', 49)" onmouseover="showTip(event, 'fs32', 49)" class="i">seq</span> [ <span class="s">&quot;style&quot;</span>, <span class="s">&quot;&lt;style&gt;table { background:#f0f0f0; }&lt;/style&gt;&quot;</span> ],
  <span class="i">body</span> )
</code></pre></td>
</tr>
</table>
<p>The result of the formatter function is actually <code>seq&lt;string * string&gt; * string</code>. The tuple consists
of two things:</p>
<ul>
<li>
<p>The second element is the HTML body that represents the formatted value.
Typically, editors will embed this into HTML output.</p>
</li>
<li>
<p>A sequence of key value pairs that represents additional styles and sripts used that are
required by the body. The keys can be <code>style</code> or <code>script</code> (or other custom keys supported by
editors) and can be treated in a special way by the editors (e.g. loading JavaScript
dynamically in Atom requires placing the HTML content in an <code>&lt;iframe&gt;</code>`).</p>
</li>
</ul>
<p>You can now define a table as follows:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs33', 50)" onmouseover="showTip(event, 'fs33', 50)" class="i">table</span> <span class="o">=</span>
  [ [ <span class="s">&quot;Test&quot;</span>; <span class="s">&quot;More&quot;</span>]
    [ <span class="s">&quot;1234&quot;</span>; <span class="s">&quot;5678&quot;</span>] ]
  <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs34', 51)" onmouseover="showTip(event, 'fs34', 51)" class="f">array2D</span> <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs27', 52)" onmouseover="showTip(event, 'fs27', 52)" class="p">Table</span>
</code></pre></td>
</tr>
</table>
<p>In the current version, the value is only formatted when <code>Table</code> is returned as a direct result
of an expression. This means that you need to evaluate an expression of type <code>Table</code> rather than,
for example, a value binding as above:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span onmouseout="hideTip(event, 'fs33', 53)" onmouseover="showTip(event, 'fs33', 53)" class="i">table</span>
</code></pre></td>
</tr>
</table>
<p>When you run the above in Atom, you will see a table formatted as HTML <code>&lt;table&gt;</code> element. (Some
more styling is needed to actually make this pretty, but this is a good start. Oh and did you
know that Atom supports the <code>&lt;marquee&gt;</code> tag?!)</p>
<h3>Themes, parameters and servers</h3>
<p>In practice, there are a few other concerns that make formatting objects as HTML harder. For
example, some of the HTML formatters can implement lazy loading where they use a simple web server
running in the background to provide data to the view (which calls the server using JavaScript).
Also, it is nice if all the HTML formatters can share the same visual theme. To make these
possible, the FsInteractiveService also defines <code>fsi.HtmlPrinterParameters</code> which is a global
value of type <code>IDictionary&lt;string, obj&gt;</code> that can be used for storing various shared configuration.</p>
<p>For example, the <code>html-standalone-output</code> parameter specifies whether the generated HTML code
should be stand-alone, or whether it is allowed to use JavaScript to load data lazily (the latter
is used for Deedle frames in the talk and it means you can scroll through the data, but you need
to hava a server running in the background):</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="prep">#if</span> <span class="i">HAS_FSI_ADDHTMLPRINTER</span>
<span class="inactive">let</span><span class="inactive"> </span><span class="inactive">standaloneHtmlOutput</span><span class="inactive"> </span><span class="inactive">=</span>
<span class="inactive">  </span><span class="inactive">fsi.HtmlPrinterParameters.[&quot;html-standalone-output&quot;]</span><span class="inactive"> </span><span class="inactive">:?&gt;</span><span class="inactive"> </span><span class="inactive">bool</span>
<span class="prep">#endif</span>
</code></pre></td>
</tr>
</table>
<p>There are a couple of examples of how this dictionary can be used in the standard FsLab formatters:</p>
<ul>
<li>
<p><a href="https://github.com/fslaborg/FsLab.Formatters/blob/8dc5a1c372afa05025b5244c83494be83bfab3d9/src/Themes/DefaultWhite.fs">The <code>DefaultWhite.fsx</code> file</a>
shows the different kind of parameters that you can specify for default FsLab formatters. You can
copy &amp; edit it to create new visual styles for FsLab (and send a PR to <a href="https://github.com/fslaborg/FsLab.Formatters">FsLab.Formatters</a>
if they correspond to a common Atom theme!)</p>
</li>
<li>
<p><a href="https://github.com/fslaborg/FsLab.Formatters/blob/8dc5a1c372afa05025b5244c83494be83bfab3d9/src/Html/XPlot.fs#L79">The XPlot formatter in <code>XPlot.fs</code></a>
is a good example of a formatter that reads the above visual styles and uses it to customize the look
of the HTML it generates.</p>
</li>
<li>
<p><a href="https://github.com/fslaborg/FsLab.Formatters/blob/8dc5a1c372afa05025b5244c83494be83bfab3d9/src/Html/Deedle.fs#L298">The Deedle formatter in <code>Deedle.fs</code></a>
uses a lightweight Suave server running in the background to load data from a frame or series on demand.
This is a good example of a more sophisticated formatter.</p>
</li>
</ul>
<h2>FsLab Journal and looking ahead</h2>
<h3>Formatting in FsLab journals</h3>
<p>The <a href="http://fslab.org/download/">FsLab downloads page</a> also lets you download a <a href="https://github.com/fslaborg/FsLab.Templates/archive/journal.zip">FsLab Journal
template</a>. This is something that
has been available in FsLab for longer time, but I never wrote much about it. The summary is:</p>
<blockquote>
<p>FsLab Journal lets you turn your F# scripts consisting of F# code snippets and Markdown
formatted comments into a nice HTML report.</p>
</blockquote>
<p>When you download the template, you can just run <code>build run</code> and your script will be turned into
a HTML report in the background. When you change your script, the background runner will upadate
and reload your report. If you want to produce stand-alone HTML (that does not require background
server), you can run <code>build html</code>. The following is an opened journal, running on my machine.</p>
<a href="http://tomasp.net/blog/2016/fslab-ionide/journal.png">
<img src="http://tomasp.net/blog/2016/fslab-ionide/journal.png" style="margin:0px 5% 15px 5%;width:90%" />
</a>
<p>In the latest version of FsLab, the formatting for journals is based on
the same <code>fsi.AddHtmlPrinter</code> formatters. This means we get to reuse the code for it, but most
importantly, when <em>your</em> write your own formatter, it will work with both Ionide and also with
FsLab journals.</p>
<h3>Formatting in Jupyter notebooks</h3>
<p>One of the related projects in the F# and data science space is the <a href="https://github.com/fsprojects/IfSharp">F# bindings for Jupyter
Notebooks</a>. This does not yet use the same model for
registering HTML formatters via <code>fsi.AddHtmlPrinter</code>. Instead, it has its own mechanism for
<a href="https://github.com/fsprojects/IfSharp/blob/jupyter/src/IfSharp.Kernel/Printers.fs">registering printers</a>,
but I expect that it will be possible to merge the two so that you can just write <code>fsi.AddHtmlPrinter</code>
once and use it in Ionide, FsLab Journals as well as Jupyter.</p>


<div class="tip" id="fs1">namespace Deedle</div>
<div class="tip" id="fs2">Multiple items<br />namespace FSharp<br /><br />--------------------<br />namespace Microsoft.FSharp</div>
<div class="tip" id="fs3">Multiple items<br />namespace FSharp.Data<br /><br />--------------------<br />namespace Microsoft.FSharp.Data</div>
<div class="tip" id="fs4">namespace XPlot</div>
<div class="tip" id="fs5">namespace XPlot.GoogleCharts</div>
<div class="tip" id="fs6">module Deedle<br /><br />from XPlot.GoogleCharts</div>
<div class="tip" id="fs7">val wb : WorldBankData.ServiceTypes.WorldBankDataService<br /><br />Full name: Fslab-ionide.wb</div>
<div class="tip" id="fs8">type WorldBankData =<br />&#160;&#160;static member GetDataContext : unit -&gt; WorldBankDataService<br />&#160;&#160;nested type ServiceTypes<br /><br />Full name: FSharp.Data.WorldBankData<br /><em><br /><br />&lt;summary&gt;Typed representation of WorldBank data. See http://www.worldbank.org for terms and conditions.&lt;/summary&gt;</em></div>
<div class="tip" id="fs9">WorldBankData.GetDataContext() : WorldBankData.ServiceTypes.WorldBankDataService</div>
<div class="tip" id="fs10">val cz : WorldBankData.ServiceTypes.Indicators<br /><br />Full name: Fslab-ionide.cz</div>
<div class="tip" id="fs11">property WorldBankData.ServiceTypes.WorldBankDataService.Countries: WorldBankData.ServiceTypes.Countries</div>
<div class="tip" id="fs12">val eu : WorldBankData.ServiceTypes.Indicators<br /><br />Full name: Fslab-ionide.eu</div>
<div class="tip" id="fs13">val czschool : Series&lt;int,float&gt;<br /><br />Full name: Fslab-ionide.czschool</div>
<div class="tip" id="fs14">val series : observations:seq&lt;&#39;a * &#39;b&gt; -&gt; Series&lt;&#39;a,&#39;b&gt; (requires equality)<br /><br />Full name: Deedle.F# Series extensions.series</div>
<div class="tip" id="fs15">val euschool : Series&lt;int,float&gt;<br /><br />Full name: Fslab-ionide.euschool</div>
<div class="tip" id="fs16">val abs : value:&#39;T -&gt; &#39;T (requires member Abs)<br /><br />Full name: Microsoft.FSharp.Core.Operators.abs</div>
<div class="tip" id="fs17">Multiple items<br />module Series<br /><br />from Deedle<br /><br />--------------------<br />type Series =<br />&#160;&#160;new : ?type:string -&gt; Series<br />&#160;&#160;member ShouldSerializeannotations : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeareaOpacity : unit -&gt; bool<br />&#160;&#160;member ShouldSerializecolor : unit -&gt; bool<br />&#160;&#160;member ShouldSerializecurveType : unit -&gt; bool<br />&#160;&#160;member ShouldSerializefallingColor : unit -&gt; bool<br />&#160;&#160;member ShouldSerializelineWidth : unit -&gt; bool<br />&#160;&#160;member ShouldSerializepointShape : unit -&gt; bool<br />&#160;&#160;member ShouldSerializepointSize : unit -&gt; bool<br />&#160;&#160;member ShouldSerializerisingColor : unit -&gt; bool<br />&#160;&#160;...<br /><br />Full name: XPlot.GoogleCharts.Configuration.Series<br /><br />--------------------<br />type Series&lt;&#39;K,&#39;V (requires equality)&gt; =<br />&#160;&#160;interface IFsiFormattable<br />&#160;&#160;interface ISeries&lt;&#39;K&gt;<br />&#160;&#160;new : pairs:seq&lt;KeyValuePair&lt;&#39;K,&#39;V&gt;&gt; -&gt; Series&lt;&#39;K,&#39;V&gt;<br />&#160;&#160;new : keys:&#39;K [] * values:&#39;V [] -&gt; Series&lt;&#39;K,&#39;V&gt;<br />&#160;&#160;new : keys:seq&lt;&#39;K&gt; * values:seq&lt;&#39;V&gt; -&gt; Series&lt;&#39;K,&#39;V&gt;<br />&#160;&#160;new : index:IIndex&lt;&#39;K&gt; * vector:IVector&lt;&#39;V&gt; * vectorBuilder:IVectorBuilder * indexBuilder:IIndexBuilder -&gt; Series&lt;&#39;K,&#39;V&gt;<br />&#160;&#160;member After : lowerExclusive:&#39;K -&gt; Series&lt;&#39;K,&#39;V&gt;<br />&#160;&#160;member Aggregate : aggregation:Aggregation&lt;&#39;K&gt; * observationSelector:Func&lt;DataSegment&lt;Series&lt;&#39;K,&#39;V&gt;&gt;,KeyValuePair&lt;&#39;TNewKey,OptionalValue&lt;&#39;R&gt;&gt;&gt; -&gt; Series&lt;&#39;TNewKey,&#39;R&gt; (requires equality)<br />&#160;&#160;member Aggregate : aggregation:Aggregation&lt;&#39;K&gt; * keySelector:Func&lt;DataSegment&lt;Series&lt;&#39;K,&#39;V&gt;&gt;,&#39;TNewKey&gt; * valueSelector:Func&lt;DataSegment&lt;Series&lt;&#39;K,&#39;V&gt;&gt;,OptionalValue&lt;&#39;R&gt;&gt; -&gt; Series&lt;&#39;TNewKey,&#39;R&gt; (requires equality)<br />&#160;&#160;member AsyncMaterialize : unit -&gt; Async&lt;Series&lt;&#39;K,&#39;V&gt;&gt;<br />&#160;&#160;...<br /><br />Full name: Deedle.Series&lt;_,_&gt;<br /><br />--------------------<br />new : ?type:string -&gt; Series<br /><br />--------------------<br />new : pairs:seq&lt;System.Collections.Generic.KeyValuePair&lt;&#39;K,&#39;V&gt;&gt; -&gt; Series&lt;&#39;K,&#39;V&gt;<br />new : keys:seq&lt;&#39;K&gt; * values:seq&lt;&#39;V&gt; -&gt; Series&lt;&#39;K,&#39;V&gt;<br />new : keys:&#39;K [] * values:&#39;V [] -&gt; Series&lt;&#39;K,&#39;V&gt;<br />new : index:Indices.IIndex&lt;&#39;K&gt; * vector:IVector&lt;&#39;V&gt; * vectorBuilder:Vectors.IVectorBuilder * indexBuilder:Indices.IIndexBuilder -&gt; Series&lt;&#39;K,&#39;V&gt;</div>
<div class="tip" id="fs18">val sort : series:Series&lt;&#39;K,&#39;V&gt; -&gt; Series&lt;&#39;K,&#39;V&gt; (requires equality and comparison)<br /><br />Full name: Deedle.Series.sort</div>
<div class="tip" id="fs19">val rev : series:Series&lt;&#39;K,&#39;T&gt; -&gt; Series&lt;&#39;K,&#39;T&gt; (requires equality)<br /><br />Full name: Deedle.Series.rev</div>
<div class="tip" id="fs20">val take : count:int -&gt; series:Series&lt;&#39;K,&#39;T&gt; -&gt; Series&lt;&#39;K,&#39;T&gt; (requires equality)<br /><br />Full name: Deedle.Series.take</div>
<div class="tip" id="fs21">type Chart =<br />&#160;&#160;static member Annotation : data:seq&lt;#seq&lt;DateTime * &#39;V * string * string&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires &#39;V :&gt; value)<br />&#160;&#160;static member Annotation : data:seq&lt;DateTime * #value * string * string&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Area : data:seq&lt;#seq&lt;&#39;K * &#39;V&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires &#39;K :&gt; key and &#39;V :&gt; value)<br />&#160;&#160;static member Area : data:seq&lt;#key * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Bar : data:seq&lt;#seq&lt;&#39;K * &#39;V&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires &#39;K :&gt; key and &#39;V :&gt; value)<br />&#160;&#160;static member Bar : data:seq&lt;#key * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Bubble : data:seq&lt;string * #value * #value * #value * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Bubble : data:seq&lt;string * #value * #value * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Bubble : data:seq&lt;string * #value * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;static member Calendar : data:seq&lt;DateTime * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart<br />&#160;&#160;...<br /><br />Full name: XPlot.GoogleCharts.Chart</div>
<div class="tip" id="fs22">static member Chart.Line : data:Frame&lt;&#39;K,&#39;V&gt; * ?Options:Options -&gt; GoogleChart (requires equality and equality)<br />static member Chart.Line : data:Series&lt;&#39;K,#value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires equality and &#39;K :&gt; key)<br />static member Chart.Line : data:seq&lt;Series&lt;&#39;K,#value&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires equality and &#39;K :&gt; key)<br />static member Chart.Line : data:seq&lt;#seq&lt;&#39;K * &#39;V&gt;&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart (requires &#39;K :&gt; key and &#39;V :&gt; value)<br />static member Chart.Line : data:seq&lt;#key * #value&gt; * ?Labels:seq&lt;string&gt; * ?Options:Options -&gt; GoogleChart</div>
<div class="tip" id="fs23">static member Chart.WithOptions : options:Options -&gt; chart:GoogleChart -&gt; GoogleChart</div>
<div class="tip" id="fs24">Multiple items<br />type Options =<br />&#160;&#160;new : unit -&gt; Options<br />&#160;&#160;member ShouldSerializeaggregationTarget : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeallValuesSuffix : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeallowHtml : unit -&gt; bool<br />&#160;&#160;member ShouldSerializealternatingRowStyle : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeanimation : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeannotations : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeannotationsWidth : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeareaOpacity : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeavoidOverlappingGridLines : unit -&gt; bool<br />&#160;&#160;...<br /><br />Full name: XPlot.GoogleCharts.Configuration.Options<br /><br />--------------------<br />new : unit -&gt; Options</div>
<div class="tip" id="fs25">Multiple items<br />type Legend =<br />&#160;&#160;new : unit -&gt; Legend<br />&#160;&#160;member ShouldSerializealignment : unit -&gt; bool<br />&#160;&#160;member ShouldSerializemaxLines : unit -&gt; bool<br />&#160;&#160;member ShouldSerializenumberFormat : unit -&gt; bool<br />&#160;&#160;member ShouldSerializeposition : unit -&gt; bool<br />&#160;&#160;member ShouldSerializetextStyle : unit -&gt; bool<br />&#160;&#160;member alignment : string<br />&#160;&#160;member maxLines : int<br />&#160;&#160;member numberFormat : string<br />&#160;&#160;member position : string<br />&#160;&#160;...<br /><br />Full name: XPlot.GoogleCharts.Configuration.Legend<br /><br />--------------------<br />new : unit -&gt; Legend</div>
<div class="tip" id="fs26">static member Chart.WithLabels : labels:seq&lt;string&gt; -&gt; chart:GoogleChart -&gt; GoogleChart</div>
<div class="tip" id="fs27">Multiple items<br />union case Table.Table: string [,] -&gt; Table<br /><br />--------------------<br />type Table = | Table of string [,]<br /><br />Full name: Fslab-ionide.Table</div>
<div class="tip" id="fs28">Multiple items<br />val string : value:&#39;T -&gt; string<br /><br />Full name: Microsoft.FSharp.Core.Operators.string<br /><br />--------------------<br />type string = System.String<br /><br />Full name: Microsoft.FSharp.Core.string</div>
<div class="tip" id="fs29">val fsi : Compiler.Interactive.InteractiveSession<br /><br />Full name: Microsoft.FSharp.Compiler.Interactive.Settings.fsi</div>
<div class="tip" id="fs30">module String<br /><br />from Microsoft.FSharp.Core</div>
<div class="tip" id="fs31">val concat : sep:string -&gt; strings:seq&lt;string&gt; -&gt; string<br /><br />Full name: Microsoft.FSharp.Core.String.concat</div>
<div class="tip" id="fs32">Multiple items<br />val seq : sequence:seq&lt;&#39;T&gt; -&gt; seq&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Core.Operators.seq<br /><br />--------------------<br />type seq&lt;&#39;T&gt; = System.Collections.Generic.IEnumerable&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Collections.seq&lt;_&gt;</div>
<div class="tip" id="fs33">val table : Table<br /><br />Full name: Fslab-ionide.table</div>
<div class="tip" id="fs34">val array2D : rows:seq&lt;#seq&lt;&#39;T&gt;&gt; -&gt; &#39;T [,]<br /><br />Full name: Microsoft.FSharp.Core.ExtraTopLevelOperators.array2D</div>
