-- app-wide settings written by AxiCMD (e.g. category 'axicmd', type 'product', names 'accountname'/'reponame'); username = who last changed it
<<
CREATE TABLE IF NOT EXISTS axi_command_settings (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    propertycategory VARCHAR(100) NOT NULL,
    propertytype VARCHAR(100) NOT NULL,
    propertyname VARCHAR(200) NOT NULL,
    propertyvalue VARCHAR(4000),
    username VARCHAR(255),
    createdon TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    modifiedon TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_axi_command_settings UNIQUE (propertycategory, propertytype, propertyname)
)
>>
