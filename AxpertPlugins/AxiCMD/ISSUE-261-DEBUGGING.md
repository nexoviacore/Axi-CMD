# Issue #261: blank tstruct after opening it from AxiCMD and then from its normal place

Debugging log, 27 Oct 2026. Environment: `http://localhost/Axpert11.4_DEV/aspx/mainnew.aspx`, Postgres, local Redis (db0).

## 1. Symptom

1. Open a developer form from the command line, for example `sdk dev option` (form `axstc`) or `sdk axpert data sources` (form `b_sql`). It opens in the main frame (`middle1`) and works.
2. Open the same form from its normal place, for example the header **Dev Options** or **Axpert Data Sources** tile. A modal opens with the toolbar but **no form body** (blank), and the console shows no errors.
3. Opening the form from the header first works. Only "command line first, then header" fails.

## 2. Short answer

The AxiCMD open leaves a row in the server-side Redis cache `tstHtmlLs` for the form with an **empty HTML value**. When the same form is opened later in another frame (the Developer Studio modal), the server returns the page without the form body. Deleting that one Redis key makes the header open render.

Not yet known: why the stored HTML is empty after the AxiCMD open, and which server check treats the empty row as "already cached". Both are in product code.

## 3. How the form cache works (relevant code)

| Layer | Where | Role |
| --- | --- | --- |
| Browser `localStorage` | `tstHtml♠<transid>-<app url>♥<time>♥<date>` | Pristine form HTML. Written by `StoreTstHmtlLoad` (`Js/main-tstruct.js:1281`), only when the frame is `middle1` or `axMultiiframe_*`. |
| Browser `localStorage` | `originaltrIds-<app url>` | List of transids that have a cached copy. Read by `LoadIframe` (`Js/main.js:1845`). |
| Server Redis | `<proj>~<db>-<transid>-tstHtmlLs-default-eng` (hash) | Form HTML. The row is created as a placeholder by `SetTstHtmlLs` (`App_Code/Util.cs:7914`) and filled by `StoreLsTstHtml` (`aspx/tstruct.aspx.cs:9543`). |
| Server Redis | `<proj>~<db>-<transid>-FormLoadRes-eng` (hash) | Result of the form-load call, written by `SetFormLoadData` (`Util.cs` ~6112). |
| Shell global | `lastLoadtstId` (`Js/main.js:1780`) | URL of the last tstruct loaded into `middle1`. |

Two URL shapes decide which path the server takes (`aspx/tstruct.aspx.cs`):

- `dummyload=true♠<key>`: dummy load of a cached form (line 241 onward), renders through `tstDummyLoad` (`main-tstruct.js:13360`).
- `dummyload=false♠`: server looks for cached HTML in Redis (line 353 onward). If none, it renders fully.

The header's Dev Options list builds its form URL in `Js/iview.js:3602-3625`. If a `tstHtml♠<transid>-…` entry exists in `localStorage` it sends `dummyload=true♠<key>`, otherwise `dummyload=false♠`.

## 4. How AxiCMD opens a form

`redirectToTstruct` in `HTMLPages/js/axicmdmain.js` builds, for a new form:

```
../aspx/tstruct.aspx?transid=axstc&hltype=open&createaxiflag=true&isDupTab=false&dummyload=false
```

The menu and header send only `transid` and `hltype=open` (see `Js/dashBoard.js:119`). Some `axi_command_config` rows also carry `act=load&dummyload=false?` as extra params. The trailing `?` is deliberate-looking but malformed (the server only recognises `false` or `false♠…`), and the URL ended up with `dummyload` twice.

## 5. Steps taken, in order

### 5.1 Compare the two entry points
- Header tile: `openDeveloperStudio("icsqlist")` in `aspx/mainHomeConfigTemplate.html:738` opens `DeveloperStudio.aspx` in a **modal** (`Js/main.js:5793`). Its form loads in the inner iframe `#axpiframeac`, not `middle1`.
- AxiCMD: `top.window.LoadIframe(url)` loads into `middle1`.
- Conclusion: both write and read the same per-form caches, but from different frames.

### 5.2 Check the browser cache
Console:
```js
Object.keys(localStorage).filter(k=>k.startsWith("tstHtml♠axstc"))
```
- Before `sdk dev option`: `0` keys.
- After `sdk dev option`: `1` key. The AxiCMD open (in `middle1`) writes it.
- After the header open: still `1`.

### 5.3 Check what the header sent
Network tab, failing request:
```
tstruct.aspx?act=open&transid=axstc&openerIV=dop_list&isIV=true&isDupTab=false&dummyload=true%E2%99%A0axstc_3713475
```
Because a `localStorage` copy existed, `iview.js` chose the cached path `true♠axstc_3713475`.

Also verified that the piece used (`split('*$*')[11]`) is valid, which ruled out an `undefined` key:
```js
const k = Object.keys(localStorage).filter(k=>k.startsWith("tstHtml♠axstc"));
console.log(k.length, JSON.stringify(localStorage[k[0]]?.split('♠♠♠')[1]?.split('*$*')[11]));
// 1 "axstc_3713475"
```

### 5.4 Dead end found in `tstDummyLoad` (real defect, not the cause of this blank)
In `tstDummyLoad` (`main-tstruct.js:13409-13433`) the code compares `lastLoadtstId` (the URL AxiCMD loaded into `middle1`) with the modal's URL. Both are `axstc`, so it takes the branch that only acts when the frame is `middle1`. In the modal frame nothing happens and the page stays blank.

