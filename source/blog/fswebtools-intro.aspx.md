F# Web Tools: "Ajax" applications made simple
=============================================

 - date: 2007-07-13T04:32:29.0000000
 - description: This article introduces the F# Web Toolkit, which is an \"Ajax\" web framework that solves three major problems that many people have to face when developing modern web applications.
 - layout: article
 - tags: web,f#
 - title: F# Web Tools: \"Ajax\" applications made simple
 - url: fswebtools-intro.aspx

--------------------------------------------------------------------------------
 - standalone

<p>Traditional "Ajax" application consists of the server-side code and the client-side part written in JavaScript 
  (the more dynamicity you want, the larger JS files you have to write), which exchanges some data with the server-side
  code using XmlHttpRequest, typically in JSON format. I think this approach has 3 main problems, which we
  tried to solve in F# Web Toolkit. There are a few projects that try to solve some of them already - the most interesting 
  projects are Volta from Microsoft [<a href="#fswtintrolinks">1</a>], Links language [<a href="#fswtintrolinks">3</a>] 
  from the University of Edinburgh and Google Web Toolkit [<a href="#fswtintrolinks">2</a>], but none of the projects solve 
  all three problems at once. </p> 

<ol>
<li>Limited client-side environment</li>
<li>Discontinuity between server and client side</li>
<li>Components in web frameworks are only server-side</li>
</ol>
<p>The aim of the F# Web Toolkit is to solve all these three problems...</p>

--------------------------------------------------------------------------------


<p>I started thinking about working on "Ajax" framework quite a long time ago - the key thing
  I really wanted from the beginning was using the same language for writing both client and 
  server side code and the integration between these two sides, so you could write an event handler
  and specify if it should be executed on the client or on the server side. About a year ago I 
  visited Cambridge (thanks to the MVP program) and I had a chance to talk with 
  <a href="http://blogs.msdn.com/dsyme">Don Syme</a> [<a href="http://blogs.msdn.com/dsyme" target="_blank">^</a>].
  Don showed me a few things in F# and suggested using F# for this project, so when I was later selected
  to do an internship at MSR, this was one of the projects that I wanted to work on.</p>

<p>The original reason for using F# was its support for meta-programming ([<a href="#fswtintrolinks">8</a>], which
  makes it extremely easy to translate part of the page code-behind code to JavaScript. During my internship,
  the F# team was also working on a feature called <em>computational expressions</em>, which proved to be
  extremely useful for the F# Web Tools as well - I bet you'll hear a lot about this from Don soon, so I'll 
  describe only the aspects that are important for this project. Aside from these two key features that F# has,
  I also quite enjoyed programming in F# itself - I already used it for a few things during the last year, but
  I could finally work on a large project in F# (and discuss the solution with the real experts!) and I don't believe
  I would be able to finish the project of similar complexity during less than three months in any other language
  (but this is a different topic, which deserves separate blog post).
</p> 

<h2>What makes "Ajax" difficult?</h2>
<p>Traditional "Ajax" application consists of the server-side code and the client-side part written in JavaScript 
  (the more dynamicity you want, the larger JS files you have to write), which exchanges some data with the server-side
  code using XmlHttpRequest, typically in JSON format. I think this approach has 3 main problems, which we
  tried to solve in F# Web Tools. There are a few projects that try to solve some of them already - the most interesting 
  projects are Volta from Microsoft [<a href="#fswtintrolinks">1</a>], Links language [<a href="#fswtintrolinks">3</a>] 
  from the University of Edinburgh and Google Web Toolkit [<a href="#fswtintrolinks">2</a>], but none of the projects solve 
  all three problems at once.</p> 
   
<h3>1. Limited client-side environment</h3>
<p>First of the problems with "Ajax" style applications is that significant part of the application
  runs on the client-side (in a web browser). Currently majority of the web applications use 
  JavaScript to execute code in the browser, so in the F# Web Tools we wanted to use JavaScript
  as well, however in the future, when installation of Silverlight [<a href="#fswtintrolinks">4</a>] becomes more common, we would like to 
  allow using Silverlight as an alternative.</p>
    
