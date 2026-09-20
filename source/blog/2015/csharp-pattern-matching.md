Pattern matching in action using C# 6
=====================================

 - date: 2015-04-01T11:41:49.5611652+01:00
 - description: The new release of the C# language migth seem as a minor version, but in fact, it contains a hidden gem. In fact, it contains awesome support for pattern matching - but for some reason, this is concealed under the name 'exception filters'.
 - layout: article
 - image: http://tomasp.net/blog/2015/csharp-pattern-matching/hat.png
 - tags: c#,fun,functional programming
 - title: Pattern matching in action using C# 6
 - url: 2015/csharp-pattern-matching
 - format: bakedin

--------------------------------------------------------------------------------
<img src="http://tomasp.net/blog/2015/csharp-pattern-matching/hat.png" style="float:right;margin:20px" />
<p>On year ago, on this very day, <a href="http://tomasp.net/blog/2014/csharp-6-released/index.html">I wrote about the open-sourcing of C#
6.0</a>. Thanks to a very special
information leak, I learned about this about a week before Microsoft officially announced
it. However, my information were slightly incorrect - rather then releasing the <a href="https://github.com/Microsoft/visualfsharp/">much
improved version of the language</a>, Microsoft
continued working on language version internally called "Small C#", which is now available
as "C# 6" in the <a href="https://www.visualstudio.com/en-us/news/vs2015-vs.aspx">Visual Studio 2015 preview</a>.</p>
<p>It is my understanding that, with this release, Microsoft is secretly testing the reaction of the
developer audience to some of the amazing features that F# developers loved and used for
the last 7 years and that are coming to C# <em>very soon</em>. To avoid shock, these are however
carefuly hidden!</p>
<p>In this blog post, I'm going to show you <em>pattern matching</em> which is probably the most useful
hidden C# feature and its improvements in C# 6. For reasons that elude me, pattern matching in
C# 6 is called <em>exception filters</em> and has some unfortunate restrictions. But we can still
use it to write nice functional code!</p>


--------------------------------------------------------------------------------
<h1>Pattern matching in action using C# 6</h1>
<img src="http://tomasp.net/blog/2015/csharp-pattern-matching/hat.png" style="float:right;margin:20px" />
<p>On year ago, on this very day, <a href="http://tomasp.net/blog/2014/csharp-6-released/index.html">I wrote about the open-sourcing of C#
6.0</a>. Thanks to a very special
information leak, I learned about this about a week before Microsoft officially announced
it. However, my information were slightly incorrect - rather then releasing the <a href="https://github.com/Microsoft/visualfsharp/">much
improved version of the language</a>, Microsoft
continued working on language version internally called "Small C#", which is now available
as "C# 6" in the <a href="https://www.visualstudio.com/en-us/news/vs2015-vs.aspx">Visual Studio 2015 preview</a>.</p>
<p>It is my understanding that, with this release, Microsoft is secretly testing the reaction of the
developer audience to some of the amazing features that F# developers loved and used for
the last 7 years and that are coming to C# <em>very soon</em>. To avoid shock, these are however
carefuly hidden!</p>
<p>In this blog post, I'm going to show you <em>pattern matching</em> which is probably the most useful
hidden C# feature and its improvements in C# 6. For reasons that elude me, pattern matching in
C# 6 is called <em>exception filters</em> and has some unfortunate restrictions. But we can still
use it to write nice functional code!</p>
<blockquote>
<p><strong>UPDATE:</strong> In case you are reading this article later than on the day when it was
published, let me just point out that this was released on 1 April 2015. Keep that
in mind before putting the code in production. Have fun ;-).</p>
</blockquote>
<h2>Formatting simple math expressions with F#</h2>
<p>For easier understanding, I'll first introduce the idea of pattern matching using F#.
I'll use a fairly standard functional programming example - formatting and
evaluation of simple algebraic expression. We want to work with expressions like <span class="math">\(x * (1 + 2)\)</span>,
so we'll need variables, constants, addition and multiplication. In F#, we can define a
discriminated union to model the four cases:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span class="t">Expression</span> <span class="o">=</span> 
  | <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="p">Variable</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="t">string</span>
  | <span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="p">Constant</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs4', 4)" onmouseover="showTip(event, 'fs4', 4)" class="t">int</span>
  | <span onmouseout="hideTip(event, 'fs5', 5)" onmouseover="showTip(event, 'fs5', 5)" class="p">Add</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs6', 6)" onmouseover="showTip(event, 'fs6', 6)" class="t">Expression</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs6', 7)" onmouseover="showTip(event, 'fs6', 7)" class="t">Expression</span>
  | <span onmouseout="hideTip(event, 'fs7', 8)" onmouseover="showTip(event, 'fs7', 8)" class="p">Mul</span> <span class="k">of</span> <span onmouseout="hideTip(event, 'fs6', 9)" onmouseover="showTip(event, 'fs6', 9)" class="t">Expression</span> <span class="o">*</span> <span onmouseout="hideTip(event, 'fs6', 10)" onmouseover="showTip(event, 'fs6', 10)" class="t">Expression</span>
