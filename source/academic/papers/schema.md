# Schema Evolution in Interactive Programming Systems

 - description: Many improvements to programming came from tighter feedback loops, but making a
     program change that involves changing the schema (or type) of program is difficult -- you
     have to manually update code and data to match the new schema. We present an analysis of the
     problem of schema change in programming and a number of diverse challenge problems to
     inspire research on the problem.
 - tags: academic, publication, top, home
 - layout: article
 - icon: fa fa-paint-roller
 - date: 15 October 2024
 - title: Schema Evolution in Interactive Programming Systems
 - subtitle: Jonathan Edwards, Tomas Petricek, Tijs van der Storm, Geoffrey Litt. The Art, Science, and Engineering of Programming, 2024

> Jonathan Edwards, Tomas Petricek, Tijs van der Storm, Geoffrey Litt
>
> The Art, Science, and Engineering of Programming, 2024

Many improvements to programming have come from shortening feedback loops, for example with
Integrated Development Environments, Unit Testing, Live Programming, and Distributed Version
Control. A barrier to feedback that deserves greater attention is Schema Evolution. When
requirements on the shape of data change then existing data must be migrated into the new shape,
and existing code must be modified to suit. Currently these adaptations are often performed
manually, or with ad hoc scripts. Manual schema evolution not only delays feedback but since it
occurs outside the purview of version control tools it also interrupts collaboration.

Schema evolution has long been studied in databases. We observe that the problem also occurs in
non-database contexts that have been less studied. We present a suite of challenge problems
exemplifying this range of contexts, including traditional database programming as well as live
front-end programming, model-driven development, and collaboration in computational documents.
We systematize these various contexts by defining a set of layers and dimensions of schema evolution.

We offer these challenge problems to ground future research on the general problem of schema
evolution in interactive programming systems and to serve as a basis for evaluating the results
of that research. We hope that better support for schema evolution will make programming more
live and collaboration more fluid.

## Paper and more information

 - See [the paper record](https://programming-journal.org/2025/9/2/) in The Art, Science, and Engineering of Programming journal.
 - Download [local copy of the final version of the paper (PDF)](paper.pdf)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @article{schema,
      title={Schema Evolution in Interactive Programming Systems},
      author={Edwards, Jonathan and Petricek, Tomas and van der Storm, Tijs and Litt, Geoffrey},
      journal={The Art, Science, and Engineering of Programming},
      volume={9},
      number={2},
      year={2024},
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
