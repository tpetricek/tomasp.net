namespace FsBlog

open System

type SiteConfig = 
  { Root : string    
    Source : string
    Layouts : string
    Calendar : string
    Output : string
    Cache : string 
    Blog : string
    Academic : string
    }

/// Represents an article - the properties are read from a list at the begining
/// of the document and can all be missing. Article is generic so that 'T can be
/// `MarkdownParagraphs` (after parsing) or `string` (after formatting) doc.
type Article<'T> =
  { Title : string
    Subtitle : string
    Description : string
    Image : string
    LargeImage : bool
    Tags : seq<string>
    Date : DateTime
    HasDate : bool
    References : bool
    Layout : string option
    Abstract : 'T
    Body : 'T 
    Icon : string
    Url : string }
  member x.With(abs, body) = 
    { Subtitle = x.Subtitle; Title = x.Title; Description = x.Description; Image = x.Image
      LargeImage = x.LargeImage; Tags = x.Tags; Date = x.Date; Url = x.Url; References = x.References
      Icon = x.Icon; HasDate = x.HasDate; Layout = x.Layout; Abstract = abs; Body = body }

// Used in DotLiquid

type Category = 
  { Name : string
    Url : string
    Count : int }

type Archives = 
  { Tags : seq<Category>
    History : seq<Category> }

type Site = 
  { Posts : seq<Article<string>> 
    PostsTitle : string
    Archives : Archives
    Papers : seq<Article<string>> } 

type ArticleModel = 
  { Article : Article<string>
    Archives : Archives }


// Calendar

type Day = 
  { Day : int
    Highlighted : bool }

type Month = 
  { Name : string
    Link : string }

type CalendarYear =
  { Year : string
    Months : seq<Month> 
    Archives : Archives }
