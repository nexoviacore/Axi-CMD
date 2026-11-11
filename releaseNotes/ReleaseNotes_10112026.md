# Release Notes: Axi Command Line (AxiCMD)

- **Release date:** 11 November 2026
- **Components:** AxiCMD palette (`axicmdmain.js`), Axi Commandline Configuration page (`AxiCMDConfig.html`), AxiCMD database scripts (PostgreSQL and Oracle)

---

## Summary

This release adds the **Install** command and repository configuration, a standalone **Axi Commandline Configuration** page, **server-side paging** for large lists, **Arabic (RTL)** support, view filters applied to record lists, and trace controls through the **Configure** command.

---

## Enhancements

### 1. Install command and repository configuration (TSK-0921)

Products, plugins and change requests can be installed from the command line. The GitHub account and repository used for each type are configured first.

#### Configure the repository

- `configure Product <github-account> <github-repository>`: repository for products.
- `configure Plugin <github-account> <github-repository>`: repository for plugins.
- `configure ChangeRequest <github-account> <github-repository>`: repository for change requests.

Notes:

- Account and repository names may contain only letters, numbers, dot, underscore and hyphen.
- The command offers **Save** (`Ctrl + S`) once both names are valid.
- **Access:** Configuration Studio access (`AppMgrAccess`) **and** Developer Studio access (`Build`).

#### Install

- `install Product <directory> [<version>]`: install a product.
- `install Plugin <directory> [<version>]`: install a plugin.
- `install ChangeRequest <directory> [<version>]`: install a change request.

Notes:

- Directories and versions are listed from the configured repository (AxiGit API).
- A version is required when the directory has versions, and is not asked for when it has none.
- The request is queued through the ARM `PushToQueue` API.
- **Access:** Developer Studio access (`Build`).

Note to QA:

- The install command works only when the latest **AxiInstall Consumer** and the latest **AxInstaller.dll** are deployed. Test it on a setup that has both.

### 2. Axi Commandline Configuration page

`AxiCMDConfig.html` is a standalone page for configuring the options of the **Configure** and **SDK** commands:

- Turn built-in options on or off.
- Add, edit or deactivate custom options.

Custom options are kept when the scripts are re-run. New Configure / SDK options are added through this page, not through SQL.

- **Open from:** the AxiCMD settings (gear) menu → **Configure**.
- **Access:** Configuration Studio access **and** Developer Studio access.
- **Settings (gear) button:** the button in the AxiCMD toolbar (used for **Records per load** and **Configure**) is part of the product's main page (`mainHomeConfigTemplate.html`). Test the settings by configuring **Records per load** (pagination) and by opening the **Axi Commandline Configuration** page.


Note to QA:

- `mainHomeConfigTemplate.html` will also be released in the next product patch release.

### 3. Paging for large lists (TKT-1389, customer: BDF)

Forms, reports and records are loaded page by page from the server, and typed searches (3+ characters) are filtered on the server.

- More rows load as the user scrolls to the end of a list.
- **Records per load** can be set from the settings (gear) menu: 100 to 5000, default 100. The value is saved per user.
- **Display limit:** a list shows at most **1000** rows. Once 1000 rows are loaded, scrolling loads no more pages and the list shows "Showing first 1000 - type more to narrow the results". With **Records per load** above 1000, one request can fetch up to 5000 rows, but only the first 1000 are shown.
- **Access:** every user.

### 4. Arabic (RTL) support

The command line, suggestions, hints and the help tour support Arabic (right-to-left) layout. Command words and option values stay in English, because they are what the user types.

- **Header CSS changes:** the header and sidebar spacing for Arabic (RTL) mode is fixed in `LayoutTheme.css`. Before this fix, when Axi Command Line was enabled and the user switched to Arabic, the profile icons were not shown on the left side.

Note to QA:

- `LayoutTheme.css` will also be released in the next product patch release.

### 5. View filters applied to record lists (TKT-1387, customer: BDF)

`view` and `edit` now list only the records the form's view filter allows (`<transid>_filter`), matching the Entity / Listing page. Before, all records were listed.

### 6. User Activation controlled by license activation (TSK-0896, Issue #288)

**Configure → User Activation** is shown only to users whose `axusers.licactivation` is true (`T` / `true`, any case), admin included. A missing row, a null value or a failed lookup hides it.

- New ADS: `axi_userlicactivation` (PostgreSQL and Oracle).

### 7. Trace controls through the Configure command (Issue #30)

- `configure Trace On`: turn trace on.
- `configure Trace Off`: turn trace off.
- `configure Trace Files`: open the trace files dialog.

Notes:

- These commands offer **Go** only (no Pop-Up).
- **Access:** Developer Studio access.

---

## Bug Fixes

1. **TKT-0841 (`help`):** the **Source** option is now explained in the interactive help (customer: Agile Labs Pvt Ltd).
2. **TKT-1301 (Cost Centre Mapping):** **View** and **Edit** of "Rent Expenses" failed with `Error: 42703: column p.rent expenses does not exist`.
3. **#266 (Arabic):** the full command was not visible because the "Ready to Run" label overlapped it.
4. **#289 (`view`):** with two forms sharing a caption, `view` opened the wrong one. The form's transid is now kept, e.g. `"Test Form2 (tst2)" view`.
5. **Arabic:** the **Clear all** button overlapped the command line.

---



