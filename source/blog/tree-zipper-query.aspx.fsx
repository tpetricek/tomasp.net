(**
Processing trees with F# zipper computation
===========================================

 - date: 2012-12-19T14:22:47.0000000
 - description: One of the less frequently advertised new features in F# 3.0 is the query syntax. It allows adding custom operations to a computation expression block. This article shows how to define a custom computation for processing trees using zippers. We'll add navigation over a tree as custom operations to get a simple syntax.
 - layout: post
 - tags: f#,haskell,research,monads,linq
 - title: Processing trees with F# zipper computation
 - url: tree-zipper-query.aspx

--------------------------------------------------------------------------------
 - standalone

One of the less frequently advertised new features in F# 3.0 is the _query syntax_.
It is an extension that makes it possible to add custom operations in an F#
computation expression. The standard `query { .. }` computation uses this to define
operations such as sorting (`sortBy` and `sortByDescending`) or operations for taking
and skipping elements (`take`, `takeWhile`, ...). For example, you can write:
*)

query { for x in 1 .. 10 do
        take 3
        sortByDescending x }

(**
In this article I'll use the same notation for processing trees using the _zipper_
pattern. I'll show how to define a computation that allows you to traverse a tree
and perform transformations on (parts) of the tree. For example, we'll be able to 
say "Go to the left sub-tree, multiply all values by 2. Then go back and to the
right sub-tree and divide all values by 2" as follows:
*)

(*** include:final-example ***)

(**
This example behaves quite differently to the usual `query` computation. It mostly
relies on custom operations like `left`, `right` and `up` that allow us to navigate
through a tree (descend along the left or right sub-tree, go back to the parent node). 
The only operation that _does something_ is the `map` operation which transforms the
current sub-tree.
*)
(*** hide ***)
type Tree<'T> = 
  | Node of Tree<'T> * Tree<'T>
  | Leaf of 'T
  override x.ToString() = (*[omit:(...)]*)
    match x with
    | Node(l, r) -> sprintf "(%O, %O)" l r
    | Leaf v -> sprintf "%O" v(*[/omit]*)

type Path<'T> = 
  | Top 
  | Left of Path<'T> * Tree<'T>
  | Right of Path<'T> * Tree<'T>
  override x.ToString() = (*[omit:(...)]*)
    match x with
    | Top -> "T"
    | Left(p, t) -> sprintf "L(%O, %O)" p t
    | Right(p, t) -> sprintf "R(%O, %O)" p t(*[/omit]*)

type TreeZipper<'T> = 
  | TZ of Tree<'T> * Path<'T>
  override x.ToString() = (*[omit:(...)]*)
    let (TZ(t, p)) = x in sprintf "%O [%O]" t p(*[/omit]*)

/// Navigates to the left sub-tree
let left = function
  | TZ(Leaf _, _) -> failwith "cannot go left"
  | TZ(Node(l, r), p) -> TZ(l, Left(p, r))

/// Navigates to the right sub-tree
let right = function
  | TZ(Leaf _, _) -> failwith "cannot go right"
  | TZ(Node(l, r), p) -> TZ(r, Right(p, l))

/// Gets the value at the current position
let current = function
  | TZ(Leaf x, _) -> x
  | _ -> failwith "cannot get current"

// Navigate to the parent node
let up = function
  | TZ(l, Left(p, r))
  | TZ(r, Right(p, l)) -> TZ(Node(l, r), p)
  | TZ(_, Top) -> failwith "cannot go up"

// Navigate to the root of the tree
let rec top = function
  | TZ(_, Top) as t -> t
  | tz -> top (up tz)

/// Build tree zipper with singleton tree
let unit v = TZ(Leaf v, Top)


/// Transform leaves in the current sub-tree of 'treeZip'
/// into other trees using the provided function 'f'
let bindSub f treeZip = 
  let rec bindT = function
    | Leaf x -> let (TZ(t, _)) = top (f x) in t
    | Node(l, r) -> Node(bindT l, bindT r)
  let (TZ(current, path)) = treeZip
  TZ(bindT current, path)

