using ASBExt;
using System;
using System.Data;
using System.IO;
using System.Web;
using StackExchange.Redis;
using System.Configuration;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Text.RegularExpressions;


/// <summary>
/// Summary description for RedisServer
/// </summary>
/// 
/// 
/// 
/// 
/// 
[Serializable]
public sealed class RedisServer
{
    private static readonly RedisServer instance = new RedisServer();
    LogFile.Log logObj = new LogFile.Log();
    public string schemaNameKey = string.Empty;
    private readonly ConfigurationOptions config;
    private static ConnectionMultiplexer redis;
    string redisIP = string.Empty;
    string redisPass = string.Empty;
    public static RedisServer Instance
    {
        get
        {
            return instance;
        }
    }

    public RedisServer()
    {
        if (redis == null)
        {
            if (HttpContext.Current.Session["RedisCachePwd"] != null && HttpContext.Current.Session["RedisCachePwd"].ToString() != "")
            {
                redisPass = HttpContext.Current.Session["RedisCachePwd"].ToString();
            }
            config = new ConfigurationOptions
            {
                SyncTimeout = int.MaxValue,
                KeepAlive = 60,
                Password = redisPass,
                AbortOnConnectFail = false,
                AllowAdmin = true,
                //DefaultDatabase = 0,
                //    Ssl = true
            };
            if (HttpContext.Current.Session["RedisCacheIP"] != null && HttpContext.Current.Session["RedisCacheIP"].ToString() != "")
            {
                redisIP = HttpContext.Current.Session["RedisCacheIP"].ToString();
                if (redisIP != "")
                {
                    foreach (var rIP in redisIP.Split(','))
                    {
                        config.EndPoints.Add(rIP);
                    }
                }

                if (config.EndPoints.Count == 0)
                {
                    //config.EndPoints.Add("127.0.0.1:6379");
                }

                try
                {

                    redis = ConnectionMultiplexer.Connect(config);
                }
                catch (Exception ex)
                { }
            }
        }

        try
        {

            if (!redis.IsConnected)
            {
                schemaNameKey = string.Empty;
            }
        }
        catch (Exception ex)
        {
            schemaNameKey = string.Empty;
            logObj.CreateLog("Redis Server Constructor(RedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        if (schemaNameKey == string.Empty)
        {
            return;
        }

    }



    #region saveRedisServer
    public bool SaveInRedisServer(string key, object value, string type)
    {
        bool added = false;
        if (schemaNameKey == string.Empty)
        {
            return added;
        }
        byte[] bytes;
        try
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, value);
                bytes = stream.ToArray();
            }
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                key = schemaNameKey + '-' + key;
                added = cacheClient.StringSet(key, bytes);
                //SaveRedisServerKeys(key, type);
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(SaveInRedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
            //throw ex;
        }
        return added;
    }



    //public bool SaveInRedisServerobj(string key, object value, string type)
    //{
    //    bool added = false;
    //    try
    //    {
    //       
    //        IDatabase cacheClient = redis.GetDatabase();
    //        key = schemaNameKey + '-' + key;
    //        added = cacheClient.StringSet(key, value);
    //        SaveRedisServerKeys(key, type);
    //    }
    //    catch (Exception ex)
    //    { }
    //    return added;
    //}

