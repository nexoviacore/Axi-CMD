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
VALUES(7, 'DevTools', '', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(9, 'Run', '', 'T')
>>

<<
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(10, 'Analyse', '', 'T')
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
VALUES('10655119-ba93-42e8-8aef-0aefccae5a80'::uuid, 4, 2, 'object type', '', NULL, 'PEG,Form Notification,Scheduled Notification,Peg Form Notification,Rule,KeyField,News And Announcement,User,Users,User Permission,User Permissions,User Activation,User Group,Role,Roles,Role Permissions,Actor,Actors,Publish Axpert API,Publish Config Studio,Card,Responsibility,Responsibilities,Dimension,Application Properties,Settings', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('0f99d918-0caa-4523-9260-f18b5bd162bf'::uuid, 4, 3, 'object name', 'Axi_PegList,Axi_FormNotifyList,Axi_ScheduleNotifyList,Axi_PEGNotifyList,Axi_RuleNamesList,axi_structlist,axi_newsandannounce,Axi_Dummy,Axi_Dummy,axi_userlist,axi_userlist,axi_useractivation,axi_usergrouplist,Axi_Dummy,Axi_Dummy,axi_rolelist,axi_actorlist,Axi_Dummy,axi_publishapi,Axi_ServernameList,axi_cardlist,axi_resposibilitylist,Axi_Dummy,axi_dimensionlist,Axi_Dummy,Axi_Dummy', NULL, '', NULL, ':userresp,:mode,:structtype', NULL)
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
VALUES('369efe5f-7e05-4b00-b2fd-10fae4bd3f72'::uuid, 7, 2, 'type', NULL, NULL, 'Tstruct,Iview,Axpert Data Sources,Page,Arrange Menu,Dev Option,App Variables,Db Explorer,API Plugin,Axpert Job,Language,Publish,Custom Data Type,Email Definition,Table Field Descriptor,Custom Plugin,Queue Listing,Out Bound Queue,In Bound Queue,Mem DB Console', NULL, NULL, NULL)
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
INSERT INTO axi_command_prompts
(id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl)
VALUES('8fbbe05b-af25-4be7-b97c-de71825267f7'::uuid, 10, 2, 'entity name', 'axi_structmetalist', NULL, NULL, NULL, ':username,:userroles,:userresp,:mode,:structtype', NULL)
>>
