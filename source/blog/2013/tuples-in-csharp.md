How many tuple types are there in C#?
=====================================

 - date: 2013-09-17T14:11:57.7562922+01:00
 - description: In a recent StackOverflow question, the poster asked about the choice between a function that takes a tuple and a function that uses the curried form. In this article I look at the problem from the C# perspective.
 - layout: article
 - tags: c#,f#,functional programming
 - title: How many tuple types are there in C#?
 - url: 2013/tuples-in-csharp
 - rawbody: true

--------------------------------------------------------------------------------
<p>In a <a href="http://stackoverflow.com/questions/18718232/when-should-i-write-my-functions-in-curried-form/18721711">recent StackOverflow question</a>
the poster asked about the difference between <em>tupled</em> and <em>curried</em> form of a function in F#.
In F#, you can use pattern matching to easily define a function that takes a tuple as an argument.
For example, the poster's function was a simple calculation that multiplies the number
of units sold <em>n</em> by the price <em>p</em>:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="f">salesTuple</span> (<span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="i">price</span>, <span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="i">count</span>) <span class="o">=</span> <span onmouseout="hideTip(event, 'fs2', 4)" onmouseover="showTip(event, 'fs2', 4)" class="i">price</span> <span class="o">*</span> (<span onmouseout="hideTip(event, 'fs4', 5)" onmouseover="showTip(event, 'fs4', 5)" class="f">float</span> <span onmouseout="hideTip(event, 'fs3', 6)" onmouseover="showTip(event, 'fs3', 6)" class="i">count</span>)
</code></pre></td>
</tr>
</table>
<p>The function takes a single argument of type <code>Tuple&lt;float, int&gt;</code> (or, using the nicer F# notation
<code>float * int</code>) and immediately decomposes it into two variables, <code>price</code> and <code>count</code>. The other
alternative is to write a function in the <em>curried</em> form:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs5', 7)" onmouseover="showTip(event, 'fs5', 7)" class="f">salesCurried</span> <span onmouseout="hideTip(event, 'fs2', 8)" onmouseover="showTip(event, 'fs2', 8)" class="i">price</span> <span onmouseout="hideTip(event, 'fs3', 9)" onmouseover="showTip(event, 'fs3', 9)" class="i">count</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs2', 10)" onmouseover="showTip(event, 'fs2', 10)" class="i">price</span> <span class="o">*</span> (<span onmouseout="hideTip(event, 'fs4', 11)" onmouseover="showTip(event, 'fs4', 11)" class="f">float</span> <span onmouseout="hideTip(event, 'fs3', 12)" onmouseover="showTip(event, 'fs3', 12)" class="i">count</span>)
</code></pre></td>
</tr>
</table>
<p>Here, we get a function of type <code>float -&gt; int -&gt; float</code>. Usually, you can read this just as a
function that takes <code>float</code> and <code>int</code> and returns <code>float</code>. However, you can also use <em>partial
function application</em> and call the function with just a single argument - if the price of
an apple is $1.20, we can write <code>salesCurried 1.20</code> to get a <em>new</em> function that takes just
<code>int</code> and gives us the price of specified number of apples. The poster's question was:</p>
<blockquote>
<p>So when I want to implement a function that would have taken <em>n &gt; 1</em> arguments,
should I for example always use a curried function in F# (...)? Or should I take
the simple route and use regular function with an n-tuple and curry later on
if necessary?</p>
</blockquote>
<p>You can see <a href="http://stackoverflow.com/questions/18718232/when-should-i-write-my-functions-in-curried-form/18721711#18721711">my answer on StackOverflow</a>.
The point of this short introduction was that the question inspired me to think about how
the world looks from the C# perspective...</p>


