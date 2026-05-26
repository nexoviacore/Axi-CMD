<<
UPDATE AXDIRECTSQL SET SQLSRC =CASE sqlsrc WHEN 'Application' THEN 'For users' WHEN 'Metadata' THEN 'Internal'
ELSE 'For users' end,SQLSRCCND =CASE sqlsrc WHEN 'Application' THEN 3 WHEN 'Metadata' THEN 1
ELSE 3  END
>>

<<
CREATE OR REPLACE TYPE axi_getstructs_obj AS OBJECT (
    displaydata   CLOB,
    id            CLOB,
    caption       CLOB,
    isfield       CLOB
)
>>

<<
CREATE OR REPLACE TYPE axi_getstructs_obj_tbl AS TABLE OF axi_getstructs_obj
>>

<<
CREATE OR REPLACE TYPE AXI_GETSTRUCTURES_META AS OBJECT (
    displaydata    VARCHAR2(4000),
    caption        VARCHAR2(4000),
    name           VARCHAR2(4000),
    stype          VARCHAR2(4000),
    dimension      VARCHAR2(4000),
    permission     VARCHAR2(4000),
    createallowed  VARCHAR2(4000),
    viewallowed    VARCHAR2(4000),
    keyfield       VARCHAR2(4000),
    primarytable   VARCHAR2(4000)
)
>>

<<
CREATE OR REPLACE TYPE AXI_GETSTRUCTURES_META_tbl AS TABLE OF AXI_GETSTRUCTURES_META
>>

<<
CREATE OR REPLACE FUNCTION fn_axi_getstructs_obj (
    pcmd            VARCHAR2,
    pusername       VARCHAR2,
    puserrole       VARCHAR2,
    ptransid        VARCHAR2,
    pselectedfield  VARCHAR2,
    pdimension      VARCHAR2,
    ppermission     VARCHAR2,
    pkeyfield       VARCHAR2,
    pprimarytable   VARCHAR2,
    pglobalvars     VARCHAR2
)
RETURN AXI_GETSTRUCTS_OBJ_TBL
PIPELINED
AS

    v_sql                       CLOB;

    v_keyfield_normalized       VARCHAR2(10);
    v_keyfield_srctbl           VARCHAR2(200);
    v_keyfield_srcfld           VARCHAR2(200);

    v_selectedfld_normalized    VARCHAR2(10);
    v_selectedfld_srctbl        VARCHAR2(200);
    v_selectedfld_srcfld        VARCHAR2(200);

    v_dimension_filter          CLOB;

    v_includedcomps             CLOB;
    v_excludedcomps             CLOB;

    v_viewctrl                  VARCHAR2(10);
    v_editctrl                  VARCHAR2(10);
    v_fullcontrol               VARCHAR2(10);

    v_fieldlist_sql             CLOB;
    v_keyfield_sql              CLOB;
    v_selectedfld_sql           CLOB;

    TYPE refcur IS REF CURSOR;

    rc refcur;

    r_displaydata   CLOB;
    r_id            CLOB;
    r_caption       CLOB;
    r_isfield       CLOB;

