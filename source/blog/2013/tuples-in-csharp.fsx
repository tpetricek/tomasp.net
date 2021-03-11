(**

How many tuple types are there in C#?
=====================================

 - date: 2013-09-17T14:11:57.7562922+01:00
 - description: In a recent StackOverflow question, the poster asked about the choice between a function that takes a tuple and a function that uses the curried form. In this article I look at the problem from the C# perspective.
 - layout: article
 - tags: c#,f#,functional programming
 - title: How many tuple types are there in C#?
 - url: 2013/tuples-in-csharp

--------------------------------------------------------------------------------
 - standalone

In a [recent StackOverflow question](http://stackoverflow.com/questions/18718232/when-should-i-write-my-functions-in-curried-form/18721711)
the poster asked about the difference between _tupled_ and _curried_ form of a function in F#. 
In F#, you can use pattern matching to easily define a function that takes a tuple as an argument.
For example, the poster's function was a simple calculation that multiplies the number 
of units sold _n_ by the price _p_:
*)

let salesTuple (price, count) = price * (float count)

(**
The function takes a single argument of type `Tuple<float, int>` (or, using the nicer F# notation
`float * int`) and immediately decomposes it into two variables, `price` and `count`. The other
alternative is to write a function in the _curried_ form:
*)

let salesCurried price count = price * (float count)

(**
Here, we get a function of type `float -> int -> float`. Usually, you can read this just as a 
function that takes `float` and `int` and returns `float`. However, you can also use _partial
function application_ and call the function with just a single argument - if the price of
an apple is $1.20, we can write `salesCurried 1.20` to get a _new_ function that takes just
`int` and gives us the price of specified number of apples. The poster's question was:

> So when I want to implement a function that would have taken _n > 1_ arguments, 
> should I for example always use a curried function in F# (...)? Or should I take 
> the simple route and use regular function with an n-tuple and curry later on 
> if necessary?

You can see [my answer on StackOverflow](http://stackoverflow.com/questions/18718232/when-should-i-write-my-functions-in-curried-form/18721711#18721711).
The point of this short introduction was that the question inspired me to think about how
the world looks from the C# perspective...


--------------------------------------------------------------------------------
*)
(***hide***)
open System

(**
In a [recent StackOverflow question](http://stackoverflow.com/questions/18718232/when-should-i-write-my-functions-in-curried-form/18721711)
the poster asked about the difference between _tupled_ and _curried_ form of a function in F#. 
In F#, you can use pattern matching to easily define a function that takes a tuple as an argument.
For example, the poster's function was a simple calculation that multiplies the number 
of units sold _n_ by the price _p_:
*)

let salesTuple (price, count) = price * (float count)

(**
The function takes a single argument of type `Tuple<float, int>` (or, using the nicer F# notation
`float * int`) and immediately decomposes it into two variables, `price` and `count`. The other
alternative is to write a function in the _curried_ form:
*)

let salesCurried price count = price * (float count)

(**
Here, we get a function of type `float -> int -> float`. Usually, you can read this just as a 
function that takes `float` and `int` and returns `float`. However, you can also use _partial
function application_ and call the function with just a single argument - if the price of
an apple is $1.20, we can write `salesCurried 1.20` to get a _new_ function that takes just
`int` and gives us the price of specified number of apples. The poster's question was:

> So when I want to implement a function that would have taken _n > 1_ arguments, 
> should I for example always use a curried function in F# (...)? Or should I take 
> the simple route and use regular function with an n-tuple and curry later on 
> if necessary?

You can see [my answer on StackOverflow](http://stackoverflow.com/questions/18718232/when-should-i-write-my-functions-in-curried-form/18721711#18721711).
The point of this short introduction was that the question inspired me to think about how
the world looks from the C# perspective...

To curry or not to curry?
-------------------------

I will not repeat the whole answer in the blog post. The key idea is that you should use
tuple when the tuple has some _logical meaning_. For example, if you have a function that
takes a range or 2D coordinates, it makes sense to use `float * float`.

This makes sense because you can then nicely compose multiple functions that work with 
ranges. For example, let's say we have a function `normalizeRange` and `expandRange`:
*)

let normalizeRange (lo, hi) =
  if lo > hi then (hi, lo) else (lo, hi)

let expandRange offset (lo, hi) =
  (lo - offset, hi + offset)

(**
Now we can easily write code that takes some range, normalizes it and expands it by 10:
*)

expandRange 10 (normalizeRange(50, 30))
// [fsi:val it : int * int = (20, 60)]

(**
So, if your tuple has some logical meaning, taking tuple as an argument leads to more 
composable code and makes it easier to understand. On the other hand, if there is no 
logical connection, it is better to use the curried form - this makes it possible to 
use partial function application.

How about tuples in C#?
-----------------------

In C#, we can work with tuples using the `Tuple<T1, T2, ...>` family of types. This is
certainly possible, but it is not particularly convenient, because you need to write 
the long type name repeatedly (you can use `var` inside method, but not in the method
declaration).

However, there is another place where tuples appear in C# - it is perfectly reasonable
to treat all .NET methods as functions that take a single tuple as the input and return
some other type as the result. This is how .NET methods look when you call them from 
F#:

*)

Math.Round(4.5, MidpointRounding.ToEven) 

(**

We do not usually think about this as a tuple - it is just a method call - but what if 
C# had (in [some future version](http://visualstudio.uservoice.com/forums/121579-visual-studio/suggestions/2405699-build-list-dictionary-and-tuple-into-the-language)) 
syntactic support for tuples and let you write `(42, "Hello world")` to create a tuple 
value of type `Tuple<int, string>`?

How many tuple types are there in .NET?
---------------------------------------

This inspired me to do a quick analysis of the standard .NET libraries to have a look
at the tuples that standard .NET methods take. How many of them follow the good practice 
and take a tuple that actually means something? And how many of them should instead use
the curried form, because the tuple has no logical meaning?

Checking the logical meaning will be difficult, but we can see how many of the tuples 
are used by more than one or two methods. If they are used in multiple places, it 
likely means that they represent some common pattern or some common single-purpose
data structure. 

This is pretty easy analysis to do using F# Interactive. Let's first look at all the types
in the current `AppDomain` (this uses assemblies that are loaded by default in F# - so 
nothing fancy). We also only look at "mscorlib" and "System" assemblies:
*)
open System
open System.Reflection

// Get all types in currently loaded assemblies
let types = seq {
  for asm in AppDomain.CurrentDomain.GetAssemblies() do
    if asm.FullName.StartsWith("System") || 
       asm.FullName.StartsWith("mscorlib") then
      yield! asm.GetTypes() }

types |> Seq.length

(**
The code is a simple _sequence expression_ that iterates over all assemblies and 
yields all types. On my machine, this gives us some 17000 types. Now, let's get a 
list with all tuples - we'll iterate over all methods in each type and generate a 
list with the names of parameter types. We skip all methods with less than 2 parameters:

*)
let tuples = seq { 
  for typ in types do
    // Get declared, public, both instance and static methods
    let flags = BindingFlags.DeclaredOnly ||| BindingFlags.Public |||
                BindingFlags.Static ||| BindingFlags.Instance
    let methods = typ.GetMethods(flags)
    // Generate tuples with parameters types for each method
    for meth in methods do
      let pars = meth.GetParameters()
      if pars.Length > 1 then
        yield [ for p in meth.GetParameters() -> p.ParameterType.FullName ] }

tuples |> Seq.length

(**
So, on my machine there are 16463 methods in .NET that take some tuple as an argument.
Now, the question is, how many of them are used repeatedly? We can easily group the 
tuples by the list of strings (F# implements structural comparison, so this is easy to do),
calculate the counts for each group and sort the results:
*)
let counts =
  tuples 
  |> Seq.groupBy id
  |> Array.ofSeq
  |> Array.map (fun (k, vs) -> k, Seq.length vs)
  |> Array.sortBy snd
  |> Array.rev


(**

Most common tuples in .NET
--------------------------

If we run `Seq.length counts`, we get 5805 as the result. This means that there are 5 thousand
distinct tuples (among roughly 15 thousand different methods). That certainly does not look like
most of them have some logical connection. But some of the top ones certainly do - here are the
top 8 (ignoring generics) with their counts:

  1. `string * string` (714) - looks like many methods take two strings - not sure if there
     is any logical meaning, but there probably are a few common uses
  2. `byte[] * int * int` (341) - this one looks like an array with offset and length - clearly
     this is a nice tuple with logical meaning
  3. `int * int` (327) - similar to two strings
  4. `object * object` (180) - hmm, maybe .NET likes untyped API :-)
  5. `int * object` (165) - I was a bit puzzled by this one, so I checked the methods that
      use this type. Good old untyped collections from the .NET 1.0 days!
  6. `char[] * int * int` (159) - similarly to the number 2, another nice logical tuple!
  7. `string * string * string` (156) - wow, so many methods take 3 strings
  8. `ITypeDescriptorContext * Type` (152) - huh??

How many are actually useful?
-----------------------------

It looks like there is quite a few tuple types that actually mean something useful. But what
is the distribution? Let's use [the FSharp.Charting](http://fsharp.github.io/FSharp.Charting/)
library to draw a quick chart that draws a column chart plotting the counts for every single
of the 5000 tuple types:
*)
#load "..\packages\FSharp.Charting.0.84\FSharp.Charting.fsx"
open FSharp.Charting

Chart.Column(Seq.map snd counts).WithYAxis(Log=true)

(**
If you create a chart using just `Chart.Column`, then you will not see very much - the number
of counts drops very quickly from the high numbers that we've seen for the first 10 types. 
But if we make the Y scale logarithmic (a good way to create misleading charts!) then we
can actually see something:

<div style="text-align:center;margin:0px 0px 10px 0px">
<img src="chart.png" alt="How many times are common tuples used in .NET?" />
</div>

The cart shows that a vast majority of tuples are used less than 10 times and only 2000
(of some 5000) are used more than once. The analysis based on just the number of occurrences
is definitely not precise, but let's say that tuples which are used more than 10 times 
are useful and those that are used more than 3 times are possibly useful. We can then
easily draw a chart showing the proportions:

*)

counts 
|> Seq.countBy (fun (k, v) -> 
    if v <= 2 then "Useless"
    elif v <= 10 then "Maybe useful"
    else "Useful")
|> Chart.Doughnut

(**
This snippet gives us the following nice chart (I tweaked the look a bit - a nice feature
of F# chart is that you can use `Ctrl+G` to open a property grid and change the fonts 
rather than doing everything from code):

<div style="text-align:center;margin:0px 0px 10px 0px">
<img src="chart2.png" alt="Proportion of useful tuple types in .NET" />
</div>

Surely, this is ridiculous!
---------------------------

Yes, I can hear that. I'm comparing incomparable here - it does not make sense to look at 
.NET libraries as if they were F# libraries and then claim that they are poorly designed.
The new version of my blog does not even have comments, but you can still [argue with me
on Twitter](https://twitter.com/tomaspetricek).

But before doing that - I'm not trying to criticise the design of .NET libraries in any way.
If your only option is to define a method that takes parameters "as a tuple" then that's 
the way to go. I'm certainly not suggesting that .NET should use curried form using 
`Func<T1, ...>` delegates or that people should use `Tuple<T1, ...>` instead of ordinary
methods.

This article is merely a thought experiment with some interesting analysis of .NET types.
We can see that there are a few "natural tuples" in .NET library design (like 
`byte[] * int * int`) but the parameters of a majority of methods do not logically form
a tuple.

So, is it better to use languages that make a clear distinction between (curried) functions
and functions taking a tuple? I think so - it makes it easier to write composable code
(by writing functions that take and return simple "ad-hoc" types as tuples) and it gives
you an easy way of grouping related types. There is no class representing _array range_ in
.NET because adding an entire class for this would be over-kill. A simple type like tuple
(supported by the language) makes this perfectly possible. On the other hand, you need to
think more carefully about library design to make sure that you use tuples correctly. 

*)
