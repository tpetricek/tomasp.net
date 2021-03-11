# F# Web Tools: Rich client/server web applications in F#

 - description:  This paper presents one of the first "Ajax" programming frameworks that let you develop
    integrated client/server applications in an integrated way using a single language. The
    framework lets you use F# on both sides and provides a smooth integration between client
    and server-side code.
 - tags: academic, publication, article
 - date: 30 April 2007
 - layout: article
 - title: F# Web Tools: Rich client/server web applications in F#
 - subtitle: Tomas Petricek. Unpublished draft, 2007
 

> Tomas Petricek, Don Syme
>
> Unpublished draft (2007)

"Ajax" programming is becoming a de-facto standard for certain types of web applications, but 
unfortunately developing this kind of application is a difficult task. Developers have to deal 
with problems like a language impedance mismatch, limited execution runtime for the code running 
in web browser on the client-side and no integration between client and server-side parts that 
are developed as a two independent applications, but typically form a single and homogenous application. 

In this paper we present the first project that deals with all three mentioned problems but which 
still integrates with existing web development technologies such as ASP.NET on the server and 
JavaScript on the client. We use the F# language for writing both client and server-side part 
of the web application, which lets us develop client-side code in a type-safe programming language 
using a subset of the F# library, and we provide a way to write both server-side and client-side 
code as a part of single homogeneous module defining the web page logic. This code is executed 
heterogeneously, part as JavaScript on the client, and part as native code on the server. Finally 
we use monadic syntax for the separation of client and server-side code, tracking this separation 
through the F# type system.   

## Article and more information

 - Download [the paper draft (PDF)](fswebtools-ml.pdf)
 - Downlaod [talk slides](fswebtools-v1.pdf) from Microsoft Research
 - The [source code](http://fswebtools.codeplex.com/) is no longer maintained, but is available

## <a id="cite">Bibtex</a>
This is an unreviewed article, but it still has some good ideas and inspired recent projects such as 
[WebSharper](http://www.websharper.com/) and [FunScript](http://www.funscript.info), 
so if you want to cite the article, you can use roughly the following:

    [lang=tex]
    @other{fswebtools,
      author       = {Petricek, Tomas and Syme, Don},
      title        = {F# Web Tools: Rich client/server web applications in F#},
      howpublished = {Unpublished draft, available online},
      year         = {2007},
      url          = {http://tomasp.net/academic/articles/fswebtools/fswebtools-ml.pdf},
    }

If you have any comments, suggestions or related ideas, I'll be happy to 
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
