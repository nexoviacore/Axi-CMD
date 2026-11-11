---
target: AxiCMDConfig.html
total_score: 25
max_score: 40
na_heuristics: 
p0_count: 0
p1_count: 2
target_identity: "file:D:\\Axpert11.4\\AxpertWebLatest\\AxpertPlugins\\AxiCMD\\HTMLPages\\AxiCMDConfig.html"
target_fingerprint: "sha256:b3104d742e271ad33af0bc6ec8bdf924fd78fb21eddf673dacdd9d07118d6e40"
target_path: "D:\\Axpert11.4\\AxpertWebLatest\\AxpertPlugins\\AxiCMD\\HTMLPages\\AxiCMDConfig.html"
timestamp: 2026-10-29T10-03-35Z
slug: axpertplugins-axicmd-htmlpages-axicmdconfig-html
closed: true
---
Method: dual-agent (A: design-review sub-agent, B: detector sub-agent)

## Design Health Score
| # | Heuristic | Score | Key issue |
|---|---|---|---|
| 1 | Visibility of System Status | 2 | Saving/Saved note far from the toggled switch, shifts rows, never clears; loading is only dimmed rows |
| 2 | Match System / Real World | 3 | Plain copy; jargon leaks (Param field, raw ADS names, raw tstruct vs "Form (tstruct)") |
| 3 | User Control and Freedom | 2 | No undo for a global switch-off; Esc/scrim discards a half-filled drawer |
| 4 | Consistency and Standards | 3 | Two save models in the drawer; two different counts |
| 5 | Error Prevention | 3 | No confirm on switch-off; duplicate check only sees the current page |
| 6 | Recognition Rather Than Recall | 3 | Tab counts ignore search |
| 7 | Flexibility and Efficiency | 2 | No filter, sort, bulk or page size; no Add shortcut |
| 8 | Aesthetic and Minimalist Design | 3 | Header range floats away from what it counts |
| 9 | Error Recovery | 2 | Load error has no retry; failed toggle only reverts with a top note |
| 10 | Help and Documentation | 2 | Record source / Param field / Extra parameters unexplained |
| Total | | 25/40 | Acceptable |

## Design Specificity Verdict
Command-word tabs are product-specific; everything below is a stock admin kit. Missed opportunity: rows never show the typed phrase.
Detector: real page 0 findings (runtime rows invisible to it); mocks with real CSS: 2 low-contrast on tr.ac-off (name 3.6:1, type/target 2.3:1). flat-type-hierarchy on first mock run was a false positive (CSS failed to load). Browser overlay unavailable (no mutable browser tool).

## Priority Issues
- [P1] Toggling loses keyboard focus and shifts the table (axiconfigpage.js:167-185, 453-508; note at AxiCMDConfig.html:36). Fix: patch the row in place, keep focus, reserved note slot or bottom toast. Command: /impeccable harden
- [P1] Switch-off has no "affects everyone" cue and no undo. Fix: subtitle says "for everyone in this app"; per-row "Hidden for all users. Undo" for ~6s; auto-clear note. Command: /impeccable clarify
- [P2] Inactive rows fail contrast (2.3:1, 3.6:1) via opacity 0.55. Fix: drop opacity; Slate text + switch + label. Command: /impeccable polish
- [P2] role="tabpanel" on the table breaks table semantics; no-access and load-error states silent. Fix: move role to .ac-scroll; standing no-access note; Try again button. Command: /impeccable harden
- [P2] Saved option not findable; tab counts ignore search (CountCommandOptionsByCommand has no search); header range ambiguous. Fix: jump to and highlight the row; search-aware counts; range by the pager; All/Custom/Inactive filter. Command: /impeccable layout

## Persona Red Flags
- Alex: no Custom-only filter; other-tab matches invisible; table jumps after toggle; tab through Type/Name/URL to Save.
- Sam: table semantics lost; focus lost after each toggle; stale Saved note; disabled Built-in fields skipped by Tab.
- App manager: cannot see what was customised; global switch-off without cue; RTL layout but English-only strings.

## Minor Observations
- Name column has no max width (200-char names squeeze Target).
- Table shows raw types, drawer friendly labels.
- Drawer is a flat six-field stack; Advanced disclosure; Record source lists 28 raw names.
- Esc/scrim discards unsaved input without asking.
- Plain "Loading options..." text; skeleton rows would keep layout steady.

## Questions to Consider
- Should each row read as the typed phrase, with the record source as a faint third word?
- Is "calm" the right register for the control that can hide Users for a whole company?
- Are Built-in and Custom really one list?
