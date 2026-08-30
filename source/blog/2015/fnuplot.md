FnuPlot: Cross-platform charting with gnuplot
=============================================

 - date: 2015-01-15T16:58:17.9508797+00:00
 - description: There is a bunch of visualization and charting libraries for F#. Sadly, perhaps the most advanced ones do not work particularly well outside of Windows. This blog post introduces a new work-in-progress library called FnuPlot which is a lightweight and cross-platform wrapper for gnuplot.
 - layout: article
 - image: http://fsprojects.github.io/FnuPlot/img/logo.png
 - tags: f#,fslab,data science
 - title: FnuPlot: Cross-platform charting with gnuplot
 - url: 2015/fnuplot
 - rawbody: true

--------------------------------------------------------------------------------
<img src="http://fsprojects.github.io/FnuPlot/img/logo.png" style="width:120px;float:right;margin:10px" />
<p>There is a bunch of visualization and charting libraries for F#. Sadly, perhaps the most
advanced one, <a href="http://fsharp.github.io/FSharp.Charting/">F# Charting</a>, does not work
particularly well outside of Windows at the moment. There are also some work-in-progress
libraries based on HTML like <a href="http://fsprojects.github.io/Foogle.Charts/">Foogle Charts</a> and
<a href="http://tahahachana.github.io/FsPlot/">FsPlot</a>, which are cross-platform, but not quite
ready yet.</p>
<p>Before Christmas, I got a <a href="https://github.com/fsprojects/FnuPlot/pull/2">notification from GitHub</a>
about a pull request for a simple gnuplot wrapper that I wrote a long time ago (and which used
to be carefully hidden <a href="http://fsxplat.codeplex.com">on CodePlex</a>).</p>
<p>The library is incomplete and I don't expect to dedicate too much time to maintaining it,
but it works quite nicely for basic charts and so I though I'd add the
<a href="http://fsprojects.github.io/ProjectScaffold/">ProjectScaffold</a> structure, do a few tweaks
and make it available as a modern F# project.</p>


