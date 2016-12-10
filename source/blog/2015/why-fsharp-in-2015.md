Why you should learn F# in 2015 (and how)?
==========================================

 - date: 2015-01-07T13:36:04.9887235+00:00
 - description: I guess that it might be a bit too late for adding to your list of new year's resolution now.But just if you still have an empty slot (or in case an originally taken slot has become available), your new year's resolution should be to get involved with F#!
 - layout: post
 - image: http://tomasp.net/blog/2015/why-fsharp-in-2015/gingerbread.jpg
 - tags: c#,f#,functional programming,talks
 - title: Why you should learn F# in 2015 (and how)?
 - url: 2015/why-fsharp-in-2015

--------------------------------------------------------------------------------
 - standalone

<img src="http://tomasp.net/blog/2015/why-fsharp-in-2015/gingerbread.jpg" style="width:200px;float:right;margin:15px" />

I guess that it might be a bit too late for adding to your list of new year's resulution now.
But just if you still have an empty slot (or in case an originally taken slot has become
available), your new year's resolution should be to get involved with F#!

Obviously, the goal of this blog post is to sell you some of my F# trainings and other materials -
including the online course on [F# in Finance](http://fsharpworks.com/workshops/finance.html) and our
[FastTrack to F#](http://fsharpworks.com/workshops/fast-track.html) course in London and New York
and also the [F# Deep Dives](http://manning.com/petricek2) book. But to conceal this fact, I'm
going to fill most of the blog post with useful information about F#, the F# Software Foundation
and the F# community (but if you really just want to read about my courses, scroll down to the
[second section](http://tomasp.net/blog/2015/why-fsharp-in-2015/index.html#courses)).

--------------------------------------------------------------------------------


<img src="http://tomasp.net/blog/2015/why-fsharp-in-2015/gingerbread.jpg" style="width:200px;float:right;margin:15px" />

I guess that it might be a bit too late for adding to your list of new year's resolution now.
But just if you still have an empty slot (or in case an originally taken slot has become 
available), your new year's resolution should be to get involved with F#!

Obviously, the goal of this blog post is to sell you some of my F# trainings and other materials - 
including the online course on [F# in Finance](http://fsharpworks.com/workshops/finance.html) and our 
[FastTrack to F#](http://fsharpworks.com/workshops/fast-track.html) course in London and New York
and also the [F# Deep Dives](http://manning.com/petricek2) book. But to conceal this fact, I'm
going to fill most of the blog post with useful information about F#, the F# Software Foundation
and the F# community (but if you really just want to read about my courses, scroll down to the 
[second section](#courses)).

Why you should get into F# now?
-------------------------------

F# has been around for some time. So, why should you be interested in F# in 2015? 
Both the F# community and the F# language and ecosystem have been evolving in the
last few years. In this article, I'll look at the most interesting recent trends 
that, I think, make F# more interesting than ever before.

### Friendly and active community

<img src="man.png" style="float:right;margin:15px;width:150px" />

Perhaps the most amazing thing about F# (and one of the reasons for looking into F#)
is the community around it. In the last year, I [visited 10 user groups](http://lanyrd.com/profile/tomasp/past/)
in Europe and US and I met fantastic people at every single meetup. For an even more
comprehensive study, see [Mathias Brandewinder's tour report](http://www.clear-lines.com/blog/post/The-2014-F-Tour-in-numbers.aspx).


Where else do you
see [8 year olds making an army of 3D man](http://seantrelford.blogspot.co.uk/2014/11/skills-matter-in-f-hackathon.html)
or [startup founders having fun with Warhol and Mona Lisa](https://twitter.com/orlandpm/status/510483892889845761)?

The number of user groups on [the Community for F# page](http://c4fsharp.net/groups.html)
is now 31 (compare that with 19 user groups a year ago and 12 user groups two years ago).
The [F# meetup in London](http://www.meetup.com/FSharpLondon/) now has over 860 members
(more than the [.NET user group](http://www.meetup.com/London-NET-User-Group/)). Finally,
the [number of #fsharp tweets per day](https://twitter.com/evelgab/status/553185963082792961)
is growing too!

### Growing slowly, but surely

The number of F# user groups simply follows a general trend. F# is not driven by any 
hype, so there are no crazy jumps, but instead a steady growth. To learn more, you can check
the growing list of happy [F# user testimonials](http://fsharp.org/testimonials/) including
line-of-business applications, financial systems, startups and even (real) architects.

Last year, F# even managed to get on [12th position on TIOBE](http://www.eweek.com/developer/microsofts-f-language-number-12-with-a-bullet.html),
which may be a suspicious methodology of measuring programming language popularity, but it
is certainly showing *some* trend.

Another great way to see the F# growth is to check [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) -
a newsletter aggregating F# articles and other resources. It is difficult to give a concrete
number (because of holidays etc.), but it feels that we might need *F# Daily* soon!

### Serious open source engineering

One of the reasons why I'm really excited about the F# community is that it takes open source
very seriously. I guess that this can be summarized by the following tweet:

<div style="margin-left:20px">
<blockquote class="twitter-tweet" lang="en"><p>Wow. Already 114 pull requests for the official <a href="https://twitter.com/VisualFSharp">@@VisualFSharp</a> compiler. As comparison <a href="https://twitter.com/hashtag/roslyn?src=hash">#roslyn</a> has 18. <a href="https://twitter.com/hashtag/fsharp?src=hash">#fsharp</a> community rocks.</p>&mdash; Steffen Forkmann (@@sforkmann) <a href="https://twitter.com/sforkmann/status/508869959364386816">September 8, 2014</a></blockquote>
<script async src="//platform.twitter.com/widgets.js" charset="utf-8"></script>
</div>

However, this is talking only about the F# compiler and core libraries. The F# community
is developing an amazing number of libraries (see [fsprojects](http://github.com/fsprojects/) on GitHub)
and is very serious about their quality. This is best demonstrated by the
[ProjectScaffold](http://fsprojects.github.io/ProjectScaffold/) template, which makes it easy
to create well-documented and tested libraries (see my talk
[Taking your craft seriously with F#](http://vimeo.com/111091463)).

Why does this matter? It encourages the community to be active. **When we do not like 
something, we do not complain, but we change it!** The best example of this is the
[Visual F# PowerTools](http://fsprojects.github.io/VisualFSharpPowerTools/) project -
a community effort that adds refactoring and other advanced tooling for F# (and solves
a common complaint about F#). 

### Web, cloud and data science

What would be a blog post without some buzz-words? However, all the three terms in the
heading are important and will, no doubt, matter even more in 2015. And the reason why
I'm mentioning them is that there are great open-source projects for all three of them
- two of those were actually open-sourced with a permissive license at the end of 2014, 
so I'm looking forward to the developments next year!

 * In the area of web development, you should watch [WebSharper 3.0, which is now Apache
   licensed](http://www.websharper.com/blog-entry/4124)! WebSharper lets you write both
   client-side and server-side code with F# and is an amazing piece of work.
 * For cloud-based and distributed computing, the recently open-sourced 
   [MBrace Framework](http://www.m-brace.net/) gives you a fantastic model based on 
   distributed computations with the "cloud monad".
 * For data science, we're actively working on [FsLab](http://fslab.org/), which puts
   together [F# Data type-providers](http://fsharp.github.io/FSharp.Data/), data analytics
   libraries such as [Deedle](http://bluemountaincapital.github.io/Deedle/) and other. If
   you're using Visual Studio, check out the [FsLab Journal](https://visualstudiogallery.msdn.microsoft.com/45373b36-2a4c-4b6a-b427-93c7a8effddb) template.

Now, there are many other libraries that I didn't mention here. The main reason is that the
above 3 have been started (or open-sourced under Apache) in 2014, so I expect there will
be a lot happening over the next year.

### Setting solid foundations with the F# Foundation

At the end of 2014, the F# Software Foundation has become an official 
non-profit organization. This reflects the fact that the community around [fsharp.org](http://fsharp.org)
has become the center-point of many F# activities and needs a proper ownership structure
with a board elected by the community. At the moment, we are nearly done with setting up the 
necessary structure and you can [join the foundation](http://foundation.fsharp.org/join) and 
participate in board elections in 2015.

The F# ecosystem is very community-driven and I believe that, once a proper board is elected, the 
F# Foundation will be an important mechanism for the development of the F# language, core libraries 
and tools, but also for supporting the community, running interesting events and providing an 
important long-term guarantee that makes F# attractive to businesses.

<a name="courses"></a>

So, if you want to be  one of the people who were there since the beginning, 
you can [join now](http://foundation.fsharp.org/join)!

The advertising part: Books, trainings and consulting
-----------------------------------------------------

As I promised at the beginning, I'll conclude this article with a bit of advertising. So, if
you added F# to the list of your new year resolutions, where should you start?

### F# and Functional Programming in Finance

<img src="http://tomasp.net/blog/2014/fsharpworks-events/qsh.jpg" style="float:right;width:100px;margin:10px" /> 

This is an online course teaching F# from the beginnings. The theme of the course is focused
on finance, so there is one lecture on numerical computing and one lecture on time-series analysis,
but it can serve as a good F# introduction even if you're working in another domain.

You can find more information on [course page at QuantsHub](http://quantshub.com/content/f-and-functional-programming-finance-tomas-petricek-1).
Also, there is a **20% early bird** discount if you book by Friday 9th January 2015
(drop me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net) by Friday if you 
need to get an approval and Friday is too soon).

### Fast Track to F# in London and NYC

We will be also running the [FastTrack to F#](https://skillsmatter.com/courses/473-tomas-petricek-phil-trelford-fast-track-to-fsharp) course
with SkillsMatter in London and New York this year. The page does not yet list all the dates,
but there will be more. I'm happy to offer **10% off** to the readers, so drop me an
email at [tomas@tomasp.net](mailto:tomas@tomasp.net) if you're interested!

The FastTrack to F# course is a 2 day course that combines introduction to the language 
(to get you started in the first day) with a focus on solving practical problems ranging
from working with data and type providers to testing and domain specific languages in the
second day.

### On-site trainings from fsharpWorks

<img src="http://tomasp.net/blog/2014/fsharpworks-events/logo-sm.png" style="float:right;width:100px" /> 

Finally, if you are looking for an on-site training or F# consulting (or a combination of
both) to get your team started with F#, then you should check out [fsharpWorks](http://fsharpworks.com).
I started fsharpWorks together with three other F# experts: [Phil Trelford](http://trelford.com/blog/),
[Mathias Brandewinder](http://www.clear-lines.com/blog/) and [Scott Wlaschin](http://fsharpforfunandprofit.com/).

fsharpWorks covers pretty much all F# areas that you might be interested in - financial computing,
domain driven design, machine learning and much more, so if you're ready to take F# seriously in
2015, drop me an
email at [tomas@tomasp.net](mailto:tomas@tomasp.net)!

### New book: F# Deep Dives

<img src="http://tomasp.net/blog/2015/why-fsharp-in-2015/dd.jpg" style="width:100px;float:right;margin:15px" />

Finally, after a long wait, the [F# Deep Dives book](http://manning.com/petricek2) is finally out!
Unlike existing books, F# Deep Dives is not really aimed at teaching you F#. Instead, it tries to
show you the different domains where F# is used in industry and demonstrate how F# developers approach
problems. It is a collection of real-world case studies, each written by expert practitioners including
[Phil Trelford](http://trelford.com/), [Dmitry Morozov](https://twitter.com/mitekm), [Evelina Gabasova](http://www.evelinag.com)
and many other. 

You can read this book in a number of ways - pick the domain that you are interested in (games, finance,
line-of-business applications, etc.), pick the technique (domain-specific languages, type providers, etc.),
or you can skim the first few pages of each chapter to see how F# is used to solve real-world problems.
