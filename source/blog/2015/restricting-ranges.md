Comparing date range handling in C# and F#
==========================================

 - date: 2015-04-22T16:55:20.5498839+01:00
 - description: I was recently working on some code for handling data ranges and I wrote initial version in C#. Later, I realized that I needed an F# version, so I went through the process of rewriting a simple function from C# to F#. This blog post compares the two versions.
 - layout: article
 - image: http://tomasp.net/blog/2015/restricting-ranges/card.png
 - tags: f#,c#,deedle,linq,functional programming
 - title: Comparing date range handling in C# and F#
 - url: 2015/restricting-ranges
 - rawbody: true

--------------------------------------------------------------------------------
<p>I was recently working on some code for handling date ranges in
<a href="http://github.com/blueMountainCapital/Deedle">Deedle</a>. Although Deedle is written in F#,
I also wrote some internal integration code in C#. After doing that, I realized that the
code I wrote is actually reusable and should be a part of Deedle itself and so I went through
the process of rewriting a simple function from (fairly functional) C# to F#. This is a small
(and by no means representative!) example, but I think it nicely shows some of the reasons why
I like F#, so I thought I'd share it.</p>
<p>One thing that we are adding to Deedle is a "BigDeedle" implementation of internal data
structures. The idea is that you can load very big frames and series without actually loading
all data into memory.</p>
<p>When you perform slicing on a large series and then merge some of the parts of the series
(say, years 2010, 2012 and 2014), you end up with a series that combines a
couple of chunks. If you then restrict the series (say, from June 2012 to June 2014), you
need to restrict the ranges of the chunks:</p>
<img src="http://tomasp.net/blog/2015/restricting-ranges/drawing.png" alt="Demonstration" style="margin:15px; width:370px" />
<p>As the diagram shows, this is just a matter of iterating over the chunks, keeping those
in the range, dropping those outside of the range and restrictingthe boundaries of the other
chunks. So, let's start with the C# version I wrote.</p>


