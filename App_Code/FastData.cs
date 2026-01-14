using Newtonsoft.Json;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Summary description for FastData
/// </summary>
[Serializable]
public class FastData
{
    static RedisServer objRS = RedisServer.Instance;
    public string db_type = String.Empty;
    public int rowCountAC = 0;
    public string db_connection = String.Empty;
    string errorLog = string.Empty;
    Util.Util util = new Util.Util();
    LogFile.Log logObj = new LogFile.Log();

    public FastData()
    {
        if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["axdb"] != null && HttpContext.Current.Session["axconstr"] != null)
        {
            db_type = HttpContext.Current.Session["axdb"].ToString();
            db_connection = HttpContext.Current.Session["axconstr"].ToString();
        }
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        HttpContext.Current.Response.Write("<script language='javascript'>");
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write("</script>");
    }

    public bool IsNullOrEmpty(String value)
    {
        return (value == null || value.Length == 0);
    }

    public void CreateFastData()
    {
        string strProj = ConfigurationManager.AppSettings["proj"];
        if (!IsNullOrEmpty(strProj))
        {
            string contents = HttpContext.Current.Application["axApps"].ToString();
            HttpContext.Current.Application["ProjUser"] = strProj;
            objRS.schemaNameKey = strProj;
            GetDBConnection(strProj, contents);
            if (objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION) == null)
            {

                DataSet DsDefinition = GetDefinition();
                if (DsDefinition.Tables.Count > 0)
                    CreateFdDataset(DsDefinition, false);
            }
            //CreateFDThread();
        }
    }

    public void CreateFDThread()
    {
        //TODO: Thread to run the Pop Service for Refresh
        Thread popTh = new Thread(PopRefreshDs);
        popTh.Start();
        //logObj.CreateLog("Popping thread started", "7777", "LogThreadPath", "new", "true");
    }

    public void PopRefreshDs()
    {
        while (true)
        {
            var dsRefresh = objRS.DataTableFromRedis(Constants.FD_REFRESH_DT);
            if (dsRefresh != null)
            {
                DataTable dt = (DataTable)dsRefresh;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var curDsName = dt.Rows[i]["refDsName"].ToString();
                    //Pop the record
                    dt.Rows.RemoveAt(0);
                    dt.AcceptChanges();
                    objRS.PushPopInRedisServer(Constants.FD_REFRESH_DT, dt, "");
                    //logObj.CreateLog("POP Started----------In PopRefreshDs, Currently Popping-" + curDsName, "7777", "LogThreadPath", "", "true");

                    //Call Refresh for the removed record
                    RefreshFDDataSet(curDsName);

                    //logObj.CreateLog("POP Completed----------In PopRefreshDs, Currently Popping-" + curDsName, "7777", "LogThreadPath", "", "true");
                }
            }
        }

    }

    public void RefreshFDDataSet(string dsName)
    {
        //logObj.CreateLog("Refresh Started----------In RefreshFDDataSet, Currently Refreshing-" + dsName, "7777", "LogThreadPath", "", "true");
        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        if (memFastDataDef != null)
        {
            var dsDef = (DataSet)memFastDataDef;
            try
            {
                //logObj.CreateLog("Refreshing ds-" + dsName, HttpContext.Current.Session.SessionID, "RefreshFastDS", "");
                DataTable dsData = new DataTable();
                var dsQuery = dsDef.Tables[Constants.FD_DT_DEFINITION].AsEnumerable().Where(s => s.Field<string>("Datasetname") == dsName).Select(r => new { sqlText = r.Field<string>("sqltext"), sqlparam = r.Field<string>("Params") }).First();
                Ihelper objHelper = new Helper().SetDatabase(db_type, db_connection);
                dsData.Merge(objHelper.ExecuteDataSetSqlInline(dsQuery.sqlText).Tables[0]);
                objRS.SaveInRedisServer(dsName, dsData, "");
                //logObj.CreateLog("Refresh Completed----------In RefreshFDDataSet, Currently Refreshing-" + dsName, "7777", "LogThreadPath", "", "true");
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception in RefreshFastDataset-" + ex.Message, "7777", "RefreshFastDS", "new", "true");
            }
        }
    }

    public void PushRefreshDs(string dsName, string user)
    {
        var dtRefresh = objRS.DataTableFromRedis(Constants.FD_REFRESH_DT);

        if (dtRefresh != null)
        {
            //logObj.CreateLog("PUSH Started----------In PushRefreshDs, Currently Pushing-" + dsName + "User-" + user, "7777", "LogThreadPath", "", "true");
            bool hasDs = dtRefresh.AsEnumerable().Any(row => dsName == row.Field<String>("refDsName"));
            if (hasDs)
                return;
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("refDsName");
            dtRefresh = dt;
        }

        DataRow dr = dtRefresh.NewRow();
        dr["refDsName"] = dsName;
        dtRefresh.Rows.Add(dr);
        objRS.PushPopInRedisServer(Constants.FD_REFRESH_DT, dtRefresh, "");
        //logObj.CreateLog("PUSH Completed----------In PushRefreshDs, Currently Pushing-" + dsName, "7777", "LogThreadPath", "", "true");

    }

    public void GetDBConnection(string strProj, string contents)
    {
        DataSet dataSet = new DataSet();
        DataTable dataTable = new DataTable(strProj);
        dataTable.Columns.Add("type", typeof(string));
        dataTable.Columns.Add("db", typeof(string));
        dataTable.Columns.Add("version", typeof(string));
        dataTable.Columns.Add("driver", typeof(string));
        dataTable.Columns.Add("dbcon", typeof(string));
        dataTable.Columns.Add("dbuser", typeof(string));
        dataTable.Columns.Add("structurl", typeof(string));
        dataTable.Columns.Add("dataurl", typeof(string));
        dataTable.Columns.Add("pwd", typeof(string));
        dataSet.Tables.Add(dataTable);
        System.IO.StringReader xmlSR = new System.IO.StringReader(contents);
        dataSet.ReadXml(xmlSR, XmlReadMode.IgnoreSchema);
        string dbuser = dataSet.Tables[0].Rows[0]["dbuser"].ToString();
        string dbtype = dataSet.Tables[0].Rows[0]["db"].ToString();
        string pwd = dataSet.Tables[0].Rows[0]["pwd"].ToString();
        string Server = dataSet.Tables[0].Rows[0]["dbcon"].ToString();
        string database = dataSet.Tables[0].Rows[0]["dbuser"].ToString();
        string cs = "";
        pwd = util.GetDBPassword(database);

        if (dbtype.ToLower() == "ms sql")
        {
            cs = @"Server = " + Server + "; Database= " + database + "; User Id = " + dbuser + "; Password = " + pwd + ";";
        }
        if (dbtype.ToLower() == "oracle")
        {
            cs = @"Data Source=" + Server + ";User Id=" + dbuser + ";Password=" + pwd + ";Pooling=False;";
        }
        if (dbtype.ToLower() == "mysql" || dbtype.ToLower() == "mariadb")
        {
            cs = @"Server=" + Server + ";Database=" + database + ";Uid=" + dbuser + ";Pwd=" + pwd + ";";
        }
        if (dbtype.ToLower() == "postgresql"|| dbtype.ToLower() == "postgre")
        {
            if (dbuser.Contains("\\") || database.Contains("\\"))
            {
                string[] userDtls = dbuser.Split('\\');
                string[] databaseDtls = database.Split('\\');
                if ((userDtls.Length > 1 && userDtls[1] != "") && (databaseDtls.Length > 1 && databaseDtls[1] != ""))
                {
                    dbuser = userDtls[1];
                    database = databaseDtls[1];
                }
                else
                {
                    dbuser = userDtls[0];
                    database = databaseDtls[0];
                }
            }
            else
            {
                database = "axpertdb";

            }
            cs = @"Server=" + Server + ";Database=" + database + ";Uid=" + dbuser + ";Pwd=" + pwd + ";";
        }
            db_type = dbtype;
        db_connection = cs;
    }

    public void FastDataMultiSchema()
    {
        if (HttpContext.Current.Session["dbuser"] != null)
        {
            objRS.schemaNameKey = HttpContext.Current.Session["dbuser"].ToString();
            if (objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION) == null)
            {
                DataSet DsDefinition = GetDefinition();
                if (DsDefinition.Tables.Count > 0)
                    CreateFdDataset(DsDefinition, false);
            }
            // CreateFDThread();
        }
    }

    public DataSet GetDefinition()
    {
        DataSet DfDataset = new DataSet();
        try
        {
            Ihelper objHelper = new Helper().SetDatabase(db_type, db_connection);
            DfDataset.Tables.Add(Constants.FD_DT_DEFINITION);
            DfDataset.Tables[Constants.FD_DT_DEFINITION].Merge(objHelper.ExecuteDataSetSqlInline(Constants.FD_TBL_DEFINITION).Tables[0]);
            DfDataset.Tables.Add(Constants.FD_DT_ASSOCIATION);
            DfDataset.Tables[Constants.FD_DT_ASSOCIATION].Merge(objHelper.ExecuteDataSetSqlInline(Constants.FD_TBL_ASSOCIATION).Tables[0]);
            objRS.SaveInRedisServer(Constants.RS_FD_DEFINITION, DfDataset, "");
        }
        catch (Exception ex)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                logObj.CreateLog("Exception in GetDefinition-" + ex.Message, HttpContext.Current.Session.SessionID, "GetDefinition", "new");
            else
                logObj.CreateLog("Exception in GetDefinition-" + ex.Message, "Fast data on application start", "GetDefinition", "new");
        }
        return DfDataset;
    }

    public void CreateFdDataset(DataSet dtDefin, bool isLogedin)
    {
        try
        {
            Ihelper objHelper = new Helper().SetDatabase(db_type, db_connection);
            foreach (DataRow row in dtDefin.Tables[Constants.FD_DT_DEFINITION].Rows)
            {
                string strDsName = row["DataSetName"].ToString();
                string strSqlQuery = row["SQLText"].ToString();
                string strLoadEvent = row["LoadEvent"].ToString();
                if (!IsNullOrEmpty(strDsName) && !IsNullOrEmpty(strSqlQuery) && !IsNullOrEmpty(strLoadEvent))
                {
                    DataTable dtName = new DataTable(strDsName);
                    try
                    {
                        if (strLoadEvent == "onlogin" && isLogedin)
                        {
                            // prashik need to do better 
                            string currentSQL = LoginSuffixAndParamsToSQL(row)[1];
                            dtName.Merge(objHelper.ExecuteDataSetSqlInline(currentSQL).Tables[0]);
                            objRS.SaveInRedisServer(strDsName + LoginSuffixAndParamsToSQL(row)[0], dtName, "");
                        }
                        else
                        {
                            dtName.Merge(objHelper.ExecuteDataSetSqlInline(strSqlQuery).Tables[0]);
                            objRS.SaveInRedisServer(strDsName, dtName, "");
                        }
                    }
                    catch (Exception ex)
                    {
                        objRS.ClearRedisServerDataByKey(strDsName, "", false);

                        if (HttpContext.Current != null && HttpContext.Current.Session != null)
                            logObj.CreateLog("Fastdata datatables- DsName-" + strDsName + "-exception-" + ex.Message, HttpContext.Current.Session.SessionID, "CreateFdDataset", "new");
                        else
                            logObj.CreateLog("Fastdata datatables- DsName-" + strDsName + "-exception-" + ex.Message, "Fast data dataset on application start", "CreateFdDataset", "new");
                    }
                }
                else
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null)
                        logObj.CreateLog("Fastdata datatables- DsName-" + strDsName + "-SqlQuery-" + strSqlQuery + "-LoadEvent-" + strLoadEvent, HttpContext.Current.Session.SessionID, "CreateFdDataset", "new");
                    else
                        logObj.CreateLog("Fastdata datatables- DsName-" + strDsName + "-SqlQuery-" + strSqlQuery + "-LoadEvent-" + strLoadEvent, "Fast data dataset on application start", "CreateFdDataset", "new");
                }
            }
        }
        catch (Exception ex)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                logObj.CreateLog("Exception in CreateFdDataset-" + ex.Message, HttpContext.Current.Session.SessionID, "CreateFdDataset", "new");
            else
                logObj.CreateLog("Exception in CreateFdDataset-" + ex.Message, "Fast data dataset on application start", "CreateFdDataset", "new");
        }
    }

    public string[] LoginSuffixAndParamsToSQL(DataRow row)
    {
        //NOte: opType = suffix/params
        string[] returnString = new string[2];
        returnString[1] = row["SQLText"].ToString();
        string[] Parameters = row["Params"].ToString().Split(',');
        foreach (string par in Parameters)
        {
            String[] SeparatePar = par.Split(':');

            if (!String.IsNullOrEmpty(HttpContext.Current.Session[SeparatePar[1].Trim()].ToString()))
            {
                string setValue = HttpContext.Current.Session[SeparatePar[1].Trim()].ToString();
                setValue = util.ReplaceSqlInjChar(setValue);
                if (ParseString(setValue))
                {
                    setValue = "'" + setValue + "'";
                }
                returnString[0] += "-" + HttpContext.Current.Session[SeparatePar[1].Trim()].ToString();
                returnString[1] = returnString[1].Replace(":" + SeparatePar[0].Trim(), setValue);
            }
            else
            {
                returnString[1] = String.Empty;
            }
        }
        return returnString;
    }



    public void RefreshFastDataset(string dsName)
    {
        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        DataTable dsData = (DataTable)objRS.DataTableFromRedis(dsName);
        if (memFastDataDef != null)
        {
            var ds = (DataSet)memFastDataDef;
            DataSet DfDataset = new DataSet();
            try
            {
                var dsQuery = ds.Tables[Constants.FD_DT_DEFINITION].AsEnumerable().Where(s => s.Field<string>("Datasetname") == dsName).Select(r => new { sqlText = r.Field<string>("sqltext"), sqlparam = r.Field<string>("Params") }).First();
                Ihelper objHelper = new Helper().SetDatabase(db_type, db_connection);
                dsData.Merge(objHelper.ExecuteDataSetSqlInline(dsQuery.sqlText).Tables[0]);
                objRS.SaveInRedisServer(dsName, dsData, "");
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception in RefreshFastDataset-" + ex.Message, "7777", "RefreshFastDS", "new", "true");
            }
        }
    }

    public string RefreshFastDatasetUtil(string dsName)
    {

        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        DataTable dsData = (DataTable)objRS.DataTableFromRedis(dsName);
        if (memFastDataDef != null)
        {
            var ds = (DataSet)memFastDataDef;
            DataSet DfDataset = new DataSet();
            try
            {
                var dsQuery = ds.Tables[Constants.FD_DT_DEFINITION].AsEnumerable().Where(s => s.Field<string>("Datasetname") == dsName).Select(r => new { sqlText = r.Field<string>("sqltext"), sqlparam = r.Field<string>("Params") }).First();
                Ihelper objHelper = new Helper().SetDatabase(db_type, db_connection);
                dsData.Merge(objHelper.ExecuteDataSetSqlInline(dsQuery.sqlText).Tables[0]);
                objRS.SaveInRedisServer(dsName, dsData, "");
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception in RefreshFastDataset-" + ex.Message, HttpContext.Current.Session.SessionID, "RefreshFastDS", "new");
                return ex.Message;
            }
        }
        return "1";
    }
    #region Fast Data
    public string GetFastDataFldMaps(string fldName, TStructDef tstDef)
    {
        string fldMappint = string.Empty;
        if (tstDef.dsFastDataDef.Tables.Count > 0)
        {
            fldMappint = tstDef.dsFastDataDef.Tables[Constants.FD_DT_ASSOCIATION].AsEnumerable().Where(s => s.Field<string>("FieldOrParamName") == fldName && s.Field<string>("Transid") == tstDef.transId).Select(r => r.Field<string>("FldMapping")).First();
            if (fldMappint == null)
            {
                fldMappint = string.Empty;
            }
        }
        return fldMappint;
    }


    /// <summary>
    /// Function to get the datalist constructued for the given fields in fieldname
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="tid"></param>
    /// <param name="tstData"></param>
    /// <returns></returns>
    public string GetDepFastDataJson(string fieldName, string tid, TStructData tstData, string rowNo)
    {
        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        string result = string.Empty;
        StringBuilder strFlds = new StringBuilder();
        StringBuilder resultJson = new StringBuilder();
        if (memFastDataDef != null)
        {
            var ds = (DataSet)memFastDataDef;
            foreach (var fldName in fieldName.Split(','))
            {
                if (!IsNullOrEmpty(fldName))
                {
                    try
                    {
                        DataTable dt_Assoc = ds.Tables[Constants.FD_DT_ASSOCIATION];
                        var fastTableName = dt_Assoc.AsEnumerable().Where(s => s.Field<string>("FieldOrParamName") == fldName && s.Field<string>("Transid") == tid).Select(r => new { Fasttablename = r.Field<string>("Fasttablename"), filterstring = r.Field<string>("Filterstring") }).First();
                        //DataTable dt = objRS.DataTableFromRedis(fastTableName.Fasttablename);
                        DataTable dtDef = dt_Assoc.AsEnumerable().Where(x => x.Field<string>("FieldOrParamName") == fldName && x.Field<string>("Transid") == tstData.tstStrObj.transId && x.Field<string>("Filterstring") != null).CopyToDataTable();
                        foreach (DataRow row in dtDef.Rows)
                        {
                            string fastTblName = row["fasttablename"].ToString();
                            DataTable dtr = null;
                            var temdtr = ds.Tables[Constants.FD_DT_DEFINITION].AsEnumerable().Where(x => x.Field<string>("datasetname") == fastTblName && x.Field<string>("loadevent").ToLower().ToString() == "onlogin");
                            if (temdtr.Any())
                            {
                                dtr = temdtr.CopyToDataTable();
                            }
                            if (dtr != null)
                                row["fasttablename"] = fastTblName + LoginSuffixAndParamsToSQL(dtr.Rows[0])[0];
                        }
                        if (strFlds.ToString() == string.Empty)
                            strFlds.Append(fldName + rowNo + "F" + tstData.tstStrObj.GetFieldDc(fldName));
                        else
                            strFlds.Append("," + fldName + rowNo + "F" + tstData.tstStrObj.GetFieldDc(fldName));


                        //call function to get filtered data
                        DataTable dt = objRS.DataTableFromRedis(fastTableName.Fasttablename);
                        dt = GetFilteredData(dt, fastTableName.filterstring, tstData, "", "tstruct");

                        if (dt != null)
                            result += GetDataListItems(dtDef, dt, tstData.tstStrObj);
                        else
                            resultJson.Append(CreateAutoCompList(fldName, new DataTable(), rowNo, "", tstData.tstStrObj));
                    }
                    catch (Exception ex)
                    {
                        if (strFlds.ToString().IndexOf(fldName + rowNo + "F" + tstData.tstStrObj.GetFieldDc(fldName)) != -1)
                        {
                            string flds = strFlds.ToString().Replace(fldName + rowNo + "F" + tstData.tstStrObj.GetFieldDc(fldName), "");
                            strFlds = new StringBuilder();
                            strFlds.Append(flds);
                        }
                        logObj.CreateLog("Exception in GetDepFastDataJson-" + ex.Message, HttpContext.Current.Session.SessionID, "GetDepFastData", "new");
                    }
                }
            }
        }
        return strFlds + "*$*" + result + resultJson.ToString();
    }

    private DataTable GetFilteredDs(DataTable dt, string filterString, TStructData tstData, string[] pageData, string isSelectFld, string fldValue)
    {
        dt = GetFilteredData(dt, filterString, tstData, "", "tstruct");
        try
        {
            if (!string.IsNullOrEmpty(fldValue))
            {
                //To filter the values with autocomplete entered text
                dt = dt.AsEnumerable().Where(x => x.Field<string>(dt.Columns[1]).ToLower().Contains(fldValue.ToLower())).CopyToDataTable();
            }
            rowCountAC = dt.Rows.Count;
            if (isSelectFld.ToLower() != "true")
            {
                int pageNo = int.Parse(pageData[0]);
                int pageSize = int.Parse(Constants.ACPICKPAGESIZE);
                int pageStart = pageNo * pageSize - pageSize;
                int pageEnd = pageSize;
                int pageIndex = pageStart + pageSize;
                if (pageIndex > rowCountAC)
                {
                    pageEnd = rowCountAC - pageStart;
                }
                dt = dt.AsEnumerable().Skip(pageStart).Take(pageEnd).CopyToDataTable();
            }
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }

    /// <summary>
    /// Function to replace params and return the dataset
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="filterString"></param>
    /// <param name="tstData"></param>
    /// <param name="dashFilterValues"></param>
    /// <param name="calledFrom"></param>
    /// <returns></returns>
    public DataTable GetFilteredData(DataTable dt, string filterString, TStructData tstData, string dashFilterValues, string calledFrom)
    {
        Array filterParams = filterString.Split(',').ToArray();
        foreach (var parms in filterParams)
        {
            //Parameter can have direct values or mapped column names- ex: country='INDIA' or country=:ctry
            string paramData = parms.ToString();
            string pattern = @":[\w]*";
            var fldMapName = Regex.Matches(parms.ToString(), pattern);
            for (int i = 0; i < fldMapName.Count; i++)
            {
                string fldMap = fldMapName[i].ToString().Remove(0, 1);
                string paramValue = string.Empty;
                if (calledFrom == "tstruct")
                {
                    int fldDc = tstData.tstStrObj.GetFieldDc(fldMap);
                    paramValue = tstData.GetFieldValue(fldDc.ToString(), fldMap);
                }
                else if (calledFrom == "dashboard")
                {
                    Array dbfilters = dashFilterValues.Split('~').ToArray();
                    foreach (var dbfltValue in dbfilters)
                    {
                        string keyFld = dbfltValue.ToString().Split(':')[0];
                        string valueFld = dbfltValue.ToString().Split(':')[1];
                        if (keyFld == fldMap)
                            paramValue = valueFld;
                    }
                }
                if (ParseString(paramValue))
                    paramData = paramData.Replace(fldMapName[i].ToString(), "'" + paramValue + "'");
                else
                    paramData = paramData.Replace(fldMapName[i].ToString(), paramValue);
            }
            try
            {
                dt = dt.Select(paramData).CopyToDataTable();
            }
            catch (Exception ex)
            {
                dt = null;
            }
        }
        return dt;
    }

    public bool ParseString(string str)
    {
        Int32 intValue;
        Int64 bigintValue;
        Double doubleValue;
        Decimal decimalValue;
        DateTime dateValue;

        if (Int32.TryParse(str, out intValue))
            return false;
        else if (Int64.TryParse(str, out bigintValue))
            return false;
        else if (Double.TryParse(str, out doubleValue))
            return false;
        else if (Decimal.TryParse(str, out decimalValue))
            return false;
        else if (DateTime.TryParse(str, out dateValue))
            return true;
        else return true;

    }

    private string GetFDDataSetName(string fieldName, string transid)
    {
        string dsName = string.Empty;

        var fastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        if (fastDataDef != null)
        {
            try
            {
                var ds = (DataSet)fastDataDef;
                var fastTableName = ds.Tables[Constants.FD_DT_ASSOCIATION].AsEnumerable().Where(s => s.Field<string>("FieldOrParamName") == fieldName && s.Field<string>("Transid") == transid).Select(r => new { Fasttablename = r.Field<string>("Fasttablename"), filterstring = r.Field<string>("Filterstring") }).First();
                dsName = fastTableName.Fasttablename + "`" + fastTableName.filterstring;
            }
            catch (Exception ex)
            {

            }
        }

        return dsName;
    }

    public string GetFromFastData(string fieldName, string transid, TStructData tstData, string[] pageData, string isSelectFld, string fldValue)
    {
        string result = string.Empty;
        var jsonForAC = string.Empty;
        string dsName = string.Empty;
        string filterStr = string.Empty;
        string dsDetails = GetFDDataSetName(fieldName, transid);
        if (dsDetails != string.Empty && dsDetails.IndexOf('`') != -1)
        {
            var strDsDtls = dsDetails.Split('`');
            if (strDsDtls.Length > 1)
            {
                dsName = strDsDtls[0].ToString();
                filterStr = strDsDtls[1].ToString();
            }
            var memFastDataDef = objRS.DataTableFromRedis(dsName);
            string fldMapping = GetFastDataFldMaps(fieldName, tstData.tstStrObj);
            var strFldMap = fldMapping.Split(',');

            if (memFastDataDef != null)
            {
                var dt = (DataTable)memFastDataDef;
                dt = GetFilteredDs(dt, filterStr, tstData, pageData, isSelectFld, fldValue);
                jsonForAC = "{\"pickdata\":[{\"rcount\":\"" + rowCountAC + "\"},{\"fname\":\"" + fieldName + "\"},[";
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strFldMap.Length > 1)
                            jsonForAC += "{\"i\":\"" + dt.Rows[i][strFldMap[1].ToString()] + "\",\"v\":\"" + dt.Rows[i][strFldMap[0].ToString()] + "\"},";
                        else
                            jsonForAC += "{\"i\":\"\",\"v\":\"" + dt.Rows[i][strFldMap[0].ToString()] + "\"},";
                    }
                }
                jsonForAC = jsonForAC.Substring(0, jsonForAC.Length - 1);
                jsonForAC += "]]}";
            }
        }
        return jsonForAC;
    }

    public void GetFastDataRefEvents(TStructDef tstDef)
    {
        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        if (memFastDataDef != null)
        {
            var ds = (DataSet)memFastDataDef;
            try
            {
                //Refresh Events = transid-savetask,actionname~transid2-save
                var refEvents = ds.Tables[Constants.FD_DT_DEFINITION].AsEnumerable().Where(s => s.Field<string>("RefreshEvents") != null).Select(r => new { RefreshEvents = r.Field<string>("RefreshEvents"), RefDataSet = r.Field<string>("Datasetname") }).ToList();
                for (int i = 0; i < refEvents.Count; i++)
                {
                    string[] refEvts = refEvents[i].ToString().Split('~');
                    for (int j = 0; j < refEvts.Length; j++)
                    {
                        if (refEvts[i].ToString().StartsWith(tstDef.transId + "-"))
                        {
                            tstDef.fastDataRefDSName.Add(refEvents[i].RefDataSet);
                            tstDef.fastDataRefEvent.Add(refEvents[i].RefreshEvents);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception in GetFastDataRefEvents-" + ex.Message, HttpContext.Current.Session.SessionID, "GetFastDataRefEvents", "new");
            }
        }
    }

    public string GetRedisServerFields(TStructDef tstDef)
    {
        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        string fastDataScript = string.Empty;
        if (memFastDataDef != null)
        {
            try
            {
                DataTable dtAssociation = memFastDataDef.Tables[Constants.FD_DT_ASSOCIATION].AsEnumerable().Where(s => s.Field<string>("Transid") == tstDef.transId).CopyToDataTable();
                var str = dtAssociation.AsEnumerable().Select(r => r.Field<string>("FieldOrParamName")).ToList();
                tstDef.fastDataFlds = string.Join(",", str.Select(x => x.ToString()).ToArray());

                //Dynamic Fields 
                var strDynamicfld = dtAssociation.AsEnumerable().Where(x => x.Field<string>("Isdynamic") == "T").Select(r => r.Field<string>("FieldOrParamName")).ToList();
                tstDef.fastDataDynamicFlds = string.Join(",", strDynamicfld.Select(x => x.ToString()).ToArray());

                //Active Fields
                var strActivefld = dtAssociation.AsEnumerable().Where(x => x.Field<string>("Isdynamic") == "T" && x.Field<string>("IsActive") == "T").Select(r => r.Field<string>("FieldOrParamName")).ToList();
                tstDef.fastDataActiveFlds = string.Join(",", strActivefld.Select(x => x.ToString()).ToArray());
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception in GetRedisServerFields-" + ex.Message, HttpContext.Current.Session.SessionID, "GetRedisServerFields", "new");
            }
        }
        return tstDef.fastDataFlds;
    }

    public bool IsFastFillGrid(TStructDef tstDef, string fillGridName, string dc)
    {
        bool isFastFillGrid = false;
        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        string fastDataScript = string.Empty;
        if (memFastDataDef != null)
        {
            isFastFillGrid = memFastDataDef.Tables[Constants.FD_DT_ASSOCIATION].AsEnumerable().Any(row => fillGridName == row.Field<String>("FieldOrParamName") && row.Field<string>("dcname") == "dc" + dc && row.Field<string>("Transid") == tstDef.transId);
        }
        return isFastFillGrid;
    }

    public string GetFastFillGridXML(TStructDef tstDef, string fillGridName, string dc)
    {
        string fastXML = string.Empty;
        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        string fastDataScript = string.Empty;
        if (memFastDataDef != null)
        {
            if (IsFastFillGrid(tstDef, fillGridName, dc))
            {

                var fastTableName = memFastDataDef.Tables[Constants.FD_DT_ASSOCIATION].AsEnumerable().Where(s => s.Field<string>("FieldOrParamName") == fillGridName && s.Field<string>("Transid") == tstDef.transId).Select(r => new { Fasttablename = r.Field<string>("Fasttablename") }).First();
                DataTable dt = objRS.DataTableFromRedis(fastTableName.Fasttablename);
                fastXML = CustomDatatableToXML(dt);
            }

        }
        return fastXML;
    }


    public string CustomDatatableToXML(DataTable dt)
    {
        XDocument doc = new XDocument();
        XElement root = new XElement("root");
        XElement response = new XElement("response");
        response.SetAttributeValue("multiselet", "true");
        XElement headrow = new XElement("headrow");
        headrow.SetAttributeValue("tlhw", "0,0,0,0");
        foreach (DataColumn col in dt.Columns)
        {
            XElement coll = new XElement(col.ColumnName.ToString());
            coll.SetAttributeValue("width", "");
            coll.Value = col.ColumnName.ToString();
            headrow.Add(coll);
        }

        response.Add(headrow);

        foreach (DataRow row in dt.Rows)
        {

            XElement rows = new XElement("row");
            foreach (DataColumn col in dt.Columns)
            {


                XElement roww = new XElement(col.ColumnName.ToString());
                roww.Value = row[col.ColumnName].ToString();
                rows.Add(roww);
            }
            response.Add(rows);
        }

        root.Add(response);
        doc.Add(root);
        return doc.ToString();

    }


    public string GetFastAjaxData(string fldName, string fldValue, string calledFrom, TStructDef tstDef)
    {
        string result = string.Empty;
        DataSet dtDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        DataTable dtFldData = null;
        string fldMapName = string.Empty;
        try
        {
            var fldProps = dtDef.Tables[Constants.FD_DT_ASSOCIATION].AsEnumerable().Where(s => s.Field<string>("Transid") == tstDef.transId && s.Field<string>("Fieldorparamname") == fldName).Select(x => new { fastTableName = x.Field<string>("Fasttablename"), fldMapp = x.Field<string>("Fldmapping") }).First();
            dtFldData = objRS.DataTableFromRedis(fldProps.fastTableName);
            var strFldMap = fldProps.fldMapp.ToString().Split(',');
            if (strFldMap.Length > 1)
                fldMapName = strFldMap[1].ToString();
            else
                fldMapName = strFldMap[0].ToString();

            var dsFilteredData = dtFldData.AsEnumerable().Where(s => s.Field<string>(fldMapName).Contains(fldValue)).CopyToDataTable();
            result = CreateAutoCompList(fldName, dsFilteredData, "", calledFrom, tstDef);
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Exception in GetFastAjaxData-" + ex.Message, HttpContext.Current.Session.SessionID, "GetFastAjaxData", "new");
        }
        return result;
    }

    public string FastDataddlBind(string transId, TStructData tstData)
    {
        string json = string.Empty;
        StringBuilder draftJson = new StringBuilder();
        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        if (memFastDataDef != null)
        {
            var ds = (DataSet)memFastDataDef;
            try
            {
                DataTable dtAssociation = ds.Tables[Constants.FD_DT_ASSOCIATION];
                ////Fields that do not have any parameter in query
                DataTable dtFlds = dtAssociation.AsEnumerable().Where(x => x.Field<string>("Transid") == transId && x.Field<string>("Filterstring") == null && x.Field<string>("Isdynamic") != "T").CopyToDataTable();
                foreach (DataRow row in dtFlds.Rows)
                {
                    string fastTblName = row["fasttablename"].ToString();
                    DataTable dtr = null;
                    var temdtr = ds.Tables[Constants.FD_DT_DEFINITION].AsEnumerable().Where(x => x.Field<string>("datasetname") == fastTblName && x.Field<string>("loadevent").ToLower().ToString() == "onlogin");
                    if (temdtr.Any())
                    {
                        dtr = temdtr.CopyToDataTable();
                    }
                    if (dtr != null)
                        row["fasttablename"] = fastTblName + LoginSuffixAndParamsToSQL(dtr.Rows[0])[0];
                }
                draftJson.Append(GetDataListItems(dtFlds, null, tstData.tstStrObj));

                ////Fields that have parameter in query
                DataTable dtFldsWithParam = null;
                var tempDs = dtAssociation.AsEnumerable().Where(x => x.Field<string>("Transid") == transId && x.Field<string>("Filterstring") != null && x.Field<string>("Isdynamic") != "T");
                if (tempDs.Any())
                {
                    dtFldsWithParam = tempDs.CopyToDataTable();
                }
                if (dtFldsWithParam != null)
                {
                    var fastTableName = dtFldsWithParam.AsEnumerable().Where(s => s.Field<string>("Transid") == transId).Select(r => r.Field<string>("Fieldorparamname")).ToList();
                    string fldsWithParams = string.Join(",", fastTableName.Select(x => x.ToString()).ToArray());
                    draftJson.Append(GetDepFastDataJson(fldsWithParams, transId, tstData, "0"));
                }
            }
            catch (Exception ex)
            {
                draftJson = new StringBuilder();
                logObj.CreateLog("Exception in FastDataddlBind-" + ex.Message, HttpContext.Current.Session.SessionID, "FastDataddlBind", "new");
            }
        }
        return draftJson.ToString();
    }

    private string GetDataListItems(DataTable dtFlds, DataTable dtFilteredData, TStructDef tstDef)
    {
        StringBuilder strList = new StringBuilder();
        string json = string.Empty;
        if (dtFilteredData == null)
        {
            for (int i = 0; i < dtFlds.Rows.Count; i++)
            {
                try
                {
                    dtFilteredData = objRS.DataTableFromRedis(dtFlds.Rows[i]["Fasttablename"].ToString());
                    if (dtFilteredData != null)
                        strList.Append(CreateAutoCompList(dtFlds.Rows[i]["FieldOrParamName"].ToString(), dtFilteredData, "", "", tstDef));
                }
                catch (Exception e)
                {
                    logObj.CreateLog("Exception in GetDataListItems1-" + e.Message, HttpContext.Current.Session.SessionID, "GetDataListItems", "new");
                }
            }
        }
        else
        {
            try
            {
                strList.Append(CreateAutoCompList(dtFlds.Rows[0]["FieldOrParamName"].ToString(), dtFilteredData, "", "", tstDef));
            }
            catch (Exception e)
            {
                logObj.CreateLog("Exception in GetDataListItems2-" + e.Message, HttpContext.Current.Session.SessionID, "GetDataListItems", "new");
            }
        }
        return strList.ToString();
    }

    private string CreateAutoCompList(string fieldName, DataTable dt, string fldRowNo, string source, TStructDef tstDef)
    {
        StringBuilder dtList = new StringBuilder();
        var idx = tstDef.GetFieldIndex(fieldName);
        TStructDef.FieldStruct fld = (TStructDef.FieldStruct)tstDef.flds[idx];
        string isFldNormalised = "no";
        if (fld.savenormal == true)
            isFldNormalised = "yes";

        int rowNo = 0;
        if (fldRowNo == "")
        {
            if (tstDef.IsDcGrid(fld.fldframeno))
                rowNo = 1;
        }
        else
            rowNo = Convert.ToInt16(fldRowNo);

        string[] fldMappint = GetFastDataFldMaps(fieldName, tstDef).Split(',');
        if (source != "keypress")
            dtList.Append("<datalist id='axlist-" + fieldName + tstDef.GetRowNoHelper(rowNo) + "F" + fld.fldframeno + "'>");

        if (source == "keypress")
        {
            if (isFldNormalised == "yes")
            {
                HttpContext.Current.Session["fdFldData-" + fieldName + ""] = dt;
                HttpContext.Current.Session["fd-fm-" + fieldName + ""] = fldMappint[0] + "~" + fldMappint[1];
            }
            else
            {
                HttpContext.Current.Session["fdFldData-" + fieldName + ""] = dt;
                HttpContext.Current.Session["fd-fm-" + fieldName + ""] = fldMappint[0];
            }
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (isFldNormalised == "yes")
                    dtList.Append("<option data-value='" + dt.Rows[i][fldMappint[0]].ToString() + "' value='" + dt.Rows[i][fldMappint[1].Trim()].ToString() + "'></option>");
                else
                    dtList.Append("<option value='" + dt.Rows[i][fldMappint[0]].ToString() + "'></option>");
            }
        }
        if (source != "keypress")
            dtList.Append("</datalist>");

        return dtList.ToString();
    }

    public string GetAutoFilterFastData(string SessKey, string FldName, string FldValue, string fltValue)
    {
        string json = string.Empty;
        try
        {
            FldName = FldName.Substring(0, FldName.LastIndexOf("F") - 3);
            if (HttpContext.Current.Session["fd-HugeFlds"] != null)
            {
                if (HttpContext.Current.Session["fd-HugeFlds"].ToString() != FldName)
                    util.ClearFDFldSession();
            }

            HttpContext.Current.Session["fd-HugeFlds"] = FldName;
            if (HttpContext.Current.Session["fdFldData-" + FldName + ""] == null)
            {
                TStructData tstData = (TStructData)HttpContext.Current.Session[SessKey];
                GetFastAjaxData(FldName, FldValue, "keypress", tstData.tstStrObj);
            }
            string fldMapping = HttpContext.Current.Session["fd-fm-" + FldName + ""].ToString();
            var strFldMap = fldMapping.Split('~');
            DataTable dtAutoFlt = (DataTable)HttpContext.Current.Session["fdFldData-" + FldName + ""];
            if (strFldMap.Length > 1)
            {
                var autoCompleteData = dtAutoFlt.AsEnumerable().Where(r => r.Field<string>(strFldMap[1]).ToLower().Contains(fltValue.ToLower())).Select(x => new { Item = x.Field<string>(strFldMap[1]), Value = x.Field<decimal>(strFldMap[0]) }).Take(50).ToList();
                json = JsonConvert.SerializeObject(autoCompleteData, Newtonsoft.Json.Formatting.Indented);
            }
            else
            {
                var autoCompleteData = dtAutoFlt.AsEnumerable().Where(r => r.Field<string>(strFldMap[0]).ToLower().Contains(fltValue.ToLower())).Select(x => new { Item = x.Field<string>(strFldMap[0]), Value = "" }).Take(50).ToList();
                json = JsonConvert.SerializeObject(autoCompleteData, Newtonsoft.Json.Formatting.Indented);
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Exception in GetAutoFilterFastData -" + ex.Message, HttpContext.Current.Session.SessionID, "GetAutoFilterFastData", "new");
        }
        return json;
    }

    public string GetFastAdvPicklist(string tid, string inputxml, string fldName, string sqlFldName, string tempSearch, string pageNo, string pageSize, bool isActiveFld)
    {
        int page = int.Parse(pageNo);
        int pageSiz = int.Parse(pageSize);
        int skipRows = 0;
        StringBuilder resultJson = new StringBuilder();
        var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
        if (memFastDataDef != null)
        {
            var ds = (DataSet)memFastDataDef;
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(inputxml);
            XmlNode oMainNode = xd.SelectSingleNode("/sqlresultset");
            var ss = oMainNode.InnerXml.ToString();
            string inputxmls = oMainNode.OuterXml.Replace(ss, "");
            inputxml = inputxmls.Replace("></sqlresultset>", "");
            resultJson.Append(inputxml);
            resultJson.Append(" dcol='' colmap='' map=''>");

            try
            {
                DataTable dt = new DataTable();
                var fastTableName = ds.Tables[Constants.FD_DT_ASSOCIATION].AsEnumerable().Where(s => s.Field<string>("Transid") == tid && s.Field<string>("FieldOrParamName") == fldName).Select(r => new { Fasttablename = r.Field<string>("Fasttablename"), filterstring = r.Field<string>("Filterstring"), PicklistColumns = r.Field<string>("PicklistColumns") }).First();
                if (isActiveFld)
                {
                    var ActFieldSQl = ds.Tables[Constants.FD_DT_DEFINITION].AsEnumerable().Where(s => s.Field<string>("datasetname") == fastTableName.Fasttablename).Select(r => new { sqlText = r.Field<string>("sqlText") }).First();
                    DBContext obj = new DBContext();
                    dt = obj.GetDataTableInline(ActFieldSQl.sqlText);
                }
                else
                    dt = objRS.DataTableFromRedis(fastTableName.Fasttablename);
                if (dt.Rows.Count > 0 && !IsNullOrEmpty(fastTableName.PicklistColumns))
                {
                    if (tempSearch != "")
                    {
                        tempSearch = tempSearch.Substring(1, tempSearch.Length - 1);
                        dt = dt.AsEnumerable().Where(x => x.Field<string>(fldName).ToLower().Contains(tempSearch.ToLower())).CopyToDataTable();
                    }
                    var total = dt.Rows.Count;
                    skipRows = pageSiz * (page - 1);
                    DataTable advData = dt.AsEnumerable().Skip(skipRows).Take(pageSiz).ToList().CopyToDataTable();
                    if (advData.Rows.Count > 0)
                    {
                        string[] pickCloumns = fastTableName.PicklistColumns.Split(',');
                        ArrayList pickColumns = new ArrayList();
                        pickColumns.AddRange(pickCloumns);

                        resultJson.Append("<response totalrows='" + total + "'>");
                        foreach (DataRow dr in advData.Rows)
                        {
                            resultJson.Append("<row>");
                            foreach (DataColumn dc in advData.Columns)
                            {
                                if (pickColumns.IndexOf(dc.ColumnName.ToLower()) != -1)
                                    resultJson.Append("<" + dc.ColumnName + " w=''>" + dr[dc].ToString() + "</" + dc.ColumnName + ">");
                            }
                            resultJson.Append("</row>");
                        }
                        resultJson.Append("</response>");
                    }
                }
                else
                {
                    resultJson.Append("<response totalrows='0'></response>");
                }
            }
            catch (Exception ex)
            {
                resultJson.Append("<response totalrows='0'></response>");
                logObj.CreateLog("Get Fast Adv Picklist -" + ex.Message, HttpContext.Current.Session.SessionID, "GetFastAdvPicklist", "new");
            }
            resultJson.Append("</sqlresultset>");
        }
        return resultJson.ToString();
    }

    public DataTable GetDashBoardData(string widgetIdB, string fltParams)
    {
        DataTable dbdt = new DataTable();
        try
        {
            var memFastDataDef = objRS.DataSetFromRedis(Constants.RS_FD_DEFINITION);
            string result = string.Empty;
            StringBuilder strFlds = new StringBuilder();
            StringBuilder resultJson = new StringBuilder();
            if (memFastDataDef != null)
            {
                var ds = (DataSet)memFastDataDef;
                if (!IsNullOrEmpty(widgetIdB))
                {
                    DataTable dt_Assoc = ds.Tables[Constants.FD_DT_ASSOCIATION];
                    var fastTableName = dt_Assoc.AsEnumerable().Where(s => s.Field<string>("Transid") == "Dashboard" && s.Field<string>("widgetId") == widgetIdB).Select(r => new { Fasttablename = r.Field<string>("Fasttablename"), filterstring = r.Field<string>("Filterstring") }).First();
                    dbdt = objRS.DataTableFromRedis(fastTableName.Fasttablename);
                    if (fltParams != "" && fastTableName.filterstring != null)
                        dbdt = GetFilteredData(dbdt, fastTableName.filterstring, null, fltParams, "dashboard");
                }
            }
        }
        catch (Exception ex)
        {

        }
        return dbdt;
    }
    #endregion

}
