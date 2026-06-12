# Axi Command Palette | Release Notes

This release brings stability improvements, database schema optimizations, security auditing, and documentation enhancements to the **Axi Command Palette** plugin (supporting both Oracle and PostgreSQL backends).

---

## 🚀 Key Highlights

*   **Oracle Database Support & Refactoring:** Restructured custom database functions and added schema auto-recompilation scripts for seamless setup in Oracle environments.
*   **Security hardening:** Audited console logs to remove plaintext password/session leaks.
*   **Command Palette Polish:** Enhanced loading animations, resolved spelling inconsistencies, and corrected token indexing bugs.
*   **Postgres Schema Correction:** Aligned prompt mappings in Postgres config studio parameters.

---

## 🛠 Detailed Changelog

### 1. Frontend & UI/UX Improvements
*   **Loading Animations:** Added loading spinners to the Favourites Save and Delete modal buttons to provide visual feedback during backend API calls.
*   **Favourites Splicing Fix:** Resolved a UI consistency issue by preventing local favorites array splicing when backend API responses fail or are missing.
*   **Autocomplete Token Alignment:** Fixed index mismatch bugs for `View` and `Configure` commands.
*   **Analyse Command Popup Fix:** Restrained the `analyse` command to standard frame execution (`window.LoadIframe`) and corrected autocomplete suggestions to offer the `Go [Ctrl + Enter]` option only upon entering the second token.
*   **Iview/Tstruct Conflict Resolution:** Fixed a collision bug occurring when a Tstruct and an Iview shared the same name.

### 2. Security Enhancements
*   **Credential Leak Prevention:** Removed a debugging `console.log(payload)` from `axicmdmain.js` that exposed plain-text login credentials and active session IDs to the browser console.

### 3. Database & Metadata Updates
*   **Oracle PL/SQL Restructuring (`axi_functions.sql`):**
    *   Refactored `axi_firesql_v2` to return `SYS_REFCURSOR` directly instead of a pipelined custom object table, simplifying query executions.
    *   Updated `fn_axi_getstructs_obj` to use a `FOR rec IN (...) LOOP` pattern instead of `SELECT INTO`, eliminating `NO_DATA_FOUND` exceptions.
    *   Added `ALTER FUNCTION COMPILE` and `DBMS_UTILITY.COMPILE_SCHEMA` blocks at the end of scripts to automatically compile and validate all invalid schema objects post-installation.
    *   Aligned the spelling of `'analyze'` to `'analyse'` in Oracle's `fn_axi_getstructures_meta` to match PostgreSQL and the Javascript command parser.
*   **Metadata Table Registrations (`axi_axdirectsql_tables.sql`):**
    *   Converted the Postgres query for ID `99999999990039` to an Oracle-compatible insert (using `TABLE(fn_axi_getstructs_obj(...))` and datetime formats) and registered it.
    *   Updated the `SQLSRC` column values from `'Internal'` to `'Metadata'` for multiple registrations (including `axi_useractivation`, `axi_userlist`, `axi_actorlist`, `axi_adscolumnlist`, and `axi_newsandannounce`) to ensure correct metadata classification.
*   **Postgres Schema Correction (`axi_command_tables.sql`):**
    *   Corrected the Configure prompt source for the object name from `axi_structlist` to `axi_structmetalist` and updated missing context variables in `extraparams`.

### 4. Code Cleanup & Deployment Documentation
*   **Mermaid Deployment Guide:** Enhanced the `README.md` guide with detailed flow diagrams, prerequisite badges, and specific instructions to copy the `AxiApi_Beta` folder to the target `Arm microservices` server directory.
*   **Codebase Cleanup:** Removed unused/backup `.zip` files from the plugin folders.

### 5. Bug Fixes
*   Existing and newly created Dimensions are available in Configure "Application Properties" instead of Configure Dimension cmd: Configure "Application Properties" Branch -> Fixed #76 
*   When clicked on the options available in Configure Publish Config Studio,getting error: Access violation at address 0000000000DED5D6 in module 'ASBTStruct.dll'. Read of address 0000000000000000 -> Fixed #74 
*   When clicked on the options available in Configure Actor,getting error:Access violation at address 0000000000DED5D6 in module 'ASBTStruct.dll'. Read of address 0000000000000000 -> Fixed #73 
*   Existing and newly created Cards are available in Configure Responsibility instead of Configure Card cmd: Configure Responsibility KPI Cards -> Fixed #75 
*   Issue #74 - Existing and newly created Cards are available in Configure Responsibility instead of Configure Card cmd: Configure Responsibility KPI Cards -> Fixed 
*   Issue #71 Iview and tstruct with same name -> Fixed 

---

## 📋 QA Validation & Plugin Rollout Guidelines

### 1. Cross-Database QA Verification (Axi_Beta)
*   **Testing Requirement:** This release **must** be thoroughly tested and verified in both **PostgreSQL** and **Oracle** environments.
*   **Approval Gate:** Under no circumstances should this release be applied to `agile-axi`, `alpha-axi`, or any other shared/production/shared instances without complete verification and explicit QA sign-off.

### 2. Plugin Naming & Transition Strategy
*   **Current State:** The plugin is packaged and named as **Axi_Beta**. During this phase, it must be treated strictly as a testing and verification plugin.
*   **Production Rollout:** Once all validations, testing, and environment verifications are completed successfully, the existing **Axi** plugin will be replaced and overwritten by **Axi_Beta** for production use.

---

## ⚠️ Manual Intervention Required


### 1. ERPGoldDemo Environment Setup
When applying this release to the **erpgolddemo** environment, the following database adjustments must be performed manually:
*   **Delete & Recreate ADS:** The specific ADS (`axi_adscolumnlist`) needs to be manually deleted and recreated using the query below.
*   **SmartView Table Compatibility:** Since the upcoming SmartView database changes are already implemented in the `erpgolddemo` schema, the **ADS Column List** is expected to operate and fetch data using these new SmartView tables.

#### Reconstitution SQL Scripts for `erpgolddemo`:


```sql
-- Step 1: Delete the existing ADS configuration
DELETE FROM axdirectsql WHERE sqlname = 'axi_adscolumnlist';

-- Step 2: Recreate the ADS configuration with SmartView integration
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990028, 'F', 0, NULL, 'admin', '2026-01-30 00:00:00.000', 'admin', '2026-01-30 00:00:00.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_adscolumnlist', NULL, 'Metadata', 0, 'select  displaydata,name,caption,normalized,fdatatype,sourcetable,sourcefld,filters  
from fn_smartview_metadata(:param1)', 'param1', 'param1~Character~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL);
```

### 2. Other Environments
*   **Axi CMD Consistency:** The upcoming SmartView modifications are not yet officially released across all other environments. Therefore, in all environments other than `erpgolddemo`, **Axi CMD** must continue using the currently available tables.
