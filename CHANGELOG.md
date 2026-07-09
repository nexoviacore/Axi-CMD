# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [11.4.0-beta.7] - 2026-07-09

### Added
- **Structure-Level Permissions for First-Token Suggestions**: Applied the `view` command structure-level permissions (`isViewAllowed` check) to filter structure listings when the command palette is first opened or when typing the first token. Structures that the user does not have permission to view are now excluded from initial suggestions.
- **`isField` Validation for Edit Command**: Implemented the same `isField` check used by the View command on the Edit command. Users are prompted if they select a field without supplying a corresponding field value.

### Changed
- **Favorites Search Input Redesign**: Modernized layout of the Favorites search input inside `AxiCMDMainPage.html` with a search icon, smooth border hover/focus transitions, and a subtle focus glow.
- **Redundant Startup Loading Removal**: Removed the initial `loadFavorites()` call from `initCommands()` to optimize startup loading speed and prevent unsolicited background requests.

### Fixed
- **Dropdown Close on Search Click**: Excluded the `megaDropdown` element inside the document-wide click-outside handler to prevent the Favorites dropdown from unexpectedly closing when users click or type inside the Favorites search filter input.
- **Run/Send and Refresh Button Tooltips**: Integrated native HTML tooltips for Run/Send and Refresh toolbar buttons by removing the hardcoded `initialized` class.
- **resolvedParams Overwrite Prevention**: Switched token check in `handleInput()` to retrieve raw/unswapped tokens using `getTokens(text, false)` to prevent accidental resets of resolved parameters during target-first command typing.

## [11.4.0-beta.6] - 2026-07-02

### Added
- **Structure Capability Action Constraints**: Restricted available actions for target-first commands based on target structure type (`stype`). Targets with `stype === "t"` (TStruct) allow `create`, `edit`, and `view`, while targets with other types (e.g. `iview`, `ads`, `page`) are constrained strictly to `view`.
- **Context-Aware Duplicate Target Resolution**: Configured target entity lookup (`getTargetEntityObj`) and token resolver (`tryResolveToken`) to handle duplicate names/captions (e.g., TStruct named `"test 1"` vs Page named `"test 1"`). Prioritizes TStruct resolution when action is `create` or `edit`, and Page/IView/ADS resolution when action is `view`. In autocomplete (`suggestLocal`), checks if the target has been selected and resolved in `resolvedParams` to suggest the correct actions, falling back to the union of all matching target capabilities only if not yet resolved.
- **First-Token Space Auto-Quoting**: Configured the space key handler in the `keydown` event listener to automatically double-quote unquoted typed text on spacebar press, now checking the first token as well as long as it does not match a defined command verb (such as `create`, `edit`, or `view`).

### Fixed
- **Autocomplete Hidden on First-Token Auto-Quoting**: Allowed suggestion matching for unclosed quoted strings inside `suggestLocal()` even when a trailing space is present inside the quotes (e.g. `'"sales '`), resolving an issue where the autocomplete panel incorrectly hid upon spacebar auto-quoting.
- **Backspace Token Interchanging**: Prevented the `keydown` event listener from mutating/swapping the raw `tokens` array in-place. Cloned the tokens array into `normalizedTokens` for group key verification, ensuring raw typed token order (e.g. `"Sales Order FORM" view`) is preserved when reconstructing the input value after a backspace deletion.
- **Raw Input Token Retrieval in Keydown**: Configured `getTokens` inside the `keydown` event listener to disable automatic token normalization swapping (`shouldNormalize=false`) when fetching raw input elements, resolving an issue where backspacing `"Sales Order FORM" view` deleted the target caption instead of the action verb.

## [11.4.0-beta.5] - 2026-07-01

