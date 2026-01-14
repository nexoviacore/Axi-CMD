DROP TABLE IF EXISTS AX_HP_USER_LEVEL_WIDGET;
DROP TABLE IF EXISTS AX_PAGE_RESPONSIBILITY;
DROP TABLE IF EXISTS AX_WIDGET_PUBLISHED;
DROP TABLE IF EXISTS AX_PAGES;

DROP TABLE IF EXISTS AX_PAGE_SD_RESPONSIBILITY;
DROP TABLE IF EXISTS AX_WIDGET_SAVED;
DROP TABLE IF EXISTS AX_PAGE_SAVED;
DROP TABLE IF EXISTS AX_PAGE_TEMPLATES;

CREATE TABLE AX_PAGES(
    PAGE_ID INT(10) AUTO_INCREMENT PRIMARY KEY,
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
    CREATED_ON TIMESTAMP  DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP  DEFAULT CURRENT_TIMESTAMP,    
    IS_MIGRATED VARCHAR(1) DEFAULT 'N',
    ORDER_BY INT
);

CREATE TABLE AX_PAGE_SAVED(
    PAGE_ID INT(10) AUTO_INCREMENT PRIMARY KEY,
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
    CREATED_ON TIMESTAMP  DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP  DEFAULT CURRENT_TIMESTAMP,    
    IS_MIGRATED VARCHAR(1) DEFAULT 'N',
    IS_PUBLISH VARCHAR(1) DEFAULT 'N',
    IS_PRIVATE VARCHAR(1) DEFAULT 'N',
    PARENT_PAGE_ID INT(10),
    RESPONSIBILITY TEXT,
    ORDER_BY INT
);

CREATE TABLE AX_PAGE_RESPONSIBILITY(     
    PAGE_ID INT(10),
    FOREIGN KEY (PAGE_ID) REFERENCES AX_PAGES(PAGE_ID) ON DELETE CASCADE,
    RESPONSIBILITY VARCHAR(255),
    RESPONSIBILITY_ID INT
);

CREATE TABLE AX_PAGE_SD_RESPONSIBILITY(  
    PAGE_ID INT(10),
    FOREIGN KEY (PAGE_ID) REFERENCES AX_PAGE_SAVED(PAGE_ID) ON DELETE CASCADE,
    RESPONSIBILITY VARCHAR(255),
    RESPONSIBILITY_ID INT
);


CREATE TABLE AX_WIDGET_SAVED (
    WIDGET_ID INT(10) AUTO_INCREMENT PRIMARY KEY,
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
    PAGE_ID INT,
    CREATED_ON TIMESTAMP  DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP  DEFAULT CURRENT_TIMESTAMP, 
    IS_MIGRATED VARCHAR(1) DEFAULT 'N',
    FOREIGN KEY (PAGE_ID) REFERENCES AX_PAGE_SAVED(PAGE_ID) ON DELETE CASCADE
);

CREATE TABLE AX_WIDGET_PUBLISHED (
    WIDGET_ID INT(10) AUTO_INCREMENT PRIMARY KEY,
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
    CREATED_ON TIMESTAMP  DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP  DEFAULT CURRENT_TIMESTAMP, 
    IS_MIGRATED VARCHAR(1) DEFAULT 'N',
    FOREIGN KEY (PARENT_WIDGET_ID) REFERENCES AX_WIDGET_SAVED(WIDGET_ID) ON DELETE CASCADE,
    FOREIGN KEY (PAGE_ID) REFERENCES AX_PAGES(PAGE_ID) ON DELETE CASCADE
);

