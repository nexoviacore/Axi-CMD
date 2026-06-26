using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using StackExchange.Redis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using Newtonsoft.Json;

/// <summary>
/// Summary description for FDW
/// </summary>
[Serializable()]
public sealed class FDW
{
    LogFile.Log logObj = new LogFile.Log();
    Util.Util utilObj = new Util.Util();
    public string schemaNameKey = string.Empty;
    public string db_type = String.Empty;
    public string db_connection = String.Empty;
    [NonSerialized]
    private ConfigurationOptions config;
    string redisIP = string.Empty;
    string redisPass = string.Empty;
    public string redisIpLocalHost = string.Empty;

    public FDW(string SelectedProj = "")
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

    public void RedisClose(ConnectionMultiplexer redis)
    {
        if (redis != null)
        {
            redis.Close(false);
            redis.Dispose();
        }
    }

    public void Initialize(string strProj)
    {
        var redis = RedisConnect();
        try
        {
            if (!IsNullOrEmpty(strProj) && (redis != null ? redis.IsConnected == true : false))
            {
                if (strProj != string.Empty)
                    schemaNameKey = strProj;
                else
                {
                    string contents = "";
                    if (HttpContext.Current.Application["axApps"] != null)
                        contents = HttpContext.Current.Application["axApps"].ToString();
                    else if (HttpContext.Current.Session["axApps"] != null)
                        contents = HttpContext.Current.Session["axApps"].ToString();
                    HttpContext.Current.Application["ProjUser"] = strProj;
                    GetDBConnection(strProj, contents);
                }
            }
        }
        catch (Exception ex) { }
        finally
        {
            RedisClose(redis);
        }

    }

