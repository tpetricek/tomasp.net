(**

Library patterns: Multiple levels of abstraction
==============================================

 - date: 2015-02-03T15:54:30.6183730+00:00
 - description: Over the last few years, I created or contributed to a number of libraries. In this blog post (or perhaps a series), I'll share some of the things I learned when trying to answer the question: What should a good library look like? I'll start by looking at a library design pattern that I call 'levels of abstraction'.
 - layout: article
 - image: http://tomasp.net/blog/2015/library-layers/layers.png
 - tags: f#,open source,functional programming
 - title: Library patterns: Multiple levels of abstraction
 - url: 2015/library-layers

--------------------------------------------------------------------------------
 - standalone

<img src="http://tomasp.net/blog/2015/library-layers/layers.png" style="float:right;margin:15px 0px 15px 10px" />

Over the last few years, I created or contributed to a number of libraries including 
[F# Data](https://github.com/fsharp/FSharp.Data) for data access,
[Deedle](http://bluemountaincapital.github.io/Deedle/) for exploratory data science with C# and F#,
Markdown parser and code-formatter [F# Formatting](http://tpetricek.github.io/FSharp.Formatting/)
and other fun libraries such as one for composing 3D objects [used in my Christmas blog 
post](http://tomasp.net/blog/2014/composing-christmas/).

Building libraries is a fun and challenging task - even if we ignore all the additional things
that you need to get right such as testing, documentation and building (see [my earlier blog
post](http://tomasp.net/blog/2013/great-open-source/)) and focus just on the API design.
In this blog post (or perhaps a series), I'll share some of the things I learned when trying 
to answer the question: What should a good library look like?

I was recently watching Mark Seemann's course [A Functional architecture with F#](http://blog.ploeh.dk/2014/01/22/a-functional-architecture-with-f/),
which is a great material on designing functional *applications*. But I also realised that not much
has been written on designing functional *libraries*. For some aspect, you can use functional patterns
like monads (see [Scott Wlaschin's presentation](https://skillsmatter.com/skillscasts/6120-functional-programming-design-patterns-with-scott-wlaschin)),
but this only answers a part of the question - it tells you how to design individual types, but
not an entire library...

--------------------------------------------------------------------------------
*)
(*** hide ***)
#nowarn "1189"

// Castle using Functional 3D library
#I "../packages/2014/functional3d"
#r "OpenTK.dll"
#r "OpenTK.GLControl.dll"
#load "functional3d.fs"
open System
open System.Drawing
open Functional3D
open OpenTK
open OpenTK.Graphics
open OpenTK.Graphics.OpenGL

let tower x z = 
  (Fun.cylinder
     |> Fun.scale (1.0, 1.0, 3.0) 
     |> Fun.translate (0.0, 0.0, 1.0)
     |> Fun.color Color.DarkGoldenrod ) $ 
  (Fun.cone 
     |> Fun.scale (1.3, 1.3, 1.3) 
     |> Fun.translate (0.0, 0.0, -1.0)
     |> Fun.color Color.Red )
  |> Fun.rotate (90.0, 0.0, 0.0)
  |> Fun.translate (x, 0.5, z)

let sizedCube height = 
  Fun.cube 
  |> Fun.scale (0.5, height, 1.0) 
  |> Fun.translate (-0.5, height/2.0 - 1.0, 0.0)

let twoCubes =
  sizedCube 0.8 $ (sizedCube 1.0 |> Fun.translate (0.5, 0.0, 0.0))

let block = 
  [ for offset in -4.0 .. +4.0 ->
      twoCubes |> Fun.translate (offset, 0.0, 0.0) ]
  |> Seq.reduce ($)
  |> Fun.scale (0.5, 2.0, 0.3)
  |> Fun.color Color.DarkGray

let wall offs rotate = 
  let rotationArg = if rotate then (0.0, 90.0, 0.0) else (0.0, 0.0, 0.0)
  let translationArg = if rotate then (offs, 0.0, 0.0) else (0.0, 0.0, offs)
  block |> Fun.rotate rotationArg |> Fun.translate translationArg

#r "C:/tomas/public/fsharp.data/bin/fsharp.data.dll"
#I "../packages/FSharp.Compiler.Service.0.0.82/lib/net40"
#I "../packages/FSharp.Formatting.2.6.3/lib/net40"
#r "FSharp.Compiler.Service.dll"
#r "FSharp.Literate.dll"
#r "FSharp.CodeFormat.dll"
#r "FSharp.Markdown.dll"
open FSharp.Literate
open FSharp.Markdown
open System.IO

let guide = @"C:\Tomas\Web\FSharp.Foundation\guides\data-science\index.md"
let webHome = @"C:\Tomas\Web\FSharp.Foundation\guides"
let webOutput = @"C:\Temp\__guides"
let root = "https://raw.githubusercontent.com/fsharp/fsfoundation/gh-pages/guides/"
let api = "https://api.github.com"
(**


Over the last few years, I created or contributed to a number of libraries including 
[F# Data](https://github.com/fsharp/FSharp.Data) for data access,
[Deedle](http://bluemountaincapital.github.io/Deedle/) for exploratory data science with C# and F#,
Markdown parser and code-formatter [F# Formatting](http://tpetricek.github.io/FSharp.Formatting/)
and other fun libraries such as one for composing 3D objects [used in my Christmas blog 
post](http://tomasp.net/blog/2014/composing-christmas/).

Building libraries is a fun and challenging task - even if we ignore all the additional things
that you need to get right such as testing, documentation and building (see [my earlier blog
post](http://tomasp.net/blog/2013/great-open-source/)) and focus just on the API design.
In this blog post (or perhaps a series), I'll share some of the things I learned when trying 
to answer the question: What should a good library look like?

### Library design patterns

I was recently watching Mark Seemann's course [A Functional architecture with F#](http://blog.ploeh.dk/2014/01/22/a-functional-architecture-with-f/),
which is a great material on designing functional *applications*. But I also realised that not much
has been written on designing functional *libraries*. For some aspect, you can use functional patterns
like monads (see [Scott Wlaschin's presentation](https://skillsmatter.com/skillscasts/6120-functional-programming-design-patterns-with-scott-wlaschin)),
but this only answers a part of the question - it tells you how to design individual types, but
not an entire library.

The key library design principles that I find useful (and follow in libraries that I work on) are:

 * **Iterative design.** First of all, you should never start designing a library by *designing
   a library*. In F#, you can put a lot of functionality in just a script file and reference it
   (or copy it to other projects). This is the best way to explore what you'll actually need from 
   a library. Once you have a better idea, you can turn the (single) file into a new project.

 * **Composable.** Composability is the key principle of functional programming *in general* and
   it is equally important for library design. A library should always expose functionality in a
   way that lets the user build more complex behaviour from simpler building blocks (in contrast
   to offering simple complex entry point that can be, to some extent, parameterized).

 * **Avoid callbacks.** Callbacks are probably the easiest way of breaking composability. When
   you are writing some complex functionality (say, processing Markdown documentation), you might
   be tempted to parameterize some step using a callback (say, to plug-in a pre-processor that 
   transforms document between parsing and rendering). 
   The problem is that callbacks impose too much structure on your code. They do not give you 
   the flexibility to put individual *simpler pieces* together in a different way. Instead, 
   inserting callbacks makes the design *more complicated* than it was.

 * **Levels of abstraction.** So, how can we expose simple API that is simple to use and
   can be used in multiple different ways? The key idea is to provide multiple levels of 
   abstraction. This is what I want to discuss in this article, so continue reading!

The concepts in the above list are related, but I think they are all key to good library design.
They would each deserve a separate article (which may happen if there is enough interest :-)), but
I'm going to focus on levels of abstractions first...

How are libraries used?
-----------------------

<img src="layers.png" style="float:right;margin:15px 0px 15px 10px" />

Each library is designed with some typical scenarios in mind. For example, F# Formatting lets
you [generate documentation from all files in a directory](http://tpetricek.github.io/FSharp.Formatting/literate.html#Processing-entire-directories).
This is all you need in, perhaps, 80% of scenarios. Sometimes, you need to handle individual
files differently (say, use a different template). A good library needs to [make this possible 
too](http://tpetricek.github.io/FSharp.Formatting/literate.html#Processing-individual-files).
But sometimes, you actually want to process a file differently - for example, add an automatically
generated TOC (table of contents). 

An approach that I find extremely useful is to build a library that exposes the functionality
as multiple layers of abstraction. At the highest level, you can handle the 80% of scenarios with
just a single function call. If you need, you can go one level deeper and implement the next 15%
of more interesting scenarios. And if you really need, you can go even deeper and implement the
next 4% of scenarios (and for the last 1%, you have to send a pull request!) 

As you'll see, this *design pattern* is heavily used in core functional libraries such as the F#
library for working with lists. Following this pattern well also turns your libraries into 
*domain specific languages* and makes the code more readable.

Demo #1: Working with functional lists
--------------------------------------

The idea of having multiple levels of abstraction can be easily demonstrated using lists.

### High-level: Higher-order functions

At the high level, you can process lists using higher-order functions (or using LINQ in C#).
As a (somewhat) silly example, if we want to get a list of positive `sin` values of integers
from 0 to 100, we can write:
*)
[0 .. 100]
|> List.map (float >> sin)
|> List.filter (fun n -> n > 0.0)
(**
Higher-order functions like `List.map` and `List.filter` cover the 80% of scenarios when 
working with lists (or perhaps even more). But sometimes you may want to do something that
is not covered by the higher-order functions.

### Low-level: Recursion and patter matching

For example, say we want to split a list into two, around the point where the sign of 
the values in the list changes. That is `splitAtSignChange [1; 4; -3; 2]` should return
`[1; 4]` and `[-3; 2]`. This is not easy to do using higher-order functions, but we can 
use the lower-level API and write a recursive function that pattern matches on the list:
*)
/// Split a list at the point where the 
/// sign of adjacent elements changes
let inline splitAtSignChange list =
  // Keeps the last element and elements visited so
  // far (before the sign change) in a reversed list
  let rec loop last acc = function
    | [] -> List.rev acc, []
    | first::tail as list when sign first <> sign last -> 
        List.rev acc, list
    | first::tail -> loop first (first::acc) tail

  // Use the first element as 'last' argument of 'loop'
  match list with
  | [] -> [], []
  | head::tail -> loop head [head] tail
(**
The function is more difficult to understand than the code we can write with higher-order
functions, but it is still quite readable. Inside `loop`, there are 3 cases that handle
the situations when 1.) all elements have the same sign 2.) we find a sign change and 3.)
we skip over element before a sign change.

If you were writing this in C# using `IEnumerable<T>`, you don't get nice pattern matching 
on lists, but you still get _some_ lower-level API (using `GetEnumerator`, temporary collections
and mutation).

### Going from low-level to high-level

The beautiful thing about the collection API design is that we can now move back from the 
lower level to the higher level. The function `splitAtSignChange` can be seen as an instance
of a more general operation where we split a list based on a predicate applied to two adjacent
elements.
*)
module List =
  /// Splits a list into two when the predicate 'f'
  /// returns 'true' on two adjacent elements of the list
  let splitAt f list =
    // Same as before with 'f first last' in the second case
    let rec loop last acc = function
      | [] -> List.rev acc, []
      | first::tail as list when f last first -> 
          List.rev acc, list
      | first::tail -> loop first (first::acc) tail

    // Use the first element as 'last' argument of 'loop'
    match list with
    | [] -> [], []
    | head::tail -> loop head [head] tail
(**
The function is pretty much the same as before - it takes an additional parameter `f` and uses
it to decide when to break the list. Although the function is itself implemented using the
lower-level API, it gives us a way to get back to the higher-level!

<img src="loop.png" />

Getting back to the higher level means that we can recover the much simpler programming
style. Say, we wanted to take `sin` values of integers from 1 to 10 and split them into
two lists, based on when the value crosses the X axis:
*)
[1 .. 10]
|> List.map (float >> sin)
|> List.splitAt (fun a b -> sign a <> sign b)
(**
Now we are back at the high-level and we can solve the problem using two simple and easy
to understand lines of code.

Functional lists with their low-level pattern matching and high-level functions are
a nice example of a library with multiple levels of abstractions. But how does this
work in other scenarios?

> **Non-Example: Reactive Extensions**
>
> One of the things I always found quite unfortunate about the Rx library is that it
> fails this principle. It provides a nice high-level abstraction (through the standard
> LINQ operators), but it does not give you any convenient way to implement your own
> operators using a lower level API. You can see this when you look at the 
> standard `Select` operator, which [is 125 lines of code](https://rx.codeplex.com/SourceControl/latest#Rx.NET/Source/System.Reactive.Linq/Reactive/Linq/Observable/Select.cs)
> (compare this with `List.map` in the F# library, which is [16 lines](https://github.com/fsharp/fsharp/blob/master/src/fsharp/FSharp.Core/local.fs#L43)
> and could be 3 if it was not optimized using mutation).
>
> Of course, Rx is solving significantly more complicated task than the `List` module, but
> that is not the point - the point is that `Select` should be easy to implement using *some* lower-level
> abstraction. What should this look like? In F#, this would probably be done using
> [agents](http://www.developerfusion.com/article/139804/an-introduction-to-f-agents/) or
> library such as [Hopac](http://hopac.github.io/Hopac/Hopac.html).

Demo #2: Domain-specific languages for 3D 
-----------------------------------------

In a [blog post](http://tomasp.net/blog/2014/composing-christmas/) that I wrote for the 
[F# Advent Calendar](https://sergeytihon.wordpress.com/2014/11/24/f-advent-calendar-in-english-2014/), 
I used a library for composing 3D objects. This is another good example of a library that
provides multiple levels of abstraction - especially when you use it to model custom objects.

### Very-high-level: Building castles

For example, have a look at what you can achieve with the following snippet:
*)
tower -2.0 -2.0 $ tower 2.0 -2.0 $ 
tower -2.0 2.0 $ tower 2.0 2.0 $
wall -2.0 true $ wall 2.0 true $
wall -2.0 false $ wall 2.0 false
(**
<p style="text-align:center"><img src="castle.png" style="width:407px" /></p>

At the highest level of abstraction, we can create a castle with just 4 lines of code!
Of course, this level of abstraction lets you create only very limited things (castles
with walls and towers), but if you are a castle-builder, this might be what you need...

> **Domain-specific languages**
>
> The term "domain-specific languages" (DSLs) means different things to different people.
> For me, a library like this is an (embedded) DSL, because it lets you express your 
> intentions in a very readable way using just a vocabulary of the specific domain.
> In a sense, the higher levels of abstraction of any composable functional library
> should always be domain-specific languages.

### High-level: Composing 3D objects

Now, what if we wanted to build a castle with different towers? To do that, we still don't
need to look at any low-level 3D rendering code. The `tower` function itself is written
in terms of another "language" or "level of abstraction". A tower is just a coloured
cylinder, composed with a coloured cone that are appropriately rotated:
*)
let tower x z = 
  (Fun.cylinder
     |> Fun.scale (1.0, 1.0, 3.0) 
     |> Fun.translate (0.0, 0.0, 1.0)
     |> Fun.color Color.DarkGoldenrod ) $ 
  (Fun.cone 
     |> Fun.scale (1.3, 1.3, 1.3) 
     |> Fun.translate (0.0, 0.0, -1.0)
     |> Fun.color Color.Red )
  |> Fun.rotate (90.0, 0.0, 0.0)
  |> Fun.translate (x, 0.5, z)
(**
When using the library, you might start at the very high level of abstraction (solving
specific narrow problems), but as you become more familiar with it, you can move one 
level down. At this level, you can build your own languages (abstractions) in the same
way as when we implemented `List.splitAt` in terms of recursion and pattern matching.

### Low-level: Rendering faces with OpenGL

Now, there is still a lower level at which the actual 3D rendering happens. In this 
library, the lower level is simply calling OpenGL primitives (using the OpenTK wrapper),
and so this is not particularly interesting, but it is not very complicated either:
*)
let cube = DF (fun ctx ->
  GL.Material
    ( MaterialFace.FrontAndBack, 
      MaterialParameter.Diffuse, ctx.Color)
  GL.Begin(BeginMode.Quads)
  GLEx.Face 
    (-1.f, 0.f, 0.f) 
    [ (-0.5f, -0.5f, -0.5f); (-0.5f, -0.5f,  0.5f); 
      (-0.5f,  0.5f,  0.5f);  (-0.5f,  0.5f, -0.5f) ] 
  (*[omit:(Remaining 5 faces omitted)]*)
  GLEx.Face 
    ( 1.f, 0.f, 0.f)
    [ ( 0.5f, -0.5f, -0.5f); ( 0.5f, -0.5f,  0.5f);
      ( 0.5f,  0.5f,  0.5f); ( 0.5f,  0.5f, -0.5f) ]
  GLEx.Face 
    (0.f, -1.f, 0.f)
    [ (-0.5f, -0.5f, -0.5f); (-0.5f, -0.5f,  0.5f);
      ( 0.5f, -0.5f,  0.5f); ( 0.5f, -0.5f, -0.5f) ]
  GLEx.Face 
    (0.f, 1.f, 0.f)
    [ (-0.5f,  0.5f, -0.5f); (-0.5f,  0.5f,  0.5f);
      ( 0.5f,  0.5f,  0.5f); ( 0.5f,  0.5f, -0.5f) ]
  GLEx.Face 
    (0.f, 0.f, -1.f)
    [ (-0.5f, -0.5f, -0.5f); (-0.5f,  0.5f, -0.5f);
      ( 0.5f,  0.5f, -0.5f); ( 0.5f, -0.5f, -0.5f) ]
  GLEx.Face 
    (0.f, 0.f, 1.f)
    [ (-0.5f, -0.5f,  0.5f); (-0.5f,  0.5f,  0.5f);
      ( 0.5f,  0.5f,  0.5f); ( 0.5f, -0.5f,  0.5f) ](*[/omit]*)
  GL.End() )
(**
If we wanted to make it easier to create custom basic shapes, we could
certainly add another layer between the 3D primitives and rendering, but 
this was not the aim here. Even without that, you can see that having multiple 
levels of abstraction has important benefits. The most obvious one is that you
can easily move from a higher level to a lower level to explore how things are created
(say, how `tower` is constructed from `cone` and `cylinder`) and use the lower-level 
primitives in a different way. 

In other words, if you implemented `tower` as a 
function that directly calls OpenGL, you could hardly provide enough overloads
(and optional parameters) to capture all possible kinds of towers that someone
might want to create. But if your library has multiple levels of abstraction, this
is not a problem.

Demo #3: Documentation generation 
---------------------------------

The examples I discussed so far may look like special libraries - one was a core 
functional library with just two levels and another was specifically designed as
a domain-specific language. To convince you that this approach is valuable _in general_,
I'll look at one more example, this time from the [F# Formatting library](http://tpetricek.github.io/FSharp.Formatting/).

The library consists of a Markdown parser and F# code formatter (and is used by 
most F# projects around). The most common scenario for using the library is to turn
a folder with Markdown files and F# Script files into an output folder with generated
HTML documentation. This is what the [ProjectScaffold template does](https://github.com/fsprojects/ProjectScaffold/blob/master/docs/tools/generate.template#L104).

### High-level: Processing directories

So assuming `webHome` is a folder with Markdown files (like the
[F# Foundation guides](https://github.com/fsharp/fsfoundation/tree/gh-pages/guides)), we can
generate HTML documentation just by calling `Literate.ProcessDirectory`:
*)
Literate.ProcessDirectory
  ( inputDirectory=webHome, 
    outputDirectory=webOutput, 
    processRecursive=true )
(**
This is the high-level version of the API provided by the library and also the code that
you'll need to write in 80% of cases. The `ProcessDirectory` has a number of arguments that
let you specify how the processing is done (such as a template file, F# Compiler instance
for colorization of snippets etc.)

However, it does not let you customize which files in the directory are processed and it
does not let you perform any transformations on the individual files. For that, we need
to look at the lower-level API.

### Medium-level: Processing individual files

For an example, let's say that we don't want to look at Markdown files in a local folder, 
but instead we want to crawl the [guides directory](https://github.com/fsharp/fsfoundation/tree/gh-pages/guides)
of the F# Foundation page directly from GitHub. To do this, we'll use the GitHub API
(to get a list of folders and files in them). Using the [JSON type provider](http://fsharp.github.io/FSharp.Data/library/JsonProvider.html), 
we start with a function that returns the result of GitHub contents query:
*)
open FSharp.Data

/// Type representing returned JSON, inferred from a sample
type GitHubDir = JsonProvider<"../data/repos.json">

/// Query the contents of a specified 'dir' in a 'repo'
let queryDir repo dir =
  Http.RequestString
    ( sprintf "%s/repos/%s/contents/%s?anon=1" api repo dir,
      headers = [HttpRequestHeaders.UserAgent "My App"] )
  |> GitHubDir.Parse
(**
Now, we can use `queryDir` to get all sub-directories in the "guides" folder and 
then get all the files under each guide. We can then process each file using the
medium-level `Literate.ProcessMarkdown` function:
*)
// Iterate over all Markdown guides on fsharp.org
let wc = new System.Net.WebClient()
let guides = queryDir "fsharp/fsfoundation" "guides"
for subdir in guides do
  let files = queryDir "fsharp/fsfoundation" ("guides/" + subdir.Name)
  for file in files do
    // Downloda the guide to a local Temp file
    let tempFile = Path.GetTempFileName()
    wc.DownloadFile(root + subdir.Name + "/" + file.Name, tempFile)
    let dir = Path.Combine(webOutput, subdir.Name)
    Directory.CreateDirectory(dir) |> ignore
    // Process the guide using medium-level API
    let out = Path.ChangeExtension(Path.Combine(dir, file.Name), ".html")
    Literate.ProcessMarkdown(tempFile, output=out)
(**
So far, we replaced the default behaviour for processing directories with our own
custom behaviour for crawling documents on GitHub. This looks quite simple - which
is the point of the library! But imagine some alternative approaches that you could 
use here.

> **Non-example: Abstracting directories with interfaces**
>
> If you were solving this problem in an object-oriented style, you might try to
> design an interface that models "directory structure browser" or something like 
> that. The `Literate.ProcessDirectory` method would then take `IDirectoryBrowser`.
>
> Your `IDirectoryBrowser` would have methods `GetSubDirectories` and `GetFiles` and
> default implementation walking over file structure. You could then implement it 
> differently using the GitHub API.
>
> The problem with this is that it inverts the control - rather than being a *library*
> that you can call to do things you want, it becomes a *framework* that dictates 
> what you need to provide. And if you need something else, then you're out of luck!
> For example, if you suddenly wanted to process documents that do not naturally fit
> into a tree structure, the `IDirectoryBrowser` abstraction would break. With a
> library that provides multiple levels of abstraction, this is not a problem, because
> *you* are in control.

So far, we looked at processing whole directories and individual files, but there is one
more level we can look at...

### Low-level: Transforming documents

Now, let's say that we wanted to perform some transformation on the documents as part of 
the processing. If you're using something like Jekyll (on GitHub), then the way to do this
is to use [Jekyll plugins](http://jekyllrb.com/docs/plugins/). There are four kinds of 
plugins (generators, converters, commands and tags) - so, no matter what you're doing, it
will have to fit into one of the patterns (interfaces) that Jekyll developers expect you 
to use.

In F# Formatting, we again avoid using inversion of control caused by interfaces (or plugins).
Instead, you can look one level deeper and access the data structure that represents the
parsed Markdown and perform some transformation on the structure.

Let's say that we are currently processing a file `guide`. Rather than using `Literate.ProcessMarkdown`,
we can call `Literate.ParseMarkdownFile`, which gives us the result of parsing (first of the 
two steps that processing does):
*)
// Parse the Literate Markdown file
let doc = Literate.ParseMarkdownFile(guide)

/// Helper function that collects headers
let rec collectHeaders = function
  | Heading(level, content) -> [level, content]
  | Matching.ParagraphNested(_, pars) ->
      pars |> List.concat |> List.collect collectHeaders
  | _ -> []

// Returns a list of headings with their level
doc.Paragraphs |> List.collect collectHeaders
(**
The low level of the API is now starting to look similar to the list processing function in the
first example. We get the paragraphs of the document as a value of the `MarkdownParagraphs` type
and we can pattern match on it. The pattern matching uses active patterns, so we do not need
to handle all possible kinds of paragraphs, but just the one we are interested in (`Heading`) and
one that represents nesting (you can find more about this pattern in Chapter 3 of [F# Deep Dives](http://manning.com/petricek2/)).

Now that we have the headings, we can generate new Markdown paragraphs representing
the `<ol>` element with the table of contents and create a new document that includes
the TOC as an additional part of the body:
*)
/// Generates Markdown nodes with the TOC
let generateToc items = 
  /// Split the given list of 'items' into the first node,
  /// its children (following items with larger level) and 
  /// process the remaining items recursively.
  let rec getChildren acc items = 
    (*[omit:(Implementation hidden)]*)
    match items with
    | [] -> List.rev acc
    | [_, leaf] -> [Span leaf]
    | ((level, this)::rest) as items ->
        let nested, other = 
          rest |> List.splitAt (fun _ (next, _) -> next <= level)
        let sub = getChildren [] nested
        let par = [ListBlock(Ordered, sub |> List.map (fun s -> [s])); Span this]
        getChildren (List.append par acc) other(*[/omit]*)
  getChildren [] items

// Generate new Markdown content
// (collect all headers & generate TOC)
let tocPars = 
  doc.Paragraphs 
  |> List.collect collectHeaders
  |> generateToc

// Create document with additional paragraphs
// (for the TOC) and format it as HTML
doc.With(List.append tocPars doc.Paragraphs)
|> Literate.WriteHtml
(**
Just like in the example with processing lists, we could now identify a more
general pattern and provide a higher-level function that encapsulates the behaviour.
But having _access_ to the lower-level API means that we are not limited to a couple
of functions that the library developers thought of! Instead, we can always switch 
to the lower-level API when there is something we want to do that is not directly 
exposed at the higher-level.

Summary
-------

In this article, I talked about one of the design patterns that I find useful when
designing libraries in functional style. The idea of the pattern is quite simple.

### Levels of abstraction pattern

In your library, you should provide high-level functions that handle 80% of the tasks
that your users need. For the next 15% of tasks, the library should provide a lower-level
API. The key point is that the high-level API should be easy to express in terms
of the lower-level API. This means that one should be able to "unroll" the single
higher-level call into code that composes a couple of lower-level calls and customize
the code to do whatever is needed. Then, you should be able to "fold" the lower-level
code back into a new reusable higher-level operation.

I demonstrated this using three examples - when working with functional lists, you can
"unroll" higher-order functions to recursive processing and pattern matching; when 
composing 3D graphics, you can "unroll" concepts such as tower or wall into primitive
objects like cubes. Finally, the documentation generation library F# Formatting lets
you do the same thing. You can "unroll" code that processes entire folder into calls
to process individual files - and even further, you can "unroll" processing and insert
custom code between the parsing and rendering phases.

### Benefits of the pattern

Why would you design libraries in this way? I discussed some of the advantages along the
way - but the key point is that libraries with multiple levels of abstraction make the
most common 80% of tasks really easy. But they also do not restrict the remaining tasks
to what the author of the library could imagine when designing it. Instead, you are
giving the library users the power to use it in interesting ways and customize it to 
their needs.

In case of F# Formatting, this already paid off - the high-level API is used by many
F# open-source projects through [ProjectScaffold](https://github.com/fsprojects/ProjectScaffold/), but
the low-level API provides enough flexibility for amazing tools like [FsReveal](http://fsprojects.github.io/FsReveal/).
*)
