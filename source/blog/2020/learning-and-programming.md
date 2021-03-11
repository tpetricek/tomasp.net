Is deep learning a new kind of programming? Operationalistic look at programming
===========================================

 - title: Is deep learning a new kind of programming? Operationalistic look at programming
 - date: 2020-10-07T01:43:16.3109375+01:00
 - description: Making a concept synonymous with the operations for working with it is
    an idea introduced by P. W. Bridgman to think about measurements in physics. A length
    measured by a ruler is a different concept than a length measured by the time it takes
    light to travel. However, what if we thought about programs in the same way? Is a program
    constructed manually the same as a program obtained by training a neural network?
 - layout: article
 - references: true
 - icon: fa fa-code
 - image-large: http://tomasp.net/blog/2020/learning-and-programming/pyrometer.jpg
 - tags: programming languages, philosophy, research

----------------------------------------------------------------------------------------------------

In most discussions about how to make programming better, someone eventually says
something along the lines of _"we'll just have to wait until deep learning solves
the problem!"_ I think this is a [naively optimistic idea](https://en.wikipedia.org/wiki/AI_winter),
but it raises one interesting question: In what sense are programs created using deep
learning a _different kind_ of programs than those written by hand?

<div class="rdecor">
<img src="http://tomasp.net/blog/2020/learning-and-programming/bridgman.jpg" style="max-width:350px"/>
</div>