    public bool SaveInRedisServerDT(string key, object value, string type, string schemaName = "")
    {
        bool added = false;
        string jsonstr = JsonConvert.SerializeObject(value);
        if (!string.IsNullOrEmpty(jsonstr))
        {
            byte[] bytes;
            var redis = RedisConnect();
            try
            {
                using (var stream = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(stream, jsonstr);
                    bytes = stream.ToArray();
                }
                IDatabase cacheClient = redis.GetDatabase();
                lock (cacheClient)
                {
                    if (redis.IsConnected)
                    {
                        key = schemaName == "" ? schemaNameKey + '-' + key : schemaName + '-' + key;
                        added = cacheClient.StringSet(key, bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Redis Server Functon(SaveInRedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
            }
            finally
            {
                RedisClose(redis);
            }
        }
        return added;
    }

    public bool SaveInRedisServer(string key, object value, string type, string schemaName = "")
    {
        bool added = false;
        byte[] bytes;
        var redis = RedisConnect();
        try
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, value);
                bytes = stream.ToArray();
            }
            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    key = schemaName == "" ? schemaNameKey + '-' + key : schemaName + '-' + key;
                    added = cacheClient.StringSet(key, bytes);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(SaveInRedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public bool ClearRedisServerDataByKey(string key, string type, bool schemaexist, string schemaName = "")
    {
        bool removed = false;
        if (schemaNameKey == string.Empty)
            return removed;
        var redis = RedisConnect();
        try
        {
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                string keyname = string.Empty;
                if (!schemaexist)
                    keyname = schemaName == "" ? schemaNameKey + '-' + key : schemaName + '-' + key;
                else
                    keyname = key;

                string result = string.Empty;
                if (cacheClient.KeyExists(keyname))
                    removed = cacheClient.KeyDelete(keyname);
            }
        }
        catch (Exception ex)
        {
            removed = false;
            logObj.CreateLog("Redis Server Functon(ClearRedisServerDataByKey), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return removed;
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

    public DataSet DSasJsonFromRedis(string key, string schemaName = "")
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
                key = schemaName == "" ? schemaNameKey + '-' + key : schemaName + '-' + key;
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
            logObj.CreateLog("Redis Server Functon(ObjectJsonFromRedisDT), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
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
            return result;

        byte[] bytes = null;
        var redis = RedisConnect();
        try
        {
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                key = schemaNameKey + '-' + key;
                bytes = cacheClient.StringGet(key);
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
            logObj.CreateLog("Redis Server Functon(DataSetFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
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

    public void PushToDsRefresh(string KeyName)
    {
        //lock (dsRefresh)
        //{
        //    int idx = dsRefresh.IndexOf(KeyName);
        //    if (idx == -1)
        //        dsRefresh.Add(KeyName);
        //}
    }

    public void PopInDsRefresh(string KeyName)
    {
        //lock (dsRefresh)
        //{
        //    int idx = dsRefresh.IndexOf(KeyName);
        //    dsRefresh.RemoveAt(idx);
        //}
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
                    result = (DataTable)new BinaryFormatter().Deserialize(stream);
                }
            }

        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(DataTableFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    public void LogFDWDetails()
    {
        //string logStr = string.Empty;
        //logStr = "No of connections made in FDW-" + FDWConCount + "----- No of Definitions created in redis-" + FDWCreateDef
        //    + "----- No of times Refresh DS is called-" + FDWRefresh;
        //logObj.CreateLog(logStr, GetSessionId(), "LogFDW", "new", "true");
    }

    public bool PushPopInRedisServer(string key, object value, string type)
    {
        bool added = false;
        if (schemaNameKey == string.Empty)
        {
            return added;
        }
        byte[] bytes;
        var redis = RedisConnect();
        try
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, value);
                bytes = stream.ToArray();
            }

            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    key = schemaNameKey + '-' + key;
                    added = cacheClient.StringSet(key, bytes);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(SaveInRedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
            //throw ex;
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public void RefreshFDDataSet(string dsName)
    {
        //logObj.CreateLog("Refresh Started----------In RefreshFDDataSet, Currently Refreshing-" + dsName, GetSessionId(), "LogThreadPath", "", "true");
        var memFastDataDef = DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
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
                SaveInRedisServerDT(dsName, dsData, "");
                //logObj.CreateLog("Refresh Completed----------In RefreshFDDataSet, Currently Refreshing-" + dsName, GetSessionId(), "LogThreadPath", "", "true");
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception in RefreshFastDataset-" + ex.Message, GetSessionId(), "RefreshFastDS", "new", "true");
            }
        }
    }

    public DataSet GetDefinition(string schemaName = "")
    {
        DataSet DfDataset = new DataSet();
        try
        {
            Ihelper objHelper = new Helper().SetDatabase(db_type, db_connection);
            DfDataset.Tables.Add(Constants.FD_DT_DEFINITION);
            DfDataset.Tables[Constants.FD_DT_DEFINITION].Merge(objHelper.ExecuteDataSetSqlInline(Constants.FD_TBL_DEFINITION).Tables[0]);
            DfDataset.Tables.Add(Constants.FD_DT_ASSOCIATION);
            DfDataset.Tables[Constants.FD_DT_ASSOCIATION].Merge(objHelper.ExecuteDataSetSqlInline(Constants.FD_TBL_ASSOCIATION).Tables[0]);
            SaveInRedisServerDT(Constants.RS_FD_DEFINITION, DfDataset, "", schemaName);
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

    public void GetAxRelations(string schemaName = "")
    {
        DataTable arDatatable = new DataTable();
        try
        {
            Ihelper objHelper = new Helper().SetDatabase(db_type, db_connection);
            arDatatable = objHelper.ExecuteDataSetSqlInline(Constants.TBL_AXRELATIONS).Tables[0];
            SaveInRedisServer(Constants.RS_AXRELATIONS, arDatatable, "", schemaName);
        }
        catch (Exception ex)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                logObj.CreateLog("Exception in GetAxRelations-" + ex.Message, HttpContext.Current.Session.SessionID, "GetAxRelations", "new");
            else
                logObj.CreateLog("Exception in GetAxRelations-" + ex.Message, "AxRelations data on application start", "GetAxRelations", "new");
        }
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
        Util.Util util = new Util.Util();
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
        if (dbtype.ToLower() == "postgresql" || db_type.ToLower() == "postgre")
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
        schemaNameKey = dbuser;
    }

    public bool IsNullOrEmpty(String value)
    {
        return (value == null || value.Length == 0);
    }

    #region FastDataUtility
    public bool DeleteAllKeys(string key, string schemaNameKey = "")
    {
        bool removed = false;
        if (schemaNameKey == string.Empty)
        {
            return removed;
        }
        var redis = RedisConnect();
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                string[] multikeys = key.Split('&');
                for (int i = 0; i < multikeys.Length; i++)
                {
                    if (cacheClient.KeyExists(multikeys[i]))
                    {
                        removed = cacheClient.KeyDelete(multikeys[i]);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(DeleteKey), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return removed;
    }
    #endregion

    public bool FlushRedisKeysByRoles(ArrayList roles, ArrayList ignoreList = null)
    {
        bool flush = false;
        var redis = RedisConnect();

        if (ignoreList == null)
        {
            ignoreList = new ArrayList();
        }

        try
        {
            if (redis.IsConnected)
            {
                var endpoints = redis.GetEndPoints(true);
                IDatabase cacheClient = redis.GetDatabase();
                foreach (var endpoint in endpoints)
                {
                    var server = redis.GetServer(endpoint);
                    foreach (var role in roles)
                    {
                        var roleKeys = server.Keys(pattern: "*-" + role + "-*");
                        var defaultRoleKeys = server.Keys(pattern: role + "-*");
                        var serverKeys = roleKeys.Concat(defaultRoleKeys);
                        foreach (string key in serverKeys)
                        {
                            if (key.Contains(schemaNameKey) && cacheClient.KeyExists(key) && ignoreList.IndexOf(key) < 0)
                                flush = cacheClient.KeyDelete(key);
                        }
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


    #region New code
    public bool WriteKey(string KeyName, string KeyValue)
    {
        bool added = false;
        byte[] bytes;
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
                return added = false;

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, KeyValue);
                bytes = stream.ToArray();
            }
            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
                    added = cacheClient.StringSet(KeyName, bytes);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(WriteKey), Message:" + ex.Message, GetSessionId(), "WriteKey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public bool WriteKeyAutoExpire(string KeyName, string KeyValue, int expirtime)
    {
        bool added = false;
        byte[] bytes;
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
                return added = false;

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, KeyValue);
                bytes = stream.ToArray();
            }
            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
                    added = cacheClient.StringSet(KeyName, bytes, new TimeSpan(0, expirtime, 0));
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(WriteKey), Message:" + ex.Message, GetSessionId(), "WriteKey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public bool WriteStringKey(string KeyName, string KeyValue)
    {
        bool added = false;
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
                return added = false;

            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
                    added = cacheClient.StringSet(KeyName, KeyValue);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(WriteStringKey), Message:" + ex.Message, GetSessionId(), "WriteStringKey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public bool Deletekey(string KeyName)
    {
        bool removed = false;
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
                return removed = false;

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
                if (cacheClient.KeyExists(KeyName))
                    removed = cacheClient.KeyDelete(KeyName);
            }
        }
        catch (Exception ex)
        {
            removed = false;
            logObj.CreateLog("Redis Server Functon(Deletekey), Message:" + ex.Message, GetSessionId(), "Deletekey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return removed;
    }

    public bool DeleteKeys(ArrayList KeyNameWithWildCard)
    {
        bool removed = false;
        var redis = RedisConnect();
        try
        {
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                for (int i = 0; i < KeyNameWithWildCard.Count; i++)
                {
                    if (cacheClient.KeyExists(KeyNameWithWildCard[i].ToString()))
                    {
                        removed = cacheClient.KeyDelete(KeyNameWithWildCard[i].ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(DeleteKeys), Message:" + ex.Message, GetSessionId(), "DeleteKeys", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return removed;

    }

    public bool WriteKeyNoSchema(string KeyName, string KeyValue, int expirtime)
    {
        bool added = false;
        byte[] bytes;
        var redis = RedisConnect();
        try
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, KeyValue);
                bytes = stream.ToArray();
            }
            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    added = cacheClient.StringSet(KeyName, bytes, new TimeSpan(0, expirtime, 0));
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(WriteKeyNoSchema), Message:" + ex.Message, GetSessionId(), "WriteKeyNoSchema", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }
    #endregion

    public bool HashSetKey(string KeyName, string KeyParam, string KeyValue)
    {
        bool added = false;
        byte[] bytes;
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
                return added = false;

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, KeyValue);
                bytes = stream.ToArray();
            }
            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
                    added = cacheClient.HashSet(KeyName, KeyParam, bytes);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(HashSetKey), Message:" + ex.Message, GetSessionId(), "HashSetKey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public bool HashSetKeyWithSchema(string KeyName, string KeyParam, string KeyValue, string schemaName)
    {
        bool added = false;
        byte[] bytes;
        var redis = RedisConnect();
        try
        {
            if (schemaNameKey == string.Empty)
            {
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            else if (schemaNameKey == string.Empty && schemaName == string.Empty)
                return added = false;

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, KeyValue);
                bytes = stream.ToArray();
            }
            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
                    added = cacheClient.HashSet(KeyName, KeyParam, bytes);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(HashSetKeyWithSchema), Message:" + ex.Message, GetSessionId(), "HashSetKeyWithSchema", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public bool HashDeletekey(string KeyName, string KeyParam)
    {
        bool removed = false;
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
                return removed = false;

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
                if (cacheClient.HashExists(KeyName, KeyParam))
                    removed = cacheClient.HashDelete(KeyName, KeyParam);
            }
        }
        catch (Exception ex)
        {
            removed = false;
            logObj.CreateLog("Redis Server Functon(HashDeletekey), Message:" + ex.Message, GetSessionId(), "HashDeletekey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return removed;
    }

    public bool HashDeletekeyNew(string KeyName, string KeyParam, string schemaName = "")
    {
        bool removed = false;
        var redis = RedisConnect();
        try
        {
            if (schemaNameKey == string.Empty)
            {
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            else if (schemaNameKey == string.Empty && schemaName == string.Empty)
                return removed = false;

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
                if (cacheClient.HashExists(KeyName, KeyParam))
                    removed = cacheClient.HashDelete(KeyName, KeyParam);
            }
        }
        catch (Exception ex)
        {
            removed = false;
            logObj.CreateLog("Redis Server Functon(HashDeletekey), Message:" + ex.Message, GetSessionId(), "HashDeletekey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return removed;
    }
    public bool HashDeleteAllkey(string KeyName, string schemaName = "")
    {
        bool removed = false;
        var redis = RedisConnect();
        try
        {
            if (schemaNameKey == string.Empty)
            {
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            else if (schemaNameKey == string.Empty && schemaName == string.Empty)
                return false;

            IDatabase cacheClient = redis.GetDatabase();
            if (!redis.IsConnected)
                return false;
            KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
            removed = cacheClient.KeyDelete(KeyName);
        }
        catch (Exception ex)
        {
            removed = false;
            logObj.CreateLog("Redis Server Functon(HashDeleteAllkey), Message:" + ex.Message, GetSessionId(), "HashDeleteAllkey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return removed;
    }

    public bool HashSetSaveInRedisServer(string key, string subkey, object value, string type, string schemaName = "")
    {
        bool added = false;
        byte[] bytes;
        var redis = RedisConnect();
        try
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, value);
                bytes = stream.ToArray();
            }
            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    key = schemaName == "" ? schemaNameKey + '-' + key : schemaName + '-' + key;
                    added = cacheClient.HashSet(key, subkey, bytes);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(HashSetSaveInRedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public bool WriteKeyAutoExpireInSec(string KeyName, string KeyValue, int expirtime)
    {
        bool added = false;
        byte[] bytes;
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
                return added = false;

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, KeyValue);
                bytes = stream.ToArray();
            }
            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    KeyName = schemaName == "" ? schemaNameKey + '-' + KeyName : schemaName + '-' + KeyName;
                    added = cacheClient.StringSet(KeyName, bytes, new TimeSpan(0, 0, expirtime));
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(WriteKey), Message:" + ex.Message, GetSessionId(), "WriteKey", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public bool HashSetEntries(string key, HashEntry[] hashEntries, int expiry = 0)
    {
        bool added = false;
        var redis = RedisConnect();
        try
        {
            var db = redis.GetDatabase();
            db.HashSet(key, hashEntries);

            if (expiry > 0)
            {
                added = db.KeyExpire(key, TimeSpan.FromSeconds(expiry));
            }
            else
            {
                added = true;
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(HashSetEntries), Message:" + ex.Message, GetSessionId(), "HashSetEntries", "new");
        }
        finally
        {
            RedisClose(redis);
        }

        return added;
    }

    public bool DeleteWildCharKeys(ArrayList keyPatterns)
    {
        bool removed = false;
        var redis = RedisConnect();

        try
        {
            if (redis != null && redis.IsConnected)
            {
                IDatabase db = redis.GetDatabase();

                foreach (var endpoint in redis.GetEndPoints())
                {
                    var server = redis.GetServer(endpoint);
                    if (!server.IsConnected) continue;

                    foreach (string pattern in keyPatterns)
                    {
                        var keys = server.Keys(pattern: schemaNameKey + '-' + pattern);

                        var keyList = new List<RedisKey>();

                        foreach (var key in keys)
                        {
                            keyList.Add(key);
                        }

                        if (keyList.Count > 0)
                        {
                            db.KeyDelete(keyList.ToArray());
                            removed = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(DeleteKeys), Message:" + ex.Message, GetSessionId(), "DeleteKeys", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return removed;
    }
}
