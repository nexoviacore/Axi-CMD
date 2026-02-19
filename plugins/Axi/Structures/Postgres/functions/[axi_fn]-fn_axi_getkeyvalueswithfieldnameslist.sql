


CREATE OR REPLACE FUNCTION axidev.fn_axi_getkeyvalueswithfieldnameslist(p_tstruct text, p_fieldname text)
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
  
    -- 1. Try to get keyfield from axp_tstructprops1
    SELECT keyfield
    INTO v_keyfield
    FROM axp_tstructprops
    WHERE name = p_tstruct
    LIMIT 1;

    -- 2. If not found, get from axpflds based on rules
    IF v_keyfield IS NULL then
       
        --SELECT fname INTO v_keyfield  FROM axpflds WHERE tstruct = p_tstruct
        --AND dcname = 'dc1' and (modeofentry = 'autogenerate' OR (((LOWER(allowduplicate) = 'f') AND (LOWER(allowempty) = 'f') 
		--AND datatype = 'c') AND LOWER(hidden) = 'f')) 
		--ORDER BY ordno ASC,CASE WHEN modeofentry = 'autogenerate' THEN 1 
		--WHEN LOWER(allowduplicate) = 'f' THEN 2 WHEN datatype = 'c' THEN 3 ELSE 4 END, ordno ASC LIMIT 1;
    
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

   
-- Case 1: Field-based source
-- Case 1: Field-based source
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

    ORDER BY isfield ASC, displaydata ASC
    $sql$,
    p_fieldname,
    v_keyfield,
    v_tablename,
    p_fieldname,
    p_tstruct
);

-- Case 2: Table-based source
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
;
