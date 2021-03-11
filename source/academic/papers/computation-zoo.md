# The F# Computation Expression Zoo

 - description:  F# computation expressions F# provide a _uniform_ syntax for computations such as
      monads, monad transformers and applicative functors. The syntax is _adaptable_ and
      close to built-in language features of Python and C#. This article provides the details
      shows that the right syntax can often be determined by considering laws.
 - tags: academic, publication
 - layout: article
 - title: The F# Computation Expression Zoo
 - subtitle: Tomas Petricek and Don Syme. In Proceedings of PADL 2014.
 - date: 3 February 2014

> Tomas Petricek and Don Syme
>
> In Proceedings of PADL 2014

Many computations can be structured using abstract computation types such as monoids, monad transformers or
applicative functors. Functional programmers use those abstractions directly while main-stream
languages often integrate concrete instances as language features - e.g. generators in Python
or asynchronous computations in C# 5.0. The question is, is there a sweet spot between
convenient, hardwired language features, and an inconvenient but flexible libraries?

F# _computation expressions_ answer this question in the affirmative. Unlike
the `do` notation in Haskell, computation expressions are not tied to a single kind of abstraction.
They support a wide range of computations, depending on what operations are available. They also
provide greater syntactic flexibility leading to a more intuitive syntax, without resorting to
full macro-based meta-programming.

We show that computation expressions can structure well-known computations including monoidal
list comprehensions, monadic parsers, applicative formlets and asynchronous sequences based on the
list monad transformer. We also present typing rules for computation expressions that are capable of
capturing all these applications.


## Paper and more information

 - Download [the paper (PDF)](computation-zoo.pdf)
 - View [poster from TFP 2012](poster-tfp.pdf)
 - View [talk slides from TFP 2012](talk-tfp.pdf)

## Related papers

An earlier draft of the paper appeared in pre-proceedings of TFP 2012. The PADL 2014 version
differs in that it emphasizes the different abstractions that can be encoded (over different
applications) and it discusses laws in more details, as well as how to use laws to choose
between different syntactic options.
The older draft might still be interesting as it provides more tutorial style overview,
especially for those already familiar with F#:

 -  [Syntax Matters: Writing abstract computations in F#](syntax-matters.pdf) (PDF)
    (Tomas Petricek and Don Syme, In Pre-proceedings of TFP 2012)

## Try Joinads

<img src="tryjoinads.png" style="float:right;margin:0px 20px 0px 30px" />

[Try Joinads](http://tryjoinads.org) is a web site, using the
[open-source release of F#](https://github.com/fsharp/fsharp) that comes
with a browser-based F# console where you can experiment with the computations
described in the paper and explore the full implementation. It also implements
syntax for _applicative functors_ that is not available in the current version of F#.

 - See the [paper page on Try Joinads](http://tryjoinads.org/index.html?computations/home.html)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the [paper page on ACM](#).

    [lang=tex]
    @inproceedings{computation-zoo-padl14,
      author    = {Petricek, Tomas and Syme, Don},
      title     = {The F\# Computation Expression Zoo},
      booktitle = {Proceedings of Practical Aspects of Declarative Languages},
      series    = {PADL 2014},
      location  = {San Diego, CA, USA},
      year      = {2014}
    }


If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
