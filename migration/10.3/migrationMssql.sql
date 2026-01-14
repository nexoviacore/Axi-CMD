
DROP TABLE AX_HP_USER_LEVEL_WIDGET;
DROP TABLE AX_PAGE_RESPONSIBILITY;
DROP TABLE AX_WIDGET_PUBLISHED;
DROP TABLE AX_PAGES;

DROP TABLE AX_PAGE_SD_RESPONSIBILITY;
DROP TABLE AX_WIDGET_SAVED;
DROP TABLE AX_PAGE_SAVED;

DROP TABLE AX_PAGE_TEMPLATES;

CREATE TABLE AX_PAGES(
    PAGE_ID INT IDENTITY(1,1) PRIMARY KEY,
    TITLE VARCHAR(255),
    TYPE VARCHAR(255),
    MODULE VARCHAR(255),
    TEMPLATE VARCHAR(255),
    PAGE_MENU VARCHAR(255),
    CONTENT TEXT,
    CREATED_BY VARCHAR(255),
    UPDATED_BY VARCHAR(255),
    IS_DELETED VARCHAR(1) DEFAULT 'N',
    IS_DEFAULT VARCHAR(1) DEFAULT 'N',
    IS_PRIVATE VARCHAR(1) DEFAULT 'N',
    CREATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP,  
    IS_MIGRATED VARCHAR(1) DEFAULT 'N',
    ORDER_BY INT
);

CREATE TABLE AX_PAGE_SAVED(
    PAGE_ID INT IDENTITY(1,1) PRIMARY KEY,
    TITLE VARCHAR(255),
    TYPE VARCHAR(255),
    MODULE VARCHAR(255),
    TEMPLATE VARCHAR(255),
    PAGE_MENU VARCHAR(255),
    CONTENT TEXT,
    CREATED_BY VARCHAR(255),
    UPDATED_BY VARCHAR(255),
    IS_DELETED VARCHAR(1) DEFAULT 'N',
    IS_DEFAULT VARCHAR(1) DEFAULT 'N',
    CREATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP,  
    IS_MIGRATED VARCHAR(1) DEFAULT 'N',
    IS_PUBLISH VARCHAR(1) DEFAULT 'N',
    IS_PRIVATE VARCHAR(1) DEFAULT 'N',
    PARENT_PAGE_ID INT,
    RESPONSIBILITY TEXT,
    ORDER_BY INT
);

CREATE TABLE AX_PAGE_RESPONSIBILITY(     
    PAGE_ID INT REFERENCES AX_PAGES(PAGE_ID) ON DELETE CASCADE,
    RESPONSIBILITY VARCHAR(255),
    RESPONSIBILITY_ID INT
);

CREATE TABLE AX_PAGE_SD_RESPONSIBILITY(  
    PAGE_ID INT REFERENCES AX_PAGE_SAVED(PAGE_ID) ON DELETE CASCADE,
    RESPONSIBILITY VARCHAR(255),
    RESPONSIBILITY_ID INT
);


CREATE TABLE AX_WIDGET_SAVED (
    WIDGET_ID INT IDENTITY(1,1) PRIMARY KEY,
    TITLE VARCHAR(255),
    WIDGET_TYPE VARCHAR(255),
    CONTENT TEXT,
    TARGET VARCHAR(255),
    IS_PRIVATE VARCHAR(1) DEFAULT 'N',
    CREATED_BY VARCHAR(255),
    UPDATED_BY VARCHAR(255),
    IS_DELETED VARCHAR(1) DEFAULT 'N',
    IS_LOCK VARCHAR(1) DEFAULT 'N',
    ORDER_BY INT,
    is_publish VARCHAR(1) DEFAULT 'N',
    PARENT_WIDGET_ID INT,
    PAGE_ID INT REFERENCES AX_PAGE_SAVED(PAGE_ID) ON DELETE CASCADE,
    CREATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP, 
    IS_MIGRATED VARCHAR(1) DEFAULT 'N'
);

