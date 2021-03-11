(**
Write your own Excel in 100 lines of F#
==============================================

 - date: 2018-11-12T12:58:50.3238452+00:00
 - description: Use Fable, Elmish architecture, parser combinators and the magic of functional-first 
     programming with F# to build a simple browser-based spreadsheet application! I have been teaching
     F# for 7 years now and while the libraries and examples change, the core ideas remain the same.
     In this blog post, I'll cover my latest favorite example. You will see how the pragmatic F# design
     makes it easy to integrate with the external world (such as the JavaScript ecosystem and the React
     library) and how the core functional features make your code elegant and composable.
 - layout: article
 - image: http://tomasp.net/blog/2018/write-your-own-excel/logo.png
 - tags: f#, functional, training, fable
 - title: Write your own Excel in 100 lines of F#
 - icon: fa fa-table
 - url: 2018/write-your-own-excel

---------------------------------------------------------------------------------------------------

I've been teaching F# for over seven years now, both in the public F# FastTrack course that we run 
at SkillsMatter in London and in various custom trainings for private companies. Every time I teach
the F# FastTrack course, I modify the material in one way or another. I wrote about some of this
interesting history [last year in an fsharpWorks article](#). The course now has a stable half-day 
introduction to the language and a stable focus on the ideas behind functional-first programming,
but there are always new examples and applications that illustrate this style of programming.

<img src="http://tomasp.net/blog/2018/write-your-own-excel/logo.png" class="rdecor" />

When we started, we mostly focused on teaching functional programming concepts that might be useful
even if you use C# and on building analytical components that your could integrate into a larger 
.NET solution. Since then, the F# community has matured, established the [F# Software Foundation](http://fsharp.org),
but also built a number of mature end-to-end ecosystems that you can rely on such as [Fable](http://fable.io),
the F# to JavaScript compiler, and [SAFE Stack](https://safe-stack.github.io) for full-stack web development.

For the upcoming December course in London, I added a number of demos and hands-on tasks built
using Fable, partly because running F# in a browser is an easy way to illustrate many concepts
and partly because Fable has some amazing functional-first libraries. 

> _<i class="fa fa-hand-o-right" style="font-size:110%;margin:0px 5px 0px 0px"></i>_
> If you are interested in learning F# and attending our course, the next [F# FastTrack](https://skillsmatter.com/courses/473-tomas-petricek-phil-trelford-fast-track-to-fsharp)
> takes place on **6-7 December** in London at SkillsMatter. We also offer custom 
> on-site trainings. Get in touch at [@tomaspetricek](http://twitter.com/tomaspetricek)
> or email [tomas@tomasp.net](mailto:tomas@tomasp.net) for a 10% discount for the course. 

One of the new samples I want to show, which I also [live coded at NDC 2018](https://vimeo.com/281241807), 
is building a simple web-based Excel-like spreadsheet application. The spreadsheet demonstrates
all the great F# features such as domain modeling with types, the power of compositionality 
and also how functional-first approach can be amazingly powerful for building user interfaces. 

---------------------------------------------------------------------------------------------------
*)
(*** hide ***)
#load "../packages/2018/write-your-own-excel/parsec.fs"
#I "../packages/2018/packages"
#r "NETStandard.Library/build/netstandard2.0/ref/netstandard.dll"
#r "Fable.Core/lib/netstandard2.0/Fable.Core.dll"
#r "Fable.Import.Browser/lib/netstandard1.6/Fable.Import.Browser.dll"
#r "Fable.React/lib/netstandard2.0/Fable.React.dll"
#r "Fable.Elmish/lib/netstandard2.0/Fable.Elmish.dll"
#r "Fable.Elmish.React/lib/netstandard1.6/Fable.Elmish.React.dll"
(**

<style>
  .excel { margin:0px auto 0px auto; font-family:sans-serif; display:inline-block }
  .excel table { border-spacing: 0px; border-bottom:1px solid #e0e0e0; border-right:1px solid #e0e0e0; }
  .excel td, .excel th { text-align:left; height:22px; width:65px; border-left:1px solid #e0e0e0; border-top:1px solid #e0e0e0; padding:5px; }    
  .excel td.selected { padding:0px; }
  .excel td input { width:65px; height:30px; }
</style>
<script>
  var spreadsheetConfig = 
    { "columns":"H",
      "rows":8,
      "cells": [
        ["B1", "1"], ["B2", "1"], ["B3", "=B1+B2"], ["B4", "=B2+B3"], ["B5", "=B3+B4"], ["B6", "=B4+B5"], ["B7", "=B5+B6"], ["B8", "=B6+B7"],
        ["D1", "1"], ["D2", "=D1*2"], ["D3", "=D2*3"], ["D4", "=D3*4"], ["D5", "=D4*5"], ["D6", "=D5*6"], ["D7", "=D6*7"], ["D8", "=D7*8"]
      ] }
</script>

What is a spreadsheet?
---------------------

The sample compiles to JavaScript, so the best way of explaining what we want to build is 
to give you a live demo you can play with! Since this is a blog post about functional programming,
I already implemented both Fibonacci numbers (column B) and factorial (column D) in the spreadsheet for you!

<script src="bundle.js"></script>
<div style="width:100%;padding-right:20px;text-align:center;"><div class="excel" id="main"></div></div>

You can click on any cell to edit the cells. To confirm your edit, just click on any other cell.
You can enter numbers such as `1` (in cell B1) or formulas such as `=B1+B2` in cell B3. Formulas
support parentheses and four standard numerical operators. When you make an edit, the spreadsheet
automatically updates. If you make a syntax error, reference empty cell or create a recursive 
reference, the spreadsheet will show `#ERR`.

> _<i class="fa fa-hand-point-right" style="font-size:110%;margin:0px 5px 0px 0px"></i>_
> Full source code is available in my [elmish-spreadsheet repository on GitHub](https://github.com/tpetricek/elmish-spreadsheet/)
> (as a hands-on exercise in `master` branch and fully working in the `completed` branch), but you
> can also play with it in the [Fable REPL](https://fable.io/repl/) (see Samples, Elmish, Spreadsheet),
> which lets you edit and run F# in the browser.

### Defining the domain model

Following the typical F# type-driven development style, the first thing we need to think about
is the domain model. Our types should capture what we work with in a spreadsheet application.
In our case, we have positions such as `A5` or `C10`, expressions such as `=A1+3` and the sheet
itself which has user input in some of the cells. To model these, we define types for `Position`, 
`Expr` and `Sheet`:

*)
type Position = char * int

type Expr = 
  | Number of int
  | Reference of Position
  | Binary of Expr * char * Expr

type Sheet = Map<Position, string>
(**
A `Position` is simply a pair of column name and a number. An expression is more interesting, 
because it is recursive. For example, `A1+3` is an application of a binary operator on sub-expressions
`A1`, which is a reference and `3` which is a numerical constant. In F#, we capture this nicely
using a discriminated union. In the `Binary` case, the left and right sub-expressions are themselves
values of the `Expr` type, so our `Expr` type is recursive.

The type `Sheet` is a map from positions to raw user inputs. We could also store parsed expressions or 
even evaluated results, but we always need the original input so that the user can edit it. To make
things simple, we'll just store the original input and parse it each time we need to evaluate the
value of a cell. To do the parsing and evaluation, we'll later define two functions:

    [lang=fsharp]
    val parse : string -> Expr option
    val evaluate : Expr * Sheet -> int option

We will talk about these later when we discuss the logic behind our spreadsheet, but writing the
type down early is useful. Given these types, we can already see how everything fits together. 
Given a position, we can do a lookup into `Sheet` to find the entered text, then we can parse it
using `parse` to get `Expr` and, finally, pass the expression to `evaluate` to get the resulting
value. We also see that both `parse` and `evaluate` might fail. The first one will fail if the 
input is not a valid formula and the second might fail if you reference an empty cell. 

Now, all we have to do is to keep writing the rest of Excel until the type checker is happy!

*)
(*** hide ***)
open Parsec
(*** define:parser1 ***)
let operator = char '+' <|> char '-' <|> char '*' <|> char '/'
let reference = letter <*> integer |> map Reference
let number = integer |> map Number

(*** define:parser2 ***)
let exprSetter, expr = slot ()
let brack = 
  char '(' <*>> anySpace <*>> expr <<*> anySpace <<*> char ')'
let term = number <|> reference <|> brack 
let binary = 
  term <<*> anySpace <*> operator <<*> anySpace <*> term 
  |> map (fun ((l,op), r) -> Binary(l, op, r))
let exprAux = binary <|> term
exprSetter.Set exprAux

(*** define:parser3 ***)
let formula = char '=' <*>> anySpace <*>> expr
let equation = anySpace <*>> (formula <|> number) <<*> anySpace 
let parse input = run equation input

(*** define:evaluator1 ***)
let rec evaluate cells expr = 
  match expr with
  | Number num -> 
      num 

  | Binary(l, op, r) -> 
      let ops = dict [ '+', (+); '-', (-); '*', (*); '/', (/) ]
      let l, r = evaluate cells l, evaluate cells r
      ops.[op] l r

  | Reference pos -> 
      let parsed = parse (Map.find pos cells)
      evaluate cells (Option.get parsed)

(*** define:evaluator2 ***)
let rec evaluate visited cells expr = 
  match expr with
  | Number num -> 
      Some num

  | Binary(l, op, r) -> 
      let ops = dict [ '+', (+); '-', (-); '*', (*); '/', (/) ]
      evaluate visited cells l |> Option.bind (fun l ->
        evaluate visited cells r |> Option.map (fun r ->
          ops.[op] l r ))

  | Reference pos when Set.contains pos visited ->
      None

  | Reference pos -> 
      Map.tryFind pos cells |> Option.bind (fun value ->
        parse value |> Option.bind (fun parsed ->
          evaluate (Set.add pos visited) cells parsed))
(*** hide ***)
open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Fable.Import

(**
Creating user interface using Elmish
------------------------------------

I'm going to start by discussing the user interface and then get back to implementing the parsing
and evaluation logic. For creating user interfaces, Fable comes with a great library called 
[Elmish](https://elmish.github.io/elmish). Elmish implements a functional-first user interface
architecture popularized by the Elm language, which is also known as _model view update_.

The idea of the architecture is extremely simple. You just need the following two types and 
two functions:

    [lang=fsharp]
    type State = (*[omit:(Record capturing the state)]*){ State : string }(*[/omit]*)
    type Event = (*[omit:(Union listing possible events)]*)EventOne | EventTwo(*[/omit]*)

    val update : State -> Event -> State
    val view : State -> (Event -> unit) -> Html

The two types and two functions define the user interface as follows:

 * `State` stores all the user interface state that you need in order to render it.
 * `Event` is a union of different events that can happen when the user interacts with the UI.
 * `update` is a function that takes an original state and an event and produces a new modified state.
 * `view` takes the state and generates a HTML document; it also takes a function `Event -> unit`
    which can be used in event handlers of the HTML document to trigger an event.

Conceptually, you can think that the application starts with an initial state, renders a page and,
when some action happens and event is triggered, updates the state using `update` and re-renders 
the page using `view`. The key trick that makes this work is that Elmish does not replace the
whole DOM, but diffs the new document with the last one and only updates DOM elements that have
changed.

What state and events are there in our spreadsheet? As with the whole spreadsheet application,
the first step in implementing the user interface is to define a few types:

*)
type Event =
  | UpdateValue of Position * string
  | StartEdit of Position

type State =
  { Rows : int list
    Cols : char list
    Active : Position option
    Cells : Sheet }
(**
In the state, we keep a list of row and column keys (this typically starts from `A1`, but 
we do not require that), currently selected cell (this can be `None` if no cell is selected)
and, finally, the cells of the spreadsheet. There are two events that can happen.
The `UpdateValue` event happens when you change the text in the current cell; the `StartEdit` 
event happens when you click on some other cell to start editing it. 

### Updating the spreadsheet after event

Writing the `update` function is quite easy - as with the main spreadsheet logic, we just need 
to write code until the type checker is happy! 

In Elmish, the `update` function is a little bit more complicated than I said above. In 
addition to returning new state, we can also return a list of _commands_. The commands are 
used to tell the system that it should start some action after updating the state. This can
be things such as starting a HTTP web request to fetch some information from the server.
In our case, we do not need any commands, so we just return `Cmd.none`:

*)
let update msg state = 
  match msg with 
  | StartEdit(pos) ->
      { state with Active = Some pos }, Cmd.none

  | UpdateValue(pos, value) ->
      let newCells = Map.add pos value state.Cells
      { state with Cells = newCells }, Cmd.none

(**
The implementation uses the `with` construct, which creates a clone of the `state` record
and updates some of its fields. In the case of `StartEdit`, we set the active cell to the
newly selected one. In the case of `UpdateValue`, we first add the new value to the sheet
(the `Map.add` function replaces existing value if there is one already) and then set the 
`Cells` of the spreadsheet. 

### Rendering the spreadsheet

To construct the HTML document, Elmish comes with a lightweight wrapper built on top of
React (although you can use other virtual DOM libraries too). The wrapper defines typed
functions for creating HTML elements and specifying their attributes. 

We'll first implement the main `view` function which generates the spreadsheet grid and
then discuss the `renderCell` helper which renders an individual cell.
*)
(*** include:render-view ***)
(**
Here, we're using F# list comprehensions to generate the HTML document. For example, the
lines 4-7 generate the header of the table. We create a `tr` element with no attributes
(the first argument) containing a couple of `th` elements (the second argument). We're 
using `yield` to generate the elements - first, we create the empty `th` element in the
left top corner and then we iterate over all the columns and produce a header for each of
the columns. The `col` variable is a character, so we first turn it into a string using 
`string` before turning it into HTML content using `str` function provided by Elmish.

The nice thing about writing your HTML rendering in this way is that it is composable.
We do not have to put everything inside one massive function. Here, we call `renderCell`
(line 12) to render the contents of a cell.

### Rendering spreadsheet cell

There are two different ways in which we render a cell. For the selected cell, we need
to render an editor with an input box containing the original entered text. For all other
cells, we need to parse the formula, evaluate it and display the result. The `renderCell` 
function chooses the branch and, in the latter case, handles the evaluation:
*)
(*** include:render-cell ***)
(**

We test whether the cell that is being rendered is the active one using the 
`state.Active = Some pos` condition. Rather than comparing two `Position` values,
we compare `Position option` values and do not have to worry about the case when
`state.Active` is `None`.

If the current cell is active, we take the entered value or empty string and pass
it to `renderEditor` (defined next). If no, then we try to get the input - if there is
no input, we call `renderView` with `Some ""` to render valid but empty cell. Otherwise, we 
use a sequence of `parse` and `evaluate` to get the result. We will look at both of these 
functions below, when discussing how the spreadsheet logic is implemented. Both
`parse` and `evaluate` may fail, so we use the option type to compose them. `Option.bind`
runs `evaluate` only when `parse` succeeds; otherwise it propagates the `None` result.
We also use `Option.map` to transform the optional result of type `int` into an 
optional string which we then pass to `renderView`.

So far, we have not created any handlers that would trigger events when something 
happens in the user interface. We're finally going to do this in `renderEditor` and 
`renderView`, which are both otherwise quite straightforward:
*)
let renderView trigger pos (value:option<_>) = 
  let color = if value.IsNone then "#ffb0b0" else "white"
  td 
    [ Style [Background color] 
      OnClick (fun _ -> trigger(StartEdit(pos)) ) ] 
    [ str (defaultArg value "#ERR") ]

let renderEditor trigger pos value =
  td [ Class "selected"] [ 
    input [
      AutoFocus true
      OnInput (fun e -> trigger(UpdateValue(pos, e.target?value)))
      Value value ]
  ]
(**
In `renderView`, we create red background and use the `#ERR` string if the value to display 
is empty (indicating an error). We also add an `OnClick` handler. When you click on the cell,
we want to trigger the `StartEdit` event in order to move the editor to the current cell. To
do this, we specify the `OnClick` attribute and, when a click happens, trigger the event using
the `trigger` function which we got as an input argument for the `view` function (and which 
we first passed to `renderCell` and then to `renderView`).

The `renderEditor` function is similar. We specify the `OnInput` handler and, whenever the text
in the input changes, trigger the `UpdateValue` event to update the value and recalculate 
everything in the spreadsheet. We also specify `AutoFocus` attribute which ensures that the
element is active immediately after it is created (when you click on a cell).

*)
(*** define:render-cell ***)
let renderCell trigger pos state =
  if state.Active = Some pos then
    let text = Map.tryFind pos state.Cells 
    renderEditor trigger pos (defaultArg text "")
  else
    match Map.tryFind pos state.Cells with 
    | Some input -> 
        let result = 
          parse input
          |> Option.bind (evaluate Set.empty state.Cells) 
          |> Option.map string
        renderView trigger pos result
    | _ -> renderView trigger pos (Some "")

(*** define:render-view ***)
let view state trigger =
  table [] [
    thead [] [ 
      tr [] [
        yield th [] []
        for col in state.Cols -> th [] [ str (string col) ]
      ] 
    ]
    tbody [] [
      for row in state.Rows -> tr [] [
        yield th [] [ str (string row) ]
        for col in state.Cols -> renderCell trigger (col, row) state
      ]
    ]
  ]

(**
### Putting it all together

Now we have all the four components we need to run our user interface. We have the `State` and 
`Event` type definitions and we have the `update` and `view` functions. To put everything together,
we need to define the initial state, specify the ID of the HTML element in which the application 
should be rendered and start it.
*)

let initial () = 
  { Cols = ['A' .. 'K']
    Rows = [1 .. 15]
    Active = None
    Cells = Map.empty },
  Cmd.Empty    
 
Program.mkProgram initial update view
|> Program.withReact "main"
|> Program.run
(**
The initial state defines the ranges of available rows and columns and specifies that there
are no values in any of the cells (the demo embedded above specifies the initial cells for
computing factorial and Fibonacci here). Then we use `mkProgram` to compose all the 
components together, we specify React as our execution engine and we start the Elmish application!

Implementing spreadsheet logic
------------------------------

So far, we defined the domain model which specifies what a spreadsheet is using F# types and
we implemented the user interface using Elmish. The only thing we skipped so far is the 
spreadsheet logic - that is, parsing of formulas and evaluation. Completing these two is going to 
be easier than you might expect!

*)
(** 
### Evaluating spreadsheet formulas

First, let's have a look at how to evaluate formulas. In the beginning, we defined the `Expr`
type as a discriminated union with three cases: `Number`, `Binary` and `Reference`. To 
evaluate an expression, we need to write a recursive function that uses pattern matching and
appropriately handles each case. We'll start with a simple version that does not handle errors
and does not check for recursive formulas:
*)
(*** include:evaluator1 ***)
(** 
The function takes the spreadsheet `cells` as a first argument, because it may need to lookup 
values of cells referenced by the current expression. It also takes the expression `expr` and
pattern matches on it. Handling `Number` is easy - we just return the number.

Handling `Binary` is a bit more interesting, because we need to call `evaluate` recursively 
to evaluate the value of the left and right sub-expressions. Once we have them, we use a simple
dictionary to map the operator to a function (written using standard F# operators) and run the
function.

Finally, when handling a `Reference`, we first get the input at the given cell, parse it and
then (again) recursively call `evaluate`. This can fail in many ways - the cell might be empty
or the parser could fail. We improve this in the next version of our evaluator where the 
function returns `int option` rather than `int`. The missing value `None` indicates that 
something went wrong.
*)
(*** include:evaluator2 ***)
(** 
In case of `Number`, we now return `Some num`. In this case, evaluation cannot fail.
In case of `Binary`, both recursive calls can fail and we get two option values. To handle this,
we use `Option.bind` and `Option.map` - both of these will call the specified function only when 
the previous operation succeeded, otherwise, they immediately return `None` indicating a failure.
If both the left and the right sub-expressions can be evaluated, we can then apply binary numerical
operator to their results. Handling of `Reference` is similar - we sequence a number of operations
that may fail using `Option.bind`.

Another interesting feature we added in this version is checking for recursive references. To
do this, the `evaluate` function now takes the `visited` parameter which is a set of cells that 
were accessed during the evaluation. We add cells to the set using `Set.add pos visited` on 
line 18. When we find a reference to a cell that we already visited (line 12), then we immediately
return `None`, because this would lead to an infinite loop.

### Parsing formulas
Finally, the last part of logic that we need to implement is the parsing of formulas entered by
the user into values of our `Expr` type. For this, we're going to use a very simple parser combinator
library (which you can find in the [full source code](https://github.com/tpetricek/elmish-spreadsheet/blob/master/src/helpers/parsec.fs)).
There are four key concepts in the library:

    [lang=fsharp]
    type Parser<'TChar, 'TResult> = 'TChar list -> option<'TResult * 'TChar list>

    val ( <*> ) : Parser<'T1> -> Parser<'T2> -> Parser<'T1 * 'T2>
    val ( <|> ) : Parser<'T> -> Parser<'T> -> Parser<'T>
    val map : ('T -> 'R) -> Parser<'T> -> Parser<'R>

 * `Parser<char, 'T>` represents a parser that takes a list of characters as the input. It
    returns `None` if the parser cannot parse the input. Otherwise, the parser parses a value and 
    returns it together with the rest of the input. The fact that parsers do not have to consume the 
    entire input makes it easy to compose them.

 * `<*>` is a binary operator that takes two parsers; it runs the first parser first, getting a
   value of type `'T1` and then runs the second parser on the rest of the input, getting a value of
   type `'T2`. It succeeds only if both parsers succeed and then it returns a pair with both values.

 * `<|>` is a binary operator that also takes two parsers, but they both have to recognise values of
   the same type. It tries to run the first parser and, if that fails, tries to run the second one.
   It succeeds if either of the parsers succeed and returns whatever the successful parser returned.

 * Finally, `map` is a function that transforms the value that a parser produces. Given a parser of
   type `Parser<'T>` and a function `'T -> 'R`, it returns a parser that runs the original parser and,
   if that is successful, applies the function to the result.

The following snippet shows how we use these three ideas to create simple parsers to recognise 
operators, references and numbers:
*)
(*** include:parser1 ***)
(** 
The `char` function creates a parser that recognises only the given character (and then returns it as the
result). Thus, the `operator` parser recognises the four standard numerical binary operators and accepts
no other characters. The `reference` parser recognises a letter followed by a number. This returns 
a `char * int` pair which we turn into the `Reference` value of `Expr` using the `map` function.
Parsing a number is even easier - we just run the built-in `integer` parser and wrap it in `Number`. 
Note that the type of `reference` and `number` is now the same - `Parser<char, Expr>`. This means that
we can compose them using `<|>` to create parser that recognises either of the two expression types.

Finishing the rest of the parsing is a bit more work, because we need to handle parentheses as 
`(1+2)*3` and also ignore whitespace, but the concepts are the same:
*)
(*** include:parser2 ***)
(** 
To deal with recursion, the library allows us to create a parser using `slot`, use it, and then define 
what it is later using `exprSetter`. In our case, we define `expr` on line 1, use it when defining
`brack` (line 3) and then define it on line 9. This is a recursive reference;  `exprAux` can
be `binary`, which contains `term`, which can be `brack` and that, in turn, contains `expr`.

The only other clever thing in the snippet are the `<<*>` and `<*>>` operators. Those behave like
`<*>`, but return only the result from the parser on the left or right (wherever the double arrow points).
This is useful, because we can write `anySpace <*>> expr <<*> anySpace` to parser expression surrounded
by whitespace, but get a parser that returns just the result of `expr` (we do not care what the whitespace 
was).

Finally, we define a formula which is `=` followed by an expression and an equation - that is, the thing
that you can type in the spreadsheet - which is either a formula or a number.
*)
(*** include:parser3 ***)
(** 
The `parse` function defined on the last line lets us run the main `equation` parser on a given input.
It takes a sequence of characters and produces `option<Expr>`, which is exactly what we've used earlier
in the article.

## Conclusions

In total, this article showed you some 125 lines of code. If we did not worry about nice formatting
and skipped all the blank lines, we could have written a simple spreadsheet application in some 100
lines of code! Aside from standard Fable libraries, the only thing I did not count is the parser combinator
library. I wrote that on my own, but there are similar existing libraries that you could use (though
you'd need to find one that works with Fable). 

The final spreadsheet application is quite simple, but it does a number of interesting things. It 
runs in a web browser and you can scroll back to the start of the article to play with it again! 
On the technical side, it has a user interface where you can select and edit cells, it parses the 
formulas you enter and it also evaluates them, handling errors and recursive references. 

> _<i class="fa fa-hand-o-right" style="font-size:110%;margin:0px 5px 0px 0px"></i>_
> If you enjoyed this post and want to learn more about F# and also Fable, join 
> our [F# FastTrack](https://skillsmatter.com/courses/473-tomas-petricek-phil-trelford-fast-track-to-fsharp)
> course on **6-7 December** in **London** at SkillsMatter. We'll cover Fable, Elmish, but
> also many other F# examples. Get in touch at [@tomaspetricek](http://twitter.com/tomaspetricek)
> or email [tomas@tomasp.net](mailto:tomas@tomasp.net) for a 10% discount for the course,
> or if you are interested in custom on-site training.

I like this example, because it shows how a number of nice aspects of the F# language and also the
F# community can come together to provide a fantastic overall experience. In case of our spreadsheet,
this includes:  

* [Fable](https://fable.io/) makes it possible to compile F# to JavaScript, but more importantly,
  it also gives us access to the JavaScript ecosystem. Fable follows the pragmatic style of
  functional-first F# programming. This makes it possible to integrate with libraries such as React
  and build different architectures on top of them.

* The Elm architecture, as implemented by [the Elmish library](https://elmish.github.io/elmish/),
  is a fantastic way to write functional-first user interfaces. All we had to do to implement the
  spreadsheet user interface was to define types for the state and events and then implement the
  `update` and `view` functions.

* Finally, the example also used compositionality of functional programming in two ways. First, an
  expression is elegantly expressed by a recursive type `Expr` which can consist of other `Expr`
  values. Second, we composed a parser for spreadsheet formulas from just a few primitives using
  just two operators, `<|>` and `<*>`.

If you want to have a look at the complete source code, you can find it [in my elmish-spreadsheet
repository on GitHub](https://github.com/tpetricek/elmish-spreadsheet/). The repository is designed
as a hands-on exercise where you can start with a template, complete a number of tasks and end
up with a spreadsheet, but there is also `completed` branch where you find the finished source code.
You can also edit and run the code in your browser using the [Fable REPL](https://fable.io/repl/) 
(you'll find it under Samples, Elmish, Spreadsheet),

*)

