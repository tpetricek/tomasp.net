# Collecting Hollywood's Garbage: Avoiding Space-Leaks in Composite Events


 - description:  The article discusses memory leaks that can occur in a reactive programming model based on events.
    It presents a formal garbage collection algorithm that could be used in this scenario and a
    correct reactive library based on this idea, implemented in F#.
 - date: 11 February 2010
 - tags: academic, publication
 - layout: paper
 - title: Collecting Hollywood's Garbage: Avoiding Space-Leaks in Composite Events
 - subtitle: Tomas Petricek and Don Syme. In Proceedings of ISMM 2010.
 

> Tomas Petricek and Don Syme
>
> In Proceedings of ISMM 2010

The reactive programming model is largely different to what we're used to as we don't 
have full control over the application's control flow. If we mix the declarative and 
imperative programming style, which is usual in the ML family of languages, the situation 
is even more complex. It becomes easy to introduce patterns where the usual garbage collector 
for objects cannot automatically dispose all components that we intuitively consider garbage.

In this paper we discuss a duality between the definitions of garbage for objects and events. 
We combine them into a single one, to specify the notion of garbage for reactive programming 
model in a mixed functional/imperative language and we present a formal algorithm for collecting 
garbage in this environment.

Building on top of the theoretical model, we implement a library for reactive programming that 
does not cause leaks when used in the mixed declarative/imperative model. The library allows us 
to safely combine both of the reactive programming patterns. As a result, we can take advantage 
of the clarity and simplicity of the declarative approach as well as the expressivity of the 
imperative model.

## Paper and more information

 - Download [the paper from ACM (free PDF)](http://dl.acm.org/authorize?357245), or use a [local backup (PDF)](hollywood.pdf)
 - View [slides from ISMM 2010 (PDF)](ismm-talk.pdf)
 - Read a [related blog post](http://tomasp.net/blog/event-object-duality.aspx)

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information, or
get full details from the [paper page on ACM](http://dl.acm.org/citation.cfm?id=1806651.1806662&coll=DL&dl=GUIDE&CFID=375487526&CFTOKEN=86636259).

    [lang=tex]
    @@inproceedings{hollywood-ismm10,
      author    = {Petricek, Tomas and Syme, Don},
      title     = {Collecting {H}ollywood's garbage: 
                    {A}voiding space-leaks in composite events},
      booktitle = {Proceedings of International Symposium on Memory Management},
      series    = {ISMM 2010},
      location  = {Toronto, Ontario, Canada},
    }

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@@tomaspetricek](http://twitter.com/tomaspetricek).
