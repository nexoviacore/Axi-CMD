-- app-wide settings written by AxiCMD (e.g. category 'axicmd', type 'product', names 'accountname'/'reponame'); username = who last changed it
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
