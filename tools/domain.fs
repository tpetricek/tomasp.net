namespace FsBlog

open System

type SiteConfig = 
  { Root : string    
    Source : string
    Layouts : string
    Calendar : string
    /// Folder with data files that layouts are generated from (highlights.md)
    Data : string
    /// Repo folder holding layouts/ and source/ - where calendar.txt lives
    Website : string
    /// Public base URL the calendar images are served from
    CalendarRoot : string
    Output : string
    Blog : string
    Academic : string
    /// Folder with the long reads and the LaTeX macros they share
    LongReads : string
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
      Icon = x.Icon; HasDate = x.HasDate; Layout = x.Layout
      Abstract = abs; Body = body }

/// A long read - a LaTeX article published from the same `.tex` as its PDF. It shares only
/// the property-list header with `Article`. Passed to `longread.html` as the model, so these
/// fields are the template's variables.
type LongRead =
  { Title : string
    Description : string
    Image : string
    Date : DateTime
    Url : string
    Layout : string option
    /// Raw HTML for the page <head>, from the `head` section
    Head : string
    /// The title block, from the `frontmatter` section
    FrontMatter : string
    /// The converted `.tex`
    Body : string }

// Used in DotLiquid

type Category = 
  { Name : string
    Url : string
    Count : int }

type Archives = 
  { Tags : seq<Category>
    History : seq<Category> }

/// One entry of the homepage highlights strip, read from `data/highlights.md`
type Highlight =
  { Title : string
    Link : string
    Image : string
    /// Body of the highlight, formatted as HTML
    Body : string }

type Site =
  { Posts : seq<Article<string>>
    PostsTitle : string
    Archives : Archives
    ImageRoot : string
    Papers : seq<Article<string>>
    /// Long reads, newest first - for listing them on the homepage
    LongReads : seq<LongRead>
    /// Homepage highlights, grouped into rows of two for the two-column layout
    Highlights : Highlight[][] }

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
    ImageRoot : string
    Archives : Archives }
