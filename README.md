## Fixed issue by Salman 

- Axi configure -Properties-run-The Previously configured data is not listed in  axpert properties page (The data is displayed initially, but upon checking again, it is not displayed.)
- After click on Axi Refresh button -> Axi - configure - properties, it is throwing ""Error: ADS definition 'axi_dummy' not found"".
- settings-Previously configured data is not displaying in settings page
- "When a user marks a particular command as Favorite, and immediately opens it from 
My Favorites, the data is not reflected immediately it is displaying new blank form but after user reload from myfavorites  existing data is displaying
Ex:When a job (e.g., test Jobs 2) is marked as favorite and opened from My Favorites, it opens a new blank form instead of the existing configured job  but after user reload from myfavorites  existing data is displaying"
- "When the user attempts to edit a data , the form loads as a new transaction instead of displaying the existing data
Edit smokefeb26_3 aglab001 with Shorttextfield test"
- "In the AXI Edit Command, selecting a numeric field and pressing the space key clears the selected value and triggers the error message: ""Please enter a valid numeric value."" This action also causes the command line to stop functioning.
Command Note: Edit ""Employee Master"" EMP-202500001 with ""Employee Age"" 
Schema: pgbase114
Screen Details: Employee Master" -> Workind fine Not Reproducible. 
- The Open command is not handling invalid or unsupported sources properly. When a user enters an invalid command after Open, the application throws a JavaScript alert instead of displaying a proper validation message.(EG:Open set)" -> Fixed
- - "In the AXI Edit Command, existing records fail to load and return the error: ""column ""axispop"" does not exist"".
Command Note: Edit ""Disable Test Form (Old model)"" field1 Hari 
Schema: pgbase114
Screen Details: Disable Test Form (Old model), Hide Test Form (Old model)"


## Issues to be checked 
- analyse command - It is opening the Analytics Page,but not navigating to the corresponding form -> The patch was not applied in QA Server. 
- "Command - Open AppVar, Configure (server,job,api)
Once Application Constants and Variable screen is opened, on click of Listview, application is getting hanged"
- News and announcements not listing existing records -> This will be taken up after today's release
"


- "In the AXI Edit Command, selecting the numeric field (Mobile No, Pin Code) throws the error: ""Error: 42703: column ""pincode"" does not exist."" This issue is isolated to these specific fields only.
Command Note: Edit ""Employee Master"" ""Mobile No"" 
Schema: pgbase114
Screen Details: Employee Master"-> mobile no is not a dc1 fiel

parent.LoadIframe("../AxpertPlugins/Axi/HTMLPages/Analytics.html?entity=slord&calendar=t&isDupTab=true-1770626614111&hdnbElapsTime=0")




