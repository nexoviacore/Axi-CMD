<<
delete from axdirectsql where sqlname='ds_smartlist_filters'
>>

<<
delete from axdirectsql where sqlname='ds_smartlist_kpicharts'
>>

<<
delete from axdirectsql where sqlname='ds_smartlist_ads_metadata'
>>

<<
delete from axdirectsql where sqlname='ds_getsmartlists'
>>

<<
ALTER TABLE axdirectsql ADD pagination VARCHAR2(1) NULL
>>

<<
ALTER TABLE axdirectsql ADD applydimensions VARCHAR2(1) NULL
>>


<<
INSERT INTO AXDIRECTSQL (AXDIRECTSQLID, CANCEL, SOURCEID, MAPNAME, USERNAME, MODIFIEDON, CREATEDBY, CREATEDON, WKID, APP_LEVEL, APP_DESC, APP_SLEVEL, CANCELREMARKS, WFROLES, SQLNAME, DDLDATATYPE, SQLTEXT, PARAMCAL, SQLPARAMS, ACCESSSTRING, GROUPNAME, SQLSRC, SQLSRCCND, SQLQUERYCOLS, ENCRYPTEDFLDS, CACHEDATA, CACHEINTERVAL, SMARTLISTCND, ADSDESC, PAGINATION, APPLYDIMENSIONS) VALUES(7420440000000, 'F', 0, NULL, 'admin', TIMESTAMP '2026-05-19 19:05:04.000000', 'admin', TIMESTAMP '2026-05-19 19:05:04.000000', NULL, 1, 1, NULL, NULL, NULL, 'ds_smartlist_filters', NULL, 'SELECT * from table(fn_axpanalytics_filterdata( :ptransid, :psrctxt))', 'ptransid,psrctxt', 'ptransid~Character~,psrctxt~Character~', 'ALL', NULL, 'For developers', 2, NULL, NULL, 'F', '6 Hr', NULL, NULL, 'T', 'F')
>>

