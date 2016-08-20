# Context-aware programming languages (PhD thesis)


 - description: The key point made by this thesis is the realization that an execution environment or a context is
     fundamental for writing modern applications and that programming languages should provide abstractions
     for programming with context and verifying how it is accessed. The thesis summarizes all my work on
     coeffects.
 - tags: academic, publication, draft
 - layout: paper
 - title: Context-aware programming languages (PhD thesis)
 - subtitle: Tomas Petricek. Submitted in December 2014
 - date: 18 December 2014
 
> Supervised by Alan Mycroft
>
> University of Cambridge, submitted December 2014

The development of programming languages needs to reflect important changes in the way
programs execute. In recent years, this has included e.g. the development of parallel programming 
models (in reaction to the multi-core revolution) or improvements in data access technologies. 
This thesis is a response to another such revolution - the diversification of devices and 
systems where programs run. 

The key point made by this thesis is the realization that an execution environment or
a _context_ is fundamental for writing modern applications and that programming 
languages should provide abstractions for programming with context and verifying how 
it is accessed. 

We identify a number of program properties that were not connected before, but model some notion 
of context. Our examples include tracking different execution platforms (and their versions) 
in cross-platform development, resources available in different execution environments (e.g. GPS
sensor on a phone and database on the server), but also more traditional notions such as 
variable usage (e.g. in liveness analaysis and linear logics) or past values in 
stream-based data-flow programming.

Our first contribution is the discovery of the connection between the above examples and
their novel presentation in the form of calculi (_coeffect systems_). The presented type 
systems and formal semantics highlight the relationship between different notions of context. 
Our second and third contributions are two unified coeffect calculi that capture commonalities 
in the presented examples. In particular, our _flat coeffect calculus_ models languages 
with contextual properties of the execution environment and our _structural coeffect 
calculus_ models languages where the contextual properties are attached to the variable usage.
We close the thesis with a _unified coeffect calculus_ that captures the two notions
using a single parameterized system.

Although the focus of this thesis is on the syntactic properties of the presented 
systems, we also discuss their category-theoretical motivation. We introduce the notion of
an _indexed_ comonad (based on dualisation of the well-known monad structure) and show 
how they provide semantics of the two coeffect calculi. 

## Thesis and more information

 - Download [the thesis (PDF)](thesis.pdf)
 - For flat coeffect calculus, see [ICALP 2013 paper](../../papers/coeffects/)
 - For structural coeffect calculus, see [ICFP 2014 paper](../../papers/structural/)

## <a id="cite">Bibtex</a>
Most of the (interesting) work from the thesis has been better described in the
ICALP 2013 and ICFP 2014 papers mentioned above, but the thesis can be cited using
the following information:

    [lang=tex]
    @@phdthesis{coeffects-thesis,
      author       = {Petricek, Tomas}, 
      title        = {Context-aware programming languages},
      school       = {University of Cambridge},
      year         = {2014},
      month        = {12},
      note         = {Submitted}
    }

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@@tomaspetricek](http://twitter.com/tomaspetricek).
