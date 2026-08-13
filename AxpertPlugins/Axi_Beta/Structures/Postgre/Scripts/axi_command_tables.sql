<<
DROP TABLE axi_commands
>>

<<
DROP TABLE axi_command_prompts
>>

<<
CREATE TABLE axi_commands (
	cmdtoken int4 NOT NULL,
	command_group varchar(50) NOT NULL,
	command varchar(50) NOT NULL,
	active varchar(1) NULL DEFAULT 'T'::character varying,
	CONSTRAINT axi_commands_pkey PRIMARY KEY (cmdtoken)
)
>>

<<
CREATE TABLE axi_command_prompts (
	id uuid NOT NULL DEFAULT gen_random_uuid(),
	cmdtoken int4 NULL,
	wordpos int4 NULL,
	prompt varchar(200) NULL,
	promptsource varchar(500) NULL,
	promptparams varchar(100) NULL,
	promptvalues varchar(500) NULL,
	props varchar(100) NULL,
	extraparams varchar(1000) NULL,
	requesturl varchar(2000) NULL,
	CONSTRAINT axi_command_prompts_pkey PRIMARY KEY (id)
)
>>

<<
CREATE TABLE axp_tstructprops (
	"name" varchar(5) NULL,
	caption varchar(500) NULL,
	keyfield varchar(200) NULL,
	userconfigured bpchar(1) NULL,
	createdon varchar(30) NULL,
	updatedon varchar(30) NULL,
	createdby varchar(100) NULL,
	updatedby varchar(100) NULL
)
>>

