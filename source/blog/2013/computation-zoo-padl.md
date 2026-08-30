The F# Computation Expression Zoo (PADL'14)
=============================================

 - date: 2013-11-08T06:42:56.7559875+00:00
 - description: A month or so back, I wrote a tweet saying that F# computation expressions are amazing and no other language has anything like that. I can finally provide all the evidence by linking to a PADL 2014 paper about them.
 - layout: article
 - tags: haskell,research,f#,functional programming
 - title: The F# Computation Expression Zoo (PADL'14)
 - url: 2013/computation-zoo-padl
 - rawbody: true

--------------------------------------------------------------------------------
<p>F# <a href="http://msdn.microsoft.com/en-us/library/dd233182.aspx">computation expressions</a> are the
syntactic language mechanism that is used by features like sequence expressions and asynchronous
workflows. The aim of F# computation expressions is to provide a <em>single</em> syntactic mechanism
that provides convenient notation for writing a wide range of computations.</p>
<p>The syntactic mechanisms that are unified by computation expressions include Haskell <code>do</code>
notation and list comprehensions, C# iterators, asynchronous methods and LINQ queries,
Scala <code>for</code> comprehensions and Python generators to name just a few.</p>
<p>Some time ago, I started working on an academic article to explain what makes computation
expressions unique - and I think there is quite a few interesting aspects. Sadly, this is
often not very well explained and so the general perception is more like this...</p>


