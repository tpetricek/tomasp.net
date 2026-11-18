# Write your own tiny programming system(s)!

 - title: Write your own tiny programming system(s)!
 - layout: article
 - tags: course
 - date: 7 December 2025
 - image: https://tomasp.net/academic/courses/tinysystems/1-intro-1-whytiny.png
 - description: Learn how programming systems like F#, BASIC, Prolog, Self and Excel work by 
    writing a small version of the system on your own! Watch the videos and complete small 
    hands-on tasks using the F# language to implement the core logic yourself.

----

Wondering how to write your own programming language or an interactive programming system?
In this course, you learn  how fundamental programming language techniques, algorithms
and systems work by writing their miniature versions. 
The course is made out of six modules, each looking at one langauge or system idea.
It covers functional (TinyML and TinyHM),
imperative (TinyBASIC), object-oriented (TinySelf) and
end-user programming (TinyExcel).

This is a hands-on course. Each lecture starts with a few videos 
that provide background and explain key concepts.
After watching the videos, the source code for each lecture gives you a basic
structure of the language or system and tasks that you have to go through to 
implement the working language or system. There are also a few bonus problems
for those interested in doing more!

The course uses the [F# langauge](https://fsharp.org), partly because I like
F# and partly because F# is a great fit for the material. The course
does not assume that you know F# already. Most of the tasks use simple
algebraic data types (unions) and recursive functions and a few extra tricks
are covered in the videos.

I ran this course in-person at Charles University in [2023/24](https://d3s.mff.cuni.cz/teaching/nprg077/2023-24/)
and [2025/26](https://d3s.mff.cuni.cz/teaching/nprg077/2025-26/) and we're now using some
of the ideas in the [Principles of Programming Languages course](https://d3s.mff.cuni.cz/teaching/nprg084/).

- [<i class="fa-brands fa-youtube"></i> **Watch all the videos on YouTube**](https://www.youtube.com/watch?v=rCbT1J25sEM&list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ)
- [<i class="fa-brands fa-github"></i> **Get all the source code from GitHub**](https://github.com/tpetricek/tiny-systems/)

---

</article>
<style type="text/css">
article.course { margin-bottom:0px; }
article.course ul { margin:20px 0px 0px 0px; }
article.course ul li { list-style-type: none; margin-left:10px; margin-bottom:0px; }
article.course ul li i { margin-right:5px; }
h2 { font-weight:200; }
.lecture-info { max-width:750px; margin:0px auto; padding:0px 10px 20px 20px; }
.lecture-info h2 { margin:0px 0px 5px 0px; }
.lecture-info p { margin-bottom:20px; }
.lecture-info p.summary { margin:10px 0px 10px 0px; }
.videos { background:#f0f0f0; padding:30px max(20px, calc((100% - 1240px) / 2)) 40px; margin-bottom:40px; }
.videos { display:grid; column-gap:30px; row-gap:25px; grid-template-columns:1fr; }
.video img { width:100%; max-width:280px; display:block; border:1px solid #e0e0e0; }
.video h3 { margin:0px 0px 4px 0px; font-size:14pt; }
.video p { font-size:11pt; color:#555; margin-bottom:8px; }
@media only screen and (min-width : 600px) {
  .videos { grid-template-columns:1fr 1fr; }
}
@media only screen and (min-width : 1020px) {
  .videos { display:flex; flex-wrap:wrap; justify-content:center; }
  .video { flex:0 0 calc((100% - 90px) / 4); }
}
</style>
<div class="lectures">
<section class="lecture">
  <div class="lecture-info">
    <h2><strong>Welcome:</strong> Write your own tiny programming system(s)!</h2>
    <p class="summary">
      A tiny programming system can be both educational tool and a research artifact to explore
      new ideas. I start by showing a few of my own research tiny systems and explaining the
      approach. I then explain why I talk about <a href="https://tomasp.net/techdims">programming
      systems</a> and not languages, before introducing F# through a couple of web-based demos.
    </p>
    <p>Slides: <a href="https://tpetricek.github.io/teaching/2025/tiny-systems/intro.html">Web-based</a> or <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/1-intro.pdf">PDF format</a><br />Code: <a href="https://github.com/tpetricek/tiny-systems/tree/master/00-introduction">Demos from the lecture</a></p>
  </div>
  <div class="videos">
    <div class="video">
      <h3>1.1&nbsp; <a href="https://www.youtube.com/watch?v=rCbT1J25sEM&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Why such a strange topic?</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/1-intro-1-whytiny.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/1-intro-1-whytiny.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=rCbT1J25sEM&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="1-intro-1-whytiny.png" /></a>
    </div>
    <div class="video">
      <h3>1.2&nbsp; <a href="https://www.youtube.com/watch?v=rGboZ88Ipis&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Tiny systems methodology</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/1-intro-2-methods.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/1-intro-2-methods.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=rGboZ88Ipis&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="1-intro-2-methods.png" /></a>
    </div>
    <div class="video">
      <h3>1.3&nbsp; <a href="https://www.youtube.com/watch?v=847avvqx7Ek&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Programming systems</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/1-intro-3-systems.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/1-intro-3-systems.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=847avvqx7Ek&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="1-intro-3-systems.png" /></a>
    </div>
    <div class="video">
      <h3>1.4&nbsp; <a href="https://www.youtube.com/watch?v=OqW-RvL8C-c&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">A taste of the F# language</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/1-intro-4-fsharp.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/1-intro-4-fsharp.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=OqW-RvL8C-c&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="1-intro-4-fsharp.png" /></a>
    </div>
  </div>
</section>
<section class="lecture">
  <div class="lecture-info">
    <h2><strong>TinyML</strong>: A tiny functional programming language interpreter</h2>
    <p class="summary">
    ML is a small functional-first programming language with origins in the 1970s that inspired F#.
    In the first lab, we write an interpreter for a small ML-like language. Implementing the
    interpreter is a good way to learn about basic aspects of the language such as variable
    scoping, lazy and eager evaluation and data types such as records and unions.</p>
    <p>Slides: <a href="https://tpetricek.github.io/teaching/2025/tiny-systems/tinyml.html">Web-based</a> or <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/2-tinyml.pdf">PDF format</a><br />Code: <a href="https://github.com/tpetricek/tiny-systems/blob/master/01-tiny-ml/demos/demo.fsx">Demos from the lecture</a>, <a href="https://github.com/tpetricek/tiny-systems/tree/master/01-tiny-ml/tasks">Tasks for the lab</a></p>
  </div>
  <div class="videos">
    <div class="video">
      <h3>2.1&nbsp; <a href="https://www.youtube.com/watch?v=wb1y02yjJ0M&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">What you need to know about F#</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/2-tinyml-1-fsharp.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/2-tinyml-1-fsharp.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=wb1y02yjJ0M&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="2-tinyml-1-fsharp.png" /></a>
    </div>
    <div class="video">
      <h3>2.2&nbsp; <a href="https://www.youtube.com/watch?v=eyVY-_oWabI&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Language features and theory</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/2-tinyml-2-language.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/2-tinyml-2-language.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=eyVY-_oWabI&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="2-tinyml-2-language.png" /></a>
    </div>
    <div class="video">
      <h3>2.3&nbsp; <a href="https://www.youtube.com/watch?v=ZZbLp5fq0Ec&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Interpreter and lab guide</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/2-tinyml-3-stepbystep.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/2-tinyml-3-stepbystep.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=ZZbLp5fq0Ec&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="2-tinyml-3-stepbystep.png" /></a>
    </div>
  </div>
</section>
<section class="lecture">
  <div class="lecture-info">
    <h2><strong>TinyBASIC</strong>: A tiny interactive imperative programming system</h2>
    <p class="summary">Looking at BASIC is perhaps unexpected, but it shows two new ideas. First,
      you'll learn how to manage mutable state and how to implement control flow constructs such as
      <code>goto</code>. Second, we will also look at BASIC as an interactive programming system
      where programmers can edit input using line numbers that were used by BASIC on microcomputers
      such as <a href="https://tomasp.net/commodore64/">Commodore 64</a>.</p>
    <p>Slides: <a href="https://tpetricek.github.io/teaching/2025/tiny-systems/tinybasic.html">Web-based</a> or <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/3-tinybasic.pdf">PDF format</a><br />Code: <a href="https://github.com/tpetricek/tiny-systems/tree/master/02-tiny-basic/demos">Demos from the lecture</a>, <a href="https://github.com/tpetricek/tiny-systems/tree/master/02-tiny-basic/tasks">Tasks for the lab</a></p>
  </div>
  <div class="videos">
    <div class="video">
      <h3>3.1&nbsp; <a href="https://www.youtube.com/watch?v=jR0m-7WLuMo&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Commodore 64 BASIC</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/3-tinybasic-1-commodore.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/3-tinybasic-1-commodore.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=jR0m-7WLuMo&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="3-tinybasic-1-commodore.png" /></a>
    </div>
    <div class="video">
      <h3>3.2&nbsp; <a href="https://www.youtube.com/watch?v=4nVW8v6GWrg&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">What you need to know about F#</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/3-tinybasic-2-fsharp.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/3-tinybasic-2-fsharp.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=4nVW8v6GWrg&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="3-tinybasic-2-fsharp.png" /></a>
    </div>
    <div class="video">
      <h3>3.3&nbsp; <a href="https://www.youtube.com/watch?v=fSGOw_nfxZ8&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Interpreter and lab guide</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/3-tinybasic-3-stepbystep.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/3-tinybasic-3-stepbystep.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=fSGOw_nfxZ8&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="3-tinybasic-3-stepbystep.png" /></a>
    </div>
  </div>
</section>
<section class="lecture">
  <div class="lecture-info">
    <h2><strong>TinyHM</strong>: Writing the Hindley-Milner type inference algorithm</h2>
    <p class="summary">Languages like ML and F# are statically typed, but you do not have to
      write types explicitly. They are inferred using a type-inference algorithm. In this lab,
      we revisit our previous TinyML implementation and implement the classic Hindley-Milner
      algorithm to automatically infer types for variables and functions in the code.
      We'll also see how this relates to error conditions in our interpreter!</p>
    <p>Slides: <a href="https://tpetricek.github.io/teaching/2025/tiny-systems/tinyhm.html">Web-based</a> or <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/4-tinyhm.pdf">PDF format</a><br />Code: <a href="https://github.com/tpetricek/tiny-systems/tree/master/03-tiny-hm/demos">Demos from the lecture</a>, <a href="https://github.com/tpetricek/tiny-systems/tree/master/03-tiny-hm/tasks">Tasks for the lab</a></p>
  </div>
  <div class="videos">
    <div class="video">
      <h3>4.1&nbsp; <a href="https://www.youtube.com/watch?v=FUFQ4jcfslU&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">How type inference in ML works</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/4-tinyhm-1-inference.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/4-tinyhm-1-inference.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=FUFQ4jcfslU&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="4-tinyhm-1-inference.png" /></a>
    </div>
    <div class="video">
      <h3>4.2&nbsp; <a href="https://www.youtube.com/watch?v=G6F6Ie3uMPU&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Type systems and constraints</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/4-tinyhm-2-constraints.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/4-tinyhm-2-constraints.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=G6F6Ie3uMPU&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="4-tinyhm-2-constraints.png" /></a>
    </div>
    <div class="video">
      <h3>4.3&nbsp; <a href="https://www.youtube.com/watch?v=hX_0A4hINxM&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Peano and step by step guide</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/4-tinyhm-3-peano.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/4-tinyhm-3-peano.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=hX_0A4hINxM&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="4-tinyhm-3-peano.png" /></a>
    </div>
  </div>
</section>
<section class="lecture">
  <div class="lecture-info">
    <h2><strong>TinyProlog</strong>: Writing tiny logic programming language</h2>
    <p class="summary">
    In logic programming languages like Prolog, programs are written as a database of facts or
    derivation rules and evaluation of a program is inference using those rules. In this lab, 
    you'll understand what exactly this means by writing a small Prolog interpreter. You'll also
    see that the core logic of the algorithm is remarkably similar to our earlier type inference 
    logic!</p>
    <p>Slides: <a href="https://tpetricek.github.io/teaching/2025/tiny-systems/tinyprolog.html">Web-based</a> or <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/5-tinyprolog.pdf">PDF format</a><br />Code: <a href="https://github.com/tpetricek/tiny-systems/tree/master/04-tiny-prolog/demos">Demos from the lecture</a>, <a href="https://github.com/tpetricek/tiny-systems/tree/master/04-tiny-prolog/tasks">Tasks for the lab</a></p>
  </div>
  <div class="videos">
    <div class="video">
      <h3>5.1&nbsp; <a href="https://www.youtube.com/watch?v=KC7A5QPi5Uo&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">How logic programming works</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/5-tinyprolog-1-logic.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/5-tinyprolog-1-logic.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=KC7A5QPi5Uo&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="5-tinyprolog-1-logic.png" /></a>
    </div>
    <div class="video">
      <h3>5.2&nbsp; <a href="https://www.youtube.com/watch?v=JQJNkRpvFGs&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Structure, unification, resolution</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/5-tinyprolog-2-theory.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/5-tinyprolog-2-theory.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=JQJNkRpvFGs&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="5-tinyprolog-2-theory.png" /></a>
    </div>
    <div class="video">
      <h3>5.3&nbsp; <a href="https://www.youtube.com/watch?v=u05RcKFDmC4&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">F# tricks and step by step guide</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/5-tinyprolog-3-steps.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/5-tinyprolog-3-steps.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=u05RcKFDmC4&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="5-tinyprolog-3-steps.png" /></a>
    </div>
  </div>
</section>
<section class="lecture">
  <div class="lecture-info">
    <h2><strong>TinySelf</strong>: Writing an object-oriented programming system</h2>
    <p class="summary">
    Self is a minimalistic object-oriented programming system that uses prototype-based model.
    Thanks to the unified design where everything is an object, it is a fun programming system
    to implement on your own! In the lab, you'll learn about prototypes, method lookup and 
    activation records &mdash; ideas that are behind many object-oriented systems.</p>
    <p>Slides: <a href="https://tpetricek.github.io/teaching/2025/tiny-systems/tinyself.html">Web-based</a> or <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/6-tinyself.pdf">PDF format</a><br />Code: <a href="https://github.com/tpetricek/tiny-systems/tree/master/05-tiny-self/demos">Demos from the lecture</a>, <a href="https://github.com/tpetricek/tiny-systems/tree/master/05-tiny-self/tasks">Tasks for the lab</a></p>
  </div>
  <div class="videos">
    <div class="video">
      <h3>6.1&nbsp; <a href="https://www.youtube.com/watch?v=7Pre_H7SKSU&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">History and foundations</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/6-tinyself-1-foundations.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/6-tinyself-1-foundations.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=7Pre_H7SKSU&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="6-tinyself-1-foundations.png" /></a>
    </div>
    <div class="video">
      <h3>6.2&nbsp; <a href="https://www.youtube.com/watch?v=6Lge9IsAGRo&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Using the Self system</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/6-tinyself-2-self.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/6-tinyself-2-self.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=6Lge9IsAGRo&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="6-tinyself-2-self.png" /></a>
    </div>
    <div class="video">
      <h3>6.3&nbsp; <a href="https://www.youtube.com/watch?v=HSCb_oJu7wQ&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Working with mutable data in F#</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/6-tinyself-3-mutable.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/6-tinyself-3-mutable.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=HSCb_oJu7wQ&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="6-tinyself-3-mutable.png" /></a>
    </div>
    <div class="video">
      <h3>6.4&nbsp; <a href="https://www.youtube.com/watch?v=HRLp8IaWUwo&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Structure &amp; step by step guide</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/6-tinyself-4-stepbystep.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/6-tinyself-4-stepbystep.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=HRLp8IaWUwo&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="6-tinyself-4-stepbystep.png" /></a>
    </div>
  </div>
</section>
<section class="lecture">
  <div class="lecture-info">
    <h2><strong>TinyExcel</strong>: Implementing reactive graph-based computations</h2>
    <p class="summary">
    Some say that Excel is the most widely used programming language. Whether you believe this
    or not, it is a great way to explore writing reactive programming system where outputs recompute
    when inputs change. You'll also learn about some powerful end-user programming tricks such 
    as the "drag down" operation that extends a formula to apply over a region of cells.</p>
    <p>Slides: <a href="https://tpetricek.github.io/teaching/2025/tiny-systems/tinyexcel.html">Web-based</a> or <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/7-tinyexcel.pdf">PDF format</a><br />Code: <a href="https://github.com/tpetricek/tiny-systems/tree/master/06-tiny-excel/demos">Demos from the lecture</a>, <a href="https://github.com/tpetricek/tiny-systems/tree/master/06-tiny-excel/tasks">Tasks for the lab</a></p>
  </div>
  <div class="videos">
    <div class="video">
      <h3>7.1&nbsp; <a href="https://www.youtube.com/watch?v=ghRAxGAJgbw&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Technical dimensions</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/7-tinyexcel-1-dimensions.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/7-tinyexcel-1-dimensions.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=ghRAxGAJgbw&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="7-tinyexcel-1-dimensions.png" /></a>
    </div>
    <div class="video">
      <h3>7.2&nbsp; <a href="https://www.youtube.com/watch?v=WV5Sft-uf-k&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Architecture and F# events</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/7-tinyexcel-2-architecture.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/7-tinyexcel-2-architecture.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=WV5Sft-uf-k&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="7-tinyexcel-2-architecture.png" /></a>
    </div>
    <div class="video">
      <h3>7.3&nbsp; <a href="https://www.youtube.com/watch?v=3p9RiP-GNCE&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ">Structure &amp; step-by-step guide</a></h3>
      <p>Download: <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/7-tinyexcel-3-stepbystep.pdf">slides</a>, <a href="https://d3s.mff.cuni.cz/files/teaching/nprg077/2025/7-tinyexcel-3-stepbystep.mp4">video</a></p>
      <a href="https://www.youtube.com/watch?v=3p9RiP-GNCE&amp;list=PLRBVoLoCYzDML8e9g58f6zb-faG1rMHIJ"><img src="7-tinyexcel-3-stepbystep.png" /></a>
    </div>
  </div>
</section>
</div>
<article>