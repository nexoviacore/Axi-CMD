
DROP TRIGGER AX_PAGES_bir;
DROP TRIGGER AX_PAGE_SAVED_bir;
DROP TRIGGER AX_PAGE_TEMPLATE_bir;

DROP TABLE AX_HP_USER_LEVEL_WIDGET cascade constraints;
DROP TABLE AX_PAGE_RESPONSIBILITY cascade constraints;
DROP TABLE AX_WIDGET_PUBLISHED cascade constraints;
DROP TABLE AX_PAGES cascade constraints;

DROP TABLE AX_PAGE_SD_RESPONSIBILITY cascade constraints;
DROP TABLE AX_WIDGET_SAVED cascade constraints;
DROP TABLE AX_PAGE_SAVED cascade constraints;

DROP SEQUENCE ax_pages_seq;
DROP SEQUENCE ax_page_saved_seq;

DROP SEQUENCE ax_widget_saved_seq;
DROP SEQUENCE ax_widget_publish_seq;


DROP TABLE AX_PAGE_TEMPLATES cascade constraints;
DROP SEQUENCE ax_page_template_seq;


CREATE TABLE AX_PAGES(
	PAGE_ID NUMBER(10,0) primary key,
	TITLE VARCHAR2(255),
    TYPE VARCHAR2(255),
    MODULE VARCHAR2(255),
	TEMPLATE VARCHAR2(255),
    PAGE_MENU VARCHAR2(255),
	CONTENT NCLOB,
	CREATED_BY VARCHAR2(255),
	UPDATED_BY VARCHAR2(255),
	IS_DELETED VARCHAR2(1) DEFAULT 'N',
    IS_PRIVATE VARCHAR2(1) DEFAULT 'N',
	CREATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP,
	UPDATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP,	
	IS_MIGRATED VARCHAR2(1) DEFAULT 'N',
  IS_DEFAULT VARCHAR2(1) DEFAULT 'N',
	ORDER_BY NUMBER
);

CREATE SEQUENCE ax_pages_seq START WITH 1;

CREATE OR REPLACE TRIGGER AX_PAGES_bir 
BEFORE INSERT ON AX_PAGES FOR EACH ROW 
BEGIN 
    :new.PAGE_ID := ax_pages_seq.nextval; 
END;
/


CREATE TABLE AX_PAGE_SAVED(
	PAGE_ID NUMBER(10,0) primary key,
	TITLE VARCHAR2(255),
    TYPE VARCHAR2(255),
    MODULE VARCHAR2(255),
	TEMPLATE VARCHAR2(255),
    PAGE_MENU VARCHAR2(255),
	CONTENT NCLOB,
	CREATED_BY VARCHAR2(255),
	UPDATED_BY VARCHAR2(255),
	IS_DELETED VARCHAR2(1) DEFAULT 'N',
	CREATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP,
	UPDATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP,	
	IS_MIGRATED VARCHAR2(1) DEFAULT 'N',
    IS_LOCK VARCHAR2(1) DEFAULT 'N',
    IS_PUBLISH VARCHAR2(1) DEFAULT 'N',
    IS_PRIVATE VARCHAR2(1) DEFAULT 'N',
    IS_DEFAULT VARCHAR2(1) DEFAULT 'N',
	PARENT_PAGE_ID NUMBER(10,0),
	RESPONSIBILITY NCLOB,
	ORDER_BY NUMBER
);

CREATE SEQUENCE ax_page_saved_seq START WITH 1;

CREATE OR REPLACE TRIGGER AX_PAGE_SAVED_bir 
BEFORE INSERT ON AX_PAGE_SAVED FOR EACH ROW 
BEGIN 
    :new.PAGE_ID := ax_page_saved_seq.nextval; 
END;
/

CREATE TABLE AX_PAGE_RESPONSIBILITY(	 
	PAGE_ID NUMBER REFERENCES AX_PAGES(PAGE_ID) ON DELETE CASCADE,
	RESPONSIBILITY VARCHAR2(255),
  	RESPONSIBILITY_ID NUMBER
);

CREATE TABLE AX_PAGE_SD_RESPONSIBILITY(	 
	PAGE_ID NUMBER REFERENCES AX_PAGE_SAVED(PAGE_ID) ON DELETE CASCADE,
	RESPONSIBILITY VARCHAR2(255),
  	RESPONSIBILITY_ID NUMBER
);


CREATE TABLE AX_WIDGET_SAVED (
    WIDGET_ID NUMBER(10,0) primary key,
    TITLE VARCHAR2(255),
    WIDGET_TYPE VARCHAR2(255),
    CONTENT NCLOB,
    TARGET VARCHAR2(255),
    IS_PRIVATE VARCHAR2(1) DEFAULT 'N',
    CREATED_BY VARCHAR2(255),
    UPDATED_BY VARCHAR2(255),
    IS_DELETED VARCHAR2(1) DEFAULT 'N',
    IS_LOCK VARCHAR2(1) DEFAULT 'N',
    ORDER_BY NUMBER,
    is_publish varchar2(1) DEFAULT 'N',
    PARENT_WIDGET_ID NUMBER,
    PAGE_ID REFERENCES AX_PAGE_SAVED(PAGE_ID) ON DELETE CASCADE,
    CREATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP, 
    IS_MIGRATED VARCHAR2(1) DEFAULT 'N'
);


