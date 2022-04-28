No-code, no thought? Substrates for simple programming for all
==============================================================

 - title: No-code, no thought? Substrates for simple programming for all
 - date: 2022-04-28T10:37:00.4275767+01:00
 - description: Is it really possible to eliminate programming load? What would real
    progress on making programming easier for all mean? In this article, I take a critical look
    at no-code programming platforms using the technical dimensions framework and the idea
    of a "programming substrate".
 - layout: article
 - icon: fa fa-magic
 - image-large: http://tomasp.net/blog/2022/no-code-substrates/card.png
 - tags: academic, research, programming languages
 - references: true

----------------------------------------------------------------------------------------------------

<div class="rdecor" style="text-align:center">
<img src="http://tomasp.net/blog/2022/no-code-substrates/flow-matic.png" style="max-width:350px" />
<p style="max-width:350px"><strong>Figure 1.</strong> Virtually eliminates your coding load. FLOW-MATIC promotional
brochure (1957)</p>
</div>

No-code is a hot new topic for programming startups. The idea is to develop a system
that allows end-users to do the programming they need without the difficult task of writing
code. There are no-code systems for building mobile apps, analysing data and many more.

It is perhaps not a surprise that "eliminating programming load" is not as new idea as some
people may think and there is an excellent blog series on [no-code history by Instadeq][history],
going back to 1959.

Funnily enough, the 1957 [promotional brochure about FLOW-MATIC][flowmatic],
a predecessor to COBOL created by Grace Hopper, uses almost the same language that you will
find in startup pitch decks today (Figure 1). Of course, in 1957, coding referred to the
tedious process of transcribing the desired program to low-level assembler or (more often)
directly to machine code and "virtually eliminating your coding load" meant having a symbolic
high-level programming language so easy that a reasonably skilled mathematician would be able
to use it.

So, is there really anything new about no-code systems? Is it really possible to "eliminate
your programming load"? And what would it really take to make some real progress in that direction?

----------------------------------------------------------------------------------------------------

## 1. Substrates for interaction

If we look beyond buzzwords, what can no-code platforms really attempt to do? To understand this,
let me introduce the idea of _programming substrate_. The idea is that if you have any sort of
software system, you can interact with it at multiple levels through multiple different
substrates. If you are _building_ a software, you will interact with it through a programming
language by modifying its source code. If you are _using_ a software, you will interact with it
through some (structured) graphical interface. All of those - code in a language and interactions
with a graphical interface - are interactions with different (programming) substrates.

<div class="rdecor" style="text-align:center">
<img src="substrate-accounting.png" style="max-width:520px;width:100%" />
<p style="max-width:520px"><strong>Figure 2.</strong> A hypothetical accounting system with substrates for
development (source code), configuration (graphical or textual) and use (user interface).</p>
</div>

The idea of _programming substrates_ is generalizing the usual distinction between programming a
software system and using a software system. This is useful, because there are often more than
two different substrates. It is also useful because we can analyze the different substrates in terms
of what kinds of change to the software they allow and what is the difficulty of using them.

For example, Figure 2 illustrates different substrates that a hypothetical software for accounting
might have. As a user, you interact ("use") with some graphical interface. This is easy, but it
only allows limited _"change"_ - you can enter your invoices and get the tax return forms!
A more advanced user can also edit system configuration. This may also be graphical or a textual
config file. This is harder, but it lets you modify, for example, how the system calculates taxes.
Finally, you (or the company that develops the system) can modify the code of the system itself.
This is very hard, but it allows changing it in arbitrary ways. You may need this if the government
invents a new kind of tax calculation that nobody expected and so it cannot be added just through
configuration. This is a pretty obvious example, but it shows some important aspects of programming
substrates:

