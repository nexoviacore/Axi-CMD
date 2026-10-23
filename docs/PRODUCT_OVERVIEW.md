# Axpert Web: Product and Codebase Overview

A guide to the repository `AxpertWebLatest` (version `AW_11.4.0.0`, see `versionInfo.json`). It covers what the product is, how the code is laid out, and where the AxiCMD command palette fits in.

> Note: facts below come from the repo (folder names, `Web.config`, `AGENTS.md`, `CONTEXT.md`, `docs/`). Statements marked **(inferred)** are my reading of naming and structure and should be checked against the team's own product documentation.

---

## 1. What the product is

**Axpert** is a low-code / metadata-driven business application platform. Developers do not hand-code each screen. They *define* forms, reports, workflows and data sources as metadata, and the runtime renders and executes them.

**Axpert Web** (this repo) is the **browser client and web host** for such an application. It:

- Authenticates users (forms login, LDAP, SAML/SSO, MSAL are all present in `App_Code`).
- Renders the application shell (menu, home page, dashboards, notifications).
- Renders metadata-defined **forms** and **reports** and sends user actions to the Axpert engine.
- Hosts **Developer Studio**, the browser tools used to design those forms, reports and data sources.
- Hosts admin screens (users, roles, responsibilities, workflows, logs).

### Core vocabulary

| Term | Meaning |
|---|---|
| **Tstruct** (transaction structure) | A data-entry form defined as metadata. Route: `tstruct.aspx?transid=<name>`. |
| **IView** (interactive view) | A report / list view with filters and drill-down. Route: `iview.aspx?ivname=<name>`. |
| **ADS** (Axpert Data Source) | A named SQL query (or function call) stored in the `axdirectsql` table. Pages call it by name with parameters. |
| **Page / Smartview / Card** | Home-page and dashboard building blocks defined as metadata. |
| **Transid** | Short id of a Tstruct, for example `sect` (Page designer) or `b_sql` (ADS builder). |
| **PEG** | Process / approval workflow engine (`AxPEG.aspx`, `AxProcessBuilder`, `WF*.js`). |
| **Developer Studio** | Browser designer for Tstructs (`tstreact`) and IViews (`ivreact`). |
| **AxiCMD / Axi** | The command palette and AI-assistant style search bar added to the shell. |

---

## 2. Technology stack

| Layer | Technology |
|---|---|
| Web host | ASP.NET **Web Forms**, .NET Framework 4.6 (`Web.config`, `AxpertWeb.sln`), IIS |
| Server code | C# in `App_Code/` (about 60 classes) plus `.aspx.cs` code-behind |
| Services | ASMX web services: `WebService.asmx` (about 110 `[WebMethod]`s), `CustomWebService.asmx`, `CloudService.asmx`, `WidgetWebService.asmx` |
| Databases | Oracle, PostgreSQL, SQL Server, MySQL helpers (`Oraclehelper.cs`, `PosrgreSQL.cs`, `Sqlhelper.cs`, `Mysqlhelper.cs`) |
| Caching / state | `MemCache.cs`, `CacheManager.cs`, `RedisServer.cs`; session mode `StateServer` |
| Real-time | SignalR (`Js/signalr`) |
| Frontend | jQuery, Handlebars, Bootstrap-style theme (`AxpertUI`), plus React pages (`aspx/reactPages.aspx`) |
| Build toolchain | Node / Gulp / Webpack / SCSS in `UI/compiler/tools` |
| Modern API | `AxiApi/` (ASP.NET Core, .NET 8) |
| Tests | `tests/*.test.js` (Node built-in runner, mock-based). No UI test suite. |

---

## 3. Repository map

