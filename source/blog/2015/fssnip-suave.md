Creating web sites with Suave: How to contribute to F# Snippets
===============================================================

 - date: 2015-09-15T23:26:01.8511959+01:00
 - description: The core of many web sites and web APIs is very simple. Given an HTTP request, produce a HTTP response. Sounds pretty simple, so why are there so many evil frameworks that make simple web programming difficult? In this blog post, I'll write about Suave -a nice composable library for web programming with F#. The blog post also shows a few interesting samples from the new version of F# Snippets and you are welcome to contribute!
 - layout: article
 - image: http://tomasp.net/blog/2015/fssnip-suave/logo.png
 - tags: f#,web
 - title: Creating web sites with Suave: How to contribute to F# Snippets
 - url: 2015/fssnip-suave
 - rawbody: true

--------------------------------------------------------------------------------
<img src="http://tomasp.net/blog/2015/fssnip-suave/logo.png" style="width:130px;float:right;margin-left:10px" />
<p>The core of many web sites and web APIs is very simple. Given an HTTP request,
produce a HTTP response. In F#, we can represent this as a function with type
<code>Request -&gt; Response</code>. To make our server scalable, we should make the function
<em>asynchronous</em> to avoid unnecessary blocking of threads. In F#, this can be
captured as <code>Request -&gt; Async&lt;Response&gt;</code>. Sounds pretty simple, right? So why
are there so many <a href="http://tomasp.net/blog/2015/library-frameworks/">evil frameworks</a>
that make simple web programming difficult?</p>
<p>Fortunately, there is a nice F# library called <a href="http://suave.io">Suave.io</a> that
is based exactly on the above idea:</p>
<blockquote>
<p>Suave is a simple web development F# library providing a lightweight web server
and a set of combinators to manipulate route flow and task composition.</p>
</blockquote>
<p>I recently decided to start a new version of the <a href="http://www.fssnip.net">F# Snippets</a>
web site and I wanted to keep the implementation functional, simple,
cross-platform and easy to contrbute to. I wrote a <a href="https://github.com/tpetricek/FsSnip.Website/">first prototype of the
implementation</a> using Suave and
already received a few contributions via pull requests! In this blog post, I'll
share a few interesting aspects of the implementation and I'll give you some
good pointers where you can learn more about Suave. <em>There is no excuse for not
contributing to F# Snippets v2 after reading this blog post</em>!</p>


