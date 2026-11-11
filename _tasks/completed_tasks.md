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


  axi_axi_getstructsdata_param1:View|param2:admin|param3:default|param4:tmmsg|param5:0|param6:F|param7:F|param8:msgid|param9:tmmsg1|param10:h16zbdhmfok3|page:1|size:100_ent_v2


  {
    "d": "{\"result\":{\"message\":\"success\",\"ADSNames\":\"axi_getstructsdata\",\"data\":[{\"adsname\":\"axi_getstructsdata\",\"data\":[{\"displaydata\":\"Message ID (msgid) [field]\",\"id\":\"0\",\"caption\":\"Message ID\",\"isfield\":\"t\",\"transrecordid\":\"0\"},{\"displaydata\":\"msg242\",\"id\":\"0\",\"caption\":\"msg242\",\"isfield\":\"f\",\"transrecordid\":\"1984550000019\"},{\"displaydata\":\"msg241\",\"id\":\"0\",\"caption\":\"msg241\",\"isfield\":\"f\",\"transrecordid\":\"1984440000009\"},{\"displaydata\":\"msg240\",\"id\":\"0\",\"caption\":\"msg240\",\"isfield\":\"f\",\"transrecordid\":\"1984330000020\"},{\"displaydata\":\"msg237\",\"id\":\"0\",\"caption\":\"msg237\",\"isfield\":\"f\",\"transrecordid\":\"1984010000021\"},{\"displaydata\":\"msg239\",\"id\":\"0\",\"caption\":\"msg239\",\"isfield\":\"f\",\"transrecordid\":\"1984220000009\"},{\"displaydata\":\"msg238\",\"id\":\"0\",\"caption\":\"msg238\",\"isfield\":\"f\",\"transrecordid\":\"1984110000009\"},{\"displaydata\":\"msg236\",\"id\":\"0\",\"caption\":\"msg236\",\"isfield\":\"f\",\"transrecordid\":\"1983990003002\"},{\"displaydata\":\"msg235\",\"id\":\"0\",\"caption\":\"msg235\",\"isfield\":\"f\",\"transrecordid\":\"1983880000001\"},{\"displaydata\":\"msg234\",\"id\":\"0\",\"caption\":\"msg234\",\"isfield\":\"f\",\"transrecordid\":\"1983770000001\"},{\"displaydata\":\"msg233\",\"id\":\"0\",\"caption\":\"msg233\",\"isfield\":\"f\",\"transrecordid\":\"1983660000001\"},{\"displaydata\":\"msg232\",\"id\":\"0\",\"caption\":\"msg232\",\"isfield\":\"f\",\"transrecordid\":\"1983550000001\"},{\"displaydata\":\"msg231\",\"id\":\"0\",\"caption\":\"msg231\",\"isfield\":\"f\",\"transrecordid\":\"1983440000003\"},{\"displaydata\":\"msg229\",\"id\":\"0\",\"caption\":\"msg229\",\"isfield\":\"f\",\"transrecordid\":\"1983220000001\"},{\"displaydata\":\"msg230\",\"id\":\"0\",\"caption\":\"msg230\",\"isfield\":\"f\",\"transrecordid\":\"1983330000001\"},{\"displaydata\":\"msg228\",\"id\":\"0\",\"caption\":\"msg228\",\"isfield\":\"f\",\"transrecordid\":\"1983110000003\"},{\"displaydata\":\"msg227\",\"id\":\"0\",\"caption\":\"msg227\",\"isfield\":\"f\",\"transrecordid\":\"1983010000000\"},{\"displaydata\":\"msg226\",\"id\":\"0\",\"caption\":\"msg226\",\"isfield\":\"f\",\"transrecordid\":\"1982990000000\"},{\"displaydata\":\"msg225\",\"id\":\"0\",\"caption\":\"msg225\",\"isfield\":\"f\",\"transrecordid\":\"1982880000001\"},{\"displaydata\":\"msg224\",\"id\":\"0\",\"caption\":\"msg224\",\"isfield\":\"f\",\"transrecordid\":\"1982770000001\"},{\"displaydata\":\"msg222\",\"id\":\"0\",\"caption\":\"msg222\",\"isfield\":\"f\",\"transrecordid\":\"1982550000001\"},{\"displaydata\":\"msg223\",\"id\":\"0\",\"caption\":\"msg223\",\"isfield\":\"f\",\"transrecordid\":\"1982660000001\"},{\"displaydata\":\"msg221\",\"id\":\"0\",\"caption\":\"msg221\",\"isfield\":\"f\",\"transrecordid\":\"1982440000000\"},{\"displaydata\":\"msg220\",\"id\":\"0\",\"caption\":\"msg220\",\"isfield\":\"f\",\"transrecordid\":\"1982330000000\"},{\"displaydata\":\"msg219\",\"id\":\"0\",\"caption\":\"msg219\",\"isfield\":\"f\",\"transrecordid\":\"1982220000000\"},{\"displaydata\":\"msg218\",\"id\":\"0\",\"caption\":\"msg218\",\"isfield\":\"f\",\"transrecordid\":\"1982110000000\"},{\"displaydata\":\"msg217\",\"id\":\"0\",\"caption\":\"msg217\",\"isfield\":\"f\",\"transrecordid\":\"1982010000001\"},{\"displaydata\":\"msg214\",\"id\":\"0\",\"caption\":\"msg214\",\"isfield\":\"f\",\"transrecordid\":\"1981770000001\"},{\"displaydata\":\"msg216\",\"id\":\"0\",\"caption\":\"msg216\",\"isfield\":\"f\",\"transrecordid\":\"1981990000001\"},{\"displaydata\":\"msg215\",\"id\":\"0\",\"caption\":\"msg215\",\"isfield\":\"f\",\"transrecordid\":\"1981880000000\"},{\"displaydata\":\"msg213\",\"id\":\"0\",\"caption\":\"msg213\",\"isfield\":\"f\",\"transrecordid\":\"1981660000002\"},{\"displaydata\":\"msg212\",\"id\":\"0\",\"caption\":\"msg212\",\"isfield\":\"f\",\"transrecordid\":\"1981550000001\"},{\"displaydata\":\"msg211\",\"id\":\"0\",\"caption\":\"msg211\",\"isfield\":\"f\",\"transrecordid\":\"1981440000001\"},{\"displaydata\":\"msg210\",\"id\":\"0\",\"caption\":\"msg210\",\"isfield\":\"f\",\"transrecordid\":\"1981330000000\"},{\"displaydata\":\"msg208\",\"id\":\"0\",\"caption\":\"msg208\",\"isfield\":\"f\",\"transrecordid\":\"1981110000001\"},{\"displaydata\":\"msg209\",\"id\":\"0\",\"caption\":\"msg209\",\"isfield\":\"f\",\"transrecordid\":\"1981220000001\"},{\"displaydata\":\"msg206\",\"id\":\"0\",\"caption\":\"msg206\",\"isfield\":\"f\",\"transrecordid\":\"1980990000001\"},{\"displaydata\":\"msg207\",\"id\":\"0\",\"caption\":\"msg207\",\"isfield\":\"f\",\"transrecordid\":\"1981010000002\"},{\"displaydata\":\"msg205\",\"id\":\"0\",\"caption\":\"msg205\",\"isfield\":\"f\",\"transrecordid\":\"1980880000001\"},{\"displaydata\":\"msg203\",\"id\":\"0\",\"caption\":\"msg203\",\"isfield\":\"f\",\"transrecordid\":\"1980660000000\"},{\"displaydata\":\"msg204\",\"id\":\"0\",\"caption\":\"msg204\",\"isfield\":\"f\",\"transrecordid\":\"1980770000001\"},{\"displaydata\":\"msg202\",\"id\":\"0\",\"caption\":\"msg202\",\"isfield\":\"f\",\"transrecordid\":\"1980550000000\"},{\"displaydata\":\"msg200\",\"id\":\"0\",\"caption\":\"msg200\",\"isfield\":\"f\",\"transrecordid\":\"1980330000001\"},{\"displaydata\":\"msg201\",\"id\":\"0\",\"caption\":\"msg201\",\"isfield\":\"f\",\"transrecordid\":\"1980440000000\"},{\"displaydata\":\"msg199\",\"id\":\"0\",\"caption\":\"msg199\",\"isfield\":\"f\",\"transrecordid\":\"1980220000002\"},{\"displaydata\":\"msg198\",\"id\":\"0\",\"caption\":\"msg198\",\"isfield\":\"f\",\"transrecordid\":\"1980110000000\"},{\"displaydata\":\"msg197\",\"id\":\"0\",\"caption\":\"msg197\",\"isfield\":\"f\",\"transrecordid\":\"1980010000000\"},{\"displaydata\":\"msg195\",\"id\":\"0\",\"caption\":\"msg195\",\"isfield\":\"f\",\"transrecordid\":\"1979880000000\"},{\"displaydata\":\"msg196\",\"id\":\"0\",\"caption\":\"msg196\",\"isfield\":\"f\",\"transrecordid\":\"1979990000004\"},{\"displaydata\":\"msg194\",\"id\":\"0\",\"caption\":\"msg194\",\"isfield\":\"f\",\"transrecordid\":\"1979770000000\"},{\"displaydata\":\"msg193\",\"id\":\"0\",\"caption\":\"msg193\",\"isfield\":\"f\",\"transrecordid\":\"1979660000000\"},{\"displaydata\":\"msg192\",\"id\":\"0\",\"caption\":\"msg192\",\"isfield\":\"f\",\"transrecordid\":\"1979550000000\"},{\"displaydata\":\"msg191\",\"id\":\"0\",\"caption\":\"msg191\",\"isfield\":\"f\",\"transrecordid\":\"1979440000002\"},{\"displaydata\":\"msg190\",\"id\":\"0\",\"caption\":\"msg190\",\"isfield\":\"f\",\"transrecordid\":\"1979330000000\"},{\"displaydata\":\"msg188\",\"id\":\"0\",\"caption\":\"msg188\",\"isfield\":\"f\",\"transrecordid\":\"1979110000000\"},{\"displaydata\":\"msg189\",\"id\":\"0\",\"caption\":\"msg189\",\"isfield\":\"f\",\"transrecordid\":\"1979220000000\"},{\"displaydata\":\"msg187\",\"id\":\"0\",\"caption\":\"msg187\",\"isfield\":\"f\",\"transrecordid\":\"1979010000000\"},{\"displaydata\":\"msg186\",\"id\":\"0\",\"caption\":\"msg186\",\"isfield\":\"f\",\"transrecordid\":\"1978990000000\"},{\"displaydata\":\"msg185\",\"id\":\"0\",\"caption\":\"msg185\",\"isfield\":\"f\",\"transrecordid\":\"1978880000000\"},{\"displaydata\":\"msg184\",\"id\":\"0\",\"caption\":\"msg184\",\"isfield\":\"f\",\"transrecordid\":\"1978770000000\"},{\"displaydata\":\"msg183\",\"id\":\"0\",\"caption\":\"msg183\",\"isfield\":\"f\",\"transrecordid\":\"1978660000000\"},{\"displaydata\":\"msg182\",\"id\":\"0\",\"caption\":\"msg182\",\"isfield\":\"f\",\"transrecordid\":\"1978550000000\"},{\"displaydata\":\"msg180\",\"id\":\"0\",\"caption\":\"msg180\",\"isfield\":\"f\",\"transrecordid\":\"1978330000000\"},{\"displaydata\":\"msg181\",\"id\":\"0\",\"caption\":\"msg181\",\"isfield\":\"f\",\"transrecordid\":\"1978440000000\"},{\"displaydata\":\"msg179\",\"id\":\"0\",\"caption\":\"msg179\",\"isfield\":\"f\",\"transrecordid\":\"1978220000000\"},{\"displaydata\":\"msg178\",\"id\":\"0\",\"caption\":\"msg178\",\"isfield\":\"f\",\"transrecordid\":\"1978110000000\"},{\"displaydata\":\"msg176\",\"id\":\"0\",\"caption\":\"msg176\",\"isfield\":\"f\",\"transrecordid\":\"1977990000001\"},{\"displaydata\":\"msg177\",\"id\":\"0\",\"caption\":\"msg177\",\"isfield\":\"f\",\"transrecordid\":\"1978010000001\"},{\"displaydata\":\"msg175\",\"id\":\"0\",\"caption\":\"msg175\",\"isfield\":\"f\",\"transrecordid\":\"1977880000001\"},{\"displaydata\":\"msg174\",\"id\":\"0\",\"caption\":\"msg174\",\"isfield\":\"f\",\"transrecordid\":\"1977770000001\"},{\"displaydata\":\"msg173\",\"id\":\"0\",\"caption\":\"msg173\",\"isfield\":\"f\",\"transrecordid\":\"1977660000001\"},{\"displaydata\":\"msg172\",\"id\":\"0\",\"caption\":\"msg172\",\"isfield\":\"f\",\"transrecordid\":\"1977550000001\"},{\"displaydata\":\"msg171\",\"id\":\"0\",\"caption\":\"msg171\",\"isfield\":\"f\",\"transrecordid\":\"1977440000001\"},{\"displaydata\":\"msg170\",\"id\":\"0\",\"caption\":\"msg170\",\"isfield\":\"f\",\"transrecordid\":\"1977330000001\"},{\"displaydata\":\"msg169\",\"id\":\"0\",\"caption\":\"msg169\",\"isfield\":\"f\",\"transrecordid\":\"1977220000001\"},{\"displaydata\":\"msg168\",\"id\":\"0\",\"caption\":\"msg168\",\"isfield\":\"f\",\"transrecordid\":\"1977110000000\"},{\"displaydata\":\"msg167\",\"id\":\"0\",\"caption\":\"msg167\",\"isfield\":\"f\",\"transrecordid\":\"1977010000000\"},{\"displaydata\":\"msg165\",\"id\":\"0\",\"caption\":\"msg165\",\"isfield\":\"f\",\"transrecordid\":\"1976880000001\"},{\"displaydata\":\"msg166\",\"id\":\"0\",\"caption\":\"msg166\",\"isfield\":\"f\",\"transrecordid\":\"1976990000000\"},{\"displaydata\":\"msg164\",\"id\":\"0\",\"caption\":\"msg164\",\"isfield\":\"f\",\"transrecordid\":\"1976770000001\"},{\"displaydata\":\"msg162\",\"id\":\"0\",\"caption\":\"msg162\",\"isfield\":\"f\",\"transrecordid\":\"1976550000000\"},{\"displaydata\":\"msg163\",\"id\":\"0\",\"caption\":\"msg163\",\"isfield\":\"f\",\"transrecordid\":\"1976660000002\"},{\"displaydata\":\"msg161\",\"id\":\"0\",\"caption\":\"msg161\",\"isfield\":\"f\",\"transrecordid\":\"1976440000000\"},{\"displaydata\":\"msg160\",\"id\":\"0\",\"caption\":\"msg160\",\"isfield\":\"f\",\"transrecordid\":\"1976330000001\"},{\"displaydata\":\"msg159\",\"id\":\"0\",\"caption\":\"msg159\",\"isfield\":\"f\",\"transrecordid\":\"1976220000001\"},{\"displaydata\":\"msg158\",\"id\":\"0\",\"caption\":\"msg158\",\"isfield\":\"f\",\"transrecordid\":\"1976110000000\"},{\"displaydata\":\"msg157\",\"id\":\"0\",\"caption\":\"msg157\",\"isfield\":\"f\",\"transrecordid\":\"1976010000000\"},{\"displaydata\":\"msg156\",\"id\":\"0\",\"caption\":\"msg156\",\"isfield\":\"f\",\"transrecordid\":\"1975990000002\"},{\"displaydata\":\"msg155\",\"id\":\"0\",\"caption\":\"msg155\",\"isfield\":\"f\",\"transrecordid\":\"1975880000003\"},{\"displaydata\":\"msg154\",\"id\":\"0\",\"caption\":\"msg154\",\"isfield\":\"f\",\"transrecordid\":\"1975770000003\"},{\"displaydata\":\"msg153\",\"id\":\"0\",\"caption\":\"msg153\",\"isfield\":\"f\",\"transrecordid\":\"1975660000001\"},{\"displaydata\":\"msg152\",\"id\":\"0\",\"caption\":\"msg152\",\"isfield\":\"f\",\"transrecordid\":\"1975550000000\"},{\"displaydata\":\"msg149\",\"id\":\"0\",\"caption\":\"msg149\",\"isfield\":\"f\",\"transrecordid\":\"1975220000001\"},{\"displaydata\":\"msg151\",\"id\":\"0\",\"caption\":\"msg151\",\"isfield\":\"f\",\"transrecordid\":\"1975440000000\"},{\"displaydata\":\"msg150\",\"id\":\"0\",\"caption\":\"msg150\",\"isfield\":\"f\",\"transrecordid\":\"1975330000000\"},{\"displaydata\":\"msg148\",\"id\":\"0\",\"caption\":\"msg148\",\"isfield\":\"f\",\"transrecordid\":\"1975110003301\"},{\"displaydata\":\"msg147\",\"id\":\"0\",\"caption\":\"msg147\",\"isfield\":\"f\",\"transrecordid\":\"1975010000000\"},{\"displaydata\":\"msg145\",\"id\":\"0\",\"caption\":\"msg145\",\"isfield\":\"f\",\"transrecordid\":\"1974880003302\"},{\"displaydata\":\"msg146\",\"id\":\"0\",\"caption\":\"msg146\",\"isfield\":\"f\",\"transrecordid\":\"1974990003301\"},{\"displaydata\":\"msg143\",\"id\":\"0\",\"caption\":\"msg143\",\"isfield\":\"f\",\"transrecordid\":\"1974660006001\"}]}],\"success\":true}}"
}

1. For Search 



11/11/2026
1. TSK-0926 - AxiCMDLine : Enhance Multilingual Support with User-Configurable Language Settings
2. Duplicate field shoud not be allowed set as keyfield.





