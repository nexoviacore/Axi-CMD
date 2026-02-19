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
$$;
