using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class aspx_iviewBuilder : System.Web.UI.Page
{
    public IviewData objIview;
    Util.Util util;
    LogFile.Log logobj = new LogFile.Log();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();

    public string direction = "ltr";
    public string langType = "en";
    public string language = string.Empty;

    public string schemaName = string.Empty;
    public bool isCloudApp = false;

    public string iName = string.Empty;

    public bool globalGetAjaxIviewData = true;
    public bool getAjaxIviewData = true;
    public bool isMobile = false;

    string fileName;
    public string errLog;

    public string tid = string.Empty;
    public string AxRole = string.Empty;
    public string proj = string.Empty;
    public string sid = string.Empty;
    public string user = string.Empty;

    string clientCulture = null;
    string strGlobalVar = string.Empty;

    private bool IsIviewPop = false;
    string IsSqlPagination = "false";
    public string hidetoolbar = "false";
    public string hideParameters = "false";

    public string axpResp = "false";

    public string tst_Scripts = string.Empty;

    bool callParamPlusStructure = false;
    bool isCache = false;

    string pXml;
    IDictionary<string, string> paramss = new Dictionary<string, string>();
    public static StringBuilder paramssBuilder = new StringBuilder();

    public string refreshTime = string.Empty;

    public string enableBackForwButton = string.Empty;

    public bool paramsExist;
    public bool IsParamsVisible = false;

    ////if iview web service will return row count or not
    public bool getIviewRowCount = false;

    static JObject jsonForGrid = new JObject();
    public dynamic toolbarJSON = new JObject();

    public string redisLoadKey = string.Empty;
    public string redisLoadType = string.Empty;

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
        try
        {
            util = new Util.Util();
            util.IsValidSession();
            ResetSessionTime();

            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString().ToLower() == "true")
                isMobile = true;

            if (Session["project"] == null || Session["axApps"] == null || Convert.ToString(Session["project"]) == string.Empty)
            {
                SessExpires();
                return;
            }
            else
            {
                if (util.IsValidQueryString(Request.RawUrl) == false)
                    HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.INVALIDURL);

                //if (!util.licencedValidSessionCheck())
                //{
                //    HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG, false);
                //    return;
                //}

                if (!(string.IsNullOrEmpty(Session["language"].ToString())))
                {
                    language = Session["language"].ToString();
                }
                else
                {
                    language = string.Empty;
                }

                if (HttpContext.Current.Session["dbuser"] != null)
                {
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                }

                objIview = GetStructDef(proj, sid, user, iName, AxRole);

                objIview.getIviewRowCount = getIviewRowCount;

                GetGlobalVariables();
                CallStructure(iName);

                if (objIview.requestJSON && !objIview.isObjFromCache)
                {
                    try
                    {
                        FDW fdwObj = new FDW();
                        string fdKey = Constants.RedisIviewBuilderObj;
                        string fdSettingsKey = Constants.RedisIviewBuilderSettingsObj;
                        //if (Request.QueryString["tstcaption"] != null)
                        //{
                        //    fdKey = Constants.RedisListviewObj;
                        //    fdSettingsKey = Constants.RedisListviewSettings;
                        //}
                        fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKey, iName), objIview, fdKey, schemaName);
                        if (objIview.smartviewSettings != "")
                        {
                            fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdSettingsKey, iName, user), objIview.smartviewSettings, fdSettingsKey, schemaName);
                        }
                    }
                    catch (Exception ex) { }
                }

                else if (globalGetAjaxIviewData)
                {
                    getAjaxIviewData = true;
                }
                else
                {
                    getAjaxIviewData = false;
                }
                enableBackForwButton = "<script type=\'text/javascript\' > enableBackButton='" + Convert.ToBoolean(Session["enableBackButton"]) + "';" + " enableForwardButton='" + Convert.ToBoolean(Session["enableForwardButton"]) + "'; isSqlPagination='" + IsSqlPagination + "';var axTheme='" + Session["themeColor"].ToString() + "';var paramsExist=" + paramsExist.ToString().ToLower() + ";var isParamVisible='" + IsParamsVisible + "';var isFilterEnabled='" + objIview.iviewParams.IsFilterEnabled.ToString() + "';</script>";
                //if (ConfigurationManager.AppSettings["isCloudApp"] != null)
                //{
                //    isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString());
                //}

                XmlDocument structureXmlDoc = new XmlDocument();
                structureXmlDoc.LoadXml(objIview.StructureXml.ToString());
                string structureJSON = JsonConvert.SerializeXmlNode(structureXmlDoc);

                Page.ClientScript.RegisterStartupScript(GetType(), "set global var in iview", "<script>var isCloudApp = '" + isCloudApp.ToString() + "';var getIviewRowCount = " + getIviewRowCount.ToString().ToLower() + ";var iviewDataWSRows = " + objIview.iviewDataWSRows.ToString() + ";var totRowCount = '';</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ivirHeaderData", "var ivirHeaderData = " + jsonForGrid + ";var getAjaxIviewData=" + getAjaxIviewData.ToString().ToLower() + ";var isListView=" + (objIview.purposeString == "" ? "false" : "true") + ";var showParam=" + objIview.iviewParams.showParam.ToString().ToLower() + ";var iviewWrap = " + util.IviewWrap.ToString().ToLower() + ";var isIviewPopup = " + IsIviewPop.ToString().ToLower() + ";var toolbarJSON=JSON.stringify(" + toolbarJSON.ToString().Replace("\r\n", "") + ");var requestJSON = " + objIview.requestJSON.ToString().ToLower() + ";var redisLoadType = '" + redisLoadType.ToString() + "';var redisLoadKey = '" + redisLoadKey.ToString() + "';var breadCrumbStr = '" + objIview.Menubreadcrumb.ToString() + "';var structureJSON = " + structureJSON.ToString().Replace("\r\n", "") + ";", true);
            }
        }
        catch (Exception ex) { }
    }

    private string CallStructure(string iName)
    {
        string returnStructure = string.Empty;

        fileName = "openiview-" + iName;
        errLog = logobj.CreateLog("Loading iview.aspx before post back (get and set parameters)", sid, fileName, "new");

        logobj.CreateLog("Get Iview Parameters", sid, fileName, string.Empty);



        string ires = string.Empty;
        string istructure = string.Empty;
        string fdKeyIVIEWPARAM = Constants.IVIEWPARAM;
        string fdKeyIVIEWSTRUCT = Constants.IVIEWSTRUCT;
        bool cacheParamsXML = false;
        string ivupdateOn = string.Empty;
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

        string iXml = string.Empty;
        DateTime asStart = DateTime.Now;
        errLog = logobj.CreateLog("Call to GetParams Web Service", sid, fileName, string.Empty);
        iXml = "<root " + objIview.purposeString + " name =\"" + iName + "\" axpapp = \"" + proj + "\" sessionid = \"" + sid + "\" appsessionkey=\"" + HttpContext.Current.Session["AppSessionKey"].ToString() + "\" username=\"" + HttpContext.Current.Session["username"].ToString() + "\" trace = \"" + errLog + "\" firsttime=\"" + callParamPlusStructure.ToString().ToLower() + "\"  >";
        //GetParams input xml will contain the query string values passed by previous iview
        ConstructParamXml();
        iXml += pXml;
        iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        ires = objWebServiceExt.CallGetParamsWS(iName, iXml, objIview);
        if (ires != string.Empty)
        {
            ires = ires.Split('♠')[1];
            if (ires.StartsWith("<"))
                ires = util.ReplaceFirstOccurrence(ires, "#$#", "#$♥#");
            string[] splitRes = ires.Split(new[] { "#$♥#" }, StringSplitOptions.None);
            if (splitRes.Length > 0)
            {
                objIview.iviewParams.ParamXML = ires = splitRes[0];
            }
            if (splitRes.Length > 1)
            {
                objIview.AccessControlXml = splitRes[1];
            }
            if (splitRes.Length > 2)
            {
                objIview.StructureXml = istructure = splitRes[2];
            }
            if (splitRes.Length > 3)
            {
                objIview.DsIviewConfig = processXmlConfigAsDt(splitRes[3]);
            }
        }

        string errMsg = string.Empty;
        errMsg = util.ParseXmlErrorNode(ires);
        if (errMsg != string.Empty)
        {
            if (errMsg == Constants.SESSIONERROR)
            {
                Session.RemoveAll();
                Session.Abandon();
                SessExpires();
                return "";
            }
            else if (errMsg == Constants.SESSIONEXPMSG)
            {
                SessExpires();
                return "";
            }
            else if (errMsg == Constants.ERAUTHENTICATION)
            {
                Response.Redirect(util.ERRPATH + Constants.ERAUTHENTICATION);
            }
            else
            {
                Response.Redirect(util.ERRPATH + errMsg);
            }
        }


        tst_Scripts = tst_Scripts + "<script type=\"text/javascript\">var hideParams='true';var proj = '" + proj + "';var user='" + user + "';var AxRole='" + AxRole + "'; var sid='" + sid + "';var iName='" + iName + "'; gl_language='" + HttpContext.Current.Session["language"].ToString() + "'; validateParamOnGo='false'; " + strGlobalVar + "; var axpResp = '" + axpResp + "';</script>";

        if (!String.IsNullOrEmpty(objIview.iviewParams.ParamXML))
        {
            returnStructure = objIview.iviewParams.ParamXML;
        }

        return returnStructure;
    }

    private void GetGlobalVariables()
    {
        //if (Application["ValidateIviewParamOnGo"] != null)
        //    validateParamOnGo = Convert.ToBoolean(Application["ValidateIviewParamOnGo"].ToString());
        //else
        //    validateParamOnGo = false;

        iName = Request.QueryString["ivname"];

        if (!string.IsNullOrEmpty(iName))
        {
            if (iName.Contains(":"))
            {
                string viewName = iName.Substring(iName.LastIndexOf(':') + 1);
                iName = iName.Substring(0, iName.LastIndexOf(":"));
                objIview.MyViewName = viewName;
            }

        }
        iName = util.CheckSpecialChars(iName);
        if (iName == null || iName == string.Empty)
        {
            if (objIview.Ivname != null && Convert.ToString(objIview.Ivname) != string.Empty)
            {
                iName = objIview.Ivname.ToString();
            }
            else if (Request.Form["ivname"] != null)
            {
                iName = Request.Form["ivname"].ToString();
            }
        }

        if (!util.IsValidIvName(iName))
            Response.Redirect(Constants.PARAMERR);

        if (iName != null)
        {
            objIview.IName = iName;
            objIview.Ivname = iName;
        }
        else
        {
            iName = objIview.IName.ToString();
        }

        objIview.IName = iName;
        Session["iName"] = iName;
        if (Request.QueryString["ivname"] != null && Request.QueryString["ivname"] != string.Empty)
        {
            tid = Request.QueryString["ivname"].ToString();
        }
        tid = util.CheckSpecialChars(tid);
        objIview.Tid = tid;
        AxRole = Session["AxRole"].ToString();
        AxRole = util.CheckSpecialChars(AxRole);
        proj = Session["project"].ToString();
        proj = util.CheckSpecialChars(proj);
        sid = Session["nsessionid"].ToString();
        sid = util.CheckSpecialChars(sid);
        user = Session["user"].ToString();
        user = util.CheckSpecialChars(user);


        if (Session["ClientLocale"] != null)
            clientCulture = Convert.ToString(Session["ClientLocale"]);


        if (string.IsNullOrEmpty(clientCulture))
            clientCulture = "en-gb";

        strGlobalVar = util.GetGlobalVarString();
        CreateGlobalVarArray(strGlobalVar);

        if (HttpContext.Current.Session["AxDbPagination"] != null && HttpContext.Current.Session["AxDbPagination"].ToString() == "")
            IsSqlPagination = "true";
        else if (HttpContext.Current.Session["AxDbPagination"] != null && HttpContext.Current.Session["AxDbPagination"].ToString() != "")
            IsSqlPagination = Session["AxDbPagination"].ToString().ToLower();
        //if IView is opened as popup, the hyperlinks in the popup will not support navigation
        if (Session["AxIsPop"] != null)
        {
            if (Convert.ToString(Session["AxIsPop"]) == "IviewPop")
            {
                IsIviewPop = true;
                Session["AxIsPop"] = string.Empty;
            }
        }

        if (Session["AxFromHypLink"] != null)
            objIview.FromHyperLink = Session["AxFromHypLink"].ToString();
        CheckCustomIview();

        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();

        //if (iName == "inmemdb")
        //{
        //    directiview = false;
        //}
    }

    private void CheckCustomIview()
    {
        if (Session["AxCustomIviews"] != null)
        {
            //the value can be ivname:t,ivname:p,ivname:*
            string customIviews = Session["AxCustomIviews"].ToString();
            if (customIviews != string.Empty)
            {
                string[] arrCusIv = customIviews.Split(',');
                for (int i = 0; i < arrCusIv.Length; i++)
                {
                    int idx = arrCusIv[i].ToString().IndexOf(":");
                    if (idx != -1)
                    {
                        if (arrCusIv[i].ToString().Substring(0, idx) == iName)
                        {
                            string str = arrCusIv[i].ToString().Substring(idx + 1);
                            str = str.Trim().ToLower();
                            if (str == "t")
                                hidetoolbar = "true";
                            else if (str == "p")
                                hideParameters = "true";
                            else if (str == "*")
                            {
                                hideParameters = "true";
                                hidetoolbar = "true";
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    private void CreateGlobalVarArray(string strGlobalVar)
    {
        string globalVar = strGlobalVar;
        if (globalVar != string.Empty)
        {
            string[] globalVarArr = new string[globalVar.Split(';').Length];
            globalVarArr = globalVar.Split(';');
            foreach (var glvar in globalVarArr)
            {
                if (glvar != string.Empty || (glvar != " ") || (glvar != null))
                {
                    if (glvar.Length > 0 && glvar.Contains('='))
                    {
                        string glvarNameval = glvar.Split('=')[1].Replace('"', ' ').Trim();

                        if (objIview.GolbalVarName != null && objIview.GolbalVarValue != null)
                        {
                            objIview.GolbalVarName.Add(glvarNameval.Split('~')[0]);
                            objIview.GolbalVarValue.Add(glvarNameval.Split('~')[1]);
                        }
                    }
                }
            }
        }

    }

    private DataTable processXmlConfigAsDt(string configurationXml)
    {
        DataTable returnDt = new DataTable();

        try
        {
            FDW fdwObj = new FDW();
            if (configurationXml != string.Empty)
            {
                logobj.CreateDirectDBLog("ConfigurationXML- " + iName, "ConfigurationXML", "", "IView Name :  " + iName, "ConfigurationXML : " + Environment.NewLine + Environment.NewLine + configurationXml);

                configurationXml = configurationXml.Replace("<axconfig>", "<dataSet>").Replace("</axconfig>", "</dataSet>").Replace("<row>", "<table0>").Replace("</row>", "</table0>");

                bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
                string axpStructKeyIview = Constants.AXCONFIGIVIEW; //Constants.AXCONFIGURATIONS;       
                string axpConfigTableIview = Constants.AXNODATACONFIGIVIEW; //Constants.AXCONFIGURATIONTABLE;

                DataTable axpConfigStrIview = new DataTable();
                bool isAxpConfig = true;
                string axpConfigTblIview = string.Empty;

                DataSet dsConfig = new DataSet();
                StringReader strReader = new StringReader(configurationXml);
                dsConfig.ReadXml(strReader);

                if (dsConfig.Tables["Table0"] == null || dsConfig.Tables["Table0"].Rows.Count == 0)
                {
                    isAxpConfig = false;
                }

                if ((axpConfigStrIview == null || axpConfigStrIview.Rows.Count == 0) && (isAxpConfig == false || (isAxpConfig && (dsConfig == null || dsConfig.Tables.Count == 0 || dsConfig.Tables["Table0"].Rows.Count == 0))) && axpConfigTblIview == string.Empty && isRedisConnected)
                    fdwObj.SaveInRedisServer(util.GetNoDataConfigCacheKey(axpConfigTableIview, "", iName, AxRole, "ALL"), "NoData", axpConfigTableIview, schemaName);


                if (isRedisConnected)
                {
                    if (dsConfig.Tables.Count > 0 && dsConfig.Tables["Table0"] != null && dsConfig.Tables["Table0"].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt = dsConfig.Tables[0];
                        fdwObj.SaveInRedisServerDT(util.GetConfigCacheKey(axpStructKeyIview, "", iName, AxRole, "ALL"), dt, axpStructKeyIview, schemaName);
                        axpConfigStrIview = dt;
                    }

                }
                else
                    axpConfigStrIview = dsConfig.Tables[0];

                if ((axpConfigStrIview != null) && (axpConfigStrIview.Rows.Count > 0))
                {
                    objIview.DsIviewConfig = axpConfigStrIview;
                    returnDt = axpConfigStrIview;
                    objIview.GetAxpStructConfig(objIview, "");
                }
            }
        }
        catch (Exception ex) { }

        return returnDt;
    }

    ///Function to construct the parameter xml on which the iview is dependent.
    protected void ConstructParamXml()
    {
        string str = string.Empty;
        ResetParamsOnSaveCHWindow(false);
        str = hdnparamValues.Value;
        string[] strp = util.AxSplit1(str, "¿");
        int i = 0;
        param.Value = string.Empty;
        for (i = 0; i <= strp.Length - 1; i++)
        {
            if (!string.IsNullOrEmpty(strp[i]))
            {
                string[] arrparam = strp[i].ToString().Split('~');
                string pName = util.CheckSpecialChars(arrparam[0].ToString());
                string pValue = util.CheckSpecialChars(arrparam[1].ToString());
                if (pValue.Contains("&amp;grave;") == true)
                {
                    pValue = pValue.Replace("&amp;grave;", "~");
                }
                pXml = pXml + "<" + pName + ">";
                pXml = pXml + pValue;
                pXml = pXml + "</" + pName + ">";

                if (pName == "mrefresh")
                {
                    if (pValue != "")
                        refreshTime = pValue;
                }

                UpdateParamValues(pName, pValue);
                if (objIview.iviewParams.ParamCaption.Count > 0 && objIview.iviewParams.ParamCaption.ToString() != "")
                {
                    paramss[objIview.iviewParams.ParamCaption[i].ToString()] = pValue;
                }

                pValue = pValue.Replace("~", "&grave;");
                if (!string.IsNullOrEmpty(param.Value))
                {
                    param.Value = param.Value + "~" + pName + "♠" + pValue;
                }
                else
                {
                    param.Value = pName + "♠" + pValue;
                }
            }
        }
        //write code to display the param selected values in the label
        int iCount = 0;
        if (objIview.iviewParams.ParamCaption.Count > 0 && objIview.iviewParams.ParamCaption.ToString() != "")
        {
            paramssBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> entry in paramss)
            {

                if (!string.IsNullOrEmpty(entry.Key) && !string.IsNullOrEmpty(entry.Value))
                {
                    iCount++;

                    paramssBuilder.Append("<tr><td><b>" + entry.Key + "</b> </td><td>: " + entry.Value + "</td></tr>");
                }
            }



            string paramValuesShown = string.Empty;
            paramValuesShown = paramssBuilder.ToString();

            paramValuesShown = "<span class=\"filtertooltip\" style='text-decoration: none;'>" + iCount + "<span class=\"tooltiptext\"><table>" + paramValuesShown + "</table></span></span>";
            //FilterValues.InnerHtml = paramValuesShown.ToString();

        }
    }

    private void ConstructHdnParamValues()
    {
        StringBuilder pValue = new StringBuilder();
        int i = 0;
        for (i = 0; i < objIview.iviewParams.ParamNames.Count; i++)
        {
            pValue.Append(objIview.iviewParams.ParamNames[i] + "~" + objIview.iviewParams.ParamChangedVals[i] + "¿");
        }
        hdnparamValues.Value = pValue.ToString();
    }

    private void ResetParamsOnSaveCHWindow(bool delFromSession)
    {
        if (Session["IsFromChildWindow"] != null)//&& Session["IsFromChildWindow"] == "true"
        {

            if (Request.QueryString["ivname"] != null)
                iName = Request.QueryString["ivname"];
            if (Session["paramValues" + iName] != null)
            {
                hdnparamValues.Value = Session["paramValues" + iName].ToString();
                hdnSelParamsAftrChWin.Value = Session["paramValues" + iName].ToString();
            }
            if (delFromSession)
                Session["IsFromChildWindow"] = null;
        }
    }

    private void UpdateParamValues(string pName, string pValue)
    {
        int idx = -1;
        idx = objIview.iviewParams.ParamNames.IndexOf(pName);
        if (idx == -1)
        {
            objIview.iviewParams.ParamNames.Add(pName);
            objIview.iviewParams.ParamValsOnLoad.Add(pValue);
            objIview.iviewParams.ParamChangedVals.Add(pValue);
        }
        else
        {
            objIview.iviewParams.ParamChangedVals[idx] = pValue;
        }
    }

    public IviewData GetStructDef(string projectName, string sessionID, string userName, string iName, string AxRole)
    {
        IviewData returnIview = null;
        try
        {
            string fdKey = Constants.RedisIviewBuilderObj;
            //if (Request.QueryString["tstcaption"] != null)
            //{
            //    fdKey = Constants.RedisListviewObj;
            //}

            string fileName = "opentiview-" + iName;

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
            {
                fObj = new FDR();
                HttpContext.Current.Session["FDR"] = fObj;
            }
            logobj.CreateLog("Get IviewObj from Cache, User: " + userName + " Role: " + AxRole, sessionID, fileName, "");

            returnIview = (IviewData)fObj.IviewObjFromRedis(util.GetRedisServerkey(fdKey, iName));

            if (returnIview != null)
            {
                string ivupdateOn = string.Empty;

                if (returnIview.StructureXml != string.Empty && returnIview.StructureXml != "false")
                {
                    isCache = true;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(returnIview.StructureXml);
                    if (xmlDoc.DocumentElement.Attributes["updatedon"] != null)
                    {
                        ivupdateOn = xmlDoc.DocumentElement.Attributes["updatedon"].Value;
                    }
                }
                bool isStructureUpdated = ivupdateOn != string.Empty && returnIview.IsStructureUpdated(ivupdateOn, iName);
                if (!isStructureUpdated)
                {
                    returnIview.isObjFromCache = true;

                    string fdSettingsKey = Constants.RedisIviewBuilderSettingsObj;
                    //if (Request.QueryString["tstcaption"] != null)
                    //{
                    //    fdKey = Constants.RedisListviewObj;
                    //    fdSettingsKey = Constants.RedisListviewSettings;
                    //}

                    string smartviewSettingsTemp = fObj.StringFromRedis(util.GetRedisServerkey(fdSettingsKey, iName, user));

                    if (smartviewSettingsTemp != "")
                    {
                        returnIview.smartviewSettings = smartviewSettingsTemp;
                    }

                    logobj.CreateLog("Getting IviewObj from cache " + userName + " Role: " + AxRole, sessionID, fileName, "");
                }
                else
                {
                    returnIview = new IviewData();
                }
            }
            else
            {
                returnIview = new IviewData();
            }
        }
        catch (Exception ex)
        {
            returnIview = new IviewData();
        }
        return returnIview;
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", true);
        }
    }

    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    [WebMethod]
    public static string getHyperlinkInfo(string type, string ivTstName)
    {
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        string sql = string.Empty;
        string result = string.Empty;
        try
        {

            if (type == "i")
            {
                if (ivTstName == string.Empty)
                {
                    //sql = "select a.name as value,a.caption from iviews a";
                    sql = "select a.name as value,a.caption from iviews a inner join (select c.iname from iviewparams c group by c.iname) as b on a.name=b.iname group by a.name,a.caption order by a.caption";
                }
                else
                {
                    sql = "select a.pname as value,a.pcaption as caption from iviewparams a where iname='" + ivTstName + "' order by a.pcaption";
                }
            }
            else if (type == "t")
            {
                if (ivTstName == string.Empty)
                {
                    //sql = "select a.name as value,a.caption from tstructs a";
                    sql = "select a.name as value,a.caption from tstructs a inner join (select c.tstruct from axpflds c group by c.tstruct) as b on a.name=b.tstruct group by a.name,a.caption order by a.caption";
                }
                else
                {
                    sql = "select a.fname as value,a.caption from axpflds a where a.tstruct='" + ivTstName + "' order by a.caption";
                }
            }


            result = asbExt.ExecuteSQL("", sql, "JSON");

        }
        catch (Exception ex) { }
        return result;

    }
}