--------------------------------------------------------------------------------
<h1><span class="hm">FnuPlot</span><span class="hs"> Cross-platform charting with gnuplot</span></h1>
<img src="http://fsprojects.github.io/FnuPlot/img/logo.png" style="width:120px;float:right;margin:10px" />
<p>There is a bunch of visualization and charting libraries for F#. Sadly, perhaps the most
advanced one, <a href="http://fsharp.github.io/FSharp.Charting/">F# Charting</a>, does not work
particularly well outside of Windows at the moment. There are also some work-in-progress
libraries based on HTML like <a href="http://fsprojects.github.io/Foogle.Charts/">Foogle Charts</a> and
<a href="http://tahahachana.github.io/FsPlot/">FsPlot</a>, which are cross-platform, but not quite
ready yet.</p>
<p>Before Christmas, I got a <a href="https://github.com/fsprojects/FnuPlot/pull/2">notification from GitHub</a>
about a pull request for a simple gnuplot wrapper that I wrote a long time ago (and which used
to be carefully hidden <a href="http://fsxplat.codeplex.com">on CodePlex</a>).</p>
<p>The library is incomplete and I don't expect to dedicate too much time to maintaining it,
but it works quite nicely for basic charts and so I though I'd add the
<a href="http://fsprojects.github.io/ProjectScaffold/">ProjectScaffold</a> structure, do a few tweaks
and make it available as a modern F# project.</p>
<ul>
<li><a href="http://fsprojects.github.io/FnuPlot/">FnuPlot documentation &amp; web site</a></li>
<li>Get <a href="http://www.nuget.org/packages/FnuPlot">FnuPlot from Nuget</a> or use <a href="http://fsprojects.github.io/FnuPlot/tutorial.html#Installing-and-configuring-FnuPlot">Paket GitHub dependency</a></li>
<li><a href="https://github.com/fsprojects/FnuPlot">FnuPlot source code on GitHub</a></li>
</ul>
<h2>Introducing FnuPlot</h2>
<p>FnuPlot is a simple DSL for composing charts. In some ways, it is similar to
<a href="http://fsharp.github.io/FSharp.Charting/">F# Charting</a>, but it has a few specific
aspects that are designed based on how gnuplot works.</p>
<p>Assuming you already have FnuPlot referenced from NuGet, you can start by
creating a new instance of <code>GnuPlot</code> (this is <code>IDisposable</code> and the <code>Dispose</code> method
stops the underlying <code>gnuplot</code> process). The constructor takes a full path to
<code>gnuplot</code> as an argument, in case this is not available in your <code>PATH</code>:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">open</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="i">FnuPlot</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="i">System</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs4', 4)" onmouseover="showTip(event, 'fs4', 4)" class="i">Drawing</span>

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs5', 5)" onmouseover="showTip(event, 'fs5', 5)" class="i">path_win</span> <span class="o">=</span> <span class="s">&quot;C:\Program Files\gnuplot</span><span class="e">\b</span><span class="s">in\Wgnuplot.exe&quot;</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs6', 6)" onmouseover="showTip(event, 'fs6', 6)" class="i">path_nix</span> <span class="o">=</span> <span class="s">&quot;/usr/local/bin/gnuplot&quot;</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs7', 7)" onmouseover="showTip(event, 'fs7', 7)" class="i">gp</span> <span class="o">=</span> <span class="k">new</span> <span onmouseout="hideTip(event, 'fs8', 8)" onmouseover="showTip(event, 'fs8', 8)" class="t">GnuPlot</span>(<span onmouseout="hideTip(event, 'fs1', 9)" onmouseover="showTip(event, 'fs1', 9)" class="i">path</span>)
</code></pre></td>
</tr>
</table>
<p>To create charts, you can now use <code>gp.Plot</code>. This has a couple of overloads. The
most basic one just takes a string with the function you want to plot:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span onmouseout="hideTip(event, 'fs7', 10)" onmouseover="showTip(event, 'fs7', 10)" class="i">gp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs9', 11)" onmouseover="showTip(event, 'fs9', 11)" class="f">Plot</span>(<span class="s">&quot;sin(x)&quot;</span>)
</code></pre></td>
</tr>
</table>
<p><img src="sin.png" alt="" /></p>
<p>If you want to create charts based on data calculated in F#, then you'll need to
use a type called <code>Series</code>. This provides static methods for creating various kinds
of series (lines, histograms, ...). The following creates a line series from X and Y
values and a function series with additional configuration:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
<span class="l">7: </span>
<span class="l">8: </span>
<span class="l">9: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="c">// Line created from X and Y values</span>
<span onmouseout="hideTip(event, 'fs10', 12)" onmouseover="showTip(event, 'fs10', 12)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 13)" onmouseover="showTip(event, 'fs11', 13)" class="f">XY</span> [<span class="k">for</span> <span onmouseout="hideTip(event, 'fs12', 14)" onmouseover="showTip(event, 'fs12', 14)" class="i">x</span> <span class="k">in</span> <span class="n">0.0</span> <span class="o">..</span> <span class="n">0.5</span> <span class="o">..</span> <span class="n">10.0</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs12', 15)" onmouseover="showTip(event, 'fs12', 15)" class="i">x</span>, <span onmouseout="hideTip(event, 'fs13', 16)" onmouseover="showTip(event, 'fs13', 16)" class="f">sin</span> <span onmouseout="hideTip(event, 'fs12', 17)" onmouseover="showTip(event, 'fs12', 17)" class="i">x</span>]
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs7', 18)" onmouseover="showTip(event, 'fs7', 18)" class="i">gp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs9', 19)" onmouseover="showTip(event, 'fs9', 19)" class="f">Plot</span>