CREATE SEQUENCE ax_widget_saved_seq START WITH 1;

CREATE OR REPLACE TRIGGER AX_WIDGET_SAVED_bir 
BEFORE INSERT ON AX_WIDGET_SAVED FOR EACH ROW 
BEGIN 
    :new.WIDGET_ID := ax_widget_saved_seq.nextval; 
END;
/

CREATE TABLE AX_WIDGET_PUBLISHED (
    WIDGET_ID NUMBER(10,0) primary key,
    TITLE VARCHAR2(255),
    WIDGET_TYPE VARCHAR2(255),
    CONTENT NCLOB,
    TARGET VARCHAR2(255),
    IS_PRIVATE VARCHAR2(1) DEFAULT 'N',
    CREATED_BY VARCHAR2(255),
    UPDATED_BY VARCHAR2(255),
    IS_DELETED VARCHAR2(1) DEFAULT 'N',
    IS_LOCK VARCHAR2(1) DEFAULT 'N',
    ORDER_BY NUMBER,
    is_publish varchar2(1) DEFAULT 'N',
    PARENT_WIDGET_ID REFERENCES AX_WIDGET_SAVED(WIDGET_ID) ON DELETE CASCADE,
    PAGE_ID REFERENCES AX_PAGES(PAGE_ID) ON DELETE CASCADE,
    CREATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP, 
    IS_MIGRATED VARCHAR2(1) DEFAULT 'N'
);

CREATE SEQUENCE ax_widget_publish_seq START WITH 1;

CREATE OR REPLACE TRIGGER AX_WIDGET_PUBLISH_bir 
BEFORE INSERT ON AX_WIDGET_PUBLISHED FOR EACH ROW 
BEGIN 
    :new.WIDGET_ID := ax_widget_publish_seq.nextval; 
END;
/

