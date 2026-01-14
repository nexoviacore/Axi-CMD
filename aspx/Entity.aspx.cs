using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;
using StackExchange.Redis.Extensions.Core.Extensions;

public partial class aspx_Entity : System.Web.UI.Page
{
    #region Variable Declaration
    Util.Util util;
    public string proj = string.Empty;
    public string sid = string.Empty;
    public string language = string.Empty;
    public string trace = string.Empty;
    public string user = string.Empty;
    public string entityName = string.Empty;
    public string transId = string.Empty;
    public string selectedFields = string.Empty;
    public string applyFilter = string.Empty;
    public string langType = "en";
    public string direction = "ltr";
    #endregion

    protected override void InitializeCulture()
    {
        if (Session["language"] != null)
        {
            util = new Util.Util();
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

        if (Request.QueryString["tstid"] != null && !string.IsNullOrEmpty(Request.QueryString["tstid"].ToString()))
        {
            transId = Request.QueryString["tstid"].ToString();
            util = new Util.Util();
            util.IsValidSession();

            if (Session["project"] == null)
            {
                SessionExpired();
                return;
            }
            else
            {
                if (!util.CheckValidLogin())
                {
                    SessionExpired();
                    return;
                }

                Entity entity = new Entity();

                List<Dictionary<string, Object>> filters = new List<Dictionary<string, Object>>();
                int pageSize = 100;

                if (Request.QueryString["applyfilter"] != null && Request.QueryString["applyfilter"].ToString() == "true" && Request.QueryString["filterfld"] != null)
                {
                    Dictionary<string, Object> filterObj = new Dictionary<string, object>();
                    filterObj.Add("ftransid", transId);
                    filterObj.Add("fldname", Request.QueryString["filterfld"].ToString());
                    filterObj.Add("condition", "EQUALS");
                    if (Request.QueryString["filtertype"] != null && Request.QueryString["filtertype"].ToString() == "date")
                    {
                        filterObj.Add("datatype", "DATE");
                        if (Request.QueryString["filterfrom"] != null)
                            filterObj.Add("from", Request.QueryString["filterfrom"].ToString());
                        if (Request.QueryString["filterto"] != null)
                            filterObj.Add("to", Request.QueryString["filterto"].ToString());
                    }
                    else
                    {
                        filterObj.Add("datatype", "TEXT");
                        filterObj.Add("value", Request.QueryString["filterval"].ToString());
                    }

                    filters.Add(filterObj);
                }

                hdnEntityPageLoadData.Value = entity.GetEntityListPageLoadData("Entity", transId, 1, pageSize, filters, false);

                string axDisallowCreate = "";
                if (HttpContext.Current.Session["AxDisallowCreate"] != null && HttpContext.Current.Session["AxDisallowCreate"].ToString() != "")
                    axDisallowCreate = HttpContext.Current.Session["AxDisallowCreate"].ToString();


                //if (entityListData.IndexOf("\"count\":0,") != -1 && !axDisallowCreate.Split(',').Contains(transId) && applyFilter != "true")
                //{
                //    Response.Redirect("tstruct.aspx?transid=" + transId);
                //    return;
                //}

                IncludeCustomFiles();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "EntityListData", "var axDisallowCreate =  `" + axDisallowCreate + "`;", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "EntityMissing", "alert('Entity details is missing.')", true);
        }
    }

    private void IncludeCustomFiles()
    {
        string projName = HttpContext.Current.Session["Project"].ToString();
        FileInfo filtcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/datalist/js/" + transId + ".js"));
        if (filtcustom.Exists)
        {
            HtmlGenericControl js = new HtmlGenericControl("script");
            js.Attributes["type"] = "text/javascript";
            string path = "../" + projName + "/datalist/js/" + transId + ".js?v=" + filtcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
            js.Attributes["src"] = path;
            ScriptManager.Controls.Add(js);
        }
        else
        {
            FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/datalist/js/custom.js"));
            if (filcustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("script");
                js.Attributes["type"] = "text/javascript";
                string path = "../" + projName + "/datalist/js/custom.js?v=" + filcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["src"] = path;
                ScriptManager.Controls.Add(js);
            }
        }

        FileInfo filtcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/datalist/css/" + transId + ".css"));
        if (filtcsscustom.Exists)
        {
            HtmlGenericControl js = new HtmlGenericControl("link");
            js.Attributes["type"] = "text/css";
            js.Attributes["rel"] = "stylesheet";
            string path = "../" + projName + "/datalist/css/" + transId + ".css?v=" + filtcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
            js.Attributes["href"] = path;
            ScriptManager.Controls.Add(js);
        }
        else
        {
            FileInfo filcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/datalist/css/custom.css"));
            if (filcsscustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("link");
                js.Attributes["type"] = "text/css";
                js.Attributes["rel"] = "stylesheet";
                string path = "../" + projName + "/datalist/css/custom.css?v=" + filcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["href"] = path;
                ScriptManager.Controls.Add(js);
            }
        }
    }

    private static string GetEntityListData(string transId, string fields, int pageNo, int pageSize, bool metaData, string filter)
    {
        //string ARMSessionId = GetARMSessionId();
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;

        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();


        string tasksUrl = ARM_URL + "/api/v1/GetEntityListData";
        List<string> transIds = new List<string>();
        transIds.Add(transId);
        //Dictionary<string, string> viewFilters = GetViewFilters(transIds);
        var entityDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            TransId = transId,
            Fields = fields,
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString(),
            Language = HttpContext.Current.Session["language"].ToString(),
            PageNo = pageNo,
            //ViewFilters = viewFilters,
            PageSize = pageSize,
            MetaData = metaData,
            Filter = filter
        };

        var entities = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(entityDetails));
        return entities;

    }

    private static string GetEntityChartsData(string entityName, string transId, string condition, string criteria)
    {
        //string ARMSessionId = GetARMSessionId();
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;

        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();


        string tasksUrl = ARM_URL + "/api/v1/GetEntityChartsData";

        List<string> transIds = new List<string>();
        transIds.Add(transId);
        //Dictionary<string, string> viewFilters = GetViewFilters(transIds);

        var entityDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            EntityName = entityName,
            TransId = transId,
            Condition = condition,
            Criteria = criteria,
            //ViewFilters = viewFilters,
            Language = HttpContext.Current.Session["language"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString()
        };

        var entities = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(entityDetails));
        return entities;

    }


    private static string GetEntityChartsMetaData(string entityName, string transId)
    {
        //string ARMSessionId = GetARMSessionId();
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;

        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();


        string tasksUrl = ARM_URL + "/api/v1/GetEntityChartsMetaData";

        var entityDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            EntityName = entityName,
            TransId = transId,
            Language = HttpContext.Current.Session["language"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString()
        };

        var entities = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(entityDetails));
        return entities;

    }


    [WebMethod]
    public static string GetEntityChartsDataWS(string entityName, string transId, string condition, string criteria)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        return GetEntityChartsData(entityName, transId, condition, criteria);
    }

    [WebMethod]
    public static string GetEntityChartsMetaDataWS(string entityName, string transId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        return GetEntityChartsMetaData(entityName, transId);
    }

    [WebMethod]
    public static string GetEntityListDataWS(string transId, string fields, int pageNo, int pageSize, List<Dictionary<string, Object>> filter)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        Entity entity = new Entity();
        var result = entity.GetEntityListPageLoadData("Entity", transId, pageNo, pageSize, filter, true);
        return result;
        //return GetEntityListData(transId, fields, pageNo, pageSize, false, filter);
    }

    [WebMethod]
    public static string GetEntityEditableFlagWS(string page, string transId, string recordId, string action)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        Entity entity = new Entity();
        var result = entity.GetEntityEditableFlag("Entity", transId, recordId, action);
        return result;
    }

    [WebMethod]
    public static string GetEntityListWithGridDataWS(string transId, string fields, int pageNo, int pageSize, string filter, string gridDc)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string gridDcFlds = "";
        if (gridDc == "T")
        {
            gridDcFlds = GetSelectedDisplayGridFields(transId);
        }

        if (fields == "All")
        {
            string dcFlds = GetSelectedDisplayFields(transId);
            if (!string.IsNullOrEmpty(dcFlds))
                fields = dcFlds;
        }

        if (!string.IsNullOrEmpty(gridDcFlds))
            fields = fields + "^" + gridDcFlds;



        return GetEntityListData(transId, fields, pageNo, pageSize, false, filter);
    }

    private static string GetSelectedDisplayFields(string transId)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        if (fObj != null)
            return fObj.StringFromRedis("SelectedDisplayFields-" + transId, schemaName);
        else
            return "";
    }

    private static string GetSelectedDisplayGridFields(string transId)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        if (fObj != null)
            return fObj.StringFromRedis("SelectedDisplayGridFields-" + transId, schemaName);
        else
            return "";
    }

    private static string GetSelectedEntityFields(string transId)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        if (fObj != null)
            return fObj.StringFromRedis("SelectedFields-" + transId, schemaName);
        else
            return "";
    }

    private static string GetEntityKeyField(string transId)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        if (fObj != null)
            return fObj.StringFromRedis("SelectedKeyField-" + transId, schemaName);
        else
            return "";
    }
    private static string SetSelectedEntityFields(string transId, string fields, string keyField)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDW fdwObj = new FDW();
        fdwObj.SaveInRedisServer("SelectedFields-" + transId, fields, "", schemaName);
        if (fields == "" || fields == "All")
            fdwObj.ClearRedisServerDataByKey("SelectedKeyField-" + transId, "", false, schemaName);
        else
            fdwObj.SaveInRedisServer("SelectedKeyField-" + transId, keyField, "", schemaName);
        return "Success";
    }


    [WebMethod]
    public static string GetSelectedEntityFieldsWS(string transId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        return GetSelectedEntityFields(transId);
    }

    [WebMethod]
    public static string SetSelectedEntityFieldsWS(string transId, string fields, string keyField)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        return SetSelectedEntityFields(transId, fields, keyField);
    }


    private static string GetValue(string key)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        if (fObj != null)
            return fObj.StringFromRedis(key, schemaName);
        else
            return "";
    }

    [WebMethod]
    public static string GetValueWS(string key)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        return GetValue(key);
    }

    private static string SetValue(string key, string value)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDW fdwObj = new FDW();
        fdwObj.SaveInRedisServer(key, value, "", schemaName);
        return "Success";
    }

    [WebMethod]
    public static string SetValueWS(string key, string value)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        return SetValue(key, value);
    }


    private static string GetSelectedEntityCharts(string transId)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        if (fObj != null)
            return fObj.StringFromRedis("SelectedCharts-" + transId, schemaName);
        else
            return "";
    }
    private static string SetSelectedEntityCharts(string transId, string charts)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDW fdwObj = new FDW();
        fdwObj.SaveInRedisServer("SelectedCharts-" + transId, charts, "", schemaName);
        return "Success";
    }


    [WebMethod]
    public static string GetSelectedEntityChartsWS(string transId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        return GetSelectedEntityCharts(transId);
    }

    [WebMethod]
    public static string SetSelectedEntityChartsWS(string transId, string charts)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        return SetSelectedEntityCharts(transId, charts);
    }

    private static string GetSelectedEntityFilters(string transId)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        if (fObj != null)
            return fObj.StringFromRedis("SelectedFilters-" + transId, schemaName);
        else
            return "";
    }
    private static string SetSelectedEntityFilters(string transId, string filters)
    {
        string schemaName = HttpContext.Current.Session["project"].ToString();
        FDW fdwObj = new FDW();
        fdwObj.SaveInRedisServer("SelectedFilters-" + transId, filters, "", schemaName);
        return "Success";
    }

    [WebMethod]
    public static string SetSelectedEntityFiltersWS(string transId, string filters)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        return SetSelectedEntityFilters(transId, filters);
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
        Response.End();
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", true);
        }
    }




    //private static string CallWebAPI(string url, string method = "GET", string contentType = "application/json", string body = "", string calledFrom = "")
    //{
    //    AddToARMLog("URL:" + url + Environment.NewLine + "Input Json: " + Environment.NewLine + body);
    //    string result = string.Empty;
    //    try
    //    {

    //        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
    //        httpRequest.Method = method;
    //        httpRequest.ContentType = contentType;
    //        if (HttpContext.Current.Session["ARM_Token"] != null && HttpContext.Current.Session["ARM_Token"].ToString() != string.Empty)
    //        {
    //            var token = HttpContext.Current.Session["ARM_Token"].ToString();
    //            httpRequest.Headers.Add("Authorization", "Bearer " + token);
    //        }

    //        using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
    //        {
    //            streamWriter.Write(body);
    //        }

    //        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
    //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //        {
    //            result = streamReader.ReadToEnd();
    //        }
    //    }
    //    catch (WebException e)
    //    {
    //        try
    //        {
    //            using (WebResponse response = e.Response)
    //            {
    //                HttpWebResponse httpResponse = (HttpWebResponse)response;
    //                //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);

    //                //if (calledFrom == "" && httpResponse.StatusCode == HttpStatusCode.Unauthorized)
    //                //{
    //                //    return RefreshSessionAndRecallAPI(url, method, contentType, body, calledFrom = "Error");
    //                //}

    //                using (Stream data = response.GetResponseStream())
    //                using (var reader = new StreamReader(data))
    //                {
    //                    result = reader.ReadToEnd();
    //                }

    //                //if (calledFrom == "" && result.IndexOf("SessionId is not valid") > -1)
    //                //{
    //                //    return RefreshSessionAndRecallAPI(url, method, contentType, body, calledFrom = "Error");
    //                //}
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            result = JsonConvert.SerializeObject(new { error = ex.Message });
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        result = JsonConvert.SerializeObject(new { error = e.Message });
    //    }
    //    AddToARMLog("Output Json: " + Environment.NewLine + result);
    //    return result;
    //}

    //private static string RefreshSessionAndRecallAPI(string url, string method = "GET", string contentType = "application/json", string body = "", string calledFrom = "")
    //{
    //    HttpContext.Current.Session.Remove("ARM_SessionId");
    //    HttpContext.Current.Session.Remove("ARM_Token");
    //    var ARMSessionId = GetARMSessionId();
    //    try
    //    {
    //        var jsonBody = JObject.Parse(body);
    //        if (jsonBody["ARMSessionId"] != null)
    //        {
    //            jsonBody["ARMSessionId"] = ARMSessionId;
    //            body = JsonConvert.SerializeObject(jsonBody);
    //        }
    //    }
    //    catch { }
    //    return CallWebAPI(url, method, contentType, body, calledFrom = "Error");
    //}

    private static string MD5Hash(string text)
    {
        MD5 md5 = new MD5CryptoServiceProvider();

        //compute hash from the bytes of text  
        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

        //get hash result after compute it  
        byte[] result = md5.Hash;

        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < result.Length; i++)
        {
            //change it into 2 hexadecimal digits  
            //for each byte  
            strBuilder.Append(result[i].ToString("x2"));
        }

        return strBuilder.ToString();
    }

    private static string GetARMSessionId()
    {
        try
        {
            var ARMSessionId = "";
            string sessionId = HttpContext.Current.Session.SessionID;
            if (HttpContext.Current.Session["ARM_SessionId"] == null)
            {
                //string privateKey = ConfigurationManager.AppSettings["ARM_PrivateKey"].ToString();
                string privateKey = String.Empty;
                if (HttpContext.Current.Session["ARM_PrivateKey"] != null)
                    privateKey = HttpContext.Current.Session["ARM_PrivateKey"].ToString();
                else
                    return "Error in ARM connection.";

                string hashedKey = MD5Hash(privateKey + sessionId);
                var axpertDetails = new
                {
                    user = HttpContext.Current.Session["user"].ToString(),
                    key = hashedKey,
                    AxSessionId = sessionId,
                    Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
                    AppName = HttpContext.Current.Session["project"].ToString()
                };
                string ARM_URL = string.Empty;
                if (HttpContext.Current.Session["ARM_URL"] != null)
                    ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();
                else
                    return "Error in ARM connection.";
                string connectionUrl = ARM_URL + "/api/v1/ARMConnectFromAxpert";

                AnalyticsUtils _aUtils = new AnalyticsUtils();
                var connectionResult = _aUtils.CallWebAPI(connectionUrl, "POST", "application/json", JsonConvert.SerializeObject(axpertDetails));

                var jObj = Newtonsoft.Json.Linq.JObject.Parse(connectionResult);
                if (jObj != null && jObj["result"] != null)
                {
                    if (Convert.ToBoolean(jObj["result"]["success"]))
                    {
                        ARMSessionId = jObj["result"]["connectionid"].ToString();
                        var token = jObj["result"]["token"].ToString();
                        HttpContext.Current.Session["ARM_SessionId"] = ARMSessionId;
                        HttpContext.Current.Session["ARM_Token"] = token;
                    }
                    else
                    {
                        return "Error in ARM connection.";
                    }
                }
            }
            else
            {
                ARMSessionId = HttpContext.Current.Session["ARM_SessionId"].ToString();
            }
            return ARMSessionId;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private static Dictionary<string, string> GetViewFilters(List<string> transIds = null)
    {
        Dictionary<string, string> viewFilters = new Dictionary<string, string>();
        if (HttpContext.Current.Session["ViewFiltersDictionary"] == null)
        {
            string xml = HttpContext.Current.Session["axGlobalVars"].ToString();

            XElement root = XElement.Parse(xml);
            Dictionary<string, string> globalVars = new Dictionary<string, string>();

            foreach (XElement element in root.Elements())
            {
                string key = element.Name.LocalName;
                if (globalVars.ContainsKey(key))
                {
                    globalVars.Remove(key);
                }

                string value = element.Value;
                globalVars.Add(key, value);
            }

            foreach (var kvp in globalVars)
            {
                if (kvp.Key.ToLower().EndsWith("_filter"))
                {
                    string sql = kvp.Value;

                    sql = ReplaceSqlParameters(sql, globalVars);
                    viewFilters.Add(kvp.Key.ToLower().Replace("_filter", ""), sql);
                }
            }
            HttpContext.Current.Session["ViewFiltersDictionary"] = JsonConvert.SerializeObject(viewFilters);
        }
        else
        {
            string json = (string)HttpContext.Current.Session["ViewFiltersDictionary"];
            viewFilters = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        Dictionary<string, string> result = new Dictionary<string, string>();
        if (transIds != null && viewFilters != null)
        {
            foreach (var transId in transIds)
            {
                if (viewFilters.ContainsKey(transId))
                {
                    result.Add(transId, viewFilters[transId]);
                }
            }
        }
        return result;
    }

    static string ReplaceSqlParameters(string sql, Dictionary<string, string> parameters)
    {
        Regex regex = new Regex(@"\:\w+");

        // Use StringBuilder for efficient string manipulation
        StringBuilder sb = new StringBuilder(sql);

        // Find all parameter matches in the SQL string
        MatchCollection matches = regex.Matches(sql);

        // Iterate over matches in reverse order to avoid index issues
        for (int i = matches.Count - 1; i >= 0; i--)
        {
            Match match = matches[i];
            string paramKey = match.Value.Substring(1); // Remove leading ':'

            // Check if the parameter key exists in the dictionary
            if (parameters.ContainsKey(paramKey))
            {
                // Replace the parameter in SQL with its corresponding value
                sb.Remove(match.Index, match.Length); // Remove original parameter
                sb.Insert(match.Index, "'" + parameters[paramKey] + "'"); // Insert replacement value
            }
        }

        return sb.ToString();
    }

    public static void AddToARMLog(string text)
    {
        if (HttpContext.Current.Session["AxTrace"].ToString() == "true")
        {
            text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " - " + text;
            LogFile.Log logobj = new LogFile.Log();
            string sessID = "";
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logobj.CreateLog(text, sessID, "Entity List Page Logs", "");
        }
    }

    [WebMethod]
    public static string GetARMLogsWS()
    {
        if (HttpContext.Current.Session["ARMLogs"] == null)
        {
            return "";
        }
        else
        {
            return JsonConvert.SerializeObject(HttpContext.Current.Session["ARMLogs"]);
        }
    }
}
