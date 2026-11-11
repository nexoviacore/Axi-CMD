# 0001. Large Dataset Autocomplete for Axi Command

We decided to support tables with 50,000+ records in Axi Command by introducing server-side pagination and dynamic search filtering directly into the underlying database stored function `fn_axi_getstructs_obj` and ADS `axi_getstructsdata`, combined with a 300ms debounced client-side query manager in `axicmdmain.js`.

## Context
Axi Command previously fetched all records for a selected Tstruct at once on `edit <transid>`. For large tables (> 50,000 records), this resulted in excessive network payload, high database strain, and browser UI freezes.

## Decision
1. **Database Function Extension**: Extend `fn_axi_getstructs_obj` in both PostgreSQL and Oracle with 3 optional parameters (`psearchterm DEFAULT ''`, `ppageno DEFAULT 1`, `ppagesize DEFAULT 100`) and update `axdirectsql` to map `:param11`, `:param12`, `:param13`.
2. **Initial Load**: Fetch an initial page of 100 recent records on `edit <tstruct>`.
3. **Dynamic Search**: When typing 3 or more characters, buffer keystrokes with a 300ms debounce before dispatching an ADS request to the server with SQL-level filtering (`LIKE %term%`).
4. **Adaptive Fallback**: `axicmdmain.js` incorporates client-side safety slicing and filtering to remain 100% functional even when deployed against unmigrated 10-parameter database environments.
5. **On-Demand Token Resolution**: Record identifiers typed or pasted directly without autocomplete selection are queried on-demand from the server before executing command redirections.