BEGIN

    SELECT srckey,
           LOWER(srctf),
           LOWER(srcfld)
    INTO v_keyfield_normalized,
         v_keyfield_srctbl,
         v_keyfield_srcfld
    FROM axpflds
    WHERE tstruct = ptransid
      AND LOWER(fname) = LOWER(pkeyfield);

    BEGIN

        SELECT srckey,
               LOWER(srctf),
               LOWER(srcfld)
        INTO v_selectedfld_normalized,
             v_selectedfld_srctbl,
             v_selectedfld_srcfld
        FROM axpflds
        WHERE tstruct = ptransid
          AND LOWER(fname) = LOWER(pselectedfield);

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            v_selectedfld_normalized := 'F';
            v_selectedfld_srctbl := NULL;
            v_selectedfld_srcfld := NULL;
    END;

    IF pdimension = 'T' THEN

        BEGIN

            SELECT filtercnd
            INTO v_dimension_filter
            FROM TABLE(
                fn_permissions_getpermission(
                    'axi',
                    ptransid,
                    pusername,
                    puserrole,
                    pglobalvars
                )
            )
            WHERE ROWNUM = 1;

            IF LENGTH(v_dimension_filter) > 2 THEN

                v_dimension_filter :=
                    ' AND ' ||
                    REPLACE(
                        v_dimension_filter,
                        '{primarytable.}',
                        'p.'
                    );

            END IF;

        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                v_dimension_filter := NULL;
        END;

    END IF;

    IF ppermission = 'T' THEN

        BEGIN

            SELECT LOWER(view_includedc || view_includeflds),
                   LOWER(view_excludedc || view_excludeflds),
                   viewctrl,
                   editctrl,
                   fullcontrol
            INTO v_includedcomps,
                 v_excludedcomps,
                 v_viewctrl,
                 v_editctrl,
                 v_fullcontrol
            FROM TABLE(
                fn_permissions_getpermission(
                    'axi',
                    ptransid,
                    pusername,
                    puserrole,
                    pglobalvars
                )
            )
            WHERE ROWNUM = 1;

        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                v_viewctrl := '0';
                v_fullcontrol := 'T';
        END;

        IF v_fullcontrol = 'T'
           OR v_viewctrl = '0' THEN

            v_fieldlist_sql :=
                   'SELECT '
                || '(caption || '' ('' || fname || '') [field]'') displaydata, '
                || '''0'' id, '
                || 'caption, '
                || '''t'' isfield '
                || 'FROM axpflds '
                || 'WHERE tstruct = '''
                || ptransid || ''' '
                || 'AND dcname = ''dc1'' '
                || 'AND hidden = ''F'' '
                || 'AND savevalue = ''T''';

        ELSIF NVL(v_fullcontrol,'F') = 'F'
              AND v_viewctrl = '1' THEN

            v_fieldlist_sql :=
                   'SELECT '
                || '(caption || '' ('' || fname || '') [field]'') displaydata, '
                || '''0'' id, '
                || 'caption, '
                || '''t'' isfield '
                || 'FROM axpflds '
                || 'WHERE tstruct = '''
                || ptransid || ''' '
                || 'AND dcname = ''dc1'' '
                || 'AND hidden = ''F'' '
                || 'AND savevalue = ''T'' '
                || 'AND LOWER(fname) IN ('''
                || REPLACE(v_includedcomps, ',', ''',''')
                || ''')';

        ELSIF NVL(v_fullcontrol,'F') = 'F'
              AND v_viewctrl = '2' THEN

            v_fieldlist_sql :=
                   'SELECT '
                || '(caption || '' ('' || fname || '') [field]'') displaydata, '
                || '''0'' id, '
                || 'caption, '
                || '''t'' isfield '
                || 'FROM axpflds '
                || 'WHERE tstruct = '''
                || ptransid || ''' '
                || 'AND dcname = ''dc1'' '
                || 'AND hidden = ''F'' '
                || 'AND savevalue = ''T'' '
                || 'AND LOWER(fname) NOT IN ('''
                || REPLACE(v_excludedcomps, ',', ''',''')
                || ''')';

        END IF;

    ELSE

        v_fieldlist_sql :=
               'SELECT '
            || '(caption || '' ('' || fname || '') [field]'') displaydata, '
            || '''0'' id, '
            || 'caption, '
            || '''t'' isfield '
            || 'FROM axpflds '
            || 'WHERE tstruct = '''
            || ptransid || ''' '
            || 'AND dcname = ''dc1'' '
            || 'AND hidden = ''F'' '
            || 'AND savevalue = ''T''';

    END IF;

    IF v_keyfield_normalized = 'T' THEN

        v_keyfield_sql :=
               'SELECT '
            || 's.' || v_keyfield_srcfld || ' displaydata, '
            || '''0'' id, '
            || 's.' || v_keyfield_srcfld || ' caption, '
            || '''f'' isfield '
            || 'FROM ' || LOWER(pprimarytable) || ' p '
            || 'JOIN ' || v_keyfield_srctbl || ' s '
            || 'ON p.' || LOWER(pkeyfield)
            || ' = s.' || v_keyfield_srctbl || 'id '
            || 'WHERE p.' || LOWER(pkeyfield)
            || ' IS NOT NULL '
            || NVL(v_dimension_filter,'')
            || ' ORDER BY p.modifiedon DESC';

    ELSE

        v_keyfield_sql :=
               'SELECT '
            || 'p.' || LOWER(pkeyfield) || ' displaydata, '
            || '''0'' id, '
            || 'p.' || LOWER(pkeyfield) || ' caption, '
            || '''f'' isfield '
            || 'FROM ' || LOWER(pprimarytable) || ' p '
            || 'WHERE p.' || LOWER(pkeyfield)
            || ' IS NOT NULL '
            || NVL(v_dimension_filter,'')
            || ' ORDER BY p.modifiedon DESC';

    END IF;

    IF pselectedfield <> '0' THEN
        IF v_selectedfld_normalized = 'T' THEN
            IF v_keyfield_normalized = 'F' THEN

                v_selectedfld_sql :=
                       'SELECT '
                    || '(s.' || v_selectedfld_srcfld
                    || ' || ''['' || p.' || LOWER(pkeyfield)
                    || ' || '']'') displaydata, '
                    || '''0'' id, '
                    || 's.' || v_selectedfld_srcfld || ' caption, '
                    || '''f'' isfield '
                    || 'FROM ' || LOWER(pprimarytable) || ' p '
                    || 'JOIN ' || v_selectedfld_srctbl || ' s '
                    || 'ON p.' || LOWER(pselectedfield)
                    || ' = s.' || v_selectedfld_srctbl || 'id '
                    || 'WHERE p.' || LOWER(pselectedfield)
                    || ' IS NOT NULL '
                    || NVL(v_dimension_filter,'')
                    || ' ORDER BY p.modifiedon DESC';

            ELSIF v_keyfield_normalized = 'T' THEN

                v_selectedfld_sql :=
                       'SELECT '
                    || '(s.' || v_selectedfld_srcfld
                    || ' || ''['' || k.' || v_keyfield_srcfld
                    || ' || '']'') displaydata, '
                    || '''0'' id, '
                    || 's.' || v_selectedfld_srcfld || ' caption, '
                    || '''f'' isfield '
                    || 'FROM ' || LOWER(pprimarytable) || ' p '
                    || 'JOIN ' || v_selectedfld_srctbl || ' s '
                    || 'ON p.' || LOWER(pselectedfield)
                    || ' = s.' || v_selectedfld_srctbl || 'id '
                    || 'JOIN ' || v_keyfield_srctbl || ' k '
                    || 'ON p.' || LOWER(pkeyfield)
                    || ' = k.' || v_keyfield_srctbl || 'id '
                    || 'WHERE p.' || LOWER(pselectedfield)
                    || ' IS NOT NULL '
                    || NVL(v_dimension_filter,'')
                    || ' ORDER BY p.modifiedon DESC';

            END IF;

        ELSE

            IF v_keyfield_normalized = 'F' THEN

                v_selectedfld_sql :=
                       'SELECT '
                    || '(p.' || LOWER(pselectedfield)
                    || ' || ''['' || p.' || LOWER(pkeyfield)
                    || ' || '']'') displaydata, '
                    || '''0'' id, '
                    || 'p.' || LOWER(pselectedfield) || ' caption, '
                    || '''f'' isfield '
                    || 'FROM ' || LOWER(pprimarytable) || ' p '
                    || 'WHERE p.' || LOWER(pselectedfield)
                    || ' IS NOT NULL '
                    || NVL(v_dimension_filter,'')
                    || ' ORDER BY p.modifiedon DESC, '
                    || 'p.' || LOWER(pselectedfield);


            ELSIF v_keyfield_normalized = 'T' THEN

                v_selectedfld_sql :=
                       'SELECT '
                    || '(p.' || LOWER(pselectedfield)
                    || ' || ''['' || s.' || v_keyfield_srcfld
                    || ' || '']'') displaydata, '
                    || '''0'' id, '
                    || 'p.' || LOWER(pselectedfield) || ' caption, '
                    || '''f'' isfield '
                    || 'FROM ' || LOWER(pprimarytable) || ' p '
                    || 'JOIN ' || v_keyfield_srctbl || ' s '
                    || 'ON p.' || LOWER(pkeyfield)
                    || ' = s.' || v_keyfield_srctbl || 'id '
                    || 'WHERE p.' || LOWER(pselectedfield)
                    || ' IS NOT NULL '
                    || NVL(v_dimension_filter,'')
                    || ' ORDER BY p.modifiedon DESC';

            END IF;

        END IF;

    END IF;

    IF pselectedfield = '0' THEN

        v_sql :=
               'SELECT * FROM ('
            || v_keyfield_sql
            || ') UNION ALL '
            || v_fieldlist_sql;

    ELSE

        v_sql := v_selectedfld_sql;

    END IF;

    OPEN rc FOR v_sql;

    LOOP

        FETCH rc
        INTO r_displaydata,
             r_id,
             r_caption,
             r_isfield;

        EXIT WHEN rc%NOTFOUND;

        PIPE ROW (
            AXI_GETSTRUCTS_OBJ(
                r_displaydata,
                r_id,
                r_caption,
                r_isfield
            )
        );

    END LOOP;

    CLOSE rc;

    RETURN;

