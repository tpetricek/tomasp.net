module FsBlog.Latex

open System
open System.IO

// The parser is deliberately incomplete - it understands only the commands the
// existing long reads use and fails loudly on anything else.

// ----------------------------------------------------------------------------
// DataTypes
// ----------------------------------------------------------------------------

type BibtexEntry = 
  { Type : string
    Properties : Map<string, string> }

type LatexToken = 
  | Command of string * string option * string option
  | ParagraphBreak
  | Literal of string

type Span =
  | Endnote of Span list
  | Span of string
  | Teletype of string
  | Figref of string
  /// `\ref` - renders just the number, the document supplies its own "Figure" or "§"
  | Ref of string
  | Cite of parenthesize:bool * page:string option * string

type Paragraph =
  /// `label` is the `\label` that follows the heading, if any. `anchor` is the unique id
  /// `indexHeadings` assigns - repeated titles would otherwise collide - and is empty until then.
  | Heading of level:int * heading:string * label:string option * anchor:string
  | Paragraph of spans:Span list
  | Image of source:string
  | Caption of label:string option * caption:Span list
  | Figure of body:Paragraph list
  | Quote of body:Paragraph list
  | Listing of language:string option * body:string
  | InlineBlock of html:string 


// ----------------------------------------------------------------------------
// LaTeX "tokenizer"
// ----------------------------------------------------------------------------

let string cs = System.String(Array.ofSeq cs)

let (|Reverse|) els = List.rev els

let (|Quoted|_|) qo qc cs =
  let rec loop acc qocount cs =
    match cs with 
    | c::cs when c = qc && qocount = 0 -> Some(List.rev acc, cs)
    | c::cs when c = qc -> loop (c::acc) (qocount - 1) cs
    | c::cs when c = qo -> loop (c::acc) (qocount + 1) cs
    | c::cs -> loop (c::acc) qocount cs
    | [] -> None
  match cs with 
  | c::cs when c = qo ->
      match loop [] 0 cs with 
      | Some(quoted, cs) -> Some(string quoted, cs)
      | _ -> None
  | _ -> None

let (|ZeroOrMore|) f input = 
  let rec loop acc cs = 
    match cs with 
    | c::cs when f c -> loop (c::acc) cs
    | _ -> string (List.rev acc), cs
  loop [] input

let (|OneOrMore|_|) f input =
  let rec loop acc cs =
    match cs with
    | c::cs when f c -> loop (c::acc) cs
    | _ -> List.rev acc, cs
  match loop [] input with
  | [], _ -> None
  | chars, cs -> Some(string chars, cs)

/// One line break, however the file is saved. Matching only "\r\n" would silently turn an
/// LF-saved document into a single paragraph.
let (|NewLine|_|) cs =
  match cs with
  | '\r'::'\n'::cs -> Some cs
  | '\n'::cs -> Some cs
  | _ -> None

let rec tokenize acc cs = seq {
  let letterOrAst c = Char.IsLetter(c) || c = '*'
  let notNewLine c = c <> '\n' && c <> '\r'
  let spaceOrTab c = c = ' ' || c = '\t'
  let currentLiteral () = 
    let acc = string(List.rev acc)
    if String.IsNullOrWhiteSpace acc then [] 
    else [ Literal(acc) ]

  match cs with 
  | '\\'::'\\'::Quoted '[' ']' (_, cs) ->
      yield! tokenize acc cs
  | '\\'::(('_' | '#') as c)::cs ->
      yield! tokenize (c::acc) cs
  | '~'::cs ->
      yield! tokenize (List.rev(List.ofSeq "&nbsp;") @ acc) cs

  | '\\'::OneOrMore letterOrAst (cmd, Quoted '{' '}' (args, Quoted '[' ']' (opts1, Quoted '{' '}' (opts2, cs)))) ->
      yield! currentLiteral ()
      yield Command(cmd, Some (opts1+","+opts2), Some args)
      yield! tokenize [] cs
  | '\\'::OneOrMore letterOrAst (cmd, Quoted '{' '}' (opts, Quoted '{' '}' (args, cs))) 
  | '\\'::OneOrMore letterOrAst (cmd, Quoted '{' '}' (args, Quoted '[' ']' (opts, cs))) 
  | '\\'::OneOrMore letterOrAst (cmd, Quoted '[' ']' (opts, Quoted '{' '}' (args, cs))) ->
      yield! currentLiteral ()
      yield Command(cmd, Some opts, Some args)
      yield! tokenize [] cs
  | '\\'::OneOrMore letterOrAst (cmd, Quoted '{' '}' (args, cs)) ->
      yield! currentLiteral ()
      yield Command(cmd, None, Some args)
      yield! tokenize [] cs
  | '\\'::OneOrMore letterOrAst (cmd, cs) ->
      yield! currentLiteral ()
      yield Command(cmd, None, None)
      yield! tokenize [] cs
  | NewLine(NewLine cs)
  | NewLine(OneOrMore spaceOrTab (_, NewLine cs)) ->
      yield! currentLiteral ()
      yield ParagraphBreak
      yield! tokenize [] cs
  | '%'::ZeroOrMore notNewLine (_, cs) ->
      yield! tokenize acc cs
  | c::cs -> 
      yield! tokenize (c::acc) cs
  | [] ->
      yield! currentLiteral () }

