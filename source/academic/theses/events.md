# Reactive Programming with Events (Master thesis)

 - subtitle: Master thesis. Charles University in Prague, 2010
 - description: The thesis uses early version of joinad language extension for F# and garbage collection
     for events. Based on these concepts, it builds a simple reactive library for F# that allows
     writing reactive computations in both control-flow and data-flow styles.
 - tags: academic, publication, thesis
 - date: 17 August 2010
 - layout: paper
 - title: Reactive Programming with Events (Master thesis)

> Supervised by Don Syme (Microsoft Research, Cambridge)
>
> Charles University in Prague, 2010

The reactive programming model is largely different to what we're used to as we 
don't have a full control over the application's control flow. As a result, reactive 
applications need to be structured differently and need to be written using different 
patterns. We build an easier way for developing reactive applications. Our work 
integrates declarative and imperative approach to reactive programming and uses the 
notion of event as the unifying concept. We discuss the problem of memory management 
in this scenario and develop a technique for collecting not only events that are not 
reachable, but also events that cannot trigger any action. Next, we present a language 
extension that makes it possible to wait for occurrences of events that match some 
defined pattern. The extension isn't bound to the reactive programming model and can 
be used for concurrent and parallel programming scenarios as well. Finally, we develop 
a reactive programming library that builds on top of the ideas discussed earlier and 
we present semantics of the library to enable formal reasoning about reactive programs.

## Thesis and more information

 - Download [the thesis (PDF)](events.pdf)
 - For garbage collection of events see [ISMM 2010 paper](../../papers/hollywood/)
 - For pattern matching on monads see [PADL 2011 paper](../../papers/joinads/)

## <a id="cite">Bibtex</a>
Most of the (interesting) work from the thesis has been better described in the
ISMM 2010 and PADL 2011 papers mentioned above, but the thesis can be cited using
the following information:

    [lang=tex]
    @@MastersThesis{events-thesis,
      author = {Petricek, Tomas},
      title  = {Reactive {P}rogramming with {E}vents},
      school = {Charles University in Prague},
      year   = {2010}
    }

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
