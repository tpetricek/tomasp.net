# tomasp.net

Source and static-site generator for [tomasp.net](https://tomasp.net). The generator is a small
F# console app in `tools/`, so all you need is the [.NET 10 SDK](https://dotnet.microsoft.com/download).

## Running it

```bash
./build.sh              # rebuild, watch for changes, serve on http://localhost:11112
./build.sh build        # one-off full rebuild into ../output
./build.sh calendar     # resize calendar photos and upload them (see below)
```

Use `build.cmd` instead on Windows. The site is written to `../output`, a scratch directory
outside the repo that is created on demand and can be deleted at any time.

## Writing a post

Posts live in `source/blog/<year>/` and academic pages in `source/academic/`. A post is Markdown
with a header, an abstract, and the body, separated by `---` rules:

```
Title of the post
=================

 - title: Title of the post
 - date: 2026-02-02T14:52:03.4153334+01:00
 - description: Shown in listings, search results and social previews.
 - layout: article
 - tags: research, programming languages

--------------------------------------------------------------------------------
 - standalone

The abstract, shown in listings and the RSS feed.

--------------------------------------------------------------------------------

The body.
```

`standalone` means the abstract is not repeated at the top of the body. A title containing
`[DRAFT]` is built but kept out of listings. Everything else in `source/` — images, demos,
stylesheets — is copied across unchanged.

Some older posts carry `rawbody: true` in the header, meaning their abstract and body are raw HTML
rather than Markdown. Leave that alone; see `CLAUDE.md` for why.

## Calendar photos

Photos are not kept in this repo. Drop the original as `../calendar/<year>/<month>.jpg` and run
`./build.sh calendar`, which resizes it, uploads all three sizes to Cloudflare R2 and records the
month in `calendar.txt`. Credentials go in a `.env` file — copy `.env.example` and fill it in.

## Publishing

Push to `master`. GitHub Actions rebuilds the site and commits it to the `gh-pages` branch; there
is nothing to run locally and no secrets to configure.
