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
using System.Web.Script.Serialization;

public partial class aspx_Mainnew : System.Web.UI.Page
{

    Util.Util util;
    public string appName;
    public string appTitle;
    public string axpProjectCaption = string.Empty;
    public string themeColor = string.Empty;
    int alertsTimeout = 3000;
    int errorTimeout = 0;
    bool errorEnable = false;
    public string scriptsPath = string.Empty;
    public string RestDllPath = string.Empty;
    public string ArmRestDllPath = string.Empty;
    LogFile.Log logobj = new LogFile.Log();
    string signinProj = string.Empty;
    public string thmProj = string.Empty;
    public string thmUser = string.Empty;
    public string thmSid = string.Empty;
    public bool manage = false;
    public bool userAccess = false;
    public bool workflow = false;
    public string signOutPath = string.Empty;
    public string errorlog = "false";
    string strGlobalVar = string.Empty;
    string loginPath = "";
    public string Navigationpage = "";
    public string regthmdata = string.Empty;
    string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    public string user = string.Empty;
    public string language = string.Empty;
    public string appsessionKey = string.Empty;
    public string hdnpsid = string.Empty;
    public string PeriodicRCKey = string.Empty;
    public string direction = "ltr";
    public string langType = "en";
    public string glCulture = string.Empty;
    public string strLang = string.Empty;
    public string MenuHTML = "<ul class=\"mainParent\">";

    protected bool axChangePwd = false;
    protected bool axChangePwdReDir = false;

    string proj = string.Empty;
    public string AxRole = string.Empty;
    string sid = string.Empty;
    string axApps = string.Empty;
    string axProps = string.Empty;
    string axRegId = string.Empty;
    string axSchema = string.Empty;
    public string printInterval = string.Empty;
    public string draftScript = string.Empty;
    public string traceStatus = "F";
    public bool traceValue = false;
    //string enableDraftSave = "false";
    public string[] gloApp_Split = null;
    string globalUsr_vars = null;
    string global_vars = null;
    ArrayList globalVariables = new ArrayList();
    ArrayList userVariables = new ArrayList();
    string forcePwdChange = string.Empty;
    Boolean useCulture = false;
    public bool build = false;
    public bool export = false;
    public bool import = false;
    public bool history = false;
    bool isResultXml = false;
    bool isLockOnRead = false;
    public int pwdExpDays = 0;
    bool isCloud = false;
    public bool isCloudApp = false;
    public bool IsLogoAvail = false;
    public string fromSSO = "false";
    public string copyRightTxt = "";
    public string HelpIview = string.Empty;
    public string IviewNews = string.Empty;
    public string IviewLive = string.Empty;
    public string menuStr = string.Empty;
    public string menuStrLo = string.Empty;
    public bool isLoggedIn = false;
    public string isNewMenu = "true";
    public string menuStyle = "modern";
    public string menuXmlData = string.Empty;
    public string utilNode = string.Empty;
    public string Utl = string.Empty;
    public string redisUtl = string.Empty;
    string loginTrace = "false";
    public bool IsCustomLogoAvail = false;
    public string axUtlGlobalVars = String.Empty;
    public string axUtlUserVars = String.Empty;
    public string axUtlApps = String.Empty;
    public string axMainPageReload = String.Empty;
    public string axMenuReSize = String.Empty;
    public string axProjectTile = String.Empty;
    string notifyTimeout = string.Empty;
    string signalRNotifications = "false";
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    ASB.WebService asbWebService = new ASB.WebService();
    // static RedisServer objRS = RedisServer.Instance;
    public StringBuilder strParamsRF = new StringBuilder();
    public string PageBuilderAccess = string.Empty;
    StringBuilder strMenuHtml1 = new StringBuilder();
    static string paramTransid = string.Empty;
    string disableSplit = "false";
    FDW fdwObj = new FDW();
    string rnd_key = string.Empty;
    string pwd = string.Empty;
    string cacKey = string.Empty;
    public string templateText = string.Empty;
    Dictionary<string, string> UserOptions = new Dictionary<string, string>();
    public string axUserOptions = string.Empty;
    public bool compressedMode = true;
    public bool showMenu = true;
    public string googleMapsApiKey = string.Empty;
    public string privateSSO = string.Empty;
    public string privateSsoToken = string.Empty;
    public string GlobalParameterForm = string.Empty;
    public string lastOpenPage = string.Empty;
    public string hybridGUID = "";
    public string hybridDeviceId = "";
    public string hybridDefaultPage = string.Empty;
    public string timeZone = "";
    public string userDetails = "";
    public string ShowImaWidActionBtn = "true";
    public string ssoredirecturl = string.Empty;
    public string oktaclientKey = string.Empty;
    public string office365clientKey = string.Empty;
    public string homepagews = string.Empty;
    public string navigationshow = "true";
    public string localStorUser = "false";
    ExecTrace ObjExecTr = ExecTrace.Instance;
    public string requestProcess_logtime = string.Empty;
    public string serverprocesstime = string.Empty;
    public string hdnSSTime = string.Empty;

    public string homePageType = string.Empty;
    public bool cardsEnabled = false;
    public string cardsDataVal = (new JArray()).ToString();
    public string cardsDesignVal = (new JArray()).ToString();

    public string userLandingPage = string.Empty;
    public string rolesLandingPage = string.Empty;
    public string autoSessExtend = "false";

    public string armKey = string.Empty;
    public string armUrl = string.Empty;
    public string isPEG = string.Empty;
    public string axGloVarsHtml = string.Empty;
    public string isHomeConfigPage = "false";
    public string newHomeCards = "false";
    public string ExecTraceOn = "false";
    public string axOldModelFlag = "false";
    public string axpertstudioReact = string.Empty;
    public string pwdExpiryAlert = string.Empty;
    JObject AxGeneralconfigs = new JObject();

