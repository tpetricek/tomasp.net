# Linked Visualisations via Galois Dependencies

 - description: We present new language-based dynamic analysis techniques for linking
     visualisations to data in a fine-grained way, allowing a user to interactively explore how
     data attributes map to visual or other output elements by selecting substructures of interest.
 - tags: academic, publication, top, home
 - layout: article
 - icon: fa fa-chart-line
 - date: 16 January 2022
 - title: Linked Visualisations via Galois Dependencies
 - subtitle: Roly Perera, Minh Nguyen, Tomas Petricek and Meng Wang. In Proceedings of POPL 2022

> Roly Perera, Minh Nguyen, Tomas Petricek and Meng Wang
>
> In Proceedings of POPL 2022

We present new language-based dynamic analysis techniques for linking visualisations and other
structured outputs to data in a fine-grained way, allowing a user to interactively explore how data
attributes map to visual or other output elements by selecting (focusing on) substructures of
interest. This can help both programmers and end-users understand how data sources and complex
outputs are related, which can be a challenge even for someone with expert knowledge of the problem
domain. Our approach builds on bidirectional program slicing techniques based on Galois
connections, which provide desirable round-tripping properties.

Unlike the prior work in program slicing, our approach allows selections to be negated. In a
setting with negation, the bidirectional analysis has a De Morgan dual, which can be used to link
different outputs generated from the same input. This offers a principled language-based foundation
for a popular interactive visualisation feature called brushing and linking where selections in one
chart automatically select corresponding elements in another related chart. Although such view
coordination features are valuable comprehension aids, they tend be to hard-coded into specific
applications or libraries, or require programmer effort.

## Paper and more information

 - Get [a copy of the paper from arXiv](https://arxiv.org/abs/2109.00445)
 - Download [a local copy of the paper (PDF)](linkedviz.pdf)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @inproceedings{linkedviz-popl22,
      author    = {Roly Perera and Minh Nguyen and Tomas Petricek and Meng Wang},
      title     = {Linked Visualisations via Galois Dependencies},
      booktitle = {Proceedings of Principles of Programming Languages Conference},
      series    = {POPL 2022},
      location  = {Philadelphia, United States}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
