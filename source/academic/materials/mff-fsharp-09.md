# Functional Programming in F#

 - title: Functional Programming in F#
 - layout: paper
 - date: 1 March 2009
 - description: The course explains basic functional concepts such as function composition, functional data types and higher-order functions. It highlights some interesting connections between these and concepts that student may already know or will learn in various mathematics lectures. The site includes lecture slides and a number of simple homeworks that usually require some interesting insight.";

<a href="poster-en.pdf">
<img src="poster.png" style="width:200px;float:right;margin:20px 0px 20px 10px;border:none" /></a>

<div style="margin-right:200px">
<ul>
  <li><strong>Slides 1</strong>: Expression as a basic building block (<a href="slides1.pptx">pptx</a>, <a href="slides1.pdf">pdf</a> format)</li>
  <li><strong>Tutorial</strong>: Sample project for Visual Studio 2008, that should work after installing F# CTP (<a href="Tutorial.zip">zip</a> format)</li>
  <li><strong>Slides 2</strong>: Refactoring code using functions (<a href="slides2.pptx">pptx</a>, <a href="slides2.pdf">pdf</a>)</li>
  <li><strong>Series demo</strong>: Console application that uses <code>ReadLine</code> to calculate the sum of first X members of a number series(<a href="seriesdemo.zip">zip</a> format)</li>
  <li><strong>Slides 3</strong>: Composing primitive types into data (<a href="slides3.pptx">pptx</a>, <a href="slides3.pdf">pdf</a> format)</li>
  <li><strong>Drawing</strong>: Sample from the lecture that demonstrates how to draw using Windows Forms and a template for your homework <em>plot</em> (<a href="drawing.zip">zip</a> format)</li>
  <li><strong>Slides 4</strong>: Generic and recursive types (<a href="slides4.pptx">pptx</a>, <a href="slides4.pdf">pdf</a> format)</li>
  <li><strong>Recursion</strong>: Examples of recursive functions that traverse lists using structural recursion (<a href="recursion.zip">zip</a> format)</li>
  <li><strong>Slides 5</strong>: Hiding recursion using function-as-values
(<a href="slides5.pptx">pptx</a>, <a href="slides5.pdf">pdf</a> format)</li>
  <li><strong>Sequences</strong>: A few notes regarding the "folds" homework and samples of using sequnece expressions (<a href="sequences.zip">zip</a> format)</li>
  <li><strong>Slides 6</strong>: Sequence expressions, computation expressions &amp; asynchronous workflows (<a href="slides6.pptx">pptx</a>, <a href="slides6.pdf">pdf</a> format)</li>
  <li><strong>Monads</strong>: Examples of computation expressions (also known as monads) and asynchronous computations (<a href="monads.zip">zip</a> format)</li>
</ul>
</div>

<h3>Homeworks</h3>
<p>Most of the homeworks are puzzles that should be easy to solve once you get the idea. There will be shorter deadlines for these, so that we can reveal the solution afterwards :-). For more complicated homeowrks where you actually need to write some F# code, you'll have, of course, more time.</p>

<table class="table table-striped">
<tr><td></td><td><strong>Description</strong></td><td class="one"><strong>ID</strong></td><td class="three"><strong>Points</strong></td></tr>

