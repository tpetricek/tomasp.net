Cities and programming [DRAFT]
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

- Second, there are a number of more concrete ideas in architecture and urban planning that
  might directly apply to software. For example, can programmers learn how to deal with complexity
  of software by looking at how urban planners deal with the complexity of cities? Or, can we learn
  about software maintenance by looking at how buildings evolve in time?

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

### 1.3 Elegant theories that do not work

My last reason for looking at work in architecture and urban planning is that the two disciplines
were heavily influenced by theories that are elegant, but do not actually work in practice.
Stewart Brand's claim that _architects are incapable of producing pleasant spaces, except by
accident_ from [How Buildings Learn][learn] is perhaps too extreme, but I imagine some people
may feel very similarly about the software discipline. Just like some work on programming, theories
of architecture and urban planning often leave human aspects out of the picture. Consequently,
the theories are not just slightly imprecise, but completely wrong and actively harmful.
Interestingly, a few urban planners and architects reflect critically on this and that's
where programming researchers can learn.

#### Form and context
In [Notes on the Synthesis of Form][notes], Christopher Alexander points out that design always
speaks of form and its context. A good design is not just a property of the form, but it is a
matter of fit between the form and the context. The reason why we cannot evaluate an isolated
form is not because we are unable to precisely describe the form itself, but because we are
unable to precisely describe the context with which it will interact:

> [T]he opportunity to evaluate the form (...) depends on the fact that we can give a
> precise mathematical description of the context. In general, unfortunately, we cannot
> give an adequate description of the context we are dealing with. There is as yet no theory
> (...) capable of expressing a unitary description of the varied phenomena we encounter
> in the urban context (...).

Exactly the same limitations exist in the world of programming. No matter how precisely we can
talk about programs, we also need to exactly understand the environment with which they interact.
This is the hard part. As David Parnas writes in his well known essay [No Silver Bullet][bullet]:

> Much of the complexity [software engineer] must master is arbitrary complexity, forced
> without rhyme or reason by the many human institutions and systems to which his
> interfaces must confirm.