    LoginHelper loginHelper = new LoginHelper();
    protected override void InitializeCulture()
    {
        if (Request.Form["hdnLanguage"] != null || Session["language"] != null)
        {
            string langProj = Request.Form["hdnLanguage"] == null ? Session["language"].ToString() : Request.Form["hdnLanguage"];
            util = new Util.Util();
            string dirLang = string.Empty;
            dirLang = util.SetCulture(langProj.ToUpper());
            if (!string.IsNullOrEmpty(dirLang))
            {
                direction = dirLang.Split('-')[0];
                langType = dirLang.Split('-')[1];
                Application["LangSess"] = langProj;
            }
            FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/Js/lang/content-" + langType + ".js"));
            if (!(filcustom.Exists))
            {
                langType = "en";
                direction = "ltr";
            }
        }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        util = new Util.Util();
        util.IsValidSession();
        if (!IsPostBack)
        {
            if ((Request.UserAgent.IndexOf("AppleWebKit") > 0) || (Request.UserAgent.IndexOf("Unknown") > 0) || (Request.UserAgent.IndexOf("Chrome") > 0))
            {
                Request.Browser.Adapters.Clear();
            }
            Page.ClientTarget = "uplevel";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Form["hdnSSTime"] != null || (Request.Form["hdnSSTime"] == null && Session["etServerTime"] != null))
        {
            try
            {
                if (Util.Util.CheckCrossScriptingInString(Request.Form["hdnSSTime"]) || (Request.Form["hdnbrowserElapsTime"] != null && Util.Util.CheckCrossScriptingInString(Request.Form["hdnbrowserElapsTime"])))
                {
                    try
                    {
                        Response.Redirect(Constants.LOGINPAGE, true);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Thread.ResetAbort();
                    }
                }
                if (Util.Util.ContainsXSS(Request.Form["hdnSSTime"]) || (Request.Form["hdnbrowserElapsTime"] != null && Util.Util.ContainsXSS(Request.Form["hdnbrowserElapsTime"])))
                {
                    try
                    {
                        Response.Redirect(Constants.LOGINPAGE, true);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Thread.ResetAbort();
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect(Constants.LOGINPAGE, true);
                }
                catch (ThreadAbortException tex)
                {
                    Thread.ResetAbort();
                }
            }
            string browserElapsTime = Request.Form["hdnbrowserElapsTime"] != null ? Request.Form["hdnbrowserElapsTime"] : "0";
            hdnSSTime = Request.Form["hdnSSTime"] != null ? Request.Form["hdnSSTime"] : Session["etServerTime"].ToString();
            requestProcess_logtime += ObjExecTr.WireElapsTime(browserElapsTime, hdnSSTime);
        }

        try
        {
            if (HttpContext.Current.Session["AxInternalRefresh"] == null || (HttpContext.Current.Session["AxInternalRefresh"] != null && HttpContext.Current.Session["AxInternalRefresh"].ToString() != "true"))
                AntiforgeryChecker.Check(this, _antiforgery);
        }
        catch (Exception ex)
        {
            Response.Redirect("~/CusError/AxCustomError.aspx");
        }

        try
        {
            if (Util.Util.CheckCrossScriptingInString(Request.Form["hdnAxGKey"]) || Util.Util.CheckCrossScriptingInString(Request.Form["hdnLanguage"]) || (Request.Form["hdnAxLoggedUser"] != null && Util.Util.CheckCrossScriptingInString(Request.Form["hdnAxLoggedUser"])))
            {
                try
                {
                    Response.Redirect(Constants.LOGINPAGE, true);
                }
                catch (ThreadAbortException ex)
                {
                    Thread.ResetAbort();
                }
            }
            if (Util.Util.ContainsXSS(Request.Form["hdnAxGKey"]) || Util.Util.ContainsXSS(Request.Form["hdnLanguage"]) || (Request.Form["hdnAxLoggedUser"] != null && Util.Util.ContainsXSS(Request.Form["hdnAxLoggedUser"])))
            {
                try
                {
                    Response.Redirect(Constants.LOGINPAGE, true);
                }
                catch (ThreadAbortException ex)
                {
                    Thread.ResetAbort();
                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                Response.Redirect(Constants.LOGINPAGE, true);
            }
            catch (ThreadAbortException tex)
            {
                Thread.ResetAbort();
            }
        }

        try
        {
            if (Request.Form["hdnAxProjs"] != null && !string.IsNullOrEmpty(Request.Form["hdnAxProjs"].ToString()))
            {
                if (Util.Util.CheckCrossScriptingInString(Request.Form["hdnAxProjs"]))
                {
                    try
                    {
                        Response.Redirect(Constants.LOGINPAGE, true);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Thread.ResetAbort();
                    }
                }
                if (Util.Util.ContainsXSS(Request.Form["hdnAxProjs"]))
                {
                    try
                    {
                        Response.Redirect(Constants.LOGINPAGE, true);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Thread.ResetAbort();
                    }
                }
                HttpContext.Current.Session["Project"] = Request.Form["hdnAxProjs"].ToString();
            }

            if (Request.Form["hdnCloudDb"] != null && !string.IsNullOrEmpty(Request.Form["hdnCloudDb"].ToString()))
            {
                if (Util.Util.CheckCrossScriptingInString(Request.Form["hdnCloudDb"]))
                {
                    try
                    {
                        Response.Redirect(Constants.LOGINPAGE, true);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Thread.ResetAbort();
                    }
                }
                if (Util.Util.ContainsXSS(Request.Form["hdnCloudDb"]))
                {
                    try
                    {
                        Response.Redirect(Constants.LOGINPAGE, true);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Thread.ResetAbort();
                    }
                }
                HttpContext.Current.Session["AxCloudDB"] = Request.Form["hdnCloudDb"].ToString();
            }
        }
        catch (Exception ex)
        {
            try
            {
                Response.Redirect(Constants.LOGINPAGE, true);
            }
            catch (ThreadAbortException tex)
            {
                Thread.ResetAbort();
            }
        }

        //To Determine if login has already been done from the new page.
        if (Request.Form["hdnAxGKey"] != null || (Request.Form["hdnAxGKey"] == null && Session["AxInternalRefresh"] != null && Session["AxInternalRefresh"].ToString() == "true"))
        {
            if (Request.Form["hdnAxGKey"] != null)
            {
                string gid = Request.Form["hdnAxGKey"].ToString();
                string helperString = string.Empty;
                helperString = GetLoginObject(gid);
                if (helperString != string.Empty)
                {
                    loginHelper = JsonConvert.DeserializeObject<LoginHelper>(helperString);
                    if (loginHelper.sessions != null && loginHelper.sessions.Count > 0)
                    {
                        foreach (var item in loginHelper.sessions)
                        {
                            string key = item.Key;
                            string value = item.Value;

                            Session[item.Key] = loginHelper.sessions[item.Key];
                        }
                    }

                    Session["project"] = signinProj = loginHelper.proj;


                    //Session["user"] = loginHelper.user;
                    //Session["username"] = loginHelper.user;
                    //user = loginHelper.user;


                    Session["pwd"] = loginHelper.password;
                    Session["nsessionid"] = Session.SessionID;
                    sid = Session.SessionID;
                    Session["language"] = loginHelper.selectedLanguage;
                    Session["axp_language"] = loginHelper.selectedLanguage.ToLower();
                    Session["MobileView"] = loginHelper.isMobile;
                    hybridGUID = loginHelper.hybridGUID;
                    timeZone = loginHelper.timeZone;
                    Session["hybridGUID"] = hybridGUID;
                    hybridDeviceId = loginHelper.hybridDeviceId;
                    Session["hybridDeviceId"] = hybridDeviceId;
                    hybridDefaultPage = loginHelper.hybridDefaultPage;
                    userDetails = loginHelper.userDetails;
                    Session["userDetails"] = userDetails;
                    Session["rnd_key"] = rnd_key = loginHelper.rnd_key == null ? "" : loginHelper.rnd_key;
                    Session["isSSOLogin"] = loginHelper.isSSO;
                    Session["SSOLoginType"] = loginHelper.SSOType;
                    Session["staySignedId"] = loginHelper.staySignedId;
                    Session["Svrlic_redis"] = loginHelper.lic_redis;
                    lastOpenPage = loginHelper.lastOpenPage;
                    Session["loggedBroserId"] = loginHelper.loggedBroserId;
                    Session["dbotpauth"] = loginHelper.otpauthlogin == null ? "" : loginHelper.otpauthlogin;
                }
                else
                {
                    SessExpires();
                    return;
                }
            }
            else
            {
                signinProj = Session["Project"].ToString();
                user = Session["username"].ToString();
                pwd = Session["pwd"].ToString();
                rnd_key = Session["rnd_key"].ToString();
                language = Session["language"].ToString();
                Session["AxInternalRefresh"] = null;
                if (Session["newHomeCards"] != null && Session["newHomeCards"].ToString() == "true")
                    newHomeCards = "true";
            }
        }
        else
        {
            SessExpires();
            return;
        }

        CheckCloudRequest();
        LoadAppConfiguration();

        if (HttpContext.Current.Session["Project"] == null || Convert.ToString(HttpContext.Current.Session["Project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        else
        {
            if (ConfigurationManager.AppSettings["ExecutionTrace"] != null && ConfigurationManager.AppSettings["ExecutionTrace"].ToString() != string.Empty && ConfigurationManager.AppSettings["ExecutionTrace"].ToString() == "true")
                ExecTraceOn = "true";
            else
                ExecTraceOn = "false";

            //if (ConfigurationManager.AppSettings["axpertstudioReact"] != null && ConfigurationManager.AppSettings["axpertstudioReact"].ToString() != string.Empty)
            //    axpertstudioReact = ConfigurationManager.AppSettings["axpertstudioReact"].ToString();
            //else
            //    axpertstudioReact = string.Empty;

            GetSessionVars();
            SetCloudSignoutPath();

            axProps = Application["axProps"].ToString();
            axChangePwd = Convert.ToBoolean(Application["axChangePwd"]);
            axChangePwdReDir = Convert.ToBoolean(Application["axChangePwdReDir"]);

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!IsPostBack || Session["isLanguageRefresh"] != null)
            {
                Session.Remove("isLanguageRefresh");

                if (Request.UserAgent.IndexOf("AppleWebKit") > 0 | Request.UserAgent.IndexOf("Unknown") > 0 | Request.UserAgent.IndexOf("Chrome") > 0)
                {
                    Request.Browser.Adapters.Clear();
                }

                logobj.CreateLog("Loading main.aspx", Session.SessionID.ToString(), "login", "new");
                Session["backForwBtnPressed"] = false;

                if (Session["validated"] == null || Session["validated"].ToString() == "")
                {
                    logobj.CreateLog("Calling ValidatePage", Session.SessionID.ToString(), "login", "");
                    if (!isCloud)
                        ValidatePage();

                    if (Session["project"].ToString() == "bafco")//temporary code added to analyse the issue duplicate user issue in BACFO
                    {
                        string AxLoggedUser = string.Empty;
                        if (Request.Form["hdnAxLoggedUser"] != null)
                            AxLoggedUser = Request.Form["hdnAxLoggedUser"].ToString();
                        SaveLoggedUserInfo(AxLoggedUser, user);
                    }
                }
                else
                {
                    SetAccessFromSession();
                    if (Session["cardsDesignVal"] != null)
                    {
                        cardsDesignVal = Session["cardsDesignVal"].ToString();
                    }
                    else
                    {
                        try
                        {
                            string result = loginHelper.result;
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(result);

                            XmlNode resultNode = xmlDoc.SelectSingleNode("/result/carddetails");

                            cardsDesignVal = resultNode.InnerText;
                            Session["cardsDesignVal"] = cardsDesignVal;
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    try
                    {
                        string result = loginHelper.result;
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(result);

                        XmlNode resultNode = xmlDoc.SelectSingleNode("/result");

                        if (resultNode.Attributes["userlandingpage"] != null)
                            Session["userLandingPage"] = resultNode.Attributes["userlandingpage"].Value;
                        if (resultNode.Attributes["roleslandingpage"] != null)
                            Session["rolesLandingPage"] = resultNode.Attributes["roleslandingpage"].Value;
                    }
                    catch (Exception ex)
                    { }
                }

                util.ClearUserIviewData();

                GetGlobalVariables();
                createTopLinks();
                SetPrintInterval();
            }
            else
            {
                if (Session["project"] != null)
                {
                    traceValue = Convert.ToBoolean(Session["AxTrace"]);
                    traceStatus = (string)Session["traceStatus"];
                    SetGlobalVariables();
                    SetTheme();
                }
            }
            AddCustomLinks();

            if (Session["AxEnableCards"] != null)
            {
                homePageType = Session["AxEnableCards"].ToString() == "true" ? "cards" : "";
            }

            if (homePageType == string.Empty)
            {
                homePageType = "page";
            }

            //if (Session["project"] != null)//Moved this condition after menu service because homeconfig flag will be receiving from Menu.
            //{
            //    SetLandingPage();
            //}

            // Authorize or valdiate hybrid user based on device id.
            try
            {
                if (hybridDeviceId != string.Empty && Convert.ToString(Session["isUniqueHybrid"]) == "T")
                {
                    ValidateHybridUserDeviceId();
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("ValidateHybridUserDeviceId - \n\tCurrent Device ID- " + hybridDeviceId + "\n\tisUniqueHybrid- " + Convert.ToString(Session["isUniqueHybrid"]) + "\n\tError - " + ex.Message, HttpContext.Current.Session.SessionID, "ValidateHybridUserDeviceId", "new", "true");
            }

            GetMenuAndCardsData();
            if (Session["project"] != null)
            {
                SetLandingPage();
            }
            if (ConfigurationManager.AppSettings["ExecutionTrace"] != null && ConfigurationManager.AppSettings["ExecutionTrace"].ToString() != string.Empty && ConfigurationManager.AppSettings["ExecutionTrace"].ToString() == "true")
                DeletePrevExecKeys();
            if (Session["project"] != null)
            {
                SetnotifyTimeout();
                GetConfigs();

                if (Session["projTitle"] != null)
                    appName = Session["projTitle"].ToString();
                if (Session["AxAppTitle"] != null && Session["AxAppTitle"].ToString() != "")
                    appName = Session["AxAppTitle"].ToString();
                if (Session["axp_projectcaption"] != null && Session["axp_projectcaption"].ToString() != "")
                    appName = Session["axp_projectcaption"].ToString();
                appTitle = appName;
                var axApp = UserOptions.Where(x => x.Key == "axAppName").ToList();
                if (axApp.Count > 0)
                    UserOptions["axAppName"] = "\"appName\":\"" + appName + "\"♠\"onclick\":\"LoadIframe('loadhomepage');\"";
                else
                    UserOptions.Add("axAppName", "\"appName\":\"" + appName + "\"♠\"onclick\":\"LoadIframe('loadhomepage');\"");

                string dynamicScript = "isLockOnRead = " + isLockOnRead.ToString().ToLower() + ";";
                if (pwdExpDays > 0)
                {
                    dynamicScript += "$j('#dvMessage').show();";

                    pwdExpiryAlert = "showPwdExpiryAlert('Your password will expire in " + pwdExpDays + " days. Would you like to change it?');";
                }

                //If the global vars tstruct is there and the AxShowHideGloVars is false, then show the tstruct on page load

                string isGloVarTrue = string.Empty;
                if (Session["AxGloHideShow"] != null)
                    isGloVarTrue = Session["AxGloHideShow"].ToString();
                string url = "ParamsTstruct.aspx?transid=" + paramTransid + "";

                DoUtlEncode();

                if (Session["AxDisableSplit"] != null)
                {
                    disableSplit = Session["AxDisableSplit"].ToString();
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "set global var to unlock tstruct records", "<script>" + dynamicScript + "var navigatePage='" + Navigationpage + "';" + "var isCloudApp = '" + isCloudApp.ToString() + "'; " + "var userResp = '" + Session["AxResponsibilities"] + "';" + "var sesslanguage='" + language + "';" + "var mainRestDllPath='" + RestDllPath + "';" + "var mainArmRestDllPath='" + ArmRestDllPath + "';" + "var mainProject='" + proj + "';" + "var mainSessionId='" + sid + "';" + "var mainUserName='" + Session["user"].ToString() + "';" + "var AxHelpIview='" + HelpIview + "';" + " var axUtlGlobalVars='" + axUtlGlobalVars + "'; var axUtlUserVars='" + axUtlUserVars + "'; var axUtlApps='" + axUtlApps + "';var alertsTimeout='" + alertsTimeout.ToString() + "';var errorTimeout='" + errorTimeout.ToString() + "';var errorEnable='" + errorEnable.ToString().ToLower() + "';var notifyTimeout ='" + notifyTimeout + "';var appDiableSplit ='" + disableSplit + "';" + "var armKey ='" + armKey + "';var armUrl = '" + armUrl + "';var isPEG ='" + isPEG + "';var isHomeConfigPage='" + isHomeConfigPage + "';" + "var genConfigurations=" + AxGeneralconfigs.ToString(Newtonsoft.Json.Formatting.None) + ";var signalRNotifications='" + signalRNotifications + "';" + pwdExpiryAlert + " </script>");

                if (!IsPostBack)
                {
                    if (isGloVarTrue != string.Empty && isGloVarTrue != "")
                    {
                        if (isGloVarTrue == "F")
                        {
                            string AxGloRecId = string.Empty;
                            if (Session["AxGloRecId"] != null)
                                AxGloRecId = Session["AxGloRecId"].ToString();
                            if (AxGloRecId != "")
                            {
                                url = "../aspx/ParamsTstruct.aspx?transid=" + paramTransid + "&recordid=" + AxGloRecId + "";
                            }
                            else
                            {
                                url = "../aspx/ParamsTstruct.aspx?transid=" + paramTransid + "";
                            }
                            Navigationpage = url;
                            //hdHomeUrl.Value = Navigationpage;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "GlobalParams", "setTimeout(function() {setTimeout(function() {BootstrapDialogAppParamsShow('" + Navigationpage + "');}, 200);}, 0);", true);
                        }
                    }
                    else
                    {
                        editAxGlo2.Visible = false;
                        // UserOptions.Add("GlobalParams", "\"display\":\"block\"♠\"title\":\"Global Parameters\"");
                    }
                }
            }
            InitSessExpiryAlert();

            util.GetRedisConnDetailsStudio();
            if (Session["axpertstudioReact"] != null && Session["axpertstudioReact"].ToString() != string.Empty)
                axpertstudioReact = HttpContext.Current.Session["axpertstudioReact"].ToString();
        }

        EncryptConnDtls();
        SetPageLoadVars();
        CheckConfigLock();
        if (Session["AxCloudDB"] != null)
        {
            complaintLI.Visible = true;
        }
        else
        {
            complaintLI.Visible = false;
        }
        //HttpContext.Current.Session["AxTrace"] = "true";

        if (cardsDataVal != string.Empty)
        {
            cardsData.Value = util.CheckSpecialChars(cardsDataVal);

            //cards developement code
            //ASB.WebService asbWebService = new ASB.WebService();
            //cardsData.Value = asbWebService.GetCardsData();
        }

        if (cardsDesignVal != string.Empty && Session["cardsDesignVal"] == null)
        {
            cardsDesign.Value = cardsDesignVal;
        }
        else if (Session["cardsDesignVal"] != null)
        {
            cardsDesign.Value = Session["cardsDesignVal"].ToString();
        }

        if (hybridDeviceId != "")
            SaveMobileHybridDetails();
        SsoClientId();
        GetUserOptions();

        try
        {
            if (HttpContext.Current.Session["AdvConfigsGeneral"] != null)
            {
                Session.Remove("AdvConfigsGeneral");
            }
        }
        catch (Exception ex) { }

        Session["etServerTime"] = hdnSSTime;
        requestProcess_logtime += ObjExecTr.RequestProcessTime("Response");
        serverprocesstime = ObjExecTr.TotalServerElapsTime();

    }

    private void ValidateHybridUserDeviceId()
    {
        string hybridAuthId = string.Empty;
        try
        {
            string dbType = string.Empty;
            if (Session["axdb"] != null && Session["axdb"].ToString() != string.Empty)
            {
                dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
            }

            string getDeviceIdSql = Constants.AXHYBRID_GETUSERDEVICEID;
            if (Session["username"] != null && Session["username"].ToString() != string.Empty)
            {
                getDeviceIdSql = getDeviceIdSql.Replace("$USERNAME$", HttpContext.Current.Session["username"].ToString());
            }
            if (Session["project"] != null && Session["project"].ToString() != string.Empty)
            {
                getDeviceIdSql = getDeviceIdSql.Replace("$PROJECTNAME$", HttpContext.Current.Session["project"].ToString());
            }

            string getDeviceIdSqlResult = objWebServiceExt.ExecuteSQL("", getDeviceIdSql, "JSON");
            JObject parsedResultRows = JObject.Parse(getDeviceIdSqlResult);

            if (parsedResultRows != null && parsedResultRows["result"] != null && parsedResultRows["result"]["row"] != null)
            {
                hybridAuthId = dbType == "oracle" ? parsedResultRows["result"]["row"][0]["DEVICEID"].ToString() : parsedResultRows["result"]["row"][0]["deviceid"].ToString();
                if (hybridAuthId != string.Empty && hybridAuthId != hybridDeviceId)
                {
                    Response.Redirect(HttpContext.Current.Application["SessExpiryPath"] + "?showmsg=This device is not authorized to login.", true);
                }

            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("ValidateHybridUserDeviceId - \n\tCurrent Device ID- " + hybridDeviceId + "\n\tSaved Device ID- " + hybridAuthId + "\n\tError - " + ex.Message, HttpContext.Current.Session.SessionID, "ValidateHybridUserDeviceId", "new", "true");
        }
    }

    protected string GetLoginObject(string gid)
    {
        string loginObj = string.Empty;
        try
        {
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
            {
                string thisPorj = gid.Split('-')[0];
                fObj = new FDR(thisPorj);
            }
            loginObj = fObj.ReadKeyNoSchema(gid);
            if (loginObj != string.Empty)
            {
                FDW fdwObj = new FDW();
                ArrayList arList = new ArrayList();
                arList.Add(gid);
                fdwObj.DeleteKeys(arList);
            }
        }
        catch (Exception ex) { }
        if (loginObj == string.Empty && HttpContext.Current.Cache[gid] != null)
        {
            loginObj = HttpContext.Current.Cache.Get(gid).ToString();
            HttpContext.Current.Cache.Remove(gid);
        }
        return loginObj;
    }

    protected void SaveLoggedUserInfo(string signinDetail, string mainUser)
    {
        try
        {
            string[] signinDetails = signinDetail.Split('~');
            string axsuname = signinDetails[0];
            string axosid = signinDetails[1];
            string axsid = signinDetails[2];
            string axguid = signinDetails[3];
            string sqlInsertLog = "insert into ax_loggedInUsers(susername,oldsessionid,newsessionid,guid,musername,loggeddate) values('" + axsuname + "','" + axosid + "','" + axsid + "','" + axguid + "','" + mainUser + "',CURRENT_TIMESTAMP)";
            DBContext obj = new DBContext();
            obj.ExecuteSqlQueryInline(sqlInsertLog);
        }
        catch (Exception ex) { }
    }

    private void SsoClientId()
    {
        try
        {
            string _strProj = Session["Project"].ToString();
            FDR fdrObj = new FDR();
            string SSOJsoncontent = fdrObj.StringFromRedis(Constants.AXSSO_CONN_KEY, _strProj);
            if (SSOJsoncontent != string.Empty)
            {
                JObject config = JObject.Parse(SSOJsoncontent);
                foreach (var section in config)
                {
                    if (section.Key == "office365")
                    {
                        JObject sectionObject = section.Value as JObject;
                        if (sectionObject != null)
                        {
                            foreach (JProperty property in sectionObject.Properties())
                            {
                                switch (property.Name)
                                {
                                    case "of365clientkey":
                                        office365clientKey = property.Value.ToString();
                                        break;
                                    case "of365redirecturl":
                                        if (property.Value.ToString() != "")
                                            ssoredirecturl = property.Value.ToString();
                                        break;
                                }
                            }
                        }
                    }
                    else if (section.Key == "okta")
                    {
                        JObject sectionObject = section.Value as JObject;
                        if (sectionObject != null)
                        {
                            foreach (JProperty property in sectionObject.Properties())
                            {
                                switch (property.Name)
                                {
                                    case "oktaclientkey":
                                        oktaclientKey = property.Value.ToString();
                                        break;
                                    case "oktaredirecturl":
                                        if (property.Value.ToString() != "")
                                            ssoredirecturl = property.Value.ToString();
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void GetUserOptions()
    {
        string KeyData = Constants.REDISAXUSEROPTIONS;
        string schemaName = string.Empty;
        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();
        try
        {
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
                fObj = new FDR();
            axUserOptions = fObj.StringFromRedis(util.GetRedisServerkey(KeyData, "axUserOptions", Session["username"].ToString()), schemaName);
            // btnSetParams.Click()
        }
        catch (Exception) { }
        if (axUserOptions == string.Empty && (Session["axUserOptions"] != null && Session["axUserOptions"].ToString() != "cached"))
            axUserOptions = Session["axUserOptions"].ToString();

        if (Session["axUserOptions"] == null || axUserOptions == string.Empty)// (Session["axUserOptions"] != null && Session["axUserOptions"].ToString() != "cached"))
        {
            if (GlobalParameterForm == "hide")
            {
                var AxGlo = UserOptions.Where(x => x.Key == "GlobalParams").ToList();
                if (AxGlo.Count > 0)
                    UserOptions["GlobalParams"] = "\"display\":\"none\"♠\"title\":\"Global Parameters\"";
                else
                    UserOptions.Add("GlobalParams", "\"display\":\"none\"♠\"title\":\"Global Parameters\"");
            }

            string strLang = string.Empty;
            if (Session["AxLanguages"] != null)
            {
                strLang = Session["AxLanguages"].ToString();
            }

            UserOptions.Add("About", "\"onclick\":\"showAbout()\"♠\"title\":\"About\"");
            UserOptions.Add("Language", "\"onclick\":\"$.axpertUI.rightSideBar.switchLanguage(this);\"♠\"title\":\"Language\"♠\"options\":\"" + strLang + "\"");
            UserOptions.Add("Refresh", "\"onclick\":\"refreshMenu()\"♠\"title\":\"Refresh Menu\"");
            UserOptions.Add("RefreshParams", "\"onclick\":\"refreshVarParams()\"♠\"title\":\"Refresh App Vars/Params\"");
            if (Convert.ToString(Session["Build"]) == "F")
                UserOptions.Add("InMemoryDB", "\"display\":\"none\"♠\"onclick\":\"DoUtilitiesEvent('InMemoryDB')\"♠\"title\":\"In-Memory DB\"");
            else
                UserOptions.Add("InMemoryDB", "\"display\":\"block\"♠\"onclick\":\"DoUtilitiesEvent('InMemoryDB')\"♠\"title\":\"In-Memory DB\"");

            //if (Convert.ToString(Session["Build"]) == "F")

            //    UserOptions.Add("Adminconsole", "\"display\":\"none\"♠\"onclick\":\"AdminConsoleOpenMenu();\"♠\"title\":\"Admin Console\"");
            //else
            //    UserOptions.Add("Adminconsole", "\"display\":\"block\"♠\"onclick\":\"AdminConsoleOpenMenu();\"♠\"title\":\"Admin Console\"");

            if (Convert.ToString(Session["Build"]) == "F")
                UserOptions.Add("AppConfig", "\"display\":\"none\"♠\"onclick\":\"LoadIframe('tstruct.aspx?transid=axstc')\"♠\"title\":\"Developer Options\"");
            else
                UserOptions.Add("AppConfig", "\"display\":\"none\"♠\"onclick\":\"LoadIframe('tstruct.aspx?transid=axstc')\"♠\"title\":\"Developer Options\"");
            if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
                UserOptions.Add("qrCodeBtnWrapper", "\"display\":\"none\"♠\"title\":\"Mobile QR\"");
            else
                UserOptions.Add("qrCodeBtnWrapper", "\"display\":\"block\"♠\"title\":\"Mobile QR\"");

            if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
                UserOptions.Add("splitIcon", "\"display\":\"none\"");
            else if (Session["MobileView"] != null && Session["MobileView"].ToString() == "False")
            {
                if (Session["AxDisableSplit"] != null)
                {
                    disableSplit = Session["AxDisableSplit"].ToString();
                    if (disableSplit == "true")
                        UserOptions.Add("splitIcon", "\"display\":\"none\"");
                    else
                        UserOptions.Add("splitIcon", "\"display\":\"block\"");
                }
            }
            else
                UserOptions.Add("splitIcon", "\"display\":\"block\"");

            if (Convert.ToString(Session["Build"]) == "F")
                UserOptions.Add("Dashboard", "\"display\":\"none\"♠\"onclick\":\"nodeApi ? ToogleLeftMenu('dashboardPanel', 'true','Dashboard','') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Dashboard\"");
            else
                UserOptions.Add("Dashboard", "\"display\":\"none\"♠\"onclick\":\"nodeApi ? ToogleLeftMenu('dashboardPanel', 'true','Dashboard','') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Dashboard\"");

            string isDevInstance = string.Empty;
            if (HttpContext.Current.Session["AxDevInstance"] != null)
                isDevInstance = HttpContext.Current.Session["AxDevInstance"].ToString();
            //string homepagews = string.Empty;
            if (ConfigurationManager.AppSettings["homepagews"] != null)
                homepagews = ConfigurationManager.AppSettings["homepagews"].ToString();

            bool hasPageBuildAccess = false;
            string[] buildModeAccessLength = PageBuilderAccess.Split(',');
            if (Session["AxResponsibilities"] != null)
            {
                string userResp = Session["AxResponsibilities"].ToString();
                string[] userRespArray = userResp.Split(',');
                for (var i = 0; i < buildModeAccessLength.Count(); i++)
                {
                    var list = userRespArray.Where(x => x == buildModeAccessLength[i]).ToList();
                    if (list.Count > 0)
                    {
                        hasPageBuildAccess = true;
                        break;
                    }
                }
            }
            HttpContext.Current.Session["hasPageBuildAccess"] = hasPageBuildAccess;
            if (homepagews == "true" && hasPageBuildAccess)
            {
                if (isDevInstance == "true")
                    UserOptions.Add("pageBuilderBtn", "\"display\":\"block\"♠\"onclick\":\"nodeApi ? LoadIframe('PageDesigner.aspx') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Page Builder\"♠\"div\":\"#pageBuilderBtn\"");
                else
                    UserOptions.Add("pageBuilderBtn", "\"display\":\"none\"♠\"onclick\":\"nodeApi ? LoadIframe('PageDesigner.aspx') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Page Builder\"♠\"div\":\"#pageBuilderBtn\"");
            }
            else if (hasPageBuildAccess)
                UserOptions.Add("pageBuilderBtn", "\"display\":\"block\"♠\"onclick\":\"nodeApi ? LoadIframe('PageDesigner.aspx') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Page Builder\"♠\"div\":\"#pageBuilderBtn\"");
            else
                UserOptions.Add("pageBuilderBtn", "\"display\":\"none\"♠\"onclick\":\"nodeApi ? LoadIframe('PageDesigner.aspx') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Page Builder\"♠\"div\":\"#pageBuilderBtn\"");

            UserOptions.Add("traceValue", "\"traceVal\":\"" + traceValue + "\"♠\"onclick\":\"ToggleShowLog(true)\"");
            UserOptions.Add("uiSelector", "\"title\":\"Modern UI\"");
            UserOptions.Add("modernUiValue", "\"onclick\":\"updateCompressedmode\"");
            if (Convert.ToString(Session["Build"]) == "F")
            {
                UserOptions.Add("developerworkbench", "\"display\":\"none\"♠\"title\":\"Axpert Developer\"");
                UserOptions.Add("axpertstudio", "\"display\":\"none\"♠\"title\":\"Axpert Developer Studio\"");
                //UserOptions.Add("pluginCustomCode", "\"display\":\"none\"♠\"title\":\"Axpert Developer\"");
            }
            else
            {
                if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
                {
                    UserOptions.Add("developerworkbench", "\"display\":\"none\"♠\"title\":\"Axpert Developer\"");
                    UserOptions.Add("axpertstudio", "\"display\":\"none\"♠\"title\":\"Axpert Developer Studio\"");
                    //UserOptions.Add("pluginCustomCode", "\"display\":\"none\"♠\"title\":\"Axpert Developer\"");
                }
                else
                {
                    string axpertdeveloperUrl = string.Empty;
                    if (ConfigurationManager.AppSettings["axpertdeveloper"] != null)
                    {
                        string adDetails = Session["project"].ToString();
                        adDetails += "♦" + Session["user"].ToString();
                        adDetails += "♦" + Session["pwd"].ToString();
                        adDetails += "♦" + Session["language"].ToString();
                        adDetails += "♦" + hybridGUID;
                        if (Session["AxAppTitle"] == null)
                            adDetails += "♦" + Session["projTitle"].ToString();
                        else
                            adDetails += "♦" + Session["AxAppTitle"].ToString();  //Session["axApps"].ToString();
                        adDetails += "♦" + userDetails;
                        axpertdeveloperUrl = ConfigurationManager.AppSettings["axpertdeveloper"].ToString();
                        adDetails = util.encrtptDecryptAES(adDetails);
                        axpertdeveloperUrl += "aspx/dwbsignin.aspx?adInfo=" + adDetails;
                    }
                    //UserOptions.Add("developerworkbench", "\"display\":\"block\"♠\"onclick\":\"showWorkBench('" + axpertdeveloperUrl + "')\"♠\"title\":\"Axpert Developer\"");
                    UserOptions.Add("developerworkbench", "\"display\":\"none\"♠\"title\":\"Axpert Developer\"");
                    UserOptions.Add("axpertstudio", "\"display\":\"block\"♠\"title\":\"Axpert Developer Studio\"");
                    //UserOptions.Add("pluginCustomCode", "\"display\":\"block\"♠\"onclick\":\"openPluginEditor()\"♠\"title\":\"Plugin Custom Code\"");
                }
            }

            if (Session["AppMgrAccess"] != null && Session["AppMgrAccess"].ToString() == "F")
                UserOptions.Add("configurationStudio", "\"display\":\"none\"♠\"onclick\":\"\"♠\"title\":\"Configuration\"");
            else
            {
                if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
                    UserOptions.Add("configurationStudio", "\"display\":\"none\"♠\"onclick\":\"\"♠\"title\":\"Configuration\"");
                else
                    UserOptions.Add("configurationStudio", "\"display\":\"block\"♠\"onclick\":\"OpenConfigurationStudio();\"♠\"title\":\"Configuration\"");
            }

            if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True" && hybridGUID != "")
            {
                UserOptions.Add("uiMobileNotification", "\"display\":\"none\"♠\"title\":\"Mobile Notification\"");
            }
            else
                UserOptions.Add("uiMobileNotification", "\"display\":\"none\"♠\"title\":\"Mobile Notification\"");
            try
            {
                StringBuilder usOprions = new StringBuilder();
                foreach (var option in UserOptions)
                {
                    usOprions.Append("\"" + option.Key + "\":{" + option.Value.Replace("♠", ",") + "},");
                }
                axUserOptions = "{" + usOprions.ToString().Substring(0, usOprions.Length - 1) + "}";
                FDW fdwObj = new FDW();
                bool IsCache = fdwObj.SaveInRedisServer(util.GetRedisServerkey(KeyData, "axUserOptions", Session["username"].ToString()), axUserOptions, Constants.REDISAXUSEROPTIONS, schemaName);
                if (IsCache == false)
                    Session["axUserOptions"] = axUserOptions;
                else
                    Session["axUserOptions"] = "cached";
            }
            catch (Exception ex)
            { }
        }

        try
        {
            string opsDetails = Session["project"].ToString();
            opsDetails += "♦" + Session["user"].ToString();
            opsDetails += "♦" + Session["pwd"].ToString();
            opsDetails += "♦" + Session["language"].ToString();
            opsDetails += "♦" + hybridGUID;
            opsDetails += "♦" + userDetails;
            opsDetails = util.encrtptDecryptAES(opsDetails);
            hdnOpenNewtabSite.Value = "aspx/autosignin.aspx?adInfo=" + opsDetails;
        }
        catch (Exception ex) { }
    }

    private void AddCustomLinks()
    {
        Custom cusObj = Custom.Instance;
        string projName = HttpContext.Current.Session["Project"].ToString();
        for (int i = 0; i < cusObj.jsMainFiles.Count; i++)
        {
            string fileName = cusObj.jsMainFiles[i].ToString();
            FileInfo filtcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/" + fileName));
            if (filtcustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("script");
                js.Attributes["type"] = "text/javascript";
                string path = "../" + projName + "/" + fileName + "?v=" + filtcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["src"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }

        for (int j = 0; j < cusObj.cssMainFiles.Count; j++)
        {
            string fileName = cusObj.cssMainFiles[j].ToString();
            FileInfo filtcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/" + fileName));
            if (filtcsscustom.Exists)
            {
                HtmlGenericControl css = new HtmlGenericControl("link");
                css.Attributes["type"] = "text/css";
                css.Attributes["rel"] = "stylesheet";
                string path = "../" + projName + "/" + fileName + "?v=" + filtcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                css.Attributes["href"] = path;
                ScriptManager1.Controls.Add(css);
            }
        }
    }

    private void CheckConfigLock()
    {
        if (!IsPostBack)
        {
            try
            {
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();

                string lockedBy = String.Empty;

                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fdrObj == null)
                    fdrObj = new FDR();
                lockedBy = fdrObj.StringFromRedis(Constants.CONFIG_LOCK_KEY, schemaName);
                string[] lockDetails = lockedBy.Split('♦');
                bool clearLock = false;

                if (lockDetails.Length == 4)
                {
                    DateTime lockedOn = Convert.ToDateTime(lockDetails[1]);
                    bool lockExpired = lockedOn.AddMinutes(10) < DateTime.Now;
                    if (lockDetails[0] != user)
                    {
                        if (lockExpired)
                            clearLock = true; //Different user + lock expired -> Clear Lock
                    }
                    else
                    {
                        if (HttpContext.Current.Session["lictype"] != null && HttpContext.Current.Session["lictype"].ToString() == "unlimited")
                        {
                            if (lockExpired)
                                clearLock = true; //Same user + un-limited user licence + lock expired -> Clear Lock
                        }
                        else
                            clearLock = true; //Same user + limited user licence -> Clear Lock
                    }
                }

                if (clearLock)
                    fdwObj.ClearRedisServerDataByKey(Constants.CONFIG_LOCK_KEY, Constants.CONFIG_LOCK_KEY, false, schemaName);
            }
            catch (Exception) { }
        }
    }

    private void SetPageLoadVars()
    {
        if (ConfigurationManager.AppSettings["NodeAPI"] != null)
            utilNode = ConfigurationManager.AppSettings["NodeAPI"].ToString();
        if (Convert.ToString(Session["Build"]) == "F")
        {
            //configApp.Style.Add("display", "none");
            configApp1.Style.Add("display", "none");
            UserOptions.Add("globalsettings", "\"display\":\"none\"♠\"onclick\":\"DoUtilitiesEvent('ConfigApp')\"♠\"title\":\"Global Settings\"");
        }
        else
            UserOptions.Add("globalsettings", "\"display\":\"block\"♠\"onclick\":\"DoUtilitiesEvent('ConfigApp')\"♠\"title\":\"Global Settings\"");

        if (Convert.ToString(Session["workflowEnabled"]) == "False" || Convert.ToString(Session["workflowEnabled"]) == "false")
        {
            li_WorkFlow1.Style.Add("display", "none");
            UserOptions.Add("WorkFlow", "\"display\":\"none\"♠\"onclick\":\"LoadIframe('workflownew.aspx')\"♠\"title\":\"WorkFlow\"");
        }
        else
            UserOptions.Add("WorkFlow", "\"display\":\"block\"♠\"onclick\":\"LoadIframe('workflownew.aspx')\"♠\"title\":\"WorkFlow\"");

        if (ConfigurationManager.AppSettings["AxpertWebLogs"] != null && ConfigurationManager.AppSettings["AxpertWebLogs"].ToString() == "true" && Session["user"].ToString() == "admin")
        {
            axpertLogs1.Style.Add("display", "block");
        }

        // FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/images/Custom/logo.png"));

        string projName = HttpContext.Current.Session["Project"].ToString();

        string customLogo = asbWebService.GetProjectAppLogo(projName);

        string logo = string.Empty;
        try
        {
            logo = JObject.Parse(customLogo)["logo"].ToString();
        }
        catch (Exception ex) { }

        if (logo != string.Empty)
        {

            IsCustomLogoAvail = true;
            UserOptions.Add("AxAppLogo", "\"imgSrc\":\"" + logo + "\"");
        }
        else
            UserOptions.Add("AxAppLogo", "\"imgSrc\":\"../images/loginlogo.png\"");
        if (Session["AppSessionKey"] != null && Session["AppSessionKey"].ToString() != "")
        {
            if (Session["lictype"] != null && Session["lictype"].ToString() == "limited")
                appsessionKey = "l~";
            else
                appsessionKey = "u~";
            appsessionKey += Session["AppSessionKey"].ToString();
        }
        if (Session["nsessionid"] != null)
        {
            hdnpsid = Session["nsessionid"].ToString();
        }
        if (Session["project"] != null)
        {
            //string projName = Session["project"].ToString();
            if (Application[projName + "-PeriodicRCKey"] != null)
                PeriodicRCKey = "true";
            else
            {
                Application[projName + "-PeriodicRCKey"] = "true";
                PeriodicRCKey = "false";
            }
        }
    }

    private void EncryptConnDtls()
    {
        if (!(string.IsNullOrEmpty(Session["axdb"].ToString()) && string.IsNullOrEmpty(Session["dbuser"].ToString()) && string.IsNullOrEmpty(Session["axconstr"].ToString())))
        {
            string strDB = string.Empty;
            if (Session["axdb"] != null && Session["axdb"].ToString() != String.Empty)
                strDB = Session["axdb"].ToString();

            Utl = Session["dbuser"].ToString() + "," + strDB + ",";
            string conStr = Session["axconstr"].ToString();
            string conId = string.Empty;
            string conpwd = string.Empty;

            if (strDB.ToLower() == "mysql" || strDB.ToLower() == "mariadb")
            {
                for (int i = 0; i < conStr.Split(';').Length; i++)
                {
                    string temp = conStr.Split(';')[i];
                    if (temp.Contains("Server"))
                        conId = temp.Split('=')[1];
                    if (temp.Contains("Pwd"))
                        conpwd = temp.Split('=')[1];
                }
            }
            else if (strDB.ToLower() == "ms sql" || strDB.ToLower() == "mssql")
            {
                for (int i = 0; i < conStr.Split(';').Length; i++)
                {
                    string temp = conStr.Split(';')[i];
                    if (temp.Contains("Server"))
                        conId = temp.Split('=')[1];
                    if (temp.Contains("Password"))
                        conpwd = temp.Split('=')[1];
                }
            }
            else if (strDB.ToLower() == "postgresql" || strDB.ToLower() == "postgre")
            {
                for (int i = 0; i < conStr.Split(';').Length; i++)
                {
                    string temp = conStr.Split(';')[i];
                    if (temp.Contains("Server"))
                        conId = temp.Split('=')[1];
                    if (temp.Contains("Pwd"))
                        conpwd = temp.Split('=')[1];
                    if (temp.Contains("Database"))
                    {
                        string dbUserName = Session["dbuser"].ToString();
                        dbUserName = dbUserName.Split('~')[0];
                        Utl = dbUserName + "\\" + temp.Split('=')[1] + "," + strDB + ",";
                    }
                }
            }

            else
            {
                for (int i = 0; i < conStr.Split(';').Length; i++)
                {
                    string temp = conStr.Split(';')[i];
                    if (temp.Contains("Data Source"))
                        conId = temp.Split('=')[1];
                    if (temp.Contains("Password"))
                        conpwd = temp.Split('=')[1];
                }
            }

            Utl += conId + "," + conpwd + "^~)";

            if (ConfigurationManager.AppSettings["EncryptionKey"] != null && ConfigurationManager.AppSettings["EncryptionIV"] != null && ConfigurationManager.AppSettings["EncryptionKey"] != string.Empty && ConfigurationManager.AppSettings["EncryptionIV"] != string.Empty)
            {
                string[] keyStr = ConfigurationManager.AppSettings["EncryptionKey"].ToString().Split(',');
                byte[] keyBytes = keyStr.Select(Byte.Parse).ToArray();
                string[] ivStr = ConfigurationManager.AppSettings["EncryptionIV"].ToString().Split(',');
                byte[] ivBytes = ivStr.Select(Byte.Parse).ToArray();
                byte[] encryptedUtl = util.EncryptStringToBytes_Aes(Utl, keyBytes, ivBytes);
                Utl = BitConverter.ToString(encryptedUtl).Replace("-", string.Empty);
                Session["utl"] = Utl;
            }
        }
        //if (ConfigurationManager.AppSettings["redisIP"] != null && ConfigurationManager.AppSettings["redisIP"].ToString() != String.Empty && ConfigurationManager.AppSettings["redisPass"] != null)
        //{
        //    redisUtl = ConfigurationManager.AppSettings["redisIP"].ToString() + "," + ConfigurationManager.AppSettings["redisPass"].ToString() + "^~)";

        //    if (ConfigurationManager.AppSettings["EncryptionKey"] != null && ConfigurationManager.AppSettings["EncryptionIV"] != null && ConfigurationManager.AppSettings["EncryptionKey"] != string.Empty && ConfigurationManager.AppSettings["EncryptionIV"] != string.Empty)
        //    {
        //        string[] keyStr = ConfigurationManager.AppSettings["EncryptionKey"].ToString().Split(',');
        //        byte[] keyBytes = keyStr.Select(Byte.Parse).ToArray();
        //        string[] ivStr = ConfigurationManager.AppSettings["EncryptionIV"].ToString().Split(',');
        //        byte[] ivBytes = ivStr.Select(Byte.Parse).ToArray();
        //        byte[] encryptedUtl = util.EncryptStringToBytes_Aes(redisUtl, keyBytes, ivBytes);
        //        redisUtl = BitConverter.ToString(encryptedUtl).Replace("-", string.Empty);
        //    }
        //}

        if (HttpContext.Current.Session["RedisCacheIP"] != null && HttpContext.Current.Session["RedisCacheIP"].ToString() != String.Empty)
        {
            redisUtl = HttpContext.Current.Session["RedisCacheIP"].ToString() + "," + HttpContext.Current.Session["RedisCachePwd"].ToString() + "^~)";

            if (ConfigurationManager.AppSettings["EncryptionKey"] != null && ConfigurationManager.AppSettings["EncryptionIV"] != null && ConfigurationManager.AppSettings["EncryptionKey"] != string.Empty && ConfigurationManager.AppSettings["EncryptionIV"] != string.Empty)
            {
                string[] keyStr = ConfigurationManager.AppSettings["EncryptionKey"].ToString().Split(',');
                byte[] keyBytes = keyStr.Select(Byte.Parse).ToArray();
                string[] ivStr = ConfigurationManager.AppSettings["EncryptionIV"].ToString().Split(',');
                byte[] ivBytes = ivStr.Select(Byte.Parse).ToArray();
                byte[] encryptedUtl = util.EncryptStringToBytes_Aes(redisUtl, keyBytes, ivBytes);
                redisUtl = BitConverter.ToString(encryptedUtl).Replace("-", string.Empty);
            }
        }
    }
    private void GetConfigs()
    {
        ObjExecTr.SetCurrentTimeMethod();
        string LandingPage = util.GetAdvConfigs("Change Password");
        if (LandingPage != "")
        {
            if (LandingPage == "disable")
            {
                changepassword.Visible = false;
                UserOptions.Add("ChangePassword", "\"display\":\"none\"♠\"onclick\":\"showChangePasswordDialog()\"♠\"title\":\"Reset Password\"");
            }
            else if (LandingPage == "enable")
            {
                changepassword.Visible = true;
                UserOptions.Add("ChangePassword", "\"display\":\"block\"♠\"onclick\":\"showChangePasswordDialog()\"♠\"title\":\"Reset Password\"");
            }
        }
        else
            UserOptions.Add("ChangePassword", "\"display\":\"block\"♠\"onclick\":\"showChangePasswordDialog()\"♠\"title\":\"Reset Password\"");
        string mainReload = util.GetAdvConfigs("Main Page Reload");
        if (mainReload != "")
        {
            axMainPageReload = mainReload;
            Session["AxReloadCloud"] = mainReload;
        }

        string MenuSize = util.GetAdvConfigs("Menu Size");
        if (MenuSize != "")
            axMenuReSize = MenuSize;
        string ProjectTile = util.GetAdvConfigs("Project Title");
        if (ProjectTile != "")
            axProjectTile = appName;
        string DateFormat = util.GetAdvConfigs("Date Format");
        if (DateFormat != "")
        {
            DateFormat = DateFormat.ToLower();
            Session["ClientLocale"] = DateFormat;
            glCulture = DateFormat;
        }

        googleMapsApiKey = util.GetAdvConfigs("Google Maps Api Key");

        GlobalParameterForm = util.GetAdvConfigs("Global Parameter Form");

        string ShowImaWidAction = util.GetAdvConfigs("Show Image Widget Action Button");
        if (ShowImaWidAction != string.Empty)
            ShowImaWidActionBtn = ShowImaWidAction;

        string navigationbutton = util.GetAdvConfigs("Show Navigation Button");
        if (navigationbutton != string.Empty)
            navigationshow = navigationbutton;
        if (Session["lictype"] != null && Session["lictype"].ToString() == "unlimited")
            localStorUser = "true";

        listviewAsDefault.Value = util.GetAdvConfigs("Listview as Default", "tstruct");

        listviewLoadFromSearch.Value = util.GetAdvConfigs("Listview as default from search", "tstruct");

        enableListviewExtraHyperlinks.Value = util.GetAdvConfigs("enable listview extra hyperlinks");

        string autoExtend = util.GetAdvConfigs("Keep Current Session Alive");
        if (autoExtend != string.Empty && autoExtend == "true")
            autoSessExtend = "true";
        else
            autoSessExtend = "false";

        //if (ConfigurationManager.AppSettings["AxOldModel"] != null && ConfigurationManager.AppSettings["AxOldModel"].ToString() == "true")
        //    axOldModelFlag = "true";
        //else
        //{
        axOldModelFlag = util.GetAdvConfigs("AxOldDataList");
        if (axOldModelFlag != string.Empty)
            Session["IsAxOldDataList"] = axOldModelFlag;
        else if (axOldModelFlag == string.Empty)
            axOldModelFlag = "false";
        //}

        try
        {
            DataTable configData = util.getAllGenConfigsData();
            if (configData != null)
            {
                DataSet ds = new DataSet();
                ds.Merge(configData.Copy());
                ds.DataSetName = "configurations";
                ds.Tables[0].TableName = "config";
                AxGeneralconfigs = JObject.Parse(JsonConvert.SerializeObject(ds));
            }
        }
        catch (Exception ex)
        {
            AxGeneralconfigs = new JObject();
        }
        string serverprocesstime = ObjExecTr.TotalServerElapsTimeMethod();
        requestProcess_logtime += " ♦ GetConfigs keys:" + serverprocesstime + " (This time is included in Response processed at server) ♦ ";
    }

    private void SetnotifyTimeout()
    {
        //notifyTimeout = util.GetAdvConfigs("notification time interval");        
    }

    private void SetLandingPage()
    {
        ObjExecTr.SetCurrentTimeMethod();
        string approle = "";
        approle = AxRole;
        //TODO:add condition for cloudapp
        if (ConfigurationManager.AppSettings["isCloudApp"] == null || ConfigurationManager.AppSettings["isCloudApp"].ToString().ToLower() == "false")
        {
            if (approle[approle.Length - 1] == ',' || approle[approle.Length - 1] == ';')
                approle = approle.Substring(0, approle.Length - 1);
        }

        //If there is a role specific default page then that will have higher priority, hence no todo check if approle is given
        string todo = "false";
        if (ViewState["todo"] != null)
            todo = Convert.ToString(ViewState["todo"]);

        #region new code for role check
        if (!string.IsNullOrEmpty(approle))
        {
            foreach (var role in approle.Split(','))
            {
                if (Application[role] != null)
                {
                    Navigationpage = Application[role].ToString();
                    break;
                }
            }
        }
        if (string.IsNullOrEmpty(Navigationpage))
        {
            if (todo.ToLower() == "true")
                Navigationpage = "../aspx/iview.aspx?ivname=todo";
            else if (Session["AxLandingPage"] != null)
                Navigationpage = Session["AxLandingPage"].ToString();
        }
        #endregion
        #region old code for role check
        //if (Application[approle] == null)
        //{
        //    if (todo.ToLower() == "true")
        //        Navigationpage = "../aspx/iview.aspx?ivname=todo";
        //    else if (Session["AxLandingPage"] != null)
        //        Navigationpage = Session["AxLandingPage"].ToString();
        //}
        //else
        //{
        //    Navigationpage = Application[approle].ToString();
        //}
        #endregion



        string compressedModeString = util.GetAdvConfigs("ApplicationCompressedMode");

        if (compressedModeString == "")
        {
            compressedMode = true;
        }
        else
        {
            compressedMode = compressedModeString == "true";
        }

        //showMenu = util.GetAdvConfigs("Show Application Menu on Login") != "false";
        string _showMenuOption = util.GetAdvConfigs("Show Application Menu on Login");
        showMenu = string.IsNullOrEmpty(_showMenuOption) ? false : bool.Parse(_showMenuOption);

        string LandingPage = util.GetAdvConfigs("Landing Structure");// util.GetAdvConfigs("Landing Structure");

        //string tempProj = "";

        string headerPathFull = string.Empty;
        string headerPath = string.Empty;

        headerPath = util.GetAdvConfigs("ApplicationTemplate");

        string applicationTemplatePath = "~/CustomPages/" + headerPath;

        string defaultTemplatePath = "~/aspx/mainPageTemplate.html";

        if (isHomeConfigPage == "true")
        {
            string _AxOldCards = util.GetAdvConfigs("AxOldCards");
            if (_AxOldCards == "true")
                defaultTemplatePath = "~/aspx/mainHomeConfigTemplate_old.html";
            else
                defaultTemplatePath = "~/aspx/mainHomeConfigTemplate.html";
        }

        try
        {
            if ((ConfigurationManager.AppSettings["AbMainPageHTML"] != null && ConfigurationManager.AppSettings["AbMainPageHTML"].ToString() == "true") || headerPath != string.Empty)
            {
                string projName = string.Empty;
                string path = string.Empty;
                if (HttpContext.Current.Session["Project"] != null)
                {
                    projName = HttpContext.Current.Session["Project"].ToString();
                    path = "~/CustomPages/" + projName + "-MainPage.html";
                }
                string deafultPath = "~/CustomPages/AgileBizMainPage.html";

                if (File.Exists(Server.MapPath(path)))
                {
                    headerPathFull = Server.MapPath(path);
                }
                else if (headerPath != "" && headerPath.ToLower().IndexOf(".html") >= 0 && File.Exists(Server.MapPath(applicationTemplatePath)))
                {
                    headerPathFull = Server.MapPath(applicationTemplatePath);
                }
                else if (ConfigurationManager.AppSettings["AbMainPageHTML"] != null && ConfigurationManager.AppSettings["AbMainPageHTML"].ToString() == "true" && File.Exists(Server.MapPath(deafultPath)))
                {
                    headerPathFull = Server.MapPath(deafultPath);
                }
                else if (File.Exists(Server.MapPath(defaultTemplatePath)))
                {
                    headerPathFull = Server.MapPath(defaultTemplatePath);
                }
            }
            else if (File.Exists(Server.MapPath(defaultTemplatePath)))
            {
                headerPathFull = Server.MapPath(defaultTemplatePath);
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("MainPage Templete - \n\tPath- " + headerPath + "\n\tError - " + ex.Message, HttpContext.Current.Session.SessionID, "MainPage Templete", "");
        }

        if (headerPathFull != string.Empty)
        {
            try
            {
                templateText = File.ReadAllText(headerPathFull);
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("MainPage Templete - \n\tPath- " + headerPath + "\n\tError - " + ex.Message, HttpContext.Current.Session.SessionID, "MainPage Templete", "");
            }
        }

        templateStage.Text = templateText;

        if (Session["userLandingPage"] != null)
        {
            userLandingPage = Session["userLandingPage"].ToString();
        }

        if (Session["rolesLandingPage"] != null)
        {
            rolesLandingPage = Session["rolesLandingPage"].ToString();
        }

        //Landing page can set in User login and user roles, and user logins is the priority. and developer option and application variable landing pages withdrawn but backward compatibility will be there.
        if (userLandingPage != "" && userLandingPage != "0" && userLandingPage != "1" && userLandingPage != "None" && userLandingPage.ToLower() != "page.aspx?axpage_id=0")
        {
            if (userLandingPage != "" && userLandingPage == "2")
                Navigationpage = "../aspx/processflow.aspx?activelist=t";
            else if (userLandingPage != "" && userLandingPage == "3")
                Navigationpage = "../aspx/processflow.aspx?dashboard=t";
            else if (userLandingPage != "" && userLandingPage == "4")
                Navigationpage = "../aspx/processflow.aspx?calendar=t";
            else if (userLandingPage != "" && userLandingPage.StartsWith("i"))
            {
                string userLanding = userLandingPage.Substring(1);
                Navigationpage = "../aspx/iview.aspx?ivname=" + userLanding;
            }
            else if (userLandingPage != "" && userLandingPage.StartsWith("t"))
            {
                string userLanding = userLandingPage.Substring(1);
                Navigationpage = "../aspx/tstruct.aspx?transid=" + userLanding + "&openerIV=" + userLanding + "&isIV=false";
            }
            else
                Navigationpage = userLandingPage;
        }
        else if (rolesLandingPage != "" && rolesLandingPage != "0" && rolesLandingPage != "1" && rolesLandingPage != "None" && rolesLandingPage.ToLower() != "page.aspx?axpage_id=0")
        {
            if (rolesLandingPage != "" && rolesLandingPage == "2")
                Navigationpage = "../aspx/processflow.aspx?activelist=t";
            else if (rolesLandingPage != "" && rolesLandingPage == "3")
                Navigationpage = "../aspx/processflow.aspx?dashboard=t";
            else if (rolesLandingPage != "" && rolesLandingPage == "4")
                Navigationpage = "../aspx/processflow.aspx?calendar=t";
            else if (rolesLandingPage != "" && rolesLandingPage.StartsWith("i"))
            {
                string userLanding = rolesLandingPage.Substring(1);
                Navigationpage = "../aspx/iview.aspx?ivname=" + userLanding;
            }
            else if (rolesLandingPage != "" && rolesLandingPage.StartsWith("t"))
            {
                string userLanding = rolesLandingPage.Substring(1);
                Navigationpage = "../aspx/tstruct.aspx?transid=" + userLanding + "&openerIV=" + userLanding + "&isIV=false";
            }
            else
                Navigationpage = rolesLandingPage;
        }
        else if (homePageType != "cards" && (userLandingPage != "" || rolesLandingPage != ""))
        {
            Navigationpage = "../aspx/Page.aspx";
        }
        else if (LandingPage != "")
        {
            string[] landingValue = LandingPage.Split('♦');
            if (landingValue[0].ToLower() == "tstruct")
            {
                Navigationpage = "../aspx/tstruct.aspx?transid=" + landingValue[1] + "&openerIV=" + landingValue[1] + "&isIV=false";
                if (Session["AxCloudDB"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>displayBootstrapModalDialog('Company SetUp', 'lg', '510px', true, '" + Navigationpage + "', true, cloudSetupInit)</script>");
                    Navigationpage = "";
                }
            }
            else if (landingValue[0].ToLower() == "iview")
            {
                Navigationpage = "../aspx/iview.aspx?ivname=" + landingValue[1];
                if (Session["AxCloudDB"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>displayBootstrapModalDialog('Company SetUp', 'lg', '510px', true, '" + Navigationpage + "', true, cloudSetupInit)</script>");
                    Navigationpage = "";
                }
            }
            else if (landingValue[0].ToLower() == "general")
            {
                Navigationpage = "../" + proj + "/aspx/" + landingValue[2];
                hdHomeUrl.Value = Navigationpage;
            }
        }


        if (lastOpenPage != string.Empty)
            hdKeepMeDefaultUrl.Value = lastOpenPage;
        if (hybridDefaultPage != string.Empty)
        {
            string hydefPageType = hybridDefaultPage.Split('^')[0];
            string hydefPage = hybridDefaultPage;
            if (hydefPageType == "t")
            {
                hydefPage = hydefPage.Substring(2);
                string parms = hydefPage.Replace("^", "&").Replace("~", "=");
                hdKeepMeDefaultUrl.Value = "tstruct.aspx?" + parms;
            }
            else if (hydefPageType == "i")
            {
                hydefPage = hydefPage.Substring(2);
                string parms = hydefPage.Replace("^", "&").Replace("~", "=");
                hdKeepMeDefaultUrl.Value = "ivtoivload.aspx?" + parms;
            }
            else if (hydefPageType == "p")
            {
                hydefPage = hydefPage.Substring(2);
                string parms = hydefPage.Replace("^", "&").Replace("~", "=");
                hdKeepMeDefaultUrl.Value = "Page.aspx?" + parms;
            }
            else if (hydefPageType == "h")
            {
                hydefPage = hydefPage.Substring(2);
                string parms = hydefPage.Replace("^", "&").Replace("~", "=");
                hdKeepMeDefaultUrl.Value = "htmlPages.aspx?" + parms;
            }
        }
        hdHomeUrl.Value = Navigationpage;
        string serverprocesstime = ObjExecTr.TotalServerElapsTimeMethod();
        requestProcess_logtime += " ♦ SetLandingPage:" + serverprocesstime + " (This time is included in Response processed at server) ♦ ";
    }

    private void SetAccessFromSession()
    {
        if (Session["IsSSO"] != null)
            fromSSO = Session["IsSSO"].ToString();

        if (Convert.ToString(Session["Build"]) == "T")
            build = true;

        if (Convert.ToString(Session["ImportAccess"]).ToLower() == "true")
            import = true;

        if (Convert.ToString(Session["ExportAccess"]).ToLower() == "true")
            export = true;

        if (Convert.ToString(Session["HistoryAccess"]).ToLower() == "true")
            history = true;

        if (Session["forcePwdChange"] != null)
            forcePwdChange = Session["forcePwdChange"].ToString();

        if (Session["draftScript"] != null)
            draftScript = Session["draftScript"].ToString();

        try
        {
            if (Session["FDR"] == null)
            {
                FDR fObj = new FDR();
                fObj.schemaNameKey = Session["dbuser"].ToString();
                Session["FDR"] = fObj;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void InitSessExpiryAlert()
    {
        //Session Alert start
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            string keepAlive = "false";
            if (Session["staySignedId"] != null)
                keepAlive = Session["staySignedId"].ToString();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
            SessionStateSection section = (SessionStateSection)config.GetSection("system.web/sessionState");
            int timeout = (int)section.Timeout.TotalMinutes * 1000 * 60;
            if (keepAlive == "true")
            {
                string ssotype = string.Empty;
                if (HttpContext.Current.Session["isSSOLogin"] != null && HttpContext.Current.Session["isSSOLogin"].ToString() == "True")
                    ssotype = HttpContext.Current.Session["SSOLoginType"].ToString();
                if (Session["hybridDeviceId"] == null || Session["hybridDeviceId"].ToString() == "")
                {
                    Session["KeepMeWebExpiry"] = "true";
                    util.SaveKeepAliveWebDetails("");
                }
                ClientScript.RegisterStartupScript(GetType(), "SessionAlert Var", "<script>var keepAlive = '" + keepAlive.ToString() + "';KeepSessionAlive(" + timeout + ");var ssologinType='" + ssotype + "';</script>");
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);

        }
        //Session Alert end
    }

    private void SetCloudSignoutPath()
    {
        if (Session["AxAxCLOUD"] != null)
            isCloud = Convert.ToBoolean(Session["AxAxCLOUD"].ToString());

        if (isCloud)
        {
            signOutPath = "../" + Session["domainName"];
            UserOptions.Add("Logout", "\"onclick\":\"signout('" + signOutPath + "')\"");
        }
        else
        {
            signOutPath = util.SIGNOUTPATH;
            UserOptions.Add("Logout", "\"onclick\":\"signout('" + signOutPath + "')\"");
        }
    }

    private void GetSessionVars()
    {
        if (ConfigurationManager.AppSettings["LoginTrace"] != null)
            loginTrace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
        if (Session["AxMenuStyle"] != null && Session["AxMenuStyle"].ToString() == "classic")
        {
            isNewMenu = "false";
            menuStyle = "classic";
        }
        if (Session["AxHomeBuildAccess"] != null)
            PageBuilderAccess = Session["AxHomeBuildAccess"].ToString();
        else
            PageBuilderAccess = "default";

        if (Session["themeColor"] != null)
            themeColor = Session["themeColor"].ToString();

        try
        {
            alertsTimeout = HttpContext.Current.Session["AxAlertTimeout"] != null ? (Convert.ToInt32(HttpContext.Current.Session["AxAlertTimeout"])) : 3000;
        }
        catch (Exception ex)
        {
            alertsTimeout = 3000;
        }
        //Added by Souvik
        //to show/hide error/failure alert messages based on config settings
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
        if (ConfigurationManager.AppSettings["isCloudApp"] != null)
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString());

        if (HttpContext.Current.Application["ScriptsPath"] != null)
            scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        if (ConfigurationManager.AppSettings["RestDllPath"] != null)
            RestDllPath = ConfigurationManager.AppSettings["RestDllPath"].ToString();


        if (Session["project"] != null && Session["project"].ToString() != "")
            util.GetAxARMConnection(Session["project"].ToString());

        if (Session["project"] != null && Session["project"].ToString() != "")
            util.GetAxConfigFileServer(Session["project"].ToString());

        if (Session["ARM_Scripts_URL"] != null)
        {
            ArmRestDllPath = Session["ARM_Scripts_URL"].ToString();
        }

        if (Session["ARM_PrivateKey"] != null)
        {
            armKey = Session["ARM_PrivateKey"].ToString();
        }

        if (Session["ARMPEG"] != null)
        {
            isPEG = Session["ARMPEG"].ToString();
        }

        if (Session["ARM_URL"] != null && Session["ARM_URL"].ToString() != "")
        {
            signalRNotifications = "true";
            armUrl = Session["ARM_URL"].ToString();
        }
        else
            signalRNotifications = "false";

        if (Session["AxCopyRightText"] != null)
        {
            copyRightTxt = Session["AxCopyRightText"].ToString();
            copyRightTxt = copyRightTxt.Replace("#br#", "<br/>");
        }
        if (Session["AxTrace"] == null || Session["AxTrace"].ToString() == "")
            Session["AxTrace"] = "false";
        if (Session["AxTrace"] != null)
            traceValue = Convert.ToBoolean(Session["AxTrace"]);
        if (Session["AxHelpIview"] != null)
            HelpIview = Session["AxHelpIview"].ToString();
        loginPath = Application["LoginPath"].ToString();
    }

    private void CheckCloudRequest()
    {
        if (Request.QueryString["id"] != null)
        {
            string result = string.Empty;
            string appDetails = string.Empty;
            Session["Thmcolor"] = "Blue";

            string guid = Request.QueryString["ID"].ToString();

            result = util.GetAppAndUserInfo(guid, Request.UserHostAddress);

            if (result.ToLower().Contains("error"))
            {
                Response.Redirect(util.ACERRPATH + "1_Error_In_Connection");
            }
            else if (result != string.Empty)
            {
                string[] usrDetails = result.Split('~');
                foreach (string usrStr in usrDetails)
                {
                    int idx = usrStr.IndexOf("=");
                    if (idx != -1 && usrStr.Substring(0, idx).ToLower() != "appdb" && usrStr.Substring(0, idx).ToLower() != "dbtype")
                    {
                        Session[usrStr.Substring(0, idx)] = usrStr.Substring(idx + 1);
                    }
                    else if (usrStr.Substring(0, idx).ToLower() == "appdb")
                    {
                        appDetails = usrStr.Substring(idx + 1);
                        appDetails = appDetails.Replace("[fs]", "/").Replace("[bs]", "\\");

                    }
                }

                result = util.GetCloudGlobalVariables(appDetails);
                if (result.ToLower().Contains("error"))
                {
                    Response.Redirect(util.ACERRPATH + "2");
                }
                else
                {
                    ParseGlobalVars(result);
                    LogFile.Log objLog = new LogFile.Log();
                    objLog.CreateLog(result, "", "ParseGlobalVars()", "new");
                }
            }
        }
        if (Session["IscloudRelogin"] != null && Session["IscloudRelogin"].ToString() == "true")
        {
            Session["IscloudRelogin"] = null;
            string result = util.GetCloudGlobalVariables();
            if (result.ToLower().Contains("error"))
            {
                Response.Redirect(util.ACERRPATH + "2");
            }
            else
            {
                ParseGlobalVars(result);
                LogFile.Log objLog = new LogFile.Log();
                objLog.CreateLog(result, "", "ParseGlobalVars()", "new");
            }
        }
    }

    //This function is referred in the cient side
    private void checkDashboard()
    {
        if (HttpContext.Current.Session["user"] != null)
        {
            DataSet dsdasboard = new DataSet();
            DBContext obj = new DBContext();
            dsdasboard = obj.GetMainPageDBInline("main");
            if (dsdasboard.Tables.Count > 0)
            {
                if (dsdasboard.Tables[0].Rows.Count != 0)
                {
                    if (dsdasboard.Tables[0].Rows[0]["rcount"].ToString() != "0" && !string.IsNullOrEmpty(dsdasboard.Tables[0].Rows[0]["rcount"].ToString()))
                    {
                        //hdndashBoardIcon.Value = "t";
                    }
                }
            }
        }
        else
        {
            SessExpiresStatic();
        }
    }
    [WebMethod]
    public static void delALLNotificiationKeyfromRedis(string key)
    {
        string schemaName = string.Empty;
        string userName = string.Empty;
        FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
        FDW fdwObj = new FDW();
        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();
        if (HttpContext.Current.Session["username"] != null)
            userName = HttpContext.Current.Session["username"].ToString();
        string NotifyKey = schemaName + "-" + userName + Constants.NOTIFICATION_PHASE;
        //string NotifyKey = schemaName + "-" + HttpContext.Current.Session["username"].ToString() + Constants.NOTIFICATION_PHASE;
        ArrayList NotifyKeys = fdrObj.GetAllKeys(NotifyKey);
        if (NotifyKeys.Count > 0)
        {
            foreach (string item in NotifyKeys)
            {

                fdwObj.DeleteAllKeys(item, schemaName);
            }
        }
    }

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
    public static string getGlobalSearchData(string keyword, string cond)
    {
        DateTime startTime = DateTime.Now;
        DataSet dsSearchData = new DataSet();
        string json = string.Empty; int limit = 10000;
        if (keyword.Contains("'")) keyword = keyword.Replace("'", "''");
        string srchKey = "'%" + keyword + "%'";
        if (cond == "StartsWith") srchKey = srchKey.Replace("'%", "'");
        int SessCount = 0; string SessKeyword = string.Empty;
        bool pickFromCache = false; bool isKeyChanged = false;
        int newFilterCount = 0; bool callService = false;
        string sessCond = "Contains";
        LogFile.Log logObj = new LogFile.Log();
        DataTable filteredData = new DataTable();
        try
        {
            logObj.CreateLog("Timetaken in milliseconds details for keyword-" + keyword + ",Cond=" + cond, HttpContext.Current.Session.SessionID, "GlobalSearch-Time", "new");
            if (HttpContext.Current.Session["user"] != null)
            {
                string axLanguage = HttpContext.Current.Session["language"].ToString();
                logObj.CreateLog("Start to search with keyword-" + keyword, HttpContext.Current.Session.SessionID, "GlobalSearch", "new");
                if (HttpContext.Current.Session["AxGlobalSrchLimit"] != null)
                {
                    if (HttpContext.Current.Session["AxGlobalSrchLimit"].ToString() != string.Empty)
                        limit = Convert.ToInt32(HttpContext.Current.Session["AxGlobalSrchLimit"].ToString());
                }

                //If data is available in session set varaibles from session 
                if (HttpContext.Current.Session["Axp_App_Srch"] != null)
                {
                    try
                    {
                        DateTime st1 = DateTime.Now;
                        dsSearchData = (DataSet)HttpContext.Current.Session["Axp_App_Srch"];
                        if (dsSearchData.Tables.Count > 0)
                            SessCount = dsSearchData.Tables[0].Rows.Count;
                        SessKeyword = HttpContext.Current.Session["Axp_App_SrchKey"].ToString();
                        sessCond = HttpContext.Current.Session["Axp_App_SrchCond"].ToString();
                        logObj.CreateLog("Getting data from session-" + st1.Subtract(DateTime.Now).TotalMilliseconds.ToString(), HttpContext.Current.Session.SessionID, "GlobalSearch-Time", "");
                    }
                    catch (Exception ex)
                    {
                        callService = true;
                        logObj.CreateLog("Getting data from session- exception:" + ex.Message, HttpContext.Current.Session.SessionID, "GlobalSearch-Time-exception", "new", "true");
                    }
                }
                else
                    callService = true;

                if (SessKeyword == keyword)
                {
                    pickFromCache = true;
                    try
                    {
                        if (dsSearchData.Tables.Count > 0)
                            filteredData = dsSearchData.Tables[0];
                    }
                    catch (Exception ex)
                    {
                        callService = true;
                        logObj.CreateLog("Getting data from filteredData - exception:" + ex.Message, HttpContext.Current.Session.SessionID, "GlobalSearch-filteredData-exception", "new", "true");
                    }
                }
                else if (SessKeyword != "" && keyword.StartsWith(SessKeyword))
                {
                    pickFromCache = true;
                    isKeyChanged = true;
                }
                else
                {
                    pickFromCache = false;
                    callService = true;
                    HttpContext.Current.Session.Remove("Axp_App_SrchKey");
                    HttpContext.Current.Session.Remove("Axp_App_Srch");
                }

                //If the condition modified from contains to starts with or vice-versa the service should be called
                if (sessCond != cond)
                {
                    pickFromCache = false;
                    callService = true;
                }

                if (pickFromCache)
                {
                    //First If - Key is changed and session has data                    
                    if (isKeyChanged && dsSearchData != null)
                    {
                        try
                        {
                            DateTime st2 = DateTime.Now;
                            filteredData = FilterSearchTable(dsSearchData.Tables[0], keyword.ToLower());
                            newFilterCount = filteredData.Rows.Count;
                            logObj.CreateLog("Filtering data from session-" + st2.Subtract(DateTime.Now).TotalMilliseconds.ToString(), HttpContext.Current.Session.SessionID, "GlobalSearch-Time", "");
                        }
                        catch (Exception ex)
                        {
                            callService = true;
                            logObj.CreateLog("Filtering data from session- exception:" + ex.Message, HttpContext.Current.Session.SessionID, "GlobalSearch-FilterSearchTable-Time", "new", "true");
                        }
                    }

                    if (SessCount == limit && newFilterCount == 0 && isKeyChanged)
                    {
                        HttpContext.Current.Session.Remove("Axp_App_SrchKey");
                        HttpContext.Current.Session.Remove("Axp_App_Srch");
                        callService = true;
                    }
                    else if (SessCount == limit && isKeyChanged)
                    {
                        HttpContext.Current.Session.Remove("Axp_App_SrchKey");
                        HttpContext.Current.Session.Remove("Axp_App_Srch");
                        callService = true;
                    }
                    else if (SessCount < limit && isKeyChanged && filteredData != null && filteredData.Rows.Count > 0 && newFilterCount == 0)
                    {
                        callService = true;
                    }
                    //else if (SessCount < limit && newFilterCount != 0 && isKeyChanged && filteredData != null)
                    else if (SessCount < limit && isKeyChanged && filteredData != null && filteredData.Rows.Count > 0)
                    {
                        try
                        {
                            dsSearchData = new DataSet();
                            dsSearchData.Tables.Add("Table");
                            dsSearchData.Tables["Table"].Merge(filteredData);
                            callService = false;
                            logObj.CreateLog("Getting from cache-" + keyword + "-with count =" + newFilterCount, HttpContext.Current.Session.SessionID, "GlobalSearch", "");
                        }
                        catch (Exception ex)
                        {
                            callService = true;
                            logObj.CreateLog("Getting from cache-" + keyword + "-with count =" + newFilterCount + " exception:" + ex.Message, HttpContext.Current.Session.SessionID, "GlobalSearch-exception-Merge", "new", "true");
                        }
                    }
                    else if (filteredData == null || filteredData.Rows.Count == 0)
                    {
                        callService = true;
                    }
                    //If the sessCount is less than the limit then it will pick from cache                  
                }
                if (callService)
                {
                    DateTime st3 = DateTime.Now;
                    DBContext obj = new DBContext();
                    string query = "";
                    if (axLanguage.ToLower() == "english")
                        query = Constants.GET_SEARCH_DATA.Replace("$KEYWORD$", srchKey.ToLower());
                    else
                    {
                        string _db_type = HttpContext.Current.Session["axdb"].ToString();
                        if (_db_type.ToLower() == "ms sql")
                            query = Constants.GET_SEARCH_DATA_LANG_MSSQL.Replace("$KEYWORD$", srchKey.ToLower()).Replace(":language", "'" + axLanguage.ToLower() + "'");
                        else
                            query = Constants.GET_SEARCH_DATA_LANG.Replace("$KEYWORD$", srchKey.ToLower()).Replace(":language", "'" + axLanguage.ToLower() + "'");
                    }
                    logObj.CreateLog("Getting from DB-" + keyword + "-with query =" + query, HttpContext.Current.Session.SessionID, "GlobalSearch", "");
                    dsSearchData = obj.GetSearchData(query, limit);
                    if (HttpContext.Current.Session["AxDirectDBException"] != null)
                    {
                        string exMSg = "error:" + HttpContext.Current.Session["AxDirectDBException"].ToString();
                        HttpContext.Current.Session.Remove("AxDirectDBException");
                        return exMSg;
                    }
                    logObj.CreateLog("Getting data from DB-" + st3.Subtract(DateTime.Now).TotalMilliseconds.ToString(), HttpContext.Current.Session.SessionID, "GlobalSearch-Time", "");

                    //If view is not present or data is not returned from view- get the data filtered on menu
                    if (dsSearchData == null || dsSearchData.Tables.Count == 0 || (dsSearchData.Tables.Count > 0 && dsSearchData.Tables[0].Rows.Count == 0))
                    {
                        DataTable dt = new DataTable();
                        dt = GetMenu();
                        if (dt.Rows.Count > 0)
                        {
                            dsSearchData = new DataSet();
                            dsSearchData.Tables.Add("Table");
                            dsSearchData.Tables[0].Merge(FilterSearchTable(dt, keyword.ToLower(), cond));
                        }
                        else
                            dsSearchData = new DataSet();
                        logObj.CreateLog("Getting from Menu-" + keyword, HttpContext.Current.Session.SessionID, "GlobalSearch", "");
                    }
                }

                //If the records in DB is

                HttpContext.Current.Session["Axp_App_Srch"] = dsSearchData;
                HttpContext.Current.Session["Axp_App_SrchKey"] = keyword;
                HttpContext.Current.Session["Axp_App_SrchCond"] = cond;

                json = GetPageWiseSearchData(0, 100, dsSearchData.Tables[0]);
                logObj.CreateLog("Result =" + json, HttpContext.Current.Session.SessionID, "GlobalSearch", "");
            }
            else
            {
                json = "Session Expired";
            }
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Getting exception in getGlobalSearchData- stack:" + ex.StackTrace + " Exception:" + ex.Message + " -with keyword =" + keyword + " callService:" + callService + " json:" + json, HttpContext.Current.Session.SessionID, "GlobalSearch-exception", "new", "true");
            return "getting exception in code";
        }

        logObj.CreateLog("Total GetGlobalSearchServerTime-" + startTime.Subtract(DateTime.Now).TotalMilliseconds.ToString(), HttpContext.Current.Session.SessionID, "GlobalSearch-Time", "");
        return json;
    }


    private static DataTable FilterSearchTable(DataTable dt, string keyword, string cond = "")
    {
        DataTable filteredData = dt;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (cond == "StartsWith")
            {
                var data = dt.AsEnumerable().Where(x => x.Field<string>("SEARCHTEXT").ToLower().StartsWith(keyword)).Select(x => new { SEARCHTEXT = x.Field<string>("SEARCHTEXT"), HLTYPE = x.Field<string>("HLTYPE"), STRUCTNAME = x.Field<string>("STRUCTNAME"), PARAMS = x.Field<string>("PARAMS"), OLDAPPURL = x.Field<string>("OLDAPPURL") }).ToList();
                filteredData = data.ToDataTable();
            }
            else
            {
                var data = dt.AsEnumerable().Where(x => x.Field<string>("SEARCHTEXT").ToLower().Contains(keyword)).Select(x => new { SEARCHTEXT = x.Field<string>("SEARCHTEXT"), HLTYPE = x.Field<string>("HLTYPE"), STRUCTNAME = x.Field<string>("STRUCTNAME"), PARAMS = x.Field<string>("PARAMS"), OLDAPPURL = x.Field<string>("OLDAPPURL") }).ToList();
                filteredData = data.ToDataTable();
            }
        }
        return filteredData;
    }

    [WebMethod]
    public static string GetMoreSearchData(string pageIndex, string pageSize)
    {
        DateTime st = DateTime.Now;
        string result = string.Empty;
        LogFile.Log logObj = new LogFile.Log();
        if (HttpContext.Current.Session["Axp_App_Srch"] != null)
        {
            DataSet ds = (DataSet)HttpContext.Current.Session["Axp_App_Srch"];
            try
            {
                int nextRec = int.Parse(pageIndex) * int.Parse(pageSize);
                result = GetPageWiseSearchData(int.Parse(pageIndex), int.Parse(pageSize), ds.Tables[0]);
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception while bringing more records on scroll with page Index=" + pageIndex + "and Exception details=" + ex.Message, HttpContext.Current.Session.SessionID, "GlobalSearch", "");
                result = JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);
            }
        }
        logObj.CreateLog("Total timetaken on scroll-pageno=" + pageIndex + st.Subtract(DateTime.Now).TotalMilliseconds.ToString(), HttpContext.Current.Session.SessionID, "GlobalSearch-Time", "");

        return result;
    }

    [WebMethod]
    public static string saveCardsDesign(string design)
    {
        string result = string.Empty;

        ASB.WebService asbWebService = new ASB.WebService();

        result = asbWebService.saveCardsDesign(design);

        return result;
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

    [WebMethod]
    public static string cardScripts(string cardId, string btnName)
    {
        string result = string.Empty;

        Util.Util util = new Util.Util();

        string cardJson = saveLoadCardsToRedis(createDummyCards(cardId), false, false);

        try
        {
            JArray cardArray = JArray.Parse(cardJson);
            JObject thisCard = (JObject)cardArray[0];

            JObject btnObj = (JObject)thisCard["cardbuttons"]["btnoption"].Where(btn => (string)btn["Button"] == btnName).FirstOrDefault();

            string scripts = btnObj["scripts"].ToString();


            try
            {
                LogFile.Log logobj = new LogFile.Log();

                string actFileName = "cardscripts-" + cardId + "-" + btnName;

                string errLog = logobj.CreateLog("Call to RemoteDoAction Web Service", HttpContext.Current.Session["nsessionid"].ToString(), actFileName, string.Empty);

                string iXml = "<root axpapp =\"" + HttpContext.Current.Session["project"].ToString() + "\" trace =\"" + errLog + "\" sessionid =\"" + HttpContext.Current.Session["nsessionid"].ToString() + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' stype=\"\" sname=\"\" actname=\"axpcardscripts\" ><cardscripts>" + scripts + "</cardscripts>";
                iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

                string ires = string.Empty;

                //Call service
                if (iXml != string.Empty)
                    iXml = iXml.Replace("<root", "<root scriptpath='" + ConfigurationManager.AppSettings["ScriptsPath"].ToString() + "'");

                ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                result = objWebServiceExt.callRemoteDoScriptWS(actFileName, iXml, ires, 100000);
            }
            catch (Exception ex)
            { }


            //result = ires;
        }
        catch (Exception ex)
        { }

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

            //string redisReturnString = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISCARDROLES, ""), schemaName);

            //if (redisReturnString != string.Empty)
            //{
            //try
            //{
            //cardRoles = JObject.Parse(redisReturnString);

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
            //}
            //catch (Exception ex)
            //{ }
            //}
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
                //JArray fields = (JArray)obj["cardsql"]["fields"];

                //JValue paramvars = fields.Select(c => (JValue)c["paramvars"]).Where(e => e != null).ToList().FirstOrDefault();

                //paramString = paramvars.ToString();

                paramString = obj["cardsql"]["paramvars"].ToString();

                //paramString = String.Join("~", paramString.Split('~').Distinct().ToList());
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

                            string redisParamString = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISCARDPARAMS, obj[cardsIdAccess].ToString()), schemaName);

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

                            string redisCardData = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISCARDKEYS, finalKeyAccess), schemaName);

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

    public static string GetPageWiseSearchData(int pageIndex, int pageSize, DataTable dtData)
    {
        string result = string.Empty;
        int nextRec = pageIndex * pageSize;
        int gsRowCount = 0;
        if (dtData.Rows.Count > 0)
        {
            IEnumerable<DataRow> MyDataPage = dtData.AsEnumerable().Skip(nextRec).Take(pageSize);
            DataTable NewDT = MyDataPage.CopyToDataTable();
            DataSet ds1 = new DataSet();
            ds1.Tables.Add("Table");
            ds1.Tables["Table"].Merge(NewDT);
            result = JsonConvert.SerializeObject(ds1, Newtonsoft.Json.Formatting.Indented);
            if (ds1.Tables.Count > 0 && pageIndex == 0)
                gsRowCount = dtData.Rows.Count;
            else
                gsRowCount = ds1.Tables[0].Rows.Count;
            result = gsRowCount + "*♦*" + result;
        }
        return result;
    }

    #region global search
    public static DataTable GetMenu()
    {
        string iXml = string.Empty;
        string errlog = string.Empty;
        string fileName = string.Empty;
        string searchtext = string.Empty;
        string hltype = string.Empty;
        string oldappurl = string.Empty;
        string structname = string.Empty;
        string strparams = string.Empty;
        string result = string.Empty;
        try
        {
            Util.Util objUtil = new Util.Util();
            string fdKeyMenuData = Constants.REDISMENUDATA;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj != null)
                result = fObj.StringFromRedis(objUtil.GetRedisServerkey(fdKeyMenuData, "Menu"), schemaName);
        }
        catch (Exception) { }

        if (result == string.Empty && HttpContext.Current.Session["MenuData"] != null)
            result = HttpContext.Current.Session["MenuData"].ToString();
        if ((result.Length == 0) || (result.Substring(1, 7) == Constants.ERROR))
        {
            return null;
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(result);

        DataTable dt = new DataTable();

        dt.Columns.Add("SEARCHTEXT");
        dt.Columns.Add("HLTYPE");
        dt.Columns.Add("STRUCTNAME");
        dt.Columns.Add("PARAMS");
        dt.Columns.Add("OLDAPPURL");

        string max = "0";
        XmlNodeList RootNodes = default(XmlNodeList);
        RootNodes = xmlDoc.SelectNodes("/root");
        // To get the maximum depth value at root node     
        foreach (XmlNode RootNode in RootNodes)
        {
            max = RootNode.Attributes["max"].Value;
        }
        XmlNodeList parentNodes = default(XmlNodeList);
        try
        {
            parentNodes = RootNodes[0].ChildNodes;
            foreach (XmlNode menuchldNode in parentNodes)
            {
                //Parent Node
                searchtext = menuchldNode.Attributes["name"].Value;
                hltype = menuchldNode.Attributes["target"].Value;
                oldappurl = menuchldNode.Attributes["oldappurl"].Value;
                if (!string.IsNullOrEmpty(hltype))
                {
                    structname = hltype.Split('=')[1];
                    hltype = hltype.Split('.')[0];
                    dt = AddRow(dt, searchtext, hltype, structname, strparams, oldappurl);
                }
                XmlNodeList zeroChldNode = default(XmlNodeList);
                zeroChldNode = menuchldNode.ChildNodes;
                foreach (XmlNode firstChildNodes in zeroChldNode)
                {
                    //First child Node
                    searchtext = firstChildNodes.Attributes["name"].Value;
                    hltype = firstChildNodes.Attributes["target"].Value;
                    oldappurl = menuchldNode.Attributes["oldappurl"].Value;
                    if (!string.IsNullOrEmpty(hltype))
                    {
                        structname = hltype.Split('=')[1];
                        hltype = hltype.Split('.')[0];
                        dt = AddRow(dt, searchtext, hltype, structname, strparams, oldappurl);
                    }
                    foreach (XmlNode secondChldNode in firstChildNodes)
                    {
                        //Second child Node
                        searchtext = secondChldNode.Attributes["name"].Value;
                        hltype = secondChldNode.Attributes["target"].Value;
                        oldappurl = menuchldNode.Attributes["oldappurl"].Value;
                        if (!string.IsNullOrEmpty(hltype))
                        {
                            structname = hltype.Split('=')[1];
                            hltype = hltype.Split('.')[0];
                            dt = AddRow(dt, searchtext, hltype, structname, strparams, oldappurl);
                        }
                        foreach (XmlNode thirdChildNodes in secondChldNode)
                        {
                            //Third child Node
                            searchtext = thirdChildNodes.Attributes["name"].Value;
                            hltype = thirdChildNodes.Attributes["target"].Value;
                            oldappurl = menuchldNode.Attributes["oldappurl"].Value;
                            if (!string.IsNullOrEmpty(hltype))
                            {
                                structname = hltype.Split('=')[1];
                                hltype = hltype.Split('.')[0];
                                dt = AddRow(dt, searchtext, hltype, structname, strparams, oldappurl);
                            }
                        }
                    }
                }
            }
        }
        catch
        {

        }
        return dt;
    }

    private static DataTable AddRow(DataTable dt, string parent, string child, string target, string level, string oldappurl)
    {
        DataRow dr = dt.NewRow();
        dr["SEARCHTEXT"] = parent;
        dr["HLTYPE"] = child;
        dr["STRUCTNAME"] = target;
        dr["PARAMS"] = level;
        dr["OLDAPPURL"] = oldappurl;
        dt.Rows.Add(dr);
        return dt;
    }
    #endregion

    private static void SessExpiresStatic()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
    }

    private void ParseGlobalVars(string result)
    {
        StringBuilder globalVarToShow = new StringBuilder();
        Dictionary<string, string> vars = new Dictionary<string, string>();
        string[] aryGloVar = result.Split('~');
        Session["isowner"] = false;
        Dictionary<string, string> NewglobalVariables = new Dictionary<string, string>();
        foreach (var gloVar in aryGloVar)
        {
            string[] tempVar = gloVar.Split('=');
            if (tempVar[0].ToString() == "axglo_recordid")
                Session["AxGloRecId"] = tempVar[1].ToString();
            else if (tempVar[0].ToString() == "AXGLO_HIDE")
                Session["AxGloHideShow"] = tempVar[1].ToString();
            else if (tempVar[0].ToString() == "AXP_DISPLAYTEXT")
            {
                string sbDspText = string.Empty;
                globalVarToShow.Append(sbDspText + FormatGlobalVariables(tempVar[1].ToString()));
            }
            else if (tempVar[0].ToString() == "responsibilies")
            {
                Session["AxResponsibilities"] = tempVar[1].ToString();
                if (tempVar[1] != null && tempVar[1].ToString().Contains("default"))
                {
                    Session["isowner"] = true;
                }
            }
            if (tempVar[0] != "Done#")
                NewglobalVariables.Add(tempVar[0], tempVar[1]);
        }
        string golVarNode = "<globalvars>";

        ArrayList UpglobalVariables = GetGlobalVarsArray("globalvar");

        if (UpglobalVariables == null)
            UpglobalVariables = new ArrayList();
        StringBuilder paramlist = new StringBuilder();
        int i = 0;
        int paramCnt = 0;
        foreach (string item in UpglobalVariables)
        {
            int valueIndex = item.IndexOf("=");
            string key = item.Substring(0, valueIndex);
            string val = string.Empty;
            if (NewglobalVariables.Count > 0)
                val = NewglobalVariables.AsEnumerable().Where(x => x.Key == key).Select(y => y.Value).FirstOrDefault();
            if (val == null)
                val = item.Substring(valueIndex + 1);
            golVarNode += "<" + key + ">" + val + "</" + key + ">";
            val = util.CheckSpecialChars(val);
            if (!Constants.globalVarPathVaribles.Contains(key.ToLower()))
            {
                paramlist.Append("Parameters[" + i + "] = " + "\"" + key + "~" + val + "\";  ");
                i++;
                paramCnt = paramCnt + 1;
            }
            if (key == "project" && val.Contains(','))
                Session[key] = val.Split(',')[1];
            else
                Session[key] = val;
        }
        golVarNode += util.VirtualFilePathUpdate();
        golVarNode += util.GetGblVarsFromIni();
        golVarNode += "</globalvars>";
        Session["axGlobalVars"] = golVarNode;
        DisplayGloVars(globalVarToShow.ToString());

        string dtCulture = Request.UserLanguages[0];
        //If useCulture flag is false , we are not checking the browser culture.
        //If it's true ,we are setting the culture based on the client browser language.
        if (Session["AxUSCulture"] != null)
            useCulture = Convert.ToBoolean(Session["AxUSCulture"].ToString());
        if (!useCulture)
            dtCulture = "en-gb";
        else
        {
            if (dtCulture.ToLower() == "en" || dtCulture.ToLower() == "en-us")
                dtCulture = "en-us";
        }
        paramlist.Append("Parameters[" + paramCnt + "] = " + "\"username~" + user + "\";  ");
        paramlist.Append("Parameters[" + (paramCnt + 1) + "] = " + "\"Culture~" + dtCulture + "\";  ");
        Session["ClientLocale"] = dtCulture;
        glCulture = dtCulture;
        if (paramlist.ToString() != string.Empty)
            util.SetGlobalVarString(paramlist.ToString());
    }

    private string FormatGlobalVariables(string gloVarStr)
    {
        string result = string.Empty;
        if (gloVarStr == "")
        {
            return "";
        }

        try
        {
            editAxGlo2.Visible = true;
            var AxGlo = UserOptions.Where(x => x.Key == "GlobalParams").ToList();
            if (AxGlo.Count > 0)
                UserOptions["GlobalParams"] = "\"display\":\"block\"♠\"title\":\"Global Parameters\"";
            else
                UserOptions.Add("GlobalParams", "\"display\":\"block\"♠\"title\":\"Global Parameters\"");
            bool isEditbtnVsbl = false;
            StringBuilder sbDspText = new StringBuilder("");
            StringBuilder sbAppParamsDspText = new StringBuilder("");
            string[] displayText = gloVarStr.Split(',');
            if (isNewMenu != "true")
            {
                foreach (var txtStr in displayText)
                {
                    string[] innerGloVar = txtStr.Split(':');
                    if (innerGloVar.Length > 1)
                    {
                        if (innerGloVar[1].ToString() != "")
                        {
                            sbDspText.Append(innerGloVar[1].ToString() + ", ");
                            isEditbtnVsbl = true;
                        }
                    }
                }
                if (sbDspText.ToString() != "")
                    result = sbDspText.ToString().Remove(sbDspText.ToString().LastIndexOf(','));// + "."
            }
            else
            {
                int varslength = 0;
                sbDspText.Append("<table class=\"table fw-normal text-start menu-link px-5 m-0\">");
                sbAppParamsDspText.Append("<table class=\"table fw-normal text-start menu-link px-5 m-0\">");
                foreach (var txtStr in displayText)
                {
                    string[] innerGloVar = txtStr.Split(':');
                    if (innerGloVar.Length > 1)
                    {
                        sbDspText.Append("<tr>");
                        if (innerGloVar[0].ToString() != "")
                            sbDspText.Append("<td>" + innerGloVar[0].ToString() + "</td>");
                        if (innerGloVar[1].ToString() != "")
                            sbDspText.Append("<td>:</td><td>" + innerGloVar[1].ToString() + "</td>");
                        sbDspText.Append("</tr>");

                        if (varslength == 0 || (varslength + innerGloVar[1].ToString().Length < 40))
                        {
                            axGloVarsHtml += "<a href=\"javascript:void(0);\" class=\"Application_Parameter-Items\" title=\"" + innerGloVar[0].ToString() + "\" data-toggle=\"tooltip\" data-placement=\"top\">" + innerGloVar[1].ToString() + "</a>";
                            varslength += innerGloVar[1].ToString().Length;
                        }
                        else
                        {
                            sbAppParamsDspText.Append("<tr>");
                            if (innerGloVar[0].ToString() != "")
                                sbAppParamsDspText.Append("<td>" + innerGloVar[0].ToString() + "</td>");
                            if (innerGloVar[1].ToString() != "")
                                sbAppParamsDspText.Append("<td>:</td><td>" + innerGloVar[1].ToString() + "</td>");
                            sbAppParamsDspText.Append("</tr>");
                        }
                    }
                }
                sbDspText.Append("</table>");
                sbAppParamsDspText.Append("</table>");
            }
            //if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
            //    myPopover.Visible = false;
            //else
            AxGloVarHTML.InnerHtml = axGloVarsHtml;
            myPopover.InnerHtml = sbDspText.ToString();
            AxGloVarPopOverHTML.InnerHtml = sbAppParamsDspText.ToString();
            result = string.Empty;
        }
        catch (Exception ex)
        {
            result = "";
        }
        return result;
    }

    private void toggleEditGloButton(bool result)
    {
        if (result)
        {
            editAxGlo2.Visible = true;
            var AxGlo = UserOptions.Where(x => x.Key == "GlobalParams").ToList();
            if (AxGlo.Count > 0)
                UserOptions["GlobalParams"] = "\"display\":\"block\"♠\"title\":\"Global Parameters\"";
            else
                UserOptions.Add("GlobalParams", "\"display\":\"block\"♠\"title\":\"Global Parameters\"");
        }
        else
        {
            editAxGlo2.Visible = false;
            var AxGlo = UserOptions.Where(x => x.Key == "GlobalParams").ToList();
            if (AxGlo.Count > 0)
                UserOptions["GlobalParams"] = "\"display\":\"none\"♠\"title\":\"Global Parameters\"";
            else
                UserOptions.Add("GlobalParams", "\"display\":\"none\"♠\"title\":\"Global Parameters\"");
            Session["AxGloHideShow"] = "F";
        }
    }

    private void DisplayGloVars(string globalVarToShow)
    {
        // Below is used to display the saved Global Variables.
        string url = string.Empty;
        string AxGloHideShow = string.Empty;
        if (Session["AxGloHideShow"] != null && Session["AxGloHideShow"].ToString() != "")
            AxGloHideShow = Session["AxGloHideShow"].ToString();
        string AxGloRecId = string.Empty;
        if (Session["AxGloRecId"] != null && Session["AxGloHideShow"].ToString() != "")
            AxGloRecId = Session["AxGloRecId"].ToString();
        if (AxGloRecId != string.Empty)
            url = "../aspx/ParamsTstruct.aspx?transid=" + paramTransid + "&recordid=" + AxGloRecId + "";
        else
            url = "../aspx/ParamsTstruct.aspx?transid=" + paramTransid + "";
        editAxGlo2.Attributes.Add("onclick", "BootstrapDialogAppParamsShow('" + url + "');");
        // editAxGlo2.Attributes.Add("onclick", "displayBootstrapModalDialog('Application Params', 'lg', 'calc(100vh - 50px)', true,'" + url + "',false,removePageHeaders);");
        var AxGlo = UserOptions.Where(x => x.Key == "GlobalParamsonclick").ToList();
        if (AxGlo.Count > 0)
            UserOptions["GlobalParamsonclick"] = "\"onclick\":\"BootstrapDialogAppParamsShow('" + url + "');\"";
        else
            UserOptions.Add("GlobalParamsonclick", "\"onclick\":\"BootstrapDialogAppParamsShow('" + url + "');\"");
        if (globalVarToShow.Length > 100)
        {
            string globalValuesShown = string.Empty;
            globalValuesShown = globalVarToShow.ToString();
            globalVarToShow.Remove(100, ((globalValuesShown.Length - 1) - 100));
        }
        hdParamsValues.Value = globalVarToShow.ToString();
    }

    private void SetPrintInterval()
    {
        string prInterval = ConfigurationManager.AppSettings["PrintUpdateInterval"];
        if (prInterval == null)
            prInterval = "-1";
        else
            prInterval = prInterval.ToString();

        printInterval = "<script language=\"javascript\" type=\"text/javascript\" >var prInterval='" + prInterval + "';</script>";
    }

    private void GetMenuAndCardsData()
    {
        if (newHomeCards == "true")
        {
            string _AxOldCards = util.GetAdvConfigs("AxOldCards");
            if (_AxOldCards == "true")
            {
                GetMenuAndCardsData_Old();
                return;
            }
        }

        ObjExecTr.SetCurrentTimeMethod();
        string commonResult = string.Empty;
        string menuResult = string.Empty;
        string cardsResult = string.Empty;

        string err = string.Empty;
        string lang_at = "";
        if (Session["language"] != null && Session["language"].ToString().ToUpper() != "ENGLISH")
            lang_at = " lang=\"" + language + "\"";

        if (homePageType == "cards" && Navigationpage == string.Empty)
        {
            cardsEnabled = true;
        }

        bool menuCached = false;
        bool cardsCached = false;

        string errMsg = string.Empty;
        try
        {
            string fdKeyMenuData = Constants.REDISMENUDATA;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDW fdwObj = new FDW();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj != null)
                menuResult = fObj.StringFromRedis(util.GetRedisServerkey(fdKeyMenuData, "Menu"), schemaName);

            if (menuResult != string.Empty)
            {
                menuCached = true;

                if (menuResult.IndexOf("axhomeconfig=\"true\"") > -1)
                {
                    isHomeConfigPage = "true";
                }
                else
                {
                    if (newHomeCards == "true")
                        isHomeConfigPage = "true";
                    else
                        isHomeConfigPage = "false";
                }
            }

            if (cardsEnabled)
            {
                //if (fObj != null)
                //{
                //    try
                //    {
                //        bool isRedisConnected = fObj.IsConnected;
                //        if (isRedisConnected)
                //        {
                //            cardsResult = asbWebService.GetCardsWithData(true, false);
                //        }
                //    }
                //    catch (Exception ex)
                //    { }
                //}
                cardsResult = "";
            }

            if (!menuCached)
            {
                string sXml = string.Empty;
                string errlog = logobj.CreateLog("Getting Menu", sid, "GetMultiMenu", "new");
                if (loginTrace.ToLower() == "true")
                    errlog = logobj.CreateLog("Getting Menu", sid, "GetMultiMenu", "new", "true");

                sXml = sXml + "<root  menucached='" + menuCached.ToString().ToLower() + "' axpapp='" + proj + "' sessionid='" + sid + "' trace='" + errlog + "' mname =\"\" " + lang_at + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'";
                sXml = sXml + "> ";
                sXml = sXml + Session["axApps"].ToString() + axProps + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString();
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
                            Session["MenuData"] = menuResult;

                        if (menuResult.IndexOf("axhomeconfig=\"true\"") > -1)
                        {
                            isHomeConfigPage = "true";
                        }
                        else
                        {
                            if (newHomeCards == "true")
                                isHomeConfigPage = "true";
                            else
                                isHomeConfigPage = "false";
                        }

                    }
                }

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
                SessExpires();
                if (isCloudApp)
                    Response.Redirect(util.ACERRPATH + "14");
                else
                    Response.Redirect(util.SESSEXPIRYPATH);
            }
            else
            {
                if (isCloudApp)
                    Response.Redirect(util.ACERRPATH + "15");
                else
                    Response.Redirect(util.SESSEXPIRYPATH);
            }
        }
        else
        {
            menuXmlData = menuResult;
            menuXmlData = menuXmlData.Replace(@"\", ";bkslh");
            menuXmlData = Regex.Replace(menuXmlData, "'", "&apos;");
            menuXmlData = Regex.Replace(menuXmlData, "&quot;", " ");

            if (cardsResult != string.Empty)
            {
                cardsDataVal = cardsResult;
            }
        }

        string serverprocesstime = ObjExecTr.TotalServerElapsTimeMethod();
        requestProcess_logtime += " ♦ GetMenuAndCardsData ARM:" + serverprocesstime + " (This time is included in Response processed at server) ♦ ";
    }

    private void GetMenuAndCardsData_Old()
    {
        string commonResult = string.Empty;
        string menuResult = string.Empty;
        string cardsResult = string.Empty;

        string err = string.Empty;
        string lang_at = "";
        if (Session["language"] != null && Session["language"].ToString().ToUpper() != "ENGLISH")
            lang_at = " lang=\"" + language + "\"";

        if (homePageType == "cards" && Navigationpage == string.Empty)
        {
            cardsEnabled = true;
        }

        bool menuCached = false;
        bool cardsCached = false;

        string errMsg = string.Empty;
        try
        {
            string fdKeyMenuData = Constants.REDISMENUDATA;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDW fdwObj = new FDW();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj != null)
                menuResult = fObj.StringFromRedis(util.GetRedisServerkey(fdKeyMenuData, "Menu"), schemaName);

            if (menuResult != string.Empty)
            {
                menuCached = true;

                if (menuResult.IndexOf("axhomeconfig=\"true\"") > -1)
                {
                    isHomeConfigPage = "true";
                }
                else
                {
                    if (newHomeCards == "true")
                        isHomeConfigPage = "true";
                    else
                        isHomeConfigPage = "false";
                }
            }

            if (cardsEnabled)
            {
                if (fObj != null)
                {
                    try
                    {
                        //bool isRedisConnected = fObj.IsConnected;
                        //if (isRedisConnected)
                        //{
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
                        //}
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else
            {
                cardsCached = true;
            }

            if (!menuCached || !cardsCached)
            {
                string sXml = string.Empty;
                string errlog = logobj.CreateLog("Getting Menu", sid, "GetMultiMenu", "new");
                if (loginTrace.ToLower() == "true")
                    errlog = logobj.CreateLog("Getting Menu", sid, "GetMultiMenu", "new", "true");

                string cardAttribute = string.Empty;

                if (!cardsCached)
                {
                    cardAttribute = " homepageflag='" + homePageType + "' ";
                }

                sXml = sXml + "<root " + cardAttribute + " menucached='" + menuCached.ToString().ToLower() + "' axpapp='" + proj + "' sessionid='" + sid + "' trace='" + errlog + "' mname =\"\" " + lang_at + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'";
                sXml = sXml + "> ";
                sXml = sXml + Session["axApps"].ToString() + axProps + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString();
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
                            Session["MenuData"] = menuResult;

                        if (menuResult.IndexOf("axhomeconfig=\"true\"") > -1)
                        {
                            isHomeConfigPage = "true";
                        }
                        else
                        {
                            if (newHomeCards == "true")
                                isHomeConfigPage = "true";
                            else
                                isHomeConfigPage = "false";
                        }

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
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
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
                else if (homePageType == "cards" && cardsResult != "" && cardsResult != (new JArray()).ToString())
                {
                    cardsResult = refreshCards(cardsResult);
                }
            }
            else if (homePageType == "cards" && cardsResult != "" && cardsResult != (new JArray()).ToString())
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
                SessExpires();
                if (isCloudApp)
                    Response.Redirect(util.ACERRPATH + "14");
                else
                    Response.Redirect(util.SESSEXPIRYPATH);
            }
            else
            {
                if (isCloudApp)
                    Response.Redirect(util.ACERRPATH + "15");
                else
                    Response.Redirect(util.SESSEXPIRYPATH);
            }
        }
        else
        {
            menuXmlData = menuResult;
            menuXmlData = menuXmlData.Replace(@"\", ";bkslh");
            menuXmlData = Regex.Replace(menuXmlData, "'", "&apos;");
            menuXmlData = Regex.Replace(menuXmlData, "&quot;", " ");


            if (cardsResult != string.Empty)
            {
                cardsDataVal = cardsResult;
            }

        }
    }

    private void CreateMenuSellecterDropbox(XmlNode item)
    {
        if (item.HasChildNodes)
        {
            strMenuHtml1.Append("<li onclick=\"menuSellecter('" + item.Attributes["oname"].Value + "')\"><a id=\"" + item.Attributes["oname"].Value + "\" class=\"newMenuDropdown\" href=\"javascript:void(0)\">" + item.Attributes["name"].Value + "</a></li>");
        }
    }
    private void CreatePanelsWrapper(XmlNode item)
    {
        string hreflink = string.Empty;
        if (item.HasChildNodes)
        {
            strMenuHtml1.Append("<div class=\"panelsWrapper hideInClose " + item.Attributes["oname"].Value + "\" style=\"display: none;\">");
            for (int i = 0; i < item.ChildNodes.Count; i++)
            {
                if (item.ChildNodes[i].HasChildNodes)
                {
                    strMenuHtml1.Append("<div class=\"transactionsPanel\"><div class=\"panelHeading\">" + item.ChildNodes[i].Attributes["name"].Value + "</div><ul class=\"listUl\">");
                    for (int j = 0; j < item.ChildNodes[i].ChildNodes.Count; j++)
                    {
                        var target = item.ChildNodes[i].ChildNodes[j].Attributes["target"].Value;
                        if (target.Contains("iviewInteractive"))
                            target = target.Replace("iviewInteractive", "iview");
                        if (target.Contains("iview.aspx") || target.Contains("tstruct.aspx"))
                            hreflink = getSmallMenuName(target);
                        if (hreflink.Length > 0)
                        {
                            strMenuHtml1.Append("<li><a href=\"#" + hreflink + "\" onclick='LoadIframe(\"" + target + "\")'>" + item.ChildNodes[i].ChildNodes[j].Attributes["name"].Value + "</a></li>");
                        }
                        else
                        {
                            strMenuHtml1.Append("<li><a href=\"javascript:void(0)\" onclick='LoadIframe(\"" + target + "\")'>" + item.ChildNodes[i].ChildNodes[j].Attributes["name"].Value + "</a></li>");
                        }

                    }
                    strMenuHtml1.Append("</ul></div>");
                }
            }
            strMenuHtml1.Append("</div>");
        }
    }

    private void SessExpires()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    private void GetGlobalVariables()
    {
        if (Session["traceStatus"] != null && Session["traceStatus"].ToString() != string.Empty)
            traceStatus = (string)Session["traceStatus"];
        if (Session["workflowEnabled"] != null)
            workflow = Convert.ToBoolean(Session["workflowEnabled"].ToString());
        if (Session["userAccessEnabled"] != null)
            userAccess = Convert.ToBoolean(Session["userAccessEnabled"].ToString());
        if (Session["AxAppTitle"] != null)
            appName = Session["AxAppTitle"].ToString();
        appTitle = appName;
        var axApp = UserOptions.Where(x => x.Key == "axAppName").ToList();
        if (axApp.Count > 0)
            UserOptions["axAppName"] = "\"appName\":\"" + appName + "\"♠\"onclick\":\"LoadIframe('loadhomepage');\"";
        else
            UserOptions.Add("axAppName", "\"appName\":\"" + appName + "\"♠\"onclick\":\"LoadIframe('loadhomepage');\"");

        Session.Add("SelectedLang", "");
        AxRole = Session["AxRole"].ToString();
        AxRole = util.CheckSpecialChars(AxRole);
        ViewState["AxRole"] = AxRole;
        proj = Session["project"].ToString();
        proj = util.CheckSpecialChars(proj);
        ViewState["proj"] = proj;
        sid = Session["nsessionid"].ToString();
        sid = util.CheckSpecialChars(sid);
        ViewState["sid"] = sid;
        user = Session["user"].ToString();
        user = util.CheckSpecialChars(user);
        ViewState["user"] = user;
        language = Session["language"].ToString();
        ViewState["language"] = language;

        strGlobalVar = util.GetGlobalVarString();
    }
    private void SetGlobalVariables()
    {
        AxRole = ViewState["AxRole"].ToString();
        proj = ViewState["proj"].ToString();
        sid = ViewState["sid"].ToString();
        user = ViewState["user"].ToString();
        language = ViewState["language"].ToString();
    }
    //private string CheckSpecialChars(string str) // This method there in util.cs as a common method so it is used from util.cs
    //{
    //    str = Regex.Replace(str, "&", "&amp;");
    //    str = Regex.Replace(str, "<", "&lt;");
    //    str = Regex.Replace(str, ">", "&gt;");
    //    str = Regex.Replace(str, "'", "&apos;");
    //    str = Regex.Replace(str, "\"", "&quot;");
    //    string delimited = @"\\";
    //    str = Regex.Replace(str, delimited, ";bkslh");
    //    return str;
    //}

    private void SetTheme()
    {
        if (Session["Thmcolor"] == null)
            Session["Thmcolor"] = "Blue";
        themeColor = Session["themeColor"].ToString();
        //Themes are written as script for direct access in the javascript
        regthmdata = "<script language=\"javascript\" type=\"text/javascript\" >";
        regthmdata += "var thmProj = '" + proj + "';var thmUser = '" + user + "';var thmRole = '" + AxRole + "';var thmSid = '" + sid + "';var thmSelected = '" + Session["Thmcolor"].ToString() + "';var g_lang='" + Session["language"].ToString() + "';";
        regthmdata += "</script>";
    }

    protected void ValidatePage()
    {
        ObjExecTr.SetCurrentTimeMethod();
        string result = string.Empty;
        bool IsProjSelected = false;
        string errlog = string.Empty;
        result = loginHelper.result;
        Session["project"] = loginHelper.proj;

        //Session["user"] = loginHelper.user;
        //Session["username"] = loginHelper.user;
        //thmUser = loginHelper.user;

        proj = loginHelper.proj;
        thmSid = sid;
        Session["pwd"] = loginHelper.password;
        Session["nsessionid"] = sid;
        Session["validated"] = "True";
        Session["language"] = loginHelper.selectedLanguage;
        Session["axp_language"] = loginHelper.selectedLanguage.ToLower();
        language = loginHelper.selectedLanguage;

        try
        {
            if (Session["FDR"] == null)
            {
                FDR fObj = new FDR();
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

            string newUser = string.Empty;
            if (Session["transidlist"] != null)
            {
                string licType = string.Empty, transidlist = string.Empty;
                if (Session["transidlist"].ToString().Contains('~'))
                    transidlist = Session["transidlist"].ToString().Split('~')[3];
                if (transidlist == "9867")
                {
                    licType = "unlimited";
                    Session["lictype"] = "unlimited";
                    try
                    {
                        if (Application["lstunlimited"] != null)
                        {
                            string lstunlimited = Application["lstunlimited"].ToString();
                            var lstunlim = lstunlimited.Split(',').Where(x => x == proj).ToList();
                            if (lstunlim.Count == 0)
                            {
                                lstunlimited += "," + proj;
                                Application["lstunlimited"] = lstunlimited;
                            }
                        }
                        else
                            Application["lstunlimited"] = proj;
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    licType = "limited";
                    Session["lictype"] = "limited";
                    newUser = user + "♦" + sid + "♣" + licType;
                    util.SetDupUserInfo(proj, user, newUser);
                }
            }

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
            if (Session["isSSOLogin"] == null || Session["isSSOLogin"].ToString().ToLower() != "true")
            {
                HttpContext.Current.Session["AxCPWDOnLogin"] = util.GetConfigAttrValue(Session["project"].ToString(), "AxCPWDOnLogin");
                if ((forcePwdChange == "1" && Session["AxCPWDOnLogin"] != null && Session["AxCPWDOnLogin"].ToString() == "true") || forcePwdChange == "2" || forcePwdChange == "3")
                {
                    if (Session["dbotpauth"] != null && Session["dbotpauth"].ToString() != "T")
                    {
                        Session["validated"] = "";
                        try
                        {
                            Response.Redirect("../aspx/cpwd.aspx?remark=" + forcePwdChange, true);
                        }
                        catch (ThreadAbortException ex)
                        {
                            Thread.ResetAbort();
                        }
                    }
                }
            }
        }
        else
        {
            Session["user"] = loginHelper.user;
            Session["username"] = loginHelper.user;
            thmUser = loginHelper.user;
            ParseStringResult(result);
        }

        // user selected application from app dropdown
        if (IsProjSelected)
        {
            // onApp event datatables will be created in redis for the selected(dropdown value) application                 
            fdwObj.Initialize(proj);
            ////This function will create datasets for Login event
            //CreateFDOnLogin(fdwObj);
        }
        else
        {
            ////This function will create datasets for Login event     
            //CreateFDOnLogin(fdwObj);
        }

        Session["CSRFToken"] = "";
        string cookieName = "CSRFToken";
        Response.Cookies.Add(new System.Web.HttpCookie(cookieName, ""));
        if (Request.Cookies[cookieName] != null)
        {
            Response.Cookies[cookieName].Value = string.Empty;
            Response.Cookies[cookieName].Expires = DateTime.Now.AddMonths(-20);
        }
        string serverprocesstime = ObjExecTr.TotalServerElapsTimeMethod();
        requestProcess_logtime += " ♦ Login result process time:" + serverprocesstime + " (This time is included in Response processed at server) ♦ ";
    }


    public void CreateFDOnLogin(FDW fdwObj)
    {
        string schemaName = string.Empty;
        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();
        if (Session["username"] != null && Session["username"].ToString().ToLower() == "admin")
            fdwObj.GetAxRelations(schemaName);//If anything updated in AxRelations table either directly or indirectly needs to login admin once in web.
    }

    private void ParseStringResult(string result)
    {
        string[] themeres = result.Split(',');
        if (themeres.Length > 1)
        {
            themeColor = themeres[0].ToString();
            Session["themeColor"] = themeres[0].ToString();
            //Themes are written as script for direct access in the javascript
            regthmdata = "<script language=\"javascript\" type=\"text/javascript\" >";
            regthmdata += "var thmProj = '" + proj + "';var thmUser = '" + user + "';var thmRole = '" + AxRole + "';var thmSid = '" + sid + "';var thmSelected = '" + themeres[0].ToString() + "';var g_lang='" + Session["language"].ToString() + "';";
            regthmdata += "</script>";
            result = themeres[1].ToString();
        }
        else
        {
            Session["themeColor"] = "Gray";
            Session["themeColor"] = "Gray";
            regthmdata = "<script language=\"javascript\" type=\"text/javascript\" >";
            regthmdata += "var thmProj = '" + proj + "';var thmUser = '" + user + "';var thmRole = '" + AxRole + "';var thmSid = '" + sid + "';var thmSelected = '" + "Blue" + "';var g_lang='" + Session["language"].ToString() + "';";
            regthmdata += "</script>";
            result = themeres[0].ToString();
        }

        string[] splitout = result.Split('~');
        Session["AxRole"] = splitout[1].ToString().Substring(0, splitout[1].ToString().Length - 1);
        string[] reg_gv = result.Split('#');
        if (reg_gv.Length > 0)
        {
            global_vars = reg_gv[1].ToString();
            string[] glo_Split = global_vars.Split('^');
            string globalApp_vars = glo_Split[0].ToString();
            globalUsr_vars = null;
            if (glo_Split.Length > 1)
            {
                globalUsr_vars = glo_Split[1].ToString();
            }
            else
            {
                globalUsr_vars = "";
            }
            Session["global_vars"] = global_vars;
            Session["globalUsr_vars"] = globalUsr_vars;
            CreateGlobalVars(globalApp_vars);
        }


        //Build, firstTimeLogin,Manage,Workflow

        Session["Build"] = splitout[0].ToString().Substring(0, 1);
        if (Convert.ToString(Session["Build"]) == "T")
            build = true;

        if (splitout[0].Substring(2, 1) == "T")
        {
            manage = true;
        }

        if (splitout[0].Substring(2, 1) == "T")
        {
            userAccess = true;
            Session["userAccessEnabled"] = true;
        }

        if (splitout[0].Substring(3, 1) == "T")
        {
            workflow = true;
            Session["workflowEnabled"] = workflow;
        }

        if (splitout[0].Length > 4)
        {
            if (splitout[0].Substring(4, 1) == "T")
            {
                import = true;
                Session["ImportAccess"] = import;
            }
            if (splitout[0].Substring(5, 1) == "T")
            {
                export = true;
                Session["ExportAccess"] = import;
            }

            if (import)
            {
                history = true;
                Session["HistoryAccess"] = history;
            }
        }


        if (manage == true || Session["AxRole"].ToString() == "default_")
        {
            traceStatus = "T";
            Session["traceStatus"] = traceStatus;
            util.sysErrorlog = true;
            logobj.errorlog = "true";
        }
        else
        {
            Session["AxTrace"] = "false";
        }

        if (splitout[0].Substring(1, 1) == "T" && axChangePwdReDir == true)
        {
            Session["validated"] = "";
            try
            {
                Response.Redirect("../aspx/cpwd.aspx?remark=firsttime", true);
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

    }

    private void CreateGlobalVars(string globalApp_vars)
    {
        // Session["globalvarstring"] = string.Empty;
        StringBuilder paramlist = new StringBuilder();
        int paramno = 0;

        gloApp_Split = globalApp_vars.Split('~');
        Session["paramstring"] = global_vars;
        if (gloApp_Split.Length > 0)
        {
            int glo_inx = 0;
            for (glo_inx = 1; glo_inx <= gloApp_Split.Length - 1; glo_inx++)
            {
                int indx = gloApp_Split[glo_inx].ToString().IndexOf("=");
                string leftreg_glovar = gloApp_Split[glo_inx].ToString().Substring(0, indx);
                string rightreg_glovar = gloApp_Split[glo_inx].Substring(indx + 1);
                if (!Constants.globalVarPathVaribles.Contains(leftreg_glovar.ToLower()))
                {
                    paramlist.Append("Parameters[" + paramno + "] = " + "\"" + leftreg_glovar + "~" + util.CheckSpecialChars(rightreg_glovar) + "\";  ");
                    paramno = paramno + 1;
                }
                string nSessname = leftreg_glovar.ToString();
                string nSessvalue = rightreg_glovar.ToString();
                Session[nSessname] = nSessvalue;
            }
        }


        string[] gloUsr_Split = globalUsr_vars.Split('~');
        if (gloUsr_Split.Length > 0)
        {
            int glo_inx1 = 0;
            for (glo_inx1 = 1; glo_inx1 <= gloUsr_Split.Length - 1; glo_inx1++)
            {
                int indx1 = gloUsr_Split[glo_inx1].ToString().IndexOf("=");
                string leftreg_glovar1 = gloUsr_Split[glo_inx1].ToString().Substring(0, indx1);
                string rightreg_glovar1 = gloUsr_Split[glo_inx1].ToString().Substring(indx1 + 1);
                if (!Constants.globalVarPathVaribles.Contains(leftreg_glovar1.ToLower()))
                {
                    paramlist.Append("Parameters[" + paramno + "] = " + "\"" + leftreg_glovar1 + "~" + rightreg_glovar1 + "\";  ");
                    paramno = paramno + 1;
                }
                string nSessname1 = leftreg_glovar1.ToString();
                string nSessvalue1 = rightreg_glovar1.ToString();
                Session[nSessname1] = nSessvalue1;
            }
        }

        string dtCulture = Request.UserLanguages[0];

        //If useCulture flag is false , we are not checking the browser culture.
        //If it's true ,we are setting the culture based on the client browser language.
        if (Session["AxUSCulture"] != null)
            useCulture = Convert.ToBoolean(Session["AxUSCulture"].ToString());
        if (!useCulture)
            dtCulture = "en-gb";
        else
        {
            if (dtCulture.ToLower() == "en" || dtCulture.ToLower() == "en-us")
                dtCulture = "en-us";
        }
        paramlist.Append("Parameters[" + paramno + "] = " + "\"username~" + user + "\";  ");
        paramlist.Append("Parameters[" + (paramno + 1) + "] = " + "\"Culture~" + dtCulture + "\";  ");

        Session["ClientLocale"] = dtCulture;
        glCulture = dtCulture;
        if (paramlist.ToString() != string.Empty)
            util.SetGlobalVarString(paramlist.ToString());
    }

    private ArrayList GetGlobalVarsArray(string type)
    {
        ArrayList Vars = new ArrayList();
        if (type == "globalvar")
        {
            if (Session["axGlobalVars"] != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Session["axGlobalVars"].ToString());
                XmlNode globNode = xmlDoc.SelectSingleNode("/globalvars");
                foreach (XmlNode xmlNode in globNode.ChildNodes)
                {
                    Vars.Add(xmlNode.Name + "=" + xmlNode.InnerText);
                }
            }
        }
        else if (type == "uservar")
        {
            if (Session["axUserVars"] != null)
            {
                //Session["axUserVars"] = "<uservars>" + childNode.InnerXml + "</uservars>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Session["axUserVars"].ToString());
                XmlNode globNode = xmlDoc.SelectSingleNode("/uservars");
                foreach (XmlNode xmlNode in globNode.ChildNodes)
                {
                    Vars.Add(xmlNode.Name + "=" + xmlNode.InnerText);
                }
            }
        }
        return Vars;
    }

    private void CheckResultFormat(string result)
    {
        if (result.TrimStart().StartsWith("<") && result.TrimEnd().EndsWith(">"))
            isResultXml = true;
        else
            isResultXml = false;
    }

    private string GetBrowserDetails()
    {
        System.Web.HttpBrowserCapabilities browser = Request.Browser;
        string userDetails = timeZone + "¿" + browser.Type + "¿" + browser.Browser + "¿"
            + browser.Version + "¿" + browser.MajorVersion + "¿"
            + browser.MinorVersion + "¿" + browser.Platform + "¿"
            + Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];

        if (userDetails.Length > 200)
            userDetails = userDetails.Substring(0, 200);

        return userDetails;
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

        if (resultNode.Attributes["username"] != null)
        {
            loginHelper.user = resultNode.Attributes["username"].Value;
            Session["user"] = loginHelper.user;
            Session["username"] = loginHelper.user;
            user = loginHelper.user;
            thmUser = loginHelper.user;
        }



        if (resultNode.Attributes["theme"] != null)
            ApplyTheme(resultNode.Attributes["theme"].Value);
        if (resultNode.Attributes["nickname"] != null)
            Session["AxNickName"] = resultNode.Attributes["nickname"].Value;
        if (resultNode.Attributes["email_id"] != null)
        {
            UserOptions.Add("AxUserEmail", "\"display\":\"block\"♠\"email\":\"" + resultNode.Attributes["email_id"].Value + "\"");
            if (resultNode.Attributes["email_id"].Value != string.Empty)
                Session["userEmail_id"] = resultNode.Attributes["email_id"].Value;
        }
        else
            UserOptions.Add("AxUserEmail", "\"display\":\"block\"♠\"email\":\"\"");
        if (resultNode.Attributes["mobileno"] != null && resultNode.Attributes["mobileno"].Value != string.Empty)
            Session["userMobileno"] = resultNode.Attributes["mobileno"].Value;

        if (resultNode.Attributes["authkey"] != null)
        {
            newHomeCards = "true";
            Session["newHomeCards"] = "true";
        }

        if (resultNode.Attributes["userlandingpage"] != null)
            Session["userLandingPage"] = resultNode.Attributes["userlandingpage"].Value;
        if (resultNode.Attributes["roleslandingpage"] != null)
            Session["rolesLandingPage"] = resultNode.Attributes["roleslandingpage"].Value;

        //if (resultNode.Attributes["db_otpauth"] != null)
        //    Session["dbotpauth"] = resultNode.Attributes["db_otpauth"].Value;

        //displaying an icon(next to setting menu) - to display license related warning messages onclick 
        if (resultNode.Attributes["licmsg"] != null)
        {
            string licInfo = resultNode.Attributes["licmsg"].Value;
            liLicInfo.Visible = true;
            aLicinfo.Attributes.Add("onclick", "showAlertDialog('info','" + licInfo + "')");
            UserOptions.Add("LicInfo", "\"display\":\"block\"♠\"onclick\":\"showAlertDialog('info', '" + licInfo + "')\"♠\"title\":\"License Infomation\"");
        }
        else
            UserOptions.Add("LicInfo", "\"display\":\"none\"♠\"onclick\":\"showAlertDialog('info', '')\"♠\"title\":\"License Infomation\"");

        //Below code checks if there is a global variable for 'nickname', if defined then the display name will be nickname
        if (Session["AxNickName"] == null || Session["AxNickName"].ToString() == string.Empty)
            Session["AxNickName"] = Session["user"];

        if (Session["AxEmailId"] == null || Session["AxEmailId"].ToString() == string.Empty)
            Session["AxEmailId"] = resultNode.Attributes["email_id"].Value;

        if (Session["AxNickName"] != null)
            UserOptions.Add("AxNickName", "\"NickName\":\"" + Session["AxNickName"].ToString() + "\"");

        string lstLoginDetails = GetLastLoginDetails();
        if (lstLoginDetails != string.Empty)
        {
            UserOptions.Add("AxLastLoggIn", "\"display\":\"block\"♠\"DateTime\":\"" + lstLoginDetails + "\"");
        }
        else
        {
            UserOptions.Add("AxLastLoggIn", "\"display\":\"none\"♠\"DateTime\":\"" + lstLoginDetails + "\"");
        }

        string profileSrc = GetProfilePic();
        if (profileSrc != string.Empty)
        {
            UserOptions.Add("AxUserProfile", "\"display\":\"block\"♠\"ppSrc\":\"" + profileSrc + "\"");
        }
        else
        {
            UserOptions.Add("AxUserProfile", "\"display\":\"none\"♠\"ppSrc\":\"" + profileSrc + "\"");
        }


        if (resultNode.Attributes["axglo"] != null)
        {
            string axGlovalue = resultNode.Attributes["axglo"].Value;
            if (axGlovalue.IndexOf("~") != -1)
            {
                string[] AxGloFld = axGlovalue.Split('~');
                Session["AxGloFldCount"] = AxGloFld[0];
                paramTransid = AxGloFld[1];
            }
            else
            {
                Session["AxGloFldCount"] = axGlovalue;
                paramTransid = "axglo";
            }
        }

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
                string virtualdownloadpath = util.VirtualFilePathUpdate();
                string glbVarsFromIni = util.GetGblVarsFromIni();
                Session["axGlobalVars"] = "<globalvars>" + childNode.InnerXml + axGridAttachPath + axAttachmentpath + virtualdownloadpath + glbVarsFromIni + "</globalvars>";
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
                        isLockOnRead = true;
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
                        sbDspText += FormatGlobalVariables(xmlNode.InnerText);
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
                    Session["axGlobalVars"] = "<globalvars>" + childNode.InnerXml + axGridAttachPath + axAttachmentpath + virtualdownloadpath + glbVarsFromIni + "</globalvars>";
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
                    cardsDesignVal = childNode.InnerText;
                    Session["cardsDesignVal"] = cardsDesignVal;
                }
            }

        }

        if (globalVariables.Count > 0 || userVariables.Count > 0)
        {
            CreateParamaterArray();
        }
        // Below is used to display the saved Global Variables.
        string url = string.Empty;
        string AxGloHideShow = string.Empty;
        if (Session["AxGloHideShow"] != null && Session["AxGloHideShow"].ToString() != "")
            AxGloHideShow = Session["AxGloHideShow"].ToString();
        string AxGloRecId = string.Empty;
        if (Session["AxGloRecId"] != null && Session["AxGloHideShow"] != "")
            AxGloRecId = Session["AxGloRecId"].ToString();
        if (AxGloRecId != string.Empty)
            url = "../aspx/ParamsTstruct.aspx?transid=" + paramTransid + "&recordid=" + AxGloRecId + "";
        else
            url = "../aspx/ParamsTstruct.aspx?transid=" + paramTransid + "";
        editAxGlo2.Attributes.Add("onclick", "BootstrapDialogAppParamsShow('" + url + "');");

        var AxGlo = UserOptions.Where(x => x.Key == "GlobalParamsonclick").ToList();
        if (AxGlo.Count > 0)
            UserOptions["GlobalParamsonclick"] = "\"onclick\":\"BootstrapDialogAppParamsShow'" + url + "');\"";
        else
            UserOptions.Add("GlobalParamsonclick", "\"onclick\":\"BootstrapDialogAppParamsShow('" + url + "');\"");
        if (globalVarToShow.Length > 100)
        {
            string globalValuesShown = string.Empty;
            globalValuesShown = globalVarToShow.ToString();
            globalVarToShow.Remove(100, ((globalValuesShown.Length - 1) - 100));
            globalVarToShow.Append("<a  onclick='javascript:ShowMore(\"" + globalValuesShown + "\");'> More</a>");
        }
        hdParamsValues.Value = globalVarToShow.ToString();
    }

    protected string GetLastLoginDetails()
    {
        string strRes = string.Empty;
        try
        {
            string KeyData = Constants.REDISLASTLOGGEDIN;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
                fObj = new FDR();
            strRes = fObj.StringFromRedis(util.GetRedisServerkey(KeyData, Session["username"].ToString()), schemaName);
            SaveLastLogin();
        }
        catch (Exception ex)
        {
            strRes = string.Empty;
        }
        return strRes;
    }
    protected void SaveLastLogin()
    {
        try
        {
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();

            string strDt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            FDW fdwObj = new FDW();
            string KeyData = Constants.REDISLASTLOGGEDIN;
            fdwObj.SaveInRedisServer(util.GetRedisServerkey(KeyData, Session["username"].ToString()), strDt, Constants.REDISLASTLOGGEDIN, schemaName);
        }
        catch (Exception ex)
        { }
    }

    protected string GetProfilePic()
    {
        string strRes = string.Empty;
        try
        {
            string KeyData = Constants.REDISPROFILEPIC;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
                fObj = new FDR();
            strRes = fObj.StringFromRedis(util.GetRedisServerkey(KeyData, Session["username"].ToString()), schemaName);
            if (strRes == string.Empty)
            {
                DBContext obj = new DBContext();
                strRes = obj.GetUserProfilePic();
            }

            if (strRes != string.Empty)
            {
                if (strRes == "no profile pic")
                    strRes = string.Empty;
                else
                    strRes = String.Format("data:image/png;base64,{0}", strRes);
            }
        }
        catch (Exception ex)
        {
            strRes = string.Empty;
        }
        return strRes;
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
        Session["AppMgrAccess"] = "F";

        foreach (XmlNode xmlNode in childNode.ChildNodes)
        {
            if (xmlNode.Name == "build" && xmlNode.InnerText == "T")
            {
                Session["Build"] = xmlNode.InnerText;
                build = true;
                manage = true;
            }
            else if (xmlNode.Name == "forcepwdchange")
            {
                forcePwdChange = xmlNode.InnerText;
                Session["forcePwdChange"] = xmlNode.InnerText;
            }
            else if (xmlNode.Name == "users" && xmlNode.InnerText == "T")
            {
                userAccess = true;
                Session["userAccessEnabled"] = true;
            }
            else if (xmlNode.Name == "workflow" && xmlNode.InnerText == "T")
            {
                workflow = true;
                Session["workflowEnabled"] = workflow;
            }
            else if (xmlNode.Name == "import" && xmlNode.InnerText == "T")
            {
                import = true;
                Session["ImportAccess"] = import;
            }
            else if (xmlNode.Name == "export" && xmlNode.InnerText == "T")
            {
                export = true;
                Session["ExportAccess"] = import;
            }
            else if (xmlNode.Name == "sessionexists")
            {
                // not in use - To display a message to the user that the previous session is expired.
            }
            else if (xmlNode.Name == "pwdexpiresin")
            {
                Session["pwdExpiresIn"] = xmlNode.InnerText;
                pwdExpDays = Convert.ToInt32(Session["pwdExpiresIn"]);
            }
            else if (xmlNode.Name == "isuniquehybrid" && xmlNode.InnerText == "T")
            {
                Session["isUniqueHybrid"] = xmlNode.InnerText;
            }
            else if (xmlNode.Name == "appmgr" && xmlNode.InnerText == "T")
            {
                Session["AppMgrAccess"] = xmlNode.InnerText;
            }
        }
        if (import || export)
        {
            history = true;
            Session["HistoryAccess"] = history;
        }
    }

    /// <summary>
    /// Function to apply the theme from the login result for the logged in user, Default theme will be blue.
    /// </summary>
    /// <param name="theme"></param>
    private void ApplyTheme(string theme)
    {
        if (theme != string.Empty)
        {
            themeColor = theme;
            Session["themeColor"] = theme;
            regthmdata = "<script language=\"javascript\" type=\"text/javascript\" >";
            regthmdata += "var thmProj = '" + proj + "';var thmUser = '" + user + "';var thmRole = '" + AxRole + "';var thmSid = '" + sid + "';var thmSelected = '" + theme + "';var g_lang='" + Session["language"].ToString() + "';";
            regthmdata += "</script>";

        }
        else
        {
            themeColor = "Default";
            Session["themeColor"] = "Gray";
            regthmdata = "<script language=\"javascript\" type=\"text/javascript\" >";
            regthmdata += "var thmProj = '" + proj + "';var thmUser = '" + user + "';var thmRole = '" + AxRole + "';var thmSid = '" + sid + "';var thmSelected = '" + "Blue" + "';var g_lang='" + Session["language"].ToString() + "';";
            regthmdata += "</script>";
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
        if (Session["AxUSCulture"] != null)
            useCulture = Convert.ToBoolean(Session["AxUSCulture"].ToString());
        if (!useCulture)
            dtCulture = "en-gb";
        else
        {
            if (dtCulture.ToLower() == "en" || dtCulture.ToLower() == "en-us")
                dtCulture = "en-us";
        }
        //paramCnt = globalVariables.Count + userVariables.Count;
        paramCnt = i + userVariables.Count;
        paramlist.Append("Parameters[" + paramCnt + "] = " + "\"username~" + user + "\";  ");
        paramlist.Append("Parameters[" + (paramCnt + 1) + "] = " + "\"Culture~" + dtCulture + "\";  ");

        Session["ClientLocale"] = dtCulture;
        glCulture = dtCulture;
        if (paramlist.ToString() != string.Empty)
            util.SetGlobalVarString(paramlist.ToString());
    }

    /// <summary>
    /// Function to return the param value from the global variables array.
    /// </summary>
    /// <param name="paramName"></param>
    /// <returns></returns>

    public void createTopLinks()
    {

        if (userAccess || workflow || export || import)
        {
            Boolean useWorkflowScript = false;
            if (ConfigurationManager.AppSettings["WorkflowScript"] != null)
                useWorkflowScript = Convert.ToBoolean(ConfigurationManager.AppSettings["WorkflowScript"].ToString());
        }
        if (build)
        {
            li_Trace1.Visible = true;
            UserOptions.Add("showLog", "\"display\":\"block\"♠\"onclick\":\"showTraceFileDialog()\"♠\"title\":\"Show Logs\"");
            //if (ConfigurationManager.AppSettings["ExecutionTrace"] != null && ConfigurationManager.AppSettings["ExecutionTrace"].ToString() == "true")
            //    UserOptions.Add("executionLog", "\"display\":\"block\"♠\"onclick\":\"showElTraceFileDialog()\"♠\"title\":\"View Execution Trace \"");
            //else
            //    UserOptions.Add("executionLog", "\"display\":\"none\"♠\"onclick\":\"showElTraceFileDialog()\"♠\"title\":\"View Execution Trace \"");
            if (ConfigurationManager.AppSettings["ExecutionTrace"] != null && ConfigurationManager.AppSettings["ExecutionTrace"].ToString() == "true")
                UserOptions.Add("exectraceValue", "\"exectraceVal\":\"true\"♠\"onclick\":\"ToggleExecShowLog(true)\"");
            else
                UserOptions.Add("exectraceValue", "\"exectraceVal\":\"false\"♠\"onclick\":\"ToggleExecShowLog(true)\"");
            UserOptions.Add("executionLog", "\"display\":\"block\"♠\"onclick\":\"showElTraceFileDialog()\"♠\"title\":\"View Execution Trace \"");
            UserOptions.Add("auditReport", "\"display\":\"block\"♠\"onclick\":\"LoadIframe('iview.aspx?ivname=auditlog')\"♠\"title\":\"Audit Report\"");
        }
        else
        {
            UserOptions.Add("showLog", "\"display\":\"none\"♠\"onclick\":\"showTraceFileDialog()\"♠\"title\":\"Show Logs\"");
            UserOptions.Add("exectraceValue", "\"exectraceVal\":\"false\"♠\"onclick\":\"ToggleExecShowLog(true)\"");
            UserOptions.Add("executionLog", "\"display\":\"none\"♠\"onclick\":\"showElTraceFileDialog()\"♠\"title\":\"View Execution Trace \"");
            UserOptions.Add("auditReport", "\"display\":\"none\"♠\"onclick\":\"LoadIframe('iview.aspx?ivname=auditlog')\"♠\"title\":\"Audit Report\"");
        }

        string userManualData = util.GetAdvConfigs("User Manual");
        if (userManualData.StartsWith("true"))
        {
            string userManualPath = userManualData.Split('♦')[1];
            if (userManualPath != string.Empty)
                Session["UserManualPath"] = userManualPath;
            UserOptions.Add("userManual", "\"display\":\"block\"♠\"onclick\":\"showUserManual()\"♠\"title\":\"User Manual\"");
        }
        else
            UserOptions.Add("userManual", "\"display\":\"none\"♠\"onclick\":\"showUserManual()\"♠\"title\":\"User Manual\"");

        string isDevInstance = string.Empty;
        if (HttpContext.Current.Session["AxDevInstance"] != null)
            isDevInstance = HttpContext.Current.Session["AxDevInstance"].ToString();
        //string homepagews = string.Empty;
        if (ConfigurationManager.AppSettings["homepagews"] != null)
            homepagews = ConfigurationManager.AppSettings["homepagews"].ToString();

        if (AxRole.Contains("default"))
        {
            if (homepagews == "true")
            {
                if (isDevInstance == "true")
                {
                    li_WidgetBuilder1.Visible = true;
                    UserOptions.Add("WidgetBuilder", "\"display\":\"block\"♠\"onclick\":\"nodeApi ? DoUtilitiesEvent('WidgetBuilder') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Widget Builder\"");
                }
                else
                    UserOptions.Add("WidgetBuilder", "\"display\":\"none\"♠\"onclick\":\"nodeApi ? DoUtilitiesEvent('WidgetBuilder') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Widget Builder\"");
            }
            else
            {
                li_WidgetBuilder1.Visible = true;
                UserOptions.Add("WidgetBuilder", "\"display\":\"block\"♠\"onclick\":\"nodeApi ? DoUtilitiesEvent('WidgetBuilder') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Widget Builder\"");
            }
        }
        else
            UserOptions.Add("WidgetBuilder", "\"display\":\"none\"♠\"onclick\":\"nodeApi ? DoUtilitiesEvent('WidgetBuilder') : showAlertDialog('error', 1000, 'client')\"♠\"title\":\"Widget Builder\"");

        if (AxRole.Contains("default") || build)
        {
            li_Responsibilities1.Visible = true;
            UserOptions.Add("Responsibilities", "\"display\":\"block\"♠\"onclick\":\"DoUtilitiesEvent('Responsibilities')\"♠\"title\":\"Responsiblity\"");
        }
        else
            UserOptions.Add("Responsibilities", "\"display\":\"none\"♠\"onclick\":\"DoUtilitiesEvent('Responsibilities')\"♠\"title\":\"Responsiblity\"");

        if (export)
        {
            li_ExportData1.Visible = true;
            UserOptions.Add("ExportData", "\"display\":\"block\"♠\"onclick\":\"DoUtilitiesEvent('ExportData')\"♠\"title\":\"Export Data\"");
        }
        else
        {
            li_ExportData1.Visible = false;
            UserOptions.Add("ExportData", "\"display\":\"none\"♠\"onclick\":\"DoUtilitiesEvent('ExportData')\"♠\"title\":\"Export Data\"");
        }

        if (import)
        {
            li_ImportData1.Visible = li_ImportHistory1.Visible = true;
            UserOptions.Add("ImportData", "\"display\":\"block\"♠\"onclick\":\"DoUtilitiesEvent('ImportData')\"♠\"title\":\"Import Data\"");
            UserOptions.Add("ImportHistory", "\"display\":\"block\"♠\"onclick\":\"DoUtilitiesEvent('ImportHistory')\"♠\"title\":\"Import History\"");
        }
        else
        {
            li_ImportData1.Visible = li_ImportHistory1.Visible = false;
            UserOptions.Add("ImportData", "\"display\":\"none\"♠\"onclick\":\"DoUtilitiesEvent('ImportData')\"♠\"title\":\"Import Data\"");
            UserOptions.Add("ImportHistory", "\"display\":\"none\"♠\"onclick\":\"DoUtilitiesEvent('ImportHistory')\"♠\"title\":\"Import History\"");
        }
    }

    //Functions for Global Variable
    protected void btnSetParams_Click(object sender, EventArgs e)
    {
        string showHideAxGlo = string.Empty;
        string paramValue = hdParamsValues.Value.ToString();
        string decodedParam = HttpContext.Current.Server.UrlDecode(paramValue);

        string encodedDispTxt = hdnDisplayTxt.Value.ToString();
        string formatedDispTxt = HttpContext.Current.Server.UrlDecode(encodedDispTxt);
        hdnDisplayTxt.Value = util.CheckSpecialChars(formatedDispTxt);
        string tstGloVal = decodedParam;
        string[] arrTstGlo = tstGloVal.Split('¿');    //Array of modified values
        string sbDspText = string.Empty;
        string[] strGloVar = new string[arrTstGlo.Length];
        string value = string.Empty;
        string[] myValues = new string[arrTstGlo.Length];
        StringBuilder strglobalValues = new StringBuilder();
        ArrayList userVar = new ArrayList();

        globalVariables = GetGlobalVarsArray("globalvar");
        userVariables = GetGlobalVarsArray("uservar");

        //isLoggedIn = true; // This is for forcing the user to save Global vars if he doesnot have.
        Session["AxGloHideShow"] = "T";
        if (globalVariables == null)
            globalVariables = new ArrayList();
        if (userVariables == null)
            userVariables = new ArrayList();
        for (int i = 0; i < arrTstGlo.Length; i++)
        {
            string[] myStrVar = arrTstGlo[i].ToString().Split('♣');

            if (myStrVar.Length > 1)
            {
                myValues[i] = util.CheckSpecialChars(myStrVar[1].ToString());
            }
            strGloVar[i] = myStrVar[0];
            userVar.Add(strGloVar[i]);
            for (int j = 0; j < globalVariables.Count; j++)
            {
                bool flag = false;
                string[] strVar = globalVariables[j].ToString().Split('=');
                value = util.CheckSpecialChars(strVar[1].ToString());
                if (strGloVar[i] != "" && strVar[0].ToLower().Contains(strGloVar[i].ToLower()) && globalVariables[j].ToString().Split('=')[0].ToLower() == strGloVar[i].ToLower())
                {
                    string test = string.Empty;
                    flag = true;
                    globalVariables[j] = strGloVar[i] + "=" + myValues[i];
                    test = strglobalValues.ToString();
                    if (test.Contains(globalVariables[j].ToString()))
                        continue;
                    else
                        strglobalValues.Append(globalVariables[j].ToString() + ", ");
                }
                if (strVar[0].ToString().ToLower() == "axp_displaytext")
                {
                    globalVariables[j] = globalVariables[j].ToString().Split('=')[0] + "=" + hdnDisplayTxt.Value;
                }
                if (flag)
                    break;

            }
        }
        string globalValuesShown = strglobalValues.ToString();
        if (hdnDisplayTxt.Value.Length > 100)
        {
            strglobalValues.Remove(100, ((globalValuesShown.Length - 1) - 100));
            strglobalValues.Append("<a  onclick='javascript:ShowMore(\"" + globalValuesShown + "\");'> More</a>");

            if (isCloudApp)
            {
                //globalVariableValues.InnerHtml = sbDspText + FormatGlobalVariables(formatedDispTxt);//
            }
            else
            {
                FormatGlobalVariables(formatedDispTxt);
            }

        }
        else
        {
            if (isCloudApp)
            {
                //  globalVariableValues.InnerHtml = sbDspText + FormatGlobalVariables(formatedDispTxt);//
            }
            else
            {
                FormatGlobalVariables(formatedDispTxt);

            }
        }
        string url = string.Empty;
        if (hdnAxGloRecId.Value != "")
            url = "../aspx/ParamsTstruct.aspx?transid=" + paramTransid + "&recordid=" + hdnAxGloRecId.Value + "";
        //if (AxGloRecId != string.Empty)
        editAxGlo2.Attributes.Add("onclick", "BootstrapDialogAppParamsShow('" + url + "');");
        var AxGlo = UserOptions.Where(x => x.Key == "GlobalParamsonclick").ToList();
        if (AxGlo.Count > 0)
            UserOptions["GlobalParamsonclick"] = "\"onclick\":\"BootstrapDialogAppParamsShow('" + url + "');\"";
        else
            UserOptions.Add("GlobalParamsonclick", "\"onclick\":\"BootstrapDialogAppParamsShow('" + url + "');\"");
        string golVarNode = "<globalvars>";
        foreach (string item in globalVariables)
        {
            int valueIndex = item.IndexOf("=");
            string key = item.Substring(0, valueIndex);
            if (userVar.Contains(key))
                key = key.ToUpper();
            string val = item.Substring(valueIndex + 1);
            golVarNode += "<" + key + ">" + val + "</" + key + ">";
        }
        golVarNode += util.VirtualFilePathUpdate();
        golVarNode += util.GetGblVarsFromIni();
        golVarNode += "</globalvars>";
        Session["axGlobalVars"] = golVarNode;
        CreateParamaterArray();

        if (HttpContext.Current.Session["axGlobalVarsJson"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["axGlobalVarsJson"].ToString())) HttpContext.Current.Session.Remove("axGlobalVarsJson");
        if (HttpContext.Current.Session["ViewFiltersDictionary"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["ViewFiltersDictionary"].ToString())) HttpContext.Current.Session.Remove("ViewFiltersDictionary");
        if (HttpContext.Current.Session["ARM_SessionId"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["ARM_SessionId"].ToString())) HttpContext.Current.Session.Remove("ARM_SessionId");


        FDR fdrObj;
        if (HttpContext.Current.Session["FDR"] != null)
            fdrObj = (FDR)HttpContext.Current.Session["FDR"];
        else
            fdrObj = new FDR();

        FDW fdwObj = new FDW();
        string userName = HttpContext.Current.Session["username"].ToString();
        ArrayList arrKeys = fdrObj.GetPrefixedKeys("ARM-CARDDATA-General-" + userName, true);
        foreach (string key in arrKeys)
        {
            fdwObj.ClearRedisServerDataByKey(key, String.Empty, true);
        }

        GetMenuAndCardsData();

        if (cardsDataVal != string.Empty)
        {
            cardsData.Value = util.CheckSpecialChars(cardsDataVal);
        }


        if (Session["axp_projectcaption"] != null && Session["axp_projectcaption"].ToString() != "")
        {
            appName = Session["axp_projectcaption"].ToString();
            appTitle = appName;
            axpProjectCaption = appTitle;
        }
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
    public string getSmallMenuName(string fullname)
    {
        string nametosend = fullname;
        if (nametosend.Length > 0)
        {
            if (nametosend.Contains("tstruct.aspx"))
            {
                nametosend = "t" + fullname.Substring(fullname.IndexOf("transid=") + 8);
            }
            else
            {
                nametosend = "i" + fullname.Substring(fullname.IndexOf("ivname=") + 7);
            }
        }
        return nametosend;
    }

    protected void SaveMobileHybridDetails()
    {
        try
        {
            string hybridsessionexpiry = string.Empty;
            if (HttpContext.Current.Session["AxSessionExpiryDays"] != null && HttpContext.Current.Session["AxSessionExpiryDays"].ToString() != "")
                hybridsessionexpiry = HttpContext.Current.Session["AxSessionExpiryDays"].ToString();
            if (Session["hybridDeviceId"] != null && hybridsessionexpiry != string.Empty && hybridsessionexpiry != "0")
            {
                string hgDevId = Session["hybridDeviceId"].ToString();
                FDW fdwObj = new FDW();
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                string userName = string.Empty;
                string pwd = string.Empty;
                if (HttpContext.Current.Session["username"] != null)
                    userName = HttpContext.Current.Session["username"].ToString();
                if (HttpContext.Current.Session["pwd"] != null)
                    pwd = HttpContext.Current.Session["pwd"].ToString();

                string urlDomain = System.Web.HttpContext.Current.Request.Url.ToString();
                urlDomain = urlDomain.Substring(0, urlDomain.ToLower().IndexOf("/aspx/"));
                string hyProj = HttpContext.Current.Session["project"].ToString();
                string hySignedIn = HttpContext.Current.Session["staySignedId"].ToString();
                string hylanguage = HttpContext.Current.Session["language"].ToString();

                string hybridDetails = fdrObj.ReadKeyNoSchema(fdrObj.MakeKeyName(Constants.REDISHYBRIDINFO, hgDevId));
                if (hybridDetails != string.Empty)
                {
                    string[] hySavedInfo = hybridDetails.Split('~');
                    if (hySavedInfo[0] != hyProj || hySavedInfo[1] != userName || hySavedInfo[2] != pwd || hySavedInfo[4] != hylanguage || hySavedInfo[5] != urlDomain || hySavedInfo[6] != hybridsessionexpiry)
                    {
                        string datajson = hyProj + "~" + userName + "~" + pwd + "~" + hySignedIn + "~" + hylanguage + "~" + urlDomain + "~" + hybridsessionexpiry;
                        int expiryTime = int.Parse(hybridsessionexpiry) * 24 * 60;
                        fdwObj.WriteKeyNoSchema(fdrObj.MakeKeyName(Constants.REDISHYBRIDINFO, hgDevId), datajson, expiryTime);
                    }
                }
                else
                {
                    string datajson = hyProj + "~" + userName + "~" + pwd + "~" + hySignedIn + "~" + hylanguage + "~" + urlDomain + "~" + hybridsessionexpiry;
                    int expiryTime = int.Parse(hybridsessionexpiry) * 24 * 60;
                    fdwObj.WriteKeyNoSchema(fdrObj.MakeKeyName(Constants.REDISHYBRIDINFO, hgDevId), datajson, expiryTime);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    #region Application Configuration
    /// <summary>
    /// Loading Project's configuration details
    /// </summary>
    public void LoadAppConfiguration()
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
        // if (HttpContext.Current.Session["AxLandingPage"] != null)
        //     Application["-dPage-"] = "../aspx/Page.aspx";
        Session["AxEnableOldTheme"] = "false";
        Session["AxLogging"] = "false";

    }

    [WebMethod]
    public static void ExecutionTraceInterval(string ExecutionLongText, bool isSingout, string keepAlivems, string currPageUrl)
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
                if (HttpContext.Current.Session["KeepMeWebExpiry"] != null && currPageUrl != "")
                {
                    Util.Util utss = new Util.Util();
                    utss.SaveKeepAliveWebDetails(currPageUrl);
                }
                if (keepAlivems == "false" && HttpContext.Current.Session["KeepMeWebExpiry"] != null)
                    HttpContext.Current.Session.Remove("KeepMeWebExpiry");

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

    [WebMethod]
    public static string GetFullExecutionTrace()
    {
        string ExecutionLongText = string.Empty;
        try
        {
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            string sessId = HttpContext.Current.Session.SessionID;
            string ExecTraceKey = "Executiontrace-" + userName + "-" + sessId;
            ExecutionLongText = fdrObj.ReadKey(ExecTraceKey);
        }
        catch (Exception ex) { }
        return ExecutionLongText;
    }

    public static void DeletePrevExecKeys()
    {
        try
        {
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            string userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                userName = HttpContext.Current.Session["username"].ToString();
            string sessionId = HttpContext.Current.Session.SessionID;
            string thisKey = "Executiontrace-" + userName + "-" + sessionId;
            FDW fdwObj = new FDW();
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            ArrayList execKeys = fdrObj.GetAllKeys("-Executiontrace-" + userName + "-");
            if (execKeys.Count > 0)
            {
                string prevDelKeys = string.Empty;
                foreach (string exKey in execKeys)
                {
                    if (!exKey.EndsWith(thisKey))
                    {
                        string ExecutionLongText = fdrObj.ReadKeyNoSchema(exKey);
                        string sessId = exKey.Split('-').Last();
                        prevDelKeys += exKey + "&";
                        DBContext obj = new DBContext();
                        obj.SaveExecutionTrace(ExecutionLongText, sessId);
                    }
                }
                if (prevDelKeys != string.Empty)
                {
                    fdwObj.DeleteAllKeys(prevDelKeys, schemaName);
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("DeletePrevExecKeys--" + ex.Message, HttpContext.Current.Session.SessionID, "DeletePrevExecKeys", "new", "true");
        }
    }
    #endregion

    #region MainPage Banner

    private string ReadMainPageBannerJson()
    {
        string path = string.Empty;
        string project = HttpContext.Current.Session["Project"].ToString();
        path = HttpContext.Current.Server.MapPath("~/" + project);
        string filename = path + "\\mainpagebanner.json";
        string jsonContent = "";
        if (File.Exists(filename))
        {
        }
        else
        {
            path = HttpContext.Current.Server.MapPath("~/");
            filename = path + "\\mainpagebanner.json";
        }

        try
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    jsonContent = sr.ReadToEnd();
                }
            }
            var serializer = new JavaScriptSerializer();
            var items = serializer.Deserialize<List<Dictionary<string, string>>>(jsonContent);
            jsonContent = GenerateMainPageBannerHtml(items);
        }
        catch (Exception ex)
        {
            logobj.CreateLog(ex.Message, "Main page Banner", "login", "new");
        }
        return jsonContent;
    }

    public static string GenerateMainPageBannerHtml(List<Dictionary<string, string>> items)
    {
        StringBuilder htmlBuilder = new StringBuilder();
        htmlBuilder.AppendLine("<div id=\"Slider_Section\" class=\"row\">");
        htmlBuilder.AppendLine("<div id=\"HomeBanner\" class=\"row\">");
        htmlBuilder.AppendLine("<div class=\"axp-home-hero-section section\">");
        htmlBuilder.AppendLine("<div class=\"container\">");
        htmlBuilder.AppendLine("<div id=\"carouselExampleDark\" class=\"carousel carousel-dark slide\" data-ride=\"carousel\" data-wrap=\"true\" data-pause=\"hover\">");
        htmlBuilder.AppendLine("<div class=\"carousel-indicators\">");

        for (int i = 0; i < items.Count; i++)
        {
            htmlBuilder.AppendLine("<button type=\"button\" data-bs-target=\"#carouselExampleDark\" data-bs-slide-to=\"" + i + "\" aria-label=\"" + items[i]["title"] + "\" class=\" " + (i == 0 ? "active" : "") + "\" aria-current=\"" + (i == 0 ? "true" : "false") + "\"></button>");
        }

        htmlBuilder.AppendLine("</div>");
        htmlBuilder.AppendLine("<div class=\"carousel-inner\">");

        for (int i = 0; i < items.Count; i++)
        {
            htmlBuilder.AppendLine("<div class=\"carousel-item " + (i == 0 ? "active" : "") + "\" data-bs-interval=\"10000\">");
            htmlBuilder.AppendLine("<div class=\"row align-items-center\">");
            htmlBuilder.AppendLine("<div class=\"col-lg-12\">");
            htmlBuilder.AppendLine("<!-- Hero Content Start -->");
            htmlBuilder.AppendLine("<div class=\"hero-content\">");
            htmlBuilder.AppendLine("<img src=\"" + items[i]["image"] + "\" alt=\"\">");
            htmlBuilder.AppendLine("<div class=\"d-flex flex-column\">");
            htmlBuilder.AppendLine("<h2 class=\"title\">" + items[i]["title"] + "</h2>");
            htmlBuilder.AppendLine("<p class=\"text\">" + items[i]["description"] + "</p>");
            htmlBuilder.AppendLine("</div>");
            htmlBuilder.AppendLine("</div>");
            htmlBuilder.AppendLine("<!-- Hero Content End -->");
            htmlBuilder.AppendLine("</div>");
            htmlBuilder.AppendLine("</div>");
            htmlBuilder.AppendLine("</div>");
        }

        htmlBuilder.AppendLine("</div>");
        htmlBuilder.AppendLine("<button class=\"carousel-control-prev\" type=\"button\" data-bs-target=\"#carouselExampleDark\" data-bs-slide=\"prev\">");
        htmlBuilder.AppendLine("<span class=\"carousel-control-prev-icon\" aria-hidden=\"true\"><i class=\"fa fa-angle-left\"></i></span>");
        htmlBuilder.AppendLine("<span class=\"visually-hidden\">Previous</span>");
        htmlBuilder.AppendLine("</button>");
        htmlBuilder.AppendLine("<button class=\"carousel-control-next\" type=\"button\" data-bs-target=\"#carouselExampleDark\" data-bs-slide=\"next\">");
        htmlBuilder.AppendLine("<span class=\"carousel-control-next-icon\" aria-hidden=\"true\"><i class=\"fa fa-angle-right\"></i></span>");
        htmlBuilder.AppendLine("<span class=\"visually-hidden\">Next</span>");
        htmlBuilder.AppendLine("</button>");
        htmlBuilder.AppendLine("</div>");
        htmlBuilder.AppendLine("</div>");
        htmlBuilder.AppendLine("</div>");
        htmlBuilder.AppendLine("</div>");
        htmlBuilder.AppendLine("</div>");

        return htmlBuilder.ToString();
    }
    #endregion

    #region HTML Plugin based Home Page


    [WebMethod]
    public static string GetCardsWithDataWS(bool isHomePage, bool refreshData)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        ASB.WebService asbWebService = new ASB.WebService();
        string cardsData = asbWebService.GetCardsWithData(isHomePage, refreshData);
        if (cardsData.IndexOf("SessionId is not valid") > -1)
            return Constants.SESSIONTIMEOUT;
        else
            return cardsData;
    }

    [WebMethod]
    public static string GetCardsWithDataOnLoginWS()
    {
        bool isHomePage = true;
        bool refreshData = false;
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        ASB.WebService asbWebService = new ASB.WebService();
        string cardsData = asbWebService.GetCardsWithData(isHomePage, refreshData);
        Util.Util util = new Util.Util();
        cardsData = util.CheckSpecialChars(cardsData);
        if (cardsData.IndexOf("SessionId is not valid") > -1)
            return Constants.SESSIONTIMEOUT;
        else
            return cardsData;
    }


    #endregion

}

