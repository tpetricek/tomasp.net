Power of mathematics: Reasoning about functional types
======================================================

 - date: 2013-05-14T17:54:00.0000000
 - description: In this article, I explore the amazing relationship between functional data types and algebraic operations. We will use this relationship to reason about domain model and understand the differences between several possible representations.
 - layout: article
 - tags: f#,research,functional programming
 - title: Power of mathematics: Reasoning about functional types
 - url: types-and-math.aspx
 - rawbody: true

--------------------------------------------------------------------------------
<img src="http://tomasp.net/articles/types-and-maths/distributivity.png" class="rdecor" />
<p>One of the most amazing aspects of mathematics is that it applies to such a wide range
of areas. The same mathematical rules can be applied to completely different objects
(say, forces in physics or markets in economics) and they work exactly the same way.</p>
<p>In this article, we'll look at one such fascinating use of mathematics - we'll use
elementary school algebra to reason about functional data types.</p>
<p>In functional programming, the best way to start solving a problem is to think about
the data types that are needed to represent the data that you will be working with.
This gives you a simple starting point and a great tool to communicate and
develop your ideas. I call this approach <a href="http://tomasp.net/blog/type-first-development.aspx">Type-First Development and I wrote about
it earlier</a>, so I won't repeat
that here.</p>
<p>The two most elementary types in functional languages are <em>tuples</em> (also called pairs
or product types) and <em>discriminated unions</em> (also called algebraic data types, case
classes or sum types). It turns out that these two types are closely related to
<em>multiplication</em> and <em>addition</em> in algebra...</p>


--------------------------------------------------------------------------------
<h1><span class="hm">Power of mathematics</span><span class="hs"> Reasoning about functional types</span></h1>
<img src="http://tomasp.net/articles/types-and-maths/distributivity.png" class="rdecor" />
<p>One of the most amazing aspects of mathematics is that it applies to such a wide range
of areas. The same mathematical rules can be applied to completely different objects
(say, forces in physics or markets in economics) and they work exactly the same way.</p>
<p>In this article, we'll look at one such fascinating use of mathematics - we'll use
elementary school algebra to reason about functional data types.</p>
<p>In functional programming, the best way to start solving a problem is to think about
the data types that are needed to represent the data that you will be working with.
This gives you a simple starting point and a great tool to communicate and
develop your ideas. I call this approach <a href="http://tomasp.net/blog/type-first-development.aspx">Type-First Development and I wrote about
it earlier</a>, so I won't repeat
that here.</p>
<p>The two most elementary types in functional languages are <em>tuples</em> (also called pairs
or product types) and <em>discriminated unions</em> (also called algebraic data types, case
classes or sum types). It turns out that these two types are closely related to
<em>multiplication</em> and <em>addition</em> in algebra.</p>
<h2>What do we know about types?</h2>
<p>During a recent F# training in New York, I talked about modelling European stock
options (a version of this example is also available <a href="http://www.tryfsharp.org/Learn/financial-computing#understanding-european-options">in Try F#</a>).
The idea is that we want to model stock options - a stock option is either a primitive
put or call option (meaning that we have a contract to buy or sell a commodity) and
a combination of the two.</p>
<p>As we talked about the problem, we tried a number of approaches and tried to find the
most natural representation. Among others, we looked at the following two models:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="c">// Model #1: A stock option is either &#39;Put&#39; or &#39;Call&#39;</span>
<span class="c">// or it is a combination of two other options</span>
<span class="k">type</span> <span class="t">OptionPCC</span> <span class="o">=</span> 
  | <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="p">Put</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="t">float</span> 
  | <span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="p">Call</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs2', 4)" onmouseover="showTip(event, 'fs2', 4)" class="t">float</span>
  | <span onmouseout="hideTip(event, 'fs4', 5)" onmouseover="showTip(event, 'fs4', 5)" class="p">Combine</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs5', 6)" onmouseover="showTip(event, 'fs5', 6)" class="t">OptionPCC</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs5', 7)" onmouseover="showTip(event, 'fs5', 7)" class="t">OptionPCC</span>
