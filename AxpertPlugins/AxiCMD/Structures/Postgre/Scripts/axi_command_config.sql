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
	is_system varchar(1) DEFAULT 'F',
	record_source varchar(100) NULL,
	CONSTRAINT pk_axi_command_config PRIMARY KEY (config_id)
)
>>


<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_users', 'Configure', 'user listing', 'axusers', 'iview', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
ALTER TABLE axi_command_config ADD COLUMN IF NOT EXISTS is_system varchar(1) DEFAULT 'F'
>>

<<
-- Record source of a Custom Option (third word of the command); NULL = None (Axi_Dummy). Built-in Options leave it NULL
ALTER TABLE axi_command_config ADD COLUMN IF NOT EXISTS record_source varchar(100) NULL
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_user', 'Configure', 'user', 'axusr', 'tstruct', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_roles', 'Configure', 'role listing', 'ad___url', 'iview', NULL, '../aspx/iview.aspx?ivname=ad___url', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_role', 'Configure', 'role', 'ad_ur', 'tstruct', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_responsibilities', 'Configure', 'responsibility listing', 'response', 'iview', NULL, '../aspx/iview.aspx?ivname=response', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_responsibility', 'Configure', 'responsibility', 'axrol', 'url', 'name', '../aspx/AddEditResponsibility.aspx', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_actor_listing', 'Configure', 'actor listing', 'ad__act', 'iview', NULL, '../aspx/iview.aspx?ivname=ad__act', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_actor', 'Configure', 'actor', 'ad_am', 'tstruct', 'actorname', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_dimension_listing', 'Configure', 'dimension listing', 'ad___upg', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=ad___upg', 'AxOpenAct=true&isDupTab=false', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_dimension', 'Configure', 'dimension', 'a_pgm', 'tstruct', 'grpname', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_smartview_listing', 'Configure', 'smart view listing', 'a___smtl', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=a___smtl', 'AxOpenAct=true&isDupTab=false', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_smartview_attrs', 'Configure', 'smart view attributes', 'a__sl', 'tstruct', 'adsname', NULL, 'act=load&dummyload=false', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_user_group', 'Configure', 'user group', 'a__ug', 'tstruct', 'users_group_name', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_user_activation', 'Configure', 'user activation', 'axurg', 'tstruct', 'pusername', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_user_permissions', 'Configure', 'user permissions', 'ad___upm', 'ivtoivload', 'pusername', '../aspx/ivtoivload.aspx?ivname=ad___upm', 'AxOpenAct=true&isDupTab=false', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_user_perm_setup', 'Configure', 'user permission setup', 'a__up', 'tstruct', 'axusername', NULL, 'fromsource=U&openerIV=a__up&isIV=true&isDupTab=false&dummyload=false?', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_role_permissions', 'Configure', 'role permissions', 'ad___ups', 'ivtoivload', 'prole', '../aspx/ivtoivload.aspx?ivname=ad___ups', 'AxOpenAct=true&isDupTab=false', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_publish_api', 'Configure', 'publish axpert api', 'ad_pa', 'tstruct', 'publickey', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_publish_listing', 'Configure', 'publish config studio', 'axpub/ad_pbcs', 'tstruct/iview', 'servername', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_card', 'Configure', 'card', 'a__cd', 'tstruct', 'cardname', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_peg', 'Configure', 'peg', 'ad_pm', 'processflow', 'processname', '../aspx/processflow.aspx?loadcaption=AxProcessBuilder', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_rule', 'Configure', 'rule', 'ad_re', 'tstruct', 'rulename', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_form_notif', 'Configure', 'form notification', 'a__fn', 'tstruct', 'stransid', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_peg_form_notif', 'Configure', 'peg form notification', 'ad_pn', 'tstruct', 'name', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_sched_notif', 'Configure', 'scheduled notification', 'a__pn', 'tstruct', 'name', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_keyfield', 'Configure', 'keyfield', 'axi_tstructprops_insupd', 'action', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_app_props', 'Configure', 'application properties', 'ad_pr', 'url', NULL, '../aspx/tstruct.aspx', 'act=load&transid=ad_pr&axpdef_axpertpropsid=1&dummyload=false', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_settings', 'Configure', 'settings', 'configuration.aspx', 'url', NULL, '../aspx/configuration.aspx', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_trace_on', 'Configure', 'trace on', 'trace_on', 'shellaction', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_trace_off', 'Configure', 'trace off', 'trace_off', 'shellaction', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_configure_trace_files', 'Configure', 'trace files', 'trace_files', 'shellaction', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_ads', 'SDK', 'axpert data sources', 'b_sql', 'tstruct', 'sqlname', NULL, 'act=load&dummyload=false?', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_page', 'SDK', 'page', 'sect', 'tstruct', 'caption', NULL, 'act=load&dummyload=false?', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_app_var', 'SDK', 'app variables', 'axvar', 'tstruct', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_dev_option', 'SDK', 'dev option', 'axstc', 'tstruct', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_db_explorer', 'SDK', 'db explorer', 'AxDBScript.aspx', 'url', NULL, '../aspx/AxDBScript.aspx', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_arrange_menu', 'SDK', 'arrange menu', 'ArrangeMenu.aspx', 'url', NULL, '../aspx/ArrangeMenu.aspx', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_api_plugin', 'SDK', 'api plugin', 'apidg', 'tstruct', 'ExecAPIDefName', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_job', 'SDK', 'axpert job', 'job_s', 'tstruct', 'jobid', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_language', 'SDK', 'language', 'ad_lg', 'tstruct', 'language', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_publish', 'SDK', 'publish', 'axpubls', 'iview', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_custom_data_type', 'SDK', 'custom data type', 'ctype', 'tstruct', 'typename', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_email_def', 'SDK', 'email definition', 'axeml', 'tstruct', 'emaildefname', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_table_desc', 'SDK', 'table field descriptor', 'a__td', 'tstruct', 'dname', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_memdb_console', 'SDK', 'mem db console', 'inmemdb', 'iview', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_custom_plugin', 'SDK', 'custom plugin', 'PluginCustomCode.aspx', 'url', NULL, '../aspx/PluginCustomCode.aspx', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_queue_listing', 'SDK', 'queue listing', 'ad__qls', 'iview', NULL, NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_outbound_queue', 'SDK', 'out bound queue', 'a__qm', 'tstruct', 'axqueuename', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_sdk_inbound_queue', 'SDK', 'in bound queue', 'a__iq', 'tstruct', 'axqueuename', NULL, NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_upload_default', 'Upload', 'default', 'importall.aspx', 'url', NULL, '../aspx/importall.aspx', NULL, 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>

