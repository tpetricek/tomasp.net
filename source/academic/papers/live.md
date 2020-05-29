# Foundations of a live data exploration environment

 - description: The way data analysts write code is different from the way software engineers do so.
    They use few abstractions, work interactively and rely heavily on external libraries.
    We capture this way of working and build a programming environment that makes data exploration
    easier by providing instant live feedback.
 - tags: academic, publication, top
 - layout: paper
 - date: 1 March 2020
 - title: Foundations of a live data exploration environment
 - subtitle: Tomas Petricek. The Art, Science, and Engineering of Programming, 2020

> Tomas Petricek
>
> The Art, Science, and Engineering of Programming, 2020

A growing amount of code is written to explore and analyze data, often by data analysts
who do not have a traditional background in programming, for example by journalists.
The way such data anlysts write code is different from the way software engineers do so.
They use few abstractions, work interactively and rely heavily on external libraries.
We aim to capture this way of working and build a programming environment that makes data exploration
easier by providing instant live feedback.

We combine theoretical and applied approach. We present the \emph{data exploration calculus},
a formal language that captures the structure of code written by data analysts. We then
implement a data exploration environment that evaluates code instantly during editing and shows
previews of the results.
We formally describe an algorithm for providing instant previews for the data exploration calculus
that allows the user to modify code in an unrestricted way in a text editor. Supporting interactive
editing is tricky as any edit can change the structure of code and fully recomputing the output
would be too expensive.
We prove that our algorithm is correct and that it reuses previous results when updating previews
after a number of common code edit operations. We also illustrate the practicality of our approach
with an empirical evaluation and a case study.

As data analysis becomes an ever more important use of programming, research on programming languages
and tools needs to consider new kinds of programming workflows appropriate for those domains and
conceive new kinds of tools that can support them. The present paper is one step in this important direction.

## Paper and more information

 - See [the paper record](https://programming-journal.org/2020/4/8/) in The Art, Science, and Engineering of Programming journal.
 - Download [local copy of the final version of the paper (PDF)](paper.pdf)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @article{monads,
      author  = {Petricek, Tomas},
      title   = {Foundations of a live data exploration environment},
      journal = {The Art, Science, and Engineering of Programming},
      year    = {2020}
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