END
>>

<<
CREATE OR REPLACE FUNCTION fn_axi_getstructures_meta(
    pusername        IN VARCHAR2, 
    puserrole        IN VARCHAR2, 
    presponsiblity   IN VARCHAR2, 
    pmode            IN VARCHAR2, 
    pstype           IN VARCHAR2
) RETURN AXI_GETSTRUCTURES_META_tbl PIPELINED 
AS    
    type t_cursor IS REF CURSOR;
    v_cursor t_cursor;

    v_displaydata   VARCHAR2(4000);
    v_caption       VARCHAR2(4000);
    v_name          VARCHAR2(4000);
    v_stype         VARCHAR2(4000);
    v_dimension     VARCHAR2(4000);
    v_permission    VARCHAR2(4000);
    v_createallowed VARCHAR2(4000);
    v_viewallowed   VARCHAR2(4000);
    v_keyfield      VARCHAR2(4000);
    v_primarytable  VARCHAR2(4000);
BEGIN

   
        OPEN v_cursor FOR
        WITH ts AS(SELECT DISTINCT
               CAST((a.caption || ' (' || a.name || ') [tstruct]') AS VARCHAR2(4000)) AS displaydata,
               a.caption AS tcaption,
               a.name AS transid,
               CAST('t' AS VARCHAR2(4000)) AS stype,
               CAST(COALESCE(g.dimensions, 'F') AS VARCHAR2(4000)) AS dimensions,
               CAST(COALESCE(p.permissions, 'F') AS VARCHAR2(4000)) AS permissions,
               CAST(COALESCE(p.newrecord, 'T') AS VARCHAR2(4000)) AS createallowed,
               CAST(COALESCE(p.viewctrl, 'T') AS VARCHAR2(4000)) AS viewallowed,
               kf.kfld,
               d.tablename
        FROM tstructs a
        JOIN axpdc d ON a.name = d.tstruct AND d.dname = 'dc1'
        LEFT JOIN axpages b ON b.pagetype = 't' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name
        LEFT JOIN (
            SELECT name AS tstruct, kfld 
            FROM (
                SELECT combined_results.name, combined_results.kfld,
                       ROW_NUMBER() OVER (PARTITION BY combined_results.name ORDER BY combined_results.ord ASC) as rn
                FROM (
                    SELECT a.name, a.keyfield AS kfld, 1 AS ord       
                    FROM axp_tstructprops a 
                    UNION ALL
                    SELECT tstruct AS name, fname AS kfld,
                        CASE 
                            WHEN modeofentry = 'autogenerate' THEN 2
                            WHEN LOWER(allowduplicate) = 'f' AND LOWER(allowempty) = 'f' AND datatype = 'c' AND LOWER(hidden) = 'f' THEN 3
                            WHEN LOWER(hidden) = 'f' AND datatype = 'c' THEN 4
                            ELSE 5
                        END AS ord       
                    FROM axpflds
                    WHERE dcname = 'dc1'
                ) combined_results
            ) WHERE rn = 1
        ) kf ON kf.tstruct = a.name
        LEFT JOIN (SELECT ftransid, 'T' AS dimensions FROM axgrouptstructs) g ON a.name = g.ftransid
        LEFT JOIN (
            SELECT formtransid, newrecord, permissions, viewctrl
            FROM (
                SELECT formtransid, newrecord, permissions, viewctrl,
                       ROW_NUMBER() OVER (PARTITION BY formtransid ORDER BY ord ASC) AS rn
                FROM (
                    SELECT formtransid, CASE WHEN allowcreate = 'Yes' THEN 'T' else 'F' END as newrecord, 'U' as type, 1 as ord, 'T' as permissions, CASE WHEN viewctrl = '4' THEN 'F' else 'T' END as viewctrl
                    FROM axpermissions
                    WHERE axusername = pusername AND comptype = 'Form'
                    UNION ALL
                    SELECT formtransid, CASE WHEN allowcreate = 'Yes' THEN 'T' else 'F' END as newrecord, 'R' as type, 2 as ord, 'T' as permissions, CASE WHEN viewctrl = '4' THEN 'F' else 'T' END as viewctrl
                    FROM axpermissions
                    WHERE REGEXP_LIKE(',' || puserrole || ',', ',' || axuserrole || ',') AND comptype = 'Form'
                ) combined_permissions
            ) WHERE rn = 1
        ) p ON a.name = p.formtransid
        WHERE (pmode = 'dev' OR b.visible = 'T')
          AND (
                REGEXP_LIKE(',' || presponsiblity || ',', ',default,')
                OR (ua.stype = 't' AND REGEXP_LIKE(',' || presponsiblity || ',', ',' || ua.rname || ','))
              )),
        iv as(
        SELECT DISTINCT
               CAST((a.caption || ' (' || a.name || ') [iview]') AS VARCHAR2(4000)) AS displaydata,
               a.caption,
               a.name,
               CAST('i' AS VARCHAR2(4000)) AS stype,
               CAST('NA' AS VARCHAR2(4000)) dimensions, CAST('NA' AS VARCHAR2(4000)) permissions, CAST('NA' AS VARCHAR2(4000)) createallowed, CAST('NA' AS VARCHAR2(4000)) viewallowed, CAST('NA' AS VARCHAR2(4000)) kfld, CAST('NA' AS VARCHAR2(4000))
        FROM iviews a
        LEFT JOIN axpages b ON b.pagetype = 'i' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name     
        WHERE (LOWER(pmode) = 'dev' OR b.visible = 'T')
          AND (
                REGEXP_LIKE(',' || presponsiblity || ',', ',default,')
                OR (ua.stype = 'i' AND REGEXP_LIKE(',' || presponsiblity || ',', ',' || ua.rname || ','))
              )),