--------------------------------------------------------------------------------
<h1>The F# Computation Expression Zoo (PADL'14)</h1>
<p>F# <a href="http://msdn.microsoft.com/en-us/library/dd233182.aspx">computation expressions</a> are the
syntactic language mechanism that is used by features like sequence expressions and asynchronous
workflows. The aim of F# computation expressions is to provide a <em>single</em> syntactic mechanism
that provides convenient notation for writing a wide range of computations.</p>
<p>The syntactic mechanisms that are unified by computation expressions include Haskell <code>do</code>
notation and list comprehensions, C# iterators, asynchronous methods and LINQ queries,
Scala <code>for</code> comprehensions and Python generators to name just a few.</p>
<p>Some time ago, I started working on an academic article to explain what makes computation
expressions unique - and I think there is quite a few interesting aspects. Sadly, this is
often not very well explained and so the general perception is more like this:</p>
<img src="tweets.png" style="margin:10px 0px 30px 40px;" alt="Proof by Tweet!" />
<p>This is something hat we tried to clarify in the upcoming PADL 2014 paper which explains
how computation expression fit with standard abstract computation types <em>such as</em> monads
and shows what makes them unique. If you're interested in the details, then follow the link
to the paper below. In the rest of the blog post, I'll try to give a quick summary with one
interesting example...</p>
<ul>
<li><a href="http://tomasp.net/academic/papers/computation-zoo/">The F# Computation Expression Zoo</a> - To appear in PADL 2014</li>
<li><a href="http://tryjoinads.org/computations">Source with all examples from the paper</a> - on TryJoinads.org</li>
</ul>
<h2>Why computation expressions are more?</h2>
<p>There are two key things about computation expressions that make them quite different
to what other languages provide - the first is emphasis on syntax (to make them look like
single-purpose language features) and the second is flexibility (to make them reusable).</p>
<h3>Syntax</h3>
<p>Computation expressions reuse the normal syntax of the F# language, so
you can write code using standard constructs like <code>let</code>, <code>for</code> and <code>try</code> .. <code>with</code>,
but with a different semantics, depending on the kind of computation. For example,
<code>try</code> .. <code>with</code> inside <code>async</code> captures exceptions that happen on another thread and
propagates them, but all of this is done using standard language mechanism.</p>
<h3>Flexibility</h3>
<p>Computation expressions are not tied to a single abstract type of
computations (e.g. monad). Instead, they give us a single syntactic mechanism, that can
give a nice syntax for multiple different abstract computation types. These include
monads, but also applicative functors (using a research extension), monoids, additive
monads, computations constructed using monad transformers and more.</p>
<p>But what's the point? Aren't <em>monad transformers</em> and <em>additive monads</em> just <em>monads</em>?
Of course, they are monads, but they have some additional structure. For example, additive
monads have the <code>mplus</code> operation of type <code>'a m -&gt; 'a m -&gt; 'a m</code> and <code>mzero</code> of type <code>'a m</code>.
In Haskell, you can just use the <code>do</code> notation, but to access the additional structure,
you'll need to use the combinators, which can make the syntax more complex.</p>
<p>When using computation expressions, the operations like <code>mplus</code> and <code>mzero</code> become
<em>a part of the syntax</em>. In many cases, you will not need to call them explicitly via
a function, because they <em>enable</em> additional syntax in the computation expression that
calls them.</p>
<h3>Limitations</h3>
<p>To be fair, I should mention common criticism of computation expressions too.
F# does not easily let you write code that is polymorphic over the type of computation
(it can be done, but it is not particularly idiomatic and it is certainly not recommended).</p>
<p>If you learned about monads in Haskell, this might sound like a major issue to you.
However, it is not a big problem in practice, because F# and Haskell use monads (and
other computations) in very different ways. In Haskell, they are fundamental part of the
language and you need them all the time. In F#, they are only used when you actually
want to write some concrete computation with non-standard behaviour. For example, asynchronous
computation or a formlet - in such case, you will almost always want to work with a
concrete computation type. As we'll see later, this has another useful consequence -
it means that the syntax of computation expressions can be tuned to a concrete use case.</p>
<h2>Case study: Two additive monads</h2>
<p>If you want to see all the abstractions that are supported by computation expressions,
then <a href="http://tomasp.net/academic/papers/computation-zoo/">read the paper</a>. I will not
repeat everything in this blog post.</p>
<p>However, I want to show one example that demonstrates the key point of computation expressions
very well. The example is encoding of <em>additive monads</em> - that is, computation types that
are monads and have additional operations <code>mplus : 'a m -&gt; 'a m -&gt; 'a m</code> and <code>mzero : 'a m</code>.
In Haskell, these are captured by the <code>MonadPlus</code> type class.</p>
<p>I'll show how computation expressions look for two different computation. The first one
is based on the list monad (although I'll use more idiomatic F# <code>seq&lt;'T&gt;</code> computations)
and the other is essentially a delayed <code>option&lt;'T&gt;</code> type (aka Maybe monad), but it more
closely models the Haskell <code>IO</code> instance for <code>MonadPlus</code>.</p>
<h3>Sequence expressions</h3>
<p>Let's say that we want to write a function that takes a list, such as <code>[1; 2; 3]</code> and
duplicates each element in the list returning <code>[1; 1; 2; 2; 3; 3]</code>. The syntax for doing
this is very close to the one of <a href="http://freepythontips.wordpress.com/2013/09/29/the-python-yield-keyword-explained/">Python generators</a>
and looks as follows:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs44', 87)" onmouseover="showTip(event, 'fs44', 87)" class="f">duplicate</span> <span onmouseout="hideTip(event, 'fs45', 88)" onmouseover="showTip(event, 'fs45', 88)" class="i">input</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs26', 89)" onmouseover="showTip(event, 'fs26', 89)" class="i">seq</span> { 
  <span class="k">for</span> <span onmouseout="hideTip(event, 'fs46', 90)" onmouseover="showTip(event, 'fs46', 90)" class="i">num</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs45', 91)" onmouseover="showTip(event, 'fs45', 91)" class="i">input</span> <span class="k">do</span>
    <span class="k">yield</span> <span onmouseout="hideTip(event, 'fs46', 92)" onmouseover="showTip(event, 'fs46', 92)" class="i">num</span> 
    <span class="k">yield</span> <span onmouseout="hideTip(event, 'fs46', 93)" onmouseover="showTip(event, 'fs46', 93)" class="i">num</span> }
