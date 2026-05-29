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

<<
CREATE TABLE axdirectsql (
    axdirectsqlid NUMBER(16) NOT NULL,
    cancel VARCHAR2(1) NULL,
    sourceid NUMBER(16) NULL,
    mapname VARCHAR2(20) NULL,
    username VARCHAR2(50) NULL,
    modifiedon TIMESTAMP NULL,
    createdby VARCHAR2(50) NULL,
    createdon TIMESTAMP NULL,
    wkid VARCHAR2(15) NULL,
    app_level NUMBER(3) NULL,
    app_desc NUMBER(1) NULL,
    app_slevel NUMBER(3) NULL,
    cancelremarks VARCHAR2(150) NULL,
    wfroles VARCHAR2(250) NULL,
    sqlname VARCHAR2(50) NULL,
    ddldatatype VARCHAR2(20) NULL,
    sqlsrc VARCHAR2(30) NULL,
    sqlsrccnd NUMBER(10) NULL,
    sqltext CLOB NULL,
    paramcal VARCHAR2(200) NULL,
    sqlparams VARCHAR2(2000) NULL,
    accessstring VARCHAR2(500) NULL,
    groupname VARCHAR2(50) NULL,
    sqlquerycols VARCHAR2(4000) NULL,
    cachedata VARCHAR2(1) NULL,
    cacheinterval VARCHAR2(10) NULL,
    encryptedflds VARCHAR2(4000) NULL,
    adsdesc CLOB NULL,
    CONSTRAINT aglaxdirectsqlid PRIMARY KEY (axdirectsqlid)
)
>>

<<
ALTER TABLE axdirectsql ADD sqlquerycols varchar(4000) NULL
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
ALTER TABLE axdirectsql ADD smartlistcnd varchar(500) NULL
>>

<<
ALTER TABLE axdirectsql ADD pagination varchar(1) NULL
>>

<<
ALTER TABLE axdirectsql ADD applydimensions varchar(1) NULL
>>

<<
CREATE TABLE axdirectsql_metadata (
    axdirectsql_metadataid NUMBER(16) NOT NULL,
    axdirectsqlid NUMBER(16) NULL,
    axdirectsql_metadatarow NUMBER(10) NULL,
    fldname VARCHAR2(100) NULL,
    fldcaption VARCHAR2(100) NULL,
    "normalized" VARCHAR2(3) NULL,
    sourcetable VARCHAR2(50) NULL,
    sourcefld VARCHAR2(50) NULL,
    CONSTRAINT aglaxdirectsql_metadataid PRIMARY KEY (axdirectsql_metadataid)
)
>>

<<
ALTER TABLE axdirectsql_metadata ADD tbl_normalizedsource VARCHAR2(2000) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD tbl_hyperlink VARCHAR2(4000) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD hyp_struct VARCHAR2(500) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD hyp_structtype VARCHAR2(20) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD hyp_transid VARCHAR2(100) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD datatypeui VARCHAR2(20) NULL
>>

