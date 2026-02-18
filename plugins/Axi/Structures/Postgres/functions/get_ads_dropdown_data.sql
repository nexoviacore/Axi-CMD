-- DROP FUNCTION get_ads_dropdown_data(text, text);

CREATE OR REPLACE FUNCTION get_ads_dropdown_data(p_tablename text, p_fieldname text)
 RETURNS TABLE(displaydata text, name text)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY EXECUTE format(
        'SELECT %I::text AS displaydata, %I::text AS name FROM %I Where %I Is Not Null',
        p_fieldname,
        p_fieldname,
        p_tablename,
        p_fieldname
    );
END;
$function$
;
