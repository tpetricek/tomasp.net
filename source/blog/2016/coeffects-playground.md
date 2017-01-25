Coeffects playground: Interactive essay based on my PhD thesis
==============================================================

 - date: 2016-04-12T15:33:52.3133917+01:00
 - description: In my PhD thesis, I worked on integrating contextual information into a type system of functional programming languages. Most of the work is on theory, but I wanted to make it more accessible. To do this, I built an interactive essay where you can write, run, type check and parse simple context-aware programs and learn more about the (dry) theory in a more fun way.
 - layout: post
 - image: http://tomasp.net/blog/2016/coeffects-playground/coeffects.png
 - tags: coeffects,research,functional programming,programming languages
 - title: Coeffects playground: Interactive essay based on my PhD thesis
 - url: 2016/coeffects-playground

--------------------------------------------------------------------------------
 - standalone

In my [PhD thesis](http://tomasp.net/academic/theses/coeffects/), I worked on integrating
_contextual information_ into a type system of functional programming languages. For example,
say your mobile application accesses something from the environment such as GPS sensor or your
Facebook friends. With _coeffects_, this could be a part of the type. Rather than having type
`string -> Person`, the type of a function would also include resources and would be
`string -{ gps, fb }-> Person`. I wrote [longer introduction to coeffects](http://tomasp.net/blog/2014/why-coeffects-matter/) on this
blog before.

As one might expect, the PhD thesis is more theoretical and it looks at other kinds of contextual
information (e.g. past values in stream-based data-flow programming) and it identifies abstract
_coeffect algebra_ that captures the essence of contextual information that can be nicely tracked
in a functional language.

I always thought that the most interesting thing about the thesis is that it gives people a nice way
to think about _context_ in a unified way. Sadly, the very theoretical presentation in the thesis
makes this quite hard for those who are not doing programming language theory.

To make it a bit easier to explore the ideas behind coeffects, I wrote a coeffect playground
that runs in a web browser and lets you learn about coeffects, play with two example context-aware languages,
run a couple of demos and learn more about how the theory works. Go [check it out now](http://tomasp.net/coeffects/)
or continue below to learn more about some interesting internals!

<a href="http://tomasp.net/coeffects/">
<img src="http://tomasp.net/blog/2016/coeffects-playground/screenshot.png" style="width:90%; margin:0px 5% 10px 5%" />
</a>

--------------------------------------------------------------------------------


In my [PhD thesis](http://tomasp.net/academic/theses/coeffects/), I worked on integrating
_contextual information_ into a type system of functional programming languages. For example,
say your mobile application accesses something from the environment such as GPS sensor or your
Facebook friends. With _coeffects_, this could be a part of the type. Rather than having type
`string -> Person`, the type of a function would also include resources and would be
`string -{ gps, fb }-> Person`. I wrote [longer introduction to coeffects](http://tomasp.net/blog/2014/why-coeffects-matter/) on this
blog before.

As one might expect, the PhD thesis is more theoretical and it looks at other kinds of contextual
information (e.g. past values in stream-based data-flow programming) and it identifies abstract
_coeffect algebra_ that captures the essence of contextual information that can be nicely tracked
in a functional language.

I always thought that the most interesting thing about the thesis is that it gives people a nice way
to think about _context_ in a unified way. Sadly, the very theoretical presentation in the thesis
makes this quite hard for those who are not doing programming language theory.

To make it a bit easier to explore the ideas behind coeffects, I wrote a coeffect playground
that runs in a web browser and lets you learn about coeffects, play with two example context-aware languages,
run a couple of demos and learn more about how the theory works. Go [check it out now](http://tomasp.net/coeffects/)
or continue below to learn more about some interesting internals!

<a href="http://tomasp.net/coeffects/">
<img src="http://tomasp.net/blog/2016/coeffects-playground/screenshot.png" style="width:90%; margin:0px 5% 10px 5%" />
</a>

Interactive essays
------------------

The [coeffects playground](http://tomasp.net/coeffects/) is something between an article about coeffects and an
implementation of a simple coeffect language - it guides you through an explanation (as an article would), it
lets you run examples, but it also lets you write your own little programs and run them or type check them and
explore the typing derivations.

Probably the best explanation of what my interactive coeffect playground tries to offer is from
Bret Victor's essay called [Explorable Explanations](http://worrydream.com/ExplorableExplanations):

> An active reader asks questions, considers alternatives, (...) tries to
> generalize specific examples, and devise specific examples for generalities.
> An active reader doesn't passively sponge up information, but uses the
> author's argument as a springboard for critical thought and deep understanding.
>
> Do our reading environments encourage active reading? Or do they utterly
> oppose it? A typical reading tool, such as a book or website, displays the
> author's argument, and nothing else. The reader's line of thought remains
> internal and invisible, vague and speculative. We form questions, but can't
> answer them. We consider alternatives, but can't explore them.
>
> <p style="text-align:right;font-style:italic;">Bret Victor, <a href="http://worrydream.com/ExplorableExplanations">Explorable Explanations</a></p>

Bret Victor also has a nice sample showing the ideas of Explorable Explanations on the [climate
change](http://worrydream.com/TenBrighterIdeas) and a [network science
paper](http://worrydream.com/#!/ScientificCommunicationAsSequentialArt).
For network science or climate change, you can imagine interesting visualizations and how changes
of parameters done by the reader can affect them. For presenting coeffects, I had to figure out how
to use similar ideas in a fairly theoretical environment. Programming language theory papers usually
have just a couple of typing rules or translation rules written as dry mathematical equations.

Quite surprisingly, I actually learned about Bret Victor's nice demos only later when I started working
on my demo. What made me think about different ways of presenting academic research earlier was Robert Pirsig's
[Zen and the Art of Motorcycle Maintenance](http://www.amazon.com/Zen-Art-Motorcycle-Maintenance-Inquiry/dp/0060589469)
and [The Medium is the Massage](http://www.amazon.com/Medium-Massage-Marshall-McLuhan/dp/1584230703/ref=mt_paperback?_encoding=UTF8&me=)
by Marshall McLuhan and Quentin Fiore. Finally, it was also the [Future Programming Workshop](http://www.future-programming.org/)
organized by Jonathan Edwards. (Thanks!)

Playing in the playground
-------------------------

The playground has a JavaScript implementation (not _written_ in JavaScript, of course!) of a simple
context-aware programming language. This includes a parser, type checker, translator (which turns your
source code into a simple _desugared_ version) and an interpreter for the target language, so pretty
much all you'd need in a real implementation. But those components are used in a different way than how
you would use them in a real-world implementation. The playground lets you "look under the cover" in a
number of ways.

### Run your own small programs

<img src="ia-run.png" style="float:right;margin:5px 0px 10px 10px" />

You can run a couple of samples or your own programs. This type-checks your code (shows you the type),
infers the required context and generates a simple UI where you can enter the contextual information
(required _implicit parameters_ or _past values_). Then it translates your source code into the simpler
target language and runs the interpreter. This is the closest view to "normal" running of a program.

<div style="clear:both"></div>

### Interactive data-flow demo

<img src="ia-mouse.png" style="float:right;margin:5px 0px 10px 10px" />

One of the context-aware languages is a simple data-flow language where you can write programs over
streams. In addition to a simple demo where you just enter N past values by hand in textbox, the essay
offers a more fun version - where last N values are taken from X and Y coordinates of your mouse moves!

<div style="clear:both"></div>

### Explorable typing derivations

<img src="ia-tc.png" style="float:right;margin:5px 0px 10px 10px" />

The most interesting new thing in my thesis is a _coeffect type system_ that tracks the contextual information
about programs in the types of functions. But when you run a type checker, you usually just see "Good" or "Bad"
as the result! In the essay, you can explore the typing derivation - the reasoning that the algorithm did to
prove that your program is well-typed. If you see a typing judgement, click on the assumptions to go up or
on the conclusions to go down!

<div style="clear:both"></div>

### Comonadic language translation

<img src="ia-transl.png" style="float:right;margin:5px 0px 10px 10px" />

Programs you write may contain special _contextual_ language constructs like `?fst` (implicit parameter) or
`prev` (past value in data-flow). The way programs run is that they are translated to a simpler target language
without those special constructs (a bit like how the [do notation for monads works](https://en.wikibooks.org/wiki/Haskell/do_notation)).
The playground exposes this part too and you can see the translated version of your source programs.

<div style="clear:both"></div>

### More essay features

<img src="ia-more.png" style="float:right;margin:5px 0px 10px 10px" />

In addition to the interactive parts, I also experimented with a few additional presentation options in the
essay. When you come, some parts of essay are hidden and you have to explicitly reveal them - the idea is
that when you just skim read the essay, you should see the most important parts. There are also a couple of
mini-slide decks that compare coeffects with effects.

<div style="clear:both"></div>

I have to say, writing an interactive essay is probably more work than writing an academic paper about
the work, but I think it makes a dry topic like programming language theory much easier to understand.
After exploring the playground, you should see a pop-up with a [brief
survey](https://docs.google.com/forms/d/17iptGK2LBtAkYLIi-g7bGesdFa9-ZVhFoFpzJhn1Bk8/viewform), so
please use it to give me feedback about the project! I'm curious to see what people think about this
way of presenting programming language research!

Behind the scenes
-----------------

    [hide]
    type Coeffect = Coeffect
    type Var = Var

All the code in the essay runs as JavaScript on the client-side, but I'm not crazy enough to write
a parser and type checker for a language in JavaScript, so I used F# and [FunScript](http://funscript.info/)
to compile F# into JavaScript. All source code [is available on GitHub](https://github.com/coeffects/coeffects-playground),
so feel free to have a look there!

Thanks to F# and FunScript, the whole implementation including parser, type checker, translator,
interpreter and even most of the user interface are only some 2800 lines of code. It compiles to
46000 lines of ugly JavaScript - partly because of advanced F# features like generic and pattern
matching and partly because FunScript does not try to optimize much. (I'm quite curious to see
how [Fable](https://github.com/fsprojects/Fable) or [WebSharper](http://websharper.com/) would compare!)

There is a number of really nice things that make the implementation so short and simple.

### Algebraic data types

Any general purpose programming language should have a way of defining _algebraic data types_. In
F# those are called _discriminated unions_ and they let us define what expressions and types look
like in the context-aware programming language that you can use in the playground:

    type Expr =
      // Variable, constants, functions & application
      | Var of string
      | Number of float
      | Fun of Var * Typed
      | App of Typed * Typed
      // Special contextual language features
      | Prev of Typed
      | QVar of string
      | (*[omit:(other cases omitted)]*)(other cases omitted)(*[/omit]*)

    and Type =
      | Primitive of string
      | Func of Coeffect * Type * Type
      | (*[omit:(other cases omitted)]*)(other cases omitted)(*[/omit]*)

    and Typed = Type * Expr

I simplified the definition a bit, but it shows the idea. The `Expr` type captures different kinds
of expressions. For example `(fun x -> x) 10` is parsed as an application `App` with `Fun` on the
left-hand side and `Number` on the right-hand side. There are two additional constructs for the
context-aware language features. `QVar` models special `?foo` variables and `Prev` lets you access
previous value in data-flow computations such as `(x + prev x) / 2`.

The `Type` is interesting too. There is `Primitive` for primitive types like `num` and a `Func` type
representing a function. You can immediately see that a function is not just `T1 -> T2` with two types,
but that it also carries a `Coeffect` annotation with information about context that needs to be
available when you run the function.

### Parser combinators

    [hide]
    aa <*> bb

To parse the source code into `Expr` values, I wrote a minimal parser combinator library (sadly,
I cannot use [FParsec](http://www.quanttec.com/fparsec/), because it needs to compile to JavaScript).
The parsing is still done in two steps - we first turn a sequence of characters into a sequence of
tokens and then turn tokens into `Expr` value. This makes the parsing code easy to read and write.
For example, `fun x -> ...` and `prev ...` are parsed like this:

    let func =
      ( token Token.Fun <*>
        ident <*>
        token Token.Arrow <*>
        expr  )
      |> map (fun (((_, var), _), body) ->  
          Expr.Fun(var, body))

    let prev =
      ( token Token.Prev <*>
        term () )
      |> map (fun (_, body) ->
          Expr.Prev(body))


The `<*>` operator represents sequential composition. For `func`, we need to parse the `fun` keyword
followed by an identifier, `->` token and an expression. The expression to access previous value of
a data stream, we just write `prev` followed by a term. (There is a distinction between expressions
and terms to deal with function application and operators. The parser also deals with operator
precedence and things like that.)

### Type checking and constraint solving

The next part of the code is [the type checker](https://github.com/coeffects/coeffects-playground/blob/master/typechecker.fs#L149).
This walks over the parsed `Expr` value, annotates
everything with types and generates _constraints_. Initially, a lot of the types are variables
(think `'a'` in F# generics) and constraints specify equalities. For example, if you write
`(fun x -> x) 5`, the type of the function is `'a -> b'` and there will be constraints `'a = 'b`
(generated from the body) and `'a = num` (generated from the application).
Similar constraints are also collected about the coeffect annotations.

With F#, the constraint resolution becomes just a recursive loop with pattern matching:

    let rec solve constraints assigns cequals =
      match constraints with
      | [] -> assigns, cequals
      | (l, r)::rest when l = r -> solve rest assigns cequals
      | (Type.Func(c1, l1, r1), Type.Func(c2, l2, r2))::rest ->
          let constrints = (l1, l2)::(r1, r2)::rest
          solve constraints assigns ((c1, c2)::cequals)
      | (*[omit:(other cases omitted)]*)(other cases omitted)(*[/omit]*)

If there are no constraints left (first case), we are done and we return the results. If
there is a constraint `t = t` (with both things equal), we can drop it. The last case is
the most typical one - if we have two functions, `l1 -> l2` and `r1 -> r2`, we turn the
constraint `(l1 -> l2) = (r1 -> r2)` into a pair of constraints `l1 = r1` and `l2 = r2`
and we also collect a new equality requirement for coeffect annotations. One of
the omitted case (for variables) then collects the final `assigns` which maps variable
names to types. (In the final version, I sadly had to turn this into a more explicit
loop, because JavaScript does not do tail-calls...).

### Translation to target language

When you run a coeffect program, it is parsed, type checked and then it is translated to
a target language. This replaces the special constructs like `Expr.Prev` and `Expr.QVar`
into expressions that pass the context around explicitly in ordinary variable. For details,
see the essay. The translation is, again, just a recursive function with pattern matching.
Two cases look like this:

    let rec translate ctx e =
      match e with
      | Expr.Number(n) -> Expr.Number(n)
      | Expr.Var(v) -> Expr.Builtin("counit") |@| ctx
      | (*[omit:(more cases omitted)]*)(more cases omitted)(*[/omit]*)

The function takes `ctx` which is an expression that represents a newly generated variable
that keeps the context. If the expression `e` is a number, we just turn it into a number.
When translating a variable access `Var(v)`, we produce an expression that calls a special
_counit_ operation on the context carried by the variable represented by `ctx`. (The
`|@|` operator is just a custom operator that creates `Expr.App` value).

### And even the user interface

Although I mainly used F# for the language implementation part, I also used FunScript
jQuery bindings to write some of the user interface. For example when you are looking
at a typing derivation, you can click on parts of the typing judgement to navigate through the
derivation. This is handled by code that looks roughly like this:

    /// Remembers the current path
    let locations = ref []

    /// Generate path for the specified 'jo' element
    let getNewPath (jo:JQuery) =
      (*[omit:(Implementation omitted)]*)(Implementation omitted)(*[/omit]*)

    jq("#typing .mtable")
      .on("click", fun e o ->
        jq("#typing .p-span")
          .css("backgroundColor", "transparent")
        locations.Value <- snd (getNewPath (jthis()))
        typeset ()

The `locations` variable keeps the current path that the viewer followed (if you click on
the first assumption of the first judgement and then on the second assumption of the second
judgement, this will be a list `[0; 1]`). The `jq` function is an F# wrapper for writing
`$("...")` in JavaScript and `jthis` is a special wrapper for writing `$(this)` which is
not directly available in F#.

Summary
-------

In the case of coeffect playgrounds, I did the interactive essay after writing the thesis, but I
think this way of presenting research is not just about giving additional and "easier to understand"
presentation. It actually changes which part of the research problem become important.

When writing papers, you have to extract minimal calculus that can be neatly presented in a
paper. When writing interactive essays, you instead have to extract some core aspects of the
functionality that you're creating to convince people that this is interesting. This means that you
also start thinking differently about the problem. I actually wrote about this briefly in
[earlier post about programming languages and philosophy of science](http://tomasp.net/blog/2014/philosophy-pl/).

Most of the features in the interactive essay are really experiments based on what I thought
could be useful when learning about _coeffects_. I would really like to hear from you - do you
think this is useful? Should every PhD thesis come with an interactive essay? There is a [brief
survey you can fill on the page](https://docs.google.com/forms/d/17iptGK2LBtAkYLIi-g7bGesdFa9-ZVhFoFpzJhn1Bk8/viewform)
or contact me directly at [@tomaspetricek](http://twitter.com/tomaspetricek) or
[tomas@tomasp.net](mailto:tomas@tomasp.net).
