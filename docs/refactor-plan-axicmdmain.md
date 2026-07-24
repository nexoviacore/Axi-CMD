# Refactor Plan: Axi Command Palette Architecture (`axicmdmain.js`)

## Problem Statement

The `axicmdmain.js` file is a monolithic ~14,200 line browser script. It handles command tokenization, DOM rendering, keyboard shortcuts, local storage, backend `WebService.asmx` requests, and tour walkthroughs within a single procedural IIFE. 

Because business logic, UI manipulation, and network calls are tightly coupled, the file suffers from:
- High friction when modifying or extending command logic.
- Inability to unit test parsing or data transformation logic without mock DOM environments.
- Risk of unintended side effects when modifying event handlers or helper functions.

## Solution

Refactor `axicmdmain.js` using internal Domain Module Abstractions within the IIFE scope (Option A). We will decouple the file into five clear domain responsibilities:

1. **`AxiCmdConfig`**: Centralized system constants, command icons, and action shortcut definitions.
2. **`AxiCommandParser`**: Pure, stateless tokenization and command parsing logic.
3. **`AxiApiService`**: Asynchronous HTTP client interface encapsulating `WebService.asmx` requests.
4. **`AxiFavoritesManager`**: Encapsulated state persistence and validation for user favorites and history.
5. **`AxiPaletteViewController`**: UI render engine, DOM event binding, and autocomplete suggestions controller.

All changes will be executed in tiny, incremental commits that preserve 100% backward compatibility and zero runtime regressions.

## Commits

1. `refactor(cmd-palette): Extract AxiCmdConfig for centralized constants and action icons`
   - Define `AxiCmdConfig` object containing `MAX_HISTORY`, `MAX_FAVORITES`, `SHORTCUT_OPTIONS`, and `COMMAND_ICONS`.
   - Replace magic numbers and hardcoded string keys throughout the script with config references.

2. `refactor(cmd-palette): Extract pure AxiCommandParser module for tokenization`
   - Extract `tokenize()`, `parseCommand()`, `getTokens()`, and string helpers into `AxiCommandParser`.
   - Ensure all string parsing functions are pure and free of DOM side-effects.

3. `refactor(cmd-palette): Extract AxiApiService for backend web service communication`
   - Isolate `fetch` calls (`GetSession`, `getARMSessionId`, `GetDataFromAxList`) inside `AxiApiService`.
   - Standardize session timeout validation checks and error logging.

4. `refactor(cmd-palette): Extract AxiFavoritesManager for local storage and state`
   - Move `loadFavorites()`, `toggleFavorite()`, and `localStorage` key generation into `AxiFavoritesManager`.
   - Consolidate validation rules (e.g. blocking `help`/`run` commands from favorites).

5. `refactor(cmd-palette): Connect UI controller handlers to extracted domain modules`
   - Update `handleInput()`, `createSuggestionLi()`, `render()`, and event listeners to call domain module methods.
   - Verify that all public window hooks (`LoadIframeac`) remain intact.

## Decision Document

- **Modules Built / Modified**:
  - `axicmdmain.js`: Refactored internally into `AxiCmdConfig`, `AxiCommandParser`, `AxiApiService`, `AxiFavoritesManager`, and UI Controller.
  - `axicmdmain.css` & `AxiCMDMainPage.html`: Version query parameters updated for cache busting.
- **Architectural Decisions**:
  - **Single Bundle**: Retain single-file distribution inside `axicmdmain.js` to ensure zero impact on ASP.NET runtime pages (`Default.aspx`).
  - **Stateless Parser**: Keep parser functions pure to allow future extraction into unit tests.
- **API Contracts**:
  - All existing WebService request endpoints (`WebService.asmx/GetSession`, `GetDataFromAxList`) remain identical.

## Testing Decisions

- **Testing Approach**: Manual regression testing across all command palette user workflows since no automated unit test runner is currently integrated in `UI/compiler/tools/`.
- **Target Workflows to Validate**:
  1. Keyboard Shortcut Activation (`Ctrl+Space`).
  2. Autocomplete Suggestions & Command Verb Filtering (`create`, `edit`, `view`, `configure`, `sdk`, `upload`, `download`).
  3. Command Execution (`Enter`, `Ctrl+Enter`, `Ctrl+Shift+Enter`).
  4. Favorites Add, Edit, Delete, and Sync across page reloads.
  5. Session Validation Timeout & Redirect handling.

## Out of Scope

- Backend C# code or `WebService.asmx` WebMethod modifications.
- Webpack / Gulp build toolchain restructuring.
- Walkthrough tour library (`intro.min.js`) modifications.

## Further Notes

Each step of this refactor plan can be applied incrementally and verified in the browser before moving to the next commit.
