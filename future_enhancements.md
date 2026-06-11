# Axi Command Palette | Future UX & Architecture Enhancements

This document outlines proposed feature enhancements, UI optimizations, and architectural improvements to make the Axi Command Palette more user-friendly, responsive, and feature-rich.

---

## 🔍 1. Search & Autocomplete Enhancements

### Fuzzy Search Matching
*   **Proposed Improvement:** Replace the current synchronous sequential regex tokenizer with a fuzzy-matching search engine (e.g., Fuse.js or a scoring algorithm like LiquidMetal).
*   **User Benefit:** Users can type non-contiguous or misspelled queries (e.g., `"usr perm"` matches `"User Permissions"`) and still find the correct command.

### Substring Highlighting
*   **Proposed Improvement:** Dynamically wrap matching substrings inside suggestion labels with HTML/CSS tags (e.g., `<span class="search-match">`).
*   **User Benefit:** Provides clear visual confirmation of why a specific command appeared in the suggestions.

---

## 🎨 2. UI/UX & Visual Enhancements

### Command Badging and Iconography
*   **Proposed Improvement:** Introduce visual badges or distinct SVG icons next to suggestions representing their Command Group (`Create`, `Edit`, `Configure`, `DevTools`).
*   **User Benefit:** Enhances scannability, letting users differentiate commands at a glance.

### Contextual Metadata Tags
*   **Proposed Improvement:** Display type-specific metadata tags next to search results (e.g., `[Tstruct]`, `[Iview]`, `[Direct SQL]`).
*   **User Benefit:** Ensures clarity about what actions are triggered (e.g., opening a form vs. executing a raw script).

---

## ⚡ 3. Functional & Operational Features

### Smart Navigation History
*   **Proposed Improvement:** Keep a "Recently Used" section at the top of the command list when the search field is empty.
*   **User Benefit:** Eliminates repetitive typing for users navigating back and forth between the same set of screens.

### Favorite Command Pinning
*   **Proposed Improvement:** Add a star or bookmark action next to suggestions allowing users to pin frequently used commands.
*   **User Benefit:** Keeps critical workflows permanently accessible at the top of the palette.

---

## 🧠 4. Contextual Intelligence

### Page-Aware Suggestions
*   **Proposed Improvement:** Make the command palette aware of the active frame/Tstruct in the Axpert workspace.
*   **User Benefit:** Pre-fills parameters or prioritizes related commands at the top (e.g., showing "Edit current Tstruct" when viewing an active transaction page).

---

## 💬 5. User Interaction & Parameter Entry

### Guided Multi-Step Modals
*   **Proposed Improvement:** Convert the prompt-parameter input field into a guided multi-step wizard inside the palette modal.
*   **User Benefit:** Provides a clean interface for entering parameters (e.g., dropdown selectors for Tstructs and search filters for Record IDs) instead of typing raw command strings.

### Sidebar Previews
*   **Proposed Improvement:** Render a side-drawer preview panel when navigating suggestions.
*   **User Benefit:** Displays information such as structural definitions, query descriptions, or author metadata before executing the command.
