# Spec: Axi Command Configuration Management Interface (`AxiCMDConfig`)

## Problem Statement

Currently, extending and maintaining the Axi Command Palette requires manually managing two separate database tables (`axi_command_config` for dynamic navigation routes, and `axi_command_prompts` for auto-complete prompt tokens). Administrators and developers face several major challenges:

1. **Manual & Error-Prone Setup:** Registering a new command route requires writing raw SQL `INSERT` statements with exact positional matching between position-2 `promptvalues` and position-3 `promptsource` (`Axi_Dummy` placeholders). A single typo or misaligned token breaks autocomplete or causes execution failures.
2. **Lack of Visual Management:** Administrators cannot easily inspect existing routes, filter by command groups, verify URL parameter placeholders (`:username`, `:userroles`, `:appname`), or test navigation without trial-and-error typing in the command palette.
3. **No Hot-Reloading:** Database updates require manual browser cache clearing (`localStorage.removeItem("axi_command_config_v2")` and `Ctrl + F5`) before newly inserted commands become visible.
4. **Elevated Barrier for Non-DBA Admins:** Application administrators without direct database access cannot create or manage custom commands.

## Solution

Introduce a comprehensive, graphical **Axi Command Configuration Management Suite** (`AxiCMDConfig.html`) accessible directly from the command palette via `Configure "Axi_CMD"`:

1. **Two-Tab Visual Management Console (`AxiCMDConfig.html`)**:
   - **Tab 1 — Dynamic Routes (`axi_command_config`)**: Full CRUD management of command routes with real-time filtering, parameter placeholder assistance, live URL simulation, and test execution (`Go` in shell / `Pop` in popup container). Includes a **Smart-Link Helper** toggle that automatically creates matching autocomplete tokens in `axi_command_prompts`.
   - **Tab 2 — Autocomplete Prompts (`axi_command_prompts`)**: Granular editor for prompt words, token lists (`promptvalues`), data sources (`promptsource`), and positional index alignment validation.
2. **Backend REST API Suite (`AxiApi_Beta`)**:
   - Dedicated REST endpoints to save/delete dynamic routes and retrieve/update autocomplete prompt tokens atomically across PostgreSQL and Oracle databases.
3. **Instant Cache Invalidation & Event Broadcast**:
   - Real-time cache flushing and cross-window event notification (`AXI_REFRESH_CONFIG`) so newly saved commands are immediately active in the palette without browser reload.
4. **Role-Based Security Gate**:
   - Gated under `appMgrAccess == true` to ensure only authorized administrators can modify command configurations.

## User Stories

1. As an application administrator, I want to type `Configure "Axi_CMD"` in the command palette, so that the system opens the visual Axi Command Configuration manager in the main application frame.
2. As an application administrator, I want to execute `Configure "Axi_CMD"` with the `Pop` action, so that the configuration manager opens as a tab inside `PopupContainer.html`.
3. As a non-administrator user without `appMgrAccess`, I want the `Configure "Axi_CMD"` command to be hidden and inaccessible, so that unauthorized users cannot view or alter command configurations.
4. As an administrator, I want to view all registered dynamic command routes in an interactive data table, so that I can see the active routing rules at a glance.
5. As an administrator, I want to filter dynamic routes by command group (`Configure`, `SDK`, `Upload`, `Download`, `View`, `Create`, `Edit`), so that I can quickly focus on specific command domains.
6. As an administrator, I want to search routes by configuration ID, command verb, prompt option, target TransID, or URL, so that I can find existing routes instantly.
7. As an administrator, I want to add a new dynamic route using a structured modal form, so that I don't have to write raw SQL `INSERT` statements.
8. As an administrator, I want to select the navigation type from supported modes (`tstruct`, `iview`, `tstruct/iview`, `url`, `processflow`, `ivtoivload`, `action`), so that the system formats the navigation URL automatically.
9. As an administrator, I want to specify parameter fields and extra query parameters, so that records open in edit mode or with required contextual flags.
10. As an administrator, I want a helper dropdown for dynamic placeholders (`:username`, `:userroles`, `:userresp`, `:appname`, `:param`, `:paramField`), so that I don't have to memorize token syntax.
11. As an administrator, I want an "Auto-link to Autocomplete Prompts" toggle in the Add Route modal, so that the prompt token is automatically added to `axi_command_prompts` with `Axi_Dummy` source without manual double-entry.
12. As an administrator, I want to simulate any command route in real-time, so that I can preview the computed URL and verified session parameters before saving.
13. As an administrator, I want a "Test in Shell (`Go`)" button on any route row, so that I can verify that the target page opens correctly in the main iframe.
14. As an administrator, I want a "Test in Tabbed Popup (`Pop`)" button, so that I can verify that the target page loads correctly inside a popup container tab.
15. As an administrator, I want to edit existing route properties (target URL, parameter field, extra params), so that I can update routes when backend forms change.
16. As an administrator, I want to toggle a route's active status between Active and Disabled, so that I can temporarily deactivate routes without deleting them.
17. As an administrator, I want to delete obsolete dynamic command routes with a confirmation dialogue, so that database clutter is prevented.
18. As an administrator, I want to switch to Tab 2 ("Autocomplete Prompts") to view raw prompt tokens per command group, so that I have complete visibility over multi-level dropdown prompts.
19. As an administrator, I want to edit `promptvalues` and `promptsource` strings directly in Tab 2, so that advanced multi-level prompts with custom data sources can be fine-tuned.
20. As an administrator, I want the UI to validate that the count of tokens in `promptvalues` matches the count of data sources in `promptsource`, so that positional index misalignment bugs are prevented before saving.
21. As an administrator, I want saving a route or prompt to automatically invalidate `localStorage` cache and notify the active command palette, so that newly added commands are usable immediately without restarting the browser.
22. As a developer, I want the backend API to work identically against PostgreSQL and Oracle databases, so that multi-tenant database environments are fully supported.