</code></pre></td>
</tr>
</table>
<p>How does this work? The <code>for</code> keyword is mapped to the <em>bind</em> operation of the underlying
computation and the <code>yield</code> keyword is mapped to the <em>return</em> operation of the monad.
When the body contains multiple statements that return a value, the statements are combined
using the additive operation (<code>mplus</code>).</p>
<p>The syntax and semantics of the computation is determined by the <code>seq</code> identifier - this
is not actually an identifier, but an <em>object</em> that defines a number of members that tell
the compiler that 1) it should enable <code>for</code>, <code>yield</code> and multiple statements in a body
and 2) what is the implementation of the three underlying operations.</p>
<p>When defining the syntax in the <code>seq</code> value, we could have chosen another syntax - we could
use <code>let!</code> and <code>return</code> instead of using <code>for</code> and <code>yield</code>, but the latter option gives a
more convenient and intuitive syntax.</p>
<p>What do I mean by "more intuitive"? Let's look at the laws! The <a href="http://www.haskell.org/haskellwiki/MonadPlus"><em>left distribution</em>
law</a> about <code>MonadPlus</code> (which holds for lists)
states that the following two computations should be equivalent:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
<span class="l">7: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs47', 94)" onmouseover="showTip(event, 'fs47', 94)" class="f">left</span> <span onmouseout="hideTip(event, 'fs48', 95)" onmouseover="showTip(event, 'fs48', 95)" class="i">in1</span> <span onmouseout="hideTip(event, 'fs49', 96)" onmouseover="showTip(event, 'fs49', 96)" class="i">in2</span> <span onmouseout="hideTip(event, 'fs50', 97)" onmouseover="showTip(event, 'fs50', 97)" class="f">f</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs26', 98)" onmouseover="showTip(event, 'fs26', 98)" class="i">seq</span> { 
  <span class="k">for</span> <span onmouseout="hideTip(event, 'fs51', 99)" onmouseover="showTip(event, 'fs51', 99)" class="i">x</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs48', 100)" onmouseover="showTip(event, 'fs48', 100)" class="i">in1</span> <span class="k">do</span> <span class="k">yield!</span> <span onmouseout="hideTip(event, 'fs50', 101)" onmouseover="showTip(event, 'fs50', 101)" class="f">f</span> <span onmouseout="hideTip(event, 'fs51', 102)" onmouseover="showTip(event, 'fs51', 102)" class="i">x</span>
  <span class="k">for</span> <span onmouseout="hideTip(event, 'fs51', 103)" onmouseover="showTip(event, 'fs51', 103)" class="i">x</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs49', 104)" onmouseover="showTip(event, 'fs49', 104)" class="i">in2</span> <span class="k">do</span> <span class="k">yield!</span> <span onmouseout="hideTip(event, 'fs50', 105)" onmouseover="showTip(event, 'fs50', 105)" class="f">f</span> <span onmouseout="hideTip(event, 'fs51', 106)" onmouseover="showTip(event, 'fs51', 106)" class="i">x</span> }

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs52', 107)" onmouseover="showTip(event, 'fs52', 107)" class="f">right</span> <span onmouseout="hideTip(event, 'fs48', 108)" onmouseover="showTip(event, 'fs48', 108)" class="i">in1</span> <span onmouseout="hideTip(event, 'fs49', 109)" onmouseover="showTip(event, 'fs49', 109)" class="i">in2</span> <span onmouseout="hideTip(event, 'fs50', 110)" onmouseover="showTip(event, 'fs50', 110)" class="f">f</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs26', 111)" onmouseover="showTip(event, 'fs26', 111)" class="i">seq</span> { 
  <span class="k">for</span> <span onmouseout="hideTip(event, 'fs51', 112)" onmouseover="showTip(event, 'fs51', 112)" class="i">x</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs26', 113)" onmouseover="showTip(event, 'fs26', 113)" class="i">seq</span> { <span class="k">yield!</span> <span onmouseout="hideTip(event, 'fs48', 114)" onmouseover="showTip(event, 'fs48', 114)" class="i">in1</span>; <span class="k">yield!</span> <span onmouseout="hideTip(event, 'fs49', 115)" onmouseover="showTip(event, 'fs49', 115)" class="i">in2</span> } <span class="k">do</span>
    <span class="k">yield!</span> <span onmouseout="hideTip(event, 'fs50', 116)" onmouseover="showTip(event, 'fs50', 116)" class="f">f</span> <span onmouseout="hideTip(event, 'fs51', 117)" onmouseover="showTip(event, 'fs51', 117)" class="i">x</span> }
