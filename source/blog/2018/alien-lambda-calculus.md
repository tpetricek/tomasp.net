Would aliens understand lambda calculus? [DRAFT]
========================================

 - title: Would aliens understand lambda calculus?
 - date: 2018-01-22T17:42:38.3286628+01:00
 - description: TODO
 - layout: post
 - image-large: http://tomasp.net/blog/2017/design-side-of-pl/bauhaus.jpg
 - tags: academic, research, programming languages, philosophy

----------------------------------------------------------------------------------------------------

Unless you are a sci-fi author or some secret government agency, the question whether aliens
would understand lambda calculus is probably not your main practical concern. However, the question
is intriguing because it nicely vividly formulates a fundamental question about our formal mathematical 
knowledge. Are mathematical theories and results about them _invented_, i.e. constructed by 
humans, or _discovered_, i.e. are they eternal truths that exist regardless of whether there are
humans to know them?

<img src="http://tomasp.net/blog/2018/alien-lambda-calculus/human.jpg" class="rdecor"
    style="width:40%;max-width:400px;margin-left:30px;margin-top:0px;margin-bottom:0px" />

The question makes for a fantastic late night pub debate, but how can we go about answering it using
a more serious methodology? Is there a paper one can read to better understand the problem? 
Occasionally, a [talk](https://www.youtube.com/watch?list=PLcGKfGEEONaCIl5eU53uPBnRJ9rbIH32R&v=IOiZatlZtGU) 
or an [online comment](https://www.quora.com/Do-aliens-have-LISP-or-Scheme) 
by a computer scientist comments on this question, but way too often, people miss the fact that 
the nature of mathematical entities is one of the fundamental questions of _philosophy of 
mathematics_. Alas, all those discussions are carefully hidden in the humanities department!

I believe that knowing a bit about philosophy of mathematics is important if we want to have a 
meaningful debate about philosophical questions of mathematics (sic!) and so I did a talk 
[on this very subject at CodeMesh 2017](https://www.youtube.com/watch?v=JoWH2jNlvQQ). 
This article is slightly refined and hopefully
more polished version of the talk for those who, like me, prefer reading over watching. 
Keep in mind that the question about the nature of mathematical entities is one of the fundamental
questions of an _entire academic discipline_. As such, this article cannot possibly cover all the
relevant discussions. Compared to some other writings in this space, this article is, at least, 
based on a couple of philosophical books that, I believe, have useful things to say on the subject!

----------------------------------------------------------------------------------------------------

Crash course in philosophy of mathematics
-----------------------------------------

Before we get to lambda calculus, I want to discuss four different ways of thinking about 
mathematical theories. Again, these do not cover all the broad range of ideas that exist in 
philosophy of mathematics, but they will tell us interesting things about lambda calculus 
(and aliens!) later in the article. 

### Platonism and eternal truths

The first idea, going back to Plato, is that _the existence of mathematical objects is 
independent of us, our language, thoughts and practices._ In other words, mathematical theories
like lambda calculus and theorems exist independently of humans and will continue to exist 
when all humans (and their textbooks!) are gone. They have an independent existence and we can,
through some mechanism discover them.

This is an appealing view. If you follow simple logical steps to derive an idea from basic 
axioms, it feels that we are simply watching shadows of some true ideal derivations that are
free of all human mistakes. This is, however, just a feeling you might (or might not) have about
mathematics and there is no way to directly access this world of ideal mathematical entities.

[Lakoff and Núñez (2001)](http://amzn.to/2FEu0eb) refer to this as _The Romance of Mathematics_ and point out some of the
sad consequences. _The Romance of Mathematics makes a wonderful story, but it intimidates, it 
helps to maintain an elite, it rewards incomprehensibility._ In other words, you either can
or cannot _see_ the world of eternal mathematical truths. If no, than bad luck - you probably 
won't be a mathematician (or a theoretical computer scientist) and our writing is only 
incomprehensible to you, because you do not see the truths in it.

### Mathematics as a social process

<img src="http://tomasp.net/blog/2018/alien-lambda-calculus/poly.png" class="rdecor"
    style="width:40%;max-width:400px;margin-left:30px;margin-top:0px;margin-bottom:0px" />

When you read a mathematical (or a theoretical programming language) textbook, it gives a few
axioms and then proves interesting results that logically follow from the axioms. This orderly
presentation is not how mathematics is done. First, what definitions are _interesting_ is a
question that depends on the community of mathematicians. In other words, it is a social problem.
Second, it often takes some time to get the axioms right so that they cover all intended use cases
and allow all proofs that we want.

The process is beautifully documented in [Imre Lakatos' Proofs and Refutations](http://amzn.to/2GEXwl2),
which looks at the [Euler characteristic of polyhedra](https://en.wikipedia.org/wiki/Euler_characteristic).
The polyhedra in the illustration is one of those that break the original formula (because its sides
are stars that cross, rather than triangles). To quote Lakatos:

> Mathematics does not grow through increase of the number of established theorems, but through 
> improvement by speculation and criticism, by the method of _proofs and refutations_. 

The social side of mathematics is particularly relevant because it helps to explain why the  
same thing often appears independently at a similar time (it answers a question that the community cares about)
and how comes that there are isomorphism between remote theories (some of them were adapted and improved to match).

### Cultural roots of mathematics

Mathematics is not shaped by social processes, but some aspects of mathematics also depend on 
our human culture more generally. [Lakoff and Núñez (2001)](http://amzn.to/2FEu0eb) give a 
couple of examples of how Western culture found its way into the very fabric of mathematics
that are also relevant to programming:

 * The idea of an _essence_ goes back to Aristotle. Believing that there is such essence that,
   somehow, accurately captures the nature of a thing is rooted in our culture and it is perfectly
   reasonable to imagine that other cultures might not share the concept of essence.
   
 * The idea of _foundations_ for a subject matter is another culturally rooted concept. 
   The famous [Hilbert's program](https://en.wikipedia.org/wiki/Hilbert%27s_program) was trying
   to provide foundations for mathematics. If it was not for our culture, the program would 
   likely not be interesting and influential in the community.
  
 * The idea that human reason is a form of logic is another idea that goes back to Aristotle.
   Any form of _reasoning_ about programs using _laws_ relies on this cultural fabric of mathematics.

### Theory of embodied mathematics

The book that had the most influence on my talk about aliens and lambda calculus is 
[Where Mathematics Comes From](http://amzn.to/2FEu0eb) by Lakoff and Núñez. The key idea is that
_"The only mathematics we know or can know is a brain-and-mind-based mathematics."_ In other words,
if we want to understand the nature of mathematics, we need to look at how it happens in the brain.
Of course, we are very far from understanding how the brain works, but cognitive sciences have 
some interesting results that we can rely on.

This has some important consequences. In particular, the question whether mathematical ideas 
exists as an independent eternal entities is more a religious question than a scientific one.
If they exist and are truly independent, then we have no way of accessing them and all we can
do is to believe. In contrast, the theory of embodied mathematics has some concrete scientific 
methods that we can use to study the nature of mathematics - and perhaps also the nature of 
programming language theories!

Cognitive science of mathematics
--------------------------------

<a href="http://amzn.to/2FEu0eb"><img src="http://tomasp.net/blog/2018/alien-lambda-calculus/where.jpg" class="rdecor"
  style="width:40%;max-width:400px;margin-left:30px;margin-top:0px;margin-bottom:0px" /></a>

The work on embodied mathematics also tells us interesting things about programming language 
theory. Moreover, it can be almost directly applied to the question of aliens and lambda calculus,
because the central point is that our human brain-and-mind mathematics relies on our human
brain-and-mind perception of the world. How would aliens perceive the world and what are the
conditions under which they would be likely to develop ideas such as the lambda calculus?

### Understanding mathematics through metaphors

The central idea of the theory of embodied mathematics is that _metaphors_ are not just a 
literary device, but the key to understanding of our thinking. The authors cite results from
cognitive science research showing that abstract concepts are understood, via metaphors, 
in terms of more concrete concepts. In particular:

> Many mathematical ideas are ways of mathematicizing ordinary ideas, 
> as when derivatives mathematicize the idea of instantaneous change.

Understanding the derivatives is one thing, but how does one understand more abstract 
mathematical concepts such as predicate logic, monoids or the lambda calculus? The understanding
is constructed using the following components:

 * **Innate arithmetic.** We are born with some very basic mathematical capabilities.
   In an experiment on 6 month babies (see image below), researchers remove one toy behind
   a curtain and measure how long the babies look at the result - they look longer if the
   unexpected thing happens (because a toy is secretly put back behind a curtain). This
   suggests that we are capable of basic addition and subtraction of small numbers.
 
 * **Conceptual metaphors.** Basic metaphors link different concepts via neural conflation.
   For example, our innate arithmetic capability of counting to three is linked with real-world
   ideas such as collections of objects or movement following a line. This allows us to extend
   the concept of number from just three to numbers appearing in the nature.
   
 * **Layering metaphors.** Finally, more abstract mathematical concepts are constructed using
   layering metaphors that link between multiple metaphorically constructed ideas. This is how
   we can go, for example, from a number series to a more abstract structure such as a monoid.

<img src="mickey.png" class="img-responsive" />
 
### How is arithmetic constructed?

How can we discover those metaphors? One way (cheaper than monitoring the brain activity)
is to look at the language we use for talking about abstract mathematical entities and 
real-world entities they arise from. For example, I mentioned that arithmetic can be explained
via a metaphor as a collection of objects.

<img src="objects.png" class="rdecor" style="max-width:600px" />

When we say _"add onions and carrots to the soup"_, we are using the word _add_ for working 
with object collection (things in a soup) and it happens to be the same word we use for addition.
This is a metaphorical link! We sometimes say _"5 is bigger than 7"_ rather than _greater_
(even though they are the same size on your screen), because we think of those numbers as 
collections of objects.

The table from [Lakoff and Núñez (2001)](http://amzn.to/2FEu0eb) illustrates the metaphor.
This allows us to create abstract concepts in terms of concrete things that we interact with 
in the world. Interestingly, the metaphors also give rise to laws. For example, if you have 
an object collection (soup) and first add onions before adding carrots, it is the same as if
you add carrots, before adding onions. This physical property of object collection explains 
the symmetry of addition. Of course, the metaphors have limits - for example, collection with
no objects in it is not really a collection, so this metaphor does not explain zero very well,
but there are other metaphors which do.

Lambda calculus is discovered, Angular is invented
--------------------------------------------------

Saying that something is discovered suggests that it has a profound structure that would
exist without any humans. As discussed before, this is essentially a belief in Platonism.
On the other hand, saying that something is invented suggests that the entity is not
one of those eternal truths that a Platonist believes in.

Philip Wadler made a remark [in one of his talks](https://www.youtube.com/watch?list=PLcGKfGEEONaCIl5eU53uPBnRJ9rbIH32R&v=IOiZatlZtGU)
that lambda calculus and functional languages are discovered while other programming
languages are discovered (which is why aliens would understand lambda calculus, but not C).
How do we know that lambda calculus is discovered? The strongest argument is that the same structure
appeared independently in logic, computation and category theory. This is known as the 
[Curry-Howard-Lambek correspondence](https://en.wikipedia.org/wiki/Curry%E2%80%93Howard_correspondence)
and I'll say a few words about it before discussing a number of philosophical arguments against this idea.

### Curry-Howard-Lambek correspondence

The idea behind the [Curry-Howard-Lambek correspondence](https://en.wikipedia.org/wiki/Curry%E2%80%93Howard_correspondence)
is that there are corresponding structures in lambda calculus, logic and category theory. This is
useful in many ways - for example, you can take ideas from logic and turn them into type system 
features. As a brief example:

$$$
\begin{array}{rcccl}
\textsf{PROGRAMS} & \Longleftrightarrow & \textsf{LOGIC} & \Longleftrightarrow & \textsf{CATEGORIES}\\
\textsf{type} & \Longleftrightarrow & \textsf{formula} & \Longleftrightarrow & \textsf{object}\\
\textsf{function} & \Longleftrightarrow & \textsf{implication} & \Longleftrightarrow & \textsf{arrow}\\
\textsf{tuple} & \Longleftrightarrow & \textsf{conjunction} & \Longleftrightarrow & \textsf{product}\\
\end{array}

Types in lambda calculus correspond to logical formulas and objects in category theory.
For example, a tuple $A \times B$ (which contains values of both $A$ and $B$) corresponds to a 
formula $A \,\&\, B$ (which is true when both $A$ and $B$ are true) and can be modelled as
categorical product. A function $A \rightarrow B$ matches logical implication $A \rightarrow B$.
If we have a value $A$, we can call the function and get a value $B$. If you have a proof of $A$ 
and a proof of $A \rightarrow B$, you can use the [Modus ponens rule](https://en.wikipedia.org/wiki/Modus_ponens)
to derive a proof of $B$.

I hope you can see why many people find this elegant! Even without understanding all the details,
you can see that the structures are similar - you can see that simply from the fact that I 
can describe corresponding concepts using sentences of a very similar structure. So, why do 
I have objections against the idea that lambda calculus (and logic and category theory)
are discovered?

### Philosopher's take: Category mistakes

<a href="http://amzn.to/2BTAuYw"><img src="http://tomasp.net/blog/2018/alien-lambda-calculus/mechanizing.jpg" class="rdecor"
  style="width:35%;max-width:300px;margin-left:30px;margin-top:0px;margin-bottom:0px" /></a>

First of all, even if the Curry-Howard-Lambek correspondence had no problems, it talks about
_mathematical entities_. Programming language theoreticians use lambda calculus as a formal model
of computation, but that does not make it a _programming language_. A programming language is
a technical artifact with a compiler (which can have bugs) while formal models are (if we are
Platonist) eternal and ideal.

[James Fetzer calls](https://dl.acm.org/citation.cfm?id=48530) this problem a _category mistake_.
He argues that this is why you cannot formally verify a program - because formal proof is a 
different kind of thing than a computer program. This philosophical analysis upset a number
of people working on program verification and you can read the history in Donald MacKenzie's
[Mechanizing Proof book](http://amzn.to/2BTAuYw). In our case, this means that even if lambda
calculus was discovered, all programming languages including the most elegant functional 
languages are invented. 

### Sociologist's take: Communities and processes

If we consider how the social process of mathematics contributes to the Curry-Howard-Lambek 
correspondence, it becomes less magical. First, I mentioned [Imre Lakatos' Proofs and Refutations](http://amzn.to/2GEXwl2)
earlier. The idea is that mathematical theorems develop and improve over time in order to 
deal with problematic counter-examples or other new contexts. Could this be the case here?
The correspondence is between very specific kinds of theories - you need, _simply typed_ lambda
calculus, _intuitionistic_ logic and _cartesian closed_ categories. This does not make it
any less interesting useful, but it shows that the correspondence is a result carefully constructed 
by mathematicians. And our ambition to unify disjoint branches of mathematics that made this
work possible is likely a product of our human culture.

There is one more way in which the social process of mathematics contributed to the 
correspondence. The work on both (modern) formal logic and lambda calculus is a response
to [Hilbert's program](https://en.wikipedia.org/wiki/Hilbert%27s_program) aiming to provide
foundations of mathematics. In other words, intuitionistic logic and lambda calculus both
developed from the same community, solving the same problem - and so it is not all that 
surprising that they share notable structural similarities.

### Cognitive scientist's take: Embodied experience

Same embodied experience

Where lambda calculus comes from
--------------------------------

### Container schema

### Directionality

Would aliens understand lambda calculus?
----------------------------------------
 
### Arrival

### Solaris

### Dust cloud


























a