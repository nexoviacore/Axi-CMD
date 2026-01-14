using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Oracle.DataAccess.Client;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;

/// <summary>
/// Summary description for Iviewdb
/// </summary>
public class DBContext
{
    private string db_type = String.Empty;
    private string _connectionString = String.Empty;
    LogFile.Log logobj = new LogFile.Log();
    FDR objfd;

    public DBContext()
    {

        db_type = HttpContext.Current.Session["axdb"].ToString();
        _connectionString = HttpContext.Current.Session["axconstr"].ToString();
    }



    public DataTable GetDataTableInline(string query)
    {
        DataTable dt = new DataTable();
        try
        {
            Ihelper _helper = new Helper().SetDatabase(db_type, _connectionString);
            dt = _helper.ExecuteDataSetSqlInline(query).Tables[0];
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    public DataSet GetMainPageDataDB(string query) // using for stored procedure call with multi select queries 
    {
        Ihelper _helper = new Helper().SetDatabase(db_type, _connectionString);
        DataSet ds = new DataSet();
        try
        {
            if (db_type.ToLower() == "oracle")
            {
                logobj.CreateLog("Dashboard ,oracledb-User:" + HttpContext.Current.Session["user"], HttpContext.Current.Session.SessionID, "dashBoard-db", "new");
                _helper.AddInParameter("@IUser", HttpContext.Current.Session["user"], DbType.String);
                _helper.AddOutParameter("ORes1", DbType.Object);
                _helper.AddOutParameter("ORes2", DbType.Object);
                _helper.AddOutParameter("ORes3", DbType.Object);
                _helper.AddOutParameter("ORes4", DbType.Object);
                _helper.AddOutParameter("ORes5", DbType.Object);
                _helper.AddOutParameter("ORes6", DbType.Object);
                ds = _helper.ExecuteDataSet("sp_dashboardmultisql");
            }
            else if (db_type.ToLower() == "ms sql")
            {
                logobj.CreateLog("Dashboard mssqldb-User:" + HttpContext.Current.Session["user"], HttpContext.Current.Session.SessionID, "dashBoard-db", "new");
                _helper.AddInParameter("@IUser", HttpContext.Current.Session["user"], DbType.String);
                ds = _helper.ExecuteDataSet("sp_dashboardmultisql");
            }
            else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb")
            {
                logobj.CreateLog("Dashboard mysqldb-User:" + HttpContext.Current.Session["user"], HttpContext.Current.Session.SessionID, "dashBoard-db", "new");
                _helper.AddInParameter("IUser", HttpContext.Current.Session["user"], DbType.String);
                ds = _helper.ExecuteDataSet("sp_dashboardmultisql");
            }

            else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
            {
                logobj.CreateLog("Dashboard Postgresdb-User:" + HttpContext.Current.Session["user"], HttpContext.Current.Session.SessionID, "dashBoard-db", "new");
                _helper.AddInParameter("IUser", HttpContext.Current.Session["user"], DbType.String);
                ds = _helper.ExecuteDataSet("sp_dashboardmultisql");


            }
        }


        catch (Exception ex)
        {
            throw ex;
        }
        return ds;
    }

    public DataSet GetRapidDefData(string procName, string transId) // using for stored procedure call with multi select queries 
    {
        Ihelper _helper = new Helper().SetDatabase(db_type, _connectionString);
        DataSet ds = new DataSet();
        try
        {
            if (db_type.ToLower() == "oracle")
            {
                logobj.CreateLog("GetRapidDef ,oracledb-Transid:" + transId, HttpContext.Current.Session.SessionID, "GetRapidDefSP-db", "new");
                _helper.AddInParameter("@ITid", transId, DbType.String);
                _helper.AddOutParameter("ORes1", DbType.Object);
                _helper.AddOutParameter("ORes2", DbType.Object);
                ds = _helper.ExecuteDataSet(procName);
            }
            else if (db_type.ToLower() == "ms sql")
            {
                _helper.AddInParameter("@ITid", transId, DbType.String);
                ds = _helper.ExecuteDataSet(procName);
            }
            else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb")
            {
                _helper.AddInParameter("ITid", transId, DbType.String);
                ds = _helper.ExecuteDataSet(procName);
            }
            else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
            {

                _helper.AddInParameter("ITid", transId, DbType.String);
                ds = _helper.ExecuteDataSet(procName);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ds;
    }
    public DataSet GetMainPageDBInline(string type) // for inline query call with multi select queries 
    {
        DataSet ds = new DataSet();
        try
        {
            if (type == "dashboard")
            {
                Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
                string quickl = "select * from axquicklinks where name='" + HttpContext.Current.Session["user"] + "' order by AXQUICKLINKSID asc";
                ds = _helper1.ExecuteDataSetSqlInline(quickl);

                string task = "select * from axtasks where sname='" + HttpContext.Current.Session["user"] + "'";
                ds.Tables.Add("Table1");
                ds.Tables["Table1"].Merge(_helper1.ExecuteDataSetSqlInline(task).Tables[0]);

                string mess = "select * from axmessages where mname='" + HttpContext.Current.Session["user"] + "'";
                ds.Tables.Add("Table2");
                ds.Tables["Table2"].Merge(_helper1.ExecuteDataSetSqlInline(mess).Tables[0]);

                string dbwdetails = "select axp_dbwdetailsid,username,title,widgettype,sqltext,tablehlinks from axp_dbwdetails where username='" + HttpContext.Current.Session["user"] + "'";
                ds.Tables.Add("Table3");
                ds.Tables["Table3"].Merge(_helper1.ExecuteDataSetSqlInline(dbwdetails).Tables[0]);

                string srparams = "select s.* from axp_searchparams s,axp_dbwdetails m where s.WIDGETNAME=m.title and m.username='" + HttpContext.Current.Session["user"] + "'";
                ds.Tables.Add("Table4");
                ds.Tables["Table4"].Merge(_helper1.ExecuteDataSetSqlInline(srparams).Tables[0]);
            }
            else
            {
                Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
                //string mess = "select * from axmessages where mname='" + HttpContext.Current.Session["user"] + "'";
                //ds = _helper1.ExecuteDataSetSqlInline(mess);

                //string dbwdetails = "select count(*) as rcount from axp_dbwdetails where uname='" + HttpContext.Current.Session["user"] + "'";
                //string dbwdetails = "SELECT count(*) as rcount from axugwidgets u join axp_chartconfig d on u.widgetname = d.axp_chartconfigid where u.axugwidgetsid in (SELECT u.axugwidgetsid from axusers a join axuserlevelgroups b on a.axusersid = b.axusersid join axusergroups c on b.usergroup = c.groupname join axugwidgets u on c.axusergroupsid = u.axusergroupsId where a.username = '" + HttpContext.Current.Session["user"] + "' group by u.axugwidgetsid)";
                string dbwdetails = "SELECT count(*) as rcount from AX_WIDGET ax join AX_WIDGET_ROLE ar on ax.WIDGET_ID = ar.WIDGET_ID WHERE ar.ROLENAME IN(SELECT distinct c.groupname from axusers a join axuserlevelgroups b on a.username = b.username join axusergroups c on b.usergroup = c.groupname where a.username = '" + HttpContext.Current.Session["user"] + "')";

                //ds.Tables.Add("Table1");
                try
                {
                    ds = _helper1.ExecuteDataSetSqlInline(dbwdetails);
                }
                catch (Exception ex)
                {
                    return ds;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ds;
    }

    public DataSet GetMainPageDBTableInline(string querys, string fltParams, string widgetId) // single select query call 
    {
        if (HttpContext.Current.Session["FDR"] != null)
            objfd = (FDR)HttpContext.Current.Session["FDR"];
        else
            objfd = new FDR();
        DataSet ds = new DataSet();
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            List<string> widgets = widgetId.Split(';').ToList();
            string uName = "username:" + HttpContext.Current.Session["user"];
            if (fltParams != "")
                fltParams = uName + "~" + fltParams;
            else
                fltParams = uName;
            for (int i = 0; i < widgets.Count; i++)
            {
                DataTable dt = new DataTable();
                dt = objfd.GetDashBoardData(widgets[i].ToString(), fltParams);
                if (dt == null || dt.Rows.Count == 0)
                {
                    string subQuery = querys.Split(';')[i];
                    subQuery = subQuery.Replace(":username", "'" + HttpContext.Current.Session["user"] + "'").ToString();
                    subQuery = subQuery.Replace(":USERNAME", "'" + HttpContext.Current.Session["user"] + "'").ToString();
                    if (!string.IsNullOrEmpty(fltParams))
                    {
                        try
                        {
                            for (int prCount = 0; prCount < fltParams.Split('~').Length; prCount++)
                            {
                                string paramValue = fltParams.Split('~')[prCount];
                                subQuery = subQuery.Replace(":" + paramValue.Split(':')[0].ToLower(), "'" + paramValue.Split(':')[1].ToString() + "'").ToString();
                                subQuery = subQuery.Replace(":" + paramValue.Split(':')[0].ToUpper(), "'" + paramValue.Split(':')[1].ToString() + "'").ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            logobj.CreateLog("filter params " + ex.Message, "dashBoard-filter", "filter-exception", "new");
                        }
                    }
                    subQuery = Regex.Replace(subQuery, @"(:)\w+", "''").ToString();
                    if (subQuery != "")
                    {
                        string query = subQuery.Replace("  ", " ");
                        ds.Tables.Add("Table" + i + "");
                        try
                        {
                            ds.Tables["Table" + i + ""].Merge(_helper1.ExecuteDataSetSqlInline(query).Tables[0]);
                        }
                        catch (Exception ex)
                        {
                            logobj.CreateLog("Dashboard-sql " + ex.Message, query, "sqlquery-exception", "new");
                        }
                    }
                }
                else
                {
                    ds.Tables.Add("Table" + i + "");
                    ds.Tables["Table" + i + ""].Merge(dt);
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Dashboard " + ex.Message, HttpContext.Current.Session.SessionID, "dashBoard-exception", "new");
        }
        return ds;
    }

    public DataSet GetSearchData(string querys, int limit) // single select query call 
    {
        DataSet ds = new DataSet();
        try
        {
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            querys = querys.Replace(":username", "'" + HttpContext.Current.Session["user"] + "'").ToString();
            querys = querys.Replace(":USERNAME", "'" + HttpContext.Current.Session["user"] + "'").ToString();

            if (db_type.ToLower() == "oracle")
            {
                querys = querys + "and rownum <= " + limit.ToString();
            }
            else if (db_type.ToLower() == "ms sql")
            {
                querys = querys.Replace("select", "select TOP " + limit.ToString());
            }
            else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb")
            {
                querys = querys.Replace(@"\", "\\\\");
                querys = querys + " limit " + limit.ToString();
            }
            else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
            {
                querys = querys.Replace(@"\", "\\\\");
                querys = querys + " limit " + limit.ToString();

            }
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            ds = _helper1.ExecuteDataSetSqlInline(querys);
            //if (HttpContext.Current.Session["AxDirectDBException"] != null)
            //{
            //    string exMSg = HttpContext.Current.Session["AxDirectDBException"].ToString();
            //    HttpContext.Current.Session.Remove("AxDirectDBException");
            //    //ds.Tables.Add(CreateExcTable(exMSg));
            //}
        }
        catch (Exception ex)
        {
            logobj.CreateLog("SearchData " + ex.Message, HttpContext.Current.Session.SessionID, "SearchData-exception", "new");
        }

        return ds;
    }

    private DataTable CreateExcTable(string excMessage)
    {
        DataTable dt = new DataTable();
        dt.TableName = "AxException";
        dt.Columns.Add("exp");
        DataRow dr = dt.NewRow();
        dr[0] = excMessage;
        dt.Rows.Add(dr);
        dt.AcceptChanges();
        return dt;
    }

    public bool ExecuteSqlQueryInline(string query) // insert, update & delete query call 
    {
        bool flagcount = false;
        try
        {
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            flagcount = _helper1.ExecuteNonQuerySqlinline(query) == 1 ? true : false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return flagcount;
    }

    private static void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
    }

    public DataSet InsertMainPageDBTableInline(string linkName, string linkUrl) // insert, update & delete query call 
    {
        DataSet ds = new DataSet();
        try
        {
            if (HttpContext.Current.Session["user"] != null)
            {
                Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
                var query = "";
                if (db_type.ToLower() == "oracle")
                    query = "insert into axquicklinks(axquicklinksid,name,linkname,linkurl) values(axquicklinks_seq.NEXTVAL,'" + HttpContext.Current.Session["user"] + "','" + linkName + "','" + linkUrl + "')";
                else
                    query = "insert into axquicklinks(name,linkname,linkurl) values('" + HttpContext.Current.Session["user"] + "','" + linkName + "','" + linkUrl + "')";

                if (_helper1.ExecuteNonQuerySqlinline(query) == 1)
                {
                    ds = _helper1.ExecuteDataSetSqlInline("select max(AXQUICKLINKSID) AS ID from axquicklinks");
                }
            }
            else
            {
                SessExpires();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ds;
    }

    public DataSet GetChoices(string sql, string session)
    {
        Ihelper _helper = new Helper().SetDatabase(db_type, _connectionString);
        DataSet Ds = new DataSet();
        try
        {
            if (db_type.ToLower() == "oracle")
            {

                _helper.AddInParameter("Isql", sql, DbType.String);
                _helper.AddInParameter("IsessionID", session, DbType.String);
                _helper.AddInOutParameter("p_return_cur", DbType.Object);
            }
            else
            {
                _helper.AddInParameter("@Isql", sql, DbType.String);
                _helper.AddInParameter("@IsessionID", session, DbType.String);
            }

            Ds = _helper.ExecuteDataSet("sp_getchoices");

            //if(db_type.ToLower() == "mysql")
            //{
            //    Ds = GetResultXML(Ds);                
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return Ds;
    }

    //private DataSet GetResultXML(DataSet Ds)
    //{
    //    if (Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
    //    {
    //        if (Ds.Tables[0].Rows[0][0].ToString() != "done" && !Ds.Tables[0].Rows[0][0].ToString().Trim().StartsWith("<error>"))
    //        {
    //            Ds.DataSetName = "response";
    //            Ds.Tables[0].TableName = "row";
    //            string xmlRes = Ds.GetXml();
    //            xmlRes = "<sqlresultset>" + xmlRes + "</sqlresultset>";
    //            DataTable table = Ds.Tables[0];
    //            if (Ds.Tables.CanRemove(table))
    //            {
    //                Ds.Tables.Remove(table);
    //            }
    //            // create new table
    //            DataTable newDT = new DataTable("row");
    //            newDT.Columns.Add("result", typeof(string));
    //            Ds.Tables.Add(newDT);
    //            DataRow row = newDT.NewRow();
    //            newDT.Rows.Add(row);
    //            Ds.Tables[0].Rows[0][0] = xmlRes;

    //        }
    //    }
    //    return Ds;
    //}



    public DataSet GetIviewStructure(string iviewname)
    {

        Ihelper _helper = new Helper().SetDatabase(db_type, _connectionString);
        DataSet ds = new DataSet();
        try
        {
            string lang = string.IsNullOrEmpty(HttpContext.Current.Session["language"].ToString()) ? string.Empty :
                           (HttpContext.Current.Session["language"].ToString());
            _helper.AddInParameter("@Iiviewname", iviewname, DbType.String);
            _helper.AddInParameter("@ILanguage", lang, DbType.String);


            if (db_type.ToLower() == "oracle")
                _helper.AddOutParameter("Ocur1", DbType.Object);

            ds = _helper.ExecuteDataSet("sp_GetIviewStructure");
        }
        catch (Exception ex)
        {
            logobj.CreateLog("call get Iview structure Direct -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "directDB", string.Empty);
            throw ex;
        }
        return ds;
    }


    public DataSet GetIviewDataDB(string query, string pageno, string pageSize, bool isGrandtol)
    {
        Ihelper _helper = new Helper().SetDatabase(db_type, _connectionString);
        DataSet ds = new DataSet();
        try
        {
            _helper.AddInParameter("@ISql", query, DbType.String);
            _helper.AddInParameter("@INoofRec", pageSize, DbType.String);
            _helper.AddInParameter("@IpageNo", pageno, DbType.String);

            //New parameters added
            isGrandtol = true;
            _helper.AddInParameter("@ICountFlag", isGrandtol == false ? 0 : 1, DbType.String);

            if (db_type.ToLower() == "oracle")
            {
                _helper.AddOutParameter("OIviewCount", DbType.Object);
                _helper.AddOutParameter("OResult", DbType.Object);
            }
            ds = _helper.ExecuteDataSet("GetIview");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ds;
    }


    public DataSet GetIviewDataDBInline(string query, string pageno, string pageSize, bool isGrandtol)
    {
        DataSet ds = new DataSet();
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            string pagination = "select a.* from (select rownum as rowno, '' as axrowtype, a.* from ( " + query + ")a )a ";
            pagination += " where rowno between " + ((int.Parse(pageno) - 1) * (int.Parse(pageSize)) + 1) + " and " + (int.Parse(pageno) * int.Parse(pageSize));

            ds = _helper1.ExecuteDataSetSqlInline(pagination);
            if (isGrandtol == true)
            {
                string count = "select count(*) as IVIEWCOUNT from (" + query + ")a ";
                ds.Tables.Add("Table1");
                ds.Tables["Table1"].Merge(_helper1.ExecuteDataSetSqlInline(count).Tables[0]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ds;
    }
    //Added on 15-07-2016 for direct db call to get pdflist values for tstruct

    public string GetPDFList(string trasnsid, string sessionid)
    {
        Ihelper _helper = new Helper().SetDatabase(db_type, _connectionString);
        DataSet ds = new DataSet();
        try
        {
            _helper.AddInParameter("@p_Itid", trasnsid, DbType.String);
            _helper.AddInParameter("@IsessionID", sessionid, DbType.String);
            if (db_type.ToLower() == "oracle")
                _helper.AddOutParameter("p_return_cur", DbType.Object);
            ds = _helper.ExecuteDataSet("sp_GetPDFList");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            return ds.Tables[0].Rows[0][0].ToString();
        else
            return string.Empty;
    }

    public DataSet GetAxConfigurations(string transid, string structType, bool tstDesing, string configType)
    {
        DataSet ds = new DataSet();
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            string sqlTstDesign = string.Empty;
            string SqlQuery = string.Empty;
            string username = string.Empty;
            if (HttpContext.Current.Session["user"] != null)
                username = HttpContext.Current.Session["user"].ToString();
            if (configType == "" || configType == "configs")
            {
                if (db_type.ToLower() == "oracle")
                    SqlQuery = Constants.SQL_GET_AXPCONFIGS;
                else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb")
                    SqlQuery = Constants.SQL_GET_AXPCONFIGS_MYSQL;
                else if (db_type.ToLower() == "ms sql")
                    SqlQuery = Constants.SQL_GET_AXPCONFIGS_MSSQL;
                else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
                    SqlQuery = Constants.SQL_GET_AXPCONFIGS_POSTGRESQL;

                SqlQuery = SqlQuery.Replace("$username$", username);
                SqlQuery = SqlQuery.Replace("$stype$", structType);
                SqlQuery = SqlQuery.Replace("$transid$", transid);
            }
            if (structType == "Tstruct" && (configType == "design" || configType == ""))
            {
                sqlTstDesign = Constants.SQL_GET_TSTRUCTDESIGN;
                if (tstDesing)
                    sqlTstDesign = sqlTstDesign.Replace("$tblname$", "AX_LAYOUTDESIGN_SAVED");
                else
                    sqlTstDesign = sqlTstDesign.Replace("$tblname$", "AX_LAYOUTDESIGN");
                sqlTstDesign = sqlTstDesign.Replace("$transid$", transid);
                sqlTstDesign = sqlTstDesign.Replace("$mod$", "TSTRUCT");
                if (SqlQuery != "")
                    SqlQuery += ";" + sqlTstDesign;
                else
                    SqlQuery = sqlTstDesign;
            }

            try
            {
                ds = _helper1.ExecuteDataSetSqlConfig(SqlQuery);
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Get AxConfigurations DB Exception - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetAxConfigurations-db", string.Empty, "true");
                return ds;
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Get AxConfigurations - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetAxConfigurations", string.Empty, "true");
            throw ex;
        }
        return ds;
    }

    public string GetAxOldDesign(string transid)
    {
        string oldDesign = string.Empty;
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            string SqlQuery = Constants.SQL_GET_OLDTSTRUCTDESIGN;
            SqlQuery = SqlQuery.Replace("$transid$", transid);
            try
            {
                DataSet dsOldDesing = new DataSet();
                dsOldDesing = _helper1.ExecuteDataSetSqlConfig(SqlQuery);
                if (dsOldDesing.Tables[0].Rows.Count > 0)
                    oldDesign = dsOldDesing.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Get AxOldDesign DB Exception - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetAxOldDesign-db", string.Empty, "true");
                return oldDesign;
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Get AxOldDesign - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetAxOldDesign", string.Empty, "true");
            throw ex;
        }
        return oldDesign;
    }

    public string GetAxNewDesign(string transid)
    {
        string newDesign = string.Empty;
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            string SqlQuery = Constants.SQL_GET_TSTRUCTDESIGN;
            SqlQuery = SqlQuery.Replace("$tblname$", "AX_LAYOUTDESIGN");
            SqlQuery = SqlQuery.Replace("$transid$", transid);
            SqlQuery = SqlQuery.Replace("$mod$", "TSTRUCT");
            try
            {
                DataSet dsNewDesing = new DataSet();
                dsNewDesing = _helper1.ExecuteDataSetSqlConfig(SqlQuery);
                if (dsNewDesing.Tables[0].Rows.Count > 0)
                    newDesign = dsNewDesing.Tables[0].Rows[0]["CONTENT"].ToString();
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Get GetAxNewDesign DB Exception - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetAxNewDesign-db", string.Empty, "true");
                return newDesign;
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Get GetAxNewDesign - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetAxNewDesign", string.Empty, "true");
            throw ex;
        }
        return newDesign;
    }

    public string SaveDesignJson(string transid, string designContent)
    {
        string SaveId = string.Empty;
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            string SqlQuery = string.Empty;
            string username = string.Empty;
            if (HttpContext.Current.Session["user"] != null)
                username = HttpContext.Current.Session["user"].ToString();
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            SqlQuery = "select DESIGN_ID from AX_LAYOUTDESIGN_SAVED where transid='" + transid + "'";
            DataSet dsNewDesing = new DataSet();
            dsNewDesing = _helper1.ExecuteDataSetSqlInline(SqlQuery);
            if (dsNewDesing.Tables[0].Rows.Count > 0 && dsNewDesing.Tables[0].Rows[0]["DESIGN_ID"].ToString() != "")
            {
                Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
                if (db_type.ToLower() == "oracle")
                {
                    SqlQuery = "update AX_LAYOUTDESIGN_SAVED SET CONTENT=:CONTENT,MODULE='TSTRUCT',UPDATED_BY='" + username + "',is_deleted='N',is_publish='N',updated_on=CURRENT_TIMESTAMP where transid='" + transid + "'";
                    _helper2.AddInParameter(":CONTENT", designContent, DbType.String);
                }
                else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb" || db_type.ToLower() == "ms sql")
                {
                    SqlQuery = "update AX_LAYOUTDESIGN_SAVED SET CONTENT=@CONTENT,MODULE='TSTRUCT',UPDATED_BY='" + username + "',is_deleted='N',is_publish='N',updated_on=CURRENT_TIMESTAMP where transid='" + transid + "'";
                    _helper2.AddInParameter("@CONTENT", designContent, DbType.String);
                }
                else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
                {
                    SqlQuery = "update AX_LAYOUTDESIGN_SAVED SET CONTENT=:CONTENT,MODULE='TSTRUCT',UPDATED_BY='" + username + "',is_deleted='N',is_publish='N',updated_on=CURRENT_TIMESTAMP where transid='" + transid + "'";
                    _helper2.AddInParameter(":CONTENT", designContent, DbType.String);
                }

                if (_helper2.ExecuteNonQuerySqlinline(SqlQuery) == 1)
                    SaveId = dsNewDesing.Tables[0].Rows[0]["DESIGN_ID"].ToString();
            }
            else
            {
                Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
                if (db_type.ToLower() == "oracle")
                {
                    SqlQuery = "insert into AX_LAYOUTDESIGN_SAVED (TRANSID,CONTENT,MODULE,CREATED_BY) values ('" + transid + "',:CONTENT,'TSTRUCT','" + username + "')";
                    _helper2.AddInParameter(":CONTENT", designContent, DbType.String);
                }
                else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb" || db_type.ToLower() == "ms sql")
                {
                    SqlQuery = "insert into AX_LAYOUTDESIGN_SAVED (TRANSID,CONTENT,MODULE,CREATED_BY) values ('" + transid + "',@CONTENT,'TSTRUCT','" + username + "')";
                    _helper2.AddInParameter("@CONTENT", designContent, DbType.String);
                }
                else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
                {
                    SqlQuery = "insert into AX_LAYOUTDESIGN_SAVED (TRANSID,CONTENT,MODULE,CREATED_BY) values ('" + transid + "',:CONTENT,'TSTRUCT','" + username + "')";
                    _helper2.AddInParameter(":CONTENT", designContent, DbType.String);
                }
                if (_helper2.ExecuteNonQuerySqlinline(SqlQuery) == 1)
                {
                    SqlQuery = "select DESIGN_ID from AX_LAYOUTDESIGN_SAVED where transid='" + transid + "'";
                    Ihelper _helper3 = new Helper().SetDatabase(db_type, _connectionString);
                    dsNewDesing = _helper3.ExecuteDataSetSqlInline(SqlQuery);
                    if (dsNewDesing.Tables[0].Rows.Count > 0 && dsNewDesing.Tables[0].Rows[0]["DESIGN_ID"].ToString() != "")
                        SaveId = dsNewDesing.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Get SaveDesignJson - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "SaveDesignJson", string.Empty, "true");
        }
        return SaveId;
    }

    public string PublishDesignJson(string transid, string SavedId)
    {
        string PublishId = string.Empty;
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            string SqlQuery = string.Empty;
            string username = string.Empty;
            if (HttpContext.Current.Session["user"] != null)
                username = HttpContext.Current.Session["user"].ToString();
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            SqlQuery = "select DESIGN_ID,TRANSID,CONTENT,MODULE,CREATED_BY,RESPONSIBILITY,PARENT_DESIGN_ID from AX_LAYOUTDESIGN_SAVED where TRANSID='" + transid + "';select DESIGN_ID from AX_LAYOUTDESIGN where TRANSID='" + transid + "';";
            DataSet dsNewDesing = new DataSet();
            dsNewDesing = _helper1.ExecuteDataSetSqlConfig(SqlQuery);
            if (dsNewDesing.Tables.Count > 0)
            {
                string Content = string.Empty, Module = string.Empty, Created_by = string.Empty, Updated_by = string.Empty, P_Design_Id = string.Empty;
                if (dsNewDesing.Tables["Table0"].Rows.Count > 0)
                {
                    if (SavedId == "")
                        SavedId = dsNewDesing.Tables["Table0"].Rows[0]["DESIGN_ID"].ToString();
                    Content = dsNewDesing.Tables["Table0"].Rows[0]["CONTENT"].ToString();
                    Module = dsNewDesing.Tables["Table0"].Rows[0]["MODULE"].ToString();
                    Updated_by = Created_by = dsNewDesing.Tables["Table0"].Rows[0]["CREATED_BY"].ToString();
                    P_Design_Id = dsNewDesing.Tables["Table0"].Rows[0]["PARENT_DESIGN_ID"].ToString();

                    if (dsNewDesing.Tables["Table1"].Rows.Count > 0 && dsNewDesing.Tables["Table1"].Rows[0]["DESIGN_ID"].ToString() != "")
                    {
                        Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
                        if (db_type.ToLower() == "oracle")
                        {
                            SqlQuery = "update AX_LAYOUTDESIGN SET CONTENT=:CONTENT,MODULE='" + Module + "',UPDATED_BY='" + Updated_by + "',UPDATED_ON=CURRENT_TIMESTAMP where TRANSID='" + transid + "'";
                            _helper2.AddInParameter(":CONTENT", Content, DbType.String);
                        }
                        else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb" || db_type.ToLower() == "ms sql")
                        {
                            SqlQuery = "update AX_LAYOUTDESIGN SET CONTENT=@CONTENT,MODULE='" + Module + "',UPDATED_BY='" + Updated_by + "',UPDATED_ON=CURRENT_TIMESTAMP where TRANSID='" + transid + "'";
                            _helper2.AddInParameter("@CONTENT", Content, DbType.String);
                        }
                        else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
                        {
                            SqlQuery = "update AX_LAYOUTDESIGN SET CONTENT=:CONTENT,MODULE='" + Module + "',UPDATED_BY='" + Updated_by + "',UPDATED_ON=CURRENT_TIMESTAMP where TRANSID='" + transid + "'";
                            _helper2.AddInParameter(":CONTENT", Content, DbType.String);
                        }


                        if (_helper2.ExecuteNonQuerySqlinline(SqlQuery) == 1)
                        {
                            SqlQuery = "select DESIGN_ID from AX_LAYOUTDESIGN where TRANSID='" + transid + "'";
                            Ihelper _helper3 = new Helper().SetDatabase(db_type, _connectionString);
                            dsNewDesing = _helper3.ExecuteDataSetSqlInline(SqlQuery);
                            if (dsNewDesing.Tables[0].Rows.Count > 0 && dsNewDesing.Tables[0].Rows[0]["DESIGN_ID"].ToString() != "")
                                P_Design_Id = PublishId = dsNewDesing.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                        }
                    }
                    else
                    {
                        Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
                        if (db_type.ToLower() == "oracle")
                        {
                            SqlQuery = "insert into AX_LAYOUTDESIGN (TRANSID,CONTENT,MODULE,CREATED_BY) values ('" + transid + "',:CONTENT,'" + Module + "','" + Created_by + "')";
                            _helper2.AddInParameter(":CONTENT", Content, DbType.String);
                        }
                        else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb" || db_type.ToLower() == "ms sql")
                        {
                            SqlQuery = "insert into AX_LAYOUTDESIGN (TRANSID,CONTENT,MODULE,CREATED_BY) values ('" + transid + "',@CONTENT,'" + Module + "','" + Created_by + "')";
                            _helper2.AddInParameter("@CONTENT", Content, DbType.String);
                        }
                        else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
                        {
                            SqlQuery = "insert into AX_LAYOUTDESIGN (TRANSID,CONTENT,MODULE,CREATED_BY) values ('" + transid + "',:CONTENT,'" + Module + "','" + Created_by + "')";
                            _helper2.AddInParameter(":CONTENT", Content, DbType.String);
                        }


                        if (_helper2.ExecuteNonQuerySqlinline(SqlQuery) == 1)
                        {
                            SqlQuery = "select DESIGN_ID from AX_LAYOUTDESIGN where TRANSID='" + transid + "'";
                            Ihelper _helper3 = new Helper().SetDatabase(db_type, _connectionString);
                            dsNewDesing = _helper3.ExecuteDataSetSqlInline(SqlQuery);
                            if (dsNewDesing.Tables[0].Rows.Count > 0 && dsNewDesing.Tables[0].Rows[0]["DESIGN_ID"].ToString() != "")
                                P_Design_Id = PublishId = dsNewDesing.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                        }
                    }

                    if (PublishId != string.Empty)
                    {
                        Ihelper _helper4 = new Helper().SetDatabase(db_type, _connectionString);
                        SqlQuery = "update AX_LAYOUTDESIGN_SAVED SET PARENT_DESIGN_ID='" + P_Design_Id + "',IS_PUBLISH='Y' where DESIGN_ID='" + SavedId + "'";
                        if (_helper4.ExecuteNonQuerySqlinline(SqlQuery) == 1)
                            return PublishId;
                    }
                }
            }
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
            {
                PublishId = "error:" + HttpContext.Current.Session["AxDirectDBException"].ToString();
                HttpContext.Current.Session.Remove("AxDirectDBException");
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Get PublishDesignJson - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "PublishDesignJson", string.Empty, "true");
        }
        return PublishId;
    }

    public string ResetDesignJson(string transid, string SavedId)
    {
        string Reset = string.Empty;
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            string SqlQuery = string.Empty;
            string username = string.Empty;
            if (HttpContext.Current.Session["user"] != null)
                username = HttpContext.Current.Session["user"].ToString();
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            SqlQuery = "select DESIGN_ID from AX_LAYOUTDESIGN_SAVED where TRANSID='" + transid + "'";
            DataSet dsNewDesing = new DataSet();
            dsNewDesing = _helper1.ExecuteDataSetSqlInline(SqlQuery);
            if (dsNewDesing.Tables[0].Rows.Count > 0 && dsNewDesing.Tables[0].Rows[0]["DESIGN_ID"].ToString() != "")
            {
                SqlQuery = "DELETE from AX_LAYOUTDESIGN_SAVED where transid='" + transid + "'";
                _helper1.ExecuteNonQuerySqlinline(SqlQuery);
                Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
                SqlQuery = "DELETE from AX_LAYOUTDESIGN where transid='" + transid + "'";
                _helper2.ExecuteNonQuerySqlinline(SqlQuery);

                Ihelper _helper3 = new Helper().SetDatabase(db_type, _connectionString);
                SqlQuery = "delete from templates where lower(iviewid) = '" + transid + "'";
                _helper3.ExecuteNonQuerySqlinline(SqlQuery);

                Reset = "reseted";
            }
            else
            {
                try
                {
                    Ihelper _helper3 = new Helper().SetDatabase(db_type, _connectionString);
                    SqlQuery = "delete from templates where lower(iviewid) = '" + transid.ToLower() + "'";
                    _helper3.ExecuteNonQuerySqlinline(SqlQuery);
                }
                catch (Exception ex) { }
                Reset = "500";
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Get ResetDesignJson - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "ResetDesignJson", string.Empty, "true");
        }
        return Reset;
    }

    public string SavePublishDesign(string transid, string designContent, string SavedId)
    {
        string SaveId = string.Empty, PublishId = string.Empty;
        try
        {
            SaveId = SaveDesignJson(transid, designContent);
            PublishId = PublishDesignJson(transid, SavedId);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Get SavePublishDesign - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "SavePublishDesign", string.Empty, "true");
        }
        return SaveId + "~" + PublishId;
    }

    public void SaveExecutionTrace(string logtext, string prevSessId = "")
    {
        try
        {
            string SqlQuery = string.Empty;
            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            string sessId = HttpContext.Current.Session.SessionID;
            sessId = prevSessId != "" ? prevSessId : sessId;
            Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
            if (db_type.ToLower() == "oracle")
            {
                SqlQuery = "insert into axp_webtrace (USERNAME,SESSIONID,DATETIME,LOGTEXT) values ('" + userName + "','" + sessId + "',CURRENT_TIMESTAMP,:LOGTEXT)";
                _helper2.AddInParameter(":LOGTEXT", logtext, DbType.String);
            }
            else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb" || db_type.ToLower() == "ms sql")
            {
                SqlQuery = "insert into axp_webtrace (USERNAME,SESSIONID,DATETIME,LOGTEXT)  values ('" + userName + "','" + sessId + "',CURRENT_TIMESTAMP,@LOGTEXT)";
                _helper2.AddInParameter("@LOGTEXT", logtext, DbType.String);
            }
            else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
            {
                SqlQuery = "insert into axp_webtrace (USERNAME,SESSIONID,DATETIME,LOGTEXT) values ('" + userName + "','" + sessId + "',CURRENT_TIMESTAMP,:LOGTEXT)";
                _helper2.AddInParameter(":LOGTEXT", logtext, DbType.String);
            }
            _helper2.ExecuteNonQuerySqlinline(SqlQuery);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("SaveExecutionTrace - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "SaveExecutionTrace", string.Empty, "true");
        }
    }

    public void SaveMobileNotification(string guid, string fb_Id, string imei_no, string status)
    {
        try
        {
            string SqlQuery = string.Empty;
            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            string projectName = HttpContext.Current.Session["project"].ToString();
            string sessId = HttpContext.Current.Session.SessionID;
            SqlQuery = "select * from ax_mobilenotify where username='" + userName + "' and projectname='" + projectName + "'";
            DataSet dtMn = new DataSet();
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            dtMn = _helper1.ExecuteDataSetSqlInline(SqlQuery);
            if (dtMn.Tables[0].Rows.Count > 0)
            {
                Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
                SqlQuery = "update ax_mobilenotify SET GUID='" + guid + "',FIREBASE_ID='" + fb_Id + "',IMEI_NO='" + imei_no + "',STATUS='" + (status == "true" ? "t" : "f") + "' where username='" + userName + "' and projectname='" + projectName + "'";
                _helper2.ExecuteNonQuerySqlinline(SqlQuery);
            }
            else
            {
                Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
                SqlQuery = "insert into ax_mobilenotify (USERNAME,PROJECTNAME,GUID,FIREBASE_ID,IMEI_NO,STATUS) values ('" + userName + "','" + HttpContext.Current.Session["project"].ToString() + "','" + sessId + "','" + fb_Id + "','" + imei_no + "','" + (status == "true" ? "t" : "f") + "')";
                _helper2.ExecuteNonQuerySqlinline(SqlQuery);
            }
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
            {
                string _fillgdResize = "error:" + HttpContext.Current.Session["AxDirectDBException"].ToString();
                HttpContext.Current.Session.Remove("AxDirectDBException");
                logobj.CreateLog("SaveSearchViewColumns - " + _fillgdResize + "", HttpContext.Current.Session["nsessionid"].ToString(), "SaveSearchViewColumns", string.Empty, "true");
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("SaveMobileNotification - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "SaveMobileNotification", string.Empty, "true");
        }
    }

    public string GetSearchViewColumns(string transId)
    {
        string vcSelected = string.Empty;
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            string SqlQuery = string.Empty;
            string username = string.Empty;
            if (HttpContext.Current.Session["user"] != null)
                username = HttpContext.Current.Session["user"].ToString();
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            SqlQuery = "Select PROPS from searchdef where transid='" + transId + "' and username='" + username + "' order by userlevel desc";
            DataSet tblVCS = new DataSet();
            tblVCS = _helper1.ExecuteDataSetSqlInline(SqlQuery);
            if (tblVCS.Tables.Count > 0 && tblVCS.Tables[0].Rows.Count > 0 && tblVCS.Tables[0].Rows[0]["PROPS"].ToString() != "")
            {
                vcSelected = tblVCS.Tables[0].Rows[0]["PROPS"].ToString();
            }

            if (vcSelected == string.Empty && HttpContext.Current.Session["AxDirectDBException"] != null)
            {
                vcSelected = "error:" + HttpContext.Current.Session["AxDirectDBException"].ToString();
                HttpContext.Current.Session.Remove("AxDirectDBException");
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("GetSearchViewColumns - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetSearchViewColumns", string.Empty, "true");
        }
        return vcSelected;
    }

    public void SaveSearchViewColumns(string transId, string selectedFlds, bool isViewColUpdate)
    {
        try
        {
            string SqlQuery = string.Empty;
            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            string sessId = HttpContext.Current.Session.SessionID;
            Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
            //selectedFlds = "189,388,350,590,1~~~~" + selectedFlds;
            selectedFlds = selectedFlds.Replace("~", "\r\n");
            if (isViewColUpdate)
            {
                if (db_type.ToLower() == "oracle")
                {
                    SqlQuery = "update searchdef SET PROPS=:PROPS,USERLEVEL=3 where TRANSID='" + transId + "' and USERNAME='" + userName + "'";
                    _helper2.AddInParameter(":PROPS", selectedFlds, DbType.String);
                }
                else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb" || db_type.ToLower() == "ms sql")
                {
                    SqlQuery = "update searchdef SET PROPS=@PROPS,USERLEVEL=3 where TRANSID='" + transId + "' and USERNAME='" + userName + "'";
                    _helper2.AddInParameter("@PROPS", selectedFlds, DbType.String);
                }
                else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
                {
                    SqlQuery = "update searchdef SET PROPS=:PROPS,USERLEVEL=3 where TRANSID='" + transId + "' and USERNAME='" + userName + "'";
                    _helper2.AddInParameter(":PROPS", selectedFlds, DbType.String);
                }
            }
            else
            {
                if (db_type.ToLower() == "oracle")
                {
                    SqlQuery = "insert into searchdef  (TRANSID, USERNAME, PROPS, USERLEVEL, SMODE) values ('" + transId + "','" + userName + "',:PROPS,3,'c')";
                    _helper2.AddInParameter(":PROPS", selectedFlds, DbType.String);
                }
                else if (db_type.ToLower() == "mysql" || db_type.ToLower() == "mariadb" || db_type.ToLower() == "ms sql")
                {
                    SqlQuery = "insert into searchdef  (TRANSID, USERNAME, PROPS, USERLEVEL, SMODE)  values ('" + transId + "','" + userName + "',@PROPS,3,'c')";
                    _helper2.AddInParameter("@PROPS", selectedFlds, DbType.String);
                }
                else if (db_type.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
                {
                    SqlQuery = "insert into searchdef  (TRANSID, USERNAME, PROPS, USERLEVEL, SMODE) values ('" + transId + "','" + userName + "',:PROPS,3,'c')";
                    _helper2.AddInParameter(":PROPS", selectedFlds, DbType.String);
                }
            }
            _helper2.ExecuteNonQuerySqlinline(SqlQuery);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("SaveSearchViewColumns - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "SaveSearchViewColumns", string.Empty, "true");
        }
    }

    public string GetUserProfilePic()
    {
        string strRes = string.Empty;
        try
        {
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            string SqlQuery = string.Empty;
            string username = string.Empty;
            if (HttpContext.Current.Session["user"] != null)
                username = HttpContext.Current.Session["user"].ToString();
            SqlQuery = "select a.img,a.blobno from axusrprofilepic a,axusers b where a.recordid =b.axusersid and b.username ='" + username + "'";
            DataSet tblppc = new DataSet();
            tblppc = _helper1.ExecuteDataSetSqlInline(SqlQuery);
            if (tblppc.Tables.Count > 0 && tblppc.Tables[0].Rows.Count > 0)
            {
                if (tblppc.Tables[0].Rows[0]["IMG"] != "")
                {
                    byte[] Uimage = (byte[])tblppc.Tables[0].Rows[0]["IMG"];
                    strRes = Convert.ToBase64String(Uimage);
                }
            }

            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();

            FDW fdwObj = new FDW();
            string KeyData = Constants.REDISPROFILEPIC;
            Util.Util util = new Util.Util();
            if (strRes != string.Empty)
            {
                fdwObj.SaveInRedisServer(util.GetRedisServerkey(KeyData, HttpContext.Current.Session["username"].ToString()), strRes, Constants.REDISPROFILEPIC, schemaName);
            }
            else
            {
                strRes = "no profile pic";
                fdwObj.SaveInRedisServer(util.GetRedisServerkey(KeyData, HttpContext.Current.Session["username"].ToString()), strRes, Constants.REDISPROFILEPIC, schemaName);
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("GetUserProfilePic - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetUserProfilePic", string.Empty, "true");
        }
        return strRes;
    }


    public string SaveFillgridResize(string transid, string fillgridName, string fgvalues)
    {
        try
        {
            string SqlQuery = string.Empty;
            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
                HttpContext.Current.Session.Remove("AxDirectDBException");
            string projectName = HttpContext.Current.Session["project"].ToString();
            string sessId = HttpContext.Current.Session.SessionID;
            SqlQuery = "select * from fillgriddef where transid='" + transid + "' and fg_name='" + fillgridName + "'";
            DataSet dtMn = new DataSet();
            Ihelper _helper1 = new Helper().SetDatabase(db_type, _connectionString);
            dtMn = _helper1.ExecuteDataSetSqlInline(SqlQuery);
            if (dtMn.Tables[0].Rows.Count > 0)
            {
                Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
                SqlQuery = "update fillgriddef SET Colsize='" + fgvalues + "' where transid='" + transid + "' and fg_name='" + fillgridName + "'";
                _helper2.ExecuteNonQuerySqlinline(SqlQuery);
            }
            else
            {
                Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
                SqlQuery = "insert into fillgriddef (TRANSID,FG_NAME,FORMSIZE,COLSIZE) values ('" + transid + "','" + fillgridName + "','155,452,506,631','" + fgvalues + "')";
                _helper2.ExecuteNonQuerySqlinline(SqlQuery);
            }

            if (HttpContext.Current.Session["AxDirectDBException"] != null)
            {
                string _fillgdResize = "error:" + HttpContext.Current.Session["AxDirectDBException"].ToString();
                HttpContext.Current.Session.Remove("AxDirectDBException");
                return _fillgdResize;
            }
            else
                return "Saved successfully.";
        }
        catch (Exception ex)
        {
            logobj.CreateLog("SaveFillgridResize - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "SaveFillgridResize", string.Empty, "true");
            return "Error while saving.";
        }
    }

    public string UpdateALCUserInfo(string username, string isActive, string authKey, string userKey)
    {
        string _results = string.Empty;
        try
        {
            string SqlQuery = string.Empty;
            Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
            SqlQuery = "update axusers SET ACTIVE='" + isActive + "',ACTFLAG='" + isActive + "',AUTHKEY='" + authKey + "',USERKEY='" + userKey + "' where username='" + username + "'";
            //_helper2.ExecuteNonQuerySqlinline(SqlQuery);
            _results = _helper2.ExecuteNonQuerySqlinlineAlc(SqlQuery);
            if (_results == "0" || _results.StartsWith("Error:"))
            {
                logobj.CreateLog("UpdateALCUserInfo error:- " + _results + "", HttpContext.Current.Session["nsessionid"].ToString(), "UpdateALCUserInfo", string.Empty, "true");
            }
        }
        catch (Exception ex)
        {
            _results = "Error: " + ex.Message;
            logobj.CreateLog("UpdateALCUserInfo - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "UpdateALCUserInfo", string.Empty, "true");
        }
        return _results;
    }
}





