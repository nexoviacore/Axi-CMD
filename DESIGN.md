---
name: AxiCMD
description: A keyboard-first command palette that lives quietly inside the Axpert shell.
colors:
  console-ink: "#1d2547"
  console-ink-hover: "#2c3766"
  signal-indigo: "#3f51c2"
  signal-tint: "#eceffb"
  hint-amber: "#f59e0b"
  slate: "#5b6482"
  paper: "#f4f5f9"
  sheet: "#ffffff"
  header-wash: "#f8f9fc"
  rule: "#dde1eb"
  field-stroke: "#c9cedb"
  error-red: "#a3261a"
  error-tint: "#fcefed"
typography:
  display:
    fontFamily: "Bahnschrift, DIN Alternate, Segoe UI, system-ui, sans-serif"
    fontSize: "28px"
    fontWeight: 600
    lineHeight: 1.15
  headline:
    fontFamily: "Bahnschrift, DIN Alternate, Segoe UI, system-ui, sans-serif"
    fontSize: "20px"
    fontWeight: 600
    lineHeight: 1.2
  body:
    fontFamily: "Segoe UI, system-ui, -apple-system, Noto Sans Arabic, sans-serif"
    fontSize: "14px"
    fontWeight: 400
    lineHeight: 1.5
  label:
    fontFamily: "Segoe UI, system-ui, -apple-system, Noto Sans Arabic, sans-serif"
    fontSize: "12.5px"
    fontWeight: 600
    lineHeight: 1.4
  code:
    fontFamily: "Consolas, Cascadia Mono, Courier New, monospace"
    fontSize: "12.5px"
    fontWeight: 400
    lineHeight: 1.45
rounded:
  xs: "4px"
  sm: "6px"
  md: "8px"
  pill: "999px"
spacing:
  xs: "4px"
  sm: "8px"
  md: "16px"
  lg: "24px"
components:
  button-primary:
    backgroundColor: "{colors.console-ink}"
    textColor: "{colors.sheet}"
    rounded: "{rounded.sm}"
    padding: "0 16px"
    height: "38px"
  button-primary-hover:
    backgroundColor: "{colors.console-ink-hover}"
    textColor: "{colors.sheet}"
  button-secondary:
    backgroundColor: "{colors.sheet}"
    textColor: "{colors.console-ink}"
    rounded: "{rounded.sm}"
    padding: "0 16px"
    height: "38px"
  button-secondary-hover:
    backgroundColor: "{colors.paper}"
  button-small:
    backgroundColor: "{colors.sheet}"
    textColor: "{colors.console-ink}"
    rounded: "{rounded.sm}"
    padding: "0 10px"
    height: "30px"
  input-text:
    backgroundColor: "{colors.sheet}"
    textColor: "{colors.console-ink}"
    rounded: "{rounded.sm}"
    padding: "0 12px"
    height: "38px"
  command-tab:
    textColor: "{colors.slate}"
    typography: "{typography.display}"
  command-tab-selected:
    textColor: "{colors.console-ink}"
  table-sheet:
    backgroundColor: "{colors.sheet}"
    rounded: "{rounded.md}"
  table-header:
    backgroundColor: "{colors.header-wash}"
    textColor: "{colors.slate}"
    typography: "{typography.label}"
    padding: "10px 16px"
  badge-builtin:
    backgroundColor: "{colors.paper}"
    textColor: "{colors.slate}"
    rounded: "{rounded.xs}"
    padding: "1px 6px"
  switch-on:
    backgroundColor: "{colors.signal-indigo}"
    rounded: "{rounded.pill}"
    width: "34px"
    height: "20px"
  page-current:
    backgroundColor: "{colors.signal-tint}"
    textColor: "{colors.signal-indigo}"
    rounded: "{rounded.sm}"
  note-info:
    backgroundColor: "{colors.signal-tint}"
    textColor: "{colors.console-ink}"
    rounded: "{rounded.sm}"
  note-error:
    backgroundColor: "{colors.error-tint}"
    textColor: "{colors.error-red}"
    rounded: "{rounded.sm}"
---

# Design System: AxiCMD

## Overview

**Creative North Star: "The Quiet Console"**

