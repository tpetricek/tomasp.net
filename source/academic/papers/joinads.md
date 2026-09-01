# Joinads: a retargetable control-flow construct for reactive, parallel and concurrent programming

 - description:   Reactive, parallel and concurrent programming models are often difficult to encode in
    general-purpose programming languages. We present a lightweight language extension based on
    pattern matching that can be used for encoding a wide range of these models.
 - date: 20 January 2011
 - tags: academic, publication
 - layout: article
 - title: Joinads: a retargetable control-flow construct for reactive, parallel and concurrent programming
 - subtitle: Tomas Petricek and Don Syme. In Proceedings of PADL 2011.


> Tomas Petricek and Don Syme
>
> In Proceedings of PADL 2011

Modern challenges led to a design of a wide range of programming models for reactive,
parallel and concurrent programming, but these are often difficult to encode in general
purpose languages. We present an abstract type of computations called joinads together
with a syntactic language extension that aims to make it easier to use joinads in modern
functional languages.

Our extension generalizes pattern matching to work on abstract computations. It keeps a
familiar syntax and semantics of pattern matching making it easy to reason about code,
even in a non-standard programming model. We demonstrate our extension using three
important programming models – a reactive model based on events; a concurrent model based
on join calculus and a parallel model using futures. All three models are implemented as
libraries that benefit from our syntactic extension. This makes them easier to use and also
opens space for exploring new useful programming models.

## Paper and more information

 - Download [the paper (PDF)](joinads.pdf)
 - View [slides from PADL 2011 (PDF)](padl-talk.pdf)
 - Get the [prototype implementation](http://tomasp.net/blog/fsharp-variations-joinads.aspx)
 - Watch a [talk about earlier version](http://langnetsymposium.com/2009/talks/22-TomasPatricek-Reactive.html)

## Related papers

This paper presents the original version of joinads in F#. There is also a
newer version (implemented in Haskell) that is more theoretically oriented and discusses
reasoning about joinads in more details. The main differences are that the updated version
requires all joinads to be monads (which is more restrictive) and it is translated in
terms of simpler operations. See [Extending Monads with Pattern Matching](../docase/)
for more information.

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
get full details from the [paper page on ACM](http://dl.acm.org/citation.cfm?id=1946313.1946336&coll=DL&dl=GUIDE&CFID=375487526&CFTOKEN=86636259).

    [lang=tex]
    @inproceedings{petricek-2011-joinads,
      author    = {Tomas Petricek and Don Syme},
      title     = {Joinads: {A} Retargetable Control-Flow Construct for
                   Reactive, Parallel and Concurrent Programming},
      booktitle = {Practical Aspects of Declarative Languages - 13th
                   International Symposium, {PADL} 2011, Austin, TX, USA,
                   January 24-25, 2011. Proceedings},
      series    = {Lecture Notes in Computer Science},
      volume    = {6539},
      pages     = {205--219},
      publisher = {Springer},
      year      = {2011},
      doi       = {10.1007/978-3-642-18378-2\_17}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via BlueSky at [@tomasp.net](https://bsky.app/profile/tomasp.net).