--------------------------------------------------------------------------------
<h1><span class="hm">Creating web sites with Suave</span><span class="hs"> How to contribute to F# Snippets</span></h1>
<img src="http://tomasp.net/blog/2015/fssnip-suave/logo.png" style="width:130px;float:right;margin-left:10px" />
<p>The core of many web sites and web APIs is very simple. Given an HTTP request,
produce a HTTP response. In F#, we can represent this as a function with type
<code>Request -&gt; Response</code>. To make our server scalable, we should make the function
<em>asynchronous</em> to avoid unnecessary blocking of threads. In F#, this can be
captured as <code>Request -&gt; Async&lt;Response&gt;</code>. Sounds pretty simple, right? So why
are there so many <a href="http://tomasp.net/blog/2015/library-frameworks/">evil frameworks</a>
that make simple web programming difficult?</p>
<p>Fortunately, there is a nice F# library called <a href="http://suave.io">Suave.io</a> that
is based exactly on the above idea:</p>
<blockquote>
<p>Suave is a simple web development F# library providing a lightweight web server
and a set of combinators to manipulate route flow and task composition.</p>
</blockquote>
<p>I recently decided to start a new version of the <a href="http://www.fssnip.net">F# Snippets</a>
web site and I wanted to keep the implementation functional, simple,
cross-platform and easy to contrbute to. I wrote a <a href="https://github.com/tpetricek/FsSnip.Website/">first prototype of the
implementation</a> using Suave and
already received a few contributions via pull requests! In this blog post, I'll
share a few interesting aspects of the implementation and I'll give you some
good pointers where you can learn more about Suave. <em>There is no excuse for not
contributing to F# Snippets v2 after reading this blog post</em>!</p>
<h2>Getting started with Suave</h2>
<p>I recently did a couple of talks about Suave at user groups and conferences and
many of them have been recorded. There are also a couple of nice examples online
and some good documentation on the official web site. So if you want to learn
more about Suave, here are some links for you:</p>
<ul>
<li>
<p><strong>Channel 9 Interview.</strong> <a href="https://twitter.com/sethjuarez">Seth Juarez</a> did
an interview with me when I was in Redmond and I did a quick 20 minute demo
showing how to <a href="https://channel9.msdn.com/Blogs/Seth-Juarez/Deploying-an-F-Web-Application-with-Suave">Deploy an F# Web Application with Suave to
Azure</a>.
This is the last part of a mini series, so you might also want to check out
<a href="http://channel9.msdn.com/Blogs/Seth-Juarez/Making-the-Case-for-using-F-with-Tomas-Petricek">Making the Case for using F#</a>
if you are new to F#, <a href="http://channel9.msdn.com/Blogs/Seth-Juarez/Domain-Modeling-in-F-with-Tomas-Petricek">Domain Modeling in F#</a>
and <a href="http://channel9.msdn.com/Blogs/Seth-Juarez/Type-Providers-in-F-with-Tomas-Petricek">Type Providers in F#</a>.</p>
</li>
<li>
<p><strong>NDC Oslo Talk.</strong> Next, I talked about Suave at NDC in Oslo. The talk shows
two demo application - a web portal showing weather and news and a simple chat
written using agents. The talk
<a href="https://vimeo.com/131641270">End-to-end Functional Web Development</a> has been
recorded and you can also get <a href="https://github.com/tpetricek/Talks/tree/master/2015/end-to-end-web/ndc/code-done">the full source code</a>.
and <a href="http://tpetricek.github.io/Talks/2015/end-to-end-web/ndc/">slides</a>.
It is also worth noting that I'm using the <a href="https://github.com/fsprojects/atom-fsharp/">awesome F# Atom plugin</a>
in the talk, together with some custom <a href="http://fsharp.github.io/FAKE/">FAKE build scripts</a>
to get a nice live reloading when developing the web sites.</p>
</li>
<li>
<p><strong>Community for F# Talk.</strong> <a href="https://twitter.com/henrikfeldt">Henrik Feldt</a> who is one of
the Suave contributors did a nice talk <a href="https://www.youtube.com/watch?v=ujxwW6fFXOc">Suave from Scratch</a>
on the Community for F# channel. This shows many more Suave features and so it is a
great follow-up to the above. Also, Henrik is showing Suave <a href="http://fsharp.org/use/mac/">on Mac using Xamarin Studio</a>,
so you can see that it truly is cross-platform.</p>
</li>
<li>
<p><strong>Web Site and Dojo.</strong> For more information, there is a bunch of examples and
documentation on the <a href="http://suave.io/">official Suave web site</a>. This includes various
ways of deploying Suave applications too. If you then want to get some hands-on
experience, try completing <a href="https://github.com/tpetricek/Dojo-Suave-FsHome">the simple Suave Dojo that I put together!</a></p>
</li>
</ul>
<img src="video.png" style="width:75%;margin-left:12%;margin-bottom:20px" title="It is big. Really big. You just won't believe how vastly, hugely, mind-bogglingly big it is. I mean, you may think it's a long way down the road to the chemist, but that's just peanuts to space." />
<h2>Introducing F# Snippets v2</h2>
<p>As already mentioned, I started using Suave for the new version of the F# Snippets web
site. The web site is basically a pastebin for F# code snippets. The nice thing is that
it uses <a href="http://tpetricek.github.io/FSharp.Formatting/">F# Formatting</a> for formatting the
code snippets and generating tool tips. I never released the source code for the old
version, because it was simoply too ugly. The new version fixes this!</p>
<ul>
<li>
The <a href="https://github.com/tpetricek/FsSnip.Website/">source code is on GitHub</a> - 
to run it locally, you'll need to download sample data as discussed in the README.
</li>
<li>
The <a href="http://fssnip.azurewebsites.net/">prototype runs on Azure</a> - this is
automatically deployed from the <code>master</code> branch in the GitHub project and it
runs as Azure Website.
</li>
<li>
And <a href="https://github.com/tpetricek/FsSnip.Website/labels/status-priority">here is a list of remaining issues before it can replace the old 
version</a> -
the project is quite simple, so this is a great place where you can contribute!
</li>
</ul>
<p>The previous version of F# Snippets stored all data in an SQL database. When creating the new
one, I was wondering what is the best option given the size of the web site. It turns out
that the meta-data about all the snippets is small enough to fit in memory (about 1MB
in JSON format) and so the new version is a lot simpler.</p>
<p>It keeps the meta-data in memory. The formatted snippets are stored in local file system
(when testing things locally) or in Azure blob storage (when running on Azure) - though
you can also use Azure storage during development. When the meta-data change, it is also
saved to a JSON file in the blob storage (so that it can be reloaded if the application
is shut down).</p>
<p>You can find more details in the <a href="https://github.com/tpetricek/FsSnip.Website/#project-architecture--structure">project architecture section</a>
of the project README document.</p>
<h2>Interesting Suave snippets</h2>
<p>There is a number of things that make Suave really nice to use. As you can have a look
at the materials above to learn everything about it, I want to give you just a few
examples based on my experience with F# Snippets.</p>
<p>The first nice thing about Suave is that it is a library rather than a framework. This
means that you are in control of starting and running the server. This makes it easy
to deploy it to Azure, Heroku or anywhere else. In F# Snippets, we have one entry-point
in the <code>app.fsx</code> file. This composes the server from individual components.</p>
<h3>Composing server from web parts</h3>
<p>The <a href="https://github.com/tpetricek/FsSnip.Website/blob/f857d7b84ba5603708db036a509bd3ca9141f0ca/app.fsx#L70">following code snippet</a>
shows how the server is composed. As you can see, we have functionality for showing
the home page, displaying snippets, inserting new snippets, listing snippets and the
RSS feed:</p>
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
<span class="l">16: </span>
<span class="l">17: </span>
<span class="l">18: </span>
<span class="l">19: </span>
<span class="l">20: </span>
<span class="l">21: </span>
<span class="l">22: </span>
<span class="l">23: </span>
<span class="l">24: </span>
<span class="l">25: </span>
<span class="l">26: </span>
<span class="l">27: </span>
<span class="l">28: </span>
<span class="l">29: </span>
<span class="l">30: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs9', 16)" onmouseover="showTip(event, 'fs9', 16)" class="f">app</span> <span class="o">=</span> 
  <span onmouseout="hideTip(event, 'fs10', 17)" onmouseover="showTip(event, 'fs10', 17)" class="f">choose</span> 
    [ <span class="c">// When accessing &#39;/&#39; we display the homepage</span>
      <span onmouseout="hideTip(event, 'fs11', 18)" onmouseover="showTip(event, 'fs11', 18)" class="f">path</span> <span class="s">&quot;/&quot;</span> <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span class="i">Home</span><span class="o">.</span><span class="i">showHome</span> 
      
      <span class="c">// Display snippet (latest, specific version and raw source)</span>
      <span class="i">pathWithId</span> <span class="s">&quot;/%s&quot;</span> (<span class="k">fun</span> <span onmouseout="hideTip(event, 'fs12', 19)" onmouseover="showTip(event, 'fs12', 19)" class="i">id</span> <span class="k">-&gt;</span> <span class="i">Snippet</span><span class="o">.</span><span class="i">showSnippet</span> <span onmouseout="hideTip(event, 'fs12', 20)" onmouseover="showTip(event, 'fs12', 20)" class="i">id</span> <span class="i">Latest</span>) 
      <span class="i">pathWithId</span> <span class="s">&quot;/raw/%s&quot;</span> (<span class="k">fun</span> <span onmouseout="hideTip(event, 'fs12', 21)" onmouseover="showTip(event, 'fs12', 21)" class="i">id</span> <span class="k">-&gt;</span> <span class="i">Snippet</span><span class="o">.</span><span class="i">showRawSnippet</span> <span onmouseout="hideTip(event, 'fs12', 22)" onmouseover="showTip(event, 'fs12', 22)" class="i">id</span> <span class="i">Latest</span>) 
      <span onmouseout="hideTip(event, 'fs13', 23)" onmouseover="showTip(event, 'fs13', 23)" class="f">pathScan</span> <span class="s">&quot;/</span><span class="pf">%s</span><span class="s">/</span><span class="pf">%d</span><span class="s">&quot;</span> 
        (<span class="k">fun</span> (<span onmouseout="hideTip(event, 'fs14', 24)" onmouseover="showTip(event, 'fs14', 24)" class="i">id</span>, <span onmouseout="hideTip(event, 'fs15', 25)" onmouseover="showTip(event, 'fs15', 25)" class="i">r</span>) <span class="k">-&gt;</span> <span class="i">Snippet</span><span class="o">.</span><span class="i">showSnippet</span> <span onmouseout="hideTip(event, 'fs14', 26)" onmouseover="showTip(event, 'fs14', 26)" class="i">id</span> (<span class="i">Revision</span> <span onmouseout="hideTip(event, 'fs15', 27)" onmouseover="showTip(event, 'fs15', 27)" class="i">r</span>)) 
      <span onmouseout="hideTip(event, 'fs13', 28)" onmouseover="showTip(event, 'fs13', 28)" class="f">pathScan</span> <span class="s">&quot;/raw/</span><span class="pf">%s</span><span class="s">/</span><span class="pf">%d</span><span class="s">&quot;</span>  
        (<span class="k">fun</span> (<span onmouseout="hideTip(event, 'fs14', 29)" onmouseover="showTip(event, 'fs14', 29)" class="i">id</span>, <span onmouseout="hideTip(event, 'fs15', 30)" onmouseover="showTip(event, 'fs15', 30)" class="i">r</span>) <span class="k">-&gt;</span> <span class="i">Snippet</span><span class="o">.</span><span class="i">showRawSnippet</span> <span onmouseout="hideTip(event, 'fs14', 31)" onmouseover="showTip(event, 'fs14', 31)" class="i">id</span> (<span class="i">Revision</span> <span onmouseout="hideTip(event, 'fs15', 32)" onmouseover="showTip(event, 'fs15', 32)" class="i">r</span>)) 
      
      <span class="c">// Insert page, with simple REST API to check snippet for errors</span>
      <span onmouseout="hideTip(event, 'fs11', 33)" onmouseover="showTip(event, 'fs11', 33)" class="f">path</span> <span class="s">&quot;/pages/insert&quot;</span> <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span class="i">Insert</span><span class="o">.</span><span class="i">insertSnippet</span> 
      <span onmouseout="hideTip(event, 'fs11', 34)" onmouseover="showTip(event, 'fs11', 34)" class="f">path</span> <span class="s">&quot;/pages/insert/check&quot;</span> <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span class="i">Insert</span><span class="o">.</span><span class="i">checkSnippet</span> 
      
      <span class="c">// Listing of snippets by author and by tag</span>
      <span onmouseout="hideTip(event, 'fs11', 35)" onmouseover="showTip(event, 'fs11', 35)" class="f">path</span> <span class="s">&quot;/authors/&quot;</span> <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span class="i">Author</span><span class="o">.</span><span class="i">showAll</span> 
      <span onmouseout="hideTip(event, 'fs13', 36)" onmouseover="showTip(event, 'fs13', 36)" class="f">pathScan</span> <span class="s">&quot;/authors/</span><span class="pf">%s</span><span class="s">&quot;</span> <span class="i">Author</span><span class="o">.</span><span class="i">showSnippets</span> 
      <span onmouseout="hideTip(event, 'fs11', 37)" onmouseover="showTip(event, 'fs11', 37)" class="f">path</span> <span class="s">&quot;/tags/&quot;</span> <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span class="i">Tag</span><span class="o">.</span><span class="i">showAll</span> 
      <span onmouseout="hideTip(event, 'fs13', 38)" onmouseover="showTip(event, 'fs13', 38)" class="f">pathScan</span> <span class="s">&quot;/tags/</span><span class="pf">%s</span><span class="s">&quot;</span> <span class="i">Tag</span><span class="o">.</span><span class="i">showSnippets</span> 
      
      <span class="c">// Display RSS feed (allowing number of different path formats)</span>
      ( <span onmouseout="hideTip(event, 'fs11', 39)" onmouseover="showTip(event, 'fs11', 39)" class="f">path</span> <span class="s">&quot;/rss/&quot;</span> <span class="o">&lt;|&gt;</span> <span onmouseout="hideTip(event, 'fs11', 40)" onmouseover="showTip(event, 'fs11', 40)" class="f">path</span> <span class="s">&quot;/rss&quot;</span> <span class="o">&lt;|&gt;</span> 
        <span onmouseout="hideTip(event, 'fs11', 41)" onmouseover="showTip(event, 'fs11', 41)" class="f">path</span> <span class="s">&quot;/pages/Rss&quot;</span> <span class="o">&lt;|&gt;</span> <span onmouseout="hideTip(event, 'fs11', 42)" onmouseover="showTip(event, 'fs11', 42)" class="f">path</span> <span class="s">&quot;/pages/Rss/&quot;</span> ) <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span class="i">Rss</span><span class="o">.</span><span class="i">getRss</span> 
      
      <span class="c">// Otherwise, try to process the request as a static file</span>
      <span class="c">// (this handles all the CSS and JS files as well as images)</span>
      <span class="i">browseStaticFiles</span> ] 
