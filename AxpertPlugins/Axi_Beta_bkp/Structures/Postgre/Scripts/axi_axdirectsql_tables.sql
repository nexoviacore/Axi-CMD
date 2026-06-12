
<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990001
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990002
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990003
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990004
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990005
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990006
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990007
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990008
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990009
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990010
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990011
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990012
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990013
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990014
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990015
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990016
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990017
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990018
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990019
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990020
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990021
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990022
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990023
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990024
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990025
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990026
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990027
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990028
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990029
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990030
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990031
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990032
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990033
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990034
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990035
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990036
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990037
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990038
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990039
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990040
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990041
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990042
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990043
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990044
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990045
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990046
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990047
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990048
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990049
>>

<<
DELETE FROM axdirectsql where axdirectsqlid = 99999999990050
>>

-- axdirectsql DDL

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

-- axdirectsql UPDATE

<<
UPDATE axdirectsql SET sqlname='axi_smartlist_ads_metadata', sqltext='select
	a1.sqlname,
	a1.sqltext,
	a1.sqlparams,
	a1.sqlquerycols,
	a1.encryptedflds,
	a1.cachedata,
	a1.cacheinterval,
	b.fldname,
	b.fldcaption,
	b."normalized" ,
	b.sourcetable ,
	b.sourcefld ,
	hl.hyp_structtype,
	hl.hyp_transid,
	hl.tbl_hyperlink,hl.hyp_inline,
	case
		when lower(sqltext) like ''%--axp_filter%'' then ''T''
		else ''F''
	end as filters
from axdirectsql a1
join axpdef_smartlist a on a1.sqlname = a.adsname
join axpdef_smartlist_mdata b on a.axpdef_smartlistid =b.axpdef_smartlistid
left join(select axpdef_smartlistid,hfldname,hyp_structtype,hyp_transid, tbl_hyperlink,hyp_inline from axpdef_smartlist_hlink)hl
on hl.axpdef_smartlistid=a.axpdef_smartlistid and b.fldname = hl.hfldname
where a.adsname = :adsname
order by b.axpdef_smartlist_mdatarow ' WHERE sqlname='axi_smartlist_ads_metadata'
>>

