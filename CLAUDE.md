# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

The source and custom static-site generator for **tomasp.net**. The generator ("FsBlog") is a
small .NET 10 console app in `tools/`, built with `dotnet`.

This repo holds everything the site is built from. Two directories sit *outside* it, as siblings
of the repo folder:

- `../output/` — where the generator writes. Created on demand; purely a scratch build directory,
  since deployment happens in CI. Safe to delete.
- `../calendar/` — the original calendar photos (`<year>/<month>.jpg`) plus the `na.png`
  placeholder, ~817 MB. Only `build.sh calendar` reads them; a clone without this folder builds
  the whole site fine.

The generator finds the repo by walking out of its own binary folder looking for a directory
holding both `layouts` and `source`, so it runs from anywhere; the sibling paths hang off that.

## Commands

Run from the repo root (`build.cmd` on Windows, `build.sh` elsewhere — both just
`dotnet run --project tools -c Release -- "$@"`):

```
build.sh              # default "run" — rebuild, watch, serve on :11112, open a browser
build.sh build        # wipe output/ and regenerate everything
build.sh calendar     # resize calendar photos and upload them to R2 (local only)
build.sh longreads    # build each long read's PDF with pdflatex, then watch (local only)
```

There is no test suite and no linter. To check a change against what is published, point `../output` at a
checkout of the `gh-pages` branch and regenerate: a change that alters nothing produces an empty
`git -C ../output diff`. Use `git diff` rather than `git status` — status reports stat-cache
noise on files whose line endings git normalises.

A full build from scratch takes about a minute. There is no cache: every article is re-rendered
every time, by design (the old mtime-keyed JSON cache silently served content rendered by a
different FSharp.Formatting version — see "Posts with `format: bakedin`" below).

## Architecture

`tools/main.fs` holds the `SiteConfig` (all paths, `Root = "http://tomasp.net"`), the
`updateSite` / `regenerateSite` entry points, an `HttpListener` dev server with websocket
live-reload, and a `FileSystemWatcher`. Editing anything in `layouts/` triggers a full pass;
editing content refreshes only the changed files. `regenerate` deletes everything in `output/`
except `.git`.

Module compile order (`domain` → `helpers` → `latex` → `dotliquid` → `document` → `r2` → `calendar` → `blog` → `main`):

- **`helpers.fs`** — mtime comparisons, plus `trackingTag` and `injectTracking` (see
  "Analytics" below).

- **`domain.fs`** — `SiteConfig`, `Article<'T>` (generic over `MarkdownParagraphs` before formatting
  and `string` after), `Site`/`ArticleModel` (the DotLiquid view models), calendar types.
- **`latex.fs`** — the LaTeX→HTML converter behind long reads, plus the `build.sh longreads`
  PDF builder. See "Long reads" below.
