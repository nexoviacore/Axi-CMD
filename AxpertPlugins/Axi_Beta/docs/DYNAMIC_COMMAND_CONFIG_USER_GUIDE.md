# User & Administrator Guide: Dynamic Command Configuration Engine (`axi_command_config`)

**Document Version:** `1.0.0`  
**Target Audience:** Axpert Developers, System Administrators, Technical QA Engineers  
**Applies To:** Axi Command Palette (`Axi_Beta`) v0.4.0+ / Axpert Web 11.4+  

---

## 📖 1. Overview & Purpose

The **Dynamic Command Configuration Engine** is a metadata-driven navigation architecture in the Axi Command Palette. Historically, adding or modifying command navigation routes required editing JavaScript source code (`axicmdmain.js`). 

With the introduction of the `axi_command_config` table and REST API, **100% of command navigation is managed directly in the database**. System administrators and developers can register new commands, link them to forms, reports, or custom pages, and configure contextual parameters **without changing a single line of JavaScript**.


---

## 🗄 2. Database Schema Reference (`axi_command_config`)

The table `axi_command_config` is stored in the application database (PostgreSQL or Oracle).

### Column Specifications

| Column Name | Data Type | Nullable | Description & Purpose |
| :--- | :--- | :---: | :--- |
| **`config_id`** | `VARCHAR(50)` | No | Primary Key. Unique identifier for the configuration rule (e.g. `cfg_configure_user`). |
| **`command`** | `VARCHAR(50)` | No | The root command verb (e.g. `Configure`, `SDK`, `Upload`, `Download`, `View`, `Create`, `Edit`). |
| **`prompt_options`** | `VARCHAR(200)` | No | The specific prompt option value to match (e.g. `user`, `role listing`, `app`, `default`). Case-insensitive. |
| **`prompt_id`** | `VARCHAR(50)` | No | Target identifier: `TransID` for Tstructs, `IViewName` for Iviews, or filename for custom pages. |
| **`prompt_option_type`** | `VARCHAR(20)` | No | Execution mode determining how the client shell handles the navigation. |
| **`param_field`** | `VARCHAR(100)` | Yes | Query parameter key to bind the user's input parameter to (e.g. `name`, `type`, `actorname`). |
| **`target_url`** | `VARCHAR(500)` | Yes | Explicit target URL template override. If omitted, constructed dynamically based on `prompt_option_type`. |
| **`extra_params`** | `VARCHAR(500)` | Yes | Additional query string parameters or dynamic tokens to append (e.g. `&status=true&user=:username`). |
| **`active`** | `VARCHAR(1)` | Yes | Status flag (`'T'` for Active, `'F'` for Disabled). Default: `'T'`. |

---

## ⚙️ 3. Supported Navigation Modes (`prompt_option_type`)

The engine provides specialized execution handlers tailored to Axpert structure types:

### 1. `tstruct` (Transaction Forms)
- **Use Case:** Opens an Axpert Tstruct form for creating a new record or editing an existing record.
- **Behavior:** If user provides a search value and `param_field` is specified, it opens in edit mode (`hltype=load`); otherwise opens in create mode (`hltype=open`).
- **Example:**
  ```sql
  INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, active)
  VALUES ('cfg_configure_user', 'Configure', 'user', 'axusr', 'tstruct', 'uname', 'T');
  ```

### 2. `iview` (Interactive Views / Reports)
- **Use Case:** Opens an Axpert interactive view report.
- **Behavior:** Routes directly to `iview.aspx?ivname=<prompt_id>`.
- **Example:**
  ```sql
  INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, active)
  VALUES ('cfg_configure_users', 'Configure', 'user listing', 'axusers', 'iview', 'T');
  ```

### 3. `tstruct/iview` or `iview/tstruct` (Dual-Mode Conditional Routing)
- **Use Case:** Single command that opens a listing report by default, but directly opens a specific record form if an entity name/ID is supplied.
- **Syntax for `prompt_id`:** `<TstructID>/<IviewName>` (e.g. `ad_ur/ad___url`).
- **Behavior:** 
  - If user types `Configure Role` $\rightarrow$ Opens IView report `ad___url`.
  - If user types `Configure Role "Admin"` $\rightarrow$ Opens Tstruct form `ad_ur` with record `Admin`.
