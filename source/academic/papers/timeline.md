# Timeline: Adding the Time Dimension to Spreadsheets

 - description: Timeline extends spreadsheets with discrete time, making it possible to use
     the powerful spreadsheet paradigm for implementing physics simulations, agent-based
     modelling, financial data analyses and even simple interactive games!
 - tags: academic, publication, top, home
 - icon: fas fa-table
 - layout: article
 - date: 6 October 2026
 - title: Timeline: Adding the Time Dimension to Spreadsheets
 - subtitle: Tomas Petricek, Tomáš Boďa. In Proceedings of OOPSLA 2026


> Tomas Petricek, Tomáš Boďa
>
> In Proceedings of OOPSLA 2026

Spreadsheets make it easy to express computations over two-dimensional data, but two dimensions
are not enough to express rich computations involving time such as physics simulations, agent-based
modelling, financial data analyses, or simple interactive applications.

We present Timeline, a system that adds discrete time to spreadsheets. We motivate the design
of Timeline through a small-scale formative study, present a _core calculus_ that
models system runtime, use it to formalise a static analysis to determine the required number of
past values, and evaluate the system using a series of case studies from the four aforementioned
areas.

The paper also shows that a large number of advanced programming language concepts
can be directly applied in the context of spreadsheets with time. Timeline draws from
_dataflow languages_ to express computations over time, _coeffect systems_ to ensure
bounded memory usage, _functional reactive programming_ to support interactivity, and
_grammars of graphics_ to support composable visualizations.

## Paper and more information

 - Download the [paper (PDF)](paper.pdf)
 - Check out the [Timeline project homepage](https://timelinesheets.com/)
 - Live demo: [Planetary orbit simulation](https://timelinesheets.com/spreadsheet/1fdc6e97-b1fd-4f49-9623-d885dd541199)
 - Live demo: [Flocking simulation](https://timelinesheets.com/spreadsheet/c9b53ee9-0f1b-4d5d-a73e-19f0d201e204)
 - Live demo: [Moving average crossover](https://timelinesheets.com/spreadsheet/e653e57b-9ba9-463d-9f2b-44ddd5a426d3)
 - Live demo: [Flappy Bird clone](https://timelinesheets.com/spreadsheet/1187c0bd-fe59-4efd-9541-622f35587031)


## Teaser figure

<img src="flappy.png" style="max-width:94%;margin:10px 3% 10px 3%" />

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @article{petricek-2026-timeline,
      author    = {Tomas Petricek and Tomáš Boďa},
      title     = {Timeline: Adding the Time Dimension to Spreadsheets},
      journal   = {Proc. {ACM} Program. Lang.},
      volume    = {10},
      number    = {{OOPSLA2}},
      articleno = {370},
      year      = {2026},
      doi       = {10.1145/3839502}
    }
    
If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via BlueSky at [@tomasp.net](https://bsky.app/profile/tomasp.net).
