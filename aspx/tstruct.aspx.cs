using CacheMgr;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;


public partial class Tstruct : System.Web.UI.Page
{
    #region Variable Declaration
    Util.Util util;
    public string proj = string.Empty;
    public string sid = string.Empty;
    public string language = string.Empty;
    public string trace = string.Empty;
    public string user = string.Empty;
    public string transId = string.Empty;
    public string AxRole = string.Empty;
    public string rid = string.Empty;
    public string searchVal = string.Empty;
    public string direction = "ltr";
    public string classdir = "left";
    public string appsessionKey = string.Empty;
    public int actbtncount = 0;
    int docHgt = 0;
    public string optStr = string.Empty;
    //Variables to store toolbar buttons and their left values
    ArrayList leftBtns = new ArrayList();    //left index of the btns, to arrange the order
    ArrayList tmpLeftBtns = new ArrayList();
    ArrayList toolBarBtns = new ArrayList();
    ArrayList actionBarBtns = new ArrayList();
    ArrayList sortedBtns = new ArrayList();
    ArrayList paramNames = new ArrayList();
    ArrayList paramValues = new ArrayList();
    ArrayList clientParamValues = new ArrayList();
    ArrayList headNames = new ArrayList();
    ArrayList customBtnHtml = new ArrayList();
    string AxOnApproveDisable = "false";
    string AxOnRejectDisable = "false";
    string AxOnReturnSave = "false";
    string AxOnRejectSave = "false";

    string AxLogTimeTaken = "false";
    //variables to store the Html in the page
    StringBuilder tstHTML = new StringBuilder();
    public StringBuilder submitCancelBtns = new StringBuilder();
    StringBuilder taskBtnHtml = new StringBuilder();
    StringBuilder attHtml = new StringBuilder();
    public StringBuilder toolbarBtnHtml = new StringBuilder();
    public StringBuilder modernToolbarBtnHtml = new StringBuilder();
    public StringBuilder modernFooterActBtnHtml = new StringBuilder();

    public StringBuilder dcHtml = new StringBuilder();
    public StringBuilder tstHeader = new StringBuilder();
    public String tstCss = string.Empty;
    StringBuilder tstSavedHtml = new StringBuilder();


    //Public varaibles declaration     
    public Custom customObj = null;
    string actstr = string.Empty;
    string actstrType = "open";
    string formFoadData = "";
    string transactionROMode = "";
    string loadResult = string.Empty;
    string strGlobalVar = string.Empty;
    string fileName = string.Empty;
    public string errorLog = string.Empty;
    string queryStr = string.Empty;
    public string tstCaption = string.Empty;
    public string menuBreadCrumb = string.Empty;
    public string tstName = string.Empty;
    public StringBuilder tstVars = new StringBuilder();
    public StringBuilder tstJsArrays = new StringBuilder();
    public StringBuilder tstScript = new StringBuilder();
    public StringBuilder tstTabScript = new StringBuilder();
    public string tstDraftsScript = string.Empty;
    public string traceLog = string.Empty;
    // public string tstTimeVars = string.Empty;
    public string btnFunction = string.Empty;
    string btnStyle = "handCur";
    string btnHTML = string.Empty;
    string customFolder = string.Empty;
    string customPage = string.Empty;
    LogFile.Log logobj = new LogFile.Log();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    public string structXml = string.Empty;
    public string jsFromCache = string.Empty;
    public string htmlFromCache = string.Empty;
    Boolean isTstInCache = false;
    Boolean isTstructCached = false;
    string draftsPath = string.Empty;
    public static string enableBackForwButton = string.Empty;
    public bool isTstPop = false;
    public string axpRefreshParent = "false";
    public string axRefreshSelect = "false";
    public string axRefreshSelectID = "";
    public string axSrcSelectID = "";
    public string axRefreshSelectType = "";
    public string langauge = "ENGLISH";
    // public string ptransid = string.Empty;
    public string exportTallyTid = string.Empty;
    TStructDef strObj = null;
    public string dcGridOnSave = "true";
    public string designModeBtnHtml = String.Empty;

    public string isAxpImagePath = "false";
    #endregion

    public StringBuilder getLang = new StringBuilder();
    public StringBuilder strLogTime = new StringBuilder();
    public float formLogTime;
    public float pageLogTime;
    DateTime stTime;
    DateTime edTime;
    public int FetchPickListRows = 1000;
    public bool isRapidLoad = false;
    public bool loadFromMem = true;
    public string defaultDepFlds = String.Empty;
    public string langType = "en";
    //public bool newDesigner = false;
    public bool theModeDesign = false;
    public string axWizardType = "";
    public string wizardClass = "";
    public string wizardLoadRecord = "false";
    public string schemaName = string.Empty;
    string isTstFromHyperLink = "false";
    string designJson = string.Empty;
    DataTable axpConfigStr = new DataTable();
    string AxpIsAutoSplit = string.Empty;
    string AxpIviewDisableSplit = string.Empty;
    string AxpFileUploadlmt = string.Empty;
    int googleMapsZoom = 11;
    string AxpCameraOption = string.Empty;
    string AxpNotFillDepFields = string.Empty;
    string AxpFillDepFields = string.Empty;
    string AxpFillDepEnabled = "false";
    string AxpForceValidation = string.Empty;
    string AxpSaveImageDb = "false";
    string AxpGridFormCols = string.Empty;
    string AxpCustomJs = string.Empty;
    string AxpCustomCss = string.Empty;
    public string AxpTstButtonStyle = "classic";
    public string AxShowControlBorders = "true";
    string axpShowKeyboard = string.Empty;
    public string dvFooterHtml = string.Empty;
    string AutosaveDraftt = string.Empty;
    string AutosaveDraftTime = string.Empty;
    string UploadFileTypess = string.Empty;
    string UploadFileTypesVal = string.Empty;
    string savedraftKeyCreatedtime = string.Empty;
    public bool isTstCustomHtml = false;
    public bool isMobileView = false;
    string gridScrollBar = string.Empty;
    string gridFixedHeader = string.Empty;
    bool AxSplit = false;
    public string btnfooterlist = string.Empty;
    public string btnfooteropenlist = string.Empty;
    ExecTrace ObjExecTr = ExecTrace.Instance;
    public string requestProcess_logtime = string.Empty;
    public string serverprocesstime = string.Empty;
    string mobileCardLayout = "none";
    bool openFastReportInNewWindow = false;
    public string formLayoutClass = "";
    bool fillGridDataQueryOrder = false;
    bool fillGridDataShowAll = false;
    string AxMemVarClient = "false";
    string ConfigDataClient = "false";
    string DropdownSearchStartsWith = "contains";
    string isPegEdit = "false";
    public string AxFillAutoSelectFlds = "false";
    public string AxEnableCheckbox = "false";
    public string AxCustomTranSaveSuccess = "false";
    public string AxtstAttachFSDB = "false";
    public string AxHideSelectAll = "false";
    public string FreezeGridCols = string.Empty;
    public string AxMandatoryWfComments = "true";
    public string TstPopupCloseOnSubmit = "true";
    public string tstIsFileProtected = "false";
    JObject StructConfigs = new JObject();
    public string isServerSide = "true";
    public string isServerDummyPost = "false";
    public string isServerDummyRec = "";
    public string isDraftbutton = "false";
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

    #region PageLoad
    /// <summary>
    /// Page Load event of the tstruct page where the tstruct construction is initialized.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

        if (Request.QueryString["dummyload"] != null && (Request.QueryString["dummyload"].ToString() == "true" || Request.QueryString["dummyload"].ToString().StartsWith("true♠")) && Request.QueryString["theMode"] == null)
        {
            isServerSide = "false";
            isServerDummyRec = "&dummyload=" + Request.QueryString["dummyload"].ToString();
            isServerDummyPost = "true";
            string _thistrId = Request.QueryString["transid"].ToString();
            if (Request.QueryString["dummyload"].ToString().StartsWith("true♠"))
            {
                string _tstKey = Request.QueryString["dummyload"].ToString();
                string _thisKey = _tstKey.Split('♠')[1];
                if (Request.QueryString["recordid"] != null && Request.QueryString["recordid"].ToString() != "")
                {
                    string isDupTab = "";
                    if (Request.QueryString["isDupTab"] != null)
                        isDupTab = Request.QueryString["isDupTab"].ToString();
                    string _thisRecId = Request.QueryString["recordid"].ToString();
                    string _strQuery = GetQueryStringList();
                    hdnTstLoadDummy.Value = GetLoadDataValues(_thisKey, _thisRecId, _strQuery, isDupTab, _thistrId, "true");
                    IncludeJsFilesDummy(_thistrId);
                    if (Session["project"] != null)
                        hdnTstSInfo.Value = Session["project"].ToString() + "~" + Session["user"].ToString() + "~" + _thistrId + "~" + Session["nsessionid"].ToString() + "~" + Session["AxRole"].ToString() + "~" + Session["AxTrace"].ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "dummyload", "tstDummyLoad('" + _thistrId + "');", true);
                    return;
                }
                else
                {

                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                    string _flKey = fdrObj.MakeKeyName(Constants.FORMLOADRES, _thistrId);
                    string _thisFlSubKey = _thistrId + "♦" + DateTime.Now.ToString("dd/MM/yyyy");
                    string result = fdrObj.HashGetKey(_flKey, _thisFlSubKey);
                    if (result == "true")
                    {
                        string _thsHtml = GetLsTstHtml(_thistrId, _thisFlSubKey);
                        if (_thsHtml.Contains("*loaddata*"))
                        {
                            FDW fdwObjNew = new FDW();
                            fdwObjNew.HashDeletekey(_flKey, _thisFlSubKey);
                            isServerSide = "true";
                            isServerDummyPost = "false";
                        }
                        else
                        {
                            GetConfigOnDummyLoad(_thistrId);
                            string isDupTab = "";
                            if (Request.QueryString["isDupTab"] != null)
                                isDupTab = Request.QueryString["isDupTab"].ToString();
                            string _strQuery = GetQueryStringList();
                            hdnTstLoadDummy.Value = GetFormLoadValues(_thisKey, _strQuery, "false", "false", isDupTab, _thistrId, "true");
                            IncludeJsFilesDummy(_thistrId);
                            if (Session["project"] != null)
                                hdnTstSInfo.Value = Session["project"].ToString() + "~" + Session["user"].ToString() + "~" + _thistrId + "~" + Session["nsessionid"].ToString() + "~" + Session["AxRole"].ToString() + "~" + Session["AxTrace"].ToString();
                            if (Session["savedraftKeyCreatedtime"] != null)
                            {
                                hdnTstAutoSD.Value = Session["savedraftKeyCreatedtime"].ToString();
                                Session.Remove("savedraftKeyCreatedtime");
                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "dummyload", "tstDummyLoad('" + _thistrId + "');", true);
                            return;
                        }
                    }
                    else
                    {
                        string isDupTab = "";
                        if (Request.QueryString["isDupTab"] != null)
                            isDupTab = Request.QueryString["isDupTab"].ToString();
                        string _strQuery = GetQueryStringList();
                        hdnTstLoadDummy.Value = GetFormLoadValues(_thisKey, _strQuery, "false", "false", isDupTab, _thistrId, "true");
                        IncludeJsFilesDummy(_thistrId);
                        if (Session["project"] != null)
                            hdnTstSInfo.Value = Session["project"].ToString() + "~" + Session["user"].ToString() + "~" + _thistrId + "~" + Session["nsessionid"].ToString() + "~" + Session["AxRole"].ToString() + "~" + Session["AxTrace"].ToString();
                        if (Session["savedraftKeyCreatedtime"] != null)
                        {
                            hdnTstAutoSD.Value = Session["savedraftKeyCreatedtime"].ToString();
                            Session.Remove("savedraftKeyCreatedtime");
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "dummyload", "tstDummyLoad('" + _thistrId + "');", true);
                        return;
                    }
                }
            }
            else
            {
                SaveDraftOnDummy(_thistrId);
                IncludeJsFilesDummy(_thistrId);
                string _strGlobalVar = util.GetGlobalVarString();
                if (_strGlobalVar != string.Empty)
                {
                    hdnDummyParams.Value = _strGlobalVar;
                }
                if (Session["project"] != null)
                    hdnTstSInfo.Value = Session["project"].ToString() + "~" + Session["user"].ToString() + "~" + _thistrId + "~" + Session["nsessionid"].ToString() + "~" + Session["AxRole"].ToString() + "~" + Session["AxTrace"].ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "dummyload", "tstDummyLoad('" + _thistrId + "');", true);
                return;
            }
        }
        else if (Request.QueryString["dummyload"] != null && (Request.QueryString["dummyload"].ToString() == "false" || Request.QueryString["dummyload"].ToString().StartsWith("false♠")) && Request.QueryString["theMode"] == null)
        {
            isServerDummyRec = "&dummyload=" + Request.QueryString["dummyload"].ToString();
            string _thisTrId = Request.QueryString["transid"].ToString();

            string _thisdttKey = "";
            bool forceCall = false;
            if (Request.QueryString["dummyload"].ToString().StartsWith("false♠"))
            {
                string _tstKey = Request.QueryString["dummyload"].ToString();
                if (_tstKey.Split('♠')[1] == "forcecall")
                    forceCall = true;
                else
                    _thisdttKey = _tstKey.Split('♠')[1];
            }

            string isDupTab = "";
            if (Request.QueryString["isDupTab"] != null)
                isDupTab = Request.QueryString["isDupTab"].ToString();

            string _thisRecId = "";
            if (Request.QueryString["recordid"] != null)
                _thisRecId = Request.QueryString["recordid"].ToString();

            string tstglobaVars = GetTstHtmllsGlobalVars(_thisTrId);
            if (tstglobaVars != "")
            {
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                string _flKey = fdrObj.MakeKeyName(Constants.FORMLOADRES, _thisTrId);
                string _thisFlSubKey = _thisTrId + "♦" + DateTime.Now.ToString("dd/MM/yyyy");
                if (forceCall)
                {
                    FDW fdwObjNew = new FDW();
                    fdwObjNew.HashDeletekey(_flKey, _thisFlSubKey);
                    isServerSide = "true";
                    isServerDummyPost = "false";
                }
                else
                {
                    string result = fdrObj.HashGetKey(_flKey, _thisFlSubKey);
                    if (result == "true")
                    {
                        string _thsHtml = GetLsTstHtml(_thisTrId, _thisFlSubKey);
                        //if (_thsHtml != "" && _thsHtml.Contains("*loaddata*"))
                        //{
                        //    FDW fdwObjNew = new FDW();
                        //    fdwObjNew.HashDeletekey(_flKey, _thisFlSubKey);
                        //    isServerSide = "true";
                        //    isServerDummyPost = "false";
                        //}
                        if (_thsHtml == "")
                        {
                            FDW fdwObjNew = new FDW();
                            fdwObjNew.HashDeletekey(_flKey, _thisFlSubKey);
                            isServerSide = "true";
                            isServerDummyPost = "false";
                        }
                        if (_thsHtml != "")
                        {
                            GetConfigOnDummyLoad(_thisTrId);
                            string qryStrVars = GetQueryStringList();
                            if (_thisRecId != "")
                            {
                                hdnTstLoadDummy.Value = GetLoadDataValues(_thisdttKey, _thisRecId, qryStrVars, isDupTab, _thisTrId, "true");
                                IncludeJsFilesDummy(_thisTrId);
                                isServerSide = "false";
                                isServerDummyPost = "true";
                                hdntstHtmlDummy.Value = _thsHtml;
                                if (Session["project"] != null)
                                    hdnTstSInfo.Value = Session["project"].ToString() + "~" + Session["user"].ToString() + "~" + _thisTrId + "~" + Session["nsessionid"].ToString() + "~" + Session["AxRole"].ToString() + "~" + Session["AxTrace"].ToString();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "dummyload", "tstHtmlDummyLoad('" + _thisTrId + "');", true);
                            }
                            else
                            {
                                hdnisHtmlLsLoad.Value = "*formload*";
                                hdnTstLoadDummy.Value = GetFormLoadValues(_thisdttKey, qryStrVars, "false", "false", isDupTab, _thisTrId, "true");
                                IncludeJsFilesDummy(_thisTrId);
                                isServerSide = "false";
                                isServerDummyPost = "true";
                                hdntstHtmlDummy.Value = _thsHtml;
                                if (Session["project"] != null)
                                    hdnTstSInfo.Value = Session["project"].ToString() + "~" + Session["user"].ToString() + "~" + _thisTrId + "~" + Session["nsessionid"].ToString() + "~" + Session["AxRole"].ToString() + "~" + Session["AxTrace"].ToString();
                                if (Session["savedraftKeyCreatedtime"] != null)
                                {
                                    hdnTstAutoSD.Value = Session["savedraftKeyCreatedtime"].ToString();
                                    Session.Remove("savedraftKeyCreatedtime");
                                }
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "dummyload", "tstHtmlDummyLoad('" + _thisTrId + "');", true);
                            }
                            return;
                        }
                    }
                    else if (result == "false")
                    {
                        string _thsHtml = GetLsTstHtml(_thisTrId, _thisFlSubKey);
                        if (_thsHtml != "")
                        {
                            GetConfigOnDummyLoad(_thisTrId);
                            string global_Scripts = string.Empty;
                            if (_thisRecId != "")
                            {
                                string qryStrVars = GetQueryStringList();
                                hdnTstLoadDummy.Value = GetLoadDataValues(_thisdttKey, _thisRecId, qryStrVars, isDupTab, _thisTrId, "true");
                            }
                            else
                            {
                                SaveDraftOnDummy(_thisTrId);

                                string _strGlobalVar = util.GetGlobalVarString();
                                if (_strGlobalVar != string.Empty)
                                {
                                    hdnDummyParams.Value = _strGlobalVar;
                                }
                                hdnisHtmlLsLoad.Value = "*formload*";

                                string qryStrVars = GetQueryStringList();
                                hdnTstLoadDummy.Value = GetFormLoadValues(_thisdttKey, qryStrVars, "false", "false", isDupTab, _thisTrId, "true");
                            }
                            IncludeJsFilesDummy(_thisTrId);
                            isServerSide = "false";
                            isServerDummyPost = "true";
                            hdntstHtmlDummy.Value = _thsHtml;
                            if (Session["project"] != null)
                                hdnTstSInfo.Value = Session["project"].ToString() + "~" + Session["user"].ToString() + "~" + _thisTrId + "~" + Session["nsessionid"].ToString() + "~" + Session["AxRole"].ToString() + "~" + Session["AxTrace"].ToString();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "dummyload", "tstHtmlDummyLoad('" + _thisTrId + "');", true);
                            return;
                        }
                    }
                    else
                    {
                        string tstglobaVarKey = CreatetstGlbVarKey(tstglobaVars);
                        string strDopTime = "";
                        try
                        {
                            string fldKeyDop = Constants.REDISTSTRUCTDOPTIME;
                            string schemaName = string.Empty;
                            if (HttpContext.Current.Session["dbuser"] != null)
                                schemaName = HttpContext.Current.Session["dbuser"].ToString();
                            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                            if (fObj != null)
                                strDopTime = fObj.StringFromRedis(util.GetRedisServerkey(fldKeyDop, _thisTrId, tstglobaVarKey), schemaName);
                        }
                        catch (Exception ex) { }

                        if (strDopTime != "")
                            tstglobaVarKey = strDopTime;
                        else
                            tstglobaVarKey += "♦" + DateTime.Now.ToString("dd/MM/yyyy");
                        if (tstglobaVars != "novars")
                        {
                            string _thsHtml = GetLsTstHtml(_thisTrId, tstglobaVarKey);
                            if (_thsHtml != "")
                            {
                                GetConfigOnDummyLoad(_thisTrId);
                                if (_thisRecId != "")
                                {
                                    string qryStrVars = GetQueryStringList();
                                    hdnTstLoadDummy.Value = GetLoadDataValues(_thisdttKey, _thisRecId, qryStrVars, isDupTab, _thisTrId, "true");
                                }
                                else
                                {
                                    hdnisHtmlLsLoad.Value = "*formload*";
                                    string qryStrVars = GetQueryStringList();
                                    hdnTstLoadDummy.Value = GetFormLoadValues(_thisdttKey, qryStrVars, "false", "false", isDupTab, _thisTrId, "true");
                                }
                                IncludeJsFilesDummy(_thisTrId);
                                isServerSide = "false";
                                isServerDummyPost = "true";
                                hdntstHtmlDummy.Value = _thsHtml;
                                if (Session["project"] != null)
                                    hdnTstSInfo.Value = Session["project"].ToString() + "~" + Session["user"].ToString() + "~" + _thisTrId + "~" + Session["nsessionid"].ToString() + "~" + Session["AxRole"].ToString() + "~" + Session["AxTrace"].ToString();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "dummyload", "tstHtmlDummyLoad('" + _thisTrId + "');", true);
                                return;
                            }
                        }
                        else
                        {
                            string _thsHtml = GetLsTstHtml(_thisTrId, tstglobaVarKey);
                            if (_thsHtml != "")
                            {
                                GetConfigOnDummyLoad(_thisTrId);
                                if (_thisRecId != "")
                                {
                                    string qryStrVars = GetQueryStringList();
                                    hdnTstLoadDummy.Value = GetLoadDataValues(_thisdttKey, _thisRecId, qryStrVars, isDupTab, _thisTrId, "true");
                                }
                                else
                                {
                                    hdnisHtmlLsLoad.Value = "*formload*";
                                    string qryStrVars = GetQueryStringList();
                                    hdnTstLoadDummy.Value = GetFormLoadValues(_thisdttKey, qryStrVars, "false", "false", isDupTab, _thisTrId, "true");
                                }
                                IncludeJsFilesDummy(_thisTrId);
                                isServerSide = "false";
                                isServerDummyPost = "true";
                                hdntstHtmlDummy.Value = _thsHtml;
                                if (Session["project"] != null)
                                    hdnTstSInfo.Value = Session["project"].ToString() + "~" + Session["user"].ToString() + "~" + _thisTrId + "~" + Session["nsessionid"].ToString() + "~" + Session["AxRole"].ToString() + "~" + Session["AxTrace"].ToString();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "dummyload", "tstHtmlDummyLoad('" + _thisTrId + "');", true);
                                return;
                            }
                        }
                    }
                }
            }
        }


        if (!IsPostBack && Request.QueryString["hdnbElapsTime"] != null)
        {
            string browserElapsTime = Request.QueryString["hdnbElapsTime"] != null ? Request.QueryString["hdnbElapsTime"] : "0";
            requestProcess_logtime += ObjExecTr.WireElapsTime(browserElapsTime);
        }
        else if (IsPostBack && hdnbElapsTimeGo.Value != "")
        {
            string browserElapsTime = hdnbElapsTimeGo.Value == "" ? "0" : hdnbElapsTimeGo.Value;
            requestProcess_logtime += ObjExecTr.WireElapsTime(browserElapsTime);
        }
        if (Request.QueryString["reqProc_logtime"] != null)
            requestProcess_logtime += Request.QueryString["reqProc_logtime"];
        if (!IsPostBack)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "controlDimmer", "eval(callParent(\"loadFrame\", \"function\"));", true);
        util = new Util.Util();
        stTime = DateTime.Now;
        DateTime startTime = DateTime.Now;
        //logobj.CreateLog("Page load " , sid, "LogTimeTaken", "new");
        util.IsValidSession();
        ResetSessionTime();

        DateTime webStart = DateTime.Now;
        Response.ExpiresAbsolute = DateTime.Now;
        Response.Expires = -1441;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.AddHeader("Pragma", "no-store");
        Response.AddHeader("cache-control", "no-cache");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoServerCaching();

        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }
        else
        {
            if (util.IsValidQueryString(Request.RawUrl, true) == false)
            {
                requestProcess_logtime += "Server - " + Constants.INVALIDURL + " ♦ ";
                HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.INVALIDURL + "*♠*" + requestProcess_logtime);
            }
            SetSessionVariables();
            //if (!util.licencedValidSessionCheck())
            //{
            //    try
            //    {
            //        HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG, true);
            //    }
            //    catch (ThreadAbortException ex)
            //    {​​​​​
            //      Thread.ResetAbort();
            //    }​​​​​
            //    return;
            //}

            //TODO: ask malakonda and remove the below code if not used
            if (HttpContext.Current.Session["fd-HugeFlds"] != null)
                util.ClearFDFldSession();

            DateTime sdate = DateTime.Now;
            ConstructTstruct();
            DateTime enddate = DateTime.Now;
            strLogTime.Append("TotConstructTstruct-" + sdate.Subtract(enddate).TotalMilliseconds.ToString());

            CustomDiv.InnerHtml = customObj.GetCustomDivHtml();
            //IncludeJsFiles();
            IncludeJsFilesNew();
            SetLangStyles();
        }
        util.TempAttaServerFiles();
        util.DeleteKeyOnRefreshSave();

        UpdateTstAttLoad();

        UpdateNavigation();
        CheckDraftPopup(transId);
        if (!isTstPop && rid == "0")
            util.RemovelvListPageLoad();

        //Code to store the timetaken details
        TStructData tstData = (TStructData)Session[hdnDataObjId.Value];
        if (tstData != null && AxLogTimeTaken == "true")
        {
            tstData.strServerTime = stTime.Subtract(webStart).TotalMilliseconds + "," + edTime.Subtract(stTime).TotalMilliseconds + "," + DateTime.Now.Subtract(edTime).TotalMilliseconds;
            Session[hdnDataObjId.Value] = tstData;
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "controlDimmer", "closeParentFrame();", true);
        edTime = DateTime.Now;
        strLogTime.Append("PageLoad FUll-" + startTime.Subtract(edTime).TotalMilliseconds.ToString());
        pageLogTime = float.Parse(edTime.Subtract(startTime).TotalMilliseconds.ToString());
        pageLogTime = pageLogTime - formLogTime;
        if (Session["AxDcGridOnSave"] != null)
            dcGridOnSave = HttpContext.Current.Session["AxDcGridOnSave"].ToString();
        AxpFileUploadlmt = AxpFileUploadlmt == string.Empty ? "0" : AxpFileUploadlmt.ToString();//UploadFileTypesVal//UploadFileTypes
        int axDecimal = -1;
        try
        {
            if (HttpContext.Current.Session["axdecimal"] != null && HttpContext.Current.Session["axdecimal"].ToString() != "")
                axDecimal = int.Parse(HttpContext.Current.Session["axdecimal"].ToString());
        }
        catch (Exception ex)
        {
            axDecimal = -1;
        }
        try
        {
            GetFreezeCols(transId);
        }
        catch (Exception ex)
        { }
        try
        {
            if (ConfigurationManager.AppSettings["axpertWebDirPath"] != null && ConfigurationManager.AppSettings["axpertWebDirPath"].ToString() != "")
                hdnAxpertWebDirPath.Value = ConfigurationManager.AppSettings["axpertWebDirPath"].ToString();
        }
        catch (Exception ex) { }
        tstScript.Append("<script>var dcGridOnSave = '" + dcGridOnSave.ToString() + "';var theModeDesign = '" + theModeDesign.ToString().ToLower() + "';var AxpIsAutoSplit = '" + AxpIsAutoSplit.ToLower() + "';var AxpIviewDisableSplit = '" + AxpIviewDisableSplit.ToLower() + "';var AxpFileUploadlmt='" + AxpFileUploadlmt.ToString() + "';var AxpCameraOption ='" + AxpCameraOption.ToString() + "';var AxpNotFillDepFields='" + AxpNotFillDepFields.ToString() + "';var AxpFillDepFields='" + AxpFillDepFields.ToString() + "';var AxpFillDepEnabled='" + AxpFillDepEnabled.ToString() + "';var AutosaveDraft='" + AutosaveDraftt.ToLower() + "';var AutosaveDraftTime='" + AutosaveDraftTime.ToLower() + "';var UploadFileTypes='" + UploadFileTypess.ToLower() + "';var UploadFileTypesVal='" + UploadFileTypesVal.ToLower() + "';var savedraftKeyCreatedtime='" + savedraftKeyCreatedtime.ToLower() + "';var IsObjCustomHtml='" + strObj.IsObjCustomHtml + "';var gridScrollBar='" + gridScrollBar.ToLower() + "';var gridFixedHeader='" + gridFixedHeader.ToLower() + "';var AxpGridFormCols='" + AxpGridFormCols.ToString() + "';var axpShowKeyboard='" + axpShowKeyboard + "';var breadCrumbStr='" + menuBreadCrumb.Replace("'", "\\'") + "';var mobileCardLayout='" + mobileCardLayout + "';var openFastReportInNewWindow = " + openFastReportInNewWindow.ToString().ToLower() + ";var fillGridDataQueryOrder=" + fillGridDataQueryOrder.ToString().ToLower() + ";var fillGridDataShowAll=" + fillGridDataShowAll.ToString().ToLower() + ";var googleMapsZoom = " + googleMapsZoom.ToString() + ";var AxMemVarClient=" + AxMemVarClient + ";var gloAxDecimal=" + axDecimal + ";var DropdownSStartsWith='" + DropdownSearchStartsWith.ToString() + "';var AxpForceValidation='" + AxpForceValidation.ToString() + "';var isPegEditMode='" + isPegEdit + "';var AxFillAutoSelectFlds='" + AxFillAutoSelectFlds + "';var tstConfigurations = " + StructConfigs.ToString(Newtonsoft.Json.Formatting.None) + ";var AxEnableCheckbox='" + AxEnableCheckbox + "';var AxCustomTranSaveSuccess='" + AxCustomTranSaveSuccess + "';var AxtstAttachFSDB='" + AxtstAttachFSDB + "';var AxHideSelectAll='" + AxHideSelectAll + "';var gridFreezeColName='" + FreezeGridCols + "';var AxMandatoryWfComments='" + AxMandatoryWfComments + "';var TstPopupCloseOnSubmit='" + TstPopupCloseOnSubmit + "';</script>");
        requestProcess_logtime += ObjExecTr.RequestProcessTime("Response");
        serverprocesstime = ObjExecTr.TotalServerElapsTime();
    }

    protected void SaveDraftOnDummy(string _thistrId)
    {
        if (Session["tstautosaveDraftt"] != null)
        {
            AutosaveDraftt = Session["tstautosaveDraftt"].ToString();
            CheckDraftPopup(_thistrId);
        }
        if (Session["savedraftKeyCreatedtime"] != null)
        {
            hdnTstAutoSD.Value = Session["savedraftKeyCreatedtime"].ToString();
            Session.Remove("savedraftKeyCreatedtime");
        }
    }
    protected string GetQueryStringList()
    {
        string res = string.Empty;
        if (Request.QueryString["dummyload"].ToString().StartsWith("false♠") || Request.QueryString["dummyload"].ToString().StartsWith("true♠"))
        {
            foreach (string _key in Request.QueryString)
            {
                if (Request.QueryString[_key] != "" && Request.QueryString[_key].ToString().Contains("♠"))
                    res += _key + "=" + Request.QueryString[_key].Split('♠')[0] + "♠";
                else
                    res += _key + "=" + Request.QueryString[_key] + "♠";
            }
        }
        return res;
    }
    private void UpdateTstAttLoad()
    {
        try
        {
            if (Session["tstHeaderAttachDbFileOnLoad"] != null && Session["tstHeaderAttachDbFileOnLoad"].ToString() != "")
            {
                if (Session["tstHeaderAttachDbFile"] != null && Session["tstHeaderAttachDbFile"].ToString() != "")
                    Session["tstHeaderAttachDbFile"] = Session["tstHeaderAttachDbFile"] + "," + Session["tstHeaderAttachDbFileOnLoad"].ToString();
                else
                    Session["tstHeaderAttachDbFile"] = Session["tstHeaderAttachDbFileOnLoad"].ToString();
                Session.Remove("tstHeaderAttachDbFileOnLoad");
            }
        }
        catch (Exception ex) { }
    }

    private void UpdateNavigation()
    {
        if (Session["AxFromHypLink"] != null)
            isTstFromHyperLink = Session["AxFromHypLink"].ToString();

        if (!string.IsNullOrEmpty(Request.QueryString["axp_IsSaveUrl"]))
        {
            if (Request.QueryString["axp_IsSaveUrl"].ToString() == "true")
                Session["axp_IsSaveUrl"] = "true";
        }
        string frameName = string.Empty;
        if (Request.QueryString["axpfrm"] != null)
            frameName = Convert.ToString(Request.QueryString["axpfrm"]);
        if (Session["backForwBtnPressed"] == null || (Session["backForwBtnPressed"] != null && !Convert.ToBoolean(Session["backForwBtnPressed"])) && (Request.QueryString.Count < 2 || Request.UrlReferrer != null && (Request.UrlReferrer.AbsolutePath.Contains("listIview.aspx") || Request.UrlReferrer.AbsolutePath.Contains("iview.aspx") || Request.UrlReferrer.AbsolutePath.Contains("tstruct.aspx") || Request.UrlReferrer.AbsolutePath.Contains("tstructdesign.aspx") || Request.UrlReferrer.AbsolutePath.Contains("mainnew.aspx") || Request.UrlReferrer.AbsolutePath.Contains("axmain.aspx"))) && frameName != "t")
        {
            if (isTstPop)
                Session["enableBackButton"] = "false";
            else if (Session["AxHypTstRefresh"] != null && Session["AxHypTstRefresh"].ToString() == "true")
            {
                Session["AxHypTstRefresh"] = "false";
            }
            else if (Request.QueryString["AxHypTstRefresh"] != null && Request.QueryString["AxHypTstRefresh"].ToString() == "true")
            {

            }
            else
                util.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri);
        }
        if (Session["axp_IsSaveUrl"] != null)
            Session["axp_IsSaveUrl"] = null;
        Session["backForwBtnPressed"] = false;
        if (Session["RapidTsTruct"] != null && Session["RapidTsTruct"].ToString() == "true")
            enableBackForwButton = "<script language=\'javascript\' type=\'text/javascript\' > enableBackButton='" + Convert.ToBoolean(Session["enableBackButton"]) + "';" + " enableForwardButton='" + Convert.ToBoolean(Session["enableForwardButton"]) + "'; var fromHyperLink='" + isTstFromHyperLink + "';var isRapidLoad='" + isRapidLoad + "';var defaultDepFlds='" + defaultDepFlds + "';</script>";
        else
            enableBackForwButton = "<script language=\'javascript\' type=\'text/javascript\' > enableBackButton='" + Convert.ToBoolean(Session["enableBackButton"]) + "';" + " enableForwardButton='" + Convert.ToBoolean(Session["enableForwardButton"]) + "'; var fromHyperLink='" + isTstFromHyperLink + "';var isRapidLoad='" + false + "';var defaultDepFlds='" + defaultDepFlds + "';</script>";
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", true);
        }
    }

    #endregion


    #region formload functions

    private void GetBreadCrumb()
    {
        try
        {
            Session["menubreadcrumb"] = string.Empty;
            //string strv = Session["MenuData"].ToString();
            string strv = string.Empty;
            try
            {
                string fdKeyMenuData = Constants.REDISMENUDATA;
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj != null)
                    strv = fObj.StringFromRedis(util.GetRedisServerkey(fdKeyMenuData, "Menu"), schemaName);
            }
            catch (Exception) { }

            if (strv == string.Empty && Session["MenuData"] != null)
                strv = Session["MenuData"].ToString();

            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.LoadXml(strv);
            string code = "tstruct.aspx?transid=" + transId;
            XmlNode node = null;
            XmlElement basicDataRoot = xmlDoc1.DocumentElement;
            if (basicDataRoot.SelectSingleNode("descendant::child[@target='" + code + "']") != null)
            {
                node = basicDataRoot.SelectSingleNode("descendant::child[@target='" + code + "']");
            }
            else if (basicDataRoot.SelectSingleNode("descendant::parent[@target='" + code + "']") != null)
            {
                node = basicDataRoot.SelectSingleNode("descendant::parent[@target='" + code + "']");
            }

            if (node != null)
            {
                string s = node.Attributes["name"].Value;
                string s1 = string.Empty;
                XmlNode parnode = default(XmlNode);
                if (node.ParentNode != null)
                {
                    parnode = node.ParentNode;
                    while (parnode.Name != "root")
                    {
                        s1 = parnode.Attributes["name"].Value + " > " + s1;
                        parnode = parnode.ParentNode;
                    }
                }
                Session["menubreadcrumb"] = s1;
                menuBreadCrumb = HttpContext.Current.Session["menubreadcrumb"].ToString();
            }
            else
            {
                Session["menubreadcrumb"] = string.Empty;
                menuBreadCrumb = HttpContext.Current.Session["menubreadcrumb"].ToString();
            }
        }
        catch (Exception)
        {
            SessionExpired();
            return;
        }
    }

    #region ConstructTstruct
    /// <summary>
    /// Function to call all formload related functions
    /// </summary>
    private void ConstructTstruct()
    {
        // To set the values for the global variables like transid,sessionid, user etc..
        SetGlobalVariables();
        if (!isTstPop && goval.Value == "" && !AxSplit)
            util.DeleteTstIvObject(transId);
        //Check desing access 
        CheckDesignAccess();
        stTime = DateTime.Now;
        //Design json and Config string will be retrieved from direct db instead of node js
        Boolean designMode = false;
        if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
        {
            designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
            ClearCacheDesignKeys(transId);
        }
        GetDesignModeData();
        //GetStructDesignAndConfig(); //Design & Config data merged into structure XML(service call), so this direct db call not required for run mode. 
        CacheManager cacheMgr = GetCacheObject();
        strObj = GetStrObject(cacheMgr);
        GetStructConfig(strObj);

        if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
            isMobileView = true;
        else
            isMobileView = false;

        customObj = Custom.Instance;

        if (goval.Value == "go")
        {
            if (lvPage.SelectedValue != "")
                callWebservice(lvPage.SelectedValue);
        }
        else
        {
            if (Session["StructureHtml"] != null)
                Session.Remove("StructureHtml");
            if (Session["ToolbarBtnIcons"] != null)
                Session.Remove("ToolbarBtnIcons");
            if (Session["ModernToolbarBtnOpen"] != null)
                Session.Remove("ModernToolbarBtnOpen");
            // To write the tstruct details to the client.
            WriteGlobalVariables();
            // to get the language from login page
            GetLanguage();

            if (strObj == null)
                return;
            structXml = strObj.structRes;
            edTime = DateTime.Now;
            strLogTime.Append("GetStructure-" + stTime.Subtract(edTime).TotalMilliseconds.ToString());
            formLogTime = float.Parse(edTime.Subtract(stTime).TotalMilliseconds.ToString());
            //logobj.CreateLog("Get Structure- timetaken-" + stTime.Subtract(edTime).TotalMilliseconds.ToString(), sid, "LogTimeTaken", "");
            try
            {
                SaveID.Value = strObj.save_id;
                PublishID.Value = strObj.Publish_id;
                IsPublish.Value = strObj.Is_Publish;
            }
            catch (Exception ex) { }

            parseXMLDoc(strObj.pdfList);

            WriteTstJsArrayDef(strObj);

            stTime = DateTime.Now;
            //if (strObj.IsObjFromCache )
            //{
            //    getDesignedData(strObj);
            //}
            try
            {
                // To get structure details from the object
                GetStructureDetails(strObj, cacheMgr);
            }
            catch (Exception ex)
            {
                Response.Redirect(util.ERRPATH + "Clear in-memory keys and try again!");
                return;
            }

            if (strObj.axdesignJObject != null)
            {
                if (Session["Axp_DesignJson"] != null && Session["Axp_DesignJson"].ToString() != "" && designMode)
                {
                    designHidden.Value = Session["Axp_DesignJson"].ToString();
                }
                else
                {
                    Session.Remove("Axp_DesignJson");
                    designHidden.Value = "[" + JsonConvert.SerializeObject(cacheMgr.axdesignJObcmg) + "]";
                }
                if ((strObj.axdesignJObject.wizardDC == true && strObj.dcs.Count > 1) || (Session["MobileView"] != null && Session["MobileView"].ToString() == "True" && strObj.dcs.Count > 1 && mobileCardLayout == "none"))
                    strObj.isWizardTstruct = true;

                getFormLayoutClass();
            }

            // To fill search dropdown list
            FillSearchList(strObj);
            edTime = DateTime.Now;
            strLogTime.Append("Constructing Tstrct/FromCache-" + stTime.Subtract(edTime).TotalMilliseconds.ToString());
            // To write the jsondata to the client  
            stTime = DateTime.Now;

            GetAxRulesDef(strObj, transId);

            GetAxRulesDefBtnConMsg(transId);

            LoadStructure(strObj);

            string AxvalErrorcode = util.GetAxvalErrorcode(transId);
            if (AxvalErrorcode != "")
            {
                string AxvalErrorcode_Scripts = "<script language='javascript' type='text/javascript' >var AxvalErrorCodes='" + AxvalErrorcode + "'</script>";
                tstScript.Append(AxvalErrorcode_Scripts);
            }

            strGlobalVar = util.GetGlobalVarString();
            if (strGlobalVar != string.Empty)
            {
                string global_Scripts = "<script language='javascript' type='text/javascript' >" + strGlobalVar + "</script>";
                tstScript.Append(global_Scripts);
            }

            if (strObj.isWizardTstruct)
                wizardClass = "wizardTstruct";

            if (rid != "0" && Request.QueryString["recPos"] != null && Request.QueryString["recPos"] != "" && Request.QueryString["recPos"] != "null" && Request.QueryString["recordid"] != null)
                wizardLoadRecord = "true";

            edTime = DateTime.Now;
            strLogTime.Append("LoadStructure-" + stTime.Subtract(edTime).TotalMilliseconds.ToString());
        }
    }

    private void getFormLayoutClass()
    {
        var formWidth = "";
        var formPosition = "";
        try
        {
            if (strObj.axdesignJObject != null)
            {
                string dcLayout = strObj.axdesignJObject.dcLayout != "default" ? strObj.axdesignJObject.dcLayout : "";
                switch (strObj.axdesignJObject.formWidth)
                {
                    case "50":
                        formWidth = "col-md-6 formLayoutWidth";
                        break;
                    case "60":
                        formWidth = "col-md-8 formLayoutWidth";
                        break;
                    case "70":
                        formWidth = "col-md-9 formLayoutWidth";
                        break;
                    case "80":
                        formWidth = "col-md-10 formLayoutWidth";
                        break;
                    case "90":
                        formWidth = "col-md-11 formLayoutWidth";
                        break;
                    default:
                        formWidth = "";
                        break;
                }

                switch (strObj.axdesignJObject.formAlignment)
                {
                    case "center":
                        formPosition = "mx-auto";
                        break;
                    case "left":
                        formPosition = "float-start";
                        break;
                    case "right":
                        formPosition = "float-end";
                        break;
                    default:
                        formPosition = "";
                        break;
                }
                formLayoutClass = formWidth + " " + dcLayout + " " + formPosition;
            }
        }
        catch (Exception)
        { }
    }

    public void CheckDraftPopup(string _thistrId)
    {
        if (AutosaveDraftt == "true")
        {
            var redisDraftkey = _thistrId + "-" + HttpContext.Current.Session["user"].ToString() + Constants.UNIQUE_DRAFT_KEY_PHRASE + tstCaption;
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            if (schemaName == "" && HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            var redisDraftkeys = fdrObj.GetAllKeys(schemaName + "-" + redisDraftkey);
            if (redisDraftkeys.Count > 0)
            {
                savedraftKeyCreatedtime = redisDraftkeys[0].ToString().Split('-').Last();
                var Createdyear = savedraftKeyCreatedtime.Substring(0, 8);
                var day = Createdyear.Substring(0, 2);
                var month = Createdyear.Substring(2, 2);
                var year = Createdyear.Substring(4, 4);
                var rkeycompareYear = day + "-" + month + "-" + year;
                var rKeyCompareTime = savedraftKeyCreatedtime.Substring(8, 4);
                var hours = rKeyCompareTime.Substring(0, 2);
                var mins = rKeyCompareTime.Substring(2, 2);
                rKeyCompareTime = hours + ":" + mins;
                savedraftKeyCreatedtime = rkeycompareYear + " " + rKeyCompareTime;
                HttpContext.Current.Session["savedraftKeyCreatedtime"] = savedraftKeyCreatedtime;

            }
        }

    }

    private void GetDesignModeData()
    {
        try
        {
            Boolean designMode = false;
            if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
                designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
            if (designMode)
            {
                DataSet dsDesign = new DataSet();
                DataTable dtDesignData = new DataTable();
                DBContext objdb = new DBContext();
                dsDesign = objdb.GetAxConfigurations(transId, Constants.STRUCTTYPE_TSTRUCT, designMode, Constants.CONFIGTYPE_DESIGN);
                dtDesignData = dsDesign.Tables["Table0"];
                if (dtDesignData.Rows.Count > 0)
                {
                    var newDesign = dtDesignData.AsEnumerable().Select(x => new { newdesignJson = x.Field<string>("CONTENT") }).ToList();
                    designJson = newDesign[0].newdesignJson;
                }
                Session["Axp_DesignJson"] = designJson;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("GetDesignModeData -" + ex.Message, sessID, "GetDesignModeData", "new");
        }
    }
    private void GetStructConfig(TStructDef strObj)
    {
        try
        {
            string axpStructKey = Constants.AXCONFIGTSTRUCT;
            if (HttpContext.Current.Session["AxDtConfigs"] != null)
            {
                axpConfigStr = (DataTable)HttpContext.Current.Session["AxDtConfigs"];
                HttpContext.Current.Session.Remove("AxDtConfigs");
            }
            if (axpConfigStr != null && axpConfigStr.Rows.Count > 0)
                SetAxpStructConfigProps(strObj);
            else
            {
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                axpConfigStr = fObj.DataTableFromRedis(util.GetConfigCacheKey(axpStructKey, transId, "", AxRole, "ALL"));
                SetAxpStructConfigProps(strObj);
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("GetStructConfig -" + ex.Message, sessID, "GetStructConfig", "new", "true");
        }
    }

    private void SetAxpStructConfigProps(TStructDef strObj)
    {
        try
        {
            string Displayformgridintbl = string.Empty;
            if (axpConfigStr != null && axpConfigStr.Rows.Count > 0)
            {
                var lstMultiFlds = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "multi select" && x.Field<string>("PROPSVAL").ToLower() == "true")
                 .Select(x => x.Field<string>("SFIELD")).ToList();
                if (lstMultiFlds.Count > 0)
                    Session["AxpFldMultiSelect"] = lstMultiFlds;
                var lstMultiFldsp = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "multi select" && x.Field<string>("PROPSVAL").ToLower() == "true")
                .Select(x => x.Field<string>("SFIELD") + "♦" + x.Field<string>("PROPVALUE2")).ToList();
                if (lstMultiFldsp.Count > 0)
                    Session["AxpFldMultiSelSp"] = lstMultiFldsp;

                var strAutoSPlit = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "autosplit")
                   .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (strAutoSPlit.Count > 0)
                    AxpIsAutoSplit = strAutoSPlit[0].splitVal;

                var strDisSPlit = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "disablesplit")
                   .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (strDisSPlit.Count > 0)
                    AxpIviewDisableSplit = strDisSPlit[0].splitVal;

                var fldAlign = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "text")
                  .Select(x => new { fldName = x.Field<string>("SFIELD"), alignProp = x.Field<string>("PROPSVAL") }).ToList();

                var autoCompPatt = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "autocomplete search pattern")
                 .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (autoCompPatt.Count > 0)
                {
                    Session["AxpautoCompPatt"] = autoCompPatt[0].splitVal;
                    DropdownSearchStartsWith = autoCompPatt[0].splitVal;
                }
                try
                {
                    strObj.WebServiceTimeout = Convert.ToInt32(axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "webservice timeout").Select(x => x.Field<string>("PROPSVAL")).FirstOrDefault());
                }
                catch (Exception ex) { };

                try
                {
                    googleMapsZoom = Convert.ToInt32(axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "google maps zoom").Select(x => x.Field<string>("PROPSVAL")).FirstOrDefault());
                }
                catch (Exception ex) { };

                var AutosaveDraft = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "auto save draft")
                   .Select(x => new { autosavedraft = x.Field<string>("PROPSVAL"), AutosaveDraftTime = x.Field<string>("PROPVALUE2") }).ToList();
                if (AutosaveDraft.Count > 0)
                {
                    AutosaveDraftt = AutosaveDraft[0].autosavedraft;
                    //AutosaveDraftTime = AutosaveDraft[0].AutosaveDraftTime == "" ? "120000" : AutosaveDraft[0].AutosaveDraftTime;
                    //Session["tstautosaveDraftt"] = AutosaveDraftt;
                }
                //else if (Session["tstautosaveDraftt"] != null)
                //{
                //    Session.Remove("tstautosaveDraftt");
                //}

                var UploadFileTypes = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "upload file types")
                   .Select(x => new { autosavedraft = x.Field<string>("PROPSVAL"), UploadFileTypesVal = x.Field<string>("PROPVALUE2") }).ToList();
                if (UploadFileTypes.Count > 0)
                {
                    UploadFileTypess = UploadFileTypes[0].autosavedraft;//UploadFileTypes[0].autosavedraft
                    UploadFileTypesVal = UploadFileTypes[0].UploadFileTypesVal;

                }
                var GridscrollBar = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "grid scrollbar")
                 .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (GridscrollBar.Count > 0)
                    gridScrollBar = GridscrollBar[0].splitVal;

                var GridFixedHeader = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "fixed header for grid")//Fixed Header for Grid
                 .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (GridFixedHeader.Count > 0)
                    gridFixedHeader = GridFixedHeader[0].splitVal;

                var strFileUpload = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "file upload limit")
                   .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (strFileUpload.Count > 0)
                    AxpFileUploadlmt = strFileUpload[0].splitVal;

                var strcameraopt = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "camera option")
                  .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (strcameraopt.Count > 0)
                    AxpCameraOption = strcameraopt[0].splitVal;

                var notfilldependentfields = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "not fill dependent fields" && x.Field<string>("PROPSVAL").ToLower() == "true").Select(x => x.Field<string>("SFIELD")).ToList();
                if (notfilldependentfields.Count > 0)
                    AxpNotFillDepFields = string.Join(",", notfilldependentfields);

                var filldependentfields = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "fill dependent fields" && x.Field<string>("PROPSVAL").ToLower() == "true").Select(x => x.Field<string>("SFIELD")).ToList();
                if (filldependentfields.Count > 0)
                {
                    AxpFillDepFields = string.Join(",", filldependentfields);
                    AxpFillDepEnabled = "true";
                }
                else
                    AxpFillDepEnabled = "false";

                var strSaveImagedb = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "saveimage")
                 .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (strSaveImagedb.Count > 0)
                    AxpSaveImageDb = strSaveImagedb[0].splitVal;
                if (transId == "a__na")
                    AxpSaveImageDb = "true";
                if (AxpSaveImageDb.ToLower() == "true")
                {
                    isAxpImagePath = "false";
                    isAxpImagePathHidden.Value = isAxpImagePath;
                }
                Session["AxpSaveImageDb"] = AxpSaveImageDb;

                var ExcelExport = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "excel export")
                    .Select(x => new { excel = x.Field<string>("PROPSVAL") }).ToList();
                if (ExcelExport.Count > 0)
                    Session["AxpExcelExport"] = ExcelExport[0].excel;
                else
                    Session["AxpExcelExport"] = null;

                // Enable mobile-ui for tsruct grid options - all/mobile/none
                var applyMobileUI = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "apply mobile ui").Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (applyMobileUI.Count > 0)
                    mobileCardLayout = applyMobileUI[0].splitVal;
                else if ((Session["MobileView"] != null && Session["MobileView"].ToString() == "True"))
                {
                    mobileCardLayout = "mobile";
                }


                JavaScriptSerializer ser = new JavaScriptSerializer();
                hdnFldAlgnProp.Value = ser.Serialize(fldAlign);

                //Tstruct grid edit option - popup/inline
                var gridEditOption = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "gridedit")
                 .Select(x => new { gridtype = x.Field<string>("PROPSVAL"), formCols = x.Field<string>("PROPVALUE2") }).ToList();

                if (gridEditOption.Count > 0)
                {
                    Session["AxInlineGridEdit"] = gridEditOption[0].gridtype != "inline" ? "false" : "true";
                    AxpGridFormCols = gridEditOption[0].gridtype + "♠" + gridEditOption[0].formCols;
                }
                else if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
                    Session["AxInlineGridEdit"] = "false";
                else
                    Session["AxInlineGridEdit"] = "true";

                if (transId == "sect")
                {
                    Session["AxInlineGridEdit"] = "false";
                    AxpGridFormCols = "form" + "♠";
                }

                var loadCustomJs = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "custom javascript" && x.Field<string>("PROPSVAL").ToLower() == "true")
                                   .Select(x => new { applyon = x.Field<string>("APPLYON") }).ToList();
                if (loadCustomJs.Count > 0)
                    AxpCustomJs = loadCustomJs[0].applyon;

                var loadCustomCss = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "custom css" && x.Field<string>("PROPSVAL").ToLower() == "true")
                   .Select(x => new { applyon = x.Field<string>("APPLYON") }).ToList();
                if (loadCustomCss.Count > 0)
                    AxpCustomCss = loadCustomCss[0].applyon;

                var buttonstyle = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "tstruct button style").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (buttonstyle.Count > 0)
                    AxpTstButtonStyle = buttonstyle[0].val.ToLower();

                var controlBorder = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "show control borders").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (controlBorder.Count > 0)
                    AxShowControlBorders = controlBorder[0].val.ToLower();

                var PickListFetchSize = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "dorp down fetch size").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (PickListFetchSize.Count > 0)
                {
                    FetchPickListRows = PickListFetchSize[0].val == "" ? 1000 : int.Parse(PickListFetchSize[0].val);
                    Session["PickListFetchSize"] = FetchPickListRows;
                }
                else if (Session["PickListFetchSize"] != null)
                    Session.Remove("PickListFetchSize");

                var strFastReportInTab = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "open fast report in new window")
                 .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (strFastReportInTab.Count > 0)
                {
                    openFastReportInNewWindow = strFastReportInTab[0].splitVal.ToString().ToLower() == "true" ? true : false;
                }

                var fillgridOrder = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "popup fillgrid data based on query order")
               .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (fillgridOrder.Count > 0)
                    fillGridDataQueryOrder = fillgridOrder[0].splitVal.ToString().ToLower() == "true" ? true : false;

                var fillgridShowAll = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "popup fillgrid data show all")
               .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (fillgridShowAll.Count > 0)
                    fillGridDataShowAll = fillgridShowAll[0].splitVal.ToString().ToLower() == "true" ? true : false;

                var forceFieldVal = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "force field validation" && x.Field<string>("PROPSVAL").ToLower() == "true").Select(x => x.Field<string>("PROPVALUE2")).ToList();
                if (forceFieldVal.Count > 0)
                    AxpForceValidation = forceFieldVal[0];//string.Join(",", forceFieldVal);

                var showSqlColumnsAdvSearch = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "show sql columns in advance search in field level" && x.Field<string>("PROPSVAL").ToLower() == "true").Select(x => x.Field<string>("PROPVALUE2")).ToList();
                if (showSqlColumnsAdvSearch.Count > 0)
                    Session["showSqlColumnsAdvSearch"] = showSqlColumnsAdvSearch[0] == "" ? "All" : showSqlColumnsAdvSearch[0];
                else
                    Session.Remove("showSqlColumnsAdvSearch");

                var fillautoselect = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "fill autoselect fields for dropdown fields").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (fillautoselect.Count > 0)
                    AxFillAutoSelectFlds = fillautoselect[0].val;

                var enablecheckbox = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "enable checkbox instead of switchbox in forms").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (enablecheckbox.Count > 0)
                {
                    Session["switchboxToCheckbox"] = AxEnableCheckbox = enablecheckbox[0].val;
                }
                else
                    Session.Remove("switchboxToCheckbox");

                var custTranSaveSuccess = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "customized popups for transaction save success").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (custTranSaveSuccess.Count > 0)
                    AxCustomTranSaveSuccess = custTranSaveSuccess[0].val;

                var tstAttachFileserveDB = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "tstruct attachments stores in file server as well as db").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (tstAttachFileserveDB.Count > 0)
                {
                    AxtstAttachFSDB = tstAttachFileserveDB[0].val;
                    Session["AxtstAFSDB"] = AxtstAttachFSDB;
                }
                else
                {
                    Session["AxtstAFSDB"] = "false";
                }

                var hideselectall = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "hide select all option in multiselect component").Select(x => new { val = x.Field<string>("PROPSVAL"), sfld = x.Field<string>("SFIELD") }).ToList();
                if (hideselectall.Count > 0)
                    AxHideSelectAll = hideselectall[0].val + "♦" + hideselectall[0].sfld;

                var workflowComments = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "mandatory workflow comments").Select(x => new { val = x.Field<string>("PROPSVAL"), sfld = x.Field<string>("SFIELD") }).ToList();
                if (workflowComments.Count > 0)
                    AxMandatoryWfComments = workflowComments[0].val;

                var TstPopupClose = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "tstructs popup close on submit").Select(x => new { val = x.Field<string>("PROPSVAL"), sfld = x.Field<string>("SFIELD") }).ToList();
                if (TstPopupClose.Count > 0)
                    TstPopupCloseOnSubmit = TstPopupClose[0].val;

                var TstAllowVideoAtta = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "allow video file in attachments").Select(x => new { val = x.Field<string>("PROPSVAL"), alfiles = x.Field<string>("PROPVALUE2") }).ToList();
                if (TstAllowVideoAtta.Count > 0 && TstAllowVideoAtta[0].val != "" && TstAllowVideoAtta[0].val.ToLower() == "true")
                    Session["TstAllowVideoAtta-" + transId] = TstAllowVideoAtta[0].alfiles;
                else
                    Session.Remove("TstAllowVideoAtta-" + transId);

                var fileprotected = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "file protected in tstruct attachments").Select(x => new { val = x.Field<string>("PROPSVAL"), sfld = x.Field<string>("SFIELD") }).ToList();
                if (fileprotected.Count > 0)
                    tstIsFileProtected = fileprotected[0].val;

                var formdatavalidateworkflowaction = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "form data validate on workflow action").Select(x => new { val = x.Field<string>("PROPSVAL"), sfld = x.Field<string>("SFIELD") }).ToList();
                if (formdatavalidateworkflowaction.Count > 0 && formdatavalidateworkflowaction[0].val != "" && formdatavalidateworkflowaction[0].val.ToLower() == "true")
                    Session["TstDataValidateWFAction-" + transId] = "true";
                else
                    Session.Remove("TstDataValidateWFAction-" + transId);

                var Optionavoidmultipledraft = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "option to avoid multiple draft per a transaction").Select(x => new { val = x.Field<string>("PROPSVAL"), sfld = x.Field<string>("SFIELD") }).ToList();
                if (Optionavoidmultipledraft.Count > 0 && Optionavoidmultipledraft[0].val != "" && Optionavoidmultipledraft[0].val.ToLower() == "true")
                    Session["TstAvoidmultipledraft-" + transId] = "true";
                else
                    Session.Remove("TstAvoidmultipledraft-" + transId);

                if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
                {
                    var _Displayformgridintbl = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "display form grids in tabular format for mobile").Select(x => new { val = x.Field<string>("PROPSVAL"), sfld = x.Field<string>("SFIELD") }).ToList();
                    if (_Displayformgridintbl.Count > 0 && _Displayformgridintbl[0].val != "" && _Displayformgridintbl[0].val.ToLower() == "true")
                    {
                        Displayformgridintbl = "true";
                        Session["AxInlineGridEdit"] = "true";
                        AxpGridFormCols = "inline♠";
                    }
                }

                try
                {
                    DataSet ds = new DataSet();
                    ds.Merge(axpConfigStr.Copy());
                    ds.DataSetName = "configurations";
                    ds.Tables[0].TableName = "config";
                    StructConfigs = JObject.Parse(JsonConvert.SerializeObject(ds));
                }
                catch (Exception ex) { }
            }
            else
            {
                if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
                    Session["AxInlineGridEdit"] = "false";
                else
                    Session["AxInlineGridEdit"] = "true";
                Session["AxpSaveImageDb"] = null;
                Session["AxpautoCompPatt"] = null;
                Session["AxpExcelExport"] = null;
                Session["AxpFldMultiSelect"] = null;
                Session["AxpFldMultiSelSp"] = null;
                if (strObj.AxpTstButtonStyleDef != null && strObj.AxpTstButtonStyleDef != AxpTstButtonStyle)
                    AxpTstButtonStyle = strObj.AxpTstButtonStyleDef;
            }
            if ((Session["MobileView"] != null && Session["MobileView"].ToString() == "True"))
            {
                if (Displayformgridintbl == "true")
                {
                    Session["AxInlineGridEdit"] = "true";
                    AxpGridFormCols = "inline♠";
                }
                else if (mobileCardLayout != "none")
                {
                    Session["AxInlineGridEdit"] = "false";
                    AxpGridFormCols = "form♠";
                }
                AxpIsAutoSplit = "false";
            }
        }
        catch (Exception ex)
        {
            if (strObj.AxpTstButtonStyleDef != null && strObj.AxpTstButtonStyleDef != AxpTstButtonStyle)
                AxpTstButtonStyle = strObj.AxpTstButtonStyleDef;

            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("SetAxpStructConfigProps -" + ex.Message, sessID, "SetAxpStructConfigProps", "new", "true");
        }
    }


    #endregion

    #region SetGlobalVariables
    /// <summary>
    /// Function to Set the Global variables like transid, user, role etc...
    /// </summary>
    private void SetGlobalVariables()
    {
        if (!IsPostBack)
        {
            GetWorkflowGlobalVars();

            hdnAxIsPerfCode.Value = Session["AxIsPerfCode"].ToString();

            proj = Session["project"].ToString();
            //ViewState["proj"] = proj;
            user = Session["user"].ToString();
            //ViewState["user"] = user;
            sid = Session["nsessionid"].ToString();
            //ViewState["sid"] = sid;
            AxRole = Session["AxRole"].ToString();
            //ViewState["AxRole"] = AxRole;
            language = Session["language"].ToString();
            //ViewState["language"] = language;
            transId = Request.QueryString["transid"].ToString();
            if (!util.IsTransIdValid(transId))
                Response.Redirect(Constants.PARAMERR);
            Session.Add("transid", transId);
            //ViewState["tid"] = transId;
            fileName = "opentstruct-" + transId;
            errorLog = logobj.CreateLog("Loading Structure.", sid, fileName, "new");

            if ((!string.IsNullOrEmpty(Request.QueryString["recordid"])))
            {
                rid = Request.QueryString["recordid"];
                rid = CheckSpecialChars(rid);
                //ViewState["rid"] = rid;
            }
            else
            {
                rid = "0";
            }
            if (Request.QueryString.Count < 2 && (Session["lstRecordIds"] != null || Session["recordTransId"] != null || Session["navigationInfoTable"] != null) || Request.QueryString.ToString().Contains("axpdraftid"))
                util.ClearSession();

            //remove key from querystring
            if (Request.QueryString["AxIsPop"] != null)
            {
                System.Reflection.PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                isreadonly.SetValue(this.Request.QueryString, false, null);
                this.Request.QueryString.Remove("AxIsPop");
            }
            //The AxPop is used to identify if the tstruct is opened as pop up and hide navigate buttons
            if (Request.QueryString["AxPop"] != null)
                isTstPop = Convert.ToBoolean(Request.QueryString["AxPop"].ToString());

            if (Request.QueryString["AxHypTstRefresh"] != null)
                Session["AxHypTstRefresh"] = Request.QueryString["AxHypTstRefresh"].ToString();

            if (Session["AxLogging"] != null)
                AxLogTimeTaken = Session["AxLogging"].ToString().ToLower();

        }
        else
        {
            proj = Session["project"].ToString();
            sid = Session["nsessionid"].ToString();
            user = Session["user"].ToString();
            if (Request.QueryString["transid"] != null && Request.QueryString["transid"].ToString() != "")
                transId = Request.QueryString["transid"].ToString();
            else
                transId = Session["transid"].ToString();
            AxRole = Session["AxRole"].ToString();
            language = Session["language"].ToString();
        }
        if ((Request.QueryString["act"] != null))
        {
            actstr = " act='" + Request.QueryString["act"].ToString().ToLower() + "'";
            actstrType = Request.QueryString["act"].ToString().ToLower();
        }
        if (actstr == string.Empty && (Request.QueryString["hltype"] != null))
        {
            actstr = " act='" + Request.QueryString["hltype"].ToString().ToLower() + "'";
            actstrType = Request.QueryString["hltype"].ToString().ToLower();
        }

        if ((Request.QueryString["loadformdata"] != null))
        {
            formFoadData = Request.QueryString["loadformdata"].ToString();
            if (formFoadData != string.Empty)
                formFoadData = " loadformdata='" + formFoadData + "' ";
        }
        if (Request.QueryString["trromode"] != null)
        {
            transactionROMode = Request.QueryString["trromode"].ToString();
        }

        if (Request.QueryString["AxSplit"] != null)
        {
            AxSplit = Request.QueryString["AxSplit"].ToString() == "true" ? true : false;
        }

        try
        {
            if (Request.QueryString["openerIV"] != null)
            {
                if (Request.QueryString["isIV"] != null && Request.QueryString["isIV"].ToString() == "true")
                    Session["openerIV"] = Request.QueryString["openerIV"].ToString() + "~IV";
                else if (Request.QueryString["isIV"] != null && Request.QueryString["isIV"].ToString() == "false")
                    Session["openerIV"] = Request.QueryString["openerIV"].ToString() + "~LV";
            }
            else
            {
                Session.Remove("openerIV");
            }
        }
        catch (Exception ex) { }

        try
        {
            if (Request.QueryString["isDupTab"] != null)
            {
                Session["isDupTab"] = Request.QueryString["isDupTab"].ToString();
            }
            else
            {
                Session["isDupTab"] = "false";
            }
        }
        catch (Exception ex) { }

        try
        {
            if (Request.QueryString["FromProcess"] != null)
            {
                Session["IsFromProcess"] = Request.QueryString["FromProcess"].ToString();
            }
            else
            {
                Session["IsFromProcess"] = "false";
            }
        }
        catch (Exception ex) { }

        try
        {
            if (Request.QueryString["ispegedit"] != null)
                isPegEdit = Request.QueryString["ispegedit"].ToString();
            else
                isPegEdit = "false";
        }
        catch (Exception ex) { }

        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();
    }

    private void GetWorkflowGlobalVars()
    {
        if (Application["AxOnApproveDisable"].ToString().ToLower() == "true")
            AxOnApproveDisable = "true";
        if (Application["AxOnRejectDisable"].ToString().ToLower() == "true")
            AxOnRejectDisable = "true";
        if (Application["AxOnReturnSave"].ToString().ToLower() == "true")
            AxOnReturnSave = "true";
        if (Application["AxOnRejectSave"].ToString().ToLower() == "true")
            AxOnRejectSave = "true";
    }

    private void SetSessionVariables()
    {
        if (Session["AxDisplayAutoGenVal"] != null)
            hdnShowAutoGenFldValue.Value = Session["AxDisplayAutoGenVal"].ToString();

        if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
            axWizardType = "modern";
        else if (Session["AxWizardType"] != null)
            axWizardType = Session["AxWizardType"].ToString();

        if (Session["AxDrafts"] != null)
            tstDraftsScript = Session["AxDrafts"].ToString();

        if (!String.IsNullOrEmpty(Session["AxExportTallyTid"].ToString()))
            exportTallyTid = Session["AxExportTallyTid"].ToString().ToLower();
        if (Session["AxIsPerfCode"] != null && Session["AxIsPerfCode"].ToString().ToLower() == "true")
            isRapidLoad = true;

        traceLog = logobj.CreateLog("AsbDefineRest-savetstruct", Session.SessionID.ToString(), "tstruct-add-field", "new");
        traceLog = traceLog.Replace("\\", "\\\\");
        //If a Tstruct is opened from HypLink, then, it should not be loaded from In Mem during Rapid Load.
        if (HttpContext.Current.Session["AxFromHypLink"] != null && HttpContext.Current.Session["AxFromHypLink"].ToString().ToUpper() == "TRUE")
            loadFromMem = false;

        //If a Tstruct is opened from Actions, then, it should not be loaded from In Mem during Rapid Load.
        if (Request.QueryString["IsFrmAct"] != null && Request.QueryString["IsFrmAct"].ToString().ToUpper() == "TRUE")
            loadFromMem = false;

        if (Request.QueryString["theMode"] != null && Request.QueryString["theMode"].ToString().ToLower() == "design")
            theModeDesign = true;
        if (theModeDesign)
            HttpContext.Current.Session[Request.QueryString["transid"] + "IsDesignMode"] = theModeDesign;
        else
            HttpContext.Current.Session.Remove(Request.QueryString["transid"] + "IsDesignMode");

        //AutoSave and publish
        if (theModeDesign == true && Session["AutoSavePublish"] != null)
        {
            if (Session["AutoSavePublish"].ToString().ToLower() == "true")
            {
                saveDesign.Visible = false;
                PublishDesign.Visible = false;
                PublishSaveDesign.Visible = true;
            }
        }
        string scriptsUrlPath = Application["ScriptsurlPath"].ToString();
        if (HttpContext.Current.Session["AxDraftSavePath"] != null)
            draftsPath = HttpContext.Current.Session["AxDraftSavePath"].ToString() + "axpert\\drafts\\";
        hdnScriptsUrlpath.Value = scriptsUrlPath;
        hdnScriptspath.Value = scriptsUrlPath;

        if (Session["AppSessionKey"] != null)
            appsessionKey = Session["AppSessionKey"].ToString();

        //if (ConfigurationManager.AppSettings["FetchPickListRows"] != null && ConfigurationManager.AppSettings["FetchPickListRows"].ToString() != "")
        //    FetchPickListRows = int.Parse(ConfigurationManager.AppSettings["FetchPickListRows"].ToString());
        langauge = Session["language"].ToString();

        if (HttpContext.Current.Session["AxImagePath"] != null && HttpContext.Current.Session["AxImagePath"].ToString() != "")
            isAxpImagePath = "true";
        if (HttpContext.Current.Session["AxpImagePathGbl"] != null && HttpContext.Current.Session["AxpImagePathGbl"].ToString() != "")
            isAxpImagePath = "true";
        if (AxpSaveImageDb.ToLower() == "true")
            isAxpImagePath = "false";

        isAxpImagePathHidden.Value = isAxpImagePath;
    }

    #endregion

    #region WriteGlobalVariables
    /// <summary>
    /// Function to write the Global variables info to the javascript.
    /// </summary>
    private void WriteGlobalVariables()
    {
        tstVars.Append("<script type='text/javascript'>");
        tstVars.Append("function GetFormDetails() { var a = '" + proj + "';var b='" + user + "';var c='" + transId + "';var d='" + sid + "';var e = '" + AxRole + "';var f='" + Session["AxTrace"] + "';SetTstProps(a,b,c,d,e,f);}");
        tstVars.Append("</script>");
    }

    private void GetLanguage()
    {
        getLang.Append("<script type='text/javascript'>");
        getLang.Append("function Getlanguage() { var l = '" + language + "';SetLangProps(l);}");
        getLang.Append("</script>");
    }

    private void WriteTstJsArrayDef(TStructDef strObj)
    {
        tstJsArrays.Append("<script type='text/javascript'>");
        if (strObj.tstHyperLink)
        {
            tstJsArrays.Append("var HLinkPop = new Array();var HLinkName = new Array();var HLinkSource = new Array();var HLinkLoad = new Array();var HLinkParamName = new Array();var HLinkParamValue = new Array();");
        }
        if (strObj.popdcs.Count > 0)
        {
            tstJsArrays.Append("TstructHasPop = true; var PopParentDCs = new Array();var PopParentFlds = new Array();var PopSqlFill = new Array();var PopSummaryParent = new Array();");
            tstJsArrays.Append("var PopSummaryFld = new Array();var PopSummDelimiter = new Array();var PopGridDCs = new Array();var PopGridDCFirm = new Array();");
            tstJsArrays.Append("var ParentDcNo = new Array();var ParentClientRow = new Array();var PopGridDcNo = new Array();var PopCondition = new Array();");
            tstJsArrays.Append("var PopParentsStr = new Array();var PopRows = new Array();");
        }
        if (strObj.dcs.Count > 0)
        {
            tstJsArrays.Append("var DCName = new Array();var DCCaption = new Array();var DCFrameNo = new Array();var DCIsGrid = new Array();var DCIsPopGrid = new Array();var DCHasDataRows = new Array();var DCAllowEmpty = new Array();var DCAllowChange= new Array();");
            tstJsArrays.Append("var DcIsFormatGrid = new Array();var DcKeyColumns = new Array(); var DcSubTotCols = new Array();var DcKeyColValues = new Array();var DcMultiSelect = new Array();var DcAllowAdd = new Array();var DcAcceptMRFlds = new Array();");
        }

        tstJsArrays.Append("var TabDCs = new Array();var TabDCStatus = new Array();var TabDCAlignmentStatus = new Array();var PagePositions = new Array();");

        if (strObj.flds.Count > 0)
        {
            tstJsArrays.Append("var FNames = new Array();var FldsFrmLst = new Array();var ExprPosArray= new Array();var FLowerNames = new Array();var FToolTip = new Array();var FDataType = new Array();var FTableTypeVal = new Array();");
            tstJsArrays.Append("var FMaxLength = new Array();var FDecimal = new Array();var FDupDecimals=new Array(); var FldValidateExpr = new Array();var FCaption = new Array();var HTMLFldNames = new Array();var FCustDecimal=new Array();var FNumDisplayTot=new Array();");
            tstJsArrays.Append("var FldFrameNo = new Array();var FldDcRange = new Array();var FProps = new Array(); var ExpFldNames = new Array();");
            tstJsArrays.Append("var Expressions = new Array();var Formcontrols = new Array();var tstActionName=new Array();var tstActionCaption=new Array();var actParRefresh=new Array();var actSaveTask=new Array();var PatternNames = new Array();var Patterns = new Array();var SFormControls=new Array();var SFCFldNames=new Array();var SFCApply=new Array();var SFCActionName=new Array();var actScriptTask=new Array();var actScriptCancel=new Array();var actScriptActive=new Array();var actScriptPushtoQueue=new Array();var SFCIsActive=new Array();var actScriptQueueName=new Array();");
            tstJsArrays.Append("var FMoe = new Array();var FldDependents = new Array();var FldParents = new Array();var ClientFldParents = new Array();var FldAutoSelect = new Array();var FldIsSql = new Array();var FldAlignType = new Array();var FldIsAPI = new Array();var FldDSqlParams=new Array();");
            tstJsArrays.Append("var FldRapidDeps = new Array();var FldRapidDepType = new Array();var FldRapidExpDeps = new Array();var FldRapidParents = new Array();var FldPurpose= new Array();var FSetCarry=new Array();");
        }
        //Add dependency arrays 
        tstJsArrays.Append("var DArray = new Array();var PArray = new Array();var CArray = new Array();var FldChkSeparator = new Array();var FldMgsSeparator=new Array();var FFieldType=new Array();;var FFieldHidden=new Array();var FFieldAcceptApi=new Array();var FFieldIntelli=new Array();var FldMaskType=new Array();var FldMaskDetails=new Array();var FFieldReadOnly=new Array();var FldFillFromAPI=new Array();");
        //add general arrays
        tstJsArrays.Append("var Parameters = new Array();var VisibleDCs = new Array();var FillAutoShow = new Array();var FillMultiSelect = new Array();var FillParamFld = new Array();var FillParamDCs = new Array();var FillCondition = new Array();var FillSourceDc = new Array();var FillGridName = new Array();var FillGridVExpr = new Array();var FillGridTotalFldVal = new Array();var FillGridExecOnSave = new Array();var FillGridCaption = new Array();var AxpFileUploadFields=new Array();var AxpListofRagVarflds=new Array();");

        //AxRulesDef Arrays
        tstJsArrays.Append("var AxRDCompType = new Array();var AxRDCompName = new Array();var AxRDValidation = new Array();var AxRDFilter = new Array();var AxRDFormControl = new Array();var AxRDComputeScript = new Array();var AxRDAllowDuplicate = new Array();var AxRDAllowEmpty = new Array();var AxRDIsApplicable=new Array();var AxRDRuleType=new Array();var AxMemParameters=new Array();var AxCdParameters=new Array();var AxRDFormControlParent=new Array();var AxRDScriptOnLoad=new Array();var AxRDBtnMsgName=new Array();var AxRDBtnMsgCond=new Array();var AxRDBtnMsgConMsg=new Array();var AxRDLevelNo=new Array();var AxRDPegRule=new Array();");

        tstJsArrays.Append("</script>");
    }
    #endregion

    #region GetCacheObject
    private CacheManager GetCacheObject()
    {
        CacheManager cacheMgr = null;

        try
        {
            cacheMgr = new CacheManager(errorLog);
        }
        catch (Exception ex)
        {
            requestProcess_logtime += "Server - " + ex.Message + " ♦ ";
            Response.Redirect(util.ERRPATH + ex.Message + "*♠*" + requestProcess_logtime);
        }

        if (cacheMgr == null)
        {
            requestProcess_logtime += "Server - Server error. Please try again later ♦ ";
            Response.Redirect(util.ERRPATH + "Server error. Please try again later" + "*♠*" + requestProcess_logtime);
        }
        return cacheMgr;
    }
    #endregion

    #region GetStrObject
    private TStructDef GetStrObject(CacheManager cacheMgr)
    {
        TStructDef strObj = null;
        // cachemanager and TStructDef objects throw exceptions
        try
        {
            string language = HttpContext.Current.Session["language"].ToString();
            strObj = cacheMgr.GetStructDef(proj, sid, user, transId, AxRole);
            isTstCustomHtml = strObj.IsObjCustomHtml;
            requestProcess_logtime += cacheMgr.requestProcess_log;
            ClearDcHasDataRows(strObj);
        }
        catch (Exception ex)
        {
            if (ex.Message.IndexOf("♠") > -1)
            {
                requestProcess_logtime += ex.Message.Split('♠')[0];
                requestProcess_logtime += ObjExecTr.ResponseErrorMsg("Server - " + ex.Message.Split('♠')[1]);
                if (ex.Message.Split('♠')[1] == Constants.SESSIONEXPMSG)
                {
                    Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG + "*♠*" + requestProcess_logtime);
                    return null;
                }
                else
                {
                    Response.Redirect(util.ERRPATH + ex.Message.Replace(Environment.NewLine, "").Split('♠')[1] + "*♠*" + requestProcess_logtime);
                }
            }
            else
            {
                if (ex.Message == Constants.SESSIONEXPMSG)
                {
                    Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG);
                    return null;
                }
                else if (ex.Message == "Object reference not set to an instance of an object.")
                {
                    Response.Redirect(util.ERRPATH + "Structure XML is not proper. Please resave the definition and retry.");
                }
                else
                {
                    Response.Redirect(util.ERRPATH + ex.Message.Replace(Environment.NewLine, ""));
                }
            }
        }

        if (strObj == null)
        {
            requestProcess_logtime += "Server - Server error. Please try again later ♦ ";
            Response.Redirect(util.ERRPATH + "Server error. Please try again later" + "*♠*" + requestProcess_logtime);
        }

        return strObj;
    }

    #endregion

    #region GetStructureDetails

    /// <summary>
    /// Gets the structure of the tstruct either from the customized folder, or the Cache or Database
    /// </summary>
    /// <param name="strObj"></param>
    private void GetStructureDetails(TStructDef strObj, CacheManager cacheMgr)
    {
        Boolean objFromCache = strObj.IsObjFromCache;

        tstScript = ClearStringBuilders(tstScript);
        tstTabScript = ClearStringBuilders(tstTabScript);
        if (objFromCache)
        {
            tstTabScript.Append(strObj.GenerateTabScript(strObj));
            cacheMgr.GetStructureHTML(transId, AxRole, sid, language);
            tstHTML.Append(cacheMgr.StructureHtml);
            tstHeader.Append(cacheMgr.TstHeaderHtml);
            toolbarBtnHtml.Append(cacheMgr.ToolbarBtnIcons);
            if (cacheMgr.ModernToolbarBtnOpen != "")
            {
                modernToolbarBtnHtml.Append(cacheMgr.ModernToolbarBtnOpen.Split('♦')[0]);
                btnfooteropenlist = cacheMgr.ModernToolbarBtnOpen.Split('♦')[1];
                modernFooterActBtnHtml.Append(cacheMgr.ModernToolbarBtnOpen.Split('♦')[2]);
            }
            tstScript.Append(cacheMgr.StructureScript);
            tstCaption = cacheMgr.StructureCaption;
            tstName = cacheMgr.StructureName;

            if (AxpTstButtonStyle == "old")
            {
                if (ShowSubCanBtns() && !theModeDesign)
                {
                    if ((strObj.dcs.Count >= 0) && Session["MobileView"] != null)
                    {
                        submitCancelBtns.Append("<div id = 'dvsubmitCancelBtns' class=''><center><table><input id = 'btnSaveTst' type=button class='saveTask btn btn-primary hotbtn'  onclick='javascript:FormSubmit();'  value='Submit' title='Submit'>&nbsp;&nbsp;<input id = 'New' type=button class='newTask btn btn-primary coldbtn'  onclick='javascript:NewTstruct();'  value='Reset' title='Reset'>&nbsp;&nbsp;</table></center></div>");
                        // dvFooter.Visible = true;
                        heightframe.Attributes.Add("data-submitcancel", "true");
                    }
                }
                else
                {
                    //dvFooter.Visible = false;
                    submitCancelBtns = ClearStringBuilders(submitCancelBtns);
                    heightframe.Attributes.Add("data-submitcancel", "false");
                }
                dvFooterHtml = submitCancelBtns.ToString();
            }

            if (string.IsNullOrEmpty(tstHTML.ToString()) || string.IsNullOrEmpty(tstHeader.ToString()) || string.IsNullOrEmpty(tstScript.ToString()))
            {
                objFromCache = false;
                attHtml = ClearStringBuilders(attHtml);
                taskBtnHtml = ClearStringBuilders(taskBtnHtml);
                dcHtml = ClearStringBuilders(dcHtml);
                tstHeader = ClearStringBuilders(tstHeader);
                submitCancelBtns = ClearStringBuilders(submitCancelBtns);
                tstScript = ClearStringBuilders(tstScript);
                tstCaption = string.Empty;
                tstName = string.Empty;
            }
            else
            {
                if (isTstructCached && isTstInCache)
                {
                    //tstScript.Append(cacheMgr.TabScript);
                    tstHTML = ClearStringBuilders(tstHTML);
                    WriteHtml(strObj, cacheMgr);
                }
                else
                    wBdr.InnerHtml = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + tstHTML.ToString();
                //wBdr.InnerHtml = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + Constants.WIZARD_TEMPLATE + tstHTML.ToString();
            }
        }

        if (util.BreadCrumb)
        {
            GetBreadCrumb();
        }

        if (!objFromCache)
        {

            tstCaption = strObj.tstCaption;
            //tstCaption = Session["menubreadcrumb"].ToString() + strObj.tstCaption;
            ParseStructure(strObj, cacheMgr);
            tstName = strObj.transId;
            cacheMgr.StructureHtml = tstHTML.ToString();
            cacheMgr.axdesignJObcmg = strObj.axdesignJObject;
            cacheMgr.StructureScript = tstScript.ToString();
            cacheMgr.StructureCaption = tstCaption;
            cacheMgr.StructureName = tstName;
            cacheMgr.StructureSubmitCancel = submitCancelBtns.ToString();
            cacheMgr.TstHeaderHtml = tstHeader.ToString();
            cacheMgr.ToolbarBtnIcons = toolbarBtnHtml.ToString();
            cacheMgr.ModernToolbarBtnOpen = modernToolbarBtnHtml.ToString() + "♦" + btnfooteropenlist + "♦" + modernFooterActBtnHtml;
            string language = HttpContext.Current.Session["language"].ToString();
            string fdKey = Constants.REDISTSTRUCT;
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                fdKey = Constants.REDISTSTRUCTMOB;
            string pgKey = Constants.AXPAGETITLE;
            ArrayList redisvalues = new ArrayList();
            Boolean designMode = false;
            if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
                designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
            if (!designMode)
            {
                cacheMgr.fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKey, transId), strObj, Constants.REDISTSTRUCT, schemaName);

                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

                var redisvalues1 = fObj.ObjectJsonFromRedis(util.GetRedisServerkey(pgKey, ""));
                if (redisvalues1 == null)
                    redisvalues.Add(Title + "♠" + tstCaption + "♠" + transId);
                else
                {
                    redisvalues = (ArrayList)redisvalues1;
                    string newValue = Title + "♠" + tstCaption + "♠" + transId;
                    if (!redisvalues.Contains(newValue))
                        redisvalues.Add(newValue);

                }
                cacheMgr.fdwObj.SaveInRedisServer(util.GetRedisServerkey(pgKey, transId), redisvalues, Constants.AXPAGETITLE, schemaName);

            }
            cacheMgr.SetStructureHTML(transId, AxRole, language);
        }

        //strGlobalVar = util.GetGlobalVarString();
        //if (strGlobalVar != string.Empty)
        //{
        //    string global_Scripts = "<script language='javascript' type='text/javascript' >" + strGlobalVar + "</script>";
        //    tstScript.Append(global_Scripts);
        //}
    }

    private StringBuilder ClearStringBuilders(StringBuilder strName)
    {
        strName.Remove(0, strName.ToString().Length);
        return strName;
    }

    #endregion

    #region
    private void GetAxRulesDef(TStructDef strObj, string transId)
    {
        try
        {
            string ScriptOnSubmitXML = string.Empty;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            string strAxRulesUser = string.Empty;
            string conRuleUser = Constants.AXRULESDEFUserRole;
            strAxRulesUser = fObj.StringFromRedis(util.GetRedisServerkey(conRuleUser, transId), schemaName);
            if (strAxRulesUser != string.Empty && strAxRulesUser != "" && strAxRulesUser != "<axrulesdef></axrulesdef>")// User role wise rules 
            {
                int ruledefNo = 0;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strAxRulesUser);

                XmlNode axruleDefChildNodes = default(XmlNode);
                axruleDefChildNodes = xmlDoc.DocumentElement.SelectSingleNode("//axrulesdef"); //ruleDefNodes.ChildNodes;
                StringBuilder ruledeflist = new StringBuilder();
                string ScriptOnLoad = "false", ScriptOnSubmit = "false", formcontrol = "false", formcontrolparents = "false";
                foreach (XmlNode rulesNode in axruleDefChildNodes)
                {
                    foreach (XmlNode rulesChildNode in rulesNode)
                    {
                        if (rulesChildNode.Name.ToLower() == "scriptonload")
                        {
                            string strSOL = "";
                            if (rulesChildNode.InnerText != "")
                            {
                                ScriptOnLoad = "true";
                                strSOL = rulesChildNode.InnerText.Replace("\n", "♥").Replace("^", "♥");
                            }
                            ruledeflist.Append("AxRDScriptOnLoad[" + ruledefNo + "] = " + "\"" + strSOL + "\";");
                        }
                        else if (rulesChildNode.Name.ToLower() == "onsubmit")
                        {
                            if (rulesChildNode.InnerText != "")
                            {
                                ScriptOnSubmit = "true";
                                ScriptOnSubmitXML = rulesChildNode.InnerText.Replace("^", "\n");
                            }
                        }
                        else if (rulesChildNode.Name.ToLower() == "formcontrolparent")
                        {
                            if (rulesChildNode.InnerText != "")
                                formcontrolparents = "true";
                            ruledeflist.Append("AxRDFormControlParent[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                        }
                        else if (rulesChildNode.Name.ToLower() == "formcontrol")
                        {
                            string strFc = "";
                            if (rulesChildNode.InnerText != "")
                            {
                                formcontrol = "true";
                                strFc = rulesChildNode.InnerText.Replace("\n", "♥").Replace("{^}", "♣♣").Replace("^", "♥");
                                strFc = strFc.Replace("♣♣", "{^}");
                            }
                            ruledeflist.Append("AxRDFormControl[" + ruledefNo + "] = " + "\"" + strFc + "\";");
                        }
                        else if (rulesChildNode.Name.ToLower() == "isapplicable")
                        {
                            ruledeflist.Append("AxRDIsApplicable[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                        }
                        else if (rulesChildNode.Name.ToLower() == "levelno")
                        {
                            ruledeflist.Append("AxRDLevelNo[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                        }
                        else if (rulesChildNode.Name.ToLower() == "pegrule")
                        {
                            ruledeflist.Append("AxRDPegRule[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                        }
                    }
                    ruledefNo++;
                }
                string strRulesDefEngin = ScriptOnLoad + "~" + ScriptOnSubmit + "~" + formcontrol + "~" + formcontrolparents;
                string jsRuleDefArray = ruledeflist.ToString() + "var AxRulesEngine=\"" + strRulesDefEngin + "\";";
                tstScript.Append("<script language='javascript' type='text/javascript' >" + jsRuleDefArray + "</script>");
            }
            else if (strAxRulesUser == string.Empty || strAxRulesUser == "")//If user role wise is empty rule then need to check in full rule key
            {
                string strAxRules = string.Empty;
                string conRule = Constants.AXRULESDEF;
                strAxRules = fObj.StringFromRedis(util.GetRedisServerkey(conRule, transId), schemaName);

                if (strAxRules != string.Empty && strAxRules != "" && strAxRules != "<axrulesdef></axrulesdef>")
                {
                    StringBuilder ruledeflist = new StringBuilder();
                    int ruledefNo = 0;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(strAxRules);

                    XmlNode axruleDefChildNodes = default(XmlNode);
                    axruleDefChildNodes = xmlDoc.DocumentElement.SelectSingleNode("//axrulesdef"); //ruleDefNodes.ChildNodes;

                    StringBuilder sbUserRules = new StringBuilder();
                    string ScriptOnLoad = "false", ScriptOnSubmit = "false", formcontrol = "false", formcontrolparents = "false";
                    foreach (XmlNode rulesNode in axruleDefChildNodes)
                    {
                        XmlDocument xmlDocNode = new XmlDocument();
                        xmlDocNode.LoadXml(rulesNode.OuterXml);
                        XmlNode axruleDefNodes = default(XmlNode);
                        axruleDefNodes = xmlDocNode.SelectSingleNode("//uroles");
                        bool isRolesMath = false;
                        if (axruleDefNodes.InnerText != "")
                        {
                            string[] rdefRoles = axruleDefNodes.InnerText.Split(',');
                            string userRoles = Session["AxRole"].ToString();
                            string[] userRolesList = userRoles.Split(',');
                            foreach (var rdrName in rdefRoles)
                            {
                                int index = Array.IndexOf(userRolesList, rdrName);
                                if (index != -1 || (rdrName != "" && rdrName.ToLower() == "default"))
                                {
                                    isRolesMath = true;
                                    break;
                                }
                            }
                        }
                        if (isRolesMath)
                        {
                            sbUserRules.Append(rulesNode.OuterXml);

                            foreach (XmlNode rulesChildNode in rulesNode)
                            {
                                if (rulesChildNode.Name.ToLower() == "scriptonload")
                                {
                                    string strSOL = "";
                                    if (rulesChildNode.InnerText != "")
                                    {
                                        ScriptOnLoad = "true";
                                        strSOL = rulesChildNode.InnerText.Replace("\n", "♥").Replace("^", "♥");
                                    }
                                    ruledeflist.Append("AxRDScriptOnLoad[" + ruledefNo + "] = " + "\"" + strSOL + "\";");
                                }
                                else if (rulesChildNode.Name.ToLower() == "onsubmit")
                                {
                                    if (rulesChildNode.InnerText != "")
                                    {
                                        ScriptOnSubmit = "true";
                                        ScriptOnSubmitXML = rulesChildNode.InnerText.Replace("^", "\n");
                                    }
                                }
                                else if (rulesChildNode.Name.ToLower() == "formcontrolparent")
                                {
                                    if (rulesChildNode.InnerText != "")
                                        formcontrolparents = "true";
                                    ruledeflist.Append("AxRDFormControlParent[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                                }
                                else if (rulesChildNode.Name.ToLower() == "formcontrol")
                                {
                                    string strFc = "";
                                    if (rulesChildNode.InnerText != "")
                                    {
                                        formcontrol = "true";
                                        //strFc = rulesChildNode.InnerText.Replace("\n", "♥").Replace("^", "♥");
                                        strFc = rulesChildNode.InnerText.Replace("\n", "♥").Replace("{^}", "♣♣").Replace("^", "♥");
                                        strFc = strFc.Replace("♣♣", "{^}");
                                    }
                                    ruledeflist.Append("AxRDFormControl[" + ruledefNo + "] = " + "\"" + strFc + "\";");
                                }
                                else if (rulesChildNode.Name.ToLower() == "isapplicable")
                                {
                                    ruledeflist.Append("AxRDIsApplicable[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                                }
                                else if (rulesChildNode.Name.ToLower() == "levelno")
                                {
                                    ruledeflist.Append("AxRDLevelNo[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                                }
                                else if (rulesChildNode.Name.ToLower() == "pegrule")
                                {
                                    ruledeflist.Append("AxRDPegRule[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                                }
                            }
                            ruledefNo++;
                        }
                    }

                    try
                    {
                        FDW fdwObj = new FDW();
                        string conRuleuser = Constants.AXRULESDEFUserRole;
                        fdwObj.SaveInRedisServer(util.GetRedisServerkey(conRuleuser, transId), "<axrulesdef>" + sbUserRules + "</axrulesdef>", Constants.AXRULESDEFUserRole, schemaName);
                    }
                    catch (Exception ex)
                    { }

                    string strRulesDefEngin = ScriptOnLoad + "~" + ScriptOnSubmit + "~" + formcontrol + "~" + formcontrolparents;
                    string jsRuleDefArray = ruledeflist.ToString() + "var AxRulesEngine=\"" + strRulesDefEngin + "\";";
                    tstScript.Append("<script language='javascript' type='text/javascript' >" + jsRuleDefArray + "</script>");
                }
            }

            if (ScriptOnSubmitXML != string.Empty)
                Session["AxRuleOnSubmit"] = "<onsubmit>" + ScriptOnSubmitXML + "</onsubmit>";
            else
                Session.Remove("AxRuleOnSubmit");
        }
        catch (Exception ex)
        { }
    }

    private void GetAxRulesDefBtnConMsg(string transId)
    {
        try
        {
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

            string strAxRulesBtnUser = string.Empty;
            string conRuleUser = Constants.AXRULESDEFBTNCONMSGUSERROLE;
            strAxRulesBtnUser = fObj.StringFromRedis(util.GetRedisServerkey(conRuleUser, transId), schemaName);
            if (strAxRulesBtnUser != string.Empty && strAxRulesBtnUser != "" && strAxRulesBtnUser != "<axrulesdef_btnmsgs></axrulesdef_btnmsgs>")// User role wise btn rules 
            {
                StringBuilder ruledeflistBtn = new StringBuilder();
                int ruledefNo = 0;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strAxRulesBtnUser);
                XmlNode axruleDefChildNodes = default(XmlNode);
                axruleDefChildNodes = xmlDoc.DocumentElement.SelectSingleNode("//axrulesdef_btnmsgs");
                string AxRulesBtnConMsg = "false";
                foreach (XmlNode rulesNode in axruleDefChildNodes)
                {
                    foreach (XmlNode rulesChildNode in rulesNode)
                    {
                        AxRulesBtnConMsg = "true";
                        if (rulesChildNode.Name.ToLower() == "btnname")
                        {
                            ruledeflistBtn.Append("AxRDBtnMsgName[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                        }
                        else if (rulesChildNode.Name.ToLower() == "btnapplicable")
                        {
                            string strCondition = "";
                            if (rulesChildNode.InnerText != "")
                            {
                                strCondition = rulesChildNode.InnerText.Replace("\n", " ").Replace("^", " ");
                            }
                            ruledeflistBtn.Append("AxRDBtnMsgCond[" + ruledefNo + "] = " + "\"" + strCondition + "\";");
                        }
                        else if (rulesChildNode.Name.ToLower() == "confirm_msg")
                        {
                            string strConMsg = "";
                            if (rulesChildNode.InnerText != "")
                            {
                                strConMsg = rulesChildNode.InnerText.Replace("\n", " ");
                            }
                            ruledeflistBtn.Append("AxRDBtnMsgConMsg[" + ruledefNo + "] = " + "\"" + strConMsg + "\";");
                        }
                    }
                    ruledefNo++;
                }
                string jsRuleDefArray = ruledeflistBtn.ToString() + "var AxRulesBtnConMsg=\"" + AxRulesBtnConMsg + "\";";
                tstScript.Append("<script language='javascript' type='text/javascript' >" + jsRuleDefArray + "</script>");
            }
            else if (strAxRulesBtnUser == string.Empty || strAxRulesBtnUser == "")//If user role wise is empty btn rule then need to check in full btn rule key
            {
                string strAxRulesBtnMsg = string.Empty;
                string conRuleBtnMsg = Constants.AXRULESDEFBTNCMSG;
                strAxRulesBtnMsg = fObj.StringFromRedis(util.GetRedisServerkey(conRuleBtnMsg, transId), schemaName);
                if (strAxRulesBtnMsg != string.Empty && strAxRulesBtnMsg != "")
                {
                    StringBuilder ruledeflistBtn = new StringBuilder();
                    int ruledefNo = 0;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(strAxRulesBtnMsg);
                    XmlNode axruleDefChildNodes = default(XmlNode);
                    axruleDefChildNodes = xmlDoc.DocumentElement.SelectSingleNode("//axrulesdef_btnmsgs");
                    string AxRulesBtnConMsg = "false";
                    StringBuilder sbUserRules = new StringBuilder();
                    foreach (XmlNode rulesNode in axruleDefChildNodes)
                    {
                        XmlDocument xmlDocNode = new XmlDocument();
                        xmlDocNode.LoadXml(rulesNode.OuterXml);
                        XmlNode axruleDefNodes = default(XmlNode);
                        axruleDefNodes = xmlDocNode.SelectSingleNode("//btnuroles");
                        bool isRolesMath = false;
                        if (axruleDefNodes.InnerText != "")
                        {
                            string[] rdefRoles = axruleDefNodes.InnerText.Split(',');
                            string userRoles = Session["AxRole"].ToString();
                            string[] userRolesList = userRoles.Split(',');
                            foreach (var rdrName in rdefRoles)
                            {
                                int index = Array.IndexOf(userRolesList, rdrName);
                                if (index != -1 || (rdrName != "" && rdrName.ToLower() == "default"))
                                {
                                    isRolesMath = true;
                                    break;
                                }
                            }
                        }
                        if (isRolesMath)
                        {
                            sbUserRules.Append(rulesNode.OuterXml);
                            foreach (XmlNode rulesChildNode in rulesNode)
                            {
                                AxRulesBtnConMsg = "true";
                                if (rulesChildNode.Name.ToLower() == "btnname")
                                {
                                    ruledeflistBtn.Append("AxRDBtnMsgName[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                                }
                                else if (rulesChildNode.Name.ToLower() == "btnapplicable")
                                {
                                    string strCondition = "";
                                    if (rulesChildNode.InnerText != "")
                                    {
                                        strCondition = rulesChildNode.InnerText.Replace("\n", " ").Replace("^", " ");
                                    }
                                    ruledeflistBtn.Append("AxRDBtnMsgCond[" + ruledefNo + "] = " + "\"" + strCondition + "\";");
                                }
                                else if (rulesChildNode.Name.ToLower() == "confirm_msg")
                                {
                                    string strConMsg = "";
                                    if (rulesChildNode.InnerText != "")
                                    {
                                        strConMsg = rulesChildNode.InnerText.Replace("\n", " ");
                                    }
                                    ruledeflistBtn.Append("AxRDBtnMsgConMsg[" + ruledefNo + "] = " + "\"" + strConMsg + "\";");
                                }
                            }
                            ruledefNo++;
                        }
                    }

                    try
                    {
                        FDW fdwObj = new FDW();
                        string conRuleuser = Constants.AXRULESDEFBTNCONMSGUSERROLE;
                        fdwObj.SaveInRedisServer(util.GetRedisServerkey(conRuleuser, transId), "<axrulesdef_btnmsgs>" + sbUserRules + "</axrulesdef_btnmsgs>", Constants.AXRULESDEFBTNCONMSGUSERROLE, schemaName);
                    }
                    catch (Exception ex)
                    { }

                    string jsRuleDefArray = ruledeflistBtn.ToString() + "var AxRulesBtnConMsg=\"" + AxRulesBtnConMsg + "\";";
                    tstScript.Append("<script language='javascript' type='text/javascript' >" + jsRuleDefArray + "</script>");
                }
            }
        }
        catch (Exception ex)
        { }
    }
    #endregion   

    #region Structure Related Methods

    #region ParseStructure
    /// <summary>
    /// Function to create Html for the Structure.
    /// </summary>
    /// <param name="strObj"></param>
    private void ParseStructure(TStructDef strObj, CacheManager cacheMgr)
    {
        if (!strObj.IsObjCustomHtml || theModeDesign)
        {
            if (!strObj.hideToolBar)
                if (strObj.dwbToolbar)
                {
                    toolbarBtnHtml.Length = 0;
                    if (AxpTstButtonStyle != "old") //if (AxpTstButtonStyle != "" && AxpTstButtonStyle != "old")
                        toolbarBtnHtml = createModernToolbarButtons(strObj, new ArrayList());
                    else
                    {
                        toolbarBtnHtml = createDwbToolbarButtons(strObj, new ArrayList());
                        PrintHTMLtoPDFNew(strObj);
                        if (Session["axDesign"].ToString() == "true")
                        {
                            designModeBtnHtml = Constants.DESIGN_MODE_BTN_HTML_NEW;
                            toolbarBtnHtml.Append(designModeBtnHtml.ToString());
                        }
                    }
                }
                else
                {
                    if (AxpTstButtonStyle != "old")
                        toolbarBtnHtml = createModernToolbarButtons(strObj, new ArrayList());
                    else
                    {
                        CreateToolbarButtons(strObj);
                        if (Session["axDesign"].ToString() == "true")
                        {
                            designModeBtnHtml = "<a href=\"javascript:void(0)\" id='design' title='Design Mode' onclick='goToDesignMode()' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">Design Mode</a>";
                            toolbarBtnHtml.Append(designModeBtnHtml.ToString());
                        }
                    }
                }
            toolbarBtnHtml.Append(ClearCacheAndReloadBtn());
        }
        else
        {
            tstToolBarBtn.InnerHtml = string.Empty;
            if (cacheMgr.tstCustomHTML.Split('@').Length > 1)
                toolbarBtnHtml.Append(cacheMgr.tstCustomHTML.Split('@')[1] != "" ? cacheMgr.tstCustomHTML.Split('@')[1] : designModeBtnHtml.ToString());
            else if (designModeBtnHtml != string.Empty)
                toolbarBtnHtml.Append(designModeBtnHtml.ToString());

            // footer buttons custom html
            if (cacheMgr.tstCustomHTML.Split('@').Length > 3)
                modernToolbarBtnHtml.Append(cacheMgr.tstCustomHTML.Split('@')[3]);
        }
        CreateHeaderHtml(strObj, cacheMgr);
        if (!strObj.IsObjCustomHtml || theModeDesign)
            CreateDcHtml(strObj);
        strObj.CreateTabArrays();
        tstScript.Append(strObj.GetJScriptArrays(strObj));
        if (!strObj.IsObjCustomHtml || theModeDesign)
            WriteHtml(strObj, cacheMgr);
        else
        {
            string gridElementsHeightScript = "<script type='text/javascript'>SetGridElementsHeight();</script>";
            if (cacheMgr.tstCustomHTML.Split('@').Length > 2)
                tstHTML.Append(cacheMgr.tstCustomHTML.Split('@')[2] + gridElementsHeightScript);
            else
                tstHTML.Append(cacheMgr.tstCustomHTML + gridElementsHeightScript);
            //wBdr.InnerHtml = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + Constants.WIZARD_TEMPLATE + tstHTML.ToString();
            wBdr.InnerHtml = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + tstHTML.ToString();
            tstTabScript.Append(strObj.GenerateTabScript(strObj));//Temp added for Custom HTML Issue
        }
        tstScript.Append(tstTabScript.ToString());
    }

    private string ClearCacheAndReloadBtn()
    {
        string clearBtnHtml = string.Empty;
        try
        {
            if (Session["Build"] != null && Session["Build"].ToString() == "T")
            {
                string _btnFunction = " onclick=\"javascript:clearCacheReloadForm('" + transId + "');\" ";
                string _title = "Clear Cache and Reload Form";
                if (AxpTstButtonStyle == "old")
                    clearBtnHtml = "<a href=\"javascript:void(0)\" id='btnClearCacheReloadForm' title='" + _title + "' " + _btnFunction.ToString() + " class=\"dwbBtn btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\"><i class='material-icons'>domain</i><span class='tbCaption'>" + _title + "</span></a>";
                else if (AxpTstButtonStyle == "classic")
                    clearBtnHtml = "<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='btnClearCacheReloadForm' title='" + _title + "' " + _btnFunction.ToString() + " class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">C</span></div><span class=\"dropdownIconName w-100 text-truncate overflow-hidden\">" + _title + "</span></a></div>";
                else
                    clearBtnHtml = "<div class=\"col-4\"><a href=\"javascript:void(0)\" id='btnClearCacheReloadForm' title='" + _title + "' " + _btnFunction.ToString() + " class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">C</span></div><span class=\"dropdownIconName w-100 text-truncate overflow-hidden\">" + _title + "</span></a></div>";
            }
        }
        catch (Exception ex) { }
        return clearBtnHtml;
    }

    #endregion

    #region Toolbar Creation Methods

    /// <summary>
    /// Function to create the toolbar buttons from the button array in the structdef object.
    /// <RELEASENOTE> For action button, the image of the button needs tobe available at AxpImages folder. 
    /// Note on png files are supported.</RELEASENOTE>
    /// </summary>
    /// <param name="strObj">StructDef object returned for the structure</param>
    //TODO: Steps needed to add new task
    private void CreateToolbarButtons(TStructDef strObj)
    {
        //The loop is reversed to sort the toolbar button left values which come in the descending order.   
        toolbarBtnHtml.Length = 0;


        for (int i = strObj.btns.Count - 1; i >= 0; i--)
        {
            TStructDef.ButtonStruct btn = (TStructDef.ButtonStruct)strObj.btns[i];
            string[] arrLeft = null;
            string tlhw = string.Empty;

            tlhw = btn.dimension;
            if (!string.IsNullOrEmpty(tlhw))
            {
                arrLeft = tlhw.Split(',');
                if ((arrLeft.Length > 0 & arrLeft[1] != string.Empty))
                {
                    if (leftBtns.IndexOf(arrLeft[1].ToString()) != -1)
                        arrLeft[1] = Convert.ToString(Convert.ToInt32(arrLeft[1], 10) + 1);
                    leftBtns.Add(arrLeft[1]);
                    tmpLeftBtns.Add(arrLeft[1]);
                }
                else
                {
                    logobj.CreateLog("    Button left value is missing:  " + btn.caption, sid, fileName, "");
                }
            }
            string hint = btn.hint;
            string caption = btn.caption;
            string task = btn.task.ToLower();
            string action = btn.action.ToLower();
            string btnName = "";
            if (!string.IsNullOrEmpty(caption)) btnName = caption;
            else if (!string.IsNullOrEmpty(hint)) btnName = hint;
            btnFunction = string.Empty;
            btnHTML = string.Empty;
            btnStyle = string.Empty;
            btnStyle = "";
            string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;

            switch (task)
            {
                case "new":
                    if (string.IsNullOrEmpty(btnName)) btnName = "New";
                    if (hint == "New")
                    {
                        btnFunction = " onclick='javascript:NewTstruct();' ";
                        toolBarBtns.Add("<a href=\"javascript:void(0)\" id='new' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                        btnStyle = "newTask";
                    }
                    else
                    {
                        btnFunction = " onclick='javascript:NewTstruct();' ";
                        toolBarBtns.Add("<a href=\"javascript:void(0)\" id='new' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    }
                    break;

                case "save":
                    if (string.IsNullOrEmpty(btnName)) btnName = "Save";
                    btnFunction = " onclick='javascript:FormSubmit();' ";
                    toolBarBtns.Add("<a href=\"javascript:void(0)\" id='imgSaveTst' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + hint + "'  class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");

                    break;

                case "search":
                    if (string.IsNullOrEmpty(btnName)) btnName = "Search";
                    btnFunction = " onclick=\"javascript:OpenSearch('" + transId + "');\" ";
                    btnStyle = "handCur";
                    toolBarBtns.Add("<a href=\"javascript:void(0)\" id='search' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    break;

                case "remove":
                    if (string.IsNullOrEmpty(btnName)) btnName = "Remove";
                    btnFunction = " onclick='javascript:DeleteTstruct();' ";
                    btnStyle = "handCur";
                    toolBarBtns.Add("<a href=\"javascript:void(0)\" id='remove' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + hint + "' title='Delete' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    break;
                case "print":
                    if (string.IsNullOrEmpty(btnName)) btnName = "Print";
                    btnFunction = " onclick=\"javascript:OpenPrint('" + transId + "');\" ";
                    btnStyle = "handCur";
                    if (strObj.tstPform == "yes")
                    {
                        toolBarBtns.Add("<a href=\"javascript:void(0)\" id='print' " + btnFunction.ToString() + " data-extra='" + _hint + "'  alt='" + hint + "' title='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    }
                    else
                    {
                        toolBarBtns.Add("");
                    }
                    break;

                case "pdf":
                    if (string.IsNullOrEmpty(btnName)) btnName = "PDF";
                    string hideClass = string.Empty;
                    if (strObj.pdfList == "")
                    {
                        hideClass = " class=\"hide\" ";
                    }
                    btnFunction = " onclick='javascript:OpenPdfDocList();'";
                    btnStyle = "handCur";
                    toolBarBtns.Add("<a href=\"javascript:void(0)\" id='pdf' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + hint + "' title='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    break;

                case "view history":
                    if (string.IsNullOrEmpty(btnName)) btnName = "View History";
                    btnFunction = " onclick=\"javascript:OpenHistory('" + transId + "');\" ";
                    btnStyle = "handCur";
                    toolBarBtns.Add("<a href=\"javascript:void(0)\" id='viewhistory' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + hint + "' title='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    break;

                case "listview":
                    if (Session["IsAxOldDataList"] != null && Session["IsAxOldDataList"].ToString() == "true")
                    {
                        if (string.IsNullOrEmpty(btnName)) btnName = "List View";
                        btnFunction = " onclick=\"javascript:CallListView('" + transId + "');\" ";
                        btnStyle = "handCur";
                        toolBarBtns.Add("<a href=\"javascript:void(0)\" id='listview' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='List View' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(btnName)) btnName = "List View";
                        btnFunction = " onclick=\"javascript:CallEntityList('" + transId + "');\" ";
                        btnStyle = "handCur";
                        toolBarBtns.Add("<a href=\"javascript:void(0)\" id='listview' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='List View' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    }
                    break;
                case "draft":
                    isDraftbutton = "true";
                    toolBarBtns.Add("<a href=\"javascript: void(0)\" title='Draft' data-extra='" + _hint + "' class=\"menu-link text-gray-600 text-hover-white\"><span class=\"menu-title\">" + lblTaskBtn.Text + "</span><span class=\"menu-arrow text-gray-600 text-hover-white\"></span></a><div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"top-end\">");
                    toolBarBtns.Add("<div class=\"liTaskItems menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftSave' onclick=\"SaveAsDraftNew();\" title='Draft Save' class=\"TaskItems menu-link px-3\">Save</a></div>");
                    toolBarBtns.Add("<div class=\"liTaskItems menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftLoad' onclick=\"getDraftsListNew(this);\" title='Draft Load' class=\"TaskItems menu-link px-3\">Load</a></div>");
                    toolBarBtns.Add("</div>");
                    break;

                case "attach":
                    if (string.IsNullOrEmpty(btnName)) btnName = "Attach";
                    btnFunction = " onclick='javascript:AttachFiles();' ";
                    btnStyle = "handCur";
                    toolBarBtns.Add("<a href=\"javascript:void(0)\" id='attach' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    break;

                case "tasks":

                    btnFunction = " onclick='javascript:FindPos();ShowTaskList();' ";
                    btnStyle = "handCur";
                    if (strObj.taskBtns.Count > 0)
                    {
                        StringBuilder TaskBtnHTML = new StringBuilder();
                        //TaskBtnHTML.Append("<li class='dropdown'><a href='javascript:void(0)' id='tasks' class='dropdown-toggle' data-toggle='dropdown' data-hover='dropdown' title='Tasks' data-close-others='true'>" + lblTaskBtn.Text + "&nbsp;<span class='icon-arrows-down'></span></a><ul class='dropdown-menu'>");
                        TaskBtnHTML.Append("<div class=\"menu menu-dropdown menu-item btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2 p-0\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-start\"><a href=\"javascript: void(0)\" title='Options' class=\"menu-link text-gray-600 text-hover-white\"><span class=\"menu-title\">" + lblTaskBtn.Text + "</span><span class=\"menu-arrow text-gray-600 text-hover-white\"></span></a><div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"top-end\">");
                        TaskBtnHTML.Append(CreateTaskButtonsOld(strObj));
                        TaskBtnHTML.Append("</div></div>");
                        toolBarBtns.Add(TaskBtnHTML.ToString());
                    }
                    else
                    {
                        toolBarBtns.Add("");
                    }
                    break;
                case "preview":

                    btnFunction = " onclick=\"javascript:OpenPrint('" + transId + "')\" ";
                    if (string.IsNullOrEmpty(btnName)) btnName = "Preview";
                    btnStyle = "handCur";
                    if (strObj.tstPform == "yes")
                    {
                        toolBarBtns.Add("<a href=\"javascript:void(0)\" id='preview' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + hint + "' title='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                    }
                    else
                    {
                        toolBarBtns.Add("");
                    }
                    break;

                case "":
                    if (btn.action != "")
                    {
                        if (!string.IsNullOrEmpty(btn.fileupload))
                        {

                            btnStyle = "handCur";
                            string actConfirmMsg = string.Empty;
                            string actRem = string.Empty;
                            string manRem = string.Empty;
                            for (int m = 0; m <= strObj.actions.Count - 1; m++)
                            {
                                TStructDef.ActionStruct actn = (TStructDef.ActionStruct)strObj.actions[m];
                                if (actn.actname == btn.action)
                                {
                                    actConfirmMsg = actn.actdesc;
                                    actRem = actn.actRem;
                                    manRem = actn.manRem;
                                }
                            }
                            if (!util.IsImageAvailable(btn.image))
                                btn.image = "";

                            if (btn.fileupload == "y")
                            {

                                btnFunction = " onclick=\"javascript:CallFileUploadAction('" + btn.action + "','" + btn.fmessage + "','" + btn.ftype + "','" + actConfirmMsg + "');\" ";

                                if (!string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(btn.image))
                                {
                                    //Display Image with caption as hint.
                                    toolBarBtns.Add("<input type=hidden id=cb_sactbu name=cb_sactbu/><a href=\"javascript:void(0)\" id='" + hint + "'  alt='" + caption + "' " + btnFunction.ToString() + " class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + caption + "</a>");
                                }
                                else if (!string.IsNullOrEmpty(btn.image) && string.IsNullOrEmpty(caption))
                                {
                                    //Display only image
                                    toolBarBtns.Add("<input href=\"javascript:void(0)\" type=hidden id=cb_sactbu name=cb_sactbu/><a href=\"javascript:void(0)\" id='" + hint + "' alt='" + hint + "' " + btnFunction.ToString() + " class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + hint + "</a>");
                                }
                                else
                                {
                                    toolBarBtns.Add("<input type=hidden id=cb_sactbu name=cb_sactbu/><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " title='" + caption + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + caption + "</a>");
                                }

                            }
                            else if (btn.fileupload == "a")
                            {
                                btnFunction = " onclick='javascript:AttachFiles();' ";
                                toolBarBtns.Add("<input type=hidden id=cb_sactbu name=cb_sactbu/><a href=\"javascript:void(0)\" id='attach' " + btnFunction.ToString() + " alt='" + hint + "' title='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btnName + "</a>");
                            }
                            else
                            {

                                btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "');\" ";
                                if ((btn.fileupload.IndexOf("\\") != -1))
                                {
                                    btn.fileupload = btn.fileupload.Replace("\\", "\\\\");
                                }
                                toolBarBtns.Add("<a href=\"javascript:void(0)\" id=\"" + btn.caption + "\" " + btnFunction.ToString() + " data-extra='" + _hint + "' alt=\"" + btn.caption + "\" title=\"" + btn.caption + "\" class=\"action actionBtn btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btn.caption + "</a>");
                            }
                            btn.fileupload = "";
                        }
                        else
                        {
                            CreateActionButtons(btn, strObj);
                            if (leftBtns.Count > toolBarBtns.Count)
                            {
                                toolBarBtns.Add("");
                            }
                            btn.fileupload = "";
                            btn.cancelBtn = "";
                        }
                    }
                    else
                    {
                        if (!util.IsImageAvailable(btn.image))
                            btn.image = "";
                        //TODO: provide the button like any other case, on click it should alert "No task defined"                     
                        btnStyle = "handCur";
                        if (!string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(btn.image))
                        {
                            //Display Image with caption as hint.
                            toolBarBtns.Add("<a href=\"javascript:void(0)\" id='" + hint + "' data-extra='" + _hint + "' alt='" + caption + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + caption + "</a>");
                        }
                        else if (!string.IsNullOrEmpty(btn.image) && string.IsNullOrEmpty(caption))
                        {
                            //Display only image
                            toolBarBtns.Add("<a href=\"javascript:void(0)\" id='" + hint + "' data-extra='" + _hint + "' alt='" + hint + "' class=\"btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + hint + "</a>");
                        }
                        else
                        {
                            toolBarBtns.Add("<a href=\"javascript:void(0)\" id=\"" + hint + "\" data-extra='" + _hint + "' class=\"actionBtn btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btn.caption + "</a>");
                        }
                    }
                    break;

                default:
                    if ((task == "close") || (task == "gofirst") || (task == "gonext") || (task == "goprior") || (task == "golast"))
                    {
                        toolBarBtns.Add("");
                    }
                    else
                    {
                        CreateActionButtons(btn, strObj);
                        if (leftBtns.Count > toolBarBtns.Count)
                        {
                            toolBarBtns.Add("");
                        }
                    }

                    break;
            }
        }
        //toolbarBtnHtml.Append(toolBarBtns.ToString());
        try
        {
            if (strObj.customBtns.Count > 0)
            {
                customBtnHtml = customObj.AxGetCustomTstBtns(strObj);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "controlDimmer", "closeParentFrame();", true);
            throw ex;
        }

        leftBtns.Sort(new Util.CustomComparer());

        AlignToolbarBtns(strObj);

        PrintHTMLtoPDF(strObj);

        if (isDraftbutton == "false" && AutosaveDraftt == "true")
        {
            toolbarBtnHtml.Append("<div class=\"gropuedBtn menu menu-dropdown menu-item btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2 p-0\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-start\"><a href=\"javascript: void(0)\" title='Draft' data-extra='Draft' class=\"menu-link text-gray-600 text-hover-white\"><span class=\"menu-title\">Draft</span><span class=\"menu-arrow text-gray-600 text-hover-white\"></span></a><div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"top-end\">");
            toolbarBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftSave' onclick=\"SaveAsDraftNew();\" alt='Draft Save' title='Draft Save' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">S</span></div><span class=\"dropdownIconName\">Save</span></a></div>");
            toolbarBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftLoad' onclick=\"getDraftsListNew(this);\" alt='Draft Load' title='Draft Load' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">L</span></div><span class=\"dropdownIconName\">Load</span></a></div>");
            toolbarBtnHtml.Append("</div></div>");
        }

    }

    private StringBuilder createDwbToolbarButtons(TStructDef strObj, ArrayList buttons, string parentID = "")
    {

        StringBuilder BtnHtml = new StringBuilder();
        ArrayList footerbuttonList = new ArrayList();
        ArrayList buttonList;
        string buttonClass = "";
        if (parentID != "")
        {
            buttonList = new ArrayList(buttons);
            buttonClass = " listItem ";
        }
        else
        {
            buttonList = new ArrayList(strObj.btns);
            buttonClass = " dwbBtn ";
        }
        for (int i = 0; i < buttonList.Count; i++)
        {
            TStructDef.ButtonStruct btn = (TStructDef.ButtonStruct)buttonList[i];
            string id = btn.ID;
            string caption = btn.caption;
            string hint = btn.hint;
            string task = btn.task.ToLower();
            string action = btn.action.ToLower();
            bool script = btn.isScript;
            bool isDropDwn = btn.isDrpDwn;
            string isFooter = btn.footer;
            bool isVisible = btn.visible == "true" ? true : false;
            string iconStyle = string.Empty;
            btnFunction = string.Empty;
            btnStyle = string.Empty;
            btnStyle = "";
            if (btn.image != "" && util.IsImageAvailable(btn.image, "icon"))
            {
                iconStyle = "<img src='" + btn.image + "' class='tbIcon'>";
            }
            else if (btn.icon.text != "" && btn.icon.addclass != "")
            {
                iconStyle = "<i class='" + btn.icon.addclass + " material-icons-style material-icons-2'>" + btn.icon.text + "</i>";
            }
            switch (task)
            {
                case "new":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? "add" : caption) : hint;
                        if (isFooter == "true" && !isMobileView)
                        {
                            footerbuttonList.Add("ftbtn_iNew~" + id + "~" + _hint);
                        }
                        else
                        {
                            btnFunction = " onclick='javascript:NewTstruct();' ";
                            btnStyle = "handCur";
                            //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " alt='" + caption + "' title='" + caption + "' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                            BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='add' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                        }

                    }
                    break;

                case "save":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        if (isFooter == "true" && !isMobileView)
                            footerbuttonList.Add("ftbtn_iSave~" + id + "~" + _hint);
                        else
                        {
                            btnFunction = " onclick='javascript:FormSubmit();' ";
                            btnStyle = "handCur";
                            //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " alt='" + caption + "' title='" + caption + "' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                            BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                        }
                    }
                    break;

                case "search":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick=\"javascript:OpenSearch('" + transId + "');\" ";
                        btnStyle = "handCur";
                        //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " title='" + caption + "' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                        BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                    }
                    break;

                case "remove":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        if (isFooter == "true" && !isMobileView)
                            footerbuttonList.Add("ftbtn_iRemove~" + id + "~" + _hint);
                        else
                        {
                            btnFunction = " onclick='javascript:DeleteTstruct();' ";
                            btnStyle = "handCur";
                            //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " alt='" + caption + "' title='delete' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                            BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                        }
                    }
                    break;
                case "print":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick=\"javascript:OpenPrint('" + transId + "');\" ";
                        btnStyle = "handCur";
                        if (strObj.tstPform == "yes")
                        {
                            //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + "  alt='" + caption + "' title='" + caption + "' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                            BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                        }
                    }
                    break;

                case "pdf":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        string hideStyle = string.Empty;
                        if (strObj.pdfList == "")
                        {
                            hideStyle = " style=\"display:none\"";
                        }
                        btnFunction = " onclick='javascript:OpenPdfDocList();'";
                        btnStyle = "handCur";
                        //BtnHtml.Append("<li" + buttonClass.ToString() + hideStyle.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " alt='" + caption + "' title='" + caption + "' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                        BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                    }
                    break;

                case "view history":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick=\"javascript:OpenHistory('" + transId + "');\" ";
                        btnStyle = "handCur";
                        //BtnHtml.Append("<li" + buttonClass.ToString() + " ><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + "  alt='" + caption + "' title='" + caption + "' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                        BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                    }
                    break;

                case "listview":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        if (Session["IsAxOldDataList"] != null && Session["IsAxOldDataList"].ToString() == "true")
                        {
                            if (isFooter == "true" && !isMobileView)
                            {
                                footerbuttonList.Add("ftbtn_iList~" + id + "~" + _hint);
                                btnfooterlist = "javascript:CallListView('" + transId + "')";
                            }
                            else
                            {
                                btnFunction = " onclick=\"javascript:CallListView('" + transId + "');\" ";
                                btnStyle = "handCur";
                                //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' src=\"../AxpImages/toolicons/view2.png\" " + btnFunction.ToString() + " title='List View' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                                BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                            }
                        }
                        else
                        {
                            if (isFooter == "true" && !isMobileView)
                            {
                                footerbuttonList.Add("ftbtn_iList~" + id + "~" + _hint);
                                btnfooterlist = "javascript:CallEntityList('" + transId + "')";
                            }
                            else
                            {
                                btnFunction = " onclick=\"javascript:CallEntityList('" + transId + "');\" ";
                                btnStyle = "handCur";
                                //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' src=\"../AxpImages/toolicons/view2.png\" " + btnFunction.ToString() + " title='List View' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                                BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                            }
                        }
                    }
                    break;

                case "attach":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick='javascript:AttachFiles();' ";
                        btnStyle = "handCur";
                        //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " title='" + caption + "' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                        BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                    }
                    break;

                case "tasks":
                    if (isVisible)
                    {
                        btnFunction = " onclick='javascript:FindPos();ShowTaskList();' ";
                        btnStyle = "handCur";
                        if (strObj.taskBtns.Count > 0)
                        {
                            StringBuilder TaskBtnHTML = new StringBuilder();
                            //TaskBtnHTML.Append("<li class='dropdown'><a href='javascript:void(0)' id='tasks' class='dropdown-toggle' data-toggle='dropdown' data-hover='dropdown' title='Tasks' data-close-others='true'>" + lblTaskBtn.Text + "&nbsp;<span class='icon-arrows-down'></span></a><ul class='dropdown-menu'>");
                            TaskBtnHTML.Append("<div id='tasks' class=\"menu menu-dropdown menu-item btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2 p-0\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-start\"><a href=\"javascript: void(0)\" title='Tasks' class=\"menu-link text-gray-600 text-hover-white\"><span class=\"menu-title\">" + lblTaskBtn.Text + "</span><span class=\"menu-arrow text-gray-600 text-hover-white\"></span></a><div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"top-end\">");
                            TaskBtnHTML.Append(CreateTaskButtons(strObj, true));
                            TaskBtnHTML.Append("</div></div>");
                            toolBarBtns.Add(TaskBtnHTML.ToString());
                        }
                        else
                        {
                            toolBarBtns.Add("");
                        }
                    }
                    break;

                case "preview":
                    if (isVisible)
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick=\"javascript:OpenPrint('" + transId + "')\" ";
                        btnStyle = "handCur";
                        if (strObj.tstPform == "yes")
                        {
                            //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " alt='" + caption + "' title='" + caption + "' class='" + btnStyle + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                            BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + caption + "' title='" + caption + "' class=\"" + buttonClass + " btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                        }
                    }
                    break;

                case "":
                    if (btn.action != "")
                    {
                        string actConfirmMsg = string.Empty;
                        string actRem = string.Empty;
                        string manRem = string.Empty;
                        for (int m = 0; m <= strObj.actions.Count - 1; m++)
                        {
                            TStructDef.ActionStruct actn = (TStructDef.ActionStruct)strObj.actions[m];
                            if (actn.actname == btn.action)
                            {
                                actConfirmMsg = actn.actdesc;
                                actRem = actn.actRem;
                                manRem = actn.manRem;
                                break;
                            }
                        }
                        btnStyle = "handCur";
                        if (btn.action == "Custom" && !btn.isformbtn)
                            btnFunction = " onclick=\"javascript:" + id + "onclick();\" ";
                        else if (!string.IsNullOrEmpty(btn.fileupload))
                        {
                            if (btn.fileupload == "y")
                            {
                                btnFunction = " onclick=\"javascript:CallFileUploadAction('" + btn.action + "','" + btn.fileupload + "');\" ";
                                BtnHtml.Append("<input type=hidden id='cb_sactbu' name='cb_sactbu'>");
                            }
                            else if (btn.fileupload == "a")
                            {
                                btnFunction = " onclick='javascript:AttachFiles();' ";
                            }
                            else
                            {
                                if ((btn.fileupload.IndexOf("\\") != -1))
                                {
                                    btn.fileupload = btn.fileupload.Replace("\\", "\\\\");
                                }
                                btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";
                            }
                            btn.fileupload = "";
                        }
                        else
                            btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "',''," + script.ToString().ToLower() + ");\" ";
                        string btnId = "actbtn_" + ID;

                        if (!string.IsNullOrEmpty(btn.caption))
                        {
                            //toolbarBtnHtml.Append("<li><a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " class=\"action \" title=\"" + caption + "\" ><span>" + caption + "</span></a></li>");
                            //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='actbtn_" + id + "' " + btnFunction.ToString() + " alt=\"" + caption + "\" class=\"action " + btnStyle + "\" title=\"" + caption + "\" >" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                            string _hint = string.IsNullOrEmpty(hint) ? caption : hint;
                            if (parentID == "")
                                BtnHtml.Append("<a href=\"javascript:void(0)\" id='actbtn_" + id + "' " + btnFunction.ToString() + " alt='" + caption + "' title='" + caption + "' data-extra='" + _hint + "' class=\"" + buttonClass + " caption btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                            else
                            {
                                iconStyle = iconStyle.Replace(" material-icons-2", " material-icons-2 text-gray-500");
                                BtnHtml.Append("<a href=\"javascript:void(0)\" id='actbtn_" + id + "' " + btnFunction.ToString() + " alt='" + caption + "' title='" + caption + "' data-extra='" + _hint + "' class=\"" + buttonClass + " caption menu-item px-3\"><span class='tbCaption menu-link px-3'>" + iconStyle + "<span class=\"text-truncate d-inline-block\">" + caption + "</span></span></a>");
                            }
                        }
                    }
                    else if (isDropDwn)
                    {
                        if (parentID == "")
                            //BtnHtml.Append("<li class='dwbBtn dropdown'><a href=\"javascript:void(0)\" id='" + id + "' alt='" + caption + "' title='" + caption + "' class='dropdown-toggle' data-toggle='dropdown' >" + iconStyle + "<span class='tbCaption'>" + caption + "</span><span class='icon-arrows-down'></span></a> <ul class=\"dropdown-menu\">");
                            BtnHtml.Append("<div class=\"dwbBtn dropdown menu menu-dropdown menu-item btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2 p-0\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-start\"><a href=\"javascript: void(0)\" id='" + id + "' alt='" + caption + "' title='" + caption + "' class=\"menu-link text-gray-600 text-hover-white\"><span class=\"menu-title\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></span><span class=\"menu-arrow text-gray-600 text-hover-white\"></span></a><div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"top-end\">");
                        else
                            //BtnHtml.Append("<li class='dwbBtn dropdown dropdown-submenu'><a href=\"javascript:void(0)\" id='" + id + "' alt='" + caption + "' title='" + caption + "' class='dropdown-toggle' data-toggle='dropdown' >" + iconStyle + "<span class='tbCaption'>" + caption + "</span><span class='icon-arrows-down'></span></a> <ul class=\"dropdown-menu\">");
                            BtnHtml.Append("<div class=\"menu-item px-3\" data-kt-menu-trigger=\"hover\" data-kt-menu-placement=\"left-start\" data-kt-menu-flip=\"center, top\"><a href=\"javascript:void(0)\" id='" + id + "' alt='" + caption + "' title='" + caption + "' class=\"menu-link px-3\"><span class=\"menu-title\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></span><span class=\"menu-arrow\"></span></a><div class=\"menu-sub menu-sub-dropdown w-175px py-4\">");
                        if (btn.childBtns.Count > 0)
                            BtnHtml.Append(createDwbToolbarButtons(strObj, new ArrayList(btn.childBtns), id));
                        BtnHtml.Append("</div></div>");
                    }
                    else
                    {
                        string _hint = string.IsNullOrEmpty(hint) ? caption : hint;
                        //BtnHtml.Append("<li " + buttonClass.ToString() + "><a href=\"javascript:void(0)\" id='" + id + "'  alt='" + caption + "'>" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></li>");
                        BtnHtml.Append("<a href=\"javascript:void(0)\" id='" + id + "'  alt='" + caption + "' data-extra='" + _hint + "' title='" + caption + "' class=\"" + buttonClass + " caption btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                    }
                    break;

            }
        }
        if (footerbuttonList.Count > 0)
            btnfooteropenlist += string.Join(",", footerbuttonList.ToArray()) + ",";
        return BtnHtml;

    }

    private StringBuilder createModernToolbarButtons(TStructDef strObj, ArrayList buttons, string parentID = "")
    {
        StringBuilder BtnHtml = new StringBuilder();
        ArrayList buttonList; //= new ArrayList(strObj.btns);
        ArrayList footerbuttonList = new ArrayList();
        string btnType = AxpTstButtonStyle;
        if (parentID != "")
            buttonList = new ArrayList(buttons);
        else
            buttonList = new ArrayList(strObj.btns);

        for (int i = 0; i < buttonList.Count; i++)
        {
            TStructDef.ButtonStruct btn = (TStructDef.ButtonStruct)buttonList[i];
            string id = btn.ID;
            string caption = btn.caption;
            string hint = btn.hint;
            string task = btn.task.ToLower();
            string action = btn.action.ToLower();
            bool script = btn.isScript;
            bool isDropDwn = btn.isDrpDwn;
            string isFooter = btn.footer;
            string isVisible = btn.visible;
            if (isVisible != "true")
                continue;
            string iconStyle = string.Empty;
            btnFunction = string.Empty;
            btnStyle = string.Empty;
            btnStyle = "";
            if (btn.image != "" && util.IsImageAvailable(btn.image, "icon"))
            {
                iconStyle = "<img src='" + btn.image + "' class='tbIcon'>";
            }
            else if (btn.icon.text != "" && btn.icon.addclass != "")
            {
                iconStyle = "<i class='" + btn.icon.addclass + "'>" + btn.icon.text + "</i>";
            }
            bool isScriptApi = btn.scriptApi;

            switch (task)
            {
                case "new":
                    if (isVisible == "true")
                        footerbuttonList.Add("ftbtn_iNew~" + id + "~" + hint);
                    break;

                case "save":
                    if (isVisible == "true")
                        footerbuttonList.Add("ftbtn_iSave~" + id + "~" + hint);
                    break;

                case "search":
                    if (isVisible == "true")
                    {
                        btnFunction = " onclick=\"javascript:OpenSearch('" + transId + "');\" ";
                        string title = string.IsNullOrEmpty(caption) ? "Search" : caption;
                        string _hint = string.IsNullOrEmpty(hint) ? title : hint;
                        modernToolbarBtnHtml.Append("<a id='" + id + "' " + btnFunction.ToString() + " title='" + title + "' data-extra='" + _hint + "' href=\"javascript:void(0)\" class=\"btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2\"><span class=\"material-icons material-icons-style\">search</span></a>");
                    }
                    break;

                case "remove":
                    if (isVisible == "true")
                    {
                        string actBtnNames = (caption == "" ? task : caption).ToString();
                        string strFirstChars = "A";
                        if (actBtnNames != "")
                            strFirstChars = actBtnNames.Substring(0, 1);
                        btnFunction = " onclick='javascript:DeleteTstruct();' ";
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        if (isMobileView)
                        {
                            if (btnType == "classic")
                                BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='Delete' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                            else
                                BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='Delete' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");

                        }
                        else if (isFooter != "true")
                            modernToolbarBtnHtml.Append("<a id='" + id + "' " + btnFunction.ToString() + " title='Delete' data-extra='" + _hint + "' href=\"javascript:void(0)\" class=\"btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2\"><span class=\"material-icons material-icons-style\">remove</span></a>");
                        else
                            footerbuttonList.Add("ftbtn_iRemove~" + id + "~" + hint);
                    }
                    break;
                case "print":
                    if (isVisible == "true")
                    {
                        string actBtnNames = (caption == "" ? task : caption).ToString();
                        string strFirstChars = "A";
                        if (actBtnNames != "")
                            strFirstChars = actBtnNames.Substring(0, 1);
                        btnFunction = " onclick=\"javascript:OpenPrint('" + transId + "');\" ";
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        if (strObj.tstPform == "yes")
                        {
                            if (isMobileView)
                            {
                                if (btnType == "classic")
                                    BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                else
                                    BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");

                            }
                            else
                                modernToolbarBtnHtml.Append("<a id='" + id + "' " + btnFunction.ToString() + " title='" + (caption == "" ? task : caption) + "' data-extra='" + _hint + "' href=\"javascript:void(0)\" class=\"btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2\"><span class=\"material-icons material-icons-style\">print</span></a>");
                        }
                    }
                    break;
                case "pdf":
                    if (isVisible == "true")
                    {
                        string actBtnNames = (caption == "" ? task : caption).ToString();
                        string strFirstChars = "A";
                        if (actBtnNames != "")
                            strFirstChars = actBtnNames.Substring(0, 1);
                        string hideStyle = string.Empty;
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        if (strObj.pdfList == "")
                        {
                            hideStyle = " style=\"display:none\"";
                        }
                        btnFunction = " onclick='javascript:OpenPdfDocList();'";
                        if (btnType == "classic")
                            BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0);\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                        else
                            BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                    }
                    break;

                case "view history":
                    if (isVisible == "true")
                    {
                        string actBtnNames = (caption == "" ? task : caption).ToString();
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        string strFirstChars = "A";
                        if (actBtnNames != "")
                            strFirstChars = actBtnNames.Substring(0, 1);
                        btnFunction = " onclick=\"javascript:OpenHistory('" + transId + "');\" ";
                        if (btnType == "classic")
                            BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                        else
                            BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                    }
                    break;

                case "listview":
                    if (isVisible == "true")
                    {
                        if (Session["IsAxOldDataList"] != null && Session["IsAxOldDataList"].ToString() == "true")
                        {
                            string actBtnNames = (caption == "" ? task : caption).ToString();
                            string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                            string strFirstChars = "A";
                            if (actBtnNames != "")
                                strFirstChars = actBtnNames.Substring(0, 1);
                            btnFunction = " onclick=\"javascript:CallListView('" + transId + "');\" ";
                            if (isMobileView)
                            {
                                if (btnType == "classic")
                                    BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                else
                                    BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");

                            }
                            else if (isFooter != "true")
                                modernToolbarBtnHtml.Append("<a id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' href=\"javascript:void(0)\" class=\"btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2\"><span class=\"material-icons material-icons-style\">format_list_bulleted</span></a>");
                            else
                            {
                                footerbuttonList.Add("ftbtn_iList~" + id + "~" + hint);
                                btnfooterlist = "javascript:CallListView('" + transId + "')";
                            }
                        }
                        else
                        {
                            string actBtnNames = (caption == "" ? task : caption).ToString();
                            string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                            string strFirstChars = "A";
                            if (actBtnNames != "")
                                strFirstChars = actBtnNames.Substring(0, 1);
                            btnFunction = " onclick=\"javascript:CallEntityList('" + transId + "');\" ";
                            if (isMobileView)
                            {
                                if (btnType == "classic")
                                    BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                else
                                    BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");

                            }
                            else if (isFooter != "true")
                                modernToolbarBtnHtml.Append("<a id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' href=\"javascript:void(0)\" class=\"btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2\"><span class=\"material-icons material-icons-style\">format_list_bulleted</span></a>");
                            else
                            {
                                footerbuttonList.Add("ftbtn_iList~" + id + "~" + hint);
                                btnfooterlist = "javascript:CallEntityList('" + transId + "')";
                            }
                        }
                    }
                    break;

                case "attach":
                    if (isVisible == "true")
                    {
                        string actBtnNames = (caption == "" ? task : caption).ToString();
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        string strFirstChars = "A";
                        if (actBtnNames != "")
                            strFirstChars = actBtnNames.Substring(0, 1);
                        btnFunction = " onclick='javascript:AttachFiles();' ";
                        if (isMobileView)
                        {
                            if (btnType == "classic")
                                BtnHtml.Append("<div class=\"menu-item\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + actBtnNames + "' title='" + actBtnNames + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                            else
                                BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");

                        }
                        else
                            modernToolbarBtnHtml.Append("<a id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + actBtnNames + "' href=\"javascript:void(0)\" class=\"btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2\"><span class=\"material-icons material-icons-style\">attach_file</span></a>");
                    }
                    break;

                case "draft":
                    if (isVisible == "true")
                    {
                        isDraftbutton = "true";
                        string actBtnNames = (caption == "" ? task : caption).ToString();
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        string strFirstChars = "D";
                        if (actBtnNames != "")
                            strFirstChars = actBtnNames.Substring(0, 1);
                        if (isMobileView)
                        {
                            if (btnType == "classic")
                            {
                                BtnHtml.Append("<div class=\"menu-item px-3\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"left-start\" data-kt-menu-flip=\"center, top\"><a href=\"javascript:void(0)\" id='" + id + "' data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><span class=\"menu-title\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars.ToUpper() + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></span><span class=\"menu-arrow\"></span></a><div class=\"menu-sub menu-sub-dropdown w-175px py-4\" style=\"\">");
                                BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftSave' onclick=\"SaveAsDraftNew();\" alt='Draft Save' title='Draft Save' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">S</span></div><span class=\"dropdownIconName\">Save</span></a></div>");
                                BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftLoad' onclick=\"getDraftsListNew(this);\" alt='Draft Load' title='Draft Load' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">L</span></div><span class=\"dropdownIconName\">Load</span></a></div>");
                                BtnHtml.Append("</div></div>");
                            }
                            else
                            {
                                BtnHtml.Append("<div class=\"col-4 menu-item\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-end\" data-kt-menu-flip=\"bottom\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (string.IsNullOrEmpty(caption) ? task : caption) + "'class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3 menu-link\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars.ToUpper() + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span><span class=\"mt-3 menu-arrow transform-90\"></span></a><div class=\"menu-sub menu-sub-dropdown w-175px py-4\" style=\"overflow-x: hidden;\">");
                                BtnHtml.Append("<div class=\"d-flex flex-wrap\"><div class=\"col-6\"><a href=\"javascript:void(0)\" id='DraftSave' onclick=\"SaveAsDraftNew();\" alt='Draft Save' title='Draft Save' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">S</span></div><span class=\"dropdownIconName\">Save</span></a></div>");
                                BtnHtml.Append("<div class=\"col-6\">" + "<a href=\"javascript:void(0)\" id='DraftLoad' onclick=\"getDraftsListNew(this);\" alt='Draft Load' title='Draft Load' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">L</span></div><span class=\"dropdownIconName\">Load</span></a></div></div></div></div>");
                            }

                        }
                        else
                        {
                            modernToolbarBtnHtml.Append("<span class=\"align-items-center\">");
                            modernToolbarBtnHtml.Append("<button type=\"button\" id=\"btnAppsDraft\" data-extra='" + _hint + "' runat=\"server\" class=\"btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm menu-dropdown\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-end\" data-kt-menu-flip=\"top-end\">");
                            modernToolbarBtnHtml.Append("<span class=\"material-icons material-icons-style\" title=\"Draft\">drafts</span>");
                            modernToolbarBtnHtml.Append("</button>");
                            if (btnType == "classic")
                            {
                                modernToolbarBtnHtml.Append("<div class=\"dvbtnAppsDraft menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" style=\"z-index: 106; position: fixed; inset: 0px auto auto 0px; margin: 0px; transform: translate(131px, 52px);\" data-popper-placement=\"bottom-end\">");
                                modernToolbarBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftSave' onclick=\"SaveAsDraftNew();\" alt='Draft Save' title='Draft Save' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">S</span></div><span class=\"dropdownIconName\">Save</span></a></div>");
                                modernToolbarBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftLoad' onclick=\"getDraftsListNew(this);\" alt='Draft Load' title='Draft Load' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">L</span></div><span class=\"dropdownIconName\">Load</span></a></div>");
                                modernToolbarBtnHtml.Append("</div>");
                            }
                            else
                            {
                                modernToolbarBtnHtml.Append("<div class=\"dvbtnAppsDraft menu menu-sub menu-sub-dropdown menu-column w-100 w-sm-350px \" data-kt-menu=\"true\" style=\"z-index: 105; position: fixed; inset: 0px auto auto 0px; margin: 0px; transform: translate(919px, 90px);\" data-popper-placement=\"bottom-end\">");
                                modernToolbarBtnHtml.Append("<div class=\"card\">");
                                modernToolbarBtnHtml.Append("<div class=\"card-body py-5\">");
                                modernToolbarBtnHtml.Append("<div class=\"mh-450px scroll-y me-n5 pe-5\">");
                                modernToolbarBtnHtml.Append("<div class=\"row g-2\">");
                                modernToolbarBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='DraftSave' onclick=\"SaveAsDraftNew();\" alt='Draft Save' title='Draft Save' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">S</span></div><span class=\"dropdownIconName\">Save</span></a></div>");
                                modernToolbarBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='DraftLoad' onclick=\"getDraftsListNew(this);\" alt='Draft Load' title='Draft Load' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">L</span></div><span class=\"dropdownIconName\">Load</span></a></div>");
                                modernToolbarBtnHtml.Append("</div>");
                                modernToolbarBtnHtml.Append("</div>");
                                modernToolbarBtnHtml.Append("</div>");
                                modernToolbarBtnHtml.Append("</div>");
                                modernToolbarBtnHtml.Append("</div>");
                            }
                            modernToolbarBtnHtml.Append("</span>");
                        }
                    }
                    break;

                case "tasks":
                    if (isVisible == "true")
                    {
                        btnFunction = " onclick='javascript:FindPos();ShowTaskList();' ";
                        if (parentID == "")
                        {
                            if (btnType == "classic")
                                BtnHtml.Append("<div class=\"menu-item px-3\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"left-start\" data-kt-menu-flip=\"center, top\"><a href=\"javascript:void(0)\" id='tasks' alt='Tasks' title='Tasks' class=\"menu-link px-3\"><span class=\"menu-title\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">T</span></div><span class=\"dropdownIconName\">Tasks</span></span><span class=\"menu-arrow\"></span></a><div class=\"menu-sub menu-sub-dropdown w-175px py-4\" style=\"\">");
                            else
                                BtnHtml.Append("<div class=\"col-4 menu-item\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-end\" data-kt-menu-flip=\"bottom\"><a href=\"javascript:void(0)\" id='tasks' alt='Tasks' title='Tasks' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3 menu-link\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">T</span></div><span class=\"dropdownIconName\">Tasks</span><span class=\"mt-3 menu-arrow transform-90\"></span></a><div class=\"menu-sub menu-sub-dropdown py-4 w-100 w-sm-350px mh-450px scroll-y\" style=\"\"><div class=\"col-12 d-flex flex-wrap\">");
                        }
                        if (strObj.taskBtns.Count > 0)
                        {
                            if (strObj.dwbToolbar)
                                BtnHtml.Append(createModernToolbarButtons(strObj, new ArrayList(strObj.taskBtns), id));
                            else
                                BtnHtml.Append(CreateModernTaskButtons(strObj));
                        }
                        BtnHtml.Append("</div></div>");
                        if (btnType != "classic")
                            BtnHtml.Append("</div>");
                    }
                    break;

                case "preview":
                    if (isVisible == "true")
                    {
                        string actBtnNames = (caption == "" ? task : caption).ToString();
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        string strFirstChars = "A";
                        if (actBtnNames != "")
                            strFirstChars = actBtnNames.Substring(0, 1);
                        btnFunction = " onclick=\"javascript:OpenPrint('" + transId + "')\" ";
                        if (strObj.tstPform == "yes")
                        {
                            if (btnType == "classic")
                                BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                            else
                                BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                        }
                    }
                    break;
                case "":
                    if (btn.action != "")
                    {
                        string actConfirmMsg = string.Empty;
                        string actRem = string.Empty;
                        string manRem = string.Empty;
                        for (int m = 0; m <= strObj.actions.Count - 1; m++)
                        {
                            TStructDef.ActionStruct actn = (TStructDef.ActionStruct)strObj.actions[m];
                            if (actn.actname == btn.action)
                            {
                                actConfirmMsg = actn.actdesc;
                                actRem = actn.actRem;
                                manRem = actn.manRem;
                                break;
                            }
                        }
                        if (strObj.grpActBtnsList.IndexOf(btn.action) > -1)
                        {
                            string actBtnNames = (caption == "" ? task : caption).ToString();
                            string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                            string strFirstChars = "A";
                            if (actBtnNames != "")
                                strFirstChars = actBtnNames.Substring(0, 1);
                            btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";

                            if (btnType == "classic")
                                actionBarBtns.Add("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">" + actBtnNames + "</span></a></div>");
                            else
                                actionBarBtns.Add("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">" + actBtnNames + "</span></a></div>");
                        }
                        else if (!string.IsNullOrEmpty(btn.fileupload))
                        {
                            if (btn.fileupload == "y")
                            {
                                btnFunction = " onclick=\"javascript:CallFileUploadAction('" + btn.action + "','" + btn.fmessage + "','" + btn.ftype + "','" + actConfirmMsg + "');\" ";
                                string actBtnNames = (caption == "" ? task : caption).ToString();
                                string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                                string strFirstChars = "A";
                                if (actBtnNames != "")
                                    strFirstChars = actBtnNames.Substring(0, 1);
                                if (isFooter == "true" && parentID == "")
                                    modernFooterActBtnHtml.Append("<a href=\"javascript: void(0)\" id=" + id + " " + btnFunction.ToString() + " data-extra='" + _hint + "' class=\"btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm\"><span class=\"material-icons\">" + btn.icon.text + "</span>" + actBtnNames + "</a><input type=hidden id='cb_sactbu' name='cb_sactbu'>");
                                else
                                {
                                    if (btnType == "classic")
                                        BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div><input type=hidden id='cb_sactbu' name='cb_sactbu'>");
                                    else
                                        BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div><input type=hidden id='cb_sactbu' name='cb_sactbu'>");
                                }
                            }
                            else if (btn.fileupload == "a")
                            {
                                string actBtnNames = (caption == "" ? task : caption).ToString();
                                string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                                string strFirstChars = "A";
                                if (actBtnNames != "")
                                    strFirstChars = actBtnNames.Substring(0, 1);
                                btnFunction = " onclick='javascript:AttachFiles();' ";
                                if (isFooter == "true" && parentID == "")
                                    modernFooterActBtnHtml.Append("<a href=\"javascript: void(0)\" id=" + id + " " + btnFunction.ToString() + " data-extra='" + _hint + "' class=\"btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm\"><span class=\"material-icons\">" + btn.icon.text + "</span>" + actBtnNames + "</a>");
                                else
                                {
                                    if (btnType == "classic")
                                        BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                    else
                                        BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                }
                            }
                            else
                            {
                                if ((btn.fileupload.IndexOf("\\") != -1))
                                {
                                    btn.fileupload = btn.fileupload.Replace("\\", "\\\\");
                                }
                                string actBtnNames = (caption == "" ? task : caption).ToString();
                                string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                                string strFirstChars = "A";
                                if (actBtnNames != "")
                                    strFirstChars = actBtnNames.Substring(0, 1);
                                btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";
                                if (isFooter == "true" && parentID == "")
                                    modernFooterActBtnHtml.Append("<a href=\"javascript: void(0)\" id=" + id + " " + btnFunction.ToString() + " data-extra='" + _hint + "' class=\"btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm\"><span class=\"material-icons\">" + btn.icon.text + "</span>" + actBtnNames + "</a>");
                                else
                                {
                                    if (btnType == "classic")
                                        BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                    else
                                        BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                }
                            }
                            btn.fileupload = "";
                        }
                        else if (btn.action == "Custom" && !btn.isformbtn)
                        {
                            string actBtnNames = (caption == "" ? task : caption).ToString();
                            string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                            string strFirstChars = "A";
                            if (actBtnNames != "")
                                strFirstChars = actBtnNames.Substring(0, 1);
                            btnFunction = " onclick=\"javascript:" + id + "onclick();\" ";
                            if (btnType == "classic")
                                BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">" + actBtnNames + "</span></a></div>");
                            else
                                BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100\">" + actBtnNames + "</span></a></div>");
                        }
                        else if (btn.action == "Add New Field")
                        {
                            string actBtnNames = (caption == "" ? task : caption).ToString();
                            string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                            string strFirstChars = "A";
                            if (actBtnNames != "")
                                strFirstChars = actBtnNames.Substring(0, 1);
                            btnFunction = " onclick=\"callAddFormRuntime('addfield','" + transId + "','" + tstCaption + "');\" ";
                            if (isFooter == "true" && parentID == "")
                                modernFooterActBtnHtml.Append("<a href=\"javascript: void(0)\" id=" + id + " " + btnFunction.ToString() + " data-extra='" + _hint + "' class=\"btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm\"><span class=\"material-icons\">" + btn.icon.text + "</span>" + actBtnNames + "</a>");
                            else
                            {
                                if (btnType == "classic")
                                    BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">" + actBtnNames + "</span></a></div>");
                                else
                                    BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100\">" + actBtnNames + "</span></a></div>");
                            }
                        }
                        else if (btn.action == "Add New Form")
                        {
                            string actBtnNames = (caption == "" ? task : caption).ToString();
                            string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                            string strFirstChars = "A";
                            if (actBtnNames != "")
                                strFirstChars = actBtnNames.Substring(0, 1);
                            btnFunction = " onclick=\"getAxpertStudioAddFormData();\" ";
                            if (isFooter == "true" && parentID == "")
                                modernFooterActBtnHtml.Append("<a href=\"javascript: void(0)\" id=" + id + " " + btnFunction.ToString() + " data-extra='" + _hint + "' class=\"btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm\"><span class=\"material-icons\">" + btn.icon.text + "</span>" + actBtnNames + "</a>");
                            else
                            {
                                if (btnType == "classic")
                                    BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">" + actBtnNames + "</span></a></div>");
                                else
                                    BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100\">" + actBtnNames + "</span></a></div>");
                            }
                        }
                        else
                        {
                            string actBtnNames = (caption == "" ? task : caption).ToString();
                            string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                            string strFirstChars = "A";
                            if (actBtnNames != "")
                                strFirstChars = actBtnNames.Substring(0, 1);
                            btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";
                            if (isFooter == "true" && parentID == "")
                                modernFooterActBtnHtml.Append("<a href=\"javascript: void(0)\" id=" + id + " " + btnFunction.ToString() + " data-extra='" + _hint + "' class=\"btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm\"><span class=\"material-icons\">" + btn.icon.text + "</span>" + actBtnNames + "</a>");
                            else
                            {
                                if (btnType == "classic")
                                    BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100px overflow-hidden\">" + actBtnNames + "</span></a></div>");
                                else
                                    BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100\">" + actBtnNames + "</span></a></div>");
                            }
                        }
                    }
                    else if (isDropDwn)
                    {
                        string actBtnName = (caption == "" ? task : caption).ToString();
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        string strFirstChar = "A";
                        if (actBtnName != "")
                            strFirstChar = actBtnName.Substring(0, 1);

                        if (parentID == "")
                        {
                            if (btnType == "classic")
                                BtnHtml.Append("<div class=\"menu-item px-3\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"left-start\" data-kt-menu-flip=\"center, top\"><a href=\"javascript:void(0)\" id='" + id + "' data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><span class=\"menu-title\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChar + "</span></div><span class=\"dropdownIconName text-truncate w-100px overflow-hidden\">" + actBtnName + "</span></span><span class=\"menu-arrow\"></span></a><div class=\"menu-sub menu-sub-dropdown w-175px py-4\" style=\"\">");
                            else
                                BtnHtml.Append("<div class=\"col-4 menu-item\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-end\" data-kt-menu-flip=\"bottom\"><a href=\"javascript:void(0)\" id='" + id + "' data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3 menu-link\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChar + "</span></div><span class=\"dropdownIconName text-truncate w-100\">" + actBtnName + "</span><span class=\"mt-3 menu-arrow transform-90\"></span></a><div class=\"menu-sub menu-sub-dropdown py-4 w-100 w-sm-350px mh-450px scroll-y\" style=\"\"><div class=\"col-12 d-flex flex-wrap\">");
                        }
                        else
                        {
                            if (btnType == "classic")
                                BtnHtml.Append("<div class=\"menu-item px-3\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"left-start\" data-kt-menu-flip=\"center, top\"><a href=\"javascript:void(0)\" id='" + id + "' data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><span class=\"menu-title\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChar + "</span></div><span class=\"dropdownIconName text-truncate w-100px overflow-hidden\">" + actBtnName + "</span></span><span class=\"menu-arrow\"></span></a><div class=\"menu-sub menu-sub-dropdown w-175px py-4\" style=\"\">");
                            else
                                BtnHtml.Append("<div class=\"col-4 menu-item\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-end\" data-kt-menu-flip=\"bottom\"><a href=\"javascript:void(0)\" id='" + id + "' data-extra='" + _hint + "' alt='" + (caption == "" ? task : caption) + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3 menu-link\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChar + "</span></div><span class=\"dropdownIconName text-truncate w-100\">" + actBtnName + "</span><span class=\"mt-3 menu-arrow transform-90\"></span></a><div class=\"menu-sub menu-sub-dropdown py-4 w-100 w-sm-350px mh-450px scroll-y\" style=\"\"><div class=\"col-12 d-flex flex-wrap\">");
                        }
                        if (btn.childBtns.Count > 0)
                            BtnHtml.Append(createModernToolbarButtons(strObj, new ArrayList(btn.childBtns), id));
                        BtnHtml.Append("</div></div>");
                        if (btnType != "classic")
                            BtnHtml.Append("</div>");
                    }
                    else if (isScriptApi)
                    {
                        string actBtnNames = (caption == "" ? task : caption).ToString();
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        string strFirstChars = "A";
                        if (actBtnNames != "")
                            strFirstChars = actBtnNames.Substring(0, 1);
                        btnFunction = " onclick=\"javascript:callExecuteScriptApi('t','" + btn.ID + "','');\" ";
                        if (isFooter == "true" && parentID == "")
                            modernFooterActBtnHtml.Append("<a href=\"javascript: void(0)\" id=" + id + " " + btnFunction.ToString() + " data-extra='" + _hint + "' class=\"btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm\"><span class=\"material-icons\">" + btn.icon.text + "</span>" + actBtnNames + "</a>");
                        else
                        {
                            if (btnType == "classic")
                                BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">" + actBtnNames + "</span></a></div>");
                            else
                                BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName text-truncate w-100\">" + actBtnNames + "</span></a></div>");
                        }
                    }
                    else
                    {
                        string actBtnName = (caption == "" ? task : caption).ToString();
                        string _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        string strFirstChar = "A";
                        if (actBtnName != "")
                            strFirstChar = actBtnName.Substring(0, 1);
                        if (isFooter == "true" && parentID == "")
                            modernFooterActBtnHtml.Append("<a href=\"javascript: void(0)\" id=" + id + " data-extra='" + _hint + "' class=\"btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm\"><span class=\"material-icons\">" + btn.icon.text + "</span>" + actBtnName + "</a>");
                        else
                        {
                            if (btnType == "classic")
                                BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + id + "' alt='" + (caption == "" ? task : caption) + "' data-extra='" + _hint + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChar + "</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">" + actBtnName + "</span></a></div>");
                            else
                                BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + id + "' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChar + "</span></div><span class=\"dropdownIconName text-truncate w-100\">" + actBtnName + "</span></a></div>");
                        }
                    }
                    break;
            }
        }
        if (parentID == "")
        {

            BtnHtml.Append(AlignToolbarBtnsModern(strObj, btnType));
            BtnHtml.Append(PrintHTMLtoPDFModern(strObj, btnType));
            if (isDraftbutton == "false" && AutosaveDraftt == "true")
            {
                if (isMobileView)
                    BtnHtml.Append(GetDraftButon(btnType));
                else
                    modernToolbarBtnHtml.Append(GetDraftButon(btnType));
            }
            if (Session["axDesign"].ToString() == "true")
            {
                if (btnType == "classic")
                    BtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='design' title='Design Mode' onclick='goToDesignMode();' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">D</span></div><span class=\"dropdownIconName\">Design Mode</span></a></div>");
                else
                    BtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='design' title='Design Mode' onclick='goToDesignMode();' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">D</span></div><span class=\"dropdownIconName\">Design Mode</span></a></div>");
            }
            if (btnType == "classic")
                BtnHtml.Append("<div class=\"menu-item px-3 d-none\" id=\"dvRefreshFromLoadModern\"><a href=\"javascript:void(0)\" id='RefreshFormLoad' data-extra='Refresh FormLoad' title='Refresh FormLoad' onclick='ResetFormLoadCache();' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">R</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">Refresh FormLoad</span></a></div>");
            else
                BtnHtml.Append("<div class=\"col-4 d-none\" id=\"dvRefreshFromLoadModern\"><a href=\"javascript:void(0)\" id='RefreshFormLoad' data-extra='Refresh FormLoad' title='Refresh FormLoad' onclick='ResetFormLoadCache();' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">R</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">Refresh FormLoad</span></a></div>");
        }
        if (footerbuttonList.Count > 0)
            btnfooteropenlist += string.Join(",", footerbuttonList.ToArray()) + ",";
        return BtnHtml;
    }

    private string GetDraftButon(string btnType)
    {
        StringBuilder draftbtn = new StringBuilder();
        try
        {
            string actBtnNames = "Draft";
            string strFirstChars = "D";
            if (actBtnNames != "")
                strFirstChars = actBtnNames.Substring(0, 1);
            if (isMobileView)
            {
                draftbtn.Append("<div class=\"menu-item px-3\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"left-start\" data-kt-menu-flip=\"center, top\"><a href=\"javascript:void(0)\" id='draft' alt='draft' title='draft' class=\"menu-link px-3\"><span class=\"menu-title\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars.ToUpper() + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></span><span class=\"menu-arrow\"></span></a><div class=\"menu-sub menu-sub-dropdown w-175px py-4\" style=\"\">");
                draftbtn.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftSave' onclick=\"SaveAsDraftNew();\" alt='Draft Save' title='Draft Save' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">S</span></div><span class=\"dropdownIconName\">Save</span></a></div>");
                draftbtn.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftLoad' onclick=\"getDraftsListNew(this);\" alt='Draft Load' title='Draft Load' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">L</span></div><span class=\"dropdownIconName\">Load</span></a></div>");
                draftbtn.Append("</div></div>");
            }
            else
            {
                draftbtn.Append("<span class=\"align-items-center\">");
                draftbtn.Append("<button type=\"button\" id=\"btnAppsDraft\" runat=\"server\" class=\"btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm menu-dropdown\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-end\" data-kt-menu-flip=\"top-end\">");
                draftbtn.Append("<span class=\"material-icons material-icons-style\" title=\"Draft\">drafts</span>");
                draftbtn.Append("</button>");
                if (btnType == "classic")
                {
                    draftbtn.Append("<div class=\"dvbtnAppsDraft menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" style=\"z-index: 106; position: fixed; inset: 0px auto auto 0px; margin: 0px; transform: translate(131px, 52px);\" data-popper-placement=\"bottom-end\">");
                    draftbtn.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftSave' onclick=\"SaveAsDraftNew();\" alt='Draft Save' title='Draft Save' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">S</span></div><span class=\"dropdownIconName\">Save</span></a></div>");
                    draftbtn.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='DraftLoad' onclick=\"getDraftsListNew(this);\" alt='Draft Load' title='Draft Load' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">L</span></div><span class=\"dropdownIconName\">Load</span></a></div>");
                    draftbtn.Append("</div>");
                }
                else
                {
                    draftbtn.Append("<div class=\"dvbtnAppsDraft menu menu-sub menu-sub-dropdown menu-column w-100 w-sm-350px \" data-kt-menu=\"true\" style=\"z-index: 105; position: fixed; inset: 0px auto auto 0px; margin: 0px; transform: translate(919px, 90px);\" data-popper-placement=\"bottom-end\">");
                    draftbtn.Append("<div class=\"card\">");
                    draftbtn.Append("<div class=\"card-body py-5\">");
                    draftbtn.Append("<div class=\"mh-450px scroll-y me-n5 pe-5\">");
                    draftbtn.Append("<div class=\"row g-2\">");
                    draftbtn.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='DraftSave' onclick=\"SaveAsDraftNew();\" alt='Draft Save' title='Draft Save' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">S</span></div><span class=\"dropdownIconName\">Save</span></a></div>");
                    draftbtn.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='DraftLoad' onclick=\"getDraftsListNew(this);\" alt='Draft Load' title='Draft Load' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">L</span></div><span class=\"dropdownIconName\">Load</span></a></div>");
                    draftbtn.Append("</div>");
                    draftbtn.Append("</div>");
                    draftbtn.Append("</div>");
                    draftbtn.Append("</div>");
                    draftbtn.Append("</div>");
                }
                draftbtn.Append("</span>");
            }
            return draftbtn.ToString();
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    /// <summary>
    /// tstruct html to pdf buttons append to defaultbut variable.
    /// </summary>
    /// <param name="strObj"></param>
    private void PrintHTMLtoPDF(TStructDef strObj)
    {
        try
        {
            if (!string.IsNullOrEmpty(strObj.htmlToPDF))
            {
                String[] htmlToPDFBtns = strObj.htmlToPDF.Split('~');
                foreach (var htmlbtnlen in htmlToPDFBtns)
                {
                    string htmlToPDFBtnName = string.Empty;
                    List<string> htmlToPDFForms = new List<string>();
                    if (htmlbtnlen.Contains("-"))
                    {
                        htmlToPDFBtnName = htmlbtnlen.Split('-')[0];
                        String[] htmlToPDFParams = new String[] { };
                        htmlToPDFParams = htmlbtnlen.Split('-')[1].Split('^');
                        foreach (var formlen in htmlToPDFParams)
                        {
                            if (formlen.Contains("{"))
                                htmlToPDFForms.Add(formlen.Split('{')[0]);
                            else
                                htmlToPDFForms.Add(formlen);
                        }
                    }
                    if (htmlToPDFForms.Count > 1)
                    {
                        StringBuilder btnHtml = new StringBuilder();
                        //toolbarBtnHtml.Append("<ul><li class=\"gropuedBtn\"><div class=\"dropdown\"><button class=\"actionsBtn dropdown-toggle printhtmltopdfbtn\" type=\"button\" data-toggle=\"dropdown\">" + htmlToPDFBtnName + "<span class=\"icon-arrows-down\"></span></button> <ul id=\"uldropelements\" class=\"dropdown-menu\">");
                        toolbarBtnHtml.Append("<div class=\"gropuedBtn menu menu-dropdown menu-item btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2 p-0\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-start\"><a href=\"javascript: void(0)\" title='Tasks' class=\"actionsBtn menu-link text-gray-600 text-hover-white\"><span class=\"menu-title\">" + htmlToPDFBtnName + "</span><span class=\"menu-arrow text-gray-600 text-hover-white\"></span></a><div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"top-end\">");
                        foreach (var htpForms in htmlToPDFForms)
                        {
                            string formName = htpForms.ToString();
                            if (formName.Contains("."))
                                formName = formName.Split('.')[0];
                            //btnHtml.Append("<li class=\"printhtmltopdf\" id=" + htpForms.ToString() + ">" + formName + "</li>");
                            btnHtml.Append("<a href=\"javascript:void(0)\" id=" + htpForms.ToString() + " class=\"printhtmltopdf btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + formName + "</a>");
                        }
                        toolbarBtnHtml.Append(btnHtml.ToString());
                        toolbarBtnHtml.Append("</div></div>");
                    }
                    else if (htmlToPDFForms.Count == 1)
                    {
                        //toolbarBtnHtml.Append("<li><a href=\"javascript:void(0)\" style=\"width:auto\" class=\"printhtmltopdf\" id=" + htmlToPDFForms[0].ToString() + ">" + htmlToPDFBtnName + "</a></li>");
                        toolbarBtnHtml.Append("<a href=\"javascript:void(0)\" id=" + htmlToPDFForms[0].ToString() + " class=\"printhtmltopdf btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + htmlToPDFBtnName + "</a>");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "controlDimmer", "closeParentFrame();", true);
            //throw ex;
        }
    }

    private void PrintHTMLtoPDFNew(TStructDef strObj)
    {
        try
        {
            if (!string.IsNullOrEmpty(strObj.htmlToPDF))
            {
                String[] htmlToPDFBtns = strObj.htmlToPDF.Split('~');
                foreach (var htmlbtnlen in htmlToPDFBtns)
                {
                    string htmlToPDFBtnName = string.Empty;
                    List<string> htmlToPDFForms = new List<string>();
                    if (htmlbtnlen.Contains("-"))
                    {
                        htmlToPDFBtnName = htmlbtnlen.Split('-')[0];
                        String[] htmlToPDFParams = new String[] { };
                        htmlToPDFParams = htmlbtnlen.Split('-')[1].Split('^');
                        foreach (var formlen in htmlToPDFParams)
                        {
                            if (formlen.Contains("{"))
                                htmlToPDFForms.Add(formlen.Split('{')[0]);
                            else
                                htmlToPDFForms.Add(formlen);
                        }
                    }
                    if (htmlToPDFForms.Count > 1)
                    {
                        StringBuilder btnHtml = new StringBuilder();
                        //toolbarBtnHtml.Append("<ul><li class=\"dwbBtn gropuedBtn\"><div class=\"dropdown\"><button class=\"actionsBtn dropdown-toggle printhtmltopdfbtn\" type=\"button\" data-toggle=\"dropdown\"> <i class='material-icons'>print</i><span>" + htmlToPDFBtnName + "</span><span class=\"icon-arrows-down\"></span></button> <ul id=\"uldropelements\" class=\"dropdown-menu\">");
                        toolbarBtnHtml.Append("<div class=\"dwbBtn gropuedBtn menu menu-dropdown menu-item btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2 p-0\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-start\"><a href=\"javascript: void(0)\" title='Tasks' class=\"actionsBtn printhtmltopdfbtn menu-link text-gray-600 text-hover-white\"><span class=\"menu-title\"><i class='material-icons'>print</i><span>" + htmlToPDFBtnName + "</span></span><span class=\"menu-arrow text-gray-600 text-hover-white\"></span></a><div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"top-end\">");
                        foreach (var htpForms in htmlToPDFForms)
                        {
                            string formName = htpForms.ToString();
                            if (formName.Contains("."))
                                formName = formName.Split('.')[0];
                            //btnHtml.Append("<li class=\"dwbBtn printhtmltopdf\" id=" + htpForms.ToString() + "><i class='material-icons'>article</i><span>" + formName + "<span></li>");
                            btnHtml.Append("<a href=\"javascript:void(0)\" id=" + htpForms.ToString() + " class=\"dwbBtn printhtmltopdf btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\"><i class='material-icons'>article</i><span>" + formName + "<span></a>");
                        }
                        toolbarBtnHtml.Append(btnHtml.ToString());
                        toolbarBtnHtml.Append("</div></div>");
                    }
                    else if (htmlToPDFForms.Count == 1)
                    {
                        //toolbarBtnHtml.Append("<li class=\"dwbBtn\" ><a href=\"javascript:void(0)\" style=\"width:auto\" class=\"printhtmltopdf\" id=" + htmlToPDFForms[0].ToString() + "> <i class='material-icons'>print</i><span>" + htmlToPDFBtnName + "</span></a></li>");
                        toolbarBtnHtml.Append("<a href=\"javascript:void(0)\" id=" + htmlToPDFForms[0].ToString() + " class=\"dwbBtn printhtmltopdf btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\"><i class='material-icons'>print</i><span>" + htmlToPDFBtnName + "</span></a>");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "controlDimmer", "closeParentFrame();", true);
            //throw ex;
        }
    }

    private string PrintHTMLtoPDFModern(TStructDef strObj, string btnType)
    {
        StringBuilder htmlpdfButtons = new StringBuilder();
        try
        {
            if (!string.IsNullOrEmpty(strObj.htmlToPDF))
            {
                String[] htmlToPDFBtns = strObj.htmlToPDF.Split('~');
                foreach (var htmlbtnlen in htmlToPDFBtns)
                {
                    string htmlToPDFBtnName = string.Empty;
                    List<string> htmlToPDFForms = new List<string>();
                    if (htmlbtnlen.Contains("-"))
                    {
                        htmlToPDFBtnName = htmlbtnlen.Split('-')[0];
                        String[] htmlToPDFParams = new String[] { };
                        htmlToPDFParams = htmlbtnlen.Split('-')[1].Split('^');
                        foreach (var formlen in htmlToPDFParams)
                        {
                            if (formlen.Contains("{"))
                                htmlToPDFForms.Add(formlen.Split('{')[0]);
                            else
                                htmlToPDFForms.Add(formlen);
                        }
                    }
                    if (htmlToPDFForms.Count > 1)
                    {
                        StringBuilder btnHtml = new StringBuilder();
                        string actBtnName = htmlToPDFBtnName.ToString();
                        string strFirstChar = "H";
                        if (actBtnName != "")
                            strFirstChar = actBtnName.Substring(0, 1);
                        if (btnType == "classic")
                            htmlpdfButtons.Append("<div class=\"menu-item px-3\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"left-start\" data-kt-menu-flip=\"center, top\"><a href=\"javascript:void(0)\" id='" + htmlToPDFBtnName + "' alt='" + htmlToPDFBtnName + "' title='" + htmlToPDFBtnName + "' class=\"menu-link px-3 printhtmltopdfbtn\"><span class=\"menu-title\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChar + "</span></div><span class=\"dropdownIconName\">" + htmlToPDFBtnName + "</span></span><span class=\"menu-arrow\"></span></a><div class=\"menu-sub menu-sub-dropdown w-175px py-4\" style=\"\">");
                        else
                            htmlpdfButtons.Append("<div class=\"col-4 menu-item\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-end\" data-kt-menu-flip=\"bottom\"><a href=\"javascript:void(0)\" id='" + htmlToPDFBtnName + "' alt='" + htmlToPDFBtnName + "' title='" + htmlToPDFBtnName + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3 menu-link printhtmltopdfbtn\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChar + "</span></div><span class=\"dropdownIconName\">" + htmlToPDFBtnName + "</span><span class=\"mt-3 menu-arrow transform-90\"></span></a><div class=\"menu-sub menu-sub-dropdown py-4 w-100 w-sm-350px mh-450px scroll-y\" style=\"\"><div class=\"col-12 d-flex flex-wrap\">");

                        foreach (var htpForms in htmlToPDFForms)
                        {
                            string formName = htpForms.ToString();
                            if (formName.Contains("."))
                                formName = formName.Split('.')[0];
                            string actBName = formName.ToString();
                            string strFirstC = "H";
                            if (actBName != "")
                                strFirstC = actBName.Substring(0, 1);
                            if (btnType == "classic")
                                htmlpdfButtons.Append("<div class=\"menu-item px-3 printhtmltopdf\"><a href=\"javascript:void(0)\" id='" + htpForms.ToString() + "' alt='" + formName + "' title='" + formName + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstC + "</span></div><span class=\"dropdownIconName\">" + formName + "</span></a></div>");
                            else
                                htmlpdfButtons.Append("<div class=\"col-4 printhtmltopdf\"><a href=\"javascript:void(0)\" id='" + htpForms.ToString() + "' alt='" + formName + "' title='" + formName + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstC + "</span></div><span class=\"dropdownIconName\">" + formName + "</span></a></div>");
                        }
                        htmlpdfButtons.Append("</div></div>");
                        if (btnType != "classic")
                            htmlpdfButtons.Append("</div>");
                    }
                    else if (htmlToPDFForms.Count == 1)
                    {
                        string actBtnName = htmlToPDFBtnName.ToString();
                        string strFirstChar = "H";
                        if (actBtnName != "")
                            strFirstChar = actBtnName.Substring(0, 1);
                        if (btnType == "classic")
                            htmlpdfButtons.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='" + htmlToPDFForms[0].ToString() + "' title='" + htmlToPDFBtnName + "' class=\"menu-link px-3 printhtmltopdf\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChar + "</span></div><span class=\"dropdownIconName\">" + htmlToPDFBtnName + "</span></a></div>");
                        else
                            htmlpdfButtons.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='" + htmlToPDFForms[0].ToString() + "' title='" + htmlToPDFBtnName + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3 printhtmltopdf\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChar + "</span></div><span class=\"dropdownIconName\">" + htmlToPDFBtnName + "</span></a></div>");
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return htmlpdfButtons.ToString();
    }
    private void CheckDesignAccess()
    {
        try
        {
            Session["axDesign"] = "false";
            if (HttpContext.Current.Session["AxResponsibilities"] != null)
            {
                if (user.ToLower() == "admin" || (Session["Build"] != null && Session["Build"].ToString() == "T"))
                {
                    Session["axDesign"] = "true";
                    designModeBtnHtml = Constants.DESIGN_MODE_BTN_HTML;
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("CheckDesignAccess -" + ex.Message, HttpContext.Current.Session.SessionID, "CheckDesignAccess", "new");
        }
    }

    /// <summary>
    /// Function to align the buttons and append to defaultbut variable.
    /// </summary>
    /// <remarks></remarks>
    private void AlignToolbarBtns(TStructDef strObj)
    {
        int tempLftCnt = 0;
        int BtnLftCnt = 0;
        for (BtnLftCnt = 0; BtnLftCnt < leftBtns.Count; BtnLftCnt++)
        {
            for (tempLftCnt = 0; tempLftCnt < tmpLeftBtns.Count; tempLftCnt++)
            {
                if (leftBtns[BtnLftCnt].ToString() == tmpLeftBtns[tempLftCnt].ToString())
                {
                    if (toolBarBtns.Count > tempLftCnt)
                        sortedBtns.Add(toolBarBtns[tempLftCnt]);
                    //else if (strObj.grpActBtns != "" && actionBarBtns.Count > tempLftCnt)
                    //{
                    //    sortedBtns.Add(actionBarBtns[tempLftCnt]);
                    //}
                    break;
                }
            }
        }
        for (int j = 0; j < customBtnHtml.Count; j++)
        {
            toolbarBtnHtml.Append(customBtnHtml[j]);
        }
        if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
        {
            for (int j = 0; j <= sortedBtns.Count - 1; j++)
            {
                if ((!string.IsNullOrEmpty(sortedBtns[j].ToString())))
                {
                    toolbarBtnHtml.Append(sortedBtns[j]);
                }
            }
        }
        else
        {
            for (int j = 0; j <= sortedBtns.Count - 1; j++)
            {
                if ((!string.IsNullOrEmpty(sortedBtns[j].ToString())))
                {
                    toolbarBtnHtml.Append(sortedBtns[j]);
                }
            }
        }
        if (strObj.HlpText != "")
        {
            btnFunction = "onclick=\"javascript:ShowTstHelp('" + transId + "');\" ";
            btnStyle = "handCur";
            //toolbarBtnHtml.Append("<li><a href=\"javascript:void(0)\" id='showHelp' " + btnFunction.ToString() + " border=0  alt='Hint' class='" + btnStyle + "'>Help(Info)</a></li>");
        }
        string groupButtons = strObj.grpActBtns;
        if (actionBarBtns.Count > 0)
        {

            if (groupButtons != "")
            {
                ArrayList tempChecker = actionBarBtns;

                String[] btnGroups = groupButtons.Split('~');

                for (int i = 0; i < btnGroups.Length; i++)
                {
                    StringBuilder actionbarBtnHtml = new StringBuilder();
                    string[] seperateButton = (btnGroups[i].Split('-')[1]).ToString().Split(',');
                    for (int j = 0; j < seperateButton.Length; j++)
                    {

                        for (int k = 0; k < actionBarBtns.Count; k++)
                        {
                            if (actionBarBtns[k].ToString().Contains("onclick=\"javascript:CallAction('" + seperateButton[j] + "',")) //if (actionBarBtns[k].ToString().Contains("onclick='javascript:CallAction(\"" + seperateButton[j] + "\","))
                            {
                                actionbarBtnHtml.Append(actionBarBtns[k].ToString());
                                tempChecker.RemoveAt(k);
                            }
                        }
                    }

                    if (actionbarBtnHtml.ToString() != "")
                    {
                        toolbarBtnHtml.Append("<div class=\"gropuedBtn menu menu-dropdown menu-item btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2 p-0 actionsBtn\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-start\"><a href=\"javascript: void(0)\" title='Tasks' class=\"menu-link text-gray-600 text-hover-white\"><span class=\"menu-title\">" + btnGroups[i].Split('-')[0].ToString() + "</span><span class=\"menu-arrow text-gray-600 text-hover-white\"></span></a><div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"top-end\">");
                        toolbarBtnHtml.Append(actionbarBtnHtml.ToString());
                        toolbarBtnHtml.Append("</div></div>");
                    }
                }

                foreach (string item in tempChecker)
                {
                    if (item != "")
                    {
                        toolbarBtnHtml.Append(item.Replace("class=\"action \"", "class=\"action singleaction \""));
                    }
                }
            }
            else
            {
                StringBuilder actionbarBtnHtml = new StringBuilder();
                for (int i = 0; i < actionBarBtns.Count; i++)
                {

                    toolbarBtnHtml.Append(actionBarBtns[i].ToString().Replace("class=\"action \"", "class=\"action singleaction \""));
                }

            }
        }
        if (Session["AxExportTallyTid"] != null)
        {
            if (transId.ToLower() == Session["AxExportTallyTid"].ToString())
                toolbarBtnHtml.Append("<a href=\"javascript:void(0)\" onclick=\"javascript:AxCustomExportToXml();\" id=\"covcxtally\" alt=\"Export Tally XML\" title=\"Export Tally XML\" class=\"action singleaction btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">Export Tally XML</a>");
        }
    }

    private string AlignToolbarBtnsModern(TStructDef strObj, string btnType)
    {
        StringBuilder grooupBtnsModern = new StringBuilder();

        string groupButtons = strObj.grpActBtns;
        if (actionBarBtns.Count > 0)
        {

            if (groupButtons != "")
            {
                String[] btnGroups = groupButtons.Split('~');
                for (int i = 0; i < btnGroups.Length; i++)
                {
                    string actBtnName = btnGroups[i].Split('-')[0].ToString();
                    string strFirstChar = "A";
                    if (actBtnName != "")
                        strFirstChar = actBtnName.Substring(0, 1);

                    if (btnType == "classic")
                        grooupBtnsModern.Append("<div class=\"menu-item px-3\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"left-start\" data-kt-menu-flip=\"center, top\"><a href=\"javascript:void(0)\" alt='" + actBtnName + "' title='" + actBtnName + "' class=\"menu-link px-3\"><span class=\"menu-title\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChar + "</span></div><span class=\"dropdownIconName text-truncate w-100 overflow-hidden\">" + actBtnName + "</span></span><span class=\"menu-arrow\"></span></a><div class=\"menu-sub menu-sub-dropdown w-175px py-4\" style=\"\">");
                    else
                        grooupBtnsModern.Append("<div class=\"col-4 menu-item\" data-kt-menu-trigger=\"click\" data-kt-menu-placement=\"bottom-end\" data-kt-menu-flip=\"bottom\"><a href=\"javascript:void(0)\" alt='" + actBtnName + "' title='" + actBtnName + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3 menu-link\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChar + "</span></div><span class=\"dropdownIconName text-truncate w-100\">" + actBtnName + "</span><span class=\"mt-3 menu-arrow transform-90\"></span></a><div class=\"menu-sub menu-sub-dropdown py-4 w-100 w-sm-350px mh-450px scroll-y\" style=\"\"><div class=\"col-12 d-flex flex-wrap\">");

                    string[] seperateButton = (btnGroups[i].Split('-')[1]).ToString().Split(',');
                    for (int j = 0; j < seperateButton.Length; j++)
                    {
                        for (int k = 0; k < actionBarBtns.Count; k++)
                        {
                            if (actionBarBtns[k].ToString().Contains("onclick=\"javascript:CallAction('" + seperateButton[j] + "',"))
                            {
                                grooupBtnsModern.Append(actionBarBtns[k].ToString());
                            }
                        }
                    }
                    grooupBtnsModern.Append("</div></div>");
                    if (btnType != "classic")
                        grooupBtnsModern.Append("</div>");
                }
            }
        }
        return grooupBtnsModern.ToString();
    }

    /// <summary>
    /// Function to create buttons in the task list i.e. buttons under the task toolbar button.
    /// </summary>
    private string CreateTaskButtons(TStructDef strObj, bool newToolBar = false)
    {
        if (strObj.taskBtns.Count > 0)
        {
            string task = string.Empty;
            string action = string.Empty;
            //taskBtnHtml.Append("<div id='taskListPopUp' style='display:none;min-width:100px;z-index:30000;' onclick=\"javascript:HideTaskList('true')\">");
            //taskBtnHtml.Append("<div><table style='width:100%'>");
            for (int i = 0; i < strObj.taskBtns.Count; i++)
            {
                TStructDef.ButtonStruct btn = (TStructDef.ButtonStruct)strObj.taskBtns[i];
                task = btn.task;
                task = task.ToLower();
                bool script = false;
                if (newToolBar)
                    script = btn.isScript;
                action = btn.action.ToLower();
                string hint = btn.hint;
                string caption = btn.caption;
                btnFunction = string.Empty;
                btnHTML = string.Empty;
                btnStyle = "handCur";

                switch (task)
                {
                    case "attach":

                        btnFunction = " onclick='javascript:AttachFiles();' ";
                        taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" id='attach' title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;
                    case "email":

                        btnFunction = " onclick=\"javascript:openEMail('" + transId + "','tstruct',0);\" ";
                        taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" id='email' title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;
                    case "print":

                        btnFunction = " onclick=\"javascript:OpenPrint('" + transId + "');\" ";
                        taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" id='print' title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;
                    case "save as":

                        btnFunction = " onclick='javascript:CallSaveAs();' ";
                        taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;
                    case "preview":
                        taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a id='preview' class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;
                    case "view history":

                        btnFunction = " onclick=\"javascript:OpenHistory('" + transId + "');\" ";
                        taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;
                    case "pdf":

                        btnFunction = " onclick='javascript:OpenPdfDocList();'";
                        //btnFunction = " onclick='javascript:ProcessRow(1);' ";
                        string hideClass = string.Empty;
                        if (strObj.pdfList == "")
                        {
                            hideClass = " hide ";
                        }
                        taskBtnHtml.Append("<li  class='liTaskItems " + hideClass + "' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\"  title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;

                    case "new":
                        btnFunction = " onclick='javascript:NewTstruct();' ";
                        taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;
                    case "search":
                        btnFunction = " onclick=\"javascript:OpenSearch('" + transId + "');\" ";
                        taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;
                    case "save":
                        btnFunction = " onclick='javascript:FormSubmit();' ";
                        taskBtnHtml.Append("<li class='liTaskItems'  " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        break;
                    case "remove":
                        btnFunction = " onclick='javascript:DeleteTstruct();' ";
                        taskBtnHtml.Append("<li class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=\"Delete\" >" + btn.caption + "</a></li>");
                        break;
                    case "":
                        if (btn.action != "")
                        {
                            string actConfirmMsg = string.Empty;
                            string actRem = string.Empty;
                            string manRem = string.Empty;
                            for (int m = 0; m <= strObj.actions.Count - 1; m++)
                            {
                                TStructDef.ActionStruct actn = (TStructDef.ActionStruct)strObj.actions[m];
                                if (actn.actname == btn.action)
                                {
                                    actConfirmMsg = actn.actdesc;
                                    actRem = actn.actRem;
                                    manRem = actn.manRem;
                                }
                            }
                            if (!string.IsNullOrEmpty(btn.fileupload))
                            {
                                if (btn.fileupload == "y")
                                {
                                    btnFunction = " onclick=\"javascript:CallFileUploadAction('" + btn.action + "','" + btn.fileupload + "');\" ";
                                    taskBtnHtml.Append("<li class='liTaskItems'><a class='TaskItems' href=\"javascript:void(0)\" " + btnFunction.ToString() + "");
                                    taskBtnHtml.Append(">" + btn.caption + "</a></li><input type=hidden id='cb_sactbu' name='cb_sactbu'>");
                                }
                                else if (btn.fileupload == "a")
                                {
                                    btnFunction = " onclick='javascript:AttachFiles();' ";
                                    taskBtnHtml.Append("<li class='liTaskItems'><a class='TaskItems' href=\"javascript:void(0)\" " + btnFunction.ToString() + "");
                                    taskBtnHtml.Append(">" + btn.caption + "</a></li>");
                                }
                                else
                                {
                                    if ((btn.fileupload.IndexOf("\\") != -1))
                                    {
                                        btn.fileupload = btn.fileupload.Replace("\\", "\\\\");
                                    }

                                    btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";
                                    actionBarBtns.Add("<li><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " class=\"action \">" + btn.caption + "</a></li>");
                                }
                                btn.fileupload = "";
                            }
                            //Note: CancelTstruct, Task function is no more supported.
                            //else if (!string.IsNullOrEmpty(btn.cancelBtn))
                            //{
                            //    if (btn.cancelBtn == "y")
                            //    {
                            //        btnFunction = " onclick='javascript:CancelTstruct();' ";
                            //        taskBtnHtml.Append("<tr><td><a><img  id=" + hint + " src='../AxpImages/icons/16x16/cancel.png' " + btnFunction.ToString() + " border=0 alt=" + hint + " title=" + hint + " class='" + btnStyle + "'></a>&nbsp;</td></tr>");
                            //    }
                            //    btn.cancelBtn = "";
                            //}
                            else
                            {
                                btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";
                                taskBtnHtml.Append("<li class='liTaskItems' " + btnFunction + " ><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " ");
                                taskBtnHtml.Append(" >" + btn.caption + "</a></li>");
                            }
                        }
                        else
                        {
                            taskBtnHtml.Append("<li class='liTaskItems'><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " onclick='javascript:AlertNoAction();' ");
                            taskBtnHtml.Append(">" + btn.caption + "</a></li>");
                        }
                        break;
                    default:
                        taskBtnHtml.Append("<li class='liTaskItems'><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " onclick='javascript:AlertNoAction();' ");
                        taskBtnHtml.Append(">" + btn.caption + "</a></li>");
                        break;
                }
                //taskBtnHtml.Append("</tr>");
            }
            //taskBtnHtml.Append("</table></div>");
            //taskBtnHtml.Append("</div>");
            return taskBtnHtml.ToString();
        }
        else return string.Empty;
    }

    private string CreateTaskButtonsOld(TStructDef strObj, bool newToolBar = false)
    {
        if (strObj.taskBtns.Count > 0)
        {
            string task = string.Empty;
            string action = string.Empty;
            //taskBtnHtml.Append("<div id='taskListPopUp' style='display:none;min-width:100px;z-index:30000;' onclick=\"javascript:HideTaskList('true')\">");
            //taskBtnHtml.Append("<div><table style='width:100%'>");
            for (int i = 0; i < strObj.taskBtns.Count; i++)
            {
                TStructDef.ButtonStruct btn = (TStructDef.ButtonStruct)strObj.taskBtns[i];
                task = btn.task;
                task = task.ToLower();
                bool script = false;
                if (newToolBar)
                    script = btn.isScript;
                action = btn.action.ToLower();
                string hint = btn.hint;
                string caption = btn.caption;
                btnFunction = string.Empty;
                btnHTML = string.Empty;
                btnStyle = "handCur";

                switch (task)
                {
                    case "attach":

                        btnFunction = " onclick='javascript:AttachFiles();' ";
                        //taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" id='attach' title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" id='attach' title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "email":

                        btnFunction = " onclick=\"javascript:openEMail('" + transId + "','tstruct',0);\" ";
                        //taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" id='email' title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" id='email' title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "print":

                        btnFunction = " onclick=\"javascript:OpenPrint('" + transId + "');\" ";
                        //taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" id='print' title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" id='print' title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "save as":

                        btnFunction = " onclick='javascript:CallSaveAs();' ";
                        //taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "preview":
                        //taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a id='preview' class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" id='preview' title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "view history":

                        btnFunction = " onclick=\"javascript:OpenHistory('" + transId + "');\" ";
                        //taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "pdf":

                        btnFunction = " onclick='javascript:OpenPdfDocList();'";
                        //btnFunction = " onclick='javascript:ProcessRow(1);' ";
                        string hideClass = string.Empty;
                        if (strObj.pdfList == "")
                        {
                            hideClass = " d-none ";
                        }
                        //taskBtnHtml.Append("<li  class='liTaskItems " + hideClass + "' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\"  title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3" + hideClass + "\" " + btnFunction + "><a href=\"javascript:void(0)\" title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;

                    case "new":
                        btnFunction = " onclick='javascript:NewTstruct();' ";
                        //taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "search":
                        btnFunction = " onclick=\"javascript:OpenSearch('" + transId + "');\" ";
                        //taskBtnHtml.Append("<li  class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "save":
                        btnFunction = " onclick='javascript:FormSubmit();' ";
                        //taskBtnHtml.Append("<li class='liTaskItems'  " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "remove":
                        btnFunction = " onclick='javascript:DeleteTstruct();' ";
                        //taskBtnHtml.Append("<li class='liTaskItems' " + btnFunction + "><a class='TaskItems' href=\"javascript:void(0)\" title=\"Delete\" >" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + "><a href=\"javascript:void(0)\" title=\"Delete\" class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                    case "":
                        if (btn.action != "")
                        {
                            string actConfirmMsg = string.Empty;
                            string actRem = string.Empty;
                            string manRem = string.Empty;
                            for (int m = 0; m <= strObj.actions.Count - 1; m++)
                            {
                                TStructDef.ActionStruct actn = (TStructDef.ActionStruct)strObj.actions[m];
                                if (actn.actname == btn.action)
                                {
                                    actConfirmMsg = actn.actdesc;
                                    actRem = actn.actRem;
                                    manRem = actn.manRem;
                                }
                            }
                            if (!string.IsNullOrEmpty(btn.fileupload))
                            {
                                if (btn.fileupload == "y")
                                {
                                    btnFunction = " onclick=\"javascript:CallFileUploadAction('" + btn.action + "','" + btn.fileupload + "');\" ";
                                    //taskBtnHtml.Append("<li class='liTaskItems'><a class='TaskItems' href=\"javascript:void(0)\" " + btnFunction.ToString() + "");
                                    //taskBtnHtml.Append(">" + btn.caption + "</a></li><input type=hidden id='cb_sactbu' name='cb_sactbu'>");
                                    taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div><input type=hidden id='cb_sactbu' name='cb_sactbu'>");
                                }
                                else if (btn.fileupload == "a")
                                {
                                    btnFunction = " onclick='javascript:AttachFiles();' ";
                                    //taskBtnHtml.Append("<li class='liTaskItems'><a class='TaskItems' href=\"javascript:void(0)\" " + btnFunction.ToString() + "");
                                    //taskBtnHtml.Append(">" + btn.caption + "</a></li>");
                                    taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                                }
                                else
                                {
                                    if ((btn.fileupload.IndexOf("\\") != -1))
                                    {
                                        btn.fileupload = btn.fileupload.Replace("\\", "\\\\");
                                    }

                                    btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";
                                    //actionBarBtns.Add("<li><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " class=\"action \">" + btn.caption + "</a></li>");
                                    taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " title=" + btn.caption + " class=\"action menu-link px-3\">" + btn.caption + "</a></div>");
                                }
                                btn.fileupload = "";
                            }
                            else
                            {
                                btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";
                                //taskBtnHtml.Append("<li class='liTaskItems' " + btnFunction + " ><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " ");
                                //taskBtnHtml.Append(" >" + btn.caption + "</a></li>");
                                taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" " + btnFunction + " ><a href=\"javascript:void(0)\" title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                            }
                        }
                        else
                        {
                            //taskBtnHtml.Append("<li class='liTaskItems'><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " onclick='javascript:AlertNoAction();' ");
                            //taskBtnHtml.Append(">" + btn.caption + "</a></li>");
                            taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" ><a href=\"javascript:void(0)\" onclick='javascript:AlertNoAction();' title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        }
                        break;
                    default:
                        //taskBtnHtml.Append("<li class='liTaskItems'><a class='TaskItems' href=\"javascript:void(0)\" title=" + btn.caption + " onclick='javascript:AlertNoAction();' ");
                        //taskBtnHtml.Append(">" + btn.caption + "</a></li>");
                        taskBtnHtml.Append("<div class=\"liTaskItems menu-item px-3\" ><a href=\"javascript:void(0)\" onclick='javascript:AlertNoAction();' title=" + btn.caption + " class=\"TaskItems menu-link px-3\">" + btn.caption + "</a></div>");
                        break;
                }
            }
            return taskBtnHtml.ToString();
        }
        else return string.Empty;
    }

    private string CreateModernTaskButtons(TStructDef strObj)
    {
        if (strObj.taskBtns.Count > 0)
        {
            string btnType = AxpTstButtonStyle;
            string task = string.Empty;
            string action = string.Empty;
            for (int i = 0; i < strObj.taskBtns.Count; i++)
            {
                TStructDef.ButtonStruct btn = (TStructDef.ButtonStruct)strObj.taskBtns[i];
                task = btn.task;
                task = task.ToLower();
                bool script = btn.isScript;
                action = btn.action.ToLower();
                string hint = btn.hint;
                string caption = btn.caption;
                btnFunction = string.Empty;
                string _hint = string.Empty;
                btnHTML = string.Empty;
                btnStyle = "handCur";

                switch (task)
                {
                    case "attach":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick='javascript:AttachFiles();' ";
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='attach' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='attach' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>A</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "email":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick=\"javascript:openEMail('" + transId + "','tstruct',0);\" ";
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='email' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='email' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>E</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "print":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick=\"javascript:OpenPrint('" + transId + "');\" ";
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='print' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='print' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>P</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "save as":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick='javascript:CallSaveAs();' ";
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='saveas' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='saveas' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>S</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "preview":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='preview' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='preview' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>P</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "view history":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick=\"javascript:OpenHistory('" + transId + "');\" ";
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" id='viewhistory' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" id='viewhistory' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>V</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "pdf":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick='javascript:OpenPdfDocList();'";
                        string hideClass = string.Empty;
                        if (strObj.pdfList == "")
                        {
                            hideClass = " d-none ";
                        }
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3" + hideClass + "\"><a href=\"javascript:void(0)\" id='pdf' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4" + hideClass + "\"><a href=\"javascript:void(0)\" id='pdf' " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>P</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;

                    case "new":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick='javascript:NewTstruct();' ";
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>N</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "search":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick=\"javascript:OpenSearch('" + transId + "');\" ";
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>S</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "save":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick='javascript:FormSubmit();' ";
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>S</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "remove":
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        btnFunction = " onclick='javascript:DeleteTstruct();' ";
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='Delete' class=\"menu-link px-3\">" + (caption == "" ? task : caption) + "</a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='Delete' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div>R</div><span class=\"fw-boldest\">" + (caption == "" ? task : caption) + "</span></a></div>");
                        break;
                    case "":
                        if (btn.action != "")
                        {
                            string actConfirmMsg = string.Empty;
                            string actRem = string.Empty;
                            string manRem = string.Empty;
                            for (int m = 0; m <= strObj.actions.Count - 1; m++)
                            {
                                TStructDef.ActionStruct actn = (TStructDef.ActionStruct)strObj.actions[m];
                                if (actn.actname == btn.action)
                                {
                                    actConfirmMsg = actn.actdesc;
                                    actRem = actn.actRem;
                                    manRem = actn.manRem;
                                }
                            }
                            if (!string.IsNullOrEmpty(btn.fileupload))
                            {
                                if (btn.fileupload == "y")
                                {
                                    btnFunction = " onclick=\"javascript:CallFileUploadAction('" + btn.action + "','" + btn.fileupload + "');\" ";

                                    string actBtnNames = (caption == "" ? task : caption).ToString();
                                    _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                                    string strFirstChars = "A";
                                    if (actBtnNames != "")
                                        strFirstChars = actBtnNames.Substring(0, 1);

                                    if (btnType == "classic")
                                        taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div><input type=hidden id='cb_sactbu' name='cb_sactbu'>");
                                    else
                                        taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div><input type=hidden id='cb_sactbu' name='cb_sactbu'>");
                                }
                                else if (btn.fileupload == "a")
                                {
                                    string actBtnNames = (caption == "" ? task : caption).ToString();
                                    _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                                    string strFirstChars = "A";
                                    if (actBtnNames != "")
                                        strFirstChars = actBtnNames.Substring(0, 1);
                                    btnFunction = " onclick='javascript:AttachFiles();' ";
                                    if (btnType == "classic")
                                        taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                    else
                                        taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                }
                                else
                                {
                                    if ((btn.fileupload.IndexOf("\\") != -1))
                                    {
                                        btn.fileupload = btn.fileupload.Replace("\\", "\\\\");
                                    }

                                    string actBtnNames = (caption == "" ? task : caption).ToString();
                                    _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                                    string strFirstChars = "A";
                                    if (actBtnNames != "")
                                        strFirstChars = actBtnNames.Substring(0, 1);

                                    btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";
                                    if (btnType == "classic")
                                        taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                    else
                                        taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                }
                                btn.fileupload = "";
                            }
                            else
                            {
                                string actBtnNames = (caption == "" ? task : caption).ToString();
                                _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                                string strFirstChars = "A";
                                if (actBtnNames != "")
                                    strFirstChars = actBtnNames.Substring(0, 1);
                                btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "','','" + script.ToString().ToLower() + "');\" ";
                                if (btnType == "classic")
                                    taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                                else
                                    taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" " + btnFunction.ToString() + " data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                            }
                        }
                        else
                        {
                            string actBtnNames = (caption == "" ? task : caption).ToString();
                            _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                            string strFirstChars = "A";
                            if (actBtnNames != "")
                                strFirstChars = actBtnNames.Substring(0, 1);
                            if (btnType == "classic")
                                taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" onclick='javascript:AlertNoAction();' data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                            else
                                taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" onclick='javascript:AlertNoAction();' data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChars + "</span></div><span class=\"dropdownIconName\">" + actBtnNames + "</span></a></div>");
                        }
                        break;
                    default:
                        string actBtnName = (caption == "" ? task : caption).ToString();
                        _hint = string.IsNullOrEmpty(hint) ? (caption == "" ? task : caption) : hint;
                        string strFirstChar = "A";
                        if (actBtnName != "")
                            strFirstChar = actBtnName.Substring(0, 1);
                        if (btnType == "classic")
                            taskBtnHtml.Append("<div class=\"menu-item px-3\"><a href=\"javascript:void(0)\" onclick='javascript:AlertNoAction();' data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"menu-link px-3\"><div class=\"symbol symbol-25px symbol-circle me-5 dropdownIconUI\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle\">" + strFirstChar + "</span></div><span class=\"dropdownIconName\">" + actBtnName + "</span></a></div>");
                        else
                            taskBtnHtml.Append("<div class=\"col-4\"><a href=\"javascript:void(0)\" onclick='javascript:AlertNoAction();' data-extra='" + _hint + "' title='" + (caption == "" ? task : caption) + "' class=\"d-flex flex-column flex-center text-center text-gray-800 text-hover-primary bg-hover-light rounded p-3 mb-3\"><div class=\"symbol symbol-circle dropdownIconUI symbol-40px\"><span class=\"symbol-label bg-primary text-white fw-normal fs-3 iconUITitle mb-1\">" + strFirstChar + "</span></div><span class=\"dropdownIconName\">" + actBtnName + "</span></a></div>");
                        break;
                }
            }
            return taskBtnHtml.ToString();
        }
        else return string.Empty;
    }

    /// <summary>
    /// Function to append the action buttons to the toolbar buttons.
    /// </summary>
    /// <param name="btn">Button structure as defined in the StructDef object.</param>
    private void CreateActionButtons(TStructDef.ButtonStruct btn, TStructDef strObj)
    {
        string actConfirmMsg = string.Empty;
        string actRem = string.Empty;
        string manRem = string.Empty;
        for (int m = 0; m <= strObj.actions.Count - 1; m++)
        {
            TStructDef.ActionStruct actn = (TStructDef.ActionStruct)strObj.actions[m];
            if (actn.actname == btn.action)
            {
                actConfirmMsg = actn.actdesc;
                actRem = actn.actRem;
                manRem = actn.manRem;
                break;
            }
        }
        btnFunction = string.Empty;
        btnStyle = "handCur";

        btnFunction = " onclick=\"javascript:CallAction('" + btn.action + "','" + btn.fileupload + "','" + actConfirmMsg + "','" + actRem + "','" + manRem + "');\" ";
        if (!util.IsImageAvailable(btn.image))
            btn.image = "";

        string btnId = "actbtn_" + btn.action;

        if (!string.IsNullOrEmpty(btn.caption) & !string.IsNullOrEmpty(btn.image))
        {
            //Display Image with caption as hint.
            //actionBarBtns.Add("<li><a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " class=\"action \" title='" + btn.hint + "' >" + btn.hint + "</a></li>");
            actionBarBtns.Add("<a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " title='" + btn.hint + "' class=\"action btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btn.hint + "</a>");
        }
        else if (!string.IsNullOrEmpty(btn.image) & string.IsNullOrEmpty(btn.caption))
        {
            //Display only image
            //actionBarBtns.Add("<li><a href=\"javascript:void(0)\" id='" + btnId + "'  " + btnFunction.ToString() + " class=\"action \" title='" + btn.hint + "' >" + btn.hint + "</a></li>");
            actionBarBtns.Add("<a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " title='" + btn.hint + "' class=\"action btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btn.hint + "</a>");
        }
        else if (!string.IsNullOrEmpty(btn.caption) & string.IsNullOrEmpty(btn.image))
        {
            //Display button with Caption
            actbtncount++;
            //Check the image icon of png format with the same name as caption and apply
            if (util.IsImageAvailable("toolicons\\actionbtns\\" + btn.caption.ToLower() + ".png"))
            {
                //actionBarBtns.Add("<li><a href=\"javascript:void(0)\" id='attach' " + btnFunction.ToString() + " class=\"action \" title='" + btn.caption + "' class='" + btnStyle + "' data-hint='" + btn.hint + "'>" + btn.caption + "</a></li>");
                actionBarBtns.Add("<a href=\"javascript:void(0)\" id='attach' " + btnFunction.ToString() + " title='" + btn.caption + "' data-hint='" + btn.hint + "' class=\"action btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btn.caption + "</a>");
            }
            else
                //actionBarBtns.Add("<li><a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " class=\"action \" title=\"" + btn.caption + "\" data-hint =\"" + btn.hint + "\">" + btn.caption + "</a></li>");
                actionBarBtns.Add("<a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " title='" + btn.caption + "' data-hint='" + btn.hint + "' class=\"action btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btn.caption + "</a>");

        }
        else if (!string.IsNullOrEmpty(btn.hint) && string.IsNullOrEmpty(btn.image))
        {
            //actionBarBtns.Add("<li><a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " class=\"action \" title=\"" + btn.hint + "\" >" + btn.hint + "</a></li>");
            actionBarBtns.Add("<a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " title='" + btn.caption + "' data-hint='" + btn.hint + "' class=\"action btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\">" + btn.hint + "</a>");
        }
        else
        {
            //throw error
            actionBarBtns.Add("");
        }

        if (strObj.grpActBtns == null || strObj.grpActBtns == "")
        {
            toolBarBtns.Add(actionBarBtns[actionBarBtns.Count - 1].ToString().Replace("class=\"action \"", "class=\"action singleaction \""));
            actionBarBtns.RemoveAt(actionBarBtns.Count - 1);
        }
    }

    #endregion

    #region CreateTstructHtml

    /// <summary>
    /// Function to return the tstruct header html.
    /// </summary>
    /// <param name="strObj"></param>
    public void CreateHeaderHtml(TStructDef strObj, CacheManager cacheMgr)
    {
        tstHeader.Length = 0;
        string navButtons = string.Empty;
        string adirction = "left";
        if (Session["language"].ToString() == "ARABIC")
            adirction = "right";
        //if (language.ToLower() == "arabic")
        //    tstHeader.Append("<div id='backforwrdbuttons' class='hide backbutton " + adirction + " '><span class='navLeft icon-arrows-right-double handCur' onclick=\"javascript:BackForwardButtonClicked(\"back\");\" id='" + "goback" + "' alt=\"Previous Page\" title=\"Click here to go back\" ></span></div>");
        //else
        //    tstHeader.Append("<div id='backforwrdbuttons' class='hide backbutton " + adirction + " '><span class='navLeft icon-arrows-left-double-32 handCur' onclick=\"javascript:BackForwardButtonClicked(\"back\");\" id='" + "goback" + "'  title=\"Click here to go back\"></span></div>");

        ////searchBar.InnerHtml = string.Empty;
        ///* searchBar.InnerHtml =*/
        //tstHeader.Append("<div id='backtohm' class='backbutton " + adirction + "'><a><img id='" + "homeico" + "' src='../AxpImages/icons/24X24/home.png'  alt=\"Go to List\" title=\"Go to List\" class=\"handCur\"/></a></div>");
        if (!strObj.IsObjCustomHtml || theModeDesign)
            tstHeader.Append(strObj.GetHeaderHtml(tstCaption));
        else if (cacheMgr.tstCustomHTML.Split('@').Length > 1)
            tstHeader.Append(cacheMgr.tstCustomHTML.Split('@')[0]);
        //divmainheader.InnerHtml = string.Empty;
        //divmainheader.InnerHtml = strObj.headerHtml.ToString();
        //bbcrumb.InnerHtml = strObj.headerHtml.ToString();
    }

    /// <summary>
    /// Function to construct the DC Html for all the Dc's in the structdef.
    /// </summary>
    /// <param name="strObj"></param>
    private void CreateDcHtml(TStructDef strObj)
    {
        //if (strObj.tstLayout == Constants.TILE)
        //    CreateTileDcHtml(strObj);
        //else
        {
            int dcCount = strObj.pagePositions.Count;
            for (int i = 0; i < dcCount; i++)
            {
                dcHtml.Append(strObj.GetPagePositionHtml(strObj.pagePositions[i].ToString()));

                if (i == dcCount - 1)
                    dcHtml.Append("<div id=\"waitDiv\" class=\"page-loader rounded-2 bg-radial-gradient\"><div class=\"loader-box-wrapper d-flex bg-white p-20 shadow rounded\"><span class=\"loader\"></span></div></div>");
            }
            tstTabScript.Append(strObj.GenerateTabScript(strObj));
        }
    }

    private void CreateTileDcHtml(TStructDef strObj)
    {
        StringBuilder strDcHtml = new StringBuilder();
        StringBuilder popGridHtml = new StringBuilder();
        int dcCnt = 0;
        int totWidth = 0;
        for (int i = 0; i < strObj.dcs.Count; i++)
        {
            TStructDef.DcStruct dc = (TStructDef.DcStruct)strObj.dcs[i];
            if (!dc.ispopgrid)
            {
                dcCnt++;
                if (dcCnt % 2 == 0)
                {
                    totWidth += dc.dcWidht;
                    strDcHtml.Append("<div id=\"dvRightWrapper" + dc.frameno + "\" style=\"width:" + dc.dcWidht + "px;\" class=\"Rightdiv\" >");
                    strDcHtml.Append(strObj.GetPagePositionHtml(strObj.pagePositions[i].ToString()) + "</div>");
                    // Close wrapper row div
                    dcHtml.Append("<div id=dcRow" + dc.frameno + " style=\"width:" + totWidth + "px;\" class=\"TileWrapper\">");
                    dcHtml.Append(strDcHtml + "</div>");
                    strDcHtml = new StringBuilder();
                    //Add clear div
                    strDcHtml.Append("<div class=\"clear\"></div>");
                    totWidth = 0;
                }
                else
                {
                    totWidth += dc.dcWidht + 10;
                    //TODO:Add a expand and collapse button for the wrapper
                    //dcHtml.Append("<div id=dcRow" + dc.frameno + " class=\"TileWrapper\">");
                    strDcHtml.Append("<div id=\"dvLeftWrapper" + dc.frameno + "\" style=\"width:" + dc.dcWidht + "px;\" class=\"Leftdiv\" >");
                    strDcHtml.Append(strObj.GetPagePositionHtml(strObj.pagePositions[i].ToString()) + "</div>");
                    if (strObj.dcs.Count == dc.frameno)
                    {
                        dcHtml.Append("<div id=dcRow" + dc.frameno + " style=\"width:" + totWidth + "px;\" class=\"TileWrapper\">");
                        dcHtml.Append(strDcHtml + "</div>");
                    }
                }
            }
            else
            {
                popGridHtml.Append(strObj.GetPagePositionHtml(strObj.pagePositions[i].ToString()));
            }
        }
        if (strObj.dcs.Count % 2 != 0)
            dcHtml.Append("</div>");

        dcHtml.Append(popGridHtml.ToString());
        tstTabScript.Append(strObj.GenerateTabScript(strObj));
    }

    #endregion

    #region WriteHtml
    /// <summary>
    /// Function to write the html on to the response page.
    /// </summary>
    /// <param name="strObj"></param>
    private void WriteHtml(TStructDef strObj, CacheManager cacheMgr)
    {
        string docHt = strObj.docHeight.ToString() + "px";
        int dcTop = strObj.docHeight;

        if (AxpTstButtonStyle == "old")
        {
            //Check for submit and cancel while writing from html
            if (ShowSubCanBtns() && !theModeDesign)
            {
                if ((strObj.dcs.Count >= 0) && Session["MobileView"] != null)
                {
                    submitCancelBtns.Append("<div id = 'dvsubmitCancelBtns' class=''><center><table><input id = 'btnSaveTst' type=button class='saveTask btn btn-primary hotbtn'  onclick='javascript:FormSubmit();'  value='Submit' title='Submit'>&nbsp;&nbsp;<input id = 'New' type=button class='newTask btn btn-primary coldbtn'  onclick='javascript:NewTstruct();'  value='Reset' title='Reset'>&nbsp;&nbsp;</table></center></div>");
                    //dvFooter.Visible = true;
                    heightframe.Attributes.Add("data-submitcancel", "true");
                }
            }
            else
            {
                //dvFooter.Visible = false;
                heightframe.Attributes.Add("data-submitcancel", "false");
                submitCancelBtns = ClearStringBuilders(submitCancelBtns);
            }
            dvFooterHtml = submitCancelBtns.ToString();
        }

        // Code for attachments div ' 
        attHtml.Append("<div id=\"attachment-overlay\" class=\"col-12 pb-2 frmAtt attachmentsContainer d-none\" ><div class=\"mb-1 mb-xl-2 shadow-sm overflow-auto\">");
        attHtml.Append("<div id=\"dropzone_tstattach\" class=\"dropzone dz-clickabhle py-3\"><div class=\"d-flex dropzone-panel\"><a class=\"dropzone-select fs-4 w-100 text-start\"><span class=\"material-icons material-icons-style material-icons-1 float-start me-2\">attach_file</span> Drop files here or click to upload</a><span class=\"material-icons material-icons-style material-icons-1 float-end d-none fileuploadmore \" data-bs-toggle=\"popover\" data-bs-sanitize=\"false\" data-bs-placement=\"bottom\" data-bs-html=\"true\">more</span>");
        attHtml.Append("<div class=\"position-relative ms-2 d-none\" id=\"hattachCounter\"><span class=\"text-gray-600 spanAttCount fs-4 border border-gray-300 badge bg-white py-1 px-2 shadow\"></span><span class=\"bullet bullet-dot bg-success h-6px w-6px position-absolute translate-middle top-0 start-50 animation-blink\"></span></div>");
        attHtml.Append("<a class=\"dropzone-remove-all btn btn-sm btn-light-primary d-none\">Remove All</a></div><div class=\"d-flex gap-3 dropzone-items wm-200px d-none\"><div class=\"d-flex align-items-center rounded-2 p-3 btn btn-active-light-primary dropzone-item\" style=\"display: none\"><div class=\"dropzone-file\"><div class=\"dropzone-filename\" title=\"some_image_file_name.jpg\"><span data-dz-name>some_image_file_name.jpg</span></div><div class=\"dropzone-error\" data-dz-errormessage></div></div><div class=\"dropzone-progress d-none\"><div class=\"progress\"><div class=\"progress-bar bg-primary\" role=\"progressbar\" aria-valuemin=\"0\" aria-valuemax=\"100\" aria-valuenow=\"0\" data-dz-uploadprogress></div></div></div><div class=\"dropzone-toolbar\"><span class=\"dropzone-delete\" data-dz-remove><span class=\"material-icons material-icons-style material-icons-2 float-end dropzoneItemDeleteHeader\">clear</span></span></div></div></div></div>");
        attHtml.Append("</div></div>");

        if (isTstInCache)
        {
            dcHtml = new StringBuilder();
            dcHtml.Append(htmlFromCache);
        }

        string gridElementsHeightScript = "<script type='text/javascript'>SetGridElementsHeight();</script>";
        tstHTML.Append("<div id='wbdrHtml'>" + attHtml.ToString() + dcHtml.ToString() + "</div>" + gridElementsHeightScript);
        //wBdr.InnerHtml = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + Constants.WIZARD_TEMPLATE + tstHTML.ToString();
        wBdr.InnerHtml = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + tstHTML.ToString();
    }
    #endregion

    private string GetParamValues()
    {
        StringBuilder paramXml = new StringBuilder();
        for (int qs = 0; qs <= paramNames.Count - 1; qs++)
        {
            paramValues[qs] = CheckSpecialChars(paramValues[qs].ToString());
            if (paramNames[qs].ToString().ToLower() != "transid" && paramNames[qs].ToString().ToLower() != "themode" && paramNames[qs].ToString().ToLower() != "hltype" && paramNames[qs].ToString().ToLower() != "torecid" && paramNames[qs].ToString().ToLower() != "layout" && paramNames[qs].ToString().ToLower() != "act" && paramNames[qs].ToString().ToLower() != "loadformdata" && paramNames[qs].ToString().ToLower() != "trromode")
            {
                paramXml.Append("<" + paramNames[qs].ToString() + ">" + paramValues[qs].ToString() + "</" + paramNames[qs].ToString() + ">");
            }
        }
        return paramXml.ToString();
    }

    #region LoadStructureData
    /// <summary>
    /// Function to check if the transaction is new or to load a selected transaction.
    /// </summary>
    /// <param name="structRes">Structure result stored in the struct def object.</param>
    private void LoadStructure(TStructDef strObj)
    {
        string structRes = strObj.structRes;
        string loadXml = string.Empty;
        string loadRes = string.Empty;
        bool isDraft = false;
        string draftID = string.Empty;
        string draftLoadStr = string.Empty;
        string AxDisplayAutoGenVal = Session["AxDisplayAutoGenVal"].ToString();
        string isTstParamLoad = string.Empty;
        UpdateParamsFrmQueryStr();
        string dvRefreshFromLoadModern = "false";
        bool wsPerfFormLoad = strObj.wsPerfFormLoadCall;
        bool isDoFormload = false;
        if (Session["DraftName"] != null)
        {
            isDraft = true;
            draftID = Session["DraftName"].ToString();
            draftLoadStr = "IsDraftLoad = true;";
            Session["DraftName"] = null;
        }
        else
        {
            string queryString = string.Empty;
            queryString = GetParamValues();
            string visibleDCs = string.Empty;
            visibleDCs = strObj.GetVisibleDCs();
            logobj.CreateLog("    Recordid = " + rid, sid, fileName, "");

            if (rid != "0")
            {
                LoadRecidFromList();
                string ConfigDataAttr = string.Empty;
                string AxVarAttr = string.Empty;
                string dbmemvarsXML = string.Empty;
                dbmemvarsXML = util.GetDBMemVarsXML(transId);
                string cdVarsXML = util.GetConfigDataVarsXML(transId);
                if (cdVarsXML == string.Empty)
                {
                    try
                    {
                        if (HttpContext.Current.Session["configparam_transids"] != null && HttpContext.Current.Session["configparam_transids"].ToString() != "")
                        {
                            string[] dbVarformloadList = HttpContext.Current.Session["configparam_transids"].ToString().Split(',');
                            var isDbVarExist = dbVarformloadList.AsEnumerable().Where(x => x == transId).ToList();
                            if (isDbVarExist.Count > 0)
                            {
                                ConfigDataAttr = " configdata_cached='f' ";
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    GetConfigDataFromCache(transId);
                }

                if (dbmemvarsXML == string.Empty)
                {
                    try
                    {
                        if (HttpContext.Current.Session["forms_transids"] != null && HttpContext.Current.Session["forms_transids"].ToString() != "")
                        {
                            string[] dbVarformloadList = HttpContext.Current.Session["forms_transids"].ToString().Split(',');
                            var isDbVarExist = dbVarformloadList.AsEnumerable().Where(x => x == transId).ToList();
                            if (isDbVarExist.Count > 0)
                            {
                                AxVarAttr = " axvars_cached='f' ";
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    dbmemvarsXML = dbmemvarsXML.Replace("<dbmemvars>", "").Replace("</dbmemvars>", "");
                    ParseAxMemVarResult(dbmemvarsXML, transId, false);
                }


                string imagefromdb = "false";
                if (Session["AxpSaveImageDb"] != null)
                    imagefromdb = Session["AxpSaveImageDb"].ToString();
                loadXml = loadXml + "<root" + actstr + AxVarAttr + ConfigDataAttr + " ispegedit='" + isPegEdit + "' imagefromdb='" + imagefromdb + "' axpapp='" + proj + "' sessionid='" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' transid='" + transId + "' recordid='" + rid + "' dcname='" + visibleDCs + "' trace='" + errorLog + "'>";
                logobj.CreateLog("    Loading Tstruct.", sid, fileName, "");
                loadXml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + dbmemvarsXML + cdVarsXML + Session["axUserVars"].ToString();
                loadXml += "</root>";
                //loadXml = util.ReplaceImagePath(loadXml);//AxpImagePath needs to be replaced with empty if the "save Image in DB" key exist in advance configuration. 
                stTime = DateTime.Now;
                //Axp_RefreshExpsOnLoad
                loadRes = objWebServiceExt.CallLoadDataWS(transId, loadXml, structRes, rid, proj);
                requestProcess_logtime += loadRes.Split('♠')[0];
                loadRes = loadRes.Split('♠')[1];
                if (ConfigDataAttr != "")
                    loadRes = GetConfigDataVars(strObj, loadRes, transId);
                if (AxVarAttr != "")
                    loadRes = GetAxMemVars(strObj, loadRes, transId);
                wsPerfFormLoad = true;
                edTime = DateTime.Now;
                strLogTime.Append("LoadData-" + stTime.Subtract(edTime).TotalMilliseconds.ToString());
                formLogTime = formLogTime + float.Parse(edTime.Subtract(stTime).TotalMilliseconds.ToString());
                //logobj.CreateLog("LoadData-" + stTime.Subtract(edTime).TotalMilliseconds.ToString(), sid, "LogTimeTaken", "");               
                Page.Title = "Load Tstruct";
                Session["axp_lockonrecid"] = transId + "~" + rid;

                dynamic flGblExistingKeys = util.GetFormLoadKey(transId, strObj.FormLoadGlobalVarNode);
                util.SetFormLoadData("true", transId, flGblExistingKeys);
                hdnisHtmlLsLoad.Value = "*loaddata*";
            }
            else
            {
                bool callDFforCOnfigData = false;// IF WSFLD is false and the form have Configdata then need to call forcelly formload service.
                try
                {
                    if (HttpContext.Current.Session["configparam_transids"] != null && HttpContext.Current.Session["configparam_transids"].ToString() != "")
                    {
                        string[] dbVarformloadList = HttpContext.Current.Session["configparam_transids"].ToString().Split(',');
                        var isDbVarExist = dbVarformloadList.AsEnumerable().Where(x => x == transId).ToList();
                        if (isDbVarExist.Count > 0)
                        {
                            callDFforCOnfigData = true;
                            if (actstr == "" && !strObj.wsPerfFormLoadCall)
                                actstr = " act='open'";
                        }
                    }
                }
                catch (Exception ex) { }

                if (queryString != "" && actstrType.ToLower() == "load")
                    isTstParamLoad = "true";
                else if (queryString != "" && actstrType.ToLower() == "open")
                    isTstParamLoad = "false";
                loadXml = loadXml + "<root" + actstr + formFoadData + " axpapp='" + proj + "' sessionid='" + sid + "' transid='" + transId + "' recordid='" + rid + "' dcname='" + visibleDCs + "' trace='" + errorLog + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'>";
                logobj.CreateLog("    Opening Tstruct.", sid, fileName, "");
                loadXml += queryString;
                loadXml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString();
                loadXml += "</root>";
                DateTime stTime11 = DateTime.Now;
                if (queryString != "" && actstrType.ToLower() == "open" && !strObj.wsPerfFormLoadCall)//Any parameter is select + sql, act is open and WSFLD is false then need to forcelly call the dofomrload to get back fill field values.
                {
                    XmlDocument xmlDocParam = new XmlDocument();
                    xmlDocParam.LoadXml("<root>" + queryString + "</root>");
                    XmlNode listNode = xmlDocParam.SelectSingleNode("//root");
                    foreach (XmlNode ndName in listNode.ChildNodes)
                    {
                        int pIndx = strObj.GetFieldIndex(ndName.Name);
                        if (pIndx > -1)
                        {
                            TStructDef.FieldStruct pfld = (TStructDef.FieldStruct)strObj.flds[pIndx];
                            if (pfld.isFieldSql == "True" && pfld.moe == "Select")
                            {
                                isTstParamLoad = "true";
                                break;
                            }
                        }
                    }
                }

                bool iswsPerfFGDcName = false;
                if (isTstParamLoad != "true" && !strObj.wsPerfFormLoadCall && strObj.wsPerfFGDcName != null && strObj.wsPerfFGDcName.Length > 0)
                {
                    string[] visibleDCsList = visibleDCs.Split(',');
                    var _visibleDcFlag = strObj.wsPerfFGDcName.Where(x => visibleDCsList.Contains(x)).ToList();
                    if (_visibleDcFlag != null && _visibleDcFlag.Count() > 0)
                    {
                        iswsPerfFGDcName = true;
                    }
                }

                // Call service
                if (isTstParamLoad == "true" || (isTstParamLoad == "false" && strObj.wsPerfFormLoadCall) || iswsPerfFGDcName)
                {
                    AxDisplayAutoGenVal = "false";
                    wsPerfFormLoad = true;
                    loadRes = objWebServiceExt.CallDoFormLoadWS(transId, loadXml, structRes);
                    requestProcess_logtime += loadRes.Split('♠')[0];
                    loadRes = loadRes.Split('♠')[1];
                    loadRes = GetConfigDataVars(strObj, loadRes, transId);
                    loadRes = GetAxMemVars(strObj, loadRes, transId);
                    HandleFormLoadErr(loadRes, queryString);
                }
                else if (strObj.wsPerfFormLoadCall || (!strObj.wsPerfFormLoadCall && callDFforCOnfigData))
                {
                    AxDisplayAutoGenVal = "false";
                    dvRefreshFromLoad.Visible = true;
                    dvRefreshFromLoadModern = "true";
                    wsPerfFormLoad = true;

                    dynamic flGblExistingKeys = util.GetFormLoadKey(transId, strObj.FormLoadGlobalVarNode);
                    string flKey = flGblExistingKeys.GetType().GetProperty("flKey").GetValue(flGblExistingKeys, null);
                    string matchflGlobalVar = flGblExistingKeys.GetType().GetProperty("matchflGlobalVar").GetValue(flGblExistingKeys, null);
                    if (flKey != "none" && matchflGlobalVar != "")
                        loadRes = util.GetFormLoadData(transId, flGblExistingKeys);
                    if (loadRes == string.Empty || loadRes == "true")
                    {
                        loadRes = objWebServiceExt.CallDoFormLoadWS(transId, loadXml, structRes);
                        requestProcess_logtime += loadRes.Split('♠')[0];
                        loadRes = loadRes.Split('♠')[1];
                        loadRes = GetConfigDataVars(strObj, loadRes, transId);
                        loadRes = GetAxMemVars(strObj, loadRes, transId);
                        HandleFormLoadErr(loadRes, queryString);
                        if (loadRes != "" && flKey != "none")
                        {
                            string flgvValue = util.SetFormLoadData(loadRes, transId, flGblExistingKeys);
                            strObj.FormLoadGlobalVarNode = flgvValue;
                            string fdKey = Constants.REDISTSTRUCT;
                            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                                fdKey = Constants.REDISTSTRUCTMOB;
                            FDW fdwObj = new FDW();
                            fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKey, transId), strObj, Constants.REDISTSTRUCT, schemaName);
                        }
                        else if (flKey == "none")
                        {
                            util.SetFormLoadData("true", transId, flGblExistingKeys);
                        }
                    }
                    else if (loadRes != string.Empty)
                    {
                        GetConfigDataFromCache(transId);
                        if (strObj.AxMemVarAvailable)
                            GetFormLoadAxMemVers(strObj, transId, queryString);
                    }
                }
                else if (!strObj.wsPerfFormLoadCall)
                {
                    try
                    {
                        util.SetFormLoadData("false", transId, "");

                        if (Session["forms_transids"] != null && Session["forms_transids"].ToString() != "")
                        {
                            string[] dbVarformloadList = Session["forms_transids"].ToString().Split(',');
                            var isDbVarExist = dbVarformloadList.AsEnumerable().Where(x => x == transId).ToList();
                            if (isDbVarExist.Count > 0)
                                GetFormLoadAxMemVers(strObj, transId, queryString);
                        }
                    }
                    catch (Exception ex) { }
                }


                DateTime edTime11 = DateTime.Now;
                strLogTime.Append("DoFormLoad-" + stTime11.Subtract(edTime11).TotalMilliseconds.ToString());
                formLogTime = formLogTime + float.Parse(edTime11.Subtract(stTime11).TotalMilliseconds.ToString());

                if ((!string.IsNullOrEmpty(queryString)))
                    Page.Title = "Load TStruct with QS";
                else
                    Page.Title = "Tstruct";
                hdnisHtmlLsLoad.Value = "*formload*";
            }

            isDoFormload = true;
            loadRes = loadRes.Trim();
            //loadRes = loadRes.Replace("\\n", "");
            loadRes = loadRes.Replace("\n", "");
            loadRes = loadRes.Replace("\\", ";bkslh");

            if (loadRes != "" && strObj.FEncryptFlag.Count > 0)
            {
                ASB.WebService objws = new ASB.WebService();
                loadRes = objws.AxpFieldDataDecrypt(loadRes, strObj.FEncryptFlag);
            }
        }
        string key = string.Empty;
        tstScript.Append("<script language='javascript' type='text/javascript' >var rSFlds='" + strObj.fastDataFlds + "'</script>");

        //The empty check for result has been removed since the tstruct data object will not be created.
        TStructData strDataObj = null;
        string attachJson = string.Empty;
        try
        {
            DateTime wstime = DateTime.Now;
            strDataObj = new TStructData(loadRes, transId, rid, strObj);
            GetTabDcHTMLOnFormLoad(strObj, strDataObj, rid);
            DateTime wetime = DateTime.Now;
            if (rid != "0")
            {
                strDataObj.loadDataRes = loadRes;
            }
            strLogTime.Append("TSTDATAOBJ-" + wstime.Subtract(wetime).TotalMilliseconds.ToString());
            //logobj.CreateLog("Tstruct data object-" + wstime.Subtract(wetime).TotalMilliseconds.ToString(), sid, "LogTimeTaken", "");

            key = util.GetTstDataId(transId);
            loadRes = loadRes.Replace("'", "&quot;");
            strDataObj.transid = transId.ToString();
            if (rid != "0" && strDataObj.AxGridAttNotExistList.Count > 0)
                tstScript.Append("<script language='javascript' type='text/javascript' >var AxGridAttNotExistList=['" + string.Join("','", strDataObj.AxGridAttNotExistList.ToArray()) + "'];</script>");

            //if a tstruct data is loading from another source for a new record then take the recordid as axp_recid1 column from json response
            try
            {
                var axp_recid1 = strDataObj.GetFieldValue("1", "axp_recid1");
                if (rid == "0" && axp_recid1 != string.Empty && axp_recid1 != "0")
                    rid = axp_recid1.ToString();
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Exception in Tstruct data get axp_recid1 from dataset :--- " + ex.StackTrace, HttpContext.Current.Session["nsessionid"].ToString(), "Exception-" + transId, "");
            }

            strDataObj.recordid = rid.ToString();
            Session.Add(key, strDataObj);
            hdnDataObjId.Value = key;
            if (HttpContext.Current.Session["isDupTab"] != null && HttpContext.Current.Session["isDupTab"].ToString().StartsWith("true-"))
            {
                string _thisDupTab = HttpContext.Current.Session["isDupTab"].ToString();
                _thisDupTab = _thisDupTab.Split('-')[1];
                HttpContext.Current.Session["isDupTab"] = "false";
                if (Session["tstivobjkey-duptab-" + _thisDupTab] != null && Session["tstivobjkey-duptab-" + _thisDupTab].ToString() != string.Empty)
                    Session["tstivobjkey-duptab-" + _thisDupTab] = Session["tstivobjkey-duptab-" + _thisDupTab].ToString() + "," + key;
                else
                    Session["tstivobjkey-duptab-" + _thisDupTab] = key;
            }
            else
            {
                if (Session["tstivobjkey"] != null && Session["tstivobjkey"].ToString() != string.Empty)
                    Session["tstivobjkey"] = Session["tstivobjkey"].ToString() + "," + key;
                else
                    Session["tstivobjkey"] = key;
            }
            GetImageArrays(strDataObj);
            //On loading a record, the format grid dc html will also be constructed and set to the div, 
            //since there is no format grid construction method in the client.
            if (rid != "0" && !isTstInCache)
                GetFormatGridHtml(strObj, strDataObj);

            if (strDataObj.attachDirPath != string.Empty && rid != "0" && strObj.tstAttachment == "True")
                attachJson = LoadAttachFromLoc(transId, rid, strDataObj, loadRes);
            if (rid != "0" && strObj.tstAttachment == "True" && loadRes != "")
                LoadAttachFromDbFile(loadRes);
        }
        catch (Exception ex)
        {
            if (util.sysErrorlog)
                logobj.CreateLog("Exception in Tstruct data object creation :--- " + ex.StackTrace, HttpContext.Current.Session["nsessionid"].ToString(), "Exception-" + transId, "");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "controlDimmer", "closeParentFrame();", true);
            Session["DraftHTML"] = Constants.DRAFT_REFRESH;
            Response.Redirect("err.aspx?errmsg=" + ex.Message.Replace(Environment.NewLine, ""));
        }
        if (AxDisplayAutoGenVal != null)
            hdnShowAutoGenFldValue.Value = AxDisplayAutoGenVal;

        //Removeing script tag code
        var localloadRes = loadRes;
        localloadRes = util.CheckScriptTag(localloadRes);
        StringBuilder tmpSB = new StringBuilder();
        tmpSB.Append("<script language='javascript' type='text/javascript'>");
        tmpSB.Append("var LoadResult = '");
        tmpSB.Append(localloadRes + attachJson);
        tmpSB.Append("'; </script>");
        loadResult += tmpSB.ToString();
        loadResult = loadResult.Replace("\n", "");
        StringBuilder xHtm = new StringBuilder();
        xHtm.Append("<table class='tsthiddentable'>");
        xHtm.Append("<tr><td colspan=2>");
        xHtm.Append("<input type=hidden id='recordid000F0' name='recordid000F0' value='" + rid + "'>");
        xHtm.Append("<input type=hidden id='hdnTraceValue' name='hdnTraceValue' value='" + errorLog.ToString() + "'>");
        xHtm.Append("<INPUT type='hidden' id='html_transid000F0' name='html_transid000F0' value='" + transId + "'>");
        xHtm.Append("<INPUT type='hidden' id='pickfld000F0' name='pickfld000F0' value=''>");
        xHtm.Append("</td></tr></table>");
        string strObjwsPerfFields = string.Empty;
        if (strObj.wsPerfFields != null)
            strObjwsPerfFields = string.Join("','", strObj.wsPerfFields.Split(','));
        string strObjwsPerfEvalExpClient = string.Empty;
        if (strObj.wsPerfEvalExpClient != null)
            strObjwsPerfEvalExpClient = string.Join("','", strObj.wsPerfEvalExpClient.Split(','));
        StringBuilder regTransIdRecId = new StringBuilder();
        regTransIdRecId.Append("<script language='javascript' type='text/javascript'>");
        regTransIdRecId.Append("transid = '" + transId + "';recordid = '" + rid + "';gl_language = '" + language + "';" + draftLoadStr + "var tstDataId='" + hdnDataObjId.Value + "';var axTheme='" + Session["themeColor"].ToString() + "';AxIsTstructCached =" + isTstructCached.ToString().ToLower() + "; displayAutoGenVal='" + AxDisplayAutoGenVal + "';var axpRefreshParent='" + axpRefreshParent + "';var axpRefSelect='" + axRefreshSelect + "';var axpRefSelectID='" + axRefreshSelectID + "';var axpSrcSelectID='" + axSrcSelectID + "';var axRefreshSelectType='" + axRefreshSelectType + "';var wsPerfFormLoadCall=" + wsPerfFormLoad.ToString().ToLower() + ";var wsPerfEnabled=" + strObj.wsPerfEnabled.ToString().ToLower() + ";var wsPerfFields=['" + strObjwsPerfFields + "'];");
        if (strObj.wsPerfFGDcName != null && strObj.wsPerfFGDcName.Count() > 0)
            regTransIdRecId.Append("var wsPerfFGDcName =['" + string.Join("','", strObj.wsPerfFGDcName) + "'];");
        if (wsPerfFormLoad == false || AxMemVarClient == "true" || ConfigDataClient == "true")
            regTransIdRecId.Append("var wsPerfEvalExpClient =['" + strObjwsPerfEvalExpClient + "'];");
        if (wsPerfFormLoad == false && isTstParamLoad == "false")
            regTransIdRecId.Append("var formParamFlds =['" + string.Join("','", paramNames.ToArray()) + "'];var formParamVals =['" + string.Join("','", clientParamValues.ToArray()) + "'];");
        regTransIdRecId.Append("AxOnApproveDisable=" + AxOnApproveDisable + ";AxOnReturnSave=" + AxOnReturnSave + ";AxOnRejectSave=" + AxOnRejectSave + ";AxOnRejectDisable=" + AxOnRejectDisable + ";");
        regTransIdRecId.Append("AxLogTimeTaken='" + AxLogTimeTaken + "';var headerAttachDir = '" + strDataObj.attachDir + "';");
        regTransIdRecId.Append("var isWizardTstruct = " + strObj.isWizardTstruct.ToString().ToLower() + "; var hideToolBar = " + strObj.hideToolBar.ToString().ToLower() + ";var dvRefreshFromLoadModern='" + dvRefreshFromLoadModern + "';");
        regTransIdRecId.Append("</script>");
        tstScript.Append(regTransIdRecId.ToString());
        if (isTstInCache)
            tstScript.Append(jsFromCache);

        tstScript.Append("<script language='javascript' type='text/javascript' >function setDocht(){ }</script>");
        tstScript.Append(loadResult + xHtm.ToString());

        if (isDoFormload)
        {
            string ImgVals = "";
            if (rid != "0")
                ImgVals = GetImageArraysNew(strDataObj);
            string tstVarScript = string.Empty;
            tstVarScript = wsPerfFormLoad.ToString().ToLower() + ";";
            tstVarScript += strObj.wsPerfEvalExpClient + ";";// strObjwsPerfEvalExpClient + ";";
            tstVarScript += strDataObj.attachDir;

            //string thisResult = key + "*$*" + loadRes;
            string thisResult = string.Empty;
            if (rid != "0")
                thisResult = key + "*$*";
            else
                thisResult = key + "*$*" + loadRes;

            string dcHmtl = GetTabDcHTMLFormLoad(strObj, strDataObj, "0");
            string wbdrHtml = string.Empty;
            wbdrHtml = GetTstHtml(transId, strObj, errorLog);
            string tabDCStatus = string.Join(",", strObj.tabDCStatus.ToArray());
            string tstCancelled = strObj.tstCancelled;

            string _Thisresult = wbdrHtml + "*$*" + dcHmtl + "*$*" + tstVarScript.ToString() + "*$*" + ImgVals + "*$*" + tabDCStatus + "*$*" + AxDisplayAutoGenVal + "*$*" + tstCancelled;
            _Thisresult += "*$*" + strObj.ArrFFieldHidden.ToString() + "*$*" + strObj.ArrFFieldReadOnly.ToString() + "*$*" + thisResult;

            hdnTstHtmLs.Value = _Thisresult;
        }
        else
        {
            hdnTstHtmLs.Value = "";
        }

        logobj.CreateLog("Loading tstruct.aspx completed", sid, fileName, "");
        logobj.CreateLog("End Time : " + DateTime.Now.ToString(), sid, fileName, "");
    }

    private void ClearDcHasDataRows(TStructDef strObj)
    {
        for (int i = 0; i < strObj.dcs.Count; i++)
        {
            TStructDef.DcStruct dc = (TStructDef.DcStruct)strObj.dcs[i];
            if (dc.isgrid)
            {
                dc.DCHasDataRows = false;
                strObj.dcs[i] = dc;
            }
        }
    }

    private void GetTabDcHTMLOnFormLoad(TStructDef strObj, TStructData strDataObj, string recordId)
    {
        var _designMode = false;
        if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
        {
            _designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
        }
        string tabDcsHtml = string.Empty;
        for (int i = 0; i < strObj.tabDCs.Count; i++)
        {
            if (strObj.tabDCStatus[i].ToString() == "0")
            {
                int _dcNo = int.Parse(strObj.tabDCs[i].ToString());
                TStructDef.DcStruct tabDc = (TStructDef.DcStruct)strObj.dcs[_dcNo - 1];
                if (recordId == "0" && strObj.wsPerfFGDcName != null && _designMode != true)
                {
                    string[] flGridDc = strObj.wsPerfFGDcName;
                    var loaddcFG = flGridDc.Where(d => d.ToLower() == "dc" + strObj.tabDCs[i]).ToList();
                    if (loaddcFG.Count > 0 && !tabDc.DCHasDataRows)//Fillgrid dc hasdata is true then this condition should not execute.
                        continue;
                }
                string temptabHTML = string.Empty;
                string tempDcDesignJson = string.Empty;
                try
                {
                    temptabHTML = strObj.GetTabDcHTML(Convert.ToInt32(strObj.tabDCs[i]), strDataObj, "false");
                    if (strDataObj.tstStrObj.axdesignJObject.dcLayout == null || strDataObj.tstStrObj.axdesignJObject.dcLayout == "default")
                    {
                        Dc curDc = strDataObj.tstStrObj.axdesignJObject.dcs.FirstOrDefault(elm => elm.dc_id == strObj.tabDCs[i].ToString());
                        tempDcDesignJson = new JavaScriptSerializer().Serialize(curDc);
                    }
                    else
                    {
                        Dc curDc = strDataObj.tstStrObj.axdesignJObject.newdcs.FirstOrDefault(elm => elm.dc_id == strObj.tabDCs[i].ToString());
                        tempDcDesignJson = new JavaScriptSerializer().Serialize(curDc);
                    }
                }
                catch (Exception ex)
                {

                }
                if (tabDcsHtml == string.Empty)
                    tabDcsHtml += strDataObj.AxDepArrays.ToString() + "*♦*" + strDataObj.GetMasterRowFlds() + "*♠**♠*" + temptabHTML + "*♠*" + tempDcDesignJson;
                else
                    tabDcsHtml += "*♠♠*" + strDataObj.AxDepArrays.ToString() + "*♦*" + strDataObj.GetMasterRowFlds() + "*♠**♠*" + temptabHTML + "*♠*" + tempDcDesignJson;
            }
        }
        hdnTabHtml.Value = tabDcsHtml;
    }

    public static string getLoaddraftJson(string draftID, ref TStructData strDataObj)
    {
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        strDataObj = fObj.TstructDataFromRedis(draftID, true);
        if (strDataObj == null)
        {
            throw new Exception("REDIS might have reset/Flushed. Please Try Again.");
        }
        ASBExt.WebServiceExt objExt = new ASBExt.WebServiceExt();
        //strDataObj.tstStrObj = strObj;
        strDataObj.objWebServiceExt = objExt;
        strDataObj.sessionid = HttpContext.Current.Session["nsessionid"].ToString();
        strDataObj.IsDraftObj = true;
        string loadRes = strDataObj.CreateJsonForDraft();
        LogFile.Log logObj = new LogFile.Log();
        logObj.CreateLog(loadRes, HttpContext.Current.Session.SessionID, "LoadDraft", "new");
        return loadRes;
    }

    private void HandleFormLoadErr(string loadRes, string queryString)
    {
        if (loadRes.Contains("\"error\"") == true && loadRes.Contains(Constants.SESSIONEXPMSG))
        {
            Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG + "*♠*" + requestProcess_logtime);
            return;
        }
        else if (loadRes.Contains("\"error\"") == true && loadRes.Contains(Constants.ERAUTHENTICATION))
        {
            Response.Redirect(util.ERRPATH + Constants.ERAUTHENTICATION + "*♠*" + requestProcess_logtime);
            return;
        }
    }

    private void LoadRecidFromList()
    {
        Dictionary<int, string> lvRecordListing = new Dictionary<int, string>();
        lvRecordListing = util.GetlvRecordList(transId);
        if (lvRecordListing != null)
        {
            try
            {
                if (lvRecordListing != null)
                {
                    var recordkeyValuePair = lvRecordListing.Single(x => x.Value.IndexOf(rid) > -1);
                    int recordKey = recordkeyValuePair.Key;
                    Session["lvRecordKey"] = recordKey;
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog(ex.Message, sid, "Exception in LoadStructure ListViewNavigation Details", "new");
            }
        }
    }

    private void UpdateParamsFrmQueryStr()
    {
        for (int qn = 1; qn <= Request.QueryString.Count - 1; qn++)
        {
            if (Request.QueryString.AllKeys[qn] == null)
                continue;
            if (Request.QueryString.Keys[qn] == "axp_refresh")
            {
                axpRefreshParent = Request.QueryString.Get(qn);
            }
            else if (Request.QueryString.Keys[qn] == "AxRefSelect")
            {
                axRefreshSelect = Request.QueryString.Get(qn);
            }
            else if (Request.QueryString.Keys[qn] == "AxRefSelectID")
            {
                axRefreshSelectID = Request.QueryString.Get(qn);
            }
            else if (Request.QueryString.Keys[qn] == "AxSrcSelectID")
            {
                axSrcSelectID = Request.QueryString.Get(qn);
            }
            else if (Request.QueryString.Keys[qn] == "AxRefType")
            {
                axRefreshSelectType = Request.QueryString.Get(qn);
            }
            else
            {
                if (Request.QueryString.AllKeys[qn].ToLower() == "axfromhyperlink" || Request.QueryString.AllKeys[qn].ToLower() == "axpop" || Request.QueryString.AllKeys[qn].ToLower() == "axhyptstrefresh" || Request.QueryString.AllKeys[qn].ToLower() == "recpos" || Request.QueryString.AllKeys[qn].ToLower() == "pagetype" || Request.QueryString.AllKeys[qn].ToLower() == "curpage" || Request.QueryString.AllKeys[qn].ToLower() == "dynnavtst" || Request.QueryString.AllKeys[qn].ToLower() == "openeriv" || Request.QueryString.AllKeys[qn].ToLower() == "isduptab" || Request.QueryString.AllKeys[qn].ToLower() == "fromprocess" || Request.QueryString.AllKeys[qn].ToLower() == "ispegedit" || Request.QueryString.AllKeys[qn].ToLower() == "isiv" || Request.QueryString.AllKeys[qn].ToLower() == "axsplit" || Request.QueryString.AllKeys[qn].ToLower() == "hdnbelapstime" || Request.QueryString.AllKeys[qn].ToLower() == "reqproc_logtime" || Request.QueryString.AllKeys[qn].ToLower() == "hltype" || Request.QueryString.AllKeys[qn].ToLower() == "torecid" || Request.QueryString.AllKeys[qn].ToLower() == "act" || Request.QueryString.AllKeys[qn].ToLower() == "dummyload" || Request.QueryString.AllKeys[qn].ToLower() == "loadformdata" || Request.QueryString.AllKeys[qn].ToLower() == "trromode" || Request.QueryString.AllKeys[qn].ToLower() == "ivdatakey")
                    continue;

                string strParams = string.Empty;
                if (Request.QueryString.AllKeys[qn].ToLower() == "istsivparamcache" && Request.QueryString.Get(qn) == "true")
                {
                    if (Session["AxIvParamsForTstLoad-" + transId] != null)
                    {
                        strParams = Session["AxIvParamsForTstLoad-" + transId].ToString();
                        Session.Remove("AxIvParamsForTstLoad-" + transId);
                    }
                    if (strParams != string.Empty)
                    {
                        string[] arrParams = strParams.Split('&');
                        foreach (string _ptname in arrParams)
                        {
                            if (_ptname != "")
                            {
                                paramNames.Add(_ptname.Split('=')[0]);
                                string val = string.Empty;
                                val = _ptname.Split('=')[1];
                                val = val.Replace("--.--", "&");
                                val = val.Replace("amp;", "&");

                                //val = val.Replace("%2b", "+");
                                val = util.CheckReverseUrlSpecialChars(val);
                                val = util.ReverseCheckSpecialChars(val);
                                paramValues.Add(val);
                                if (val.Contains("'")) val = val.Replace("'", "%27");
                                val = val.Replace(";bkslh", "%5C");
                                val = val.Replace("\\", "%5C");
                                clientParamValues.Add(val);
                            }
                        }
                    }
                }
                else
                {
                    // eliminate Name from querystring            
                    paramNames.Add(Request.QueryString.Keys[qn]);
                    string val = string.Empty;
                    val = Request.QueryString.Get(qn);
                    val = val.Replace("--.--", "&");
                    val = val.Replace("amp;", "&");

                    //val = val.Replace("%2b", "+");
                    val = util.CheckReverseUrlSpecialChars(val);
                    val = util.ReverseCheckSpecialChars(val);
                    paramValues.Add(val);
                    if (val.Contains("'")) val = val.Replace("'", "%27");
                    val = val.Replace(";bkslh", "%5C");
                    val = val.Replace("\\", "%5C");
                    clientParamValues.Add(val);
                }
            }
        }
    }

    //Function to construct the image arrays for all the images in the tstruct with values from the tstruct data object.
    private void GetImageArrays(TStructData tstData)
    {
        StringBuilder strImgArr = new StringBuilder();
        strImgArr.Append("<script language='javascript' type='text/javascript'>");
        for (int i = 0; i < tstData.imageFldNames.Count; i++)
        {
            if (tstData.imageFldSrc.Count > 0)
            {
                strImgArr.Append("imgNames[" + i + "]='" + tstData.imageFldNames[i].ToString() + "';");
                string src = tstData.imageFldSrc[i].ToString();
                src = src.Replace("\\", ";bkslh");
                strImgArr.Append("imgSrc[" + i + "]='" + src + "';");
            }
        }
        strImgArr.Append("</script>");
        loadResult += strImgArr.ToString();
    }

    /// <summary>
    /// Function to construct the html for the format grid dc on Loading a record.
    /// </summary>
    /// <param name="strObj"></param>
    /// <param name="tstData"></param>
    private void GetFormatGridHtml(TStructDef strObj, TStructData tstData)
    {
        StringBuilder strDcHtml = new StringBuilder();
        for (int i = 0; i < strObj.visibleDCs.Count; i++)
        {
            int dcNo = Convert.ToInt32(strObj.visibleDCs[i].ToString());
            int dcIndNo = strObj.dcsPositionIndex.IndexOf(dcNo.ToString());
            if (strObj.IsDcFormatGrid(dcIndNo, dcNo))
            {
                strDcHtml.Append(strObj.GetTabDcHTML(dcNo, tstData, "false"));
            }
        }

        dvFormatDc.InnerHtml = strDcHtml.ToString();
    }


    #endregion

    #endregion

    #region Get ConfigData from FormLoad
    private string GetConfigDataVars(TStructDef objSts, string resJson, string transId)
    {
        string result = resJson;
        try
        {
            string configDataJson = string.Empty;
            if (resJson.TrimStart().StartsWith("{")) //Json
            {
                try
                {
                    string[] orgResult = resJson.Split(new[] { "$*$" }, StringSplitOptions.None);
                    resJson = orgResult[0];//0: Formload result,1: ConfigData
                    if (orgResult.Length >= 2)
                    {
                        configDataJson = orgResult[1];
                    }
                }
                catch (Exception ex) { }
                if (configDataJson != string.Empty)
                {
                    ConfigDataClient = "true";
                    ParseConfigDataResult(configDataJson, transId);
                }

                result = resJson;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("GetConfigDataVars -" + ex.Message + "- FormLoad Result-" + resJson, HttpContext.Current.Session.SessionID, "GetConfigDataVars", "new");
            result = resJson;
        }
        return result;
    }


    public void ParseConfigDataResult(string configDataJson, string transId)
    {
        try
        {
            StringBuilder cdParamlist = new StringBuilder();
            if (configDataJson != string.Empty)
            {
                configDataJson = Regex.Replace(configDataJson, "<", "&lt;");
                configDataJson = Regex.Replace(configDataJson, ">", "&gt;");
                string configVars = "<configdata>" + configDataJson + "</configdata>";
                try
                {
                    string schemaname = string.Empty;
                    if (HttpContext.Current.Session["dbuser"] != null)
                        schemaname = HttpContext.Current.Session["dbuser"].ToString();
                    string user = HttpContext.Current.Session["user"].ToString();
                    string fdKey = Constants.CONFIGDATAVARSFORMLOAD;
                    FDW fdwObj = new FDW();
                    bool isCached = fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKey, transId), configVars, Constants.CONFIGDATAVARSFORMLOAD, schemaname);
                    if (!isCached)
                        Session["configdata_" + transId] = configVars;
                }
                catch (Exception exc)
                {
                    Session["configdata_" + transId] = configVars;
                }

                dynamic dynJson = JsonConvert.DeserializeObject(configDataJson);
                string varDataType = string.Empty;
                int paramCnt = 0;
                foreach (var item in dynJson.configdata)
                {
                    string nodeName = item.Name;
                    string nodeVal = item.First.Value == null ? "" : item.First.Value.ToString();
                    nodeVal = Regex.Replace(nodeVal, "\"", "&quot;");
                    cdParamlist.Append("AxCdParameters[" + paramCnt + "]=" + "\"" + nodeName + "♠" + nodeVal + "\";");
                    paramCnt++;
                }
                string AxMem_Scripts = "<script language='javascript' type='text/javascript' >" + cdParamlist.ToString() + "</script>";
                tstScript.Append(AxMem_Scripts);
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("ParseConfigDataResult -" + ex.Message + "- JSON-" + configDataJson, HttpContext.Current.Session.SessionID, "ParseConfigDataResult", "new");
        }
    }

    public void GetConfigDataFromCache(string transId)
    {
        try
        {
            StringBuilder cdParamlist = new StringBuilder();
            string configDataJson = string.Empty;
            configDataJson = util.GetConfigDataVarsXML(transId);
            if (configDataJson != string.Empty)
            {
                configDataJson = configDataJson.Replace("<configdata>", "").Replace("</configdata>", "");
                dynamic dynJson = JsonConvert.DeserializeObject(configDataJson);
                string varDataType = string.Empty;
                int paramCnt = 0;
                foreach (var item in dynJson.configdata)
                {
                    string nodeName = item.Name;
                    string nodeVal = item.First.Value == null ? "" : item.First.Value.ToString();
                    nodeVal = Regex.Replace(nodeVal, "\"", "&quot;");
                    cdParamlist.Append("AxCdParameters[" + paramCnt + "]=" + "\"" + nodeName + "♠" + nodeVal + "\";");
                    paramCnt++;
                }
                string AxMem_Scripts = "<script language='javascript' type='text/javascript' >" + cdParamlist.ToString() + "</script>";
                tstScript.Append(AxMem_Scripts);
                ConfigDataClient = "true";
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("GetConfigDataFromCache -" + ex.Message, HttpContext.Current.Session.SessionID, "GetConfigDataFromCache", "new");
        }
    }

    #endregion

    #region Get AxMemVar from Formload
    private string GetAxMemVars(TStructDef objSts, string resJson, string transId)
    {
        string result = resJson;
        try
        {
            string memvarJson = string.Empty;
            string memvarFunction = string.Empty;
            if (resJson.TrimStart().StartsWith("{")) //Json
            {
                try
                {
                    string[] orgResult = resJson.Split(new[] { "*#*" }, StringSplitOptions.None);
                    resJson = orgResult[0];//0: Formload result,1: Memvar result
                    if (orgResult.Length >= 2)
                    {
                        memvarJson = orgResult[1];
                    }
                }
                catch (Exception ex) { }
                if (memvarJson != string.Empty)
                {
                    objSts.AxMemVarAvailable = true;
                    AxMemVarClient = "true";
                    ParseAxMemVarResult(memvarJson, transId);
                }

                result = resJson;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("GetAxMemVars -" + ex.Message + "- FormLoad Result-" + resJson, HttpContext.Current.Session.SessionID, "GetAxMemVars", "new");
            result = resJson;
        }
        return result;
    }

    public void ParseAxMemVarResult(string memvarJson, string transId, bool isNotFromLoadData = true)
    {
        try
        {
            StringBuilder amParamlist = new StringBuilder();
            if (memvarJson != string.Empty)
            {
                if (isNotFromLoadData)
                {
                    string dbmemvars = "<dbmemvars>" + memvarJson + "</dbmemvars>";
                    //Session["dbmemvars_" + transId] = dbmemvars;
                    try
                    {
                        string schemaname = string.Empty;
                        if (HttpContext.Current.Session["dbuser"] != null)
                            schemaname = HttpContext.Current.Session["dbuser"].ToString();
                        string user = HttpContext.Current.Session["user"].ToString();
                        string fdKey = Constants.DBMEMVARSFORMLOAD;
                        FDW fdwObj = new FDW();
                        bool iscahced = fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKey, transId, user), dbmemvars, Constants.DBMEMVARSFORMLOAD, schemaname);
                        if (!iscahced)
                            Session["dbmemvars_" + transId] = dbmemvars;
                    }
                    catch (Exception exc)
                    {
                        Session["dbmemvars_" + transId] = dbmemvars;
                    }
                }

                dynamic dynJson = JsonConvert.DeserializeObject(memvarJson);
                string varDataType = string.Empty;
                int paramCnt = 0;
                foreach (var item in dynJson[0])
                {
                    string nodeName = item.Name;
                    string nodeVal = item.First.Value == null ? "" : item.First.Value.ToString();
                    nodeVal = Regex.Replace(nodeVal, "\"", "&quot;");
                    nodeName = nodeName.Remove(nodeName.Length - 1, 1);
                    amParamlist.Append("AxMemParameters[" + paramCnt + "]=" + "\"" + nodeName + "~" + nodeVal + "\";");
                    paramCnt++;
                }
                string AxMem_Scripts = "<script language='javascript' type='text/javascript' >" + amParamlist.ToString() + "</script>";
                tstScript.Append(AxMem_Scripts);
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("ParseAxMemVarResult -" + ex.Message + "- JSON-" + memvarJson, HttpContext.Current.Session.SessionID, "ParseAxMemVarResult", "new");
        }
    }

    private void GetFormLoadAxMemVers(TStructDef strObj, string transId, string amQueryString)
    {
        try
        {
            LogFile.Log logobj = new LogFile.Log();
            string proj = HttpContext.Current.Session["project"].ToString();
            string sid = HttpContext.Current.Session["nsessionid"].ToString();
            string fileName = "FormLoadAxMemVers-" + transId;
            string errorLog = logobj.CreateLog("Get FormLoad AxMemVers.", sid, fileName, "new");
            //string loadXml = "<root axpapp='" + proj + "' sessionid='" + sid + "' axmemfunc='" + strObj.AxMemVarFunction + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
            string loadXml = "<root axpapp='" + proj + "' sessionid='" + sid + "' transid='" + transId + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
            loadXml += amQueryString;
            loadXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
            loadXml += "</root>";

            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            string axMemRes = objWebServiceExt.CallAxMemVarAvailable(transId, loadXml);
            try
            {
                string[] orgResult = axMemRes.Split(new[] { "*#*" }, StringSplitOptions.None);
                axMemRes = orgResult[1];//0: Time,1: Memvar result
                if (axMemRes != string.Empty)
                    AxMemVarClient = "true";
            }
            catch (Exception ex) { }
            ParseAxMemVarResult(axMemRes, transId);
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("GetFormLoadAxMemVers -" + ex.Message + "- QueryString-" + amQueryString, HttpContext.Current.Session.SessionID, "GetFormLoadAxMemVers", "new");
        }
    }

    #endregion

    #endregion

    #region General Functions


    /// <summary>
    /// Function to include the js files in the aspx page.
    /// </summary>
    private void IncludeJsFiles()
    {
        string projName = HttpContext.Current.Session["Project"].ToString();
        for (int i = 0; i < customObj.jsFiles.Count; i++)
        {
            string[] jsFileStr = customObj.jsFiles[i].ToString().Split('¿');
            string tid = jsFileStr[0].ToString().ToLower();
            string fileName = jsFileStr[1].ToString();
            if (transId.ToLower() == tid)
            {
                HtmlGenericControl js = new HtmlGenericControl("script");
                js.Attributes["type"] = "text/javascript";
                string path = "../" + projName + "/" + fileName;
                js.Attributes["src"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }

        for (int j = 0; j < customObj.jsGlobalFiles.Count; j++)
        {
            HtmlGenericControl js = new HtmlGenericControl("script");
            js.Attributes["type"] = "text/javascript";
            string path = "../" + projName + "/" + customObj.jsGlobalFiles[j].ToString();
            js.Attributes["src"] = path;
            ScriptManager1.Controls.Add(js);

        }

        for (int i = 0; i < customObj.cssFiles.Count; i++)
        {
            string[] jsFileStr = customObj.cssFiles[i].ToString().Split('¿');
            string tid = jsFileStr[0].ToString().ToLower();
            string fileName = jsFileStr[1].ToString();
            if (transId.ToLower() == tid)
            {
                HtmlGenericControl js = new HtmlGenericControl("link");
                js.Attributes["type"] = "text/css";
                js.Attributes["rel"] = "stylesheet";
                string path = "../" + projName + "/" + fileName;
                js.Attributes["href"] = path;
                ScriptManager1.Controls.Add(js);

            }
        }

        for (int i = 0; i < customObj.cssGlobalFiles.Count; i++)
        {
            HtmlGenericControl js = new HtmlGenericControl("link");
            js.Attributes["type"] = "text/css";
            js.Attributes["rel"] = "stylesheet";
            string path = "../" + projName + "/" + customObj.cssGlobalFiles[i].ToString();
            js.Attributes["href"] = path;
            ScriptManager1.Controls.Add(js);

        }
    }

    private void IncludeJsFilesNew()
    {
        string projName = HttpContext.Current.Session["Project"].ToString();
        if (AxpCustomJs != string.Empty && AxpCustomJs.ToLower() == "single")
        {
            FileInfo filtcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/tstruct/js/" + transId + ".js"));
            if (filtcustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("script");
                js.Attributes["type"] = "text/javascript";
                string path = "../" + projName + "/tstruct/js/" + transId + ".js?v=" + filtcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["src"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }
        else if (AxpCustomJs != string.Empty && AxpCustomJs.ToLower() == "all")
        {
            FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/tstruct/js/custom.js"));
            if (filcustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("script");
                js.Attributes["type"] = "text/javascript";
                string path = "../" + projName + "/tstruct/js/custom.js?v=" + filcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["src"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }

        if (AxpCustomCss != string.Empty && AxpCustomCss.ToLower() == "single")
        {
            FileInfo filtcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/tstruct/css/" + transId + ".css"));
            if (filtcsscustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("link");
                js.Attributes["type"] = "text/css";
                js.Attributes["rel"] = "stylesheet";
                string path = "../" + projName + "/tstruct/css/" + transId + ".css?v=" + filtcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["href"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }
        else if (AxpCustomCss != string.Empty && AxpCustomCss.ToLower() == "all")
        {
            FileInfo filcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/tstruct/css/custom.css"));
            if (filcsscustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("link");
                js.Attributes["type"] = "text/css";
                js.Attributes["rel"] = "stylesheet";
                string path = "../" + projName + "/tstruct/css/custom.css?v=" + filcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["href"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }

        try
        {
            FDW fdwObj = new FDW();
            if (HttpContext.Current.Session["dbuser"] != null)
                fdwObj.Initialize(HttpContext.Current.Session["dbuser"].ToString());
            string fdKey = Constants.REDISTSTCUSTOMJSCSS;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKey, transId), AxpCustomJs + "~" + AxpCustomCss, Constants.REDISTSTCUSTOMJSCSS, schemaName);
        }
        catch (Exception ex)
        {
        }

        if (AxpCustomJs == string.Empty && AxpCustomCss == string.Empty)
            IncludeJsFiles();
    }

    private void IncludeJsFilesDummy(string _transid)
    {
        string[] _jscssfile;
        try
        {
            string fdKey = Constants.REDISTSTCUSTOMJSCSS;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            string user = HttpContext.Current.Session["user"].ToString();
            if (fObj != null)
            {
                string jscss = fObj.StringFromRedis(util.GetRedisServerkey(fdKey, _transid, user), schemaName);
                _jscssfile = jscss.Split(new[] { "~" }, StringSplitOptions.None);

                string projName = HttpContext.Current.Session["Project"].ToString();
                if (_jscssfile.Count() > 0 && _jscssfile[0].ToLower() == "single")
                {
                    FileInfo filtcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/tstruct/js/" + _transid + ".js"));
                    if (filtcustom.Exists)
                    {
                        HtmlGenericControl js = new HtmlGenericControl("script");
                        js.Attributes["type"] = "text/javascript";
                        string path = "../" + projName + "/tstruct/js/" + _transid + ".js?v=" + filtcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                        js.Attributes["src"] = path;
                        ScriptManager1.Controls.Add(js);
                    }
                }
                else if (_jscssfile.Count() > 0 && _jscssfile[0].ToLower().ToLower() == "all")
                {
                    FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/tstruct/js/custom.js"));
                    if (filcustom.Exists)
                    {
                        HtmlGenericControl js = new HtmlGenericControl("script");
                        js.Attributes["type"] = "text/javascript";
                        string path = "../" + projName + "/tstruct/js/custom.js?v=" + filcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                        js.Attributes["src"] = path;
                        ScriptManager1.Controls.Add(js);
                    }
                }

                if (_jscssfile.Count() > 0 && _jscssfile[1].ToLower().ToLower() == "single")
                {
                    FileInfo filtcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/tstruct/css/" + _transid + ".css"));
                    if (filtcsscustom.Exists)
                    {
                        HtmlGenericControl js = new HtmlGenericControl("link");
                        js.Attributes["type"] = "text/css";
                        js.Attributes["rel"] = "stylesheet";
                        string path = "../" + projName + "/tstruct/css/" + _transid + ".css?v=" + filtcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                        js.Attributes["href"] = path;
                        ScriptManager1.Controls.Add(js);
                    }
                }
                else if (_jscssfile.Count() > 0 && _jscssfile[1].ToLower().ToLower() == "all")
                {
                    FileInfo filcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/tstruct/css/custom.css"));
                    if (filcsscustom.Exists)
                    {
                        HtmlGenericControl js = new HtmlGenericControl("link");
                        js.Attributes["type"] = "text/css";
                        js.Attributes["rel"] = "stylesheet";
                        string path = "../" + projName + "/tstruct/css/custom.css?v=" + filcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                        js.Attributes["href"] = path;
                        ScriptManager1.Controls.Add(js);
                    }
                }

                if (_jscssfile.Count() == 0 || jscss == "~")
                {
                    customObj = Custom.Instance;
                    IncludeJsFiles();
                }
            }
        }
        catch (Exception) { }
    }


    /// <summary>
    /// function for replacing the special characters in a given string.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string CheckSpecialChars(string str)
    {
        str = Regex.Replace(str, "&", "&amp;");
        str = Regex.Replace(str, "<", "&lt;");
        str = Regex.Replace(str, ">", "&gt;");
        str = Regex.Replace(str, "'", "&apos;");
        str = Regex.Replace(str, "\"", "&quot;");

        return str;
    }

    /// <summary>
    /// function for handling session timeout.
    /// </summary>
    /// <remarks></remarks>
    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

    public void btnHtml_Click(object sender, EventArgs e)
    {

    }

    private Boolean ShowSubCanBtns()
    {
        Boolean ShowBtns = true;
        if (Session["AxShowSubmitCancel"] != null)
        {

            if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
            {
                ShowBtns = false;
                return ShowBtns;
            }
            string ShowSubmitCancelBtns = HttpContext.Current.Session["AxShowSubmitCancel"].ToString();
            if (ShowSubmitCancelBtns != string.Empty)
            {
                if (ShowSubmitCancelBtns.ToLower() == "false")
                    ShowBtns = false;
            }

        }
        return ShowBtns;

    }
    private void SetLangStyles()
    {
        if (Session["language"].ToString() == "ARABIC")
        {
            direction = "rtl";
            classdir = "right";
            //searchoverlay.Attributes["class"] = "arabicoverlay hide";
            //dvsrchclose.Attributes["style"] = "text-align: left;";
            //dvsrchfor.Attributes["style"] = "margin-right: 50px;";
            //dvsrchfor.Attributes["class"] = "right";
        }
        else
        {
            classdir = "left";
            //searchoverlay.Attributes["class"] = "overlay hide";
            //dvsrchclose.Attributes["style"] = "text-align: right;";
            //dvsrchfor.Attributes["style"] = "margin-left: 50px;";
            //dvsrchfor.Attributes["class"] = "left";
        }
    }

    #endregion

    #region Search Methods

    /// <summary>
    /// Function to fill the search drop down with fields.
    /// </summary>
    /// <param name="strObj"></param>
    private void FillSearchList(TStructDef strObj)
    {
        ddlSearch.Items.Clear();
        ddlSearch.Items.Add(new ListItem("--Select--", ""));
        for (int i = 0; i < strObj.searchDataCaptions.Count; i++)
        {
            if (strObj.searchDataCaptions[i].ToString() == "&nbsp")
                strObj.searchDataCaptions[i] = "";

            if (strObj.searchDataCaptions[i].ToString() != string.Empty)
                ddlSearch.Items.Add(new ListItem(strObj.searchDataCaptions[i].ToString(), strObj.searchDataNames[i].ToString()));
        }

        for (int j = 0; j < ddlSearch.Items.Count; j++)
        {
            string strSearch = string.Empty;
            if (Request.Form["ddlSearch"] != null)
                strSearch = Request.Form["ddlSearch"].ToString();
            if (ddlSearch.Items[j].Text == strSearch)
            {
                ddlSearch.SelectedIndex = j;
            }
        }
    }

    /// <summary>
    /// function to bind the gridview while searching the transactions.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="totRows"></param>
    /// <param name="curPageNo"></param>
    private void BindDataGrid(string searchResult, int totRows, string curPageNo)
    {
        grdSearchRes.Columns.Clear();
        DataSet ds = new DataSet();
        StringReader sr = new StringReader(searchResult);

        ds.ReadXml(sr);

        // Important : the datasource store in session as datatable. for paging and sorting

        // IMP : Create a new dataset - use clone - which create new structure then change
        // Column datatype to int, double,string and date - which is needed for Sorting
        int colCnt = 0;
        DataSet ds1 = new DataSet();
        ds1 = ds.Clone();
        foreach (DataColumn dc1 in ds1.Tables[0].Columns)
        {
            dc1.DataType = typeof(string);

            colCnt = colCnt + 1;
        }

        int rowNo = 0;
        foreach (DataRow dr1 in ds.Tables[0].Rows)
        {
            // Before import ds to ds1 change the row value from str to date while datacol type is date
            // rno for find row no and id for col ... make new date then attach to dr1 -datarow then import

            ds1.Tables[0].ImportRow(dr1);
            rowNo = rowNo + 1;
        }

        Session["order"] = ds1.Tables[0];
        int resRows = ds1.Tables[0].Rows.Count;

        if (ds1.Tables.Count > 0)
        {
            foreach (DataColumn dc in ds1.Tables[0].Columns)
            {
                BoundField field = new BoundField();
                //'initialiae the data field value
                field.DataField = dc.ColumnName;
                //'initialise the header text value
                field.HeaderText = dc.ColumnName;
                //' add newly created columns to gridview
                grdSearchRes.Columns.Add(field);
            }
        }
        string fldId = Request.Form["ddlSearch"];
        try
        {
            ds1 = customObj.axAfterSearch(transId, ds1, searchVal, fldId);
        }
        catch (Exception ex)
        {
        }
        if (ds1.Tables[0].Rows.Count < 1)
        {
            totRows = 0;
            pgCap.Visible = false;
            lvPage.Visible = false;
        }
        grdSearchRes.DataSource = ds1;

        // to change the header Name and set the column width
        int idx = 0;
        for (idx = 0; idx <= headNames.Count - 1; idx++)
        {
            if (idx == 0)
                grdSearchRes.Columns[idx].HeaderText = "Select";
            // For change the Column Heading from fld name to Caption
            else
                grdSearchRes.Columns[idx].HeaderText = headNames[idx].ToString();
        }

        grdSearchRes.DataBind();
        double pg = (int)totRows / (int)grdSearchRes.PageSize;
        int pg1 = (int)Math.Floor(pg);
        if ((totRows % grdSearchRes.PageSize) > 0)
        {
            pg1 += 1;
        }

        if (totRows > 0)
        {
            records.Text = "Total no. of records: " + totRows;
            pages.Text = " of " + pg1;
            //records.CssClass = "seartotrecords h5 my-3";
            pgCap.Visible = true;
            lvPage.Visible = true;
        }
        else
        {
            records.Text = lblNodata.Text;
            //records.CssClass = "searnorecords";
            pages.Text = "";
        }

        int pgNo = 0;
        if (curPageNo == "1")
        {
            lvPage.Items.Clear();
            for (pgNo = 1; pgNo <= pg1; pgNo++)
            {
                lvPage.Items.Add(pgNo.ToString());
            }
        }


    }

    /// <summary>
    /// Function to bind the resulting search data into the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSearchRes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        System.Data.DataRowView drv = default(System.Data.DataRowView);
        drv = (System.Data.DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (drv != null)
            {
                // first change col 1 to check box
                string catName = drv[0].ToString();
                // for change the content to component like check box or input box
                //to Remove checkbox from first column
                e.Row.Cells[0].CssClass = "";
                e.Row.Cells[0].Text = "<div class=\"form-check form-check-custom form-check-solid\"><input type=radio name='radioselect' value=" + catName + " class=\"form-check-input\" onclick=loadTstruct(this.value);></div>";
                int n = 0;
                for (n = 0; n <= e.Row.Cells.Count - 1; n++)
                {
                    if (e.Row.Cells[n].Text == "~!@*")
                    {
                        e.Row.Cells[n].Text = "";
                    }
                }
            }
        }
        //for NOWRAP in IE
        int m = 0;
        for (m = 0; m <= e.Row.Cells.Count - 1; m++)
        {
            if (e.Row.Cells[m].Text.Length > 0)
            {
                e.Row.Cells[m].Text = "<nobr>" + e.Row.Cells[m].Text + "</nobr>";
            }
        }
    }

    /// <summary>
    /// Handles pagination for search grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSearchRes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dtOrders = new DataTable();
        dtOrders = (DataTable)Session["order"];
        grdSearchRes.PageIndex = e.NewPageIndex;
        grdSearchRes.DataSource = dtOrders.DefaultView;
        grdSearchRes.DataBind();
    }

    /// <summary>
    /// Function to handle Pagination page changed event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lvPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        string pgNo = lvPage.SelectedValue;
        callWebservice(pgNo);
    }

    bool IsAllDigits(string s)
    {
        return s.All(char.IsDigit);
    }
    /// <summary>
    /// Function to call service for filling the grid with search result.
    /// </summary>
    /// <param name="pgno"></param>
    public void callWebservice(string pageNo)
    {
        string pageSize = string.Empty;
        grdSearchRes.Columns.Clear();
        string qs = queryStr;
        float f;
        headNames.Clear();

        //searchVal = Request.Form["searstr"].Replace("&", "&amp;");
        searchVal = Request.Form["hdnSearchStr"];
        searchVal = util.CheckReverseUrlSpecialChars(searchVal);
        searchVal = util.CheckSpecialChars(searchVal);

        fileName = "Search-" + transId;
        errorLog = logobj.CreateLog("Loading Search List.", sid, fileName, "new");
        pageSize = grdSearchRes.PageSize.ToString();
        try
        {
            pageSize = customObj.axBeforeSearch(transId, pageSize);
        }
        catch (Exception ex)
        {
        }

        TStructData _tstData = (TStructData)Session[hdnDataObjId.Value];
        if (_tstData != null)
        {
            int fIdx = strObj.GetFieldIndex(Request.Form["ddlSearch"]);
            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)strObj.flds[fIdx];
            //fld.datatype;

            ;
            if (fld.datatype != "Date/Time" && fld.datatype != "Numeric" && ((fld.datatype == "Character" && !(searchVal.GetType().Name == "String" || searchVal.GetType().Name == "Character")) || (fld.datatype != "Character" && searchVal.GetType().Name != fld.datatype)))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "searchInTstruct", "$('#records').text('Enter " + fld.datatype.ToLower() + " values')", true);
                return;
            }

            else if (fld.datatype == "Numeric" && (!IsAllDigits(searchVal) && !float.TryParse(searchVal, out f)))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "searchInTstruct", "$('#records').text('Enter " + fld.datatype.ToLower() + " values')", true);
                return;
            }

            string iXml = string.Empty;
            iXml = "<sqlresultset axpapp=\"" + proj + "\" transid=\"" + Session["transid"] + "\" sessionid=\"" + sid + "\" trace=\"" + errorLog + "\" pageno=\"" + pageNo + "\" pagesize=\"" + int.Parse(pageSize) + "\" appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'>";
            iXml = iXml + "<fields>" + qs + "</fields><searchfor>" + Request.Form["ddlSearch"] + "</searchfor><value>" + searchVal + "</value>";

            string _thisTransId = Session["transid"].ToString();
            string dbmemvarsXML = string.Empty;
            dbmemvarsXML = util.GetDBMemVarsXML(_thisTransId);
            string cdVarsXML = util.GetConfigDataVarsXML(_thisTransId);
            iXml = iXml + Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + dbmemvarsXML + cdVarsXML + Session["axUserVars"].ToString() + "</sqlresultset>";
            string res = string.Empty;

            //Call service
            res = objWebServiceExt.CallGetSearchValWS(transId, iXml, strObj.structRes);
            requestProcess_logtime += res.Split('♠')[0];
            res = res.Split('♠')[1];
            if (res.ToLower().Contains("ora-"))
            {
                string strErrMsg = "Error occurred(2). Please try again or contact administrator.";
                requestProcess_logtime += "Server - Error occurred(2). Please try again or contact administrator. ♦ ";
                Response.Redirect("err.aspx?errmsg" + strErrMsg + "*♠*" + requestProcess_logtime);
            }

            Session["srchdata"] = res;

            if ((res.IndexOf(Constants.ERROR) == -1))
            {
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1.LoadXml(res);

                XmlNode cNode = default(XmlNode);
                cNode = xmlDoc1.SelectSingleNode("//response");

                int totalRows = 0;
                if (pageNo == "1")
                {
                    XmlNode tnode = cNode.Attributes["totalrows"];
                    if (tnode == null)
                    {
                        totalRows = 0;
                    }
                    else
                    {
                        totalRows = Convert.ToInt32(tnode.Value);
                        cNode.Attributes.RemoveNamedItem("totalrows");
                    }
                    Session["s_noofpages"] = totalRows;
                }
                else
                {
                    totalRows = Convert.ToInt32(Session["s_noofpages"]);
                }

                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                cNode.WriteTo(xw);

                string ires2 = null;
                ires2 = sw.ToString();

                XmlDocument xmlDoc2 = new XmlDocument();
                XmlNodeList productNodes2 = default(XmlNodeList);
                XmlNodeList baseDataNodes2 = default(XmlNodeList);
                xmlDoc2.LoadXml(ires2);

                productNodes2 = xmlDoc2.SelectNodes("//row");

                int p = 0;
                foreach (XmlNode productNode2 in productNodes2)
                {
                    if (p > 0)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                    baseDataNodes2 = productNode2.ChildNodes;
                    foreach (XmlNode baseDataNode2 in baseDataNodes2)
                    {
                        headNames.Add(baseDataNode2.Attributes["cap"].Value);
                    }
                    p = p + 1;
                }

                //Remove attribute Cap
                XmlDocument xmlDoc3 = new XmlDocument();
                xmlDoc3.LoadXml(sw.ToString());

                XmlNodeList productNodes3 = default(XmlNodeList);
                XmlNodeList baseDataNodes3 = default(XmlNodeList);

                productNodes3 = xmlDoc3.SelectNodes("//row");

                foreach (XmlNode productNode3 in productNodes3)
                {
                    baseDataNodes3 = productNode3.ChildNodes;
                    foreach (XmlNode baseDataNode3 in baseDataNodes3)
                    {
                        baseDataNode3.Attributes.RemoveNamedItem("cap");
                    }
                }

                string nXml = null;
                nXml = xmlDoc3.OuterXml;

                if (nXml == "<response />")
                {
                    records.Text = lblNodata.Text;
                    //records.CssClass = "searnorecords";
                    grdSearchRes.Visible = false;
                    pgCap.Visible = false;
                    lvPage.Visible = false;
                    pages.Text = "";
                }
                else
                {
                    records.Text = string.Empty;
                    grdSearchRes.Visible = true;
                    BindDataGrid(nXml, totalRows, pageNo);
                }
            }
            else
            {
                if (util.sysErrorlog)
                {
                    logobj.CreateLog("Error in Search Tstruct Service :--- " + res, sid, fileName, "");
                }
                res = res.Replace(Constants.ERROR, string.Empty);
                res = res.Replace("</error>", string.Empty);
                res = res.Replace("\n", string.Empty);
                Response.Redirect(util.ERRPATH + res + "*♠*" + requestProcess_logtime);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "searchInTstruct", "if(jQuery('#grdSearchRes').length)bindUpdownEvents('grdSearchRes','single');", true);
            //  Page.ClientScript.RegisterStartupScript(this.GetType(), "searchInTstruct", "alert();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "searchInTstructds", "CheckSessionTimeout('" + Constants.DUPLICATESESS + "');", true);
        }
    }

    /// <summary>
    /// Function to fill the search result.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGo_Click(object sender, EventArgs e)
    {
        callWebservice("1");
    }

    #endregion

    protected void colBtn1_Click(object sender, EventArgs e)
    {

        Session["layoutstyle"] = "onecolumn";
        if (Request.QueryString["transid"] != null)
        {
            try
            {
                Response.Redirect("tstruct.aspx?transid=" + Request.QueryString["transid"].ToString(), true);
            }
            catch (ThreadAbortException ex)
            {​​​​​ 
             Thread.ResetAbort();
            }​​​​​
        }
    }
    protected void colBtn2_Click(object sender, EventArgs e)
    {
        Session["layoutstyle"] = "twocolumn";
        if (Request.QueryString["transid"] != null)
        {
            try
            {
                Response.Redirect("tstruct.aspx?transid=" + Request.QueryString["transid"].ToString(), true);
            }
            catch (ThreadAbortException ex)
            {​​​​​
             Thread.ResetAbort();
            }​​​​​
        }

    }

    #region pdf Docs
    private void parseXMLDoc(string ires)
    {
        if (ires != String.Empty)
        {
            try
            {
                string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
                ires = _xmlString + ires;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(ires);
                XmlDocument gcxmlDoc = new XmlDocument();
                XmlNodeList gcproductNodes;
                XmlNodeList gcbaseDataNodes;
                gcxmlDoc.LoadXml(ires);
                gcproductNodes = gcxmlDoc.SelectNodes("/" + transId + "_pdflist");
                string selectStr = "<select id=\"pdfFName\" name=\"fname\" class=\"combotem Family form-control input-sm form-select form-select-sm\">";
                foreach (XmlNode gcproductNode in gcproductNodes[0])
                {
                    gcbaseDataNodes = gcproductNodes[0].ChildNodes;
                    int tNo = 1;
                    if (gcproductNode.Name != "transid")
                    {
                        optStr = optStr + "<option value= " + gcproductNode.Attributes["source"].Value + "$" + tNo + " class=\"" + gcproductNode.InnerText + "\">" + gcproductNode.InnerText + "</option>";
                        tNo = tNo + 1;
                    }
                }
                selectStr += optStr + "</select>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myScript", "LoadPdfDDL('" + selectStr + "');", true);
            }
            catch (Exception ex)
            {
                logobj.CreateLog("parseXMLDoc -" + ex.Message + "--" + ires, HttpContext.Current.Session.SessionID, "parseXMLDoc", "new");
            }
        }
    }
    #endregion

    [WebMethod]
    public static string GetFilterFastData(string SessKey, string FldName, string FldValue, string fltValue)
    {
        string json = string.Empty;
        try
        {
            //FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            //json = fObj.GetAutoFilterFastData(SessKey, FldName, FldValue, fltValue);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("GetFilterFastData -" + ex.Message, HttpContext.Current.Session.SessionID, "GetFilterFastData", "new");
        }
        return json;
    }
    [WebMethod]
    public static string GetAutoCompleteData(string tstDataId, string FldName, string FltValue, ArrayList ChangedFields, ArrayList ChangedFieldDbRowNo, ArrayList ChangedFieldValues, ArrayList DeletedDCRows, string pageData, string fastdll, string fldNameAc, string refreshAC, string pickArrow, string parentsFlds, string rfSave, string IsApiFld, string tblSourceParams, string isTstHtmlLs, string ddlFldSqlParams, string ddlSqlPNames)
    {
        string requestProcess_logtime = string.Empty;
        ExecTrace ObjExecTrace = ExecTrace.Instance;
        ObjExecTrace.SetCurrentTime();

        string json = string.Empty;
        DateTime stTime = DateTime.Now;
        try
        {
            ASB.WebService objws = new ASB.WebService();
            json = objws.GetdllAutoComplete(tstDataId, FldName, FltValue, ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, pageData, fastdll, fldNameAc, refreshAC, pickArrow, parentsFlds, rfSave, IsApiFld, tblSourceParams, isTstHtmlLs, ddlFldSqlParams, ddlSqlPNames);
            if (json.IndexOf('♠') > -1)
            {
                requestProcess_logtime += json.Split('♠')[1];
                json = json.Split('♠')[0];
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get AutoComplete Data -" + ex.Message, sessID, "GetAutoCompleteData", "new");
        }
        DateTime etTime = DateTime.Now;

        requestProcess_logtime += ObjExecTrace.RequestProcessTime("Response", json);
        string serverprocesstm = ObjExecTrace.TotalServerElapsTime();
        json = json + "*♠*" + serverprocesstm + "*♠*" + requestProcess_logtime;
        return json;
    }
    [WebMethod]
    public static string GetWrkFlwCmmt(string tid, string rid)
    {
        string wrkXml = string.Empty;
        string wrkflwTblStr = string.Empty;
        wrkXml = "<root axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' transid='" + tid + "' recordid='" + rid + "' trace='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";

        string dbmemvarsXML = string.Empty;
        Util.Util utilwf = new Util.Util();
        dbmemvarsXML = utilwf.GetDBMemVarsXML(tid);
        string cdVarsXML = utilwf.GetConfigDataVarsXML(tid);
        wrkXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + dbmemvarsXML + cdVarsXML + HttpContext.Current.Session["axUserVars"].ToString();
        wrkXml += "</root>";
        string jsonText = string.Empty;
        try
        {
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            wrkflwTblStr = objWebServiceExt.CallViewCommentsWS(tid, wrkXml);
            DataSet ds = new DataSet();
            StringReader stringReader = new StringReader(wrkflwTblStr);
            ds.ReadXml(stringReader);
            DataTable dt = ds.Tables["row"];
            jsonText = JsonConvert.SerializeObject(dt);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get workflow comments -" + ex.Message, sessID, "GetWorkFolwComment", "new");
        }
        return jsonText;
    }


    public string LoadAttachFromLoc(string transId, string recId, TStructData strDataObj, string strloadRes)
    {
        string attachJson = string.Empty;
        if (strloadRes != "" && strloadRes.IndexOf("*$*{\"attachment\":[{") == -1)
        {
            string attachDirPath = strDataObj.attachDirPath;
            if (!attachDirPath.Contains(":\\") && !attachDirPath.StartsWith("\\"))
            {
                strDataObj.CreateDirectoryInCommonDir(ref attachDirPath);
            }
            try
            {
                DirectoryInfo dir = new DirectoryInfo(attachDirPath + "\\" + transId + "\\");
                string fileValues = string.Empty;
                //if (dir.Exists)
                //{
                FileInfo[] info = dir.GetFiles(recId + "-*.*");
                foreach (FileInfo FI in info)
                    fileValues += FI.ToString().Substring(FI.ToString().IndexOf("-") + 1) + ",";
                if (fileValues.Length > 0)
                {
                    fileValues = fileValues.TrimEnd(',');
                    attachJson = "*$*{\"attachment\":[{\"att\":\"" + fileValues + "\"}]}";
                }
                //}
            }
            catch (Exception ex)
            {
                string _path = attachDirPath + "\\" + transId + "\\";
                logobj.CreateLog("LoadAttachFromLoc - Path:" + _path + " Exception:" + ex.Message, HttpContext.Current.Session.SessionID, "tstattachmentpath", "new");
            }
        }
        return attachJson;
    }

    public void LoadAttachFromDbFile(string loadRes)
    {
        try
        {
            if (loadRes != "" && loadRes.Contains("*$*{\"attachment\":[{\"att\":\""))
            {
                string[] jsonSplit = loadRes.Split(new[] { "*$*" }, StringSplitOptions.None);
                foreach (var _jVal in jsonSplit)
                {
                    if (_jVal.Contains("{\"attachment\":[{\"att\""))
                    {
                        try
                        {
                            JObject message = JObject.Parse(_jVal.ToString());
                            string _fileNames = message["attachment"][0]["att"].ToString();
                            Session["tstHeaderAttachDbFileOnLoad"] = _fileNames;
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    private void getDesignedData(TStructDef strObj)
    {
        var designMode = false;
        if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
        {
            designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
        }
        string session_id = HttpContext.Current.Session["nsessionid"].ToString();
        string utl = HttpContext.Current.Session["utl"].ToString();
        string userName = HttpContext.Current.Session["username"].ToString();
        string nodeAccessToken = string.Empty;
        if (HttpContext.Current.Session["nodeAccessToken"] != null)
        {
            nodeAccessToken = HttpContext.Current.Session["nodeAccessToken"].ToString();
        }
        string appsesskey = HttpContext.Current.Session["AppSessionKey"].ToString();
        if (designHidden.Value == string.Empty && !designMode && nodeAccessToken != string.Empty)
        {
            designHidden.Value = strObj.GetPublishNodeApiCall(nodeAccessToken, session_id, utl, userName, appsesskey);
        }
        else if (designHidden.Value == string.Empty && designMode && nodeAccessToken != string.Empty)
        {
            designHidden.Value = strObj.GetSaveNodeApiCall(nodeAccessToken, session_id, utl, userName, appsesskey);
        }
        if (designHidden.Value == string.Empty)
        {
            designHidden.Value = strObj.axdesignJson;
        }
        strObj.axdesignJObject = strObj.getDesignObjectFromJson(designHidden.Value);
    }


    [WebMethod]
    public static string SavePublishDesign(string Transid, string DesignType, string Content, string SavedId)
    {
        string tstDesign = string.Empty;
        DBContext objdb = new DBContext();
        if (DesignType == "SAVE")
            tstDesign = objdb.SaveDesignJson(Transid, Content);
        else if (DesignType == "PUBLISH")
        {
            tstDesign = objdb.PublishDesignJson(Transid, SavedId);
            ClearCacheDesignKeys(Transid);
        }
        else if (DesignType == "SAVEPUBLISH")
        {
            tstDesign = objdb.SavePublishDesign(Transid, Content, SavedId);
            ClearCacheDesignKeys(Transid);
        }
        else if (DesignType == "RESET")
        {
            tstDesign = objdb.ResetDesignJson(Transid, SavedId);
            ClearCacheDesignKeys(Transid);
        }
        if (tstDesign == string.Empty && HttpContext.Current.Session["AxDirectDBException"] != null)
        {
            tstDesign = "error:" + HttpContext.Current.Session["AxDirectDBException"].ToString();
            HttpContext.Current.Session.Remove("AxDirectDBException");
        }
        return tstDesign;
    }
    public static void ClearCacheDesignKeys(string Transid)
    {
        try
        {
            FDW fdwObj = new FDW();
            if (HttpContext.Current.Session["dbuser"] != null)
                fdwObj.Initialize(HttpContext.Current.Session["dbuser"].ToString());
            string fdKey = Constants.REDISTSTRUCT;
            string fdKeyMob = Constants.REDISTSTRUCTMOB;
            string designKey = Constants.REDISTSTRUCTAXDESIGN;
            string designCustHtmlKey = Constants.REDISTSTRUCTAXCUSTHTML;
            string designREDISTSTHTMLLS = Constants.REDISTSTHTMLLS;
            string designREDISTSTHTMLLSMob = Constants.REDISTSTHTMLLSMob;
            //string fdkey1 = Constants.AXPAGETITLE;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            Util.Util utilObj = new Util.Util();
            //fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(fdkey1, Transid), "", false, schemaName);
            fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(fdKey, Transid), "", false, schemaName);
            fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(fdKeyMob, Transid), "", false, schemaName);
            fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designKey, Transid), "", false, schemaName);
            fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designCustHtmlKey, Transid), "", false, schemaName);
            fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designREDISTSTHTMLLS, Transid), "", false, schemaName);
            fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designREDISTSTHTMLLSMob, Transid), "", false, schemaName);
        }
        catch (Exception ex) { }
    }


    [WebMethod]
    public static string GetFormLoadValues(string key, string tstQureystr, string isDraft, string forceRefresh, string isDupTab, string isTstHtmlLs, string isFromDummytrue = "false")
    {
        string requestProcess_logtime = string.Empty;
        ExecTrace ObjExecTrace = ExecTrace.Instance;
        ObjExecTrace.SetCurrentTime();

        string result = string.Empty;
        LogFile.Log logobj = new LogFile.Log();
        Util.Util utils = new Util.Util();
        string queryString = string.Empty;
        string isTstParamLoad = string.Empty;
        string actstrType = string.Empty;
        string formFoadData = "";
        string _strParamName = string.Empty;
        string _strParamValue = string.Empty;
        string AxDisplayAutoGenVal = HttpContext.Current.Session["AxDisplayAutoGenVal"].ToString();
        bool wsPerfFormLoad = false;
        try
        {
            if (HttpContext.Current.Session["ExpFldsForAxVarsandCOnfig"] != null)
                HttpContext.Current.Session.Remove("ExpFldsForAxVarsandCOnfig");
            utils.TempAttaServerFiles();
            utils.DeleteKeyOnRefreshSave();
            string queryStringParamsData = QueryStringParams(tstQureystr);
            queryString = queryStringParamsData.Split('♠')[0];
            actstrType = queryStringParamsData.Split('♠')[1];
            formFoadData = queryStringParamsData.Split('♠')[3];
            _strParamName = queryStringParamsData.Split('♠')[4];
            _strParamValue = queryStringParamsData.Split('♠')[5];
            if (formFoadData != string.Empty)
                formFoadData = " loadformdata='" + formFoadData + "' ";
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utils.SESSTIMEOUT;
            if (isDupTab != "" && isDupTab.StartsWith("true-"))
                HttpContext.Current.Session["isDupTab"] = isDupTab;
            else
                HttpContext.Current.Session["isDupTab"] = "false";
            string _isTstLoadFromLSdelObj = string.Empty;
            if (isTstHtmlLs != "" && HttpContext.Current.Session[key] == null)
            {
                utils.DeleteTstIvObject(key.Split('_')[0]);
                _isTstLoadFromLSdelObj = key = utils.GetReGenTstDataObj(key, isTstHtmlLs);
                utils.tstIvObjUpdate(key);
            }
            TStructData tstData = (TStructData)HttpContext.Current.Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string transId = tstData.transID;
            string errorLog = logobj.CreateLog("Form Load Service", HttpContext.Current.Session["nsessionid"].ToString(), "FormData-" + transId, "new");
            DateTime eTime1 = DateTime.Now;
            TStructDef strObj = tstData.tstStrObj;
            wsPerfFormLoad = strObj.wsPerfFormLoadCall;
            if (queryString != "" && actstrType.ToLower() == "load")
                isTstParamLoad = "true";
            else if (queryString != "" && actstrType.ToLower() == "open")
                isTstParamLoad = "false";

            string actstr = " act='" + actstrType + "'";

            string rid = "0";
            bool callDFforCOnfigData = false;// IF WSFLD is false and the form have Configdata then need to call forcelly formload service.
            try
            {
                if (HttpContext.Current.Session["configparam_transids"] != null && HttpContext.Current.Session["configparam_transids"].ToString() != "")
                {
                    string[] dbVarformloadList = HttpContext.Current.Session["configparam_transids"].ToString().Split(',');
                    var isDbVarExist = dbVarformloadList.AsEnumerable().Where(x => x == transId).ToList();
                    if (isDbVarExist.Count > 0)
                    {
                        callDFforCOnfigData = true;
                        if (actstr == "" && !strObj.wsPerfFormLoadCall)
                            actstr = " act='open'";
                    }
                }
            }
            catch (Exception ex) { }
            string loadXml = string.Empty;
            loadXml += "<root" + actstr + formFoadData + " axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' transid='" + transId + "' recordid='" + rid + "' dcname='" + strObj.GetVisibleDCs() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
            loadXml += queryString;
            loadXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
            loadXml += "</root>";

            DateTime stTime11 = DateTime.Now;

            if (queryString != "" && actstrType.ToLower() == "open" && !strObj.wsPerfFormLoadCall)//Any parameter is select + sql, act is open and WSFLD is false then need to forcelly call the dofomrload to get back fill field values.
            {
                XmlDocument xmlDocParam = new XmlDocument();
                xmlDocParam.LoadXml("<root>" + queryString + "</root>");
                XmlNode listNode = xmlDocParam.SelectSingleNode("//root");
                foreach (XmlNode ndName in listNode.ChildNodes)
                {
                    int pIndx = strObj.GetFieldIndex(ndName.Name);
                    if (pIndx > -1)
                    {
                        TStructDef.FieldStruct pfld = (TStructDef.FieldStruct)strObj.flds[pIndx];
                        if (pfld.isFieldSql == "True" && pfld.moe == "Select")
                        {
                            isTstParamLoad = "true";
                            break;
                        }
                    }
                }
            }

            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            bool iswsPerfFGDcName = false;
            if (isTstParamLoad != "true" && !strObj.wsPerfFormLoadCall && strObj.wsPerfFGDcName != null && strObj.wsPerfFGDcName.Length > 0)
            {
                string visibleDCs = strObj.GetVisibleDCs();
                string[] visibleDCsList = visibleDCs.Split(',');
                var _visibleDcFlag = strObj.wsPerfFGDcName.Where(x => visibleDCsList.Contains(x)).ToList();
                if (_visibleDcFlag != null && _visibleDcFlag.Count() > 0)
                {
                    iswsPerfFGDcName = true;
                }
            }

            if ((isTstParamLoad == "true" || (isTstParamLoad == "false" && strObj.wsPerfFormLoadCall) || iswsPerfFGDcName) && isDraft != "true" && !isDraft.StartsWith("true♦"))
            {
                result = objWebServiceExt.CallDoFormLoadWS(transId, loadXml, strObj.structRes);
                requestProcess_logtime += result.Split('♠')[0];
                result = result.Split('♠')[1];
                result = GetConfigDataVarsNew(strObj, result, transId);
                result = GetAxMemVarsNew(strObj, result, transId);
                HandleFormLoadErrNew(result, queryString, requestProcess_logtime);
                wsPerfFormLoad = true;
                if (isTstParamLoad == "true")
                {
                    AxDisplayAutoGenVal = "true";
                }
                else
                {
                    string stsResult = result.Replace("*$*", "¿");
                    stsResult = stsResult.Split('¿').Last();
                    Util.Util utilities = new Util.Util();
                    string stsGlobal = utilities.ParseJSonResultNode(stsResult);

                    if (stsGlobal == string.Empty)
                    {
                        AxDisplayAutoGenVal = "false";
                    }
                    else
                    {
                        AxDisplayAutoGenVal = "true";
                    }
                }
            }
            else if (isDraft == "true")
            {
                wsPerfFormLoad = true;
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                var rediskey = (transId + "-" + HttpContext.Current.Session["user"].ToString() + Constants.UNIQUE_DRAFT_KEY_PHRASE);
                var draftallkeys = fObj.GetAllKeys(HttpContext.Current.Session["dbuser"].ToString() + "-" + rediskey);
                result = getLoaddraftJson(draftallkeys[0].ToString(), ref tstData);
            }
            else if (isDraft.StartsWith("true♦"))
            {
                string[] _isDraftList = isDraft.Split('♦');
                wsPerfFormLoad = true;
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                var rediskey = _isDraftList[1];
                var draftallkeys = fObj.GetAllKeys(rediskey);
                result = getLoaddraftJson(draftallkeys[0].ToString(), ref tstData);
            }

            else if (strObj.wsPerfFormLoadCall || (!strObj.wsPerfFormLoadCall && callDFforCOnfigData))
            {
                AxDisplayAutoGenVal = "false";
                dynamic flGblExistingKeys = utils.GetFormLoadKey(transId, strObj.FormLoadGlobalVarNode);
                string flKey = flGblExistingKeys.GetType().GetProperty("flKey").GetValue(flGblExistingKeys, null);
                string matchflGlobalVar = flGblExistingKeys.GetType().GetProperty("matchflGlobalVar").GetValue(flGblExistingKeys, null);
                if (flKey != "none" && matchflGlobalVar != "" && forceRefresh != "true")
                    result = utils.GetFormLoadData(transId, flGblExistingKeys);
                if (result == string.Empty || result == "true")
                {
                    result = objWebServiceExt.CallDoFormLoadWS(transId, loadXml, strObj.structRes);
                    requestProcess_logtime += result.Split('♠')[0];
                    result = result.Split('♠')[1];
                    result = GetConfigDataVarsNew(strObj, result, transId);
                    result = GetAxMemVarsNew(strObj, result, transId);
                    HandleFormLoadErrNew(result, queryString, requestProcess_logtime);
                    if (result != "" && flKey != "none")
                    {
                        string flgvValue = utils.SetFormLoadData(result, transId, flGblExistingKeys);
                        strObj.FormLoadGlobalVarNode = flgvValue;
                        string fdKey = Constants.REDISTSTRUCT;
                        if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                            fdKey = Constants.REDISTSTRUCTMOB;
                        string schemaName = string.Empty;
                        if (HttpContext.Current.Session["dbuser"] != null)
                            schemaName = HttpContext.Current.Session["dbuser"].ToString();
                        FDW fdwObj = new FDW();
                        fdwObj.SaveInRedisServer(utils.GetRedisServerkey(fdKey, transId), strObj, Constants.REDISTSTRUCT, schemaName);
                    }
                    else if (flKey == "none")
                    {
                        utils.SetFormLoadData("true", transId, flGblExistingKeys);
                    }
                }
                else if (result != string.Empty)
                {
                    GetConfigDataFromCacheNew(transId);
                    if (strObj.AxMemVarAvailable)
                        GetFormLoadAxMemVersNew(strObj, transId, queryString);
                }
            }
            else if (!strObj.wsPerfFormLoadCall)
            {
                try
                {
                    utils.SetFormLoadData("false", transId, "");

                    if (HttpContext.Current.Session["forms_transids"] != null && HttpContext.Current.Session["forms_transids"].ToString() != "")
                    {
                        string[] dbVarformloadList = HttpContext.Current.Session["forms_transids"].ToString().Split(',');
                        var isDbVarExist = dbVarformloadList.AsEnumerable().Where(x => x == transId).ToList();
                        if (isDbVarExist.Count > 0)
                            GetFormLoadAxMemVersNew(strObj, transId, queryString);
                    }
                }
                catch (Exception ex) { }
            }
            if (result != "")
            {
                result = result.Trim();
                result = result.Replace("\\n", "");
                result = result.Replace("\\", ";bkslh");
                if (result != "" && strObj.FEncryptFlag.Count > 0)
                {
                    ASB.WebService objws = new ASB.WebService();
                    result = objws.AxpFieldDataDecrypt(result, strObj.FEncryptFlag);
                }
            }
            TStructData strDataObj = null;
            strDataObj = new TStructData(result, transId, "0", strObj);
            try
            {
                var axp_recid1 = strDataObj.GetFieldValue("1", "axp_recid1");
                if (rid == "0" && axp_recid1 != string.Empty && axp_recid1 != "0")
                    rid = axp_recid1.ToString();
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Exception in Tstruct data get axp_recid1 from dataset :--- " + ex.StackTrace, HttpContext.Current.Session["nsessionid"].ToString(), "Exception-" + transId, "");
            }
            //if (isDraft == "false")
            //{
            //    if (isFromDummytrue == "true" && rid == "0")
            //    {
            //        string axpStructKey = Constants.AXCONFIGTSTRUCT;
            //        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            //        DataTable _axpConfigStr = fObj.DataTableFromRedis(utils.GetConfigCacheKey(axpStructKey, transId, "", HttpContext.Current.Session["AxRole"].ToString(), "ALL"));
            //        if (_axpConfigStr != null && _axpConfigStr.Rows.Count > 0)
            //        {
            //            string _AutosaveDraftt = string.Empty;
            //            var _AutosaveDraft = _axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "auto save draft")
            //       .Select(x => new { autosavedraft = x.Field<string>("PROPSVAL"), AutosaveDraftTime = x.Field<string>("PROPVALUE2") }).ToList();
            //            if (_AutosaveDraft.Count > 0)
            //            {
            //                _AutosaveDraftt = _AutosaveDraft[0].autosavedraft;

            //                if (_AutosaveDraftt == "true")
            //                {
            //                    string _schemaname = string.Empty;
            //                    if (HttpContext.Current.Session["dbuser"] != null)
            //                        _schemaname = HttpContext.Current.Session["dbuser"].ToString();
            //                    var redisDraftkey = transId + "-" + HttpContext.Current.Session["user"].ToString() + Constants.UNIQUE_DRAFT_KEY_PHRASE + strObj.tstCaption;
            //                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            //                    var redisDraftkeys = fdrObj.GetAllKeys(_schemaname + "-" + redisDraftkey);
            //                    if (redisDraftkeys.Count > 0)
            //                    {
            //                        string savedraftKeyCreatedtime = redisDraftkeys[0].ToString().Split('-').Last();
            //                        var Createdyear = savedraftKeyCreatedtime.Substring(0, 8);
            //                        var day = Createdyear.Substring(0, 2);
            //                        var month = Createdyear.Substring(2, 2);
            //                        var year = Createdyear.Substring(4, 4);
            //                        var rkeycompareYear = day + "-" + month + "-" + year;
            //                        var rKeyCompareTime = savedraftKeyCreatedtime.Substring(8, 4);
            //                        var hours = rKeyCompareTime.Substring(0, 2);
            //                        var mins = rKeyCompareTime.Substring(2, 2);
            //                        rKeyCompareTime = hours + ":" + mins;
            //                        savedraftKeyCreatedtime = rkeycompareYear + " " + rKeyCompareTime;
            //                        HttpContext.Current.Session["savedraftKeyCreatedtime"] = savedraftKeyCreatedtime;
            //                    }
            //                }
            //                else
            //                    Util.Util.DeletedraftRediskeys(transId);
            //            }
            //        }
            //        else
            //            Util.Util.DeletedraftRediskeys(transId);
            //    }
            //    else
            //        Util.Util.DeletedraftRediskeys(transId);
            //}
            string dcHmtl = GetTabDcHTMLFormLoad(strObj, strDataObj, rid, transId);
            key = utils.GetTstDataId(transId);
            result = result.Trim();
            result = result.Replace("\\n", "");
            result = result.Replace("\\", ";bkslh");
            result = result.Replace("'", "&quot;");
            strDataObj.transid = transId.ToString();
            strDataObj.recordid = rid.ToString();

            bool isTstPop = (HttpContext.Current.Session["isTstPop"] != null && HttpContext.Current.Session["isTstPop"].ToString() == "true") ? true : false;
            bool AxSplit = (queryStringParamsData.Split('♠')[2].ToLower() == "true") ? true : false;

            if (!isTstPop && !AxSplit)
                utils.DeleteTstIvObject(transId);
            else if (_isTstLoadFromLSdelObj != string.Empty)
                utils.DeleteTstIvObject(_isTstLoadFromLSdelObj.Split('_')[0]);
            HttpContext.Current.Session.Add(key, strDataObj);
            utils.tstIvObjUpdate(key);

            if (strDataObj.attachDirPath != string.Empty && rid != "0" && strObj.tstAttachment == "True")
                result += LoadAttachFromLocNew(transId, rid, strDataObj, result);
            string ImgVals = "";
            if (rid != "0")
                ImgVals = GetImageArraysNew(strDataObj);
            string tstVarScript = string.Empty;
            string strObjwsPerfEvalExpClient = string.Empty;
            string _strParamNamevals = string.Empty;
            string isAxVarAndConfigEnabled = "false";
            if (HttpContext.Current.Session["ExpFldsForAxVarsandCOnfig"] != null)
                isAxVarAndConfigEnabled = HttpContext.Current.Session["ExpFldsForAxVarsandCOnfig"].ToString();

            if ((strObj.wsPerfEvalExpClient != null && (wsPerfFormLoad == false || isDraft == "true")) || isAxVarAndConfigEnabled == "true")
                strObjwsPerfEvalExpClient = string.Join(",", strObj.wsPerfEvalExpClient.Split(','));
            if (wsPerfFormLoad == false && isTstParamLoad == "false")
                _strParamNamevals = _strParamName + ";" + _strParamValue;
            else
                _strParamNamevals = ";";
            tstVarScript = wsPerfFormLoad.ToString().ToLower() + ";";
            tstVarScript += strObjwsPerfEvalExpClient + ";";
            tstVarScript += strDataObj.attachDir + ";" + _strParamNamevals;

            string thisResult = key + "*$*" + result;
            DateTime sTime2 = DateTime.Now;
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));
            logobj.CreateLog("Form Load Service completed", HttpContext.Current.Session["nsessionid"].ToString(), "FormData-" + transId, "");
            string wbdrHtml = string.Empty;
            if (isFromDummytrue == "false")
                wbdrHtml = GetTstHtml(transId, strObj, errorLog);
            string tabDCStatus = string.Join(",", strObj.tabDCStatus.ToArray());
            string tstCancelled = strObj.tstCancelled;
            result = wbdrHtml + "*$*" + dcHmtl + "*$*" + tstVarScript.ToString() + "*$*" + ImgVals + "*$*" + tabDCStatus + "*$*" + AxDisplayAutoGenVal + "*$*" + tstCancelled;
            result += "*$*" + strObj.ArrFFieldHidden.ToString() + "*$*" + strObj.ArrFFieldReadOnly.ToString() + "*$*" + thisResult;
            requestProcess_logtime += ObjExecTrace.RequestProcessTime("Response", result);
            string serverprocesstm = ObjExecTrace.TotalServerElapsTime();
            result = result + "*♠♦*" + serverprocesstm + "*♠♦*" + requestProcess_logtime;
        }
        catch (Exception ex)
        {

        }
        return result;
    }
    private static string GetAxMemVarsNew(TStructDef objSts, string resJson, string transId)
    {
        string result = resJson;
        try
        {
            string memvarJson = string.Empty;
            string memvarFunction = string.Empty;
            if (resJson.TrimStart().StartsWith("{")) //Json
            {
                try
                {
                    string[] orgResult = resJson.Split(new[] { "*#*" }, StringSplitOptions.None);
                    resJson = orgResult[0];//0: Formload result,1: Memvar result
                    if (orgResult.Length >= 2)
                    {
                        memvarJson = orgResult[1];
                    }
                }
                catch (Exception ex) { }
                if (memvarJson != string.Empty)
                {
                    HttpContext.Current.Session["ExpFldsForAxVarsandCOnfig"] = "true";
                    objSts.AxMemVarAvailable = true;
                    ParseAxMemVarResultNew(memvarJson, transId);
                }

                result = resJson;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("GetAxMemVarsNew -" + ex.Message + "- FormLoad Result-" + resJson, HttpContext.Current.Session.SessionID, "GetAxMemVarsNew", "new");
            result = resJson;
        }
        return result;
    }

    public static void ParseAxMemVarResultNew(string memvarJson, string transId, bool isNotFromLoadData = true)
    {
        try
        {
            Util.Util utils = new Util.Util();
            StringBuilder amParamlist = new StringBuilder();
            if (memvarJson != string.Empty)
            {
                if (isNotFromLoadData)
                {
                    string dbmemvars = "<dbmemvars>" + memvarJson + "</dbmemvars>";
                    //Session["dbmemvars_" + transId] = dbmemvars;
                    try
                    {
                        string schemaname = string.Empty;
                        if (HttpContext.Current.Session["dbuser"] != null)
                            schemaname = HttpContext.Current.Session["dbuser"].ToString();
                        string user = HttpContext.Current.Session["user"].ToString();
                        string fdKey = Constants.DBMEMVARSFORMLOAD;
                        FDW fdwObj = new FDW();
                        bool iscahced = fdwObj.SaveInRedisServer(utils.GetRedisServerkey(fdKey, transId, user), dbmemvars, Constants.DBMEMVARSFORMLOAD, schemaname);
                        if (!iscahced)
                            HttpContext.Current.Session["dbmemvars_" + transId] = dbmemvars;
                    }
                    catch (Exception exc)
                    {
                        HttpContext.Current.Session["dbmemvars_" + transId] = dbmemvars;
                    }
                }

                dynamic dynJson = JsonConvert.DeserializeObject(memvarJson);
                string varDataType = string.Empty;
                int paramCnt = 0;
                foreach (var item in dynJson[0])
                {
                    string nodeName = item.Name;
                    string nodeVal = item.First.Value == null ? "" : item.First.Value.ToString();
                    nodeVal = Regex.Replace(nodeVal, "\"", "&quot;");
                    nodeName = nodeName.Remove(nodeName.Length - 1, 1);
                    amParamlist.Append("AxMemParameters[" + paramCnt + "]=" + "\"" + nodeName + "~" + nodeVal + "\";");
                    paramCnt++;
                }
                string AxMem_Scripts = "<script language='javascript' type='text/javascript' >" + amParamlist.ToString() + "</script>";
                HttpContext.Current.Session["ExpFldsForAxVarsandCOnfig"] = "true";
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("ParseAxMemVarResultNew -" + ex.Message + "- JSON-" + memvarJson, HttpContext.Current.Session.SessionID, "ParseAxMemVarResultNew", "new");
        }
    }

    private static void GetFormLoadAxMemVersNew(TStructDef strObj, string transId, string amQueryString)
    {
        try
        {
            LogFile.Log logobj = new LogFile.Log();
            string proj = HttpContext.Current.Session["project"].ToString();
            string sid = HttpContext.Current.Session["nsessionid"].ToString();
            string fileName = "FormLoadAxMemVers-" + transId;
            string errorLog = logobj.CreateLog("Get FormLoad AxMemVers.", sid, fileName, "new");
            string loadXml = "<root axpapp='" + proj + "' sessionid='" + sid + "' transid='" + transId + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
            loadXml += amQueryString;
            loadXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
            loadXml += "</root>";

            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            string axMemRes = objWebServiceExt.CallAxMemVarAvailable(transId, loadXml);
            try
            {
                string[] orgResult = axMemRes.Split(new[] { "*#*" }, StringSplitOptions.None);
                axMemRes = orgResult[1];//0: Time,1: Memvar result
            }
            catch (Exception ex) { }
            ParseAxMemVarResultNew(axMemRes, transId);
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("GetFormLoadAxMemVersNew -" + ex.Message + "- QueryString-" + amQueryString, HttpContext.Current.Session.SessionID, "GetFormLoadAxMemVersNew", "new");
        }
    }

    [WebMethod]
    public static void DeletedraftRediskeys(string key, string Isclear)
    {
        try
        {
            Util.Util.DeletedraftRediskeys(key);

        }
        catch (Exception ex)
        {

        }
    }

    [WebMethod]
    public static void DeletedraftRediskeysNew(string key, string keyNames)
    {
        try
        {
            Util.Util.DeletedraftRediskeysNew(key, keyNames);

        }
        catch (Exception ex)
        {

        }
    }

    private static void HandleFormLoadErrNew(string loadRes, string queryString, string requestProcess_logtime)
    {
        Util.Util objUtil = new Util.Util();
        if (loadRes.Contains("\"error\"") == true && loadRes.Contains(Constants.SESSIONEXPMSG))
        {
            HttpContext.Current.Response.Redirect(objUtil.ERRPATH + Constants.SESSIONEXPMSG + "*♠*" + requestProcess_logtime);
            return;
        }
        else if (loadRes.Contains("\"error\"") == true && loadRes.Contains(Constants.ERAUTHENTICATION))
        {
            HttpContext.Current.Response.Redirect(objUtil.ERRPATH + Constants.ERAUTHENTICATION + "*♠*" + requestProcess_logtime);
            return;
        }
    }

    [WebMethod]
    public static string GetLoadDataValues(string key, string recordid, string tstQureystr, string isDupTab, string isTstHtmlLs, string isFromDummytrue = "false")
    {
        string requestProcess_logtime = string.Empty;
        ExecTrace ObjExecTrace = ExecTrace.Instance;
        ObjExecTrace.SetCurrentTime();
        string result = string.Empty;
        LogFile.Log logobj = new LogFile.Log();
        Util.Util utils = new Util.Util();
        try
        {
            utils.TempAttaServerFiles();
            utils.DeleteKeyOnRefreshSave();
            string queryStringParamsData = QueryStringParams(tstQureystr);
            if (HttpContext.Current.Session["project"] == null)
                return utils.SESSTIMEOUT;
            if (isDupTab != "" && isDupTab.StartsWith("true-"))
                HttpContext.Current.Session["isDupTab"] = isDupTab;
            else
                HttpContext.Current.Session["isDupTab"] = "false";
            //if (isTstHtmlLs != "")
            //    key = utils.GetReGenTstDataObj(key, isTstHtmlLs);
            string _isTstLoadFromLSdelObj = string.Empty;
            if (isTstHtmlLs != "" && HttpContext.Current.Session[key] == null)
            {
                utils.DeleteTstIvObject(key.Split('_')[0]);
                _isTstLoadFromLSdelObj = key = utils.GetReGenTstDataObj(key, isTstHtmlLs);
                utils.tstIvObjUpdate(key);
            }
            TStructData tstData = (TStructData)HttpContext.Current.Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string transId = tstData.transID;
            Util.Util.DeletedraftRediskeys(transId);
            string errorLog = logobj.CreateLog("Load Data service.", HttpContext.Current.Session["nsessionid"].ToString(), "LoadData-" + transId, "new");
            //LoadRecidFromListNew(transId, recordid); // Not required to call here because this session getting updated through webservice method
            result = tstData.CallGetLoadData(tstData, recordid, errorLog);
            requestProcess_logtime += result.Split('♠')[0];
            result = result.Split('♠')[1];
            TStructData strDataObj = null;
            TStructDef strObj = tstData.tstStrObj;
            if (result != "" && strObj.FEncryptFlag.Count > 0)
            {
                ASB.WebService objws = new ASB.WebService();
                result = objws.AxpFieldDataDecrypt(result, strObj.FEncryptFlag);
            }
            strDataObj = new TStructData(result, transId, recordid, strObj);
            string dcHmtl = GetTabDcHTMLFormLoad(strObj, strDataObj, recordid, transId);
            key = utils.GetTstDataId(transId);
            result = result.Trim();
            result = result.Replace("\\n", "");
            result = result.Replace("\\", ";bkslh");
            result = result.Replace("'", "&quot;");

            strDataObj.loadDataRes = result;

            strDataObj.transid = transId.ToString();
            strDataObj.recordid = recordid;

            bool isTstPop = (HttpContext.Current.Session["isTstPop"] != null && HttpContext.Current.Session["isTstPop"].ToString() == "true") ? true : false;
            bool AxSplit = (queryStringParamsData.Split('♠')[2].ToLower() == "true") ? true : false;

            if (!isTstPop && !AxSplit)
                utils.DeleteTstIvObject(transId);
            else if (_isTstLoadFromLSdelObj != string.Empty)
                utils.DeleteTstIvObject(_isTstLoadFromLSdelObj.Split('_')[0]);
            HttpContext.Current.Session.Add(key, strDataObj);
            utils.tstIvObjUpdate(key);
            string strDcHtml = string.Empty;
            if (recordid != "0")
                strDcHtml = GetFormatGridHtmlNew(strObj, strDataObj);

            if (strDataObj.attachDirPath != string.Empty && recordid != "0" && strObj.tstAttachment == "True")
                result += LoadAttachFromLocNew(transId, recordid, strDataObj, result);

            if (recordid != "0" && strObj.tstAttachment == "True" && result != "")
                LoadAttachFromDbFileNew(result);

            string thisResult = key + "*$*" + result;
            string ImgVals = GetImageArraysNew(strDataObj);
            string tstVarScript = string.Empty;
            string strObjwsPerfEvalExpClient = string.Empty;
            tstVarScript = "true;";
            tstVarScript += strObjwsPerfEvalExpClient + ";";
            tstVarScript += strDataObj.attachDir;
            string wbdrHtml = string.Empty;
            if (isFromDummytrue == "false")
                wbdrHtml = GetTstHtml(transId, strObj, errorLog);
            string AxGANotExistList = string.Join(",", strDataObj.AxGridAttNotExistList.ToArray());
            string tabDCStatus = string.Join(",", strObj.tabDCStatus.ToArray());
            string tstCancelled = strObj.tstCancelled;
            result = wbdrHtml + "*$*" + dcHmtl + "*$*" + tstVarScript.ToString() + "*$*" + ImgVals + "*$*" + tabDCStatus + "*$*" + tstCancelled + "*$*" + AxGANotExistList;
            result += "*$*" + strObj.ArrFFieldHidden.ToString() + "*$*" + strObj.ArrFFieldReadOnly.ToString() + "*$*" + thisResult;
            requestProcess_logtime += ObjExecTrace.RequestProcessTime("Response", result);
            string serverprocesstm = ObjExecTrace.TotalServerElapsTime();
            result = result + "*♠♦*" + serverprocesstm + "*♠♦*" + requestProcess_logtime;
            logobj.CreateLog("Load Data service completed.", HttpContext.Current.Session["nsessionid"].ToString(), "LoadData-" + transId, "");
        }
        catch (Exception ex)
        {

        }
        return result;
    }

    private static string GetTstHtml(string transId, TStructDef strObj, string errorLog)
    {
        string AjaxTstHTML = string.Empty;
        try
        {
            CacheManager cacheMgr = new CacheManager(errorLog);
            cacheMgr.GetStructureHTML(transId, HttpContext.Current.Session["AxRole"].ToString(), HttpContext.Current.Session["nsessionid"].ToString(), HttpContext.Current.Session["language"].ToString());
            string ismodernTb = string.Empty;
            if (cacheMgr.StructureHtml == "")
            {
                ismodernTb = HttpContext.Current.Session["ModernToolbarBtnOpen"].ToString();
                //AjaxTstHTML = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + Constants.WIZARD_TEMPLATE + HttpContext.Current.Session["StructureHtml"].ToString() + "*$*" + HttpContext.Current.Session["ToolbarBtnIcons"].ToString() + "*$*" + ismodernTb;
                AjaxTstHTML = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + HttpContext.Current.Session["StructureHtml"].ToString() + "*$*" + HttpContext.Current.Session["ToolbarBtnIcons"].ToString() + "*$*" + ismodernTb;
            }
            else
            {
                ismodernTb = cacheMgr.ModernToolbarBtnOpen.ToString();
                //AjaxTstHTML = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + Constants.WIZARD_TEMPLATE + cacheMgr.StructureHtml.ToString() + "*$*" + cacheMgr.ToolbarBtnIcons.ToString() + "*$*" + ismodernTb;
                AjaxTstHTML = "<script type='text/javascript'>$j('div#wBdr').hide();</script>" + cacheMgr.StructureHtml.ToString() + "*$*" + cacheMgr.ToolbarBtnIcons.ToString() + "*$*" + ismodernTb;
            }
            if (HttpContext.Current.Session["axDesign"] != null && HttpContext.Current.Session["axDesign"].ToString() == "true" && !strObj.IsObjCustomHtml && ismodernTb == "♦")
                AjaxTstHTML += Constants.DESIGN_MODE_BTN_HTML;
        }
        catch (Exception ex)
        { }
        return AjaxTstHTML;
    }

    public static string GetTabDcHTMLFormLoad(TStructDef strObj, TStructData strDataObj, string recordId, string transId = "")
    {
        string tabDcsHtml = string.Empty;
        for (int i = 0; i < strObj.tabDCs.Count; i++)
        {
            if (strObj.tabDCStatus[i].ToString() == "0")
            {
                try
                {
                    if (transId != "")
                    {
                        DataTable axpConfigStr = new DataTable();
                        Util.Util utils = new Util.Util();
                        string AxRole = HttpContext.Current.Session["AxRole"].ToString();
                        string axpStructKey = Constants.AXCONFIGTSTRUCT;
                        if (HttpContext.Current.Session["AxDtConfigs"] != null)
                        {
                            axpConfigStr = (DataTable)HttpContext.Current.Session["AxDtConfigs"];
                            HttpContext.Current.Session.Remove("AxDtConfigs");
                        }
                        if (axpConfigStr != null && axpConfigStr.Rows.Count > 0)
                            SetAxpStructConfigPropsDummy(strObj, axpConfigStr);
                        else
                        {
                            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                            axpConfigStr = fObj.DataTableFromRedis(utils.GetConfigCacheKey(axpStructKey, transId, "", AxRole, "ALL"));
                            SetAxpStructConfigPropsDummy(strObj, axpConfigStr);
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                if (recordId == "0" && strObj.wsPerfFGDcName != null)
                {
                    string[] flGridDc = strObj.wsPerfFGDcName;
                    var loaddcFG = flGridDc.Where(d => d.ToLower() == "dc" + strObj.tabDCs[i]).ToList();
                    if (loaddcFG.Count > 0)
                        continue;
                }
                string temptabHTML = string.Empty;
                string tempDcDesignJson = string.Empty;
                try
                {
                    temptabHTML = strObj.GetTabDcHTML(Convert.ToInt32(strObj.tabDCs[i]), strDataObj, "false");
                    if (strDataObj.tstStrObj.axdesignJObject.dcLayout == null || strDataObj.tstStrObj.axdesignJObject.dcLayout == "default")
                    {
                        Dc curDc = strDataObj.tstStrObj.axdesignJObject.dcs.FirstOrDefault(elm => elm.dc_id == strObj.tabDCs[i].ToString());
                        tempDcDesignJson = new JavaScriptSerializer().Serialize(curDc);
                    }
                    else
                    {
                        Dc curDc = strDataObj.tstStrObj.axdesignJObject.newdcs.FirstOrDefault(elm => elm.dc_id == strObj.tabDCs[i].ToString());
                        tempDcDesignJson = new JavaScriptSerializer().Serialize(curDc);
                    }
                }
                catch (Exception ex)
                {

                }
                if (tabDcsHtml == string.Empty)
                    tabDcsHtml += strDataObj.AxDepArrays.ToString() + "*♦*" + strDataObj.GetMasterRowFlds() + "*♠**♠*" + temptabHTML + "*♠*" + tempDcDesignJson;
                else
                    tabDcsHtml += "*♠♠*" + strDataObj.AxDepArrays.ToString() + "*♦*" + strDataObj.GetMasterRowFlds() + "*♠**♠*" + temptabHTML + "*♠*" + tempDcDesignJson;
            }
        }
        return tabDcsHtml;
    }
    private static void SetAxpStructConfigPropsDummy(TStructDef strObj, DataTable axpConfigStr)
    {
        try
        {
            if (axpConfigStr != null && axpConfigStr.Rows.Count > 0)
            {
                var enablecheckbox = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "enable checkbox instead of switchbox in forms").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (enablecheckbox.Count > 0)
                {
                    HttpContext.Current.Session["switchboxToCheckbox"] = enablecheckbox[0].val;
                }
            }
        }
        catch (Exception ex)
        { }
    }
    private static string QueryStringParams(string tstQureystr)
    {
        Util.Util utils = new Util.Util();
        StringBuilder returnString = new StringBuilder();
        string hltype = "open";
        string isAxSplit = "false";
        string Isloadformdata = "";
        string transactionROMode = "";
        ArrayList paramNames = new ArrayList();
        ArrayList clientParamValues = new ArrayList();
        if (tstQureystr != string.Empty)
        {
            if (tstQureystr != "")
                tstQureystr = tstQureystr.Replace("♠dummyload=true♠", "♠dummyload=true").Replace("♠dummyload=false♠", "♠dummyload=false");
            string[] quertStr = tstQureystr.Split('?');
            string[] arrParams = null;
            if (quertStr.Length > 0)
            {
                arrParams = quertStr[quertStr.Length - 1].Split('♠');
            }
            string _thisTransidId = string.Empty;
            if (arrParams != null)
                for (int i = 0; i < arrParams.Length; i++)
                {
                    if (arrParams[i] != "")
                    {
                        string prName = arrParams[i].Split('=')[0];
                        string prValue = arrParams[i].Split('=')[1];
                        switch (prName.ToLower())
                        {
                            case "axp_issaveurl":
                                HttpContext.Current.Session["axp_IsSaveUrl"] = prValue;
                                break;
                            case "axfromhyplink":
                                HttpContext.Current.Session["AxFromHypLink"] = prValue;
                                break;
                            case "axpfrm":
                                HttpContext.Current.Session["axpfrm"] = prValue;
                                break;
                            case "axpop":
                                HttpContext.Current.Session["isTstPop"] = prValue;
                                break;
                            case "hltype":
                            case "act":
                                hltype = prValue;
                                break;
                            case "isduptab":
                                if (prValue != null && prValue != "")
                                    HttpContext.Current.Session["isDupTab"] = prValue;
                                else
                                    HttpContext.Current.Session["isDupTab"] = "false";
                                break;
                            case "axsplit":
                                isAxSplit = prValue;
                                break;
                            case "transid":
                                _thisTransidId = prValue;
                                break;
                            case "themode":
                            case "torecid":
                            case "layout":
                            case "tstname":
                            case "axfromhyperlink":
                            case "axhyptstrefresh":
                            case "recpos":
                            case "pagetype":
                            case "curpage":
                            case "dynnavtst":
                            case "openeriv":
                            case "isiv":
                            case "hdnbelapstime":
                            case "dummyload":
                            case "fromprocess":
                            case "ispegedit":
                            case "ivdatakey":
                                break;
                            case "loadformdata":
                                Isloadformdata = prValue;
                                break;
                            case "trromode":
                                transactionROMode = prValue;
                                break;
                            case "istsivparamcache":
                                if (prValue == "true")
                                {
                                    string strParams = string.Empty;
                                    if (HttpContext.Current.Session["AxIvParamsForTstLoad-" + _thisTransidId] != null)
                                    {
                                        strParams = HttpContext.Current.Session["AxIvParamsForTstLoad-" + _thisTransidId].ToString();
                                        HttpContext.Current.Session.Remove("AxIvParamsForTstLoad-" + _thisTransidId);
                                    }
                                    if (strParams != string.Empty)
                                    {
                                        string[] _arrParams = strParams.Split('&');
                                        foreach (string _ptname in _arrParams)
                                        {
                                            if (_ptname != "")
                                            {
                                                string _thisParamName = _ptname.Split('=')[0];
                                                string _thisParamVal = _ptname.Split('=')[1];

                                                string _thisVals = utils.CheckReverseUrlSpecialChars(_thisParamVal.ToString());
                                                _thisVals = CheckSpecialChars(_thisVals);

                                                returnString.Append("<" + _thisParamName.ToString() + ">" + _thisVals + "</" + _thisParamName.ToString() + ">");
                                                paramNames.Add(_thisParamName);
                                                clientParamValues.Add(_thisVals);
                                            }
                                        }
                                    }
                                }
                                break;
                            default:
                                string _thisVal = utils.CheckReverseUrlSpecialChars(prValue.ToString());
                                _thisVal = CheckSpecialChars(_thisVal.ToString());
                                returnString.Append("<" + prName.ToString() + ">" + _thisVal + "</" + prName.ToString() + ">");
                                paramNames.Add(prName);
                                clientParamValues.Add(_thisVal);
                                break;
                        }
                    }
                }
            UpdateNavigationNew();
        }
        string _strParamName = string.Empty;
        string _strParamValue = string.Empty;
        if (paramNames.Count > 0)
        {
            _strParamName = string.Join(",", paramNames.ToArray());
            _strParamValue = string.Join(",", clientParamValues.ToArray());
        }
        return returnString.ToString() + "♠" + hltype + "♠" + isAxSplit + "♠" + Isloadformdata + "♠" + _strParamName + "♠" + _strParamValue;
    }

    public static void GetConfigOnDummyLoad(string transId)
    {
        try
        {
            if (transId != "")
            {
                DataTable axpConfigStr = new DataTable();
                Util.Util utils = new Util.Util();
                string AxRole = HttpContext.Current.Session["AxRole"].ToString();
                string axpStructKey = Constants.AXCONFIGTSTRUCT;
                if (HttpContext.Current.Session["AxDtConfigs"] != null)
                {
                    axpConfigStr = (DataTable)HttpContext.Current.Session["AxDtConfigs"];
                    HttpContext.Current.Session.Remove("AxDtConfigs");
                }
                if (axpConfigStr != null && axpConfigStr.Rows.Count > 0)
                    SetAxpStructConfigPropsOnDummy(axpConfigStr, transId);
                else
                {
                    FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                    axpConfigStr = fObj.DataTableFromRedis(utils.GetConfigCacheKey(axpStructKey, transId, "", AxRole, "ALL"));
                    SetAxpStructConfigPropsOnDummy(axpConfigStr, transId);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public static void SetAxpStructConfigPropsOnDummy(DataTable axpConfigStr, string transId)
    {
        try
        {
            if (axpConfigStr != null && axpConfigStr.Rows.Count > 0)
            {
                var lstMultiFlds = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "multi select" && x.Field<string>("PROPSVAL").ToLower() == "true")
                 .Select(x => x.Field<string>("SFIELD")).ToList();
                if (lstMultiFlds.Count > 0)
                    HttpContext.Current.Session["AxpFldMultiSelect"] = lstMultiFlds;
                var lstMultiFldsp = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "multi select" && x.Field<string>("PROPSVAL").ToLower() == "true")
                .Select(x => x.Field<string>("SFIELD") + "♦" + x.Field<string>("PROPVALUE2")).ToList();
                if (lstMultiFldsp.Count > 0)
                    HttpContext.Current.Session["AxpFldMultiSelSp"] = lstMultiFldsp;

                var autoCompPatt = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "autocomplete search pattern")
                 .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (autoCompPatt.Count > 0)
                {
                    HttpContext.Current.Session["AxpautoCompPatt"] = autoCompPatt[0].splitVal;
                }

                string AxpSaveImageDb = "";
                var strSaveImagedb = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "saveimage")
                 .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
                if (strSaveImagedb.Count > 0)
                    AxpSaveImageDb = strSaveImagedb[0].splitVal;
                if (transId == "a__na")
                    AxpSaveImageDb = "true";
                HttpContext.Current.Session["AxpSaveImageDb"] = AxpSaveImageDb;

                var ExcelExport = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "excel export")
                    .Select(x => new { excel = x.Field<string>("PROPSVAL") }).ToList();
                if (ExcelExport.Count > 0)
                    HttpContext.Current.Session["AxpExcelExport"] = ExcelExport[0].excel;
                else
                    HttpContext.Current.Session["AxpExcelExport"] = null;

                //Tstruct grid edit option - popup/inline
                var gridEditOption = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "gridedit")
                 .Select(x => new { gridtype = x.Field<string>("PROPSVAL"), formCols = x.Field<string>("PROPVALUE2") }).ToList();

                if (gridEditOption.Count > 0)
                {
                    HttpContext.Current.Session["AxInlineGridEdit"] = gridEditOption[0].gridtype != "inline" ? "false" : "true";
                }
                else if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                    HttpContext.Current.Session["AxInlineGridEdit"] = "false";
                else
                    HttpContext.Current.Session["AxInlineGridEdit"] = "true";

                if (transId == "sect")
                {
                    HttpContext.Current.Session["AxInlineGridEdit"] = "false";
                }


                var PickListFetchSize = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "dorp down fetch size").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (PickListFetchSize.Count > 0)
                {
                    int FetchPickListRows = PickListFetchSize[0].val == "" ? 1000 : int.Parse(PickListFetchSize[0].val);
                    HttpContext.Current.Session["PickListFetchSize"] = FetchPickListRows;
                }
                else if (HttpContext.Current.Session["PickListFetchSize"] != null)
                    HttpContext.Current.Session.Remove("PickListFetchSize");

                var showSqlColumnsAdvSearch = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "show sql columns in advance search in field level" && x.Field<string>("PROPSVAL").ToLower() == "true").Select(x => x.Field<string>("PROPVALUE2")).ToList();
                if (showSqlColumnsAdvSearch.Count > 0)
                    HttpContext.Current.Session["showSqlColumnsAdvSearch"] = showSqlColumnsAdvSearch[0] == "" ? "All" : showSqlColumnsAdvSearch[0];
                else
                    HttpContext.Current.Session.Remove("showSqlColumnsAdvSearch");

                var enablecheckbox = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "enable checkbox instead of switchbox in forms").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (enablecheckbox.Count > 0)
                {
                    HttpContext.Current.Session["switchboxToCheckbox"] = enablecheckbox[0].val;
                }
                else
                    HttpContext.Current.Session.Remove("switchboxToCheckbox");

                var tstAttachFileserveDB = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "tstruct attachments stores in file server as well as db").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                if (tstAttachFileserveDB.Count > 0)
                {
                    HttpContext.Current.Session["AxtstAFSDB"] = tstAttachFileserveDB[0].val;
                }
                else
                {
                    HttpContext.Current.Session["AxtstAFSDB"] = "false";
                }

                var TstAllowVideoAtta = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "allow video file in attachments").Select(x => new { val = x.Field<string>("PROPSVAL"), alfiles = x.Field<string>("PROPVALUE2") }).ToList();
                if (TstAllowVideoAtta.Count > 0 && TstAllowVideoAtta[0].val != "" && TstAllowVideoAtta[0].val.ToLower() == "true")
                    HttpContext.Current.Session["TstAllowVideoAtta-" + transId] = TstAllowVideoAtta[0].alfiles;
                else
                    HttpContext.Current.Session.Remove("TstAllowVideoAtta-" + transId);

                var formdatavalidateworkflowaction = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "form data validate on workflow action").Select(x => new { val = x.Field<string>("PROPSVAL"), sfld = x.Field<string>("SFIELD") }).ToList();
                if (formdatavalidateworkflowaction.Count > 0 && formdatavalidateworkflowaction[0].val != "" && formdatavalidateworkflowaction[0].val.ToLower() == "true")
                    HttpContext.Current.Session["TstDataValidateWFAction-" + transId] = "true";
                else
                    HttpContext.Current.Session.Remove("TstDataValidateWFAction-" + transId);

                var Optionavoidmultipledraft = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "option to avoid multiple draft per a transaction").Select(x => new { val = x.Field<string>("PROPSVAL"), sfld = x.Field<string>("SFIELD") }).ToList();
                if (Optionavoidmultipledraft.Count > 0 && Optionavoidmultipledraft[0].val != "" && Optionavoidmultipledraft[0].val.ToLower() == "true")
                    HttpContext.Current.Session["TstAvoidmultipledraft-" + transId] = "true";
                else
                    HttpContext.Current.Session.Remove("TstAvoidmultipledraft-" + transId);
            }
            else
            {
                if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                    HttpContext.Current.Session["AxInlineGridEdit"] = "false";
                else
                    HttpContext.Current.Session["AxInlineGridEdit"] = "true";
                HttpContext.Current.Session["AxpSaveImageDb"] = null;
                HttpContext.Current.Session["AxpautoCompPatt"] = null;
                HttpContext.Current.Session["AxpExcelExport"] = null;
                HttpContext.Current.Session["AxpFldMultiSelect"] = null;
                HttpContext.Current.Session["AxpFldMultiSelSp"] = null;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("SetAxpStructConfigProps -" + ex.Message, sessID, "SetAxpStructConfigProps", "new", "true");
        }
    }

    private static void UpdateNavigationNew()
    {
        Util.Util utils = new Util.Util();
        string isTstFromHyperLink = string.Empty;
        if (HttpContext.Current.Session["AxFromHypLink"] != null)
            isTstFromHyperLink = HttpContext.Current.Session["AxFromHypLink"].ToString();

        string frameName = string.Empty;
        if (HttpContext.Current.Session["axpfrm"] != null)
            frameName = HttpContext.Current.Session["axpfrm"].ToString();
        if (HttpContext.Current.Session["backForwBtnPressed"] == null || (HttpContext.Current.Session["backForwBtnPressed"] != null && !Convert.ToBoolean(HttpContext.Current.Session["backForwBtnPressed"])) && frameName != "t")
        {
            if (HttpContext.Current.Session["isTstPop"] != null && HttpContext.Current.Session["isTstPop"].ToString() == "true")
                HttpContext.Current.Session["enableBackButton"] = "false";
            else if (HttpContext.Current.Session["AxHypTstRefresh"] != null && HttpContext.Current.Session["AxHypTstRefresh"].ToString() == "true")
            {
                HttpContext.Current.Session["AxHypTstRefresh"] = "false";
            }
            else
                utils.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri);
        }
        if (HttpContext.Current.Session["axp_IsSaveUrl"] != null)
            HttpContext.Current.Session["axp_IsSaveUrl"] = null;
        if (HttpContext.Current.Session["axpfrm"] != null)
            HttpContext.Current.Session["axpfrm"] = null;
        HttpContext.Current.Session["backForwBtnPressed"] = false;
        enableBackForwButton = "<script language=\'javascript\' type=\'text/javascript\' > enableBackButton='" + Convert.ToBoolean(HttpContext.Current.Session["enableBackButton"]) + "';" + " enableForwardButton='" + Convert.ToBoolean(HttpContext.Current.Session["enableForwardButton"]) + "'; var fromHyperLink='" + isTstFromHyperLink + "';var isRapidLoad='" + false + "';var defaultDepFlds='';</script>";
    }

    private static void LoadRecidFromListNew(string transid, string rcId)
    {
        Util.Util utils = new Util.Util();
        Dictionary<int, string> lvRecordListing = new Dictionary<int, string>();
        lvRecordListing = utils.GetlvRecordList(transid);
        if (lvRecordListing != null)
        {
            try
            {
                if (lvRecordListing != null)
                {
                    var recordkeyValuePair = lvRecordListing.Single(x => x.Value.IndexOf(rcId) > -1);
                    int recordKey = recordkeyValuePair.Key;
                    HttpContext.Current.Session["lvRecordKey"] = recordKey;
                }
            }
            catch (Exception ex)
            {
                //logobj.CreateLog(ex.Message, sid, "Exception in LoadStructure ListViewNavigation Details", "new");
            }
        }
    }

    //Function to construct the image arrays for all the images in the tstruct with values from the tstruct data object.
    private static string GetImageArraysNew(TStructData tstData)
    {
        string strImg = string.Empty;
        StringBuilder strImgArr = new StringBuilder();
        for (int i = 0; i < tstData.imageFldNames.Count; i++)
        {
            if (tstData.imageFldSrc.Count > 0)
            {
                if (strImgArr.ToString() != string.Empty)
                    strImgArr.Append("♠");
                strImgArr.Append(tstData.imageFldNames[i].ToString() + "♦");
                string src = tstData.imageFldSrc[i].ToString();
                src = src.Replace("\\", ";bkslh");
                strImgArr.Append(src);
            }
        }
        strImg = strImgArr.ToString();
        return strImg;
    }

    private static string GetFormatGridHtmlNew(TStructDef strObj, TStructData tstData)
    {
        StringBuilder strDcHtml = new StringBuilder();
        for (int i = 0; i < strObj.visibleDCs.Count; i++)
        {
            int dcNo = Convert.ToInt32(strObj.visibleDCs[i].ToString());
            int dcIndNo = strObj.dcsPositionIndex.IndexOf(dcNo.ToString());
            if (strObj.IsDcFormatGrid(dcIndNo, dcNo))
            {
                strDcHtml.Append(strObj.GetTabDcHTML(dcNo, tstData, "false"));
            }
        }
        return strDcHtml.ToString();
    }

    private static string LoadAttachFromLocNew(string transId, string recId, TStructData strDataObj, string strloadRes)
    {
        string attachJson = string.Empty;
        if (strloadRes != "" && strloadRes.IndexOf("*$*{\"attachment\":[{") == -1)
        {
            string attachDirPath = strDataObj.attachDirPath;
            if (!attachDirPath.Contains(":\\") && !attachDirPath.StartsWith("\\"))
            {
                strDataObj.CreateDirectoryInCommonDir(ref attachDirPath);
            }

            DirectoryInfo dir = new DirectoryInfo(attachDirPath + "\\" + transId + "\\");
            string fileValues = string.Empty;
            if (dir.Exists)
            {
                FileInfo[] info = dir.GetFiles(recId + "-*.*");
                foreach (FileInfo FI in info)
                    fileValues += FI.ToString().Substring(FI.ToString().IndexOf("-") + 1) + ",";
                if (fileValues.Length > 0)
                {
                    fileValues = fileValues.TrimEnd(',');
                    attachJson = "*$*{\"attachment\":[{\"att\":\"" + fileValues + "\"}]}";
                }
            }
        }
        return attachJson;
    }

    private static void LoadAttachFromDbFileNew(string loadRes)
    {
        try
        {
            if (loadRes != "" && loadRes.Contains("*$*{\"attachment\":[{\"att\":\""))
            {
                string[] jsonSplit = loadRes.Split(new[] { "*$*" }, StringSplitOptions.None);
                foreach (var _jVal in jsonSplit)
                {
                    if (_jVal.Contains("{\"attachment\":[{\"att\""))
                    {
                        try
                        {
                            JObject message = JObject.Parse(_jVal.ToString());
                            string _fileNames = message["attachment"][0]["att"].ToString();
                            if (HttpContext.Current.Session["tstHeaderAttachDbFile"] != null && HttpContext.Current.Session["tstHeaderAttachDbFile"].ToString() != "")
                                HttpContext.Current.Session["tstHeaderAttachDbFile"] = HttpContext.Current.Session["tstHeaderAttachDbFile"] + "," + _fileNames;
                            else
                                HttpContext.Current.Session["tstHeaderAttachDbFile"] = _fileNames;
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    [WebMethod]
    public static string GetMultiSelectValues(string tstDataId, string FldName, ArrayList ChangedFields, ArrayList ChangedFieldDbRowNo, ArrayList ChangedFieldValues, ArrayList DeletedDCRows, string fldNameMs, string refreshMs, string parentsFlds, string rfSave, string pageData, string isTstHtmlLs)
    {
        string json = string.Empty;
        try
        {
            ASB.WebService objws = new ASB.WebService();
            json = objws.GetMultiSelectValues(tstDataId, FldName, "", ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, fldNameMs, refreshMs, parentsFlds, rfSave, pageData, isTstHtmlLs);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get MultiSelect Values -" + ex.Message, sessID, "GetMultiSelectValues", "new");
        }
        return json;
    }


    [WebMethod]
    public static object GetCurrentTime(string setTimeFormat)
    {
        string result = string.Empty;
        string status = string.Empty;
        try
        {
            if (setTimeFormat == "True")
                result = System.DateTime.Now.ToString("HH:mm:ss");
            else
                result = System.DateTime.Now.ToString("HH:mm");
            status = "success";
            return new { result = result, status = status };
        }
        catch (Exception ex)
        {
            status = "failure";
            return new { result = result, status = status };

        }


    }

    [WebMethod]
    public static string callExecuteSQL(string queryString)
    {
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        string result = "";
        try
        {
            result = asbExt.ExecuteSQL("", queryString, "JSON");
            //DataSet ds = new DataSet();
            //System.IO.StringReader sr = new System.IO.StringReader(result);
            //ds.ReadXml(sr);

        }
        catch (Exception ex) { }
        return result;

    }

    [WebMethod]
    public static string GetWfPdComments(string wftype, string wfid, string strTransid)
    {
        string result = "";
        try
        {
            string sqlQuery = Constants.SQL_GET_WFPDCOMMENTS;
            sqlQuery = sqlQuery.Replace("$actiontype$", wftype);
            sqlQuery = sqlQuery.Replace("$tid$", strTransid);
            sqlQuery = sqlQuery.Replace("$wfid$", wfid);
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            result = asbExt.ExecuteSQL(strTransid, sqlQuery, "JSON");
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Workflow PdComments -" + ex.Message, sessID, "GetWfPdComments", "new");
        }
        return result;
    }

    [WebMethod]
    public static object signatureUpload(string saveSignInfo)
    {
        Util.Util util = new Util.Util();

        var returnObject = new Dictionary<string, object>();
        dynamic parseSignInfo = JsonConvert.DeserializeObject(saveSignInfo);
        string base64 = parseSignInfo.imgSrc.Value;
        string[] substr = base64.Split(',');
        //string[] imagePath = (parseSignInfo.imgSrc.Value).Split(",");
        string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        string scriptsUrlPath = HttpContext.Current.Application["ScriptsurlPath"].ToString();
        string sessionID = string.Empty;

        if (HttpContext.Current.Session["nsessionid"] != null && HttpContext.Current.Session["project"] != null)
            sessionID = HttpContext.Current.Session["nsessionid"].ToString();
        else
            return util.SESSTIMEOUT;

        try
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(substr[1])))
            {
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string filePath = "";
                using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
                {
                    string fieldName = parseSignInfo.fieldName.Value;
                    int fIdx = fieldName.LastIndexOf("F");
                    string signImageName = string.Empty;
                    string tmpFldName = string.Empty;
                    if (fIdx != -1)
                    {
                        signImageName = timeStamp + fieldName.Substring(0, fIdx - 3) + ".png";
                        tmpFldName = fieldName.Substring(0, fIdx - 3);
                    }
                    DirectoryInfo di = new DirectoryInfo(scriptsPath + "axpert\\" + sessionID + "\\" + tmpFldName);
                    if (tmpFldName.StartsWith(Constants.IMGPrefix))
                    {
                        //The folder path for uploading the image. 
                        returnObject.Add("imgURL", scriptsUrlPath + "axpert/" + sessionID + "/" + tmpFldName + "/" + System.Uri.EscapeDataString(signImageName));
                        filePath = scriptsPath + "axpert\\" + sessionID + "\\" + tmpFldName + "\\" + signImageName;
                    }
                    else if (parseSignInfo.isAxpImagePath.Value == "true")
                    {
                        //The folder path for uploading the image - AxpimagePath. 
                        returnObject.Add("imgURL", scriptsUrlPath + "axpert/" + sessionID + "/" + tmpFldName + "/" + System.Uri.EscapeDataString(signImageName));
                        filePath = scriptsPath + "axpert\\" + sessionID + "\\" + tmpFldName + "\\" + signImageName;
                    }
                    else
                    {
                        di = new DirectoryInfo(scriptsPath + "axpert\\" + sessionID);
                        returnObject.Add("imgURL", scriptsUrlPath + "axpert/" + sessionID + "/" + System.Uri.EscapeDataString(signImageName));
                        filePath = scriptsPath + "axpert\\" + sessionID + "\\" + signImageName;
                    }

                    if (!di.Exists)
                        di.Create();
                    returnObject.Add("imageName", signImageName);
                    bm2.Save(filePath);
                }
            }
            returnObject.Add("msg", "done");
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Signature Upload -" + ex.Message, sessionID, "signatureUpload", "new");
            returnObject.Add("msg", ex.Message);
        }

        return returnObject;
    }

    [WebMethod]
    public static string GetLabelHlStructures(string StrType)
    {
        string result = string.Empty;
        try
        {
            string fldSql = string.Empty;
            if (StrType == "tstruct")
                fldSql = Constants.LBLHL_GETTSTRUCTS;
            else if (StrType == "iview")
                fldSql = Constants.LBLHL_GETIVIEWS;
            else
                fldSql = Constants.LBLHL_GETPAGES;
            Util.Util utilobj = new Util.Util();
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            result = asbExt.ExecuteSQL("", fldSql, "JSON");
            string errrMsg = utilobj.ParseJSonErrorNode(result);
            if (errrMsg == string.Empty)
            {

            }
        }
        catch (Exception ex)
        {

        }
        return result;
    }

    [WebMethod]
    public static string GetLabelHlStrParams(string StrType, string StructName)
    {
        string result = string.Empty;
        try
        {
            string fldSql = string.Empty;
            if (StrType == "tstruct")
            {
                fldSql = Constants.LBLHL_GETTSTRUCTFIELDS;
                fldSql = fldSql.Replace("$stransid$", "'" + StructName + "'");
            }
            else if (StrType == "iview")
            {
                fldSql = Constants.LBLHL_GETIVIEWPARAMS;
                fldSql = fldSql.Replace("$iname$", "'" + StructName + "'");
            }
            //else
            //    fldSql = Constants.LBLHL_GETPAGES;
            Util.Util utilobj = new Util.Util();
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            result = asbExt.ExecuteSQL("", fldSql, "JSON");
            string errrMsg = utilobj.ParseJSonErrorNode(result);
            if (errrMsg == string.Empty)
            {

            }
        }
        catch (Exception ex)
        {

        }
        return result;
    }

    [WebMethod]
    public static string GetSavedViewColumns(string TransId)
    {
        string SelectedFields = string.Empty;
        try
        {
            DBContext objdb = new DBContext();
            SelectedFields = objdb.GetSearchViewColumns(TransId);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("GetSavedViewColumns -" + ex.Message, sessID, "GetSavedViewColumns", "new");
        }
        return SelectedFields;
    }

    [WebMethod]
    public static string SaveViewColumns(string TransId, string selectedFlds, bool isViewColUpdate)
    {
        string result = string.Empty;
        try
        {
            DBContext objdb = new DBContext();
            objdb.SaveSearchViewColumns(TransId, selectedFlds, isViewColUpdate);
            if (HttpContext.Current.Session["AxDirectDBException"] != null)
            {
                result = "error:" + HttpContext.Current.Session["AxDirectDBException"].ToString();
                HttpContext.Current.Session.Remove("AxDirectDBException");
            }
            else
                result = "success";
        }
        catch (Exception ex)
        {
            result = "error";
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("SaveViewColumns -" + ex.Message, sessID, "SaveViewColumns", "new");
        }
        return result;
    }

    #region Get ConfigData from FormLoad on new
    private static string GetConfigDataVarsNew(TStructDef objSts, string resJson, string transId)
    {
        string result = resJson;
        try
        {
            string configDataJson = string.Empty;
            if (resJson.TrimStart().StartsWith("{")) //Json
            {
                try
                {
                    string[] orgResult = resJson.Split(new[] { "$*$" }, StringSplitOptions.None);
                    resJson = orgResult[0];//0: Formload result,1: ConfigData
                    if (orgResult.Length >= 2)
                    {
                        configDataJson = orgResult[1];
                    }
                }
                catch (Exception ex) { }
                if (configDataJson != string.Empty)
                {
                    HttpContext.Current.Session["ExpFldsForAxVarsandCOnfig"] = "true";
                    ParseConfigDataResultNew(configDataJson, transId);
                }

                result = resJson;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("GetConfigDataVars -" + ex.Message + "- FormLoad Result-" + resJson, HttpContext.Current.Session.SessionID, "GetConfigDataVars", "new");
            result = resJson;
        }
        return result;
    }


    public static void ParseConfigDataResultNew(string configDataJson, string transId)
    {
        LogFile.Log logobj = new LogFile.Log();
        Util.Util utils = new Util.Util();
        try
        {
            StringBuilder cdParamlist = new StringBuilder();
            if (configDataJson != string.Empty)
            {
                configDataJson = Regex.Replace(configDataJson, "<", "&lt;");
                configDataJson = Regex.Replace(configDataJson, ">", "&gt;");
                string configVars = "<configdata>" + configDataJson + "</configdata>";
                try
                {
                    string schemaname = string.Empty;
                    if (HttpContext.Current.Session["dbuser"] != null)
                        schemaname = HttpContext.Current.Session["dbuser"].ToString();
                    string user = HttpContext.Current.Session["user"].ToString();
                    string fdKey = Constants.CONFIGDATAVARSFORMLOAD;
                    FDW fdwObj = new FDW();
                    bool isCached = fdwObj.SaveInRedisServer(utils.GetRedisServerkey(fdKey, transId), configVars, Constants.CONFIGDATAVARSFORMLOAD, schemaname);
                    if (!isCached)
                        HttpContext.Current.Session["configdata_" + transId] = configVars;
                }
                catch (Exception exc)
                {
                    HttpContext.Current.Session["configdata_" + transId] = configVars;
                }

                dynamic dynJson = JsonConvert.DeserializeObject(configDataJson);
                string varDataType = string.Empty;
                int paramCnt = 0;
                foreach (var item in dynJson.configdata)
                {
                    string nodeName = item.Name;
                    string nodeVal = item.First.Value == null ? "" : item.First.Value.ToString();
                    nodeVal = Regex.Replace(nodeVal, "\"", "&quot;");
                    cdParamlist.Append("AxCdParameters[" + paramCnt + "]=" + "\"" + nodeName + "♠" + nodeVal + "\";");
                    paramCnt++;
                }
                string AxMem_Scripts = "<script language='javascript' type='text/javascript' >" + cdParamlist.ToString() + "</script>";
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("ParseConfigDataResult -" + ex.Message + "- JSON-" + configDataJson, HttpContext.Current.Session.SessionID, "ParseConfigDataResult", "new");
        }
    }

    public static void GetConfigDataFromCacheNew(string transId)
    {
        LogFile.Log logobj = new LogFile.Log();
        Util.Util utils = new Util.Util();
        try
        {
            StringBuilder cdParamlist = new StringBuilder();
            string configDataJson = string.Empty;
            configDataJson = utils.GetConfigDataVarsXML(transId);
            if (configDataJson != string.Empty)
            {
                configDataJson = configDataJson.Replace("<configdata>", "").Replace("</configdata>", "");
                dynamic dynJson = JsonConvert.DeserializeObject(configDataJson);
                string varDataType = string.Empty;
                int paramCnt = 0;
                foreach (var item in dynJson.configdata)
                {
                    string nodeName = item.Name;
                    string nodeVal = item.First.Value == null ? "" : item.First.Value.ToString();
                    nodeVal = Regex.Replace(nodeVal, "\"", "&quot;");
                    cdParamlist.Append("AxCdParameters[" + paramCnt + "]=" + "\"" + nodeName + "♠" + nodeVal + "\";");
                    paramCnt++;
                }
                string AxMem_Scripts = "<script language='javascript' type='text/javascript' >" + cdParamlist.ToString() + "</script>";
                HttpContext.Current.Session["ExpFldsForAxVarsandCOnfig"] = "true";
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("GetConfigDataFromCache -" + ex.Message, HttpContext.Current.Session.SessionID, "GetConfigDataFromCache", "new");
        }
    }

    #endregion

    [WebMethod]
    public static string GetCloneFormLoadValues(string key, string tstQureystr, string isTstHtmlLs)
    {
        string requestProcess_logtime = string.Empty;
        ExecTrace ObjExecTrace = ExecTrace.Instance;
        ObjExecTrace.SetCurrentTime();
        string result = string.Empty;
        LogFile.Log logobj = new LogFile.Log();
        Util.Util utils = new Util.Util();
        try
        {
            utils.TempAttaServerFiles();
            utils.DeleteKeyOnRefreshSave();
            QueryStringParams(tstQureystr);
            if (HttpContext.Current.Session["project"] == null)
                return utils.SESSTIMEOUT;
            //if (isTstHtmlLs != "")
            //    key = utils.GetReGenTstDataObj(key, isTstHtmlLs);
            if (isTstHtmlLs != "")
            {
                utils.DeleteTstIvObject(key.Split('_')[0]);
                key = utils.GetReGenTstDataObj(key, isTstHtmlLs);
            }
            TStructData tstData = (TStructData)HttpContext.Current.Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string transId = tstData.transID;
            string errorLog = logobj.CreateLog("Load Data service.", HttpContext.Current.Session["nsessionid"].ToString(), "LoadData-" + transId, "new");
            string recordid = "0";
            //LoadRecidFromListNew(transId, recordid);// Not required to call here because this session getting updated through webservice method
            TStructDef strObj = tstData.tstStrObj;
            tstData.recordID = "";
            string dcHmtl = GetTabDcHTMLFormLoad(strObj, tstData, recordid, transId);
            string strDcHtml = string.Empty;
            if (recordid != "0")
                strDcHtml = GetFormatGridHtmlNew(strObj, tstData);

            if (tstData.attachDirPath != string.Empty && recordid != "0" && strObj.tstAttachment == "True")
                result += LoadAttachFromLocNew(transId, recordid, tstData, result);

            string thisResult = key + "*$*" + result;
            string ImgVals = GetImageArraysNew(tstData);
            string tstVarScript = string.Empty;
            string strObjwsPerfEvalExpClient = string.Empty;
            tstVarScript = "true;";
            tstVarScript += strObjwsPerfEvalExpClient + ";";
            tstVarScript += tstData.attachDir;
            string wbdrHtml = string.Empty;
            wbdrHtml = GetTstHtml(transId, strObj, errorLog);
            string AxGANotExistList = string.Join(",", tstData.AxGridAttNotExistList.ToArray());
            string tabDCStatus = string.Join(",", strObj.tabDCStatus.ToArray());
            string tstCancelled = strObj.tstCancelled;
            result = wbdrHtml + "*$*" + dcHmtl + "*$*" + tstVarScript.ToString() + "*$*" + ImgVals + "*$*" + tabDCStatus + "*$*" + tstCancelled + "*$*" + AxGANotExistList;
            result += "*$*" + strObj.ArrFFieldHidden.ToString() + "*$*" + strObj.ArrFFieldReadOnly.ToString() + "*$*" + thisResult;
            requestProcess_logtime += ObjExecTrace.RequestProcessTime("Response", result);
            string serverprocesstm = ObjExecTrace.TotalServerElapsTime();
            result = result + "*♠♦*" + serverprocesstm + "*♠♦*" + requestProcess_logtime;
            logobj.CreateLog("Load Data service completed.", HttpContext.Current.Session["nsessionid"].ToString(), "LoadData-" + transId, "");
        }
        catch (Exception ex)
        {

        }
        return result;
    }

    [WebMethod]
    public static string GetLoadDataForDiscard(string key, string recordid, string tstQureystr, string isDupTab, string isTstHtmlLs)
    {
        string requestProcess_logtime = string.Empty;
        ExecTrace ObjExecTrace = ExecTrace.Instance;
        ObjExecTrace.SetCurrentTime();
        string result = string.Empty;
        LogFile.Log logobj = new LogFile.Log();
        Util.Util utils = new Util.Util();
        try
        {
            utils.TempAttaServerFiles();
            utils.DeleteKeyOnRefreshSave();
            string queryStringParamsData = QueryStringParams(tstQureystr);
            if (HttpContext.Current.Session["project"] == null)
                return utils.SESSTIMEOUT;
            if (isDupTab != "" && isDupTab.StartsWith("true-"))
                HttpContext.Current.Session["isDupTab"] = isDupTab;
            else
                HttpContext.Current.Session["isDupTab"] = "false";
            //if (isTstHtmlLs != "")
            //    key = utils.GetReGenTstDataObj(key, isTstHtmlLs);
            string _isTstLoadFromLSdelObj = string.Empty;
            if (isTstHtmlLs != "")
            {
                utils.DeleteTstIvObject(key.Split('_')[0]);
                _isTstLoadFromLSdelObj = key = utils.GetReGenTstDataObj(key, isTstHtmlLs);
                utils.tstIvObjUpdate(key);
            }
            TStructData tstData = (TStructData)HttpContext.Current.Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string transId = tstData.transID;
            Util.Util.DeletedraftRediskeys(transId);
            string errorLog = logobj.CreateLog("Load Data service.", HttpContext.Current.Session["nsessionid"].ToString(), "LoadData-" + transId, "new");
            //LoadRecidFromListNew(transId, recordid);// Not required to call here because this session getting updated through webservice method
            result = tstData.loadDataRes;
            if (result == "")
            {
                result = tstData.CallGetLoadData(tstData, recordid, errorLog);
                requestProcess_logtime += result.Split('♠')[0];
                result = result.Split('♠')[1];
            }
            TStructData strDataObj = null;
            TStructDef strObj = tstData.tstStrObj;
            if (result != "" && strObj.FEncryptFlag.Count > 0)
            {
                ASB.WebService objws = new ASB.WebService();
                result = objws.AxpFieldDataDecrypt(result, strObj.FEncryptFlag);
            }
            strDataObj = new TStructData(result, transId, recordid, strObj);
            string dcHmtl = GetTabDcHTMLFormLoad(strObj, strDataObj, recordid, transId);
            key = utils.GetTstDataId(transId);
            result = result.Trim();
            result = result.Replace("\\n", "");
            result = result.Replace("\\", ";bkslh");
            result = result.Replace("'", "&quot;");

            strDataObj.loadDataRes = result;

            strDataObj.transid = transId.ToString();
            strDataObj.recordid = recordid;

            bool isTstPop = (HttpContext.Current.Session["isTstPop"] != null && HttpContext.Current.Session["isTstPop"].ToString() == "true") ? true : false;
            bool AxSplit = (queryStringParamsData.Split('♠')[2].ToLower() == "true") ? true : false;

            if (!isTstPop && !AxSplit)
                utils.DeleteTstIvObject(transId);
            else if (_isTstLoadFromLSdelObj != string.Empty)
                utils.DeleteTstIvObject(_isTstLoadFromLSdelObj.Split('_')[0]);
            HttpContext.Current.Session.Add(key, strDataObj);
            utils.tstIvObjUpdate(key);
            string strDcHtml = string.Empty;
            if (recordid != "0")
                strDcHtml = GetFormatGridHtmlNew(strObj, strDataObj);

            if (strDataObj.attachDirPath != string.Empty && recordid != "0" && strObj.tstAttachment == "True")
                result += LoadAttachFromLocNew(transId, recordid, strDataObj, result);

            if (recordid != "0" && strObj.tstAttachment == "True" && result != "")
                LoadAttachFromDbFileNew(result);

            string thisResult = key + "*$*" + result;
            string ImgVals = GetImageArraysNew(strDataObj);
            string tstVarScript = string.Empty;
            string strObjwsPerfEvalExpClient = string.Empty;
            tstVarScript = "true;";
            tstVarScript += strObjwsPerfEvalExpClient + ";";
            tstVarScript += strDataObj.attachDir;
            string wbdrHtml = string.Empty;
            wbdrHtml = GetTstHtml(transId, strObj, errorLog);
            string AxGANotExistList = string.Join(",", strDataObj.AxGridAttNotExistList.ToArray());
            string tabDCStatus = string.Join(",", strObj.tabDCStatus.ToArray());
            string tstCancelled = strObj.tstCancelled;
            result = wbdrHtml + "*$*" + dcHmtl + "*$*" + tstVarScript.ToString() + "*$*" + ImgVals + "*$*" + tabDCStatus + "*$*" + tstCancelled + "*$*" + AxGANotExistList;
            result += "*$*" + strObj.ArrFFieldHidden.ToString() + "*$*" + strObj.ArrFFieldReadOnly.ToString() + "*$*" + thisResult;
            requestProcess_logtime += ObjExecTrace.RequestProcessTime("Response", result);
            string serverprocesstm = ObjExecTrace.TotalServerElapsTime();
            result = result + "*♠♦*" + serverprocesstm + "*♠♦*" + requestProcess_logtime;
            logobj.CreateLog("Load Data service completed.", HttpContext.Current.Session["nsessionid"].ToString(), "LoadData-" + transId, "");
        }
        catch (Exception ex)
        {

        }
        return result;
    }

    [WebMethod]
    public static string GetCachedSaveNotificationJSON(string transid, string axmsgid)
    {
        string result = string.Empty;
        try
        {
            ASB.WebService objws = new ASB.WebService();
            result = objws.GetCachedSaveNotificationJSON(transid, axmsgid);
        }
        catch (Exception ex)
        {
            result = "error";
        }
        return result;
    }

    [WebMethod]
    public static string PublishAPISubmitDataJSON(string strTrId)
    {
        string strJson = string.Empty;
        try
        {
            string sql = "select a.fname,a.dcname from axpflds a where a.tstruct='" + strTrId + "'";
            string tstFieldjson = string.Empty;
            string tstDCInfo = string.Empty;

            try
            {
                ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
                tstFieldjson = asbExt.ExecuteSQL("", sql, "JSON");
                sql = "SELECT a.dname,a.asgrid FROM axpdc a where a.tstruct ='" + strTrId + "' order by a.dname";
                tstDCInfo = asbExt.ExecuteSQL("", sql, "JSON");

                strJson = tstFieldjson + "¿" + tstDCInfo;
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("Create Request JSON:" + ex.Message, HttpContext.Current.Session.SessionID, "CreateRequestJSON", "");
                strJson = "error";
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Create Request JSON:" + ex.Message, HttpContext.Current.Session.SessionID, "CreateRequestJSON", "");
            strJson = "error";
        }
        return strJson;
    }

    [WebMethod]
    public static string CreateSqlRequestJSON(string custSqlName)
    {
        string strJson = string.Empty;
        try
        {
            string errorlog = "false";
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["AxTrace"] != null)
                errorlog = HttpContext.Current.Session["AxTrace"].ToString();
            Util.Util utilobj = new Util.Util();
            string fldSql = Constants.AxAPIField_SqlData;
            fldSql = fldSql.Replace("$sqlName$", "'" + custSqlName + "'");
            string paramList = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            string result = asbExt.ExecuteSQL("", fldSql, "JSON");
            string errrMsg = utilobj.ParseJSonErrorNode(result);
            if (errrMsg == string.Empty)
            {
                try
                {
                    JObject obj = JObject.Parse(result);
                    string sqlparams = obj["result"]["row"][0]["sqlparams"].ToString();
                    if (sqlparams != "")
                    {
                        foreach (var param in sqlparams.Split(','))
                        {
                            string[] paramSplit = param.Split('~');

                            paramList += "\"" + paramSplit[0] + "\":\"\",";
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (paramList != "")
                strJson = ",\"sqlparams\":{" + paramList.TrimEnd(',') + "}";
            else
                strJson = "";
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Create Sql Request JSON:" + ex.Message, HttpContext.Current.Session.SessionID, "CreateSqlRequestJSON", "");
            strJson = "";
        }
        return strJson;
    }

    [WebMethod]
    public static void SaveFreezeCols(string transid, string freezcols)
    {
        try
        {
            Util.Util utilobj = new Util.Util();
            string schemaname = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaname = HttpContext.Current.Session["dbuser"].ToString();
            string user = HttpContext.Current.Session["user"].ToString();
            FDW fdwObj = new FDW();
            string freezeCols = Constants.REDISFREEZEGRDCOLS;
            fdwObj.SaveInRedisServer(utilobj.GetRedisServerkey(freezeCols, transid, user), freezcols, Constants.REDISFREEZEGRDCOLS, schemaname);
            ClearCacheDesignKeys(transid);
        }
        catch (Exception ex) { }
    }

    protected void GetFreezeCols(string transid)
    {
        try
        {
            string freezeCols = Constants.REDISFREEZEGRDCOLS;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            string user = HttpContext.Current.Session["user"].ToString();
            if (fObj != null)
                FreezeGridCols = fObj.StringFromRedis(util.GetRedisServerkey(freezeCols, transid, user), schemaName);
        }
        catch (Exception) { }
    }

    [WebMethod]
    public static string GetTstSearchData(string key, string transId, string hdnSearchStr, string ddlSearch, string pageNo, string pageSize, string isTstHtmlLs)
    {
        Util.Util utils = new Util.Util();
        LogFile.Log logobjs = new LogFile.Log();
        try
        {
            string searchVal = hdnSearchStr;
            //searchVal = utils.CheckReverseUrlSpecialChars(searchVal);
            searchVal = utils.CheckSpecialChars(searchVal);
            if (HttpContext.Current.Session["nsessionid"] == null)
                return "Error♠Error occurred(2). Please try again or contact administrator.";
            float f;
            if (isTstHtmlLs != "")
            {
                utils.DeleteTstIvObject(key.Split('_')[0]);
                key = utils.GetReGenTstDataObj(key, isTstHtmlLs);
            }
            TStructData _tstData = (TStructData)HttpContext.Current.Session[key];
            if (_tstData != null)
            {
                string sid = HttpContext.Current.Session["nsessionid"].ToString();
                TStructDef strObj = _tstData.tstStrObj;
                string[] _fltddlSearch = ddlSearch.Split(new[] { "~~" }, StringSplitOptions.None);
                string[] _fltddlSearchVar = searchVal.Split(new[] { "~~" }, StringSplitOptions.None);
                for (int i = 0; i < _fltddlSearch.Length; i++)
                {
                    string _ddlSearch = _fltddlSearch[i];
                    string _searchVal = _fltddlSearchVar[i];
                    //_searchVal = _searchVal.Replace("%", "").Replace("&quot;", "");
                    _searchVal = _searchVal.Replace("%", "").Replace("&apos;", "");
                    int fIdx = strObj.GetFieldIndex(_ddlSearch);
                    TStructDef.FieldStruct fld = (TStructDef.FieldStruct)strObj.flds[fIdx];
                    if (fld.datatype != "Date/Time" && fld.datatype != "Numeric" && ((fld.datatype == "Character" && !(_searchVal.GetType().Name == "String" || _searchVal.GetType().Name == "Character")) || (fld.datatype != "Character" && _searchVal.GetType().Name != fld.datatype)))
                    {
                        return "Error♠records:Enter " + fld.datatype.ToLower() + " values";
                    }
                    else if (fld.datatype == "Numeric" && (!IsAllDigitsNew(_searchVal) && !float.TryParse(_searchVal, out f)))
                    {
                        return "Error♠records:Enter " + fld.datatype.ToLower() + " values";
                    }
                }

                string _errorLog = logobjs.CreateLog("Loading Search List.", sid, "GetTstSearchData-" + transId, "new");
                string iXml = string.Empty;
                iXml = "<sqlresultset axpapp=\"" + HttpContext.Current.Session["project"].ToString() + "\" transid=\"" + transId + "\" sessionid=\"" + sid + "\" trace=\"" + _errorLog + "\" pageno=\"" + pageNo + "\" pagesize=\"" + int.Parse(pageSize) + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                string _fldsSelected = ddlSearch.Replace("~~", ",");
                iXml = iXml + "<fields>" + _fldsSelected + "</fields><searchfor>" + ddlSearch + "</searchfor><value>" + searchVal + "</value>";

                string dbmemvarsXML = string.Empty;
                dbmemvarsXML = utils.GetDBMemVarsXML(transId);
                string cdVarsXML = utils.GetConfigDataVarsXML(transId);
                iXml = iXml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + dbmemvarsXML + cdVarsXML + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                string res = string.Empty;

                ASBExt.WebServiceExt _objWebServiceExt = new ASBExt.WebServiceExt();
                res = _objWebServiceExt.CallGetSearchValWS(transId, iXml, strObj.structRes);
                res = res.Split('♠')[1];
                if (res.ToLower().Contains("ora-"))
                {
                    string strErrMsg = "Error occurred(2). Please try again or contact administrator.";
                    return "Error♠" + strErrMsg;
                }
                if ((res.IndexOf(Constants.ERROR) == -1))
                {
                    XmlDocument xmlDoc1 = new XmlDocument();
                    xmlDoc1.LoadXml(res);

                    XmlNode cNode = default(XmlNode);
                    cNode = xmlDoc1.SelectSingleNode("//response");

                    int totalRows = 0;
                    if (pageNo == "1")
                    {
                        XmlNode tnode = cNode.Attributes["totalrows"];
                        if (tnode == null)
                        {
                            totalRows = 0;
                        }
                        else
                        {
                            totalRows = Convert.ToInt32(tnode.Value);
                            cNode.Attributes.RemoveNamedItem("totalrows");
                        }
                        HttpContext.Current.Session["s_noofpages"] = totalRows;
                    }
                    else
                    {
                        totalRows = Convert.ToInt32(HttpContext.Current.Session["s_noofpages"]);
                    }

                    StringWriter sw = new StringWriter();
                    XmlTextWriter xw = new XmlTextWriter(sw);
                    cNode.WriteTo(xw);

                    string ires2 = null;
                    ires2 = sw.ToString();

                    XmlDocument xmlDoc2 = new XmlDocument();
                    XmlNodeList productNodes2 = default(XmlNodeList);
                    XmlNodeList baseDataNodes2 = default(XmlNodeList);
                    xmlDoc2.LoadXml(ires2);

                    productNodes2 = xmlDoc2.SelectNodes("//row");
                    ArrayList headNames = new ArrayList();
                    int p = 0;
                    foreach (XmlNode productNode2 in productNodes2)
                    {
                        if (p > 0)
                        {
                            break; // TODO: might not be correct. Was : Exit For
                        }
                        baseDataNodes2 = productNode2.ChildNodes;
                        foreach (XmlNode baseDataNode2 in baseDataNodes2)
                        {
                            headNames.Add(baseDataNode2.Attributes["cap"].Value);
                        }
                        p = p + 1;
                    }
                    string headRow = "";
                    string fgRows = "";
                    int idx = 0;
                    for (idx = 0; idx <= headNames.Count - 1; idx++)
                    {
                        if (idx == 0)
                            headRow = headRow + "<th></th>";
                        else
                            headRow = headRow + "<th>" + headNames[idx].ToString() + "</th>";
                    }

                    //Remove attribute Cap
                    XmlDocument xmlDoc3 = new XmlDocument();
                    xmlDoc3.LoadXml(sw.ToString());

                    XmlNodeList productNodes3 = default(XmlNodeList);
                    XmlNodeList baseDataNodes3 = default(XmlNodeList);

                    productNodes3 = xmlDoc3.SelectNodes("//row");

                    foreach (XmlNode productNode3 in productNodes3)
                    {
                        baseDataNodes3 = productNode3.ChildNodes;
                        foreach (XmlNode baseDataNode3 in baseDataNodes3)
                        {
                            baseDataNode3.Attributes.RemoveNamedItem("cap");
                        }
                    }

                    string nXml = null;
                    nXml = xmlDoc3.OuterXml;

                    XmlNodeList dataNodes;
                    dataNodes = xmlDoc3.SelectNodes("//response/row");
                    foreach (XmlNode dataNode in dataNodes)
                    {
                        XmlNodeList baseDataNodes = dataNode.ChildNodes;
                        ArrayList ExitFld = new ArrayList();
                        ArrayList ExitFldVal = new ArrayList();
                        string cRows = "";
                        int ii = 0;
                        foreach (XmlNode baseDataNode in baseDataNodes)
                        {
                            if (ii == 0)
                            {
                                cRows = "<td><div class=\"form-check form-check-custom form-check-solid\"><input type=radio name='radioselect' value=" + baseDataNode.InnerText + " class=\"form-check-input\" onclick=loadTstruct(this.value);></div></td>";
                            }
                            else
                            {
                                string strVal = CheckSpecialChars(baseDataNode.InnerText);
                                cRows = cRows + "<td><label>" + strVal + "</label></td>";
                            }
                            ii++;
                        }

                        fgRows = fgRows + "<tr>" + cRows + "</tr>";
                    }

                    string fgHTML = "<table id='grdSearchRes' class='gridData dataTable table border-gray-300 overflow-auto'><thead><tr>" + headRow + "</tr></thead><tbody>" + fgRows + "</tbody></table>";

                    int _pageSize = int.Parse(pageSize);
                    double pg = (int)totalRows / (int)_pageSize;
                    int pg1 = (int)Math.Floor(pg);
                    if ((totalRows % _pageSize) > 0)
                    {
                        pg1 += 1;
                    }
                    string _lbltext = "";
                    int pgNo = 0;
                    if (pageNo == "1")
                    {
                        for (pgNo = 1; pgNo <= pg1; pgNo++)
                        {
                            _lbltext += "," + pgNo.ToString();
                        }
                    }
                    if (totalRows > 0)
                        _lbltext += "♠Total no. of records: " + totalRows + "♠" + " of " + pg1;

                    return _lbltext + "♠♠" + fgHTML;
                }
                else
                {
                    res = res.Replace(Constants.ERROR, string.Empty);
                    res = res.Replace("</error>", string.Empty);
                    res = res.Replace("\n", string.Empty);
                    return "Error♠" + res;
                }
            }
            else
            {
                return "Error♠" + Constants.DUPLICATESESS;
            }
        }
        catch (Exception ex)
        {
            string sid = HttpContext.Current.Session["nsessionid"].ToString();
            logobjs.CreateLog("Loading Search List." + ex.Message, sid, "GetTstSearchData-error", "new");
            return "Error♠" + Constants.DUPLICATESESS;
        }
    }

    public static bool IsAllDigitsNew(string s)
    {
        return s.All(char.IsDigit);
    }

    [WebMethod]
    public static void StoreLsTstHtml(string _transid, string _tstHtml)
    {
        try
        {
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            string _flKey = fdrObj.MakeKeyName(Constants.REDISTSTHTMLLS, _transid);
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                _flKey = Constants.REDISTSTHTMLLSMob;
            string formloadsbuKey = "";
            if (HttpContext.Current.Session["formloadsubkey-" + _transid] != null)
            {
                formloadsbuKey = HttpContext.Current.Session["formloadsubkey-" + _transid].ToString();
                HttpContext.Current.Session.Remove("formloadsubkey-" + _transid);
            }
            FDW fdwObj = new FDW();
            fdwObj.HashSetKey(_flKey, formloadsbuKey, _tstHtml);
        }
        catch (Exception ex) { }
    }

    public string GetLsTstHtml(string _transid, string _thisFlSubKey)
    {
        string _tstHtml = string.Empty;
        try
        {
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            string _flKey = fdrObj.MakeKeyName(Constants.REDISTSTHTMLLS, _transid);
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                _flKey = Constants.REDISTSTHTMLLSMob;
            _tstHtml = fdrObj.HashGetKey(_flKey, _thisFlSubKey);
        }
        catch (Exception) { }
        return _tstHtml;
    }

    public static string GetTstHtmllsGlobalVars(string transId)
    {
        string _res = string.Empty;
        try
        {
            Util.Util utilobj = new Util.Util();
            string _storetstHtml = Constants.REDISTSTHTMLLSGLOBALVARS;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj != null)
            {
                _res = fObj.StringFromRedis(utilobj.GetRedisServerkey(_storetstHtml, transId), schemaName);
            }

            if (_res == string.Empty && HttpContext.Current.Session["tstHtmlLsGlbVars-" + transId] != null)
            {
                _res = HttpContext.Current.Session["tstHtmlLsGlbVars-" + transId].ToString();
            }
        }
        catch (Exception ex)
        { }
        return _res;
    }

    public string CreatetstGlbVarKey(string _res)
    {
        string res = string.Empty;
        try
        {
            XmlDocument xmlDocgbl = new XmlDocument();
            if (HttpContext.Current.Session["axGlobalVars"] != null)
                xmlDocgbl.LoadXml(HttpContext.Current.Session["axGlobalVars"].ToString());
            string[] _theseVars = _res.Split(',');
            foreach (string glbVar in _theseVars)
            {
                XmlNodeList parms = xmlDocgbl.SelectNodes("globalvars/" + glbVar);
                if (parms.Count > 0)
                    res += glbVar + ":" + parms[0].InnerText + "♦";
            }
            if (res != "")
            {
                res = res.TrimEnd('♦');
            }
        }
        catch (Exception ex)
        {

        }
        return res;
    }

    [WebMethod]
    public static void ClearCacheTstKeys(string Transid)
    {
        try
        {
            FDW fdwObj = new FDW();
            if (HttpContext.Current.Session["dbuser"] != null)
                fdwObj.Initialize(HttpContext.Current.Session["dbuser"].ToString());

            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            string tKey = schemaName + "-" + Transid + "-*";
            ArrayList tstCachedKeys = fObj.GetAllKeys(tKey, true);
            for (int i = 0; i < tstCachedKeys.Count; i++)
            {
                string keyName = tstCachedKeys[i].ToString();
                fdwObj.DeleteAllKeys(keyName, schemaName);
            }
        }
        catch (Exception ex) { }
    }

    [WebMethod]
    public static object renderHtmlPagesFiles(string htmlFileName, string[] cssFileName, string[] jsFileName, bool getTemplate)
    {
        var returnObject = new Dictionary<string, object>();

        string axpertWebDir = string.Empty;
        if (ConfigurationManager.AppSettings["axpertWebDirPath"] != null && ConfigurationManager.AppSettings["axpertWebDirPath"].ToString() != "" && !getTemplate)
            axpertWebDir = ConfigurationManager.AppSettings["axpertWebDirPath"].ToString();
        else
            axpertWebDir = HttpRuntime.AppDomainAppPath;
        string projName = HttpContext.Current.Session["project"].ToString();
        string cssFileDir = getTemplate ? axpertWebDir + "AxpTemplates\\css\\" : axpertWebDir + projName + "\\HTMLPages\\css\\";
        string jsFileDir = getTemplate ? axpertWebDir + "AxpTemplates\\js\\" : axpertWebDir + projName + "\\HTMLPages\\js\\";
        string htmlFileDir = getTemplate ? axpertWebDir + "AxpTemplates\\" : axpertWebDir + projName + "\\HTMLPages\\";


        returnObject.Add("html", getFileContent(htmlFileDir + htmlFileName));

        Dictionary<string, string> cssFiles = new Dictionary<string, string>();

        foreach (string css in cssFileName)
        {
            cssFiles.Add(css, getFileContent(cssFileDir + css));
        }
        returnObject.Add("css", cssFiles);

        Dictionary<string, string> jsFiles = new Dictionary<string, string>();

        foreach (string js in jsFileName)
        {
            jsFiles.Add(js, getFileContent(jsFileDir + js));
        }

        returnObject.Add("js", jsFiles);

        return returnObject;
    }
    private static string getFileContent(string fullPath)
    {
        string returnCOntent = string.Empty;
        try
        {
            returnCOntent = File.ReadAllText(fullPath);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Load HtmlPages File - \n\tPath- " + fullPath + "\n\tError - " + ex.Message, HttpContext.Current.Session.SessionID, "Load HtmlPages File", "");
        }
        return returnCOntent;
    }

    [WebMethod]
    public static string removeFromMenuAndFolder(string pageNo)
    {
        Util.Util util = new Util.Util();
        if (HttpContext.Current.Session["project"] == null)
            return util.SESSTIMEOUT;

        string axpertWebDir = string.Empty;
        string projectName = HttpContext.Current.Session["project"].ToString();
        string curSessId = HttpContext.Current.Session["nsessionid"].ToString();
        string result = string.Empty;
        string inputXML = string.Empty;

        if (ConfigurationManager.AppSettings["axpertWebDirPath"] != null && ConfigurationManager.AppSettings["axpertWebDirPath"].ToString() != "")
            axpertWebDir = ConfigurationManager.AppSettings["axpertWebDirPath"].ToString() + projectName;
        else
            axpertWebDir = HttpRuntime.AppDomainAppPath + projectName;

        LogFile.Log logObj = new LogFile.Log();

        string[] filePaths = { axpertWebDir + "\\HTMLPages\\", axpertWebDir + "\\HTMLPages\\css\\", axpertWebDir + "\\HTMLPages\\js\\" };
        if (pageNo != "")
        {
            try
            {
                foreach (string path in filePaths)
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    if (di.Exists)
                    {
                        string[] allFiles = Directory.GetFiles(path);
                        foreach (string file in allFiles)
                        {
                            if (file.Contains("_" + pageNo + "."))
                            {
                                File.Delete(file);
                            }
                        }
                    }
                }
                result = "done";
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Delete HtmlPages File - \n\tPage Name- 'HP" + pageNo + "' \n\tError - " + ex.Message, curSessId, "Delete HtmlPages File", "");
                result = ex.Message;
            }
        }
        else
        {
            logObj.CreateLog("Delete HtmlPages File - \n\tPage Name- 'HP" + pageNo + "' \n\tError - pageno value is not found", curSessId, "Delete HtmlPages File", "");
            result = "PageNo is not found";
        }
        return result;
    }

    [WebMethod]
    public static string htmlPagePublish(string htmlContent, string[] cssFileName, string[] cssContents, string[] jsFileName, string[] jsContents, string pageCaption, bool addToMenu, string pageNo, bool isNewPage)
    {
        Util.Util util = new Util.Util();
        if (HttpContext.Current.Session["project"] == null)
            return util.SESSTIMEOUT;
        LogFile.Log logObj = new LogFile.Log();
        string pageName = pageCaption.Replace(" ", "_") + "_" + pageNo + ".html";
        string curSessId = HttpContext.Current.Session["nsessionid"].ToString();
        try
        {
            string axpertWebDir = string.Empty;
            if (ConfigurationManager.AppSettings["axpertWebDirPath"] != null && ConfigurationManager.AppSettings["axpertWebDirPath"].ToString() != "")
                axpertWebDir = ConfigurationManager.AppSettings["axpertWebDirPath"].ToString();
            else
                axpertWebDir = HttpRuntime.AppDomainAppPath;

            string projName = HttpContext.Current.Session["project"].ToString();
            string cssFileDir = axpertWebDir + projName + "\\HTMLPages\\css\\";
            string jsFileDir = axpertWebDir + projName + "\\HTMLPages\\js\\";
            string htmlFileDir = axpertWebDir + projName + "\\HTMLPages\\";

            int i = 0, j = 0;
            foreach (string Name in cssFileName)
            {
                string cssFile = cssFileDir + Name;
                DirectoryInfo dir = new DirectoryInfo(cssFileDir);
                if (!dir.Exists)
                {
                    dir.Create();
                    File.Create(cssFile).Close();
                }
                else
                {
                    if (!File.Exists(cssFile))
                        File.Create(cssFile).Close();
                    else
                    {
                        File.Delete(cssFile);
                        File.Create(cssFile).Close();
                    }
                }

                using (StreamWriter writer = new StreamWriter(cssFile, false))
                {
                    writer.WriteLine(cssContents[i++]);
                    writer.Flush();
                }
            }
            foreach (string Name in jsFileName)
            {
                string jsFile = jsFileDir + Name;
                DirectoryInfo dir = new DirectoryInfo(jsFileDir);
                if (!dir.Exists)
                {
                    dir.Create();
                    File.Create(jsFile).Close();
                }
                else
                {
                    if (!File.Exists(jsFile))
                        File.Create(jsFile).Close();
                    else
                    {
                        File.Delete(jsFile);
                        File.Create(jsFile).Close();
                    }

                }
                using (StreamWriter writer = new StreamWriter(jsFile, false))
                {
                    writer.WriteLine(jsContents[j++]);
                    writer.Flush();
                }
            }

            string htmlFile = htmlFileDir + pageName;
            htmlContent = util.ReverseCheckSpecialChars(htmlContent);
            DirectoryInfo di = new DirectoryInfo(htmlFileDir);
            if (!di.Exists)
            {
                di.Create();
                File.Create(htmlFile).Close();
            }
            else
            {
                if (!File.Exists(htmlFile))
                    File.Create(htmlFile).Close();
                else
                {
                    File.Delete(htmlFile);
                    File.Create(htmlFile).Close();
                }
            }
            using (StreamWriter writer = new StreamWriter(htmlFile, false))
            {
                writer.WriteLine(htmlContent);
                writer.Flush();
            }
            return "done";
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Save HtmlPages File - \n\tPage Name- " + pageName + "\n\tError - " + ex.Message, curSessId, "Save HtmlPages File", "");
            return ex.Message;
        }
    }

    [WebMethod]
    public static string GetFromDBJSON(string funName, string funParams)
    {
        string ResJSON = string.Empty;
        try
        {
            string funParamVals = "";
            if (funParams != "")
            {
                string[] paramList = funParams.Split(',');
                XmlDocument xmlDocgbl = new XmlDocument();
                xmlDocgbl.LoadXml(HttpContext.Current.Session["axGlobalVars"].ToString());
                foreach (var paramName in paramList)
                {
                    XmlNode findNode = xmlDocgbl.SelectSingleNode("//" + paramName);
                    if (findNode != null)
                        funParamVals += "'" + findNode.InnerText + "',";
                    else
                        funParamVals += "'',";
                }
                if (funParamVals != "")
                    funParamVals = funParamVals.Substring(0, funParamVals.Length - 1);
            }

            string sqlFunName = string.Empty;
            string _dbType = string.Empty;
            if (HttpContext.Current.Session["axdb"] != null && HttpContext.Current.Session["axdb"].ToString() != string.Empty)
                _dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
            if (_dbType != "" && _dbType.ToLower() == "oracle")
                sqlFunName = "select " + funName + "(" + funParamVals + ") from dual";
            else if (_dbType != "")
                sqlFunName = "select " + funName + "(" + funParamVals + ")";

            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            string result = asbExt.ExecuteSQL("", sqlFunName);
            if (result != "")
            {
                //ResJSON = result;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                XmlNode xNode = xmlDoc.SelectSingleNode("//sqlresultset/response/row/column1");
                if (xNode != null)
                    ResJSON = xNode.InnerText;
                else
                {
                    XmlNode erroNode = xmlDoc.SelectSingleNode("//sqlresultset/response");
                    ResJSON = "error:" + erroNode.Attributes["value"].InnerText;
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Get From DB Variable JSON:" + ex.Message, HttpContext.Current.Session.SessionID, "GetFromDBJSON", "new");
        }
        return ResJSON;
    }

    [WebMethod]
    public static string GetInterExterResources()
    {
        Util.Util utilwf = new Util.Util();
        string res = string.Empty;
        try
        {
            string proj = HttpContext.Current.Session["project"].ToString();
            string intRes = utilwf.GetAxARMInternalResources(proj);
            string extRes = utilwf.GetAxARMExternalResources(proj);
            JObject intResObj;
            try
            {
                intResObj = JObject.Parse(intRes);
            }
            catch
            {
                intResObj = new JObject();
            }
            JObject extResObj = new JObject();
            if (!string.IsNullOrWhiteSpace(extRes) && extRes != "noarmconnection")
            {
                try
                {
                    JObject obj = JObject.Parse(extRes);
                    JToken namedUrlsToken = obj["NamedURLS"];

                    if (namedUrlsToken != null && namedUrlsToken.HasValues)
                    {
                        extResObj = JObject.Parse(namedUrlsToken.ToString());
                    }
                }
                catch
                {
                    extResObj = new JObject();
                }
            }
            var resourcesObj = new JObject();
            resourcesObj["intres"] = intResObj;
            resourcesObj["extres"] = extResObj;
            var finalObj = new JObject();
            finalObj["resources"] = resourcesObj;
            res = finalObj.ToString();
        }
        catch
        {
            res = "{\"resources\": { \"intres\": {}, \"extres\": {} }}";
        }
        return res;
    }

}
