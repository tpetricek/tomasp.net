# Benchmarking a Baseline Fully-in-Place Functional Language Compiler

 - description: xx
 - tags: academic, publication, home
 - icon: fas fa-stopwatch-20
 - layout: article
 - date: 4 June 2026
 - title: Benchmarking a Baseline Fully-in-Place Functional Language Compiler
 - subtitle: Jaromír Procházka, Vít Šefl and Tomas Petricek. In Proceedings of TFP 2026


> Jaromír Procházka, Vít Šefl and Tomas Petricek
>
> In Proceedings of TFP 2026

Functional programming makes code easier to reason about,
inherently thread-safe, and more testable. However, immutable data struc
tures introduce efficiency costs, as many operations require copying rather
than performing in-place modifications. To address this, the Koka lan
guage introduced a novel mechanism called fully-in-place functional pro
gramming (FIP). The mechanism enables safe in-place updates while
minimizing unnecessary memory allocations. Nevertheless, Koka’s garbage
collection and extensive feature set complicate the task of isolating FIP’s
specific memory efficiency advantages. We design a minimal functional
language that supports fully in-place updates based on the FIP calculus
while omitting garbage collection. This lets us compare the performance
of the FIP approach with that of a conventional implementation. Using
finger trees, red-black trees, and quicksort as case studies, we show that
a language employing the FIP calculus in a garbage-collection-free envi
ronment can achieve a significant increase in performance and memory
efficiency. Our work confirms the results of the original authors, but it
also uncovers potential limitations of the FIP approach

## Paper and more information

 - Download [the paper (PDF)](tfp26.pdf)
 - [StaFip compiler source code](https://github.com/JaromirProchazka/FipCompiler) on GitHub

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @inproceedings{prochazka-2026-stafip,
      author    = {Jaromír Procházka, Vít Šefl and Tomas Petricek},
      title     = {Benchmarking a Baseline Fully-in-Place 
                   Functional Language Compiler},
      booktitle = {Trends in Functional Programming},
      year      = {2026},
      publisher = {Springer Nature Switzerland},
      address   = {Cham},
      pages     = {1--19},
      note      = {To appear}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via BlueSky at [@tomasp.net](https://bsky.app/profile/tomasp.net).