</code></pre></td>
</tr>
</table>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="c">// Model #2: A stock option is either primitive European</span>
<span class="c">// option (which is either put or call) or a combination</span>
<span class="k">type</span> <span onmouseout="hideTip(event, 'fs6', 8)" onmouseover="showTip(event, 'fs6', 8)" class="t">OptionKind</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs7', 9)" onmouseover="showTip(event, 'fs7', 9)" class="p">Put</span> | <span onmouseout="hideTip(event, 'fs8', 10)" onmouseover="showTip(event, 'fs8', 10)" class="p">Call</span>
<span class="k">type</span> <span onmouseout="hideTip(event, 'fs9', 11)" onmouseover="showTip(event, 'fs9', 11)" class="t">OptionEC</span> <span class="o">=</span> 
  | <span onmouseout="hideTip(event, 'fs10', 12)" onmouseover="showTip(event, 'fs10', 12)" class="p">European</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs6', 13)" onmouseover="showTip(event, 'fs6', 13)" class="t">OptionKind</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs2', 14)" onmouseover="showTip(event, 'fs2', 14)" class="t">float</span>
  | <span onmouseout="hideTip(event, 'fs11', 15)" onmouseover="showTip(event, 'fs11', 15)" class="p">Combine</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs9', 16)" onmouseover="showTip(event, 'fs9', 16)" class="t">OptionEC</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs9', 17)" onmouseover="showTip(event, 'fs9', 17)" class="t">OptionEC</span>
</code></pre></td>
</tr>
</table>
<p>Discussing which one is better (or easier to process) is one topic, but there is
a more fundamental question. Do they represent the same thing, or does each of the
types model slightly different domain? (You can probably look at the two types and
think that they model, in fact, the same structure, but how do you know that?)
I'll answer this question soon, but I first need to say a bit
more about tuples and discriminated unions.</p>
<h2>Product types and sum types</h2>
<h3>Tuples aka product types</h3>
<p>I mentioned that the two types are also called <em>product</em> and <em>sum</em> types, so let's look
why. A tuple (product) is simply a type that groups together two or more values of
(possibly) different types. In F#, we can define a type alias to give a name to a
tuple:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs12', 18)" onmouseover="showTip(event, 'fs12', 18)" class="t">Point</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs13', 19)" onmouseover="showTip(event, 'fs13', 19)" class="t">byte</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs13', 20)" onmouseover="showTip(event, 'fs13', 20)" class="t">byte</span>
</code></pre></td>
</tr>
</table>
<p>For simplicity, a point is simply a pair of bytes. Why is the type written using <code>*</code>?
This should be easy to see with points - a byte here represents one axis from 0 to 256.
A pair of bytes thus represents a 2D area of size 256*256. This means that the number
of values that <code>Point</code> can have is the number of values <code>byte</code> can have squared. In
other words, if <em>b</em> is the number of values:</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/fig1.png" />
</div>
<h3>Unions aka sum types</h3>
<p>Next, let's take a look at the second type. A <em>discriminated union</em> can be used to
represent a choice between several options (a bit like enumeration). Let's say we
need a type that can represent two cases - one case is that we have a <code>byte</code> value
and the other is that the value is not set. In F# this can be done using the <code>option</code>
type (<code>Maybe</code> in Haskell). A simplified version for bytes looks like this:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs14', 21)" onmouseover="showTip(event, 'fs14', 21)" class="t">ByteOption</span> <span class="o">=</span> 
  | <span onmouseout="hideTip(event, 'fs15', 22)" onmouseover="showTip(event, 'fs15', 22)" class="p">Some</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs13', 23)" onmouseover="showTip(event, 'fs13', 23)" class="t">byte</span>
  | <span onmouseout="hideTip(event, 'fs16', 24)" onmouseover="showTip(event, 'fs16', 24)" class="p">None</span> 
