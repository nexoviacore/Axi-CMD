CREATE TABLE axiconfig (axienabled varchar2(1), mainpagetemplate varchar2(255)); 

INSERT INTO axiconfig (axienabled, mainpagetemplate) VALUES ('T','AxiCMDMainPage.html'); 

CREATE TABLE Axi_UserFavourites (
    Id VARCHAR2(36) PRIMARY KEY,
    UserName VARCHAR2(255) NOT NULL,
    CommandText CLOB NOT NULL,
    TargetURL VARCHAR2(4000) NOT NULL,
    FavOrder NUMBER(10),
    CreatedOn TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_user_command UNIQUE (UserName, Id)
);
