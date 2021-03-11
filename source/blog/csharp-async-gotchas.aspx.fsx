(**
Async in C# and F#: Asynchronous gotchas in C#
==============================================

 - date: 2013-04-15T04:00:03.0000000
 - description: This article is inspired by an MVP summit talk about common pitfalls in the C# asynchronous programming model. I look at a number of easy to make mistakes when writing asynchronous code in C# and demonstrate that most of them would not easily happen when using F#.
 - layout: article
 - tags: async,c#,f#
 - title: Async in C# and F#: Asynchronous gotchas in C#
 - url: csharp-async-gotchas.aspx

--------------------------------------------------------------------------------
 - standalone


Back in February, I attended the annual MVP summit - an [event organized by Microsoft
for MVPs](http://www.2013mvpsummit.com/about). I used that opportunity to also visit
Boston and New York and do two F# talks and to record a [Channel9 lecutre about type
providers][love-data].
Despite all the _other activities_ (often involving pubs, other F# people and long 
sleeping in the mornings), I also managed to come to some talks!

<div style="margin-left:auto;margin-right:auto;width:379px;margin-top:10px;margin-bottom:20px;">
<img src="http://tomasp.net/articles/csharp-async-gotchas/async-clinic.png" style="width:379px;" />
</div>

One (non-NDA) talk was the [Async Clinic][async-clinic] talk about the new `async` and `await` keywords 
in C# 5.0. Lucian and Stephen talked about common problems that C# developers face when 
writing asynchronous programs. In this blog post, I'll look at some of the problems from 
the F# perspective. The talk was quite lively, and someone recorded the reaction of the 
F# part of the audience as follows...

--------------------------------------------------------------------------------
*)
(*** hide ***)
open System
open System.Threading
open System.Threading.Tasks

(**
Back in February, I attended the annual MVP summit - an [event organized by Microsoft
for MVPs](http://www.2013mvpsummit.com/about). I used that opportunity to also visit
Boston and New York and do two F# talks and to record a [Channel9 lecutre about type
providers][love-data].
Despite all the _other activities_ (often involving pubs, other F# people and long 
sleeping in the mornings), I also managed to come to some talks!

<div style="margin-left:auto;margin-right:auto;width:379px;margin-top:10px;margin-bottom:20px;">
<img src="http://tomasp.net/articles/csharp-async-gotchas/async-clinic.png" style="width:379px;" />
</div>

One (non-NDA) talk was the [Async Clinic][async-clinic] talk about the new `async` and `await` keywords 
in C# 5.0. Lucian and Stephen talked about common problems that C# developers face when 
writing asynchronous programs. In this blog post, I'll look at some of the problems from 
the F# perspective. The talk was quite lively, and someone recorded the reaction of the 
F# part of the audience as follows:

<div style="margin-left:auto;margin-right:auto;width:379px;margin-top:10px;margin-bottom:20px;">
<a href="https://twitter.com/josefajardo/status/303998917027192832"><img src="http://tomasp.net/articles/csharp-async-gotchas/tweet.png" style="border-style:none" /></a>
</div>

Why is that? It turns out that many of the common errors are not possible (or much less
likely) when using the F# asynchronous model (which has been around [since F# 1.9.2.7, which
was released in 2007](http://blogs.msdn.com/b/dsyme/archive/2007/07/27/f-1-9-2-7-released.aspx)
and have been shipped with Visual Studio 2008).

  [love-data]: http://channel9.msdn.com/posts/Tomas-Petricek-How-F-Learned-to-Stop-Worrying-and-Love-the-Data "Tomas Petricek (Channel 9): How F# Learned to Stop Worrying and Love the Data"
  [async-clinic]: http://blogs.msdn.com/b/pfxteam/archive/2013/02/20/mvp-summit-presentation-on-async.aspx "Lucian Wischik, Stephen Toub: Async Clinic"

## Gotcha #1: Async does not run asynchronously