### Added
- **Target-First Command Restructure**: Reversed the command syntax for primary entity actions (`create`, `view`, `edit`) from `<action> <target>` to `<target> <action>`.
- **Target Autocomplete Suggestions**: Restructured the autocomplete suggestions to show target entities (`tstructs` and `iviews` loaded from `axi_structmetalist` dynamically via `loadList` on startup) and unreversed commands first. Restricts secondary suggestions strictly to `create`, `edit`, and `view` based on target capability.
- **Go/Popup Suggestions for Completed Actions**: Updated `suggestLocal()` to display `Go` and `Popup` options immediately when a target-first action (like `create` or `view`) is fully typed.
- **Token Ingestion Swapping Layer**: Integrated virtual token normalization swapping inside `suggestLocal()`, `apply()`, `handleInput()`, `executeCommandsV2()`, and the keydown backspace handlers in `axicmdmain.js` to route target-first inputs seamlessly through the existing execution, routing, and verification engines.
- **Master Target Lookup Fallback**: Integrated a fallback search inside the master metadata list `axi_structmetalist` inside `tryResolveToken` and `getResolvedParamType` to resolve target names/captions (including quoted strings like `"Sales Order FORM"`) to their exact transaction IDs.
- **Case-Insensitive Configuration and Handler Resolution**: Added case-insensitive resolvers (`getCommandConfig` and `getGroupHandlers`) for commands dictionary and `COMMAND_HANDLERS` objects to prevent case-mismatch errors when routing lowercased swapped action keys against capitalized configuration headers.

## [11.4.0-beta.4] - 2026-07-01

### Fixed
- **Help Command Input Blocking**: Disabled typing, backspacing, or key navigation in the search command line while the Help walkthrough/tour is active.
- **Help Tour Resize Positioning**: Registered a debounced window resize event listener during the tour to call `tour.refresh()` and ensure help dialog positions dynamically update when screen resolution changes.
- **Help Tour Toolbar Button Disabling**: Disabled all toolbar buttons inside the `.AXI-Sec` container (like send, history, favourites, and refresh) using pointer-events and opacity styling when the Help walkthrough tour is active.

## [11.4.0-beta.3] - 2026-06-30

### Fixed
- **Preview Modal Run Prevention**: Disabled displaying and executing run commands when the `loadPopUpPage` preview modal is visible, or when running inside an iframe with ID/name `loadPopUpPage` or name/ID/class `middle` / `middle1`.
- **PostgreSQL DDL script updates**: Added missing `pagination` and `applydimensions` columns to `axdirectsql` table structure inside PostgreSQL scripts.

## [11.4.0-beta.2] - 2026-06-29

### Added
- **Refresh Icon Animation**: Added a rotation animation to the refresh button icon when clicked. The animation starts automatically when the request starts and stops cleanly when the request completes or fails.

### Changed
- **Favorites Modal Transitions**: Implemented smooth entry and exit transitions for the Favorites modals (Add, Rename, and Delete modals) using CSS transitions, transforming scale, opacity, and backdrop blur.
- **Resource Versioning**: Bumped the resource query parameters for `axicmdmain.js` and `axicmdmain.css` in both `CustomPages/AxiCMDMainPage.html` and `AxpertPlugins/Axi_Beta/HTMLPages/AxiCMDMainPage.html` to prevent browser caching.

### Fixed
- **Roles Popup Configuration**: Configured the roles popup container and fixed the new icon click loading behavior inside `Js/iview.js`.
- **Oracle DDL Datatypes**: Corrected non-Oracle-compatible datatypes (`varchar` to `VARCHAR2`, and `text` to `CLOB`) inside `axi_axdirectsql_tables.sql` and `axi_dependent_tables.sql`.
- **Undefined Appname/ArmUrl Fallbacks**: Resolved timing issues when `axicmdmain.js` loads before `main.min.js` by adding a deferred retry initialization loop (`startInit()`) and dynamic frame/parent/top and `callParentNew` fallbacks for `mainProject`, `mainUserName`, and `armUrl`.
- **Run Command Favorites Validation**: Added validation in `toggleFavorite()` and `confirmAddFavorite()` to prevent users from adding or renaming favorites to `run` commands.

## [11.4.0-beta.1] - 2026-06-26

### Added
- **Configuration Fallback**: Created `axicmd-config.json` with an `axiarmurl` placeholder to allow configuring the ARM URL locally.

