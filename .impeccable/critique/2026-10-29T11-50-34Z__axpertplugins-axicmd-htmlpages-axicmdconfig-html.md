---
target: AxiCMDConfig.html
total_score: 26
max_score: 40
na_heuristics: 
p0_count: 0
p1_count: 1
target_identity: "file:D:\\Axpert11.4\\AxpertWebLatest\\AxpertPlugins\\AxiCMD\\HTMLPages\\AxiCMDConfig.html"
target_fingerprint: "sha256:21dda63a3ede569658c9c00b41b29f6fdf2b22c3758f62b37df3ee5dd3012af4"
target_path: "D:\\Axpert11.4\\AxpertWebLatest\\AxpertPlugins\\AxiCMD\\HTMLPages\\AxiCMDConfig.html"
timestamp: 2026-10-29T11-50-34Z
slug: axpertplugins-axicmd-htmlpages-axicmdconfig-html
---
Method: dual-agent (A: design-review sub-agent, B: detector sub-agent). Second run, after steps 1-7.

## Design Health Score
| # | Heuristic | Score | Key issue |
|---|---|---|---|
| 1 | Visibility of System Status | 3 | Failed first load leaves tab counts at 0 |
| 2 | Match System / Real World | 3 | "Target" is jargon; shows a form ID as if a URL |
| 3 | User Control and Freedom | 3 | Undo only for switch-off; toast clears after 8s |
| 4 | Consistency and Standards | 3 | Edit/View buttons differ in width |
| 5 | Error Prevention | 2 | No inline warning on switch-off; duplicate check page-local |
| 6 | Recognition Rather Than Recall | 3 | No marker for Custom Options |
| 7 | Flexibility and Efficiency | 2 | No / shortcut, no row keys, no bulk; ~50 Tab stops |
| 8 | Aesthetic and Minimalist Design | 3 | 28px tabs outrank the 20px title |
| 9 | Error Recovery | 2 | Load error is a polite status, Add stays active |
| 10 | Help and Documentation | 2 | Where Refresh is, how to find a form ID |
| Total | | 26/40 | Acceptable |

## Design Specificity Verdict
"You type" column and command-word tabs are product-specific; rest is operate-mode furniture. Detector: 11 scans clean; one text-overflow on #probe was the test harness (discarded). A's "counts ignore filter" was a stub artifact (real API filters counts since step 5); toast overflow re-check unnecessary (measured clean in step 2). Confirmed against code: zero counts on failed load, role=status error, focus lost after Undo button; off track #c3c8d6 ~1.7:1 (non-text contrast).

## Priority Issues
- [P1] Failed load looks like an empty list: error-note + role=alert, "-" counts, disable Add until a load succeeds. /impeccable harden
- [P2] Off switch track fails 3:1 control contrast: darker track or border. /impeccable polish
- [P2] Undo short-lived and loses focus: keep toast while Undo available, refocus switch, show "Undo (Ctrl+Z)". /impeccable harden
- [P2] Keyboard-first half delivered: / to search, Esc clears; optional row arrows/Space/Enter. /impeccable adapt
- [P3] Built-in drawer noisy: hide help when read-only, pin explanation, fixed-width row actions. /impeccable polish

## Persona Red Flags
- Alex: no / shortcut, pager is a mouse trip, Inactive filter not keyboard.
- Sam: ~50 Tab stops; load failure not an alert; Undo button loses focus; Ctrl+Z only in a tooltip.
- App manager: no Custom marker; form ID source unclear; Refresh location unclear; access note doesn't say who grants it.

## Minor Observations
- Target wraps 4 lines at 520px; hide on narrow.
- No page-number jump for many pages.
- Hex values outside tokens (#cdd3f1, #efc4be, #f7f8fc, #c3c8d6).
- English strings in an Arabic shell (known trade-off).

## Questions to Consider
- Should search accept the palette phrase?
- Is the risk model upside down (switch-off cheap, rename guarded)?
- Would a keyboard-driven row list fit better?
