# Axi Command Palette (`Axi_Beta`)

[![Plugin Version](https://img.shields.io/badge/Axi--Plugin-v0.5.0--rc-orange.svg)](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta)
[![Runtime](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Database](https://img.shields.io/badge/Database-PostgreSQL%20%7C%20Oracle-green.svg)](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta/Structures)

The **Axi Command Palette** (`Axi_Beta`) is an in-app developer and user command navigation engine designed for the Axpert Web Shell interface. It provides real-time token autocompletion, dynamic transaction routing, multi-entity structure execution (TStructs, IViews, Axpert Data Sources / ADS, Pages, and Inbox), form keyfield management, tabbed record popups, and user favorites.

---

## Architecture Overview

Axi operates across a decoupled three-tier architecture that integrates directly into the Axpert shell:

```mermaid
graph TD
    User([Axpert Web User]) <-->|Ctrl + Space / Search Bar| Frontend[Frontend UI Shell]
    Frontend <-->|REST API Requests| Backend[AxiApi Backend Service]
    Backend <-->|Npgsql / Oracle Data Access| DB[(PostgreSQL / Oracle DB)]
    Backend <-->|Distributed Cache| Redis[(Redis Cache)]

    subgraph Frontend Components
        Frontend -->|Command Engine & Parser| Main[axicmdmain.js]
        Frontend -->|Tabbed Record Container| Popup[PopupContainer.html]
        Frontend -->|Interactive Lists| Smart[Smartview.html]
    end
```

### 1. Frontend Layer
* **`AxiCMDMainPage.html`**: Host template loaded by Axpert as the application shell interface.
* **`axicmdmain.js`**: Core client engine handling input parsing, token resolution, state tracking, suggestion ranking, dynamic command execution, and favorites synchronization.
* **`Smartview.html`**: Interactive list view container supporting dynamic filters, column groupings, and bulk record operations.
* **`PopupContainer.html`**: Tabbed layout engine allowing users to execute and view multiple transaction records concurrently.

### 2. Backend API (`AxiApi`)
* Built on **ASP.NET Core (.NET 8)** with asynchronous request handlers.
* Hosted as an IIS Application inside the ARM microservices environment.
* Manages command configuration retrieval, grammar bootstrapping, user favorites persistence, and form keyfield configuration (`setkeyfield`).
* Features automated database connection retry resilience to protect against transient stream disconnects.

### 3. Database Layer
* Stores command catalog definitions, prompt sequences, user permission mappings, and TStruct property metadata across PostgreSQL and Oracle.
* Scripts located under [`Structures/`](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta/Structures/).

---

## Core Capabilities

* **Dynamic Command Routing (`axi_command_config`):** Eliminates hardcoded transaction IDs and navigation URLs. Commands under `Configure`, `SDK`, `Upload`, and `Download` are dynamically fetched and cached from the database.
* **Form Keyfield Configuration:** Direct execution of `Configure KeyField <TStruct> <Field>` via the backend API (`/api/v1/Axi/setkeyfield`), replacing legacy database stored functions with secure, parameterized SQL.
* **Studio Tool Permissions:** Granular role-based developer option filtering for SDK commands, ensuring core studio tools (`tstruct`, `iview`) remain accessible based on user permissions.
* **Structure Disambiguation:** Resolves entities sharing identical names across TStructs (`[form]`), IViews (`[iview]`), ADS (`[ads]`), Pages (`[page]`), and Inbox with context-aware default rules.
* **User Favorites & Custom Aliasing:** Allows bookmarking frequent commands with custom display aliases and direct iframe deep linking.
* **Session & State Management:** Preserves resolved parameter states across token edits while resetting cleanly on clear button clicks, empty inputs, or refresh.

---

## Database Objects Catalog

The database setup scripts are organized by provider under [`Structures/Postgre/Scripts/`](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta/Structures/Postgre/Scripts/) and [`Structures/Oracle/Scripts/`](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta/Structures/Oracle/Scripts/).

### Core Tables

| Table | Purpose |
| :--- | :--- |
| `axi_command_config` | Dynamic routing rules, target prompt IDs, option types (`tstruct`, `iview`, `action`, `url`), and active status flags. |
| `axi_commands` | Root command verbs, aliases, and active statuses. |
| `axi_command_prompts` | Dynamic autocomplete prompt fields, positional order, and data sources. |
| `axp_tstructprops` | TStruct property definitions and primary key field configurations (`keyfield`, `userconfigured`). |
| `axi_userfavourites` | User-defined command shortcuts, custom alias labels, and target URLs. |
| `axdirectsql_metadata` | Cached metadata and field captions for Direct SQL / ADS queries. |

### Core Stored Procedures & Functions

* **`fn_axi_getstructures_meta`**: Returns metadata for all TStructs, IViews, Pages, and ADS structures accessible to the current user.
* **`fn_axi_getstructs_obj`**: Dynamically queries records from a target structure with permission and row-level dimension filtering.
* **`fn_permissions_getpermission`**: Validates user access rights (`Create`, `View`, `Edit`) for transaction IDs (`transid`).

---

## Deployment & Setup

```mermaid
graph LR
    Step1[1. Core Plugin Copy] --> Step2[2. Template Setup]
    Step2 --> Step3[3. App Template Config]
    Step3 --> Step4[4. IIS API Hosting]
```

### 1. Core Files Placement
1. Install or copy the `Axi_Beta` plugin folder into `/AxpertPlugins/Axi_Beta/`.
2. Copy `AxiCMDMainPage.html` from `AxpertPlugins/Axi_Beta/HTMLPages/` into `../CustomPages/AxiCMDMainPage.html`.

> [!IMPORTANT]
> Do not rename `AxiCMDMainPage.html`. The Axpert routing system relies on this exact filename.

### 2. Axpert Environment Configuration
1. Log in to **AxpertWeb** $\rightarrow$ Navigate to **Dev Options**.
2. Set **Application Template** property to `AxiCMDMainPage.html`.
3. If `AxiCMDMainPage.html` is not visible in the dropdown, navigate to **Configuration Property List**, edit **Application Template**, and append `AxiCMDMainPage.html` to the Values collection.

### 3. IIS Backend API (`AxiApi`) Hosting
1. Publish and copy `AxiApi` into your target **Arm microservices** directory.
2. In IIS Manager, create an Application Pool named `AxiApi_Beta` set to **No Managed Code**.
3. Create a new IIS Application pointing to the `AxiApi` directory under the default website.
4. Ensure `appsettings.ini` is present in the parent microservices folder to supply database connection credentials.
5. Grant **Read & Write** file permissions to the IIS Application Pool identity (`IIS_IUSRS`).

---

## Key Files Reference

* [`HTMLPages/js/axicmdmain.js`](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta/HTMLPages/js/axicmdmain.js): Command palette core JavaScript engine.
* [`AxiCMDVersioninfo.json`](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta/AxiCMDVersioninfo.json): Version tracker manifest.
* [`CustomPages/AxiCMDMainPage.html`](file:///D:/Axpert11.4/AxpertWebLatest/CustomPages/AxiCMDMainPage.html): Main application template.
* [`HTMLPages/Smartview.html`](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta/HTMLPages/Smartview.html): Dynamic list view engine for ADS and IViews.
* [`HTMLPages/PopupContainer.html`](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta/HTMLPages/PopupContainer.html): Multi-tab popup window manager.
* [`DOCUMENTATION.md`](file:///D:/Axpert11.4/AxpertWebLatest/AxpertPlugins/Axi_Beta/DOCUMENTATION.md): Detailed technical & developer guide.