Fix applied (product file, with backup): at the start of `tstDummyLoad`, in any frame other than `middle1` or `axMultiiframe_*`, redirect to the same URL with `dummyload=false%E2%99%A0` so the server renders fully. The change is in `Js/main-tstruct.js` and the generated `Js/main-tstruct.min.js`. I confirmed the live bundle (`/Js/formInit`) served the fix.

After this fix the modal request became `dummyload=false%E2%99%A0` and the form was **still blank**, so there was a second cause.

### 5.5 Frame-by-frame inspection (the form exists, its body is empty)
Console in the top window, walking all frames:

```
DeveloperStudio.aspx?ivtstName=idop_list         text=0   inputs=2
  tstruct.aspx?act=open&transid=axstc&...        text=4   inputs=78  dvlayout display=flex visible h=573
  tstruct.aspx?transid=axstc&hltype=open&createaxiflag=true&...  text=258  inputs=115
```

- The modal's tstruct frame had a layout but only 4 characters of visible text: the toolbar rendered and the form body did not.
- The AxiCMD frame in `middle1` rendered normally.

### 5.6 Inspect Redis
```
redis-cli keys '*'
redis-cli type  "devpgbase114~axpertdb-axstc-tstHtmlLs-default-eng"
redis-cli hkeys "devpgbase114~axpertdb-axstc-tstHtmlLs-default-eng"
redis-cli hstrlen "devpgbase114~axpertdb-axstc-tstHtmlLs-default-eng" "♦27/10/2026"
```
Found for `axstc`:

| Key | Field | Value length |
| --- | --- | --- |
| `devpgbase114~axpertdb-axstc-tstHtmlLs-default-eng` | `♦27/10/2026` | 0 |
| `devpgbase114~axpertdb-axstc-FormLoadRes-eng` | `♦27/10/2026` | 0 |

Both rows exist and are empty. Other `axstc` keys (`TstflDopTime--eng`, `tstHtmlLsGlbVars-default-eng`) were only read.

### 5.7 Isolate the cause by deleting keys

| Test | Result |
| --- | --- |
| Delete both keys above, then open the header | Renders |
| `sdk dev option`, then header | Blank again; Redis shows both rows back, empty |
| `sdk dev option`, delete **only** `tstHtmlLs`, then header | **Renders** |

Conclusion: the empty `tstHtmlLs` row created by the AxiCMD (`middle1`) open is what blanks the later modal open. The `FormLoadRes` row is also recreated empty by the header render, but it does not cause the blank.

## 6. Reproduce for review

1. Sign in with the Redis keys for the form absent: `redis-cli del "devpgbase114~axpertdb-axstc-tstHtmlLs-default-eng"` (and the `FormLoadRes` key).
2. Run `sdk dev option`. The form opens in the main frame.
3. `redis-cli hstrlen "devpgbase114~axpertdb-axstc-tstHtmlLs-default-eng" "♦27/10/2026"` prints `0` (the date is the day you run it).
4. Open **Dev Options** from the header. The modal has a toolbar but no form body.
5. `redis-cli del "devpgbase114~axpertdb-axstc-tstHtmlLs-default-eng"`.
6. Open **Dev Options** from the header again. The form shows.

Same pattern with `sdk axpert data sources` and the Axpert Data Sources tile (form `b_sql`).

## 7. What is still unknown

1. Why `StoreLsTstHtml` leaves the row empty after the AxiCMD open. The client builds the HTML in `StoreTstHmtlLoad` and calls it at `main-tstruct.js:1308`, inside a `try/catch (ex) { }` that swallows errors. Next experiment: log the HTML length at that line and watch the network call.
2. Which server check treats an empty `tstHtmlLs` row as "cached" in the modal's render. The branches at `tstruct.aspx.cs:353-555` guard with `_thsHtml != ""`, so the dependency is somewhere else (for example the key/session-subkey logic in `Util.cs:6040-6112` and `7881-7912`).
3. Whether the same happens when a form is opened from the **menu** into `middle1` and then from a modal. The `.min` bundle shows the same store code, so it probably does.

## 8. Changes made during this investigation

| File | Change | Status |
| --- | --- | --- |
| `Js/main-tstruct.js` | Non-`middle1` frames in `tstDummyLoad` redirect to a full render | Product code; backup in `Js/__bkp/` |
| `Js/main-tstruct.min.js` | Same change (generated file, untracked) | Product code; backup in `Js/__bkp/` |
| `HTMLPages/js/axicmdmain.js` | `redirectToTstruct` removes a duplicate or malformed `dummyload` from extra params, and `act=load` when no record is opened. Debug `console.log` after the URL is built. | Plugin; `?v=61` in `aspx/mainHomeConfigTemplate.html` |

Backups:

- `Js/__bkp/main-tstruct_bkp_27102026.js`
- `Js/__bkp/main-tstruct_bkp_27102026.min.js`

To roll back, copy them over `Js/main-tstruct.js` and `Js/main-tstruct.min.js`.

## 9. Side notes

- The console message "Blocked aria-hidden on an element because its descendant retained focus" on `#devStudioPopup` is a product accessibility warning from the `BSModal` close button. It is unrelated.
- `Permissions policy violation: unload is not allowed in this document` lines are noise in this environment.
- Product defects to hand over: the empty `tstHtmlLs` row after a `middle1` open; the `tstDummyLoad` `lastLoadtstId` branch that does nothing in a non-`middle1` frame; `StoreTstHmtlLoad` swallowing failures.
