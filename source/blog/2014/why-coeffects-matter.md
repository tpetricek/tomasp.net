Coeffects: The next big programming challenge
=============================================

 - date: 2014-01-08T15:31:49.9070738+00:00
 - description: This blog post is a tabloid-style introduction to my PhD thesis. I look at how practical motivations influence language design. One such practical motivation, that is becoming increasingly important, is capturing how program uses the context (or environment) where it runs. Coeffects are a way to track this property...
 - layout: post
 - tags: research,coeffects,functional programming,comonads
 - title: Coeffects: The next big programming challenge
 - url: 2014/why-coeffects-matter

--------------------------------------------------------------------------------
 - standalone

Many advances in programming language design are driven by some practical motivations.
Sometimes, the practical motivations are easy to see - for example, when they come from
some external change such as the rise of multi-core processors. Sometimes, discovering 
the practical motivations is a tricky task - perhaps because everyone is used to a 
certain way of doing things that we do not even _see_ how poor our current solution is. 

The following two examples are related to the work done in F# (because this is what
I'm the most familiar with), but you can surely find similar examples in other languages:

 * **Multi-core** is an easy to see challenge caused by an external development. 
   It led to the popularity of _immutable_ data structures (and functional programming,
   in general) and it was also partly motivation for [asynchronous workflows][async].

 * **Data access** is a more subtle challenge. Technologies like [LINQ][linq] make it
   significantly easier, but it was not easy to see that inline SQL was a poor solution.
   This is even more the case for F# _type providers_. You will not realize how poor the
   established data access story is until you _see_ something like 
   the [WorldBank and R provider][rdemo] or [CSV type provider][csv].

I believe that the next important practical challenge for programming language designers
is of the kind that is not easy to see - because we are so used to doing things in 
certain ways that we cannot see how poor they are. The problem is designing languages
that are better at working with (and understanding) the _context_ in which programs are
executed.

--------------------------------------------------------------------------------


Many advances in programming language design are driven by some practical motivations.
Sometimes, the practical motivations are easy to see - for example, when they come from
some external change such as the rise of multi-core processors. Sometimes, discovering 
the practical motivations is a tricky task - perhaps because everyone is used to a 
certain way of doing things that we do not even _see_ how poor our current solution is. 

The following two examples are related to the work done in F# (because this is what
I'm the most familiar with), but you can surely find similar examples in other languages:

 * **Multi-core** is an easy to see challenge caused by an external development. 
   It led to the popularity of _immutable_ data structures (and functional programming,
   in general) and it was also partly motivation for [asynchronous workflows][async].

 * **Data access** is a more subtle challenge. Technologies like [LINQ][linq] make it
   significantly easier, but it was not easy to see that inline SQL was a poor solution.
   This is even more the case for F# _type providers_. You will not realize how poor the
   established data access story is until you _see_ something like 
   the [WorldBank and R provider][rdemo] or [CSV type provider][csv].

I believe that the next important practical challenge for programming language designers
is of the kind that is not easy to see - because we are so used to doing things in 
certain ways that we cannot see how poor they are. The problem is designing languages
that are better at working with (and understanding) the _context_ in which programs are
executed.