<span class="c">// Function with specified title &amp; line color</span>
<span onmouseout="hideTip(event, 'fs10', 20)" onmouseover="showTip(event, 'fs10', 20)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs14', 21)" onmouseover="showTip(event, 'fs14', 21)" class="f">Function</span>
  ( <span class="s">&quot;sin(x)&quot;</span>, <span class="i">title</span><span class="o">=</span><span class="s">&quot;sin&quot;</span>, 
    <span class="i">lineColor</span><span class="o">=</span><span onmouseout="hideTip(event, 'fs15', 22)" onmouseover="showTip(event, 'fs15', 22)" class="t">Color</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs16', 23)" onmouseover="showTip(event, 'fs16', 23)" class="i">BurlyWood</span> )
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs7', 24)" onmouseover="showTip(event, 'fs7', 24)" class="i">gp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs9', 25)" onmouseover="showTip(event, 'fs9', 25)" class="f">Plot</span>
</code></pre></td>
</tr>
</table>
<p>Here, we're using an overload of <code>gp.Plot</code> that takes a single series. You can also
call it with a sequence of series, to combine multiple lines into a single chart.</p>
<p>The following combines the simple function chart with a (not very smooth) line generated
using an F# list comprehension:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
<span class="l">7: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp">[ <span onmouseout="hideTip(event, 'fs10', 26)" onmouseover="showTip(event, 'fs10', 26)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 27)" onmouseover="showTip(event, 'fs11', 27)" class="f">XY</span>
    ( [<span class="k">for</span> <span onmouseout="hideTip(event, 'fs12', 28)" onmouseover="showTip(event, 'fs12', 28)" class="i">x</span> <span class="k">in</span> <span class="n">0.0</span> <span class="o">..</span> <span class="n">0.5</span> <span class="o">..</span> <span class="n">10.0</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs12', 29)" onmouseover="showTip(event, 'fs12', 29)" class="i">x</span>, <span onmouseout="hideTip(event, 'fs13', 30)" onmouseover="showTip(event, 'fs13', 30)" class="f">sin</span> <span onmouseout="hideTip(event, 'fs12', 31)" onmouseover="showTip(event, 'fs12', 31)" class="i">x</span>], 
      <span class="i">title</span><span class="o">=</span><span class="s">&quot;sin&quot;</span>, <span class="i">lineColor</span><span class="o">=</span><span onmouseout="hideTip(event, 'fs15', 32)" onmouseover="showTip(event, 'fs15', 32)" class="t">Color</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs16', 33)" onmouseover="showTip(event, 'fs16', 33)" class="i">BurlyWood</span>, <span class="i">weight</span><span class="o">=</span><span class="n">2</span> )
  <span onmouseout="hideTip(event, 'fs10', 34)" onmouseover="showTip(event, 'fs10', 34)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs14', 35)" onmouseover="showTip(event, 'fs14', 35)" class="f">Function</span>
    ( <span class="s">&quot;cos(x)&quot;</span>, <span class="i">title</span><span class="o">=</span><span class="s">&quot;cos&quot;</span>, 
      <span class="i">lineColor</span><span class="o">=</span><span onmouseout="hideTip(event, 'fs15', 36)" onmouseover="showTip(event, 'fs15', 36)" class="t">Color</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs17', 37)" onmouseover="showTip(event, 'fs17', 37)" class="i">DodgerBlue</span>, <span class="i">weight</span><span class="o">=</span><span class="n">2</span>) ]
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs7', 38)" onmouseover="showTip(event, 'fs7', 38)" class="i">gp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs9', 39)" onmouseover="showTip(event, 'fs9', 39)" class="f">Plot</span>
</code></pre></td>
</tr>
</table>
<p><img src="sincos.png" alt="" /></p>
<p>Here, we're calling <code>gp.Plot</code> with the pipeline operator and we only specified the data.
However, the <code>gp.Plot</code> method has a number of other optional parameters that can be used
to configure how the chart looks. You can, for example, specify the range:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
<span class="l">7: </span>
<span class="l">8: </span>
<span class="l">9: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs18', 40)" onmouseover="showTip(event, 'fs18', 40)" class="i">series</span> <span class="o">=</span> <span class="c">(***[omit:(Same as above)]***)</span>
  [ <span onmouseout="hideTip(event, 'fs10', 41)" onmouseover="showTip(event, 'fs10', 41)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 42)" onmouseover="showTip(event, 'fs11', 42)" class="f">XY</span>
      ( [<span class="k">for</span> <span onmouseout="hideTip(event, 'fs12', 43)" onmouseover="showTip(event, 'fs12', 43)" class="i">x</span> <span class="k">in</span> <span class="n">0.0</span> <span class="o">..</span> <span class="n">0.5</span> <span class="o">..</span> <span class="n">10.0</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs12', 44)" onmouseover="showTip(event, 'fs12', 44)" class="i">x</span>, <span onmouseout="hideTip(event, 'fs13', 45)" onmouseover="showTip(event, 'fs13', 45)" class="f">sin</span> <span onmouseout="hideTip(event, 'fs12', 46)" onmouseover="showTip(event, 'fs12', 46)" class="i">x</span>], 
        <span class="i">title</span><span class="o">=</span><span class="s">&quot;sin&quot;</span>, <span class="i">lineColor</span><span class="o">=</span><span onmouseout="hideTip(event, 'fs15', 47)" onmouseover="showTip(event, 'fs15', 47)" class="t">Color</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs16', 48)" onmouseover="showTip(event, 'fs16', 48)" class="i">BurlyWood</span>, <span class="i">weight</span><span class="o">=</span><span class="n">2</span> )
    <span onmouseout="hideTip(event, 'fs10', 49)" onmouseover="showTip(event, 'fs10', 49)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs14', 50)" onmouseover="showTip(event, 'fs14', 50)" class="f">Function</span>
      ( <span class="s">&quot;cos(x)&quot;</span>, <span class="i">title</span><span class="o">=</span><span class="s">&quot;cos&quot;</span>, 
        <span class="i">lineColor</span><span class="o">=</span><span onmouseout="hideTip(event, 'fs15', 51)" onmouseover="showTip(event, 'fs15', 51)" class="t">Color</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs17', 52)" onmouseover="showTip(event, 'fs17', 52)" class="i">DodgerBlue</span>, <span class="i">weight</span><span class="o">=</span><span class="n">2</span>) ]<span class="c">(***[/omit]***)</span>

<span onmouseout="hideTip(event, 'fs7', 53)" onmouseover="showTip(event, 'fs7', 53)" class="i">gp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs9', 54)" onmouseover="showTip(event, 'fs9', 54)" class="f">Plot</span>( <span onmouseout="hideTip(event, 'fs18', 55)" onmouseover="showTip(event, 'fs18', 55)" class="i">series</span>, <span class="i">range</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs19', 56)" onmouseover="showTip(event, 'fs19', 56)" class="i">Range</span><span class="o">.</span>[<span class="n">0.0</span> <span class="o">..</span> <span class="n">3.14</span>, <span class="o">-</span><span class="n">1.5</span> <span class="o">..</span> <span class="n">1.5</span>] )
</code></pre></td>
</tr>
</table>
<p>The DSL for specifying ranges is using F# range expressions, which is a nice trick (it
does not actually generate a range!) and you can read more about it <a href="http://fsprojects.github.io/FnuPlot/tutorial.html#Configuring-ranges-and-styles">in the
documentation</a>.</p>
<h2>Visualizing WorldBank data</h2>
<p>To look at a larger example, I'm going to use the usual WorldBank type provider
from <a href="http://fsharp.github.io/FSharp.Data/">F# Data</a> and create a chart showing
inequality using the <a href="http://en.wikipedia.org/wiki/Gini_coefficient">GINI index</a> for
the countries of the Eurozone (paying with Euro).</p>
<p>First, let's generate some colours for the lines of the chart. The following snippet
uses a couple of pre-defined colours and than adds a darker version to the palette:</p>
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
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="c">/// Base colors converted from HTML format</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs20', 57)" onmouseover="showTip(event, 'fs20', 57)" class="i">coreColors</span> <span class="o">=</span> 
  [ <span class="s">&quot;#5DA5DA&quot;</span>; <span class="s">&quot;#FAA43A&quot;</span>; <span class="s">&quot;#60BD68&quot;</span>; <span class="s">&quot;#F17CB0&quot;</span>; 
    <span class="s">&quot;#B2912F&quot;</span>; <span class="s">&quot;#B276B2&quot;</span>; <span class="s">&quot;#DECF3F&quot;</span>; <span class="s">&quot;#F15854&quot;</span> ]
  <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs21', 58)" onmouseover="showTip(event, 'fs21', 58)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs22', 59)" onmouseover="showTip(event, 'fs22', 59)" class="f">map</span> <span onmouseout="hideTip(event, 'fs23', 60)" onmouseover="showTip(event, 'fs23', 60)" class="t">ColorTranslator</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs24', 61)" onmouseover="showTip(event, 'fs24', 61)" class="f">FromHtml</span>

<span class="c">/// Infinite sequence with core colors followed by</span>
<span class="c">/// a darker version and then repeated recursively</span>
<span class="k">let</span> <span class="k">rec</span> <span onmouseout="hideTip(event, 'fs25', 62)" onmouseover="showTip(event, 'fs25', 62)" class="i">allColors</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs26', 63)" onmouseover="showTip(event, 'fs26', 63)" class="i">seq</span> {
  <span class="k">yield!</span> <span onmouseout="hideTip(event, 'fs20', 64)" onmouseover="showTip(event, 'fs20', 64)" class="i">coreColors</span>
  <span class="k">for</span> <span onmouseout="hideTip(event, 'fs27', 65)" onmouseover="showTip(event, 'fs27', 65)" class="i">c</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs20', 66)" onmouseover="showTip(event, 'fs20', 66)" class="i">coreColors</span> <span class="k">do</span> 
    <span class="k">yield</span> <span onmouseout="hideTip(event, 'fs15', 67)" onmouseover="showTip(event, 'fs15', 67)" class="t">Color</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs28', 68)" onmouseover="showTip(event, 'fs28', 68)" class="f">FromArgb</span>(<span onmouseout="hideTip(event, 'fs29', 69)" onmouseover="showTip(event, 'fs29', 69)" class="f">int</span> <span onmouseout="hideTip(event, 'fs27', 70)" onmouseover="showTip(event, 'fs27', 70)" class="i">c</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs30', 71)" onmouseover="showTip(event, 'fs30', 71)" class="i">R</span><span class="o">/</span><span class="n">2</span>, <span onmouseout="hideTip(event, 'fs29', 72)" onmouseover="showTip(event, 'fs29', 72)" class="f">int</span> <span onmouseout="hideTip(event, 'fs27', 73)" onmouseover="showTip(event, 'fs27', 73)" class="i">c</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs31', 74)" onmouseover="showTip(event, 'fs31', 74)" class="i">G</span><span class="o">/</span><span class="n">2</span>, <span onmouseout="hideTip(event, 'fs29', 75)" onmouseover="showTip(event, 'fs29', 75)" class="f">int</span> <span onmouseout="hideTip(event, 'fs27', 76)" onmouseover="showTip(event, 'fs27', 76)" class="i">c</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs32', 77)" onmouseover="showTip(event, 'fs32', 77)" class="i">B</span><span class="o">/</span><span class="n">2</span>) 
  <span class="k">yield!</span> <span onmouseout="hideTip(event, 'fs25', 78)" onmouseover="showTip(event, 'fs25', 78)" class="i">allColors</span> }
</code></pre></td>
</tr>
</table>
<p>If you're a gnuplot expert, you can configure the palete directly. The <code>gp</code> object provides
a method <code>gp.SendCommand</code> where you can send arbitrary command to gnuplot. Here, we're
going to specify colours explicitly using the <code>lineColor</code> parameter.</p>
<p>Now, we want to iterate over all countries in a specified region, get the GINI index
values and construct a list of <code>Series.XY</code> charts that can then be passed to <code>gp.Plot</code>.
The whole snippet looks as follows:</p>
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
<span class="l">14: </span>
<span class="l">15: </span>
<span class="l">16: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">open</span> <span onmouseout="hideTip(event, 'fs33', 79)" onmouseover="showTip(event, 'fs33', 79)" class="i">FSharp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs34', 80)" onmouseover="showTip(event, 'fs34', 80)" class="i">Data</span>

<span class="c">// Get EURO area countries from WorldBank</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs35', 81)" onmouseover="showTip(event, 'fs35', 81)" class="i">wb</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs36', 82)" onmouseover="showTip(event, 'fs36', 82)" class="t">WorldBankData</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs37', 83)" onmouseover="showTip(event, 'fs37', 83)" class="f">GetDataContext</span>()
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs38', 84)" onmouseover="showTip(event, 'fs38', 84)" class="i">euro</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs35', 85)" onmouseover="showTip(event, 'fs35', 85)" class="i">wb</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs39', 86)" onmouseover="showTip(event, 'fs39', 86)" class="i">Regions</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs39', 87)" onmouseover="showTip(event, 'fs39', 87)" class="i">``Euro area``</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs39', 88)" onmouseover="showTip(event, 'fs39', 88)" class="i">Countries</span>