<<
ALTER TABLE axdirectsql_metadata ADD fdatatype VARCHAR2(2) NULL
>>

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
        when lower(a1.sqltext) like ''%--axp_filter%'' then ''T''
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

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990001,'F',0,null,'admin',to_date('17-02-26','DD-MM-RR'),'admin',to_date('17-02-26','DD-MM-RR'),null,1,1,null,null,null,'ds_smartlist_ads_metadata',null,'Internal',1,TO_NCLOB(q'[select a.sqlname,b.fldname,b.fldcaption,b.fdatatype, b.normalized ,b.sourcetable ,b.sourcefld ,hyp_structtype,b.hyp_transid, b.tbl_hyperlink,

case when smartlistcnd like '%Dynamic select columns%' then 'T' else 'F' end dynamiccolumns,

case when smartlistcnd like '%Filter%' then coalesce(b.filter,'No') else 'F' end filters,

case when smartlistcnd like '%Pagination%' then 'T' else 'F' end pagination,

case when smartlistcnd like '%Sorting%' then 'T' else 'F' end sorting

from axdirectsql a left]')
|| TO_NCLOB(q'[ join axdirectsql_metadata b on a.axdirectsqlid =b.axdirectsqlid 

where sqlname = :adsname

order by b.axdirectsql_metadatarow ]'),'adsname','adsname~Character~','ALL',null,null,'T','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990002,'F',0,null,'admin',to_date('17-02-26','DD-MM-RR'),'admin',to_date('17-02-26','DD-MM-RR'),null,1,1,null,null,null,'ds_smartlist_filters',null,'Internal',1,'SELECT * from TABLE(fn_axpanalytics_filterdata( :ptransid, :psrctxt))','ptransid,psrctxt','ptransid~Character~,psrctxt~Character~','ALL',null,'column_value','T','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990003,'F',0,null,'admin',to_date('17-02-26','DD-MM-RR'),'admin',to_date('17-02-26','DD-MM-RR'),null,1,1,null,null,null,'ds_getsmartlists',null,'Internal',1,'select sqlname from axdirectsql a where sqlsrc=''Application''',null,null,'ALL',null,'sqlname','T','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990004,'F',0,null,'admin',to_date('17-02-26','DD-MM-RR'),'admin',to_date('17-02-26','DD-MM-RR'),null,1,1,null,null,null,'Axi_metadata_struct_obj',null,'Internal',1,'SELECT * from TABLE(fn_axi_struct_metadata( :pstructtype, :ptransid , :pobjtype ))','pstructtype,ptransid,pobjtype','pstructtype~Character~,ptransid~Character~,pobjtype~Character~','ALL',null,'objtype,objcaption,objname,dcname,asgrid','T','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990005,'F',0,null,'admin',to_date('17-02-26','DD-MM-RR'),'admin',to_date('17-02-26','DD-MM-RR'),null,1,1,null,null,null,'Axi_getmetadata',null,'Internal',1,'SELECT * FROM TABLE(fn_axi_metadata( :pstructtype , :pusername ))','pstructtype,pusername','pstructtype~Character~,pusername~Character~','ALL',null,'structtype,caption,transid','T','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990006,'F',0,null,'admin',to_date('20-05-26','DD-MM-RR'),'admin',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_formnotifylist',null,'Internal',1,'select form as displaydata, stransid name from axformnotify',null,null,'ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990007,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_pegnotifylist',null,'Internal',1,'select name as displaydata from axnotificationdef',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990008,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_publishapi',null,'Internal',1,'select publickey || '' '' || ''(''||apitype||'')'' as displaydata,publickey caption,publickey name from axpdef_publishapi  order by publickey asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990009,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_servernamelist',null,'Internal',1,'select servername as displaydata from dwb_publishprops',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990010,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_usergrouplist',null,'Internal',1,'select users_group_name as displaydata from axpdef_usergroups',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990011,'F',0,null,'admin',to_date('09-11-23','DD-MM-RR'),'admin',to_date('09-11-23','DD-MM-RR'),null,1,1,0,null,null,'Text_Field_Intelligence',null,'Internal',1,TO_NCLOB(q'[select id,caption,source from(
select fname id,caption,'Form' source,2 ord from axpflds where asgrid='F' and tstruct = :txttransid 
union all
select db_varname,db_varcaption,'Axvars' ,3 ord from axpdef_axvars_dbvar a,axpdef_axvars b
where b.axpdef_axvarsid=a.axpdef_axvarsid 
union all
select 'username','Login username','App vars' ,4 ord from dual
union all
select 'usergroup','User role','App vars' ,4 ord from dual
union all
select fname,caption,'Glovar',5 ord from axpflds where tstruct='axglo'
o]')
|| TO_NCLOB(q'[rder by 4,1)a
]'),'txttransid','txttransid','ALL',null,null,'T','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990012,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_apinameslist',null,'Internal',1,'select execapidefname as displaydata from executeapidef',null,null,'ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990013,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_cardlist',null,'Internal',1,'select cardname as displaydata from axp_cards',null,null,'ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990014,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_customtype',null,'Internal',1,'select typename || '' (''||datatype||'')'' as displaydata,typename as caption,typename as name from axp_customdatatype order by typename asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990015,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_dimensionlist',null,'Internal',1,'select grpcaption ||'' ('' || grpname ||'')'' as displaydata, grpname as caption,grpname as name from axgroupingmst order by grpcaption asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990016,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_emaildef',null,'Internal',1,'select emaildefname || '' (''||emailwhat||'')'' as displaydata,emaildefname as caption,emaildefname as name from emaildef order by emaildefname asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990017,'F',0,null,'admin',to_date('20-05-26','DD-MM-RR'),'admin',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_firesql',null,'Internal',1,'SELECT * FROM TABLE(ISTF1415.axi_firesql_v2( :param1, :param2, :param3, :param4))','param1,param2,param3,param4','param1~~,param2~~,param3~~,param4~~','ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990018,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_inbound',null,'Internal',1,'select axqueuename as displaydata,axqueuename as caption from AxInQueues order by axqueuename asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990019,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_jobs',null,'Internal',1,'select jname || '' ('' || jobid ||'')'' as displaydata, jobid as caption,jobid as name from axpdef_jobs order by jname asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990020,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_outbound',null,'Internal',1,'select axqueuename as displaydata, axqueuename as caption from AxOutQueuesmst order by axqueuename asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990021,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_adsfilteroperators',null,'Internal',1,'SELECT ''='' AS displaydata, ''='' AS name UNION ALL SELECT ''<'',''<'' UNION ALL SELECT ''>'',''>'' UNION ALL SELECT ''<='',''<='' UNION ALL SELECT ''>='',''>='' UNION ALL SELECT ''between'',''between''',null,null,'ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990022,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_language',null,'Internal',1,'select language as displaydata, language as caption from axpdef_language order by language asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990023,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_nongridfieldlist',null,'Internal',1,'select caption||'' (''||fname||'')'' displaydata, caption, fname name, 
tstruct,SUBSTR(modeofentry, 1, 1) AS moe, axpflds.datatype ,fldsql,dcname,asgrid,listvalues fromlist,srckey normalized
 from axpflds where tstruct = :param1 and asgrid = ''F'' and hidden = ''F'' and modeofentry in (''accept'',''select'') 
 and savevalue = ''T'' and axpflds.datatype <> ''i'' order by ordno ASC','param1','param1~Character~','ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990024,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_peglist',null,'Internal',1,'select caption as displaydata from axpdef_peg_processmaster',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990025,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_primaryfieldlist',null,'Internal',1,'SELECT caption||'' (''||fname||'')'' displaydata, caption, fname name FROM axpflds WHERE tstruct = :param1 and dcname = ''dc1'' AND (modeofentry = ''autogenerate'' OR ((LOWER(allowduplicate) = ''f'' OR datatype = ''c'') AND LOWER(hidden) = ''f'')) order by ordno asc','param1','param1~Character~','ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990026,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_resposibilitylist',null,'Internal',1,'select distinct rname displaydata, rname caption, rname name from axuseraccess order by rname',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990027,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_rolelist',null,'Internal',1,'select groupname as displaydata from axusergroups',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990028,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_rulenameslist',null,'Internal',1,'select rulename as displaydata from axpdef_ruleeng',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990029,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_schedulenotifylist',null,'Internal',1,'select name as displaydata from axperiodnotify',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990030,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_setfieldlist',null,'Internal',1,'select caption||'' (''||fname||'')'' displaydata, caption, fname name, tstruct,substring(modeofentry,1,1) moe, axpflds.datatype,fldsql sql 
