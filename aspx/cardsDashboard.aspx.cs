using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Security.Principal;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;
using Axpert_Object;
using System.Web.UI;
using System.Linq;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Configuration;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Globalization;
using System.Net;
using System.Threading;

public partial class CustomPages_cardsDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string GetMenuAndCardsData()
    {

        Util.Util util;
        util = new Util.Util();
        LogFile.Log logobj = new LogFile.Log();
        FDW fdwObj = new FDW();
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        ASB.WebService asbWebService = new ASB.WebService();

        string proj = string.Empty;
        string language = string.Empty;
        string loginTrace = "false";
        string AxRole = string.Empty;
        string sid = string.Empty;
        string axApps = string.Empty;
        string axProps = string.Empty;
        string axRegId = string.Empty;
        string axSchema = string.Empty;
        string requestProcess_logtime = string.Empty;
        string commonResult = string.Empty;
        string menuResult = string.Empty;
        string menuXmlData = string.Empty;
        string cardsResult = string.Empty;
        string err = string.Empty;
        string lang_at = "";
        string errMsg = string.Empty;

        bool cardsEnabled = true;
        bool menuCached = false;
        bool cardsCached = false;

        JObject returnCardsObj = new JObject{
            {"data","" },
            {"design","" },
            {"menu","" }
        };

        //if (HttpContext.Current.Session["project"] == null)
        //    return util.SESSTIMEOUT;

        proj = HttpContext.Current.Session["project"].ToString();
        proj = util.CheckSpecialChars(proj);
        sid = HttpContext.Current.Session["nsessionid"].ToString();
        sid = util.CheckSpecialChars(sid);
        axProps = HttpContext.Current.Application["axProps"].ToString();
        AxRole = HttpContext.Current.Session["AxRole"].ToString();
        AxRole = util.CheckSpecialChars(AxRole);

        if (HttpContext.Current.Session["language"] != null && HttpContext.Current.Session["language"].ToString().ToUpper() != "ENGLISH")
            lang_at = " lang=\"" + language + "\"";

        if (ConfigurationManager.AppSettings["LoginTrace"] != null)
            loginTrace = ConfigurationManager.AppSettings["LoginTrace"].ToString();

        try
        {
            string fdKeyMenuData = Constants.REDISMENUDATA;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj != null)
                menuResult = fObj.StringFromRedis(util.GetRedisServerkey(fdKeyMenuData, "Menu"), schemaName);

            if (menuResult != string.Empty)
            {
                menuCached = true;
            }

            /*if (cardsEnabled)
            {
                if (fObj != null)
                {
                    try
                    {
                        bool isRedisConnected = fObj.IsConnected;
                        if (isRedisConnected)
                        {
                            string suffix = string.Empty;
                            if (HttpContext.Current.Session["language"] != null)
                            {
                                suffix = "-" + HttpContext.Current.Session["language"].ToString().Substring(0, 3);
                            }
                            ArrayList cardKeys = fObj.GetPrefixedKeys("General-" + Constants.REDISCARDROLES + "", true, suffix);
                            if (cardKeys.Count > 0)
                            {
                                cardsCached = true;
                                cardsResult = getRoleCards(AxRole);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logobj.CreateLog("GetMenuAndCardsData -- " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetMenuAndCardsData-exception", "new");
                    }
                }
            }
            else
            {
                cardsCached = true;
            }*/

            if (!menuCached || !cardsCached)
            {
                string sXml = string.Empty;
                string errlog = logobj.CreateLog("Getting Menu", sid, "GetMultiMenu", "new");
                if (loginTrace.ToLower() == "true")
                    errlog = logobj.CreateLog("Getting Menu", sid, "GetMultiMenu", "new", "true");

                string cardAttribute = string.Empty;

                if (!cardsCached)
                {
                    cardAttribute = " homepageflag='cards' ";
                }

                sXml = sXml + "<root " + cardAttribute + " menucached='" + menuCached.ToString().ToLower() + "' axpapp='" + proj + "' sessionid='" + sid + "' trace='" + errlog + "' mname =\"\" " + lang_at + " appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'";
                sXml = sXml + "> ";
                sXml = sXml + HttpContext.Current.Session["axApps"].ToString() + axProps + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
                sXml = sXml + "</root>";

                commonResult = objWebServiceExt.CallGetMultiLevelMenuWS("main", sXml);

                string[] splittedResult = commonResult.Split(new[] { "*$*" }, StringSplitOptions.None);

                if (splittedResult.Length > 0 && !menuCached)
                {
                    menuResult = splittedResult[0];

                    requestProcess_logtime += menuResult.Split('♠')[0];
                    menuResult = menuResult.Split('♠')[1];

                    menuResult = Regex.Replace(menuResult, ";fwdslh", "/");
                    menuResult = Regex.Replace(menuResult, ";hpn", "-");
                    menuResult = Regex.Replace(menuResult, ";bkslh", "\\");
                    menuResult = Regex.Replace(menuResult, ";eql", "=");
                    menuResult = Regex.Replace(menuResult, ";qmrk", "?");

                    errMsg = util.ParseXmlErrorNode(menuResult);
                    if (errMsg == string.Empty)
                    {
                        bool IsMenuCache = fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKeyMenuData, "Menu"), menuResult, Constants.REDISMENUDATA, schemaName);
                        if (IsMenuCache == false)
                            HttpContext.Current.Session["MenuData"] = menuResult;
                    }
                }
                if (splittedResult.Length > 1 && splittedResult[1] != "" && !cardsCached)
                {
                    try
                    {
                        foreach (string roleData in splittedResult[1].Split(new[] { "\n" }, System.StringSplitOptions.None))
                        {
                            if (roleData != string.Empty)
                            {
                                try
                                {
                                    string[] cardRoleSplit = roleData.Split('=');

                                    fdwObj.SaveInRedisServer(fObj.MakeKeyName(Constants.REDISCARDROLES, cardRoleSplit[0]), cardRoleSplit[1].ToString().Trim(','), "", schemaName);
                                }
                                catch (Exception ex)
                                {
                                    logobj.CreateLog("GetMenuAndCardsData -- " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetMenuAndCardsData-exception", "new");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logobj.CreateLog("GetMenuAndCardsData -- " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetMenuAndCardsData-exception", "new");
                    }
                }

                if (splittedResult.Length > 2 && splittedResult[2] != "")
                {
                    errMsg = util.ParseJSonErrorNode(splittedResult[2]);
                    if (errMsg == string.Empty)
                    {
                        try
                        {
                            cardsResult = JObject.Parse(splittedResult[2])["result"].ToString();
                        }
                        catch (Exception ex)
                        {
                            cardsResult = string.Empty;
                        }

                        cardsResult = refreshCards(cardsResult, true);
                    }
                }
                else if (cardsResult != "" && cardsResult != (new JArray()).ToString())
                {
                    cardsResult = refreshCards(cardsResult);
                }
            }
            else if (cardsResult != "" && cardsResult != (new JArray()).ToString())
            {
                cardsResult = refreshCards(cardsResult);
            }

        }
        catch (Exception ex)
        {
            logobj.CreateLog("GetMenuAndCardsData -- " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetMenuAndCardsData-exception", "new");
        }

        if (errMsg != string.Empty)
        {
            if (errMsg == Constants.SESSIONERROR || errMsg == Constants.SESSIONEXPMSG)
            {
                SessExpiresStatic();
            }
        }
        else
        {
            if (menuResult != string.Empty)
            {
                menuXmlData = menuResult;
                menuXmlData = menuXmlData.Replace(@"\", ";bkslh");
                menuXmlData = Regex.Replace(menuXmlData, "'", "&apos;");
                menuXmlData = Regex.Replace(menuXmlData, "&quot;", " ");
                returnCardsObj["menu"] = menuXmlData;
            }


            if (cardsResult != string.Empty)
            {
                //cardsDataVal = cardsResult;
                returnCardsObj["data"] = cardsResult;
            }

        }

        if (HttpContext.Current.Session["cardsDesignVal"] != null)
        {
            //cardsDesignVal = HttpContext.Current.Session["cardsDesignVal"].ToString();
            returnCardsObj["design"] = HttpContext.Current.Session["cardsDesignVal"].ToString();
        }
        else if (HttpContext.Current.Session["isAxMain"] != null && HttpContext.Current.Session["isAxMain"].ToString() == "true")
        {
            string result = string.Empty;
            string sqlInput = "select carddetails from axusers where username='" + HttpContext.Current.Session["username"].ToString() + "'";

            sqlInput = util.CheckSpecialChars(sqlInput);

            result = objWebServiceExt.ExecuteSQL("", sqlInput, "JSON");
            try
            {
                if (result != "")
                {
                    result = JToken.Parse(result)["result"]["row"][0]["carddetails"].ToString();
                    HttpContext.Current.Session["cardsDesignVal"] = result;
                } else
                {
                    result = "[]";
                }
            }
            catch (Exception)
            {
                result = "[]";
            }

            returnCardsObj["design"] = result;
        }

        return returnCardsObj.ToString();
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

        try
        {
            foreach (string cardId in cards.Split(','))
            {
                cardsArray.Add(new JObject{
                    { "axp_cardsid", cardId },
                    { "isDummy", true }
                });
            }

            returnString = cardsArray.ToString();
        }
        catch (Exception ex)
        { }

        return returnString;
    }

    private static string getRoleCards(string roles)
    {
        try
        {
            FDW fdwObj = new FDW();
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
                    string redisReturnString = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISCARDROLES, role), schemaName);
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

    private static string saveLoadCardsToRedis(string saveLoadJSON, bool isSave = true)
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


                            fdwObj.SaveInRedisServer(fObj.MakeKeyName(Constants.REDISCARDPARAMS, obj[cardsIdAccess].ToString()), keyAndParams["key"].ToString(), "", schemaName);
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

                            IsCardsCache = fdwObj.SaveInRedisServer(fObj.MakeKeyName(Constants.REDISCARDKEYS, jsonArray[ind][cardsIdAccess].ToString() + keyPostFix), jsonArray[ind].ToString(), "", schemaName);
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
                            string redisParamString = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISCARDPARAMS, obj["axp_cardsid"].ToString()), schemaName);

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

                            string finalKeyAccess = obj["axp_cardsid"].ToString();

                            if (paramKey != string.Empty)
                            {
                                finalKeyAccess += "-" + paramKey;
                            }

                            string redisCardData = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISCARDKEYS, finalKeyAccess), schemaName);

                            if (redisCardData != string.Empty)
                            {
                                finalArray.Add(JObject.Parse(redisCardData));
                            }
                            else
                            {
                                finalArray.Add(jsonArray[ind]);
                            }
                        }
                        catch (Exception ex)
                        {
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
                        oldIndex = oldArr
                        .Select((x, index) => new { Id = x.Value<string>("axp_cardsid"), isDummy = x.Value<bool>("isDummy"), Index = index })
                        .First(x => x.Id == objNew["axp_cardsid"].ToString() || x.isDummy)
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
}
