using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class WidgetWebService : WebService
{
    ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    [WebMethod(EnableSession = true)]
    public object GetHomePageDetail(string module, string title, string pageid)
    {
        string publishedpages = string.Empty, pagetemplates = string.Empty, pagewidgets = string.Empty, getwidgetdetails = string.Empty;
        string errorLog = logobj.CreateLog("Get home page detail", HttpContext.Current.Session["nsessionid"].ToString(), "GetHomePageDetail", "new");
        try
        {
            string result = string.Empty;
            if (module != string.Empty)
            {
                string getPageInfo = string.Empty;
                getPageInfo = " module='" + module + "' title='" + title + "' pageid='" + pageid + "'";
                string inputXml = "<sqlresultset " + getPageInfo + " pagetemplate='false' axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                inputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                result = asbExt.CallGetHomePageDetailsWS(inputXml);
                if (result != string.Empty)
                {
                    dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(result);
                    publishedpages = jsonObj["publishedpages"].ToString();
                    pagetemplates = jsonObj["pagetemplates"].ToString();
                    pagewidgets = jsonObj["pagewidgets"].ToString();
                    getwidgetdetails = jsonObj["getwidgetdetails"].ToString();
                }
            }
            else
            {
                publishedpages = GetCachedData("publishedpages");
                pagetemplates = GetCachedData("pagetemplates");
                if (publishedpages == string.Empty)
                {
                    string pagetemplate = "false";
                    if (pagetemplates != string.Empty)
                        pagetemplate = "true";
                    string inputXml = "<sqlresultset pagetemplate='" + pagetemplate + "' axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                    inputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    result = asbExt.CallGetHomePageDetailsWS(inputXml);
                    if (result != string.Empty)
                    {
                        dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(result);
                        publishedpages = jsonObj["publishedpages"].ToString();
                        if (pagetemplate != "true")
                            pagetemplates = jsonObj["pagetemplates"].ToString();
                        pagewidgets = jsonObj["pagewidgets"].ToString();
                        if (jsonObj["getwidgetdetails"] != null)
                            getwidgetdetails = jsonObj["getwidgetdetails"].ToString();
                        SetDataCache(publishedpages, pagetemplates, pagewidgets, getwidgetdetails);
                    }
                }
                else
                {
                    pagewidgets = GetCachedData("pagewidgets");
                    getwidgetdetails = GetCachedData("getwidgetdetails");
                    getwidgetdetails = UpdateIndOnLoad(getwidgetdetails, pagewidgets, publishedpages);
                }
            }
        }
        catch (Exception ex)
        {
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logobj.CreateLog("GetHomePageDetail exception -" + ex.Message, sessID, "GetHomePageDetail-exception", "new");
        }
        return new { ppData = publishedpages, ptData = pagetemplates, pwData = pagewidgets, gwdData = getwidgetdetails };
    }

    [WebMethod(EnableSession = true)]
    public object GetIndividualWidgetDetail(string widgetIds, string tPageId, string widGetType, string ivName, string ivPageSize)
    {
        string result = string.Empty, widgetdetails = string.Empty;
        string errorLog = logobj.CreateLog("Get Individual Widget Detail", HttpContext.Current.Session["nsessionid"].ToString(), "GetIndividualWidgetDetail", "new");
        try
        {
            string typeOfData = "getwidgetdetails";
            string sqlQuery = string.Empty;
            string wgDetails = CheckIndWidgetTime(widgetIds, typeOfData);
            if (wgDetails != string.Empty && wgDetails.Split('$')[0] == "false")
                widgetdetails = wgDetails.Split('$')[1];
            else
            {
                sqlQuery = util.CheckSpecialChars(wgDetails.Split('$')[1]);
                string inputXml = "<sqlresultset pageid='" + tPageId + "' axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                if (widGetType == "widget")
                    inputXml += "<widget sqltext='" + sqlQuery + "'>" + widgetIds + "</widget>";
                else if (widGetType == "custom__sql")
                    inputXml += "<custom__sql sqltext='" + sqlQuery + "'>" + widgetIds + "</custom__sql>";
                else if (widGetType == "custom__mytsk")
                    inputXml += "<custom__mytsk sqltext='" + sqlQuery + "'>" + widgetIds + "</custom__mytsk>";
                else if (widGetType == "iview")
                    inputXml += "<iview iviewname='" + ivName + "' pagesize='" + ivPageSize + "' sqltext='" + sqlQuery + "'>" + widgetIds + "</iview>";
                inputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                result = asbExt.CallGetIndividualWidgetDetailsWS(inputXml);
                if (result != string.Empty)
                {
                    dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(result);
                    widgetdetails = jsonObj["getwidgetdetails"].ToString();
                    widgetdetails = UpdateIndWidgetData(widgetdetails, typeOfData, widgetIds, widGetType);
                }
            }
        }
        catch (Exception ex)
        {
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logobj.CreateLog("GetIndividualWidgetDetail exception -" + ex.Message, sessID, "GetIndividualWidgetDetail-exception", "new");
        }
        return new { gwdData = widgetdetails };
    }

    private string UpdateIndOnLoad(string getwidgetdetails, string pagewidgets, string publishedpages)
    {
        string result = string.Empty;
        string errorLog = logobj.CreateLog("Get Individual Widget Detail", HttpContext.Current.Session["nsessionid"].ToString(), "GetIndividualWidgetDetail", "new");
        try
        {
            string widgetIds = string.Empty, widGetType = string.Empty, tPageId = string.Empty, ivName = string.Empty;
            string typeOfData = "getwidgetdetails";
            var jObpp = JObject.Parse(publishedpages);
            tPageId = jObpp["data"][0][0].ToString().ToString();
            var jObpwg = JObject.Parse(pagewidgets);
            string inputSqlXML = string.Empty;
            ArrayList lstWGType = new ArrayList();
            foreach (var inData in jObpwg["data"])
            {
                widgetIds = inData[0].ToString();
                widGetType = inData[2].ToString().ToLower();
                //if (widGetType == "widget")
                //    widgetIds = inData[4].ToString().Substring(6);
                //else if (widGetType == "iview")
                if (widGetType == "iview")
                    ivName = inData[4].ToString();
                string sqlQuery = string.Empty;
                string wgDetails = string.Empty;
                if (widGetType == "widget" || widGetType == "iview" || widGetType == "custom__sql" || widGetType == "custom__mytsk")
                    wgDetails = CheckIndWidgetTimeOnLoad(getwidgetdetails, widgetIds, widGetType);
                if (wgDetails != string.Empty && wgDetails.Split('$')[0] == "true")
                {
                    lstWGType.Add(widgetIds + "," + widGetType);
                    sqlQuery = util.CheckSpecialChars(wgDetails.Split('$')[1]);
                    if (widGetType == "widget")
                        inputSqlXML += "<widget sqltext='" + sqlQuery + "'>" + widgetIds + "</widget>";
                    else if (widGetType == "custom__sql")
                        inputSqlXML += "<custom__sql sqltext='" + sqlQuery + "'>" + widgetIds + "</custom__sql>";
                    else if (widGetType == "custom__mytsk")
                        inputSqlXML += "<custom__mytsk sqltext='" + sqlQuery + "'>" + widgetIds + "</custom__mytsk>";
                    else if (widGetType == "iview")
                        inputSqlXML += "<iview iviewname='" + ivName + "' pagesize='' sqltext='" + sqlQuery + "'>" + widgetIds + "</iview>";
                }
            }
            if (inputSqlXML != string.Empty)
            {
                string inputXml = "<sqlresultset pageid='" + tPageId + "' axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                inputXml += inputSqlXML;
                inputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                result = asbExt.CallGetIndividualWidgetDetailsWS(inputXml);
                if (result != string.Empty)
                {
                    dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(result);
                    result = jsonObj["getwidgetdetails"].ToString();
                    getwidgetdetails = UpdateIndWidgetDataOnLoad(result, typeOfData, lstWGType);
                }
            }
        }
        catch (Exception ex)
        {
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logobj.CreateLog("GetIndividualWidgetDetail exception -" + ex.Message, sessID, "GetIndividualWidgetDetail-exception", "new");
        }
        return getwidgetdetails;
    }

    private void SetDataCache(string publishedpages, string pagetemplates, string pagewidgets, string getwidgetdetails)
    {
        try
        {
            FDW fdwObj = new FDW();
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            string lang = "Eng";
            if (HttpContext.Current.Session["language"] != null)
                lang = HttpContext.Current.Session["language"].ToString().Substring(0, 3);

            string ppKey = "publishedpages-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;
            fdwObj.SaveInRedisServer(ppKey, publishedpages, "", schemaName);

            string ptKey = "pagetemplates-" + lang;
            fdwObj.SaveInRedisServer(ptKey, pagetemplates, "", schemaName);

            string pwKey = "pagewidgets-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;
            fdwObj.SaveInRedisServer(pwKey, pagewidgets, "", schemaName);

            string gwdKey = "getwidgetdetails-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;
            fdwObj.SaveInRedisServer(gwdKey, getwidgetdetails, "", schemaName);
        }
        catch (Exception ex) { }
    }

    private string GetCachedData(string datatype)
    {
        string resData = string.Empty;
        try
        {
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            string lang = "Eng";
            if (HttpContext.Current.Session["language"] != null)
                lang = HttpContext.Current.Session["language"].ToString().Substring(0, 3);
            switch (datatype)
            {
                case "publishedpages":
                    string ppKey = "publishedpages-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;
                    resData = fObj.StringFromRedis(ppKey);
                    break;
                case "pagetemplates":
                    string ptKey = "pagetemplates-" + lang;
                    resData = fObj.StringFromRedis(ptKey);
                    break;
                case "pagewidgets":
                    string pwKey = "pagewidgets-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;
                    resData = fObj.StringFromRedis(pwKey);
                    break;
                case "getwidgetdetails":
                    string gwdKey = "getwidgetdetails-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;
                    resData = fObj.StringFromRedis(gwdKey);
                    break;
            }
        }
        catch (Exception ex) { }
        return resData;
    }

    private string UpdateIndWidgetData(string widgetdetails, string typeOfData, string wId, string widGetType)
    {
        string result = string.Empty;
        string cachedWidgetData = GetCachedData(typeOfData);
        if (cachedWidgetData != string.Empty)
        {
            var jObjects = JObject.Parse(cachedWidgetData);
            var jObject = JObject.Parse(widgetdetails)[wId];
            switch (widGetType)
            {
                case "widget":
                    jObjects[wId]["sqldata"] = jObject["data"];
                    jObjects[wId]["sqlmetaData"] = jObject["sqlmetaData"];
                    jObjects[wId]["cache_setup"][0]["datetime"] = DateTime.Now.ToString("ddMMyyyyHHmm");
                    break;
                case "custom__sql":
                    jObjects[wId]["data"] = jObject["data"];
                    jObjects[wId]["sqlmetaData"] = jObject["sqlmetaData"];
                    jObjects[wId]["cache_setup"][0]["datetime"] = DateTime.Now.ToString("ddMMyyyyHHmm");
                    break;
                case "custom__mytsk":
                    jObjects[wId]["data"] = jObject["data"];
                    jObjects[wId]["sqlmetaData"] = jObject["sqlmetaData"];
                    jObjects[wId]["cache_setup"][0]["datetime"] = DateTime.Now.ToString("ddMMyyyyHHmm");
                    break;
                case "iview":
                    jObjects[wId]["data"] = jObject["data"];
                    jObjects[wId]["iviewmetaData"] = jObject["iviewmetaData"];
                    jObjects[wId]["cache_setup"][0]["datetime"] = DateTime.Now.ToString("ddMMyyyyHHmm");
                    break;
            }
            string output = JsonConvert.SerializeObject(jObjects, Formatting.Indented);
            UpdateIndCachedData(output);
            result = "{" + jObjects[wId].Parent.ToString() + "}";
        }
        return result;
    }

    private string UpdateIndWidgetDataOnLoad(string widgetdetails, string typeOfData, ArrayList lstWGType)
    {
        string result = string.Empty;
        string cachedWidgetData = GetCachedData(typeOfData);
        if (cachedWidgetData != string.Empty)
        {
            var jObjects = JObject.Parse(cachedWidgetData);
            foreach (var lstWG in lstWGType)
            {
                string wId = lstWG.ToString().Split(',')[0];
                string widGetType = lstWG.ToString().Split(',')[1];
                var jObject = JObject.Parse(widgetdetails)[wId];
                switch (widGetType)
                {
                    case "widget":
                        jObjects[wId]["sqldata"] = jObject["data"];
                        jObjects[wId]["sqlmetaData"] = jObject["sqlmetaData"];
                        jObjects[wId]["cache_setup"][0]["datetime"] = DateTime.Now.ToString("ddMMyyyyHHmm");
                        break;
                    case "custom__sql":
                        jObjects[wId]["data"] = jObject["data"];
                        jObjects[wId]["sqlmetaData"] = jObject["sqlmetaData"];
                        jObjects[wId]["cache_setup"][0]["datetime"] = DateTime.Now.ToString("ddMMyyyyHHmm");
                        break;
                    case "custom__mytsk":
                        jObjects[wId]["data"] = jObject["data"];
                        jObjects[wId]["sqlmetaData"] = jObject["sqlmetaData"];
                        jObjects[wId]["cache_setup"][0]["datetime"] = DateTime.Now.ToString("ddMMyyyyHHmm");
                        break;
                    case "iview":
                        jObjects[wId]["data"] = jObject["data"];
                        jObjects[wId]["iviewmetaData"] = jObject["iviewmetaData"];
                        jObjects[wId]["cache_setup"][0]["datetime"] = DateTime.Now.ToString("ddMMyyyyHHmm");
                        break;
                }
            }
            string output = JsonConvert.SerializeObject(jObjects, Formatting.Indented);
            UpdateIndCachedData(output);
            result = jObjects.ToString();
        }
        return result;
    }

    private void UpdateIndCachedData(string getwidgetdetails)
    {
        try
        {
            FDW fdwObj = new FDW();
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            string lang = "Eng";
            if (HttpContext.Current.Session["language"] != null)
                lang = HttpContext.Current.Session["language"].ToString().Substring(0, 3);

            string gwdKey = "getwidgetdetails-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;
            fdwObj.SaveInRedisServer(gwdKey, getwidgetdetails, "", schemaName);
        }
        catch (Exception ex) { }
    }

    private string CheckIndWidgetTime(string widgetId, string widGetType)
    {
        string widgetData = string.Empty;
        string cachedWidgetData = GetCachedData(widGetType);
        if (cachedWidgetData != string.Empty)
        {
            var jObjects = JObject.Parse(cachedWidgetData)[widgetId];
            if (jObjects["cache_setup"] != null)
            {
                string sqltext = jObjects["cache_setup"][3]["sqltext"].ToString();
                widgetData = "true$" + sqltext;
            }
            else
                widgetData = "false$" + "{" + jObjects.Parent.ToString() + "}";
        }
        return widgetData;
    }

    private string CheckIndWidgetTimeOnLoad(string cachedWidgetData, string widgetId, string widGetType)
    {
        string widgetData = string.Empty;
        if (cachedWidgetData != string.Empty)
        {
            var jObjects = JObject.Parse(cachedWidgetData)[widgetId];
            if (jObjects != null)
            {
                if (jObjects["cache_setup"] != null)
                {
                    if (jObjects["cache_setup"][0]["cwd"] != null)
                    {
                        string sqltext = jObjects["cache_setup"][1]["sqltext"].ToString();
                        widgetData = "true$" + sqltext;
                    }
                    else
                    {
                        string sqltext = jObjects["cache_setup"][3]["sqltext"].ToString();
                        if (widGetType == "iview" || widGetType == "custom__mytsk")
                            widgetData = "true$" + sqltext;
                        else
                        {
                            string dtTIme = jObjects["cache_setup"][0]["datetime"].ToString();
                            int cei = int.Parse(jObjects["cache_setup"][2]["cei"].ToString());
                            long cdTime = long.Parse(DateTime.ParseExact(dtTIme, "ddMMyyyyHHmm", null).AddMinutes(cei).ToString("ddMMyyyyHHmm"));
                            long crTime = long.Parse(DateTime.Now.ToString("ddMMyyyyHHmm"));
                            if (cdTime <= crTime)
                                widgetData = "true$" + sqltext;
                        }
                    }
                }
            }
        }
        return widgetData;
    }

    [WebMethod(EnableSession = true)]
    public void ClearRedisWidgetData()
    {
        try
        {
            if (ConfigurationManager.AppSettings["homepagews"] != null && ConfigurationManager.AppSettings["homepagews"].ToString() == "false")
                return;
            string lang = "Eng";
            if (HttpContext.Current.Session["language"] != null)
                lang = HttpContext.Current.Session["language"].ToString().Substring(0, 3);
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDW fdwObj = new FDW();
            fdwObj.Initialize(schemaName);

            //string ppKey = schemaName + "-publishedpages-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;
            //string ptKey = schemaName + "-pagetemplates-" + lang;
            //string pwKey = schemaName + "-pagewidgets-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;
            //string gwdKey = schemaName + "-getwidgetdetails-" + HttpContext.Current.Session["username"].ToString() + "-" + lang;

            //fdwObj.DeleteAllKeys(ppKey + "&" + ptKey + "&" + pwKey + "&" + gwdKey, schemaName);

            string ppKey = schemaName + "-publishedpages-*";
            string ptKey = schemaName + "-pagetemplates-*";
            string pwKey = schemaName + "-pagewidgets-*";
            string gwdKey = schemaName + "-getwidgetdetails-*";

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

            ArrayList ppKeykeys = fObj.GetAllKeys(ppKey);
            for (int i = 0; i < ppKeykeys.Count; i++)
            {
                string keyName = ppKeykeys[i].ToString();
                fdwObj.DeleteAllKeys(keyName, schemaName);
            }

            ArrayList ptKeykeys = fObj.GetAllKeys(ptKey);
            for (int i = 0; i < ptKeykeys.Count; i++)
            {
                string keyName = ptKeykeys[i].ToString();
                fdwObj.DeleteAllKeys(keyName, schemaName);
            }

            ArrayList pwKeykeys = fObj.GetAllKeys(pwKey);
            for (int i = 0; i < pwKeykeys.Count; i++)
            {
                string keyName = pwKeykeys[i].ToString();
                fdwObj.DeleteAllKeys(keyName, schemaName);
            }

            ArrayList gwdKeykeys = fObj.GetAllKeys(gwdKey);
            for (int i = 0; i < gwdKeykeys.Count; i++)
            {
                string keyName = gwdKeykeys[i].ToString();
                fdwObj.DeleteAllKeys(keyName, schemaName);
            }
        }
        catch (Exception ex) { }
    }
}
