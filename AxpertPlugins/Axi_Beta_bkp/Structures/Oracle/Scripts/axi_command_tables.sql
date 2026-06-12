<<
DROP TABLE axi_commands
>>

<<
DROP TABLE axi_command_prompts
>>

<<
CREATE TABLE AXI_COMMANDS 
   (	"CMDTOKEN" NUMBER(10,0) NOT NULL ENABLE, 
	"COMMAND_GROUP" VARCHAR2(50) NOT NULL ENABLE, 
	"COMMAND" VARCHAR2(50) NOT NULL ENABLE, 
	"ACTIVE" VARCHAR2(1) DEFAULT 'T', 
	 CONSTRAINT "AXI_COMMANDS_PKEY" PRIMARY KEY ("CMDTOKEN"))
>>

<<
CREATE TABLE AXI_COMMAND_PROMPTS 
   (	"ID" RAW(16) DEFAULT SYS_GUID() NOT NULL ENABLE, 
	"CMDTOKEN" NUMBER(10,0), 
	"WORDPOS" NUMBER(10,0), 
	"PROMPT" VARCHAR2(200), 
	"PROMPTSOURCE" VARCHAR2(500), 
	"PROMPTPARAMS" VARCHAR2(100), 
	"PROMPTVALUES" VARCHAR2(500), 
	"PROPS" VARCHAR2(100), 
	"EXTRAPARAMS" VARCHAR2(1000), 
	"REQUESTURL" VARCHAR2(2000), 
	 CONSTRAINT "AXI_COMMAND_PROMPTS_PKEY" PRIMARY KEY ("ID"))
>>
	
<<
CREATE TABLE AXP_TSTRUCTPROPS
   (	"NAME" VARCHAR2(5), 
	"CAPTION" VARCHAR2(500), 
	"KEYFIELD" VARCHAR2(200), 
	"USERCONFIGURED" CHAR(1), 
	"CREATEDON" VARCHAR2(30), 
	"UPDATEDON" VARCHAR2(30), 
	"CREATEDBY" VARCHAR2(100), 
	"UPDATEDBY" VARCHAR2(100)
   )
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(1, 'Create', ' ', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(2, 'Edit', ' ', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(3, 'View', ' ', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(4, 'Configure', ' ', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(5, 'Upload', ' ', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(6, 'Download', ' ', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(7, 'DevTools', ' ', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(9, 'Run', ' ', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(10, 'Analyse', ' ', 'T')
>>

--axi_command_prompts
<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('BF3CDB0564C24FA7A120D431E79DAD9F', 1, 2, 'tstruct name', 'axi_structmetalist', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('6F37FEBA1F2A48B1A843E8892D53AC39', 2, 2, 'tstruct name', 'axi_structmetalist', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('9BD6F43DE54A42B98E00C092141590BC', 2, 3, 'search value', 'axi_getstructsdata', NULL, NULL, NULL, ':cmd,:username,:userrole,:transid,:selectedfield,:dimension,:permission,:keyfield,:primarytable,:globalvars', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('E16A5ED4468C46D7BC63EA3815F49041', 2, 5, 'with values', NULL, NULL, 'With', NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('8D36BC3859474B27970AD3654F55A7CB', 2, 4, 'object name', 'axi_getstructsdata', NULL, NULL, NULL, ':cmd,:username,:userrole,:transid,:selectedfield,:dimension,:permission,:keyfield,:primarytable,:globalvars', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('CCFCFA109A0D4A528227E9A67365DE27', 2, 6, 'field name', 'axi_nongridfieldlist', '2', NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('8ADB95690A364F6DA8EE66E890423D44', 3, 2, 'object name', 'axi_structmetalist', NULL, 
'Tstruct,Iview,Ads,Page', NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('9397F98CBBA94BFEA1E8A16553CA58BF', 3, 3, 'search value', 'axi_getstructsdata,axi_dummylist,axi_adscolumnlist,axi_dummylist', NULL, NULL, NULL, ':cmd,:username,:userrole,:transid,:selectedfield,:dimension,:permission,:keyfield,:primarytable,:globalvars', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('2B3247C93FFB4B04A11DCEE5901260EE', 3, 4, 'object name', 'axi_getstructsdata', NULL, NULL, NULL, ':cmd,:username,:userrole,:transid,:selectedfield,:dimension,:permission,:keyfield,:primarytable,:globalvars', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('B141E931BB7144788EC8B60253361FFB', 4, 2, 'object type', NULL, NULL, 'PEG,Form Notification,Scheduled Notification,Peg Form Notification,Rule,KeyField,News And Announcement,User,Users,User Permission,User Permissions,User Activation,User Group,Role,Roles,Role Permissions,Actor,Actors,Publish Axpert API,Publish Config Studio,Card,Responsibility,Responsibilities,Dimension,Application Properties,Settings', NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('8C14DFDF564E4A06840CB160AB710385', 4, 4, 'key field', 'axi_primaryfieldlist', '3', NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('726EB7B2772742CEA2DBC0A91AB3DA16', 4, 3, 'object name', 'Axi_PegList,Axi_FormNotifyList,Axi_ScheduleNotifyList,Axi_PEGNotifyList,Axi_RuleNamesList,axi_structmetalist,axi_newsandannounce,Axi_Dummy,Axi_Dummy,axi_userlist,axi_userlist,axi_useractivation,axi_usergrouplist,Axi_Dummy,Axi_Dummy,axi_rolelist,axi_actorlist,Axi_Dummy,axi_publishapi,Axi_ServernameList,axi_cardlist,axi_resposibilitylist,Axi_Dummy,axi_dimensionlist,Axi_Dummy,Axi_Dummy', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('E7944B5EE0334473BE362031CBCB3B54', 5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('53A5F63DAD9F4A0E84421659B9160904', 6, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('643B7E52385F46A38DC09F05CA811776', 7, 2, 'type', NULL, NULL, 'Tstruct,Iview,Axpert Data Sources,Page,Arrange Menu,Dev Option,App Variables,Db Explorer,API Plugin,Axpert Job,Language,Publish,Custom Data Type,Email Definition,Table Field Descriptor,Custom Plugin,Queue Listing,Out Bound Queue,In Bound Queue,Mem DB Console', NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('6DC84B80511E4C7AAE88573143584F1F', 7, 3, 'name', 'axi_structmetalist,axi_structmetalist,axi_structmetalist,axi_structmetalist,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_APINamesList,axi_jobs,axi_language,Axi_Dummy,axi_customtype,axi_emaildef,axi_tabledesc,Axi_Dummy,Axi_Dummy,axi_outbound,axi_inbound,Axi_Dummy', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('57E65A66C98C4BF7B167B0FB182FD14F', 8, 2, 'field name', 'Axi_SetFieldList', NULL, NULL, NULL, ':transid', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('494F14790B6B406BB2E0D227907BB724', 10, 2, 'entity name', 'axi_structmetalist', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>
