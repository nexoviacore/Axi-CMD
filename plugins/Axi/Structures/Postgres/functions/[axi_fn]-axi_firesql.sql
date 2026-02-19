
--axi_firesql

/*
✅ Exact SELECT column order preserved
✅ First selected column renamed to displaydata
✅ NULL → ""
✅ Numbers → string
✅ Boolean → string
✅ No extra quotes
✅ Works even with a.username, a.* pattern
*/

CREATE OR REPLACE FUNCTION axi_firesql(
    p_sql TEXT,
    p_param_string TEXT
)
RETURNS JSON
LANGUAGE plpgsql
AS
$$
DECLARE
    v_sql TEXT := p_sql;
    v_pair TEXT;
    v_pairs TEXT[];
    v_param_name TEXT;
    v_param_value TEXT;

    v_final_sql TEXT;
    v_result JSON;
BEGIN

    ----------------------------------------------------------------
    -- 1. Replace parameters (unchanged)
    ----------------------------------------------------------------
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

    ----------------------------------------------------------------
    -- 2. Preserve TRUE column order
    ----------------------------------------------------------------
    v_final_sql := '
        SELECT json_build_object(
            ''data'',
            COALESCE(json_agg(row_data), ''[]''::json)
        )
        FROM (
            SELECT row_id,
                   json_object_agg(
                       CASE 
                           WHEN ordinality = 1 THEN ''displaydata''
                           ELSE key
                       END,
                       COALESCE(value, '''')
                       ORDER BY ordinality
                   ) AS row_data
            FROM (
                SELECT 
                    row_number() OVER () AS row_id,
                    js
                FROM (
                    SELECT row_to_json(q) AS js
                    FROM (' || v_sql || ') q
                ) x
            ) r,
            LATERAL json_each_text(r.js) WITH ORDINALITY
            GROUP BY row_id
        ) s
    ';

    EXECUTE v_final_sql INTO v_result;

    RETURN v_result;

END;
$$;