let formatToken = function
  | ParagraphBreak -> "\r\n\r\n"
  | Literal s -> s
  | Command(name, opts, cmd) -> 
      "\\" + name + (match opts with Some opts -> "[" + opts + "]" | _ -> "")
        + (match cmd with Some cmd -> "{" + cmd + "}" | _ -> "")

// ----------------------------------------------------------------------------
// LaTeX "parser"
// ----------------------------------------------------------------------------

module List = 
  let splitUsing f xs = 
    let rec loop acc xs = 
      match xs with 
      | x::xs when f x -> List.rev acc, x::xs
      | x::xs -> loop (x::acc) xs
      | [] -> List.rev acc, []
    loop [] xs

let (|WhiteSpace|_|) s = 
  if String.IsNullOrWhiteSpace s then Some() else None

let literalToHtml (str:string) = 
  str.Replace("---", "&#8212;").Replace("``", "&#8220;").Replace("''", "&#8221;")
    .Replace("\\#","#").Replace("~", "&nbsp;")

let parseOptions (opts:string option) = 
  match opts with 
  | Some opts -> opts.Split(",") |> Seq.choose (fun kv -> 
      match kv.Split("=") with [|k;v|] ->Some (k, v) | _ -> None) |> dict
  | None -> dict []

let rec parse acc ts = seq {
  let currentParagraph () = 
    if List.isEmpty acc then [] 
    else [ Paragraph(List.rev acc) ]

  match ts with 
  // Environments - code listing (body is text)
  | Command("begin", opts, Some(("lstlisting" | "verbatim") as env))::ts ->
      yield! currentParagraph()
      let opts = parseOptions opts
      let lang = if opts.ContainsKey "language" then Some(opts.["language"]) else None
      let tse, ts = List.splitUsing (function Command("end", _, Some e) -> e = env | _ -> false) ts
      let ts = if ts = [] then failwithf "Missing: \end{%s}" env else List.tail ts
      yield Listing(lang, String.concat "" (Seq.map formatToken tse))
      yield! parse [] ts

  // Environments - minipage, addmargin (ignore)
  | Command("begin", opts, Some(("minipage" | "tiny" | "addmargin") as env))::ts ->
      yield! currentParagraph()
      let tse, ts = List.splitUsing (function Command("end", _, Some e) -> e = env | _ -> false) ts
      let ts = if ts = [] then failwithf "Missing: \end{%s}" env else List.tail ts
      yield! parse [] (tse @ ts)

  // Environments - quote, figure - body is latex
  | Command("begin", opts, Some(env))::ts ->
      yield! currentParagraph()
      let tse, ts = List.splitUsing (function Command("end", _, Some e) -> e = env | _ -> false) ts
      let ts = if ts = [] then failwithf "Missing: \end{%s}" env else List.tail ts
      if env = "figure" then yield Figure(List.ofSeq(parse [] tse))
      elif env = "quote" then yield Quote(List.ofSeq(parse [] tse))
      else failwith $"Unsupported environment: {env}"
      yield! parse [] ts


  // Paragraph-level entities. A heading may be followed by a `\label` that `\ref` points at,
  // which is why each level is matched twice.
  | Command(("chapter"|"chapter*"), _, Some heading)::Literal(WhiteSpace _)::Command("label", _, Some lbl)::ts
  | Command(("chapter"|"chapter*"), _, Some heading)::Command("label", _, Some lbl)::ts ->
      yield! currentParagraph()
      yield Heading(1, heading, Some lbl, "")
      yield! parse [] ts
  | Command(("chapter"|"chapter*"), _, Some heading)::ts ->
      yield! currentParagraph()
      yield Heading(1, heading, None, "")
      yield! parse [] ts
  | Command(("section"|"section*"), _, Some heading)::Literal(WhiteSpace _)::Command("label", _, Some lbl)::ts
  | Command(("section"|"section*"), _, Some heading)::Command("label", _, Some lbl)::ts ->
      yield! currentParagraph()
      yield Heading(2, heading, Some lbl, "")
      yield! parse [] ts
  | Command(("section"|"section*"), _, Some heading)::ts ->
      yield! currentParagraph()
      yield Heading(2, heading, None, "")
      yield! parse [] ts
  | Command(("subsection"|"subsection*"), _, Some heading)::Literal(WhiteSpace _)::Command("label", _, Some lbl)::ts
  | Command(("subsection"|"subsection*"), _, Some heading)::Command("label", _, Some lbl)::ts ->
      yield! currentParagraph()
      yield Heading(3, heading, Some lbl, "")
      yield! parse [] ts
  | Command(("subsection"|"subsection*"), _, Some heading)::ts ->
      yield! currentParagraph()
      yield Heading(3, heading, None, "")
      yield! parse [] ts
  | Command("includegraphics", _, Some src)::ts ->
      yield! currentParagraph()
      yield Image(src)
      yield! parse [] ts
  | Command("caption", _, Some cap)::Literal(WhiteSpace _)::Command("label", _, Some lbl)::ts 
  | Command("caption", _, Some cap)::Command("label", _, Some lbl)::ts ->
      yield! currentParagraph()
      let lbl = lbl.Replace(":","_")
      yield Caption(Some lbl, parseSpans "caption" cap)
      yield! parse [] ts
  | Command("caption", _, Some cap)::ts ->
      yield! currentParagraph()
      yield Caption(None, parseSpans "caption" cap)
      yield! parse [] ts

  // Span-level entities
  | Command("figref", None, Some ref)::ts ->
      let ref = ref.Replace(":","_")
      yield! parse (Figref ref::acc) ts
  | Command("ref", None, Some ref)::ts ->
      yield! parse (Ref ref::acc) ts
  | Command("citet", pg, Some cite)::ts ->
      yield! parse (Cite(false, pg, cite)::acc) ts
  | Command(("citep" | "cite"), pg, Some cite)::ts ->
      yield! parse (Cite(true, pg, cite)::acc) ts
  | Command("emph", None, Some body)::ts ->
      yield! parse (Span("<em>" + literalToHtml body + "</em>")::acc) ts
  | Command("url", None, Some url)::ts ->
      yield! parse (Span($"<a href='{url}'>{url}</a>")::acc) ts
  | Command("S", None, None)::ts ->
      yield! parse (Span "&#167;"::acc) ts
  | Command("ldots", None, None)::ts ->
      yield! parse (Span "&#8230;"::acc) ts
  | Command("endnote", None, Some endnote)::ts ->
      yield! parse (Endnote(parseSpans "endnote" endnote)::acc) ts
  | Command("texttt", None, Some body)::ts ->
      let body = body.Replace("\\_", "_")
      yield! parse (Teletype body::acc) ts
  
  // Whitespace & other ignored
  | Command(("vspace" | "quad" | "newpage" | "raisebox" | "noindent"), _, _)::ts ->
      yield! parse acc ts
  | Command(("centering" | "raggedleft" | "theendnotes"), _, _)::ts ->
      yield! parse acc ts

  // Ignored but wrapping things
  | Command("boxed", None, Some body)::ts ->
      let ts = List.ofSeq (tokenize [] (List.ofSeq body)) @ ts
      yield! parse acc ts
 
  // Ignored - keep as latex. These show up verbatim on the page, so we can see them in
  // context and decide what they should become.
  | Command(("textwidth"), _, _)::ts ->
      yield! parse (Span "\textwidth"::acc) ts
  | Command(("paragraph" | "href" | "textsc" | "TeX") as cmd, opts, arg)::ts ->
      let latex =
        @"\" + cmd
        + (match opts with Some o -> "{" + o + "}" | _ -> "")
        + (match arg with Some a -> "{" + a + "}" | _ -> "")
      yield! parse (Span(System.Web.HttpUtility.HtmlEncode latex)::acc) ts

  | Command(c, opts, arg)::cs ->
      failwithf "Unsupported command: %s[%A]{%A}\nBefore: %A" c opts arg (List.truncate 10 cs)
  | ParagraphBreak::ts -> 
      yield! currentParagraph()
      yield! parse [] ts
  | Literal(s)::ts ->
      yield! parse (Span(literalToHtml s)::acc) ts
  | [] ->
      yield! currentParagraph() }

