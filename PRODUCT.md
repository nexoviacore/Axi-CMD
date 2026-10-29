# Product

<!-- impeccable:product-schema 1 -->

Scope: the AxiCMD command palette plugin (`AxpertPlugins/AxiCMD/`) and its API (`AxiApi/`). The Axpert Web shell is the host it lives in, not part of this product; its design is out of scope.

## Platform

web

## Users

- **Everyday Axpert users** open the palette (Ctrl+Space or the search bar in the shell) and type a command to jump to a form, report, page or record instead of clicking through menus. They work inside an Axpert business application all day and want to get somewhere fast without leaving the keyboard.
- **App managers and developers** (users with Configure Access and/or build access) also use the `configure` and `sdk` commands to open admin screens and Developer Studio builders, and, when they have both accesses (Config Page Access), the AxiCMD Config page to switch command options on or off and add Custom Options.

## Product Purpose

AxiCMD is a keyboard-driven command palette embedded in the Axpert Web shell. It parses what the user types into tokens, autocompletes structures, records and options as they type, and runs the command: opening TStructs, IViews, ADS, pages, inbox items and Developer Studio builders, in the shell or in tabbed popups. It also keeps user favourites. Success means a user reaches the screen or record they want in a few keystrokes, including on very large tables, and that administrators can change what the commands offer without code or SQL changes.

## Positioning

AxiCMD is built for Axpert specifically: its command grammar and options are data (`axi_command_config`, `axi_command_prompts`, Axpert Data Sources) rather than code, it understands Axpert's own entities (TStruct, IView, ADS, Page, Inbox) and permissions (Configure Access, build access), and it resolves records server-side on tables with 50,000+ rows. A generic command palette could not do this without Axpert's metadata.

## Operating Context

- Runs inside the Axpert shell (`aspx/mainHomeConfigTemplate.html`); the palette sits in the shell's `AXI-Sec` container, and its pages (AxiCMD Config, PopupContainer, Smartview, Analytics) open in shell iframes such as `middle1` or popups, next to Axpert's own pages.
- Commands run against customer databases on PostgreSQL or Oracle; AxiApi (.NET 8, IIS) serves command configuration and favourites, with Redis as the shared cache.
- Customer deployments may be offline or locked down.

## Capabilities and Constraints

- Commands: `configure`, `sdk`, `upload`, `download` and navigation to structures and records; options and sources come from the database. New Configure/SDK options are added only through the AxiCMD Config page, never by hand-written SQL.
- Terminology (see `CONTEXT.md`): Configure Access, build access, Config Page Access, Built-in Option, Custom Option, Dynamic Command Configuration.
- Binding constraints for every design:
  - Arabic / RTL: every page must mirror correctly under `dir="rtl"`.
  - No CDN or external assets: pages load only local CSS, JS and fonts.
  - Lives inside the Axpert shell: pages must fit in shell iframes and sit well next to Axpert's own pages; the shell header and `web.config` are not changed.
  - Keyboard-first: everything reachable and usable from the keyboard.

## Brand Commitments

Product name: AxiCMD (Axi Command Palette).

## Evidence on Hand

- Documentation: `AxpertPlugins/AxiCMD/README.md`, `AxpertPlugins/AxiCMD/docs/DOCUMENTATION.md`, `AxpertPlugins/AxiCMD/docs/DYNAMIC_COMMAND_CONFIG_USER_GUIDE.md` (and `.pdf`), `CONTEXT.md`.
- No customer testimonials, usage metrics or benchmarks exist in the repo; future work must not invent them.

## Product Principles

1. Fewest keystrokes wins: the palette exists to replace menu clicking, so every flow is judged by how quickly a typed command reaches its target.
2. Configuration is data: behaviour that varies by app or customer belongs in database rows edited through the product, not in code.
3. Respect permissions: users see and change only what their Axpert access allows.
4. A guest in the shell: AxiCMD fits into Axpert rather than restyling it.

## Accessibility & Inclusion

Keyboard-first operation and correct right-to-left (Arabic) layout are required. Reduced motion is respected.
