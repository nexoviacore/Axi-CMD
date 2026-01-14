using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Summary description for FDR
/// </summary>
[Serializable()]
public class FDR
{
    LogFile.Log logObj = new LogFile.Log();
    public string schemaNameKey = string.Empty;
    [NonSerialized]
    private ConfigurationOptions config;
    string redisIP = string.Empty;
    string redisPass = string.Empty;
    public int rowCountAC = 0;
    Util.Util utilObj = new Util.Util();
    public string redisIpLocalHost = string.Empty;

    public FDR(string SelectedProj = "")
    {
        if (SelectedProj == "" && HttpContext.Current.Session != null && HttpContext.Current.Session["RedisCacheIP"] != null && HttpContext.Current.Session["RedisCacheIP"].ToString() != "")
        {
            redisIP = HttpContext.Current.Session["RedisCacheIP"].ToString();
            if (HttpContext.Current.Session["RedisCachePwd"] != null && HttpContext.Current.Session["RedisCachePwd"].ToString() != "")
                redisPass = HttpContext.Current.Session["RedisCachePwd"].ToString();
            redisIpLocalHost = "";
        }
        else
        {
            string rcDetails = utilObj.GetRedisConnDetails(SelectedProj);
            if (rcDetails != "" && rcDetails != "localhost")
            {
                redisIP = rcDetails.Split('♣')[0];
                redisPass = rcDetails.Split('♣')[1];
                redisIpLocalHost = "";
            }
            else if (rcDetails != "" && rcDetails == "localhost")
            {
                redisIpLocalHost = "localhost";
            }
        }
    }