Let's go straight to the first tricky aspect of the C# asynchronous programming model. Take 
a look at the following example and figure out in what order will the strings be printed 
(I could not find the exact code shown at the talk, but I remember Lucian showing something
similar):

    [lang=csharp]
    async Task WorkThenWait() {
      Thread.Sleep(1000);
      Console.WriteLine("work");
      await Task.Delay(1000);
    }
    
    void Demo() {
      var child = WorkThenWait();
      Console.WriteLine("started");
      child.Wait();
      Console.WriteLine("completed");
    }

If you guessed that it prints "started", "work" and "completed" then you're wrong. The code
prints "work", "started" and "completed", try it! What the author intended was to start 
the work (by calling `WorkThenWait`) and then await for the task later. The problem is that
`WorkThenWait` starts by doing some heavy computations (here, `Thread.Sleep`) and only after
that uses `await`.

In C#, the first part of the code in `async` method is executed synchronously (on the 
thread of the caller). You could fix that, for example, by adding `await Task.Yield()` at the 
beginning.

### Corresponding F# code

This is not a problem in F#. When writing async code in F#, the entire code inside 
`async { ... }` block is all delayed and only started later (when you explicitly start it).
The above C# code corresponds to the following F#:

*)

let workThenWait() = 
  Thread.Sleep(1000)
  printfn "work done"
  async { do! Async.Sleep(1000) }

let demo() = 
  let work = workThenWait() |> Async.StartAsTask
  printfn "started"
  work.Wait()
  printfn "completed"

(**
It is quite clear that the `workThenWait` function is not doing the work (`Thread.Sleep`) 
as part of the asynchronous computation and that it will be executed when the function
is called (and not when the async workflow is started).
The usual F# pattern is to wrap the entire function body in `async`. In F#, you
would write the following, which works as expected:
*)

let workThenWait() = async { 
  Thread.Sleep(1000)
  printfn "work done"
  do! Async.Sleep(1000) }

(**
## Gotcha #2: Ignoring results

Here is another gotcha in the C# asynchronous programming model (this one is taken directly
from Lucian's slides). Guess what happens when you run the following asynchronous method:

    [lang=csharp]
    async Task Handler() {
      Console.WriteLine("Before");
      Task.Delay(1000);
      Console.WriteLine("After");
    }

Were you expecting that it prints "Before", waits 1 second and then prints "After"? Wrong!
It prints both messages immediately without any waiting in between. The problem is that
`Task.Delay` _returns_ a `Task` and we forgot to await until it completes using `await`.

### Corresponding F# code

Again, you would probably not hit this issue in F#. You can surely write code that calls
`Async.Sleep` and ignores the returned `Async<unit>`:
*)

let handler() = async {
  printfn "Before"
  Async.Sleep(1000)
  printfn "After" }

(**
If you paste the code in Visual Studio, MonoDevelop or Try F#, you get an immediate 
feedback with a warning saying that:

> warning FS0020: This expression should have type `unit`, but has type 
> `Async<unit>`. Use `ignore` to discard the result of the expression, or 
> `let` to bind the result to a name.

You can still compile the code and run it, but if you read the warning, you'll see
that the expression returns `Async<unit>` and you need to await it using `do!`:

*)

let handler() = async {
  printfn "Before"
  do! Async.Sleep(1000)
  printfn "After" }

