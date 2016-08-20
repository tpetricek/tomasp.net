(**
Comparing date range handling in C# and F#
==========================================

 - date: 2015-04-22T16:55:20.5498839+01:00
 - description: I was recently working on some code for handling data ranges and I wrote initial version in C#. Later, I realized that I needed an F# version, so I went through the process of rewriting a simple function from C# to F#. This blog post compares the two versions.
 - layout: post
 - image: http://tomasp.net/blog/2015/restricting-ranges/card.png
 - tags: f#,c#,deedle,linq,functional programming
 - title: Comparing date range handling in C# and F#
 - url: 2015/restricting-ranges

--------------------------------------------------------------------------------
 - standalone

I was recently working on some code for handling date ranges in 
[Deedle](http://github.com/blueMountainCapital/Deedle). Although Deedle is written in F#, 
I also wrote some internal integration code in C#. After doing that, I realized that the 
code I wrote is actually reusable and should be a part of Deedle itself and so I went through
the process of rewriting a simple function from (fairly functional) C# to F#. This is a small
(and by no means representative!) example, but I think it nicely shows some of the reasons why
I like F#, so I thought I'd share it.

One thing that we are adding to Deedle is a "BigDeedle" implementation of internal data 
structures. The idea is that you can load very big frames and series without actually loading
all data into memory.

When you perform slicing on a large series and then merge some of the parts of the series
(say, years 2010, 2012 and 2014), you end up with a series that combines a
couple of chunks. If you then restrict the series (say, from June 2012 to June 2014), you
need to restrict the ranges of the chunks:

<img src="http://tomasp.net/blog/2015/restricting-ranges/drawing.png" alt="Demonstration" style="margin:15px; width:370px" />

As the diagram shows, this is just a matter of iterating over the chunks, keeping those 
in the range, dropping those outside of the range and restrictingthe boundaries of the other
chunks. So, let's start with the C# version I wrote.

--------------------------------------------------------------------------------


I was recently working on some code for handling date ranges in 
[Deedle](http://github.com/blueMountainCapital/Deedle). Although Deedle is written in F#, 
I also wrote some internal integration code in C#. After doing that, I realized that the 
code I wrote is actually reusable and should be a part of Deedle itself and so I went through
the process of rewriting a simple function from (fairly functional) C# to F#. This is a small
(and by no means representative!) example, but I think it nicely shows some of the reasons why
I like F#, so I thought I'd share it.

The problem
-----------

One thing that we are adding to Deedle is a "BigDeedle" implementation of internal data 
structures. The idea is that you can load very big frames and series without actually loading
all data into memory.

When you perform slicing on a large series and then merge some of the parts of the series
(say, years 2010, 2012 and 2014), you end up with a series that combines a
couple of chunks. If you then restrict the series (say, from June 2012 to June 2014), you
need to restrict the ranges of the chunks:

<img src="drawing.png" alt="Demonstration" style="margin:15px 0px 15px 25px" />

As the diagram shows, this is just a matter of iterating over the chunks, keeping those 
in the range, dropping those outside of the range and restrictingthe boundaries of the other
chunks. So, let's start with the C# version I wrote.

Restricting ranges in C#
------------------------

To keep the sample self-contained, I'll also include a simple definition of a `Date` type
that I was using in my experiments. This is not the key part, but it is worth showing. A
date is essentially an integer - a number of days since the beginning of the universe (or 
some other important milestone):

    [lang=csharp]
    /// <summary>
    /// A date as a number of days since the beginning
    /// </summary>
    class Date {
      public int Offset { get; set; }
      public static bool operator <=(Date d1, Date d2) { return d1.Offset <= d2.Offset; }
      public static bool operator >=(Date d1, Date d2) { return d1.Offset >= d2.Offset; }
      public static bool operator <(Date d1, Date d2) { return d1.Offset < d2.Offset; }
      public static bool operator >(Date d1, Date d2) { return d1.Offset > d2.Offset; }
    }
    /// <summary>
    /// An array of ranges represented as date pairs
    /// </summary>
    class Ranges {
      public Tuple<Date, Date>[] Ranges { get; set; }
    }

The `Date` type defines a couple of custom operators so that we can compare dates (I only defined
those that I needed). The `Ranges` type is the simplest possible wrapper over an array of `Date`
pairs. The type is internal, so I was just using tuples to save some typing.
 
Next, let's have a look at the `RestrictRanges` function. This takes `Ranges` together with
lower and upper bound of the restriction. It then iterates over the ranges using `SelectMany`
and returns the range unmodified (if it is within the restriction), skips it (if it is outside)
or adjusts its lower and upper bounds:

    [lang=csharp]
    /// <summary>
    /// Restrict the specified collection of ranges 
    /// according to the provided restriction range
    /// </summary>
    static Ranges RestrictRanges(Ranges ranges, Date loRestr, Date hiRestr)
    {
      var newRanges = ranges.Ranges.SelectMany(range =>
      {
        if (range.Item1 >= loRestr && range.Item2 <= hiRestr)
          return new[] { range };
        else if (range.Item2 < loRestr || range.Item1 > hiRestr)
          return new Tuple<Date, Date>[0];
        else
          return new[] { Tuple.Create
              ( range.Item1 > loRestr ? range.Item1 : loRestr, 
                range.Item2 < hiRestr ? range.Item2 : hiRestr ) };
      }).ToArray();
      return new Ranges { Ranges = newRanges };
    }

This is fairly simple and readable piece of code. We might be able to make it a bit nicer if we
used _iterators_, but that would require a separate method (because we are returning `Ranges`
and not `IEnumerable<T>` here). We could also use a named type rather than tuple (to replace
`Item1` and `Item2` with `Lower` and `Upper`), which would make it a bit more readable, but it
wouldn't change the structure. Before discussing the code further, let's look at the F# version.

Restricting ranges in F#
------------------------

As with the C# version, we need to start with type definitions. This is not really the important
part, but I wanted to have a self-contained sample for the blog, so I'm including those too:
*)
/// A date as a number of days since the beginning
type Date = { Offset : int }
/// An array of ranges represented as date pairs
type Ranges = { Ranges : (Date*Date)[] }
(**
If we're happy to use a simple F# record, then this is all we need. For records, the compiler
automatically provides structural equality and structural comparison. This means that we can 
write `{ Offset = 123 } <= { Offset = 125 }` straight away and the result is `true`. We can also
omit the `<summary>` tag in the comment (F# adds it automatically).

This is not really the main thing though. In practice, I would use records for simple types that
do not have complex internal logic - and so I might start with the above `Date` record, but later
turn it into something that is closer to the C# type with explicit definitions. Nevertheless, it
is nice that we can write simple record in the first step and F# gets all the defaults right
(makes it immutable, adds equality and comparison).

The more interesting thing is the `restrictRanges` function. We follow _exactly_ the same logic
as before (even using `Array.collect` which is an equivalent of `SelectMany`):
*)
/// Restrict the specified collection of ranges 
/// according to the provided restriction range
let restrictRanges (loRestr:Date, hiRestr:Date) ranges =
  let newRanges = 
    ranges.Ranges |> Array.collect (fun (lo, hi) ->
        if lo >= loRestr && hi <= hiRestr then [| lo, hi |]
        elif hi < loRestr || lo > hiRestr then [| |]
        else [| max lo loRestr, min hi hiRestr |] )
  { Ranges = newRanges }
(**
Alternatively, we could use sequence expressions (which I prefer in this case) and rewrite replace
the `Array.collect` function with the `[| .. |]` block, which gives us:
*)
/// Restrict the specified collection of ranges 
/// according to the provided restriction range
let restrictRangesArrExpr (loRestr:Date, hiRestr:Date) ranges =
  let newRanges = 
    [| for lo, hi in ranges.Ranges do
        if lo >= loRestr && hi <= hiRestr then yield lo, hi
        elif hi < loRestr || lo > hiRestr then ()
        else yield max lo loRestr, min hi hiRestr |]
  { Ranges = newRanges }
(**
Comparing the two versions
--------------------------

It is certainly a matter of taste, but I was quite surprised by how different the F# version
looks - both versions implement the same logic and both use functional programming style, but
there are a few little details that (in my opinion) make the F# version nicer.

 - Pattern matching on tuples really helps here. We can just write `(lo, hi)` as the function
   parameter and then use the two variables rather than accessing the items using 
   `range.Item1` and `range.Item2` (or, if we had a named type `range.Lower` and `range.Upper`).
   As we are using the variables locally (on just 4 lines of code), I think the additional 
   verbosity is not really helping readablity in this case.

 - Type inference and array literals mean that we can just write `[| |]` to return an empty
   array. This is quite a simplification from `new Tuple<Date, Date>[0]`, which is what we
   had to write before. This is even easier with sequence expressions, where we just do nothing
   using `()` and use `yield` in other branches.

 - The fact that we can use `Array.collect` rather than using `SelectMany` on `IEnumerable<T>`
   is a nice little detail too - we do not have to explicitly convert the result to array using
   `ToArray`.

 - Finally, the `max` and `min` functions in F# are [generic numerical 
   functions](http://tomasp.net/blog/fsharp-generic-numeric.aspx/), which means that they work
   on any type that supports comparison. They are also `inline` and so they are fast and do not 
   require boxing (which would be the case if you wrote a function like this in C# - probably a 
   reason why `Math.Max` does not have a generic overload...).

For me, the interesting thing about this comparison is that it is not looking at any big ideas.
It is comparing two functions written in the same style, using pretty much the same code. But
even then, the using F# gives us a couple of little benefits that make the code (I think) nicer.

There is no fundamental reason why C# could not do any of these in a future version. In fact, I 
think that pattern matching on tuples gets mentioned quite often. But this were just 4 "little 
things" that I found in one 7-line function...

It might also be the case that I'm more used to writing and reading F# - this is, of course, true - 
but if we look at what we deleted, I think it was mostly noise: things like `new [] { .. }`,
`Tuple<Date, Date>`, `range.Item1 > loRestr ? .. : ..`, `ToArray()`, `.Item1` and `.Item2` are
all about the implementation details and not about the function logic.

*)