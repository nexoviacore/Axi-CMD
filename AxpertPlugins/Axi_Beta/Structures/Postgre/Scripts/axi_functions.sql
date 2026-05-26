<<
drop function fn_axi_get_fieldvalues_with_keysuffix_list
>>

<<
drop function fn_axi_getkeyvalueswithfieldnameslist
>>

<<
drop function fn_upsert_config_by_condition
>>

<<
drop function get_ads_dropdown_data
>>

<<
drop function get_dynamic_field
>>

<<
drop function populate_axdirectsql_metadata
>>

<<
drop function fn_axi_metadata
>>

<<
drop function fn_axi_struct_metadata
>>

<<
drop function axi_firesql_v2
>>

<<
drop function axi_fn_getstructlist
>>

<<
drop function axi_fn_getaxobjectlist
>>

<<
drop function fn_axi_getstructures_meta
>>

<<
drop function fn_permissions_getpermission
>>

<<
drop function fn_axi_getstructs_obj
>>


<<
CREATE OR REPLACE FUNCTION fn_axi_get_fieldvalues_with_keysuffix_list
(
    p_tstruct text,
    p_fieldname text
) 
RETURNS TABLE(displaydata text, id text, caption text) 
LANGUAGE plpgsql AS
$function$
DECLARE 
    v_sql text; 
    v_tablename text; 
    v_sourcekey text; 
    v_srcfld text; 
    v_srctf text; 
    v_keyfield text; 
BEGIN 
    p_fieldname := lower(p_fieldname);
    SELECT keyfield INTO v_keyfield 
    FROM axp_tstructprops 
    WHERE name = p_tstruct 
    LIMIT 1; 
    IF v_keyfield IS NULL THEN 
        SELECT fname INTO v_keyfield
        FROM axpflds
        WHERE tstruct = p_tstruct
          AND dcname = 'dc1'
        ORDER BY
            CASE 
                WHEN modeofentry = 'autogenerate' THEN 1
                WHEN LOWER(allowduplicate) = 'f' 
                     AND LOWER(allowempty) = 'f' 
                     AND datatype = 'c' 
                     AND LOWER(hidden) = 'f' THEN 2
                WHEN LOWER(hidden) = 'f'
                     AND datatype = 'c' THEN 3
                ELSE 4
            END,
            ordno ASC
        LIMIT 1;
    END IF; 
    SELECT tablename INTO v_tablename 
    FROM axpdc 
    WHERE tstruct = p_tstruct 
      AND dname = 'dc1'; 
    SELECT srckey, srcfld, srctf INTO v_sourcekey, v_srcfld, v_srctf 
    FROM axpflds 
    WHERE tstruct = p_tstruct 
      AND lower(fname) = lower(p_fieldname); 
    IF v_keyfield IS NULL
       OR v_tablename IS NULL
       OR v_sourcekey IS NULL THEN
        RETURN;
    END IF;
    IF v_sourcekey = 'F' THEN
        v_sql := format(
            'SELECT (%I || ''['' || %I || '']'')::text AS displaydata,
                    ''0''::text AS id,
                    %I::text AS caption
             FROM %I
             WHERE %I IS NOT NULL
             ORDER BY displaydata ASC',
            p_fieldname,
            v_keyfield,
            v_keyfield,
            v_tablename,
            p_fieldname
        );
    ELSE
           v_sql := format(
			'SELECT (s.%I || ''['' || t.%I || '']'')::text AS displaydata,
					s.%I::text AS id,
					t.%I::text AS caption
			 FROM %I s
			 JOIN %I t ON s.%I = t.%I
			 WHERE s.%I IS NOT NULL
			 ORDER BY displaydata ASC',
			v_srcfld,                -- s.source field
			v_keyfield,              -- t.key field
			lower(v_srctf) || 'id',  -- caption field 
			v_keyfield,              -- id from key table -- caption field
			lower(v_srctf),          -- main table
			v_tablename,             -- key table
			--lower(v_srctf) || 'id',  -- join column in main table
			lower(v_srctf) || 'id',  -- join column in key table (adjust if different)
			p_fieldname,
			--p_fieldname              -- not null field from main table
			v_srcfld
		);
    END IF;
    RETURN QUERY EXECUTE v_sql; 
END; 
$function$
>>

<<
CREATE OR REPLACE FUNCTION fn_axi_getkeyvalueswithfieldnameslist(p_tstruct text, p_fieldname text)
 RETURNS TABLE(displaydata text, id text, caption text, isfield text)
 LANGUAGE plpgsql
AS $function$
DECLARE
  v_sql        text;
  v_tablename  text;
  v_sourcekey  text;
  v_srcfld     text;
  v_srctf      text;
  v_keyfield   text;
BEGIN
    SELECT keyfield
    INTO v_keyfield
    FROM axp_tstructprops
    WHERE lower(name) = lower(p_tstruct)
    LIMIT 1;
    IF v_keyfield IS NULL then
        SELECT fname INTO v_keyfield
        FROM axpflds
        WHERE lower(tstruct) = lower(p_tstruct)
          AND dcname = 'dc1'
        ORDER BY
            CASE 
                WHEN modeofentry = 'autogenerate' THEN 1
                WHEN LOWER(allowduplicate) = 'f' 
                     AND LOWER(allowempty) = 'f' 
                     AND datatype = 'c' 
                     AND LOWER(hidden) = 'f' THEN 2
                WHEN LOWER(hidden) = 'f'
                     AND datatype = 'c' THEN 3
                ELSE 4
            END,
            ordno ASC
        LIMIT 1;
    END IF;
  SELECT tablename
  INTO v_tablename
  FROM axpdc
  WHERE tstruct = p_tstruct
    AND dname = 'dc1';
  SELECT srckey, srcfld, srctf
  INTO v_sourcekey, v_srcfld, v_srctf
  FROM axpflds
  WHERE lower(tstruct) = lower(p_tstruct)
    AND lower(fname)   = lower(p_fieldname);
 IF v_keyfield IS NULL THEN
    RAISE EXCEPTION 'Key field could not be determined for tstruct %', p_tstruct;
END IF;
IF v_tablename IS NULL THEN
    RAISE EXCEPTION 'Base table not found for tstruct %', p_tstruct;
END IF;
IF v_sourcekey IS NULL THEN
    RAISE EXCEPTION 'Source metadata not found for field % in tstruct %',
        p_fieldname, p_tstruct;
END IF;
IF v_sourcekey <> 'F' AND (v_srcfld IS NULL OR v_srctf IS NULL) THEN
    RAISE EXCEPTION 'Source table/field missing for field % in tstruct %',
        p_fieldname, p_tstruct;
