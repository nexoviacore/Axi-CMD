using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;

/// <summary>
/// Summary description for IviewData
/// </summary>
[Serializable()]
public class IviewData
{
    public IviewData()
    {
        //
        // TODO: Add constructor logic here
        //
        ReportHdrs = new ArrayList();
        IViewWhenEmpty = string.Empty;
        
        
        
        
        
        ShowHiddengridCols = new ArrayList();
        IsRunningTotal = new ArrayList();
        GolbalVarName = new ArrayList();
        GolbalVarValue = new ArrayList();
        SubtColName = new ArrayList();
        SubtCaption = new ArrayList();
        SubtHeader = new ArrayList();
        SubtFooter = new ArrayList();
        SubtOrderNo = new ArrayList();
        SubtSpaceFooter = new ArrayList();
        SubtPageBreak = new ArrayList();
        RunningTotalValue = new DataTable();
        RunningPageCount = new int();
        ColNoRepeat = new ArrayList();
        
        customBtnIV = new ArrayList();

        iviewParams = new IviewParams();

        iviewParamsList = new ArrayList();

        EncryptedColumns = new ArrayList();
    }


    #region privatevariables

    private ArrayList colZeroOff;

    private ArrayList colNoRepeat;

    
    public ArrayList customBtnIV;

    public IviewParams iviewParams;

    public ArrayList iviewParamsList;

    private int totalRows;



    //private bool isPerfXxml = false;
    private string iVType;
    public string IVType
    {
        get { return iVType; }
        set { iVType = value; }
    }

    private string globalVars = string.Empty;
    public string GlobalVars
    {
        get { return globalVars; }
        set { globalVars = value; }
    }









    private string grdPageSize = "1000";

    private ArrayList ivCaption;

    private string currentPageNo;


    //TODOP: use only one of the below property (ivname, iname, tid)
    private string ivname;

    private string iName;

    private string tid;

    private string iviewCaption;

    private bool isIviewStagLoad;

    

    private List<DataTable> stagTables;

    private string rxml;

    private string fromHyperLink;

    private string myViewName;

    private ArrayList reportHdrs;

    private string iViewWhenEmpty;

    private string srNoColumName;

    private string toolbarHtml;

    private bool isWordWithHtml;


    private string iviewFooter;

    public string IviewFooter
    {
        get { return iviewFooter; }
        set { iviewFooter = value; }
    }

    

    

    public bool isPivotReport = false;


    public ArrayList PivotMergeHeaders { get; set; }
    public ArrayList PivotStartCol { get; set; }
    public ArrayList PivotEndCol { get; set; }

    //private bool isDirectDBcall;

    private string structureXml = string.Empty;

    private string accessControlXml = string.Empty;



    private string paramAndLvXml = string.Empty;



    private DataTable dsIviewConfig;

    private ArrayList golbalVarName;

    private ArrayList golbalVarValue;


    public ArrayList ColNoRepeat
    {
        get { return colNoRepeat; }
        set { colNoRepeat = value; }
    }


    public ArrayList GolbalVarName
    {
        get { return golbalVarName; }
        set { golbalVarName = value; }
    }

    public ArrayList GolbalVarValue
    {
        get { return golbalVarValue; }
        set { golbalVarValue = value; }
    }

    public DataTable DsIviewConfig
    {
        get { return dsIviewConfig; }
        set { dsIviewConfig = value; }
    }


    public string StructureXml
    {
        get { return structureXml; }
        set { structureXml = value; }
    }

    public string AccessControlXml
    {
        get { return accessControlXml; }
        set { accessControlXml = value; }
    }



    public string ParamAndLvXml
    {
        get { return paramAndLvXml; }
        set { paramAndLvXml = value; }
    }

    public bool IsWordWithHtml
    {
        get { return isWordWithHtml; }
        set { isWordWithHtml = value; }
    }

    public int TotalRows
    {
        get { return totalRows; }
        set { totalRows = value; }
    }


