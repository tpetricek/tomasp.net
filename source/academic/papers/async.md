# The F# Asynchronous Programming Model

 - subtitle: Don Syme, Tomas Petricek and Dmitry Lomov. In PADL 2011.
 - description: We describe the asynchronous programming model in F#, and its applications. It combines a core
     language with a non-blocking modality to author lightweight asynchronous tasks. This allows smooth
     transitions between synchronous and asynchronous code and eliminates the inversion of control.
 - date: 18 January 2011
 - tags: academic, publication
 - layout: paper
 - title: The F# Asynchronous Programming Model

> Don Syme, Tomas Petricek and Dmitry Lomov
>
> In Proceedings of PADL 2011

We describe the asynchronous programming model in F#, and its applications to reactive, 
parallel and concurrent programming. The key feature combines a core language with a 
non-blocking modality to author lightweight asynchronous tasks, where the modality has 
control flow constructs that are syntactically a superset of the core language and are 
given an asynchronous semantic interpretation. 

This allows smooth transitions between synchronous and asynchronous code and eliminates 
callback-style treatments of inversion of control, without disturbing the foundation of 
CPU-intensive programming that allows F# to interoperate smoothly and compile efficiently. 
An adapted version of this approach has recently been announced for a future version of C#.


## Paper and more information

 - Download [the paper (PDF)](async.pdf)
 - Read more on [Don Syme's blog](http://blogs.msdn.com/b/dsyme/archive/2010/10/21/the-f-asynchronous-programming-model-padl-2010-pre-publication-draft.aspx)
 - Watch an earlier [interview with Don Syme](http://channel9.msdn.com/Blogs/Charles/Don-Syme-Whats-new-in-F-Asynchronous-Workflows-and-welcome-to-the-NET-family)
 
## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the [paper page on ACM](http://dl.acm.org/citation.cfm?id=1946313.1946334&coll=DL&dl=GUIDE&CFID=375487526&CFTOKEN=86636259).

    [lang=tex]
    @@inproceedings{async-padl11,
      author    = {Syme, Don and Petricek, Tomas and Lomov, Dmitry}, 
      title     = {The {F}\# {A}synchronous {P}rogramming {M}odel},
      booktitle = {Proceedings of Practical Aspects of Declarative Languages},
      series    = {PADL 2011},
      location  = {Austin, TX, USA},
    } 

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@@tomaspetricek](http://twitter.com/tomaspetricek).