pg as(
        SELECT DISTINCT
               CAST((b.caption || ' [page]') AS VARCHAR2(4000)) AS displaydata,
               b.caption,
               CAST(b.props AS VARCHAR2(4000)) AS name,
               CAST('p' AS VARCHAR2(4000)) AS stype,
               CAST('NA' AS VARCHAR2(4000)) dimensions, CAST('NA' AS VARCHAR2(4000)) permissions, CAST('NA' AS VARCHAR2(4000)) createallowed, CAST('NA' AS VARCHAR2(4000)) viewallowed,CAST('NA' AS VARCHAR2(4000)) kfld, CAST('NA' AS VARCHAR2(4000)) tablename
        FROM axpages b 
        LEFT JOIN axuseraccess ua ON ua.sname = b.name     
        WHERE b.pagetype = 'web'
          AND (LOWER(pmode) = 'dev' OR b.visible = 'T')
          AND (
                REGEXP_LIKE(',' || presponsiblity || ',', ',default,')
                OR REGEXP_LIKE(',' || presponsiblity || ',', ',' || ua.rname || ',')
              )),
ads as(
        SELECT CAST((a.sqlname || ' (' || a.sqlsrc || ') [ads]') AS VARCHAR2(4000)) AS displaydata,
               a.sqlname AS caption,
               a.sqlname AS name,
               CAST('ads' AS VARCHAR2(4000)) AS stype,
               CAST('NA' AS VARCHAR2(4000)) AS dimension,
               CAST(CASE WHEN a2.axpermissionsid > 0 THEN 'T' else 'NA' END AS VARCHAR2(4000)) AS permission,
               CAST('NA' AS VARCHAR2(4000)) AS createallowed,
               CAST(CASE WHEN a2.axpermissionsid > 0 OR a.createdby = pusername OR REGEXP_LIKE(',' || puserrole || ',', ',default,') THEN 'T' else 'NA' END AS VARCHAR2(4000)) AS viewallowed,
               CAST('NA' AS VARCHAR2(4000)) AS keyfield,
               CAST('NA' AS VARCHAR2(4000)) AS primarytable
        FROM axdirectsql a
        LEFT JOIN axpermissions a2 ON a.sqlname = a2.formcap AND (a2.axusername = pusername OR REGEXP_LIKE(',' || puserrole || ',', ',' || a2.axuserrole || ','))
        WHERE a.sqlsrc != 'Metadata'),
   inbox as(
            SELECT
                   CAST('Inbox' AS VARCHAR2(4000)) AS displaydata,
                   CAST('Inbox' AS VARCHAR2(4000)) AS caption,
                   CAST('Inbox' AS VARCHAR2(4000)) AS name,
                   CAST('Inbox' AS VARCHAR2(4000)) AS stype,
                   CAST('NA' AS VARCHAR2(4000)) dimensions, CAST('NA' AS VARCHAR2(4000)) permissions, CAST('NA' AS VARCHAR2(4000)) createallowed, CAST('NA' AS VARCHAR2(4000)) viewallowed, CAST('NA' AS VARCHAR2(4000)) kfld, CAST('NA' AS VARCHAR2(4000)) tablenae
            FROM DUAL
        )
 SELECT * FROM (
            SELECT * 
            FROM ts WHERE pstype IN ('t', 'all', 'analyze')
            UNION ALL
            SELECT * FROM iv WHERE pstype IN ('i', 'all')
            UNION ALL
            SELECT * FROM pg WHERE pstype IN ('p', 'all')
            UNION ALL
            SELECT * FROM ads WHERE pstype IN ('ads', 'all', 'analyze')
            UNION ALL
            SELECT * FROM inbox WHERE pstype IN ('all')
        ) final_result
        ORDER BY displaydata;
    
    IF v_cursor%ISOPEN THEN
        LOOP
            FETCH v_cursor INTO 
                v_displaydata, v_caption, v_name, v_stype, v_dimension, 
                v_permission, v_createallowed, v_viewallowed, v_keyfield, v_primarytable;
            EXIT WHEN v_cursor%NOTFOUND;
            
            PIPE ROW(AXI_GETSTRUCTURES_META(
                v_displaydata, v_caption, v_name, v_stype, v_dimension, 
                v_permission, v_createallowed, v_viewallowed, v_keyfield, v_primarytable
            ));
        END LOOP;
        CLOSE v_cursor;
    END IF;

    RETURN;
END
>>

<<
CREATE OR REPLACE FUNCTION fn_ads_getsqlcols(payload IN CLOB)
RETURN CLOB
IS
    result_string CLOB;
BEGIN
    SELECT RTRIM(
               XMLAGG(
                   XMLELEMENT(e, columnname || '~' || axdatatype, ',').EXTRACT('//text()')
                   ORDER BY ROWNUM
               ).GETCLOBVAL(), 
               ','
           )
    INTO result_string
    FROM JSON_TABLE(
        TREAT(payload AS JSON),
        '$.result.data[*]'
        COLUMNS (
            columnname VARCHAR2(4000) PATH '$.columnname',
            axdatatype VARCHAR2(4000) PATH '$.axdatatype'
        )
    );

    RETURN result_string;

EXCEPTION 
    WHEN OTHERS THEN 
        RETURN 'ERROR AT FUNCTION: ' || SQLERRM; 
END
>>

<<
CREATE OR REPLACE FUNCTION fn_ADS_getsql_json(
    p_appschema IN VARCHAR2,
    p_sqltext   IN VARCHAR2
) RETURN VARCHAR2 
IS
    v_json_output clob;
