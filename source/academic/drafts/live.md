# Design and implementation of a live coding environment for data science

 - description: Data science can be done by directly manipulating data using spreadsheets, or by 
    writing data manipulation scripts using a programming language. The former is error-prone and
    does not scale, while the latter requires expert skills. Live coding has the potential to 
    bridge this gap and make writing of transparent, reproducible scripts more accessible.
 - tags: academic, publication, draft
 - layout: paper
 - date: 15 November 2017
 - title: Design and implementation of a live coding environment for data science
 - subtitle: Tomas Petricek. Unpublished draft.
 

> Tomas Petricek
>
> Unpublished draft

Data science can be done by directly manipulating data using spreadsheets, or by writing 
data manipulation scripts using a programming language. The former is error-prone and
does not scale, while the latter requires expert skills. Live coding has the potential to 
bridge this gap and make writing of transparent, reproducible scripts more accessible.

In this paper, we describe a live programming environment for data science that provides instant 
previews and contextual hints, while allowing the user to edit code in an unrestricted way in
a text editor. 

Supporting a text editor is challenging as any edit can significantly change the structure of code and
fully recomputing previews after every change is too expensive. We present a technique that allows the type
checker and the interpreter to run on the fly, while code is written, and reuse results of previous 
runs. This makes it possible to efficiently provide instant feedback and live previews during development.

We formalise how programs are interpreted and how previews are computed, prove correctness
of the previews and formally specify when can previews be reused. We believe this work provides
solid and easy to reuse foundations for the emerging trend of live programming environments.

## Draft and more information

 - Download [the draft paper (PDF)](live.pdf)

## Comments are welcome!

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
