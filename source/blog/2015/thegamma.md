The Gamma: Simple code behind interactive articles
=================================================

 - date: 2015-09-28T17:07:08.0598479+01:00
 - description: Computer programming may not be the new literacy, but it is finding its way intomany areas of modern society. In particular, data journalism turns traditional news reports from a mix of text and images into something that is much closer to a computer program.By treating articles as programs, we can make data journalism more transparent, reproducible and interactive. This is what I've been working on recently, so check out the prototype!
 - layout: post
 - image-large: http://tomasp.net/blog/2015/thegamma/chart.png
 - tags: thegamma,type providers,data journalism,programming languages
 - title: The Gamma: Simple code behind interactive articles
 - url: 2015/thegamma

--------------------------------------------------------------------------------
 - standalone

<img src="http://tomasp.net/blog/2015/thegamma/dj.png" style="float:right;width:300px"
  title="Illustration from 'The Data Journalism Handbook'" />

There are huge amounts of data around us that we could use to better understand the world.
Every company collects large amounts of data about their sales or customers. Governments and
international organizations increasingly release interesting data sets to the public through
various _open government data_ initiatives ([data.gov](http://data.gov) or 
[data.gov.uk](http://data.gov.uk)). But raw data does not tell you much.

An interesting recent development is _data journalism_. Data journalists tell stories using
data. A data driven article is based on an interesting observation from the data, it includes
(interactive) visualizations that illustrate the point and it often allows the reader to get
the raw data.

Adding a chart produced in, say, Excel to an article is easy, but building good interactive
visualization is much harder. Ideally, the data driven article should not be just text with
static pictures, but a _program_ that links the original data source to the visualization.
This lets readers explore how the data is used, update the content when new data is available
and change parameters of the visualization if they need to understand different aspect of the
topic.

This is in short what I'm trying to build with [The Gamma project](http://thegamma.net). If
you're interested in building better reports or data driven articles, continue reading!

> <img src="http://tomasp.net/blog/2015/thegamma/sl.png" style="float:right;width:150px" />
>
> I did a talk about The Gamma project at the fantastic [Future Programming workshop](http://www.future-programming.org/programSL.html)
> at the [StrangeLoop conference](http://www.thestrangeloop.com/) last week (thanks for inviting me!)
> and there is a [recording of my 40 minute talk on YouTube](https://www.youtube.com/watch?v=cYoO2RvZn7Y&feature=youtu.be&a),
> so if you prefer to watch videos, check it out!

Are you a data journalist or data analyst? We're looking for early partners!
I joined the <a href="http://www.joinef.com" title="Nothing to do with Entity Framework, don't worry!">EF
programme</a> to work on this and if the project sounds like something you'd like to see happen,
<a href="mailto:tomas@tomasp.net">please get in touch</a> or share your contact details
on [The Gamma page](http://thegamma.net)!

--------------------------------------------------------------------------------


<img src="http://tomasp.net/blog/2015/thegamma/dj.png" style="float:right;width:300px"
  title="Illustration from 'The Data Journalism Handbook'" />

There are huge amounts of data around us that we could use to better understand the world.
Every company collects large amounts of data about their sales or customers. Governments and
international organizations increasingly release interesting data sets to the public through
various _open government data_ initiatives ([data.gov](http://data.gov) or 
[data.gov.uk](http://data.gov.uk)). But raw data does not tell you much.

An interesting recent development is _data journalism_. Data journalists tell stories using
data. A data driven article is based on an interesting observation from the data, it includes
(interactive) visualizations that illustrate the point and it often allows the reader to get
the raw data.

Adding a chart produced in, say, Excel to an article is easy, but building good interactive
visualization is much harder. Ideally, the data driven article should not be just text with
static pictures, but a _program_ that links the original data source to the visualization.
This lets readers explore how the data is used, update the content when new data is available
and change parameters of the visualization if they need to understand different aspect of the
topic.

This is in short what I'm trying to build with [The Gamma project](http://thegamma.net). If
you're interested in building better reports or data driven articles, continue reading!

> <img src="http://tomasp.net/blog/2015/thegamma/sl.png" style="float:right;width:150px" />
>
> I did a talk about The Gamma project at the fantastic [Future Programming workshop](http://www.future-programming.org/programSL.html)
> at the [StrangeLoop conference](http://www.thestrangeloop.com/) last week (thanks for inviting me!)
> and there is a [recording of my 40 minute talk on YouTube](https://www.youtube.com/watch?v=cYoO2RvZn7Y&feature=youtu.be&a),
> so if you prefer to watch videos, check it out!

Are you a data journalist or data analyst? We're looking for early partners!
I joined the <a href="http://www.joinef.com" title="Nothing to do with Entity Framework, don't worry!">EF
programme</a> to work on this and if the project sounds like something you'd like to see happen,
<a href="mailto:tomas@tomasp.net">please get in touch</a> or share your contact details
on [The Gamma page](http://thegamma.net)!

Why reporting needs innovation
------------------------------

There is a number of reasons why data journalism and reporting in general need better tools. 
When I see a report on a problem that I want to understand better, I want to be able to see how
exactly is the report created and how are the inputs pre-processed. I want to be able to modify
some of the parameters (for example, add another country to a comparison) or see what a different
view of the data would say (look at _per capita_ values rather than absolute numbers). Sadly, this
is almost never possible. What I would like to see is an environment that offers these three 
properties:

 * **Reproducibility** - The report should be fully reporoducible. Serious newspapers cite their
   data sources, but reproducing report using just a citation involves a lot of manual work. 
   It should be possible to reproduce the results and _verify their correctness_ just by rerunning
   the code behind the report.
 
 * **Transparency** - Reproducibility is a good start, but we should also be able to change the
   report easily. I want to be able to change parameters and see how that affects the result. Does
   it still support the story? I want to see that the report is _not misleading_,  intentionally 
   (or unintentionally).

 * **Interactivity** - Finally, I think we should also enable novel user experience with reports.
   Newspaper are no longer (just) printed on paper where we need text and images. We should use
   the reproducibility and transparency behind reports to allow new user experiences. The reader
   should be able to explore the data and do some of the changes without being a programmer.

I think many would agree with these points. It's just hard to make this easy and accessible enough.
 The [Data Journalism Handbook](http://datajournalismhandbook.org) describes some of the tools
that people use currently. Most non-programmers obtain data by hand and then use tools like 
Google Sprehadsheets and Excel (making reproducibility and transparency hard). Pre-processing is
done using ad hoc scripts (using Perl, R, Python etc.) that are not very general and would not work
on different inputs. And finally, the few reports that are interactive are usually single-purpose
applications (typically written in JavaScript).

How can we do better
--------------------

Modern programming languages have a number of features that make working with data _much_ easier
than before. Features like LINQ in C# (inspired by functional style) make it easy to transform data
and _type providers_ in F# integrate external data sources directly into the programming language.
If you have not seen type providers before, the following is a demo showing the [World Bank type
provider](http://fsharp.github.io/FSharp.Data/) running inside the [Ionide project](http://ionide.io/)
(inside the Atom editor):

<div style="text-align:center;">
<img src="atom.png" style="max-width:500px;margin-left:auto;margin-right:auto;" />
</div>

The type provider imports all data sources from [the World Bank](http://data.worldbank.org) directly
into the type system. You type "." and see all the countries, then you type "." again and you can
choose one from the thousands of available indicators.
 
F# type providers are fantastic if you are programmer, but I think they also illustrate the kind
of experience that we could build for anyone who is working with data. Typing "." in the editor is
not _that different_ from clicking the "Search" button in Google and choosing an option in the
Atom auto-complete is not _that different_ from choosing an option in a drop-down on a web page.

Try The Gamma prototype
----------------------

I would not be writing this blog post if I didn't have anything to show! I put together a prototype
that shows some of the ideas. It is very basic, but it demonstrates the experience that I believe
all visualizations on the web should have in the future. The example uses the [World Bank data](http://data.worldbank.org)
and compares countries in the world based on their CO2 emissions:

> This is very much work in progress. I focused on building a demo that illustrates the 
> idea, but there are certainly issues. You can [report them on GitHub](https://github.com/tpetricek/TheGamma)
> and if the whole demo is down, ping me at [@@tomaspetricek](http://twitter.com/tomaspetricek)!

<div style="position:absolute;width:100%;z-index:-100">
<div style="position:relative;left:100%;width:260px;margin-top:25px;display:none;padding:10px;border:1px solid #d0d0d0;background:#f0f0f0" id="carbon-side">
</div>
</div>
<iframe src="http://prototype.thegamma.net/carbon-plain?iframe=true" style="overflow:hidden;height:300px;width:100%;border-style:none;margin:0px" seamless="seamless" id="carbon">
</iframe>
<style type="text/css">
 #carbon-side h2 { margin-top:0px; font-size:15pt; }
 #carbon-side p { font-size:9pt; }
 #carbon-side h2, #carbon-side p { color:#404040; }
</style>
<script type="text/javascript">
  var eventMethod = window.addEventListener ? "addEventListener" : "attachEvent";
  var messageEvent = eventMethod == "attachEvent" ? "onmessage" : "message";
  window[eventMethod](messageEvent, function(e) {
    if (e.data.action == "showtip") 
      document.getElementById('carbon-side').style.display = e.data.visible?'block':'none';
    if (e.data.action == "tip") 
      document.getElementById('carbon-side').innerHTML = e.data.html;
    if (e.data.action == "resize") 
      document.getElementById('carbon').style.height = (e.data.height + 0) + 'px';
  }, false);
</script>

It looks like China is the largest producer of CO2 emissions in the world followed by the US. But
there are a few interesting questions you can ask about the data - and the interactive visualization
lets you explore those:

 - What was the situation like in 1990 or 1980? You can explore this by clicking "options" and 
   changing the year using the drop down. (When did China overtook US as the largest polluter?)
 
 - What if we compare CO2 emissions _per capita_ rather than absolute numbers? (It turns out that
   large developing countries like China and India are no longer the largest polluters...)

 - How is this actually created? There is no pre-processing in this demo, but you can still see
   all the details (like the color scheme) if you click on the "source" button.

As you can see, it is possible to embed visualization created using The Gamma into your own web page
(ping me if you are interested). The exmple is part of a larger demo that I created, which shows
other interesting aspects of the project. Check out the [full version of the CO2 emissions demo](http://thegamma.net/carbon)
on The Gamma web site. 

The third example on the page (pie chart) is a bit more interesting, because it implements a simple
pre-processing. It takes top 6 countries and shows them together with all other countries. This is
an aspect that cannot be changed in "options", but you can see it in the "source" view:

    let sumRest =
      countries.skip(6).sum()
      
    let topAndRest =
      countries.take(6)
        .append("Other countries", sumRest)
        
    chart.pie(topAndRest).show()
    
I believe this kind of transformation is easy enough and with good tooling, we can make it 
accessible not just for programmers, but even for technically skilled journalists or anyone
who works on data analytics. Feel free to experiment with the prototype, though keep in mind that
the library is very much incomplete.

Article is a literate program
-----------------------------

The key idea that I think can change how data reporting is done is to treat _articles as programs_.
In the prototype, the source code for the CO2 report is simply a [Markdown document on 
GitHub](https://github.com/tpetricek/TheGamma/blob/master/web/demos/carbon.md).

Everything else is generated from the source. When you open the report in a web browser, you 
see a rendering that shows the text with the resulting charts (the code is executed). The more
interesting thing is that when you click on "options", The Gamma looks for specific patterns in the
source code and generates editors for them. The current implementation looks for two patterns.

 1. When there is a choice from one of several members, for example years in 
    `world.byYear.2010`, we generate drop-down with other members.
 2. When the code creates a list, for example `[ countries.USA; countries.UK ]` in the
    last example in the CO2 report, we generate a select element.

This is just a very basic thing that can be done when we have the full source code and there are
many other interesting features I'd like to add. If you see a number in text, say the GDP of USA
is 16.77 trillion USD, this does not tell you much - but if we know the source of the number and
how it is computed, we can automatically provide the context. What about other countries? And how 
has it been changing?  

Summary
-------

Saying that programming is the new literacy would be a (perhaps quite silly) overstatement, but
I do think that understanding information around us is becoming increasingly important. This 
applies to public information (e.g. open government data) but also to business data inside 
companies.

Journalists and data analysts help us by finding interesting information in data and presenting
them to us. But how can we make sure that the analysis is done correctly and does not show 
misleading information? And how can we build on top of it to explore the information further
and get better understanding?

This is exactly what I'm trying to do with The Gamma project and I would like to hear from anyone
interested! Get in touch at [tomas@tomasp.net](mailto:tomas@tomasp.net) or [@@tomaspetricek](http://twitter.com/tomaspetricek).
