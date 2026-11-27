-- Oracle version of dbupdate.sql
-- axi_command_settings: Git account, repository and token for Product, Plugin and ChangeRequest.
-- One INSERT per row (Oracle has no multi-row VALUES). A row is added only when its category/type/name is not there yet, so a re-run changes nothing.
-- id is left out: its column default fills it. gittoken is left empty: set the real token in the a__ax form, never in this script.
<<
INSERT INTO axi_command_settings (axi_command_settingsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, propertycategory, propertytype, propertyname, propertyvalue, dupchk) SELECT 1059110000003, 'F', 0, NULL, 'admin', TO_DATE('2026-11-25 10:06:56', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-11-25 10:06:56', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'axicmd', 'product', 'accountname', 'salmanagilelabs', 'axicmdproductaccountname' FROM dual WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings WHERE propertycategory = 'axicmd' AND propertytype = 'product' AND propertyname = 'accountname')
>>

<<
INSERT INTO axi_command_settings (axi_command_settingsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, propertycategory, propertytype, propertyname, propertyvalue, dupchk) SELECT 1059220000000, 'F', 0, NULL, 'admin', TO_DATE('2026-11-25 10:12:50', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-11-25 10:12:50', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'axicmd', 'product', 'reponame', 'AxiProduct', 'axicmdproductreponame' FROM dual WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings WHERE propertycategory = 'axicmd' AND propertytype = 'product' AND propertyname = 'reponame')
>>

<<
INSERT INTO axi_command_settings (axi_command_settingsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, propertycategory, propertytype, propertyname, propertyvalue, dupchk) SELECT 1059330000000, 'F', 0, NULL, 'admin', TO_DATE('2026-11-25 10:13:32', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-11-25 10:13:32', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'axicmd', 'product', 'gittoken', NULL, 'axicmdproductgittoken' FROM dual WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings WHERE propertycategory = 'axicmd' AND propertytype = 'product' AND propertyname = 'gittoken')
>>

<<
INSERT INTO axi_command_settings (axi_command_settingsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, propertycategory, propertytype, propertyname, propertyvalue, dupchk) SELECT 1061440000007, 'F', 0, NULL, 'admin', TO_DATE('2026-11-25 15:51:18', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-11-25 15:51:18', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'axicmd', 'plugin', 'accountname', 'salmanagilelabs', 'axicmdpluginaccountname' FROM dual WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings WHERE propertycategory = 'axicmd' AND propertytype = 'plugin' AND propertyname = 'accountname')
>>

<<
INSERT INTO axi_command_settings (axi_command_settingsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, propertycategory, propertytype, propertyname, propertyvalue, dupchk) SELECT 1061550000019, 'F', 0, NULL, 'admin', TO_DATE('2026-11-25 15:51:52', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-11-25 15:51:52', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'axicmd', 'plugin', 'reponame', 'AxiPlugin', 'axicmdpluginreponame' FROM dual WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings WHERE propertycategory = 'axicmd' AND propertytype = 'plugin' AND propertyname = 'reponame')
>>

<<
INSERT INTO axi_command_settings (axi_command_settingsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, propertycategory, propertytype, propertyname, propertyvalue, dupchk) SELECT 1061660000000, 'F', 0, NULL, 'admin', TO_DATE('2026-11-25 15:52:43', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-11-25 15:52:43', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'axicmd', 'plugin', 'gittoken', NULL, 'axicmdplugingittoken' FROM dual WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings WHERE propertycategory = 'axicmd' AND propertytype = 'plugin' AND propertyname = 'gittoken')
>>

<<
INSERT INTO axi_command_settings (axi_command_settingsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, propertycategory, propertytype, propertyname, propertyvalue, dupchk) SELECT 1061770000001, 'F', 0, NULL, 'admin', TO_DATE('2026-11-25 15:52:56', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-11-25 15:52:56', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'axicmd', 'changerequest', 'gittoken', NULL, 'axicmdchangerequestgittoken' FROM dual WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings WHERE propertycategory = 'axicmd' AND propertytype = 'changerequest' AND propertyname = 'gittoken')
>>

<<
INSERT INTO axi_command_settings (axi_command_settingsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, propertycategory, propertytype, propertyname, propertyvalue, dupchk) SELECT 1061880000001, 'F', 0, NULL, 'admin', TO_DATE('2026-11-25 15:53:18', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-11-25 15:53:18', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'axicmd', 'changerequest', 'reponame', 'AxiChangeRequest', 'axicmdchangerequestreponame' FROM dual WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings WHERE propertycategory = 'axicmd' AND propertytype = 'changerequest' AND propertyname = 'reponame')
>>

<<
INSERT INTO axi_command_settings (axi_command_settingsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, propertycategory, propertytype, propertyname, propertyvalue, dupchk) SELECT 1061990000002, 'F', 0, NULL, 'admin', TO_DATE('2026-11-25 15:53:33', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-11-25 15:53:33', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'axicmd', 'changerequest', 'accountname', 'salmanagilelabs', 'axicmdchangerequestaccountname' FROM dual WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings WHERE propertycategory = 'axicmd' AND propertytype = 'changerequest' AND propertyname = 'accountname')
>>


-- Configure Product, Plugin and Change Request Delete Sql Queries
<<
DELETE FROM AXI_COMMAND_CONFIG WHERE config_id IN ('cfg_configure_product', 'cfg_configure_plugin', 'cfg_configure_changerequest')
>>


-- Configure: Product, Plugin and ChangeRequest (Git account / repository) are no longer set from AxiCMD. The two statements below take them
-- out of a database that already has them. Sources go first (their guard reads the option list, which the next statement changes); each
-- source is dropped at the same position as its option, so Custom Options keep their sources. A re-run changes nothing.
<<
UPDATE AXI_COMMAND_PROMPTS s SET promptsource = (SELECT LISTAGG(x.src, ',') WITHIN GROUP (ORDER BY x.lvl) FROM (SELECT LEVEL AS lvl, REGEXP_SUBSTR(ps.promptsource, '[^,]+', 1, LEVEL) AS src, REGEXP_SUBSTR(pv.promptvalues, '[^,]+', 1, LEVEL) AS val FROM (SELECT promptsource FROM AXI_COMMAND_PROMPTS WHERE cmdtoken = 4 AND wordpos = 3) ps CROSS JOIN (SELECT promptvalues FROM AXI_COMMAND_PROMPTS WHERE cmdtoken = 4 AND wordpos = 2) pv CONNECT BY LEVEL <= REGEXP_COUNT(RTRIM(ps.promptsource, ', '), ',') + 1) x WHERE x.val IS NULL OR LOWER(TRIM(x.val)) NOT IN ('product', 'plugin', 'changerequest')) WHERE s.cmdtoken = 4 AND s.wordpos = 3 AND EXISTS (SELECT 1 FROM AXI_COMMAND_PROMPTS p2 WHERE p2.cmdtoken = 4 AND p2.wordpos = 2 AND LOWER(p2.promptvalues) LIKE '%changerequest%')
>>

<<
UPDATE AXI_COMMAND_PROMPTS SET promptvalues = (SELECT LISTAGG(x.val, ',') WITHIN GROUP (ORDER BY x.lvl) FROM (SELECT LEVEL AS lvl, REGEXP_SUBSTR(pv.promptvalues, '[^,]+', 1, LEVEL) AS val FROM (SELECT promptvalues FROM AXI_COMMAND_PROMPTS WHERE cmdtoken = 4 AND wordpos = 2) pv CONNECT BY LEVEL <= REGEXP_COUNT(RTRIM(pv.promptvalues, ', '), ',') + 1) x WHERE LOWER(TRIM(x.val)) NOT IN ('product', 'plugin', 'changerequest')) WHERE cmdtoken = 4 AND wordpos = 2 AND LOWER(promptvalues) LIKE '%changerequest%'
>>
