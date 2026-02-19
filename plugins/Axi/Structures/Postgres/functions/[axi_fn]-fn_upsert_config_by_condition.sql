
CREATE OR REPLACE FUNCTION fn_upsert_config_by_condition(
    p_tablename      text,
    p_fieldnames     text,
    p_fieldvalues    text,
    p_where_clause   text
)
RETURNS TABLE(status text)--RETURNS void
LANGUAGE plpgsql
AS $function$
DECLARE
    v_exists     boolean;
    v_set_clause text;
    v_cols_arr   text[];
    v_vals_arr   text[];
BEGIN
    -- Convert to arrays
    v_cols_arr := string_to_array(p_fieldnames, ',');
    v_vals_arr := string_to_array(p_fieldvalues, ',');

    -- Validate array length
    IF array_length(v_cols_arr, 1)
       IS DISTINCT FROM
       array_length(v_vals_arr, 1) THEN
        RAISE EXCEPTION
            'Field names and values count mismatch';
    END IF;

    -- Build SET clause (values already quoted by caller)
    SELECT string_agg(
               format('%I=%s', trim(col), trim(val)),
               ', '
           )
    INTO v_set_clause
    FROM unnest(v_cols_arr, v_vals_arr) AS t(col, val)
    WHERE col IS NOT NULL
      AND trim(col) <> '';

    -- Check existence
    EXECUTE format(
        'SELECT EXISTS (SELECT 1 FROM %I WHERE %s)',
        p_tablename,
        p_where_clause
    )
    INTO v_exists;

    -- UPDATE or INSERT
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
$function$;

