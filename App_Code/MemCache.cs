using ASBExt;
using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Util;
using System.Configuration;

/// <summary>
/// Summary description for MemCache
/// </summary>
/// 
/// 
/// 
/// 
/// 
[Serializable]
public class MemCache
{
    string schemaNameKey = string.Empty;
    public MemCache()
    {
        //
        // TODO: Add constructor logic here
        //
        Util.Util utilObj = new Util.Util();
        schemaNameKey = utilObj.GetSchemaName();
        if (schemaNameKey == string.Empty) schemaNameKey = "Global";
    }

    public void CreateMemServer()
    {

        string memIP = string.Empty;
        if (ConfigurationManager.AppSettings["MemCacheIP"] != null)
            memIP = ConfigurationManager.AppSettings["MemCacheIP"].ToString();

        if (memIP == string.Empty)
            memIP = "127.0.0.1";
        string[] serverlist = { "" + memIP + ":11211" };
        SockIOPool pool = SockIOPool.GetInstance();
        pool.SetServers(serverlist);

        pool.InitConnections = 3;
        pool.MinConnections = 3;
        pool.MaxConnections = 5;

        //pool.SocketConnectTimeout = 1000;
        // pool.SocketTimeout = 3000;

        pool.MaintenanceSleep = 30;
        pool.Failover = true;

        pool.Nagle = false;
        pool.Initialize();


    }


    #region savememcache
    public void SaveInMemCac(string key, string value, string type)
    {
        MemcachedClient mc = new MemcachedClient();
        mc.EnableCompression = false;
        key = schemaNameKey + '-' + key;
        mc.Set(key, value);
        SaveMemcacKeys(key, type);
    }

    public void SaveInMemCacobj(string key, object value, string type)
    {
        try
        {
            MemcachedClient mc = new MemcachedClient();
            mc.EnableCompression = false;
            key = schemaNameKey + '-' + key;
            mc.Set(key, value);
            SaveMemcacKeys(key, type);
        }
        catch (Exception ex)
        { }
    }

