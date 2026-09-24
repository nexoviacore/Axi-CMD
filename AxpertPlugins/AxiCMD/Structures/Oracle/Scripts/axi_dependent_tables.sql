
-- <<
-- CREATE TABLE axiconfig (axienabled varchar2(1), mainpagetemplate varchar2(255))
-- >>

-- <<
-- DELETE FROM axiconfig where  mainpagetemplate = 'AxiCMDMainPage.html'
-- <<


-- <<
-- INSERT INTO axiconfig (axienabled, mainpagetemplate) VALUES ('T','AxiCMDMainPage.html') 
-- >>


-- TODO: Remove this on next release. 
<<
DROP TABLE axiconfig;
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
ALTER TABLE AXI_USERFAVOURITES  ADD originalcommandtext VARCHAR2(4000) NULL
>>

-- TODO: remove this in the next release
<<
DELETE FROM AXPSTRUCTCONFIG WHERE PROPVALUE1 = 'axicmdmainpage.html';
>>

-- Insert AxiCMD Developer Option into axpstructconfigprops table
<<
INSERT INTO axpstructconfigprops (axpstructconfigpropsid, cancel, sourceid, mapname, username, modifiedon, createdby, createdon, wkid, app_level, app_desc, app_slevel, cancelremarks, wfroles, configprops, propcode, description, dupchk, context, ptype, caction, chyperlink, cfields, alltstructs, alliviews, alluserroles)
VALUES (1844770000001, 'F', 0, NULL, 'admin', TO_DATE('2026-09-23 12:37:37', 'YYYY-MM-DD HH24:MI:SS'), 'admin', TO_DATE('2026-09-23 12:00:24', 'YYYY-MM-DD HH24:MI:SS'), NULL, 1, 1, NULL, NULL, NULL, 'AxiCMD', 'General', 'AxiCMD Developer Option', 'configtypeAxiCMD', NULL, 'All', 'F', 'F', 'T', 'F', 'T', 'F')
>>

-- Insert AxiCMD Developer Option into axpstructconfigproval table
<<
INSERT INTO axpstructconfigproval (axpstructconfigprovalid, axpstructconfigpropsid, axpstructconfigprovalrow, configvalues)
VALUES (1844770000002, 1844770000001, 1, 'True')
>>

<<
INSERT INTO axpstructconfig (axpstructconfigid,cancel,sourceid,mapname,username,modifiedon,createdby,createdon,wkid,app_level,app_desc,app_slevel,cancelremarks,wfroles,asprops,setype,props,context,propvalue1,uploadfiletype,propvalue2,propsval,alluserroles,structcaption,structname,structelements,structelements1,sfield,icolumn,sbutton,hlink,stype,userroles,dupchk,purpose) VALUES (1016010000022,'F',0,NULL,'admin',TO_DATE('2026-09-24 00:00:00','YYYY-MM-DD HH24:MI:SS'),'admin',TO_DATE('2026-09-24 00:00:00','YYYY-MM-DD HH24:MI:SS'),NULL,1,1,NULL,NULL,NULL,'Axi CMD Option','All','General',NULL,'true',NULL,NULL,'true',NULL,'ALL Forms','ALL Forms',NULL,NULL,NULL,NULL,NULL,NULL,'All','ALL','Axi CMD OptionALL FormstrueALL',NULL)
>>


