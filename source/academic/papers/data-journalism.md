# The Gamma: Programming tools for data journalism

 - description: The Gamma simplifies the process of connecting data sources to interactive
    data visualizations. It allows readers not only to interact with predefined parameters
    but also to access the underlying source code, enabling transparency and reproducibility.
 - tags: academic, publication
 - layout: article
 - date: 3 October 2015
 - title: The Gamma: Programming tools for data journalism
 - subtitle: Tomas Petricek. Presented at Computation + Journalism Symposium 2015

> Tomas Petricek
>
> Presented at Computation + Journalism Symposium 2015

Data journalism encourages reader interaction. This is often done
through simple user interfaces. For more advanced readers, there is
typically a download with the raw data behind the visualization.
However, there is an interesting gap between the two. What if the
reader wants to change a parameter of a visualization that is not
exposed through the user interface? What if they want to re-create
the same visualization, but using data from a different source?

We believe that the fundamental reason for this inflexibility is
the fact that accessing data and building interactive visualizations
is a difficult programming problem. As a result, data journalists use
a wide range of tools, often involving manual steps, which makes
it hard to publish the entire process as a reproducible program.

In this paper, we present The Gamma, a project that reduces
the number of steps needed to link a data source to an end-user
visualization. The Gamma uses programming language techniques
to make data sources easier to access and to automatically build
user interfaces that let readers modify parameters of visualizations.
But behind the visualization and the user interface, there is full
source code, which makes reports transparent, more reproducible
and more accountable.

## Paper and more information

 - Download [the extended abstract (PDF)](extended-abstract.pdf)
 - [Talk slides from Cambridge Spark Data Science Summit 2017](http://tpetricek.github.io/Talks/2017/thegamma-data-science/)
 - For more information, see [The Gamma project homepage](http://thegamma.net)

## Watch the talk

I talked about The Gamma project as part of the [Fellow Short Talks series](https://www.youtube.com/channel/UCcr5vuAH5TPlYox-QLj4ySw)
organised by the Alan Turing Institute. This provides an accessible introduction to the project.
You can watch the talk below.

<div style="padding-left:20px">
<iframe width="640" height="360" src="https://www.youtube.com/embed/aHjgpmzFjOA" frameborder="0" allowfullscreen></iframe>
</div>

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @misc{data-journalism,
      author       = {Tomas Petricek},
      title        = {The Gamma: Programming Tools for Data Journalism},
      year         = {2015},
      howpublished = {Extended abstract presented at Computation + Journalism Symposium, New York},
      url          = {https://tomasp.net/academic/papers/data-journalism/extended-abstract.pdf},
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