<div class="tip" id="fs1">val salesTuple : price:float * count:int -&gt; float<br /><br />Full name: Tuples-in-csharp.salesTuple</div>
<div class="tip" id="fs2">val price : float</div>
<div class="tip" id="fs3">val count : int</div>
<div class="tip" id="fs4">Multiple items<br />val float : value:&#39;T -&gt; float (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.float<br /><br />--------------------<br />type float = System.Double<br /><br />Full name: Microsoft.FSharp.Core.float<br /><br />--------------------<br />type float&lt;&#39;Measure&gt; = float<br /><br />Full name: Microsoft.FSharp.Core.float&lt;_&gt;</div>
<div class="tip" id="fs5">val salesCurried : price:float -&gt; count:int -&gt; float<br /><br />Full name: Tuples-in-csharp.salesCurried</div>
--------------------------------------------------------------------------------
<h1>How many tuple types are there in C#?</h1>
<p>In a <a href="http://stackoverflow.com/questions/18718232/when-should-i-write-my-functions-in-curried-form/18721711">recent StackOverflow question</a>
the poster asked about the difference between <em>tupled</em> and <em>curried</em> form of a function in F#.
In F#, you can use pattern matching to easily define a function that takes a tuple as an argument.
For example, the poster's function was a simple calculation that multiplies the number
of units sold <em>n</em> by the price <em>p</em>:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="f">salesTuple</span> (<span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="i">price</span>, <span onmouseout="hideTip(event, 'fs4', 4)" onmouseover="showTip(event, 'fs4', 4)" class="i">count</span>) <span class="o">=</span> <span onmouseout="hideTip(event, 'fs3', 5)" onmouseover="showTip(event, 'fs3', 5)" class="i">price</span> <span class="o">*</span> (<span onmouseout="hideTip(event, 'fs5', 6)" onmouseover="showTip(event, 'fs5', 6)" class="f">float</span> <span onmouseout="hideTip(event, 'fs4', 7)" onmouseover="showTip(event, 'fs4', 7)" class="i">count</span>)
</code></pre></td>
</tr>
</table>
<p>The function takes a single argument of type <code>Tuple&lt;float, int&gt;</code> (or, using the nicer F# notation
<code>float * int</code>) and immediately decomposes it into two variables, <code>price</code> and <code>count</code>. The other
alternative is to write a function in the <em>curried</em> form:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs6', 8)" onmouseover="showTip(event, 'fs6', 8)" class="f">salesCurried</span> <span onmouseout="hideTip(event, 'fs3', 9)" onmouseover="showTip(event, 'fs3', 9)" class="i">price</span> <span onmouseout="hideTip(event, 'fs4', 10)" onmouseover="showTip(event, 'fs4', 10)" class="i">count</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs3', 11)" onmouseover="showTip(event, 'fs3', 11)" class="i">price</span> <span class="o">*</span> (<span onmouseout="hideTip(event, 'fs5', 12)" onmouseover="showTip(event, 'fs5', 12)" class="f">float</span> <span onmouseout="hideTip(event, 'fs4', 13)" onmouseover="showTip(event, 'fs4', 13)" class="i">count</span>)
</code></pre></td>
</tr>
</table>
<p>Here, we get a function of type <code>float -&gt; int -&gt; float</code>. Usually, you can read this just as a
function that takes <code>float</code> and <code>int</code> and returns <code>float</code>. However, you can also use <em>partial
function application</em> and call the function with just a single argument - if the price of
an apple is $1.20, we can write <code>salesCurried 1.20</code> to get a <em>new</em> function that takes just
<code>int</code> and gives us the price of specified number of apples. The poster's question was:</p>
<blockquote>
<p>So when I want to implement a function that would have taken <em>n &gt; 1</em> arguments,
should I for example always use a curried function in F# (...)? Or should I take
the simple route and use regular function with an n-tuple and curry later on
if necessary?</p>
</blockquote>
<p>You can see <a href="http://stackoverflow.com/questions/18718232/when-should-i-write-my-functions-in-curried-form/18721711#18721711">my answer on StackOverflow</a>.
The point of this short introduction was that the question inspired me to think about how
the world looks from the C# perspective...</p>
<h2>To curry or not to curry?</h2>
<p>I will not repeat the whole answer in the blog post. The key idea is that you should use
tuple when the tuple has some <em>logical meaning</em>. For example, if you have a function that
takes a range or 2D coordinates, it makes sense to use <code>float * float</code>.</p>
<p>This makes sense because you can then nicely compose multiple functions that work with
ranges. For example, let's say we have a function <code>normalizeRange</code> and <code>expandRange</code>:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs7', 14)" onmouseover="showTip(event, 'fs7', 14)" class="f">normalizeRange</span> (<span onmouseout="hideTip(event, 'fs8', 15)" onmouseover="showTip(event, 'fs8', 15)" class="i">lo</span>, <span onmouseout="hideTip(event, 'fs9', 16)" onmouseover="showTip(event, 'fs9', 16)" class="i">hi</span>) <span class="o">=</span>
  <span class="k">if</span> <span onmouseout="hideTip(event, 'fs8', 17)" onmouseover="showTip(event, 'fs8', 17)" class="i">lo</span> <span class="o">&gt;</span> <span onmouseout="hideTip(event, 'fs9', 18)" onmouseover="showTip(event, 'fs9', 18)" class="i">hi</span> <span class="k">then</span> (<span onmouseout="hideTip(event, 'fs9', 19)" onmouseover="showTip(event, 'fs9', 19)" class="i">hi</span>, <span onmouseout="hideTip(event, 'fs8', 20)" onmouseover="showTip(event, 'fs8', 20)" class="i">lo</span>) <span class="k">else</span> (<span onmouseout="hideTip(event, 'fs8', 21)" onmouseover="showTip(event, 'fs8', 21)" class="i">lo</span>, <span onmouseout="hideTip(event, 'fs9', 22)" onmouseover="showTip(event, 'fs9', 22)" class="i">hi</span>)

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs10', 23)" onmouseover="showTip(event, 'fs10', 23)" class="f">expandRange</span> <span onmouseout="hideTip(event, 'fs11', 24)" onmouseover="showTip(event, 'fs11', 24)" class="i">offset</span> (<span onmouseout="hideTip(event, 'fs12', 25)" onmouseover="showTip(event, 'fs12', 25)" class="i">lo</span>, <span onmouseout="hideTip(event, 'fs13', 26)" onmouseover="showTip(event, 'fs13', 26)" class="i">hi</span>) <span class="o">=</span>
  (<span onmouseout="hideTip(event, 'fs12', 27)" onmouseover="showTip(event, 'fs12', 27)" class="i">lo</span> <span class="o">-</span> <span onmouseout="hideTip(event, 'fs11', 28)" onmouseover="showTip(event, 'fs11', 28)" class="i">offset</span>, <span onmouseout="hideTip(event, 'fs13', 29)" onmouseover="showTip(event, 'fs13', 29)" class="i">hi</span> <span class="o">+</span> <span onmouseout="hideTip(event, 'fs11', 30)" onmouseover="showTip(event, 'fs11', 30)" class="i">offset</span>)
