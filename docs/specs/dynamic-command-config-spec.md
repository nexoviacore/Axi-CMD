# Spec: Dynamic AXI Command Configuration and Redirection Engine

## Problem Statement

Currently, transaction IDs, IView names, and URL redirections for command palette verbs (specifically `Configure`, `SDK`, `Upload`, and `Download`) are hardcoded directly within client-side JavaScript handlers in `axicmdmain.js`. This creates several issues:

1. **Tight Coupling**: Any change to a transaction ID, parameter field, or target URL requires modifying and re-deploying frontend bundle scripts.
2. **Poor Extensibility**: Adding new plugins, administrative forms, or developer studio tools requires writing new custom JavaScript functions.
3. **High Maintenance Overhead**: Dispersed redirection functions create boilerplate redundancy and elevate the risk of regressions across command options.

## Solution

Introduce a database-driven dynamic command configuration architecture:

1. **Configuration Table (`axi_command_config`)**: A dedicated database table storing command-to-structure mappings, target types, parameter fields, target URLs, and extra query arguments for PostgreSQL and Oracle databases.
2. **Backend API Endpoint**: A clean layered endpoint (`GET /api/v1/Axi/command-config`) in `AxiApi` leveraging `AxExtend` and `ARMCommon` to fetch active command configurations per application.
3. **Dynamic Frontend Dispatcher**: A generic navigation execution engine in `axicmdmain.js` that fetches the configuration on startup and dynamically resolves and navigates to target TStructs, IViews, IView-to-IView loaders, or custom URLs without hardcoded routing functions.

## User Stories

1. As an application developer, I want AXI Command palette navigation routes to be stored in a centralized database table, so that transaction IDs and target URLs can be updated or extended without modifying frontend JavaScript files.
2. As a system administrator, I want to type `configure user listing` in the command palette, so that the system redirects dynamically to the `axusers` interactive view.
3. As a system administrator, I want to type `configure user` with an optional username, so that the system opens the `axusr` transaction structure in create or edit mode with the username populated.
4. As a system administrator, I want to type `configure role listing`, so that the system loads the `ad___url` IView dynamically.
5. As a system administrator, I want to type `configure role` with a role name, so that the system opens the `ad_ur` transaction structure with the selected role.
6. As an administrator, I want to type `configure responsibility listing`, so that the system dynamically loads the `response` interactive view.
7. As an administrator, I want to type `configure responsibility`, so that the system opens `AddEditResponsibility.aspx` in add mode or edit mode dynamically.
8. As an administrator, I want to type `configure actor listing`, so that the system dynamically opens the `ad__act` interactive view.
9. As an administrator, I want to type `configure actor` with an actor name, so that the system opens the `ad_am` transaction structure with the `actorname` field populated.
10. As a developer, I want to type `configure dimension listing`, so that the system opens `ivtoivload.aspx?ivname=ad___upg` with appropriate loader flags.
11. As a developer, I want to type `configure dimension`, so that the system loads the `a_pgm` transaction structure with the `grpname` parameter.
12. As a developer, I want to type `configure smart view listing`, so that the system opens `ivtoivload.aspx?ivname=a___smtl` dynamically.
13. As a developer, I want to type `configure smart view attributes`, so that the system loads `a__sl` with the `adsname` parameter.
14. As an administrator, I want to type `configure user group`, so that the system opens the `a__ug` transaction structure with the `users_group_name` field.
15. As an administrator, I want to type `configure user activation`, so that the system opens the `axurg` transaction structure with the `pusername` field.
16. As an administrator, I want to type `configure user permissions`, so that the system loads `ivtoivload.aspx?ivname=ad___upm`.
17. As an administrator, I want to type `configure user permission setup`, so that the system opens `a__up` with `axusername` and appropriate caller parameters.
18. As an administrator, I want to type `configure role permissions`, so that the system loads `ivtoivload.aspx?ivname=ad___ups` with the `prole` parameter.
19. As a developer, I want to type `configure publish axpert api`, so that the system opens the `ad_pa` structure with the `publickey` field.
20. As a developer, I want to type `configure publish config studio`, so that the system opens the `ad_pbcs` IView.
21. As a developer, I want to type `configure card`, so that the system opens the `a__cd` transaction structure with the `cardname` parameter.
22. As an administrator, I want to type `configure peg`, `configure rule`, `configure form notification`, `configure peg form notification`, `configure scheduled notification`, `configure keyfield`, `configure application properties`, or `configure settings`, so that the system resolves each to its configured target structure or configuration page.
23. As a developer, I want to type `sdk axpert data sources`, `sdk page`, `sdk app variables`, `sdk dev option`, `sdk db explorer`, `sdk arrange menu`, `sdk api plugin`, `sdk axpert job`, `sdk language`, `sdk publish`, `sdk custom data type`, `sdk email definition`, `sdk table field descriptor`, `sdk mem db console`, `sdk custom plugin`, `sdk queue listing`, `sdk out bound queue`, or `sdk in bound queue`, so that each target is dynamically resolved and loaded without hardcoded handlers in the frontend.
24. As an administrator, I want to type `upload` / `import data`, so that the system dynamically routes to `importall.aspx`.
25. As an administrator, I want to type `download` / `export data`, so that the system dynamically routes to `exportnew.aspx`.
26. As a developer, I want newly added plugin commands inserted into `axi_command_config` to be immediately executable without modifying or rebuilding client JS code.