    private ConnectionMultiplexer RedisConnect(bool GetServerInfo = false)
    {
        try
        {
            if (config == null)
            {
                HashSet<string> redisCommands = new HashSet<string>
                {
                    "CLUSTER",
                    "PING", "ECHO", "CLIENT",
                    "SUBSCRIBE", "UNSUBSCRIBE", "NULL"
                };

                if (!GetServerInfo)
                {
                    redisCommands.Add("INFO");
                    redisCommands.Add("CONFIG");
                }
                config = new ConfigurationOptions
                {
                    SyncTimeout = int.MaxValue,
                    KeepAlive = 60,
                    Password = redisPass,
                    AbortOnConnectFail = true,
                    AllowAdmin = true,
                    CommandMap = CommandMap.Create(redisCommands, available: false)
                };
                if (redisIP != "")
                {
                    foreach (var rIP in redisIP.Split(','))
                    {
                        config.EndPoints.Add(rIP);
                    }
                }
            }
            if (redisIP != "")
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);
                try
                {
                    HttpContext.Current.Session["RedisIsConnected"] = redis.IsConnected;
                    if (!redis.IsConnected)
                    {
                        schemaNameKey = string.Empty;
                    }
                    else if (HttpContext.Current.Session != null && HttpContext.Current.Session["dbuser"] != null)
                    {
                        schemaNameKey = HttpContext.Current.Session["dbuser"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    if (redis != null)
                    {
                        HttpContext.Current.Session["RedisIsConnected"] = "false";
                        RedisClose(redis);
                    }
                }
                return redis;
            }
            else
            {
                HttpContext.Current.Session["RedisIsConnected"] = "false";
                return null;
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Session["RedisIsConnected"] = "false";
            schemaNameKey = string.Empty;
            logObj.CreateLog("Redis Server Constructor(RedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new"); return null;
        }

        if (schemaNameKey == string.Empty)
        {
            return null;
        }
    }

    private void RedisClose(ConnectionMultiplexer redis)
    {
        if (redis != null)
        {
            redis.Close(false);
            redis.Dispose();
        }
    }

    private string GetSessionId()
    {
        try
        {
            if (HttpContext.Current.Session != null)
            {
                return HttpContext.Current.Session.SessionID.ToString();
            }
            else
            {
                return Constants.GeneralLog;
            }
        }
        catch (Exception)
        {
            return Constants.GeneralLog;
        }
    }

    public TStructDef TstructDefFromRedis(string key)
    {
        TStructDef result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {
            if (redis == null)
                return result;

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                key = schemaNameKey + '-' + key;
                if (cacheClient.KeyExists(key))
                {
                    bytes = (byte[])cacheClient.StringGet(key);
                }
                if (bytes != null)
                {
                    using (var stream = new MemoryStream(bytes))
                    {
                        result = (TStructDef)new BinaryFormatter().Deserialize(stream);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(TstructDefFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public IviewData IviewObjFromRedis(string key)
    {
        IviewData result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                key = schemaNameKey + '-' + key;
                if (cacheClient.KeyExists(key))
                {
                    bytes = (byte[])cacheClient.StringGet(key);
                }
                if (bytes != null)
                {
                    using (var stream = new MemoryStream(bytes))
                    {
                        result = (IviewData)new BinaryFormatter().Deserialize(stream);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(TstructDefFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public DataTable DTasJsonFromRedis(string key)
    {
        DataTable result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {

                key = schemaNameKey + '-' + key;
                bytes = (byte[])cacheClient.StringGet(key);
            }
            if (bytes != null)
            {
                using (var stream = new MemoryStream(bytes))
                {
                    var str = (object)new BinaryFormatter().Deserialize(stream);
                    result = JsonConvert.DeserializeObject<DataTable>(str.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(ObjectJsonFromRedisDT), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }
    public DataTable DataTableFromRedis(string key)
    {
        DataTable result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {

            IDatabase cacheClient = redis.GetDatabase();

            if (redis.IsConnected)
            {
                key = schemaNameKey + '-' + key;
                bytes = (byte[])cacheClient.StringGet(key);
            }

            if (bytes != null)
            {
                using (var stream = new MemoryStream(bytes))
                {
                    var str = (object)new BinaryFormatter().Deserialize(stream);
                    result = JsonConvert.DeserializeObject<DataTable>(str.ToString());
                }
            }

        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(DataTableFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public string StringFromRedis(string key, string schemaName = "")
    {

        string result = string.Empty;
        if (schemaNameKey == string.Empty && schemaName == "")
        {
            return result;
        }
        if (key == string.Empty)
            return string.Empty;
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                key = schemaName == "" ? schemaNameKey + '-' + key : schemaName + '-' + key;
                bytes = (byte[])cacheClient.StringGet(key);

            }
            if (bytes != null)
            {
                using (var stream = new MemoryStream(bytes))
                {
                    result = (string)new BinaryFormatter().Deserialize(stream);
                    if (result == null) result = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(StringFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public object ObjectJsonFromRedis(string key, string schemaName = "")
    {
        object result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {

                // key = schemaNameKey + '-' + key;
                key = schemaName == "" ? schemaNameKey + '-' + key : schemaName + '-' + key;
                bytes = (byte[])cacheClient.StringGet(key);
            }
            if (bytes != null)
            {
                using (var stream = new MemoryStream(bytes))
                {
                    result = (object)new BinaryFormatter().Deserialize(stream);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(ObjectJsonFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public DataSet DataSetFromRedis(string key)
    {
        DataSet result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                key = schemaNameKey + '-' + key;
                bytes = (byte[])cacheClient.StringGet(key);
            }
            if (bytes != null)
            {
                using (var stream = new MemoryStream(bytes))
                {
                    result = (DataSet)new BinaryFormatter().Deserialize(stream);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(DataSetFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public DataSet DSasJsonFromRedis(string key)
    {
        DataSet result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {

                key = schemaNameKey + '-' + key;
                bytes = (byte[])cacheClient.StringGet(key);
            }
            if (bytes != null)
            {
                using (var stream = new MemoryStream(bytes))
                {
                    var str = (object)new BinaryFormatter().Deserialize(stream);
                    result = JsonConvert.DeserializeObject<DataSet>(str.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(ObjectJsonFromRedisDT), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
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

    public void GetFastDataRefEvents(TStructDef tstDef)
    {
        var memFastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
        if (memFastDataDef != null)
        {
            var ds = (DataSet)memFastDataDef;
            try
            {
                //Refresh Events = transid-savetask,actionname
                var refEvents = ds.Tables[Constants.FD_DT_DEFINITION].AsEnumerable().Where(s => s.Field<string>("RefreshEvents") != null).Select(r => new { RefreshEvents = r.Field<string>("RefreshEvents"), RefDataSet = r.Field<string>("Datasetname") }).ToList();
                for (int i = 0; i < refEvents.Count; i++)
                {
                    if (refEvents[i].RefreshEvents.StartsWith(tstDef.transId + "-"))
                    {
                        tstDef.fastDataRefDSName.Add(refEvents[i].RefDataSet);
                        tstDef.fastDataRefEvent.Add(refEvents[i].RefreshEvents);
                    }
                }
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception in GetFastDataRefEvents-" + ex.Message, HttpContext.Current.Session.SessionID, "GetFastDataRefEvents", "new");
            }
        }
    }

    private string GetFDDataSetName(string fieldName, string transid)
    {
        string dsName = string.Empty;

        var fastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
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
        var resJsonForAC = string.Empty;
        try
        {
            StringBuilder jsonForAC = new StringBuilder();
            string dsName = string.Empty;
            string filterStr = string.Empty;
            string dsDetails = GetFDDataSetName(fieldName, transid);
            if (dsDetails != string.Empty && dsDetails.IndexOf('`') != -1)
            {
                var strDsDtls = dsDetails.Split('`');
                if (strDsDtls.Length > 1)
                {
                    dsName = strDsDtls[0].ToString();
                    dsName = utilObj.GetFastDataDSName(null, dsName);
                    filterStr = strDsDtls[1].ToString();
                }
                var memFastDataDef = DTasJsonFromRedis(dsName);
                string fldMapping = GetFastDataFldMaps(fieldName, tstData.tstStrObj);
                var depFldMapping = fldMapping.Split('*');
                var strFldMap = depFldMapping[0].Split(',');

                if (memFastDataDef != null)
                {
                    var dt = (DataTable)memFastDataDef;
                    dt = GetFilteredDs(dt, filterStr, tstData, pageData, isSelectFld, fldValue, strFldMap.Last());
                    resJsonForAC = "{\"pickdata\":[{\"rcount\":\"" + rowCountAC + "\"},{\"fname\":\"" + fieldName + "\"},[";
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string mapColName = string.Empty; string mapidCol = string.Empty;
                            try
                            {
                                if (strFldMap.Length > 1)
                                {
                                    mapColName = strFldMap[1].ToString().Trim();
                                    mapidCol = strFldMap[0].ToString().Trim();
                                    jsonForAC.Append("{\"i\":\"" + dt.Rows[i][mapColName] + "\",\"v\":\"" + dt.Rows[i][mapidCol] + "\"");
                                }
                                else
                                {
                                    mapidCol = strFldMap[0].ToString().Trim();
                                    jsonForAC.Append("{\"i\":\"\",\"v\":\"" + dt.Rows[i][mapidCol] + "\"");
                                }

                                if (depFldMapping.Length > 1)
                                {
                                    var depMapping = depFldMapping[1].Split('^');
                                    string jsonForDep = string.Empty;
                                    jsonForDep = ",\"d\":\"";
                                    for (int j = 0; j < depMapping.Count(); j++)
                                    {
                                        var depMappFld = depMapping[j].Split('~');
                                        if (depMappFld.Length > 1)
                                            jsonForDep += depMappFld[0].Trim() + "~" + dt.Rows[i][depMappFld[1].Trim()] + "^";
                                        else
                                            jsonForDep += depMappFld[0].Trim() + "^";
                                    }
                                    jsonForDep = jsonForDep.ToString().Substring(0, jsonForDep.Length - 1);
                                    jsonForAC.Append(jsonForDep + "\"},");
                                }
                                else
                                    jsonForAC.Append("},");
                            }
                            catch
                            {
                                jsonForAC.Append("},");
                            }
                        }
                    }
                    resJsonForAC += jsonForAC.ToString().Substring(0, jsonForAC.Length - 1);
                    resJsonForAC += "]]}";
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("GetFromFastData -" + ex.Message, HttpContext.Current.Session.SessionID, "GetFromFastData", "");
        }
        return resJsonForAC;
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
        Array filterParams = filterString.Split('^').ToArray();
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
                    // For deleting the fields from the list.
                    if (tstData.tstStrObj.GetFieldIndex(fldMap) != -1)
                        paramValue = tstData.GetFieldValue(fldDc.ToString(), fldMap);
                    else
                    {
                        try
                        {
                            paramValue = HttpContext.Current.Session[fldMap].ToString();
                        }
                        catch (Exception ex)
                        { }
                    }
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

    public string GetFastAdvPicklist(string tid, string inputxml, string fldName, string sqlFldName, string tempSearch, string pageNo, string pageSize, bool isActiveFld)
    {
        int page = int.Parse(pageNo);
        int pageSiz = int.Parse(pageSize);
        int skipRows = 0;
        StringBuilder resultJson = new StringBuilder();
        var memFastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
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
                    dt = DTasJsonFromRedis(fastTableName.Fasttablename);
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
    private DataTable GetFilteredDs(DataTable dt, string filterString, TStructData tstData, string[] pageData, string isSelectFld, string fldValue, string FldMap)
    {
        dt = GetFilteredData(dt, filterString, tstData, "", "tstruct");
        try
        {
            if (!string.IsNullOrEmpty(fldValue))
            {
                //To filter the values with autocomplete entered text
                dt = dt.AsEnumerable().Where(x => x.Field<string>(dt.Columns[FldMap]).ToLower().Contains(fldValue.ToLower())).CopyToDataTable();
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

    public bool IsNullOrEmpty(String value)
    {
        return (value == null || value.Length == 0);
    }

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

    public string GetRedisServerFields(TStructDef tstDef)
    {
        var memFastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
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
        var memFastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
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
        var memFastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
        string fastDataScript = string.Empty;
        if (memFastDataDef != null)
        {
            if (IsFastFillGrid(tstDef, fillGridName, dc))
            {

                var fastTableName = memFastDataDef.Tables[Constants.FD_DT_ASSOCIATION].AsEnumerable().Where(s => s.Field<string>("FieldOrParamName") == fillGridName && s.Field<string>("Transid") == tstDef.transId).Select(r => new { Fasttablename = r.Field<string>("Fasttablename") }).First();
                DataTable dt = DTasJsonFromRedis(fastTableName.Fasttablename);
                fastXML = CustomDatatableToXML(dt);
            }

        }
        return fastXML;
    }

    public DataTable GetDashBoardData(string widgetIdB, string fltParams)
    {
        DataTable dbdt = new DataTable();
        try
        {
            var memFastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
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
                    dbdt = DTasJsonFromRedis(fastTableName.Fasttablename);
                    if (fltParams != "" && fastTableName.filterstring != null)
                        dbdt = GetFilteredData(dbdt, fastTableName.filterstring, null, fltParams, "dashboard");
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Get DashBoard Data -" + ex.Message, HttpContext.Current.Session.SessionID, "GetDashBoardData", "new");
        }
        return dbdt;
    }


    public ArrayList GetSelectFieldData(string fieldName, TStructDef tstDef)
    {
        ArrayList arrFldValues = new ArrayList();
        string fldValue = string.Empty;
        var memFastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
        string dsDetails = GetFDDataSetName(fieldName, tstDef.transId);
        if (dsDetails != string.Empty && dsDetails.IndexOf('`') != -1)
        {
            try
            {
                string dsName = string.Empty;
                string filterStr = string.Empty;
                var strDsDtls = dsDetails.Split('`');
                if (strDsDtls.Length > 1)
                {
                    dsName = strDsDtls[0].ToString().Trim();
                    filterStr = strDsDtls[1].ToString().Trim();
                }
                var dtFromFD = DTasJsonFromRedis(utilObj.GetFastDataDSName(memFastDataDef, dsName));
                var dt = (DataTable)dtFromFD;
                string fldMapping = GetFastDataFldMaps(fieldName, tstDef);
                //court_name_mastid,court_name*court_nameid~court_name_mastid^court_code~court_code^court_type_id~court_type_mastid^court_type~court_type^hierarchy_level~hierarchy_level^ctehsil_id~tehsil_name^csdo_nameid~sdo_name
                string[] strFldMap;
                if (fldMapping.Contains("*"))
                {
                    string[] strMapDtls = fldMapping.Split('*');
                    strFldMap = strMapDtls[0].Split(',');
                    string[] depFlds = strMapDtls[1].ToString().Split('^');
                    if (dt.Rows.Count > 0)
                    {
                        string mapColName = string.Empty; string mapidCol = string.Empty;
                        mapidCol = strFldMap[0].ToString().Trim();
                        if (strFldMap.Length > 1)
                        {
                            mapColName = strFldMap[1].ToString().Trim();
                            arrFldValues.Add(dt.Rows[0][mapidCol].ToString() + "~" + dt.Rows[0][mapColName].ToString());
                        }
                        else
                            arrFldValues.Add(dt.Rows[0][mapidCol].ToString());

                        for (int i = 0; i < depFlds.Length; i++)
                        {
                            string[] depDtls = depFlds[i].ToString().Split('~');
                            arrFldValues.Add(depDtls[0].ToString() + "♠" + dt.Rows[0][depDtls[1].ToString().Trim()].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        return arrFldValues;
    }

    public string GetAcceptFieldData(string fieldName, TStructDef tstDef)
    {
        string fldValue = string.Empty;
        string dsDetails = GetFDDataSetName(fieldName, tstDef.transId);
        if (dsDetails != string.Empty && dsDetails.IndexOf('`') != -1)
        {
            string dsName = string.Empty;
            string filterStr = string.Empty;
            var strDsDtls = dsDetails.Split('`');
            if (strDsDtls.Length > 1)
            {
                dsName = strDsDtls[0].ToString().Trim();
                dsName = utilObj.GetFastDataDSName(null, dsName);
                filterStr = strDsDtls[1].ToString().Trim();
            }
            var memFastDataDef = DTasJsonFromRedis(dsName);
            string fldMapping = GetFastDataFldMaps(fieldName, tstDef);

            string[] strFldMap = fldMapping.Split(',');
            if (memFastDataDef != null)
            {
                var dt = (DataTable)memFastDataDef;
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        string mapColName = string.Empty; string mapidCol = string.Empty;
                        mapidCol = strFldMap[0].ToString().Trim();
                        if (strFldMap.Length > 1)
                        {
                            mapColName = strFldMap[1].ToString().Trim();
                            fldValue = dt.Rows[0][mapidCol].ToString() + "~" + dt.Rows[0][mapColName].ToString();
                        }
                        else
                            fldValue = dt.Rows[0][mapidCol].ToString();
                    }
                    else
                        fldValue = "";
                }
                catch (Exception ex)
                { }
            }

        }
        return fldValue;
    }


    public string GetChecklistFieldData(string fieldName, TStructDef tstDef)
    {
        StringBuilder strJson = new StringBuilder();
        string fldValue = string.Empty;
        string idCol = string.Empty;
        var memFastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
        string dsDetails = GetFDDataSetName(fieldName, tstDef.transId);
        if (dsDetails != string.Empty && dsDetails.IndexOf('`') != -1)
        {
            try
            {
                string dsName = string.Empty;
                string filterStr = string.Empty;
                var strDsDtls = dsDetails.Split('`');
                if (strDsDtls.Length > 1)
                {
                    dsName = strDsDtls[0].ToString().Trim();
                    filterStr = strDsDtls[1].ToString().Trim();
                }
                var dtFromFD = DTasJsonFromRedis(utilObj.GetFastDataDSName(memFastDataDef, dsName));
                var dt = (DataTable)dtFromFD;
                string fldMapping = GetFastDataFldMaps(fieldName, tstDef);
                string[] strFldMap = fldMapping.Split(',');
                if (dt.Rows.Count > 0)
                {
                    string mapColName = string.Empty; string mapidCol = string.Empty;
                    mapidCol = strFldMap[0].ToString().Trim();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strFldMap.Length > 1)
                        {
                            mapColName = strFldMap[1].ToString().Trim();
                            idCol = "yes";
                            strJson.Append(",{\"v\":\"" + dt.Rows[i][mapidCol].ToString() + "~" + dt.Rows[i][mapColName].ToString() + "\",\"t\":\"dv\"}");
                        }
                        else
                        {
                            idCol = "no";
                            strJson.Append(",{\"v\":\"" + dt.Rows[i][mapidCol].ToString() + "\",\"t\":\"dv\"}");
                        }
                    }
                    if (!string.IsNullOrEmpty(idCol))
                        strJson.Append("♠" + idCol + "");
                }
            }
            catch (Exception ex)
            { }
        }
        return strJson.ToString();
    }

    public string GetInMemKeyValue(string transid, string role, string inMemKeys)
    {
        string key = transid + "-" + role;
        try
        {
            string[] customKeys = inMemKeys.Split(',');
            for (int i = 0; i < customKeys.Length; i++)
            {
                string keyName = customKeys[i].ToString().Trim();
                if (HttpContext.Current.Session[keyName] != null)
                    key += HttpContext.Current.Session[keyName].ToString().Trim().Replace(" ", "");
            }
        }
        catch (Exception Ex)
        {
        }
        return key;
    }

    #region FastDataUtility
    public ArrayList GetAllKeys(string pattern, bool SchemaContains = true)
    {

        ArrayList list = new ArrayList();
        var redis = RedisConnect();
        if (pattern != "*")
        {
            pattern = "*" + pattern + "*";
        }
        try
        {
            if (redis != null && redis.IsConnected)
            {
                var endpoints = redis.GetEndPoints(true);
                foreach (var endpoint in endpoints)
                {
                    var server = redis.GetServer(endpoint);
                    if (server.IsConnected)
                    {
                        foreach (string key in server.Keys(pattern: pattern))
                        {
                            if (SchemaContains && key.Contains(schemaNameKey))
                                list.Add(key);
                            else if (!SchemaContains)
                                list.Add(key);
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(GetAllKeys), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return list;
    }


    public Dictionary<string, string[]> GetKeysForForms(ArrayList forms, string pattern, string proj, bool generalKeysRequired, ref bool redisConnected)
    {
        ArrayList list = new ArrayList();
        redisConnected = false;
        Dictionary<string, string[]> selcKeys = new Dictionary<string, string[]>();
        var redis = RedisConnect();
        try
        {
            if (redis != null && redis.IsConnected)
            {
                redisConnected = true;
                var endpoints = redis.GetEndPoints(true);
                foreach (var endpoint in endpoints)
                {
                    var server = redis.GetServer(endpoint);
                    if (server.IsConnected)
                    {
                        foreach (string key in server.Keys(pattern: pattern))
                        {
                            if (key.Contains(schemaNameKey)) //key exists in Redis
                            {
                                foreach (var item in forms)
                                {
                                    if (key.IndexOf(proj + "-" + item) != -1) //if pattern exists with 'projname-formname'
                                    {
                                        if (selcKeys.ContainsKey(item.ToString())) //key already selected
                                        {
                                            var curVal = selcKeys[item.ToString()];
                                            string keyCount = curVal[0].ToString(); //key count for specific form
                                            string keyPattern = curVal[1].ToString();//redis key
                                            selcKeys[item.ToString()] = new string[2] { (int.Parse(keyCount) + 1).ToString(), keyPattern + "&" + key };
                                        }
                                        else
                                            selcKeys[item.ToString()] = new string[2] { "1", key }; //new key then select key and add key count to 1
                                    }
                                    else
                                    {
                                        //other than iview, tstruct are general keys
                                        if (generalKeysRequired && key.IndexOf("localdemo-inmemdb-") != 0) //append general keys only if required(in iview parameter selection has All/General)
                                            selcKeys[key] = new string[2] { "1", key };
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
                redisConnected = false;
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Iview - Redis Server Function(GetAllKeys), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return selcKeys;
    }


    public ArrayList GetPrefixedKeys(string strPrefix, bool prefixWithSchema = false, string suffix = "", bool secondCheck = true)
    {
        string keyForPattern = string.Empty;
        if (prefixWithSchema)
        {
            keyForPattern = schemaNameKey + '-';
        }
        ArrayList list = new ArrayList();
        var redis = RedisConnect();
        try
        {
            if (redis != null && redis.IsConnected)
            {
                var endpoints = redis.GetEndPoints(true);
                foreach (var endpoint in endpoints)
                {
                    var server = redis.GetServer(endpoint);

                    if (server.IsConnected)
                    {
                        if (strPrefix == "General")
                        {
                            foreach (string key in server.Keys(pattern: keyForPattern + strPrefix + "-*" + suffix))
                            {
                                if (key.Contains(keyForPattern + "General" + "-") || !secondCheck)
                                    list.Add(key);
                            }
                        }
                        else if (keyForPattern != "")
                        {
                            foreach (string key in server.Keys(pattern: keyForPattern + strPrefix + "-*" + suffix))
                            {
                                if (key.Contains(keyForPattern + strPrefix) || !secondCheck)
                                    list.Add(key);

                            }
                        }
                        else
                        {
                            foreach (string key in server.Keys(pattern: "*-" + strPrefix + "-*" + suffix))
                            {
                                if (key.Contains(keyForPattern + strPrefix) || !secondCheck)
                                    list.Add(key);
                            }
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(GetPrefixedKeys), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return list;
    }

    public bool FlushAllRedisKeys()
    {
        bool flush = false;
        ArrayList list = new ArrayList();
        var redis = RedisConnect();
        try
        {
            if (redis.IsConnected)
            {
                var endpoints = redis.GetEndPoints(true);
                IDatabase cacheClient = redis.GetDatabase();
                foreach (var endpoint in endpoints)
                {
                    var server = redis.GetServer(endpoint);
                    string dbType = "", schemaTitle = "";
                    if (HttpContext.Current.Session["axdb"] != null && HttpContext.Current.Session["project"] != null)
                    {
                        dbType = HttpContext.Current.Session["axdb"].ToString().ToLower().Replace(" ", "").Substring(0, 3);
                        schemaTitle = HttpContext.Current.Session["project"].ToString();
                    }
                    var formKeys = server.Keys(pattern: schemaNameKey + "-*");
                    var widgetKeys = server.Keys(pattern: dbType + "~" + schemaTitle + "~*");
                    var serverKeys = formKeys.Concat(widgetKeys);
                    foreach (string key in serverKeys)
                    {
                        if (key.Contains(schemaNameKey) && cacheClient.KeyExists(key))
                            flush = cacheClient.KeyDelete(key);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(FlushAllRedisKeys), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return flush;
    }


    public ArrayList GetMemoryDetails()
    {
        ArrayList serverinfo = new ArrayList();
        var redis = RedisConnect(true);
        try
        {
            if (redis != null && redis.IsConnected)
            {
                var endpoints = redis.GetEndPoints(true);
                var server = redis.GetServer(endpoints[0]);
                string abc = server.InfoRaw("Memory");
                string[] abc2 = Regex.Split(abc, "\r\n");


                foreach (var configredis in server.ConfigGet())
                {
                    if (configredis.Key == "maxmemory")
                    {
                        serverinfo.Add(configredis.Key + ":" + configredis.Value);
                        break;
                    }
                }
                serverinfo.Add(abc2[1]);
            }
            else
                logObj.CreateLog("Redis Server Function(GetMemoryDetails), Message:" + "Unable to connect to Redis Server", GetSessionId(), "RedisServer", "new");
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(GetMemoryDetails), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return serverinfo;
    }

    #endregion




    public TStructData TstructDataFromRedis(string key, bool IsSchemaNameExist = false)
    {
        TStructData result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                key = IsSchemaNameExist ? key : schemaNameKey + '-' + key;
                if (cacheClient.KeyExists(key))
                {
                    bytes = (byte[])cacheClient.StringGet(key);
                }
                if (bytes != null)
                {
                    using (var stream = new MemoryStream(bytes))
                    {
                        result = (TStructData)new BinaryFormatter().Deserialize(stream);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(TstructDefFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }


    #region FDR New

    private string GetAxRoleReader(string AxRole)
    {
        string axroles = string.Empty;
        try
        {
            var LstRoles = AxRole.Split(',').OrderBy(x => x).ToList();
            axroles = string.Join(",", LstRoles);
            HttpContext.Current.Session["AxRoleReader"] = axroles;
            return axroles;
        }
        catch (Exception ex)
        {
            HttpContext.Current.Session["AxRoleReader"] = AxRole;
            return AxRole;
        }
    }

    public string MakeKeyName(string type, string pageId, string user = "ALL", string fldName = "", int fldIndex = 0)
    {
        string AxRole = string.Empty;
        string lang = string.Empty;
        string key = string.Empty;
        if (HttpContext.Current.Session != null)
        {
            if (HttpContext.Current.Session["AxRoleReader"] != null)
                AxRole = HttpContext.Current.Session["AxRoleReader"].ToString();
            else if (HttpContext.Current.Session["AxRole"] != null)
            {
                AxRole = HttpContext.Current.Session["AxRole"].ToString();
                AxRole = GetAxRoleReader(AxRole);
            }

            if (HttpContext.Current.Session["language"] != null)
                lang = HttpContext.Current.Session["language"].ToString().Substring(0, 3);
        }
        switch (type)
        {
            case Constants.REDISMENUDATA:
                key = "General-MenuData-" + AxRole + '-' + lang;
                break;
            case Constants.REDISTSTRUCT:
                key = pageId + '-' + AxRole + '-' + lang;
                break;
            case Constants.REDISCARDSDATA:
                key = "General-CardsData-" + pageId + '-' + lang;
                break;
            case Constants.REDISCARDKEYS:
            case Constants.REDISCARDPARAMS:
            case Constants.REDISCARDROLES:
            case Constants.REDISAPIKEYS:
                key = "General-" + type + "-" + pageId + '-' + lang;
                break;
            case Constants.REDISTSTRUCTMOB:
                key = pageId + "-Mob-" + AxRole + '-' + lang;
                break;
            case Constants.REDISTSTRUCTTABLE:
                key = pageId + "-DcTable" + '-' + AxRole + '-' + lang;
                break;
            case Constants.REDISTSTRUCTDOFORM:
                key = pageId + "-DoForm" + '-' + AxRole + '-' + lang;
                break;
            case Constants.REDISTSTRUCTDESIGN:
                key = pageId + "-dsgn" + '-' + AxRole + '-' + lang;
                break;
            case Constants.REDISTSTRUCTAXCUSTHTML:
                key = pageId + "-axTstCustomHtml" + '-' + lang;
                break;
            case Constants.REDISTSTRUCTAXDESIGN:
                key = pageId + "-axDesign" + '-' + lang;
                break;
            case Constants.REDISTSTRUCTDTLS:
                key = pageId + "-TSTDTLS" + '-' + AxRole + '-' + lang;
                break;
            case Constants.REDISTSTRUCTDTLSMOB:
                key = pageId + "-TSTDTLSMOB" + '-' + AxRole + '-' + lang;
                break;
            case Constants.REDISTSTRUCTDESIGNDTLS:
                key = pageId + "-dsgnTSTDTLS" + '-' + AxRole + '-' + lang;
                break;
            case Constants.IVIEWSTRUCT:
                key = pageId + "-StructXml-" + AxRole + '-' + lang;
                break;
            case Constants.IVIEWPARAM:
                key = pageId + "-getparam-" + AxRole + '-' + lang;
                break;
            case Constants.AXCONFIGURATIONS:
                key = pageId + "-axconfigstruct-" + AxRole + '-' + lang;
                break;
            case Constants.AXCONFIGURATIONTABLE:
                key = pageId + "-axpConfigTable-" + AxRole + '-' + lang;
                break;
            case Constants.AXOLDDESIGN:
                key = pageId + "-axOldDesign-" + lang;
                break;
            case Constants.AXPAGETITLE:
                key = "axpagetitle-" + lang;
                break;
            case Constants.AXHYBRIDINFO:
                key = pageId + "-axgetHybridInfo";
                break;

            case Constants.RedisIviewObjList:
                key = pageId + "-ivList-" + AxRole + '-' + lang;
                break;
            case Constants.RedisIviewObj:
                key = pageId + "-iv-" + fldIndex + "-" + AxRole + '-' + lang;
                break;
            case Constants.RedisListviewObjList:
                key = pageId + "-lvList-" + AxRole + '-' + lang;
                break;
            case Constants.RedisListviewObj:
                key = pageId + "-lv-" + fldIndex + "-" + AxRole + '-' + lang;
                break;
            case Constants.RedisIvData:
            case Constants.RedisLvData:
                key = pageId + "-" + type + "-" + user + "-" + lang + "-" + fldName + "-pageNo$" + (fldIndex == -1 ? "" : "-" + fldIndex.ToString());
                break;
            case Constants.RedisIviewSettings:
                key = pageId + "-ivSettings-" + AxRole + '-' + user + '-' + lang;
                break;
            case Constants.RedisListviewSettings:
                key = pageId + "-lvSettings-" + AxRole + '-' + user + '-' + lang;
                break;
            case Constants.RedisOldIviewSettings:
                key = "ivSettings-" + AxRole + '-' + user + '-' + lang;
                break;
            case Constants.RedisOldListviewSettings:
                key = "lvSettings-" + AxRole + '-' + user + '-' + lang;
                break;
            case Constants.RedisOldIviewTemplates:
                key = pageId + "-ivTemplates-" + AxRole + '-' + user + '-' + lang;
                break;
            case Constants.RedisOldListviewTemplates:
                key = pageId + "-lvTemplates-" + AxRole + '-' + user + '-' + lang;
                break;
            case Constants.REDISLVRECORDLISTING:
                key = "lvRecordListing-" + user;
                break;
            case Constants.ListViewFieldsInfo:
                key = pageId + "-LstFldInfo" + '-' + AxRole + '-' + lang;
                break;

            case Constants.DEPFLDARRAY:
                key = pageId + '-' + fldName + "-array-" + AxRole + '-' + lang;
                break;
            case Constants.FIELDNAME:
                key = pageId + '-' + fldName + "-" + AxRole + '-' + lang;
                break;
            case Constants.FIELDINDEX:
                key = pageId + '-' + fldName + "-" + fldIndex + "-" + AxRole + '-' + lang;
                break;
            case Constants.FORMLOADARRAY:
                key = pageId + "-FormLoadArray-" + lang;
                break;
            case Constants.FORMLOADINDEX:
                key = pageId + "-FormLoadIndex-" + fldIndex + "-" + lang;
                break;

            case Constants.FORMLOADRES:
                key = pageId + "-FormLoadRes-" + lang;
                break;

            case Constants.REDISHYBRIDINFO:
                key = pageId + "-axmobilehybridinfo";
                break;

            case Constants.REDISKEEPWEBINFO:
                key = pageId + "-keepaliveweb";
                break;

            case Constants.REDISAXAUTOINTELLI:
                key = pageId + "-AxAutoIntelli";
                break;
            case Constants.REDISCARDSDATADASHBOARD:
                key = "General-DashBoardCardsData-" + pageId + '-' + lang;
                break;
            case Constants.REDISCARDKEYSDASHBOARD:
            case Constants.REDISCARDPARAMSDASHBOARD:
            case Constants.REDISCARDROLESDASHBOARD:
                key = "General-" + type + "-" + pageId + '-' + lang;
                break;
            case Constants.REDISHOMEPAGECARDS:
                key = "General-" + type + '-' + lang;
                break;
            case Constants.REDISTSTHTMLLS:
                key = pageId + "-tstHtmlLs-" + AxRole + '-' + lang;
                break;
            case Constants.REDISTSTHTMLLSMob:
                key = pageId + "-tstHtmlLsMob-" + AxRole + '-' + lang;
                break;
            default:
                key = "General-" + AxRole + '-' + lang;
                break;
        }
        return key;
    }

    public static List<string> GetGlobalVars()
    {
        List<string> gblList = new List<string>();
        if (HttpContext.Current.Session["axGlobalVars"] != null)
        {
            XmlDocument xmlDocgbl = new XmlDocument();
            xmlDocgbl.LoadXml(HttpContext.Current.Session["axGlobalVars"].ToString());
            foreach (XmlNode parms in xmlDocgbl.ChildNodes[0].ChildNodes)
            {
                if (parms.Name.ToLower() != "responsibilies" && parms.Name.ToLower() != "rolename" && parms.Name.ToLower() != "sesid" && parms.Name.ToLower() != "usergroup" && parms.Name.ToLower() != "project" && parms.Name.ToLower() != "groupno" && parms.Name.ToLower() != "userroles" && parms.Name.ToLower() != "pageaccess" && parms.Name.ToLower() != "transidlist" && parms.Name.ToLower() != "appvartypes")
                    gblList.Add(parms.Name + ":" + parms.InnerText);
            }
        }
        return gblList;
    }

    public int MakeVarKeyName(List<string> keyList)
    {
        int keyIndex = -1;
        var globalVars = GetGlobalVars();
        if (globalVars.Count > 0)
        {
            for (int i = 0; i < keyList.Count(); i++)
            {
                bool isKeyBound = true;
                var loopKey = keyList[i].Split('♦');
                if (loopKey[0] == "")
                {
                    keyIndex = keyList.FindIndex(w => w == keyList[i]);
                    break;
                }
                else
                {
                    for (int j = 0; j < loopKey.Count() - 1; j++)
                    {
                        var filetKeys = globalVars.AsEnumerable().Where(x => x.ToLower() == loopKey[j].ToLower()).ToList();
                        if (filetKeys.Count == 0 && loopKey[j] != "")
                        {
                            isKeyBound = false;
                            break;
                        }
                    }
                    if (isKeyBound == true)
                    {
                        keyIndex = keyList.FindIndex(w => w == keyList[i]);
                        break;
                    }
                }
            }
        }
        else
            keyIndex = 0;
        return keyIndex;
    }

    public int GetKeyIndex(string ThisKey)
    {
        int keyIndex = -1;
        var redis = RedisConnect();
        try
        {
            string schemaName = string.Empty;
            if (schemaNameKey == string.Empty)
            {
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            else if (schemaNameKey == string.Empty && schemaName == string.Empty)
                return keyIndex = -3;

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                ThisKey = schemaName == "" ? schemaNameKey + '-' + ThisKey : schemaName + '-' + ThisKey;
                if (cacheClient.KeyExists(ThisKey))
                    keyIndex = 0;
            }
            else
                keyIndex = -2;// Redis not connected 
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(GetKeyIndex), Message:" + ex.Message, GetSessionId(), "GetKeyIndex", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return keyIndex;
    }

    public string ReadKey(string ThisKey)
    {
        string result = string.Empty;
        var redis = RedisConnect();
        try
        {
            byte[] bytes = null;
            string schemaName = string.Empty;
            if (schemaNameKey == string.Empty)
            {
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            else if (schemaNameKey == string.Empty && schemaName == string.Empty)
                return result = "schemaNamecannotbenull";
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                ThisKey = schemaName == "" ? schemaNameKey + '-' + ThisKey : schemaName + '-' + ThisKey;
                bytes = (byte[])cacheClient.StringGet(ThisKey);
                if (bytes != null)
                {
                    using (var stream = new MemoryStream(bytes))
                    {
                        result = (string)new BinaryFormatter().Deserialize(stream);
                        if (result == null) result = string.Empty;
                    }
                }
            }
            else
                result = "redisnotconnected";// Redis not connected 
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(GetKeyIndex), Message:" + ex.Message, GetSessionId(), "GetKeyIndex", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public ArrayList GetWildCardKeyNames(string prefixKey)
    {
        ArrayList list = new ArrayList();
        if (prefixKey != "*")
        {
            prefixKey = prefixKey + "*";
        }
        var redis = RedisConnect();
        try
        {
            string schemaName = string.Empty;
            if (schemaNameKey == string.Empty)
            {
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            else if (schemaNameKey == string.Empty && schemaName == string.Empty)
                return list;
            if (redis != null && redis.IsConnected)
            {
                var endpoints = redis.GetEndPoints(true);
                foreach (var endpoint in endpoints)
                {
                    var server = redis.GetServer(endpoint);
                    if (server.IsConnected)
                    {
                        prefixKey = schemaName == "" ? schemaNameKey + '-' + prefixKey : schemaName + '-' + prefixKey;
                        foreach (string key in server.Keys(pattern: prefixKey))
                        {
                            list.Add(key);
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(GetWildCardKeyNames), Message:" + ex.Message, GetSessionId(), "GetWildCardKeyNames", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return list;
    }

    public string ReadStringKey(string ThisKey)
    {
        string result = string.Empty;
        var redis = RedisConnect();
        try
        {
            string schemaName = string.Empty;
            if (schemaNameKey == string.Empty)
            {
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            else if (schemaNameKey == string.Empty && schemaName == string.Empty)
                return result = "schemaNamecannotbenull";

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                ThisKey = schemaName == "" ? schemaNameKey + '-' + ThisKey : schemaName + '-' + ThisKey;
                result = cacheClient.StringGet(ThisKey);
            }
            else
                result = "redisnotconnected";// Redis not connected 
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(ReadStringKey), Message:" + ex.Message, GetSessionId(), "ReadStringKey", "new");
        }
        finally
        {
            RedisClose(redis);
        }

        return result;
    }

    public string ReadStringKeywithSchema(string ThisKey, string schemaName = "")
    {
        string result = string.Empty;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        var redis = RedisConnect();
        try
        {
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                ThisKey = schemaName == "" ? schemaNameKey + '-' + ThisKey : schemaName + '-' + ThisKey;
                result = cacheClient.StringGet(ThisKey);
            }
            else
                result = "redisnotconnected";// Redis not connected 
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(ReadStringKey), Message:" + ex.Message, GetSessionId(), "ReadStringKey", "new");
        }
        finally
        {
            RedisClose(redis);
        }

        return result;
    }

    public string ReadKeyNoSchema(string ThisKey)
    {
        string result = string.Empty;
        var redis = RedisConnect();
        try
        {
            byte[] bytes = null;
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                bytes = (byte[])cacheClient.StringGet(ThisKey);
                if (bytes != null)
                {
                    using (var stream = new MemoryStream(bytes))
                    {
                        result = (string)new BinaryFormatter().Deserialize(stream);
                        if (result == null) result = string.Empty;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(ReadKeyNoSchema), Message:" + ex.Message, GetSessionId(), "ReadKeyNoSchema", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public string ReadKeyStringValueNoSchema(string ThisKey)
    {
        string result = string.Empty;
        var redis = RedisConnect();
        try
        {
            byte[] bytes = null;
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                result = cacheClient.StringGet(ThisKey);
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(ReadKeyNoSchema), Message:" + ex.Message, GetSessionId(), "ReadKeyNoSchema", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }
    #endregion

    public string HashGetKey(string ThisKey, string ThisKeyParam)
    {
        string result = string.Empty;
        var redis = RedisConnect();
        try
        {
            byte[] bytes = null;
            string schemaName = string.Empty;
            if (schemaNameKey == string.Empty)
            {
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            else if (schemaNameKey == string.Empty && schemaName == string.Empty)
                return result = "schemaNamecannotbenull";
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                ThisKey = schemaName == "" ? schemaNameKey + '-' + ThisKey : schemaName + '-' + ThisKey;
                bytes = (byte[])cacheClient.HashGet(ThisKey, ThisKeyParam);
                if (bytes != null)
                {
                    using (var stream = new MemoryStream(bytes))
                    {
                        result = (string)new BinaryFormatter().Deserialize(stream);
                        if (result == null) result = string.Empty;
                    }
                }
            }
            else
                result = "redisnotconnected";// Redis not connected 
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(HashGetKey), Message:" + ex.Message, GetSessionId(), "HashGetKey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public IviewParams HashGetParamObjFromRedis(string key, string subKey)
    {
        IviewParams result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                key = schemaNameKey + '-' + key;
                if (cacheClient.KeyExists(key))
                {
                    bytes = (byte[])cacheClient.HashGet(key, subKey);
                }
                if (bytes != null)
                {
                    using (var stream = new MemoryStream(bytes))
                    {
                        result = (IviewParams)new BinaryFormatter().Deserialize(stream);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(TstructDefFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }
}
