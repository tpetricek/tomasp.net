# Data exploration through dot-driven development

 - description: Data literacy is becoming increasingly important. While spreadsheets make 
     simple data analytics accessible to a large number of people, creating transparent scripts
     requires expert programming skills. In this paper, we describe the design of a data 
     exploration language that makes the task more accessible by embedding advanced programming 
     concepts into a simple core language.
 - tags: academic, publication
 - layout: paper
 - date: 12 April 2017
 - title: Data exploration through dot-driven development
 - subtitle: Tomas Petricek. To appear in proceedings of ECOOP 2017.
 
> Tomas Petricek
>
> To appear in proceedings of ECOOP 2017

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
 
## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @inproceedings{pivot-ecoop2017,
      author    = {Petricek, Tomas},
      title     = {Data exploration through dot-driven development},
      booktitle = {European Conference on Object-Oriented Programming},
      series    = {ECOOP 2017},
      location  = {Barcelona, Spain}
    }

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
