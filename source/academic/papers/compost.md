# Composable data visualizations

 - description: Compost is a data visualization library that lets you compose rich interactive data
      visualizations from a small number of basic primitives. The library is based on the
      functional programming idea of composable domain-specific languages. This functional
      programming pearl is a tutorial that introduces Compost through a wide range of examples.
 - tags: academic, publication, top, home
 - layout: article
 - icon: fa fa-chart-bar
 - date: 17 June 2021
 - title: Data exploration through dot-driven development
 - subtitle: Tomas Petricek. Journal of Functional Programming, 2021

> Tomas Petricek
>
> Journal of Functional Programming, vol. 31, 2021

When visualizing data, you have to choose between high-level data visualization libraries
such as Google Charts or low-level libraries like D3. In high-level libraries, you specify
the type of chart you want, but you are limited to the features that the author of the library
implemented. With low-level libraries, you have full control over the look of the chart, but
you have to tediously transform your values to coordinates in pixels yourself.

What would an elegant functional approach to data visualization look like? A functional programmer
would want a domain-specific language that has a small number of primitives that allow us to define
high level abstractions such as a bar chart, but does not limit what we can do. When creating
charts, we should be able to use domain values such as a currency exchange rate, rather than pixels
in its basic building blocks.

As is often the case with domain-specific languages, finding the right primitives is more of an art
than science. For this reason, we present our solution, a library named Compost, as a functional
pearl. We hope to convince the reader that Compost is elegant and we illustrate this with a wide
range of examples

## Paper and more information

 - Download [local copy (PDF)](jfp.pdf) or [through open-access](https://www.cambridge.org/core/journals/journal-of-functional-programming/article/composable-data-visualizations/CFC3E7AFACBEE62AE3AC70AD6DF4F3D5)
 - Learn more at [the Compost.js project home page](https://compostjs.github.io/compost/)

## Sample chart

The following brief example, from the [project homepage](https://compostjs.github.io/compost/)
shows how to use the Compost JavaScript API to create a chart that combines a line chart,
showing the GBP-USD exchange rate, with a two-part background distinguishing the time before
the Brexit vote and after the Brexit vote.

<script src="https://compostjs.github.io/compost/releases/compost-latest.js"></script>
<script src="https://compostjs.github.io/compost/lib/docs.js"></script>
<div id="demo" class="compost-out" style="max-width:600px;height:250px;margin:30px auto 10px auto"></div>

```
// Exchange rate range for the background
let lo = 1.25, hi = 1.52;
// Overlay three shapes and add axes on three sides
let xchg = c.axes("left right bottom", c.overlay([
  // Fill area behind first 16 values in blue
  c.fillColor("#1F77B460",  c.shape(
    [ [0,lo], [16,lo], [16,hi], [0,hi] ])),
  // Fill area behind the remaining values in red
  c.fillColor("#D6272860",  c.shape(
    [ [gbpusd.length-1,lo], [16,lo], [16,hi], [gbpusd.length-1,hi] ])),
  // Draw a black line using 'gbpusd' array
  c.strokeColor("#202020", c.line(gbpusd.map((v, i) => [i, v])))
]));
// Render chart on &lt;div id="demo" /&gt;
c.render("demo", xchg);
```

<script type="text/javascript">
// Exchange rate range for the background
let lo = 1.25, hi = 1.52;
// Overlay three shapes and add axes on three sides
let xchg = c.axes("left right bottom", c.overlay([
  // Fill area behind first 16 values in blue
  c.fillColor("#1F77B460",  c.shape(
    [ [0,lo], [16,lo], [16,hi], [0,hi] ])),
  // Fill area behind the remaining values in red
  c.fillColor("#D6272860",  c.shape(
    [ [gbpusd.length-1,lo], [16,lo], [16,hi], [gbpusd.length-1,hi] ])),
  // Draw a black line using 'gbpusd' array
  c.strokeColor("#202020", c.line(gbpusd.map((v, i) => [i, v])))
]));
// Render chart on &lt;div id="demo" /&gt;
c.render("demo", xchg);
</script>

## Talk recording
Watch a short video presentation associated with the JFP paper:

<div style="padding:5px 20px 0px 20px;position:relative;">
<div style="padding:56.25% 0 0 0;position:relative;"><iframe src="https://player.vimeo.com/video/669589488?h=752f8068c9&amp;badge=0&amp;autopause=0&amp;player_id=0&amp;app_id=58479" frameborder="0" allow="autoplay; fullscreen; picture-in-picture" allowfullscreen style="position:absolute;top:0;left:0;width:100%;height:100%;" title="Composable data visualizations (Journal of Functional Programming)"></iframe></div><script src="https://player.vimeo.com/api/player.js"></script>
</div>

## <a id="cite">Bibtex</a>
If you want to cite the paper, you can use the following BibTeX information.

    [lang=tex]
    @article{compost-jfp2021,
      author={Petricek, Tomas},
      title={Composable data visualizations},
      volume={31},
      doi={10.1017/S0956796821000046},
      journal={Journal of Functional Programming},
      publisher={Cambridge University Press},
      year={2021},
    }

If you have any comments, suggestions or related ideas, I'll be happy to
hear from you! Send me an email at [tomas@tomasp.net](mailto:tomas@tomasp.net)
or get in touch via Twitter at [@tomaspetricek](http://twitter.com/tomaspetricek).
