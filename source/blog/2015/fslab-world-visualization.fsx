(**
Visualizing interesting world facts with FsLab
==============================================

 - date: 2015-06-30T16:07:13.5363212+01:00
 - description: This blog post combines two interesting FsLab libraries. It uses the  World Bank type provider from F# Data to get a number of interesting indicators about different countries of the world and it visualizes them using the XPlot library which makes it easy to build rich HTML5 visualizations using Google Charts and Plotly.
 - layout: article
 - image-large: http://tomasp.net/blog/2015/fslab-world-visualization/world-large.png
 - tags: f#,fslab,data science,data journalism,thegamma
 - title: Visualizing interesting world facts with FsLab
 - url: 2015/fslab-world-visualization

--------------------------------------------------------------------------------
 - standalone


In case you missed my recent [official FsLab announcement](http://tomasp.net/blog/2015/announcing-fslab/), 
FsLab is a data-science package for .NET built around F# that makes it easy to get data using _type providers_, 
analyze them interactively (with great R integration) and visualize the results. You can find more on
on [fslab.org](http://fslab.org), which also has links to [some](http://channel9.msdn.com/posts/Understanding-the-World-with-F)
[videos](https://channel9.msdn.com/Events/dotnetConf/2015/The-F-Path-to-Data-Scripting-Nirvana) and
[download page with templates](http://fslab.org/download/) and other instructions.

Last time, I mentioned that we are working on integrating FsLab with the [XPlot charting 
library](http://tahahachana.github.io/XPlot/). XPlot is a wonderful F# library built by 
[Taha Hachana](https://twitter.com/TahaHachana) that wraps two powerful HTML5 visualization libraries - 
[Google Charts](https://developers.google.com/chart/) and [plot.ly](https://plot.ly/). 

I thought I'd see what interesting visualizations I can built with XPlot, so I opened the [World Bank type
provider](http://fsharp.github.io/FSharp.Data/library/WorldBank.html) to get some data about the world
and Euro area, to make the blog post relevant to what is happening in the world today.

<img src="http://tomasp.net/blog/2015/fslab-world-visualization/world-sm.png" />

--------------------------------------------------------------------------------
*)
(*** hide ***)
#I "../packages/2015/"
#load @"C:\Tomas\Public\fslaborg-contrib\XPlot\docs\content\credentials.fsx"
(**

In case you missed my recent [official FsLab announcement](http://tomasp.net/blog/2015/announcing-fslab/), 
FsLab is a data-science package for .NET built around F# that makes it easy to get data using _type providers_, 
analyze them interactively (with great R integration) and visualize the results. You can find more on
on [fslab.org](http://fslab.org), which also has links to [some](http://channel9.msdn.com/posts/Understanding-the-World-with-F)
[videos](https://channel9.msdn.com/Events/dotnetConf/2015/The-F-Path-to-Data-Scripting-Nirvana) and
[download page with templates](http://fslab.org/download/) and other instructions.

Last time, I mentioned that we are working on integrating FsLab with the [XPlot charting 
library](http://tahahachana.github.io/XPlot/). XPlot is a wonderful F# library built by 
[Taha Hachana](https://twitter.com/TahaHachana) that wraps two powerful HTML5 visualization libraries - 
[Google Charts](https://developers.google.com/chart/) and [plot.ly](https://plot.ly/). 

I thought I'd see what interesting visualizations I can built with XPlot, so I opened the [World Bank type
provider](http://fsharp.github.io/FSharp.Data/library/WorldBank.html) to get some data about the world
and Euro area, to make the blog post relevant to what is happening in the world today.

With type providers, getting data is amazingly easy. So if you have not seen much F# before, the following
9 lines is all I need to set things up:
*)
#load "packages/FsLab/FsLab.fsx"
open System
open FSharp.Data
open XPlot.GoogleCharts
open XPlot.Plotly

// Pass hidden credentials to Plotly & load WorldBank
Plotly.Signin MyCredentials.userAndKey
let wb = WorldBankData.GetDataContext()
(**
Visualization #1: Structure of GDP in the Euro area
---------------------------------------------------

<p>In the first visualization, let's look at the countries that contribute the most to the total GDP
of the Euro area (this is countries within the European Union that are using the Euro currency).
To get the countries, we can use WorldBank type provider which exposes the region as
<code>wb.Regions.&#96;&#96;Euro area&#96;&#96;</code>. We first return the data for the root 
element (the whole Euro area) and then yield data for each of the member countries:</p>

*)
let euroGDP = 
 [ let euro = wb.Regions.``Euro area``
   yield "EU","",euro.Indicators.``GDP (current US$)``.[2010] 
   for c in euro.Countries do
     yield c.Name,"EU",c.Indicators.``GDP (current US$)``.[2010]]
(**
When writing the code in an F#-enabled editor, you get auto-completion support on the
indicators, so you can choose &#96;&#96;GDP (current US$)&#96;&#96; among thousands of
indicators available from the World Bank.

Now we can pass the data to `Chart.Treemap` (which takes a sequence of node name, parent
node and value) and set some options to make the visualization nicer:
*)
Chart.Treemap(euroGDP)
|> Chart.WithOptions 
    (Options(minColor="#B24590", midColor="#449AB5", 
      maxColor="#76B747", headerHeight=0, showScale=true))
(**
<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript">
  google.load("visualization", "1", {packages:["treemap","geochart","corechart"]})
  google.setOnLoadCallback(drawChart);
  function drawChart() {
    var data = new google.visualization.DataTable({"cols": [{"type": "string" ,"id": "Country" ,"label": "Country" }, {"type": "string" ,"id": "Region" ,"label": "Region" }, {"type": "number" ,"id": "GDP (current $)" ,"label": "GDP (current $)" }, {"type": "number" ,"id": "GDP" ,"label": "GDP" }], "rows" : [{"c" : [{"v": "EU"}, {"v": ""}, {"v": 12632263801735.7}, {"v": 12632263801735.7}]}, {"c" : [{"v": "Austria"}, {"v": "EU"}, {"v": 389656071767.183}, {"v": 389656071767.183}]}, {"c" : [{"v": "Belgium"}, {"v": "EU"}, {"v": 484404271608.088}, {"v": 484404271608.088}]}, {"c" : [{"v": "Cyprus"}, {"v": "EU"}, {"v": 23132450331.1258}, {"v": 23132450331.1258}]}, {"c" : [{"v": "Germany"}, {"v": "EU"}, {"v": 3412008772736.86}, {"v": 3412008772736.86}]}, {"c" : [{"v": "Spain"}, {"v": "EU"}, {"v": 1431587612302.26}, {"v": 1431587612302.26}]}, {"c" : [{"v": "Estonia"}, {"v": "EU"}, {"v": 19479012423.353}, {"v": 19479012423.353}]}, {"c" : [{"v": "Finland"}, {"v": "EU"}, {"v": 247799815768.477}, {"v": 247799815768.477}]}, {"c" : [{"v": "France"}, {"v": "EU"}, {"v": 2646837111794.78}, {"v": 2646837111794.78}]}, {"c" : [{"v": "Greece"}, {"v": "EU"}, {"v": 299598056253.272}, {"v": 299598056253.272}]}, {"c" : [{"v": "Ireland"}, {"v": "EU"}, {"v": 218435251789.115}, {"v": 218435251789.115}]}, {"c" : [{"v": "Italy"}, {"v": "EU"}, {"v": 2126620402889.09}, {"v": 2126620402889.09}]}, {"c" : [{"v": "Luxembourg"}, {"v": "EU"}, {"v": 52143650382.9908}, {"v": 52143650382.9908}]}, {"c" : [{"v": "Latvia"}, {"v": "EU"}, {"v": 24009680459.9868}, {"v": 24009680459.9868}]}, {"c" : [{"v": "Malta"}, {"v": "EU"}, {"v": 8163841059.60265}, {"v": 8163841059.60265}]}, {"c" : [{"v": "Netherlands"}, {"v": "EU"}, {"v": 836389937229.197}, {"v": 836389937229.197}]}, {"c" : [{"v": "Portugal"}, {"v": "EU"}, {"v": 238303443425.21}, {"v": 238303443425.21}]}, {"c" : [{"v": "Slovak Republic"}, {"v": "EU"}, {"v": 89011919205.298}, {"v": 89011919205.298}]}, {"c" : [{"v": "Slovenia"}, {"v": "EU"}, {"v": 47972988741.7219}, {"v": 47972988741.7219}]}]});
    var options = {"fontSize":12,"legend":{"position":"none"},"headerHeight":0,"maxColor":"#76B747","midColor":"#449AB5","minColor":"#B24590","showScale":true} 
    var chart = new google.visualization.TreeMap(document.getElementById('a68e65c0-e49d-494b-be9c-019bac3a9021'));
    chart.draw(data, options);
  }
</script>
<div id="a68e65c0-e49d-494b-be9c-019bac3a9021" style="margin-left:auto;margin-right:auto;width:500px; height:350px;"></div>

The tree map gives us a very nice overview of how different countries in the Euro area
contribute to the total GDP of the region. As you can see above, we are showing total 
GDP (in current US$) and so larger countries obviously contribute more with Germany,
France and Italy producing over half of the GDP.

Visualization #2: GDP per capita in the Euro area
-------------------------------------------------

Another interesting indicator we can get from the World Bank data is GDP per capita.
Using this indicator, we can find smaller countries that have higher GDP (which was
not visible in the previous visualization). This time, we'll use the Plotly bindings
and create a bar chart using the `Bar` trace. To follow the visual theme of the 
previous visualization, I also added a bit of code to calculate the colour (which you
can find in the [full blog post source](https://github.com/tpetricek/TomaspNet.Website/blob/master/source/blog/2015/fslab-world-visualization.fsx)

*)

let euroGDPperCap = 
 [ for c in wb.Regions.``Euro area``.Countries ->
   c.Name, c.Indicators.``GDP per capita (current US$)``.[2010]]
 |> List.sortBy snd |> List.rev

(*[omit: Calculating colours omitted ]*)
let lo = euroGDPperCap |> List.map snd |> List.min
let hi = euroGDPperCap |> List.map snd |> List.max

let midColor clr1 clr2 v =
  let mb a b = int (float a + (float b - float a) * v)
  let clr1 = System.Drawing.ColorTranslator.FromHtml(clr1)
  let clr2 = System.Drawing.ColorTranslator.FromHtml(clr2)
  System.Drawing.Color.FromArgb
    (mb clr1.R clr2.R, mb clr1.G clr2.G, mb clr1.B clr2.B)

let getColor (_, v) = 
  let k = (v - lo) / (hi - lo)
  System.Drawing.ColorTranslator.ToHtml
   ( if k < 0.5 then midColor "#B24590" "#449AB5" (2.0*k)
     else midColor "#449AB5" "#76B747" ((k-0.5)*2.0) )
(*[/omit]*)

let barChart = Bar(
  x = List.map fst euroGDPperCap,
  y = List.map snd euroGDPperCap,
  marker = Marker(color=List.map getColor euroGDPperCap),
  orientation = "v")
Figure(Data([barChart]), Layout(title="GDP per capita in Euro area"))
(**
<iframe width="600" height="350" frameborder="0" seamless="seamless" 
           scrolling="no" src="https://plot.ly/~tomasp/239.embed?width=600&height=350"></iframe>

Here, we can see that most countries in the Euro area are fairly ballanced, except for
Luxembourg which has two times the GDP per capita than the second country. As one would expect,
the new member countries are at the end of the scale and western countries are at the 
beginning of the scale.

Visualization #3: GDP growth in Euro area
-----------------------------------------

There is yet another interesting indicator in the World Bank that we can look at.
This is the GDP growth indicator, which gives us annual growth rate of the GDP. 
In this visualization, we'll look how the growth has been changing over the last
50 years. For most countries, this is very homogeneous, but there are some interesting
spikes. 

We'll use Plotly again. One nice feature is that we can get data for all countries,
but make only a few of them visible by default. Click on the country name on the right 
to add/remove them from the cart!

*)
let visible = set ["Greece";"Ireland";"Latvia";"Malta"]
let data = 
 [ for ct in wb.Regions.``Euro area``.Countries ->
   let data = ct.Indicators.``GDP growth (annual %)``
   let vis = if visible.Contains ct.Name then "" else "legendonly"
   Scatter(x=Seq.map fst data, y=Seq.map snd data, 
     name=ct.Name, visible=vis) ]

Figure
 ( Data.From data, 
   Layout(title="GDP growth (annual %) in Euro area") )
(**
<iframe width="600" height="450" frameborder="0" seamless="seamless" style="margin-left:auto;margin-right:auto;"
           scrolling="no" src="https://plot.ly/~tomasp/243.embed?width=600&height=450"></iframe>

Here, we can see (and you can also zoom in!) that there is a big spike in the economy of Malta 
(20% growth in 1975) and big spike in Latvia (-32% in 1992, followed by a fairly quick recovery).
Most countries also suffered after the 2008 crisis.

Visualization #4: Correlating GDP and life expectancy
-----------------------------------------------------

So far, I was looking at GDP, because it is one of the economic indicators that is easy to get
from the World Bank. Let's look if GDP is correlated with some other indicators we can obtain.
The following gets the GDP (per capita) and Life expectancy (in years) for all countries of the
world (looking at Europe would not tell us much).

Now we draw a scatter plot showing life expectancy (on the Y axis) and logarithm (base 10) of 
the GDP per capita (on the X axis). The following uses Google charts and also uses the
`trendlines` parameter to add a linear trendline to the chart:
*)
let gdpVsLifeExp = 
 [ for c in wb.Countries -> 
   log10 c.Indicators.``GDP per capita (current US$)``.[2010],
   c.Indicators.``Life expectancy at birth, total (years)``.[2010]]

let options = Options(pointSize=3, colors=[|"#3B8FCC"|], 
  trendlines=[|Trendline(opacity=0.5,lineWidth=10,color="#C0D9EA")|],
  hAxis=Axis(title="Log of GDP (per capita)"), 
  vAxis=Axis(title="Life expectancy (years)"))

Chart.Scatter(gdpVsLifeExp)
|> Chart.WithOptions(options)
(**

<script type="text/javascript">
google.setOnLoadCallback(drawChart3);
function drawChart3() {
  var data = new google.visualization.DataTable({"cols": [{"type": "number" ,"id": "Column 1" ,"label": "Column 1" }, {"type": "number" ,"id": "Column 2" ,"label": "Column 2" }], "rows" : [{"c" : [{"v": 4.38541216522114}, {"v": 74.9520243902439}]}, {"c" : [{"v": 2.74911581862957}, {"v": 59.6000975609756}]}, {"c" : [{"v": 3.6251734056464}, {"v": 50.6541707317073}]}, {"c" : [{"v": 3.61218604730331}, {"v": 76.978512195122}]}, {"c" : [{"v": NaN}, {"v": NaN}]}, {"c" : [{"v": 4.53001935094468}, {"v": 76.5986097560976}]}, {"c" : [{"v": 4.05919887209414}, {"v": 75.6635609756098}]}, {"c" : [{"v": 3.49482012091758}, {"v": 74.2196585365854}]}, {"c" : [{"v": NaN}, {"v": NaN}]}, {"c" : [{"v": 4.11452126038337}, {"v": 75.3339024390244}]}, {"c" : [{"v": 4.71433756850395}, {"v": 81.6951219512195}]}, {"c" : [{"v": 4.66829837178151}, {"v": 80.5804878048781}]}, {"c" : [{"v": 3.7666214504864}, {"v": 70.4502926829268}]}, {"c" : [{"v": 2.34149348065392}, {"v": 52.6240243902439}]}, {"c" : [{"v": 4.6469745083888}, {"v": 80.2341463414634}]}, {"c" : [{"v": 2.83885052647522}, {"v": 58.7466829268293}]}, {"c" : [{"v": 2.76243013242123}, {"v": 55.006756097561}]}, {"c" : [{"v": 2.88241281343084}, {"v": 69.4858048780488}]}, {"c" : [{"v": 3.81827960782062}, {"v": 73.5121951219512}]}, {"c" : [{"v": 4.31272658668849}, {"v": 76.2648536585366}]}, {"c" : [{"v": 4.34127362384964}, {"v": 74.5923902439024}]}, {"c" : [{"v": 3.6415340236832}, {"v": 75.8066829268293}]}, {"c" : [{"v": 3.76483752464009}, {"v": 70.4048780487805}]}, {"c" : [{"v": 3.65584278847947}, {"v": 73.2704878048781}]}, {"c" : [{"v": 4.94550466434467}, {"v": 79.2885365853658}]}, {"c" : [{"v": 3.28660601437536}, {"v": 66.3197073170732}]}, {"c" : [{"v": 4.04053352146238}, {"v": 73.0753170731707}]}, {"c" : [{"v": 4.19899448996056}, {"v": 74.8017804878049}]}, {"c" : [{"v": 4.4896821416807}, {"v": 77.9886585365854}]}, {"c" : [{"v": 3.3446556225312}, {"v": 67.0046829268293}]}, {"c" : [{"v": 3.84387793392459}, {"v": 46.4402926829268}]}, {"c" : [{"v": 2.65950110196317}, {"v": 48.0987317073171}]}, {"c" : [{"v": 4.67637665035827}, {"v": 80.8934878048781}]}, {"c" : [{"v": 4.87085270813546}, {"v": 82.2463414634147}]}, {"c" : [{"v": NaN}, {"v": 79.8314878048781}]}, {"c" : [{"v": 4.10317970808604}, {"v": 79.0504634146342}]}, {"c" : [{"v": 3.64673112614455}, {"v": 74.8850243902439}]}, {"c" : [{"v": 3.11771085916624}, {"v": 49.6752926829268}]}, {"c" : [{"v": 3.0589454215409}, {"v": 53.6948292682927}]}, {"c" : [{"v": 2.53995919637774}, {"v": 48.9876829268293}]}, {"c" : [{"v": 3.46544333732042}, {"v": 57.2040243902439}]}, {"c" : [{"v": 3.79097233487723}, {"v": 73.3676829268293}]}, {"c" : [{"v": 2.87898721298095}, {"v": 60.2034146341463}]}, {"c" : [{"v": 3.53316980007216}, {"v": 73.8569756097561}]}, {"c" : [{"v": 3.89059903113399}, {"v": 79.2797073170732}]}, {"c" : [{"v": 3.75602436979758}, {"v": 78.7177804878049}]}, {"c" : [{"v": NaN}, {"v": NaN}]}, {"c" : [{"v": NaN}, {"v": NaN}]}, {"c" : [{"v": 4.44543352123956}, {"v": 79.3098292682927}]}, {"c" : [{"v": 4.29587518672292}, {"v": 77.4243902439025}]}, {"c" : [{"v": 4.62037933651685}, {"v": 79.9878048780488}]}, {"c" : [{"v": 3.13135975154775}, {"v": 60.2911951219512}]}, {"c" : [{"v": 3.84053563251288}, {"v": NaN}]}, {"c" : [{"v": 4.7607836798824}, {"v": 79.1}]}, {"c" : [{"v": 3.72389874569386}, {"v": 72.7921463414634}]}, {"c" : [{"v": 3.63844625715063}, {"v": 70.6166097560976}]}, {"c" : [{"v": 3.6662083509312}, {"v": 75.6477073170732}]}, {"c" : [{"v": 3.44770566669955}, {"v": 70.4508292682927}]}, {"c" : [{"v": 2.56672938869964}, {"v": 61.1850975609756}]}, {"c" : [{"v": 4.48764737970783}, {"v": 81.6268292682927}]}, {"c" : [{"v": 4.16523391811268}, {"v": 75.4292682926829}]}, {"c" : [{"v": 2.53616708771456}, {"v": 61.4679512195122}]}, {"c" : [{"v": 4.66466467825053}, {"v": 79.8707317073171}]}, {"c" : [{"v": 3.56221936869505}, {"v": 69.3823170731707}]}, {"c" : [{"v": 4.60965926401512}, {"v": 81.6634146341463}]}, {"c" : [{"v": NaN}, {"v": 80.6439024390244}]}, {"c" : [{"v": 3.45308102612131}, {"v": 68.6217317073171}]}, {"c" : [{"v": 3.97137390577695}, {"v": 62.289756097561}]}, {"c" : [{"v": 4.5839175591207}, {"v": 80.4024390243902}]}, {"c" : [{"v": 3.41726519642048}, {"v": 73.6747317073171}]}, {"c" : [{"v": 3.12257366586603}, {"v": 60.5995609756098}]}, {"c" : [{"v": 2.63893718584892}, {"v": 55.298}]}, {"c" : [{"v": 2.75308345431508}, {"v": 58.133512195122}]}, {"c" : [{"v": 2.7276612635472}, {"v": 53.5584390243903}]}, {"c" : [{"v": 4.22110454112488}, {"v": 51.5330731707317}]}, {"c" : [{"v": 4.42912961093512}, {"v": 80.3878048780488}]}, {"c" : [{"v": 3.86721205285359}, {"v": 72.3368780487805}]}, {"c" : [{"v": NaN}, {"v": 70.8368292682927}]}, {"c" : [{"v": 3.4597521436143}, {"v": 70.9959756097561}]}, {"c" : [{"v": NaN}, {"v": 78.1008536585366}]}, {"c" : [{"v": 3.45847951159474}, {"v": 65.7024878048781}]}, {"c" : [{"v": 4.51255096930315}, {"v": 82.9780487804878}]}, {"c" : [{"v": 3.31771362045254}, {"v": 72.8503170731707}]}, {"c" : [{"v": 4.13036125458511}, {"v": 76.4756097560976}]}, {"c" : [{"v": 2.82554744973666}, {"v": 61.866756097561}]}, {"c" : [{"v": 4.11255575045033}, {"v": 74.2073170731707}]}, {"c" : [{"v": 3.46932944720763}, {"v": 70.1678536585366}]}, {"c" : [{"v": NaN}, {"v": NaN}]}, {"c" : [{"v": 3.15139241145955}, {"v": 65.694243902439}]}, {"c" : [{"v": 4.68034312292398}, {"v": 80.7439024390244}]}, {"c" : [{"v": 3.75396004416416}, {"v": 73.1301463414634}]}, {"c" : [{"v": 3.65066787103657}, {"v": 68.829756097561}]}, {"c" : [{"v": 4.62009432789643}, {"v": 81.8975609756098}]}, {"c" : [{"v": 4.48502429871213}, {"v": 81.6024390243903}]}, {"c" : [{"v": 4.55480070510122}, {"v": 82.0365853658537}]}, {"c" : [{"v": 3.69170225758919}, {"v": 72.8471219512195}]}, {"c" : [{"v": 3.6405530891281}, {"v": 73.4358780487805}]}, {"c" : [{"v": 4.63255088696564}, {"v": 82.8426829268293}]}, {"c" : [{"v": 3.95763840824412}, {"v": 68.2953658536585}]}, {"c" : [{"v": 2.9902405756245}, {"v": 59.5470731707317}]}, {"c" : [{"v": 2.94450131439298}, {"v": 69.3}]}, {"c" : [{"v": 2.89355032543616}, {"v": 70.6433658536585}]}, {"c" : [{"v": 3.18725200577384}, {"v": 67.8838292682927}]}, {"c" : [{"v": 4.12145978773931}, {"v": NaN}]}, {"c" : [{"v": 4.34539743718287}, {"v": 80.5512195121951}]}, {"c" : [{"v": 3.51632770847895}, {"v": 69.9}]}, {"c" : [{"v": 4.586412647127}, {"v": 74.1619268292683}]}, {"c" : [{"v": 3.05032263894858}, {"v": 66.8984390243902}]}, {"c" : [{"v": 3.94229831129232}, {"v": 79.2527804878049}]}, {"c" : [{"v": 2.51402187226068}, {"v": 59.4343170731707}]}, {"c" : [{"v": 4.09256098323882}, {"v": 74.7924878048781}]}, {"c" : [{"v": 3.84597792513049}, {"v": 74.4110243902439}]}, {"c" : [{"v": NaN}, {"v": 81.8414634146342}]}, {"c" : [{"v": 3.38021406008774}, {"v": 73.7552682926829}]}, {"c" : [{"v": 3.03463459756735}, {"v": 47.4834146341463}]}, {"c" : [{"v": 4.07379785750019}, {"v": 73.2682926829268}]}, {"c" : [{"v": 5.01223373381129}, {"v": 80.6317073170732}]}, {"c" : [{"v": 4.0586730228697}, {"v": 73.4829268292683}]}, {"c" : [{"v": 4.72465165056731}, {"v": 79.6929512195122}]}, {"c" : [{"v": NaN}, {"v": 78.7219512195122}]}, {"c" : [{"v": 3.45066991425313}, {"v": 70.1715853658537}]}, {"c" : [{"v": 5.16205585262261}, {"v": NaN}]}, {"c" : [{"v": 3.21259661620548}, {"v": 68.4581219512195}]}, {"c" : [{"v": 2.61715010201412}, {"v": 63.3497317073171}]}, {"c" : [{"v": 3.81640600167079}, {"v": 76.7863170731707}]}, {"c" : [{"v": 3.9503983997119}, {"v": 76.6902926829268}]}, {"c" : [{"v": 3.49506070627474}, {"v": NaN}]}, {"c" : [{"v": 3.64760788151167}, {"v": 74.7221463414634}]}, {"c" : [{"v": 2.82846310584792}, {"v": 53.7707317073171}]}, {"c" : [{"v": 4.29436162407677}, {"v": 81.3975609756098}]}, {"c" : [{"v": NaN}, {"v": 64.583}]}, {"c" : [{"v": 3.82191099369031}, {"v": 74.4167317073171}]}, {"c" : [{"v": 3.35900884962251}, {"v": 66.8946097560976}]}, {"c" : [{"v": NaN}, {"v": NaN}]}, {"c" : [{"v": 2.62750401192575}, {"v": 49.1370731707317}]}, {"c" : [{"v": 2.98996158802392}, {"v": 61.023243902439}]}, {"c" : [{"v": 3.89053838298771}, {"v": 72.9673170731707}]}, {"c" : [{"v": 2.55579495660137}, {"v": 53.4657073170732}]}, {"c" : [{"v": 3.94221858862864}, {"v": 74.4955853658537}]}, {"c" : [{"v": 3.71413530481498}, {"v": 62.4802926829268}]}, {"c" : [{"v": NaN}, {"v": 77.4731707317073}]}, {"c" : [{"v": 2.55606265810496}, {"v": 56.9856341463415}]}, {"c" : [{"v": 3.36377373250075}, {"v": 51.2894146341464}]}, {"c" : [{"v": 3.18616274583556}, {"v": 73.7951219512195}]}, {"c" : [{"v": 4.70189815495529}, {"v": 80.7024390243902}]}, {"c" : [{"v": 4.93498366082552}, {"v": 80.9975609756098}]}, {"c" : [{"v": 2.77507981590477}, {"v": 67.1049268292683}]}, {"c" : [{"v": 4.51819210321115}, {"v": 80.7024390243902}]}, {"c" : [{"v": 4.32061683070223}, {"v": 76.0458048780488}]}, {"c" : [{"v": 3.00995873017016}, {"v": 66.1263414634146}]}, {"c" : [{"v": 3.89397822031961}, {"v": 76.9487073170732}]}, {"c" : [{"v": 3.70547683585676}, {"v": 73.9050975609756}]}, {"c" : [{"v": 3.32958465851313}, {"v": 68.2302682926829}]}, {"c" : [{"v": 3.98457831703857}, {"v": NaN}]}, {"c" : [{"v": 3.15128272309135}, {"v": 62.0078048780488}]}, {"c" : [{"v": 4.0963563047742}, {"v": 76.2463414634146}]}, {"c" : [{"v": 4.42222846977193}, {"v": 78.182243902439}]}, {"c" : [{"v": NaN}, {"v": 68.8979268292683}]}, {"c" : [{"v": 4.35292797807064}, {"v": 79.0268292682927}]}, {"c" : [{"v": 3.49147867401973}, {"v": 72.0266585365854}]}, {"c" : [{"v": 3.36897820674482}, {"v": 72.6404878048781}]}, {"c" : [{"v": NaN}, {"v": 75.6933902439024}]}, {"c" : [{"v": 4.8543677239179}, {"v": 78.1460487804878}]}, {"c" : [{"v": 3.91057887485493}, {"v": 73.4585365853659}]}, {"c" : [{"v": 4.02978011834887}, {"v": 68.8560975609756}]}, {"c" : [{"v": 2.72086594806141}, {"v": 62.2121463414634}]}, {"c" : [{"v": 4.28615506619628}, {"v": 75.0756097560976}]}, {"c" : [{"v": 3.15821866398759}, {"v": 61.4782926829268}]}, {"c" : [{"v": 2.99939137748647}, {"v": 62.8418780487805}]}, {"c" : [{"v": 4.66810333231918}, {"v": 81.5414634146342}]}, {"c" : [{"v": 3.11216671007281}, {"v": 67.0666585365854}]}, {"c" : [{"v": 2.65149271579896}, {"v": 44.8389512195122}]}, {"c" : [{"v": 3.53712066009755}, {"v": 71.6344146341464}]}, {"c" : [{"v": NaN}, {"v": 83.1593791574279}]}, {"c" : [{"v": NaN}, {"v": 54.0236585365854}]}, {"c" : [{"v": 3.73233726727101}, {"v": 74.3365853658537}]}, {"c" : [{"v": 3.19924127373873}, {"v": 53.465}]}, {"c" : [{"v": 3.05230204122627}, {"v": 65.8536829268293}]}, {"c" : [{"v": 3.92019599129272}, {"v": 70.3358292682927}]}, {"c" : [{"v": 4.21774435527048}, {"v": 75.1121951219512}]}, {"c" : [{"v": 4.36954321163856}, {"v": 79.4219512195122}]}, {"c" : [{"v": 4.71663975254623}, {"v": 81.4512195121951}]}, {"c" : [{"v": 3.51343110408463}, {"v": 48.345756097561}]}, {"c" : [{"v": NaN}, {"v": NaN}]}, {"c" : [{"v": 4.03514240662378}, {"v": 73.1975609756098}]}, {"c" : [{"v": NaN}, {"v": 74.8676341463415}]}, {"c" : [{"v": NaN}, {"v": NaN}]}, {"c" : [{"v": 2.95870716554551}, {"v": 49.7698536585366}]}, {"c" : [{"v": 2.70170768279748}, {"v": 55.4688780487805}]}, {"c" : [{"v": 3.68148209160907}, {"v": 73.8137804878049}]}, {"c" : [{"v": 2.86907451882178}, {"v": 66.995512195122}]}, {"c" : [{"v": 3.64273348022657}, {"v": 65.0182195121951}]}, {"c" : [{"v": 2.94242307456375}, {"v": 65.9444634146342}]}, {"c" : [{"v": 3.54983398386194}, {"v": 72.1826341463415}]}, {"c" : [{"v": 4.19396037993529}, {"v": 69.5973658536585}]}, {"c" : [{"v": 3.62450430974798}, {"v": 74.6024390243903}]}, {"c" : [{"v": 4.00585584256221}, {"v": 74.2111951219512}]}, {"c" : [{"v": 3.51034337979981}, {"v": NaN}]}, {"c" : [{"v": 2.84998861933567}, {"v": 59.1817073170732}]}, {"c" : [{"v": 2.74293128179306}, {"v": 57.2965365853659}]}, {"c" : [{"v": 3.47334045026304}, {"v": 70.2653658536586}]}, {"c" : [{"v": 4.06185331902522}, {"v": 76.6162195121951}]}, {"c" : [{"v": 4.68464246863556}, {"v": 78.5414634146342}]}, {"c" : [{"v": 3.13895984591317}, {"v": 67.858756097561}]}, {"c" : [{"v": 3.79460745951819}, {"v": 72.1849268292683}]}, {"c" : [{"v": 4.13223172210138}, {"v": 74.1704634146342}]}, {"c" : [{"v": NaN}, {"v": 79.1731707317073}]}, {"c" : [{"v": 3.12502022204993}, {"v": 75.3117317073171}]}, {"c" : [{"v": 3.47213486564455}, {"v": 70.8389024390244}]}, {"c" : [{"v": 3.53867044148135}, {"v": 72.4080243902439}]}, {"c" : [{"v": 3.14442859514843}, {"v": 62.5276585365854}]}, {"c" : [{"v": 3.86864210316628}, {"v": 54.390756097561}]}, {"c" : [{"v": 3.18562270443947}, {"v": 54.5275609756098}]}, {"c" : [{"v": 2.85923718779753}, {"v": 53.5931219512195}]}]});
  var options = {"colors":["#3B8FCC"],"hAxis":{"title":"Log of GDP (per capita)"},"legend":{"position":"none"},"pointSize":3,"vAxis":{"title":"Life expectancy (years)"},"trendlines":[{"color":"#C0D9EA","lineWidth":10,"opacity":0.5}]} 
  var chart = new google.visualization.ScatterChart(document.getElementById('58c7d0e5-50a4-4045-ae9c-c3a8623a927e'));
  chart.draw(data, options);
}
</script>
<div id="58c7d0e5-50a4-4045-ae9c-c3a8623a927e" style="margin-left:auto;margin-right:auto;width: 500px; height: 350px;"></div>

If we look at the trendline, we can roughly say that countries with life expectancy greater _by 10 years_
have _10 times_ larger GDP per capita. (That said, we are looking just at the data from 2010 here and we 
are also not checking any statistical significance, just building an interesting visualization!)

Visualization #5: EU is getting older
-------------------------------------

Another interesting fact we can nicely visualize is how the EU is getting older. We look at the enitre European
Union here (which contains more countries than just the Euro area). In this visualization, we build three overlaying
histograms that show the percentage of population over 65 years. To do that, we use Plotly, which lets us nicely
compose histograms.
*)
let getGrowths y = 
  let data = [ for c in wb.Regions.``European Union``.Countries -> 
    c.Indicators.``Population ages 65 and above (% of total)``.[y] ]
  Histogram(x=data, name=string y, opacity=0.6)

Figure
  ( Data.From(List.map getGrowths [1960; 1985; 2010]), 
    Layout(barmode="overlay",
      title="Population ages 65 and above (% of total)"))
(**
<iframe width="600" height="350" frameborder="0" seamless="seamless" style="margin-left:auto;margin-right:auto;"
           scrolling="no" src="https://plot.ly/~tomasp/271.embed?width=600&height=350"></iframe>

We can see that in 1960, most countries had 10-12% of population over the age of 65 (with just one
country having 12-14%). 25 years later, in 1985, most countries had 12-14% of elderly population.
In 2010, most countries have 16-18% of population over the age of 65 and for some countries, this
is even 20-20%.

Visualization #6: World's biggest polluters
-------------------------------------------

For the next two visualizations, we'll change the topic and look at green issues rather than the
Europe. The XPlot wrapper for Google Charts has a nice wrapper for creating geo charts, and so
we can easily take the CO2 emissions indicator from the World Bank and plot the biggest polluters,
based on the values from 2010, on a map:
*)
let emissions =
 [ for c in wb.Countries do
   let v = c.Indicators.``CO2 emissions (kt)``.[2010]
   if not(Double.IsNaN(v)) then yield c.Name, v ]

Chart.Geo(emissions)
|> Chart.WithLabels ["Name"; "Emissions (total kt)"]
|> Chart.WithOptions
    (Options(colorAxis=ColorAxis(colors=(*[omit:(color scale)]*)[| "#6CC627"; "#DB9B3B"; "#DB7532"; "#DD5321"; "#DB321C"; "#E00B00" |](*[/omit]*))))
(**
<script type="text/javascript">
google.setOnLoadCallback(drawChart4);
function drawChart4() {
  var data = new google.visualization.DataTable({"cols": [{"type": "string" ,"id": "Name" ,"label": "Name" }, {"type": "number" ,"id": "Emissions (total kt)" ,"label": "Emissions (per capita)" }], "rows" : [{"c" : [{"v": "Aruba"}, {"v": 2321.211}]}, {"c" : [{"v": "Afghanistan"}, {"v": 8236.082}]}, {"c" : [{"v": "Angola"}, {"v": 30417.765}]}, {"c" : [{"v": "Albania"}, {"v": 4283.056}]}, {"c" : [{"v": "Andorra"}, {"v": 517.047}]}, {"c" : [{"v": "United Arab Emirates"}, {"v": 167596.568}]}, {"c" : [{"v": "Argentina"}, {"v": 180511.742}]}, {"c" : [{"v": "Armenia"}, {"v": 4220.717}]}, {"c" : [{"v": "Antigua and Barbuda"}, {"v": 513.38}]}, {"c" : [{"v": "Australia"}, {"v": 373080.58}]}, {"c" : [{"v": "Austria"}, {"v": 66897.081}]}, {"c" : [{"v": "Azerbaijan"}, {"v": 45731.157}]}, {"c" : [{"v": "Burundi"}, {"v": 308.028}]}, {"c" : [{"v": "Belgium"}, {"v": 108946.57}]}, {"c" : [{"v": "Benin"}, {"v": 5188.805}]}, {"c" : [{"v": "Burkina Faso"}, {"v": 1683.153}]}, {"c" : [{"v": "Bangladesh"}, {"v": 56152.771}]}, {"c" : [{"v": "Bulgaria"}, {"v": 44678.728}]}, {"c" : [{"v": "Bahrain"}, {"v": 24202.2}]}, {"c" : [{"v": "Bahamas, The"}, {"v": 2464.224}]}, {"c" : [{"v": "Bosnia and Herzegovina"}, {"v": 31125.496}]}, {"c" : [{"v": "Belarus"}, {"v": 62221.656}]}, {"c" : [{"v": "Belize"}, {"v": 421.705}]}, {"c" : [{"v": "Bermuda"}, {"v": 476.71}]}, {"c" : [{"v": "Bolivia"}, {"v": 15456.405}]}, {"c" : [{"v": "Brazil"}, {"v": 419754.156}]}, {"c" : [{"v": "Barbados"}, {"v": 1503.47}]}, {"c" : [{"v": "Brunei Darussalam"}, {"v": 9160.166}]}, {"c" : [{"v": "Bhutan"}, {"v": 476.71}]}, {"c" : [{"v": "Botswana"}, {"v": 5232.809}]}, {"c" : [{"v": "Central African Republic"}, {"v": 264.024}]}, {"c" : [{"v": "Canada"}, {"v": 499137.372}]}, {"c" : [{"v": "Switzerland"}, {"v": 38756.523}]}, {"c" : [{"v": "Chile"}, {"v": 72258.235}]}, {"c" : [{"v": "China"}, {"v": 8286891.952}]}, {"c" : [{"v": "Cote d'Ivoire"}, {"v": 5804.861}]}, {"c" : [{"v": "Cameroon"}, {"v": 7234.991}]}, {"c" : [{"v": "Congo, Dem. Rep."}, {"v": 3039.943}]}, {"c" : [{"v": "Congo, Rep."}, {"v": 2027.851}]}, {"c" : [{"v": "Colombia"}, {"v": 75679.546}]}, {"c" : [{"v": "Comoros"}, {"v": 139.346}]}, {"c" : [{"v": "Cabo Verde"}, {"v": 355.699}]}, {"c" : [{"v": "Costa Rica"}, {"v": 7770.373}]}, {"c" : [{"v": "Cuba"}, {"v": 38364.154}]}, {"c" : [{"v": "Cayman Islands"}, {"v": 590.387}]}, {"c" : [{"v": "Cyprus"}, {"v": 7708.034}]}, {"c" : [{"v": "Czech Republic"}, {"v": 111751.825}]}, {"c" : [{"v": "Germany"}, {"v": 745383.756}]}, {"c" : [{"v": "Djibouti"}, {"v": 539.049}]}, {"c" : [{"v": "Dominica"}, {"v": 135.679}]}, {"c" : [{"v": "Denmark"}, {"v": 46303.209}]}, {"c" : [{"v": "Dominican Republic"}, {"v": 20964.239}]}, {"c" : [{"v": "Algeria"}, {"v": 123475.224}]}, {"c" : [{"v": "Ecuador"}, {"v": 32636.3}]}, {"c" : [{"v": "Egypt, Arab Rep."}, {"v": 204776.281}]}, {"c" : [{"v": "Eritrea"}, {"v": 513.38}]}, {"c" : [{"v": "Spain"}, {"v": 269674.847}]}, {"c" : [{"v": "Estonia"}, {"v": 18338.667}]}, {"c" : [{"v": "Ethiopia"}, {"v": 6494.257}]}, {"c" : [{"v": "Finland"}, {"v": 61843.955}]}, {"c" : [{"v": "Fiji"}, {"v": 1290.784}]}, {"c" : [{"v": "France"}, {"v": 361272.84}]}, {"c" : [{"v": "Faeroe Islands"}, {"v": 711.398}]}, {"c" : [{"v": "Micronesia, Fed. Sts."}, {"v": 102.676}]}, {"c" : [{"v": "Gabon"}, {"v": 2574.234}]}, {"c" : [{"v": "United Kingdom"}, {"v": 493504.86}]}, {"c" : [{"v": "Georgia"}, {"v": 6241.234}]}, {"c" : [{"v": "Ghana"}, {"v": 8998.818}]}, {"c" : [{"v": "Guinea"}, {"v": 1235.779}]}, {"c" : [{"v": "Gambia, The"}, {"v": 473.043}]}, {"c" : [{"v": "Guinea-Bissau"}, {"v": 238.355}]}, {"c" : [{"v": "Equatorial Guinea"}, {"v": 4679.092}]}, {"c" : [{"v": "Greece"}, {"v": 86717.216}]}, {"c" : [{"v": "Grenada"}, {"v": 260.357}]}, {"c" : [{"v": "Greenland"}, {"v": 634.391}]}, {"c" : [{"v": "Guatemala"}, {"v": 11118.344}]}, {"c" : [{"v": "Guyana"}, {"v": 1701.488}]}, {"c" : [{"v": "Hong Kong SAR, China"}, {"v": 36288.632}]}, {"c" : [{"v": "Honduras"}, {"v": 8107.737}]}, {"c" : [{"v": "Croatia"}, {"v": 20883.565}]}, {"c" : [{"v": "Haiti"}, {"v": 2119.526}]}, {"c" : [{"v": "Hungary"}, {"v": 50582.598}]}, {"c" : [{"v": "Indonesia"}, {"v": 433989.45}]}, {"c" : [{"v": "India"}, {"v": 2008822.937}]}, {"c" : [{"v": "Ireland"}, {"v": 39999.636}]}, {"c" : [{"v": "Iran, Islamic Rep."}, {"v": 571611.96}]}, {"c" : [{"v": "Iraq"}, {"v": 114667.09}]}, {"c" : [{"v": "Iceland"}, {"v": 1961.845}]}, {"c" : [{"v": "Israel"}, {"v": 70655.756}]}, {"c" : [{"v": "Italy"}, {"v": 406307.267}]}, {"c" : [{"v": "Jamaica"}, {"v": 7157.984}]}, {"c" : [{"v": "Jordan"}, {"v": 20821.226}]}, {"c" : [{"v": "Japan"}, {"v": 1170715.419}]}, {"c" : [{"v": "Kazakhstan"}, {"v": 248728.943}]}, {"c" : [{"v": "Kenya"}, {"v": 12427.463}]}, {"c" : [{"v": "Kyrgyz Republic"}, {"v": 6398.915}]}, {"c" : [{"v": "Cambodia"}, {"v": 4180.38}]}, {"c" : [{"v": "Kiribati"}, {"v": 62.339}]}, {"c" : [{"v": "St. Kitts and Nevis"}, {"v": 249.356}]}, {"c" : [{"v": "Korea, Rep."}, {"v": 567567.259}]}, {"c" : [{"v": "Kuwait"}, {"v": 93695.517}]}, {"c" : [{"v": "Lao PDR"}, {"v": 1873.837}]}, {"c" : [{"v": "Lebanon"}, {"v": 20403.188}]}, {"c" : [{"v": "Liberia"}, {"v": 799.406}]}, {"c" : [{"v": "Libya"}, {"v": 59035.033}]}, {"c" : [{"v": "St. Lucia"}, {"v": 403.37}]}, {"c" : [{"v": "Sri Lanka"}, {"v": 12709.822}]}, {"c" : [{"v": "Lesotho"}, {"v": 18.335}]}, {"c" : [{"v": "Lithuania"}, {"v": 13560.566}]}, {"c" : [{"v": "Luxembourg"}, {"v": 10828.651}]}, {"c" : [{"v": "Latvia"}, {"v": 7616.359}]}, {"c" : [{"v": "Macao SAR, China"}, {"v": 1030.427}]}, {"c" : [{"v": "Morocco"}, {"v": 50608.267}]}, {"c" : [{"v": "Moldova"}, {"v": 4855.108}]}, {"c" : [{"v": "Madagascar"}, {"v": 2013.183}]}, {"c" : [{"v": "Maldives"}, {"v": 1074.431}]}, {"c" : [{"v": "Mexico"}, {"v": 443673.997}]}, {"c" : [{"v": "Marshall Islands"}, {"v": 102.676}]}, {"c" : [{"v": "Macedonia, FYR"}, {"v": 10872.655}]}, {"c" : [{"v": "Mali"}, {"v": 623.39}]}, {"c" : [{"v": "Malta"}, {"v": 2588.902}]}, {"c" : [{"v": "Myanmar"}, {"v": 8995.151}]}, {"c" : [{"v": "Montenegro"}, {"v": 2581.568}]}, {"c" : [{"v": "Mongolia"}, {"v": 11510.713}]}, {"c" : [{"v": "Mozambique"}, {"v": 2882.262}]}, {"c" : [{"v": "Mauritania"}, {"v": 2214.868}]}, {"c" : [{"v": "Mauritius"}, {"v": 4118.041}]}, {"c" : [{"v": "Malawi"}, {"v": 1239.446}]}, {"c" : [{"v": "Malaysia"}, {"v": 216804.041}]}, {"c" : [{"v": "Namibia"}, {"v": 3175.622}]}, {"c" : [{"v": "New Caledonia"}, {"v": 3920.023}]}, {"c" : [{"v": "Niger"}, {"v": 1411.795}]}, {"c" : [{"v": "Nigeria"}, {"v": 78910.173}]}, {"c" : [{"v": "Nicaragua"}, {"v": 4547.08}]}, {"c" : [{"v": "Netherlands"}, {"v": 182077.551}]}, {"c" : [{"v": "Norway"}, {"v": 57186.865}]}, {"c" : [{"v": "Nepal"}, {"v": 3755.008}]}, {"c" : [{"v": "New Zealand"}, {"v": 31550.868}]}, {"c" : [{"v": "Oman"}, {"v": 57201.533}]}, {"c" : [{"v": "Pakistan"}, {"v": 161395.671}]}, {"c" : [{"v": "Panama"}, {"v": 9633.209}]}, {"c" : [{"v": "Peru"}, {"v": 57579.234}]}, {"c" : [{"v": "Philippines"}, {"v": 81590.75}]}, {"c" : [{"v": "Palau"}, {"v": 216.353}]}, {"c" : [{"v": "Papua New Guinea"}, {"v": 3135.285}]}, {"c" : [{"v": "Poland"}, {"v": 317254.172}]}, {"c" : [{"v": "Korea, Dem. Rep."}, {"v": 71623.844}]}, {"c" : [{"v": "Portugal"}, {"v": 52361.093}]}, {"c" : [{"v": "Paraguay"}, {"v": 5075.128}]}, {"c" : [{"v": "West Bank and Gaza"}, {"v": 2365.215}]}, {"c" : [{"v": "French Polynesia"}, {"v": 883.747}]}, {"c" : [{"v": "Qatar"}, {"v": 70531.078}]}, {"c" : [{"v": "Romania"}, {"v": 78745.158}]}, {"c" : [{"v": "Russian Federation"}, {"v": 1740776.238}]}, {"c" : [{"v": "Rwanda"}, {"v": 594.054}]}, {"c" : [{"v": "Saudi Arabia"}, {"v": 464480.555}]}, {"c" : [{"v": "Sudan"}, {"v": 14172.955}]}, {"c" : [{"v": "Senegal"}, {"v": 7058.975}]}, {"c" : [{"v": "Singapore"}, {"v": 13520.229}]}, {"c" : [{"v": "Solomon Islands"}, {"v": 201.685}]}, {"c" : [{"v": "Sierra Leone"}, {"v": 689.396}]}, {"c" : [{"v": "El Salvador"}, {"v": 6248.568}]}, {"c" : [{"v": "Somalia"}, {"v": 608.722}]}, {"c" : [{"v": "Serbia"}, {"v": 45962.178}]}, {"c" : [{"v": "Sao Tome and Principe"}, {"v": 99.009}]}, {"c" : [{"v": "Suriname"}, {"v": 2383.55}]}, {"c" : [{"v": "Slovak Republic"}, {"v": 36094.281}]}, {"c" : [{"v": "Slovenia"}, {"v": 15328.06}]}, {"c" : [{"v": "Sweden"}, {"v": 52515.107}]}, {"c" : [{"v": "Swaziland"}, {"v": 1023.093}]}, {"c" : [{"v": "Seychelles"}, {"v": 704.064}]}, {"c" : [{"v": "Syrian Arab Republic"}, {"v": 61858.623}]}, {"c" : [{"v": "Turks and Caicos Islands"}, {"v": 161.348}]}, {"c" : [{"v": "Chad"}, {"v": 469.376}]}, {"c" : [{"v": "Togo"}, {"v": 1540.14}]}, {"c" : [{"v": "Thailand"}, {"v": 295281.508}]}, {"c" : [{"v": "Tajikistan"}, {"v": 2860.26}]}, {"c" : [{"v": "Turkmenistan"}, {"v": 53054.156}]}, {"c" : [{"v": "Timor-Leste"}, {"v": 183.35}]}, {"c" : [{"v": "Tonga"}, {"v": 157.681}]}, {"c" : [{"v": "Trinidad and Tobago"}, {"v": 50681.607}]}, {"c" : [{"v": "Tunisia"}, {"v": 25878.019}]}, {"c" : [{"v": "Turkey"}, {"v": 298002.422}]}, {"c" : [{"v": "Tanzania"}, {"v": 6846.289}]}, {"c" : [{"v": "Uganda"}, {"v": 3784.344}]}, {"c" : [{"v": "Ukraine"}, {"v": 304804.707}]}, {"c" : [{"v": "Uruguay"}, {"v": 6644.604}]}, {"c" : [{"v": "United States"}, {"v": 5433056.536}]}, {"c" : [{"v": "Uzbekistan"}, {"v": 104443.494}]}, {"c" : [{"v": "St. Vincent and the Grenadines"}, {"v": 209.019}]}, {"c" : [{"v": "Venezuela, RB"}, {"v": 201747.339}]}, {"c" : [{"v": "Vietnam"}, {"v": 150229.656}]}, {"c" : [{"v": "Vanuatu"}, {"v": 117.344}]}, {"c" : [{"v": "Samoa"}, {"v": 161.348}]}, {"c" : [{"v": "Yemen, Rep."}, {"v": 21851.653}]}, {"c" : [{"v": "South Africa"}, {"v": 460124.159}]}, {"c" : [{"v": "Zambia"}, {"v": 2427.554}]}, {"c" : [{"v": "Zimbabwe"}, {"v": 9427.857}]}]});
  var options = {"legend":{"position":"none"},"colorAxis":{"colors":["#6CC627","#DB9B3B","#DB7532","#DD5321","#DB321C","#E00B00"]}} 
  var chart = new google.visualization.GeoChart(document.getElementById('ec04665f-9973-46ad-877c-e1ffd99f932e'));
  chart.draw(data, options);
}
</script>
<div id="ec04665f-9973-46ad-877c-e1ffd99f932e" style="margin-left:auto;margin-right:auto;width: 500px; height: 350px;"></div>

This shows the expected results. The world's biggest polluters are China, followed by USA, 
India and Russia. The next polluters are smaller and include Germany, UK, Canada and Brazil.
This indicator returns the total number of CO2 emissions in kilotons, so larger countries 
are bigger polluters. What if we look at emissions per capita?

Visualization #7: CO2 emissions per capita
------------------------------------------

The World Bank data set does not directly include an indicator for CO2 emissions per capita,
but it contains CO2 emissions and total population, so we can do the math ourselves. The following
is very similar to the previous snippet, except that we get both of the indicators return
the ratio:
*)
let emissionPerCapita =
 [ for c in wb.Countries do
   let emissions = c.Indicators.``CO2 emissions (kt)``.[2010]
   let population = c.Indicators.``Population, total``.[2010]
   if not(Double.IsNaN(emissions)) then
     yield c.Name, emissions / population ]

emissionPerCapita |> List.sortBy snd |> List.rev

Chart.Geo(emissionPerCapita)
|> Chart.WithLabels ["Name"; "Emissions (per capita)"]
|> Chart.WithOptions
    (Options(colorAxis=ColorAxis(colors=(*[omit:(color scale)]*)[|"#6CC627"; "#DD5321"; "#E00B00"|](*[/omit]*))))
(**
<script type="text/javascript">
  google.setOnLoadCallback(drawChart2);
  function drawChart2() {
      var data = new google.visualization.DataTable({"cols": [{"type": "string" ,"id": "Name" ,"label": "Name" }, {"type": "number" ,"id": "Emissions (per capita)" ,"label": "Emissions (per capita)" }], "rows" : [{"c" : [{"v": "Aruba"}, {"v": 0.022847239583846}]}, {"c" : [{"v": "Afghanistan"}, {"v": 0.000290025231521358}]}, {"c" : [{"v": "Angola"}, {"v": 0.00155596562792277}]}, {"c" : [{"v": "Albania"}, {"v": 0.00149931616254293}]}, {"c" : [{"v": "Andorra"}, {"v": 0.00663672070545651}]}, {"c" : [{"v": "United Arab Emirates"}, {"v": 0.0198537977148}]}, {"c" : [{"v": "Argentina"}, {"v": 0.00447096498994012}]}, {"c" : [{"v": "Armenia"}, {"v": 0.00142423576748543}]}, {"c" : [{"v": "Antigua and Barbuda"}, {"v": 0.00588515813969484}]}, {"c" : [{"v": "Australia"}, {"v": 0.0169337312430215}]}, {"c" : [{"v": "Austria"}, {"v": 0.00797364802924895}]}, {"c" : [{"v": "Azerbaijan"}, {"v": 0.00505074885701121}]}, {"c" : [{"v": "Burundi"}, {"v": 3.33625301142574E-05}]}, {"c" : [{"v": "Belgium"}, {"v": 0.00997654362455441}]}, {"c" : [{"v": "Benin"}, {"v": 0.000545627257277179}]}, {"c" : [{"v": "Burkina Faso"}, {"v": 0.000108309024468279}]}, {"c" : [{"v": "Bangladesh"}, {"v": 0.00037156390079171}]}, {"c" : [{"v": "Bulgaria"}, {"v": 0.00604125886219629}]}, {"c" : [{"v": "Bahrain"}, {"v": 0.019338352857701}]}, {"c" : [{"v": "Bahamas, The"}, {"v": 0.00683561073847844}]}, {"c" : [{"v": "Bosnia and Herzegovina"}, {"v": 0.00809310208274776}]}, {"c" : [{"v": "Belarus"}, {"v": 0.00655654963119073}]}, {"c" : [{"v": "Belize"}, {"v": 0.00136653218619874}]}, {"c" : [{"v": "Bermuda"}, {"v": 0.0073200356243474}]}, {"c" : [{"v": "Bolivia"}, {"v": 0.00152180882167174}]}, {"c" : [{"v": "Brazil"}, {"v": 0.00215026804394612}]}, {"c" : [{"v": "Barbados"}, {"v": 0.00536195238163169}]}, {"c" : [{"v": "Brunei Darussalam"}, {"v": 0.0228678854329716}]}, {"c" : [{"v": "Bhutan"}, {"v": 0.000664924073038292}]}, {"c" : [{"v": "Botswana"}, {"v": 0.00265713708291251}]}, {"c" : [{"v": "Central African Republic"}, {"v": 6.06962747139546E-05}]}, {"c" : [{"v": "Canada"}, {"v": 0.0146782340880418}]}, {"c" : [{"v": "Switzerland"}, {"v": 0.00495296788754987}]}, {"c" : [{"v": "Chile"}, {"v": 0.00421312145934058}]}, {"c" : [{"v": "China"}, {"v": 0.00619485757472686}]}, {"c" : [{"v": "Cote d'Ivoire"}, {"v": 0.000305895928182664}]}, {"c" : [{"v": "Cameroon"}, {"v": 0.000350798616954732}]}, {"c" : [{"v": "Congo, Dem. Rep."}, {"v": 4.88806279078791E-05}]}, {"c" : [{"v": "Congo, Rep."}, {"v": 0.000493188608646271}]}, {"c" : [{"v": "Colombia"}, {"v": 0.00162945150498878}]}, {"c" : [{"v": "Comoros"}, {"v": 0.000203996304977009}]}, {"c" : [{"v": "Cabo Verde"}, {"v": 0.000729487839442495}]}, {"c" : [{"v": "Costa Rica"}, {"v": 0.00166400367476607}]}, {"c" : [{"v": "Cuba"}, {"v": 0.00340054448912617}]}, {"c" : [{"v": "Cayman Islands"}, {"v": 0.0106358788664901}]}, {"c" : [{"v": "Cyprus"}, {"v": 0.00698390754608425}]}, {"c" : [{"v": "Czech Republic"}, {"v": 0.0106690329097295}]}, {"c" : [{"v": "Germany"}, {"v": 0.00911484150847923}]}, {"c" : [{"v": "Djibouti"}, {"v": 0.000646313828180079}]}, {"c" : [{"v": "Dominica"}, {"v": 0.00190648755743533}]}, {"c" : [{"v": "Denmark"}, {"v": 0.00834640497663619}]}, {"c" : [{"v": "Dominican Republic"}, {"v": 0.00209290844169049}]}, {"c" : [{"v": "Algeria"}, {"v": 0.0033315118493412}]}, {"c" : [{"v": "Ecuador"}, {"v": 0.00217559785060694}]}, {"c" : [{"v": "Egypt, Arab Rep."}, {"v": 0.00262279131517288}]}, {"c" : [{"v": "Eritrea"}, {"v": 8.94209688322515E-05}]}, {"c" : [{"v": "Spain"}, {"v": 0.00578988434974533}]}, {"c" : [{"v": "Estonia"}, {"v": 0.0137731966428209}]}, {"c" : [{"v": "Ethiopia"}, {"v": 7.45649698288476E-05}]}, {"c" : [{"v": "Finland"}, {"v": 0.0115308402282752}]}, {"c" : [{"v": "Fiji"}, {"v": 0.00149993666907208}]}, {"c" : [{"v": "France"}, {"v": 0.00555606556201175}]}, {"c" : [{"v": "Faeroe Islands"}, {"v": 0.0143481978983885}]}, {"c" : [{"v": "Micronesia, Fed. Sts."}, {"v": 0.000990899352435364}]}, {"c" : [{"v": "Gabon"}, {"v": 0.00165415602658233}]}, {"c" : [{"v": "United Kingdom"}, {"v": 0.00786256874999851}]}, {"c" : [{"v": "Georgia"}, {"v": 0.00140164256198347}]}, {"c" : [{"v": "Ghana"}, {"v": 0.000370887965952629}]}, {"c" : [{"v": "Guinea"}, {"v": 0.000113624057595265}]}, {"c" : [{"v": "Gambia, The"}, {"v": 0.000281465989146992}]}, {"c" : [{"v": "Guinea-Bissau"}, {"v": 0.0001502277792344}]}, {"c" : [{"v": "Equatorial Guinea"}, {"v": 0.00672122062665998}]}, {"c" : [{"v": "Greece"}, {"v": 0.00777492030719811}]}, {"c" : [{"v": "Grenada"}, {"v": 0.00248724170543672}]}, {"c" : [{"v": "Greenland"}, {"v": 0.0111482470784641}]}, {"c" : [{"v": "Guatemala"}, {"v": 0.000775252594275552}]}, {"c" : [{"v": "Guyana"}, {"v": 0.00216439603829412}]}, {"c" : [{"v": "Hong Kong SAR, China"}, {"v": 0.00516622989094844}]}, {"c" : [{"v": "Honduras"}, {"v": 0.00106383938810718}]}, {"c" : [{"v": "Croatia"}, {"v": 0.00472716166781468}]}, {"c" : [{"v": "Haiti"}, {"v": 0.000214171415868397}]}, {"c" : [{"v": "Hungary"}, {"v": 0.00505824816602922}]}, {"c" : [{"v": "Indonesia"}, {"v": 0.00180320669881813}]}, {"c" : [{"v": "India"}, {"v": 0.00166620924707571}]}, {"c" : [{"v": "Ireland"}, {"v": 0.00877155184418073}]}, {"c" : [{"v": "Iran, Islamic Rep."}, {"v": 0.0076765269475778}]}, {"c" : [{"v": "Iraq"}, {"v": 0.00370343268185456}]}, {"c" : [{"v": "Iceland"}, {"v": 0.00616852858593703}]}, {"c" : [{"v": "Israel"}, {"v": 0.00926803032687969}]}, {"c" : [{"v": "Italy"}, {"v": 0.00685433488102223}]}, {"c" : [{"v": "Jamaica"}, {"v": 0.00266014573974366}]}, {"c" : [{"v": "Jordan"}, {"v": 0.00344380185246444}]}, {"c" : [{"v": "Japan"}, {"v": 0.00918565086532956}]}, {"c" : [{"v": "Kazakhstan"}, {"v": 0.015239267752309}]}, {"c" : [{"v": "Kenya"}, {"v": 0.000303781663359097}]}, {"c" : [{"v": "Kyrgyz Republic"}, {"v": 0.00117456542888085}]}, {"c" : [{"v": "Cambodia"}, {"v": 0.000291012884085555}]}, {"c" : [{"v": "Kiribati"}, {"v": 0.000637784803003796}]}, {"c" : [{"v": "St. Kitts and Nevis"}, {"v": 0.00476306540342298}]}, {"c" : [{"v": "Korea, Rep."}, {"v": 0.0114868054003081}]}, {"c" : [{"v": "Kuwait"}, {"v": 0.0313197430789081}]}, {"c" : [{"v": "Lao PDR"}, {"v": 0.000292983284271824}]}, {"c" : [{"v": "Lebanon"}, {"v": 0.00470001280783729}]}, {"c" : [{"v": "Liberia"}, {"v": 0.000201972718475792}]}, {"c" : [{"v": "Libya"}, {"v": 0.00977302183950898}]}, {"c" : [{"v": "St. Lucia"}, {"v": 0.00227382650213927}]}, {"c" : [{"v": "Sri Lanka"}, {"v": 0.000615398344066237}]}, {"c" : [{"v": "Lesotho"}, {"v": 9.1267899534128E-06}]}, {"c" : [{"v": "Lithuania"}, {"v": 0.00437821483481323}]}, {"c" : [{"v": "Luxembourg"}, {"v": 0.0213602661390701}]}, {"c" : [{"v": "Latvia"}, {"v": 0.00363106521640672}]}, {"c" : [{"v": "Macao SAR, China"}, {"v": 0.00192737913980989}]}, {"c" : [{"v": "Morocco"}, {"v": 0.00159938345306734}]}, {"c" : [{"v": "Moldova"}, {"v": 0.00136301141619491}]}, {"c" : [{"v": "Madagascar"}, {"v": 9.5504160149286E-05}]}, {"c" : [{"v": "Maldives"}, {"v": 0.00329889712429458}]}, {"c" : [{"v": "Mexico"}, {"v": 0.00376357223518329}]}, {"c" : [{"v": "Marshall Islands"}, {"v": 0.0019584191653315}]}, {"c" : [{"v": "Macedonia, FYR"}, {"v": 0.00517199707356428}]}, {"c" : [{"v": "Mali"}, {"v": 4.45725538631203E-05}]}, {"c" : [{"v": "Malta"}, {"v": 0.00624572263985255}]}, {"c" : [{"v": "Myanmar"}, {"v": 0.000173212743599319}]}, {"c" : [{"v": "Montenegro"}, {"v": 0.0041632955853941}]}, {"c" : [{"v": "Mongolia"}, {"v": 0.00424320852216469}]}, {"c" : [{"v": "Mozambique"}, {"v": 0.000120258277279448}]}, {"c" : [{"v": "Mauritania"}, {"v": 0.000613635431731414}]}, {"c" : [{"v": "Mauritius"}, {"v": 0.00321489877619593}]}, {"c" : [{"v": "Malawi"}, {"v": 8.25543667001605E-05}]}, {"c" : [{"v": "Malaysia"}, {"v": 0.00766746732678275}]}, {"c" : [{"v": "Namibia"}, {"v": 0.0014573979321394}]}, {"c" : [{"v": "New Caledonia"}, {"v": 0.015680092}]}, {"c" : [{"v": "Niger"}, {"v": 8.88270770150725E-05}]}, {"c" : [{"v": "Nigeria"}, {"v": 0.000494090976657493}]}, {"c" : [{"v": "Nicaragua"}, {"v": 0.000780988796520359}]}, {"c" : [{"v": "Netherlands"}, {"v": 0.0109583649355531}]}, {"c" : [{"v": "Norway"}, {"v": 0.011696444568617}]}, {"c" : [{"v": "Nepal"}, {"v": 0.000139872076363212}]}, {"c" : [{"v": "New Zealand"}, {"v": 0.00722351481294931}]}, {"c" : [{"v": "Oman"}, {"v": 0.0204089432304065}]}, {"c" : [{"v": "Pakistan"}, {"v": 0.000932118497777866}]}, {"c" : [{"v": "Panama"}, {"v": 0.00261905213739163}]}, {"c" : [{"v": "Peru"}, {"v": 0.00196765774192038}]}, {"c" : [{"v": "Philippines"}, {"v": 0.000873148290379805}]}, {"c" : [{"v": "Palau"}, {"v": 0.0105692721055203}]}, {"c" : [{"v": "Papua New Guinea"}, {"v": 0.000457108928559713}]}, {"c" : [{"v": "Poland"}, {"v": 0.00830863204054989}]}, {"c" : [{"v": "Korea, Dem. Rep."}, {"v": 0.00292336015725381}]}, {"c" : [{"v": "Portugal"}, {"v": 0.00495229336712979}]}, {"c" : [{"v": "Paraguay"}, {"v": 0.000785657461057529}]}, {"c" : [{"v": "West Bank and Gaza"}, {"v": 0.000620611833532663}]}, {"c" : [{"v": "French Polynesia"}, {"v": 0.00329676384458993}]}, {"c" : [{"v": "Qatar"}, {"v": 0.0403100839966326}]}, {"c" : [{"v": "Romania"}, {"v": 0.00388925073904012}]}, {"c" : [{"v": "Russian Federation"}, {"v": 0.0122257951603689}]}, {"c" : [{"v": "Rwanda"}, {"v": 5.4818556000093E-05}]}, {"c" : [{"v": "Saudi Arabia"}, {"v": 0.0170399134402193}]}, {"c" : [{"v": "Sudan"}, {"v": 0.000397536020557836}]}, {"c" : [{"v": "Senegal"}, {"v": 0.000545070855601347}]}, {"c" : [{"v": "Singapore"}, {"v": 0.00266319242815215}]}, {"c" : [{"v": "Solomon Islands"}, {"v": 0.000383105991676275}]}, {"c" : [{"v": "Sierra Leone"}, {"v": 0.000119853768513638}]}, {"c" : [{"v": "El Salvador"}, {"v": 0.00100488453642898}]}, {"c" : [{"v": "Somalia"}, {"v": 6.31705138544109E-05}]}, {"c" : [{"v": "Serbia"}, {"v": 0.00630358382080018}]}, {"c" : [{"v": "Sao Tome and Principe"}, {"v": 0.000555518773705591}]}, {"c" : [{"v": "Suriname"}, {"v": 0.00454044117647059}]}, {"c" : [{"v": "Slovak Republic"}, {"v": 0.00669475341226851}]}, {"c" : [{"v": "Slovenia"}, {"v": 0.00748227433303898}]}, {"c" : [{"v": "Sweden"}, {"v": 0.00559974423461574}]}, {"c" : [{"v": "Swaziland"}, {"v": 0.000857473674682437}]}, {"c" : [{"v": "Seychelles"}, {"v": 0.00784297649548847}]}, {"c" : [{"v": "Syrian Arab Republic"}, {"v": 0.00287278303498869}]}, {"c" : [{"v": "Turks and Caicos Islands"}, {"v": 0.00520594973058433}]}, {"c" : [{"v": "Chad"}, {"v": 4.00464781314487E-05}]}, {"c" : [{"v": "Togo"}, {"v": 0.000244233520572584}]}, {"c" : [{"v": "Thailand"}, {"v": 0.00444685555847179}]}, {"c" : [{"v": "Tajikistan"}, {"v": 0.000375001671621221}]}, {"c" : [{"v": "Turkmenistan"}, {"v": 0.0105224531162764}]}, {"c" : [{"v": "Timor-Leste"}, {"v": 0.000171932157361763}]}, {"c" : [{"v": "Tonga"}, {"v": 0.00151473611404638}]}, {"c" : [{"v": "Trinidad and Tobago"}, {"v": 0.0381611307926014}]}, {"c" : [{"v": "Tunisia"}, {"v": 0.00245310206557905}]}, {"c" : [{"v": "Turkey"}, {"v": 0.00413103076725122}]}, {"c" : [{"v": "Tanzania"}, {"v": 0.000152229977188703}]}, {"c" : [{"v": "Uganda"}, {"v": 0.000111346111256607}]}, {"c" : [{"v": "Ukraine"}, {"v": 0.00664486713741015}]}, {"c" : [{"v": "Uruguay"}, {"v": 0.0019705336505355}]}, {"c" : [{"v": "United States"}, {"v": 0.0175641599948689}]}, {"c" : [{"v": "Uzbekistan"}, {"v": 0.00365667780018486}]}, {"c" : [{"v": "St. Vincent and the Grenadines"}, {"v": 0.00191206227816605}]}, {"c" : [{"v": "Venezuela, RB"}, {"v": 0.00694643711594175}]}, {"c" : [{"v": "Vietnam"}, {"v": 0.00172811843671814}]}, {"c" : [{"v": "Vanuatu"}, {"v": 0.00049659118320433}]}, {"c" : [{"v": "Samoa"}, {"v": 0.000867327137166786}]}, {"c" : [{"v": "Yemen, Rep."}, {"v": 0.000959963331735419}]}, {"c" : [{"v": "South Africa"}, {"v": 0.0090405314610284}]}, {"c" : [{"v": "Zambia"}, {"v": 0.000183669271017558}]}, {"c" : [{"v": "Zimbabwe"}, {"v": 0.000720950742595116}]}]});
      var options = {"legend":{"position":"none"},"colorAxis":{"colors":["#6CC627","#DD5321","#E00B00"]}} 
      var chart = new google.visualization.GeoChart(document.getElementById('5564ab99-baab-449c-a2a3-711e5fe15ee3'));
      chart.draw(data, options);
  }
</script>
<div id="5564ab99-baab-449c-a2a3-711e5fe15ee3" style="margin-left:auto;margin-right:auto;width: 500px; height: 350px;"></div>

Here, we can see quite different picture from the previous visualization.
The biggest polluters _per capita_ include small Persian Gulf states including
Quatar, Kuwait, UAR and Oman, followed by large developed countries (USA, Canada
and Australia). We can still see China among the polluters too, but India almost
completely disappears from this picture.

Building your own visualizations
--------------------------------

I'm not a journalist or a statistician, so I'm sure many of the readers could build 
much more interesting visualizations than I did. The main point of this article is to 
show just how easy it is to put together data source like the [World Bank](http://fsharp.github.io/FSharp.Data/library/WorldBank.html)
with nice visualization libraries available thanks to [XPlot](http://tahahachana.github.io/XPlot/).
So, if you want to build your own visualizations, here are some links to get you started:

 * Go to the [FsLab download page](http://fslab.org/download/) and either download a template
   (the easiest option) or reference the FsLab package from NuGet. 
 * If you have any questions, ask on [StackOverflow with 'fslab' tag](http://stackoverflow.com/questions/tagged/f%23%20or%20deedle%20or%20fslab%20)
   or [open a GitHub issue](https://github.com/fslaborg/FsLab/issues).
 * The [XPlot library has comprehensive documentation](http://tahahachana.github.io/XPlot/)
   with many examples on using the [Google Charts wrapper](http://tahahachana.github.io/XPlot/google-charts.html)
   as well as the [Plotly wrapper](http://tahahachana.github.io/XPlot/plotly.html).
 * To get data, check out the other [F# Data type providers](http://fsharp.github.io/FSharp.Data/) these
   give you easy ways to read CSV, XML and call JSON-based REST services. There are also
   nice libraries for [calling Twitter](http://fsprojects.github.io/FSharp.Data.Toolbox/TwitterProvider.html)
   and [accessing SQL databases](http://fsprojects.github.io/SQLProvider/).

Finally, if you are interested in using some of the libraries in a commercial setting
and are interested in help, support or trainings, [get in touch with me and my colleagues
at fsharpWorks](http://www.fsharpworks.com/). We'll be happy to help.


*)
