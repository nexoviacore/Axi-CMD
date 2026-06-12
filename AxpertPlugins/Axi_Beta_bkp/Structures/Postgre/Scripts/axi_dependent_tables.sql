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

--This may not be needed
DROP TABLE axiconfig; 

<<
CREATE TABLE axiconfig (axienabled varchar(1), mainpagetemplate varchar(255)); 
>>

<<
INSERT INTO axiconfig (axienabled, mainpagetemplate) VALUES ('T','AxiCMDMainPage.html'); 
>>