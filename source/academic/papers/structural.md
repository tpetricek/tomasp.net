# Coeffects: A calculus of context-dependent computation

 - description:  We present a general calculus for tracking contextual properties of computations,
    including per-variable properties (usage patterns, caching requirements) and
    whole-context properties (platform version, rebindable resources). This resolves
    questions that were left unanswered in the ICALP 2013 paper below.
 - tags: academic, publication, top, home
 - layout: article
 - icon: fa fa-server
 - title: Coeffects: A calculus of context-dependent computation
 - subtitle: Tomas Petricek, Dominic Orchard and Alan Mycroft. In Proceedings of ICFP 2014
 - date: 29 August 2014

> Tomas Petricek, Dominic Orchard and Alan Mycroft
>
> In Proceedings of ICFP 2014

The notion of _context_ in functional languages no longer refers just to variables
in scope. Context can capture additional properties of variables (usage patterns
in linear logics; caching requirements in dataflow languages) as well as additional
resources or properties of the execution environment (rebindable resources; platform version in a  
cross-platform application). The recently introduced notion of coeffects captures the
latter, whole-context properties, but it failed to capture fine-grained per-variable properties.

We remedy this by developing a generalized coeffect system with annotations indexed by
a coeffect _shape_. By instantiating a concrete shape, our system captures previously
studied _flat_ (whole-context) coeffects, but also _structural_ (per-variable) coeffects,
making coeffect analyses more useful. We show that the structural system enjoys desirable syntactic
properties and we give a categorical semantics using extended notions of _indexed comonad_.

The examples presented in this paper are based on analysis of established language features
(liveness, linear logics, dataflow, dynamic scoping) and we argue that such context-aware
properties will also be useful for future development of languages for increasingly
heterogeneous and distributed platforms.

## Watch the talk

Thanks to the ICFP workshop organizers, the video from my talk,
[Coeffects: A Calculus of Context-Dependent Computation](https://www.youtube.com/watch?v=xtxx4iADMbM) is on YouTube!

<iframe style="margin-left:25px"  width="560" height="315" src="//www.youtube.com/embed/xtxx4iADMbM" frameborder="0" allowfullscreen></iframe>

## Paper and more information

 - Download [the paper (PDF)](coeffects-icfp.pdf)
 - This work extends our previous [ICALP paper](../coeffects/index.html)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the paper [paper page on ACM](http://dl.acm.org/citation.cfm?id=2628160).

    [lang=tex]
    @inproceedings{coeffects-icfp14,
      author    = {Petricek, Tomas and Orchard, Dominic and Mycroft, Alan},
      title     = {Coeffects: A calculus of context-dependent computation},
      booktitle = {Proceedings of International Conference on Functional Programming},
      series    = {ICFP 2014},
      location  = {Gothenburg, Sweden}
    }


If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
