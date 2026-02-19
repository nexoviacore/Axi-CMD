
-- populate_axdirectsql_metadata
-- to populate metadata based on axdriectsql sql - for tetsing purpose . This data should be enetered manaually by the enduser.

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
        -- extract SELECT column list
        cols :=
            substring(
                r.sqltext
                FROM '(?i)select\s+(.*?)\s+from'
            );

        row_no := 1;

        -- split columns by comma
        FOR col IN
            SELECT trim(value)
            FROM regexp_split_to_table(cols, ',') AS value
        LOOP
            -- handle alias: take name after AS or last token
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
$$;