BEGIN
    SELECT '{"appname":"' || p_appschema || '","sqlquery":"' || p_sqltext || '"}' AS json_output 
    INTO v_json_output
    FROM dual;

    RETURN v_json_output;
EXCEPTION
    WHEN OTHERS THEN
        RETURN NULL;
END
>>

<<
CREATE OR REPLACE FUNCTION fn_get_axpertcomps_name (
    pinput IN varchar2
) RETURN CLOB 
IS
    v_result CLOB;
BEGIN
    IF pinput IS NULL OR LENGTH(pinput) <= 2 THEN
        RETURN NULL;
    END IF;

   SELECT
    listagg(rtrim(SUBSTR(split_value, INSTR(split_value, '-(')+ 2), ')'), ',') WITHIN GROUP(
    ORDER BY 1) INTO v_result
FROM
    (
    SELECT
        DISTINCT TRIM(REGEXP_SUBSTR( pinput, '[^,]+', 1, LEVEL)) AS split_value
    FROM
        dual
    CONNECT BY
        REGEXP_SUBSTR ( pinput,
        '[^,]+',
        1,
        LEVEL) IS NOT NULL);

    RETURN v_result;
END
>>

<<
DROP TYPE AXPDEF_PERMISSION_MDATA_OBJ
>>

<<
DROP TYPE AXPDEF_PERMISSION_MDATA
>>

<<
CREATE OR REPLACE TYPE "AXPDEF_PERMISSION_MDATA" AS OBJECT (
    transid VARCHAR2(250),
    fullcontrol VARCHAR2(1),
    userrole VARCHAR2(250),
    allowcreate VARCHAR2(3),
    view_access VARCHAR2(250),
    view_includedc VARCHAR2(4000),
    view_excludedc VARCHAR2(4000),
    view_includeflds clob,
    view_excludeflds clob,
    edit_access VARCHAR2(250),
    edit_includedc VARCHAR2(4000),
    edit_excludedc VARCHAR2(4000),
    edit_includeflds clob,
    edit_excludeflds clob,
    maskedflds clob,
    filtercnd NCLOB,
    encryptedflds clob,
    permissiontype varchar2(10),viewctrl varchar2(10),editctrl varchar2(10)
    )
>>

<<
CREATE OR REPLACE TYPE "AXPDEF_PERMISSION_MDATA_OBJ"                                          
   AS TABLE OF AXPDEF_PERMISSION_MDATA
>>
   
<<
CREATE OR REPLACE FUNCTION fn_permissions_getpermission(
    pmode          IN VARCHAR2,
    ptransid       IN VARCHAR2,
    pusername      IN VARCHAR2,
    proles         IN VARCHAR2 DEFAULT 'All',
    pglobalvars    IN VARCHAR2 DEFAULT 'NA'
) RETURN AXPDEF_PERMISSION_MDATA_OBJ PIPELINED
AS    
    rc  SYS_REFCURSOR;
    -- Declare local variables
    v_menuaccess_count NUMBER(10);     
    v_sql_roles VARCHAR2(4000);
   -- v_sql_permission_check VARCHAR2(4000);
    rolesql clob;
    v_permissionsql clob;
    v_permissionexists number(10);
    
    -- Variables to hold results before piping
    v_transid_loop VARCHAR2(250);
    v_fullcontrol VARCHAR2(1);
    v_userrole VARCHAR2(250);
    v_allowcreate VARCHAR2(10);
    v_view_access VARCHAR2(250);
    v_view_includedc VARCHAR2(4000);
    v_view_excludedc VARCHAR2(4000);
    v_view_includeflds clob;
    v_view_excludeflds clob; 
    v_edit_access VARCHAR2(250);
    v_edit_includedc VARCHAR2(4000);
    v_edit_excludedc VARCHAR2(4000);
    v_edit_includeflds clob;
    v_edit_excludeflds clob;
    v_maskedflds clob;
    v_filtercnd NCLOB;
    v_viewctrl VARCHAR2(1);
    v_editctrl VARCHAR2(1);
    --v_viewlist VARCHAR2(4000);
    --v_editlist VARCHAR2(4000);
    v_encryptedflds clob;
    v_permissiontype varchar2(10);
        
    v_view_includedflds    clob;
    v_view_excludedflds    clob;
    v_edit_includedflds    clob;
    v_edit_excludedflds    clob;
    v_fieldmaskstr         clob;
    v_cnd                  NCLOB;              
   


BEGIN
    -- Loop through each transid in the comma-separated string
    FOR rec_transid IN (SELECT COLUMN_VALUE AS transid FROM TABLE(string_to_array(ptransid, ','))) LOOP
        v_transid_loop := rec_transid.transid; 

        
        SELECT COUNT(*)
        INTO v_menuaccess_count
        FROM (
            SELECT 1 AS cnt
            FROM axusergroups a
            JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid
            JOIN axuseraccess a2 ON b.roles_id = a2.rname AND a2.stype = 't'
            WHERE a2.sname = v_transid_loop
              AND EXISTS (SELECT 1 FROM TABLE(string_to_array(proles, ',')) val WHERE val.COLUMN_VALUE = a.groupname)
            UNION ALL
            SELECT 1 FROM DUAL WHERE proles LIKE '%default%'
            UNION ALL
            SELECT 1 FROM axuserlevelgroups WHERE username = pusername AND usergroup = 'default'
            UNION ALL
            SELECT 1 AS cnt
            FROM axusergroups a
            JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid
            JOIN axuseraccess a2 ON b.roles_id = a2.rname AND a2.stype = 't'
            JOIN axuserlevelgroups u ON a.groupname = u.usergroup AND u.username = pusername
            WHERE a2.sname = v_transid_loop AND proles = 'All'
               UNION ALL
        SELECT 1 AS cnt FROM axusergroups a
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        JOIN axuserlevelgroups u ON a.groupname = u.usergroup 
        where b.ROLES_ID ='default' AND u.USERNAME = pusername
        );

       if proles='All' then 

