# Interaction vs. Abstraction: Managed Copy and Paste

 - description: Inspired by version control systems like git, we
    propose managed copy & paste, in which the programming environment records copy & paste operations,
    along with structural edit operations, so that it can track the differences between copies and
    reconcile them on command.
 - tags: academic, publication
 - layout: article
 - icon: fa fa-paste
 - date: 5 December 2022
 - title: Interaction vs. Abstraction: Managed Copy and Paste
 - subtitle: Jonathan Edwards, Tomas Petricek. Proceedings of PAINT 2022

> Jonathan Edwards, Tomas Petricek
>
> In Proceedings of Programming Abstractions and  
> Interactive Notations, Tools, and Environments (PAINT) 2022

Abstraction is at the core of programming, but it has a cost. We exhort programmers to use proper
abstractions like functions but they often find it easier to copy & paste instead. Copy & paste
is roundly criticized because subsequent changes to copies may have to be manually reconciled,
which is easily overlooked and easily mistaken. It seems there is a conflict between the generality
and reusability of abstraction with the simplicity of copying and modifying code.

We suggest that this conflict arises because we are still thinking in terms of paper-based notations.
Indeed the term "copy & paste" originates from the practice of physically cutting and gluing slips
of paper. But an interactive programming environment can free us from the limitations of paper. We
propose managed copy & paste, in which the programming environment records copy & paste operations,
along with structural edit operations, so that it can track the differences between copies and
reconcile them on command. These capabilities mitigate the aforementioned problems of copy & paste,
allowing abstraction to be deferred or reduced.

Managed copy & paste resembles version control as in git, except that it works not between versions
of a program but between copies within the program. It is based on a new theory of structural
editing and version control that offers precise differencing based on edit history rather than
the heuristic differencing of textual version control. We in- formally explain this theory and
demonstrate a prototype implementation of a data science notebook. Lastly, we suggest further
mechanisms of gradual abstraction that could be provided by the programming environment to
lessen the cognitive load of programming.

## Paper and more information

 - Download [paper pre-print (PDF)](paint22.pdf)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @inproceedings{copypaste-paint22,
      author    = {Edwards, Jonathan and Petricek, Tomas},
      title     = {Interaction vs. Abstraction: Managed Copy and Paste},
      booktitle = {Proceedings of Programming Abstractions and Interactive
                   Notations, Tools, and Environments},
      series    = {PAINT 2022},
      location  = {Auckland, New Zealand},
      year      = {2022}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
