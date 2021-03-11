(**
Pattern matching in action using C# 6
=====================================

 - date: 2015-04-01T11:41:49.5611652+01:00
 - description: The new release of the C# language migth seem as a minor version, but in fact, it contains a hidden gem. In fact, it contains awesome support for pattern matching - but for some reason, this is concealed under the name 'exception filters'.
 - layout: article
 - image: http://tomasp.net/blog/2015/csharp-pattern-matching/hat.png
 - tags: c#,fun,functional programming
 - title: Pattern matching in action using C# 6
 - url: 2015/csharp-pattern-matching

--------------------------------------------------------------------------------
 - standalone


<img src="http://tomasp.net/blog/2015/csharp-pattern-matching/hat.png" style="float:right;margin:20px" />

On year ago, on this very day, [I wrote about the open-sourcing of C# 
6.0](http://tomasp.net/blog/2014/csharp-6-released/index.html). Thanks to a very special
information leak, I learned about this about a week before Microsoft officially announced
it. However, my information were slightly incorrect - rather then releasing the [much 
improved version of the language](https://github.com/Microsoft/visualfsharp/), Microsoft
continued working on language version internally called "Small C#", which is now available
as "C# 6" in the [Visual Studio 2015 preview](https://www.visualstudio.com/en-us/news/vs2015-vs.aspx).

It is my understanding that, with this release, Microsoft is secretly testing the reaction of the 
developer audience to some of the amazing features that F# developers loved and used for 
the last 7 years and that are coming to C# _very soon_. To avoid shock, these are however
carefuly hidden!

In this blog post, I'm going to show you _pattern matching_ which is probably the most useful
hidden C# feature and its improvements in C# 6. For reasons that elude me, pattern matching in 
C# 6 is called _exception filters_ and has some unfortunate restrictions. But we can still 
use it to write nice functional code!


--------------------------------------------------------------------------------


<img src="http://tomasp.net/blog/2015/csharp-pattern-matching/hat.png" style="float:right;margin:20px" />

On year ago, on this very day, [I wrote about the open-sourcing of C# 
6.0](http://tomasp.net/blog/2014/csharp-6-released/index.html). Thanks to a very special
information leak, I learned about this about a week before Microsoft officially announced
it. However, my information were slightly incorrect - rather then releasing the [much 
improved version of the language](https://github.com/Microsoft/visualfsharp/), Microsoft
continued working on language version internally called "Small C#", which is now available
as "C# 6" in the [Visual Studio 2015 preview](https://www.visualstudio.com/en-us/news/vs2015-vs.aspx).

It is my understanding that, with this release, Microsoft is secretly testing the reaction of the 
developer audience to some of the amazing features that F# developers loved and used for 
the last 7 years and that are coming to C# _very soon_. To avoid shock, these are however
carefuly hidden!

In this blog post, I'm going to show you _pattern matching_ which is probably the most useful
hidden C# feature and its improvements in C# 6. For reasons that elude me, pattern matching in 
C# 6 is called _exception filters_ and has some unfortunate restrictions. But we can still 
use it to write nice functional code!

> **UPDATE:** In case you are reading this article later than on the day when it was
> published, let me just point out that this was released on 1 April 2015. Keep that
> in mind before putting the code in production. Have fun ;-).

Formatting simple math expressions with F#
------------------------------------------

For easier understanding, I'll first introduce the idea of pattern matching using F#.
I'll use a fairly standard functional programming example - formatting and
evaluation of simple algebraic expression. We want to work with expressions like $x * (1 + 2)$,
so we'll need variables, constants, addition and multiplication. In F#, we can define a 
discriminated union to model the four cases:
*)
type Expression = 
  | Variable of string
  | Constant of int
  | Add of Expression * Expression
  | Mul of Expression * Expression
(**
This says that an `Expression` can be one of four different cases. If it is a variable, it 
contains the name (as a `string`), if it is addition, it contains two sub-expressions and so on.

Now, writing formatting is quite easy, because we can write a recursive function `format` that
takes an expression and uses the `match` construct to handle the different cases:
*)
let rec format e = 
  match e with
  | Variable s -> s
  | Constant n -> string n
  | Add(l, r) -> sprintf "(%s + %s)" (format l) (format r)
  | Mul(l, r) -> sprintf "%s*%s" (format l) (format r)
(**

The code is quite straightforward. When we get a variable, we just return its name. When we get
a constant, we turn the number to a string and return it. Addition and multiplication is 
a little more interesting - we call `format` recursively on the two sub-expressions and then
build a composed string.

Defining discriminated unions in C#
-----------------------------------

Unfortunately, C# does not give us a simple way to define custom discriminated union types
(you can send a pull request from [this repo](https://github.com/Microsoft/visualfsharp/) 
to [this repo](https://github.com/dotnet/roslyn/), although you might get labelled as troll).
However, there is *one* discriminated union hiding in C#!

To find it, we need a bit of programming language theory (which has nothing to do with monads,
by the way). The [Types and Programming Languages](http://books.google.cz/books/about/Types_and_Programming_Languages.html?id=ti6zoAC9Ph8C&redir_esc=y)
book (page 177) says the following about ML exceptions:

> The same idea can be refined to leave room for user-defined exceptions
> by taking $T_{exn}$ to be an _extensible variant type_. ML adopts this
> idea, providing a single extensible variant type called `exn`.

So, interestingly, you can see ML (and F#) exceptions as _a single discriminated union_ and
custom exceptions as cases of this union! If we stretch the idea a bit, we can use it and
extend the _only_ discriminated union that is available in C# by defining our expressions
as custom exceptions:

    [lang=csharp]
    public class Variable : Exception {
      public string Name { get; set; } 
    }
    public class Constant : Exception {
      public int Value { get; set; } 
    }
    public class Add : Exception {
      public Exception Left { get; set; }
      public Exception Right { get; set; } 
    }
    public class Multiply : Exception {
      public Exception Left { get; set; }
      public Exception Right { get; set; } 
    }

This is a bit tedious, but I'm pretty sure that Resharper can (with some nice plugin) generate 
this for you from the original F# code. If you are a Clean Coder, be sure to split this definition
across 4 different files.

Formatting math expressions in C#
---------------------------------

Now we have our discriminated union, so let's have a look how we can rewrite the expression 
formatting code in C#. The amazing thing about this first example is that it does not even
need C# 6 - which means you can adopt this technique today! But because I want to be cooler
and show off my cutting edge coding skills, I'll also use _string interpolation_ from C# 6.

The idea is quite simple, we get the expression as an argument of type `Exception`, we 
`throw` it and we use the limited form of pattern matching provided by C# `catch` construct:

<pre lang="csharp"><span class="k">public</span> <span class="k">static</span> <span class="k">string</span> Format(Exception e) {
  <span class="k">try</span> { <span class="k">throw</span> e; }
  <span class="k">catch</span> (Constant n) { <span class="k">return</span> n.Value.ToString(); }
  <span class="k">catch</span> (Variable v) { <span class="k">return</span> v.Name; }
  <span class="k">catch</span> (Add a) { <span class="k">return</span> <span class="s">$"({</span>Format(a.Left)<span class="s">} + {</span>Format(a.Right)<span class="s">})"</span>; }
  <span class="k">catch</span> (Multiply a) { <span class="k">return</span> <span class="s">$"{</span>Format(a.Left)<span class="s">} * {</span>Format(a.Right)<span class="s">}"</span>; }
}</pre>

This amazing technique is actually quite close to what you can do with F#. Each `catch` clause
corresponds to one clause of the `match` construct in F#. Another nice thing is that this not 
just checks that the value has the right type, say `Multiply`, but it also type casts the input
to the right type _for free_ - so in the body, we can access `a.Left` and `a.Right` directly.

Now, there are some limitations - because the `Exception` discriminated union is _open_, the 
C# compiler cannot do exhaustiveness checks. That is, F# will warn you if you forget a case but
C# will not. For completeness let's see how this works using a sample input:

    [lang=csharp]    
    var expr = new Multiply {
      Left = new Variable { Name = "x" },
      Right = new Add {
        Left = new Constant { Value = 1 },
        Right = new Constant { Value = 2 } }
    };
    Console.WriteLine(Format(expr));

To run this, you do not even need C# 6, so you can try it right now - and you should see that the code
not only _does not throw an exception_ but gives the correct result which is `"x * (1 + 2)"`.

Evaluating expressions with exception filters
---------------------------------------------

As I mentioned, pattern matching is significantly improved in C# 6 thanks to _exception filters_.
To demonstrate this feature, I'll extend our example with a simple expression evaluator. In 
addition to an expression, the method also takes a dictionary that defines values for the
variables in the expression.

When evaluating `Variable`, we need to distinguish two different cases - if the variable is
in the dictionary, we just return its value. Otherwise, we report an error. This can be done
elegantly with exception filters:

    [lang=csharp]
    public static int Evaluate(Exception e, IDictionary<string, int> vars) {
      int res;
      try { throw e; }
      catch (Constant n) { return n.Value; }
      catch (Variable v) when (vars.TryGetValue(v.Name, out res)) { return res; }
      catch (Variable _) { throw new ArgumentException("Variable not found!"); }
      catch (Add a) { return Evaluate(a.Left, vars) + Evaluate(a.Right, vars); }
      catch (Multiply a) { return Evaluate(a.Left, vars) + Evaluate(a.Right, vars); }
    }

As you can see, exception filters give us some more of the pattern matching power from F#.
Here, we can use two _patterns_ `(Variable v) when (...)` handles the case when a variable
value is defined in the dictionary `vars` and the pattern `(Variable _)` is used to handle
all remaining cases.

To test this, we can call the `Evaluate` function as follows:

    Console.WriteLine(Evaluate(expr, 
      new Dictionary<string, int> { { "x", 4 } }));

To run this, you'll actually need a version of C# that supports exception filters - either
Visual Studio 2015 preview, or latest build [of Roslyn](https://github.com/dotnet/roslyn/).
If you run it, you'll get 12 as the result, as expected.

Summary
-------

Pattern matching is one of the most useful concepts in F# and functional programming, 
because it lets you express complex logic in a very clear way with just a few lines of
code. Unfortunately, the full power of pattern matching is not yet available in C#.

As a C# developer, you have basically two options.

 - One option is to learn [F#](http://fsharp.org/), which supports pattern matching and
   many other useful concepts. There is a [lot of great learning material](http://fsharp.org/about/learning.html),
   learning F# is quite fun and there is [a lively community on Twitter](https://twitter.com/search?q=%23fsharp)
   that will help you.

 - The other option is to use some of the less well explored corners of the C# language.
   It turns out that there is already _some_ nice support for pattern matching in C# and
   C# 6 goes even further with _exception filters_. There is one little limitation, which
   is that it only works on _exceptions_. 
   
Why support pattern matching _only_ on exceptions?
Don't ask me! It seems a bit silly to me too - if I was in charge of C# language
design, I would obviously add pattern matching on the JSON.NET [JContainer 
classes](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JContainer.htm), 
because that would be useful _at least for some things_.

*)
