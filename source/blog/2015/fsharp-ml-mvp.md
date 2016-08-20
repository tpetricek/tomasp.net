F# + ML |> MVP Summit: Talk recordings, slides and source code
==============================================================

 - date: 2015-11-18T02:03:44.7462498+00:00
 - description: I was fortunate enough to make it to the Microsoft MVP summit this year. I didn't learn anything secret (and even if I did, I wouldn't tell you!) but one thing Idid learn is that there is a lot of interest in data science and machine learning. What was less expected and more exciting was that there was also a lot of interest in F#!
 - layout: post
 - image: http://tomasp.net/blog/2015/fsharp-ml-mvp/fsml.png
 - tags: f#,fslab,talks,machine learning,data science
 - title: F# + ML |> MVP Summit Talks
 - url: 2015/fsharp-ml-mvp

--------------------------------------------------------------------------------
 - standalone

<a href="http://www.functional-programming.net">
<img src="http://tomasp.net/blog/2015/fsharp-ml-mvp/fsml.png" style="width:200px;float:right;margin:0px 0px 15px 15px" />
</a>

I was fortunate enough to make it to the Microsoft MVP summit this year. I didn't learn anything 
secret (and even if I did, I wouldn't tell you!) but one thing I did learn is that there is a lot
of interest in data science and machine learning both inside Microsoft and in the MVP community.
What was less expected and more exciting was that there was also a lot of interest in F#, which 
is a perfect fit for both of these topics!

When I visited Microsoft back in May to talk about [Scalable Machine Learning and Data Science 
with F#](http://tpetricek.github.io/Talks/2015/scalable-ml-ds-fsharp/redmond/) at an internal event, 
I ended up chatting with the organizer about F# and we agreed that it would be nice to do more
on F#, which is how we ended up organizing the [F# + ML |> MVP Summit 2015](http://fsharpworks.com/mvp-summit/2015.html)
mini-conference on the Friday after the summit.

--------------------------------------------------------------------------------


<a href="http://www.functional-programming.net">
<img src="http://tomasp.net/blog/2015/fsharp-ml-mvp/fsml.png" style="width:200px;float:right;margin:0px 0px 15px 15px" />
</a>

I was fortunate enough to make it to the Microsoft MVP summit this year. I didn't learn anything 
secret (and even if I did, I wouldn't tell you!) but one thing I did learn is that there is a lot
of interest in data science and machine learning both inside Microsoft and in the MVP community.
What was less expected and more exciting was that there was also a lot of interest in F#, which 
is a perfect fit for both of these topics!

When I visited Microsoft back in May to talk about [Scalable Machine Learning and Data Science 
with F#](http://tpetricek.github.io/Talks/2015/scalable-ml-ds-fsharp/redmond/) at an internal event, 
I ended up chatting with the organizer about F# and we agreed that it would be nice to do more
on F#, which is how we ended up organizing the [F# + ML |> MVP Summit 2015](http://fsharpworks.com/mvp-summit/2015.html)
mini-conference on the Friday after the summit.

<img src="garage.jpg" style="margin:0px 0px 15px 0px" />

We run the event in the [pretty cool Garage space](http://news.microsoft.com/stories/garage/index.html)
on the campus. This means that we had to limit the number of attendees to about 45. We thought that
was enough only to find out that 50 more MVPs and Microsoft employees ended up on the waiting list 
and could not fit in! Fortunately, the whole event was also recorded by [the 
awesome](https://twitter.com/Golnaz89) [folks from Channel 9](https://twitter.com/sethjuarez)
and so you can watch the recordings and get the code below!

### Doing data science with FsLab

<img src="t1.jpg" style="width:250px;float:right;margin:0px 0px 15px 15px" />

In the morning, we did two talks. I talked about the [FsLab project](http://www.fslab.org)
and data science libraries including [F# Data](http://fsharp.github.io/FSharp.Data/), 
[Deedle](http://bluemountaincapital.github.io/Deedle/), [R type 
provider](http://bluemountaincapital.github.io/FSharpRProvider/) and 
[XPlot](http://tahahachana.github.io/XPlot/):

 * [Watch the recording](https://channel9.msdn.com/Events/FSharp-Events/fsharp-ML-MVP-Summit-2015/WelcomeIntroduction-and-Doing-Data-Science-with-FsLab) on Channel 9
 * [Get the slide deck](http://tpetricek.github.io/Talks/2015/data-science-with-fslab/#/)
   created using FsReveal
 * [Get the source code](https://github.com/tpetricek/Talks/tree/master/2015/data-science-with-fslab/code-done)
   from GitHub

### Crunching through big data with MBrace

<img src="t2.jpg" style="width:250px;float:right;margin:0px 0px 15px 15px" />

The second talk on the day was by Mathias Brandewinder who talked about scaling
machine learning algorithms to the cloud using the awesome [MBrace](http://mbrace.io/)
project:

 * [Watch the recording](https://channel9.msdn.com/Events/FSharp-Events/fsharp-ML-MVP-Summit-2015/Crunching-through-big-data-with-MBrace-Azure-and-F) on Channel 9
 * [Get the source code](https://github.com/mathias-brandewinder/Presentations/tree/master/data-crunching-with-mbrace)
   from GitHub

### Machine learning with F# and Accord.NET 

<img src="t3.jpg" style="width:250px;float:right;margin:0px 0px 15px 15px" />

And although this was not a part of the F# + ML event, Alena aka [@@lenadroid](https://twitter.com/lenadroid/)
also talked about [machine learning and F# at the .NET user group](http://www.meetup.com/NET-Developers-Association/events/224427645/) in Redmond
earlier in the week, so let me include it in the list:

* [Watch the recording](https://www.youtube.com/watch?v=6Yiiy-B1ix8) from DevDay in Krakow
* [Get the slides](http://lenadroid.github.io/presentations.html)
  from Alena's presentations page

After the lunch, we spent 2.5 hours working on a coding dojo where all the attendees had to solve
a machine learning problem in F# almost from scratch. We worked on a simple algorithm that 
recognizes the language of a text (downloaded from Wikipedia using an asynchronous crawler)
using k-nearest neighbor algorithm and also using a simple perceptron. The last part of the event,
which was equal fun, involved ask the experts sessions featuring [the numl library](http://numl.net/),
the [Prajna project](https://github.com/MSRCCS/Prajna) and [Azure ML](https://azure.microsoft.com/en-gb/services/machine-learning/) demos. 

Summary
-------

The one takeaway from the event was that there is a lot of interest in machine learning and
data science and F# is a great fit for both. If you are a .NET developer, then using F# is an 
obvious choice and [there is a great book for you](http://www.apress.com/9781430267676). If you
are not a .NET developer, then F# still has a lot to offer and [you can check out this free O'Reilly
report](http://www.oreilly.com/programming/free/analyzing-visualizing-data-f-sharp.csp). With
50 people on the waiting list, I think we'll just need to make a bigger event in the future!

Finally, if you are interested in using F# for machine learning and data science at work, let
me add that me and Mathias are both offering [F# training and workshops](http://fsharpworks.com/workshops.html)
via fsharpWorks and so if you want to learn more, drop us an email!