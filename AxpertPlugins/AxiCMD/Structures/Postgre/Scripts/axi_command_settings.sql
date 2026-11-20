-- app-wide settings written by AxiCMD (e.g. category 'axicmd', type 'product', names 'accountname'/'reponame'/'gittoken'); username = who last changed it
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

-- Golden Dump: Git account, repository and PAT (gittoken) of each install type (product = ProductVersion, plugin, changerequest).
-- Rows are added only when absent, so values already set on a server are kept and a re-run changes nothing.
-- gittoken is shipped empty on purpose: set the PAT on each server, never in this script.
-- An empty accountname / reponame makes install show "Repository not set" for that type until it is filled in.
<<
INSERT INTO axi_command_settings (propertycategory, propertytype, propertyname, propertyvalue, username)
SELECT v.propertycategory, v.propertytype, v.propertyname, v.propertyvalue, 'admin'
FROM (VALUES
    ('axicmd', 'product', 'accountname', 'salmanagilelabs'),
    ('axicmd', 'product', 'reponame', 'AxiProduct'),
    ('axicmd', 'product', 'gittoken', ''),
    ('axicmd', 'plugin', 'accountname', 'salmanagilelabs'),
    ('axicmd', 'plugin', 'reponame', 'AxiPlugin'),
    ('axicmd', 'plugin', 'gittoken', ''),
    ('axicmd', 'changerequest', 'accountname', 'salmanagilelabs'),
    ('axicmd', 'changerequest', 'reponame', 'AxiChangeRequest'),
    ('axicmd', 'changerequest', 'gittoken', '')
) AS v(propertycategory, propertytype, propertyname, propertyvalue)
WHERE NOT EXISTS (SELECT 1 FROM axi_command_settings s WHERE s.propertycategory = v.propertycategory AND s.propertytype = v.propertytype AND s.propertyname = v.propertyname)
>>
