Where programs live? Vague spaces and software systems
======================================================

 - title: Where programs live? Vague spaces and software systems
 - date: 2023-02-10T23:44:19.4904357+01:00
 - description: Terrain vague in a city is abandoned space without clear purpose. Does it also
    exist in programs and can we learn something useful about reading programs and the space in
    which program exist from the inter-disciplinary study of terrain vague in cities?
 - layout: article
 - icon: fa fa-road-barrier
 - image-large: http://tomasp.net/blog/2023/vague-spaces/zizkov.jpg
 - tags: research, academic, programming languages, design, architecture

----------------------------------------------------------------------------------------------------

Architecture and urban planning have been a useful source of ideas for thinking about programming.
I have written [various blog posts](https://tomasp.net/blog/tag/design/) and a paper
[Programming as Architecture, Design, and Urban Planning](https://tomasp.net/academic/papers/metaphors/)
that argue why and explore some of those ideas. Like urban planning and architecture, the design of
any interesting software system deals with complex problems that can rarely be analysed in full and
with structures that will continue to evolve in unexpected ways after they are created.

My most recent reading on cities was a book [The City Inside Out](https://www.academia.cz/mesto-naruby-2-dotisk--haluzik-radan--academia--2021)
(Czech only, unfortunately) that explore places referred to as [terrain vague](https://www.atributosurbanos.es/en/terms/terrain-vague/).
This term refers to unused and abandoned spaces that have lost their purpose or
do not have a clear use, but are used in various ways nevertheless. For various historical
reasons, there seem to be quite a few of such places in Prague (Figure 1) and, more generally,
Central European cities, which is the focus of the book.

The book is an interesting inspiration for thinking about programming in many ways.
It uses an inter-disciplinary approach ranging from history and philosophy to
[archaeobotany](https://en.wikipedia.org/wiki/Paleoethnobotany), which is much needed for thinking
about programming too. (Not archeobotany, but inter-disciplinary thinking certainly!)
More specifically, it makes you think about the concept of a _space_ in
which cities and programs exist, how the spaces inhabited by the two differ, what would it look
like if they were different and what structures get created in those spaces as a result of
social and technical forces.

<div class="wdecor" style="text-align:center">
<a href="https://iprpraha.cz/projekt/120/nakladove-nadrazi-zizkov"><img src="http://tomasp.net/blog/2023/vague-spaces/zizkov.jpg" style="max-width:100%;border:solid 4px black" /></a>

**Figure 1.** Nákladové nádraží (freight railway station) Žižkov - an example of a large  
space in Prague that no longer serves its original purpose ([photo source](https://hotelove.cz/nakladove-nadrazi-zizkov/))

</div>

----------------------------------------------------------------------------------------------------

## Vague spaces in programs and cities

I will start with the most concrete ideas and look at a couple of ways in which terrain vague
emerges in cities, what is its structure and whether there are similar structures in programs
or programming systems. It is worth saying that this metaphor for thinking about programs is,
just like any other metaphor, not perfect. There are no truly unused structures or structures
that serve no purpose in computer programs (unless you want to talk about dead code) and
the nature of space in which cities and programs exists is different. Still, I think we can get
some interesting ideas from thinking about the similarities.

### Dialectic between the planned and the unplanned

<div class="rdecor" style="text-align:center">
<img src="http://tomasp.net/blog/2023/vague-spaces/naruby.jpg" style="max-width:320px;border:solid 4px black" />
<div style="max-width:320px">

**Figure 2.** [Město naruby (The City Inside Out)](https://www.academia.cz/mesto-naruby-2-dotisk--haluzik-radan--academia--2021), Radan Haluzík, ed., Academia 2021

</div></div>

One way of understanding terrain vague is as _places that have not been intentionally planned_.
In a city, this happens for historical reasons, when an place that had some purpose earlier
no longer serves this purpose. This may be the case for a river port that is no longer used,
abandoned factories or railways and other industrial areas. It also happens at what the authors
of [The City Inside Out](https://www.academia.cz/mesto-naruby-2-dotisk--haluzik-radan--academia--2021)
call _inner borders_. Those borders may be created by a path that cuts through the city and
separates it, such as a railway track, highway or a river, as well as at the boundaries of two
clearly distinguishable regions such as an industrial complex and a residential district.

The main surprising point made in the book is that the terrain vague is not just some temporarily
forgotten area that will soon be redeveloped, eventually leading to a nice and clean "fully
optimized" city. There is a dialectic relationship between the planned (city) and unplanned
(vague spaces) and the two are in a mutual relationship. Creation of planned spaces inevitably
also creates unplanned spaces at the borders or in temporarily unused areas. The existence of
unplanned spaces creates the potential for redevelopment and the building of new planned structures.

Vague spaces around old structures, as well as around carefully designed new structures also
exist in software systems.

### Historicity of unplanned spaces

<div class="rdecor" style="text-align:center">
<img src="http://tomasp.net/blog/2023/vague-spaces/brana.jpg" style="max-width:400px;border:solid 4px black" />
<div style="max-width:400px">

**Figure 3.** [Gate of No Return memorial](https://en.wikipedia.org/wiki/The_Holocaust_in_Bohemia_and_Moravia) at Praha-Bubny railway station
commemorating the deportation of tens of thousands Jews via the station ([image source](http://www.tady.cz/dadajena1/120.htm))

</div></div>

The first and the most obvious source of unplanned spaces in cities is history. As cities evolve,
large harbours, freight railway stations and industrial complexes are no longer needed for their
original purpose and become vague spaces. Often, these also serve as reminder of troublesome
history as is the case with the [Bubny-Zátory](https://iprpraha.cz/project/66/bubny-zatory) district
in Prague (Figure 3):

> Terrain vague refers to the state of memory of the society, more exactly to its
> displaced parts and sometimes even directly to societal traumas.
>
> <div style="text-align:right;margin-top:10px"><p style="display:inline-block;max-width:400px">
> (Terrain vague as social memory and social mirror
> Radoslava Schmelzová in The City Inside Out)</p></div>

One interesting aspect of such historical spaces is that they do not follow the modern logic
that is accepted in the rest of the city. The spaces had their own logic, but this may not be
commensurable with our modern thinking about the city:

> A part of the well-being of each system is the presence of empty spaces, existing outside of
> the syntax of the system. Spaces, which preserve the fragments of older syntaxes (...) or
> spaces that simply remind us of the possibility of other syntaxes and of the existence of
> forces that shape our order as well as other orders.
>
> <div style="text-align:right"><p style="display:inline-block;max-width:400px">
> (Empty spaces in cities (and their analogy), Michal Ajvaz in The City Inside Out)</p></div>

I believe the same is the case for large software systems. When a system is around for long
enough, it will include legacy components that have not been fully replaced in recent rewrites
or that are necessary for some arcane legacy reasons like the [memory allocator hack
in Windows 95](https://www.joelonsoftware.com/2000/05/24/strategy-letter-ii-chicken-and-egg-problems/)
added to make SimCity work.

The legacy code often has its own inner logic that may not be compatible with the new design
and, as a result, requires various wrappers to be created. Such wrappers are quite like
the terrain vague in cities. They are the result of historical processes and do not have their
own purpose (in the sense of providing specific functionality), but are needed to make the new
design and new logic work.

I experienced this myself when working on a project that was replacing an existing data
manipulation library in a C# system with the F# data frame library [Deedle](https://fslab.org/Deedle/).
After replacing most of the existing logic with Deedle, we inevitably found a place where
this was not easily doable and so fully ripping out the old code was not (an easy) option.
But after some more investigation, it of course turned out that there was yet another bit of
code for working with data tables, used only in one or two places, that was a leftover of an
even earlier version of the system and was left in place for the very same reasons. I admit this
is not a story to be proud of, but I'm afraid it is way too common and real.

In those legacy parts of code base, we find the aforementioned spaces, _"which preserve the
fragments of old syntaxes"_ and _"remind us of the existence of forces that shape our order."_
Just like we need to look at history in order to make sense of vague spaces, we often need to
look at history in order to understand legacy code bases and their (now forgotten) inner logic.
As with the terrain vague in cities, legacy bits of code surrounded by layers of wrappers
also sometimes refer to [team] traumas.

### Unplanned spaces around inner borders

<div class="rdecor" style="text-align:center">
<img src="http://tomasp.net/blog/2023/vague-spaces/zlin.jpg" style="max-width:400px;border:solid 4px black" />
<div style="max-width:400px">

**Figure 4.** [Zlín, a "hypermodern industrial city with functionalist character"](https://en.wikipedia.org/wiki/Zl%C3%ADn)
with a separation between residential and working districts and vague spaces along the borders.
([image source](https://www.denik.cz/regiony/region-zlinsko-bude-bez-zlina-tratit20110411.html))

</div></div>

The second common source of vague spaces in cities are inner borders, either along linear paths
or between districts with different inner logic. Modernist cities which assign different
functions to separate districts would be one such example (Figure 4).
To quote from The City Inside Out (with my poor translation):

> This conceptualization of the terrain vague as the meeting point of various kinds of order (...)
> [is an] adequate description of its physical form. (...) [Such spaces] are characterized
> by localization at the interface between different parts or functions of the city (...).
> These are places where it is difficult to predict what (...) will be around the corner.
>
> <div style="text-align:right"><p style="display:inline-block;max-width:400px">
> (Terrain vague as a blind spot of urban planning, Cyril Říha in The City Inside Out)</p></div>

We can definitely find similar interfaces between different parts of a system in software systems.
In the [microservice architecture](https://en.wikipedia.org/wiki/Microservices), each microservice
is an independent component with its own function and own internal structure and logic. However,
putting all those services together requires a lot of integration and infrastructure, or code
that does not have any particular (application-specific) purpose.

Terrain vague in this case would be all the tedious parsing, data marshalling, messaging and
logging that has to be there in order to make the components with neat logical inner order work.
(This also sounds similar to the idea of "dark twins" from [Scott's Seeing Like a State](https://amzn.to/3XeVSNm)
which are informal structures and practices that inevitably supplement modernist designs in
order to make them actually work in practice.)

The authors of The City Inside Out see "terrain vague as an unintended consequence of a master plan"
of the city. In other words, any attempt to produce a logical order in a part of the city will
also result in the inevitably vague borders around it. Moreover, the more rigid the structure
is (the more zoning constraints there are), the larger and more vague spaces emerge around it.

This seems to be the case with software systems as well. The more clearly you try to separate
individual components and the more freedom they have in defining their own inner logic, the
more "vague code" such as parsers, messaging, wrappers and integrations are needed:

* In a microservice architecture, each service can not just use its own logic, but even a
  different language. The consequence is that each service also needs to parse and format data
  it is exchanging into a common format like JSON, requiring more "vague code".

* In a software system in a single language, organized using modules, some of the logic is the
  same across all the modules (they use the same language and may directly exchange data).
  There may still be some vague code to initialize and connect all the components.

* In a "spaghetti code" where code is not clearly separated into components, there is no
  clear overall logic, but this also eliminates a lot of the "vague code" that is needed in
  more structured approaches.

Having more freedom in defining a logic for individual components leads to more vague spaces
as illustrated by the socialist housing estates in Prague (Figure 5) that were often greenfield
developments with no restrictions imposed by neighbouring districts. In the world of software,
various constraints arguably also lead to smaller amount of vague code. You will probably not
find a lot of "vague code" in the [source code of Wolfenstein 3D](https://github.com/id-Software/wolf3d)
and other early highly optimized games. There just wasn't any "space" for it.

<div class="wdecor" style="text-align:center">
<img src="http://tomasp.net/blog/2023/vague-spaces/jizak.jpg" style="max-width:100%;border:solid 4px black" />
<div style="max-width:80%;display:inline-block">

**Figure 5.** [Jižní Město housing estate](https://en.wikipedia.org/wiki/Ji%C5%BEn%C3%AD_M%C4%9Bsto),
built in the 1970s in Prague. This was a "radical" greenfield development with very few space constraints,
making room for a lot of vague spaces (photo by Jaromír Čejka, [source](https://vikend.hn.cz/c1-65448130-jizni-mesto-ctyricet-let-betonovych-krabicek-od-sirek-ktere-jako-by-nekomu-vypadly-z-kapsy))

</div></div>

## Comparing program and city space

One objection against what I wrote about structures emerging in a city and structures emerging
in software systems is that the nature of the space in which they exist is different. In particular,
terrain vague in cities often exists in now fairly central areas that were once peripheral.
Because the nature of the space is fixed, terrain vague ends up occupying valuable central space.
No such fixed structure of space exists in programs, at least not at the first sight.

* **City space.** Cities exist in (more or less) two-dimensional fixed space. Areas of that
  space tend to be (temporarily) occupied by various districts, facilities or regions. There are
  physical borders between those areas and the areas evolve in time. Some of them lose their
  earlier function and become available for new uses while some expand.

* **Program space.** Programs in conventional programming languages exist in space that can be
  arbitrarily created. There are links between components (method calls, nesting), but you can
  always allocate space for a new component (add a file or a class to a file) and create new links
  between existing areas and the new one. If an area is no longer needed (dead code) it can be
  removed and disappears from the space.

### Space in non-conventional programming systems

The above may be the case for current conventional main-stream programming systems, but
there are quite a few systems that use space that is not as infinitely expandable as that in
conventional programming languages. Here are a few examples:

* **BASIC on Microcomputers** where you have to prefix each line with a line number uses
  a sort of one-dimensional space. If you leave enough gaps in your numbering scheme, you can
  expand the space and you can also attach more space with `GOTO`, but you have to think about
  space when programming (and perhaps have different components in different line ranges).

* **Dynamicland** ([read more](https://dynamicland.org/)) embeds computation in ordinary physical
  space. (As far as I understand, there is still a lot of ordinary code too, but a large-scale
  computation taking a whole room would certainly have its vague spaces!)

* **Scratch and visual programming languages** more generally typically exist in fixed (sometimes
  expandable) two-dimensional space. In something like Labview, the layout may correspond to
  physical layout of instruments. In Scratch, the two-dimensional space is (as far as I can see)
  not used in any meaningful way - a missed opportunity perhaps!

* And finally, **Spreadsheets** (Figure 6) are the obvious example of a programming system that
  exists in a space with interesting characteristics. Spreadsheet programs exist in three-dimensional
  space (grid + multiple sheets) where every location has a known address. The space is fairly
  fixed, although you can insert columns and rows.

Out of these, BASIC and Spreadsheets are perhaps the two that would deserve a more detailed
analysis, because people have used them to create complex systems that evolved over time.
I'm pretty sure that there are some, if unwritten, guidelines for how to use the limited space
effectively when programming so that you avoid having clashes between districts with different logic.

I'm not familiar enough with either of those to say whether there are vague spaces in
BASIC or Excel programs at the border between different areas with inner logic. (Though I certainly
have used the space just around a table in an Excel spreadsheet for adding random notes that
would not fit into the table or for off-hand calculations like currency conversions, so perhaps
those are good examples of terrain vague in spreadsheets.)

<div class="wdecor" style="text-align:center">
<img src="http://tomasp.net/blog/2023/vague-spaces/excel.png" style="max-width:100%;border:solid 4px black" />
<div style="max-width:70%;display:inline-block">

**Figure 6.** [The UK 2050 calculator](http://2050-calculator-tool-wiki.decc.gov.uk/)
is an Excel spreadsheet that "enables users to experiment with many different ways of
meeting the UK’s target to reduce emissions 80% by 2050".

</div></div>

### Imagining new kinds of spaces

It would be interesting to characterize the different properties of the spaces used
by the above (more interesting) programming system and think about what those properties
allow. (And see if any of them correspond to properties of [Topological
spaces](https://en.wikipedia.org/wiki/Topological_space) known in mathematics.)
A space in which program exists may be:

* **Expandable,** that is you can allocate more space in between existing things if you need
  to insert additional logic. This is true in conventional languages, to a limited extent in
  BASIC and often involves manually moving things around in visual languages.

* **Zoomable.** You can "zoom in" to see more details. I'm not sure if any programming system
  does this in a satisfactory way - but I would like to be able to view program in space in
  a high-level way and be able to focus on details as needed.

* **Addressable,** meaning that you can uniquely identify locations in the program. This is the
  case for spreadsheets and BASIC. In conventional programs, you can only address certain locations
  (methods). An interesting case is addressing of DOM elements using CSS, which makes it possible
  to address pretty much any part of the DOM space, but without primitive indices as in the case
  of BASIC line numbers.

I believe that if we think about the properties of space in which programs may exist more
deeply, we can come up with interesting alternatives to the conventional "infinitely expandable"
space. There are clear benefits of having more limited number of dimensions (you can better
visualize it), addressability and zoomability.

I do not expect we will ever be able to live in a city that exists in a space with characteristics
other than those of our usual physical space. Yet, it would be fun to see what would the behaviour
of the city be if someone created a city-building game (Figure 7) where you can arbitrarily expand
space or create areas that are _bigger on the inside_.

<div class="wdecor" style="text-align:center">
<img src="http://tomasp.net/blog/2023/vague-spaces/simcity.png" style="max-width:100%;border:solid 4px black" />
<div style="max-width:600px;display:inline-block">

**Figure 7.** SimCity 2000 game screenshot. Could a game like this explore what cities
would look like in a more flexible space?

</div></div>

## Ways of looking at computer programs

The authors of the book talk about a couple of feelings that are typical for terrain vague. One
of those is the feeling of emptiness. When you enter a vague space in a city, it seems that
there is not much there. But when you talk to locals, you'll soon find out this space has
many uses. The feeling of emptiness does not come from a true emptiness of the space:

> It is not so much an emptiness, but a lack of central unified code, or even (as we may add here)
> unified sense and function, through which spaces we typically read, and perceive, spaces in a city.
>
> <div style="text-align:right;margin-top:10px"><p style="display:inline-block;max-width:400px">
> (Introduction: Looking at the city from the inside out,
> Radan Haluzík in The City Inside Out)</p></div>

<div class="rdecor" style="text-align:center">
<img src="http://tomasp.net/blog/2023/vague-spaces/prazelenina.jpg" style="max-width:500px;border:solid 4px black" />
<div style="max-width:500px">

**Figure 8.** [Prazelenina](https://prazelenina.cz) - a community garden in Praha Holešovice, in
a vague space near a former port ([image source](https://prazelenina.cz/pages/galerie/))

</div></div>

The authors talk about multiple overlapping layers of meaning in vague spaces. The old use of the
space (and the structures that remain from those times) are interleaved with new emerging uses,
both official and unofficial (Figure 8). When passing through the space, you can view it
very differently depending on your perspective, none of which is quite in line with typical
thinking about orderly city.

Again, I find some of those perspectives very useful for thinking about programs. Like in cities,
there are spaces that have _multiple overlapping layers of meaning that are not obvious to see
and that may be read differently depending on how you look.

### Clashes of narrative strategies

One example of the different reading of a space discussed in the book is a clash between
locals who view the vague space as a park (and want it preserved) and planners who see it as
brownfield (and want to redevelop it). This is an example of epistemic struggle between two
perspectives that interpret the space through a different language and live in different
(incommensurable) worlds.

<div class="rdecor" style="text-align:center">
<img src="http://tomasp.net/blog/2023/vague-spaces/pi.png" style="max-width:400px;border:solid 4px black" />
<div style="max-width:400px">

**Figure 9.** [roemer.c](https://www.ioccc.org/1989/roemer.c) from the
Obfuscated C Contest (1989), which plays with the contrast between human and computer
meaning of code (<span style="text-decoration:underline dashed" title="The code prints the number 'e'.">hint</span>).

</div></div>

The basic similar overlapping layers of meanings in software systems is the meaning as intended
for the human reader of the code (supported by the names and comments) and the meaning as
intended for the machine executing the program (to which all names and comments can be ignored).
We typically try to keep those aligned (in a somewhat tricky sense), but you can also creatively
abuse this multi-layered meaning (Figure 9).

Another basic distinction is between the programmer and the user of a system. The way programmer
looks at software system, while being aware of its internal logic, will inevitably be quite
different from the look of a user.

This is likely the case even between a different kinds of programmers looking at code. As someone
interested in understanding the mathematical model that code implements, you will be looking at
very different aspects of code than someone interested in reliable and reproducible deployment.
Perhaps the epistemic struggles of terrain vague in cities can help programmers understand
such issues about reading code better.

### Exploring vague spaces

Reading code and finding your way through it is another topic that can take inspiration from
city planning. It is also something that [I have looked at before](https://tomasp.net/blog/2020/cities-and-programming/)
(section 2.3), but focusing more on how could code be organized in order to be more legible.
With terrain vague, we are facing spaces that are not (immediately) legible and that we want
to explore in order to find somewhat unexpected and hidden points. Methodologies for doing this
may suggest interesting ways of looking at code, perhaps complementing the obvious way
where you focus on making sense of the program logic.

In "How we researched the places in-between", the authors of The City Inside Out talk about
various methodologies that they have used in order to map the vague spaces in a city.
The idea, inspired by the [Dérive strategy](https://en.wikipedia.org/wiki/D%C3%A9rive) is
to explore the urban space in some systematic way that explicitly avoids using the typical
strategies for moving in space (such as the ones you use when going to a shop).
Their various strategies range from one where you take snapshots of the space and "zoom in"
on what you find interesting to regular or random sampling of the physical space as well as
to a strategy where you try to follow a fixed line through the space as closely as possible.

Code of an unfamiliar software system could likely be read in similar ways:

* **Zoom-in.** Start at the entry point, pick an interesting method or a function call,
  look at that and continue until you reach a function with no further (internal) calls.
  After a few iterations, you will likely get a good sense of how deeply nested (and complex)
  or uniform the system is.

* **Random sampling.** Just look at a number of randomly generated locations. You will likely
  see how much "core program logic" there is when contrasted with other things like tests,
  infrastructure, build scripts and so on.

* **Regular sampling.** With more regularity in your samples, you can probably get a sense
  of the overall terrain. Are there many different areas, one big uniform space with a few
  additional (differently looking) spaces?

* **Following a line.** Following the linear order of the code alphabetically (or in order
  defined by the language) is probably less interesting, though it will certainly reveal
  characteristics of languages that require [strict dependency ordering](https://fsharpforfunandprofit.com/series/dependency-cycles/).
  Perhaps there are other lines one could follow though!

All of these are probably interesting complements to the practice of "close reading" that
exists in [critical code studies](https://en.wikipedia.org/wiki/Critical_code_studies). In close
reading, one typically focuses on careful reading of a small snippet of code, unpacking its many
connections with the social and cultural context. With the above, you may get a somewhat more
technical perspective, but still one that forces you to not think just about the obvious
question - what the code does - and understood more about how it has been written and what are
its non-functional characteristics.

## Potential and limitations of the city metaphor

In place of conclusions, I want to return to the dangers and virtues of using metaphors for
thinking about programs and programming. There are certainly some ways in which thinking
about terrain vague in cities can provide useful ideas for thinking about programs. I think
seeing various wrappers (around sub-components or historical code) is one useful idea.
Finding new strategies for navigating through and reading code is another one. Perhaps most
importantly, the analysis of terrain vague in The City Inside Out is a nice example of how you
can approach one problem from a wide range of perspectives. Similarly, analysis of programs
should range from technical to historical, literary and philosophical.

There are also some limitations that are apparent in my analysis. The most obvious one,
that I discussed extensively, is that the nature of space in conventional programming
languages is quite different from that of cities. In most programming systems, terrain
vague is not occupying valuable central space that you would want to reclaim and use for
redevelopment (as in the case of cities). But perhaps, if we had programming systems
where this was the case, they would more explicitly require maintenance and, consequently,
we would end up with long-living high quality systems that do not become obsolete.
