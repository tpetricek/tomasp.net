Creating interactive You Draw bar chart with Compost
======================================================

 - title: Creating interactive You Draw bar chart with Compost
 - date: 2020-07-16T22:20:16.3109375+01:00
 - description: Compost makes it easy to create custom interactive data visualizations!
    In this blog post, I explained how to use Compost to create an interactive "You Draw"
    bar chart inspired by the awesome interactive line charts from New York Times.
    The example shows the power of Compost including the use of domain-specific values,
    compositionality and the use of Model View Update architecture of interactivity.
 - layout: post
 - references: false
 - image-large: http://tomasp.net/blog/2020/youdraw-compost-visualization/card.png
 - tags: functional, functional programming, data science, thegamma, data journalism, visualization

----------------------------------------------------------------------------------------------------

For a long time, I've been thinking about how to design a data visualization library that would
make it easier to compose charts from simple components. On the one hand, there are charting libraries
like [Google Charts](https://developers.google.com/chart), which offer a long list of pre-defined
charts. On the other hand, there are libraries like [D3.js](https://d3js.org/), which let you
construct any data visualization, but in a very low-level way. There is also [Vega](https://vega.github.io/vega/),
based the idea of _grammar of graphics_, which is somewhere in between, but requires you to
specify charts in a fairly complex language including [a huge number of transformations](https://vega.github.io/vega/docs/transforms/)
that you need to write in JSON.

In the spirit of functional domain specific languages, I wanted to have a small number of
simple but powerful primitives that can be composed by writing code in a normal programming language
like F# or JavaScript, rather than using JSON.

My final motivation for working on this was the [You Draw It article series](https://www.nytimes.com/interactive/2017/01/15/us/politics/you-draw-obama-legacy.html)
by New York Times, which uses interactive charts where the reader first has to make their own guess
before seeing the actual data. I wanted to recreate this, but for bar charts, when working on
[visualizing government spending using The Gamma](http://turing.thegamma.net/expenditure/).

The code for this was somewhat hidden inside The Gamma, but last month, I finally extracted all
the functionality into a new stand-alone library [Compost.js](https://compostjs.github.io/compost/)
with simple and clean [source code on GitHub](https://github.com/compostjs/compost) and
an accompanying [paper draft that describes it (PDF)](https://compostjs.github.io/compost/paper.pdf).

In this article, I will show how to use Compost.js to implement a "You Draw" bar chart inspired
by the NYT article. When loaded, all bars show the average value. You have to drag
the bars to positions that you believe represent the actual values. Once you do this, you can
click "Show me how I did" and the chart will animate to show the actual data, revealing how good
your guess was. Before looking at the code, you can have a look at the resulting interactive chart,
showing the top 5 areas from the 2015 UK budget (in % of GDP):

----------------------------------------------------------------------------------------------------

<style>
.youguess button { margin-top:10px; border-style:solid; border-color:#606060; background:#404040;
  color:#e0e0e0; padding:10px 30px 10px 30px; border-radius:6px; cursor:pointer; font-size:12pt; }
.youguess button:hover, .youguess button:focus, .youguess button:active {
  border-color:#606060;color:#e0e0e0;background:#606060; }
.youguess button:disabled { border-color:#606060; color:#e0e0e0; background:#606060; opacity:0.5; }
</style>
<div id="out1" style="margin:20px auto 40px auto;max-width:600px">
</div>

## Creating a simple bar chart

One of the main principles behind Compost is that you can gradually compose data visualizations.
You can start with a relatively simple version and keep adding features until you have a rich,
customized, interactive visualization. To show this, we'll start by building a simple bar chart.
For this, we'll need our data and a color theme. Compost is a minimalistic library, so you
need to define things like colors yourself. Here, I'm using the `category10` colors from
[Vega](https://vega.github.io/vega/docs/schemes/):

```javascript
var data =
 [ ["Social protection", 14.10],
   ["Health", 7.40], ["Education", 4.50],
   ["General public services", 3.10],
   ["Economic affairs", 2.40] ]

var colors =
  [ "#1f77b4", "#ff7f0e", "#2ca02c", "#d62728",
    "#9467bd", "#8c564b", "#e377c2", "#7f7f7f",
    "#bcbd22", "#17becf" ]
```

When you reference Compost using a stand-alone JavaScript file, it defines a global variable
`c` for accessing its API. If you want more control you can [reference Compost using
npm](https://compostjs.github.io/compost/usage.html).

To create a bar chart, we need to create an array of "bar" shapes and combine those using
`c.overlay`, which automatically aligns shapes. The result of `c.overlay` is a new shape.
To get an axis with labels, we pass the shape to `c.axes`, which adds axes and, again, returns
a new shape:

```javascript
var chart =
  c.axes("left bottom",
    c.overlay(data.map((v, i) =>
      c.fillColor(colors[i], c.bar(v[1], v[0]))
  )))

c.render("out2", chart)
```

<div id="out2" style="text-align:center;max-width:600px;height:260px;margin-left:100px">
</div>

The first interesting thing to note is that the arguments of `c.bar` are not coordinates in
pixels. We just use a JavaScript number as the X value (first argument) and a string as the
Y value (second argument). Compost treats numbers as numerical values and strings as categorical
values. There is a bit more about categorical values, but we will get back to this later.

The second interesting thing is the `c.overlay` operation. This takes an array of shapes that
have coordinates specified in terms of some categorical and continuous values. The operation is
clever enough to align those values and infer a common _x_ and _y_ scale (meaning, a range of
values to be mapped onto an axis). In the above example, the _x_ axis becomes just a range from
_0_ to _14_ whereas the _y_ axis is a categorical axis containing all the 5 different categories.

## Interactive chart state

To create interactive charts, Compost uses the [Model View Update
architecture](https://guide.elm-lang.org/architecture/). We will get to how this works shortly.
For now, we want to construct a nicer version of the bar chart that is less like the simple
bar chart above and more like the one in the full demo.
For this, we will need one aspect from the final chart, which is to create a data structure that
stores all information about the state of the interactive chart.

The following calculates some basic statistics about the data including the average value and
maximum. It then creates an array of objects representing individual bars with their color,
category name, the actual value and the current value as drawn by the user (initialized to be
the average), together with a flag specifying whether the bar has been moved by
the user (once you move a bar, it becomes darker). We also include a randomly generated value,
which is used to show the bars in random order:

```javascript
var nums = data.map(v => v[1])
var sum = nums.reduce((a,b) => a + b)
var max = Math.max.apply(null, nums)
var avg = sum / data.length;

var values =
  data.map((v, i) => ({
    color: colors[i], category: v[0], value: avg,
    moved: false, correct: v[1], rnd: Math.random()
  })).sort((a, b) => a.rnd - b.rnd);

var init =
  { animation:0, guessed:false,
    values:values, max:Math.floor(max * 1.2) }
```

Once we have the array `values`, we use it as part of a value `init` that represents the initial
state of the chart. This contains the individual values and a `max` value that is used as the maximal
possible length of a bar that the user can draw. In addition, there is `animation` which goes from
0 to 1 when the "Show me how I did" button is pressed and `guessed` which becomes `true` when
the user assigns a value to all bars.

## Creating a nicer bar chart

Even without the interactivity, the "You Draw" bar chart that we saw at the start of this article
has a number of fatures that we do not have in the ordinary bar chart above. We will recreate
those before adding the interactivity. The features are:

 - There is a grey background behind every bar (indicating that it can be moved within
   the available space)
 - There is a vertical line that is moved with the bar while the user is guessing, but then
   stays there when the chart shows the actual value (so that the user can see how right or
   wrong they were)
 - Rather than having an axis with labels on the left, we draw the labels directly in the chart.

To create those, we just need to overlay a few more shape than just a single `c.bar`.
The following snippet sets `state` to the `initial` state (later, the state will become a
function parameter). It the defines `drawBar` which composes all the shapes needed to draw
a single bar. Finally, we call `drawBar` for each of the values in `state.values`, overlay
the results, add a bottom axis and draw the resulting shape:

```javascript
var state = init

function drawBar(v, value, fillclr) {
  return c.padding(10,0,10,0,c.overlay([
    c.fillColor("#a0a0a030", c.bar(state.max, v.category)),
    c.fillColor(fillclr, c.bar(value, v.category)),
    c.font("13pt sans-serif", v.color, c.text
      (state.max*0.98, [v.category, 0.5], v.category, "end")),
    c.strokeColor(v.color, c.line([
      [v.value, [v.category, 0]],
      [v.value, [v.category, 1]] ]))
  ]));
}

var chart = c.axes("bottom",
  c.overlay(state.values.map(v =>
    drawBar(v, v.correct, v.color+"90") )))
c.render("out3", chart)
```

<div id="out3" style="text-align:center;width:600px;height:300px;margin:0px auto 0px auto">
</div>

The `drawBar` function creates a list of shapes, overlays them using `c.overlay`
and then adds 10px padding from the top and the bottom. It composes 4 shapes:

 1. a grey bar (the background) with `state.max` as the X value
 2. a bar filled using a provided category color with `value` as the X value
    (in this snippet, we use the actual `v.correct` value; later, this will be the
    guessed value initially and then the correct value after the animation completes)
 3. A text label. This is created using `c.text` which takes X and Y coordinates for
    the label as the first two arguments, the text as the third one and alignment as the fourth
    argument.
 4. A vertical line at the X coordinate specified by `v.value` (the initial, average value).
    The line is defined by two points with one Y value being the top of the space allocated
    for the categorical value and the other being the bottom.

The example shows one more important feature in Compost. When specifying a coordinate on a
numerical axis (X axis), we need just a number as this defines a unique point. However, when
specifying a coordinate on a categorical axis (Y axis), a value such as `"Health"` would
correspond to a whole region allocated for the category. To specify a unique point (as needed
for example for the two ends of the line or for the location of the label), we specify a
pair of values such as `["Health", 0.5]`. The first element identifies the category and the
second a position within the available range. For this reason, the top and the bottom points
of the line are specified by `[v.value, [v.category, 0]]` and `[v.value, [v.category, 1]]`.
Here, `v.value` is the (numerical) X coordinate and `[v.category, 0]` with `[v.category, 1]`
identify the bottom and the top of the bar.

## Model View Update architecture in Compost

Let's now look at what it takes to make the chart interactive! As mentioned earlier,
Compost does this using the [Model View Update architecture](https://guide.elm-lang.org/architecture/).
The idea is that you have a type representing `Event` (different things that can happen
in your application) and a type representing `State` (which stores the current state of the
application). Then you define functions `update` and `view` that look as follows (using a
TypeScript notation for types):

```javascript
function update(state: State, evt: Event) : State
function view(trigger:(evt:Event) => void, state: State) : Html
```

The `update` function takes the current state and an event and produces a new state.
The `view` function takes the current state (the second parameter) and produces an object
that represents the HTML of the page (or the shape of a data visualization).
The first argument of `view` is a function that can be used in event handlers to trigger
events. For example, when the user drags a bar, we will trigger an event to update the
current position of the bar.  

## Handling You Draw chart events

In our You Draw bar chart, there are two kinds of events. The first occurs when the
user drags a bar to a new position. The event is represented as an object with the
`category` to be updated and a new `value`. The second event occurs during the animation
and it simply indicates one "tick" (so it does not carry any other data). Sample
event values are:

```javascript
var one = { kind:"set", category:"Health", value:13 }
var two = { kind:"animate" }
```

In the `update` function, we get the current state (an object that has the same structure
as the `state` value defined earlier) and one of the two kinds of events. We then calculate
a new state and return it as the result. To return a cloned object with some values changed,
we can use the [JavaScript `...` spread operator](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/Spread_syntax):

```javascript
function update(state, evt) {
  switch(evt.kind) {
    case "animate":
      return {...state, animation:state.animation + 0.02}
    case "set":
      var newValues = state.values.map(v => {
        var match = v.category == evt.category;
        var val = match ? evt.value : v.value;
        return { ...v, moved: match || v.moved, value: val } });
      var newGuessed = newValues.every(v => v.moved);
      return {...state, values:newValues, guessed:newGuessed }
  }
}
```

When we get the `animate` event, we simply increment the `animation` field of the `state` by some
small constant. In the `render` function, we will make sure that this is triggered repeatedly using
a timer until the value reaches 1.

When we get the `set` event, we first need to update the value of the corresponding item in the
`state.values` array. This is done using `map`. When we iterate over the value that has the same
category as the one which we want to update, we set the `value` to a new value and we also update
the `moved` flag to indicate that the bar has been updated by the user. We then set the `values`
field to the new array and also update `guessed` which is only `true` if all bars have been `moved`
(meaning that the user made all guesses and can click the "Show me how I did" button).

## Implementing You Draw chart view function

The most interesting part of our You Draw bar chart is the `view` function. We already have the
core logic implemented in the `drawBar` helper, but there are still a few things left. First,
we need to add code to handle mouse events and trigger the `update` function. Second, we need
to add the "Show me how I did" button.

To keep the code more readable, I split it into two parts. In `viewChart`, we render just the
chart itself (without the button). This includes the `drawBar` helper as a nested function.
The most interesting parts are the `handler` helper and the use of `c.on` for registering it
as an event handler for `mousemove` and `mousedown`:

```javascript
function viewChart(trigger, state) {
  // Draw a bar with background and text label
  function drawBar(v, value, fillclr) {
    // (Implementation same as above)
  }
  // Trigger 'set' event when mouse moves
  function handler(x, y, e) {
    if (state.animation == 0 && e.buttons > 0)
    trigger({kind:"set", value:x, category:y[0] })
  }
  // Create a chart with event handlers
  return c.axes("bottom",
    c.on({ mousemove: handler, mousedown: handler },
      c.overlay(state.values.map(v => {
        var val = v.correct * state.animation
          + v.value * (1 - state.animation)
        var clr = v.color + (v.moved?"90":"30");
        return drawBar(v, val, clr)
      })
    )));
}
```

The `c.on` primitive lets us register handlers for events that happen inside the chart.
When a mouse button is pressed or the mouse moves, Compost invokes our `handler`. One
important feature is that the `x` and `y` coordinates passed to `handler` are _not_ in
pixels, but instead in domain terms. If you click in the middle of the bar for `"Health"`
somewhere near 10% GDP value, the `x` and `y` values will be e.g. `10.1` and `["Health", 0.49]`.
This makes it very easy for us to extract the data we want and use `trigger` to invoke the
`"set"` event.

The other interesting thing in the above snippet is the calculation of the `val` value on
line 15. This implements the animation where the value changes from `v.value` (when `state.animation == 0`)
to `v.correct` (when `state.animation == 1`).

Now we just need to call `viewChart` from the main `view` function and add a button:

```javascript
function view(trigger, state) {
  // While animating, keep triggering 'animate' events
  if (state.animation > 0 && state.animation < 1)
    window.setTimeout(() => trigger({kind:"animate"}), 10)

  // Create a chart with event handlers
  var chart = viewChart(trigger, state);

  // Create surrounding HTML elements with a button
  var dis = state.guessed ? {} : {disabled:""}
  return c.html("div", { class:"youguess" }, [
    c.svg(600, 300, chart),
    c.html("div", {style:"text-align:center"}, [
      c.html("button", {...dis,
        click:() => trigger({kind:"animate"}) },
          [ "Show me how I did" ]) ]) ]);
}

c.interactive("out4", init, update, view)
```
<div id="out4" style="margin:0px auto 40px auto;max-width:600px">
</div>

The code first checks if the animation is running. If so, it creates a timer to trigger the
`"animate"` event after 10ms. Then it calls the main `viewChart` function to get a
representation of the whole `chart`.

Now we need to add the button. To do this, we need to turn the chart into a HTML `<svg>`
element and then add some custom HTML elements including `<button>` around it. This means
stepping "outside" of the world of basic charts into the world of HTML. Compost allows us
to do this (exactly for cases like this) using `c.svg` (which renders a shape as SVG).
Once we have that, we can use `c.html` to create custom HTML elements. The `c.html` operation
is similar to the `h` function from [HyperScript](https://github.com/hyperhype/hyperscript).
Here, we create a button with `click` event handler that, when invoked, triggers the first
`"animate"` event to start the animation.

## Conclusions

In this blog post, I explained how to use the new [Compost.js](https://compostjs.github.io/compost/)
data visualization library to create an interactive "You Draw" bar chart inspired by the
awesome interactive line charts [from New York Times](https://www.nytimes.com/interactive/2017/01/15/us/politics/you-draw-obama-legacy.html).

This example is, in fact, what prompted me to think about how to design Compost.
Adam Pearce who created the New York Times chart [shared a D3 implementation](https://bl.ocks.org/1wheel/07d9040c3422dac16bd5be741433ff1e)
of the visualization and, when I was trying to understand how it works, I could not stop
thinking that there should be an easier way for creating visualizations like this.

The Compost library makes this easy through three main things:

1. You can specify all coordinates in "domain values", which can be numerical (% of GDP)
   or categorical (like our "Health" category). When you compose a chart from individual
   components (such as bars), the range of values for axes is inferred and
   the components are automatically aligned.

2. There is no special language for composing components. It's just plain JavaScript with
   functions that take shapes (or arrays of shapes) and produce new shapes. While this means
   that you sometimes need to write a bit more code (lots of `map` calls), it also
   means that all this code is perfectly clear. If you want, you can make it more readable
   by extracting functionality into a helper function like our `drawBar`.

3. The interactivity is implemented using the Model View Update architecture. This may be
   a personal preference (I like its functional programming style!) but I think this is a perfect fit
   for problems like interactive charts. In our case, the state of the chart is quite simple
   and there are only two events, which means that the whole logic can easily fit in a single brain.

If you found this interesting, you can learn more about Compost on the [Compost.js project
page](https://compostjs.github.io/compost/), which includes [plenty of demos](https://compostjs.github.io/compost/),
[API reference](https://compostjs.github.io/compost/api.html) and also an
[overview paper draft about its design](https://compostjs.github.io/compost/paper.pdf).
The core logic of Compost is implemented in [some 800 lines of F#](https://github.com/compostjs/compost/blob/master/src/compost/core.fs)
and is actually not that complex. Finally, the best way to run the interactive You Draw chart
that I described in this blog post is to clone and run the [compost-node-demos](https://github.com/compostjs/compost-node-demos/)
repository, which includes a full source code (just 70 lines) of the demo.



<script src="https://compostjs.github.io/compost/releases/compost-0.0.5.js"></script>
<script src="demos.js"></script>
