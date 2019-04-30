Cities and programming
====================================================

 - title: TBD
 - date: 2019-04-08T12:22:57.7521867+01:00
 - description: TBD
 - layout: post
 - references: true
 - image-large: http://tomasp.net/blog/2019/software-engineering/sdi.jpg
 - tags: academic, programming languages, philosophy

----------------------------------------------------------------------------------------------------

Despite having the term _science_ in its name, it is not always clear what kind of
discipline _computer science_ actually is. Research on programming is sometimes like
science, sometimes like mathematics, sometimes like engineering, sometimes like design
and sometimes like art. It also has a long tradition of importing ideas from a wide range
of other disciplines.

In this article, I will look at ideas from architecture and urban planning. Architecture
has already been an inspiration for _design patterns_, although some would say that we did
quite poor job and imported a trivialized (and not very useful) version of the idea. However,
there are many other interesting ideas in architecture and urban planning worth exploring.

To explain why learning from architecture and urban planning is a good idea, I will first
discuss similarities between problems solved by architects or urban planners and programmers.
I will then look at a number of concrete ideas that we can learn, mostly taking inspiration
from four books that I've read recently. There are two general areas:

- First, writing about architecture and urban planning often uses interesting methodologies
  that research on programming could adopt to gain new insights into systems, programming
  and its problems.

- Second, there are a number of more concrete links that are worth exploring. For example, can
  programmers learn how to deal with complexity of software by looking at how urban planners
  deal with the complexity of cities? Or, can we learn about software maintenance by looking at
  how buildings evolve in time?

The nature of problems that programmers face are often more similar to the problems that
architects and urban planners have to deal with than, say, the problems that scientists, engineers
or mathematicians need to solve. We might not want to go all the way and completely rebuild
how we do programming to mirror architecture and urban planning, but treating the ideas from those
disciplines as equal to those from science or engineering will make programming richer and more
productive discipline.

----------------------------------------------------------------------------------------------------

## 1. Why software is like buildings and cities

Many people have linked programming with writing, gardening or even planning a dinner party.
Whether we can learn something useful from making those analogies crucially depends on whether
those activities actually have any structural similarities with programming.
Before talking about what we can learn from buildings and cities, I will discuss some of the
structural similarities to convince you that they are, in fact, a good analogies!

### 1.1 Problems of organized complexity

My first resource for this article is a 1961 book [The Death and Life of Great American Cities][life]
by Jane Jacobs. The book focuses on problems in urban planning and points out numerous flaws in the
1950s urban planning ideas that were behind (mostly failed) housing projects and poorly planned
parks that contributed to the rise of criminality in cities like New York. Jacobs talks about
how neighbourhoods work and why some of them work better than others.

However, the point that I want to discuss first is from the last chapter, which discusses "what
kind of a problem city is". Jacobs refers to an essay on science and complexity by Warren Weaver,
which lists three stages of development in the history of scientific thought:

1. **Problems of simplicity.** Science first learned how to solve problems involving
   small number of variables, for example, how to analytically and experimentally analyse
   how gas pressure depends on the volume of the gas.

2. **Problems of unorganized complexity.** The second kind of problems that science tackled
   are problems that involved huge number of variables (say, millions of solid balls rather than
   just two solid balls), but where the behaviour is sufficiently random. We might not be able
   to give exact answers, but we can get useful insights using statistics and probability theory.

3. **Problems of organized complexity.** The most challenging problems are complex, but
   at the same time, involve sophisticated organization that is essential and cannot be easily
   abstracted away through statistics. For example, how exactly is genetic code
   reflected in the characteristics of an organism?

As you can guess, urban planning is a problem of _organized complexity_. They involve many
different problems, interconnected through a huge number of variables. The processes involved
in those problems have rich structure that cannot be usefully summarized using statistics.
For example, how a park is used depends on how it is designed, but also on who lives around and
what businesses exist in the neighbourhood, which, in turn, depend on what is the size of blocks
and age of buildings in the surroundings. To learn anything useful about a park, you have to
study all those aspects in their full complexity. Whether a park works depends on a highly
sophisticated network of factors.

Software systems are also problems of organized complexity. Any large software system involves
a huge number of variables and interconnected processes that are linked in subtle ways and
often influence each other. Analysing software systems using simplified models often fails,
because the model has to ignore some aspects and, often, ends up ignoring aspects that are
actually important.

A nice argument for the irreducible complexity of software systems was written by David Parnas
in [Software Aspects of Strategic Defence Systems][sds]. Parnas describes three kinds of systems
with increasing levels of complexity:

