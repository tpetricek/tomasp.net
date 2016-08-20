(**

Stateful computations in F# with update monads
==============================================

 - date: 2014-05-13T15:41:01.7294268+01:00
 - description: Most discussions about monads, even in F#, start by looking at the well-known monads for handling state (reader, writer and state). In a recent paper, Danel Ahman and Tarmo Uustalu revisit these and build a nicer abstraction called <em>update monads</em>. I implemented the idea in F# and I find that update monads are an excellent fit for F# computation expressions!
 - layout: post
 - image: http://tomasp.net/blog/2014/update-monads/code.png
 - tags: f#,research,functional programming,monads
 - title: Stateful computations in F# with update monads
 - url: 2014/update-monads

--------------------------------------------------------------------------------
 - standalone

<img src="http://tomasp.net/blog/2014/update-monads/code.png" style="float:right;margin:20px;width:200px" />

Most discussions about monads, even in F#, start by looking at the well-known standard 
monads for handling state from Haskell. The _reader_ monad gives us a way to propagate 
some read-only state, the _writer_ monad makes it possible to (imperatively) produce 
values such as logs and the _state_ monad encapsulates state that can be read and changed.

These are no doubt useful in Haskell, but I never found them as important for F#.
The first reason is that F# supports state and mutation and often it is just easier
to use a mutable state. The second reason is that you have to implement three 
different computation builders for them. This does not fit very well with the F# style
where you need to name the computation explicitly, e.g. by writing `async { ... }`
(See also my [recent blog about the F# Computation Zoo paper][zoo]).

When visiting the Tallinn university in December (thanks to James Chapman, Juhan Ernits 
& Tarmo Uustalu for hosting me!), I discovered the work on _update monads_ by Danel Ahman 
and Tarmo Uustalu on [update monads][um], which elegantly unifies _reader_, _writer_ and 
_state_ monads using a single abstraction.

In this article, I implement the idea of _update monads_ in F#. Update monads are 
parameterized by _acts_ that capture the operations that can be done on the state.
This means that we can define just a single computation expression `update { ... }` 
and use it for writing computations of all three aforementioned kinds. 

--------------------------------------------------------------------------------
*)
(*** hide ***)
#nowarn "1189"

(**

<img src="http://tomasp.net/blog/2014/update-monads/code.png" style="float:right;margin:20px;width:200px" />

Most discussions about monads, even in F#, start by looking at the well-known standard 
monads for handling state from Haskell. The _reader_ monad gives us a way to propagate 
some read-only state, the _writer_ monad makes it possible to (imperatively) produce 
values such as logs and the _state_ monad encapsulates state that can be read and changed.

These are no doubt useful in Haskell, but I never found them as important for F#.
The first reason is that F# supports state and mutation and often it is just easier
to use a mutable state. The second reason is that you have to implement three 
different computation builders for them. This does not fit very well with the F# style
where you need to name the computation explicitly, e.g. by writing `async { ... }`
(See also my [recent blog about the F# Computation Zoo paper][zoo]).

When visiting the Tallinn university in December (thanks to James Chapman, Juhan Ernits 
& Tarmo Uustalu for hosting me!), I discovered the work on [update monads][um] by Danel 
Ahman and Tarmo Uustalu, which elegantly unifies _reader_, _writer_ and 
_state_ monads using a single abstraction.

In this article, I implement the idea of _update monads_ in F#. Update monads are 
parameterized by _acts_ that capture the operations that can be done on the state.
This means that we can define just a single computation expression `update { ... }` 
and use it for writing computations of all three aforementioned kinds. 

Introducing update monads
-------------------------

Before looking at the definition of update monads, it is useful to review the three
monads that we want to unify. The [update monads paper][um] has more details and you 
can also find other tutorials that implement these in F# - here, we'll only look at the
type definitions:
*)
/// Given a readonly state, produces a value
type Reader<'TState, 'T> = 'TState -> 'T
/// Produces a value together with additional state
type Writer<'TState, 'T> = 'TState * 'T
/// Given state, produces new state & a value
type State<'TState, 'T>  = 'TState -> 'TState * 'T
(**
If you look at the definitions, it looks like _reader_ and _writer_ are both a versions
of the _state_ with some aspect missing - reader does not produce a new state and writer
does not take previous state. 

How can we define a parameterized computation type that allows leaving one or the other
out? The idea of _update monads_ is quite simple. The trick is that we'll take two different
types - one representing the _state_ we can read and another representing the _updates_
that specify how to change the state:
*)

/// Represents an update monad - given a state, produce 
/// value and an update that can be applied to the state
type UpdateMonad<'TState, 'TUpdate, 'T> = 
  UM of ('TState -> 'TUpdate * 'T)

(**
To make the F# implementation a bit nicer, this is not defined as a type alias, but as a 
new type labeled with `UM` (this makes sure that the inferred types will always use the
name `UpdateMonad` for the type, rather than its definition).

To make this work, we also need some operations on the types representing states and updates.
In particular, we need:

 - **Unit update** which represents that no update is applied.
 - **Composition on updates** that allows combining multiple updates on 
   the state into a single update.
 - **Application** that takes a state and an update and applies the update
   on the state, producing new state as the result.
   
In more formal terms, the type of updates needs to be a monoid (with unit and composition).
In the paper, the two types (sets) together with the operations are called _act_ and are
defined as $(S, (P,\circ,\oplus), \downarrow)$ where $\circ$ is the unit, $\oplus$ is 
composition and $\downarrow$ is the application.

> **Note on naming**
>
> In the last case, I'm using different naming than the original paper. In the paper, applying
> update $u$ to a state $s$ is written as $s \downarrow u$. You can see the "$\downarrow u$" part 
> as an _action_ that transforms state and so the authors use the name _action_. I'm going to use 
> _apply_ instead because I refer more to the operation $\downarrow$ than to the entire 
> (partially-applied action).

Implementing update monads in F#
--------------------------------

To implement this idea in F#, we can use [static member constraints][smc]. If you do not
know about static member constrains in F#, you can think of it as a form of duck typing 
(or lightweight type classes). If a type defines certain members, then the code will 
compile and run fine, otherwise you'll get a compile-time error. We will require
the user to define two types representing `State` and `Update`, respectively. The `Update`
type will need to define the three operations. An abstract definition (not valid F# code)
would look like this:

<pre>
<span class="k">type</span> <span class="i">State</span>
<span class="k">type</span> <span class="i">Update</span> <span class="o">=</span>
  <span class="k">static</span> <span class="i">Unit</span>    <span class="o">:</span> <span class="i">Update</span>
  <span class="k">static</span> <span class="i">Combine</span> <span class="o">:</span> <span class="i">Update</span> <span class="o">*</span> <span class="i">Update</span> <span class="o">-&gt;</span> <span class="i">Update</span>
  <span class="k">static</span> <span class="i">Apply</span>   <span class="o">:</span> <span class="i">State</span> <span class="o">*</span> <span class="i">Update</span> <span class="o">-&gt;</span> <span class="i">State</span>
</pre>

Invocation of members via static member constraints is not entirely easy (the feature is 
used mainly by library implementors for things like generic numerical code). But the idea
is that we can define inline functions (`unit`, `++` and `apply`) that invoke the 
corresponding operation on the type (specified either explicitly or via type inference).

If you're not familiar with F#, you can freely skip over this definition, just remember
that we now have functions `unit` and `apply` and an operator `++`:
*)

/// Returns the value of 'Unit' property on the ^S type
let inline unit< ^S when ^S : 
    (static member Unit : ^S)> () : ^S =
  (^S : (static member Unit : ^S) ()) 

/// Invokes Combine operation on a pair of ^S values
let inline (++)< ^S when ^S : 
    (static member Combine : ^S * ^S -> ^S )> a b : ^S =
  (^S : (static member Combine : ^S * ^S -> ^S) (a, b)) 

/// Invokes Apply operation on state and update ^S * ^U
let inline apply< ^S, ^U when ^U : 
    (static member Apply : ^S * ^U -> ^S )> s a : ^S =
  (^U : (static member Apply : ^S * ^U -> ^S) (s, a)) 

(**
The last thing that we need to do before we can start playing with some
update monads is to implement the monadic operators. In F#, this is done by
defining a _computation builder_ - a type that has `Bind` and `Return` 
operations (as well as some others that we'll see later). The compiler then
automatically translates a block `update { .. }` using operations `update.Return`
and `update.Bind`.

The computation builder is a normal class with members. Because we are using 
static member constraints and inline functions, we need to mark the members
as `inline` too:

*)
type UpdateBuilder() = 
  /// Returns the specified value, together
  /// with empty update obtained using 'unit'
  member inline x.Return(v) : UpdateMonad<'S, 'U, 'T> = 
    UM (fun s -> (unit(),v))

  /// Compose two update monad computations
  member inline x.Bind(UM u1, f:'T -> UpdateMonad<'S, 'U, 'R>) =  
    UM (fun s -> 
      // Run the first computation to get first update
      // 'u1', then run 'f' to get second computation
      let (u1, x) = u1 s
      let (UM u2) = f x
      // Apply 'u1' to original state & run second computation
      // then return result with combined state updates
      let (u2, y) = u2 (apply s u1)
      (u1 ++ u2, y))

/// Instance of the computation builder
/// that defines the update { .. } block
let update = UpdateBuilder()

(**
The implementation of the `Return` operation is quite simple - we return
the specified value and call `unit()` to get the unit of the monoid of 
updates - as a result, we get a computation that returns the value without
performing any update on the state.

The `Bind` member is more interesting - it runs the first computation which
returns a value `x` and an update `u1`. The second computation needs to be
run in an updated state and so we use `apply s u1` to calculate a new state
that reflects the update. After running the second computation, we get the
final resulting value `y` and a second update `u2`. The result of the 
computation combines the two updates using `u1 ++ u2`. 

How does this actually work? Let's start by looking at reader and writer
monads (which are special cases of the update monad.

Implementing the reader monad
-----------------------------

The reader monad keeps some state, but it does not give us any way of modifying it.
In terms of update monads, this means that there is some state, but the monoid of
updates is trivial - in principle, we can just use `unit` as the type of updates.
You can see that when looking at the type too - the type of reader monad is
`'TState -> 'T`. To get a type with a structure matching to update monads, we
can use an equivalent type `'TState -> unit * 'T`.

### Reader state and update

In practice, we still need to define a type for updates, so that we can provide the
required static members. We use a single-case discriminated union with just one value
`NoUpdate`:
*)
/// The state of the reader is 'int'
type ReaderState = int
/// Trivial monoid of updates 
type ReaderUpdate = 
  | NoUpdate
  static member Unit = NoUpdate
  static member Combine(NoUpdate, NoUpdate) = NoUpdate
  static member Apply(s, NoUpdate) = s
(**
None of the operations on the `ReaderUpdate` type does anything interesting.
Both unit and combine simply returns the only possible value and the apply
operation returns the state without a change.

### Reader monad primitives

Next, we need a primitive that allows us to read the state and a run operation
that executes a computation implemented using the reader monad (given the
value of the read-only state). The operations look as follows:

*)
/// Read the current state (int) and return it as 'int'
let read = UM (fun (s:ReaderState) -> 
  (NoUpdate, s))
/// Run computation and return the result 
let readRun (s:ReaderState) (UM f) = f s |> snd
(**
When you look at the type of computations (hover the mouse pointer over the
`read` identifier), you can see a parameterized update monad type. The `read`
primitive has a type `UpdateMonad<ReaderState, ReaderUpdate, ReaderState>`. This
means that we have an update monad that uses `ReaderState` and `ReaderUpdate` as
the _act_ (specifying the computation details) and, when executed, produces a 
value of `ReaderState`.

### Sample reader computations

Now we can use the `update { .. }` block together with the `read` primitive
to write computations that can read an immutable state. The following 
basic example reads the state and adds one (in `demo1`), and then adds 
1 again in `demo2`:

*)
/// Returns state + 1
let demo1 = update { 
  let! v = read
  return v + 1 }
/// Returns the result of demo1 + 1
let demo2 = update { 
  let! v = demo1
  return v + 1 }

// Run it with state 40 
demo2 |> readRun 40

(**
If you run the code, you'll see that the result is 42. The interesting thing
about this approach is that we only had to define two types. The `update { .. }`
computation works for all update monads and so we get the computation builder
"for free". However, thanks to the parameterization, the computation really represents
an immutable state - there is no way to mutate it.

Implementing the writer monad
-----------------------------

Similarly to the reader monad, the writer monad is just a simple special case of the
update monad. This time, the _state_ is trivial and all the interesting things are
happening in the _updates_. The type of the usual writer monad is `'TState * 'T` and
so if we want to make this a special case of update monad, we can define the type
as `unit -> 'TState * 'T`.

### Writer state and update

The state needs to be a monoid (with unit and composition) so that we can compose
the states of multiple sub-computations. The following example uses a list as a
concrete example. We define a (writer) monad that keeps a list of `'TLog` values
and returns that as the result (more generally, we could use an arbitrary monoid
instead of a list):

*)
/// Writer monad has no readable state
type WriterState = NoState

/// Updates of writer monad form a list
type WriterUpdate<'TLog> = 
  | Log of list<'TLog>
  /// Returns the empty log (monoid unit)
  static member Unit = Log []
  /// Combines two logs (operation of the monoid)
  static member Combine(Log a, Log b) = Log(List.append a b)
  /// Applying updates to state does not affect the state
  static member Apply(NoState, _) = NoState
(**

The writer monad appears (in some informal sense) dual to the earlier reader monad.
The state (that can be read) is always empty and is represented by the `NoState` value,
while all the interesting aspects are captured by the `WriterUpdate` type - which is a
list of values produced by the computation. The updates of a writer monad have to form 
a monoid - here, we use a list that concatenates all logged values. You could easily
change the definition to implement other monoids (e.g. to keep the last produced value).


### Writer monad primitives

Similarly to the previous example, we now need two primitives - one to add a new element 
to the log (`write` of the writer monad) and one to run a computation and extract the
result and the log:
*)
/// Writes the specified value to the log 
let write v = UM (fun s -> (Log [v], ()))
/// Runs a "writer monad computation" and returns 
/// the log, together with the final result
let writeRun (UM f) = let (Log l, v) = f NoState in l, v
(**
The `write` function creates a singleton list containing the specified value `Log [v]` as
the update and returns the unit value as the result of the computation. When combined with
other computations, the updates are concatenated and so this will become a part of the
list `l` in the result `(Log l, v)` that is made accessible in the `writerRun` function.

### Sample writer computations

Let's have a look at a sample computation using the new definitions - the remarkable thing
(from the practical F# programming perspective) is that we wrap the computation
in the `update { .. }` block just like in the previous example. But this time, we'll use
the `write` primitive to write 20 and then 10 to the log and the F# compiler correctly
infers that we are using `WriterState` and `WriterUpdate` types:

*)
/// Writes '20' to the log and returns "world"
let demo3 = update {
  do! write 20
  return "world" }
/// Calls 'demo3' and then writes 10 to the log
let demo4 = update {
  let! w = demo3
  do! write 10
  return "Hello " + w }

/// Returns "Hello world" with 20 and 10 in the log
demo4 |> writeRun

(**
If you run the code, the `demo3` computation first writes 20 to the log,
which is then combined (using the `++` operator that invokes `WriterUpdate.Combine`)
with the value 10 written in `demo4`.

Building richer computations
----------------------------

One of the key things about F# computation expressions that I emphasized in my 
[previous blog post][zoo] and in the [PADL 2014 paper][zoopaper] is that computation
expressions provide rich syntax that includes resource management (the `use` keyword),
exception handling or loops (`for` and `while`) - in simple words, it mirrors the
normal syntax of F#. 

So far, we have not used any of these for update monads. All these additional constructs
have to be provided in the computation builder (so that the author can define them in
the most suitable way). The great thing about update monads (for F#) is that we have just
a single computation builder and so we can define a number of additional operations to
enable richer syntax.

The following snippet extends `UpdateBuilder` defined earlier with more operations. If you're
not interested in the details, feel free to skip to the next section. The key idea is
that this only has to be written once!
*)

/// Extends UpdateBuilder to support additional syntax
type UpdateBuilder with
  /// Represents monadic computation that returns unit
  /// (e.g. we can now omit 'else' branch in 'if' computation)
  member inline x.Zero() = x.Return(())

  /// Delays a computation with (uncontrolled) side effects
  member inline x.Delay(f) = x.Bind(x.Zero(), f)

  /// Sequential composition of two computations where the
  /// first one has no result (returns a unit value)
  member inline x.Combine(c1, c2) = x.Bind(c1, fun () -> c2)

  /// Enable the 'return!' keyword to return another computation
  member inline x.ReturnFrom(m : UpdateMonad<'S, 'P, 'T>) = m

  /// Ensure that resource 'r' is disposed of at the end of the
  /// computation specified by the function 'f'
  member inline x.Using(r,f) = UM(fun s -> 
    use rr = r in let (UM g) = f rr in g s)

  /// Support 'for' loop - runs body 'f' for each element in 'sq'
  member inline x.For(sq:seq<'V>, f:'V -> UpdateMonad<'S, 'P, unit>) = 
    let rec loop (en:System.Collections.Generic.IEnumerator<_>) = 
      if en.MoveNext() then x.Bind(f en.Current, fun _ -> loop en)
      else x.Zero()
    x.Using(sq.GetEnumerator(), loop)

  /// Supports 'while' loop - run body 'f' until condition 't' holds
  member inline x.While(t, f:unit -> UpdateMonad<'S, 'P, unit>) =
    let rec loop () = 
      if t() then x.Bind(f(), loop)
      else x.Zero()
    loop()

(**
You can find more details about these operations in the [F# Computation Zoo paper][zoopaper]
or in the [F# language specification][fsspec]. In fact, the definitions mostly follow
the samples from the F# specification. It is worth noting that all the members are
marked as `inline`, which allows us to use _static member constrains_ and to write
code that will work for any update monad (as defined by a pair of _update_ and 
_state_ types).

Let's look at a trivial example using the writer computation:
*)
/// Logs numbers from 1 to 10
let logNumbers = update {
  for i in 1 .. 10 do 
    do! write i }
(**
As expected, when we run the computation using `writeRun`, the result is a tuple containing
a list with numbers from 1 to 10 and a unit value. The computation does not explicitly 
return and so the `Zero` member is automatically used.

Implementing the state monad
----------------------------

Interestingly, the standard state monad is _not_ a special case of update monads. However, we
can define a computation that implements the same functionality - a computation with state
that we can read and write.

### States and updates

In this final example, both the type representing _state_ and the type representing
_update_ will have a useful role. We make both of the types generic over the value they
carry. State is simply a wrapper containing the value (current state). Update can be of
two kinds - we have an empty update (do nothing) and an update to set the state:

*)
/// Wraps a state of type 'T
type StateState<'T> = State of 'T

/// Represents updates on state of type 'T
type StateUpdate<'T> = 
  | Set of 'T | SetNop
  /// Empty update - do not change the state
  static member Unit = SetNop
  /// Combine updates - return the latest (rightmost) 'Set' update
  static member Combine(a, b) = 
    match a, b with 
    | SetNop, v | v, SetNop -> v 
    | Set a, Set b -> Set b
  /// Apply update to a state - the 'Set' update changes the state
  static member Apply(s, p) = 
    match p with SetNop -> s | Set s -> State s
(**
This definition is a bit more interesting than the previous two, because there is some
interaction between the _states_ and _updates_. In particular, when the update is `Set v`
(we want to replace the current state with a new one), the `Apply` member returns a new
state instead of the original. For the `Unit` member, we need an update `SetNop` which 
simply means that we want to keep the original state (and so `Apply` just returns the
original value in this case).

Another notable thing is the `Combine` operation - it takes two updates (which may be 
either empty updates or set updates) and produces a single one. If you read a composition
`a1 ++ a2 ++ .. ++ an` as a sequence of state updates (either `Set` or `SetNop`), then the 
`Combine` operation returns the last `Set` update in the sequence (or `SetNop` if there are
no `Set` updates). In other words, it builds an update that sets the last state that was
set during the whole sequence.

### State monad primitives

Now that we have the type definitions, it is quite easy to add the usual primitives:
*)
/// Set the state to the specified value
let set s = UM (fun _ -> (Set s,()))
/// Get the current state 
let get = UM (fun (State s) -> (SetNop, s))
/// Run a computation using a specified initial state
let setRun s (UM f) = f (State s) |> snd
(**
The `set` operation is a bit different than the usual one for state monad. It ignores the
state and it builds an _update_ that tells the computation to set the new state. 
The `get` operation reads the state and returns it - but as it does not intend to change it,
it returns `SetNop` as the update.

### Sample stateful computation

If you made it this far in the article, you can already expect how the example will look!
We'll again use the `update { .. }` computation. This time, we define a computation
`demo5` that increments the state and call it from a loop in `demo6`:
*)
/// Increments the state by one
let demo5 = update { 
  let! v = get
  do! set (v + 1) }
/// Call 'demo5' repeatedly in a loop
/// and then return the final state
let demo6 = update {
  for i in 1 .. 10 do 
    do! demo5
  return! get }
// Run the sample with initial state 0
demo6 |> setRun 0
(**
Running the code yields 10 as expected - we start with zero and then increment the
state ten times. Since we extended the definition of the `UpdateBuilder` (in the 
previous section), we now got a few nice things for free - we can use the `for` loop
and write computations (like `demo5`) without explicit `return` if they just need to
modify the state.

Conclusions
-----------

People coming to F# from the Haskell background often dislike the fact that
F# does not let you write code polymorphic over monads and that computation
expressions always explicitly state the type of computations such as 
`async { .. }`. I think there are good reasons for this and tried to explain some
of them in [a recent blog post and PADL paper][zoo].

As a result, using reader, writer and state monads in F# was always a bit 
cumbersome. In this blog post, I looked at an F# implementation of the recent
idea called _update monads_ (see [the original paper (PDF)][um]), which unifies
the three state-related monads into a single type. This works very nicely with F#
- we can define just a single computation builder for all state-related computations
and then define a concrete state-related monad by defining two simple types.
I used the approach to define a reader monad, writer monad useful for logging and
a state monad (that keeps a state and allows changing it).

I guess that making update monads part of standard library and standard programming
style in Haskell will be tricky because of historical reasons. However, for F#
libraries that try to make purely functional programming easier, I think that 
update monads are the way to go.


  [zoo]: http://tomasp.net/blog/2013/computation-zoo-padl
  [zoopaper]: http://tomasp.net/academic/papers/computation-zoo/
  [um]: http://cs.ioc.ee/~tarmo/papers/types13.pdf
  [smc]: http://msdn.microsoft.com/en-us/library/dd233203.aspx
  [fsspec]: http://fsharp.org/about/index.html#specification
*)
