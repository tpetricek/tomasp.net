# Data exploration through dot-driven development

 - description: Data literacy is becoming increasingly important. While spreadsheets make
     simple data analytics accessible to a large number of people, creating transparent scripts
     requires expert programming skills. In this paper, we describe the design of a data
     exploration language that makes the task more accessible by embedding advanced programming
     concepts into a simple core language.
 - tags: academic, publication, top
 - layout: paper
 - date: 12 April 2017
 - title: Data exploration through dot-driven development
 - subtitle: Tomas Petricek. In proceedings of ECOOP 2017.

> Tomas Petricek
>
> In proceedings of ECOOP 2017

Data literacy is becoming increasingly important in the modern world. While spreadsheets make
simple data analytics accessible to a large number of people, creating transparent scripts that
can be checked, modified, reproduced and formally analyzed requires expert programming skills.
In this paper, we describe the design of a data exploration language that makes the task more
accessible by embedding advanced programming concepts into a simple core language.

The core language uses type providers, but we employ them in a novel way -- rather than providing
types with members for accessing data, we provide types with members that allow the user to also
compose rich and correct queries using just member access (``dot''). This way, we recreate
functionality that usually requires complex type systems (row polymorphism, type state and dependent
typing) in an extremely simple object-based language.

We formalize our approach using an object-based calculus and prove that programs constructed using
the provided types represent valid data transformations. We discuss a case study developed using the
language, together with additional editor tooling that bridges some of the gaps between programming
and spreadsheets. We believe that this work provides a pathway towards democratizing data science
-- our use of type providers significantly reduce the complexity of languages that one needs to
understand in order to write scripts for exploring data.

## Draft and more information

 - Download [pre-print of the paper (PDF)](pivot-ecoop17.pdf)
 - Watch [Fellow Short Talk](https://www.youtube.com/watch?v=aHjgpmzFjOA) from Alan Turing Institute

## Watch the talk

My talk about the paper has been pre-recorded at the Alan Turing Institute and so you can watch
it below. If you are looking for a more general introduction to The Gamma project, then consider
watching the [Fellow Short Talk](../storytelling) that is available for a more general paper about
the project.

<div style="padding-left:20px">
<iframe width="640" height="360" src="https://www.youtube.com/embed/hel0ElbGong" frameborder="0" allowfullscreen></iframe>
</div>

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @inproceedings{pivot-ecoop2017,
      author    = {Petricek, Tomas},
      title     = {Data exploration through dot-driven development},
      booktitle = {European Conference on Object-Oriented Programming},
      series    = {ECOOP 2017},
      location  = {Barcelona, Spain},
      year      = {2017}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