    public string SrNoColumName
    {
        get { return srNoColumName; }
        set { srNoColumName = value; }
    }

    public string IViewWhenEmpty
    {
        get { return iViewWhenEmpty; }
        set { iViewWhenEmpty = value; }
    }

    public List<DataTable> StagTables
    {
        get { return stagTables; }
        set { stagTables = value; }
    }

    public bool IsIviewStagLoad
    {
        get { return isIviewStagLoad; }
        set { isIviewStagLoad = value; }
    }



    private DataTable dtCurrentdata;

    public DataTable DtCurrentdata
    {
        get { return dtCurrentdata; }
        set { dtCurrentdata = value; }
    }

    private string sessioKey;

    public string SessioKey
    {
        get { return sessioKey; }
        set { sessioKey = value; }
    }
    private string toolbarbtn;
    public string ToolBarBtn
    {
        get { return toolbarbtn; }
        set { toolbarbtn = value; }
    }






    private string menubreadcrumb = string.Empty;


    private ArrayList showHideCols;

    private ArrayList showHiddengridCols;












    private ArrayList subtColName;
    private ArrayList subtCaption;
    private ArrayList subtHeader;
    private ArrayList subtFooter;
    private ArrayList subtOrderNo;
    private ArrayList subtSpaceFooter;
    private ArrayList subtPageBreak;


    private DataTable runningTotalValue;
    private ArrayList isRunningTotal;
    private int runningPageCount;

    private ArrayList encryptedColumns;

    public int RunningPageCount
    {
        get { return runningPageCount; }
        set { runningPageCount = value; }
    }
    public DataTable RunningTotalValue
    {
        get { return runningTotalValue; }
        set { runningTotalValue = value; }
    }

    public ArrayList IsRunningTotal
    {
        get { return isRunningTotal; }
        set { isRunningTotal = value; }
    }

    public ArrayList EncryptedColumns
    {
        get { return encryptedColumns; }
        set { encryptedColumns = value; }
    }

    private string associatedTStruct;
    private string axpAutoSplit;
    private string axpIviewDisableSplit;
    Dictionary<string, string> actBtnNavigation;
    Dictionary<string, string> hypLnkNavigation;
    private int webServiceTimeout = 100000;
    private bool retainIviewParams = false;
    private bool iviewSessionCaching = true;
    LogFile.Log logobj = new LogFile.Log();

    #endregion


    #region publicvariables



    public ArrayList SubtColName
    {
        get { return subtColName; }
        set { subtColName = value; }
    }

    public ArrayList SubtCaption
    {
        get { return subtCaption; }
        set { subtCaption = value; }
    }

    public ArrayList SubtHeader
    {
        get { return subtHeader; }
        set { subtHeader = value; }
    }

    public ArrayList SubtFooter
    {
        get { return subtFooter; }
        set { subtFooter = value; }
    }

    public ArrayList SubtOrderNo
    {
        get { return subtOrderNo; }
        set { subtOrderNo = value; }
    }

    public ArrayList SubtSpaceFooter
    {
        get { return subtSpaceFooter; }
        set { subtSpaceFooter = value; }
    }

    public ArrayList SubtPageBreak
    {
        get { return subtPageBreak; }
        set { subtPageBreak = value; }
    }



    #endregion


    #region publicvariables



    public ArrayList ShowHiddengridCols
    {
        get { return showHiddengridCols; }
        set { showHiddengridCols = value; }
    }

    public ArrayList ShowHideCols
    {
        get { return showHideCols; }
        set { showHideCols = value; }
    }
    public string Tid
    {
        get { return tid; }
        set { tid = value; }
    }

    public string IName
    {
        get { return iName; }
        set { iName = value; }
    }

    public string Ivname
    {
        get { return ivname; }
        set { ivname = value; }
    }


    public string CurrentPageNo
    {
        get { return currentPageNo; }
        set { currentPageNo = value; }
    }


