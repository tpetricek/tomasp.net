# The Gamma: Democratizing data science

 - title: The Gamma: Democratizing data science
 - layout: post
 - tags: thegamma, data journalism
 - date: 2 February 2017
 - description: The goal of The Gamma project is to democratize data science. Our tools encourage 
   everyone — including journalists and interested citizens — to understand how presented claims are 
   justified, explore data on their own and make their own transparent factual claims. If the society 
   is to benefit from the possibilities available through data science, it is essential to make 
   data-driven storytelling widely accessible, open and engaging.
 
----

The rise of Big Data and Open Government Data initiatives means that there is an increasing 
amount of raw data available. At the same time, "post-truth" has been chosen as [the word of 2016][post]
and the general public [increasingly distrusts statistics][stat]. In other words, data science has more 
capabilities to help us understand the world than ever before, yet it is becoming less relevant 
in public discussion.

This should perhaps not be a surprise as data science is often opaque, non-experts find results 
difficult to interpret and verify, and creating data-driven reports is limited to a small number of 
specialists. The goal of The Gamma project is to democratize data science. Our tools encourage 
everyone — including journalists and interested citizens — to understand how presented claims are 
justified, explore data on their own and make their own transparent factual claims. If the society 
is to benefit from the possibilities available through data science, it is essential to make 
data-driven storytelling widely accessible, open and engaging.

---

## Open and engaging data-driven storytelling

On one hand, spreadsheets made data exploration accessible to a large number of people, but 
operations performed on spreadsheets are error-prone and cannot be easily reproduced or replicated 
with different data source. On the other hand, data analyses written as a programs can be modified
and run repeatedly, but even with the simplest programming tools available, building an end-to-end
analysis that reads data from a government data source, performs analysis and produces an 
interactive visualization requires expert programming  and data science skills.

The Gamma aims to build programming tools that let anyone explore data from a wide range of
data sources, including open government data, and publish data-driven reports that are:
 
 1. **Transparent and accountable.** Readers can review how data is used and discover misleading uses of data.
 2. **Reproducible and connected.** Readers can run the analysis themselves using the original data source.
 3. **Open and engaging.** Readers can modify parameters and share reports on different aspects of the data.

To achieve this, we treat data-driven reports as reproducible programs written in a simple 
web-based scripting language that is integrated with primary data sources (using the type 
providers mechanism) and we develop editor tooling that bridges the gap between programming
and spreadsheets.

## Simple programming tools for data exploration

The work done so far has been focused on building a simple web-based library that 
could be used by data journalists to present visualizations obtained by aggregating 
and summarizing tabular data. For example, given a data table recording individual
medals awarded over the entire history of the Olympic games, we can easily calculate the number 
of medals per country. The following animation illustrates how a column chart with top 8 countries
can be created.

<div class="wdecor">
  <div style="max-width:900px;padding:0px;margin:10px auto 10px auto;">
    <img src="preview-1.gif" style="display:none" id="img1a" />
    <img src="preview-1-still.png" style="max-width:100%;cursor:pointer;border:1px solid #d8d8d8;border-radius:6px;padding:10px;margin-bottom:5px;" id="img1b" />
    <script type="text/javascript">
      var p1 = false;
      document.getElementById("img1b").onclick = function() {
        document.getElementById("img1b").src = 
          p1 ? "preview-1-still.png" : document.getElementById("img1a").src;
        p1 = !p1;
      }
    </script>
  </div>
</div>

The project is innovative in two ways. It creates a new simple scripting language for working
with data and it complements the language with spreadsheet-inspired tooling:

 * **Simple data-aware language.** When writing code, the programming language understands 
   the data source and data transformations performed so far and offers all available operations 
   when `.` is typed. For example, `by Gold descending` is offered after typing `sort data` because
   the language understands what columns will be available after grouping data. This means that 
   the user can construct the whole program just by choosing one of the available operations.
 
 * **Spreadsheet-inspired editing.** One of the reasons why spreadsheets are easy to use is that the 
   user can always see the data they are working with and manipulate it directly. We adapt this 
   paradigm to programming — in our live editor, the user can always see preview of the aggregation 
   constructed so far, making data exploration easier. Furthermore, many transformations can be 
   created using the user interface without writing code directly. Yet, the final result is still
   an open and reproducible script.

