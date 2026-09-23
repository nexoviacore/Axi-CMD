<<
CREATE EXTENSION IF NOT EXISTS "uuid-ossp"
>>

--This may not be needed
DROP TABLE Axi_UserFavourites


<<
CREATE TABLE Axi_UserFavourites (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    UserName VARCHAR(255) NOT NULL,
    CommandText TEXT NOT NULL,
    TargetURL VARCHAR(4000) NOT NULL,
    FavOrder INT,
    CreatedOn TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_user_command UNIQUE (UserName, CommandText)
)
>>

<<
ALTER TABLE axi_userfavourites ADD originalcommandtext varchar(500) NULL;
>>

<<
--This may not be needed
DROP TABLE axiconfig; 
>>

-- TODO: Remove this in the next release
<<
DELETE FROM axpstructconfig WHERE propvalue1 = 'axicmdmainpage.html';
>>

-- <<
-- CREATE TABLE axiconfig (axienabled varchar(1), mainpagetemplate varchar(255)); 
-- >>

-- <<
-- INSERT INTO axiconfig (axienabled, mainpagetemplate) VALUES ('T','AxiCMDMainPage.html'); 
-- >>

-- Insert AxiCMD Developer Option into axpstructconfigprops table
<<
INSERT INTO axpstructconfigprops (axpstructconfigpropsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, configprops, propcode, description, dupchk, context, ptype, caction, chyperlink, cfields, alltstructs, alliviews, alluserroles)
VALUES (1844770000001, 'F', 0, NULL, 'admin', '2026-09-23 12:37:37', 'admin', '2026-09-23 12:00:24', NULL, 1, 1, NULL, NULL, NULL, 'AxiCMD', 'General', 'AxiCMD Developer Option', 'configtypeAxiCMD', NULL, 'All', 'F', 'F', 'T', 'F', 'T', 'F');
>>

-- Insert AxiCMD Developer Option into axpstructconfigproval table
<<
INSERT INTO axpstructconfigproval (axpstructconfigprovalid, axpstructconfigpropsid, axpstructconfigprovalrow, configvalues)
VALUES (1844770000002, 1844770000001, 1, 'True');
>>
