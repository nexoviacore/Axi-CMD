1. TSK-0566 – AXI Command Line: Separate AXI-Sec Layer from Main Page and Support Dynamic Enablement

2. TKT-1214 - AXI Command Line- Input field loses focus and becomes unresponsive after pressing Backspace
   Issue: When clicking "New" inside a pop-up container, the loader was not showing, and subsequently pressing Backspace in AxiCMD caused the command line to hang.

   Root Cause:
   1. In `Entity-Common.js`, clicking "New" executes `parent.ShowDimmer(true)` followed by `parent.PopupManager.openForm("", link)`.
   2. When `PopupContainer.html` ignored `ShowDimmer(true)`, the product's official dimmer (`#waitDiv`) was not displayed.
   3. Previously, when it escalated to `top.ShowDimmer(true)`, `mainnew.aspx` set `document.onkeydown = EatKeyPress (return false)`, and because the tab loaded inside the popup, `top.ShowDimmer(false)` was never called to dismiss the dimmer, permanently swallowing all keystrokes in AxiCMD.

   Fix Applied:
   1. In `PopupContainer.html`:
      - Used the product's official `ShowDimmer`: `window.ShowDimmer(true)` cleanly invokes `window.parent.ShowDimmer(true)` to display `#waitDiv`.
      - Attached an auto-dismiss safety timer (7 seconds) so the dimmer never hangs if child frames error or disconnect.
      - On tab load (`ifr.onload`), form ready (`waitForTstructReady`), tab close (`closeTab`), and popup close/destroy (`btn-close`, `forceDestroy`), `window.ShowDimmer(false)` is called.
      - `window.ShowDimmer(false)` invokes `window.parent.ShowDimmer(false)`, cleans up any trapped `EatKeyPress` handlers, and removes `page-loading`.
      - No custom loader DOM elements or "loading..." placeholders were added to `PopupContainer.html`.
   2. In `axicmdmain.js`:
      - Input `focus` and `click` run `releaseStuckDimmer()`, ensuring AxiCMD is always immediately responsive.
      - Guarded 2-token target-first commands (e.g. `tmmsg View`) in Backspace handling so they delete character-by-character rather than wiping tokens.


Ui Verification for when the axi cmd is enabled: 

1. Sidebar width should decreased when the axi cmd.
2. 
   
