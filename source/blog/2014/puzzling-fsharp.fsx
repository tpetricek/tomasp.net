(**

Solving fun puzzles with F#
===========================

 - date: 2014-03-25T14:27:08.0544354+00:00
 - description: Do you need to convince your friends and family that programming can be fun? Here is a story of how I managed to do that with F# at New Year's eve! I got a puzzle as a gift for Christmas and after a few days of failed attempts, I managed to solve it with F# in about 1 hour...
 - layout: article
 - image: http://tomasp.net/blog/2014/puzzling-fsharp/cube.jpg
 - tags: f#,fun,functional programming
 - title: Solving fun puzzles with F#
 - url: 2014/puzzling-fsharp

--------------------------------------------------------------------------------
 - standalone

Do you need to convince your friends & family that programming can be fun? 
For the last Christmas, I got a puzzle as a gift. It is a number of small
cubes, joined together, that can be rotated and folded to form a bigger
(4x4x4) cube. 

We spent the last few days of the year with family and a couple of friends
and I left the puzzle on the table. Every time I walked around, someone was
playing with it without much success. In the end, I said that if noone solves
it until 31 December, I'll write a program to do it. Which I did between 7 PM
and 8 PM and, voilà, here is what I got...

<div style="text-align:center;margin:10px">
<a href="http://tomasp.net/blog/2014/puzzling-fsharp/puzzle.jpg" target="_blank"><img src="http://tomasp.net/blog/2014/puzzling-fsharp/puzzle.jpg" style="height:200px;margin-right:20px;border-style:none" /></a>
<a href="http://tomasp.net/blog/2014/puzzling-fsharp/cube.jpg" target="_blank"><img src="http://tomasp.net/blog/2014/puzzling-fsharp/cube.jpg" style="height:200px;margin-left:20px;border-style:none" /></a>
</div>

So, how do you solve a puzzle in about 1 hour on New Year's eve?

--------------------------------------------------------------------------------
*)
(*** hide ***)
#nowarn "1189"
#I "../packages/2014/functional3d"
(**

Do you need to convince your friends & family that programming can be fun? 
For the last Christmas, I got a puzzle as a gift. It is a number of small
cubes, joined together, that can be rotated and folded to form a bigger
(4x4x4) cube. 

We spent the last few days of the year with family and a couple of friends
and I left the puzzle on the table. Every time I walked around, someone was
playing with it without much success. In the end, I said that if noone solves
it until 31 December, I'll write a program to do it. Which I did between 7 PM
and 8 PM and, voilà, here is what I got...

<div style="text-align:center;margin:10px">
<a href="http://tomasp.net/blog/2014/puzzling-fsharp/puzzle.jpg" target="_blank"><img src="http://tomasp.net/blog/2014/puzzling-fsharp/puzzle.jpg" style="height:200px;margin-right:20px;border-style:none" /></a>
<a href="http://tomasp.net/blog/2014/puzzling-fsharp/cube.jpg" target="_blank"><img src="http://tomasp.net/blog/2014/puzzling-fsharp/cube.jpg" style="height:200px;margin-left:20px;border-style:none" /></a>
</div>

So, how do you solve a puzzle in about 1 hour on New Year's eve?

Modeling the problem
--------------------

As with any problem in F#, we start by modeling the domain. The puzzle consists
of parts (small cubes) that have two important properties:
*)
type Color = Black | White
type Kind = Straight | Turn
type Part = Color * Kind
(**
Each part has a color (black or white) and there are two kinds of parts. In one
kind, the string connecting the parts goes straight through the part (and so it
does not allow any useful rotation). In the other part, the string turns and 
so the next part can point in one of four directions. Assuming the previous part
points from bottom to the top, we can now go to the front, back, left or right.

We'll also need to represent positions of the parts in the final cube and the
direction in which a part is pointing:
*)
type Position = int * int * int
type Direction = int * int * int
(**
Here, the position will be integers from 0 to 3 and direction will always contain
exactly one 1 or -1 value (with zeros for all other axes). We could make the model
more precise, but this will make calculations easy (note that we only have 1 hour
to finish :-)).