<<
INSERT INTO AXDIRECTSQL (AXDIRECTSQLID, CANCEL, SOURCEID, MAPNAME, USERNAME, MODIFIEDON, CREATEDBY, CREATEDON, WKID, APP_LEVEL, APP_DESC, APP_SLEVEL, CANCELREMARKS, WFROLES, SQLNAME, DDLDATATYPE, SQLTEXT, PARAMCAL, SQLPARAMS, ACCESSSTRING, GROUPNAME, SQLSRC, SQLSRCCND, SQLQUERYCOLS, ENCRYPTEDFLDS, CACHEDATA, CACHEINTERVAL, SMARTLISTCND, ADSDESC, PAGINATION, APPLYDIMENSIONS) VALUES(7420550000000, 'F', 0, NULL, 'admin', TIMESTAMP '2026-05-19 19:05:48.000000', 'admin', TIMESTAMP '2026-05-19 19:05:48.000000', NULL, 1, 1, NULL, NULL, NULL, 'ds_smartlist_kpicharts', NULL, 'select b.kpicaption chartcaption,''KPI'' charttype,null grpcol,kpi_aggfunc agg_func,kpi_aggcol agg_col,1 ord,b.axpdef_smartlist_kpirow ord2 from axpdef_smartlist a
join axpdef_smartlist_kpi b on a.axpdef_smartlistid = b.axpdef_smartlistid
where a.adsname = :adsname
union all
select b.chartcaption,b.charttype,b.chart_grpcol,chart_aggfun,chart_aggcol,2 ord,b.axpdef_smartlist_chartsrow from axpdef_smartlist a
join axpdef_smartlist_charts b on a.axpdef_smartlistid = b.axpdef_smartlistid
where a.adsname = :adsname
order by ord,ord2', 'adsname', 'adsname~Character~', 'ALL', NULL, 'For developers', 2, NULL, NULL, 'F', '6 Hr', NULL, NULL, 'T', 'F')
>>

<<
INSERT INTO AXDIRECTSQL (AXDIRECTSQLID, CANCEL, SOURCEID, MAPNAME, USERNAME, MODIFIEDON, CREATEDBY, CREATEDON, WKID, APP_LEVEL, APP_DESC, APP_SLEVEL, CANCELREMARKS, WFROLES, SQLNAME, DDLDATATYPE, SQLTEXT, PARAMCAL, SQLPARAMS, ACCESSSTRING, GROUPNAME, SQLSRC, SQLSRCCND, SQLQUERYCOLS, ENCRYPTEDFLDS, CACHEDATA, CACHEINTERVAL, SMARTLISTCND, ADSDESC, PAGINATION, APPLYDIMENSIONS) VALUES(7420330000000, 'F', 0, NULL, 'admin', TIMESTAMP '2026-05-19 19:04:10.000000', 'admin', TIMESTAMP '2026-05-19 19:04:10.000000', NULL, 1, 1, NULL, NULL, NULL, 'ds_getsmartlists', NULL, 'select sqlname from axdirectsql a where sqlsrccnd=3', NULL, NULL, 'ALL', NULL, 'For developers', 2, NULL, NULL, 'F', '6 Hr', NULL, NULL, 'T', 'F')
>>

<<
INSERT INTO AXDIRECTSQL (AXDIRECTSQLID, CANCEL, SOURCEID, MAPNAME, USERNAME, MODIFIEDON, CREATEDBY, CREATEDON, WKID, APP_LEVEL, APP_DESC, APP_SLEVEL, CANCELREMARKS, WFROLES, SQLNAME, DDLDATATYPE, SQLTEXT, PARAMCAL, SQLPARAMS, ACCESSSTRING, GROUPNAME, SQLSRC, SQLSRCCND, SQLQUERYCOLS, ENCRYPTEDFLDS, CACHEDATA, CACHEINTERVAL, SMARTLISTCND, ADSDESC, PAGINATION, APPLYDIMENSIONS) VALUES(7421660000000, 'F', 0, NULL, 'admin', TIMESTAMP '2026-05-19 19:42:40.000000', 'admin', TIMESTAMP '2026-05-19 19:42:40.000000', NULL, 1, 1, NULL, NULL, NULL, 'ds_smartlist_ads_metadata', NULL, 'SELECT 
    a.adsname AS sqlname,
    b.fldname,
    b.fldcaption,
    b.fdatatype, 
    b.normalized,
    f.tablename AS sourcetable,
    f.fname AS sourcefld,
    hl.hyp_structtype,
    hl.hyp_transid, 
    REGEXP_REPLACE(
    hl.tbl_hyperlink, 
    ''[A-Za-z0-9 *-]+-\(([^)]+)\)'', 
    ''\1''
) AS tbl_hyperlink,
    hl.hyp_inline,
    ''T'' AS dynamiccolumns,
    ''T'' AS filters,
    ''T'' AS pagination,
    ''T'' AS sorting,
    a.allowedit,
    a.newforms,
    a.newforms_transid,
    b.hide AS col_hide,
    b.filter AS col_filter,
    a.bulksave,
    b.keyfield 
FROM axpdef_smartlist a 
JOIN axpdef_smartlist_mdata b ON a.axpdef_smartlistid = b.axpdef_smartlistid
LEFT JOIN axpflds f ON b.srctransid = f.tstruct AND b.srcfldname = f.fname 
LEFT JOIN (
    SELECT axpdef_smartlistid, hfldname, hyp_structtype, hyp_transid, tbl_hyperlink, hyp_inline 
    FROM axpdef_smartlist_hlink
) hl ON hl.axpdef_smartlistid = a.axpdef_smartlistid AND b.fldname = hl.hfldname
WHERE a.adsname = :adsname
ORDER BY b.axpdef_smartlist_mdatarow', 'adsname', 'adsname~Character~', NULL, NULL, 'For developers', 2, NULL, NULL, NULL, NULL, NULL, NULL, 'T', NULL)
>>