CREATE TABLE AX_WIDGET_PUBLISHED (
    WIDGET_ID INT IDENTITY(1,1) PRIMARY KEY,
    TITLE VARCHAR(255),
    WIDGET_TYPE VARCHAR(255),
    CONTENT TEXT,
    TARGET VARCHAR(255),
    IS_PRIVATE VARCHAR(1) DEFAULT 'N',
    CREATED_BY VARCHAR(255),
    UPDATED_BY VARCHAR(255),
    IS_DELETED VARCHAR(1) DEFAULT 'N',
    IS_LOCK VARCHAR(1) DEFAULT 'N',
    ORDER_BY INT,
    is_publish VARCHAR(1) DEFAULT 'N',
    PARENT_WIDGET_ID INT REFERENCES AX_WIDGET_SAVED(WIDGET_ID) ON DELETE CASCADE,
    PAGE_ID INT REFERENCES AX_PAGES(PAGE_ID) ON DELETE CASCADE,
    CREATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP, 
    IS_MIGRATED VARCHAR(1) DEFAULT 'N'
);

CREATE TABLE AX_HP_USER_LEVEL_WIDGET(
    PAGE_ID INT REFERENCES AX_PAGES(PAGE_ID) ON DELETE CASCADE,    
    WIDGETS  TEXT,
    USERNAME VARCHAR(255),
    IS_DELETED VARCHAR(1) DEFAULT 'N',
    CREATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP,
);

