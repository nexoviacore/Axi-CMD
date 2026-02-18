-- DROP FUNCTION fn_axi_get_fieldvalues_with_keysuffix_list(text, text);

CREATE OR REPLACE FUNCTION fn_axi_get_fieldvalues_with_keysuffix_list(p_tstruct text, p_fieldname text)
 RETURNS TABLE(displaydata text, id text, caption text)
 LANGUAGE plpgsql
AS $function$
DECLARE 
    v_sql text; 
    v_tablename text; 
    v_sourcekey text; 
    v_srcfld text; 
    v_srctf text; 
    v_keyfield text; 
BEGIN 
    -- 1. Try to get keyfield from axp_tstructprops1 
    SELECT keyfield INTO v_keyfield 
    FROM axp_tstructprops 
    WHERE name = p_tstruct 
    LIMIT 1; 

    -- 2. If not found, get from axpflds based on rules 
    IF v_keyfield IS NULL THEN 
        --SELECT fname INTO v_keyfield 
        --FROM axpflds 
        --WHERE tstruct = p_tstruct 
          --AND dcname = 'dc1' 
          --AND (
            --  modeofentry = 'autogenerate' OR 
              --(
                --LOWER(allowduplicate) = 'f' AND 
                --LOWER(allowempty) = 'f' AND 
                --datatype = 'c' AND 
                --LOWER(hidden) = 'f'
              --)
          --)
        --ORDER BY ordno ASC,
          --       CASE 
            --         WHEN modeofentry = 'autogenerate' THEN 1 
              --       WHEN LOWER(allowduplicate) = 'f' THEN 2 
                --     WHEN datatype = 'c' THEN 3 
                  --   ELSE 4 
                 --END, 
               --  ordno ASC 
--        LIMIT 1; 
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
    SELECT tablename INTO v_tablename 
    FROM axpdc 
    WHERE tstruct = p_tstruct 
      AND dname = 'dc1'; 

    -- Get source metadata 
    SELECT srckey, srcfld, srctf INTO v_sourcekey, v_srcfld, v_srctf 
    FROM axpflds 
    WHERE tstruct = p_tstruct 
      AND fname = p_fieldname; 

    -- Validate before dynamic SQL
    IF v_keyfield IS NULL
       OR v_tablename IS NULL
       OR v_sourcekey IS NULL THEN
        RETURN;
    END IF;

    -- Case 1: Field-based source
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

    -- Case 2: Table-based source
    ELSE
        v_sql := format(
            'SELECT (%I || ''['' || %I || '']'')::text AS displaydata,
                    %I::text AS id,
                    %I::text AS caption
             FROM %I
             WHERE %I IS NOT NULL
             ORDER BY displaydata ASC',
            v_srcfld,
            v_keyfield,
            v_keyfield,
            lower(v_srctf) || 'id',
            lower(v_srctf),
            p_fieldname
        );
    END IF;

    RETURN QUERY EXECUTE v_sql; 
END; 
$function$
;
