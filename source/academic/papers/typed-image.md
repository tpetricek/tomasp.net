# Typed Image-based Programming with Structure Editing

 - description: Many beloved programming system like Smalltalk and LISP are image-based.
     This encourages exploration and allows flexibility, but it makes collaboration hard.
     We tackle the problem of schema change in image-based programming systems using static types.
 - tags: academic, publication
 - layout: article
 - icon: fa fa-edit
 - date: 19 October 2021
 - title: Typed Image-based Programming with Structure Editing
 - subtitle: Jonathan Edwards and Tomas Petricek. Human Aspects of Types and Reasoning Assistants, 2021

> Jonathan Edwards, Tomas Petricek
>
> Presented at Human Aspects of Types and Reasoning Assistants (HATRA), 2021

Many beloved programming systems are image-based: self-contained worlds that persist both code and
data in a single file. Examples include Smalltalk, LISP, HyperCard, Flash, and spreadsheets.
Image-based programming avoids much of the complexity of modern programming technology stacks
and encourages more casual and exploratory programming. However conventional file-based programming
has better support for collaboration and deployment. These problems have been blamed for the limited
commercial success of Smalltalk. We propose to enable collaboration in image-based programming via
types and structure editing.

We focus on the problem of schema change on persistent data. We turn to static types, which
paradoxically require more schema change but also provide a mechanism to express and execute those
changes. To determine those changes we turn to structure editing, so that we can capture changes
in type definitions with sufficient fidelity to automatically adapt the data to suit. We conjecture
that typical schema changes can be handled through structure editing of static types.

That positions us to tackle collaboration with what could be called version control for structure
editing. We present a theory realizing this idea, which is our main technical contribution. While
we focus here on editing types, if we can extend the approach to cover the entire programming
experience then it would offer a new way to collaborate in image-based programming.

## Paper and more information

 - Download [the paper (PDF)](typed-image.pdf)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @unpublished{typedimage-hatra21,
      author = {Edwards, Jonathan and Petricek, Tomas},
      title  = {Foundations of a live data exploration environment},
      note   = {Online at \url{http://tomasp.net/academic/papers/typed-image}},
      year   = {2021}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
