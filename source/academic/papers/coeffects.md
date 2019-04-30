# Coeffects: Unified static analysis of context-dependence

 - description:  Effects capture how computations _affect_ the environment, coeffects capture the
    _requirements_ that computations place on the environment. We present a unified
    coeffect system that can be used for checking liveness and properties of
    data-flow or distributed programs,
 - tags: academic, publication, top
 - layout: paper
 - title: Coeffects: Unified static analysis of context-dependence
 - date: 17 June 2013
 - subtitle: Tomas Petricek, Dominic Orchard and Alan Mycroft. In Proceedings of ICALP 2013.


> Tomas Petricek, Dominic Orchard and Alan Mycroft
>
> In Proceedings of ICALP 2013

Monadic effect systems provide a unified way of tracking
effects of computations, but there is no unified mechanism for
tracking how computations rely on the environment in which they are
executed.  This is becoming an important problem for modern software
- we need to track where distributed computations run, which
resources a program uses and how they use other capabilities of the
environment.

We consider three examples of context-dependence analysis: _liveness_ analysis,
tracking the use of _implicit parameters_ (similar to tracking of _resource usage_ in
distributed computation), and calculating caching requirements for
_dataflow_ programs. Informed by these cases, we present a unified calculus for
tracking context dependence in functional languages together with a
categorical semantics based on _indexed comonads_.
We believe that indexed comonads are the right foundation for constructing
context-aware languages and type systems and that following an approach akin to
monads can lead to a widespread use of the concept.


## Paper and more information

 - Download [the paper (PDF)](coeffects-icalp.pdf)
 - View talk slides from [ICALP conference](icalp-talk.pdf)
 - Our [ICFP 2014](../structural/index.html) paper extends the work presented here.

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the [paper page on ACM](http://dl.acm.org/citation.cfm?id=2525971.2526009&coll=DL&dl=GUIDE&CFID=375487526&CFTOKEN=86636259).

    [lang=tex]
    @inproceedings{coeffects-icalp13,
      author    = {Petricek, Tomas and Orchard, Dominic and Mycroft, Alan},
      title     = {Coeffects: unified static analysis of context-dependence},
      booktitle = {Proceedings of International Conference on Automata,
                   Languages, and Programming - Volume Part II},
      series    = {ICALP 2013},
      location  = {Riga, Latvia},
      year      = {2013}
    }


If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
