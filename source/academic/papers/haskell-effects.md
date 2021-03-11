# Embedding effect systems in Haskell

 - description: This paper leverages recent advances in Haskell's type system
    to embed fine-grained monadic computations in Haskell, providing
    user-programmable effect systems. We explore a number of practical
    examples that make Haskell even better and safer for effectful
    programming.
 - tags: academic, publication
 - layout: article
 - title: Embedding effect systems in Haskell
 - subtitle: Dominic Orchard, Tomas Petricek. Haskell 2014
 - date: 19 August 2014

> Dominic Orchard, Tomas Petricek
>
> Haskell Symposium, 2014

Monads are now an everyday tool in functional programming for
abstracting and delimiting effects. The link between monads and
effect systems is well-known, but in their typical use, monads provide
a much more coarse-grained view of effects. Effect systems
capture fine-grained information about the effects, but monads provide
only a binary view: effectful or pure.

Recent theoretical work has unified fine-grained effect systems
with monads using a monad-like structure indexed by a monoid of
effect annotations (called parametric effect monads). This aligns
the power of monads with the power of effect systems.

This paper leverages recent advances in Haskell's type system
(as provided by GHC) to embed this approach in Haskell, providing
user-programmable effect systems. We explore a number of practical
examples that make Haskell even better and safer for effectful
programming. Along the way, we relate the examples to other concepts,
such as Haskell's implicit parameters and coeffects.

## Paper and more information

 - Download [the paper (PDF)](haskell-effects.pdf)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the paper page on ACM (TBA).

    [lang=tex]
    @incollection{haskell-effects,
      title={Embedding effect systems in Haskell},
      author={Orchard, Dominic and Petricek, Tomas},
      booktitle = {Proceedings of Haskell Symposium},
      series    = {Haskell 2014},
      location  = {Gothenburg, Sweden},
      year      = {2014}
    }    

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
