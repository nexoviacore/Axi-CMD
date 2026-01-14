using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Linq;
using System.Globalization;
using System.IO.Compression;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Services;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Net;
using System.Net.Mail;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.Threading;
using CacheMgr;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;

public partial class iview : System.Web.UI.Page
{
    #region Variables
    public IviewData objIview;
    public IviewParams objParams;
    public Custom customObj = Custom.Instance;
    string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    Util.Util util;
    LogFile.Log logobj = new LogFile.Log();
    ASB.WebService webService = new ASB.WebService();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    string gridPageSize = "";
    static JObject jsonForGrid = new JObject();
    string strGlobalVar = string.Empty;
    public string tst_Scripts = string.Empty;
    public string direction = "ltr";
    public string btn_direction = "end";
    public string bread_direction = "start";
    public bool isSqlFld = false;
    public ArrayList arrFillList = new ArrayList();
    public ArrayList arrFillListDataAttr = new ArrayList();
    public bool isSelectWithMultiColumn = false;
    public string isSelectParamsString = string.Empty;
    public string resXhtm;
    public bool HasCKB = false;
    public bool paramsExist;
    public string strFillDepPName = string.Empty;
    string pXml = string.Empty;
    string clientCulture = null;
    public string lblHeading = string.Empty;
    public bool IsParamsVisible = false;
    bool callParamPlusStructure = false;
    bool isCache = false;
    bool dataFetched = false;
    // create arrays for Header Names and width
    ArrayList headName = new ArrayList();
    ArrayList colWidth = new ArrayList();
    ArrayList colType = new ArrayList();
    ArrayList colApplyComma = new ArrayList();
    ArrayList colHide = new ArrayList();
    ArrayList colFld = new ArrayList();
    ArrayList colDec = new ArrayList();
    ArrayList colHlink = new ArrayList();
    ArrayList colHlinkPop = new ArrayList();
    ArrayList colRefreshParent = new ArrayList();
    IDictionary<string, string> actRefreshParent = new Dictionary<string, string>();
    ArrayList colHlinktype = new ArrayList();
    ArrayList colMap = new ArrayList();
    ArrayList colHAction = new ArrayList();
    ArrayList colHlink_c = new ArrayList();
    ArrayList colHlinktype_c = new ArrayList();
    ArrayList colMap_c = new ArrayList();
    ArrayList colHAction_c = new ArrayList();
    ArrayList colAlign = new ArrayList();
    ArrayList colNoPrint = new ArrayList();

    ArrayList colNoRepeat = new ArrayList();
    ArrayList colZeroOff = new ArrayList();


    //Arrays for toolbar button alignment
    ArrayList arrButtons = new ArrayList();
    ArrayList arrBtnLeftVals = new ArrayList();
    ArrayList arrSortedButtons = new ArrayList();
    ArrayList arrTempBtnLeft = new ArrayList();
    ArrayList pivotGroupHeaderNames = new ArrayList();
    ArrayList pivotStartCol = new ArrayList();
    ArrayList pivotEndCol = new ArrayList();
    ArrayList ivhead = new ArrayList();
    ArrayList actionBtns = new ArrayList();
    ArrayList iviewParams = new ArrayList();
    ArrayList iviewParamValues = new ArrayList();
    ArrayList submerge = new ArrayList();
    ArrayList submergecol = new ArrayList();
    ArrayList showHiddenCols = new ArrayList();

    IDictionary<string, string> paramss = new Dictionary<string, string>();
    public static StringBuilder paramssBuilder = new StringBuilder();

    int ivAttachRid = -1;
    int ivAttachTransid = -1;
    int ivAttachRowNo = -1;
    int ivAttExt = -1;

    ArrayList FilesIndexes = new ArrayList();
    ArrayList arrHdnColMyViews = new ArrayList();

    public DataTable dtFilterConds = new DataTable();
    public string iviewcap = string.Empty;

    //TreeMethod
    public int root_class_index = -1;
    public int root_account_index = -1;
    public int root_atype_index = -1;
    public int iframe_index = -1;

    public string @params = string.Empty;
    public string iName = string.Empty;
    public string ActBut = string.Empty;
    public string defaultBut = string.Empty;
    //public string TaskBut = string.Empty;
    public string actionButvs = string.Empty;
    public string enableBackForwButton = string.Empty;
    public string noRec = string.Empty;
    public string cac_order = string.Empty;
    public string cac_pivot = string.Empty;
    public string srNoColumnName = string.Empty;
    int totalRows = 0;
    int directDbtotalRows = 0;
    int dataRows = 0;
    public string tid = string.Empty;
    public StringBuilder tskList = new StringBuilder();
    public string AxRole = string.Empty;
    public string proj = string.Empty;
    public string sid = string.Empty;
    public string user = string.Empty;
    public string language = string.Empty;
    public string errLog;
    public int topwidth;
    private string toolBarHtml = string.Empty;
    public dynamic toolbarJSON = new JObject();
    bool buttonsCreated = false;
    int checkcnt = 0;
    string fileName;
    public bool isCallWS = false;
    string pivothead = string.Empty;
    bool onlyParams = false;
    bool hideChkBox = false;
    bool incrementPivot = false;

    public StringBuilder paramHtml = new StringBuilder();
    public StringBuilder strJsArrays = new StringBuilder();
    public StringBuilder strParamDetails = new StringBuilder();
    public StringBuilder strBreadCrumb = new StringBuilder();
    public string breadCrumb = string.Empty;
    public StringBuilder filterCondDispText = new StringBuilder();
    public StringBuilder filterCondDispFullText = new StringBuilder();

    public static StringBuilder sortingMyView = new StringBuilder();
    public static StringBuilder hiddenColMyView = new StringBuilder();
    public static StringBuilder filterColMyView = new StringBuilder();


    public static StringBuilder sortingMyView1 = new StringBuilder();
    public static StringBuilder hiddenColMyView1 = new StringBuilder();
    public static StringBuilder filterColMyView1 = new StringBuilder();
    public static string paramValueMyView = string.Empty;

    public static ArrayList arrMyViews = new ArrayList();

    public ArrayList arrVisibleParamList = new ArrayList();

    public string strBreadCrumbBtns = string.Empty;
    string IsSqlPagination = "false";
    Boolean validateParamOnGo = false;
    int noOfPages = 0;
    int currentPageNo = 0;
    string ivCaption = string.Empty;
    DataTable navigationInfo = new DataTable();
    private bool IsIviewPop = false;
    private bool AxSplit = false;
    public string hidetoolbar = "false";
    public string hideParameters = "false";

    //Below variables are used for caching the iview pages for sqlpagination
    XmlDocument xmlParamDoc = new XmlDocument();
    bool isParamSame = true;
    bool isPageExist = false;
    public string loadString = string.Empty;
    public int gvWidth = 0;
    public string axp_refresh = "false";
    public double webTime2;
    public static StringBuilder strTimetaken = new StringBuilder();
    int displayRowCnt = 20;
    string Ivkey = string.Empty;
    string IvParamKey = string.Empty;
    public string enableFilter = "false";
    public bool isShowHideColExist = false;
    public string refreshTime = string.Empty;

    string AsbTime = string.Empty;
    public bool isCloudApp = false;
    public string schemaName = string.Empty;

    public bool hasBuildAccess = false;

    ////if iview web service will return row count or not
    public bool getIviewRowCount = false;

    public string axpResp = "false";
    public string langType = "en";
    public string PrintTitle = "";
    public string paramsCache = String.Empty;
    private bool isFromPopup = false;

    public bool isMobile = false;

    public string redisLoadKey = string.Empty;
    public string redisLoadType = string.Empty;

    public bool requestJSON = true;

    public string notifyTimeout = string.Empty;

    public string iviewButtonStyle = "classic";

    public string inputControlType = "border";

    bool dotNetSubmit = false;

    bool isException = false;

    string originalRecsPerPage = string.Empty;

    dynamic flKey = null;

    ExecTrace ObjExecTr = ExecTrace.Instance;
    public string requestProcess_logtime = string.Empty;
    public string serverprocesstime = string.Empty;
    public string rsaPublicKey = string.Empty;

