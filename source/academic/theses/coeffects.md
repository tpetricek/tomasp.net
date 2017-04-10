# Context-aware programming languages (PhD thesis)


 - description: The key point made by this thesis is the realization that an execution environment or a context is
     fundamental for writing modern applications and that programming languages should provide abstractions
     for programming with context and verifying how it is accessed. The thesis summarizes all my work on
     coeffects.
 - tags: academic, publication, thesis
 - layout: paper
 - title: Context-aware programming languages (PhD thesis)
 - subtitle: Tomas Petricek. University of Cambridge, 2017
 - date: 18 December 2014
 
> Supervised by Alan Mycroft
>
> University of Cambridge, March 2017

The development of programming languages needs to reflect important changes in the way
programs execute. In recent years, this has included the development of parallel programming
models (in reaction to the multi-core revolution) or improvements in data access technologies.
This thesis is a response to another such revolution -- the diversification of devices and
systems where programs run.

The key point made by this thesis is the realization that an execution environment or
a _context_ is fundamental for writing modern applications and that programming
languages should provide abstractions for programming with context and verifying how
it is accessed.

We identify a number of program properties that were not connected before, but model some notion
of context. Our examples include tracking different execution platforms (and their versions)
in cross-platform development, resources available in different execution environments (e.g. GPS
sensor on a phone and database on the server), but also more traditional notions such as
variable usage (e.g. in liveness analysis and linear logics) or past values in
stream-based dataflow programming. Our first contribution is the discovery of the connection
between the above examples and their novel presentation in the form of calculi (_coeffect systems_).
The presented type systems and formal semantics highlight the relationship between different
notions of context.

Our second contribution is the definition of two unified coeffect calculi that capture the common
structure of the examples. In particular, our _flat coeffect calculus_ models languages
with contextual properties of the execution environment and our _structural coeffect
calculus_ models languages where the contextual properties are attached to the variable usage.
We define the semantics of the calculi in terms of category theoretical structure of an
_indexed comonad_ (based on dualisation of the well-known monad structure), use it
to define operational semantics and prove type safety of the calculi.

Our third contribution is a novel presentation of our work in the form of web-based
_interactive essay_. This provides a simple implementation of three context-aware programming
languages and lets the reader write and run simple context-aware programs, but also explore the
theory behind the implementation including the typing derivation and semantics.

## Thesis and more information

 - Download [the thesis (PDF)](thesis-final.pdf)
 - For flat coeffect calculus, see [ICALP 2013 paper](../../papers/coeffects/)
 - For structural coeffect calculus, see [ICFP 2014 paper](../../papers/structural/)

## Interactive essay

To present the ideas behind coeffects, I also made an [interactive essay](http://tomasp.net/coeffects)
(discussed in Chapter 7 of the thesis), which provides an easy to understand interactive explanation
of coeffects. This is the best place to start if you want to learn about what I did and it is a lot
shorter than the whole thesis!

<a href="http://tomasp.net/coeffects">
<img src="essay.png" style="width:80%;margin:10px 10% 0px 10%; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);"/></a>

## <a id="cite">Bibtex</a>
Most of the (interesting) work from the thesis has been better described in the
ICALP 2013 and ICFP 2014 papers mentioned above, but the thesis can be cited using
the following information:

    [lang=tex]
    @phdthesis{coeffects-thesis,
      author       = {Petricek, Tomas}, 
      title        = {Context-aware programming languages},
      school       = {University of Cambridge},
      year         = {2017},
      month        = {3},
    }

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