<<
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active, is_system) VALUES ('cfg_download_default', 'Download', 'default', 'ExportNew.aspx', 'url', NULL, '../aspx/ExportNew.aspx', 'action=export', 'T', 'T') ON CONFLICT (config_id) DO UPDATE SET is_system = 'T'
>>
>>

<<
CREATE INDEX IF NOT EXISTS idx_axi_cmd_config_cmd ON axi_command_config(command)
>>

<<
-- Custom Options (is_system = 'F') live in axi_command_config, but the palette reads option names and record sources from
-- axi_command_prompts, which axi_command_tables.sql recreates on every install. Run this file after axi_command_tables.sql:
-- it puts every Custom Option name back at the end of the option list of its command (wordpos 2) and its record_source (Axi_Dummy = None
-- when null) at the same position of the paired source list (wordpos 3). Names already in the list are skipped, so it is safe to run again.
-- Configure = cmdtoken 4, SDK = cmdtoken 7. One statement, so both lists change together.
-- The columns hold 4000 characters: a command whose lists would pass that after the append is skipped as a whole (both lists stay as they
-- are, paired) instead of failing the install, the same as the Oracle script; add the missing options from the AxiCMD Config page after removing others.
UPDATE axi_command_prompts p
SET promptvalues = CASE WHEN p.wordpos = 2 THEN COALESCE(p.promptvalues, '') || m.add_names ELSE p.promptvalues END,
    promptsource = CASE WHEN p.wordpos = 3 THEN COALESCE(p.promptsource, '') || m.add_sources ELSE p.promptsource END
FROM (
    SELECT k.cmdtoken,
           string_agg(',' || c.prompt_options, '' ORDER BY c.config_id) AS add_names,
           string_agg(',' || COALESCE(c.record_source, 'Axi_Dummy'), '' ORDER BY c.config_id) AS add_sources
    FROM (VALUES ('configure', 4), ('sdk', 7)) AS k(command, cmdtoken)
    JOIN axi_command_config c ON LOWER(c.command) = k.command AND COALESCE(c.is_system, 'F') = 'F'
    JOIN axi_command_prompts n ON n.cmdtoken = k.cmdtoken AND n.wordpos = 2
    JOIN axi_command_prompts s ON s.cmdtoken = k.cmdtoken AND s.wordpos = 3
    WHERE strpos(',' || LOWER(REPLACE(REPLACE(COALESCE(n.promptvalues, ''), ', ', ','), ' ,', ',')) || ',', ',' || LOWER(c.prompt_options) || ',') = 0
    GROUP BY k.cmdtoken
    HAVING MAX(LENGTH(COALESCE(n.promptvalues, ''))) + LENGTH(string_agg(',' || c.prompt_options, '' ORDER BY c.config_id)) <= 4000
       AND MAX(LENGTH(COALESCE(s.promptsource, ''))) + LENGTH(string_agg(',' || COALESCE(c.record_source, 'Axi_Dummy'), '' ORDER BY c.config_id)) <= 4000
) m
WHERE p.cmdtoken = m.cmdtoken AND p.wordpos IN (2, 3)
>>