</code></pre></td>
</tr>
</table>
<p>The <code>choose</code> combinator takes a list of <em>web parts</em> and composes them. A Suave web
part is essentially one of those functions from the introduction - web parts can handle
requests and produce response. Here, we are building a single web part that goes through
the web parts in the list and uses the first one that can handle an incoming request.
The <code>path</code> combinator is used to restrict what requests a web part handles - so for
example <code>path "/" &gt;&gt;= Home.showHome</code> means that we should display the home page if the
request is for the path <code>/</code>. A very nice function is <code>pathScan</code> - it takes an F#
<em>format string</em> and builds a web part that recognizes requests to URL with the specified
pattern. We can, for example, say <code>pathScan "/raw/%s/%d"</code> to detect URLs such as
<code>/raw/cJ/5</code>.</p>
<h3>Displaying snippets with DotLiquid</h3>
<p>The Suave library does not force you to use any specific templating engine and I actually
used Suave for some time with just string concatenation or <code>str.Replace</code>. But if you want
to use some templating library, it is really easy to add support for it. To see just how
easy, look at my <a href="https://github.com/SuaveIO/suave/pull/267">pull request adding support for DotLiquid</a>.
We're using <a href="http://dotliquidmarkup.org/">DotLiquid</a> in F# snippets, so here is how the
code looks.</p>
<p>The <a href="https://github.com/tpetricek/FsSnip.Website/blob/f857d7b84ba5603708db036a509bd3ca9141f0ca/code/pages/snippet.fs#L16">code sample below</a>
shows how we handle request to display a snippet. We get the snippet ID, get information about
it from the meta-data and read the file from storage. If everything succeeds, we create
a record <code>FormattedSnippet</code> and pass it to the template loaded from <code>snippet.html</code>:</p>
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
<span class="l">16: </span>
<span class="l">17: </span>
<span class="l">18: </span>
<span class="l">19: </span>
<span class="l">20: </span>
<span class="l">21: </span>
<span class="l">22: </span>
<span class="l">23: </span>
<span class="l">24: </span>
<span class="l">25: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">type</span> <span onmouseout="hideTip(event, 'fs16', 43)" onmouseover="showTip(event, 'fs16', 43)" class="t">FormattedSnippet</span> <span class="o">=</span>
  { <span onmouseout="hideTip(event, 'fs17', 44)" onmouseover="showTip(event, 'fs17', 44)" class="i">Html</span> <span class="o">:</span> <span onmouseout="hideTip(event, 'fs18', 45)" onmouseover="showTip(event, 'fs18', 45)" class="t">string</span>
    <span onmouseout="hideTip(event, 'fs19', 46)" onmouseover="showTip(event, 'fs19', 46)" class="i">Details</span> <span class="o">:</span> <span onmouseout="hideTip(event, 'fs20', 47)" onmouseover="showTip(event, 'fs20', 47)" class="i">Data</span><span class="o">.</span><span class="i">Snippet</span>
    <span onmouseout="hideTip(event, 'fs21', 48)" onmouseover="showTip(event, 'fs21', 48)" class="i">Revision</span> <span class="o">:</span> <span onmouseout="hideTip(event, 'fs22', 49)" onmouseover="showTip(event, 'fs22', 49)" class="t">int</span> }

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs23', 50)" onmouseover="showTip(event, 'fs23', 50)" class="f">showSnippet</span> <span onmouseout="hideTip(event, 'fs24', 51)" onmouseover="showTip(event, 'fs24', 51)" class="i">id</span> <span onmouseout="hideTip(event, 'fs25', 52)" onmouseover="showTip(event, 'fs25', 52)" class="i">r</span> <span class="o">=</span>
  <span class="k">let</span> <span onmouseout="hideTip(event, 'fs26', 53)" onmouseover="showTip(event, 'fs26', 53)" class="i">id&#39;</span> <span class="o">=</span> <span class="i">demangleId</span> <span onmouseout="hideTip(event, 'fs24', 54)" onmouseover="showTip(event, 'fs24', 54)" class="i">id</span>
  <span class="k">let</span> <span onmouseout="hideTip(event, 'fs27', 55)" onmouseover="showTip(event, 'fs27', 55)" class="i">snippetOpt</span> <span class="o">=</span> 
    <span class="i">publicSnippets</span>
    <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs28', 56)" onmouseover="showTip(event, 'fs28', 56)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs29', 57)" onmouseover="showTip(event, 'fs29', 57)" class="f">tryFind</span> (<span class="k">fun</span> <span onmouseout="hideTip(event, 'fs30', 58)" onmouseover="showTip(event, 'fs30', 58)" class="i">s</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs30', 59)" onmouseover="showTip(event, 'fs30', 59)" class="i">s</span><span class="o">.</span><span class="i">ID</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs26', 60)" onmouseover="showTip(event, 'fs26', 60)" class="i">id&#39;</span>) 
  <span class="k">match</span> <span onmouseout="hideTip(event, 'fs27', 61)" onmouseover="showTip(event, 'fs27', 61)" class="i">snippetOpt</span> <span class="k">with</span>
  | <span onmouseout="hideTip(event, 'fs31', 62)" onmouseover="showTip(event, 'fs31', 62)" class="p">Some</span> <span onmouseout="hideTip(event, 'fs32', 63)" onmouseover="showTip(event, 'fs32', 63)" class="i">snippetInfo</span> <span class="k">-&gt;</span> 
      <span class="k">match</span> <span onmouseout="hideTip(event, 'fs20', 64)" onmouseover="showTip(event, 'fs20', 64)" class="i">Data</span><span class="o">.</span><span class="i">loadSnippet</span> <span onmouseout="hideTip(event, 'fs24', 65)" onmouseover="showTip(event, 'fs24', 65)" class="i">id</span> <span onmouseout="hideTip(event, 'fs25', 66)" onmouseover="showTip(event, 'fs25', 66)" class="i">r</span> <span class="k">with</span>
      | <span onmouseout="hideTip(event, 'fs31', 67)" onmouseover="showTip(event, 'fs31', 67)" class="p">Some</span> <span onmouseout="hideTip(event, 'fs33', 68)" onmouseover="showTip(event, 'fs33', 68)" class="i">snippet</span> <span class="k">-&gt;</span>
          { <span class="i">Html</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs33', 69)" onmouseover="showTip(event, 'fs33', 69)" class="i">snippet</span>
            <span class="i">Details</span> <span class="o">=</span>
              <span onmouseout="hideTip(event, 'fs20', 70)" onmouseover="showTip(event, 'fs20', 70)" class="i">Data</span><span class="o">.</span><span class="i">snippets</span> 
              <span class="o">|&gt;</span> <span onmouseout="hideTip(event, 'fs28', 71)" onmouseover="showTip(event, 'fs28', 71)" class="t">Seq</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs34', 72)" onmouseover="showTip(event, 'fs34', 72)" class="f">find</span> (<span class="k">fun</span> <span onmouseout="hideTip(event, 'fs30', 73)" onmouseover="showTip(event, 'fs30', 73)" class="i">s</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs30', 74)" onmouseover="showTip(event, 'fs30', 74)" class="i">s</span><span class="o">.</span><span class="i">ID</span> <span class="o">=</span> <span class="i">demangleId</span> <span onmouseout="hideTip(event, 'fs24', 75)" onmouseover="showTip(event, 'fs24', 75)" class="i">id</span>)
            <span class="i">Revision</span> <span class="o">=</span>
              <span class="k">match</span> <span onmouseout="hideTip(event, 'fs25', 76)" onmouseover="showTip(event, 'fs25', 76)" class="i">r</span> <span class="k">with</span> 
              | <span onmouseout="hideTip(event, 'fs35', 77)" onmouseover="showTip(event, 'fs35', 77)" class="i">Latest</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs32', 78)" onmouseover="showTip(event, 'fs32', 78)" class="i">snippetInfo</span><span class="o">.</span><span class="i">Versions</span> <span class="o">-</span> <span class="n">1</span> 
              | <span class="i">Revision</span> <span onmouseout="hideTip(event, 'fs25', 79)" onmouseover="showTip(event, 'fs25', 79)" class="i">r</span> <span class="k">-&gt;</span> <span onmouseout="hideTip(event, 'fs25', 80)" onmouseover="showTip(event, 'fs25', 80)" class="i">r</span> }
          <span class="o">|&gt;</span> <span class="i">DotLiquid</span><span class="o">.</span><span class="i">page</span><span class="o">&lt;</span><span onmouseout="hideTip(event, 'fs16', 81)" onmouseover="showTip(event, 'fs16', 81)" class="i">FormattedSnippet</span><span class="o">&gt;</span> <span class="s">&quot;snippet.html&quot;</span>
      | <span onmouseout="hideTip(event, 'fs36', 82)" onmouseover="showTip(event, 'fs36', 82)" class="p">None</span> <span class="k">-&gt;</span> <span class="i">invalidSnippetId</span> <span onmouseout="hideTip(event, 'fs24', 83)" onmouseover="showTip(event, 'fs24', 83)" class="i">id</span>
  | <span onmouseout="hideTip(event, 'fs36', 84)" onmouseover="showTip(event, 'fs36', 84)" class="p">None</span> <span class="k">-&gt;</span> <span class="i">invalidSnippetId</span> <span onmouseout="hideTip(event, 'fs24', 85)" onmouseover="showTip(event, 'fs24', 85)" class="i">id</span>