/// The argument of a command that is itself LaTeX - a caption or an endnote. It has to run
/// through the tokenizer again, which is what lets `\texttt` and `\citet` work inside one.
and parseSpans what (text:string) =
  match parse [] (List.ofSeq (tokenize [] (List.ofSeq text))) |> List.ofSeq with
  | [ Paragraph spans ] -> spans
  | [] -> []
  | pars -> failwithf "%s - expected a single paragraph, but got: %A" what pars

let makeFigureMap pars =
  let rec loop map = function
    | Caption(Some lbl, _) -> Map.add lbl (Map.count map + 1) map
    | Paragraph _ | Heading _ | Image _ | Listing _ | InlineBlock _ | Caption(None, _) -> map
    | Quote pars | Figure pars -> List.fold loop map pars
  List.fold loop Map.empty pars

/// Give every heading a unique anchor, and number the sections for `\ref`. Anchors come from
/// the heading text, so a title used twice would otherwise produce two identical ids. Returns the
/// rewritten paragraphs and a map from each `\label` to the number and anchor `\ref` needs.
let indexHeadings pars =
  let seen = System.Collections.Generic.Dictionary<string, int>()
  let mutable sections = 0
  let mutable refs = Map.empty
  let rec loop = function
    | Heading(level, heading, label, _) ->
        let name = heading.ToLower().Replace(" ", "_")
        let n = (match seen.TryGetValue name with true, k -> k | _ -> 0) + 1
        seen.[name] <- n
        let anchor = if n = 1 then name else $"{name}_{n}"
        // Headings themselves are not numbered on the web - the contents list is an <ol>,
        // which numbers them already. The count is only here so `\ref` has a number to show.
        if level = 2 then sections <- sections + 1
        match label with
        | Some lbl when level = 2 -> refs <- Map.add lbl (sections, anchor) refs
        | _ -> ()
        Heading(level, heading, label, anchor)
    | Quote pars -> Quote(List.map loop pars)
    | Figure pars -> Figure(List.map loop pars)
    | p -> p
  List.map loop pars, refs