AxiCMD is a precise tool that stays out of the way. It lives inside the Axpert shell, next to Axpert's own forms and reports, so it never competes with its host: surfaces are flat, white and thinly ruled, the density is that of a working admin tool, and almost everything is ink on paper. The work is done by type and by one signal colour. The words a user types (the command verbs such as Configure and SDK) are the only element allowed to be large.

Colour is information, never decoration. Console Ink means "read this" or "press this"; Signal Indigo means "this is selected or switched on". Each palette verb keeps its own small colour mark so users learn to recognise commands at a glance. Motion appears only in answer to something the user did: a drawer sliding in, the selected-tab bar moving, rows fading in after a change.

The frosted-glass look with the amber-to-coral gradient (still on the version modal, `axicmdmain.css` section 9) is legacy. It is not part of this system, and that surface should be brought in line when it is next touched. The gear menu and the "Records per load" panel (section 8) already follow this system.

**Key Characteristics:**
- Flat white sheets on a cool paper page, separated by 1px rules.
- One signal colour (indigo) for state; navy ink for text and actions.
- Bahnschrift for command words, titles and numbers; Segoe UI for everything else.
- Small, consistent corners (4-8px); nothing pill-shaped except switches.
- Keyboard-first, right-to-left ready, local assets only.

## Colors

A cool, near-monochrome navy-and-paper palette with one indigo signal and one amber hint.

### Primary
- **Console Ink** (`console-ink`): body text, headings, primary buttons (Add option, Save option) and the selected command word. The colour of reading and of acting. Its hover step is **Console Ink Hover** (`console-ink-hover`).

### Secondary
- **Signal Indigo** (`signal-indigo`): only for state. The bar under the selected command tab, an Active switch that is on, the current page number, focus rings and the focused field border. **Signal Tint** (`signal-tint`) is its pale wash, used behind the current page number and informational notes.

### Tertiary
- **Hint Amber** (`hint-amber`): the inline completion hint inside the palette's input. Nowhere else.

### Neutral
- **Slate** (`slate`): secondary text, column headers, unselected command words, help text, counts.
- **Paper** (`paper`): the page background behind sheets; also the hover fill for secondary buttons.
- **Sheet** (`sheet`): tables, drawers, inputs and buttons.
- **Header Wash** (`header-wash`): table header rows.
- **Rule** (`rule`): dividers, sheet borders, row separators.
- **Field Stroke** (`field-stroke`): input and button borders, one step darker than Rule so controls read as controls.
- **Error Red** (`error-red`) with **Error Tint** (`error-tint`): field errors and failure notes.

### Verb marks
Each palette verb has a fixed colour on its small square icon in the command list (`--axi-cmd-color-*` in `axicmdmain.css`; for example configure violet `#7c3aed`, sdk magenta `#c026d3`, run teal `#0d9488`, help blue `#2563eb`). These marks are recognition aids, not accents: they stay on the 20px icon square and never spread to text, borders or backgrounds.

### Named Rules
**The Signal Means On Rule.** Signal Indigo marks only what is selected, switched on, current or focused. If a thing is not in one of those states, it is not indigo.

**The No Gradient Rule.** No gradients anywhere: not on buttons, page backgrounds or highlights.

**The Red Is Not An Action Rule.** Red appears only for errors. Primary actions are Console Ink, never red, coral or amber.

## Typography

**Display Font:** Bahnschrift (with DIN Alternate, then Segoe UI and system-ui)
**Body Font:** Segoe UI (with system-ui, -apple-system, Noto Sans Arabic)
**Code Font:** Consolas (with Cascadia Mono, Courier New)

**Character:** Bahnschrift, the DIN-style face that ships with Windows, gives command words and numbers an engineered, signage-like voice; Segoe UI keeps everything else familiar to Windows-based Axpert users. No web fonts are loaded; on systems without Bahnschrift the display role falls back to Segoe UI and the layout still holds.

### Hierarchy
- **Display** (600, 28px, 1.15): command words as tabs (Configure, SDK). 24px below 600px wide.
- **Headline** (600, 20px, 1.2): page titles and drawer titles.
- **Body** (400, 14px, 1.5): table cells, notes, descriptions; descriptions capped at 70ch.
- **Label** (600, 12.5-13px): column headers, field labels, switch labels, small buttons. Sentence case.
- **Code** (400, 12.5px, 1.45): URLs and identifiers only (the Target column), always left-to-right.