    public bool SaveRedisServerKeys(string key, string type)
    {
        bool added = false;
        if (schemaNameKey == string.Empty)
        {
            return added;
        }
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                if (type != string.Empty)
                {
                    string result = string.Empty;
                    string keyValue = string.Empty;
                    string keyname = schemaNameKey + '-' + type + "-Keys";
                    keyValue = StringFromRedis(keyname);
                    if (string.IsNullOrEmpty(keyValue))
                    {
                        keyValue = key;
                    }
                    else
                    {
                        if (!keyValue.Contains(key))
                            keyValue = keyValue + ',' + key;
                    }
                    added = cacheClient.StringSet(keyname, keyValue);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(SaveRedisServerKeys), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return added;
    }

    #endregion RedisServer


    public ArrayList GetAllKeys(string pattern)
    {// "*"

        ArrayList list = new ArrayList();
        if (pattern != "*")
        {
            pattern = "*" + pattern + "*";
        }
        try
        {
            var endpoints = redis.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = redis.GetServer(endpoint);
                if (server.IsConnected)
                {
                    foreach (string key in server.Keys(pattern: pattern))
                    {
                        if (key.Contains(schemaNameKey))
                            list.Add(key);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(GetAllKeys), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return list;
    }


    #region getfromRedisServer

    public string StringFromRedis(string key)
    {

        string result = string.Empty;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        if (key == string.Empty) return string.Empty;

        IDatabase cacheClient = redis.GetDatabase();


        byte[] bytes = null;
        try
        {
            if (redis.IsConnected)
            {
                key = schemaNameKey + '-' + key;
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
            logObj.CreateLog("Redis Server Functon(StringFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return result;
    }

    public object ObjectJsonFromRedis(string key)
    {
        object result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
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
                    result = (object)new BinaryFormatter().Deserialize(stream);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ObjectJsonFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
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
            logObj.CreateLog("Redis Server Functon(DataSetFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return result;
    }
    public TStructDef TstructDefFromRedis(string key)
    {
        TStructDef result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
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
                        result = (TStructDef)new BinaryFormatter().Deserialize(stream);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(TstructDefFromRedis), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return result;
    }

    public bool CheckKey(string key)
    {
        bool exist = false;
        if (schemaNameKey == string.Empty)
        {
            return exist;
        }
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                key = schemaNameKey + '-' + key;
                if (cacheClient.KeyExists(key))
                {
                    exist = true;
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(CheckKey), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return exist;
    }

    //public void GetAllKeys(string searchStr)
    //{
    //    if (schemaNameKey == string.Empty)
    //    {
    //        return;
    //    }
    //    string keyList = string.Empty;
    //    try
    //    {

    //        IDatabase cacheClient = redis.GetDatabase();
    //        if (redis.IsConnected)
    //        {
    //            var endpoints = redis.GetEndPoints(true);
    //            foreach (var endpoint in endpoints)
    //            {
    //                var server = redis.GetServer(endpoint);
    //                //server.FlushAllDatabases();
    //                if (server.IsConnected)
    //                {
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logObj.CreateLog("Redis Server Functon(FlushRedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
    //    }
    //}

    public void FlushRedisServer()
    {
        if (schemaNameKey == string.Empty)
        {
            return;
        }
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                var endpoints = redis.GetEndPoints(true);
                foreach (var endpoint in endpoints)
                {
                    var server = redis.GetServer(endpoint);
                    if (server.IsConnected)
                    {
                        server.FlushAllDatabases();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(FlushRedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
    }

    #endregion

    #region PushPop Redis Server
    public bool PushPopInRedisServer(string key, object value, string type)
    {
        bool added = false;
        if (schemaNameKey == string.Empty)
        {
            return added;
        }
        byte[] bytes;
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
        return added;
    }

    public DataTable GetDataTableForPushPop(string key)
    {
        DataTable result = null;
        if (schemaNameKey == string.Empty)
        {
            return result;
        }
        byte[] bytes = null;
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
        return result;
    }
    #endregion

    private bool ClearRedisServerbyType(string type)
    {
        bool removed = false;
        if (schemaNameKey == string.Empty)
        {
            return removed;
        }
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                string result = string.Empty;
                string keyname = HttpContext.Current.Session["project"].ToString() + '-' + type + "Keys";
                keyname = schemaNameKey + '-' + keyname;
                result = StringFromRedis(keyname);
                if (result != string.Empty)
                {
                    string[] arrkeys = result.Split(',');
                    foreach (string key in arrkeys)
                    {
                        if (cacheClient.KeyExists(key))
                        {
                            cacheClient.KeyDelete(key);
                        }
                    }
                    cacheClient.KeyDelete(keyname);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ClearRedisServerbyType), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return removed;
    }

    private void ClearALLRedisServerData()
    {
        if (schemaNameKey == string.Empty)
        {
            return;
        }
        try
        {
            ClearRedisServerbyType(Constants.CACMENU);
            ClearRedisServerbyType(Constants.REDISTSTRUCT);
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ClearALLRedisServerData), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
    }

    public bool ClearRedisServerDataByKey(string key, string type, bool schemaexist)
    {
        bool removed = false;
        if (schemaNameKey == string.Empty)
        {
            return removed;
        }
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                string keyname = string.Empty;
                if (!schemaexist)
                {
                    keyname = schemaNameKey + '-' + key;
                }
                else
                {
                    keyname = key;
                }
                string result = string.Empty;
                if (cacheClient.KeyExists(keyname))
                {
                    cacheClient.KeyDelete(keyname);
                }
            }
        }
        catch (Exception ex)
        {
            removed = false;
            logObj.CreateLog("Redis Server Functon(ClearRedisServerDataByKey), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return removed;
    }

    public bool DeleteKey(string key)
    {
        bool removed = false;
        if (schemaNameKey == string.Empty)
        {
            return removed;
        }
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                if (cacheClient.KeyExists(key))
                {
                    removed = cacheClient.KeyDelete(key);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(DeleteKey), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return removed;
    }

    public bool DeleteAllKeys(string key)
    {
        bool removed = false;
        if (schemaNameKey == string.Empty)
        {
            return removed;
        }
        try
        {

            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                string[] multikeys = key.Split(',');
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
        return removed;
    }

    public void GetRolesByResponsible(string respon)
    {
        if (schemaNameKey == string.Empty)
        {
            return;
        }
        try
        {
            string sql = "select userroles from axusergroups where groupname  = '" + respon + "'";
            WebServiceExt objExt = new WebServiceExt();
            string result = objExt.CallGetChoiceWS("", sql);
            if (result.Contains(Constants.ERROR) == true)
            {
                result = result.Replace(Constants.ERROR, "");
                result = result.Replace("</error>", "");
                result = result.Replace("\n", "");
                throw (new Exception(result));
            }
            if (result != string.Empty)
            {
                DataSet ds = new DataSet();
                StringReader sr = new StringReader(result);
                ds.ReadXml(sr);
                DataTable dt = ds.Tables["row"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string rolename = dt.Rows[1]["userroles"].ToString();
                    ClearRedisServerMenuBasedOnRole(rolename);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(GetRolesByResponsible), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }

    }


    #region clearmenus
    public string ClearRedisServerMenuBasedOnRole(string role)
    {
        if (schemaNameKey == string.Empty)
        {
            return string.Empty;
        }
        string cackey = string.Empty;
        try
        {
            if (role != string.Empty)
            {
                string applanguages = HttpContext.Current.Session["AxLanguages"].ToString();
                if (applanguages != string.Empty)
                {
                    string[] arrlanguages = applanguages.Split(',');

                    foreach (string language in arrlanguages)
                    {
                        cackey = "MenuData-" + HttpContext.Current.Session["project"] + '-' + role + '-' + language;
                        ClearRedisServerDataByKey(cackey, Constants.CACMENU, false);

                        cackey = "MenuHtml-" + HttpContext.Current.Session["project"] + '-' + role + '-' + language;
                        ClearRedisServerDataByKey(cackey, Constants.CACMENU, false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ClearRedisServerMenuBasedOnRole), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return "success";
    }
    #endregion


    #region clearTstruct

    public void ClearRedisServerByTid(string transid)
    {
        if (schemaNameKey == string.Empty)
        {
            return;
        }
        try
        {
            string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.REDISTSTRUCT + "Keys";
            string keyvalue = StringFromRedis(keyname);
            string[] arrkeys = keyvalue.Split(',');
            string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + transid + "-"));
            foreach (string key in arrtidkeys)
                ClearRedisServerDataByKey(key, Constants.REDISTSTRUCT, false);
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ClearRedisServerByTid), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
    }

    public void ClearRedisServerTstructByRole(string Rolename)
    {
        if (schemaNameKey == string.Empty)
        {
            return;
        }
        try
        {
            string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.REDISTSTRUCT + "Keys";
            string keyvalue = StringFromRedis(keyname);
            string[] arrkeys = keyvalue.Split(',');
            string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + "tstructstruct-" + Rolename + "-"));
            foreach (string key in arrtidkeys)
                ClearRedisServerDataByKey(key, Constants.REDISTSTRUCT, false);
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ClearRedisServerTstructByRole), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
    }
    #endregion


    #region Clear Iviews



    public string ClearRedisServerIviewStructByRole(string roleName)
    {
        if (schemaNameKey == string.Empty)
        {
            return string.Empty;
        }
        try
        {
            string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.IVIEWSTRUCT + "Keys";
            string keyvalue = StringFromRedis(keyname);
            string[] arrkeys = keyvalue.Split(',');
            string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + "tstructstruct-" + roleName.ToLower() + "-"));
            foreach (string key in arrtidkeys)
                ClearRedisServerDataByKey(key, Constants.IVIEWSTRUCT, false);
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ClearRedisServerIviewStructByRole), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return "Cache Cleared successfully";
    }

    public string ClearRedisServerIviewStruct(string Iviewname)
    {
        if (schemaNameKey == string.Empty)
        {
            return string.Empty;
        }
        try
        {
            string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.IVIEWSTRUCT + "Keys";
            string keyvalue = StringFromRedis(keyname);
            string[] arrkeys = keyvalue.Split(',');
            string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + Iviewname.ToLower() + "-"));
            foreach (string key in arrtidkeys)
                ClearRedisServerDataByKey(key, Constants.IVIEWSTRUCT, false);
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ClearRedisServerIviewStruct), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return "Cache Cleared successfully";
    }


    public string ClearRedisServerIviewParamByRole(string Iviewname)
    {
        if (schemaNameKey == string.Empty)
        {
            return string.Empty;
        }
        try
        {
            string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.IVIEWPARAM + "Keys";
            string keyvalue = StringFromRedis(keyname);
            string[] arrkeys = keyvalue.Split(',');
            string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-getparam-" + Iviewname.ToLower() + "-"));
            foreach (string key in arrtidkeys)
                ClearRedisServerDataByKey(key, Constants.IVIEWSTRUCT, false);
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ClearRedisServerIviewParamByRole), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return "Cache Cleared successfully";
    }

    public string ClearRedisServerIviewParam(string Iviewname)
    {
        if (schemaNameKey == string.Empty)
        {
            return string.Empty;
        }
        try
        {
            string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.IVIEWPARAM + "Keys";
            string keyvalue = StringFromRedis(keyname);
            string[] arrkeys = keyvalue.Split(',');
            string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + Iviewname.ToLower() + "-"));
            foreach (string key in arrtidkeys)
                ClearRedisServerDataByKey(key, Constants.IVIEWSTRUCT, false);
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(ClearRedisServerIviewParam), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return "Cache Cleared successfully";
    }


    #endregion

    private string GetSessionId()
    {
        if (schemaNameKey == string.Empty)
        {
            return string.Empty;
        }
        try
        {
            if (HttpContext.Current.Session["nsessionid"] != null)
            {
                return HttpContext.Current.Session["nsessionid"].ToString();
            }
            else
            {
                return "111";
            }
        }
        catch (Exception)
        {
            return "111";
        }
    }

    #region Memory details
    public ArrayList GetMemoryDetails()
    {
        ArrayList serverinfo = new ArrayList();

        try
        {
            if (redis.IsConnected)
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
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(GetMemoryDetails), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }
        return serverinfo;
    }
    #endregion

    #region find key based on pattern
    public bool FlushAllRedisKeys()
    {
        bool flush = false;
        ArrayList list = new ArrayList();

        try
        {
            if (redis.IsConnected)
            {
                var endpoints = redis.GetEndPoints(true);
                IDatabase cacheClient = redis.GetDatabase();
                foreach (var endpoint in endpoints)
                {
                    var server = redis.GetServer(endpoint);
                    foreach (string key in server.Keys(pattern: "*" + schemaNameKey + "*"))
                    {
                        if (key.Contains(schemaNameKey))
                        {
                            list.Add(key);
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (cacheClient.KeyExists(list[i].ToString()))
                                {
                                    flush = cacheClient.KeyDelete(list[i].ToString());
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Functon(FlushAllRedisKeys), Message:" + ex.Message, GetSessionId(), "RedisServer", "new");
        }

        return flush;
    }
    #endregion


}
