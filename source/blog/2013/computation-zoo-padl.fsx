(**

The F# Computation Expression Zoo (PADL'14)
=============================================

 - date: 2013-11-08T06:42:56.7559875+00:00
 - description: A month or so back, I wrote a tweet saying that F# computation expressions are amazing and no other language has anything like that. I can finally provide all the evidence by linking to a PADL 2014 paper about them.
 - layout: article
 - tags: haskell,research,f#,functional programming
 - title: The F# Computation Expression Zoo (PADL'14)
 - url: 2013/computation-zoo-padl

--------------------------------------------------------------------------------
 - standalone


F# [computation expressions](http://msdn.microsoft.com/en-us/library/dd233182.aspx) are the 
syntactic language mechanism that is used by features like sequence expressions and asynchronous
workflows. The aim of F# computation expressions is to provide a _single_ syntactic mechanism
that provides convenient notation for writing a wide range of computations.

The syntactic mechanisms that are unified by computation expressions include Haskell `do` 
notation and list comprehensions, C# iterators, asynchronous methods and LINQ queries,
Scala `for` comprehensions and Python generators to name just a few.

Some time ago, I started working on an academic article to explain what makes computation
expressions unique - and I think there is quite a few interesting aspects. Sadly, this is 
often not very well explained and so the general perception is more like this...


--------------------------------------------------------------------------------
*)
(***hide***)
open System
open System.Collections.Generic

type Imperative<'T> = unit -> option<'T>

type ImperativeBuilder() = 
  member x.Combine(a, b) = (fun () ->
    match a() with 
    | Some(v) -> Some(v) 
    | _ -> b() )
  member x.Delay(f:unit -> Imperative<_>) : Imperative<_> = (fun () -> f()())
  member x.Return(v) : Imperative<_> = (fun () -> Some(v))
  member x.Zero() = (fun () -> None)
  member x.Run(imp) = 
    match imp() with
    | Some(v) -> v
    | _ -> failwith "Nothing returned!"
  member x.For(inp:seq<_>, f) =
    let rec loop(en:IEnumerator<_>) = 
      if not(en.MoveNext()) then x.Zero() else
        x.Combine(f(en.Current), x.Delay(fun () -> loop(en)))
    loop(inp.GetEnumerator())
  member x.While(gd, body) = 
    let rec loop() =
      if not(gd()) then x.Zero() else
        x.Combine(body, x.Delay(fun () -> loop()))
    loop()