</code></pre></td>
</tr>
</table>
<p>This looks like a very reasonable law to require for any sequence or list-like computation.
So, if you have a list-like computation, then this is the right syntax to use. For example,
<a href="http://www.haskell.org/haskellwiki/MonadPlus">asynchronous sequences</a> use it too.</p>
<p>However, what if we have a computation where the above law does not hold?</p>
<h3>Imperative computations</h3>
<p>Another example of additive monad are imperative computations
that I discussed in <a href="http://tomasp.net/blog/imperative-i-return.aspx/">earlier article on this blog</a>.
The aim is to let you write computations that are similar to C-like imperative control flow
with <code>return</code>, <code>break</code> and <code>continue</code> keywords. The following is a simple example of using
the imperative-style <code>return</code> - assuming we have a list of <code>blockedWords</code>, we  want to
return <code>true</code> when the given message contains any word.</p>
<p>Of course, this could easily be written using a built-in function like <code>List.exists</code>, but that's
not the point - the point is that we can define computation expression that closely models
the C-like syntax and could be used to define more complex computations and implement
more complex behaviours:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs53', 118)" onmouseover="showTip(event, 'fs53', 118)" class="f">blockMessage</span> (<span onmouseout="hideTip(event, 'fs54', 119)" onmouseover="showTip(event, 'fs54', 119)" class="i">msg</span><span class="o">:</span><span onmouseout="hideTip(event, 'fs55', 120)" onmouseover="showTip(event, 'fs55', 120)" class="t">string</span>) <span class="o">=</span> <span onmouseout="hideTip(event, 'fs42', 121)" onmouseover="showTip(event, 'fs42', 121)" class="i">imperative</span> {
  <span class="k">for</span> <span onmouseout="hideTip(event, 'fs56', 122)" onmouseover="showTip(event, 'fs56', 122)" class="i">word</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs43', 123)" onmouseover="showTip(event, 'fs43', 123)" class="i">blockedWords</span> <span class="k">do</span>
    <span class="k">if</span> <span onmouseout="hideTip(event, 'fs54', 124)" onmouseover="showTip(event, 'fs54', 124)" class="i">msg</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs57', 125)" onmouseover="showTip(event, 'fs57', 125)" class="f">Contains</span>(<span onmouseout="hideTip(event, 'fs56', 126)" onmouseover="showTip(event, 'fs56', 126)" class="i">word</span>) <span class="k">then</span> <span class="k">return</span> <span class="k">true</span>
  <span class="k">return</span> <span class="k">false</span> }
</code></pre></td>
</tr>
</table>
<p>Again, the syntax and the semantics is determined by the <code>imperative</code> object. You can
find the details of the implementation in the <a href="http://tomasp.net/blog/imperative-i-return.aspx/">blog post mentioned earlier</a>.
The key thing is that the object determines that we want to use the <code>return</code> keyword
for the <em>return</em> operation of the monad.</p>
<p>The <code>for</code> loop used here is just an ordinary
loop that iterates over sequence - it is defined in terms of standard iteration over
lists and the <em>bind</em> operation of the monad. This is an example of the flexibility
of computation expressions - because they are used with <em>specific</em> computations, it makes
sense to design them so that they give us the most intuitive syntax (here, resembling
C-like imperative computations).</p>
<p>Interestingly, imperative computations do not obey the <em>left distribution</em> law that worked
for sequences, but instead, they obey the <a href="http://www.haskell.org/haskellwiki/MonadPlus"><em>left catch</em> law</a>.
The law statest that the following two computations are equivalent:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs58', 127)" onmouseover="showTip(event, 'fs58', 127)" class="i">left</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs42', 128)" onmouseover="showTip(event, 'fs42', 128)" class="i">imperative</span> {
  <span class="k">return</span> <span class="n">0</span>
  <span onmouseout="hideTip(event, 'fs59', 129)" onmouseover="showTip(event, 'fs59', 129)" class="f">printfn</span> <span class="s">&quot;Unreachable!&quot;</span> }

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs60', 130)" onmouseover="showTip(event, 'fs60', 130)" class="i">right</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs42', 131)" onmouseover="showTip(event, 'fs42', 131)" class="i">imperative</span> {
  <span class="k">return</span> <span class="n">0</span> }
