# Evaluation strategies for monadic computations</h1>

 - description:  When translating pure functional code to a monadic version, we need to use different translation depending
    on the desired evaluation strategy. In this paper, we present an unified translation that is parameterized by
    an operation <code>malias</code>. We also show how to give <em>call-by-need</em> translation for certain
    monads.
 - tags: academic, publication
 - layout: article
 - title: Evaluation strategies for monadic computations
 - subtitle: Tomas Petricek. In Proceedings of MSFP 2012.
 - date: 17 March, 2012

> Tomas Petricek
>
> In Proceedings of MSFP 2012

Monads have become a powerful tool for structuring effectful computations in functional
programming, because they make the order of effects explicit. When translating pure code
to a monadic version, we need to specify evaluation order explicitly. Two standard
translations give _call-by-value_ and _call-by-name_ semantics.
The resulting programs have different structure and types, which makes revisiting
the choice difficult.

In this paper, we translate pure code to monadic using an additional operation
`malias` that abstracts out the evaluation strategy.  The `malias` operation is
based on _computational comonads_; we use a categorical
framework to specify the laws that are required to hold about the operation.

For any monad, we show implementations of `malias` that give _call-by-value_
and _call-by-name_ semantics. Although we do not give _call-by-need_ semantics
for _all_ monads, we show how to turn certain monads into an extended monad with
_call-by-need_ semantics, which partly answers an open question.
Moreover, using our unified translation, it is possible to change the evaluation strategy
of functional code translated to the monadic form without changing its structure or types.

## Paper and more information

 - Download [the paper pre-print (PDF)](malias.pdf)
 - Download [the published paper (PDF)](http://arxiv.org/pdf/1202.2921.pdf), from arXiv
 - View [slides from MSFP 2012 (PDF)](talk-msfp.pdf)
 - Download [associated Haskell source code (ZIP)](malias-src.zip)

## Related papers

The `malias` abstraction has been partly inspired by the author's work on
_joinads_ which add pattern matching support to monadic computations. Joinads
require the `malias` operation for a related reason - for more information see
also [Extending monads with pattern matching](../docase/).

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the [paper page on arXiv](http://arxiv.org/abs/1202.2921).

    [lang=tex]
    @inproceedings{malias-msfp12,
      author    = {Petricek, Tomas},
      title     = {Evaluations strategies for monadic computations},
      booktitle = {Proceedings of Mathematically Structured Functional Programming},
      series    = {MSFP 2012},
      location  = {Tallinn, Estonia},
      year      = {2012}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
