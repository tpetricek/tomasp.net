module FsBlog.Document

open System
open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions

open FSharp.Formatting.Literate
open FSharp.Formatting.Markdown

open FsBlog.Helpers

// --------------------------------------------------------------------------------------
// Document transformations
// --------------------------------------------------------------------------------------

let private (</>) a b = Path.Combine(a, b)
let private ensureDirectory d = 
  if not (Directory.Exists(d)) then Directory.CreateDirectory(d) |> ignore

let private (|CharSeparatedSpans|_|) (sep:char) spans =
  let rec loop before spans =
    match spans with
    | Literal(text=s)::rest when s.Contains(sep.ToString()) ->
        let s1, s2 = s.Substring(0, s.IndexOf(sep)).Trim(), s.Substring(s.IndexOf(sep)+1)
        let before = List.rev before
        let before = if String.IsNullOrWhiteSpace(s1) then before else Literal(s1, MarkdownRange.zero)::before
        let rest = if String.IsNullOrWhiteSpace(s2) then rest else Literal(s2, MarkdownRange.zero)::rest
        Some(before, rest)
    | [] -> None
    | x::xs -> loop (x::before) xs
  loop [] spans

let private (|ColonSeparatedSpans|_|) spans = (|CharSeparatedSpans|_|) ':' spans
let private (|QMarkSeparatedSpans|_|) spans = (|CharSeparatedSpans|_|) '?' spans

/// Format inline spans as HTML. A `Span` paragraph renders inline, without the `<p>` that
/// a `Paragraph` would add. The trailing newline `ToHtml` appends has to go - these strings
/// end up inside `<title>` and `<meta>` tags.
let private formatSpans spans =
  Markdown.ToHtml(MarkdownDocument([ Span(spans, MarkdownRange.zero) ], dict [])).TrimEnd('\r', '\n')

let private formatPlainSpans spans =
  let sb = Text.StringBuilder()
  let rec loop spans =
    for span in spans do
      match span with
      | DirectLink(body=body) -> loop body
      | Literal(text=t) -> sb.Append(t) |> ignore
      | _ -> failwithf "Unsupported span: %A" span
  loop spans
  sb.ToString()

let private generateSubheadings = function
  | Heading(size=1; body=QMarkSeparatedSpans(before, after)) ->
        InlineHtmlBlock
          (sprintf "<h1><span class=\"hmq\">%s</span><span class=\"hs\">%s</span></h1>"
            (formatSpans before) (formatSpans after), None, MarkdownRange.zero)
  | Heading(size=1; body=ColonSeparatedSpans(before, after)) ->
        InlineHtmlBlock
          (sprintf "<h1><span class=\"hm\">%s</span><span class=\"hs\">%s</span></h1>"
            (formatSpans before) (formatSpans after), None, MarkdownRange.zero)
  | p -> p

// --------------------------------------------------------------------------------------
// Document parsing
// --------------------------------------------------------------------------------------

let private readProperty = function
  | [Span(body=ColonSeparatedSpans(before, after))] ->
      match formatPlainSpans before with
      | "description" -> "description", formatSpans after
      | s -> s, (formatPlainSpans after).Trim()
  | p -> failwithf "Failed to read property: %A" p

let private (|Properties|) = function
  | ListBlock(kind=MarkdownListKind.Unordered; items=props)::rest ->
      props |> List.map readProperty |> dict, rest
  | rest -> dict [], rest

let private (|Abstract|) = function
  | HorizontalRule(_)::ListBlock(kind=MarkdownListKind.Unordered; items=props)::rest
  | HorizontalRule(_)::Let [] (props, rest) ->
      let rec split acc = function
        | HorizontalRule _ :: rest -> List.rev acc, rest
        | p :: rest -> split (p::acc) rest
        | _ -> failwith "Parsing abstract failed"
      let standalone = props |> Seq.exists(function [Span(body=[Literal(text="standalone")])] -> true | _ -> false)
      let abs, rest = split [] rest
      Some(standalone, abs), rest
  | rest -> None, rest

let private readMetadata (pars:MarkdownParagraphs) =
  match pars with
  | Heading(size=1; body=title)::Properties(props, Abstract(abs, rest)) -> title, props, abs, rest
  | f -> failwithf "No metadata: %A" f

let private tryFind k (props:IDictionary<string, string>) =
  if props.ContainsKey k then Some(props.[k]) else None

let private parseMetadata (cfg:SiteConfig) (file:string) (title, props, abstractOpt, body) =
  let abs, body =
    match abstractOpt with
    | Some(true, abs) -> abs, Heading(1, title, MarkdownRange.zero)::body
    | Some(false, abs) -> abs, Heading(1, title, MarkdownRange.zero)::(abs @ body)
    | None -> [], Heading(1, title, MarkdownRange.zero)::body
  let date = tryFind "date" props |> Option.map DateTime.Parse
  let references = tryFind "references" props = Some "true"

  { Title = formatSpans title
    Subtitle = defaultArg (tryFind "subtitle" props) ""
    Icon = defaultArg (tryFind "icon" props) ""
    Description = defaultArg (tryFind "description" props) ""
    Image = match tryFind "image" props, tryFind "image-large" props with Some i, _ | _, Some i -> i | _ -> ""
    LargeImage = (tryFind "image-large" props).IsSome
    References = references
    Tags =
      (defaultArg (tryFind "tags" props) "").Split([| ',' |], StringSplitOptions.RemoveEmptyEntries)
      |> Seq.map (fun s -> s.Trim()) |> List.ofSeq
    Date = defaultArg date DateTime.MinValue
    HasDate = date.IsSome
    Url = cfg.Root + (Path.ChangeExtension(file.Substring(cfg.Source.Length), "").TrimEnd('.')).Replace('\\', '/')  + "/"
    Layout = tryFind "layout" props
    Abstract = abs; Body = body }

