1. 	TKT-0841 - AXI CMD line-In Help cmd,Source option needs to be mentioned in interactive suggestions. Customer - Agile Labs Pvt Ltd Completed. 

2. TKT-1389 - ! Enhance Axi Command to support dynamic autocomplete for large datasets, Customer - BDF -> Status: Completed

3. TKT-1387 - ! The data is not being filtered correctly. On the Entity/Listing page, the records are displayed according to the applied filter.In Axi Command, all the data is visible instead of showing only the filtered records. Customer - BDF

param10 holds the session variables, used by the form’s view filter (<transid>_filter, e.g. and Form11.branch=:branch). Without it in the key, a user on branch=Chennai could get the cached rows of branch=Mumbai.

4. TSK-0896 - Show "User Activation" under the Configure command based on axusers.licactivation: a user, admin included, sees it only when their licactivation is true (true / T, any case). A missing row, null or a failed lookup hides it. New ADS axi_userlicactivation (Postgres and Oracle) is read through loadList before the commands are built.
Issue #288

5. TSK-0921 - Axi Command Line – Install Command Implementation

Implement the Install command in Axi Command Line to support installing Products, Plugins, and Change Requests. Ensure proper version handling, installation flow, and integration with the existing AxInstaller REST API and Console components.

Validate the complete installation workflow, including dependency handling, data object processing, and error handling, while maintaining compatibility with the existing Axi Command Line architecture.


6. TKT-1301 
Issue1: Cost Centre Mapping View "Rent Expenses",getting "Error: 42703: column p.rent expenses does not exist" on click of "Rent Expenses"  Issue2: Cost Centre Mapping Edit "Rent Expenses",getting "Error: 42703: column p.rent expenses does not exist" on click of "Rent Expens

7. The entire command is not visible in the cmd line.'Ready to run' label is overlapped with the command.-Arabic language login #266  
Backlog: Records should be in Arabic 

8. 	In cmd line,"clear all" button is overlapped-Arabic language login

9. Setting trace on or off and viewing the files should be available through configure command.
Issue #30  showTraceFileDialog(), 

10. Make axi_command_config configurable by the user: new AxiCMDConfig.html page (opened from the AxiCMD gear menu "Configure", only for users with Configure Access) to turn Built-in Options of the configure and sdk commands on/off and add, edit or deactivate Custom Options, through new AxiApi command-config endpoints. Custom Options survive re-installs (axi_command_prompts is no longer dropped). New configure/sdk options are added only through this page, not SQL.
Issue #281 (tickets #282 - #287)




Release Notes: 
1. Install command implementation
2. AxiCmdConfiguration Page
3. Pagination support Axi cmd
4. Axi CMD Arabic compatiblity. 

Note to QA 
1. There will be a Product release for Settings button and header css changes


discussion points
1. In Axicmdconfig.html rename the Title to Axi Commandline Configuration. 




Fieldname issuee.


axi_axi_getstructsdata_param1:Edit|param2:salmansheriff9944|param3:default|param4:tmmsg|param5:0|param6:F|param7:F|param8:msgid|param9:tmmsg1|param10:h28kd0nq2sua|page:1|size:100_ent_v2  -> Edit

axi_axi_getstructsdata_param1:View|param2:salmansheriff9944|param3:default|param4:tmmsg|param5:0|param6:F|param7:F|param8:msgid|param9:tmmsg1|param10:h28kd0nq2sua|page:1|size:100_ent_v2 -> View

axi_axi_structmetalist_param1:salmansheriff9944|param2:default|param3:default|param4:all|param5:all|page:1|size:100_v2 -> All Records (tstruct,iview,ads, pages)




axi_axi_structmetalist_param1:salmansheriff9944|param2:default|param3:default|param4:dev|param5:i|page:1|size:100_v2 -> Sdk iview ()

axi_axi_structmetalist_param1:salmansheriff9944|param2:default|param3:default|param4:dev|param5:t|page:1|size:100_v2 -> sdk tstruct ()

axi_axi_structmetalist_param1:salmansheriff9944|param2:default|param3:default|param4:run|param5:t|page:1|size:100_v2 -> run 



 1. const MAX_CACHED_SEARCH_KEYS = 20; ->
When there are more than 20, the oldest entry is deleted, along with its paging state (axPageState).
 2. const CACHE_KEY_MAX_VALUE_CHARS = 40; 
 40 characters or fewer (and no @@VARS@@): the value goes into the key unchanged, e.g. param4:tmmsg|.
Longer, or containing @@VARS@@: the value becomes h plus about 11 characters of hash.

  3. purgeLongListCacheKeys - This function may not be needed. 
  function remove

  4. Lookpage size should be configure  const LOOKUP_PAGE_SIZE = 50;

  const LOOKUP_PAGE_SIZE = getPageSize()
  5. Fields names were not listing. 




