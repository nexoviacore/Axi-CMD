# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [11.4.0-beta] - 2026-06-25

### Added
- **SQL Metadata**: Added direct SQL tables and updated metadata mapping scripts for PostgreSQL (`axi_axdirectsql_tables.sql`).
- **Autocomplete Suggestions**: Restored visibility of `iview` items (which natively return `viewallowed = "NA"`) under the `view` autocomplete list.

### Changed
- **Script Caching**: Bumped frontend loader caching versions (`AxiCMDMainPage.html` to `?v=90`) to enforce loading of the latest command palette logic.
- **PostgreSQL Script Optimization**: Optimized functions inside PostgreSQL `axi_functions.sql` and removed deprecated `fn_get_axpertcomps_name` function.
- **Cleanup**: Deprecated and deleted unused/legacy wrapper scripts (`Entity-Common.js` and `Entity-Common.min.js`) in `PopUpcontainer/js/`.
- **Command Palette Deprecations**: Deprecated the legacy `analyse` command.
- **Toast Notifications UI**: Redesigned toast messages with modern glassmorphism (frosted blur effect, semi-transparent colored backgrounds, success/error/info styling), dynamic layout stacking, close actions, and positioned them higher (`bottom: 80px`) to prevent covering the bottom toolbar.
- **Mobile Compatibility**: Refined toast notification styling with media queries (`max-width: 576px`) to scale and center correctly on mobile viewports.
- **Repository Maintenance**:
  - Configured Git to ignore local `.zip` release and backup files by updating `.gitignore`.
  - Cleaned up formatting and indentation inside `smartview.js`.

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

