Can programming be liberated from function abstraction?
=======================================================

 - date: 2016-09-27T16:53:32.8941805+01:00
 - description: Functions are the reason why many nice features become hard or impossible to implement.
     Functions make type inference hard and they make it impossible to use tools that rely on 
     manipulation with concrete values - because functions introduce names with unknown values
     and types. Can we take inspiration from spreadsheet programming and build alternative 
     abstraction mechanism that does not introduce this problematic property?     
 - layout: post
 - image: http://tomasp.net/blog/2016/no-functions/lambda.png
 - tags: thegamma,research,programming languages
 - title: Can programming be liberated from function abstraction?
 
----------------------------------------------------------------------------------------------------

<img src="http://tomasp.net/blog/2016/no-functions/lambda.png" style="width:180px" class="rdecor" />

When you start working in the programming language theory business, you'll soon find out that
lambda abstraction and functions break many nice ideas or, at least, make your life very hard.
For example, type inference is easy if you only have `var x = ...`, but it gets hard once you
want to infer type of `x` in something like `fun x -> ...` because we do not know what is 
assigned to `x`. Distributed programming is another example - sending around data is easy, but
once you start sending around function values, things become hard. 

Every programming language researcher soon learns this trick. When someone tells you about a nice
idea, you reply _"Interesting... but how does this interact with lambda abstraction?"_ and the 
other person replies _"Whoa, hmm, let me think more about this."_ Then they go back and either give up,
because it does not work, or produce something that works, in theory, well with lambda abstraction, 
but is otherwise quite unusable. 