> This article is a brief summary of the work that I did (am doing) during my PhD.
> It focuses on clearly explaining the motivation for the work, but if you're interested
> in the solution (and the theory), then you can find more on [my academic home page](http://www.cl.cam.ac.uk/~tp322/).

Context-aware programming matters
---------------------------------

The phrase _context in which programs are executed_ sounds quite abstract and generic.
What are some concrete examples of such _context_? For example: 

 - When writing a cross-platform application, different platforms (and even different 
   versions of the same platform) provide different contexts - the API functions that are 
   available.
      
 - When creating a mobile app, the different capabilities that you may (or may not) have
   access to are context (GPS sensor, accelerometer, battery status).

 - When working with data (be it sensitive database or social network data from Facebook),
   you have permissions to access only some of the data (depending on your identity) and
   you may want to track _provenance_ information. This is another example of a context.
 
These are all fairly standard problems that developers deal with today. As the number of 
devices where programs need to run increases, dealing with diverse contexts will be becoming
more and more important (and I'm not even talking about _ubiquitous computing_ where you need
to compile your code to a coffee machine).

We do not perceive the above things as problems (at best, annoyances that we just have to 
deal with), because we do not realize that there should be a better way. Let me dig into
four examples in a bit more detail.

    [hide]
    let (|StringEquals|_|) s1 s2 = Some()
    let headers = ["",""]
    let req : System.Net.HttpWebRequest = failwith ""
    let hasContentType = ref false

### Context awareness #1: Platform versioning

The number of different platform versions that you need to target is increasing, no matter what 
platform you are using. For Android, there is a number called [API Level](http://developer.android.com/guide/topics/manifest/uses-sdk-element.html#ApiLevels)
with mostly additive changes (*mostly* sounds very reassuring). In the .NET world, there are multiple
versions, mobile editions, multiple versions of Silverlight etc. So, code that targets multiple
frameworks easily ends up looking as the following sample from [the Http module in F# Data](https://github.com/fsharp/FSharp.Data/blob/master/src/Net/Http.fs):

    for header, value in headers do
      match header with
      | StringEquals "accept" -> req.Accept <- value
    #if FX_NO_WEBREQUEST_USERAGENT
      | StringEquals "user-agent" -> 
          req.Headers.[HttpRequestHeader.UserAgent] <- value
    #else
      | StringEquals "user-agent" -> req.UserAgent <- value
    #endif
    #if FX_NO_WEBREQUEST_REFERER
      | StringEquals "referer" -> 
          req.Headers.[HttpRequestHeader.Referer] <- value
    #else
      | StringEquals "referer" -> req.Referer <- value
    #endif
      | _ -> req.Headers.[header] <- value

This is difficult to write (you won't know if the code even compiles until you try building all 
combinations) and maintaining such code is not particularly nice (try adding another platform version).
Maybe we could refactor the code in F# Data to improve it, but that's not really my point - supporting
multiple platforms should be a lot easier.

On Android, you can access API from higher level platform dynamically using techniques like 
[reflection and writing wrappers](http://android-developers.blogspot.cz/2009/04/backward-compatibility-for-android.html).
Again, this sounds very error prone - to quote the linked article 
_"Remember the mantra: if you haven't tried it, it doesn't work"._
Testing is no doubt important. But at least in statically typed languages, we
should not need to worry about calling a method that does not exist when writing
multi-platform applications!

### Context awareness #2: System capabilities

Another example related to the previous one is when you're using something like LINQ or 
[FunScript](http://funscript.info/) to translate code written in a sub-set of C# or F#
to some other language - such as SQL or JavaScript. This is another important area, because
you can use this technique to target JavaScript, GPUs, SQL, Erlang runtime or other components 
that use a particular programming language.

For example, take a look at the following LINQ query that selects product names where the
first upper case letter is "C":

    [lang=csharp]
    var db = new NorthwindDataContext();

    from p in db.Products
    where p.ProductName.First(c => Char.IsUpper(c)) == "C"
    select p.ProductName;

This looks like a perfectly fine piece of code and it compiles fine. But when you try running
it, you get the following error:

> Unhandled Exception: `System.NotSupportedException`: Sequence operators 
> not supported for type `System.String`.

The problem is that LINQ can only translate a _subset_ of normal C# code. Here, we are using
the `First` method to iterate over characters of a string and that's not supported. This is
not a technical limitation of LINQ, but a fundamental problem of the approach. If we target a 
limited language, we simply cannot support the full source language. Is this an important 
problem? If you search on Google for ["has no supported translation to SQL"](https://www.google.co.uk/#q=%22has+no+supported+translation+to+SQL%22)
(which is a part of a similar error message that you get in another case), you get some 26900
links. So yes - this is an issue that people are hitting all the time.

### Context awareness #3: Confidentiality and provenance

    [hide]
    let name = ""
    open System.Data.SqlClient
    let conn = SqlConnection("..")

The previous two examples were mainly related to the (non-)existence of some API functions
or to their behaviour. However, this is not the only case of context-awareness that is important
in every day programming.

Most readers of this blog will immediately see what is wrong with the following code, but that
does not change the fact that it can be compiled and deployed (and that there is a large number
of systems that contain something similar):

    let query = sprintf "SELECT * FROM Products WHERE ProductName = '%s'" name
    use cmd = new SqlCommand(query)
    use reader = cmd.ExecuteReader()

The problem is obviously [SQL Injection](http://en.wikipedia.org/wiki/SQL_injection). Concatenating
strings to build an SQL command is a bad practice, but it is an example of a more general problem.

Sometimes, we have special kinds of variables that should have some contextual meta-data associated
with them. Such meta-data can dictate how the variable can be used. Here, `name` comes from the user
input and this _provenance_ information should propagate to `query` and we should get an error when
we try passing this - potentially unsafe - input to `SqlCommand`. Similarly, if you have `password` or
`creditCard`, it should be annotated with meta-data saying that this is a sensitive piece of data and
should not, for example, be sent over an unsecured network connection.
As a side note - this idea is related to a technique called [taint checking](http://en.wikipedia.org/wiki/Taint_checking).

In another context, if you are working with data (e.g. in some data journalism application), it would be
great if your sources were annotated with meta-data about the quality and source of the data (e.g.
can it be trusted? how up-to-date is it?) The meta-data could propagate to the result and tell you how
accurate and trustworthy your result is.

### Context-awareness #4: Resource & data availability

A vast majority of applications accesses some data sources (like database) or resources
(like GPS sensor on a phone). This is more tricky for client/server applications where a part
of program runs on the server-side and another part runs on the client-side. I believe that 
these two parts should be written as a single program that is cross-compiled to two parts 
(and I tried to [make that possible with F# Web Tools](http://tomasp.net/blog/fswebtools-intro.aspx/)
a long time ago; more recently [WebSharper](http://websharper.com/) implemented a similar idea).

So, say we have a function `validateInput`, `readData` and `displayMessage` in my program.
I want to look at their types and see what resources (or _context_) they require. For example,
`readData` requires _database_ (or perhaps a database with a specific name), `displayMessage` 
requires access to _user interface_ and `validateInput` has no special requirements.

This means that I can call `validateInput` from both server-side and client-side code - it is
safe to share this piece of code, because it has no special requirements. However, when I write
a code that calls all three functions without any remote calls, it will only run on a thick
client that has access to a database as well as user interface.

I'll demonstrate this idea with a sample (pseudo-)code in a later section, so do not worry if
it sounds a bit abstract at first.

Coeffects: Towards context-aware languages
------------------------------------------

The above examples cover a couple of different scenarios, but they share a common theme - 
they all talk about some _context_ in which an expression is evaluated. The context has 
essentially two aspects:

 - **Flat context** represents additional data, resources and meta-data that are 
   available in the execution environment (regardless of where in the program you 
   access them). Examples include resources like GPS sensors or databases, battery status,
   framework version and similar. 

 - **Structural context** contains additional meta-data related to variables. This can include
   provenance (source of the variable value), usage information (how often is the value
   accessed) or security information (does it contain sensitive data). 

As a proponent of statically typed functional languages I believe that a context-aware 
programming language should capture such context information in the type system and make
sure that basic errors (like the ones demonstrated in the four examples above) are ruled
out at compile time.  

This is essentially the idea behind _coeffects_. Let's look at an example showing the
idea in (a very simplified) practice and then I'll say a few words about the theory
(which is the main topic of my upcoming PhD thesis).

### Case study: Coeffects in action

    [hide]
    type Database = DB
    type Location = LOC
    let password = ""
    let query(database,password:string) = DB
    let selectNews(db:Database, loc:Location) = [""]
    type RemoteBuilder() = 
      member x.Delay(f) = f()
      member x.Zero() = ()
    let remote = new RemoteBuilder()
    let createCocoaWidget f = f() |> ignore
    let gpsLocation () = LOC
    let createMetroWidget f = f() |> ignore

So, how should a context-aware language look? Surely, there is a wide range of options, but I hope
I convinced you that it needs to be *context-aware* in some way! I'll write my pseudo-example in a
language that looks like F#, is fully statically typed and uses type inference.

I think that type inference is particularly important here - we want to check quite a few properties
that should not that difficult to infer (If I call a function that needs GPS, I need to have GPS access!)
Writing all such information by hand would be very cumbersome.

So, let's say that we want to write a client/server news reader where the news are stored in a 
database on a server. When a client (iPhone or Windows phone) runs, we get GPS location from the
phone and query the server that needs to connect to the "News" database using a password defined
somewhere earlier (perhaps loaded from a server-side config file):

    let lookupNews(location) =
      let db = query("News", password)
      selectNews(db, location)  

    let readNews() =
      let loc = gpsLocation()       
      remote { 
        lookupNews(loc) 
      } 

    let iPhoneMain() =
      createCocoaWidget(readNews)

    let windowsMain() =    
      createMetroWidget(readNews)

The idea is that `lookupNews` is a server-side function that queries the "News" database based on 
the specified `location`. This is called from the client-side by `readNews` which get the current GPS
position and uses a `remote { .. }` block to invoke the `lookupNews` function remotely (how exactly would
this be written is a separate question - but imagine a simple REST request here).

Then, we have two main functions, `iPhoneMain` and `windowsMain` that will serve as two entry points
for iPhone and Windows build of the client-side application. They are both using a corresponding
platform-specific function to build the user interface, which takes a function for reading news as an
argument.

If you wanted to write and compile something like this today, you could use [F# in Xamarin 
Studio](http://docs.xamarin.com/guides/cross-platform/fsharp/fsharp_support_overview/) to target iPhone and
Window phone, but you'd either need two separate end-application projects or a large number of unmaintainable
`#if` constructs. Why not just use a single project, if the application is fairly simple?

I imagine that a context-aware statically typed language would let you write the above code and 
if you inspected the types of the functions, you would see something like this:

<div class="tip" style="display:block;margin:0px 40px 20px 20px;padding:10px 10px 10px 20px;font-size:100%">
<strong style="font-size:90%;font-family:'Droid Sans Mono'">password&#160;&#160;&#160;</strong> &#160;
  : &#160; string <sup>{ sensitive }</sup><br />
<strong style="font-size:90%;font-family:'Droid Sans Mono'">lookupNews&#160;</strong> &#160;
  : &#160; Location <sup>{ database }</sup> &#8594; list&lt;News&gt; <br />
<br />
<strong style="font-size:90%;font-family:'Droid Sans Mono'">gpsLocation</strong> &#160;
  : &#160; unit <sup>{ gps }</sup> &#8594; Location <br />
<strong style="font-size:90%;font-family:'Droid Sans Mono'">readNews&#160;&#160;&#160;</strong> &#160;
  : &#160; unit <sup>{ rpc, gps }</sup> &#8594; Async&lt;list&lt;News&gt;&gt; <br />
<br />
<strong style="font-size:90%;font-family:'Droid Sans Mono'">iPhoneMain&#160;</strong> &#160;
  : &#160; unit <sup>{ cocoa, gps, rpc }</sup> &#8594; unit <br />
<strong style="font-size:90%;font-family:'Droid Sans Mono'">windowsMain</strong> &#160;
  : &#160; unit <sup>{ metro, gps, rpc }</sup> &#8594; unit <br />
</div>

The syntax is something that I just made up for the purpose of this article - it could
look different. Some information could even be mapped to other visual representations
(e.g. blueish background for the function body in your editor). The key thing is that
we can learn quite a lot about the context usage:

 - `password` is available in the context, but is sensitive and so we cannot return it
   as a result from a function that is called via an RPC call.
 - `lookupNews` requires database access and so it can only run on the server-side
   or on a thick client with local copy of the database.
 - `gpsLocation` accesses GPS and since we call it in `readNews`, this function
   also requires GPS (the requirement is propagated automatically).
 - We can compile the program for two client-side platforms - the entry points require
   GPS, the ability to make RPC calls and Cocoa or Metro UI platform, respectively.

When writing the application, I want to be always able to see this information (perhaps
similarly to how you can see type information in the various F# editors). I want to be
able to reference multiple versions of base libraries - one for iPhone and another for
Windows and see all the API functions at the same time, with appropriate annotations.
When a function is available on both platforms, I want to be able to reuse the code that
calls it. When some function is available on only one platform, I want to solve this by
designing my own abstraction, rather than resorting to ugly `#if` pragmas.

Then, I want to take this single program (again, structured using whatever abstractions
I find appropriate) and compile it. As a result, I want to get a component (containing
`lookupNews`) that I can deploy to the server-side and two packages for iPhone and 
Windows respectively, that reference only one or the other platform.

### Coeffects: Theory of context dependence

If you're expecting a "Download!" button or (even better) a "Buy now!" button at the end of this article,
then I'll disappoint you. I have no implementation that would let you do all of this.
My work in this area has been (so far) on the theoretical side. This is a great way to 
understand what is _actually_ going on and what does the _context_ mean. And if you made
it this far, then it probably worked, because I understood the problem well enough to be 
write a readable article about it!

If you are interested in the theory, then go ahead and have a look at our papers about
coeffects, otherwise continue reading and I'll try to introduce the key ideas in a more
accessible form!

 * [Coeffects: Unified static analysis of context-dependence](http://tomasp.net/academic/papers/coeffects/)
   (ICALP 2013) is an overview of the flat context (such as resources, framework version, etc.)
 * [Analysing context dependence in programs](http://tomasp.net/academic/drafts/coeffects-structural/)
   (Work in progress) is a revised version that also looks at structural (per-variable) coeffects
   such as security or variable usage

#### Brief introduction to type systems

I won't try to reproduce the entire content of the papers in this blog post - but I will
try to give you some background in case you are interested (that should make it easier to
look at the papers above). We'll start from the basics, so readers familiar with theory of 
programming languages can skip to the next section.

Type systems are usually described in the form of _typing judgement_ that have the following form:

<div style="margin-bottom:15px;text-align:center"><img src="typing-1.png" /></div>

The judgement means that, given some variables described by _Γ_, the expression or program _e_
has a type _τ_. What does this mean? For example, what is a type of the expression `x + y`?
Well, this depends - in F# it could be some numeric type or even a string, depending on the types
of `x` and `y`. That's why we need the variable context _Γ_ which specifies the types of variables.
So, for example, we can have:

<div style="margin-bottom:15px;text-align:center"><img src="typing-2.png" /></div>

Here, we assume that the types of `x` and `y` (on the left hand side) are both `int` and as a result,
we derive that the type of `x + y` is also an `int`. This is a valid typing, for the expression, but
not the only one possible - if `x` and `y` were of type `string`, then the result would also be 
`string`.

#### Checking what program _does_ with effect systems

Type systems can be extended in various interesting ways. Essentially, they give us an approximation
of the possible values that we can get as a result. For example [refinement types](http://en.wikipedia.org/wiki/Refinement_(computing)#Refinement_types)
can estimate numerical values more precisely (e.g. less than 100). However, it is also possible to 
track what a program does - how it _affects_ the world. For example, let's look at the following
expression that prints a number:

<div style="margin-bottom:15px;text-align:center"><img src="typing-3.png" /></div>

This is a reasonable typing in F# (and ML languages), but it ignores the important fact that the 
expression has a _side-effect_ and prints the number to the console. In Haskell, this would not be
a valid typing, because `print` would return an `IO` computation rather than just plain `unit` (for
more information see [IO in Haskell](http://www.haskell.org/haskellwiki/IO_inside)).

However, monads are not the only way to be more precise about side-effects. Another option is
to use an [effect system](http://en.wikipedia.org/wiki/Effect_system) which essentially annotates the 
result of the typing judgement with more information about the _effects_ that occur as part of
evaluation of the expression (here, in orange):

<div style="margin-bottom:15px;text-align:center"><img src="typing-4.png" /></div>

The effect annotation is now part of the type - so, the expression has a type `unit & { io }` meaning
that it does not return anything useful, but it performs some I/O operation. Note that we do not track
what _exactly_ it does - just some useful over-approximation. How do we infer the information?
The compiler needs to know about certain language primitives (or basic library functions). Here, 
`print` is a function that performs I/O operation.

The main role of the type system is dealing with composition - so, if we have a function `read` that
reads from the console (I/O operation) and a function `send` that sends data over network, the type
system will tell us that the type and effects of `send (read ())` are `unit & {io, network}`.

Effect systems are a fairly established idea - and they are a nice way to add better purity checking
to ML-like languages like F#. However, they are not that widely adopted (interestingly, checked
exceptions in Java are probably the most major use of effect system). However, effect systems are
also a good example of general approach that we can use for tracking contextual information...

#### Checking what program _requires_ with coeffect systems

How could we use the same idea of _annotating_ the types to capture information about the context?
Let's look at a part of the program from the case study that I described earlier:

<div style="margin-bottom:15px;text-align:center"><img src="typing-5.png" /></div>

The expression queries a database and gets back a value of the `NewsDb` type (for now, let's say
that `"News"` is a constant string and `query` behaves like the [SQL type provider in F#](http://www.pinksquirrellabs.com/post/2013/12/09/The-Erasing-SQL-type-provider.aspx)
and generates the `NewsDb` type automatically).

What information do we want to capture? First of all, we want to add an annotation saying that
the expression requires _database access_. Secondly, we want to mark the `pass` variable as 
_secure_ to guarantee that it will not be sent over an unsecured network connection etc.
The _coeffect typing judgement_ representing this information looks like this:

<div style="margin-bottom:15px;text-align:center"><img src="typing-6.png" /></div>

Rather than attaching the annotations to the _resulting type_, they are attached to the 
variable _context_. In other words, the equation is saying - given a variable `pass` that is
marked as secure and additional environment providing database access, the expression
`query("News", pass)` is well typed and returns a `NewsDb` value.

As a side-note, it is well known that _effects_ correspond to _monads_ (and Haskell uses
monads as a way of implementing limited effect checking). Quite interestingly, _coeffects_
correspond to the dual concept called _comonads_ and, with a syntactic extension akin to 
the `do` notation or _computation expressions_, you could capture contextual properties by
adding comonads to a language.

Summary
-------

This article is essentially a tabloid style report on my (upcoming) PhD thesis :-). 

I started by explaining the
motivation for my work - different problems that arise when we are writing programs that are
aware of the context in which they run. The context includes things such as execution environment
(databases, resources, available devices), platform and framework (different versions, different
platforms) and meta-data about data we access (sensitivity, security, provenance). 

This may not be perceived as a major problem - we are all used to write code that deals with
such things. However, I believe that the area of _context-aware_ programming is a source of
many problems and pains - and programming languages can help!

In the second half of the article, I gave a brief introduction to _coeffects_ - a programming
language theory that can simplify dealing with context.
The key idea is that we can use types to track and check additional information about
the _context_. By propagating such information throughout the program (using type system
that is aware of the annotations), we can make sure that none of the errors that I used
as a motivation for this article happen. 

### Where to go next?

If you want to learn more about the theory of _coeffects_ (and how they relate to 
_comonads_) then check out the two papers I mentioned earlier:

 * [Coeffects: Unified static analysis of context-dependence](http://tomasp.net/academic/papers/coeffects/)
   (ICALP 2013) is an overview of the flat context (such as resources, framework version, etc.)
 * [Analysing context dependence in programs](http://tomasp.net/academic/drafts/coeffects-structural/)
   (Work in progress) is a revised version that also looks at structural (per-variable) coeffects
   such as security or variable usage

Dominic Orchard (who is co-author of the above two) also did some nice work on
integrating comonads into Haskell:

  * [A Notation for Comonads (PDF)](http://www.cl.cam.ac.uk/~dao29/publ/codo-notation-orchard-ifl12.pdf)
    (Dominic Orchard and Alan Mycroft) extends Haskell with a syntax (akin to `do`) for 
    comonads and might be a nice way to embed coeffects in Haskell


 [async]: http://msdn.microsoft.com/en-us/library/dd233250.aspx "Asynchronous Workflows (F#)"
 [linq]: http://msdn.microsoft.com/en-us/library/bb397926.aspx "LINQ (Language-Integrated Query)"
 [rdemo]: http://www.youtube.com/watch?v=cCuGgA9Yqrs "F# R Type Provider Demo"
 [csv]: http://fsharp.github.io/FSharp.Data/library/CsvProvider.html "F# Data: CSV type provider"
