Critical Architecture/Software Theory
=====================================

 - title: Critical Architecture/Software Theory
 - date: 2025-04-28T12:00:00.0000000+02:00
 - description: Post-modern architects use architecture to make critical, ironic and revealing comments on architectural history, practice and its social context. Is it possible to embed similar criticism into the language of software and make the software practice more self-aware and critical?
 - format: longread
 - image-large: http://tomasp.net/longreads/architecture/fig/web-dancing-gs.jpg

----------------------------------------------------------------------------------------------------
 - head

<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=EB+Garamond:ital,wght@0,400..800;1,400..800&family=Manrope:wght@200..800&family=Roboto+Mono:wght@100..700&display=swap" rel="stylesheet">
<meta name="keywords" content="post-modern architecture, software systems, programming languages, critical reading" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.11.1/languages/ocaml.min.js" integrity="sha512-x+P0DBw3wkY5qDtODK0RT0dDw7hcRV1BgYJAdM5RlTvPsEcIoLFCd4bF2zETdwDkZlkXtR0XdvNyk0TfSlZgGw==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.11.1/languages/reasonml.min.js" integrity="sha512-lyK79qXPy7ZrtRkTJGXu8WyCfTFhLgRgVRlgb9+TmWW+M7kYvs0gYAKUY4BkwvBPBIhqc1jNJ+iJoe4heIWZYA==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.11.1/languages/haskell.min.js" integrity="sha512-wV1s4ylNcflirsC0Ug9dDahOxjj/JSQheHv0loB9Q3bv7G0TYLduOWWmhz2MjMKRO6+LS8AgeuhBB8Gny4pdFQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<style type="text/css">
  #fig_ai img { width:900px; }
</style>

----------------------------------------------------------------------------------------------------
 - frontmatter

<h1>Critical <em>Architecture/Software</em> Theory</h1>
<div class="sidenote">
  <img src="fig/web-dancing.jpg">
  <p>
    Illustration: Dancing House in Prague (<a href="https://en.wikipedia.org/wiki/Dancing_House">Source: Wikipedia</a>)</p>
  <p>
    Post-modern architects use architecture to make critical, ironic and revealing comments on architectural
    history, practice and its social context. Is it possible to embed similar criticism into the language
    of software and make the software practice more self-aware and critical?</p>  
  <p>
    This essay is a very early exploration of this question. It will evolve over time as I keep thinking
    about the topic and I would love to hear your thoughts!</p>
  <p>
    <i class="fa-solid fa-envelope"></i><a href="mailto:tomas@tomasp.net">tomas@tomasp.net</a><br>
    <i class="fa-brands fa-bluesky"></i><a href="https://bsky.app/profile/tomasp.net">@tomasp.net</a><br>
    <i class="fa-solid fa-globe"></i><a href="https://tomasp.net">https://tomasp.net</a><br>
  </p>
</div>


<h3>Tomas Petricek</h3>
<p class="lead">28 April 2025</p>

<h3>Preface</h3>
<p class="lead">
  I have been interested in architecture and how it relates to software for some time now.
  I have written about this topic <a href="https://tomasp.net/blog/tag/architecture/">on my blog</a> and also in <a href="https://tomasp.net/academic/papers/metaphors/metaphors.pdf">Onward! 2021 paper</a>.
  My <a href="https://tomasp.net/blog/2020/cities-and-programming/">earlier writings</a> have been inspired by Christopher Alexander, Jane Jacobs, Kevin Lynch and Stewart Brand.
  Two years ago, I got my hands on <a href="https://www.goodreads.com/book/show/144899417-oppositions-reader">Oppositions Reader</a>, which is a collection of
  mostly theoretical and critical writings on architecture from a range of different post-modern perspectives.
</p>
<p class="lead">
  Reading critical architectural writings inspired me to ask different kinds of questions about software. 
  The works cited in this text are focused more on the meaning of architecture than (directly) on the problems of achieving fit or producing living structures.
  However, it is not just writings. Post-modern architects often used architecture itself to question architectural practice and its social context.
  I believe it is worthwhile to explore software from the same critical perspective. 
  I also believe that finding ways of using software as the medium for making statements can make the much needed critical investigations of software more commonplace.
</p>

<h3>Disclaimer</h3>
<p class="lead">
  I think I found some interesting questions to explore, but I certainly do not have the answers to many of them yet. 
  While this text may be nicely formatted, it is a collection of early notes at best.
  I tried to summarize my understanding of some of the architectural ideas and find interesting
  counterparts in the world of software and programming systems, but those still need to be fully worked out. 
  I hope this text will gets you to think about interesting things. I would love to hear about those,
  as well as any other comments and feedback that you may have!
</p>
<ul class="lead">
  <li>This page looks better on a bigger screen. It is also <a href="pdf/architecture.pdf">available as a PDF</a>.</li>
  <li>Tell me what you think! I'm <a href="https://bsky.app/profile/tomasp.net">@tomasp.net</a> on BlueSky.</li>
  <li>If you have more to say, please write to <a href="mailto:tomas@tomasp.net">tomas@tomasp.net</a>.</li>
  <li>Typos? LaTeX source of this <a href="https://github.com/tpetricek/critical-software-theory">can be found on GitHub</a>.</li>
</ul>

<h3>Table of Contents</h3>