# 02 — Execution Routing for Developer Studio & Form Builders

**What to build:**
Selecting or pressing Enter on "Create New" launches Developer Studio builders or configuration forms in creation mode:
- `sdk tstruct "Create New"` -> `window.openDeveloperStudio("tstreact", "", true)`
- `sdk iview "Create New"` -> `window.openDeveloperStudio("ivreact", "", true)`
- `sdk page "Create New"` -> `window.LoadIframe("../aspx/tstruct.aspx?transid=sect")`
- `sdk axpert data sources "Create New"` -> `window.LoadIframe("../aspx/tstruct.aspx?transid=b_sql")`

**Blocked by:** #170

**Status:** ready-for-agent

- [ ] Execute `openDeveloperStudio("tstreact", "", true)` when "Create New" is selected for `sdk tstruct`.
- [ ] Execute `openDeveloperStudio("ivreact", "", true)` when "Create New" is selected for `sdk iview`.
- [ ] Load `../aspx/tstruct.aspx?transid=sect` when "Create New" is selected for `sdk page`.
- [ ] Load `../aspx/tstruct.aspx?transid=b_sql` when "Create New" is selected for `sdk ads`.
