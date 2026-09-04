# AxiApi

Backend service layer powering the **AXI (Axpert Command Line Interface)** engine. AxiApi provides dynamic command resolution, command grammar bootstrapping, customizable user favorites, form keyfield configuration, multi-tenant database connectivity, and distributed Redis caching for the Axpert web shell.

---

## Architecture Overview

AxiApi is built on **ASP.NET Core (.NET 8)** adhering to a clean, decoupled layered architecture:

```
┌────────────────────────────────────────────────────────┐
│               AxiController (API Endpoints)            │
└───────────────────────────┬────────────────────────────┘
                            │
┌───────────────────────────▼────────────────────────────┐
│      Service Layer (Business Logic & Validation)       │
│  - CommandConfigService       - UserFavouritesService  │
│  - KeyfieldService            - GrammarService         │
│  - GrammarBootstrapService                             │
└───────────────────────────┬────────────────────────────┘
                            │
┌───────────────────────────▼────────────────────────────┐
│    Repository Layer (Data Access via AxExtend & SQL)   │
│  - CommandConfigRepository    - UserFavouritesRepo     │
│  - KeyfieldRepository         - GrammarRepository      │
│  - SqlQueries (Centralized Queries & DB Functions)     │
└───────────────────────────┬────────────────────────────┘
                            │
┌───────────────────────────▼────────────────────────────┐
│       Database & Cache (PostgreSQL / Oracle / Redis)   │
└────────────────────────────────────────────────────────┘
```

### Key Capabilities

- **Dynamic Command Configuration**: Eliminates hardcoded transaction routing by querying and caching the `axi_command_config` table for `Configure`, `SDK`, `Upload`, and `Download` actions.
- **Form Keyfield Configuration (`setkeyfield`)**: Directly configures and updates primary keyfields for TStruct forms (`axp_tstructprops`) via parameterized database operations and database-level timestamp functions.
- **Grammar Metadata Bootstrapping**: Generates and parses command trees, prompt sources, parameter positions, and autocompletion tokens.
- **User Favorites Management**: Supports creating, listing, renaming, and removing custom command aliases with deep link target URLs.
- **Multi-Tenant Connection Resilience**: Utilizes `AxExtend` and `ARMCommon` with automated retry logic to handle transient connection pool stream disconnects.
- **Resilient Redis Caching**: Automatic fallback to direct database queries if the distributed cache is unavailable, with force-refresh cache invalidation.

---

## API Reference

All endpoints are versioned under the `/api/v1/Axi` route prefix.

### 1. Dynamic Command Configuration

Retrieves the active mapping of command verbs, prompt options, target transaction IDs, and navigation URLs.

```http
GET /api/v1/Axi/command-config?appname={appname}&username={username}&forceRefresh={forceRefresh}
```

#### Query Parameters

| Parameter | Type | Required | Default | Description |
| :--- | :--- | :--- | :--- | :--- |
| `appname` | string | Yes | — | Axpert application database identifier |
| `username` | string | Yes | — | Active logged-in username |
| `forceRefresh` | boolean | No | `false` | When `true`, invalidates the Redis cache and queries the database directly |

#### Response (`200 OK`)

```json
[
  {
    "configId": "cfg_configure_users",
    "command": "Configure",
    "promptOptions": "user listing",
    "promptId": "axusers",
    "promptOptionType": "iview",
    "paramField": null,
    "targetUrl": null,
    "extraParams": null,
    "active": "T"
  },
  {
    "configId": "cfg_configure_user",
    "command": "Configure",
    "promptOptions": "user",
    "promptId": "axusr",
    "promptOptionType": "tstruct",
    "paramField": null,
    "targetUrl": null,
    "extraParams": null,
    "active": "T"
  }
]
```

---

### 2. Form Keyfield Configuration

Configures the primary search/key field for a specified TStruct form. Automatically performs an upsert into `axp_tstructprops` using database-level timestamp evaluation.

```http
POST /api/v1/Axi/setkeyfield
Content-Type: application/json
```

#### Request Body

```json
{
  "appName": "axpbase",
  "transId": "axusr",
  "keyField": "username",
  "username": "admin"
}
```

| Field | Type | Required | Description |
| :--- | :--- | :---: | :--- |
| `appName` | string | Yes | Axpert application database instance identifier |
| `transId` | string | Yes | TStruct structure ID / form name |
| `keyField` | string | Yes | Database column name to configure as keyfield |
| `username` | string | Yes | Active user login performing the configuration |

#### Response (`200 OK`)

```json
{
  "success": true,
  "message": "Success",
  "statusCode": 200
}
```

---

### 3. Command Grammar Metadata

Fetches the complete grammar syntax tree for autocomplete suggestion parsing.

```http
GET /api/v1/Axi/axi_get?view={view}&forceRefresh={forceRefresh}&appname={appname}
```

#### Query Parameters

| Parameter | Type | Required | Description |
| :--- | :--- | :--- | :--- |
| `view` | string | Yes | View identifier (e.g. `Metadata`) |
| `appname` | string | Yes | Application database identifier |
| `forceRefresh` | boolean | No | Bypass and refresh cached grammar |

---

### 4. User Favorites

