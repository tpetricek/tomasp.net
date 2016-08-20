(**

Advent Art: Generating Hokusai paintings
========================================

 - date: 2014-01-21T18:49:00.0000000
 - description: This article is a contribution to the F# Advent Calendar that is organized every year by the Japanese F# community - and so the article is inspired by the Japanese culture. We look at a list of Japanese artists in Freebase and try to recreate one piece using fractals and FunScript!
 - layout: post
 - image: http://tomasp.net/blog/2014/japan-advent-art-en/hokusai_sm.jpg
 - tags: f#,art,fractals,funscript,f# data
 - title: Advent Art: Generating Hokusai paintings
 - url: 2014/japan-advent-art-en

--------------------------------------------------------------------------------
 - standalone

<div id="myModal" class="reveal-modal xlarge" data-reveal>
  <iframe src="http://tomasp.net/blog/2014/japan-advent-art-en/hokusai.html" style="width:100%; height:850px;border-style:none;"></iframe>
  <a class="close-reveal-modal">&#215;</a>
</div>

<div class="rdecor" style="text-align:center">
<a href="#" data-reveal-id="myModal" style="text-align:center">
<img src="http://tomasp.net/blog/2014/japan-advent-art-en/hokusai_sm.jpg" style="margin-bottom:10px;border:4px solid black" title="The Great Wave off Kanagawa" /><br />
<small style="font-size:90%">Click here to see the result live!</small>
</a>
</div>

For the last few years, the Japanese F# community has been running the F# Advent Calendar 
([2010](http://atnd.org/events/10685), [2011](http://partake.in/events/1c24993a-c475-4fc2-bca4-7a1cd2f81869), [2012](http://atnd.org/events/33927)). 
Each advent day, one person writes an article about something interesting in F#. I have 
been following the advent calendar last year on Twitter and when the planning started for 
this year, I offered to write an article too. You might have noticed that I posted a 
[Japanse version of the article](http://tomasp.net/blog/2013/japan-advent-art/index.html)
in December as part of the [advent calendar 2013](http://connpass.com/event/3935/).

A number of people helped to make this happen - [@@igeta](http://twitter.com/igeta) arranged everything and 
helped with reviewing, [@@yukitos](http://twitter.com/yukitos) translated the article and 
[@@gab_km](http://twitter.com/gab_km) reviewed the translation. Thanks!

But what should I write about? It would be nice to look at some of the F# open-source libraries
and projects that have been developing over the last year in the F# community. At the same time,
can I relate the topic of the article to Japan? After some thinking, I came up with the following
plan:

 * First, we'll use the [F# Data][fsdata] library and the [Freebase](http://www.freebase.com) to learn
   something about Japanese art. I should add that thanks to [@@yukitos](https://twitter.com/yukitos) the library now also has 
   a [documentation in Japanese][fsdatajp].

 * Then we'll pick one art work and try to recreate it in F#. Given my artistic skills, this 
   part will definitely fail, but we can try :-).

 * Finally, we'll use the [FunScript project](http://funscript.info) to turn our F# code into
   JavaScript, so that we can run it as a pure HTML web application that also works on mobile
   phones and other devices.

--------------------------------------------------------------------------------


<div id="myModal" class="reveal-modal xlarge" data-reveal>
  <iframe src="http://tomasp.net/blog/2014/japan-advent-art-en/hokusai.html" style="width:100%; height:850px;border-style:none;"></iframe>
  <a class="close-reveal-modal">&#215;</a>
</div>

<div class="rdecor" style="text-align:center">
<a href="#" data-reveal-id="myModal" style="text-align:center">
<img src="http://tomasp.net/blog/2014/japan-advent-art-en/hokusai_sm.jpg" style="margin-bottom:10px;border:4px solid black" title="The Great Wave off Kanagawa" /><br />
<small style="font-size:90%">Click here to see the result live!</small>
</a>
</div>

For the last few years, the Japanese F# community has been running the F# Advent Calendar 
([2010](http://atnd.org/events/10685), [2011](http://partake.in/events/1c24993a-c475-4fc2-bca4-7a1cd2f81869), [2012](http://atnd.org/events/33927)). 
Each advent day, one person writes an article about something interesting in F#. I have 
been following the advent calendar last year on Twitter and when the planning started for 
this year, I offered to write an article too. You might have noticed that I posted a 
[Japanese version of the article](http://tomasp.net/blog/2013/japan-advent-art/index.html)
in December as part of the [advent calendar 2013](http://connpass.com/event/3935/).

A number of people helped to make this happen - [@@igeta](http://twitter.com/igeta) arranged everything and 
helped with reviewing, [@@yukitos](http://twitter.com/yukitos) translated the article and 
[@@gab_km](http://twitter.com/gab_km) reviewed the translation. Thanks!

But what should I write about? It would be nice to look at some of the F# open-source libraries
and projects that have been developing over the last year in the F# community. At the same time,
can I relate the topic of the article to Japan? After some thinking, I came up with the following
plan:

 * First, we'll use the [F# Data][fsdata] library and the [Freebase](http://www.freebase.com) to learn
   something about Japanese art. I should add that thanks to [@@yukitos](https://twitter.com/yukitos) the library now also has 
   a [documentation in Japanese][fsdatajp].

 * Then we'll pick one art work and try to recreate it in F#. Given my artistic skills, this 
   part will definitely fail, but we can try :-).

 * Finally, we'll use the [FunScript project](http://funscript.info) to turn our F# code into
   JavaScript, so that we can run it as a pure HTML web application that also works on mobile
   phones and other devices.

[fsdata]: http://fsharp.github.io/FSharp.Data/
[fsdatajp]: http://fsharp.github.io/FSharp.Data/jp

Exploring Japanese art
----------------------

[Freebase](http://www.freebase.com) is an online graph database with schematized information,
mostly based on data from Wikipedia. It contains information about a wide range of topics
including society (governments, celebrities, etc.), sports (different kinds), science (computers,
chemistry, etc.) and also art.

The F# type provider for Freebase in F# Data gives us a way to access all this knowledge directly
from a code editor. To use it, you can install NuGet package `FSharp.Data` and reference the 
assembly as follows: 
*)
#I "../packages/FSharp.Data.1.1.10/lib/net40"
#r "FSharp.Data.dll"

open FSharp.Data
open System.Linq
(**
Aside from `FSharp.Data`, we'll also use some of the LINQ methods to explore the data.
The Freebase can be accessed using `FreebaseData` or using `FreebaseDataProvider` - the
latter one takes an API key which is needed for code that makes larger number of 
requests. To run the complete sample, you'll probably need to register:
*)
type FreebaseData = FreebaseDataProvider<(*[omit:(insert your key)]*)"AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w"(*[/omit]*)>
let fb = FreebaseData.GetDataContext()
(**
Now you can type `fb.` and see a list of areas that are available in Freebase. 
For example, we can look at the "Visual Art" category that contains paintings and
painters, get the first painter in the list (called "Visual Artists") and explore
some information about the painter:
*)
let art = fb.``Arts and Entertainment``.``Visual Art``

let ldv = art.``Visual Artists``.First()
ldv.``Country of nationality``
ldv.Artworks
(**
If you try running the code, you'll see that the first painter returned from the list
is Leonardo da Vinci. The `Country of nationality` property actually returns a list,
but here we get just a single country, which is Italy. We can also get a list of paintings
using the `Artworks` property.

Next, we can write a query that will find all artists with Japanese nationality (to 
make things simpler, we only check the first nationality in the list):
*)
// Query to find all Japanese artists
let artists =
  query { for p in art.``Visual Artists`` do
          where (p.``Country of nationality``.Count() > 0)
          where (p.``Country of nationality``.First().Name = "Japan") }

// Print the names of the artists found
for a in artists do 
  printfn "%s (%s)" a.Name a.``Date of birth``
(**
When you run the snippet, the first artists that are returned are Yoshitaka Amano, Isamu 
Noguchi and Takashi Murakami, all born in the 20th century. We could add `sortBy` or 
`sortByDescending` to specify the order. We can also search for a specified artist
and use `head` to get detailed information about a single artist:
*)
// Find detailed information about Hokusai
let hok = 
  query { for p in art.``Visual Artists`` do
          where (p.Name = "Hokusai")
          head }

// Print image URL, description & list of artworks
printfn "<img src=\"%s\" />" hok.MainImage
printfn "<p>%s</p>" (hok.Blurb.First())
for a in hok.Artworks do
  printfn "<h3>%s</h3><img src=\"%s\" />" a.Name a.MainImage

(**
The second part of the snippet prints different information about Hokusai in HTML format.
The results look as follows:

<img src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/commons_id/447534?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" class="rdecor" />

<blockquote>
<p>
Katsushika Hokusai (葛飾 北斎, September 23, 1760 – May 10, 1849) was a Japanese artist, 
ukiyo-e painter and printmaker of the Edo period. He was influenced by such painters as 
Sesshu, and other styles of Chinese painting. Born in Edo (now Tokyo), Hokusai is best 
known as author of the woodblock print series Thirty-six Views of Mount Fuji (富嶽三十六景, 
Fugaku Sanjūroku-kei, c. 1831) which includes the internationally recognized print, The 
Great Wave off Kanagawa, created during the 1820s. 
</p>
<div class="row">
  <div class="large-3 columns" style="text-align:center">
    <strong>The Dream of the Fisherman's Wife</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/commons_id/120391?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>The Great Wave off Kanagawa</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/commons_id/2646210?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Travellers Crossing the Oi River</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/m/07h65g7?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Red Fuji</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/commons_id/313228?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
</div> 
<div class="row">
  <div class="large-3 columns" style="text-align:center">
    <strong>Feminine Wave</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/m/07h6hz5?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Masculine Wave</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/m/07h6kpr?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Black Fuji</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/m/07h6qjh?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
  <div class="large-3 columns" style="text-align:center">
    <strong>Oceans of Wisdom</strong><br /><img style="margin:10px" src="https://usercontent.googleapis.com/freebase/v1/image/wikipedia/images/en_id/25174350?key=AIzaSyCEgJTvxPjj6zqLKTK06nPYkUooSO1tB0w" />
  </div>
</div> 
</blockquote>

Hokusai and fractals
--------------------

If you look at the paintings, you can see that some of them look like fractals. 
Searching for [Hokusai and fractal on Google images](https://www.google.com/search?q=hokusai+fractal&tbm=isch) 
gives us a number of fractals that are similar to some of the Hokusai's paintings. 

In this article, I'll use the Julia set fractal.
The fractal is quite easy to draw (especially using F#). To make it look similar to 
The Great Wave off Kanagawa, we'll use a carefully chosen color palette to colorize the
fractal.

### Calculating Julia set

You can find more information about [Julia sets on Wikipedia](http://en.wikipedia.org/wiki/Julia_set),
so I will not try to explain how exactly it works - the idea is that we generate a sequence
of complex numbers - at each step we calculate the next value as 
<em>c<sub>next</sub> = c<sub>prev</sub><sup>2</sup> + c</em> where <em>c</em> is some constant.
The iteration can be nicely written as a recursive sequence expression that generates
(potentially) infinite sequence:
*)
open System.Drawing
open System.Numerics

/// Constant that generates nice fractal
let c = Complex(-0.70176, -0.3842)

/// Generates sequence for given coordinates
let iterate x y =
  let rec loop current = seq { 
    yield current
    yield! loop (current ** 2.0 + c) }
  loop (Complex(x, y))
(**
The function takes coordinates (in range from -1 to +1) and generates a sequence for one pixel
on the screen. Next, we need to count the number of iterations until the absolute value of the
number is greater than 2 (or until the maximum number of iterations exceeds a specified limit).
To do that, we can use functions from the `Seq` module:
*)
let countIterations max x y = 
  iterate x y
  |> Seq.take (max - 1)
  |> Seq.takeWhile (fun v -> Complex.Abs(v) < 2.0)
  |> Seq.length
(**
First, we restrict the number of iterations to the specified number, then we keep taking values
with small absolute value and, finally, we get the length of the sequence (when the absolute
value exceeds 2.0, the `takeWhile` function will end the sequence and `length` will be smaller
than `max - 1`). We could write the code in an imperative way - and it would likely be faster - but
this way, it clearly expresses the definition of Julia set. 

### Generating color palette

To draw the fractal, we'll iterate over all pixels where we want to draw, call 
`countInterations` with a specified `x` and `y` arguments and then pick a color from a palette
based on the number of the iterations. To make the fractal that resembles Hokusai's paintings,
we need to carefully generate the palette - the idea is that we'll create a transition between
a number of colors that are used in The Great Wave off Kanagawa.

For this, we'll use a combination of two custom F# operators that let us write `clr1 -- count --> clr2`.
The expression generates a color transition between `clr1` and `clr2` that has `count` number
of steps:
*)
// Transition between colors in 'count' steps
let (--) clr count = clr, count
let (-->) ((r1,g1,b1), count) (r2,g2,b2) = [
  for c in 0 .. count - 1 ->
    let k = float c / float count
    let mid v1 v2 = 
      int (float v1 + ((float v2) - (float v1)) * k) 
    Color.FromArgb(mid r1 r2, mid g1 g2, mid b1 b2) ]
(**
The `--` operator is a simple syntactic trick that just builds a tuple. Because the expression
is parsed as `(clr1 -- count) --> clr2`, the second operator gets both the initial color and
the count as arguments and it can then generate a list with color transitions.

Generating the palette can now be nicely done using array expression with `yield!` to compose
individual transitions. We start with the color of the sky, then generate gradient with different
shades of blue (for water) and then add a number of white shades:
*)
// Palette with colors used by Hokusai
let palette = 
  [| // 3x sky color & transition to light blue
     yield! (245,219,184) --3--> (245,219,184) 
     yield! (245,219,184) --4--> (138,173,179)
     // to dark blue and then medium dark blue
     yield! (138,173,179) --4--> (2,12,74)
     yield! (2,12,74)     --4--> (61,102,130)
     // to wave color, then light blue & back to wave
     yield! (61,102,130)  -- 8--> (249,243,221) 
     yield! (249,243,221) --32--> (138,173,179) 
     yield! (138,173,179) --32--> (61,102,130) |]

(**
### Drawing the fractal

Now we have everything we need to draw the fractal. To keep the first version simple, let's just
use Windows Forms and `Bitmap` from `System.Drawing`. I'll skip over the code that creates the
form (but you can find the [full source code on GitHub](https://github.com/tpetricek/TomaspNet.Website/tree/master/source/blog/2013)).

After we create the form and bitmap, we simply iterate over all pixels of the bitmap,
count the number of iterations (using the palette length as the maximal number) and
then index into the palette to get the color:
*)

(*[omit:(Create form with picture box)]*)
open System.Windows.Forms

let f = new Form(Visible=true, ClientSize=Size(400, 300))
let i = new PictureBox()
i.SizeMode <- PictureBoxSizeMode.Zoom
i.Dock <- DockStyle.Fill
f.Controls.Add(i)(*[/omit]*)

// Specifies what range of the set to draw
let w = -0.4, 0.4
let h = -0.95, -0.35

// Create bitmap that matches the size of the form
let width = float f.ClientSize.Width
let height = float f.ClientSize.Height
let bmp = new Bitmap(f.ClientSize.Width, f.ClientSize.Height)

// For each pixel, transform to the specified range
// and get color using countInterations and palette
for x in 0 .. bmp.Width - 1 do
  for y in 0 .. bmp.Height - 1 do 
    let x' = (float x / width * (snd w - fst w)) + fst w
    let y' = (float y / height * (snd h - fst h)) + fst h
    let it = countIterations palette.Length x' y' 
    bmp.SetPixel(x, y, palette.[it])
i.Image <- bmp
(**
The parameters `w` and `h` are tuples that specify the part of the fractal that we want to
draw. You can change the values to `-2.0, 2.0` and `-1.5, 1.5` to see the entire fractal.
Here, I picked a special part of the fractal to get the following nice picture:

<div style="text-align:center;padding-right:40px">
<img src="compared.jpg" style="margin-left:auto;margin-right:auto;" />
</div>
*)

(*** hide ***)
#r @"..\packages\FunScript\FunScript.TypeScript.Binding.lib.dll"
let palette = [|(0.0,0.0,0.0)|]
let setPixel (img:ImageData) x y width (r, g, b) = ()
let (?) (doc:Document) name :'R = failwith "!"

(**
Adding some Fun(Script)
-----------------------

So, we now have a code that draws the fractal, but you probably want to see it running live,
without creating a new Windows Forms project. Luckily, we can use [FunScript](http://funscript.info/)
which is an F# to JavaScript compiler and we can render our fractal using HTML5 `<canvas>` element.
FunScript already has an example that draws Mandelbrot fractals using HTML5, so this will be
quite easy - but the key thing is, we can just reuse all the code we wrote so far. As I don't have
space to describe all the details, here are some important links:

 * For more information about FunScript, see the [project homepage](http://funscript.info)
 * The [source code for the FunScript version](https://github.com/tpetricek/FunScript/tree/master/Examples/Hokusai)
   is available in my GitHub.
 * You can also have a look at [the live running version of the code](hokusai.html).

Let's start by looking how the rendering function changes. As you can see in the live JavaScript
version, the rendering is a bit slow and so we update the displayed picture after drawing each 
line (since we just copied the original code to dynamically typed JavaScript, the slow down is
expected - we could use imperative style to make the code faster).

However, we can elegantly change the code to update the picture by wrapping the rendering
function in an F# `async` workflow and adding `Async.Sleep` after the drawing of a single
line is finished:
*)
/// Render fractal asynchronously with sleep after every line
let render () = async {
  // Get <canvas> element & create image for drawing
  let canv : HTMLCanvasElement = Globals.document?canvas
  let ctx = canv.getContext_2d()
  let img = ctx.createImageData(float width, float height)
    
  // For each pixel, transform to the specified range
  // and get color using countInterations and palette
  for x in 0 .. int width - 1 do
    for y in 0 .. int height - 1 do 
      let x' = (float x / width * (snd w - fst w)) + fst w
      let y' = (float y / height * (snd h - fst h)) + fst h
      let it = countIterations palette.Length x' y' 
      setPixel img x y width palette.[it]

    // Insert non-blocking waiting & update the fractal
    do! Async.Sleep(1)
    ctx.putImageData(img, 0.0, 0.0) }
(**
The code is mostly the same as previously. Note that FunScript has typed wrappers for most
of the JavaScript libraries, so all variables in the code are statically typed (e.g. `canv`
is `HTMLCanvasElement` and has methods like `getContext_2d` for getting rendering context).
We use a helper function `setPixel` to set a color of a single pixel and a dynamic lookup
operator `?` to get HTML element by ID (both shown in the next paragraph). To start the 
rendering, we need to setup an event handler and call `StartImmediate`:
*)
// Setup button event handler to start the rendering
let go : HTMLButtonElement = Globals.document?go
go.addEventListener_click(fun _ -> 
  render() |> Async.StartImmediate; null)  
(**
The two helper functions that we need are fairly simple too - `setPixel` calculates the offset
in `ImageData` array and sets the R, G and B components (as well as the alpha channel). The 
dynamic operator just calls `getElementById` and casts the returned element to the expected
type:
*)

/// Set pixel value in ImageData to a given color
let setPixel (img:ImageData) x y width (r, g, b) =
  let index = (x + y * int width) * 4
  img.data.[index+0] <- r
  img.data.[index+1] <- g
  img.data.[index+2] <- b
  img.data.[index+3] <- 255.0

/// Dynamic operator that returns HTML element by ID
let (?) (doc:Document) name :'R =  
  doc.getElementById(name) :?> 'R
(**
Conclusions
-----------

First of all, I hope that you had as much fun reading the article as I had writing it and
I wish you a very nice Christmas season!

As I mentioned in the introduction, I wanted to show some interesting libraries that have
been recently developed by the F# community and I wanted to do this using a theme inspired
by Japan. We started by using [F# Data](http://fsharp.github.io/FSharp.Data/) and, more
specifically, the Freebase type provider, to get a list of Japanese visual artists and their
art works.

Then we picked Hokusai and tried to recreate his Great Wave off Kanagawa using Julia set 
with an appropriately chosen palette. Our first version was rendered using Windows Forms.
To make the code runnable directly in a web browser, we used FunScript to translate F# to
JavaScript - and we did not have to do almost any changes in the code. We only added asynchronous
workflows (as a bonus) so that we can see how the rendering of the fractal progresses.

*)
