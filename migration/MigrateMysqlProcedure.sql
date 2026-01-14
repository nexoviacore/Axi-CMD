DROP PROCEDURE IF EXISTS Get_AxPages_Hierarchy;
DROP PROCEDURE IF EXISTS GetIview;
DROP PROCEDURE IF EXISTS pro_axplogstatextract;
DROP PROCEDURE IF EXISTS SP_GetChoices;
DROP PROCEDURE IF EXISTS sp_GetIviewStructure;
DROP PROCEDURE IF EXISTS SP_CreateLog;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `Get_AxPages_Hierarchy`( language VARCHAR(50),responsibility text)
BEGIN
  -- This procedure is used to get the AxPages Hierarchy
  declare v_Done tinyint unsigned default 0;
  declare v_Name text;
    declare v_Name2 varchar(100);
    declare v_PageType text;
    declare v_Visible text;
    declare v_Caption VARCHAR(200);
  declare leaveLoop int default false;
  
    -- Temp Table creation
    DROP TEMPORARY TABLE IF EXISTS tmpAxPages;    
  CREATE TEMPORARY TABLE `tmpAxPages`(
    `NAME` varchar(100) NOT NULL,
    `CAPTION` varchar(200) DEFAULT NULL,
    `PAGETYPE` varchar(200) DEFAULT NULL,
    `VISIBLE` varchar(8000) DEFAULT NULL
  ) ENGINE=MEMORY; 
    
    INSERT INTO tmpAxPages(NAME, CAPTION, PAGETYPE, VISIBLE) 
    SELECT axp.NAME, CASE WHEN IFNULL(axl.compcaption,'') <> '' THEN axl.compcaption ELSE axp.CAPTION END, 
    axp.PAGETYPE, axp.VISIBLE FROM axpages axp
    LEFT OUTER JOIN axlanguage axl on axp.NAME = axl.compname AND axl.lngname =language 
    WHERE (axp.PARENT IS NULL OR IFNULL(axp.PARENT,'') = '');

  BEGIN
    -- Cursor to fetch the data for looping
    DECLARE v_getpages CURSOR FOR
    Select axp.NAME, CASE WHEN IFNULL(axl.compcaption,'') <> '' THEN axl.compcaption ELSE axp.CAPTION END, axp.PAGETYPE, axp.VISIBLE from axpages axp 
        LEFT OUTER JOIN axlanguage axl on axp.NAME = axl.compname AND axl.lngname =language 
        WHERE (axp.PARENT IS NOT NULL AND IFNULL(axp.PARENT,'') <> '');
    
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET leaveLoop = TRUE;
        
    OPEN v_getpages;
    pagesLoop: LOOP
      -- Looping is done to fetch the page hierarchial order. Parent of current page is fetched and again looped back to fetch the parent.
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
            SELECT `parent` FROM axpages where name = v_Name INTO @parentname;
                                  
            SELECT `visible` FROM axpages where name = @parentname INTO @visible;
                        
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
    
    -- Filtering the records based on the condition.
    SELECT a.* FROM 
    (SELECT CASE WHEN SUBSTRING(tmp.PAGETYPE,1,1) = 'n' THEN 'i' ELSE SUBSTRING(tmp.PAGETYPE,1,1) END as STYPE, 
    SUBSTRING(tmp.PAGETYPE,2) STYPENAME, tmp.CAPTION CAPTION    
    FROM tmpAxPages tmp
    JOIN axpages axp ON axp.name = tmp.NAME
    WHERE IFNULL(tmp.PAGETYPE,'') <> '' AND NOT INSTR(UPPER(tmp.VISIBLE),'F') > 0
    AND (('default' in (responsibility)) or 
    (('default' not in (responsibility) 
    AND axp.parent IN (SELECT SNAME FROM AXUSERACCESS WHERE RNAME IN (responsibility) and SUBSTRING(tmp.PAGETYPE,1,1) = 'p') ))) and SUBSTRING(tmp.PAGETYPE,1,1) is not null) a    
  ORDER BY a.STYPE,a.CAPTION;
    
    -- Dropping the temporary table
    DROP TEMPORARY TABLE IF EXISTS tmpAxPages;  
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetIview`(ISql longtext,INoofRec int,IpageNo int, ICountFlag int)
Begin  
/* 
 To execute the iview query with param values
 Input will be Iview SQL, Pagination details like number of records per page and calling page number and count flag
 It will return 2 datasets (1. iview data, 2. total record count)
 if countflag=1 then it will return count of total record count
*/
 declare v_pos1 int;  declare v_pos2 int; declare v_pos3 int;    
 declare v_Qry longtext;    
 declare v_orderby varchar(1000); 
 set v_pos1 = 0;    
 set v_pos2 = 0;    
 set v_Qry = ISql;    
 set v_orderby = '';  

set @row_no:=0;
   -- Formatting the query for pagination data if INoofRec>0 AND IpageNo>0 else as it is
   -- By default we need to add empty columns axrowtype, rowno with given query
   if (INoofRec>0 and IpageNo>0)     
   then    
      set ISql=CONCAT('select * from (select a.*, (@row_no:=@row_no+1) as rowno, '''' as axrowtype from ( ',v_Qry, ') as a  ) xy  where rowno between ' , cast(((INoofRec * (IpageNo-1))+1) as char(50) ) , ' and ' , cast((INoofRec * (IpageNo)) as char(50) ) , ' order by rowno ');      
  else    
      set ISql=CONCAT('select a.*, (@row_no:=@row_no+1) as rowno, '''' as axrowtype from ( ',v_Qry, ') as a ');     
   end if; 
  
-- Executing the iview query here
   set @stmt_ISql =  ISql;
   prepare stmt from @stmt_ISql;
   execute stmt;
   deallocate prepare stmt;    
   
 -- Taking the iview record count if ICountFlag = 1 and IpageNo <= 1  
 if (ICountFlag = 1 and IpageNo <= 1)    
 then    
   set v_Qry = CONCAT('select  count(*) as IviewCount from  (', v_Qry ,')a');    
   -- Executing the query to get the iview record count
   set @stmt_str =  v_Qry;
   prepare stmt from @stmt_str;
   execute stmt;
   deallocate prepare stmt;    
 end if;   
End$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `pro_axplogstatextract`(fdate DATE)
BEGIN
   DELETE FROM usagedtl  WHERE date(executeddate)=fdate;

INSERT INTO usagedtl(executeddate,
                     code,
                     title,
                     cnt)
   SELECT date(calledon) cdate,
               'NOT' CODE,
               'Total No. of Transactions' title,
               COUNT(*) cnt
          FROM axpertlog
         WHERE date(calledon) = fdate
      GROUP BY date(calledon)
      UNION ALL
        SELECT date(calledon) cdate,
               'NOL' CODE,
               'Total No. of Logins' title,
               COUNT(*) cnt
          FROM axpertlog
         WHERE date(calledon) = fdate AND servicename = 'Login'
      GROUP BY date(calledon)
      UNION ALL
        SELECT date(calledon) cdate,
               'NOU' CODE,
               'Total No. of Users' title,
               COUNT(DISTINCT username) cnt
          FROM axpertlog
         WHERE date(calledon) = fdate AND servicename = 'Login'
      GROUP BY date(calledon)
      UNION ALL
        SELECT date(calledon) cdate,
               'NOD' CODE,
               'Total No. of Deadlock Execptions' title,
               COUNT(*) cnt
          FROM axpertlog
         WHERE date(calledon) = fdate
               AND serviceresult LIKE 'trans%dead%'
      GROUP BY date(calledon)
      UNION ALL
        SELECT date(calledon) cdate,
               'MTJ' CODE,
               'More time taken Saves (> 8 Sec)' title,
               COUNT(*) cnt
          FROM axpertlog
         WHERE     date(calledon) = fdate
               AND servicename = 'saving data'
               AND serviceresult = 'success'
               AND recordid = 0
               AND (timetaken / 1000) > 8
      GROUP BY date(calledon)
      UNION ALL
        SELECT date(calledon) cdate,
               'MTL' CODE,
               'More time taken Loads (> 8 Sec)' title,
               COUNT(*) cnt
          FROM axpertlog
         WHERE     date(calledon) = fdate
               AND servicename = 'load data'
               AND serviceresult = 'success'
               AND (timetaken / 1000) > 8
      GROUP BY date(calledon)
      UNION ALL
      SELECT date(calledon) cdate,
             'MTL' CODE,
             'More time taken reports (> 8 Sec)' title,
             COUNT(*) cnt
        FROM axpertlog
       WHERE     date(calledon) = fdate
             AND servicename = 'Get IView'
             AND serviceresult = 'success'
             AND (timetaken / 1000) > 8
GROUP BY date(calledon);


    DELETE FROM axpexception   WHERE date(exp_date)=fdate;

INSERT INTO axpexception(EXP_DATE,
                         STRUCTNAME,
                         SERVICENAME,
                         SERVICERESULT,
                         COUNT)
   SELECT date(calledon),
               structname,
               servicename,
               serviceresult,
               COUNT(*)
          FROM axpertlog
         WHERE SERVICERESULT <> 'success' AND date(calledon) = fdate
      GROUP BY date(calledon),
               structname,
               servicename,
               serviceresult;

  
    DELETE FROM ut_timetaken  WHERE DATE(executed_date)=fdate;

INSERT INTO ut_timetaken(executed_date,
                         object_type,
                         service_name,
                         object_name,
                         tot_count,
                         count_8s,
                         count_30s,
                         count_90s,
                         min_time,
                         max_time,
                         avg_time)
   SELECT curdate() exec_date,
               'tstruct' obj_type,
               'Saving Data' service_name,
               b.caption,
               COUNT(*) cnt,
               SUM(CASE WHEN timetaken > 8000 THEN 1 ELSE 0 END) cnt8,
               SUM(CASE WHEN timetaken > 30000 THEN 1 ELSE 0 END) cnt30,
               SUM(CASE WHEN timetaken > 90000 THEN 1 ELSE 0 END) cnt90,
               MIN(timetaken) / 1000 mintime,
               MAX(timetaken) / 1000 maxtime,
               AVG(timetaken) / 1000 avgtime
          FROM    axpertlog a
               JOIN
                  (  SELECT name, caption
                       FROM tstructs
                   GROUP BY name, caption) b
               ON a.structname = b.name
         WHERE     LOWER(servicename) = 'saving data'
               AND serviceresult = 'success'
               AND a.recordid = 0
               AND date(calledon) = fdate
      GROUP BY b.caption;

INSERT INTO ut_timetaken(executed_date,
                         object_type,
                         service_name,
                         object_name,
                         tot_count,
                         count_8s,
                         count_30s,
                         count_90s,
                         min_time,
                         max_time,
                         avg_time)
   SELECT curdate() exec_date,
               'tstruct' obj_type,
               'Load Data' service_name,
               b.caption,
               COUNT(*) cnt,
               SUM(CASE WHEN timetaken > 8000 THEN 1 ELSE 0 END) cnt8,
               SUM(CASE WHEN timetaken > 30000 THEN 1 ELSE 0 END) cnt30,
               SUM(CASE WHEN timetaken > 90000 THEN 1 ELSE 0 END) cnt90,
               MIN(timetaken) / 1000 mintime,
               MAX(timetaken) / 1000 maxtime,
               AVG(timetaken) / 1000 avgtime
          FROM    axpertlog a
               JOIN
                  (  SELECT name, caption
                       FROM tstructs
                   GROUP BY name, caption) b
               ON a.structname = b.name
         WHERE     LOWER(servicename) = 'load data'
               AND serviceresult = 'success'
               AND date(calledon) = fdate
      GROUP BY b.caption;

INSERT INTO ut_timetaken(executed_date,
                         object_type,
                         service_name,
                         object_name,
                         tot_count,
                         count_8s,
                         count_30s,
                         count_90s,
                         min_time,
                         max_time,
                         avg_time)
   SELECT curdate() exec_date,
               'tstruct' obj_type,
               'Load Report' service_name,
               b.caption,
               COUNT(*) cnt,
               SUM(CASE WHEN timetaken > 8000 THEN 1 ELSE 0 END) cnt8,
               SUM(CASE WHEN timetaken > 30000 THEN 1 ELSE 0 END) cnt30,
               SUM(CASE WHEN timetaken > 90000 THEN 1 ELSE 0 END) cnt90,
               MIN(timetaken) / 1000 mintime,
               MAX(timetaken) / 1000 maxtime,
               AVG(timetaken) / 1000 avgtime
          FROM    axpertlog a
               JOIN
                  (  SELECT name, caption
                       FROM iviews
                   GROUP BY name, caption) b
               ON a.structname = b.name
         WHERE     LOWER(servicename) = 'get iview'
               AND serviceresult = 'success'
               AND date(calledon) = fdate
      GROUP BY b.caption;
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GetChoices`(Isql nvarchar(5000), IsessionID varchar(30))
BEGIN
/*
To execute any query passed as an input param
This procedure also validates the session 
*/