<span class="c">// Specify chart range (to get space for legend)</span>
<span onmouseout="hideTip(event, 'fs7', 89)" onmouseover="showTip(event, 'fs7', 89)" class="i">gp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs40', 90)" onmouseover="showTip(event, 'fs40', 90)" class="f">Set</span>(<span class="i">range</span><span class="o">=</span><span onmouseout="hideTip(event, 'fs41', 91)" onmouseover="showTip(event, 'fs41', 91)" class="i">RangeX</span><span class="o">.</span>[<span class="o">..</span> <span class="n">2020.0</span>])

<span class="c">//</span>
[ <span class="k">for</span> <span onmouseout="hideTip(event, 'fs42', 92)" onmouseover="showTip(event, 'fs42', 92)" class="i">color</span>, <span onmouseout="hideTip(event, 'fs43', 93)" onmouseover="showTip(event, 'fs43', 93)" class="i">country</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs21', 94)" onmouseover="showTip(event, 'fs21', 94)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs44', 95)" onmouseover="showTip(event, 'fs44', 95)" class="f">zip</span> <span onmouseout="hideTip(event, 'fs25', 96)" onmouseover="showTip(event, 'fs25', 96)" class="i">allColors</span> <span onmouseout="hideTip(event, 'fs38', 97)" onmouseover="showTip(event, 'fs38', 97)" class="i">euro</span> <span class="k">-&gt;</span>
    <span class="k">let</span> <span onmouseout="hideTip(event, 'fs45', 98)" onmouseover="showTip(event, 'fs45', 98)" class="i">values</span> <span class="o">=</span> 
      <span onmouseout="hideTip(event, 'fs43', 99)" onmouseover="showTip(event, 'fs43', 99)" class="i">country</span><span class="o">.</span><span class="i">Indicators</span><span class="o">.</span><span class="i">``GINI index``</span>
      <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs21', 100)" onmouseover="showTip(event, 'fs21', 100)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs22', 101)" onmouseover="showTip(event, 'fs22', 101)" class="f">map</span> (<span class="k">fun</span> (<span onmouseout="hideTip(event, 'fs46', 102)" onmouseover="showTip(event, 'fs46', 102)" class="i">y</span>, <span onmouseout="hideTip(event, 'fs47', 103)" onmouseover="showTip(event, 'fs47', 103)" class="i">v</span>) <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs48', 104)" onmouseover="showTip(event, 'fs48', 104)" class="f">float</span> <span onmouseout="hideTip(event, 'fs46', 105)" onmouseover="showTip(event, 'fs46', 105)" class="i">y</span>, <span onmouseout="hideTip(event, 'fs47', 106)" onmouseover="showTip(event, 'fs47', 106)" class="i">v</span>)
    <span onmouseout="hideTip(event, 'fs10', 107)" onmouseover="showTip(event, 'fs10', 107)" class="t">Series</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 108)" onmouseover="showTip(event, 'fs11', 108)" class="f">XY</span>(<span onmouseout="hideTip(event, 'fs45', 109)" onmouseover="showTip(event, 'fs45', 109)" class="i">values</span>, <span class="i">title</span><span class="o">=</span><span onmouseout="hideTip(event, 'fs43', 110)" onmouseover="showTip(event, 'fs43', 110)" class="i">country</span><span class="o">.</span><span class="i">Name</span>, <span class="i">weight</span><span class="o">=</span><span class="n">2</span>, <span class="i">lineColor</span><span class="o">=</span><span onmouseout="hideTip(event, 'fs42', 111)" onmouseover="showTip(event, 'fs42', 111)" class="i">color</span>) ]
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs7', 112)" onmouseover="showTip(event, 'fs7', 112)" class="i">gp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs9', 113)" onmouseover="showTip(event, 'fs9', 113)" class="f">Plot</span>
</code></pre></td>
</tr>
</table>
<p><img src="countries.png" alt="" /></p>
<p>One last interesting thing demonstrated by the above snippet is the <code>gp.Set</code> function.
You can use this to configure a number of properties globally, for all subsequent charts.</p>
<h2>Looking for contributors!</h2>
<p>This article shows a couple of things that you can do with the current FnuPlot library.
However, as I mentioned before, the library is really a fairly simple prototype that
I implemented a long time ago. I think using gnuplot is a good way to get nice cross-platform
charts, so I did a bit more work and turned it into a proper F# project under the
fsprojects GitHub organization. But there is a lot that needs to be done if we want to
support all of gnuplot. So if you're interested, <a href="https://github.com/fsprojects/FnuPlot">start discussions &amp; send pull requests
for FnuPlot on GitHub</a>!</p>