    private void SaveMemcacKeys(string key, string type)
    {
        if (type != string.Empty)
        {
            string result = string.Empty;
            string keyValue = string.Empty;
            MemcachedClient mc = new MemcachedClient();
            string keyname = schemaNameKey + '-' + type + "-Keys";
            keyValue = GetFromMemCac(keyname);

            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = key;
            }
            else
            {
                if (!keyValue.Contains(key))
                    keyValue = keyValue + ',' + key;
            }
            mc.EnableCompression = false;
            mc.Set(keyname, keyValue);
        }
    }

    #endregion memcache


    #region getfrommemcache

    public string GetFromMemCac(string key)
    {
        string result = string.Empty;
        MemcachedClient mc = new MemcachedClient();
        mc.EnableCompression = false;
        key = schemaNameKey + '-' + key;
        result = (string)mc.Get(key);
        if (result == null) result = string.Empty;
        return result;
    }

    public object GetFromMemCacobj(string key)
    {
        object result = string.Empty;
        MemcachedClient mc = new MemcachedClient();
        key = schemaNameKey + '-' + key;
        mc.EnableCompression = false;
        result = mc.Get(key);
        return result;
    }

    #endregion 


    private void ClearMemCacbyType(string type)
    {
        MemcachedClient mc = new MemcachedClient();
        string result = string.Empty;
        string keyname = HttpContext.Current.Session["project"].ToString() + '-' + type + "Keys";
        result = GetFromMemCac(keyname);
        if (result != string.Empty)
        {
            string[] arrkeys = result.Split(',');
            foreach (string key in arrkeys)
            {
                mc.KeyExists(key);
                mc.Delete(key);
            }
            mc.Delete(keyname);
        }
    }

    private void ClearALLMemcacheData()
    {

        ClearMemCacbyType(Constants.CACMENU);
        //ClearMemCacbyType(Constants.MEMCACTSTRUCT);
    }

    private void ClearMemCacDataByKey(string key, string type)
    {
        MemcachedClient mc = new MemcachedClient();
        string keyname = HttpContext.Current.Session["project"].ToString() + '-' + type + "Keys";
        string result = string.Empty;

        if (mc.KeyExists(key))
            mc.Delete(key);

        result = GetFromMemCac(keyname);
        result = result.Replace(key, string.Empty);
        result = result.Replace(",,", ",");
        mc.EnableCompression = false;
        mc.Set(keyname, result);
    }

    public void DeleteKey(string key)
    {
        MemcachedClient mc = new MemcachedClient();
        if (mc.KeyExists(key))
            mc.Delete(key);
    }

    public void GetRolesByResponsible(string respon)
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
                ClearMemCacMenuBasedOnRole(rolename);
            }
        }


    }


    #region clearmenus
    public string ClearMemCacMenuBasedOnRole(string role)
    {
        string cackey = string.Empty;
        if (role != string.Empty)
        {
            string applanguages = HttpContext.Current.Session["AxLanguages"].ToString();
            if (applanguages != string.Empty)
            {
                string[] arrlanguages = applanguages.Split(',');

                foreach (string language in arrlanguages)
                {
                    cackey = "MenuData-" + HttpContext.Current.Session["project"] + '-' + role + '-' + language;
                    ClearMemCacDataByKey(cackey, Constants.CACMENU);

                    cackey = "MenuHtml-" + HttpContext.Current.Session["project"] + '-' + role + '-' + language;
                    ClearMemCacDataByKey(cackey, Constants.CACMENU);
                }
            }
        }
        return "success";
    }
    #endregion


    #region clearTstruct

    public void ClearMemCacByTid(string transid)
    {
        //string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.MEMCACTSTRUCT + "Keys";
        //string keyvalue = GetFromMemCac(keyname);
        //string[] arrkeys = keyvalue.Split(',');
        //string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + transid + "-"));
        //foreach (string key in arrtidkeys)
        //    ClearMemCacDataByKey(key, Constants.MEMCACTSTRUCT);
    }

    public void ClearMemCacTstructByRole(string Rolename)
    {
        //string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.MEMCACTSTRUCT + "Keys";
        //string keyvalue = GetFromMemCac(keyname);
        //string[] arrkeys = keyvalue.Split(',');
        //string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + "tstructstruct-" + Rolename + "-"));
        //foreach (string key in arrtidkeys)
        //    ClearMemCacDataByKey(key, Constants.MEMCACTSTRUCT);
    }
    #endregion


    #region Clear Iviews



    public string ClearMemCacIviewStructByRole(string roleName)
    {
        string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.IVIEWSTRUCT + "Keys";
        string keyvalue = GetFromMemCac(keyname);
        string[] arrkeys = keyvalue.Split(',');
        string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + "tstructstruct-" + roleName.ToLower() + "-"));
        foreach (string key in arrtidkeys)
            ClearMemCacDataByKey(key, Constants.IVIEWSTRUCT);
        return "Cache Cleared successfully";
    }

    public string ClearMemCacIviewStruct(string Iviewname)
    {
        string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.IVIEWSTRUCT + "Keys";
        string keyvalue = GetFromMemCac(keyname);
        string[] arrkeys = keyvalue.Split(',');
        string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + Iviewname.ToLower() + "-"));
        foreach (string key in arrtidkeys)
            ClearMemCacDataByKey(key, Constants.IVIEWSTRUCT);
        return "Cache Cleared successfully";
    }


    public string ClearMemCacIviewParamByRole(string Iviewname)
    {
        string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.IVIEWPARAM + "Keys";
        string keyvalue = GetFromMemCac(keyname);
        string[] arrkeys = keyvalue.Split(',');
        string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-getparam-" + Iviewname.ToLower() + "-"));
        foreach (string key in arrtidkeys)
            ClearMemCacDataByKey(key, Constants.IVIEWSTRUCT);
        return "Cache Cleared successfully";
    }

    public string ClearMemCacIviewParam(string Iviewname)
    {
        string keyname = HttpContext.Current.Session["project"].ToString() + '-' + Constants.IVIEWPARAM + "Keys";
        string keyvalue = GetFromMemCac(keyname);
        string[] arrkeys = keyvalue.Split(',');
        string[] arrtidkeys = Array.FindAll(arrkeys, element => element.ToLower().Contains("-" + Iviewname.ToLower() + "-"));
        foreach (string key in arrtidkeys)
            ClearMemCacDataByKey(key, Constants.IVIEWSTRUCT);
        return "Cache Cleared successfully";
    }


    #endregion




}
