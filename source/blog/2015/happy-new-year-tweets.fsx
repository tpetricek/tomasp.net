(**

Happy New Year 2016 around the World: Behind the scenes of my #FsAdvent project
===============================================================================

 - date: 2015-12-30T18:09:34.5852095+00:00
 - description: This year, my #FsAdvent contribution ended up on December 31.To celebrate the beginning of the New Year 2016, I built an interactive web application that visualizes 'Happy New Year' tweets across the globe. It uses a range of interesting F# libraries including F# Data Toolbox for calling Twitter, Suave.io web server and F# agents.
 - layout: post
 - image: http://tomasp.net/blog/2015/happy-new-year-tweets/thumb.png
 - tags: f#,data journalism,data science,visualization
 - title: Happy New Year 2016 around the World
 - url: 2015/happy-new-year-tweets

--------------------------------------------------------------------------------
 - standalone


Just like [last year](http://tomasp.net/blog/2014/composing-christmas/) and the 
[year](http://tomasp.net/blog/2013/japan-advent-art/index.html) 
[before](http://tomasp.net/blog/2014/japan-advent-art-en/), I wanted to participate in the
[#FsAdvent](https://sergeytihon.wordpress.com/2015/10/25/f-advent-calendar-in-english-2015/)
event, where someone writes a blog post about something they did with F# during December. 
Thanks to [Sergey Tihon](https://sergeytihon.wordpress.com/) for the organization of the English
version and the [Japanese F# community](http://connpass.com/event/22056/) for coming up with the 
idea a few years ago!

As my blog post ended up on 31 December, I wanted to do something that would fit well with the
theme of ending of 2015 and starting of the new year 2016 and so I decided to write a little 
interactive web site that tracks the "Happy New Year" tweets live across the globe. This is
partly inspired by [Happy New Year Tweets](http://twitter.github.io/interactive/newyear2014/)
from Twitter in 2014, but rather than analyzing data in retrospect, you can watch 2016 come live!


--------------------------------------------------------------------------------
*)
(*** hide ***)
#I "../packages/2015/packages/FSharp.Data.Toolbox.Twitter/lib/net40/"
#I "../packages/2015/packages/FSharp.Data/lib/net40/"
#r "../packages/2015/packages/Suave/lib/net40/Suave.dll"
#r "FSharp.Data.Toolbox.Twitter.dll"
#r "FSharp.Data.dll"
#load "../packages/2015/async.fs"
#load "../packages/2015/config.fsx"

open System
open System.Web
open System.Collections.Generic
open FSharp.Data
open FSharp.Data.Toolbox.Twitter
open AsyncHelpers
open System.Text
open System.Text.RegularExpressions

open Suave
open Suave.Web
open Suave.Http
open Suave.Http.Applicatives
open Suave.Sockets
open Suave.Sockets.Control
open Suave.Sockets.AsyncSocket
open Suave.WebSocket
open Suave.Utils

/// Information we collect about tweets. The `Inferred` fields are calculated later 
/// by geolocating the user, all other information is filled when tweet is received
type Tweet = 
  { Tweeted : DateTime
    Text : string
    OriginalArea : string
    UserName : string
    UserScreenName : string
    PictureUrl : string 
    OriginalLocation : option<decimal * decimal>
    Phrase : int
    IsRetweet : bool
    // Filled later by geolocating the tweet
    GeoLocationSource : string
    InferredArea : option<string>
    InferredLocation : option<decimal * decimal> }

let root = IO.Path.Combine(__SOURCE_DIRECTORY__, "web")

(**


Just like [last year](http://tomasp.net/blog/2014/composing-christmas/) and the 
[year](http://tomasp.net/blog/2013/japan-advent-art/index.html) 
[before](http://tomasp.net/blog/2014/japan-advent-art-en/), I wanted to participate in the
[#FsAdvent](https://sergeytihon.wordpress.com/2015/10/25/f-advent-calendar-in-english-2015/)
event, where someone writes a blog post about something they did with F# during December. 
Thanks to [Sergey Tihon](https://sergeytihon.wordpress.com/) for the organization of the English
version and the [Japanese F# community](http://connpass.com/event/22056/) for coming up with the 
idea a few years ago!

As my blog post ended up on 31 December, I wanted to do something that would fit well with the
theme of ending of 2015 and starting of the new year 2016 and so I decided to write a little 
interactive web site that tracks the "Happy New Year" tweets live across the globe. This is
partly inspired by [Happy New Year Tweets](http://twitter.github.io/interactive/newyear2014/)
from Twitter in 2014, but rather than analyzing data in retrospect, you can watch 2016 come live!

Without further ado, here are the important links:

 - [Happy New Year 2016 around the World](http://newyear-tweets.cloudapp.net/) live web site!<br />
   (It will stay alive for a few days around 31 December 2015, but not forever.)   
 - [F# source code for the project](https://github.com/tpetricek/new-year-tweets-2016) on GitHub<br />
   (Feel free to modify it and use it for other events!)
 - [Continute reading](#continue) if you want to learn about how it works!
 
Before we get to the technical details, here is a brief screenshot showing the project live:

<a href="http://newyear-tweets.cloudapp.net/">
<img src="http://tomasp.net/blog/2015/happy-new-year-tweets/screen.gif" />
</a>
<br />
<a name="continue"></a>

Overview 
--------

On the front-end side, the web site displays three different things - it shows live tweets on a 
map, it shows live tweets in a feed (below on the right) and it shows a word cloud with most 
common phrases. Everything is updated live using a three web socket connections with the server.

On the back-end side, the server uses Twitter Streaming API to receive "Happy New Year" tweets as 
they happen. It then uses various techniques for getting locations of some tweets so that they
can appear on the map and it calculates statistics (e.g. for the word cloud) on the fly.

If you look at the source code, pretty much all back-end is implemented in 
[a single F# script file](https://github.com/tpetricek/new-year-tweets-2016/blob/master/app.fsx).
For the front-end, I didn't do anything fancy and [hacked together some 
JavaScript](https://github.com/tpetricek/new-year-tweets-2016/blob/master/web/index.html)
using the great D3-based [Datamaps](http://datamaps.github.io/) library for the map.

There are a couple of nice things in the code including the connection to Twitter,
F# type providers (as always), agents for reactive programming and Suave web server
for implementing web sockets.

Getting a stream of tweets
--------------------------

To get the tweets, I'm using the [F# Data Toolbox library](http://fsprojects.github.io/FSharp.Data.Toolbox/TwitterProvider.html),
which comes with a nice Twitter API wrapper built using F# type providers. As a single-user
application (all is happening on the server), we can directly provider the application
access token & secret and connect to the Twitter directly. Then we can use the 
`twitter.Streaming.FilterTweets` method to search for tweets that contain any of the known
"Happy New Year" phrases:
*)
let phrases = 
 ["새해 복 많이 받으세요"; "สวัสดีปีใหม่";
  "šťastný nový rok"; "عام سعيد"; (*[omit:(...)]*)
  "manigong bagong taon"; "Срећна Нова година"; "честита нова година"; "selamat tahun baru";  
  "С Новым Годом"; "あけまして　おめでとう　ございます"; "新年快乐";  "Щасливого Нового Року"; "שנה טובה"
  "yeni yılınız kutlu olsun"; "feliz año nuevo"; "happy new year"; "Καλή Χρονιά";"godt nyttår"
  "bon any nou"; "felice anno nuovo"; "sretna nova godina"; "godt nytår"; "gelukkig nieuwjaar"
  "Frohes neues Jahr"; "urte berri on"; "bonne année"; "boldog új évet"; "gott nytt år"
  "szczęśliwego nowego roku"; "blwyddyn newydd dda"; "feliz ano novo"; "sugeng warsa enggal" (*[/omit]*) ]

let ctx = (*[omit:(Provide key and secrets)]*)
  { ConsumerKey = Config.TwitterKey; ConsumerSecret = Config.TwitterSecret; 
     AccessToken = Config.TwitterAccessToken; AccessSecret = Config.TwitterAccessSecret } (*[/omit]*)
let twitter = Twitter(UserContext(ctx))

// Search for the phrases and start the stream
let search = twitter.Streaming.FilterTweets phrases 
search.Start()
(**
The `search.TweetReceived` event will be triggered when a new tweet happens. The `status` object
has a bunch of properties (inferred by a type provider). It turns out that event `status.Text` is
optional and so parsing the tweets involves a lot of pattern matching:
*)
let liveTweets = 
  search.TweetReceived
  |> Observable.choose (fun status -> 
      // Parse the location, if the tweet has it
      let origLocation = parseLocation status.Geo

      // Get user name, text of the tweet and location
      match status.User, status.Text with
      | Some user, Some text ->
        { Tweeted = DateTime.UtcNow; OriginalArea = user.Location
          Text = text;PictureUrl = user.ProfileImageUrl; 
          (*[omit:(Populate other properties)]*)
          UserName = user.Name; UserScreenName = user.ScreenName; 
          OriginalLocation = origLocation; Phrase = getPhrase text
          InferredArea = None; InferredLocation = None; 
          IsRetweet = isRT status; GeoLocationSource = "NA"(*[/omit]*) } |> Some
      | _ -> None )
(**

The code is slightly simplified, but it is pretty representative. Now we have a value
`liveTweets` of type `IObservable<Tweet>` which is an event that is triggered every time
we get a new (not completely silly) tweet.

Geolocating tweets and users 
----------------------------

The hardest bit turns out to be getting good tweets for the map. Not a lot of tweets come
with GPS coordinates and so I had to do a couple of tricks. When more people start tweeting
around the New Year, we should be able to use mostly tweets with GPS coordinates, but there
are some backup strategies:

 1. If a tweet has GPS coordinates, use this as the location
 2. Every now and then use [MapQuest](http://www.mapquestapi.com/geocoding/) or [Bing](https://www.bingmapsportal.com/)
    to geolocate the user based on their location in the profile
 3. If we didn't produce enough tweets using (1) or (2), locate tweet based on 
    the language of the phrase and put it in some place where a previous tweet
    with the same phrase appeared.

In priciple, geolocating users based on their profile would work good enough, but all the
geolocation services have rate limits that are easy to hit when the site is running live and
so I added (3) as the last resort. If I had more time, I would probably try to build an index
with country and city names, which would likely cover enough tweets (at least from users with
a reasonable text in their "location").

### Tweets with GPS coordinates

All of the methods report tweets to a "replay" agent (see below) that replays the tweets with
a specified delay. This is done using `replay.AddEvent` at the end of the pipeline. For 
tweets with GPS coordinates, we simply copy the already provided data to `InferredLocation`
(coordinates) and `InferredArea` (text):
*)
liveTweets
|> Observable.choose (fun tw ->
    match tw.OriginalLocation with
    | Some loc -> 
       (tw.Tweeted.AddSeconds(5.0),
        { tw with 
           InferredArea = Some tw.OriginalArea 
           InferredLocation = Some loc }) |> Some
    | _ -> None)
|> Observable.add replay.AddEvent
(**

### Geolocating tweets using Bing and MapQuest

For locating tweets based on the user's location, we will be calling Bing and MapQuest APIs.
This is done using type providers (see below) and wrapped in a nice `MapQuest.locate` and
`Bing.locate` functions. We also need to limit rate at which we use these - the following 
geolocates one tweet per 5 seconds using MapQuest:
*)

liveTweets
|> Observable.limitRate 5000
|> Observable.mapAsyncIgnoreErrors (fun tw -> async {
    let! located = MapQuest.locate tw.OriginalArea
    return located |> Option.map (fun (area, loc) ->
      tw.Tweeted.AddSeconds(10.0),
      { tw with 
         InferredLocation = Some loc; 
         InferredArea = Some area }) })
|> Observble.choose id
|> Observable.add replay.AddEvent
(**

Time zones and geolocating with type providers
----------------------------------------------

As in every F# project, I'm making a heavy use of [F# Data type providers](http://fsharp.github.io/FSharp.Data/)
when calling REST-based geolocation services. As a bonus, I also needed to find time zones of countries of the
world, which can be done by extracting the information from 
[List of time zones by country](https://en.wikipedia.org/wiki/List_of_time_zones_by_country) Wikipedia page
using the HTML type provider.

### Extracting time zone information

The HTML type provider gives us access to the tables on the Wikipedia page and so we can get the country
and time zones just by writing `r.Country` and `r.''Time Zone''` (using backticks to wrap the space).
As far as I know, Datamaps does not easily let me display multiple time zones per country and so I just
pick the middle time zone:
*)
type TimeZones = HtmlProvider<(*[omit:"http://.../List_of_time_zones_by_country"]*)"https://en.wikipedia.org/wiki/List_of_time_zones_by_country"(*[/omit]*)>
let reg = Regex("""UTC([\+\-][0-9][0-9]\:[0-9][0-9])?""")

let timeZones = 
 [ for r in TimeZones.GetSample().Tables.Table1.Rows do
    let tz = r.``Time Zone``.Replace("−", "-")
    let matches = reg.Matches(tz)
    if matches.Count > 0 then
      yield r.Country, matches.[matches.Count/2].Value ]
(**
There are a few explicitly defined countries in the actual source code for countries where the middle time
zone is very wrong and for countries that are named differently on Wikipedia.

### Geolocating using MapQuest

Both Bing and MapQuest provide a nice REST end-point that we can call using the JSON type provider.
To compose the sample URL, we need to use the `Literal` attribute and append a key (which is stored in
a separate config file). The JSON type provider infers the type from the response and gives us nice typed
access to the results:

*)
// Use JSON provider to get a type for calling the API
let [<Literal>] MapQuestSample = 
  (*[omit:"http://mapquestapi.com/geocoding/v1/address?location=Prague"]*)"http://www.mapquestapi.com/geocoding/v1/address?location=Prague&key=" + Config.MapQuestKey(*[/omit]*)
type MapQuest = JsonProvider<MapQuestSample>

let locate (place:string) = 
  let url = 
    "http://www.mapquestapi.com/geocoding/v1/address?key=" +
      Config.MapQuestKey + "&location=" + (HttpUtility.UrlEncode place)
  MapQuest.Load(url).Results
  |> Seq.choose (fun loc ->
      // Pick the first returned location if there were any
      if loc.Locations.Length = 0 then None
      else Some(loc, loc.Locations.[0]) )
  |> Seq.map (fun (info, loc) ->
      // Return the location with lattitude and longitude
      info.ProvidedLocation.Location, 
        (loc.LatLng.Lat, loc.LatLng.Lng) )
(**
As usual, using the JSON type provider for calling REST APIs makes things very easy.
The `Results` property is inferred to be an array of records and information such as
`loc.LatLng.Lat` is also statically typed.

Reactive programming with F# agents
-----------------------------------

The project does quite a lot of interesting reactive event processing. In F#, you can,
of course, use [Reactive Extensions (Rx)](http://reactivex.io/), but I always found Rx
a bit hard to use because they lack simple underlying primitives (more about this in 
[my rant on library design](http://tomasp.net/blog/2015/library-layers/)). F# comes with
a simple set of primitives in the `Observable` module which covers some 80% of what you 
need and you can easily implement additional primitives using F# agents.

For example, the following is a simple agent that I wrote to limit the rate of requests.
The idea is that the agent will emit an event it receives and then it will ignore all
other events for the specified number of milliseconds:
*)
/// Limits the rate of emitted messages to at most 
/// one per the specified number of milliseconds
type RateLimitAgent<'T>(timeout) = 
  let event = Event<'T>()
  let agent = MailboxProcessor.Start(fun inbox -> 
    // We remember the last time we emitted a message
    let rec loop (lastMessageTime:DateTime) = async {
      let! e = inbox.Receive()
      let now = DateTime.UtcNow
      // If we waited long enough, report the event
      // otherwise ignore it and wait some more
      let ms = (now - lastMessageTime).TotalMilliseconds
      if ms > timeout then
        event.Trigger(e)
        return! loop now
      else 
        return! loop lastMessageTime }
    loop DateTime.MinValue )

  /// Triggered when an event happens
  member x.EventOccurred = event.Publish
  /// Send an event to the agent
  member x.AddEvent(event) = agent.Post(event)
(**
Agents are the much needed _lower level primitive_ of the Reactive Extensions. You can
quite easily express any logic you need using just a state machine encoded as a recursive
asynchronous loop. The implementation then wraps the agent in a higher-level primitive
`Observable.limitRate` that was used in the earlier snippet.

Handling websockets with Suave
------------------------------

One more nice thing in the project is the handling of web sockets. The server serves
static files from the `web` sub-directory, but it also communicates with the front-end
via three web sockets (for the map, feed and wordcloud). When a client connects, we
simply want to start sending updates to it from one of the `IObservable<T>` events that
we defined earlier (e.g. by serializing tweets from `liveTweets` as JSON).

To do this, I first defined a helper `socketOfObservable`, which uses Suave's `socket { .. }`
computation builder and repeatedly awaits an update from the specified `updates` and
reports it to via the socket:
*)
let socketOfObservable 
    updates (webSocket:WebSocket) ctx = socket {
  while true do
    // Wait for the next update from the source
    let! update = updates |> Async.AwaitObservable 
                          |> SocketOp.ofAsync
    // Report it to the front-end over the wire!
    do! webSocket.send Text (UTF8.bytes update) true }
(**
The main server is then composed from a number of web parts - the first three handle the
communication via web sockets, the fourth one returns information about time zones that we
downloaded from Wikipedia and the last two serve static files:
*)
let part =
  choose 
    [ path "/maptweets" >>= handShake (socketOfObservable mapTweets)
      path "/feedtweets" >>= handShake (socketOfObservable feedTweets)
      path "/frequencies" >>= handShake (socketOfObservable phraseUpdates)
      path "/zones" >>= Successful.OK timeZonesJson
      path "/">>= Files.browseFile root "index.html" 
      Files.browse root ]
(**
Summary
-------

The main part of the project in `app.fsx` is some 350 lines long and I find it pretty amazing
how much you can do in this small number of lines. If you're writing a project like this in F#,
you get to use a number of nice libraries including [F# Data Toolbox](http://fsprojects.github.io/FSharp.Data.Toolbox/TwitterProvider.html)
for the Twitter API, [Suave.io](https://suave.io/) for the web server and [F# Data type 
providers](http://fsharp.github.io/FSharp.Data/) for calling REST APIs. Finally, I deployed the
service using Azure VM, but you could also use [MBrace](http://mbrace.io/) which can host 
[web servers in a cluster](http://mbrace.io/starterkit/HandsOnTutorial/examples/200-starting-a-web-server-example.html),
or any other hosting - all the libraries I'm using are cross-platform.

If you're reading this around December 31, 2015 then definitely check out the 
[project running live](http://newyear-tweets.cloudapp.net/). I didn't plan to turn this into
a reusable application, but who knows! :-) If you want to use it for tracking tweets related
to some other events you can [find the full source on GitHub](https://github.com/tpetricek/new-year-tweets-2016)
under the Apache license - and also get in touch if you have some interesting use for this work!

*)
