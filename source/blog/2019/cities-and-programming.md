On architecture, urban planning and software construction [DRAFT]
=========================================================

 - title: On architecture, urban planning and software construction
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
planners in Section 3.3.

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

#### Form and context in software and architecture
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

## 2. Learning from urban planning and architecture methodologies

As mentioned earlier, I think the software world can learn from architecture and urban planning
in two ways. We can look at concrete ideas about buildings and cities or we can look at
methodologies that critics of architecture and urban planning use in their work. In this
section, I'll focus on the latter.

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
There are [some attempts][socioplt] to explain why, but we mostly just [disregard those](http://danghica.blogspot.com/2016/09/what-else-are-we-getting-wrong.html)
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
Antranig Basman, which discusses a system which succeeded precisely because it did not follow
the principle of _information hiding_.

### 2.2 Learning about how software evolves

Another important aspect of the methodology used both by Jane Jacobs and Stewart Brand is that
they look at cities and buildings over a long period of time. Buildings and cities will be around
for decades and centuries and we need to consider them with respect to the time dimension.
The same is the case for software systems. Many software systems will be around for decades and
will need to adapt to new circumstances.
Computer scientists are aware of the time dimension and the [open/closed principle](https://en.wikipedia.org/wiki/Open%E2%80%93closed_principle),
introduced by Bertrand Meyer in [Object-Oriented Software Construction][open], is one theoretical
answer to those challenges. However, looking at how buildings and cities evolve would give us new
methods and ideas for understanding how software evolves.

#### How buildings evolve

[Christopher Alexander][notes] distinguishes between _unselfconscious_ culture
that achieve a good fit between context and form through practice and gradual adaptation and
_self-conscious_ culture that aims to achieve theoretical understanding of the complexity
of the system and design a solution. The invention of self-conscious "architecture" destroyed
the old process of building. The evolution over time is one of the aspects of designing that
self-conscious architecture often gets wrong. Stewart Brand opens his book [How Buildings Learn][learn]
with a damning summary:

> Almost no buildings adapt well. They're _designed_ not to adapt; also budgeted and financed
> not to, constructed not to, administered not to, maintained not to, regulated and taxed
> not to, even remodelled not to. But all buildings (...) adapt anyway, however poorly,
> because the usages in and around them are changing constantly.

There might be useful ideas that computing can learn from unselfconscious cultures and I will
return to this topic in Section 3.1. For self-conscious cultures, designing in a way that allows
forms to adapt when the context changes seems to be a major challenge, both for architects and
for programmers.

Brand's book does not give a simple answer to the question of how to design an adaptable building,
but it suggests a possible methodology and takes the first step. He documents how numerous existing
buildings evolved over time and looks for common patterns. _Low Road_ buildings are flexible,
cheap to modify and can easily adapt to a very different purpose (such as from a warehouse to a
co-working space). _High Road_ buildings adapt slowly with more respect to their history, are more
expensive to maintain, but they develop a unique character.

Concepts such as the distinction between High Road and Low Road might directly apply to
software systems. On one hand, some software systems provide robust core structure, but can be
easily adapted and make it easy to throw away parts that are no longer needed. On the other hand,
there are software systems that evolved more slowly, have longer history that they respect  and
are more expensive to maintain, but can reliably provide services that are complex and cannot
be easily replaced.

That said, whether categories such as High and Low Road apply to software is perhaps the less
interesting lesson here. The more interesting is the methodology that Stewart Brand proposes and
follows. Just like he documents the way buildings evolve over time, we should be documenting how
different kinds of software systems evolve. Only then we can meaningfully start looking for
various patterns in such evolutions and use this to design more adaptable buildings.

Brand suggests that _"the wisdom acquired looking backward must be translated into wisdom
looking forward"_ and asks _"how to design new buildings that will endear themselves to
preservationists sixty years from now"?_ Similarly, we should examine existing software systems
with respect to time, look for those that evolved in appealing ways (and endeared themselves
to preservationists) and use the knowledge we can gain by reflecting on those to design better
software systems for the future.

#### How cities evolve

As I argued earlier, the complexity of some software systems is more akin to the complexity of
an entire city than akin to the complexity of a single building. The way city districts evolve and
adapt is one of the important topics in [The Death and Life of Great American Cities][life] by Jane
Jacobs. City districts are built by planners, city architects, businesses and the thousands of their
inhabitants. A similar range of people have influence on software including the programmers, owners
of interfaces that the software interacts with and the thousands of users. Just like cities adapt
to serve their inhabitants, software adapts to serve its users and to fit with the environment.

Jane Jacobs argues that city districts that work well for their inhabitants are those with a
structure that supports and generates diversity of both uses and inhabitants. Diversity in the
structure of a city allows people to stay in the same district as their jobs, interests and family
situation changes. A good city district is not one where different functions are clearly separated
into, say, office district, a shopping mall and suburb for living, but one that integrates many
different functions.

Does this teach us anything about software systems? One of the widely accepted good design practices
when building software is [separation of concerns](https://en.wikipedia.org/wiki/Separation_of_concerns),
introduced by Dijkstra in his 1974 paper [On the role of scientific thought][separation] and so
it seems that diversity achieved through integration of different functions is exactly against good
software engineering principles. However, separation of concerns is something that we often start
with and which gradually erodes during the adaptation process that software goes through.

Arnaud Bailly reflects on this reality of software in his talk [On the Mode of Existence of
Software](http://videos.ncrafts.io/video/221100040), which borrows ideas on _technical objects_
introduced by Gilbert Simondon in his 1958 book [On the Mode of Existence of Technical Objects][mode].
One of the crucial properties of technical objects, such as a car engine, is that they undergo a
process of _concretization_ through which components that were initially designed for separate
functions start serving multiple purposes. This is very similar to the process through which
software evolves and through which it becomes more like the city district discussed by Jane Jacobs.

Urban planners of the 1950s thought that separation of concerns is a good principle for city
design. Jane Jacobs pointed out that this is not the reality of welcoming city districts and
pioneered a new perspective on urban planning that acknowledges, studies and celebrates this
diversity. There might be a similar opportunity for understanding software systems. If we look
at software systems that evolved over time, we might learn what factors make the process of
concretization work favourably and, fundamentally, learn how to create software that will grow well.

### 2.3 Navigating and understanding software and cities

So far, I discussed two interesting methodologies that some authors followed in urban planning
and architecture. I talked about examining real systems that work in spite of theory and I talked
about the need for looking at how systems evolve over time. The third methodology I will consider
is to look at how people understand and conceptualize cities and software. This section is mostly
inspired by the 1960 book [The Image of the City][image] by Kevin Lynch.

In his book, Kevin Lynch studies the legibility of a city. A legible city is _"one whose districts
or landmarks or pathways are easily identifiable and are easily grouped into an overall pattern."_
A legible city is more pleasant and easy to live in for its inhabitants. Similarly, I believe that
programming needs to strive to produce legible software, both for its users and for its
developers and future contributors. I will focus on the latter. The former is obviously also
important, but it is a topic for a blog post on user experience design. The methodology for
studying legibility of a city (and software) needs to be not only technical, but also
psychological. To quote Lynch _"we must consider not just the city as a thing in itself, but the
city being perceived by its inhabitants."_

Lynch identifies a number of aspects of a city that are important for its legibility such as
_paths_, _districts_ and _landmarks_. He looks at three cities as examples and discusses how
different people navigate in the city and what characteristics of districts, paths and other
aspects make a city legible. Some interesting points are that more knowledgeable people typically
follow paths, while visitors rely on districts; districts are a useful guide for navigation if
they each have a different character (such as red brick houses in one and stone buildings in
another). The navigation also depends on the mode of transport. Subway (and railroads) gives
a disconnected image of the city that is quite different than the one obtained by walking or
cycling around.

Thinking about code base of a large software system, we can easily imagine very similar ideas.
There are paths that one might follow when reading code, such as the execution order; some paths
may be disconnected such as when you search for uses of a certain function or class; there might
even be districts in which the code has different character, perhaps because some parts are much
older or use a different style (ironically, using inconsistent coding styles might actually help
the developer understand that they are in a, say, poorly tested "code district" where making
changes is more dangerous).

As before, there might be some concrete similarities between aspects that make a city legible
and aspects that make a large code base legible, but the general idea is perhaps more interesting.
Legibility is an important property of any non-trivial software. Following Lynch, we should look
at a number of good and bad examples, see what aspects of a code structure contribute to
legibility and use such lessons for building software and perhaps also developer tools and
languages that promote good practices.

## 3. Borrowing concrete urban planning and architecture ideas

The previous section mostly focused on methodological concerns. The three books about architecture
and urban planning by [Jane Jacobs][life], [Stewart Brand][learn] and [Kevin Lync][image] that
I used as my main inspiration all follow a method of inquiry that can be adapted to study software
systems and that would give us valuable insights about them. However, I suggested earlier that
there is a more profound similarity between urban planning or architecture and programming.
More specifically, the kind of complexity that programmers need to control is not unlike the
complexity that urban planners need to deal with. Because of this similarity, I believe that there
is a number of concrete ideas from urban planning and architecture that would translate well to
the world of programming. In this section, I look at three such cases.

### 3.1 Designing adaptable software

There is a wealth of good ideas about designing buildings that adapt well over time in
Stewart Brand's [How Buildings Learn][learn]. Those that I find the most relevant for software
are ideas around _maintenance_ and, more specifically, around _materials_.
When discussing maintenance, Brand mentions the cautionary tale of [vinyl siding](https://en.wikipedia.org/wiki/Vinyl_siding),
which is used to avoid problems with peeling paint. Rather than repainting a wooden wall, you
cover it with a layer of vinyl siding, which is durable and weather resistant. The problem is that
vinyl siding blocks moisture and the humidity behind it can cause structural damage to the building.
Many traditional materials have the attractive property that they _look bad before they act bad_
and, furthermore, _the problems with traditional materials are well understood_.

I suppose that the material from which software is built would include things like programming
languages and libraries. The lesson about using traditional materials has a relatively easy
parallel. If you build software using tools whose problems you understand, you will be able to
expect and resolve those problems. If you are using a new material, you will not anticipate where
problems might occur. The lesson about materials that look bad before they act bad suggests a
more interesting challenge. How do we build software so that it gradually and gracefully degrades
rather than abruptly stops working?

Another point made by Brand is about maintenance more generally. Just like software systems, any
building built using any kind of materials requires some maintenance over time. And just like with
software systems, building owners are often bad at performing the necessary maintenance.

> Too often a new building is a teacher of bad maintenance habits. After the initial shakedown
> period, everything pretty much works, and the owner and inhabitants gratefully stop paying
> attention to the place. Once attention is deferred, deferring of maintenance comes naturally.

A clever answer is to design buildings so that they teach good maintenance habits and design
some parts of the original work as intentionally ephemeral. If there are parts that will require
maintenance within a year, we will get into a good habit that is necessary anyway once the building
is older. The same seems to be a very good suggestion for building software systems. If we
build our systems in a way that intentionally makes some parts degrade more quickly, we will
establish the right methods and processes for maintenance that will be valuable in the long run.
The idea of [chaos engineering](https://en.wikipedia.org/wiki/Chaos_engineering) is
perhaps a first step in this direction.

More generally, Brand also points out that _all buildings are predictions_ and _all predictions
are wrong_. When designing a building, doing so for one specific use will soon make it wrong,
because it turns out that our understanding of the use case was not correct. In software, we know
only too well that requirements change. Rather than developing a concrete plan based on our best
understanding, we should develop a strategy that is _designed to encompass unforeseeably changing
conditions_. Brand suggests that architects should use strategic planning methods such as
[scenario planning](https://en.wikipedia.org/wiki/Scenario_planning). Again, there might be
interesting lessons here for software development methodologies.

### 3.2 Vernacular design method

When discussing how buildings evolve, I mentioned the contrast between unselfconscious cultures
and our modern self-conscious architecture that [Christopher Alexander][notes] identifies.
Self-conscious approach to buildings requires us to analyse all aspects of the context and design
a solution to problems we identify. The unselfconscious approach achieves a good fit through
practice without understanding. For example, [Musgum mud huts](https://en.wikipedia.org/wiki/Musgum_mud_huts)
evolved to use the mathematically ideal catenary arch and are extremely good at keeping houses cool
inside on hot summer days. [Stewart Brand][learn] also discusses how buildings are adapted by
non-architects and uses the term _vernacular_ architecture. The common lesson is that there are
ways of achieving a good design without explicitly designing. Some practices of such vernacular or
unselfconscious approaches might work equally well when building software systems.

Vernacular design restricts the scope of the problem by limiting architectural ideas to what is
typically used in the local context. This reduces the design task and allows the builder to focus
on skilful solutions to specific problems rather than at reinventing forms. Such folk architecture
might appear homogeneous and unified at first, but is rich and diversified in details. As Alexander
acknowledges, unselfconscious cultures never face the problem of complexity that we face, but they
are still worth studying because they have a very efficient way of solving problems in a more
narrow context.

In software construction, we often start by _reinventing the form_ and, consequently, we have to
face a very wide range of design problems. Are there cases of software construction that are more
akin to the vernacular or unselfconscious design? One possible area of interest might be how
people solve problems in spreadsheet systems like Excel. Spreadsheets define a relatively fixed
form and allow the user to focus on skilful solutions to specific problems. Thanks to the fixed
form, such specific problem solutions often transfer well between different applications. Perhaps
there are other problem domains where having a fixed form would allow us be more efficient and
allow us to focus on specific problems rather than on reinventing the form, which introduces a
very wide range of challenging design problems.

Christopher Alexander makes a number of useful observations about the process used by
unselfconscious cultures. To paraphrase the main point, the design process is self-adjusting
and produces well-fitting forms by actively maintaining an equilibrium with context. If the
good fit is disrupted in any way, the unselfconscious culture will seek ways of adapting their
practice to resolve the mismatch. For this, two conditions are necessary. First, there must be
enough time for finding the adaptation.

> The adjustment of forms must proceed more quickly than the drift of the culture context
? Unless this condition is fulfilled the system can never produce well-fitting forms, for the
> equilibrium of the adaptation will not be sustained.

The second condition is that the process needs to provide feedback to allow direct
response.

> If the process is to maintain the good fit of dwelling forms while the culture drifts,
> it needs a feedback sensitive enough to take action the moment that one of the potential
> failures actually occurs.

The immediate feedback mechanism allows quick response to design challenges and it prevents
the build-up of multiple failures. Such multiple failures would require simultaneous correction,
which is where a more self-conscious approach to dealing with complexity becomes necessary.

The above analysis might provide us with useful hints on how to design programming tools that
allow users (or programmers) to build software systems in a way that does not require us to
solve complex design problems. We have to start with a standard (or traditional) form that
is sufficiently constrained so that there is no room for complete reinvention of the form.
We then need good feedback mechanisms that reveal misfits between the form and what we try to
achieve. Finally, the process also needs to be slow enough to give sufficient time for making
individual adaptations and prevent build-up of mismatches. This approach would perhaps get us
closer to the dream of end-user development, which seems like the closest software analogy to
vernacular or unselfconscious cultures.

### 3.3 Dealing with organized complexity

In Section 1.1, I argued that there is a similarity between the kind of complexity that urban
planners have to deal with and the kind of complexity that software engineers have to handle.
As pointed out by [David Parnas][sds], software systems are _non-repetitive digital systems_,
meaning that they have an intractable number of state and there is no obvious way of reducing
this number. Similarly, urban planning is a problem of _organized complexity_ and there is no
obvious way of reducing this complexity, say, through statistics.

Urban planners that [Jane Jacobs][life] criticises viewed cities as problems of _unorganized
complexity_ which makes it possible to reduce the complexity using statistical analyses:

> In the form of statistics, [citizens] were no longer components of any unit except the family,
> and could be dealt with intellectually like grains of sand, or electrons, or billiard balls.
> (...) It became possible to map out master plans for the statistical city, and people take these
> more seriously, for we are all accustomed to believe that maps and reality are necessarily related,
> or if they are not, we can make them so by altering reality.

Software engineers and programming theoreticians nowadays seem to me to be a bit like urban
planners of the 1930s. We treat software systems as systems of complexity that can be
reduced, typically via logic rather than using statistics, to allow us to fully understand
the systems we are building. I believe we need to accept that this is infeasible and
[Life and Death of Great American Cities][life] by Jane Jacobs is a good source of ideas about
how to deal with non-reducible complexity. She summarizes her observations by saying:

> In the case of understanding cities, I think the most important habits of thought are these:
> (1) to think about processes; (2) to work inductively; (3) to seek for 'unaverage' clues
> involving very small quantities, which reveal the way larger and more 'average' quantities are
> operating.

An example of unaverage value discussed by Jacobs is the case of a chain of five bookshops with
four locations in New York. Four of those stay open until 10pm or midnight, but one in Brooklyn
downtown closes at 8pm. _"Here is a management which keeps its stores open late, if there is
business to be had."_ The fifth store tells us that Brooklyn's downtown is dead by 8pm, which
is a valuable insight for an urban planner.

I believe programming language researchers have much to learn from Jane Jacobs. When studying
programming languages, we often try to find their _essence_ or _foundations_ and we end up
looking at a reduced version of the problem that hides interesting unaverage properties.
Rather than looking at reduced essence, we should perhaps be taking non-reductionist view
and look at interesting unexpected cases, examples and applications.

## 4. Conclusions

This blog post is by no means trying to present any concrete results or even give any concrete
advice that software engineers or programming researchers could follow. It is perhaps best seen
as my reading notes on four books on architecture and urban planning. Specifically, I talked
about [The Death and Life of Great American Cities][life] by Jane Jacobs, [How Buildings Learn][learn]
by Stewart Brand, [Notes on the Synthesis of Form][notes] by Christopher Alexander and
[The Image of the City][image] by Kevin Lynch.

My motivation for reading those books in the first place is that I find the typical sources of
ideas for programming and programming research of only limited use. Programming imported a number
of useful ideas from sciences, mathematics and engineering, but I believe we need to be looking
further if we want to come up with new and better ways of constructing software. Architecture and
urban planning might be valuable and inspiring sources of ideas and I started this article by
discussing why. There is a number of similarities between those and programming, most importantly
the fact that they deal with a similar kind of complexity.

I then looked at two ways in which programming research could learn from architecture and
urban planning. More generally, the aforementioned books follow interesting methodologies
that would be worth imitating in the context of software. We should look for software that works
well in spite of theory; we should study how software evolves and we should study what image of
code base do programmers keep in their mind.

More specifically, there are also a number of concrete
ideas from urban planning and architecture that could, more or less directly, be applied to
programming. Those are ideas around designing adaptable buildings (and adaptable software),
understanding how vernacular (unselfconscious) design method finds fit between a form and a context
and ways of studying unorganized complexity by focusing on unaverage values in their full richness.

So far, the software industry borrowed the idea of _design patterns_ from architecture, although
many would say that our version of the idea is a mere caricature of what architects envisioned.
I would not be surprised if the next great idea in programming was also inspired by architecture or
urban planning. I'm just curious to see whether it will be one of those discussed in this blog post
or not.


 [arguments]: https://xxx "Rebecca Slayton (????). Arguments that Count"
 [learn]: https://xxx "Stewart Brand (????). How Buildings Learn: What happens after they're built"
 [life]: https://xxx "Jane Jacobs (1961). The Death and Life of Great American Cities"
 [sds]: http://?? "David Parnas (????). Software Aspects of Strategic Defence Systems"
 [notes]: http://?? "Christopher Alexander (????). Notes on the Synthesis of Form"
 [bullet]: http://?? "Fred Brooks (????). No Silver Bullet"
 [naur]: http://?? "Peter Naur (????). The Place of Strictly Defined Notation in Human Insight"
 [farmer]: http://??? "Susanne Bødker, Henrik Korsgaard, Joanna Saad-Sulonen (???). A Farmer, a Place and at least 20 Members: The Development of Artifact Ecologies in Volunteer-based Communities"
 [gpii]: https://www.shift-society.org/salon/papers/2017/revised/externalization.pdf "Colin Clark, Antranig Basman (2017). Tracing a Paradigm for Externalization: Avatars and the GPII Nexus"
 [open]: https://amzn.to/2z5EYI5 "Bertrand Meyer (1988). Object-Oriented Software Construction. Prentice Hall."
 [separation]: https://www.cs.utexas.edu/users/EWD/transcriptions/EWD04xx/EWD447.html "Edsger Dijkstra (1974). On the role of scientific thought."
 [mode]: https://www.upress.umn.edu/book-division/books/on-the-mode-of-existence-of-technical-objects "Gilbert Simondon (1958). On the Mode of Existence of Technical Objects"
 [image]: https://?? "Kevin Lynch (1960). The Image of the City"
 [socioplt]: https://lmeyerov.github.io/projects/socioplt/paper0413.pdf "Leo Meyerovich and Ariel Rabkin (2012). Socio-PLT: Principles for programming language adoption"