```
AxpertWebLatest/
├─ Default.aspx, Global.asax, Web.config    Entry point, app lifecycle, configuration
├─ WebService.asmx / CustomWebService.asmx  Main server API used by the JS client
├─ App_Code/                                C# business logic (see section 4)
├─ aspx/                                    About 190 Web Forms pages (see section 5)
├─ Js/                                      Page scripts (*.js with *.min.js pairs)
├─ Css/, Scss/, App_Themes/, theme1/        Styling and themes
├─ CustomPages/                             Customer-overridable page templates and styles
├─ AssetsNew/, assets/, images/, fonts/     Static assets
├─ AxpertPlugins/AxiCMD/                    The AxiCMD command palette plugin
├─ AxiApi/                                  ASP.NET Core API (favourites, command config)
├─ UI/compiler/tools/                       Node/Gulp/Webpack build system
├─ UI/axpertUI/                             Compiled UI framework sources
├─ migration/                               Versioned DB migration scripts (10.2, 10.2.1, 10.3)
├─ ThirdParty/                              Pinned vendor libraries: DO NOT EDIT
├─ docs/                                    ADRs, specs, refactor plan, agent docs
├─ tests/                                   Node tests
├─ Config/, AxPaths.ini, ConfigAuthentication.ini, ExportConfig.ini   Runtime config
├─ __bkp/, *_bkp_*, *.zip                   Backups and release archives (ignore)
└─ CLAUDE.md, AGENTS.md, CONTEXT.md         Contributor / agent guidance and glossary
```

The repo contains many backup copies (`*_bkp_*`, `__bkp`, zip files). Always confirm you are editing the live file, not a backup.

---

## 4. Server side (`App_Code`, services)

Grouped by responsibility (from file names):

- **Data access:** `DBContext`, `DBquery`, `Connection`, `Sqlhelper`, `Oraclehelper`, `PosrgreSQL`, `Mysqlhelper`. These give one API across several database vendors.
- **Metadata / runtime:** `TStructData`, `IView`, `IviewData`, `IviewParams`, `ListviewData`, `Intelliview`, `ADS`, `FDR` / `FDW` / `FastData` (fast data read/write utilities), `Entity`.
- **Security / identity:** `NewLogin`, `LdapAuthentication`, `SSO`, `Saml`, `AxpertRestAPIToken`, `AntiforgeryChecker`, `Impersonator`.
- **Platform services:** `CacheManager`, `MemCache`, `RedisServer`, `LogFile`, `ExecTrace`, `AppConfiguration`, `Constants`, `Serializer`, `CloudService`.
- **Analytics and export:** `Analytics`, `AnalyticsUtils`, `IviewDataExport`, `BulkUserActivate`.

**(inferred)** Web Forms pages and ASMX methods mostly act as a thin layer. They collect the request and call a separate Axpert engine ("Axpert Scripts", path set by `ScriptsPath` / `scriptsUrlPath` in `Web.config`) that holds the metadata and executes the heavy logic. This repo is the presentation and hosting tier.

---

## 5. Pages (`aspx/`)

| Area | Examples |
|---|---|
| Login and session | `Signin`, `Forgotpassword`, `ResetPassword`, `sess`, `signout`, `samlresponse`, `Reconnect` |
| Shell / home | `AxMain`, `Mainnew`, `HomePage`, `HomeBuilder`, `dashBoard`, `cardsDashboard`, `mainHomeConfigTemplate.html` |
| Runtime forms and reports | `tstruct`, `tstructhtml`, `iview`, `htmliv`, `pdfiview`, `xmliview`, `ivpicklist`, `tsttable` |
| Developer tooling | `DeveloperStudio`, `iviewBuilder`, `AxDBScript`, `configurationStudio`, `ParamsTstruct`, `reactPages` |
| Workflow | `AxPEG`, `Workflow*`, `WorkFlowDelegation`, `processflow`, `Workflownotification` |
| Administration | `Users`, `Roles`, `Responsibilities`, `AddEditResponsibility`, `AxpertAdmin`, `adminconsole`, `AxpertLogs`, `AppSettings` |
| Data movement | `ImportAll`, `ImportNew`, `ExportNew`, `ExportasXML`, `FastDataUtility`, `excelimportgrid` |
| Misc | `Help`, `UserManual`, `Support`, `Comments`, `Drafts`, `Message`, `MobileLanding` |

Each page normally has a matching script in `Js/` (for example `tstruct` with `Js/tstruct*.js`, `iview` with `Js/iview*.js`). Both a readable `.js` and a `.min.js` exist. Edit the readable file, then regenerate or keep the minified copy in sync if the page loads it.

---

## 6. Frontend

- **Shell:** `aspx/mainHomeConfigTemplate.html` is the main layout. It holds the navigation, an `#middle1` iframe where pages load, and the `AXI-Sec` palette container.
- **Page loading model:** pages open inside iframes through shell functions in `Js/main.js` (`top.window.LoadIframe`, `ShowDimmer`, `openDeveloperStudio`). New code should reuse these, not add parallel mechanisms.
- **Build:** `UI/compiler/tools` compiles SCSS and bundles JS (`npm run build`, `watch`, `localhost`, `buildBundle`). Output goes to `dist/` (generated).
- **Theming:** `Css/`, `Scss/`, `App_Themes/`, `theme1/`, with RTL variants through `buildBundle`.
- **Customisation:** `CustomPages/` lets a deployment override templates and global styles without touching core files.

