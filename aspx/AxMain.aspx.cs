using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using LogFile;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.EnterpriseServices;
using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Util;
using Word.Api.Interfaces;

public partial class AxMain : System.Web.UI.Page
{
    public string appTitle = string.Empty;
    public string language = string.Empty;
    public string direction = "ltr";
    public string langType = "en";
    public string glCulture = string.Empty;
    public string redisUtl = string.Empty;
    public string hdnpsid = string.Empty;
    public string AxRole = string.Empty;
    public string axMainPageReload = String.Empty;
    public bool compressedMode = true;
    public string googleMapsApiKey = string.Empty;
    public string hybridGUID = "";
    public string hybridDeviceId = "";
    public string hybridDefaultPage = string.Empty;
    public string ShowImaWidActionBtn = "true";
    public string homepagews = string.Empty;
    public string navigationshow = "true";
    public string requestProcess_logtime = string.Empty;
    public string serverprocesstime = string.Empty;
    public string hdnSSTime = string.Empty;
    public string axpProjectCaption = string.Empty;
    public string isMobDevice = "true";
    public string timeZone = string.Empty;
    public string sid = string.Empty;
    bool isResultXml = false;
    public bool manage = false;
    public string traceStatus = "F";
    public string regthmdata = string.Empty;
    public string proj = string.Empty;
    public string user = string.Empty;
    public string userName = string.Empty;
    public string AxRoles = string.Empty;
    public string Navigationpage = "";
    public bool isCloudApp = false;
    public string scriptsPath = string.Empty;
    public string RestDllPath = string.Empty;
    public string ArmRestDllPath = string.Empty;
    public string axUtlGlobalVars = String.Empty;
    public string axUtlUserVars = String.Empty;
    public string axUtlApps = String.Empty;
    int alertsTimeout = 3000;
    int errorTimeout = 0;
    bool errorEnable = false;
    string notifyTimeout = string.Empty;
    string disableSplit = "false";
    public string axOldModelFlag = "false";
    public string axListViewAsDefault = string.Empty;
    ArrayList globalVariables = new ArrayList();
    ArrayList userVariables = new ArrayList();
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string authKey = string.Empty;
            string encAuth = string.Empty;
            string pName = string.Empty;
            string pParams = string.Empty;
            if (Request.QueryString["authKey"] != null)
                authKey = Request.QueryString["authKey"].ToString();

            if (Request.QueryString["signout"] != null && Request.QueryString["signout"].ToString() == "true")
            {
                if (Session["project"] != null)
                {
                    SignOut();
                }
                return;
            }

            if (Request.QueryString["encAuth"] != null)
                encAuth = util.encrtptDecryptAES(Request.QueryString["encAuth"].ToString(), false);
            if (Request.QueryString["pname"] != null)
                pName = Request.QueryString["pname"].ToString();
            if (Request.QueryString["params"] != null)
            {
                pParams = Request.QueryString["params"].ToString();
                pParams = "&" + pParams.Replace("~", "=").Replace("^", "&");
            }

            if (Request.QueryString["mailapproval"] != null && Request.QueryString["mailapproval"].ToString().ToLower() == "true")
            {
                Session.Remove("project");
            }

            if (encAuth != string.Empty)
            {
                dynamic dynJson = JsonConvert.DeserializeObject(encAuth);
                //string privateKey = dynJson.privatekey.Value;
                //string secret = dynJson.secret.Value;
                proj = dynJson.project.Value;
                userName = dynJson.username.Value;
                language = dynJson.lang.Value;
                AxRoles = dynJson.roles.Value;//"default";//
                //hybridGUID = dynJson.hguid.Value;
                //hybridDeviceId = dynJson.hdeviceid.Value;

                pName = dynJson.pName.Value;

                if (Session["project"] == null)
                {
                    OpenNewSession(AxRoles, proj, userName, language);
                }
                else
                {
                    GetSessionVars();
                    LoadAppConfiguration(proj);
                }

                GetConfigs();
                DoUtlEncode();

                if (pName != null && pName != string.Empty)
                {
                    hdHomeUrl.Value = pName;
                }
            }
            else if (Session["project"] == null)
            {
                if (authKey != string.Empty)
                {
                    proj = authKey.Split('-')[2];
                    FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                    if (fObj == null)
                    {
                        fObj = new FDR(proj);
                    }

                    string authKeyInof = fObj.ReadKeyStringValueNoSchema(authKey);
                    if (Request.QueryString["mailapproval"] != null && Request.QueryString["mailapproval"].ToString().ToLower() == "true")
                    {
                        FDW fdwObj = new FDW(proj);
                        fdwObj.Deletekey(authKey);
                    }

                    if (!string.IsNullOrEmpty(authKeyInof))
                    {
                        dynamic dynJson = JsonConvert.DeserializeObject(authKeyInof);
                        string privateKey = dynJson.privatekey.Value;
                        string secret = dynJson.secret.Value;
                        proj = dynJson.project.Value;
                        userName = dynJson.username.Value;
                        language = dynJson.lang.Value;
                        AxRoles = dynJson.roles.Value;
                        hybridGUID = dynJson.hguid.Value;
                        hybridDeviceId = dynJson.hdeviceid.Value;


                        if (proj != null && proj != "")
                            util.GetAxARMConnection(proj);

                        string agileconnect = "";
                        if (Session["ARM_PrivateKey"] != null)
                            agileconnect = Session["ARM_PrivateKey"].ToString();
                        string hashKey = MD5Hash(agileconnect + secret);
                        if (privateKey != hashKey)
                        {
                            logobj.CreateLog("AxMain authentication error. privateKey:" + privateKey + " hashKey:" + hashKey, Session.SessionID, "axmainauth", "", "true");
                            Page.ClientScript.RegisterStartupScript(GetType(), "opensessionerror", "<script>authError('Authentication failed please try again.')</script>");
                            return;
                        }
                    }
                    else
                    {
                        logobj.CreateLog("AxMain authentication error. authKeyInof from redis:" + authKeyInof, Session.SessionID, "axmainauth-authKeyInof", "", "true");
                        Page.ClientScript.RegisterStartupScript(GetType(), "opensessionerror", "<script>authError('Authentication failed please try again.')</script>");
                        return;
                    }
                }
                else
                {
                    logobj.CreateLog("AxMain authentication error. authKey key should not be empty or null.", Session.SessionID, "axmainauth", "", "true");
                    //Page.ClientScript.RegisterStartupScript(GetType(), "opensessionerror", "<script>authError('Authentication failed please try again.')</script>");
                    string url = util.SESSEXPIRYPATH;
                    url = url + "?axmain=true";
                    Response.Write("<script language='javascript'>");
                    Response.Write("parent.parent.location.href='" + url + "';");
                    Response.Write("</script>");
                    return;
                }
                OpenNewSession(AxRoles, proj, userName, language);
                GetConfigs();
                DoUtlEncode();
                SetLandingPage(pName, pParams);
            }
            else
            {
                GetSessionVars();
                LoadAppConfiguration(proj);
                GetConfigs();
                DoUtlEncode();
                SetLandingPage(pName, pParams);
            }
            InitSessExpiryAlert();
            Page.ClientScript.RegisterStartupScript(GetType(), "axmain vars", "<script>var navigatePage='" + Navigationpage + "';" + "var isCloudApp = '" + isCloudApp.ToString() + "'; " + "var userResp = '" + Session["AxResponsibilities"] + "';" + "var sesslanguage='" + language + "';" + "var mainRestDllPath='" + RestDllPath + "';" + "var mainArmRestDllPath='" + ArmRestDllPath + "';" + "var mainProject='" + proj + "';" + "var mainSessionId='" + sid + "';" + "var mainUserName='" + Session["user"].ToString() + "';" + "var AxHelpIview='';" + " var axUtlGlobalVars='" + axUtlGlobalVars + "'; var axUtlUserVars='" + axUtlUserVars + "'; var axUtlApps='" + axUtlApps + "';var alertsTimeout='" + alertsTimeout.ToString() + "';var errorTimeout='" + errorTimeout.ToString() + "';var errorEnable='" + errorEnable.ToString().ToLower() + "';var notifyTimeout ='" + notifyTimeout + "';var appDiableSplit ='" + disableSplit + "'" + "</script>");