1. **Analog systems.** Analog systems are modelled as continuous functions. Those can be analysed
   using well-understood mathematical tools. To show that a system is reliable, you just need to show
   that the values stay within operating range.

2. **Repetitive digital systems.** Digital systems are not continuous. Analysing those is harder
   and does not scale well with increasing number of states. However, if we have a large number of
   repeated components (like in a CPU), then the analysis is still doable.

3. **Non-repetitive digital systems.** Software systems are not continuous and also do not have
   repetitive structure and, consequently, have orders of magnitude more states. This is a
   fundamental difficulty that makes building reliable software systems hard.

The kind of complexity in the case of software is not exactly the same as the kind of complexity
in the case of urban planning and life sciences, but there are notable similarities. In the easy
case (problems of simplicity and analog systems), we can fully analyse the system. In the second
case (unorganized complexity and repetitive digital systems), we can reduce the full complexity
either by using statistical methods or by exploiting the repetition. In the last case
(organized complexity and non-repetitive digital systems), the complexity of the problem cannot
be reduced - we need to consider a large number of interacting processes or components. At the
same time, all of them are equally important and play an important role in some aspect of the
system. As Jane Jacobs puts it, _the large number of interrelated variables form an organic
whole_.

Software engineers and researchers working on programming tools have done a fair amount of work
towards being able to analyse complex systems with large number of states, mostly by building
tools that scale better and can handle more states. However, the number of states grows
faster than the size of the system and so our tools still hit their limits very easily. Urban
planners never attempted to understand every little detail about how cities work, yet, they have
learned valuable knowledge about cities. I will sketch some ideas for how we can learn from urban
planners in Section 3.2.

### 1.2 Structures obtained by gradual development

To discuss the second structural similarity between buildings and software, I will refer to
Rebecca Slayton's history of anti-ballistic missile defence from [Arguments that Count][arguments]
and Stewart Brand's interview with Christopher Alexander from his book [How Buildings Learn][learn].

> Alexander is inspired by how design occurs in the natural world. "Things that are good have
> a certain kind of structure," he told me. "You can't get that structure except dynamically.
> Period. In nature you've got continuous very-small-feedback-loop adaptation going on, which is
> why things get to be harmonious. That's why they have the qualities that we value. If it
> wasn't for the time dimension, it wouldn't happen."

Brand argues that many great buildings achieved their greatness by gradual stepwise evolution
over time. New buildings need to be designed with the expectation that they will evolve, because
they usually outlive their initial use. Even if a building does not change its use (e.g. it remains
the same university department), the needs of its users will change and the building will need to
adapt. This should be done, for example, by making sure that changing the space layout in the
building is possible without changing the structure.

A similar line of thought can be found in the debate about building an anti-ballistic missile
software that would automatically track and destroy Soviet missiles. In the debate, a number of
computer scientists pointed out that such system cannot be reliably built for fundamental software
engineering reasons. Slayton quotes Weizenbaum's letter saying that _"large computing systems are
products of evolutionary development"_ and that large computer systems _"only became reliable
through a process of slow testing and adaptation to an operational environment"_. In the case of
anti-ballistic missile software, the problem is that the change rate in the environment is faster
than the rate at which the software can evolve (it is cheaper to build a new, more effective,
offensive weapon than a defence against it).

