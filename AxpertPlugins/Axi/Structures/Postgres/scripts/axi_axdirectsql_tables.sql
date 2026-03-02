<<
CREATE TABLE axdirectsql (
	axdirectsqlid numeric(16) NOT NULL,
	cancel varchar(1) NULL,
	sourceid numeric(16) NULL,
	mapname varchar(20) NULL,
	username varchar(50) NULL,
	modifiedon timestamp NULL,
	createdby varchar(50) NULL,
	createdon timestamp NULL,
	wkid varchar(15) NULL,
	app_level numeric(3) NULL,
	app_desc numeric(1) NULL,
	app_slevel numeric(3) NULL,
	cancelremarks varchar(150) NULL,
	wfroles varchar(250) NULL,
	sqlname varchar(50) NULL,
	ddldatatype varchar(20) NULL,
	sqlsrc varchar(30) NULL,
	sqlsrccnd numeric(10) NULL,
	sqltext text NULL,
	paramcal varchar(200) NULL,
	sqlparams varchar(2000) NULL,
	accessstring varchar(500) NULL,
	groupname varchar(50) NULL,
	sqlquerycols varchar(4000) NULL,
	cachedata varchar(1) NULL,
	cacheinterval varchar(10) NULL,
	encryptedflds varchar(4000) NULL,
	adsdesc text NULL,
	CONSTRAINT aglaxdirectsqlid PRIMARY KEY (axdirectsqlid)
)
>>

<<
ALTER TABLE axdirectsql ADD cachedata varchar(1) NULL
>>

<<
ALTER TABLE axdirectsql ADD cacheinterval varchar(10) NULL
>>

<<
ALTER TABLE axdirectsql ADD encryptedflds varchar(4000) NULL
>>

<<
ALTER TABLE axdirectsql ADD adsdesc text NULL
>>

<<
CREATE TABLE axdirectsql_metadata (
	axdirectsql_metadataid numeric(16) NOT NULL,
	axdirectsqlid numeric(16) NULL,
	axdirectsql_metadatarow int4 NULL,
	fldname varchar(100) NULL,
	fldcaption varchar(100) NULL,
	"normalized" varchar(3) NULL,
	sourcetable varchar(50) NULL,
	sourcefld varchar(50) NULL,
	CONSTRAINT aglaxdirectsql_metadataid PRIMARY KEY (axdirectsql_metadataid)
)
>>

<<
ALTER TABLE axdirectsql_metadata ADD tbl_normalizedsource varchar(2000) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD tbl_hyperlink varchar(8000) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD hyp_struct varchar(500) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD hyp_structtype varchar(20) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD hyp_transid varchar(100) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD datatypeui varchar(20) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD fdatatype varchar(2) NULL
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990001, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_jobnameslist', NULL, 'General', 5, 'select jname as displaydata from axpdef_jobs', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990002, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_formnotifylist', NULL, 'General', 5, 'select form as displaydata,stransid name from axformnotify', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990003, 'F', 0, NULL, 'admin', '2025-12-24 19:34:05.000', 'admin', '2025-12-24 19:34:05.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_userlist', NULL, 'General', 5, 'select username as displaydata from axusers', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990004, 'F', 0, NULL, 'admin', '2025-12-23 20:50:43.000', 'admin', '2025-12-23 20:50:43.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_fieldvaluelist', NULL, 'General', 5, 'SELECT * FROM get_dynamic_field(:param1, :param2) as displaydata;', 'param1,param2', 'param1~~,param2~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990005, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_tstructlist', NULL, 'General', 5, 'select caption||'' (''||name||'')'' displaydata, caption, name from tstructs', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990006, 'F', 0, NULL, 'admin', '2025-12-22 14:31:09.000', 'admin', '2025-12-20 16:20:58.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_adslist', NULL, 'General', 5, 'select sqlname||'' (''||sqlsrc||'')'' displaydata,sqlname name,sqlsrc  from axdirectsql a', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990007, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_apinameslist', NULL, 'General', 5, 'select execapidefname as displaydata from executeapidef', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990008, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_iviewlist', NULL, 'General', 5, 'select caption||'' (''||name||'')'' displaydata, caption, name from Iviews', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990009, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_pegnotifylist', NULL, 'General', 5, 'select name as displaydata from axnotificationdef', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990010, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_rulenameslist', NULL, 'General', 5, 'select rulename as displaydata from axpdef_ruleeng', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990011, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_schedulenotifylist', NULL, 'General', 5, 'select name as displaydata from axperiodnotify', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990012, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_peglist', NULL, 'General', 5, 'select caption as displaydata from axpdef_peg_processmaster', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990013, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_actorlist', NULL, 'General', 5, 'select actorname as displaydata from axpdef_peg_actor', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990014, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_cardlist', NULL, 'General', 5, 'select cardname as displaydata from axp_cards', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990015, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_printformlist', NULL, 'General', 5, 'select template_name as displaydata from ax_configure_fast_prints', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990016, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_servernamelist', NULL, 'General', 5, 'select servername as displaydata from dwb_publishprops', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990017, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_usergrouplist', NULL, 'General', 5, 'select users_group_name as displaydata from axpdef_usergroups', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990018, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_rolelist', NULL, 'General', 5, 'select groupname as displaydata from axusergroups', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990019, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_dimensionlist', NULL, 'General', 5, 'select grpnamedb as displaydata from AxGrouping', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990020, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_pagelist', NULL, 'General', 5, 'select caption as displaydata,props as requesturl from axpages where pagetype = ''web''', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990021, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_keyfieldlist', NULL, 'General', 5, 'SELECT keyfield FROM (SELECT keyfield, 1 AS priority, NULL AS modeofentry, NULL AS allowduplicate, NULL AS datatype, NULL AS ordno FROM axp_tstructprops WHERE name = :param1 UNION ALL SELECT fname AS keyfield, 2 AS priority, modeofentry, allowduplicate, datatype, ordno FROM axpflds WHERE tstruct = :param1 and dcname = ''dc1'' AND (modeofentry = ''autogenerate'' OR ((LOWER(allowduplicate) = ''f'' OR datatype = ''c'') AND LOWER(hidden) = ''f''))) t ORDER BY priority, CASE WHEN modeofentry = ''autogenerate'' THEN 1 WHEN LOWER(allowduplicate) = ''f'' THEN 2 WHEN datatype = ''c'' THEN 3 ELSE 4 END, ordno ASC LIMIT 1', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