END IF;
--Need to remove lowercase convertion , currently axpflds takes user entered mixed case
--but db tables have lowercase fieldnames, this cuases issue with dynamic sql , so added lower()
p_fieldname = lower(p_fieldname);
v_keyfield = lower(v_keyfield);
IF v_sourcekey = 'F' THEN
    v_sql := format(
    $sql$
    SELECT (%I)::text AS displaydata,
           '0'::text AS id,
           %I::text AS caption,
           'f'::text AS isfield
    FROM %I
    WHERE %I IS NOT NULL
    UNION ALL
    SELECT (caption || ' (' || fname || ')' || ' [' || 'field' || ']')::text AS displaydata,
           '0'::text AS id,
           caption::text AS caption,
           't'::text AS isfield  
    FROM axpflds
    WHERE tstruct = %L
      AND lower(hidden) = 'f'
      AND lower(savevalue) = 't'
	  AND lower(asgrid) = 'f'
    ORDER BY isfield ASC, displaydata ASC
    $sql$,
    p_fieldname,
    v_keyfield,
    v_tablename,
    p_fieldname,
    p_tstruct
);
ELSE
    v_sql := format(
    $sql$
    SELECT (%I)::text AS displaydata,
           %I::text AS id,
           %I::text AS caption,
           'f'::text AS isfield
    FROM %I
    WHERE %I IS NOT NULL
    UNION ALL
    SELECT (caption || ' (' || fname || ') [' || 'field' || ']')::text AS displaydata,
           '0'::text AS id,
           caption::text AS caption,
           't'::text AS isfield
    FROM axpflds
    WHERE tstruct = %L
      AND lower(hidden) = 'f'
      AND lower(savevalue) = 't'
	  AND lower(asgrid) = 'f'
    ORDER BY isfield ASC, displaydata ASC
    $sql$,
    v_srcfld,
    lower(v_srctf) || 'id',
    v_keyfield,
    lower(v_srctf),
    p_fieldname,
    p_tstruct
);
END IF;
  RETURN QUERY EXECUTE v_sql;
END;
$function$
>>

<<
CREATE OR REPLACE FUNCTION fn_upsert_config_by_condition(
    p_tablename      text,
    p_fieldnames     text,
    p_fieldvalues    text,
    p_where_clause   text
)
RETURNS TABLE(status text)
LANGUAGE plpgsql
AS $function$
DECLARE
    v_exists     boolean;
    v_set_clause text;
    v_cols_arr   text[];
    v_vals_arr   text[];
BEGIN
    v_cols_arr := string_to_array(p_fieldnames, ',');
    v_vals_arr := string_to_array(p_fieldvalues, ',');
    IF array_length(v_cols_arr, 1)
       IS DISTINCT FROM
       array_length(v_vals_arr, 1) THEN
        RAISE EXCEPTION
            'Field names and values count mismatch';
    END IF;
    SELECT string_agg(
               format('%I=%s', trim(col), trim(val)),
               ', '
           )
    INTO v_set_clause
    FROM unnest(v_cols_arr, v_vals_arr) AS t(col, val)
    WHERE col IS NOT NULL
      AND trim(col) <> '';
    EXECUTE format(
        'SELECT EXISTS (SELECT 1 FROM %I WHERE %s)',
        p_tablename,
        p_where_clause
    )
    INTO v_exists;
    IF v_exists THEN
        EXECUTE format(
            'UPDATE %I SET %s WHERE %s',
            p_tablename,
            v_set_clause,
            p_where_clause
        );
    ELSE
        EXECUTE format(
            'INSERT INTO %I (%s) VALUES (%s)',
            p_tablename,
            p_fieldnames,
            p_fieldvalues
        );
    END IF;
END;
$function$
>>