</code></pre></td>
</tr>
</table>
<p>How many possible values does <code>ByteOption</code> have? This is quite easy to count - for
every value <code>b</code> of type <code>byte</code>, there is one value <code>Some(b)</code>, which gives us 256
possible values. In addition, there is one special value <code>None</code>, so we get
<em>256+1</em> possible values altogether. In other words:</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/fig2.png" />
</div>
<p>In general, a <em>sum</em> type corresponds to the sum of the individual components. To relate
this to the earlier geometrical analogy, you can think of a type that can represent
256 positive byte values and 256 negative byte values (that is, 512 possible values
altogether). This would be defined simply as:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs17', 25)" onmouseover="showTip(event, 'fs17', 25)" class="t">TwoRangeByte</span> <span class="o">=</span> 
  | <span onmouseout="hideTip(event, 'fs18', 26)" onmouseover="showTip(event, 'fs18', 26)" class="p">Positive</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs13', 27)" onmouseover="showTip(event, 'fs13', 27)" class="t">byte</span>
  | <span onmouseout="hideTip(event, 'fs19', 28)" onmouseover="showTip(event, 'fs19', 28)" class="p">Negative</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs13', 29)" onmouseover="showTip(event, 'fs13', 29)" class="t">byte</span>
</code></pre></td>
</tr>
</table>
<p>I'll extend the analogy between types and the <em>number of values</em> that a type can have
a bit further. In F#, the <code>unit</code> type is a type that only has one possible value,
written <code>()</code>. This means that it corresponds to <em>1</em> in mathematics. It also means that
<code>None</code> case of <code>ByteOption</code> (that I discussed earlier) could also be written as
<code>None of unit</code>.</p>
<p>And a one brief side-note: A tuple consisting of <em>n</em> values of type <code>T</code> corresponds
to the n<em>th</em> power of <code>T</code>. This encourages us to view <code>unit</code> as a tuple of zero elements,
because <em>zeroth</em> power of any type is <em>1</em>.</p>
<h2>Representing stock options</h2>
<p>The correspondence between types and algebraic operations gives us a powerful way to
reason about data types. Let's look how we can use it on our two representations of stock
options starting with the latter version:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs6', 30)" onmouseover="showTip(event, 'fs6', 30)" class="i">OptionKind</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs7', 31)" onmouseover="showTip(event, 'fs7', 31)" class="i">Put</span> | <span onmouseout="hideTip(event, 'fs8', 32)" onmouseover="showTip(event, 'fs8', 32)" class="i">Call</span>
</code></pre></td>
</tr>
</table>
<p>The <code>OptionKind</code> type is simply a choice between two alternatives (both can be seen as values
of type <code>unit</code>, because they both have exactly one value). This means we can write them as:</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/fig3.png" />
</div>
<p>The <code>OptionEC</code> type then contains <code>OptionKind</code> combined with <code>float</code> or two options:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs9', 33)" onmouseover="showTip(event, 'fs9', 33)" class="i">OptionEC</span> <span class="o">=</span> 
  | <span onmouseout="hideTip(event, 'fs10', 34)" onmouseover="showTip(event, 'fs10', 34)" class="i">European</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs6', 35)" onmouseover="showTip(event, 'fs6', 35)" class="i">OptionKind</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs2', 36)" onmouseover="showTip(event, 'fs2', 36)" class="i">float</span>
  | <span onmouseout="hideTip(event, 'fs11', 37)" onmouseover="showTip(event, 'fs11', 37)" class="i">Combine</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs9', 38)" onmouseover="showTip(event, 'fs9', 38)" class="i">OptionEC</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs9', 39)" onmouseover="showTip(event, 'fs9', 39)" class="i">OptionEC</span>
</code></pre></td>
</tr>
</table>
<p>This means that <code>OptionEC</code> is a choice (using the <code>+</code> operator) between two alternatives, one
consisting of the <em>kind</em> and a floating-point value that I'll simply write as <em>f</em> and another,
containing two options:</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/fig4.png" />
</div>
<p>The first line directly corresponds to the <code>OptionEC</code> type. The second line simply expands
the definition of <em>kind</em> shown earlier. Now, let's look at the second type:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs5', 40)" onmouseover="showTip(event, 'fs5', 40)" class="i">OptionPCC</span> <span class="o">=</span> 
  | <span onmouseout="hideTip(event, 'fs7', 41)" onmouseover="showTip(event, 'fs7', 41)" class="i">Put</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs2', 42)" onmouseover="showTip(event, 'fs2', 42)" class="i">float</span> 
  | <span onmouseout="hideTip(event, 'fs8', 43)" onmouseover="showTip(event, 'fs8', 43)" class="i">Call</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs2', 44)" onmouseover="showTip(event, 'fs2', 44)" class="i">float</span>
  | <span onmouseout="hideTip(event, 'fs11', 45)" onmouseover="showTip(event, 'fs11', 45)" class="i">Combine</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs5', 46)" onmouseover="showTip(event, 'fs5', 46)" class="i">OptionPCC</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs5', 47)" onmouseover="showTip(event, 'fs5', 47)" class="i">OptionPCC</span>