--------------------------------------------------------------------------------
<h1>Comparing date range handling in C# and F#</h1>
<p>I was recently working on some code for handling date ranges in
<a href="http://github.com/blueMountainCapital/Deedle">Deedle</a>. Although Deedle is written in F#,
I also wrote some internal integration code in C#. After doing that, I realized that the
code I wrote is actually reusable and should be a part of Deedle itself and so I went through
the process of rewriting a simple function from (fairly functional) C# to F#. This is a small
(and by no means representative!) example, but I think it nicely shows some of the reasons why
I like F#, so I thought I'd share it.</p>
<h2>The problem</h2>
<p>One thing that we are adding to Deedle is a "BigDeedle" implementation of internal data
structures. The idea is that you can load very big frames and series without actually loading
all data into memory.</p>
<p>When you perform slicing on a large series and then merge some of the parts of the series
(say, years 2010, 2012 and 2014), you end up with a series that combines a
couple of chunks. If you then restrict the series (say, from June 2012 to June 2014), you
need to restrict the ranges of the chunks:</p>
<img src="drawing.png" alt="Demonstration" style="margin:15px 0px 15px 25px" />
<p>As the diagram shows, this is just a matter of iterating over the chunks, keeping those
in the range, dropping those outside of the range and restrictingthe boundaries of the other
chunks. So, let's start with the C# version I wrote.</p>
<h2>Restricting ranges in C#</h2>
<p>To keep the sample self-contained, I'll also include a simple definition of a <code>Date</code> type
that I was using in my experiments. This is not the key part, but it is worth showing. A
date is essentially an integer - a number of days since the beginning of the universe (or
some other important milestone):</p>
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
<td class="snippet"><pre class="fssnip highlighted"><code lang="csharp"><span class="c">/// &lt;summary&gt;</span>
<span class="c">/// A date as a number of days since the beginning</span>
<span class="c">/// &lt;/summary&gt;</span>
<span class="k">class</span> Date {
  <span class="k">public</span> <span class="k">int</span> Offset { get; set; }
  <span class="k">public</span> <span class="k">static</span> <span class="k">bool</span> <span class="k">operator</span> <span class="o">&lt;</span><span class="o">=</span>(Date d<span class="n">1</span>, Date d<span class="n">2</span>) { <span class="k">return</span> d<span class="n">1</span>.Offset <span class="o">&lt;</span><span class="o">=</span> d<span class="n">2</span>.Offset; }
  <span class="k">public</span> <span class="k">static</span> <span class="k">bool</span> <span class="k">operator</span> <span class="o">&gt;</span><span class="o">=</span>(Date d<span class="n">1</span>, Date d<span class="n">2</span>) { <span class="k">return</span> d<span class="n">1</span>.Offset <span class="o">&gt;</span><span class="o">=</span> d<span class="n">2</span>.Offset; }
  <span class="k">public</span> <span class="k">static</span> <span class="k">bool</span> <span class="k">operator</span> <span class="o">&lt;</span>(Date d<span class="n">1</span>, Date d<span class="n">2</span>) { <span class="k">return</span> d<span class="n">1</span>.Offset <span class="o">&lt;</span> d<span class="n">2</span>.Offset; }
  <span class="k">public</span> <span class="k">static</span> <span class="k">bool</span> <span class="k">operator</span> <span class="o">&gt;</span>(Date d<span class="n">1</span>, Date d<span class="n">2</span>) { <span class="k">return</span> d<span class="n">1</span>.Offset <span class="o">&gt;</span> d<span class="n">2</span>.Offset; }
}
<span class="c">/// &lt;summary&gt;</span>
<span class="c">/// An array of ranges represented as date pairs</span>
<span class="c">/// &lt;/summary&gt;</span>
<span class="k">class</span> Ranges {
  <span class="k">public</span> Tuple&lt;Date, Date&gt;[] Ranges { get; set; }
}
</code></pre></td></tr></table>
<p>The <code>Date</code> type defines a couple of custom operators so that we can compare dates (I only defined
those that I needed). The <code>Ranges</code> type is the simplest possible wrapper over an array of <code>Date</code>
pairs. The type is internal, so I was just using tuples to save some typing.</p>
<p>Next, let's have a look at the <code>RestrictRanges</code> function. This takes <code>Ranges</code> together with
lower and upper bound of the restriction. It then iterates over the ranges using <code>SelectMany</code>
and returns the range unmodified (if it is within the restriction), skips it (if it is outside)
or adjusts its lower and upper bounds:</p>
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
<span class="l">17: </span>
<span class="l">18: </span>
<span class="l">19: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="csharp"><span class="c">/// &lt;summary&gt;</span>
<span class="c">/// Restrict the specified collection of ranges </span>
<span class="c">/// according to the provided restriction range</span>
<span class="c">/// &lt;/summary&gt;</span>
<span class="k">static</span> Ranges RestrictRanges(Ranges ranges, Date loRestr, Date hiRestr)
{
  <span class="k">var</span> newRanges <span class="o">=</span> ranges.Ranges.SelectMany(range <span class="o">=</span><span class="o">&gt;</span>
  {
    <span class="k">if</span> (range.Item<span class="n">1</span> <span class="o">&gt;</span><span class="o">=</span> loRestr <span class="o">&amp;</span><span class="o">&amp;</span> range.Item<span class="n">2</span> <span class="o">&lt;</span><span class="o">=</span> hiRestr)
      <span class="k">return</span> <span class="k">new</span>[] { range };
    <span class="k">else</span> <span class="k">if</span> (range.Item<span class="n">2</span> <span class="o">&lt;</span> loRestr <span class="o">|</span><span class="o">|</span> range.Item<span class="n">1</span> <span class="o">&gt;</span> hiRestr)
      <span class="k">return</span> <span class="k">new</span> Tuple&lt;Date, Date&gt;[<span class="n">0</span>];
    <span class="k">else</span>
      <span class="k">return</span> <span class="k">new</span>[] { Tuple.Create
          ( range.Item<span class="n">1</span> <span class="o">&gt;</span> loRestr <span class="o">?</span> range.Item<span class="n">1</span> <span class="o">:</span> loRestr, 
            range.Item<span class="n">2</span> <span class="o">&lt;</span> hiRestr <span class="o">?</span> range.Item<span class="n">2</span> <span class="o">:</span> hiRestr ) };
  }).ToArray();
  <span class="k">return</span> <span class="k">new</span> Ranges { Ranges <span class="o">=</span> newRanges };
}
</code></pre></td></tr></table>
<p>This is fairly simple and readable piece of code. We might be able to make it a bit nicer if we
used <em>iterators</em>, but that would require a separate method (because we are returning <code>Ranges</code>
and not <code>IEnumerable&lt;T&gt;</code> here). We could also use a named type rather than tuple (to replace
<code>Item1</code> and <code>Item2</code> with <code>Lower</code> and <code>Upper</code>), which would make it a bit more readable, but it
wouldn't change the structure. Before discussing the code further, let's look at the F# version.</p>
<h2>Restricting ranges in F#</h2>
<p>As with the C# version, we need to start with type definitions. This is not really the important
part, but I wanted to have a self-contained sample for the blog, so I'm including those too:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="c">/// A date as a number of days since the beginning</span>
<span class="k">type</span> <span class="t">Date</span> <span class="o">=</span> { <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="i">Offset</span> <span class="o">:</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="t">int</span> }
<span class="c">/// An array of ranges represented as date pairs</span>
<span class="k">type</span> <span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="t">Ranges</span> <span class="o">=</span> { <span onmouseout="hideTip(event, 'fs4', 4)" onmouseover="showTip(event, 'fs4', 4)" class="i">Ranges</span> <span class="o">:</span> (<span onmouseout="hideTip(event, 'fs5', 5)" onmouseover="showTip(event, 'fs5', 5)" class="t">Date</span><span class="o">*</span><span onmouseout="hideTip(event, 'fs5', 6)" onmouseover="showTip(event, 'fs5', 6)" class="t">Date</span>)[] }
</code></pre></td>
</tr>
</table>
<p>If we're happy to use a simple F# record, then this is all we need. For records, the compiler
automatically provides structural equality and structural comparison. This means that we can
write <code>{ Offset = 123 } &lt;= { Offset = 125 }</code> straight away and the result is <code>true</code>. We can also
omit the <code>&lt;summary&gt;</code> tag in the comment (F# adds it automatically).</p>
<p>This is not really the main thing though. In practice, I would use records for simple types that
do not have complex internal logic - and so I might start with the above <code>Date</code> record, but later
turn it into something that is closer to the C# type with explicit definitions. Nevertheless, it
is nice that we can write simple record in the first step and F# gets all the defaults right
(makes it immutable, adds equality and comparison).</p>
<p>The more interesting thing is the <code>restrictRanges</code> function. We follow <em>exactly</em> the same logic
as before (even using <code>Array.collect</code> which is an equivalent of <code>SelectMany</code>):</p>
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
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="c">/// Restrict the specified collection of ranges </span>
<span class="c">/// according to the provided restriction range</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs6', 7)" onmouseover="showTip(event, 'fs6', 7)" class="f">restrictRanges</span> (<span onmouseout="hideTip(event, 'fs7', 8)" onmouseover="showTip(event, 'fs7', 8)" class="i">loRestr</span><span class="o">:</span><span onmouseout="hideTip(event, 'fs5', 9)" onmouseover="showTip(event, 'fs5', 9)" class="t">Date</span>, <span onmouseout="hideTip(event, 'fs8', 10)" onmouseover="showTip(event, 'fs8', 10)" class="i">hiRestr</span><span class="o">:</span><span onmouseout="hideTip(event, 'fs5', 11)" onmouseover="showTip(event, 'fs5', 11)" class="t">Date</span>) <span onmouseout="hideTip(event, 'fs9', 12)" onmouseover="showTip(event, 'fs9', 12)" class="i">ranges</span> <span class="o">=</span>
  <span class="k">let</span> <span onmouseout="hideTip(event, 'fs10', 13)" onmouseover="showTip(event, 'fs10', 13)" class="i">newRanges</span> <span class="o">=</span> 
    <span onmouseout="hideTip(event, 'fs9', 14)" onmouseover="showTip(event, 'fs9', 14)" class="i">ranges</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 15)" onmouseover="showTip(event, 'fs11', 15)" class="i">Ranges</span> <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs12', 16)" onmouseover="showTip(event, 'fs12', 16)" class="t">Array</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs13', 17)" onmouseover="showTip(event, 'fs13', 17)" class="f">collect</span> (<span class="k">fun</span> (<span onmouseout="hideTip(event, 'fs14', 18)" onmouseover="showTip(event, 'fs14', 18)" class="i">lo</span>, <span onmouseout="hideTip(event, 'fs15', 19)" onmouseover="showTip(event, 'fs15', 19)" class="i">hi</span>) <span class="k">-&gt;</span>
        <span class="k">if</span> <span onmouseout="hideTip(event, 'fs14', 20)" onmouseover="showTip(event, 'fs14', 20)" class="i">lo</span> <span class="o">&gt;</span><span class="o">=</span> <span onmouseout="hideTip(event, 'fs7', 21)" onmouseover="showTip(event, 'fs7', 21)" class="i">loRestr</span> <span class="o">&amp;&amp;</span> <span onmouseout="hideTip(event, 'fs15', 22)" onmouseover="showTip(event, 'fs15', 22)" class="i">hi</span> <span class="o">&lt;=</span> <span onmouseout="hideTip(event, 'fs8', 23)" onmouseover="showTip(event, 'fs8', 23)" class="i">hiRestr</span> <span class="k">then</span> [| <span onmouseout="hideTip(event, 'fs14', 24)" onmouseover="showTip(event, 'fs14', 24)" class="i">lo</span>, <span onmouseout="hideTip(event, 'fs15', 25)" onmouseover="showTip(event, 'fs15', 25)" class="i">hi</span> |]
        <span class="k">elif</span> <span onmouseout="hideTip(event, 'fs15', 26)" onmouseover="showTip(event, 'fs15', 26)" class="i">hi</span> <span class="o">&lt;</span> <span onmouseout="hideTip(event, 'fs7', 27)" onmouseover="showTip(event, 'fs7', 27)" class="i">loRestr</span> <span class="o">||</span> <span onmouseout="hideTip(event, 'fs14', 28)" onmouseover="showTip(event, 'fs14', 28)" class="i">lo</span> <span class="o">&gt;</span> <span onmouseout="hideTip(event, 'fs8', 29)" onmouseover="showTip(event, 'fs8', 29)" class="i">hiRestr</span> <span class="k">then</span> [| |]
        <span class="k">else</span> [| <span onmouseout="hideTip(event, 'fs16', 30)" onmouseover="showTip(event, 'fs16', 30)" class="f">max</span> <span onmouseout="hideTip(event, 'fs14', 31)" onmouseover="showTip(event, 'fs14', 31)" class="i">lo</span> <span onmouseout="hideTip(event, 'fs7', 32)" onmouseover="showTip(event, 'fs7', 32)" class="i">loRestr</span>, <span onmouseout="hideTip(event, 'fs17', 33)" onmouseover="showTip(event, 'fs17', 33)" class="f">min</span> <span onmouseout="hideTip(event, 'fs15', 34)" onmouseover="showTip(event, 'fs15', 34)" class="i">hi</span> <span onmouseout="hideTip(event, 'fs8', 35)" onmouseover="showTip(event, 'fs8', 35)" class="i">hiRestr</span> |] )
  { <span onmouseout="hideTip(event, 'fs3', 36)" onmouseover="showTip(event, 'fs3', 36)" class="i">Ranges</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs10', 37)" onmouseover="showTip(event, 'fs10', 37)" class="i">newRanges</span> }
</code></pre></td>
</tr>
</table>
<p>Alternatively, we could use sequence expressions (which I prefer in this case) and rewrite replace
the <code>Array.collect</code> function with the <code>[| .. |]</code> block, which gives us:</p>
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
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="c">/// Restrict the specified collection of ranges </span>
<span class="c">/// according to the provided restriction range</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs18', 38)" onmouseover="showTip(event, 'fs18', 38)" class="f">restrictRangesArrExpr</span> (<span onmouseout="hideTip(event, 'fs7', 39)" onmouseover="showTip(event, 'fs7', 39)" class="i">loRestr</span><span class="o">:</span><span onmouseout="hideTip(event, 'fs5', 40)" onmouseover="showTip(event, 'fs5', 40)" class="t">Date</span>, <span onmouseout="hideTip(event, 'fs8', 41)" onmouseover="showTip(event, 'fs8', 41)" class="i">hiRestr</span><span class="o">:</span><span onmouseout="hideTip(event, 'fs5', 42)" onmouseover="showTip(event, 'fs5', 42)" class="t">Date</span>) <span onmouseout="hideTip(event, 'fs9', 43)" onmouseover="showTip(event, 'fs9', 43)" class="i">ranges</span> <span class="o">=</span>
  <span class="k">let</span> <span onmouseout="hideTip(event, 'fs10', 44)" onmouseover="showTip(event, 'fs10', 44)" class="i">newRanges</span> <span class="o">=</span> 
    [| <span class="k">for</span> <span onmouseout="hideTip(event, 'fs14', 45)" onmouseover="showTip(event, 'fs14', 45)" class="i">lo</span>, <span onmouseout="hideTip(event, 'fs15', 46)" onmouseover="showTip(event, 'fs15', 46)" class="i">hi</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs9', 47)" onmouseover="showTip(event, 'fs9', 47)" class="i">ranges</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs11', 48)" onmouseover="showTip(event, 'fs11', 48)" class="i">Ranges</span> <span class="k">do</span>
        <span class="k">if</span> <span class="i">lo</span> <span class="o">&gt;</span><span class="o">=</span> <span onmouseout="hideTip(event, 'fs7', 49)" onmouseover="showTip(event, 'fs7', 49)" class="i">loRestr</span> <span class="o">&amp;&amp;</span> <span onmouseout="hideTip(event, 'fs15', 50)" onmouseover="showTip(event, 'fs15', 50)" class="i">hi</span> <span class="o">&lt;=</span> <span onmouseout="hideTip(event, 'fs8', 51)" onmouseover="showTip(event, 'fs8', 51)" class="i">hiRestr</span> <span class="k">then</span> <span class="k">yield</span> <span class="i">lo</span>, <span onmouseout="hideTip(event, 'fs15', 52)" onmouseover="showTip(event, 'fs15', 52)" class="i">hi</span>
        <span class="k">elif</span> <span onmouseout="hideTip(event, 'fs15', 53)" onmouseover="showTip(event, 'fs15', 53)" class="i">hi</span> <span class="o">&lt;</span> <span onmouseout="hideTip(event, 'fs7', 54)" onmouseover="showTip(event, 'fs7', 54)" class="i">loRestr</span> <span class="o">||</span> <span class="i">lo</span> <span class="o">&gt;</span> <span onmouseout="hideTip(event, 'fs8', 55)" onmouseover="showTip(event, 'fs8', 55)" class="i">hiRestr</span> <span class="k">then</span> ()
        <span class="k">else</span> <span class="k">yield</span> <span onmouseout="hideTip(event, 'fs16', 56)" onmouseover="showTip(event, 'fs16', 56)" class="f">max</span> <span class="i">lo</span> <span onmouseout="hideTip(event, 'fs7', 57)" onmouseover="showTip(event, 'fs7', 57)" class="i">loRestr</span>, <span onmouseout="hideTip(event, 'fs17', 58)" onmouseover="showTip(event, 'fs17', 58)" class="f">min</span> <span onmouseout="hideTip(event, 'fs15', 59)" onmouseover="showTip(event, 'fs15', 59)" class="i">hi</span> <span onmouseout="hideTip(event, 'fs8', 60)" onmouseover="showTip(event, 'fs8', 60)" class="i">hiRestr</span> |]
  { <span onmouseout="hideTip(event, 'fs3', 61)" onmouseover="showTip(event, 'fs3', 61)" class="i">Ranges</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs10', 62)" onmouseover="showTip(event, 'fs10', 62)" class="i">newRanges</span> }
</code></pre></td>
</tr>
</table>
<h2>Comparing the two versions</h2>
<p>It is certainly a matter of taste, but I was quite surprised by how different the F# version
looks - both versions implement the same logic and both use functional programming style, but
there are a few little details that (in my opinion) make the F# version nicer.</p>
<ul>
<li>
<p>Pattern matching on tuples really helps here. We can just write <code>(lo, hi)</code> as the function
parameter and then use the two variables rather than accessing the items using
<code>range.Item1</code> and <code>range.Item2</code> (or, if we had a named type <code>range.Lower</code> and <code>range.Upper</code>).
As we are using the variables locally (on just 4 lines of code), I think the additional
verbosity is not really helping readablity in this case.</p>
</li>
<li>
<p>Type inference and array literals mean that we can just write <code>[| |]</code> to return an empty
array. This is quite a simplification from <code>new Tuple&lt;Date, Date&gt;[0]</code>, which is what we
had to write before. This is even easier with sequence expressions, where we just do nothing
using <code>()</code> and use <code>yield</code> in other branches.</p>
</li>
<li>
<p>The fact that we can use <code>Array.collect</code> rather than using <code>SelectMany</code> on <code>IEnumerable&lt;T&gt;</code>
is a nice little detail too - we do not have to explicitly convert the result to array using
<code>ToArray</code>.</p>
</li>
<li>
<p>Finally, the <code>max</code> and <code>min</code> functions in F# are <a href="http://tomasp.net/blog/fsharp-generic-numeric.aspx/">generic numerical
functions</a>, which means that they work
on any type that supports comparison. They are also <code>inline</code> and so they are fast and do not
require boxing (which would be the case if you wrote a function like this in C# - probably a
reason why <code>Math.Max</code> does not have a generic overload...).</p>
</li>
</ul>
<p>For me, the interesting thing about this comparison is that it is not looking at any big ideas.
It is comparing two functions written in the same style, using pretty much the same code. But
even then, the using F# gives us a couple of little benefits that make the code (I think) nicer.</p>
<p>There is no fundamental reason why C# could not do any of these in a future version. In fact, I
think that pattern matching on tuples gets mentioned quite often. But this were just 4 "little
things" that I found in one 7-line function...</p>
<p>It might also be the case that I'm more used to writing and reading F# - this is, of course, true -
but if we look at what we deleted, I think it was mostly noise: things like <code>new [] { .. }</code>,
<code>Tuple&lt;Date, Date&gt;</code>, <code>range.Item1 &gt; loRestr ? .. : ..</code>, <code>ToArray()</code>, <code>.Item1</code> and <code>.Item2</code> are
all about the implementation details and not about the function logic.</p>


<div class="tip" id="fs1">Date.Offset: int</div>
<div class="tip" id="fs2">Multiple items<br />val int : value:&#39;T -&gt; int (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.int<br /><br />--------------------<br />type int = int32<br /><br />Full name: Microsoft.FSharp.Core.int<br /><br />--------------------<br />type int&lt;&#39;Measure&gt; = int<br /><br />Full name: Microsoft.FSharp.Core.int&lt;_&gt;</div>
<div class="tip" id="fs3">type Ranges =<br />&#160;&#160;{Ranges: (Date * Date) [];}<br /><br />Full name: Restricting-ranges.Ranges<br /><em><br /><br />&#160;An array of ranges represented as date pairs</em></div>
<div class="tip" id="fs4">Multiple items<br />Ranges.Ranges: (Date * Date) []<br /><br />--------------------<br />type Ranges =<br />&#160;&#160;{Ranges: (Date * Date) [];}<br /><br />Full name: Restricting-ranges.Ranges<br /><em><br /><br />&#160;An array of ranges represented as date pairs</em></div>
<div class="tip" id="fs5">type Date =<br />&#160;&#160;{Offset: int;}<br /><br />Full name: Restricting-ranges.Date<br /><em><br /><br />&#160;A date as a number of days since the beginning</em></div>
<div class="tip" id="fs6">val restrictRanges : loRestr:Date * hiRestr:Date -&gt; ranges:Ranges -&gt; Ranges<br /><br />Full name: Restricting-ranges.restrictRanges<br /><em><br /><br />&#160;Restrict the specified collection of ranges <br />&#160;according to the provided restriction range</em></div>
<div class="tip" id="fs7">val loRestr : Date</div>
<div class="tip" id="fs8">val hiRestr : Date</div>
<div class="tip" id="fs9">val ranges : Ranges</div>
<div class="tip" id="fs10">val newRanges : (Date * Date) []</div>
<div class="tip" id="fs11">Ranges.Ranges: (Date * Date) []</div>
<div class="tip" id="fs12">module Array<br /><br />from Microsoft.FSharp.Collections</div>
<div class="tip" id="fs13">val collect : mapping:(&#39;T -&gt; &#39;U []) -&gt; array:&#39;T [] -&gt; &#39;U []<br /><br />Full name: Microsoft.FSharp.Collections.Array.collect</div>
<div class="tip" id="fs14">val lo : Date</div>
<div class="tip" id="fs15">val hi : Date</div>
<div class="tip" id="fs16">val max : e1:&#39;T -&gt; e2:&#39;T -&gt; &#39;T (requires comparison)<br /><br />Full name: Microsoft.FSharp.Core.Operators.max</div>
<div class="tip" id="fs17">val min : e1:&#39;T -&gt; e2:&#39;T -&gt; &#39;T (requires comparison)<br /><br />Full name: Microsoft.FSharp.Core.Operators.min</div>
<div class="tip" id="fs18">val restrictRangesArrExpr : loRestr:Date * hiRestr:Date -&gt; ranges:Ranges -&gt; Ranges<br /><br />Full name: Restricting-ranges.restrictRangesArrExpr<br /><em><br /><br />&#160;Restrict the specified collection of ranges <br />&#160;according to the provided restriction range</em></div>
