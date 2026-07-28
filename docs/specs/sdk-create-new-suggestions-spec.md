# Specification: SDK "Create New" Suggestions Feature

## Problem Statement

Developers and system administrators using the Axi CMD command palette currently have to navigate through lists of existing structures or know explicit creation URLs to open builders for creating new TStructs, IViews, Pages, or Axpert Data Sources. When entering commands like `sdk tstruct` or `sdk iview`, only existing structures are listed in auto-suggestions, introducing unnecessary friction when creating new items from scratch.

## Solution

Incorporate a top-level, pinned **"Create New"** option in Axi CMD suggestions for developer studio builder commands (`sdk tstruct`, `sdk iview`, `sdk page`, `sdk axpert data sources` / `sdk ads`). Selecting or executing "Create New" directly launches the corresponding developer studio builder or configuration form in creation mode. The "Create New" suggestion item is pinned at the top of the suggestions list regardless of typed search text.

## User Stories

1. As a developer, I want to see a "Create New" option at the top of Axi suggestions when I type `sdk tstruct`, so that I can immediately open the TStruct Builder to create a new transaction structure.
2. As a developer, I want to see a "Create New" option at the top of Axi suggestions when I type `sdk iview`, so that I can open the IView Builder in creation mode without selecting an existing report.
3. As a developer, I want to see a "Create New" option at the top of Axi suggestions when I type `sdk page`, so that I can launch the Page Designer form for creating a new system page.
4. As a developer, I want to see a "Create New" option at the top of Axi suggestions when I type `sdk axpert data sources` (or `sdk ads`), so that I can open the ADS SQL Builder form to define a new data source.
5. As a developer, I want the "Create New" suggestion item to remain pinned at the top of the suggestion dropdown even when I type partial search text, so that creation remains accessible without clearing my input.
6. As a developer, I want the "Create New" item to feature an action badge icon, so that it is visually distinguished from existing structure names.
7. As a system administrator, I want keyboard navigation (Arrow keys and Enter) to seamlessly select and execute the "Create New" option, so that I can maintain a high-velocity command-line workflow.

## Implementation Decisions

- **Target Commands**: The "Create New" option is scoped to four SDK builder command categories: `sdk tstruct`, `sdk iview`, `sdk page`, and `sdk axpert data sources` (including `sdk ads`).
- **Suggestion Ingestion & Pinning**: When generating suggestions for these SDK commands, a specialized item (`{ displaydata: "Create New", name: "Create New", isCreateNew: true }`) is prepended as the very first suggestion item and pinned against filtering.
- **Execution Routing**:
  - `sdk tstruct "Create New"` → Invokes Developer Studio TStruct Builder (`tstreact`) in create mode.
  - `sdk iview "Create New"` → Invokes Developer Studio IView Builder (`ivreact`) in create mode.
  - `sdk page "Create New"` → Loads the Page Designer form (`tstruct.aspx?transid=sect`).
  - `sdk axpert data sources "Create New"` → Loads the ADS Builder form (`tstruct.aspx?transid=b_sql`).
- **Permission Enforcement**: "Create New" options are restricted to users with developer/build access permissions (`buildAccess`).

## Testing Decisions

- **Testing Seams**: Manual verification seam via browser execution in `Default.aspx` command shell environment.
- **Test Scenarios**:
  - Verify suggestion rendering when typing `sdk tstruct`, `sdk iview`, `sdk page`, and `sdk ads`.
  - Verify "Create New" remains pinned at the top when typing partial filter text.
  - Verify mouse click and Enter key execution open the correct target builder URLs/iframes.
  - Verify unauthorized users without build access do not see SDK builder commands.

## Out of Scope

- Support for "Create New" on non-SDK view/create command groups.
- Modifying the internal UI/schema of Developer Studio builders (`tstreact` / `ivreact`).

## Further Notes

- Script versioning in `AxiCMDMainPage.html` should be incremented upon implementation to invalidate browser cache.