<p>F# Web Tools allows you to write client-side code in F# 
  (so if you don't know JavaScript, you don't have to learn it!) and also use your existing knowledge 
  of .NET and F# classes and functions (so you can use some of the .NET and F# types when
  writing client-side code). The code you write in F# is of course executed in JavaScript, so it 
  runs in any browser that supports JavaScript - the current implementation is tested with IE and 
  Firefox, but it could be easily tested with other browsers.</p>

<h3>2. Discontinuity between server and client side</h3>
<p>The second major problem with "Ajax" applications is that the web application has to be written 
  as two separate parts - client-side part (when written in JavaScript) consists of several JS files
  and the server-side part (for example in ASP.NET) is written as a set of ASPX and C# or VB files.
  Also when using JavaScript, both sides use different formats to store the data, so bridging this
  gap is difficult. In Silverlight [<a href="#fswtintrolinks">4</a>] (or in GWT [<a href="#fswtintrolinks">2</a>]), the gap is still there, even though both parts are 
  written using the same technology - the client-side part is usually even a separate project.</p>

<p>In F# Web Tools we wanted to make this discontinuity as small as possible - You can write
  both server and client-side code in a same file (as a code-behind code). You can also call server-side
  functions from the client-side code and you can use certain data-types (including your own) in both
  sides and you can send them as an arguments from one side to the other. What's also important is that 
  these calls are done without blocking the browser, but without the usual cumbersome programming 
  style (calling a function, setting a callback and writing the rest of the code in the callback).
</p>
<h3>3. Components in web frameworks are only server-side</h3>
<p>The third key problem appears once we tightly integrate client and server side code, because there
  is one more step that has to be done - most of the web frameworks have some way for composing web
  site from smaller pieces (in ASP.NET this is done using controls) and by defining the interaction 
  between these pieces, however they allow defining the interaction only for the server-side, which is
  rather problematic in "Ajax" applications, where most of the interaction between components is done
  on the client-side. </p>
<p>Since F# Web Tools is built using ASP.NET, we wanted to allow same compositionality as ASP.NET -
  to achieve this, controls written using F# Web Tools can wrap both server and client side functionality.
  Controls than expose both server and client side properties and events that can be used by the page
  to implement server-side, respectively client-side interaction between components.
  </p>  

<h2>Example - "Ajax" dictionary</h2>
<img src="/articles/fswebtools/intro_dict.png" alt="Ajax dictionary" style="float:right; margin:10px;" />
<p>
  I will demonstrate some of the F# Web Tools features using an "Ajax" dictionary application 
  (see screenshot on the right side) which displays possible matching words as you type the word
  you want to find. This is one of the typical "Ajax" tasks, so it's a good example to start with.
  First, we need to define the code-behind file for the ASP.NET page - the page itself is quite simple,
  it contains just two controls - textbox for entering word that you're looking for (<code>txtInput</code>)
  and generic element for displaying results (<code>ctlOutput</code>), so let's look at the code-behind:</p>

    // F# record type, which will be used for sending lookup 
    // results from the server-side to the client-side
    type SearchResult = { English:string; Other:string; }

    // Code-behind type for the ASP.NET page
    [<MixedSide>]
    type Suggest = 
      inherit ClientPage as base
  
      // Controls for entering text and displaying result
      val mutable txtInput : TextBox
      val mutable ctlOutput : Element

      (* .. interaction of components will go here .. *)

<p>Aside from the code-behind class, we also defined a type (<code>SearchResult</code>), which will be used
  for returning loaded results from server to the client side - it is just a F# record containing two strings
  (word in English and word in the language we're translating to). Now let's look at the methods that define
  the interaction logic. The first method that we will look at is a method running on the server-side that
  takes entered text as an argument and returns a collection of results (<code>ResizeArray&lt;SearchResult&gt;</code>)
  - it is a static method, so it can't modify anything else on the page (I will write about non-static methods 
  in one of the next articles):</p>

    // Searches the dictionary for specified prefix
    static member LoadSuggestions(prefix) = 
      server
        { let db = DictionaryDb(connectionString)
          let res = 
            SQL <@@ { for p in §db.Dictionary
                     when p.English.StartsWith(§prefix) 
                     -> {English=p.English; Other=p.Other} }
                     |> truncate 10 @@> 
          return (new ResizeArray<_>(res)) }

