# Don't Call Us, We'll Call You: Towards Mixed-Initiative Inte&shy;ractive Proof Assistants for Programming Language Theory

 - description: When constructing a proof about programming language theory, one typically states
     the property and then develops the proof manually, until an automatic strategy can fill the
     remaining gaps. We propose a mixed-initiative proof assistant where the tool starts with a
     an automatic search, but seeks feedback from the user when needed.
 - tags: academic, publication, home
 - layout: article
 - icon: fa fa-list-check
 - date: 20 October 2024
 - title: Don't Call Us, We'll Call You: Towards Mixed-Initiative Interactive Proof Assistants for Programming Language Theory
 - subtitle: Jan Liam Verter and Tomas Petricek. Human Aspects of Types and Reasoning Assistants, 2024

> Jan Liam Verter, Tomas Petricek
>
> Presented at Human Aspects of Types and Reasoning Assistants (HATRA), 2024

There are two kinds of systems that programming language researchers use for their work.
Semantics engineering tools let them interactively explore their definitions, while proof
assistants can be used to check the proofs of their properties. The disconnect between the two
kinds of systems leads to errors in accepted publications and also limits the modes of interaction
available when writing proofs.

When constructing a proof, one typically states the property and then develops the proof manually
until an automatic strategy can fill the remaining gaps. We believe that an integrated and more
interactive tool that leverages the typical structure of programming language could do better. A
proof assistant aware of the typical structure of programming language proofs could require less
human input, assist the user in understanding their proofs, but also use insights from the
exploration of executable semantics in proof construction.

In the early work presented in this paper, we focus on the problem of interacting with a proof
assistant and leave the semantics engineering part to the future. Rather than starting with manual
proof construction and then completing the last steps automatically, we propose a way of working
where the tool starts with an automatic proof search and then breaks when it requires feedback from
the user. We build a small proof assistant that follows this mode of interaction and illustrates
the idea using a simple proof of the commutativity of the "+" operation for Peano arithmetic. Our
early experience suggests that this way of working can make proof construction easier.

## Paper and more information

 - Download [the paper (PDF)](paper.pdf)
 - Get the [archived version from arXiv](https://arxiv.org/abs/2409.13872)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @unpublished{typedimage-hatra21,
      author = {Verter, Jan Liam and Petricek, Tomas},
      title  = {Don't Call Us, We'll Call You: Towards Mixed-Initiative
                Interactive Proof Assistants for Programming Language Theory},
      note   = {Presented at Human Aspects of Types and Reasoning Assistants (HATRA).
                Online at \url{https://arxiv.org/abs/2409.13872}},
      doi    = {10.48550/arXiv.2409.13872}
      year   = {2024}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