</code></pre></td>
</tr>
</table>
<p>Now we can easily write code that takes some range, normalizes it and expands it by 10:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span onmouseout="hideTip(event, 'fs10', 31)" onmouseover="showTip(event, 'fs10', 31)" class="f">expandRange</span> <span class="n">10</span> (<span onmouseout="hideTip(event, 'fs7', 32)" onmouseover="showTip(event, 'fs7', 32)" class="f">normalizeRange</span>(<span class="n">50</span>, <span class="n">30</span>))
<span class="fsi">val it : int * int = (20, 60)</span>
</code></pre></td>
</tr>
</table>
<p>So, if your tuple has some logical meaning, taking tuple as an argument leads to more
composable code and makes it easier to understand. On the other hand, if there is no
logical connection, it is better to use the curried form - this makes it possible to
use partial function application.</p>
<h2>How about tuples in C#?</h2>
<p>In C#, we can work with tuples using the <code>Tuple&lt;T1, T2, ...&gt;</code> family of types. This is
certainly possible, but it is not particularly convenient, because you need to write
the long type name repeatedly (you can use <code>var</code> inside method, but not in the method
declaration).</p>
<p>However, there is another place where tuples appear in C# - it is perfectly reasonable
to treat all .NET methods as functions that take a single tuple as the input and return
some other type as the result. This is how .NET methods look when you call them from
F#:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span onmouseout="hideTip(event, 'fs14', 33)" onmouseover="showTip(event, 'fs14', 33)" class="t">Math</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs15', 34)" onmouseover="showTip(event, 'fs15', 34)" class="f">Round</span>(<span class="n">4.5</span>, <span onmouseout="hideTip(event, 'fs16', 35)" onmouseover="showTip(event, 'fs16', 35)" class="t">MidpointRounding</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs17', 36)" onmouseover="showTip(event, 'fs17', 36)" class="i">ToEven</span>) 
</code></pre></td>
</tr>
</table>
<p>We do not usually think about this as a tuple - it is just a method call - but what if
C# had (in <a href="http://visualstudio.uservoice.com/forums/121579-visual-studio/suggestions/2405699-build-list-dictionary-and-tuple-into-the-language">some future version</a>)
syntactic support for tuples and let you write <code>(42, "Hello world")</code> to create a tuple
value of type <code>Tuple&lt;int, string&gt;</code>?</p>
<h2>How many tuple types are there in .NET?</h2>
<p>This inspired me to do a quick analysis of the standard .NET libraries to have a look
at the tuples that standard .NET methods take. How many of them follow the good practice
and take a tuple that actually means something? And how many of them should instead use
the curried form, because the tuple has no logical meaning?</p>
<p>Checking the logical meaning will be difficult, but we can see how many of the tuples
are used by more than one or two methods. If they are used in multiple places, it
likely means that they represent some common pattern or some common single-purpose
data structure.</p>
<p>This is pretty easy analysis to do using F# Interactive. Let's first look at all the types
in the current <code>AppDomain</code> (this uses assemblies that are loaded by default in F# - so
nothing fancy). We also only look at "mscorlib" and "System" assemblies:</p>
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
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">open</span> <span onmouseout="hideTip(event, 'fs1', 37)" onmouseover="showTip(event, 'fs1', 37)" class="i">System</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs1', 38)" onmouseover="showTip(event, 'fs1', 38)" class="i">System</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs18', 39)" onmouseover="showTip(event, 'fs18', 39)" class="i">Reflection</span>

<span class="c">// Get all types in currently loaded assemblies</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs19', 40)" onmouseover="showTip(event, 'fs19', 40)" class="i">types</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs20', 41)" onmouseover="showTip(event, 'fs20', 41)" class="i">seq</span> {
  <span class="k">for</span> <span onmouseout="hideTip(event, 'fs21', 42)" onmouseover="showTip(event, 'fs21', 42)" class="i">asm</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs22', 43)" onmouseover="showTip(event, 'fs22', 43)" class="t">AppDomain</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs23', 44)" onmouseover="showTip(event, 'fs23', 44)" class="i">CurrentDomain</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs24', 45)" onmouseover="showTip(event, 'fs24', 45)" class="f">GetAssemblies</span>() <span class="k">do</span>
    <span class="k">if</span> <span onmouseout="hideTip(event, 'fs21', 46)" onmouseover="showTip(event, 'fs21', 46)" class="i">asm</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs25', 47)" onmouseover="showTip(event, 'fs25', 47)" class="i">FullName</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs26', 48)" onmouseover="showTip(event, 'fs26', 48)" class="f">StartsWith</span>(<span class="s">&quot;System&quot;</span>) <span class="o">||</span> 
       <span onmouseout="hideTip(event, 'fs21', 49)" onmouseover="showTip(event, 'fs21', 49)" class="i">asm</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs25', 50)" onmouseover="showTip(event, 'fs25', 50)" class="i">FullName</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs26', 51)" onmouseover="showTip(event, 'fs26', 51)" class="f">StartsWith</span>(<span class="s">&quot;mscorlib&quot;</span>) <span class="k">then</span>
      <span class="k">yield!</span> <span onmouseout="hideTip(event, 'fs21', 52)" onmouseover="showTip(event, 'fs21', 52)" class="i">asm</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs27', 53)" onmouseover="showTip(event, 'fs27', 53)" class="f">GetTypes</span>() }