type TreeZipperBuilder() = 
  /// Enables the 'for x in xs do ..' syntax
  member x.For(tz:TreeZipper<'T>, f) : TreeZipper<'T> = bindSub f tz
  /// Enables the 'yield x' syntax
  member x.Yield(v) = unit v

/// Global instance of the computation builder
let tree = TreeZipperBuilder()

type TreeZipperBuilder with
  // Operations for navigation through the tree
  [<CustomOperation("left", MaintainsVariableSpace=true)>]
  member x.Left(tz) = left tz
  [<CustomOperation("right", MaintainsVariableSpace=true)>]
  member x.Right(tz) = right tz
  [<CustomOperation("up", MaintainsVariableSpace=true)>]
  member x.Up(tz) = up tz
  [<CustomOperation("top", MaintainsVariableSpace=true)>]
  member x.Top(tz) = top tz

  /// Extracts the current value and returns it
  [<CustomOperation("current", MaintainsVariableSpace=false)>]
  member x.Current(tz) = current tz

  /// Transform the current sub-tree using 'f'
  [<CustomOperation("map", MaintainsVariableSpace=true)>]
  member x.Select(tz, [<ProjectionParameter>] f) = bindSub (f >> unit) tz


(*** define:final-example ***)
tree { for x in sample do
       left 
       map (x * 2) 
       up
       right
       map (x / 2) 
       top }
(**
This was just a brief introduction to what is possible, so let's take a detailed look
at how this works...

--------------------------------------------------------------------------------


One of the less frequently advertised new features in F# 3.0 is the _query syntax_.
It is an extension that makes it possible to add custom operations in an F#
computation expression. The standard `query { .. }` computation uses this to define
operations such as sorting (`sortBy` and `sortByDescending`) or operations for taking
and skipping elements (`take`, `takeWhile`, ...). For example, you can write:
*)

query { for x in 1 .. 10 do
        take 3
        sortByDescending x }

(**
In this article I'll use the same notation for processing trees using the _zipper_
pattern. I'll show how to define a computation that allows you to traverse a tree
and perform transformations on (parts) of the tree. For example, we'll be able to 
say "Go to the left sub-tree, multiply all values by 2. Then go back and to the
right sub-tree and divide all values by 2" as follows:
*)

(*** include:final-example ***)

(**
This example behaves quite differently to the usual `query` computation. It mostly
relies on custom operations like `left`, `right` and `up` that allow us to navigate
through a tree (descend along the left or right sub-tree, go back to the parent node). 
The only operation that _does something_ is the `map` operation which transforms the
current sub-tree.

This was just a brief introduction to what is possible, so let's take a detailed look
at how this works...

## Trees and zippers

First of all, we need to define a data type that represents the tree. This is going
to be a standard binary tree with values in leafs (but you could do the similar
thing for other kinds of trees or structures):
*)

type Tree<'T> = 
  | Node of Tree<'T> * Tree<'T>
  | Leaf of 'T
  override x.ToString() = (*[omit:(...)]*)
    match x with
    | Node(l, r) -> sprintf "(%O, %O)" l r
    | Leaf v -> sprintf "%O" v(*[/omit]*)

(**
Next, we need to look at the concept of a _zipper_. Intuitively, a zipper is something
that allows you to navigate through the tree. When you go the left sub-tree, we want
to get a value that contains this sub-tree together with the path that describes how
we got there (and contains remaining branches that are needed to reconstruct the original
tree when we go back up).

The [paper that introduced Zippers][zipper] is very readable and introduces the idea
in more details. As a fun fact, you can also read about how zippers can be derived
[automatically using _differentiation_][diff].
In our case, the zipper data type for the tree is defined as follows:

  [zipper]: http://www.st.cs.uni-saarland.de/edu/seminare/2005/advanced-fp/docs/huet-zipper.pdf "G. Huet: The Zipper (Journal of Functional Programming)"
  [diff]: http://strictlypositive.org/diff.pdf "C. McBride: The Derivative of a Regular Type is its Type of One-Hole Contexts"

*)
    
type Path<'T> = 
  | Top 
  | Left of Path<'T> * Tree<'T>
  | Right of Path<'T> * Tree<'T>
  override x.ToString() = (*[omit:(...)]*)
    match x with
    | Top -> "T"
    | Left(p, t) -> sprintf "L(%O, %O)" p t
    | Right(p, t) -> sprintf "R(%O, %O)" p t(*[/omit]*)

type TreeZipper<'T> = 
  | TZ of Tree<'T> * Path<'T>
  override x.ToString() = (*[omit:(...)]*)
    let (TZ(t, p)) = x in sprintf "%O [%O]" t p(*[/omit]*)

(**

The `TreeZipper<'T>` type pairs a tree (the current sub-tree that we are focused at)
with a path which describes "how we got there". If we are at the root, then the path
is `Top`. If we descend to the left-sub tree, then the path is represented using 
`Left(path, tree)` where `path` is the previous path and `tree` is the tree on the
right (that we just skipped over).

## Working with zippers

Let's look at three simple functions that operate on the tree zipper and then 
we'll navigate through a simple tree using them:
*)

/// Navigates to the left sub-tree
let left = function
  | TZ(Leaf _, _) -> failwith "cannot go left"
  | TZ(Node(l, r), p) -> TZ(l, Left(p, r))

/// Navigates to the right sub-tree
let right = function
  | TZ(Leaf _, _) -> failwith "cannot go right"
  | TZ(Node(l, r), p) -> TZ(r, Right(p, l))

/// Gets the value at the current position
let current = function
  | TZ(Leaf x, _) -> x
  | _ -> failwith "cannot get current"

(**
The `left` and `right` functions are symmetric. If the `TreeZipper<'T>` represents a 
position where the current tree is `Leaf` then they fail, because we cannot descend to
a sub-tree of a leaf. If the tree is a `Node` then they pick the left (or right) sub-tree
and make it a current tree by using it as the first argument of `TZ`. The path is
constructed by appending the other tree (`r` or `l`, respectively) to the previous
path using `Left` or `Right`.

On the other hand, the `current` operation only works when the zipper points at 
a leaf node. In that case, it simply returns the value from the leaf.
The following example defines a simple tree and then uses zipper operations to
get to one of the leaves in the tree:
*)

// Create a sample tree
let branches = 
  Node( Node(Leaf 1, Leaf 3), 
        Node(Leaf 7, Node(Leaf 12, Leaf 20)) )

// Wrap it as a zipper & print
let sample = TZ(branches, Top)
printfn "%O" sample 

// Get one of the tree leaves
sample |> right |> right |> left |> current

(**
To make this example complete, we also need to add operation `up` that goes to the
parent. We will also need an operation that (recursively) goes back to the top of the tree:
*)

// Navigate to the parent node
let up = function
  | TZ(l, Left(p, r))
  | TZ(r, Right(p, l)) -> TZ(Node(l, r), p)
  | TZ(_, Top) -> failwith "cannot go up"

// Navigate to the root of the tree
let rec top = function
  | TZ(_, Top) as t -> t
  | tz -> top (up tz)

(**
The `up` operation fails when we are already in the root and the path is just `Top`.
Otherwise, it gets the current tree and takes the other branch from the path value.
These two trees are combined into a new `Node` which is then returned as the current
tree. 

### Adding computational operations

In order to define an F# computation expression, we need to define the meaning of
`for` and `yield`. Most often, these operations are implemented by monadic _bind_
and _unit_. We will not follow this pattern exactly. If we did that, we wouldn't be
able to implement the motivating example as nicely.

The operations that we need to define are `unit`, which
simply creates a tree containing just a leaf, and `bindSub` which transforms
all leaves in the current sub-tree using a specified function:
*)

/// Build tree zipper with singleton tree
let unit v = TZ(Leaf v, Top)

/// Transform leaves in the current sub-tree of 'treeZip'
/// into other trees using the provided function 'f'
let bindSub f treeZip = 
  let rec bindT = function
    | Leaf x -> let (TZ(t, _)) = top (f x) in t
    | Node(l, r) -> Node(bindT l, bindT r)
  let (TZ(current, path)) = treeZip
  TZ(bindT current, path)

(**
The type of `bindSub` is `('T -> TreeZipper<'T>) -> TreeZipper<'T> -> TreeZipper<'T>`,
which is almost like monadic _bind_ with the exception that the transformation function
needs to transform elements of type `'T` into trees containing leaves of _the same_ type.

This requiremenet follows from the fact that we run the transformation only on the
current tree (`bindT current`) while the `path` is left unchanged (and since path contains
other trees of the same type, the type needs to stay the same). You could implement
`bind` that applies `f` to trees in the path, but that would not be as interesting
(because all transformations like `map` would happen on the whole tree - not just
the current sub-tree).

## Defining the Computation builder

### Providing monadic operations

Now we have everything we need to define an F# computation builder for working with 
tree zippers. We start with a simple definition that allows just `for` and `yield` 
and then add all the (interesting) custom operators. Our `yield` is defined as `unit`
and `for` corresponds to `bindSub`:

*)

type TreeZipperBuilder() = 
  /// Enables the 'for x in xs do ..' syntax
  member x.For(tz:TreeZipper<'T>, f) : TreeZipper<'T> = bindSub f tz
  /// Enables the 'yield x' syntax
  member x.Yield(v) = unit v

/// Global instance of the computation builder
let tree = TreeZipperBuilder()

(**
Equipped with the above definition, we can write computations that transform the
entire tree. For example, to multiply all values of the tree by 2, we can write
the following computation (you can pass the result to `printfn "%O"` to get a nicer
output)
*)

tree { for x in sample do
       yield x * 2 }

(**
As discussed earlier, the `bindSub` operation (and thus our `for`) only allows the
result to have the same type as the input. Writing, for example, `x.ToString()`, would
be invalid. In this case, it does not seem very logical, but that's because we have not
added operations for navigating through the tree - the `for` and `yield` can be
only used to transform the entire tree.

### Adding custom operations

Now we're getting to the most interesting bit of the article. How do we add custom 
operations to the computation expression? To do that, we need to annotate the method
with `CustomOperation` attribute. The attribute defines the name of the operation
and can specify additional parameters.

The fact that makes custom operations interesting is that they can have types such 
as `TreeZipper<'T> -> TreeZipper<'T>` which is exactly the type of our navigational
operations. The syntax allows other operations - such as zipping, grouping and
joinging - but we will not need these in this article.

To add navigational operations, transformations and getter for the current element,
we can write the following code:

*)
type TreeZipperBuilder with
  // Operations for navigation through the tree
  [<CustomOperation("left", MaintainsVariableSpace=true)>]
  member x.Left(tz) = left tz
  [<CustomOperation("right", MaintainsVariableSpace=true)>]
  member x.Right(tz) = right tz
  [<CustomOperation("up", MaintainsVariableSpace=true)>]
  member x.Up(tz) = up tz
  [<CustomOperation("top", MaintainsVariableSpace=true)>]
  member x.Top(tz) = top tz

  /// Extracts the current value and returns it
  [<CustomOperation("current", MaintainsVariableSpace=false)>]
  member x.Current(tz) = current tz

  /// Transform the current sub-tree using 'f'
  [<CustomOperation("map", MaintainsVariableSpace=true)>]
  member x.Select(tz, [<ProjectionParameter>] f) = bindSub (f >> unit) tz

(**
The `MaintainsVariableSpace` parameter allows us to specify that an operation transforms
values in the abstract data type without touching them. If you have an operation
`M<'T> -> M<'T>` then it is generally `true`, but if you have an operation such as
`M<'T> -> M<int>` then the variable space would not be preserved (because variables
are stored in a tuple used in place of `'T`, so turning that to `int` loses the 
values in the tuple). In our case, navigation and transformation maintains the
variable space, but `current` operation does not.

The `map` operation uses another interesting attribute. If we annotate a function
argument with `ProjectionParameter` then we can write `map (x * 2)` and the argument
expression `x * 2` is implicitly turned into a function that takes all the variables
and returns the result. Something like `fun vars -> vars.x * 2`.

### Sample tree computations

Let's finish the article with two examples that process the `sample` tree defined 
earlier. The first one corresponds to our earlier computation (written using 
pipelines) that picks a specific leaf:
*)

tree { for x in sample do
       right
       right
       left
       current }

(**
The computation expression syntax always has to start with `for .. in .. do`, so we
write that even if we do not actually access the value assigned to `x`. This is 
simply translated to `For` followed by `Yield` (which does not change the tree).

The `for` can then be followed by custom operations. In this example, we move
right, right, left and then get the current value from the tree. Note that adding
`left` after `current` gives a type error, because `current` extracts a value 
(which is not a tree zipper that can be transformed).

We can now also write the sample from the introduction of the article:
*)

tree { for x in sample do
       left
       map (x * 2)
       up
       right
       map (x / 2) 
       top }

(**
Here, we write `for` followed by `left` to navigate to a left sub-tree. Then
we perform a transformation using `map`. Because this is implemented using the
`bindSub` operation, the transformation only affects the current sub-tree, but
not the parents (the root node and its right sub-tree).

This example is more interesting, because it actually uses the variable `x` in
the transformations (written using `map`). The computation expression notation 
actually gives us some benefits over a simple pipeline, because we do not need
to create an explicit lambda each time we write `map`. Thanks to `ProjectionParameter`
attribute, this is done automatically.

## Summary

The main goal of this article is to demonstrate how you can define simple 
_computation expressions_ with _custom operations_ which is a new (and pretty powerful)
feature in F# 3.0. In the example discussed in this article, I used the _tree zipper_.

The custom operations make it possible to navigate through the tree using 
a conventient syntax and perform transformations on current sub-tree just by
writing commands like `left` and `right`. When writing the transformations using
`map (x / 2)` we also do not need to write explicit lambda functions.

This article is really just an introduction - there are many topics that I did 
not talk about, because the custom constructs can also implement operations that
resemble grouping or joining. But I'll leave that for another article!
*)

(*** define:final-example ***)
tree { for x in sample do
       left 
       map (x * 2) 
       up
       right
       map (x / 2) 
       top }