    public StringBuilder ivVarNode = new StringBuilder();

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
    /// Onload get the construct params if present else getiview data and display.
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && Request.QueryString["hdnbElapsTime"] != null)
        {
            string browserElapsTime = Request.QueryString["hdnbElapsTime"] != null ? Request.QueryString["hdnbElapsTime"] : "0";
            requestProcess_logtime += ObjExecTr.WireElapsTime(browserElapsTime);
        }
        else if (IsPostBack && hdnbElapsTimeGo.Value != "")
        {
            ObjExecTr.SetCurrentTime();
        }
        if (Request.Form["reqProc_logtime"] != null)
            requestProcess_logtime += Request.Form["reqProc_logtime"];
        if (!IsPostBack && Request.QueryString["tstcaption"] != null)
        {
            Title = "Listview";
        }
        try
        {
            util = new Util.Util();
            util.IsValidSession();
            ResetSessionTime();
            if (HttpContext.Current.Session["AxPrintTitle"] != null)
                PrintTitle = HttpContext.Current.Session["AxPrintTitle"].ToString();
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString().ToLower() == "true")
                isMobile = true;
            if (Request.UrlReferrer != null)
            {
            }
            DateTime asStart = DateTime.Now;
            if (Session["project"] == null || Session["axApps"] == null || Convert.ToString(Session["project"]) == string.Empty)
            {
                SessExpires();
                return;
            }
            else
            {
                if (!util.CheckValidLogin())
                {
                    SessExpires();
                    return;
                }
                if (Request.QueryString["ivname"] != null)
                    iName = Request.QueryString["ivname"];
                if (iName != string.Empty)
                {
                    if (IsPostBack && hdnparamValues.Value != "" && Session["xmlPrivateKey-" + iName] != null)
                    {
                        try
                        {
                            var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(hdnparamValues.Value);
                            string encryptedAesKey = payload.key;
                            string aesEncryptedData = payload.data;

                            using (RSA rsa = RSA.Create())
                            {
                                rsa.FromXmlString(Session["xmlPrivateKey-" + iName].ToString());
                                byte[] encryptedKeyBytes = Convert.FromBase64String(encryptedAesKey);
                                byte[] decryptedKeyBytes = rsa.Decrypt(encryptedKeyBytes, RSAEncryptionPadding.Pkcs1);
                                string aesKeyBase64 = Encoding.UTF8.GetString(decryptedKeyBytes);
                                byte[] keyBytes = Convert.FromBase64String(aesKeyBase64);

                                // Decrypt AES data
                                byte[] aesBytes = Convert.FromBase64String(aesEncryptedData);
                                var decrypted = DecryptWithAesBase64(aesEncryptedData, keyBytes); // Helper function
                                hdnparamValues.Value = decrypted;
                            }
                        }
                        catch (Exception)
                        {
                            throw new Exception("Could not process the request. Please try again.");
                        }
                    }
                    else
                    {
                        //var rsa = new RSACryptoServiceProvider(2048);
                        var rsa = new RSACryptoServiceProvider(4096);
                        string xmlPublicKey = rsa.ToXmlString(false);
                        Session["xmlPrivateKey-" + iName] = rsa.ToXmlString(true);
                        rsaPublicKey = ConvertXmlPublicKeyToPem(xmlPublicKey).Replace("\r", "").Replace("\n", "\\n");
                    }
                }

                if (util.IsValidQueryString(Request.RawUrl, true) == false)
                {
                    requestProcess_logtime += "Server - " + Constants.INVALIDURL + " ♦ ";
                    HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.INVALIDURL + "*♠*" + requestProcess_logtime);
                }
                if (Session["IsFromChildWindow"] != null && Session["IsFromChildWindow"].ToString() == "true")
                    isFromPopup = true;

                getIviewRowCount = false;

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

                if (Request.Form.Count > 0 && Request.Form["redisLoadKey"] != null)
                {
                    redisLoadKey = Request.Form["redisLoadKey"];
                }

                if (!IsPostBack)
                {
                    iName = Request.QueryString["ivname"];

                    logobj.CreateLog("Loading iview.aspx", sid, "openiview-dev-" + iName, "new");

                    requestJSON = true;// GetRequestType();

                    notifyTimeout = util.GetAdvConfigs("notification time interval");

                    if (flKey == null)
                    {
                        flKey = GenerateGlobalSmartViewsKey(iName, Request.QueryString["tstcaption"] != null);
                    }

                    #region New Iview Caching logic
                    if (!requestJSON)
                    {
                        objIview = new IviewData();
                        objParams = new IviewParams();
                    }
                    else
                    {
                        user = Session["user"].ToString();
                        user = util.CheckSpecialChars(user);

                        objIview = GetGlobalSmartViews(iName, flKey);
                        if (objIview == null)
                        {
                            objIview = new IviewData();
                        }
                        if (objParams == null)
                        {
                            objParams = new IviewParams();
                        }
                    }

                    #endregion



                    objIview.getIviewRowCount = getIviewRowCount;
                    objIview.requestJSON = requestJSON;
                    objIview.notifyTimeout = notifyTimeout;
                    strTimetaken = new StringBuilder();
                    arrMyViews.Clear();
                    CleardtFilterCond();
                    GetGlobalVariables();
                    if (!IsIviewPop)
                    {
                        if (!AxSplit)
                            util.DeleteTstIvObject(iName);
                        ClearNavigationData();
                        util.RemovelvListPageLoad();
                    }
                    if (util.BreadCrumb)
                    {
                        GetBreadCrumb();
                    }

                    //GetAxpStructConfig();

                    try
                    {
                        if (objIview.RetainIviewParams || (Session["IsFromChildWindow"] != null && Session["IsFromChildWindow"].ToString() == "true"))
                        {
                            loadString = webService.GetIviewNavigationData(iName);
                        }

                        DateTime starttime = DateTime.Now;
                        if (!objIview.requestJSON)
                        {
                            GetIviewStructure(iName);
                        }
                        else
                        {
                            GetIviewStructureNew(iName);
                        }
                        if (objIview.StructureXml != string.Empty && objIview.StructureXml != "false" && objIview.StructureXml != "")
                        {

                            GenericRedisFunction(Title, objIview.IName, objIview.StructureXml, schemaName);

                        }


                        double totTime = DateTime.Now.Subtract(starttime).Milliseconds;

                    }
                    catch (Exception ex)
                    {
                        if (Constants.RELEASINGSOONPAGES != "" && (Constants.RELEASINGSOONPAGES.IndexOf(iName + ",") > -1 || Constants.RELEASINGSOONPAGES.IndexOf("," + iName) > -1) && ex.Message.IndexOf("<error>List index out of bounds (0)</error>") > -1)
                        {
                            HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.RELEASINGSOON, false);
                        }
                    }

                    IncludeCustomLinksNew(objIview);

                    hdnParamChngd.Value = "false";
                    cac_order = sid + "order";
                    cac_pivot = sid + iName + "pivot";
                    if ((Session["backForwBtnPressed"] != null && !Convert.ToBoolean(Session["backForwBtnPressed"])) && Request.QueryString["homeicon"] == null)
                    {
                        if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.IndexOf("ivtoivload.aspx") == -1 && Request.RequestType != "POST")
                        {

                            string strUrlParams = hdnparamValues.Value;

                            strUrlParams = strUrlParams.Replace("~", "=");
                            strUrlParams = strUrlParams.Replace("¿", "&");
                            util.UpdateNavigateUrl("ivtoivload.aspx?ivname=" + iName + "&" + strUrlParams);

                        }
                    }
                    //remove key from querystring
                    if (Request.QueryString["homeicon"] != null)
                    {
                        System.Reflection.PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                        isreadonly.SetValue(this.Request.QueryString, false, null);
                        this.Request.QueryString.Remove("homeicon");
                    }
                    Session["backForwBtnPressed"] = false;
                    UpdateRecordsPerPage();
                    enableBackForwButton = "<script type=\'text/javascript\' >var axTheme='" + Session["themeColor"].ToString() + "';</script>";
                    divFilterCond.InnerHtml = string.Empty;
                    Ivkey = util.GetIviewId(iName);
                    IvParamKey = Ivkey + "_param";
                    hdnKey.Value = Ivkey;

                    if (HttpContext.Current.Session["isDupTab"] != null && HttpContext.Current.Session["isDupTab"].ToString().StartsWith("true-"))
                    {
                        string _thisDupTab = HttpContext.Current.Session["isDupTab"].ToString();
                        _thisDupTab = _thisDupTab.Split('-')[1];
                        HttpContext.Current.Session["isDupTab"] = "false";
                        if (Session["tstivobjkey-duptab-" + _thisDupTab] != null && Session["tstivobjkey-duptab-" + _thisDupTab].ToString() != string.Empty)
                            Session["tstivobjkey-duptab-" + _thisDupTab] = Session["tstivobjkey-duptab-" + _thisDupTab].ToString() + "," + Ivkey + "," + IvParamKey;
                        else
                            Session["tstivobjkey-duptab-" + _thisDupTab] = Ivkey + "," + IvParamKey;
                    }
                    else
                    {
                        if (Session["tstivobjkey"] != null && Session["tstivobjkey"].ToString() != string.Empty)
                            Session["tstivobjkey"] = Session["tstivobjkey"].ToString() + "," + Ivkey + "," + IvParamKey;
                        else
                            Session["tstivobjkey"] = Ivkey + "," + IvParamKey;
                    }
                    Session[Ivkey] = objIview;
                    Session[Ivkey + "_param"] = objParams;
                    arrHdnColMyViews.Clear();
                    showHiddenCols.Clear();

                    hdnToggleDimmer.Value = "false";

                }
                else
                {
                    Ivkey = hdnKey.Value;
                    objIview = (IviewData)Session[Ivkey];
                    objParams = (IviewParams)Session[Ivkey + "_param"];
                    requestJSON = objIview.requestJSON;
                    notifyTimeout = objIview.notifyTimeout;
                    SetGlobalVariables();
                    Session["AxFromHypLink"] = objIview.FromHyperLink;
                    if ((Session["backForwBtnPressed"] != null && !Convert.ToBoolean(Session["backForwBtnPressed"])) && Request.QueryString["homeicon"] == null)
                    {
                        if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.IndexOf("ivtoivload.aspx") == -1 && Request.RequestType != "POST")
                        {
                            string strUrlParams = hdnparamValues.Value;
                            if (strUrlParams != string.Empty)
                            {
                                strUrlParams = strUrlParams.Replace("~", "=");
                                strUrlParams = strUrlParams.Replace("¿", "&");
                                util.UpdateNavigateUrl("ivtoivload.aspx?ivname=" + iName + "&" + strUrlParams);
                            }
                            util.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri);
                        }
                    }

                    CheckCustomIview();

                    if (hdnIvRefresh.Value == "true")
                    {
                        isPageExist = false;
                        hdnIvRefresh.Value = "false";
                    }

                    //In postback event the sub heading will be reconstructed


                    if (objIview.IvCaption != null)
                    {
                        ArrayList arrIvCap = (ArrayList)objIview.IvCaption;
                        ConstructSubHeading(arrIvCap);
                    }
                    if (objIview.CurrentPageNo != null)
                        Session["currentPageNo" + iName] = objIview.CurrentPageNo;
                    if (gridPageSize == "" && objIview.GrdPageSize != null)
                        gridPageSize = objIview.GrdPageSize;

                    if (!string.IsNullOrEmpty(hdnAct.Value))
                    {
                        hdnAct.Value = string.Empty;
                    }
                    else
                    {
                        fileName = "openiview-" + iName;
                        logobj.CreateLog(string.Empty, sid, fileName, string.Empty);
                        errLog = logobj.CreateLog("Loading iview.aspx after post back (get and load iview data)", sid, fileName, string.Empty);
                        if (hdnGo.Value == "refreshparams" || hdnGo.Value == "updateCache")
                        {
                            if (iName != "inmemdb")
                            {
                                GetParams();
                            }
                        }
                        if (hdnGo.Value == "Go" || hdnGo.Value == "clear" || hdnGo.Value == "TSSave")
                        {
                            logobj.CreateLog("Get and Load Iviewdata", sid, fileName, string.Empty);
                            DateTime asStart2 = DateTime.Now;
                            GetIviewData();
                            webTime2 = DateTime.Now.Subtract(asStart2).TotalMilliseconds;
                            strTimetaken.Append("GetIview-" + webTime2.ToString() + " ");
                            GridView2Wrapper.Visible = true;
                            if (objIview.AxpIsAutoSplit == "true")
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "assocateIframe", "callParentNew('assocateIframe(true)', 'function');", true);
                            dvFilteredRowCount.Visible = false;
                            dtFilterConds.Rows.Clear();
                            Session["dtFilterConds"] = dtFilterConds;
                            divFilterCond.InnerHtml = string.Empty;
                        }
                    }

                    string strtemp = hdnparamValues.Value;
                    strtemp = strtemp.Replace("'", "quot;");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ParamValues", "<script>SetParamValues('" + strtemp + "');evalParamExprHandler();AxWaitCursor(true);ShowDimmer(true);</script>", false);

                    hdnToggleDimmer.Value = "false";
                    Session[Ivkey] = objIview;
                    Session[Ivkey + "_param"] = objParams;

                }
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ivDataVar", "<script>" + ivVarNode.ToString() + ";</script>", false);
                if (objIview.requestJSON && !objIview.isObjFromCache && !objParams.ForceDisableCache)
                {
                    if (flKey == null)
                    {
                        flKey = GenerateGlobalSmartViewsKey(iName, Request.QueryString["tstcaption"] != null);
                    }
                    if (objParams.isIviewParamCache)
                        SetGlobalSmartViews(objIview, iName, flKey);

                    hdnisIviewParamCache.Value = objParams.isIviewParamCache.ToString();
                }

                setPageDirection();
                IsSqlPagination = IsSqlPagination.ToLower();
                ConstructBreadCrumb();
                if (string.IsNullOrEmpty(paramHtml.ToString()))
                {
                    if (hdnParamHtml.Value != "" && hdnGo.Value != "clear")
                        paramHtml.Append(hdnParamHtml.Value);
                    else
                        paramHtml.Append(objParams.ParamHtml);
                }
                if (string.IsNullOrEmpty(strJsArrays.ToString()) | strJsArrays.ToString() == "<script type=\"text/javascript\">")
                {
                    strJsArrays.Append(objParams.strJsArrays);
                }
                logobj.CreateLog("End Time : " + DateTime.Now.ToString(), sid, fileName, string.Empty);

                enableBackForwButton = "<script type=\'text/javascript\' > enableBackButton='" + Convert.ToBoolean(Session["enableBackButton"]) + "';" + " enableForwardButton='" + Convert.ToBoolean(Session["enableForwardButton"]) + "'; isSqlPagination='" + IsSqlPagination + "';var axTheme='" + Session["themeColor"].ToString() + "';var paramsExist=" + paramsExist.ToString().ToLower() + ";var isParamVisible='" + IsParamsVisible + "';var isFilterEnabled='" + objParams.IsFilterEnabled.ToString() + "';</script>";
                if (ConfigurationManager.AppSettings["isCloudApp"] != null)
                {
                    isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString());
                }

                iviewButtonStyle = objIview.iviewButtonStyle;

                inputControlType = objIview.inputControlType;

                string toolbarJsonString = toolbarJSON.ToString().Replace("\r\n", "");

                Page.ClientScript.RegisterStartupScript(GetType(), "set global var in iview", "<script>var isCloudApp = '" + isCloudApp.ToString() + "';var getIviewRowCount = " + getIviewRowCount.ToString().ToLower() + ";var totRowCount = '';</script>");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ivirHeaderData", "var ivirHeaderData = " + jsonForGrid + ";var isListView=" + (objIview.purposeString == "" ? "false" : "true") + ";var showParam=" + objParams.showParam.ToString().ToLower() + ";var dataFetched = " + dataFetched.ToString().ToLower() + ";var iviewWrap = " + util.IviewWrap.ToString().ToLower() + ";var isIviewPopup = " + IsIviewPop.ToString().ToLower() + ";var toolbarJSON=JSON.stringify(" + toolbarJsonString + ");var requestJSON = " + objIview.requestJSON.ToString().ToLower() + ";var iviewDataWSRows = " + objIview.iviewDataWSRows.ToString() + ";var isPivotReport = " + objIview.isPivotReport.ToString().ToLower() + ";var redisLoadType = '" + redisLoadType.ToString() + "';var redisLoadKey = '" + redisLoadKey.ToString() + "';var breadCrumbStr = '" + objIview.Menubreadcrumb.ToString().Replace("'", "\\'") + "';var iviewButtonStyle = '" + iviewButtonStyle.ToString() + "';var inputControlType = '" + inputControlType.ToString() + "';var originalRecsPerPage = '" + originalRecsPerPage.ToString() + "';var dotNetSubmit = " + dotNetSubmit.ToString().ToLower() + ";", true);

            }

            ShowHideFilters();

            if (hdnGo.Value == "Go" || hdnGo.Value == "clear")
            {
                webTime2 = DateTime.Now.Subtract(asStart).TotalMilliseconds;
                strTimetaken.Append("PageLoad2-" + webTime2.ToString() + " ");
            }
            else
            {
                webTime2 = DateTime.Now.Subtract(asStart).TotalMilliseconds;
                strTimetaken.Append("PageLoad1-" + webTime2.ToString() + " ");
            }

            if (hdnToggleDimmer.Value.ToLower() == "false")
            {
                hdnToggleDimmer.Value = "true";
                CloseDimmerJS();

            }

            if (Session["build"] != null && Session["build"].ToString() != "")
            {
                hasBuildAccess = System.Web.HttpContext.Current.Session["build"].ToString() == "T";
            }

            if (ConfigurationManager.AppSettings["timetaken"].ToString() == "true")
                lblTime.Text = strTimetaken.ToString();


            printRowsMaxLimitField.Value = Session["AxPrintRowsMaxLimit"].ToString();

            if (hdnGo.Value == "Go" || hdnGo.Value == "clear")
            {
                dvStatictime.Style.Add("display", "block");
                if (refreshTime != "")
                    lblRefresh.Text = "Last Refreshed On :" + refreshTime;
            }
        }
        catch (Exception ex)
        {
            if (ex.Message != "" && ex.Message.IndexOf(Constants.SESSIONEXPMSG) > -1)
            {
                string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
                Response.Redirect(url);
            }
            else if (ex.Message != "" && objIview == null && ex.Message == "Could not process the request. Please try again.")
            {
                Response.Redirect("err.aspx?errmsg=Could not process the request. Please try again.");
            }
            else if (ex.Message != "" && objIview == null)
            {
                Response.Redirect("err.aspx?errmsg=Needs logout and login again since in-memory is cleared.");
            }
        }

        if (objIview != null)
        {
            if (objIview.AxpIsAutoSplit != null)
                hdnAutoSplit.Value = objIview.AxpIsAutoSplit;
            if (objIview.AxpIviewDisableSplit != null)
                hdnDisableSplit.Value = objIview.AxpIviewDisableSplit;
        }
        //Import history modal dialog - for removing breadcrum panel 
        if (Request.QueryString["importhistory"] != null)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ImportHistory", "$('#breadcrumb-panel').remove()", true);
        requestProcess_logtime += ObjExecTr.RequestProcessTime("Response");
        serverprocesstime = ObjExecTr.TotalServerElapsTime();
        if (hdnGo.Value == "Go")
            hdnbElapsTimeGo.Value = serverprocesstime + "♠" + requestProcess_logtime;
    }

    public static string DecryptWithAesBase64(string base64Cipher, byte[] keyBytes)
    {
        try
        {
            var cipherBytes = Convert.FromBase64String(base64Cipher);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] iv = cipherBytes.Take(16).ToArray(); // First 16 bytes = IV
                byte[] actualCipher = cipherBytes.Skip(16).ToArray();

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                {
                    var decryptedBytes = decryptor.TransformFinalBlock(actualCipher, 0, actualCipher.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        catch (Exception)
        {
            throw new Exception("Could not process the request. Please try again.");
        }
    }


    public string ConvertXmlPublicKeyToPem(string xml)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(xml);
        var parameters = rsa.ExportParameters(false);

        var keyParam = new RsaKeyParameters(
            false, // isPrivate = false
            new BigInteger(1, parameters.Modulus),
            new BigInteger(1, parameters.Exponent)
        );

        using (var sw = new StringWriter())
        {
            var pemWriter = new PemWriter(sw);
            pemWriter.WriteObject(keyParam); // <-- This gives PKCS#8
            pemWriter.Writer.Flush();
            return sw.ToString(); // This can be safely embedded in JS
        }
    }

    private void GetAxpStructConfig()
    {
        FDW fdwObj = new FDW();
        bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
        string axpStructKeyIview = Constants.AXCONFIGIVIEW;
        string axpConfigTableIview = Constants.AXNODATACONFIGIVIEW;

        string structType = Constants.STRUCTTYPE_IVIEW;
        string configType = Constants.CONFIGTYPE_CONFIGS;

        if (Request.QueryString["tstcaption"] != null)
        {
            axpStructKeyIview = Constants.AXCONFIGTSTRUCT;
            axpConfigTableIview = Constants.AXNODATACONFIGTSTRUCT;

            structType = Constants.STRUCTTYPE_TSTRUCT;
        }

        DBContext objdb = new DBContext();
        DataSet dsConfig = new DataSet();
        DataTable axpConfigStrIview = new DataTable();
        bool isAxpConfig = true;
        string axpConfigTblIview = string.Empty;
        try
        {
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

            if (isRedisConnected)
            {

                if (Request.QueryString["tstcaption"] != null)
                    axpConfigStrIview = fObj.DataTableFromRedis(util.GetConfigCacheKey(axpStructKeyIview, iName, "", AxRole, "ALL"));
                else
                    axpConfigStrIview = fObj.DataTableFromRedis(util.GetConfigCacheKey(axpStructKeyIview, "", iName, AxRole, "ALL"));


                if (axpConfigStrIview == null || axpConfigStrIview.Rows.Count == 0)
                    axpConfigTblIview = fObj.StringFromRedis(util.GetNoDataConfigCacheKey(axpConfigTableIview, "", iName, AxRole, "ALL"));
                else
                    axpConfigStrIview.TableName = "Table";
            }
            if (!objIview.requestJSON && (axpConfigTblIview == string.Empty && (axpConfigStrIview == null || axpConfigStrIview.Rows.Count == 0)))
            {
                dsConfig = objdb.GetAxConfigurations(iName, structType, false, configType);

                if (dsConfig.Tables["Table0"] == null || dsConfig.Tables["Table0"].Rows.Count == 0)
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
                string lvFetchSize = string.Empty;
                if (Request.QueryString["tstcaption"] != null)
                {
                    if (Request.QueryString["lvFetchSize"] != null)
                    {
                        lvFetchSize = Request.QueryString["lvFetchSize"].ToString();
                    }
                    else
                    {
                        lvFetchSize = getLvFetchSize(iName, Request.QueryString["tstcaption"].ToString());
                    }
                }
                objIview.GetAxpStructConfig(objIview, lvFetchSize);
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("GetAxpStructConfig -" + ex.Message, sessID, "openiview-dev-" + iName, "");
        }

    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", true);
        }
    }

    private void ShowHideFilters()
    {
        if (hdnIsParaVisible.Value != string.Empty)
        {
            if (hdnIsParaVisible.Value == "0")
            {


                myFiltersBody.Style.Add("display", "block");



            }
            else
            {


                myFiltersBody.Style.Add("display", "block");
            }
        }

        else
        {




            myFiltersBody.Style.Add("display", "block");
        }
        if (objParams.IsFilterEnabled == false && (objParams.IsParameterExist == false || objParams.NoVisibleParam == true))
        {
            hdnIsParaVisible.Value = "hidden";
            Filterscollapse.Attributes.Add("class", Filterscollapse.Attributes["class"].ToString().Replace("in", ""));

            myFiltersBody.Style.Add("display", "none");
        }
        else
        {
            hdnIsParaVisible.Value = "visible";
        }
    }

    private void CheckParamaterDB()
    {

        {
            if (iName != "inmemdb")
            {
                GetParams();
            }
            else
            {
                tst_Scripts = tst_Scripts + "<script type=\"text/javascript\">var hideParams='true';var proj = '" + proj + "';var user='" + user + "';var AxRole='" + AxRole + "'; var sid='" + sid + "';var iName='" + iName + "'; gl_language='" + HttpContext.Current.Session["language"].ToString() + "'; validateParamOnGo='false'; " + strGlobalVar + "; var axpResp = '" + axpResp + "';</script>";
            }
        }
    }

    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }

    protected void CloseDimmerJS()
    {
        if ((GridView1.Rows.Count > 0) || (objIview.IViewWhenEmpty != string.Empty) || onlyParams == true)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "controlDimmer", "closeParentFrame();", true);
        }
    }


    #region DirectDBcall


    ///<summary>
    ///<para>Function to get iview structure from either redis or direct db(based on global configuration)</para>
    ///</summary>
    private void GetIviewStructure(string ivname)
    {
        string result = string.Empty;
        string structxml = string.Empty;
        bool isParamExist = false;
        string errorCond = string.Empty;

        try
        {
            #region New Code to cache Iview structure XML for DirectDB call 
            DataSet ds = new DataSet();
            string ivupdateOn = string.Empty;
            string fdKeyIVIEWSTRUCT = Constants.IVIEWSTRUCT;
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

            //Check iview structure xml in cache
            string[] fullRedisData = fObj.StringFromRedis(util.GetRedisServerkey(fdKeyIVIEWSTRUCT, iName)).Split(new[] { "*$*" }, StringSplitOptions.None);
            structxml = fullRedisData[0];

            if (structxml != string.Empty && structxml != "false" && objIview.StructureXml != "")
            {
                GenericRedisFunction(Title, objIview.IName, structxml, schemaName);

            }

            //If xml available, then check the flag in cache that whether iview is suitable for direct db call
            if (fullRedisData.Length == 3)
            {
                isParamExist = Convert.ToBoolean(fullRedisData[1]);
                errorCond = fullRedisData[2];
            }

            if (structxml != string.Empty && structxml != "false")
            {
                isCache = true;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(structxml);
                if (xmlDoc.DocumentElement.Attributes["updatedon"] != null)
                {
                    ivupdateOn = xmlDoc.DocumentElement.Attributes["updatedon"].Value;
                }
            }

            bool isStructureUpdated = ivupdateOn != string.Empty && objIview.IsStructureUpdated(ivupdateOn, ivname);

            if (isStructureUpdated)
            {
                if (structxml != string.Empty)
                {
                    structxml = string.Empty;
                }
            }

            #endregion

            if (structxml != string.Empty && structxml != "false")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(structxml);
                XmlNode cNode = default(XmlNode);
                cNode = xmldoc.SelectSingleNode("//iview/b38");
                if (cNode != null)
                {
                    if (cNode.InnerText != null)
                    {
                        objIview.IVType = cNode.InnerText;
                        dvData.Attributes["class"] = "ivType-" + objIview.IVType;
                    }
                    else
                    {
                        objIview.IVType = "Classic";

                    }
                }
                else
                {
                    objIview.IVType = "Classic";
                }

                if (xmldoc.SelectSingleNode("//iview/b39") != null)
                    paramsCache = xmldoc.SelectSingleNode("//iview/b39").InnerText;


            }
            //If directiview is false in cache then
            // If “false” is returned then
            if ((structxml == string.Empty || structxml == "false"))
            {
                callParamPlusStructure = true;
            }
            objIview.StructureXml = structxml;
            parseIvSxmlEncryptColumns(objIview.StructureXml, objIview);
            objParams.IsParameterExist = isParamExist;

            strTimetaken.Append("ISStructCached--:" + isCache.ToString() + "--");
            if (isCache)
            {
                logobj.CreateDirectDBLog("openiview-dev-" + ivname, "RedisCachedStructure - GetIviewStructure", "", "IView Name :  " + ivname, "Success --" + ivname
                                                     + Environment.NewLine + "ISCached : " + isCache.ToString() + " - StructureXML : "
                                                     + Environment.NewLine + Environment.NewLine + objIview.StructureXml);
            }
        }
        catch (Exception ex)
        {
            string errMsg = ex.Message.Replace("\n", "");
            requestProcess_logtime += "Server - " + errMsg + " ♦ ";
            Response.Redirect(util.ERRPATH + errMsg + "*♠*" + requestProcess_logtime);
            logobj.CreateDirectDBLog("openiview-dev- " + ivname, "GetIviewStructure", errMsg, "IView Name :  " + ivname, "Fail - " + ivname + " - StructureXML");

        }
        if (structxml != string.Empty && structxml != "false" && objIview.StructureXml != "")
        {

            GenericRedisFunction(Title, objIview.IName, structxml, schemaName);

        }
        fileName = "openiview-" + iName;
        errLog = logobj.CreateLog("Loading iview.aspx before post back (get and set parameters)", sid, fileName, "new");
        XmlDocument doc = new XmlDocument();

        callParamAndData(ivname);
    }

    private void callParamAndData(string ivname)
    {
        additionalPageInfo();

        if ((Request.Form.Count > 0 && Request.Form["redisLoadKey"] == null && Request.Form.Keys[0] != "reqProc_logtime") || loadString != string.Empty)
        {
            isCallWS = true;
            logobj.CreateLog("Loading popup iviews", sid, fileName, string.Empty);
            logobj.CreateLog("Get Iview Parameters", sid, fileName, string.Empty);
            SetParamValues();
            //Process parameters
            CheckParamaterDB();
            if (Session["project"] == null)
                return;
            logobj.CreateLog("Get and Load Iviewdata", sid, fileName, string.Empty);
            if (Request.Form["AxOpenAct"] != null && Request.Form["AxOpenAct"].ToString() == "true")
            {
                if (CheckQueryStringValParamBound())
                    GetIviewData();
            }
            else
            {
                GetIviewData();
            }
            GridView2Wrapper.Visible = true;
        }
        else if (Request.QueryString["tstcaption"] != null)
        {
            if (!objIview.requestJSON)
            {
                objIview.purposeString = " purpose=\"list\" ";
                objIview.IviewCaption = Request.QueryString["tstcaption"].ToString();
                tst_Scripts += "<script language=javascript> var proj = '" + proj + "';var user='" + user + "';var sid='" + sid + "';var iName='" + iName + "'; var ivna='" + iName + "';var tst ='" + iName + "'; var ivtype='listview';var trace = '" + Session["AxTrace"] + "';var AxRole='" + AxRole + "';var gl_language = '" + language + "';</script>";
                GetIviewData();
            }
            else
            {
                objIview.purposeString = " purpose=\"list\" ";
                objIview.IviewCaption = Request.QueryString["tstcaption"].ToString();

                isCallWS = false;
                logobj.CreateLog("Get ListView Parameters", sid, fileName, string.Empty);
                DateTime asStart1 = DateTime.Now;
                //Process parameters
                CheckParamaterDB();
                onlyParams = true;
                webTime2 = DateTime.Now.Subtract(asStart1).TotalMilliseconds;
                strTimetaken.Append("GetParams-" + webTime2.ToString() + " ");
                if (Session["project"] == null)
                    return;
            }

        }
        else if (iName == "inmemdb")
        {
            axpResp = "true";
            objIview.IviewCaption = "In-Memory DB";
            GetIviewData();
            CheckParamaterDB();
        }
        else
        {
            isCallWS = false;
            logobj.CreateLog("Get Iview Parameters", sid, fileName, string.Empty);
            DateTime asStart1 = DateTime.Now;
            //Process parameters
            CheckParamaterDB();
            onlyParams = true;
            webTime2 = DateTime.Now.Subtract(asStart1).TotalMilliseconds;
            strTimetaken.Append("GetParams-" + webTime2.ToString() + " ");
            if (Session["project"] == null)
                return;
        }
    }

    private bool CheckQueryStringValParamBound()
    {
        bool _isParamBound = true;
        try
        {
            int _hideParamCount = 0;
            bool isParamExist = false;
            for (int j = 0; j < iviewParams.Count; j++)
            {
                if (arrVisibleParamList[j].ToString() == "false")
                {
                    isParamExist = false;
                    for (int i = 0; i < Request.Form.Keys.Count; i++)
                    {
                        if (iviewParams[j].ToString().ToLower() == Request.Form.Keys[i].ToString().ToLower())
                        {
                            isParamExist = true;
                            break;
                        }
                    }
                    if (!isParamExist && iviewParamValues[j].ToString() != "")
                        isParamExist = true;

                    if (!isParamExist)
                        break;
                }
                else if (arrVisibleParamList[j].ToString() == "true")
                {
                    _hideParamCount++;
                }
            }
            if (!isParamExist && iviewParams.Count == _hideParamCount)
                _isParamBound = true;
            else
                _isParamBound = isParamExist;
        }
        catch (Exception ex)
        {
            _isParamBound = true;
        }
        return _isParamBound;
    }
    private string RemoveQueryStringValFromParamXML(string _ipXML)
    {
        string ixmlParamXML = _ipXML;
        if (Request.Form.Count > 0)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(ixmlParamXML);
                for (int i = 0; i < Request.Form.Keys.Count; i++)
                {
                    if (Request.Form.Keys[i].ToString() != "")
                    {
                        string _thidParamName = Request.Form.Keys[i].ToString();
                        try
                        {
                            XmlNode _paramNode = xmlDoc.SelectSingleNode("//root/" + _thidParamName);
                            if (_paramNode != null)
                            {
                                _paramNode.Attributes["value"].Value = "";
                            }
                        }
                        catch (XmlException ex) { }
                    }
                }
                ixmlParamXML = xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                ixmlParamXML = _ipXML;
            }
        }
        return ixmlParamXML;
    }

    private void additionalPageInfo()
    {
        gridPageSize = objIview.GrdPageSize;
        if (gridPageSize == "") gridPageSize = "30";
        GridView1.PageSize = Convert.ToInt32(gridPageSize);

        objIview.GrdPageSize = gridPageSize;
        currentPageNo = 1;
        Session["currentPageNo" + iName] = "1";
    }

    private void GetIviewStructureNew(string ivname)
    {
        fileName = "openiview-" + iName;
        errLog = logobj.CreateLog("Loading iview.aspx before post back (get and set parameters)", sid, fileName, "");
        callParamAndData(ivname);
    }




    ///<summary>
    ///<para>Function parameters and global parameters for input query</para>
    ///</summary>
    private string ReplaceSqlParamByvalues(string sqlQuery, bool isSQL = true)
    {
        //parar values array list was used by referencde hence passing the new araylist by value.. Issue related to appending "to_date(" 
        sqlQuery = ReplaceParamvalues(sqlQuery, new ArrayList(objParams.ParamNames), new ArrayList(objParams.ParamChangedVals), "param", isSQL);
        if (objIview.GolbalVarName != null && objIview.GolbalVarName.Count > 0)
            sqlQuery = ReplaceParamvalues(sqlQuery, objIview.GolbalVarName, objIview.GolbalVarValue, "global", isSQL);

        return sqlQuery;
    }


    ///<summary>
    ///<para>Function replace param with param values for input query</para>
    ///</summary>
    private string ReplaceParamvalues(string sqlQuery, ArrayList paramNames, ArrayList paramValues, string type, bool isSQL = true)
    {
        try
        {
            Util.Util utilGlo = new Util.Util();
            string dbType = string.Empty;
            string dlmtr = string.Empty;

            if (!string.IsNullOrEmpty(HttpContext.Current.Session["axdb"].ToString()))
                dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
            for (int i = 0; i < paramNames.Count; i++)
            {
                if (paramNames[i].ToString().ToUpper() != "AXISPOP")
                {
                    if (paramValues[i] != "")
                        paramValues[i] = utilGlo.ReverseCheckSpecialCharsInQuery(paramValues[i].ToString());
                    if ((type == "param") && (dbType.ToLower() == "oracle"))
                    {

                        if (GetParamType(paramNames[i].ToString()) == "Date/Time")
                        {
                            dlmtr = string.Empty;
                            string dateParam = paramValues[i].ToString();

                            if (clientCulture.ToLower() == "en-us")
                            {
                                dateParam = util.GetClientDateString(clientCulture, dateParam);
                                if (isSQL)
                                    paramValues[i] = "to_date('" + dateParam + "','mm/dd/yyyy hh24:mi:ss')";
                                else
                                    paramValues[i] = dateParam;
                            }
                            else
                            {
                                if (isSQL)
                                    paramValues[i] = "to_date('" + dateParam + "','dd/mm/yyyy hh24:mi:ss')";
                                else
                                    paramValues[i] = dateParam;
                            }
                        }
                        else
                        {
                            dlmtr = "'";
                        }
                    }
                    else
                    {
                        dlmtr = "'";
                    }

                    string paramName = ":" + paramNames[i].ToString();
                    string paramnSpace = ":" + paramNames[i] + " ";
                    string paramBrace = ":" + paramNames[i] + ")";
                    string paramColn = ":" + paramNames[i] + ";";
                    string paramBrash = "{" + paramNames[i] + "}";

                    bool isNumericEmptyParam = false;
                    if (type == "param" && GetParamType(paramNames[i].ToString()) == "Numeric")
                    {
                        dlmtr = String.Empty;
                        if (paramValues[i] == "")
                        {
                            isNumericEmptyParam = true;
                            paramValues[i] = 0;
                        }
                    }

                    if (sqlQuery.Contains(paramBrash))
                        sqlQuery = sqlQuery.Replace(paramBrash, paramValues[i].ToString());

                    if (sqlQuery.Contains(paramnSpace))
                        sqlQuery = sqlQuery.Replace(paramName, dlmtr + paramValues[i].ToString() + dlmtr);

                    else if (sqlQuery.Contains(paramnSpace.ToLower()))
                        sqlQuery = sqlQuery.Replace(paramName.ToLower(), dlmtr + paramValues[i].ToString() + dlmtr);

                    else if (sqlQuery.Contains(paramnSpace.ToUpper()))
                        sqlQuery = sqlQuery.Replace(paramName.ToUpper(), dlmtr + paramValues[i].ToString() + dlmtr);

                    if (sqlQuery.Contains(paramBrace))
                        sqlQuery = sqlQuery.Replace(paramName, dlmtr + paramValues[i].ToString() + dlmtr);

                    else if (sqlQuery.Contains(paramBrace.ToLower()))
                        sqlQuery = sqlQuery.Replace(paramName.ToLower(), dlmtr + paramValues[i].ToString() + dlmtr);

                    else if (sqlQuery.Contains(paramBrace.ToUpper()))
                        sqlQuery = sqlQuery.Replace(paramName.ToUpper(), dlmtr + paramValues[i].ToString() + dlmtr);


                    else if (sqlQuery.Contains(paramColn))
                        sqlQuery = sqlQuery.Replace(paramName, dlmtr + paramValues[i].ToString() + dlmtr);

                    else if (sqlQuery.Contains(paramColn.ToLower()))
                        sqlQuery = sqlQuery.Replace(paramName.ToLower(), dlmtr + paramValues[i].ToString() + dlmtr);

                    else if (sqlQuery.Contains(paramColn.ToUpper()))
                        sqlQuery = sqlQuery.Replace(paramName.ToUpper(), dlmtr + paramValues[i].ToString() + dlmtr);


                    if (sqlQuery.Contains(paramName))
                    {
                        sqlQuery = sqlQuery.Replace(paramName, dlmtr + paramValues[i].ToString() + dlmtr);
                    }
                    else if (sqlQuery.Contains(paramName.ToLower()))
                    {
                        sqlQuery = sqlQuery.Replace(paramName.ToLower(), dlmtr + paramValues[i].ToString() + dlmtr);
                    }
                    else if (sqlQuery.Contains(paramName.ToUpper()))
                    {
                        sqlQuery = sqlQuery.Replace(paramName.ToUpper(), dlmtr + paramValues[i].ToString() + dlmtr);
                    }

                    if (isNumericEmptyParam)
                    {
                        paramValues[i] = "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return sqlQuery;
    }




    #endregion DirectDBcall


    public void setPageDirection()
    {

        if (language.ToLower() == "arabic")
        {
            direction = "rtl";
            btn_direction = "start";
            bread_direction = "end";
        }

        if (language.ToLower() == "arabic")
        {
            dvRowsPerPage.Style.Add("float", "left");
            dvRowsPerPage.Style.Add("margin-top", "4px");
            divcontainer.Style.Add("margin-top", "0%");
            leftPanel.Style.Add("margin-top", "1%");

        }
        else
        {
            dvRowsPerPage.Style.Add("float", "right");
            dvRowsPerPage.Style.Add("margin-top", "4px");


        }
    }

    public void CleardtFilterCond()
    {
        dtFilterConds.Rows.Clear();
        dtFilterConds.Columns.Clear();
        dtFilterConds.Columns.Add("FilterColumn", typeof(string));
        dtFilterConds.Columns.Add("Value", typeof(string));
        Session["dtFilterConds"] = dtFilterConds;

    }

    private void SetParamValues()
    {
        if (Request.Form.Count > 0 && Request.Form["redisLoadKey"] == null)
        {
            string ixml1 = string.Empty;

            for (int i = 0; i < Request.Form.Keys.Count; i++)
            {
                if (Request.Form.Keys[i].ToString() != "pop" && Request.Form.Keys[i].ToString() != "AxHypTstRefresh" && Request.Form.Keys[i].ToString() != "reqProc_logtime")
                {
                    string val = string.Empty;
                    val = Request.Form[i].ToString();
                    val = util.CheckReverseUrlSpecialChars(val);
                    //changed to ReverseCheckSpecialChars as it was failing in case of &quot; double quotes
                    val = util.ReverseCheckSpecialChars(val);
                    ixml1 += Request.Form.Keys[i].ToString() + "~" + val + "¿";
                }
            }
            hdnparamValues.Value = ixml1;
            Session["paramValues" + iName] = ixml1;
        }
    }

    private void ClearNavigationData()
    {
        Session["iNavigationInfoTable"] = null;
    }

    /// Call to GetParams Web Service
    public void GetParams()
    {
        string ires = string.Empty;
        string istructure = string.Empty;
        string fdKeyIVIEWPARAM = Constants.IVIEWPARAM;
        string fdKeyIVIEWSTRUCT = Constants.IVIEWSTRUCT;
        bool cacheParamsXML = false;
        string ivupdateOn = string.Empty;
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        //check if IView Structure exist in Redis and is Latest

        if (!objIview.isObjFromCache || hdnGo.Value == "updateCache")
        {
            if (callParamPlusStructure && !objIview.requestJSON)
            {
                if (!isCache)
                {
                    string[] fullRedisData = fObj.StringFromRedis(util.GetRedisServerkey(fdKeyIVIEWSTRUCT, iName)).Split(new[] { "*$*" }, StringSplitOptions.None);

                    istructure = fullRedisData[0];

                    if (istructure != string.Empty && istructure != "false")
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(istructure);
                        if (xmlDoc.DocumentElement.Attributes["updatedon"] != null)
                        {
                            ivupdateOn = xmlDoc.DocumentElement.Attributes["updatedon"].Value;
                        }


                        if (ivupdateOn != string.Empty && !objIview.IsStructureUpdated(ivupdateOn, iName))
                        {
                            objIview.StructureXml = istructure;
                            parseIvSxmlEncryptColumns(objIview.StructureXml, objIview);
                            callParamPlusStructure = false;
                            isCache = true;
                        }
                    }
                }
                else
                {
                    isCache = false;
                }
            }


            string iXml = string.Empty;
            DateTime asStart = DateTime.Now;
            errLog = logobj.CreateLog("Call to GetParams Web Service", sid, fileName, string.Empty);
            iXml = "<root " + objIview.purposeString + " name =\"" + iName + "\" axpapp = \"" + proj + "\" sessionid = \"" + sid + "\" appsessionkey=\"" + HttpContext.Current.Session["AppSessionKey"].ToString() + "\" username=\"" + HttpContext.Current.Session["username"].ToString() + "\" trace = \"" + errLog + "\" firsttime=\"" + (!objIview.requestJSON ? callParamPlusStructure.ToString().ToLower() : "true") + "\"  >";
            //GetParams input xml will contain the query string values passed by previous iview
            ConstructParamXml();
            iXml += "<params>" + pXml + "</params>";
            iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

            if (hdnGo.Value != "refreshparams" && hdnGo.Value != "updateCache" && (isCache || objIview.requestJSON))
            {
                //Call service     
                if (!objIview.requestJSON)
                {
                    if (Session["IsFromChildWindow"] != null && objParams.ParamXML != string.Empty)
                    {
                        ires = objParams.ParamXML;
                        Session["IsFromChildWindow"] = null;
                        ires = ReturnModified(ires);
                        Session["UpdateParamsCollection"] = null;
                    }
                    else
                    {
                        try
                        {
                            {
                                if (HttpContext.Current.Session[iName + "_" + sid] != null)
                                {
                                    ires = HttpContext.Current.Session[iName + "_" + sid].ToString();
                                }

                                if (ires == String.Empty)
                                {
                                    ires = fObj.StringFromRedis(util.GetRedisServerkey(fdKeyIVIEWPARAM, iName));
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            if (!objIview.requestJSON)
            {
                if (ires == string.Empty)
                {
                    //Call “GetParams” web service. -> (Structure xml also will be retuned during first call -> new change)
                    ires = objWebServiceExt.CallGetParamsWS(iName, iXml, objIview);
                    requestProcess_logtime += ires.Split('♠')[0];
                    ires = ires.Split('♠')[1];

                    if (parseError(ires))
                    {
                        isException = true;
                        throw (new Exception(requestProcess_logtime + "♠" + ires));
                    }

                    if (ires != string.Empty && callParamPlusStructure)
                    {
                        if (ires.StartsWith("<"))
                            ires = util.ReplaceFirstOccurrence(ires, "#$#", "#$♥#");
                        string[] splitRes = ires.Split(new[] { "#$♥#" }, StringSplitOptions.None);
                        if (splitRes.Length == 2)
                        {
                            objParams.ParamXML = ires = splitRes[0];
                            istructure = splitRes[1];
                            if (istructure != string.Empty)
                            {
                                FDW fdwObj = new FDW();
                                //  Keep XML in cache.
                                fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKeyIVIEWSTRUCT, iName), istructure, fdKeyIVIEWSTRUCT, schemaName);
                                callParamPlusStructure = false;
                                objIview.StructureXml = istructure;
                                parseIvSxmlEncryptColumns(objIview.StructureXml, objIview);
                                isCache = false;
                                logobj.CreateDirectDBLog("openiview-dev-" + iName, "CallGetParams Plus Structure(firsttime=true) - GetParams", "", "IView Name :  " + iName, "Success --" + iName
                                                         + Environment.NewLine + "ISCached : " + isCache.ToString() + Environment.NewLine + " StructureXML : "
                                                         + Environment.NewLine + Environment.NewLine + objIview.StructureXml + Environment.NewLine + Environment.NewLine + " ParamXML :" + ires);
                            }
                        }
                    }
                    cacheParamsXML = true;
                }


                if (!cacheParamsXML)
                {
                    logobj.CreateDirectDBLog("openiview-dev-" + iName, "RedisCachedParams - GetParams", "", "IView Name :  " + iName, "Success --" + iName
                                                             + Environment.NewLine + "ISCached : " + (!cacheParamsXML).ToString() + " - ParamXML : "
                                                             + Environment.NewLine + Environment.NewLine + ires);
                }

                if (isCache)
                {
                    logobj.CreateDirectDBLog("openiview-dev-" + iName, "RedisCachedStructure - GetParams", "", "IView Name :  " + iName, "Success --" + iName
                                                             + Environment.NewLine + "ISCached : " + isCache.ToString() + " - StructureXML : "
                                                             + Environment.NewLine + Environment.NewLine + objIview.StructureXml);
                }

            }
            else
            {
                ires = objWebServiceExt.CallGetParamsWS(iName, iXml, objIview);
                requestProcess_logtime += ires.Split('♠')[0];
                ires = ires.Split('♠')[1];

                //ires = Constants.ERROR + Constants.SESSIONEXPMSG + Constants.ERRORCLOSE;

                if (parseError(ires))
                {
                    isException = true;
                    throw (new Exception(requestProcess_logtime + "♠" + ires));
                }

                if (ires != string.Empty)
                {
                    if (ires.StartsWith("<"))
                        ires = util.ReplaceFirstOccurrence(ires, "#$#", "#$♥#");
                    string[] splitRes = ires.Split(new[] { "#$♥#" }, StringSplitOptions.None);
                    if (splitRes.Length > 0)
                    {
                        objParams.ParamXML = ires = splitRes[0];
                        if (Request.Form["AxOpenAct"] != null && Request.Form["AxOpenAct"].ToString() == "true")
                        {
                            ires = objParams.ParamXML = RemoveQueryStringValFromParamXML(objParams.ParamXML);
                        }
                    }
                    if (splitRes.Length > 1)
                    {
                        objIview.AccessControlXml = splitRes[1];
                    }
                    if (splitRes.Length > 2)
                    {
                        objIview.StructureXml = istructure = splitRes[2];
                        parseIvSxmlEncryptColumns(objIview.StructureXml, objIview);
                    }
                    if (splitRes.Length > 3)
                    {
                        objIview.DsIviewConfig = processXmlConfigAsDt(splitRes[3]);
                    }
                    if (splitRes.Length > 4)
                    {
                        //objParams.GlobalVars = splitRes[4];
                        objIview.GlobalVars = splitRes[4];
                    }

                    if (splitRes.Length > 5)
                    {
                        try
                        {
                            string smartviewJSON = splitRes[5].Trim();

                            FDW fdwObj = new FDW();

                            {//smartview settings section
                                string fdSettingsKey = Constants.RedisIviewSettings;
                                if (Request.QueryString["tstcaption"] != null)
                                {
                                    fdSettingsKey = Constants.RedisListviewSettings;
                                }

                                if (smartviewJSON != string.Empty)
                                {
                                    JObject smartViewSettings = JObject.Parse(smartviewJSON);

                                    JObject forUserSettings = new JObject();
                                    try
                                    {
                                        foreach (var setting in smartViewSettings)
                                        {
                                            string profile = setting.Key;
                                            JToken settingValue = setting.Value;
                                            if (profile != string.Empty)
                                            {
                                                if (profile == user || profile == "all")
                                                {
                                                    forUserSettings[profile] = settingValue;
                                                }

                                                fdwObj.SaveInRedisServer(fObj.MakeKeyName(fdSettingsKey, iName, profile), settingValue.ToString(), fdSettingsKey, schemaName);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    { }
                                    objIview.smartviewSettings = forUserSettings.ToString();
                                }
                            }

                        }
                        catch (Exception ex)
                        { }
                    }

                    logobj.CreateDirectDBLog("openiview-dev-" + iName, "Call Params Plus Access Structure Config - GetParams", "",
                        "IView Name :  " + iName, "Success --" + iName + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                        (splitRes.Length > 0 ? " ParamXML : " + splitRes[0] + Environment.NewLine + Environment.NewLine : "") +
                        (splitRes.Length > 1 ? " AccessControl : " + splitRes[1] + Environment.NewLine + Environment.NewLine : "") +
                        (splitRes.Length > 2 ? " StructureXML : " + splitRes[2] + Environment.NewLine + Environment.NewLine : "") +
                        (splitRes.Length > 3 ? " Configuration : " + splitRes[3] + Environment.NewLine + Environment.NewLine : "") +
                        (splitRes.Length > 4 ? " GlobalVars : " + splitRes[4] + Environment.NewLine + Environment.NewLine : ""));
                }
            }


            DateTime asEnd = DateTime.Now;
            double webTime1 = asStart.Subtract(asStart).TotalMilliseconds;
            double webTime2 = DateTime.Now.Subtract(asEnd).TotalMilliseconds;
            double asbTime = asEnd.Subtract(asStart).TotalMilliseconds;
            AsbTime += "GetParams-" + asbTime.ToString();
            string errMsg = string.Empty;
            errMsg = util.ParseXmlErrorNode(ires);
            if (errMsg != string.Empty)
            {
                if (errMsg == Constants.SESSIONERROR)
                {
                    Session.RemoveAll();
                    Session.Abandon();
                    SessExpires();
                    return;
                }
                else if (errMsg == Constants.SESSIONEXPMSG)
                {
                    SessExpires();
                    return;
                }
                else if (errMsg == Constants.ERAUTHENTICATION)
                {
                    requestProcess_logtime += "Server - " + errMsg + " ♦ ";
                    Response.Redirect(util.ERRPATH + Constants.ERAUTHENTICATION + "*♠*" + requestProcess_logtime);
                }
                else
                {
                    requestProcess_logtime += "Server - " + errMsg + " ♦ ";
                    Response.Redirect(util.ERRPATH + errMsg + "*♠*" + requestProcess_logtime);
                }
            }
            else if (!objIview.requestJSON)
            {
                callParamPlusStructure = false;
                if (ires != "" && cacheParamsXML)
                {
                    try
                    {
                        if (paramsCache == "Session")
                        {
                            HttpContext.Current.Session[iName + "_" + sid] = ires;
                        }
                        else if (paramsCache == "InMemory")
                        {
                            FDW fdwObj = new FDW();
                            fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKeyIVIEWPARAM, iName), ires, fdKeyIVIEWPARAM, schemaName);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
            }
            //}
            setDataCachingFlag();
        }
        else
        {
            ires = objParams.ParamXML;
        }

        if (objIview.RetainIviewParams || (Session["IsFromChildWindow"] != null && Session["IsFromChildWindow"].ToString() == "true"))
        {
            loadString = webService.GetIviewNavigationData(iName);
        }

        ConstructParamsHtml(ires);
    }

    private void parseIvSxmlEncryptColumns(string _sXML, IviewData objIview)
    {
        try
        {
            if (objIview.EncryptedColumns.Count == 0)
            {
                XElement root = XElement.Parse(_sXML);
                var encryptedNodes = root
                    .Elements()
                    .Where(e => (string)e.Attribute("encrypted") == "T");

                foreach (var node in encryptedNodes)
                {
                    var a2Value = node.Element("a2").Value;
                    objIview.EncryptedColumns.Add(a2Value);
                }
            }
        }
        catch (Exception ex) { }
    }

    private void setDataCachingFlag()
    {
        try
        {
            string ivupdateOn = string.Empty;

            if (objIview.StructureXml != string.Empty && objIview.StructureXml != "false")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(objIview.StructureXml);

                if (xmldoc.DocumentElement.Attributes["updatedon"] != null)
                {
                    ivupdateOn = xmldoc.DocumentElement.Attributes["updatedon"].Value;
                }

                System.Globalization.CultureInfo culture = default(System.Globalization.CultureInfo);
                culture = new System.Globalization.CultureInfo("en-GB");
                if (HttpContext.Current.Session["axdb"] != null)
                {
                    if (HttpContext.Current.Session["axdb"].ToString().ToLower() == "ms sql")
                        culture = new System.Globalization.CultureInfo("en-US");
                }

                if (object.ReferenceEquals(ivupdateOn, ""))
                    ivupdateOn = "01/01/2000 00:00:01 AM";
                DateTime cacheTime;
                try
                {
                    cacheTime = DateTime.Parse(ivupdateOn, culture);
                }
                catch (System.FormatException ex)
                {
                    cacheTime = DateTime.Parse(ivupdateOn);
                }

                DateTime boundryDate;
                if (!DateTime.TryParseExact("11/18/2022", "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out boundryDate))
                {
                    //Invalid date
                }


                if (boundryDate != null && boundryDate.Date <= cacheTime.Date)
                {
                    if (xmldoc.SelectSingleNode("//iview/b39") != null)
                    {
                        string dataCache = xmldoc.SelectSingleNode("//iview/b39").InnerText;
                        objIview.dataCache = dataCache;
                    }
                }
                else
                {
                    objIview.dataCache = "none";
                }
            }
        }
        catch (Exception ex)
        {
            objIview.dataCache = "none";
            logobj.CreateLog("Exception in setDataCachingFlag: " + ex.Message, sid, "IvsetDataCachingFlag-" + iName, string.Empty);
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
                logobj.CreateDirectDBLog("openiview-dev-" + iName, "ConfigurationXML", "", "IView Name :  " + iName, "ConfigurationXML : " + Environment.NewLine + Environment.NewLine + configurationXml);

                configurationXml = configurationXml.Replace("<axconfig>", "<dataSet>").Replace("</axconfig>", "</dataSet>").Replace("<row>", "<table0>").Replace("</row>", "</table0>");

                bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
                string axpStructKeyIview = Constants.AXCONFIGIVIEW;
                string axpConfigTableIview = Constants.AXNODATACONFIGIVIEW;

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
                    string lvFetchSize = string.Empty;
                    if (Request.QueryString["tstcaption"] != null)
                    {
                        if (Request.QueryString["lvFetchSize"] != null)
                        {
                            lvFetchSize = Request.QueryString["lvFetchSize"].ToString();
                        }
                        else
                        {
                            lvFetchSize = getLvFetchSize(iName, Request.QueryString["tstcaption"].ToString());
                        }
                    }
                    objIview.GetAxpStructConfig(objIview, lvFetchSize);
                }
            }
        }
        catch (Exception ex) { }

        return returnDt;
    }

    public string getLvFetchSize(string iName, string tstCaption)
    {
        string lvFetchSizeVal = string.Empty;
        try
        {
            FDW fdwObj = new FDW();
            bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
            if (isRedisConnected)
            {
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                {
                    fObj = new FDR();
                    HttpContext.Current.Session["FDR"] = fObj;
                }

                DataTable lvConfigs = new DataTable();

                lvConfigs = fObj.DataTableFromRedis(util.GetConfigCacheKey(Constants.AXCONFIGTSTRUCT, iName, "", HttpContext.Current.Session["AxRole"].ToString(), "ALL"));

                if (lvConfigs == null || lvConfigs.Rows.Count == 0)
                {
                    CacheManager cacheMgr = util.GetCacheObject();
                    TStructDef strObj = null;
                    strObj = util.GetStrObject(cacheMgr, iName);
                    lvConfigs = util.GetStructConfig(iName);

                    string fdKey = Constants.REDISTSTRUCT;
                    if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                        fdKey = Constants.REDISTSTRUCTMOB;
                    string pgKey = Constants.AXPAGETITLE;
                    ArrayList redisvalues = new ArrayList();
                    cacheMgr.fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKey, iName), strObj, Constants.REDISTSTRUCT, schemaName);

                    var redisvalues1 = fObj.ObjectJsonFromRedis(util.GetRedisServerkey(pgKey, ""));
                    if (redisvalues1 == null)
                        redisvalues.Add(Title + "♠" + tstCaption + "♠" + iName);
                    else
                    {
                        redisvalues = (ArrayList)redisvalues1;
                        string newValue = Title + "♠" + tstCaption + "♠" + iName;
                        if (!redisvalues.Contains(newValue))
                            redisvalues.Add(newValue);

                    }
                    cacheMgr.fdwObj.SaveInRedisServer(util.GetRedisServerkey(pgKey, iName), redisvalues, Constants.AXPAGETITLE, schemaName);
                }

                lvFetchSizeVal = lvConfigs.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "fetchsize").Select(x => x.Field<string>("PROPSVAL")).FirstOrDefault();

                if (lvFetchSizeVal == null)
                {
                    lvFetchSizeVal = "";
                }

            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("getListviewFetchSize -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in getLvFetchSize", "new");

        }
        return lvFetchSizeVal;
    }

    private string GetParamsCacheType(string ires)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ires);
            XmlNode rootNode = xmlDoc.SelectSingleNode("//root");

            if (rootNode != null && rootNode.Attributes["cachetype"] != null)
            {
                paramsCache = rootNode.Attributes["cachetype"].Value.ToString();
            }
            return paramsCache;
        }
        catch (Exception Ex) { }

        return String.Empty;
    }

    private string ReturnModified(string ires)
    {
        XmlDocument xmlDoc = new XmlDocument();
        Dictionary<string, string> DepFields;
        XmlNode responseNode;
        try
        {

            DepFields = (Dictionary<string, string>)Session["UpdateParamsCollection"];
            if (DepFields == null)
                return ires;

            xmlDoc.LoadXml(ires);
            foreach (var depField in DepFields)
            {

                responseNode = xmlDoc.SelectSingleNode("//" + depField.Key.ToString() + "/response");
                if (responseNode != null)
                    responseNode.InnerXml = depField.Value;
                else
                {
                    responseNode = xmlDoc.SelectSingleNode("//" + depField.Key.ToString());
                    XmlElement elem = xmlDoc.CreateElement("response");
                    elem.InnerXml = depField.Value;
                    responseNode.AppendChild(elem);
                }

            }
        }
        catch (Exception exc)
        {
            return ires;
        }
        return xmlDoc.OuterXml.ToString();
    }

    ///Function to get the iview data by calling the service.
    public void GetIviewData()
    {
        iviewFrame.Style.Add("display", "block");
        if (!IsPostBack)
        {
            loadString = util.AdditionalSpecialCharsCheck(loadString);
            string[] loadArr = loadString.Split('&');
            bool loadArrayExist = loadArr.Length > 0;
            if ((Request.Form.Count > 0 && Request.Form["redisLoadKey"] == null))
            {
                string ixml1 = string.Empty;
                int j = 0;
                for (j = 0; j < iviewParams.Count; j++)
                {
                    bool isParamExist = false;
                    for (int i = 0; i < Request.Form.Keys.Count; i++)
                    {
                        if (iviewParams[j].ToString().ToLower() == Request.Form.Keys[i].ToString().ToLower())
                        {
                            if (Request.Form.Keys[i].ToString() != "pop" && Request.Form.Keys[i].ToString() != "AxHypTstRefresh" && Request.Form.Keys[i].ToString() != "reqProc_logtime")
                            {
                                string val = string.Empty;
                                val = Request.Form[i].ToString();
                                val = util.CheckReverseUrlSpecialChars(val);
                                val = util.ReverseCheckSpecialChars(val);
                                ixml1 += Request.Form.Keys[i].ToString() + "~" + val + "¿";
                            }
                            isParamExist = true;
                            break;
                        }
                    }
                    if (isParamExist)
                        continue;

                    string pVal = string.Empty;
                    pVal = iviewParamValues[j].ToString();
                    pVal = pVal.Replace("quot;", "'");
                    if (iviewParams[j].ToString() != "axp_refresh")
                        ixml1 += iviewParams[j].ToString() + "~" + pVal + "¿";

                }
                hdnparamValues.Value = ixml1;
                Session["paramValues" + iName] = ixml1;

            }
            else if (loadArrayExist)
            {
                string ixml1 = string.Empty;
                int j = -1;
                foreach (string par in iviewParams)
                {
                    bool isParamExist = false;

                    j++;

                    foreach (string loadStr in loadArr)
                    {
                        string[] keyValArr = loadStr.Split('=');
                        try
                        {
                            string key = keyValArr[0];
                            string value = keyValArr[1].Replace("--.--", "&").Replace("@eq@", "=").Replace("~", "&grave;");

                            if (par.ToLower() == key.ToLower())
                            {
                                if (loadStr != "pop" && key != "AxHypTstRefresh" && key != "reqProc_logtime")
                                {
                                    string val = string.Empty;
                                    val = value;
                                    val = util.CheckReverseUrlSpecialChars(val);
                                    val = util.ReverseCheckSpecialChars(val);
                                    ixml1 += key.ToString() + "~" + val + "¿";
                                }
                                isParamExist = true;
                                break;
                            }
                        }
                        catch (Exception ex) { }
                    }
                    if (isParamExist)
                        continue;

                    string pVal = string.Empty;
                    pVal = iviewParamValues[j].ToString();
                    pVal = pVal.Replace("quot;", "'");
                    if (iviewParams[j].ToString() != "axp_refresh")
                        ixml1 += iviewParams[j].ToString() + "~" + pVal + "¿";
                }
                hdnparamValues.Value = ixml1;
                Session["paramValues" + iName] = ixml1;
            }
        }
        else
        {
            //If the iview is laoded from a drilldown and refreshed through child window, 
            //the parameters were not being saved as param html construction is not handling set param values from request.from,
            //hence storing it in the viewstate and in postback setting the param values from viewstate.
            if (Request.Form.Count > 0 && Request.Form.Keys[0] != "ScriptManager1" && Request.Form["redisLoadKey"] == null)
            {
                if (Session["paramValues" + iName] != null)
                    hdnparamValues.Value = Session["paramValues" + iName].ToString();
            }
            else
            {
                ResetParamsOnSaveCHWindow(true);
                if (!string.IsNullOrEmpty(hdnparamValues.Value))
                    Session["paramValues" + iName] = hdnparamValues.Value;

            }
        }

        if (Session["project"] == null || Convert.ToString(Session["project"]) == string.Empty)
        {
            SessExpires();
        }
        else
        {
            if (hdnGo.Value == "Go")
            {
                currentPageNo = 1;
            }
            else if (isFromPopup && Session["ivPageNum"] != null && Session["ivPageNum"].ToString() != "")
            {
                currentPageNo = Convert.ToInt32(Session["ivPageNum"]);
                Session["ivPageNum"] = null;
            }
            else if (string.IsNullOrEmpty(lvPage.SelectedValue) | hdnGo.Value == "TSSave")
            {
                currentPageNo = 1;
                if (Session["currentPageNo" + iName] != null)
                {
                    currentPageNo = Convert.ToInt32(Session["currentPageNo" + iName]);
                    Session["currentPageNo" + iName] = null;
                }
            }
            else
            {
                currentPageNo = Convert.ToInt32(lvPage.SelectedValue);
                if (Session["currentPageNo" + iName] != null)
                {
                    currentPageNo = Convert.ToInt32(Session["currentPageNo" + iName]);
                    Session["currentPageNo" + iName] = null;
                }
            }


            if (hdnGo.Value != "clear")
            {
                {
                    GenericRedisFunction2();

                    ConstructParamXml();

                    string idata = string.Empty;
                    JArray ivDataArray = new JArray();
                    if (iName == "inmemdb")
                    {
                        XmlDocument doc1 = new XmlDocument();
                        doc1 = GetRedisData();
                        idata = doc1.OuterXml;
                    }
                    else
                    {
                        if (redisLoadKey != string.Empty && redisLoadKey != null)
                        {
                            idata = processNotificationLoadData(redisLoadKey, user, schemaName);
                        }

                        string sql = "";
                        string cols = "";
                        string hyp = "";
                        string currView = string.Empty;
                        if (idata == string.Empty)
                        {
                            string lvXml = string.Empty;
                            if (Request.QueryString["tstcaption"] != null)
                            {
                                if (hdnLvChangedStructure.Value != "")
                                {
                                    if (hdnLvChangedStructure.Value == "main")
                                    {
                                        hdnLvChangedStructure.Value = objIview.StructureXml;
                                    }
                                    else
                                    {
                                        cols = hdnLvSelectedCols.Value;
                                        hyp = hdnLvSelectedHyperlink.Value;
                                    }
                                }
                                else
                                {
                                    cols = hdnLvSelectedCols.Value;
                                    hyp = hdnLvSelectedHyperlink.Value;

                                    if (cols == string.Empty && hyp == string.Empty && objIview.smartviewSettings != string.Empty)
                                    {
                                        hdnLvChangedStructure.Value = string.Empty;
                                        try
                                        {
                                            string uName = HttpContext.Current.Session["username"].ToString();

                                            JObject smartJSON = JObject.Parse(objIview.smartviewSettings);

                                            JObject viewObj = null;
                                            if (smartJSON[uName] != null && smartJSON[uName]["views"] != null && smartJSON[uName]["views"]["@defaultView"] != null && smartJSON[uName]["views"][smartJSON[uName]["views"]["@defaultView"].ToString()] != null)
                                            {
                                                viewObj = (JObject)smartJSON[uName]["views"][smartJSON[uName]["views"]["@defaultView"].ToString()];
                                            }
                                            else if (smartJSON["all"] != null && smartJSON["all"]["views"] != null && smartJSON["all"]["views"]["@defaultView"] != null && smartJSON["all"]["views"][smartJSON["all"]["views"]["@defaultView"].ToString()] != null)
                                            {
                                                viewObj = (JObject)smartJSON["all"]["views"][smartJSON["all"]["views"]["@defaultView"].ToString()];
                                            }

                                            try
                                            {
                                                if (viewObj != null)
                                                {
                                                    currView = viewObj["groupName"].ToString();
                                                    hdnLvChangedStructure.Value = util.CheckReverseUrlSpecialChars(viewObj["visibleColumns"][0]["structure"].ToString());

                                                    if (hdnLvChangedStructure.Value != string.Empty && viewObj["visibleColumns"][0]["dcs"] != null && viewObj["visibleColumns"][0]["hyperlink"] != null)
                                                    {
                                                        ArrayList arrCols = new ArrayList();
                                                        JObject dcs = (JObject)viewObj["visibleColumns"][0]["dcs"];

                                                        string tempCols = string.Empty;
                                                        string tempHyp = string.Empty;

                                                        try
                                                        {
                                                            foreach (var dc in dcs)
                                                            {
                                                                JObject dcObj = (JObject)dc.Value;
                                                                JArray thisFields = (JArray)dcObj["fields"];
                                                                if (thisFields != null)
                                                                {
                                                                    foreach (var value in thisFields)
                                                                    {
                                                                        arrCols.Add(value.ToString());
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        { }

                                                        tempCols = string.Join(",", (string[])arrCols.ToArray(Type.GetType("System.String")));
                                                        tempHyp = viewObj["visibleColumns"][0]["hyperlink"].ToString();

                                                        cols = hdnLvSelectedCols.Value = tempCols;
                                                        hyp = hdnLvSelectedHyperlink.Value = tempHyp;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            { }
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                }
                                lvXml =
                                "<listviewCols>" +
                                    "<selectedCols>" + cols + "</selectedCols>" +
                                    "<selectedHyperlink>" + hyp + "</selectedHyperlink>" +
                                "</listviewCols>";
                            }

                            ivDataArray = checkCacheData(objIview.purposeString == "" ? pXml : lvXml, objIview.purposeString);

                            if (ivDataArray.Count == 0)
                            {
                                ivDataArray = new JArray();
                                idata = objIview.GetData(iName, hdnWebServiceViewName.Value == "" ? 1 : 0, objIview.iviewDataWSRows, pXml, lvXml, objIview.purposeString, hdnLvChangedStructure.Value).ToString();

                                string[] idataSplit = idata.Split('♠');

                                requestProcess_logtime += idataSplit[0];
                                idata = idataSplit[1];

                                if (idataSplit.Length > 2)
                                {
                                    originalRecsPerPage = idataSplit.LastOrDefault();
                                }

                            }
                            else
                            {
                                idata = ivDataArray[0].ToString();
                            }



                            if (cols != "" && hyp != "")
                            {
                                if (idata.StartsWith("<"))
                                    idata = util.ReplaceFirstOccurrence(idata, "#$#", "#$♥#");
                                string[] resultSplitter = idata.Split(new[] { "#$♥#" }, StringSplitOptions.None);
                                idata = resultSplitter[0];
                                if (resultSplitter.Length > 1)
                                {
                                    try
                                    {
                                        hdnLvChangedStructure.Value = resultSplitter[1];

                                        logobj.CreateDirectDBLog("openiview-dev-" + iName, "Call ListView Cols Plus Structure - GetData", "",
                                            "IView Name :  " + iName, "Success --" + iName + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                                            (resultSplitter.Length > 1 ? " LV StructureXML : " + resultSplitter[1] + Environment.NewLine + Environment.NewLine : ""));
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                        }
                        try
                        {
                            idata = splitAndParseJsonResponse(idata, currView);
                        }
                        catch (Exception ex) { }

                        if (redisLoadKey == null || redisLoadKey == string.Empty)
                        {
                            hdnWebServiceViewName.Value = "";
                        }
                    }

                    //parseError(idata.ToString());
                    if (parseError(idata.ToString()))
                    {
                        isException = true;
                        throw (new Exception(requestProcess_logtime + "♠" + idata.ToString()));
                    }

                    string finalData = string.Empty;

                    try
                    {
                        if (!objIview.requestJSON || iName == "inmemdb")
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(idata.ToString());
                            //get action node from structure xml
                            if (!string.IsNullOrEmpty(objIview.StructureXml))
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(objIview.StructureXml);
                                XmlNode structureAction = xmlDoc.SelectSingleNode("//actions");
                                if (structureAction != null)
                                {
                                    XmlNode tempNode = doc.ImportNode(structureAction, true);
                                    doc.DocumentElement.AppendChild(tempNode);
                                }
                            }

                            //get configurations
                            if (objIview.DsIviewConfig != null && (objIview.DsIviewConfig.Rows.Count > 0))
                            {
                                DataSet ds = new DataSet();
                                ds.Merge(objIview.DsIviewConfig.Copy());
                                ds.DataSetName = "configurations";
                                ds.Tables[0].TableName = "config";
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(ds.GetXml());
                                XmlNode tempNode = doc.ImportNode(xmlDoc.DocumentElement, true);
                                doc.DocumentElement.AppendChild(tempNode);
                            }

                            DataTable templateDT = new DataTable();
                            templateDT = getTempleteStringChoices(iName);
                            if (templateDT.Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                ds.Tables.Add(templateDT.Copy());
                                ds.DataSetName = "templetes";
                                ds.Tables[0].TableName = "templete";
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(ds.GetXml());
                                XmlNode tempNode = doc.ImportNode(xmlDoc.DocumentElement, true);
                                doc.DocumentElement.AppendChild(tempNode);
                            }

                            CreateToolbarButtons(doc);
                            GetSubCaptions(doc);
                            if (!objIview.requestJSON)
                            {
                                CreateHeaderRow(doc, "1", "yes");
                            }

                            if (pivotGroupHeaderNames.Count > 0)
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
                                xmlDoc.PrependChild(xmlDec);
                                XmlElement elemRoot = xmlDoc.CreateElement("PivotAndMerge");
                                XmlElement elem = null;
                                int dataIndex = 0;
                                foreach (string head in pivotGroupHeaderNames)
                                {
                                    dataIndex++;

                                    XmlNode newElem = xmlDoc.CreateNode("element", "header" + dataIndex.ToString(), "");
                                    newElem.InnerText = head;

                                    XmlAttribute sKey = xmlDoc.CreateAttribute("s");
                                    sKey.Value = pivotStartCol[dataIndex - 1].ToString();
                                    XmlAttribute eKey = xmlDoc.CreateAttribute("e");
                                    eKey.Value = pivotEndCol[dataIndex - 1].ToString();

                                    newElem.Attributes.Append(sKey);
                                    newElem.Attributes.Append(eKey);

                                    elemRoot.AppendChild(newElem);
                                }
                                xmlDoc.AppendChild(elemRoot);
                                XmlNode tempNode = doc.ImportNode(xmlDoc.DocumentElement, true);
                                doc.DocumentElement.AppendChild(tempNode);
                            }

                            string dataString = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None);
                            ivVarNode = new StringBuilder();
                            if (ivDataArray.Count > 1)
                            {
                                ivDataArray[0] = dataString;
                                //hdnIViewData.Value = ivDataArray.ToString(Newtonsoft.Json.Formatting.None);
                                ivVarNode.Append("var ivVarNode=JSON.stringify(" + ivDataArray.ToString(Newtonsoft.Json.Formatting.None) + ")");
                                dataFetched = true;
                            }
                            else
                            {
                                //hdnIViewData.Value = dataString;
                                ivVarNode.Append("var ivVarNode=JSON.stringify(" + dataString + ")");
                                dataFetched = true;
                            }
                        }
                        else
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(objIview.StructureXml.ToString());

                            CreateToolbarButtons(doc);
                            GetSubCaptions(doc);

                            string dataString = idata;
                            ivVarNode = new StringBuilder();
                            if (ivDataArray.Count > 1)
                            {
                                ivDataArray[0] = dataString;
                                //hdnIViewData.Value = ivDataArray.ToString(Newtonsoft.Json.Formatting.None);
                                ivVarNode.Append("var ivVarNode=JSON.stringify(" + ivDataArray.ToString(Newtonsoft.Json.Formatting.None) + ")");
                                dataFetched = true;
                            }
                            else
                            {
                                //hdnIViewData.Value = dataString;
                                ivVarNode.Append("var ivVarNode=JSON.stringify(" + dataString + ")");
                                dataFetched = true;
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        logobj.CreateLog("Iview Data Parsing Exception: " + ex.Message, sid, "openiview-dev-" + iName, "");
                    }
                    GridView1.Visible = false;
                    GridView2Wrapper.Visible = true;
                }
            }


            string strUrlParams = hdnparamValues.Value;

            strUrlParams = strUrlParams.Replace("~", "=");
            strUrlParams = strUrlParams.Replace("¿", "&");
            if (objIview.FromHyperLink != null && objIview.FromHyperLink.ToString().ToUpper() != "TRUE")
                util.UpdateNavigateUrl("ivtoivload.aspx?ivname=" + iName + "&" + strUrlParams);
        }
    }

    private string splitAndParseJsonResponse(string result, string currView)
    {
        if (objIview.requestJSON)
        {
            if (result.StartsWith("<"))
                result = util.ReplaceFirstOccurrence(result, "#$#", "#$♥#");
            string[] resultSplitter = result.Split(new[] { "#$♥#" }, StringSplitOptions.None);
            JObject resultJSON = JObject.Parse(resultSplitter[0]);

            JObject resultData = null;

            if (resultJSON["pivot"] != null && resultJSON["pivot"]["pivot"] != null && resultJSON["pivot"]["pivot"].ToString() == "true")
            {
                objIview.isPivotReport = true;
            }

            //json response data only
            if (resultJSON["data"] != null)
            {
                resultData = (JObject)resultJSON["data"];
            }
            //if json data contains headrow then add them in object and cache
            if (resultSplitter[0] != string.Empty && resultData != null && (resultData["headrow"] != null))
            {
                try
                {
                    objIview.headerJSON = resultData["headrow"].ToString().Trim();
                }
                catch (Exception ex) { }

                if (objIview.headerJSON == null)
                {
                    objIview.headerJSON = string.Empty;
                }
            }
            else if (resultSplitter[0] != string.Empty && resultData != null && resultData["headrow"] == null && objIview.headerJSON != string.Empty)
            {
                resultData["headrow"] = JObject.Parse(objIview.headerJSON);
            }

            //if json data contains report headers and footers then add them in object and cache
            if (resultSplitter[0] != string.Empty && resultData != null && (resultData["reporthf"] != null))
            {
                try
                {
                    objIview.reportHF = resultData["reporthf"].ToString().Trim();
                }
                catch (Exception ex) { }

                if (objIview.reportHF == null)
                {
                    objIview.reportHF = string.Empty;
                }
            }
            else if (resultSplitter[0] != string.Empty && resultData != null && resultData["reporthf"] == null && objIview.reportHF != string.Empty)
            {
                resultData["reporthf"] = JObject.Parse(objIview.reportHF);
            }

            if (objIview.reportHF != string.Empty)
            {
                try
                {
                    JObject reporthf = JObject.Parse(objIview.reportHF);
                    if (Session["AxShowAppTitle"] != null && Session["AxShowAppTitle"].ToString().ToLower() == "true")
                    {
                        if (Session["AxAppTitle"] != null && Session["AxAppTitle"].ToString() != string.Empty)
                        {
                            reporthf["exportAppTitle"] = Session["AxAppTitle"].ToString();
                        }
                        else if (Session["projTitle"] != null && Session["projTitle"].ToString() != string.Empty)
                        {
                            reporthf["exportAppTitle"] = Session["projTitle"].ToString();
                        }
                    }
                    objIview.reportHF = (resultData["reporthf"] = reporthf).ToString();
                }
                catch (Exception ex) { }
            }


            //if json data contains template then add it to object and cache
            if (resultSplitter.Length > 1)
            {
                try
                {
                    objIview.ivRowTemplate = resultSplitter[1].Trim();//html can also contain ~ symbol
                }
                catch (Exception ex) { }
            }
            //if row template exist then add it to json in proper heirarchy
            if (objIview.ivRowTemplate != string.Empty)
            {
                try
                {
                    JObject template = JObject.Parse(objIview.ivRowTemplate);
                    if (resultData != null && template != null)
                    {
                        resultData.Add("templetes", template);
                    }
                }
                catch (Exception ex) { }
            }


            if (objIview.smartviewSettings != string.Empty)
            {
                try
                {
                    JObject settings = new JObject();

                    settings.Add("settings", JObject.Parse(objIview.smartviewSettings));

                    if (resultData != null && objIview.smartviewSettings != "" && objIview.smartviewSettings != "{}")
                    {
                        resultData.Add("smartview", settings);
                    }
                }
                catch (Exception ex) { }
            }

            if (objIview.DsIviewConfig != null && (objIview.DsIviewConfig.Rows.Count > 0))
            {
                JObject jObj = null;
                jObj = generateConfigurationJson(objIview.DsIviewConfig);
                if (jObj != null)
                {
                    resultData.Add("configurations", jObj);
                }
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(objIview.StructureXml.ToString());
                XmlNode structureAction = doc.SelectSingleNode("//actions");
                if (structureAction != null)
                {
                    resultData.Add("actions", JObject.Parse(JsonConvert.SerializeXmlNode(structureAction))["actions"]);
                }
            }
            catch (Exception ex) { }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(objIview.StructureXml.ToString());
                XmlNode structureScripts = doc.SelectSingleNode("//scripts");
                if (structureScripts != null)
                {
                    resultData.Add("scripts", JObject.Parse(JsonConvert.SerializeXmlNode(structureScripts))["scripts"]);
                }
            }
            catch (Exception ex) { }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(objIview.StructureXml.ToString());
                XmlNode structureAction = doc.SelectSingleNode("//hyperlinks");
                if (structureAction != null)
                {
                    resultData.Add("hyperlinks", JObject.Parse(JsonConvert.SerializeXmlNode(structureAction))["hyperlinks"]);
                }
            }
            catch (Exception ex) { }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(objIview.StructureXml.ToString());
                XmlNode structureAction = doc.SelectSingleNode("//comps");
                if (structureAction != null)
                {
                    resultData.Add("comps", JObject.Parse(JsonConvert.SerializeXmlNode(structureAction))["comps"]);
                }
            }
            catch (Exception ex) { }

            resultJSON["currView"] = currView;

            result = resultJSON.ToString(Newtonsoft.Json.Formatting.None);
            //caching row tempate and configuration logic
        }
        return result;
    }

    private bool parseError(string data)
    {
        string errMsg = string.Empty;
        string returnString = string.Empty;
        if (!objIview.requestJSON || data.StartsWith(Constants.ERROR))
        {
            returnString = util.ParseXmlErrorNode(data);
        }
        else
        {
            returnString = util.ParseJSonErrorNode(data, false);
        }
        errMsg = returnString;
        if (errMsg != string.Empty)
        {
            if (errMsg == Constants.SESSIONERROR)
            {
                Session.RemoveAll();
                Session.Abandon();
                SessExpires();
                return true;
            }
            else if (errMsg == Constants.SESSIONEXPMSG)
            {
                SessExpires();
                return true;
            }
            else if (errMsg == Constants.ERAUTHENTICATION)
            {
                requestProcess_logtime += "Server - " + errMsg + " ♦ ";
                Response.Redirect(util.ERRPATH + Constants.ERAUTHENTICATION + "*♠*" + requestProcess_logtime);
                return true;
            }
            else
            {
                errMsg = errMsg.Replace("\n", string.Empty);
                requestProcess_logtime += "Server - " + errMsg + " ♦ ";
                Response.Redirect(util.ERRPATH + errMsg + "*♠*" + requestProcess_logtime, false);
                return true;
            }
        }
        return false;
    }

    private JArray checkCacheData(string paramsKey, string purposeString)
    {
        JArray returnArray = new JArray();

        try
        {
            FDW fdwObj = new FDW();

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

            string keyAccess = Constants.RedisIvData;

            if (purposeString != "")
            {
                keyAccess = Constants.RedisLvData;
            }

            string keyPattern = fObj.MakeKeyName(keyAccess, iName, user, paramsKey, -1);

            ArrayList keyList = fObj.GetPrefixedKeys(keyPattern, true);

            keyList.Sort();

            JArray respArray = new JArray();

            if (keyList.Contains(schemaName + "-" + keyPattern + "-0"))
            {
                respArray.Add(fObj.StringFromRedis(keyPattern + "-0", schemaName));
            }
            else
            {
                int index = 0;
                foreach (string key in keyList)
                {
                    string data = fObj.StringFromRedis(key.Substring(key.IndexOf("-") + 1), schemaName);
                    if (index > 0)
                    {
                        if (data.StartsWith("<"))
                            data = util.ReplaceFirstOccurrence(data, "#$#", "#$♥#");
                        data = data.Split(new[] { "#$♥#" }, StringSplitOptions.None)[0];
                    }
                    respArray.Add(data);
                    index++;
                }
            }

            //if (respArray.Count > 0) {
            //    //returnString = respArray.ToString(Newtonsoft.Json.Formatting.None);
            //}

            returnArray = respArray;
        }
        catch (Exception ex)
        { }

        return returnArray;
    }

    public void initMergeAndPivot(String baseDataNodeName)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(objIview.StructureXml);
            XmlNode a61a62Parent = doc.SelectSingleNode("//" + baseDataNodeName + "[@cat='querycol']");
            if (a61a62Parent == null)
            {
                if (baseDataNodeName != "axp__font" && baseDataNodeName != "axrowtype")
                {
                    submerge.Add("-");
                    submergecol.Add("-");
                }
            }
            else
            {
                XmlNode a61 = a61a62Parent.SelectSingleNode("//" + a61a62Parent.Name + "/a61");
                XmlNode a62 = a61a62Parent.SelectSingleNode("//" + a61a62Parent.Name + "/a62");
                if (a61 != null && a62 != null)
                {
                    structureMergeAndPivotLogic(a61);
                    structureMergeAndPivotLogic(a62);
                }
                else
                {
                    submerge.Add("-");
                    submergecol.Add("-");
                }
            }
        }
        catch (Exception ex) { }
    }

    public void structureMergeAndPivotLogic(XmlNode childNode)
    {
        switch (childNode.Name)
        {
            case "a61":

                if (childNode.InnerText == string.Empty)
                    submergecol.Add("-");
                else
                    submergecol.Add(childNode.InnerText);

                if (childNode.InnerText == "0")
                {
                    pivotStartCol.Add(submergecol.Count);
                }
                else
                {
                    if (submergecol[(submergecol.Count - 1)].ToString() != submergecol[submergecol.Count - 2].ToString())
                    {
                        int nodeData = -1;
                        try
                        {
                            nodeData = Convert.ToInt32(childNode.InnerText);
                        }
                        catch (Exception ex)
                        {
                            nodeData = Convert.ToInt32(pivotEndCol[pivotEndCol.Count - 1]);
                        }


                        if (nodeData <= Convert.ToInt32(pivotStartCol[pivotStartCol.Count - 1]))
                        {
                            incrementPivot = true;
                            pivotStartCol.Add(nodeData + 2);
                        }
                        else
                        {
                            pivotStartCol.Add(nodeData);
                        }
                    }
                }
                break;

            case "a62":
                int increment = 0;
                if (incrementPivot)
                {
                    increment = 2;
                    incrementPivot = false;
                }
                if (childNode.InnerText == string.Empty)
                    submerge.Add("-");
                else
                    submerge.Add(childNode.InnerText);

                if (childNode.InnerText == string.Empty)
                {
                    pivotGroupHeaderNames.Add("");
                    pivotEndCol.Add(submergecol.Count + 1 + increment);
                }
                else
                {
                    if (submerge[(submerge.Count - 1)].ToString() != submerge[submerge.Count - 2].ToString())
                    {
                        pivotGroupHeaderNames.Add(childNode.InnerText);
                        pivotEndCol.Add(Convert.ToInt32(pivotStartCol[pivotEndCol.Count]) + 1);
                    }
                    else
                    {
                        pivotEndCol[pivotEndCol.Count - 1] = Convert.ToInt32(pivotEndCol[pivotEndCol.Count - 1]) + 1;
                    }
                }
                break;
            default:
                break;
        }
    }

    private void resetIViewWithoutRowCount()
    {
        if (objIview != null)
        {
            objIview.realRowCount = 0;
            objIview.lastPageCached = false;
            objIview.newPagesArray = new ArrayList();
            objIview.realPageSize = new List<int>();
            objIview.pageSizeWithGTandST = new ArrayList();
            objIview.dsIvPages = new DataSet();
            objIview.cachedPage = 0;
            objIview.paramCacheString = "";
        }
    }

    [WebMethod]
    public static object GetIViewData(string iName, string ivKey, int pageno = 0, int recsPerPage = 0, string paramX = "", string lvXml = "", string lvStructure = "")
    {
        string result = string.Empty, status = string.Empty;
        Util.Util util = new Util.Util();
        IviewData objIview = (IviewData)HttpContext.Current.Session[ivKey];
        if (objIview == null)
            return new { status = "failure", result = Constants.ERAUTHENTICATION };
        if (paramX != "" && HttpContext.Current.Session["xmlPrivateKey-" + iName] != null)
        {
            string encrypted = paramX;
            try
            {
                var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(encrypted);
                string encryptedAesKey = payload.key;
                string aesEncryptedData = payload.data;

                using (RSA rsa = RSA.Create())
                {
                    rsa.FromXmlString(HttpContext.Current.Session["xmlPrivateKey-" + iName].ToString());
                    byte[] encryptedKeyBytes = Convert.FromBase64String(encryptedAesKey);
                    byte[] decryptedKeyBytes = rsa.Decrypt(encryptedKeyBytes, RSAEncryptionPadding.Pkcs1);
                    string aesKeyBase64 = Encoding.UTF8.GetString(decryptedKeyBytes);
                    byte[] keyBytes = Convert.FromBase64String(aesKeyBase64);

                    // Decrypt AES data
                    byte[] aesBytes = Convert.FromBase64String(aesEncryptedData);
                    var decrypted = DecryptWithAesBase64(aesEncryptedData, keyBytes); // Helper function
                    paramX = decrypted;
                }
            }
            catch (Exception ex)
            {
                return new { status = "error", result = "Could not process the request. Please try again." };
            }
        }

        var ds = objIview.GetData(iName, pageno, recsPerPage, paramX, lvXml, objIview.purposeString, lvStructure);
        if (ds != null)
        {
            try
            {
                ds = ds.ToString().Split('♠')[1];

                string errMsg = util.ParseXmlErrorNode(ds.ToString());

                if (errMsg == string.Empty)
                {
                    if (ds.ToString().StartsWith("<"))
                        ds = util.ReplaceFirstOccurrence(ds.ToString(), "#$#", "#$♥#");
                    string[] resultSplitter = ds.ToString().Split(new[] { "#$♥#" }, StringSplitOptions.None);
                    ds = resultSplitter[0];
                }
                else
                {
                    return new { status = "failure", result = errMsg };
                }
            }
            catch (Exception ex)
            {
                return new { status = "failure", result = ds };
            }
        }
        XmlDocument doc = new XmlDocument();
        try
        {
            if (!objIview.requestJSON)
            {
                doc.LoadXml(ds.ToString());
                var json = JsonConvert.SerializeXmlNode(doc);
                return new { status = "success", result = json };
            }
            else
            {
                return new { status = "success", result = ds.ToString() };
            }
        }
        catch (Exception ex)
        {
            return new { status = "failure", result = Constants.CUSTOMERROR };
        }
    }

    [WebMethod(EnableSession = true)]
    public static void ClearRedisDataCache(string iName, string calledFrom, bool isListView)
    {
        Util.Util util = new Util.Util();

        string result = string.Empty, status = string.Empty;
        //IviewData objIview = (IviewData)HttpContext.Current.Session[ivKey];
        //if (objIview == null)
        //    return new { status = "failure", result = Constants.ERAUTHENTICATION };

        string user = HttpContext.Current.Session["user"].ToString();
        user = util.CheckSpecialChars(user);

        string schemaName = string.Empty;
        if (HttpContext.Current.Session["dbuser"] != null)
        {
            schemaName = HttpContext.Current.Session["dbuser"].ToString();
        }

        FDW fdwObj = new FDW();

        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

        string keyPattern = string.Empty;
        ArrayList keyList = new ArrayList();

        if (calledFrom == "refreshParams")
        {
            keyPattern = schemaName + "-" + iName;
            keyList = fObj.GetAllKeys(keyPattern);
        }
        else
        {
            string keyAccess = Constants.RedisIvData;
            if (isListView)
            {
                keyAccess = Constants.RedisLvData;
            }
            keyPattern = fObj.MakeKeyName(keyAccess, iName, user, "*", -1);
            keyList = fObj.GetPrefixedKeys(keyPattern, true, string.Empty, false);
        }

        fdwObj.DeleteKeys(keyList);
    }

    private void GetSubCaptions(XmlDocument xmlDoc)
    {
        XmlNodeList compNodes = default(XmlNodeList);
        XmlNodeList cbaseDataNodes = default(XmlNodeList);
        {
            compNodes = xmlDoc.SelectNodes("//comps");

        }
        ivhead.Clear();
        foreach (XmlNode compNode in compNodes)
        {
            cbaseDataNodes = compNode.ChildNodes;
            int compNodeCnt = 0;
            int toolbarBtnCnt = 0;
            toolbarBtnCnt = cbaseDataNodes.Count - 1;

            if (compNode.Name == "X__Head")
            {
                ivCaption = compNode.Attributes["caption"].ToString();
                ivCaption = ivCaption.Replace("&&", "&");
                lblHeading = ivCaption;
                objIview.IviewCaption = ivCaption;
            }


            for (compNodeCnt = 0; compNodeCnt <= toolbarBtnCnt; compNodeCnt++)
            {
                if (cbaseDataNodes[compNodeCnt].Name.Substring(0, 3) == "lbl")
                {
                    string ivHeading = string.Empty;
                    if (cbaseDataNodes[compNodeCnt].Attributes["hint"] != null && cbaseDataNodes[compNodeCnt].Attributes["hint"].Value != "")
                    {
                        ivHeading = cbaseDataNodes[compNodeCnt].Attributes["hint"].Value;
                    }
                    else if (cbaseDataNodes[compNodeCnt].Attributes["caption"] != null && cbaseDataNodes[compNodeCnt].Attributes["caption"].Value != "")
                    {
                        ivHeading = cbaseDataNodes[compNodeCnt].Attributes["caption"].Value;
                    }
                    ivHeading = ivHeading.Replace("&&", "&");
                    ivhead.Add(ivHeading);
                }
            }
        }
        if (ivhead.Count > 0)
        {
            ConstructSubHeading(ivhead);
        }
    }

    private void ConstructSubHeading(ArrayList ivhead)
    {
        int iv = 0;
        ivCap1.Controls.Clear();
        for (iv = 0; iv <= ivhead.Count - 1; iv++)
        {
            string subHeading = string.Empty;
            subHeading = ivhead[iv].ToString();
            if (subHeading.IndexOf(":") > -1)
            {
                subHeading = ReplaceSqlParamByvalues(subHeading, false);
            }
            subHeading = subHeading.Replace("&&", "&");
            HtmlGenericControl i = new HtmlGenericControl("i");
            i.Attributes.Add("class", "glyphicon glyphicon-chevron-right subb-heading icon-arrows-right");
            Label lbl = new Label();
            lbl.Text = subHeading;
            lbl.CssClass = "IVcapTxt";
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("runat", "server");
            div.Controls.Add(i);
            div.Controls.Add(lbl);
            ivCap1.Controls.Add(div);
        }
        if (ivCap1.Controls.Count > 0 && objIview.IVType != "Interactive")
            ivCap1.Visible = true;
        else
            ivCap1.Visible = false;
    }

    private DataSet GetStaggeredTables(DataSet ds)
    {
        int totalRowCount = ds.Tables[0].Rows.Count;

        List<DataTable> stagTables = SplitTable(ds.Tables[0], displayRowCnt);
        for (int i = 0; i < stagTables.Count; i++)
        {
            DataTable dtTemp = stagTables[i].Copy();
            if (dtTemp.Rows.Count == 1)
            {
                if (dtTemp.Rows[0]["axrowtype"].Equals("gtot"))
                {
                    stagTables[i - 1].Merge(dtTemp);
                    stagTables[i - 1].AcceptChanges();
                    displayRowCnt = stagTables[i - 1].Rows.Count;
                    stagTables.RemoveAt(i);
                }
            }
        }

        objIview.StagTables = stagTables;
        DataSet dsNew = new DataSet();
        DataTable dt = ds.Tables[0].Rows.Cast<System.Data.DataRow>().Take(displayRowCnt).CopyToDataTable();
        dsNew.Tables.Add(dt);
        hdnStagTableNo.Value = "1";
        return dsNew;

    }

    private static List<DataTable> SplitTable(DataTable originalTable, int batchSize)
    {
        List<DataTable> tables = new List<DataTable>();
        int i = 0;
        int j = 1;
        int rowCnt = 0;
        DataTable newDt = originalTable.Clone();
        newDt.TableName = "Table_" + j;
        newDt.Clear();

        foreach (DataRow row in originalTable.Rows)
        {
            DataRow newRow = newDt.NewRow();
            newRow.ItemArray = row.ItemArray;
            newDt.Rows.Add(newRow);
            i++;
            rowCnt++;
            if (i == batchSize)
            {
                tables.Add(newDt);
                j++;
                newDt = originalTable.Clone();
                newDt.TableName = "Table_" + j;
                newDt.Clear();
                i = 0;
            }
            if (rowCnt == originalTable.Rows.Count && newDt.Rows.Count > 0)
            {
                tables.Add(newDt);
            }
        }
        return tables;
    }




    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetJsonForivirGrid(string ivKey)
    {
        IviewData objIview = (IviewData)HttpContext.Current.Session[ivKey];
        if (objIview != null)
        {

            jsonForGrid["customObjIV"] = JsonConvert.SerializeObject(objIview.customBtnIV);
            return jsonForGrid.ToString();
        }
        else
            return "";
    }

    /// Function to load the items into the drop down box which is a parameter to the iview.
    public void ComboFill(XmlNode baseDataNode, string fldName = "")
    {
        foreach (XmlNode rowdNode in baseDataNode.ChildNodes)
        {
            if (rowdNode.ChildNodes[0] != null)
            {
                if (rowdNode.ChildNodes[0].InnerText != "*")
                {
                    string ddlValue = util.CheckSpecialChars(rowdNode.ChildNodes[0].InnerText);
                    ddlValue = util.AdditionalSpecialCharsCheck(ddlValue);
                    arrFillList.Add(ddlValue);
                }
            }

            if (rowdNode.ChildNodes.Count > 1)
            {
                string arrFillListDataAttrStr = string.Empty;
                int ind = -1;
                foreach (XmlNode col in rowdNode.ChildNodes)
                {
                    ind++;
                    if (col.InnerText != "*")
                    {
                        string ddlValue = util.CheckSpecialChars(col.InnerText);
                        ddlValue = util.AdditionalSpecialCharsCheck(ddlValue);
                        arrFillListDataAttrStr += " data-optionsss" + (ind == 0 ? fldName : col.Name) + "=\"" + ddlValue + "\" " + (ind == 0 || col.Attributes["dt"] == null ? "" : " data-dtypeee" + (ind == 0 ? fldName : col.Name) + "=\"" + (col.Attributes["dt"].Value) + "\" ") + " ";
                    }
                }
                arrFillListDataAttr.Add(arrFillListDataAttrStr);
            }
            else
            {
                arrFillListDataAttr.Add(string.Empty);
            }
        }
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
                pValue = util.AdditionalSpecialCharsCheck(pValue);

                pXml = pXml + "<" + pName + ">";
                pXml = pXml + pValue;
                pXml = pXml + "</" + pName + ">";

                if (pName == "mrefresh")
                {
                    if (pValue != "")
                        refreshTime = pValue;
                }

                UpdateParamValues(pName, pValue);
                if (objParams.ParamCaption.Count > 0 && objParams.ParamCaption.ToString() != "")
                {
                    if (i >= 0 && i < objParams.ParamCaption.Count)
                        paramss[objParams.ParamCaption[i].ToString()] = pValue;
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
        if (objParams.ParamCaption.Count > 0 && objParams.ParamCaption.ToString() != "")
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

        }
    }

    private void ResetParamsOnSaveCHWindow(bool delFromSession)
    {
        if (Session["IsFromChildWindow"] != null)
        {

            if (Request.QueryString["ivname"] != null)
                iName = Request.QueryString["ivname"];
            if (Session["paramValues" + iName] != null)
            {
                hdnparamValues.Value = Session["paramValues" + iName].ToString();
                hdnSelParamsAftrChWin.Value = Session["paramValues" + iName].ToString();
            }
            //if (delFromSession)
            Session["IsFromChildWindow"] = null;
        }
    }

    private void ConstructHdnParamValues()
    {
        StringBuilder pValue = new StringBuilder();
        int i = 0;
        for (i = 0; i < objParams.ParamNames.Count; i++)
        {
            pValue.Append(objParams.ParamNames[i] + "~" + objParams.ParamChangedVals[i] + "¿");
        }
        hdnparamValues.Value = pValue.ToString();
    }

    private void UpdateParamValues(string pName, string pValue)
    {
        int idx = -1;
        idx = objParams.ParamNames.IndexOf(pName);
        if (idx == -1)
        {
            objParams.ParamNames.Add(pName);
            objParams.ParamValsOnLoad.Add(pValue);
            objParams.ParamChangedVals.Add(pValue);
        }
        else
        {
            objParams.ParamChangedVals[idx] = pValue;
        }
    }

    protected void button2_Click(object sender, System.EventArgs e)
    {
        pgCap.Visible = false;
        hdnNoOfRecords.Value = string.Empty;
        pages.Text = string.Empty;
        lvPage.Items.Clear();
        lvPage.Visible = false;
        hdnparamValues.Value = string.Empty;
        GridView1.Columns.Clear();
        GridView1.DataSource = null;
        GridView1.DataBind();
        ivCap1.InnerHtml = string.Empty;
        ivCap1.Visible = false;
        GridView2Wrapper.Visible = false;
        resetIViewWithoutRowCount();
    }

    private LinkButton GetButton(string action, string iviewName, string confirm, string allRows, string btnImageId, string caption, bool isFileUpload = false)
    {
        actionButvs = actionButvs + action + '~' + iviewName + '~' + confirm + '~' + allRows + '~' + btnImageId + '~' + caption + '^';
        LinkButton b = new LinkButton();
        b.CssClass = "action handCur l2";
        b.ID = "btn_" + action;
        b.CommandName = "ID";
        b.ToolTip = caption;
        b.CommandArgument = "btn_" + action.ToString();
        if (!isFileUpload)
        {
            b.OnClientClick = "return ActButtonClick('" + b.ClientID + "','" + confirm + "','" + allRows + "')";
        }
        else
        {
            b.OnClientClick = "javascript:callFileUploadAction('" + action + "','" + iviewName + "','" + confirm + "','" + allRows + "');";
        }
        b.Text = b.ToolTip;

        return b;
    }


    private void UpdateHdrFtrInObj(XmlNode hdrFtrNode)
    {
        XmlNodeList chNodes = hdrFtrNode.ChildNodes;
        if (objIview.ReportHdrs == null)
        {
            objIview.ReportHdrs = new ArrayList();
        }

        foreach (XmlNode chNode in chNodes)
        {
            if (hdrFtrNode.Name == "header")
            {
                objIview.ReportHdrs.Add(ReplaceSqlParamByvalues(chNode.InnerText, false));
            }
        }
    }

    private void CreateToolbarButtons(XmlDocument xmlDoc)
    {
        dynamic toolbarData = new JObject();
        XmlNodeList compNodes = default(XmlNodeList);
        XmlNodeList cbaseDataNodes = default(XmlNodeList);
        string Lsttask = null;
        string action = null;
        string tskCap = null;
        ArrayList arrDDBtntype = new ArrayList();
        CreateButtonPos(xmlDoc);

        if (buttonsCreated == false)
        {

            {
                compNodes = xmlDoc.SelectNodes("//comps");
            }



            tskList.Length = 0;
            objIview.ReportHdrs.Clear();

            foreach (XmlNode compNode in compNodes)
            {

                if (compNode.Attributes["dwbtb"] != null && compNode.Attributes["dwbtb"].Value.ToString().ToLower() == "true")
                {
                    string structName = string.Empty;
                    toolbarJSON = CreateDWBToolbarButtons(compNode, structName);
                }
                else
                {
                    cbaseDataNodes = compNode.ChildNodes;
                    int cnt = 0;
                    int compNodeCnt = 0;
                    int toolbarBtnCnt = 0;
                    toolbarBtnCnt = cbaseDataNodes.Count - 1;
                    bool issaveas = true;
                    bool istoxl = true;
                    for (compNodeCnt = 0; compNodeCnt <= toolbarBtnCnt; compNodeCnt++)
                    {
                        if (cbaseDataNodes[compNodeCnt].Name == "header" || cbaseDataNodes[compNodeCnt].Name == "footer")
                        {
                            UpdateHdrFtrInObj(cbaseDataNodes[compNodeCnt]);

                            if (cbaseDataNodes[compNodeCnt].Name == "footer")
                                foreach (XmlElement item in cbaseDataNodes[compNodeCnt])
                                    if (!string.IsNullOrEmpty(objIview.IviewFooter))
                                        objIview.IviewFooter += "|" + item.InnerText;
                                    else
                                        objIview.IviewFooter = item.InnerText;

                            continue;
                        }

                        if (cbaseDataNodes[compNodeCnt].Name.Length >= 6)
                        {
                            if (cbaseDataNodes[compNodeCnt].Name.Substring(0, 7) == "x__head")
                            {
                                ivCaption = cbaseDataNodes[compNodeCnt].Attributes["caption"].Value;
                                ivCaption = ivCaption.Replace("&&", "&");
                            }
                        }

                        string tlhw = string.Empty;
                        string actionn = string.Empty;
                        string taskk = string.Empty;
                        if (cbaseDataNodes[compNodeCnt].Attributes["tlhw"] != null)
                        {
                            tlhw = cbaseDataNodes[compNodeCnt].Attributes["tlhw"].Value;
                            if (cbaseDataNodes[compNodeCnt].Attributes["action"] != null)
                                actionn = cbaseDataNodes[compNodeCnt].Attributes["action"].Value;
                            if (cbaseDataNodes[compNodeCnt].Attributes["task"] != null)
                                taskk = cbaseDataNodes[compNodeCnt].Attributes["task"].Value;
                        }

                        if (cbaseDataNodes[compNodeCnt].Name.Substring(0, 3) == "btn")
                        {
                            string[] arrLeft = null;
                            //If there is a button with empty task and empty action no need to add button

                            if (!(string.IsNullOrEmpty(taskk)) && taskk == "printersetting")
                            {
                                continue;
                            }
                            if (!(string.IsNullOrEmpty(tlhw)) && (!(string.IsNullOrEmpty(actionn)) || !(string.IsNullOrEmpty(taskk))))
                            {
                                arrLeft = tlhw.Split(',');
                                if (arrLeft.Length > 0)
                                {
                                    if (arrTempBtnLeft.IndexOf(arrLeft[1].ToString()) != -1)
                                        arrLeft[1] = Convert.ToString(Convert.ToInt32(arrLeft[1], 10) + 1);
                                    arrTempBtnLeft.Add(arrLeft[1]);
                                }
                            }


                            string task = string.Empty;
                            if (cbaseDataNodes[compNodeCnt].Attributes["task"] != null)
                            {
                                task = cbaseDataNodes[compNodeCnt].Attributes["task"].Value;
                            }

                            JObject toolbarJO = new JObject();
                            toolbarJO.Add("isRoot", false);
                            if (task != "tasks")
                            {
                                toolbarJO.Add("structure", iName);
                            }

                            JObject taskJO = null;

                            string caption = string.Empty;

                            string hint = string.Empty;
                            if (cbaseDataNodes[compNodeCnt].Attributes["hint"] != null)
                            {
                                hint = cbaseDataNodes[compNodeCnt].Attributes["hint"].Value;
                            }
                            string sname = string.Empty;
                            if (cbaseDataNodes[compNodeCnt].Attributes["sname"] != null && cbaseDataNodes[compNodeCnt].Attributes["sname"].Value != "")
                            {
                                sname = cbaseDataNodes[compNodeCnt].Attributes["sname"].Value;
                            }
                            objIview.AssociatedTStruct = sname;

                            string actionVal = string.Empty;
                            if (cbaseDataNodes[compNodeCnt].Attributes["action"] != null)
                            {
                                actionVal = cbaseDataNodes[compNodeCnt].Attributes["action"].Value;
                            }
                            string fileUpload = string.Empty;
                            if (cbaseDataNodes[compNodeCnt].Attributes["fileupload"] != null)
                            {
                                fileUpload = cbaseDataNodes[compNodeCnt].Attributes["fileupload"].Value;
                                if (task != "tasks")
                                {
                                    toolbarJO.Add("fileUpload", fileUpload);
                                }
                            }
                            string confirm = null;
                            XmlNode cnf = cbaseDataNodes[compNodeCnt].Attributes["desc"];
                            if (cnf == null)
                            {
                                confirm = string.Empty;
                            }
                            else
                            {
                                confirm = cnf.Value;
                            }
                            if (iName == "inmemdb")
                            {
                                confirm = "Are you sure you want to delete the selected key(s)?";
                            }
                            if (task != "tasks")
                            {
                                toolbarJO.Add("confirm", confirm);
                            }
                            string appl = null;
                            XmlNode ap = cbaseDataNodes[compNodeCnt].Attributes["apply"];
                            if (ap == null)
                            {
                                appl = string.Empty;
                            }
                            else
                            {
                                appl = ap.Value;
                            }
                            if (task != "tasks")
                            {
                                toolbarJO.Add("allRows", appl);
                            }
                            if (task == "refresh")
                            {
                                arrButtons.Add(string.Empty);
                                //Currently not available for this version

                            }
                            else if (task == "view")
                            {
                                arrButtons.Add(string.Empty);
                                //Currently not available for this version
                            }
                            else if (task == "print")
                            {
                                arrButtons.Add("<li class='liPrint'><a class='print' title='Print' href=\"javascript:toHTML('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ",'true');\" >" + hint + "</a></li>");
                                toolbarJO.Add("href", "javascript:toHTML('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ",'true');");
                            }
                            else if (task == "preview")
                            {
                                arrButtons.Add(string.Empty);
                                //Currently not available for this version
                            }
                            else if (task == "analyze")
                            {
                                arrButtons.Add(string.Empty);
                            }
                            else if (task == "delete")
                            {
                                //Below line is to avoid creating the delete button if the associated tstruct is not given for the iview
                                string deleteStructureName = string.Empty;
                                if (sname == string.Empty)
                                    deleteStructureName = iName;
                                else
                                    deleteStructureName = sname;
                                arrButtons.Add("<li class='liDelete'><a class='delete' title='" + hint + "' href=\"javascript:callDelete('" + deleteStructureName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");\">" + hint + "</a></li>");
                                toolbarJO["structure"] = deleteStructureName;
                                toolbarJO.Add("href", "javascript:callDelete('" + deleteStructureName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");
                                caption = hint;
                            }
                            else if (task == "new")
                            {
                                string newStructureName = string.Empty;
                                if (sname == string.Empty)
                                    newStructureName = iName;
                                else
                                    newStructureName = sname;
                                arrButtons.Add("<li class='liNew'><a class='add' title='" + hint + "' href=\"javascript:callOpenAction('opentstruct','" + newStructureName + "');\">" + hint + "</a></li>");
                                toolbarJO["structure"] = newStructureName;
                                toolbarJO.Add("href", "javascript:callOpenAction('opentstruct','" + newStructureName + "');");
                            }
                            else if (task == "pdf")
                            {
                                arrButtons.Add("<li class='liPdf'><a class='pdf' title='PDF' href=\"javascript:toPDF('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");\">" + hint + "</a></li>");
                                toolbarJO.Add("href", "javascript:toPDF('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");
                            }
                            else if (task == "tasks")
                            {
                                XmlNodeList taskListNodes = default(XmlNodeList);
                                taskListNodes = cbaseDataNodes[compNodeCnt].ChildNodes;
                                if (taskListNodes.Count > 0)
                                {
                                    toolbarJO["isRoot"] = true;
                                    toolbarJO.Add("href", "javascript:void(0);");
                                    Custom custObj = Custom.Instance;
                                    try
                                    {
                                        hint = custObj.AxBeforeAddTaskTitle(hint);
                                    }
                                    catch (Exception ex)
                                    {
                                        hint = "Tasks";
                                    }
                                    arrButtons.Add(string.Empty);

                                    string tasksUlWrapperStart = "<li class='dropdown'><a href='javascript:void(0)' id='tasks' class='dropdown-toggle' data-toggle='dropdown' data-hover='dropdown' title='Tasks' data-close-others='true'>" + "Tasks" + "&nbsp;<span class='caret'></span></a><ul class='dropdown-menu'>";

                                    tskList.Append(tasksUlWrapperStart);
                                    int leftCounter = -1000;
                                    foreach (XmlNode taskItem in taskListNodes)
                                    {
                                        Lsttask = taskItem.Attributes["task"].Value;
                                        tskCap = taskItem.Attributes["caption"].Value;
                                        action = taskItem.Attributes["action"].Value;
                                        Lsttask = Lsttask.ToLower();
                                        if (!string.IsNullOrEmpty(Lsttask) & string.IsNullOrEmpty(action))
                                        {
                                            taskJO = new JObject();
                                            if (Lsttask == "attach")
                                            {
                                                tskList.Append("<li  class='liTaskItems' onclick='javascript:AttachFiles();'><a class='TaskItems' href=\"javascript:void(0)\" id='attach' title=" + tskCap + " >" + tskCap + "</a></li>");
                                                taskJO.Add("href", "javascript:void(0);");
                                                taskJO.Add("onclick", "javascript:AttachFiles();");
                                            }
                                            else if (Lsttask == "email")
                                            {
                                                tskList.Append("<li  class='liTaskItems' onclick='javascript:openEMail(\"" + tid + "\",\"tstruct\",\"0\");'><a class='TaskItems' href=\"javascript:void(0)\" id='email' title=" + tskCap + " >" + tskCap + "</a></li>");
                                                taskJO.Add("href", "javascript:void(0);");
                                                taskJO.Add("onclick", "javascript:openEMail('" + tid + "','tstruct','0');");
                                            }
                                            else if (Lsttask == "print")
                                            {
                                                //Currently not available for this version
                                            }
                                            else if (Lsttask == "pdf")
                                            {
                                                tskList.Append("<li  class='liTaskItems' onclick='javascript:toPDF(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ");'><a class='TaskItems' href=\"javascript:void(0)\" id='pdf' title=" + tskCap + " >" + tskCap + "</a></li>");
                                                taskJO.Add("href", "javascript:void(0);");
                                                taskJO.Add("onclick", "javascript:toPDF('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");
                                            }
                                            else if (Lsttask == "save as")
                                            {
                                                issaveas = false;
                                                //to display PDF and HTML options in Save as dropdown
                                                tskList.Append("<li  class='liTaskItems' onclick='javascript:toPDF(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ");'><a class='TaskItems' href=\"javascript:void(0)\" id='pdf' title='PDF' >PDF</a></li>");
                                                tskList.Append("<li  class='liTaskItems' onclick='javascript:toHTML(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ",true);'><a class='TaskItems' href=\"javascript:void(0)\" id='HTML' title='Print' >Print</a></li>");

                                                taskJO.Add("href", "javascript:void(0);");
                                                taskJO.Add("onclick", JObject.Parse("{\"PDF\": \"javascript:toPDF('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");\", \"Print\": \"javascript:toHTML('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ",true);\", \"HTML\": \"javascript:SetDatatableExport('html');\", \"JSON\": \"javascript:SetDatatableExport('json');\", \"Copy\": \"javascript:SetDatatableExport('copy');\"}"));
                                            }
                                            else if (Lsttask == "preview")
                                            {
                                                //Currently not available for this version
                                            }
                                            else if (Lsttask == "to xl")
                                            {
                                                istoxl = false;
                                                tskList.Append("<li  class='liTaskItems' onclick='javascript:toExcelWeb(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ");'><a class='TaskItems' href=\"javascript:void(0)\" id='excel' title='Excel'>Excel</a></li>");
                                                taskJO.Add("href", "javascript:void(0);");
                                                taskJO.Add("onclick", "javascript:toExcelWeb('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");
                                                tskCap = "Excel";
                                            }
                                            else if (Lsttask == "params")
                                            {
                                            }

                                            taskJO.Add("isRoot", false);
                                            taskJO.Add("task", Lsttask);
                                            taskJO.Add("caption", tskCap);
                                            taskJO.Add("isAction", false);
                                            taskJO.Add("structure", iName);
                                            taskJO.Add("key", Lsttask);
                                            taskJO.Add("left", (leftCounter++).ToString());
                                            toolbarJO.Add(Lsttask, taskJO);
                                        }
                                    }

                                    if (tskList.ToString() == tasksUlWrapperStart)
                                    {
                                        tskList.Clear();
                                    }
                                    else
                                    {
                                        tskList.Append("</ul></li>");
                                    }
                                }
                                else
                                {
                                    arrButtons.Add(string.Empty);
                                }

                                arrButtons[(arrButtons.Count) - 1] += (tskList).ToString();
                            }
                            else if (task == "find")
                            {
                                arrButtons.Add(string.Empty);
                                defaultBut = defaultBut + string.Empty;
                            }
                            else if (task == "save as")
                            {
                                issaveas = false;
                                //to display PDF and HTML options in Save as dropdown
                                tskList.Append("<li  class='liSaveAs' onclick='javascript:toPDF(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ");'><a class='SaveAs' href=\"javascript:void(0)\" id='pdf2' title='PDF' >PDF</a></li>");
                                tskList.Append("<li  class='liSaveAs' onclick='javascript:toHTML(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ",true);'><a class='SaveAs' href=\"javascript:void(0)\" id='HTML2' title='Print' >Print</a></li>");
                                toolbarJO.Add("href", "javascript:void(0);");
                                toolbarJO.Add("onclick", JObject.Parse("{\"PDF\": \"javascript:toPDF('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");\", \"Print\": \"javascript:toHTML('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ",true);\", \"HTML\": \"javascript:SetDatatableExport('html');\", \"JSON\": \"javascript:SetDatatableExport('json');\", \"Copy\": \"javascript:SetDatatableExport('copy');\"}"));
                            }
                            else if (task == "to xl")
                            {
                                istoxl = false;
                                arrButtons.Add("<li class='liToExcel'><a class='toexcel' title='Save as Excel' href='javascript:toExcelWeb(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ");'>" + hint + "</a></li>");
                                toolbarJO.Add("href", "javascript:toExcelWeb('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");
                            }
                            else if (task == string.Empty && actionVal != string.Empty)
                            {
                                string imgName = string.Empty;
                                if (cbaseDataNodes[compNodeCnt].Attributes["caption"] != null)
                                {
                                    caption = cbaseDataNodes[compNodeCnt].Attributes["caption"].Value;
                                }

                                string btnImage = string.Empty;
                                if (cbaseDataNodes[compNodeCnt].Attributes["img"] != null)
                                {
                                    imgName = cbaseDataNodes[compNodeCnt].Attributes["img"].Value;
                                    btnImage = imgName.ToString();
                                }

                                if (!util.IsImageAvailable(btnImage))
                                    btnImage = "";
                                else
                                {
                                    btnImage = btnImage.Substring(0, btnImage.IndexOf(".") + 1) + "png";
                                }


                                if (actionVal != string.Empty)
                                {
                                    toolbarJO.Add("isAction", true);
                                    if (string.IsNullOrEmpty(cbaseDataNodes[compNodeCnt].Attributes["caption"].Value) && cbaseDataNodes[compNodeCnt].Attributes["hint"].Value == "New")
                                    {
                                        arrButtons.Add(string.Empty);
                                        actionBtns.Add(GetButton(actionVal, iName, confirm, appl, btnImage, "New"));
                                        toolbarJO.Add("href", "javascript:void(0);");
                                        toolbarJO.Add("onclick", "return ActButtonClick('btn_" + actionVal + "','" + confirm + "','" + appl + "');");
                                    }
                                    else if (fileUpload == "y")
                                    {

                                        actionBtns.Add(GetButton(actionVal, iName, confirm, appl, btnImage, (hint != string.Empty ? hint : caption), true));

                                        toolbarJO.Add("href", "javascript:void(0);");
                                        toolbarJO.Add("onclick", "javascript:callFileUploadAction('" + actionVal + "','" + iName + "','" + confirm + "','" + appl + "');");
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(caption) & !string.IsNullOrEmpty(imgName))
                                        {
                                            //Display Image with caption as hint.
                                            arrButtons.Add(string.Empty);
                                            actionBtns.Add(GetButton(actionVal, iName, confirm, appl, btnImage, caption));
                                            toolbarJO.Add("href", "javascript:void(0);");
                                            toolbarJO.Add("onclick", "return ActButtonClick('btn_" + actionVal + "','" + confirm + "','" + appl + "');");
                                        }
                                        else if (!string.IsNullOrEmpty(imgName) & string.IsNullOrEmpty(caption))
                                        {
                                            //Display only image
                                            arrButtons.Add(string.Empty);
                                            actionBtns.Add(GetButton(actionVal, iName, confirm, appl, btnImage, (hint != string.Empty ? hint : caption)));
                                            toolbarJO.Add("href", "javascript:void(0);");
                                            toolbarJO.Add("onclick", "return ActButtonClick('btn_" + actionVal + "','" + confirm + "','" + appl + "');");
                                        }
                                        else if (!string.IsNullOrEmpty(caption) & string.IsNullOrEmpty(imgName))
                                        {
                                            //Display button with Caption
                                            arrButtons.Add(string.Empty);
                                            actionBtns.Add(GetButton(actionVal, iName, confirm, appl, btnImage, caption));
                                            toolbarJO.Add("href", "javascript:void(0);");
                                            toolbarJO.Add("onclick", "return ActButtonClick('btn_" + actionVal + "','" + confirm + "','" + appl + "');");
                                        }
                                        else if (string.IsNullOrEmpty(caption) & string.IsNullOrEmpty(imgName))
                                        {
                                            arrButtons.Add(string.Empty);
                                            actionBtns.Add(GetButton(actionVal, iName, confirm, appl, btnImage, (hint != string.Empty ? hint : caption)));
                                            toolbarJO.Add("href", "javascript:void(0);");
                                            toolbarJO.Add("onclick", "return ActButtonClick('btn_" + actionVal + "','" + confirm + "','" + appl + "');");
                                        }

                                    }
                                }
                                else
                                {
                                    toolbarJO.Add("isAction", false);
                                }

                                if (!string.IsNullOrEmpty(task))
                                {
                                    arrButtons.Add(string.Empty);
                                }
                                else if (string.IsNullOrEmpty(task) & string.IsNullOrEmpty(actionVal))
                                {
                                    arrBtnLeftVals.Add(string.Empty);

                                }
                            }

                            if (objIview.purposeString != "" && Session["IsAxOldDataList"] != null && Session["IsAxOldDataList"].ToString() == "true")
                            {
                                int leftCounter = -1000;
                                string tasksUlWrapperStart = string.Empty;
                                if (issaveas)
                                {
                                    task = "tasks";
                                    toolbarJO["isRoot"] = true;
                                    toolbarJO.Add("href", "javascript:void(0);");
                                    arrButtons.Add(string.Empty);
                                    tasksUlWrapperStart = "<li class='dropdown'><a href='javascript:void(0)' id='tasks' class='dropdown-toggle' data-toggle='dropdown' data-hover='dropdown' title='Tasks' data-close-others='true'>" + "Tasks" + "&nbsp;<span class='caret'></span></a><ul class='dropdown-menu'>";
                                    tskList.Append(tasksUlWrapperStart);
                                    taskJO = new JObject();
                                    tskList.Append("<li  class='liTaskItems' onclick='javascript:toPDF(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ");'><a class='TaskItems' href=\"javascript:void(0)\" id='pdf' title='PDF' >PDF</a></li>");
                                    tskList.Append("<li  class='liTaskItems' onclick='javascript:toHTML(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ",true);'><a class='TaskItems' href=\"javascript:void(0)\" id='HTML' title='Print' >Print</a></li>");

                                    taskJO.Add("href", "javascript:void(0);");
                                    taskJO.Add("onclick", JObject.Parse("{\"PDF\": \"javascript:toPDF('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");\", \"Print\": \"javascript:toHTML('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ",true);\", \"HTML\": \"javascript:SetDatatableExport('html');\", \"JSON\": \"javascript:SetDatatableExport('json');\", \"Copy\": \"javascript:SetDatatableExport('copy');\"}"));
                                    taskJO.Add("isRoot", false);
                                    taskJO.Add("task", "save as");
                                    taskJO.Add("caption", tskCap);
                                    taskJO.Add("isAction", false);
                                    taskJO.Add("structure", iName);
                                    taskJO.Add("key", "save as");
                                    taskJO.Add("left", (leftCounter++).ToString());
                                    toolbarJO.Add("save as", taskJO);

                                }
                                if (istoxl)
                                {
                                    task = "tasks";
                                    arrButtons.Add(string.Empty);
                                    tasksUlWrapperStart = "<li class='dropdown'><a href='javascript:void(0)' id='tasks' class='dropdown-toggle' data-toggle='dropdown' data-hover='dropdown' title='Tasks' data-close-others='true'>" + "Tasks" + "&nbsp;<span class='caret'></span></a><ul class='dropdown-menu'>";
                                    tskList.Append(tasksUlWrapperStart);
                                    taskJO = new JObject();
                                    tskList.Append("<li  class='liTaskItems' onclick='javascript:toExcelWeb(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ");'><a class='TaskItems' href=\"javascript:void(0)\" id='excel' title='Excel'>Excel</a></li>");
                                    taskJO.Add("href", "javascript:void(0);");
                                    taskJO.Add("onclick", "javascript:toExcelWeb('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");
                                    tskCap = "Excel";
                                    taskJO.Add("isRoot", false);
                                    taskJO.Add("task", "to xl");
                                    taskJO.Add("caption", tskCap);
                                    taskJO.Add("isAction", false);
                                    taskJO.Add("structure", iName);
                                    taskJO.Add("key", "to xl");
                                    taskJO.Add("left", (leftCounter++).ToString());
                                    toolbarJO.Add("to xl", taskJO);
                                }
                                issaveas = false;
                                istoxl = false;
                            }

                            if (toolbarJO != null)
                            {
                                if (task != "tasks")
                                {
                                    toolbarJO.Add("hint", hint);
                                    toolbarJO.Add("caption", caption);
                                }
                                else
                                {
                                    toolbarJO["isRoot"] = true;
                                    toolbarJO.Add("groupName", "Options");
                                }
                                if (arrTempBtnLeft.Count == 0)
                                {
                                    toolbarJO.Add("left", "0");
                                }
                                else
                                {
                                    toolbarJO.Add("left", arrTempBtnLeft[arrTempBtnLeft.Count - 1].ToString());
                                }
                                string keyName = task != string.Empty ? task : actionVal;
                                if (task == "new" || task == "delete")
                                {
                                    keyName = task;
                                }
                                toolbarJO["key"] = keyName;
                                toolbarJSON[keyName] = toolbarJO;
                            }

                        }
                    }//end of for
                }
            }
            AlignToolbarBtns();
        }

        if (string.IsNullOrEmpty(lblHeading) & !string.IsNullOrEmpty(ivCaption))
        {
            lblHeading = ivCaption;
        }
        else
        {

        }

        buttonsCreated = true;


        ConstructActionBtns();
    }

    private JObject CreateDWBToolbarButtons(XmlNode compNode, string sname, string parentID = "")
    {
        XmlNodeList cbaseDataNodes = default(XmlNodeList);
        JObject mainToolbarJO = new JObject();

        cbaseDataNodes = compNode.ChildNodes;
        foreach (XmlNode cbaseDataNode in cbaseDataNodes)
        {
            if (cbaseDataNode.Name.Length >= 6)
            {
                if (cbaseDataNode.Name.Length >= 7 && cbaseDataNode.Name.Substring(0, 7) == "x__head")
                {
                    ivCaption = cbaseDataNode.Attributes["caption"].Value;
                    ivCaption = ivCaption.Replace("&&", "&");
                }
            }
            string action = string.Empty;
            string task = string.Empty;
            string visible = "true";
            string script = string.Empty;
            string caption = string.Empty;
            string btnHint = string.Empty;
            string key = string.Empty;
            string actionVal = string.Empty;
            string fileUpload = string.Empty;
            dynamic icon;
            string confirm = null;
            bool isroot = false;
            JObject taskJO = null;
            JObject toolbarJO = new JObject();
            if (cbaseDataNode.Name != "icon")
            {
                if (cbaseDataNode.Attributes["task"] != null)
                {
                    task = cbaseDataNode.Attributes["task"].Value.ToLower();

                    if (parentID != string.Empty && task == string.Empty)
                    {
                        continue;
                    }
                }
                if (cbaseDataNode.Attributes["visible"] != null)
                {
                    visible = cbaseDataNode.Attributes["visible"].Value.ToLower();
                }
                if (!(string.IsNullOrEmpty(task)) && task == "printersetting")
                {
                    continue;
                }
                if (visible == "false")
                {
                    continue;
                }
                if (cbaseDataNode.Attributes["folder"] != null)
                {
                    isroot = cbaseDataNode.Attributes["folder"].Value.ToLower() == "true" ? true : false;
                }
                if (cbaseDataNode.Attributes["key"] != null)
                {
                    key = cbaseDataNode.Attributes["key"].Value;
                }
                if (cbaseDataNode.Attributes["caption"] != null)
                {
                    caption = cbaseDataNode.Attributes["caption"].Value;
                }
                if (cbaseDataNode.Attributes["hint"] != null)
                {
                    btnHint = cbaseDataNode.Attributes["hint"].Value;
                }
                if (cbaseDataNode.Attributes["action"] != null)
                {
                    actionVal = cbaseDataNode.Attributes["action"].Value;
                }
                if (cbaseDataNode.Attributes["fileupload"] != null)
                {
                    fileUpload = cbaseDataNode.Attributes["fileupload"].Value;
                    if (task != "tasks")
                    {
                        toolbarJO.Add("fileUpload", fileUpload);
                    }
                }
                if (cbaseDataNode.Attributes["img"] != null && cbaseDataNode.Attributes["img"].Value != "" && util.IsImageAvailable(cbaseDataNode.Attributes["img"].Value, "icon"))
                {
                    icon = cbaseDataNode.Attributes["img"].Value;
                }
                else if (cbaseDataNode.SelectSingleNode("icon") != null)
                {
                    icon = new JObject();
                    XmlNode iconNode = cbaseDataNode.SelectSingleNode("icon");
                    if (iconNode.SelectSingleNode("text") != null && iconNode.SelectSingleNode("addClass") != null)
                    {
                        icon["text"] = iconNode.SelectSingleNode("text").InnerText;
                        icon["addClass"] = iconNode.SelectSingleNode("addClass").InnerText;
                    }
                }
                else
                {
                    icon = string.Empty;
                }
                if (cbaseDataNode.Attributes["sname"] != null && cbaseDataNode.Attributes["sname"].Value != "")
                {
                    sname = cbaseDataNode.Attributes["sname"].Value;
                }
                objIview.AssociatedTStruct = sname;
                if (task != "tasks")
                {
                    toolbarJO.Add("structure", iName);
                }
                XmlNode cnf = cbaseDataNode.Attributes["desc"];
                if (cnf == null)
                {
                    confirm = string.Empty;
                }
                else
                {
                    confirm = cnf.Value;
                }
                if (iName == "inmemdb")
                {
                    confirm = "Are you sure you want to delete the selected key(s)?";
                }
                if (task != "tasks")
                {
                    toolbarJO.Add("confirm", confirm);
                }
                string appl = null;
                XmlNode ap = cbaseDataNode.Attributes["apply"];
                if (ap == null)
                {
                    appl = string.Empty;
                }
                else
                {
                    appl = ap.Value;
                }
                if (task != "tasks")
                {
                    toolbarJO.Add("allRows", appl);
                }
                if (isroot)
                {
                    JObject childJO = null;
                    if (task == "tasks")
                        childJO = CreateDWBToolbarButtons(cbaseDataNode, sname, "task");
                    else
                        childJO = CreateDWBToolbarButtons(cbaseDataNode, sname);
                    toolbarJO.Merge(childJO, new JsonMergeSettings
                    {
                        // union array values together to avoid duplicates
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                }
                else
                {
                    switch (task)
                    {

                        case "refresh":
                            {
                                //Currently not available for this version
                                break;
                            }
                        case "view":
                            {
                                //Currently not available for this version
                                break;
                            }
                        case "print":
                            {

                                toolbarJO.Add("href", "javascript:toHTML('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ",'true');");
                                break;
                            }
                        case "preview":
                            {
                                break;
                            }
                        case "analyze":
                            {
                                break;
                            }
                        case "delete":
                            {
                                //Below line is to avoid creating the delete button if the associated tstruct is not given for the iview
                                string deleteStructureName = string.Empty;
                                if (sname == string.Empty)
                                    deleteStructureName = iName;
                                else
                                    deleteStructureName = sname;
                                toolbarJO["structure"] = deleteStructureName;
                                toolbarJO.Add("href", "javascript:callDelete('" + deleteStructureName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");

                                break;
                            }
                        case "new":
                            {
                                string newStructureName = string.Empty;
                                if (sname == string.Empty)
                                    newStructureName = iName;
                                else
                                    newStructureName = sname;
                                toolbarJO["structure"] = newStructureName;
                                toolbarJO.Add("href", "javascript:callOpenAction('opentstruct','" + newStructureName + "');");
                                break;
                            }
                        case "pdf":
                            {
                                toolbarJO.Add("href", "javascript:toPDF('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");
                                break;
                            }

                        case "find":
                            {
                                break;
                            }
                        case "save as":
                            {
                                toolbarJO.Add("href", "javascript:void(0);");
                                toolbarJO.Add("onclick", JObject.Parse("{\"PDF\": \"javascript:toPDF('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");\", \"Print\": \"javascript:toHTML('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ",true);\", \"HTML\": \"javascript:SetDatatableExport('html');\", \"JSON\": \"javascript:SetDatatableExport('json');\", \"Copy\": \"javascript:SetDatatableExport('copy');\"}"));
                                break;
                            }
                        case "to xl":
                            {
                                toolbarJO.Add("href", "javascript:toExcelWeb('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");
                                break;
                            }
                        case "tasks":
                            {
                                isroot = true;
                                JObject childJO = null;
                                childJO = CreateDWBToolbarButtons(cbaseDataNode, sname, "task");
                                toolbarJO.Merge(childJO, new JsonMergeSettings
                                {
                                    // union array values together to avoid duplicates
                                    MergeArrayHandling = MergeArrayHandling.Union
                                });
                                break;
                            }
                        case "":
                            {
                                if (actionVal != string.Empty)
                                {
                                    if (actionVal != string.Empty)
                                    {
                                        toolbarJO.Add("isAction", true);
                                        if (string.IsNullOrEmpty(cbaseDataNode.Attributes["caption"].Value) && cbaseDataNode.Attributes["hint"].Value == "New")
                                        {
                                            arrButtons.Add(string.Empty);
                                            toolbarJO.Add("href", "javascript:void(0);");
                                            toolbarJO.Add("onclick", "return ActButtonClick('btn_" + actionVal + "','" + confirm + "','" + appl + "');");

                                        }
                                        else if (fileUpload == "y")
                                        {

                                            toolbarJO.Add("href", "javascript:void(0);");
                                            toolbarJO.Add("onclick", "javascript:callFileUploadAction('" + actionVal + "','" + iName + "','" + confirm + "','" + appl + "');");
                                        }
                                        else
                                        {
                                            toolbarJO.Add("href", "javascript:void(0);");
                                            toolbarJO.Add("onclick", "return ActButtonClick('btn_" + actionVal + "','" + confirm + "','" + appl + "');");
                                        }
                                    }
                                    else
                                    {
                                        toolbarJO.Add("isAction", false);
                                    }

                                    if (!string.IsNullOrEmpty(task))
                                    {
                                        arrButtons.Add(string.Empty);
                                    }
                                    else if (string.IsNullOrEmpty(task) & string.IsNullOrEmpty(actionVal))
                                    {
                                        arrBtnLeftVals.Add(string.Empty);

                                    }
                                }
                                break;
                            }
                    }
                }

                if (toolbarJO != null)
                {

                    if (task != "tasks")
                    {
                        toolbarJO.Add("groupName", caption);
                    }
                    else
                    {
                        toolbarJO.Add("groupName", "Options");
                    }

                    toolbarJO.Add("hint", (btnHint == "" ? caption : btnHint));
                    toolbarJO.Add("caption", caption);
                    if (key == "")
                    {
                        key = task != string.Empty ? task : actionVal;
                    }
                    if (task == "new" || task == "delete")
                    {
                        key = task;
                    }
                    toolbarJO["key"] = key;
                    toolbarJO.Add("icon", icon);
                    toolbarJO.Add("isRoot", isroot);
                    if (mainToolbarJO[key] == null)
                    {
                        mainToolbarJO[key] = toolbarJO;
                    }
                }

            }
        }
        //end of for
        buttonsCreated = true;
        return mainToolbarJO;
    }
    private void ConstructActionBtns()
    {
        string groupButtons = "";
        objIview.customBtnIV.Clear();
        if (ViewState["ivGroupBtns"] != null)
        {
            groupButtons = ViewState["ivGroupBtns"].ToString();
        }
        if (actionBtns.Count > 0)
        {
            if (groupButtons != "")
            {
                try
                {
                    ArrayList tempChecker = actionBtns;
                    String[] btnGroups = groupButtons.Split('~');
                    for (int i = 0; i < btnGroups.Length; i++)
                    {

                        string[] seperateButton = (btnGroups[i].Split('-')[1]).ToString().Split(',');

                        JObject actionButtonJO = new JObject();

                        for (int j = 0; j < seperateButton.Length; j++)
                        {

                            for (int k = 0; k < actionBtns.Count; k++)
                            {
                                if (((System.Web.UI.Control)(actionBtns[k])).ClientID.Equals("btn_" + seperateButton[j]))
                                {

                                    tempChecker.RemoveAt(k);
                                }

                                if (toolbarJSON[seperateButton[j]] != null && toolbarJSON[seperateButton[j]]["isRoot"] == false)
                                {
                                    actionButtonJO.Add(seperateButton[j], toolbarJSON[seperateButton[j]]);
                                    toolbarJSON.Property(seperateButton[j]).Remove();
                                }
                            }

                        }



                        string btnGroupCaption = btnGroups[i].Split('-')[0].ToString();

                        actionButtonJO.Add("isRoot", true);
                        actionButtonJO.Add("groupName", btnGroupCaption);
                        actionButtonJO["key"] = "customGroup" + (i + 1);
                        toolbarJSON.Add("customGroup" + (i + 1), actionButtonJO);


                    }


                }
                catch (Exception ex)
                {
                    logobj.CreateLog("Iview - Grouped Button -\r\nValue: " + groupButtons + "\r\nException: " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "openiview-dev-" + iName, string.Empty);
                }
            }

        }
        objIview.ToolBarBtn = actionButvs;
    }

    private void CreateHeaderRow(XmlDocument xmlDoc, string pageNo, string firsttime)
    {

        if (hdnGo.Value == "refreshparams")
            return;

        if (firsttime.ToLower() == "yes")
        {
            {

                XmlNodeList productNodes = default(XmlNodeList);
                XmlNodeList baseDataNodes = default(XmlNodeList);
                productNodes = xmlDoc.SelectNodes("//headrow");
                foreach (XmlNode productNode in productNodes)
                {

                    baseDataNodes = productNode.ChildNodes;
                    foreach (XmlNode baseDataNode in baseDataNodes)
                    {

                        {
                            XmlNode pivotNode = baseDataNodes.Cast<XmlNode>().Where(item => item.Name == "pivotghead").ToList().FirstOrDefault();
                            if (baseDataNode.Name != "pivotghead" && pivotNode == null || pivotNode.OuterXml == "<pivotghead />")
                            {
                                initMergeAndPivot(baseDataNode.Name);
                            }
                        }
                    }
                }
            }


        }
        else
        {
            //clear the headrow attributes
            XmlNodeList productNodes = default(XmlNodeList);
            XmlNodeList baseDataNodes = default(XmlNodeList);
            productNodes = xmlDoc.SelectNodes("//headrow");
            foreach (XmlNode productNode in productNodes)
            {
                baseDataNodes = productNode.ChildNodes;
                foreach (XmlNode baseDataNode in baseDataNodes)
                {
                    baseDataNode.RemoveAll();
                }
            }
        }
    }


    private void AlignToolbarBtns()
    {
        if (IsPostBack)
            defaultBut = string.Empty;
        int tempLftCnt = 0;
        int BtnLftCnt = 0;
        int i = 0;

        for (i = 0; i <= arrButtons.Count; i++)
        {
            arrSortedButtons.Add(string.Empty);
        }
        bool IsBtnCnt = false;

        if (objIview.purposeString == "")
        {

            for (BtnLftCnt = 0; BtnLftCnt < arrBtnLeftVals.Count; BtnLftCnt++)
            {
                for (tempLftCnt = 0; tempLftCnt < arrTempBtnLeft.Count; tempLftCnt++)
                {
                    if (arrBtnLeftVals[BtnLftCnt].ToString() == arrTempBtnLeft[tempLftCnt].ToString())
                    {
                        if (arrTempBtnLeft.Count != arrButtons.Count)
                        {
                            IsBtnCnt = true;
                        }
                        else
                        {
                            arrSortedButtons[BtnLftCnt] = arrButtons[tempLftCnt];
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            arrSortedButtons = arrButtons;
        }

        int j = 0;
        if (IsBtnCnt == true)
        {
            for (j = 0; j <= arrButtons.Count - 1; j++)
            {
                defaultBut += arrButtons[j].ToString();
            }
        }

        for (j = 0; j <= arrSortedButtons.Count - 1; j++)
        {
            if (!string.IsNullOrEmpty(arrSortedButtons[j].ToString()))
            {
                defaultBut += arrSortedButtons[j].ToString();
            }
        }
        if (iName == "inmemdb")
        {
            defaultBut += "<li  class='actionWrapper' onclick='window.location.href = window.location.href;'><a class='action singleaction' href=\"javascript:void(0)\" id='inMemRefresh' title='Reload' >Reload</a></li>";


            JObject redisReload = new JObject();
            redisReload.Add("isRoot", false);
            redisReload.Add("structure", iName);
            redisReload.Add("confirm", "");
            redisReload.Add("allRows", "");
            redisReload.Add("left", "");
            redisReload.Add("hint", "Reload");
            redisReload.Add("caption", "Reload");
            redisReload.Add("href", "javascript:void(0);");
            redisReload.Add("onclick", "window.location.href = window.location.href;");
            redisReload["key"] = "redisReload";
            toolbarJSON["redisReload"] = redisReload;
        }


        if (Request.QueryString["tstcaption"] != null && Session["AxpExcelExport"] != null && Session["AxpExcelExport"].ToString() == "true")
        {
            defaultBut += "<li class='liToExcel'><a class='toexcel' title='Save as Excel' href='javascript:toExcelWeb(\"" + iName + "\"," + (objIview.purposeString == "" ? "\"Iview\"" : "\"lview\"") + ");'>Save as Excel</a></li>";
            JObject axpExcelExport = new JObject();
            axpExcelExport.Add("isRoot", false);
            axpExcelExport.Add("structure", iName);
            axpExcelExport.Add("confirm", "");
            axpExcelExport.Add("allRows", "");
            axpExcelExport.Add("left", "");
            axpExcelExport.Add("hint", "Save as Excel");
            axpExcelExport.Add("caption", "Save as Excel");
            axpExcelExport.Add("href", "javascript:toExcelWeb('" + iName + "'," + (objIview.purposeString == "" ? "'Iview'" : "'lview'") + ");");
            axpExcelExport.Add("onclick", "");
            axpExcelExport["key"] = "axpExcelExport";
            toolbarJSON["axpExcelExport"] = axpExcelExport;
        }

        objIview.ToolbarHtml = defaultBut;

        ShowHideFilters();

    }

    private void CreateButtonPos(XmlDocument xmldoc)
    {
        XmlNodeList compNodes = default(XmlNodeList);
        XmlNodeList cbaseDataNodes = default(XmlNodeList);

        compNodes = xmldoc.SelectNodes("//data/comps");

        foreach (XmlNode compNode in compNodes)
        {
            cbaseDataNodes = compNode.ChildNodes;
            int cnt1 = 0;
            int compNodeCnt = 0;
            int ToolbarBtnCnt = 0;
            string[] arrLeft = null;
            ToolbarBtnCnt = cbaseDataNodes.Count - 1;
            for (compNodeCnt = ToolbarBtnCnt; compNodeCnt >= 0; compNodeCnt += -1)
            {
                if (cbaseDataNodes[compNodeCnt].Name.Substring(0, 3) == "btn")
                {
                    string tlhw = string.Empty;
                    string actionn = string.Empty;
                    string taskk = string.Empty;
                    if (cbaseDataNodes[compNodeCnt].Attributes["tlhw"] != null)
                    {
                        tlhw = cbaseDataNodes[compNodeCnt].Attributes["tlhw"].Value;
                        if (cbaseDataNodes[compNodeCnt].Attributes["action"] != null)
                            actionn = cbaseDataNodes[compNodeCnt].Attributes["action"].Value;
                        if (cbaseDataNodes[compNodeCnt].Attributes["task"] != null)
                            taskk = cbaseDataNodes[compNodeCnt].Attributes["task"].Value;
                    }
                    //If there is a button with empty task and empty action no need to add button
                    if (!(string.IsNullOrEmpty(tlhw)) && (!(string.IsNullOrEmpty(actionn)) || !(string.IsNullOrEmpty(taskk))) && taskk != "printersetting")
                    {
                        arrLeft = tlhw.Split(',');
                        if (arrLeft.Length > 0)
                        {
                            arrBtnLeftVals.Add(arrLeft[1]);
                        }
                    }
                }

            }
        }
        arrBtnLeftVals.Sort(new Util.CustomComparer());
    }

    public void MessageBox(string Msg)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script>showAlertDialog('info','" + Msg + "');</script>");
    }

    protected void LinkButton_Click(object sender, EventArgs e)
    {
        if (hdnIsPostBack.Value != "false")
            ActionClick(((LinkButton)sender).ID);
    }

    private string GetRowXmlFromDS(DataRow dr)
    {
        StringBuilder rowXml = new StringBuilder();
        DataSet ds = new DataSet();

        ds.Tables.Add(dr.Table.Clone());
        ds.Tables[0].ImportRow(dr);
        ds.Tables[0].TableName = "row";
        //Not using DataTable.WriteXml as empty columns were escaped by the method, hence looping through all the nodes.
        rowXml.Append("<row>");
        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
        {
            string colName = ds.Tables[0].Columns[i].ColumnName;
            string colVal = dr[colName].ToString().Replace("&nbsp;", " ");
            colVal = util.CheckSpecialChars(colVal);
            rowXml.Append("<" + colName + ">" + colVal + "</" + colName + ">");
        }
        rowXml.Append("</row>");
        return rowXml.ToString();
    }

    private void ActionClick(string btnId, bool isScript = false)
    {
        btnId = btnId.ToString();
        string actName = btnId.Substring(4);
        XmlDocument xmlDoc = new XmlDocument();
        bool xmlEmpty = false;
        try
        {
            {
                if (objIview.ResultXml != "")
                    xmlDoc.LoadXml(objIview.ResultXml);
                else
                    return;
            }

        }
        catch (Exception ex)
        {
            xmlEmpty = true;
        }
        string xml = string.Empty;

        if (!xmlEmpty)
        {
            string[] rowArys = hdnSRows.Value.Split('♣');

            for (int i = 0; i < rowArys.Length - 1; i++)
            {
                int row = Convert.ToInt32(rowArys[i].ToString());
                xml = xml + xmlDoc.SelectNodes("//row")[row].OuterXml;
            }
        }
        string iXml = string.Empty;
        string actFileName = "Action-" + actName + "-" + iName;
        errLog = logobj.CreateLog("Call to RemoteDoAction Web Service", sid, actFileName, string.Empty);

        iXml = "<root " + objIview.purposeString + " axpapp =\"" + proj + "\" trace =\"" + errLog + "\" sessionid =\"" + sid + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' stype=\"iviews\" sname=\"" + iName + "\" actname=\"" + actName + "\" ><params>";
        ConstructParamXml();
        iXml = iXml + pXml + "</params><varlist>" + xml + "</varlist>";
        iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
        string ires = string.Empty;
        Session["currentPageNo" + iName] = objIview.CurrentPageNo;
        //Call service
        if (iXml != string.Empty)
            iXml = iXml.Replace("<root", "<root scriptpath='" + ConfigurationManager.AppSettings["ScriptsPath"].ToString() + "'");

        if (isScript)
            ires = objWebServiceExt.callRemoteDoScriptWS(iName, iXml, ires, objIview.WebServiceTimeout);
        else
            ires = objWebServiceExt.CallRemoteDoActionWS(iName, iXml, ires, objIview.WebServiceTimeout);

        if (ires != null)
            ires = ires.Split('♠')[1];
        ires = ires.Replace("'", ";quot");
        ires = ires.Replace("\\", ";bkslh");
        ires = ires.Replace("\n", "<br>");
        if (navigationInfo.Rows.Count > 0)
            Session["iNavigationInfoTable"] = navigationInfo;
        if (objIview.ActBtnNavigation != null && objIview.ActBtnNavigation.ContainsKey(actName))
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "myActionTest", "<script>CallAssignLoadVals('" + ires + "', \"Iview\",'" + actName + "','" + objIview.ActBtnNavigation[actName].ToLower() + "');</script>", false);
        else
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "myActionTest", "<script>CallAssignLoadVals('" + ires + "', \"Iview\",'" + actName + "');</script>", false);
    }

    private void GetGlobalVariables()
    {
        if (Application["ValidateIviewParamOnGo"] != null)
            validateParamOnGo = Convert.ToBoolean(Application["ValidateIviewParamOnGo"].ToString());
        else
            validateParamOnGo = false;


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

        string replaceWith = "\\";
        strGlobalVar = util.GetGlobalVarString().Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
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

        if (Session["AxSplit"] != null && Session["AxSplit"].ToString() == "true")
        {
            AxSplit = true;
            Session.Remove("AxSplit");
        }
        if (Request.QueryString["AxSplit"] != null && Request.QueryString["AxSplit"] == "true")
        {
            AxSplit = true;
        }

        if (Session["AxFromHypLink"] != null)
            objIview.FromHyperLink = Session["AxFromHypLink"].ToString();
        CheckCustomIview();

        try
        {
            if (Request.QueryString["isDupTab"] != null || Request.Form["isDupTab"] != null)
            {
                Session["isDupTab"] = Request.Form["isDupTab"] == null ? Request.QueryString["isDupTab"].ToString() : Request.Form["isDupTab"];
            }
            else
            {
                Session["isDupTab"] = "false";
            }
        }
        catch (Exception ex) { }

        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();

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


    private void CheckCustomIview()
    {
        if (Session["AxCustomIviews"] != null)
        {
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

    private void SetGlobalVariables()
    {
        if (objIview.IName != null)
            iName = objIview.IName.ToString();
        else
            iName = string.Empty;

        if (objIview.Tid != null)
            tid = objIview.Tid.ToString();
        else
            tid = string.Empty;

        AxRole = Session["AxRole"].ToString();
        AxRole = util.CheckSpecialChars(AxRole);
        proj = Session["project"].ToString();
        proj = util.CheckSpecialChars(proj);
        sid = Session["nsessionid"].ToString();
        sid = util.CheckSpecialChars(sid);
        user = Session["user"].ToString();
        user = util.CheckSpecialChars(user);

        cac_order = sid + "order";
        cac_pivot = sid + iName + "pivot";

        if (clientCulture == null)
        {
            clientCulture = Convert.ToString(Session["ClientLocale"]);
            if (string.IsNullOrEmpty(clientCulture))
                clientCulture = "en-gb";
        }

        if (objIview.IviewCaption != null)
            lblHeading = objIview.IviewCaption.ToString();

        if (objIview.ToolbarHtml != null)
            defaultBut = objIview.ToolbarHtml;

        if (objIview.ToolBarBtn != null && hdnAct.Value == string.Empty)
        {
            ConstructActionBtns();
        }
        if (HttpContext.Current.Session["AxDbPagination"] != null && HttpContext.Current.Session["AxDbPagination"].ToString() == "")
            IsSqlPagination = "true";
        else if (HttpContext.Current.Session["AxDbPagination"] != null && HttpContext.Current.Session["AxDbPagination"].ToString() != "")
            IsSqlPagination = Session["AxDbPagination"].ToString().ToLower();


        if (objIview.ShowHiddengridCols != null)
            showHiddenCols = (ArrayList)objIview.ShowHiddengridCols;
        if (objIview.ShowHideCols != null)
            arrHdnColMyViews = (ArrayList)objIview.ShowHideCols;
    }

    private void GetBreadCrumb()
    {
        try
        {
            objIview.Menubreadcrumb = string.Empty;
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
            string code = (Request.QueryString["tstcaption"] != null ? "tstruct.aspx?transid=" + iName : "iview.aspx?ivname=" + iName);
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
                        s1 = parnode.Attributes["name"].Value + " / " + s1;
                        parnode = parnode.ParentNode;
                    }
                }

                objIview.Menubreadcrumb = s1;
            }
            else
            {
                objIview.Menubreadcrumb = string.Empty;

            }
        }
        catch (Exception)
        {
            SessExpires();
            return;
        }
    }

    private void ConstructBreadCrumb()
    {
        string direction1 = "start";
        if (language.ToLower() == "arabic")
            direction1 = "end";
        if (language.ToLower() == "arabic")
            strBreadCrumbBtns = "<div id='backforwrdbuttons' class='d-none d-flex backbutton " + direction1 + " '><span class='navLeft icon-arrows-left-double-32 handCur' onclick='javascript:BackForwardButtonClicked(\"back\");' id='" + "goback" + "' title=\"Click here to go back\" ></span></div>";
        else
            strBreadCrumbBtns = "<div id='backforwrdbuttons' class='d-none d-flex backbutton " + direction1 + " '><span class='navLeft icon-arrows-left-double-32 handCur' onclick='javascript:BackForwardButtonClicked(\"back\");' id='" + "goback" + "' title=\"Click here to go back\" ></span></div>";

        if (!(string.IsNullOrEmpty(objIview.IviewCaption.ToString())))
        {
            breadCrumb = objIview.IviewCaption.ToString();
        }
        else
        {
            breadCrumb = string.Empty;
        }

        strBreadCrumb.Append("<h1 class=\"text-dark fw-boldest my-1 fs-2 page-caption\">" + breadCrumb + "</h1>");
    }

    private void ConstructParamsHtml(string result)
    {
        result = _xmlString + result;
        logobj.CreateLog("Loading and setting parameters components", sid, fileName, string.Empty);
        string parameterName = string.Empty;
        string paramCaption = string.Empty;
        string paramType = string.Empty;
        string paramHidden = string.Empty;
        string paramMOE = string.Empty;
        string paramValue = string.Empty;
        string paramSql = string.Empty;
        string paramDepStr = string.Empty;
        string columnDepStr = string.Empty;
        Boolean unHideParams = false;
        string expr = string.Empty;
        bool exprSuggestion = false;
        string vExpr = string.Empty;
        string decPts = string.Empty;

        XmlDocument xmlDoc = new XmlDocument();
        XmlNodeList productNodes = default(XmlNodeList);
        XmlNodeList baseDataNodes = default(XmlNodeList);
        int iCnt = 0;
        int fldNo = 0;
        int dpCnt = 0;
        bool paramsBound = true;

        try
        {
            xmlDoc.LoadXml(result);
        }
        catch (XmlException ex)
        {
            requestProcess_logtime += "Server - " + ex.Message + " ♦ ";
            Response.Redirect(util.ERRPATH + ex.Message + "*♠*" + requestProcess_logtime);
        }

        if (xmlDoc.DocumentElement.Attributes["cvalue"] != null)
        {
            string cvalue = xmlDoc.DocumentElement.Attributes["cvalue"].Value;
            if (cvalue != "")
                ViewState["ivGroupBtns"] = cvalue;
            else
                ViewState["ivGroupBtns"] = null;
        }

        if (!objIview.isObjFromCache)
        {

            productNodes = xmlDoc.SelectNodes("//root");


            if (productNodes[0].Attributes["showparams"] != null && productNodes[0].Attributes["showparams"].Value != string.Empty)
                objParams.showParam = Convert.ToBoolean(productNodes[0].Attributes["showparams"].Value);
            if (productNodes[0].ChildNodes.Count > 0)
            {
                paramsExist = true;
                objParams.ParamsExist = true;
                GridView2Wrapper.Visible = false;
            }
            else
            {
                paramsExist = false;
                objParams.ParamsExist = false;
                iviewFrame.Style.Add("display", "block");
                paramCont.Style.Add("display", "none");
                GridView2Wrapper.Visible = true;
            }
            strJsArrays.Append("<script type='text/javascript'>");

            ivCaption = xmlDoc.SelectSingleNode("root").Attributes["caption"].Value;
            ivCaption = ivCaption.Replace("&&", "&");
            lblHeading = ivCaption;
            if (Request.QueryString["tstcaption"] == null)
            {
                objIview.IviewCaption = ivCaption;
            }

            foreach (XmlNode productNode in productNodes)
            {
                baseDataNodes = productNode.ChildNodes;
                paramHtml.Append("<table id=\"ivParamTable\" class=\"table table-borderless gy-0\"><tr>");
                string _isParamClearCache = "false";
                foreach (XmlNode baseDataNode in baseDataNodes)
                {
                    exprSuggestion = false;
                    string dynamicFilterString = string.Empty;
                    if (baseDataNode.Attributes["cat"].Value == "params")
                    {
                        paramValue = string.Empty;
                        if (baseDataNode.Attributes["value"] != null)
                            paramValue = baseDataNode.Attributes["value"].Value;

                        iviewParamValues.Add(paramValue);

                        foreach (XmlNode tstNode in baseDataNode)
                        {
                            if (tstNode.Name == "a0")
                            {
                                parameterName = tstNode.InnerText;
                                iviewParams.Add(parameterName);
                                UpdateParamValues(parameterName, paramValue);
                                if (parameterName != "" && parameterName == "axpResp")
                                {
                                    axpResp = paramValue == "true" || paramValue == "false" ? paramValue : "true";
                                }
                            }
                            else if (tstNode.Name == "a2")
                            {
                                paramCaption = tstNode.InnerText;
                                strParamDetails.Append(paramCaption + ",");
                                if (paramCaption.ToLower() == "axp_refresh")
                                    objParams.Axp_refresh = "true";

                                objParams.ParamCaption.Add(paramCaption);
                            }
                            else if (tstNode.Name == "a4")
                            {
                                paramType = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a5")
                            {
                                if (!string.IsNullOrEmpty(paramType) && paramType.ToLower() == "numeric")
                                {
                                    decPts = tstNode.InnerText;
                                }
                            }
                            else if (tstNode.Name == "a6")
                            {
                                expr = tstNode.InnerText;

                                if (expr.ToLower().IndexOf("date()") > -1)
                                {
                                    objParams.ForceDisableCache = true;
                                }

                                try
                                {
                                    exprSuggestion = Convert.ToBoolean(tstNode.Attributes["sugg"].Value);
                                }
                                catch (Exception ex) { }
                            }
                            else if (tstNode.Name == "a10")
                            {
                                vExpr = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a21")
                            {
                                paramHidden = tstNode.InnerText;
                                strParamDetails.Append(paramHidden + "~");
                                if (paramHidden == "true")
                                {
                                    objParams.ParamCaption.RemoveAt(objParams.ParamCaption.Count - 1);
                                    objParams.ParamCaption.Add("");
                                }
                                if (paramHidden != "true" && objParams.NoVisibleParam == true)
                                {
                                    objParams.NoVisibleParam = false;
                                }
                                arrVisibleParamList.Add(paramHidden);
                            }
                            else if (tstNode.Name == "a13")
                            {
                                paramMOE = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a56")
                            {
                                //pvalue = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a11")
                            {
                                paramSql = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a15")
                            {
                                foreach (XmlNode selNode in tstNode)
                                {
                                    if (selNode.Name == "s")
                                    {
                                        paramDepStr = selNode.InnerText.ToString();

                                        if (paramDepStr != string.Empty)
                                        {
                                            objParams.ForceDisableCache = true;
                                        }
                                    }
                                }
                                try
                                {
                                    isSelectWithMultiColumn = baseDataNode["response"].ChildNodes[0].ChildNodes.Count > 1;



                                    ArrayList parList = new ArrayList();

                                    int ind = -1;
                                    foreach (XmlNode node in baseDataNode["response"].ChildNodes[0].ChildNodes)
                                    {
                                        ind++;
                                        if (ind > 0)
                                        {
                                            parList.Add(node.Name);
                                        }
                                    }

                                    columnDepStr = String.Join(",", parList.ToArray());

                                }
                                catch (Exception ex) { }

                                isSelectParamsString = util.getSqlParameters(paramSql);

                                if (isSelectParamsString != string.Empty)
                                {
                                    objParams.ForceDisableCache = true;
                                }
                            }
                            else if (tstNode.Name.ToLower() == "a16")
                            {
                                foreach (XmlNode node in tstNode)
                                {
                                    string dfTemp = string.Empty;
                                    for (int i = 0; i < node.Attributes.Count; i++)
                                        dfTemp += node.Attributes[i].Value + "~";

                                    if (dfTemp.EndsWith("~"))
                                        dfTemp = dfTemp.Substring(0, dfTemp.Length - 1);

                                    dfTemp += "|";
                                    dynamicFilterString += dfTemp;

                                }
                                if (dynamicFilterString.EndsWith("|"))
                                {
                                    dynamicFilterString = dynamicFilterString.Substring(0, dynamicFilterString.Length - 1);
                                    paramHtml.Append("<script>var dfval" + parameterName + "= '" + dynamicFilterString + "' </script>");
                                }
                            }
                            else if (tstNode.Name == "a68")
                            {
                                _isParamClearCache = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "response")
                            {
                                ComboFill(tstNode, parameterName);
                                isSqlFld = true;
                            }

                        }
                        objParams.ParamNameType.Add(parameterName + "♣" + paramType);
                        if (paramMOE == "Accept" && paramSql != "")
                            objParams.isIviewParamCache = false;
                    }

                    if (paramType == "Date/Time" && clientCulture.ToLower() == "en-us")
                    {
                        paramValue = util.GetClientDateString(clientCulture, paramValue);

                        if (paramMOE.ToLower() == "select" && arrFillList.Count > 0)
                        {
                            for (int y = 0; y <= arrFillList.Count - 1; y++)
                            {
                                arrFillList[y] = util.GetClientDateString(clientCulture, arrFillList[y].ToString());
                            }
                        }
                    }

                    //construction of parameters.
                    paramValue = util.CheckSpecialChars(paramValue);
                    strJsArrays.Append("parentArr[" + fldNo + "]='" + parameterName + "';typeArr[" + fldNo + "]='" + paramMOE + "';paramType[" + fldNo + "]='" + paramType + "';depArr[" + fldNo + "]='" + paramDepStr + "';hiddenArr[" + fldNo + "]='" + paramHidden + "';Expressions[" + fldNo + "]='" + expr.Replace("'", "\\'") + "';exprSuggestions[" + fldNo + "]=" + exprSuggestion.ToString().ToLower() + ";pCurArr[" + fldNo + "]= '" + paramValue + "';vExpressions[" + fldNo + "]='" + vExpr + "';sqlParamsArr[" + fldNo + "]='" + isSelectParamsString + "';columnDepArr[" + fldNo + "]='" + columnDepStr + "'; ");
                    paramHidden = paramHidden.ToLower();
                    if (paramHidden == "false")
                    {
                        objParams.ArrParamType.Add(paramType);
                        objParams.IsParameterExist = true;
                        IsParamsVisible = true;
                        if (paramsBound && paramValue == string.Empty)
                        {
                            paramsBound = false;
                        }

                        unHideParams = true;
                        string CallValidateExpr = string.Empty;
                        string CallValidateExprDate = string.Empty;
                        string onlyTime = String.Empty;
                        if (parameterName.StartsWith("axptm_") || parameterName.StartsWith("axpdbtm_"))
                        {
                            onlyTime = " onlyTime ";
                        }

                        string acceptNumericCorrection = "valdDecPts(this," + decPts + ");";
                        string hideMobKeyBoard = (isMobile ? " onFocus=\"blur();\" " : "");
                        if (!string.IsNullOrEmpty(vExpr))
                        {
                            if (paramMOE.ToLower() == "accept")
                            {
                                if (paramType == "Date/Time")
                                {
                                    CallValidateExpr = "onChange=\"ValidateVexpr('" + parameterName + "','" + vExpr + "');FillDependents('\" + parameterName + \"');\" ";
                                    //CallValidateExpr = "onBlur=\"FillDependents('" + parameterName + "');ValidateVexpr('" + parameterName + "','" + vExpr + "');\" ";
                                    CallValidateExprDate = "data-onVexpr=\"" + vExpr + "\" ";
                                }
                                else
                                    CallValidateExpr = "onBlur=\"" + (paramType.ToLower() == "numeric" ? acceptNumericCorrection : "") + "FillDependents('" + parameterName + "');ValidateVexpr('" + parameterName + "','" + vExpr + "');\" ";

                            }
                            else if ((paramMOE.ToLower() == "select") | (paramMOE.ToLower() == "pick list"))
                            {
                                CallValidateExpr = "onChange=\"FillDependents('" + parameterName + "');ValidateVexpr('" + parameterName + "','" + vExpr + "');\" ";
                            }
                        }
                        else
                        {
                            if (paramMOE.ToLower() == "accept")
                            {
                                CallValidateExpr = "onBlur=\"" + (paramType.ToLower() == "numeric" ? acceptNumericCorrection : "") + "FillDependents('" + parameterName + "');\" ";
                            }
                            else if ((paramMOE.ToLower() == "select") | (paramMOE.ToLower() == "pick list"))
                            {
                                CallValidateExpr = "onChange=\"FillDependents('" + parameterName + "');\" ";
                            }
                            else if (paramMOE.ToLower() == "multi select")
                            {
                                CallValidateExpr = " ";
                            }
                        }



                        if ((paramMOE.ToLower() == "accept") & (paramType == "Date/Time") & (!string.IsNullOrEmpty(expr) || !string.IsNullOrEmpty(vExpr)))
                        {
                            if (expr.ToLower() != "date()")
                            {
                                paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group paramtd2'><input type='text' id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "' class='date form-control' " + CallValidateExprDate + " data-input onfocus=\"ExprHandler(this.name,this.value, true);" + (isMobile ? "blur();" : "") + "\"><span class='input-group-text' data-toggle><span class='material-icons material-icons-style cursor-pointer fs-4'>calendar_today</span></span></div></div></td>");

                            }
                            else
                            {
                                paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group paramtd2'><input type='text' id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "' class='date form-control'" + CallValidateExpr + "" + hideMobKeyBoard + " data-input><span class='input-group-text' data-toggle><span class='material-icons material-icons-style cursor-pointer fs-4'>calendar_today</span></span></div></div></td>");

                            }
                        }
                        else if ((paramMOE.ToLower() == "accept") & (paramType == "Date/Time") & (string.IsNullOrEmpty(expr)))
                        {
                            if (paramDepStr != string.Empty)
                            {
                                paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group paramtd2'><input type='text' id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "' class='date form-control'" + CallValidateExpr + "" + hideMobKeyBoard + " data-input><span class='input-group-text' data-toggle><span class='material-icons material-icons-style cursor-pointer fs-4'>calendar_today</span></span></div></div></td>");

                            }
                            else
                            {
                                paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group paramtd2'><input type='text' id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "' class='date form-control'" + CallValidateExpr + "" + hideMobKeyBoard + " data-input><span class='input-group-text' data-toggle><span class='material-icons material-icons-style cursor-pointer fs-4'>calendar_today</span></span></div></div></td>");

                            }
                        }
                        else if ((paramMOE.ToLower() == "accept") & (!string.IsNullOrEmpty(expr)))
                        {
                            if (paramType.ToLower() == "numeric")
                            {
                                paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group paramtd2'><input type='text' id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "' class='" + onlyTime + "form-control ' onfocus=\"ExprHandler(this.name,this.value, false);" + (isMobile ? "blur();" : "") + "\" " + CallValidateExpr + " " + (onlyTime != string.Empty ? "data-input" : "") + ">" + (onlyTime != string.Empty ? "<span class='input-group-text' data-toggle><span class='material-icons material-icons-style cursor-pointer fs-4'>schedule</span></span>" : "") + "</div></div></td>");
                            }
                            else
                            {
                                paramHtml.Append("<td><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group paramtd2'><input type='text' id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "' class='" + onlyTime + "form-control ' onfocus=\"ExprHandler(this.name,this.value, false);" + (isMobile ? "blur();" : "") + "\" " + CallValidateExpr + " " + (onlyTime != string.Empty ? "data-input" : "") + ">" + (onlyTime != string.Empty ? "<span class='input-group-text' data-toggle><span class='material-icons material-icons-style cursor-pointer fs-4'>schedule</span></span>" : "") + "</div></div></td>");
                            }

                        }
                        else if ((paramMOE.ToLower() == "accept"))
                        {
                            if (paramType.ToLower() == "numeric")
                            {
                                paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group paramtd2'><input type='text' id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "' class='" + onlyTime + "form-control ' " + CallValidateExpr + " " + (onlyTime != string.Empty ? "data-input" : "") + ">" + (onlyTime != string.Empty ? "<span class='input-group-text' data-toggle><span class='material-icons material-icons-style cursor-pointer fs-4'>schedule</span></span>" : "") + "</div></div></td>");
                            }
                            else
                            {
                                paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group paramtd2'><input type='text' id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "' class='" + onlyTime + " form-control '" + CallValidateExpr + " " + (onlyTime != string.Empty ? "data-input" : "") + ">" + (onlyTime != string.Empty ? "<span class='input-group-text' data-toggle><span class='material-icons material-icons-style cursor-pointer fs-4'>schedule</span></span>" : "") + "</div></div></td>");
                            }

                        }
                        else if ((paramMOE.ToLower() == "pick list"))
                        {
                            paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group pick-list paramtd2 flex-nowrap'><select type='text' id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "' class='form-select trySelectPl h-40px' " + CallValidateExpr + ">" + (paramValue != string.Empty ? ("<option selected value=\"" + paramValue + "\" " + paramValue + " >" + paramValue + "</option>") : "") + "</select><div data-id='" + parameterName + "' class='btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm px-6 h-40px border border-gray-400 input-group-text rounded-0 pickListddlIcon' tabindex='0' title='Pick List'><span class='material-icons'>manage_search</span></div></div></td>");

                            arrFillList.Clear();
                            arrFillListDataAttr.Clear();
                            isSelectWithMultiColumn = false;
                            isSelectParamsString = string.Empty;
                        }
                        else if ((paramMOE.ToLower() == "select") & isSqlFld == true & dynamicFilterString != string.Empty)
                        {
                            paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "<span class=\"required ivparamselect\"></span></label><div class='input-group paramtd2 flex-nowrap'><select id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' class='form-select trySelectDF h-40px'  onmouseover=\"showHideTooltip(this.id);\"  " + CallValidateExpr + ">");

                            int i = 0;
                            if (arrFillList.Count == 0)
                            {
                                paramHtml.Append("<option selected>" + Constants.EMPTYOPTION + "</option>");
                            }
                            for (i = 0; i <= arrFillList.Count - 1; i++)
                            {
                                if (i == 0)
                                {
                                    paramHtml.Append("<option value=\"\" " + arrFillListDataAttr[i] + " >" + Constants.EMPTYOPTION + "</option>");
                                }

                                if (arrFillList[i].ToString() == paramValue)
                                {
                                    paramHtml.Append("<option selected value=\"" + arrFillList[i].ToString() + "\" " + arrFillListDataAttr[i] + " >" + arrFillList[i].ToString() + "</option>");
                                }
                                else
                                {
                                    paramHtml.Append("<option value=\"" + arrFillList[i].ToString() + "\" " + arrFillListDataAttr[i] + " >" + arrFillList[i].ToString() + "</option>");
                                }
                            }
                            if (paramValue != "" && arrFillList.IndexOf(paramValue) == -1)
                            {
                                paramsBound = false;
                            }
                            paramHtml.Append("</select>");
                            paramHtml.Append("<div data-id='" + parameterName + "' class='btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm px-6 h-40px border border-gray-400 input-group-text rounded-0 selectDfIcon' tabindex='0' title='Select'><span class='material-icons'>search</span></div>");
                            paramHtml.Append("</div></div></td>");
                            arrFillList.Clear();
                            arrFillListDataAttr.Clear();
                            isSelectWithMultiColumn = false;
                            isSelectParamsString = string.Empty;
                            isSqlFld = false;
                        }
                        else if (paramMOE.ToLower() == "select" & isSqlFld == false & dynamicFilterString != string.Empty)
                        {
                            paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "<span class=\"required ivparamselect\"></span></label><div class='input-group paramtd2 flex-nowrap'><select id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' class='form-select trySelectDF h-40px' " + CallValidateExpr + " ></select>");
                            paramHtml.Append("<div data-id='" + parameterName + "' class='btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm px-6 h-40px border border-gray-400 input-group-text rounded-0 selectDfIcon' tabindex='0' title='Select'><span class='material-icons'>search</span></div>");
                            paramHtml.Append("</div></div></td>");
                        }
                        else if ((paramMOE.ToLower() == "select") & isSqlFld == true)
                        {
                            paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "<span class=\"required ivparamselect\"></span></label><div class='input-group paramtd2'><select id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' class='form-select trySelect'  onmouseover=\"showHideTooltip(this.id);\"  " + CallValidateExpr + ">");

                            int i = 0;
                            if (arrFillList.Count == 0)
                            {
                                paramHtml.Append("<option selected>" + Constants.EMPTYOPTION + "</option>");
                            }
                            for (i = 0; i <= arrFillList.Count - 1; i++)
                            {
                                if (i == 0)
                                {
                                    paramHtml.Append("<option value=\"\" " + arrFillListDataAttr[i] + " >" + Constants.EMPTYOPTION + "</option>");
                                }

                                if (arrFillList[i].ToString() == paramValue)
                                {
                                    paramHtml.Append("<option selected value=\"" + arrFillList[i].ToString() + "\" " + arrFillListDataAttr[i] + " >" + arrFillList[i].ToString() + "</option>");
                                }
                                else
                                {
                                    paramHtml.Append("<option value=\"" + arrFillList[i].ToString() + "\" " + arrFillListDataAttr[i] + " >" + arrFillList[i].ToString() + "</option>");
                                }
                            }
                            if (paramValue != "" && arrFillList.IndexOf(paramValue) == -1)
                            {
                                paramsBound = false;
                            }
                            paramHtml.Append("</select></div></div></td>");
                            arrFillList.Clear();
                            arrFillListDataAttr.Clear();
                            isSelectWithMultiColumn = false;
                            isSelectParamsString = string.Empty;
                            isSqlFld = false;
                        }
                        else if (paramMOE.ToLower() == "select" & isSqlFld == false)
                        {
                            paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "<span class=\"required ivparamselect\"></span></label><div class='input-group paramtd2'><select id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' class='form-select trySelect' " + CallValidateExpr + " ></select></div></div></td>");
                        }
                        else if (((paramMOE.ToLower() == "multi select")))
                        {
                            if (_isParamClearCache != string.Empty && _isParamClearCache.ToLower() == "true")
                                objParams.isIviewParamClearCache = true;
                            paramHtml.Append("<td class='d-block d-sm-table-cell col-12 col-sm-4'><div class='agform pb-2 '><label for='" + parameterName + "' class='form-label col-form-label pb-1 fw-boldest paramtd1 cap '>" + paramCaption + "</label><div class='input-group multi-select paramtd2'>");

                            if (!objIview.requestJSON)
                            {

                                paramHtml.Append("<div style=\"\"  name=\"" + parameterName + "\" data-caption=\"" + paramCaption + "\"  class=\"spnSelectAll\"><input type='checkbox' id='"
                                        + parameterName + "' data-caption=\"" + paramCaption + "\" onclick=\"GetAllChecked(this);\" class=\"chkAllList chkSelectAll\" "
                                        + string.Empty + "/>Select All</div><div id=\"" + parameterName + "\" class=\"paramtd2 chkListBdr\">");

                                int i = 0;
                                for (i = 0; i <= arrFillList.Count - 1; i++)
                                {

                                    if (arrFillList[i].ToString() == paramValue)
                                    {
                                        paramHtml.Append("<span><input type=\"checkbox\" onclick=\"UncheckChkAll(this);\" checked='true' id=\"" + parameterName + "\" data-caption=\"" + paramCaption + "\" class=\" chkAllList chkShwSel\" value=\"" + arrFillList[i].ToString() + "\"/>" + arrFillList[i].ToString() + "</span><br />");

                                    }
                                    else
                                    {
                                        paramHtml.Append("<span><input type=\"checkbox\" id=\"" + parameterName + "\" data-caption=\"" + paramCaption + "\" class=\" chkAllList chkShwSel\" onclick=\"UncheckChkAll(this);\" value=\"" + arrFillList[i].ToString() + "\"/>" + arrFillList[i].ToString() + "</span><br />");

                                    }

                                }
                                paramHtml.Append("</div>");

                            }
                            else
                            {
                                string separator = "&grave;";
                                paramHtml.Append("<select class=\"form-select trySelectMs\" multiple=\"multiple\" name=\"" + parameterName + "\" id='" + parameterName + "' data-caption=\"" + paramCaption + "\" data-valuelist=\"" + string.Join(separator.ToString(), arrFillList.ToArray().Where(x => !string.IsNullOrEmpty(x.ToString())).ToArray()) + "\"  data-selectedlist=\"" + paramValue + "\"  data-separator=\"" + separator.ToString() + "\"  " + CallValidateExpr + " />");
                                if (paramValue != "")
                                {
                                    bool isbound = false;
                                    string[] _defaultParamVal = paramValue.Split(',');
                                    for (int z = 0; z <= arrFillList.Count - 1; z++)
                                    {
                                        bool _exists = Array.Exists(_defaultParamVal, element => element == arrFillList[z].ToString());
                                        if (_exists)
                                        {
                                            isbound = true;
                                            paramHtml.Append("<option selected value=\"" + arrFillList[z].ToString() + "\" " + arrFillListDataAttr[z] + " >" + arrFillList[z].ToString() + "</option>");
                                        }
                                    }
                                    if (paramValue != "" && !isbound)
                                    {
                                        paramsBound = false;
                                    }
                                }
                                paramHtml.Append("</select>");
                            }
                            paramHtml.Append("</div></div></td>");
                            arrFillList.Clear();
                            arrFillListDataAttr.Clear();
                            isSelectWithMultiColumn = false;
                            isSelectParamsString = string.Empty;
                            isSqlFld = false;
                        }


                        iCnt = iCnt + 1;
                        if (CallValidateExpr != string.Empty && paramDepStr != string.Empty)
                        {
                            strJsArrays.Append("depParamArr[" + dpCnt + "]='" + parameterName + "';");
                            dpCnt++;
                        }
                        CallValidateExpr = string.Empty;

                    }
                    else
                    {
                        objParams.ArrParamType.Add(paramType);
                        if (!string.IsNullOrEmpty(paramValue))
                        {
                            paramHtml.Append("<input type=hidden id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value='" + paramValue + "'/>");
                        }
                        else
                        {
                            paramHtml.Append("<input type=hidden id='" + parameterName + "' name='" + parameterName + "' data-caption='" + paramCaption + "' value=''/>");
                        }
                        arrFillList.Clear();
                        arrFillListDataAttr.Clear();
                        isSelectWithMultiColumn = false;
                        isSelectParamsString = string.Empty;

                    }
                    fldNo = fldNo + 1;
                    paramDepStr = string.Empty;
                    columnDepStr = string.Empty;
                    if (iCnt == 3)
                    {
                        iCnt = 0;
                        paramHtml.Append("</tr><tr>");
                    }

                }
            }
            paramHtml.Append("</div></tr></table>");

            strJsArrays.Append("</script>");

            objParams.ParamHtml = paramHtml;
            objParams.strJsArrays = strJsArrays.ToString();
            objParams.iviewParams = iviewParams;
            objParams.iviewParamValues = iviewParamValues;
            objParams.arrVisibleParamList = arrVisibleParamList;
            objParams.axpResp = axpResp;
            objParams.paramsBound = paramsBound;
        }
        else
        {
            paramHtml = objParams.ParamHtml != null ? objParams.ParamHtml : new StringBuilder();
            strJsArrays.Append(objParams.strJsArrays);
            iviewParams = objParams.iviewParams;
            iviewParamValues = objParams.iviewParamValues;
            arrVisibleParamList = objParams.arrVisibleParamList;
            axpResp = objParams.axpResp;
            axp_refresh = objParams.Axp_refresh;
            unHideParams = IsParamsVisible = objParams.IsParameterExist;
            paramsBound = objParams.paramsBound;
        }
        if (!objIview.requestJSON)
        {
            Session["paramDetails"] = strParamDetails.ToString();
        }

        if (objParams.ParamsExist == false || redisLoadKey != string.Empty)
        {
            GetIviewData();
        }
        else if (unHideParams == false && isCallWS == false)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script type=\"text/javascript\">document.getElementById('button1').click();</script>");
            iviewFrame.Style.Add("display", "block");
            paramCont.Style.Add("display", "none");
            dotNetSubmit = true;
        }

        JObject resultData = new JObject();
        if (objIview.DsIviewConfig != null && (objIview.DsIviewConfig.Rows.Count > 0))
        {

            JObject jObj = null;
            jObj = generateConfigurationJson(objIview.DsIviewConfig);
            if (jObj != null)
            {
                resultData.Add("configurations", jObj);
            }
        }
        tst_Scripts = tst_Scripts + "<script type=\"text/javascript\">var hideParams='" + hideParameters + "';var proj = '" + proj + "';var user='" + user + "';var AxRole='" + AxRole + "'; var sid='" + sid + "';var iName='" + iName + "';var gl_language='" + language + "'; validateParamOnGo=" + validateParamOnGo.ToString().ToLower() + "; " + strGlobalVar + "var axpResp='" + axpResp + "';var axp_refresh = '" + objParams.Axp_refresh + "';var globalIvConfigurations = " + resultData.ToString(Newtonsoft.Json.Formatting.None) + ";</script>";
        logobj.CreateLog("Loading and setting parameters components completed", sid, fileName, string.Empty);
        if (paramsBound == true && unHideParams == true && isCallWS == false)
        {

            if (redisLoadKey == string.Empty)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "FillDependentsStartupPlus", "<script type=\"text/javascript\">AxWaitCursor(true);ShowDimmer(true);GetUserLocale(); FillDependentsStartup(true);</script>");
            }
        }
        else
        {
            if (objParams.ParamsExist)
            {
                dvRowsPerPage.Style.Add("display", "none");
            }
            if (redisLoadKey == string.Empty)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "FillDependentsStartup", "<script type=\"text/javascript\">FillDependentsStartup(false);</script>");
            }
        }
    }

    private string GetParamType(string paramName)
    {
        foreach (string strParam in objParams.ParamNameType)
        {
            string[] arrParam = strParam.Split('♣');
            if (arrParam.Length == 2 && arrParam[0].ToString().ToLower() == paramName.ToLower())
                return arrParam[1].ToString();
        }
        return string.Empty;
    }

    private void ShowHideFilterDiv(bool showParam)
    {
        if (showParam)
        {
            hdnShowParams.Value = "true";
            paramCont.Style.Add("display", "block");

            Filterscollapse.Attributes.Add("class", Filterscollapse.Attributes["class"].ToString().Replace("collapse", "collapse in"));
        }
        else
        {
            hdnShowParams.Value = "false";
            hideParameters = "true";
            paramCont.Style.Add("display", "none");

            Filterscollapse.Attributes.Add("class", Filterscollapse.Attributes["class"].ToString().Replace("in", ""));
        }

    }


    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }




    #region IView Filter


    private void UpdateRecordsPerPage()
    {
        int recIdx = recPerPage.Items.IndexOf(new System.Web.UI.WebControls.ListItem(gridPageSize));
        if (recIdx == -1)
        {
            List<int> list = new List<int> { 50, 100, 200, 500 };
            int ivPageSize = Convert.ToInt32(gridPageSize);
            int closest = list.OrderBy(item => Math.Abs(ivPageSize - item)).First();
            if (closest > ivPageSize && list.IndexOf(closest) > 0)
                recIdx = list.IndexOf(closest) - 1;
            else if (closest > ivPageSize && list.IndexOf(closest) == 0)
                recIdx = 0;
            else
                recIdx = list.IndexOf(closest) + 1;
            recPerPage.Items.Insert(recIdx, new System.Web.UI.WebControls.ListItem(gridPageSize));
            recPerPage.SelectedIndex = recIdx;
        }
        else
        {
            recPerPage.SelectedIndex = recIdx;
        }
    }


    //myviews
    //get my views

    #endregion
    private void IncludeCustomLinks()
    {
        string projName = HttpContext.Current.Session["Project"].ToString();
        Custom customObj = Custom.Instance;
        for (int i = 0; i < customObj.jsIviewFiles.Count; i++)
        {
            string[] jsFileStr = customObj.jsIviewFiles[i].ToString().Split('¿');
            string iviewName = jsFileStr[0].ToString().ToLower();
            string fileName = jsFileStr[1].ToString();
            if (iName.ToLower() == iviewName)
            {
                HtmlGenericControl js = new HtmlGenericControl("script");
                js.Attributes["type"] = "text/javascript";
                string path = "../" + projName + "/" + fileName;
                js.Attributes["src"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }

        for (int j = 0; j < customObj.jsIviewGlobalFiles.Count; j++)
        {
            HtmlGenericControl js = new HtmlGenericControl("script");
            js.Attributes["type"] = "text/javascript";
            string path = "../" + projName + "/" + customObj.jsIviewGlobalFiles[j].ToString();
            js.Attributes["src"] = path;
            ScriptManager1.Controls.Add(js);
        }

        for (int i = 0; i < customObj.cssIviewFiles.Count; i++)
        {
            string[] jsFileStr = customObj.cssIviewFiles[i].ToString().Split('¿');
            string iviewName = jsFileStr[0].ToString().ToLower();
            string fileName = jsFileStr[1].ToString();
            if (iName.ToLower() == iviewName)
            {
                HtmlGenericControl js = new HtmlGenericControl("link");
                js.Attributes["type"] = "text/css";
                js.Attributes["rel"] = "stylesheet";
                string path = "../" + projName + "/" + fileName;
                js.Attributes["href"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }

        for (int i = 0; i < customObj.cssIviewGlobalFiles.Count; i++)
        {
            HtmlGenericControl js = new HtmlGenericControl("link");
            js.Attributes["type"] = "text/css";
            js.Attributes["rel"] = "stylesheet";
            string path = "../" + projName + "/" + customObj.cssIviewGlobalFiles[i].ToString();
            js.Attributes["href"] = path;
            ScriptManager1.Controls.Add(js);
        }
    }

    private void IncludeCustomLinksNew(IviewData objIview)
    {
        string projName = HttpContext.Current.Session["Project"].ToString();
        if (objIview.axpCustomJs != string.Empty && objIview.axpCustomJs.ToLower() == "single")
        {
            FileInfo filtcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/report/js/" + iName + ".js"));
            if (filtcustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("script");
                js.Attributes["type"] = "text/javascript";
                string path = "../" + projName + "/report/js/" + iName + ".js?v=" + filtcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["src"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }
        else if (objIview.axpCustomJs != string.Empty && objIview.axpCustomJs.ToLower() == "all")
        {
            FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/report/js/custom.js"));
            if (filcustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("script");
                js.Attributes["type"] = "text/javascript";
                string path = "../" + projName + "/report/js/custom.js?v=" + filcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["src"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }

        if (objIview.axpCustomCss != string.Empty && objIview.axpCustomCss.ToLower() == "single")
        {
            FileInfo filtcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/report/css/" + iName + ".css"));
            if (filtcsscustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("link");
                js.Attributes["type"] = "text/css";
                js.Attributes["rel"] = "stylesheet";
                string path = "../" + projName + "/report/css/" + iName + ".css?v=" + filtcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["href"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }
        else if (objIview.axpCustomCss != string.Empty && objIview.axpCustomCss.ToLower() == "all")
        {
            FileInfo filcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/report/css/custom.css"));
            if (filcsscustom.Exists)
            {
                HtmlGenericControl js = new HtmlGenericControl("link");
                js.Attributes["type"] = "text/css";
                js.Attributes["rel"] = "stylesheet";
                string path = "../" + projName + "/report/css/custom.css?v=" + filcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                js.Attributes["href"] = path;
                ScriptManager1.Controls.Add(js);
            }
        }

        if (objIview.axpCustomJs == string.Empty && objIview.axpCustomCss == string.Empty)
            IncludeCustomLinks();
    }

    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        if (Session["AxMergeRowIviews"] != null)
        {
            string mergeRowIviews = string.Empty;
            mergeRowIviews = Session["AxMergeRowIviews"].ToString();
            Boolean rowMerge = false;
            int mergeColumns = 0;
            if (!string.IsNullOrEmpty(mergeRowIviews))
            {
                string[] mIviews = mergeRowIviews.Split(',');
                for (int i = 0; i < mIviews.Length; i++)
                {
                    string mIview = mIviews[i].Substring(0, mIviews[i].IndexOf(':'));
                    if (iName == mIview)
                    {
                        rowMerge = true;
                        mergeColumns = Convert.ToInt32(mIviews[i].Substring(mIviews[i].IndexOf(':') + 1));
                        mergeColumns = mergeColumns + 1;
                        break;
                    }
                }
            }
            if (rowMerge)
            {
                ArrayList cellsWithValues = new ArrayList();
                for (int i = GridView1.Rows.Count - 1; i > 0; i--)
                {
                    GridViewRow row = GridView1.Rows[i];

                    if (i == GridView1.Rows.Count - 1)
                    {
                        for (int c = 0; c < row.Cells.Count; c++)
                        {
                            int sl;
                            if (!string.IsNullOrEmpty(row.Cells[c].Text) && !StringExtensions.IsNullOrWhiteSpace(row.Cells[c].Text))
                            {
                                if (c != 0 && int.TryParse(row.Cells[c].Text, out sl) != true)
                                    cellsWithValues.Add(c);
                            }
                        }
                    }
                    GridViewRow previousRow = GridView1.Rows[i - 1];
                    bool firstElement = false;
                    for (int n = 0; n < cellsWithValues.Count; n++)
                    {
                        string rowText = row.Cells[int.Parse(cellsWithValues[n].ToString())].Text;
                        string prevRowText = previousRow.Cells[int.Parse(cellsWithValues[n].ToString())].Text;
                        if (rowText.Equals(prevRowText) && rowText == prevRowText)
                        {
                            if (n == 0)
                                firstElement = true;

                            if (n == (cellsWithValues.Count - 1))
                                break;

                            string rowTextNext = row.Cells[int.Parse(cellsWithValues[n + 1].ToString())].Text;
                            string prevRowTextNext = previousRow.Cells[int.Parse(cellsWithValues[n + 1].ToString())].Text;

                            if (n == 0)
                                if (rowTextNext != prevRowTextNext && !rowTextNext.Equals(prevRowTextNext))
                                    break;

                            if (firstElement == true)
                            {
                                if (previousRow.Cells[int.Parse(cellsWithValues[n].ToString())].RowSpan == 0)
                                {
                                    if (row.Cells[int.Parse(cellsWithValues[n].ToString())].RowSpan == 0)
                                        previousRow.Cells[int.Parse(cellsWithValues[n].ToString())].RowSpan += 2;
                                    else
                                        previousRow.Cells[int.Parse(cellsWithValues[n].ToString())].RowSpan = row.Cells[int.Parse(cellsWithValues[n].ToString())].RowSpan + 1;

                                    row.Cells[int.Parse(cellsWithValues[n].ToString())].Visible = false;
                                    previousRow.Cells[int.Parse(cellsWithValues[n].ToString())].Style.Add("vertical-align", "middle");
                                }
                            }
                        }
                        else if (n == 0)
                            break;
                    }
                }
            }
        }
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetIviewPickListData(string iviewName, string pageNo, string pageSize, string fieldName, string fieldValue, string depParamVal)
    {
        Util.Util utilGlo = new Util.Util();
        string pickDepParams = string.Empty;
        try
        {
            if (depParamVal != "")
            {
                string[] pdParamStr = depParamVal.Split('¿');
                for (int i = 0; i < pdParamStr.Count(); i++)
                {
                    string pdNameVal = pdParamStr[i];
                    if (pdNameVal != "")
                    {
                        string[] pdNames = pdNameVal.Split('~');
                        pickDepParams = pickDepParams + "<" + pdNames[0] + ">";
                        pickDepParams = pickDepParams + utilGlo.CheckSpecialChars(pdNames[1]);
                        pickDepParams = pickDepParams + "</" + pdNames[0] + ">";
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        fieldValue = utilGlo.CheckSpecialChars(fieldValue);
        string iXml = string.Empty;
        string filename = "iviewpicklist-" + iviewName;
        LogFile.Log logobj = new LogFile.Log();
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        string errlog = logobj.CreateLog("", HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
        iXml = iXml + "<sqlresultset axpapp=\"" + HttpContext.Current.Session["project"] + "\" value=\"" + fieldValue + "\" sessionid= \"" + HttpContext.Current.Session["nsessionid"] + "\" pname =\"" + fieldName + "\" trace=\"" + errlog + "\" user=\"" + HttpContext.Current.Session["user"] + "\" ivname=\"" + iviewName + "\" pageno =\"" + pageNo + "\" pagesize=\"" + pageSize + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
        if (pickDepParams != string.Empty)
            iXml = iXml + "<varlist>" + pickDepParams + "</varlist>";
        iXml = iXml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
        iXml = iXml + "</sqlresultset>";

        string result = asbExt.CallGetParamChoicesWS(iviewName, iXml);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(result);

        string json = JsonConvert.SerializeXmlNode(doc);

        return json;
    }
    [WebMethod]
    public static string SaveJsonInDB(string jsonString, bool isListView)
    {
        string result = String.Empty;
        string sql = String.Empty;
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string fileName = "openiview-dev---";
        if (HttpContext.Current.Session["nsessionid"] == null)
            return Constants.ERAUTHENTICATION;
        string sid = HttpContext.Current.Session["nsessionid"].ToString();
        LogFile.Log logobj = new LogFile.Log();

        string fdSettingsKey = Constants.RedisOldIviewSettings;
        if (isListView)
        {
            fdSettingsKey = Constants.RedisOldListviewSettings;
        }

        try
        {
            sql = Constants.MYSQL_QUERY_IVIR_SAVE_JSON;
            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());
                sql = sql.Replace("$VALUE$", jsonString.Replace("&", "&amp;"));
            }
            result = objCWbSer.GetChoicesCustomWS("", sql);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Smartviews Save Exeception - " + ex.Message, sid, fileName, string.Empty);
        }

        if (result == "done")
        {
            try
            {
                Util.Util util = new Util.Util();
                string user = HttpContext.Current.Session["user"].ToString();
                FDW fdwObj = new FDW();
                fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdSettingsKey, "", user), jsonString, fdSettingsKey, HttpContext.Current.Session["dbuser"].ToString());
            }
            catch (Exception ex) { }
        }

        return result;
    }
    [WebMethod]
    public static object GetJsonFromDB(bool isListView)
    {
        string sqlResult = String.Empty;
        string result = string.Empty, status = string.Empty;
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string fileName = "openiview-dev---";
        if (HttpContext.Current.Session["nsessionid"] == null)
            return new { result = Constants.ERAUTHENTICATION, status = "failure" };
        string sid = HttpContext.Current.Session["nsessionid"].ToString();
        LogFile.Log logobj = new LogFile.Log();
        string sql = string.Empty;

        string fdSettingsKey = Constants.RedisOldIviewSettings;
        if (isListView)
        {
            fdSettingsKey = Constants.RedisOldListviewSettings;
        }

        try
        {
            Util.Util util = new Util.Util();

            string user = HttpContext.Current.Session["user"].ToString();

            try
            {

                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                {
                    fObj = new FDR();
                    HttpContext.Current.Session["FDR"] = fObj;
                }

                result = fObj.StringFromRedis(util.GetRedisServerkey(fdSettingsKey, "", user));
            }
            catch (Exception ex) { }

            if (result != string.Empty)
            {
                status = "success";
            }
            else
            {
                util.CheckUserSettings();
                sql = Constants.MYSQL_QUERY_IVIR_GET_JSON;
                if (!string.IsNullOrEmpty(sql))
                {
                    sql = sql.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());
                }
                sqlResult = objCWbSer.GetChoicesCustomWS("", sql);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sqlResult);
                XmlNodeList xml = doc.SelectNodes("//row//IR_CONFIG | //row//ir_config");
                if (xml.Count != 0)
                {
                    result = xml[0].InnerText;
                    status = "success";
                    try
                    {
                        FDW fdwObj = new FDW();
                        fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdSettingsKey, "", user), result, fdSettingsKey, HttpContext.Current.Session["dbuser"].ToString());
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    status = "failure";
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Smartviews Load Exception - " + ex.Message, sid, fileName, string.Empty);
            status = "failure";
        }
        return new { result = result, status = status };
    }

    public static string ConvertBytesToMegabytes(double size)
    {
        double sizeMB = Convert.ToDouble(size / 1024f / 1024f);
        sizeMB = Math.Round(sizeMB);
        return sizeMB.ToString();

    }

    [WebMethod]
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
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            chartdetails = (ArrayList)fdrObj.GetMemoryDetails();
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
            return "failure";
        }
    }

    [WebMethod]
    public static object ActionBtnClick(string iName, string ivkey, string iXml, string actName, bool isScript = false)
    {
        var objIview = (IviewData)HttpContext.Current.Session[ivkey];
        if (objIview == null)
            return new { result = Constants.ERAUTHENTICATION };
        string filename = "Action-" + iName;
        LogFile.Log logobj = new LogFile.Log();
        string errlog = logobj.CreateLog("", HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
        iXml = iXml.Replace("♦♦trace♦♦", errlog);
        iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
        //Call service
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        string ires = string.Empty;

        if (iName == "inmemdb")
        {
            string delKeyResp = DeleteRedisKeys(iXml, HttpContext.Current.Session["dbuser"].ToString());
            return new { result = delKeyResp };
        }
        if (iXml != string.Empty)
            iXml = iXml.Replace("<root", "<root scriptpath='" + ConfigurationManager.AppSettings["ScriptsPath"].ToString() + "'");

        if (isScript)
            ires = objWebServiceExt.callRemoteDoScriptWS(iName, iXml, ires, objIview.WebServiceTimeout);
        else
            ires = objWebServiceExt.CallRemoteDoActionWS(iName, iXml, ires, objIview.WebServiceTimeout);

        if (ires != null)
            ires = ires.Split('♠')[1];
        ires = ires.Replace("'", ";quot");
        ires = ires.Replace("\\", ";bkslh");
        ires = ires.Replace("\n", "<br>");
        string result = "success";
        if (ires == string.Empty)
            result = "failure";
        if (objIview.ActBtnNavigation != null && objIview.ActBtnNavigation.ContainsKey(actName))
            return new { result = result, actResponse = ires, ActBtnNavigation = objIview.ActBtnNavigation[actName].ToLower() };
        else
            return new { result = result, actResponse = ires };

    }

    [WebMethod]
    public static string GetTstructFieldsForListView(string iName)
    {
        ASB.WebService asbWebService = new ASB.WebService();
        return asbWebService.GetTstructFieldsForListView(iName);
    }

    //modifaction done after redis logic changed
    //souvik
    public static string DeleteRedisKeys(string xml, string schemaName)
    {
        LogFile.Log logobj = new LogFile.Log();
        string selectedKeys = string.Empty;
        ArrayList redisvalues = new ArrayList();
        string pgKey = Constants.AXPAGETITLE;
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        Util.Util util = new Util.Util();
        redisvalues = (ArrayList)fObj.ObjectJsonFromRedis(util.GetRedisServerkey(pgKey, ""), schemaName);
        var node = "";
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            ArrayList formList = new ArrayList();
            bool delRecords = false;
            var items = "";
            string tname = "";
            foreach (XmlNode childNode in doc.SelectNodes("//root"))
            {
                foreach (XmlNode chdNode in doc.SelectNodes("//row"))
                {
                    node = WebUtility.HtmlDecode(chdNode.SelectSingleNode("TNAME").InnerText);
                    var keys = WebUtility.HtmlDecode(chdNode.SelectSingleNode("KEYS").InnerText);
                    selectedKeys += keys + "&";
                    formList.Add(node);


                }
            }
            if (selectedKeys != string.Empty)
            {
                selectedKeys = selectedKeys.Substring(0, selectedKeys.Length - 1);
                FDW objFdw = new FDW();
                string proj = string.Empty;
                delRecords = objFdw.DeleteAllKeys(selectedKeys, schemaName);

            }
            if (delRecords == true && redisvalues.Count > 0)
            {
                ArrayList redisvaluesTemp = new ArrayList(redisvalues);
                foreach (var item in redisvalues)
                {
                    var id = item.ToString().Split('♠')[2];
                    tname = id.ToString();
                    foreach (var item1 in formList)
                    {
                        if (tname.IndexOf(item1.ToString()) != -1)
                        {
                            redisvaluesTemp.Remove(item);
                            break;

                        }

                    }

                }
                FDW fdwObj = new FDW();
                fdwObj.SaveInRedisServer(util.GetRedisServerkey(pgKey, ""), redisvaluesTemp, Constants.AXPAGETITLE);
                return "success";
            }
            else return "failure";
        }

        catch (Exception ex)
        {
            logobj.CreateLog("Iview - DeleteRedisKeys -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "openiview-dev---", string.Empty);
        }
        return "success";
    }

    // generic method for converting string to xmldocument
    //souvik
    private static XmlDocument GetElement(string xml)
    {
        XmlDocument doc1 = new XmlDocument();
        doc1.LoadXml(xml);
        return doc1;
    }


    public void GenericRedisFunction(string Title, string objIName, string structure, string schemaName)
    {

        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        XmlDocument xmldoc1 = new XmlDocument();
        xmldoc1.LoadXml(structure);
        XmlNode cNode = default(XmlNode);
        cNode = xmldoc1.SelectSingleNode("//iview/a2");
        string pgKey = Constants.AXPAGETITLE;
        ArrayList redisvalues = new ArrayList();
        FDW fdwObj = new FDW();
        Util.Util util = new Util.Util();
        var redisvalues1 = fObj.ObjectJsonFromRedis(util.GetRedisServerkey(pgKey, ""), schemaName);
        if (redisvalues1 == null)
            redisvalues.Add(Title + "♠" + cNode.InnerText + "♠" + objIName);
        else
        {
            redisvalues = (ArrayList)redisvalues1;
            string newValue = Title + "♠" + cNode.InnerText + "♠" + objIName;
            if (!redisvalues.Contains(newValue))

                redisvalues.Add(newValue);

        }
        if (Title == "Listview" && cNode.InnerText != "")
            objIview.IviewCaption = cNode.InnerText;


        fdwObj.SaveInRedisServer(util.GetRedisServerkey(pgKey, objIName), redisvalues, Constants.AXPAGETITLE, HttpContext.Current.Session["dbuser"].ToString());
    }

    public void GenericRedisFunction2()
    {
        string pgKey = Constants.AXPAGETITLE;
        ArrayList redisvalues = new ArrayList();
        FDW fdwObj = new FDW();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

        var redisvalues1 = fObj.ObjectJsonFromRedis(util.GetRedisServerkey(pgKey, ""));
        if (redisvalues1 == null)
            redisvalues.Add(Title + "♠" + objIview.IviewCaption + "♠" + objIview.IName);
        else
        {
            redisvalues = (ArrayList)redisvalues1;

            string newValue = Title + "♠" + objIview.IviewCaption + "♠" + objIview.IName;
            if (Request.QueryString["tstcaption"] == null)
            {
                if (!redisvalues.Contains(newValue))

                    redisvalues.Add(newValue);
            }
        }

        fdwObj.SaveInRedisServer(util.GetRedisServerkey(pgKey, objIview.IName), redisvalues, Constants.AXPAGETITLE, schemaName);
    }

    //get redis data logic changed
    //no webservice used
    //souvik
    public XmlDocument GetRedisData()
    {
        XmlDocument doc1 = GetElement(Constants.REDISXML);
        XmlDocument doc2 = GetElement("<row TNAME =\"\" FTYPE=\"\" CAPTION=\"\" KEYSIZE=\"\" TOTALKEYS=\"\" KEYS=\"\"/>");
        XmlNode node = doc2;
        XmlNode root = doc1.DocumentElement;
        LogFile.Log logobj = new LogFile.Log();
        try
        {
            ArrayList formList = new ArrayList();
            ArrayList redisvalues = new ArrayList();
            string pgKey = Constants.AXPAGETITLE;
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            FDW fdwObj = new FDW();
            bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
            if (isRedisConnected)
            {
                redisvalues = (ArrayList)fdrObj.ObjectJsonFromRedis(util.GetRedisServerkey(pgKey, ""), schemaName);
                ArrayList redisvaluesGeneral = fdrObj.GetPrefixedKeys("General", true);
                ArrayList redisKeys = new ArrayList();

                ArrayList addedKeys = new ArrayList();
                XmlElement elem = doc1.CreateElement("rowdata");
                root.InsertAfter(elem, root.LastChild);
                XmlNode rowDataNode = doc1.SelectSingleNode("//rowdata");
                StringBuilder aaa = new StringBuilder();
                aaa.Append("<row FTYPE=\"\" TNAME=\"\" CAPTION=\"\" KEYSIZE=\"\" TOTALKEYS=\"\" KEYS=\"\" /> ");
                elem.InnerText = aaa.ToString();
                foreach (var item in redisvalues)
                {
                    try
                    {
                        var type = item.ToString().Split('♠')[0];
                        var tstivname = item.ToString().Split('♠')[1];
                        var id = item.ToString().Split('♠')[2];
                        string tname = id.ToString();
                        redisKeys = fdrObj.GetPrefixedKeys(tname, true);
                        string keysData = String.Join("&amp;", (string[])redisKeys.ToArray(Type.GetType("System.String")));
                        if (redisKeys.Count > 0)
                        {
                            aaa.AppendLine(string.Format("<row FTYPE=\"{0}\" TNAME=\"{1}\"  CAPTION=\"{2}\" KEYSIZE=\"{3}\" TOTALKEYS=\"{4}\" KEYS=\"{5}\" /> ", WebUtility.HtmlEncode(type), WebUtility.HtmlEncode(tname), WebUtility.HtmlEncode(tstivname), "", redisKeys.Count, WebUtility.HtmlEncode(keysData)));
                            elem.InnerXml = aaa.ToString();
                        }
                    }
                    catch (Exception ex) { }
                }
                if (redisvaluesGeneral.Count > 0)
                {
                    string keysData1 = String.Join("&amp;", (string[])redisvaluesGeneral.ToArray(Type.GetType("System.String")));
                    aaa.AppendLine(string.Format("<row FTYPE=\"{0}\" TNAME=\"{1}\"  CAPTION=\"{2}\" KEYSIZE=\"{3}\" TOTALKEYS=\"{4}\" KEYS=\"{5}\" /> ", "General", "General", "General", "", redisvaluesGeneral.Count, keysData1));
                }
                elem.InnerXml = aaa.ToString();
            }


        }
        catch (Exception ex)
        {
            logobj.CreateLog("Iview - GetRedisKeys -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "openiview-dev---", string.Empty);
        }
        return doc1;
    }

    public DataTable getTempleteStringChoices(string iName)
    {
        string result = string.Empty;
        DataTable templateDT = new DataTable();
        string sqlQuery = string.Empty;

        string fdSettingsKey = Constants.RedisOldIviewTemplates;
        if (objIview.purposeString != "")
        {
            fdSettingsKey = Constants.RedisOldListviewTemplates;
        }

        try
        {

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
            {
                fObj = new FDR();
                HttpContext.Current.Session["FDR"] = fObj;
            }

            result = fObj.StringFromRedis(util.GetRedisServerkey(fdSettingsKey, iName, user));
        }
        catch (Exception ex) { }

        if (result == string.Empty)
        {

            string errorLog = logobj.CreateLog("GetIviewTemplete.", Session["nsessionid"].ToString(), "GetIviewTemplete-" + iName + string.Empty, "new");

            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = Constants.GET_IV_TEMPLETE.Replace(Constants.VAR_IVNAME, iName);
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application
                ["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";

            try
            {
                result = objWebServiceExt.CallGetChoiceWS(iName, query);
            }
            catch (Exception ex) { }
        }

        try
        {
            if (result != string.Empty || (!result.StartsWith("<error>")) || (result.Contains("error")))
            {
                DataSet ds = new DataSet();
                System.IO.StringReader sr = new System.IO.StringReader(result);
                ds.ReadXml(sr);

                if ((ds.Tables.Count > 0) && (ds.Tables["row"].Rows.Count > 0))
                {
                    templateDT = ds.Tables["row"];
                }
            }
        }
        catch (Exception ex) { }

        try
        {
            FDW fdwObj = new FDW();
            fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdSettingsKey, iName, user), result, fdSettingsKey, HttpContext.Current.Session["dbuser"].ToString());
        }
        catch (Exception ex) { }

        return templateDT;
    }


    public bool GetRequestType()
    {
        bool returnBool = requestJSON;
        try
        {
            string config = "";
            if (Request.QueryString["tstcaption"] != null)
            {
                config = util.GetAdvConfigs("Load Old Model Views", "tstruct", iName);
            }
            else
            {
                config = util.GetAdvConfigs("Load Old Model Views", "iview", iName);
            }
            if (config != "true")
            {
                config = "false";
            }
            returnBool = config == "false";
        }
        catch (Exception ex) { }
        //return false;
        return returnBool;
    }

    public IviewData GetStructDef(string iName, string datakey, string datasubkey, string ivKey)
    {
        IviewData returnIview = null;
        IviewParams returnParams = null;
        try
        {
            if (flKey == null)
            {
                flKey = GenerateGlobalSmartViewsKey(iName, Request.QueryString["tstcaption"] != null);
            }

            string fileName = "openiview-" + iName;

            string sessionID = Session["nsessionid"].ToString();
            sessionID = util.CheckSpecialChars(sessionID);

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
            {
                fObj = new FDR();
                HttpContext.Current.Session["FDR"] = fObj;
            }
            logobj.CreateLog("Get IviewObj from Cache, User: " + user + " Role: " + AxRole, sessionID, fileName, "");

            returnIview = (IviewData)fObj.IviewObjFromRedis(ivKey);

            if (returnIview != null && returnIview.iviewParamsList != null && returnIview.iviewParamsList.Count > 0)
            {
                ArrayList keyList = returnIview.iviewParamsList;
                int keyIndex = -1;

                datasubkey = MakeVarKeyName(returnIview.GlobalVars);
            }

            returnParams = (IviewParams)fObj.HashGetParamObjFromRedis(datakey, datasubkey);



            if (returnIview != null)
            {
                returnIview.smartviewSettings = string.Empty;



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
                    if (!returnParams.isIviewParamClearCache)
                        returnIview.isObjFromCache = true;

                    string fdSettingsKey = Constants.RedisIviewSettings;
                    if (Request.QueryString["tstcaption"] != null)
                    {
                        fdSettingsKey = Constants.RedisListviewSettings;
                    }

                    JToken userPofileSmartViews = new JObject();
                    JToken allProfileSmartViews = new JObject();

                    try
                    {
                        userPofileSmartViews = JToken.Parse(fObj.StringFromRedis(fObj.MakeKeyName(fdSettingsKey, iName, user), schemaName));
                    }
                    catch (Exception ex)
                    { }
                    try
                    {
                        allProfileSmartViews = JToken.Parse(fObj.StringFromRedis(fObj.MakeKeyName(fdSettingsKey, iName, "all"), schemaName));
                    }
                    catch (Exception ex)
                    { }

                    JObject forUserSettings = new JObject();

                    if (userPofileSmartViews.ToString() != "{}")
                    {
                        forUserSettings[user] = userPofileSmartViews;
                    }

                    if (allProfileSmartViews.ToString() != "{}")
                    {
                        forUserSettings["all"] = allProfileSmartViews;
                    }

                    returnIview.smartviewSettings = forUserSettings.ToString();
                    if (returnParams != null)
                    {
                        returnIview.iviewParams = returnParams;
                        objParams = returnParams;
                    }


                    logobj.CreateLog("Getting IviewObj from cache " + user + " Role: " + AxRole, sessionID, fileName, "");
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

        if (returnIview != null && objParams == null)
        {
            if (returnIview.isObjFromCache)
            {
                returnIview.isObjFromCache = false;
            }
        }

        return returnIview;
    }

    public bool SetStructDef(string datakey, string datasubkey, string ivKey)
    {
        bool returnSuccess = false;
        if (isException)
        {
            return returnSuccess;
        }
        try
        {
            if (!(objIview.iviewParamsList.IndexOf(datasubkey) > -1))
            {
                objIview.iviewParamsList.Add(datasubkey);
            }

            if (flKey == null)
            {
                flKey = GenerateGlobalSmartViewsKey(iName, Request.QueryString["tstcaption"] != null);
            }

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
            {
                fObj = new FDR();
                HttpContext.Current.Session["FDR"] = fObj;
            }

            FDW fdwObj = new FDW();


            string tempSmartviewSettings = objIview.smartviewSettings;
            IviewParams tempIviewParams = objIview.iviewParams;

            objIview.smartviewSettings = string.Empty;
            objIview.iviewParams = null;

            returnSuccess = fdwObj.SaveInRedisServer(ivKey, objIview, "", schemaName);

            if (objParams.ParamXML != string.Empty)
                fdwObj.HashSetSaveInRedisServer(datakey, datasubkey, objParams, "", schemaName);

            objIview.smartviewSettings = tempSmartviewSettings;
            objIview.iviewParams = tempIviewParams;

        }
        catch (Exception ex) { }

        return returnSuccess;
    }

    public object GenerateGlobalSmartViewsKey(string iName, bool isListview)
    {
        string fldarrkey = string.Empty, arraySubKey = string.Empty;

        string fdObjKey = Constants.RedisIviewObj;
        if (isListview)
        {
            fdObjKey = Constants.RedisListviewObj;
        }

        string ivKey = util.GetRedisServerkey(fdObjKey, iName);

        try
        {
            int keyIndex = -1;

            string fdListKey = Constants.RedisIviewObjList;
            if (isListview)
            {
                fdListKey = Constants.RedisListviewObjList;
            }

            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fdrObj == null)
            {
                fdrObj = new FDR();
                HttpContext.Current.Session["FDR"] = fdrObj;
            }

            fldarrkey = fdrObj.MakeKeyName(fdListKey, iName);
            int ExistKeyind = fdrObj.GetKeyIndex(fldarrkey);
            if (ExistKeyind == 0 && objIview != null)
            {
                ArrayList keyList = objIview.iviewParamsList;

                arraySubKey = MakeVarKeyName(objIview.GlobalVars);
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("GenerateGlobalSmartViewsKey -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in GenerateGlobalSmartViewsKey", "new");
        }
        return new { arrKey = fldarrkey, arraySubKey = arraySubKey, ivKey = ivKey };
    }

    public IviewData GetGlobalSmartViews(string iName, dynamic flKey)
    {
        IviewData returnIview = null;
        try
        {
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            string arrkey = flKey.GetType().GetProperty("arrKey").GetValue(flKey, null);
            string arraySubKey = flKey.GetType().GetProperty("arraySubKey").GetValue(flKey, null);
            string ivKey = flKey.GetType().GetProperty("ivKey").GetValue(flKey, null);
            if (arrkey != string.Empty)
            {
                returnIview = GetStructDef(iName, arrkey, arraySubKey, ivKey);
            }
            else
            {
                returnIview = new IviewData();
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("GetGlobalSmartViews -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in GetGlobalSmartViews", "new");
        }
        return returnIview;
    }

    public bool SetGlobalSmartViews(IviewData objIview, string iName, dynamic flKey)
    {
        bool returnSuccess = false;
        try
        {
            string arrkey = flKey.GetType().GetProperty("arrKey").GetValue(flKey, null);
            string arraySubKey = flKey.GetType().GetProperty("arraySubKey").GetValue(flKey, null);
            string ivKey = flKey.GetType().GetProperty("ivKey").GetValue(flKey, null);

            string stsResult = objIview.GlobalVars;

            //string stsGlobal = util.ParseJSonResultNode(stsResult);
            string stsGlobal = MakeVarKeyName(stsResult);
            string strflKeys = string.Empty;

            strflKeys = stsGlobal;

            FDW fdwObj = new FDW();

            returnSuccess = SetStructDef(arrkey, strflKeys, ivKey);
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("SetGlobalSmartViews -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in SetGlobalSmartViews", "new");
        }
        return returnSuccess;
    }

    public string processNotificationLoadData(string redisLoadKey, string user, string schemaName)
    {
        string returnData = string.Empty;

        try
        {
            string redisVal = string.Empty;
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            redisVal = fObj.ReadStringKeywithSchema(user + "-notify-" + redisLoadKey, schemaName);
            if (redisVal != string.Empty && redisVal != "redisnotconnected")
            {
                JObject redisJObject = new JObject();

                redisJObject = JObject.Parse(redisVal);

                JArray notifyArray = (JArray)redisJObject["msg"];

                JObject loadTypeNode = new JObject();

                loadTypeNode = (JObject)notifyArray.FirstOrDefault(x => x["loadtype"] != null);

                JObject sessionLoadType = JObject.Parse(Session["iview-" + iName + "-" + loadTypeNode["loadtype"].ToString()].ToString());

                redisLoadType = sessionLoadType["type"].ToString();
                JObject headrow = (JObject)sessionLoadType["headrow"];

                if (headrow != null)
                {
                    objIview.headerJSON = headrow.ToString().Trim();
                }

                string paramsVal = string.Empty;

                paramsVal = sessionLoadType["params"].ToString();

                if (paramsVal != string.Empty)
                {
                    hdnparamValues.Value = paramsVal;
                }

                hdnWebServiceViewName.Value = sessionLoadType["viewName"].ToString();

                JObject cmdNode = (JObject)notifyArray.FirstOrDefault(x => x["cmd"] != null);

                string cmdVal = cmdNode["cmdval"].ToString();

                string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();

                string folderPath = ScriptsPath + "Axpert\\" + Session["nsessionid"].ToString() + "\\";

                string filePath = folderPath + cmdVal;

                System.IO.FileInfo sfile = new System.IO.FileInfo(filePath);

                if (sfile.Exists)
                {
                    returnData = System.IO.File.ReadAllText(@"" + filePath);
                }
            }
        }
        catch (Exception ex) { }

        return returnData;
    }

    private JObject generateConfigurationJson(DataTable dt)
    {
        JObject returnJObject = new JObject();
        if (dt != null && (dt.Rows.Count > 0))
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Merge(dt.Copy());
                ds.DataSetName = "configurations";
                ds.Tables[0].TableName = "config";
                returnJObject = JObject.Parse(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex) { }
        }
        return returnJObject;
    }

    private string MakeVarKeyName(string result)
    {
        string returnString = string.Empty;

        var globalVars = FDR.GetGlobalVars();

        var pickData = JsonConvert.DeserializeObject<globalVar>(result.ToString());
        if (pickData != null && pickData.globalVars != null && pickData.globalVars.Count() > 0 && globalVars.Count > 0)
        {
            returnString = string.Join("♦", pickData.globalVars.Select(i =>
            {
                var filterKeys = globalVars.AsEnumerable().Where(x => x.ToLower().StartsWith(i.n.ToLower() + ":")).ToList();
                if (filterKeys.Count > 0)
                {
                    return filterKeys[0];
                }
                else
                {
                    return i.n + ":" + "";
                }
            }
            ));
        }

        return returnString;
    }

    private class globalVar
    {
        public List<globalVars> globalVars { get; set; }
    }

    private class globalVars
    {
        public string n { get; set; }
        public string v { get; set; }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetIviewSelectData(string iviewName, string pageNo, string pageSize, string fieldName, string fieldValue, string depParamVal, string clearPCache)
    {
        Util.Util utilGlo = new Util.Util();
        string pickDepParams = string.Empty;

        try
        {
            if (clearPCache == "true")
            {
                ClearRedisDataCache(iviewName, "refreshParams", false);
            }
        }
        catch (Exception ex)
        { }

        try
        {
            if (depParamVal != "")
            {
                string[] pdParamStr = depParamVal.Split('¿');
                for (int i = 0; i < pdParamStr.Count(); i++)
                {
                    string pdNameVal = pdParamStr[i];
                    if (pdNameVal != "")
                    {
                        string[] pdNames = pdNameVal.Split('~');
                        pickDepParams = pickDepParams + "<" + pdNames[0] + ">";
                        pickDepParams = pickDepParams + utilGlo.CheckSpecialChars(pdNames[1]);
                        pickDepParams = pickDepParams + "</" + pdNames[0] + ">";
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        fieldValue = utilGlo.CheckSpecialChars(fieldValue);
        string iXml = string.Empty;
        string filename = "iviewpicklist-" + iviewName;
        LogFile.Log logobj = new LogFile.Log();
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        string errlog = logobj.CreateLog("", HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
        fieldValue = "";
        iXml = iXml + "<sqlresultset axpapp=\"" + HttpContext.Current.Session["project"] + "\" value=\"" + fieldValue + "\" sessionid= \"" + HttpContext.Current.Session["nsessionid"] + "\" pname =\"" + fieldName + "\" trace=\"" + errlog + "\" user=\"" + HttpContext.Current.Session["user"] + "\" ivname=\"" + iviewName + "\" pageno =\"" + pageNo + "\" pagesize=\"" + pageSize + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
        if (pickDepParams != string.Empty)
            iXml = iXml + "<varlist>" + pickDepParams + "</varlist>";
        iXml = iXml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
        iXml = iXml + "</sqlresultset>";

        string result = asbExt.CallGetParamChoicesWS(iviewName, iXml);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(result);

        string json = JsonConvert.SerializeXmlNode(doc);

        return json;
    }

    [WebMethod(EnableSession = true)]
    public static string CallPublish(string structs, ArrayList structArray, string schema, string destScripts, string comment, string devPublish)
    {
        string returnString = new JObject().ToString();
        try
        {
            JObject exportResp = PublishExportfromsource(structs, schema, destScripts, comment, devPublish);

            if (exportResp != null)
            {
                if (exportResp["result"][0]["error"] != null)
                {
                    JArray finalArray = new JArray();
                    foreach (string str in structArray)
                    {
                        finalArray.Add(new JObject{
                            { "importstructurename", str },
                            { "status", exportResp["result"][0]["error"]["msg"] },
                            { "importedon", "" }
                        });
                    }

                    exportResp["result"][0]["importstatus"] = finalArray;
                    returnString = PublishUpdatepublish(exportResp, structs, schema, destScripts);
                }
                else
                {
                    JObject uploadResponse = PublishUploadfiles(exportResp, structs, schema, destScripts);
                    if (uploadResponse != null && uploadResponse.ToString() != returnString)
                    {
                        if (uploadResponse["result"][0]["error"] != null)
                        {
                            JArray finalArray = new JArray();
                            foreach (string str in structArray)
                            {
                                finalArray.Add(new JObject{
                                    { "importstructurename", str },
                                    { "status", uploadResponse["result"][0]["error"]["msg"] },
                                    { "importedon", "" }
                                });
                            }
                            uploadResponse["result"][0]["importstatus"] = finalArray;
                            returnString = PublishUpdatepublish(uploadResponse, structs, schema, destScripts);
                        }
                        else
                        {
                            JObject importResponse = PublishImporttoTarget(uploadResponse, exportResp["result"][0]["exportfilenames"].ToString(), structs, schema, destScripts, comment, devPublish);
                            if (importResponse != null && importResponse.ToString() != returnString)
                            {
                                if (importResponse["result"][0]["error"] != null)
                                {
                                    JArray finalArray = new JArray();
                                    foreach (string str in structArray)
                                    {
                                        finalArray.Add(new JObject{
                                            { "importstructurename", str },
                                            { "status", importResponse["result"][0]["error"]["msg"] },
                                            { "importedon", "" }
                                        });
                                    }
                                    importResponse["result"][0]["importstatus"] = finalArray;
                                    returnString = PublishUpdatepublish(importResponse, structs, schema, destScripts);
                                }
                                else
                                {
                                    returnString = PublishUpdatepublish(importResponse, structs, schema, destScripts);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("Publish -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "Publish", string.Empty);
        }

        return returnString;
    }

    private static JObject PublishExportfromsource(string structs, string schema, string destScripts, string comment, string devPublish)
    {
        JObject returnObj = new JObject();
        JObject data = new JObject();
        JObject param = new JObject();
        try
        {
            if (HttpContext.Current.Session["ARM_Scripts_URL"] != null && HttpContext.Current.Session["ARM_Scripts_URL"].ToString() != "")
            {
                destScripts = HttpContext.Current.Session["ARM_Scripts_URL"].ToString();
                if (destScripts.Substring(destScripts.Length - 1) != "/")
                    destScripts += "/";
            }
            LogFile.Log logobj = new LogFile.Log();
            string trace = logobj.CreateLog("Publish - Exportfromsource WS", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - Exportfromsource", string.Empty);

            string scriptURL = destScripts + "ASBPublishRest.dll/datasnap/rest/TASBPublishRest/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(scriptURL + "Exportfromsource");
            request.Method = "POST";
            request.ContentType = "application/json";

            param["axpapp"] = HttpContext.Current.Session["project"].ToString();
            param["username"] = HttpContext.Current.Session["username"].ToString();
            param["s"] = HttpContext.Current.Session["nsessionid"].ToString();
            param["publishto"] = schema;
            param["comments"] = comment;
            param["trace"] = trace;
            param["exportstructures"] = structs;
            param["exportlocation"] = "";
            param["publishonlyappschema"] = devPublish == "T" ? "false" : "true";

            data["exportfromsource"] = param;

            request.ContentLength = data.ToString().Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(data.ToString());
            requestWriter.Close();
            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                returnObj = JObject.Parse(response);
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Publish - Exportfromsource WS -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - Exportfromsource", string.Empty);
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("Publish - Exportfromsource Method -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - Exportfromsource", string.Empty);
        }

        return returnObj;
    }

    private static JObject PublishUploadfiles(JObject exportResp, string structs, string schema, string destScripts)
    {
        JObject returnObj = exportResp;
        string exportLocation = string.Empty;
        string[] exportFiles = new string[] { };
        JObject data = new JObject();

        try
        {
            LogFile.Log logobj = new LogFile.Log();
            string trace = logobj.CreateLog("Publish - Uploadfiles WS", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - Uploadfiles", string.Empty);
            exportLocation = exportResp["result"][0]["exportlocation"].ToString();
            exportFiles = exportResp["result"][0]["exportfilenames"].ToString().Split(',');
            JArray files = new JArray();
            foreach (string file in exportFiles)
            {
                string fileData = string.Empty;
                try
                {
                    string filePath = exportLocation + file;

                    System.IO.FileInfo sfile = new System.IO.FileInfo(filePath);

                    if (sfile.Exists)
                    {
                        Byte[] bytes = System.IO.File.ReadAllBytes(@"" + filePath);
                        fileData = Convert.ToBase64String(bytes);
                        sfile.Delete();
                    }
                }
                catch (Exception ex) { }

                files.Add(new JObject{
                    { "filename", file },
                    { "maindir", "" },
                    { "subdir", "" },
                    { "b64filedata", fileData }
                });
            }

            string destScriptsURL = destScripts + "ASBPublishRest.dll/datasnap/rest/TASBPublishRest/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destScriptsURL + "Uploadfiles");
            request.Method = "POST";
            request.ContentType = "application/json";

            data["files"] = files;
            data["trace"] = trace;
            data["username"] = HttpContext.Current.Session["username"].ToString();

            request.ContentLength = data.ToString().Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(data.ToString());
            requestWriter.Close();
            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                returnObj = JObject.Parse(response);
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Publish - Uploadfiles WS -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - Uploadfiles", string.Empty);
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("Publish - Uploadfiles Method -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - Uploadfiles", string.Empty);
        }

        return returnObj;
    }

    private static JObject PublishImporttoTarget(JObject uploadResponse, string importfilenames, string structs, string schema, string destScripts, string comment, string devPublish)
    {
        JObject returnObj = uploadResponse;
        JObject data = new JObject();
        JObject param = new JObject();
        try
        {
            LogFile.Log logobj = new LogFile.Log();
            string trace = logobj.CreateLog("Publish - ImporttoTarget WS", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - ImporttoTarget", string.Empty);
            string destScriptsURL = destScripts + "ASBPublishRest.dll/datasnap/rest/TASBPublishRest/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destScriptsURL + "ImporttoTarget");
            request.Method = "POST";
            request.ContentType = "application/json";
            param["axpapp"] = schema;
            param["username"] = HttpContext.Current.Session["username"].ToString();
            param["s"] = HttpContext.Current.Session["nsessionid"].ToString();
            param["trace"] = trace;
            param["comments"] = comment;
            param["importfilenames"] = importfilenames;
            param["importfilepath"] = "";
            param["publishonlyappschema"] = devPublish == "T" ? "false" : "true";
            data["importtotarget"] = param;
            request.ContentLength = data.ToString().Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(data.ToString());
            requestWriter.Close();
            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                returnObj = JObject.Parse(response);
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Publish - ImporttoTarget WS -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - ImporttoTarget", string.Empty);
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("Publish - ImporttoTarget Method -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - ImporttoTarget", string.Empty);
        }

        return returnObj;
    }

    private static string PublishUpdatepublish(JObject importResponse, string structs, string schema, string destScripts)
    {
        JObject returnObj = importResponse;
        JObject data = new JObject();
        JObject param = new JObject();
        JArray importStatus = new JArray();

        try
        {
            importStatus = (JArray)importResponse["result"][0]["importstatus"];
        }
        catch (Exception ex) { }

        try
        {
            if (HttpContext.Current.Session["ARM_Scripts_URL"] != null && HttpContext.Current.Session["ARM_Scripts_URL"].ToString() != "")
            {
                destScripts = HttpContext.Current.Session["ARM_Scripts_URL"].ToString();
                if (destScripts.Substring(destScripts.Length - 1) != "/")
                    destScripts += "/";
            }
            LogFile.Log logobj = new LogFile.Log();
            string trace = logobj.CreateLog("Publish - Updatepublish WS", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - Updatepublish", string.Empty);
            string scriptURL = destScripts + "ASBPublishRest.dll/datasnap/rest/TASBPublishRest/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(scriptURL + "UpdatePublish");
            request.Method = "POST";
            request.ContentType = "application/json";
            param["axpapp"] = HttpContext.Current.Session["project"].ToString();
            param["username"] = HttpContext.Current.Session["username"].ToString();
            param["s"] = HttpContext.Current.Session["nsessionid"].ToString();
            param["trace"] = trace;
            param["importstatus"] = importStatus;
            data["updatepublish"] = param;
            request.ContentLength = data.ToString().Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(data.ToString());
            requestWriter.Close();
            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                returnObj = JObject.Parse(response);
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Publish - Updatepublish WS -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - Updatepublish", string.Empty);
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("Publish - Updatepublish Method -" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "Publish - Updatepublish", string.Empty);
        }
        string returnString = string.Empty;
        if (returnObj != null && returnObj.ToString() != (new JObject().ToString()))
        {
            if (returnObj["result"][0]["error"] != null)
            {
                returnObj["result"][0]["error"]["importstatus"] = importResponse;
                returnString = returnObj["result"][0]["error"].ToString();
            }
            else
            {
                returnObj["result"][0]["importstatus"] = importResponse;
                returnString = returnObj["result"][0].ToString();
            }
        }
        return returnString;
    }
}


public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string value)
    {
        return value == null || value.All(char.IsWhiteSpace);
    }
}