<span onmouseout="hideTip(event, 'fs19', 54)" onmouseover="showTip(event, 'fs19', 54)" class="i">types</span> <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs28', 55)" onmouseover="showTip(event, 'fs28', 55)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs29', 56)" onmouseover="showTip(event, 'fs29', 56)" class="f">length</span>
</code></pre></td>
</tr>
</table>
<p>The code is a simple <em>sequence expression</em> that iterates over all assemblies and
yields all types. On my machine, this gives us some 17000 types. Now, let's get a
list with all tuples - we'll iterate over all methods in each type and generate a
list with the names of parameter types. We skip all methods with less than 2 parameters:</p>
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
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs30', 57)" onmouseover="showTip(event, 'fs30', 57)" class="i">tuples</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs20', 58)" onmouseover="showTip(event, 'fs20', 58)" class="i">seq</span> { 
  <span class="k">for</span> <span onmouseout="hideTip(event, 'fs31', 59)" onmouseover="showTip(event, 'fs31', 59)" class="i">typ</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs19', 60)" onmouseover="showTip(event, 'fs19', 60)" class="i">types</span> <span class="k">do</span>
    <span class="c">// Get declared, public, both instance and static methods</span>
    <span class="k">let</span> <span onmouseout="hideTip(event, 'fs32', 61)" onmouseover="showTip(event, 'fs32', 61)" class="i">flags</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs33', 62)" onmouseover="showTip(event, 'fs33', 62)" class="t">BindingFlags</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs34', 63)" onmouseover="showTip(event, 'fs34', 63)" class="i">DeclaredOnly</span> <span class="o">|||</span> <span onmouseout="hideTip(event, 'fs33', 64)" onmouseover="showTip(event, 'fs33', 64)" class="t">BindingFlags</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs35', 65)" onmouseover="showTip(event, 'fs35', 65)" class="i">Public</span> <span class="o">|||</span>
                <span onmouseout="hideTip(event, 'fs33', 66)" onmouseover="showTip(event, 'fs33', 66)" class="t">BindingFlags</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs36', 67)" onmouseover="showTip(event, 'fs36', 67)" class="i">Static</span> <span class="o">|||</span> <span onmouseout="hideTip(event, 'fs33', 68)" onmouseover="showTip(event, 'fs33', 68)" class="t">BindingFlags</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs37', 69)" onmouseover="showTip(event, 'fs37', 69)" class="i">Instance</span>
    <span class="k">let</span> <span onmouseout="hideTip(event, 'fs38', 70)" onmouseover="showTip(event, 'fs38', 70)" class="i">methods</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs31', 71)" onmouseover="showTip(event, 'fs31', 71)" class="i">typ</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs39', 72)" onmouseover="showTip(event, 'fs39', 72)" class="f">GetMethods</span>(<span onmouseout="hideTip(event, 'fs32', 73)" onmouseover="showTip(event, 'fs32', 73)" class="i">flags</span>)
    <span class="c">// Generate tuples with parameters types for each method</span>
    <span class="k">for</span> <span onmouseout="hideTip(event, 'fs40', 74)" onmouseover="showTip(event, 'fs40', 74)" class="i">meth</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs38', 75)" onmouseover="showTip(event, 'fs38', 75)" class="i">methods</span> <span class="k">do</span>
      <span class="k">let</span> <span onmouseout="hideTip(event, 'fs41', 76)" onmouseover="showTip(event, 'fs41', 76)" class="i">pars</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs40', 77)" onmouseover="showTip(event, 'fs40', 77)" class="i">meth</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs42', 78)" onmouseover="showTip(event, 'fs42', 78)" class="f">GetParameters</span>()
      <span class="k">if</span> <span onmouseout="hideTip(event, 'fs41', 79)" onmouseover="showTip(event, 'fs41', 79)" class="i">pars</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs43', 80)" onmouseover="showTip(event, 'fs43', 80)" class="i">Length</span> <span class="o">&gt;</span> <span class="n">1</span> <span class="k">then</span>
        <span class="k">yield</span> [ <span class="k">for</span> <span onmouseout="hideTip(event, 'fs44', 81)" onmouseover="showTip(event, 'fs44', 81)" class="i">p</span> <span class="k">in</span> <span onmouseout="hideTip(event, 'fs40', 82)" onmouseover="showTip(event, 'fs40', 82)" class="i">meth</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs42', 83)" onmouseover="showTip(event, 'fs42', 83)" class="f">GetParameters</span>() <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs44', 84)" onmouseover="showTip(event, 'fs44', 84)" class="i">p</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs45', 85)" onmouseover="showTip(event, 'fs45', 85)" class="i">ParameterType</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs46', 86)" onmouseover="showTip(event, 'fs46', 86)" class="i">FullName</span> ] }

<span onmouseout="hideTip(event, 'fs30', 87)" onmouseover="showTip(event, 'fs30', 87)" class="i">tuples</span> <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs28', 88)" onmouseover="showTip(event, 'fs28', 88)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs29', 89)" onmouseover="showTip(event, 'fs29', 89)" class="f">length</span>
</code></pre></td>
</tr>
</table>
<p>So, on my machine there are 16463 methods in .NET that take some tuple as an argument.
Now, the question is, how many of them are used repeatedly? We can easily group the
tuples by the list of strings (F# implements structural comparison, so this is easy to do),
calculate the counts for each group and sort the results:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
<span class="l">7: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs47', 90)" onmouseover="showTip(event, 'fs47', 90)" class="i">counts</span> <span class="o">=</span>
  <span onmouseout="hideTip(event, 'fs30', 91)" onmouseover="showTip(event, 'fs30', 91)" class="i">tuples</span> 
  <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs28', 92)" onmouseover="showTip(event, 'fs28', 92)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs48', 93)" onmouseover="showTip(event, 'fs48', 93)" class="f">groupBy</span> <span onmouseout="hideTip(event, 'fs49', 94)" onmouseover="showTip(event, 'fs49', 94)" class="f">id</span>
  <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs50', 95)" onmouseover="showTip(event, 'fs50', 95)" class="t">Array</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs51', 96)" onmouseover="showTip(event, 'fs51', 96)" class="f">ofSeq</span>
  <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs50', 97)" onmouseover="showTip(event, 'fs50', 97)" class="t">Array</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs52', 98)" onmouseover="showTip(event, 'fs52', 98)" class="f">map</span> (<span class="k">fun</span> (<span onmouseout="hideTip(event, 'fs53', 99)" onmouseover="showTip(event, 'fs53', 99)" class="i">k</span>, <span onmouseout="hideTip(event, 'fs54', 100)" onmouseover="showTip(event, 'fs54', 100)" class="i">vs</span>) <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs53', 101)" onmouseover="showTip(event, 'fs53', 101)" class="i">k</span>, <span onmouseout="hideTip(event, 'fs28', 102)" onmouseover="showTip(event, 'fs28', 102)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs29', 103)" onmouseover="showTip(event, 'fs29', 103)" class="f">length</span> <span onmouseout="hideTip(event, 'fs54', 104)" onmouseover="showTip(event, 'fs54', 104)" class="i">vs</span>)
  <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs50', 105)" onmouseover="showTip(event, 'fs50', 105)" class="t">Array</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs55', 106)" onmouseover="showTip(event, 'fs55', 106)" class="f">sortBy</span> <span onmouseout="hideTip(event, 'fs56', 107)" onmouseover="showTip(event, 'fs56', 107)" class="f">snd</span>
  <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs50', 108)" onmouseover="showTip(event, 'fs50', 108)" class="t">Array</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs57', 109)" onmouseover="showTip(event, 'fs57', 109)" class="f">rev</span>