    public ArrayList IvCaption
    {
        get { return ivCaption; }
        set { ivCaption = value; }
    }

    public string GrdPageSize
    {
        get { return grdPageSize; }
        set { grdPageSize = value; }

    }

    public string ResultXml
    {
        get { return rxml; }
        set { rxml = value; }

    }

    public string IviewCaption
    {
        get { return iviewCaption; }
        set { iviewCaption = value; }
    }

    public string Menubreadcrumb
    {
        get { return menubreadcrumb; }
        set { menubreadcrumb = value; }
    }



    public string FromHyperLink
    {
        get { return fromHyperLink; }
        set { fromHyperLink = value; }
    }

    public ArrayList ReportHdrs
    {
        get { return reportHdrs; }
        set { reportHdrs = value; }
    }

    public string MyViewName
    {
        get { return myViewName; }
        set { myViewName = value; }
    }


    public string ToolbarHtml
    {
        get { return toolbarHtml; }
        set { toolbarHtml = value; }
    }





    public string AssociatedTStruct
    {
        get { return associatedTStruct; }
        set { associatedTStruct = value; }

    }

    public string AxpIsAutoSplit
    {
        get { return axpAutoSplit; }
        set { axpAutoSplit = value; }

    }

    public string AxpIviewDisableSplit
    {
        get { return axpIviewDisableSplit; }
        set { axpIviewDisableSplit = value; }

    }
    public Dictionary<string, string> ActBtnNavigation
    {
        get { return actBtnNavigation; }
        set { actBtnNavigation = value; }
    }
    public Dictionary<string, string> HypLnkNavigation
    {
        get { return hypLnkNavigation; }
        set { hypLnkNavigation = value; }
    }

    public int WebServiceTimeout
    {
        get { return webServiceTimeout > 0 ? webServiceTimeout : 100000; }
        set { webServiceTimeout = value; }
    }

    public bool RetainIviewParams
    {
        get { return retainIviewParams; }
        set { retainIviewParams = value; }
    }

    public bool IviewSessionCaching
    {
        get { return iviewSessionCaching; }
        set { iviewSessionCaching = value; }
    }

    //if iview web service will return row count or not
    public bool getIviewRowCount = false;
    //page limit for iview data call
    public int iviewDataWSRows = 1000;
    //real page size returned by web service
    public int realRowCount = 0;
    //wheather we received last page or not;
    public bool lastPageCached = false;
    //page no being opened after caching
    public ArrayList newPagesArray = new ArrayList();
    //real page size in the page number respective of newPagesArray
    public List<int> realPageSize = new List<int>();
    //page size including grand total + sub total ++++++ respective of newPagesArray
    public ArrayList pageSizeWithGTandST = new ArrayList();
    //cached dataset with tables as pages
    public DataSet dsIvPages = new DataSet();
    //current wabservice cached page no
    public int cachedPage = 0;
    //param string for cached data = "";
    public string paramCacheString = "";

    public bool requestJSON = true;

    public string notifyTimeout = string.Empty;

    public string iviewButtonStyle = "classic";

    public string inputControlType = "border";

    public string headerJSON = "";

    public string reportHF = "";

    public string ivRowTemplate = "";

    public string smartviewSettings = "";

    public string dataCache = "Session";

    #endregion

    //exportVerticalAlign = top|middle|bottom
    public string exportVerticalAlign = "middle";

    public string purposeString = string.Empty;

    public bool isObjFromCache = false;



    public string axpCustomJs = string.Empty;
    public string axpCustomCss = string.Empty;







    Util.Util objUtil = new Util.Util();
    #region Functions

    /// <summary>
    /// To return IView data as datatable
    /// </summary>
    /// <param name="iName">IView name</param>
    /// <param name="pageNo">To load page number</param>
    /// <param name="recsPerPage">Records per page to be displayed</param>
    /// <returns></returns>
    public object GetData(string IvName, int pageNo, int recsPerPage, string paramX, string lvXml, string purposeString, string lvStructure)
    {
        string iXml = string.Empty;
        string errlog = string.Empty;
        string result = string.Empty;