(**

## Gotcha #3: Async void methods

Quite a lot of time in the talk was dedicated to _async void_ methods. If you write
`async void Foo() { ... }`, then the C# compiler generates a method that returns
`void`. Under the cover, it creates and starts a task. This means that you have no way
of telling when the work has actually happened.

Here is a recommendation on the _async void_ pattern from the talk:

<div style="margin-left:auto;margin-right:auto;width:379px;margin-top:10px;margin-bottom:20px;">
<img src="http://tomasp.net/articles/csharp-async-gotchas/async-void.png" style="width:379px;" />
</div>

To be fair - async void methods _can_ be useful when you're writing an event handler.
Event handlers should return `void` and they often start some work that continues in 
background. But I do not think this is really useful in the world of MVVM - but it 
surely makes nice demos at conference talks.

Let me demonstrate the problem using a snippet from [MSDN Magazine article][best-pract]
on asynchronous programming in C#:

    [lang=csharp]
    async void ThrowExceptionAsync() {
      throw new InvalidOperationException();
    }

    public void CallThrowExceptionAsync() {
      try {
        ThrowExceptionAsync();
      } catch (Exception) {
        Console.WriteLine("Failed");
      }
    }

Do you think that the code prints "Failed"? I suppose you already understood the style
of this blog post... Indeed, the exception is not handled because `ThrowExceptionAsync`
starts the work and returns immediately (and the exception happens somewhere on a background
thread).

  [best-pract]: http://msdn.microsoft.com/en-us/magazine/jj991977.aspx "Stephen Cleary: Best Practices in Asynchronous Programming"

### Corresponding F# code

So, if you should not be using a programming language feature, then it is probably 
better not to include the feature in the first place. F# does not let you write
_async void_ functions - when you wrap function body in the `async { ... }` block,
its return type will be `Async<T>`. If you used type annotations and demanded `unit`, 
you would get a type mismatch.

You can still write code that corresponds to the above C# using `Async.Start`:
*)

let throwExceptionAsync() = async {
  raise <| new InvalidOperationException() }

let callThrowExceptionAsync() = 
  try
    throwExceptionAsync()
    |> Async.Start
  with e ->
    printfn "Failed"
(** 

This will also not handle the exception. But it is more obvious what is going on because
we had to write `Async.Start` explicitly. If we did not write it, we would get a 
warning saying that the function returns `Async<void>` and we are ignoring the result
(the same as in the earlier section "Ignoring results").

## Gotcha #4: Async void lambda functions

Even trickier case is when you pass asynchronous lambda function to some method as a 
delegate. In this case, the C# compiler infers the type of method from the delegate type.
If you use the `Action` delegate (or similar), then the compiler produces async void
function (which starts the work and returns `void`). If you use the `Func<Task>` delegate,
the compiler generates a function that returns `Task`.

Here is a sample from Lucian's slides. Does the following (perfectly valid) code finish 
in 1 second (after all the tasks finish sleeping), or does it finish immediately?

    [lang=csharp]
    Parallel.For(0, 10, async i => {
      await Task.Delay(1000);
    });

You cannot know that, unless you know that `For` only has overloads that take `Action`
delegates - and thus the lambda function will always be compiled as async void. This
also means that adding such (maybe useful?) overload would be a breaking change.

### Corresponding F# code

The F# language does not have special "async lambda functions", but you can surely
write a lambda function that returns asynchronous computation. The return type of such
function will be `Async<T>` and so it cannot be passed as an argument to methods that
expect void-returning delegate. The following F# code does not compile:
*)

Parallel.For(0, 10, fun i -> async {
  do! Async.Sleep(1000) 
})

(**
The error message simply says that a function type `int -> Async<unit>` is not 
compatible with the `Action<int>` delegate (which would be `int -> unit` in F#):

> error FS0041: No overloads match for method `For`. The available overloads 
> are shown below (or in the Error List window).

To get the same behaviour as the above C# code, we need to explicitly start the
work. If you want to start asynchronous workflow in the background, then you can
easily do that using `Async.Start` (which takes a unit-returning asynchronous
computation, schedules it and returns `unit`):
*)

Parallel.For(0, 10, fun i -> Async.Start(async {
  do! Async.Sleep(1000) 
}))

(** 
You can certainly write this, but it is quite easy to see what is going on. 
It is also not difficult to see that we are wasting resources, because the point
of `Parallel.For` is that it runs _CPU-intensive_ computations (which are typically
synchronous functions) in parallel. 

## Gotcha #5: Nesting of tasks

I think that Lucian included the next one just to test the mental-compilation
skills of the people in the audience, but here it is. The question is, does the
following code wait 1 second between the two prints?

    [lang=csharp]
    Console.WriteLine("Before");
    await Task.Factory.StartNew(
      async () => { await Task.Delay(1000); });
    Console.WriteLine("After");

Again, quite unexpectedly, this does not actually wait between the two writes.
How is that possible? The `StartNew` method takes a delegate and returns a `Task<T>`
where `T` is the type returned by the delegate. In the above case, the delegate
returns `Task`, so we get `Task<Task>` as the result. Using `await` waits only
for the completion of the outer task (which immediately returns the inner task)
and the inner task is then ignored.

In C#, you can fix this by using `Task.Run` instead of `StartNew` (or by dropping
the `async` and `await` in the lambda function).

Can we write something similar in F#? We can create a task that will return
`Async<unit>` using `Task.Factory.StartNew` and lambda function that returns an
async block. To await the task, we will need to convert it to asynchronous workflo
using `Async.AwaitTask`. This means we will get `Async<Async<unit>>`:

*)

