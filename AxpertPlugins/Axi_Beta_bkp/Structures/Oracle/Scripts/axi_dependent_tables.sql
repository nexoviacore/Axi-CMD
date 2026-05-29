
<<
CREATE TABLE axiconfig (axienabled varchar2(1), mainpagetemplate varchar2(255))
>>

<<
DELETE FROM axiconfig where  mainpagetemplate = 'AxiCMDMainPage.html'
<<


<<
INSERT INTO axiconfig (axienabled, mainpagetemplate) VALUES ('T','AxiCMDMainPage.html') 
>>

<<
CREATE TABLE Axi_UserFavourites (
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
    CommandText VARCHAR2(4000) NOT NULL,
    TargetURL VARCHAR2(4000) NOT NULL,
    FavOrder NUMBER(10),
    CreatedOn TIMESTAMP DEFAULT CURRENT_TIMESTAMP
)
>>

<<
ALTER TABLE AXI_USERFAVOURITES  ADD originalcommandtext varchar(4000) NULL
>>