from axpflds where tstruct = :param1 
and dcname = ''dc1'' and hidden = ''F'' and savevalue = ''T'' and modeofentry in (''accept'',''select'') and axpflds.datatype <> ''i'' order by ordno asc','param1','param1~Character~','ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990031,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_structlist',null,'Internal',1,'select * from axi_fn_getstructlist(:param1,:param2,:param3)','param1,param2,param3','param1~Character~,param2~Character~,param3~Character~','ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990032,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_structmetalist',null,'Internal',1,'SELECT * from fn_axi_getstructures_meta(:param1,:param2,:param3,:param4,:param5)','param1,param2,param3,param4,param5','param1~Character~,param2~Character~,param3~Character~,param4~Character~,param5~Character~','ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990033,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_tabledesc',null,'Internal',1,'select dname as displaydata,dname as caption from axp_tabledescriptor order by dname asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990034,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_useractivation',null,'Internal',1,'select pusername as displaydata from axuseractivations order by pusername asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990035,'F',0,null,'admin',to_date('21-05-26','DD-MM-RR'),'admin',to_date('21-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_userlist',null,'Internal',1,'select username as displaydata from axusers',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990036,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_actorlist',null,'Internal',1,'select actorname as displaydata from axpdef_peg_actor',null,null,'ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990037,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_adscolumnlist',null,'Internal',1,'select b.fldcaption || ''(''||b.fldname||'')'' displaydata,b.fldname name,b.fldcaption caption,b.normalized,b.fdatatype, b.sourcetable,b.sourcefld , CASE WHEN lower(sqltext) LIKE ''%--axp_filter%'' THEN ''T'' ELSE ''F'' END AS filters from axdirectsql a left join axdirectsql_metadata b on a.axdirectsqlid =b.axdirectsqlid where sqlname = :param1','param1','param1~Character~','ALL',null,null,'F','6 Hr',null,null,null,null,null)
>>

<<
Insert into AXDIRECTSQL (AXDIRECTSQLID,CANCEL,SOURCEID,MAPNAME,USERNAME,MODIFIEDON,CREATEDBY,CREATEDON,WKID,APP_LEVEL,APP_DESC,APP_SLEVEL,CANCELREMARKS,WFROLES,SQLNAME,DDLDATATYPE,SQLSRC,SQLSRCCND,SQLTEXT,PARAMCAL,SQLPARAMS,ACCESSSTRING,GROUPNAME,SQLQUERYCOLS,CACHEDATA,CACHEINTERVAL,ENCRYPTEDFLDS,ADSDESC,SMARTLISTCND,PAGINATION,APPLYDIMENSIONS) values (99999999990038,'F',0,null,'rekhancia',to_date('20-05-26','DD-MM-RR'),'rekhancia',to_date('20-05-26','DD-MM-RR'),null,1,1,null,null,null,'axi_newsandannounce',null,'Internal',1,'select title as displaydata,title as caption,title as name from axpdef_news_events order by title asc',null,null,'ALL',null,null,'F','6 Hr',null,null,null,'T','F')
>>