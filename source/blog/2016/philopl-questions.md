Philosophical questions about programming
=========================================

 - date: 2016-05-26T13:33:26.1498716+01:00
 - description: Combining philosophy and computer science might appear a bit odd, but the fact that the disciplines do not overlap might very well be the reason why putting them together is interesting - the antidisciplinary field that opens presents a number of important questions about programming language research and computer science in general!
 - layout: post
 - image: http://tomasp.net/blog/2016/philopl-questions/thinker-sq.jpg
 - tags: philosophy,programming languages
 - title: Philosophical questions about programming
 - url: 2016/philopl-questions

--------------------------------------------------------------------------------
 - standalone

Combining philosophy and computer science might appear a bit odd. The disciplines have very little
overlap. Both philosophers and computer scientists get taught formal logic at some point in their
undergraduate courses, but that's probably as close as they get.

But the fact that the disciplines do not overlap much might very well be the reason why putting them
together is interesting. In an article about [Design and Science](http://jods.mitpress.mit.edu/pub/designandscience),
Joichi Ito (from MIT Media Lab), describes the term _antidisciplinary_ and nicely
summarizes why looking at such unusual combinations is worthwhile:

> Interdisciplinary work is when people from different disciplines work together. But
> *antidisciplinary* is something very different; it's about working in spaces that
> simply do not fit into any existing academic discipline.
>
> [When focusing on disciplines, it] takes more and more effort and resources to make a
> unique contribution. While the space between and beyond the disciplines can be academically
> risky, it (...) requires fewer resources to try promising, unorthodox
> approaches; and provides the potential to have tremendous impact (...).

As you can see from some of my [earlier blog posts](http://tomasp.net/blog/tag/philosophy/),
I think the space between philosophy and computer science is an interesting area. In this article,
I'll explain why. Unlike some of the previous posts (about [miscomputation](http://tomasp.net/blog/2015/failures/index.html),
[types](http://tomasp.net/blog/2015/against-types/index.html) and [philosophy of
science](http://tomasp.net/blog/2014/philosophy-pl/index.html)), this post is quite broad and
does not go into much detail.

At the danger of sounding like a collection of random rants, I look at a number of
questions that arise when you look at computer science from the philosophical perspective,
but I won't attempt to answer them. You can see this article as a research proposal too -
and I hope to write more about some of the questions in the future.
I wish antidisciplinary work was more common and I believe looking
into such questions could have the tremendous impact that Joichi Ito mentioned.

--------------------------------------------------------------------------------


Combining philosophy and computer science might appear a bit odd. The disciplines have very little
overlap. Both philosophers and computer scientists get taught formal logic at some point in their
undergraduate courses, but that's probably as close as they get.

But the fact that the disciplines do not overlap much might very well be the reason why putting them
together is interesting. In an article about [Design and Science](http://jods.mitpress.mit.edu/pub/designandscience),
Joichi Ito (from MIT Media Lab), describes the term _antidisciplinary_ and nicely
summarizes why looking at such unusual combinations is worthwhile:

> Interdisciplinary work is when people from different disciplines work together. But
> *antidisciplinary* is something very different; it's about working in spaces that
> simply do not fit into any existing academic discipline.
>
> [When focusing on disciplines, it] takes more and more effort and resources to make a
> unique contribution. While the space between and beyond the disciplines can be academically
> risky, it (...) requires fewer resources to try promising, unorthodox
> approaches; and provides the potential to have tremendous impact (...).

As you can see from some of my [earlier blog posts](http://tomasp.net/blog/tag/philosophy/),
I think the space between philosophy and computer science is an interesting area. In this article,
I'll explain why. Unlike some of the previous posts (about [miscomputation](http://tomasp.net/blog/2015/failures/index.html),
[types](http://tomasp.net/blog/2015/against-types/index.html) and [philosophy of
science](http://tomasp.net/blog/2014/philosophy-pl/index.html)), this post is quite broad and
does not go into much detail.

At the danger of sounding like a collection of random rants, I look at a number of
questions that arise when you look at computer science from the philosophical perspective,
but I won't attempt to answer them. You can see this article as a research proposal too -
and I hope to write more about some of the questions in the future.
I wish antidisciplinary work was more common and I believe looking
into such questions could have the tremendous impact that Joichi Ito mentioned.

> **Thoughts? Comments?** This is very much a draft and I am very interested in feedback! To make
> this easier, I also [posted the article on PubPub](http://www.pubpub.org/pub/philosophy-of-programming),
> which is a nice platform for reviewing and commenting. Please share your thoughts there!

How we work
-----------

<img src="http://tomasp.net/blog/2016/philopl-questions/ghosts.jpg" style="width:150px;float:right;margin:0px 0px 15px 15px" />

I grouped the questions into three fairly general categories. The first one is mostly a
philosophical reflection on how research is done in computer science with, given my own background,
focus on programming languages research.

This is perhaps where combining philosophy and computer science could have the most direct
influence. If we take a step back and think about what is it that we are doing, perhaps we can
discover that our ordinary way of working is not the only (or the best!) option.

### How do you tell (computer) science from pseudoscience?

The question how to distinguish between science and pseudoscience is known as the
[Demarcation problem](https://en.wikipedia.org/wiki/Demarcation_problem). This is harder than it
seems - programming language research is published in academic conferences or journals, but
the criteria for this is that other scientists _think_ it is scientific. Saying that "scientific"
is what is "scientific" is not particularly helpful!

Typical programming language paper might include a performance evaluation (if it is
implementing something, like a garbage collector) or it might include simple mathematical model
of a language feature with a proof (programs in the model do not have certain bugs). Those might
be the most common ways of evaluating work in programming language research - but many other
disciplines work differently.

Perhaps more importantly, did we choose the above criteria of being scientific because we think that
this is what science should be, or did we choose them just because they are _easy to assess_?
Asking whether a programming language is more intuitive or easier to use is not "scientific" - but
is that just because our field (somehow) converged on demarcation criteria that exclude such
questions?

You could also ask differently - [medium is the message](https://en.wikipedia.org/wiki/The_medium_is_the_message)
and the fact that computer science research is published as papers changes what questions we
ask, because we only worry about questions that can be answered in the paper format. Should
we be making [screencasts and demos](http://www.future-programming.org/) or [interactive
essays](http://tomasp.net/coeffects/) instead?

### What formal models tell us about the world?

Work on programming and programming languages often involves three layers. There is some
_intuitive idea_, which is turned into _actual program_ and formal reasoning is done about
a simplified _mathematical model_. In particular, programming language research often uses
simplified models (take a look at [every other POPL paper](http://www.sigplan.org/Conferences/POPL/)
for example).

One question that is almost never asked is, what is the relationship between these three vertical
layers? We simply assume that proving properties in the mathematical model tells us
something about the (significantly more complex) program or perhaps even the original
idea - but this assumption is rarely made explicit. Is focusing on the _mathematical model_ level
leaving out many important questions about the other levels?

In philosophy of mathematics, similar problem appears in the context of informal
mathematics and proofs (see [Proofs and Refutations](http://tomasp.net/blog/2015/reading-list/)).
Informal entities allow only imprecise reasoning, while fully formal versions are precise, but
disconnected from the original entity. In computer science, we often sacrifice some of the
original intuition about problems in favor of working with formal entities that can be
treated more precisely. But do we lose something essential about the original problem by doing that?

### Unreasonable (in)effectiveness of mathematics

In a famous paper [The Unreasonable Effectiveness of Mathematics in the Natural
Sciences](https://en.wikipedia.org/wiki/The_Unreasonable_Effectiveness_of_Mathematics_in_the_Natural_Sciences),
physicist Eugene Wigner points out how mathematics often works not just as a model, but also
leads to new discoveries in natural sciences like physics. In natural sciences, this is based on
a long history of the field.

In other disciplines [such as economics](https://en.wikipedia.org/wiki/Unreasonable_ineffectiveness_of_mathematics),
the effectiveness of mathematics has been a lot less unequivocal. In computer science, we also
often take the effectiveness of mathematics as granted, but we _use_ it in a very different way than
physicists. We do not use mathematics as an _analysis tool_, but we try to use it as a _construction tool_.
And unlike physics, I'm not sure we have long enough history to support the idea that this method works well.

For example, one big difference between the use of mathematics in natural sciences and in
computer science is that physicists cannot _change the world_ to make the mathematics work.
They have to tweak the models so that they get close to how reality works. In computer science,
when we build something that follows our intuition, but does not quite work mathematically, we
can just change it so that the mathematics works. But does this take us further from the
original idea, just because we choose mathematics as our construction tool?

What we think
-------------

<img src="http://tomasp.net/blog/2016/philopl-questions/thinker.jpg" style="width:150px;float:right;margin:0px 0px 15px 15px" />

The previous three questions were mostly reflections over how we (as computer scientists)
do things. Philosophy lets us take a step back and think why that is the case and whether
this is a good way (or, at least, what would be the alternatives).

In this section, I'll take one more step back and focus more at the thinking behind
what we are doing rather than at the concrete scientific outcomes. Just like _scientific
practice_ has implicit assumptions (the "right way" of doing things), so does thinking have
its hidden assumptions.

### What we can _not_ think?

This will get a little meta - but when we think about a problem (or even when we think
what is a well-formed problem in the first place!) we rely on some broader underlying "apparatus"
that makes this thinking possible.

Our modern scientific thinking is certainly very different than the thinking of people in the
middle ages. This is not (just) because we are smarter - it is because our thoughts are based
on different foundations. In philosophy, [Michel Foucault calls this
episteme](https://en.wikipedia.org/wiki/Episteme) and in science, the concept is similar to
[Thomas Kuhn's paradigms](https://en.wikipedia.org/wiki/Paradigm).

Why this matters? The interesting question is whether there are some things that we
_cannot even think_ because they do not fit with our episteme. In computer science, our focus
on proofs, measurements and other forms of evaluation might be arising from a single episteme.
Would it be possible to think about problems differently? Perhaps in a way that would give
more space to linking the three vertical layers (ideas, implementation, mathematics) and less
space to moving horizontally (translating abstractions between different mathematical models
or comparing language features)?

### How we invent abstractions?

One of the core ideas in computer science is _abstraction_. The idea that we can find common
patterns that are more general than a concrete structure, yet capture all its important properties.
(This thinking is very likely part of our _episteme_!) But one question that is almost never asked
is where do these abstractions come from? Are we just looking for structural patterns, or do
abstractions arise from some intuitive ideas?

Many common abstractions arise from intuitive metaphors. For example, in the [First Draft of a
Report on the EDVAC](https://en.wikipedia.org/wiki/First_Draft_of_a_Report_on_the_EDVAC), John
von Neumann described modern computer architecture. Rather than calling the individual units
_components_ or _units_, he called them _organs_. You can see a biological metaphor here, right
at the foundations of modern computing! Similarly, how did we end up calling programming languages
_languages_? It is yet another metaphor - faithful in some respects and lacking in others.

If we want to understand abstractions, should we be _mathematicians_, or should we instead
become _literary critics_? And isn't category theory just a source of mathematical metaphors?

### Is computer science discovered or invented?

The question whether mathematics is invented or discovered is a fundamental topic in [philosophy
of mathematics](https://en.wikipedia.org/wiki/Philosophy_of_mathematics) (and there are several
other positions too). Are numbers abstract entities that exist independently of humans that we
discover? Or are they just things we constructed based on our environments?

You might think this does not have many practical consequences, but
I just had to put this question on the list after seeing Phil Wadler's talk
[Propositions as Types](https://www.youtube.com/watch?v=aeRVdYN6fE8). In the talk, he uses the
idea that some mathematical objects (lambda calculus, in particular) are _discovered_ while other
programming models are _invented_ to hint that the former are better.

The position in the talk is a bit unusual in that it mixes both philosophical positions on
mathematics into a single one (how exactly do we tell which programming model is invented and which
is discovered?), but nevertheless, it is an interesting idea that combines computer science
and philosophy. And if there was a sound philosophical argument for demarcating between two kinds
of objects, should we treat one kind as better than the other?

Historical reflections
----------------------

<img src="http://tomasp.net/blog/2016/philopl-questions/balzac.jpg" style="width:150px;float:right;margin:0px 0px 15px 15px" />

In philosophy of science, many of the arguments about the success of particular scientific methods
are based on the history. By looking at the long history of natural sciences, we can understand
what makes the scientific method so effective and try to replicate the method in other disciplines.
Although the history of computer science is not very long, there is certainly enough interesting
material there that we can examine when searching for answers to some of the questions above.

### How we misinterpret the history?

One thing makes looking at history very difficult. When we try to analyze history through our
modern perspective, it is easy to see it through the modern eyes and forget about the original
context in which the work was done.

For example, when we talk about Ada Lovelace as the first programmer, we are using a term that not
only did not _exist_ in 19th century but, in fact, had a very different meaning even in 1950s when
first electronic computers were created! However, precisely because programming did not exist back
then, Ada Lovelace's work and thoughts were even more interesting! And if we see her in the
context of 19th century, it might be even more fascinating than through the modern perspective of
a "first programmer". ([Science of Operations](http://tomasp.net/blog/2015/reading-list/) has a
great chapter on Babbage and Lovelace.)

The other important point when looking at history is that _our work is in the hands of its later
users_ (see [Science in Action](http://tomasp.net/blog/2015/reading-list/)). When you read a
foundational paper, it is important because of the work that _builds on top of it_ which often
takes the idea in another direction - possibly quite different than what the author intended.
What would we learn about our discipline if we approached its history with respect to the
original context, rather than reinterpreting it through the modern perspective (using what
is known as [Whig interpretation](https://en.wikipedia.org/wiki/Whig_history#In_the_history_of_science))?

### How paradigms shape our thinking?

History can also teach us about hidden assumptions in our thinking. Scientists (including computer
scientists) often do not question _all_ assumptions in their work. When something does not work
as expected, we blame _auxiliary assumptions_ rather than the _hard core_ of the research. This
is the idea behind [Research programmes](https://en.wikipedia.org/wiki/Research_program) as described
by Imre Lakatos.

In computer science, one example is the _Algol research programme_ (again from [Science of
Operations](http://tomasp.net/blog/2015/reading-list/)). While the Algol language never
got popular in practice, it defined a hugely influential set of core assumptions for academic
programming research - the idea of using mathematical logic for ensuring program correctness
can be traced back to Algol.

Seeing the "correctness through logic" idea as a core assumption of a particular research programme
explains why this is rarely questioned in programming language research, but it also makes it
easier to see alternative perspectives. For example, many of the [Future Programming
Workshop](http://www.future-programming.org/) demos follow a different research programme. They
are not just early (not yet formalized) works - they are works that may never _need_ to be
formalized.

Conclusions
-----------

As a scientific discipline, computer science often paints the picture that it is gradually
progressing towards some ideal goal - perfect programming language, provably correct programs
and so on. Academic papers support this illusion of continuity by building on previous work,
even when they completely change the direction of what the original author intended.

I believe that using the _antidisciplinary_ method and combining computer science and philosophy
is a great way to bring more innovative ideas to the discipline - be it programming language
research or other areas.

A little background in philosophy lets us understand that things are not always as simple as they
seem. It is not that easy to distinguish between good and bad science; we often build models, but
rarely try to understand how and what they tell us about the world. We use certain methods without
worrying about the consequences they might have - both on the results we get, but also on
_questions we can ask_.

This article certainly does not aim to tell you that everything we are doing is wrong. That is
not at all what I believe. But I hope to inspire the readers to think about what we do in a
slightly different, more reflective, philosophical way. Reading [a great book on history of computing
or philosophy of science](http://tomasp.net/blog/2015/reading-list/) is a great way to get started.

> **Thoughts? Comments?** As mentioned at the beginning I am very interested in feedback! If you
> have related ideas or comments related to this article, please [add them to the PubPub
> version of this article](http://www.pubpub.org/pub/philosophy-of-programming)!
