What can routers at Centre Pompidou teach us about software evolution?
======================================================================

 - title: What can routers at Centre Pompidou teach us about software evolution?
 - date: 2023-12-07T18:30:00.8577507+01:00
 - description: The later addition to the building is a perfect metaphor for the typical problems
     of software development. The building uses a colour scheme to indicate its different basic
     structures, but the routers, being a recent addition, do not fit the scheme. Can we come up
     with a way of building buildings and software that makes such large-scale adaptations
     possible? What would a Centre Pompidou plan look like if it was supposed to accommodate the
     late addition of new basic networks?
 - layout: article
 - icon: fa fa-archway
 - image: http://tomasp.net/blog/2023/pompidou/router-sq.jpg
 - tags: research, academic, programming languages, design, architecture

----------------------------------------------------------------------------------------------------

Back in June, I was in Paris for the [NewCrafts conference](https://ncrafts.io/) to talk about
[the growing opacity of software systems](https://vimeopro.com/newcrafts/newcrafts/video/842234359).
This was fun, partly because NewCrafts is a fantastic conference (you can already [get your
tickets](https://ncrafts.io/) for 2024!) and also partly because my talk (arguing against many
established "good engineering" practices) was in many ways arguing for the exact opposite than
one of the keynotes, leading to many interesting conversations.

While in Paris, I also visited the [famous Centre Pompidou](https://www.centrepompidou.fr/en/).
Perhaps to the dismay of many modern art lovers, I spent a lot of time staring at the ceiling
looking for routers.

<div class="wdecor" style="text-align:center;margin-bottom:50px">
<img src="https://tomasp.net/blog/2023/pompidou/router1.jpg" style="margin:10px; max-width:30%;border:solid 4px black" /></a>
<img src="https://tomasp.net/blog/2023/pompidou/router2.jpg" style="margin:10px; max-width:30%;border:solid 4px black" /></a>
<img src="https://tomasp.net/blog/2023/pompidou/router3.jpg" style="margin:10px; max-width:30%;border:solid 4px black" /></a>
<p>Spot the routers at Centre Pompidou!</p>
</div>

----------------------------------------------------------------------------------------------------

## Routers at the Centre Pompidou

The iconic building is sometimes called "inside out" or "high-tech" building. It exposes the
typically (more or less well) hidden infrastructure of the building, putting much of it on the
exterior, and uses a colour coding to ["enliven its facades and outline its structure"](https://www.centrepompidou.fr/en/collection/our-building) using:

<ul>
<li style="margin-bottom:0px"><strong><span style="color:#005d98;">Blue for air flows (air-conditioning)</span></strong></li>
<li style="margin-bottom:0px"><strong><span style="color:#c7a400;">Yellow for electricity</span></strong></li>
<li style="margin-bottom:0px"><strong><span style="color:#007318;">Green for water circuits</span></strong></li>
<li style="margin-bottom:0px"><strong><span style="color:#a70000;">Red for pedestrian flow (escalators and lifts)</span></strong></li>
</ul>

You can see all of these colours in the above photos and they definitely give the building its
iconic look. But it is also clear that there are things that do not fit into the nice colour
scheme designed when the building was built in the early 1970s. Routers are definitely one of
those things!

Once you start looking, you will find quite a few routers in various random places in the building.
They are typically somewhat near the yellow-coloured electricity racks. But they are in other
places too. Sometimes, there is no electric rack nearby (and you need a lot of routers for a
decent wifi) and sometimes the electric rack is behind something (like the green water pipe)
and so you find routers sticking from the ceiling on various kinds of extensions, attached
to green water pipes, blue air pipes or whatever else was available.

This is, of course, a beautiful metaphor for software construction. When we build software
systems, we also understand the requirements at the time of construction, come up with a good
way of structuring software and then build the system according to this structure. In software
development, we now know that requirements change over time and we have development methodologies
that make it possible to adapt systems. But, are we really refactoring systems in ways that are
like adding a new colour to the Centre Pompidou building in order to reflect the need for a new
kind of network? Or are we instead attaching routers to whatever pipe is available in the right
location?

Modifying a physical building is certainly in some ways harder than modifying software system,
so the metaphor is not perfect. You can do modifications while the existing version of the
software system is being used without "closing it down for maintenance".
Centre Pompidou itself will, in fact, [close for renovations for 5 years](https://www.centrepompidou.fr/en/the-centre-pompidou/renovation-programme).
When it reopens in 2030, will there be a new colour for the internet? And how soon before a new
kind of infrastructure will be needed?

## If they asked me to build it...

<div class="rdecor" style="text-align:center">
<img src="brand.jpg" style="max-width:400px;border:solid 4px black" />
<div style="max-width:400px">

[How Buildings Learn](https://en.wikipedia.org/wiki/How_Buildings_Learn) by Stewart Brand

</div></div>

Software engineers are well known for giving advice about things we do not have a clue about,
so here is my contribution! How should a building like Centre Pompidou should be built so that
it does not end up with routers sticking out from the ceiling?

In [How Buildings Learn](https://en.wikipedia.org/wiki/How_Buildings_Learn)
(a book I already referenced [in a blog post](https://tomasp.net/blog/2020/cities-and-programming/)
and a [related paper](https://tomasp.net/academic/papers/metaphors/)),
Stewart Brand looks at how buildings evolve after they are built. They need maintenance and
(somewhat) continuous attention, but this is hard to teach:

> Too often a new building is a teacher of bad maintenance habits. After the initial shakedown
> period, everything pretty much works, and the owner and inhabitants gratefully stop paying
> attention to the place. (...) It might be better if some of the original work were intentionally
> ephemeral, with everyone knowing it will require maintenance or replacement within a year.

Anyone who was a maintainer of a project based on the JavaScript ecosystem knows that this
applies to software too. If you leave a project untouched for a couple of months and run
`npm update`, you will probably have an unpleasant time. If you, instead, get into the habit
of running `npm update` every month, you will probably be able to cope.

But can this idea be generalized to (building or software) construction? Imagine that you want
to build the Centre Pompidou in a way that will make it possible to adapt it when it turns out
that a new kind of infrastructure is needed. The way to do this may be to plan it so that you
start with just some of the infrastructure, knowing in advance that you will need to add more
soon. What if you build the building with just the yellow and green parts (electricity and water)
and intentionally did not, in advance, plan the air flows and pedestrian flows? You would need
to design the basic structure to be adaptable in a way that accommodates any possible design
of the structures that you will need to add later.

I admit, this is probably not a way to build an iconic post-modern building, but can we
learn from this idea to make building of software systems easier?

## Is there a silver bullet?

<div class="rdecor" style="text-align:center">
<img src="bullet.gif" style="max-width:340px;border:solid 4px black" />
<div style="max-width:340px">

[No Silver Bullet (illustration)](https://ieeexplore.ieee.org/document/1663532) by Fred Brooks

</div></div>

In a joint presentation that I did recently with [Joel Jakubovic](https://programmingmadecomplicated.wordpress.com/)
at [Mycroftfest](https://dorchard.github.io/mycroftfest), we were talking about Joel's research
and also the guiding question of whether programming can be made easier. The talk will (hopefully)
turn in the paper, but in the meantime, I will borrow one idea that is relevant to the routers
at the ceiling of Centre Pompidou.

If we want to imagine a way of making software development easier, we face the challenge
posed by Fred Brooks in his [No Silver Bullet](https://ieeexplore.ieee.org/document/1663532)
article:

> There is no single development, in either technology or management
technique, which by itself promises even one order-of-magnitude
improvement within a decade in productivity, in reliability, in simplicity.

Brooks' claim is based on an insightful analysis. He distinguishes between _essential complexity_,
which is the complexity resulting from the inherent difficulty of the real world problem and
_accidental complexity_, which results from our imperfect tools. We can reduce the accidental
complexity (by building better tools), but the essential complexity will remain. But unless the
ratio between accidental and essential is more than 10:1, reducing the accidental complexity
(even to 0) will not yield an order of magnitude improvement.

There is no way of avoiding the essential complexity in big and complex software systems. But what
if there is? In the talk with Joel, we suggest that perhaps software could be build by a sequence
of gradual adaptations, starting from a simple initial system. Arguably, this is how software
systems today often evolve anyway. You start with something that is relatively simple and
gradually add features to it over time. (In other words, you do not typically plan a software
release on a CD a couple of years in advance these days!)

If we take the idea of gradual adaptations seriously, we can avoid the Brooks' challenge.
But one important part of the solution is that we should not need to fully understand the
entirety of the software system we are modifying each time we are making a change. In other words,
if we build a Centre Pompidou skeleton with just electricity and water, we should not need to
worry about how those structures work when we are adding other aspects later. (The problem
is that the structures interact, so they need to be built in a flexible enough way - and likely
expensive and wasteful way - to accommodate all possible interactions that may be needed later.)

But if we can build software in a way where we start with initial scaffolding and then gradually
add structures to it with the help of new kinds of tools that help us understand where they may
fit, we could perhaps spread out the (inevitable) essential complexity over time and make software
construction, in the long term, an order of magnitude easier.

## In lieu of conclusions

The routers sticking from the ceiling at Centre Pompidou are a symptom of little flexibility
and too much up-front design that naively thinks we can understand the problem well enough to
make unchangeable structural decisions. In principle, software should be more adaptable, but it
typically is not. Any large software system will have "patches, ad hoc constructions, band-aids
and tourniquets, bells and whistles, glue, spit and polish" (to quote [De Millo, Lipton and
Perlis](https://dl.acm.org/doi/10.1145/359104.359106)) that were not intended in the original
clean design.

How can we avoid this? What would be the software equivalent of building just the "yellow and
green" structures of Centre Pompidou, so that adding the "blue and red" structures teaches
us how to add more structures in case we need "purple" for the wifi? Late binding as envisioned
in languages like Smalltalk (and the follow-up research project, including Joel's favourite
[Open, extensible object models](https://tinlizzie.org/VPRIPapers/tr2006003a_objmod.pdf))
is one part of the answer. But I think we also need different way of thinking about programming.
We need architectures that are designed for adaptation. And perhaps, we also need to
build software in a less optimized and more wasteful way, because that is the only way of
making sure that future adaptations and additions will be possible in a way unconstrained by
earlier design decisions.