When working on [The Gamma project](http://tomasp.net/blog/2016/thegamma-olympic-medalists) and the
little [scripting language it runs](github.com/the-gamma/thegamma-script), I recently went through
a similar thinking process. Instead of letting lambda abstraction spoil the party _again_, 
I think we need to think about different ways of code reuse. 

----------------------------------------------------------------------------------------------------

## On not adding functions to The Gamma

I wrote about [The Gamma project](http://tomasp.net/blog/2016/thegamma-dni) in the last blog post.
The goal of the project is to build easy to use programming tools that can be used to build 
open and reproducible data-driven stories. Rather than including a chart in an article, you should
be able to embed a piece of code that _gets the data_ and _creates the chart_. This way, anyone
can see how the data is processed and can also modify and reuse the code. The [Olympic medalists
visualization](http://tomasp.net/blog/2016/thegamma-olympic-medalists/) illustrates the ideas.

The Gamma implements a [simple programming language](https://github.com/the-gamma/thegamma-script)
that runs in the web browser and lets you get the data and build visualizations. It uses type
providers for integrating data into the language. In the Olympics visualization, you can get all
US and UK medalists for marathon as follows:

<div class="imgbox">
  <img src="blog-1.gif" style="display:none" id="img1a" />
  <img src="blog-1-still.gif" class="imgvid" id="img1b" /><br />
  (<a href="blog-1.gif" target="_blank">Open the image in a new window</a>)<!--_ -->
</div>

The example shows a case where we might want to use a function to avoid code duplication. Rather
than getting the medalists for the two countries and then filtering them by discipline _twice_, 
could we write a function to perform filtering by discipline and then call it with data for 
the two countries?

The obvious answer is to add functions (either named or lambda functions). However, this would
break many of the nice features that can work exactly because the language does not have functions.

For the record, I'm mainly thinking about scripting and data analytics and so if you read this 
article with data analytics in mind, it might make more sense. That said, I think the same ideas
would apply to "complex systems" - it just requires more imagination.


## What is wrong with function abstraction

Let me go through two examples that illustrate some of the problems with function abstraction.
The first one will be using the [World Bank type provider](http://fsharp.github.io/FSharp.Data/library/WorldBank.html)
in F# Data and it illustrates difficulties with _types_. The second one is using the Chrome
JavaScript console and illustrates difficulties with _values_.

### How functions break type-based tooling

The World Bank type provider lets you easily access information about countries from F#. The
type provider imports country names and available indicators into the _type system_ and so you
can find available data just by typing `.`. This works beautifully in scripts, but what if
we want to remove code duplication and write a function that returns the GDP (in current 
US dollars) for a given country in a given year? This is not so easy:

<div class="imgbox">
  <img src="blog-2.gif" style="display:none" id="img2a" />
  <img src="blog-2-still.gif" class="imgvid" id="img2b" /><br />
  (<a href="blog-2.gif" target="_blank">Open the image in a new window</a>)<!--_ -->
</div>

When you define a function taking `country` and try access the members using `.`, F# will only 
offer you members that all .NET objects have. This is not very useful. Now, there is a number
of options that we have:

 * In F#, you can add a type annotation that specifies the type of `country`. With World Bank
   type provider, the provided type is somewhat hard to find, but it has a name and you can 
   use it. This is harder with some type providers where types of entities do not have obvious 
   names and so finding the right type for a type annotation is not something you'd want to do.

 * In languages with structural type system, you could allow the user to access property of any
   name (say `GDP (current USD)`) and infer a type specifying that the `country` value needs to 
   have this property. But this means the auto-completion cannot give you any useful hints and 
   you'll only discover typos when you try calling the function - telling you that the name is 
   actually `US$` and not `USD`.

 * You could do some more fancy whole-program type inference and deduce the type of `getGdpInYear`
   from the later part of the code where you call the function, but this forces you to write the
   code in an odd order - you need to declare your function, then use it and then go back to 
   actually implement it.
 
### How functions break value-based tooling

I think the F# example illustrates an issue that is not F#-specific, but in case it did not
convince you, let's look at another example. This time, I'm using the Chrome developer console
to extract title of a page and then I want to wrap it into a helper function `getTitle`:

<div class="imgbox">
  <img src="blog-3.gif" style="display:none" id="img3a" />
  <img src="blog-3-still.gif" class="imgvid" id="img3b" /><br />
  (<a href="blog-3.gif" target="_blank">Open the image in a new window</a>)<!--_ -->
</div>

Even though JavaScript is dynamic, Chrome developer tools can give you useful auto-completion
hints. This seems to be based mostly on values. When you have a value `el` in scope and you type
`el.` the auto-complete can just give you access to all members of the object. Chrome also uses
various heuristic hints (like name of the variable) and so it sometimes gives you more (or less)
useful hints on other variables.

Is there any chance for making value-based auto-completion to work in presence of functions? 
I think the best that can be done will inevitably rely on making a best guess. This also means
that we do not get other nice things that are possible when we know values of every variable in 
scope that various live programming systems have 
(like [Sean McDirmid's work](http://research.microsoft.com/en-us/um/people/smcdirm/apx/) 
or [Python tutor](http://www.pythontutor.com/) project).

In the F# example, the problem with functions is that we do not know the _type_ of their arguments.
In the JavaScript example, the problem is that we do not know the _value_ of their arguments. With
types, you can give the type explicitly (via annotation). With values, you could give a sample 
value for primitives (numbers or strings), but I doubt this can be done in a usable way for 
complex values (say, a chart skeleton).

## Abstraction without functions

Looking at the vertical scroll bar, I'm sure you're wondering when am I finally going to offer my 
alternative solution to this problem. How can we program in a reasonable way without a mechanism
similar to functions? I do not have a clear answer, but there is one programming system that 
has all the properties I would like regular programming languages to have:

 1. When you have a variable, you always know what is its type and value
 2. You can reuse existing piece of code and apply it to multiple inputs 

### Formula abstraction in Excel

The one programming tool that has both of these properties is Excel. You might not think about
Excel as a programming language, but [in many ways, it is one](https://vimeo.com/145492419).
The following example illustrates how "abstraction" works in excel. I want to do a simple
calculation over all rows of a table, so I write the calculation using one concrete value (row) 
and then "drag it down" to apply the same calculation on the whole table:

<div class="imgbox">
  <img src="blog-4.gif" style="display:none" id="img4a" />
  <img src="blog-4-still.gif" class="imgvidnb" id="img4b" /><br />
  (<a href="blog-4.gif" target="_blank">Open the image in a new window</a>)<!--_ -->
</div>

Excel spreadsheets have many issues, but I think the way it lets you apply the same computation
on multiple inputs while _always_ working with _concrete, relevant values_ is one thing that it
does amazingly well. The Excel feature does have good and bad aspects:

 * The formula in the above example is copied and the reference to the original one is lost,
   so changing the formula in all rows of the table is error-prone. 
   
 * If you mark the data as table explicitly, Excel will let you refer to columns using the `[@Rate]`
   syntax, which makes the formulas nicer and it also automatically extends the formula over all
   rows, but this works because spreadsheets are dealing with a fairly simple grid. 
   
### From spreadsheets to general-purpose programming   

I do not think any programming language achieves quite the same experience as Excel. In the above
example, Excel works so nicely because you never work with a name that does not have a 
concrete value. You can only refer to cells using their index (B4 or C4) and those always have 
valid values. 

 * In the Haskell [point-free style](https://wiki.haskell.org/Pointfree), 
   you can write `(sum .) . zipWith (*)` with no names (aside from known functions). Here, we are
   not transforming values, but instead composing a function. There are no concrete values _at all_.
   You might like this style for many reasons, but it does not make it possible to get the nice
   programming tooling that that you get with `.` when you know the value of an object.
   
 * Language that is perhaps the closest to this idea is Smalltalk with its runtime system
   where you work with concrete objects (values) and you modify them live. However, in Smalltalk,
   this works good at the entire-system level, but it is not (as far as I'm aware) used that much
   when it comes to writing code at message-level.

 * A possibly related design point is LINQ in C# and [especially in Visual 
   Basic](https://msdn.microsoft.com/en-us/library/bb384667.aspx). In VB, you start your query
   by specifying data source using `From` and then select members at the end using `Select`. 
   According to a rumor I've heard, the original proposal was to use SQL-style with 
   `Select .. From ..`, but there was no way to make the user experience as good as if the 
   names refer to objects of known types.

 * Interestingly, visual tools for UI design have similar problem. For example, tools for 
   Silverlight have [elaborate methods for generating sample data](https://msdn.microsoft.com/en-us/library/ee341450(v=expression.40).aspx),
   so that when you design the UI, you are working with concrete values. But having to explicitly
   generate fake data does not sound like a reasonable general mechanism to me.
 
## Conclusions

The one principle that makes Excel's dragging of formulas work extremely nicely in contrast
to functions in main-stream programming languages like F# and JavaScript is that the Excel
abstraction mechanism never produces names without concrete values (or at least types).

Could there be an abstraction mechanism that works nicely in a general-purpose programming 
language, which has the same property? Even though I have not yet implemented it, I think
the answer is _yes_. I imagine you should be able to write data analytics script using 
concrete input and then attach name to it and specify which of the inputs can be overridden.
This is something that we absolutely need for project like [The Gamma](http://thegamma.net/),
which tries to make data exploration accessible to everyone, but there might be useful lessons
for general purpose programming too. To keep an eye on future developments arond The Gamma, follow 
[@thegamma_net](https://twitter.com/thegamma_net) on Twitter.

<style>
.imgbox { padding:0px;margin:10px auto 10px auto;text-align:center; }
.imgvid { max-width:100%;cursor:pointer;border:1px solid #d8d8d8;border-radius:6px;padding:10px;margin:0px auto 5px auto; }
.imgvidnb { max-width:100%;cursor:pointer;border:1px solid white;border-radius:6px;padding:10px;margin:0px auto 5px auto; }
</style>

<script>
function setupVid(i) {
  var p = false;
  document.getElementById("img" + i + "b").onclick = function() {
    document.getElementById("img" + i + "b").src = 
      p ? "blog-" + i + "-still.gif" : document.getElementById("img" + i + "a").src;
    p = !p;
  };  
}
setupVid(1);
setupVid(2);
setupVid(3);
setupVid(4);
</script>