</code></pre></td>
</tr>
</table>
<p>In the original formulation of the law, the expression following <code>return 0</code> in the left
expression can be anything - but calling <code>printfn</code> is completely sufficient for this
quick overview.</p>
<p>It is not surprising that the law holds for imperative computations. So again, the law
nicely accompanies the syntax that we have chosen when defining the imperative computation
builder (the <code>imperative</code> value). If we used the <code>yield</code> keyword in this example, or
if we used <code>return</code> in the previous one (sequences), then there would be a mismatch between
the syntax and the laws.</p>
<h3>MonadPlus versus MonadOr</h3>
<p>Interestingly, the distinction between the two sample computations that I used in the
previous examples exists in Haskell too. The <a href="http://www.haskell.org/haskellwiki/MonadPlus_reform_proposal">MonadPlus reform proposal</a>
suggests that computations satisfying <em>left distribution</em> should form a type class
<code>MonadPlus</code>, while computations satisfying <em>left catch</em> should be captured by <code>MonadOr</code>.</p>
<p>Having two different type classes would mean that you'd explicitly use one of the
two combinators - either <code>mplus</code> or <code>morelse</code> and so the resulting Haskell code would
be somewhat less generic over the type of computation and a bit closer to what you get with
F# computation expression encoding of <em>additive monads</em>.</p>
<p>I believe it is interesting that you can discover this important distinction from
multiple directions - by considering the laws, or by looking at the most natural syntax
for writing such computations.</p>
<h2>Summary</h2>
<p>This blog post is a quick (and over-simplified) overview of what you can find in our
paper <a href="http://tomasp.net/academic/papers/computation-zoo/">The F# Computation Expression Zoo</a>
that has been accepted at the PADL workshop 2014.</p>
<p>I tried to give two simple examples that demonstrate the main points:</p>
<ul>
<li>
<p><strong>Syntax matters</strong> - computation expressions aim to match with built-in language features
like Python generators and so the ability to choose the right syntax is crucial. However,
this is done without full support for macros. While macros can achieve this too, they tend
to be too powerful.</p>
</li>
<li>
<p><strong>Expressivity</strong> - the author of a computation should be able to decide how the computation
looks and define more operations (e.g. to support multiple <code>yield</code> constructs) if the
computation type allows this.</p>
</li>
</ul>
<p>Another point of this article is to show that computation expressions are not, in fact,
just an <em>"enterprise-y name for monad"</em>. Firstly, they are based on quite different principles
and, secondly, they can capture wider range of computations (including additive monads and
monad transformers) and <em>leverage</em> the additional operations available in these computations.</p>
<p>The slogan of the last feature could be <strong>require less, leverage more</strong>. In the research
extension available on <a href="http://tryjoinads.org">TryJoinads.org</a>, you can use computation
expressions even if you have <em>just</em> applicative functor. But if you have more - say, a monad
or even additive monad - then the additional structure enables more syntactic options and so
you do not need to resort to explicit use of combinators as often.</p>