// ----------------------------------------------------------------------------
// HTML formatting
// ----------------------------------------------------------------------------

type FormattingContext = 
  { NextFootnote : unit -> int 
    LanguageMap : string -> string
    References : System.Collections.Generic.IDictionary<string, BibtexEntry> 
    FigureMap : Map<string, int>
    /// Each section `\label` to that section's number and anchor, for `\ref`
    SectionMap : Map<string, int * string>
    AllEndNotes : ResizeArray<string> }

let getAuthorAbbr (ref:BibtexEntry) =
  let s = 
    ref.Properties.TryFind "author" 
    |> Option.defaultWith (fun _ -> ref.Properties.["editor"])
  let auths = s.Split(" and ") |> Array.map (fun a -> a.Split(",").[0].Split(' ') |> Seq.last) |> List.ofArray
  match auths with 
  | [a1] -> a1
  | [a1; a2] -> a1 + " and " + a2
  | a1::_ -> a1 + " et al."
  | [] -> failwith "getAuhtorsAbbr: Expected non-empty list"

let rec formatSpans fmt spans = seq { 
  for s in spans do 
    match s with 
    // Escaped: `\texttt{<script>}` would otherwise open a real script element and the browser
    // would swallow the rest of the page as JavaScript
    | Teletype(s) -> $"<code>{System.Web.HttpUtility.HtmlEncode s}</code>"
    | Cite(paren, pg, cits) ->
        let cits = cits.Split(',') |> Array.map (fun cit ->
          let ref = try fmt.References.[cit] with _ -> failwith $"Cannot find reference: {cit}"
          let year, author = ref.Properties.["year"], getAuthorAbbr ref
          let pgyear = year + if pg.IsSome then ", " + pg.Value else ""
          let link = 
            if paren then author + ", " + pgyear
            else author + " (" + pgyear + ")"
          $"<a href='#{cit}'>{link}</a>")
        if paren then "(" + String.concat "; " cits + ")"
        else String.concat ", " cits

    | Endnote(s) -> 
        let note = fmt.NextFootnote()
        let s = String.concat "" (formatSpans fmt s)
        let ftnt = $"<span class='endnote' id='endnote_{note}'><sup>{note}</sup> {s}</span>"
        fmt.AllEndNotes.Add(ftnt)
        $"<sup class='noteref'><a href='#note_{note}'>{note}</a></sup>" + 
        $"<span class='note' id='note_{note}'><sup>{note}</sup> {s}</span>"

    | Figref(ref) ->
        $"<a href='#{ref}'>Figure {fmt.FigureMap.[ref]}</a>"
    | Ref(ref) ->
        let figref = ref.Replace(":", "_")
        match fmt.FigureMap.TryFind figref, fmt.SectionMap.TryFind ref with
        | Some n, _ -> $"<a href='#{figref}'>{n}</a>"
        | _, Some(n, anchor) -> $"<a href='#{anchor}'>{n}</a>"
        | _ -> failwith $"Cannot find reference: {ref}"
    | Span(s) -> s }

let rec formatPars fmt pars = seq {
  for p in pars do
    match p with 
    | InlineBlock(h) -> 
        yield h
    | Heading(n, h, _, anchor) ->
        yield $"<h{n} id='{anchor}'>{h}</h{n}>\n"
    | Caption(lbl, cap) ->
        let cap = String.concat "" (formatSpans fmt cap)
        yield $"<p class='caption'><strong>Figure {fmt.FigureMap.[lbl.Value]}.</strong> {cap}</p>\n"
    | Image(src) ->
        yield $"<img src=\"{src}\">\n"
    | Paragraph(spans) ->   
        yield "<p>"
        yield! formatSpans fmt spans
        yield "</p>\n\n"
    | Figure(pars) ->        
        yield "<div class=\"figure\">\n"
        yield! formatPars fmt pars    
        yield "</div>\n"
    | Quote(pars) ->
        yield "\n<blockquote>\n"
        yield! formatPars fmt pars
        yield "</blockquote>\n\n" 
    | Listing(lang, body) ->
        match lang with 
        | Some lang -> yield $"\n<pre><code class=\"language-{fmt.LanguageMap lang}\">" 
        | None -> yield "\n<pre><code class='language-plaintext'>"
        yield System.Web.HttpUtility.HtmlEncode(body.Trim())
        yield "</code></pre>\n\n" }

let (|ImagesWithCaption|_|) pars =
  let rec loop acc pars =
    match pars with
    | [ Caption(Some lbl,c) ] -> Some(List.rev acc, lbl, c)
    | (Image i) :: pars -> loop (i::acc) pars
    | _ -> None
  loop [] pars