rolesql := 'select a.axuserrole,case when viewctrl=''1'' then viewlist else null end view_includedflds,
case when viewctrl=''2'' then viewlist else null end view_excludedflds,case when editctrl=''1'' then editlist else null end edit_includedflds,
case when editctrl=''2'' then editlist else null end edit_excludedflds,
a.fieldmaskstr,b.cnd1 cnd,viewctrl,allowcreate,editctrl,''Role'' permissiontype
from AxPermissions a 
join axuserlevelgroups a2 on a2.axusergroup = a.axuserrole 
join axusers u on a2.axusersid = u.axusersid  
--left join (select b.axuserrole,b.cnd from axusers a join axuserpermissions b on a.axusersid =b.axusersid where a.username = '''||pusername||''')b on a.axuserrole=b.axuserrole  
left join axusergrouping b on u.axusersid = b.axusersid
where a.formtransid='''||rec_transid.transid||''' and u.username = '''||pusername||''' 
union all 
select a.axuserrole,case when viewctrl=''1'' then viewlist else null end view_includedflds,
case when viewctrl=''2'' then viewlist else null end view_excludedflds,case when editctrl=''1'' then editlist else null end edit_includedflds,
case when editctrl=''2'' then editlist else null end edit_excludedflds,
a.fieldmaskstr,b.cnd cnd,viewctrl,allowcreate,editctrl,''User'' permissiontype 
from AxPermissions a 
left join axuserdpermissions b on a.AxPermissionsid = b.AxPermissionsid 
where a.axusername = '''||pusername||''' and a.formtransid='''||rec_transid.transid||'''';

v_permissionsql :='select count(cnt)  from(select 1 cnt
from AxPermissions a 
join axuserlevelgroups a2 on a2.axusergroup = a.axuserrole 
join axusers u on a2.axusersid = u.axusersid  
--left join axuserpermissions b on a.axuserrole = b.axuserrole  
left join axusergrouping b on u.axusersid = b.axusersid
where a.formtransid='''||rec_transid.transid||''' and u.username = '''||pusername||''' 
union all 
select 1 cnt 
from AxPermissions a 
left join axuserdpermissions b on a.AxPermissionsid = b.AxPermissionsid 
where a.axusername = '''||pusername||''' and a.formtransid='''||rec_transid.transid||''')a';

else

rolesql := 'select a.axuserrole,case when viewctrl=''1'' then viewlist else null end view_includedflds,
case when viewctrl=''2'' then viewlist else null end view_excludedflds,case when editctrl=''1'' then editlist else null end edit_includedflds,
case when editctrl=''2'' then editlist else null end edit_excludedflds,
a.fieldmaskstr,b.cnd,viewctrl,allowcreate,editctrl,''Role'' permissiontype 
from AxPermissions a 
--left join (select b.axuserrole,b.cnd from axusers a join axuserpermissions b on a.axusersid =b.axusersid where a.username = '''||pusername||''')b on a.axuserrole=b.axuserrole
left join (
select a2.usergroup ,b.cnd1 cnd from axusers a join axuserlevelgroups a2 on a2.axusersid = a.axusersid left join axusergrouping b on a.axusersid =b.axusersid  where a.username = '''||pusername||''')b on a.axuserrole=b.usergroup
where exists (select 1 from table(string_to_array('''||proles||''','','')) where column_value in (a.axuserrole))
and a.formtransid='''||rec_transid.transid||'''   
union all
select a.axuserrole,case when viewctrl=''1'' then viewlist else null end view_includedflds,
case when viewctrl=''2'' then viewlist else null end view_excludedflds,case when editctrl=''1'' then editlist else null end edit_includedflds,
case when editctrl=''2'' then editlist else null end edit_excludedflds,
a.fieldmaskstr,b.cnd cnd,viewctrl,allowcreate,editctrl,''User'' permissiontype 
from AxPermissions a left join axuserDpermissions b on a.AxPermissionsid = b.AxPermissionsid 
where a.axusername = '''||pusername||''' and a.formtransid='''||rec_transid.transid||'''';

v_permissionsql := 'select count(cnt) from(select 1 cnt
from AxPermissions a left join axuserpermissions b on a.axuserrole = b.axuserrole 
left join axusers u on b.axusersid = u.axusersid  and u.username = '''||pusername||'''
left join (
select a2.usergroup ,b.cnd1 cnd from axusers a join axuserlevelgroups a2 on a2.axusersid = a.axusersid left join axusergrouping b on a.axusersid =b.axusersid  where a.username = '''||pusername||''')b on a.axuserrole=b.usergroup
where exists (select 1 from table(string_to_array('''||proles||''','','')) where column_value in (a.axuserrole))
and a.formtransid='''||rec_transid.transid||'''   
union all
select 1 cnt
from AxPermissions a left join axuserDpermissions b on a.AxPermissionsid = b.AxPermissionsid 
where a.axusername = '''||pusername||''' and a.formtransid='''||rec_transid.transid||''')a';

end if;

EXECUTE immediate v_permissionsql into  v_permissionexists;

        IF v_menuaccess_count > 0 AND v_permissionexists = 0 
        THEN            
            v_fullcontrol := 'T';
            v_userrole := NULL;
            v_view_includedc := NULL;
            v_view_excludedc := NULL;
            v_view_includeflds := NULL;
            v_view_excludeflds := NULL;
            v_edit_includedc := NULL;
            v_edit_excludedc := NULL;
            v_edit_includeflds := NULL;
            v_edit_excludeflds := NULL; 
            v_maskedflds := NULL;
            v_view_access := NULL;
            v_edit_access := NULL;
            v_allowcreate := NULL;
            v_filtercnd := NULL;
            SELECT LISTAGG(fname, ',') WITHIN GROUP (ORDER BY fname) INTO v_encryptedflds FROM axpflds 
           WHERE tstruct = v_transid_loop AND encrypted = 'T';        
            

            -- Pipe the row
            PIPE ROW (AXPDEF_PERMISSION_MDATA(
                v_transid_loop, v_fullcontrol, v_userrole, v_allowcreate, v_view_access,
                v_view_includedc, v_view_excludedc, v_view_includeflds, v_view_excludeflds,
                v_edit_access, v_edit_includedc, v_edit_excludedc, v_edit_includeflds, v_edit_excludeflds,
                v_maskedflds, v_filtercnd,v_encryptedflds,NULL,'0','0'));
               