</code></pre></td>
</tr>
</table>
<h2>Most common tuples in .NET</h2>
<p>If we run <code>Seq.length counts</code>, we get 5805 as the result. This means that there are 5 thousand
distinct tuples (among roughly 15 thousand different methods). That certainly does not look like
most of them have some logical connection. But some of the top ones certainly do - here are the
top 8 (ignoring generics) with their counts:</p>
<ol>
<li>
<code>string * string</code> (714) - looks like many methods take two strings - not sure if there
is any logical meaning, but there probably are a few common uses
</li>
<li>
<code>byte[] * int * int</code> (341) - this one looks like an array with offset and length - clearly
this is a nice tuple with logical meaning
</li>
<li><code>int * int</code> (327) - similar to two strings</li>
<li><code>object * object</code> (180) - hmm, maybe .NET likes untyped API :-)</li>
<li>
<code>int * object</code> (165) - I was a bit puzzled by this one, so I checked the methods that
use this type. Good old untyped collections from the .NET 1.0 days!
</li>
<li><code>char[] * int * int</code> (159) - similarly to the number 2, another nice logical tuple!</li>
<li><code>string * string * string</code> (156) - wow, so many methods take 3 strings</li>
<li><code>ITypeDescriptorContext * Type</code> (152) - huh??</li>
</ol>
<h2>How many are actually useful?</h2>
<p>It looks like there is quite a few tuple types that actually mean something useful. But what
is the distribution? Let's use <a href="http://fsharp.github.io/FSharp.Charting/">the FSharp.Charting</a>
library to draw a quick chart that draws a column chart plotting the counts for every single
of the 5000 tuple types:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="prep">#load</span> <span class="s">&quot;..\packages\FSharp.Charting.0.84\FSharp.Charting.fsx&quot;</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs58', 110)" onmouseover="showTip(event, 'fs58', 110)" class="i">FSharp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs59', 111)" onmouseover="showTip(event, 'fs59', 111)" class="i">Charting</span>

<span onmouseout="hideTip(event, 'fs60', 112)" onmouseover="showTip(event, 'fs60', 112)" class="t">Chart</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs61', 113)" onmouseover="showTip(event, 'fs61', 113)" class="f">Column</span>(<span onmouseout="hideTip(event, 'fs28', 114)" onmouseover="showTip(event, 'fs28', 114)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs62', 115)" onmouseover="showTip(event, 'fs62', 115)" class="f">map</span> <span onmouseout="hideTip(event, 'fs56', 116)" onmouseover="showTip(event, 'fs56', 116)" class="f">snd</span> <span onmouseout="hideTip(event, 'fs47', 117)" onmouseover="showTip(event, 'fs47', 117)" class="i">counts</span>)<span class="o">.</span><span class="f">WithYAxis</span>(<span class="i">Log</span><span class="o">=</span><span class="k">true</span>)
</code></pre></td>
</tr>
</table>
<p>If you create a chart using just <code>Chart.Column</code>, then you will not see very much - the number
of counts drops very quickly from the high numbers that we've seen for the first 10 types.
But if we make the Y scale logarithmic (a good way to create misleading charts!) then we
can actually see something:</p>
<div style="text-align:center;margin:0px 0px 10px 0px">
<img src="chart.png" alt="How many times are common tuples used in .NET?" />
</div>
<p>The cart shows that a vast majority of tuples are used less than 10 times and only 2000
(of some 5000) are used more than once. The analysis based on just the number of occurrences
is definitely not precise, but let's say that tuples which are used more than 10 times
are useful and those that are used more than 3 times are possibly useful. We can then
easily draw a chart showing the proportions:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span onmouseout="hideTip(event, 'fs47', 118)" onmouseover="showTip(event, 'fs47', 118)" class="i">counts</span> 
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs28', 119)" onmouseover="showTip(event, 'fs28', 119)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs63', 120)" onmouseover="showTip(event, 'fs63', 120)" class="f">countBy</span> (<span class="k">fun</span> (<span onmouseout="hideTip(event, 'fs53', 121)" onmouseover="showTip(event, 'fs53', 121)" class="i">k</span>, <span onmouseout="hideTip(event, 'fs64', 122)" onmouseover="showTip(event, 'fs64', 122)" class="i">v</span>) <span class="k">-&gt;</span> 
    <span class="k">if</span> <span onmouseout="hideTip(event, 'fs64', 123)" onmouseover="showTip(event, 'fs64', 123)" class="i">v</span> <span class="o">&lt;=</span> <span class="n">2</span> <span class="k">then</span> <span class="s">&quot;Useless&quot;</span>
    <span class="k">elif</span> <span onmouseout="hideTip(event, 'fs64', 124)" onmouseover="showTip(event, 'fs64', 124)" class="i">v</span> <span class="o">&lt;=</span> <span class="n">10</span> <span class="k">then</span> <span class="s">&quot;Maybe useful&quot;</span>
    <span class="k">else</span> <span class="s">&quot;Useful&quot;</span>)