### Changed
- **ARM URL Resolution Fallback**: Implemented fallback logic in `axicmdmain.js` to fetch `AxiArmUrl` from the local `axicmd-config.json` file if the global `armUrl` variable is not found in the environment.
- **Asynchronous Script Loading**: Reverted the script loading mechanism of `axicmdmain.js` in `AxiCMDMainPage.html` (for both `CustomPages` and `AxpertPlugins`) back to `files.js.push` to ensure reliable path resolution in IIS virtual directories and prevent early execution timing conflicts.
- **Fetch Optimization**: Added execution cache flags (`isConfigLoaded`) to prevent concurrent and redundant config fetches to `axicmd-config.json` during timer-based DOM element retry loops.

## [11.4.0-beta] - 2026-06-25

### Added
- **SQL Metadata**: Added direct SQL tables and updated metadata mapping scripts for PostgreSQL (`axi_axdirectsql_tables.sql`).
- **Autocomplete Suggestions**: Restored visibility of `iview` items (which natively return `viewallowed = "NA"`) under the `view` autocomplete list.

### Changed
- **Script Caching**: Bumped frontend loader caching versions (`AxiCMDMainPage.html` to `?v=92`) to enforce loading of the latest command palette logic.
- **PostgreSQL Script Optimization**: Optimized functions inside PostgreSQL `axi_functions.sql` and removed deprecated `fn_get_axpertcomps_name` function.
- **Cleanup**: Deprecated and deleted unused/legacy wrapper scripts (`Entity-Common.js` and `Entity-Common.min.js`) in `PopUpcontainer/js/` and obsolete resource `axi_axdirectsql_tables.zip` under Oracle script folders.
- **Command Palette Deprecations**: Deprecated the legacy `analyse` command.
- **Toast Notifications UI**: Redesigned toast messages with modern glassmorphism (frosted blur effect, semi-transparent colored backgrounds, success/error/info styling), dynamic layout stacking, close actions, and positioned them higher (`bottom: 80px`) to prevent covering the bottom toolbar.
- **Mobile Compatibility**: Refined toast notification styling with media queries (`max-width: 576px`) to scale and center correctly on mobile viewports.
- **Repository Maintenance**:
  - Configured Git to ignore local `.zip` release and backup files by updating `.gitignore`.
  - Cleaned up formatting and indentation inside `smartview.js`.
- **Entity Dimmer Integration**: Wrapped `ShowDimmer` calls inside `Entity-Common.js` with dynamic type verification checks (`typeof ShowDimmer === "function"`) across parent, top, and local window frames to prevent exceptions on deeply nested iframe pages.
- **Command Suggestions Restriction**: Excluded the `Pop-Up` action option from the suggestions dropdown when typing the `configure responsibilities` command.

### Fixed
- **Iview Auto-complete Suggestions**: Fixed a bug where `iview` items having `"viewallowed": "NA"` were blocked from suggestion dropdowns under the `/view` command group. Bypassed the `"NA"` check to allow them.
- **Run Command Exclusions**: Disabled execution and suggestion listing for the `run` command in:
  - Active preview modal windows.
  - Developer/Design studio workspaces (`qadev`, `adInfo=`, `axidev` url params).
  - Admin/System utility pages (`ArrangeMenu.aspx`, `AxDBScript.aspx`, `tstruct.aspx?transid=ad_lg`, `iview.aspx?ivname=inmemdb`).
  - Dashboard and calendar pages running via `processflow.aspx` (where `dashboard=t` or `calendar=t` are in query string).
- **Smartview Popup Links**: Resolved routing bugs in `smartview.js` to correctly route popup tstructs via `../../../aspx/tstruct.aspx`.
- **Modals Interaction**: Fixed cancellation button triggers inside the favorites configuration modals to properly close when opened.
- **Dropdown & Filters**: Resolved page-view suggestions and filter plugin custom code dropdown trigger handlers.
- **SQL Syntactical Issue**: Corrected quoted column `normalized` mapping inside the Oracle table script `axi_axdirectsql_tables.sql` to avoid structural compilation errors.
- **Oracle DDL Script Validation**: Added metadata reference sanity checks (`AND b.axdirectsqlid IS NOT NULL`) and corrected format queries in the Oracle SQL scripts.
- **PostgreSQL DDL Structures**: Added structural column updates (`pagination` and `applydimensions`) to direct SQL table structures on PostgreSQL.