-- These are the Exception handlers to Catch the Exception 
-- If any error or exception occures we send that exception or error as response in the form of XML string
-- e.g <error>Index was bounds of the array</error>

 -- Declare exception handler for failed query
  DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
      GET DIAGNOSTICS condition 1
      @SQLState = RETURNED_SQLSTATE, @SQLMessage = MESSAGE_TEXT; 
            SELECT CONCAT('<error>', @SQLMessage,'</error>') As errorString; 
        END;
 
 -- Declare exception handler for duplicate entry
  DECLARE EXIT HANDLER FOR 1062 
    BEGIN
      GET DIAGNOSTICS condition 1
      @SQLState = RETURNED_SQLSTATE, @SQLMessage = MESSAGE_TEXT;  
            SELECT CONCAT('<error>', @SQLMessage,'</error>') As errorString; 
        END;
 -- Declare exception handler for duplicate key
 -- It throws message like Message: Cannot add or update a child row: a foreign key constraint fails
  DECLARE EXIT HANDLER FOR SQLSTATE '23000'
      BEGIN
      GET DIAGNOSTICS condition 1
      @SQLState = RETURNED_SQLSTATE, @SQLMessage = MESSAGE_TEXT;  
            SELECT CONCAT('<error>', @SQLMessage,'</error>') As errorString; 
        END;