        string title = string.Empty;
        string subTitle = string.Empty;
        string customText = string.Empty;
        string footer = string.Empty;
        DataSet dst = new DataSet();
        string fileName = "openiview-" + IvName;

        bool dataCachingEnabled = true;

        if (IviewSessionCaching)
        {
            dataCachingEnabled = dataCache == "Session";
        }
        else
        {
            dataCachingEnabled = IviewSessionCaching;
        }

        string blankLvXML = "<listviewCols>" +
                                "<selectedCols>" + "" + "</selectedCols>" +
                                "<selectedHyperlink>" + "" + "</selectedHyperlink>" +
                            "</listviewCols>";

        string lvXmlToProcess = (lvStructure == string.Empty ? lvXml : blankLvXML);

        string sessionId = string.Empty;
        bool IsDbPagination = false;
        if (HttpContext.Current.Session["AxDbPagination"] != null && HttpContext.Current.Session["AxDbPagination"].ToString() == "")
            IsDbPagination = true;
        else if (HttpContext.Current.Session["AxDbPagination"] != null && HttpContext.Current.Session["AxDbPagination"].ToString() != "")
            IsDbPagination = Convert.ToBoolean(HttpContext.Current.Session["AxDbPagination"].ToString().ToLower());
        string proj = HttpContext.Current.Session["Project"].ToString();
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();

        string headerCompsCachedAttr = string.Empty;
        if (requestJSON && (purposeString == "" || (lvStructure == string.Empty && lvXmlToProcess.IndexOf("<selectedCols></selectedCols>") > -1)))
        {
            //if (HeaderCompsJson == "#$#")
            //{
            //    HeaderCompsJson = string.Empty;
            //}
            //headerCompsCached = HeaderCompsJson != string.Empty;
            headerCompsCachedAttr = " headercached='" + (headerJSON != "" && !isPivotReport && ParamAndLvXml == (purposeString == "" ? paramX : lvXmlToProcess)).ToString().ToLower() + "' ";
        }
        else if (requestJSON && purposeString != "" && (lvStructure != string.Empty || lvXmlToProcess.IndexOf("<selectedCols></selectedCols>") == -1))
        {
            headerCompsCachedAttr = " headercached='false' ";
        }

        //auto fetch data and scroll logic
        int openRecsPerPage = 0;
        {
            if (HttpContext.Current.Session["OpenedRecord-" + IvName] != null && pageNo <= 1)
            {
                //if (dataCachingEnabled) {
                int openRecord = Convert.ToInt32(HttpContext.Current.Session["OpenedRecord-" + IvName]);
                int openPageNo = pageNo;
                openRecsPerPage = recsPerPage;
                if (openRecsPerPage > 0)
                {
                    bool fetchExtraPage = (openRecord % openRecsPerPage) == 0;
                    int fetchRecsPerPage = (openRecsPerPage * (int)Math.Ceiling(Convert.ToDouble(openRecord) / Convert.ToDouble(openRecsPerPage)));
                    if (fetchExtraPage)
                    {
                        fetchRecsPerPage += openRecsPerPage;
                    }
                    recsPerPage = fetchRecsPerPage;
                }
                //}
                HttpContext.Current.Session.Remove("OpenedRecord-" + IvName);
            }
        }