--axi_commands starts here
<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(1, 'Create', '', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(2, 'Edit', '', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(3, 'View', '', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(4, 'Configure', '', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(5, 'Upload', '', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(6, 'Download', '', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(7, 'SDK', '', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(9, 'Run', '', 'T')
>>

--axi_command_prompts starts here
<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('b767f878-6f6f-4d72-8a52-f987d5dc9064'::uuid, 1, 2, 'tstruct name', 'axi_structmetalist', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('8faae04b-af25-4be7-b97c-de72815255f4'::uuid, 2, 2, 'tstruct name', 'axi_structmetalist', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('29ce28d9-72f1-41ff-b872-faf9107774b6'::uuid, 2, 3, 'search value', 'axi_getstructsdata', '', NULL, NULL, ':cmd,:username,:userrole,:transid,:selectedfield,:dimension,:permission,:keyfield,:primarytable,:globalvars', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('8faae04b-af25-4be7-b97c-de72815267f5'::uuid, 2, 4, 'object name', 'axi_getstructsdata', '', NULL, NULL, ':cmd,:username,:userrole,:transid,:selectedfield,:dimension,:permission,:keyfield,:primarytable,:globalvars', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('7faae15b-af25-4be7-b86c-de73925267f5'::uuid, 2, 5, 'with values', '', '', 'With', NULL, '', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('b878f939-6f7f-4d72-8a78-f912d5dc9669'::uuid, 2, 6, 'field name', 'axi_nongridfieldlist', '2', NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('46647ef0-f107-4551-8379-3b844d430016'::uuid, 3, 2, 'object name', 'axi_structmetalist', NULL, 'Tstruct,Iview,Ads,Page', NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('46d7bb0e-12e4-4249-b508-f9e824717957'::uuid, 3, 3, 'search value', 'axi_getstructsdata,axi_dummylist,axi_adscolumnlist,axi_dummylist', '', NULL, NULL, ':cmd,:username,:userrole,:transid,:selectedfield,:dimension,:permission,:keyfield,:primarytable,:globalvars', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('8faae09b-af52-4be8-b97c-de72815276f4'::uuid, 3, 4, 'object name', 'axi_getstructsdata', '', NULL, NULL, ':cmd,:username,:userrole,:transid,:selectedfield,:dimension,:permission,:keyfield,:primarytable,:globalvars', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('10655119-ba93-42e8-8aef-0aefccae5a80'::uuid, 4, 2, 'object type', '', NULL, 'PEG,Form Notification,Scheduled Notification,Peg Form Notification,Rule,KeyField,User,User Listing,User Permission Setup,User Permissions,User Activation,User Group,Role,Role Listing,Role Permissions,Actor,Actor Listing,Publish Axpert API,Publish Config Studio,Card,Responsibility,Responsibility Listing,Dimension,Dimension Listing, Application Properties,Settings,Smart View Attributes,Smart View Listing', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('0f99d918-0caa-4523-9260-f18b5bd162bf'::uuid, 4, 3, 'object name', 'Axi_PegList,Axi_FormNotifyList,Axi_ScheduleNotifyList,Axi_PEGNotifyList,Axi_RuleNamesList,axi_structmetalist,Axi_Dummy,Axi_Dummy,axi_userlist,axi_userlist,axi_useractivation,axi_usergrouplist,Axi_Dummy,Axi_Dummy,axi_rolelist,axi_actorlist,Axi_Dummy,axi_publishapi,Axi_ServernameList,axi_cardlist,axi_resposibilitylist,Axi_Dummy,axi_dimensionlist,Axi_Dummy,Axi_Dummy,Axi_Dummy,axi_smartviewlist,Axi_Dummy', NULL, '', NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('0f67d918-0caa-4523-9260-f18b5bd982fb'::uuid, 4, 4, 'key field', 'axi_primaryfieldlist', '3', '', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('b148e471-7ace-49e7-a7ab-16d3338908cf'::uuid, 5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('3ddb84e6-6e76-48c8-8dd5-4d46fc4f9542'::uuid, 6, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('369efe5f-7e05-4b00-b2fd-10fae4bd3f72'::uuid, 7, 2, 'type', NULL, NULL, 'TStruct,IView,Axpert Data Sources,Page,Arrange Menu,Dev Option,App Variables,Db Explorer,API Plugin,Axpert Job,Language,Publish,Custom Data Type,Email Definition,Table Field Descriptor,Custom Plugin,Queue Listing,Out Bound Queue,In Bound Queue,Mem DB Console', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('4aa86d11-a357-4ba1-ab1d-b4251676ba8f'::uuid, 7, 3, 'name', 'axi_structmetalist,axi_structmetalist,axi_structmetalist,axi_structmetalist,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_APINamesList,axi_jobs,axi_language,Axi_Dummy,axi_customtype,axi_emaildef,axi_tabledesc,Axi_Dummy,Axi_Dummy,axi_outbound,axi_inbound,Axi_Dummy', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('8fbbe05b-af25-4be7-b97c-de71825267f6'::uuid, 8, 2, 'field name', 'Axi_SetFieldList', NULL, NULL, NULL, ':transid', NULL)
>>

<<
CREATE TABLE IF NOT EXISTS axi_command_config (
	config_id varchar(50) NOT NULL,
	command varchar(50) NOT NULL,
	prompt_options varchar(200) NOT NULL,
	prompt_id varchar(50) NOT NULL,
	prompt_option_type varchar(20) NOT NULL,
	param_field varchar(100) NULL,
	target_url varchar(500) NULL,
	extra_params varchar(500) NULL,
	active varchar(1) DEFAULT 'T',
	CONSTRAINT pk_axi_command_config PRIMARY KEY (config_id)
)
>>

<<
<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_users', 'Configure', 'user listing', 'axusers', 'iview', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_user', 'Configure', 'user', 'axusr', 'tstruct', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_roles', 'Configure', 'role listing', 'ad___url', 'iview', NULL, '../aspx/iview.aspx?ivname=ad___url', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_role', 'Configure', 'role', 'ad_ur', 'tstruct', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_responsibilities', 'Configure', 'responsibility listing', 'response', 'iview', NULL, '../aspx/iview.aspx?ivname=response', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_responsibility', 'Configure', 'responsibility', 'axrol', 'url', 'name', '../aspx/AddEditResponsibility.aspx', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_actor_listing', 'Configure', 'actor listing', 'ad__act', 'iview', NULL, '../aspx/iview.aspx?ivname=ad__act', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_actor', 'Configure', 'actor', 'ad_am', 'tstruct', 'actorname', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_dimension_listing', 'Configure', 'dimension listing', 'ad___upg', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=ad___upg', 'AxOpenAct=true&isDupTab=false', 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_dimension', 'Configure', 'dimension', 'a_pgm', 'tstruct', 'grpname', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_smartview_listing', 'Configure', 'smart view listing', 'a___smtl', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=a___smtl', 'AxOpenAct=true&isDupTab=false', 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_smartview_attrs', 'Configure', 'smart view attributes', 'a__sl', 'tstruct', 'adsname', NULL, 'act=load&dummyload=false', 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_user_group', 'Configure', 'user group', 'a__ug', 'tstruct', 'users_group_name', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_user_activation', 'Configure', 'user activation', 'axurg', 'tstruct', 'pusername', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_user_permissions', 'Configure', 'user permissions', 'ad___upm', 'ivtoivload', 'pusername', '../aspx/ivtoivload.aspx?ivname=ad___upm', 'AxOpenAct=true&isDupTab=false', 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_user_perm_setup', 'Configure', 'user permission setup', 'a__up', 'tstruct', 'axusername', NULL, 'fromsource=U&openerIV=a__up&isIV=true&isDupTab=false&dummyload=false?', 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_role_permissions', 'Configure', 'role permissions', 'ad___ups', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=ad___ups', 'AxOpenAct=true&isDupTab=false', 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_publish_api', 'Configure', 'publish axpert api', 'ad_pa', 'tstruct', 'publickey', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_publish_listing', 'Configure', 'publish config studio', 'axpub/ad_pbcs', 'tstruct/iview', 'servername', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_card', 'Configure', 'card', 'a__cd', 'tstruct', 'cardname', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_peg', 'Configure', 'peg', 'ad_pm', 'processflow', 'processname', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_rule', 'Configure', 'rule', 'ad_re', 'tstruct', 'rulename', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_form_notif', 'Configure', 'form notification', 'a__fn', 'tstruct', 'stransid', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_peg_form_notif', 'Configure', 'peg form notification', 'ad_pn', 'tstruct', 'name', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_sched_notif', 'Configure', 'scheduled notification', 'a__pn', 'tstruct', 'name', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_keyfield', 'Configure', 'keyfield', 'axi_tstructprops_insupd', 'action', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_app_props', 'Configure', 'application properties', 'ad_pm', 'tstruct', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_settings', 'Configure', 'settings', 'configuration.aspx', 'url', NULL, '../aspx/configuration.aspx', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_ads', 'SDK', 'axpert data sources', 'b_sql', 'tstruct', 'sqlname', NULL, 'act=load&dummyload=false?', 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_page', 'SDK', 'page', 'sect', 'tstruct', 'caption', NULL, 'act=load&dummyload=false?', 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_app_var', 'SDK', 'app variables', 'axvar', 'tstruct', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_dev_option', 'SDK', 'dev option', 'axstc', 'tstruct', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_db_explorer', 'SDK', 'db explorer', 'AxDBScript.aspx', 'url', NULL, '../aspx/AxDBScript.aspx', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_arrange_menu', 'SDK', 'arrange menu', 'ArrangeMenu.aspx', 'url', NULL, '../aspx/ArrangeMenu.aspx', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_api_plugin', 'SDK', 'api plugin', 'apidg', 'tstruct', 'ExecAPIDefName', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_job', 'SDK', 'axpert job', 'job_s', 'tstruct', 'jobid', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_language', 'SDK', 'language', 'ad_lg', 'tstruct', 'language', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_publish', 'SDK', 'publish', 'axpubls', 'iview', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_custom_data_type', 'SDK', 'custom data type', 'ctype', 'tstruct', 'typename', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_email_def', 'SDK', 'email definition', 'axeml', 'tstruct', 'emaildefname', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_table_desc', 'SDK', 'table field descriptor', 'a__td', 'tstruct', 'dname', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_memdb_console', 'SDK', 'mem db console', 'inmemdb', 'iview', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_custom_plugin', 'SDK', 'custom plugin', 'PluginCustomCode.aspx', 'url', NULL, '../aspx/PluginCustomCode.aspx', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_queue_listing', 'SDK', 'queue listing', 'ad__qls', 'iview', NULL, NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_outbound_queue', 'SDK', 'out bound queue', 'a__qm', 'tstruct', 'axqueuename', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_sdk_inbound_queue', 'SDK', 'in bound queue', 'a__iq', 'tstruct', 'axqueuename', NULL, NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_upload_default', 'Upload', 'default', 'importall.aspx', 'url', NULL, '../aspx/importall.aspx', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_download_default', 'Download', 'default', 'exportnew.aspx', 'url', NULL, '../aspx/exportnew.aspx', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
>>
>>

<<
CREATE INDEX IF NOT EXISTS idx_axi_cmd_config_cmd ON axi_command_config(command)
>>