#### Radiant garden city beautiful
The book [The Death and Life of Great American Cities][life] by Jane Jacobs, which I mentioned
already, is a critique of urban planning theories that are appealing but completely wrong.
This includes Le Corbusier's [Radiant City](https://en.wikipedia.org/wiki/Ville_Radieuse)
(replacing streets with towers in a park), British [Garden City](https://en.wikipedia.org/wiki/Garden_city_movement)
movement (separating functions in a city and building suburb-like residential areas)
and [City Beautiful](https://en.wikipedia.org/wiki/City_Beautiful_movement) movement
(introducing beautification and monumental grandeur in cities).

All of the theories criticised by Jacobs are based on simple and convincing principles that
are supported by reasonable arguments. A city with a centre surrounded by residential areas
with open spaces and parks _should_ be a way of combining benefits of countryside and city
environments. However, it turns out that this idea often does not work for various complex
reasons. One point made by Jacobs is that public spaces only work if they combine different
functions so that people have reasons for passing through the space throughout the day, but
this requires diverse neighbourhoods that combine residential spaces with shops, restaurants,
cultural venues and offices.

In software engineering and programming research, we also often come up with simple and convincing
principles supported by reasonable arguments. To poke at my own community: if we want to make
sure that programming languages have well-defined behaviour, it seems reasonable to define a
small formal model that captures the essential properties of the language and formally study
its properties. Except that it turns out that the non-essential properties are often equally
important and a lot of usability of programming languages comes from things that we do not normally
even think about as parts of the language such as package management or tooling.

Can the criticism of urban planning theories, like the one by Jane Jacobs, teach us something
about our own programming theories and software engineering principles? I will discuss some
concrete ideas in Section 2.1.

#### The separation of design from making

There is one area where the practice of building software already underwent a change that
is similar to a change that has not quite happened in architecture yet and that some critics
have been calling for. [Stewart Brand][learn] quotes Christopher Alexander who, unlike many
other architects, makes on-site adjustments to his buildings:

> There is real misunderstanding about whether buildings are something dynamic or something
> static. (...) Anything different from the idea that you make a set of drawings and someone
> else builds the thing is incredibly threatening. People get just absolutely freaked out.

Alexander also notes that this is likely for contractual reasons. If you make on-site adjustments,
then those will inevitably change the final price. To a person familiar with software engineering,
these comments read as the motivation behind [Agile software development](https://en.wikipedia.org/wiki/Agile_software_development)
methodologies. I think most people in our industry have, by now, realised that a typical
software system is something dynamic and that we need to make "on-site" adjustments while building
it. Of course, various concrete practices arising from Agile are now also being treated as
simple overarching theories that will solve all our problems, just like the urban planning
theories discussed in the previous section.

In any case, it seems that similar problems exist in architecture and software development, which
suggests that looking at architecture for inspiration about programming is worthwhile.

## 2. New methodologies for programming research

As mentioned earlier, I think the software world can learn from architecture and urban planning
in two ways. We can look at concrete ideas about buildings and cities or we can look at
methodologies that critics of architecture and urban planning use in their work. In this
section, I'll focus on the former.

Perhaps the first thing that we should learn is to spend more time critically reflecting
on how our methods work, when they do not work and why. In academic programming research,
[Onward! Essays](https://www.sigplan.org/Conferences/Onward/) and [The Art, Science, and Engineering
of Programming](https://programming-journal.org/) journal publish some reflections on how we
work, but I don't know of any systematic, longer critique akin to Jane Jacob's
[The Death and Life of Great American Cities][life]. Looking at the critiques of architecture
and urban planning, I can think of three ways of reflecting on software and
programming that would give us invaluable knowledge and, possibly, inspire new directions
for future work.

### 2.1 What cities and buildings work in spite of theory

The methodology that Jane Jacobs uses to criticize urban planning theories could be equally
applied to criticize programming theories. It is obvious that any theory makes simplifications
and does not always precisely work in its purest form, but are there cases where theories are
blatantly wrong? Jane Jacobs does a great job at documenting such cases in the urban planning
context. She looks at concrete city districts that function well in spite of urban planning theories
that present theoretical arguments for why those districts must be unpleasant places to live.

Theories inspired by the Radiant City and Garden City ideas support separation of functions
in a city and argue for large parks. Yet, two of Jacobs' examples of healthy districts,
Greenwich Village in New York and North End of Boston (in 1950s) have none of that. They
rely on active "sidewalk life" which is made possible thanks to the diverse use of buildings
in the district. Those districts are safe, because the mixed use guarantees that local people
are always passing the area, implicitly keeping an eye on what is going on. They "unslum", because
they are still attractive places to live for those members of the community who get better jobs
and start earning more money. In other words, there are many complex intricate social processes
that are nothing like what the urban planning theories of 1950s consider a good model for a city.

In the world of programming, we also have many systems that work well, in spite of being
completely wrong according to our theories of what a good programming system should look like.
Popular programming languages like PHP, JavaScript or R are the most obvious examples.
There are [some attempts](!socioplt) to explain why, but we mostly just [disregard those](!ghica)
saying that they got popular by accident. For a community that prides itself
in being thorough and scientific, this is a very shallow argument. As Peter Naur wrote
in [The Place of Strictly Defined Notation in Human Insight][naur]:

> It is curious to observe how the authors in this field, who in the formal aspects of their
> work require painstaking demonstration and proof, in the informal aspects are satisfied with
> subjective claims that have not the slightest support, neither in argument nor in verifiable
> evidence. Surely common sense will indicate that such a manner is scientifically unacceptable.

Following Jane Jacobs, we should undertake detailed studies of existing programming systems
that, in some sense, work well. I believe such approach would have to take human aspects more
seriously into account, because those are often essential for understanding why a system works.
We might need to learn from research on human-computer interaction which has experience with such
research. For example, a fun 2016 paper [A Farmer, a Place and at least 20 Members: The Development
of Artifact Ecologies in Volunteer-based Communities][farmer] looks how an eco-farming community
uses an evolving range of computing systems and tools over a number of years. Could a perspective
like this help us understand why JavaScript or the R ecosystem are successful programming
systems? For JavaScript, there might even be two distinct answers. One for the 1990s era when you
could view source and copy other people's rollover or mouse-follow effects and the 2000s era of
node.js.

One interesting example that documents something along those lines is the discussion about
MIDI devices in a [Salon des Refusés](https://www.shift-society.org/salon/) paper
[Tracing a Paradigm for Externalization: Avatars and the GPII Nexus][gpii] by Colin Clark and
Antranig Basman.
 

### 2.2 Learning about how software learns

#### How buildings evolve

almost no buildings are designed to adapt, but they adapt anyway (learn, p2)
random: software architects make the same mistakes as actual architects - they ignore learning
wisdom acquired looking back must be translated to forward-looking wisdom (life, p109)
examine buildings as a whole, both in space and time (learn, p3)

#### How cities evolve

city structure consists of mixture of uses, generating diversity (life, p390)
-> similar to the process of concretization in Simondon
population change jobs and interests - diversified districts allow them to stay (life, p149)

### 2.3 How we navigate and understand

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

in programming, we cannot fully understand things (programming by poking)
we cannot hope to do this - but we can build local understanding by poking
enough in some region. we should be able to navigate around such understood
regions. We also can have small fully isolated regions that we can then fully
understand, e.g. by using fancy types and immutability. (Jeremie)


TODO: Search for "(!" and add references!


 [arguments]: https://xxx "Rebecca Slayton (????). Arguments that Count"
 [learn]: https://xxx "Stewart Brand (????). How Buildings Learn: What happens after they're built"
 [life]: https://xxx "Jane Jacobs (1961). The Death and Life of Great American Cities"
 [sds]: http://?? "David Parnas (????). Software Aspects of Strategic Defence Systems"
 [notes]: http://?? "Christopher Alexander (????). Notes on the Synthesis of Form"
 [bullet]: http://?? "Fred Brooks (????). No Silver Bullet"
 [naur]: http://?? "Peter Naur (????). The Place of Strictly Defined Notation in Human Insight"
 [farmer]: http://??? "Susanne Bødker, Henrik Korsgaard, Joanna Saad-Sulonen (???). A Farmer, a Place and at least 20 Members: The Development of Artifact Ecologies in Volunteer-based Communities"
 [gpii]: https://www.shift-society.org/salon/papers/2017/revised/externalization.pdf "Colin Clark, Antranig Basman (2017). Tracing a Paradigm for Externalization: Avatars and the GPII Nexus"