            if (Request.QueryString["mailapproval"] != null && Request.QueryString["mailapproval"].ToString().ToLower() == "true")
            {
                Session["mailapproval"] = "true";
            }
        }
    }

    #region Open Session Service and Variables
    protected void OpenNewSession(string AxRoles, string proj, string userName, string axlanguage)
    {

        string lang_at = "";
        if (axlanguage != null && axlanguage.ToUpper() != "ENGLISH")
            lang_at = " lang=\"" + axlanguage + "\"";
        string lic_redis = GetServerLicDetails();
        if (lic_redis.StartsWith("error:"))
            lic_redis = "";
        string ipaddress = util.GetIpAddress();
        string browserDetails = GetBrowserDetails();
        string scriptsPath = "";
        if (ConfigurationManager.AppSettings["scriptsUrlPath"] != null)
            scriptsPath = ConfigurationManager.AppSettings["scriptsUrlPath"].ToString();
        string loginTrace = ConfigurationManager.AppSettings["LoginTrace"].ToString();

        sid = Session.SessionID;
        Random rand = new Random();
        string rnd_key = rand.Next(1000, 9999).ToString();

        util.GetAxApps(proj);
        string axApps = HttpContext.Current.Session["axApps"].ToString();
        string axProps = HttpContext.Current.Application["axProps"].ToString();

        LogFile.Log logobj = new LogFile.Log();
        string errlog = string.Empty;
        if (loginTrace.ToLower() == "true")
            errlog = logobj.CreateLog("Call to Login Web Service", sid, "OpenSession", "", "true");
        else
            errlog = logobj.CreateLog("Call to Login Web Service", sid, "OpenSession", "");

        string loginXml = "<login userroles='" + AxRoles + "' " + lang_at + " clouddb='' " + lic_redis + " ip='" + ipaddress + "' other='" + browserDetails + "' timediff='0' seed='" + rnd_key + "'  axpapp='" + proj + "' sessionid='" + sid + "' username='" + userName + "' password='' url='' direct='t' scriptpath='" + scriptsPath + "' trace='" + errlog + "'>" + axApps + axProps + "</login>";
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        string result = objWebServiceExt.CallOpenSessionWS(loginXml);
        if (result == string.Empty || result.StartsWith(Constants.ERROR) || result.Contains(Constants.ERROR))
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "opensessionerror", "<script></script>");
            //Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG);
            return;
        }
        else
        {
            Session["isAxMain"] = "true";
            Session["project"] = proj;
            Session["user"] = userName;
            Session["username"] = userName;
            user = userName;
            Session["pwd"] = "";
            Session["nsessionid"] = Session.SessionID;
            sid = Session.SessionID;
            Session["language"] = axlanguage;
            Session["axp_language"] = axlanguage;
            Session["MobileView"] = "True";
            timeZone = "";
            Session["hybridGUID"] = hybridGUID;
            Session["hybridDeviceId"] = hybridDeviceId;
            hybridDefaultPage = "";
            Session["userDetails"] = "";
            Session["isSSOLogin"] = "false";
            Session["SSOLoginType"] = "";
            Session["staySignedId"] = "false";
            Session["Svrlic_redis"] = lic_redis;
            Session["loggedBroserId"] = "";
            Session["validated"] = "True";
            language = axlanguage;
            LoadAppConfiguration(proj);
            ValidatePage(result);
            Session["MenuData"] = "";
            string dirLang = util.SetCulture(axlanguage.ToUpper());
            if (!string.IsNullOrEmpty(dirLang))
            {
                direction = dirLang.Split('-')[0];
                langType = dirLang.Split('-')[1];
                Application["LangSess"] = axlanguage;
            }
            FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/Js/lang/content-" + langType + ".js"));
            if (!(filcustom.Exists))
            {
                langType = "en";
                direction = "ltr";
            }

            string licType = "limited";
            Session["lictype"] = "limited";
            string newUser = user + "♦" + sid + "♣" + licType;
            util.SetDupUserInfo(proj, user, newUser);
        }
    }
    private string GetServerLicDetails()
    {
        string licdetails = string.Empty;
        try
        {
            string redisIp = string.Empty;
            string redisPwd = string.Empty;
            //if (ConfigurationManager.AppSettings["axpLic_RedisIp"] != null)
            //    redisIp = ConfigurationManager.AppSettings["axpLic_RedisIp"].ToString();

            //if (ConfigurationManager.AppSettings["axpLic_RedisPass"] != null)
            //    redisPwd = ConfigurationManager.AppSettings["axpLic_RedisPass"].ToString();
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["axpLic_RedisIP"] != null && HttpContext.Current.Session["axpLic_RedisIP"].ToString() != "")
            {
                redisIp = HttpContext.Current.Session["axpLic_RedisIP"].ToString();
                if (HttpContext.Current.Session["axpLic_RedisPwd"] != null && HttpContext.Current.Session["axpLic_RedisPwd"].ToString() != "")
                    redisPwd = HttpContext.Current.Session["axpLic_RedisPwd"].ToString();
            }
            else
            {
                string rcDetails = util.GetAxpLicRedisConnDetails();
                if (rcDetails != "")
                {
                    redisIp = rcDetails.Split('♣')[0];
                    redisPwd = rcDetails.Split('♣')[1];
                }
            }

            if (redisIp != string.Empty)
            {
                string rlicConn = util.GetServerLicDetails(redisIp, redisPwd);
                switch (rlicConn)
                {
                    case "notConnected":
                        licdetails = "error:Redis Connection details for Axpert license is not proper. Please contact your support person.";
                        break;
                    case "keyNotExists":
                        licdetails = "error:Server seems to be not licensed. Please contact your support person.";
                        break;
                    case "keyNotMatch":
                        licdetails = "error:Redis IP for Axpert license should be set as 127.0.0.1. Please contact your support person.";
                        break;
                    case "keyExists":
                        if (redisPwd != string.Empty)
                            redisPwd = util.EncryptPWD(redisPwd);
                        licdetails = "lic_redis='" + redisIp + "~" + redisPwd + "'";
                        break;
                }
            }
            else
                licdetails = string.Empty;
        }
        catch (Exception ex)
        {
        }
        return licdetails;
    }
    private string GetBrowserDetails()
    {
        System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
        string browserDetails = string.Empty;
        if (isMobDevice == "True")
        {
            string lattitude = "", longitude = "";
            if (hybridGUID != string.Empty)
            {
                string hybridInfo = string.Empty;
                try
                {
                    FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                    if (fObj == null)
                    {
                        fObj = new FDR(proj);
                    }
                    hybridInfo = fObj.StringFromRedis(util.GetRedisServerkey(Constants.AXHYBRIDINFO, hybridGUID), "♠");

                }
                catch (Exception ex) { }

                if (hybridInfo != string.Empty)
                {
                    try
                    {
                        JObject parsedHybridInfo = JObject.Parse(hybridInfo);
                        lattitude = parsedHybridInfo["location"]["coords"]["latitude"].ToString();
                        longitude = parsedHybridInfo["location"]["coords"]["longitude"].ToString();

                    }
                    catch (Exception ex) { }
                }
                browserDetails = "hyb" + "~" + hybridGUID + "~" + lattitude + "~" + longitude + "¿" + timeZone + "¿" + browser.Type + "¿" + browser.Browser + "¿"
                   + browser.Version + "¿" + browser.MajorVersion + "¿"
                   + browser.MinorVersion + "¿" + browser.Platform + "¿"
                   + HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            }
            else
            {
                browserDetails = "hyb¿" + timeZone + "¿" + browser.Type + "¿" + browser.Browser + "¿"
                    + browser.Version + "¿" + browser.MajorVersion + "¿"
                    + browser.MinorVersion + "¿" + browser.Platform + "¿"
                    + HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            }
        }
        else
        {
            browserDetails = timeZone + "¿" + browser.Type + "¿" + browser.Browser + "¿"
                + browser.Version + "¿" + browser.MajorVersion + "¿"
                + browser.MinorVersion + "¿" + browser.Platform + "¿"
                + HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
        }

        if (browserDetails.Length > 200)
            browserDetails = browserDetails.Substring(0, 200);

        return browserDetails;
    }
    #endregion

    #region validate OpenSession result
    protected void ValidatePage(string result)
    {
        bool IsProjSelected = false;
        string errlog = string.Empty;
        try
        {
            if (Session["FDR"] == null)
            {
                FDR fObj = new FDR(proj);
                fObj.schemaNameKey = Session["dbuser"].ToString();
                Session["FDR"] = fObj;
            }
        }
        catch (Exception ex)
        {

        }

        if (!string.IsNullOrEmpty(language))
            Session["language"] = language;
        else
            Session["language"] = string.Empty;
        CheckResultFormat(result);

        if (isResultXml)
        {

            ParseLoginResult(result);

            if (manage == true || Session["AxRole"].ToString() == "default_")
            {
                traceStatus = "T";
                Session["traceStatus"] = traceStatus;
                util.sysErrorlog = true;
                logobj.errorlog = "true";
                errlog = "true";
            }
            else
            {
                Session["AxTrace"] = "false";
            }
        }
    }

    /// <summary>
    /// Function to parse the login result xml and set the access properties and global variables
    /// </summary>
    /// <param name="result"></param>
    private void ParseLoginResult(string result)
    {
        StringBuilder globalVarToShow = new StringBuilder();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(result);

        XmlNode resultNode = xmlDoc.SelectSingleNode("/result");
        if (resultNode.Attributes["theme"] != null)
            ApplyTheme(resultNode.Attributes["theme"].Value);
        if (resultNode.Attributes["nickname"] != null)
            Session["AxNickName"] = resultNode.Attributes["nickname"].Value;

        if (resultNode.Attributes["userlandingpage"] != null)
            Session["userLandingPage"] = resultNode.Attributes["userlandingpage"].Value;
        if (resultNode.Attributes["roleslandingpage"] != null)
            Session["rolesLandingPage"] = resultNode.Attributes["roleslandingpage"].Value;

        //Below code checks if there is a global variable for 'nickname', if defined then the display name will be nickname
        if (Session["AxNickName"] == null || Session["AxNickName"].ToString() == string.Empty)
            Session["AxNickName"] = Session["user"];

        if (Session["AxEmailId"] == null || Session["AxEmailId"].ToString() == string.Empty)
            Session["AxEmailId"] = resultNode.Attributes["email_id"].Value;

        foreach (XmlNode childNode in resultNode.ChildNodes)
        {
            if (childNode.Name == "globalvars")
            {
                //storing config app global fields in axGlobalVars
                string axGridAttachPath = string.Empty, axAttachmentpath = string.Empty;
                if (Session["AxGridAttachPath"] != null && Session["AxGridAttachPath"].ToString() != "")
                {
                    if (childNode.SelectSingleNode("AXPIMAGEPATH") != null)
                    {
                        childNode.SelectSingleNode("AXPIMAGEPATH").InnerText = Session["AxGridAttachPath"].ToString();
                        axGridAttachPath = "";
                    }
                    else
                    {
                        axGridAttachPath = Session["AxGridAttachPath"].ToString();
                        axGridAttachPath = "<AXPIMAGEPATH>" + axGridAttachPath + "</AXPIMAGEPATH>";
                    }
                }
                if (Session["AxAttachFilePath"] != null && Session["AxAttachFilePath"].ToString() != "")
                {
                    if (childNode.SelectSingleNode("AXPATTACHMENTPATH") != null)
                    {
                        childNode.SelectSingleNode("AXPATTACHMENTPATH").InnerText = Session["AxAttachFilePath"].ToString();
                        axAttachmentpath = "";
                    }
                    else
                    {
                        axAttachmentpath = Session["AxAttachFilePath"].ToString();
                        axAttachmentpath = "<AXPATTACHMENTPATH>" + axAttachmentpath + "</AXPATTACHMENTPATH>";
                    }
                }
                try
                {
                    if (Session["AxTimezoneVariation"] != null && Session["AxTimezoneVariation"].ToString() == "false")
                    {
                        if (childNode.SelectSingleNode("timediff") != null)
                        {
                            childNode.SelectSingleNode("timediff").InnerText = "0";
                        }
                    }
                }
                catch (Exception ex) { }
                bool axp_timezone = false;
                string glbVarsFromIni = util.GetGblVarsFromIni();
                Session["axGlobalVars"] = "<globalvars>" + childNode.InnerXml + axGridAttachPath + axAttachmentpath + glbVarsFromIni + "</globalvars>";
                foreach (XmlNode xmlNode in childNode.ChildNodes)
                {
                    if (xmlNode.Name.ToLower() == "axp_timezone" && xmlNode.InnerText != "")
                    {
                        axp_timezone = true;
                        if (childNode.SelectSingleNode("timediff") != null)
                        {
                            childNode.SelectSingleNode("timediff").InnerText = "";
                        }
                    }
                    if (xmlNode.Name.ToLower() == "axp_lockonread" && (xmlNode.InnerText.ToLower() == "t" || xmlNode.InnerText.ToLower() == "true"))
                    {
                        Session["axp_isLockOnRead"] = "true";
                    }
                    globalVariables.Add(xmlNode.Name + "=" + xmlNode.InnerText);
                    if (xmlNode.Name.ToLower() == "axglo_hide")
                        Session["AxGloHideShow"] = xmlNode.InnerText;
                    if (xmlNode.Name.ToLower() == "axglo_recordid")
                        Session["AxGloRecId"] = xmlNode.InnerText;
                    if (xmlNode.Name.ToLower() == "axp_displaytext")
                    {
                        string sbDspText = string.Empty;
                        if (Session["appname"] != null)
                        {
                            sbDspText = "<span class='globalProjname'>" + Session["appname"].ToString() + "</span> ";
                        }
                        //sbDspText += FormatGlobalVariables(xmlNode.InnerText);
                        if (sbDspText != "")
                            globalVarToShow.Append(sbDspText.ToString());
                    }
                    if (xmlNode.Name.ToLower() == "rolename")
                        Session["AxRole"] = xmlNode.InnerText;
                    if (xmlNode.Name.ToLower() == "responsibilies")
                        Session["AxResponsibilities"] = xmlNode.InnerText;

                    if (xmlNode.Name.ToLower() == "axpimagepath")
                        Session["AxpImagePathGbl"] = xmlNode.InnerText;
                    if (xmlNode.Name.ToLower() == "axpimageserver")
                        Session["AxpImageServerGbl"] = xmlNode.InnerText;
                    if (xmlNode.Name.ToLower() == "axpattachmentpath")
                        Session["AxpAttachmentPathGbl"] = xmlNode.InnerText;
                }
                if (axp_timezone)
                {
                    Session["axGlobalVars"] = "<globalvars>" + childNode.InnerXml + axGridAttachPath + axAttachmentpath + glbVarsFromIni + "</globalvars>";
                }
            }
            else if (childNode.Name == "uservars")
            {
                Session["axUserVars"] = "<uservars>" + childNode.InnerXml + "</uservars>";
                foreach (XmlNode xmlNode in childNode.ChildNodes)
                    userVariables.Add(xmlNode.Name + "=" + xmlNode.InnerText);
            }
            else if (childNode.Name == "access")
            {
                SetUserAccess(childNode);
            }
            // for getting application title from Webservice Result.
            else if (childNode.Name == "name")
            {
                Session["projTitle"] = childNode.InnerText;
                if (Session["AxAppTitle"] != null && Session["AxAppTitle"].ToString() == "") Session["AxAppTitle"] = childNode.InnerText;
            }
            else if (childNode.Name == "pwdconf")
            {
                foreach (XmlNode xmlNode in childNode.ChildNodes)
                {
                    if (xmlNode.Name == "minpwdchars")
                    {
                        Session["minPwdChars"] = xmlNode.InnerText;
                    }
                    else if (xmlNode.Name == "pwdalphanumeric")
                    {
                        Session["IsPwdAlphaNumeric"] = xmlNode.InnerText;
                    }
                }
            }
            else if (childNode.Name == "carddetails")
            {
                if (childNode.InnerText != string.Empty)
                {
                    Session["cardsDesignVal"] = childNode.InnerText;
                }
            }
        }

        if (globalVariables.Count > 0 || userVariables.Count > 0)
        {
            CreateParamaterArray();
        }
        hdParamsValues.Value = globalVarToShow.ToString();
    }

    private void CheckResultFormat(string result)
    {
        if (result.TrimStart().StartsWith("<") && result.TrimEnd().EndsWith(">"))
            isResultXml = true;
        else
            isResultXml = false;
    }

    /// <summary>
    /// Function to apply the theme from the login result for the logged in user, Default theme will be blue.
    /// </summary>
    /// <param name="theme"></param>
    private void ApplyTheme(string theme)
    {
        if (theme != string.Empty)
        {
            Session["themeColor"] = theme;
            regthmdata = "<script language=\"javascript\" type=\"text/javascript\" >";
            regthmdata += "var thmProj = '" + proj + "';var thmUser = '" + user + "';var thmRole = '" + AxRole + "';var thmSid = '" + sid + "';var thmSelected = '" + theme + "';var g_lang='" + Session["language"].ToString() + "';";
            regthmdata += "</script>";

        }
        else
        {
            Session["themeColor"] = "Gray";
            regthmdata = "<script language=\"javascript\" type=\"text/javascript\" >";
            regthmdata += "var thmProj = '" + proj + "';var thmUser = '" + user + "';var thmRole = '" + AxRole + "';var thmSid = '" + sid + "';var thmSelected = '" + "Blue" + "';var g_lang='" + Session["language"].ToString() + "';";
            regthmdata += "</script>";
        }
    }

    /// <summary>
    /// The user access properties will be set for managing the application.
    /// </summary>
    /// <param name="childNode"></param>
    private void SetUserAccess(XmlNode childNode)
    {
        Session["Build"] = "F";
        Session["forcePwdChange"] = "0";
        Session["userAccessEnabled"] = false;
        Session["workflowEnabled"] = false;
        Session["ImportAccess"] = false;
        Session["ExportAccess"] = false;
        Session["pwdExpiresIn"] = "0";
        Session["isUniqueHybrid"] = "F";

        foreach (XmlNode xmlNode in childNode.ChildNodes)
        {
            if (xmlNode.Name == "build" && xmlNode.InnerText == "T")
            {
                Session["Build"] = xmlNode.InnerText;
                manage = true;
            }
            else if (xmlNode.Name == "forcepwdchange")
            {
                Session["forcePwdChange"] = xmlNode.InnerText;
            }
            else if (xmlNode.Name == "users" && xmlNode.InnerText == "T")
            {
                Session["userAccessEnabled"] = true;
            }
            else if (xmlNode.Name == "workflow" && xmlNode.InnerText == "T")
            {
                Session["workflowEnabled"] = true;
            }
            else if (xmlNode.Name == "import" && xmlNode.InnerText == "T")
            {
                Session["ImportAccess"] = true;
            }
            else if (xmlNode.Name == "export" && xmlNode.InnerText == "T")
            {
                Session["ExportAccess"] = true;
            }
            else if (xmlNode.Name == "sessionexists")
            {
                // not in use - To display a message to the user that the previous session is expired.
            }
            else if (xmlNode.Name == "pwdexpiresin")
            {
                Session["pwdExpiresIn"] = xmlNode.InnerText;
            }
            else if (xmlNode.Name == "isuniquehybrid" && xmlNode.InnerText == "T")
            {
                Session["isUniqueHybrid"] = xmlNode.InnerText;
            }
        }
        if ((Session["ImportAccess"] != null && bool.Parse(Session["ImportAccess"].ToString())) || (Session["ExportAccess"] != null && bool.Parse(Session["ExportAccess"].ToString())))
        {
            Session["HistoryAccess"] = true;
        }
    }

    /// <summary>
    /// Function to create the Parameter array scripts for javascript and stores in the Redis or session variable Session["globalvarstring"]
    /// </summary>
    private void CreateParamaterArray()
    {
        StringBuilder paramlist = new StringBuilder();
        string value = string.Empty;
        int paramCnt = 0;

        //for (int i = 0; i < globalVariables.Count; i++)
        //{
        int i = 0;
        foreach (string item in globalVariables)
        {
            string[] strVar = item.ToString().Split('=');
            value = strVar[1];
            if (strVar[0].ToString() == "project" && value.Contains(','))
                Session[strVar[0].ToString()] = value.Split(',')[1];
            else
                Session[strVar[0].ToString()] = value;
            value = util.CheckSpecialChars(value);
            if (!Constants.globalVarPathVaribles.Contains(strVar[0].ToLower()))
            {
                paramlist.Append("Parameters[" + i + "] = " + "\"" + strVar[0] + "~" + value + "\";  ");
                i++;
            }
        }
        for (int j = 0; j < userVariables.Count; j++)
        {
            string[] strVar = userVariables[j].ToString().Split('=');
            value = strVar[1];
            Session[strVar[0].ToString()] = value;
            value = util.CheckSpecialChars(value);
            paramlist.Append("Parameters[" + j + "] = " + "\"" + strVar[0] + "~" + value + "\";  ");

        }

        string dtCulture = Request.UserLanguages[0];
        //If useCulture flag is false , we are not checking the browser culture.
        //If it's true ,we are setting the culture based on the client browser language.
        bool useCulture = false;
        if (Session["AxUSCulture"] != null)
            useCulture = Convert.ToBoolean(Session["AxUSCulture"].ToString());
        if (!useCulture)
            dtCulture = "en-gb";
        else
        {
            if (dtCulture.ToLower() == "en" || dtCulture.ToLower() == "en-us")
                dtCulture = "en-us";
        }
        paramCnt = i + userVariables.Count;
        paramlist.Append("Parameters[" + paramCnt + "] = " + "\"username~" + user + "\";  ");
        paramlist.Append("Parameters[" + (paramCnt + 1) + "] = " + "\"Culture~" + dtCulture + "\";  ");

        Session["ClientLocale"] = dtCulture;
        glCulture = dtCulture;
        if (paramlist.ToString() != string.Empty)
            util.SetGlobalVarString(paramlist.ToString());
    }

    #endregion

    public void LoadAppConfiguration(string signinProj)
    {
        string strProj = string.Empty;

        if (HttpContext.Current.Session["Project"] != null)
            strProj = HttpContext.Current.Session["Project"].ToString();
        else
        {
            if (ConfigurationManager.AppSettings["proj"] != null && ConfigurationManager.AppSettings["proj"].ToString() != "")
                strProj = ConfigurationManager.AppSettings["proj"].ToString();
            else
                strProj = signinProj;
            Session["project"] = strProj;
        }
        if (strProj != string.Empty)
        {
            if (strProj == null) strProj = "";
            string configStr = util.GetConfigAppJSON(strProj);
            util.UpdateConfiginSession(configStr);      // Updating the session with configuration variables
        }
        Session["AxEnableOldTheme"] = "false";
        Session["AxLogging"] = "false";
        if (ConfigurationManager.AppSettings["isCloudApp"] != null)
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString());
        if (HttpContext.Current.Application["ScriptsPath"] != null)
            scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        if (ConfigurationManager.AppSettings["RestDllPath"] != null)
            RestDllPath = ConfigurationManager.AppSettings["RestDllPath"].ToString();

        if (Session["project"] != null && Session["project"].ToString() != "")
            util.GetAxARMConnection(Session["project"].ToString());

        if (Session["ARM_Scripts_URL"] != null)
        {
            ArmRestDllPath = Session["ARM_Scripts_URL"].ToString();
        }

        if (Session["nsessionid"] != null)
        {
            hdnpsid = Session["nsessionid"].ToString();
        }

        if (Session["AxDisableSplit"] != null)
        {
            disableSplit = Session["AxDisableSplit"].ToString();
        }

        if (Session["hybridGUID"] != null)
            hybridGUID = Session["hybridGUID"].ToString();
        if (Session["hybridDeviceId"] != null)
            hybridDeviceId = Session["hybridDeviceId"].ToString();

        try
        {
            alertsTimeout = HttpContext.Current.Session["AxAlertTimeout"] != null ? (Convert.ToInt32(HttpContext.Current.Session["AxAlertTimeout"])) : 3000;
        }
        catch (Exception ex)
        {
            alertsTimeout = 3000;
        }
        try
        {
            var session = HttpContext.Current.Session;
            if (session["AxErrorMsg"] != null)
            {
                errorEnable = Convert.ToBoolean(session["AxErrorMsg"]);
                errorTimeout = session["AxErrorMsgTimeout"] != null ? (Convert.ToInt32(session["AxErrorMsgTimeout"]) * 1000) : 0;
            }
            else
            {
                errorTimeout = 0;
                errorEnable = false;
            }
        }
        catch (Exception ex)
        {
            errorTimeout = 0;
            errorEnable = false;
        }
    }

    private void GetConfigs()
    {
        string mainReload = util.GetAdvConfigs("Main Page Reload");
        if (mainReload != "")
        {
            axMainPageReload = mainReload;
            Session["AxReloadCloud"] = mainReload;
        }

        string DateFormat = util.GetAdvConfigs("Date Format");
        if (DateFormat != "")
        {
            DateFormat = DateFormat.ToLower();
            Session["ClientLocale"] = DateFormat;
            glCulture = DateFormat;
        }

        googleMapsApiKey = util.GetAdvConfigs("Google Maps Api Key");

        string ShowImaWidAction = util.GetAdvConfigs("Show Image Widget Action Button");
        if (ShowImaWidAction != string.Empty)
            ShowImaWidActionBtn = ShowImaWidAction;

        string navigationbutton = util.GetAdvConfigs("Show Navigation Button");
        if (navigationbutton != string.Empty)
            navigationshow = navigationbutton;

        axListViewAsDefault = util.GetAdvConfigs("Listview as Default", "tstruct");
        listviewAsDefault.Value = axListViewAsDefault;

        listviewLoadFromSearch.Value = util.GetAdvConfigs("Listview as default from search", "tstruct");

        axOldModelFlag = util.GetAdvConfigs("AxOldDataList");
        if (axOldModelFlag != string.Empty)
            Session["IsAxOldDataList"] = axOldModelFlag;
        else if (axOldModelFlag == string.Empty)
            axOldModelFlag = "false";

        notifyTimeout = util.GetAdvConfigs("notification time interval");

        string compressedModeString = util.GetAdvConfigs("ApplicationCompressedMode");
        if (compressedModeString == "")
            compressedMode = true;
        else
            compressedMode = compressedModeString == "true";
    }

    private void DoUtlEncode()
    {
        try
        {
            JObject objConfig = null;
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(Session["axGlobalVars"].ToString());
            string axGlobalVars = JsonConvert.SerializeXmlNode(xmlDoc);
            objConfig = JObject.Parse(axGlobalVars);
            axGlobalVars = objConfig["globalvars"].ToString().Replace("\r", "").Replace("\n", "");
            axUtlGlobalVars = util.UtlEncode(axGlobalVars);

            xmlDoc.LoadXml(Session["axUserVars"].ToString());
            string axUserVars = JsonConvert.SerializeXmlNode(xmlDoc);
            objConfig = JObject.Parse(axUserVars);
            axUserVars = objConfig["uservars"].ToString().Replace("\r", "").Replace("\n", "");
            axUtlUserVars = util.UtlEncode(axUserVars);

            xmlDoc.LoadXml(Session["axApps"].ToString());
            string axApps = JsonConvert.SerializeXmlNode(xmlDoc);
            axUtlApps = util.UtlEncode(axApps);
        }
        catch (Exception Ex)
        {
        }
    }

    public static string MD5Hash(string text)
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

    private void SetLandingPage(string pName, string pParams)
    {
        if (pName != null)
        {
            string pType = pName.Substring(0, 1);
            pName = pName.Substring(1);
            if (pType.ToLower() == "t")
            {
                if (CheckListViewAsDefault(pName) == "true")
                {
                    if (axOldModelFlag == "true")
                    {
                        Navigationpage = "../aspx/iview.aspx?ivname=" + pName + "&tstcaption=" + pName;
                    }
                    else
                    {
                        Navigationpage = "../aspx/entity.aspx?tstid=" + pName;
                    }
                }
                else
                {
                    Navigationpage = "../aspx/tstruct.aspx?transid=" + pName + pParams + "&openerIV=" + pName + "&isIV=false";
                }
            }
            else if (pType.ToLower() == "l")
            {
                if (CheckListViewAsDefault(pName) == "true")
                {

                    if (axOldModelFlag == "true")
                    {
                        Navigationpage = "../aspx/iview.aspx?ivname=" + pName + "&tstcaption=" + pName;
                    }
                    else
                    {
                        Navigationpage = "../aspx/entity.aspx?tstid=" + pName;
                    }
                }
                else
                {
                    Navigationpage = "../aspx/tstruct.aspx?transid=" + pName + pParams + "&openerIV=" + pName + "&isIV=false";
                }
            }
            else if (pType.ToLower() == "i")
            {
                if (string.IsNullOrEmpty(pParams))
                    Navigationpage = "../aspx/iview.aspx?ivname=" + pName;
                else
                    Navigationpage = "../aspx/ivtoivload.aspx?ivname=" + pName + pParams;
            }
            else if (pType.ToLower() == "p")
            {
                Navigationpage = "../aspx/page.aspx?pageid" + pName;
            }
            else if (pType.ToLower() == "h")
            {
                if (pName.ToLower().StartsWith("p") && Regex.IsMatch(pName.ToLower().Substring(1), @"^\d+$"))
                {
                    pName = pName.Substring(1);
                    Navigationpage = "../aspx/htmlPages.aspx?load=" + pName + pParams;
                }
                else
                {
                    if (Regex.IsMatch(pName.ToLower().Substring(1), @"^\d+$"))
                        Navigationpage = "../aspx/htmlPages.aspx?load=" + pName + pParams;
                    else
                        Navigationpage = "../aspx/htmlPages.aspx?loadcaption=" + pName + pParams;
                }
            }
            else if (pType.ToLower() == "d" && pName.ToLower() == "dashboard")
            {
                Navigationpage = "../aspx/processflow.aspx?dashboard=t";
            }
            else if (pType.ToLower() == "d" && pName.ToLower() == "calendar")
            {
                Navigationpage = "../aspx/processflow.aspx?calendar=t";
            }
            else if (pType.ToLower() == "d" && pName.ToLower() == "analytics")
            {
                Navigationpage = "../aspx/Analytics.aspx";
            }
        }
        hdHomeUrl.Value = Navigationpage;
    }

    private void GetSessionVars()
    {
        if (Session["project"] != null)
            proj = Session["project"].ToString();
        if (Session["username"] != null)
        {
            user = Session["user"].ToString();
            userName = user;
        }
        if (Session["language"] != null)
            language = Session["language"].ToString();
        string dirLang = util.SetCulture(language.ToUpper());
        if (!string.IsNullOrEmpty(dirLang))
        {
            direction = dirLang.Split('-')[0];
            langType = dirLang.Split('-')[1];
            Application["LangSess"] = language;
        }
        FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/Js/lang/content-" + langType + ".js"));
        if (!(filcustom.Exists))
        {
            langType = "en";
            direction = "ltr";
        }
    }

    private void InitSessExpiryAlert()
    {
        //Session Alert start
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
            SessionStateSection section = (SessionStateSection)config.GetSection("system.web/sessionState");
            int timeout = (int)section.Timeout.TotalMinutes * 1000 * 60;
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);
        }
        //Session Alert end
    }

    private string CheckListViewAsDefault(string structName)
    {
        try
        {
            var jsonArray = JArray.Parse(axListViewAsDefault);
            var match = jsonArray.FirstOrDefault(x =>
            {
                var stypeProp = x.Children<JProperty>()
                                 .FirstOrDefault(p => p.Name.Equals("stype", StringComparison.OrdinalIgnoreCase));
                var structProp = x.Children<JProperty>()
                                  .FirstOrDefault(p => p.Name.Equals("structname", StringComparison.OrdinalIgnoreCase));
                var stypeVal = stypeProp != null && stypeProp.Value != null ? stypeProp.Value.ToString() : null;
                var structVal = structProp != null && structProp.Value != null ? structProp.Value.ToString() : null;
                return string.Equals(stypeVal, "Tstruct", StringComparison.OrdinalIgnoreCase)
                    && string.Equals(structVal, structName, StringComparison.OrdinalIgnoreCase);
            });

            if (match != null)
            {
                var prop = match.Children<JProperty>()
                .FirstOrDefault(p => p.Name.Equals("propsval", StringComparison.OrdinalIgnoreCase));
                return (prop != null && prop.Value != null) ? prop.Value.ToString() : null;
            }
            var allForms = jsonArray.FirstOrDefault(x =>
            {
                var stypeProp = x.Children<JProperty>()
                                 .FirstOrDefault(p => p.Name.Equals("stype", StringComparison.OrdinalIgnoreCase));
                var structProp = x.Children<JProperty>()
                                  .FirstOrDefault(p => p.Name.Equals("structname", StringComparison.OrdinalIgnoreCase));
                var stypeVal = (stypeProp != null && stypeProp.Value != null) ? stypeProp.Value.ToString() : null;
                var structVal = (structProp != null && structProp.Value != null) ? structProp.Value.ToString() : null;
                return string.Equals(stypeVal, "Tstruct", StringComparison.OrdinalIgnoreCase)
                    && string.Equals(structVal, "ALL Forms", StringComparison.OrdinalIgnoreCase);
            });

            if (allForms != null)
            {
                var prop = allForms.Children<JProperty>()
                .FirstOrDefault(p => p.Name.Equals("propsval", StringComparison.OrdinalIgnoreCase));
                return (prop != null && prop.Value != null) ? prop.Value.ToString() : null;
            }
        }
        catch (Exception ex)
        {
        }
        return "true";
    }

    #region WebMethod

    [WebMethod]
    public static void delNotificiationKeyfromRedis(string key)
    {
        string schemaName = string.Empty;
        string userName = string.Empty;
        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();
        if (HttpContext.Current.Session["username"] != null)
            userName = HttpContext.Current.Session["username"].ToString();
        string NotifyKey = schemaName + "-" + userName + Constants.NOTIFICATION_PHASE + key;
        FDW fdwObj = new FDW();
        fdwObj.DeleteAllKeys(NotifyKey, schemaName);

    }
    [WebMethod]
    public static string CheckNotificiationStringinRedis(string key)
    {
        JArray finalJArray = new JArray();
        try
        {
            FDW fdwObj = new FDW();
            string showMessage = "";
            string keynotify = "";
            StringBuilder showMessagelist = new StringBuilder();
            //FDR fdrObj = new FDR();
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            string schemaName = string.Empty;
            string userName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();

            string NotifyKey = schemaName + "-" + userName + Constants.NOTIFICATION_PHASE;
            ArrayList NotifyKeys = fdrObj.GetAllKeys(NotifyKey);
            if (NotifyKeys.Count > 0)
            {
                foreach (var item in NotifyKeys)
                {


                    string time = item.ToString().Split('-').Last();
                    int schemakeylength = schemaName.Length;

                    keynotify = item.ToString().Substring(schemakeylength + 1, item.ToString().Length - (schemakeylength + 1));

                    showMessage = fdrObj.ReadStringKey(keynotify.ToString());// +"-" + time + "/";


                    if (showMessage != string.Empty)
                    {
                        JObject resJObj = new JObject();
                        resJObj["data"] = showMessage;
                        resJObj["keyId"] = time;
                        finalJArray.Add(resJObj);
                    }

                }
            }

        }
        catch (Exception ex) { }
        return finalJArray.ToString();
    }

    [WebMethod]
    public static void ExecutionTraceInterval(string ExecutionLongText, bool isSingout)
    {
        try
        {
            FDW fdwObj = new FDW();
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            string sessId = HttpContext.Current.Session.SessionID;
            string ExecTraceKey = "Executiontrace-" + userName + "-" + sessId;
            string ExecTraceMsg = fdrObj.ReadKey(ExecTraceKey);
            if (ExecTraceMsg != string.Empty)
            {
                ExecTraceMsg += ExecutionLongText;
                fdwObj.WriteKey(ExecTraceKey, ExecTraceMsg);
            }
            else
                fdwObj.WriteKey(ExecTraceKey, ExecutionLongText);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("ExecutionTraceInterval--" + ex.Message, HttpContext.Current.Session.SessionID, "ExecutionTraceInterval", "new", "true");
        }

        if (isSingout)
        {
            try
            {
                ASB.WebService asbWebService = new ASB.WebService();
                asbWebService.SignOut();
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("ExecutionTraceInterval Signout--" + ex.Message, HttpContext.Current.Session.SessionID, "ExecutionTraceInterval-singout", "new", "true");
            }
        }
    }

    #endregion

    private void SignOut()
    {
        try
        {
            string sXml = string.Empty;
            string user = string.Empty;
            string sid = string.Empty;
            string proj = string.Empty;
            string Svrlic_redis = string.Empty;

            user = Session["user"].ToString();
            sid = Session["nsessionid"].ToString();

            if (sid != null)
            {
                if (Session["Svrlic_redis"] != null)
                    Svrlic_redis = Session["Svrlic_redis"].ToString();
                else
                    Svrlic_redis = string.Empty;

                proj = Session["project"].ToString();
                //sXml = sXml + "<root axpapp=" + "\"" + proj + "\"" + " " + Svrlic_redis + " sessionid=" + "\"" + sid + "\"" + " trace=" + "\"" + Session["trace"] + "\"" + " >";
                //if (Session["axApps"] != null)
                //    sXml += Session["axApps"].ToString() + Application["axProps"].ToString();
                //else if (proj != "" && Application[proj + "-axApps"] != null)
                //    sXml += Application[proj + "-axApps"].ToString() + Application["axProps"].ToString();
                //sXml += "</root>";
                util.DeleteKeepAliveWebKey();
                util.SaveExecutionText();
                util.DeleteUnwantedKeys();
                util.TempAttaServerFiles();
                util.ClearUserIviewData();
                util.RemoveLoggedUserDetails(proj, sid, user);
                //ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                //string res = objWebServiceExt.CallLogoutWS("sess", sXml);

                string errorLog = logobj.CreateLog("Calling AxMain Signout ws", sid, "Signout", "new");
                string _axApps = string.Empty;
                string _axProps = string.Empty;
                if (Session["axApps"] != null)
                {
                    _axApps = Session["axApps"].ToString();
                    _axProps = Application["axProps"].ToString();
                }
                else if (proj != "" && Application[proj + "-axApps"] != null)
                {
                    _axApps = Application[proj + "-axApps"].ToString();
                    _axProps = Application["axProps"].ToString();
                }
                if (errorLog != "")
                    errorLog = errorLog.Replace("\\", "\\\\");
                _axApps = _axApps.Replace(@"\", "\\\\");
                string _UserName = string.Empty;
                if (Session["username"] != null)
                    _UserName = Session["username"].ToString();
                string signOutJson = "{\"logout\":{\"axpapp\":\"" + proj + "\",\"username\":\"" + _UserName + "\",\"sessionid\":\"" + sid + "\"," + Svrlic_redis + " \"trace\":\"" + errorLog + "\"},\"axapps\":\"" + _axApps + "\",\"axprops\":\"" + _axProps + "\"}";
                string res = ALCClient.CallALCClientLogout(_UserName, signOutJson);
                string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                scriptsPath = scriptsPath + "Axpert\\" + sid;

                if (Directory.Exists(scriptsPath) && sid != null)
                {
                    try
                    {
                        Directory.Delete(scriptsPath, true);
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }
            }
        }
        catch (Exception ex)
        {
            util.KillSession();
        }

    }

}