</code></pre></td>
</tr>
</table>
<p>This is simply a choice (that is <code>+</code> operation) between two floating-point values and
a combination consisting of product of two options. In the language of mathematics:</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/fig5.png" />
</div>
<p>Now comes the important step. We have two equations that describe the two different types.
The key thing is that fundamental algebraic laws (that hold about numbers) also hold about
functional data types. We can use the <a href="http://en.wikipedia.org/wiki/Distributive_property">distributivity law</a>
to show the following:</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/fig6.png" />
</div>
<p>And that's all we need to show that the two representations of stock options represent, in fact,
the same domain (and so we can freely choose which one to use, based on which we find more
natural or easier to process - the key fact is that the choice does not matter for the program logic):</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/fig7.png" />
</div>
<h2>Representing contact details</h2>
<p>Let's look at one more example - this time, we look at two possible representations of
contact information. The example is inspired by <a href="http://fsharpforfunandprofit.com/posts/designing-with-types-discovering-the-domain/">the excellent F# for Fun and Profit
article</a>.</p>
<p>A type representing contact details may contain a phone number (for simplicity,
represented as <code>int</code>) and an address (stored as <code>string</code>). One way to represent
such information is to assume that both of the details are optional and use a
record storing two <code>option</code> types:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs20', 48)" onmouseover="showTip(event, 'fs20', 48)" class="t">Contact1</span> <span class="o">=</span> 
  { <span onmouseout="hideTip(event, 'fs21', 49)" onmouseover="showTip(event, 'fs21', 49)" class="i">Address</span> <span class="o">:</span> <span onmouseout="hideTip(event, 'fs22', 50)" onmouseover="showTip(event, 'fs22', 50)" class="t">string</span> <span onmouseout="hideTip(event, 'fs23', 51)" onmouseover="showTip(event, 'fs23', 51)" class="t">option</span>
    <span onmouseout="hideTip(event, 'fs24', 52)" onmouseover="showTip(event, 'fs24', 52)" class="i">Number</span> <span class="o">:</span> <span onmouseout="hideTip(event, 'fs25', 53)" onmouseover="showTip(event, 'fs25', 53)" class="t">int</span> <span onmouseout="hideTip(event, 'fs23', 54)" onmouseover="showTip(event, 'fs23', 54)" class="t">option</span> }
</code></pre></td>
</tr>
</table>
<p>The second representation we can use is a discriminated union that lists a number
of options explicitly - a contact can have both address and phone number, or just
one of these two:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs26', 55)" onmouseover="showTip(event, 'fs26', 55)" class="t">Contact2</span> <span class="o">=</span> 
  | <span onmouseout="hideTip(event, 'fs27', 56)" onmouseover="showTip(event, 'fs27', 56)" class="p">AddressAndNumber</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs22', 57)" onmouseover="showTip(event, 'fs22', 57)" class="t">string</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs25', 58)" onmouseover="showTip(event, 'fs25', 58)" class="t">int</span>
  | <span onmouseout="hideTip(event, 'fs28', 59)" onmouseover="showTip(event, 'fs28', 59)" class="p">Address</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs22', 60)" onmouseover="showTip(event, 'fs22', 60)" class="t">string</span>
  | <span onmouseout="hideTip(event, 'fs29', 61)" onmouseover="showTip(event, 'fs29', 61)" class="p">Number</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs25', 62)" onmouseover="showTip(event, 'fs25', 62)" class="t">int</span>
