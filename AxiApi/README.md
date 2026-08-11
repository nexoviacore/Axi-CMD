# AxiApi

Backend service layer powering the **AXI (Axpert Command Line Interface)** engine. AxiApi provides dynamic command resolution, command grammar bootstrapping, customizable user favorites, multi-tenant database connectivity, and distributed Redis caching for the Axpert web shell.

---

## Architecture Overview

AxiApi is built on **ASP.NET Core (.NET 8)** adhering to a clean, decoupled layered architecture:

```
┌────────────────────────────────────────────────────────┐
│               AxiController (API Endpoints)            │
└───────────────────────────┬────────────────────────────┘
                            │
┌───────────────────────────▼────────────────────────────┐
│      Service Layer (Business Logic & Redis Caching)    │
│  - CommandConfigService       - UserFavouritesService  │
│  - GrammarBootstrapService    - GrammarService         │
└───────────────────────────┬────────────────────────────┘
                            │
┌───────────────────────────▼────────────────────────────┐
│    Repository Layer (Data Access via AxExtend & SQL)   │
│  - CommandConfigRepository    - UserFavouritesRepo     │
│  - GrammarRepository          - SqlQueries             │
└───────────────────────────┬────────────────────────────┘
                            │
┌───────────────────────────▼────────────────────────────┐
│       Database & Cache (PostgreSQL / Oracle / Redis)   │
└────────────────────────────────────────────────────────┘
```

### Key Capabilities

- **Dynamic Command Configuration**: Eliminates hardcoded transaction routing by querying and caching the `axi_command_config` table for `Configure`, `SDK`, `Upload`, and `Download` actions.
- **Grammar Metadata Bootstrapping**: Generates and parses command trees, prompt sources, parameter positions, and autocompletion tokens.
- **User Favorites Management**: Supports creating, listing, renaming, and removing custom command aliases with deep link target URLs.
- **Multi-Tenant Connection Pooling**: Utilizes `AxExtend` and `ARMCommon` for dynamic database connection resolution across distinct Axpert application instances (`appname`).
- **Resilient Redis Caching**: Automatic fallback to direct database queries if the distributed cache is unavailable, with force-refresh cache invalidation.

---

## API Reference

All endpoints are versioned under the `/api/v1/Axi` route prefix.

### 1. Dynamic Command Configuration

Retrieves the active mapping of command verbs, prompt options, target transaction IDs, and navigation URLs.

```http
GET /api/v1/Axi/command-config?appname={appname}&forceRefresh={forceRefresh}
```

#### Query Parameters

| Parameter | Type | Required | Default | Description |
| :--- | :--- | :--- | :--- | :--- |
| `appname` | string | Yes | — | Axpert application database identifier |
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

### 2. Command Grammar Metadata

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

### 3. User Favorites

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
| `prompt_option_type`| `VARCHAR(20)` | Not Null | Navigation mode (`tstruct`, `iview`, `ivtoivload`, `url`) |
| `param_field` | `VARCHAR(100)` | Nullable | Parameter field name used for record loading |
| `target_url` | `VARCHAR(500)` | Nullable | Explicit redirection URL override |
| `extra_params` | `VARCHAR(500)` | Nullable | Additional query parameters (e.g. `AxOpenAct=true&isDupTab=false`) |
| `active` | `VARCHAR(1)` | Default `'T'` | Status flag (`T` / `F`) |

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

## Distributed Caching Strategy

AxiApi utilizes distributed Redis caching for high-throughput command resolution:

> [!TIP]
> Redis keys are generated deterministically using the format:
> `{appname}-command_config-`
> When `forceRefresh=true` is supplied, the cache key is deleted via `KeysDeleteAsync` and repopulated directly from the database.

If the Redis connection fails, the service gracefully degrades to querying the underlying database directly without interrupting end-user navigation.

---

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Axpert runtime dependencies (`ARMCommon.dll`, `AxExtend.dll`) located at `../../ARMMICRO/AxPlugins/AxExtend`
- Redis server instance (optional, for caching)

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

- `DatabaseException`: Thrown when SQL execution fails; maps to HTTP `500 Internal Server Error` with contextual query details in server logs.
- `NotFoundException`: Thrown on missing resource lookups; maps to HTTP `404 Not Found`.
- `RedisCacheConnectionException`: Handled internally with database fallback logging.