</code></pre></td>
</tr>
</table>
<p>This says that an <code>Expression</code> can be one of four different cases. If it is a variable, it
contains the name (as a <code>string</code>), if it is addition, it contains two sub-expressions and so on.</p>
<p>Now, writing formatting is quite easy, because we can write a recursive function <code>format</code> that
takes an expression and uses the <code>match</code> construct to handle the different cases:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span class="k">rec</span> <span onmouseout="hideTip(event, 'fs8', 11)" onmouseover="showTip(event, 'fs8', 11)" class="f">format</span> <span onmouseout="hideTip(event, 'fs9', 12)" onmouseover="showTip(event, 'fs9', 12)" class="i">e</span> <span class="o">=</span> 
  <span class="k">match</span> <span onmouseout="hideTip(event, 'fs9', 13)" onmouseover="showTip(event, 'fs9', 13)" class="i">e</span> <span class="k">with</span>
  | <span onmouseout="hideTip(event, 'fs1', 14)" onmouseover="showTip(event, 'fs1', 14)" class="p">Variable</span> <span onmouseout="hideTip(event, 'fs10', 15)" onmouseover="showTip(event, 'fs10', 15)" class="i">s</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs10', 16)" onmouseover="showTip(event, 'fs10', 16)" class="i">s</span>
  | <span onmouseout="hideTip(event, 'fs3', 17)" onmouseover="showTip(event, 'fs3', 17)" class="p">Constant</span> <span onmouseout="hideTip(event, 'fs11', 18)" onmouseover="showTip(event, 'fs11', 18)" class="i">n</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs2', 19)" onmouseover="showTip(event, 'fs2', 19)" class="f">string</span> <span onmouseout="hideTip(event, 'fs11', 20)" onmouseover="showTip(event, 'fs11', 20)" class="i">n</span>
  | <span onmouseout="hideTip(event, 'fs5', 21)" onmouseover="showTip(event, 'fs5', 21)" class="p">Add</span>(<span onmouseout="hideTip(event, 'fs12', 22)" onmouseover="showTip(event, 'fs12', 22)" class="i">l</span>, <span onmouseout="hideTip(event, 'fs13', 23)" onmouseover="showTip(event, 'fs13', 23)" class="i">r</span>) <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs14', 24)" onmouseover="showTip(event, 'fs14', 24)" class="f">sprintf</span> <span class="s">&quot;(</span><span class="pf">%s</span><span class="s"> + </span><span class="pf">%s</span><span class="s">)&quot;</span> (<span onmouseout="hideTip(event, 'fs8', 25)" onmouseover="showTip(event, 'fs8', 25)" class="f">format</span> <span onmouseout="hideTip(event, 'fs12', 26)" onmouseover="showTip(event, 'fs12', 26)" class="i">l</span>) (<span onmouseout="hideTip(event, 'fs8', 27)" onmouseover="showTip(event, 'fs8', 27)" class="f">format</span> <span onmouseout="hideTip(event, 'fs13', 28)" onmouseover="showTip(event, 'fs13', 28)" class="i">r</span>)
  | <span onmouseout="hideTip(event, 'fs7', 29)" onmouseover="showTip(event, 'fs7', 29)" class="p">Mul</span>(<span onmouseout="hideTip(event, 'fs12', 30)" onmouseover="showTip(event, 'fs12', 30)" class="i">l</span>, <span onmouseout="hideTip(event, 'fs13', 31)" onmouseover="showTip(event, 'fs13', 31)" class="i">r</span>) <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs14', 32)" onmouseover="showTip(event, 'fs14', 32)" class="f">sprintf</span> <span class="s">&quot;</span><span class="pf">%s</span><span class="s">*</span><span class="pf">%s</span><span class="s">&quot;</span> (<span onmouseout="hideTip(event, 'fs8', 33)" onmouseover="showTip(event, 'fs8', 33)" class="f">format</span> <span onmouseout="hideTip(event, 'fs12', 34)" onmouseover="showTip(event, 'fs12', 34)" class="i">l</span>) (<span onmouseout="hideTip(event, 'fs8', 35)" onmouseover="showTip(event, 'fs8', 35)" class="f">format</span> <span onmouseout="hideTip(event, 'fs13', 36)" onmouseover="showTip(event, 'fs13', 36)" class="i">r</span>)