<div class="rdecor" style="text-align:center">
<img src="substrate-excel.png" style="max-width:520px;width:100%"/>
<p style="max-width:520px"><strong>Figure 3.</strong> Excel has substrates for entering data (tables), writing
computations (equations), scripting (VBA macros) and source code for modifying the system
itself, accessible only to Excel developers.</p>
</div>

 * If there are multiple substrates, sooner or later, users who are familiar with one will hit
   its limits and will need to invest into learning the next more powerful substrate. This may be
   a large (or even unbridgeable) gap.

 * If you need to change system in complex ways, you will need a substrate that is sufficiently
   expressive. In other words, the right bottom corner in the diagram will always be empty. This
   is a point made by Fred Brooks in his [No Silver Bullet][manmonth] essay -
   no matter how much we reduce the _accidental complexity_ caused by our poor tools, the
   _essential complexity_ of the problem remains.

A system with a more interesting structure of substrates is Excel (Figure 3). Note that I separated
data entry from writing equations, because many people use it for just recording data. Thanks to
macros, Excel has a more expressive substrate that allows greater change, but is also more complex.
This also means there is a more notable gap - moving from writing equations to VBA scripting has a
non-trivial learning curve.

## 2. Challenges for no-code

I have two simple programming problems that I worked on recently that would, I imagine, ideally
be solvable by a suitable no-code framework. Looking at these illustrates some of the challenges
that no-code systems face.

### 2.1. Case study: Managing Year in Computing admissions

