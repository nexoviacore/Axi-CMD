using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DBquery
/// </summary>
public class DBquery
{
    private string QueryString = string.Empty;
    private string db_type = string.Empty;
    private string db_owner = string.Empty;
    public DBquery()
    {
        db_type = HttpContext.Current.Session["axdb"].ToString();
        if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
        {
            string dbUserName = HttpContext.Current.Session["dbuser"].ToString();
            db_owner = dbUserName.Split('~')[0];
        }
        else
            db_owner = HttpContext.Current.Session["dbuser"].ToString();
    }

    public string getSQLqueryString(string objName, string QueryName, string objtype = "")
    {
        //DBQueries test = new DBQueries();
        //string query = String.Format(test.Table.Oracle, char.ToUpper(objtype[0]) + objtype.Substring(1));
        try
        {
            if (db_type.ToLower() == "oracle")
            {
                //User_objects stores all objects belongs to current user, no need to filter by user/owner.
                if (QueryName == "getObjList")
                    return "select object_name from user_objects where object_type = '" + char.ToUpper(objtype[0]) + objtype.Substring(1) + "' ORDER BY object_name";
                else if (QueryName == "getSource")
                    return "select dbms_metadata.get_ddl('" + objtype + "','" + objName + "','" + db_owner.ToUpper() + "') from dual";
                else if (QueryName == "getStruct")
                {
                    if (objtype == "INDEX")
                        return @"SELECT column_name,Table_name,Column_position,Column_length FROM all_ind_columns WHERE index_name = '" + objName + "'";
                    else if (objtype == "TABLE" || objtype == "VIEW")
                        return "select COLUMN_NAME,DATA_TYPE,DATA_DEFAULT,NULLABLE from user_tab_columns where table_name = '" + objName + "'";
                }
                else if (QueryName == "getData")
                    return "select * from " + objName;
                else if (QueryName == "getIndex")
                    return "select * from user_indexes where table_name='" + objName + "'";
                else if (QueryName == "getTrig")
                    return "select * from user_triggers where table_name='" + objName + "'";
                else if (QueryName == "getError")
                    return "select LINE,POSITION,TEXT AS ERROR from USER_ERRORS where type='" + objtype + "' and NAME='" + objName + "'";
                else if (QueryName == "getArgs")
                    return "select * from DBA_ARGUMENTS where object_name='" + objName + "' and owner='" + db_owner.ToUpper() + "'";
                if (QueryName == "getSeqInfo")
                    return @"SELECT * FROM user_sequences WHERE SEQUENCE_NAME = '" + objName + "'";
                else if (QueryName == "getTblClmnHint")
                    return "select table_name AS \"table_name\",column_name AS \"column_name\" from user_tab_columns order by table_name";
                else if (QueryName == "allObjData")
                    return "select * from user_objects where object_type = '" + char.ToUpper(objtype[0]) + objtype.Substring(1) + "' ORDER BY object_name";

            }
            else if (db_type.ToLower() == "ms sql")
            {
                switch (objtype)
                {
                    case "TABLE":
                        objtype = "U";
                        break;
                    case "VIEW":
                        objtype = "V";
                        break;
                    case "FUNCTION":
                        objtype = "TF";
                        break;
                    case "PROCEDURE":
                        objtype = "P";
                        break;
                    case "TRIGGER":
                        objtype = "TR";
                        break;
                }
                if (QueryName == "getObjList")
                {
                    if (objtype == "INDEX")
                    {
                        return @"SELECT I.NAME 
                                  FROM SYS.TABLES T
                                       INNER JOIN SYS.SCHEMAS S
                                    ON T.SCHEMA_ID = S.SCHEMA_ID
                                       INNER JOIN SYS.INDEXES I
                                    ON I.OBJECT_ID = T.OBJECT_ID
                                       INNER JOIN SYS.INDEX_COLUMNS IC
                                    ON IC.OBJECT_ID = T.OBJECT_ID
                                       INNER JOIN SYS.COLUMNS C
                                    ON C.OBJECT_ID  = T.OBJECT_ID
                                 WHERE  S.NAME ='" + db_owner + @"'
                                   AND IC.INDEX_ID    = I.INDEX_ID
                                   AND IC.COLUMN_ID = C.COLUMN_ID
                                 ORDER BY I.NAME";
                    }
                    if (objtype == "SEQUENCE")
                    {
                        return @"SELECT name from sys.sequences";
                    }

                    return @"SELECT name  FROM sys.objects WHERE type='" + objtype + "' AND schema_id = SCHEMA_ID('" + db_owner + "') ORDER BY name";

                }
                if (QueryName == "getSource")
                {
                    return @"SELECT ISNULL(smsp.definition, ssmsp.definition) AS [Definition] FROM
                            sys.all_objects AS sp
                            LEFT OUTER JOIN sys.sql_modules AS smsp ON smsp.object_id = sp.object_id
                            LEFT OUTER JOIN sys.system_sql_modules AS ssmsp ON ssmsp.object_id = sp.object_id
                            WHERE sp.type = '" + objtype + "' and sp.name ='" + objName + "' AND schema_id = SCHEMA_ID('" + db_owner + "')";
                }
                else if (QueryName == "getStruct")
                {
                    if (objtype == "INDEX")
                    {
                        return @"SELECT COL_NAME(ic.object_id,ic.column_id) AS column_name ,OBJECT_NAME('638625318') AS table_name 
                                      ,ic.index_column_id,ic.is_descending_key,ic.key_ordinal,ic.is_included_column  
                                FROM sys.indexes AS i  
                                INNER JOIN sys.index_columns AS ic
                                    ON i.object_id = ic.object_id AND i.index_id = ic.index_id  
                                WHERE i.name ='" + objName + "'";
                    }
                    return @"SELECT COLUMN_NAME,DATA_TYPE,COLUMN_DEFAULT,ORDINAL_POSITION,IS_NULLABLE,ORDINAL_POSITION FROM INFORMATION_SCHEMA. COLUMNS WHERE TABLE_NAME = '" + objName + "'";
                }
                else if (QueryName == "getData")
                    return "select * from " + objName;
                else if (QueryName == "getIndex")
                    return "SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('" + db_owner + "." + objName + "')";
                else if (QueryName == "getTrig")
                    return @"select t.* 
                             from sys.all_sql_modules m 
                             inner join  
                             sys.triggers t
                             on m.object_id = t.object_id 
                             inner join sys.objects o
                             on o.object_id = t.parent_id
                             Where o.name = '" + objName + "'";
                else if (QueryName == "getError")
                    return "";
                else if (QueryName == "getArgs")
                {
                    return @"SELECT P.name,P.parameter_id ,TYPE_NAME(P.user_type_id) ,
                                   P.max_length ,P.is_output 
                            FROM sys.objects AS SO
                            INNER JOIN sys.parameters AS P
                            ON SO.OBJECT_ID = P.OBJECT_ID
                            WHERE SO.OBJECT_ID IN ( SELECT OBJECT_ID
                            FROM sys.objects
                            WHERE TYPE = '" + objtype + @"')
                            ORDER BY P.name";
                }
                else if (QueryName == "getSeqInfo")
                {
                    return @"SELECT CAST(ISNULL(seq.start_value,N'''') AS varchar) AS [Start Value],
                            CAST(ISNULL(seq.increment,N'''') AS int) AS [Increment Value],
                            CAST(ISNULL(seq.minimum_value,N'''') AS int) AS [Min Value],
                            CAST(ISNULL(seq.maximum_value,N'''') AS int) AS [Max Value],
                            CAST(seq.scale AS int) AS [Numeric Scale],
                            CAST(seq.is_cycling AS bit) AS [Is Cycle Enabled],
                            ISNULL(seq.cache_size,0) AS [Cache Size]
                            FROM sys.sequences AS seq
                            WHERE seq.name ='" + objName + "'";
                }
                else if (QueryName == "getTblClmnHint")
                    return @"SELECT table_name,column_name FROM INFORMATION_SCHEMA. COLUMNS ";
                else if (QueryName == "allObjData")
                {
                    if (objtype == "INDEX")
                    {
                        return @"SELECT I.NAME,I.OBJECT_ID 
                                  FROM SYS.TABLES T
                                       INNER JOIN SYS.SCHEMAS S
                                    ON T.SCHEMA_ID = S.SCHEMA_ID
                                       INNER JOIN SYS.INDEXES I
                                    ON I.OBJECT_ID = T.OBJECT_ID
                                       INNER JOIN SYS.INDEX_COLUMNS IC
                                    ON IC.OBJECT_ID = T.OBJECT_ID
                                       INNER JOIN SYS.COLUMNS C
                                    ON C.OBJECT_ID  = T.OBJECT_ID
                                 WHERE  S.NAME ='" + db_owner + @"'
                                   AND IC.INDEX_ID    = I.INDEX_ID
                                   AND IC.COLUMN_ID = C.COLUMN_ID
                                 ORDER BY I.NAME";
                    }
                    if (objtype == "SEQUENCE")
                    {
                        return @"SELECT * from sys.sequences";
                    }

                    return @"SELECT *  FROM sys.objects WHERE type='" + objtype + "' AND schema_id = SCHEMA_ID('" + db_owner + "') ORDER BY name";

                }

            }
            else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb")
            {
                if (QueryName == "getObjList")
                {
                    if (objtype == "TABLE" || objtype == "VIEW")
                        return @"SELECT TABLE_NAME AS OBJECT_NAME FROM information_schema." + objtype + @"s WHERE TABLE_SCHEMA ='" + db_owner + "' ORDER BY OBJECT_NAME";
                    else if (objtype == "TRIGGER")
                        return @"SELECT trigger_name FROM information_schema.triggers WHERE trigger_schema ='" + db_owner + "' ORDER BY trigger_name ";
                    else if (objtype == "INDEX")
                        return @"SELECT DISTINCT INDEX_NAME FROM INFORMATION_SCHEMA.STATISTICS WHERE TABLE_SCHEMA ='" + db_owner + "' ORDER BY index_name";
                    else if (objtype == "FUNCTION" || objtype == "PROCEDURE")
                        return @"SELECT ROUTINE_NAME FROM information_schema.ROUTINES where ROUTINE_SCHEMA='" + db_owner + "' and ROUTINE_TYPE='" + objtype.ToUpper() + "' ORDER BY routine_name";
                }
                else if (QueryName == "getSource")
                {
                    if (objtype == "FUNCTION" || objtype == "PROCEDURE")
                        return "SELECT routine_definition FROM information_schema.ROUTINES where routine_name ='" + objName + "' AND ROUTINE_SCHEMA='" + db_owner + "' AND ROUTINE_TYPE='" + objtype.ToUpper() + "'";

                }
                else if (QueryName == "getStruct")
                    if (objtype == "INDEX")
                        return @"SELECT column_name,table_name,table_schema,Nullable,Seq_in_index FROM INFORMATION_SCHEMA.STATISTICS WHERE and index_name ='" + objName + "'";
                    else
                        return "SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT, COLUMN_KEY FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '" + objName + "' AND table_schema = '" + db_owner + "'";

                else if (QueryName == "getData")
                    return "select * from " + objName;
                else if (QueryName == "getIndex")
                    return "select * from information_schema.STATISTICS where table_name='" + objName + "'";
                else if (QueryName == "getTrig")
                    return "select * from information_schema.triggers where table_name='" + objName + "'";
                else if (QueryName == "getError")
                    return "";
                else if (QueryName == "getArgs")
                {
                    return @"select p.parameter_name,
                               p.data_type,
                               case when p.parameter_mode is null and p.data_type is not null
                                    then 'RETURN'
                                    else parameter_mode end as parameter_mode,
                               p.character_maximum_length as char_length,
                               p.numeric_precision,
                               p.numeric_scale
                        from information_schema.routines r
                        left join information_schema.parameters p
                                  on p.specific_schema = r.routine_schema
                                  and p.specific_name = r.specific_name
                        WHERE r.routine_name ='" + objName + @"' AND ROUTINE_SCHEMA='" + db_owner + "' AND ROUTINE_TYPE='" + objtype.ToUpper() + "'";
                }
                else if (QueryName == "getTblClmnHint")
                    return @"SELECT table_name, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS ";
                if (QueryName == "allObjData")
                {
                    if (objtype == "TABLE" || objtype == "VIEW")
                        return @"SELECT * FROM information_schema." + objtype + @"s WHERE TABLE_SCHEMA ='" + db_owner + "' ORDER BY OBJECT_NAME";
                    else if (objtype == "TRIGGER")
                        return @"SELECT * FROM information_schema.triggers WHERE trigger_schema ='" + db_owner + "' ORDER BY trigger_name ";
                    else if (objtype == "INDEX")
                        return @"SELECT * FROM INFORMATION_SCHEMA.STATISTICS WHERE TABLE_SCHEMA ='" + db_owner + "' ORDER BY index_name";
                    else if (objtype == "FUNCTION" || objtype == "PROCEDURE")
                        return @"SELECT * FROM information_schema.ROUTINES where ROUTINE_SCHEMA='" + db_owner + "' and ROUTINE_TYPE='" + objtype.ToUpper() + "' ORDER BY routine_name";
                }


            }
            else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
            {
                if (QueryName == "getObjList")
                {
                    if (objtype == "FUNCTION")
                    {
                        return @"SELECT distinct routine_name
                      FROM information_schema.routines
                     WHERE specific_schema ='" + db_owner + @"'
                       AND type_udt_name != 'trigger' order by routine_name";
                    }
                    else if (objtype == "PROCEDURE")
                    {
                        return @"select distinct p.proname   
                             from pg_proc p
                        left join pg_namespace n on p.pronamespace = n.oid
                        left join pg_language l on p.prolang = l.oid 
                          where n.nspname = '" + db_owner + @"'          
                         order by p.proname ";
                    }
                    else if (objtype == "TRIGGER")
                    {
                        return @"SELECT DISTINCT trigger_name
                          FROM information_schema.triggers
                         WHERE trigger_schema = '" + db_owner + @"' 
                            order by trigger_name";
                    }
                    //else
                    //{
                    //    return @" select cls.relname as ObjectName
                    //    from pg_class cls
                    //    join pg_roles rol 
                    //        on rol.oid = cls.relowner
                    //    join pg_namespace nsp 
                    //        on nsp.oid = cls.relnamespace
                    //    where nsp.nspname not in ('information_schema', 'pg_catalog')
                    //        and cls.relowner = '" + db_owner + @"'
                    //        and nsp.nspname not like 'pg_toast%' and cls.relkind =  case '" + objtype + @"'
                    //            when 'TABLE' then 'r'
                    //            when 'INDEX' then 'i'
                    //            when 'SEQUENCE' then 'S'
                    //            when 'VIEW' then 'v'
                    //        end
                    //    order by cls.relname";
                    //}
                    else if (objtype == "INDEX")
                    {
                        return "SELECT indexname FROM pg_indexes WHERE schemaname = '" + db_owner + "' ORDER BY indexname";
                    }
                    else if (objtype == "TABLE")
                    {
                        //   return "SELECT table_name FROM information_schema.tables WHERE table_schema = '" + db_owner + "' and table_type ='BASE TABLE' ORDER BY table_name";

                        return "SELECT tablename FROM pg_tables where schemaname ='" + db_owner + "' ORDER by tablename";
                    }
                    else if (objtype == "VIEW")
                    {
                        return "select viewname  from pg_catalog.pg_views where schemaname = '" + db_owner + "' ORDER BY viewname";
                    }
                    else if (objtype == "SEQUENCE")
                    {
                        return "SELECT sequence_name FROM information_schema.sequences where sequence_schema = '" + db_owner + "' ORDER BY sequence_name";
                    }
                }
                else if (QueryName == "getSource")
                {

                    if (objtype == "FUNCTION")
                    {
                        return @"SELECT
                            routine_definition 
                        FROM
                            information_schema.routines 
                        WHERE
                            specific_schema LIKE '" + db_owner + @"'
                            AND routine_name LIKE '" + objName + "'";
                    }
                    if (objtype == "PROCEDURE")
                    {
                        return @"select DISTINCT prosrc from pg_proc where proname = '" + objName + "'";
                    }
                    if (objtype == "INDEX")
                    {
                        return @"select indexdef from pg_indexes  where indexname ='" + objName + "' AND  schemaname = '" + db_owner + "'";
                    }
                    if (objtype == "VIEW")
                        return @"select definition from  pg_catalog.pg_views where viewname ='" + objName + "' AND  schemaname = '" + db_owner + "'";
                    if (objtype == "TRIGGER")
                        return @"SELECT action_statement
                          FROM information_schema.triggers
                         WHERE trigger_name ='" + objName + "' and  trigger_schema = '" + db_owner + @"'
                            order by trigger_name";
                }
                else if (QueryName == "getStruct")
                    if (objtype == "INDEX")
                        return @"select array_to_string(array_agg(a.attname), ', ') as column_names ,i.indisunique,i.indisclustered,i.indisvalid
                                from pg_namespace n 
                                join pg_class c on c.relnamespace = n.oid
                                join pg_index i on c.oid = i.indexrelid and c.relkind='i' and c.relname ='" + objName + "' and n.nspname ='" + db_owner + @"'
                                join pg_class t on t.oid = i.indrelid
                                left join pg_attribute a on a.attrelid = t.oid and a.attnum = ANY(i.indkey) 
                                group by t.relname,c.relname,i.indisunique,i.indisclustered,i.indisvalid";
                    else
                        return @"SELECT *
                                 FROM information_schema.columns
                                WHERE table_schema = '" + db_owner + @"'
                                 AND table_name   = '" + objName + "'";
                else if (QueryName == "getData")
                    return "select * from " + objName + " LIMIT 500";
                else if (QueryName == "getIndex")
                    return " select * from pg_indexes  where tablename='" + objName + "' AND  schemaname = '" + db_owner + "'";
                else if (QueryName == "getTrig")
                    return "select * from information_schema.triggers where event_object_table='" + objName + "'";
                else if (QueryName == "getError")
                    return "";
                else if (QueryName == "getArgs")
                    //return @"SELECT pg_proc.proargnames,
                    //        pg_proc.proargtypes,
                    //        pg_proc.proallargtypes,
                    //        pg_proc.proargmodes,
                    //        pg_language.lanname
                    //    FROM pg_catalog.pg_proc
                    //        JOIN pg_catalog.pg_namespace ON (pg_proc.pronamespace = pg_namespace.oid)
                    //        JOIN pg_catalog.pg_language ON (pg_proc.prolang = pg_language.oid) 
                    //      WHERE  pg_proc.proname ilike '" + objName + @"'
                    //        AND pg_namespace.nspname = '" + db_owner + "'";
                    return @"select args.parameter_name,
                               proc.external_language,
                               args.parameter_mode,
                               args.data_type
                        from information_schema.routines  proc
                        left join information_schema.parameters  args
                                  on proc.specific_schema = args.specific_schema
                                  and proc.specific_name = args.specific_name
                        where proc.specific_schema ='" + db_owner + @"'
                              and proc.routine_type = 'FUNCTION'
                              and  proc.routine_name = '" + objName + "'";
                else if (QueryName == "getSeqInfo")
                    return @"select  data_type, start_value, minimum_value, maximum_value, increment,cycle_option FROM information_schema.sequences where sequence_name ='" + objName + "' AND sequence_schema ='" + db_owner + "'";

                else if (QueryName == "getTblClmnHint")
                    return @"SELECT table_name,column_name
                     FROM information_schema.columns
                    WHERE table_schema = '" + db_owner + @"'
                    ORDER BY table_name";
                else if (QueryName == "allObjData")
                {
                    if (objtype == "FUNCTION")
                    {
                        return @"SELECT *
                      FROM information_schema.routines
                     WHERE specific_schema ='" + db_owner + @"'
                       AND type_udt_name != 'trigger' order by routine_name";
                    }
                    else if (objtype == "PROCEDURE")
                    {
                        return @"select *   
                             from pg_proc p
                        left join pg_namespace n on p.pronamespace = n.oid
                        left join pg_language l on p.prolang = l.oid 
                          where n.nspname = '" + db_owner + @"'          
                         order by p.proname ";
                    }
                    else if (objtype == "TRIGGER")
                    {
                        return @"SELECT *
                          FROM information_schema.triggers
                         WHERE trigger_schema = '" + db_owner + @"' 
                            order by trigger_name";
                    }
                    //else
                    //{
                    //    return @" select cls.relname as ObjectName
                    //    from pg_class cls
                    //    join pg_roles rol 
                    //        on rol.oid = cls.relowner
                    //    join pg_namespace nsp 
                    //        on nsp.oid = cls.relnamespace
                    //    where nsp.nspname not in ('information_schema', 'pg_catalog')
                    //        and cls.relowner = '" + db_owner + @"'
                    //        and nsp.nspname not like 'pg_toast%' and cls.relkind =  case '" + objtype + @"'
                    //            when 'TABLE' then 'r'
                    //            when 'INDEX' then 'i'
                    //            when 'SEQUENCE' then 'S'
                    //            when 'VIEW' then 'v'
                    //        end
                    //    order by cls.relname";
                    //}
                    else if (objtype == "INDEX")
                    {
                        return "SELECT * FROM pg_indexes WHERE schemaname = '" + db_owner + "' ORDER BY indexname";
                    }
                    else if (objtype == "TABLE")
                    {
                        return "SELECT * FROM information_schema.tables WHERE table_schema = '" + db_owner + "' ORDER BY table_name";
                    }
                    else if (objtype == "VIEW")
                    {
                        return "select *  from pg_catalog.pg_views where schemaname = '" + db_owner + "' ORDER BY viewname";
                    }
                    else if (objtype == "SEQUENCE")
                    {
                        return "SELECT * FROM information_schema.sequences where sequence_schema = '" + db_owner + "' ORDER BY sequence_name";
                    }
                }
            }

        }
        catch (Exception ex)
        {

        }
        return "";
    }


}

//[Serializable]
//public class DBQueries
//{
//    public DBTable Table { get; set; }
//    //public List<DBView> View { get; set};
//    //public List<DBFunction> Function { get; set};
//    //public List<DBProcedure> Procedure { get; set};
//    //public List<DBTrigger> Trigger { get; set};
//    //public List<DBIndex> Index { get; set};
//    //public List<DBSequence> Sequence { get; set};
//    // "Table", "View", "Function", "Procedure", "Trigger", "Index", "Sequence"
//}
//[Serializable]
//public class DBTable
//{
//    public string Oracle { get; set;}
//    DBTable()
//    {
//        Oracle = "select object_name from user_objects where object_type = '{0}' ORDER BY object_name";
//    }

//}
