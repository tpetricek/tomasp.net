# Baseline: Operation-Based Evolution and Versioning of Data

 - description: Baseline is a platform for richly structured data supporting change in multiple 
    dimensions: mutation over time, collaboration across space, and evolution through design 
    changes. It is built upon operational differencing, a new technique for managing data in 
    terms of high-level operations that include refactorings and schema changes.
 - tags: academic, publication, draft
 - icon: fas fa-database
 - layout: article
 - date: 1 September 2026
 - title: Baseline: Operation-Based Evolution and Versioning of Data
 - subtitle: Jonathan Edwards, Tomas Petricek. Unpublished Draft


> Jonathan Edwards, Tomas Petricek
>
> Unpublished Draft.


Baseline is a platform for richly structured data supporting change in multiple dimensions: mutation 
over time, collaboration across space, and evolution through design changes. It is built upon 
_Operational Differencing_, a new technique for managing data in terms of high-level operations 
that include refactorings and schema changes. We use operational differencing to construct an 
operation-based form of version control on data structures used in programming languages and 
relational databases.

This approach to data version control offers high-fidelity diffing and merging despite intervening 
structural transformations like schema changes. It offers users a simplified conceptual model of 
version control for ad hoc usage: There is no repo; Branching is just copying. The information 
maintained in a repo can be synthesized more precisely from the append-only histories of 
branches. Branches can be flexibly shared as is commonly done with document files, except with the 
added benefit of diffing and merging.

We conjecture that queries can be _operationalized_ into a sequence of schema and data operations. 
We develop that idea on a query language fragment containing selects and joins.
Operationalized queries are represented as a _future timeline_ that is speculatively executed as a 
branch off of the present state, returning a value from its _hypothetical future_. Operationalized 
queries get rewritten to accommodate schema change "for free" by the machinery of operational differencing.

We discuss the design of Baseline's direct manipulation UI supporting interactive schema changes 
as well as diffing and merging between databases.
We evaluate Baseline with case studies of schema evolution problems identified iin a literature 
review, including the set of challenge problems we identified in a prior paper.


## Draft and more information

 - Download [the paper draft (PDF)](paper.pdf)
 - Check out [the interactive Baseline demo!](https://thebaseline.dev/Prog26submission/)
 - Send your feedback and corrections [via GitHub](https://github.com/JonathanMEdwards/prog26)

## Comments are welcome!

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via BlueSky at [@tomasp.net](https://bsky.app/profile/tomasp.net).