---

## 7. AxiCMD (the part you built)

A command palette in the shell. Users type commands such as `create`, `edit`, `view`, `source`, `configure`, `sdk`, `analyse`, `upload`, `download` and get ranked, token-by-token autocomplete.

```
AxpertPlugins/AxiCMD/
├─ HTMLPages/js/axicmdmain.js     The engine (one large IIFE)
├─ Structures/Postgre/Scripts/    axi_axdirectsql_tables.sql, axi_functions.sql, axi_command_tables.sql
├─ Structures/Oracle/Scripts/     same set for Oracle (keep in sync)
└─ PluginScripts/AxiCMDApi        Backend for favourites and command config
```

Key design points:

1. **Commands are data, not code.** Verbs, prompt options and sources live in `axi_command_config` and related tables, so adding a command does not need a JS change.
2. **Data comes from ADS rows.** The client calls `getList`, which calls an ADS with `param1..paramN`. Rows live in `axdirectsql`.
3. **Large lists are paged on the server.** Sources in `PAGED_SOURCES` use `param11` (search), `param12` (page), `param13` (page size, 100). The client loads on scroll and searches the server at 3+ characters. See `docs/adr/0001-large-dataset-autocomplete.md`.
4. **Name lookups must not assume the full list is loaded.** Use `findStructRow(...)` with the `requestStructSearch(...)` fallback.
5. **Three cache layers:** in-memory `axDatasourceObj`, `localStorage` (30-minute TTL), and the server-side ADS cache (6 hours). Recycle the app pool or clear the ADS cache after changing an ADS row.
6. **Cache-busting:** bump `axicmdmain.js?v=N` in `mainHomeConfigTemplate.html` whenever the script changes (currently `v=11`).

Full detail is in `CLAUDE.md`, `CONTEXT.md` and `docs/adr/`.

---

## 8. Typical request flow

```
Browser (shell + iframe pages)
   │  jQuery / fetch -> ASMX WebMethod (WebService.asmx) or ADS call
   ▼
ASP.NET host (App_Code: auth, cache, DB helpers, ADS runner)
   │  loads metadata, runs ADS SQL / functions
   ▼
Axpert engine and database (Oracle / PostgreSQL / SQL Server / MySQL)
   ▲
   └─ results returned as JSON / HTML and rendered by Js/*.js
```

Example: typing `edit invoice 1001` in AxiCMD resolves `invoice` through `axi_structmetalist`, finds record `1001` through `axi_getstructsdata`, then calls `LoadIframe` to open `tstruct.aspx?transid=...&recordid=...` in the shell.

---

## 9. Working in the repo

- **Build:** `cd UI/compiler/tools; npm install; npm run build`.
- **Syntax check the palette:** `node --check AxpertPlugins/AxiCMD/HTMLPages/js/axicmdmain.js`.
- **Tests:** `node --test tests/` (mock-based, they do not load the real script).
- **Validation:** manual, in the browser (Ctrl+F5, DevTools Network and console). There is no UI test suite.
- **Style:** 4 spaces, no tabs. Preserve each file's line endings (SQL files are CRLF).
- **Do not touch:** `ThirdParty/`. Do not commit secrets or environment-specific URLs.
- **Commits:** Conventional Commits, `type(scope): subject`.
- **Issues:** GitHub Issues via `gh`. See `docs/agents/`.

## 10. Hazards to know about

- Many backup files and zips sit next to live code. Check the path before editing.
- `Web.config` has `compilation debug="true"`, and holds environment paths (`ScriptsPath`, `scriptsUrlPath`). Do not commit environment-specific values.
- `.js` and `.min.js` pairs can drift. Confirm which one the page actually loads.
- Oracle and PostgreSQL scripts must stay in sync. Oracle paging rows and Oracle `axi_getstructsdata` parameter typing are still untested.
- Open item: `fn_axi_getstructs_obj` search on a structure with no key field (`NA`) fails with `42703 column p.na does not exist` (Postgres, and the Oracle version has the same weakness).