<<
CREATE OR REPLACE FUNCTION get_ads_dropdown_data(
    p_tablename  TEXT,
    p_fieldname  TEXT
)
RETURNS TABLE (
    displaydata TEXT,
    name        TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY EXECUTE format(
        'SELECT %I::text AS displaydata, %I::text AS name FROM %I',
        p_fieldname,
        p_fieldname,
        p_tablename
    );
END;
$$
>>

<<
CREATE OR REPLACE FUNCTION get_dynamic_field(p_tstruct text, p_fieldname text)
 RETURNS TABLE(displaydata text, id text)
 LANGUAGE plpgsql
AS $function$
DECLARE
  v_sql        text;
  v_tablename  text;
  v_sourcekey  text;
  v_srcfld     text;
  v_srctf      text;
BEGIN
  SELECT tablename
  INTO v_tablename
  FROM axpdc
  WHERE tstruct = p_tstruct
    AND dname = 'dc1';
  SELECT srckey, srcfld, srctf
  INTO v_sourcekey, v_srcfld, v_srctf
  FROM axpflds
  WHERE tstruct = p_tstruct
    AND fname   = p_fieldname;
  IF v_sourcekey = 'F' THEN
    v_sql := format(
      'SELECT %I::text, ''0''::text AS id FROM %I',
      p_fieldname,
      v_tablename
    );
  ELSE
    v_sql := format(
      'SELECT %I::text AS displaydata, %I::text AS id FROM %I',
      v_srcfld,
      lower(v_srctf)||'id',
      lower(v_srctf)
    );
  END IF;
  RETURN QUERY EXECUTE v_sql;
END;
$function$
>>

<<
CREATE OR REPLACE FUNCTION populate_axdirectsql_metadata()
RETURNS void
LANGUAGE plpgsql
AS $$
DECLARE
    r RECORD;
    cols TEXT;
    col TEXT;
    row_no INT;
BEGIN
    FOR r IN
        SELECT axdirectsqlid, sqltext
        FROM axdirectsql
    LOOP
        cols :=
            substring(
                r.sqltext
                FROM '(?i)select\s+(.*?)\s+from'
            );
        row_no := 1;
        FOR col IN
            SELECT trim(value)
            FROM regexp_split_to_table(cols, ',') AS value
        LOOP
            IF col ~* '\s+as\s+' THEN
                col := regexp_replace(col, '.*\s+as\s+', '', 'i');
            ELSIF col ~ '\s' THEN
                col := split_part(col, ' ', array_length(string_to_array(col, ' '), 1));
            END IF;
            INSERT INTO axdirectsql_metadata (
                axdirectsql_metadataid,
                axdirectsqlid,
                axdirectsql_metadatarow,
                fldname,
                fldcaption,
                normalized,
                datatypeui,
                fdatatype
            )
            VALUES (
                CAST(
                    '2' || lpad(nextval('axdirectsql_metadata_seq')::text, 14, '0')
                    AS BIGINT
                ),
                r.axdirectsqlid,
                row_no,
                col,
                col,
                'F',
                'character',
                'c'
            );
            row_no := row_no + 1;
        END LOOP;
    END LOOP;
END;
$$
>>

<<
CREATE OR REPLACE FUNCTION fn_axi_metadata(pstructtype character varying, pusername character varying)
 RETURNS TABLE(structtype text, caption text, transid character varying)
 LANGUAGE plpgsql
AS $function$
declare 
declare v_sql varchar;
begin
 if pstructtype='tstructs' then
  v_sql = 'select ''tstruct'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join tstructs t on a5.sname = t.name
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where t.blobno =1 and ag.username =$1
 union all
 select ''tstruct'',caption || ''-('' || name || '')'' cap,name
 from tstructs t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.blobno =1 and ag.username =$1
 union all
       SELECT ''tstruct'',caption || ''-('' || name || '')'' cap,name from tstructs t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.blobno =1';        
 elsif pstructtype='iviews' then
  v_sql = 'select ''iview'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join iviews t on a5.sname = t.name
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where t.blobno =1 and ag.username =$1
 union all
 select ''iview'',caption || ''-('' || name || '')'' cap,name
 from iviews t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.blobno =1 and ag.username =$1
 union all
       SELECT ''iview'',caption || ''-('' || name || '')'' cap,name from iviews t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.blobno =1';
elsif pstructtype='pages' then 
 v_sql = 'select ''page'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join axpages p on a5.sname = p.name and p.pagetype =''web''
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where ag.username =$1
 union all
 select ''page'',caption || ''-('' || name || '')'' cap,name
 from axpages t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.pagetype =''web'' and ag.username =$1
 union all
       SELECT ''page'',caption || ''-('' || name || '')'' cap,name from axpages t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.pagetype =''web''';   
elsif pstructtype='ads' then  
v_sql = 'select ''ADS'',sqlname::text,sqlname from axdirectsql';
else
v_sql = 'select ''tstruct'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join tstructs t on a5.sname = t.name
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where t.blobno =1 and ag.username =$1
 union all
 select ''tstruct'',caption || ''-('' || name || '')'' cap,name
 from tstructs t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.blobno =1 and ag.username =$1
 union all
       SELECT ''tstruct'',caption || ''-('' || name || '')'' cap,name from tstructs t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.blobno =1
union all  
select ''iview'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join iviews t on a5.sname = t.name
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where t.blobno =1 and ag.username =$1
 union all
 select ''iview'',caption || ''-('' || name || '')'' cap,name
 from iviews t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.blobno =1 and ag.username =$1
 union all
       SELECT ''iview'',caption || ''-('' || name || '')'' cap,name from iviews t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.blobno =1
union all
select ''page'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join axpages p on a5.sname = p.name and p.pagetype =''web''
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where ag.username =$1
 union all
 select ''page'',caption || ''-('' || name || '')'' cap,name
 from axpages t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.pagetype =''web'' and ag.username =$1
 union all
       SELECT ''page'',caption || ''-('' || name || '')'' cap,name from axpages t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.pagetype =''web''
union all
select ''ADS'',sqlname::text,sqlname from axdirectsql';
end if;
return query execute v_sql using pusername;
END; 
$function$
>>

<<
CREATE OR REPLACE FUNCTION fn_axi_struct_metadata(pstructtype character varying, ptransid character varying, pobjtype character varying)
 RETURNS TABLE(objtype character varying, objcaption text, objname character varying, dcname character varying, asgrid character varying)
 LANGUAGE plpgsql
AS $function$
declare 
declare v_sql varchar;
begin
 if pstructtype='tstruct' then
  if pobjtype = 'fields' then 
  v_sql = 'select ''Field''::varchar,caption||''(''||fname||'')'',fname,dcname,asgrid from axpflds where tstruct =$1';
  elsif pobjtype = 'genmaps' then 
  v_sql = 'select ''Genmap''::varchar,caption||''(''||gname||'')'',gname,null::varchar,null::varchar from axpgenmaps where tstruct =$1';
  elsif pobjtype = 'mdmaps' then 
  v_sql = 'select ''MDmap''::varchar,caption||''(''||mname||'')'',mname,null::varchar,null::varchar from axpmdmaps where tstruct =$1';  
  end if;
 elsif pstructtype='iview' then
  if pobjtype = 'columns' then
  v_sql = 'select ''Column''::varchar,f_caption||''(''||f_name||'')'',f_name,null::varchar,null::varchar from iviewcols where iname =$1';  
  elsif pobjtype = 'params' then
  v_sql = 'select ''Param''::varchar,pcaption||''(''||pname||'')'',pname,null::varchar,null::varchar from iviewparams where iname =$1';  
  end if; 
end if;
return query execute v_sql using ptransid;
END; 
$function$
>>

<<
CREATE OR REPLACE FUNCTION axi_firesql_v2(
    p_sql TEXT,
    p_param_string TEXT,
    p_sourcekey TEXT,
    p_fromlist TEXT
)
RETURNS TABLE (
    id TEXT,
    displaydata TEXT
)
LANGUAGE plpgsql
AS
$$
DECLARE
    v_sql TEXT := p_sql;
    v_pair TEXT;
    v_pairs TEXT[];
    v_param_name TEXT;
    v_param_value TEXT;
BEGIN
    IF p_param_string IS NOT NULL
       AND trim(p_param_string) <> ''
       AND position(':' IN v_sql) > 0
    THEN
        v_pairs := string_to_array(p_param_string, ';');

        FOREACH v_pair IN ARRAY v_pairs
        LOOP
            IF trim(v_pair) = '' THEN
                CONTINUE;
            END IF;

            v_param_name  := trim(split_part(v_pair, '~', 1));
            v_param_value := trim(split_part(v_pair, '~', 2));

            IF v_param_name <> '' THEN
                v_sql := replace(
                            v_sql,
                            ':' || v_param_name,
                            quote_literal(v_param_value)
                         );
            END IF;
        END LOOP;
    END IF;
    IF p_fromlist IS NOT NULL AND trim(p_fromlist) <> '' AND trim(p_fromlist) <> 'null' THEN

        RETURN QUERY EXECUTE
        'SELECT 
             ''0'' AS id,
             trim(value) AS displaydata
         FROM unnest(string_to_array(' || quote_literal(p_fromlist) || ', '','')) AS value
		 WHERE value IS NOT NULL
           AND trim(value) <> ''''';
    ELSE        
        IF upper(coalesce(p_sourcekey,'F')) = 'T' THEN

            RETURN QUERY EXECUTE
            'SELECT 
                 col1::text AS id,
                 trim(trailing ''.'' from trim(trailing ''0'' from col2::text)) AS displaydata
             FROM (
                 SELECT *
                 FROM (' || v_sql || ') q
             ) sub(col1, col2) WHERE col2 IS NOT NULL
               AND trim(col2::text) <> ''''';        
        ELSE

            RETURN QUERY EXECUTE
            'SELECT 
                 ''0'' AS id,
                 trim(trailing ''.'' from trim(trailing ''0'' from col1::text)) AS displaydata
             FROM (
                 SELECT *
                 FROM (' || v_sql || ') q
             ) sub(col1) WHERE col1 IS NOT NULL
               AND trim(col1::text) <> ''''';

        END IF;

    END IF;

END;
$$
>>

<<
CREATE OR REPLACE FUNCTION axi_fn_getstructlist(
    p_roles text,
    p_mode text,
    p_structtype text
)
RETURNS TABLE (
    displaydata text,
    caption text,
    name text
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_roles text[];
BEGIN
    v_roles := string_to_array(p_roles, ',');

    IF lower(p_structtype) = 'i' THEN
        RETURN QUERY
        SELECT DISTINCT
               (a.caption || ' (' || a.name || ')')::text,
               a.caption::text,
               a.name::text
        FROM iviews a
        JOIN axpages b ON b.pagetype = 'i' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name
        WHERE (lower(p_mode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(v_roles)
                OR (ua.stype = 'i' AND ua.rname = ANY(v_roles))
              )
        ORDER BY 2;

    ELSE
        RETURN QUERY
        SELECT DISTINCT
               (a.caption || ' (' || a.name || ')')::text,
               a.caption::text,
               a.name::text
        FROM tstructs a
        JOIN axpages b ON b.pagetype = 't' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name
        WHERE (lower(p_mode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(v_roles)
                OR (ua.stype = 't' AND ua.rname = ANY(v_roles))
              )
        ORDER BY 2;
    END IF;

END;
$$
>>


<<
CREATE OR REPLACE FUNCTION axi_fn_getaxobjectlist(p_userroles text)
RETURNS TABLE (
    displaydata text,
    caption text,
    name text
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_roles text[];
BEGIN
    v_roles := string_to_array(p_userroles, ',');

    RETURN QUERY
    SELECT *
    FROM (

        -- TSTRUCT
        SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [tstruct]')::text AS displaydata,
               a.caption::text AS caption,
               a.name::text AS name
        FROM tstructs a
        JOIN axpages b
             ON b.pagetype = 't' || a.name
        LEFT JOIN axuseraccess ua
             ON ua.sname = a.name
        WHERE b.visible = 'T'
          AND (
                'default' = ANY(v_roles)
                OR (ua.stype = 't' AND ua.rname = ANY(v_roles))
              )

        UNION

        -- IVIEW
        SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [iview]')::text displaydata,
               a.caption::text caption,
               a.name::text name
        FROM iviews a
        JOIN axpages b
             ON b.pagetype = 'i' || a.name
        LEFT JOIN axuseraccess ua
             ON ua.sname = a.name
        WHERE b.visible = 'T'
          AND (
                'default' = ANY(v_roles)
                OR (ua.stype = 'i' AND ua.rname = ANY(v_roles))
              )

        UNION

        -- PAGE
        SELECT DISTINCT
               (p.caption || ' [page]')::text displaydata,
               p.caption::text caption,
               p.props::text name
        FROM axpages p
        LEFT JOIN axuseraccess ua
             ON ua.sname = p.props
        WHERE p.pagetype = 'web'
          AND p.visible = 'T'
          AND p.props IS NOT NULL
          AND p.props <> ''
          AND (
                'default' = ANY(v_roles)
                OR (ua.stype = 'p' AND ua.rname = ANY(v_roles))
              )

        UNION

        -- ADS (no role check)
        SELECT
               (a.sqlname || ' (' || a.sqlsrc || ') [ads]')::text displaydata,
               a.sqlsrc::text caption,
               a.sqlname::text name
        FROM axdirectsql a
        WHERE EXISTS (
                SELECT 1
                FROM axdirectsql_metadata m
                WHERE m.axdirectsqlid = a.axdirectsqlid
              )

        UNION

        -- Inbox
        SELECT
               'Inbox'::text displaydata,
               'Inbox'::text caption,
               'Inbox'::text name

    ) src
    ORDER BY displaydata;

END;
$$
>>

--fn_permissions_getpermission | to be removed from axi , as this will be added to product code.

<<
CREATE OR REPLACE FUNCTION fn_permissions_getpermission(pmode character varying, ptransid character varying, pusername character varying, proles character varying DEFAULT 'All'::character varying, pglobalvars character varying DEFAULT 'NA'::character varying)
 RETURNS TABLE(transid character varying, fullcontrol character varying, userrole character varying, allowcreate character varying, view_access character varying, view_includedc character varying, view_excludedc character varying, view_includeflds character varying, view_excludeflds character varying, edit_access character varying, edit_includedc character varying, edit_excludedc character varying, edit_includeflds character varying, edit_excludeflds character varying, maskedflds character varying, filtercnd text, encryptedflds character varying, permissiontype character varying, viewctrl character varying, editctrl character varying)
 LANGUAGE plpgsql
AS $function$
declare 
rec record;
rolesql text;
v_permissionsql text;
v_permissionexists numeric;
v_menuaccess numeric;
rec_transid record;
v_final_conditions text[] DEFAULT  ARRAY[]::text[];
rec_glovars record;
rec_glovars_varname varchar;
rec_glovars_varvalue varchar;
rec_dbconditions record;
v_dimensionsexists numeric;
v_applypermissions numeric;
v_matched varchar;
v_condition varchar;
v_used_vars varchar[] DEFAULT  ARRAY[]::varchar[];
v_usercondition text;
begin


select count(*) into v_applypermissions from axgrouptstructs where ftransid = ptransid;

if v_applypermissions > 0 then

select  case when length(cnd1)>2 then 1 else 0 end,cnd1 into v_dimensionsexists,v_usercondition from axusergrouping a 
 join axusers b on a.axusersid = b.axusersid 
 join axgrouptstructs a2 on a2.ftransid=ptransid
 where b.username  = fn_permissions_getpermission.pusername;


if pglobalvars !='NA' then

FOR rec_glovars IN
    SELECT unnest(string_to_array(pglobalvars,'~~')) glovars
LOOP

    rec_glovars_varname  := split_part(rec_glovars.glovars,'=',1);
    rec_glovars_varvalue := split_part(rec_glovars.glovars,'=',2);

   v_condition := concat('{primarytable.}',rec_glovars_varname,' in (',rec_glovars_varvalue,',''All'')');

        v_final_conditions := array_append(v_final_conditions, v_condition);

END LOOP;

else 


v_condition := v_usercondition;
v_final_conditions :=array_append(v_final_conditions, v_condition);

end if;

end if;

for rec_transid in(select unnest(string_to_array(ptransid,',')) transid) loop

select sum(cnt) into v_menuaccess from 
(select 1 cnt from axusergroups a join axusergroupsdetail b on a.axusergroupsid = b.axusergroupsid
join axuseraccess a2 on b.roles_id = a2.rname and stype ='t' 
where a2.sname = rec_transid.transid
and exists(select 1 from unnest(string_to_array(proles,',')) val where val = a.groupname)
union all
select 1 from dual where proles like '%default%'
union all
select 1 from axuserlevelgroups where username = pusername and usergroup='default'
union all
select 1 cnt from axusergroups a join axusergroupsdetail b on a.axusergroupsid = b.axusergroupsid
join axuseraccess a2 on b.roles_id = a2.rname and stype ='t'
join axuserlevelgroups u on a.groupname = u.usergroup and u.username = pusername 
where a2.sname = ptransid and proles = 'All'
   UNION ALL
        SELECT 1 AS cnt FROM axusergroups a
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        JOIN axuserlevelgroups u ON a.groupname = u.usergroup 
        where b.ROLES_ID ='default' AND u.USERNAME = pusername
)a;

if proles='All' then 
rolesql := 'select a.axuserrole,case when viewctrl=''1'' then viewlist else null end view_includedflds,
case when viewctrl=''2'' then viewlist else null end view_excludedflds,case when editctrl=''1'' then editlist else null end edit_includedflds,
case when editctrl=''2'' then editlist else null end edit_excludedflds,
a.fieldmaskstr,b.cnd1 cnd,viewctrl,allowcreate,editctrl,''Role'' permissiontype 
from AxPermissions a 
join axuserlevelgroups a2 on a2.axusergroup = a.axuserrole 
join axusers u on a2.axusersid = u.axusersid  
left join axusergrouping b on u.axusersid = b.axusersid
where a.formtransid='''||rec_transid.transid||''' and u.username = '''||pusername||''' 
union all 
select a.axuserrole,case when viewctrl=''1'' then viewlist else null end view_includedflds,
case when viewctrl=''2'' then viewlist else null end view_excludedflds,case when editctrl=''1'' then editlist else null end edit_includedflds,
case when editctrl=''2'' then editlist else null end edit_excludedflds,
a.fieldmaskstr,b.cnd,viewctrl,allowcreate,editctrl,''User'' permissiontype 
from AxPermissions a 
left join axuserdpermissions b on a.AxPermissionsid = b.AxPermissionsid 
where a.axusername = '''||pusername||''' and a.formtransid='''||rec_transid.transid||'''';

v_permissionsql := 'select count(cnt) from(select 1 cnt
from AxPermissions a 
join axuserlevelgroups a2 on a2.axusergroup = a.axuserrole 
join axusers u on a2.axusersid = u.axusersid  
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
left join (
select a2.usergroup ,b.cnd1 cnd from axusers a join axuserlevelgroups a2 on a2.axusersid = a.axusersid left join axusergrouping b on a.axusersid =b.axusersid  where a.username = '''||pusername||''')b on a.axuserrole=b.usergroup
where exists (select 1 from unnest(string_to_array('''||proles||''','','')) val where val in (a.axuserrole))
and a.formtransid='''||rec_transid.transid||'''   
union all
select a.axuserrole,case when viewctrl=''1'' then viewlist else null end view_includedflds,
case when viewctrl=''2'' then viewlist else null end view_excludedflds,case when editctrl=''1'' then editlist else null end edit_includedflds,
case when editctrl=''2'' then editlist else null end edit_excludedflds,
a.fieldmaskstr,b.cnd,viewctrl,allowcreate,editctrl,''User'' permissiontype 
from AxPermissions a left join axuserDpermissions b on a.AxPermissionsid = b.AxPermissionsid 
where a.axusername = '''||pusername||''' and a.formtransid='''||rec_transid.transid||'''';

v_permissionsql :='select count(cnt) from(select 1 cnt
from AxPermissions a 
left join (
select a2.usergroup ,b.cnd1 cnd,a.axusersid from axusers a join axuserlevelgroups a2 on a2.axusersid = a.axusersid left join axusergrouping b on a.axusersid =b.axusersid  where a.username = '''||pusername||''')b on a.axuserrole=b.usergroup
left join axusers u on b.axusersid = u.axusersid  and u.username = '''||pusername||'''
where exists (select 1 from unnest(string_to_array('''||proles||''','','')) val where val in (a.axuserrole))
and a.formtransid='''||rec_transid.transid||'''   
union all
select 1 cnt
from AxPermissions a left join axuserDpermissions b on a.AxPermissionsid = b.AxPermissionsid 
where a.axusername = '''||pusername||''' and a.formtransid='''||rec_transid.transid||''')a'; 

end if;

execute v_permissionsql into  v_permissionexists;



if v_menuaccess > 0 and v_permissionexists = 0 then 

fullcontrol:= 'T';
transid := rec_transid.transid;
userrole := null;
view_includedc  :=null;
view_excludedc  :=null;   
view_includeflds:=null;
view_excludeflds :=null;
edit_includedc :=null;
edit_excludedc :=null;   
edit_includeflds :=null;
edit_excludeflds :=null;
maskedflds := null;    
view_access := null;
edit_access := null;
view_includeflds := null;  
view_includedc :=null;
allowcreate := null;
filtercnd := case when v_applypermissions > 0 then array_to_string(v_final_conditions,' and ') else null end;
viewctrl := '0';
editctrl :='0';
select string_agg(fname,',') into encryptedflds  from axpflds where tstruct=rec_transid.transid and encrypted='T'; 
 
return next;

else

for rec in execute rolesql
loop 
  transid := rec_transid.transid;
  userrole := rec.axuserrole;
  select string_agg(dname,',') into view_includedc  from axpdc where tstruct=rec_transid.transid and exists (select 1 from unnest(string_to_array( concat('dc1,',rec.view_includedflds),',')) val where val = dname);
  select string_agg(dname,',') into view_excludedc  from axpdc where tstruct=rec_transid.transid and exists (select 1 from unnest(string_to_array( rec.view_excludedflds,',')) val where val = dname);   
  select string_agg(fname,',') into view_includeflds  from axpflds where tstruct=rec_transid.transid and savevalue='T' and exists (select 1 from unnest(string_to_array( rec.view_includedflds,',')) val where val = fname);
  select string_agg(fname,',') into view_excludeflds  from axpflds where tstruct=rec_transid.transid and savevalue='T' and exists (select 1 from unnest(string_to_array( rec.view_excludedflds,',')) val where val = fname);
  select string_agg(dname,',') into edit_includedc  from axpdc where tstruct=rec_transid.transid and exists (select 1 from unnest(string_to_array( rec.edit_includedflds,',')) val where val = dname);
  select string_agg(dname,',') into edit_excludedc  from axpdc where tstruct=rec_transid.transid and exists (select 1 from unnest(string_to_array( rec.edit_excludedflds,',')) val where val = dname);   
  select string_agg(fname,',') into edit_includeflds  from axpflds where tstruct=rec_transid.transid and savevalue='T' and exists (select 1 from unnest(string_to_array( rec.edit_includedflds,',')) val where val = fname);
  select string_agg(fname,',') into edit_excludeflds  from axpflds where tstruct=rec_transid.transid and savevalue='T' and exists (select 1 from unnest(string_to_array( rec.edit_excludedflds,',')) val where val = fname);
  maskedflds := rec.fieldmaskstr;    
  view_access := case when rec.viewctrl='4' then 'None' else null end;
  edit_access := case when rec.editctrl='4' then 'None' else null end;
  view_includeflds := case when rec.viewctrl='0' then view_includeflds else concat(view_includeflds,',',edit_includeflds) end;  
  view_includedc :=case when rec.viewctrl='0' then view_includedc else  concat(view_includedc,',',edit_includedc) end;
  allowcreate := rec.allowcreate;
  --filtercnd := rec.cnd;
filtercnd := array_to_string(v_final_conditions,' and ');
  select string_agg(fname,',') into encryptedflds  from axpflds  where tstruct=rec_transid.transid and encrypted='T' and exists (select 1 from unnest(string_to_array(view_includeflds,',')) val where val = fname);
  fullcontrol:= null;
  permissiontype := rec.permissiontype;
viewctrl := rec.viewctrl;
editctrl :=rec.editctrl;
  return next;

end loop;

end if;

end loop;
 
return;
 
END; 
$function$
>>


--fn_axi_getstructures_meta with userpermission meta data
<<
CREATE OR REPLACE FUNCTION fn_axi_getstructures_meta(pusername character varying, puserrole character varying, presponsiblity character varying, pmode character varying, pstype character varying)
 RETURNS TABLE(displaydata character varying, caption character varying, name character varying, stype character varying, dimension character varying, permission character varying, createallowed character varying, viewallowed character varying, keyfield character varying, primarytable character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN

if pstype='t' then
    RETURN QUERY
SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [tstruct]')::varchar,
               a.caption tcaption,
               a.name transid,'t'::varchar stype,coalesce(g.dimensions,'F')::varchar dimensions,
    coalesce(p.permissions,'F')::varchar,coalesce(p.newrecord,'T')::varchar,coalesce(p.viewctrl,'T')::varchar,kf.kfld,d.tablename
        FROM tstructs a
join axpdc d on a.name = d.tstruct and d.dname='dc1'
        left JOIN axpages b ON b.pagetype = 't' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name
left join (SELECT DISTINCT ON (combined_results.name) 
    combined_results.name tstruct,  
    kfld, 
    ord
FROM (
    SELECT 
        a.name,
        a.keyfield AS kfld,
        1 AS ord       
    FROM axp_tstructprops a 
    UNION ALL
    SELECT 
        tstruct AS name,
        fname AS kfld,
        CASE 
            WHEN modeofentry = 'autogenerate' THEN 2
            WHEN LOWER(allowduplicate) = 'f' AND LOWER(allowempty) = 'f' AND datatype = 'c' AND LOWER(hidden) = 'f' THEN 3
            WHEN LOWER(hidden) = 'f' AND datatype = 'c' THEN 4
            ELSE 5
        END AS ord       
    FROM axpflds
    WHERE dcname = 'dc1'
) combined_results
ORDER BY combined_results.name, ord ASC)kf on kf.tstruct = a.name
        left join (select ftransid,'T' dimensions from axgrouptstructs)g on a."name" = g.ftransid
        left join (SELECT DISTINCT ON (formtransid) 
    formtransid, 
    newrecord, 
    permissions,viewctrl
FROM (
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'U' as type, 1 as ord ,'T'permissions,case when viewctrl='4' then 'F' else 'T' end viewctrl
    FROM axpermissions
    WHERE axusername = pusername AND comptype = 'Form'
    UNION ALL
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'R' as type, 2 as ord ,'T' permissions,case when viewctrl='4' then 'F' else 'T' end viewctrl
    FROM axpermissions
    WHERE axuserrole = ANY(string_to_array(puserrole, ',')) and  comptype = 'Form'
) combined_permissions
ORDER BY formtransid, ord ASC)p on a.name=p.formtransid
        WHERE (pmode = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.stype = 't' AND ua.rname = ANY(string_to_array(presponsiblity,','))
              )) order by 1; 

elsif pstype='i' then
  RETURN QUERY
SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [iview]')::varchar,
               a.caption,
               a.name,'i'::varchar stype,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar
        FROM iviews a
        left JOIN axpages b ON b.pagetype = 'i' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name     
        WHERE (lower(pmode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.stype = 'i' AND ua.rname = ANY(string_to_array(presponsiblity,','))
              ))order by 1;
elsif pstype='p' then
  RETURN QUERY
 SELECT DISTINCT
               (b.caption || ' [page]')::varchar,
               b.caption,
               b.props::varchar name,'p'::varchar stype,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar
        FROM  axpages b 
        left JOIN axuseraccess ua ON ua.sname = b.name     
        WHERE b.pagetype='web'
        and (lower(pmode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.rname = ANY(string_to_array(presponsiblity,','))
              ))order by 1; 

elsif pstype='ads' then
  RETURN QUERY
select (a.sqlname || ' (' || a.sqlsrc || ') [ads]')::varchar displaydata,sqlname caption,sqlname name,'ads'::varchar stype,'NA'::varchar dimension,
case when a2.axpermissionsid>0 then 'T' else 'NA' end::varchar permission,
'NA'::varchar createallowed,
case when a2.axpermissionsid>0 or a.createdby=pusername or 'default' = ANY(string_to_array(puserrole,',')) then 'T' else 'NA' end::varchar viewallowed,
'NA'::varchar keyfield,'NA'::varchar primarytable
from axdirectsql a
left join axpermissions a2 on a.sqlname =a2.formcap and (a2.axusername = pusername or a2.axuserrole = ANY(string_to_array(puserrole,',')))
where sqlsrc !='Metadata'
order by 1;

elsif pstype='all' then

return query
select * from(SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [tstruct]')::varchar,
               a.caption tcaption,
               a.name transid,'t'::varchar stype,coalesce(g.dimensions,'F')::varchar dimensions,
    coalesce(p.permissions,'F')::varchar,coalesce(p.newrecord,'T')::varchar,coalesce(p.viewctrl,'T')::varchar,kf.kfld,d.tablename
        FROM tstructs a
join axpdc d on a.name = d.tstruct and d.dname='dc1'
        left JOIN axpages b ON b.pagetype = 't' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name
left join (SELECT DISTINCT ON (combined_results.name) 
    combined_results.name tstruct,  
    kfld, 
    ord
FROM (
    SELECT 
        a.name,
        a.keyfield AS kfld,
        1 AS ord       
    FROM axp_tstructprops a 
    UNION ALL
    SELECT 
        tstruct AS name,
        fname AS kfld,
        CASE 
            WHEN modeofentry = 'autogenerate' THEN 2
            WHEN LOWER(allowduplicate) = 'f' AND LOWER(allowempty) = 'f' AND datatype = 'c' AND LOWER(hidden) = 'f' THEN 3
            WHEN LOWER(hidden) = 'f' AND datatype = 'c' THEN 4
            ELSE 5
        END AS ord       
    FROM axpflds
    WHERE dcname = 'dc1'
) combined_results
ORDER BY combined_results.name, ord ASC)kf on kf.tstruct = a.name
        left join (select ftransid,'T' dimensions from axgrouptstructs)g on a."name" = g.ftransid
        left join (SELECT DISTINCT ON (formtransid) 
    formtransid, 
    newrecord, 
    permissions,viewctrl
FROM (
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'U' as type, 1 as ord ,'T'permissions,case when viewctrl='4' then 'F' else 'T' end viewctrl
    FROM axpermissions
    WHERE axusername = pusername AND comptype = 'Form'
    UNION ALL
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'R' as type, 2 as ord ,'T' permissions,case when viewctrl='4' then 'F' else 'T' end viewctrl
    FROM axpermissions
    WHERE axuserrole = ANY(string_to_array(puserrole, ',')) and  comptype = 'Form'
) combined_permissions
ORDER BY formtransid, ord ASC)p on a.name=p.formtransid
        WHERE (pmode = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.stype = 't' AND ua.rname = ANY(string_to_array(presponsiblity,','))
              ))
union all
SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [iview]')::varchar,
               a.caption,
               a.name,'i'::varchar stype,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar
        FROM iviews a
        left JOIN axpages b ON b.pagetype = 'i' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name     
        WHERE (lower(pmode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.stype = 'i' AND ua.rname = ANY(string_to_array(presponsiblity,','))
              ))
union all
 SELECT DISTINCT
               (b.caption || ' [page]')::varchar,
               b.caption,
               b.props,'p'::varchar stype,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar,'NA'::varchar
        FROM  axpages b 
        left JOIN axuseraccess ua ON ua.sname = b.name     
        WHERE b.pagetype='web'
        and (lower(pmode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.rname = ANY(string_to_array(presponsiblity,','))
              ))
union all
select (a.sqlname || ' (' || a.sqlsrc || ') [ads]')::varchar displaydata,sqlname caption,sqlname name,'ads'::varchar stype,'NA'::varchar dimension,
case when a2.axpermissionsid>0 then 'T' else 'NA' end::varchar permission,
'NA'::varchar createallowed,
case when a2.axpermissionsid>0 or a.createdby=pusername or 'default' = ANY(string_to_array(puserrole,',')) then 'T' else 'NA' end::varchar viewallowed,
'NA'::varchar keyfield,'NA'::varchar primarytable
from axdirectsql a
left join axpermissions a2 on a.sqlname =a2.formcap and (a2.axusername = pusername or a2.axuserrole = ANY(string_to_array(puserrole,',')))
where sqlsrc !='Metadata'
union all
SELECT
               'Inbox'::varchar displaydata,
               'Inbox'::varchar caption,
               'Inbox'::varchar name,'Inbox'::varchar stype,'NA','NA','NA','NA','NA','NA')a order by 1;
elsif pstype='analyse' then 
RETURN QUERY
SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [tstruct]')::varchar,
               a.caption tcaption,
               a.name transid,'t'::varchar stype,coalesce(g.dimensions,'F')::varchar dimensions,
    coalesce(p.permissions,'F')::varchar,coalesce(p.newrecord,'T')::varchar,coalesce(p.viewctrl,'T')::varchar,kf.kfld,d.tablename
        FROM tstructs a
join axpdc d on a.name = d.tstruct and d.dname='dc1'
        left JOIN axpages b ON b.pagetype = 't' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name
left join (SELECT DISTINCT ON (combined_results.name) 
    combined_results.name tstruct,  
    kfld, 
    ord
FROM (
    SELECT 
        a.name,
        a.keyfield AS kfld,
        1 AS ord       
    FROM axp_tstructprops a 
    UNION ALL
    SELECT 
        tstruct AS name,
        fname AS kfld,
        CASE 
            WHEN modeofentry = 'autogenerate' THEN 2
            WHEN LOWER(allowduplicate) = 'f' AND LOWER(allowempty) = 'f' AND datatype = 'c' AND LOWER(hidden) = 'f' THEN 3
            WHEN LOWER(hidden) = 'f' AND datatype = 'c' THEN 4
            ELSE 5
        END AS ord       
    FROM axpflds
    WHERE dcname = 'dc1'
) combined_results
ORDER BY combined_results.name, ord ASC)kf on kf.tstruct = a.name
        left join (select ftransid,'T' dimensions from axgrouptstructs)g on a."name" = g.ftransid
        left join (SELECT DISTINCT ON (formtransid) 
    formtransid, 
    newrecord, 
    permissions,viewctrl
FROM (
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'U' as type, 1 as ord ,'T'permissions,case when viewctrl='4' then 'F' else 'T' end viewctrl
    FROM axpermissions
    WHERE axusername = pusername AND comptype = 'Form'
    UNION ALL
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'R' as type, 2 as ord ,'T' permissions,case when viewctrl='4' then 'F' else 'T' end viewctrl
    FROM axpermissions
    WHERE axuserrole = ANY(string_to_array(puserrole, ',')) and  comptype = 'Form'
) combined_permissions
ORDER BY formtransid, ord ASC)p on a.name=p.formtransid
        WHERE (pmode = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.stype = 't' AND ua.rname = ANY(string_to_array(presponsiblity,','))
              ))
union all
select (a.sqlname || ' (' || a.sqlsrc || ') [ads]')::varchar displaydata,sqlname caption,sqlname name,'ads'::varchar stype,'NA'::varchar dimension,
case when a2.axpermissionsid>0 then 'T' else 'NA' end::varchar permission,
'NA'::varchar createallowed,
case when a2.axpermissionsid>0 or a.createdby=pusername or 'default' = ANY(string_to_array(puserrole,',')) then 'T' else 'NA' end::varchar viewallowed,
'NA'::varchar keyfield,'NA'::varchar primarytable
from axdirectsql a
left join axpermissions a2 on a.sqlname =a2.formcap and (a2.axusername = pusername or a2.axuserrole = ANY(string_to_array(puserrole,',')))
where sqlsrc !='Metadata'
order by 1;
end if;
 
END;
$function$
>>

-- fn_axi_getstructs_obj | replace for primaryfieldvalue+fieldnames and selected fieldvalue with primary fieldvalue suffix
<<
CREATE OR REPLACE FUNCTION fn_axi_getstructs_obj(pcmd character varying, pusername character varying, puserrole character varying, ptransid character varying, pselectedfield character varying, pdimension character varying, ppermission character varying, pkeyfield character varying, pprimarytable character varying, pglobalvars character varying)
 RETURNS TABLE(displaydata text, id text, caption text, isfield text)
 LANGUAGE plpgsql
AS $function$
declare 
v_sql text;
v_keyfield_normalized varchar;
v_keyfield_srctbl varchar;
v_keyfield_srcfld varchar;
v_keyfield_sql text;
v_selectedfld_normalized varchar;
v_selectedfld_srctbl varchar;
v_selectedfld_srcfld varchar;
v_selectedfld_sql text;
v_dimension_filter text;
v_includedcomps text;
v_excludedcomps text;
v_viewctrl varchar;
v_editctrl varchar;
v_finalcomps text;
v_fullcontrol varchar;
v_fieldlist_sql text;
begin
------Get flds metadata for keyfield
select srckey,lower(srctf),lower(srcfld)
into v_keyfield_normalized,v_keyfield_srctbl,v_keyfield_srcfld
from axpflds
where tstruct = ptransid and lower(fname) = lower(pkeyfield);

------Get flds metadata for selected field
select srckey,lower(srctf),lower(srcfld)
into v_selectedfld_normalized,v_selectedfld_srctbl,v_selectedfld_srcfld
from axpflds
where tstruct = ptransid and lower(fname)= lower(pselectedfield); 

------Get dimensions filter condition
if pdimension = 'T' then

 SELECT filtercnd into v_dimension_filter 
 from fn_permissions_getpermission('axi', ptransid, pusername, puserrole, pglobalvars);

 v_dimension_filter := case when length(v_dimension_filter) > 2 then concat(' and ',replace(v_dimension_filter,'{primarytable.}','p.')) end;

end if;

-----Get included and excluded dcs, fields 
if ppermission = 'T' then 

 SELECT lower(concat(view_includedc,view_includeflds)),lower(concat(view_excludedc,view_excludeflds)),viewctrl,editctrl,fullcontrol
 into v_includedcomps,v_excludedcomps,v_viewctrl,v_editctrl ,v_fullcontrol
 from fn_permissions_getpermission('axi', ptransid, pusername, puserrole, pglobalvars);

 if v_fullcontrol = 'T' or v_viewctrl = '0' then
  v_fieldlist_sql := format($sql$
           SELECT (caption || ' (' || fname || ')' || ' [' || 'field' || ']')::text AS displaydata,
                 '0'::text AS id,
                 caption::text AS caption,
                 't'::text AS isfield
            FROM axpflds
            WHERE tstruct = %L
         and dcname='dc1'
            AND hidden = 'F'
            AND savevalue = 'T'       
         $sql$,
         ptransid);
 elsif coalesce(v_fullcontrol,'F') = 'F' and v_viewctrl = '1' then
  v_fieldlist_sql := format($sql$
           SELECT (caption || ' (' || fname || ')' || ' [' || 'field' || ']')::text AS displaydata,
                 '0'::text AS id,
                 caption::text AS caption,
                 't'::text AS isfield,2::numeric ord 
            FROM axpflds
            WHERE tstruct = %L
         and dcname='dc1'
            AND hidden = 'F'
            AND savevalue = 'T' 
         and lower(fname) = ANY(string_to_array(%L,','))       
         $sql$,
         ptransid,
         lower(v_includedcomps));
 elsif coalesce(v_fullcontrol,'F') = 'F' and v_viewctrl = '2' then
   v_fieldlist_sql := format($sql$
            SELECT (caption || ' (' || fname || ')' || ' [' || 'field' || ']')::text AS displaydata,
                  '0'::text AS id,
                  caption::text AS caption,
                  't'::text AS isfield
             FROM axpflds
             WHERE tstruct = %L
          and dcname='dc1'
             AND hidden = 'F'
             AND savevalue = 'T' 
          and lower(fname) != ALL(string_to_array(%L,','))         
          $sql$,
          ptransid,
          v_excludedcomps);

 end if;
else  
 v_fieldlist_sql := format($sql$
      SELECT (caption || ' (' || fname || ')' || ' [' || 'field' || ']')::text AS displaydata,
      '0'::text AS id,
      caption::text AS caption,
      't'::text AS isfield 
      FROM axpflds
      WHERE tstruct = %L
      and dcname='dc1'
      AND hidden = 'F'
      AND savevalue = 'T'          
      $sql$,
      ptransid);

end if;

if v_keyfield_normalized = 'T' then 

v_keyfield_sql := format(
    $sql$
    SELECT   (s.%I)::text AS displaydata,
           '0'::text AS id,
           (s.%I)::text AS caption,
           'f'::text AS isfield
    FROM %I p 
    JOIN %I s ON p.%I = s.%I
    WHERE p.%I IS NOT NULL
 %s
 order by p.modifiedon desc
    $sql$,
 v_keyfield_srcfld,
    v_keyfield_srcfld,          
    lower(pprimarytable), 
    v_keyfield_srctbl, 
    lower(pkeyfield),   
    v_keyfield_srctbl||'id',   
 lower(pkeyfield), 
 v_dimension_filter    
);
else
v_keyfield_sql := format(
    $sql$
    SELECT (p.%I)::text AS displaydata,
           '0'::text AS id,
           p.%I::text AS caption,
           'f'::text AS isfield
    FROM %I p
    WHERE p.%I IS NOT NULL
 %s
 order by p.modifiedon desc
$sql$,
    lower(pkeyfield),
    lower(pkeyfield),
    lower(pprimarytable),
    lower(pkeyfield),
 v_dimension_filter
);

end if;

if pselectedfield!='0' then 
 if v_selectedfld_normalized = 'T' then 
  v_selectedfld_sql := case when v_keyfield_normalized='F' then
         format(
            $sql$
            SELECT --distinct on (p.%I,s.%I) 
           (s.%I || '[' || p.%I || ']')::text AS displaydata,
                   '0'::text AS id,
                   (s.%I)::text AS caption,
                   'f'::text AS isfield
            FROM %I p 
            JOIN %I s ON p.%I = s.%I
            WHERE p.%I IS NOT NULL
         %s  
         order by p.modifiedon desc      
            $sql$, 
         lower(pkeyfield),
         v_selectedfld_srcfld,
         v_selectedfld_srcfld,     
         lower(pkeyfield),
            v_selectedfld_srcfld,     
            lower(pprimarytable), 
            v_selectedfld_srctbl, 
            lower(pselectedfield),   
            v_selectedfld_srctbl||'id',   
            lower(pselectedfield),
         v_dimension_filter  
        ) 
       when v_keyfield_normalized='T' then 
        format(
            $sql$
            SELECT --distinct on (s.%I,k.%I) 
           (s.%I || '[' || k.%I || ']')::text AS displaydata,
                   '0'::text AS id,
                   (s.%I)::text AS caption,
                   'f'::text AS isfield
            FROM %I p 
            JOIN %I s ON p.%I = s.%I
         join %I k on p.%I = k.%I
            WHERE p.%I IS NOT NULL
         %s
         order by p.modifiedon desc 
            $sql$,
         v_selectedfld_srcfld,
         v_keyfield_srcfld,
         v_selectedfld_srcfld,     
         v_keyfield_srcfld,
            v_selectedfld_srcfld,     
            lower(pprimarytable), 
            v_selectedfld_srctbl, 
            lower(pselectedfield),   
            v_selectedfld_srctbl||'id',   
         v_keyfield_srctbl,
         lower(pkeyfield),
         v_keyfield_srctbl||'id',
            lower(pselectedfield),
         v_dimension_filter    
        )
end;
 else
  v_selectedfld_sql := case when v_keyfield_normalized='F' then 
        format(
           $sql$
           SELECT  (p.%I || '[' || p.%I || ']')::text AS displaydata,
                  '0'::text AS id,
                  p.%I::text AS caption,
                  'f'::text AS isfield
           FROM %I p
           WHERE p.%I IS NOT NULL
        %s
        order by p.modifiedon desc,p.%I
       $sql$,
           lower(pselectedfield),
        lower(pkeyfield),
           lower(pselectedfield),
           lower(pprimarytable),
           lower(pselectedfield),
        v_dimension_filter,
        lower(pselectedfield)
       )
       when v_keyfield_normalized='T' then 
        format(
           $sql$
           SELECT (p.%I || '[' || s.%I || ']')::text AS displaydata,
                  '0'::text AS id,
                  p.%I::text AS caption,
                  'f'::text AS isfield
           FROM %I p
        join %I s on p.%I = s.%I
           WHERE p.%I IS NOT NULL
        %s
        order by p.modifiedon desc
        $sql$,
           lower(pselectedfield),
        v_keyfield_srcfld,
        lower(pselectedfield),
        lower(pprimarytable),
        v_keyfield_srctbl,
           lower(pkeyfield),
        v_keyfield_srctbl||'id',
        lower(pkeyfield),
        v_dimension_filter
       ) end;
      
 end if;
end if;


if pselectedfield='0' then
v_sql := concat(' select * from(',v_keyfield_sql,')a',' union all',v_fieldlist_sql);
else
v_sql := v_selectedfld_sql;
end if;
return query execute v_sql;

END; $function$
>>