</code></pre></td>
</tr>
</table>
<p>A record type is simply a tuple with named elements and so it also corresponds to
<em>multiplication</em> (we could have used <code>(string option) * (int option)</code>, but I wanted
to keep the sample more idiomatic). Recall our discussion about <code>option</code> types earlier -
we said that this is just like adding one to the original type. Now, the second
representation is simply a choice between three options, meaning that we will represent
it using <code>+</code> over the three cases. Altogether, this means that the two types can be
mathematically described as:</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/fig8.png" />
</div>
<p>Now we can apply some more elementary school algebra on the first equation and expand
the multiplication. This way we get the following (just like in mathematics <em>1 * 1 = 1</em> when
we talk about types):</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/fig9.png" />
</div>
<p>Looking at the resulting equation, we can very clearly see that the two types are different.
Moreover, the inequality also explains <em>how</em> they are different:</p>
<div style="text-align:center;margin-bottom:10px;">
<img src="http://tomasp.net/articles/types-and-maths/figa.png" />
</div>
<p>On the left-hand side, we have essentially a choice with four cases while on the
right-hand side, we only have three cases. The cases are the same, so the only difference
is additional <em>1</em> case on the left. This corresponds to the situation when none of the
contact details are provided - this is something that we can represent only using the
<code>Contact1</code> type (by writing <code>{ Address=None; Number=None}</code>).</p>
<p>If we wanted to add this
possibility to <code>Contact2</code>, we can do that quite easily - just add a case <code>NoContact</code> with
no attributes, or use <code>Contact2 option</code> (because this also builds <em>c<sub>2</sub> + 1</em>).</p>
<h2>Summary</h2>
<p>I think that the main takeaway message from this article is that <strong>reasoning about
functional types is easy</strong>. Most of the calculations that I showed in this blog post
are easy to do in your head, without even writing any mathematics. But I wanted to make
them explicit to show how they work in details.</p>
<p>All of the standard algebraic laws such as <a href="http://en.wikipedia.org/wiki/Associative_property">associativity</a>,
<a href="http://en.wikipedia.org/wiki/Distributive_property">distributivity</a> and <a href="http://en.wikipedia.org/wiki/Commutative_property">commutativity</a>
correspond to simple operations that you can apply to your types when building a domain
model. This gives you simple set of basic <em>refactorings</em> that work on <em>types</em> and help
you design an easier to use model.</p>
<p>I will stop here and limit myself to just basic laws and basic types, but one can
go much further. The function type <code>T1 -&gt; T2</code> can be mathematically
modelled as an exponentiation <em>T<sub>2</sub><sup>T<sub>1</sub></sup></em>. The interesting consequence
of this is that (ignoring side-effects) <code>unit -&gt; T</code> is equivalent to just <code>T</code> (because <em>T<sup>1</sup>=T</em>)
and that <code>T1 + T2 -&gt; T</code> is equivalent to <code>(T1 -&gt; T) * (T2 -&gt; T)</code> (using the
<a href="http://mathworld.wolfram.com/ExponentLaws.html">exponent laws</a>).</p>
<p>A bit more esoteric extension (that I reference mainly just for fun) is that you can
also <em>differentiate</em> data types. There is a <a href="http://blog.lab49.com/archives/3011">fairly readable introduction</a>,
but if you want to see the full details, check out this <a href="http://www.cs.nott.ac.uk/~txa/publ/jpartial.pdf">academic paper (PDF)</a>.</p>


