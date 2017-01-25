# Types from data: Making structured data first-class citizens in F#

 - description:  The paper presents [F# Data](http://fsharp.github.io/FSharp.Data), a library of type providers
      that integrate external data in XML, CSV and JSON formats into the type system of the F# language.
      F# Data infers the shape of structured documents and uses it to guarantee a relative safety property.
 - tags: academic, publication
 - layout: paper
 - title: Types from data: Making structured data first-class citizens in F#
 - subtitle: Tomas Petricek, Gustavo Guerra and Don Syme. In Proceedings of PLDI 2016
 - date: 1 May 2016

> Tomas Petricek, Gustavo Guerra, Don Syme 
>
> In proceedings of PLDI 2016

Most modern applications interact with external services and access data in structured formats such
as XML, JSON and CSV. Static type systems do not understand such formats, often making data access
more cumbersome. Should we give up and leave the messy world of external data to dynamic typing
and runtime checks? Of course, not!

We present F# Data, a library that integrates external structured data into F#. As most real-world
data does not come with an explicit schema, we develop a shape inference algorithm that infers a
shape from representative sample documents. We then integrate the inferred shape into the F# type
system using type providers. We formalize the process and prove a relative type soundness theorem.

Our library significantly reduces the amount of data access code and it provides additional
safety guarantees when contrasted with the widely used weakly typed techniques.

## Paper and more information

 - Download [the paper (PDF)](fsharp-data.pdf)
 - [Supplementary screencast](https://vimeo.com/165159144) shows the library in action
 - For more info, see [F# Data homepage](http://fsharp.github.io/FSharp.Data/)
 - [Poster about F# Data (PDF)](fsharp-data-poster.pdf) from Student Research Competition

## Watch the talk

As far as I'm aware, the PLDI 2016 talk has not been recorded, but I did a brief practical 
demonstration of the F# Data library, similar to one that I did in a [number of industry 
talks](http://fsharpworks.com/materials.html). One good talk to watch has been recorded by 
Microsoft's Channel 9 team:

<div style="padding-left:20px">
<iframe src="https://channel9.msdn.com/posts/Understanding-the-World-with-F/player" width="640" height="360" allowFullScreen frameBorder="0"></iframe>
</div>

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the paper [paper page on ACM](http://dl.acm.org/citation.cfm?id=2908115)

    [lang=tex]
    @inproceedings{fsharp-data-pldi2016,
      author    = {Petricek, Tomas and Guerra, Gustavo and Syme, Don},
      title     = {Types from data: Making structured
                   data first-class citizens in F\#},
      booktitle = {Proceedings of Conference on Programming
                   Language Design and Implementation},
      series    = {PLDI 2016},
      location  = {Santa Barbara, California, USA}
    }


If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