--old query
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990022, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_viewlist', NULL, 'General', 5, 'select * from (select caption||'' (''||name||'')'' || '' [tstruct]'' displaydata , caption, name from tstructs union select caption||'' (''||name||'')'' || '' [iview]'' displaydata, caption, name from iviews union select caption|| '' [page]'' as displaydata ,caption, props name from axpages where pagetype = ''web'' and (props is not null and props <> '''') union select sqlname||'' (''||sqlsrc||'')'' || '' [ads]'' displaydata, sqlsrc caption, sqlname name from axdirectsql union select ''Inbox'' displaydata,''Inbox'' caption,''Inbox'' name from dual) src order by displaydata asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990022, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_viewlist', NULL, 'Application', 5, 'select * from (select a.caption||'' (''||a.name||'')'' || '' [tstruct]'' displaydata , a.caption, a.name from tstructs a,axpages b where b.visible = ''T'' and b.pagetype = ''t''||a.name union select a.caption||'' (''||a.name||'')'' || '' [iview]'' displaydata, a.caption, a.name from iviews a,axpages b where b.visible = ''T'' and b.pagetype = ''i''||a.name union select caption|| '' [page]'' as displaydata ,caption, props name from axpages where pagetype = ''web'' and (props is not null and props <> '''') and visible = ''T'' union SELECT sqlname || '' ('' || sqlsrc || '') [ads]'' AS displaydata,sqlsrc AS caption,sqlname AS name FROM axdirectsql a WHERE EXISTS (SELECT 1 FROM axdirectsql_metadata m WHERE m.axdirectsqlid = a.axdirectsqlid) union select ''Inbox'' displaydata,''Inbox'' caption,''Inbox'' name from dual) src order by displaydata asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL);
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990023, 'F', 0, NULL, 'admin', '2026-01-30 00:00:00.000', 'admin', '2026-01-30 00:00:00.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_smartlist_ads_metadata', NULL, 'System', 0, 'select a.sqlname,a.sqltext,a.sqlparams, a.sqlquerycols,a.encryptedflds,a.cachedata,a.cacheinterval, b.fldname,b.fldcaption,b."normalized" ,b.sourcetable ,b.sourcefld ,hyp_structtype,b.hyp_transid, b.tbl_hyperlink, CASE WHEN lower(sqltext) LIKE ''%--axp_filter%'' THEN ''T'' ELSE ''F'' END AS filters from axdirectsql a left join axdirectsql_metadata b on a.axdirectsqlid =b.axdirectsqlid where sqlname = :adsname', 'adsname', 'adsname~Character~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990024, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_fieldvalueswithkeysuffixlist', NULL, 'General', 5, 'select * from fn_axi_get_fieldvalues_with_keysuffix_list(:param1, :param2);', 'param1,param2', 'param1~~,param2~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990025, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_keyvalueswithfieldnameslist', NULL, 'General', 5, 'select * from  fn_axi_getkeyvalueswithfieldnameslist( :param1 , :param2);', 'param1,param2', 'param1~~,param2~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990026, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_tstructprops_insupd', NULL, 'General', 5, 'select * from fn_upsert_config_by_condition(:param1,:param2,:param3,:param4)', 'param1,param2,param3,param4', 'param1~~,param2~~,param3~~,param4~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990027, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_adsfilteroperators', NULL, 'General', 5, 'SELECT ''='' AS displaydata, ''='' AS name UNION ALL SELECT ''<'',''<'' UNION ALL SELECT ''>'',''>'' UNION ALL SELECT ''<='',''<='' UNION ALL SELECT ''>='',''>='' UNION ALL SELECT ''between'',''between''', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990028, 'F', 0, NULL, 'admin', '2026-01-30 00:00:00.000', 'admin', '2026-01-30 00:00:00.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_adscolumnlist', NULL, 'System', 0, 'select b.fldcaption || ''(''||b.fldname||'')'' displaydata,b.fldname name,b.fldcaption caption,b.normalized,b.fdatatype, b.sourcetable,b.sourcefld , CASE WHEN lower(sqltext) LIKE ''%--axp_filter%'' THEN ''T'' ELSE ''F'' END AS filters from axdirectsql a left join axdirectsql_metadata b on a.axdirectsqlid =b.axdirectsqlid where sqlname = :param1', 'param1', 'param1~Character~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990029, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_fieldlist', NULL, 'Application', 5, 'select caption||'' (''||fname||'')'' displaydata, caption, fname name, tstruct,substring(modeofentry,1,1) moe,"datatype",fldsql,dcname,asgrid,listvalues fromlist,srckey normalized from axpflds where tstruct = :param1 and dcname = ''dc1'' and hidden = ''F'' and readonly = ''F'' and modeofentry in (''accept'',''select'')and savevalue = ''T'' and "datatype" <> ''i'' order by ordno asc', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990030, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_adsdropdowntokens', NULL, 'General', 5, 'select * from get_ads_dropdown_data(:param1,:param2)', 'param1,param2', 'param1~~,param2~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990031, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_setfieldlist', NULL, 'General', 5, 'select caption||'' (''||fname||'')'' displaydata, caption, fname name, tstruct,substring(modeofentry,1,1) moe,"datatype",fldsql sql from axpflds where tstruct = :param1 and dcname = ''dc1'' and hidden = ''F'' and savevalue = ''T'' and modeofentry in (''accept'',''select'') and "datatype" <> ''i'' order by ordno asc', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc) VALUES(99999999990032, 'F', 0, NULL, 'admin', '2025-12-24 19:34:05.000', 'admin', '2025-12-24 19:34:05.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_analyticslist', NULL, 'General', 5, 'SELECT t.caption || '' ('' || t.name || '')'' AS displaydata,t.caption,t.name FROM tstructs t JOIN ax_userconfigdata u ON t.name = ANY (string_to_array(u.value, '','')) WHERE Upper(u.page) = ''ANALYTICS'' and Upper(u.keyname) = ''ENTITIES'' AND (u.username = :param1 OR u.username = ''All'')', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL)
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990033, 'F', 0, NULL, 'admin', '2025-12-23 20:50:43.000', 'admin', '2025-12-23 20:50:43.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_firesql', NULL, 'Application', 5, 'select * from axi_firesql_v2(:param1,:param2,:param3,:param4)', 'param1,param2,param3,param4', 'param1~~,param2~~,param3~~,param4~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990034, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_userpwd', NULL, 'Application', 5, 'select password  from axusers where username = :param1', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL);
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqltext, paramcal, sqlparams, accessstring, groupname, sqlsrc, sqlsrccnd, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990035, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_primaryfieldlist', NULL, 'SELECT caption||'' (''||fname||'')'' displaydata, caption, fname name FROM axpflds WHERE tstruct = :param1 and dcname = ''dc1'' AND (modeofentry = ''autogenerate'' OR ((LOWER(allowduplicate) = ''f'' OR datatype = ''c'') AND LOWER(hidden) = ''f'')) order by ordno asc', 'param1', 'param1', 'ALL', NULL, 'Application', 5, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqltext, paramcal, sqlparams, accessstring, groupname, sqlsrc, sqlsrccnd, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990036, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_runtstructlist', NULL, 'select a.caption||'' (''||a.name||'')'' displaydata, a.caption, a.name from tstructs a, axpages b where b.visible = ''T'' and pagetype = ''t''||a.name', NULL, NULL, 'ALL', NULL, 'Application', 5, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqltext, paramcal, sqlparams, accessstring, groupname, sqlsrc, sqlsrccnd, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990037, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_nongridfieldlist', NULL, 'select caption||'' (''||fname||'')'' displaydata, caption, fname name, tstruct,substring(modeofentry,1,1) moe,"datatype",fldsql,dcname,asgrid,listvalues fromlist,srckey normalized from axpflds where tstruct = :param1 and asgrid = ''F'' and hidden = ''F'' and modeofentry in (''accept'',''select'')and savevalue = ''T'' and "datatype" <> ''i'' order by ordno asc', 'param1', 'param1', 'ALL', NULL, 'Application', 5, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>