<div class="tip" id="fs1">union case OptionPCC.Put: float -&gt; OptionPCC</div>
<div class="tip" id="fs2">Multiple items<br />val float : value:&#39;T -&gt; float (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.float<br /><br />--------------------<br />type float = System.Double<br /><br />Full name: Microsoft.FSharp.Core.float<br /><br />--------------------<br />type float&lt;&#39;Measure&gt; = float<br /><br />Full name: Microsoft.FSharp.Core.float&lt;_&gt;</div>
<div class="tip" id="fs3">union case OptionPCC.Call: float -&gt; OptionPCC</div>
<div class="tip" id="fs4">union case OptionPCC.Combine: OptionPCC * OptionPCC -&gt; OptionPCC</div>
<div class="tip" id="fs5">type OptionPCC =<br />&#160;&#160;| Put of float<br />&#160;&#160;| Call of float<br />&#160;&#160;| Combine of OptionPCC * OptionPCC<br /><br />Full name: Types-and-math.aspx.OptionPCC</div>
<div class="tip" id="fs6">type OptionKind =<br />&#160;&#160;| Put<br />&#160;&#160;| Call<br /><br />Full name: Types-and-math.aspx.OptionKind</div>
<div class="tip" id="fs7">union case OptionKind.Put: OptionKind</div>
<div class="tip" id="fs8">union case OptionKind.Call: OptionKind</div>
<div class="tip" id="fs9">type OptionEC =<br />&#160;&#160;| European of OptionKind * float<br />&#160;&#160;| Combine of OptionEC * OptionEC<br /><br />Full name: Types-and-math.aspx.OptionEC</div>
<div class="tip" id="fs10">union case OptionEC.European: OptionKind * float -&gt; OptionEC</div>
<div class="tip" id="fs11">union case OptionEC.Combine: OptionEC * OptionEC -&gt; OptionEC</div>
<div class="tip" id="fs12">type Point = byte * byte<br /><br />Full name: Types-and-math.aspx.Point</div>
<div class="tip" id="fs13">Multiple items<br />val byte : value:&#39;T -&gt; byte (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.byte<br /><br />--------------------<br />type byte = System.Byte<br /><br />Full name: Microsoft.FSharp.Core.byte</div>
<div class="tip" id="fs14">type ByteOption =<br />&#160;&#160;| Some of byte<br />&#160;&#160;| None<br /><br />Full name: Types-and-math.aspx.ByteOption</div>
<div class="tip" id="fs15">union case ByteOption.Some: byte -&gt; ByteOption</div>
<div class="tip" id="fs16">union case ByteOption.None: ByteOption</div>
<div class="tip" id="fs17">type TwoRangeByte =<br />&#160;&#160;| Positive of byte<br />&#160;&#160;| Negative of byte<br /><br />Full name: Types-and-math.aspx.TwoRangeByte</div>
<div class="tip" id="fs18">union case TwoRangeByte.Positive: byte -&gt; TwoRangeByte</div>
<div class="tip" id="fs19">union case TwoRangeByte.Negative: byte -&gt; TwoRangeByte</div>
<div class="tip" id="fs20">type Contact1 =<br />&#160;&#160;{Address: string option;<br />&#160;&#160;&#160;Number: int option;}<br /><br />Full name: Types-and-math.aspx.Contact1</div>
<div class="tip" id="fs21">Contact1.Address: string option</div>
<div class="tip" id="fs22">Multiple items<br />val string : value:&#39;T -&gt; string<br /><br />Full name: Microsoft.FSharp.Core.Operators.string<br /><br />--------------------<br />type string = System.String<br /><br />Full name: Microsoft.FSharp.Core.string</div>
<div class="tip" id="fs23">type &#39;T option = Option&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Core.option&lt;_&gt;</div>
<div class="tip" id="fs24">Contact1.Number: int option</div>
<div class="tip" id="fs25">Multiple items<br />val int : value:&#39;T -&gt; int (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.int<br /><br />--------------------<br />type int = int32<br /><br />Full name: Microsoft.FSharp.Core.int<br /><br />--------------------<br />type int&lt;&#39;Measure&gt; = int<br /><br />Full name: Microsoft.FSharp.Core.int&lt;_&gt;</div>
<div class="tip" id="fs26">type Contact2 =<br />&#160;&#160;| AddressAndNumber of string * int<br />&#160;&#160;| Address of string<br />&#160;&#160;| Number of int<br /><br />Full name: Types-and-math.aspx.Contact2</div>
<div class="tip" id="fs27">union case Contact2.AddressAndNumber: string * int -&gt; Contact2</div>
<div class="tip" id="fs28">union case Contact2.Address: string -&gt; Contact2</div>
<div class="tip" id="fs29">union case Contact2.Number: int -&gt; Contact2</div>
