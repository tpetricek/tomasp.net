# Live & Local Schema Change: Challenge Problems

 - description: Schema change is an unsolved problem in both live programming and local-first
      software. It can create a mismatch between the code and data in the running environment and
      mismatches between data in different replicas. We contribute a set of challenges involving
      schema change, offered to the live and local-first communities.
 - tags: academic, publication
 - layout: article
 - icon: fa fa-database
 - date: 23 October 2023
 - title: Live & Local Schema Change: Challenge Problems
 - subtitle: Jonathan Edwards, Tomas Petricek, Tijs van der Storm. Presented at LIVE 2023

> Jonathan Edwards, Tomas Petricek, Tijs van der Storm
>
> Presented at LIVE 2023

Schema change is an unsolved problem in both live programming and local-first software. We include in schema change any change to the expected shape of data, whether that is expressed explicitly in a database schema or type system, or whether those expectations are implicit in the behaviour of the code. Schema changes during live programming can create a mismatch between the code and data in the running environment. Correspondingly, Schema changes in local-first programming can create mismatches between data in different replicas, and between data in a replica and the code co-located with it. In all of these situations the problem of schema change is to migrate or translate existing data in coordination with changes to the code.

This paper contributes a set of concrete scenarios involving schema change that are offered as challenge problems to the live programming and local-first communities. We hope that these problems will spur progress by providing concrete objectives and a basis for comparing alternative solutions.

## Paper and more information

 - Download [paper pre-print (PDF)](live-2023.pdf)
 - Get the [archived version from arXiv](https://arxiv.org/abs/2309.11406)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @article{schema-challenges-live23,
      title={Live \& Local Schema Change: Challenge Problems},
      author={Edwards, Jonathan and Petricek, Tomas and van der Storm, Tijs},
      journal={arXiv preprint arXiv:2309.11406},
      year={2023}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