## Implementation Decisions

### 1. Frontend Configuration Console (`AxpertPlugins/Axi_Beta/HTMLPages/AxiCMDConfig.html`)
- Standalone HTML page with Metronic/Axpert UI styles (`UI/axpertUI/style.bundle.css`, `UI/axpertUI/plugins.bundle.css`).
- Built with a responsive Two-Tab layout:
  - **Tab 1: Dynamic Command Routes (`axi_command_config`)**: DataTable with group filters, search, Add/Edit modals, status toggles, and live URL simulator.
  - **Tab 2: Autocomplete Prompts (`axi_command_prompts`)**: Command-grouped prompt inspector, token list editor, and positional alignment validator.
- Integration: Dispatched by `executeDynamicNavigation` upon executing `Configure "Axi_CMD"`.
- Event Broadcast: On save, emits `top.window.postMessage({ type: "AXI_REFRESH_CONFIG" }, "*")` and clears `localStorage.removeItem("axi_command_config_v2")`.

### 2. Backend REST API Suite (`AxiApi_Beta`)
Add controller endpoints under `AxiApi/Controllers/CommandConfigController.cs` and `AxiApi/Controllers/CommandPromptsController.cs`:
- `GET /api/v1/command-config/getcommandconfig`: Fetches active route configurations.
- `POST /api/v1/command-config/save`: Upserts a record into `axi_command_config`.
- `POST /api/v1/command-config/delete`: Soft-deletes or removes a route by `config_id`.
- `GET /api/v1/command-prompts/getall`: Returns prompt definitions joined with `axi_commands`.
- `POST /api/v1/command-prompts/save`: Upserts prompt values and sources for a given `cmdtoken` and `wordpos`.

### 3. Database Migration Scripts
Update seed data in PostgreSQL and Oracle structure scripts:
- `Structures/Postgre/Scripts/axi_command_config.sql` & `Structures/Oracle/Scripts/axi_command_config.sql`:
  - Register `cfg_configure_axi_cmd` $\rightarrow$ `../AxpertPlugins/Axi_Beta/HTMLPages/AxiCMDConfig.html`.
- `Structures/Postgre/Scripts/axi_command_tables.sql` & `Structures/Oracle/Scripts/axi_command_tables.sql`:
  - Append `Axi_CMD` to `promptvalues` under `Configure` (`cmdtoken = 4, wordpos = 2`).
  - Append `Axi_Dummy` to `promptsource` under `Configure` (`cmdtoken = 4, wordpos = 3`).

### 4. Client Shell Invalidation Listener (`axicmdmain.js`)
- Add message event listener for `AXI_REFRESH_CONFIG` to reload command metadata and re-index `axiCommandConfigMap` silently.

## Testing Decisions

### Seams for Testing
1. **Primary Seam — API Contract Layer (`AxiApi_Beta`)**:
   - Direct HTTP POST/GET test calls against `/api/v1/command-config/save`, `/delete`, and `/api/v1/command-prompts/save` on PostgreSQL and Oracle test databases.
2. **Integration Seam — End-to-End Navigation**:
   - Type `Configure "Axi_CMD"` in the command palette $\rightarrow$ verify page loads via `LoadIframe` and via `openPopOption`.
3. **Behavioral Seam — Real-Time Cache Update**:
   - Create a test route `Configure TestRoute` in `AxiCMDConfig.html` $\rightarrow$ save $\rightarrow$ immediately activate palette with `Ctrl + Space` and verify `TestRoute` is suggested and executable.

### What Makes a Good Test
- Tests must verify external behavior (HTTP response codes, database persistence, DOM rendering, navigation dispatch).
- No testing of private variables or internal implementation details.

## Out of Scope

1. Redesigning core Axpert developer studio pages (TStruct builder, IView builder).
2. Modifying non-command palette configurations (e.g. general Axpert application settings).
3. Allowing non-administrators to bypass `appMgrAccess` security checks.

## Further Notes

- The Two-Tab interface is designed to be extensible for future plugin management and custom workflow actions.
- All file paths and script versions are aligned with repository standards (`v=270`).
