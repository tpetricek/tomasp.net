Thinking the unthinkable: What we cannot think in programming
=============================================================

 - date: 2016-10-11T11:30:37.3906045-05:00
 - description: Our thinking is shaped by basic assumptions that we rarely question. 
     As described by philosophers of science, research paradigms determine how scientists 
     approach problems and what questions are accepted as valid scientific theories.
     In this article, I try to discover some of the hidden assumptions in the area of programming 
     research. What are assumptions that we never question and what might the world look like 
     if we based our design method on different basic principles?
 - layout: post
 - image: http://tomasp.net/blog/2016/thinking-unthinkable/bufo.jpg
 - tags: philosophy,programming languages
 
----------------------------------------------------------------------------------------------------

> _<i class="fa fa-file-pdf-o" style="font-size:110%;margin:0px 5px 0px 0px"></i> 
> This blog post is an edited and more accessible version of an article [Thinking the
> unthinkable](http://tomasp.net/academic/drafts/unthinkable/) that I recently presented at the PPIG 2016 
> conference. The [original article](http://tomasp.net/academic/drafts/unthinkable/unthinkable-ppig.pdf)
> (PDF) has proper references and more details; the [minimalistic talk 
> slides](http://tpetricek.github.io/Talks/2016/unthinkable/ppig/) give a quick summary of the article._
>
> _<i class="fa fa-hand-o-right" style="font-size:110%;margin:0px 5px 0px 0px"></i>
> If you find this interesting, you might also be interested in a [workshop we are 
> planning](https://github.com/tpetricek/anarchy-workshop/blob/master/proposal.md). To keep in 
> touch leave a [comment on GitHub](https://github.com/tpetricek/anarchy-workshop/issues/3), ping me
> at [@tomaspetricek](http://twitter.com/tomaspetricek) or email [tomas@tomasp.net](mailto:tomas@tomasp.net)!_

<div class="rdecor">
<img src="http://tomasp.net/blog/2016/thinking-unthinkable/bufo.jpg" />
</div>

Our thinking is shaped by basic assumptions that we rarely question. These assumptions exist at
different scales. Foucault's _episteme_ describes basic assumptions of an epoch (such as Renaissance);
Kuhn's _research paradigms_ determine how scientists of a given discipline approach problems and 
Lakatos' _research programmes_ provide undisputable assumptions followed by a group of scientists.

In this article, I try to discover some of the hidden assumptions in the area of programming 
language research. What are assumptions that we never question and that determine how programming 
languages are designed? And what might the world look like if we based our design method on 
different basic principles?

----------------------------------------------------------------------------------------------------

What we cannot understand 
-------------------------

The naive view of science is that it follows the _scientific method_ in order to accumulate perfect
infallible body of _sound knowledge_. This idealized view has been challenged by many philosophers.
One of the most significant issues is _incommensurability_ – the idea that two theories may not 
share a common basis that would allow evaluating them using a common metric. Incommensurable 
theories do not only assume different things, but they ask incompatible questions. 

### Episteme and human knowledge 
Michel Foucault's concept of _episteme_ captures an incommensurability at the most fundamental level 
of human knowledge. An episteme defines the assumptions that make human knowledge possible in a 
particular epoch. It provides the apparatus for separating what _may_ from what _may not_ be considered 
knowledge.

Foucault gives an example of the incommensurability between the earlier episteme of the Renaissance 
and the later episteme of 17th-18th century. The former is characterized by 
signs and resemblances while the latter is characterized by ordering and categorization. Natural 
historian Comte de Buffon (18th century) refers to the work of Ulisse Aldrovandi (16th century):

> Buffon was to express astonishment at finding in the work of a naturalist like Aldrovandi such 
> an inextricable mixture of exact descriptions, reported quotations, fables without commentary, 
> remarks dealing indifferently with an animal's anatomy, its use in heraldry, its habitat, its 
> mythological values or the uses to which it could be put in medicine or magic.

Aldrovandi's books contain precise descriptions of snakes (admired by de Buffon for their accuracy)
and dragons (I picked a few as illustrations for this article). As Foucault explains,
Aldrovandi's report was not incorrect. It linked things in accordance with the system of signs and 
resemblances, which was _incommensurable_ with the system of ordering and categorization that was 
assumed by Buffon. 

In Aldrovandi's world, snakes that you can see and describe were as real and important as dragons 
that you heard about. The color of a snake's skin mattered just as much as the magical powers of 
a dragon. This was hard to imagine for de Buffon (and is even harder for the modern reader), but
it is vivid example of the effect that grounding in a different episteme gives.

### Paradigms and research programmes

Foucault's episteme considers the whole of human knowledge and may appear too remote for talking
about programming, but related concepts in philosophy of science capture common assumptions in a 
given scientific discipline or among a sub-group of scientists.

According to Kuhn, normal science is governed by a single _research paradigm_. The paradigm sets 
standard for sensible scientific work. A paradigm is not explicitly defined. It is formed by the 
methods and assumptions that a scientist learns during her training. When a paradigm can no longer 
solve ordinary scientific problems (puzzles), it gets replaced (over the course of a generation) by 
a new one that is incommensurable with the old one. As with episteme, the new paradigm asks 
different questions and so much of the old science loses meaning. 

At a smaller scale, shared assumptions and methods used by a group of scientists have been captured 
by Lakatos as _research programmes_. A research programme recognizes that, even in regular science, 
some laws and principles are more basic than others. When a scientist expects something to work - 
and it does not - she will not blame the core assumptions, but instead addresses the issue by 
modifying some of the additional assumptions. (Did we not detect Higg's boson? That's not because
it does not exist, but because our Large Hadron Collider is not large enough!) Due to the different 
core assumptions, the work arising from different research programmes is, again, to 
some degree incommensurable. 

<div class="wdecor">
<img src="http://tomasp.net/blog/2016/thinking-unthinkable/icon.jpg" style="max-width:100%" />
</div>

Episteme and paradigms in programming
-------------------------------------

Do episteme, paradigms and research programmes affect how programming language research is done? 
As with any other science, the answer is yes. For example, programming language research heavily 
relies on mathematical methods. As a programming language researcher, you can ask whether a 
mathematical model of a language has certain properties (is a type system _sound_?), but you'd be 
treated as lunatic if you (non-jokingly) asked if a certain language feature is _beautiful_. The
accepted paradigm says that this is not a sensible scientific question. 

### Mathematization of computer science

The history of computer science suggests that the mathematical focus was not the only possibility. 
Early programmers were often seen as anything from clerical workers to chess players and artists. 
In fact, early study noted that majoring in mathematics was not found to be significantly related 
to performance as a programmer. The focus on mathematics was, however, a good move for early 
academic computer science:

> The rise of theoretical computer science was anything but inevitable. (...) Advocates of 
> theoretical computer science pursued a strategy that served them well within the university, 
> but increasingly alienated them from their colleagues in the industry.  

Mathematization of computer science established it as a legitimate academic discipline and 
differentiated it from industrial computer engineering. An essential part of the development 
was the concept of algorithm, which provided a practical agenda for advancing the discipline. 
Computer science became a normal science, pre-occupied with normal puzzle-solving activity. The
newly founded research paradigm determines which questions are scientific (various questions 
about algorithms) and how answers should be sought (through formal methods). 

For a modern computer scientist, it is hard to imagine computer science where algorithm is _not_ 
a foundational concept, but that is because we are trained to think within our dominant paradigm.
A historical coincidence made algorithm a core part of our thinking about computation and it
requires a lot of imagination to think differently. It is even harder to question the idea that 
mathematics can find relevant answers to the questions posed by programming. This is because 
mathematization has become a part of our modern episteme. But how might a computer science look 
when our episteme or research paradigms change?

### The Algol research programme 
Paradigm shifts are rare and the change of episteme even more so. At a smaller scale, much of the 
modern academic programming language research has been influenced by the Algol research programme. 
The goal of the programme is to utilize the resources of logic to increase the confidence in the 
correctness of programs.  

The Algol research programme defines core assumptions, together with a sufficiently open ended 
agenda. Much of the theoretical programming language research aims to prove programs correct 
through the use of formal logic. The methods differ, but the hidden assumption that formal proof 
provides the correct methodology is shared. Any experimental failures (such as the fact that 
proving programs correct is still difficult after 50 years of the existence of the Algol 
research programme) are attributed to less core aspects – we do not yet have sufficiently 
powerful formal methods, the problem is not properly formally specified and programmers in 
the industry are just not using the right tools.

<div class="wdecor">
<img src="http://tomasp.net/blog/2016/thinking-unthinkable/draco.jpg" style="max-width:100%"/>
</div>

Thinking the unthinkable
------------------------
I outlined how programming and science are affected by assumptions that are implicit 
in research programmes, scientific paradigms and also the episteme of the current period.
I hope my concrete examples from programming language research convinced you that paradigms 
are not just abstract philosophical concepts. 

The establishment of theoretical computer science rooted in mathematics was not an inevitable 
development and it is conceivable that computer science would evolve differently, building 
on principles other than algorithms and formal logic. In programming language research, the 
predominant Algol programme is not the only one. A largely incommensurable research programme 
was defined by Smalltalk:

> Programming [in Smalltalk] was not thought of as the task of constructing a linguistic entity, 
> but rather as a process of working interactively with the semantic representation of the program,
> using text simply as one possible interface.

In the rest of the article, I speculates on what might programming research arising from a 
different paradigm look like. As I explained, our paradigms are incommensurable with 
paradigms arising from other assumptions. They do not share the same goals and ways of thinking. 
This is also the case for my two imaginary paradigms in the next two sections - but with enough 
imagination, you can imagine another way of thinking where those would make perfect sense!

### Taxonomies of programming ideas

Theoretical computer scientists try to extract mathematical essence of programming languages and 
study its formal properties. Now consider a kind of knowledge that instead aims to explore the 
design space and build a taxonomy of objects (ideas, concepts, ...) that occupy the space. It 
considers the entities as they are, rather than trying to extract their mathematical essence. 
What would be the consequence of such way of thinking that attempts to relate and organize 
programming ideas in taxonomies, rather than abstracting?

In programming language research, many novel ideas that defy mathematization are left out because 
they are too "messy" to be considered through the mathematical perspective. If our paradigm made 
us seek relationships, those would all became within the realm of computer science. For example, 
we would see similarities between live coded music and formula editing in Excel as both of those 
represent a form of programmer interaction with immediate observable feedback (thanks 
[@SamAaron](twitter.com/samaaron)!). 

Science should not merely observe, but also "twist the lion's tail" and run experiments to probe 
the properties of the nature. If our focus is on building taxonomies, the nature of relevant 
experiments will also differ. Rather than measuring properties of simple models, experiments 
designed to reveal relationships need to highlight interesting aspects of a behaviour in its 
full complexity. They need to reproducibly demonstrate relationships that are not immediately 
obvious, similarly to how thought-experiments in sciences and philosophy highlight an intriguing 
aspect of a theory. An interesting format that appeared recently is the presentation of programming 
research in the form of screencasts - this way, we can actually record and observe certain feature
in its full complexity, without eliminating aspects that may turn out to be important!

A consequence of the focus on taxonomies in 17th and 18th century was the creation of museums, 
which present the studied objects neatly organized according to the taxonomy. A computer scientist 
of such alternative way of thinking might follow similar methods. Rather than finding mathematical 
abstractions and presenting abstract mathematical structures, she would build (online and 
interactive?) museums to present typical specimen as they appear in interesting situations in 
the real-world. 

### Crossing the vertical gaps with metaphors

Does modern computer science have something to gain from the Renaissance age centered around signs 
and resemblances? Work on programming languages is done at three layers. At the upper layer, 
language or library designs follow some intuitive ideas, which are turned into an actual program 
(middle layer). Formal reasoning is done about a simplified mathematical model, which is the lower 
layer. Programming research operates horizontally - relating different formal models or various 
implementations - but does not easily cross between the layers. Theoretical work at the bottom 
is rarely linked with the informal top layer.

Signs and resemblances provide the missing link between layers. In object-oriented design, an 
object is a metaphor for an object in the real-world, but metaphors can be found in many areas of 
programming. We use them to conceive an idea and use our intuition at the higher level to guide 
design at lower levels. Since our scientific way of thinking does not consider such resemblances 
important, we then hide them (for brevity, or out of disinterest) from our published narrative. 

Metaphors often remain present only through naming. In John von Neumann's First Draft of Report on 
EDSAC (which described modern computer architecture) individual units are called "organs" suggesting 
a curious biological metaphor for the system structure. Even more obvious metaphors are hidden in 
the plain sight. For example, when and why did we start calling programming languages "languages"?

If resemblances and metaphors played a fundamental role in our scientific thinking, we would not 
just gain interesting insights from them, but we would also ask different questions (which appear 
secondary or even unscientific when considered through the perspective of our current way of 
thinking). What research agenda would computer scientist studying resemblances ask? Looking at the 
problem of concurrent programming as an example, one might try to design programming language 
models for concurrency by studying how simultaneity is expressed in different types of novels.  
What are the postmodern alternatives to threads where concurrent processes occurring in the same 
temporal interval are described in successive textual parts of the narrative?  

<div class="wdecor">
<img src="http://tomasp.net/blog/2016/thinking-unthinkable/serpens.jpg" style="max-width:100%" />
</div>

Conclusions
-----------

Overall way of thinking that is captured by episteme or paradigms has been changing throughout the 
history and we can expect it to continue changing. Ideas grounded in different episteme or 
paradigms are often incommensurable, meaning that they have different basic assumptions and 
consider different questions. Just like de Buffon was astonished when reading the work of 
Aldrovandi, we may wonder which of our current scientific achievements will, in the future, appear 
as exact anatomical descriptions and which will appear as fables or magic.

Were we to think about programming in terms of taxonomies, much of the present work that explores 
interesting aspects of, say, programming language design space will still be relevant. Future 
readers might be astonished why we focus on irrelevant technical details rather than trying to 
present the most unique interesting aspects of the work in their full richness. 

Were we to think about programming in terms of similarities and metaphors, formal mathematical 
models will become just one of many forms of metaphors available when understanding programs, but 
future thinkers might be astonished by our attempts to find more and more remote abstractions that 
gradually lose more and more of the similarity with the original idea and become merely a 
work of art or fiction.

> _<i class="fa fa-hand-o-right" style="font-size:110%;margin:0px 5px 0px 0px"></i>
> If you got all the way to the end, you might also be interested in a [workshop we are 
> planning](https://github.com/tpetricek/anarchy-workshop/blob/master/proposal.md). To keep in 
> touch leave a [comment on GitHub](https://github.com/tpetricek/anarchy-workshop/issues/3), ping me
> at [@tomaspetricek](http://twitter.com/tomaspetricek) or email [tomas@tomasp.net](mailto:tomas@tomasp.net)!_

References
----------

The following is just a list of some of the papers and books that contributed to the ideas in this 
article in one way or another. You will find proper citations in the 
[original paper](http://tomasp.net/academic/drafts/unthinkable/unthinkable-ppig.pdf).

 1. Thomas Kuhn. _The structure of scientific revolutions_. University of Chicago Press, 1962
 2. Paul Feyerabend. _Against method_. Verso Books, 1975
 3. Michael Foucault. _The Order of Things: An Archaeology of the Human Sciences_. Routledge, 1966
 4. Imre Lakatos. _The methodology of scientific research programmes_. Cambridge University Press, 1978
 5. Nathan Ensmenger. _The computer boys take over_. MIT Press, 2012
 6. Mark Priestley. _A science of operations_. Springer, 2011
 7. John von Neumann. _First Draft of a Report on the EDVAC_. University of Pennsylvania, 1945
 
