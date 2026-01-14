using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Services;
using Newtonsoft.Json.Linq;
public class BulkUserActivate
{
    public BulkUserActivate()
    {

    }
    public void ProcessBulkActivation(string jobId, DataTable userList, string schemaName, string _dbuser, string redisDtls, string _db_type, string __connectionString)
    {
        bulkFDW fdwObj = new bulkFDW(redisDtls);
        int processedCount = 0;

        try
        {
            fdwObj.WriteKeyAutoExpire(Constants.REDISBULKUSERACTIVATE + "-" + jobId, "Processing started..", 60, _dbuser);
            foreach (DataRow row in userList.Rows)
            {
                string username = Convert.ToString(row["Username"]);
                string userType = Convert.ToString(row["UserType"]);
                if (userType == "N")
                    userType = "";
                string isActive = "T";
                string res = ALCClient.CallALCClientBulk("Activate", username, userType, isActive, schemaName);
                if (res.StartsWith("Error:") || res.Contains("The operation has timed out"))
                {
                    if (res.Contains("{\"Message\":"))
                    {
                        res = res.Replace("Error: 400 - Bad Request,  Error response body: ", "");
                        JObject jsonObject = JObject.Parse(res);
                        res = jsonObject["Message"].ToString();
                    }
                    else
                        res = "Error: Error while activating user, please try again";
                    string err = "Error: Error activating for " + username + ": " + res;
                    if (processedCount > 0)
                    {
                        err = "Error: " + processedCount + " users activated. Error activating for " + username + ": " + res;
                    }
                    fdwObj.WriteKeyAutoExpire(Constants.REDISBULKUSERACTIVATE + "-" + jobId, err, 60, _dbuser);
                    return;
                }
                var outerObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
                string authKey = outerObject["AuthKey"];
                string userKey = outerObject["UserKey"];
                BulkDBContext obj = new BulkDBContext(_db_type, __connectionString);
                string dbRes = obj.UpdateALCUserInfo(username, isActive, authKey, userKey);
                if (dbRes == "0" || dbRes.StartsWith("Error:"))
                {
                    string err = "Error: Error activating for " + username + ": " + dbRes;
                    if (processedCount > 0)
                    {
                        err = "Error: " + processedCount + " users activated. Error activating for " + username + ": " + dbRes;
                    }
                    fdwObj.WriteKeyAutoExpire(Constants.REDISBULKUSERACTIVATE + "-" + jobId, err, 60, _dbuser);
                    return;
                }
                processedCount++;
                if (processedCount % 100 == 0)
                {
                    string progressMsg = processedCount + " users activated.";
                    fdwObj.WriteKeyAutoExpire(Constants.REDISBULKUSERACTIVATE + "-" + jobId, progressMsg, 60, _dbuser);
                }
            }
            string finalMsg = "Completed. Total " + processedCount + " users activated.";
            fdwObj.WriteKeyAutoExpire(Constants.REDISBULKUSERACTIVATE + "-" + jobId, finalMsg, 60, _dbuser);
        }
        catch (Exception ex)
        {
            fdwObj.WriteKeyAutoExpire(Constants.REDISBULKUSERACTIVATE + "-" + jobId, "Error: " + ex.Message, 60, _dbuser);
        }
    }

}

[Serializable()]
public sealed class bulkFDW
{
    public string schemaNameKey = string.Empty;
    [NonSerialized]
    private ConfigurationOptions configNew;
    string redisIP = string.Empty;
    string redisPass = string.Empty;
    public string redisIpLocalHost = string.Empty;

    public bulkFDW(string rcDetails)
    {
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

    public ConnectionMultiplexer RedisConnectNew()
    {
        try
        {
            if (configNew == null)
            {
                HashSet<string> redisCommands = new HashSet<string>
                {
                    "CLUSTER",
                    "PING", "ECHO", "CLIENT",
                    "SUBSCRIBE", "UNSUBSCRIBE", "NULL"
                };

                configNew = new ConfigurationOptions
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
                        configNew.EndPoints.Add(rIP);
                    }
                }
            }
            if (redisIP != "")
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configNew);
                try
                {
                    if (!redis.IsConnected)
                    {
                        schemaNameKey = string.Empty;
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
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            schemaNameKey = string.Empty;
        }
        return null;
    }

    public void RedisClose(ConnectionMultiplexer redis)
    {
        if (redis != null)
        {
            redis.Close(false);
            redis.Dispose();
        }
    }

    public bool WriteKeyAutoExpire(string KeyName, string KeyValue, int expirtime, string schemaNameKey)
    {
        bool added = false;
        byte[] bytes;
        var redis = RedisConnectNew();
        try
        {
            if (schemaNameKey == string.Empty)
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
                    KeyName = schemaNameKey + '-' + KeyName;
                    added = cacheClient.StringSet(KeyName, bytes, new TimeSpan(0, expirtime, 0));
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            RedisClose(redis);
        }
        return added;
    }
}

public class BulkDBContext
{
    private string db_type = String.Empty;
    private string _connectionString = String.Empty;

    public BulkDBContext(string axdb, string axconstr)
    {

        db_type = axdb;
        _connectionString = axconstr;
    }
    public string UpdateALCUserInfo(string username, string isActive, string authKey, string userKey)
    {
        string _results = string.Empty;
        try
        {
            string SqlQuery = string.Empty;
            Ihelper _helper2 = new Helper().SetDatabase(db_type, _connectionString);
            SqlQuery = "update axusers SET ACTIVE='" + isActive + "',ACTFLAG='" + isActive + "',AUTHKEY='" + authKey + "',USERKEY='" + userKey + "' where username='" + username + "'";
            _results = _helper2.ExecuteNonQuerySqlinlineAlc(SqlQuery);
        }
        catch (Exception ex)
        {
            _results = "Error: " + ex.Message;
        }
        return _results;
    }
}