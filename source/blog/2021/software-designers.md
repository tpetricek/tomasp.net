Software designers, not engineers: An interview from alternative universe
=========================================================================

 - title: Software designers, not engineers: An interview from alternative universe
 - date: 2021-04-19T13:30:57.7521867+01:00
 - description: How very different the world of software is in an alternative universe which has
    software designers rather than software engineers? In this interview, we look at a range
    of topics from software design sketching to software designer education, as well as the way
    in which software designers reshape any problem they are asked to solve.
 - layout: article
 - icon: fa fa-monument
 - image: http://tomasp.net/blog/2021/software-designers/ways-sq.png
 - tags: academic, research, design, philosophy, architecture

----------------------------------------------------------------------------------------------------

While the physicists investigate the nature of the mysterious portal that has recently appeared in
North London, several human beings recently came through the portal, which appears to be a
gate into an alternative universe. As we understood from the last two people coming through the
portal, it seems to be a linked with a universe that is in many ways like ours, reached about the
same level of social and technological development, but differs in numerous curious details.
The paths through which people in this alternative universe reached similar results as our world
are often subtly different.

<div class="rdecor"><a href="https://amzn.to/3x0Ww5a"><img src="http://tomasp.net/blog/2021/software-designers/ways.png" style="max-width:260px" /></a></div>

The most recent visitor from the alternative universe is Ms Zaha Atkinson, who would most likely
be titled _software engineer_ in our world, although the title she uses in her home world is
_software designer_. She is a well-known software designer and has been also titled using the
strange-sounding title _softwarenova_, a label that we will soon say more about. As with other
technological and societal developments, the alternative universe seems to have arrived at very
similar results as our worlds. Software is eating the (alternative) world, but it is built in very
different ways. The interview with Ms Zaha Atkinson, presented below, reveals how very different
the world of software is when we think of programmers as software _designers_ rather than as
software _engineers_.

