(**
Announcing: Literate programming tools for F#
=============================================

 - date: 2013-01-22T17:35:36.0000000
 - description: This article introduces a new F# package that makes it possible to write literate F# programs that combine code with documentation. Given an F# script with a special comments or Markdown document with F# code, you get a nicely formatted HTML that can be used to build documentation or write blogs.
 - layout: article
 - tags: open source,f#,writing,literate
 - title: Announcing: Literate programming tools for F#
 - url: fsharp-literate-programming.aspx

--------------------------------------------------------------------------------
 - standalone

<img src="https://raw.github.com/tpetricek/FSharp.Formatting/master/docs/misc/logo.png" class="rdecor" style="width:120px;height:120px;" />

For some time now, I've been writing my F# blog posts (and other F# articles 
published elsewhere) by combining F# code snippets and Markdown formatting. In fact,
I even wrote a Markdown parser in F# so that I can post-process documents (to 
generate references etc). You can read about the Markdown parser in an upcoming
[F# Deep Dives](http://manning.com/petricek2/) book - currently, it is available 
as a free chapter!

During the Christmas break, I finally found enough time to clean-up the code I
was using and package it properly into a documented library that is easy to install and use.
Here are the most important links:

 - [F# Formatting home page](https://fsprojects.github.io/FSharp.Formatting/)
 - [F# Formatting source code](https://github.com/fsprojects/FSharp.Formatting) on GitHub
 - [F# Formatting package](https://nuget.org/packages/FSharp.Formatting) on NuGet

To learn more about this tool and how to use it, [continue reading](http://tomasp.net/blog/fsharp-literate-programming.aspx)!


--------------------------------------------------------------------------------


<img src="https://raw.github.com/tpetricek/FSharp.Formatting/master/docs/misc/logo.png" class="rdecor" style="width:120px;height:120px;" />

For some time now, I've been writing my F# blog posts (and other F# articles 
published elsewhere) by combining F# code snippets and Markdown formatting. In fact,
I even wrote a Markdown parser in F# so that I can post-process documents (to 
generate references etc). You can read about the Markdown parser in an upcoming
[F# Deep Dives](http://manning.com/petricek2/) book - currently, it is available 
as a free chapter!

During the Christmas break, I finally found enough time to clean-up the code I
was using and package it properly into a documented library that is easy to install and use.
Here are the most important links:

 - [F# Formatting home page](https://fsprojects.github.io/FSharp.Formatting/)
 - [F# Formatting source code](https://github.com/fsprojects/FSharp.Formatting) on GitHub
 - [F# Formatting package](https://nuget.org/packages/FSharp.Formatting) on NuGet

Introducing literate programming
--------------------------------

The package consists of two separate components - `FSharp.Markdown.dll` implements the
Markdown parser and `FSharp.CodeFormat.dll` implements tools for formatting of F# code
(with colorization and tool-tips) using the F# compiler API. 
However, the most interesting new part of the package is the `Literate.fsx` script 
that implements the idea of _literate programming_ for F#. WikiPedia defines 
[literate programming](http://en.wikipedia.org/wiki/Literate_programming) as follows:

> A literate program is an explanation of the program logic in a natural language, 
such as English, interspersed with snippets of macros and traditional source code. 

The F# incarnation of this idea is a script that makes it possible to generate nice 
readable HTML from a file that combines F# and Markdown. There are two options:

**Documents are F# script files** (`*.fsx`) and contain special comments with 
documentation in Markdown and commands for generating HTML output. The following
[sample page](http://tpetricek.github.com/FSharp.Formatting/sidescript.html) demonstrates
how to write literate F# scripts by showing the source and the output side-by-side:

<div style="text-align:center;margin-left:auto;margin-right:auto">
<a href="http://tpetricek.github.com/FSharp.Formatting/sidescript.html">
<img src="http://tomasp.net/articles/fsharp-literate-programming/sidebyside-script.png"
  style="border:none" /></a></div>

**Documents are Markdown documents** (`*.md`) and contain blocks of F# code (indented 
by four spaces as usual in Markdown) and optionally use special commands. The following
[sample page](http://tpetricek.github.com/FSharp.Formatting/sidemarkdown.html) demonstrates
how to write literate Markdown documents with F# code by showing the source and the output 
side-by-side:

<div style="text-align:center;margin-left:auto;margin-right:auto">
<a href="http://tpetricek.github.com/FSharp.Formatting/sidemarkdown.html">
<img src="http://tomasp.net/articles/fsharp-literate-programming/sidebyside-md.png"
  style="border:none" /></a></div>

Which option is better depends on whether you prefer to write code in F# editro in 
Visual Studio (with all text in comments) or in some Markdown editor (without 
syntax highlighting for F# snippets). 

## Writing F# Script files

The following example shows most of the features that can be used in a literate
F# script file. The tool looks for multi-line comments that start with double asterisk
or triple asterisk. Most of the features should be quite self-explanatory:


    (**
    # First-level heading
    Some more documentation using `Markdown`.
    *)
    
    (*** include: final-sample ***)

    (** 
    ## Second-level heading
    With some more documentation
    *)
    
    (*** define: final-sample ***)
    let helloWorld() = printfn "Hello world!"

The F# script files is processed as follows:

 - A multi-line comment starting with `(**` and ending with `*)` is 
   turned into text and is processed using the F# Markdown processor 
   (which supports standard Markdown commands).

 - A single-line comment starting with `(***` and ending with `***)` 
   is treated as a special command. The command can consist of 
   `key: value` or `key=value` pairs or just `key` command.

Two of the supported commands are `define`, which defines a named
snippet (such as `final-sample`) and removes the command together with 
the following F# code block from the main document. The snippet can then
be inserted elsewhere in the document using `include`. This makes it
possible to write documents without the ordering requirements of the
F# language.

Another supported command is `hide` (without a value) which specifies that the
following F# code block (until the next comment or command) should be 
omitted from the output.

## Writing Markdown documents

In the Markdown mode, the entire file is a valid Markdown document, which may
contain F# code snippets (but also other code snippets). As usual, snippets are
indented with four spaces. In addition, the snippets can be annotated with special
commands. Some of them are demonstrated in the following example: 

    # First-level heading

        [hide]
        let print s = printfn "%s" s

    Some more documentation using `Markdown`.

        [module=Hello]
        let helloWorld() = print "Hello world!"

    ## Second-level heading
    With some more documentation

        [lang=csharp]
        Console.WriteLine("Hello world!");

When processing the document, all F# snippets are copied to a separate file that
is type-checked using the F# compiler (to obtain colours and tool tips).
The commands are written on the first line of the snippet, wrapped in `[...]`:

 - The `hide` command specifies that the F# snippet should not be included in the
   final document. This can be used to include code that is needed to type-check
   the code, but is not visible to the reader.

 - The `module=Foo` command can be used to specify F# `module` where the snippet
   is placed. Use this command if you need multiple versions of the same snippet
   or if you need to separate code from different snippets.

 - The `lang=foo` command specifies that the language of the snippet. If the language
   is other than `fsharp`, the snippet is copied to the output as `<pre>` HTML
   tag without any processing.

## Getting and using the F# literate script

You should have no difficulties with installing the F# Formatting package. Just 
search for it in NuGet gallery or use the following command in Package Manager Console:

    Install-Package FSharp.Formatting

The package will be located in a folder such as `FSharp.Formatting.1.0.11` (depending
on the version). The subdirectory `lib/net40` contains the two assemblies together 
with `FSharp.CompilerBinding.dll` (from [F# binding](https://github.com/fsharp/fsharpbinding)),
which encapsulates the F# compiler API. If you're interested in literate programming, you need to look in the
`literate` subdirectory. It contains the `literate.fsx` script (which contains the 
implementation) together with `demo.fsx` that shows how to use it to process individual
files or an entire directory.

Assuming you installed a version 1.0.11 of the package, and you have an F# Script
file (such as `build.fsx`) in the solution folder, you can load the literate 
programming script as follows (the exact path may slightly vary):
*)
#I "packages/FSharp.Formatting.1.0.11/lib/net40"
#load "packages/FSharp.Formatting.1.0.11/literate/literate.fsx"
open FSharp.Literate
open System.IO
(**
The first line tells F# interactive to automatically search for `*.dll` assemblies
in the directory where `FSharp.CodeFormat.dll` and `FSharp.Markdown.dll` are located.
This is required by the second line, which loads the `literate.fsx` script.

Now we can open `FSharp.Literate` and use the `Literate` type to process individual
documents or entire directories.

### Processing individual files

The `Literate` type has two static methods `ProcessScriptFile` and `ProcessMarkdown`
that turn an F# script file and Markdown document, respectively, into an HTML file.
To specify the HTML file structure, you need to provide a template. Two sample templates
are included: for a [single file](https://github.com/tpetricek/FSharp.Formatting/blob/master/literate/templates/template-file.html)
and for a [project](https://github.com/tpetricek/FSharp.Formatting/blob/master/literate/templates/template-project.html),
but you can use your own.

The template should include two parameters that will be replaced with the actual
HTML: `{document}` will be replaced with the formatted document; `{tooltips}` will be
replaced with (hidden) `<div>` elements containing code for tool tips that appear
when you place mouse pointer over an identifier. Optionally, you can also use 
`{page-title}` which will be replaced with the text in a first-level heading.
The template should also reference `style.css` and `tips.js` that define CSS style
and JavaScript functions used by the generated HTML (see sample [stylesheet](https://github.com/tpetricek/FSharp.Formatting/blob/master/literate/content/style.css)
and [script](https://github.com/tpetricek/FSharp.Formatting/blob/master/literate/content/tips.js) on GitHub).

Assuming you have `template.html` in the current directory, you can write:
*)

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(source, "template.html")

(**
Then you can use the two static methods to turn single documents into HTML
as follows:
*)

let script = Path.Combine(source, "docs/script.fsx")
Literate.ProcessScriptFile(script, template)

let doc = Path.Combine(source, "docs/document.md")
Literate.ProcessMarkdown(doc, template)

(**
This sample uses `*.md` extension for Markdown documents, but this is not required when
using `ProcessMarkdown`. You can use any extension you wish. By default, the methods
will generate file with the same name (but with the `.html` extension). You can change
this by addint a third parameter with the output file name. There is a number of 
additional parameters you can specify - these are discussed below.

### Processing entire directories

If you have multiple script files and Markdown documents (this time, they need to have
the `*.md` file extension) in a single directory, you can run the tool on a directory.
It will also automatically check that files are re-generated only when they were changed.
The following sample also uses optional parameter `replacements` to specify additional
keywords that will be replaced in the template file (this matches the `template-project.html`
file which is included as a sample in the package):
*)

// Load the template & specify project information
let template = source + "template-project.html"
let projInfo =
  [ "page-description", "F# Literate programming"
    "page-author", "Tomas Petricek"
    "github-link", "https://github.com/tpetricek/FSharp.Formatting"
    "project-name", "F# Formatting" ]

// Process all files and save results to 'output' directory
Literate.ProcessDirectory
  (source, template, source + "\\output", replacements = projInfo)

(**
The sample template `template-project.html` has been used to generate this documentation
and it includes additional parameters for specifying various information about F#
projects.

### Optional parameters

All of the three methods discussed in the previous two sections take a number of optional
parameters that can be used to tweak how the formatting works or even to specify a different
version of the F# compiler:

 - `fsharpCompiler` - a `System.Reflection.Assembly` object that represents the 
   `FSharp.Compiler.dll` assembly that should be used for processing the snippets
   (specify this if you want to use custom version of the compiler!)
 - `prefix` - a string that is added to all automatically generated `id` attributes
   in the generated HTML document (to avoid collisions with other HTML elements)
 - `compilerOptions` - this can be used to pass any additional command line 
   parameters to the F# compiler (you can use any standard parameters of `fsc.exe`)
 - `lineNumbers` - if `true` then the generated F# snippets include line numbers.
 - `references` - if `true` then the script automatically adds "References" 
   section with all indirect links that are defined & used in the document.
 - `replacements` - a list of key-value pairs containing additional parameters
   that should be replaced in the tempalte HTML file.
 - `includeSource` - when `true`, parameter `{source}` will be replaced with a 
   `<pre>` tag containing the original source code of the F# Script or Markdown document.

Summary
-------

In this article, I demonstrated some of the capabilities of the [F# Formatting](http://tpetricek.github.com/FSharp.Formatting)
library. The library consists of two projects - an F# implementation of Markdown processor
and a tool for formatting F# code using the F# compiler service. These two are combined
to provide an easy to use literate programming tools.

If you want to see the literate programming tools, check out the documentation for 
the [F# Data library](http://tpetricek.github.com/FSharp.Data) (I will write about it
soon in a separate blog post). The documenation is generated from `*.fsx` files in 
the `samples` directory [on GitHub](https://github.com/tpetricek/FSharp.Data/tree/master/samples).
Other examples using the library include this blog and the documentation for [F# 
Formatting](http://tpetricek.github.com/FSharp.Formatting/) itself.


*)