Counts and page numbers use Bahnschrift with tabular figures.

### Named Rules
**The Typed Word Rule.** Only words the user types into the palette may be set at display size.

**The Sentence Case Rule.** Labels, headers and buttons are sentence case. No all-caps labels, no tracked-out eyebrows on new surfaces.

**The Code Is Code Rule.** Monospace is reserved for URLs, paths and identifiers; never for ordinary labels or numbers.

## Layout

Pages are full-width inside their iframe with 24px top padding and fluid side padding (`clamp(14px, 2.4vw, 32px)`), left-aligned (start-aligned in RTL). They stack vertically with 18px gaps: header, command tabs with search and actions sitting on one rule, the table sheet, the pager. The page scrolls as a whole, so sticky table headers stick to the top of the iframe; inner scroll boxes are avoided.

Spacing steps are 4, 8, 16 and 24px; table cells use 10px by 16px padding. At 900px and below, secondary columns (Type) drop away; at 600px and below the command tabs take a full line, search and the primary action share the next, switch labels hide, and drawers go full width.

All spacing uses logical properties (inline-start and inline-end) so pages mirror under `dir="rtl"`.

**The Own Direction Rule.** The page's strings are English even inside a right-to-left shell. Every sentence block (subtitles, notes, help, errors, status, the range) and every typed phrase takes its direction from its own text (`unicode-bidi: plaintext`), so punctuation, numbers and brackets stay in place; in RTL those sentence blocks still align right with their labels.

## Elevation & Depth

Flat by default. Surfaces are separated by 1px Rule borders and by the tonal step from Paper to Sheet, not by shadows. Shadows appear only on things that float above the page: the scrim-backed side drawer, and the palette's dropdown and modals.

### Shadow Vocabulary
- **Floating edge** (`box-shadow: -16px 0 40px -20px rgba(29, 37, 71, 0.35)`, mirrored in RTL): the side drawer's leading edge.
- **Toast lift** (`box-shadow: 0 10px 28px -14px rgba(29, 37, 71, 0.45)`): the status toast floating at the bottom of the page.
- **Scrim** (`rgba(29, 37, 71, 0.28)` fill, no blur): behind a modal drawer.

### Named Rules
**The Flat-By-Default Rule.** Anything in the page flow has no shadow. Only overlays lift.

**The No Glass Rule.** No backdrop blur and no translucent panels; overlays are solid Sheet.

## Shapes

Small, consistent corners scaled to the element's size: 4px for badges and verb icons, 6px for buttons, inputs and notes, 8px for sheets and the palette's cards. Switch tracks are the only fully rounded shape. Borders are always 1px; the only thicker line is the 3px Signal bar under the selected command tab. Sheets clip their content to their corners.

## Components

### Buttons
Precise and restrained: thin borders, small corners, state shown by colour, never by effects.
- **Shape:** gently squared (6px), 38px tall; small buttons 30px.
- **Primary:** Console Ink fill with white Segoe UI 600 text; one per view (Add option, Save option).
- **Secondary:** white with a Field Stroke border and Console Ink text (Cancel, Edit, View, Previous, Next).
- **Hover / Focus:** primary darkens to Console Ink Hover; secondary fills with Paper. Focus is a 2px Signal Indigo outline offset 2px.
- **Disabled:** 40-55% opacity.

### Inputs / Fields
- **Style:** white, 1px Field Stroke, 6px corners, 38px tall; labels above in Label style, help text below in 12px Slate.
- **Focus:** border turns Signal Indigo, plus the focus outline.
- **Error / Disabled:** Error Red border and a bold Error Red message beneath; disabled fields sit on Paper with Slate text.

### Command tabs (signature)
The command words themselves, set in Display type, unselected in Slate and selected in Console Ink. A 3px Signal Indigo bar sits under the selected word on the shared rule and slides to the newly selected word (0.24s ease-out). Option counts sit beside each word as small raised Slate numbers.

