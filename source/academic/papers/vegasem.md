# Formal Semantics and Type System for Vega Data Transformations

 - description: The reactive data transformation language in Vega is widely-used, but poorly
      documented. We define a graph-based operational semantics that models the Vega dataflow
      architecture and a type system that can prevent a range of common errors.
 - tags: academic, publication, home
 - icon: fas fa-chart-area
 - layout: article
 - date: 30 June 2026
 - title: Formal Semantics and Type System for Vega Data Transformations
 - subtitle: Kristýna Petrlíková, Tomas Petricek. In Proceedings of FTfJP 2026


> Kristýna Petrlíková, Tomas Petricek
>
> In Proceedings of FTfJP 2026

Vega is a popular declarative language for creating interactive data visualizations.
It supports reactive data transformations using its streaming dataflow architecture. 
Despite its widespread adoption, the exact semantics of Vega is subtle and poorly documented.
This leads to incorrect or confusing visualizations and difficult-to-understand error messages.

This paper makes two contributions. First, we define a graph-based operational semantics, 
providing a precise model of the streaming dataflow architecture of Vega. Second, we 
present a type system for the core data transformation language of Vega, which can prevent 
a range of common errors. We show that our type system is sound with respect to the semantics. 

While the dataflow architecture of Vega closely resembles well-studied models such as functional 
reactive programming and adaptive computation, there are important differences. The novelty of 
our work lies in making these precise and providing static analysis for such a reactive data 
visualization language. The result is a checker for Vega that can catch common real-world errors.

## Paper and more information

 - Download [the paper (PDF)](paper.pdf)
 - [Vega semantics demo](https://github.com/d3sprog/vega-semantics-demo/) with some subtle errors

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @inproceedings{petrlikova-2026-vegasem,
      author    = {Kristýna Petrlíková and Tomas Petricek},
      title     = {Formal Semantics and Type System for Vega Data Transformations},
      booktitle = {Proceedings of the 2026 Formal Techniques for Judicious 
                    Programming (FTfJP) Workshop. Brussels, Belgium, June 30, 2026},
      year      = {2026},
      note      = {To appear}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via BlueSky at [@tomasp.net](https://bsky.app/profile/tomasp.net).