CREATE TABLE AX_PAGE_TEMPLATES(
    TEMPLATE_ID INT IDENTITY(1,1) PRIMARY KEY,
    TITLE VARCHAR(255),
    MODULE VARCHAR(255),
    CONTENT TEXT,
    CREATED_BY VARCHAR(255),
    UPDATED_BY VARCHAR(255),
    IS_DELETED VARCHAR(1) DEFAULT 'N',
    CREATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON DATETIME DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('basic','{"cc":1,"img":"basic.png","name":"basic","cf":[{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('modern','{"cc":1,"img":"modern.png","name":"modern","cf":[{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","br":"14px","tr":{"p":"top","h":"70px","html":"<span style=\"float:left;font-size:45px;\">#TITLE_ICON#</span><span style=\"text-align: right;padding-top: 14px;position: relative;top: 16px;right: 5px;\">#TITLE_NAME#</span>"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('mainCard','{"cc":7,"img":"mainCard.png","name":"mainCard","cf":[{"ht":"225px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('topaz','{"cc":5,"img":"topaz.png","name":"topaz","cf":[{"ht":"225px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m8 l8","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m8 l8","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('list','{"cc":1,"img":"list.png","name":"list","cf":[{"ht":"225px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('flow','{"cc":3,"img":"flow.png","name":"flow","repeatLastWidget":true,"cf":[{"ht":"500px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"500px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('checkered','{"cc":1,"img":"checkered.png","name":"checkered","cf":[{"ht":"300px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('flow main','{"cc":3,"img":"flow_main.png","name":"flow main","repeatLastWidget":true,"cf":[{"ht":"300px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"300px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('random','{"cc":7,"img":"random.png","name":"random","cf":[{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"300px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"300px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"200px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');



/* =============================================  
Name            : axp_pr_page_creation
Author      : Abhishek 
Create date : 27-09-2018  
Description : To create or delete a page  

Example     :
page creation:
    exec axp_pr_page_creation 'PageTest7','Test13','w3test1','Head16','before', 'page.aspx?transid=page'
    Result      :
        success - 'Page created'
        failure - 'Page not found'

 page deletion:
    exec axp_pr_page_creation 'PageTest5', null, null, null,'delete', null
    Result      :
            success - 'Page deleted'
            failure - 'Page not found'
===============================================  */
DROP PROCEDURE [axp_pr_page_creation];

CREATE PROCEDURE [axp_pr_page_creation](
        @pname VARCHAR(100),            -- New Page Name/Delete Page Name
        @pcaption VARCHAR(100),     -- New Page Caption
        @ppagetype VARCHAR(100),    -- New Page Type
        @pparentname VARCHAR(100),  -- Parent Name
        @paction VARCHAR(100),      -- Before/After inserting or delete
        @props varchar(100)         -- props
          ) 
AS
BEGIN
        DECLARE @orderno as int 
        DECLARE @level   as int
        DECLARE @sysdate as VARCHAR(100) 
        
        IF @paction <> 'delete'
            BEGIN
                SELECT @pparentname=parent, @orderno= ordno, @level=levelno FROM 
                (
                SELECT 
                        parent, 
                         CASE 
                                WHEN LOWER (@paction) = 'before' THEN ordno 
                                ELSE ordno + 1 
                        END ordno,
                          levelno
                FROM axpages
                WHERE name = @pparentname AND TYPE = 'p'
                
                UNION ALL
                
                SELECT A.NAME, 
                       MAX(B.ORDNO) + 1 AS ORDNO, 
                       A.LEVELNO + 1 AS LEVELNO
                FROM AXPAGES AS A
                        LEFT OUTER JOIN AXPAGES AS B ON A.NAME = B.PARENT
                        WHERE A.NAME = @pparentname AND A.TYPE = 'h'
                        GROUP BY A.NAME, A.LEVELNO
                ) AS s
              
              IF @orderno is null
                 SELECT 'Page not found' as Result
                ELSE
                  BEGIN
                      -- date format 'dd/mm/yyyy hh24:mi:ss'
                    SET @sysdate = convert(varchar, getdate(), 103) +' '+ convert(VARCHAR(10), GETDATE(), 108) 
                                     
                    UPDATE axpages SET ordno = ordno + 1 WHERE ordno >= @orderno;
                    
                    INSERT INTO axpages (name, caption, blobno, visible, TYPE, parent, ordno, levelno, pagetype, props, createdon, updatedon, importedon) 
                     VALUES (@pname, @pcaption, 1, 'T', 'p', @pparentname, @orderno, @level, @ppagetype, @props, @sysdate, @sysdate, @sysdate);
                        
                      SELECT 'Page created' as Result
                  END        
            END
        ELSE
            BEGIN
                SELECT @orderno= ordno FROM axpages WHERE name=@pname AND TYPE='p';
                
                IF @orderno is null
                 SELECT 'Page not found' as Result
                ELSE
                 BEGIN
                    DELETE FROM axpages WHERE name = @pname;
                
                    UPDATE axpages SET ordno = ordno - 1 WHERE ordno >= @orderno;
                
                    SELECT 'Page deleted' as Result
                 END
            END  
END



CREATE PROCEDURE PR_BULK_PAGE_DELETE
AS
BEGIN
    DECLARE @I$NAME varchar(max)
    DECLARE DB_IMPLICIT_CURSOR_FOR_I CURSOR LOCAL FORWARD_ONLY FOR
    
    SELECT AXPAGES.NAME
        FROM AXPAGES
        WHERE AXPAGES.PAGETYPE = 'web' AND AXPAGES.BLOBNO = 1

    OPEN DB_IMPLICIT_CURSOR_FOR_I
    WHILE 1 = 1
        BEGIN
            FETCH DB_IMPLICIT_CURSOR_FOR_I
            INTO @I$NAME

            IF @@FETCH_STATUS = -1
            BREAK

            BEGIN
                BEGIN TRY
                    EXECUTE AXP_PR_PAGE_CREATION
                        @PNAME = @I$NAME,
                        @PCAPTION = NULL,
                        @PPAGETYPE = NULL,
                        @PPARENTNAME = NULL,
                        @PACTION = 'delete',
                        @PROPS = NULL
                END TRY
                BEGIN CATCH
                    BEGIN
                        DECLARE
                        @db_null_statement int
                    END
                END CATCH
            END
        END
    CLOSE DB_IMPLICIT_CURSOR_FOR_I
    DEALLOCATE DB_IMPLICIT_CURSOR_FOR_I
END

exec PR_BULK_PAGE_DELETE;

delete from axpages where pagetype='web';




--AxpertWebDev13-10.3.0.0
ALTER TABLE axusers DROP COLUMN homepage;
ALTER table axusers add homepage varchar(255) DEFAULT null;
ALTER TABLE axusergroups DROP COLUMN homepage;
ALTER table axusergroups add homepage varchar(255) DEFAULT null;
ALTER table AX_PAGE_SAVED add IS_DEFAULT VARCHAR(1) DEFAULT 'N';
ALTER table AX_PAGES add IS_DEFAULT VARCHAR(1) DEFAULT 'N';

--AxpertWebDev14-10.3.0.0
INSERT INTO ax_page_saved (title,TEMPLATE,module,page_menu,IS_DEFAULT) values('Homepage',1,'PAGE','Head19','Y');

INSERT INTO ax_widget_saved (title,widget_type,content,target,order_by) select title,widget_type,content,target,order_by from ax_homebuild_master;

update ax_widget_saved set created_by ='admin', page_id=1;