<span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs60', 125)" onmouseover="showTip(event, 'fs60', 125)" class="t">Chart</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs65', 126)" onmouseover="showTip(event, 'fs65', 126)" class="f">Doughnut</span>
</code></pre></td>
</tr>
</table>
<p>This snippet gives us the following nice chart (I tweaked the look a bit - a nice feature
of F# chart is that you can use <code>Ctrl+G</code> to open a property grid and change the fonts
rather than doing everything from code):</p>
<div style="text-align:center;margin:0px 0px 10px 0px">
<img src="chart2.png" alt="Proportion of useful tuple types in .NET" />
</div>
<h2>Surely, this is ridiculous!</h2>
<p>Yes, I can hear that. I'm comparing incomparable here - it does not make sense to look at
.NET libraries as if they were F# libraries and then claim that they are poorly designed.
The new version of my blog does not even have comments, but you can still <a href="https://twitter.com/tomaspetricek">argue with me
on Twitter</a>.</p>
<p>But before doing that - I'm not trying to criticise the design of .NET libraries in any way.
If your only option is to define a method that takes parameters "as a tuple" then that's
the way to go. I'm certainly not suggesting that .NET should use curried form using
<code>Func&lt;T1, ...&gt;</code> delegates or that people should use <code>Tuple&lt;T1, ...&gt;</code> instead of ordinary
methods.</p>
<p>This article is merely a thought experiment with some interesting analysis of .NET types.
We can see that there are a few "natural tuples" in .NET library design (like
<code>byte[] * int * int</code>) but the parameters of a majority of methods do not logically form
a tuple.</p>
<p>So, is it better to use languages that make a clear distinction between (curried) functions
and functions taking a tuple? I think so - it makes it easier to write composable code
(by writing functions that take and return simple "ad-hoc" types as tuples) and it gives
you an easy way of grouping related types. There is no class representing <em>array range</em> in
.NET because adding an entire class for this would be over-kill. A simple type like tuple
(supported by the language) makes this perfectly possible. On the other hand, you need to
think more carefully about library design to make sure that you use tuples correctly.</p>


