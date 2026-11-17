# AxiCMD v1.1.1 – Release Notes (16 Nov 2026)

## Bug Fixes

### 1. TKT-0945 – Configure KeyField suggestions show fields with duplicate values
- **Issue:** The Configure KeyField auto-suggestions list fields that contain duplicate values. 
- **Status**: Fixed

### 2. TKT-0782 – Edit command validation issue (#295)
- **Issue:** Validation issue in the Edit command.
- **Status:** Handled validation for number fields. 

### 3. Configure "Trace Files" accepts extra text (#305)
- **Issue:** After selecting `Configure "Trace Files"`, the user can type anything after it and run the command without any error.
- **Command used:** `Configure "Trace Files" GetMultiMenu.txt`
- **Status**: Fixed

### 4. Field names not aligned in the selection list
- **Issue:** Field names were displayed in the middle of the selection area instead of being aligned properly.
- **Test details:** Username/password: smokeuser1/agile · Schema: orclbase114 · Form: Import Encrypt
- **Status:** Fixed. Field names are now displayed at the start of the list.

### 5. Help text wrongly mentions "Source" for pages (#310)
- **Issue:** The "Source" command is not available for pages, but the Help section "Open Developer Studio" (Source tab) wrongly says: *"Use the source command to open a form, report, page or data source in Developer Studio."* (example: Monthly Sales)

### 6. Configure command issues (Axi Command Configuration page)
- **Issue 1:** A tstruct was configured from **Settings → Configure** and opened with the Configure command (`Configure Cust_tst`). The tstruct page opened, but the list view did not show the existing records.
  - **Status:** Not reproducible. This is not related to AxiCMD; please check with other tstructs.
- **Issue 2:** **Record Source** in the Advanced Configure settings does not list all the ADS.
  - **Status:** Fixed. The select component was replaced with an input component.
- **Issue 3:** The default commands under Configure and SDK can be activated or deactivated.
  - **Status:** Working as designed (this is a feature).
- **Issue 6:** There is no option to delete custom commands.
  - **Status:** Working as designed. Configure and SDK commands support only the Active and Inactive actions.

### 7. AxiCMD Configuration page does not open (random)
- **Issue (Issue 7):** Sometimes, clicking **Settings → Configure** does not open the "Axi Commandline Configuration" page in orclbase114.
- **Status:** Not reproducible.

### 8. User Activation not listed for the admin user
- **Issue:** User Activation is not listed for the `admin` username in orclbase114.
- **Issue:** Fixed

### 9. TKT-1641 – Blank tstruct page when opened again from its original place
- **Issue:** When a tstruct page (a tstruct, or a tstruct opened from the Configure or SDK commands) is opened from the command line and then opened again from its original place (pop-up, Configuration Studio, Axpert Developer Studio), a blank screen is shown. This happens wherever a tstruct page is available.
- **Status:** Malakonda Sir will release a patch containing `main-tstruct.js` and `main-tstruct.min.js`.

### 10. Configure screens are blank in pop-up mode
- **Issue:** When any screen under Configure is opened in normal mode and the same screen is then opened in pop-up mode, the pop-up page is blank. For listing screens, the issue occurs when the tstruct page is opened from the listing screen in normal mode and then in pop-up mode.
- **Status:** Malakonda Sir will release a patch containing `main-tstruct.js` and `main-tstruct.min.js`.

### 11. View → New is blank in pop-up mode
- **Issue:** Open any tstruct with the View command and click **New** in normal mode. Then open the same tstruct with the View command and click **New** in pop-up mode. The pop-up page is blank.
- **Status:** Malakonda Sir will release a patch containing `main-tstruct.js` and `main-tstruct.min.js`.
