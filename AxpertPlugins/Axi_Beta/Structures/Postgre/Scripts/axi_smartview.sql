<<
delete from axdirectsql where sqlname='ds_smartlist_filters';
>>

<<
delete from axdirectsql where sqlname='ds_smartlist_kpicharts';
>>


<<
delete from axdirectsql where sqlname='ds_smartlist_ads_metadata';
>>


<<
delete from axdirectsql where sqlname='ds_getsmartlists';
>>


<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd, pagination, applydimensions) VALUES(8887330000001, 'F', 0, NULL, 'admin', '2026-05-29 14:40:09.000', 'abinash', '2026-02-04 00:00:00.000', NULL, 1, 1, NULL, NULL, NULL, 'ds_smartlist_filters', NULL, 'Internal', 1, 'SELECT * from fn_axpanalytics_filterdata( null, :psrctxt)', 'psrctxt', 'psrctxt~Character~', 'ALL', NULL, 'datavalue', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd, pagination, applydimensions) VALUES(8887990000000, 'F', 0, NULL, 'admin', '2026-05-29 13:02:16.000', 'admin', '2026-05-22 16:16:33.000', NULL, 1, 1, NULL, NULL, NULL, 'ds_smartlist_kpicharts', NULL, 'Internal', 1, 'select b.kpicaption chartcaption,''KPI'' charttype,null grpcol,kpi_aggfunc agg_func,kpi_aggcol agg_col,1 ord,b.axpdef_smartlist_kpirow ord2 from axpdef_smartlist a
join axpdef_smartlist_kpi b on a.axpdef_smartlistid = b.axpdef_smartlistid
where a.adsname = :adsname
union all
select b.chartcaption,b.charttype,b.chart_grpcol,chart_aggfun,chart_aggcol,2 ord,b.axpdef_smartlist_chartsrow from axpdef_smartlist a
join axpdef_smartlist_charts b on a.axpdef_smartlistid = b.axpdef_smartlistid
where a.adsname = :adsname
order by ord,ord2', 'adsname', 'adsname', 'ALL', NULL, 'chartcaption,charttype,grpcol,agg_func,agg_col,ord,ord2', 'F', '6 Hr', NULL, NULL, NULL, 'T', 'F');
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd, pagination, applydimensions) VALUES(8881440000000, 'F', 0, NULL, 'admin', '2026-06-03 18:32:20.000', 'admin', '2026-01-30 00:00:00.000', NULL, 1, 1, NULL, NULL, NULL, 'ds_smartlist_ads_metadata', NULL, 'Internal', 1, 'select distinct a.sqlname,b.fldname,b.fldcaption,b.fdatatype, b."normalized" ,
f.tablename sourcetable ,b.srcfldname sourcefld ,hyp_structtype,hl.hyp_transid, REGEXP_REPLACE(hl.tbl_hyperlink, ''^.*?\((.*?)\)'', ''\1'')::text tbl_hyperlink,hl.hyp_inline,
case when smartlistcnd like ''%Dynamic select columns%'' then ''T'' else ''F'' end dynamiccolumns,
 coalesce(b.filter,''No'') filters,
case when smartlistcnd like ''%Pagination%'' then ''T'' else ''F'' end pagination,
case when smartlistcnd like ''%Sorting%'' then ''T'' else ''F'' end sorting,b.srctransid ,b.srcfldname srcfld ,b.srcfldname,
b.axpdef_smartlist_mdatarow,b.hide hide
from axdirectsql a 
left join axpdef_smartlist c on a.sqlname = c.adsname
left join axpdef_smartlist_mdata b on b.axpdef_smartlistid =c.axpdef_smartlistid
left join axpflds f on b.srctransid = f.tstruct and b.srcfldname = f.fname
left join(select axpdef_smartlistid,hfldname,hyp_structtype,hyp_transid, tbl_hyperlink,hyp_inline from axpdef_smartlist_hlink)hl 
on hl.axpdef_smartlistid=c.axpdef_smartlistid and b.fldname = hl.hfldname
where sqlname = :adsname
order by b.axpdef_smartlist_mdatarow', 'adsname', 'adsname~Character~', 'ALL', NULL, 'sqlname,fldname,fldcaption,normalized,sourcetable,sourcefld,hyp_structtype,hyp_transid,tbl_hyperlink,hyp_inline,dynamiccolumns,filters,pagination,sorting', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
>>

<<
INSERT INTO axdirectsql (axdirectsqlid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, sqlname, ddldatatype, sqlsrc, sqlsrccnd, sqltext, paramcal, sqlparams, accessstring, groupname, sqlquerycols, cachedata, cacheinterval, encryptedflds, adsdesc, smartlistcnd, pagination, applydimensions) VALUES(8887330000000, 'F', 0, NULL, 'admin', '2026-05-29 13:01:03.000', 'admin', '2025-12-23 00:00:00.000', NULL, 1, 1, NULL, NULL, NULL, 'ds_getsmartlists', NULL, 'Internal', 1, 'select sqlname from axdirectsql a where sqlsrccnd=3', NULL, NULL, 'ALL', NULL, 'sqlname', NULL, NULL, NULL, NULL, NULL, NULL, NULL);
>>