### Table sheet
- **Container:** one white sheet, 1px Rule border, 8px corners, no shadow.
- **Header:** Header Wash background, Label type in Slate, sticky to the top of the page.
- **Rows:** 1px Rule separators, a faint cool hover. Inactive rows recede by colour and weight (the option name drops to Slate, regular), never by opacity, so every cell keeps AA contrast.
- **"You type" cell (signature):** each row reads as the palette phrase: the command word in Slate, the option name in Console Ink 600, and the record source as a Slate third word in angle quotes (`‹Users›`, never broken across lines). The phrase is one isolated run, so it keeps its word order in RTL.
- **Built-in badge:** a small Paper tag with a Rule border and Slate text after the phrase.
- **Just saved:** the saved option's row is scrolled into view and tinted Signal Tint for about 2.4s, then fades out (0.8s); no fade with reduced motion.

### Active switch
A 34 by 20px track: grey when off, Signal Indigo when on, with a white knob. A Console Ink "Active" or Slate "Inactive" label sits beside it on wide screens.

### Side drawer
A solid white sheet that slides in from the inline-end edge (0.3s) over a navy scrim, with a ruled header and footer. Fields appear at once; nothing inside the drawer animates.

### Advanced disclosure
Rarely needed form fields sit under a 1px Rule in a collapsed "Advanced (optional)" section with a small CSS-drawn chevron (along the reading direction when closed, down when open, mirrored in RTL). It opens by itself when the record already uses one of its fields or when one of them has an error.

### Filter (segmented)
Joined small buttons (All / Custom / Inactive) sharing borders, 38px tall, 6px outer corners. The pressed choice is selected, so it takes Signal Tint, a Signal Indigo border and Signal Indigo text; the others stay secondary.

### Pager
The range ("76-100 of 203") in Bahnschrift Slate at the start; small secondary buttons for Previous, page numbers and Next at the end, shown only when there is more than one page. The current page uses Signal Tint, a Signal Indigo border and Signal Indigo text.

### Notes
Inline status lines: Signal Tint with Console Ink text for information, Error Tint with Error Red text for failures. 6px corners and a 1px border in a deeper tint.

### Popover (palette gear menu, "Records per load")
A small solid Sheet anchored to the button that opened it: 1px Rule border, 8px corners, a soft overlay shadow (`0 14px 36px -16px rgba(29, 37, 71, 0.4)`), and a 0.14s fade-in (none with reduced motion). Menu entries are full-width rows that fill with Paper on hover and take an inset Signal ring on keyboard focus. A settings popover has a Bahnschrift title, a Slate one-line explanation, its controls, actions at the end (secondary first, Console Ink primary last) and a ruled Slate hint at the foot. A value with a range pairs a plain number field (unit after it) with a thin slider whose filled part, the current value, is Signal Indigo; the fill starts from the right in RTL. Errors are a bold Error Red line under the control, never a box.

### Toast
The page's status after an action floats centred at the bottom of the page (Toast lift shadow), so it never pushes content. It names the object and who is affected, may carry one secondary action button (Undo, also Ctrl+Z), clears itself after 8s, and errors stay until the next message.

### Verb marks (palette)
A 20px square icon with 4px corners, filled with the verb's own colour, before each command in the palette list. The rest of the row stays neutral.

## Do's and Don'ts

### Do:
- **Do** keep surfaces flat: white Sheet on Paper with 1px Rule borders.
- **Do** use Signal Indigo only for selected, on, current and focused states.
- **Do** make Console Ink the colour of every primary action, one primary action per view.
- **Do** set command words in Bahnschrift at display size and keep all other text in Segoe UI.
- **Do** use logical properties and check every page under `dir="rtl"`.
- **Do** make every control reachable by keyboard with a visible 2px Signal Indigo focus outline.
- **Do** load only local CSS, JS and fonts.
- **Do** respect reduced motion: drawers and tab bars simply appear.

### Don't:
- **Don't** use frosted glass, backdrop blur or translucent panels.
- **Don't** use gradients on buttons, highlights or backgrounds.
- **Don't** use red, coral or amber for actions.
- **Don't** put shadows on anything that sits in the page flow.
- **Don't** add decorative motion: no staggered entrances, bouncy easing or animations the user did not cause.
- **Don't** use all-caps labels or monospace for non-code text.
- **Don't** restyle the Axpert shell around AxiCMD.