        if (sessionId == "")
            sessionId = HttpContext.Current.Session.SessionID;
        errlog = logobj.CreateLog("Call to GetIView Web Service for page no " + pageNo, sessionId, fileName, "");
        if (IsDbPagination == true && pageNo.ToString() != "1" && HttpContext.Current.Session["iv_noofpages"] != null)
        {
            int.TryParse(HttpContext.Current.Session["iv_noofpages"].ToString(), out totalRows);
            iXml = "<root " + purposeString + headerCompsCachedAttr + " name ='" + IvName + "' axpapp = '" + proj + "' sessionid = '" + sessionId + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errlog + "' pageno='" + pageNo + "' pagesize='" + recsPerPage.ToString() + "' firsttime='" + (pageNo <= 2 && headerCompsCachedAttr == " headercached='false' " ? "yes" : "no") + "' sqlpagination='" + IsDbPagination.ToString().ToLower() + "' totalrows='" + totalRows + "' getrowcount='" + getIviewRowCount.ToString().ToLower() + "' gettotalrows='false' smartview='true'><params>" + paramX + "</params>" + lvXmlToProcess;
        }
        else
        {
            iXml = "<root " + purposeString + headerCompsCachedAttr + " name ='" + IvName + "' axpapp = '" + proj + "' sessionid = '" + sessionId + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errlog + "' pageno='" + pageNo + "' pagesize='" + recsPerPage.ToString() + "' firsttime='" + (pageNo <= 2 && headerCompsCachedAttr == " headercached='false' " ? "yes" : "no") + "' sqlpagination='" + IsDbPagination.ToString().ToLower() + "' getrowcount='" + getIviewRowCount.ToString().ToLower() + "' gettotalrows='false' smartview='true'><params>" + paramX + "</params>" + lvXmlToProcess;
        }

        iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root> ";

        //Call service
        try
        {
            result = objWebServiceExt.CallGetIViewWS(IvName, iXml, (lvStructure != "" ? lvStructure : StructureXml), this);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Error in GetData => " + ex.Message + "\n\r Call to GetIView Web Service for page no " + pageNo, sessionId, fileName, "");
        }

        string[] resStringArray = result.Split('♠');

        string resString = string.Empty;

        if (resStringArray.Length > 1)
        {
            resString = resStringArray[1];
        }

        string errMsg = objUtil.ParseXmlErrorNode(resString);

        if (errMsg != string.Empty && pageNo > 1)
        {
            return errMsg;
        }
        else
        {
            try
            {
                if (EncryptedColumns.Count > 0)
                {
                    if (resString.StartsWith("<"))
                        resString = objUtil.ReplaceFirstOccurrence(resString, "#$#", "#$♥#");
                    string[] resultSplitter = resString.Split(new[] { "#$♥#" }, StringSplitOptions.None);
                    ASB.WebService objws = new ASB.WebService();
                    resultSplitter[0] = objws.AxpFieldDataDecrypt(resultSplitter[0], EncryptedColumns, "true");
                    resString = string.Join("#$♥#", resultSplitter);
                    string[] _result = result.Split('♠');                    
                    _result[1] = resString;
                    result = string.Join("♠", _result);
                }
            }
            catch (Exception ex)
            { }
            if (resString != string.Empty && errMsg == string.Empty && dataCachingEnabled)
            {
                setCacheData(IvName, purposeString == "" ? paramX : lvXml, pageNo, resString);
            }

            ParamAndLvXml = purposeString == "" ? paramX : lvXmlToProcess;
            return result + "♠" + openRecsPerPage;
        }
    }


    #endregion

    private void setCacheData(string IvName, string paramsKey, int pageNo, string result)
    {
        FDW fdwObj = new FDW();

        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

        string keyAccess = Constants.RedisIvData;

        if (purposeString != "")
        {
            // return;
            keyAccess = Constants.RedisLvData;
        }

        fdwObj.SaveInRedisServer(fObj.MakeKeyName(keyAccess, IvName, HttpContext.Current.Session["username"].ToString(), paramsKey, pageNo), result, keyAccess, HttpContext.Current.Session["dbuser"].ToString());
    }

