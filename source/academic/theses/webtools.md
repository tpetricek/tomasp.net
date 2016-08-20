# Client-side Scripting using Meta-programming (Bachelor thesis)


 - description:  The thesis presents a web development framework for F# that automatically splits a single F#
    program with monadic modality annotations into client-side JavaScript and server-side ASP.NET
    application.
 - tags: academic, publication, thesis
 - layout: paper
 - title: Client-side Scripting using Meta-programming (Bachelor thesis)
 - subtitle: Bachelor thesis. Charles University in Prague, 2007
 - date: 14 May 2007
 

> Supervised by David Bednarek
>
> Charles University in Prague, 2007

"Ajax" programming is becoming a de-facto standard for certain types of web 
applications, but unfortunately developing this kind of application is a difficult task. 
Developers have to deal with problems like a language impedance mismatch, limited 
execution runtime in web browser on the client-side and no integration between client 
and server-side parts that are developed as a two independent applications, but 
typically form a single and homogenous application.

In this work we present the first project that deals with all three mentioned problems 
but which still integrates with existing web technologies such as ASP.NET on the server 
and JavaScript on the client. We use the F# language for writing both client and 
server-side part of the web application, which makes it possible to develop client-side 
code in a type-safe programming language using a subset of the F# library, and we 
provide a way to write both server-side and client-side code as a part of single 
homogeneous type defining the web page logic. The code is executed heterogeneously, 
part as JavaScript on the client, and part as native code on the server. Finally we 
use monadic syntax for the separation of client and server-side code, tracking this 
separation through the F# type system.

## Thesis and more information

 - Download [the thesis (PDF)](webtools.pdf)
 - Download brief [technical report (PDF)](webtools-report.pdf)
 - Look at the slides from [internship talk at MSR (PDF)](webtools-msr.pdf)

## Related projects

Ideas from this work inspired an industrial product [WebSharper](http://www.intellifactory.com/)
that delivers much of the functionality described above. In the open-source world,
the [FunScript project](http://funscript.info/) provides a way to translate F#
to JavaScript with the support for asynchronous computations, but provides a novel
technique for integration with JavaScript and server-side data-sources using
F# type providers (as of 2013).

## <a id="cite">Bibtex</a>
Together with [the Links project](http://groups.inf.ed.ac.uk/links/), the project was one of the
first tools for client/server web programming using statically typed programming language,
so if you want to cite the work, you can use the following BibTeX information:

    [lang=tex]
    @@MastersThesis{fswebtools-thesis,
      author = {Petricek, Tomas},
      type   = {Bachelor Thesis},
      title  = {Client-side Scripting using Meta-programming},
      school = {Charles University in Prague},
      year   = {2007}
    }

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@@tomaspetricek](http://twitter.com/tomaspetricek).