Now, the entire puzzle (first picture) is simply a list of parts:
*)
type Shape = list<Part>
(**
The model so far can actually be understood by non-programmers. It has been 
tested on humans (but only close relatives and friends!) and it worked fine :-).
This is one of the key strengths of domain modeling with F#...

Implementing the algorithm
--------------------------

The algorithm we'll implement is quite straightforward backtracking. We'll simulate
the different ways in which the puzzle can be folded (starting from 4 different 
positions as others would be symmetric). When we hit a state that would not be valid
(there is a part already or the colors would not match), we'll go back and try
another folding.

When we have a `Straight` part, the next part of the puzzle will always be one
step further in the current direction. This is easy. The interesting thing is
when we have a `Turn` part - in that case we can go in four different directions,
which are calculated using the following functions:
*)

/// Given 'Position' and 'Direction' calculate
/// a new position (by adding the offsets)
let move (x,y,z) (dx,dy,dz) = (x+dx, y+dy, z+dz)

/// For a 'Turn' part oriented in the given 'Direction'
/// generate a list of possible next Directions
let offsets (dx, dy, dz) = 
  [ if dx = 0 then for i in [-1;1] do yield i, 0, 0
    if dy = 0 then for i in [-1;1] do yield 0, i, 0
    if dz = 0 then for i in [-1;1] do yield 0, 0, i ]

/// Given a current 'Position' and 'Direction', get a list
/// of possible new Directions and corresponding Positions
let rotate position direction : list<Direction * Position> = 
  [ for offs in offsets direction -> 
      offs, move position offs ]