Brand argues that we should think about buildings not just in spatial dimensions, but also in
the time dimension. The same is the case for software. In some way, development methodologies that
treat development as gradual process already do this (and I'm not the first person to [link Brand's
book to methodologies like Extreme Programming](http://www.manasclerk.com/blog/2003/07/23/brands-how-buildings-learn-and-why-cant-software/)),
but I think we can go further. First, I suggest that computer scientists should spend more time
studying _how software learns_ (Section 2.2). Second, I think there are lessons about adaptability
of software that we can learn from the adaptability of cities and buildings (Section 3.1).

### 1.3 Theories that should work, but do not



Radiant Garden City Beautiful (life, p314) - works in theory, not in practice
-> what is the programming language equivalent? beautiful simple calculus? (Worse is Better)

we can evaluate isolated form when given exact context description (form, p20)
 -> possible in physics (sometimes) but not urban context (and also not software)

architects are incapable of producing pleasant spaces except by accident (learn, p53)
-> dtto for working sw and computer scientists

architects make drawings, someone else builds them (learn, p64)
-> anything else freaks people out; Alexander likes to make adaptations while building
   because you can never fully imagine thing until it's being built
-> why upfront software architecture idea does not work (more agile)


## 2. New methodologies for programming research

### 2.1 What cities and buildings work in spite of theory

we need Jane Jacobs like book on programming systems - what systems should not work according
to our beliefs (planners) but actually work in practice? (Like the Boston neighbourhood -
c.f. Antranig's MIDI example)

### 2.2 Learning about how software learns

almost no buildings are designed to adapt, but they adapt anyway (learn, p2)
random: software architects make the same mistakes as actual architects - they ignore learning
wisdom acquired looking back must be translated to forward-looking wisdom (life, p109)
examine buildings as a whole, both in space and time (learn, p3)

### 2.3 How cities evolve

city structure consists of mixture of uses, generating diversity (life, p390)
-> similar to the process of concretization in Simondon
population change jobs and interests - diversified districts allow them to stay (life, p149)

### 2.4 How we navigate and understand

"consider city as perceived by its inhabitants" (image, p3)
we are continually engaged in an attempt to organise our surroundings (city, p90)
image organisation at this scale is wholly new problem (city, p119)
"a highly imageable city lets observer absorb sensor inputs easily without disruption of basic image" (city, p10)
"good framework gives possibility of choice and a start for growth" (city, p4)

## 3. Directions for new kind of research

### 3.1 Designing adaptable software

different kinds of buildings change differently - dtto for software? (learn, p7)
adaptive buildings has to allow layers to evolved independently (learn, p20)
material that "looks bad before it acts bad" is important in building (and software!) (learn, p118)
problems with traditional materials are well-documented (learn, p118)
-> you can use software with known bugs provided there are known workarounds
how to teach maintenance habits? make parts ephemeral and require replacement in a year (learn, p130)
how buildings/software learn? temporary buildings become permanent thanks to their adaptability (learn, p165)
-> should we design software to be more temporary and adaptable?
scenario planning - all predictions are wrong, so we instead need to find strategy that is
  adaptable enough to handle many different possible future scenarios (learn, p178)

#### Vernacular design method

vernacular & detail of plan is a measure of cultural disharmony (life, p132)
vernacular allows skillfull solutions to problems rather than reinventing forms (life, p135)
what does vernacular evaluation select for? (life, p150)
form froze function - immobilizing high change layer by making it too rigid (life, p157)

simple cultures never face the problem of complexity that we face (form, p32)
 -> simple cultures achieve good fit through practice, without understanding
    (we might not need to understand complex systems if they allow inspection & use through practice)
 -> for this, we also need good materials that reveal what's going on (look bad before behave bad)
system needs time to reach equillibrium each time it is disrupted (form, p51)
immediate feedback makes this possible - prevents build-up of errors (form, p51)
un-selfconscious culture achieves good fit by being self-adjusting (form, p55)
 -> this is why we need to improve programming via feedback
little is required from the individual (respond to misfits by small changes) (form, p58)
 -> maybe related to how craftsman people work
once we start questioning, we have to say "why" - and concepts like "safety" or "economics"
  do not help because they do not correspond to sub-systems (form, p65)

### 3.2 Dealing with organized complexity

planners of 1930s treat cities as problem of unorganized complexity - stats apply (life, p450)
-> PL theoreticians of 2000s treat programs as what? (calculi removes a lot)
this book is one way of treating organized complexity (life, 454-455)
-> 1) think about processes, 2) work inductively 3) seek 'unaverage'
-> city processes are too complex to be abstracted
one unaverage value (bookshop opening time) tells us more than statistic (life, p455)
-> PLs need to be considered in their complexity (unabstracted) looking at the unaverage
   (this is exactly what all formal calculi attempt to eliminate!)

### 3.3 Designing for easy navigation

goal is quality of the image in mind (city, p117)
elements: paths, edges, districts, nodes, landmarks
districts: distinct qualities, landmarks: easily identifiable
navigation: people with more knowledge used paths, people with less knowledge used districts
continuity of names: Beacon hill, Beacon street (city, p52)
subway is disconnected (like calling a method in OOP?) (city, p57)





 [arguments]: https://xxx "Rebecca Slayton (????). Arguments that count"
 [learn]: https://xxx "Stewart Brand (????). How Buildings Learn: What happens after they're built"
 [life]: https://xxx "Jane Jacobs (1961). The Death and Life of Great American Cities"
 [sds]: http://?? "David Parnas (????). Software Aspects of Strategic Defence Systems"
