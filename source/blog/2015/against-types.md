Against the definition of types
===============================

 - date: 2015-05-14T15:46:36.6659544+01:00
 - description: What is the definition of type? Having a clear and precise answer to this question would avoid many misunderstandings but it would hurt science, 'hamper the growth of knowledge' and 'deflect the course of investigation into narrow channels of things already understood'.
 - layout: article
 - image: http://tomasp.net/blog/2015/against-types/paper-square.png
 - tags: philosophy,research
 - title: Against the definition of types
 - url: 2015/against-types

--------------------------------------------------------------------------------
 - standalone

> <p style="margin-bottom:5px">Science is much more 'sloppy' and 'irrational' than its methodological image.</p>
> <p style="text-align:right">Paul Feyerabend, Against Method (1975)</p>

<a href="http://tomasp.net/academic/drafts/against-types/">
<img src="http://tomasp.net/blog/2015/against-types/paper.png" style="float:right;margin:0px 0px 5px 20px;width:160px" />
</a>

Programming languages are a fascinating area because they combine computer science (and logic) with
many other disciplines including [sociology][socioplt], [human computer interaction][usab] and things
that cannot be scientifically quantified like intuition, taste and (for better or worse) politics.

When we talk about programming languages, we often treat it mainly as scientific discussion seeking
some objective truth. This is not surprising - science is surrounded by an aura of perfection and
so it is easy to think that focusing on the core scientific essence (and leaving out everything)
else is the right way of looking at programming languages.

However this leaves out many things that make programming languages interesting. I believe that one
way to fill the missing gap is to look at philosophy of science, which can help us understand how
programming language research is done and how it should be done. I wrote about the general idea
[in a blog post (and essay) last year][ppl]. Today, I want to talk about one specific topic: _What
is the meaning of types?_

This blog post is a shorter (less philosophical and more to the point) version of an essay that
I submitted to [Onward! Essays 2015][onwess]. If you want to get a quick peek at the ideas in the
essay, then continue reading here! If you want to read the full essay (or save it for later),
you can get [the full version from here][full]. 

--------------------------------------------------------------------------------


> <p style="margin-bottom:5px">Science is much more 'sloppy' and 'irrational' than its methodological image.</p>
> <p style="text-align:right">Paul Feyerabend, Against Method (1975)</p>

<a href="http://tomasp.net/academic/drafts/against-types/">
<img src="http://tomasp.net/blog/2015/against-types/paper.png" style="float:right;margin:0px 0px 5px 20px" />
</a>

Programming languages are a fascinating area because they combine computer science (and logic) with
many other disciplines including [sociology][socioplt], [human computer interaction][usab] and things
that cannot be scientifically quantified like intuition, taste and (for better or worse) politics.

When we talk about programming languages, we often treat it mainly as scientific discussion seeking
some objective truth. This is not surprising - science is surrounded by an aura of perfection and
so it is easy to think that focusing on the core scientific essence (and leaving out everything)
else is the right way of looking at programming languages.

However this leaves out many things that make programming languages interesting. I believe that one
way to fill the missing gap is to look at philosophy of science, which can help us understand how
programming language research is done and how it should be done. I wrote about the general idea
[in a blog post (and essay) last year][ppl]. Today, I want to talk about one specific topic: _What
is the meaning of types?_

This blog post is a shorter (less philosophical and more to the point) version of an essay that
I submitted to [Onward! Essays 2015][onwess]. If you want to get a quick peek at the ideas in the
essay, then continue reading here! If you want to read the full essay (or save it for later),
you can get [the full version from here][full]. 