ELSE
    OPEN rc FOR rolesql;

    LOOP
      FETCH rc INTO  
        v_userrole,
        v_view_includedflds,
        v_view_excludedflds,
        v_edit_includedflds,
        v_edit_excludedflds,
        v_fieldmaskstr,
        v_cnd,
        v_viewctrl,
        v_allowcreate,
        v_editctrl,
        v_permissiontype;
        
       EXIT WHEN rc%NOTFOUND;

     
     
      IF v_viewctrl = '0' THEN        
        NULL;
      ELSE        
        v_view_includeflds := CASE WHEN v_view_includedflds IS NULL THEN v_edit_includedflds
                                 WHEN v_edit_includedflds IS NULL THEN v_view_includedflds
                                 ELSE v_view_includedflds || ',' || v_edit_includedflds END;
      END IF;
    
     
      IF v_editctrl = '0' THEN
         v_view_access := NULL;
      ELSIF v_viewctrl = '4' THEN
           v_view_access := 'None';
      ELSE
           v_view_access := NULL;      
      END IF;

      IF v_editctrl = '4' THEN
        v_edit_access := 'None';
      ELSE
        v_edit_access := NULL;
      END IF;
     
     
     
           SELECT LISTAGG(dname, ',') WITHIN GROUP (ORDER BY dname) INTO  v_view_includedc
FROM axpdc WHERE tstruct = rec_transid.transid
  AND (v_view_includeflds IS NOT NULL  and INSTR(',' || v_view_includeflds || ',',',' || dname || ',') > 0);
           
 
            SELECT LISTAGG(fname, ',') WITHIN GROUP (ORDER BY fname) INTO  v_view_includeflds
FROM axpflds WHERE tstruct = rec_transid.transid AND savevalue = 'T' 
  AND (v_view_includeflds IS NOT NULL  and INSTR(',' || v_view_includeflds || ',',',' || fname || ',') > 0);
           
 
      SELECT LISTAGG(dname, ',') WITHIN GROUP (ORDER BY dname) INTO  v_view_excludedc
FROM axpdc WHERE tstruct = rec_transid.transid
  AND (v_view_excludedflds IS NOT NULL  and INSTR(',' || v_view_excludedflds || ',',',' || dname || ',') > 0);
 
           
 SELECT LISTAGG(fname, ',') WITHIN GROUP (ORDER BY fname) INTO  v_view_excludeflds
FROM axpflds WHERE tstruct = rec_transid.transid AND savevalue = 'T' 
  AND (v_view_excludedflds IS NOT NULL  and INSTR(',' || v_view_excludedflds || ',',',' || fname || ',') > 0);
 
         SELECT LISTAGG(dname, ',') WITHIN GROUP (ORDER BY dname) INTO  v_edit_includedc
FROM axpdc WHERE tstruct = rec_transid.transid
  AND (v_edit_includedflds IS NOT NULL  and INSTR(',' || v_edit_includedflds || ',',',' || dname || ',') > 0);
               
   SELECT LISTAGG(fname, ',') WITHIN GROUP (ORDER BY fname) INTO  v_edit_includeflds
FROM axpflds WHERE tstruct = rec_transid.transid AND savevalue = 'T' 
  AND (v_edit_includedflds IS NOT NULL  and INSTR(',' || v_edit_includedflds || ',',',' || fname || ',') > 0);   
   
          SELECT LISTAGG(dname, ',') WITHIN GROUP (ORDER BY dname) INTO  v_edit_excludedc
FROM axpdc WHERE tstruct = rec_transid.transid
  AND (v_edit_excludedflds IS NOT NULL  and INSTR(',' || v_edit_excludedflds || ',',',' || dname || ',') > 0);
                        
   SELECT LISTAGG(fname, ',') WITHIN GROUP (ORDER BY fname) INTO  v_edit_excludeflds
FROM axpflds WHERE tstruct = rec_transid.transid AND savevalue = 'T' 
  AND (v_edit_excludedflds IS NOT NULL  and INSTR(',' || v_edit_excludedflds || ',',',' || fname || ',') > 0);   
                       
      
    SELECT LISTAGG(fname, ',') WITHIN GROUP (ORDER BY fname) INTO  v_encryptedflds
FROM axpflds WHERE tstruct = rec_transid.transid AND encrypted = 'T' 
  AND (v_view_includeflds IS NOT NULL  and INSTR(',' || v_view_includeflds || ',',',' || fname || ',') > 0);   
 
                 
      v_fullcontrol := NULL; 
      v_maskedflds:=v_fieldmaskstr;
      v_filtercnd:=v_cnd;

     PIPE ROW (AXPDEF_PERMISSION_MDATA( 
                v_transid_loop, v_fullcontrol, v_userrole, v_allowcreate, v_view_access,
                v_view_includedc, v_view_excludedc, v_view_includeflds, v_view_excludeflds,
                v_edit_access, v_edit_includedc, v_edit_excludedc, v_edit_includeflds, v_edit_excludeflds,
                v_maskedflds, v_filtercnd,v_encryptedflds,v_permissiontype,v_viewctrl,v_editctrl));


    END LOOP;

    CLOSE rc;
               
    END if;

  END LOOP;

        
     


    RETURN; 
END
>>

<<
-- Object Type
CREATE OR REPLACE TYPE axi_firesql_obj AS OBJECT (
    id          VARCHAR2(4000),
    displaydata VARCHAR2(4000)
)
>>


<<
-- Table Type
CREATE OR REPLACE TYPE axi_firesql_tab AS TABLE OF axi_firesql_obj
>>


<<
-- Function
CREATE OR REPLACE FUNCTION axi_firesql_v2 (
    p_sql          IN CLOB,
    p_param_string IN VARCHAR2,
    p_sourcekey    IN VARCHAR2,
    p_fromlist     IN VARCHAR2
)
RETURN axi_firesql_tab PIPELINED
AS
    v_sql          CLOB := p_sql;
    v_pair         VARCHAR2(4000);
    v_param_name   VARCHAR2(1000);
    v_param_value  VARCHAR2(4000);

    v_pos          NUMBER := 1;
    v_next         NUMBER;
    TYPE refcur IS REF CURSOR;
    rc refcur;
    v_col1 VARCHAR2(4000);
    v_col2 VARCHAR2(4000);