</code></pre></td>
</tr>
</table>
<p>You can find the <a href="https://github.com/tpetricek/FsSnip.Website/blob/f857d7b84ba5603708db036a509bd3ca9141f0ca/templates/snippet.html">full template on GitHub</a>.
The value of the record is exposed as <code>model</code> and we can access its properties in the
template. For example, the heading is generated by <code>&lt;h1&gt;{{ model.Details.Title }}&lt;/h1&gt;</code>.</p>
<h3>Checking F# code during insertion</h3>
<p>The new F# Snippets web site reports all the errors in your F# code on the fly when
you are inserting the snippet. Go to the <a href="http://fssnip.azurewebsites.net/pages/insert">insert snippet page</a>,
type some invalid F#, wait a second and you should see the compiler errors and warnings!</p>
<p>The implementation of this uses a simple JavaScript with timer and it calls the <code>/insert/check</code>
API end-point implemented by the server. This then returns a simple JSON with a list of
the errors and warning.</p>
<p>This is another elegant piece of F# code that uses Suave composable web parts and
the JSON type provider from <a href="http://fsharp.github.io/FSharp.Data/">F# Data</a> to generate
the JSON response. Check out <a href="https://github.com/tpetricek/FsSnip.Website/blob/f857d7b84ba5603708db036a509bd3ca9141f0ca/code/pages/insert.fs#L72">the following snippet</a>:</p>
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
<span class="l">16: </span>
<span class="l">17: </span>
<span class="l">18: </span>
<span class="l">19: </span>
<span class="l">20: </span>
<span class="l">21: </span>
<span class="l">22: </span>
<span class="l">23: </span>
<span class="l">24: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">open</span> <span onmouseout="hideTip(event, 'fs37', 86)" onmouseover="showTip(event, 'fs37', 86)" class="i">FSharp</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs38', 87)" onmouseover="showTip(event, 'fs38', 87)" class="i">Data</span>
<span class="k">type</span> <span onmouseout="hideTip(event, 'fs39', 88)" onmouseover="showTip(event, 'fs39', 88)" class="t">Errors</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs40', 89)" onmouseover="showTip(event, 'fs40', 89)" class="t">JsonProvider</span><span class="o">&lt;</span><span class="s">&quot;&quot;&quot;</span>
<span class="s">  [ {&quot;location&quot;:[1,1,10,10], &quot;error&quot;:true, &quot;message&quot;:&quot;sth&quot;} ]&quot;&quot;&quot;</span><span class="o">&gt;</span>

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs41', 90)" onmouseover="showTip(event, 'fs41', 90)" class="f">noCache</span> <span class="o">=</span> 
  <span onmouseout="hideTip(event, 'fs42', 91)" onmouseover="showTip(event, 'fs42', 91)" class="f">setHeader</span> <span class="s">&quot;Cache-Control&quot;</span> <span class="s">&quot;no-cache, no-store, must-revalidate&quot;</span>
  <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span onmouseout="hideTip(event, 'fs42', 92)" onmouseover="showTip(event, 'fs42', 92)" class="f">setHeader</span> <span class="s">&quot;Pragma&quot;</span> <span class="s">&quot;no-cache&quot;</span>
  <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span onmouseout="hideTip(event, 'fs42', 93)" onmouseover="showTip(event, 'fs42', 93)" class="f">setHeader</span> <span class="s">&quot;Expires&quot;</span> <span class="s">&quot;0&quot;</span>
  <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span onmouseout="hideTip(event, 'fs43', 94)" onmouseover="showTip(event, 'fs43', 94)" class="f">setMimeType</span> <span class="s">&quot;application/json&quot;</span>

