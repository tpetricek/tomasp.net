Announcing: Literate programming tools for F#
=============================================

 - date: 2013-01-22T17:35:36.0000000
 - description: This article introduces a new F# package that makes it possible to write literate F# programs that combine code with documentation. Given an F# script with a special comments or Markdown document with F# code, you get a nicely formatted HTML that can be used to build documentation or write blogs.
 - layout: article
 - tags: open source,f#,writing,literate
 - title: Announcing: Literate programming tools for F#
 - url: fsharp-literate-programming.aspx
 - rawbody: true

--------------------------------------------------------------------------------
<img src="https://raw.github.com/tpetricek/FSharp.Formatting/master/docs/misc/logo.png" class="rdecor" style="width:120px;height:120px;" />
<p>For some time now, I've been writing my F# blog posts (and other F# articles
published elsewhere) by combining F# code snippets and Markdown formatting. In fact,
I even wrote a Markdown parser in F# so that I can post-process documents (to
generate references etc). You can read about the Markdown parser in an upcoming
<a href="http://manning.com/petricek2/">F# Deep Dives</a> book - currently, it is available
as a free chapter!</p>
<p>During the Christmas break, I finally found enough time to clean-up the code I
was using and package it properly into a documented library that is easy to install and use.
Here are the most important links:</p>
<ul>
<li><a href="https://fsprojects.github.io/FSharp.Formatting/">F# Formatting home page</a></li>
<li><a href="https://github.com/fsprojects/FSharp.Formatting">F# Formatting source code</a> on GitHub</li>
<li><a href="https://nuget.org/packages/FSharp.Formatting">F# Formatting package</a> on NuGet</li>
</ul>
<p>To learn more about this tool and how to use it, <a href="http://tomasp.net/blog/fsharp-literate-programming.aspx">continue reading</a>!</p>
--------------------------------------------------------------------------------
<h1><span class="hm">Announcing</span><span class="hs"> Literate programming tools for F#</span></h1>
<img src="https://raw.github.com/tpetricek/FSharp.Formatting/master/docs/misc/logo.png" class="rdecor" style="width:120px;height:120px;" />
<p>For some time now, I've been writing my F# blog posts (and other F# articles
published elsewhere) by combining F# code snippets and Markdown formatting. In fact,
I even wrote a Markdown parser in F# so that I can post-process documents (to
generate references etc). You can read about the Markdown parser in an upcoming
<a href="http://manning.com/petricek2/">F# Deep Dives</a> book - currently, it is available
as a free chapter!</p>
<p>During the Christmas break, I finally found enough time to clean-up the code I
was using and package it properly into a documented library that is easy to install and use.
Here are the most important links:</p>
<ul>
<li><a href="https://fsprojects.github.io/FSharp.Formatting/">F# Formatting home page</a></li>
<li><a href="https://github.com/fsprojects/FSharp.Formatting">F# Formatting source code</a> on GitHub</li>
<li><a href="https://nuget.org/packages/FSharp.Formatting">F# Formatting package</a> on NuGet</li>
</ul>
<h2>Introducing literate programming</h2>
<p>The package consists of two separate components - <code>FSharp.Markdown.dll</code> implements the
Markdown parser and <code>FSharp.CodeFormat.dll</code> implements tools for formatting of F# code
(with colorization and tool-tips) using the F# compiler API.
However, the most interesting new part of the package is the <code>Literate.fsx</code> script
that implements the idea of <em>literate programming</em> for F#. WikiPedia defines
<a href="http://en.wikipedia.org/wiki/Literate_programming">literate programming</a> as follows:</p>
<blockquote>
<p>A literate program is an explanation of the program logic in a natural language,
such as English, interspersed with snippets of macros and traditional source code.</p>
</blockquote>
<p>The F# incarnation of this idea is a script that makes it possible to generate nice
readable HTML from a file that combines F# and Markdown. There are two options:</p>
<p><strong>Documents are F# script files</strong> (<code>*.fsx</code>) and contain special comments with
documentation in Markdown and commands for generating HTML output. The following
<a href="http://tpetricek.github.com/FSharp.Formatting/sidescript.html">sample page</a> demonstrates
how to write literate F# scripts by showing the source and the output side-by-side:</p>
<div style="text-align:center;margin-left:auto;margin-right:auto">
<a href="http://tpetricek.github.com/FSharp.Formatting/sidescript.html">
<img src="http://tomasp.net/articles/fsharp-literate-programming/sidebyside-script.png"
  style="border:none" /></a></div>
