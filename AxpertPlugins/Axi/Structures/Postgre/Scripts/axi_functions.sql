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
drop function fn_axi_getkeyvalueswithfieldnameslist_v2
>>

<<
drop function fn_axi_get_fieldvalues_with_keysuffix_list_v2
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


--fn_axi_getstructures_meta with userpermission meta data
<<
CREATE OR REPLACE FUNCTION fn_axi_getstructures_meta(pusername character varying, puserrole character varying, presponsiblity character varying, pmode character varying, pstype character varying)
 RETURNS TABLE(displaydata character varying, caption character varying, name character varying, stype character varying, dimension character varying, permission character varying, createallowed character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN

if pstype='t' then
    RETURN QUERY
SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [tstruct]')::varchar,
               a.caption,
               a.name,'t'::varchar stype,coalesce(g.dimensions,'F')::varchar dimensions,coalesce(p.permissions,'F')::varchar,coalesce(p.newrecord,'T')::varchar
        FROM tstructs a
        left JOIN axpages b ON b.pagetype = 't' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name
        left join (select ftransid,'T' dimensions from axgrouptstructs)g on a."name" = g.ftransid
        left join (SELECT DISTINCT ON (formtransid) 
    formtransid, 
    newrecord, 
    permissions
FROM (
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'U' as type, 1 as ord ,'T'permissions
    FROM axpermissions
    WHERE axusername = pusername AND comptype = 'Form'
    UNION ALL
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'R' as type, 2 as ord ,'T' permissions
    FROM axpermissions
    WHERE axuserrole = ANY(string_to_array(puserrole, ',')) and	 comptype = 'Form'
) combined_permissions
ORDER BY formtransid, ord ASC)p on a.name=p.formtransid
        WHERE (pmode = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.stype = 't' AND ua.rname = ANY(string_to_array(presponsiblity,','))
              )); 

elsif pstype='i' then
  RETURN QUERY
SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [iview]')::varchar,
               a.caption,
               a.name,'i'::varchar stype,null::varchar,null::varchar,null::varchar
        FROM iviews a
        left JOIN axpages b ON b.pagetype = 'i' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name     
        WHERE (lower(pmode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.stype = 't' AND ua.rname = ANY(string_to_array(presponsiblity,','))
              ));
elsif pstype='p' then
  RETURN QUERY
 SELECT DISTINCT
               (b.caption || ' [page]')::varchar,
               b.caption,
               b.name,'p'::varchar stype,null::varchar,null::varchar,null::varchar
        FROM  axpages b 
        JOIN axuseraccess ua ON ua.sname = b.name     
        WHERE b.pagetype='web'
        and (lower(pmode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.rname = ANY(string_to_array(presponsiblity,','))
              )); 

elsif pstype='ads' then
  RETURN QUERY
select (a.sqlname || ' (' || a.sqlsrc || ') [ads]')::varchar,sqlname,sqlname,'ads'::varchar,'F'::varchar,'T'::varchar,
null::varchar from axdirectsql a
join axpermissions a2 on a.sqlname =a2.formcap 
where (a2.axusername = pusername or a2.axuserrole = ANY(string_to_array(puserrole,',')));
elsif pstype='all' then

return query
SELECT DISTINCT
               (a.caption || ' (' || a.name || ') [tstruct]')::varchar,
               a.caption,
               a.name,'t'::varchar stype,coalesce(g.dimensions,'F')::varchar dimensions,coalesce(p.permissions,'F')::varchar,coalesce(p.newrecord,'T')::varchar
        FROM tstructs a
        left JOIN axpages b ON b.pagetype = 't' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name
        left join (select ftransid,'T' dimensions from axgrouptstructs)g on a."name" = g.ftransid
        left join (SELECT DISTINCT ON (formtransid) 
    formtransid, 
    newrecord, 
    permissions
FROM (
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'U' as type, 1 as ord ,'T'permissions
    FROM axpermissions
    WHERE axusername = pusername AND comptype = 'Form'
    UNION ALL
    SELECT formtransid, case when allowcreate='Yes' then 'T' else 'F' end newrecord, 'R' as type, 2 as ord ,'T' permissions
    FROM axpermissions
    WHERE axuserrole = ANY(string_to_array(puserrole, ',')) and	 comptype = 'Form'
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
               a.name,'i'::varchar,null dimensions,null,null
        FROM iviews a
        left JOIN axpages b ON b.pagetype = 'i' || a.name
        LEFT JOIN axuseraccess ua ON ua.sname = a.name     
        WHERE (lower(pmode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.stype = 't' AND ua.rname = ANY(string_to_array(presponsiblity,','))
              ))
union all
SELECT DISTINCT
               (b.caption || ' [page]')::varchar,
               b.caption,
               b.name,'p'::varchar,null dimensions,null,null
        FROM  axpages b 
        JOIN axuseraccess ua ON ua.sname = b.name     
        WHERE b.pagetype='web'
        and (lower(pmode) = 'dev' OR b.visible = 'T')
          AND (
                'default' = ANY(string_to_array(presponsiblity,','))
                OR (ua.rname = ANY(string_to_array(presponsiblity,','))
              ))
union all
select (a.sqlname || ' (' || a.sqlsrc || ') [ads]'),sqlname,sqlname,'ads'::varchar,'F'::varchar,'T'::varchar,null from axdirectsql a
join axpermissions a2 on a.sqlname =a2.formcap 
where (a2.axusername = pusername or a2.axuserrole = ANY(string_to_array(puserrole,',')))
union all
SELECT
               'Inbox'::varchar displaydata,
               'Inbox'::varchar caption,
               'Inbox'::varchar name,'Inbox'::varchar stype,null,null,null;
end if;
 
END;
$function$
>>