/// A figure written as several image+caption pairs, so each one is a numbered figure of its own
/// in the PDF. `ImagesWithCaption` above is the other shape - several images under one caption.
let (|ImageCaptionPairs|_|) pars =
  let rec loop acc pars =
    match pars with
    | Image img :: Caption(Some lbl, cap) :: pars -> loop ((img, lbl, cap)::acc) pars
    | [] when List.length acc >= 2 -> Some(List.rev acc)
    | _ -> None
  loop [] pars

let formatGroups fmt groups = seq {
  for pars in groups do
    match pars with 
    | [ Figure(ImagesWithCaption(imgs, lbl, cap)) ] when List.length imgs >= 3 ->
        yield $"\n<section class='figure figview' id='{lbl}'>\n"
        yield $"<div class='figbody' id='{lbl}scroll'>\n"
        for img in imgs do
          yield $"<figure><img src='{img}'></figure>\n"
        yield "</div>\n"
        yield "</section>\n"

        yield $"\n<section class='figure figpreview'>\n"
        yield $"<div class='move moveleft' onmouseout='endScroll()' onmousedown='startScroll(\"{lbl}\",-300,true)' onmouseover='startScroll(\"{lbl}\",-100)'><i class='fa-solid fa-angle-left'></i></div>\n"
        yield $"<div class='move moveright' onmouseout='endScroll()' onmousedown='startScroll(\"{lbl}\",300,true)' onmouseover='startScroll(\"{lbl}\",100)'><i class='fa-solid fa-angle-right'></i></div>\n"
        yield $"<div class='figlist' id='{lbl}previews'>\n"
        for i, img in Seq.indexed imgs do
          let cls = if i = 0 then " class='selected'" else ""
          yield $"<a{cls} href='javascript:;' onclick='switchFigure(\"{lbl}\", {i})'><img src='{img}'></a>\n"
        yield "</div>\n"
        yield! formatPars fmt [Caption(Some lbl, cap)]
        yield "</section>\n\n"

    // Several numbered figures shown as one carousel. Each slide keeps its own label as its id,
    // so `\ref` still resolves to it, and the captions are concatenated below the thumbnails.
    | [ Figure(ImageCaptionPairs slides) ] ->
        let prefix = let _, lbl, _ = List.head slides in lbl
        let number (lbl:string) = fmt.FigureMap.[lbl]

        yield $"\n<section class='figure figview'>\n"
        yield $"<div class='figbody' id='{prefix}scroll'>\n"
        for img, lbl, _ in slides do
          yield $"<figure id='{lbl}'><img src='{img}'></figure>\n"
        yield "</div>\n"
        yield "</section>\n"

        yield $"\n<section class='figure figpreview'>\n"
        yield $"<div class='move moveleft' onmouseout='endScroll()' onmousedown='startScroll(\"{prefix}\",-300,true)' onmouseover='startScroll(\"{prefix}\",-100)'><i class='fa-solid fa-angle-left'></i></div>\n"
        yield $"<div class='move moveright' onmouseout='endScroll()' onmousedown='startScroll(\"{prefix}\",300,true)' onmouseover='startScroll(\"{prefix}\",100)'><i class='fa-solid fa-angle-right'></i></div>\n"
        yield $"<div class='figlist' id='{prefix}previews'>\n"
        for i, (img, _, _) in Seq.indexed slides do
          let cls = if i = 0 then " class='selected'" else ""
          yield $"<a{cls} href='javascript:;' onclick='switchFigure(\"{prefix}\", {i})'><img src='{img}'></a>\n"
        yield "</div>\n"

        // The slides are numbered figures already, so the items carry those numbers rather than
        // restarting at (1), and the caption is labelled with the range they cover
        let first, last = number prefix, slides |> List.last |> fun (_, lbl, _) -> number lbl
        let items =
          [ for _, lbl, cap in slides ->
              $"({number lbl}) " + String.concat "" (formatSpans fmt cap) ]
        yield $"<p class='caption'><strong>Figures {first}&#8211;{last}.</strong> "
        yield String.concat " " items
        yield "</p>\n"
        yield "</section>\n\n"

    | _ ->
        let cls = 
          match pars with 
          | [Figure[Listing _; Listing _; Caption _]] -> $" class='figure listings2'"
          | [Figure(ImagesWithCaption(imgs, _, _))] -> $" class='figure images{imgs.Length}'"
          | [Figure _] -> " class='figure'" 
          | [InlineBlock(b)] when b.StartsWith("<h2 id='endnotes'>") -> " class='endnotes'"
          | _ -> ""
        let id = 
          match pars with 
          | [Figure(Reverse(Caption(Some lbl, _)::_))] -> $" id='{lbl}'"
          | _ -> ""
        yield $"\n<section{cls}{id}>\n"
        yield! formatPars fmt pars
        yield "</section>\n\n"
  }

let groupPars pars = 
  let rec loop acc pars = seq {
    let currentGroup() = 
      if List.isEmpty acc then []
      else [ List.rev acc ]
    match pars with 
    | ((Heading(1, _, _, _) | Figure _) as p)::pars ->
        yield! currentGroup ()
        yield [p]
        yield! loop [] pars 
    | (Heading _ as p)::pars ->  
        yield! currentGroup ()
        yield! loop [p] pars
    | p::pars ->
        yield! loop (p::acc) pars 
    | [] ->
        yield! currentGroup () }
  loop [] pars

