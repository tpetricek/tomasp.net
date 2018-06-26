# Wrattler: Reproducible, live and polyglot notebooks

 - description: Notebook systems such as Jupyter are a popular environment for data science,
     but they use an architecture that leads to a limited model of interaction and makes 
     versioning and reproducibility difficult. Wrattler revisits the architecture and allows
     richer forms of interactivity, efficient evaluation and guaranteed reproducibility.
 - tags: academic, publication
 - layout: paper
 - title: Wrattler: Reproducible, live and polyglot notebooks
 - date: 26 June 2018
 - subtitle: Tomas Petricek, James Geddes, Charles Sutton. In Proceedings of TaPP 2018
 
> Tomas Petricek, James Geddes, Charles Sutton
>
> In Proceedings of 10th USENIX Workshop on The Theory and Practice of Provenance (TaPP 2018)
  
Notebooks such as Jupyter became a popular environment for data science, because 
they support interactive data exploration and provide a convenient way of interleaving code, 
comments and visualizations. Alas, most notebook systems use an architecture that leads to a 
limited model of interaction and makes reproducibility and versioning difficult.

In this paper, we present Wrattler, a new notebook system built around provenance that addresses
the above issues. Wrattler separates state management from script evaluation and controls the 
evaluation using a dependency graph maintained in the web browser. This allows richer forms of 
interactivity, an efficient evaluation through caching, guarantees reproducibility and makes it 
possible to support versioning.

## Paper and more information

 - Download [the paper (PDF)](wrattler.pdf)
   
## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @inproceedings{wrattler,
      author    = {Petricek, Tomas and Geddes, James and Sutton, Charles},
      title     = {Wrattler: Reproducible, live and polyglot notebooks},
      booktitle = {Proceedings of 10th USENIX Workshop on The Theory and Practice of Provenance},
      series    = {TaPP 2018},
      location  = {London, UK}
    } 

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