CREATE TABLE AX_HP_USER_LEVEL_WIDGET(
    PAGE_ID NUMBER REFERENCES AX_PAGES(PAGE_ID) ON DELETE CASCADE,    
    WIDGETS  NCLOB,
    USERNAME VARCHAR2(255),
    IS_DELETED VARCHAR(1) DEFAULT 'N',
    CREATED_ON TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE AX_PAGE_TEMPLATES(
    TEMPLATE_ID NUMBER(10,0) primary key,
    TITLE VARCHAR2(255),
	MODULE VARCHAR2(255),
    CONTENT NCLOB,
    CREATED_BY VARCHAR2(255),
    UPDATED_BY VARCHAR2(255),
    IS_DELETED VARCHAR2(1) DEFAULT 'N',
    CREATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP,
    UPDATED_ON TIMESTAMP (6) DEFAULT CURRENT_TIMESTAMP
);

CREATE SEQUENCE ax_page_template_seq START WITH 1;

CREATE OR REPLACE TRIGGER AX_PAGE_TEMPLATE_bir 
BEFORE INSERT ON AX_PAGE_TEMPLATES FOR EACH ROW 
BEGIN 
    :new.TEMPLATE_ID := ax_page_template_seq.nextval; 
END;
/

INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('basic','{"cc":1,"img":"basic.png","name":"basic","cf":[{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('modern','{"cc":1,"img":"modern.png","name":"modern","cf":[{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","br":"14px","tr":{"p":"top","h":"70px","html":"<span style=\"float:left;font-size:45px;\">#TITLE_ICON#</span><span style=\"text-align: right;padding-top: 14px;position: relative;top: 16px;right: 5px;\">#TITLE_NAME#</span>"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('mainCard','{"cc":7,"img":"mainCard.png","name":"mainCard","cf":[{"ht":"225px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('topaz','{"cc":5,"img":"topaz.png","name":"topaz","cf":[{"ht":"225px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m8 l8","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m8 l8","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m4 l4","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('list','{"cc":1,"img":"list.png","name":"list","cf":[{"ht":"225px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('flow','{"cc":3,"img":"flow.png","name":"flow","repeatLastWidget":true,"cf":[{"ht":"500px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"500px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('checkered','{"cc":1,"img":"checkered.png","name":"checkered","cf":[{"ht":"300px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('flow main','{"cc":3,"img":"flow_main.png","name":"flow main","repeatLastWidget":true,"cf":[{"ht":"300px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"300px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"225px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');
INSERT INTO AX_PAGE_TEMPLATES (TITLE,CONTENT,CREATED_BY) values ('random','{"cc":7,"img":"random.png","name":"random","cf":[{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"130px","isResp":true,"wd":"m3 l3","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"300px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"300px","isResp":true,"wd":"m6 l6","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}},{"ht":"200px","isResp":true,"wd":"m12 l12","ms":"classic","tr":{"p":"top","h":"35px","html":"#TITLE_ICON# #TITLE_NAME#"}}]}','admin');


commit
/

CREATE OR REPLACE VIEW axp_vw_menulist AS 
     SELECT menupath,
            name,
            ordno,
            levelno,
            parent,
            TYPE,
            pagetype
       FROM (    SELECT caption,
                        DECODE (LEVEL,
                                2, caption,
                                LPAD (' ', (LEVEL + levelno * 2) - 1) || caption)
                           AS menu_tree,
                        levelno,
                        TYPE,
                        pagetype,
                        SYS_CONNECT_BY_PATH (caption, '\') || '\' AS menupath,
                        ordno,
                        parent,
                        name
                   FROM axpages
                  WHERE blobno = 1
             START WITH parent IS NULL
             CONNECT BY PRIOR name = parent)
   ORDER BY ordno, menupath
   /





CREATE OR REPLACE PROCEDURE axp_pr_page_creation (pname          VARCHAR2, --New Page Name/Delete Page Name

                                                  pcaption       VARCHAR2, --New Page Caption

                                                  ppagetype      VARCHAR2, --New Page Type

                                                  pparentname    VARCHAR2, --Parent Name

                                                  paction        VARCHAR2, -- Before/After inserting or delete

                                                  props          VARCHAR2  -- Webpage info string

                                                                         )

AS

   pparent    VARCHAR2 (50);

   pordno     NUMBER (15);

   plevelno   NUMBER (15);

   psysdate   VARCHAR2 (30);

BEGIN

   IF LOWER (paction) <> 'delete'

   THEN

      --Page Creation before or after

      SELECT parent, ordno, levelno

        INTO pparent, pordno, plevelno

        FROM (SELECT parent,

                     CASE

                        WHEN LOWER (paction) = 'before' THEN ordno

                        ELSE ordno + 1

                     END

                        ordno,

                     levelno

                FROM axp_vw_menulist

               WHERE name = pparentname AND TYPE = 'p'

              UNION ALL

                SELECT a.name,

                       MAX (b.ordno) + 1 AS ordno,

                       a.levelno + 1 AS levelno

                  FROM axp_vw_menulist a, axp_vw_menulist b

                 WHERE     a.name = pparentname

                       AND b.menupath LIKE a.menupath || '%'

                       AND a.TYPE = 'h'

              GROUP BY a.name, a.levelno);

 

      psysdate := TRIM (TO_CHAR (SYSDATE, 'dd/mm/yyyy hh24:mi:ss'));

 

      UPDATE axpages

         SET ordno = ordno + 1

       WHERE ordno >= pordno;

 

      INSERT INTO axpages (name,

                           caption,

                           blobno,

                           visible,

                           TYPE,

                           parent,

                           props,

                           ordno,

                           levelno,

                           pagetype,

                           createdon,

                           updatedon,

                           importedon)

           VALUES (pname,

                   pcaption,

                   1,

                   'T',

                   'p',

                   pparent,

                   props,

                   pordno,

                   plevelno,

                   ppagetype,

                   psysdate,

                   psysdate,

                   psysdate);

 

      COMMIT;

   ELSE

      --Page Deleting

      SELECT ordno

        INTO pordno

        FROM axp_vw_menulist

       WHERE name = pname AND TYPE = 'p';

 

 

      DELETE FROM axpages

            WHERE name = pname;

 

      UPDATE axpages

         SET ordno = ordno - 1

       WHERE ordno >= pordno;

      

       commit;

      

   END IF;

END;

 /



create or replace procedure pr_bulk_page_delete as

begin

for i in (Select name from axpages where pagetype='web' and blobno=1) 

loop

begin

axp_pr_page_creation( i.name,null,null,null,'delete',null);

exception when others then

null;

end ;

end loop;

end;
/


-- Delete the All pages from Page Menu.

exec PR_BULK_PAGE_DELETE

delete from axpages where pagetype='web'






--AxpertWebDev13-10.3.0.0
ALTER TABLE axusers DROP COLUMN homepage
/
ALTER table axusers add homepage varchar2(255) DEFAULT null
/
ALTER TABLE axusergroups DROP COLUMN homepage
/
ALTER table axusergroups add homepage varchar2(255) DEFAULT null
/
ALTER table AX_PAGE_SAVED add IS_DEFAULT VARCHAR2(1) DEFAULT 'N'
/
ALTER table AX_PAGES add IS_DEFAULT VARCHAR2(1) DEFAULT 'N'
/

--AxpertWebDev14-10.3.0.0

INSERT INTO ax_page_saved (title,TEMPLATE,module,page_menu,IS_DEFAULT) values('Homepage',1,'PAGE','Head19','Y');

INSERT INTO ax_widget_saved (title,widget_type,content,target,order_by) select title,widget_type,content,target,order_by from ax_homebuild_master;

update ax_widget_saved set created_by ='admin', page_id=1;






commit;