    public Boolean IsStructureUpdated(string ivUpdateOn, string ivName)
    {
        string isDevInstance = string.Empty;
        if (HttpContext.Current.Session["AxDevInstance"] != null)
            isDevInstance = HttpContext.Current.Session["AxDevInstance"].ToString();

        if (isDevInstance == "true")
        {
            string proj = HttpContext.Current.Session["project"].ToString();
            string sessionID = HttpContext.Current.Session["nsessionid"].ToString();

            System.Globalization.CultureInfo culture = default(System.Globalization.CultureInfo);
            culture = new System.Globalization.CultureInfo("en-GB");
            string errorLog = logobj.CreateLog("Loading Structure.", sessionID, "IsUpdatedIview", "new");

            string dbTimeSql = string.Empty, dbTimeRes = string.Empty, fileName = string.Empty;
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            dbTimeSql = "<sqlresultset axpapp='" + proj + "' sessionid='" + sessionID + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>select updatedon upd from iviews where name ='" + ivName + "' and blobno=1</sql>";

            dbTimeSql += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            fileName = "IviewStructure-" + ivName;
            logobj.CreateLog("Calling DB to check structure is updated.", sessionID, fileName, "");
            //Call service
            dbTimeRes = objWebServiceExt.CallGetChoiceWS(ivName, dbTimeSql);

            if (dbTimeRes.Contains(Constants.ERROR) == true)
            {
                dbTimeRes = dbTimeRes.Replace(Constants.ERROR, "");
                dbTimeRes = dbTimeRes.Replace("</error>", "");
                dbTimeRes = dbTimeRes.Replace("\n", "");
                throw (new Exception(dbTimeRes));
            }
            else
            {
                string dbResult = dbTimeRes.ToUpper();
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode dbUpdatedtime = null;
                xmlDoc.LoadXml(dbResult);
                dbUpdatedtime = xmlDoc.SelectSingleNode("//RESPONSE/ROW/UPD");

                string dbUpdDate = dbUpdatedtime.InnerText;
                DateTime dbDate;
                if (HttpContext.Current.Session["axdb"] != null)
                {
                    if (HttpContext.Current.Session["axdb"].ToString().ToLower() == "ms sql")
                        culture = new System.Globalization.CultureInfo("en-US");
                }


                try
                {
                    dbDate = DateTime.Parse(dbUpdDate, culture);
                }
                catch (System.FormatException ex)
                {

                    dbDate = DateTime.Parse(dbUpdDate);
                }

                DateTime cacheTime;
                string objTime = ivUpdateOn;
                if (object.ReferenceEquals(objTime, ""))
                    objTime = "01/01/2000 00:00:01 AM";

                try
                {
                    cacheTime = DateTime.Parse(objTime, culture);
                }
                catch (System.FormatException ex)
                {
                    cacheTime = DateTime.Parse(objTime);
                }

                logobj.CreateLog("    Cache time is " + cacheTime + ", DB time is " + dbDate + ", transid-" + ivName, sessionID, "GetIvCacheDetails", "");
                return dbDate > cacheTime;
            }
        }
        else
        {
            return false;
        }
    }
    // Code to write properties from xml