<span class="k">let</span> <span onmouseout="hideTip(event, 'fs44', 95)" onmouseover="showTip(event, 'fs44', 95)" class="f">checkSnippet</span> <span onmouseout="hideTip(event, 'fs45', 96)" onmouseover="showTip(event, 'fs45', 96)" class="i">ctx</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs46', 97)" onmouseover="showTip(event, 'fs46', 97)" class="i">async</span> {
  <span class="k">use</span> <span onmouseout="hideTip(event, 'fs47', 98)" onmouseover="showTip(event, 'fs47', 98)" class="i">sr</span> <span class="o">=</span> <span class="k">new</span> <span onmouseout="hideTip(event, 'fs48', 99)" onmouseover="showTip(event, 'fs48', 99)" class="t">StreamReader</span>(<span class="k">new</span> <span onmouseout="hideTip(event, 'fs49', 100)" onmouseover="showTip(event, 'fs49', 100)" class="t">MemoryStream</span>(<span onmouseout="hideTip(event, 'fs45', 101)" onmouseover="showTip(event, 'fs45', 101)" class="i">ctx</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs50', 102)" onmouseover="showTip(event, 'fs50', 102)" class="i">request</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs51', 103)" onmouseover="showTip(event, 'fs51', 103)" class="i">rawForm</span>))
  <span class="k">let</span> <span onmouseout="hideTip(event, 'fs52', 104)" onmouseover="showTip(event, 'fs52', 104)" class="i">request</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs47', 105)" onmouseover="showTip(event, 'fs47', 105)" class="i">sr</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs53', 106)" onmouseover="showTip(event, 'fs53', 106)" class="f">ReadToEnd</span>()
  <span class="k">let</span> <span onmouseout="hideTip(event, 'fs54', 107)" onmouseover="showTip(event, 'fs54', 107)" class="i">doc</span> <span class="o">=</span> 
    <span class="i">Literate</span><span class="o">.</span><span class="i">ParseScriptString</span>
      (<span onmouseout="hideTip(event, 'fs52', 108)" onmouseover="showTip(event, 'fs52', 108)" class="i">request</span>, <span class="s">&quot;/temp/Snippet.fsx&quot;</span>, <span class="i">formatAgent</span>)
  <span class="k">let</span> <span onmouseout="hideTip(event, 'fs55', 109)" onmouseover="showTip(event, 'fs55', 109)" class="i">json</span> <span class="o">=</span> 
    <span onmouseout="hideTip(event, 'fs56', 110)" onmouseover="showTip(event, 'fs56', 110)" class="t">JsonValue</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs57', 111)" onmouseover="showTip(event, 'fs57', 111)" class="p">Array</span>
      [| <span class="k">for</span> <span class="i">SourceError</span>((<span class="i">l1</span>,<span class="i">c1</span>),(<span class="i">l2</span>,<span class="i">c2</span>),<span class="i">kind</span>,<span class="i">msg</span>) <span class="k">in</span> <span onmouseout="hideTip(event, 'fs54', 112)" onmouseover="showTip(event, 'fs54', 112)" class="i">doc</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs39', 113)" onmouseover="showTip(event, 'fs39', 113)" class="i">Errors</span> <span class="k">-&gt;</span>
         <span onmouseout="hideTip(event, 'fs39', 114)" onmouseover="showTip(event, 'fs39', 114)" class="i">Errors</span><span class="o">.</span><span class="i">Root</span>
           ( [| <span class="i">l1</span>; <span class="i">c1</span>; <span class="i">l2</span>; <span class="i">c2</span> |], 
             (<span class="i">kind</span> <span class="o">=</span> <span class="i">ErrorKind</span><span class="o">.</span><span class="i">Error</span>), <span class="i">msg</span>)<span class="o">.</span><span onmouseout="hideTip(event, 'fs56', 115)" onmouseover="showTip(event, 'fs56', 115)" class="i">JsonValue</span> |]
             
  <span class="k">return!</span> <span onmouseout="hideTip(event, 'fs45', 116)" onmouseover="showTip(event, 'fs45', 116)" class="i">ctx</span> <span class="o">|&gt;</span> (<span onmouseout="hideTip(event, 'fs41', 117)" onmouseover="showTip(event, 'fs41', 117)" class="f">noCache</span> <span class="o">&gt;</span><span class="o">&gt;</span><span class="o">=</span> <span onmouseout="hideTip(event, 'fs58', 118)" onmouseover="showTip(event, 'fs58', 118)" class="t">Successful</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs59', 119)" onmouseover="showTip(event, 'fs59', 119)" class="f">OK</span>(<span onmouseout="hideTip(event, 'fs55', 120)" onmouseover="showTip(event, 'fs55', 120)" class="i">json</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs60', 121)" onmouseover="showTip(event, 'fs60', 121)" class="f">ToString</span>()) ) }
