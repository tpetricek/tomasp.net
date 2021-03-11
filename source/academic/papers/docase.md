# Extending monads with pattern matching

 - description:  The paper introduces a `docase` notation for Haskell that can be used for any monad that
    provides additional operations representing parallel composition, choice and aliasing. We require the
    operations to form a near-semiring, which gurantees that the notation resembles pattern matching.
 - date: 5 September 2011
 - tags: academic, publication
 - layout: article
 - title: Extending monads with pattern matching
 - subtitle: Tomas Petricek, Alan Mycroft and Don Syme. In Haskell Symposium 2011.


> Tomas Petricek, Alan Mycroft and Don Syme
>
> In Proceedings of Haskell Symposium 2011

Sequencing of effectful computations can be neatly captured using monads and elegantly written using
`do` notation. In practice such monads often allow additional ways of composing computations,
which have to be written explicitly using combinators.

We identify joinads, an abstract notion of computation that is stronger than monads and captures
many such ad-hoc extensions. In particular, joinads are monads with three additional operations:
one of type `m a -> m b -> m (a, b)` captures various forms of <em>parallel composition</em>,
one of type `m a -> m a -> m a` that is inspired by <em>choice</em> and one of type `m a -> m (m a)`
that captures <em>aliasing</em> of computations. Algebraically, the first two operations form a
near-semiring with commutative multiplication.

We introduce `docase` notation that can be viewed as a monadic version of `case`. Joinad laws
make it possible to prove various syntactic equivalences of programs written using `docase`
that are analogous to equivalences about `case`. Examples of joinads that benefit from the notation
include speculative parallelism, waiting for a combination of user interface events, but also
encoding of validation rules using the intersection of parsers.

## Paper and more information

 - Download [the paper (PDF)](docase.pdf)
 - Download [COQ script](docase.v) with some proofs
 - See [the original implementation](http://github.com/tpetricek/Haskell.Joinads) (pre-processor and samples) at Github
 - See [GHC fork](https://github.com/tpetricek/Haskell.Extensions) and [GHC base fork](https://github.com/tpetricek/Haskell.Extensions.Base) for a proper implementation
 - View talk slides from [Haskell Symposium](haskell-symposium.pdf)

## Try Joinads

<img src="tryjoinads.png" style="float:right;margin:0px 20px 0px 30px" />

[Try Joinads](http://tryjoinads.org) is a web site, using the
[open-source release of F#](https://github.com/fsharp/fsharp), that implements
the joinads extension for F#. It comes with an browser-based F# console where you can experiment with
joinads and numerous tutorials that demonstrate the usfulness of joinads. Tutorials include
asynchronous, parallel and concurrent programming as well as parsing.

 - Visit the [Try Joinads](http://tryjoinads.org) web site!

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the [paper page on ACM](http://dl.acm.org/citation.cfm?id=2034675.2034677&coll=DL&dl=GUIDE&CFID=375487526&CFTOKEN=86636259).

    [lang=tex]
    @inproceedings{joinads-haskell11,
      author    = {Petricek, Tomas and Mycroft, Alan and Syme, Don},
      title     = {Extending {M}onads with {P}attern {M}atching},
      booktitle = {Proceedings of Haskell Symposium},
      series    = {Haskell 2011},
      location  = {Tokyo, Japan},
      year      = {2011}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