</code></pre></td>
</tr>
</table>
<p>The code is quite straightforward. When we get a variable, we just return its name. When we get
a constant, we turn the number to a string and return it. Addition and multiplication is
a little more interesting - we call <code>format</code> recursively on the two sub-expressions and then
build a composed string.</p>
<h2>Defining discriminated unions in C#</h2>
<p>Unfortunately, C# does not give us a simple way to define custom discriminated union types
(you can send a pull request from <a href="https://github.com/Microsoft/visualfsharp/">this repo</a>
to <a href="https://github.com/dotnet/roslyn/">this repo</a>, although you might get labelled as troll).
However, there is <em>one</em> discriminated union hiding in C#!</p>
<p>To find it, we need a bit of programming language theory (which has nothing to do with monads,
by the way). The <a href="http://books.google.cz/books/about/Types_and_Programming_Languages.html?id=ti6zoAC9Ph8C&amp;redir_esc=y">Types and Programming Languages</a>
book (page 177) says the following about ML exceptions:</p>
<blockquote>
<p>The same idea can be refined to leave room for user-defined exceptions
by taking <span class="math">\(T_{exn}\)</span> to be an <em>extensible variant type</em>. ML adopts this
idea, providing a single extensible variant type called <code>exn</code>.</p>
</blockquote>
<p>So, interestingly, you can see ML (and F#) exceptions as <em>a single discriminated union</em> and
custom exceptions as cases of this union! If we stretch the idea a bit, we can use it and
extend the <em>only</em> discriminated union that is available in C# by defining our expressions
as custom exceptions:</p>
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
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="csharp"><span class="k">public</span> <span class="k">class</span> Variable <span class="o">:</span> Exception {
  <span class="k">public</span> <span class="k">string</span> Name { get; set; } 
}
<span class="k">public</span> <span class="k">class</span> Constant <span class="o">:</span> Exception {
  <span class="k">public</span> <span class="k">int</span> Value { get; set; } 
}
<span class="k">public</span> <span class="k">class</span> Add <span class="o">:</span> Exception {
  <span class="k">public</span> Exception Left { get; set; }
  <span class="k">public</span> Exception Right { get; set; } 
}
<span class="k">public</span> <span class="k">class</span> Multiply <span class="o">:</span> Exception {
  <span class="k">public</span> Exception Left { get; set; }
  <span class="k">public</span> Exception Right { get; set; } 
}
</code></pre></td></tr></table>
<p>This is a bit tedious, but I'm pretty sure that Resharper can (with some nice plugin) generate
this for you from the original F# code. If you are a Clean Coder, be sure to split this definition
across 4 different files.</p>
<h2>Formatting math expressions in C#</h2>
<p>Now we have our discriminated union, so let's have a look how we can rewrite the expression
formatting code in C#. The amazing thing about this first example is that it does not even
need C# 6 - which means you can adopt this technique today! But because I want to be cooler
and show off my cutting edge coding skills, I'll also use <em>string interpolation</em> from C# 6.</p>
<p>The idea is quite simple, we get the expression as an argument of type <code>Exception</code>, we
<code>throw</code> it and we use the limited form of pattern matching provided by C# <code>catch</code> construct:</p>
<pre lang="csharp"><span class="k">public</span> <span class="k">static</span> <span class="k">string</span> Format(Exception e) {
  <span class="k">try</span> { <span class="k">throw</span> e; }
  <span class="k">catch</span> (Constant n) { <span class="k">return</span> n.Value.ToString(); }
  <span class="k">catch</span> (Variable v) { <span class="k">return</span> v.Name; }
  <span class="k">catch</span> (Add a) { <span class="k">return</span> <span class="s">$"({</span>Format(a.Left)<span class="s">} + {</span>Format(a.Right)<span class="s">})"</span>; }
  <span class="k">catch</span> (Multiply a) { <span class="k">return</span> <span class="s">$"{</span>Format(a.Left)<span class="s">} * {</span>Format(a.Right)<span class="s">}"</span>; }
}</pre>
<p>This amazing technique is actually quite close to what you can do with F#. Each <code>catch</code> clause
corresponds to one clause of the <code>match</code> construct in F#. Another nice thing is that this not
just checks that the value has the right type, say <code>Multiply</code>, but it also type casts the input
to the right type <em>for free</em> - so in the body, we can access <code>a.Left</code> and <code>a.Right</code> directly.</p>
<p>Now, there are some limitations - because the <code>Exception</code> discriminated union is <em>open</em>, the
C# compiler cannot do exhaustiveness checks. That is, F# will warn you if you forget a case but
C# will not. For completeness let's see how this works using a sample input:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
<span class="l">6: </span>
<span class="l">7: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="csharp"><span class="k">var</span> expr <span class="o">=</span> <span class="k">new</span> Multiply {
  Left <span class="o">=</span> <span class="k">new</span> Variable { Name <span class="o">=</span> <span class="s">"x"</span> },
  Right <span class="o">=</span> <span class="k">new</span> Add {
    Left <span class="o">=</span> <span class="k">new</span> Constant { Value <span class="o">=</span> <span class="n">1</span> },
    Right <span class="o">=</span> <span class="k">new</span> Constant { Value <span class="o">=</span> <span class="n">2</span> } }
};
Console.WriteLine(Format(expr));
</code></pre></td></tr></table>
<p>To run this, you do not even need C# 6, so you can try it right now - and you should see that the code
not only <em>does not throw an exception</em> but gives the correct result which is <code>"x * (1 + 2)"</code>.</p>
<h2>Evaluating expressions with exception filters</h2>
<p>As I mentioned, pattern matching is significantly improved in C# 6 thanks to <em>exception filters</em>.
To demonstrate this feature, I'll extend our example with a simple expression evaluator. In
addition to an expression, the method also takes a dictionary that defines values for the
variables in the expression.</p>
<p>When evaluating <code>Variable</code>, we need to distinguish two different cases - if the variable is
in the dictionary, we just return its value. Otherwise, we report an error. This can be done
elegantly with exception filters:</p>
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
<td class="snippet"><pre class="fssnip highlighted"><code lang="csharp"><span class="k">public</span> <span class="k">static</span> <span class="k">int</span> Evaluate(Exception e, IDictionary&lt;<span class="k">string</span>, <span class="k">int</span>&gt; vars) {
  <span class="k">int</span> res;
  <span class="k">try</span> { <span class="k">throw</span> e; }
  <span class="k">catch</span> (Constant n) { <span class="k">return</span> n.Value; }
  <span class="k">catch</span> (Variable v) when (vars.TryGetValue(v.Name, <span class="k">out</span> res)) { <span class="k">return</span> res; }
  <span class="k">catch</span> (Variable _) { <span class="k">throw</span> <span class="k">new</span> ArgumentException(<span class="s">"Variable not found!"</span>); }
  <span class="k">catch</span> (Add a) { <span class="k">return</span> Evaluate(a.Left, vars) <span class="o">+</span> Evaluate(a.Right, vars); }
  <span class="k">catch</span> (Multiply a) { <span class="k">return</span> Evaluate(a.Left, vars) <span class="o">+</span> Evaluate(a.Right, vars); }
}
</code></pre></td></tr></table>
<p>As you can see, exception filters give us some more of the pattern matching power from F#.
Here, we can use two <em>patterns</em> <code>(Variable v) when (...)</code> handles the case when a variable
value is defined in the dictionary <code>vars</code> and the pattern <code>(Variable _)</code> is used to handle
all remaining cases.</p>
<p>To test this, we can call the <code>Evaluate</code> function as follows:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="i">Console</span><span class="o">.</span><span class="i">WriteLine</span>(<span class="i">Evaluate</span>(<span class="i">expr</span>, 
  <span class="k">new</span> <span class="i">Dictionary</span><span class="o">&lt;</span><span onmouseout="hideTip(event, 'fs2', 37)" onmouseover="showTip(event, 'fs2', 37)" class="i">string</span>, <span onmouseout="hideTip(event, 'fs4', 38)" onmouseover="showTip(event, 'fs4', 38)" class="i">int</span><span class="o">&gt;</span> { { <span class="s">&quot;x&quot;</span>, <span class="n">4</span> } }));
</code></pre></td>
</tr>
</table>
<p>To run this, you'll actually need a version of C# that supports exception filters - either
Visual Studio 2015 preview, or latest build <a href="https://github.com/dotnet/roslyn/">of Roslyn</a>.
If you run it, you'll get 12 as the result, as expected.</p>
<h2>Summary</h2>
<p>Pattern matching is one of the most useful concepts in F# and functional programming,
because it lets you express complex logic in a very clear way with just a few lines of
code. Unfortunately, the full power of pattern matching is not yet available in C#.</p>
<p>As a C# developer, you have basically two options.</p>
<ul>
<li>
<p>One option is to learn <a href="http://fsharp.org/">F#</a>, which supports pattern matching and
many other useful concepts. There is a <a href="http://fsharp.org/about/learning.html">lot of great learning material</a>,
learning F# is quite fun and there is <a href="https://twitter.com/search?q=%23fsharp">a lively community on Twitter</a>
that will help you.</p>
</li>
<li>
<p>The other option is to use some of the less well explored corners of the C# language.
It turns out that there is already <em>some</em> nice support for pattern matching in C# and
C# 6 goes even further with <em>exception filters</em>. There is one little limitation, which
is that it only works on <em>exceptions</em>.</p>
</li>
</ul>
<p>Why support pattern matching <em>only</em> on exceptions?
Don't ask me! It seems a bit silly to me too - if I was in charge of C# language
design, I would obviously add pattern matching on the JSON.NET <a href="http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JContainer.htm">JContainer
classes</a>,
because that would be useful <em>at least for some things</em>.</p>


<div class="tip" id="fs1">union case Expression.Variable: string -&gt; Expression</div>
<div class="tip" id="fs2">Multiple items<br />val string : value:&#39;T -&gt; string<br /><br />Full name: Microsoft.FSharp.Core.Operators.string<br /><br />--------------------<br />type string = System.String<br /><br />Full name: Microsoft.FSharp.Core.string</div>
<div class="tip" id="fs3">union case Expression.Constant: int -&gt; Expression</div>
<div class="tip" id="fs4">Multiple items<br />val int : value:&#39;T -&gt; int (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.int<br /><br />--------------------<br />type int = int32<br /><br />Full name: Microsoft.FSharp.Core.int<br /><br />--------------------<br />type int&lt;&#39;Measure&gt; = int<br /><br />Full name: Microsoft.FSharp.Core.int&lt;_&gt;</div>
<div class="tip" id="fs5">union case Expression.Add: Expression * Expression -&gt; Expression</div>
<div class="tip" id="fs6">type Expression =<br />&#160;&#160;| Variable of string<br />&#160;&#160;| Constant of int<br />&#160;&#160;| Add of Expression * Expression<br />&#160;&#160;| Mul of Expression * Expression<br /><br />Full name: Csharp-pattern-matching.Expression</div>
<div class="tip" id="fs7">union case Expression.Mul: Expression * Expression -&gt; Expression</div>
<div class="tip" id="fs8">val format : e:Expression -&gt; string<br /><br />Full name: Csharp-pattern-matching.format</div>
<div class="tip" id="fs9">val e : Expression</div>
<div class="tip" id="fs10">val s : string</div>
<div class="tip" id="fs11">val n : int</div>
<div class="tip" id="fs12">val l : Expression</div>
<div class="tip" id="fs13">val r : Expression</div>
<div class="tip" id="fs14">val sprintf : format:Printf.StringFormat&lt;&#39;T&gt; -&gt; &#39;T<br /><br />Full name: Microsoft.FSharp.Core.ExtraTopLevelOperators.sprintf</div>