During the pandemic, I was responsible for managing [Year in Computing](https://www.kent.ac.uk/computing/about/year-in-computing)
admissions in Kent. Students applied through Google Form, we checked their past studies in an
internal system, scheduled Zoom interview with them, sent them an email invitation and a result
after the interview. I also occasionally needed to get some report on how many students were
offered a place, accepted our offer etc.

Of course, I ended up writing an F# script (and a bit of JavaScript) to partly automate this,
but it would be a good case study for no-code, because it is mostly just an integration of existing
services (Google docs, email, Zoom). Many no-code automation tools provide integration with those.
But there are two problems.

* The first is that this may not scale. In March 2020, Zoom was still
  newish and the API was changing, so the no-code platform developers would have to keep up with that
  to make sure the integration works and exposes all that is needed.

* The second problem are the exceptions. In my case, I had to get student marks from an internal
  system, which was obscure (and was replaced the following year). I had a JavaScript hack to
  extract the data that I could paste into a browser JavaScript console, which mostly worked.
  But this was not a simple task of extracting numbers from a table - there are non-numerical marks
  (Pass/Fail), repeated years, retaken exams and other exceptions.

### 2.2. Case study: Scraping charitable giving data

Another project I was involved with was scraping data from crowdfunding platforms,
which we started to do during the pandemic and which resulted in a [joint social policy paper][covid] (Figure 5).

<div class="rdecor" style="text-align:center">
<img src="fundraisers.png" style="max-width:520px;width:100%"/>
<p style="max-width:520px"><strong>Figure 5.</strong> Charts showing number of fundraisers and amount
donated to "foodbanks", following the Covid-19 outbreak. There was a major peak during the first
wave, but no significant increase for other waves (with the exception of Christmas period).</p>
</div>

This is similarly conceptually simple task. You just need to search for various terms on the
platform, iterate over the results and fetch data about fundraiser details, dates and (when possible)
individual donations. In reality, it is not as easy:

* Some of the services just return HTML, but one loads all data dynamically through GraphQL,
  so you either need to work at a browser level (simulating clicks) which is slow, or extract data
  from different structures (funny HTML, funny JSON).

* As always, there are unexpected changes. After about a year of running my script, one of the platforms
  renamed some CSS classes; after a few more months, one page was collecting donations in Euros
  (one in Dollars appeared later too). Two years after I started doing this, one page reported
  raised amount in Portuguese. One government service was shut-down halfway through my project too.

The question then is, what kind of programming substrates do we need if we want to be
able to solve the above problems, as easily as possible (with or without code)?

## 3. Technical dimensions of no-code systems

We know how to talk about programming languages and their features, but talking about
possibly visual, interactive, stateful programming systems has been a challenge. To make this
easier, Joel Jakubovic, Jonathan Edwards and myself recently came up with the
[Technical dimensions of programming systems][techdims] framework. This is a qualitative
framework that defines a number of _dimensions_ that can be used to describe programming
systems. For example, the "feedback loops" dimension describes how a system provides
feedback to the developer or a user (in Excel, there is, for example, a live loop when editing
values in a sheet or a less immediate error checking when editing equations or VBA scripts).

Several technical dimensions are highly relevant for no-code systems:

* **Notational structure** - _What are the different notations involved in programming and using
  the system?_ In conventional programming systems, there is a primary notation - the programming
  language in which code is written - alongside with various secondary notations.

  In no-code systems, the primary notation is typically a visual language or a user interface.
  This may be complemented by some small DSL, e.g., for specifying conditions, and eventually
  also a programming language (exposed or not), e.g., for writing extensions for integrating with
  other platforms.

* **Self-sustainability** - _To what extent can the behaviour of a system be changed within itself?_
  In Java or C#, this is limited to reflection, but Smalltalk or Lisp Machines make it possible to
  modify the development environment from within itself.

  The self-sustainability of no-code systems is typically very limited. They are closed-source and
  largely written in another programming language and can be only used to build applications in
  a particular domain. Even integration for other services has to be done outside of the
  system.

* **Degrees of automation** - _What part of program logic does not need to be explicitly specified?_
  In C# or Java, the only thing that is done automatic is garbage collection, but you can go
  further. For example, Prolog and SQL automate evaluation and you need more declarative programs.

  This is perhaps one way where no-code systems can meaningfully try to do something clever.
  Some of the exceptions in my above examples could probably be handled by a [programming-by-example][pbe]
  tool like FlashFill in Excel.

You should check out our [Technical dimensions][techdims] framework for more ideas, but the three
dimensions above are a good structure for three thoughts I have on no-code programming platforms.

### 3.1. Notational structure and HyperCard

HyperCard is not a no-code platform, because it does let people write code in HyperTalk
(and besides, it was created some 30 years before no-code became a buzzword), but it has been
amazingly successful at allowing non-experts create programs. It also has a very clever way of
structuring its programming substrate. In HyperCard, you design your cards in a WYSIWYG editor
and you can attach behaviour to elements. You can specify that a button links to another card
without coding, but more advanced interactions require HyperTalk.

There are still two substrates. The software itself is written in another language (it is not
self-sustainable, except for its documentation which is a HyperCard stack) and there is the
visual HyperCard environment itself. However, the visual environment is structured in terms
of several _user levels_ (Figure 6) that gradually unlock more advanced features of the system.
These add further notations (like HyperTalk code), but they still live within a single substrate.

<div class="wdecor" style="text-align:center">
<div class="wfig">
  <img src="hypercard.png" style="width:100%"/>
  <p><strong>Figure 6.</strong> Choosing a user level in HyperCard. Each level allows
    the user to access more advanced editing or programming features.</p>
</div>
<div class="wfig">
  <img src="substrate-hypercard.png" style="width:100%"/>
  <p><strong>Figure 7.</strong> Different user levels have different notations, but
    are accessible from a single substrate - the HyperCard user interface.</p>
</div>
</div>

The nice aspect of this is that it makes the learning process gradual. As illustrated in Figure 7,
you start by browsing, move to editing text and design, then unlock capabilities for adding
new cards and only then move to writing code. This somewhat reduces the gap (as, e.g., between
the Excel substrates) that you need to bridge if you need more complex change, because you are
staying in the same substrate, albeit you get access to more complex features it provides. It
also means you do not get overwhelmed by all that is available, as more advanced features are
initially hidden. In the programming languages world, Racket follows the same model with its
[How to design programs teaching languages][htdp].

### 3.2. Self-sustainability and open systems

Despite the interesting notational structure, HyperCard is not _self-sustainable_ meaning that
it cannot be modified from itself. It is an application (apparently written in Apple Pascal!)
and if you want to change it, you have to modify its source code.

How is this relevant for no-code platforms? The more self-sustainable a system is, the
more you will be able to do from within itself. This may not matter much when you are
just integrating a couple of services. But as my case studies illustrated, there are always
exceptions and cases the platform developers couldn't have thought of. If the system can be
modified, it makes it possible for a user to gradually become programmer. (And also to develop
a shared [open ecology of function][nexus] that takes care of the scaling issue I mentioned
earlier.)

The idea that the user can gradually become programmer exists, for example, in Smalltalk (from
[User-oriented Descriptions of Smalltalk Systems][byte]):

> [T]the new user of a Smalltalk system is likely to begin by using its ready-made application
> systems [...]. After a while, he may become curious as to how his system works. He should
> then be able to "open up" the application object [...] The next thing the user might want
> to do is to build new systems similar to the one has been using.
> Finally, the expert user will want to make his own ["kits" for building such systems].

The substrate for a fully self-sustainable system needs to allow making both small and large
changes. As discussed earlier, small changes can be easy, but large changes will inevitably
be hard to do. This is illustrated in Figure 8, which shows the possible range of programming
substrates and Figure 9, which shows what we would ideally like to get.

<div class="wdecor" style="text-align:center">
<div class="wfig">
  <img src="substrate-possible.png" style="width:100%"/>
  <p><strong>Figure 8.</strong> Programming substrates can make small changes easy, but large changes will inevitably remain hard.</p>
</div>
<div class="wfig">
  <img src="substrate-ideal.png" style="width:100%"/>
  <p><strong>Figure 9.</strong> Ideal substrate makes sure that making a small change is easy rather than hard.</p>
</div>
</div>

The issue with self-sustainable systems is that they are generally quite hard to use. In
Smalltalk or Lisp Machines, you have to master a real programming language before you can
do pretty much anything. In other words, the programming substrate is in the upper region
of my chart and making even a small change is hard. The point of no-code platforms is to make
smaller (to medium) changes easy. A perfect system would have a programming substrate shown in
Figure 9. This would make small changes easy by design, but allow gradual progression to
more complex ways of working that are necessary for larger changes. (I have no idea how to
do this, but HyperCard seems like an interesting option!)

### 3.3. Limits of automation

Systems like FLOW-MATIC that I opened this post with were called "automatic programming systems"
in the 1950s and 1960s. They eventually turned into programming languages, but the hope
at the time was that you could gradually automate programming to a greater and greater degree.
Today, this is again a hope of many who are trying to make programming "easier", especially
with the idea that machine learning can magically do our work.

There are certainly areas where this has worked well, like programming by example in the
case of Excel FlashFill. David Canfield Smith, who created the [visual Pygmalion programming
environment][pyg] in 1975 is one of those who recognized the danger of this approach:

> [T]here is a danger in [automatic programming], if carried too far. By making the computer
> into a "black box" that does the actual programming, the user has to think less about the
> logical structure of the problem. [...]
>
> If successful, automatic programming systems will replace some fairly high-level thinking
> processes in humans. Instead of encouraging humans to do more and better thinking,
> automatic programming may encourage humans to do less and poorer thinking.

Pygmalion was an interesting system, because it tried to make programming simpler, but without
automating it. The approach - now known as "programming by demonstration" was to instruct
the computer how to complete task by showing the necessary steps that the computer would then
mechanically repeat.

## 4. Research directions for no-code

I introduced the idea of programming substrates and talked about our work on technical dimensions,
because these provide useful framework for thinking about the main question of this post -
what would it take to make some real progress towards making programming easer?

Now, I have not looked at every single no-code platform out there. There are certainly
many interesting and innovative ideas in the ones I looked at and I'm sure there are more in
those I do not know. But I also think that there are some fundamental limits.

* A system that is not self-sustainable will inevitably, sooner or later, make it impossible
  for the user to do something they want to do. (The horror stories I sometimes hear about some
  elaborate Excel uses in the finance sector are a proof of that.)

* Automation can only help to a certain degree. Ultimately, programming is thinking about the
  problem we are solving and trying to eliminate that would be a move in a wrong direction.
  Finding ways to encourage clear high-level thinking (possibly using visual metaphor as attempted
  by [Pygmalion][pyg] or [Boxer][boxer]) is more interesting approach!

To conclude, I think the big open question is how to create a programming substrate that
is simple enough for small changes, but embodies the potential for large changes.

To a limited extent, HyperCard does this well by having all authoring tools embedded in an environment
that looks the same way as the running program. This makes it easy for users to gradually discover
more advanced capabilities of the system. Could something like this be more visual and more
self-sustainable?

I also think the issue with "code" is quite different than what people usually think. It is not
that "code" is somehow inherently complicated. People find it _scary_ but if they actually try
it (and get decent user experience), they are able to work with code (at least, that was my
conclusion from some empirical experiments I did using [The Gamma](https://thegamma.net/)).
The issue is that it is very difficult to connect code to what we actually see happening on
the screen. And this is a problem regardless of whether the code is textual or uses some visual
programming language.

#### Acknowledgements

Some of the ideas in this post are [from a critique][crit] of a paper [Anatomy of interaction][anatomy]
by Antranig Basman, Philip Tchernavskij, Simon Bates and Michel Beaudouin-Lafon from Salon des
Refusés workshop. Discussions with Antranig, Philip and also Jonathan Edwards and Joel Jakubovic
contributed to the ideas here (but they would probably disagree with many things I'm writing!)
Parts about Pygmalion and Smalltalk were also influenced by ongoing work on notations as part of the
[PROGRAMme](https://programme.hypotheses.org/) project, especially with Liesbeth De Mol.

<style type="text/css">
.wfig {
  width:45%;
  display:inline-block;
  margin:0px 2% 0px 2%
}
@media only screen and (max-width: 600px) {
  .wfig { width:94%; }
}

</style>

 [history]: https://instadeq.com/blog/categories/history/ "No-code history - Instadeq Blog - No-code Data Analysis & Interactive Visualizations"
 [flowmatic]: https://www.computerhistory.org/collections/catalog/102646140 "Introducing a New Language for Automatic Programming Univac Flow-Matic"
 [manmonth]: https://amzn.to/38tvyLO "Fred Brooks - The Mythical Man-Month: Essays on Software Engineering"
 [covid]: http://tomasp.net/academic/papers/covid-data/ "Peter Taylor-Gooby, Tomas Petricek and Jack Cunliffe - Covid-19, Charitable Giving and Collectivism a data-harvesting approach. Journal of Social Policy, 2021"
 [techdims]: https://raw.githubusercontent.com/jdjakub/papers/master/prog-2022/prog22-master.pdf "Joel Jakubovic, Jonathan Edwards, Tomas Petricek - Technical Dimensions of Programming Systems. Submitted, 2022"
 [pbe]: https://cacm.acm.org/magazines/2012/8/153800-spreadsheet-data-manipulation-using-examples/fulltext "Sumit Gulwani, William R. Harris, Rishabh Singh - Spreadsheet Data Manipulation Using Examples. Communications of ACM, 2012"
 [htdp]: https://docs.racket-lang.org/drracket/htdp-langs.html "How to Design Programs Teaching Languages - Racket"
 [nexus]: https://www.shift-society.org/salon/papers/2017/revised/externalization.pdf "Colin Clark and Antranig Basman - Tracing a Paradigm for Externalization: Avatars and the GPII Nexus"
 [byte]: https://archive.org/details/byte-magazine-1981-08 "User-oriented Descriptions of Smalltalk Systems - Byte, August 1981"
 [pyg]: http://worrydream.com/refs/Smith%20-%20Pygmalion.pdf "PYGMALION: A Creative Programming Environment - David Canfield Smith"
 [boxer]: https://web.media.mit.edu/~mres/papers/boxer.pdf "BOXER: A Reconstructible Computational Medium - Andrea di Sessa, Harold Abelson"
 [crit]: https://www.shift-society.org/salon/papers/2018/critiques/critique-anatomy-of-interaction.pdf "Critique of ‘An Anatomy of Interaction’ - Tomas Petricek"
 [anatomy]: https://hal.archives-ouvertes.fr/hal-01854418/document "An anatomy of interaction: co-occurrences and entanglements - Antranig Basman, Philip Tchernavskij, Simon Bates, Michel Beaudouin-Lafon"
