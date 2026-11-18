# AxiCMD v1.1.2 – Release Notes (18 Nov 2026)

## Summary

This release fixes the pop-up issues reported by QA on the **Configure** command (Issues 2 to 5), makes **New** on a pop-up listing open in the same pop-up tab, and fixes the Sales Order **Edit** error when **NA** is selected (database change).

---

## Bug Fixes

### 1. Configure screens: New keeps loading after opening the screen in pop-up mode (Issue 2)

- **Issue:** Open a Configure screen (e.g. User) in normal mode and then in pop-up mode, then open its listing (User Listing) in normal mode and then in pop-up mode. Clicking **New** on the normal-mode listing kept loading.
- **Also seen on:** Actor / Actor Listing, Smart View Attributes / Smart View Listing.
- **Status:** Fixed. Opening a screen in pop-up mode no longer clears the cached state of the normal-mode screen, so New opens the form.

### 2. Publish Config Studio: List button does not work in pop-up mode (Issue 3)

- **Issue:** Open `Configure "Publish Config Studio" agilespaceqanew` in pop-up mode and click the **List** button. It did not navigate to the list view.
- **Status:** Fixed. The list now opens in the same pop-up tab. The same applies to the List button of the other Developer Studio forms (Variables, Custom Types, Jobs, Languages, Data Sources, Pages, …).

### 3. Configure screens: existing record keeps loading after opening the screen in pop-up mode (Issue 4)

- **Issue:** Open Configure Dimension in normal mode and then in pop-up mode, then open Dimension Listing in normal mode and then in pop-up mode. Clicking an existing record on the normal-mode listing kept loading.
- **Also seen on:** Smart View Attributes / Smart View Listing.
- **Status:** Fixed (same cause and fix as Issue 2).

### 4. Configure screens: user is logged out when opening a record in pop-up mode (Issue 5)

- **Issue:** Open Configure Role in normal mode and then in pop-up mode, then open Role Listing in normal mode and then in pop-up mode. Open an existing record in normal mode and then an existing record in the pop-up listing. The user was logged out.
- **Also seen on:** User / User Listing, Actor / Actor Listing.
- **Status:** Fixed. Each pop-up now keeps its own session data, so opening a record in normal mode no longer breaks the pop-up listing.

### 5. Role listing: New in pop-up mode opens a second pop-up tab

- **Issue:** Open `Configure "Role Listing"` in normal mode and click **New**, then open it in pop-up mode and click **New**. The form opened in a second pop-up tab instead of the current one, and after closing that tab the listing stayed under a loader.
- **Status:** Fixed. The form now opens in the same pop-up tab, like the List button. The same applies to New on other listings opened in a pop-up (e.g. Publish listing).

### 6. Sales Order: Edit gives an error when NA is selected

- **Issue:** Editing a Sales Order gave an error only when **NA** was selected: `Unable to load. SQL: Select BRANCHNAME from BRANCH Where BranchId =`
- **Status:** Fixed by the database change below (`fn_axi_getstructs_obj`).

---

## Database Changes

- **`fn_axi_getstructs_obj`** (PostgreSQL and Oracle, `axi_functions.sql`): the record lists of `view` / `edit` now only include records whose `cancel` column is not null (Bug Fix 6).
- **After running the script:** recycle the app pool or clear the ADS cache, otherwise the old query keeps running.

---


