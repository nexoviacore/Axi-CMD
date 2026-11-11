-- Per-user AxiCMD settings (e.g. SettingKey 'pagesize'), one row per user and key.
-- The table holds user settings, so it is never dropped: it is created only when it is not there yet.
<<
CREATE TABLE IF NOT EXISTS Axi_UserSettings (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    UserName VARCHAR(255) NOT NULL,
    SettingKey VARCHAR(100) NOT NULL,
    SettingValue VARCHAR(4000),
    ModifiedOn TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_axi_usersettings UNIQUE (UserName, SettingKey)
)
>>
