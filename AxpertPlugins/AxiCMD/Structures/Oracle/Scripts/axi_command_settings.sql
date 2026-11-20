-- app-wide settings written by AxiCMD (e.g. category 'axicmd', type 'product', names 'accountname'/'reponame'/'gittoken'); username = who last changed it
<<
DECLARE
    v_exists NUMBER;
BEGIN
    -- The table holds settings, so it is never dropped: create it only when it is not there yet
    SELECT COUNT(*) INTO v_exists FROM user_tables WHERE table_name = 'AXI_COMMAND_SETTINGS';
    IF v_exists = 0 THEN
        EXECUTE IMMEDIATE q'[CREATE TABLE axi_command_settings (
    id VARCHAR2(36)
        DEFAULT LOWER(
            REGEXP_REPLACE(
                RAWTOHEX(SYS_GUID()),
                '(.{8})(.{4})(.{4})(.{4})(.{12})',
                '\1-\2-\3-\4-\5'
            )
        )
        PRIMARY KEY,
    propertycategory VARCHAR2(100) NOT NULL,
    propertytype VARCHAR2(100) NOT NULL,
    propertyname VARCHAR2(200) NOT NULL,
    propertyvalue VARCHAR2(4000),
    username VARCHAR2(255),
    createdon TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    modifiedon TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_axi_command_settings UNIQUE (propertycategory, propertytype, propertyname)
)]';
    END IF;
END;
>>

-- Golden Dump: Git account, repository and PAT (gittoken) of each install type (product = ProductVersion, plugin, changerequest).
-- Rows are added only when absent, so values already set on a server are kept and a re-run changes nothing.
-- gittoken is shipped empty (NULL in Oracle) on purpose: set the PAT on each server, never in this script.
-- An empty accountname / reponame makes install show "Repository not set" for that type until it is filled in.
<<
MERGE INTO axi_command_settings s
USING (
    SELECT 'axicmd' AS propertycategory, 'product' AS propertytype, 'accountname' AS propertyname, 'Agileaxpert' AS propertyvalue FROM dual UNION ALL
    SELECT 'axicmd', 'product', 'reponame', 'AxiProductVersions' FROM dual UNION ALL
    SELECT 'axicmd', 'product', 'gittoken', NULL FROM dual UNION ALL
    SELECT 'axicmd', 'plugin', 'accountname', NULL FROM dual UNION ALL
    SELECT 'axicmd', 'plugin', 'reponame', NULL FROM dual UNION ALL
    SELECT 'axicmd', 'plugin', 'gittoken', NULL FROM dual UNION ALL
    SELECT 'axicmd', 'changerequest', 'accountname', NULL FROM dual UNION ALL
    SELECT 'axicmd', 'changerequest', 'reponame', NULL FROM dual UNION ALL
    SELECT 'axicmd', 'changerequest', 'gittoken', NULL FROM dual
) v
ON (s.propertycategory = v.propertycategory AND s.propertytype = v.propertytype AND s.propertyname = v.propertyname)
WHEN NOT MATCHED THEN
    INSERT (propertycategory, propertytype, propertyname, propertyvalue, username)
    VALUES (v.propertycategory, v.propertytype, v.propertyname, v.propertyvalue, 'admin')
>>