</code></pre></td>
</tr>
</table>
<p>There are a few nice things worth mentioning:</p>
<ul>
<li>
<p>The example shows an interesting use of the JSON type provider. We give it a sample
JSON (list with one error), but we're not using it to <em>read</em> data but instead to
<em>generate</em> response. As you can ee on line 20, we can then use the provided type
<code>Errors.Root</code> to easily build a JSON value representing the error or warning.</p>
</li>
<li>
<p>We need to disable all caching in the HTTP response. To do this, we use <em>composition
of web parts</em>. We define <code>noCache</code> which sets all the different HTTP headers
required for this (lines 6-9) and then we use it when producing the result
on line 24.</p>
</li>
</ul>
<p>The compositional nature of Suave means that you can really easily define reusable
components and structure your code in the way that works for you. For F# Snippets,
I wanted to make the project easy to contribute to, and so there is a fairly large
number of small independent files implementing the different components.</p>
<h2>Summary</h2>
<p>This blog post had two purposes. First, I wanted to share some of the resources
that you might find useful if you want to learn about web development with F# using
Suave. There are many more information available on the internet, including <a href="http://www.hanselman.com/blog/RunningSuaveioAndFWithFAKEInAzureWebAppsWithGitAndTheDeployButton.aspx">blog
post from Scott Hanselman</a> and
<a href="http://blog.geist.no/suave-io-introduction-and-example-part-1-intro/">a cool series by Claus Sørensen</a>,
so my list is just scratching the surface!</p>
<p>My second secret goal was to convince you to contribute to the <a href="https://github.com/tpetricek/FsSnip.Website/">new F# Snippets
project</a>. Writing the prototype was a lot of
fun and I think you'd have fun contributing too. There is also a very large number of features
that people asked about (commenting, search, clustering, suggesting tags, etc.), so
I think anyone will find something interesting. To start with, there are <a href="https://github.com/tpetricek/FsSnip.Website/labels/status-priority">a few high-priority
issues</a> that need
to be resolved before we can replace the old version.</p>