<div class="tip" id="fs1">namespace System</div>
<div class="tip" id="fs2">val salesTuple : price:float * count:int -&gt; float<br /><br />Full name: Tuples-in-csharp.salesTuple</div>
<div class="tip" id="fs3">val price : float</div>
<div class="tip" id="fs4">val count : int</div>
<div class="tip" id="fs5">Multiple items<br />val float : value:&#39;T -&gt; float (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.float<br /><br />--------------------<br />type float = Double<br /><br />Full name: Microsoft.FSharp.Core.float<br /><br />--------------------<br />type float&lt;&#39;Measure&gt; = float<br /><br />Full name: Microsoft.FSharp.Core.float&lt;_&gt;</div>
<div class="tip" id="fs6">val salesCurried : price:float -&gt; count:int -&gt; float<br /><br />Full name: Tuples-in-csharp.salesCurried</div>
<div class="tip" id="fs7">val normalizeRange : lo:&#39;a * hi:&#39;a -&gt; &#39;a * &#39;a (requires comparison)<br /><br />Full name: Tuples-in-csharp.normalizeRange</div>
<div class="tip" id="fs8">val lo : &#39;a (requires comparison)</div>
<div class="tip" id="fs9">val hi : &#39;a (requires comparison)</div>
<div class="tip" id="fs10">val expandRange : offset:int -&gt; lo:int * hi:int -&gt; int * int<br /><br />Full name: Tuples-in-csharp.expandRange</div>
<div class="tip" id="fs11">val offset : int</div>
<div class="tip" id="fs12">val lo : int</div>
<div class="tip" id="fs13">val hi : int</div>
<div class="tip" id="fs14">type Math =<br />&#160;&#160;static val PI : float<br />&#160;&#160;static val E : float<br />&#160;&#160;static member Abs : value:sbyte -&gt; sbyte + 6 overloads<br />&#160;&#160;static member Acos : d:float -&gt; float<br />&#160;&#160;static member Asin : d:float -&gt; float<br />&#160;&#160;static member Atan : d:float -&gt; float<br />&#160;&#160;static member Atan2 : y:float * x:float -&gt; float<br />&#160;&#160;static member BigMul : a:int * b:int -&gt; int64<br />&#160;&#160;static member Ceiling : d:decimal -&gt; decimal + 1 overload<br />&#160;&#160;static member Cos : d:float -&gt; float<br />&#160;&#160;...<br /><br />Full name: System.Math</div>
<div class="tip" id="fs15">Math.Round(d: decimal) : decimal<br />Math.Round(a: float) : float<br />Math.Round(d: decimal, mode: MidpointRounding) : decimal<br />Math.Round(d: decimal, decimals: int) : decimal<br />Math.Round(value: float, mode: MidpointRounding) : float<br />Math.Round(value: float, digits: int) : float<br />Math.Round(d: decimal, decimals: int, mode: MidpointRounding) : decimal<br />Math.Round(value: float, digits: int, mode: MidpointRounding) : float</div>
<div class="tip" id="fs16">type MidpointRounding =<br />&#160;&#160;| ToEven = 0<br />&#160;&#160;| AwayFromZero = 1<br /><br />Full name: System.MidpointRounding</div>
<div class="tip" id="fs17">field MidpointRounding.ToEven = 0</div>
<div class="tip" id="fs18">namespace System.Reflection</div>
<div class="tip" id="fs19">val types : seq&lt;Type&gt;<br /><br />Full name: Tuples-in-csharp.types</div>
<div class="tip" id="fs20">Multiple items<br />val seq : sequence:seq&lt;&#39;T&gt; -&gt; seq&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Core.Operators.seq<br /><br />--------------------<br />type seq&lt;&#39;T&gt; = Collections.Generic.IEnumerable&lt;&#39;T&gt;<br /><br />Full name: Microsoft.FSharp.Collections.seq&lt;_&gt;</div>
<div class="tip" id="fs21">val asm : Assembly</div>
<div class="tip" id="fs22">type AppDomain =<br />&#160;&#160;inherit MarshalByRefObject<br />&#160;&#160;member ActivationContext : ActivationContext<br />&#160;&#160;member AppendPrivatePath : path:string -&gt; unit<br />&#160;&#160;member ApplicationIdentity : ApplicationIdentity<br />&#160;&#160;member ApplicationTrust : ApplicationTrust<br />&#160;&#160;member ApplyPolicy : assemblyName:string -&gt; string<br />&#160;&#160;member BaseDirectory : string<br />&#160;&#160;member ClearPrivatePath : unit -&gt; unit<br />&#160;&#160;member ClearShadowCopyPath : unit -&gt; unit<br />&#160;&#160;member CreateComInstanceFrom : assemblyName:string * typeName:string -&gt; ObjectHandle + 1 overload<br />&#160;&#160;member CreateInstance : assemblyName:string * typeName:string -&gt; ObjectHandle + 3 overloads<br />&#160;&#160;...<br /><br />Full name: System.AppDomain</div>
<div class="tip" id="fs23">property AppDomain.CurrentDomain: AppDomain</div>
<div class="tip" id="fs24">AppDomain.GetAssemblies() : Assembly []</div>
<div class="tip" id="fs25">property Assembly.FullName: string</div>
<div class="tip" id="fs26">String.StartsWith(value: string) : bool<br />String.StartsWith(value: string, comparisonType: StringComparison) : bool<br />String.StartsWith(value: string, ignoreCase: bool, culture: Globalization.CultureInfo) : bool</div>
<div class="tip" id="fs27">Assembly.GetTypes() : Type []</div>
<div class="tip" id="fs28">module Seq<br /><br />from Microsoft.FSharp.Collections</div>
<div class="tip" id="fs29">val length : source:seq&lt;&#39;T&gt; -&gt; int<br /><br />Full name: Microsoft.FSharp.Collections.Seq.length</div>
<div class="tip" id="fs30">val tuples : seq&lt;string list&gt;<br /><br />Full name: Tuples-in-csharp.tuples</div>
<div class="tip" id="fs31">val typ : Type</div>
<div class="tip" id="fs32">val flags : BindingFlags</div>
<div class="tip" id="fs33">type BindingFlags =<br />&#160;&#160;| Default = 0<br />&#160;&#160;| IgnoreCase = 1<br />&#160;&#160;| DeclaredOnly = 2<br />&#160;&#160;| Instance = 4<br />&#160;&#160;| Static = 8<br />&#160;&#160;| Public = 16<br />&#160;&#160;| NonPublic = 32<br />&#160;&#160;| FlattenHierarchy = 64<br />&#160;&#160;| InvokeMethod = 256<br />&#160;&#160;| CreateInstance = 512<br />&#160;&#160;...<br /><br />Full name: System.Reflection.BindingFlags</div>
<div class="tip" id="fs34">field BindingFlags.DeclaredOnly = 2</div>
<div class="tip" id="fs35">field BindingFlags.Public = 16</div>
<div class="tip" id="fs36">field BindingFlags.Static = 8</div>
<div class="tip" id="fs37">field BindingFlags.Instance = 4</div>
<div class="tip" id="fs38">val methods : MethodInfo []</div>
<div class="tip" id="fs39">Type.GetMethods() : MethodInfo []<br />Type.GetMethods(bindingAttr: BindingFlags) : MethodInfo []</div>
<div class="tip" id="fs40">val meth : MethodInfo</div>
<div class="tip" id="fs41">val pars : ParameterInfo []</div>
<div class="tip" id="fs42">MethodBase.GetParameters() : ParameterInfo []</div>
<div class="tip" id="fs43">property Array.Length: int</div>
<div class="tip" id="fs44">val p : ParameterInfo</div>
<div class="tip" id="fs45">property ParameterInfo.ParameterType: Type</div>
<div class="tip" id="fs46">property Type.FullName: string</div>
<div class="tip" id="fs47">val counts : (string list * int) []<br /><br />Full name: Tuples-in-csharp.counts</div>
<div class="tip" id="fs48">val groupBy : projection:(&#39;T -&gt; &#39;Key) -&gt; source:seq&lt;&#39;T&gt; -&gt; seq&lt;&#39;Key * seq&lt;&#39;T&gt;&gt; (requires equality)<br /><br />Full name: Microsoft.FSharp.Collections.Seq.groupBy</div>
<div class="tip" id="fs49">val id : x:&#39;T -&gt; &#39;T<br /><br />Full name: Microsoft.FSharp.Core.Operators.id</div>
<div class="tip" id="fs50">type Array =<br />&#160;&#160;member Clone : unit -&gt; obj<br />&#160;&#160;member CopyTo : array:Array * index:int -&gt; unit + 1 overload<br />&#160;&#160;member GetEnumerator : unit -&gt; IEnumerator<br />&#160;&#160;member GetLength : dimension:int -&gt; int<br />&#160;&#160;member GetLongLength : dimension:int -&gt; int64<br />&#160;&#160;member GetLowerBound : dimension:int -&gt; int<br />&#160;&#160;member GetUpperBound : dimension:int -&gt; int<br />&#160;&#160;member GetValue : [&lt;ParamArray&gt;] indices:int[] -&gt; obj + 7 overloads<br />&#160;&#160;member Initialize : unit -&gt; unit<br />&#160;&#160;member IsFixedSize : bool<br />&#160;&#160;...<br /><br />Full name: System.Array</div>
<div class="tip" id="fs51">val ofSeq : source:seq&lt;&#39;T&gt; -&gt; &#39;T []<br /><br />Full name: Microsoft.FSharp.Collections.Array.ofSeq</div>
<div class="tip" id="fs52">val map : mapping:(&#39;T -&gt; &#39;U) -&gt; array:&#39;T [] -&gt; &#39;U []<br /><br />Full name: Microsoft.FSharp.Collections.Array.map</div>
<div class="tip" id="fs53">val k : string list</div>
<div class="tip" id="fs54">val vs : seq&lt;string list&gt;</div>
<div class="tip" id="fs55">val sortBy : projection:(&#39;T -&gt; &#39;Key) -&gt; array:&#39;T [] -&gt; &#39;T [] (requires comparison)<br /><br />Full name: Microsoft.FSharp.Collections.Array.sortBy</div>
<div class="tip" id="fs56">val snd : tuple:(&#39;T1 * &#39;T2) -&gt; &#39;T2<br /><br />Full name: Microsoft.FSharp.Core.Operators.snd</div>
<div class="tip" id="fs57">val rev : array:&#39;T [] -&gt; &#39;T []<br /><br />Full name: Microsoft.FSharp.Collections.Array.rev</div>
<div class="tip" id="fs58">Multiple items<br />namespace FSharp<br /><br />--------------------<br />namespace Microsoft.FSharp</div>
<div class="tip" id="fs59">namespace FSharp.Charting</div>
<div class="tip" id="fs60">type Chart =<br />&#160;&#160;static member Area : data:seq&lt;#value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Color * ?XTitle:string * ?YTitle:string -&gt; GenericChart<br />&#160;&#160;static member Area : data:seq&lt;#value * #value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Color * ?XTitle:string * ?YTitle:string -&gt; GenericChart<br />&#160;&#160;static member Bar : data:seq&lt;#value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Color * ?XTitle:string * ?YTitle:string -&gt; GenericChart<br />&#160;&#160;static member Bar : data:seq&lt;#value * #value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Color * ?XTitle:string * ?YTitle:string -&gt; GenericChart<br />&#160;&#160;static member BoxPlotFromData : data:seq&lt;#value * #seq&lt;&#39;a2&gt;&gt; * ?Name:string * ?Title:string * ?Color:Color * ?XTitle:string * ?YTitle:string * ?Percentile:int * ?ShowAverage:bool * ?ShowMedian:bool * ?ShowUnusualValues:bool * ?WhiskerPercentile:int -&gt; GenericChart (requires &#39;a2 :&gt; value)<br />&#160;&#160;static member BoxPlotFromStatistics : data:seq&lt;#value * #value * #value * #value * #value * #value * #value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Color * ?XTitle:string * ?YTitle:string * ?Percentile:int * ?ShowAverage:bool * ?ShowMedian:bool * ?ShowUnusualValues:bool * ?WhiskerPercentile:int -&gt; GenericChart<br />&#160;&#160;static member Bubble : data:seq&lt;#value * #value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Color * ?XTitle:string * ?YTitle:string * ?BubbleMaxSize:int * ?BubbleMinSize:int * ?BubbleScaleMax:float * ?BubbleScaleMin:float * ?UseSizeForLabel:bool -&gt; GenericChart<br />&#160;&#160;static member Bubble : data:seq&lt;#value * #value * #value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Color * ?XTitle:string * ?YTitle:string * ?BubbleMaxSize:int * ?BubbleMinSize:int * ?BubbleScaleMax:float * ?BubbleScaleMin:float * ?UseSizeForLabel:bool -&gt; GenericChart<br />&#160;&#160;static member Candlestick : data:seq&lt;#value * #value * #value * #value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Color * ?XTitle:string * ?YTitle:string -&gt; CandlestickChart<br />&#160;&#160;static member Candlestick : data:seq&lt;#value * #value * #value * #value * #value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Color * ?XTitle:string * ?YTitle:string -&gt; CandlestickChart<br />&#160;&#160;...<br /><br />Full name: FSharp.Charting.Chart</div>
<div class="tip" id="fs61">static member Chart.Column : data:seq&lt;#value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Drawing.Color * ?XTitle:string * ?YTitle:string -&gt; ChartTypes.GenericChart<br />static member Chart.Column : data:seq&lt;#value * #value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Drawing.Color * ?XTitle:string * ?YTitle:string -&gt; ChartTypes.GenericChart</div>
<div class="tip" id="fs62">val map : mapping:(&#39;T -&gt; &#39;U) -&gt; source:seq&lt;&#39;T&gt; -&gt; seq&lt;&#39;U&gt;<br /><br />Full name: Microsoft.FSharp.Collections.Seq.map</div>
<div class="tip" id="fs63">val countBy : projection:(&#39;T -&gt; &#39;Key) -&gt; source:seq&lt;&#39;T&gt; -&gt; seq&lt;&#39;Key * int&gt; (requires equality)<br /><br />Full name: Microsoft.FSharp.Collections.Seq.countBy</div>
<div class="tip" id="fs64">val v : int</div>
<div class="tip" id="fs65">static member Chart.Doughnut : data:seq&lt;#value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Drawing.Color * ?XTitle:string * ?YTitle:string -&gt; ChartTypes.DoughnutChart<br />static member Chart.Doughnut : data:seq&lt;#value * #value&gt; * ?Name:string * ?Title:string * ?Labels:#seq&lt;string&gt; * ?Color:Drawing.Color * ?XTitle:string * ?YTitle:string -&gt; ChartTypes.DoughnutChart</div>
