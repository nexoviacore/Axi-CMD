CREATE OR REPLACE FUNCTION pgbase114.get_dynamic_field(p_tstruct text, p_fieldname text)
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
  -- Get base table name
  SELECT tablename
  INTO v_tablename
  FROM axpdc
  WHERE tstruct = p_tstruct
    AND dname = 'dc1';

  -- Get source metadata
  SELECT srckey, srcfld, srctf
  INTO v_sourcekey, v_srcfld, v_srctf
  FROM axpflds
  WHERE tstruct = p_tstruct
    AND fname   = p_fieldname;
   
  -- Case 1: Field-based source
  IF v_sourcekey = 'F' THEN
    v_sql := format(
      'SELECT %I::text, ''0''::text AS id FROM %I',
      p_fieldname,
      v_tablename
    );

  -- Case 2: Table-based source
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
;

-- Permissions

ALTER FUNCTION pgbase114.get_dynamic_field(text, text) OWNER TO pgbase114;
GRANT ALL ON FUNCTION pgbase114.get_dynamic_field(text, text) TO pgbase114;