<div class="tip" id="fs1">namespace System</div>
<div class="tip" id="fs2">namespace System.IO</div>
<div class="tip" id="fs3">namespace Suave</div>
<div class="tip" id="fs4">module Web<br /><br />from Suave</div>
<div class="tip" id="fs5">module Http<br /><br />from Suave</div>
<div class="tip" id="fs6">module Files<br /><br />from Suave.Http</div>
<div class="tip" id="fs7">module Applicatives<br /><br />from Suave.Http</div>
<div class="tip" id="fs8">module Writers<br /><br />from Suave.Http</div>
<div class="tip" id="fs9">val app : Suave.Types.WebPart<br /><br />Full name: Fssnip-suave.app</div>
<div class="tip" id="fs10">val choose : options:Suave.Types.WebPart list -&gt; Suave.Types.WebPart<br /><br />Full name: Suave.Http.choose</div>
<div class="tip" id="fs11">val path : s:string -&gt; Suave.Types.WebPart<br /><br />Full name: Suave.Http.Applicatives.path</div>
<div class="tip" id="fs12">val id : x:&#39;T -&gt; &#39;T<br /><br />Full name: Microsoft.FSharp.Core.Operators.id</div>
<div class="tip" id="fs13">val pathScan : pf:PrintfFormat&lt;&#39;a,&#39;b,&#39;c,&#39;d,&#39;t&gt; -&gt; h:(&#39;t -&gt; Suave.Types.WebPart) -&gt; Suave.Types.WebPart<br /><br />Full name: Suave.Http.Applicatives.pathScan</div>
<div class="tip" id="fs14">val id : string</div>
<div class="tip" id="fs15">val r : int</div>
<div class="tip" id="fs16">type FormattedSnippet =<br />&#160;&#160;{Html: string;<br />&#160;&#160;&#160;Details: obj;<br />&#160;&#160;&#160;Revision: int;}<br /><br />Full name: Fssnip-suave.FormattedSnippet</div>
<div class="tip" id="fs17">FormattedSnippet.Html: string</div>
<div class="tip" id="fs18">Multiple items<br />val string : value:&#39;T -&gt; string<br /><br />Full name: Microsoft.FSharp.Core.Operators.string<br /><br />--------------------<br />type string = System.String<br /><br />Full name: Microsoft.FSharp.Core.string</div>
<div class="tip" id="fs19">FormattedSnippet.Details: obj</div>
<div class="tip" id="fs20">namespace Microsoft.FSharp.Data</div>
<div class="tip" id="fs21">FormattedSnippet.Revision: int</div>
<div class="tip" id="fs22">Multiple items<br />val int : value:&#39;T -&gt; int (requires member op_Explicit)<br /><br />Full name: Microsoft.FSharp.Core.Operators.int<br /><br />--------------------<br />type int = int32<br /><br />Full name: Microsoft.FSharp.Core.int<br /><br />--------------------<br />type int&lt;&#39;Measure&gt; = int<br /><br />Full name: Microsoft.FSharp.Core.int&lt;_&gt;</div>
<div class="tip" id="fs23">val showSnippet : id:&#39;a -&gt; r:&#39;b -&gt; &#39;c<br /><br />Full name: Fssnip-suave.showSnippet</div>
<div class="tip" id="fs24">val id : &#39;a</div>
<div class="tip" id="fs25">val r : &#39;b</div>
<div class="tip" id="fs26">val id&#39; : obj</div>
<div class="tip" id="fs27">val snippetOpt : obj option</div>
<div class="tip" id="fs28">module Seq<br /><br />from Microsoft.FSharp.Collections</div>
<div class="tip" id="fs29">val tryFind : predicate:(&#39;T -&gt; bool) -&gt; source:seq&lt;&#39;T&gt; -&gt; &#39;T option<br /><br />Full name: Microsoft.FSharp.Collections.Seq.tryFind</div>
<div class="tip" id="fs30">val s : obj</div>
<div class="tip" id="fs31">union case Option.Some: Value: &#39;T -&gt; Option&lt;&#39;T&gt;</div>
<div class="tip" id="fs32">val snippetInfo : obj</div>
<div class="tip" id="fs33">val snippet : string</div>
<div class="tip" id="fs34">val find : predicate:(&#39;T -&gt; bool) -&gt; source:seq&lt;&#39;T&gt; -&gt; &#39;T<br /><br />Full name: Microsoft.FSharp.Collections.Seq.find</div>
<div class="tip" id="fs35">val Latest : &#39;b</div>
<div class="tip" id="fs36">union case Option.None: Option&lt;&#39;T&gt;</div>
<div class="tip" id="fs37">Multiple items<br />namespace FSharp<br /><br />--------------------<br />namespace Microsoft.FSharp</div>
<div class="tip" id="fs38">Multiple items<br />namespace FSharp.Data<br /><br />--------------------<br />namespace Microsoft.FSharp.Data</div>
<div class="tip" id="fs39">type Errors = JsonProvider&lt;...&gt;<br /><br />Full name: Fssnip-suave.Errors</div>
<div class="tip" id="fs40">type JsonProvider<br /><br />Full name: FSharp.Data.JsonProvider<br /><em><br /><br />&lt;summary&gt;Typed representation of a JSON document&lt;/summary&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Sample&#39;&gt;Location of a JSON sample file or a string containing a sample JSON document&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;SampleList&#39;&gt;If true, sample should be a list of individual samples for the inference.&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;Culture&#39;&gt;The culture used for parsing numbers and dates.&lt;/param&gt;<br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&lt;param name=&#39;ResolutionFolder&#39;&gt;A directory that is used when resolving relative file references (at design time and in hosted execution)&lt;/param&gt;</em></div>
<div class="tip" id="fs41">val noCache : (Suave.Types.HttpContext -&gt; Async&lt;Suave.Types.HttpContext option&gt;)<br /><br />Full name: Fssnip-suave.noCache</div>
<div class="tip" id="fs42">val setHeader : key:string -&gt; value:string -&gt; Suave.Types.WebPart<br /><br />Full name: Suave.Http.Writers.setHeader</div>
<div class="tip" id="fs43">val setMimeType : mimeType:string -&gt; Suave.Types.WebPart<br /><br />Full name: Suave.Http.Writers.setMimeType</div>
<div class="tip" id="fs44">val checkSnippet : ctx:Suave.Types.HttpContext -&gt; Async&lt;Suave.Types.HttpContext option&gt;<br /><br />Full name: Fssnip-suave.checkSnippet</div>
<div class="tip" id="fs45">val ctx : Suave.Types.HttpContext</div>
<div class="tip" id="fs46">val async : AsyncBuilder<br /><br />Full name: Microsoft.FSharp.Core.ExtraTopLevelOperators.async</div>
<div class="tip" id="fs47">val sr : StreamReader</div>
<div class="tip" id="fs48">Multiple items<br />type StreamReader =<br />&#160;&#160;inherit TextReader<br />&#160;&#160;new : stream:Stream -&gt; StreamReader + 9 overloads<br />&#160;&#160;member BaseStream : Stream<br />&#160;&#160;member Close : unit -&gt; unit<br />&#160;&#160;member CurrentEncoding : Encoding<br />&#160;&#160;member DiscardBufferedData : unit -&gt; unit<br />&#160;&#160;member EndOfStream : bool<br />&#160;&#160;member Peek : unit -&gt; int<br />&#160;&#160;member Read : unit -&gt; int + 1 overload<br />&#160;&#160;member ReadLine : unit -&gt; string<br />&#160;&#160;member ReadToEnd : unit -&gt; string<br />&#160;&#160;...<br /><br />Full name: System.IO.StreamReader<br /><br />--------------------<br />StreamReader(stream: Stream) : unit<br />StreamReader(path: string) : unit<br />StreamReader(stream: Stream, detectEncodingFromByteOrderMarks: bool) : unit<br />StreamReader(stream: Stream, encoding: System.Text.Encoding) : unit<br />StreamReader(path: string, detectEncodingFromByteOrderMarks: bool) : unit<br />StreamReader(path: string, encoding: System.Text.Encoding) : unit<br />StreamReader(stream: Stream, encoding: System.Text.Encoding, detectEncodingFromByteOrderMarks: bool) : unit<br />StreamReader(path: string, encoding: System.Text.Encoding, detectEncodingFromByteOrderMarks: bool) : unit<br />StreamReader(stream: Stream, encoding: System.Text.Encoding, detectEncodingFromByteOrderMarks: bool, bufferSize: int) : unit<br />StreamReader(path: string, encoding: System.Text.Encoding, detectEncodingFromByteOrderMarks: bool, bufferSize: int) : unit</div>
<div class="tip" id="fs49">Multiple items<br />type MemoryStream =<br />&#160;&#160;inherit Stream<br />&#160;&#160;new : unit -&gt; MemoryStream + 6 overloads<br />&#160;&#160;member CanRead : bool<br />&#160;&#160;member CanSeek : bool<br />&#160;&#160;member CanWrite : bool<br />&#160;&#160;member Capacity : int with get, set<br />&#160;&#160;member Flush : unit -&gt; unit<br />&#160;&#160;member GetBuffer : unit -&gt; byte[]<br />&#160;&#160;member Length : int64<br />&#160;&#160;member Position : int64 with get, set<br />&#160;&#160;member Read : buffer:byte[] * offset:int * count:int -&gt; int<br />&#160;&#160;...<br /><br />Full name: System.IO.MemoryStream<br /><br />--------------------<br />MemoryStream() : unit<br />MemoryStream(capacity: int) : unit<br />MemoryStream(buffer: byte []) : unit<br />MemoryStream(buffer: byte [], writable: bool) : unit<br />MemoryStream(buffer: byte [], index: int, count: int) : unit<br />MemoryStream(buffer: byte [], index: int, count: int, writable: bool) : unit<br />MemoryStream(buffer: byte [], index: int, count: int, writable: bool, publiclyVisible: bool) : unit</div>
<div class="tip" id="fs50">Suave.Types.HttpContext.request: Suave.Types.HttpRequest</div>
<div class="tip" id="fs51">Suave.Types.HttpRequest.rawForm: byte []</div>
<div class="tip" id="fs52">val request : string</div>
<div class="tip" id="fs53">StreamReader.ReadToEnd() : string</div>
<div class="tip" id="fs54">val doc : obj</div>
<div class="tip" id="fs55">val json : JsonValue</div>
<div class="tip" id="fs56">type JsonValue =<br />&#160;&#160;| String of string<br />&#160;&#160;| Number of decimal<br />&#160;&#160;| Float of float<br />&#160;&#160;| Record of properties: (string * JsonValue) []<br />&#160;&#160;| Array of elements: JsonValue []<br />&#160;&#160;| Boolean of bool<br />&#160;&#160;| Null<br />&#160;&#160;member Request : uri:string * ?httpMethod:string * ?headers:seq&lt;string * string&gt; -&gt; HttpResponse<br />&#160;&#160;member RequestAsync : uri:string * ?httpMethod:string * ?headers:seq&lt;string * string&gt; -&gt; Async&lt;HttpResponse&gt;<br />&#160;&#160;override ToString : unit -&gt; string<br />&#160;&#160;member ToString : saveOptions:JsonSaveOptions -&gt; string<br />&#160;&#160;member WriteTo : w:TextWriter * saveOptions:JsonSaveOptions -&gt; unit<br />&#160;&#160;static member AsyncLoad : uri:string * ?cultureInfo:CultureInfo -&gt; Async&lt;JsonValue&gt;<br />&#160;&#160;static member private JsonStringEncodeTo : w:TextWriter -&gt; value:string -&gt; unit<br />&#160;&#160;static member Load : uri:string * ?cultureInfo:CultureInfo -&gt; JsonValue<br />&#160;&#160;static member Load : reader:TextReader * ?cultureInfo:CultureInfo -&gt; JsonValue<br />&#160;&#160;static member Load : stream:Stream * ?cultureInfo:CultureInfo -&gt; JsonValue<br />&#160;&#160;static member Parse : text:string * ?cultureInfo:CultureInfo -&gt; JsonValue<br />&#160;&#160;static member ParseMultiple : text:string * ?cultureInfo:CultureInfo -&gt; seq&lt;JsonValue&gt;<br />&#160;&#160;static member ParseSample : text:string * ?cultureInfo:CultureInfo -&gt; JsonValue<br /><br />Full name: FSharp.Data.JsonValue</div>
<div class="tip" id="fs57">union case JsonValue.Array: elements: JsonValue [] -&gt; JsonValue</div>
<div class="tip" id="fs58">module Successful<br /><br />from Suave.Http</div>
<div class="tip" id="fs59">val OK : a:string -&gt; Suave.Types.WebPart<br /><br />Full name: Suave.Http.Successful.OK</div>
<div class="tip" id="fs60">override JsonValue.ToString : unit -&gt; string<br />member JsonValue.ToString : saveOptions:JsonSaveOptions -&gt; string</div>