<div class="tip" id="fs1">val path : string<br /><br />Full name: Fnuplot.path</div>
<div class="tip" id="fs2">namespace FnuPlot</div>
<div class="tip" id="fs3">namespace System</div>
<div class="tip" id="fs4">namespace System.Drawing</div>
<div class="tip" id="fs5">val path_win : string<br /><br />Full name: Fnuplot.path_win</div>
<div class="tip" id="fs6">val path_nix : string<br /><br />Full name: Fnuplot.path_nix</div>
<div class="tip" id="fs7">val gp : GnuPlot<br /><br />Full name: Fnuplot.gp</div>
<div class="tip" id="fs8">Multiple items<br />type GnuPlot =<br />&#160;&#160;interface IDisposable<br />&#160;&#160;new : ?path:string -&gt; GnuPlot<br />&#160;&#160;private new : actualPath:string -&gt; GnuPlot<br />&#160;&#160;member private Dispose : disposing:bool -&gt; unit<br />&#160;&#160;override Finalize : unit -&gt; unit<br />&#160;&#160;member Plot : data:seq&lt;Series&gt; * ?style:Style * ?range:Range * ?output:Output * ?titles:Titles -&gt; unit<br />&#160;&#160;member Plot : data:Series * ?style:Style * ?range:Range * ?output:Output * ?titles:Titles -&gt; unit<br />&#160;&#160;member Plot : func:string * ?style:Style * ?range:Range * ?output:Output * ?titles:Titles -&gt; unit<br />&#160;&#160;member SendCommand : str:string -&gt; unit<br />&#160;&#160;member Set : ?style:Style * ?range:Range * ?output:Output * ?titles:Titles * ?TimeFormatX:TimeFormatX -&gt; unit<br />&#160;&#160;...<br /><br />Full name: FnuPlot.GnuPlot<br /><br />--------------------<br />new : ?path:string -&gt; GnuPlot</div>
<div class="tip" id="fs9">member GnuPlot.Plot : data:seq&lt;Series&gt; * ?style:Style * ?range:Internal.Range * ?output:Output * ?titles:Titles -&gt; unit<br />member GnuPlot.Plot : data:Series * ?style:Style * ?range:Internal.Range * ?output:Output * ?titles:Titles -&gt; unit<br />member GnuPlot.Plot : func:string * ?style:Style * ?range:Internal.Range * ?output:Output * ?titles:Titles -&gt; unit</div>
<div class="tip" id="fs10">Multiple items<br />type Series =<br />&#160;&#160;new : plot:string * data:Data * ?title:string * ?lineColor:Color * ?weight:int * ?fill:FillStyle -&gt; Series<br />&#160;&#160;member Command : string<br />&#160;&#160;member Data : Data<br />&#160;&#160;static member Function : func:string * ?title:string * ?lineColor:Color * ?weight:int * ?fill:FillStyle -&gt; Series<br />&#160;&#160;static member Histogram : data:seq&lt;float&gt; * ?title:string * ?lineColor:Color * ?weight:int * ?fill:FillStyle -&gt; Series<br />&#160;&#160;static member Lines : data:seq&lt;float&gt; * ?title:string * ?lineColor:Color * ?weight:int -&gt; Series<br />&#160;&#160;static member TimeY : data:seq&lt;DateTime * float&gt; * ?title:string * ?lineColor:Color * ?weight:int -&gt; Series<br />&#160;&#160;static member XY : data:seq&lt;float * float&gt; * ?title:string * ?lineColor:Color * ?weight:int -&gt; Series<br /><br />Full name: FnuPlot.Series<br /><br />--------------------<br />new : plot:string * data:Data * ?title:string * ?lineColor:Color * ?weight:int * ?fill:FillStyle -&gt; Series</div>
<div class="tip" id="fs11">static member Series.XY : data:seq&lt;float * float&gt; * ?title:string * ?lineColor:Color * ?weight:int -&gt; Series</div>
<div class="tip" id="fs12">val x : float</div>
<div class="tip" id="fs13">val sin : value:&#39;T -&gt; &#39;T (requires member Sin)<br /><br />Full name: Microsoft.FSharp.Core.Operators.sin</div>
<div class="tip" id="fs14">static member Series.Function : func:string * ?title:string * ?lineColor:Color * ?weight:int * ?fill:FillStyle -&gt; Series</div>
<div class="tip" id="fs15">type Color =<br />&#160;&#160;struct<br />&#160;&#160;&#160;&#160;member A : byte<br />&#160;&#160;&#160;&#160;member B : byte<br />&#160;&#160;&#160;&#160;member Equals : obj:obj -&gt; bool<br />&#160;&#160;&#160;&#160;member G : byte<br />&#160;&#160;&#160;&#160;member GetBrightness : unit -&gt; float32<br />&#160;&#160;&#160;&#160;member GetHashCode : unit -&gt; int<br />&#160;&#160;&#160;&#160;member GetHue : unit -&gt; float32<br />&#160;&#160;&#160;&#160;member GetSaturation : unit -&gt; float32<br />&#160;&#160;&#160;&#160;member IsEmpty : bool<br />&#160;&#160;&#160;&#160;member IsKnownColor : bool<br />&#160;&#160;&#160;&#160;...<br />&#160;&#160;end<br /><br />Full name: System.Drawing.Color</div>
<div class="tip" id="fs16">property Color.BurlyWood: Color</div>
<div class="tip" id="fs17">property Color.DodgerBlue: Color</div>
<div class="tip" id="fs18">val series : Series list<br /><br />Full name: Fnuplot.series</div>
<div class="tip" id="fs19">val Range : Internal.RangeImplXY<br /><br />Full name: FnuPlot.Ranges.Range</div>
<div class="tip" id="fs20">val coreColors : seq&lt;Color&gt;<br /><br />Full name: Fnuplot.coreColors<br /><em><br /><br />&#160;Base colors converted from HTML format</em></div>
<div class="tip" id="fs21">module Seq<br /><br />from Microsoft.FSharp.Collections</div>
<div class="tip" id="fs22">val map : mapping:(&#39;T -&gt; &#39;U) -&gt; source:seq&lt;&#39;T&gt; -&gt; seq&lt;&#39;U&gt;<br /><br />Full name: Microsoft.FSharp.Collections.Seq.map</div>
<div class="tip" id="fs23">type ColorTranslator =<br />&#160;&#160;static member FromHtml : htmlColor:string -&gt; Color<br />&#160;&#160;static member FromOle : oleColor:int -&gt; Color<br />&#160;&#160;static member FromWin32 : win32Color:int -&gt; Color<br />&#160;&#160;static member ToHtml : c:Color -&gt; string<br />&#160;&#160;static member ToOle : c:Color -&gt; int<br />&#160;&#160;static member ToWin32 : c:Color -&gt; int<br /><br />Full name: System.Drawing.ColorTranslator</div>
<div class="tip" id="fs24">ColorTranslator.FromHtml(htmlColor: string) : Color</div>
<div class="tip" id="fs25">val allColors : seq&lt;Color&gt;<br /><br />Full name: Fnuplot.allColors<br /><em><br /><br />&#160;Infinite sequence with core colors followed by<br />&#160;a darker version and then repeated recursively</em></div>
<div class="tip" id="fs26">Multiple items<br />val seq : sequence:seq&lt;&#39;T&gt; -&gt; seq&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Core.Operators.seq<br /><br />--------------------<br />type seq&lt;&#39;T&gt; = System.Collections.Generic.IEnumerable&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Collections.seq&lt;_&gt;</div>
<div class="tip" id="fs27">val c : Color</div>
<div class="tip" id="fs28">Color.FromArgb(argb: int) : Color<br />Color.FromArgb(alpha: int, baseColor: Color) : Color<br />Color.FromArgb(red: int, green: int, blue: int) : Color<br />Color.FromArgb(alpha: int, red: int, green: int, blue: int) : Color</div>
<div class="tip" id="fs29">Multiple items<br />val int : value:&#39;T -&gt; int (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.int<br /><br />--------------------<br />type int = int32<br /><br />Full name: Microsoft.FSharp.Core.int<br /><br />--------------------<br />type int&lt;&#39;Measure&gt; = int<br /><br />Full name: Microsoft.FSharp.Core.int&lt;_&gt;</div>
<div class="tip" id="fs30">property Color.R: byte</div>
<div class="tip" id="fs31">property Color.G: byte</div>
<div class="tip" id="fs32">property Color.B: byte</div>
<div class="tip" id="fs33">Multiple items<br />namespace FSharp<br /><br />--------------------<br />namespace Microsoft.FSharp</div>
<div class="tip" id="fs34">Multiple items<br />namespace FSharp.Data<br /><br />--------------------<br />namespace Microsoft.FSharp.Data</div>
<div class="tip" id="fs35">val wb : WorldBankData.ServiceTypes.WorldBankDataService<br /><br />Full name: Fnuplot.wb</div>
<div class="tip" id="fs36">type WorldBankData =<br />&#160;&#160;static member GetDataContext : unit -&gt; WorldBankDataService<br />&#160;&#160;nested type ServiceTypes<br /><br />Full name: FSharp.Data.WorldBankData</div>
<div class="tip" id="fs37">WorldBankData.GetDataContext() : WorldBankData.ServiceTypes.WorldBankDataService</div>
<div class="tip" id="fs38">val euro : seq&lt;obj&gt;<br /><br />Full name: Fnuplot.euro</div>
<div class="tip" id="fs39"></div>
<div class="tip" id="fs40">member GnuPlot.Set : ?style:Style * ?range:Internal.Range * ?output:Output * ?titles:Titles * ?TimeFormatX:TimeFormatX -&gt; unit</div>
<div class="tip" id="fs41">val RangeX : Internal.RangeImplX<br /><br />Full name: FnuPlot.Ranges.RangeX</div>
<div class="tip" id="fs42">val color : Color</div>
<div class="tip" id="fs43">val country : obj</div>
<div class="tip" id="fs44">val zip : source1:seq&lt;&#39;T1&gt; -&gt; source2:seq&lt;&#39;T2&gt; -&gt; seq&lt;&#39;T1 * &#39;T2&gt;<br /><br />Full name: Microsoft.FSharp.Collections.Seq.zip</div>
<div class="tip" id="fs45">val values : seq&lt;float * float&gt;</div>
<div class="tip" id="fs46">val y : int</div>
<div class="tip" id="fs47">val v : float</div>
<div class="tip" id="fs48">Multiple items<br />val float : value:&#39;T -&gt; float (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.float<br /><br />--------------------<br />type float = System.Double<br /><br />Full name: Microsoft.FSharp.Core.float<br /><br />--------------------<br />type float&lt;&#39;Measure&gt; = float<br /><br />Full name: Microsoft.FSharp.Core.float&lt;_&gt;</div>