- **Example:**
  ```sql
  INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, active)
  VALUES ('cfg_role_dual', 'Configure', 'role', 'ad_ur/ad___url', 'tstruct/iview', 'rname', 'T');
  ```

### 4. `url` (Standard Custom ASPX / HTML Pages)
- **Use Case:** Redirects to custom Axpert web pages, tools, or configuration dashboards.
- **Behavior:** Resolves relative URLs to `../aspx/<prompt_id>` or uses explicit `target_url`.
- **Example:**
  ```sql
  INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, active)
  VALUES ('cfg_config_app', 'Configure', 'app', 'appconfig.aspx', 'url', NULL, '../aspx/appconfig.aspx', 'T');
  ```

### 5. `processflow` or `url/tstruct` (Process Builder Flow)
- **Use Case:** Opens visual workflow builder diagrams.
- **Behavior:** Routes to `processflow.aspx?loadcaption=AxProcessBuilder&processname=<paramValue>`. If no parameter is passed, falls back to `ad_pm` Tstruct.
- **Example:**
  ```sql
  INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, active)
  VALUES ('cfg_configure_peg', 'Configure', 'peg', 'ad_pm', 'processflow', 'processname', '../aspx/processflow.aspx?loadcaption=AxProcessBuilder', 'T');
  ```

### 6. `ivtoivload` (IView-to-IView Parameterized Navigation)
- **Use Case:** Navigates to parameterized summary Iviews with preset filter bindings.
- **Example:**
  ```sql
  INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active)
  VALUES ('cfg_configure_dim_list', 'Configure', 'dimension listing', 'ad___upg', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=ad___upg', 'AxOpenAct=true&isDupTab=false', 'T');
  ```

---

## 🔤 4. Dynamic URL Placeholders

You can include contextual placeholders in `target_url` and `extra_params`. The client engine resolves these dynamically from the logged-in session before launching the target:

| Placeholder Token | Resolved Runtime Value | Description |
| :--- | :--- | :--- |
| **`:username`** | `window.mainUserName` | The login username of the current user. |
| **`:userroles`** | `window.AxUserRoles` | Comma-separated list of active user roles. |
| **`:userresp`** | `window.userResp` | Active responsibility of the logged-in user. |
| **`:appname`** | `appname` / `window.mainProject` | Current application project schema name. |
| **`:param`** | `paramValue` | The parameter value typed or selected by the user. |
| **`:paramField`** | `paramField` | The field name configured for the target. |

### Placeholder Usage Example
```sql
target_url: "../aspx/customdashboard.aspx?user=:username&role=:userroles&proj=:appname"
```

---

## 🛠 5. Step-by-Step Tutorial: Configuring a New Command

### Scenario
You want to add a new command: **`Configure SLA "Gold Policy"`** that opens custom page `SlaConfig.aspx` with the selected policy name and current user's role.



### Step 1: Register Autocomplete Prompt 
If your new option is under **Configure** (`cmdtoken = 4`) or **SDK** (`cmdtoken = 7`), ensure it is registered in `axi_command_prompts` so users receive auto-complete suggestions:

#### Option A: For `Configure` Commands (`cmdtoken = 4`)
```sql
-- 1. Append sub-option name to Position 2 (Object Type prompt values)
UPDATE axi_command_prompts 
SET promptvalues = promptvalues || ',SLA'
WHERE cmdtoken = 4 AND wordpos = 2;

-- 2. Append matching data source to Position 3 (Object Name data source)
-- Use 'Axi_Dummy' if the sub-option is static (no 3rd token suggestion required)
UPDATE axi_command_prompts
SET promptsource = promptsource || ',Axi_Dummy'
WHERE cmdtoken = 4 AND wordpos = 3;

-- OR if dynamic: specify your custom Axpert Data Source (e.g., ',axi_custom_ds')
-- UPDATE axi_command_prompts SET promptsource = promptsource || ',axi_custom_ds' WHERE cmdtoken = 4 AND wordpos = 3;
```