These two innovations make it possible to create web-based data-driven reports that are transparent
(anyone can see how they are created), open (readers can modify them and share their results) and
engaging (reader can explore other fun aspects of the data).

The Gamma project is the first step of an increasingly important research that aims to democratize
data science and encourage every citizen to make factual claims backed by data — be it for fun or 
to hold the government accountable.

## Open-source and research outcomes

We believe that the goals of The Gamma project can best be achieved by illustrating the 
possibilities of open and transparent storytelling by building sample projects visualizing 
interesting data sets, by creating reusable libraries that others can use to replicate our work
and by presenting the interesting research aspects of the work to the academic community.

 * **Project: [Visualizing Olympic medalists](http://rio2016.thegamma.net).** During the Olympic
   games in Rio 2016, we created a web page showing a number of open visualizations using the
   data on Olympic medalists. Each visualization allows the reader to change parameters of the 
   script (through code or user interface) and encourages the reader to explore further questions.
   What would the table of top athletes look for a different country? What if European Union or 
   the Commonwealth competed as a single country? More information about the project can be
   found in a [summary blog post](http://tomasp.net/blog/2016/thegamma-olympic-medalists/).

 * **Library: [Tools for open data-driven storytelling](https://thegamma.net).** In December,
   we released The Gamma as an open-source library that anyone can use to build their own 
   open data-driven visualizations. The package is [hosted on GitHub](http://github.com/the-gamma), 
   [available via npm](https://www.npmjs.com/package/thegamma-script) (JavaScript package 
   repository) and already started attracting [early external contributors][contr]. The
   [project announcement](http://tomasp.net/blog/2017/thegamma-package/) contains more information.
   
 * **Paper: [Data exploration through dot-driven development](paper.pdf)** (submitted).
   In a recently submitted paper, we describe how our scripting language simplifies writing scripts
   for data exploration by exposing data transformations through a unified mechanism of member
   access. The paper also proves that all data transformations constructed by choosing one of
   the available members are valid.

In the future, we intend to expand the scope of the project in two directions. First, we 
intend to make a wide range of data sources available in The Gamma. This includes data published 
by the UK Open Government Data initiative and data provided by international organizations such 
as the World Bank and OECD. An interesting aspect of this work is automatically cleaning the 
primary data sources and linking entities across multiple data sources.

Second, we are interested in extending the scripting language to allow users to gain deeper
insights into the data. Similarly to the recent [New York Times article on Obama's legacy][nyt], we would like to allow
readers to use the same simple programming style to encode their guesses about causation in the
world, before seeing how their guesses fit with the available data and using the model to make
predictions.

## References
 
 1. [How statistics lost their power – and why we should fear what comes 
     next](https://www.theguardian.com/politics/2017/jan/19/crisis-of-statistics-big-data-democracy),<br /> William Davies, The Guardian
 2. ['Post-truth' declared word of the year by Oxford Dictionaries](http://www.bbc.co.uk/news/uk-37995600), BBC News
 3. [You Draw It: What Got Better or Worse During Obama's Presidency](https://www.nytimes.com/interactive/2017/01/15/us/politics/you-draw-obama-legacy.html),<br />
     Larry Buchnan, Haeyoun Park and Adam Pearce, The New York Times
 4. [The Gamma — Visualizing Olympic Medalists](http://tomasp.net/blog/2016/thegamma-olympic-medalists/), Tomas Petricek's blog
 5. [The Gamma dataviz package now available!](http://tomasp.net/blog/2017/thegamma-package/), Tomas Petricek's blog
 6. [SQL Service: An SQL-backed data service for The Gamma](https://github.com/jazzido/thegamma-sql-service), Manuel Aristarán

[post]: http://www.bbc.co.uk/news/uk-37995600
[stat]: https://www.theguardian.com/politics/2017/jan/19/crisis-of-statistics-big-data-democracy 
[nyt]: https://www.nytimes.com/interactive/2017/01/15/us/politics/you-draw-obama-legacy.html 
[contr]: https://github.com/jazzido/thegamma-sql-service