    public void GetAxpStructConfig(IviewData iviewObj, string lvFetchSize)
    {

        Dictionary<string, string> TempHypLnkNavigation = new Dictionary<string, string>();
        Dictionary<string, string> TempActBtnNavigation = new Dictionary<string, string>();
        DataTable dsConfig = iviewObj.DsIviewConfig;
        var strConfigProps = dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "navigation")
                   .Select(x => new { hyperLnkName = x.Field<string>("HLINK"), propVal = x.Field<string>("PROPSVAL"), btnProp = x.Field<string>("SBUTTON") }).ToList();
        var strAutoSPlit = dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "autosplit")
                   .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
        if (strAutoSPlit.Count > 0)
        {
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                iviewObj.AxpIsAutoSplit = "false";
            else
                iviewObj.AxpIsAutoSplit = strAutoSPlit[0].splitVal;
        }

        try
        {
            if (lvFetchSize != string.Empty)
            {
                iviewDataWSRows = Convert.ToInt32(lvFetchSize);
            }
            else
            {
                iviewDataWSRows = Convert.ToInt32(dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "fetchsize").Select(x => x.Field<string>("PROPSVAL")).FirstOrDefault());
            }
            iviewDataWSRows = iviewDataWSRows < 1 ? 1000 : iviewDataWSRows;
            GrdPageSize = iviewDataWSRows.ToString();
        }
        catch (Exception ex)
        {
        }

        try
        {
            exportVerticalAlign = dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "exportverticalalign").Select(x => x.Field<string>("PROPSVAL")).FirstOrDefault().ToString();
        }
        catch (Exception ex)
        {
        }

        try
        {
            WebServiceTimeout = Convert.ToInt32(dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "webservice timeout").Select(x => x.Field<string>("PROPSVAL")).FirstOrDefault());
        }
        catch (Exception ex)
        {
        }

        try
        {
            iviewButtonStyle = dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "iview button style").Select(x => x.Field<string>("PROPSVAL")).First();
        }
        catch (Exception ex)
        {
        }

        try
        {
            inputControlType = dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "show control borders").Select(x => x.Field<string>("PROPSVAL")).First().ToLower() == "true" ? "border" : "material";
        }
        catch (Exception ex)
        {
        }

        try
        {
            var loadCustomJs = dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToString().ToLower() == "custom javascript" && x.Field<string>("PROPSVAL").ToString().ToLower() == "true").Select(x => new { applyon = x.Field<string>("APPLYON") }).ToList();
            if (loadCustomJs.Count > 0)
                axpCustomJs = loadCustomJs[0].applyon;
        }
        catch (Exception ex)
        {
        }

        try
        {
            var loadCustomCss = dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToString().ToLower() == "custom css" && x.Field<string>("PROPSVAL").ToString().ToLower() == "true").Select(x => new { applyon = x.Field<string>("APPLYON") }).ToList();
            if (loadCustomCss.Count > 0)
                axpCustomCss = loadCustomCss[0].applyon;
        }
        catch (Exception ex)
        {
        }

        try
        {
            var strDisSPlit = dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "disablesplit")
               .Select(x => new { splitVal = x.Field<string>("PROPSVAL") }).ToList();
            if (strDisSPlit.Count > 0)
                iviewObj.AxpIviewDisableSplit = strDisSPlit[0].splitVal;
            if (strConfigProps.Count > 0)
            {
                for (int i = 0; i < strConfigProps.Count; i++)
                {
                    if (strConfigProps[i].hyperLnkName != null)
                    {
                        if (TempHypLnkNavigation.ContainsKey(strConfigProps[i].hyperLnkName))
                            TempHypLnkNavigation[strConfigProps[i].hyperLnkName] = strConfigProps[i].propVal;
                        else
                            TempHypLnkNavigation.Add(strConfigProps[i].hyperLnkName, strConfigProps[i].propVal);
                    }
                    else if (strConfigProps[i].btnProp != null)
                    {
                        if (TempActBtnNavigation.ContainsKey(strConfigProps[i].btnProp))
                            TempActBtnNavigation[strConfigProps[i].btnProp] = strConfigProps[i].propVal;
                        else
                            TempActBtnNavigation.Add(strConfigProps[i].btnProp, strConfigProps[i].propVal);
                    }
                }
                iviewObj.HypLnkNavigation = TempHypLnkNavigation;
                iviewObj.ActBtnNavigation = TempActBtnNavigation;
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception while getting config data from DB-" + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetAxpStructConfig-" + IName, "new");
        }

        try
        {
            RetainIviewParams = Convert.ToBoolean(dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "iview retain parameters on next load").Select(x => x.Field<string>("PROPSVAL")).FirstOrDefault());
        }
        catch (Exception ex)
        {
        }

        try
        {
            string isc = dsConfig.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "iview session caching").Select(x => x.Field<string>("PROPSVAL")).FirstOrDefault();

            if (isc != null)
            {
                IviewSessionCaching = Convert.ToBoolean(isc);
            }
        }
        catch (Exception ex)
        {
        }
    }
}