#### Get User Favorites

```http
GET /api/v1/Axi/user-favourites?username={username}&appname={appname}
```

#### Add or Toggle Favorite

```http
POST /api/v1/Axi/user-favourites?appname={appname}
Content-Type: application/json
```

```json
{
  "username": "admin",
  "commandText": "My User List",
  "originalCommandText": "configure user listing",
  "action": "add",
  "favOrder": 0,
  "targetURL": "../aspx/iview.aspx?ivname=axusers"
}
```

#### Update / Rename Favorite Alias

```http
PATCH /api/v1/Axi/user-favourites/{favouritesId}?username={username}&appname={appname}
Content-Type: application/json
```

```json
{
  "commandText": "Renamed Favorite Name"
}
```

---

## Database Schema

AxiApi interacts with the following core tables (located in `AxpertPlugins/Axi_Beta/Structures/` for both PostgreSQL and Oracle):

### `axi_command_config`

| Column | Type | Constraints | Description |
| :--- | :--- | :--- | :--- |
| `config_id` | `VARCHAR(50)` | Primary Key | Unique configuration identifier (e.g. `cfg_configure_user`) |
| `command` | `VARCHAR(50)` | Not Null, Indexed | Command group verb (`Configure`, `SDK`, `Upload`, `Download`) |
| `prompt_options` | `VARCHAR(200)` | Not Null | Command prompt sub-action (e.g. `user listing`, `smart view`) |
| `prompt_id` | `VARCHAR(50)` | Not Null | Target identifier (`transid`, `ivname`, or script name) |
| `prompt_option_type`| `VARCHAR(20)` | Not Null | Navigation/Execution mode (`tstruct`, `iview`, `action`, `url`) |
| `param_field` | `VARCHAR(100)` | Nullable | Parameter field name used for record loading |
| `target_url` | `VARCHAR(500)` | Nullable | Explicit redirection URL override |
| `extra_params` | `VARCHAR(500)` | Nullable | Additional query parameters (e.g. `AxOpenAct=true&isDupTab=false`) |
| `active` | `VARCHAR(1)` | Default `'T'` | Status flag (`T` / `F`) |

### `axp_tstructprops`

| Column | Type | Description |
| :--- | :--- | :--- |
| `name` | `VARCHAR(50)` | Primary Key / TStruct identifier (e.g. `axusr`) |
| `caption` | `VARCHAR(500)` | TStruct form display caption |
| `keyfield` | `VARCHAR(200)` | Primary search key field configured for the form |
| `userconfigured` | `CHAR(1)` | Flag `'t'` indicating user-configured keyfield |
| `createdon` | `VARCHAR(30)` | Timestamp string populated via `TO_CHAR(CURRENT_TIMESTAMP, ...)` |
| `createdby` | `VARCHAR(100)` | Username who created the record |
| `updatedon` | `VARCHAR(30)` | Timestamp string of last update |
| `updatedby` | `VARCHAR(100)` | Username who updated the record |

### `axi_userfavourites`

| Column | Type | Description |
| :--- | :--- | :--- |
| `id` | `VARCHAR/UUID` | Primary Key |
| `username` | `VARCHAR(100)` | Target user login |
| `commandtext` | `VARCHAR(500)` | Display alias or custom label |
| `originalcommandtext` | `VARCHAR(500)`| Original unaliased CLI command |
| `favorder` | `INTEGER` | Display order index |
| `targeturl` | `VARCHAR(1000)` | Target iframe navigation URL |
| `createdon` | `TIMESTAMP` | Record creation timestamp |

---

## Connection Resilience & Caching

### Database Retry Resilience
> [!NOTE]
> Database repositories incorporate automated retry loops with exponential backoff. If an idle pooled TCP socket is reset by the database server (e.g. `Exception while reading from stream`), the repository automatically catches the transient error and retries with a fresh physical connection.

### Distributed Redis Caching
> [!TIP]
> Redis keys are generated deterministically using the format:
> `{appname}-command_config-`
> When `forceRefresh=true` is supplied, the cache key is evicted and repopulated directly from the database.

---

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Axpert runtime dependencies (`ARMCommon.dll`, `AxExtend.dll`) located at `../../ARMMICRO/AxPlugins/AxExtend`
- Redis server instance (optional, for distributed caching)

### Build & Run

1. **Restore and Build**:
   ```bash
   dotnet build AxiApi.csproj
   ```

2. **Run Locally**:
   ```bash
   dotnet run
   ```

3. **Verify Health**:
   Navigate to `http://localhost:5057/swagger` (or configured development port) to explore the OpenAPI / Swagger interactive documentation.

---

## Error Handling

AxiApi implements centralized exception handling via `GlobalExceptionHandler` and RFC 7807 Problem Details:

- `ArgumentException`: Thrown on invalid or missing request parameters; maps to HTTP `400 Bad Request`.
- `DatabaseException`: Thrown when SQL execution fails; maps to HTTP `400 Bad Request` with contextual query details logged on the server.
- `KeyNotFoundException`: Thrown on missing resource lookups; maps to HTTP `404 Not Found`.
- `RedisCacheConnectionException`: Handled internally with transparent database fallback.
