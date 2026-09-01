# Design Choices of Document-Oriented Programming Systems

 - description: Many interesting programming systems treat programming as document manipulation.
    This paper describes the key design choices that characterise document-oriented programming
    systems, covering systems such as Boxer, Jupyter notebooks, or spreadsheets.
 - tags: academic, publication, draft
 - icon: fa-regular fa-file-word
 - layout: article
 - date: 1 September 2026
 - title: Design Choices of Document-Oriented Programming Systems
 - subtitle: Tomas Petricek, Jonathan Edwards, Joel Jakubovic. Unpublished Draft


> Tomas Petricek, Jonathan Edwards, Joel Jakubovic
>
> Unpublished Draft.


Many interesting programming systems treat programming as document manipulation.
Examples include spreadsheets, data science notebooks, educational environments like Boxer and
multiple recent research programming systems. In such systems, the programmer interacts with a
document containing a structured representation of both code and data. They modify the code
and data, trigger computations and view the results from the unified document interface.
Those _document-oriented programming systems_ have a specific set of design choices. The
concepts that we need to understand them and design them differ from the well-understood design
choices known from programming languages and other programming systems.

The aim of this paper is to identify the key design choices that characterise different
document-oriented programming systems. We review both historical and recent examples of such
systems and identify twelve design choices that cover four aspects of the system design:
(i) what structure and representation of document they use,
(ii) how is programming embedded within the systems,
(iii) how the user interface displays documents and allows for their editing,
and (iv) how are computations within the system evaluated.

The catalogue refines our earlier work on technical dimensions of programming systems.
It is rooted in our need to understand the design choices and their consequences when
designing multiple different document-oriented programming systems over the last multiple years.
The catalogue provides a high-level map of the design space of document-oriented programming systems,
makes it possible to identify differences and similarities across different systems and also
suggests under-explored design choices and combinations of choices as areas for future research.

The key contribution of this work is perhaps not the catalogue of design choices itself, but the
fact that we identify a new programming paradigm. Document-oriented programming systems have
rich historical roots, widely-adopted contemporary examples, but they are also an active research
area. We hope the review presented in this paper will aid future development of this new paradigm.


## Draft and more information

 - Download [paper draft (PDF)](paper.pdf)
 - Send your feedback and corrections [via GitHub](https://github.com/d3sprog/design-choices/)



## Comments are welcome!

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via BlueSky at [@tomasp.net](https://bsky.app/profile/tomasp.net).