let generateToc doc = 
  [ for p in doc do
      match p with 
      // Sections only - a long read with subsections would otherwise swamp its own contents
      | Heading(level, h, _, anchor) when level <= 2 ->
          $"  <li><a href='#{anchor}'>{h}</a></li>\n"
      | _ -> () 
    "  <li><a href='#references'>References</a></li>\n" ]
  |> String.concat ""
  |> sprintf "\n<ol>\n%s</ol>\n"
  |> InlineBlock

// ----------------------------------------------------------------------------
// Bibtex parser
// ----------------------------------------------------------------------------

let (|Find|_|) k map = 
  Map.tryFind k map

let (|Identifier|_|) cs = 
  let rec loop acc cs = 
    match cs with 
    | c::cs when Char.IsLetterOrDigit(c) || c = '-' || c = '_' -> loop (c::acc) cs
    | cs when not (List.isEmpty acc) -> Some(string (List.rev acc), cs)
    | _ -> None
  loop [] cs

let sconc sep s1 s2 =
  if String.IsNullOrWhiteSpace s1 then s2 
  elif String.IsNullOrWhiteSpace s2 then s1 
  else s1 + sep + s2

let optconc sep s1 s2 = 
  match s2 with None -> s1 | Some s2 -> sconc sep s1 s2

let (+|) = sconc ", "
let (+|?) = optconc ", "
let (+-) = sconc " "
let (+-?) = optconc " "
let (+.) = sconc ". "
let (+.?) = optconc ". "

let rec skipWhite cs = 
  match cs with 
  | c::cs when Char.IsWhiteSpace c -> skipWhite cs
  | _ -> cs

let (|SkipWhite|) cs = skipWhite cs

let rec readBibValue literal acc cs = 
  match cs with 
  | '\\'::'l'::'d'::'o'::'t'::'s'::cs -> readBibValue literal (List.rev (List.ofSeq "&#8230;") @ acc) cs
  | '\\'::'#'::cs -> readBibValue literal ('#' :: acc) cs
  | '\\'::'$'::cs -> readBibValue literal ('$' :: acc) cs
  | '\\'::'&'::cs -> readBibValue literal ('&' :: acc) cs
  | ' '::cs when literal -> readBibValue literal (';'::'2'::'3'::'#'::'&'::acc) cs
  | '\\'::'u'::'r'::'l'::Quoted '{' '}' (url, cs) -> 
      let link = $"<a href='{url}'>{url}</a>" |> Seq.toList |> List.rev
      readBibValue literal (link @ acc) cs
  | '{'::cs -> readBibValue true acc cs
  | '}'::cs -> readBibValue false acc cs
  // An angle bracket in the source text is content - a title like "The Origins of the <Blink>
  // Tag" would otherwise reach the page as a tag. The link built above is already HTML and is
  // added straight to `acc`, so it does not come through here.
  | '<'::cs -> readBibValue literal (List.rev (List.ofSeq "&lt;") @ acc) cs
  | '>'::cs -> readBibValue literal (List.rev (List.ofSeq "&gt;") @ acc) cs
  | c::cs -> readBibValue literal (c::acc) cs
  | [] -> string (List.rev acc)

let rec readDefs acc cs = 
  match skipWhite cs with 
  | Identifier(id, SkipWhite('='::SkipWhite(Identifier(value, (','::cs | cs)))))
  | Identifier(id, SkipWhite('='::SkipWhite(Quoted '"' '"' (value, (','::cs | cs))))) 
  | Identifier(id, SkipWhite('='::SkipWhite(Quoted '{' '}' (value, (','::cs | cs))))) ->
      let value = readBibValue false [] (List.ofSeq value)
      // BibTeX field names are case-insensitive, so `bookTitle` has to find `booktitle`
      readDefs ((id.ToLowerInvariant(), value)::acc) cs
  | '}'::cs -> 
      Map.ofList acc, cs
  | _ -> failwith $"Expected key/value or end, but found: {string (Seq.truncate 200 cs)}..."

let rec readBib acc cs = 
  match skipWhite cs with 
  | '@'::Identifier(typ, '{'::Identifier(key, ','::cs)) ->
      let props, cs = readDefs [] cs
      readBib ((key, { Type = typ.ToLower(); Properties = props })::acc) cs
  | [] ->
      acc
  | _ -> failwith $"Expected entry, but found: {string (Seq.truncate 200 cs)}..."

let formatAuthorList (s:string) =
  [ for a in s.Split(" and ") -> a.Split(",") |> Seq.map (_.Trim()) |> Seq.rev |> String.concat " " ]
  |> String.concat ", "

