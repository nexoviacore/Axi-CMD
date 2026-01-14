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
/// Summary description for DevFDW
/// </summary>
[Serializable()]
public sealed class DevFDW
{
    //private static readonly DevFDW Devinstance = new DevFDW();
    LogFile.Log logObj = new LogFile.Log();
    Util.Util utilObj = new Util.Util();
    public string schemaNameKey = string.Empty;
    public bool IsConnected = false;
    [NonSerialized]
    private ConfigurationOptions config;
    string redisIP = string.Empty;
    string redisPass = string.Empty;

    //public static DevFDW DevInstance
    //{
    //    get
    //    {
    //        return Devinstance;
    //    }
    //}

    public DevFDW()
    {
        if (HttpContext.Current.Session != null && HttpContext.Current.Session["axDevStudioRedisIP"] != null && HttpContext.Current.Session["axDevStudioRedisIP"].ToString() != "")
        {
            redisIP = HttpContext.Current.Session["axDevStudioRedisIP"].ToString();
            if (HttpContext.Current.Session["axDevStudioRedisPass"] != null && HttpContext.Current.Session["axDevStudioRedisPass"].ToString() != "")
                redisPass = HttpContext.Current.Session["axDevStudioRedisPass"].ToString();
        }
        else
        {
            string rcDetails = utilObj.GetRedisConnDetailsStudio();
            if (rcDetails != "" && rcDetails != "localhost")
            {
                redisIP = rcDetails.Split('♣')[0];
                redisPass = rcDetails.Split('♣')[1];
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

                    IsConnected = redis.IsConnected;
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
                        RedisClose(redis);
                    }
                }
                return redis;
            }
            else
                return null;
        }
        catch (Exception ex)
        {
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
            redis.Close(false);
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


    private bool HashSetKey(string KeyName, string subkey, string KeyValue, string ptype)
    {
        bool added = false;
        string projName = string.Empty;
        var redis = RedisConnect();
        try
        {

            if (HttpContext.Current.Session["project"] != null)
                projName = HttpContext.Current.Session["project"].ToString();
            else if (HttpContext.Current.Session["Project"] != null)
                projName = HttpContext.Current.Session["Project"].ToString();
            else
                throw new Exception("Project name not found.");

            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            else
                throw new Exception("Username not found.");

            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    KeyName = projName + '♠' + userName + '♠' + ptype + '♠' + KeyName;
                    added = cacheClient.HashSet(KeyName, subkey, KeyValue);
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
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

    public bool writeSessionHash(string ptype, string subkey = "")
    {
        bool added = false;
        string KeyName = "sessioninfo";
        string KeyValue = string.Empty;

        try
        {
            string[] subkeyArray = { "sessionid", "axapps", "axprops", "axglobalvars", "axuservars" };
            if (string.IsNullOrEmpty(subkey))
            {
                foreach (var key in subkeyArray)
                {
                    KeyValue = GetSessionValue(key);
                    if (!string.IsNullOrEmpty(KeyValue))
                    {
                        added = HashSetKey(KeyName, key, KeyValue, ptype);
                    }
                }
            }
            else
            {
                KeyValue = GetSessionValue(subkey);
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    added = HashSetKey(KeyName, subkey, KeyValue, ptype);
                }
                else
                {
                    throw new Exception(subkey + " not found.");
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Exception while writing session info to developer redis, Message:" + ex.Message, GetSessionId(), "HashSetKey", "new");
        }
        return added;
    }

    private string GetSessionValue(string subkey)
    {
        string KeyValue = string.Empty;

        if (subkey == "sessionid")
        {
            if (HttpContext.Current.Session != null)
            {
                KeyValue = HttpContext.Current.Session.SessionID.ToString();
            }
        }
        else if (subkey == "axglobalvars")
        {
            if (HttpContext.Current.Session["axGlobalVars"] != null)
            {
                KeyValue = HttpContext.Current.Session["axGlobalVars"].ToString();
            }
        }
        else if (subkey == "axapps")
        {
            if (HttpContext.Current.Session["axApps"] != null)
            {
                KeyValue = HttpContext.Current.Session["axApps"].ToString();
            }
        }
        else if (subkey == "axprops")
        {
            if (HttpContext.Current.Application["axProps"] != null)
            {
                KeyValue = HttpContext.Current.Application["axProps"].ToString();
            }
        }
        else if (subkey == "axuservars")
        {
            if (HttpContext.Current.Session["axUserVars"] != null)
            {
                KeyValue = HttpContext.Current.Session["axUserVars"].ToString();
            }
        }
        else
        {
            throw new Exception("Invalid subkey.");
        }

        return KeyValue;
    }

    public string HashGetKey(string ptype)
    {
        string result = string.Empty;
        string projName = string.Empty;
        string KeyName = "sessioninfo";
        string subKey = "sessionid";
        var redis = RedisConnect();
        try
        {
            if (HttpContext.Current.Session["project"] != null)
                projName = HttpContext.Current.Session["project"].ToString();
            else if (HttpContext.Current.Session["Project"] != null)
                projName = HttpContext.Current.Session["Project"].ToString();
            else
                throw new Exception("Project name not found.");

            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            else
                throw new Exception("Username not found.");

            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    KeyName = projName + '♠' + userName + '♠' + ptype + '♠' + KeyName;
                    string bytes = cacheClient.HashGet(KeyName, subKey);
                    if (bytes != null)
                    {
                        result = bytes;
                    }
                }
                else
                    result = "redisnotconnected";
            }
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


    public string HashGetKeyLocked(string ptype, string structName)
    {
        string result = string.Empty;
        string projName = string.Empty;
        var redis = RedisConnect();
        try
        {
            if (HttpContext.Current.Session["project"] != null)
                projName = HttpContext.Current.Session["project"].ToString();
            else if (HttpContext.Current.Session["Project"] != null)
                projName = HttpContext.Current.Session["Project"].ToString();
            else
                throw new Exception("Project name not found.");

            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            else
                throw new Exception("Username not found.");

            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    string KeyName = projName + "♠changedstructurelog";
                    string subKey = "*" + ptype + "♠" + structName + "♠mode";
                    var hashKeys = cacheClient.HashKeys(KeyName);
                    if (hashKeys.Count() > 0)
                    {
                        string matchingKey = null;
                        foreach (string key in hashKeys)
                        {
                            string[] parts = key.Split('♠');
                            if (parts.Length > 2 && parts[0] != userName && parts[2] == structName)
                            {
                                matchingKey = key;
                                break;
                            }
                        }
                        if (matchingKey != null)
                            result = matchingKey;
                        else
                            HashSetKeyLocked(ptype, structName);
                    }
                    else
                        HashSetKeyLocked(ptype, structName);
                }
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Redis Server Function(HashGetKeyLocked), Message:" + ex.Message, GetSessionId(), "HashGetKeyLocked", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return result;
    }

    private bool HashSetKeyLocked(string ptype, string structName)
    {
        bool added = false;
        string projName = string.Empty;
        var redis = RedisConnect();
        try
        {
            if (HttpContext.Current.Session["project"] != null)
                projName = HttpContext.Current.Session["project"].ToString();
            else if (HttpContext.Current.Session["Project"] != null)
                projName = HttpContext.Current.Session["Project"].ToString();
            else
                throw new Exception("Project name not found.");

            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            else
                throw new Exception("Username not found.");

            IDatabase cacheClient = redis.GetDatabase();
            lock (cacheClient)
            {
                if (redis.IsConnected)
                {
                    string KeyName = projName + "♠changedstructurelog";
                    string subKey = userName + "♠" + ptype + "♠" + structName + "♠mode";
                    added = cacheClient.HashSet(KeyName, subKey, "edit");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }

    public bool HashDeletekeyLocked(string ptype, string structName)
    {
        bool removed = false;
        string projName = string.Empty;
        var redis = RedisConnect();
        try
        {
            if (HttpContext.Current.Session["project"] != null)
                projName = HttpContext.Current.Session["project"].ToString();
            else if (HttpContext.Current.Session["Project"] != null)
                projName = HttpContext.Current.Session["Project"].ToString();
            else
                throw new Exception("Project name not found.");
            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            else
                throw new Exception("Username not found.");
            IDatabase cacheClient = redis.GetDatabase();
            if (redis.IsConnected)
            {
                string KeyName = projName + "♠changedstructurelog";
                string subKey = userName + "♠" + ptype + "♠" + structName + "♠mode";
                if (cacheClient.HashExists(KeyName, subKey))
                    removed = cacheClient.HashDelete(KeyName, subKey);
            }
        }
        catch (Exception ex)
        {
            removed = false;
            logObj.CreateLog("Redis Server Functon(HashDeletekeyLocked), Message:" + ex.Message, GetSessionId(), "HashDeletekeyLocked", "new");
        }
        finally
        {
            RedisClose(redis);
        }
        return removed;
    }
}
