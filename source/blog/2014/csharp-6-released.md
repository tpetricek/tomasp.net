BREAKING: Open-source C# 6.0 released
=====================================

 - date: 2014-04-01T14:24:30.0899752+01:00
 - description: At last, the long wait is finally over! After 4 years of waiting, the fully managed implementation of the C# compiler codenamed Roslyn has been finally released.
 - layout: article
 - image: http://tomasp.net/blog/2014/csharp-6-released/csharp.jpg
 - tags: c#,fun,functional programming
 - title: BREAKING: Open-source C# 6.0 released
 - url: 2014/csharp-6-released

--------------------------------------------------------------------------------
 - standalone

<img src="http://tomasp.net/blog/2014/csharp-6-released/csharp.jpg" style="width:170px;float:right;margin:15px" />

At last, the long wait is finally over! After 4 years of waiting, the fully managed implementation
of the C# compiler [codenamed "Roslyn"][roslyn] has been finally released. In the recent months, 
"Roslyn" has been slowly turning into [vaporware][vaporware], but thanks to the recent breakthrough,
the team made an enormous progress over the last two months and even implemented a number of new
C# 6.0 features.

The C# 6.0 compiler, together with [the full source code][source] has been released today!

> **UPDATE:** In case you are reading this article later than on the day when it was
> published, let me just point out that this was released on 1 April 2014. Just to 
> avoid any disappointments. Have fun ;-).

--------------------------------------------------------------------------------


<img src="http://tomasp.net/blog/2014/csharp-6-released/csharp.jpg" style="width:170px;float:right;margin:15px" />

At last, the long wait is finally over! After 4 years of waiting, the fully managed implementation
of the C# compiler [codenamed "Roslyn"][roslyn] has been finally released. In the recent months, 
"Roslyn" has been slowly turning into [vaporware][vaporware], but thanks to the recent breakthrough,
the team made an enormous progress over the last two months and even implemented a number of new
C# 6.0 features.

The C# 6.0 compiler, together with [the full source code][source] has been released today!

> **UPDATE:** In case you are reading this article later than on the day when it was
> published, let me just point out that this was released on 1 April 2014. Just to 
> avoid any disappointments. Have fun ;-).

The new "Roslyn" implementation
-------------------------------

Roslyn team started by re-implementing the compiler in C#. A team member
who prefers to remain anonymous comments: <em>"Writing a language X in X is kind of cool, right?
We just had to try! We also have a secret project to implement Excel fully as an Excel spreadsheet."</em> 
This turned out not to be a good idea. While C# is a fine language for
writing line-of-business applications, it lacks many features that are practically a must-have when
writing a compiler such as _algebraic data types_ and _pattern matching_.