let renderReferences refs = 
  let endw (c:char) (s:string) = 
    if not(String.IsNullOrWhiteSpace s) && not(s.EndsWith(',')) 
      && not(s.EndsWith('.')) && not(s.EndsWith('?')) then s + (c.ToString()) else s
  let refs = refs |> Seq.map (fun (key, ref) ->
    // A missing field otherwise surfaces as a bare KeyNotFoundException naming neither the
    // entry nor the field, which is no help at all in an 87-entry bibliography
    let required name =
      match ref.Properties.TryFind name with
      | Some value -> value
      | None ->
          failwithf "Bibliography entry '%s' (@%s) has no '%s' field. It has: %s"
            key ref.Type name (String.concat ", " (Seq.sort ref.Properties.Keys))
    let title = required "title"
    let year = required "year"
    let authAbbr = getAuthorAbbr ref

    let auth, ed =
      match ref.Properties with 
      | Find "author" author & Find "editor" editor -> 
          formatAuthorList author, formatAuthorList editor + ", editor"
      | Find "author" author -> formatAuthorList author, ""
      | Find "editor" editor -> formatAuthorList editor + " (ed.)", ""
      | _ -> failwith "Missing author and editor"
    
    let pub, det = 
      match ref.Type with 
      | "book" -> 
          required "publisher" +|? ref.Properties.TryFind("address"),
          "" +-? ref.Properties.TryFind("isbn") +.? ref.Properties.TryFind("note")
      | "misc" -> 
          required "howpublished",
          "" +-? ref.Properties.TryFind("note")
      | "inproceedings" | "incollection" | "inbook" -> 
          "In " +-? 
            Option.map (sprintf "%s, editor, ") (ref.Properties.TryFind("editor")) +
            required "booktitle" +-?
            Option.map (sprintf "(%s)") (ref.Properties.TryFind("series")) +|?
            Option.map (sprintf "pages %s") (ref.Properties.TryFind("pages")) +|?
            ref.Properties.TryFind("address")
          , "" +-? ref.Properties.TryFind("publisher") +|? 
            Option.map (sprintf "ISBN %s") (ref.Properties.TryFind("isbn")) +|? 
            Option.map (fun d -> sprintf "doi <a href='https://dx.doi.org/%s'>%s</a>" d d) 
              (ref.Properties.TryFind("doi")) +.? 
            ref.Properties.TryFind("note")
      | "article" ->
          required "journal" +|? ref.Properties.TryFind("volume") +-?
            Option.map (sprintf "(%s)") (ref.Properties.TryFind("number")) +|?
            Option.map (sprintf "pages %s") (ref.Properties.TryFind("pages"))
          , "" +|?             
            Option.map (fun d -> sprintf "doi <a href='https://dx.doi.org/%s'>%s</a>" d d) 
              (ref.Properties.TryFind("doi")) +|? 
            ref.Properties.TryFind("publisher") +|? ref.Properties.TryFind("address") +.? 
            ref.Properties.TryFind("note")
      | "phdthesis" ->
          "PhD thesis, " + required "school"
          , "" +|? ref.Properties.TryFind("note")
      | "techreport" ->
          "Technical Report " + required "number" +|? 
          ref.Properties.TryFind("institution") +|? ref.Properties.TryFind("address")
          , "" +|? ref.Properties.TryFind("note")

      | typ -> failwith $"Unsupported bib item type: {typ}"
    
    authAbbr + " (" + year + ")",
    $"<li class='pub'><span class='author'>{endw '.' auth} </span>" + 
    $"<span id='{key}'><span class='title'>{endw '.' title}</span></span> " + 
    $"{endw '.' pub} {endw '.' year} {det}</a></li>\n"
  )
  let refs = refs |> Seq.sortBy fst |> Seq.map snd
  let body = String.concat "\n" refs
  InlineBlock("<h2 id='references'>References</h2>\n<ul>" + body + "</ul>")

let renderEndnotes fmt = 
  let body = fmt.AllEndNotes |> Seq.map (sprintf "<li>%s</li>\n") |> String.concat ""
  InlineBlock("<h2 id='endnotes'>Notes</h2>\n<ul>" + body + "</ul>")

// ----------------------------------------------------------------------------
// Long read file conventions
// ----------------------------------------------------------------------------

/// A long read is a `<name>.md` article with a sibling `<name>` folder holding
/// `<name>.tex`, `<name>.bib`, `main.tex` and the web assets
let private folderOf (mdFile:string) =
  Path.Combine(Path.GetDirectoryName(mdFile), Path.GetFileNameWithoutExtension(mdFile))

/// The `.tex` and `.bib` that a long read is generated from
let sourceFiles (mdFile:string) =
  let dir = folderOf mdFile
  let name = Path.GetFileNameWithoutExtension(mdFile)
  Path.Combine(dir, name + ".tex"), Path.Combine(dir, name + ".bib")

/// Files the page is regenerated from besides the `.md`. Empty for any other article.
let dependencies (mdFile:string) =
  let tex, bib = sourceFiles mdFile
  [ if File.Exists tex then yield tex
    if File.Exists bib then yield bib ]

// ----------------------------------------------------------------------------
// Main
// ----------------------------------------------------------------------------

let private counter () =
  let mutable n = 0
  fun () -> n <- n + 1; n

let private langmap = function
  | "csxml" -> "xml"
  | l -> l

