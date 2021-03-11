The Gamma dataviz package now available!
==============================================

 - date: 2017-01-25T12:31:14.3580990+00:00
 - description: There were a lot of rumors recently about the death of facts and statistics.
    I believe the core of the problem is that working with facts is quite tedious and the results 
    are often not particularly exciting. This is the problem that I'm trying to address with 
    The Gamma project - and today, I'm happy to share the first reusable component based on the 
    work that you can use in your data visualization projects.
 - layout: article
 - icon: fa fa-box-open
 - image-large: http://tomasp.net/blog/2017/thegamma-package/facts.jpg
 - tags: thegamma,data journalism,data science,research,visualization
 - title: The Gamma dataviz package now available!
 
----------------------------------------------------------------------------------------------------

There were a lot of rumors recently about [the death of facts](http://www.bbc.co.uk/news/uk-37995600)
and even [the death of statistics](https://www.theguardian.com/politics/2017/jan/19/crisis-of-statistics-big-data-democracy).
I believe the core of the problem is that working with facts is quite tedious and the results are
often not particularly exciting. Social media made it extremely easy to share your own opinions
in an engaging way, but what we are missing is a similarly easy and engaging way to share facts
backed by data. 

<a href="https://www.npmjs.com/package/thegamma-script">
<img src="http://tomasp.net/blog/2017/thegamma-package/npm.png" style="width:200px" class="rdecor-sm"/>
</a>

This is, in essence, the motivation for The Gamma project that I've been [working on 
recently](http://tomasp.net/blog/2016/thegamma-dni/). After several experiments, including the
[visualization of Olympic medalists](http://tomasp.net/blog/2016/thegamma-olympic-medalists/),
I'm now happy to share the first reusable component based on the work that you can try and use
in your data visualization projects. If you want to get started:

 - [Check out thegamma-script package on npm](https://www.npmjs.com/package/thegamma-script) 
 - [Minimal example of thegamma-script in action](http://thegamma-sample-web.azurewebsites.net/)
 - [How to use thegamma-script in your projects](http://thegamma.net/developers/)

The package implements a simple scripting language that anyone can use for writing simple data
aggregation and data exploration scripts. The tooling for the scripting language makes it super
easy to create and modify existing data analyses. Editor auto-complete offers all available 
operations and a spreadsheet-inspired editor lets you create scripts without writing code - yet,
you still get a transparent and reproducible script as the result.

----------------------------------------------------------------------------------------------------

What do you get?
----------------

I experimented with a couple of ideas when working on the project and the package implements
two most interesting ones:

 - **The pivot type provider** - lets you write scripts to perform data aggregation using
   just dot (full dot-driven development!) As you are writing code, it offers the available
   operations, so when you type `'filter data'.'Games is'.` the type provider offers a list
   of available values including e.g. `Rio (2016)`. This means that you do not have to learn 
   much about programming - you just need to choose what you want to do!
   
 - **Spreadsheet-inspired editor** - a nice thing about spreadsheets is that you always see
   concrete values you're working with. I [wrote about this recently](http://tomasp.net/blog/2016/no-functions/)
   and used spreadsheets as an inspiration for an editor that is included in The Gamma package.
   It shows the transformed data as you write your aggregations and it also lets you change the
   code through a simple user interface.
   
The following gif shows the ideas in action. Given a data set that consists of a list of all the
Olympic medal winners (which I used for the [Olympic medalists
project](http://tomasp.net/blog/2016/thegamma-olympic-medalists/)), we want to get a bar chart
with top countries based on medals from Rio 2016:
 
<div style="max-width:1008px;padding:0px;margin:10px auto 10px auto;text-align:center;">
  <img src="preview.gif" style="display:none" id="img1a" />
  <img src="preview-still.png" style="max-width:100%;cursor:pointer;border:1px solid #d8d8d8;border-radius:6px;padding:10px;margin-bottom:5px;" id="img1b" /><br />
  (<a href="preview.gif" target="_blank">Open the image in a new window</a>)<!--_ -->
  <script type="text/javascript">
    var p1 = false;
    document.getElementById("img1b").onclick = function() {
      document.getElementById("img1b").src = 
        p1 ? "preview-still.png" : document.getElementById("img1a").src;
      p1 = !p1;
    }
  </script>
</div>

The package provides an API for running The Gamma scripts and rendering the resulting chart
in a given `div` and it also provides an API for creating an editor that you can see in the 
above gif in a given `div`. The currently supported charting libraries are [Google 
Charts](https://developers.google.com/chart/) (though it is possible to add more!) and 
the editor uses the [Monaco editor](https://microsoft.github.io/monaco-editor/) behind the
scenes.

Getting started
---------------

The easiest way to get started is to look at the [developer documentation](http://thegamma.net/developers/)
and to explore [the sample web application](https://github.com/the-gamma/thegamma-sample-web),
which is [hosted live here](http://thegamma-sample-web.azurewebsites.net/).
If you want the use The Gamma, you will need two things. First, you will need a simple REST service
that provides the data for your visualizations and can execute simple queries constructed in the
browser. Second, you will need to configure the JavaScript component. There is a detailed
documentation for both of these on [thegamma.net](http://thegamma.net/), but I'll briefly 
summarize both here.

### Using the client-side component

The client-side component is available as [thegamma-script](https://www.npmjs.com/package/thegamma-script)
on npm. In order to use it with the [Monaco editor](https://microsoft.github.io/monaco-editor/), you
need to load it using `require.js`. Once it is loaded, all you need to do is to specify type 
providers that define what will be available to the person writing code. Type providers provide
top-level objects such as `olympics` (exposing data) and `chart` (mapping for Google charts).
The JavaScript code looks something like this:

```js
var services = "http://thegamma-services.azurewebsites.net/";
var libs = "/node_modules/thegamma-script/dist/libraries.json";

var providers = 
  g.providers.createProviders({ 
    "worldbank": g.providers.rest(services + "worldbank"),
    "olympics": g.providers.pivot(services + "pdata/olympics"),
    "libraries": g.providers.library(libs) });
    
var ctx = g.gamma.createContext(providers);
```

The `g.providers` API lets you define two kinds of type providers (the third one is
work-in-progress and I'll add it to the documentation soon):

 - The `pivot` provider takes a service that can evaluate "data aggregation" requests.
   The demo uses a [sample implementation](https://github.com/the-gamma/thegamma-services/blob/master/src/pdata/server.fsx)
   for the Olympic medals data set. The protocol that the service exposes is documented at
   [publishing data](/publishing) page. The provider automatically generates members that let you
   write data aggregations and transformations using `.` as in the demo.  

 - The `library` provider takes a JSON that specifies the types and structure of JavaScript 
   libraries - the `thegamma-script` package comes with a couple of wrappers for Google Charts
   and for generating tables that you can see in the [Olympic Medalists demo](http://rio2016.thegamma.net/).
   You can create your own too, but it's not documented yet...

Once type providers are specified, you get back a context `ctx` which can be used to
create Monaco editor, monitor errors in the code written by users and run scripts. 
Assuming the `#demo` element contains the script to run, you can execute it and display
the output in `#out1` element using:

```js
var code = document.getElementById("demo").innerText;
ctx.evaluate(code, "out1");
```

The context object provides the following functions:

 - `evaluate` lets you run user code written using The Gamma script.
 - `createEditor` lets you create Monaco editor for editing The Gamma code.
 - `errorReported` lets you register an event handler that is called code contains error.
   This is triggered when you run code using `evaluate` or while the user edits code in the editor.

Creating editor and reporting errors as shown in the [sample web 
demo](http://thegamma-sample-web.azurewebsites.net) is similarly easy to running code.
The full [developer documentation](http://thegamma.net/developers/) explains the remaining
functions of the public API.

### Implementing data provider

The type provider specified using `g.providers.pivot` takes a URL of a data source. All you 
need to do to provide your own data source is to create a simple REST service (which can be 
written in any language) that returns metadata about your data source and can evaluate
queries written using The Gamma.

The query evaluation is done on the server rather than the client. This lets you expose large
data sets that would not be easy to download (the results of the query are typically smaller and
can be truncated), but it means some more logic is needed on the server.

Assuming you specify `http://example.com/olympics` as your data source. When you create the type
provider, it will first make request with `?metadata` query to get information about the columns
that your data set contains. The response should be a JSON record with column names as keys and
types (`number` or `string`) as values:

```
GET http://example.com/olympics?metadata
```

```js
{ "Games":"string", "Year":"number", "Event":"string",
  "Discipline":"string", "Athlete":"string", 
  "Sport":"string", "Gender":"string", "Team":"string",
  "Event":"string", "Medal":"string", "Gold":"number",
  "Silver":"number", "Bronze":"number" }
```

This is all the pivot type provider needs to generate most of the members that are available
in the auto-completion list. When you finish writing code and run it, another request is issued
to get the data. For the above example where we look at Rio 2016, group data by Athlete,
sum number of Gold medals for each athlete, sort the results by the number of medals, take the top
3 and then get a data series with athlete and the number of medals, the query looks as follows:

```
GET http://example.com/olympics?
  filter(Games eq Rio (2016))$
  groupby(by Athlete,sum Gold,key)$
  sort(Gold desc)$take(3)$series(Athlete,Gold)
```  

```js
[ [ "Michael Phelps", 5 ], 
  [ "Katie Ledecky", 4 ], 
  [ "Simone Biles", 4 ] ]
```

As you can see, parts of the query are separated by `$` and they represent the indidividual steps
of the data transformation. The part `series(Athlete,Gold)` at the end specifies what data we
want to get - here, we want to get the result as a _series_, which is a simple list of key value
pairs, stored as nested lists.

If you want to learn more, the [publishing data](http://thegamma.net/publishing/) article explains 
the details of the protocol and the [Olympics
service](https://github.com/the-gamma/thegamma-services/blob/1879f5046822943074f3c01cbeff6ebda149032d/src/pdata/server.fsx)
shows a minimal F# example that exposed data based on a CSV file. I will be adding further 
examples to [The Gamma repository](http://github.com/the-gamma) soon, but let me know at
[@tomaspetricek](https://twitter.com/tomaspetricek) if you have some interesting data source
that you'd like to support!

## Summary

When I [started thinking](http://tomasp.net/blog/2015/thegamma/) about open and transparent 
visualizations more than a year ago, I did not realize how timely issue this will be. I believe
that we need to build _much_ better tools for making facts backed by data more engaging. 
Using a phrase from my recent talk, here is what we need!

<div style="text-align:center"><img src="facts.jpg" style="max-width:80%;margin:10px 0px 30px 0px" /></div>

The Gamma project is still in its early days, but the [thegamma-script](https://www.npmjs.com/package/thegamma-script)
package that you can now use is the first step towards using data in a more fun way that 
encourages people to explore data on their own and produce transparent and reproducible results. 
Thanks to the pivot type provider and the spreadsheet-inspired editor available through The 
Gamma, doing that should now be a lot easier!
