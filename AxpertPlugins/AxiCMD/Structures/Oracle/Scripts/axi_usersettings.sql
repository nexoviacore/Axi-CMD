-- Per-user AxiCMD settings (e.g. SettingKey 'pagesize'), one row per user and key.
-- The table holds user settings, so it is never dropped: it is created only when it is not there yet.
<<
DECLARE
    v_exists NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_exists FROM user_tables WHERE table_name = 'AXI_USERSETTINGS';
    IF v_exists = 0 THEN
        EXECUTE IMMEDIATE q'[CREATE TABLE Axi_UserSettings (
    Id VARCHAR2(36)
        DEFAULT LOWER(
            REGEXP_REPLACE(
                RAWTOHEX(SYS_GUID()),
                '(.{8})(.{4})(.{4})(.{4})(.{12})',
                '\1-\2-\3-\4-\5'
            )
        )
        PRIMARY KEY,

    UserName VARCHAR2(255) NOT NULL,
    SettingKey VARCHAR2(100) NOT NULL,
    SettingValue VARCHAR2(4000),
    ModifiedOn TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_axi_usersettings UNIQUE (UserName, SettingKey)
)]';
    END IF;
END;
>>