let imperative = new ImperativeBuilder()  
let blockedWords = [""]
(**
F# [computation expressions](http://msdn.microsoft.com/en-us/library/dd233182.aspx) are the 
syntactic language mechanism that is used by features like sequence expressions and asynchronous
workflows. The aim of F# computation expressions is to provide a _single_ syntactic mechanism
that provides convenient notation for writing a wide range of computations.

The syntactic mechanisms that are unified by computation expressions include Haskell `do` 
notation and list comprehensions, C# iterators, asynchronous methods and LINQ queries,
Scala `for` comprehensions and Python generators to name just a few.

Some time ago, I started working on an academic article to explain what makes computation
expressions unique - and I think there is quite a few interesting aspects. Sadly, this is 
often not very well explained and so the general perception is more like this:

<img src="tweets.png" style="margin:10px 0px 30px 40px;" alt="Proof by Tweet!" />

This is something hat we tried to clarify in the upcoming PADL 2014 paper which explains
how computation expression fit with standard abstract computation types _such as_ monads
and shows what makes them unique. If you're interested in the details, then follow the link 
to the paper below. In the rest of the blog post, I'll try to give a quick summary with one 
interesting example...

 * [The F# Computation Expression Zoo](http://tomasp.net/academic/papers/computation-zoo/) - To appear in PADL 2014
 * [Source with all examples from the paper](http://tryjoinads.org/computations) - on TryJoinads.org

Why computation expressions are more?
-------------------------------------

There are two key things about computation expressions that make them quite different
to what other languages provide - the first is emphasis on syntax (to make them look like
single-purpose language features) and the second is flexibility (to make them reusable).

### Syntax

Computation expressions reuse the normal syntax of the F# language, so 
you can write code using standard constructs like `let`, `for` and `try` .. `with`,
but with a different semantics, depending on the kind of computation. For example, 
`try` .. `with` inside `async` captures exceptions that happen on another thread and
propagates them, but all of this is done using standard language mechanism.

### Flexibility

Computation expressions are not tied to a single abstract type of 
computations (e.g. monad). Instead, they give us a single syntactic mechanism, that can
give a nice syntax for multiple different abstract computation types. These include
monads, but also applicative functors (using a research extension), monoids, additive 
monads, computations constructed using monad transformers and more.

But what's the point? Aren't _monad transformers_ and _additive monads_ just _monads_?
Of course, they are monads, but they have some additional structure. For example, additive
monads have the `mplus` operation of type `'a m -> 'a m -> 'a m` and `mzero` of type `'a m`.
In Haskell, you can just use the `do` notation, but to access the additional structure, 
you'll need to use the combinators, which can make the syntax more complex.

When using computation expressions, the operations like `mplus` and `mzero` become
_a part of the syntax_. In many cases, you will not need to call them explicitly via
a function, because they _enable_ additional syntax in the computation expression that
calls them.

### Limitations

To be fair, I should mention common criticism of computation expressions too.
F# does not easily let you write code that is polymorphic over the type of computation
(it can be done, but it is not particularly idiomatic and it is certainly not recommended).

If you learned about monads in Haskell, this might sound like a major issue to you.
However, it is not a big problem in practice, because F# and Haskell use monads (and
other computations) in very different ways. In Haskell, they are fundamental part of the
language and you need them all the time. In F#, they are only used when you actually 
want to write some concrete computation with non-standard behaviour. For example, asynchronous
computation or a formlet - in such case, you will almost always want to work with a 
concrete computation type. As we'll see later, this has another useful consequence - 
it means that the syntax of computation expressions can be tuned to a concrete use case.

Case study: Two additive monads
-------------------------------

If you want to see all the abstractions that are supported by computation expressions,
then [read the paper](http://tomasp.net/academic/papers/computation-zoo/). I will not 
repeat everything in this blog post.

However, I want to show one example that demonstrates the key point of computation expressions
very well. The example is encoding of _additive monads_ - that is, computation types that
are monads and have additional operations `mplus : 'a m -> 'a m -> 'a m` and `mzero : 'a m`.
In Haskell, these are captured by the `MonadPlus` type class.

I'll show how computation expressions look for two different computation. The first one
is based on the list monad (although I'll use more idiomatic F# `seq<'T>` computations)
and the other is essentially a delayed `option<'T>` type (aka Maybe monad), but it more
closely models the Haskell `IO` instance for `MonadPlus`.

### Sequence expressions

Let's say that we want to write a function that takes a list, such as `[1; 2; 3]` and
duplicates each element in the list returning `[1; 1; 2; 2; 3; 3]`. The syntax for doing
this is very close to the one of [Python generators](http://freepythontips.wordpress.com/2013/09/29/the-python-yield-keyword-explained/)
and looks as follows:
*)
let duplicate input = seq { 
  for num in input do
    yield num 
    yield num }
(**
How does this work? The `for` keyword is mapped to the _bind_ operation of the underlying
computation and the `yield` keyword is mapped to the _return_ operation of the monad.
When the body contains multiple statements that return a value, the statements are combined
using the additive operation (`mplus`).

The syntax and semantics of the computation is determined by the `seq` identifier - this
is not actually an identifier, but an _object_ that defines a number of members that tell
the compiler that 1) it should enable `for`, `yield` and multiple statements in a body
and 2) what is the implementation of the three underlying operations.

When defining the syntax in the `seq` value, we could have chosen another syntax - we could
use `let!` and `return` instead of using `for` and `yield`, but the latter option gives a
more convenient and intuitive syntax. 

What do I mean by "more intuitive"? Let's look at the laws! The [_left distribution_ 
law](http://www.haskell.org/haskellwiki/MonadPlus) about `MonadPlus` (which holds for lists) 
states that the following two computations should be equivalent:
*)
let left in1 in2 f = seq { 
  for x in in1 do yield! f x
  for x in in2 do yield! f x }

let right in1 in2 f = seq { 
  for x in seq { yield! in1; yield! in2 } do
    yield! f x }
(**
This looks like a very reasonable law to require for any sequence or list-like computation.
So, if you have a list-like computation, then this is the right syntax to use. For example,
[asynchronous sequences](http://www.haskell.org/haskellwiki/MonadPlus) use it too.

However, what if we have a computation where the above law does not hold?

### Imperative computations

Another example of additive monad are imperative computations 
that I discussed in [earlier article on this blog](http://tomasp.net/blog/imperative-i-return.aspx/).
The aim is to let you write computations that are similar to C-like imperative control flow
with `return`, `break` and `continue` keywords. The following is a simple example of using
the imperative-style `return` - assuming we have a list of `blockedWords`, we  want to 
return `true` when the given message contains any word.

Of course, this could easily be written using a built-in function like `List.exists`, but that's 
not the point - the point is that we can define computation expression that closely models 
the C-like syntax and could be used to define more complex computations and implement
more complex behaviours:

*)
let blockMessage (msg:string) = imperative {
  for word in blockedWords do
    if msg.Contains(word) then return true
  return false }
(**

Again, the syntax and the semantics is determined by the `imperative` object. You can 
find the details of the implementation in the [blog post mentioned earlier](http://tomasp.net/blog/imperative-i-return.aspx/).
The key thing is that the object determines that we want to use the `return` keyword
for the _return_ operation of the monad. 

The `for` loop used here is just an ordinary
loop that iterates over sequence - it is defined in terms of standard iteration over
lists and the _bind_ operation of the monad. This is an example of the flexibility
of computation expressions - because they are used with _specific_ computations, it makes
sense to design them so that they give us the most intuitive syntax (here, resembling
C-like imperative computations).

Interestingly, imperative computations do not obey the _left distribution_ law that worked
for sequences, but instead, they obey the [_left catch_ law](http://www.haskell.org/haskellwiki/MonadPlus).
The law statest that the following two computations are equivalent:
*)
let left = imperative {
  return 0
  printfn "Unreachable!" }

let right = imperative {
  return 0 }
(**
In the original formulation of the law, the expression following `return 0` in the left
expression can be anything - but calling `printfn` is completely sufficient for this
quick overview.

It is not surprising that the law holds for imperative computations. So again, the law
nicely accompanies the syntax that we have chosen when defining the imperative computation
builder (the `imperative` value). If we used the `yield` keyword in this example, or
if we used `return` in the previous one (sequences), then there would be a mismatch between
the syntax and the laws. 

### MonadPlus versus MonadOr

Interestingly, the distinction between the two sample computations that I used in the 
previous examples exists in Haskell too. The [MonadPlus reform proposal](http://www.haskell.org/haskellwiki/MonadPlus_reform_proposal)
suggests that computations satisfying _left distribution_ should form a type class 
`MonadPlus`, while computations satisfying _left catch_ should be captured by `MonadOr`.

Having two different type classes would mean that you'd explicitly use one of the 
two combinators - either `mplus` or `morelse` and so the resulting Haskell code would
be somewhat less generic over the type of computation and a bit closer to what you get with
F# computation expression encoding of _additive monads_.

I believe it is interesting that you can discover this important distinction from 
multiple directions - by considering the laws, or by looking at the most natural syntax 
for writing such computations.


Summary
-------

This blog post is a quick (and over-simplified) overview of what you can find in our
paper [The F# Computation Expression Zoo](http://tomasp.net/academic/papers/computation-zoo/)
that has been accepted at the PADL workshop 2014.

I tried to give two simple examples that demonstrate the main points:

 * **Syntax matters** - computation expressions aim to match with built-in language features
   like Python generators and so the ability to choose the right syntax is crucial. However,
   this is done without full support for macros. While macros can achieve this too, they tend
   to be too powerful.

 * **Expressivity** - the author of a computation should be able to decide how the computation
   looks and define more operations (e.g. to support multiple `yield` constructs) if the 
   computation type allows this.

Another point of this article is to show that computation expressions are not, in fact, 
just an _"enterprise-y name for monad"_. Firstly, they are based on quite different principles 
and, secondly, they can capture wider range of computations (including additive monads and
monad transformers) and _leverage_ the additional operations available in these computations.

The slogan of the last feature could be **require less, leverage more**. In the research 
extension available on [TryJoinads.org](http://tryjoinads.org), you can use computation 
expressions even if you have _just_ applicative functor. But if you have more - say, a monad
or even additive monad - then the additional structure enables more syntactic options and so
you do not need to resort to explicit use of combinators as often.

*)