/*Actual Logic*/
 -- This block executes when the input query is of type SELECT
    IF ( lower(substring(ltrim(Isql),1,6))= 'select' ) then
        begin  
           set @stmt_ISql =  Isql;
           prepare stmt from @stmt_ISql;
           execute stmt;
           deallocate prepare stmt; 
        end;
-- This block executes when the input query is of not the type SELECT, like insert, update, delete statements
    else  
        begin  
           set @stmt_ISql =  Isql;
           prepare stmt from @stmt_ISql;
           execute stmt;
           deallocate prepare stmt; 
           select 'done' as result;  -- for non select queryies the response will be "done"
        end;
    end IF;
  
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetIviewStructure`(IN Iiviewname VARCHAR(10), IN ILanguage VARCHAR(50))
BEGIN
  -- This procedure is used to get the Iview Structure and Language based values for fields.
    -- Here, we will loop through the set of records to get the structure of a particular Iview.
    -- Since, the structure can be available in multiple rows for an Ivew, we have used cursor here.
    
  declare v_xml text; 
  declare v_props text; 
  declare leaveLoop int default false;  
    set v_props = '';
  set v_xml = '';
  
  BEGIN
    DECLARE v_getprops CURSOR FOR
    select props from iviews where name = Iiviewname order by blobno;
        
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET leaveLoop = TRUE;
    
    OPEN v_getprops;
    xmlLoop: LOOP
      FETCH v_getprops INTO v_props;
                 
      IF leaveLoop THEN
        LEAVE xmlLoop;
      END IF;
      
      set v_xml = CONCAT(v_xml, v_props); 
    END LOOP;
    CLOSE v_getprops;
  END;
  
    -- Returns Iview Structure
    select v_xml as Result;
  
    -- Returns Language based values for the Iview
    select b.compname, b.compcaption, b.comphint from axlanguage b
  where lower(b.sname)= 'i' + lower(Iiviewname) and lower(b.dispname) = lower(ILanguage);
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_CreateLog`(ISessionid varchar(4000),Iservicename varchar(4000),
                                        Icalledon datetime(3),Istructname VARCHAR(4000),
                                        Irecordid VARCHAR(4000),Iserviceresult VARCHAR(4000),
                                        Idbtimetaken varchar(4000))
BEGIN
/*
This procedure is being used to log the procedures steps, in which we are using the internally. Means we are calling this procedures in the main procedures.
We log the information like username servicename, stucture name, record id 
The log will be inserted into AXPERTLOG table
*/

    declare v_IUsername varchar(1000);
    declare v_Icallfinished datetime(3);
    declare v_Itimetaken varchar(1000);

-- Declare exception handler for failed query
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
  GET DIAGNOSTICS condition 1
    @SQLState = RETURNED_SQLSTATE, @SQLMessage = MESSAGE_TEXT;      
    ROLLBACK;
    -- SELECT CONCAT('<error>', @SQLMessage,'</error>') As errorString; 
END;
    set v_Itimetaken=v_Icallfinished - Icalledon;
      select username into v_IUsername from  connections where sessionid =ISessionid; 
        
    insert into  axpertlog(sessionid,username,calledon,callfinished,structname,recordid,servicename,
    serviceresult,timetaken,dbtimetaken) values 
      (ISessionid,v_IUsername,Icalledon,v_Icallfinished,Istructname,Irecordid,
      Iservicename,Iserviceresult,v_Itimetaken,Idbtimetaken);
END$$
DELIMITER ;