-- axdirectsql INSERT

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990001, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_jobnameslist', NULL, 'Metadata', 5, 'select jname as displaydata from axpdef_jobs', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990002, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_formnotifylist', NULL, 'Metadata', 5, 'select form as displaydata,stransid name from axformnotify', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990003, 'F', 0, NULL, 'admin', '2025-12-24 19:34:05.000', 'admin', '2025-12-24 19:34:05.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_userlist', NULL, 'Metadata', 5, 'select username as displaydata from axusers', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990004, 'F', 0, NULL, 'admin', '2025-12-23 20:50:43.000', 'admin', '2025-12-23 20:50:43.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_fieldvaluelist', NULL, 'Metadata', 5, 'SELECT * FROM get_dynamic_field(:param1, :param2) as displaydata;', 'param1,param2', 'param1~~,param2~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990005, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_tstructlist', NULL, 'Metadata', 5, 'select caption||'' (''||name||'')'' displaydata, caption, name from tstructs', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990006, 'F', 0, NULL, 'admin', '2025-12-22 14:31:09.000', 'admin', '2025-12-20 16:20:58.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_adslist', NULL, 'Metadata', 5, 'select sqlname||'' (''||sqlsrc||'')'' displaydata,sqlname name,sqlsrc  from axdirectsql a', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990007, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_apinameslist', NULL, 'Metadata', 5, 'select execapidefname as displaydata from executeapidef', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990008, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_iviewlist', NULL, 'Metadata', 5, 'select caption||'' (''||name||'')'' displaydata, caption, name from Iviews', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990009, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_pegnotifylist', NULL, 'Metadata', 5, 'select name as displaydata from axnotificationdef', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990010, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_rulenameslist', NULL, 'Metadata', 5, 'select rulename as displaydata from axpdef_ruleeng', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990011, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_schedulenotifylist', NULL, 'Metadata', 5, 'select name as displaydata from axperiodnotify', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990012, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_peglist', NULL, 'Metadata', 5, 'select caption as displaydata from axpdef_peg_processmaster', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990013, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_actorlist', NULL, 'Metadata', 5, 'select actorname as displaydata from axpdef_peg_actor', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990014, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_cardlist', NULL, 'Metadata', 5, 'select cardname as displaydata from axp_cards', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990015, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_printformlist', NULL, 'Metadata', 5, 'select template_name as displaydata from ax_configure_fast_prints', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990016, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_servernamelist', NULL, 'Metadata', 5, 'select servername as displaydata from dwb_publishprops', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990017, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_usergrouplist', NULL, 'Metadata', 5, 'select users_group_name as displaydata from axpdef_usergroups', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990018, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_rolelist', NULL, 'Metadata', 5, 'select groupname as displaydata from axusergroups', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990019, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_dimensionlist', NULL, 'Metadata', 5, 'select grpcaption ||'' ('' || grpname ||'')'' as displaydata, grpname as caption,grpname as name from axgroupingmst order by grpcaption asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990020, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_pagelist', NULL, 'Metadata', 5, 'select caption as displaydata,props as requesturl from axpages where pagetype = ''web''', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990021, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_keyfieldlist', NULL, 'Metadata', 5, 'SELECT keyfield FROM (SELECT keyfield, 1 AS priority, NULL AS modeofentry, NULL AS allowduplicate, NULL AS datatype, NULL AS ordno FROM axp_tstructprops WHERE name = :param1 UNION ALL SELECT fname AS keyfield, 2 AS priority, modeofentry, allowduplicate, datatype, ordno FROM axpflds WHERE tstruct = :param1 and dcname = ''dc1'' AND (modeofentry = ''autogenerate'' OR ((LOWER(allowduplicate) = ''f'' OR datatype = ''c'') AND LOWER(hidden) = ''f''))) t ORDER BY priority, CASE WHEN modeofentry = ''autogenerate'' THEN 1 WHEN LOWER(allowduplicate) = ''f'' THEN 2 WHEN datatype = ''c'' THEN 3 ELSE 4 END, ordno ASC LIMIT 1', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990022, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_viewlist', NULL, 'Metadata', 5, 'SELECT * FROM axi_fn_getaxobjectlist(:param1)', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990023, 'F', 0, NULL, 'admin', '2026-01-30 00:00:00.000', 'admin', '2026-01-30 00:00:00.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_smartlist_ads_metadata', NULL, 'Metadata', 0, 'select a.sqlname,a.sqltext,a.sqlparams, a.sqlquerycols,a.encryptedflds,a.cachedata,a.cacheinterval, b.fldname,b.fldcaption,b."normalized" ,b.sourcetable ,b.sourcefld ,hyp_structtype,b.hyp_transid, b.tbl_hyperlink, CASE WHEN lower(sqltext) LIKE ''%--axp_filter%'' THEN ''T'' ELSE ''F'' END AS filters from axdirectsql a left join axdirectsql_metadata b on a.axdirectsqlid =b.axdirectsqlid where sqlname = :adsname', 'adsname', 'adsname~Character~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990024, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_fieldvalueswithkeysuffixlist', NULL, 'Metadata', 5, 'select * from fn_axi_get_fieldvalues_with_keysuffix_list(:param1, :param2)', 'param1,param2', 'param1~~,param2~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990025, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_keyvalueswithfieldnameslist', NULL, 'Metadata', 5, 'select * from fn_axi_getkeyvalueswithfieldnameslist(:param1, :param2)', 'param1,param2', 'param1~~,param2~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990026, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_tstructprops_insupd', NULL, 'Metadata', 5, 'select * from fn_upsert_config_by_condition(:param1,:param2,:param3,:param4)', 'param1,param2,param3,param4', 'param1~~,param2~~,param3~~,param4~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990027, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_adsfilteroperators', NULL, 'Metadata', 5, 'SELECT ''='' AS displaydata, ''='' AS name UNION ALL SELECT ''<'',''<'' UNION ALL SELECT ''>'',''>'' UNION ALL SELECT ''<='',''<='' UNION ALL SELECT ''>='',''>='' UNION ALL SELECT ''between'',''between''', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990028, 'F', 0, NULL, 'admin', '2026-01-30 00:00:00.000', 'admin', '2026-01-30 00:00:00.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_adscolumnlist', NULL, 'Metadata', 0, 'select b.fldcaption || ''(''||b.fldname||'')'' displaydata,b.fldname name,b.fldcaption caption,b.normalized,b.fdatatype, b.sourcetable,b.sourcefld , CASE WHEN lower(sqltext) LIKE ''%--axp_filter%'' THEN ''T'' ELSE ''F'' END AS filters from axdirectsql a left join axdirectsql_metadata b on a.axdirectsqlid =b.axdirectsqlid where sqlname = :param1', 'param1', 'param1~Character~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990029, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_fieldlist', NULL, 'Metadata', 5, 'select caption||'' (''||fname||'')'' displaydata, caption, fname name, tstruct,substring(modeofentry,1,1) moe,"datatype",fldsql,dcname,asgrid,listvalues fromlist,srckey normalized from axpflds where tstruct = :param1 and dcname = ''dc1'' and hidden = ''F'' and modeofentry in (''accept'',''select'')and savevalue = ''T'' and "datatype" <> ''i'' order by ordno asc', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990030, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_adsdropdowntokens', NULL, 'Metadata', 5, 'select * from get_ads_dropdown_data(:param1,:param2)', 'param1,param2', 'param1~~,param2~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990031, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_setfieldlist', NULL, 'Metadata', 5, 'select caption||'' (''||fname||'')'' displaydata, caption, fname name, tstruct,substring(modeofentry,1,1) moe,"datatype",fldsql sql from axpflds where tstruct = :param1 and dcname = ''dc1'' and hidden = ''F'' and savevalue = ''T'' and modeofentry in (''accept'',''select'') and "datatype" <> ''i'' order by ordno asc', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990032, 'F', 0, NULL, 'admin', '2025-12-24 19:34:05.000', 'admin', '2025-12-24 19:34:05.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_analyticslist', NULL, 'Metadata', 5, 'SELECT t.caption || '' ('' || t.name || '')'' AS displaydata,t.caption,t.name FROM tstructs t JOIN ax_userconfigdata u ON t.name = ANY (string_to_array(u.value, '','')) WHERE Upper(u.page) = ''ANALYTICS'' and Upper(u.keyname) = ''ENTITIES'' AND (u.username = :param1 OR u.username = ''All'')', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990033, 'F', 0, NULL, 'admin', '2025-12-23 20:50:43.000', 'admin', '2025-12-23 20:50:43.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_firesql', NULL, 'Metadata', 5, 'select * from axi_firesql_v2(:param1,:param2,:param3,:param4)', 'param1,param2,param3,param4', 'param1~~,param2~~,param3~~,param4~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990034, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_userpwd', NULL, 'Metadata', 5, 'select password  from axusers where username = :param1', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990035, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_primaryfieldlist', NULL, 'Metadata', 5, 'SELECT caption||'' (''||fname||'')'' displaydata, caption, fname name FROM axpflds WHERE tstruct = :param1 and dcname = ''dc1'' AND (modeofentry = ''autogenerate'' OR ((LOWER(allowduplicate) = ''f'' OR datatype = ''c'') AND LOWER(hidden) = ''f'')) order by ordno asc', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990036, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_structlist', NULL, 'Metadata', 5, 'select * from axi_fn_getstructlist(:param1,:param2,:param3)', 'param1,param2,param3', 'param1~~,param2~~,param3~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990037, 'F', 0, NULL, 'admin', '2025-12-23 13:35:16.000', 'admin', '2025-12-22 16:01:14.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_nongridfieldlist', NULL, 'Metadata', 5, 'select caption||'' (''||fname||'')'' displaydata, caption, fname name, tstruct,substring(modeofentry,1,1) moe,"datatype",fldsql,dcname,asgrid,listvalues fromlist,srckey normalized from axpflds where tstruct = :param1 and asgrid = ''F'' and hidden = ''F'' and modeofentry in (''accept'',''select'')and savevalue = ''T'' and "datatype" <> ''i'' order by ordno asc', 'param1', 'param1', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990038, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_structmetalist', NULL, 'Metadata', 5, 'SELECT * from fn_axi_getstructures_meta(:param1,:param2,:param3,:param4,:param5)', 'param1,param2,param3,param4,param5', 'param1~~,param2~~,param3~~,param4~~,param5~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990039, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_getstructsdata', NULL, 'Metadata', 5, 'select * from fn_axi_getstructs_obj(:param1, :param2, :param3, :param4, :param5, :param6, :param7, :param8, :param9, :param10)', 'param1,param2,param3,param4,param5,param6,param7,param8,param9,param10', 'param1~~,param2~~,param3~~,param4~~,param5~~param6~~,param7~~,param8~~,param9~~,param10~~', 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990040, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_resposibilitylist', NULL, 'Metadata', 5, 'select distinct rname displaydata, rname caption, rname name from axuseraccess order by rname', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990041, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_newsandannounce', NULL, 'Metadata', 5, 'select title as displaydata,title as caption,title as name from axpdef_news_events order by title asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990042, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_publishapi', NULL, 'Metadata', 5, 'select publickey || '' '' || ''(''||apitype||'')'' as displaydata,publickey caption,publickey name from axpdef_publishapi  order by publickey asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990043, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_jobs', NULL, 'Metadata', 5, 'select jname || '' ('' || jobid ||'')'' as displaydata, jobid as caption,jobid as name from axpdef_jobs order by jname asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990044, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_language', NULL, 'Metadata', 5, 'select language as displaydata, language as caption from axpdef_language order by language asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990045, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_customtype', NULL, 'Metadata', 5, 'select typename || '' (''||datatype||'')'' as displaydata,typename as caption,typename as name from axp_customdatatype order by typename asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990046, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_emaildef', NULL, 'Metadata', 5, 'select emaildefname || '' (''||emailwhat||'')'' as displaydata,emaildefname as caption,emaildefname as name from emaildef order by emaildefname asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990047, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_tabledesc', NULL, 'Metadata', 5, 'select dname as displaydata,dname as caption from axp_tabledescriptor order by dname asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990048, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_inbound', NULL, 'Metadata', 5, 'select axqueuename as displaydata,axqueuename as caption from AxInQueues order by axqueuename asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990049, 'F', 0, NULL, 'admin', '2025-12-23 13:22:07.000', 'admin', '2025-12-19 16:06:57.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_outbound', NULL, 'Metadata', 5, 'select axqueuename as displaydata,axqueuename as caption from AxOutQueuesmst order by axqueuename asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL)
>>

<<
INSERT INTO axdirectsql
(axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd)
VALUES(99999999990050, 'F', 0, NULL, 'admin', '2025-12-24 19:34:05.000', 'admin', '2025-12-24 19:34:05.000', NULL, 1, 1, NULL, NULL, NULL, 'axi_useractivation', NULL, 'Metadata', 5, 'select pusername as displaydata from axuseractivations order by pusername asc', NULL, NULL, 'ALL', NULL, NULL, 'F', '6 Hr', NULL, NULL, NULL);
>>