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
VALUES(7, 'SDK', ' ', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(9, 'Run', ' ', 'T')
>>


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
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('B141E931BB7144788EC8B60253361FFB', 4, 2, 'object type', NULL, NULL, 'PEG,Form Notification,Scheduled Notification,Peg Form Notification,Rule,KeyField,User,User Listing,User Permission Setup,User Permissions,User Activation,User Group,Role,Role Listing,Role Permissions,Actor,Actor Listing,Publish Axpert API,Publish Config Studio,Card,Responsibility,Responsibility Listing,Dimension,Dimension Listing, Application Properties,Settings,Smart View Attributes,Smart View Listing', NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('8C14DFDF564E4A06840CB160AB710385', 4, 4, 'key field', 'axi_primaryfieldlist', '3', NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('726EB7B2772742CEA2DBC0A91AB3DA16', 4, 3, 'object name', 'Axi_PegList,Axi_FormNotifyList,Axi_ScheduleNotifyList,Axi_PEGNotifyList,Axi_RuleNamesList,axi_structmetalist,Axi_Dummy,Axi_Dummy,axi_userlist,axi_userlist,axi_useractivation,axi_usergrouplist,Axi_Dummy,Axi_Dummy,axi_rolelist,axi_actorlist,Axi_Dummy,axi_publishapi,Axi_ServernameList,axi_cardlist,axi_resposibilitylist,Axi_Dummy,axi_dimensionlist,Axi_Dummy,Axi_Dummy,Axi_Dummy,axi_smartviewlist,Axi_Dummy', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('E7944B5EE0334473BE362031CBCB3B54', 5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('53A5F63DAD9F4A0E84421659B9160904', 6, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('643B7E52385F46A38DC09F05CA811776', 7, 2, 'type', NULL, NULL, 'TStruct,IView,Axpert Data Sources,Page,Arrange Menu,Dev Option,App Variables,Db Explorer,API Plugin,Axpert Job,Language,Publish,Custom Data Type,Email Definition,Table Field Descriptor,Custom Plugin,Queue Listing,Out Bound Queue,In Bound Queue,Mem DB Console', NULL, NULL, NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('6DC84B80511E4C7AAE88573143584F1F', 7, 3, 'name', 'axi_structmetalist,axi_structmetalist,axi_structmetalist,axi_structmetalist,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_APINamesList,axi_jobs,axi_language,Axi_Dummy,axi_customtype,axi_emaildef,axi_tabledesc,Axi_Dummy,Axi_Dummy,axi_outbound,axi_inbound,Axi_Dummy', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO AXI_COMMAND_PROMPTS (ID, CMDTOKEN, WORDPOS, PROMPT, PROMPTSOURCE, PROMPTPARAMS, PROMPTVALUES, PROPS, EXTRAPARAMS, REQUESTURL) VALUES('57E65A66C98C4BF7B167B0FB182FD14F', 8, 2, 'field name', 'Axi_SetFieldList', NULL, NULL, NULL, ':transid', NULL)
>>

<<
CREATE TABLE AXI_COMMAND_CONFIG 
   (	"CONFIG_ID" VARCHAR2(50) NOT NULL ENABLE, 
	"COMMAND" VARCHAR2(50) NOT NULL ENABLE, 
	"PROMPT_OPTIONS" VARCHAR2(200) NOT NULL ENABLE, 
	"PROMPT_ID" VARCHAR2(50) NOT NULL ENABLE, 
	"PROMPT_OPTION_TYPE" VARCHAR2(20) NOT NULL ENABLE, 
	"PARAM_FIELD" VARCHAR2(100), 
	"TARGET_URL" VARCHAR2(500), 
	"EXTRA_PARAMS" VARCHAR2(500), 
	"ACTIVE" VARCHAR2(1) DEFAULT 'T', 
	 CONSTRAINT "PK_AXI_COMMAND_CONFIG" PRIMARY KEY ("CONFIG_ID"))
>>

<<
CREATE INDEX "IDX_AXI_CMD_CONFIG_CMD" ON AXI_COMMAND_CONFIG ("COMMAND")
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_users', 'Configure', 'user listing', 'axusers', 'iview', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_user', 'Configure', 'user', 'axusr', 'tstruct', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_roles', 'Configure', 'role listing', 'ad___url', 'iview', NULL, '../aspx/iview.aspx?ivname=ad___url', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_role', 'Configure', 'role', 'ad_ur', 'tstruct', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_responsibilities', 'Configure', 'responsibility listing', 'response', 'iview', NULL, '../aspx/iview.aspx?ivname=response', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_responsibility', 'Configure', 'responsibility', 'axrol', 'url', 'name', '../aspx/AddEditResponsibility.aspx', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_actor_listing', 'Configure', 'actor listing', 'ad__act', 'iview', NULL, '../aspx/iview.aspx?ivname=ad__act', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_actor', 'Configure', 'actor', 'ad_am', 'tstruct', 'actorname', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_dimension_listing', 'Configure', 'dimension listing', 'ad___upg', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=ad___upg', 'AxOpenAct=true&isDupTab=false', 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_dimension', 'Configure', 'dimension', 'a_pgm', 'tstruct', 'grpname', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_smartview_listing', 'Configure', 'smart view listing', 'a___smtl', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=a___smtl', 'AxOpenAct=true&isDupTab=false', 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_smartview_attrs', 'Configure', 'smart view attributes', 'a__sl', 'tstruct', 'adsname', NULL, 'act=load&dummyload=false', 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_user_group', 'Configure', 'user group', 'a__ug', 'tstruct', 'users_group_name', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_user_activation', 'Configure', 'user activation', 'axurg', 'tstruct', 'pusername', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_user_permissions', 'Configure', 'user permissions', 'ad___upm', 'ivtoivload', 'pusername', '../aspx/ivtoivload.aspx?ivname=ad___upm', 'AxOpenAct=true&isDupTab=false', 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_user_perm_setup', 'Configure', 'user permission setup', 'a__up', 'tstruct', 'axusername', NULL, 'fromsource=U&openerIV=a__up&isIV=true&isDupTab=false&dummyload=false?', 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_role_permissions', 'Configure', 'role permissions', 'ad___ups', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=ad___ups', 'AxOpenAct=true&isDupTab=false', 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_publish_api', 'Configure', 'publish axpert api', 'ad_pa', 'tstruct', 'publickey', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_publish_listing', 'Configure', 'publish config studio', 'ad_pbcs', 'iview', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_card', 'Configure', 'card', 'a__cd', 'tstruct', 'cardname', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_peg', 'Configure', 'peg', 'ad_pm', 'url', 'processname', '../aspx/processflow.aspx?loadcaption=AxProcessBuilder', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_rule', 'Configure', 'rule', 'ad_re', 'tstruct', 'rulename', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_form_notif', 'Configure', 'form notification', 'a__fn', 'tstruct', 'stransid', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_peg_form_notif', 'Configure', 'peg form notification', 'ad_pn', 'tstruct', 'name', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_sched_notif', 'Configure', 'scheduled notification', 'a__pn', 'tstruct', 'name', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_keyfield', 'Configure', 'keyfield', 'ad_kf', 'tstruct', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_app_props', 'Configure', 'application properties', 'ad_pm', 'tstruct', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_configure_settings', 'Configure', 'settings', 'configuration.aspx', 'url', NULL, '../aspx/configuration.aspx', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_ads', 'SDK', 'axpert data sources', 'b_sql', 'tstruct', 'sqlname', NULL, 'act=load&dummyload=false?', 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_page', 'SDK', 'page', 'sect', 'tstruct', 'caption', NULL, 'act=load&dummyload=false?', 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_app_var', 'SDK', 'app variables', 'axvar', 'tstruct', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_dev_option', 'SDK', 'dev option', 'axstc', 'tstruct', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_db_explorer', 'SDK', 'db explorer', 'AxDBScript.aspx', 'url', NULL, '../aspx/AxDBScript.aspx', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_arrange_menu', 'SDK', 'arrange menu', 'ArrangeMenu.aspx', 'url', NULL, '../aspx/ArrangeMenu.aspx', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_api_plugin', 'SDK', 'api plugin', 'apidg', 'tstruct', 'ExecAPIDefName', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_job', 'SDK', 'axpert job', 'job_s', 'tstruct', 'jobid', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_language', 'SDK', 'language', 'ad_lg', 'tstruct', 'language', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_publish', 'SDK', 'publish', 'axpubls', 'iview', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_custom_data_type', 'SDK', 'custom data type', 'ctype', 'tstruct', 'typename', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_email_def', 'SDK', 'email definition', 'axeml', 'tstruct', 'emaildefname', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_table_desc', 'SDK', 'table field descriptor', 'a__td', 'tstruct', 'dname', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_memdb_console', 'SDK', 'mem db console', 'inmemdb', 'iview', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_custom_plugin', 'SDK', 'custom plugin', 'PluginCustomCode.aspx', 'url', NULL, '../aspx/PluginCustomCode.aspx', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_queue_listing', 'SDK', 'queue listing', 'ad__qls', 'iview', NULL, NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_outbound_queue', 'SDK', 'out bound queue', 'a__qm', 'tstruct', 'axqueuename', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_sdk_inbound_queue', 'SDK', 'in bound queue', 'a__iq', 'tstruct', 'axqueuename', NULL, NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_upload_default', 'Upload', 'default', 'importall.aspx', 'url', NULL, '../aspx/importall.aspx', NULL, 'T')
>>

<<
INSERT INTO AXI_COMMAND_CONFIG (CONFIG_ID, COMMAND, PROMPT_OPTIONS, PROMPT_ID, PROMPT_OPTION_TYPE, PARAM_FIELD, TARGET_URL, EXTRA_PARAMS, ACTIVE) VALUES ('cfg_download_default', 'Download', 'default', 'exportnew.aspx', 'url', NULL, '../aspx/exportnew.aspx', NULL, 'T')
>>



