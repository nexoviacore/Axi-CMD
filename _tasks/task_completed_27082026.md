1. Open Application Properties from cmd line using Configure cmd.Observed it is opening "Process Master page" instead of Application Properties page. Fixed 
2. The same command created in SDK and Configure.But in runtime only for Configure the option is reflecting,for SDK it is not reflecting.
Cmd name:SM56 in SDK and SM56C in Configure. Fixed 
3. For Tstruct Navigation/dual mode navigation,when Search value with param_field is given,in runtime it is giving error "Access violation at address 0000000000DED346 in module 'ASBTStruct.dll'. Read of address 0000000000000000"
Cmd name: SM56C or dualmode in Configure. Not an Issue - configuration issue
4. For URL/Page Navigation,whatever Search value with param_field is given,in runtime it is passing the random value to that field.
Cmd name: SHT_NN in Configure.          Not an Issue - configuration 
5. For Iview navigation,param_field is not given but in runtime passing Search value.Observed that no error is thrown.
Cmd name: testiview in Configure Fixed 
6. Updated the status flag from 'T' to 'F' for a command.In Runtime,user is able to select the cmd and execute it.
Getting error"Dispatch Error: No handler function found for 'Configure' -> 'testtstruct1'"
Cmd name: testtstruct1 in Configure. Fixed 
7. Updated the status flag from 'T' to Null for a command.In Runtime,user is able to select the cmd and execute it.
Getting error"Dispatch Error: No handler function found for 'Configure' -> 'cmdtest1'"
Cmd name: cmdtest1 in Configure. Fixed 
8. For URL/Page Navigation,when opened that page in pop up mode in runtime,then getting error"404 - File or directory not found.
The resource you are looking for might have been removed, had its name changed, or is temporarily unavailable."
Cmd name: SHT_NN in Configure. Not an issue 