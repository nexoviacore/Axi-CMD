1. 	TKT-0841 - AXI CMD line-In Help cmd,Source option needs to be mentioned in interactive suggestions. Customer - Agile Labs Pvt Ltd Completed. 

2. TKT-1389 - ! Enhance Axi Command to support dynamic autocomplete for large datasets, Customer - BDF -> Status: Completed

3. TKT-1387 - ! The data is not being filtered correctly. On the Entity/Listing page, the records are displayed according to the applied filter.In Axi Command, all the data is visible instead of showing only the filtered records. Customer - BDF

4. TSK-0896 - Show "User Activation" under the Configure command based on axusers.licactivation: a user, admin included, sees it only when their licactivation is true (true / T, any case). A missing row, null or a failed lookup hides it. New ADS axi_userlicactivation (Postgres and Oracle) is read through loadList before the commands are built.
Issue #288

5. Issue1: Cost Centre Mapping View "Rent Expenses",getting "Error: 42703: column p.rent expenses does not exist" on click of "Rent Expenses"

6. Issue2: Cost Centre Mapping Edit "Rent Expenses",getting "Error: 42703: column p.rent expenses does not exist" on click of "Rent Expens

7. The entire command is not visible in the cmd line.'Ready to run' label is overlapped with the command.-Arabic language login #266

8. 	In cmd line,"clear all" button is overlapped-Arabic language login

9. Setting trace on or off and viewing the files should be available through configure command.
Issue #30  showTraceFileDialog(), 

10. Make axi_command_config configurable by the user: new AxiCMDConfig.html page (opened from the AxiCMD gear menu "Configure", only for users with Configure Access) to turn Built-in Options of the configure and sdk commands on/off and add, edit or deactivate Custom Options, through new AxiApi command-config endpoints. Custom Options survive re-installs (axi_command_prompts is no longer dropped). New configure/sdk options are added only through this page, not SQL.
Issue #281 (tickets #282 - #287)

11. Two tstructs with the same caption but different transids (e.g. Test Form2 (ytdf) and Test Form2 (tst2)): "Test Form2" view opened the wrong form. The palette now keeps the form picked from the suggestions, keeps "(transid)" in the input when another structure of the same type shares the caption ("Test Form2 (tst2)" view), recognises "Caption (transid)" when typed, and blocks a shared caption typed by hand with a toast listing the choices.
Issue #289



12. New "install" command: install product / plugin / changerequest <directory> [version]. The GitHub account and repo of each type are saved with "configure product|plugin|changerequest <account> <repo>" through the new AxiApi CommandSettings API (axi_command_settings table, Postgres and Oracle); configure checks the repo through AxiGit listdirectories first and blocks a repo or account GitHub does not know. Install suggests the directories of the configured repo (AxiGit POST /api/git/listdirectories under the ARM url) and then their versions ("Latest" by default), and on run sends the install request to the ARM PushToQueue API (queue axiinstallpackagesqueue), with a progress modal in the palette.
PR #290 (branch feat/implement-install-command)



