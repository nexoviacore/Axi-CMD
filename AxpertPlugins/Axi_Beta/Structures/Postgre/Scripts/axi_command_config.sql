<<
DROP TABLE axi_command_config
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
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_peg', 'Configure', 'peg', 'ad_pm', 'processflow', 'processname', '../aspx/processflow.aspx?loadcaption=AxProcessBuilder', NULL, 'T') ON CONFLICT (config_id) DO NOTHING
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
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_configure_app_props', 'Configure', 'application properties', 'tstruct.aspx', 'url', NULL, '../aspx/tstruct.aspx', 'act=load&transid=ad_pr&axpdef_axpertpropsid=1&dummyload=false', 'T') ON CONFLICT (config_id) DO NOTHING
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
INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES ('cfg_download_default', 'Download', 'default', 'ExportNew.aspx', 'url', NULL, '../aspx/ExportNew.aspx', 'action=export', 'T') ON CONFLICT (config_id) DO NOTHING
>>
>>

<<
CREATE INDEX IF NOT EXISTS idx_axi_cmd_config_cmd ON axi_command_config(command)
>>




