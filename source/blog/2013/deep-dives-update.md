Update on the F# Deep Dives book
================================

 - date: 2013-08-27T04:15:40.2249017+01:00
 - description: It has been some time since I last wrote about F# Deep Dives, so I'd like to write a quick update. In summary, having 10 authors does not mean that the book will be finished in 1/10 of the time, but now that the holidays are almost over, you can expect more frequent updates again!
 - layout: article
 - tags: manning,f#,writing,books,deep dives
 - title: Update on the F# Deep Dives book
 - url: 2013/deep-dives-update

--------------------------------------------------------------------------------
 - standalone

<img src="http://tomasp.net/articles/manning-deep-dives/cover.jpg" class="rdecor" title="F# Deep Dives" style="margin-left:20px;margin-bottom:15px"/>

It has been some time since I [last wrote about F# Deep Dives](http://tomasp.net/blog/manning-deep-dives.aspx/)
- a new project that I'm working on together with [Manning](http://www.manning.com),
[Phil Trelford](http://trelford.com/blog) and a number of F# experts, so I'd like to write a quick
update. In summary, working on a book with more than 10 co-authors is more difficult than one
would think (and 10 people cannot write a book in 1/10 of the time :-)), but now that the holidays
are almost over, you can expect more frequent updates again!

First of all, I shoud mention that you can buy [the Early Access preview](http://www.manning.com/petricek2/)
of the book from Manning and there is already one [in advance review of the book](http://blogs.tedneward.com/2013/01/05/Review+In+Advance+F+Deep+Dives.aspx)
from Ted Neward (thanks!) who says:

> As of this writing, the early-access [...] version had only Chapters 3 and Chapter 11, 
> but the other topics [...] are juicy and meaty. [T]he prose from the MEAP edition is 
> pretty easy to read already, despite the fact that it's early-access material. In particular, 
> the Markdown parser they implement in chapter 3 is a great example of a non-trivial 
> language parser, which is not an easy task (...).

As I mentioned, the book is unique in that it is not written just by me and Phil - each chapter 
is written by a real-world F# expert and many of them use F# in production. The disadvantage is
that they are all busy people, but we have close to half of the planned chapters available already...

--------------------------------------------------------------------------------


<img src="http://tomasp.net/articles/manning-deep-dives/cover.jpg" class="rdecor" title="F# Deep Dives" style="margin-left:20px;margin-bottom:15px"/>

It has been some time since I [last wrote about F# Deep Dives](http://tomasp.net/blog/manning-deep-dives.aspx/)
- a new project that I'm working on together with [Manning](http://www.manning.com),
[Phil Trelford](http://trelford.com/blog) and a number of F# experts, so I'd like to write a quick
update. In summary, working on a book with more than 10 co-authors is more difficult than one
would think (and 10 people cannot write a book in 1/10 of the time :-)), but now that the holidays
are almost over, you can expect more frequent updates again!

First of all, I should mention that you can buy [the Early Access preview](http://www.manning.com/petricek2/)
of the book from Manning and there is already one [in advance review of the book](http://blogs.tedneward.com/2013/01/05/Review+In+Advance+F+Deep+Dives.aspx)
from Ted Neward (thanks!) who says:

> As of this writing, the early-access [...] version had only Chapters 3 and Chapter 11, 
> but the other topics [...] are juicy and meaty. [T]he prose from the MEAP edition is 
> pretty easy to read already, despite the fact that it's early-access material. In particular, 
> the Markdown parser they implement in chapter 3 is a great example of a non-trivial 
> language parser, which is not an easy task (...).

Available chapters
------------------

As I mentioned, the book is unique in that it is not written just by me and Phil - each chapter 
is written by a real-world F# expert and many of them use F# in production. The disadvantage is
that they are all busy people, but we have close to half of the planned chapters available already.

 * **Parsing text-based languages** (written by myself) explains how I wrote the [Markdown parser](http://tpetricek.github.io/FSharp.Formatting/)
   used for generating this web site, documentation for [many](http://fsharp.github.io/FSharp.Data/)
   [F#](http://funscript.info/) [projects](http://tpetricek.github.io/FSharp.RProvider//tutorial.html).

 * **Numerical computing in finance domain** (by Chao-Jen Chen from the University of Chicago) deals
   with mathematical methods for pricing stock options and discusses topics such as geometric brownian
   motion and Monte Carlo simulations.

 * **Integrating Stock Data into the F# Language** (by [Ketih Battocchi](https://twitter.com/kbattocchi) who
   worked on type providers for Microsoft) is, unsurprisingly, about type providers! It explains how to
   extend CSV provider and write a type provider specifically for getting historical stock prices.

 * **Creating games using XNA** (by Johann Deneux, author of [F# for Game Development](http://sharp-gamedev.blogspot.com/)
   discusses how to elegantly express game logic using F# computation expressions.

 * **Building Social Web Applications** (by [Yan Cui](http://theburningmonk.com/)) is based on 
   Yan's experience with developing backend services for Facebook games in F#. Also have a look
   at Yan's [podcast at .NET Rocks!](http://www.dotnetrocks.com/default.aspx?ShowNum=846).

Upcoming chapters
-----------------

Now, what chapters can you look forward to if you bought the MEAP release?
In the next four-six weeks, we would like to release the following chapters:

 * **Succeeding with functional-first languages in Industry** (by Don Syme & myself)
   will give you a broader perspective to F# and functional programming. Why and when should
   you use F#? And what is the [F# Software Foundation](http://fsharp.org/)?

 * **F# in the enterprise** (by Chris Ballard) explains the compromises that F#
   developers need to make to succeed in the enterprise. Should you use functional style
   everywhere and for everything? Or should you just stick to objects?

 * **Test driven development in F#** (by [Phil Trelford](http://trelford.com/blog/)) will
   give you answers to the question of testing F# code. You'll read about various F# 
   testing [tools like Foq](http://foq.codeplex.com/).

 * **Trading application GUI with F#** (by Dmitry Morozov). Dmitry is the author of a
   [great F# MVC framework](https://github.com/dmitry-a-morozov/fsharp-wpf-mvc-series/wiki) for WPF
   and this chapter will outline the key ideas you need to know!

Aside from these, we still have a few more chapters that we'd like to add to the book. There has been
amazing development in the F# community recently regarding data science and machine learning, so 
expect some chapters from this area too. Follow this blog for more updates!