This question recently arose in discussions that we have been having as part of the
[PROGRAMme project](https://programme.hypotheses.org/), which explores historical and
philosophical perspectives on the question "What is a (computer) program?" and so this
article owes much debt to [others involved in the project](https://programme.hypotheses.org/members),
especially Maël Pégny, Liesbeth De Mol and Nick Wiggershaus.

Many people will intuitively think that, if you train a deep neural network to solve some
a problem, you get a different kind of program than if you manually write some logic to solve
the problem. But what exactly is the difference? In both cases, the program is a sequence of
instructions that are deterministically executed by a machine, one after another, to produce
the result.

When reading the excellent book [Inventing Temperature][temp] by Hasok
Chang recently, I came across the idea of [operationalism][op],
which I believe provides a useful perspective for thinking about the issue of deep learning and
programming. The operationalist point of view was introduced by a physicist Percy Williams Bridgman. To
quote: _we mean by any concept nothing more than a set of operations; the concept is synonymous
with the corresponding set of operations_. What does this tell us about deep learning and programming?

----------------------------------------------------------------------------------------------------

## Operationalism and temperature

Before I talk about programming, I need to say a bit about _operationalism_. I will be relying on
the description from [Chang's book on Temperature](https://amzn.to/2SvTwKT), but Bridgman used
the measurement of length as an example. The key idea is that a concept, such as a length, is
defined by the operations for working with it. This means that the _length_ that is measured
using a ruler is a different kind of _length_ than an astronomical length measured in terms of
the amount of time that light takes to travel.

<div class="rdecor">
<img src="http://tomasp.net/blog/2020/learning-and-programming/pyrometer.jpg" style="max-width:350px"/>
</div>

We are so used to thinking of _length_ that this idea may seem odd, but take temperature
measurement as an example. One issue in the history of temperature was that regular thermometers
did not work for very high temperatures (the boiling point of mercury is 356.7 °C). An
alternative way of measuring very high temperatures was invented by [Josiah Wedgwood](https://en.wikipedia.org/wiki/Wedgwood_scale),
an English potter. His Wedgwood scale (°W) was based on the shrinking of small clay cylinders.
In high heat, the cylinders contracted. After removing them from the heat, you then used a
provided ruler to read the temperature. (This only worked with a specific clay from Wedgwood's
own mines, but he kindly donated all the clay to the Royal Society.)
The temperature of [red heat](https://en.wikipedia.org/wiki/Red_heat) was 0 °W, melting point
of copper 27 °W and the melting point of gold was 32 °W.

Figuring out how to match a temperature scale based on mercury thermometer with the Wedgwood
scale is hard, because the two do not operationally overlap. Wedgwood's own attempt at producing
a conversion table was a way off, giving melting point of silver as 4,717 °F (rather than 1,763°F).

## Operationalist look at programming and machine learning

Programming is not much like measuring temperature, but there are certainly practical "operations"
that are employed for doing various things with programs. Many of the operations that you use
when creating a program based on deep learning and an ordinary program are quite different.

* **Program execution.**
  First of all, execution is the operation that is actually very similar for both regular programs
  and deep neural networks. In both cases, the program is a long sequence of instructions
  with some data. It is provided with some inputs, performs a calculation using the inputs and
  produces an output. If we looked only at execution, then we would likely not see much difference.

* **Programming or training.**
  The difference becomes obvious when we start looking at how programs are constructed. In case of
  ordinary programs, you write some logic. In case of deep networks (or any other programs based on
  machine learning), the program is obtained by training, i.e. adapting some numerical parameters
  based on sample data.

* **Understanding programs.**
  A more interesting issue is that of understanding what a program does. This may be a challenge
  for an ordinary program if it is large and complicated, but you can generally study the code
  or ask people who wrote it. For machine learning, this depends on the type of algorithm used.
  While you can understand how a decision tree makes a decision, it is not clear how to "understand"
  what a deep neural network does.

* **Verifying programs.**
  If you wanted to prove that an ordinary program is correct, you can (perhaps very laboriously)
  prove that it matches its specification, which describes its key properties. For deep neural
  network, you can verify that it correctly propagates weights, but proving anything about what
  the program actually does is tricky. (Incidentally, this is also why I always found that the 2015
  [AI Open Letter][ai] is missing the point in its emphasis
  of "Verification" as a research goal for AI systems.)

## Opacity of programs and ML algorithms

<div class="rdecor">
<img src="http://tomasp.net/blog/2020/learning-and-programming/atlas.jpg" style="max-width:350px"/>
</div>

In the above list, the cases of execution and programming or training show fairly obvious
similarities and differences, respectively. The more subtle cases are that of understanding
and verifying programs. An interesting reference for thinking about these is the paper
[Computer simulations, machine learning and the Laplacean demon: Opacity in the case of high energy
physics][opacity], which looks at programs and ML algorithms in
physics experiments at CERN (thanks to Nick for the recommendation!)

The paper compares opacity, i.e. the possibility of understanding, of deep neural networks and
computer simulations, which are very large and complex, but otherwise "ordinary" programs.
It identifies a number of different kinds of opacity.

Both computer simulations and deep networks are _algorithmically transparent_, which means that
one can follow the sequence of instructions when they execute. To a human, this does not help
very much when trying to understand a program (but it is enough for a [Laplacean deamon](https://en.wikipedia.org/wiki/Laplace%27s_demon),
and so neither of the types of programs have what the authors call _fundamental opacity_).

#### Complexity of large systems

The opacity is then closely linked to the humans involved in the process.
For computer simulations, the main issue is that they are large and complex.
This is, by the way, the case for many other software systems and we could
reasonably argue that large systems (or simulations) are different kind of
programs than small ones, precisely because the operation of "understanding them"
is different for each. To quote Bridgman again:

> _Mathematics does not recognize that as the physical range increases, the fundamental
> concepts become hazy, and eventually cease entirely to have physical meaning
> and therefore must be replaced by other concepts which are operationally quite different._

I think the issue of scale is something that computer scientists ignore way
too easily. For example, the methods that we use for proving the correctness of
a small, several line long, algorithm are operationally very different than those
we use for proving the correctness of a [compiler for a realistic programming language](https://cakeml.org/).

#### The case of deep learning algorithms

The issue of complexity is even worse in the case of deep neural networks.
A useful concept referenced in the above paper is "opaqueness of paths".
In a deep neural network (with non-zero weight), the number of possible paths
through which data can flow increases exponentially with every layer. The number
of data-dependencies in a complex ordinary program may be large too, but it
won't grow exponentially with e.g. every new class.

The above paper makes one more excellent point about machine learning. The
ML algorithm has a _quasi-autonomy_ and performs iterative tuning of parameters.
For ordinary programs, the programmer writes every single line (and tunes all constants)
of the program. In other words, even if we cannot immediately understand every
single aspect of an ordinary program, we should be able to find someone who knows
(or at least knew at some point in the past). For understanding a weight in a
deep network, we'd need to backtrack through the entire learning process...

## Towards a unified understanding?

Looking at programs and machine learning from an operationalist perspective also
suggests an interesting future research question. If we have two measurement operations
that can both be applied over some domain, it is possible to make the operations
match over the common domain and unify the scales. Wedgewood himself tried to do this
by matching his scale to the Fahrenheit scale by using a scale based on metal expansion
as the intermediary between mercury based thermometers and his clay cylinders.
(The Wedgwood scale was forgotten before anybody produced a _correct mapping_, but
a correct mapping could certainly be created.)

The curious question I want to conclude with is this: Are there operations for
construction, understanding and verification that would work the same for both
"ordinary" programs and machine learning algorithms? Certainly not today and certainly not
anytime soon. However, I imagine that there might be a more interactive, machine-assisted way of
programming and doing some of the other tasks that can work with both kinds of entities -
ordinary programs at one end of the spectrum and deep neural networks at the other end.

[temp]: https://amzn.to/2SvTwKT "Hasok Chang (2004). Inventing Temperature: Measurement and Scientific Progress"
[op]: https://plato.stanford.edu/entries/operationalism/ "Hasok Chang (2019). Operationalism, The Stanford Encyclopedia of Philosophy"
[ai]: https://futureoflife.org/ai-open-letter/ "Stuart Russell, Daniel Dewey, Max Tegmark (2015). An Open Letter: Research Priorities for Robust and Beneficial Artificial Intelligence"
[opacity]: http://philsci-archive.pitt.edu/17637/ "Florian J. Boge, Paul Grünke (2019). Computer simulations, machine learning and the Laplacean demon: Opacity in the case of high energy physics"