<p>The method is all wrapped in <code>server</code> computational expression (this is one of the new F# features - it will be 
  in details described in the Expert F# [<a href="#fswtintrolinks">6</a>] book, but I'll definitely write a few lines about it as well) - for now you can read it as (almost) ordinary
  F# code wrapped in a block that specifies how the code should be executed. The <code>server</code> block is executed
  as ordinary F# code, but wrapping the code in this block allows us to call it from the client side later. The <code>server</code>
  block also changes the type of the method, so it doesn't return <code>ResizeArray&lt;SearchInfo&gt;</code>, but 
  <code>Server&lt;ResizeArray&lt;SearchInfo&gt;&gt;</code>, where the <code>Server</code> type helps to ensure that the 
  code will be executed only on the correct side.
  </p>
<p>Now, let's look at the members that define client-side interaction. The entry point on the client-side is a method
  called <code>Client_Load</code>, which is executed when the page is loaded in the browser:</p>

    /// Initialization on the client side - attach event handlers    
    [<ReflectedDefinition>]
    member this.Client_Load(sender, e) =
      client
        { do this.txtInput.ClientKeyUp.AddClient(this.UpdateSuggestions) }

<p>In this case we just register an event handler that will be called whenever user types something to the 
  <code>txtInput</code> textbox. It is important to note that this code will be executed in JavaScript - even though
  when writing it, it is easy to forget about this! You can see the method that is used as an event handler in the next 
  code sample (this whole method will be executed in JS as well):</p>

    /// When text changes, trigger server call to update suggestions
    [<ReflectedDefinition>]
    member this.UpdateSuggestions(sender, e) =
      client
        { do! asyncExecute
               (client_async 
                 { let  sprefix = this.txtInput.Text
                   let! sugs = serverExecute(Suggest.LoadSuggestions(sprefix))
                   do!  this.DisplayResponse(sugs) }) }            

<p>If you look at the method, you can see that it contains call to the <code>asyncExecute</code> function and the 
  argument given to the function is entire block of F# code marked using <code>client_async</code> computational expression.
  The <code>asyncExecute</code> function executes the given block <em>asynchronously</em>, which means that it doesn't
  block the calling function (and it doesn't block browser GUI) - you can look at it as if it created new thread (but it's actually using one trick from functional 
  programming called <em>continuation passing style</em>, because JavaScript doesn't support threads). In the 
  <code>client_async</code> block, we first read the value from the textbox and then call the <code>LoadSuggestions</code>
  static method to get collection with matching words. The call to the server-side functions is done using <code>serverExecute</code>
  function (which is a special function, because it bridges the gap between client and the server automatically). Once
  we get the result we can call the <code>DisplayResponse</code> method to display the results. You can see that we 
  used a <code>let!</code> and <code>do!</code> instead of <code>let</code> and <code>do</code> here - this is because
  we're calling a methods that are written using F# computational expressions (<code>server</code> or <code>client</code> blocks).
  In general when you're calling any ordinary code, you can use the operators without the exclamation mark, but when 
  calling a code wrapped in a block you have to use <code>let!</code> or <code>do!</code>.</p>

    /// Display newly received array with results
    [<ReflectedDefinition>]
    member this.DisplayResponse (sugs:ResizeArray<Result>) =
      client
        { let sb = StringBuilder()
          let array = sugs.ToArray()
          do  Array.iter (fun (res) -> 
                sb.Append("<li><strong>" + r.Source + 
                          "</strong> - " + r.Target + "</li>") |> ignore)
          do   this.ctlOutput.InnerHtml <- "<ul>" + (sb.ToString()) + "<ul>" }

<p>In this last sample code, we just generate HTML code from the data we received from the server and display them.
  It is interesting to note, that the code is running on the client-side (it's wrapped in the <code>client</code> block),
  but you can still use some F#/.NET types and functions in the code (we're using <code>ResizeArray</code>, 
  <code>StringBuilder</code> classes and <code>Array.iter</code> function). This is possible because F# Web Tools
  re-implements subset of the F#/.NET functionality for the client-side code. And that's all - I described slightly simplified
  version of one of the F# Web Tools demos, but you can get the full source code if you check out our CodePlex project.
  You can also look at the <a href="http://wintax.ms.mff.cuni.cz/petrt4cm/afax/dictionary/suggest.aspx">live Dictionary sample</a>
  [<a href="http://wintax.ms.mff.cuni.cz/petrt4cm/afax/dictionary/suggest.aspx" target="_blank">^</a>].</p>

<h2>More information</h2>
<p>I think this example gives you a general idea what is the F# Web Tools and why it is interesting. I will definitely
  write more about it in the future, because I didn't describe all important features in this example. If you're interested
  in this project, you can also read the Paper we submitted to the ML Workshop or slides from the presentation I did at the 
  end of my internship in MSR Cambridge (see below). This project is also available to the community at CodePlex, so 
  you can look at the source code (including two more samples).</p>

<ul>
  <li><a href="/academic/articles/fswebtools/">F# Web Tools: Rich client/server web applications in F#</a> - Research paper (unpublished draft)</li>
  <li><a href="/academic/articles/fswebtools/fswebtools_v1.pdf">Ajax-style Client/Server Programming with F#</a> (Slides, PDF) - Final Internship Presentation</li>
  <li><a href="http://www.codeplex.com/fswebtools">F# Web Tools at CodePlex</a> [<a href="http://www.codeplex.com/fswebtools" target="_blank">^</a>]- Project &amp; Samples source code</li>
</ul>    

<h2>Related projects and links<a name="fswtintrolinks"></a></h2>
<ul>
  <li>[1] <a href="http://channel9.msdn.com/Showpost.aspx?postid=324060">Erik Meijer: Volta - Wrapping the Cloud with .NET - Part 1</a> [<a href="http://channel9.msdn.com/Showpost.aspx?postid=324060" target="_blank">^</a>] - Erik Meijer, Microsoft</li>
  <li>[2] <a href="http://code.google.com/webtoolkit/">Google Web Toolkit</a> [<a href="http://code.google.com/webtoolkit/" target="_blank">^</a>] - Google</li>
  <li>[3] <a href="http://groups.inf.ed.ac.uk/links/">Links: Linking Theory to Practice for the Web</a> [<a href="http://groups.inf.ed.ac.uk/links/" target="_blank">^</a>] - Ezra Cooper, Sam Lindley, Philip Wadler, Jeremy Yallop</li>
  <li>[4] <a href="http://silverlight.net/">Microsoft Silverlight</a> [<a href="http://silverlight.net/" target="_blank">^</a>] - Microsoft</li>
  <li>[5] <a href="http://www.nikhilk.net/ScriptSharpIntro.aspx">Script#</a> [<a href="http://www.nikhilk.net/ScriptSharpIntro.aspx" target="_blank">^</a>] - Nikhil Kothari</li>
  <li>[6] Book: <a href="http://www.amazon.com/Expert-F-Antonio-Cisternino/dp/1590598504">Expert F#</a> [<a href="http://www.amazon.com/Expert-F-Antonio-Cisternino/dp/1590598504" target="_blank">^</a>] - Don Syme, Adam Granicz, Antonio Cisternino</li>
  <li>[7] Book: <a href="http://www.amazon.com/Foundations-F-Robert-Pickering/dp/1590597575">Foundations of F#</a> [<a href="http://www.amazon.com/Foundations-F-Robert-Pickering/dp/1590597575" target="_blank">^</a>] - Robert Pickering</li>
  <li>[8] <a href="http://research.microsoft.com/~dsyme/papers/ml03-syme.pdf">Leveraging .NET Meta-programming Components from F#</a> (PDF)</li>

</ul>