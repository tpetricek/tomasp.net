Miscomputation: Learning to live with errors
============================================

 - date: 2015-07-27T14:15:31.7814254+01:00
 - description: Charles Babbage once said that 'if trials of three or four simple cases have been made, it is scarcely possible that there can be any error'. We now know that errors are more common and harder to eliminate. In this blog post, I look at different strategies that programmers use for dealing with errors. It turns out that there is a surprisingly wide range of options!
 - layout: post
 - image: http://tomasp.net/blog/2015/failures/babbage.png
 - tags: philosophy,research,programming languages
 - title: Miscomputation: Learning to live with errors
 - url: 2015/failures

--------------------------------------------------------------------------------
 - standalone

> <img src="http://tomasp.net/blog/2015/failures/babbage.png" style="float:right;width:110px;margin:5px 0px 10px 20px" />
>
> <p style="margin-bottom:5px">If trials of three or four simple cases have been made, and are found
> to agree with the results given by the engine, it is scarcely possible that there can be any error
> (...).</p>
> <p style="text-align:right">Charles Babbage, On the mathematical<br /> powers of the calculating engine (1837)</p>

Anybody who has something to do with modern computers will agree that the above statement made by
Charles Babbage about the analytical engine is understatement, to say the least.

Computer programs do not always work as expected. There is a complex taxonomy of errors or
[_miscomputations_](http://link.springer.com/article/10.1007/s13347-013-0112-0). The taxonomy of
possible errors is itself interesting. Syntax errors like missing semicolons are quite obvious
and are easy to catch. Logical errors are harder to find, but at least we know that something
went wrong. For example, our algorithm does not correctly sort some lists. There are also issues that
may or may not be actual errors. For example an algorithm in online store might suggest slightly
suspicious products. Finally, we also have concurrency errors that happen very rarely in some
very specific scenario.

If Babbage was right, we would just try three or four simple cases and eradicate all errors from
our programs, but eliminating errors is not so easy. In retrospect, it is quite interesting to
see how long it took early computer engineers to realise that coding (i.e. translating
mathematical algorithm to program code) errors are a problem:

> <p style="margin-bottom:5px">Errors in coding were only gradually recognized to be a signiﬁcant
> problem: a typical early comment was that of Miller [circa 1949], who wrote that such errors,
> along with hardware faults, could be "expected, in time, to become infrequent".</p>
> <p style="text-align:right">Mark Priestley, Science of Operations (2011)</p>

We _mostly_ got rid of hardware faults, but coding errors are still here. Programmers spent
over 50 years finding different practical strategies for dealing with them. In this
blog post, I want to look at four of the strategies. Quite curiously, there is a very wide range.

--------------------------------------------------------------------------------


> <img src="http://tomasp.net/blog/2015/failures/babbage.png" style="float:right;width:110px;margin:5px 0px 10px 20px" />
>
> <p style="margin-bottom:5px">If trials of three or four simple cases have been made, and are found
> to agree with the results given by the engine, it is scarcely possible that there can be any error
> (...).</p>
> <p style="text-align:right">Charles Babbage, On the mathematical<br /> powers of the calculating engine (1837)</p>

Anybody who has something to do with modern computers will agree that the above statement made by
Charles Babbage about the analytical engine is understatement, to say the least.

Computer programs do not always work as expected. There is a complex taxonomy of errors or
[_miscomputations_](http://link.springer.com/article/10.1007/s13347-013-0112-0). The taxonomy of
possible errors is itself interesting. Syntax errors like missing semicolons are quite obvious
and are easy to catch. Logical errors are harder to find, but at least we know that something
went wrong. For example, our algorithm does not correctly sort some lists. There are also issues that
may or may not be actual errors. For example an algorithm in online store might suggest slightly
suspicious products. Finally, we also have concurrency errors that happen very rarely in some
very specific scenario.

If Babbage was right, we would just try three or four simple cases and eradicate all errors from
our programs, but eliminating errors is not so easy. In retrospect, it is quite interesting to
see how long it took early computer engineers to realise that coding (i.e. translating
mathematical algorithm to program code) errors are a problem:

> <p style="margin-bottom:5px">Errors in coding were only gradually recognized to be a signiﬁcant
> problem: a typical early comment was that of Miller [circa 1949], who wrote that such errors,
> along with hardware faults, could be "expected, in time, to become infrequent".</p>
> <p style="text-align:right">Mark Priestley, Science of Operations (2011)</p>

We _mostly_ got rid of hardware faults, but coding errors are still here. Programmers spent
over 50 years finding different practical strategies for dealing with them. In this
blog post, I want to look at four of the strategies. Quite curiously, there is a very wide range.

Introducing the heroes
----------------------

 - _"Errors are a curse and must be avoided at all costs,"_ says our first hero _"if it contains
   an error, you cannot even call it a program!"_ This sounds a bit idealistic, but our hero hopes
   that dependently typed languages will make this dream a reality.

 - _"But how do you know it works?"_ comes a reply from our second doubtful hero. _"You need to
   write a specification, or tests!"_ And our second hero becomes even more extreme _"In fact,
   I will only write new code to fix errors revealed by tests!"_

 - As our first two heroes start arguing, a third (a bit weird) hero comes in saying _"You both
   really believe you can eliminate all errors?"_ Our two heroes start looking puzzled and the
   newcomer adds _"When there is an error, just let it crash!"_ Our first two heroes burst into
   laughter, but start feeling uneasy as the third hero continues looking like she _knows something_.

 - As if the situation was not bad enough already, a new hero appears (looking a bit like a rock
   star): _"I like errors. Errors are fun!"_ Everyone else steps back a bit as our fourth hero
   continues _"How do you even tell what is an error? Just watch what's going on. Maybe it'll do
   something new and interesting!"_

Error as a curse: Avoiding errors at all costs
----------------------------------------------

<img src="http://tomasp.net/blog/2015/failures/hero1.png" style="float:right;margin:10px 0px 10px 10px;height:200px;" />

Many software developers share some of the thoughts with our first hero. Wouldn't it be nice
if we could _never_ make errors, avoid miscomputations altogether and all software we created
was fully correct? As we saw in an earlier quote, people only realized that coding errors is
a significant problem in 1950s. A bit later, in 1960s, the _Algol research programme_ first
advocated using formal logic to avoid errors:

> <p style="margin-bottom:5px">
> One of the goals of the Algol research programme was to utilize the resources of logic to
> increase the confidence (...) in the correctness of a program. As McCarthy had put it, "[instead]
> of debugging a program, one should prove that it meets its specifications (...)".</p>
> <p style="text-align:right">Mark Priestley, Science of Operations (2011)</p>

Modern statically-typed programming languages follow the same basic approach. In their case,
the "resources of logic" are concretely instantiated in type systems that rule out some of
the errors. This is captured by the slogan _"well-typed programs do not go wrong"_ that was
stated by Robin Milner in 1978 (as part of the _Semantic Soundness Theorem_).

Followers of the Algol research programme (and statically typed languages) aim to create correct
programs that never miscompute. When there is an error, the compiler will reject the program.
However, types used today can only specify some of the program properties that we need to check,
and so the Algol camp is finding ways for checking stronger properties.

One way towards checking more program properties is to use _dependently-typed programming languages_
such as Idris, Agda or Coq. With these languages, there is even more focus on using the
_resources of logic_ (as suggested by the Algol research programme) to make sure that our
programs meet specification. In fact, _proving_ that your program matches specification
becomes a part of programming!

Whether we can actually eliminate all errors in this way remains an open question. Our first
hero and his friends will, of course, tell you that the world where all software is provably
correct is close:

> <p style="margin-bottom:5px">
> [T]oday most people who write software, practitioners and academics alike, assume that the
> costs of formal program verification outweigh the benefits. The purpose of this book is to
> convince you that the technology of program verification is mature enough today that it
> makes sense to use it in a support role in many kinds of research projects in computer science.</p>
> <p style="text-align:right">Adam Chlipala, Certified Programming with Dependent Types (2013)</p>

The Algol research programme started in 1960s and there has certainly been a lot of progress
over the last 50 years towards using formal methods to ensure that programs are correct. The
ML-family of languages (including OCaml and F#) provides basic guarantees for very low
cost. That said, I think our first hero still has a few dragons to slay!

Error as a progress: Test-driven development
--------------------------------------------

<img src="http://tomasp.net/blog/2015/failures/hero2.png" style="float:right;margin:10px 0px 10px 10px;height:200px;" />

Looking through the perspective of the Algol research programme, it might be surprising that any
software actually works at all. Sir Tony Hoare, one of the proponents of the Algol research
programme asks exactly this question in his paper [How did software get so reliable without
proof?](http://dl.acm.org/citation.cfm?id=729681) The summary of the answer is that solid
engineering practices are often good enough to produce working software. This is where our
second hero comes to the scene.

One of the engineering practices for avoiding errors is to write tests. At the first sight,
this might look very similar to the first approach - we write tests to rule out errors!
You could see tests from this perspective. Regression tests and some tests for code in
dynamically-typed languages certainly play this role.

The use of tests in programming may have started from the aim to rule out errors, but it developed
in a very different direction. This is the idea summarized by _test-driven development_. In
TDD, tests are not just a method for automatically checking the absence of certain errors, but
they also become a part of specification and a _driving force_ behind development:

> <p style="margin-bottom:5px">
> [H]ere's what we do: we drive development with automated tests (...).
> In Test-Driven Development, we (1) write new code only if an automated test has failed
> (2) eliminate duplication. These are two simple rules, but they generate complex
> individual and group behavior (...).</p>
> <p style="text-align:right">Kent Beck, Test-Driven Development by Example (2003)</p>

This leads to the Red-Green-Refactor mantra of TDD:

> <p style="margin-bottom:5px">
> 1.	Red – write a little test that doesn't work, and perhaps doesn't even compile at first.<br />
> 2.	Green – Make the test work quickly, committing whatever sins necessary in the process<br />
> 3.	Refactor – Eliminate all of the duplication created in merely getting the test to work.</p>
> <p style="text-align:right">Kent Beck, Test-Driven Development by Example (2003)</p>

I don't intend to contribute to the recent heated discussion about TDD (and I'm by no means a
TDD expert). What is interesting here is that test-driven development treats errors, or
miscomputations, in a very special way. In particular, we should first produce an _isolated miscomputation_
and then write code to remove it. We write a failing test to exemplify an error present in
our implementation (unsupported behaviour) and then implement the functionality and
remove the error.

In this way, TDD _incorporates miscomputation_ as a part of the development cycle!
Saying that TDD is about avoiding miscomputations would not be accurate. In fact, it is
deliberately introducing them, and then eliminating them.

Although very different, both of the approaches discussed so far aim to eliminate miscomputation from
completed programs that are deployed or sold to customers. In the first case, this is done through proofs.
In the second case, this is done by adding tests (as a specification) and making them pass. However,
there are other ways for dealing with errors...

Error as the unavoidable: Let it crash
--------------------------------------

<img src="http://tomasp.net/blog/2015/failures/hero3.png" style="float:right;margin:10px 0px 10px 10px;height:200px;" />

A group that takes a very different approach to errors is the Erlang community. This was represented by
the third here from the introduction who even used the famous Erlang slogan "let it crash". In the Erlang
mindset, there are two separate (and quite different) situations that can cause problems. Here is a nice
summary from Joe Armstrong:

> <ul><li style="margin-left:10px;">
>   <strong>exceptions</strong> occur when the run-time system does not know what to do.
> </li><li style="margin-left:10px;">
>   <strong>errors</strong> occur when the programmer doesn’t know what to do.
> </li></ul>

I think both of these would qualify as _miscomputations_, but in a quite different way. If some code
throws an exception, we can handle it and we might be able to continue (say, return 0 when dividing by
zero). In case of error, we have _by definition_ no way of recovering. We do not have enough information
to proceed:

> <p style="margin-bottom:5px">
> Errors occur when the programmer does not know what to do. Programmers are supposed to follow
> specifications, but often the specification does not say what to do and therefore the programmer
> does not know what to do.</p>
> <p style="text-align:right">Joe Armstrong, Programming reliable systems (2003)</p>

The interesting thing is that in Erlang, _miscomputations_ of this kind are expected to happen and
programmers have a practical strategy for dealing with them:

> <p style="margin-bottom:5px">
> What kind of code must the programmer write when they find an error? The philosophy is let some
> other process fix the error, but what does this mean for their code? The answer is let it crash.</p>
> <p style="text-align:right">Joe Armstrong, Programming reliable systems (2003)</p>

Unlike in the first two cases (logic and tests) errors or miscomputations are actually expected to
happen during normal operation of Erlang programs. This works because Erlang programs typically use
a sophisticated supervision model. A supervisor process can restart the worker process that miscomputed
or it can try some alternate way of doing what needs to be done.

It is important to understand that in the Erlang philosophy, having crashing processes is a perfectly
normal thing and there is nothing wrong with it. So, miscomputation becomes not a thing to be avoided;
not a thing integrated into the development process, but something that we can deliberately introduce
into programs to deal with unexpected conditions.

Now, there is a deeper philosophical question about miscomputation which I'm going to skip in this
blog post - is it actually a _miscomputation_ when it is a normal part of the operation? If we want
to use the same definition of _miscomputation_ for all the cases covered in this blog post, then we
have to say that crashing a process _is_ a miscomputation.

You probably already guessed the structure of this article. We started by avoiding miscomputations,
then we made them a part of the development process and now we are making them a part of program
execution. The question is, can we go even further?

Error as an inspiration: Live coding
------------------------------------

<img src="http://tomasp.net/blog/2015/failures/hero4.png" style="float:right;margin:10px 0px 10px 10px;height:200px;" />

In all of the three previous cases, the miscomputations or errors were never visible to the end
user. Our third hero is going to change this. But let me start by going back to the early history
of programming and the Algol research programme. Although the Algol programme (with its focus on
formal proofs) become very influential, there were other languages. One of the first ones that
took a very different perspective was Smalltalk in 1970s:

> <p style="margin-bottom:5px">
> Smalltalk appears to represent an approach to the design of programming languages
> that is quite different from what was familiar in the Algol research programme.
> </p><p style="text-align:right">Mark Priestley, Science of Operations (2011)</p>

There is more fundamental difference than you would think. It is also hard to see, because we are
used to certain ways of thinking about programming languages that are actually very similar for
all languages in use today. In a way, Smalltalk is not a _programming language_, but a _programming
environment_. When you are working with it, you are not _writing code_, but you are _interacting_
with the Smalltalk environment:

> <p style="margin-bottom:5px">
> Programming [in Smalltalk] was not thought of as the task of constructing a linguistic entity,
> but rather as a process of working interactively with the semantic representation of the
> program, using text simply as one possible interface.
> </p><p style="text-align:right">Mark Priestley, Science of Operations (2011)</p>

When we treat computation as an interaction, miscomputation and errors take yet another form. This
is easier to see when we look at more recent work that treats programming as interaction or
[_communication_, a term that Sam Aaron uses](http://sam.aaron.name/2009/11/24/hand-shadows-as-an-analogy-for-understanding-communicative-programming.html).
A nice example of this is live coding environments for performing music. This changes our metaphors
for miscomputation in interesting ways:

> <p style="margin-bottom:5px">
> An error in the performance of classical music occurs when the performer plays a note that is not written on the page. In musical genres that are not notated so closely (...), there are no wrong notes – only notes that are more or less appropriate to the performance.
> </p><p style="text-align:right">Alan Blackwell and Nick Collins,<br /><a href="http://community.dur.ac.uk/nick.collins/research/proglangasmusicinstr.pdf">The Programming Language as a Musical Instrument</a> (2005)</p>

Live coding brings the same ideas to programming:

> <p style="margin-bottom:5px">
> [Live coders] may well prefer to accept the results of an imperfect execution.
> [They] might perhaps compensate for an unexpected result by manual intervention
> (like a guitarist lifting his finger from a discordant note), or even accept the result
> as a serendipitous alternative to the original note.
> </p><p style="text-align:right">Alan Blackwell and Nick Collins,<br /><a href="http://community.dur.ac.uk/nick.collins/research/proglangasmusicinstr.pdf">The Programming Language as a Musical Instrument</a> (2005)</p>

One interesting point is that making miscomputation apparent (we hear a dissonant note) enables
live coder to quickly react. This might mean adapting the program to correct the behaviour or
perhaps incorporating the behaviour into the system (when the accidental note fits well).

It is easy to understand this approach in live coded music, but that is just one of the domains.
In Smalltalk, live coding can be used to interactively change a running system in response to errors
and it is perfectly possible to see this exact approach used in other environments:

 - When we look at trading and finance, people are already engaged in live interactions - though
   mostly through spreadsheet-like applications. Imagine they could write live code to trade on the
   markets! The ability to see unexpected behaviour and quickly react (to adapt it) would, no doubt,
   be one of the key aspects of the programming environment.

 - In the web space, someone is typically ready to interact with the servers all the time if something
   goes wrong. The DevOps movement is making the link between developers and the system shorter to
   the extent that you can (almost?) live code your web server.  

In summary, in live coding, errors and miscomputations can be treated as something that provokes creativity.
You can easily see how this works in live coded music (and other performances), but I think this extends
further (and I believe we'll see more of this in the future).

Conclusions
-----------

Perhaps the most interesting message from the blog post is that miscomputation does not always have
to be fully avoided. Avoiding miscomputation at all cost (through formal proofs or thorough testing) is
the most common technique, but there are interesting alternatives that embrace miscomputation.

In _test-driven development_, errors become a driving force for the development process, but we still
hope to remove them from the running software. In _Erlang_, errors are a normal part of program execution,
but the runtime handles them (e.g. by restarting an agent) and so they are not visible to the user.
In _live coding_, errors are visible and are part of the live interaction with the system, be it live
coded music, web server or a (futuristic) financial system.

Another really important point here is the difference between different approaches to programming.
When you're comparing Idris, Java, Erlang and Ruby [used in Sonic Pi](http://sonic-pi.net/), you can
talk about different language features, but that's completely missing the point. The differences are
more in how they are used and how the communities approach different problems - here, the problem of
errors.

Finally, if we sort the approaches to errors by decades when they appeared, we get Algol (1960s),
Smalltalk (1970s), Erlang (1980s) and TDD (2000s), so there is still room for new ideas!
I think errors and miscomputations are here to stay, but how will future languages deal with them
is an interesting question.