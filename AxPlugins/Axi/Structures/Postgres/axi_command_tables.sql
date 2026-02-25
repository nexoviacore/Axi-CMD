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
CREATE TABLE axi_commands (
	cmdtoken int4 NOT NULL,
	command_group varchar(50) NOT NULL,
	command varchar(50) NOT NULL,
	active varchar(1) NULL DEFAULT 'T'::character varying,
	CONSTRAINT axi_commands_pkey PRIMARY KEY (cmdtoken)
)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('b767f878-6f6f-4d72-8a52-f987d5dc9064'::uuid, 1, 2, 'tstruct name', 'Axi_TStructList', NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('b678f938-6f7f-4d72-8a78-f987d5dc8780'::uuid, 1, 3, 'with values', '', '', 'with', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('b767f938-6f7f-4d72-8a78-f987d5dc9678'::uuid, 1, 4, 'field name', 'Axi_FieldList', '2', NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('8faae04b-af25-4be7-b97c-de72815255f4'::uuid, 2, 2, 'tstruct name', 'Axi_TStructList', NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('29ce28d9-72f1-41ff-b872-faf9107774b6'::uuid, 2, 3, 'search value', 'Axi_KeyValuesWithFieldNamesList', '2', NULL, NULL, 'axi_keyfieldlist', NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('8faae04b-af25-4be7-b97c-de72815267f5'::uuid, 2, 4, 'object name', 'Axi_FieldValuesWithKeySuffixList', '2,3', NULL, NULL, '', NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('7faae15b-af25-4be7-b86c-de73925267f5'::uuid, 2, 5, 'with values', '', '', 'with', NULL, '', NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('b878f939-6f7f-4d72-8a78-f912d5dc9669'::uuid, 2, 6, 'field name', 'Axi_FieldList', '2', NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('46647ef0-f107-4551-8379-3b844d430016'::uuid, 3, 2, 'object name', 'Axi_ViewList', NULL, 'tstruct,iview,ads,page', NULL, '', NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('46d7bb0e-12e4-4249-b508-f9e824717957'::uuid, 3, 3, 'search value', 'Axi_KeyValuesWithFieldNamesList,axi_dummylist,axi_adscolumnlist,axi_dummylist', '2', NULL, NULL, 'axi_keyfieldlist', NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('8faae09b-af52-4be8-b97c-de72815276f4'::uuid, 3, 4, 'object name', 'Axi_FieldValuesWithKeySuffixList', '2,3', NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('10655119-ba93-42e8-8aef-0aefccae5a80'::uuid, 4, 2, 'object type', '', NULL, 'peg,formnotification,schdulednotification,pegformnotification,job,printform,api,rule,properties,permission,access,server,keyfield', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('0f99d918-0caa-4523-9260-f18b5bd162bf'::uuid, 4, 3, 'object name', 'Axi_PegList,Axi_FormNotifyList,Axi_ScheduleNotifyList,Axi_PEGNotifyList,Axi_JobNamesList,Axi_PrintFormList,Axi_APINamesList,Axi_RuleNamesList,Axi_Dummy,axi_userlist,axi_rolelist,Axi_ServernameList,Axi_TStructList', NULL, '', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('0f67d918-0caa-4523-9260-f18b5bd982fb'::uuid, 4, 4, 'key field', 'Axi_FieldList', '3', '', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('b148e471-7ace-49e7-a7ab-16d3338908cf'::uuid, 5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('3ddb84e6-6e76-48c8-8dd5-4d46fc4f9542'::uuid, 6, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('369efe5f-7e05-4b00-b2fd-10fae4bd3f72'::uuid, 7, 2, 'type', NULL, NULL, 'tstruct,iview,ads,page,card,devoption,appvar,dbconsole,packager', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('4aa86d11-a357-4ba1-ab1d-b4251676ba8f'::uuid, 7, 3, 'name', 'Axi_TStructList,Axi_IViewList,Axi_AdsList,Axi_PageList,Axi_CardList,,Axi_Dummy,,Axi_Dummy,,Axi_Dummy,,Axi_Dummy,,Axi_Dummy', NULL, NULL, NULL, NULL, NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('8fbbe05b-af25-4be7-b97c-de71825267f6'::uuid, 8, 2, 'field name', 'Axi_SetFieldList', NULL, NULL, NULL, ':transid', NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('8fbbe05b-af25-4be7-b97c-de71825267f7'::uuid, 10, 2, 'entity name', 'Axi_AnalyticsList', NULL, NULL, NULL, ':username', NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('8fbbe05b-af25-4be7-b89c-de71825278f8'::uuid, 10, 3, 'group by', 'axi_fieldlist', '2', NULL, NULL, '', NULL)
>>

<<
INSERT INTO axi_command_prompts (id, cmdtoken, wordpos, prompt, promptsource, promptparams, promptvalues, props, extraparams, requesturl) VALUES('c6b1f464-95ef-4f87-93e0-4b78b44da6c9'::uuid, 11, 2, 'name', NULL, NULL, 'start', NULL, NULL, NULL)
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(1, 'create', '', 'T')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(2, 'edit', '', 'T')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(3, 'view', '', 'T')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(4, 'configure', '', 'T')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(5, 'upload', '', 'T')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(6, 'download', '', 'T')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(7, 'open', '', 'T')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(8, 'set', '', 'F')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(9, 'run', '', 'T')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(10, 'analyse', '', 'T')
>>

<<
INSERT INTO axi_commands (cmdtoken, command_group, command, active) VALUES(11, 'ai', '', 'T')
>>