CREATE TABLE AX_HP_USER_LEVEL_WIDGET(
    PAGE_ID INT(10),
    FOREIGN KEY (PAGE_ID) REFERENCES AX_PAGES(PAGE_ID) ON DELETE CASCADE,
    WIDGETS  TEXT,
    USERNAME VARCHAR(255),
    IS_DELETED VARCHAR(1) DEFAULT 'N',
    CREATED_ON TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE AX_PAGE_TEMPLATES(
    TEMPLATE_ID INT(10) AUTO_INCREMENT PRIMARY KEY,
    TITLE VARCHAR(255),
    MODULE VARCHAR(255),
    CONTENT TEXT,
    CREATED_BY VARCHAR(255),
    UPDATED_BY VARCHAR(255),
    IS_DELETED VARCHAR(1) DEFAULT 'N',
    CREATED_ON TIMESTAMP  DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('basic','{"cc":1,"img":"basic.png","name":"basic","cf":[{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('modern','{"cc":1,"img":"modern.png","name":"modern","cf":[{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","br":"14px","tr":{"p":"top","h":"70px","html":"<span style=\\"float:left;font-size:45px;\\">#TITLE_ICON#</span><span style=\\"text-align: right;padding-top: 14px;position: relative;top: 16px;right: 5px;\\">#TITLE_NAME#</span>"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('mainCard','{"cc":7,"img":"mainCard.png","name":"mainCard","cf":[{"ht":"225px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('topaz','{"cc":5,"img":"topaz.png","name":"topaz","cf":[{"ht":"225px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m8 l8","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m8 l8","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('list','{"cc":1,"img":"list.png","name":"list","cf":[{"ht":"225px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('flow','{"cc":3,"img":"flow.png","name":"flow","repeatLastWidget":true,"cf":[{"ht":"500px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"500px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('checkered','{"cc":1,"img":"checkered.png","name":"checkered","cf":[{"ht":"300px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('flow main','{"cc":3,"img":"flow_main.png","name":"flow main","repeatLastWidget":true,"cf":[{"ht":"300px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"300px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('random','{"cc":7,"img":"random.png","name":"random","cf":[{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"300px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"300px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"200px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');

/*=============================================  
Database    : MY SQL
Name        : axp_pr_page_creation
Author      : Abhishek 
Create date : 27-09-2018  
Description : To create or delete a page  

Example     :
page creation:
    CALL axp_pr_page_creation('PageTest7','Test13','w3test1','Head16','before', 'page.aspx?transid=page')
    Result      :
      success - 'Page created'
          failure - 'Page not found'

 page deletion:
    CALL axp_pr_page_creation('PageTest7', null, null, null,'delete', null)
    Result      :
      success - 'Page deleted'
      failure - 'Page not found'
===============================================  */
DROP PROCEDURE IF EXISTS axp_pr_page_creation;
Delimiter //

 CREATE PROCEDURE axp_pr_page_creation(
        p_pname VARCHAR(100),       -- New Page Name/Delete Page Name
        p_pcaption VARCHAR(100),        -- New Page Caption
        p_ppagetype VARCHAR(100),   -- New Page Type
        p_pparentname VARCHAR(100),     -- Parent Name
        p_paction VARCHAR(100),         -- Before/After inserting or delete
        p_props varchar(100)        -- props
          ) 
BEGIN
     DECLARE v_orderno int; 
     DECLARE v_level   int;
     
     IF p_paction <> 'delete' THEN
            SELECT parent,  ordno, levelno INTO p_pparentname, v_orderno, v_level FROM 
            (
             SELECT 
                     parent, 
                      CASE 
                             WHEN LOWER (p_paction) = 'before' THEN ordno 
                             ELSE ordno + 1 
                     END ordno,
                       levelno
             FROM axpages
             WHERE name = p_pparentname AND TYPE = 'p'
             
             UNION ALL
             
             SELECT A.NAME, 
                    MAX(B.ORDNO) + 1 AS ORDNO, 
                    A.LEVELNO + 1 AS LEVELNO
             FROM AXPAGES AS A
                     LEFT OUTER JOIN AXPAGES AS B ON A.NAME = B.PARENT
                     WHERE A.NAME = p_pparentname AND A.TYPE = 'h'
                     GROUP BY A.NAME, A.LEVELNO
            ) AS s;
          
          IF v_orderno is null THEN
             SELECT 'Page not found' as Result;         
          ELSE
                  -- date format 'dd/mm/yyyy hh24:mi:ss'
                                 
                UPDATE axpages SET ordno = ordno + 1 WHERE ordno >= v_orderno;
                
                INSERT INTO axpages (name, caption, blobno, visible, TYPE, parent, ordno, levelno, pagetype, props, createdon, updatedon, importedon) 
                 VALUES (p_pname, p_pcaption, 1, 'T', 'p', p_pparentname, v_orderno, v_level, p_ppagetype, p_props, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
                    
                  SELECT 'Page created' as Result;
            END IF;
                    
     ELSE
            SELECT  ordno INTO v_orderno FROM axpages WHERE name=p_pname AND TYPE='p';
            
            IF v_orderno is null THEN
             SELECT 'Page not found' as Result;
             -- END IF;
            ELSE
                DELETE FROM axpages WHERE name = p_pname;
            
                UPDATE axpages SET ordno = ordno - 1 WHERE ordno >= v_orderno;
            
                SELECT 'Page deleted' as Result;
             END IF;
        END IF;  
END;
//

DELIMITER ;

DROP PROCEDURE IF EXISTS Get_AxPages_Hierarchy;
Delimiter //

 CREATE PROCEDURE Get_AxPages_Hierarchy(
       language VARCHAR(50),responsibility text
        ) 
BEGIN
  declare v_Done tinyint unsigned default 0;
  declare v_Name text;
    declare v_Name2 varchar(100);
    declare v_PageType text;
    declare v_Visible text;
    declare v_Caption VARCHAR(200);
  declare leaveLoop int default false;
    DROP TEMPORARY TABLE IF EXISTS tmpAxPages;    
  CREATE TEMPORARY TABLE tmpAxPages(
    NAME varchar(100) NOT NULL,
    CAPTION varchar(200) DEFAULT NULL,
    PAGETYPE varchar(200) DEFAULT NULL,
    VISIBLE varchar(8000) DEFAULT NULL
  ) ENGINE=MEMORY; 
    INSERT INTO tmpAxPages(NAME, CAPTION, PAGETYPE, VISIBLE) 
    SELECT axp.NAME, CASE WHEN IFNULL(axl.compcaption,'') <> '' THEN axl.compcaption ELSE axp.CAPTION END, 
    axp.PAGETYPE, axp.VISIBLE FROM axpages axp
    LEFT OUTER JOIN axlanguage axl on axp.NAME = axl.compname AND axl.lngname =language 
    WHERE (axp.PARENT IS NULL OR IFNULL(axp.PARENT,'') = '');
  BEGIN
    DECLARE v_getpages CURSOR FOR
    Select axp.NAME, CASE WHEN IFNULL(axl.compcaption,'') <> '' THEN axl.compcaption ELSE axp.CAPTION END, axp.PAGETYPE, axp.VISIBLE from axpages axp 
        LEFT OUTER JOIN axlanguage axl on axp.NAME = axl.compname AND axl.lngname =language 
        WHERE (axp.PARENT IS NOT NULL AND IFNULL(axp.PARENT,'') <> '');
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET leaveLoop = TRUE;
    OPEN v_getpages;
    pagesLoop: LOOP
      FETCH v_getpages INTO v_Name, v_Caption, v_PageType, v_Visible; 
      IF leaveLoop THEN
        LEAVE pagesLoop;
      END IF;
            SET v_Name2 = v_Name;
            SET v_Done = 0;
      WHILE NOT v_Done DO
        IF EXISTS(select 1 from axpages a 
              where a.name = v_Name and (a.PARENT IS NOT NULL AND IFNULL(a.PARENT,'') <> '')) then
          BEGIN
            SELECT parent FROM axpages where name = v_Name INTO @parentname;
                                  
            SELECT visible FROM axpages where name = @parentname INTO @visible;
                        
                        SET v_Name = @parentname;
                        
                        SET v_Visible = CONCAT(v_Visible, ','); 
            SET v_Visible = CONCAT(v_Visible, @visible);                        
          END;
        ELSE
          SET v_Done = 1;
        END IF;
      END WHILE;
      INSERT INTO tmpAxPages(NAME, CAPTION, PAGETYPE, VISIBLE) 
            values(v_Name2, v_Caption, v_PageType, v_Visible);
    END LOOP;
    CLOSE v_getpages;
  END;
    SELECT a.* FROM 
    (SELECT CASE WHEN SUBSTRING(tmp.PAGETYPE,1,1) = 'n' THEN 'i' ELSE SUBSTRING(tmp.PAGETYPE,1,1) END as STYPE, 
    SUBSTRING(tmp.PAGETYPE,2) STYPENAME, tmp.CAPTION CAPTION    
    FROM tmpAxPages tmp
    JOIN axpages axp ON axp.name = tmp.NAME
    WHERE IFNULL(tmp.PAGETYPE,'') <> '' AND NOT INSTR(UPPER(tmp.VISIBLE),'F') > 0
    AND (('default' in (responsibility)) or 
    (('default' not in (responsibility) 
    AND axp.parent IN (SELECT SNAME FROM AXUSERACCESS WHERE RNAME IN (responsibility) and SUBSTRING(tmp.PAGETYPE,1,1) = 'p') ))) and SUBSTRING(tmp.PAGETYPE,1,1) is not null and LOWER(SUBSTRING(tmp.PAGETYPE,1,1)) != 'w') a    
  ORDER BY a.STYPE,a.CAPTION;
    DROP TEMPORARY TABLE IF EXISTS tmpAxPages;  
END
//

DELIMITER ;


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