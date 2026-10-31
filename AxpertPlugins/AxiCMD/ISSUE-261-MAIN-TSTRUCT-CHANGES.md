# Issue #261: changes to `Js/main-tstruct.js`

State as of 30 Oct 2026, after the old 27 Oct guard was removed from both files. Background and the debugging log are in `ISSUE-261-DEBUGGING.md`.

## Problem

If a tstruct is opened from the command line and then from its normal place (popup, Configuration Studio, Developer Studio), the second one is blank.

- Opening a form in the main frame (`middle1`) caches it: `GenTstHtmlLocalStorage` → `StoreTstHmtlLoad` writes it to localStorage (`tstHtml♠<transid>…`) and Redis (`…-<transid>-tstHtmlLs-…`). AxiCMD opens forms in `middle1`, so it caches forms that are normally only opened in modals.
- A later open with a `dummyload` param gets the cached page from the server. That page's `document.ready` exits early (`isServerDummyPost`), and the startup script (`tstDummyLoad` or `tstHtmlDummyLoad`) only finishes the render through `LoadIframe`, which loads into `middle1`. In any other frame nothing renders, so the page is blank.
- The server takes the cached path only when the URL has `dummyload` (`aspx/tstruct.aspx.cs:235` for `true…`, `:353` for `false…`). Without it, the form is rendered fully.

## Fix

In a frame where the cached page can't render, reload the same URL without `dummyload`, so the server renders the form fully. `middle1` and `axMultiiframe_*` keep the fast cached path. The extra cost is one round trip, and only in other frames.

All changes are additions to the original product code, each marked `// new code`. Nothing in the original was changed or removed.

## Changes in `Js/main-tstruct.js`

### 1. Guard in `tstDummyLoad` (function at line 13360, guard at ~13380)

Placed after the permission check and before `callParentNew("isdummyLoad=", "true")`. Covers the `dummyload=true♠…` path, which the Developer Studio list (`Js/iview.js:3602-3625`) uses when a localStorage copy exists, e.g. Dev Options after an AxiCMD open.

```js
    } catch (ex) { }

    // new code
    // The dummy-load flow below only renders through LoadIframe, which targets middle1. In any other frame
    // (e.g. the Developer Studio modal) it renders nothing, leaving a blank page. "dummyload=false♠" still returns
    // the cached page, so reload without dummyload to get a full render (#261).
    if (!isTstHtmlCacheFrame()) {
        reloadWithoutDummyLoad();
        return;
    }
    callParentNew("isdummyLoad=", "true");
```

### 2. New function `isTstHtmlCacheFrame()` (line 13450)

```js
// new code
// The cached ("dummy load") page only finishes rendering through LoadIframe, which targets middle1.
// True for the frames that path supports; any other frame (popup, Developer Studio modal) stays blank (#261).
function isTstHtmlCacheFrame() {
    try {
        let _frameId = window.frameElement ? (window.frameElement.id || "") : "";
        return _frameId == "middle1" || _frameId.startsWith("axMultiiframe_");
    } catch (ex) {
        return false;
    }
}
```

Returns `true` only in the frames where the cached page can render. A top-level window (no `frameElement`) returns `false`, so it also gets a full render instead of a script error.

### 3. New function `reloadWithoutDummyLoad()` (line 13461)

```js
// new code
// Reloads this page without the dummyload param, so the server renders the form fully instead of the cached page (#261)
function reloadWithoutDummyLoad() {
    let _thisUrl = window.location.href;
    let _newUrl = _thisUrl.replace(/([?&])dummyload=[^&#]*&?/i, "$1").replace(/[?&](#|$)/, "$1");
    if (_newUrl == _thisUrl)
        return;
    window.location.replace(_newUrl);
}
```

It removes only the `dummyload` param and keeps every other param. `location.replace` doesn't add a history entry. The reloaded URL has no `dummyload`, so the server renders fully and the guard isn't reached again, so there's no loop.