- **`document.fs`** — parses one source file. `.md` is the only file the generator looks at, and
  `transform` returns a `Content`: the ` - format:` property in the header says which case:
  - `markdown` (or no `format` at all) — `Post`, via `Literate.ParseMarkdownFile`
  - `bakedin` — `Post`; only the header is Markdown, the abstract and body are raw HTML emitted
    **verbatim**, delimited by two `----` separators
  - `longread` — `LongRead`, a **different record** with only the fields a long read uses. Its
    `----` separators are all named (see "Long reads" below) and its body comes from a `.tex`.
  `Post` carries `Article<'T>`; only those go into `Posts`/`Papers`, so listings, archives and
  RSS never see a long read.
  Raw bodies must never be round-tripped through the Markdown parser: it ends a raw HTML
  block at the first blank line, and blank lines inside `<pre>` code samples are common, so
  the rest of the snippet would be re-parsed as Markdown.
  `readHighlights` also lives here — it parses `data/highlights.md` (see "Homepage
  highlights" below), reusing the same property-list parsing as article headers.
- **`blog.fs`** — walks `source/`, decides what to regenerate (mtime of source *and all layouts* vs.
  output), renders through DotLiquid, copies static files, and builds tag/month archives and `rss.xml`.
- **`dotliquid.fs`** — DotLiquid setup: reflection-based registration of F# record types as safe
  types, template memoization keyed on last-write time, and the custom filters
  (`tagUrl`, `dateNice`, `dateShort`, `trimHtml`, `breakColons`, …) used in layouts.
- **`r2.fs`** — a hand-rolled SigV4 signer for a single S3 `PUT`, so uploading to Cloudflare R2
  needs no SDK dependency. Credentials come from `R2_*` environment variables, loaded from a
  gitignored `.env` by `build.sh` / `build.cmd` (see `.env.example`).
- **`calendar.fs`** — uploads each photo to R2 three ways: `-original.jpg` untouched, `.jpg` at
  700px and `-preview.jpg` at 240px. The imaging bits are `lazy` because System.Drawing is
  Windows-only — generating calendar *pages* must work anywhere, including Linux CI.

  **`calendar.txt` is the upload ledger, not an inventory.** It records what is already in R2,
  and doubles as the list of years the pages are generated from — which is what lets CI build
  calendar pages without the 817 MB photo library. Adding a year to it without uploading makes
  the uploader skip that year silently.

### Output that is easy to change by accident

Several filters exist purely to reproduce .NET Framework behaviour. Do not "simplify" them back
to the BCL equivalents — each rewrites a large part of the site:

- `Filters.urlEncode` reimplements .NET Framework `HttpUtility.UrlEncode` (lowercase `%xx`,
  space as `+`). Every modern .NET encoder emits **uppercase** hex, which rewrites the share
  links on every article page.
- `Filters.enGb` pins `AM`/`PM`; ICU's en-GB gives lowercase `pm` and would rewrite the
  published date on every page.
- `formatSpans` and `toHtml` in `document.fs` each strip one trailing newline that
  FSharp.Formatting 22 adds and the old version did not.

Dates are rendered in **local time**, so a build machine in another timezone shifts every
post's published time. CI needs `TZ=Europe/Prague`.

## Content conventions

Source lives in `source/`; the output URL mirrors the source path with the extension
stripped and `/index.html` appended (`source/blog/2025/foo.md` → `output/blog/2025/foo/index.html`).

Every article starts with an H1, then a bullet list of properties, then `----` fences:

```
Title Of The Post
=================

 - title: Title Of The Post
 - date: 2025-02-02T14:52:03.4153334+01:00
 - description: Shown in meta tags and listings (may span lines, may contain Markdown)
 - layout: article
 - tags: research, academic, programming languages
 - icon: fa fa-square-caret-down          # optional
 - image-large: http://tomasp.net/...png  # or "image:" for a small twitter card
 - references: true                       # appends a References section from Markdown link defs
 - format: bakedin                        # optional; see "Article formats" below

----------------------------------------------------------

Abstract paragraphs shown in listings and repeated at the top of the article.

----------------------------------------------------------

Body.
```

- `layout` names a file in `layouts/`. In practice **everything uses `article`** — the code defaults
  to `post`, but `layouts/post.html` does not exist, so omitting `layout` breaks the build. Long
  reads default to `longread` instead, and can override it like anything else.
- `format` says how the document is written — `markdown` (the default when the property is
  absent), `bakedin` or `longread`. An unrecognised value fails the build rather than falling back
  to Markdown.
- Adding ` - standalone` to the property list after the first fence means the abstract is *not*
  repeated at the start of the body.
- Titles containing `[DRAFT]` are parsed but excluded from listings. `source/blog/drafts/` holds
  in-progress posts.
- An H1 containing `:` or `?` is split into `<span class="hm">`/`<span class="hs">` halves for the
  two-tone heading style — this is why so many titles read "Something: subtitle". This applies
  only to Markdown bodies; `bakedin` posts keep whatever `<h1>` their HTML already contains.
- `source/academic/` items only appear on the publications page if they have a `date` **and** the
  `publication` tag; the `top` tag marks featured ones.
- Directory markers control traversal: `.ignore` (skip entirely), `.no-copy` (don't copy static
  files), `.no-transform` (don't parse articles). `source/articles/` is `.no-transform` — it holds
  the static assets (images, demos) that the oldest posts link to.

### Homepage highlights

The grey strip of essays and projects on the homepage comes from `data/highlights.md` — a
repo-root `data/` folder that sits next to `layouts/` and `source/`. It is *not* under
`source/`, so nothing copies or transforms it; `main.fs` watches it like `layouts/`.

Each entry is a heading, a property list and a Markdown body:

```
# Cultures of programming

 - link: http://tomasp.net/cultures
 - image: img/highlights/cultures.jpg
 - disabled: true             # optional - keeps the entry out of the page

The book tells the history of programming from the 1940s to the present (...)
```

Anything before the first heading is ignored. Entries appear in file order and `main.fs`
groups them into rows of two, so one row is one `frr` block in `layouts/index.html`; an odd
last entry sits alone in the left column, which is what `disabled` is for — it keeps a
finished entry in the file while the count stays even.
Links are absolute (the dev server rewrites them to localhost), image paths are relative to
the site root. Highlights are re-read on every pass, so `index.html` — which is regenerated
unconditionally anyway — always reflects the file.

### Posts with `format: bakedin`

177 posts carry `format: bakedin` and hold pre-rendered HTML. Two groups, same reason — their
markup cannot be regenerated:

- **`*.aspx.md` (150)** — the oldest posts, converted from an `<!-- [info] -->` HTML-comment
  format. **The `.aspx` must stay in the filename**: the output path strips only the final
  extension, so `foo.aspx.md` publishes at `/blog/foo.aspx/`. Renaming to `foo.md` would move
  the post and break every inbound link.
- **27 baked F# posts** — were literate `.fsx` scripts whose F# was type-checked to produce
  syntax colouring and hover tooltips. The original `.fsx` sits next to each `.md` as an inert
  reference copy: `copyFiles` skips `.fsx`, and no code path reads them any more.
  `source/blog/packages/` is what those scripts used to `#r` and `#load`. The restored NuGet
  trees (940 MB of DLLs) are gone; only the hand-written snippets the scripts loaded and the
  `paket.lock` files recording the versions they were built against remain. The folder keeps
  its `.ignore` marker, so nothing in it is copied or transformed.

Do not try to "restore" a baked post by re-processing its `.fsx`. Those scripts reference
packages that no longer resolve, and even where a type-check succeeds, current
FSharp.Formatting emits a different CSS class taxonomy (`pn`/`ta`/`rt`/`uc`) than the one
`custom/tooltips.css` styles (`i`/`o`/`t`/`p`) — the colouring silently disappears.

`custom/tooltips.css` and `tooltips.js` must stay: the CSS supplies `div.tip { display: none }`,
without which every tooltip's text renders inline as visible body content.

### Long reads

Longer, more academic pieces written in LaTeX and published from the same `.tex` as the PDF.
A long read is a `.md` article with ` - format: longread` plus a sibling folder of the same
name, and everything in that folder is found **by convention** — there are no properties
naming the files:

```
source/longreads/
  macros.tex                 # shared; only macros tools/latex.fs also understands
  architecture.md            # metadata + named sections of raw HTML
  architecture/
    architecture.tex         # the content, converted to the body of the page
    architecture.bib         # bibliography, rendered into a References section
    main.tex                 # pdflatex driver: its own preamble + \input{../macros}
    style.css, fig/, pdf/    # published assets
    build/                   # pdflatex scratch; carries .ignore, gitignored
```

After the header, the `.md` is a sequence of fenced **named sections** — every `----` separator
is followed by a ` - name` line, which is what lets the parser tell a name from raw HTML that
happens to start with a dash. The name is the field it fills, both optional:

- ` - head` → `LongRead.Head`, injected into the page `<head>`: this article's fonts, its
  `<meta name="keywords">`, the highlight.js language packs it needs (with their SRI hashes,
  which a generated tag could not have) and any `<style>` overrides.
- ` - frontmatter` → `LongRead.FrontMatter`, the hand-written title block shown above the body.

**This is longread-only.** `bakedin` keeps its two *unnamed* separators (abstract, body) and
`markdown` keeps `Literate.ParseMarkdownFile`; 177 posts depend on that.

Headings are **not** numbered on the web, even where the PDF numbers them: the contents list is
an `<ol>`, so the browser numbers the sections there and a prefix on the heading would show up
twice ("1. 1. Introduction"). `\ref` renders the section's position in that list, which is what
`\S\ref{sec:info}` relies on.

A `LongRead` is its own record, not an `Article` — it has no abstract, tags, icon or subtitle.
`Latex.formatDocument` returns the front matter and the body separately, and the record is passed
to `longread.html` as the model directly, so that template says `{{ model.Title }}` and
`{{ model.Head }}` with no `Article` hop.

`Blog.longReads` collects them into `Site.LongReads` (newest first, `[DRAFT]` titles excluded),
so pages rendered against `Site` — the homepage in particular — can list them. They stay out of
`Posts`, so the blog listing, the tag/month archives and `rss.xml` never see them. This means the
`.tex` is converted on every `loadSite()` as well as when the page is written; it costs nothing
measurable (a full build is ~48s either way).

The `.md` publishes at `/longreads/architecture/` under the normal URL rule, and the folder
copies to the same place — which is why `fig/x.jpg` in the `.tex` just works. `blog.fs` keeps
`.tex`/`.bib` out of the output and treats them as inputs of the `.md`, so editing the LaTeX
rebuilds the page under `build.sh`'s watcher.

`layouts/longread.html` is shared by every long read and does **not** extend `page.html`, so
there is no site nav or footer. What stays per-article is the design: `style.css` in the
article's own folder, plus whatever its `head` section adds. The shared page behaviour —
sidenote positioning and the figure carousel — is `source/custom/notes.js`; its functions must
stay global, because the converter emits inline `onclick`/`onmouseover` attributes that call
them. In the layout's `<head>`, `{{ model.Head }}` comes after `style.css` so the article's
overrides win, and `notes.js` comes last because it reads `hljs` as it registers its handler.

The converter in `tools/latex.fs` is deliberately incomplete — it pattern-matches the command
names the existing articles use and throws `Unsupported command` on anything else. That is the
intended behaviour: it never guesses. Consequences worth knowing:

- `\newcommand` is **not** expanded. A macro only works on the web if `parse` has a case for
  it, which is what `macros.tex` is for; anything defined in an article's own preamble is
  PDF-only.
- The whole `.tex` is converted - the front matter and the generated table of contents are
  placed above it rather than replacing anything.
- A few commands — `\paragraph`, `\href`, `\textsc`, `\TeX` — are deliberately passed through as
  raw LaTeX so they show up on the page, rather than being guessed at or throwing.
- The table of contents lists sections only. Anchors come from the heading text, and
  `indexHeadings` suffixes any repeat (`free_software`, `free_software_2`) so ids stay unique.

BibTeX field names are matched case-insensitively (a `bookTitle` finds `booktitle`), and a
missing required field names the entry and lists what it does have — a bare
`KeyNotFoundException` is no help in an 87-entry bibliography.

`build.sh longreads` builds the PDFs and then watches the `.tex`/`.bib`, so it can run in the
background while writing. It never touches `output/`. CI has no LaTeX, so **the PDF is
committed**. Note the raw pdflatex output for `architecture` is ~57 MB against the 1.3 MB file
actually published — the figures are full-resolution, and compressing them is a manual step.

## Layouts

DotLiquid templates in `layouts/`, all extending `page.html` (which owns `<head>`, the nav
and the footer archive list) — except `longread.html`, which is a standalone page rendered
against a `LongRead` rather than an `ArticleModel`. Records are exposed with **C# naming**, so
templates say `{{ model.Article.Title }}`, `{% for h in model.Archives.History %}`.

Special pages are rendered directly by `main.fs` rather than from a source file:
`index.html`, `404.html`, `academic/index.html` (via `papers.html`), and `blog/index.html`
(via `listing.html`, latest 20 posts). `listing.html` is reused for every tag and month archive.
The highlights strip in `index.html` is a loop over `model.Highlights`, read from
`data/highlights.md` (see "Homepage highlights" above).

Note that layouts hardcode `http://tomasp.net` in structured data and some footer links; the dev
server rewrites both `http://` and `https://` forms to `localhost` when serving.

### Analytics

No layout contains an analytics tag. `Helpers.injectTracking` inserts the Medama script right
after the opening `<head>` of **every** HTML page the generator writes — rendered pages via
`DotLiquid.render`, which every template goes through, and hand-written pages via
`Blog.copyFiles`, which for `.html` copies through `Helpers.copyHtmlFile` instead of
`File.Copy` (BOM preserved; a file that is not valid UTF-8 is copied byte-for-byte). This is
what stops standalone pages such as `source/denicek/index.html` from being missed, which is
what happened with the old Google Analytics tag.

Pages with no `<head>` (`source/blog/rss.aspx/index.html`) are left alone, and the tag is
never added twice. The dev server strips it again, so local browsing is not counted — except
for a static page requested by name rather than as a directory index
(`/coeffects/slides.html`), which `serveFile` streams unchanged.

## Deploying

Pushing to `master` triggers `.github/workflows/deploy.yml`: it checks the sources out into
`website/` and `gh-pages` into `output/` (the side-by-side layout the generator expects), runs
`build.sh build`, and commits the result to `gh-pages` if anything changed.

`TZ=Europe/Prague` is set in the workflow on purpose — post dates render in **local time**, so a
UTC runner would shift every published timestamp.

Calendar photos are the one thing CI cannot produce: the originals live outside the repo, so
`build.sh calendar` is a local-only step, run by hand when a new month's photo is added.