<p><strong>Documents are Markdown documents</strong> (<code>*.md</code>) and contain blocks of F# code (indented
by four spaces as usual in Markdown) and optionally use special commands. The following
<a href="http://tpetricek.github.com/FSharp.Formatting/sidemarkdown.html">sample page</a> demonstrates
how to write literate Markdown documents with F# code by showing the source and the output
side-by-side:</p>
<div style="text-align:center;margin-left:auto;margin-right:auto">
<a href="http://tpetricek.github.com/FSharp.Formatting/sidemarkdown.html">
<img src="http://tomasp.net/articles/fsharp-literate-programming/sidebyside-md.png"
  style="border:none" /></a></div>
<p>Which option is better depends on whether you prefer to write code in F# editro in
Visual Studio (with all text in comments) or in some Markdown editor (without
syntax highlighting for F# snippets).</p>
<h2>Writing F# Script files</h2>
<p>The following example shows most of the features that can be used in a literate
F# script file. The tool looks for multi-line comments that start with double asterisk
or triple asterisk. Most of the features should be quite self-explanatory:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l"> 1: </span>
<span class="l"> 2: </span>
<span class="l"> 3: </span>
<span class="l"> 4: </span>
<span class="l"> 5: </span>
<span class="l"> 6: </span>
<span class="l"> 7: </span>
<span class="l"> 8: </span>
<span class="l"> 9: </span>
<span class="l">10: </span>
<span class="l">11: </span>
<span class="l">12: </span>
<span class="l">13: </span>
<span class="l">14: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="c">(**</span>
<span class="c"># First-level heading</span>
<span class="c">Some more documentation using `Markdown`.</span>
<span class="c">*)</span>

<span class="c">(*** include: final-sample ***)</span>

<span class="c">(** </span>
<span class="c">## Second-level heading</span>
<span class="c">With some more documentation</span>
<span class="c">*)</span>

<span class="c">(*** define: final-sample ***)</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="fn">helloWorld</span><span class="pn">(</span><span class="pn">)</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="fn">printfn</span> <span class="s">&quot;Hello world!&quot;</span>
</code></pre></td>
</tr>
</table>
<p>The F# script files is processed as follows:</p>
<ul>
<li>
<p>A multi-line comment starting with <code>(**</code> and ending with <code>*)</code> is
turned into text and is processed using the F# Markdown processor
(which supports standard Markdown commands).</p>
</li>
<li>
<p>A single-line comment starting with <code>(***</code> and ending with <code>***)</code>
is treated as a special command. The command can consist of
<code>key: value</code> or <code>key=value</code> pairs or just <code>key</code> command.</p>
</li>
</ul>
<p>Two of the supported commands are <code>define</code>, which defines a named
snippet (such as <code>final-sample</code>) and removes the command together with
the following F# code block from the main document. The snippet can then
be inserted elsewhere in the document using <code>include</code>. This makes it
possible to write documents without the ordering requirements of the
F# language.</p>
<p>Another supported command is <code>hide</code> (without a value) which specifies that the
following F# code block (until the next comment or command) should be
omitted from the output.</p>
<h2>Writing Markdown documents</h2>
<p>In the Markdown mode, the entire file is a valid Markdown document, which may
contain F# code snippets (but also other code snippets). As usual, snippets are
indented with four spaces. In addition, the snippets can be annotated with special
commands. Some of them are demonstrated in the following example:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l"> 1: </span>
<span class="l"> 2: </span>
<span class="l"> 3: </span>
<span class="l"> 4: </span>
<span class="l"> 5: </span>
<span class="l"> 6: </span>
<span class="l"> 7: </span>
<span class="l"> 8: </span>
<span class="l"> 9: </span>
<span class="l">10: </span>
<span class="l">11: </span>
<span class="l">12: </span>
<span class="l">13: </span>
<span class="l">14: </span>
<span class="l">15: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="pn">#</span><span class="id">First</span><span class="o">-</span><span class="id">level</span> <span class="id">heading</span>

    <span class="pn">[</span><span class="id">hide</span><span class="pn">]</span>
    <span class="k">let</span> <span class="id">print</span> <span class="id">s</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs2', 3)" onmouseover="showTip(event, 'fs2', 3)" class="id">printfn</span> <span class="s">&quot;%s&quot;</span> <span class="id">s</span>

<span onmouseout="hideTip(event, 'fs3', 4)" onmouseover="showTip(event, 'fs3', 4)" class="id">Some</span> <span class="id">more</span> <span class="id">documentation</span> <span onmouseout="hideTip(event, 'fs4', 5)" onmouseover="showTip(event, 'fs4', 5)" class="id">using</span> <span class="k">`</span><span class="id">Markdown</span><span class="k">`</span><span class="pn">.</span>

    <span class="pn">[</span><span class="k">module</span><span class="o">=</span><span class="id">Hello</span><span class="pn">]</span>
    <span class="k">let</span> <span onmouseout="hideTip(event, 'fs1', 6)" onmouseover="showTip(event, 'fs1', 6)" class="id">helloWorld</span><span class="pn">(</span><span class="pn">)</span> <span class="o">=</span> <span class="id">print</span> <span class="s">&quot;Hello world!&quot;</span>

<span class="pn">#</span> <span class="id">Second</span><span class="o">-</span><span class="id">level</span> <span class="id">heading</span>
<span class="id">With</span> <span class="id">some</span> <span class="id">more</span> <span class="id">documentation</span>

    <span class="pn">[</span><span class="id">lang</span><span class="o">=</span><span class="id">csharp</span><span class="pn">]</span>
    <span class="id">Console</span><span class="pn">.</span><span class="id">WriteLine</span><span class="pn">(</span><span class="s">&quot;Hello world!&quot;</span><span class="pn">)</span><span class="pn">;</span>
</code></pre></td>
</tr>
</table>
<p>When processing the document, all F# snippets are copied to a separate file that
is type-checked using the F# compiler (to obtain colours and tool tips).
The commands are written on the first line of the snippet, wrapped in <code>[...]</code>:</p>
<ul>
<li>
<p>The <code>hide</code> command specifies that the F# snippet should not be included in the
final document. This can be used to include code that is needed to type-check
the code, but is not visible to the reader.</p>
</li>
<li>
<p>The <code>module=Foo</code> command can be used to specify F# <code>module</code> where the snippet
is placed. Use this command if you need multiple versions of the same snippet
or if you need to separate code from different snippets.</p>
</li>
<li>
<p>The <code>lang=foo</code> command specifies that the language of the snippet. If the language
is other than <code>fsharp</code>, the snippet is copied to the output as <code>&lt;pre&gt;</code> HTML
tag without any processing.</p>
</li>
</ul>
<h2>Getting and using the F# literate script</h2>
<p>You should have no difficulties with installing the F# Formatting package. Just
search for it in NuGet gallery or use the following command in Package Manager Console:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="id">Install</span><span class="o">-</span><span class="id">Package</span> <span onmouseout="hideTip(event, 'fs5', 7)" onmouseover="showTip(event, 'fs5', 7)" class="id">FSharp</span><span class="pn">.</span><span class="id">Formatting</span>
</code></pre></td>
</tr>
</table>
<p>The package will be located in a folder such as <code>FSharp.Formatting.1.0.11</code> (depending
on the version). The subdirectory <code>lib/net40</code> contains the two assemblies together
with <code>FSharp.CompilerBinding.dll</code> (from <a href="https://github.com/fsharp/fsharpbinding">F# binding</a>),
which encapsulates the F# compiler API. If you're interested in literate programming, you need to look in the
<code>literate</code> subdirectory. It contains the <code>literate.fsx</code> script (which contains the
implementation) together with <code>demo.fsx</code> that shows how to use it to process individual
files or an entire directory.</p>
<p>Assuming you installed a version 1.0.11 of the package, and you have an F# Script
file (such as <code>build.fsx</code>) in the solution folder, you can load the literate
programming script as follows (the exact path may slightly vary):</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="pp">#I</span> <span class="s">&quot;packages/FSharp.Formatting.1.0.11/lib/net40&quot;</span>
<span class="pp">#load</span> <span class="s">&quot;packages/FSharp.Formatting.1.0.11/literate/literate.fsx&quot;</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs6', 8)" onmouseover="showTip(event, 'fs6', 8)" class="id">FSharp</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs7', 9)" onmouseover="showTip(event, 'fs7', 9)" class="id">Literate</span>
<span class="k">open</span> <span onmouseout="hideTip(event, 'fs8', 10)" onmouseover="showTip(event, 'fs8', 10)" class="id">System</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs9', 11)" onmouseover="showTip(event, 'fs9', 11)" class="id">IO</span>
</code></pre></td>
</tr>
</table>
<p>The first line tells F# interactive to automatically search for <code>*.dll</code> assemblies
in the directory where <code>FSharp.CodeFormat.dll</code> and <code>FSharp.Markdown.dll</code> are located.
This is required by the second line, which loads the <code>literate.fsx</code> script.</p>
<p>Now we can open <code>FSharp.Literate</code> and use the <code>Literate</code> type to process individual
documents or entire directories.</p>
<h3>Processing individual files</h3>
<p>The <code>Literate</code> type has two static methods <code>ProcessScriptFile</code> and <code>ProcessMarkdown</code>
that turn an F# script file and Markdown document, respectively, into an HTML file.
To specify the HTML file structure, you need to provide a template. Two sample templates
are included: for a <a href="https://github.com/tpetricek/FSharp.Formatting/blob/master/literate/templates/template-file.html">single file</a>
and for a <a href="https://github.com/tpetricek/FSharp.Formatting/blob/master/literate/templates/template-project.html">project</a>,
but you can use your own.</p>
<p>The template should include two parameters that will be replaced with the actual
HTML: <code>{document}</code> will be replaced with the formatted document; <code>{tooltips}</code> will be
replaced with (hidden) <code>&lt;div&gt;</code> elements containing code for tool tips that appear
when you place mouse pointer over an identifier. Optionally, you can also use
<code>{page-title}</code> which will be replaced with the text in a first-level heading.
The template should also reference <code>style.css</code> and <code>tips.js</code> that define CSS style
and JavaScript functions used by the generated HTML (see sample <a href="https://github.com/tpetricek/FSharp.Formatting/blob/master/literate/content/style.css">stylesheet</a>
and <a href="https://github.com/tpetricek/FSharp.Formatting/blob/master/literate/content/tips.js">script</a> on GitHub).</p>
<p>Assuming you have <code>template.html</code> in the current directory, you can write:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs10', 12)" onmouseover="showTip(event, 'fs10', 12)" class="id">source</span> <span class="o">=</span> <span class="k">__SOURCE_DIRECTORY__</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs11', 13)" onmouseover="showTip(event, 'fs11', 13)" class="id">template</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs12', 14)" onmouseover="showTip(event, 'fs12', 14)" class="rt">Path</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs13', 15)" onmouseover="showTip(event, 'fs13', 15)" class="id">Combine</span><span class="pn">(</span><span onmouseout="hideTip(event, 'fs10', 16)" onmouseover="showTip(event, 'fs10', 16)" class="id">source</span><span class="pn">,</span> <span class="s">&quot;template.html&quot;</span><span class="pn">)</span>
</code></pre></td>
</tr>
</table>
<p>Then you can use the two static methods to turn single documents into HTML
as follows:</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
<span class="l">2: </span>
<span class="l">3: </span>
<span class="l">4: </span>
<span class="l">5: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs14', 17)" onmouseover="showTip(event, 'fs14', 17)" class="id">script</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs12', 18)" onmouseover="showTip(event, 'fs12', 18)" class="rt">Path</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs13', 19)" onmouseover="showTip(event, 'fs13', 19)" class="id">Combine</span><span class="pn">(</span><span onmouseout="hideTip(event, 'fs10', 20)" onmouseover="showTip(event, 'fs10', 20)" class="id">source</span><span class="pn">,</span> <span class="s">&quot;docs/script.fsx&quot;</span><span class="pn">)</span>
<span onmouseout="hideTip(event, 'fs15', 21)" onmouseover="showTip(event, 'fs15', 21)" class="rt">Literate</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs16', 22)" onmouseover="showTip(event, 'fs16', 22)" class="id">ProcessScriptFile</span><span class="pn">(</span><span onmouseout="hideTip(event, 'fs14', 23)" onmouseover="showTip(event, 'fs14', 23)" class="id">script</span><span class="pn">,</span> <span onmouseout="hideTip(event, 'fs11', 24)" onmouseover="showTip(event, 'fs11', 24)" class="id">template</span><span class="pn">)</span>

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs17', 25)" onmouseover="showTip(event, 'fs17', 25)" class="id">doc</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs12', 26)" onmouseover="showTip(event, 'fs12', 26)" class="rt">Path</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs13', 27)" onmouseover="showTip(event, 'fs13', 27)" class="id">Combine</span><span class="pn">(</span><span onmouseout="hideTip(event, 'fs10', 28)" onmouseover="showTip(event, 'fs10', 28)" class="id">source</span><span class="pn">,</span> <span class="s">&quot;docs/document.md&quot;</span><span class="pn">)</span>
<span onmouseout="hideTip(event, 'fs15', 29)" onmouseover="showTip(event, 'fs15', 29)" class="rt">Literate</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs18', 30)" onmouseover="showTip(event, 'fs18', 30)" class="id">ProcessMarkdown</span><span class="pn">(</span><span onmouseout="hideTip(event, 'fs17', 31)" onmouseover="showTip(event, 'fs17', 31)" class="id">doc</span><span class="pn">,</span> <span onmouseout="hideTip(event, 'fs11', 32)" onmouseover="showTip(event, 'fs11', 32)" class="id">template</span><span class="pn">)</span>
</code></pre></td>
</tr>
</table>
<p>This sample uses <code>*.md</code> extension for Markdown documents, but this is not required when
using <code>ProcessMarkdown</code>. You can use any extension you wish. By default, the methods
will generate file with the same name (but with the <code>.html</code> extension). You can change
this by addint a third parameter with the output file name. There is a number of
additional parameters you can specify - these are discussed below.</p>
<h3>Processing entire directories</h3>
<p>If you have multiple script files and Markdown documents (this time, they need to have
the <code>*.md</code> file extension) in a single directory, you can run the tool on a directory.
It will also automatically check that files are re-generated only when they were changed.
The following sample also uses optional parameter <code>replacements</code> to specify additional
keywords that will be replaced in the template file (this matches the <code>template-project.html</code>
file which is included as a sample in the package):</p>
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l"> 1: </span>
<span class="l"> 2: </span>
<span class="l"> 3: </span>
<span class="l"> 4: </span>
<span class="l"> 5: </span>
<span class="l"> 6: </span>
<span class="l"> 7: </span>
<span class="l"> 8: </span>
<span class="l"> 9: </span>
<span class="l">10: </span>
<span class="l">11: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="c">// Load the template &amp; specify project information</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs11', 33)" onmouseover="showTip(event, 'fs11', 33)" class="id">template</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs10', 34)" onmouseover="showTip(event, 'fs10', 34)" class="id">source</span> <span class="o">+</span> <span class="s">&quot;template-project.html&quot;</span>
<span class="k">let</span> <span onmouseout="hideTip(event, 'fs19', 35)" onmouseover="showTip(event, 'fs19', 35)" class="id">projInfo</span> <span class="o">=</span>
  <span class="pn">[</span> <span class="s">&quot;page-description&quot;</span><span class="pn">,</span> <span class="s">&quot;F# Literate programming&quot;</span>
    <span class="s">&quot;page-author&quot;</span><span class="pn">,</span> <span class="s">&quot;Tomas Petricek&quot;</span>
    <span class="s">&quot;github-link&quot;</span><span class="pn">,</span> <span class="s">&quot;https://github.com/tpetricek/FSharp.Formatting&quot;</span>
    <span class="s">&quot;project-name&quot;</span><span class="pn">,</span> <span class="s">&quot;F# Formatting&quot;</span> <span class="pn">]</span>

<span class="c">// Process all files and save results to &#39;output&#39; directory</span>
<span onmouseout="hideTip(event, 'fs15', 36)" onmouseover="showTip(event, 'fs15', 36)" class="rt">Literate</span><span class="pn">.</span><span onmouseout="hideTip(event, 'fs20', 37)" onmouseover="showTip(event, 'fs20', 37)" class="id">ProcessDirectory</span>
  <span class="pn">(</span><span onmouseout="hideTip(event, 'fs10', 38)" onmouseover="showTip(event, 'fs10', 38)" class="id">source</span><span class="pn">,</span> <span onmouseout="hideTip(event, 'fs11', 39)" onmouseover="showTip(event, 'fs11', 39)" class="id">template</span><span class="pn">,</span> <span onmouseout="hideTip(event, 'fs10', 40)" onmouseover="showTip(event, 'fs10', 40)" class="id">source</span> <span class="o">+</span> <span class="s">&quot;\\output&quot;</span><span class="pn">,</span> <span class="id">replacements</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs19', 41)" onmouseover="showTip(event, 'fs19', 41)" class="id">projInfo</span><span class="pn">)</span>
</code></pre></td>
</tr>
</table>
<p>The sample template <code>template-project.html</code> has been used to generate this documentation
and it includes additional parameters for specifying various information about F#
projects.</p>
<h3>Optional parameters</h3>
<p>All of the three methods discussed in the previous two sections take a number of optional
parameters that can be used to tweak how the formatting works or even to specify a different
version of the F# compiler:</p>
<ul>
<li>
<code>fsharpCompiler</code> - a <code>System.Reflection.Assembly</code> object that represents the 
<code>FSharp.Compiler.dll</code> assembly that should be used for processing the snippets
(specify this if you want to use custom version of the compiler!)
</li>
<li>
<code>prefix</code> - a string that is added to all automatically generated <code>id</code> attributes
in the generated HTML document (to avoid collisions with other HTML elements)
</li>
<li>
<code>compilerOptions</code> - this can be used to pass any additional command line 
parameters to the F# compiler (you can use any standard parameters of <code>fsc.exe</code>)
</li>
<li><code>lineNumbers</code> - if <code>true</code> then the generated F# snippets include line numbers.</li>
<li>
<code>references</code> - if <code>true</code> then the script automatically adds "References" 
section with all indirect links that are defined &amp; used in the document.
</li>
<li>
<code>replacements</code> - a list of key-value pairs containing additional parameters
that should be replaced in the tempalte HTML file.
</li>
<li>
<code>includeSource</code> - when <code>true</code>, parameter <code>{source}</code> will be replaced with a 
<code>&lt;pre&gt;</code> tag containing the original source code of the F# Script or Markdown document.
</li>
</ul>
<h2>Summary</h2>
<p>In this article, I demonstrated some of the capabilities of the <a href="http://tpetricek.github.com/FSharp.Formatting">F# Formatting</a>
library. The library consists of two projects - an F# implementation of Markdown processor
and a tool for formatting F# code using the F# compiler service. These two are combined
to provide an easy to use literate programming tools.</p>
<p>If you want to see the literate programming tools, check out the documentation for
the <a href="http://tpetricek.github.com/FSharp.Data">F# Data library</a> (I will write about it
soon in a separate blog post). The documenation is generated from <code>*.fsx</code> files in
the <code>samples</code> directory <a href="https://github.com/tpetricek/FSharp.Data/tree/master/samples">on GitHub</a>.
Other examples using the library include this blog and the documentation for <a href="http://tpetricek.github.com/FSharp.Formatting/">F#
Formatting</a> itself.</p>
<div class="tip" id="fs1">val helloWorld : unit -&gt; unit</div>
<div class="tip" id="fs2">val printfn : format:Printf.TextWriterFormat&lt;&#39;T&gt; -&gt; &#39;T</div>
<div class="tip" id="fs3">union case Option.Some: Value: &#39;T -&gt; Option&lt;&#39;T&gt;</div>
<div class="tip" id="fs4">val using : resource:&#39;T -&gt; action:(&#39;T -&gt; &#39;U) -&gt; &#39;U (requires &#39;T :&gt; System.IDisposable)</div>
<div class="tip" id="fs5">namespace Microsoft.FSharp</div>
<div class="tip" id="fs6">Multiple items<br />namespace FSharp<br /><br />--------------------<br />namespace Microsoft.FSharp</div>
<div class="tip" id="fs7">namespace FSharp.Literate</div>
<div class="tip" id="fs8">namespace System</div>
<div class="tip" id="fs9">namespace System.IO</div>
<div class="tip" id="fs10">val source : string</div>
<div class="tip" id="fs11">val template : string</div>
<div class="tip" id="fs12">type Path =<br />&#160;&#160;static val DirectorySeparatorChar : char<br />&#160;&#160;static val AltDirectorySeparatorChar : char<br />&#160;&#160;static val VolumeSeparatorChar : char<br />&#160;&#160;static val InvalidPathChars : char[]<br />&#160;&#160;static val PathSeparator : char<br />&#160;&#160;static member ChangeExtension : path:string * extension:string -&gt; string<br />&#160;&#160;static member Combine : [&lt;ParamArray&gt;] paths:string[] -&gt; string + 3 overloads<br />&#160;&#160;static member GetDirectoryName : path:string -&gt; string<br />&#160;&#160;static member GetExtension : path:string -&gt; string<br />&#160;&#160;static member GetFileName : path:string -&gt; string<br />&#160;&#160;...</div>
<div class="tip" id="fs13">Path.Combine([&lt;System.ParamArray&gt;] paths: string []) : string<br />Path.Combine(path1: string, path2: string) : string<br />Path.Combine(path1: string, path2: string, path3: string) : string<br />Path.Combine(path1: string, path2: string, path3: string, path4: string) : string</div>
<div class="tip" id="fs14">val script : string</div>
<div class="tip" id="fs15">type Literate =<br />&#160;&#160;static member private DefaultArguments : input:string * templateFile:string * output:string option * fsharpCompiler:Assembly option * prefix:string option * compilerOptions:string option * lineNumbers:bool option * references:bool option * replacements:(string * string) list option -&gt; string * ProcessingContext<br />&#160;&#160;static member ProcessDirectory : inputDirectory:string * templateFile:string * ?outputDirectory:string * ?fsharpCompiler:Assembly * ?prefix:string * ?compilerOptions:string * ?lineNumbers:bool * ?references:bool * ?replacements:(string * string) list -&gt; unit<br />&#160;&#160;static member ProcessMarkdown : input:string * templateFile:string * ?output:string * ?fsharpCompiler:Assembly * ?prefix:string * ?compilerOptions:string * ?lineNumbers:bool * ?references:bool * ?replacements:(string * string) list -&gt; unit<br />&#160;&#160;static member ProcessScriptFile : input:string * templateFile:string * ?output:string * ?fsharpCompiler:Assembly * ?prefix:string * ?compilerOptions:string * ?lineNumbers:bool * ?references:bool * ?replacements:(string * string) list -&gt; unit</div>
<div class="tip" id="fs16">static member Literate.ProcessScriptFile : input:string * templateFile:string * ?output:string * ?fsharpCompiler:System.Reflection.Assembly * ?prefix:string * ?compilerOptions:string * ?lineNumbers:bool * ?references:bool * ?replacements:(string * string) list -&gt; unit<br /><em><br /><br />&#160;Process F# Script file</em></div>
<div class="tip" id="fs17">val doc : string</div>
<div class="tip" id="fs18">static member Literate.ProcessMarkdown : input:string * templateFile:string * ?output:string * ?fsharpCompiler:System.Reflection.Assembly * ?prefix:string * ?compilerOptions:string * ?lineNumbers:bool * ?references:bool * ?replacements:(string * string) list -&gt; unit<br /><em><br /><br />&#160;Process Markdown document</em></div>
<div class="tip" id="fs19">val projInfo : (string * string) list</div>
<div class="tip" id="fs20">static member Literate.ProcessDirectory : inputDirectory:string * templateFile:string * ?outputDirectory:string * ?fsharpCompiler:System.Reflection.Assembly * ?prefix:string * ?compilerOptions:string * ?lineNumbers:bool * ?references:bool * ?replacements:(string * string) list -&gt; unit<br /><em><br /><br />&#160;Process directory containing a mix of Markdown documents and F# Script files</em></div>
