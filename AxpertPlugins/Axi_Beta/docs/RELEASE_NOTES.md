# Release Notes: Axi Command Palette (`Axi_Beta`)

**Release Version:** `v0.4.0-rc`  
**Release Date:** August 25, 2026  
**Target Runtime:** .NET 8.0 / Axpert Web 11.4+  
**Database Engines:** PostgreSQL 13+ / Oracle 19c+  

---

## 🚀 Highlights & Executive Summary

The **`v0.4.0`** release of the **Axi Command Palette** introduces a database-driven dynamic command architecture (`axi_command_config`), replaces hardcoded routing tables with generic redirection handlers.


---

## ⚠️ Breaking Changes
---

## 🌟 Key New Features & Enhancements

### 1. Dynamic Command Configuration Engine (`axi_command_config`)

* TSK-0520 - Introduce axi_command_config for Dynamic Command Line Navigation 
- **Metadata-Driven Navigation:** Added `axi_command_config` table and endpoint (`/api/v1/command-config/getcommandconfig`) to eliminate hardcoded JavaScript URL mappings.
- **Dynamic URL Placeholder Resolution:** Supports contextual URL token interpolation for `:username`, `:userroles`, `:userresp`, `:appname`, and dynamic parameters (`:param`, `:paramField`).
- **Flexible Navigation Types:** Handlers support `tstruct`, `iview`, `tstruct/iview`, `ivtoivload`, `processflow`, `url`, `url/tstruct`, and `action`.
---

## 🛠 Bug Fixes
* TKT-1046 - AXI CMD Line-Selected Axpert Developer Options are not reflected in Developer Studio,AXI CMD Line for schemas with AXI CMD Line


---

