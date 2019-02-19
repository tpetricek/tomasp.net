What should a Software Engineering course look like?
====================================================

 - title: What should a Software Engineering course look like?
 - date: 2019-02-08T12:22:57.7521867+01:00
 - description: Is there any fundamental knowledge about software engineering that will remain
    relevant in the next 100 years? In this blog post, I discuss why teaching software engineering
    in a university environment is difficult. I also suggest how we can design a more useful and
    software engineering course that will not go out of date with the next shift in technologies
    and methodologies. The key idea is that we need to focus on the motivation behind software
    engineering and the reasoning that leads to the adoption of particular software engineering
    methods in the face of particular problems that the software industry is attempting to address.
 - layout: post
 - references: true
 - image-large: http://tomasp.net/blog/2019/software-engineering/sdi.jpg
 - tags: academic, teaching, philosophy

----------------------------------------------------------------------------------------------------

When I joined the [School of Computing](https://www.cs.kent.ac.uk/) at the [University of
Kent](https://www.kent.ac.uk/), I was asked what subjects I wanted to teach. One of the topics
I choose was _Software Engineering_. I spent quite a lot of time reading about history of
software engineering when working on my paper [on programming
errors](/academic/papers/failures/index.html) and I go to a fair number of [professional
programming conferences](http://github.com/tpetricek/Talks), so I thought I can come up
with a good way of teaching it! Yet, I was not quite sure how to go about it or even what
_software engineering_ actually means.

In this blog post, I share my thought process on deciding what to cover in my Software
Engineering module and also a rough list of topics. The introduction explaining _why_ I choose
the these and _how_ I structure them is perhaps more important than the list itself, but it
is fairly long, so if you just want to see a list, you can
[skip ahead to Section 2](http://tomasp.net/blog/2019/software-engineering/#fund) (but please read the
introduction if you want to comment on the list!) I also add a brief reflection on why I think
this is a good approach, referencing a couple of ideas from philosophy of science in [Section 3](#phil).

----------------------------------------------------------------------------------------------------

<img src="ncrafts.jpg" style="float:right;max-height:100px;margin:10px 20px 10px 30px"/>

> I will be doing a talk based on some of the ideas in this blog post at the [NewCrafts
> conference](http://ncrafts.io) on 16-17 May in Paris, a software development conference
> for professional developers who care about quality code and bettering their practices.
> If you want to chat about these ideas, NewCrafts will be a perfect opportunity!

## 1. The problem with Software Engineering

Many universities treat _software engineering_ as a course where they should give students some
directly practically useful skills that future employers ask for. Although we believe that topics
typically covered by computer science curriculum (such as algorithms, complexity analysis, logic,
but also compilers or theories of human computer interaction) are important, we also recognize that
most students will go work as software developers and companies hiring them will be very happy if
students know some of the practices, tools and methodologies that they are using.


<div style="max-width:550px;padding:10px" class="rdecor">
<img class="img-responsive" src="ibm.jpg" style="border:solid 6px black" />
</div>

Consequently, _software engineering_ becomes just a course trying to cover _whatever methods and
tools the industry needs_. This is something that universities cannot, in principle, teach
very well. The tools of choice and even popular methodologies keep changing quite quickly.
The trend of the day is no longer object-oriented modelling, but event sourcing; three-tier
architecture got replaced by microservices with tools like Kubernetes; heavyweight processes
got replaced by Agile methods and even (some aspects of) those are frequently being questioned.
Even if a university designs an up-to-date course, it will be obsolete in 5 years.

Should we try keeping up with the industry? Should we just give up and let students learn current
methods and tools on the go? Or is there something else we can do with the dreaded software
engineering course?

### 1.1 Why teach Software Engineering at all

If we see _software engineering_ just as _whatever methods and tools that the industry needs_,
then we can just say that this is not something the university needs to be teaching at all.
We teach algorithms, complexity theory and other computer science subjects, because they will
largely remain valid in 100 years. They might not be directly applicable in many programming jobs,
but at least, they capture a valid body of knowledge that we have collectively accumulated.
Programmers need to learn new methods and tools all the time, so teaching them the first
one they'll need after they finish university is not adding a huge amount of value anyway.
(We might still do this, because some universities confuse themselves with vocational training
centres, but that's another topic.)

The question is, is there something about software engineering that is capturing knowledge that
will (likely) still be relevant in 100 years?

There certainly should be _something_. Most computer science topics are rooted in more scientific
applications of computers and, later ones, in problems arising within computer science itself.
However, computers are only ubiquitous because they stopped being a tool used by scientists and
the military and became tools for business data processing throughout 1950s and 1960s. This
introduced completely new kinds of problems - around addressing business needs, team work,
processes and reliability - that professional software developers still face today.

The problem is that (academic) computer scientists found a nice fundamental concept of an
_algorithm_ and focused their efforts around this concept. In contrast, the data processing
industry never quite found good foundations for its use of computers. In a 1960 letter to editor,
an ACM member complained (quoted in [Computer Boys Take Over][compboys]):

> All of us, I am sure, have read non-ACM articles on business data processing and found them
> lacking. They suffer, I believe, from one basic fault: They fail to report fundamental
> research in the data processing field. The question of 'fundamentalness’ is all-important
> (...) [It provides] a technique for getting the field of business data processing on a firm
> theoretical footing.

I think that a good Software Engineering curriculum needs to address this issue. What are
_fundamental_ insights about software development? Computer science, with its focus on algorithms
and mathematization, found one possible answer to this question, but definitely not the only one.
Part of the reason why I'm interested in teaching Software Engineering is that I think the course
provides a nice opportunity to find other possible interpretations of what _fundamental_ might
mean...

### 1.2 Where Software Engineering comes from

<div style="max-width:600px;padding:10px" class="rdecor">
<img class="img-responsive" src="nato.jpg" style="border:solid 6px black" />
</div>

In 1960s, programming appeared as black art. Some programmers were good at it, but there was no
clear way of recognising who has the necessary skills, training new people and scaling the
production process to large system. The [NATO Conference on Software Engineering][nato] in 1968
had a goal of _"turning black art of programming into a proper engineering discipline"_ and
it also introduced the term _Software Engineering_.

The attendees of the conference was a diverse group including computer scientists, managers and
military officials. Everybody agreed on what the problem is, but there was little agreement on
how it should be addressed. The next [NATO Conference][nato] a year later is widely regarded as a
complete failure for this reason. Computer scientists advocated use of mathematics and proofs,
managers advocated factory model of organization, while military was more used to achieving
reliability via over-engineering. In the following years, the term _Software Engineering_ largely
became the name for a managerial approach to the challenge and came to cover things such as
requirements gathering, producing specification, cost and time estimation and detailed up-front
modelling of software architecture.

The challenge of turning black art of programming into a proper engineering discipline still
remains (although we might phrase it a bit differently these days). We still have not solved the
problem and new ideas and methods appear and disappear over time. Many of the ideas that are
traditionally associated with _Software Engineering_ are product of 1970s and 1980s (or 1990s,
if we're lucky). Some of those are, no doubt, interesting, but they are not very relevant in
modern software development. More recent methods that address the problem (such as Agile) do
not favour the "software engineering" term, but they certainly fall into the same general space.

The problem is, if we just replace 1970s and 1980s content with 2000s content, we are not
developing any more fundamental knowledge. We're just replacing one, already obsolete, idea
with another, soon-to-be obsolete idea.

<a name="fund"></a>

## 2. Fundamental Software Engineering knowledge

Universities need to teach Software Engineering knowledge that is fundamental in the sense that
it does not become obsolete in the next 5 years and in the sense that it captures some unchanging
principles of Software Engineering. Heavyweight software engineering methods from 1970s and 1980s
are not this, but more modern methods like Scrum and Agile programming are not this kind of
knowledge either.

What I believe _is_ unchanging is the motivation behind software engineering and the reasoning that
led to those particular methods and tools in the face of particular problems that the industry
was attempting to address. In other words, we should not teach Waterfall, UML, Scrum or TDD.
We should teach how different circumstances, problems and goals motivate these, why the various
methods are a reasonable response to the challenge (even for Waterfall!) and circumstances when
they do _not_ work.

This also gives us a good answer when we ask what practical value does a Software Engineering
course like this have. We might not cover the favourite tools and methods of the day, but we
can help students think about the motivations for and limitations of the tools and methods.
When they work for a company that follows a particular methodology, they should be able to see
recognize whether the methodology is actually suitable given the problems, motivation and other
circumstances.

I believe such _historically situated analysis_ of Software Engineering is fundamental knowledge.
It is also a valuable thing that universities can teach, because it is hard to learn on the job.
It requires a historical perspective and an additional level of reflection. It is worth noting that
what I'm advocating here is quite different from, say, fundamental knowledge that computer science
collects about algorithms. It is historical, technical and sociological rather than mathematical,
but that does not mean it is any less _fundamental_. It's just a different kind of knowledge.

### 2.1 Historically situated Software Engineering

<div style="max-width:550px;padding:10px" class="rdecor">
<img class="img-responsive" src="snowbird.jpg" style="border:solid 6px black" />
</div>

If you came here for a list of topics that I think we should teach in a software engineering
course, you finally reached the right section! You'll notice that many of the technical topics
I want to cover are, actually, topics that are covered by typical software engineering courses.
The key difference is that I think we should spend about third of the time on the actual technical
details and two thirds on the history and critical analysis.

#### Methodologies and approaches in context

Today, outdated software engineering courses teach a variant of the _waterfall_ development
methodology while more up-to-date courses teach _agile_ or _scrum_. Both of these are wrong.
Even with agile, the way it is practices differs significantly across companies and so the
particular practices an academic course might cover will not really be directly relevant.
Instead, we should teach:


* What are the motivations that led to ideas such as waterfall and agile? For waterfall, the
  context was a more general managerial culture of 1970s, rising complexity and costs of software
  and the fact that programmers were "hackers" who were hard to hire, train and replace.
  For agile, the context was faster innovation that led to changing requirements and the need
  for more rapid response. Looking at both actually teaches us something - given _any business
  context_ and _any methodology_, students should be able to understand whether the methodology is
  appropriate.

* The history of development methodologies often provides an excellent background for understanding
  them. Memorizing the [Agile manifesto][agile] will teach you a particular response to a problem
  without even teaching you what the problem was; reading [Outline of a Paradigm Change in Software
  Engineering][paradigm] by Christiane Floyd will give you the context (and, incidentally, also
  illustrate that she made a very similar point about 15 years earlier).

#### Modelling in context

Another topic that is often featured in software engineering courses is UML. I do not often meet
people who actually use UML in their work, but the general idea of somehow capturing key ideas
about a system in a more readable form is definitely worth discussing. Again, I think the particular
methods need to be discussed with an appropriate context that explains when they work.

<div style="max-width:600px;padding:10px" class="rdecor">
<img class="img-responsive" src="pattern.jpg" style="border:solid 6px black" />
</div>

* What was the context in which UML appeared and how does it compare to other methods such as
  using algebraic data types (ADTs) in functional languages? Both are response to a particular context.
  In case of UML, this was the rise of Java-like object-oriented programming and the metaphor of
  an "architect" who designs a master plan for a system. In case of ADTs, the context is treating
  programs as mathematical entities. Both of these ideas have interesting motivations, techniques
  and limitations!

* Speaking of modelling, I think that _event sourcing_ is another idea worth discussing. This is
  more recent, but it gives a really interesting complementary perspective that has very clear
  motivations (need to have a detailed log of what has happened in financial systems). I also
  think it is a good opportunity to show how the same problem can be viewed from very different
  perspectives (both valid).

* One cross-cutting concern that is all forms of modelling are dealing with is whether the model
  can be kept in sync with the actual system and, also, how detailed and precise the model is.
  Considering this from a historical perspective, we can see that this is a problem we've been
  worried about since early days of formal specifications in 1960s, through the 1980s idea of
  [inferential programming][infer] all the way to UML and functional programming. UML is quite
  interesting as it can be used as an informal whiteboard tools, but there were also many
  (largely failed) attempts to generate code from diagrams and vice versa.

#### Software architectures in context

Many software engineering courses also include the [Gang of Four Design Patterns][gof]. I suppose
those are often presented as an answer to some problems arising in software architecture, but
really, most of them are very technical and address limitations of Java-like languages. I think
there is a useful lesson to be learned from GoF patterns if we add context, but we can also
consider software architecture issues more broadly.

* The Gang of Four design patterns are a response to particular problems of Java. This is not
  because Java is bad, but because every language has things that are hard to express!
  However, GoF patterns are also just one outcome of the patterns movement, which is taking
  inspiration from architecture and this historical context gives not only a more useful idea
  of _patterns_, but also awareness of where software engineering ideas might come from.

* Talking about software architecture more generally, I think there are a few patterns that are
  sufficiently timeless to be worth mentioning - one example might be UNIX pipes, which also
  exists in the form of function composition in functional languages. This gives us the opportunity
  to show that the same idea can look very differently.

Finally, one reference that I have not yet read, but which could be other useful source of material for
discussing software architecture in context is [The Architecture of Open Source Applications][aosa] book.

#### Current hot topics

I think the core of a software engineering course should cover _fundamental principles_ that do not
go out of date and I think the critical historical reflection on different methodologies and
architectures achieves that. However, I recognise that we also need to cover some practically useful
_hot topics_ and I like teaching some of those myself! Fortunately, the _historically situated_
approach to software engineering allows that:

* The practical topics I cover this year include git, GitHub and continuous integration. These
  are useful practical skills, but in the software engineering module, we should avoid teaching them
  as cargo cults. GitHub and git can teach us interesting things about the social side of software
  and continuous integration is a response to [changes in the way software is built][ci]. Even if
  we end up building software differently in a couple of years, this will still be relevant as a
  case of response to a particular context.

### 2.2 Unchanging principles of Software Engineering

Aside from historically situated perspectives on different contexts and software engineering
approaches responding to them, there are a few more things we should teach. In computer science,
the _halting problem_ is a fundamental piece of knowledge, because it captures the limits of what
can theoretically be done. Are there similar fundamental limitations in software engineering?
Given that software engineering is an essentially human discipline, the answer will not be
mathematical. There are not many similar principles, but there are some:

<div style="max-width:500px;padding:10px" class="rdecor">
<img class="img-responsive" src="sdi.jpg" style="border:solid 6px black" />
</div>

* One principle has been described in Fred Brooks' essay [No Silver Bullet][silver]. The
  idea is that software systems face _accidental_ and _essential_ complexity. The essential
  complexity is part of their nature and cannot be eliminated while accidental complexity is
  something that, say, better programming languages can eliminate. I believe this is one key
  point about software engineering.

* Another interesting problem is where does the complexity of software systems comes from.
  David Parnas' essay [Software Aspects of Strategic Defense Systems][sds] reflects on the
  development of anti-ballistic missile software systems, but also discusses why software systems
  are complex: unlike analog systems, digital systems don't implement continuous functions.
  Unlike, say, digital CPUs, software is not formed by repeated instances of the same unit.
  Again, I think this idea gets to the core of what makes software engineering a challenge!

* Another principle documented in the [Arguments that Count][argu] book (also mentioned by
  Parnas in the context of missile defense) is that software systems develop through continual
  adaptation in a forgiving environment. This means that certain systems, such as an effective
  missile defense can be impossible to build, because they exist in a hostile environment that
  can change at a more rapid pace (e.g. it will take you longer to build a defense system than the
  Soviet Union needs to build a missile that will circumvent the defense)

### 2.3 Where Software Engineering ideas come from

Finally, I think that another topic worth our attention in a Software Engineering course is the
diversity of sources that are useful for understanding software engineering. Good software
engineering relies on broad interdisciplinary understanding and I think it's not a surprise that
interesting ideas about how software should be built borrow ideas not just from engineering, but
also from urban planning, architecture or even gardening:

* I said earlier that the motivation for the birth of "Software Engineering" in 1968 was to
  turn programming into a proper engineering discipline. But what does that actually mean?
  To answer this, we should look at documented engineering principles - one reference (which I
  have not yet read, but that comes highly recommended) is the [Toyota Way][toyota], which
  was also the source of ideas for "lean" startup and software development methodologies.

* The design patterns movement (mentioned earlier) borrows ideas from architecture. Looking at
  how this process happened, both from the historical perspective, but also analysing what
  insights we lost in the process would be another interesting lesson. I also think it points to
  a valuable source of software engineering ideas. For example, I find that [books about urban
  planning][cities] often contain inspiring ideas applicable to software!

<a name="phil"></a>
## 3. Is this still Software Engineering?

You might be wondering if what I'm advocating as _Software Engineering_ in this blog post is still
computer science, or whether I'm saying that we should replace "normal" Software Engineering course
with something more akin to history and philosophy of software engineering.

Historically, software engineering is following the tradition of the data processing industry which
has never quite became the same thing as computer science. This is also apparent in the present-day
debates whether programming job interviews should involve computer science questions (say, the $O$ notation).
If we are teaching Software Engineering, we need to expand our focus and also our methods,
because a good Software Engineering course will need to be different.

<div style="max-width:300px;padding:10px" class="rdecor">
<img class="img-responsive" src="modern.jpg" style="border:solid 6px black" />
</div>

If learning software engineering at a university taught you the same things as learning software
engineering on the job, then I think universities should not bother teaching it. (Universities are
not, or at least should not be, training centres.) However, I believe
that teaching what I called _historically situated_ software engineering does give students
something very valuable that they will not get through work experience. It provides a framework for
critical thinking about software engineering and past examples to guide this thinking.

The approach that I'm advocating is somewhat akin to Bruno Latour's position presented in the
[We Have Never Been Modern][modern] book. To quote from the [summary on Wikipedia](https://en.wikipedia.org/wiki/We_Have_Never_Been_Modern):

> [Latour] claims [that] we must rework our thinking to conceive of a "Parliament of Things"
> wherein natural phenomena, social phenomena and the discourse about them are not seen as
> separate objects to be studied by specialists, but as hybrids made and scrutinized by the
> public interaction of people, things and concepts.

I think this is a perfect summary of what I think is a good way of thinking about and also
teaching software engineering. The technological aspects of software engineering such as
source control or design patterns need to be linked with the social phenomena surrounding them,
including the business context that motivates them, but also with the critical reflection on
those entities and their contexts.

 [compboys]: https://amzn.to/2EhHq1V "Nathan Ensmenger (2010). The Computer Boys Take Over: Computers, Programmers, and the Politics of Technical Expertise"
 [nato]: http://homepages.cs.ncl.ac.uk/brian.randell/NATO/index.html "The NATO Software Engineering Conferences"
 [paradigm]: https://link.springer.com/chapter/10.1007/978-94-011-1793-7_11 "Christiane Floyd (1993). Outline of a Paradigm Change in Software Engineering"
 [agile]: https://agilemanifesto.org/ "Manifesto for Agile Software Development "
 [infer]: https://link.springer.com/chapter/10.1007/978-94-011-1793-7_6 "William Scherlis, Dana Scott (1993). First Steps Towards Inferential Programming"
 [gof]: https://amzn.to/2BHeONy "John Vlissides, Ralph Johnson, Richard Helm, Erich Gamma (1994). Design Patterns: Elements of Reusable Object-Oriented Software"
 [aosa]: https://amzn.to/2GRZMbe "Amy Brown (ed.) (2011). The Architecture Of Open Source Applications"
 [ci]: https://tpetricek.github.io/Teaching/software-engineering/collaborative.html "Tools for collaborative development (CO886)"
 [silver]: https://dl.acm.org/citation.cfm?id=26441 "Fred Brooks (1987). No Silver Bullet Essence and Accidents of Software Engineering"
 [sds]: https://dl.acm.org/citation.cfm?id=214961 "David Parnas (1985). Software Aspects of Strategic Defense Systems"
 [argu]: https://amzn.to/2GRZMbe "Rebecca Slayton (2013). Arguments that Count: Physics, Computing, and Missile Defense, 1949-2012"
 [toyota]: https://amzn.to/2Ejc5LW "Jeffrey Liker (2011). The Toyota Way to Lean Leadership: Achieving and Sustaining Excellence through Leadership Development"
 [cities]: https://amzn.to/2V5oAAz "Jane Jacobs (1961). The Death and Life of Great American Cities"
 [modern]: https://amzn.to/2EiDzBv "Bruno Latour (1993). We Have Never Been Modern"
