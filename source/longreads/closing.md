The road to opaque software is paved with good (engineering) intentions
=======================================================================

 - title: The road to opaque software is paved with good (engineering) intentions
 - date: 2026-10-29T22:48:05.7699252+01:00
 - description: Most software we use today is black-box and cannot be easily modified by its
    users. This article follows the history of information hiding from OS/360 to Free Software
    and the history of the Web and documents how software becomes opaque through well-intended
    engineering decisions.
 - format: longread
 - keywords: software theory, opacity, alienation, software history
 - image: http://tomasp.net/longreads/closing/fig/close-me-big.png
 - banner: http://tomasp.net/longreads/closing/fig/close-me.gif

----------------------------------------------------------------------------------------------------
 - head

<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Zilla+Slab:wght@400;500;600&family=Manrope:wght@200..800&family=Roboto+Mono:wght@100..700&display=swap" rel="stylesheet">
<link href="https://tomasp.net/custom/longreads.css" rel="stylesheet">
<link href="style.css" rel="stylesheet">

----------------------------------------------------------------------------------------------------
 - frontmatter

<div class="title-box">
<h1>The road to opaque software is paved with good (engineering) intentions</h1>
<h4>Tomas Petricek</h4>
<p class="lead">29 October 2026</p>
</div>
</section>

<section>
<div class="sidenote sideintro">
  <p>
    Most software we use today is black-box and cannot be easily modified by its
    users. This article follows the history of information hiding from OS/360 to Free Software
    and the history of the Web and documents how software becomes opaque through well-intended
    engineering decisions. </p>  
  <p>
    This essay is work-in-progress, currently under submission to an academic
    journal on history and philosophy of computing. If you have any suggestions, comments
    or corrections, please get in touch!</p>
  <p>
    <i class="fa-solid fa-envelope"></i><a href="mailto:tomas@tomasp.net">tomas@tomasp.net</a><br>
    <i class="fa-brands fa-bluesky"></i><a href="https://bsky.app/profile/tomasp.net">@tomasp.net</a><br>
    <i class="fa-solid fa-globe"></i><a href="https://tomasp.net">https://tomasp.net</a><br>
  </p>
</div>

<p class="lead">
  When I was writing my first JavaScript scripts at the end of the 1990s, 
  you could learn by finding a web page that did what you wanted, clicking 
  "View source" and copying the bits of JavaScript that you needed. This was 
  surely due to the primitive nature of the web (and what I was trying to do), but
  the kind of openness that lets you explore, learn, and modify software has largely
  been lost (in most professional programming contexts) today.
</p>

<p class="lead">
  You might argue I'm just nostalgic. After all, open-source has became the dominant 
  development methodology and so users can, in fact, understand and modify a lot of
  the software they use. But I do not think open-source is the answer. The software 
  freedoms may exist in theory, but in practice, even professional programmers have
  a hard time modifying complex software systems that we rely on today. 
</p>

<p class="lead">
  This article is an attempt to understand how software becomes opaque and why.
  The conclusion, supported by two historical case studies, is that opacity comes
  about due to good engineering and managerial intentions. Knowing this may help us
  imagine a better, more open nature of software for the future.
</p>

<ul class="lead icons">
  <li><i class="fa fa-signature"></i> An <a href="https://tomasp.net/blog/2021/popup-from-hell/">earlier blog post</a> behind some of the ideas.</li>
  <li><i class="fa fa-file-pdf"></i> The essay is also <a href="pdf/closing.pdf">available as a PDF</a>.</li>
  <li><i class="fa fa-envelope"></i> Share your thoughts with <a href="https://bsky.app/profile/tomasp.net">@tomasp.net</a> or <a href="mailto:tomas@tomasp.net">tomas@tomasp.net</a>.</li>
</ul>

<h4>Table of Contents</h4>