## Implementation Decisions

### 1. Database Schema (`axi_command_config`)
Create table `axi_command_config` across PostgreSQL and Oracle DDL scripts in `AxpertPlugins/Axi_Beta/Structures`:
- `config_id` (varchar(50), Primary Key)
- `command` (varchar(50), Indexed command group verb, e.g. `Configure`, `SDK`, `Upload`, `Download`)
- `prompt_options` (varchar(200), Prompt option key matching `COMMAND_HANDLERS`)
- `prompt_id` (varchar(50), Structure ID or target identifier)
- `prompt_option_type` (varchar(20), Navigation type: `tstruct`, `iview`, `ivtoivload`, `url`)
- `param_field` (varchar(100), Target field name for parameterized value handoff)
- `target_url` (varchar(500), Explicit target URL path override)
- `extra_params` (varchar(500), Static query string parameters)
- `active` (varchar(1), Active status indicator, default `'T'`)

### 2. Backend Service Layer in `AxiApi`
- **DTO**: `CommandConfigDTO` containing all mapped table fields.
- **Repository Interface & Implementation**: `ICommandConfigRepository` and `CommandConfigRepository` utilizing `IAxExtend` (`OpenDBConnectionAsync` and `GetDB()`) to execute parameterized SELECT queries with `DatabaseException` error handling.
- **Service Interface & Implementation**: `ICommandConfigService` and `CommandConfigService` providing cached/uncached retrieval.
- **API Controller**: Add `[HttpGet("command-config")]` endpoint in `AxiController` accepting `appname` query parameter.
- **Dependency Injection**: Register scoped services and repositories in `Program.cs`.

### 3. Frontend Execution Engine in `axicmdmain.js`
- **Cache Store**: On application startup or first command interaction, asynchronously call `AxiApi/api/v1/Axi/command-config?appname={appname}` and store the result in memory (`window.axiCommandConfigStore`).
- **Dynamic Dispatcher**: Implement `executeDynamicNavigation(configRow, tokens)`:
  - If `prompt_option_type === "tstruct"`: invoke `redirectToTstruct(prompt_id, tokens[1], hasParam, param_field, paramValue)`.
  - If `prompt_option_type === "iview"`: invoke `redirectToIView(prompt_id, tokens[1])`.
  - If `prompt_option_type === "ivtoivload"`: construct URL with `target_url`, `extra_params`, and encoded parameters, then invoke `window.LoadIframe(url)`.
  - If `prompt_option_type === "url"`: construct target URL and invoke `window.LoadIframe(url)`.
- **Command Handler Delegation**: Map `COMMAND_HANDLERS.Configure`, `COMMAND_HANDLERS.SDK`, `COMMAND_HANDLERS.Upload`, and `COMMAND_HANDLERS.Download` to the dynamic dispatcher with fallback support.

## Testing Decisions

- **Testing Seam**: The highest seam for backend testing is the HTTP API boundary (`GET /api/v1/Axi/command-config`), verifying database queries, parameter bindings, and DTO serialization through integration tests.
- **Frontend Seam**: The top-level command submission and execution hook (`handleEnterKey` / `executeDynamicNavigation`), verifying URL generation and iframe redirection for each configured prompt type.
- **Test Criteria**:
  - Verify that all active entries in `axi_command_config` are returned in the API response.
  - Verify that `tstruct`, `iview`, `ivtoivload`, and `url` targets generate expected URLs and parameters.
  - Verify that disabled (`active = 'F'`) commands are excluded from execution.
  - Verify that failure to reach the API degrades gracefully with user-friendly error notification without crashing the command palette.

## Out of Scope

- Modifying core transaction structure or interactive view runtime logic (`tstruct.aspx`, `iview.aspx`).
- Refactoring NLP or AI-driven prompt parsing engines (`ask`, `ai`).
- Modifying non-navigation commands (e.g. `run` toolbar button execution, `edit` field value patching).

## Further Notes

- SQL scripts in `AxpertPlugins/Axi_Beta/Structures` adhere to the existing `<< ... >>` statement delimiter format used by the Axpert deployment installer.
- Cache version in `CustomPages/AxiCMDMainPage.html` should be bumped upon deploying the frontend script updates.