let private generateReferences (refs:System.Collections.Generic.IDictionary<_, _>) =
  [ Heading(2, [Literal("References", MarkdownRange.zero)], MarkdownRange.zero)
    ListBlock(MarkdownListKind.Ordered,
      [ for url, titleOpt in refs.Values do
          match titleOpt with
          | None -> ()
          | Some title ->
              let ref = sprintf "<a href='%s'>%s</a>" url title
              yield [ InlineHtmlBlock(ref, None, MarkdownRange.zero) ] ], MarkdownRange.zero) ]

let private transformMarkdownFile (cfg:SiteConfig) (inf:string) =
  printfn "Parsing MD file: %s" (inf.Replace(cfg.Source, ""))
  let document = Literate.ParseMarkdownFile(inf)

  let article = parseMetadata cfg inf (readMetadata document.Paragraphs)
  let body = if article.References then article.Body @ generateReferences document.DefinedLinks else article.Body
  let body = document.With(paragraphs = List.map generateSubheadings body)

  let abs = document.With(paragraphs = article.Abstract)
  article.With(Literate.ToHtml(abs), Literate.ToHtml(body))

// --------------------------------------------------------------------------------------
// Articles with a raw HTML body
// --------------------------------------------------------------------------------------

/// Marks an article whose abstract and body are raw HTML; only the header is Markdown.
/// The HTML must not be round-tripped through the Markdown parser, which ends a raw HTML
/// block at the first blank line - common inside `<pre>` code samples.
let private rawBodyRegex = Regex(@"(?m)^\s*-\s*rawbody:\s*true\s*\r?$")

/// Matches the `-----` separators that delimit the header, abstract and body
let private fenceRegex = Regex(@"(?m)^-{3,}[ \t]*\r?$")

/// Read article with a Markdown header and a raw HTML abstract and body
let private transformRawBody (cfg:SiteConfig) (inf:string) (text:string) =
  printfn "Parsing HTML file: %s" (inf.Replace(cfg.Source, ""))
  let fences = fenceRegex.Matches(text)
  if fences.Count < 2 then
    failwithf "Article with a raw body needs two '-----' separators: %s" inf
  let headerEnd = fences.[0].Index
  let absStart = fences.[0].Index + fences.[0].Length
  let bodyStart = fences.[1].Index + fences.[1].Length
  let article =
    parseMetadata cfg inf (readMetadata (Markdown.Parse(text.Substring(0, headerEnd)).Paragraphs))
  // Each separator is a line of its own, so exactly one newline follows it. Everything
  // after that is content verbatim - trimming would drop newlines the HTML relies on.
  let afterNewline (s:string) =
    if s.StartsWith("\r\n") then s.Substring(2)
    elif s.StartsWith("\n") then s.Substring(1)
    else s
  let abs = afterNewline (text.Substring(absStart, fences.[1].Index - absStart))
  let body = afterNewline (text.Substring(bodyStart))
  article.With(abs, body)

/// Read Markdown document, parse metadata and format it as HTML
let transformMarkdown cfg file =
  let text = File.ReadAllText(file:string)
  if rawBodyRegex.IsMatch(text) then transformRawBody cfg file text
  else transformMarkdownFile cfg file

// --------------------------------------------------------------------------------------
// Homepage highlights
// --------------------------------------------------------------------------------------

/// Split paragraphs into blocks, each starting with a level-2 heading
let private headingBlocks paragraphs =
  let close = function
    | Some(title, pars) -> [ title, List.rev pars ]
    | None -> []
  let rec loop finished current paragraphs =
    match paragraphs with
    | Heading(body=title)::rest ->
        loop (finished @ close current) (Some(title, [])) rest
    | par::rest ->
        match current with
        | Some(title, pars) -> loop finished (Some(title, par::pars)) rest
        // Anything before the first heading is a note for whoever edits the file
        | None -> loop finished None rest
    | [] -> finished @ close current
  loop [] None paragraphs

/// Read `data/highlights.md` - a list of blocks, each with a heading, a property list
/// (`link`, `image` and an optional `disabled`) and a Markdown body. Those that are not
/// disabled are shown on the homepage.
let readHighlights (cfg:SiteConfig) =
  let file = cfg.Data </> "highlights.md"
  if not (File.Exists file) then failwithf "Highlights file not found: %s" file
  printfn "Parsing highlights: %s" (file.Replace(cfg.Website, ""))
  let document = Markdown.Parse(File.ReadAllText file)
  [| for title, pars in headingBlocks document.Paragraphs do
      match pars with
      | Properties(props, body) ->
          // Highlights are shown two per row, so `disabled` keeps their number even
          if tryFind "disabled" props <> Some "true" then
            yield
              { Title = formatSpans title
                Link = defaultArg (tryFind "link" props) ""
                Image = defaultArg (tryFind "image" props) ""
                Body = Markdown.ToHtml(MarkdownDocument(body, document.DefinedLinks)) } |]