> This article is also available in Japanese! Read it at the POSTD web site:
> [「型」の定義に挑む ](http://postd.cc/against-the-definition-of-types/)

Looking for the meaning of types
--------------------------------

My search for the meaning of types has been inspired by two things. Firstly, I talked about the
meaning of types with Stephen Kell almost a year ago and he later wrote [an interesting essay
on this subject (PDF)][sk]. Stephen highlights the difference between "logical types" as understood
by functional programmers and "data types" as understood by the engineering community. This
resonated with my belief that many people think of different things when talking about types, but
I think the spectrum is way larger.

Secondly, I was supervising [the Types course](https://www.cl.cam.ac.uk/teaching/1415/Types/), which
had the following list of reasons why types are useful. I suspect it has been copied from
previous materials for a couple of <strike>centuries</strike> years :-)

>  - Detecting errors via type-checking, statically or dynamically
>  - Abstraction and support for structuring large systems
>  - Documentation
>  - Efficiency
>  - Whole-language safety

In the past, some programming languages used types for all of the above reasons. But the world of
modern programming languages with types is _way_ more complicated. They typically still use types
for some of the above reasons, but rarely for all (and people often disagree whether a language
is "typed", just because it picks another subset of the reasons)!

For example, [Julia uses types](http://julia.readthedocs.org/en/latest/manual/types/) almost
exclusively for performance. [TypeScript](http://www.typescriptlang.org/) uses types for structuring
large systems and documentation, but not for efficiency (they are erased!) and not for whole-language
safety (they are unsound!) [F# type providers](http://tomasp.net/academic/drafts/age-of-web/)
use types mainly for tooling (with some error detection), so they are out of this list completely.

So, how can we make sense of types? First, we need to understand that the meaning of types evolves
and changes. Then, we can see how this happens (and this is where philosophy of science is useful).
Finally, philosophy of science also suggests that this is normal (and good) thing that is necessary
for science. We can still continue using types in a narrow (well-defined) sub-areas, but I believe
it is also interesting to talk about the overall (fuzzy) concept of types and, again, we can turn
to philosophy for some interesting ideas...

How the meaning of types changes?
---------------------------------

My first point is that the meaning of 'types' changes and evolves. This is clearly the case if we
look at the history and it is equally true nowadays.

### From foundations of mathematics to lambda calculus

<img src="russell.jpg" style="float:right; margin:0px 0px 10px 10px; width:160px" />

Bertrand Russell introduced _types_ in a paper _Mathematical logic as based on the theory of type_
in 1908 to avoid logical paradoxes arising from self-reference. In his system, propositions had
types. A proposition that talked about propositions of type $n$ had a type $n+1$, ruling out
self-referential propositions.

Moving closer to programming, types were added to $\lambda$-calculus by Alonzo Church in 1940.
However, this was still done in the context of logic and foundations of mathematics (even the term
_programming language_ started to be used only around 1955!) Interestingly, Church does not very
clearly motivate the introduction of types - but even in $\lambda$-calculus, they are mainly useful
to avoid paradoxes (and get the [strong normalization
property](http://en.wikipedia.org/wiki/Normalization_property_(abstract_rewriting)), i.e. all
programs reduce to a value).

### From expression types to computation types

I'll skip the early days of types that Stephen Kell writes about and I'll jump to types as they
appeared in the early ML languages. This combined the tradition from $\lambda$-calculus (where
each expression has a type) with an engineering tradition (which introduced primitive types such
as reals, integers and so on). Note that Church and Russell did not really specify what the base
types were! They only cared about how types are _constructed_.

In ML-like languages, types denoted _sets of values_ that a computation can produce. This is
probably still the dominant way of thinking about types - and much of the programming language
theory and methodology is built around this idea (when we prove things about programming
languages, we might prove that a computation of type $\tau$ produces a value $v$ such that
$v\in\tau$).

However, even the view of "types as sets of values" does not capture some of the more interesting
type systems. For example, languages with effect systems (or Haskell monads) can track what a
computation does (and my work on [coeffects captures what computations require](http://tomasp.net/blog/2014/why-coeffects-matter/)).
A computation might then have a type $\textit{int}~\&~\{ \textit{write}~\rho, \textit{read}~\sigma \}$.
This means that the computation produces an `int` value, but it also writes something to a memory
region $\rho$ and reads a value from a memory region $\sigma$ (you then know you do not need a lock
to protect $\sigma$). This clearly goes beyond the idea of "types as sets" (and one alternative is
to see types as relations).

### Unsound, relatively sound and super-sound type systems

So far, the story was mostly linear, so it might look like we are going toward some ultimate
notion of 'type'. But if we look at some of the interesting current developments, we can see
that types are developing in very different directions. Here are some of them:

 - _Super-sound type systems_ or dependently-typed languages come back to the links with logic
   and they extend the traditional "whole-language safety" property to another level. But they
   also (interestingly) change how we think about programming - given sufficiently precise type,
   you may be able to generate the program automatically.

 - _Unsound type systems_ is another direction that includes TypeScript and Dart - both use types
   mainly for documentation and structuring code (rather than for proving logical properties) and
   so they focus on simplicity and ease of use (which introduces safety holes like covariant
   generics). But this does not break language safety, because this is enforced at runtime anyway.

 - _Relatively sound systems_ is how I would label F# type providers. Type providers very much follow
   the traditional safety properties of ML, but they make them relative. They give you safety
   provided that the "world behaves nicely". For example, World Bank type provider turns countries
   into statically typed members - and it gives you safety as long as no country disappears.

I hope this convinced you that the notion of types changes and evolves. Still, I think it is useful
to treat all of these as instances of the same (vaguely defined) thing - types are [_boundary
objects_](http://en.wikipedia.org/wiki/Boundary_object) that make it possible to translate good
ideas between different areas of programming language research.

Inconsistent theories and concept stretching
--------------------------------------------

<img src="lakatos.jpg" style="float:right; margin:0px 0px 10px 10px; width:160px" />

If we look at philosophy of science, we can see that evolving and inconsistent meanings are not
as unusual as you might think (as Feyerabend's quote in the beginning says: "Science is much more
'sloppy' and 'irrational' than its methodological image.").

Two specific ideas that apply nicely to our understanding of types are _concept stretching_ and
_research programmes_, both introduced by Imre Lakatos.

### Research programmes

When we look at science as a whole, we can identify competing _research programmes_. Each research
programme has _hard core_ which specifies core assumptions that the participants of the research
programme take for granted and _auxiliary belt_ of methods and research questions that are
considered interesting. The hard cores of multiple research programmes may very well contradict
each other. In case of types, proponents of one research programme might take _type system safety_
as a fundamental property (and will refuse to call unsound systems _type systems_). Proponents
of another research programme might take _practical usability_ as a core and will frown upon
overly complex systems.

As Lakatos points out, this is a normal way in which science works - and it is not affecting you
as long as you stay within one research programme. But when crossing the boundaries, it is good
to keep the idea of research programmes in mind, because it explains why there are so many pointless
arguments about types.


### Concept stretching

Another interesting idea introduced by Lakatos is the idea of _concept stretching_. It explains how
the meaning of a concept (say, types) changes and is refined when counter-examples (or more
generally, cases that do not nicely fit with the established model) are discovered. Lakatos presents
concept stretching in the context of mathematics - we start with a perfectly reasonable definition,
discover a counter-example (of a previously unexpected kind) and it forces us to come up with a
more refined notion.

This can explain various developments in the history of types too. For example, we start with a
nice and simple definition of "types as sets of values", but then someone starts using types to
track effects of computations, which breaks our nice model. Or, we start with "type safety", but
then someone starts to use types to talk about countries of the world - and we have to relativize
the type safety property.

Lakatos also describes some common reactions to the counter-examples or unexpected cases. My
favorite is _monster-barrers_ who label the odd case as a monster and refuse to admit them into a
well-behaved society. It is not hard to find monster-barrers in discussion about unsound type
systems. Sadly, usually in less noble forms than the following:

> <p style="margin-bottom:5px">I turn aside with a shudder of horror from this lamentable plague of functions which have no derivatives.</p>
> <p style="text-align:right">Charles Hermite (1893)</p>

Against the definition of types
-------------------------------

I hope that I have now convinced you that the whole "situation with types" is a messy business.
You might expect that I'll now say that we should properly document our _research programmes_,
look for _concept stretching_ and properly classify and organize different notions of types
that we work with. Doing that might be useful part of normal scientific work, but my main
point is that we should not _require_ this law and order. I have a lot of sympathies for
Phaedrus from Pirsig's famous book who identifies Aristotle as the founder of the modern 
scientific approach and laments:

> <p style="margin-bottom:5px">
> Phaedrus saw Aristotle as tremendously satisfied with this neat little stunt of naming 
> and classifying everything. (...) he saw him as a prototype for many millions of 
> self-satisfied and truly ignorant teachers throughout the history who have smugly and 
> callously killed the creative spirit of their students with this dumb ritual of analysis, 
> this blind, rote, eternal naming of things.
> </p>
> <p style="text-align:right">Robert Pirsig, Zen and the Art of Motorcycle Maintenance</p>

<img src="feyerabend.jpg" style="float:right; margin:0px 0px 10px 10px; width:160px" />

I suspect that Phaedrus's wording might be a bit too much for many of the scientifically-inclined 
readers, but there is some truth in it. Creative uses of _types_ and other concepts often break 
some of the established rules and principles of the time and we only find a way to reconcile 
them in retrospect - and I believe some of the historical developments around types support this.

A similar position is defended by philosopher of science Paul Feyerabend. He explains how
requiring clarity and precision for novel (and interesting) ideas changes and restricts our
thinking (and as a result, hurts science):

> [T]o 'clarify' the terms of a discussion does not mean to study the additional and as yet 
> unknown properties of the domain in question which one needs to make them fully understood, 
> it means to fill them with existing notions from the entirely different domain of logic and 
> common sense, (...) and to take care that the process of filling obeys the accepted laws of logic.
>
> <p style="margin-bottom:5px">
> So the course of an investigation is deflected into the narrow channels of things already 
> understood and the possibility of fundamental conceptual discovery is significantly reduced. 
> </p>
> <p style="text-align:right">Paul Feyerabend, Against Method (1975)</p>

The above quote comes to my mind way too often when reading programming language papers about
types. Ideas that are new are often rejected because they have some "technical flaw" that can
surely "be corrected". But such correction is not advancing the idea - instead, it forces us
to frame the idea in an existing context and, very often, throw away the parts that do not 
easily fit in this context.

Feyerabend defends his position on humanitarian grounds, but also using historical examples.
If we look at the history of science, there are many examples when a major breakthrough (in
retrospect) started as an idea that could be viewed as unscientific at the time (and did not
provide better empirical predictions).

We do not need to adopt Feyerabend's radical view to argue that some level of inexactness is
a good thing. Rather than talking about science overall (as Feyerabend does), we can talk about
individual research programmes (as Lakatos does) or research paradigms (as Kuhn does). Both
Lakatos and Kuhn show that early stages of research programmes and paradigms are often 
imprecise, imperfect (and even contradict the _accepted reality_).

So a slightly weaker (but perhaps more acceptable) position is that we can require a precise
definition of types in _established research programmes_ (say, when talking about set-based
notion of types in functional languages), but we should not ask for precise definition when
talking about work that has not yet turned into a fully grown research programmes (say,
F# type providers or unsound type systems of TypeScript and Dart).

Living without well-defined types
---------------------------------

In summary, a _type_ is not a formal concept that can have a precise definition. This can be 
the case in some narrow areas and we can use the precise definition within the narrow area, 
but how can we work with types if we want to operate and think outside of a particular research 
programme?

You can read the [full version of the essay][full] to read some of the ideas that I suggest.
I do not have a full answer to the question, but there is a couple of Briefly, there is a 
couple of methods or ways of thinking from philosophy of science that do not require precise 
definitions. I believe that these provide a useful complement to the rigorous methods that 
we use when operating within a narrow rigorous areas of an established research programme. 

### Language games and how we use types

<img src="wittgenstein.jpg" style="float:right; margin:0px 0px 10px 10px; width:160px" />

One way of understanding the meaning of a term is to look how it is used. The philosopher
who is most famous for the claim that "meaning is use" is Ludwig Wittgenstein. Wittgenstein
says that we understand the meaning through _language games_, which are specific contexts in
which a word can use. For example, when you say "this is a computer", you are using the 
_ostensive definition_ language game and the listener familiar with this way of talking can
learn what a computer is.

What would be some interesting language games in the context of programming and types? One
example I can think of is the [expression problem][exprp]. The expression problem is a puzzle that
reveals some of the abstraction and error checking properties of a system. It gives a very
specific perspective of a type system (just like individual Wittgenstein's language games),
but it can tell us interesting things about a system without requiring a precise definition.
I believe that inventing other interesting puzzles of a similar kind is one thing we can
do to better understand types in a broader sense.

### Stereotypes and the meaning of types

Another direction that we can follow is the idea of _stereotypes_ introduced by Hilary 
Putnam. When people work in different research programmes, they might have different 
definition of types but, in some sense, they are still talking about the _same thing_.
Putnam's definition of _meaning_ is interesting, because it can span multiple research
programmes - a situation that could be very useful when talking about types!

According to Putnam, an important part of meaning is a _stereotype_, which is a 
description of features that are typical or normal for the entity. Those features
should constitute ways of recognizing if a thing belongs to the kind. However, 
stereotypes are not strict (you can see them more as heuristic). Or, to use Putnam's
example even though a stereotypical tiger has stripes, an albino tiger is still a tiger.
So, the open question here is - what are the stereotypical aspects of types? 
How do we recognize if something is a type system?

### Scientific entities and doing things with types

<img src="hacking.jpg" style="float:right; margin:0px 0px 10px 10px; width:160px" />

The first two philosophies focus on the meaning of types, but we can also take a practical
attitude and instead look at just _doing_ things with types without a precise definition.
Ian Hacking uses this approach. Using a number of historical examples, he shows that 
interesting work does not necessarilly need complex theory: _"there have been important 
observations in the history of science, which have included no theoretical assumptions at all."_

An interesting side-note made by Hacking is that it is the theoreticians who appear
in the history books (which distorts our view of science): _"[there is] a certain class 
difference between the theorizer and the experimenter. (...) We find prejudices in favour 
of theory, as far back as there is institutionalized science."_

Despite the prejudices against the experimentalist approach to computer science (even the 
word _engineering_ seems to have negative connotations in some circles), I believe 
that much of computer science is based on experimentalism - before the experiments are 
silently hidden behind an "upper-class" theory that is developed later. We just need to
find a way of talking about and publishing programming language experiments!

In my [earlier essay][ppl], I suggested that something like a case study could be treated 
as a programming language experiment (showing how some notion of types helped us to build 
certain kinds of systems). The [future programming workshop][fpw] which accepts submissions 
in the form of screencasts is another intriguing option. Using Hacking's example from physics 
- no matter what theory of electro-magnetism do you believe in, you can observe what 
[Faraday's motor](http://en.wikipedia.org/wiki/Homopolar_motor) does.

Summary
-------

This blog post introduces some of the key ideas [from my recent essay][full] that partly
reacts to the misunderstandings that often happen when people talk about types. To quote
the abstract of the essay (which you still can read for more ideas!)

> _"What is the definition of type?"_ Having a clear and precise answer to this question 
> would avoid many misunderstandings and prevent meaningless discussions that arise from 
> them. But having such clear and precise answer to this question would also hurt science, 
> "hamper the growth of knowledge" and "deflect the course of investigation into narrow 
> channels of things already understood".
 
Rather than seeking the elusive definition of what is a type (which does not exist), I
believe that we should look for innovative ways to think about and work with types that 
do not require an exact formal definition. This can be useful addition to the precise
research that can be done within a specific research programme.




 [onwess]: http://2015.splashcon.org/track/onward2015-essays
 [ppl]: http://tomasp.net/blog/2014/philosophy-pl/
 [full]: http://tomasp.net/academic/drafts/against-types/
 [phd]: http://tomasp.net/academic/theses/coeffects/
 [socioplt]: http://lmeyerov.github.io/projects/socioplt/viz/index.html
 [usab]: https://www.cl.cam.ac.uk/teaching/1415/P201/
 [sk]: http://www.cl.cam.ac.uk/~srk31/research/papers/kell14in-author-version.pdf
 [exprp]: http://en.wikipedia.org/wiki/Expression_problem
 [fpw]: http://www.future-programming.org/