<div class="tip" id="fs1">namespace System</div>
<div class="tip" id="fs2">namespace System.Collections</div>
<div class="tip" id="fs3">namespace System.Collections.Generic</div>
<div class="tip" id="fs4">type Imperative&lt;&#39;T&gt; = unit -&gt; &#39;T option<br /><br />Full name: Computation-zoo-padl.Imperative&lt;_&gt;</div>
<div class="tip" id="fs5">type unit = Unit<br /><br />Full name: Microsoft.FSharp.Core.unit</div>
<div class="tip" id="fs6">type &#39;T option = Option&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Core.option&lt;_&gt;</div>
<div class="tip" id="fs7">Multiple items<br />type ImperativeBuilder =<br />&#160;&#160;new : unit -&gt; ImperativeBuilder<br />&#160;&#160;member Combine : a:(unit -&gt; &#39;h option) * b:(unit -&gt; &#39;h option) -&gt; (unit -&gt; &#39;h option)<br />&#160;&#160;member Delay : f:(unit -&gt; Imperative&lt;&#39;g&gt;) -&gt; Imperative&lt;&#39;g&gt;<br />&#160;&#160;member For : inp:seq&lt;&#39;b&gt; * f:(&#39;b -&gt; unit -&gt; &#39;c option) -&gt; (unit -&gt; &#39;c option)<br />&#160;&#160;member Return : v:&#39;f -&gt; Imperative&lt;&#39;f&gt;<br />&#160;&#160;member Run : imp:(unit -&gt; &#39;d option) -&gt; &#39;d<br />&#160;&#160;member While : gd:(unit -&gt; bool) * body:(unit -&gt; &#39;a option) -&gt; (unit -&gt; &#39;a option)<br />&#160;&#160;member Zero : unit -&gt; (unit -&gt; &#39;e option)<br /><br />Full name: Computation-zoo-padl.ImperativeBuilder<br /><br />--------------------<br />new : unit -&gt; ImperativeBuilder</div>
<div class="tip" id="fs8">val x : ImperativeBuilder</div>
<div class="tip" id="fs9">member ImperativeBuilder.Combine : a:(unit -&gt; &#39;h option) * b:(unit -&gt; &#39;h option) -&gt; (unit -&gt; &#39;h option)<br /><br />Full name: Computation-zoo-padl.ImperativeBuilder.Combine</div>
<div class="tip" id="fs10">val a : (unit -&gt; &#39;h option)</div>
<div class="tip" id="fs11">val b : (unit -&gt; &#39;h option)</div>
<div class="tip" id="fs12">union case Option.Some: Value: &#39;T -&gt; Option&lt;&#39;T&gt;</div>
<div class="tip" id="fs13">val v : &#39;h</div>
<div class="tip" id="fs14">member ImperativeBuilder.Delay : f:(unit -&gt; Imperative&lt;&#39;g&gt;) -&gt; Imperative&lt;&#39;g&gt;<br /><br />Full name: Computation-zoo-padl.ImperativeBuilder.Delay</div>
<div class="tip" id="fs15">val f : (unit -&gt; Imperative&lt;&#39;g&gt;)</div>
<div class="tip" id="fs16">member ImperativeBuilder.Return : v:&#39;f -&gt; Imperative&lt;&#39;f&gt;<br /><br />Full name: Computation-zoo-padl.ImperativeBuilder.Return</div>
<div class="tip" id="fs17">val v : &#39;f</div>
<div class="tip" id="fs18">member ImperativeBuilder.Zero : unit -&gt; (unit -&gt; &#39;e option)<br /><br />Full name: Computation-zoo-padl.ImperativeBuilder.Zero</div>
<div class="tip" id="fs19">union case Option.None: Option&lt;&#39;T&gt;</div>
<div class="tip" id="fs20">member ImperativeBuilder.Run : imp:(unit -&gt; &#39;d option) -&gt; &#39;d<br /><br />Full name: Computation-zoo-padl.ImperativeBuilder.Run</div>
<div class="tip" id="fs21">val imp : (unit -&gt; &#39;d option)</div>
<div class="tip" id="fs22">val v : &#39;d</div>
<div class="tip" id="fs23">val failwith : message:string -&gt; &#39;T<br /><br />Full name: Microsoft.FSharp.Core.Operators.failwith</div>
<div class="tip" id="fs24">member ImperativeBuilder.For : inp:seq&lt;&#39;b&gt; * f:(&#39;b -&gt; unit -&gt; &#39;c option) -&gt; (unit -&gt; &#39;c option)<br /><br />Full name: Computation-zoo-padl.ImperativeBuilder.For</div>
<div class="tip" id="fs25">val inp : seq&lt;&#39;b&gt;</div>
<div class="tip" id="fs26">Multiple items<br />val seq : sequence:seq&lt;&#39;T&gt; -&gt; seq&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Core.Operators.seq<br /><br />--------------------<br />type seq&lt;&#39;T&gt; = IEnumerable&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Collections.seq&lt;_&gt;</div>
<div class="tip" id="fs27">val f : (&#39;b -&gt; unit -&gt; &#39;c option)</div>
<div class="tip" id="fs28">val loop : (IEnumerator&lt;&#39;b&gt; -&gt; unit -&gt; &#39;c option)</div>
<div class="tip" id="fs29">val en : IEnumerator&lt;&#39;b&gt;</div>
<div class="tip" id="fs30">type IEnumerator&lt;&#39;T&gt; =<br />&#160;&#160;member Current : &#39;T<br /><br />Full name: System.Collections.Generic.IEnumerator&lt;_&gt;</div>
<div class="tip" id="fs31">val not : value:bool -&gt; bool<br /><br />Full name: Microsoft.FSharp.Core.Operators.not</div>
<div class="tip" id="fs32">Collections.IEnumerator.MoveNext() : bool</div>
<div class="tip" id="fs33">member ImperativeBuilder.Zero : unit -&gt; (unit -&gt; &#39;e option)</div>
<div class="tip" id="fs34">member ImperativeBuilder.Combine : a:(unit -&gt; &#39;h option) * b:(unit -&gt; &#39;h option) -&gt; (unit -&gt; &#39;h option)</div>
<div class="tip" id="fs35">property IEnumerator.Current: &#39;b</div>
<div class="tip" id="fs36">member ImperativeBuilder.Delay : f:(unit -&gt; Imperative&lt;&#39;g&gt;) -&gt; Imperative&lt;&#39;g&gt;</div>
<div class="tip" id="fs37">IEnumerable.GetEnumerator() : IEnumerator&lt;&#39;b&gt;</div>
<div class="tip" id="fs38">member ImperativeBuilder.While : gd:(unit -&gt; bool) * body:(unit -&gt; &#39;a option) -&gt; (unit -&gt; &#39;a option)<br /><br />Full name: Computation-zoo-padl.ImperativeBuilder.While</div>
<div class="tip" id="fs39">val gd : (unit -&gt; bool)</div>
<div class="tip" id="fs40">val body : (unit -&gt; &#39;a option)</div>
<div class="tip" id="fs41">val loop : (unit -&gt; unit -&gt; &#39;a option)</div>
<div class="tip" id="fs42">val imperative : ImperativeBuilder<br /><br />Full name: Computation-zoo-padl.imperative</div>
<div class="tip" id="fs43">val blockedWords : string list<br /><br />Full name: Computation-zoo-padl.blockedWords</div>
<div class="tip" id="fs44">val duplicate : input:seq&lt;&#39;a&gt; -&gt; seq&lt;&#39;a&gt;<br /><br />Full name: Computation-zoo-padl.duplicate</div>
<div class="tip" id="fs45">val input : seq&lt;&#39;a&gt;</div>
<div class="tip" id="fs46">val num : &#39;a</div>
<div class="tip" id="fs47">val left : in1:seq&lt;&#39;a&gt; -&gt; in2:seq&lt;&#39;a&gt; -&gt; f:(&#39;a -&gt; #seq&lt;&#39;c&gt;) -&gt; seq&lt;&#39;c&gt;<br /><br />Full name: Computation-zoo-padl.left</div>
<div class="tip" id="fs48">val in1 : seq&lt;&#39;a&gt;</div>
<div class="tip" id="fs49">val in2 : seq&lt;&#39;a&gt;</div>
<div class="tip" id="fs50">val f : (&#39;a -&gt; #seq&lt;&#39;c&gt;)</div>
<div class="tip" id="fs51">val x : &#39;a</div>
<div class="tip" id="fs52">val right : in1:seq&lt;&#39;a&gt; -&gt; in2:seq&lt;&#39;a&gt; -&gt; f:(&#39;a -&gt; #seq&lt;&#39;c&gt;) -&gt; seq&lt;&#39;c&gt;<br /><br />Full name: Computation-zoo-padl.right</div>
<div class="tip" id="fs53">val blockMessage : msg:string -&gt; bool<br /><br />Full name: Computation-zoo-padl.blockMessage</div>
<div class="tip" id="fs54">val msg : string</div>
<div class="tip" id="fs55">Multiple items<br />val string : value:&#39;T -&gt; string<br /><br />Full name: Microsoft.FSharp.Core.Operators.string<br /><br />--------------------<br />type string = String<br /><br />Full name: Microsoft.FSharp.Core.string</div>
<div class="tip" id="fs56">val word : string</div>
<div class="tip" id="fs57">String.Contains(value: string) : bool</div>
<div class="tip" id="fs58">val left : int<br /><br />Full name: Computation-zoo-padl.left</div>
<div class="tip" id="fs59">val printfn : format:Printf.TextWriterFormat&lt;&#39;T&gt; -&gt; &#39;T<br /><br />Full name: Microsoft.FSharp.Core.ExtraTopLevelOperators.printfn</div>
<div class="tip" id="fs60">val right : int<br /><br />Full name: Computation-zoo-padl.right</div>