BEGIN
    ------------------------------------------------------------------
    -- Replace Parameters
    ------------------------------------------------------------------
    IF p_param_string IS NOT NULL
       AND TRIM(p_param_string) <> ''
       AND INSTR(v_sql, ':') > 0
    THEN
        LOOP
            v_next := INSTR(p_param_string, ';', v_pos);
            IF v_next > 0 THEN
                v_pair := SUBSTR(p_param_string, v_pos, v_next - v_pos);
            ELSE
                v_pair := SUBSTR(p_param_string, v_pos);
            END IF;
            EXIT WHEN v_pair IS NULL;
            IF TRIM(v_pair) IS NOT NULL THEN
                v_param_name :=
                    TRIM(SUBSTR(v_pair, 1, INSTR(v_pair, '~') - 1));
                v_param_value :=
                    TRIM(SUBSTR(v_pair, INSTR(v_pair, '~') + 1));
                IF v_param_name IS NOT NULL THEN

                    v_sql := REPLACE(
                                v_sql,
                                ':' || v_param_name,
                                '''' || REPLACE(v_param_value, '''', '''''') || ''''
                             );
                END IF;
            END IF;
            EXIT WHEN v_next = 0;
            v_pos := v_next + 1;
        END LOOP;
    END IF;
    ------------------------------------------------------------------
    -- Handle p_fromlist
    ------------------------------------------------------------------
    IF p_fromlist IS NOT NULL
       AND TRIM(p_fromlist) <> ''
       AND LOWER(TRIM(p_fromlist)) <> 'null'
    THEN
        FOR r IN (
            SELECT TRIM(REGEXP_SUBSTR(p_fromlist, '[^,]+', 1, LEVEL)) val
            FROM dual
            CONNECT BY REGEXP_SUBSTR(p_fromlist, '[^,]+', 1, LEVEL) IS NOT NULL
        )
        LOOP
            IF r.val IS NOT NULL
               AND TRIM(r.val) <> ''
            THEN

                PIPE ROW (
                    axi_firesql_obj(
                        '0',
                        r.val
                    )
                );
            END IF;
        END LOOP;
    ELSE
        ------------------------------------------------------------------
        -- Sourcekey = T
        ------------------------------------------------------------------
        IF UPPER(NVL(p_sourcekey, 'F')) = 'T' THEN
            OPEN rc FOR
                '
                SELECT
                    col1,
                    RTRIM(
                        RTRIM(col2, ''0''),
                    ''.'') AS displaydata
                FROM (
                    ' || v_sql || '
                )
                WHERE col2 IS NOT NULL
                  AND TRIM(col2) <> ''''
                ';
            LOOP
                FETCH rc INTO v_col1, v_col2;
                EXIT WHEN rc%NOTFOUND;
                PIPE ROW (
                    axi_firesql_obj(
                        v_col1,
                        v_col2
                    )
                );
            END LOOP;
            CLOSE rc;
        ------------------------------------------------------------------
        -- Sourcekey != T
        ------------------------------------------------------------------
        ELSE
            OPEN rc FOR
                '
                SELECT
                    RTRIM(
                        RTRIM(col1, ''0''),
                    ''.'') AS displaydata
                FROM (
                    ' || v_sql || '
                )
                WHERE col1 IS NOT NULL
                  AND TRIM(col1) <> ''''
                ';
            LOOP
                FETCH rc INTO v_col1;
                EXIT WHEN rc%NOTFOUND;

                PIPE ROW (
                    axi_firesql_obj(
                        '0',
                        v_col1
                    )
                );
            END LOOP;
            CLOSE rc;
        END IF;
    END IF;
    RETURN;
END axi_firesql_v2
>>



commit;


<<
CREATE OR REPLACE TYPE obj_getstructlist AS OBJECT (
    displaydata VARCHAR2(4000),
    caption     VARCHAR2(4000),
    name        VARCHAR2(4000)
)
>>

---------------------------------------------

<<
CREATE OR REPLACE TYPE tab_getstructlist AS TABLE OF obj_getstructlist
>>

--------------------------------------------

<<
CREATE OR REPLACE FUNCTION axi_fn_getstructlist (
    p_roles      IN VARCHAR2,
    p_mode       IN VARCHAR2,
    p_structtype IN VARCHAR2
)
RETURN tab_getstructlist PIPELINED
AS
BEGIN

    IF LOWER(p_structtype) = 'i' THEN

        FOR rec IN (
            SELECT DISTINCT
                   a.caption || ' (' || a.name || ')' AS displaydata,
                   a.caption,
                   a.name
            FROM iviews a
                 JOIN axpages b
                     ON b.pagetype = 'i' || a.name
                 LEFT JOIN axuseraccess ua
                     ON ua.sname = a.name
            WHERE (LOWER(p_mode) = 'dev' OR b.visible = 'T')
              AND (
                    INSTR(',' || LOWER(p_roles) || ',', ',default,') > 0
                    OR (
                        ua.stype = 'i'
                        AND INSTR(',' || LOWER(p_roles) || ',',
                                   ',' || LOWER(ua.rname) || ',') > 0
                       )
                  )
            ORDER BY a.caption
        )
        LOOP
            PIPE ROW (
                obj_getstructlist(
                    rec.displaydata,
                    rec.caption,
                    rec.name
                )
            );
        END LOOP;

    ELSE

        FOR rec IN (
            SELECT DISTINCT
                   a.caption || ' (' || a.name || ')' AS displaydata,
                   a.caption,
                   a.name
            FROM tstructs a
                 JOIN axpages b
                     ON b.pagetype = 't' || a.name
                 LEFT JOIN axuseraccess ua
                     ON ua.sname = a.name
            WHERE (LOWER(p_mode) = 'dev' OR b.visible = 'T')
              AND (
                    INSTR(',' || LOWER(p_roles) || ',', ',default,') > 0
                    OR (
                        ua.stype = 't'
                        AND INSTR(',' || LOWER(p_roles) || ',',
                                   ',' || LOWER(ua.rname) || ',') > 0
                       )
                  )
            ORDER BY a.caption
        )
        LOOP
            PIPE ROW (
                obj_getstructlist(
                    rec.displaydata,
                    rec.caption,
                    rec.name
                )
            );
        END LOOP;

    END IF;

    RETURN;
END
>>