#### Option B: For `SDK` Commands (`cmdtoken = 7`)
```sql
-- 1. Append sub-option name to Position 2 (SDK Type prompt values)
UPDATE axi_command_prompts 
SET promptvalues = promptvalues || ',My Custom Tool'
WHERE cmdtoken = 7 AND wordpos = 2;

-- 2. Append matching data source to Position 3 (SDK Name data source)
-- Use 'Axi_Dummy' if static
UPDATE axi_command_prompts
SET promptsource = promptsource || ',Axi_Dummy'
WHERE cmdtoken = 7 AND wordpos = 3;

-- OR if dynamic: specify your custom Axpert Data Source (e.g., ',axi_custom_ds')
-- UPDATE axi_command_prompts SET promptsource = promptsource || ',axi_custom_ds' WHERE cmdtoken = 7 AND wordpos = 3;
```

> [!IMPORTANT]
> **Rules for using `param_field` (ADS Requirement)**:
> - **If you are using `param_field`**: You should first configure an Axpert Data Source (ADS) in `axdirectsql` (for example, `mycustomds`). Then, you need to add `mycustomds` to `promptsource` like this:
>   ```sql
>   UPDATE axi_command_prompts
>   SET promptsource = promptsource || ',mycustomds'
>   WHERE cmdtoken = 4 AND wordpos = 3;
>   ```
> - **If you are not using an ADS**: You should **not** use `param_field` (leave `param_field` as `NULL`).
> 

### Step 2: Insert the Configuration Row
Execute the following SQL statement in your database:

```sql
INSERT INTO axi_command_config (
    config_id,
    command,
    prompt_options,
    prompt_id,
    prompt_option_type,
    param_field,
    target_url,
    extra_params,
    active
) VALUES (
    'cfg_configure_sla',
    'Configure',
    'sla',
    'SlaConfig.aspx',
    'url',
    'policyname',
    '../aspx/SlaConfig.aspx',
    'role=:userroles&user=:username',
    'T'
); 
```

### Step 3: Test in the Command Palette
1. Open Axpert Web Shell and press `Ctrl + Space` to activate Axi Command Palette.
2. Type: `Configure SLA "Gold Policy"`
3. Press `Enter` or click **Go**.
4. The system executes:
   ```
   ../aspx/SlaConfig.aspx?policyname=Gold%20Policy&role=Developer&user=admin
   ```
5. Click **Pop** to test tabbed execution in `PopupContainer.html`.

---

## 🔍 6. Troubleshooting & FAQs

### Q1: I added a row in `axi_command_config`, but the command palette says "Unknown command".
- **Check Cache:** The configuration is loaded when the shell initializes. Refresh the browser (`Ctrl + F5`) or click the **Refresh** button on the Axi toolbar.
- **Check Matching:** Verify that `command` matches the root verb exactly (e.g. `Configure`) and `prompt_options` matches the typed sub-verb in lowercase.

### Q2: How do I create a fallback route for an entire command group?
- Set `prompt_options` to `'default'`. If no exact sub-option matches, the engine falls back to this rule:
  ```sql
  INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, active)
  VALUES ('cfg_default_sdk', 'SDK', 'default', 'devexplorer', 'url', 'T');
  ```

### Q3: My custom page opens in the main window, but I want it to open in a popup tab.
- Users can press `Ctrl + Shift + Enter` or select the **Pop** action button. The engine automatically appends `&AxIsPop=true` and loads the page within `PopupContainer.html`.

### Q4: Can I temporarily disable a command without deleting the row?
- Yes. Update the status flag: `UPDATE axi_command_config SET active = 'F' WHERE config_id = 'cfg_configure_sla';`.

### Q5: Why do I get "Access violation in ASBTStruct.dll" when passing a search value in TStruct / Dual Mode navigation?
- **Behavior of Dual-Mode Navigation (`tstruct/iview` or `iview/tstruct`)**:
  - **No parameter supplied** (e.g. `Configure SM56C`): Directly opens the **IView** listing.
  - **Parameter supplied** (e.g. `Configure SM56C "RecordName"`): Directly opens the **TStruct** form in edit mode using `param_field`.
- **Root Cause & Resolution**:
  - When passing a parameter to edit a TStruct, `param_field` must receive a valid, existing record key.
  - If `param_field` is configured without defining an ADS in `axdirectsql` (or if a non-existent key is typed), the backend TStruct loader (`ASBTStruct.dll`) cannot locate the record in the database, resulting in an access violation.
  - **Solution**: Always configure an Axpert Data Source (ADS) in `axdirectsql` for Position 3 so users select verified record keys from the auto-complete dropdown before opening the TStruct.
