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
```

There is no test suite and no linter. To check a change against what is published, point `../output` at a
checkout of the `gh-pages` branch and regenerate: a change that alters nothing produces an empty
`git -C ../output diff`. Use `git diff` rather than `git status` — status reports stat-cache
noise on files whose line endings git normalises.

A full build from scratch takes about a minute. There is no cache: every article is re-rendered
every time, by design (the old mtime-keyed JSON cache silently served content rendered by a
different FSharp.Formatting version — see "Posts with `rawbody: true`" below).

## Architecture

`tools/main.fs` holds the `SiteConfig` (all paths, `Root = "http://tomasp.net"`), the
`updateSite` / `regenerateSite` entry points, an `HttpListener` dev server with websocket
live-reload, and a `FileSystemWatcher`. Editing anything in `layouts/` triggers a full pass;
editing content refreshes only the changed files. `regenerate` deletes everything in `output/`
except `.git`.

Module compile order (`domain` → `helpers` → `dotliquid` → `document` → `r2` → `calendar` → `blog` → `main`):

- **`domain.fs`** — `SiteConfig`, `Article<'T>` (generic over `MarkdownParagraphs` before formatting
  and `string` after), `Site`/`ArticleModel` (the DotLiquid view models), calendar types.
- **`document.fs`** — parses one source file into an `Article`. `.md` is the only input format,
  in two flavours:
  - plain Markdown, via `Literate.ParseMarkdownFile`
  - `rawbody: true` in the header — only the header is Markdown; the abstract and body are
    raw HTML emitted **verbatim**
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

----------------------------------------------------------

Abstract paragraphs shown in listings and repeated at the top of the article.

----------------------------------------------------------

Body.
```

- `layout` names a file in `layouts/`. In practice **everything uses `article`** — the code defaults
  to `post`, but `layouts/post.html` does not exist, so omitting `layout` breaks the build.
- Adding ` - standalone` to the property list after the first fence means the abstract is *not*
  repeated at the start of the body.
- Titles containing `[DRAFT]` are parsed but excluded from listings. `source/blog/drafts/` holds
  in-progress posts.
- An H1 containing `:` or `?` is split into `<span class="hm">`/`<span class="hs">` halves for the
  two-tone heading style — this is why so many titles read "Something: subtitle". This applies
  only to Markdown bodies; `rawbody` posts keep whatever `<h1>` their HTML already contains.
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

### Posts with `rawbody: true`

177 posts carry `rawbody: true` and hold pre-rendered HTML. Two groups, same reason — their
markup cannot be regenerated:

- **`*.aspx.md` (150)** — the oldest posts, converted from an `<!-- [info] -->` HTML-comment
  format. **The `.aspx` must stay in the filename**: the output path strips only the final
  extension, so `foo.aspx.md` publishes at `/blog/foo.aspx/`. Renaming to `foo.md` would move
  the post and break every inbound link.
- **27 baked F# posts** — were literate `.fsx` scripts whose F# was type-checked to produce
  syntax colouring and hover tooltips. The original `.fsx` sits next to each `.md` as an inert
  reference copy: `copyFiles` skips `.fsx`, and no code path reads them any more.

Do not try to "restore" a baked post by re-processing its `.fsx`. Those scripts reference
packages that no longer resolve, and even where a type-check succeeds, current
FSharp.Formatting emits a different CSS class taxonomy (`pn`/`ta`/`rt`/`uc`) than the one
`custom/tooltips.css` styles (`i`/`o`/`t`/`p`) — the colouring silently disappears.

`custom/tooltips.css` and `tooltips.js` must stay: the CSS supplies `div.tip { display: none }`,
without which every tooltip's text renders inline as visible body content.

## Layouts

DotLiquid templates in `layouts/`, all extending `page.html` (which owns `<head>`, the nav
and the footer archive list). Records are exposed with **C# naming**, so templates say
`{{ model.Article.Title }}`, `{% for h in model.Archives.History %}`.

Special pages are rendered directly by `main.fs` rather than from a source file:
`index.html`, `404.html`, `academic/index.html` (via `papers.html`), and `blog/index.html`
(via `listing.html`, latest 20 posts). `listing.html` is reused for every tag and month archive.
The highlights strip in `index.html` is a loop over `model.Highlights`, read from
`data/highlights.md` (see "Homepage highlights" above).

Note that layouts hardcode `http://tomasp.net` in structured data and some footer links; the dev
server rewrites both `http://` and `https://` forms to `localhost` when serving.

## Deploying

Pushing to `master` triggers `.github/workflows/deploy.yml`: it checks the sources out into
`website/` and `gh-pages` into `output/` (the side-by-side layout the generator expects), runs
`build.sh build`, and commits the result to `gh-pages` if anything changed.

`TZ=Europe/Prague` is set in the workflow on purpose — post dates render in **local time**, so a
UTC runner would shift every published timestamp.

Calendar photos are the one thing CI cannot produce: the originals live outside the repo, so
`build.sh calendar` is a local-only step, run by hand when a new month's photo is added.