/// Convert a LaTeX document into the two halves of a long read: the front matter with the
/// generated table of contents, and the document with its endnotes and bibliography.
let formatDocument (texFile:string) (bibFile:string) (frontMatter:string) =
  let refs = readBib [] (List.ofSeq (File.ReadAllText bibFile))
  let tokens = tokenize [] (List.ofSeq (File.ReadAllText texFile)) |> List.ofSeq
  let doc, sections = parse [] tokens |> List.ofSeq |> indexHeadings
  let fmt =
    { NextFootnote = counter(); References = dict refs; AllEndNotes = ResizeArray<_>()
      LanguageMap = langmap; FigureMap = makeFigureMap doc; SectionMap = sections }

  let body = doc |> groupPars |> List.ofSeq
  let toc = generateToc doc

  // Forced in document order: the body fills in `AllEndNotes` and numbers the notes
  let frontHtml = formatGroups fmt [ [ InlineBlock frontMatter; toc ] ] |> String.concat ""
  let bodyHtml = formatGroups fmt body |> String.concat ""
  let extras = [ [ renderEndnotes fmt ]; [ renderReferences refs ] ]
  frontHtml, bodyHtml + (formatGroups fmt extras |> String.concat "")

// ----------------------------------------------------------------------------
// Building PDFs - local only, CI has no LaTeX so the PDF is committed
// ----------------------------------------------------------------------------

/// The pdflatex driver of a long read
let private driverFile (mdFile:string) = Path.Combine(folderOf mdFile, "main.tex")

/// Long reads that can be built as a PDF, as (article folder, name)
let longReads (cfg:SiteConfig) =
  let dir = cfg.LongReads
  if not (Directory.Exists dir) then [] else
  [ for md in Directory.GetFiles(dir, "*.md") do
      if File.Exists(driverFile md) then
        yield folderOf md, Path.GetFileNameWithoutExtension(md) ]

let private run (exe:string) (args:string) (workDir:string) (env:(string * string) list) =
  let ps =
    Diagnostics.ProcessStartInfo
      ( FileName = exe, Arguments = args, WorkingDirectory = workDir,
        UseShellExecute = false, CreateNoWindow = true )
  for k, v in env do ps.Environment.[k] <- v
  try
    use p = Diagnostics.Process.Start(ps)
    p.WaitForExit()
    Some p.ExitCode
  // No LaTeX on this machine - the command is a no-op rather than a crash
  with :? ComponentModel.Win32Exception -> None

/// Build the PDF of one long read and copy it next to the article
let buildPdf (folder:string) (name:string) =
  let build = Path.Combine(folder, "build")
  Directory.CreateDirectory(build) |> ignore
  File.WriteAllText(Path.Combine(build, ".ignore"), "")

  // pdflatex reads from the article folder and writes into `build`; bibtex has to run in
  // `build`, where the `.aux` files are, with BIBINPUTS pointing back at the `.bib`
  let pdflatex () = run "pdflatex" "-interaction=nonstopmode -output-directory=build main.tex" folder []
  let bibtex () = run "bibtex" "main" build [ "BIBINPUTS", folder + ";" ]

  printfn "Building PDF: %s" name
  match pdflatex () with
  | None -> printfn "  pdflatex not found on PATH - skipping (PDFs are built locally only)"
  | Some _ ->
    // Citation numbering only settles after bibtex and pdflatex chase each other a few times
    for _ in 1..3 do
      bibtex () |> ignore
      pdflatex () |> ignore
    let produced = Path.Combine(build, "main.pdf")
    if not (File.Exists produced) then
      printfn "  no PDF produced - see %s" (Path.Combine(build, "main.log"))
    else
      let target = Path.Combine(folder, "pdf", name + ".pdf")
      Directory.CreateDirectory(Path.GetDirectoryName target) |> ignore
      File.Copy(produced, target, true)
      printfn "  written %s (%.1f MB)" target (float (FileInfo(produced).Length) / 1048576.0)

/// Build every long read's PDF, then rebuild whenever its LaTeX sources change
let watchPdfs (cfg:SiteConfig) =
  let reads = longReads cfg
  if List.isEmpty reads then printfn "No long reads with a 'main.tex' driver found."
  for folder, name in reads do buildPdf folder name

  let agent = MailboxProcessor.Start(fun inbox -> async {
    while true do
      let! (folder, name) = inbox.Receive()
      // Coalesce the burst of events an editor save produces
      while inbox.CurrentQueueLength > 0 do inbox.Receive() |> Async.RunSynchronously |> ignore
      try buildPdf folder name
      with e -> printfn "Failed to build PDF for %s: %s" name e.Message })

  [ for folder, name in reads do
      for pattern in [ "*.tex"; "*.bib" ] do
        let fsw = new FileSystemWatcher(folder, pattern, IncludeSubdirectories = true)
        fsw.Changed.Add(fun e ->
          // Ignore what LaTeX itself writes, or the build would never settle
          if not (e.FullPath.Contains(Path.Combine(folder, "build"))) then
            printfn "Changed: %s" (Path.GetFileName e.FullPath)
            agent.Post((folder, name)))
        fsw.EnableRaisingEvents <- true
        yield fsw ]
