using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FastDataUtility : System.Web.UI.Page
{
    LogFile.Log logObj = new LogFile.Log();
    Util.Util util = new Util.Util();
    public string signOutPath = string.Empty;
    ArrayList Rediskeys = new ArrayList();
    static ArrayList RediskeysClone = new ArrayList();
    DataTable dt = null;
    StringBuilder sb = new StringBuilder();
    public string canvasText = string.Empty;
    public bool isCloudApp = false;
    string _searchItem = string.Empty;
    public string direction = "ltr";
    public string langType = "en";
    protected override void InitializeCulture()
    {
        if (Session["language"] != null)
        {
            string dirLang = string.Empty;
            dirLang = util.SetCulture(Session["language"].ToString().ToUpper());
            if (!string.IsNullOrEmpty(dirLang))
            {
                direction = dirLang.Split('-')[0];
                langType = dirLang.Split('-')[1];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        util = new Util.Util();
        util.IsValidSession();
        //util.IsValidAxpertSession();
        ResetSessionTime();
        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }

        if (Session["nsessionid"] != null)
        {
            if (ConfigurationManager.AppSettings["isCloudApp"] != null)
            {
                isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString()); ;
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "fast data utility", "<script>var isCloudApp = '" + isCloudApp.ToString() + "';</script>");

            //ScriptManager.RegisterStartupScript(this, GetType(), "Utility page charts", "ShowRedisUtilityCharts();", true);
        }
        else
            signOutPath = util.SIGNOUTPATH;
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", true);
        }
    }

    #region Methods
    #region Convert bytes to Megabytes
    public static string ConvertBytesToMegabytes(double size)
    {
        double sizeMB = Convert.ToDouble(size / 1024f / 1024f);
        sizeMB = Math.Round(sizeMB);
        return sizeMB.ToString();

        //if (memoryText == "used_memory")
        //    lblMemoryAllocated.Text = memoryText + " : " + sizeMB.ToString() + " MB";
        //else
        //    lblMemorydetails.Text = memoryText + " : " + sizeMB.ToString() + " MB";
    }
    #endregion

    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    #endregion

    /// <summary>
    /// dummy class to return Redis key info in List<FDU>
    /// </summary>
    class FDU
    {
        public string Key { get; set; }
    }

    #region Webmethods (jquery ajax calls)
    #region Get all Redis keys for the current project
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static object BindGridData()
    {
        List<FDU> keyList = new List<FDU>();
        string proj = "";
        if (HttpContext.Current.Session["dbuser"] != null)
            proj = HttpContext.Current.Session["dbuser"].ToString();
        else
            return "failure";
        FDR fdrObj = (FDR) HttpContext.Current.Session["FDR"];
        var arrayList = fdrObj.GetAllKeys(proj + "-");
        var widgetKeys = new ArrayList();
        string dbType = "", schemaTitle = "";
        if (HttpContext.Current.Session["axdb"] != null && HttpContext.Current.Session["project"] != null)
        {
            dbType = HttpContext.Current.Session["axdb"].ToString().ToLower().Replace(" ", "").Substring(0, 3);
            schemaTitle = HttpContext.Current.Session["project"].ToString();
            widgetKeys = fdrObj.GetAllKeys(dbType + "~" + schemaTitle + "~");
            arrayList.AddRange(widgetKeys);
        }
        for (int i = 0; i < arrayList.Count; i++)
        {
            keyList.Add(new FDU { Key = arrayList[i].ToString() });
        }
        return keyList.OrderBy(k => k.Key);
    }
    #endregion

    #region Delete selected Redis keys
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string DeleteKeys(string keylists)
    {
        if (HttpContext.Current.Session["HeaderCheck"] != null)
        {
            if ((bool) HttpContext.Current.Session["HeaderCheck"] == true)
            {
                keylists = "";
                if (RediskeysClone.Count > 0)
                {
                    foreach (var alkey in RediskeysClone)
                    {
                        keylists += alkey + ",";
                    }
                }
                RediskeysClone.Clear();
            }
        }

        try
        {
            FDW objFdw = new FDW();
            bool delRecords = objFdw.DeleteAllKeys(keylists);
            if (delRecords == true)
                return "success";
            else
                return "failure";
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("FastDataUtility page(DeleteKeys), Message:" + ex.Message, "", "RedisServer", "new");
            return "failure";
        }
    }
    #endregion

    #region Refresh all keys
    [System.Web.Services.WebMethod]
    public static string RefereshAll()
    {
        FastData objfd = new FastData();

        try
        {
            //objRS.FlushRedisServer();
            //objfd.DsDefinition = objfd.GetDefinition();
            objfd.CreateFdDataset(objfd.GetDefinition(), false);
            return "success";
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("FastDataUtility page(RefereshAll), Message:" + ex.Message, "", "RedisServer", "new");
            return "failure";
        }
    }
    #endregion

    #region Flushall keys
    [System.Web.Services.WebMethod]
    public static string FlushAll()
    {
        try
        {
            FDR fdrObj = (FDR) HttpContext.Current.Session["FDR"];
            bool flresult = fdrObj.FlushAllRedisKeys();
            if (flresult == true || flresult==false)
                return "success";
            else
                return "failure";
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("FastDataUtility page(FlushAll), Message:" + ex.Message, "", "RedisServer", "new");
            return "failure";
        }
    }
    #endregion

    #region Refresh selected Redis keys
    [System.Web.Services.WebMethod]
    public static string GetRefreshDatasetKeys(string dtKeylists)
    {
        string result = string.Empty;
        ArrayList resultlist = new ArrayList();
        FastData fd = new FastData();
        try
        {
            string[] rowtext2 = dtKeylists.Split('&');
            for (int r = 0; r < rowtext2.Length; r++)
            {
                string[] dtName = rowtext2[r].Split('-');
                //result = fd.RefreshFastDataset(rowtext2[r], dtName[1]);
                result = fd.RefreshFastDatasetUtil(dtName[1]);
                if (result == "1")
                    resultlist.Add(result);
            }

            if (resultlist.Count > 0)
                return "success";
            else
                return "failure";
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("FastDataUtility page(GetRefreshDatasetKeys), Message:" + ex.Message, "", "RedisServer", "new");
            return "failure";
        }
    }
    #endregion

    #region Get all Redis memory details (pie chart representation)
    [System.Web.Services.WebMethod]
    public static string MemoryDetails()
    {
        List<Dictionary<string, object>> allSeries = new List<Dictionary<string, object>>();
        string memokey = string.Empty;
        string value = string.Empty;
        string newvalue = string.Empty;
        string resultset = string.Empty;
        ArrayList redsMemDetails = new ArrayList();
        ArrayList chartdetails = new ArrayList();
        try
        {
            FDR fdrObj = (FDR) HttpContext.Current.Session["FDR"];
            chartdetails = fdrObj.GetMemoryDetails();
            if (chartdetails.Count > 0)
            {
                foreach (string arr2 in chartdetails)
                {
                    Dictionary<string, object> aSeries = new Dictionary<string, object>();
                    value = arr2.Split(':')[1];
                    aSeries["name"] = arr2.Split(':')[0];
                    newvalue = ConvertBytesToMegabytes(Convert.ToDouble(value));

                    aSeries["y"] = Convert.ToInt32(newvalue);
                    allSeries.Add(aSeries);
                }
                string pirrst = JsonConvert.SerializeObject(allSeries);
                resultset += pirrst.Replace(":[", ":").Replace("]}", "}");
            }
            return resultset;
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("FastDataUtility page(MemoryDetails), Message:" + ex.Message, "", "RedisServer", "new");
            return "failure";
        }
    }
    #endregion

    #endregion

    protected void btnGetLog_Click(object sender, EventArgs e)
    {
        FDW fdwObj = new FDW();
        fdwObj.LogFDWDetails();
    }
}
