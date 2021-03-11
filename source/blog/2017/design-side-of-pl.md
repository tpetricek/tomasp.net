The design side of programming language design
==============================================

 - title: The design side of programming language design
 - date: 2017-09-12T17:42:38.3286628+01:00
 - description: The word design is often used when talking about programming languages. In fact, 
    it even made it into the name of one of the most prestigious academic programming conferences.
    Yet, it is almost impossible to come across a paper about programming languages that uses design 
    methods to study its subject. In this article, I want to convince you that this is a missed
    opportunity.
 - layout: article
 - icon: fa fa-drafting-compass
 - image-large: http://tomasp.net/blog/2017/design-side-of-pl/bauhaus.jpg
 - tags: academic, research, programming languages, philosophy

----------------------------------------------------------------------------------------------------

<div style="text-align:center">
<a href="http://amzn.to/2gVxn8W">
  <img src="http://tomasp.net/blog/2017/design-side-of-pl/parsons.jpg" class="rdecor"
    style="max-width:220px;margin-left:30px;margin-top:20px;margin-bottom:20px;width:75%" /></a></div>

The word "design" is often used when talking about programming languages. In fact, it even made
it into the name of one of the most prestigious academic programming conferences, [Programming 
Language Design and Implementation (PLDI)](http://www.sigplan.org/Conferences/PLDI/). Yet, it is
almost impossible to come across a paper about programming languages that uses design methods to 
study its subject. We intuitively feel that "design" is an important aspect of programming 
languages, but we never found a way to talk about it and instead treat programming languages as
mathematical puzzles or as engineering problems.

This is a shame. Applying design thinking, in the sense used in applied arts, can let us talk 
about, explore and answer important questions about programming languages that are ignored when
we limit ourselves to mathematical or engineering methods. I think the programming language 
community is, perhaps unconsciously, aware of this - one of the reviews of [my recent PLDI
paper](http://tomasp.net/academic/papers/fsharp-data/) said _"this is a nice, novel design paper, 
and the community often wants more design papers in our conferences"_. The problem is that we
we do not know how to write and evaluate work that follows design methodology.

To better understand how design works, I recently read [The Philosophy of Design](http://amzn.to/2gVxn8W)
by Glenn Parsons. The book perhaps did not answer many of my questions about design, but it did give 
me a number of ideas about what design is, what questions it can explore and how those could be 
relevant for the study of programming languages...

----------------------------------------------------------------------------------------------------

> _<i class="fa fa-hand-o-right" style="font-size:110%;margin:0px 5px 0px 0px"></i>
> If you are interested in new ways of thinking about programming, please join our new
> [Shift Society mailing list](https://groups.google.com/forum/#!forum/shift-society). We'll
> use it to share announcements about events like [Salon des Refusés](https://refuses.github.io/)
> and provide space for discussions on the future of programming._

## Is "programming language design" a design activity?

First of all, is it useful to explore how design thinking applies to programming languages?
Perhaps the word "design" in the term "programming language design" is misleading and it really
is just engineering and mathematical task. In the first section, Parsons tries to answer the 
question "What is design?" and many of the points made in the section made me think of programming
languages, so I believe (some aspects of) programming language design are, indeed, design activities.

### How design changes the world

One of the very broad definitions of design says that "design [is] the intentional creation of 
plans for a new kind of thing". This is perhaps too broad, but it captures an important fact - 
design is relevant when you are trying to create something innovative and new that did not exist
before. For me, this is the most interesting part of programming languages, but it is the one that
we rarely talk about. Instead, we evaluate (prove, measure, compare) different aspects of a 
language once it is created, but we almost never discuss how that creation happened.

> Design produces items that have the _primary_ function of altering the world, rather than 
> explaining it, whereas in sciences, the primary function is explanation.

The iPhone was a successful design work, because it changed the world by creating a new kind of 
thing. It changed how most of us live today. For me, programming language design is interesting 
for the same reasons. It has the potential to transform how we interact with computers and, 
possibly, the world. 

### How programming languages change the world

Object-oriented programming certainly changed how we think about and construct programs. In case 
of Smalltalk, this was a part of [more ambitious Dynabook design vision](https://en.wikipedia.org/wiki/Dynabook), 
yet, when you read the [Wikipedia page on Smalltalk](https://en.wikipedia.org/wiki/Smalltalk), it 
spends half of the page on syntax and control structures and says very little about the design
thinking behind it. Similarly, I find type providers interesting because they change how we interact 
with data and can perhaps [democratizing access to data](https://thegamma.net/), yet, 
[my aforementioned paper](http://tomasp.net/academic/papers/fsharp-data/) was accepted because
it had sufficiently convincing proofs.

Once we start looking at programming languages through the design perspective, we can start 
thinking about the principles behind a particular design and the subtle influences that such
design decision has on the world. Let's look at two examples.

## Exhibit 1: Abstraction and design

<img src="http://tomasp.net/blog/2017/design-side-of-pl/table.jpg" class="rdecor"
  style="max-width:400px;margin-left:30px;margin-top:20px;margin-bottom:20px;width:calc(100% - 60px)" />

Design may seem merely a matter of aesthetics, but this is not the case. The way everyday
things are designed can have subtle influence. A nice example mentioned by Parsons is the 
design of a dining table: 
  
> The design of a dining table might seem to be a mere aesthetic choice, but this is not quite so. 
> At a rectangular table, someone sits at the "head position," whereas at a round table, everyone 
> has an equal position. The effect is subtle, something that the people involved may not even be 
> aware of, but differences of hierarchy and status are often shaped and maintained by just 
> such subtle influences.

This is a great example of an influence of design that you will not realize unless you explicitly
think about design. Exploring the consequences of design choices on how people think about and
use programming languages seems like one interesting area of work. 

### Subtle influence of design

The fact that a table is rectangular does not mean that people involved cannot be equal partners, 
but its subtle influence makes this unlikely. Similarly, a programming language can make certain
things possible, but it can be designed in a way that makes people unlikely to use them. As a 
simple example, if you declare mutable variables using `let mutable` as opposed to just `let`, 
you will probably use immutable variables more often. 

This is a simple obvious example, but it is an example of design aspect of programming language 
that we rarely talk about. Are there more notable examples of the subtle influence of design
in programming languages? I think abstraction is one of them...

### Abstraction and power

Many programming languages provide some mechanism for [abstraction](https://en.wikipedia.org/wiki/Abstraction_(software_engineering)).
In object-oriented languages, you can make some members of an object private and you control
what is exposed via a public interface. Similarly, a function in most languages exposes just
a public interface - you can call it - but it hides the internals of how it works. The idea is
that the functioning of a function or internals of an object are irrelevant and the caller should
not see them.

This design decision has an important consequence. It means that the library author becomes the 
person at the "head position." They get to decide what is the right use of their components and
what is not. The class distinction caused by abstraction has been hugely influential and people 
nowadays often say that a certain complex language feature (e.g. fancy types) need to be understood 
only by "library authors" and not by "library users".

Would an alternative design be possible and reasonable? In a paper [Tracing a Paradigm for 
Externalization: Avatars and the GPII Nexus (PDF)](https://refuses.github.io/preprints/avatars.pdf)
from [Salon des Refusés](https://refuses.github.io/), Colin Clark and Antranig Basman very 
convincingly argue that systems which do not hide internals behind abstraction can be 
surprisingly long-living, precisely because they do not dictate how the components should be used.

Perhaps then, thinking about programming language design from the design perspective would let
us discover more of such subtle influences of design, explore the alternatives and advance the
discipline.

## Exhibit 2: Modernist ideas and programming

<img src="http://tomasp.net/blog/2017/design-side-of-pl/bauhaus.jpg" class="rdecor"
  style="max-width:400px;margin-left:30px;margin-top:20px;margin-bottom:20px;width:calc(100% - 60px)" />

Mathematical models can be used to prove properties about models of programs. Engineering methods
can be used to make the construction process more robust. Design thinking complements these two
by letting us talk about how programming languages, libraries and programs are created. What are
the guiding principles that made them look the way they look? And what are these principles aiming
to achieve?

### Form follows function

Parson's book frames the discussion about design using modernist ideas:

> Modernism's "reconceptualization" of Design problems is nicely summed up in 
> the famous slogan "Form follows Function." The central idea is that if the Designer merely 
> constructs the object to perform its function, then expression, aesthetic value and 
> mediation become, as it were, "spin-off" values that follow effortlessly.

What would the design of a programming language or a library look if it followed the modernist 
slogan "Form follows Function?" Whether this is a design principle worth following for programming
or not is another question - just like you might prefer Art Deco architecture over modernist
architecture, perhaps you prefer more decorative programming languages over modernist ones. 

However, framing discussion about programming languages in terms of guiding design ideas might
give make the design process easier and clearer and it can also help us avoid some of the 
pointless discussions about languages. For example, very few people argue whether "modernism is 
_better_ than postmodernism". People discuss the differences and their personal preference, but
the question of which one is _better_ makes little sense. 

### Ornament and modernism in programming

As an example of what I have in mind with different design principles, let's look at two 
small examples of parser combinator code snippets. I think the topic is worth a separate
post, but I wanted a quick illustration. First, here is a part of the JSON parser using 
the [FParsec library for F#](http://www.quanttec.com/fparsec/tutorial.html):

    let listBetweenStrings sOpen sClose pElement f =
      between (str sOpen) (str sClose)
              (ws >>. sepBy (pElement .>> ws) (str "," >>. ws) |>> f)

    let jlist = 
      listBetweenStrings "[" "]" jvalue JList

    let keyValue = 
      stringLiteral .>>. (ws >>. str ":" >>. ws >>. jvalue)

In this example, there is more to the "Form" than just the "Function". To make the code shorter
and nicer to read, the form of it uses a number of custom ornamental operators such as 
`.>>.`, `>>.` and `|>>`. You need to learn what is behind those ornaments, but then you might
appreciate reading code written in this style.

If you prefer a modernist design and believe that [ornament is a 
crime](https://en.wikipedia.org/wiki/Ornament_and_Crime), you might prefer to write the
code in a way where the form directly follows function. As an example consider the following
[Brainfuck parser in Clojure](https://github.com/youngnh/parsatron):

    [lang=clojure]
    (defparser instruction []
      (choice (char \>) (char \<) (char \+)
              (char \-) (char \.) (char \,)
              (between (char \[) (char \]) (many (instruction)))))

    (defparser bf []
      (many (instruction))
      (eof))

Here, the form follows directly the function given the material (language) constraints. Two 
notes are in order. First, I'm not saying that one is better than the other. I have my
personal preference, but that is just a personal preference. Second, I don't think the language
dictates the style in this case. I can perfectly imagine ornamental parser library for Clojure
and modernist parser library for F#. 

Just like with the subtle influence of design, I think the above example is merely scratching
the surface. I believe the modernist principle that form follows function could be used as a 
building block for a new, interesting, kind of programming languages. This way of thinking 
about programming is very much unexplored. As far as I'm aware, the only relevant paper here is
[Notes on postmodern programming (PDF)](http://homepages.mcs.vuw.ac.nz/~kjx/papers/nopp.pdf) by
James Noble and Robert Biddle. I think there should be many more!

## Side-note: How design lost to mathematics and science

Before wrapping up, there was one more remark in Parsons book that I found very interesting in
the context of programming languages. One of the design movements mentioned in the book aimed
to create "design science", partly in reply to the fact that design was not recognized as a
reputable academic discipline:

> Simon (...) argued that the reliance on intuition had led to a marginalization of Design 
> within the contemporary university. In the university setting, Simon argued, the highest 
> prestige accrues to the pure sciences, due to their much vaunted "scientific method" (...).
> Situated next to the pure sciences, Design (...) looked flimsy and amateurish. 

I believe this also explains why design thinking got lost from academic thinking about 
programming languages. Compared to aspects that can be measured or formally proved, the 
design issues do perhaps look flimsy, but I believe they are equally important - they simply
look at questions that cannot be answered with engineering, scientific or mathematical ways
of thinking. In contrast to design, computer science took the right approach in order to 
succeed in the modern university. As noted by Nathan Ensmenger in [The Computer Boys Take 
Over](http://amzn.to/2vU4v3D):

> The rise of theoretical computer science was anything but inevitable. (...) Advocates of 
> theoretical computer science pursued a strategy that served them well within the university, 
> but increasingly alienated them from their colleagues in the industry.

Thanks to the success of theoretical computer science, there are now academic departments that
study things like programming languages. Sadly, it also means that the methods of study are 
limited to those that the are prestigious in the academic environment. I believe that there is
a room for looking at programming languages from the perspective of intuitive, (flimsy and
amateurish) design perspective, because it is the best we have for answering a number of 
important programming language questions.

## Summary: Towards resurrecting design 

In this blog post, I looked at two aspects of design thinking that can be applied to 
programming language design. The first was the subtle influence of design - seemingly
standard and uninteresting features of design such as rectangular shape of a table have
important consequences. The same is the case with, for example, abstraction in programming
languages. The second theme was design principles such the modernist mantra that form follows
function. How can those principles be exhibited in programming language design and what
other guiding principles can we find?

I believe that asking those questions would enrich the discussion about programming languages.
As mentioned earlier, design is the intentional creation of plans for a new kind of thing 
with the primary function of altering the world - I think that there is a lot of unexplored
space in programming languages and treating them from the design perspective is one way to 
exploring those new areas. 

I also believe that interesting programming language research is not about finding a keyword
that will make writing `for` loops 3x easier, but about changing how we interact with the 
world. For example, [can programming language research help democratize access to 
data and fight "fake news"?](https://www.youtube.com/watch?v=aHjgpmzFjOA)

One initiative in this direction has been the recent [Salon des Refusés 
workshop](http://2017.programmingconference.org/track/refuses-2017) that I helped to organize.
We hope to host another one at [@programmingconf](https://twitter.com/programmingconf) in Nice
in 2018 and we will also be launching a new web page for the Salon soon, so if you made
it to the end, follow me at [@tomaspetricek](http://twitter.com/tomaspetricek) to hear more
soon! I would also love to hear your ideas about how design thinking can be applied to 
programming languages and what good materials on "design thinking" are there!

> _<i class="fa fa-hand-o-right" style="font-size:110%;margin:0px 5px 0px 0px"></i>
> If you are interested in new ways of thinking about programming, please join our new
> [Shift Society mailing list](https://groups.google.com/forum/#!forum/shift-society). We'll
> use it to share announcements about events like [Salon des Refusés](https://refuses.github.io/)
> and provide space for discussions on the future of programming._