<tr><td valign="top">1. </td><td>Find an expression where <strong>evaluating the value of symbols first</strong> is better and another expression where <strong>replacing symbols with expressions is better</strong> (Note: better means smaller number of reduction steps (no unnecessary calculations). <em>For more information, see Slides 1, slide 33.</em>
</td><td class="one">reduction</td><td class="three">1</td>
</tr>

<tr><td valign="top">2. </td><td>Write expression that prints “yes” if the value of n is less than 10 and “no” otherwise. The trick is, that you should do this without using <strong>if</strong> and <strong>match</strong> construct. <em>This can be solved using other language features that are discussed in Slides 1.</em></td><td class="one">ifthen</td><td class="three">2</td>
</tr>

<tr><td valign="top">3. </td><td>Rewrite the following declaration to use only function declarations taking a single parameter. <em>You can use currying as explained in Slides 2 (you may need to download the latest version of slides!)</em>:
<pre >
<span class="kwrd">let</span> foo x y = 
  <span class="kwrd">let</span> add a b c = 
     (100 * a) + (10 * b) + c
  add y x     
</pre>
</td><td class="one">curry</td><td class="three">1</td>
</tr>

<tr><td valign="top">4. </td><td>Write a function <code>drawFunc</code> that takes a function as an argument and draws the graph of the given function using WinForms (<em>You can download template that demonstrates how to draw something above!</em>) The simplest possible type signature of the function is:
<pre >
<span class="kwrd">val</span> drawFunc : (float32 <span class="op">-&gt;</span> float32) <span class="op">-&gt;</span> unit
</pre>
Optionally, it can take two additional parameters to specify the X scale and Y scale (+1 point).

</td><td class="one">plot</td><td class="three">1+1</td>
</tr>

<tr><td valign="top">5. </td><td>Write a function <code>diff</code> that performs numerical differentiation of a function (<em>This follows similar pattern as other operations for working with functions such as <code>translate</code> from slide 29, Slides 2</em>). The signature of the function should be:
<pre >
<span class="kwrd">val</span> diff : (float32 <span class="op">-&gt;</span> float32) 
        <span class="op">-&gt;</span> (float32 <span class="op">-&gt;</span> float32)</pre>
You can use the standard definition of differentiation and use some small value of <em>d</em> (for example 0.01).
</td><td class="one">diff</td><td class="three">2</td>
</tr>

<tr><td valign="top">6. </td><td>We used “sum” of sets to model discriminated unions and “product” to model tuples (<em>See Slides 3, slides 6 and 19</em>). How can we use this operations to construct mathematical model of the following types:
<pre >
<span class="kwrd">type</span> Season = 
  <span class="op">|</span> Spring <span class="op">|</span> Summer <span class="op">|</span> Autumn <span class="op">|</span> Winter

<span class="kwrd">type</span> Shape = 
  <span class="op">|</span> Circle <span class="kwrd">of</span> int
  <span class="op">|</span> Rectangle <span class="kwrd">of</span> int * int
</pre>
</td><td class="one">sets</td><td class="three">1</td></tr>

<tr><td valign="top">7. </td><td>Write a function that compares two vehicles (<em>Data type in Slides 3, slide 22</em>) and prints detailed information about the more expensive one (<em>You can find the rules for comparison in Slides 3, slide 24</em>). </td><td class="one">patterns</td><td class="three">1</td></tr>


<tr><td valign="top">8. </td><td>
Write a function that counts the number of elements in the list that are larger than or equal to the average (using integer division for simplicity). The function should use just a <strong>single traversal</strong> of the list structure! It should give these results:
<pre >
foo [1; 2; 3; 4] = 3 <span class="rem">// average 2</span>
foo [1; 2; 3; 6] = 2 <span class="rem">// average 3</span>
foo [4; 4; 4; 4] = 4 <span class="rem">// average 4</span>
</pre>
(<em>Hint: You can perform some operation on the way "forward" and another operation on the way "backward", see also Slides 5, slide 14</em>)
</td><td class="one">traversal</td><td class="three">1</td></tr>

<tr><td valign="top">9. </td><td>
Write a tail-recursive function that takes a list and “removes” all odd numbers from the list (that is, returns a copy of the list that doesn't contain odd numbers):
<pre >
removeOdds [1; 2; 3; 5; 4] = [2; 4] <span class="rem">// example</span>
</pre>
(<em>Hints: 1. Tail-recursive functions do all processing when traversing the list forward. 2. You’ll need to do this during two traversals of some list</em>)
</td><td class="one">tail</td><td class="three">1</td></tr>

<tr><td valign="top">10. </td><td>
<pre >
collect : ('a <span class="op">-&gt;</span> 'b list) <span class="op">-&gt;</span> 'a list <span class="op">-&gt;</span> 'b list
</pre>
This function applies the given function to all elements of input list and concatenates all returned lists. It can be used for a wide range of different list processing operations.<br /><br />
Use this function to implement projection and filtering for lists (<code>List.map</code> and <code>List.filter</code>) with the usual type signatures (<em>See Slides 5 for more information.</em>)
</td><td class="one">collect</td><td class="three">1</td></tr>

<tr><td valign="top">11. </td><td>
<ul>
  <li><strong>fold</strong> processes list elements on the way to the front</li>
  <li><strong>foldBack</strong> on the way back (from the end to the beginning</li>
</ul>

Write a more general function (e.g. <code>twoWayFold</code>) that allows us to do both things at once - perform one operation on the way to the front and an operation on the way back. Use this function to implement <code>fold</code> and <code>foldBack</code> (1 point) and also the 8. <code>traversal</code> homework (+1 point). (<em>For more information see Slides 5, slide 14</em>).
</td><td class="one">folds</td><td class="three">1+1</td></tr>

<tr><td valign="top">12. </td><td>
We've seen how to define a computation expression builder for creating computations that can be executed step-by-step using the <code>Resumption</code> type. The sample code also contains a function <code>evaluate</code> which evaluates a single computation and prints a number of each step before running it. Your task is to write a function <code>parallel</code>, which executes two step-by-step computations in parallel. When given computations <em>first</em> and <em>second</em>, it will execute one step of <em>first</em>, one step of <em>second</em>, one step of <em>first</em> etc. To show that your function works, write two computations (e.g. one that prints first ten factorials and other, which prints first ten fibonacci numbers). (<em>See Slides 16 and download Monads demos above</em>).

</td><td class="one">resumptions</td><td class="three">2</td></tr>


<tr><td valign="top">13. </td><td>
Write a <code>bind</code> function for a simple <code>Async</code> type. This type represents an asynchronous computation:
<pre>
 type Async&lt;'a&gt; = 
  | Async of (('a -&gt; unit) -&gt; unit)
</pre>
The type is represented as a function that takes a function as an argument. When the (outer) function is executed, it starts some operation. Once the operation completes (which could be immediately, or at some later time), it reports the result by calling the (inner) function, which it got as an argument. The bind function should have the following type (which pretty much tells you what it has to do):
<pre>
val bind : Async&lt;'a&gt; -&gt; ('a -&gt; Async&lt;'b&gt;) -&gt; Async&lt;'b&gt;
</pre>

</td><td class="one">bind</td><td class="three">3</td></tr>
</table>