(**
To understand this part, you still do not need to be a programmer. And the
fact that we are writing just functions makes this quite easy too. However,
you definitely need some mathematical background.

### Checking valid moves

The next part of the preparation is to write a function for checking whether
a move is valid. There are two conditions:

 * A move is not valid if there is a part already at a given `Position`.
 * We can only put parts inside the range of the cube (all coordinates are
   within 0 .. 3)
 * The colors of the parts should match the pattern that you can see in 
   the picture above.

We'll keep a set of occupied positions using immutable F# `Set`. For color
patterns, we can build a simple dictionary with expected colors - as the 
pattern is regular, we can only store colors for smaller 2x2x2 cube (and then
check using `pos / 2`.
*)

/// A set of occupied positions
type CubeState = Set<Position>

/// Expected colors for each position
let colorMap = 
  [ (0,0,0), Black; (0,0,1), White;
    (0,1,0), White; (1,0,0), White;
    (1,1,1), White; (1,1,0), Black;
    (1,0,1), Black; (0,1,1), Black ] 
  |> dict

(**
The following `isValidPosition` function takes a `Position` and a current `CubeState`
and checks whether it is a valid position (we'll handle colors later):
*)

/// Checks that the specified position is "inside" the
/// cube and there is no part already in that place
let isValidPosition position (state:CubeState) = 
  let x, y, z = position
  let free = not (state.Contains(position))
  let inRange = 
    x >= 0 && y >= 0 && z >= 0 && 
    x <= 3 && y <= 3 && z <= 3
  free && inRange

(**
This ignores the color constraints. Mainly because I added this later when solving the 
puzzle. Surprisingly, it is quite important constraint - there are many more options without
the restriction on colors. We'll put the check in the main algorithm later.

### Generating moves

Before looking at the main part, there are two more functions we need. Given a part
(which can be `Straight` or `Turn`), current position and direction and also the current
state, we want to get all valid directions and locations for the next part:
*)
/// Given a current Position & Direction and current
/// Part, get a list of next Positions & Directions
let getPositions position direction part = 
  match part with 
  | (_, Straight) -> [ direction, move position direction ]  
  | (_, Turn) -> rotate position direction

/// Get next valid positions (with directions)
let getValidPositions pos dir part state =
  [ for dir, pos in getPositions pos dir part do
      if isValidPosition pos state then yield dir, pos ]
(**
### Backtracking using recursion

So far, most of the code was quite easy. You can explain the type definitions
to people without any technical skills. Understanding the main algorithm 
probably requires some programming background, but it is still surprisingly
simple.

We write a recursive function `solve` that keeps the current position and
direction (`pos` and `dir`), the current state of the cube (set of occupied
positions) and a _trace_. The trace is a list of places where we put cubes
earlier and this will contain the final result at the end. 

The last parameter is `shape` which is the list of parts. As we iterate, we
always take out the head of the list and call the function recursively for 
the tail (the rest of the list excluding the first part):
*)
/// Recursive function that solves the puzzle using backtracking
let rec solve pos dir state trace (shape:Shape) = seq {
  match shape, pos with 
  | [part], _ -> 
      // We have reached the end. Return the trace!
      yield (List.rev trace)
  | part::shape, (x,y,z) when fst part = colorMap.[x/2,y/2,z/2] -> 
      // Current part has the rigth color, get valid 
      // positions for the next part & try all of them
      let moves = getValidPositions pos dir part state 
      for dir, pos in moves do
        let trace = pos::trace
        yield! solve pos dir (Set.add pos state) trace shape 
  | _ -> 
      // Current part does not have the right color
      // (so we have to go back and try another layout)
      () }
(**
The `solve` function uses pattern matching to handle three different cases:

 * The pattern `[part]` checks that there is just one last part left.
   In that case, we have a solution - because we already checked that the
   position is legal. So, we just return the trace and reverse it to get
   the steps from the first to the last.

 * The pattern `part::shape` checks that we have one or more parts and
   gives us the first one as `part`. We also decompose the position into
   `x,y,z` so that we can check whether the color of the part matches
   the required color.

   If all conditions are satisfied, we call `getValidPositions` to get
   all valid positions for the next step, iterate over them and try to
   return all possible results using `yield!` (more about this later).

 * The last case represents the case when the colors do not match. In this
   situation, we just "do nothing" and return back to the previous step.

Note that the whole function is wrapped in the `seq { .. }` block. This
means that we can generate all possible solutions (we return one using
the `yield` keyword). In practice, it turns out that they are all symmetric, but
this was an interesting experiment :-).

Solving the puzzle
------------------

So, we finally have all we need to solve the puzzle! Now comes the tedious
part, which is looking at the puzzle and noting down the exact sequence of 
colors and kinds. I did this by writing down two strings:

*)
let puzzle : Shape = 
  // Lookup tables for different colors/kinds
  let clrs  = dict ['b', Black; 'w', White]
  let kinds = dict ['s', Straight; 'r', Turn]
  // Read the string and build a list of 'Part' values
  ( "bbbwwwwwwwwbbbbwwwbbbbbbwwwbbwbbwbbwwwwwbbbbbbwbwwwbbbbwwwwwbbww",
    "srssrrrrrrrrrrsrrssrrrrssrsrrrrrrsrsrrrrrrrrrsrrsrrsrrssrrrsrrss" )
  ||> Seq.map2 (fun clr kind -> clrs.[clr], kinds.[kind])
  |> List.ofSeq

(**
To my surprise, I actually wrote this down correctly at the first attempt!
Now, we just call the `solve` function - we need to pick the first location 
and the first direction. It turns out that starting in one of the corners works
fine. In that case, the direction does not matter (because they will all be 
symmetric):
*)
// Pick starting location
let start = (0, 0, 0)
// Run the 'solve' function 
let res = solve start (0, 0, 1) (set [start]) [start] puzzle 

// Pick the first solution & print positions
let solution = Seq.head res 
solution |> Seq.iteri (fun i p -> 
  printfn "%d - %A" (i+1) p)
(**
This prints a sequence of X, Y and Z coordinates of the parts. With a bit of
effort, you can actually build the puzzle using this information, because you 
always know where the next part should go. This is what I did at first.

But then I realized that I could do one more step and make the demo really fun!

Building 3D visualization
=========================

Some time ago, I wrote a simple library for composing 3D objects and it turns out
to be a perfect fit. The library provides a couple of primitive shapes like cube, 
cone and cylinder and combinators for putting them together. Here is a simple example:
*)
// Reference OpenTK library & functional DSL
#r "OpenTK.dll"
#r "OpenTK.GLControl.dll"
#load "functional3d.fs"
open Functional3D
open System.Drawing

// Create a yellow cylinder and compose it with
Fun.color Color.Yellow Fun.cylinder $
// a red cone that is translated in the Z direction
Fun.translate
  (0.0, 0.0, -1.0)
  (Fun.color Color.Red Fun.cone)
(**

<div style="float:right">
<a href="tower.jpg" target="_blank"><img src="tower.jpg" style="height:150px;border-style:none" /></a>
</div>

In just a two lines of code (if you do not try to format things nicely
for a blog), you can easily put together a simple tower!

By default, all objects are created in the middle of the world, so if
we want to compose them, we have to use `Fun.translate` to move them 
around. Then you can use the `$` operator to combine multiple shapes
into a single one.

## Visualizing cube puzzle

Building a visualization for the cube puzzle is quite simple. We write
a function `draw i` that takes the first `i` elements of the trace,
generates one cube for each of them and moves them to the right position:
*)

/// Convert coordinate to float values
let fl (x,y,z) = 
  (float x, float y, float z)

/// Draw the first 'i' steps of the puzzle
let draw i =
  solution 
  |> Seq.take i
  |> Seq.map (fun ((x, y, z) as p) -> 
      // Get the expected color based on the color map
      let color = 
        if colorMap.[x/2,y/2,z/2] = Black then 
          Color.SaddleBrown else Color.BurlyWood
      // Create a cube, make it a bit smaller, change
      // its color and move it to the right position
      Fun.cube
      |> Fun.scale (0.95,0.95,0.95)
      |> Fun.color color
      |> Fun.translate (fl p) )
  |> Seq.reduce ($)
(**
The function first generates a sequence of cubes and then composes them
into a single big 3D shape using the `Seq.reduce` function. This applies
the `$` operator gradually to all parts building the final object.

Now we can call this from a simple asynchronous loop that adds steps one-by-one,
with a 200ms delay between them:
*)
Async.StartImmediate <| async { 
  for i in 1 .. 64 do 
    do! Async.Sleep(200)
    Fun.show (draw i) }
(**
The computation waits 200ms, then it builds the 3D model of the next step
and displays it using `Fun.show`. We start the computation using 
`Async.StartImmediate`, which makes sure that all the processing is done
on the main user-interface thread and so we can access the UI elements and
actually update the control showing the visualization.

## Conclusions

First of all, let the results speak for themselves:

<div style="text-align:center;margin-left:auto;margin-right:auto;">
<iframe class="youtube-player" type="text/html" width="460" height="365" src="http://www.youtube.com/embed/w835_D0sgUc" allowfullscreen frameborder="0">
</iframe>
</div>

I think it is quite amazing how much can be done in such a small number of lines
of code in so little amount of time. 

After watching people play with the puzzle for a couple of days, I wrote most 
of the code to solve the puzzle in about 1.5 hours during a New Year's afternoon 
and eve and the puzzle was solved! This alone would be quite nice, but the fact
that I was able to add visualization in about 15 minutes made this really a nice
example of why programming with F# is fun :-).

So, if you want to impress your family and friends with your programming skills, 
learning F# is most certainly the way to go!

*)