| Before | After |
| --- | --- |
| `…?act=open&transid=axstc&isDupTab=false&dummyload=true%E2%99%A0axstc_5293511` | `…?act=open&transid=axstc&isDupTab=false` |
| `…?transid=axstc&dummyload=false%E2%99%A0&hltype=open` | `…?transid=axstc&hltype=open` |
| `…?dummyload=false&transid=axstc` | `…?transid=axstc` |
| `…?transid=x&dummyload=false#frag` | `…?transid=x#frag` |

### 4. Guard at the start of `tstHtmlDummyLoad` (line 13469)

Covers the `dummyload=false♠…` path, used when no localStorage copy exists but Redis has one.

```js
function tstHtmlDummyLoad(_transid) {
    // new code
    // The cached page cannot render outside middle1 / axMultiiframe_, so ask for a full render instead (#261)
    if (!isTstHtmlCacheFrame()) {
        reloadWithoutDummyLoad();
        return;
    }
    try {
```

### 5. End of file

The file now ends with a newline (before: `No newline at end of file`). No behaviour change.

## Why it's safe

- Outside `middle1` / `axMultiiframe_*`, both functions rendered nothing before: they only call `LoadIframe` when the frame is `middle1`. So no case that worked before is changed.
- In `middle1` / `axMultiiframe_*`, `isTstHtmlCacheFrame()` returns `true` and the original code runs unchanged.
- The functions are global and defined once (`main-tstruct.js` only), so no other script overrides them.

## History

| Date | Change |
| --- | --- |
| 27 Oct (commit `7b57535`) | A guard in `tstDummyLoad` redirected non-`middle1` frames to `dummyload=false♠`. That still returned the cached page, so it didn't fix the blank screen. |
| 30 Oct | That guard was replaced by the code above, and the `tstHtmlDummyLoad` guard and the two helpers were added. The 27 Oct guard has been removed from both `main-tstruct.js` and `main-tstruct.min.js`; it is kept only in the backup `Js/__bkp/main-tstruct_bkp_30102026.js` / `.min.js`. |

Tested 30 Oct (IIS log 07:24–07:26): each `dummyload=true♠…` / `dummyload=false♠…` request from the Developer Studio modal was followed by the same URL without `dummyload`, and Dev Options rendered.

## `Js/main-tstruct.min.js`

Same changes as `main-tstruct.js`: both helpers (`/* new code (#261) */`), the guard in `tstDummyLoad` (marked `// new code (#261)`) and the guard in `tstHtmlDummyLoad`. The 27 Oct guard has been removed from this file too, so the two files match.

Which file the browser runs depends on bundling (`~/Js/formInit`, `App_Code/BundleConfig.cs:210-224`, `EnableFileExtensionReplacements`):

- **Bundling off (debug, current dev setup):** `main-tstruct.js`.
- **Bundling on:** `main-tstruct.min.js`.

Keep both files in step when changing this code.

## Backups and rollback

| File | Content |
| --- | --- |
| `Js/__bkp/main-tstruct_bkp_27102026.js` / `.min.js` | Before the 27 Oct change (original product code) |
| `Js/__bkp/main-tstruct_bkp_30102026.js` / `.min.js` | After 27 Oct, before the 30 Oct change |

To roll back, copy the backup over `Js/main-tstruct.js` / `Js/main-tstruct.min.js`, then press Ctrl+F5. Scripts aren't versioned in debug mode, so the browser can keep an old copy.

## How to check

1. Press Ctrl+F5 (or run `iisreset` from an elevated prompt and sign in again).
2. Run `sdk dev option` from the command line, then open **Dev Options** from the header. The form should render.
3. In the Network tab, or the IIS log `C:\inetpub\logs\LogFiles\W3SVC1\u_ex<yymmdd>.log`, the `tstruct.aspx?…dummyload=…` request is followed by the same URL without `dummyload`.
4. Repeat for Axpert Data Sources (`b_sql`), a Configure item and Configuration Studio, and a tstruct opened as a popup.
5. Regression: open a form from the menu twice in the main frame. The second open still uses the cached page (no extra reload), and new, load and save work.