async {
  do! Task.Factory.StartNew(fun () -> async { 
    do! Async.Sleep(1000) }) |> Async.AwaitTask }

(**
Again, this code does not compile. The problem is that the `do!` keyword requires
`Async<unit>` on the right-hand side, but it actually gets `Async<Async<unit>>`. In
other words, we cannot simply ignore the result. We need to explicitly do something
with it (we could use `Async.Ignore` to replicate the C# behaviour). The error 
message might not be as clear as the earlier messages, but you can get the idea:

> error FS0001: This expression was expected to have type `Async<unit>` 
> but here has type `unit`

## Gotcha #6: Not running asynchronously

Here is another problematic code snippet from Lucian's slide. This time, the problem
is quite simple. The following snippet defines an asynchronous method `FooAsync` and
calls it from a `Handler`, but the code does not run asynchronously:

    [lang=csharp]
    async Task FooAsync() {
      await Task.Delay(1000);
    }
    void Handler() {
      FooAsync().Wait();
    }

It is not too difficult to spot the issue - we are calling `FooAsync().Wait()`. This 
means that we create a task and then, using `Wait`, block until it completes. Simply
removing `Wait` fixes the problem, because we just want to start the task.

You can write the same code in F#, but asynchronous workflows do not use .NET Tasks
(which were originally designed for CPU-bound computations) and instead uses F#
`Async<T>` which does not come with `Wait`. This means you have to write:
*)

let fooAsync() = async {
  do! Async.Sleep(1000) }
let handler() = 
  fooAsync() |> Async.RunSynchronously

(**
You could certainly write such code by accident, but if you face a problem that it does
not run _asynchronously_, you can easily spot that the code calls
`RunSynchronously` and so the work is done - as the name suggests - _synchronously_.

Summary
-------

In this article, I looked at six cases where the C# asynchronous programming model
behaves in an unexpected way. Most of them were based on a talk by Lucian and Stephen
at the MVP summit, so thanks to both of them for sharing an interesting list of common
pitfalls! 

I tried to find the closest corresponding code snippet in F#, using asynchronous workflows.
In most of the cases, the F# compiler reports a warning or an error - or the programming
model does not have a (direct) way to express the same code. I think this supports the 
claim that I made [in an earlier blog post][fscsas] that _"The F# programming model definitely 
feels more suitable for functional (declarative) programming languages. I also think that it 
makes it easier to reason about what is going on"_.

Finally, this article should not be understood as a devastating criticism of C# async :-). I can 
fully understand why the C# design follows the principles it follows - for C#, it makes
sense to use `Task<T>` (instead of separate `Async<T>`), which has a number of implications.
And I can understand the reasoning behind other decisions too - it is likely the best way
to integrate asynchronous programming in C#. But at the same time, I think F# does a better 
job - partly because of the composability, but more importantly because of greate additions
like the [F# agents][fsagents]. Also, F# async has its problems too (the most common gotcha
is that tail-recursive functions must use `return!` instead of `do!` to avoid leaks), but 
that is a topic for a separate blog post.

  [fscsas]: http://tomasp.net/blog/csharp-fsharp-async-intro.aspx "Tomas Petricek: Asynchronous C# and F# (I.): Simultaneous introduction"
  [fsagents]: http://www.developerfusion.com/article/139804/an-introduction-to-f-agents "Tomas Petricek: An Introduction To F# Agents"



*)
