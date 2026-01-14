using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class aspx_processflow : System.Web.UI.Page
{
    public string direction = "ltr";
    public string langType = "en";
    Util.Util util;
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
        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }
        else if (!util.CheckValidLogin())
        {
            SessionExpired();
            return;
        }
        //string axpertWebUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToLower().IndexOf("/aspx/") + 1);
        string path = string.Empty;//  axpertWebUrl;
        try
        {
            if (Request.QueryString["activelist"] != null && Request.QueryString["activelist"].ToString() == "t")
                path += "aspx/ProcessFlowTemplate.html";
            else if (Request.QueryString["dashboard"] != null && Request.QueryString["dashboard"].ToString() == "t")
            {
                if (Request.QueryString["edit"] != null && Request.QueryString["edit"].ToString() == "t")
                    path += "aspx/dashBoardTemplate.html?edit=t";
                else
                    path += "aspx/dashBoardTemplate.html";
            }
            else if (Request.QueryString["calendar"] != null && Request.QueryString["calendar"].ToString() == "t")
                path += "aspx/calendarTemplate.html";
            else if (Request.QueryString["loadcaption"] != null && Request.QueryString["loadcaption"].ToString() == "Active Lists")
            {
                string extraParams = string.Empty;
                try
                {
                    extraParams = Request.Url.Query.Substring(Request.Url.Query.IndexOf("&"));
                    ArrayList targetList = new ArrayList();
                    string[] qs = extraParams.Split('&');
                    foreach (string param in qs)
                    {
                        if (param != string.Empty)
                        {
                            string[] paramList = param.Split('=');
                            if (paramList.Length == 2 && paramList[0] != string.Empty && paramList[0] != "hltype" && paramList[0] != "hdnbElapsTime")
                            {
                                targetList.Add(param);
                            }
                        }
                    }

                    extraParams = String.Join("&", targetList.ToArray());

                    if (extraParams != string.Empty)
                    {
                        extraParams = "&" + extraParams;
                    }
                }
                catch (Exception ex) { }
                path += "aspx/Active_Lists.html?load=" + extraParams;
            }
            else if (Request.QueryString["loadcaption"] != null && Request.QueryString["loadcaption"].ToString() == "AxProcessBuilder")
            {
                string extraParams = string.Empty;
                try
                {
                    extraParams = Request.Url.Query.Substring(Request.Url.Query.IndexOf("&"));
                    ArrayList targetList = new ArrayList();
                    string[] qs = extraParams.Split('&');
                    foreach (string param in qs)
                    {
                        if (param != string.Empty)
                        {
                            string[] paramList = param.Split('=');
                            if (paramList.Length == 2 && paramList[0] != string.Empty && paramList[0] != "hltype" && paramList[0] != "hdnbElapsTime")
                            {
                                targetList.Add(param);
                            }
                        }
                    }

                    extraParams = String.Join("&", targetList.ToArray());

                    if (extraParams != string.Empty)
                    {
                        extraParams = "&" + extraParams;
                    }
                }
                catch (Exception ex) { }
                path += "aspx/AxProcessBuilder.html?load=" + extraParams;
            }
            else
            {
                Response.Redirect("err.aspx?errmsg=File doesn't exist in the path.");
                return;
            }
            //HttpWebRequest request = WebRequest.Create(path) as HttpWebRequest;
            //HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //HttpStatusCode status = response.StatusCode;
            //if (status.ToString() == "OK")
            //{			

            FileInfo _htmlFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + path.Split('?')[0].Replace('/', '\\'));
            if (_htmlFile.Exists)
            {

                if (path != string.Empty)
                {
                    try
                    {
                        Response.Write(@"<script language='javascript'> document.location.href='../" + path + "'; window.parent.callParentNew('closeFrame()','function');</script>");
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("err.aspx?errmsg=Exception while loading template.");
                    }
                }
            }
            else
            {
                Response.Redirect("err.aspx?errmsg=File doesn't exist in the path.");
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("err.aspx?errmsg=Exception while loading template.");
        }
    }

    public void SessionExpired()
    {
        Util.Util util = new Util.Util();
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

    [WebMethod]
    public static string GetDashBoardCardsData()
    {
        string proj = string.Empty;
        string AxRole = string.Empty;
        string sid = string.Empty;
        string axProps = string.Empty;
        string commonResult = string.Empty;
        string cardsResult = string.Empty;
        string lang_at = "";
        string errMsg = string.Empty;
        string cardsDataVal = string.Empty;

        if (HttpContext.Current.Session["language"] != null && HttpContext.Current.Session["language"].ToString().ToUpper() != "ENGLISH")
            lang_at = " lang=\"" + HttpContext.Current.Session["language"].ToString() + "\"";
        Util.Util util = new Util.Util();
        LogFile.Log logobj = new LogFile.Log();
        string homePageType = string.Empty;
        if (HttpContext.Current.Session["AxEnableCards"] != null)
        {
            homePageType = HttpContext.Current.Session["AxEnableCards"].ToString() == "true" ? "cards" : "";
        }
        proj = HttpContext.Current.Session["project"].ToString();
        proj = util.CheckSpecialChars(proj);
        sid = HttpContext.Current.Session["nsessionid"].ToString();
        sid = util.CheckSpecialChars(sid);
        axProps = HttpContext.Current.Application["axProps"].ToString();
        AxRole = HttpContext.Current.Session["AxRole"].ToString();
        AxRole = util.CheckSpecialChars(AxRole);

        bool cardsEnabled = true;
        bool menuCached = true;
        bool cardsCached = false;
        try
        {
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            //FDW fdwObj = new FDW();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

            if (cardsEnabled)
            {
                if (fObj != null)
                {
                    try
                    {
                        bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
                        if (isRedisConnected)
                        {
                            ASB.WebService asbWebService = new ASB.WebService();
                            cardsResult = asbWebService.GetCardsWithData(false, false);
                            if (cardsResult.IndexOf("SessionId is not valid") > -1)
                                cardsResult =  Constants.SESSIONTIMEOUT;                            
                        }
                    }
                    catch (Exception ex)
                    { }
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("GetDashBoardCardsData -- " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetDashBoardCardsData-exception", "new");
        }

        if (cardsResult != string.Empty)
        {
            cardsDataVal = cardsResult;
        }

        string cardsDesign = string.Empty;
        if (HttpContext.Current.Session["cardsDesignVal"] != null)
        {
            cardsDesign = HttpContext.Current.Session["cardsDesignVal"].ToString();
        }
        else
        {
            string result = string.Empty;
            string sqlInput = "select carddetails from axusers where username='" + HttpContext.Current.Session["username"].ToString() + "'";

            sqlInput = util.CheckSpecialChars(sqlInput);
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            result = objWebServiceExt.ExecuteSQL("", sqlInput, "JSON");
            try
            {
                if (result != "")
                {
                    result = JToken.Parse(result)["result"]["row"][0]["carddetails"].ToString();
                    HttpContext.Current.Session["cardsDesignVal"] = result;
                }
                else
                {
                    result = "[]";
                }
            }
            catch (Exception)
            {
                result = "[]";
            }

            cardsDesign = result;
        }
        return cardsDataVal + "♦" + cardsDesign;
    }

    private static string getRoleCards(string roles)
    {
        try
        {
            //FDW fdwObj = new FDW();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

            JObject cardRoles = new JObject();

            List<string> finalCardList = new List<string>();

            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            foreach (string role in roles.Split(','))
            {
                try
                {
                    string redisReturnString = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISCARDROLESDASHBOARD, role), schemaName);
                    if (redisReturnString != string.Empty)
                    {
                        List<string> tempList = redisReturnString.ToString().Split(',').ToList();
                        finalCardList = finalCardList.Union(tempList).ToList();
                    }
                }
                catch (Exception ex)
                { }
            }
            return String.Join(",", finalCardList);
        }
        catch (Exception ex)
        { }
        return string.Empty;
    }


    [WebMethod]
    public static string refreshCards(string json = "", bool isJSON = false, bool singleLoad = false)
    {
        string result = string.Empty;

        Util.Util util = new Util.Util();

        if (!isJSON)
        {
            json = createDummyCards(json);
        }

        if (!isJSON)
        {
            json = saveLoadCardsToRedis(json, false);
        }

        string expiredCards = string.Empty;

        if (!isJSON)
        {
            expiredCards = util.getExpiredCache(json, JObject.Parse("{\"id\": \"axp_cardsid\", \"cache\": \"cachedata\", \"cachedTime\": \"cachedTime\", \"refreshAfter\": \"autorefresh\"}"));
        }

        if (expiredCards != string.Empty)
        {
            ASB.WebService asbWebService = new ASB.WebService();

            string freshJSON = asbWebService.refreshCards(expiredCards);

            string errMsg = util.ParseXmlErrorNode(freshJSON);
            if (errMsg != string.Empty && errMsg == Constants.ERAUTHENTICATION)
            {
                SessExpiresStatic();
            }

            try
            {
                freshJSON = JObject.Parse(freshJSON)["result"].ToString();
            }
            catch (Exception ex)
            {
                freshJSON = string.Empty;
            }

            if (freshJSON != string.Empty)
            {
                freshJSON = saveLoadCardsToRedis(freshJSON);
            }

            if (freshJSON != string.Empty && !singleLoad)
            {
                result = mergeOldNewCards(json, freshJSON);
            }
            else if (freshJSON != string.Empty && singleLoad)
            {
                result = freshJSON;
            }
            else
            {
                result = json;
            }

        }
        else
        {
            result = json;
        }

        if (result != string.Empty && isJSON)
        {
            result = saveLoadCardsToRedis(result);
        }

        return result;
    }

    public static string createDummyCards(string cards = "")
    {
        string returnString = (new JArray()).ToString();

        JArray cardsArray = new JArray();

        string dbType = string.Empty;
        string cardsIdAccess = "axp_cardsid";

        if (HttpContext.Current.Session["axdb"] != null && HttpContext.Current.Session["axdb"].ToString() != string.Empty)
        {
            dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
        }

        if (dbType.ToLower() == "oracle")
        {
            cardsIdAccess = cardsIdAccess.ToUpper();
        }

        try
        {
            foreach (string cardId in cards.Split(','))
            {

                cardsArray.Add(new JObject{
                    { cardsIdAccess, cardId },
                    { "isDummy", true }
                });
            }

            returnString = cardsArray.ToString();
        }
        catch (Exception ex)
        { }

        return returnString;
    }

    private static string saveLoadCardsToRedis(string saveLoadJSON, bool isSave = true, bool cleanup = true)
    {
        string result = string.Empty;

        string errMsg = string.Empty;

        Util.Util util = new Util.Util();

        string dbType = string.Empty;
        if (HttpContext.Current.Session["axdb"] != null && HttpContext.Current.Session["axdb"].ToString() != string.Empty)
        {
            dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
        }

        string schemaName = string.Empty;
        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();

        JArray jsonArray = new JArray();

        try
        {
            jsonArray = JArray.Parse(saveLoadJSON);
        }
        catch (Exception ex) { }

        FDW fdwObj = new FDW();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

        string paramValue = string.Empty;

        if (isSave)
        {
            bool IsCardsCache = false;

            if (jsonArray.Count > 0)
            {
                int ind = -1;
                foreach (JObject obj in jsonArray)
                {
                    ind++;

                    if (obj["cachedTime"] == null || obj["cachedTime"].ToString() == "")
                    {
                        jsonArray[ind]["cachedTime"] = DateTime.Now.ToString("ddMMyyyyHHmm");

                        try
                        {
                            JObject keyAndParams = getKeyAndValue(obj);

                            paramValue = keyAndParams["value"].ToString();

                            string cardsIdAccess = "axp_cardsid";
                            if (dbType.ToLower() == "oracle" && obj[cardsIdAccess] == null && obj[cardsIdAccess.ToUpper()] != null)
                            {
                                cardsIdAccess = cardsIdAccess.ToUpper();
                            }


                            fdwObj.SaveInRedisServer(fObj.MakeKeyName(Constants.REDISCARDPARAMSDASHBOARD, obj[cardsIdAccess].ToString()), keyAndParams["key"].ToString(), "", schemaName);
                        }
                        catch (Exception ex)
                        { }

                        try
                        {
                            string keyPostFix = string.Empty;

                            if (paramValue != string.Empty)
                            {
                                keyPostFix = "-" + paramValue;
                            }

                            string cardsIdAccess = "axp_cardsid";
                            if (dbType.ToLower() == "oracle" && obj[cardsIdAccess] == null && obj[cardsIdAccess.ToUpper()] != null)
                            {
                                cardsIdAccess = cardsIdAccess.ToUpper();
                            }

                            IsCardsCache = fdwObj.SaveInRedisServer(fObj.MakeKeyName(Constants.REDISCARDKEYSDASHBOARD, jsonArray[ind][cardsIdAccess].ToString() + keyPostFix), jsonArray[ind].ToString(), "", schemaName);

                            if (jsonArray[ind]["cardbuttons"] != null && cleanup)
                            {
                                jsonArray[ind]["cardbuttons"] = cleanupSensitiveData((JObject)jsonArray[ind]["cardbuttons"]);
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }

            saveLoadJSON = jsonArray.ToString();

            if (IsCardsCache == false)
                HttpContext.Current.Session["CardsData"] = saveLoadJSON;
        }
        else
        {
            if (jsonArray.Count > 0)
            {
                try
                {
                    JArray finalArray = new JArray();

                    int ind = -1;
                    foreach (JObject obj in jsonArray)
                    {
                        ind++;

                        try
                        {
                            string cardsIdAccess = "axp_cardsid";
                            if (dbType.ToLower() == "oracle" && obj[cardsIdAccess] == null && obj[cardsIdAccess.ToUpper()] != null)
                            {
                                cardsIdAccess = cardsIdAccess.ToUpper();
                            }

                            string redisParamString = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISCARDPARAMSDASHBOARD, obj[cardsIdAccess].ToString()), schemaName);

                            JArray paramKeyArrayFinal = new JArray();

                            string paramKey = string.Empty;

                            if (redisParamString != string.Empty)
                            {
                                string[] paramKeyArray = redisParamString.Split(',');

                                foreach (string param in paramKeyArray)
                                {
                                    if (HttpContext.Current.Session[param] != null && HttpContext.Current.Session[param].ToString() != string.Empty)
                                    {
                                        paramKeyArrayFinal.Add(HttpContext.Current.Session[param].ToString());
                                    }
                                }
                            }

                            paramKey = String.Join("~", paramKeyArrayFinal);

                            string finalKeyAccess = obj[cardsIdAccess].ToString();

                            if (paramKey != string.Empty)
                            {
                                finalKeyAccess += "-" + paramKey;
                            }

                            string redisCardData = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISCARDKEYSDASHBOARD, finalKeyAccess), schemaName);

                            if (redisCardData != string.Empty)
                            {
                                JObject tempJsonObject = JObject.Parse(redisCardData);
                                if (tempJsonObject["cardbuttons"] != null && cleanup)
                                {
                                    tempJsonObject["cardbuttons"] = cleanupSensitiveData((JObject)tempJsonObject["cardbuttons"]);
                                }
                                finalArray.Add(tempJsonObject);
                            }
                            else
                            {
                                if (jsonArray[ind]["cardbuttons"] != null && cleanup)
                                {
                                    jsonArray[ind]["cardbuttons"] = cleanupSensitiveData((JObject)jsonArray[ind]["cardbuttons"]);
                                }
                                finalArray.Add(jsonArray[ind]);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (jsonArray != null && cleanup)
                            {
                                jsonArray[ind]["cardbuttons"] = cleanupSensitiveData((JObject)jsonArray[ind]["cardbuttons"]);
                            }
                            finalArray.Add(jsonArray[ind]);
                        }
                    }

                    saveLoadJSON = finalArray.ToString();

                    if ((saveLoadJSON == string.Empty || saveLoadJSON == (new JArray()).ToString()) && HttpContext.Current.Session["CardsData"] != null)
                    {
                        saveLoadJSON = HttpContext.Current.Session["CardsData"].ToString();
                    }
                }
                catch (Exception ex) { }
            }
        }
        return saveLoadJSON;
    }

    private static string mergeOldNewCards(string oldJSON, string newJSON)
    {
        string result = string.Empty;

        JArray oldArr = new JArray();

        JArray newArr = new JArray();

        string dbType = string.Empty;
        if (HttpContext.Current.Session["axdb"] != null && HttpContext.Current.Session["axdb"].ToString() != string.Empty)
        {
            dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
        }

        try
        {
            oldArr = JArray.Parse(oldJSON);
        }
        catch (Exception ex) { }

        try
        {
            newArr = JArray.Parse(newJSON);
        }
        catch (Exception ex) { }

        bool takeNew = false;
        JArray finalArray = new JArray();

        if (newArr.Count > 0)
        {
            JsonMergeSettings lMergeSettings = new JsonMergeSettings();
            lMergeSettings.MergeArrayHandling = MergeArrayHandling.Union;
            if (newArr.First.Type == JTokenType.Object)
            {
                int ind = -1;
                foreach (JObject objNew in newArr)
                {
                    ind++;

                    int oldIndex = -1;

                    try
                    {
                        string cardsIdAccess = "axp_cardsid";
                        if (dbType.ToLower() == "oracle")
                        {
                            cardsIdAccess = cardsIdAccess.ToUpper();
                        }

                        oldIndex = oldArr
                        .Select((x, index) => new { Id = x.Value<string>(cardsIdAccess), isDummy = x.Value<bool>("isDummy"), Index = index })
                        .First(x => x.Id == objNew[cardsIdAccess].ToString() || x.isDummy)
                        .Index;
                    }
                    catch (Exception ex) { }

                    if (oldIndex > -1)
                    {
                        oldArr[oldIndex].Remove();
                    }
                }
            }
            oldArr.Merge(newArr, lMergeSettings);
        }

        result = oldArr.ToString();

        return result;
    }

    private static void SessExpiresStatic()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
    }

    private static JObject getKeyAndValue(JObject obj)
    {
        JObject returnObj = new JObject{
            {"key","" },
            {"value","" }
        };

        string paramString = string.Empty;

        try
        {
            if (obj["cardsql"] != null && obj["cardsql"]["paramvars"] != null)
            {
                paramString = obj["cardsql"]["paramvars"].ToString();
            }
        }
        catch (Exception ex)
        { }

        try
        {
            if (paramString != string.Empty)
            {
                JArray key = new JArray();
                JArray value = new JArray();

                foreach (string kv in paramString.Split('~'))
                {
                    string[] kvArray = kv.Split('=');

                    key.Add(kvArray[0]);
                    value.Add(kvArray[1]);
                }
                returnObj["key"] = string.Join(",", key);
                returnObj["value"] = string.Join("~", value);
            }
        }
        catch (Exception ex)
        { }

        return returnObj;
    }
    public static JObject cleanupSensitiveData(JObject cardbuttons)
    {
        try
        {
            if (cardbuttons["btnoption"] != null)
            {
                foreach (JToken button in (JArray)cardbuttons["btnoption"])
                {
                    if (button["scripts"] != null)
                    {
                        button["scripts"] = true;
                    }
                }
            }
        }
        catch (Exception ex) { }

        return cardbuttons;
    }

    [WebMethod]
    public static string GetCalendarData()
    {
        if (HttpContext.Current.Session["username"] == null)
            return "";
        try
        {
            string _cardfData = "fields:[],row:[]";
            try
            {
                string _restPath = string.Empty;
                if (HttpContext.Current.Session["ARM_Scripts_URL"] != null)
                {
                    _restPath = HttpContext.Current.Session["ARM_Scripts_URL"].ToString();
                    if (_restPath.Substring(_restPath.Length - 1) != "/")
                        _restPath += "/";
                }
                else
                    _restPath = ConfigurationManager.AppSettings["RestDllPath"].ToString();
                _restPath = _restPath + "ASBMenuRest.dll/datasnap/rest/TASBMenuRest/GetSqldata";
                string axApps = xmltojson(HttpContext.Current.Session["axApps"].ToString());
                string _proj = HttpContext.Current.Session["project"].ToString();
                string DATA = "{\"_parameters\":[{\"getsqldata\":{\"axpapp\":\"" + _proj + "\",\"sqlname\":\"axcalendarsource\",\"isdropdown\":\"F\",\"trace\":\"false\"},\"" + _proj + "\":{" + axApps + "},\"sqlparams\":{\"username\":\"" + HttpContext.Current.Session["username"].ToString() + "\"}}]}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_restPath);

                request.Method = "POST";
                request.ContentType = "application/JSON";
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string result = responseReader.ReadToEnd();
                Console.Out.WriteLine(result);
                responseReader.Close();
                if (result != string.Empty)
                {
                    var obj = JObject.Parse(result);
                    if (obj["result"][0]["result"] != null)
                    {
                        _cardfData = obj["result"][0]["result"].ToString();
                        _cardfData = _cardfData.Remove(_cardfData.IndexOf("{"), 1);
                        _cardfData = _cardfData.Remove(_cardfData.LastIndexOf("}"), 1);
                    }
                }
            }
            catch (Exception ex)
            {
                _cardfData = "fields:[],row:[]";
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetCalendarData -- " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetCalendarData-exception", "new");
            }

            string _calendarJson = "[{'axp_cardsid': 'Cal12345','cardname': 'Calender','cardtype': 'calendar','charttype': '','chartjson': '{\"attributes\":{\"cck\":\"\",\"shwLgnd\":,\"xAxisL\":\"\",\"yAxisL\":\"\",\"gradClrChart\":,\"shwChartVal\":,\"threeD\":\"remove\",\"enableSlick\":,\"numbSym\":}}','cardicon': 'list_alt','pagename': '','pagedesc': '','cardbgclr': '','width': 'col-md-12','height': '100px','cachedata': 'true','autorefresh': '30','cardsql': {" + _cardfData + ",'paramvars': 'username=" + HttpContext.Current.Session["username"].ToString() + "'},'orderno': '1','accessstring': '','htransid': '','htype': '','hcaption': '','axpfile_imgcard': '','html_editor_card': '','calendarstransid': 'axcal','exp_editor_buttons': '','cachedTime': ''}]";
            string item = JsonConvert.DeserializeObject<object>(_calendarJson).ToString();
            JObject returnCardsObj = new JObject{
            {"data",item},
            {"design","" }
            };
            return returnCardsObj.ToString();
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("GetCalendarData1 -- " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetCalendarData1-exception", "new");
            return "";
        }
    }

    public static string xmltojson(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        string json = JsonConvert.SerializeXmlNode(doc);
        var parsed = JObject.Parse(json);
        var jtoken = ((Newtonsoft.Json.Linq.JContainer)parsed.First).First;
        return jtoken.ToString().Trim('{').Trim('}');
    }

    [WebMethod]
    public static string saveCardsDesign(string design)
    {
        string result = string.Empty;
        ASB.WebService asbWebService = new ASB.WebService();
        result = asbWebService.saveCardsDesign(design);
        if (result == "true")
            HttpContext.Current.Session["cardsDesignVal"] = null;
        return result;
    }
}