As is often the case, the breakthrough was due to a lucky accident: <em>"One of our colleagues came
back to work after a long party and accidentally clicked a wrong button! His vision was a bit fuzzy
and so he just mistook F for C."</em> It turns out that the button was labeled "New F# Project" 
and it saved the project. [Mads Torgersen](http://blogs.msdn.com/b/madst/) from the C# team comments:

> You know, F# has all you need to implement Roslyn - algebraic data types, pattern matching and
> even active patterns! Once we discovered that it is already in Visual Studio, we started from 
> scratch and finished the new managed C# compiler in just two months! 

To those familiar with F#, this is not a surprise. An F# community member, Neil Danson recently
[implemented a C# sub-set in F#](http://neildanson.wordpress.com/2014/02/11/building-a-c-compiler-in-f/).
Indeed, the C# team decided to stand on the shoulders of giants: <em>"It was easy. We just took
Neil's code, added a couple of missing features and it was ready to ship!"</em>
As the "new Microsoft" is a big proponent of open-source, the C# team follows and released the complete
source code:

 * [C# 6.0 compiler source code](http://fsharppowerpack.codeplex.com/SourceControl/latest#compiler/3.1/Nov2013/src/) - 
   available on CodePlex, cleverly hidden under the "F# PowerPack with F# Compiler Source Drops" project.

Why is the release done so secretly? The "new Microsoft" not only cares about open-source, but
is also sensitive to social issues. After all, the creator of C# 6.0, Anders Hejlsberg is Danish. 
He explains:

> We added a few new language features and it made the language quite powerful. Maybe a bit too much.
> If everyone starts using this, it can badly affect the unemployment rate, because solving problems
> will be just too easy! So we wanted to hide it a little bit... 

What do the new features look like? The C# team started a [nice web site where you can try it 
yourself](http://www.tryfsharp.org/Learn), but let's have a quick look.

Introducing new features in C# 6
--------------------------------
   
At the [NDC 2013 talk in London](https://channel9.msdn.com/Forums/Coffeehouse/Mads-Torgersen--NDC-London--The-Future-of-C), 
Mads Torgersen discussed some ideas for C# 6.0. You can find a [nice write-up here](http://damieng.com/blog/2013/12/09/probable-c-6-0-features-illustrated).
The final version of C# 6.0 goes even further. In the talk, Mads discussed the example of implementing
a simple `Point` class and ended up with the following:

    [lang=csharp]
    public class Point(double x, double y) {
      public double X => x;
      public double Y => y;
      public double Distance => Math.Sqrt(x*x + y*y);
    }

Compared to C# 5, there are a few nice things. First, you can write so called _main constructor_
that makes parameters `x` and `y` automatically visible to the body. Creating properties and 
methods is also simplified using the `=>` syntax.  Yet, the code was still not completely 
satisfactory. Anders Hejlsberg provides an explanation:

> In C# 3.0, we introduced the `var` keyword and type-inference. So, why do we have
> to write `double` five times? Also, there is a lot of noise when you have to mark 
> everything as `public` explicitly. We just want to make developer's life easier!

In the final version of C# 6.0,  you can write the class as follows:

    type Point(x:float, y:float) =
      member this.X = x
      member this.Y = y
      member this.Distance = sqrt(x*x + y*y)

The arguments `x` and `y` are still annotated with types, but the rest of the types are
inferred automatically. The compiler is smart enough to know that multiplication and addition
of floating point numbers is also a floating point.

You can also see that the `class` keyword has been replaced with `type`. Mads Torgersen
explains this design decision:

> Well, classes are not the only useful type. There are interfaces, delegates and so on.
> So, we just made the language more uniform and added a few more useful types!

The addition of "a few more useful types" means that we can make the `Point` even simpler
using the _record_ type:

    type Point = 
      { X:float; Y:float }
      member this.Distance = sqrt(x*x + y*y)

Another new type that has been added is called _discriminated union_. This basically lets you
implement an entire class hierarchy in just 3 lines of code. For example, say you want to model
2D shapes including `Rectangle` and `Circle`. Instead of writing abstract class with two 
inherited classes (in 3 separate files), you can just write:

    type Shape = 
      | Rectangle of x1:float*y1:float * x2:float*y2:float
      | Circle of x:float*y:float * diameter:float

The documentation is still somewhat sparse, but the new C# 6.0 features are heavily inspired
by Scala and you can find great documentation on the [Scala for Fun and Profit](http://scalaforfunandprofit.com/posts/designing-with-types-intro/)
web site.

Conclusions
-----------

This release presents an important milestone in the development of C#. Not only is the compiler
now implemented in a [fully managed and extensible way](http://fsharp.github.io/FSharp.Compiler.Service/), 
but it has also been open-sourced. The community has already used it to develop useful [addins for the
Visual Studio IDE](http://fsprojects.github.io/VisualFSharpPowerTools/) as well as [other tools](http://tpetricek.github.io/FSharp.Formatting/).

The new language features in C# 6 make the language significantly more powerful and expressive.
The only worry expressed by some is: <em>"Doesn't C# 6.0 look a little like F#?"</em> Anders Hejlsberg
dismisses this worry:

> The C# design obviously takes inspiration from other languages. So, yes, you can definitely
> see a lot of inspiration from F# in the C# 6.0 release. We are happy to adopt good ideas 
> developed elsewhere and F# simply has a lot of good ideas!

 [roslyn]: http://en.wikipedia.org/wiki/Microsoft_Roslyn
 [vaporware]: http://en.wikipedia.org/wiki/Vaporware
 [source]: http://fsharppowerpack.codeplex.com/SourceControl/latest#compiler/3.1/Nov2013/src/
