(**

Building great open-source libraries
====================================

 - date: 2013-10-31T13:29:06.2249127+00:00
 - description: The hard part about open-source development is not putting the source code on the internet. The hard part is keeping the source, releases and documentation up-to-date. I'm still amazed how nicely FAKE solves this problem for F#.
 - layout: article
 - tags: open source,f#,fake,f# formatting
 - title: Building great open-source libraries
 - url: 2013/great-open-source

--------------------------------------------------------------------------------
 - standalone


<a href="http://tpetricek.github.io/FSharp.Formatting/">
<img src="http://tpetricek.github.io/FSharp.Formatting/misc/logo.png" alt="F# documentation tools" class="rdecor" style="width:150px;height:150px" />
</a>

The hard part about successful open-source development is not putting the first 
version of your source code on GitHub. The hard part is what comes next. First
of all, there are _community aspects_ - making sure that the project fits well with
other work in the area, engaging the community and contributors, planing future
directions for the project and so on. Secondly, there is an _infrastructural side_ - 
making sure that there is a package (on [NuGet in the F# world](http://www.nuget.org/packages?q=fsharp)),
easy to run and useful tests and also up-to-date documentation and tutorials.

In this article, I want to talk about the _infrastructural side_, which is 
easier of the two, but nevertheless, difficult to get right!
Fortunately, the F# community made an amazing progress in this direction, so let's
have a look at some of the tools that make this possible...

--------------------------------------------------------------------------------
*)
(***hide***)
#r "../packages/FsCheck.0.9.1.0/lib/net40-Client/FsCheck.dll"
#r "../packages/NUnit.2.6.3/lib/nunit.framework.dll"
#r "../packages/FsUnit.1.2.1.0/lib/Net40/FsUnit.NUnit.dll"
#r "../packages/FSharp.Data.1.1.0.0/FSharp.Data.dll"
#load "../packages/2013/great-open-source/LazyList.fs"
#load "../packages/2013/great-open-source/Common.fs"
#r "System.Xml.Linq.dll"
open System
open FSharp.Data
open NUnit.Framework
open FsCheck
open FsUnit
open FSharp.DataFrame.Internal
let comparer = System.Collections.Generic.Comparer<int>.Default
  
(**
<a href="http://tpetricek.github.io/FSharp.Formatting/">
<img src="http://tpetricek.github.io/FSharp.Formatting/misc/logo.png" alt="F# documentation tools" class="rdecor" style="width:150px;height:150px" />
</a>

The hard part about successful open-source development is not putting the first 
version of your source code on GitHub. The hard part is what comes next. First
of all, there are _community aspects_ - making sure that the project fits well with
other work in the area, engaging the community and contributors, planing future
directions for the project and so on. Secondly, there is an _infrastructural side_ - 
making sure that there is a package (on [NuGet in the F# world](http://www.nuget.org/packages?q=fsharp)),
easy to run and useful tests and also up-to-date documentation and tutorials.

In this article, I want to talk about the _infrastructural side_, which is 
easier of the two, but nevertheless, difficult to get right!

On the technical side, I think that every good open-source library needs to have:

 - **Unit tests** - at least for non-trivial parts of code and to prevent regressions
 - **Random testing** - for tricky parts of code, it is useful and helps checking unexpected cases
 - **NuGet package** - or other up-to-date and easy to use release; for F# projects, we 
   might also want to have an easy to download ZIP file for simple interactive scripts
 - **Documentation** - for public API, at least when the API is not super simple
 - **Tutorials & walkthroughs** - showing how to call the API in a larger-scale scenarios
 - **Automation** - when releasing a new version, all of the above should happen with "one click"
   and documentation with tutorials must be up-to-date and correct.

Ticking all the points is a lot of work, but it is crucial - if you do not have these,
your project will be difficult to use, making a new release will take time and documentation
with tutorials will become useless. Fortunately for me, the F# community made an amazing progress 
in this direction, so let's have a look at some of the tools that make this possible...

Before going further, let me say big thanks to [Steffen Forkmann](https://twitter.com/sforkmann),
the author of FAKE, and [Gustavo Guerra](https://twitter.com/ovatsus), who wrote most of the
automation for [F# Data](https://github.com/fsharp/FSharp.Data/) that I'll use as an example.

Automate everything with FAKE
-----------------------------

Let me start from the end of the list. [FAKE](http://fsharp.github.io/FAKE/) is a F# build
automation system that does a lot more than just building. In fact, FAKE can easily call
MSBUILD scripts (and build F# projects just using an existing `fsproj` file). I think the
real value is in all the additional tools that it provides. 

For example, here is what happens when you run the [build script](https://github.com/fsharp/FSharp.Data/blob/master/build.fsx)
from the [F# Data library](https://github.com/fsharp/FSharp.Data). It:

 * Parses `RELEASE_NOTES.md` to get the information about the last version number
   and release notes (that will be used later to build NuGet package)
 * Generates `AssemblyInfo.fs` with the right version and project information
 * Builds the project and tests by calling MSBUILD (or xbuild) on `sln` files
 * Runs the NUnit tests (and stops if there is a failure), but more about testing later...
 * While running tests, it also checks that your documentation does not contain errors -
   if you do not believe, continue reading :-)
 * Builds a NuGet package and optionally pushes it to [nuget.org](http://nuget.org)
 * Automatically builds documentation using F# Formatting tool that is discussed next
 * As a bonus, it also pushes the [documentation to the gh-pages branch](http://fsharp.github.io/FSharp.Data/)
   and builds a ZIP with the binaries for easy download.

All this means that it is really easy to maintain a project. When you get a pull request
(and point the contributor to the right place to add tests and documentation), you can 
then update everything with just a single command. 

And you have a guarantee that your documentation is up-to-date and correct too, which
is done using another F# project that I'll discuss next...

Documenting libraries with F# Formatting
----------------------------------------

[F# Formatting](http://tpetricek.github.io/FSharp.Formatting/) is not your good old 
regular-expression based syntax highlighter. It calls the F# compiler (which is fully
[open-source](https://github.com/fsharp/fsharp), in case you did not know) and uses
the actual compiler to colorize code. Aside from that, it also type-checks the code
and extracts tooltip information that you'd see in MonoDevelop or Visual Studio.
It is used on this blog too, so here is an example (hover over identifiers with
mouse pointer to see tool tips):
*)
/// Say hello to the specified person
let hello person = 
  printfn "Hello %s!" person

hello "Tomas"
(**
For statically typed languages with type inference, this is extremely useful. Just 
remember when you were last looking at C# snippet using `var` and wondered what 
the type of a variable is...

To build a great documentation for a project using F# Formatting, you can use two
features. I'll use the [Deedle data manipulation library](http://bluemountaincapital.github.io/Deedle/)
as an example:

 * **Write tutorials** - these can be standard [F# script files](https://github.com/BlueMountainCapital/Deedle/blob/master/docs/content/tutorial.fsx)
   that you can run, with special comments written using `(** .. *)` that contain Markdown. F#
   Formatting turns them into [nicely formatted tutorials](http://bluemountaincapital.github.io/Deedle/tutorial.html)

 * **Generate API reference** - if you include `///` comments for public functions (written 
   in a [simple Markdown style](https://github.com/fsharp/FAKE/blob/develop/src/app/FakeLib/DocuHelper.fs#L27)), 
   you can automatically generate API reference from them, for example, like the 
   [FakeLib reference](http://fsharp.github.io/FAKE/apidocs/index.html).

Does your documentation type-check?
-----------------------------------

The last thing I mentioned is that the build process checks if your documentation is correct.
Obviously, it does not check that your documentation makes sense :-) but it does make sure
that code samples your documentation type check. This is done, for example, in the 
[F# Data documentation tests](https://github.com/fsharp/FSharp.Data/tree/master/tests/FSharp.Data.Tests.Documentation).

<div style="text-align:center">
<img src="testdoc.png" alt="Failing documentation tests, after API change"  />
</div>

What does this mean? When you change your API (add or remove parameters, change type, 
or rename function or types) without making corresponding changes to your documentation,
you'll get a unit test failure! 

This is only possible because F# Formatting can call
the compiler to do the actual formatting and checking work - and it does not only
work in `fsx` files. The same is done on `md` files that contain F# code snippets
(using 4 spaces before the snippet).

Testing with FsUnit and FsCheck
-------------------------------

Speaking of unit tests, there are a few more things to be written.
I'm not an expert when it comes to testing (the chapter by Phil Trelford in 
our [upcoming F# book](http://www.manning.com/petricek2/) is a better source!), but
tests are clearly important - especially for open-source projects with multiple
contributors that need to collaborate on the code base.

### Less painful writing and running
There are three things that make writing tests less painful. First, [FsUnit](https://github.com/fsharp/fsunit)
is a nice DSL for writing tests in a more readable way. Second, the F# ` ``backtick`` `
notation lets you use full description as a test name. And third, you can setup your
environment to make tests runnable really quickly from REPL. 

Let's look at a sample test for the XML type provider from [F# Data](https://github.com/fsharp/FSharp.Data):

*)
#if INTERACTIVE
#r "../../../bin/FSharp.Data.dll"
(*[omit:(other references omitted)]*)
#r "../../../packages/NUnit.2.6.3/lib/nunit.framework.dll"
#r "../../../packages/FsCheck.0.9.1.0/lib/net40-Client/FsCheck.dll"
#load "../../Common/FsUnit.fs"(*[/omit]*)
#else
module FSharp.Data.XmlTests
#endif

type PersonXml = XmlProvider<(*[omit:(...)]*)"""<authors><author name="Ludwig" surname="Wittgenstein" age="29" /></authors>"""(*[/omit]*)>
let newXml = """
  <authors>
    <author name="Jane" surname="Doe" age="23" />
  </authors>"""

[<Test>]
let ``Jane should have first name of Jane``() = 
    let firstPerson = PersonXml.Parse(newXml).Author
    firstPerson.Name |> should equal "Jane"
(**
The test is included in an `fs` file in a project that is compiled into a `dll` that can
be tested with standard NUnit test runners. However, the first 9 lines make the test also
runnable in F# Interactive - you can select the entire source code and hit `Alt+Enter` to
load the tests in F# Interactive and run them line-by-line, testing different inputs 
interactively. When writing tests, this is much easier then changing your code and re-compiling
tests to run them.

The test itself uses the backtic notation to include the whole test description in its
name `SoYouDoNotNeedToDecipherThis`! The FsUnit library that is also used here defines a 
simple readable DSL so that you can write your test in the form `<value> |> should <property>`.
For example, you can say `"Hello" |> should startWith "H"`. 

### Testing complex logic

Finally, the last great tool that I want to mention in this article is a random [testing
framework FsCheck](https://github.com/fsharp/FsCheck). This is particularly useful if you
need to test some algorithm or more complex function that has some (mathematical) properties.

For example, I wrote a function `binarySearchNearestGreater` that performs binary search
on a sorted array and returns the index of a specified element, or index of an element
that is the nearest greater in the array. The function has a property that the value
at the returned index is equal, or greater than the specified key (or, if the function
does not find any element, it means that all are smaller).

FsCheck can easily verify that the property holds for randomly generated inputs (and it
also generates inputs that cover corner cases):
*)
[<Test>]
let ``Binary searching for nearest greater value satisfies laws`` () =
  Check.QuickThrowOnFailure(fun (input:int[]) (key:int) -> 
    let input = Array.sort input
    match Array.binarySearchNearestGreater key comparer input with
    | Some idx -> input.[idx] >= key
    | None -> Seq.forall (fun v -> v < key) input )
(**
The operation `Check.QuickThrowOnFailure` takes a function that specifies the predicate
and automatically generates 100 (or more) random inputs for `input` and `key`. 
The above sample uses NUnit, but FsCheck also comes with xUnit integration that makes the 
testing code even simpler (just write a function with the `Property` attribute).

Random testing is certainly not useful for all tests, but it is great when you have
some property that should hold. This is often the case for algorithms, or when you 
have a pair of functions for converting "there and back again" (then you can just say
that the conversion there and back should return the original thing).

## Summary 

Building a great open-source library is a difficult thing and I certainly do not claim
that I have a recipe for that. But I'm contributing to [a](https://github.com/tpetricek/FSharp.Formatting)
[few](https://github.com/fsharp/FSharp.Data) [F#](https://github.com/fsharp/FSharp.Charting) 
[libraries](https://github.com/BlueMountainCapital/Deedle) and I think I have learned
a thing or two from my mistakes.

For me, one of the most difficult things (technically) is keeping libraries up-to-date
even when I don't have time for it. The best way to solve this is to automate everything
so that you can accept a pull request and run a single command that runs the whole build
process, including NuGet release, documentation update and as many sanity checks as possible,
both for the code itself and for the documentation.

This article gave a quick overview of the tools that make this amazingly easy with F# - 
including the awesome [FAKE build tool](http://fsharp.github.io/FAKE/), unit testing
tools like [FsUnit](https://github.com/fsharp/fsunit)  and [FsCheck](https://github.com/fsharp/FsCheck/blob/master/Docs/Documentation.md)
and documentation tools in [F# Formatting](http://tpetricek.github.io/FSharp.Formatting/index.html)
that can even be integrated with unit tests to make sure your documentation is correct.
 

*)
