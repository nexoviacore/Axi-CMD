# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

See also `AGENTS.md` (style, commit and PR rules) and `CONTEXT.md` (AxiCMD glossary). Read `docs/adr/` before changing AxiCMD data loading.

## What this is
An ASP.NET (.NET Framework, `AxpertWeb.sln`) Axpert Web shell plus a JavaScript frontend toolchain. Most day-to-day work happens in the **AxiCMD command palette** plugin under `AxpertPlugins/AxiCMD/`. `ThirdParty/` is pinned vendor code: do not edit it.

## Commands
Run from `UI/compiler/tools` (Node/Gulp/Webpack):
- `npm install`, `npm run build` (production bundles), `npm run watch`, `npm run localhost` (dev server on `localhost:8080`), `npm run buildBundle` (standard + RTL), `gulp clean`.
- `npm test` is a placeholder and fails by design.
- Repo-level tests in `tests/*.test.js` use Node's built-in runner: `node --test tests/` or one file with `node --test tests/axi-autocomplete-ticket-248.test.js`. They re-implement logic with mock data rather than loading `axicmdmain.js`, so they do not prove the real script works.
- Quick syntax check of the palette script: `node --check AxpertPlugins/AxiCMD/HTMLPages/js/axicmdmain.js`.
- There is no automated UI test. Validate in the browser (Ctrl+F5, DevTools Network and console).

## AxiCMD architecture (needs several files to see)
- **Shell host:** `aspx/mainHomeConfigTemplate.html` renders the shell and the `AXI-Sec` palette, and loads `AxpertPlugins/AxiCMD/HTMLPages/js/axicmdmain.js?v=N`. **Bump `?v=N` whenever the script changes** (browser cache).
- **Engine:** `axicmdmain.js` (very large, one IIFE) parses tokens, resolves entities, ranks suggestions, runs commands. Data lives in `axDatasourceObj`, keyed `"<source>_<params>"` (lower case); search results are stored under `<key>_q_<term>`.
- **Commands are data-driven:** command grammar comes from DB tables (`axi_command_config`, `axi_command_tables.sql`); `configure` and `sdk` prompt options and sources are rows, not code.
- **Data sources are Axpert Data Sources (ADS):** rows in `axdirectsql` (inline SQL or a call to a function). The client calls them through `getList` -> ADS request with `sqlParams` `param1..paramN`. The DB scripts exist twice, in `AxpertPlugins/AxiCMD/Structures/Postgre/Scripts` and `.../Oracle/Scripts` (`axi_axdirectsql_tables.sql`, `axi_functions.sql`). Keep both in sync.
- **Large-list paging convention:** sources in `PAGED_SOURCES` take `param11` = search term, `param12` = page no, `param13` = page size (100). Paging/search is applied in the ADS query itself, so the client sends engine `pagesize: 0`. Never rename existing SQL functions. `fn_axi_getstructures_meta` and `fn_axi_getstructs_obj` do the search/paging themselves through trailing defaulted parameters (`psearchterm`, `ppageno`, `ppagesize`), and the ADS row only passes `param11..param13`; `axi_viewlist` is still wrapped in its ADS row. In a Postgres ADS row the engine binds every param as `varchar`, so cast integers explicitly (`cast(nullif(cast(:param12 as varchar),'') as integer)`) and avoid `::` (the `:name` parser trips on it). Quotes inside the stored SQL text are doubled.
- **Cache layers to remember:** in-memory `axDatasourceObj`, `localStorage` (`axi_<ads>_<params>_v2`, JSON `{t, d}`, 30-minute TTL, size-capped), and the server-side ADS cache (`6 Hr`). After changing an ADS row, recycle the app pool or clear the ADS cache, otherwise the old query keeps running.
- **Structure list is paged:** `axi_structmetalist` returns only a first page. Code that looks up a form/report must use `findStructRow(...)` and fall back to `requestStructSearch(...)` instead of assuming the whole list is loaded. Rows merged in from a search carry `__fromSearch` and are hidden from the plain list.
- **Shell hooks:** reuse `top.window.LoadIframe`, `openDeveloperStudio`, `ShowDimmer` (all in `Js/main.js`) instead of new mechanisms. `openDeveloperStudio` is asynchronous, which is why `holdDimmerUntilStudioLoads` keeps the loader on.
- **Backend API:** `AxiCMD/PluginScripts/AxiCMDApi` and `AxiApi/` (ASP.NET Core, .NET 8) serve command configuration and favourites.

## Gotchas
- Line endings are mixed in the working copy (SQL files CRLF; git normalises). Preserve each file's existing endings and 4-space indentation.
- Editing JS with node/sed one-liners can silently drop backslashes in regexes; re-read regex lines after such edits.
- Commit messages: Conventional Commits, `type(scope): subject` (e.g. `feat(axi-cmd): ...`).
