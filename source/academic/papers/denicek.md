# Denicek: Computational Substrate for Document-Oriented End-User Programming

 - description: Building programming systems that support programming by demonstration,
     collaborative editing, incremental recomputation and structure editing is hard!
     Denicek is a computational substrate based on documents and history of edit actions
     that simplifies the task.
 - tags: academic, top, publication, home
 - layout: article
 - icon: fa fa-layer-group
 - date: 30 September 2025
 - title: Denicek: Computational Substrate for Document-Oriented End-User Programming
 - subtitle: Tomas Petricek and Jonathan Edwards. In Proceedings of UIST 2025

> Tomas Petricek, Jonathan Edwards
>
> In Proceedings of UIST 2025

User-centric programming research gave rise to a variety of compelling programming experiences,
including collaborative source code editing, programming by demonstration, incremental
recomputation, schema change control, end-user debugging and concrete programming. Those
experiences advance the state of the art of end-user programming, but they are hard to implement on
the basis of established programming languages and system.

We contribute Denicek, a computational substrate that simplifies the implementation of the above
programming experiences. Denicek represents a program as a series of edits that construct and
transform a document consisting of data and formulas. Denicek provides three operations on edit
histories: edit application, merging of histories and conflict resolution. Many programming
experiences can be easily implemented by composing these three operations.

We present the architecture of Denicek, discuss key design considerations and elaborate the
implementation of a variety of programming experiences. To evaluate the proposed substrate, we use
Denicek to develop an innovative interactive data science notebook system. The case study shows
that the Denicek computational substrate provides a suitable basis for the design of rich,
interactive end-user programming systems.

## Paper and more information

 - Download [the paper (PDF)](uist-2025.pdf)
 - View [the paper in ACM DL](https://dl.acm.org/doi/10.1145/3746059.3747646)
 - Scroll down to [supplementary videos](https://dl.acm.org/doi/10.1145/3746059.3747646#supplementary-materials)
 - [Talk slides from UIST 2025](https://1drv.ms/p/c/6ddff5260c96e30a/EYgdvAyrLHxPvKmeAEhqHuoBimeL_vkZ2DPWZfs4onGe0w?e=RoZbu0)
 - Check out the [source code on GitHub](https://github.com/d3sprog/denicek)

## Watch the talk

Check out the 5-minute video that came with our UIST 2025 submission. You can also
take a look at the [30-second teaser video](https://www.youtube.com/watch?v=vIbrmZ_ux_c).

<div style="padding-left:20px">
<iframe width="640" height="360" src="https://www.youtube.com/embed/xNl7XSgGZpY?si=xqXL8afJhzgl4Ou6" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>
</div>

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @inproceedings{10.1145/3746059.3747646,
      author = {Petricek, Tomas and Edwards, Jonathan},
      title = {Denicek: Computational Substrate for
        Document-Oriented End-User Programming},
      year = {2025},
      isbn = {9798400720376},
      publisher = {Association for Computing Machinery},
      doi = {10.1145/3746059.3747646},
      booktitle = {Proceedings of the 38th Annual ACM
        Symposium on User Interface Software and Technology},
      series = {UIST '25}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