_This article is a work of fiction. Any resemblance to actual events or persons, living or dead,
may or may not be entirely coincidental. It has been largely inspired by the book
[Designerly Ways of Knowing](https://amzn.to/3x0Ww5a) by Nigel Cross. Ms [Zaha](https://en.wikipedia.org/wiki/Zaha%5FHadid)
[Atkinson](https://en.wikipedia.org/wiki/Bill%5FAtkinson) also may or may not be entirely fictional._

----------------------------------------------------------------------------------------------------

<link rel="preconnect" href="https://fonts.gstatic.com">
<link href="https://fonts.googleapis.com/css2?family=Open+Sans:ital,wght@1,300&display=swap" rel="stylesheet">
<style type="text/css">
.interview blockquote { color:inherit; margin:0px; padding:0px; font-weight:bold; }
.interview blockquote p { color:inherit; }
</style>
<div class="interview">

## Software as a design problem

> In one of the magazines for programmers that emerged through the portal, there is a long
> article about you and your work and it calls you a _softwarenova_. I'm curious what this means.

I do not really like the term, but it is a portmanteau inspired by the term [starchitect](https://en.wikipedia.org/wiki/Starchitect)
that is used to describe architects with celebrity status. Someone in our universe though it would
be witty to combine the terms _software_ and _supernova_ to talk about influential software
designers.

> We certainly have more and less known software engineers, but you do not really get a
> celebrity status as a programmer for your programming work. How does that work?

I think we'll have to talk more about the nature of _software design_ first to understand this,
but software design in our world is a certainly a broader activity, which makes it interesting
not just for experts, and it is also more linked to personal experience.

> Okay, let's get back to this later. So what is _software design_ like?

<div class="rdecor">
<p style="font:italic 18pt 'open sans', 'pt sans', sans-serif;line-height:26pt;max-width:300px">
In my universe, we treat the creation of software as a design activity, putting it as a third item on the same level as science and art.</p>
</div>

When you told me that you have the term _software engineering_, I was curious what this means,
so I looked up the definition for _engineering_. It says that _"engineering is the use of scientific
principles to design and build machines, structures, and other items"_. This sounds a terribly
narrow description for talking about software to me.

In my universe, we treat the creation of software as a design activity, putting it as a third
item on the same level as science and art. Somewhat simplistically, science studies the natural
world using experiments and its aim is to discover the truth. Art is concerned with human experience,
it works via metaphors and aims for justice. Design studies the artificial, it does so using the
method of synthesis and its objective is appropriateness.

> That sounds very abstract to me. You already had some time to learn how we do things here,
> so maybe you can help us understand the difference more concretely?

Well, when we start a software project, we think of it in more general terms. A typical form
of what you call _specification_ in our world is a bit more like a [design brief](https://en.wikipedia.org/wiki/Design_brief).
A much more space is dedicated to the context in which the work is happening, the problem that
you are trying to solve and constraints that you are facing, but design briefs say very little
about any specific potential software solution to the problem.

It depends, of course, on the kind of problem you are solving. If it is a business problem, then
it may turn out that the client does not even need a software solution! If you are looking at a
more technical problem, then it is clear that software is the right answer, but a design brief
leaves more room for creative solutions.

So, I think this is why you have this celebrity status associated with software designers, because
good work often has quite visible effect on a lot of people.

## Designerly problem solving

> I can see how this makes sense for, say, the problem of booking holidays online, but what
> if you are solving a more specific technical problem, like sorting a collection?

Hehe, I already noticed that your universe is obsessed with sorted collections!
The thing is, something like this would not work as a design brief, because it is lacking
context. Of course, we also sort things, but nobody would think of it in such abstract terms.

You may want to present a list of countries in a way that makes it easy to find a particular
one if you know its name. How exactly to do this, that is something that software designer
should be free to decide.

> I still feel like this way of thinking can work only up to a certain point. Don't you
> need to get to a more precise problem definition at a lower, more technical level?

Not really, no. When you're solving a problem, even as you get to a more technical level,
you always keep in mind why you are solving it. I noticed that software engineers in your
universe often ignore this. You fixate on some technical solution and then completely forget
what is the context in which you're building it.

> So what are the best practices for problem solving in your universe?

<div class="rdecor">
<p style="font:italic 18pt 'open sans', 'pt sans', sans-serif;line-height:26pt;max-width:300px">
When solving problems that are not inherently ill-defined, you often find better soluti&shy;ons by treating them as such.</p>
</div>

I would say that the key idea is that a large part of problem solving is actually precisely
understanding the problem. Most problems that you face are ill-defined and open to interpretation.
And when solving problems that are not inherently ill-defined, you often find better solutions
by treating them as such!
You would probably say that they are [wicked problems](https://en.wikipedia.org/wiki/Wicked_problem).

This is clearly the case for more high-level software design questions, but in my universe,
good software designers basically treat all problems they face as ill-defined, even if they are
fairly specific. It gives them flexibility and often enables more creative solutions.

In other words, changing the problem is often a key part of your solution. And finding the right
way of formulating the problem is often half of what it takes to solve it.

## Formulating a design problem via sketching

> This is very interesting. So, when you work on a design brief, how do you approach the task
> of formulating a problem?

The problem really only becomes apparent as you try to solve it. So, the key thing is to
quickly iterate. You come up with a new framing for the problem, sketch a solution based on
your framing and see what it looks like. If it looks good to you, you show it to the customer,
or you sketch a range of different prototypes.

> I'm curious to learn more about those sketches you're talking about. Do you mean you draw the
> user interface of a system or something like this? But how does this look for a more technical
> problem?

No, no, no. You're right that sketching in traditional design is typically a drawing, but in
_software design_, we use quick prototyping tools that feel a bit like digital version of a drawing.
I guess you can think of this as something between your [wireframing tools](https://www.interaction-design.org/literature/article/10-free-to-use-wireframing-tools),
rapid application development tools like your Visual Basic and a system based on test-driven
development and logic programming.

If there is a user interface, you can draw that. For logic,
you specify some important inputs and outputs and perhaps a bit of the key logic. This gives you
something that is good enough to illustrate the problem framing. Historically, our software
sketching tools evolved from actual sketching with paper and pen, so they are very flexible and
malleable.

> So do your sketches then evolve into real software somehow?

No, they don't. It is not really what you want! Software sketches are very ambiguous. When
you are sketching, you are omitting a lot of details and so the sketching tool will give you
something that can behave in unexpected ways in certain situations.

This is actually quite valuable, because this ambiguity allows your ideas to evolve. You can
often learn quite a lot about the problem from the things that are underspecified in your
sketches.

## Software design methodologies

> That sounds really fascinating, but let me get back to the thought process that is behind
> the problem framing you mentioned earlier. A key idea in our software engineering is to
> replace ad hoc process of building software with something more systematic.

Yeah, we had similar efforts in our universe and, curiously enough, it started around the same
time! I know your software engineering discipline has been shaped by the 1968 NATO-organized
conference that set off to _"turn programming from black magic into an engineering discipline"._

In my world, there was a [similar event in the 1960s](https://www.drs2016.org/drs-history/2016/3/16/icl-1962)
that attempted to base the design process on objectivity and rationality. Despite some influence
over the next decade or so, the idea of scientific design never really took off. Software designers
of course use scientific methods in some parts of their work, including some ideas that are quite
similar to your computer science, but this is only applicable in vary narrow areas.

> So, is the problem framing just the kind of thing that designers call "flash of insight",
> without any structure?

<div class="rdecor">
<p style="font:italic 18pt 'open sans', 'pt sans', sans-serif;line-height:26pt;max-width:300px">
The most important thing in software design is problem framing
and you need a lot of expertise for that. The best software designers in our world are often
in their 50s or 60s!</p>
</div>

That is still the popular perspective on software design in our world, but the reality is
of course more complex than that. There is quite a lot of research that studies how expert software
designers work and the flash of insight very often has more structure to it.

Very often, the right problem framing makes it possible to see a problem in a new way.
You may see it as a combination of different problems you faced in the past, as a problem that
is an alternative or similar to something you faced before, or as a problem that you can talk
about through certain first principles - typically not mathematical, but equally fundamental.

> It seems to me that many of the ways in which the framing happens is based on past experience.

Yes, and I think this is really the key thing that distinguishes excellent software designers
from the rest. People in my world who are labelled using the witty _softwarenova_ label almost
always bring a unique perspective of a problem. They have both different background and experience,
which means they can often frame problems in unusual ways that reveal new possible solutions.

> This is an interesting point. In our world, most people think that the best programmers are
> young people in their 20s who have a lot of passion and energy...

Yeah, this is a difference I noticed almost as soon as I appeared here! Where I come from, this
is completely ridiculous. The most important thing in software design is problem framing
and you need a lot of expertise for that. The best software designers in our world are often
in their 50s or 60s!

## Softwarenovas and design education

> Is this how one becames a softwarenova then? Mainly by having a different background and a
> lot of experience?

I would say that this is the substantial part of it, yes. But you also need to understand that
software designer is a much more visible public activity. Famous software designers present their
work at large shows, present solutions to large publicly funded design competitions and studying
with them is very prestigious.

> As a frequent attendee of software engineering conferences, I'm very curious to learn
> more about those shows that you just mentioned. Are they anything like our conferences?

They are much more conceptual than that. You can think of it perhaps more as a crossover between
your software engineering conferences and the Venice Biennale. Each year, a show will have
some theme and the presenters submit work that reflects the theme in some innovative way. They
typically produce software that is not exactly practically usable, but that instead shows some
new potential way of thinking about programming. The best shows will shape the way things will
be done in maybe 5-10 years.

> There is one more topic that we briefly touched on, which I wanted to ask about and that is
> education. You said that aspiring software designers want to study with the famous ones.
> How exactly does this work then?

<div class="rdecor">
<p style="font:italic 18pt 'open sans', 'pt sans', sans-serif;line-height:26pt;max-width:300px">
Knowledge about design is hard to externalize. It is embedded in people and in the products they build, but it is not clear how you could turn such knowledge into a textbook. </p>
</div>

Software design education in our world is definitely very different than in yours. A significant
part of education is working with an experienced designer in their studio. In other words,
most design teachers are practitioners who educate through a form of apprenticeship.
This way, you learn the practices, methods and approaches that are so hard to write down in
a textbook, but are absolutely fundamental for being able to frame design problems in the
right way.

Software design education is obviously accompanied with more theoretical aspects. It includes
some amount of theory and history, but you also spend a lot of time learning and studying past
systems and try to reconstruct the reasoning behind their design.

> So, you cannot learn software design from a textbook then?

That's right. Knowledge about design is hard to externalize. It is embedded in people and in
the products they build, but it is not clear how you could turn such knowledge into a
textbook. You acquire it by working with experienced software designers and by studying
existing artifacts.

> My first reaction is that this cannot possibly scale if you want to educate any
> sizeable number of software designers! But I already expect you'll tell me that the
> framing of my question is wrong!

You may be surprised, but you are right. The way we do software designer education is
definitely hard to scale, because it relies on close personal contact with an experienced
mentor. A lot of designing is done by people with limited education, but it is something
many of us are trying to change.

It turns out that technologies like live streaming of the design process make it possible
for people to get some insight into the process and get at least some of the education that
you would get in a studio.

> Does this mean that a lot of software in your universe gets built by people with no
> appropriate education? How do they do this?

Well, it is a bit like with house building. You have a small number of people who will
approach the problem from the first principles and invent a truly new design that improves
on the state of the art in one way or another. But a more traditional house building,
especially as practiced in traditional communities, involves recreating almost the same
existing and well-established patterns.

Software design works in much the same way. You can do it by analysing the problem, framing
it in terms of first principles, and coming up with a radical new solution. Or you can do it
by basically repeating an existing solution with some small tweaks.

> Do you mean repeating, as in recreating the system from scratch, or do you mean
> adapting or somehow abstracting what is already out there?

Even though the digital medium makes it easy to directly copy things, this is actually mostly
done by recreating system from scratch. So you would typically start from an empty program and
construct it step by step, often by importing existing well-tested parts that you see
elsewhere.

You might think that this is not a very workable method. But it is actually very effective,
because of this iterative recreation of artifacts. The design knowledge involved in such work
is often not explicitly communicated, but it is rather embedded in the artifact itself, which
has gone through a number of iterations. Over time, the recreation produces more and more
optimal design.

The role of trained software designers is to rethink the motivating problems and offer new
alternatives that can be both used directly, but also that will influence the next
generation of non-professional designers.

> This was certainly revealing! I have to acknowledge that my idea of how things work
> in your alternative universe is still quite vague, but it definitely is an interesting
> world to explore. If the physicists ever figure out how to control the portal, I would most
> certainly want to come for a visit and see if we can integrate more of designerly thinking
> into our software development processes. Thank you for your time Ms Zaha Atkinson!

</div>
