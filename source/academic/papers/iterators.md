# Encoding monadic computations in C# using iterators

 - subtitle: Tomas Petricek. In Proceedings of ITAT 2009
 - description: The paper shows how to encode certain monadic computations (such as a continuation monad
     for asynchronus programming) using the iterator language feature in C# 2.0.
 - tags: academic, publication
 - layout: paper
 - date: 1 June 2009
 - title: Encoding monadic computations in C# using iterators

> Tomas Petricek
>
> In Proceedings of ITAT 2009

Many programming problems can be easily solved if we express them as computations 
with some non-standard aspect. This is a very important problem, because today we 
are struggling to efficiently program multi-core processors and to write asynchronous 
code. Unfortunately main-stream languages such as C# do not support any direct way 
for encoding unrestricted non-standard computations. In languages like Haskell and F#, 
this can be done using monads with syntactic extensions they provide and it has been 
successfully applied to a wide range of real-world problems. 

In this paper, we present 
a general way for encoding monadic computations in the C# 2.0 language with a convenient 
syntax using a specific language feature called iterators. This gives us a way to use 
well-known non-standard computations enabling easy asynchronous programming or for 
example the use of software transactional memory in plain C#. Moreover, it also opens 
monads in general to a wider audience which can help in the search for other useful and 
previously unknown kinds of computations.

## Paper and more information

 - Download [the paper (PDF)](iterators.pdf) and [supplementary materials (PDF)](iterators-sup.pdf)
 - Look at the [C# source code (ZIP)](iterators-src.zip)
 - For more information read a [related blog post](http://tomasp.net/blog/csharp-async.aspx)
 - View the slides from [ITAT talk (PDF)](iterators-itat.pdf)

## Corrections
This paper contains a number of issues. Most importantly, it ignores the fact that 
C# 2.0 iterators keep mutable state, which makes running a continuation multiple times
harder (as a result, it is only possible to easily encode one-shot continuations).

This limits the applicability of this approach. However, the paper is still interesting,
especially since many main-stream languages (including Python and JavaScript) are 
attempting (as of 2013) to apply the approaches discussed in the paper.


## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the [paper proceedings in CEUR-WS](http://ceur-ws.org/Vol-584/).

    [lang=tex]
    @inproceedings{iterators-itat09,
      author    = {Petricek, Tomas},
      title     = {Encoding monadic computations in C\# using iterators},
      booktitle = {Proceedings of the Conference on Theory and 
                   Practice of Information Technologies},
      series    = {ITAT 2009},
      location  = {Kralova studna, Slovakia},
    } 

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
