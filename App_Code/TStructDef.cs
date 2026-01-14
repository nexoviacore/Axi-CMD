#region NameSpaces
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Xml;
using System.Text;
using System.IO;
using System.Data;
using System.Linq;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
//using RestSharp;

#endregion

#region TstructDef Class
/// <summary>
/// TStructDef class contains complete TStruct related Data. This also has method to 
/// create the page HTML. The HTML can be generated in DC level. The necessary Javascript
/// arrays are created and passed on the client for client side validation and expression
/// processing.
/// 
/// </summary>
/// 
//  TODO: Dc, Js -> DC, JS
[Serializable()]
public class TStructDef
{
    # region attributes
    // attributes of the tstruct
    public enum moe
    {
        ACCEPT,
        SELECT,
        FILL
    };

    public string EnableOldTheme = "false";
    public string transId = string.Empty;
    public string tstCaption = string.Empty;
    public string tstPform = string.Empty;
    public string tstWorkflow = string.Empty;
    //public string tstRuntimeModification = "F";
    //public string fldRuntimeCriteria = "F";
    public string tstUpdateOn = string.Empty;
    public string tstUpdateBy = string.Empty;
    public string tstReadOnly = string.Empty;
    public string tstCancelled = string.Empty;
    public string tstPdf = string.Empty;
    public string tstVersion = string.Empty;
    public string tstAttachment = string.Empty;
    public string tstIviewButton = string.Empty;
    public Boolean tstHyperLink = false;
    public string tstLayout = string.Empty;
    public string hlpText = string.Empty;
    public string pdfList = string.Empty;
    public bool wsPerfFormLoadCall = true;
    public string wsPerfEvalExpClient = string.Empty;
    public string[] wsPerfFGDcName;
    public string[] wsPerfLDcName;
    public string wsPerfFields = string.Empty;
    public bool wsPerfEnabled = false;
    public ArrayList wsPerfGRDDcs = new ArrayList();
    public string axdesignJson = string.Empty;
    public DesignObject axdesignJObject = new DesignObject();
    public bool isFreshDesign = false;
    int fieldIndex = 0;
    public bool dwbToolbar = false;
    public string tstCachedsave = "F";
    // List with structure
    public ArrayList dcs = new ArrayList();
    public ArrayList dcsPositionIndex = new ArrayList();
    public ArrayList popdcs = new ArrayList();
    public ArrayList flds = new ArrayList();
    public ArrayList btns = new ArrayList();
    public ArrayList taskBtns = new ArrayList();
    public ArrayList fgs = new ArrayList();
    public ArrayList hlnks = new ArrayList();
    public ArrayList actions = new ArrayList();
    public ArrayList customBtns = new ArrayList();
    //public StringBuilder customBtns = new StringBuilder();

    // List without structure
    public ArrayList searchDataNames = new ArrayList();
    public ArrayList searchDataCaptions = new ArrayList();
    public ArrayList customLabels = new ArrayList();
    public ArrayList customLblDcNo = new ArrayList();
    public ArrayList pageActionButtons = new ArrayList();
    public ArrayList pageActionDcNo = new ArrayList();
    public ArrayList dcTable = new ArrayList();

    public ArrayList gridScriptButtons = new ArrayList();
    public ArrayList gridScriptDcNo = new ArrayList();
    public ArrayList gridHeaderScriptButtons = new ArrayList();
    public ArrayList gridHeaderScriptDcNo = new ArrayList();

    //holds the dc's according to the page positioning
    public ArrayList pagePositions = new ArrayList();
    public ArrayList visibleDCs = new ArrayList();
    public ArrayList tabDCs = new ArrayList(50);
    public ArrayList tabDCStatus = new ArrayList(50);

    // javascript array attributes
    public string jsFormcontrolArray = string.Empty;
    public string jsHyperlinkArray = string.Empty;
    public StringBuilder tabScript = new StringBuilder();
    public StringBuilder jsPatternArray = new StringBuilder();
    public StringBuilder jsFieldArray = new StringBuilder();
    public StringBuilder jsDCArray = new StringBuilder();
    public StringBuilder jsTabDCArray = new StringBuilder();
    public StringBuilder jsExpressionArray = new StringBuilder();
    public StringBuilder gridHeadHtml = new StringBuilder();
    public StringBuilder dummyGridHeadHtml = new StringBuilder();
    public StringBuilder gridHiddenHtml = new StringBuilder();
    public StringBuilder customLabelHtml = new StringBuilder();
    public StringBuilder btnHtml = new StringBuilder();
    public StringBuilder dcOtherRows = new StringBuilder();
    public string jsRuleDefArray = string.Empty;
    public string strRulesDefEngin = string.Empty;
    //public StringBuilder headerHtml = new StringBuilder();
    public Boolean IsObjFromCache = false;
    public Boolean IsObjCustomHtml = false;
    public ArrayList fldDcRange = new ArrayList();
    public int colCount = 0;
    //this is old ifdesignmode flag.. do not change
    public Boolean IsDesignMode = false;

    //Dependency arrays
    public ArrayList childArray = new ArrayList();
    public ArrayList parentArray = new ArrayList();
    public ArrayList dependentsArray = new ArrayList();

    public ArrayList subTotColValues = new ArrayList();
    private int fgCount = 0;
    public string srchCols = string.Empty;
    public string grpActBtns = string.Empty;
    public ArrayList grpActBtnsList = new ArrayList();
    public string refreshSelect = string.Empty;
    public string htmlToPDF = string.Empty;
    //Fastdata variables
    public string fastDataFlds = string.Empty;
    public string fastDataDynamicFlds = string.Empty;
    public string fastDataActiveFlds = string.Empty;
    public ArrayList fastDataRefDSName = new ArrayList();
    public ArrayList fastDataRefEvent = new ArrayList();
    public DataSet dsFastDataDef = new DataSet();
    public List<string> lstGlobalParams = new List<string>();

    public bool isFastFormLoad = false;
    //public string formLoadCache = String.Empty;
    public bool isWizardTstruct = false;
    public bool hideToolBar = false;

    //Variables for Rapid performance changes- EPI
    public DataTable dt_RapidFormControl = new DataTable();
    public DataTable dt_RapidDependents = new DataTable();
    public DataTable dt_RapidDependentsTemp = new DataTable();
    public bool rapidIsClientFormLoad = false;
    public bool rapidDoFormLoad = true;
    public string rapidFormLoadExpFlds = string.Empty;
    public List<string> rapidFLExpFlds = new List<string>();
    public string rapidFormLoadJSON = string.Empty;
    public string rapidGlobalParams = string.Empty;
    public string rapidFormLoadActionXML = string.Empty;

    public string fromListFlds = string.Empty;
    public ArrayList AxImageFields = new ArrayList();
    public ArrayList AxAttachFields = new ArrayList();
    public ArrayList AxpFileUploadFields = new ArrayList();
    Util.Util utilObj = new Util.Util();
    public Dictionary<string, int> AxRowLimit = new Dictionary<string, int>();
    public string FormLoadGlobalVarNode = string.Empty;
    public string AxMemVarFunction = string.Empty;
    public bool AxMemVarAvailable = false;

    public StringBuilder ArrFFieldHidden = new StringBuilder();
    public StringBuilder ArrFFieldReadOnly = new StringBuilder();

    public string fld_axp_latlong = string.Empty;
    public string AxpTstButtonStyleDef = "classic";
    public ArrayList SaveScriptList = new ArrayList();
    public ArrayList FEncryptFlag = new ArrayList();
    public ArrayList tstCommonSessList = new ArrayList();

    // Global variables
    [NonSerialized]
    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
    int exprNo = 0;
    int fldNo = 0;
    int DCNo = 0;
    int popDCNo = 0;
    int grfldNo = 0;
    int patternNo = 0;
    int lastDcHeight = 0;
    int dcArrayNo = 0;
    int gridDcColwidth = 0;
    int maxDcHeight = 0;
    int dcNo = 0;
    int DcCnt = 0;
    string dcRange = "";
    int fldGlobal = 0;
    int dcFldCount = 0;
    int dcFldVisibleCount = 0;
    string defaultDateStr = "dd/mm/yyyy";
    string defaultDateTimeStr = "dd/mm/yyyy hh:mm:ss";
    string defaultTimeStr = "HH:MM";
    string defaultColWidth = "50";
    string res = string.Empty;
    //If this property comes in Grid DC from xml, showZeroForNumeric variable should be updated.
    Boolean showZeroForNumeric = true;
    Boolean isFillGridCall = false;
    public Boolean isAxpImagePath = false;
    public Boolean ContainsImage = false;
    public Boolean ContainsGridAttach = false;
    public Boolean ContainsGridRefer = false;
    string tabFunction = string.Empty;
    public string browser = string.Empty;
    string fldCss = string.Empty;
    string fldHdnCss = string.Empty;
    string ActiveKeyColValue = string.Empty;
    public int docHeight = 0;
    public int left = 0;
    public int top = 0;
    Custom cust = Custom.Instance;
    bool flgEnblPurpose = true;

    public string isPerfCode = "false";
    public bool AlwaysServer = true;
    public string axp_inmemTids = string.Empty;
    public string axp_inmemKeys = string.Empty;

    //DesignMode IDs
    public string save_id = string.Empty;
    public string Publish_id = string.Empty;
    public string Is_Publish = "N";
    public string dcPlistaddrow = string.Empty;
    public string dcExpandCollapse = string.Empty;
    public string dcDefaultstate = string.Empty;
    Boolean IsPublish = false;
    bool isMobile = false;
    public ArrayList fldSetCarry = new ArrayList();
    public ArrayList fldAcceptFromApi = new ArrayList();
    private int webServiceTimeout = 100000;
    public int actNo = 0;
    public bool isfldDcGrid = false;
    public int WebServiceTimeout
    {
        get { return webServiceTimeout > 0 ? webServiceTimeout : 100000; }
        set { webServiceTimeout = value; }
    }
    public string listofRagVarfld = string.Empty;
    public string AxTstIsPEG = "false";
    #endregion

    #region Constructor

    /// <summary>
    /// Constructor which has the xml data.
    /// </summary>
    /// <param name="xml"></param>
    public TStructDef(string xml, DataTable customDS, DataSet dsRapidDef)
    {
        if (HttpContext.Current.Session != null)
            EnableOldTheme = HttpContext.Current.Session["AxEnableOldTheme"].ToString().ToLower();

        if (HttpContext.Current.Session["AxImagePath"] != null && HttpContext.Current.Session["AxImagePath"].ToString() != "")
            isAxpImagePath = true;

        if (HttpContext.Current.Session["ClientLocale"] != null && HttpContext.Current.Session["ClientLocale"].ToString() == "en-us")
        {
            defaultDateStr = "mm/dd/yyyy";
            defaultDateTimeStr = "mm/dd/yyyy hh:mm:ss";
        }
        if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString().ToLower() == "true")
            isMobile = true;

        ParseSearchAndGroups(customDS);
        this.structRes = xml;
        this.Create(xml);
        if (!string.IsNullOrEmpty(tstVersion) && Int64.Parse(tstVersion) > 10000)
        {
            if (dsRapidDef != null && dsRapidDef.Tables.Count > 1)
            {
                ParseRapidDef(dsRapidDef);
                SetRapidExpFlds();
            }
        }

        if (HttpContext.Current.Session["AxIsPerfCode"] != null && !utilObj.IsNullOrEmpty(HttpContext.Current.Session["AxIsPerfCode"].ToString()))
            isPerfCode = HttpContext.Current.Session["AxIsPerfCode"].ToString();
    }

    private void ParseRapidDef(DataSet dsRapidDef)
    {
        try
        {
            dt_RapidFormControl = dsRapidDef.Tables[0];
            dt_RapidDependents = dsRapidDef.Tables[1];
            dt_RapidDependentsTemp = dt_RapidDependents.Clone();
            SetRapidFormLoadProps();
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in ParseRapidDef-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exc-ParseRapidDef", "new");
        }
    }



    /// <summary>
    /// Function to get the Dependents, and their type from axp_Dependents table
    /// </summary>
    /// <param name="fld"></param>
    /// <returns></returns>
    /*private FieldStruct SetRapidDependents(FieldStruct fld)
    {
        try
        {
            if (dt_RapidDependents != null && dt_RapidDependents.Rows.Count > 0)
            {
                var strDeps = dt_RapidDependents.AsEnumerable().Where(x => x.Field<string>("parentfield") == fld.name).OrderBy(x => x.Field<Int16>("frmno")).ThenBy(x => x.Field<Int16>("ordno"))
                    .Select(x => new { dependentFld = x.Field<string>("dependenttype") + x.Field<string>("childfield"), allRows = x.Field<string>("allrows") }).Distinct().ToList();

                fld.FldRapidDeps = string.Join(",", strDeps.Select(x => x.dependentFld).ToArray());

                var strExpDeps = dt_RapidDependents.AsEnumerable().Where(x => x.Field<string>("parentfield") == fld.name && x.Field<string>("dependenttype") == "e").OrderBy(x => x.Field<Int16>("frmno")).Select(x => x.Field<string>("dependenttype") + x.Field<string>("childfield")).Distinct().ToList();
                fld.FldRapidExpDeps = string.Join(",", strExpDeps.Select(x => x.ToString()).ToArray());
                var strParents = dt_RapidDependents.AsEnumerable().Where(x => x.Field<string>("parentfield") == fld.name && x.Field<string>("dirparam") == "T")
                    .Select(x => new { parent = x.Field<string>("paramname"), allRows = x.Field<string>("allrows") }).Distinct().ToList();

                fld.FldRapidParents = string.Join(",", strParents.Select(x => x.parent).ToArray());
                fld.FldRapidAllRows = string.Join(",", strParents.Select(x => x.allRows).ToArray());
                //Loop through dependents and their parents
                List<string> strParFlds = new List<string>();
                strParFlds.AddRange(strParents.Select(x => x.parent).ToList());
                for (int i = 0; i < strDeps.Count; i++)
                {
                    var strAllPars = dt_RapidDependents.AsEnumerable().Where(x => x.Field<string>("parentfield") == strDeps[i].dependentFld.Substring(1))
                    .Select(x => x.Field<string>("paramname")).Distinct().ToList();
                    strParFlds.AddRange(strAllPars.Select(x => x.ToString()).ToList());
                }

                strParFlds = strParFlds.Distinct().ToList();
                //Loop through the parents and their parents
                List<string> strParParFlds = new List<string>();
                for (int j = 0; j < strParFlds.Count; j++)
                {
                    var strAllPars = dt_RapidDependents.AsEnumerable().Where(x => x.Field<string>("parentfield") == strParFlds[j].ToString()
                    && !string.IsNullOrEmpty(x.Field<string>("paramname"))).OrderBy(x => x.Field<Int16>("frmno"))
                      .Select(x => x.Field<string>("paramname")).Distinct().ToList();
                    strParParFlds.AddRange(strAllPars.Select(x => x.ToString()).ToList());
                    //strParParFlds.AddRange(strAllPars.Select(x => tranform(x)).Where(y => y != null).ToString().ToList();
                }

                List<string> fldAllParents = new List<string>();
                fldAllParents.AddRange(strParFlds);
                fldAllParents.AddRange(strParParFlds.Distinct().ToList());
                fldAllParents = fldAllParents.Distinct().ToList();

                fld.FldRapidAllParents = string.Join(",", fldAllParents.Select(x => x.ToString()).Distinct().ToArray());

                //var strAllParents = dt_RapidDependents.AsEnumerable().Where(x => x.Field<string>("parentfield") == fld.name)
                //   .Select(x => new { parent = x.Field<string>("paramname"), allRows = x.Field<string>("allrows") }).Distinct().ToList();
                //fld.FldRapidAllParents = string.Join(",", strAllParents.Select(x => x.parent).ToArray());               

                if (fld.FldRapidDeps == fld.FldRapidExpDeps)
                    fld.FldRapidDepType = "c";
                else
                    fld.FldRapidDepType = "w";
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in SetRapidDependents-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exc-SetRapidDep", "new");
        }

        return fld;
    }*/

    private FieldStruct SetRapidDependents(FieldStruct fld)
    {
        try
        {
            if (dt_RapidDependents != null && dt_RapidDependents.Rows.Count > 0)
            {
                dt_RapidDependentsTemp.Clear();
                if (dt_RapidDependents.Select("parentfield ='" + fld.name + "'").Length > 0)
                    dt_RapidDependentsTemp = dt_RapidDependents.Select("parentfield ='" + fld.name + "'").CopyToDataTable();

                if (dt_RapidDependentsTemp.Rows.Count > 0)
                {
                    var strDeps = dt_RapidDependentsTemp.AsEnumerable().Select(x => new { dependentFld = x.Field<string>("dependenttype") + x.Field<string>("childfield"), allRows = x.Field<string>("allrows") }).Distinct().ToList();
                    fld.FldRapidDeps = string.Join(",", strDeps.Select(x => x.dependentFld).ToArray());

                    var strExpDeps = dt_RapidDependentsTemp.AsEnumerable().Where(x => x.Field<string>("dependenttype") == "e").Select(x => x.Field<string>("dependenttype") + x.Field<string>("childfield")).Distinct().ToList();
                    fld.FldRapidExpDeps = string.Join(",", strExpDeps.Select(x => x.ToString()).ToArray());

                    var strParents = dt_RapidDependentsTemp.AsEnumerable().Where(x => x.Field<string>("dirparam") == "T")
                        .Select(x => new { parent = x.Field<string>("paramname"), allRows = x.Field<string>("allrows") }).Distinct().ToList();
                    fld.FldRapidParents = string.Join(",", strParents.Select(x => x.parent).ToArray());
                    fld.FldRapidAllRows = string.Join(",", strParents.Select(x => x.allRows).ToArray());

                    fld.FldRapidAllParents = string.Join(",", dt_RapidDependentsTemp.AsEnumerable().Select(x => x.Field<string>("paramname")).Distinct().ToArray());

                    if (fld.FldRapidDeps == fld.FldRapidExpDeps)
                        fld.FldRapidDepType = "c";
                    else
                        fld.FldRapidDepType = "w";
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in SetRapidDependents-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exc-SetRapidDep", "new");
        }

        return fld;
    }

    private void SetRapidFormLoadProps()
    {
        try
        {
            if (dt_RapidFormControl.Rows.Count == 0)
            {
                rapidDoFormLoad = false;
                rapidFormLoadJSON = string.Empty;
            }
            else
            {

                string strRapidDefs; string strRapidExpFlds;
                var rapidDefs = dt_RapidFormControl.AsEnumerable().Where(x => x.Field<string>("context") == "new")
                    .Select(r => new { fieldname = r.Field<string>("fieldname"), paramname = r.Field<string>("paramname") }).ToList();
                rapidGlobalParams = string.Join(",", rapidDefs.Select(x => x.paramname).Distinct().ToArray());
                strRapidDefs = string.Join(",", rapidDefs.Select(x => x.fieldname).ToArray());
                var rapidExpFlds = dt_RapidFormControl.AsEnumerable().Where(x => x.Field<string>("context") == "new"
                        && (string.IsNullOrWhiteSpace(x.Field<string>("fldsql")) || string.IsNullOrEmpty(x.Field<string>("fldsql"))) && !string.IsNullOrEmpty(x.Field<string>("expression"))).Select(r => r.Field<string>("fieldname")).ToList();
                strRapidExpFlds = string.Join(",", rapidExpFlds.Select(x => x.ToString()).ToArray());
                rapidFLExpFlds = rapidExpFlds;
                if (rapidDefs.Count == 0 || strRapidExpFlds == strRapidDefs)
                    rapidIsClientFormLoad = true;

            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in SetRapidFormLoadProps-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exc-SetRapidFormLoadProps", "new");
        }
    }

    private void ParseSearchAndGroups(DataTable customDS)
    {
        if (customDS != null)
        {
            if (customDS.Rows.Count > 0)
            {
                try
                {
                    foreach (DataRow dr in customDS.Rows)
                    {
                        if (dr["configname"].ToString() == "searchcols")
                        {
                            srchCols = dr["cvalue"].ToString();
                        }
                        else if (dr["configname"].ToString() == "groupbtns")
                        {
                            grpActBtns = dr["cvalue"].ToString();
                            if (grpActBtns != "")
                            {
                                try
                                {
                                    String[] btnGroups = grpActBtns.Split('~');
                                    for (int i = 0; i < btnGroups.Length; i++)
                                    {
                                        string[] seperateButton = (btnGroups[i].Split('-')[1]).ToString().Split(',');
                                        for (int j = 0; j < seperateButton.Length; j++)
                                        {
                                            grpActBtnsList.Add(seperateButton[j]);
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                        else if (dr["configname"].ToString().ToLower() == "refreshselect")
                        {
                            refreshSelect = dr["cvalue"].ToString();
                        }
                        else if (dr["configname"].ToString().ToLower() == "printform")
                        {
                            htmlToPDF = dr["cvalue"].ToString();
                            HttpContext.Current.Session["htmltopdfParams"] = dr["cvalue"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFile.Log logObj = new LogFile.Log();
                    logObj.CreateLog("Exception in ParseSearchAndGroups-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exc-ParseSearchAndGroups", "new");
                }
            }
        }
    }

    private bool CheckpurposeFld()
    {
        bool resPurpose = false;
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string dbType = string.Empty;
        if (!string.IsNullOrEmpty(HttpContext.Current.Session["axdb"].ToString()))
            dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
        string sql = string.Empty;

        sql = "Select ( CASE WHEN (Select Value  from axpCloudDevSettings where ID='" + HttpContext.Current.Session["user"] + "' and type=\'axPurpose\') IS NULL THEN 0 else (Select Value  from axpCloudDevSettings where ID='" + HttpContext.Current.Session["user"] + "' and type=\'axPurpose\') end ) AS admn,(CASE WHEN (Select Value  from axpCloudUserSettings where USERID='" + HttpContext.Current.Session["user"] + "' and \'axPurpose\') IS NULL THEN 0 else (Select Value  from axpCloudUserSettings where USERID='" + HttpContext.Current.Session["user"] + "' and type=\'axPurpose\')end ) AS usr " + (dbType == "oracle" ? "from dual" : "");
        //sql = "select * from axpCloudDevSettings where ID='" + HttpContext.Current.Session["user"] + "'";
        string result = objCWbSer.GetChoicesCustomWS(transId, sql);
        if (result != string.Empty)
        {
            DataSet ds = new DataSet();
            StringReader sr = new StringReader(result);
            ds.ReadXml(sr);
            DataTable dt = ds.Tables["row"];
            if (dt != null && dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["admn"].ToString()))
                {
                    int adminValue = Convert.ToInt32(dt.Rows[0]["admn"]);
                    int userValue = Convert.ToInt32(dt.Rows[0]["usr"]);
                    if (adminValue > 0 && userValue > 0)
                        resPurpose = true;
                    else if (adminValue <= 0 && userValue > 0)
                        resPurpose = true;
                    else if (adminValue > 0 && userValue <= 0)
                        resPurpose = false;
                    else if (adminValue <= 0 && userValue <= 0)
                        resPurpose = false;
                }
                else
                    resPurpose = false;
            }
        }
        return resPurpose;
    }
    #endregion

    # region DataMembers with get set properties

    public ArrayList lstDc1
    {
        get { return dcs; }
        set { dcs = value; }
    }

    public ArrayList lstPopDc
    {
        get { return popdcs; }
        set { popdcs = value; }
    }

    public ArrayList lstField
    {
        get { return flds; }
        set { flds = value; }
    }

    public ArrayList lstButton
    {
        get { return btns; }
        set { btns = value; }
    }

    public ArrayList lstFillgrid
    {
        get { return fgs; }
        set { fgs = value; }
    }

    public ArrayList lstHyperLink
    {
        get { return hlnks; }
        set { hlnks = value; }
    }

    public ArrayList lstSearchDataNames
    {
        get { return searchDataNames; }
        set { searchDataNames = value; }
    }

    public ArrayList lstSearchDataCaptions
    {
        get { return searchDataCaptions; }
        set { searchDataCaptions = value; }
    }

    public ArrayList lstActions
    {
        get { return actions; }
        set { actions = value; }
    }

    public string structRes
    {
        get { return res; }
        set { res = value; }
    }

    public Boolean ShowZeroForNumeric
    {
        get { return showZeroForNumeric; }
        set { showZeroForNumeric = value; }
    }

    public Boolean IsFillGridCall
    {
        get { return isFillGridCall; }
        set { isFillGridCall = value; }
    }

    public string HlpText
    {
        get { return hlpText; }
        set { hlpText = value; }
    }
    #endregion

    # region Structures
    /// <summary>
    /// Structure for Dc data.
    /// </summary>
    [Serializable]
    public struct DcStruct
    {
        string dcName;
        string dcCaption;
        int dcNo;
        int fldCount;
        int visFldCount;
        bool dcIsGrid;
        bool dcIsPopGrid;
        int dcHgt;
        int dcWdt;
        bool dcIsAllowAddRows;
        bool dcIsAllowchange;
        bool dcIsAllowDeleteRows;
        bool dcIsFormatGrid;
        bool showAddBtn;
        bool dcHasDataRows;
        bool dcAllowEmpty;
        string keyCol;
        bool multiSelect;
        string sql;
        string hideColumns;
        string mapColumn;
        string mapParColumn;
        ArrayList subTotalCols;
        string displayKeyCol;
        string displayTotCol;
        string headerStyle;
        string subTotalStyle;
        string action;
        string dcPurpose;
        ArrayList dispKeyColValues;
        ArrayList keyColumnValues;
        ArrayList tabParentFlds;
        StringBuilder dcExpDependents;
        string dcAcceptMRFields;
        string dcPlist;
        string DCblean;
        string DCDefaultstate;

        public string DcBlean
        {
            get { return DCblean; }
            set { DCblean = value; }
        }
        public string DcDefaultstate
        {
            get { return DCDefaultstate; }
            set { DCDefaultstate = value; }
        }
        public string dcPList
        {
            get { return dcPlist; }
            set { dcPlist = value; }
        }

        public int dcHeight
        {
            get { return dcHgt; }
            set { dcHgt = value; }
        }

        public int dcWidht
        {
            get { return dcWdt; }
            set { dcWdt = value; }
        }

        public int fieldCount
        {
            get { return fldCount; }
            set { fldCount = value; }
        }
        public int visibleFldCount
        {
            get { return visFldCount; }
            set { visFldCount = value; }
        }

        public string name
        {
            get { return dcName; }
            set { dcName = value; }
        }

        public string caption
        {
            get { return dcCaption; }
            set { dcCaption = value; }
        }

        public int frameno
        {
            get { return dcNo; }
            set { dcNo = value; }
        }

        public bool isgrid
        {
            get { return dcIsGrid; }
            set { dcIsGrid = value; }
        }

        public bool DCHasDataRows
        {
            get { return dcHasDataRows; }
            set { dcHasDataRows = value; }
        }

        public bool DcAllowEmpty
        {
            get { return dcAllowEmpty; }
            set { dcAllowEmpty = value; }
        }

        public bool ispopgrid
        {
            get { return dcIsPopGrid; }
            set { dcIsPopGrid = value; }
        }
        public bool isallowaddrows
        {
            get { return dcIsAllowAddRows; }
            set { dcIsAllowAddRows = value; }
        }
        public bool isallowchange
        {
            get { return dcIsAllowchange; }
            set { dcIsAllowchange = value; }
        }
        public bool isallowdeletrows
        {
            get { return dcIsAllowDeleteRows; }
            set { dcIsAllowDeleteRows = value; }
        }

        public bool IsFormatGrid
        {
            get { return dcIsFormatGrid; }
            set { dcIsFormatGrid = value; }
        }
        public bool MultiSelect
        {
            get { return multiSelect; }
            set { multiSelect = value; }
        }
        public bool ShowAddBtn
        {
            get { return showAddBtn; }
            set { showAddBtn = value; }
        }
        public string KeyColumn
        {
            get { return keyCol; }
            set { keyCol = value; }
        }
        public string Sql
        {
            get { return sql; }
            set { sql = value; }
        }
        public string HideColumns
        {
            get { return hideColumns; }
            set { hideColumns = value; }
        }
        public string Action
        {
            get { return action; }
            set { action = value; }
        }
        public string MapColumn
        {
            get { return mapColumn; }
            set { mapColumn = value; }
        }
        public string MapParentCol
        {
            get { return mapParColumn; }
            set { mapParColumn = value; }
        }
        public ArrayList SubTotalColumns
        {
            get { return subTotalCols; }
            set { subTotalCols = value; }
        }
        public string DisplayKeyCol
        {
            get { return displayKeyCol; }
            set { displayKeyCol = value; }
        }
        public string DisplayTotalCol
        {
            get { return displayTotCol; }
            set { displayTotCol = value; }
        }
        public string HeaderStyle
        {
            get { return headerStyle; }
            set { headerStyle = value; }
        }
        public string SubTotalStyle
        {
            get { return subTotalStyle; }
            set { subTotalStyle = value; }
        }
        public string DCAcceptMRFields
        {
            get { return dcAcceptMRFields; }
            set { dcAcceptMRFields = value; }
        }

        public string purpose
        {
            get { return dcPurpose; }
            set { dcPurpose = value; }
        }

        public ArrayList DispKeyColValues
        {
            get { return dispKeyColValues; }
            set { dispKeyColValues = value; }
        }
        public ArrayList KeyColValues
        {
            get { return keyColumnValues; }
            set { keyColumnValues = value; }
        }

        public ArrayList tabParentFields
        {
            get { return tabParentFlds; }
            set { tabParentFlds = value; }
        }

        public StringBuilder dcExpressionDeps
        {
            get { return dcExpDependents; }
            set { dcExpDependents = value; }
        }
    }

    /// <summary>
    /// Structure for PopDc data.
    /// </summary>
    [Serializable]
    public struct PopDcStruct
    {

        string dcCaption;
        int dcNo;
        int pDcNo;
        string pFields;
        string popField;
        string popFirm;
        string popAuto;
        string popKeyCols;
        string displaySummaryField;
        string summaryParent;
        string delimiter;
        string sqlFill;
        string popCond;

        public string caption
        {
            get { return dcCaption; }
            set { dcCaption = value; }
        }

        public int frameno
        {
            get { return dcNo; }
            set { dcNo = value; }
        }

        public int pdcno
        {
            get { return pDcNo; }
            set { pDcNo = value; }
        }

        public string pfields
        {
            get { return pFields; }
            set { pFields = value; }
        }

        public string popfield
        {
            get { return popField; }
            set { popField = value; }
        }

        public string pFirm
        {
            get { return popFirm; }
            set { popFirm = value; }
        }

        public string pAuto
        {
            get { return popAuto; }
            set { popAuto = value; }
        }

        public string pKeyCols
        {
            get { return popKeyCols; }
            set { popKeyCols = value; }
        }

        public string dispSummaryField
        {
            get { return displaySummaryField; }
            set { displaySummaryField = value; }
        }

        public string dispSummaryParent
        {
            get { return summaryParent; }
            set { summaryParent = value; }
        }

        public string summaryDelimiter
        {
            get { return delimiter; }
            set { delimiter = value; }
        }

        public string fillSql
        {
            get { return sqlFill; }
            set { sqlFill = value; }
        }

        public string PopCondition
        {
            get { return popCond; }
            set { popCond = value; }
        }

    }

    /// <summary>
    /// Structure for Action data.
    /// </summary>
    [Serializable]
    public struct ActionStruct
    {
        string actName;
        string actCap;
        string actDesc;
        string actApply;
        string actApplyOn;
        string actStrName;
        string actValidate;
        string actWeb;
        string actRemarks;
        string manRemarks;
        string actParentRefresh;
        string actsavetask;
        string actscripttask;
        string actscriptcancel;
        string ScriptActive;
        string PushtoQueue;
        string QueueName;

        public string actname
        {
            get { return actName; }
            set { actName = value; }
        }

        public string actcap
        {
            get { return actCap; }
            set { actCap = value; }
        }

        public string actdesc
        {
            get { return actDesc; }
            set { actDesc = value; }
        }

        public string actapply
        {
            get { return actApply; }
            set { actApply = value; }
        }

        public string actapplyon
        {
            get { return actApplyOn; }
            set { actApplyOn = value; }
        }

        public string actstrname
        {
            get { return actStrName; }
            set { actStrName = value; }
        }

        public string actvalidate
        {
            get { return actValidate; }
            set { actValidate = value; }
        }

        public string actweb
        {
            get { return actWeb; }
            set { actWeb = value; }
        }

        public string actRem
        {
            get { return actRemarks; }
            set { actRemarks = value; }
        }

        public string manRem
        {
            get { return manRemarks; }
            set { manRemarks = value; }
        }
        public string actParRefresh
        {
            get { return actParentRefresh; }
            set { actParentRefresh = value; }
        }
        public string actSaveTask
        {
            get { return actsavetask; }
            set { actsavetask = value; }
        }
        public string actScriptTask
        {
            get { return actscripttask; }
            set { actscripttask = value; }
        }
        public string actScriptCancel
        {
            get { return actscriptcancel; }
            set { actscriptcancel = value; }
        }
        public string scriptActive
        {
            get { return ScriptActive; }
            set { ScriptActive = value; }
        }
        public string pushtoQueue
        {
            get { return PushtoQueue; }
            set { PushtoQueue = value; }
        }

        public string queueName
        {
            get { return QueueName; }
            set { QueueName = value; }
        }
    }

    /// <summary>
    /// Structure for field data.
    /// </summary>
    [Serializable]
    public struct FieldStruct
    {
        string fldName;
        string fldCaption;
        string fldDatatype;
        string fldTableVal;
        string tableTypSql;
        string fldModeofEntry;
        string fldExpression;
        string fldValidateExpression;
        string fldTLHW;
        string fldChecktype;
        string fldAllignType;
        string fldColumnWidth;
        string fldLabelTLHW;
        string fldPattern;
        string fldPasswordCharacter;
        string fldApplyComma;
        string fldSqlList;
        string fldExpressionList;
        string fldFillList;
        string fldFillGridList;
        string fldPopDepList;
        string fldAutoSelect;
        string fldRefreshOnSave;
        string fldSourceField;
        string fldSqlField;
        string isSqlFld;
        string isApiFld;
        string fldSqlQuery;
        string fldSelAPI;
        string fieldSelApiParams;
        string fieldSelApiName;
        string fldSelectMode;
        string fldMoeValue;
        string fldTooltip;
        string fldDependents;
        string fldSqlDeps;
        string fldExpDeps;
        string fldGridSqlDeps;
        string fldSqlDepParents;
        string fldDepGridDcs;
        string fldDepFillDcs;
        string fldParents;
        string clientFldParents;
        string fldHint;
        string fldPurpose;//mAdd
        //Events for the field
        string fldOnFocus;
        string fldOnBlur;
        string fldOnKeyDown;
        string fldOnKeyUp;
        string fldOnClick;
        string fldOnDbClick;
        string fldOnKeyPress;
        string fldOnChange;
        string fldValue;
        string masterRow;
        string fldComboVals;
        string fldIdCol;
        string fldLblCss;
        string fldTxtCss;
        string fldValSeparator;
        string fldBgColor;
        ArrayList advSrchFlds;
        ArrayList advSrchFldsType;
        string pLabelTLHW;
        string pwfldtlhw;

        public int fldFrameNo;
        int fldLength;
        int fldDecimalPlaces;
        int fldParamCount;
        int fldSelectRefresh;
        int fldOrder;
        bool fldIsVisibility;
        bool fldIsAllowEmpty;
        bool fldIsReadonly;
        bool fldIsSaveNormal;
        bool fldIsEditCombo;
        bool fldIsSelectFireSql;
        bool fldIsSaveValue;
        bool fldIsAllowDuplicate;
        bool fldIsImage;
        bool fldIsFromList;
        bool fldIsOnlyPositive;
        bool fldIsTabStop;

        string fldRapidDeps;
        string fldRapidExpDeps;
        string fldRapidDepType;
        string fldRapidParents;
        string fldRapidAllParents;
        string fldRapidAllRows;
        int fieldindex;
        string fldmultiselect;
        string fldmultiselectsep;
        string fldtype;
        string fldacceptapi;
        string fldtimeformat;
        string fldIntelli;
        string fldFillfromapi;

        int pwdMinChar;
        bool pwdAlphaNumeric;
        int pwdNodExpiry;

        string fldmasktype;
        string fldmaskdetails;
        bool fldCustomDecimal;
        bool fldNumDisplayTotal;
        string fldEncryptFlag;
        public string OnFocus
        {
            get { return fldOnFocus; }
            set { fldOnFocus = value; }
        }

        public string OnBlur
        {
            get { return fldOnBlur; }
            set { fldOnBlur = value; }
        }

        public string OnKeyDown
        {
            get { return fldOnKeyDown; }
            set { fldOnKeyDown = value; }
        }

        public string OnKeyUp
        {
            get { return fldOnKeyUp; }
            set { fldOnKeyUp = value; }
        }
        public string OnClick
        {
            get { return fldOnClick; }
            set { fldOnClick = value; }
        }
        public string OnDbClick
        {
            get { return fldOnDbClick; }
            set { fldOnDbClick = value; }
        }
        public string OnKeyPress
        {
            get { return fldOnKeyPress; }
            set { fldOnKeyPress = value; }
        }
        public string OnChange
        {
            get { return fldOnChange; }
            set { fldOnChange = value; }
        }

        public string name
        {
            get { return fldName; }
            set { fldName = value; }
        }

        public string caption
        {
            get { return fldCaption; }
            set { fldCaption = value; }
        }

        public string encryptflag
        {
            get { return fldEncryptFlag; }
            set { fldEncryptFlag = value; }
        }

        public string datatype
        {
            get { return fldDatatype; }
            set { fldDatatype = value; }
        }

        public bool CustomDecimal
        {
            get { return fldCustomDecimal; }
            set { fldCustomDecimal = value; }
        }

        public bool NumDisplayTotal
        {
            get { return fldNumDisplayTotal; }
            set { fldNumDisplayTotal = value; }
        }

        public string tabletypeval
        {
            get { return fldTableVal; }
            set { fldTableVal = value; }
        }

        public string tabletypsql
        {
            get { return tableTypSql; }
            set { tableTypSql = value; }
        }

        public string moe
        {
            get { return fldModeofEntry; }
            set { fldModeofEntry = value; }
        }

        public string expr
        {
            get { return fldExpression; }
            set { fldExpression = value; }
        }

        public string vexpr
        {
            get { return fldValidateExpression; }
            set { fldValidateExpression = value; }
        }

        public string fldtlhw
        {
            get { return fldTLHW; }
            set { fldTLHW = value; }
        }

        public string lbltlhw
        {
            get { return fldLabelTLHW; }
            set { fldLabelTLHW = value; }
        }

        public string plbltlhw
        {
            get { return pLabelTLHW; }
            set { pLabelTLHW = value; }
        }

        public string pfldtlhw
        {
            get { return pwfldtlhw; }
            set { pwfldtlhw = value; }
        }
        public string pattern
        {
            get { return fldPattern; }
            set { fldPattern = value; }
        }

        public string pwdchar
        {
            get { return fldPasswordCharacter; }
            set { fldPasswordCharacter = value; }
        }

        public int pwdminchar
        {
            get { return pwdMinChar; }
            set { pwdMinChar = value; }
        }

        public bool pwdalphanumeric
        {
            get { return pwdAlphaNumeric; }
            set { pwdAlphaNumeric = value; }
        }

        public int pwdnodexpiry
        {
            get { return pwdNodExpiry; }
            set { pwdNodExpiry = value; }
        }

        public string applycomma
        {
            get { return fldApplyComma; }
            set { fldApplyComma = value; }
        }

        public string fieldHint
        {
            get { return fldHint; }
            set { fldHint = value; }
        }
        public string fieldPurpose
        {
            get { return fldPurpose; }
            set { fldPurpose = value; }
        }

        public string slist
        {
            get { return fldSqlList; }
            set { fldSqlList = value; }
        }

        public string elist
        {
            get { return fldExpressionList; }
            set { fldExpressionList = value; }
        }

        public string flist
        {
            get { return fldFillList; }
            set { fldFillList = value; }
        }

        public string sflist
        {
            get { return fldPopDepList; }
            set { fldPopDepList = value; }
        }

        public string fglist
        {
            get { return fldFillGridList; }
            set { fldFillGridList = value; }
        }

        public string ctype
        {
            get { return fldChecktype; }
            set { fldChecktype = value; }
        }

        public string type
        {
            get { return fldAllignType; }
            set { fldAllignType = value; }
        }

        public string cwid
        {
            get { return fldColumnWidth; }
            set { fldColumnWidth = value; }
        }

        public string selectmode
        {
            get { return fldSelectMode; }
            set { fldSelectMode = value; }
        }

        public string autoselect
        {
            get { return fldAutoSelect; }
            set { fldAutoSelect = value; }
        }

        public string refreshOnSave
        {
            get { return fldRefreshOnSave; }
            set { fldRefreshOnSave = value; }
        }

        public string srcfld
        {
            get { return fldSourceField; }
            set { fldSourceField = value; }
        }

        public string sqlfld
        {
            get { return fldSqlField; }
            set { fldSqlField = value; }
        }

        public string isFieldSql
        {
            get { return isSqlFld; }
            set { isSqlFld = value; }
        }

        public string fieldSqlQuery
        {
            get { return fldSqlQuery; }
            set { fldSqlQuery = value; }
        }

        public string isFieldApi
        {
            get { return isApiFld; }
            set { isApiFld = value; }
        }

        public string fieldSelAPI
        {
            get { return fldSelAPI; }
            set { fldSelAPI = value; }
        }

        public string fieldSelAPIParams
        {
            get { return fieldSelApiParams; }
            set { fieldSelApiParams = value; }
        }

        public string fieldSelAPIName
        {
            get { return fieldSelApiName; }
            set { fieldSelApiName = value; }
        }

        public int fldframeno
        {
            get { return fldFrameNo; }
            set { fldFrameNo = value; }
        }

        public int fldlength
        {
            get { return fldLength; }
            set { fldLength = value; }
        }

        public int flddecimal
        {
            get { return fldDecimalPlaces; }
            set { fldDecimalPlaces = value; }
        }

        public int pcount
        {
            get { return fldParamCount; }
            set { fldParamCount = value; }
        }

        public int refresh
        {
            get { return fldSelectRefresh; }
            set { fldSelectRefresh = value; }
        }

        public int order
        {
            get { return fldOrder; }
            set { fldOrder = value; }
        }

        public string Value
        {
            get { return fldValue; }
            set { fldValue = value; }
        }

        public bool visibility
        {
            get { return fldIsVisibility; }
            set { fldIsVisibility = value; }
        }

        public bool allowempty
        {
            get { return fldIsAllowEmpty; }
            set { fldIsAllowEmpty = value; }
        }

        public bool allowduplicate
        {
            get { return fldIsAllowDuplicate; }
            set { fldIsAllowDuplicate = value; }
        }

        public bool freadonly
        {
            get { return fldIsReadonly; }
            set { fldIsReadonly = value; }
        }

        public bool savenormal
        {
            get { return fldIsSaveNormal; }
            set { fldIsSaveNormal = value; }
        }

        public bool savevalue
        {
            get { return fldIsSaveValue; }
            set { fldIsSaveValue = value; }
        }

        public bool editcombo
        {
            get { return fldIsEditCombo; }
            set { fldIsEditCombo = value; }
        }

        public bool firesql
        {
            get { return fldIsSelectFireSql; }
            set { fldIsSelectFireSql = value; }
        }

        public bool fieldIsImage
        {
            get { return fldIsImage; }
            set { fldIsImage = value; }
        }

        public bool fieldIsFrmList
        {
            get { return fldIsFromList; }
            set { fldIsFromList = value; }
        }

        public string moeval
        {
            get { return fldMoeValue; }
            set { fldMoeValue = value; }
        }

        public string tooltip
        {
            get { return fldTooltip; }
            set { fldTooltip = value; }
        }

        public string fieldLabelCss
        {
            get { return fldLblCss; }
            set { fldLblCss = value; }
        }
        public string fieldCtrlCss
        {
            get { return fldTxtCss; }
            set { fldTxtCss = value; }
        }

        public string mRow
        {
            get { return masterRow; }
            set { masterRow = value; }
        }

        public string fldComboValues
        {
            get { return fldComboVals; }
            set { fldComboVals = value; }
        }

        public string fldIdCols
        {
            get { return fldIdCol; }
            set { fldIdCol = value; }
        }

        public string fieldParents
        {
            get { return fldParents; }
            set { fldParents = value; }
        }

        public string ClientFldParents
        {
            get { return clientFldParents; }
            set { clientFldParents = value; }
        }

        public string fieldDependents
        {
            get { return fldDependents; }
            set { fldDependents = value; }
        }

        public string fieldSqlDeps
        {
            get { return fldSqlDeps; }
            set { fldSqlDeps = value; }
        }

        public string fieldExpDeps
        {
            get { return fldExpDeps; }
            set { fldExpDeps = value; }
        }

        public string fieldSqlGridDeps
        {
            get { return fldGridSqlDeps; }
            set { fldGridSqlDeps = value; }
        }

        public string fieldSqlDepParents
        {
            get { return fldSqlDepParents; }
            set { fldSqlDepParents = value; }
        }

        public string fieldDepGridDcs
        {
            get { return fldDepGridDcs; }
            set { fldDepGridDcs = value; }
        }

        public string fieldDepFillDcs
        {
            get { return fldDepFillDcs; }
            set { fldDepFillDcs = value; }
        }

        public string fieldValSeparator
        {
            get { return fldValSeparator; }
            set { fldValSeparator = value; }
        }

        public string fldColor
        {
            get { return fldBgColor; }
            set { fldBgColor = value; }
        }

        public bool onlyPositive
        {
            get { return fldIsOnlyPositive; }
            set { fldIsOnlyPositive = value; }
        }

        public bool tabStop
        {
            get { return fldIsTabStop; }
            set { fldIsTabStop = value; }
        }

        public ArrayList AdvSrchFlds
        {
            get { return advSrchFlds; }
            set { advSrchFlds = value; }
        }

        public ArrayList AdvSrchFldsType
        {
            get { return advSrchFldsType; }
            set { advSrchFldsType = value; }
        }

        public string FldRapidDeps
        {
            get { return fldRapidDeps; }
            set { fldRapidDeps = value; }
        }
        public string FldRapidExpDeps
        {
            get { return fldRapidExpDeps; }
            set { fldRapidExpDeps = value; }
        }
        public string FldRapidDepType
        {
            get { return fldRapidDepType; }
            set { fldRapidDepType = value; }
        }
        public string FldRapidParents
        {
            get { return fldRapidParents; }
            set { fldRapidParents = value; }
        }
        public string FldRapidAllParents
        {
            get { return fldRapidAllParents; }
            set { fldRapidAllParents = value; }
        }
        public string FldRapidAllRows
        {
            get { return fldRapidAllRows; }
            set { fldRapidAllRows = value; }
        }
        public int fieldIndex
        {
            get { return fieldindex; }
            set { fieldindex = value; }
        }
        public string fldMultiSelect
        {
            get { return fldmultiselect; }
            set { fldmultiselect = value; }
        }
        public string fldMultiSelectSep
        {
            get { return fldmultiselectsep; }
            set { fldmultiselectsep = value; }
        }
        public string fldType
        {
            get { return fldtype; }
            set { fldtype = value; }
        }

        public string fldAcceptApi
        {
            get { return fldacceptapi; }
            set { fldacceptapi = value; }
        }

        public string fldIntelligence
        {
            get { return fldIntelli; }
            set { fldIntelli = value; }
        }

        public string fldFillFromAPI
        {
            get { return fldFillfromapi; }
            set { fldFillfromapi = value; }
        }

        public string fldTimeFormat
        {
            get { return fldtimeformat; }
            set { fldtimeformat = value; }
        }

        public string fldMaskType
        {
            get { return fldmasktype; }
            set { fldmasktype = value; }
        }

        public string fldMaskDetails
        {
            get { return fldmaskdetails; }
            set { fldmaskdetails = value; }
        }
    }

    /// <summary>
    /// FillGrid Structure
    /// </summary>
    [Serializable]
    public struct FGStruct
    {
        string fgsqlparams;
        string fgTargetDc;
        string fgSourceDc;
        string fgMultiSelect;
        string fgAutoShow;
        string fgCondition;
        string fgCaption;
        string fName;
        string fgValidateExp;
        string fgTotalColVal;
        ArrayList dcFldName;
        ArrayList sqlFldName;
        string fgExecOnSave;
        public string fgSqlParams
        {
            get { return fgsqlparams; }
            set { fgsqlparams = value; }
        }

        public string fgcaption
        {
            get { return fgCaption; }
            set { fgCaption = value; }
        }

        public string fgName
        {
            get { return fName; }
            set { fName = value; }
        }

        public string fgtargetdc
        {
            get { return fgTargetDc; }
            set { fgTargetDc = value; }
        }

        public string fgSourceDC
        {
            get { return fgSourceDc; }
            set { fgSourceDc = value; }
        }

        public string fgmultiselect
        {
            get { return fgMultiSelect; }
            set { fgMultiSelect = value; }
        }

        public string fgautoshow
        {
            get { return fgAutoShow; }
            set { fgAutoShow = value; }
        }

        public string FGCondition
        {
            get { return fgCondition; }
            set { fgCondition = value; }
        }
        public string FGExecOnSave
        {
            get { return fgExecOnSave; }
            set { fgExecOnSave = value; }
        }

        public string fgValExpression
        {
            get { return fgValidateExp; }
            set { fgValidateExp = value; }
        }

        public string fgtotalColVal
        {
            get { return fgTotalColVal; }
            set { fgTotalColVal = value; }
        }
        public ArrayList fgDcFldName
        {
            get { return dcFldName; }
            set { dcFldName = value; }
        }

        public ArrayList fgSqlFldName
        {
            get { return sqlFldName; }
            set { sqlFldName = value; }
        }
    }


    /// <summary>
    /// HyperLink Structure
    /// </summary>
    [Serializable]
    private struct HLinkStruct
    {
        string hlName;
        string hlSource;
        string hlPop;
        string hlLoad;
        string hlParamName;
        string hlParamValue;

        public string hlname
        {
            get { return hlName; }
            set { hlName = value; }
        }

        public string hlsource
        {
            get { return hlSource; }
            set { hlSource = value; }
        }

        public string hlpop
        {
            get { return hlPop; }
            set { hlPop = value; }
        }

        public string hlload
        {
            get { return hlLoad; }
            set { hlLoad = value; }
        }

        public string hlparamname
        {
            get { return hlParamName; }
            set { hlParamName = value; }
        }

        public string hlparamval
        {
            get { return hlParamValue; }
            set { hlParamValue = value; }
        }
    }
    /// <summary>
    /// icon structure
    /// </summary>
    [Serializable]
    public struct IconStruct
    {
        public string text;
        public string addclass;

    }

    /// <summary>
    /// Api structure
    /// </summary>
    [Serializable]
    public struct btnApiStruct
    {
        public string url;
        public string reqstr;
        public string restype;
        public string resformat;
        public string category;
        public string method;
    }


    /// <summary>
    /// Button structure
    /// </summary>
    [Serializable]
    public struct ButtonStruct
    {
        string btnTask;
        string btnActionValue;
        string btnFileUpload;
        string btnFileMessage;
        string btnFileType;
        string btnImageName;
        string btnCaption;
        string btnFooter;
        string btnVisible;
        string btnHint;
        string btnTLHW;
        string btnCancel;
        string btnID;
        bool isDrpDwnBtn;
        bool scriptapi;
        bool isFormBtn;
        IconStruct btnIcon;
        btnApiStruct btnApi;
        ArrayList childButtons;
        bool script;

        public bool isformbtn
        {
            get { return isFormBtn; }
            set { isFormBtn = value; }
        }
        public bool isDrpDwn
        {
            get { return isDrpDwnBtn; }
            set { isDrpDwnBtn = value; }
        }
        public bool scriptApi
        {
            get { return scriptapi; }
            set { scriptapi = value; }
        }
        public IconStruct icon
        {
            get { return btnIcon; }
            set
            {
                btnIcon.text = value.text;
                btnIcon.addclass = value.addclass;
            }
        }
        public btnApiStruct btnapi
        {
            get { return btnApi; }
            set
            {
                btnApi.url = value.url;
                btnApi.reqstr = value.reqstr;
                btnApi.restype = value.restype;
                btnApi.resformat = value.resformat;
                btnApi.category = value.category;
                btnApi.method = value.method;
            }
        }

        public ArrayList childBtns
        {
            get { return childButtons; }
            set { childButtons = value; }
        }

        public string ID
        {
            get { return btnID; }
            set { btnID = value; }
        }

        public string task
        {
            get { return btnTask; }
            set { btnTask = value; }
        }

        public string action
        {
            get { return btnActionValue; }
            set { btnActionValue = value; }
        }

        public string fileupload
        {
            get { return btnFileUpload; }
            set { btnFileUpload = value; }
        }

        public string cancelBtn
        {
            get { return btnCancel; }
            set { btnCancel = value; }
        }


        public string fmessage
        {
            get { return btnFileMessage; }
            set { btnFileMessage = value; }
        }

        public string ftype
        {
            get { return btnFileType; }
            set { btnFileType = value; }
        }

        public string image
        {
            get { return btnImageName; }
            set { btnImageName = value; }
        }

        public string hint
        {
            get { return btnHint; }
            set { btnHint = value; }
        }

        public string caption
        {
            get { return btnCaption; }
            set { btnCaption = value; }
        }

        public string dimension
        {
            get { return btnTLHW; }
            set { btnTLHW = value; }
        }
        public bool isScript
        {
            get { return script; }
            set { script = value; }
        }

        public string footer
        {
            get { return btnFooter; }
            set { btnFooter = value; }
        }

        public string visible
        {
            get { return btnVisible; }
            set { btnVisible = value; }
        }
    }
    #endregion

    # region Methods

    #region Private Methods

    /// <summary>
    /// function to update the tstruct related properties.
    /// </summary>
    /// <param name="xmlDoc"></param> 
    private void FillTstructProperties(XmlDocument xmlDoc)
    {
        //Loop through the root node and update the dcTitle
        //Tstruct attributes of the TstructDef class.
        XmlNodeList tstAttributeNodes = default(XmlNodeList);
        tstAttributeNodes = xmlDoc.GetElementsByTagName("root");
        foreach (XmlNode tstAttributeNode in tstAttributeNodes)
        {
            if (tstAttributeNode.Attributes["pform"] != null)
                tstPform = tstAttributeNode.Attributes["pform"].Value.ToString();
            if (tstAttributeNode.Attributes["wflow"] != null)
                tstWorkflow = tstAttributeNode.Attributes["wflow"].Value.ToString();
            if (tstAttributeNode.Attributes["updatedon"] != null)
                tstUpdateOn = tstAttributeNode.Attributes["updatedon"].Value.ToString();
            if (tstAttributeNode.Attributes["updusername"] != null)
                tstUpdateBy = tstAttributeNode.Attributes["updusername"].Value.ToString();
            if (tstAttributeNode.Attributes["readonly"] != null)
                tstReadOnly = tstAttributeNode.Attributes["readonly"].Value.ToString();
            if (tstAttributeNode.Attributes["cancelled"] != null)
                tstCancelled = tstAttributeNode.Attributes["cancelled"].Value.ToString();
            if (tstAttributeNode.Attributes["pdf"] != null)
                tstPdf = tstAttributeNode.Attributes["pdf"].Value.ToString();
            if (tstAttributeNode.Attributes["sverno"] != null)
                tstVersion = tstAttributeNode.Attributes["sverno"].Value.ToString();

            if (tstAttributeNode.Attributes["peg"] != null)
                AxTstIsPEG = tstAttributeNode.Attributes["peg"].Value.ToString();

            if (tstAttributeNode.Attributes["cachedsave"] != null && tstAttributeNode.Attributes["cachedsave"].Value.ToString() != "F")
                tstCachedsave = tstAttributeNode.Attributes["cachedsave"].Value.ToString();

            //if (tstAttributeNode.Attributes["runtimemod"] != null)
            //  tstRuntimeModification = tstAttributeNode.Attributes["runtimemod"].Value.ToString();

            // TODO : This is for temporary , we have to give all the tstruct related
            // attributes either in the root node or in the first childnode[tstruct node] of the root node. 
            XmlNode tstBaseNode = tstAttributeNode.ChildNodes[0];
            if (tstBaseNode.Attributes["cat"].Value.ToString().ToLower() == "tstruct")
            {
                foreach (XmlNode tstNode in tstBaseNode)
                {
                    if (tstNode.Name == "a1")
                        transId = tstNode.InnerText;
                    else if (tstNode.Name == "a2")
                    {
                        tstCaption = tstNode.InnerText;
                        tstCaption = tstCaption.Replace("&&", "&");
                    }
                    else if (tstNode.Name == "a18")
                        tstAttachment = tstNode.InnerText;
                    else if (tstNode.Name == "a20")
                        tstIviewButton = tstNode.InnerText;
                    //else if (tstNode.Name == "a28")
                    //    formLoadCache = tstNode.InnerText;
                    else if (tstNode.Name == "a29")
                        if (tstNode.InnerText != null && tstNode.InnerText.ToLower() == "wizard")
                            isWizardTstruct = true;
                }
            }
            if (tstBaseNode.Attributes != null && tstBaseNode.Attributes["wsflds"] != null)
            {
                string tempWsFieldDetails = string.Empty;
                try
                {
                    tempWsFieldDetails = tstBaseNode.Attributes["wsflds"].Value.ToString();
                }
                catch (Exception ex) { }

                //DoFormload~exflds~dcname`fillgrid names with auto show true~dcname`accpetsql,dcname`selectautoselectflds
                if (tempWsFieldDetails != string.Empty)
                {
                    wsPerfEnabled = true;
                    string[] wsDetails = tempWsFieldDetails.Split('~');
                    if (wsDetails.Length >= 1)//Doformload
                        wsPerfFormLoadCall = wsDetails[0].ToString().ToLower() == "true" ? true : false;
                    if (wsDetails.Length >= 2)//Expression flds
                        wsPerfEvalExpClient = wsDetails[1].ToString();
                    if (wsDetails.Length >= 3 && wsDetails[2] != "")
                    {
                        string[] flgDetails = wsDetails[2].Split(',').ToArray();
                        wsPerfFGDcName = flgDetails.Select(x => x.Split('`')[0]).ToArray();//Fillgrid dcname
                    }
                    if (wsDetails.Length >= 4 && wsDetails[3] != "")
                    {
                        string[] ldcDetails = wsDetails[3].Split(',').ToArray();
                        wsPerfLDcName = ldcDetails.Select(x => x.Split('`')[0]).ToArray();//dcname
                        wsPerfFields = string.Join(",", ldcDetails.Select(x => x.Split('`')[1]).ToArray());//acceptsql and select with autosel true fields
                    }
                }
            }
        }
    }

    /// <summary>
    /// Function to parse the format grid xml and update the dc properties.
    /// </summary>
    /// <param name="gFormatNodes"></param>
    private void XmlLoadFormatDcs(XmlNode gFormatNodes)
    {
        string actions = string.Empty;
        foreach (XmlNode dcNode in gFormatNodes)
        {
            for (int i = 0; i < dcs.Count; i++)
            {
                DcStruct dc = (DcStruct)dcs[i];
                if (dc.name == dcNode.Name)
                {
                    dc.IsFormatGrid = true;
                    foreach (XmlNode dataNode in dcNode.ChildNodes)
                    {
                        string value = string.Empty;
                        if (dataNode.Attributes["val"] != null)
                            value = dataNode.Attributes["val"].Value;
                        if (dataNode.Name.ToLower() == "keycol")
                            dc.KeyColumn = value;
                        else if (dataNode.Name.ToLower() == "keyval")
                        {
                            if (dataNode.Attributes["multiselect"] != null)
                            {
                                if (dataNode.Attributes["multiselect"].Value == "true")
                                    dc.MultiSelect = true;
                                else
                                    dc.MultiSelect = false;
                            }

                            if (value != string.Empty)
                            {
                                string[] strValues = value.Split(',');
                                for (int j = 0; j < strValues.Length; j++)
                                {
                                    dc.KeyColValues.Add(value);
                                }
                            }
                        }
                        else if (dataNode.Name.ToLower() == "headlp")
                        {
                            dc.HeaderStyle = value;
                        }
                        else if (dataNode.Name.ToLower() == "headl")
                        {
                            string[] dispKeyCol = value.Split('=');
                            dc.DisplayKeyCol = dispKeyCol[0].ToString();
                            foreach (XmlNode newItem in dataNode.ChildNodes)
                            {
                                dc.DispKeyColValues.Add(newItem.Name + "¿" + newItem.InnerText);
                            }
                        }
                        else if (dataNode.Name.ToLower() == "totlp")
                        {
                            dc.SubTotalStyle = value;
                        }
                        else if (dataNode.Name.ToLower() == "totl")
                        {
                            string[] stDetails = value.Split(',');
                            for (int j = 0; j < stDetails.Length; j++)
                            {
                                string[] cols = stDetails[j].ToString().Split('=');
                                if (cols.Length > 1)
                                {
                                    if (cols[1].Substring(0, 1) != "#")
                                        dc.DisplayTotalCol = cols[0].ToString().Trim();
                                    else
                                        dc.SubTotalColumns.Add(cols[0].ToString().Trim());
                                }
                            }
                        }
                        else if (dataNode.Name.ToLower() == "actions")
                        {
                            foreach (XmlNode actionNode in dataNode.ChildNodes)
                            {
                                if (actions == "")
                                    actions = actionNode.InnerText;
                                else
                                    actions = "," + actionNode.InnerText;
                            }
                            dc.Action = actions;
                        }
                        else if (dataNode.Name.ToLower() == "defaultvalues")
                        {
                            foreach (XmlNode defaultVal in dataNode.ChildNodes)
                            {
                                dc.KeyColValues.Add(defaultVal.Name);
                                dc.DispKeyColValues.Add(defaultVal.InnerText);
                            }
                        }
                        else if (dataNode.Name.ToLower() == "btns")
                        {
                            if (dataNode.InnerText.IndexOf("add") != -1)
                                dc.ShowAddBtn = true;
                            else
                                dc.ShowAddBtn = false;
                        }
                        else if (dataNode.Name.ToLower() == "sql")
                        {
                            if (dataNode.Attributes["hidecol"] != null)
                                dc.HideColumns = dataNode.Attributes["hidecol"].Value;
                            if (dataNode.Attributes["map"] != null)
                                dc.MapColumn = dataNode.Attributes["map"].Value;
                            if (dataNode.Attributes["mappar"] != null)
                                dc.MapParentCol = dataNode.Attributes["mappar"].Value;
                            dc.Sql = dataNode.InnerText;
                        }
                    }
                    dcs[i] = dc;
                }
                LoadFormatDcArray(dc);
            }
        }

    }

    /// <summary>
    /// Function to get the Tabbed DC's information and store in the pagePositions and visibleDC's array.
    /// </summary>
    /// <param name="iframeNodes"></param>
    private void XmlLoadTabDcs(XmlNode iframeNodes)
    {
        if (axdesignJObject.selectedLayout != null)
        {
            string visibleDC = string.Empty;
            StringBuilder strTab = new StringBuilder();
            string lastDc = string.Empty;

            string layout = axdesignJObject.selectedLayout;
            tstLayout = layout.ToLower();

            var designMode = false;

            if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
            {
                designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
            }

            if (dcs.Count == 1)
                isWizardTstruct = false;

            if ((tstLayout == "tile" && designMode) || isWizardTstruct || isMobile || dcs.Count == 1) tstLayout = "default";

            ArrayList tempPositions = new ArrayList();

            foreach (DcStruct dc in dcs)
            {
                bool isFirstDc = false;
                bool isSecondLastDc = false;
                bool isLastDc = false;
                int dcIndNo = dcsPositionIndex.IndexOf(dc.frameno.ToString());
                if (dc.frameno == 1)
                {
                    isFirstDc = true;
                }
                if (tstLayout == "default" || tstLayout == "double")
                {
                    if (dc.frameno == dcs.Count)
                    {
                        isLastDc = true;
                    }
                    if (dc.frameno == dcs.Count - 1 && dcs.Count > 2)
                    {
                        isSecondLastDc = true;
                    }
                }
                else
                {
                    if (dcIndNo == dcs.Count - 1)
                    {
                        isLastDc = true;
                    }
                    if (dcIndNo == dcs.Count - 1 && dcs.Count > 2)
                    {
                        isSecondLastDc = true;
                    }
                }

                switch (tstLayout)
                {
                    case "default":
                    case "tile":
                        {
                            tempPositions.Add(dc.frameno);

                            visibleDCs.Add(tempPositions[0].ToString());
                            pagePositions.Add(tempPositions[0].ToString());

                            tempPositions = new ArrayList();
                        }
                        break;
                    case "tabbed":
                        {
                            tempPositions.Add(dc.frameno);

                            if (isLastDc)
                            {
                                visibleDCs.Add(tempPositions[0].ToString());
                                pagePositions.Add(String.Join(",", tempPositions.ToArray()));

                                tempPositions = new ArrayList();
                            }
                        }
                        break;
                    case "double":
                        {
                            tempPositions.Add(dc.frameno);

                            if (isFirstDc)
                            {
                                visibleDCs.Add(tempPositions[0].ToString());
                                pagePositions.Add(String.Join(",", tempPositions.ToArray()));

                                tempPositions = new ArrayList();
                            }
                            if (isSecondLastDc)
                            {
                                visibleDCs.Add(tempPositions[0].ToString());
                                pagePositions.Add(String.Join(",", tempPositions.ToArray()));

                                tempPositions = new ArrayList();
                            }
                            if (isLastDc)
                            {
                                visibleDCs.Add(tempPositions[0].ToString());
                                pagePositions.Add(String.Join(",", tempPositions.ToArray()));

                                tempPositions = new ArrayList();
                            }
                        }
                        break;
                    case "single":
                        {
                            tempPositions.Add(dc.frameno);

                            if (isFirstDc)
                            {
                                visibleDCs.Add(tempPositions[0].ToString());
                                pagePositions.Add(String.Join(",", tempPositions.ToArray()));

                                tempPositions = new ArrayList();
                            }
                            if (isLastDc)
                            {
                                visibleDCs.Add(tempPositions[0].ToString());
                                pagePositions.Add(String.Join(",", tempPositions.ToArray()));

                                tempPositions = new ArrayList();
                            }
                        }
                        break;
                }
            }
        }
        else if (iframeNodes.Attributes["layout"] != null)
        {
            string visibleDC = string.Empty;
            StringBuilder strTab = new StringBuilder();
            string lastDc = string.Empty;

            //The layout attribute is expected to be there only in 9.0 structue xml
            string layout = iframeNodes.Attributes["layout"].Value;
            tstLayout = layout.ToLower();

            var designMode = false;

            //Below line will convert tile to default as tile is not working and not supported.

            if (dcs.Count == 1)
                isWizardTstruct = false;

            if (tstLayout == "tile" || isWizardTstruct || isMobile  /* || designMode*/) tstLayout = "default";
            foreach (XmlNode frmNode in iframeNodes)
            {
                //string dcName = frmNode.Attributes["name"].Value;
                //
                //string frameNo = frmNode.Attributes["fno"].Value;
                string dcTLHW = frmNode.Attributes["tlhw"].Value;
                foreach (XmlNode pageNode in frmNode)
                {
                    foreach (XmlNode dcNode in pageNode)
                    {
                        string dcName = dcNode.InnerText;
                        string frameNo = dcName.Substring(2);
                        if (dcName.Substring(0, 2) != "dc") continue;
                        SetDcWidth(frameNo, dcTLHW);

                        if (tstLayout == Constants.DEFAULT || tstLayout == Constants.TILE)
                        {
                            visibleDCs.Add(frameNo);
                            pagePositions.Add(frameNo);
                        }
                        else if (tstLayout == Constants.SINGLE || tstLayout == Constants.TABBED || tstLayout == Constants.DOUBLE)
                        {
                            if ((tstLayout == Constants.SINGLE || tstLayout == Constants.DOUBLE) && dcName == "dc1")
                            {
                                pagePositions.Add(frameNo);
                                visibleDCs.Add(frameNo);
                            }
                            else
                            {
                                if (tstLayout == Constants.DOUBLE && strTab.ToString() != string.Empty && frmNode.NextSibling == null)
                                    lastDc = frameNo;

                                if (strTab.ToString() == string.Empty)
                                {
                                    strTab.Append(frameNo);
                                    visibleDCs.Add(frameNo);
                                }
                                else
                                {
                                    if ((tstLayout == Constants.DOUBLE && lastDc == string.Empty) || (tstLayout != Constants.DOUBLE))
                                        strTab.Append("," + frameNo);
                                }
                            }
                        }
                    }
                }

                if (strTab.ToString() != string.Empty)
                {
                    pagePositions.Add(strTab.ToString());
                    strTab = new StringBuilder();
                }
            }

            if (tstLayout == Constants.DOUBLE && lastDc != string.Empty)
            {
                pagePositions.Add(lastDc);
                visibleDCs.Add(lastDc);
            }
        }
        else
        {
            foreach (XmlNode ifrNode in iframeNodes)
            {
                string visibleDC = string.Empty;
                XmlNodeList frames = ifrNode.ChildNodes;
                StringBuilder strTab = new StringBuilder();
                string lastDc = string.Empty;

                foreach (XmlNode pageNode in frames)
                {
                    if (pageNode.Name == "pages")
                    {
                        for (int i = 0; i < pageNode.ChildNodes.Count; i++)
                        {
                            string dcName = pageNode.ChildNodes[i].InnerText;
                            string tabNo = dcName.Substring(2, dcName.Length - 2);
                            if (i == 0)
                            {
                                visibleDC = tabNo;
                                strTab.Append(tabNo);
                            }
                            else
                            {
                                strTab.Append("," + tabNo);
                            }
                        }
                    }
                }
                if (strTab.ToString() != "")
                {
                    pagePositions.Add(strTab);
                    visibleDCs.Add(visibleDC);
                }
                strTab = new StringBuilder();
            }
        }
    }


    private void SetDcWidth(string frameNo, string tlhw)
    {
        int dcIndNo = dcsPositionIndex.IndexOf(frameNo.ToString());
        DcStruct dc = (DcStruct)dcs[dcIndNo];
        string[] strTLHW = tlhw.Split(',');
        if (strTLHW.Length > 3)
        {
            dc.dcWidht = Convert.ToInt32(strTLHW[3].ToString());
            dcs[dcIndNo] = dc;
        }
    }

    private string ReplaceSpecialCharsInHTML(string str)
    {
        str = str.Replace("&", "#amp:");
        str = str.Replace("<", "#lt:");
        str = str.Replace(">", "#gt:");
        str = str.Replace("'", "#apos:");
        //str = str.Replace("\"", "&quot;");
        return str;
    }

    /// <summary>
    /// function to update the dc list.
    /// </summary>
    /// <param name="dcNodes"></param>
    private void XmlLoadDc(XmlNode dcNodes)
    {
        DcStruct dc = new DcStruct();
        PopDcStruct pdc = new PopDcStruct();
        dc.isallowaddrows = true;
        dc.isallowdeletrows = true;
        dc.DCHasDataRows = false;
        dc.isallowchange = true;
        dc.tabParentFields = new ArrayList();
        dc.dcExpressionDeps = new StringBuilder();
        foreach (XmlNode dcNode in dcNodes)
        {
            if (dcNode.Name == "a1")
                dc.name = dcNode.InnerText;
            else if (dcNode.Name == "a2")
            {
                string dcCaption = dcNode.InnerText;
                dcCaption = dcCaption.Replace("&&", "&");
                dc.caption = dcCaption;
            }
            else if (dcNode.Name == "a5")
                dcTable.Add(dcNode.InnerText);
            else if (dcNode.Name == "a3")
            {
                dc.frameno = int.Parse(dcNode.InnerText);
                dcsPositionIndex.Add(dc.frameno.ToString());
            }
            else if (dcNode.Name == "a6")
            {
                dc.isgrid = bool.Parse(dcNode.InnerText);
                isfldDcGrid = bool.Parse(dcNode.InnerText);
            }
            else if (dcNode.Name == "a7")
                dc.isallowchange = bool.Parse(dcNode.InnerText);
            else if (dcNode.Name == "a17")
                dc.DcAllowEmpty = bool.Parse(dcNode.InnerText);
            else if (dcNode.Name == "a26")
            {
                if (dcNode.InnerText == string.Empty) dcNode.InnerText = "true";
                dc.isallowaddrows = bool.Parse(dcNode.InnerText);
            }
            else if (dcNode.Name == "a27")
            {
                if (dcNode.InnerText == string.Empty) dcNode.InnerText = "true";
                dc.isallowdeletrows = bool.Parse(dcNode.InnerText);
            }
            else if (dcNode.Name == "a9")
            {
                if (dcNode.Attributes["pop"] != null)
                {
                    if (dcNode.Attributes["pop"].Value.ToString() == "t")
                    {
                        //dc.ispopgrid = true;
                        //pdc = LoadXmlPopDc(pdc, dcNode);
                        //pdc.frameno = dc.frameno;
                        //if (pdc.caption != string.Empty)
                        //    dc.caption = pdc.caption;
                        //popdcs.Add(pdc);
                        dc.ispopgrid = false;
                    }
                    else
                    {
                        dc.ispopgrid = false;
                    }
                }
            }
            else if (dcNode.Name == "a31")
            {
                if (dcNode.Attributes["tlhw"] != null)
                {
                    string NonGridDcHgt = string.Empty;
                    string[] dim = dcNode.Attributes["tlhw"].Value.Split(',');
                    if (EnableOldTheme == "false")
                    {
                        NonGridDcHgt = Math.Floor((Convert.ToInt32(dim[2].ToString()) * 1.5)).ToString();
                        dc.dcHeight = Convert.ToInt32(NonGridDcHgt);
                    }
                    else
                    {
                        dc.dcHeight = Convert.ToInt32(Convert.ToInt32(dim[2].ToString()) * 1.15);
                    }
                }
            }
            else if (dcNode.Name == "a59")
                dc.purpose = dcNode.InnerText;
        }
        dc.IsFormatGrid = false;
        dc.ShowAddBtn = true;
        dc.MultiSelect = true;
        dc.Action = string.Empty;
        dc.KeyColumn = string.Empty;
        dc.KeyColValues = new ArrayList();
        dc.DispKeyColValues = new ArrayList();
        dc.SubTotalColumns = new ArrayList();
        dc.HideColumns = string.Empty;
        dc.Sql = string.Empty;
        dc.MapColumn = string.Empty;
        dc.MapParentCol = string.Empty;
        dc.SubTotalStyle = string.Empty;
        dc.HeaderStyle = string.Empty;
        dc.DisplayKeyCol = string.Empty;
        dc.DisplayTotalCol = string.Empty;
        dc.dcPList = dcPlistaddrow;
        dc.DcBlean = dcExpandCollapse;
        dc.DcDefaultstate = dcDefaultstate;
        LoadDCArray(dc, pdc);
        dcs.Add(dc);
        //For every dc add a field 'axp_recid + dcno' as a hidden field.
        CreateAxpRecIdFld(dc.frameno);
    }

    private void CreateAxpRecIdFld(int frameNo)
    {
        StringBuilder fldXml = new StringBuilder();
        fldXml.Append("<root>");
        fldXml.Append("<axp_recid" + frameNo + " cat='field'>");
        fldXml.Append("<a1>axp_recid" + frameNo + "</a1>");
        fldXml.Append("<a2>axp_recid" + frameNo + "</a2>");
        fldXml.Append("<a3>Character</a3>");
        fldXml.Append("<a4>15</a4><a5>0</a5>");
        fldXml.Append("<a6>Accept</a6><a7><a22 txt='' firesql='' param='n' pcount='0'></a22><a18></a18><a21>0</a21>");
        fldXml.Append("<a20>0</a20><a23></a23><a24></a24><a25></a25><a26 table='' cap='' stype='' scol='0'></a26><a27></a27>");
        fldXml.Append("<a28></a28><a29></a29><a30></a30><a40></a40><a41></a41><a17></a17></a7>");
        fldXml.Append("<a8>True</a8><a9>True</a9><a10>False</a10><a11>False</a11><a12>True</a12>");
        fldXml.Append("<a13></a13><a14></a14><a15>True</a15><a16>True</a16><a17>False</a17><a19></a19>");
        fldXml.Append("<a31 lbltlhw='52,10,18,150' tlhw='52,163,18,250' color='' font=',,,'></a31><a32></a32>");
        fldXml.Append("<a33></a33><a34></a34><a35></a35><a36></a36><a37></a37><a46></a46><a47></a47><a48></a48>");
        fldXml.Append("<a49></a49><a50></a50><a51></a51><a52>True</a52><a55>True</a55><a56></a56><a57></a57><a58></a58>");
        fldXml.Append("<a64></a64><a59></a59><a60></a60><a65></a65>");
        fldXml.Append("</axp_recid" + frameNo + "></root>");

        XmlDocument xmlD = new XmlDocument();
        xmlD.LoadXml(fldXml.ToString());
        XmlLoadField(xmlD.ChildNodes[0].ChildNodes[0]);
    }

    /// <summary>
    /// Function to construct the popDC and update its properties in the popDC structure PopDcStruct. 
    /// </summary>
    /// <param name="pdc"></param>
    /// <param name="PopDcNodes"></param>
    /// <returns></returns>
    private PopDcStruct LoadXmlPopDc(PopDcStruct pdc, XmlNode PopDcNodes)
    {
        pdc.PopCondition = string.Empty;
        foreach (XmlNode dcPopNode in PopDcNodes)
        {
            if (dcPopNode.Name == "a18")
                pdc.caption = dcPopNode.InnerText;
            else if (dcPopNode.Name == "a19")
                pdc.pdcno = int.Parse(dcPopNode.InnerText.ToString().Substring(2));
            else if (dcPopNode.Name == "a20")
                pdc.pfields = dcPopNode.InnerText;
            else if (dcPopNode.Name == "a21")
                pdc.popfield = dcPopNode.InnerText;
            else if (dcPopNode.Name == "a23")
            {
                if (dcPopNode.Attributes["cond"] != null)
                    pdc.PopCondition = dcPopNode.Attributes["cond"].Value;
            }
            else if (dcPopNode.Name == "a24")
            {
                pdc.pFirm = dcPopNode.Attributes["firm"].Value.ToString();
                pdc.pAuto = dcPopNode.Attributes["auto"].Value.ToString();
                pdc.pKeyCols = dcPopNode.Attributes["kcol"].Value.ToString();
                string strSql = string.Empty;
                XmlNodeList sqlNodes = dcPopNode.ChildNodes;
                if (sqlNodes.Count > 0)
                {
                    foreach (XmlNode sqlNode in dcPopNode)
                    {
                        strSql += sqlNode.InnerText;
                    }
                }
                pdc.fillSql = string.Empty;
                if (strSql != string.Empty)
                    pdc.fillSql = "1";

            }
            else if (dcPopNode.Name == "a25")
            {
                if (dcPopNode.Attributes["disp"] != null)
                    pdc.dispSummaryParent = dcPopNode.Attributes["disp"].Value;
                if (dcPopNode.Attributes["sum"] != null)
                    pdc.dispSummaryField = dcPopNode.Attributes["sum"].Value;
                if (dcPopNode.Attributes["deli"] != null)
                    pdc.summaryDelimiter = dcPopNode.Attributes["deli"].Value;
            }
        }

        return pdc;

    }

    /// <summary>
    /// function to update the javascript field array.
    /// </summary>
    /// <param name="fld"></param>
    private void LoadDCArray(DcStruct dc, PopDcStruct pdc)
    {
        StringBuilder DCArray = new StringBuilder();

        DCArray.Append("DCName[" + DCNo + "]=\"" + dc.name + "\";DCCaption[" + DCNo + "]=\"" + ReplaceSpecialCharsInHTML(dc.caption) + "\";DCFrameNo[" + DCNo + "]=\"" + dc.frameno + "\";DCIsGrid[" + DCNo + "]=\"" + dc.isgrid + "\";DCIsPopGrid[" + DCNo + "]=\"" + dc.ispopgrid + "\";DCAllowEmpty[" + DCNo + "]=\"" + dc.DcAllowEmpty.ToString() + "\";DCHasDataRows[" + DCNo + "]=\"" + dc.DCHasDataRows.ToString() + "\";DcAllowAdd[" + DCNo + "]=\"" + dc.isallowaddrows.ToString() + "\";DCAllowChange[" + DCNo + "]=\"" + dc.isallowchange.ToString() + "\";");
        DCNo++;

        if (dc.ispopgrid)
        {
            DCArray.Append("PopGridDCs[" + popDCNo + "]=\"" + dc.frameno + "\";PopParentDCs[" + popDCNo + "]=\"" + pdc.pdcno + "\";PopParentFlds[" + popDCNo + "]=\"" + pdc.pfields + "\";PopGridDCFirm[" + popDCNo + "]=\"" + pdc.pFirm + "\";PopCondition[" + popDCNo + "]=\"" + pdc.PopCondition + "\";");
            DCArray.Append("PopSqlFill[" + popDCNo + "]=\"" + pdc.fillSql + "\";PopSummaryParent[" + popDCNo + "]=\"" + pdc.dispSummaryParent + "\";PopSummaryFld[" + popDCNo + "]=\"" + pdc.dispSummaryField + "\";PopSummDelimiter[" + popDCNo + "]=\"" + pdc.summaryDelimiter + "\";");
            popDCNo++;
        }

        jsDCArray.Append(DCArray.ToString().Trim());
    }

    /// <summary>
    /// function to update the javascript field array.
    /// </summary>
    /// <param name="fld"></param>
    private void LoadDCNewArray(DcStruct dc)
    {
        StringBuilder DCNewArray = new StringBuilder();

        DCNewArray.Append("DCExpDeps[" + (dc.frameno - 1) + "]=\"" + dc.dcExpressionDeps.ToString() + "\";DcAcceptMRFlds[" + (dc.frameno - 1) + "]=\"" + dc.DCAcceptMRFields + "\";");
        DCNo++;
        jsDCArray.Append(DCNewArray.ToString().Trim());
    }

    /// <summary>
    /// Function to add dc format grid properties to the js arrays
    /// </summary>
    /// <param name="dc"></param>
    private void LoadFormatDcArray(DcStruct dc)
    {
        StringBuilder DCArray = new StringBuilder();
        StringBuilder subTotCols = new StringBuilder();
        for (int i = 0; i < dc.SubTotalColumns.Count; i++)
        {
            if (subTotCols.ToString() == "")
                subTotCols.Append(dc.SubTotalColumns[i].ToString());
            else
                subTotCols.Append("," + dc.SubTotalColumns[i].ToString());
        }
        DCArray.Append("DcIsFormatGrid[" + (dc.frameno - 1) + "]=\"" + dc.IsFormatGrid + "\";DcKeyColumns[" + (dc.frameno - 1) + "]=\"" + dc.KeyColumn + "\";DcKeyColValues[" + (dc.frameno - 1) + "]=\"" + dc.KeyColValues + "\";");
        DCArray.Append("DcSubTotCols[" + (dc.frameno - 1) + "]=\"" + subTotCols + "\";DcMultiSelect[" + (dc.frameno - 1) + "]=\"" + dc.MultiSelect + "\";");
        //DcMultiSelect
        jsDCArray.Append(DCArray.ToString().Trim());

    }

    /// <summary>
    /// function to update the field list
    /// </summary>
    /// <param name="fieldNodes"></param>
    private void XmlLoadField(XmlNode fieldNodes, string fieldType = "")
    {
        fieldIndex++;
        List<String> listSrchCols = null;
        if (srchCols != String.Empty)
            listSrchCols = srchCols.Split(',').ToList();

        string fldFromApi = string.Empty;
        FieldStruct fld = new FieldStruct();
        fld.fieldIndex = fieldIndex;
        string fldSearchSQL = string.Empty;
        fld.fieldSqlGridDeps = string.Empty;
        fld.fieldDepGridDcs = string.Empty;
        fld.isFieldSql = string.Empty;
        fld.fieldSqlQuery = string.Empty;
        fld.fieldSelAPI = string.Empty;
        fld.isFieldApi = string.Empty;
        fld.fieldValSeparator = string.Empty;
        fld.fldColor = string.Empty;
        fld.AdvSrchFlds = new ArrayList();
        fld.AdvSrchFldsType = new ArrayList();
        fld.FldRapidAllParents = string.Empty;
        fld.FldRapidAllRows = string.Empty;
        fld.fldType = fieldType;
        foreach (XmlNode fldNode in fieldNodes)
        {
            if (fldNode.Name == "a1")
            {
                fld.name = fldNode.InnerText;
                //To check if the field is a image and needs to be stored in the server folder.
                fldFromApi = string.Empty;
                if (fld.name.StartsWith(Constants.IMGPrefix))
                {
                    ContainsImage = true;
                    fld.fieldIsImage = true;
                    AxImageFields.Add(fld.name);
                }
                if (fld.name.StartsWith("axp_nga_"))
                {
                    ContainsImage = true;
                    fld.fieldIsImage = true;
                    AxImageFields.Add(fld.name);
                    AxAttachFields.Add(fld.name);
                }
                if (fld.name.ToLower().StartsWith("axpfile_") || fld.name.ToLower().StartsWith("axpfilepath_"))
                {
                    AxpFileUploadFields.Add(fld.name);
                }
                if (fld.name.ToLower() == (Constants.AXPIMAGEPATH))
                {
                    isAxpImagePath = true;
                    //fld.fieldIsImage = true;
                    //AxImageFields.Add(fld.name);
                }

                if (fld.name.StartsWith("axp_gridattach_") || (fld.name.StartsWith("dc") && fld.name.ToLower().EndsWith("_image")))
                {
                    ContainsGridAttach = true;
                    AxAttachFields.Add(fld.name);
                }
                if (fld.name.StartsWith("dc") && fld.name.ToLower().EndsWith("_referimages"))
                {
                    ContainsGridRefer = true;
                    AxAttachFields.Add(fld.name);
                }
                if (fld.name.ToLower().EndsWith("_imagepath"))
                    AxAttachFields.Add(fld.name);

                if (fld.name == "axp_latlong")
                    fld_axp_latlong = fld.name;
                if (fieldNodes.Attributes["encrypted"] != null)
                {
                    fld.encryptflag = fieldNodes.Attributes["encrypted"].Value;
                    if (fieldNodes.Attributes["encrypted"].Value == "T")
                        FEncryptFlag.Add(fld.name);
                }
                else
                    fld.encryptflag = "F";
            }
            else if (fldNode.Name == "a2")
            {
                string fldCaption = fldNode.InnerText;
                fldCaption = fldCaption.Replace("&&", "&");
                fld.caption = fldCaption;
                if (fld.caption == "")
                    fld.caption = "&nbsp";
            }
            else if (fldNode.Name == "a3")
            {
                fld.datatype = fldNode.InnerText;
                if (fldNode.InnerText.ToLower() == "image")
                {
                    ContainsImage = true;
                    fld.fieldIsImage = true;
                    AxImageFields.Add(fld.name);
                }

                if (fld.datatype == "Numeric")
                {
                    if (fieldNodes.Attributes["customdecimal"] != null)
                        fld.CustomDecimal = fieldNodes.Attributes["customdecimal"].Value.ToString() == "T" ? true : false;
                    else
                        fld.CustomDecimal = false;
                }
                else
                    fld.CustomDecimal = false;

                if (fld.datatype == "Numeric")
                {
                    if (fieldNodes.Attributes["displaytotal"] != null)
                        fld.NumDisplayTotal = fieldNodes.Attributes["displaytotal"].Value.ToString() == "T" ? true : false;
                    else
                        fld.NumDisplayTotal = false;
                }
                else
                    fld.NumDisplayTotal = false;


                if (fld.fldType == "")
                    fld.fldType = fld.datatype;
            }
            else if (fldNode.Name == "a4")
                fld.fldlength = int.Parse(fldNode.InnerText);
            else if (fldNode.Name == "a5")
            {
                fld.flddecimal = int.Parse(fldNode.InnerText);
                //if (fld.CustomDecimal && HttpContext.Current.Session["axdecimal"] != null)
                //{
                //    string axDecimal = HttpContext.Current.Session["axdecimal"].ToString();
                //    if (axDecimal != "")
                //    {
                //        fld.flddecimal = int.Parse(axDecimal);
                //    }
                //}
            }
            else if (fldNode.Name == "a6")
            {
                fld.moe = fldNode.InnerText;
                if (fldNode.Attributes["apiname"] != null)
                    fld.fldAcceptApi = fldNode.Attributes["apiname"].Value;
                else
                    fld.fldAcceptApi = "";

                if (fldNode.Attributes["fromapi"] != null)
                    fldFromApi = fld.name + "^" + fldNode.Attributes["fromapi"].Value;

                if (fldNode.Attributes["intelligence"] != null)
                    fld.fldIntelligence = fldNode.Attributes["intelligence"].Value;
                else
                    fld.fldIntelligence = "";

                if (fldNode.Attributes["fillfromapi"] != null && fldNode.Attributes["fillfromapi"].Value == "true")
                {
                    if (fldNode.Attributes["fillfromapi_source"] != null)
                        fld.fldFillFromAPI = fldNode.Attributes["fillfromapi_source"].Value;
                    else
                        fld.fldFillFromAPI = "";
                }
                else
                    fld.fldFillFromAPI = "";

            }
            else if (fldNode.Name == "a7")
                fld = UpdateDetailsofMOE(fldNode, fld);
            else if (fldNode.Name == "a8")
                fld.visibility = bool.Parse(fldNode.InnerText);
            else if (fldNode.Name == "a9")
                fld.allowempty = bool.Parse(fldNode.InnerText);
            else if (fldNode.Name == "a10")
                fld.freadonly = bool.Parse(fldNode.InnerText);
            else if (fldNode.Name == "a11")
            {
                if (bool.Parse(fldNode.InnerText) == true)
                    fldSetCarry.Add(fld.name);
            }
            else if (fldNode.Name == "a12")
                fld.savevalue = bool.Parse(fldNode.InnerText);
            else if (fldNode.Name == "a13")
            {
                fld.expr = fldNode.InnerText.Trim().Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                if (fldFromApi != "" && fld.expr != "")
                {
                    fldFromApi += "^" + fld.expr;
                    fld.expr = string.Empty;
                }
                if (fld.name.ToUpper() == "AXP_HIDE_TOOLBAR")
                {
                    if (fld.expr != string.Empty && fld.expr.Trim('{', '}').ToLower() == "t")
                        hideToolBar = true;
                }

                if (fld.expr != "" && fld.expr.ToLower().Contains("regvar("))
                {
                    if (listofRagVarfld != "")
                        listofRagVarfld += "," + fld.name;
                    else
                        listofRagVarfld += fld.name;
                }
            }
            else if (fldNode.Name == "a14")
                fld.vexpr = fldNode.InnerText.Trim().Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
            else if (fldNode.Name == "a15")
                fld.allowduplicate = bool.Parse(fldNode.InnerText);
            else if (fldNode.Name == "a16")
                fld.onlyPositive = bool.Parse(fldNode.InnerText);
            else if (fldNode.Name == "a67")
                fld.tabStop = fldNode.InnerText == "" ? true : bool.Parse(fldNode.InnerText);
            else if (fldNode.Name == "a17")
                fld.savenormal = bool.Parse(fldNode.InnerText);
            else if (fldNode.Name == "a49")
                fld = PatternCheck(fldNode, fld);
            else if (fldNode.Name == "a31")
                fld = FldDimensionCheck(fldNode, fld);
            else if (fldNode.Name == "a50")
            {
                if (fldNode.Attributes["minlength"] != null)
                    fld.pwdminchar = int.Parse(fldNode.Attributes["minlength"].Value);
                if (fldNode.Attributes["expirydays"] != null)
                    fld.pwdnodexpiry = int.Parse(fldNode.Attributes["expirydays"].Value);
                if (fldNode.Attributes["alphanumeric"] != null)
                    fld.pwdalphanumeric = bool.Parse(fldNode.Attributes["alphanumeric"].Value);

                fld.pwdchar = fldNode.InnerText;
            }
            else if (fldNode.Name == "a51")
                fld.fieldHint = fldNode.InnerText;
            else if (fldNode.Name == "a52")
                fld.applycomma = fldNode.InnerText;
            else if (fldNode.Name == "a58")
                fld = SetValueFMT(fldNode, fld);
            else if (fldNode.Name == "a59")
                fld.fieldPurpose = fldNode.InnerText;
            else if (fldNode.Name == "a64")
                fldSearchSQL = fldNode.InnerText;
            else if (fldNode.Name == "a65")
                fld = SetTooltip(fldNode, fld);
            else if (fldNode.Name == "a66")
                fld = SetParents(fldNode, fld);
            else if (fldNode.Name == "a70")
            {
                fld.tabletypeval = fldNode.InnerText.Replace("\"", "&quot;");
                if (fld.tabletypeval != "" && fld.tabletypeval.StartsWith("{"))
                {
                    string tbltypeval = utilObj.ReverseCheckSpecialChars(fld.tabletypeval);
                    dynamic tblJson = JObject.Parse(tbltypeval);
                    if (tblJson.props.sql != null)
                    {
                        fld.tabletypsql = tblJson.props.sql.Value;
                        tblJson.props.sql = "true";
                        fld.tabletypeval = utilObj.CheckSpecialChars(JsonConvert.SerializeObject(tblJson));
                    }
                }
            }
            else if (fldNode.Name == "a48")
            {
                string maskDtls = string.Empty;
                string maskType = string.Empty;
                try
                {
                    if (fldNode.Attributes["masking"] != null && fldNode.Attributes["masking"].Value != "No Mask" && fldNode.Attributes["masking"].Value == "Mask all characters")
                    {
                        string strmaRoles = String.Empty;
                        if (fldNode.Attributes["maskroles"] != null)
                            strmaRoles = fldNode.Attributes["maskroles"].Value;
                        string[] maRoles = strmaRoles.Split(',');
                        string userRoles = HttpContext.Current.Session["AxRole"].ToString();
                        string[] userRolesList = userRoles.Split(',');
                        bool isMaskMatch = false;
                        foreach (var marName in maRoles)
                        {
                            int index = Array.IndexOf(userRolesList, marName);
                            if (index != -1 || (marName != "" && marName.ToLower() == "default"))
                            {
                                isMaskMatch = true;
                                break;
                            }
                        }
                        if (isMaskMatch)
                        {
                            maskType = fldNode.Attributes["masking"].Value;
                            if (fldNode.Attributes["lastcharmask"] != null)
                                maskDtls += fldNode.Attributes["lastcharmask"].Value;
                            if (fldNode.Attributes["firstcharmask"] != null)
                                maskDtls += "♦" + fldNode.Attributes["firstcharmask"].Value;
                            if (fldNode.Attributes["maskchar"] != null)
                                maskDtls += "♦" + fldNode.Attributes["maskchar"].Value;
                            if (fldNode.Attributes["maskroles"] != null)
                                maskDtls += "♦" + fldNode.Attributes["maskroles"].Value;
                        }
                    }
                    else if (fldNode.Attributes["masking"] != null && fldNode.Attributes["masking"].Value != "No Mask" && fldNode.Attributes["masking"].Value == "Show few characters")
                    {
                        maskType = fldNode.Attributes["masking"].Value;
                        if (fldNode.Attributes["lastcharmask"] != null)
                            maskDtls += fldNode.Attributes["lastcharmask"].Value;
                        if (fldNode.Attributes["firstcharmask"] != null)
                            maskDtls += "♦" + fldNode.Attributes["firstcharmask"].Value;
                        if (fldNode.Attributes["maskchar"] != null)
                            maskDtls += "♦" + fldNode.Attributes["maskchar"].Value;
                        if (fldNode.Attributes["maskroles"] != null)
                            maskDtls += "♦" + fldNode.Attributes["maskroles"].Value;
                    }
                }
                catch (Exception ex) { }
                fld.fldMaskType = maskType;
                fld.fldMaskDetails = maskDtls;
            }
            if (ContainsGridAttach)
            {
                if (fldNode.Name == "a13" && fldNode.InnerText != "")
                {
                    ContainsGridRefer = true;
                }
            }
        }
        if (fldFromApi != string.Empty)
            fldAcceptFromApi.Add(fldFromApi);
        //To check if add row limit has been given
        if (fld.name.ToLower().Contains("ax_rowlimit"))
        {
            int rowMaxVal;
            string fldExpr = fld.expr.ToString().Replace("{", "");
            fldExpr = fldExpr.Replace("}", "");

            if (int.TryParse(fldExpr, out rowMaxVal) && rowMaxVal > 1 && !AxRowLimit.Keys.Equals(fld.name))
                AxRowLimit.Add(fld.name.ToLower(), rowMaxVal);
        }

        if (listSrchCols != null && listSrchCols.Count > 0)
        {
            //TODO: check if we can avoid the to lower check
            //if (listSrchCols.Contains(fld.name.ToLower()))
            if (listSrchCols.Contains(fld.name))
            {
                lstSearchDataNames.Add(fld.name);
                lstSearchDataCaptions.Add(fld.caption);
            }
        }
        else
        {
            if ((fld.savevalue == true || (fld.savevalue == false && fldSearchSQL != string.Empty)) && fld.datatype != "Image" && (!fld.visibility) && (fld.pwdchar == ""))
            {
                lstSearchDataNames.Add(fld.name);
                lstSearchDataCaptions.Add(fld.caption);
            }
        }

        //Update the field count for the dc.
        if (!string.IsNullOrEmpty(tstVersion) && Int64.Parse(tstVersion) > 10000)
        {
            fld = SetRapidDependents(fld);
        }

        if (!fld.visibility)
            dcFldVisibleCount++;
        dcFldCount++;
        DcStruct dc = ((DcStruct)(dcs[dcs.Count - 1]));
        dc.fieldCount = dcFldCount;
        dc.visibleFldCount = dcFldVisibleCount;
        dcs[dcs.Count - 1] = dc;


        if (HttpContext.Current.Session["AxpFldMultiSelect"] != null && HttpContext.Current.Session["AxpFldMultiSelect"].ToString() != "")
        {
            bool isMultiSelect = false;
            var msFldList = (List<string>)HttpContext.Current.Session["AxpFldMultiSelect"];
            isMultiSelect = msFldList.AsEnumerable().Where(x => x.ToString().ToLower() == fld.name.ToLower()).Any();
            if (isMultiSelect)
            {
                var MultiSelectSp = (List<string>)HttpContext.Current.Session["AxpFldMultiSelSp"];
                MultiSelectSp = MultiSelectSp.AsEnumerable().Where(x => x.ToString().ToLower().Split('♦')[0] == fld.name.ToLower()).Select(x => x.ToString().ToLower().Split('♦')[1]).ToList();
                fld.fldMultiSelect = fld.name;
                fld.fldMultiSelectSep = MultiSelectSp[0];
            }
        }

        //If field is part of formload exp deps, construct the component id here  
        if (!string.IsNullOrEmpty(tstVersion) && Int64.Parse(tstVersion) > 10000)
        {
            UpdateRapidExpCompIds(fld.name, dc);
        }

        fld.order = dcFldCount;
        //fld.fldframeno = dcs.Count;
        fld.fldframeno = dc.frameno;
        fld = LoadFieldArray(fld);
        flds.Add(fld);
    }

    private void UpdateRapidExpCompIds(string fldName, DcStruct dc)
    {
        if (rapidFLExpFlds.Count == 0) return;
        int rIdx = rapidFLExpFlds.IndexOf(fldName);
        if (rIdx != -1)
        {
            string compId = string.Empty;
            if (dc.isgrid)
                compId = fldName + "001F" + dc.frameno;
            else
                compId = fldName + "000F" + dc.frameno;
            rapidFLExpFlds[rIdx] = compId;
        }
    }

    private FieldStruct SetParents(XmlNode fldNode, FieldStruct fld)
    {
        string fmText = "";
        fmText = fldNode.InnerText.ToString();
        string[] depArray = null;
        if (fmText != "")
            depArray = fmText.Split(',');

        if (depArray != null)
        {
            for (int i = 0; i < depArray.Length; i++)
            {
                if (fld.fieldParents == null || fld.fieldParents == "")
                    fld.fieldParents = depArray[i].ToString();
                else
                    fld.fieldParents += "," + depArray[i].ToString();
            }
        }
        fld.fieldParents = cust.UpdateSqlGridParents(transId, fld.name, fld.fieldParents);
        string flParents = fld.fieldParents;
        fld.fieldParents = fld.fieldParents.Replace("~", ",");
        if (flParents != "" && flParents.Contains("~"))
            fld.ClientFldParents = flParents.Split('~')[0];
        else
            fld.ClientFldParents = fld.fieldParents;
        return fld;
    }

    /// <summary>
    /// function to update the javascript field array.
    /// </summary>
    /// <param name="fld"></param>
    private FieldStruct LoadFieldArray(FieldStruct fld)
    {
        StringBuilder fieldArray = new StringBuilder();
        if (((fld.datatype == "Numeric") && (fld.flddecimal > 0)))
            fld.fldlength = (int)fld.fldlength + 1;

        fldGlobal = fldNo;

        if (fldNo == 0)
        {
            dcNo = ((DcStruct)(dcs[dcs.Count - 1])).frameno;
            dcRange = fldNo.ToString();
        }
        string allowEmpty = "";
        if (fld.allowempty == true) allowEmpty = "T";
        else allowEmpty = "F";
        string applyComma = "";
        if (fld.applycomma.ToLower() == "true") applyComma = "T";
        else applyComma = "F";
        string visible = "";
        if (fld.visibility == true) visible = "T";
        else visible = "F";
        string onlyPositive = "";
        if (fld.onlyPositive == true) onlyPositive = "T";
        else onlyPositive = "F";
        string tabStop = "";
        if (fld.tabStop == true) tabStop = "T";
        else tabStop = "F";

        fieldArray.Append(" FNames[" + fldNo + "]=\"" + fld.name + "\";ExprPosArray[" + fldNo + "]=\"\";FLowerNames[" + fldNo + "]=\"" + fld.name.ToLower() + "\"; FDataType[" + fldNo + "]=\"" + fld.datatype + "\"; FTableTypeVal[" + fldNo + "]=\"" + fld.tabletypeval + "\"; FDecimal[" + fldNo + "]=\"" + fld.flddecimal + "\";FldValidateExpr[" + fldNo + "]=\"" + fld.vexpr.Replace("\"", "'") + "\";FDupDecimals[" + fldNo + "]=\"" + fld.flddecimal + "\";FCustDecimal[" + fldNo + "]=\"" + fld.CustomDecimal + "\";FNumDisplayTot[" + fldNo + "]=\"" + fld.NumDisplayTotal + "\";");
        fieldArray.Append("FCaption[" + fldNo + "]=\"" + fld.caption + "\";FldsFrmLst[" + fldNo + "]=\"" + fld.fieldIsFrmList + "\";");
        fieldArray.Append("FMoe[" + fldNo + "]=\"" + fld.moe + "\";");
        fieldArray.Append("FMaxLength[" + fldNo + "]=\"" + fld.fldlength + "\";");
        fieldArray.Append("FProps[" + fldNo + "]=\"" + applyComma + allowEmpty + visible + onlyPositive + tabStop + "\";");
        fieldArray.Append("FldFrameNo[" + fldNo + "]=\"" + ((DcStruct)(dcs[dcs.Count - 1])).frameno + "\";");
        fieldArray.Append("FToolTip[" + fldNo + "]=\"" + fld.tooltip + "\";");
        fieldArray.Append("fDefList[" + fldNo + "]=\"" + fld.slist + "\";fExdefList[" + fldNo + "]=\"" + fld.elist + "\";ffillList[" + fldNo + "]=\"" + fld.flist + "\";fSfList[" + fldNo + "]=\"" + fld.sflist + "\";");

        fieldArray.Append("FldDependents[" + fldNo + "]=\"" + fld.fieldDependents + "\";");
        fieldArray.Append("FldParents[" + fldNo + "]=\"" + fld.fieldParents + "\";");
        fieldArray.Append("ClientFldParents[" + fldNo + "]=\"" + fld.ClientFldParents + "\";");
        fieldArray.Append("FldAutoSelect[" + fldNo + "]=\"" + fld.autoselect + "\";");
        fieldArray.Append("FldIsSql[" + fldNo + "]=\"" + fld.isFieldSql + "\";");
        fieldArray.Append("FldIsAPI[" + fldNo + "]=\"" + fld.isFieldApi + "\";");
        fieldArray.Append("FldAlignType[" + fldNo + "]=\"" + fld.type + "\";");
        fieldArray.Append("FldChkSeparator[" + fldNo + "]=\"" + fld.fieldValSeparator + "\";");
        fieldArray.Append("FldMgsSeparator[" + fldNo + "]=\"" + fld.fldMultiSelectSep + "\";");
        fieldArray.Append("FFieldType[" + fldNo + "]=\"" + fld.fldType + "\";");
        fieldArray.Append("FFieldHidden[" + fldNo + "]=\"" + fld.visibility + "\";");
        fieldArray.Append("FldDSqlParams[" + fldNo + "]=\"" + fld.refresh + "\";");

        ArrFFieldHidden.Append(fld.visibility + ",");

        fieldArray.Append("FFieldReadOnly[" + fldNo + "]=\"" + fld.freadonly + "\";");

        ArrFFieldReadOnly.Append(fld.freadonly + ",");

        fieldArray.Append("FFieldAcceptApi[" + fldNo + "]=\"" + fld.fldAcceptApi + "\";");
        fieldArray.Append("FFieldIntelli[" + fldNo + "]=\"" + fld.fldIntelligence + "\";");
        fieldArray.Append("FldMaskType[" + fldNo + "]=\"" + fld.fldMaskType + "\";");
        fieldArray.Append("FldMaskDetails[" + fldNo + "]=\"" + fld.fldMaskDetails + "\";");
        fieldArray.Append("FldFillFromAPI[" + fldNo + "]=\"" + fld.fldFillFromAPI + "\";");
        //Rapid field arrays
        fieldArray.Append("FldRapidDeps[" + fldNo + "]=\"" + fld.FldRapidDeps + "\";");
        fieldArray.Append("FldRapidDepType[" + fldNo + "]=\"" + fld.FldRapidDepType + "\";");
        fieldArray.Append("FldRapidExpDeps[" + fldNo + "]=\"" + fld.FldRapidExpDeps + "\";");
        fieldArray.Append("FldRapidParents[" + fldNo + "]=\"" + fld.FldRapidParents + "\";");
        fieldArray.Append("FldPurpose[" + fldNo + "]=\"" + utilObj.CheckSpecialCharacterPurpose(fld.fieldPurpose) + "\";");

        if (dcNo != ((DcStruct)(dcs[dcs.Count - 1])).frameno || DcCnt == dcs.Count)
        {
            dcRange = dcRange + "," + (fldNo - 1).ToString();
            fieldArray.Append("FldDcRange[" + DcCnt + "]=\"" + dcNo.ToString() + "~" + dcRange + "\";");
            fldDcRange.Add(dcNo.ToString() + "~" + dcRange);
            dcRange = "";
            DcCnt++;
            dcNo = ((DcStruct)(dcs[dcs.Count - 1])).frameno;
            dcRange = fldNo.ToString();
        }

        if (!((DcStruct)(dcs[dcs.Count - 1])).isgrid)
        {
            fieldArray.Append("HTMLFldNames[" + fldNo + "]=\"" + fld.name + "000F0\";");
        }
        else
        {
            fieldArray.Append("HTMLFldNames[" + fldNo + "]=\"" + fld.name + "001F" + ((DcStruct)(dcs[dcs.Count - 1])).frameno + "\";");
            grfldNo++;
        }
        fldNo++;
        jsFieldArray.Append(fieldArray.ToString().Trim());

        string gridType = string.Empty;
        string gridNo = string.Empty;
        if (((DcStruct)(dcs[dcs.Count - 1])).isgrid)
        {
            gridType = "GR.";
            gridNo = "F" + ((DcStruct)(dcs[dcs.Count - 1])).frameno;
        }
        else
        {
            gridType = "NG.";
            gridNo = "F0";
        }

        if (!string.IsNullOrEmpty(fld.expr))
        {
            string tempExpr;
            if ((fld.name.StartsWith("dc") && (fld.name.ToLower().EndsWith("_referimages") || fld.name.ToLower().EndsWith("_image") || fld.name.ToLower().EndsWith("_imagepath"))) && fld.expr.Contains(@"\") || fld.name == "axpattachmentpath")
                tempExpr = fld.expr.Replace(@"\", "\\\\");      // In order to avoid the skipping of backslash 
            else if (fld.name.ToLower().StartsWith("axpfilepath_") && gridType == "GR.")
                //tempExpr = fld.expr.Replace(@"\", "\\\\\\\\");
                tempExpr = fld.expr.Replace(@"\", "\\\\");
            else if (fld.name.ToLower().StartsWith("axpfilepath_") && gridType == "NG.")
                tempExpr = fld.expr.Replace(@"\", "\\\\");
            else if (fld.expr != "" && (fld.expr.StartsWith("{\\\\") || fld.expr.StartsWith("\\\\") || fld.expr.Contains("{\\") || fld.expr.Contains("\\")))
                tempExpr = fld.expr.Replace(@"\", "\\\\");
            else
                tempExpr = fld.expr.Replace("\"", "'");
            jsExpressionArray.Append(" ExpFldNames[" + exprNo + "]=\"" + gridType + "" + fld.name + "." + gridNo + ".expr\";Expressions[" + exprNo + "]=\"" + tempExpr + "\";");
            exprNo++;
        }

        return fld;
    }

    private string GetFldDependents(string result, string depList)
    {
        if (depList != null)
        {
            if (result == "")
                result = depList;
            else if (depList != "")
                result += "," + depList;
        }
        return result;
    }

    /// <summary>
    ///function to create and append javascript arrays for handling load single dc data for tabbed dc's             
    /// </summary>
    public void CreateTabArrays()
    {
        for (int i = 0; i < tabDCs.Count; i++)
        {
            if (tabDCs[i].ToString() != "" && tabDCStatus[i].ToString() != "")
            {
                jsTabDCArray.Append("TabDCs[" + i + "]=\"" + tabDCs[i].ToString() + "\";TabDCStatus[" + i + "]=\"" + tabDCStatus[i] + "\";");
            }
            else
            {
                break;
            }
        }

        for (int j = 0; j < pagePositions.Count; j++)
        {
            jsTabDCArray.Append("PagePositions[" + j + "]=\"" + pagePositions[j].ToString() + "\";VisibleDCs[" + j + "]=\"" + visibleDCs[j] + "\";");
        }
    }

    /// <summary>
    /// function to handle the pattern for the field values.
    /// </summary>
    /// <param name="patternNode"></param>
    /// <param name="fld"></param>
    private FieldStruct PatternCheck(XmlNode patternNode, FieldStruct fld)
    {
        fld.pattern = patternNode.InnerText;

        string patName = string.Empty;
        if (!((DcStruct)(dcs[dcs.Count - 1])).isgrid)
            patName = "ng_" + fld.name;
        else
            patName = "gr_" + fld.name;
        if (!string.IsNullOrEmpty(fld.pattern))
        {
            if (fld.pattern == "24H Format")
            {
                fld.fldTimeFormat = fld.pattern;
            }
            jsPatternArray.Append("PatternNames[" + patternNo + "]='" + patName + "';Patterns[" + patternNo + "]='" + fld.pattern + "';");
            patternNo++;
        }

        return fld;
    }

    /// <summary>
    /// Update the dimensions of the field.
    /// </summary>
    /// <param name="dimensionNode"></param>
    /// <param name="fld"></param>
    private FieldStruct FldDimensionCheck(XmlNode dimensionNode, FieldStruct fld)
    {
        if (dimensionNode.Attributes["lbltlhw"] != null)
            fld.lbltlhw = dimensionNode.Attributes["lbltlhw"].Value.ToString();
        else
            fld.lbltlhw = "";

        if (dimensionNode.Attributes["lblfont"] != null)
            fld.fieldLabelCss = dimensionNode.Attributes["lblfont"].Value.ToString();
        else
            fld.fieldLabelCss = "";

        if (dimensionNode.Attributes["font"] != null)
            fld.fieldCtrlCss = dimensionNode.Attributes["font"].Value.ToString();
        else
            fld.fieldCtrlCss = "";

        if (dimensionNode.Attributes["tlhw"] != null)
            fld.fldtlhw = dimensionNode.Attributes["tlhw"].Value.ToString();
        else
            fld.fldtlhw = "";

        if (dimensionNode.Attributes["wlbltlhw"] != null)
            fld.plbltlhw = dimensionNode.Attributes["wlbltlhw"].Value.ToString();
        else
            fld.plbltlhw = "";
        if (dimensionNode.Attributes["wtlhw"] != null)
            fld.pfldtlhw = dimensionNode.Attributes["wtlhw"].Value.ToString();
        else
            fld.pfldtlhw = "";

        if (dimensionNode.Attributes["ctype"] != null)
        {
            fld.ctype = dimensionNode.Attributes["ctype"].Value.ToString();
            if (fld.ctype != null && fld.ctype.ToLower() == "check box" && isfldDcGrid)
                fld.ctype = "";
        }
        else
            fld.ctype = "";

        if (dimensionNode.Attributes["sep"] != null)
            fld.fieldValSeparator = dimensionNode.Attributes["sep"].Value.ToString();


        if (dimensionNode.Attributes["cwid"] != null)
            fld.cwid = dimensionNode.Attributes["cwid"].Value.ToString();
        else
            fld.cwid = "";

        if (dimensionNode.Attributes["type"] != null)
            fld.type = dimensionNode.Attributes["type"].Value.ToString();
        else
            fld.type = "";

        if (dimensionNode.Attributes["color"] != null)
        {
            fld.fldColor = dimensionNode.Attributes["color"].Value;
            if (fld.fldColor != string.Empty)
            {
                fld.fldColor = fld.fldColor.Substring(2);
            }
        }
        else
            fld.fldColor = "";

        return fld;
    }

    /// <summary>
    /// Set the tooltip from the xml to tooltip variable of the field structure
    /// </summary>
    /// <param name="fldNodes"></param>
    /// <param name="fld"></param>
    /// <returns></returns>
    private FieldStruct SetTooltip(XmlNode fldNodes, FieldStruct fld)
    {
        foreach (XmlNode tlNode in fldNodes)
        {
            fld.tooltip += tlNode.InnerText.Replace(@"\", "\\\\") + "/";
        }
        return fld;
    }

    /// <summary>
    /// function to update sql,expression and fill dependent list.
    /// </summary>
    /// <param name="fmtNodes"></param>
    /// <param name="fld"></param>
    private FieldStruct SetValueFMT(XmlNode fmtNodes, FieldStruct fld)
    {
        fld.fieldDependents = string.Empty;
        foreach (XmlNode fmtNode in fmtNodes)
        {
            string fmText = "";
            fmText = fmtNode.InnerText.ToString();
            if (fmText != "")
                fld.fieldDependents += fmText;
        }
        //string strNewDeps = utilObj.IncludeDependents(transId, fld.name, fld.fieldDependents);
        //strNewDeps = utilObj.ExcludeDependents(transId, fld.name, strNewDeps);
        //fld.fieldDependents = strNewDeps;
        string fldDeps = GetSqlDependents(fld);
        string[] strDeps = fldDeps.Split('~');
        fld.fieldSqlDeps = strDeps[0].ToString();
        if (strDeps.Length > 1)
            fld.fieldExpDeps = strDeps[1].ToString();

        return fld;
    }

    /// <summary>
    /// Function which returns the sql dependents for a given field.
    /// </summary>
    /// <param name="fld"></param>
    /// <returns></returns>
    private string GetSqlDependents(FieldStruct fld)
    {
        string fldDeps = fld.fieldDependents;
        StringBuilder sqlDepFlds = new StringBuilder();
        StringBuilder sqlExpFlds = new StringBuilder();
        if (fldDeps != string.Empty)
        {
            string[] deps = fldDeps.Split(',');
            for (int i = 0; i < deps.Length; i++)
            {
                string depStr = deps[i].ToString();

                //|| depStr.Substring(0,1) == "f" - NOTE - here fill dependents have not been added to the sql dep list
                if (depStr.Substring(0, 1) == "s")
                {
                    if (sqlDepFlds.ToString() == string.Empty)
                        sqlDepFlds.Append(depStr);
                    else
                        sqlDepFlds.Append("," + depStr);
                }
                else if (depStr.Substring(0, 1) == "e")
                {
                    if (sqlExpFlds.ToString() == string.Empty)
                        sqlExpFlds.Append(depStr);
                    else
                        sqlExpFlds.Append("," + depStr);
                }

            }
        }

        return sqlDepFlds.ToString() + "~" + sqlExpFlds.ToString();
    }

    /// <summary>
    /// Update the moe details from the a7 node
    /// </summary>
    /// <param name="moeDetailNodes"></param>
    /// <param name="fld"></param>
    private FieldStruct UpdateDetailsofMOE(XmlNode moeDetailNodes, FieldStruct fld)
    {
        StringBuilder hashString = new StringBuilder();
        bool isUpdate = false;
        ArrayList a26Array = new ArrayList();
        ArrayList a26ArrayType = new ArrayList();
        ArrayList a63Array = new ArrayList();
        ArrayList a63ArrayType = new ArrayList();
        string tmpMOE = fld.moe.ToUpper();
        if (tmpMOE == moe.ACCEPT.ToString() || tmpMOE == moe.SELECT.ToString() || tmpMOE == moe.FILL.ToString())
            isUpdate = true;

        if (isUpdate)
        {
            foreach (XmlNode moeDetailNode in moeDetailNodes)
            {
                if (tmpMOE == moe.ACCEPT.ToString())
                {
                    if (moeDetailNode.Name == "a22")
                    {
                        if (!string.IsNullOrEmpty(moeDetailNode.InnerText))
                            fld.selectmode = "From Master";
                        else
                            fld.selectmode = string.Empty;

                        if (moeDetailNode.Attributes["pcount"] != null)
                            fld.pcount = int.Parse(moeDetailNode.Attributes["pcount"].Value);
                        else
                            fld.pcount = 0;

                        if (moeDetailNode.ChildNodes.Count > 0 && fld.pcount == 0)
                        {
                            if (moeDetailNode.ChildNodes.Count == 1 && moeDetailNode.ChildNodes[0].InnerXml != "")
                            {
                                if (fromListFlds == "")
                                    fromListFlds = fld.name;
                                else
                                    fromListFlds += "," + fld.name;
                            }
                        }
                        if (moeDetailNode.ChildNodes.Count > 0 && moeDetailNode.ChildNodes[0].InnerText != "")
                            fld.isFieldSql = "True";

                        //if (moeDetailNode.Attributes["stype"] != null && moeDetailNode.Attributes["stype"].Value.ToLower() == "accept")
                        if (moeDetailNode.Attributes["stype"] != null && moeDetailNode.Attributes["stype"].Value.ToLower() == "from api")
                        {
                            if (moeDetailNode.Attributes["apiname"] != null)
                                fld.fieldSelAPIName = moeDetailNode.Attributes["apiname"].Value;
                            if (moeDetailNode.Attributes["url"] != null)
                                fld.fieldSelAPI = moeDetailNode.Attributes["url"].Value;
                            if (moeDetailNode.Attributes["mapname"] != null)
                                fld.fieldSelAPIParams = moeDetailNode.Attributes["mapname"].Value;
                            if (moeDetailNode.Attributes["restype"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["restype"].Value;
                            if (moeDetailNode.Attributes["resformat"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["resformat"].Value;
                            if (moeDetailNode.Attributes["paramstr"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["paramstr"].Value;
                            if (moeDetailNode.Attributes["headerstr"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["headerstr"].Value;
                            if (moeDetailNode.Attributes["reqstr"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["reqstr"].Value;
                        }
                    }
                }
                else if (tmpMOE == moe.SELECT.ToString())
                {
                    if (moeDetailNode.Name == "a22")
                    {
                        if (moeDetailNode.Attributes["param"] != null)
                        {
                            if (moeDetailNode.Attributes["param"].Value == "y")
                                fld.refresh = 1;
                            else
                                fld.refresh = 0;
                        }

                        if (moeDetailNode.Attributes["txt"] != null)
                        {
                            if (moeDetailNode.Attributes["txt"].Value == "t")
                            {
                                fld.editcombo = true;
                                if (moeDetailNode.OuterXml != "" && moeDetailNode.OuterXml.ToLower().Contains("select "))
                                    fld.editcombo = CheckDynamicFilter(moeDetailNode.OuterXml);
                            }
                            else if (moeDetailNode.Attributes["txt"].Value == "f")
                                fld.editcombo = false;
                            else
                            {
                                fld.editcombo = CheckDynamicFilter(moeDetailNode.OuterXml);
                            }
                        }

                        if (moeDetailNode.Attributes["firesql"] != null)
                        {
                            if (moeDetailNode.Attributes["firesql"].Value == "True")
                                fld.firesql = true;
                            else
                                fld.firesql = false;
                        }

                        if (fld.firesql)
                            fld.selectmode = string.Empty;
                        else
                            fld.selectmode = "From Master";

                        if (moeDetailNode.Attributes["pcount"] != null)
                            fld.pcount = int.Parse(moeDetailNode.Attributes["pcount"].Value);
                        else
                            fld.pcount = 0;

                        if (moeDetailNode.Attributes["mulselect"] != null && moeDetailNode.Attributes["mulselect"].Value == "true")
                            fld.fldMultiSelect = fld.name;
                        else
                            fld.fldMultiSelect = string.Empty;
                        if (moeDetailNode.Attributes["mulsep"] != null)
                            fld.fldMultiSelectSep = moeDetailNode.Attributes["mulsep"].Value;
                        else
                            fld.fldMultiSelectSep = string.Empty;

                        if (moeDetailNode.ChildNodes.Count > 0 && moeDetailNode.ChildNodes[0].InnerText != "")
                            fld.isFieldSql = "True";
                        for (int i = 0; i < moeDetailNode.ChildNodes.Count; i++)
                        {
                            fld.fieldSqlQuery += " " + moeDetailNode.ChildNodes[i].InnerText;
                        }
                        if (moeDetailNode.Attributes["stype"] != null && moeDetailNode.Attributes["stype"].Value.ToLower() == "select from api")
                        {
                            fld.isFieldApi = "true";
                            if (moeDetailNode.Attributes["apiname"] != null)
                                fld.fieldSelAPIName = moeDetailNode.Attributes["apiname"].Value;
                            if (moeDetailNode.Attributes["url"] != null)
                                fld.fieldSelAPI = moeDetailNode.Attributes["url"].Value;
                            if (moeDetailNode.Attributes["mapname"] != null)
                                fld.fieldSelAPIParams = moeDetailNode.Attributes["mapname"].Value;
                            if (moeDetailNode.Attributes["restype"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["restype"].Value;
                            if (moeDetailNode.Attributes["resformat"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["resformat"].Value;
                            if (moeDetailNode.Attributes["paramstr"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["paramstr"].Value;
                            if (moeDetailNode.Attributes["headerstr"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["headerstr"].Value;
                            if (moeDetailNode.Attributes["reqstr"] != null)
                                fld.fieldSelAPIParams += "♠" + moeDetailNode.Attributes["reqstr"].Value;
                        }
                    }
                    else if (moeDetailNode.Name == "a23")
                    {
                        fld.autoselect = moeDetailNode.InnerText;
                    }
                    else if (moeDetailNode.Name == "a24")
                    {
                        fld.refreshOnSave = moeDetailNode.InnerText;
                    }
                    else if (moeDetailNode.Name == "a25")
                    {
                        foreach (XmlNode selectNodes in moeDetailNode)
                        {
                            if (fld.moeval == "")
                                fld.moeval = selectNodes.InnerText;
                            else
                                fld.moeval += "," + selectNodes.InnerText;
                        }

                        if (fld.moeval != "" && fld.moeval != null)
                        {
                            if (fromListFlds == "")
                                fromListFlds = fld.name;
                            else
                                fromListFlds += "," + fld.name;
                            fld.fieldIsFrmList = true;

                        }
                        else
                            fld.fieldIsFrmList = false;
                    }
                }
                else if (tmpMOE == moe.FILL.ToString())
                {
                    if (moeDetailNode.Name == "a27")
                        fld.srcfld = moeDetailNode.InnerText;
                    else if (moeDetailNode.Name == "a28")
                        fld.sqlfld = moeDetailNode.InnerText;

                }

                if (moeDetailNode.Name == "a26")
                {
                    if ((moeDetailNode.Attributes["stype"] != null))
                    {
                        if (moeDetailNode.Attributes["stype"].Value == "tstruct" | moeDetailNode.Attributes["stype"].Value == "sql")
                        {
                            a26Array.Add(fld.caption);
                            a26ArrayType.Add(fld.datatype);
                        }
                    }
                }
                else if (moeDetailNode.Name == "a63")
                {
                    foreach (XmlNode cnNode in moeDetailNode)
                    {
                        if ((cnNode.Attributes["cn"] != null))
                        {
                            a63Array.Add(cnNode.Attributes["cn"].Value.Trim());
                            a63ArrayType.Add("");
                        }
                    }
                }


            }
            //fld.AdvSrchFlds
            if (a63Array.Count == 0)
            {
                fld.AdvSrchFlds = a26Array;
                fld.AdvSrchFldsType = a26ArrayType;
            }
            else
            {
                fld.AdvSrchFlds = a63Array;
                fld.AdvSrchFldsType = a63ArrayType;
            }
        }
        return fld;
    }

    private bool CheckDynamicFilter(string _fldOXml)
    {
        bool isDynamic = false;
        try
        {
            if (_fldOXml != "" && _fldOXml.ToLower().Contains("{dynamicfilter "))
                isDynamic = true;
            else
                isDynamic = false;
        }
        catch (Exception ex)
        {
            isDynamic = false;
        }
        return isDynamic;
    }

    /// <summary>
    /// Update the fillgrid list.
    /// </summary>
    /// <param name="fgNodes"></param>
    private void XmlLoadFillgrid(XmlNode fgNodes)
    {
        FGStruct fg = new FGStruct();
        StringBuilder fgsql = new StringBuilder();
        foreach (XmlNode fgNode in fgNodes)
        {
            if (fgNode.Name == "a1")
            {
                fg.fgName = fgNode.InnerText;

            }
            else if (fgNode.Name == "a4")
            {
                fg.fgcaption = fgNode.InnerText;
            }
            else if (fgNode.Name == "a5")
            {
                fgsql.Append(fgNode.Attributes["plist"].Value.ToString() + "");
                fg.fgSqlParams = fgsql.ToString();
            }
            else if (fgNode.Name == "a6")
            {
                fg.fgtargetdc = fgNode.InnerText;
            }
            else if (fgNode.Name == "a7")
            {
                fg.fgSqlFldName = new ArrayList();
                fg.fgDcFldName = new ArrayList();
                foreach (XmlNode mapNode in fgNode)
                {
                    fg.fgDcFldName.Add(mapNode.Name);
                    fg.fgSqlFldName.Add(mapNode.InnerText.ToLower());
                }
            }
            else if (fgNode.Name == "a8")
            {
                fg.fgmultiselect = fgNode.InnerText;
            }
            else if (fgNode.Name == "a9")
            {
                fg.fgautoshow = fgNode.InnerText;
            }
            else if (fgNode.Name == "a10")
            {
                fg.fgSourceDC = fgNode.InnerText;
            }
            else if (fgNode.Name == "a16")
            {
                fg.fgtotalColVal = fgNode.InnerText;
            }
            else if (fgNode.Name == "a17")
            {
                fg.fgValExpression = fgNode.InnerText;
            }
            else if (fgNode.Name == "a13" && fgNode.InnerText.ToLower() == "true")
            {
                fg.FGExecOnSave = fg.fgtargetdc;
            }
            else if (fgNode.Name == "a18")
            {
                if (fgNode.InnerText == "Initialize grid and add")
                    fg.FGCondition = Constants.INIT;
                else if (fgNode.InnerText == "Add only when grid is empty")
                    fg.FGCondition = Constants.AOWE;
                else if (fgNode.InnerText == "Append selected rows to grid" || fgNode.InnerText == "Append rows to grid")
                    fg.FGCondition = Constants.APPEND;
            }
        }
        fgs.Add(fg);
        jsFieldArray.Append("FillAutoShow[" + fgCount + "]=\"" + ((FGStruct)(fgs[fgCount])).fgautoshow.ToString().ToLower() + "\";FillMultiSelect[" + fgCount + "]=\"" + ((FGStruct)(fgs[fgCount])).fgmultiselect.ToString().ToLower() + "\";FillParamDCs[" + fgCount + "]=\"" + ((FGStruct)(fgs[fgCount])).fgtargetdc.ToString() + "\";FillParamFld[" + fgCount + "]=\"" + ((FGStruct)(fgs[fgCount])).fgSqlParams.ToString() + "\";FillCondition[" + fgCount + "]=\"" + fg.FGCondition + "\";FillSourceDc[" + fgCount + "]=\"" + fg.fgSourceDC + "\";FillGridName[" + fgCount + "]=\"" + fg.fgName + "\";FillGridVExpr[" + fgCount + "]=\"" + fg.fgValExpression + "\";FillGridTotalFldVal[" + fgCount + "]=\"" + fg.fgtotalColVal + "\";FillGridExecOnSave[" + fgCount + "]=\"" + fg.FGExecOnSave + "\";FillGridCaption[" + fgCount + "]=\"" + fg.fgcaption + "\";");
        fgCount++;

    }

    /// <summary>
    /// Function to construct the Action buttons in the tstruct.
    /// </summary>
    /// <param name="btnNodes"></param>
    /// <param name="btnType"></param>
    private void XmlPageLoadButton(XmlNode btnNodes, string btnType)
    {
        if ((btnNodes.Attributes["cat"] != null) && (btnNodes.Attributes["cat"].Value == "btn"))
        {
            string caption = string.Empty;
            string task = string.Empty;
            string action = string.Empty;
            string hint = string.Empty;
            string dimension = string.Empty;
            string btnTop = string.Empty;
            string btnLeft = string.Empty;
            string btnHeight = string.Empty;
            string btnWidth = string.Empty;
            string image = string.Empty;
            string fileupload = string.Empty;
            string cancelBtn = string.Empty;
            string fmessage = string.Empty;
            string ftype = string.Empty;
            string btnId = string.Empty;
            string position = string.Empty;

            int frameNo = 0;
            if (btnNodes.Attributes["parent"] != null)
            {
                frameNo = Convert.ToInt32(btnNodes.Attributes["parent"].Value.Substring(3));
            }

            caption = btnNodes.Attributes["caption"].Value;
            task = btnNodes.Attributes["task"].Value;
            action = btnNodes.Attributes["action"].Value;
            hint = btnNodes.Attributes["hint"].Value;
            dimension = btnNodes.Attributes["tlhw"].Value;
            image = btnNodes.Attributes["img"].Value;
            position = btnNodes.Attributes["position"].Value;
            if (btnNodes.Attributes["fileupload"] != null)
            {
                fileupload = btnNodes.Attributes["fileupload"].Value;
                if (btnNodes.Attributes["fmsg"] != null)
                    fmessage = btnNodes.Attributes["fmsg"].Value;
                if (btnNodes.Attributes["ftype"] != null)
                    ftype = btnNodes.Attributes["ftype"].Value;
            }

            if (btnNodes.Attributes["rem"] != null)
            {
                cancelBtn = btnNodes.Attributes["rem"].Value;
            }
            string divDirection = "left";
            if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
            {
                divDirection = "right";
            }
            btnId = btnNodes.Name;

            if (dimension != string.Empty)
            {
                string[] tlhw = dimension.Split(',');
                if (EnableOldTheme == "true")
                    btnTop = Math.Floor((Convert.ToInt32(tlhw[0].ToString()) * 1.2)).ToString() + "px";
                else
                    btnTop = Math.Floor((Convert.ToInt32(tlhw[0].ToString()) * 1.5)).ToString() + "px";
                btnLeft = tlhw[1].ToString() + "px";
                btnHeight = tlhw[2].ToString() + "px";
                btnWidth = tlhw[3].ToString() + "px";
                FieldStruct dummy = new FieldStruct();
                dummy.visibility = false;
                dummy.fldlength = 15;
                dummy.fldFrameNo = frameNo;
                dummy.ctype = "button";
                dummy.datatype = "special";
                dummy.name = btnId;

                if (action != "")
                {
                    btnHtml.Append("<div  class=\" grid-stack-item\"" + xywhGridStack(dummy) + ">");
                    btnHtml.Append("<div class=\"grid-stack-item-content\"></div>");
                    btnHtml.Append("<div id=\"dv" + btnId + "\" class=\"labelcol inputs grid-stack-item-content form-group row w-100 agform d-block align-self-end\">");
                    btnHtml.Append("<div class=\"fld-wrap1\"><label class=\"labelcolid\" for=\"" + btnId + "\" ></label></div>");
                    //                btnHtml.Append("<span class=\"AxpTstBtn\" >");
                    string btnFunction = string.Empty;

                    if (!string.IsNullOrEmpty(fileupload))
                    {
                        if (fileupload == "y")
                        {
                            btnFunction = " onclick='javascript:CallFileUploadAction(" + (char)34 + action + (char)34 + "," + (char)34 + fmessage + (char)34 + "," + (char)34 + ftype + (char)34 + ");' ";
                            if (string.IsNullOrEmpty(image))
                                btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><input id=\"" + btnId + "\"  type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                            else
                                btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><img style=\"" + position + ":10px\" id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input id=\"" + btnId + "\"  type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                        }
                        else if (fileupload == "a")
                        {
                            btnFunction = " onclick='javascript:AttachFiles();' ";
                            if (string.IsNullOrEmpty(image))
                                btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><input id=\"" + btnId + "\" type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                            else
                                btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><img style=\"" + position + ":10px\" id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input id=\"" + btnId + "\" type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                        }
                        else
                        {
                            if ((fileupload.IndexOf("\\") != -1))
                            {
                                fileupload = fileupload.Replace("\\", "\\\\");
                            }
                            btnFunction = " onclick='javascript:CallAction(" + (char)34 + action + (char)34 + "," + (char)34 + fileupload + (char)34 + ");' ";
                            if (string.IsNullOrEmpty(image))
                                btnHtml.Append("<div class=\"input-group\"><input type=button " + btnFunction.ToString() + " class=\"tstformbutton btn btn-sm col-12 m-auto shadow-sm btn btn-white btn-color-gray-900 btn-active-primary\" value=\"" + caption + "\" title=\"" + hint + "\"></div>");
                            else
                                btnHtml.Append("<img style=\"" + position + ":10px\" id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input type=button " + btnFunction.ToString() + " class=\"form-control hotbtn btn\" value=\"" + caption + "\" title=\"" + hint + "\">");
                        }
                    }
                    else
                    {
                        btnFunction = " onclick='javascript:CallAction(" + (char)34 + action + (char)34 + "," + (char)34 + fileupload + (char)34 + "," + (char)34 + string.Empty + (char)34 + "," + (char)34 + cancelBtn + (char)34 + ");' ";
                        if (!string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(image))
                        {
                            //Display Image with caption as hint.
                            //btnHtml.Append("<a href=\"javascript:void(0)\" class='action form-control'><img id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur\"></a>");
                            btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><img style=\"" + position + ":10px\" id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input id=\"" + btnId + "\"  type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                        }
                        else if (!string.IsNullOrEmpty(image) && string.IsNullOrEmpty(caption))
                        {
                            //Display only image
                            //btnHtml.Append("<a href=\"javascript:void(0)\" class='action form-control'><img id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + hint + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur\"></a>");
                            btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><img style=\"" + position + ":10px\" id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input id=\"" + btnId + "\"  type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                        }
                        else if (!string.IsNullOrEmpty(caption) & string.IsNullOrEmpty(image))
                        {
                            //Display button with Caption
                            btnHtml.Append("<div class=\"input-group\"><input type=button " + btnFunction.ToString() + " value=\"" + caption + "\" tooltip=\"" + caption + "\" class=\"tstformbutton btn btn-sm col-12 m-auto shadow-sm btn btn-white btn-color-gray-900 btn-active-primary\" title=\"" + hint + "\"></div>");
                        }
                        else
                        {
                            //throw error
                            btnHtml.Append("");
                        }
                    }
                    btnHtml.Append("</div></div>");
                }
                else if (task != "tasks")
                {
                    btnHtml.Append("<div  class=\" grid-stack-item\"" + xywhGridStack(dummy) + ">");
                    btnHtml.Append("<div class=\"grid-stack-item-content\"></div>");
                    btnHtml.Append("<div id=\"dv" + btnId + "\" class=\"labelcol inputs grid-stack-item-content form-group row w-100 agform d-block align-self-end\">");
                    btnHtml.Append("<div class=\"fld-wrap1\"><label class=\"labelcolid\" for=\"" + btnId + "\" ></label></div>");

                    string btnFunction = string.Empty;
                    btnFunction = GetBtnFunction(transId, task);

                    //TODO: provide the button like any other case, on click it should alert "No task defined"
                    //This button can be added for customisation, hence need not display the above.\
                    if (string.IsNullOrEmpty(image))
                        btnHtml.Append("<div class=\"input-group\"><input type='button' id=\"" + btnId + "\" value=\"" + caption + "\" class=\"tstformbutton btn btn-sm col-12 m-auto shadow-sm btn btn-white btn-color-gray-900 btn-active-primary\" title=\"" + hint + "\" " + btnFunction + "></div>");
                    else
                        btnHtml.Append("<img style=\"" + position + ":10px\" id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input type='button' style =\"\" value=\"" + caption + "\" class=\"hotbtn btn form-control\" title=\"" + hint + "\" " + btnFunction + ">");
                    btnHtml.Append("</div></div>");
                }
            }

            pageActionButtons.Add(btnHtml.ToString());
            pageActionDcNo.Add(dcs.Count.ToString());
            btnHtml = new StringBuilder();
        }
    }

    private void XmlPageLoadDWBButton(XmlDocument FormBtnNodes, int frameNo)
    {
        XmlNodeList formbtnlst = FormBtnNodes.SelectNodes("//root/dc" + frameNo);
        if (formbtnlst.Count > 0)
        {
            string colFldWidth = "", colFldGridStackItem = "", colDivInputPadding = "";
            if (axdesignJObject.dcLayout != null && axdesignJObject.dcLayout != "" && axdesignJObject.dcLayout != "default")
            {
                colFldWidth = "<div class=\"fld-wrap3 col-sm-12\">";
                colFldGridStackItem = " colFldGridStackWidth ";
                colDivInputPadding = " colDivInputPadding ";
            }
            foreach (XmlNode formbtns in formbtnlst)
            {
                foreach (XmlNode btnNodes in formbtns)
                {
                    XmlNode ficonNode = btnNodes.SelectSingleNode("icon");
                    if ((btnNodes.Attributes["cat"] != null) && (btnNodes.Attributes["cat"].Value == "btn"))
                    {
                        string caption = string.Empty;
                        string task = string.Empty;
                        string action = string.Empty;
                        string hint = string.Empty;
                        string dimension = string.Empty;
                        string btnTop = string.Empty;
                        string btnLeft = string.Empty;
                        string btnHeight = string.Empty;
                        string btnWidth = string.Empty;
                        string image = string.Empty;
                        string fileupload = string.Empty;
                        string cancelBtn = string.Empty;
                        string fmessage = string.Empty;
                        string ftype = string.Empty;
                        string btnId = string.Empty;
                        string position = string.Empty;
                        bool isGridBtn = false;
                        string gridBtnPosition = string.Empty;
                        bool script = false;

                        caption = btnNodes.Attributes["caption"].Value;
                        task = btnNodes.Attributes["task"].Value;
                        action = btnNodes.Attributes["action"].Value;
                        hint = btnNodes.Attributes["hint"].Value;
                        dimension = btnNodes.Attributes["tlhw"].Value;
                        image = btnNodes.Attributes["img"].Value;
                        position = btnNodes.Attributes["position"].Value;
                        script = btnNodes.Attributes["script"].Value.ToLower() == "true" ? true : false;
                        string iconStyle = string.Empty;

                        if (ficonNode != null)
                        {
                            if (ficonNode.SelectSingleNode("text") != null)
                            {
                                string ictext = ficonNode.SelectSingleNode("text").InnerText;
                                string icaddclass = ficonNode.SelectSingleNode("addClass").InnerText;
                                iconStyle = "<i class='" + icaddclass + "'>" + ictext + "</i>";
                            }
                            else
                            {
                                iconStyle = "<img src='" + caption + "' class='tbIcon'>";
                            }
                        }

                        if (position.Split('~').Length > 1)
                        {
                            isGridBtn = true;
                            gridBtnPosition = position.Split('~')[1];
                        }

                        if (btnNodes.Attributes["fileupload"] != null)
                        {
                            fileupload = btnNodes.Attributes["fileupload"].Value;
                            if (btnNodes.Attributes["fmsg"] != null)
                                fmessage = btnNodes.Attributes["fmsg"].Value;
                            if (btnNodes.Attributes["ftype"] != null)
                                ftype = btnNodes.Attributes["ftype"].Value;
                        }

                        if (btnNodes.Attributes["rem"] != null)
                        {
                            cancelBtn = btnNodes.Attributes["rem"].Value;
                        }
                        string divDirection = "left";
                        if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
                        {
                            divDirection = "right";
                        }
                        btnId = btnNodes.Name;
                        string designFontStyle = "";
                        if (axdesignJObject.buttonFieldFont != null && axdesignJObject.buttonFieldFont.Count > 0 && btnId != "")
                        {
                            var frbtnfld = axdesignJObject.buttonFieldFont.Where(elm => elm.id == btnId).ToList();
                            if (frbtnfld != null && frbtnfld.Count > 0)
                            {
                                designFontStyle = " style='" + frbtnfld[0].fontFamilly + "'";
                            }
                        }
                        if (dimension != string.Empty)
                        {
                            string[] tlhw = dimension.Split(',');
                            if (EnableOldTheme == "true")
                                btnTop = Math.Floor((Convert.ToInt32(tlhw[0].ToString()) * 1.2)).ToString() + "px";
                            else
                                btnTop = Math.Floor((Convert.ToInt32(tlhw[0].ToString()) * 1.5)).ToString() + "px";
                            btnLeft = tlhw[1].ToString() + "px";
                            btnHeight = tlhw[2].ToString() + "px";
                            btnWidth = tlhw[3].ToString() + "px";
                            FieldStruct dummy = new FieldStruct();
                            dummy.visibility = false;
                            dummy.fldlength = 15;
                            dummy.fldFrameNo = frameNo;
                            dummy.ctype = "button";
                            dummy.datatype = "special";
                            dummy.name = btnId;
                            if (!isGridBtn)
                            {
                                if (action != "" && action != "Custom")
                                {
                                    btnHtml.Append("<div  class=\" grid-stack-item " + colFldGridStackItem + "\"" + xywhGridStack(dummy) + ">");
                                    btnHtml.Append("<div class=\"grid-stack-item-content\"></div>");
                                    btnHtml.Append("<div id=\"dv" + btnId + "\" class=\"labelcol inputs grid-stack-item-content form-group row w-100 agform d-block align-self-end " + colDivInputPadding + "\">");
                                    btnHtml.Append(colFldWidth);
                                    btnHtml.Append("<div class=\"fld-wrap1\"><label class=\"labelcolid\" for=\"" + btnId + "\" ></label></div>");
                                    string btnFunction = string.Empty;

                                    if (!string.IsNullOrEmpty(fileupload))
                                    {
                                        if (fileupload == "y")
                                        {
                                            btnFunction = " onclick='javascript:CallFileUploadAction(" + (char)34 + action + (char)34 + "," + (char)34 + fmessage + (char)34 + "," + (char)34 + ftype + (char)34 + ");' ";
                                            if (string.IsNullOrEmpty(image))
                                                btnHtml.Append("<a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " alt='" + hint + "' title='" + hint + "' class='handCur tstformbutton form-control coldbtn' " + designFontStyle + ">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a><input type=hidden id=cb_sactbu name=cb_sactbu>");
                                            else
                                                btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><img id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input id=\"" + btnId + "\"  type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                                        }
                                        else if (fileupload == "a")
                                        {
                                            btnFunction = " onclick='javascript:AttachFiles();' ";
                                            if (string.IsNullOrEmpty(image))
                                                btnHtml.Append("<a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " alt='" + hint + "' title='" + hint + "' class='handCur tstformbutton form-control coldbtn' " + designFontStyle + ">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a><input type=hidden id=cb_sactbu name=cb_sactbu>");
                                            else
                                                btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><img id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input id=\"" + btnId + "\" type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                                        }
                                        else
                                        {
                                            if ((fileupload.IndexOf("\\") != -1))
                                            {
                                                fileupload = fileupload.Replace("\\", "\\\\");
                                            }
                                            btnFunction = " onclick='javascript:CallAction(\"" + action + "\",\"" + fileupload + "\",\"\",\"\",\"\",\"\",\"" + script.ToString().ToLower() + "\");' ";
                                            if (string.IsNullOrEmpty(image))
                                                btnHtml.Append("<div class=\"input-group\"><a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " alt='" + hint + "' title='" + hint + "' class='tstformbutton btn btn-sm col-12 m-auto shadow-sm btn btn-white btn-color-gray-900 btn-active-primary' " + designFontStyle + ">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></div>");
                                            else
                                                btnHtml.Append("<img id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input type=button " + btnFunction.ToString() + " class=\"form-control hotbtn btn\" value=\"" + caption + "\" title=\"" + hint + "\">");
                                        }
                                    }
                                    else
                                    {
                                        btnFunction = " onclick='javascript:CallAction(\"" + action + "\",\"" + fileupload + "\",\"\",\"" + cancelBtn + "\",\"\",\"\",\"" + script.ToString().ToLower() + "\");' ";
                                        if (!string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(image))
                                        {
                                            //Display Image with caption as hint.
                                            btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><img id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input id=\"" + btnId + "\"  type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                                        }
                                        else if (!string.IsNullOrEmpty(image) && string.IsNullOrEmpty(caption))
                                        {
                                            //Display only image
                                            btnHtml.Append("<input type=hidden id=cb_sactbu name=cb_sactbu><img id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input id=\"" + btnId + "\"  type=button " + btnFunction.ToString() + " class=\"hotbtn btn  form-control\" value=\"" + caption + "\" title=\"" + hint + "\">");
                                        }
                                        else if (!string.IsNullOrEmpty(caption) & string.IsNullOrEmpty(image))
                                        {
                                            //Display button with Caption
                                            btnHtml.Append("<div class=\"input-group\"><a href=\"javascript:void(0)\" id=\"" + btnId + "\" " + btnFunction.ToString() + " alt=\"" + hint + "\" title=\"" + hint + "\" class=\"tstformbutton btn btn-sm col-12 m-auto shadow-sm btn btn-white btn-color-gray-900 btn-active-primary\" " + designFontStyle + ">" + iconStyle + "<span class=\"tbCaption\">" + caption + "</span></a></div>");
                                        }
                                        else
                                        {
                                            //throw error
                                            btnHtml.Append("");
                                        }
                                    }
                                    if (colFldWidth != "")
                                        btnHtml.Append("</div>");
                                    btnHtml.Append("</div></div>");
                                }
                                else if (action == "Custom")
                                {
                                    btnHtml.Append("<div  class=\" grid-stack-item " + colFldGridStackItem + "\"" + xywhGridStack(dummy) + ">");
                                    btnHtml.Append("<div class=\"grid-stack-item-content\"></div>");
                                    btnHtml.Append("<div id=\"dv" + btnId + "\" class=\"labelcol inputs grid-stack-item-content form-group row w-100 agform d-block align-self-end " + colDivInputPadding + "\">");
                                    btnHtml.Append(colFldWidth);
                                    btnHtml.Append("<div class=\"fld-wrap3\"><label class=\"labelcolid\" for=\"" + btnId + "\" ></label></div>");

                                    string btnFunction = string.Empty;
                                    btnFunction = " onclick='javascript:" + btnId + "onclick();' ";
                                    if (string.IsNullOrEmpty(image))
                                        //btnHtml.Append("<a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " alt='" + caption + "' title='" + caption + "' class='handCur tstformbutton form-control coldbtn' " + designFontStyle + ">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a>");
                                        btnHtml.Append("<div class=\"input-group\"><a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " alt='" + caption + "' title='" + caption + "' class='tstformbutton btn btn-sm col-12 m-auto shadow-sm btn btn-white btn-color-gray-900 btn-active-primary' " + designFontStyle + ">" + caption + "</a></div>");
                                    else
                                        btnHtml.Append("<img id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input type='button' style =\"\" value=\"" + caption + "\" class=\"hotbtn btn form-control\" title=\"" + hint + "\" " + btnFunction + ">");
                                    if (colFldWidth != "")
                                        btnHtml.Append("</div>");
                                    btnHtml.Append("</div></div>");
                                }
                                else if (task != "tasks")
                                {
                                    btnHtml.Append("<div  class=\" grid-stack-item " + colFldGridStackItem + "\"" + xywhGridStack(dummy) + ">");
                                    btnHtml.Append("<div class=\"grid-stack-item-content\"></div>");
                                    btnHtml.Append("<div id=\"dv" + btnId + "\" class=\"labelcol inputs grid-stack-item-content form-group row w-100 agform d-block align-self-end " + colDivInputPadding + "\">");
                                    btnHtml.Append(colFldWidth);
                                    btnHtml.Append("<div class=\"fld-wrap1\"><label class=\"labelcolid\" for=\"" + btnId + "\" ></label></div>");

                                    string btnFunction = string.Empty;
                                    btnFunction = GetBtnFunction(transId, task);

                                    //TODO: provide the button like any other case, on click it should alert "No task defined"
                                    //This button can be added for customisation, hence need not display the above.\
                                    if (string.IsNullOrEmpty(image))
                                        btnHtml.Append("<div class=\"input-group\"><a href=\"javascript:void(0)\" id='" + btnId + "' " + btnFunction.ToString() + " alt='" + caption + "' title='" + caption + "' class='tstformbutton btn btn-sm col-12 m-auto shadow-sm btn btn-white btn-color-gray-900 btn-active-primary' " + designFontStyle + ">" + iconStyle + "<span class='tbCaption'>" + caption + "</span></a></div>");
                                    else
                                        btnHtml.Append("<img id='" + btnId + "' src=\"../AxpImages/" + image + "\" alt='" + caption + "' " + btnFunction.ToString() + " title='" + hint + "' class=\"handCur tstbtnIcon\"><input type='button' style =\"\" value=\"" + caption + "\" class=\"hotbtn btn form-control\" title=\"" + hint + "\" " + btnFunction + ">");
                                    if (colFldWidth != "")
                                        btnHtml.Append("</div>");
                                    btnHtml.Append("</div></div>");
                                }
                            }
                            else
                            {
                                if (gridBtnPosition == "rowlevel")
                                {
                                    string btnFunction = string.Empty;
                                    if (action == "Custom")
                                        btnFunction = " onclick='javascript:" + btnId + "onclick(this);' ";
                                    else
                                        btnFunction = " onclick=\"javascript:CallAction('" + action + "','','','','','','" + script.ToString().ToLower() + "');\" ";
                                    string gsbtn = "<a href=\"javascript:void(0)\" class=\"Grdlnk axpBtnCustom\" id=\"" + btnId + "\" " + btnFunction + ">" + caption + "</a>";
                                    gridScriptButtons.Add("<th id=\"th-" + btnId + "\" style=\"width:100px;\" class=\"fw-boldest\"><div id=\"th-" + btnId + "-sizer\"></div><div class='thhead'></div></th>♠<div class=\"gridElement form-group grid-stack-item\" style=\"\"><div class=\"fld-wrap3\"><div>" + gsbtn + "</div></div>♠<div class=\"gridElement form-group grid-stack-item\" " + xywhGridStack(dummy) + " id=\"dvGrid" + btnId + "\" style=\"\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">1</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div class=\"grid-stack-item-content\"><div>" + gsbtn + "</div></div></div>");
                                    gridScriptDcNo.Add(dcs.Count.ToString());
                                }
                                else if (gridBtnPosition == "headerlevel")
                                {
                                    string btnFunction = string.Empty;
                                    if (action == "Custom")
                                        btnFunction = " onclick='javascript:" + btnId + "onclick();' ";
                                    else
                                        btnFunction = " onclick=\"javascript:CallAction('" + action + "','','','','','','" + script.ToString().ToLower() + "');\" ";
                                    gridHeaderScriptButtons.Add("<a class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm gridheaderbutton\" id=\"" + btnId + "\" " + btnFunction + " title=\"" + caption + "\"><span class=\"material-icons material-icons-style material-icons-3\">check_box_outline_blank</span></a>");
                                    gridHeaderScriptDcNo.Add(dcs.Count.ToString());
                                }
                            }
                        }

                        pageActionButtons.Add(btnHtml.ToString());
                        pageActionDcNo.Add(dcs.Count.ToString());
                        btnHtml = new StringBuilder();
                    }
                }
            }
        }
    }

    private string GetBtnFunction(string transId, string task)
    {
        switch (task)
        {
            case "new":
                return " onclick='javascript:NewTstruct();' ";
            case "save":
                return " onclick='javascript:FormSubmit();' ";
            case "search":
                return " onclick='javascript:OpenSearch(" + (char)34 + transId + (char)34 + ");' ";
            case "remove":
                return " onclick='javascript:DeleteTstruct();' ";
            case "print":
                return " onclick='javascript:OpenPrint(" + (char)34 + transId + (char)34 + ");' ";
            case "pdf":
                return " onclick='javascript:OpenPdfDocList();'";
            case "view history":
                return " onclick='javascript:OpenHistory(" + (char)34 + transId + (char)34 + ");' ";
            case "listview":
                return " onclick='javascript:CallListView(" + (char)34 + transId + (char)34 + ");' ";
            case "attach":
                return " onclick='javascript:AttachFiles();' ";
            case "preview":
                return " onclick='javascript:OpenPrint(" + (char)34 + transId + (char)34 + ");' ";
            default:
                return " onclick='javascript:AlertNoAction();' ";
        }
    }

    /// <summary>
    /// Update the button list.
    /// </summary>
    /// <param name="btnNodes"></param>
    /// <param name="btnType"></param>
    private void XmlLoadButton(XmlNode btnNodes, string btnType)
    {
        try
        {
            foreach (XmlNode btnNode in btnNodes)
            {
                if ((btnNode.Attributes["cat"] != null) && (btnNode.Attributes["cat"].Value == "btn"))
                {
                    ButtonStruct btn = new ButtonStruct();
                    btn.visible = "true";
                    if (btnNode.Attributes["caption"] != null)
                        btn.caption = btnNode.Attributes["caption"].Value;
                    else
                        btn.caption = string.Empty;

                    if (btnNode.Attributes["task"] != null)
                        btn.task = btnNode.Attributes["task"].Value;
                    else
                        btn.task = string.Empty;
                    if (btnNode.Attributes["action"] != null)
                        btn.action = btnNode.Attributes["action"].Value;
                    else
                        btn.action = string.Empty;
                    if (btnNode.Attributes["hint"] != null)
                        btn.hint = btnNode.Attributes["hint"].Value;
                    else
                        btn.hint = "";
                    if (btnNode.Attributes["tlhw"] != null)
                        btn.dimension = btnNode.Attributes["tlhw"].Value;
                    else
                        btn.dimension = string.Empty;
                    if (btnNode.Attributes["img"] != null)
                        btn.image = btnNode.Attributes["img"].Value;
                    else
                        btn.image = string.Empty;
                    if (btnNode.Attributes["fileupload"] != null)
                    {
                        btn.fileupload = btnNode.Attributes["fileupload"].Value;
                        if (btnNode.Attributes["fmessage"] != null)
                            btn.fmessage = btnNode.Attributes["fmessage"].Value;
                        if (btnNode.Attributes["ftype"] != null)
                            btn.ftype = btnNode.Attributes["ftype"].Value;
                    }
                    if (btnNode.Attributes["rem"] != null)
                    {
                        btn.cancelBtn = btnNode.Attributes["rem"].Value;
                    }

                    //if the button is task button, needs to be added to the task button list
                    if (btn.task == "tasks")
                    {
                        if (btnNode.ChildNodes.Count > 1) SortXMLElements(btnNode);
                        XmlNodeList taskNodes = btnNode.ChildNodes;
                        foreach (XmlNode taskNode in taskNodes)
                        {
                            if (taskNode.Name != "icon")
                            {
                                ButtonStruct tskBtn = new ButtonStruct();
                                tskBtn.caption = taskNode.Attributes["caption"].Value;
                                tskBtn.action = taskNode.Attributes["action"].Value;
                                tskBtn.task = taskNode.Attributes["task"].Value;
                                if (taskNode.Attributes["fileupload"] != null)
                                    tskBtn.fileupload = taskNode.Attributes["fileupload"].Value;
                                else
                                    tskBtn.fileupload = string.Empty;

                                taskBtns.Add(tskBtn);
                            }

                        }
                        // add the task button to the buttons list only if task button contains any
                        // task
                        if (taskBtns.Count > 0)
                        {
                            btns.Add(btn);
                        }
                    }
                    else
                    {
                        btns.Add(btn);
                    }
                }
                else if ((btnNode.Attributes["t"] != null) && (btnNode.Attributes["t"].Value == "img"))
                {
                    string btnImage = string.Empty;
                    string[] btnDim = "".Split(',');
                    if (btnNode.Attributes["f"] != null)
                        btnImage = btnNode.Attributes["f"].Value;
                    if (btnImage.IndexOf("\\") != -1)
                        btnImage = btnImage.Substring(btnImage.LastIndexOf("\\") + 1);
                    if (!utilObj.IsImageAvailable(btnImage))
                        btnImage = string.Empty;
                    if (btnNode.Attributes["tlhw"] != null)
                        btnDim = btnNode.Attributes["tlhw"].Value.Split(',');
                    string imgFrameNo = string.Empty;
                    if (btnNode.Attributes["p"] != null)
                    {
                        imgFrameNo = btnNode.Attributes["p"].Value;
                        imgFrameNo = imgFrameNo.Substring(3);
                    }

                    /*This code is commneted for now By Avi kant because while uploading image from desktop images is saving inside
                     *  c:\ folder which is not accesssable in Code which is showing empty Image 
                     */
                    //customImageHtml.Append("<span style=\"left:" + btnDim[1] + "px;

                    //StringBuilder customImageHtml = new StringBuilder();
                    //customImageHtml.Append("<div id=\"dvCustImg\">"); position: absolute; top:" + btnDim[0] + "px;height:" + btnDim[2] + ";width:" + btnDim[3] + "\" > ");
                    //customImageHtml.Append("<image style=\"height:" + btnDim[2] + "px;width:" + btnDim[3] + "px;\" src=\"../AxpImages/" + btnImage + "\" />");
                    //customImageHtml.Append("</span>");
                    //customImageHtml.Append("</div>");
                    //customLabels.Add(customImageHtml.ToString());
                    //customLblDcNo.Add(imgFrameNo);

                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in XmlLoadButton - btnxml:" + btnNodes.OuterXml.ToString() + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "XmlLoadButton-" + transId, "new", "true");
        }
    }

    /// <summary>
    /// load buttons for DWB toolbar. 
    /// </summary>
    private ArrayList XmlLoadDWBButtons(XmlNode btnNodes)
    {
        ArrayList btnsArr = new ArrayList();
        try
        {
            foreach (XmlNode btnNode in btnNodes)
            {
                ButtonStruct btn = new ButtonStruct();
                IconStruct icon = new IconStruct();
                btnApiStruct btnapi = new btnApiStruct();
                XmlNode iconNode = btnNode.SelectSingleNode("icon");
                ArrayList childnodes = new ArrayList();
                if (btnNode.Name != "icon" && btnNode.Name != "x__heading")
                {
                    if (btnNode.Attributes["caption"] != null)
                        btn.caption = btnNode.Attributes["caption"].Value;
                    else
                        btn.caption = string.Empty;

                    if (btnNode.Attributes["footer"] != null)
                        btn.footer = btnNode.Attributes["footer"].Value;
                    else
                        btn.footer = string.Empty;

                    if (btnNode.Attributes["visible"] != null)
                        btn.visible = btnNode.Attributes["visible"].Value;
                    else
                        btn.visible = "true";

                    if (btnNode.Attributes["task"] != null)
                    {
                        btn.task = btnNode.Attributes["task"].Value;
                        if (btn.task == "tasks")
                            continue;
                    }
                    else
                        btn.task = string.Empty;
                    if (btnNode.Attributes["action"] != null)
                        btn.action = btnNode.Attributes["action"].Value;
                    else
                        btn.action = string.Empty;
                    if (btnNode.Attributes["hint"] != null)
                        btn.hint = btnNode.Attributes["hint"].Value;
                    else
                        btn.hint = "";
                    if (btnNode.Attributes["key"] != null)
                        btn.ID = btnNode.Attributes["key"].Value;
                    else
                        btn.ID = "";
                    if (btnNode.Attributes["folder"] != null)
                        btn.isDrpDwn = btnNode.Attributes["folder"].Value.ToLower() == "true" ? true : false;
                    else
                        btn.isDrpDwn = false;
                    if (btnNode.Attributes["script"] != null)
                        btn.isScript = btnNode.Attributes["script"].Value.ToLower() == "true" ? true : false;
                    else
                        btn.isScript = false;
                    if (iconNode != null)
                    {
                        if (iconNode.SelectSingleNode("text") != null)
                        {
                            icon.text = iconNode.SelectSingleNode("text").InnerText;
                            icon.addclass = iconNode.SelectSingleNode("addClass").InnerText;
                            btn.icon = icon;
                            btn.image = "";
                        }
                        else
                        {
                            icon.text = "";
                            icon.addclass = "";
                            btn.icon = icon;
                            btn.image = iconNode.InnerText;
                        }
                    }
                    else
                    {
                        icon.text = "";
                        icon.addclass = "";
                        btn.icon = icon;
                        btn.image = "";
                    }
                    if (btn.isDrpDwn && btnNode.ChildNodes.Count > 0)
                    {
                        btn.childBtns = new ArrayList(XmlLoadDWBButtons(btnNode));
                    }

                    if (btnNode.Attributes["api"] != null && btnNode.Attributes["api"].Value != "")
                    {
                        btn.scriptApi = true;
                        XmlNode apiNode = btnNode.SelectSingleNode("api");

                        btnapi.url = apiNode.SelectSingleNode("url").InnerText;
                        btnapi.reqstr = apiNode.SelectSingleNode("reqstr").InnerText;
                        btnapi.restype = apiNode.SelectSingleNode("restype").InnerText;
                        btnapi.resformat = apiNode.SelectSingleNode("resformat").InnerText;
                        btnapi.category = apiNode.SelectSingleNode("category").InnerText;
                        btnapi.method = apiNode.SelectSingleNode("method").InnerText;
                        btn.btnapi = btnapi;
                    }
                    else
                    {
                        btn.scriptApi = false;
                        btnapi.url = "";
                        btnapi.reqstr = "";
                        btnapi.restype = "";
                        btnapi.resformat = "";
                        btnapi.category = "";
                        btnapi.method = "";
                        btn.btnapi = btnapi;
                    }

                    btnsArr.Add(btn);
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in XmlLoadDWBButtons - btnxml:" + btnNodes.OuterXml.ToString() + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "XmlLoadDWBButtons-" + transId, "new", "true");
        }
        return btnsArr;
    }

    // Sort the Task Button Elements in ascending order based on Element name
    public static void SortXMLElements(XmlNode node)
    {
        bool changed = true;
        int num0, num1;
        while (changed)
        {
            changed = false;
            for (int i = 1; i < node.ChildNodes.Count; i++)
            {
                if (Int32.TryParse(node.ChildNodes[i].Name.ToLower().Replace("pop", ""), out num1) && Int32.TryParse(node.ChildNodes[i - 1].Name.ToLower().Replace("pop", ""), out num0) && num1 < num0)
                {
                    node.InsertBefore(node.ChildNodes[i], node.ChildNodes[i - 1]); //Replace the nodes
                    changed = true;
                }
            }
        }
    }

    /// <summary>
    /// Update the formcontrol javascript array to the string variable.
    /// </summary>
    /// <param name="actionNodes"></param>
    private void XmlLoadJsFormcontrol(XmlNode actionNodes)
    {
        ActionStruct act = new ActionStruct();
        StringBuilder formcontrol = new StringBuilder();
        StringBuilder script_fc = new StringBuilder();
        int formcontrolNo = 0, srConNo = 0;
        XmlNode formcontrolTempNode1 = default(XmlNode);
        XmlNode formcontrolTempNode2 = default(XmlNode);
        foreach (XmlNode actionNode in actionNodes)
        {
            if (actionNode.Name == "formcontrol")
            {
                XmlNodeList formcontrolNode = actionNode.ChildNodes;
                for (int i = 0; i <= formcontrolNode.Count - 1; i++)
                {
                    formcontrolTempNode1 = formcontrolNode.Item(i);
                    XmlNodeList formcontrolChildNode = formcontrolTempNode1.ChildNodes;
                    for (int j = 0; j <= formcontrolChildNode.Count - 2; j++)
                    {
                        formcontrolTempNode2 = formcontrolChildNode.Item(j);
                        formcontrol.Append("Formcontrols[" + formcontrolNo + "] = " + "\"" + formcontrolTempNode2.InnerText + "\"; ");
                        formcontrolNo += 1;
                    }
                    //Last node needs to be "e". so the array will end one node before and e is hardcoded after 
                    formcontrol.Append("Formcontrols[" + formcontrolNo + "]= " + "\"e\"; ");
                    formcontrolNo += 1;
                }
            }
            else
            {
                if (actionNode.Attributes["formcontrol"] != null && actionNode.Attributes["formcontrol"].Value == "true")
                {
                    string actstrname = string.Empty, actapplyon = string.Empty, actapply = string.Empty, actconname = string.Empty;
                    if (actionNode.Attributes["sname"] != null)
                        actstrname = actionNode.Attributes["sname"].Value.ToString();
                    if (actionNode.Attributes["apply"] != null)
                        actapply = actionNode.Attributes["apply"].Value.ToString();
                    if (actionNode.Attributes["applyon"] != null)
                        actapplyon = actionNode.Attributes["applyon"].Value.ToString();
                    string isfscriptActive = "true";
                    if (actionNode.Attributes["active"] != null)
                    {
                        isfscriptActive = actionNode.Attributes["active"].Value;
                        if (isfscriptActive != string.Empty)
                            isfscriptActive = isfscriptActive.ToLower();
                    }

                    if (actionNode.HasChildNodes)
                    {
                        try
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(actionNode.OuterXml);
                            XmlNode resultNode = xmlDoc.SelectSingleNode("//r1/param1/exprset");
                            foreach (XmlNode childNode in resultNode.ChildNodes)
                            {
                                actconname += childNode.InnerText.Replace("\"", "\'") + "♥";
                            }
                            if (actconname != string.Empty)
                                actconname = actconname.TrimEnd('♥');
                        }
                        catch (Exception ex) { }
                    }
                    script_fc.Append("SFormControls[" + srConNo + "]=" + "\"" + actconname + "\"; ");
                    script_fc.Append("SFCApply[" + srConNo + "]=" + "\"" + actapply + "\"; ");
                    script_fc.Append("SFCFldNames[" + srConNo + "]=" + "\"" + actapplyon + "\"; ");
                    script_fc.Append("SFCActionName[" + srConNo + "]=" + "\"" + actionNode.Name.ToString() + "\"; ");
                    script_fc.Append("SFCIsActive[" + srConNo + "]=" + "\"" + isfscriptActive + "\"; ");
                    srConNo++;
                }
                else
                {
                    act.actname = actionNode.Name.ToString();
                    if (actionNode.Attributes["cap"] != null)
                        act.actcap = actionNode.Attributes["cap"].Value.ToString();
                    if (actionNode.Attributes["sname"] != null)
                        act.actstrname = actionNode.Attributes["sname"].Value.ToString();
                    if (actionNode.Attributes["apply"] != null)
                        act.actapply = actionNode.Attributes["apply"].Value.ToString();
                    if (actionNode.Attributes["applyon"] != null)
                        act.actapplyon = actionNode.Attributes["applyon"].Value.ToString();
                    if (actionNode.Attributes["desc"] != null)
                        act.actdesc = actionNode.Attributes["desc"].Value.ToString();
                    if (actionNode.Attributes["validate"] != null)
                        act.actvalidate = actionNode.Attributes["validate"].Value.ToString();
                    act.actweb = "no";
                    if (actionNode.Attributes["web"] != null && actionNode.Attributes["web"].Value != null)
                        act.actweb = actionNode.Attributes["web"].Value.ToString();
                    if (actionNode.Attributes["rem"] != null)
                        act.actRem = actionNode.Attributes["rem"].Value;
                    else
                        act.actRem = "n";

                    if (actionNode.Attributes["manrem"] != null)
                        act.manRem = actionNode.Attributes["manrem"].Value;
                    else
                        act.manRem = "n";

                    if (actionNode.Attributes["apply"] != null && actionNode.Attributes["apply"].Value != null && Convert.ToString(actionNode.Attributes["apply"].Value) == "On Form Load")
                        rapidFormLoadActionXML += actionNode.OuterXml;

                    if (actionNode.Attributes["active"] != null)
                    {
                        act.scriptActive = actionNode.Attributes["active"].Value;
                        if (act.scriptActive != string.Empty)
                            act.scriptActive = act.scriptActive.ToLower();
                    }
                    else
                        act.scriptActive = "true";

                    if (actionNode.Attributes["pushtoqueue"] != null)
                        act.pushtoQueue = actionNode.Attributes["pushtoqueue"].Value;
                    else
                        act.pushtoQueue = "f";

                    if (actionNode.Attributes["queuename"] != null)
                        act.queueName = actionNode.Attributes["queuename"].Value;
                    else
                        act.queueName = "";

                    if (actionNode.HasChildNodes)
                    {
                        try
                        {
                            XmlNode node = actionNode.FirstChild;
                            XmlNodeList lstRefresh = node.ChildNodes[0].ChildNodes;
                            foreach (XmlNode ndRefresh in lstRefresh)
                            {
                                if (ndRefresh.Name == "Refresh")
                                {
                                    act.actParRefresh = ndRefresh.InnerText;
                                    break;
                                }
                            }
                        }
                        catch (Exception ex) { }

                        try
                        {
                            act.actSaveTask = "";
                            act.actScriptTask = "";
                            act.actScriptCancel = "";
                            XmlNodeList chkSaveTask = actionNode.ChildNodes;
                            foreach (XmlNode ndSave in chkSaveTask)
                            {
                                if (ndSave.Attributes["task"].Value.ToLower() == "save")
                                {
                                    act.actSaveTask = "save";
                                    break;
                                }
                                else if (ndSave.Attributes["task"].Value.ToLower() == "scripts")
                                {
                                    int _icnt = 0;
                                    XmlNode scrpList = ndSave.ChildNodes[0].FirstChild;
                                    foreach (XmlNode ndScrp in scrpList.ChildNodes)
                                    {
                                        if (ndScrp.InnerText.ToLower() == "save()")
                                        {
                                            act.actSaveTask = "save";
                                            break;
                                        }
                                        else if (ndScrp.InnerText.ToLower().StartsWith("canceltransaction("))
                                        {
                                            act.actScriptCancel = "canceltransaction♠" + ndScrp.InnerText;
                                            break;
                                        }
                                        else if (ndScrp.InnerText.ToLower().StartsWith("loadform(") && _icnt == 0)
                                        {
                                            act.actScriptTask = "loadform♠" + ndScrp.InnerText;
                                            break;
                                        }
                                        else if (ndScrp.InnerText.ToLower().StartsWith("loadformanddata(") && _icnt == 0)
                                        {
                                            act.actScriptTask = "loadformanddata♠" + ndScrp.InnerText;
                                            break;
                                        }
                                        else if (ndScrp.InnerText.ToLower().StartsWith("loadiview(") && _icnt == 0)
                                        {
                                            act.actScriptTask = "loadiview♠" + ndScrp.InnerText;
                                            break;
                                        }
                                        else if (ndScrp.InnerText.ToLower().StartsWith("loadpage(") && _icnt == 0)
                                        {
                                            act.actScriptTask = "loadpage♠" + ndScrp.InnerText;
                                            break;
                                        }
                                        else if (ndScrp.InnerText.ToLower().StartsWith("openpage(") && _icnt == 0)
                                        {
                                            act.actScriptTask = "openpage♠" + ndScrp.InnerText;
                                            break;
                                        }
                                        else if (ndScrp.InnerText.ToLower().StartsWith("doformdesign("))
                                        {
                                            act.actScriptTask = "doformdesign♠" + ndScrp.InnerText;
                                            break;
                                        }
                                        if (ndScrp.InnerText != "")
                                            _icnt++;
                                    }
                                    break;
                                }
                            }
                        }
                        catch (Exception ex) { }
                    }

                    bool isActButton = true;
                    // check whether this action is used in any buttons, if so check for file upload and set it in the button
                    for (int i = 0; i < btns.Count; i++)
                    {
                        ButtonStruct btn = (ButtonStruct)btns[i];
                        if (btn.action != act.actname)
                            continue;

                        if (actionNode.Attributes["fileupload"] != null)
                        {
                            btn.fileupload = actionNode.Attributes["fileupload"].Value;
                            if (actionNode.Attributes["fmsg"] != null)
                                btn.fmessage = actionNode.Attributes["fmsg"].Value;
                            if (actionNode.Attributes["ftype"] != null)
                                btn.ftype = actionNode.Attributes["ftype"].Value;

                            btns.RemoveAt(i);
                            btns.Insert(i, btn);
                        }
                        isActButton = false;
                        actions.Add(act);
                        break;
                    }

                    // check whether this action is used in any buttons, if so check for file upload and set it in the button
                    for (int i = 0; i < taskBtns.Count; i++)
                    {
                        ButtonStruct btn = (ButtonStruct)taskBtns[i];
                        if (btn.action != act.actname)
                            continue;

                        if (actionNode.Attributes["fileupload"] != null)
                        {
                            btn.fileupload = actionNode.Attributes["fileupload"].Value;
                            if (actionNode.Attributes["fmsg"] != null)
                                btn.fmessage = actionNode.Attributes["fmsg"].Value;
                            if (actionNode.Attributes["ftype"] != null)
                                btn.ftype = actionNode.Attributes["ftype"].Value;

                            taskBtns.RemoveAt(i);
                            taskBtns.Insert(i, btn);
                        }
                        isActButton = false;
                        actions.Add(act);
                        break;
                    }
                    if (isActButton)
                        actions.Add(act);
                    formcontrol.Append("tstActionName[" + actNo + "]= " + "\"" + act.actname + "\";tstActionCaption[" + actNo + "]= " + "\"" + act.actcap + "\";actParRefresh[" + actNo + "]= " + "\"" + act.actParRefresh + "\";actSaveTask[" + actNo + "]= " + "\"" + act.actSaveTask + "\";actScriptTask[" + actNo + "]=" + "\"" + act.actScriptTask + "\";actScriptCancel[" + actNo + "]=\"" + act.actScriptCancel + "\";actScriptActive[" + actNo + "]=\"" + act.scriptActive + "\";actScriptPushtoQueue[" + actNo + "]=\"" + act.pushtoQueue + "\";actScriptQueueName[" + actNo + "]=\"" + act.queueName + "\";");
                    if (act.actSaveTask == "save")
                        SaveScriptList.Add(act.actname);
                    actNo++;
                }
            }
        }
        jsFormcontrolArray += formcontrol.ToString() + script_fc.ToString();
    }

    /// <summary>
    /// Update hyperlink list
    /// </summary>
    /// <param name="hlinkNodes"></param>
    private void XmlLoadHyperLink(XmlNode hlinkNodes)
    {
        StringBuilder hyperlnklist = new StringBuilder();
        int hyperlnkNo = 0;
        foreach (XmlNode hlinkNode in hlinkNodes)
        {
            HLinkStruct hl = new HLinkStruct();
            if (hlinkNode.Attributes["pop"] != null)
            {
                hl.hlpop = hlinkNode.Attributes["pop"].Value;
                hyperlnklist.Append("HLinkPop[" + hyperlnkNo + "] = " + "\"" + hlinkNode.Attributes["pop"].Value.ToString() + "\";");
            }
            else
            {
                hl.hlpop = "-";
                hyperlnklist.Append("HLinkPop[" + hyperlnkNo + "] = " + "\"-\";");
            }

            if (hlinkNode.Attributes["sname"] != null)
            {
                hl.hlname = hlinkNode.Attributes["sname"].Value;
                hyperlnklist.Append("HLinkName[" + hyperlnkNo + "] = " + "\"" + hlinkNode.Attributes["sname"].Value.ToString() + "\";");
            }
            else
            {
                hl.hlname = "-";
                hyperlnklist.Append("HLinkName[" + hyperlnkNo + "] = " + "\"-\";");
            }

            if (hlinkNode.Attributes["source"] != null)
            {
                hl.hlsource = hlinkNode.Attributes["source"].Value;
                hyperlnklist.Append("HLinkSource[" + hyperlnkNo + "] = " + "\"" + hlinkNode.Attributes["source"].Value.ToString() + "\";");
            }
            else
            {
                hl.hlsource = "-";
                hyperlnklist.Append("HLinkSource[" + hyperlnkNo + "] = " + "\"-\";");
            }

            if (hlinkNode.Attributes["load"] != null)
            {
                hl.hlload = hlinkNode.Attributes["load"].Value;
                hyperlnklist.Append("HLinkLoad[" + hyperlnkNo + "] = " + "\"" + hlinkNode.Attributes["load"].Value.ToString() + "\";");
            }
            else
            {
                hl.hlload = "-";
                hyperlnklist.Append("HLinkLoad[" + hyperlnkNo + "] = " + "\"-\";");
            }

            StringBuilder paramnm = new StringBuilder();
            StringBuilder paramval = new StringBuilder();
            foreach (XmlNode paramNode in hlinkNode)
            {
                if (paramnm.Length == 0)
                {
                    paramnm.Append(paramNode.Attributes["n"].Value);
                    paramval.Append(paramNode.Attributes["v"].Value);
                }
                else
                {
                    paramnm.Append("~" + paramNode.Attributes["n"].Value);
                    paramval.Append("~" + paramNode.Attributes["v"].Value);
                }
            }

            if (paramnm.Length != 0)
            {
                hl.hlparamname = paramnm.ToString();
                hl.hlparamval = paramval.ToString();
                hyperlnklist.Append("HLinkParamName[" + hyperlnkNo + "] = " + "\"" + hl.hlparamname + "\";");
                hyperlnklist.Append("HLinkParamValue[" + hyperlnkNo + "] = " + "\"" + hl.hlparamval + "\";");
            }
            else
            {
                hl.hlparamname = "-";
                hl.hlparamval = "-";
                hyperlnklist.Append("HLinkParamName[" + hyperlnkNo + "] = " + "\"-\";");
                hyperlnklist.Append("HLinkParamValue[" + hyperlnkNo + "] = " + "\"-\";");
            }
            hlnks.Add(hl);
            hyperlnkNo++;
        }
        if (hyperlnklist.ToString() != "")
            tstHyperLink = true;
        jsHyperlinkArray = hyperlnklist.ToString();
    }

    /// <summary>
    /// function to create customized labels, the customized labels users can provide, different font
    /// styles like, bold, size, color
    /// </summary>
    /// <param name="customLabelNodes"></param>
    private void XmlCreateCustomLabel(XmlNode customLabelNodes)
    {
        XmlNode nodeLabelExists = customLabelNodes.Attributes["t"];
        if ((nodeLabelExists == null))
        {
            customLabelHtml.Append("");
        }
        else if (nodeLabelExists.Value == "lbl")
        {
            string LabelName = null;
            LabelName = customLabelNodes.Attributes["c"].Value;

            int frameNo = 0;
            if (customLabelNodes.Attributes["p"] != null)
            {
                frameNo = Convert.ToInt32(customLabelNodes.Attributes["p"].Value.Substring(3));
            }


            if (string.IsNullOrEmpty(LabelName))
                LabelName = customLabelNodes.InnerText.ToString();
            if (LabelName.IndexOf("\n") != -1)
                LabelName = LabelName.Replace("\n", "</br>");
            string Labeltlhw = null;
            Labeltlhw = "";
            string Labeltop = null;
            string Labelleft = null;
            string Labelht = null;
            string Labelwidth = null;
            XmlNode nodeLabel = customLabelNodes.Attributes["tlhw"];

            if ((nodeLabel == null))
                Labeltlhw = "";
            else
                Labeltlhw = nodeLabel.Value;

            string lblstyle = string.Empty;
            nodeLabel = customLabelNodes.Attributes["font"];
            if ((nodeLabel == null))
                lblstyle = "";
            else
                lblstyle = nodeLabel.Value;

            string style = string.Empty;
            if (lblstyle == ",,," || string.IsNullOrEmpty(lblstyle))
                style = "font-family:'Lato', sans-serif; font-size:14px; font-weight:normal; font-style:normal; color:#000000;";
            else
            {
                string[] stylearr = lblstyle.Split(',');
                if (string.IsNullOrEmpty(stylearr[0]))
                    style = "font-family:'Lato', sans-serif;";
                else
                    style = "font-family:" + stylearr[0].ToString() + ";";

                if (string.IsNullOrEmpty(stylearr[1]))
                    style += " font-size:14px;";
                else
                    style += " font-size:" + stylearr[1].ToString() + "pt;";

                if (string.IsNullOrEmpty(stylearr[2]))
                    style += " font-weight:normal; font-style:normal;";
                else
                {
                    string fontstyle = stylearr[2].ToString();
                    string fstyle = "";
                    if ((fontstyle.Substring(0, 1) == "t"))
                        fstyle = " font-weight:bold;";
                    else
                        fstyle = " font-weight:bold;";

                    if ((fontstyle.Substring(1, 1) == "t"))
                        fstyle += "font-style:italic;";
                    else
                        fstyle += "font-style:bold;";

                    if ((fontstyle.Substring(2, 1) == "t"))
                        fstyle += "text-decoration:underline";
                    else
                        fstyle += "";

                    if ((fontstyle.Substring(3, 1) == "t"))
                        fstyle += " line-through;";
                    else
                        fstyle += ";";

                    style += fstyle;
                }

                if (stylearr[3] == "clBlack")
                    style += " color:#000000;";
                else
                {
                    if ((!string.IsNullOrEmpty(stylearr[3].ToString())))
                        style += " color:" + stylearr[3].ToString().Substring(2) + ";";
                    else
                        style += " color:#000000;";
                }
            }

            if (!(string.IsNullOrEmpty(Labeltlhw)))
            {
                string[] Labelsplitout = Labeltlhw.Split(',');
                if (EnableOldTheme == "true")
                {
                    Labeltop = Math.Floor((Convert.ToInt32(Labelsplitout[0].ToString()) * 1.2)).ToString() + "px";
                    Labelleft = Math.Floor((Convert.ToInt32(Labelsplitout[1].ToString()) * 1.01)).ToString() + "px";
                }
                else
                {
                    Labeltop = Math.Floor((Convert.ToInt32(Labelsplitout[0].ToString()) * 1.6)).ToString() + "px";
                    Labelleft = Math.Floor((Convert.ToInt32(Labelsplitout[1].ToString()) * 1.07)).ToString() + "px";
                }

                Labelht = Labelsplitout[2].ToString() + "px";
                Labelwidth = Labelsplitout[3].ToString() + "px";
                string divDirection = "left";
                if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
                {
                    divDirection = "right";
                }
                bool isHyper = false;
                int n = 0;
                for (n = 0; n <= hlnks.Count - 1; n++)
                {
                    if (customLabelNodes.Name.ToLower() == ((HLinkStruct)(hlnks[n])).hlsource.ToString().ToLower())
                    {
                        isHyper = true;
                        break;
                    }
                }

                FieldStruct dummy = new FieldStruct();
                dummy.visibility = false;
                dummy.fldlength = 15;
                dummy.fldFrameNo = frameNo;
                if (isHyper)
                    dummy.ctype = "hyperlink";
                else
                    dummy.ctype = "label";
                dummy.datatype = "special";
                dummy.name = customLabelNodes.Name;

                customLabelHtml.Append("<div class=\"  grid-stack-item \"" + xywhGridStack(dummy) + ">");
                customLabelHtml.Append("<div class=\"grid-stack-item-content\"></div>");
                customLabelHtml.Append("<div id=\"dv" + customLabelNodes.Name + "\" class=\"labelcol inputs grid-stack-item-content form-group row\">");
                customLabelHtml.Append("<label class=\"gridstackCalc\" for=\"" + LabelName.Replace(" ", "") + "\" ></label>");

                if (isHyper)
                    customLabelHtml.Append("<a href=\"javascript:void(0)\" class=\"cursor-pointer\" id=\"" + ((HLinkStruct)(hlnks[n])).hlsource.ToString() + "\" onclick='javascript:Tstructhyperlink(this);'>" + LabelName + "</a></div>");
                else
                    customLabelHtml.Append("<font Style=\"" + style + "\" class=\"\">" + LabelName + "</font></div>");

                customLabelHtml.Append("</div>");
            }

            if (HttpContext.Current.Session["AxpLblCreatedDesktop-" + transId] != null && HttpContext.Current.Session["AxpLblCreatedDesktop-" + transId].ToString() == "true")
            {
                //donothing
            }
            else
            {
                customLabels.Add(customLabelHtml.ToString());
                customLblDcNo.Add(dcs.Count.ToString());
            }
            customLabelHtml = new StringBuilder();
        }
    }

    /// <summary>
    /// function to construct the dc html with field html.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <param name="fieldhtml"></param>
    /// <returns> completed field html.</returns>
    private string GetFullDCHtml(int dcIndNo, int dcNo, string fieldhtml, string isTab)
    {
        colCount = 0;
        if (dcNo == 1) docHeight = 0;
        StringBuilder dcHtml = new StringBuilder();
        string dcFields1 = string.Empty;
        string ngBorder = string.Empty;
        StringBuilder customLabel = new StringBuilder();
        StringBuilder actionButton = new StringBuilder();
        StringBuilder defaultColsHtml = new StringBuilder();
        int gridwidth = 0;
        string divDirection = "left";
        string divCollapse = "right";
        if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
        {
            divDirection = "right";
            divCollapse = "left";
        }
        var designMode = false;

        if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
        {
            designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
        }
        for (int n = 0; n < customLblDcNo.Count; n++)
        {
            if (dcNo == Convert.ToInt32(customLblDcNo[n].ToString()))
                customLabel.Append(customLabels[n].ToString());
        }

        for (int m = 0; m < pageActionDcNo.Count; m++)
        {
            if (dcNo == Convert.ToInt32(pageActionDcNo[m].ToString()))
                actionButton.Append(pageActionButtons[m].ToString());
        }

        if (((DcStruct)(dcs[dcIndNo])).isgrid)
            docHeight += 500;
        else
            docHeight += ((DcStruct)(dcs[dcIndNo])).dcHeight;

        int VisibleFlds = ((DcStruct)(dcs[dcIndNo])).visibleFldCount;
        for (int k = 0; k < flds.Count; k++)
        {
            FieldStruct fld = ((FieldStruct)(flds[k]));
            if (fld.fldframeno < dcNo) continue;
            if (fld.fldframeno > dcNo) continue;// break;
            if (fld.fldframeno == dcNo)
            {
                if (fld.visibility == false)
                {
                    gridwidth += (fld.fldlength > 51 ? 400 : (fld.fldlength <= 15) ? 150 : (fld.fldlength * 8));
                }
            }
        }
        if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "true")
            gridwidth = gridwidth + 80;
        else
            gridwidth = gridwidth + 40;//40 width for sl no column and 40 for delete button

        string dcBoolean = string.Empty;
        if (!designMode && ((DcStruct)(dcs[dcIndNo])).DcDefaultstate != "" && ((DcStruct)(dcs[dcIndNo])).DcDefaultstate.ToLower() == "collapse")
            dcBoolean = " d-none ";

        if (((DcStruct)(dcs[dcIndNo])).isgrid)
        {
            string rowSrNoClass = string.Empty;
            if (IsFillGridCall)
            {
                rowSrNoClass = "gridtdclass";
            }
            else
            {
                rowSrNoClass = "gridtdclass d-none";
            }
            string dcRowone = "";
            if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
            {
                if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "false")
                    dcRowone = "<div id=sp" + dcNo + "R001" + "F" + dcNo + " class=\"editWrapTr grid-stack\">" + fieldhtml + "</div>";
                else
                    //delete grid row
                    dcRowone = "<div  id=sp" + dcNo + "R001" + "F" + dcNo + " class=\"editWrapTr grid-stack\">" + fieldhtml + "</div>";

            }
            else
            {
                if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "false")
                {
                    dcRowone = "<div  id=sp" + dcNo + "R001" + "F" + dcNo + " class=\"editWrapTr grid-stack\"><div class='gridElement " + rowSrNoClass + "'><div><label id=\"lblSlNo001F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">1</label></div></div>" + fieldhtml + "</div>";
                }
                else
                {
                    //delete grid row
                    dcRowone = "<div  id=sp" + dcNo + "R001" + "F" + dcNo + " class=\"editWrapTr grid-stack\"><div class='gridElement " + rowSrNoClass + "'><label id=\"lblSlNo001F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">1</label></div>" + fieldhtml + "</div>";
                }
            }

            string _tabDivCollapse = "<span title='Hide Dc' onclick='showHideTabbedDc(this)' id='tabCollapseButton" + dcNo + "' class='mt-2 cursor-pointer material-icons material-icons-style'>keyboard_arrow_up</span>";

            StringBuilder gridHdHtml = new StringBuilder();

            gridHdHtml.Append("<div class='tab-content mainIframe " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : "grid-stack") + " ' id='divDc" + dcNo + "'>");

            if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "true")
            {
                xywhGridStack(new FieldStruct { visibility = false, fldFrameNo = dcNo, name = ("uniqueThHead" + dcNo) }, true);
                if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
                {
                    defaultColsHtml.Append("<th id=\"uniqueThHead" + dcNo + "\" class='thhead fw-boldest'><div id=\"uniqueThHead" + dcNo + "-sizer\"></div><div class='thhead'>S.no</div></th>");
                    colCount++;
                }
                else
                {
                    defaultColsHtml.Append("<th id=\"uniqueThHead" + dcNo + "\" class='thhead fw-boldest'><div id=\"uniqueThHead" + dcNo + "-sizer\"></div><div class='thhead'>S.no</div></th>");
                    colCount++;
                }
            }
            else
            {
                if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
                    defaultColsHtml.Append("");
                else
                {
                    xywhGridStack(new FieldStruct { visibility = false, fldFrameNo = dcNo, name = ("uniqueThHead" + dcNo) }, true);
                    defaultColsHtml.Append("<th id=\"uniqueThHead" + dcNo + "\" class='thhead fw-boldest'><div id=\"uniqueThHead" + dcNo + "-sizer\"></div><div class='thhead'>S.no</div></th>");
                    colCount++;
                }
            }

            string dcBtns = GetGridButtonHtml(dcNo.ToString());
            //TODO: need to fix the calculation of colcount, the same code is there in GetTabFullDcHtml
            colCount = 100;
            gridHdHtml.Append("<div id=\"containerDc\" class=\"tab-pane fade show active grid-icons\"><div class=\"card card-xl-stretch mb-1 mb-xl-2 border border-gray-300 rounded-0\"><div class=\"card-body px-3 pt-1 pb-3\">" + dcBtns);
            gridHdHtml.Append("<div id=\"gridheaddiv\"><span id=\"disgridhead" + dcNo + "\"></span></div><div class=\"clear\"></div>");
            string hideClass = "d-none";

            gridHdHtml.Append("<div class='row " + hideClass + "' id=\"wrapperForEditFields" + dcNo + "\">" + dcRowone + "<div class=\"col-sm-12 editLayoutFooter d-none\"> <div class=\"GridHelpdiv\"><i class=\"fa fa-question-circle\" aria-hidden=\"true\"></i> <span>Use <b>Ctrl+Left arrow</b> &amp; <b>Ctrl+Right arrow</b> to navigate between record(s).</span></div><div><div class=\"btn-group previousNextEditButton\" role=\"group\"><button title=\"Previous Record\" class=\"btn prevRec coldbtn disabled lblprevious\" disabled=\"disabled\" onclick=\"editThePreviousNextRow('','" + dcNo + "','');\">&lt; Prev</button><button title=\"Next Record\" class=\"btn nextRec disabled coldbtn lblNext\" disabled=\"disabled\" onclick=\"editThePreviousNextRow('','" + dcNo + "','');\">Next &gt;</button></div><button title=\"Save &amp; New Record\" class=\"btn hotbtn lblSaveNew\" id=\"newRecordBtn" + dcNo + "\" onclick=\"addTheValuesToGrid(" + dcNo + ",this)\">Save &amp; New</button></div></div></div>");

            if (HttpContext.Current.Session["language"].ToString().ToLower() == "arabic")
            {
                gridHdHtml.Append("<div id=\"colScroll" + dcNo + "\" class=\"griddivColumn wrapperForGridData" + dcNo + " " + dcBoolean + "\">");
                gridHdHtml.Append("<table class='customSetupTableMN gridHeader g-1 table table-striped table-bordered mt-1 mb-0 border-gray-300 ' id=\"gridHd" + dcNo + "\"><thead><tr>");
            }
            else
            {
                gridHdHtml.Append("<div id=\"colScroll" + dcNo + "\" class=\"griddivColumn wrapperForGridData" + dcNo + " " + dcBoolean + "\">");
                gridHdHtml.Append("<table class='customSetupTableMN gridHeader g-1 table table-striped table-bordered mt-1 mb-0 border-gray-300 ' id=\"gridHd" + dcNo + "\" style=\"\"><thead><tr class=\"fw-bold fs-6 text-gray-800 border-bottom-2 border-gray-300\">");
            }
            xywhGridStack(new FieldStruct { visibility = false, fldFrameNo = dcNo, name = ("uniqueEditDeleteAct" + dcNo) }, true);
            gridHdHtml.Append("<th id=\"uniqueEditDeleteAct" + dcNo + "\" class=\"text-center fw-boldest\"><div class=\"form-check form-check-sm form-check-custom ms-2\"><input type=\"checkbox\" name=\"chkallgridrow" + dcNo + "\" class=\"form-check-input fgHdrChk gridHdrChk opacity-100\" id=\"chkallgridrow" + dcNo + "\" onclick=\"javascript:CheckAllGridRow(this, " + dcNo + ");\" disabled></div></th><div class='thhead'></div></th>" + defaultColsHtml.ToString());
            gridHdHtml.Append(gridHeadHtml.ToString() + "</tr></thead><tbody></tbody></table></div>");

            StringBuilder gridHtml = new StringBuilder();
            string gridButtons = string.Empty;

            if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
            {
                gridHdHtml.Append("<div id=\"contentScroll_" + dcNo + "\" class=\"divGridContent\">");
                gridHdHtml.Append("<table class='gridContent' id='gridDc" + dcNo + "' ><tbody>");
            }
            else
            {
                gridHdHtml.Append("<div id=\"contentScroll" + dcNo + "\" class=\"divGridContent\">");
                gridHdHtml.Append("<table class='gridContent' id='gridDc" + dcNo + "'><tbody>");
            }

            gridHdHtml.Append("</tbody></table>");
            gridHdHtml.Append("</div></div></div>");
            gridHdHtml.Append("</div></div>");

            string rowCountHtml = "<input type='hidden' id='hdnRCntDc" + dcNo + "'  value='1'/><input type='hidden' id='hdnSlNoCnt" + dcNo + "' value='1'/>";

            string fullGridHtml1 = string.Empty;

            if (isTab == "true")
            {
                if (((DcStruct)(dcs[dcIndNo])).isgrid)
                {
                    fullGridHtml1 = "<div class='col-12 pb-2 dvdcframe' id=\"DivFrame" + dcNo + "\">";
                    if (!string.IsNullOrEmpty(((DcStruct)(dcs[dcIndNo])).purpose))
                        fullGridHtml1 += gridHdHtml.ToString() + rowCountHtml + "</div><div class=\"clsPrps d-none\" id=axPurpose'" + ((DcStruct)(dcs[dcIndNo])).name + ">" + ((DcStruct)(dcs[dcIndNo])).purpose + "</div>";//murali
                    else
                        fullGridHtml1 += gridHdHtml.ToString() + rowCountHtml + "</div>";//murali
                }
                else
                {
                    fullGridHtml1 = "<div class='col-12 pb-2 dvdcframe' id=\"DivFrame" + dcNo + "\">";
                    if (!string.IsNullOrEmpty(((DcStruct)(dcs[dcIndNo])).purpose))
                        fullGridHtml1 += gridHdHtml.ToString() + rowCountHtml + "</div><div id=axPurpose' class=\"clsPrps d-none\"" + ((DcStruct)(dcs[dcIndNo])).name + ">" + ((DcStruct)(dcs[dcIndNo])).purpose + "</div>";//murali
                    else
                        fullGridHtml1 += gridHdHtml.ToString() + rowCountHtml + "</div>";//murali
                }
            }
            else if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
            {
                fullGridHtml1 = "<div class='row d-none '  id=\"DivFrame" + dcNo + "\">";
                if (!string.IsNullOrEmpty(((DcStruct)(dcs[dcIndNo])).purpose))
                    fullGridHtml1 += "<input type='hidden ' id='popDcTitle" + dcNo + "' class='popDcTitle' value='" + ((DcStruct)(dcs[dcIndNo])).caption + "' /><div class=\"clsPrps d-none\" id=axPurpose'" + ((DcStruct)(dcs[dcIndNo])).name + "'>" + ((DcStruct)(dcs[dcIndNo])).purpose + "</div>";
                else
                    fullGridHtml1 += "<input type='hidden ' id='popDcTitle" + dcNo + "' class='popDcTitle' value='" + ((DcStruct)(dcs[dcIndNo])).caption + "' />";
                fullGridHtml1 += gridHdHtml.ToString() + rowCountHtml + "</div>"; //murali

            }
            else
            {
                fullGridHtml1 = "<div class='col-12 pb-2 dvdcframe' id=\"DivFrame" + dcNo + "\">";
                fullGridHtml1 += "<ul class=\"cursor-pointer nav nav-tabs mb-n2 border border-gray-300 rounded-0 bg-light\">";
                string dcBooleanHtml = string.Empty;
                if (((DcStruct)(dcs[dcIndNo])).DcBlean != "" && ((DcStruct)(dcs[dcIndNo])).DcBlean.ToUpper() == "TRUE")
                {
                    dcBooleanHtml = "<div class=\"form-check form-switch form-check-custom px-1 align-self-end my-auto \"><input type=\"checkbox\" id=\"dcBlean" + dcNo + "\" data-dcname=\"" + ((DcStruct)(dcs[dcIndNo])).name + "\" title=\"\" style=\"\" name=\"dcBlean" + dcNo + "\" class=\"form-check-input opacity-100 gridHeaderSwitch \" " + (dcBoolean == string.Empty ? "checked" : "") + "></div>";
                    fullGridHtml1 += "<li id=head" + dcNo + " class=\"nav-item d-flex p-3 shadow-sm bg-white rounded-top\">" + _tabDivCollapse + dcBooleanHtml + "<a class=\"fs-4 nav-link fw-boldest text-gray-900 rounded-0 border-0 active\" data-bs-toggle=\"tab\" aria-current=\"page\" href=\"javascript:void(0);\" onclick='javascript:ShowDc(\"" + dcNo + "\");'>" + ((DcStruct)(dcs[dcIndNo])).caption + "</a></li></ul>";
                }
                else
                    fullGridHtml1 += "<li id=head" + dcNo + " class=\"nav-item d-flex p-3 shadow-sm bg-white rounded-top\">" + _tabDivCollapse + "<a class=\"fw-bold fs-6 nav-link fw-boldest text-gray-800 rounded-0 border-0 active\" data-bs-toggle=\"tab\" aria-current=\"page\" href=\"javascript:void(0);\" onclick='javascript:ShowDc(\"" + dcNo + "\");'>" + ((DcStruct)(dcs[dcIndNo])).caption + "</a></li></ul>";

                fullGridHtml1 += gridHdHtml.ToString() + rowCountHtml + "</div>";
            }
            dcHtml.Append(fullGridHtml1 + "<div class='clear'></div>");
        }
        else
        {
            // innermost div for all non grid dcs.
            // lastDcHeight is used if there is a calendar control in the last Dc, So this 
            // was affecting the dcheight to overcome this there is check where the the greater height isa set as dcheight
            string _tabDivCollapse = "";
            if (dcNo > 1)
                _tabDivCollapse = "<span title='Hide Dc' onclick='showHideTabbedDc(this)' id='tabCollapseButton" + dcNo + "' class='cursor-pointer material-icons material-icons-style'>keyboard_arrow_up</span>";

            if (dcNo == dcs.Count)
            {
                if (((DcStruct)(dcs[dcIndNo])).dcHeight > lastDcHeight)
                {
                    lastDcHeight = ((DcStruct)(dcs[dcIndNo])).dcHeight;
                }
                //commented because form-horizontal was affecting ui.. previously added by anand.. checking need to be done after removing.
                ngBorder = "<div id=\"divDc" + dcNo + "\"  class='row mainIframe " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : "grid-stack gx-0") + " " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : dcBoolean) + "' > ";
            }
            else
            {
                int tstHieght = ((DcStruct)(dcs[dcIndNo])).dcHeight;
                if (isTab == "true")
                {
                    if (((DcStruct)(dcs[dcIndNo])).isgrid)
                        ngBorder = "<div id=\"divDc" + dcNo + "\"  class='row mainIframe " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : "grid-stack gx-0") + "' > ";
                    else
                    {
                        string visibledc = GetVisibleDCs();
                        string[] arrVisibleDc = visibledc.Split(',');
                        int pos = Array.IndexOf(arrVisibleDc, "dc" + dcNo);
                        if (pos > -1)
                        {
                            if (EnableOldTheme == "true")
                                ngBorder = "<div id=\"divDc" + dcNo + "\"  class='row mainIframe " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : "grid-stack gx-0") + " " + dcBoolean + "' > ";
                            else
                                ngBorder = "<div id=\"divDc" + dcNo + "\" class='row mainIframe " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : "grid-stack gx-0") + " " + dcBoolean + "' > ";

                        }
                        else
                            ngBorder = "<div id=\"divDc" + dcNo + "\"  class='row mainIframe " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : "grid-stack gx-0") + " " + dcBoolean + "' > ";
                    }
                }
                else
                    ngBorder = "<div id=\"divDc" + dcNo + "\"  class='row mainIframe " + ("grid-stack gx-0") + " " + dcBoolean + "' > ";
            }
            string ngFldHtml = ngBorder + fieldhtml + customLabel.ToString() + actionButton.ToString() + "</div>";

            if (isTab == "true")
            {
                if (dcNo > 1)
                {
                    dcHtml.Append("<div id=\"DivFrame" + dcNo + "\" class=\"col-12 pb-2 dvdcframe\"><div class=\"card card-xl-stretch mb-1 mb-xl-2 border border-gray-300 rounded-0 overflow-auto\"><div class=\"card-body px-3 pt-1 pb-3\">");
                    dcHtml.Append(ngFldHtml + "</div></div></div><div class='clear'></div>");


                }
                else
                {
                    //TODO: the primary dc can also be a tabbed dc
                    dcHtml.Append("<div id=\"DivFrame" + dcNo + "\" class=\"col-12 pb-2 dvdcframe\"><div class=\"card card-xl-stretch mb-1 mb-xl-2 border border-gray-300 rounded-0\"><div class=\"card-body px-3 pt-1 pb-3\">");
                    dcHtml.Append(ngFldHtml + "</div></div></div><div class='clear'></div>");

                }
            }
            else
            {
                dcHtml.Append("<div id=\"DivFrame" + dcNo + "\" class=\"col-12 pb-2 \"><div class=\"card card-xl-stretch mb-1 mb-xl-2 border border-gray-300 rounded-0\"><div class=\"card-body px-3 pt-1 pb-3\">");
                if (dcNo > 1 || tstLayout == Constants.TILE)
                {
                    string dcBooleanHtml = string.Empty;
                    if (((DcStruct)(dcs[dcIndNo])).DcBlean != "" && ((DcStruct)(dcs[dcIndNo])).DcBlean.ToUpper() == "TRUE")
                        dcBooleanHtml = "<div class=\"form-check form-switch form-check-custom px-1 align-self-end my-auto \"><input type=\"checkbox\" id=\"dcBlean" + dcNo + "\"  data-dcname=\"" + ((DcStruct)(dcs[dcIndNo])).name + "\" title=\"\" style=\"\" name=\"dcBlean" + dcNo + "\" class=\"form-check-input opacity-100 gridHeaderSwitch \" " + (dcBoolean == string.Empty ? "checked" : "") + "></div>";

                    //if (!string.IsNullOrEmpty(((DcStruct)(dcs[dcNo - 1])).purpose))
                    //    dcHtml.Append("<div id=head" + dcNo + " class='dcTitle " + divDirection + " d-flex'><a href=\"javascript:void(0)\" onclick='javascript:ShowDc(\"" + dcNo + "\");'><span data-type=\"hide\" title=\"Hide Dc\" class=\"material-icons\" id=dcButspan" + dcNo + " >keyboard_arrow_up</span></a>  <span class=\"frameCap fw-boldest fs-4 text-gray-900 " + divDirection + " me-3\" id=\"dcCaption" + dcNo + "\" >" + ((DcStruct)(dcs[dcNo - 1])).caption + "</span>" + dcBooleanHtml + "</div><div class=\"clsPrps\">" + ((DcStruct)(dcs[dcNo - 1])).purpose + "</div><hr class='hrline my-2' />");//muralli
                    //else
                    dcHtml.Append("<div id=head" + dcNo + " class='dcTitle " + divDirection + " d-flex'>" + _tabDivCollapse + dcBooleanHtml + "<a href=\"javascript:void(0)\" onclick='javascript:ShowDc(\"" + dcNo + "\");'><span data-type=\"hide\" title=\"Hide Dc\" class=\"material-icons\" id=dcButspan" + dcNo + " ></span></a>  <span class=\"frameCap fw-boldest fs-4 text-gray-900 " + divDirection + " me-3\" id=\"dcCaption" + dcNo + "\" >" + ((DcStruct)(dcs[dcIndNo])).caption + "</span></div><hr class='hrline my-2' />");
                }
                else if (dcNo == 1 && designMode)
                {
                    dcHtml.Append("<div id=head" + dcNo + " class='dcTitle " + divDirection + "'><span class=\"frameCap fw-boldest fs-4 text-gray-900 " + divDirection + "\" id=\"dcCaption" + dcNo + "\" >" + ((DcStruct)(dcs[dcIndNo])).caption + "</span></div><div class=\"clsPrps d-none\">" + ((DcStruct)(dcs[dcIndNo])).purpose + "</div><hr class='hrline my-2' />");
                }

                dcHtml.Append(ngFldHtml + "</div></div></div><div class='clear'></div>");
            }

        }
        return dcHtml.ToString();
    }

    private void XmlLoadRuleDef(XmlNode ruleDefNodes)
    {
        StringBuilder ruledeflist = new StringBuilder();
        int ruledefNo = 0;
        string validation = "false", filter = "false", formcontrol = "false", computescript = "false", allowduplicate = "false", allowempty = "false", isapplicable = "false";
        XmlNodeList axruleDefChildNodes = default(XmlNodeList);
        axruleDefChildNodes = ruleDefNodes.ChildNodes;
        foreach (XmlNode rulesNode in axruleDefChildNodes)
        {
            foreach (XmlNode rulesChildNode in rulesNode)
            {
                if (rulesChildNode.Name == "comptype")
                {
                    ruledeflist.Append("AxRDCompType[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                }
                else if (rulesChildNode.Name == "componentname")
                {
                    ruledeflist.Append("AxRDCompName[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                }
                else if (rulesChildNode.Name == "validation")
                {
                    if (rulesChildNode.InnerText != "")
                        validation = "true";
                    ruledeflist.Append("AxRDValidation[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                }
                else if (rulesChildNode.Name == "filter")
                {
                    if (rulesChildNode.InnerText != "")
                        filter = "true";
                    ruledeflist.Append("AxRDFilter[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                }
                else if (rulesChildNode.Name == "formcontrol")
                {
                    if (rulesChildNode.InnerText != "")
                        formcontrol = "true";
                    ruledeflist.Append("AxRDFormControl[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                }
                else if (rulesChildNode.Name == "computescript")
                {
                    if (rulesChildNode.InnerText != "")
                        computescript = "true";
                    ruledeflist.Append("AxRDComputeScript[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                }
                else if (rulesChildNode.Name == "allowduplicate")
                {
                    if (rulesChildNode.InnerText != "")
                        allowduplicate = "true";
                    ruledeflist.Append("AxRDAllowDuplicate[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                }
                else if (rulesChildNode.Name == "allowempty")
                {
                    if (rulesChildNode.InnerText != "")
                        allowempty = "true";
                    ruledeflist.Append("AxRDAllowEmpty[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                }
                else if (rulesChildNode.Name == "isapplicable")
                {
                    if (rulesChildNode.InnerText != "")
                        isapplicable = "true";
                    ruledeflist.Append("AxRDIsApplicable[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                }
            }
            ruledefNo++;
        }
        strRulesDefEngin = validation + "~" + filter + "~" + formcontrol + "~" + computescript + "~" + allowduplicate + "~" + allowempty + "~" + isapplicable;
        jsRuleDefArray = ruledeflist.ToString();
    }
    #endregion

    #region Public Methods

    public string GetFillGridCaption(int dcNo, string isTab)
    {
        StringBuilder fgfieldArray = new StringBuilder();
        string fgButCap = "";
        if (fgs.Count > 0)
        {
            int dcIndNo = dcsPositionIndex.IndexOf(dcNo.ToString());
            int cnt = 0;
            for (cnt = 0; cnt <= fgs.Count - 1; cnt++)
            {
                if (((DcStruct)(dcs[dcIndNo])).name == ((FGStruct)(fgs[cnt])).fgtargetdc)
                {
                    fgButCap = ((FGStruct)(fgs[cnt])).fgcaption;
                    break;
                }
            }
        }
        return fgButCap;
    }

    /// <summary>
    /// Method to initialize the fields. It takes XML returned by AWS, parses and 
    /// fills the arrays and fields. U
    /// </summary>
    /// <param name="xml"></param>
    public void Create(string xml)
    {

        xmlDoc.LoadXml(xml);
        FillTstructProperties(xmlDoc);
        XmlNodeList tstBaseDataNodes = default(XmlNodeList);
        tstBaseDataNodes = xmlDoc.DocumentElement.SelectNodes("//root");

        XmlNodeList hyperLinkNode = xmlDoc.GetElementsByTagName("hyperlinks");
        if (hyperLinkNode[0] != null)
            XmlLoadHyperLink(hyperLinkNode[0]);
        //Calling Runmode/design mode Api's 
        //getDesignData();
        if (HttpContext.Current.Session["IsCustomHtml"] == null || HttpContext.Current.Session["IsCustomHtml"].ToString() == string.Empty)
            getDesignDataPerf();
        if (HttpContext.Current.Session["IsCustomHtml"] != null && HttpContext.Current.Session["IsCustomHtml"].ToString() == "true")
            IsObjCustomHtml = true;

        XmlDocument formbtnnode = new System.Xml.XmlDocument();
        XmlNode toolbarcomps = xmlDoc.SelectSingleNode("//root/comps");
        if (HttpContext.Current.Session != null && HttpContext.Current.Session["AxpTstButtonStyleDef"] != null)
        {
            AxpTstButtonStyleDef = HttpContext.Current.Session["AxpTstButtonStyleDef"].ToString();
            HttpContext.Current.Session.Remove("AxpTstButtonStyleDef");
        }

        string pdcno = string.Empty;
        string strformbt = string.Empty;
        for (int i = toolbarcomps.ChildNodes.Count - 1; i < toolbarcomps.ChildNodes.Count; i--)
        {
            if (toolbarcomps.ChildNodes[i] == null)
                break;
            string newpdcno = string.Empty;
            string innerXML = string.Empty;
            if (toolbarcomps.ChildNodes[i].Attributes["position"] != null && toolbarcomps.ChildNodes[i].Attributes["position"].Value != "" && toolbarcomps.ChildNodes[i].Attributes["position"].Value.StartsWith("dc"))
            {
                newpdcno = toolbarcomps.ChildNodes[i].Attributes["position"].Value;
                innerXML = toolbarcomps.ChildNodes[i].OuterXml;
                newpdcno = newpdcno.Split('~')[0];
                newpdcno = newpdcno.Substring(2);
                toolbarcomps.RemoveChild(toolbarcomps.ChildNodes[i]);
            }
            if (innerXML != string.Empty)
            {
                if (pdcno == string.Empty)
                {
                    strformbt += "<dc" + newpdcno + ">";
                    strformbt += innerXML;
                    pdcno = newpdcno;
                }
                else if (pdcno == newpdcno)
                {
                    strformbt += innerXML;
                }
                else if (pdcno != newpdcno)
                {
                    strformbt += "</dc" + pdcno + ">";
                    strformbt += "<dc" + newpdcno + ">";
                    strformbt += innerXML;
                    pdcno = newpdcno;
                }
            }
        }
        if (strformbt != string.Empty)
        {
            strformbt += "</dc" + pdcno + ">";
            formbtnnode.LoadXml("<root>" + strformbt + "</root>");
        }


        foreach (XmlNode tstBaseDataNode in tstBaseDataNodes)
        {

            XmlNodeList tstChildDataNodes = default(XmlNodeList);
            tstChildDataNodes = tstBaseDataNode.ChildNodes;

            string category = string.Empty;

            foreach (XmlNode tstChildDataNode in tstChildDataNodes)
            {
                if (tstChildDataNode.Attributes["cat"] != null)
                    category = tstChildDataNode.Attributes["cat"].Value.ToString();
                else
                    category = string.Empty;
                if (tstChildDataNode.Attributes["plist"] != null)
                    dcPlistaddrow = tstChildDataNode.Attributes["plist"].Value.ToString();
                else
                    dcPlistaddrow = string.Empty;

                if (category == "dc")
                {
                    if (tstChildDataNode.Attributes["booleandc"] != null)
                        dcExpandCollapse = tstChildDataNode.Attributes["booleandc"].Value.ToString();
                    else
                        dcExpandCollapse = string.Empty;

                    if (tstChildDataNode.Attributes["defaultstate"] != null)
                        dcDefaultstate = tstChildDataNode.Attributes["defaultstate"].Value.ToString();
                    else
                        dcDefaultstate = string.Empty;

                    dcFldCount = 0;
                    dcFldVisibleCount = 0;
                    XmlLoadDc(tstChildDataNode);

                    if (formbtnnode.ChildNodes.Count > 0)
                    {
                        XmlPageLoadDWBButton(formbtnnode, DCNo);
                    }
                }
                else if (category == "field")
                {
                    string fieldType = string.Empty;
                    if (tstChildDataNode.Attributes["fieldtype"] != null)
                        fieldType = tstChildDataNode.Attributes["fieldtype"].Value.ToString();
                    else
                        fieldType = string.Empty;
                    //if (tstChildDataNode.Attributes["runtimecriteria"] != null && tstChildDataNode.Attributes["runtimecriteria"].Value == "T" && fldRuntimeCriteria == "F")
                    //  fldRuntimeCriteria = tstChildDataNode.Attributes["runtimecriteria"].Value.ToString();

                    XmlLoadField(tstChildDataNode, fieldType);
                }
                else if (category == "fillgrid")
                    XmlLoadFillgrid(tstChildDataNode);
                else if (category == "btn")
                    XmlPageLoadButton(tstChildDataNode, "btn");
                else if (category == "comps")
                {
                    if (tstChildDataNode.Attributes["dwbtb"] != null && tstChildDataNode.Attributes["dwbtb"].Value.ToString().ToLower() == "true")
                    {
                        dwbToolbar = true;
                        //btns = new ArrayList(XmlLoadDWBButtons(tstChildDataNode));
                        btns = new ArrayList(XmlLoadDWBButtons(toolbarcomps));
                    }
                    else
                        XmlLoadButton(tstChildDataNode, "btn");
                }

                else if (category == "tasks")
                    XmlLoadButton(tstChildDataNode, "task");
                else if (category == "hyperlinks")
                    continue;
                else if (category == "actions")
                    XmlLoadJsFormcontrol(tstChildDataNode);
                else if (category == "iframes")
                    XmlLoadTabDcs(tstChildDataNode);
                else if (category == "gformat")
                {
                    continue;
                    //XmlLoadFormatDcs(tstChildDataNode);
                    //Commented Bcoz this feature will be taken up later
                    //Right now it will skip the GFormat node from the tstruct
                }
                else if (category != "")
                    XmlCreateCustomLabel(tstChildDataNode);
                else if (category == string.Empty && tstChildDataNode.Name == "axrulesdef")
                    XmlLoadRuleDef(tstChildDataNode);
            }
        }

        //XmlNodeList pdfListNode = xmlDoc.GetElementsByTagName(transId + "_pdflist");
        //if (pdfListNode[0] != null)
        //    pdfList = pdfListNode[0].OuterXml;
        if (HttpContext.Current.Session[transId + "_pdflist"] != null)
        {
            pdfList = HttpContext.Current.Session[transId + "_pdflist"].ToString();
            HttpContext.Current.Session[transId + "_pdflist"] = null;
        }

        if (dcRange != "")
        {
            dcRange = dcRange + "," + (fldGlobal).ToString();
            jsFieldArray.Append("FldDcRange[" + DcCnt + "]=\"" + dcNo.ToString() + "~" + dcRange + "\";");
            fldDcRange.Add(dcNo.ToString() + "~" + dcRange);
            dcRange = "";
        }

        if (fldSetCarry.Count > 0)
        {
            jsFieldArray.Append("FSetCarry=['" + string.Join("','", fldSetCarry.ToArray()) + "'];");
        }
        if (AxpFileUploadFields.Count > 0)
            jsFieldArray.Append("AxpFileUploadFields=['" + string.Join("','", AxpFileUploadFields.ToArray()) + "'];");

        jsFieldArray.Append("AxpListofRagVarflds[0]=\"" + listofRagVarfld + "\";");


        FillNGSqlDepParents();

        IsWsPerfDeleteGridRow();

        //FDR fastData = (FDR)HttpContext.Current.Session["FDR"];
        //fastDataFlds = fastData.GetRedisServerFields(this);
        //fastData.GetFastDataRefEvents(this);
    }

    /// <summary>
    /// GridDC fields dependences are having Accept SQL or Select with AutoSelect true then only needs to call GridRowDelete service. 
    /// </summary>
    private void IsWsPerfDeleteGridRow()
    {
        if (wsPerfFields != string.Empty || wsPerfEvalExpClient != string.Empty)
        {
            string[] wsPerfFieldArr = wsPerfFields.Split(',');
            string[] wsPerfFieldExpArr = wsPerfEvalExpClient.Split(',');
            for (int i = 0; i < dcs.Count; i++)
            {
                DcStruct dc = (DcStruct)dcs[i];
                if (dc.isgrid)
                {
                    string[] dcRange = GetDcFieldRange(dc.frameno.ToString()).Split(',');
                    int startIdx = Convert.ToInt32(dcRange[0]);
                    int endIdx = Convert.ToInt32(dcRange[1]);
                    for (int k = startIdx; k <= endIdx; k++)
                    {
                        FieldStruct fld = (FieldStruct)flds[k];
                        if (fld.fieldDependents == string.Empty)
                            continue;
                        string[] fldDeparray = fld.fieldDependents.Split(',');
                        var isfldDepBound = fldDeparray.Where(fd => wsPerfFieldArr.Any(vd => fd == "s" + vd || fd == "e" + vd || fd == "f" + vd)).ToList();
                        var isfldDepExpBound = fldDeparray.Where(fd => wsPerfFieldExpArr.Any(vd => fd == "s" + vd || fd == "e" + vd || fd == "f" + vd)).ToList();
                        if (isfldDepBound.Count > 0 || isfldDepExpBound.Count > 0)
                        {
                            wsPerfGRDDcs.Add(dc.frameno.ToString());
                            break;
                        }
                        else
                        {
                            for (int j = 0; j < fldDeparray.Length; j++)
                            {
                                if (GetFieldDc(fldDeparray[j]) != dc.frameno)
                                {
                                    wsPerfGRDDcs.Add(dc.frameno.ToString());
                                    break;
                                }
                            }
                            if (wsPerfGRDDcs.Count > 0 && wsPerfGRDDcs.IndexOf(dc.frameno.ToString()) != -1)
                                break;
                        }
                    }
                }
            }
        }
    }

    private void SetRapidExpFlds()
    {
        if (rapidFLExpFlds.Count > 0)
        {
            rapidFormLoadExpFlds = string.Join(",", rapidFLExpFlds.Select(x => x.ToString()).ToArray());
            if (rapidFormLoadExpFlds != string.Empty)
                rapidFormLoadJSON = "{\"evalexpr\":\"true\", \"flds\":\"" + rapidFormLoadExpFlds + "\"}";
        }
    }


    private FieldStruct SetFieldGridSqlDeps(FieldStruct fld, DcStruct dc, int idx)
    {
        ArrayList fldDepGridDcs = new ArrayList();
        StringBuilder strSqlDeps = new StringBuilder();

        string strFldDeps = string.Empty;
        strFldDeps = fld.fieldSqlDeps;
        if (strFldDeps == "") strFldDeps = fld.fieldExpDeps;
        else if (fld.fieldExpDeps != string.Empty) strFldDeps += "," + fld.fieldExpDeps;
        if (!string.IsNullOrEmpty(strFldDeps))
        {
            string[] strDeps = strFldDeps.Split(',');

            for (int i = 0; i < strDeps.Length; i++)
            {
                string fChar = strDeps[i].ToString().Substring(0, 1);
                string fName = strDeps[i].ToString().Substring(1);
                int dIdx = GetFieldIndex(fName);

                FieldStruct depFld = (FieldStruct)flds[dIdx];
                int dcIndNo = dcsPositionIndex.IndexOf(depFld.fldframeno.ToString());
                DcStruct depDc = (DcStruct)dcs[dcIndNo];
                if (!depDc.ispopgrid && depDc.isgrid && fld.fldframeno != depFld.fldframeno)
                {
                    string depStr = depFld.name;
                    //Stores the dep grid dcs for a given field
                    if (fldDepGridDcs.IndexOf(depFld.fldframeno) == -1)
                    {
                        fldDepGridDcs.Add(depFld.fldframeno);
                        depStr += "," + "axp_recid" + depFld.fldframeno;
                    }

                    //if (depFld.fieldParents != string.Empty && fChar != "e") here checking is dependece fields are sql or not if it is sql dependent adding else not adding 
                    if (depFld.fieldParents != string.Empty)
                    {
                        depStr += "," + depFld.fieldParents;
                    }

                    if (strSqlDeps.ToString() == string.Empty)
                        strSqlDeps.Append(depStr);
                    else
                        strSqlDeps.Append("," + depStr);
                }
            }
            for (int j = 0; j < fldDepGridDcs.Count; j++)
            {
                if (fld.fieldDepGridDcs == string.Empty)
                    fld.fieldDepGridDcs = fldDepGridDcs[j].ToString();
                else
                    fld.fieldDepGridDcs += "," + fldDepGridDcs[j].ToString();
            }
        }

        fld.fieldDepGridDcs = cust.UpdateSqlDepGridDcs(transId, fld.name, fld.fieldDepGridDcs);
        //Hook to add or remove a given field's sql grid dependents            
        fld.fieldSqlGridDeps = cust.UpdateSqlGridDependents(transId, fld.name, strSqlDeps.ToString());
        //flds[idx] = fld;
        return fld;
    }

    /// <summary>
    /// This function will be called on a tabbed grid dc only
    /// all the tab grid dc parent fields will be stored in the array dc.tabParentFields.
    /// tabParentFields will have parent fields outside the current dc and parent dc is grid.
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="fld"></param>
    private void FillTabGridParentFlds(DcStruct dc, FieldStruct fld)
    {
        if (string.IsNullOrEmpty(fld.fieldParents)) return;
        string[] parFlds = fld.fieldParents.Split(',');

        for (int i = 0; i < parFlds.Length; i++)
        {
            int idx = GetFieldIndex(parFlds[i].ToString());
            if (idx != -1)
            {
                FieldStruct pFld = (FieldStruct)flds[idx];
                int dcIndNo = dcsPositionIndex.IndexOf(pFld.fldframeno.ToString());
                DcStruct depDc = (DcStruct)dcs[dcIndNo];
                if (depDc.isgrid && depDc.frameno != dc.frameno && dc.tabParentFields.IndexOf(pFld.name) == -1)
                {
                    dc.tabParentFields.Add(pFld.name);
                }
            }
        }
    }

    /// <summary>
    /// This function will be called for every dc which has fillgrid
    /// Updates the tabParentFields with the fillsql params which are grid fields and outside the current dc.
    /// </summary>
    /// <param name="frameNo"></param>
    /// <param name="dc"></param>
    private void FillTabGridParentFlds(int frameNo, DcStruct dc)
    {
        int idx = GetFillGridIndex("dc" + frameNo.ToString());
        if (idx != -1)
        {
            FGStruct fg = (FGStruct)fgs[idx];
            if (fg.fgSqlParams != string.Empty)
            {
                string[] fgParFields = fg.fgSqlParams.Split(',');
                for (int i = 0; i < fgParFields.Length; i++)
                {
                    int index = GetFieldIndex(fgParFields[i].ToString());
                    if (index != -1)
                    {
                        FieldStruct pFld = (FieldStruct)flds[index];
                        int dcIndNo = dcsPositionIndex.IndexOf(pFld.fldframeno.ToString());
                        DcStruct depDc = (DcStruct)dcs[dcIndNo];
                        if (depDc.isgrid && depDc.frameno != dc.frameno && dc.tabParentFields.IndexOf(pFld.name) == -1)
                        {
                            dc.tabParentFields.Add(pFld.name);
                        }
                    }
                }
            }
        }
    }

    private void GetGridExpDependents(DcStruct dc, FieldStruct fld)
    {
        if (fld.fieldDependents != string.Empty)
        {
            string[] depFlds = fld.fieldDependents.Split(',');
            for (int j = 0; j < depFlds.Length; j++)
            {
                if (depFlds[j].ToString() != string.Empty)
                {
                    string depName = depFlds[j].ToString().Substring(1);
                    string fChar = depFlds[j].ToString().Substring(0, 1);
                    if (fChar == "e")
                    {
                        int dIdx = GetFieldIndex(depName);
                        if (dIdx != -1)
                        {
                            FieldStruct depFld = (FieldStruct)flds[dIdx];
                            int dcIndNo = dcsPositionIndex.IndexOf(depFld.fldframeno.ToString());
                            DcStruct depDc = (DcStruct)dcs[dcIndNo];
                            if (dc.frameno < depDc.frameno)
                            {
                                if (dc.dcExpressionDeps.ToString() == "")
                                    dc.dcExpressionDeps.Append(depName);
                                else
                                    dc.dcExpressionDeps.Append("," + depName);
                            }

                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updates the dependents and their parents in the fields
    /// </summary>
    private void FillNGSqlDepParents()
    {
        ArrayList gridDcs = GetGridDcs();
        for (int dcIdx = 0; dcIdx < dcs.Count; dcIdx++)
        {
            DcStruct dc = (DcStruct)dcs[dcIdx];
            string[] dcRange = GetDcFieldRange(dc.frameno.ToString()).Split(',');
            int startIdx = Convert.ToInt32(dcRange[0]);
            int endIdx = Convert.ToInt32(dcRange[1]);
            bool isTabbedDc = Convert.ToBoolean(IsDcTab(dc.frameno));
            int fIdx = 0;
            StringBuilder strDcMRFields = new StringBuilder();
            for (int i = startIdx; i <= endIdx; i++)
            {
                TStructDef.FieldStruct fld = (TStructDef.FieldStruct)flds[i];
                fIdx = i;


                if (fld.moe.ToUpper() == moe.ACCEPT.ToString() && fld.selectmode == "From Master")
                {
                    if (strDcMRFields.ToString() == string.Empty)
                        strDcMRFields.Append(fld.name);
                    else
                        strDcMRFields.Append("," + fld.name);
                }

                //if (isTabbedDc)//AXP-C-0000185, SQL parameter field is in griddc and its depending on other grid dc field so required tabParentFields fields.
                FillTabGridParentFlds(dc, fld);

                //THis function sets the grid sql dependents outside the current dc
                fld = SetFieldGridSqlDeps(fld, dc, i);

                if (dc.isgrid)
                {
                    GetGridExpDependents(dc, fld);
                    StringBuilder depParents = new StringBuilder();
                    if (fld.fieldSqlDeps != string.Empty)
                    {
                        string[] sqlDeps = fld.fieldSqlDeps.Split(',');
                        for (int j = 0; j < sqlDeps.Length; j++)
                        {
                            string depName = sqlDeps[j].ToString().Substring(1);
                            int dIdx = GetFieldIndex(depName);

                            if (dIdx != -1)
                            {
                                FieldStruct depFld = (FieldStruct)flds[dIdx];
                                int dcIndNo = dcsPositionIndex.IndexOf(depFld.fldframeno.ToString());
                                DcStruct depDc = (DcStruct)dcs[dcIndNo];
                                if (!depDc.isgrid)
                                {
                                    //get the parents for non grid sql dependents and check for the fields in same dc
                                    string[] parFlds = depFld.fieldParents.Split(',');
                                    for (int k = 0; k < parFlds.Length; k++)
                                    {
                                        int parIdx = GetFieldIndex(parFlds[k].ToString());
                                        if (parIdx != -1)
                                        {
                                            FieldStruct parFld = (FieldStruct)flds[parIdx];
                                            int _pdcIndNo = dcsPositionIndex.IndexOf(parFld.fldframeno.ToString());
                                            DcStruct parDc = (DcStruct)dcs[_pdcIndNo];
                                            if (parDc.frameno == dc.frameno)
                                            {
                                                if (depParents.ToString() == string.Empty)
                                                    depParents.Append(parFld.name);
                                                else
                                                    depParents.Append("," + parFld.name);
                                            }
                                        }
                                        else
                                        {
                                            //TODO: this is error condition when the parent field is not available in the tstruct.
                                        }
                                    }
                                }
                            }
                        }
                    }
                    fld.fieldSqlDepParents = depParents.ToString();
                }
                else
                {
                    //Fill grid dependency for a non grid field
                    ArrayList arrDepDcs = new ArrayList();
                    if (fld.fieldDependents != string.Empty)
                    {
                        string[] depFlds = fld.fieldDependents.Split(',');
                        for (int j = 0; j < depFlds.Length; j++)
                        {
                            if (depFlds[j].ToString() != string.Empty)
                            {
                                string depName = depFlds[j].ToString().Substring(1);
                                string prefix = depFlds[j].ToString().Substring(0, 1);
                                if (prefix == "g" && depName.Substring(0, 2) == "dc")
                                {
                                    int depDcNo = Convert.ToInt32(depName.Substring(2));
                                    if (gridDcs.IndexOf(depDcNo) != -1 && arrDepDcs.IndexOf(depDcNo) == -1)
                                        arrDepDcs.Add(depDcNo);
                                }
                                else
                                {
                                    int idx = GetFieldIndex(depName);
                                    if (idx != -1)
                                    {
                                        FieldStruct depFld = (FieldStruct)flds[idx];
                                        if (gridDcs.IndexOf(depFld.fldframeno) != -1 && arrDepDcs.IndexOf(depFld.fldframeno) == -1)
                                            arrDepDcs.Add(depFld.fldframeno);
                                    }
                                }
                            }
                        }

                        StringBuilder strDepDcs = new StringBuilder();
                        for (int k = 0; k < arrDepDcs.Count; k++)
                        {
                            if (strDepDcs.ToString() == string.Empty)
                                strDepDcs.Append(arrDepDcs[k].ToString());
                            else
                                strDepDcs.Append("," + arrDepDcs[k].ToString());
                        }

                        //TODO: this will store the dep fill grid dcs for a non grid field.
                        //fld.fieldDepFillDcs = strDepDcs.ToString();

                    }
                }

                flds[fIdx] = fld;
            }
            FillTabGridParentFlds(dc.frameno, dc);
            //Update the sql fields in the dc.
            dc.DCAcceptMRFields = strDcMRFields.ToString();
            //update the parent flds for tab dc
            //dcs[dc.frameno - 1] = dc;
            dcs[dcIdx] = dc;
            LoadDCNewArray(dc);
        }
    }

    private ArrayList GetGridDcs()
    {
        ArrayList gridDcs = new ArrayList();
        foreach (DcStruct dc in dcs)
        {
            if (dc.isgrid)
                gridDcs.Add(dc.frameno);
        }
        return gridDcs;
    }


    /// <summary>
    /// Function to generate the Tstruct Header Html.
    /// </summary>
    /// <param name="ToolbarBtns"></param>
    /// <returns></returns>
    public string GetHeaderHtml(string Caption)
    {

        StringBuilder strHeader = new StringBuilder();

        //string nextimg = "../AxpImages/icons/16x16/navigate_wtright.png";
        //string previmg = "../AxpImages/icons/16x16/navigate_wtleft.png";
        //Util.Util util = new Util.Util();
        //string direction1 = "left";
        //string direction2 = "righbreadcrumb-panelt";
        //if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
        //{
        //    direction1 = "right";
        //    direction2 = "left";
        //}
        ////if (util.BreadCrumb && breadCrumStr == string.Empty)

        ////    //headerHtml.Append("<div class='icon-services bcrumb h3 " + direction1 + "' style='margin-top: 5px;text-align: left;font-size: 30px;margin-left: 13px;color:#000;'><span style='margin-bottom: 5px;margin-left: 10px;position: absolute;font-size: 20px;left: 9px;'>" + breadCrumStr + Caption + "</span></div>");

        ////else if



        //strHeader.Append("<div id='breadcrumb-panel' class='dvmainbcp'>");

        ////if (util.BreadCrumb && breadCrumStr != string.Empty)
        ////{
        ////    strHeader.Append("<div class='icon-services bcrumb " + direction1 + "'><span class=\"tstivCaption\">" + Caption + "</span><span class=\"menuBreadCrumb\"><span class=\"breadCrumbCaption\">" + breadCrumStr + "</span>" + Caption + "</span></div>");
        ////}
        ////else
        ////{
        //strHeader.Append("<div class='icon-services bcrumb " + direction1 + "'><span class=\"tstivCaption tstivtitle\">" + Caption + "</span></div>");
        ////}

        ////    //headerHtml.Append("<div class='icon-services bcrumb " + direction1 + "'style='text-align: left;font-size: 30px;margin-left: 13px;color:#000;'><span style='margin-bottom: 5px;margin-left: 10px;position: absolute;font-size: 20px;left: 9px;'>" + Caption + "</span><span>" + breadCrumStr + "</span></div>");
        ////    strHeader.Append("<div class='icon-services bcrumb " + direction1 + "' style='text-align: left;font-size: 30px;color:#000;'><span style='color: #000000;position: relative;top: -2px;font-size: 20px !important;left: 32px;'>" + Caption + "</span></div>");
        ////else
        ////strHeader.Append("<div class='icon-services bcrumb " + direction1 + "'><span>" + Caption + "</span></div>");


        ////strHeader.Append("<div id='breadcrumb' class='icon-services'>");
        ////<a href="javascript:void(0)">Service</a>  >  <a href="javascript:void(0)">Job Order</a>  >  <span>Job Order</span>


        ////strHeader.Append("<div id='icons' class='" + direction2 + "'><ul>");
        ////strHeader.Append(ToolbarBtns + "</ul></div>");
        ////strHeader.Append("</div>");


        //strHeader.Append("<div id='nextprevicons' class='nextprevicon '><a href=\"javascript:void(0)\" class='previous'><span onclick='javascript:GetRecord(\"prev\");' class=\"glyphicon glyphicon-chevron-left icon-arrows-left\"> </span></a> <a href=\"javascript:void(0)\" class='previous'><span onclick='javascript:GetRecord(\"prev\");' class=\"glyphicon glyphicon-chevron-left\"> </span></a></div>");
        //strHeader.Append("<div class='clear'></div></div>");

        strHeader.Append("<h1 class=\"text-dark fw-boldest my-1 fs-2\">" + Caption + "<small class=\"text-muted fs-6 fw-normal ms-1\"></small></h1>");

        return strHeader.ToString();
    }

    /// <summary>
    /// Main function to construct Dc Html.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <returns></returns>
    public string GetDcHtml(int dcIndNo, int dcNo)
    {
        StringBuilder dcFinalHtml = new StringBuilder();
        string fieldhtml = string.Empty;
        fldNo = 0;
        colCount = 0;
        fieldhtml = GetFieldHtml(dcIndNo, dcNo);
        dcFinalHtml.Append(GetFullDCHtml(dcIndNo, dcNo, fieldhtml, "false"));

        return dcFinalHtml.ToString();
    }

    /// <summary>
    /// Function to construct dc as per page position
    /// </summary>
    /// <param name="dcNo"></param>
    /// <returns></returns>
    public string GetPagePositionHtml(string dcNo)
    {
        StringBuilder dcFinalHtml = new StringBuilder();
        string fieldhtml = string.Empty;
        if (dcNo.Contains(","))
        {
            dcFinalHtml.Append(GetDCTabs(dcNo));
        }
        else
        {
            int dcIndNo = dcsPositionIndex.IndexOf(dcNo.ToString());
            if (IsDcFormatGrid(dcIndNo, Convert.ToInt32(dcNo)))
            {
                TStructData tstData = new TStructData("", transId, "0", this);
                dcFinalHtml.Append(GetDefaultFormatGridHtml(dcIndNo, Convert.ToInt32(dcNo), tstData, 1, "formatGrid", "false"));
            }
            else
            {
                fieldhtml = GetFieldHtml(dcIndNo, Convert.ToInt32(dcNo));

                if (tstLayout == Constants.TILE)
                {
                    dcFinalHtml.Append("<div class=\"tileLayoutDiv col-sm-6 col-md-6 col-lg-6\">");
                }

                dcFinalHtml.Append(GetFullDCHtml(dcIndNo, Convert.ToInt32(dcNo), fieldhtml, "false"));

                if (tstLayout == Constants.TILE)
                {
                    dcFinalHtml.Append("</div>");
                    if ((Convert.ToInt32(dcNo) % 2) == 0)
                    {
                        dcFinalHtml.Append("<div class=\"clearfix\"></div>");
                    }
                }
            }
        }
        return dcFinalHtml.ToString();
    }

    /// <summary>
    /// Function to construct dc as per page position in DesignMode
    /// </summary>
    /// <param name="dcNo"></param>
    /// <returns></returns>
    public string GetPagePositionHtmlDesign(string dcNo)
    {
        StringBuilder dcFinalHtml = new StringBuilder();
        string fieldhtml = string.Empty;
        if (dcNo.Contains(","))
        {
            dcFinalHtml.Append(GetDCTabsDesign(dcNo));
        }
        else
        {
            int dcIndNo = dcsPositionIndex.IndexOf(dcNo.ToString());
            if (IsDcFormatGrid(dcIndNo, Convert.ToInt32(dcNo)))
            {
                TStructData tstData = new TStructData("", transId, "0", this);
                //dcFinalHtml.Append(GetTabFullHTML(Convert.ToInt32(dcNo), 1, tstData, "false", "false"));
                dcFinalHtml.Append(GetDefaultFormatGridHtml(dcIndNo, Convert.ToInt32(dcNo), tstData, 1, "formatGrid", "false"));
            }
            else
            {
                fieldhtml = GetFieldHtml(dcIndNo, Convert.ToInt32(dcNo));
                dcFinalHtml.Append(GetFullDCHtml(dcIndNo, Convert.ToInt32(dcNo), fieldhtml, "false"));
            }
        }
        return dcFinalHtml.ToString();
    }

    private int GetMaxTabDcsHgt(string[] dcNos)
    {
        int dcHeight = 0;
        for (int j = 0; j < dcNos.Length; j++)
        {
            int tabNo = Convert.ToInt32(dcNos[j]);
            int dcIndNo = dcsPositionIndex.IndexOf(tabNo.ToString());
            DcStruct dc = (DcStruct)(dcs[dcIndNo]);
            if (dcHeight < dc.dcHeight)
                dcHeight = dc.dcHeight;
        }
        return dcHeight;
    }

    /// <summary>
    /// Function to construct tabs for dc's.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <returns></returns>
    public string GetDCTabs(string dcNo)
    {
        StringBuilder tabHdrHtml = new StringBuilder();
        StringBuilder tabInnerHtml = new StringBuilder();
        string fieldhtml = string.Empty;
        string finalHtml = string.Empty;
        string strGridHtml = string.Empty;

        string[] dcNos = dcNo.Split(',');
        string tabAlign = "left";

        if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
            tabAlign = "right";

        //write a function to get the max tab height
        maxDcHeight = GetMaxTabDcsHgt(dcNos);
        if (maxDcHeight < 250)
            maxDcHeight = 255;
        tabHdrHtml.Append("<div class=\"col-12 pb-2\"><div class=\"tab-content\" role=\"tabpanel\">");
        for (int j = 0; j < dcNos.Length; j++)
        {
            int tabNo = Convert.ToInt32(dcNos[j]);
            int dcIndNo = dcsPositionIndex.IndexOf(tabNo.ToString());
            tabDCs.Add(tabNo);
            tabFunction = string.Empty;
            //DcStruct dc = (DcStruct)(dcs[tabNo - 1]);
            DcStruct dc = (DcStruct)(dcs[dcIndNo]);

            string dcBoolean = string.Empty;
            if (dc.DcDefaultstate != "" && dc.DcDefaultstate.ToLower() == "collapse")
                dcBoolean = " d-none ";

            if (j == 0)
            {
                tabHdrHtml.Append("<ul id='myTab' class=\"cursor-pointer nav nav-tabs mb-n2 border border-gray-300 rounded-0 bg-light\">");
                tabInnerHtml.Append("<div id=\"" + "tabsCont" + dcNos[0] + "\" class='tab-content'>");
                if (IsDcFormatGrid(dcIndNo, tabNo))
                {
                    TStructData tstData = new TStructData("", transId, "0", this);
                    strGridHtml = GetDefaultFormatGridHtml(dcIndNo, tabNo, tstData, 1, "true", "false");
                }
                else
                {
                    fieldhtml = GetFieldHtml(dcIndNo, tabNo);
                    strGridHtml = "<div class='mainContent'>" + GetFullDCHtml(dcIndNo, tabNo, fieldhtml, "true") + "</div>";
                }
                tabDCStatus.Add("1");
            }
            else
            {
                tabDCStatus.Add("0");
            }
            string activeClass = String.Empty;
            activeClass = j == 0 ? "active" : "";
            string dcBooleanHtml = string.Empty;
            string _tabDivCollapse = "";
            if (tabNo > 1)
            {
                if (activeClass != "")
                    _tabDivCollapse = "<span title='Hide Dc' onclick='showHideTabbedDc(this)' id='tabCollapseButton" + tabNo + "' class='mt-2 cursor-pointer material-icons material-icons-style'>keyboard_arrow_up</span>";
                else
                    _tabDivCollapse = "<span title='Hide Dc' onclick='showHideTabbedDc(this)' id='tabCollapseButton" + tabNo + "' class='mt-2 cursor-pointer material-icons material-icons-style d-none'>keyboard_arrow_up</span>";
            }

            if (dc.DcBlean != "" && dc.DcBlean.ToUpper() == "TRUE")
            {
                //tabHdrHtml.Append("<div class=\"nav-link fw-boldest d-flex p-4 shadow-sm " + activeClass + "\">");
                if (activeClass != "")
                {
                    dcBooleanHtml = "<div class=\"form-check form-switch form-check-custom px-1 align-self-end my-auto \"><input type=\"checkbox\" id=\"dcBlean" + tabNo + "\" data-dcname=\"" + dc.name + "\" title=\"\" style=\"\" name=\"dcBlean" + tabNo + "\" class=\"form-check-input opacity-100 gridHeaderSwitch \" " + (dcBoolean == string.Empty ? "checked" : "") + "></div>";
                    tabHdrHtml.Append("<li class=\"nav-item d-flex p-3 shadow-sm bg-white rounded-top\" id=\"li" + tabNo + "\">");
                }
                else
                {
                    dcBooleanHtml = "<div class=\"form-check form-switch form-check-custom px-1 align-self-end my-auto d-none\"><input type=\"checkbox\" id=\"dcBlean" + tabNo + "\" data-dcname=\"" + dc.name + "\" title=\"\" style=\"\" name=\"dcBlean" + tabNo + "\" class=\"form-check-input opacity-100 gridHeaderSwitch \" " + (dcBoolean == string.Empty ? "checked" : "") + "></div>";
                    tabHdrHtml.Append("<li class=\"nav-item d-flex p-3 shadow-sm bg-white rounded-top\" id=\"li" + tabNo + "\">");
                }
                tabHdrHtml.Append(_tabDivCollapse + dcBooleanHtml + "<a class=\"fw-bold fs-6 nav-link fw-boldest text-gray-800 rounded-0 border-0 " + activeClass + "\" data-bs-toggle=\"tab\" data-toggle=\"tab\" href=\"#tab-" + tabNo + "\" id=\"ank" + tabNo + "\" onclick=\"javascript:GetTabData(" + dc.frameno + ");\">" + dc.caption + "</a></li>");
            }
            else
            {
                tabHdrHtml.Append("<li class=\"nav-item d-flex p-3 shadow-sm bg-white rounded-top\" id=\"li" + tabNo + "\">");
                tabHdrHtml.Append(_tabDivCollapse + "<a class=\"fw-bold fs-6 nav-link fw-boldest text-gray-800 rounded-0 border-0 " + activeClass + "\" data-bs-toggle=\"tab\" data-toggle=\"tab\" href=\"#tab-" + tabNo + "\" id=\"ank" + tabNo + "\" onclick=\"javascript:GetTabData(" + dc.frameno + ");\">" + dc.caption + "</a></li>");
            }
            tabInnerHtml.Append("<div class='tab-pane fade show " + activeClass + "  ' id=\"tab-" + dc.frameno + "\" >");
            if (j == 0)
                tabInnerHtml.Append(strGridHtml);

            tabInnerHtml.Append("</div>");

            if (j == dcNos.Length - 1)
            {
                tabHdrHtml.Append("</ul>");
                tabInnerHtml.Append("</div>");
            }
        }
        finalHtml = tabHdrHtml.ToString() + tabInnerHtml.ToString() + "</div></div>";
        return finalHtml;
    }

    /// <summary>
    /// Function to construct tabs for dc's.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <returns></returns>
    public string GetDCTabsDesign(string dcNo)
    {
        StringBuilder tabHdrHtml = new StringBuilder();
        StringBuilder tabInnerHtml = new StringBuilder();
        string fieldhtml = string.Empty;
        string finalHtml = string.Empty;
        string strGridHtml = string.Empty;

        string[] dcNos = dcNo.Split(',');
        string tabAlign = "left";

        if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
            tabAlign = "right";

        //write a function to get the max tab height
        maxDcHeight = GetMaxTabDcsHgt(dcNos);
        if (maxDcHeight < 250)
            maxDcHeight = 255;

        for (int j = 0; j < dcNos.Length; j++)
        {
            int tabNo = Convert.ToInt32(dcNos[j]);
            int dcIndNo = dcsPositionIndex.IndexOf(tabNo.ToString());
            tabDCs.Add(tabNo);
            tabFunction = string.Empty;
            DcStruct dc = (DcStruct)(dcs[dcIndNo]);

            if (j == 0)
            {
                //    tabHdrHtml.Append("<div class='demo'><div id=\"" + "tabs" + dcNos[0] + "\"><ul>");
                tabHdrHtml.Append("<div class='wrapper'><div class=''><div class='bs-example bs-example-tabs' role='tabpanel' data-example-id='togglable-tabs'>");
                tabHdrHtml.Append("<ul id='myTab' class='nav nav-tabs nav-tabs-responsive' role='tablist'>");


                tabInnerHtml.Append("<div id=\"" + "tabsCont" + dcNos[0] + "\" class='tab-content'>");
                //tabInnerHtml.Append("<div id=\"" + "tabsCont" + dcNos[0] + "\" style=\"margin-top:0px;\"> ");
                if (IsDcFormatGrid(dcIndNo, tabNo))
                {
                    TStructData tstData = new TStructData("", transId, "0", this);
                    //strGridHtml = GetTabFullHTML(Convert.ToInt32(dcNo), 1, tstData, "false", "false");
                    strGridHtml = GetDefaultFormatGridHtml(dcIndNo, tabNo, tstData, 1, "true", "false");
                }
                else
                {
                    fieldhtml = GetFieldHtml(dcIndNo, tabNo);
                    strGridHtml = "<div class='card card-xl-stretch mb-1 mb-xl-2 border border-gray-300 rounded-0 mainContent'>" + GetFullDCHtml(dcIndNo, tabNo, fieldhtml, "true") + "</div>";
                }
                tabDCStatus.Add("1");
            }
            else
            {
                tabDCStatus.Add("1");
                fieldhtml = GetFieldHtml(dcIndNo, tabNo);
                strGridHtml = "<div class='card card-xl-stretch mb-1 mb-xl-2 border border-gray-300 rounded-0 mainContent'>" + GetFullDCHtml(dcIndNo, tabNo, fieldhtml, "true") + "</div>";


            }

            string activeClass = String.Empty;
            activeClass = j == 0 ? "class='active'" : j == 1 ? "class='next'" : "";
            string active = j == 0 ? " active in" : "";
            if (((DcStruct)(dcs[dcIndNo])).isgrid)
                tabHdrHtml.Append("<li id=\"li" + tabNo + "\" role='presentation' " + activeClass + " ><a href=\"#tab-" + tabNo + "\" id=\"" + "ank" + tabNo + "\" role='tab' onclick=\"javascript:GetTabData(" + dc.frameno + ");" + tabFunction + "\" data-toggle='tab' aria-expanded='true' ><span class='text'>" + dc.caption + "</span><span class='gridoptions glyphicon material-icons' title='Grid Option'>more_vert</span></a></li>");
            else
                tabHdrHtml.Append("<li id=\"li" + tabNo + "\" role='presentation' " + activeClass + " ><a href=\"#tab-" + tabNo + "\" id=\"" + "ank" + tabNo + "\" role='tab' onclick=\"javascript:GetTabData(" + dc.frameno + ");" + tabFunction + "\" data-toggle='tab' aria-expanded='true' ><span class='text'>" + dc.caption + "</span></a></li>");
            //tabHdrHtml.Append("<li id=\"li" + tabNo + "\" style='float:" + tabAlign + "'><a  id=\"" + "ank" + tabNo + "\"  href=\"#tab-" + tabNo + "\" onclick=\"javascript:GetTabData(" + dc.frameno + ");" + tabFunction + "\"><span>" + dc.caption + "</span></a></li>");
            //tabInnerHtml.Append("<div id=\"tab-" + dc.frameno + "\" style=\"min-height:" + maxDcHeight + "px\">");
            //tabInnerHtml.Append("<div id=\"tab-" + dc.frameno + "\">");

            tabInnerHtml.Append("<div role='tabpanel' class='tab-pane fade " + active + "  ' id=\"tab-" + dc.frameno + "\" >");
            //<div class=\"container-fluid mainContent\">
            if (j == 0)
                tabInnerHtml.Append(strGridHtml);
            else
                tabInnerHtml.Append(strGridHtml);
            tabInnerHtml.Append("</div>");
            //tabInnerHtml.Append("</div>");

            if (j == dcNos.Length - 1)
            {
                tabHdrHtml.Append("</ul>");
                tabInnerHtml.Append("</div>");
            }
        }

        finalHtml = tabHdrHtml.ToString() + tabInnerHtml.ToString() + "</div></div></div>";
        return finalHtml;
    }

    /// <summary>
    /// Gets the tab inner html for the currently clicked tabbed DC.
    /// </summary>
    /// <param name="DcNo"></param>
    /// <returns></returns>
    public string GetTabHtml(int dcIndNo, string DcNo)
    {
        int tabNo = 0;
        if (DcNo != "")
            tabNo = Convert.ToInt32(DcNo);
        string fieldhtml = string.Empty;
        string finalHtml = string.Empty;
        fieldhtml = GetFieldHtml(dcIndNo, tabNo);
        finalHtml = (GetFullDCHtml(dcIndNo, tabNo, fieldhtml, "true"));
        return finalHtml;
    }

    /// <summary>
    /// Function to check if the field is checkbox in grid.
    /// </summary>
    /// <param name="moeValue"></param>
    /// <returns></returns>
    private Boolean IsGridCheckBox(string[] moeValue)
    {
        bool chkBox = false;

        if (moeValue.Length <= 3)
        {
            StringBuilder tmpStr = new StringBuilder();
            for (int cnt = 0; cnt < moeValue.Length; cnt++)
            {
                tmpStr.Append(moeValue[cnt].ToLower());
            }
            chkBox = tmpStr.ToString() == "yesno" || tmpStr.ToString() == "noyes" ? true : false;
        }
        return chkBox;
    }

    /// <summary>
    /// Function to return the currently visible DC's in the tstruct.
    /// </summary>
    /// <returns></returns>
    public string GetVisibleDCs()
    {
        int i = 0; string strDc = string.Empty;
        for (i = 0; i < visibleDCs.Count; i++)
        {
            if (i == 0)
            {
                strDc = "dc" + visibleDCs[i].ToString();
            }
            else
            {
                strDc += "," + "dc" + visibleDCs[i].ToString();
            }
        }
        return strDc;
    }

    /// <summary>
    /// Function to construct the field's id from the arrays.
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns>The Component field ID of the given field name.</returns>
    public string GetFieldID(string fieldName)
    {
        string fieldId = "";
        int dcNo = 0;
        for (int i = 0; i < flds.Count; i++)
        {
            FieldStruct fld = (FieldStruct)flds[i];
            if (fld.name == fieldName)
            {
                dcNo = fld.fldframeno;
                break;
            }
        }

        bool IsGrid = false;
        for (int j = 0; j < dcs.Count; j++)
        {
            DcStruct dc = (DcStruct)dcs[j];
            if (dc.frameno == dcNo)
            {
                IsGrid = dc.isgrid;
                break;
            }
        }

        if (IsGrid)
            fieldId = fieldName + "001F" + dcNo;
        else
            fieldId = fieldName + "000F" + dcNo;

        return fieldId;
    }

    /// <summary>
    /// Function to get the index of the given field from the fields in the tstruct.
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public int GetFieldIndex(string fieldName)
    {
        int fieldIndex = -1;
        for (int i = 0; i < flds.Count; i++)
        {
            FieldStruct fld = (FieldStruct)flds[i];
            if (fld.name.ToUpper() == fieldName.ToUpper())
            {
                fieldIndex = i;
                break;
            }
        }
        return fieldIndex;
    }
    public int GetFieldDc(string fieldName)
    {
        int dcNo = 0;
        for (int i = 0; i < flds.Count; i++)
        {
            FieldStruct fld = (FieldStruct)flds[i];
            if (fld.name.ToLower() == fieldName.ToLower())
            {
                dcNo = fld.fldframeno;
                break;
            }
        }
        return dcNo;
    }

    public string GetFieldParents(string fieldName)
    {
        string fieldParents = string.Empty;
        for (int i = 0; i < flds.Count; i++)
        {
            FieldStruct fld = (FieldStruct)flds[i];
            if (fld.name.ToLower() == fieldName.ToLower())
            {
                fieldParents = fld.fieldParents;
                break;
            }
        }
        return fieldParents;
    }

    public int GetButtonIndex(string btnName)
    {
        int btnIndex = -1;
        for (int i = 0; i < btns.Count; i++)
        {
            ButtonStruct invbtn = (ButtonStruct)btns[i];
            if (invbtn.ID.ToUpper() == btnName.ToUpper())
            {
                btnIndex = i;
                break;
            }
        }
        return btnIndex;
    }

    public string GetColumnWidth(string dcNo, string columnName)
    {
        string colWidth = defaultColWidth;
        for (int i = 0; i < fldDcRange.Count; i++)
        {
            string[] fldRange = fldDcRange[i].ToString().Split('~');
            if (dcNo == fldRange[0].ToString())
            {
                string[] dcIndex = fldRange[1].Split(',');
                int startIndex = Convert.ToInt32(dcIndex[0].ToString());
                int endIndex = Convert.ToInt32(dcIndex[1].ToString());
                for (int j = startIndex; j <= endIndex; j++)
                {
                    FieldStruct fld = (FieldStruct)flds[j];
                    if (fld.caption.ToLower() == columnName.ToLower())
                    {
                        string fldWidth = fld.cwid;
                        if (fldWidth != "" && Convert.ToInt32(fldWidth) > -1)
                        {
                            if (Convert.ToInt32(colWidth) > 130) colWidth = "100";
                            colWidth = fldWidth;
                            break;
                        }
                    }
                }
            }
        }
        return colWidth;
    }


    #region GetJScriptArrays
    /// <summary>
    /// Function to Get the javascript dependency arrays and write it on to the response
    /// </summary>
    /// <param name="strObj"></param>
    public string GetJScriptArrays(TStructDef strObj)
    {
        string tst_Scripts = string.Empty;
        StringBuilder strJSArray = new StringBuilder();
        strJSArray.Append(strObj.jsFieldArray);
        strJSArray.Append(strObj.jsFormcontrolArray);
        strJSArray.Append(strObj.jsHyperlinkArray);
        strJSArray.Append(strObj.jsPatternArray);
        strJSArray.Append(strObj.jsExpressionArray);
        strJSArray.Append(strObj.jsDCArray);
        strJSArray.Append(strObj.jsTabDCArray);
        strJSArray.Append(strObj.jsRuleDefArray);

        string tstructDetails = "var tstructName='" + strObj.transId + "';var tstructCaption='" + ReplaceSpecialCharsInHTML(strObj.tstCaption) + "';var tstructAttachment='" + strObj.tstAttachment + "';var tstructCancelled='" + strObj.tstCancelled + "';var tstructUpdateOn='" + strObj.tstUpdateOn + "';var tstructUpdatedBy='" + strObj.tstUpdateBy + "';var tstructReadonly='" + strObj.tstReadOnly + "';var tstructWorkflow='" + strObj.tstWorkflow + "';var tstructCachedsave='" + strObj.tstCachedsave + "';var AxTstIsPEG='" + AxTstIsPEG + "';";

        string listFlds = "AxFromLstFlds='" + fromListFlds + "';";

        string dcNum = "TotalDCs='" + strObj.dcs.Count + "';";
        tst_Scripts = "<script language='javascript' type='text/javascript' >" + strJSArray.ToString() + tstructDetails + dcNum + listFlds + "</script>";
        tst_Scripts = tst_Scripts.Replace("\n", "");
        return tst_Scripts;
    }

    /// <summary>
    /// Function to return the tab event by generating the script for tab container.
    /// </summary>
    /// <param name="strObj"></param>
    /// <returns></returns>
    public string GenerateTabScript(TStructDef strObj)
    {
        StringBuilder dcScript = new StringBuilder();
        dcScript.Append("<script type='text/javascript'>function LoadGridScript(){");
        tabScript = new StringBuilder();
        for (int i = 0; i < pagePositions.Count; i++)
        {
            if (pagePositions[i].ToString().Contains(","))
            {
                string[] tabContainers = pagePositions[i].ToString().Split(',');
                tabScript.Append("<script type='text/javascript'>$j(function() {$j(\"" + "#tabs" + tabContainers[0] + "\").tabs().scrollabletab();}); SafariTabDcHeight();</script>");
            }
        }
        dcScript.Append("}</script>");

        //return tabScript.ToString() + dcScript.ToString();
        return dcScript.ToString();
    }

    public Boolean IsDcGrid(int dcNo)
    {
        Boolean isGrid = false;
        for (int i = 0; i < dcs.Count; i++)
        {
            DcStruct dcStr = (DcStruct)dcs[i];
            if (dcStr.frameno == dcNo)
            {
                if (dcStr.isgrid)
                    isGrid = true;
                else
                    isGrid = false;
            }
        }
        return isGrid;
    }


    #endregion

    #endregion

    #region Field HTML

    /// <summary>
    /// function to construct the field html for a particular dc. It doesnot contains the outer
    /// part of the DC like outer border, DC header, ADD, Fill.
    /// It iterates throug the complete fields, check whether it belongs the DC by checking the
    /// frame no. Then it constructs the HTML string for that field and appended to the DC HTML.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <seealso cref="GetFinalHtml"/>
    /// <returns> returns the field html. </returns>
    private string GetFieldHtml(int dcIndNo, int dcNo)
    {
        //NOTE: Assigning javascript functions for grid and non-grid fields's is done seperately,
        //as the grid field javascript functions are written into the hidden field through the variable gridHiddenHtml. 
        //The difference for grid functions is usage of \"+quo+\" instead of \".   
        //The hidden field is used to store the dummy row value, 
        //the value assigned to hidden field will be written within double quotes and hence we use \"+quo+\" to write a double quote in the HTML.
        colCount = 0;
        Boolean isGrid = ((DcStruct)(dcs[dcIndNo])).isgrid;
        DcStruct dc = (DcStruct)(dcs[dcIndNo]);
        if (isGrid)
        {
            dcArrayNo++;
        }
        string divDirection = "left";
        if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
        {
            divDirection = "right";
        }
        int fcwidth = 0;
        string colFldCaption = "", colFldWidth = "", colFldGridStackItem = "", colDivInputPadding = "";
        if (!isGrid && (axdesignJObject.dcLayout != null && axdesignJObject.dcLayout != "" && axdesignJObject.dcLayout != "default"))
        {
            fcwidth = int.Parse(axdesignJObject.fieldCaptionWidth);
            fcwidth = fcwidth / 10;
            colFldCaption = "col-sm-" + fcwidth + "";
            fcwidth = 12 - fcwidth;
            colFldWidth = "<div class=\"input-group col-sm-" + fcwidth + "\">";
            colFldGridStackItem = " colFldGridStackWidth ";
            colDivInputPadding = " colDivInputPadding ";
        }

        if (colFldWidth == "")
            colFldWidth = "<div class=\"input-group\">";

        dummyGridHeadHtml = new StringBuilder();
        gridDcColwidth = 0;
        gridHeadHtml = new StringBuilder();
        gridHiddenHtml = new StringBuilder();
        string colSpan = string.Empty;
        string gridStackData = string.Empty;
        string lblBsClass = string.Empty;
        string lblBsIndex = string.Empty;
        string fldBsClass = string.Empty;
        Boolean isLastDcDate = false;
        int randomID = 10;
        StringBuilder fieldHtml = new StringBuilder();
        lastDcHeight = 0;
        int gridInSlackGridX = 0;
        int gridInSlackGridY = 0;
        string layoutstyle = string.Empty;
        if (HttpContext.Current.Session["layoutstyle"] != null)
        {
            layoutstyle = HttpContext.Current.Session["layoutstyle"].ToString();
        }

        for (int i = 0; i <= flds.Count - 1; i++)
        {
            FieldStruct fld = ((FieldStruct)(flds[i]));
            if (fld.fldframeno < dcNo) continue;
            if (fld.fldframeno > dcNo) continue; //break;
            if (fld.fldframeno == dcNo)
            {
                //As the following code is going to be called for each field,
                //its not made as method to avoid function call.
                string[] tlhw;
                if (fld.lbltlhw.ToString() != "")
                {
                    tlhw = fld.lbltlhw.ToString().Split(',');
                    if (EnableOldTheme == "false")
                    {
                        tlhw[0] = Math.Floor((Convert.ToInt32(tlhw[0].ToString()) * 1.6)).ToString();
                        tlhw[2] = "30";
                        tlhw[1] = Math.Floor((Convert.ToInt32(tlhw[1].ToString()) * 1.07)).ToString();
                        tlhw[3] = Math.Floor((Convert.ToInt32(tlhw[3].ToString()) * 1.15)).ToString();
                    }
                    else
                    {
                        tlhw[0] = Math.Floor((Convert.ToInt32(tlhw[0].ToString()) * 1.2)).ToString();
                        tlhw[1] = Math.Floor((Convert.ToInt32(tlhw[1].ToString()) * 1.0)).ToString();
                    }
                }
                else
                {
                    tlhw = ",,,".Split(','); // Hack: directly entering the data to the tlhw throws error
                    tlhw[0] = "";
                    tlhw[1] = "";
                    tlhw[2] = "";
                    tlhw[3] = "";
                }



                string[] fldTlhw;
                if (fld.fldtlhw.ToString() != "")
                {
                    fldTlhw = fld.fldtlhw.ToString().Split(',');
                    if (EnableOldTheme == "false")
                    {
                        fldTlhw[0] = Math.Floor((Convert.ToInt32(fldTlhw[0].ToString()) * 1.6)).ToString();
                        fldTlhw[1] = Math.Floor((Convert.ToInt32(fldTlhw[1].ToString()) * 1.07)).ToString();
                    }
                    else
                    {
                        fldTlhw[0] = Math.Floor((Convert.ToInt32(fldTlhw[0].ToString()) * 1.2)).ToString();
                        fldTlhw[1] = Math.Floor((Convert.ToInt32(fldTlhw[1].ToString()) * 1.0)).ToString();
                    }


                }
                else
                {
                    fldTlhw = ",,,".Split(','); // Hack: directly entering the data to the tlhw throws error
                    fldTlhw[0] = "0";
                    fldTlhw[1] = "0";
                    fldTlhw[2] = "0";
                    fldTlhw[3] = "0";
                }

                if (fld.fldlength != 0)
                {
                    lblBsClass = bsClassByDatawidth(fld.fldlength, i, fld);
                    lblBsIndex = fld.fieldIndex.ToString();
                }

                if (!fld.visibility)
                    gridStackData = xywhGridStack(fld);

                string name = string.Empty;
                string hiddenName = string.Empty;
                string width = string.Empty;
                string fdwidth = "0";
                int rndnumber = 0;
                Random randNum = new Random();
                rndnumber = randNum.Next(100000, 1000000);
                fldNo = i;

                if (!fld.visibility)
                    fdwidth = xywhGridStack(fld, true);

                if (isGrid)
                {
                    name = fld.name + "001F" + fld.fldframeno;
                    hiddenName = fld.name + "\" +nrno+\"";
                    // for grid its column width
                    width = fld.cwid;
                    if (!fld.visibility)
                    {
                        if (width != "")
                            gridDcColwidth += Convert.ToInt32(width);
                        else
                            gridDcColwidth += 1;
                    }
                }
                else
                {
                    name = fld.name + "000F" + fld.fldframeno;
                    width = fldTlhw[3];
                }

                if (width == "")
                    width = "0";
                else
                    width = Convert.ToString(Convert.ToInt32(width) - 4);

                if (fld.caption.Contains("~"))
                    fld.caption = fld.caption.Replace("~", "\n");


                if (fld.visibility)
                {
                    string cssAutoGen = string.Empty;
                    if (fld.moe == "AutoGenerate")
                        cssAutoGen = "class=\"autogen\"";

                    if (name.StartsWith("axp_recid"))
                    {
                        fieldHtml.Append("<div class=\"gridElement d-none form-group\" id=\"dvGrid" + fld.name.Replace(" ", "") + "\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + name + "\" type=\"hidden\" value=\"0\" name=\"" + name + "\" value=\"" + fld.Value + "\" /></div></div>");
                        if (isGrid)
                            gridHiddenHtml.Append("<div class=\" grid-stack-item form-group\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + hiddenName + "\" type=hidden value=0 name=\"" + hiddenName + "\" /></div></div>");
                    }
                    else
                    {
                        if (fld.name.ToLower().StartsWith("axpfilepath_") && cssAutoGen == "")
                        {
                            var endPart = name.Substring(12);
                            cssAutoGen = "class=\"axpFilePath_" + endPart + " axpFilePathFld \"";
                        }
                        if (fld.moe == "Select")
                            fieldHtml.Append("<div class=\"gridElement form-group d-none\" id=\"dvGrid" + fld.name.Replace(" ", "") + "\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + name + "\" type=\"hidden\" name=\"" + name + "\" " + cssAutoGen + " value=\"" + fld.Value + "\" data-val=\"" + fld.moeval + "\"/></div></div>");
                        else
                            fieldHtml.Append("<div class=\"gridElement form-group d-none\" id=\"dvGrid" + fld.name.Replace(" ", "") + "\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + name + "\" type=\"hidden\" name=\"" + name + "\" " + cssAutoGen + " value=\"" + fld.Value + "\" /></div></div>");
                        if (isGrid)
                            gridHiddenHtml.Append("<div class=\" grid-stack-item form-group\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + hiddenName + "\" type=\"hidden\" name=\"" + hiddenName + "\" " + cssAutoGen + " /></div></div>");

                    }
                    if (fld.caption.Contains("~"))
                    {
                        gridHeadHtml.Append("<th style='display:none;' id=\"th-" + fld.name + "\"  class=\"wordBreak\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead'></div></th>");
                    }
                    else
                    {
                        gridHeadHtml.Append("<th class=\"d-none\" id=\"th-" + fld.name + "\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead'></div></th>");
                    }
                }


                if (!fld.visibility)
                {
                    randomID++;
                    string popImg = string.Empty;
                    string popImgHidden = string.Empty;
                    int popwidth = 0;
                    for (int x = 0; x < popdcs.Count; x++)
                    {
                        if (((DcStruct)(dcs[dcIndNo])).frameno == ((PopDcStruct)(popdcs[x])).pdcno)
                        {
                            if (fld.name.ToLower() == ((PopDcStruct)(popdcs[x])).popfield.ToLower())
                            {
                                popImgHidden = "  <div alt='Open'  class='subGrid glyphicon glyphicon-new-window icon-basic-elaboration-browser-plus' id='001F" + ((PopDcStruct)(popdcs[x])).frameno + "'></div>";
                                popImg = "  <div alt='Open'  class='subGrid glyphicon glyphicon-new-window icon-basic-elaboration-browser-plus' id='001F" + ((PopDcStruct)(popdcs[x])).frameno + "'></div>";
                                popwidth = Convert.ToInt32(width) - 20;
                                break;
                            }
                        }
                    }


                    if (!isGrid)
                    {
                        if (fldTlhw[0] != "")
                            lastDcHeight = Convert.ToInt32(fldTlhw[0]) + Convert.ToInt32(fldTlhw[2]);
                        else if (fldTlhw[2] != "")
                            lastDcHeight = Convert.ToInt32(fldTlhw[2]);
                    }
                    bool isHyper = false;
                    int n = 0;
                    for (n = 0; n <= hlnks.Count - 1; n++)
                    {
                        if (fld.name.ToString() == ((HLinkStruct)(hlnks[n])).hlsource.ToString().Substring(3))
                        {
                            isHyper = true;
                            break;
                        }
                    }

                    string designFontStyle = "";
                    string designHyperLink = "";
                    if (!isGrid)
                    {
                        if (axdesignJObject.buttonFieldFont != null && axdesignJObject.buttonFieldFont.Count > 0)
                        {
                            var frbtnfld = axdesignJObject.buttonFieldFont.Where(elm => elm.id == name).ToList();
                            if (frbtnfld != null && frbtnfld.Count > 0)
                            {
                                designFontStyle = frbtnfld[0].fontFamilly;
                                designHyperLink = frbtnfld[0].hyperlinkJson;
                            }
                        }
                    }

                    if (isGrid)
                    {
                        string ddetails = String.Empty;
                        //if (fld.tooltip != null && fld.tooltip != "")
                        //    ddetails = "<span class=\"material-icons material-icons-style align-middle material-icons-3 cursor-pointer spanddetails\" title=\"Click on any row in this column to know more about the data in the cell.\">info</span>";
                        string allowemptycss = "";
                        if (!fld.allowempty && fld.caption != "")
                            allowemptycss = " required ";
                        colCount++;
                        if (fld.caption.Contains("~"))
                        {
                            gridHeadHtml.Append("<th id=\"th-" + fld.name + "\" style=\"width:" + fdwidth + "px;\" class=\"wordBreak fw-boldest\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead " + allowemptycss + "'>");
                        }
                        else if (fld.datatype.ToUpper() == "DATE/TIME")
                        {
                            gridHeadHtml.Append("<th id=\"th-" + fld.name + "\" style=\"width:" + fdwidth + "px;\" class=\"fw-boldest\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead " + allowemptycss + "'>");
                        }
                        else
                        {
                            gridHeadHtml.Append("<th id=\"th-" + fld.name + "\" style=\"width:" + fdwidth + "px;\" class=\"fw-boldest\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead " + allowemptycss + "'>");
                        }

                        if (isHyper)
                        {
                            gridHeadHtml.Append("<a href=\"javascript:void(0)\" class=\"cursor-pointer\"  id=\"" + ((HLinkStruct)(hlnks[n])).hlsource.ToString() + "\" onclick='javascript:Tstructhyperlink(this);'>");
                        }
                        if (fld.name.ToUpper().IndexOf("AXPBUTTON") == -1)
                        {
                            if (fld.caption.Contains("~"))
                                fld.caption = fld.caption.Replace("~", "\n");

                            gridHeadHtml.Append(fld.caption);
                        }
                        if (isHyper)
                        {
                            gridHeadHtml.Append("</a>");
                        }

                        // to handle the mandatory fields.
                        //if (!fld.allowempty && fld.caption != "")
                        //{
                        //    gridHeadHtml.Append("<span class=\"allowempty\">*</span>");
                        //}

                        //if (!string.IsNullOrEmpty(fld.fieldPurpose))
                        //    gridHeadHtml.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + fld.fieldPurpose + "\">help_outline</i></span>");
                        gridHeadHtml.Append(ddetails + "</div></th>");
                    }
                    else
                    {
                        if (fld.ctype.ToUpper() != "CHECK BOX")
                        {
                            fieldHtml.Append("<div class=' grid-stack-item" + colFldGridStackItem + "' " + gridStackData + " id=\"randomID_" + dc.frameno + randomID + "\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"labelcol inputs form-group row " + lblBsClass + " " + colDivInputPadding + " \" data-dataindex=" + lblBsIndex + ">");
                            string color = "cursor: pointer;";
                            if (fld.fieldLabelCss != "")
                            {
                                //label font will be in the format "font-family,font-size,tttt,color". here the tttt-bold/italic/underline/strikeout
                                string[] newCss = fld.fieldLabelCss.Split(',');
                                string fontFamily = "";
                                string fontSize = "";
                                string textCss = "";

                                if (newCss.Length > 0)
                                {
                                    if (newCss[0].ToString() != "")
                                        fontFamily = "font-family:" + newCss[0].ToString() + ";";
                                }
                                if (newCss.Length > 1)
                                {
                                    if (newCss[1].ToString() != "")
                                        fontSize = "font-size:" + newCss[1].ToString() + ";";
                                }

                                if (newCss.Length > 2)
                                {
                                    if (newCss[2].ToString() != "")
                                    {
                                        string fontStyle = newCss[2].ToString();

                                        if (fontStyle.Substring(0, 1) == "t")
                                            textCss = "font-weight:bold;";
                                        if (fontStyle.Substring(1, 1) == "t")
                                            textCss += "font-style:italic;";
                                        if (fontStyle.Substring(2, 1) == "t")
                                            textCss += "text-decoration:underline;";
                                        if (fontStyle.Substring(3) == "t")
                                            textCss += "text-decoration:overline;";
                                    }
                                }
                                if (newCss.Length > 3)
                                {
                                    if (newCss[3].ToString() != "")
                                    {
                                        string txtColor = newCss[3].ToString().Substring(2);
                                        textCss += " color:" + txtColor + "; ";
                                    }
                                }
                                if (newCss.Length >= 4)
                                {
                                    //changes for lable colors
                                    string lblclr = fld.fieldLabelCss.Split(',').Last();
                                    string[] lblFonts = fld.fieldLabelCss.Split(',');
                                    if (lblclr != "")
                                        color += " color:" + GetFieldColor(lblclr) + "; ";
                                    if (lblFonts[1] != "" && int.Parse(lblFonts[1]) >= 13)
                                        color += " font-size:" + lblFonts[1] + "px; ";
                                    if (lblFonts[2] != "")
                                    {
                                        if (lblFonts[0] != "" && lblFonts[2] != "ttff")
                                            color += " font-family:" + lblFonts[0] + "; ";
                                        if (lblFonts[2] == "tfff")
                                            color += " font-weight:Bold; ";
                                        else if (lblFonts[2] == "ftff")
                                            color += " font-style:Oblique; ";
                                        else if (lblFonts[2] == "ttff")
                                            color += " font-family:Bold Oblique; ";
                                    }
                                }

                                //TODO: the font family is not considered as the ui alignment is not proper in web. we can enable it  in future.
                                //fieldHtml.Append("<font class=\"labelcap\" style=\"" + fontSize + textCss + "\">");
                            }
                            if (fld.allowempty && fld.caption != "")
                                fieldHtml.Append("<div class=\"fld-wrap3 d-felx " + colFldCaption + "\" >");
                            else
                                fieldHtml.Append("<div class=\"fld-wrap3 d-felx required " + colFldCaption + "\" >");

                            if (isHyper)
                                fieldHtml.Append("<a href=\"javascript:void(0)\"  class=\"cursor-pointer\" id=\"" + ((HLinkStruct)(hlnks[n])).hlsource.ToString() + "\" onclick='javascript:Tstructhyperlink(this);'>");
                            else if (designHyperLink != "")
                                fieldHtml.Append("<a href=\"javascript:void(0)\" class=\"cursor-pointer\" id=\"hylnk" + fld.name + "\" onclick='javascript:TstructLabelhyperlink(this);' data-param='" + designHyperLink + "'>");


                            if (designHyperLink != "")
                                fieldHtml.Append("<label class=\"form-label col-form-label pb-1 fw-boldest \" style=\"text-decoration:underline;cursor: pointer;" + (designFontStyle == "" ? color : designFontStyle) + "\"  for=\"" + name + "\" " + (name.ToLower().Contains("axp_url_") ? "data-axp-url='true'" : "") + " >" + fld.caption + "</label >");
                            else
                                fieldHtml.Append("<label class=\"form-label col-form-label pb-1 fw-boldest \" style=\"" + (designFontStyle == "" ? color : designFontStyle) + "\"  for=\"" + name + "\" " + (name.ToLower().Contains("axp_url_") ? "data-axp-url='true'" : "") + " >" + fld.caption + "</label >");//if the tstruct field contains axp_url_ then add an attribute 'data-axp-url' to display hyperlink instead of label
                            //if (fld.tooltip != null && fld.tooltip != "")
                            //{
                            //    fieldHtml.Append("<span class=\"material-icons material-icons-style ms-1 align-middle material-icons-3 cursor-pointer spanddetails\" title=\"Display details\">info</span>");
                            //}
                            if (isHyper || designHyperLink != "")
                                fieldHtml.Append("</a>");

                            //if (!string.IsNullOrEmpty(fld.fieldPurpose))
                            //    fieldHtml.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + fld.fieldPurpose + "\">help_outline</i></span>");
                            fieldHtml.Append("</div>");
                        }
                        else
                        {
                            if (HttpContext.Current.Session["switchboxToCheckbox"] != null && HttpContext.Current.Session["switchboxToCheckbox"].ToString().ToLower() == "true")
                            {
                                fieldHtml.Append("<div class=' grid-stack-item" + colFldGridStackItem + "'  " + gridStackData + " id=\"randomID_" + dc.frameno + randomID + "\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"labelcol inputs checkbox form-group row " + lblBsClass + " " + colDivInputPadding + "  justify-content-start flex-nowrap flex-row-reverse align-items-center mt-7 \" data-dataindex=" + lblBsIndex + " >");
                                fieldHtml.Append("<div class=\"fld-wrap3 flex-shrink-1 ps-0 " + colFldCaption + /*(!columnModeEnabled ? "" : " d-none ")*/ "  " + "\"><label class=\"form-label col-form-label pb-1 fw-boldest\" style=\"cursor: default;\" for=\"" + name + "\">" + fld.caption + "</label></div>");
                            }
                            else
                            {
                                fieldHtml.Append("<div class=' grid-stack-item" + colFldGridStackItem + "'  " + gridStackData + " id=\"randomID_" + dc.frameno + randomID + "\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"labelcol inputs checkbox form-group row " + lblBsClass + " " + colDivInputPadding + " \" data-dataindex=" + lblBsIndex + " >");
                                if (fld.allowempty == false)
                                    fieldHtml.Append("<div class=\"fld-wrap3 d-felx required" + colFldCaption + /*(!columnModeEnabled ? "" : " d-none ")*/ "  " + "\"><label class=\"form-label col-form-label pb-1 fw-boldest\" style=\"cursor: default;\" for=\"" + name + "\">" + fld.caption + "</label></div>");
                                else
                                    fieldHtml.Append("<div class=\"fld-wrap3" + colFldCaption + /*(!columnModeEnabled ? "" : " d-none ")*/ "  " + "\"><label class=\"form-label col-form-label pb-1 fw-boldest\" style=\"cursor: default;\" for=\"" + name + "\">" + fld.caption + "</label></div>");
                            }
                        }
                    }

                    if (isGrid)
                    {
                        if (fld.name.ToLower().StartsWith("axp_gridattach") || (fld.name.StartsWith("dc") && fld.name.ToLower().EndsWith("_image")))
                        {
                            if (!fld.visibility)
                                gridStackData = xywhGridStack(fld);
                            //label normal                   
                            fieldHtml.Append("<div class=\"gridElement form-group grid-stack-item\"  " + gridStackData + "  id=\"dvGrid" + fld.name.Replace(" ", "") + "\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div class=\"grid-stack-item-content\"><label>" + fld.caption + "</label>");
                            gridInSlackGridX += 4;
                            if (gridInSlackGridX % 12 == 0)
                            {
                                gridInSlackGridX = 0;
                                gridInSlackGridY += 1;
                            }
                            if (!fld.allowempty && fld.caption != "")
                            {
                                fieldHtml.Append("<span class=\"allowempty\">*</span>");
                            }

                            //if (!string.IsNullOrEmpty(fld.fieldPurpose))
                            //    fieldHtml.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + fld.fieldPurpose + "\">help_outline</i></span>");

                            if (popImg != "")
                            {
                                fieldHtml.Append("<div>");
                            }
                            else
                            {
                                fieldHtml.Append("<div>");
                            }
                        }
                        else
                        {
                            bool chkBox = false;
                            if (fld.moe.ToUpper() == "SELECT" && fld.moeval != null)
                            {
                                string[] moeValue = fld.moeval.ToString().Split(',');
                                chkBox = IsGridCheckBox(moeValue);
                            }
                            if (!fld.visibility)
                                gridStackData = xywhGridStack(fld);
                            //label normal
                            fieldHtml.Append("<div class=\"gridElement form-group grid-stack-item\"   " + gridStackData + "  id=\"dvGrid" + fld.name.Replace(" ", "") + "\" style=\"\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div class=\"grid-stack-item-content\">");
                            if (fld.allowempty && fld.caption != "")
                                fieldHtml.Append("<div class=\"fld-wrap3\" >");
                            else
                                fieldHtml.Append("<div class=\"fld-wrap3 required\">");
                            if (!chkBox) fieldHtml.Append("<label for=\"" + name + "\">" + fld.caption + "</label>");
                            else fieldHtml.Append("<label for=\"" + name + "\">");
                            gridInSlackGridX += 4;

                            if (gridInSlackGridX % 12 == 0)
                            {
                                gridInSlackGridX = 0;
                                gridInSlackGridY += 1;
                            }
                            //if (!string.IsNullOrEmpty(fld.fieldPurpose))
                            //    fieldHtml.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + fld.fieldPurpose + "\">help_outline</i></span>");

                            fieldHtml.Append("</div>");
                            if (popImg != "")
                            {
                                fieldHtml.Append("<div>");
                            }
                            else
                            {
                                fieldHtml.Append("<div >");
                            }
                        }
                        //label normal
                        gridHiddenHtml.Append("<div class=\"gridElement form-group grid-stack-item\" style=\"\">");
                        if (fld.allowempty && fld.caption != "")
                            gridHiddenHtml.Append("<div class=\"fld-wrap3\" >");
                        else
                            gridHiddenHtml.Append("<div class=\"fld-wrap3 required\">");
                        gridHiddenHtml.Append("<label>" + fld.caption + "</label>");

                        //if (!string.IsNullOrEmpty(fld.fieldPurpose))
                        //    fieldHtml.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + fld.fieldPurpose + "\">help_outline</i></span>");

                        gridHiddenHtml.Append("</div>");
                        gridHiddenHtml.Append("<div>");
                    }
                    else
                    {
                        //if (fld.moe.ToUpper() == "SELECT" && fld.selectmode.ToUpper() == "FROM MASTER" && fld.editcombo == true)
                        //    if (divDirection == "left")
                        //    {
                        //        fieldHtml.Append("<div class=\"picklist input-group\"> ");
                        //    }
                        //    else
                        //    {
                        //        fieldHtml.Append("<span class=\"picklist\"> ");
                        //    }
                        ////else
                        ////    fieldHtml.Append("<span> ");
                    }

                    if (popwidth > 0)
                        width = popwidth.ToString();
                    string addZeroForNumeric = string.Empty;
                    if (isGrid && ShowZeroForNumeric && (fld.datatype.ToUpper() == "NUMERIC"))
                        addZeroForNumeric = " value=0 ";

                    // generate html depends on the mode of entry, like text box, check box, radio button
                    switch (fld.moe.ToUpper())
                    {
                        // accept- simple input type which accepts the user input, mostly except image.
                        case "ACCEPT":
                            if (!isGrid && fld.datatype.ToUpper() != "IMAGE")
                                fieldHtml.Append(colFldWidth);
                            SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                            if ((fld.ctype.ToUpper() == "MEMO" || fld.datatype.ToLower() == "text") && fld.ctype.ToUpper() != "CHECK BOX")
                            {

                                SetCssClass(fld.freadonly, "memotem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (!isGrid)
                                    width = Convert.ToString(Convert.ToInt32(width) - 10);

                                if (isGrid)
                                    gridHiddenHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                                fieldHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));

                            }
                            else if (fld.ctype.ToUpper() == "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "checkbox", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                                fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));

                            }
                            else if (fld.datatype.ToUpper() == "IMAGE")
                            {
                                if (isGrid)
                                    gridHiddenHtml.Append(GetImageHTML(fld, isGrid, name, i, fldTlhw, width, fcwidth));
                                fieldHtml.Append(GetImageHTML(fld, isGrid, name, i, fldTlhw, width, fcwidth));
                            }
                            else if (fld.name.ToUpper().IndexOf("AXPBUTTON") != -1)
                            {
                                if (isGrid)
                                {
                                    gridHiddenHtml.Append(GetAxpBtnHTML(fld, isGrid, name, width));
                                }
                                fieldHtml.Append(GetAxpBtnHTML(fld, isGrid, name, width));
                            }
                            else if (fld.datatype.ToUpper() == "DATE/TIME")
                            {
                                if (isGrid)
                                {
                                    width = (Convert.ToInt32(width) - 5).ToString();
                                    gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));

                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                                else
                                {
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                            }

                            else
                            {
                                if (name.StartsWith("axp_gridattach") || (fld.name.ToLower().StartsWith("dc") && fld.name.ToLower().EndsWith("_image")) || (fld.name.ToLower().StartsWith("axpfile_") && isGrid))
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                    fieldHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                }
                                else if (name.ToLower().StartsWith("axp_uploadfiledsign"))
                                {
                                    if (!isGrid)
                                        fieldHtml.Append(GetFileUploadDSignHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                }

                                else if (name.ToLower().StartsWith("dc" + dcNo + "_referimages"))
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetGridReferHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                    fieldHtml.Append(GetGridReferHTML(fld, isGrid, name, fldNo, fldTlhw, width));

                                }
                                else
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                            }
                            break;
                        case "SELECT":
                            if (!isGrid)
                                fieldHtml.Append(colFldWidth);
                            string[] moeValue;
                            if (!isGrid)
                                width = Convert.ToString(Convert.ToInt32(width) + 4);

                            if (fld.moeval != null)
                                moeValue = fld.moeval.ToString().Split(',');
                            else
                                moeValue = "".Split();

                            if (fld.ctype.ToUpper() == "CHECK LIST" && !isGrid)
                            {
                                SetCssClass(fld.freadonly, "multiFldChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckListHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                                fieldHtml.Append(GetCheckListHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                            }
                            else if (fld.ctype.ToUpper() == "RADIO GROUP" && !isGrid)
                            {
                                SetCssClass(fld.freadonly, "multiFldRdg", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                if (isGrid)
                                    gridHiddenHtml.Append(GetRadioGroupHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                                fieldHtml.Append(GetRadioGroupHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                            }
                            else if (fld.selectmode == string.Empty)
                            {
                                bool chkBox = false;
                                chkBox = IsGridCheckBox(moeValue);
                                SetCssClass(fld.freadonly, "combo", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                {
                                    if (chkBox)
                                    {
                                        SetCssClass(fld.freadonly, "gridChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                        width = Convert.ToString(Convert.ToInt32(width) - 10);
                                        gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                        fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                    }
                                    else
                                    {
                                        if (isGrid)
                                            gridHiddenHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                        fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                    }
                                }
                                else
                                {
                                    fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                }
                            }
                            else if (fld.selectmode.ToUpper() == "FROM MASTER")
                            {
                                SetCssClass(fld.freadonly, "combo", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                if (!fld.editcombo)
                                {
                                    // Adding checkbox to GridDC                                       
                                    bool chkBox = false;
                                    chkBox = IsGridCheckBox(moeValue);

                                    if (isGrid)
                                    {
                                        if (chkBox)
                                        {
                                            SetCssClass(fld.freadonly, "gridChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                            gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                            fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                        }
                                        else
                                        {
                                            gridHiddenHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));

                                            if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                            {
                                                fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                                fromListFlds += "," + fld.name;
                                            }
                                            else
                                                fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                        }
                                    }
                                    else
                                    {
                                        if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                        {
                                            fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                            fromListFlds += "," + fld.name;
                                        }
                                        else
                                            fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                    }
                                }
                                else
                                {
                                    //SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType);
                                    if (isGrid)
                                    {
                                        gridHiddenHtml.Append(GetPickListHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden, true));
                                    }

                                    fieldHtml.Append(GetPickListHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden, false));
                                }
                            }
                            else
                            {
                                SetCssClass(fld.freadonly, "combo", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                bool chkBox = false;
                                chkBox = IsGridCheckBox(moeValue);

                                if (isGrid)
                                {
                                    if (chkBox)
                                    {
                                        SetCssClass(fld.freadonly, "gridChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                        width = Convert.ToString(Convert.ToInt32(width) - 10);

                                        gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                        fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));

                                    }
                                    else
                                    {
                                        if (isGrid)
                                            gridHiddenHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                        if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                        {
                                            fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                            fromListFlds += "," + fld.name;
                                        }
                                        else
                                            fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                    }
                                }
                                else
                                {
                                    if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                    {
                                        fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                        fromListFlds += "," + fld.name;
                                    }
                                    else
                                        fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                }
                            }
                            break;
                        case "AUTOGENERATE":
                            if (!isGrid)
                                fieldHtml.Append(colFldWidth);
                            SetCssClass(fld.freadonly, "auto", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);


                            if (isGrid)
                                gridHiddenHtml.Append(GetAutoGenFieldHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden));
                            fieldHtml.Append(GetAutoGenFieldHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden));

                            break;
                        case "FILL":
                            if (!isGrid)
                                fieldHtml.Append(colFldWidth);

                            if ((fld.ctype.ToUpper() == "MEMO" || fld.datatype.ToLower() == "text") && fld.ctype.ToUpper() != "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "memotem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (!isGrid)
                                    width = Convert.ToString(Convert.ToInt32(width) - 8);

                                if (isGrid)
                                    gridHiddenHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                                fieldHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                            }
                            else if (fld.ctype.ToUpper() == "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "checkbox", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                                fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                            }
                            else
                            {
                                SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (fld.name.ToLower().StartsWith("axpfile_") && isGrid)
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                    fieldHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                }
                                else
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                            }
                            break;
                        case "CALCULATE":
                            if (!isGrid)
                                fieldHtml.Append(colFldWidth);
                            if (fld.ctype.ToUpper() == "CHECK BOX")
                                SetCssClass(fld.freadonly, "checkbox", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                            else if (fld.ctype.ToUpper() == "MEMO" || fld.datatype.ToLower() == "text")
                            {
                                SetCssClass(fld.freadonly, "memotem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (!isGrid)
                                    width = Convert.ToString(Convert.ToInt32(width) - 10);
                            }
                            else
                                SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                            if (fld.ctype.ToUpper() == "CHECK BOX")
                            {
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                            }
                            else if (fld.ctype.ToUpper() == "MEMO" || fld.datatype.ToLower() == "text")
                            {
                                if (isGrid)
                                    gridHiddenHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                                fieldHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                            }
                            else
                            {
                                if (isGrid)
                                    gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                            }
                            break;
                    }

                    if (isGrid)
                    {
                        gridHiddenHtml.Append("</div></div>");

                        fieldHtml.Append("</div></div></div>");

                        if (fld.moe.ToUpper() == "SELECT" || (fld.name.ToUpper().IndexOf("AXPBUTTON") != -1) || (popwidth > 0))
                        {
                            if (width != "")
                            {
                                if (popwidth > 0)
                                    width = fld.cwid;

                                width = (Convert.ToInt32(width) - 4).ToString();
                            }
                        }
                        dummyGridHeadHtml.Append("<TD class=agrow1 align=center ><INPUT type=\"text\" disabled  class=agrowGfrow onfocus=this.blur(); /></TD>");
                    }
                    else
                    {
                        if (fld.ctype.ToUpper() == "CHECK BOX")
                        {


                        }
                        ////fieldHtml.Append(" </span>");
                        //if (fld.moe.ToUpper() == "SELECT" && fld.selectmode.ToUpper() == "FROM MASTER" && fld.editcombo == true)
                        //{
                        //    if (divDirection == "left")
                        //        fieldHtml.Append("</div>");
                        //    else
                        //        fieldHtml.Append("</span>");
                        //}
                        fieldHtml.Append("</div></div>");
                        if (colFldWidth != "" && fld.datatype.ToUpper() != "IMAGE")
                            fieldHtml.Append("</div>");

                        fieldHtml.Append(GetCustLabelHTML(fld.name, fld.fldFrameNo));
                    }

                    if (isLastDcDate)
                        lastDcHeight = lastDcHeight + 75;
                }
            }
        }
        if (!isGrid)
            fieldHtml.Append(GetCustLabelHTML("", dcNo));

        if (isGrid)
        {
            for (int m = 0; m < gridScriptDcNo.Count; m++)
            {
                if (dcNo == Convert.ToInt32(gridScriptDcNo[m].ToString()))
                {
                    gridHeadHtml.Append(gridScriptButtons[m].ToString().Split('♠')[0]);
                    gridHiddenHtml.Append(gridScriptButtons[m].ToString().Split('♠')[1]);
                    fieldHtml.Append(gridScriptButtons[m].ToString().Split('♠')[2]);
                }
            }
        }
        return fieldHtml.ToString();
    }
    //return field color
    //Input HEX or colorname 
    // HEX (001180FF) color (red)
    //if HEX consider only last 6 as code and convert bgr to rgb else replace "cl" with empty and retun color
    public string GetFieldColor(string color)
    {
        string col = "";
        //rgbcolor = fld.fldColor.Substring(fld.fldColor.Length - 6, 6);
        if (color.Any(Char.IsDigit) && color.Length > 6)
        {
            color = color.Substring(color.Length - 6, 6);
            string b = color.Substring(0, 2);
            string g = color.Substring(2, 2);
            string r = color.Substring(4, 2);
            return col = "#" + r + g + b;
        }
        else
        {
            return color.ToLower().Replace("cl", "");
        }
    }

    private void SetCssClass(bool fReadOnly, string type, string fldDataType, string fldName, string bsClass, string fldFieldType, FieldStruct fldStr)
    {
        string dateFunClass = string.Empty;
        string timeFunClass = string.Empty;
        string randomFunClass = string.Empty;
        if (fldDataType.ToLower() == "date/time")
        {
            if (fldName.ToLower().IndexOf("axpdte_".ToLower()) > -1)
            {
                dateFunClass = " dtRoundLast ";
            }
            else if (fldName.ToLower().IndexOf("axpdts_") > -1)
            {
                dateFunClass = " dtRoundStart";
            }
        }

        if (fldName.ToLower().StartsWith("axptm_") || fldName.ToLower().StartsWith("axpdbtm_") || (fldFieldType != string.Empty && fldFieldType.ToLower() == "time"))
        {
            if (fldStr.fldTimeFormat != null && fldStr.fldTimeFormat == "24H Format")
                timeFunClass = "flatpickr-input tstOnlyTime24hours ";
            else
                timeFunClass = "flatpickr-input tstOnlyTime ";
        }

        if (fldName.Contains("axpvalid"))
        {
            if (IsFillGridCall)
            {
                fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem Family  axpvalid\" ";
                fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem Family axpvalid\" ";
            }
            else
            {
                fldCss = " class=\"tem Family  form-control  axpvalid\" ";
                fldHdnCss = " class=\"tem Family  form-control axpvalid\" ";
            }
        }
        else if (type == "tem")
        {
            if (fReadOnly)
            {
                if (fldDataType.ToLower() == "date/time")
                {
                    if (fldFieldType != string.Empty && fldFieldType.ToLower() == "timestamp")
                    {
                        if (IsFillGridCall)
                        {
                            fldCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  Family fldtimestamp " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  Family fldtimestamp " + dateFunClass + bsClass + "\" ";
                        }
                        else
                        {
                            fldCss = " disabled tabindex=\"-1\" class=\"tem flddis form-control  Family fldtimestamp " + bsClass + "\" ";
                            fldHdnCss = " disabled tabindex=\"-1\" class=\"tem flddis form-control  Family fldtimestamp " + bsClass + "\" ";
                        }
                    }
                    else
                    {
                        if (IsFillGridCall)
                        {
                            fldCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  Family  date " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  Family  date " + dateFunClass + bsClass + "\" ";
                        }
                        else
                        {
                            fldCss = " disabled tabindex=\"-1\" class=\"tem flddis form-control  Family  date " + bsClass + "\" ";
                            fldHdnCss = " disabled tabindex=\"-1\" class=\"tem flddis form-control  Family  date " + bsClass + "\" ";
                        }
                    }
                }
                else if (fldDataType.ToLower() == "numeric")
                {
                    if (IsFillGridCall)
                    {
                        fldCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  Family number " + bsClass + "\" ";
                        fldHdnCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  Family number " + bsClass + "\" ";
                    }
                    else
                    {
                        fldCss = " disabled tabindex=\"-1\" class=\"tem flddis form-control  Family number " + bsClass + "\" ";
                        fldHdnCss = " disabled  tabindex=\"-1\" class=\"tem flddis form-control  Family number " + bsClass + "\" ";
                    }
                }
                else
                {
                    if (IsFillGridCall)
                    {
                        fldCss = " readonly tabindex=\"-1\" disabled class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  Family " + timeFunClass + bsClass + "\" ";
                        fldHdnCss = " readonly tabindex=\"-1\" disabled class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  Family " + timeFunClass + bsClass + "\" ";
                    }
                    else
                    {
                        fldCss = " disabled class=\"tem flddis form-control  Family " + timeFunClass + bsClass + "\" ";
                        fldHdnCss = " disabled class=\"tem flddis form-control  Family " + timeFunClass + bsClass + "\" ";
                    }
                }
            }
            else
            {
                if (fldDataType.ToLower() == "date/time")
                {
                    if (fldFieldType != string.Empty && fldFieldType.ToLower() == "timestamp")
                    {
                        if (IsFillGridCall)
                        {
                            fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem Family fldtimestamp " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem Family fldtimestamp " + dateFunClass + bsClass + " \" ";
                        }
                        else
                        {
                            fldCss = " class=\"form-control flatpickr-input fldtimestamp " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " class=\"form-control flatpickr-input fldtimestamp " + dateFunClass + bsClass + " \" ";
                        }
                    }
                    else
                    {
                        if (IsFillGridCall)
                        {
                            fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem Family  date " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem Family  date " + dateFunClass + bsClass + " \" ";
                        }
                        else
                        {
                            fldCss = " class=\"form-control flatpickr-input active " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " class=\"form-control flatpickr-input active " + dateFunClass + bsClass + " \" ";
                        }
                    }
                }
                else if (fldDataType.ToLower() == "numeric")
                {
                    if ((fldFieldType != string.Empty && fldFieldType.ToLower() == "random number"))
                    { randomFunClass = "randomnum "; }

                    if (IsFillGridCall)
                    {

                        fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp number " + randomFunClass + bsClass + "\" ";
                        fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp number " + randomFunClass + bsClass + "\" ";
                    }
                    else
                    {
                        fldCss = " class=\"tem Family form-control  number " + randomFunClass + bsClass + "\" ";
                        fldHdnCss = " class=\"tem Family form-control  number " + randomFunClass + bsClass + "\" ";
                    }
                }
                else
                {
                    if (IsFillGridCall)
                    {
                        fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp " + timeFunClass + bsClass + "\" ";
                        fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp " + timeFunClass + bsClass + "\" ";
                    }
                    else
                    {
                        fldCss = " class=\"tem Family form-control  " + timeFunClass + bsClass + "\" ";
                        fldHdnCss = " class=\"tem Family form-control  " + timeFunClass + bsClass + "\" ";
                    }
                }
            }
        }
        else if (type == "memotem")
        {
            if (fReadOnly)
            {
                if (IsFillGridCall)
                {
                    fldCss = " tabindex=\"-1\" disabled readonly class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp flddis memofam " + bsClass + "\" ";
                    fldHdnCss = " tabindex=\"-1\" disabled readonly class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp flddis memofam " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " disabled class=\"tem flddis Family form-control memofam gridstackCalc " + bsClass + "\" ";
                    fldHdnCss = " disabled class=\"tem flddis Family form-control  memofam " + bsClass + "\" ";
                }
            }
            else
            {
                if (fldDataType.ToLower() == "date/time")
                {
                    if (fldFieldType != string.Empty && fldFieldType.ToLower() == "timestamp")
                    {
                        if (IsFillGridCall)
                        {
                            fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp fldtimestamp memofam " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp fldtimestamp memofam  " + dateFunClass + bsClass + "\" ";
                        }
                        else
                        {
                            fldCss = " class=\"tem Family fldtimestamp memofam  form-control " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " class=\"tem Family fldtimestamp memofam form-control  " + dateFunClass + bsClass + "\" ";
                        }
                    }
                    else
                    {
                        if (IsFillGridCall)
                        {
                            fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp date memofam " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp date memofam  " + dateFunClass + bsClass + "\" ";
                        }
                        else
                        {
                            fldCss = " class=\"tem Family date memofam  form-control " + dateFunClass + bsClass + "\" ";
                            fldHdnCss = " class=\"tem Family date memofam form-control  " + dateFunClass + bsClass + "\" ";
                        }
                    }
                }
                else if (fldDataType.ToLower() == "numeric")
                {
                    if ((fldFieldType != string.Empty && fldFieldType.ToLower() == "random number"))
                    { randomFunClass = "randomnum "; }

                    if (IsFillGridCall)
                    {
                        fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp number memofam  " + randomFunClass + bsClass + "\" ";
                        fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp number memofam  " + randomFunClass + bsClass + "\" ";
                    }
                    else
                    {
                        fldCss = " class=\"tem Family number memofam form-control  " + randomFunClass + bsClass + "\" ";
                        fldHdnCss = " class=\"tem Family number memofam form-control  " + randomFunClass + bsClass + "\" ";
                    }
                }
                else
                {
                    if (IsFillGridCall)
                    {
                        fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp memofam " + timeFunClass + bsClass + "\" ";
                        fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp memofam " + timeFunClass + bsClass + "\" ";
                    }
                    else
                    {
                        fldCss = " class=\"tem Family memofam form-control gridstackCalc " + timeFunClass + bsClass + "\" ";
                        fldHdnCss = " class=\"tem Family memofam form-control  " + timeFunClass + bsClass + "\" ";
                    }
                }
            }
        }
        else if (type.ToLower() == "multifld")
        {
            if (fReadOnly)
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly tabindex=\"-1\" disabled class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFld flddis " + bsClass + "\" ";
                    fldHdnCss = " readonly tabindex=\"-1\" disabled class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFld flddis " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " disabled class=\"multiFld flddis Family" + bsClass + "\" ";
                    fldHdnCss = " disabled class=\"multiFld flddis Family" + bsClass + "\" ";
                }
            }
            else
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFld " + bsClass + "\" ";
                    fldHdnCss = " tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFld " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " readonly class=\"multiFld Family" + bsClass + "\" ";
                    fldHdnCss = " class=\"multiFld Family" + bsClass + "\" ";
                }
            }
        }
        else if (type == "combo")
        {

            if (fReadOnly)
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly disabled tabindex=\"-1\" class=\"w-100 border bg-transparent overflow-hidden resize-none labelInp combotem flddis Family " + bsClass + "\" ";
                    fldHdnCss = " readonly disabled tabindex=\"-1\" class=\"w-100 border bg-transparent overflow-hidden resize-none labelInp combotem flddis Family " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " disabled class=\"combotem flddis Family  form-control  input-sm selectPaddingFix form-select" + bsClass + "\" ";
                    fldHdnCss = " disabled class=\"combotem flddis Family form-control  input-sm selectPaddingFix form-select" + bsClass + "\" ";
                }
            }
            else
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly tabindex=\"-1\" class=\"w-100 border bg-transparent overflow-hidden resize-none labelInp combotem Family " + bsClass + "\" ";
                    fldHdnCss = " readonly tabindex=\"-1\" class=\"w-100 border bg-transparent overflow-hidden resize-none labelInp combotem Family " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " class=\"combotem Family form-control  input-sm selectPaddingFix form-select" + bsClass + "\" ";
                    fldHdnCss = " class=\"combotem Family  form-control  input-sm selectPaddingFix form-select" + bsClass + "\" ";
                }
            }
        }
        else if (type == "gridChk")
        {
            if (fReadOnly)
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp gridchk flddis  " + bsClass + "\" ";
                    fldHdnCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp gridchk flddis   " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " disabled class=\"gridchk flddis Family   " + bsClass + "\" ";
                    fldHdnCss = " disabled class=\"gridchk flddis Family   " + bsClass + "\" ";
                }
            }
            else
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp gridchk " + bsClass + "\" ";
                    fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp gridchk " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " class=\"gridchk Family  " + bsClass + "\" ";
                    fldHdnCss = " class=\"gridchk Family  " + bsClass + "\" ";
                }
            }
        }
        else if (type == "auto")
        {
            if (fReadOnly)
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp autogen flddis " + bsClass + "\" ";
                    fldHdnCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp autogen flddis " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " disabled class=\"autogen flddis Family form-control  " + bsClass + "\" ";
                    fldHdnCss = " disabled class=\"autogen flddis Family form-control  " + bsClass + "\" ";
                }
            }
            else
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp autogen \" ";
                    fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp autogen " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " class=\"autogen Family form-control \" ";
                    fldHdnCss = " class=\"autogen Family form-control  " + bsClass + "\" ";
                }
            }
        }
        else if (type == "multiFldChk")
        {
            if (fReadOnly)
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFldChk flddis " + bsClass + "\" ";
                    fldHdnCss = " readonly disabled tabindex=\"-1\"  class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFldChk flddis " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " disabled class=\"multiFldChk flddis Family   " + bsClass + "\" ";
                    fldHdnCss = " disabled class=\"multiFldChk flddis   " + bsClass + "\" ";
                }
            }
            else
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFldChk Family " + bsClass + "\" ";
                    fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFldChk Family " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " class=\"multiFldChk Family   " + bsClass + "\" ";
                    fldHdnCss = " class=\"multiFldChk Family   " + bsClass + "\" ";
                }
            }
        }
        else if (type == "multiFldRdg")
        {
            if (fReadOnly)
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFldRdg flddis   " + bsClass + "\" ";
                    fldHdnCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFldRdg flddis   " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " disabled class=\"form-control p-0 form-check-input h-30px w-30px opacity-100 multiFldRdg flddis   " + bsClass + "\" ";
                    fldHdnCss = " disabled class=\"form-control p-0 form-check-input h-30px w-30px opacity-100 multiFldRdg flddis   " + bsClass + "\" ";
                }
            }
            else
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFldRdg   " + bsClass + "\" ";
                    fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp multiFldRdg   " + bsClass + "\" ";
                }
                else
                {
                    fldCss = " class=\"form-control p-0 form-check-input h-30px w-30px opacity-100 multiFldRdg   " + bsClass + "\" ";
                    fldHdnCss = " class=\"form-control p-0 form-check-input h-30px w-30px opacity-100 multiFldRdg   " + bsClass + "\" ";
                }
            }
        }
        else if (type == "checkbox")
        {
            if (fReadOnly)
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly disabled tabindex=\"-1\"  class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  " + bsClass + "\" ";
                    fldHdnCss = " readonly disabled tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem flddis  " + bsClass + "\" ";
                }
                else
                {
                    if (HttpContext.Current.Session["switchboxToCheckbox"] != null && HttpContext.Current.Session["switchboxToCheckbox"].ToString().ToLower() == "true")
                    {
                        fldCss = " disabled class=\"form-check-input h-25px w-25px opacity-100 tem flddis custInpChk " + bsClass + "\" ";
                        fldHdnCss = " disabled class=\"form-check-input h-25px w-25px opacity-100 tem flddis custInpChk " + bsClass + "\" ";
                    }
                    else
                    {
                        fldCss = " disabled class=\"form-check-input h-25px w-45px opacity-100 tem flddis custInpChk " + bsClass + "\" ";
                        fldHdnCss = " disabled class=\"form-check-input h-25px w-45px opacity-100 tem flddis custInpChk " + bsClass + "\" ";
                    }
                }
            }
            else
            {
                if (IsFillGridCall)
                {
                    fldCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem    " + bsClass + "\" ";
                    fldHdnCss = " readonly tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  labelInp tem  " + bsClass + "\" ";
                }
                else
                {
                    if (HttpContext.Current.Session["switchboxToCheckbox"] != null && HttpContext.Current.Session["switchboxToCheckbox"].ToString().ToLower() == "true")
                    {
                        fldCss = " class=\"form-check-input h-25px w-25px opacity-100 tem custInpChk   " + bsClass + "\" ";
                        fldHdnCss = " class=\"form-check-input h-25px w-25px opacity-100 tem  l custInpChk " + bsClass + "\" ";
                    }
                    else
                    {
                        fldCss = " class=\"form-check-input h-25px w-45px opacity-100 tem custInpChk   " + bsClass + "\" ";
                        fldHdnCss = " class=\"form-check-input h-25px w-45px opacity-100 tem  l custInpChk " + bsClass + "\" ";
                    }
                }

            }
        }
    }

    //Function to get the html for a memo/multiline/textarea field in the tstruct.
    private string GetMemoHTML(FieldStruct Fld, bool IsGrid, string Name, int fldNo, string[] fldTlhw, string width)
    {
        if (Fld.Value != null && Fld.Value.Contains("<br>"))
        {
            Fld.Value = Fld.Value.Replace("<br>", "\n");
        }

        StringBuilder memoHTML = new StringBuilder();

        string hiddenName = Fld.name + "\" +nrno+\"";
        int memofldheight;

        //string bgColor = "";
        //if (Fld.fldColor != string.Empty)
        //    bgColor = "background-color:" + Fld.fldColor + ";";
        string bgColor = "";
        if (Fld.fldColor != string.Empty)
            bgColor = " background-color:" + GetFieldColor(Fld.fldColor) + "; ";
        //changes for lable colors
        if (Fld.fieldCtrlCss.Split(',').Length >= 4)
        {
            string lblclr = Fld.fieldCtrlCss.Split(',').Last();
            if (lblclr != "")
                bgColor += " color:" + GetFieldColor(lblclr) + "; ";
        }


        if (EnableOldTheme == "false")
        {
            if (!String.IsNullOrEmpty(fldTlhw[2]) && Convert.ToInt32(fldTlhw[2]) > 26)
                memofldheight = (Convert.ToInt32(fldTlhw[2]) - 11);
            else
                memofldheight = 26;
        }
        else
        {

            if (!String.IsNullOrEmpty(fldTlhw[2]) && Convert.ToInt32(fldTlhw[2]) > 14)
                memofldheight = (Convert.ToInt32(fldTlhw[2]) - 11);
            else
                memofldheight = 14;
        }

        if (IsGrid)
        {
            fldCss += " min-h-25px h-30px";
            if (IsFillGridCall)
            {
                if (fldCss.Contains("flddis"))
                    memoHTML.Append("<textarea id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" name=\"" + Name + "\" " + fldCss + " data-style=\"" + bgColor + " \" data-type=\"textarea\" data-hidden=\"\" readonly");
                else
                    memoHTML.Append("<textarea id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" name=\"" + Name + "\" " + fldCss + " data-style=\"" + bgColor + " \" data-type=\"textarea\" data-hidden=\"\" readonly");

                memoHTML.Append(" >" + Fld.Value + "</textarea>");
            }
            else
            {
                memoHTML.Append("<textarea  id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" name=\"" + Name + "\" " + fldCss + " style=\"" + bgColor + " \" ");
                memoHTML.Append(" >" + Fld.Value + "</textarea>");
            }

        }
        else
        {
            memoHTML.Append("<textarea rows='1' id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" name=\"" + Name + "\" " + fldCss + " style=\"" + bgColor + ";\" ");
            memoHTML.Append(" >" + Fld.Value + "</textarea>");
        }

        return memoHTML.ToString();
    }

    //Function to get the html of an auto generated field in the tstruct.
    private string GetAutoGenFieldHTML(FieldStruct fld, bool isGrid, string name, int fldNo, string[] fldTlhw, string width, string popImg, string popHdnImg)
    {
        StringBuilder inputHTML = new StringBuilder();
        string hiddenName = fld.name + "\" +nrno+\"";

        string bgColor = "";
        if (fld.fldColor != string.Empty)
            bgColor = " background-color:" + GetFieldColor(fld.fldColor) + "; ";
        //changes for lable colors
        if (fld.fieldCtrlCss.Split(',').Length >= 4)
        {
            string lblclr = fld.fieldCtrlCss.Split(',').Last();
            if (lblclr != "")
                bgColor += " color:" + GetFieldColor(lblclr) + "; ";
        }

        if (isGrid)
        {
            if (isFillGridCall)
            {
                inputHTML.Append("<textarea type=text data-type=\"input\" id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" data-style=\"" + bgColor + " " + fldHdnCss + "\" >Auto</textarea>" + popHdnImg);
            }
            else
            {
                inputHTML.Append("<INPUT type=text value=Auto id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" style=\"" + bgColor + " " + fldHdnCss + "\" />" + popHdnImg);
            }
        }
        else
        {
            inputHTML.Append("<INPUT type=\"text\" value=\"Auto\" id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" style=\"" + bgColor + "\"" + fldCss + " />" + popImg);
        }
        return inputHTML.ToString();
    }

    //Function to get the html of an input/text/numeric/date field in the tstruct.
    private string GetInputHTML(FieldStruct fld, bool isGrid, string name, int fldNo, string[] fldTlhw, string width, string NumericCss, string popImg, string popHdnImg)
    {
        StringBuilder inputHTML = new StringBuilder();
        string hiddenName = fld.name + "\" +nrno+\"";
        string txtAlign = string.Empty;
        if ((fld.datatype.ToUpper() == "NUMERIC"))
        {
            txtAlign = "text-align:right; ";
        }
        //added calender icon,reducing the text box width for aligment
        if ((fld.datatype.ToUpper() == "DATE/TIME"))
        {
            width = (Convert.ToInt32(width) - 17).ToString();
        }

        string popRightPadding = string.Empty;
        if (popImg != "")
        {
            popRightPadding = " padding-right: 20px !important; ";
        }



        string bgColor = "";
        if (fld.fldColor != string.Empty)
            bgColor = " background-color:" + GetFieldColor(fld.fldColor) + "; ";
        //changes for lable colors
        if (fld.fieldCtrlCss.Split(',').Length >= 4)
        {
            string lblclr = fld.fieldCtrlCss.Split(',').Last();
            if (lblclr != "")
                bgColor += " color:" + GetFieldColor(lblclr) + "; ";
        }

        if (isGrid)
        {
            if (popHdnImg != "")
                inputHTML.Append("<nobr>");

            if (NumericCss != "")
            {
                if (IsFillGridCall)
                {
                    inputHTML.Append("<textarea id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" maxlength=\"" + fld.fldlength + "\" " + fldHdnCss + " data-style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\" style=\"" + txtAlign + "\" ");
                }
                else
                {
                    if ((fld.datatype.ToUpper() == "DATE/TIME") || fld.name.ToLower().StartsWith("axptm_") || fld.name.ToLower().StartsWith("axpdbtm_") || (fld.fldType != string.Empty && fld.fldType.ToLower() == "time"))
                    {
                        inputHTML.Append("<span class=\"input-group\">");
                    }

                    if ((fld.datatype.ToUpper() == "NUMERIC"))
                    {
                        inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + "\" ");
                    }
                    else if (fld.name.ToLower().StartsWith("barqr_"))
                    {
                        inputHTML.Append("<div class=\"divBarQrScan input-group-text p-2 cursor-pointer\"><span class=\"material-icons material-icons-style material-icons-1 barQrIcon\">center_focus_weak</span></div>");
                        inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\" ");
                    }
                    else if (fld.name.ToLower().StartsWith("axp_weight_"))
                    {
                        inputHTML.Append("<div class=\"weightScale\"><span class=\"material-icons weightScaleIcon\" data-weight=\"ws" + name + "\">monitor_weight</span></div>");
                        inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\"  " + (isMobile ? " onfocus=\"blur()\" " : "") + " ");
                    }
                    else
                    {
                        inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\" ");
                    }
                }
            }
            else
            {
                if (IsFillGridCall)
                {
                    inputHTML.Append("<textarea id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" maxlength=\"" + fld.fldlength + "\" " + fldHdnCss + " data-style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\" ");
                }
                else
                {
                    if ((fld.datatype.ToUpper() == "DATE/TIME") || fld.name.ToLower().StartsWith("axptm_") || fld.name.ToLower().StartsWith("axpdbtm_") || (fld.fldType != string.Empty && fld.fldType.ToLower() == "time"))
                    {
                        inputHTML.Append("<span class=\"input-group\">");
                    }
                    if ((fld.datatype.ToUpper() == "NUMERIC"))
                    {
                        inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + "\" ");
                    }
                    else
                    {
                        if (fld.name.ToLower().StartsWith("axpfilepath_"))
                        {
                            var endPart = name.Substring(12);
                            if (!fldHdnCss.Contains("axpFilePathFld"))
                                fldHdnCss = fldHdnCss.Replace("class=\"", "class=\"axpFilePath_" + endPart + " axpFilePathFld ");
                            inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\" ");
                        }
                        else if (fld.fldType != string.Empty && fld.fldType.ToLower() == "table")
                        {
                            //inputHTML.Append("<div class=\"autoinput-parent\"><input id=\"" + name + "\" name=\"" + name + "\"  value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + "\"" + NumericCss + " ");
                            inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\"  value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + "\"" + NumericCss + " ");
                        }
                        else if (fld.name.ToLower().StartsWith("barqr_"))
                        {
                            inputHTML.Append("<div class=\"divBarQrScan input-group-text p-2 cursor-pointer\"><span class=\"material-icons material-icons-style material-icons-1 barQrIcon\">center_focus_weak</span></div>");
                            inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\" ");
                        }
                        else if (fld.name.ToLower().StartsWith("axp_weight_"))
                        {
                            inputHTML.Append("<div class=\"weightScale\"><span class=\"material-icons weightScaleIcon\" data-weight=\"ws" + name + "\">monitor_weight</span></div>");
                            inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\"  " + (isMobile ? " onfocus=\"blur()\" " : "") + " ");
                        }
                        else
                            inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\" ");
                    }
                }
            }


            if (fld.pwdchar != "")
            {
                if (IsFillGridCall)
                {
                    inputHTML.Append(" type=password data-type=\"password\" data-pwdopts=\"" + fld.pwdalphanumeric + "~" + fld.pwdminchar + "~" + fld.pwdnodexpiry + "\"");
                }
                else
                {
                    if (fld.pwdchar.Length > 1)
                        inputHTML.Append(" type=password data-pwdopts=\"" + fld.pwdalphanumeric + "~" + fld.pwdminchar + "~" + fld.pwdnodexpiry + "\"");
                    else if (fld.pwdchar.Length == 1)
                        inputHTML.Append(" type=password data-pwdopts=\"\"");
                }

            }
            else
            {
                if (IsFillGridCall)
                {
                    inputHTML.Append(" type=text");
                }
                else
                {
                    inputHTML.Append(" type=text");
                }

            }

            if (fld.datatype.ToUpper() == "DATE/TIME")
            {
                if (fld.fldType != string.Empty && fld.fldType.ToLower() == "timestamp")
                {
                    if (IsFillGridCall)
                    {
                        inputHTML.Append(fldHdnCss + " title=\"" + defaultDateTimeStr + "\"  " + (isMobile ? " onfocus=\"blur()\" " : "") + "  data-type=\"datetimepicker\">" + fld.Value + "</textarea>" + popHdnImg);
                    }
                    else
                    {
                        inputHTML.Append(fldHdnCss + " title=\"" + defaultDateTimeStr + "\"  " + (isMobile ? " onfocus=\"blur()\" " : "") + " value=\"" + defaultDateTimeStr + "\"/><span class=\"input-group-text spanDateTime\"><span class=\"material-icons material-icons-style cursor-pointer fs-4\">work_history</span></span>" + popHdnImg);
                    }
                }
                else
                {
                    if (IsFillGridCall)
                    {
                        inputHTML.Append(fldHdnCss + " title=\"" + defaultDateStr + "\"  " + (isMobile ? " onfocus=\"blur()\" " : "") + "  data-type=\"datepicker\">" + fld.Value + "</textarea>" + popHdnImg);
                    }
                    else
                    {
                        inputHTML.Append(fldHdnCss + " title=\"" + defaultDateStr + "\"  " + (isMobile ? " onfocus=\"blur()\" " : "") + " value=\"" + defaultDateStr + "\"/><span class=\"input-group-text\" id=\"basic-addon2\" data-toggle><span class=\"material-icons material-icons-style cursor-pointer fs-4\">calendar_today</span></span>" + popHdnImg);
                    }
                }
            }
            else if (fld.name.ToLower().StartsWith("axptm_") || fld.name.ToLower().StartsWith("axpdbtm_") || (fld.fldType != string.Empty && fld.fldType.ToLower() == "time"))
            {
                var classList = fldHdnCss.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                bool hasTstOnlyTime = classList.Contains("tstOnlyTime");
                bool hasTstOnlyTime24 = classList.Contains("tstOnlyTime24hours");
                bool isTstOnlyTime24Only = hasTstOnlyTime24 && !hasTstOnlyTime;
                string is24Hour = "HH:MM AM/PM";
                if (isTstOnlyTime24Only)
                    is24Hour = "HH:MM";

                if (IsFillGridCall)
                {
                    inputHTML.Append(fldHdnCss + " title=\"" + defaultTimeStr + "\" placeholder=\"" + is24Hour + "\" " + (isMobile ? " onfocus=\"blurThis(this);\" " : "") + " data-type=\"input\">" + fld.Value + "</textarea>" + popHdnImg);
                }
                else
                {
                    inputHTML.Append(fldHdnCss + " title=\"" + defaultTimeStr + "\" placeholder=\"" + is24Hour + "\" " + (isMobile ? " onfocus=\"blur()\" " : "") + " value=\"" + defaultTimeStr + "\"/><span class=\"input-group-text\" data-toggle><span class=\"material-icons material-icons-style cursor-pointer fs-4\">schedule</span></span>" + popHdnImg);
                }

            }
            else if (fld.fldType != string.Empty && fld.fldType.ToLower() == "table")
            {
                fldCss = fldCss.Replace("class=\"", "class=\"fldCustTable ");
                if (IsFillGridCall)
                    inputHTML.Append(" " + fldCss + " data-type=\"input\"></textarea>");
                else
                {
                    inputHTML.Append(" " + fldCss + " />");
                    inputHTML.Append("<span class=\"input-group-text\"><span class=\"material-icons material-icons-style cursor-pointer fs-4 fldCustTableIcon\" data-clk=\"" + name + "-tbl\">apps</span></span>");
                }
            }
            else
            {
                if (fld.datatype.ToUpper() == "NUMERIC")
                {
                    if (IsFillGridCall)
                    {
                        inputHTML.Append(" " + fldHdnCss + "  data-type=\"numeric\">" + fld.Value + "</textarea>" + popHdnImg);
                    }
                    else
                    {
                        inputHTML.Append(" " + fldHdnCss + " />" + popHdnImg);
                    }
                }
                else if (fld.pwdchar != "")
                {
                    if (IsFillGridCall)
                    {
                        inputHTML.Append(" " + fldHdnCss + "  data-type=\"input\">" + fld.Value + "</textarea>");
                    }
                    else
                    {
                        inputHTML.Append(" " + fldHdnCss + " />");
                    }
                }
                else
                {
                    if (IsFillGridCall)
                    {
                        inputHTML.Append(" " + fldHdnCss + "  data-type=\"input\">" + fld.Value + "</textarea>" + popHdnImg);
                    }
                    else
                    {
                        inputHTML.Append(" " + fldHdnCss + " />" + popHdnImg);
                    }
                }
            }

            if (popHdnImg != string.Empty)
                inputHTML.Append("</nobr>");
        }
        else
        {
            if (fld.name.ToLower().StartsWith("axpfile_") || fld.name.ToLower().StartsWith("axp_nga_"))
            {
                inputHTML.Append("<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\"  value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\"  class=\"grdAttach handCur grdhandCur\" >");
                inputHTML.Append("<div class=\"form-group form-control p-0 nongrdattch\" id=\"nongrid_" + name + "\"><div id=\"dropzone_" + name + "\" name=\"" + name + "\" class=\"dropzone dropzone-queue min-h-1px border-0 px-3 py-2\"><div class=\"d-flex flex-row-auto dropzone-panel mb-lg-0 m-0\"><a class=\"dropzone-select fs-7 text-truncate me-1--\"><span class=\"material-icons material-icons-style material-icons-2 float-start mx-2--\">attach_file</span> Drop files here or click to upload</a><span class=\"material-icons material-icons-style material-icons-2 ms-auto pe-7 fileuploadmore d-none\" data-bs-toggle=\"popover\" data-bs-sanitize=\"false\" data-bs-placement=\"bottom\" data-bs-html=\"true\">more</span><div class=\"position-relative ms-0 mt-n1 d-none\" id=\"gridAttachCounter\"><span class=\"text-gray-600 spanAttCount fs-5 border border-1 border-gray-300 badge bg-white py-0 px-1 shadow-sm mt-1\"></span><span class=\"bullet bullet-dot bg-success h-6px w-6px position-absolute translate-middle top-0 start-75 animation-blink\"></span></div><a class=\"dropzone-remove-all btn btn-sm btn-light-primary d-none\">Remove All</a></div><div class=\"dropzone-items wm-200px d-none\"><div class=\"dropzone-item\" style=\"display: none\"><div class=\"dropzone-file overflow-hidden\"><div class=\"dropzone-filename\" title=\"some_image_file_name.jpg\"><span data-dz-name>some_image_file_name.jpg</span></div><div class=\"dropzone-error\" data-dz-errormessage></div></div><div class=\"dropzone-progress d-none\"><div class=\"progress\"><div class=\"progress-bar bg-primary\" role=\"progressbar\" aria-valuemin=\"0\" aria-valuemax=\"100\" aria-valuenow=\"0\" data-dz-uploadprogress></div></div></div><div class=\"dropzone-toolbar\"><span class=\"dropzone-delete\" data-dz-remove><span class=\"material-icons material-icons-style material-icons-4 float-end dropzoneItemDelete\">clear</span></span></div></div></div></div></div>");
            }
            else
            {
                if (popImg != "")
                    inputHTML.Append("<nobr>");
                //if (fld.name.ToLower().StartsWith("axp_nga_"))
                //{
                //    inputHTML.Append("<div class=\"divnonattach\"><span id=\"nonGrdAtt_" + name + "\"class=\"focus-input fa fa-paperclip upload-icon nongridAttachIcon \" style=\"\" onclick=\"ShowGridAttachPopUp(this);\"   title = '" + fld.caption + "'></span></div>");
                //}
                //if (fld.name.ToLower().StartsWith("axpfile_"))
                //{
                //    inputHTML.Append("<div class=\"divNonGridAxpFile\"><span id=\"ngAxpFileAtt_" + name + "\"class=\"focus-input fa fa-paperclip upload-icon nongridAttachIcon \" style=\"\" onclick=\"ShowAxpFileAttachPopUp(this);\"   title = '" + fld.caption + "'></span></div>");
                //}
                // bar qr scan field
                if (fld.name.ToLower().StartsWith("barqr_"))
                {
                    inputHTML.Append("<div class=\"divBarQrScan input-group-text p-2 cursor-pointer\"><span class=\"material-icons material-icons-style material-icons-1 barQrIcon\">center_focus_weak</span></div>");
                }

                //if (fld.datatype.ToUpper() == "DATE/TIME" || fld.name.ToLower().StartsWith("axptm_") || fld.name.ToLower().StartsWith("axpdbtm_") || (fld.fldType != string.Empty && fld.fldType.ToLower() == "time"))
                //{
                //    inputHTML.Append("<span class=' input-group '>");
                //}
                if (NumericCss != "")
                    inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\"  value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\"   style=\"" + bgColor + popRightPadding + " " + txtAlign + "\"" + NumericCss + " ");
                else if (fld.name.ToLower().StartsWith("axp_nga_"))
                {
                    inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\"  value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\"  class=\"grdAttach handCur grdhandCur\" ");
                }
                else if (fld.name.ToLower().StartsWith("axpfile_"))
                {
                    inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\"  value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\"  class=\"grdAttach handCur grdhandCur\" ");
                }
                else if (fld.name.ToLower().StartsWith("axpfilepath_"))
                {
                    var endPart = name.Substring(12);
                    fldCss = fldCss.Replace("class=\"", "class=\"axpFilePath_" + endPart + " axpFilePathFld ");// "axpFilePath_" + endPart + " axpFilePathFld " + fldCss;
                    inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\" title=\"" + fld.fieldHint + "\" value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + NumericCss + "\" ");
                }
                else if (fld.fldType != string.Empty && fld.fldType.ToLower() == "table")
                {
                    //inputHTML.Append("<div class=\"autoinput-parent\"><input id=\"" + name + "\" name=\"" + name + "\"  value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + "\"" + NumericCss + " ");
                    inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\"  value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + "\"" + NumericCss + " ");
                }
                else
                    inputHTML.Append("<input id=\"" + name + "\" name=\"" + name + "\"  value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" style=\"" + bgColor + popRightPadding + " " + txtAlign + "\"" + NumericCss + " ");
                if (fld.pwdchar != "" && fld.pwdchar.Length == 1)
                    inputHTML.Append(" type=password data-pwdopts=\"\"");
                else if (fld.pwdchar != "" && fld.pwdchar.Length > 1)
                    inputHTML.Append(" type=password data-pwdopts=\"" + fld.pwdalphanumeric + "~" + fld.pwdminchar + "~" + fld.pwdnodexpiry + "\"");
                else if (HttpContext.Current.Session["project"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True" && fld.datatype.ToUpper() == "NUMERIC")
                    inputHTML.Append(" type=number");
                else
                    inputHTML.Append(" type=text");

                if (fld.datatype.ToUpper() == "DATE/TIME")
                {
                    if (fld.fldType != string.Empty && fld.fldType.ToLower() == "timestamp")
                    {
                        inputHTML.Append(" " + fldCss + "  data-input title=\"" + defaultDateTimeStr + "\"  " + (isMobile ? " onfocus=\"blur()\" " : "") + " />");
                        inputHTML.Append("<span class=\"input-group-text\" data-toggle><span class=\"material-icons material-icons-style cursor-pointer fs-4\">work_history</span></span>");
                    }
                    else
                    {
                        inputHTML.Append(" " + fldCss + "  data-input title=\"" + defaultDateStr + "\"  " + (isMobile ? " onfocus=\"blur()\" " : "") + " />");
                        //inputHTML.Append("<span class=\"input-group-addon spandate \"><i class=\"glyphicon glyphicon-calendar icon-basic-calendar\" title='" + fld.caption + "'></i></span></span>");
                        inputHTML.Append("<span class=\"input-group-text\" id=\"basic-addon2\" data-toggle><span class=\"material-icons material-icons-style cursor-pointer fs-4\">calendar_today</span></span>");
                    }
                }
                else if (fld.name.ToLower().StartsWith("axptm_") || fld.name.ToLower().StartsWith("axpdbtm_") || (fld.fldType != string.Empty && fld.fldType.ToLower() == "time"))
                {
                    var classList = fldCss.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    bool hasTstOnlyTime = classList.Contains("tstOnlyTime");
                    bool hasTstOnlyTime24 = classList.Contains("tstOnlyTime24hours");
                    bool isTstOnlyTime24Only = hasTstOnlyTime24 && !hasTstOnlyTime;
                    string is24Hour = "HH:MM AM/PM";
                    if (isTstOnlyTime24Only)
                        is24Hour = "HH:MM";

                    inputHTML.Append(" " + fldCss + "  data-input title=\"" + defaultTimeStr + "\" placeholder=\"" + is24Hour + "\" " + (isMobile ? " onfocus=\"blur()\" " : "") + " />");
                    //inputHTML.Append("<span class=\"input-group-addon spanTime \"><i class=\"glyphicon glyphicon-time\" title='" + fld.caption + "'></i></span></span>");
                    inputHTML.Append("<span class=\"input-group-text\" data-toggle><span class=\"material-icons material-icons-style cursor-pointer fs-4\">schedule</span></span>");
                }
                else if (fld.fldType != string.Empty && fld.fldType.ToLower() == "table")
                {
                    fldCss = fldCss.Replace("class=\"", "class=\"fldCustTable ");
                    if (IsFillGridCall)
                        inputHTML.Append(" " + fldCss + " ></textarea>");
                    else
                    {
                        inputHTML.Append(" " + fldCss + " />");
                        inputHTML.Append("<span class=\"input-group-text\"><span class=\"material-icons material-icons-style cursor-pointer fs-4 fldCustTableIcon\" data-clk=\"" + name + "-tbl\">apps</span></span>");
                    }
                }
                else if (fld.name.ToLower().StartsWith("axp_weight_"))
                {
                    inputHTML.Append(" " + fldCss + "" + (isMobile ? " onfocus=\"blur()\" " : "") + " />");
                    inputHTML.Append("<div class=\"weightScale\"><span class=\"material-icons weightScaleIcon\" data-weight=\"ws" + name + "\">monitor_weight</span></div>");
                }

                else
                {
                    if (fld.datatype.ToUpper() == "NUMERIC")
                    {
                        inputHTML.Append(" " + fldCss + " />" + popImg);
                    }
                    else if (fld.pwdchar != "")
                    {
                        inputHTML.Append(" " + fldCss + " />");
                    }
                    else
                    {
                        inputHTML.Append(" " + fldCss + " />" + popImg);
                    }
                }
                if (popImg != "")
                    inputHTML.Append("</nobr>");
            }

        }

        //if (!string.IsNullOrEmpty(fld.fieldPurpose)) //murali
        //    inputHTML.Append("<div  id=\"axPurpose" + fld.name + "\"  class=\"clsPrps\"><span class='help-block has-success'>" + fld.fieldPurpose + "</span></div>");

        if (fld.pwdchar != "")
            inputHTML.Append("<span class=\"input-group-text\"><i toggle=#" + name + " class=\"material-icons cursor-pointer fs-4 toggle-password\">visibility_off</i></span>");


        return inputHTML.ToString();
    }

    //Function to get the html of a dropdown/select/combo box field in the tstruct with items.
    private string GetSelectHTML(FieldStruct fld, bool isGrid, string name, int fldNo, string[] fldTlhw, string width, string[] SelectValues, string popImg, string popHdnImg)
    {
        StringBuilder selectHTML = new StringBuilder();
        string hiddenName = fld.name + "\" +nrno+\"";
        string refSelsrctid = string.Empty;


        string IsFillGridCallHdnStyle = String.Empty;
        if (IsFillGridCall)
        {
            IsFillGridCallHdnStyle = " display:none ";
        }

        string bgColor = "";
        if (fld.fldColor != string.Empty)
            bgColor = " background-color:" + GetFieldColor(fld.fldColor) + "; ";
        //changes for lable colors
        if (fld.fieldCtrlCss.Split(',').Length >= 4)
        {
            string lblclr = fld.fieldCtrlCss.Split(',').Last();
            if (lblclr != "")
                bgColor += " color:" + GetFieldColor(lblclr) + "; ";
        }

        if (isPerfCode == "true" && !fld.fieldIsFrmList)
        {
            selectHTML.Append(GetFastAutoComHTML(fld, isGrid, name, fld.freadonly.ToString(), bgColor, false));
            return selectHTML.ToString();
        }

        if (isGrid)
        {
            if (IsFillGridCall)
            {

                selectHTML.Append("<textarea  id=\"txtA~" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" " + fldHdnCss + " " + fld.OnChange + " data-type=\"select\" data-style=\"" + bgColor + " " + "\"  >" + fld.Value + "</textarea>");
            }

            selectHTML.Append("<select  id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" " + fldHdnCss + " " + fld.OnChange + " style=\"" + IsFillGridCallHdnStyle + bgColor + " " + "\"  >");

        }
        else
        {
            //Code to provide add button in dropdown/ Editable combo           
            refSelsrctid = GetSelectRefreshTid(fld.name);
            //Code to provide add button in dropdown/ Editable combo end
            selectHTML.Append("<select id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" " + fld.OnChange + " style=\" " + bgColor + "  \" " + fldCss + "  >");
        }

        selectHTML.Append("<option value=\"\">" + Constants.EMPTYOPTION + "</option>");
        if (!string.IsNullOrEmpty(refSelsrctid))
            selectHTML.Append("<option value=\"createPopup('tstruct.aspx?transid=" + refSelsrctid.Split('~')[0] + "&AxPop=true&AxRefSelect=true&AxRefSelectID=" + fld.name + "&AxSrcSelectID=" + refSelsrctid.Split('~')[1] + "&AxRefType=" + refSelsrctid.Split('~')[2] + "',true)\"> + Add " + fld.caption + "</option>");

        int j = 0;
        if (fld.moeval != null)
        {
            if (fld.autoselect == "True")
            {
                if (SelectValues.Length == 2 && SelectValues[0] == "")
                {
                    fld.Value = SelectValues[1];
                }
            }
            for (j = 0; j <= SelectValues.Length - 1; j++)
            {
                if (SelectValues[j] != string.Empty)
                {
                    if (SelectValues[j] == fld.Value)
                    {

                        selectHTML.Append("<option selected=\"selected\" value=\"" + SelectValues[j] + "\">" + SelectValues[j] + "</option>");

                    }
                    else
                    {

                        selectHTML.Append("<option value=\"" + SelectValues[j] + "\">" + SelectValues[j] + "</option>");


                    }
                }
            }
        }
        if (isGrid)
        {

            selectHTML.Append("</select>" + popHdnImg);

        }
        else
        {
            selectHTML.Append("</select>" + popImg);
        }

        //if (!string.IsNullOrEmpty(fld.fieldPurpose)) //murali
        //    selectHTML.Append("<div class=\"clsPrps\" id=\"axPurpose" + fld.name + "\" ><span class='help-block has-success'>" + fld.fieldPurpose + "</span></div>");
        return selectHTML.ToString();
    }

    //Function to get the html of an empty dropdown/select/combo box field in the tstruct.
    private string GetEmptySelectHTML(FieldStruct fld, bool isGrid, string name, string width, string popImg, string popHdnImg)
    {
        StringBuilder selectHTML = new StringBuilder();
        string hiddenName = fld.name + "\" +nrno+\"";


        string IsFillGridCallHdnStyle = String.Empty;
        if (IsFillGridCall)
        {
            IsFillGridCallHdnStyle = " display:none ";
        }
        string bgColor = "";
        if (fld.fldColor != string.Empty)
            bgColor = "background-color:" + fld.fldColor + ";";

        if (isPerfCode == "true" && !fld.fieldIsFrmList)
        {
            selectHTML.Append(GetFastAutoComHTML(fld, isGrid, name, fld.freadonly.ToString(), bgColor, false));
            return selectHTML.ToString();
        }

        if (isGrid)
        {
            if (IsFillGridCall)
            {
                selectHTML.Append("<textarea  id=\"txtA~" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" data-type=\"select\" data-style=\"" + bgColor + "\" " + fldHdnCss + fld.OnChange + "\" >" + fld.Value + "</textarea>");
            }

            selectHTML.Append("<select  id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" style=\"" + IsFillGridCallHdnStyle + bgColor + "\" " + fldHdnCss + fld.OnChange + " >");

            if (fld.fldComboValues != null && fld.fldComboValues != "")
            {

                selectHTML.Append("<option value=\"\">" + Constants.EMPTYOPTION + "</option>");

                string[] choices = fld.fldComboValues.Split('♣');
                for (int i = 0; i < choices.Length; i++)
                {
                    if (choices[i].ToString() != "")
                    {
                        string[] comboItems = choices[i].ToString().Split('~');
                        string fldValue = string.Empty;
                        string fldIdVal = string.Empty;
                        if (fld.fldIdCols == "1")
                        {
                            fldIdVal = comboItems[0];
                            fldValue = comboItems[1];
                        }
                        else
                            fldValue = comboItems[0];


                        if (fldValue == fld.Value)
                        {

                            selectHTML.Append("<option selected=\"selected\" value='" + fldIdVal + "'>" + fldValue + "</option>");

                        }
                        else
                        {

                            selectHTML.Append("<option value='" + fldIdVal + "'>" + fldValue + "</option>");

                        }
                    }
                }

                selectHTML.Append("</select>" + popHdnImg);

            }
            else
            {

                selectHTML.Append("<option value=\"\">" + Constants.EMPTYOPTION + "</option></select>" + popHdnImg);

            }
        }
        else
        {
            //Code to provide add button in dropdown/ Editable combo
            string refSelsrctid = string.Empty;
            refSelsrctid = GetSelectRefreshTid(fld.name);
            //Code to provide add button in dropdown/ Editable combo end
            selectHTML.Append("<select  id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" style=\"" + bgColor + "  \" " + fldCss + fld.OnChange + " >");
            if (fld.fldComboValues != null && fld.fldComboValues != "")
            {
                selectHTML.Append("<option value=\"\">" + Constants.EMPTYOPTION + "</option>");
                if (!string.IsNullOrEmpty(refSelsrctid))
                    selectHTML.Append("<option value=\"createPopup('tstruct.aspx?transid=" + refSelsrctid.Split('~')[0] + "&AxPop=true&AxRefSelect=true&AxRefSelectID=" + fld.name + "&AxSrcSelectID=" + refSelsrctid.Split('~')[1] + "&AxRefType=" + refSelsrctid.Split('~')[2] + "',true)\"> + Add " + fld.caption + "</option>");

                string[] choices = fld.fldComboValues.Split('♣');
                for (int i = 0; i < choices.Length; i++)
                {
                    if (choices[i].ToString() != "")
                    {
                        string[] comboItems = choices[i].ToString().Split('~');
                        string fldValue = "";
                        string fldIdVal = string.Empty;
                        if (fld.fldIdCols == "1")
                        {
                            fldIdVal = comboItems[0];
                            fldValue = comboItems[1];
                        }
                        else
                            fldValue = comboItems[0];

                        if (fldValue == fld.Value)
                            selectHTML.Append("<option selected=\"selected\" value='" + fldIdVal + "'>" + fldValue + "</option>");
                        else
                            selectHTML.Append("<option value='" + fldIdVal + "'>" + fldValue + "</option>");
                    }
                }
                selectHTML.Append("</select>" + popHdnImg);
            }
            else
            {
                selectHTML.Append("<option value=\"\">" + Constants.EMPTYOPTION + "</option>" + popHdnImg);
                if (!string.IsNullOrEmpty(refSelsrctid))
                    selectHTML.Append("<option value=\"createPopup('tstruct.aspx?transid=" + refSelsrctid.Split('~')[0] + "&AxPop=true&AxRefSelect=true&AxRefSelectID=" + fld.name + "&AxSrcSelectID=" + refSelsrctid.Split('~')[1] + "&AxRefType=" + refSelsrctid.Split('~')[2] + "',true)\"> + Add " + fld.caption + "</option>");
                selectHTML.Append("</select>");
            }
        }
        return selectHTML.ToString();
    }

    private string GetSelectRefreshTid(string fldName)
    {
        string refTid = string.Empty;
        string srcTid = string.Empty;
        string tidType = "no";
        //srctid(transId)^srcFieldname-fieldname1,idcol{yes/no}~srctid(transId)-fieldname2,idcol{yes/no}
        if (!string.IsNullOrEmpty(refreshSelect))
        {
            try
            {
                string[] strRefSel = refreshSelect.Split('~');
                for (int i = 0; i < strRefSel.Length; i++)
                {
                    if (strRefSel[i].ToString() != "")
                    {
                        string fldRefSel = strRefSel[i].Split('-')[1];
                        if (strRefSel[i].Split('-')[0].Contains("^"))
                        {
                            refTid = strRefSel[i].Split('-')[0].Split('^')[0];
                            srcTid = strRefSel[i].Split('-')[0].Split('^')[1];

                            if (fldRefSel.Split(',')[0] == fldName)
                            {
                                tidType = fldRefSel.Split(',')[1];
                                return refTid + "~" + srcTid + "~" + tidType;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        return string.Empty;
    }

    //Function to get the html of a picklist field in the tstruct.
    private string GetPickListHTML(FieldStruct fld, bool isGrid, string name, int fldNo, string[] fldTlhw, string width, string popImg, string popHdnImg, Boolean gridHidden)
    {
        StringBuilder pickListHTML = new StringBuilder();
        string hiddenName = fld.name + "\" +nrno+\"";
        string rowFrmNo = name.Substring(name.LastIndexOf("F") - 3);
        string fldReadOnly = "";
        if (fld.freadonly)
        {
            if (IsFillGridCall)
            {
                fldReadOnly = "readonly";
                fldHdnCss = " disabled readonly tabindex=\"-1\" class=\"w-100 border bg-transparent overflow-hidden resize-none labelInp combotem flddis Family \"";
                fldCss = " disabled readonly tabindex=\"-1\" class=\"w-100 border bg-transparent overflow-hidden resize-none labelInp combotem flddis Family \"";
            }
            else
            {
                fldReadOnly = "readonly=\"readonly\"";
                fldHdnCss = "disabled class=\"tem pickdis inputClass2 pixelwidth form-control form-select\"";
                fldCss = " disabled class=\"tem pickdis inputClass2 pixelwidth form-control form-select\"";
            }
        }
        else
        {
            if (IsFillGridCall)
            {
                fldHdnCss = "readonly tabindex=\"-1\" class=\"w-100 border bg-transparent overflow-hidden resize-none labelInp combotem Family \"";
                fldCss = " readonly tabindex=\"-1\" class=\"w-100 border bg-transparent overflow-hidden resize-none labelInp combotem Family \"";
            }
            else
            {
                fldHdnCss = "class=\"tem inputClass2 pixelwidth form-control form-select\"";
                fldCss = "class=\"tem inputClass2 pixelwidth form-control form-select\"";
            }
        }

        string bgColor = "";
        if (fld.fldColor != string.Empty)
            bgColor = "background-color:" + fld.fldColor + ";";
        if (isPerfCode == "true")
        {
            pickListHTML.Append(GetFastAutoComHTML(fld, isGrid, name, fldReadOnly, bgColor, true));
            return pickListHTML.ToString();
        }

        if (gridHidden)
        {
            pickListHTML.Append("<nobr><span class=\"picklist\"><INPUT type=text id=\"" + name + "\" tilte=\"" + fld.fieldHint + "\" name=\"" + name + "\" " + fldReadOnly + " value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" " + fldHdnCss + " style=\"" + bgColor + "\"  ");
            pickListHTML.Append(" /> <a><img alt=search src=../AxpImages/search-back.jpg title='' id=img~" + name + "  class=\"handCur pickimg\" /></a><input type=hidden id=pickIdVal_" + name + " value=\"\" /></span>");
            pickListHTML.Append(popHdnImg + "</nobr>");
        }
        else
        {
            if (isGrid)
            {
                if (IsFillGridCall)
                {
                    pickListHTML.Append("<textarea type=\"text\" id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" " + fldReadOnly + " maxlength=\"" + fld.fldlength + "\" " + fldCss + " data-type=\"picklist\" data-hidden=\"\" data-style=\"" + bgColor + " \" ");
                    pickListHTML.Append(" >" + fld.Value + "</textarea>");
                }
                else
                {
                    pickListHTML.Append("<nobr><span class=\"picklist input-group \"><INPUT type=\"text\" id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" " + fldReadOnly + " value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" " + fldCss + " style=\"" + bgColor + " \" ");
                    pickListHTML.Append(" /> <span class=\"input-group-addon handCur pickimg \" id=\"img~" + name + "\"><i class=\"glyphicon glyphicon-search icon-basic-magnifier\" title=\"Search " + fld.caption + "\" ></i></span><div class=\"picklisttxtclear icon-arrows-remove\" title=\"Clear Text\"></div><input type=hidden id=\"pickIdVal_" + name + "\" value=\"\" /></span>");
                    pickListHTML.Append(popImg + "</nobr>");
                }
            }
            else
            {
                pickListHTML.Append("<INPUT type=\"text\" id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" " + fldReadOnly + " value=\"" + fld.Value + "\" maxlength=\"" + fld.fldlength + "\" " + fldCss + " style=\"" + bgColor + "  \" />");
                pickListHTML.Append("<span class=\"input-group-addon handCur pickimg nongridpick\" id=\"img~" + name + "\"><i class=\"glyphicon glyphicon-search icon-basic-magnifier\" title=\"Search " + fld.caption + "\"></i></span><div class=\"picklisttxtclear icon-arrows-remove\" title=\"Clear Text\"></div><input type=hidden id=\"pickIdVal_" + name + "\" value=\"\" />");
                pickListHTML.Append(popImg);
            }
        }

        return pickListHTML.ToString();
    }

    private string GetCheckListHTML(FieldStruct fld, bool isGrid, string name, int fldNo, string[] fldTlhw, string width, string[] listValues)
    {
        StringBuilder chkLstHTML = new StringBuilder();
        string hiddenName = fld.name + "\" +nrno+\"";
        bool IsChkSql = false;

        string align = string.Empty;
        //fld.type - Immaterial of the alignment for checklist, in web only vertical checklis is supported.
        align = "<div class='clear'></div>";

        string bgColor = "";
        if (fld.fldColor != string.Empty)
            bgColor = " background-color:" + GetFieldColor(fld.fldColor) + "; ";
        //changes for lable colors
        if (fld.fieldCtrlCss.Split(',').Length >= 4)
        {
            string lblclr = fld.fieldCtrlCss.Split(',').Last();
            if (lblclr != "")
                bgColor += " color:" + GetFieldColor(lblclr) + "; ";
        }

        string selStr = "";
        ArrayList arrValues = new ArrayList();
        char separator;
        if (fld.fieldValSeparator == "")
            separator = ',';
        else
            separator = Convert.ToChar(fld.fieldValSeparator);
        if (fld.Value != null)
        {


            string[] SelValues = fld.Value.Split(separator);
            for (int i = 0; i < SelValues.Length; i++)
            {
                arrValues.Add(SelValues[i].ToString());
            }
        }

        if (fld.isFieldSql.ToLower() == "true")
        {
            IsChkSql = true;
            listValues = new string[] { };
            if (fld.fldComboValues != null)
                listValues = fld.fldComboValues.Split('♣');
        }

        string sqlCss = string.Empty;
        if (IsChkSql)
        {
            if (fld.freadonly)
                sqlCss = "disabled class=\"multiChkSpan flddis\"";
            else
                sqlCss = "class=multiChkSpan";
        }
        else
        {
            sqlCss = fldCss;
            sqlCss = sqlCss.Replace("multiFldChk", "multiFldChk chkListBdr row ");
        }

        bool isGridField = false;
        isGridField = IsDcGrid(fld.fldframeno);

        string chkStyle = string.Empty;
        if (listValues.Length == 0 || fld.freadonly)
            chkStyle = " disabled=disabled";

        string chkAllClass = string.Empty;
        if (fld.freadonly)
            chkAllClass = "class=\"chkAllList flddis\"";
        else
            chkAllClass = "class=\"chkAllList\"";

        if (isGridField)
        {

            chkLstHTML.Append("<div name=\"" + "spnChkAll_" + name + "\" class=\"spnSelectAll form-control \" ><input type='checkbox' id=\"chkAll_" + name + "\" onclick=\"CheckAllListItems(this);\" " + chkAllClass + " " + chkStyle + "/>Select All <a href=\"javascript:void(0)\" class='achklist'><span class=\"glyphicon glyphicon-sort icon-arrows-switch-vertical pull-right\"></span></a></div>");

            chkLstHTML.Append("<div   name=\"" + name + "\" " + sqlCss + ">");
            chkLstHTML.Append("<span> <input type=\"checkbox\" id=\"hideAll_" + name + "\" onclick=\"ShowSelectedChkItems(this.id);\" class=\"chkAllList chkShwSel\">Selected </span><span>  <img id=\"" + name + "\" src=\"../AxpImages/multiSelectsearch.jpg\" alt=\"MultiSelectSearch\" onclick=\"OpenMulSelSrchPop(this)\"> </span><br> ");

        }
        else
        {

            //chkLstHTML.Append("<div class=\"row\" style=\"border:1px solid #ccc \"><span  name=\"" + "spnChkAll_" + name + "\" class=\"spnSelectAll pull-left col-md-3\"  ><input type='checkbox' id=\"chkAll_" + name + "\" onclick=\"CheckAllListItems(this);\" " + chkAllClass + " " + chkStyle + "/>Select All</span><span class=\"col-md-3\"></span><span class=\" col-md-3\" style=\"text-align:right\"><img id = 'img_" + name + "' src='../AxpImages/multiSelectsearch.jpg' alt='MultiSelectSearch' onClick='OpenMulSelSrchPop(this)'/></span><span  name=\"" + "spnChkAll_" + name + "\" class=\"spnSelectAll pull-left col-md-3\"  ><input type='checkbox' id=\"hideAll_" + name + "\" onclick=\"ShowSelectedChkItems(this.id);\" class=\"chkAllList chkShwSel\" />Selected</span>");





            //chkLstHTML.Append("<div class=\"row gridstackCalc\" style=\"border:1px solid #ccc \"><span  name=\"" + "spnChkAll_" + name + "\" style=\"" + bgColor + "\"  class=\"spnSelectAll pull-left col-md-12\"  ><input type='checkbox' id=\"chkAll_" + name + "\" onclick=\"CheckAllListItems(this);\" " + chkAllClass + " " + chkStyle + "/>Select All</span>");
            //chkLstHTML.Append("<span   name=\"" + name + "\" class=\"multiChkSpan gridstackCalcChecklist col-lg-12 col-md-12 col-sm-12 col-xs-12 \" style='overflow-y:auto;  border-top:solid 1px #bbb;" + bgColor + " '  >");

            //chkLstHTML.Append("<div class=\"row gridstackCalc\" style=\"border:1px solid #ccc \"><span  name=\"" + "spnChkAll_" + name + "\" style=\"" + bgColor + "\"  class=\"spnSelectAll pull-left col-md-12\"  ><input type='checkbox' id=\"chkAll_" + name + "\" onclick=\"CheckAllListItems(this);\" " + chkAllClass + " " + chkStyle + "/>Select All</span>");
            //chkLstHTML.Append("<span   name=\"" + name + "\" class=\"multiChkSpan gridstackCalcChecklist col-lg-12 col-md-12 col-sm-12 col-xs-12 \" style='overflow-y:auto;  border-top:solid 1px #bbb;" + bgColor + " '  >");


        }

        if (listValues.Length > 0 && !IsDesignMode)
        {
            if (fld.freadonly)
                //if (isGridField)
                fldCss = fldCss + " disabled";
            //else
            //    fldCss = fldCss + "";
            int k = 0;
            for (k = 0; k <= listValues.Length - 1; k++)
            {
                if (listValues[k] != string.Empty)
                {
                    if (arrValues.IndexOf(listValues[k]) != -1)
                        if (isGridField)
                            selStr = " checked=\"checked\"";
                        //else
                        //    selStr += listValues[k] + "♣|♣";
                        else
                            selStr = string.Empty;
                    if (isGridField)
                    {
                        chkLstHTML.Append(align + "<span>");
                        chkLstHTML.Append("<input type=checkbox " + fldCss + " id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" value=\"" + listValues[k] + "\"" + selStr + " /> " + listValues[k] + " </span>");
                    }
                }
            }
        }
        else
        {
            if (isGridField)
            {
                chkLstHTML.Append("<input type=checkbox " + fldCss + " id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\"  disabled=disabled/> ");
            }
        }

        if (isGridField)
            chkLstHTML.Append("</div>");
        else
        {
            string cklreadyOnly = "";
            if (fld.freadonly)
                cklreadyOnly = " disabled ";
            string[] selectedValuesArray = arrValues.ToArray(typeof(string)) as string[];
            if (fld.isFieldSql.ToLower() == "true")
            {
                string refreshChlist = "";
                if (fld.refreshOnSave != "" && fld.refreshOnSave == "True")
                    refreshChlist = " isrefreshsave ";
                chkLstHTML.Append("<select class=\"form-select multiFldChk " + refreshChlist + "\" " + cklreadyOnly + " multiple data-control=\"select2\" id=\"" + name + "\" name=\"" + name + "\" data-placeholder=\"Select an option\" data-allow-clear=\"true\" data-valuelist=\"" + string.Join(separator.ToString(), listValues.Where(x => !string.IsNullOrEmpty(x)).ToArray()) + "\"  data-selectedlist=\"" + string.Join(separator.ToString(), selectedValuesArray.Where(x => !string.IsNullOrEmpty(x)).ToArray()) + "\"  data-separator=\"" + separator.ToString() + "\"></select>");
            }
            else
                chkLstHTML.Append("<select class=\"form-select multiFldChklist\" " + cklreadyOnly + " multiple data-control=\"select2\" id=\"" + name + "\" name=\"" + name + "\" data-placeholder=\"Select an option\" data-allow-clear=\"true\" data-valuelist=\"" + string.Join(separator.ToString(), listValues.Where(x => !string.IsNullOrEmpty(x)).ToArray()) + "\"  data-selectedlist=\"" + string.Join(separator.ToString(), selectedValuesArray.Where(x => !string.IsNullOrEmpty(x)).ToArray()) + "\"  data-separator=\"" + separator.ToString() + "\"></select>");
            chkLstHTML.Append("");
        }
        return chkLstHTML.ToString();
    }

    //Function to get the html of a radio group list field in the tstruct.
    private string GetRadioGroupHTML(FieldStruct fld, bool isGrid, string name, int fldNo, string[] fldTlhw, string width, string[] groupValues)
    {
        StringBuilder radioGrpHTML = new StringBuilder();
        string hiddenName = fld.name + "\" +nrno+\"";
        string spnclass = (fld.type.ToUpper() == "V") ? "radio rdinput radiovert d-inline-flex col-12 py-2" : "radio rdinput radiohori d-flex";
        string bgColor = "";
        if (fld.fldColor != string.Empty)
            bgColor = " background-color:" + GetFieldColor(fld.fldColor) + "; ";
        //changes for lable colors
        if (fld.fieldCtrlCss.Split(',').Length >= 4)
        {
            string lblclr = fld.fieldCtrlCss.Split(',').Last();
            if (lblclr != "")
                bgColor += " color:" + GetFieldColor(lblclr) + "; ";
        }
        string align = string.Empty;
        if (fld.type.ToUpper() == "V")
            align = "<div class='clear'></div>";
        else
            align = "&nbsp;&nbsp;&nbsp;&nbsp;";

        bool isRadioSql = false;
        if (fld.isFieldSql.ToLower() == "true")
        {
            isRadioSql = true;
            groupValues = null;
            if (fld.fldComboValues != null)
                groupValues = fld.fldComboValues.Split('♣');
            fld.moeval = "";
        }

        if (isGrid)
        {
            radioGrpHTML.Append(" <div class=\"panel-group\"><div class=\"panel panel-default\"><div class=\"panel-heading\"><a data-toggle=\"collapse\" href=\"#collapse1\">Collapsible panel</a></div><div id=\"collapse1\" class=\"panel-collapse collapse\">");
        }
        string selStr = string.Empty;
        if (groupValues != null)
        {
            int k = 0;
            if (fld.moeval != null)
            {
                for (k = 0; k <= groupValues.Length - 1; k++)
                {
                    if (groupValues[k] != "")
                    {
                        if (fld.Value == groupValues[k])
                            selStr = "checked=\"checked\"";
                        else
                            selStr = "";
                        if (fld.isFieldSql.ToLower() != "true")
                        {
                            radioGrpHTML.Append("<span class=\"" + spnclass + "\"><input type=radio " + fldCss + " id=\"" + name + "\" title=\"" + fld.fieldHint + "\" name=\"" + name + "\" value=\"" + groupValues[k] + "\"" + selStr + " ><span style=\"" + bgColor + "\" class=\"d-flex align-content-center flex-wrap px-2\"> " + groupValues[k] + "</span></span>");
                        }
                        else
                        {
                            radioGrpHTML.Append("<span class=\"" + spnclass + "\"><input data-dummyRadio=true type=radio " + fldCss + " id=\"" + name + "\" title=\"" + fld.fieldHint + "\" style=\"" + bgColor + "\" name=\"" + name + "\"  disabled=disabled /></span>");
                        }
                    }
                }
                if (fld.isFieldSql.ToLower() == "true")
                {
                    radioGrpHTML.Append("<br>");
                }
            }
        }
        else
        {
            radioGrpHTML.Append("<span class=\"" + spnclass + "\"><input data-dummyRadio=true type=radio " + fldCss + " id=\"" + name + "\" title=\"" + fld.fieldHint + "\" style=\"" + bgColor + "\" name=\"" + name + "\"  disabled=disabled /></span>");
        }
        //radioGrpHTML.Append("</div>");
        if (isGrid)
            radioGrpHTML.Append("</div></div></div>");


        return radioGrpHTML.ToString();
    }

    //Function to get the html of a checkBox field in the tstruct.
    private string GetCheckBoxHTML(FieldStruct Fld, bool IsGrid, string Name, int fldNo, string[] fldTlhw, string width, bool isHidden, string fldMoeVal)
    {
        StringBuilder chkHTML = new StringBuilder();
        string hiddenName = Fld.name + "\" +nrno+\"";
        string chk = ""; if (Fld.Value == null) Fld.Value = "";
        string[] fldMoeValArr = new String[0];
        string bgColor = "";
        if (Fld.fldColor != string.Empty)
            bgColor = " background-color:" + GetFieldColor(Fld.fldColor) + "; ";
        //changes for lable colors
        if (Fld.fieldCtrlCss.Split(',').Length >= 4)
        {
            string lblclr = Fld.fieldCtrlCss.Split(',').Last();
            if (lblclr != "")
                bgColor += " color:" + GetFieldColor(lblclr) + "; ";
        }
        if (IsFillGridCall)
        {
            fldMoeValArr = fldMoeVal.ToString().Split(new[] { "," }, StringSplitOptions.None);
            fldMoeValArr = fldMoeValArr.Where(val => val != "").ToArray();

        }
        if (Fld.Value.ToLower() == "t" || Fld.Value.ToLower() == "yes")
        {
            if (IsFillGridCall)
            {
                if (fldMoeValArr.Length == 2)
                {
                    chk = fldMoeValArr[0];
                }
                else
                {
                    chk = "Yes";
                }
            }
            else
            {
                chk = "checked";
            }
        }
        else
        {
            if (IsFillGridCall)
            {
                if (fldMoeValArr.Length == 2)
                {
                    chk = fldMoeValArr[1];
                }
                else
                {
                    chk = "No";
                }
            }
            else
            {
                chk = "";
            }
        }

        if (isHidden)
        {
            chkHTML.Append("<INPUT type=checkbox  id=\"" + Name + "\" name=\"" + Name + "\" " + chk + " " + fldHdnCss + " alt=\"" + fldMoeVal + "\">");
            if (!IsGrid)
                chkHTML.Append(" <font class=labelcap>" + Fld.caption + "</font>");
        }
        else
        {
            if (IsGrid)
            {
                if (IsFillGridCall)
                {
                    //chkHTML.Append("<textarea data-type=\"checkbox\" id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" name=\"" + Name + "\" " + chk + " " + fldCss + " data-style=\"vertical-align:middle;\" data-hidden=\"" + fldMoeVal + "\">" + chk + "</textarea>");
                    fldCss = fldCss.Replace(" gridchk ", " gridchk d-none ");
                    chkHTML.Append("<textarea data-type=\"checkbox\" id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" name=\"" + Name + "\" " + chk + " " + fldCss + " data-style=\"\" data-hidden=\"" + fldMoeVal + "\">" + chk + "</textarea>");
                    chkHTML.Append("<div id=\"dvgrdchkbox" + Name + "\" class=\"form-check form-switch form-check-custom px-1 align-self-end py-2\"><input title=\"\" value=\"" + chk + "\" class=\"form-check-input opacity-100 Family h-25px w-45px dvgrdchkboxnonedit\" type=\"checkbox\"");
                    if (chk != "" && (chk.ToLower() == "yes" || chk.ToLower() == "t"))
                        chkHTML.Append(" checked=\"checked\" ");
                    chkHTML.Append(" ></input></div>");
                }
                else
                {
                    if (fldMoeVal != "" && fldMoeVal.ToLower().Contains("no,yes"))
                    {
                        string chGrdfldMoeVal = "";
                        foreach (var fcGrdVal in fldMoeVal.Split(',').Reverse())
                        {
                            chGrdfldMoeVal += fcGrdVal + ",";
                        }
                        if (chGrdfldMoeVal != "")
                            fldMoeVal = "," + chGrdfldMoeVal.TrimEnd(',');
                    }
                    //chkHTML.Append("<INPUT type=\"checkbox\" id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" name=\"" + Name + "\" " + chk + " " + fldCss + " style=\"vertical-align:middle;\"></div></div>");
                    chkHTML.Append("<INPUT type=\"checkbox\" id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\"  style=\"" + bgColor + "\" name=\"" + Name + "\" " + chk + " " + fldCss + " alt=\"" + fldMoeVal + "\">");
                    string checkBoxClass = "gridChkSpan checkBoxSpn customGridchkbox";
                    if (Fld.allowempty)
                        checkBoxClass += " noempty";
                    if (string.IsNullOrEmpty(Fld.fieldPurpose))
                        checkBoxClass += " nopurpose";

                    chkHTML.Append("<span class='" + checkBoxClass + "'>" + Fld.caption);

                    //if (!string.IsNullOrEmpty(Fld.fieldPurpose))
                    //    chkHTML.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + Fld.fieldPurpose + "\">help_outline</i></span>");
                    chkHTML.Append("</span></label>");
                }
            }
            else
            {
                string ckreadyOnly = "";
                if (Fld.freadonly)
                    ckreadyOnly = "form-check-solid";
                if (HttpContext.Current.Session["switchboxToCheckbox"] != null && HttpContext.Current.Session["switchboxToCheckbox"].ToString().ToLower() == "true")
                    chkHTML.Append("<div class=\"form-check form-switch-- form-check-custom-- px-1-- align-self-end--- " + ckreadyOnly + "\">");
                else
                    chkHTML.Append("<div class=\"form-check form-switch form-check-custom px-1 align-self-end--- " + ckreadyOnly + "\">");
                chkHTML.Append("<INPUT type=\"checkbox\" id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" style=\"" + bgColor + "\" name=\"" + Name + "\" " + chk + " " + fldCss + "/>");
                string checkBoxClass = "form-check-label form-label col-form-label pb-1 fw-boldest opacity-100";// "checkBoxSpn customchkbox";
                if (Fld.allowempty)
                    checkBoxClass += " noempty";
                if (string.IsNullOrEmpty(Fld.fieldPurpose))
                    checkBoxClass += " nopurpose";
                //chkHTML.Append("<span class='" + checkBoxClass + "' style='" + bgColor + "' >" + Fld.caption);

                // bool columnModeEnabled = axdesignJObject.dcLayout != null && axdesignJObject.dcLayout != "" && axdesignJObject.dcLayout != "default";

                chkHTML.Append("<span class='" + checkBoxClass + " " + /*(!columnModeEnabled ? " d-none " : "")*/ " d-none " + " '>" + Fld.caption);
                //if (!string.IsNullOrEmpty(Fld.fieldPurpose))
                //    chkHTML.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + Fld.fieldPurpose + "\">help_outline</i></span>");
                chkHTML.Append("</span></div>");
            }
        }

        return chkHTML.ToString();
    }

    private string GetGridAttachHTML(FieldStruct Fld, bool IsGrid, string Name, int fldNo, string[] fldTlhw, string width)
    {
        string rowFrmNo = Name.Substring(Name.LastIndexOf("F") - 3);
        //Name  - axp_gridattach_2001F2
        string imgId = "grdAtt_img_" + rowFrmNo;
        string hlnkId = "grdAtt_hlnk_" + rowFrmNo;
        string divId = "GridAttach" + rowFrmNo;
        string divAxpFileId = "GridAxpFile" + Name;
        string axpFilehlnkId = "GridAxpFile_hlnk_" + rowFrmNo;

        StringBuilder grdAttHTML = new StringBuilder();
        StringBuilder grdLinks = new StringBuilder();
        StringBuilder grdLinksInline = new StringBuilder();

        string hlnkSrc = string.Empty;
        string strClass = "class=\"grdAttach handCur\"";
        bool hasMultipleFiles = false;
        if (Fld.freadonly && !Name.ToLower().StartsWith("axpfile_"))
            strClass = "";

        if (!string.IsNullOrEmpty(Fld.Value))
        {
            Util.Util utilObj = new Util.Util();
            hlnkSrc = utilObj.ScriptsurlPath + "axpert//" + HttpContext.Current.Session["nsessionid"].ToString() + "//";
            if (Fld.Value.Contains(","))
            {
                hasMultipleFiles = true;
                string[] fldValues = Fld.Value.Split(',');
                for (int indx = 0; indx < fldValues.Length; indx++)
                {

                    if (IsFillGridCall)
                    {
                        grdLinksInline.Append("<div id=\"Link_" + rowFrmNo + "_" + fldValues[indx].ToString().Replace(' ', '♠').Replace('(', '♦').Replace(')', '♣') + "\"  class='atchfile inlinediv'>");
                        grdLinksInline.Append("<a onclick=\"DeleteFileFromRow('" + Name + "','" + rowFrmNo + "','" + fldValues[indx].ToString() + "')\"><i class=\"glyphicon glyphicon-remove close icon-arrows-remove attachmentcrossicon\"></i></a>");

                        if (!Name.ToLower().StartsWith("axpfile_"))
                            grdLinksInline.Append("<a href=\"javascript:void(0)\" id='grdAtt_hlnk_" + rowFrmNo + "' class='grdAttach handCur' onclick='ShowGridAttLink(\"" + hlnkSrc + fldValues[indx].ToString() + "\")'>" + fldValues[indx].ToString() + "</a>");
                        else
                            grdLinksInline.Append("<a href=\"javascript:void(0)\" id='GridAxpFile_hlnk_" + rowFrmNo + "' class='grdAttach handCur' onclick='ShowAxpFileuploadLink(\"" + Name + "\",\"" + fldValues[indx].ToString() + "\",\"" + hlnkSrc + fldValues[indx].ToString() + "\")'>" + fldValues[indx].ToString() + "</a>");
                        grdLinksInline.Append("</div>");
                    }
                    else
                    {
                        grdLinks.Append("<div id=\"Link_" + rowFrmNo + "_" + fldValues[indx].ToString().Replace(' ', '♠').Replace('(', '♦').Replace(')', '♣') + "\"><a href=\"javascript:void(0)\" onclick=\"DeleteFileFromRow('" + Name + "','" + rowFrmNo + "','" + fldValues[indx].ToString() + "')\"><i class=\"glyphicon glyphicon-remove close icon-arrows-remove attachmentcrossicon\"></i></a>");
                        if (!Name.ToLower().StartsWith("axpfile_"))
                            grdLinks.Append("<a href=\"javascript:void(0)\" id=\"" + hlnkId + "\" tabindex=\"0\" " + strClass + " onclick=\"ShowGridAttLink('" + hlnkSrc + fldValues[indx].ToString() + "')\">" + fldValues[indx].ToString() + "</a>");
                        else
                            grdLinks.Append("<a href=\"javascript:void(0)\" id=\"" + axpFilehlnkId + "\" tabindex=\"0\" " + strClass + " onclick=\"ShowAxpFileuploadLink(\"" + Name + "\",\"" + fldValues[indx].ToString() + "\",'" + hlnkSrc + fldValues[indx].ToString() + "')\">" + fldValues[indx].ToString() + "</a>");
                        grdLinks.Append("</div>");
                    }

                }
            }
            else
            {
                if (IsFillGridCall)
                {
                    grdLinksInline.Append("<div id=\"Link_" + rowFrmNo + "_" + Fld.Value.ToString().Replace(' ', '♠').Replace('(', '♦').Replace(')', '♣') + "\"  class='atchfile inlinediv'>");
                    grdLinksInline.Append("<a onclick=\"DeleteFileFromRow('" + Name + "','" + rowFrmNo + "','" + Fld.Value.ToString() + "')\"><i class=\"glyphicon glyphicon-remove close icon-arrows-remove attachmentcrossicon\"></i></a>");
                    if (!Name.ToLower().StartsWith("axpfile_"))
                        grdLinksInline.Append("<a href=\"javascript:void(0)\" id='grdAtt_hlnk_" + rowFrmNo + "' class='grdAttach handCur' onclick='ShowGridAttLink(\"" + hlnkSrc + Fld.Value.ToString() + "\")'>" + Fld.Value.ToString() + "</a>");
                    else
                        grdLinksInline.Append("<a href=\"javascript:void(0)\" id='GridAxpFile_hlnk_" + rowFrmNo + "' class='grdAttach handCur' onclick='ShowAxpFileuploadLink(\"" + Name + "\",\"" + Fld.Value.ToString() + "\",\"" + hlnkSrc + Fld.Value.ToString() + "\")'>" + Fld.Value.ToString() + "</a>");
                    grdLinksInline.Append("</div>");
                }
                else
                {
                    hlnkSrc = hlnkSrc + Fld.Value;
                }
            }
        }


        if (IsGrid)
        {
            if (IsFillGridCall)
            {
                if (!Name.ToLower().StartsWith("axpfile_"))
                    grdAttHTML.Append("<textarea class=\"form-control w-100 border bg-transparent overflow-hidden resize-none labelInp \" style=\"display: none;\" maxlength=\"\" data-style=\"\" data-type=\"gridattach\" data-hidden=\"\"  id=\"" + Name + "\"  readonly=\"\">" + Fld.Value + "</textarea>");
                else
                    grdAttHTML.Append("<textarea class=\"axpAttach form-control w-100 border bg-transparent overflow-hidden resize-none labelInp \" style=\"display: none;\" maxlength=\"\" data-style=\"\" data-type=\"gridattach\" data-hidden=\"\"  id=\"" + Name + "\"  readonly=\"\">" + Fld.Value + "</textarea>");
                if (HttpContext.Current.Session["AxInlineGridEdit"] != null)
                    grdAttHTML.Append("<div><input type=\"hidden\" id=\"" + Name + "\" tabindex=\"-1\" " + strClass + " value=\"" + Fld.Value + "\"></input>");
            }
            else
            {
                grdAttHTML.Append("<input type=\"hidden\" id=\"" + Name + "\" tabindex=\"-1\" " + strClass + "></input>");
            }
            if (!IsFillGridCall)
            {
                if (!Name.ToLower().StartsWith("axpfile_"))
                    grdAttHTML.Append("<div class=\"form-group form-control p-0 grdattch\" id=\"" + divId + "\"><div id=\"dropzone_" + Name + "\" name=\"" + Name + "\" class=\"dropzone dropzone-queue min-h-1px border-0 px-3 py-2 dropzoneGrid\"><div class=\"d-flex flex-row-auto dropzone-panel mb-lg-0 m-0\"><a id=\"" + Name + "\" class=\"dropzone-select fs-7 text-truncate me-1-- gridattach\"><span class=\"material-icons material-icons-style material-icons-2 float-start mx-2--\">attach_file</span> Drop files here or click to upload</a><span class=\"material-icons material-icons-style material-icons-2 ms-auto pe-7 fileuploadmore d-none\" data-bs-toggle=\"popover\" data-bs-sanitize=\"false\" data-bs-placement=\"bottom\" data-bs-html=\"true\">more</span><div class=\"position-relative ms-0 mt-n1 d-none\" id=\"gridAttachCounter\"><span class=\"text-gray-600 spanAttCount fs-5 border border-1 border-gray-300 badge bg-white py-0 px-1 shadow-sm mt-1\"></span><span class=\"bullet bullet-dot bg-success h-6px w-6px position-absolute translate-middle top-0 start-75 animation-blink\"></span></div><a class=\"dropzone-remove-all btn btn-sm btn-light-primary d-none\">Remove All</a></div><div class=\"dropzone-items wm-200px d-none\"><div class=\"dropzone-item\" style=\"display: none\"><div class=\"dropzone-file overflow-hidden\"><div class=\"dropzone-filename\" title=\"some_image_file_name.jpg\"><span data-dz-name>some_image_file_name.jpg</span></div><div class=\"dropzone-error\" data-dz-errormessage></div></div><div class=\"dropzone-progress d-none\"><div class=\"progress\"><div class=\"progress-bar bg-primary\" role=\"progressbar\" aria-valuemin=\"0\" aria-valuemax=\"100\" aria-valuenow=\"0\" data-dz-uploadprogress></div></div></div><div class=\"dropzone-toolbar\"><span class=\"dropzone-delete\" data-dz-remove><span class=\"material-icons material-icons-style material-icons-2 float-end\">clear</span></span></div></div></div></div></div>");
                else
                {
                    grdAttHTML.Append("<div class=\"form-group form-control p-0 grdattch\" id=\"" + divAxpFileId + "\"><div id=\"dropzone_" + Name + "\" name=\"" + Name + "\" class=\"dropzone dropzone-queue min-h-1px border-0 px-3 py-2\"><div class=\"d-flex flex-row-auto dropzone-panel mb-lg-0 m-0\"><a id=\"" + Name + "\" class=\"dropzone-select fs-7 text-truncate me-1-- gridattach\"><span class=\"material-icons material-icons-style material-icons-2 float-start mx-2--\">attach_file</span> Drop files here or click to upload</a><span class=\"material-icons material-icons-style material-icons-2 ms-auto pe-7 fileuploadmore d-none\" data-bs-toggle=\"popover\" data-bs-sanitize=\"false\" data-bs-placement=\"bottom\" data-bs-html=\"true\">more</span><div class=\"position-relative ms-0 mt-n1 d-none\" id=\"gridAttachCounter\"><span class=\"text-gray-600 spanAttCount fs-5 border border-1 border-gray-300 badge bg-white py-0 px-1 shadow-sm mt-1\"></span><span class=\"bullet bullet-dot bg-success h-6px w-6px position-absolute translate-middle top-0 start-75 animation-blink\"></span></div><a class=\"dropzone-remove-all btn btn-sm btn-light-primary d-none\">Remove All</a></div><div class=\"dropzone-items wm-200px d-none\"><div class=\"dropzone-item\" style=\"display: none\"><div class=\"dropzone-file overflow-hidden\"><div class=\"dropzone-filename\" title=\"some_image_file_name.jpg\"><span data-dz-name>some_image_file_name.jpg</span></div><div class=\"dropzone-error\" data-dz-errormessage></div></div><div class=\"dropzone-progress d-none\"><div class=\"progress\"><div class=\"progress-bar bg-primary\" role=\"progressbar\" aria-valuemin=\"0\" aria-valuemax=\"100\" aria-valuenow=\"0\" data-dz-uploadprogress></div></div></div><div class=\"dropzone-toolbar\"><span class=\"dropzone-delete\" data-dz-remove><span class=\"material-icons material-icons-style material-icons-2 float-end\">clear</span></span></div></div></div></div></div>");
                }
            }
            else if (IsFillGridCall && HttpContext.Current.Session["AxInlineGridEdit"] != null)
            {
                if (!Name.ToLower().StartsWith("axpfile_"))
                    grdAttHTML.Append("<div class=\"form-group form-control p-0 grdattch\" id=\"" + divId + "\"><div id=\"dropzone_" + Name + "\" name=\"" + Name + "\" class=\"dropzone dropzone-queue min-h-1px border-0 px-3 py-2 dropzoneGrid\"><div class=\"d-flex flex-row-auto dropzone-panel mb-lg-0 m-0\"><a id=\"" + Name + "\" class=\"dropzone-select fs-7 text-truncate me-1-- gridattach\"><span class=\"material-icons material-icons-style material-icons-2 float-start mx-2--\">attach_file</span> Drop files here or click to upload</a><span class=\"material-icons material-icons-style material-icons-2 ms-auto pe-7 fileuploadmore d-none\" data-bs-toggle=\"popover\" data-bs-sanitize=\"false\" data-bs-placement=\"bottom\" data-bs-html=\"true\">more</span><div class=\"position-relative ms-0 mt-n1 d-none\" id=\"gridAttachCounter\"><span class=\"text-gray-600 spanAttCount fs-5 border border-1 border-gray-300 badge bg-white py-0 px-1 shadow-sm mt-1\"></span><span class=\"bullet bullet-dot bg-success h-6px w-6px position-absolute translate-middle top-0 start-75 animation-blink\"></span></div><a class=\"dropzone-remove-all btn btn-sm btn-light-primary d-none\">Remove All</a></div><div class=\"dropzone-items wm-200px d-none\"><div class=\"dropzone-item\" style=\"display: none\"><div class=\"dropzone-file overflow-hidden\"><div class=\"dropzone-filename\" title=\"some_image_file_name.jpg\"><span data-dz-name>some_image_file_name.jpg</span></div><div class=\"dropzone-error\" data-dz-errormessage></div></div><div class=\"dropzone-progress d-none\"><div class=\"progress\"><div class=\"progress-bar bg-primary\" role=\"progressbar\" aria-valuemin=\"0\" aria-valuemax=\"100\" aria-valuenow=\"0\" data-dz-uploadprogress></div></div></div><div class=\"dropzone-toolbar\"><span class=\"dropzone-delete\" data-dz-remove><span class=\"material-icons material-icons-style material-icons-2 float-end\">clear</span></span></div></div></div></div></div>");
                else
                    grdAttHTML.Append("<div class=\"form-group form-control p-0 grdattch\" id=\"" + divAxpFileId + "\"><div id=\"dropzone_" + Name + "\" name=\"" + Name + "\" class=\"dropzone dropzone-queue min-h-1px border-0 px-3 py-2\"><div class=\"d-flex flex-row-auto dropzone-panel mb-lg-0 m-0\"><a id=\"" + Name + "\" class=\"dropzone-select fs-7 text-truncate me-1-- gridattach\"><span class=\"material-icons material-icons-style material-icons-2 float-start mx-2--\">attach_file</span> Drop files here or click to upload</a><span class=\"material-icons material-icons-style material-icons-2 ms-auto pe-7 fileuploadmore d-none\" data-bs-toggle=\"popover\" data-bs-sanitize=\"false\" data-bs-placement=\"bottom\" data-bs-html=\"true\">more</span><div class=\"position-relative ms-0 mt-n1 d-none\" id=\"gridAttachCounter\"><span class=\"text-gray-600 spanAttCount fs-5 border border-1 border-gray-300 badge bg-white py-0 px-1 shadow-sm mt-1\"></span><span class=\"bullet bullet-dot bg-success h-6px w-6px position-absolute translate-middle top-0 start-75 animation-blink\"></span></div><a class=\"dropzone-remove-all btn btn-sm btn-light-primary d-none\">Remove All</a></div><div class=\"dropzone-items wm-200px d-none\"><div class=\"dropzone-item\" style=\"display: none\"><div class=\"dropzone-file overflow-hidden\"><div class=\"dropzone-filename\" title=\"some_image_file_name.jpg\"><span data-dz-name>some_image_file_name.jpg</span></div><div class=\"dropzone-error\" data-dz-errormessage></div></div><div class=\"dropzone-progress d-none\"><div class=\"progress\"><div class=\"progress-bar bg-primary\" role=\"progressbar\" aria-valuemin=\"0\" aria-valuemax=\"100\" aria-valuenow=\"0\" data-dz-uploadprogress></div></div></div><div class=\"dropzone-toolbar\"><span class=\"dropzone-delete\" data-dz-remove><span class=\"material-icons material-icons-style material-icons-2 float-end\">clear</span></span></div></div></div></div></div>");
            }
            if (hlnkSrc != string.Empty)
            {
                if (hasMultipleFiles)
                {
                    grdAttHTML.Append(grdLinks);

                }
                else
                {
                    if (IsFillGridCall)
                    {
                        grdAttHTML.Append(grdLinks);
                    }
                    else
                    {
                        if (!Name.ToLower().StartsWith("axpfile_"))
                            grdAttHTML.Append("<a href=\"javascript:void(0)\" id=\"" + hlnkId + "\" tabindex=\"0\" " + strClass + " onclick=\"ShowGridAttLink('" + hlnkSrc + "')\">" + Fld.Value + "</a>");
                        else
                            grdAttHTML.Append("<a href=\"javascript:void(0)\" id=\"" + axpFilehlnkId + "\" tabindex=\"0\" " + strClass + " onclick=\"ShowAxpFileuploadLink(\"" + Name + "\",\"" + Fld.Value + "\",'" + hlnkSrc + "')\">" + Fld.Value + "</a>");
                    }
                }
            }
            //else if (!Name.ToLower().StartsWith("axpfile_"))
            //    grdAttHTML.Append("<a href=\"javascript:void(0)\" id=\"" + hlnkId + "\" tabindex=\"-1\" " + strClass + " >" + Fld.Value + "</a>");
            ////else
            ////    grdAttHTML.Append("<a href=\"javascript:void(0)\" id=\"" + axpFilehlnkId + "\" tabindex=\"-1\" " + strClass + " >" + Fld.Value + "</a>");
            //if (!IsFillGridCall)
            //{
            //    if (!Name.ToLower().StartsWith("axpfile_"))
            //        grdAttHTML.Append("</div>");
            //}
            else if (IsFillGridCall && HttpContext.Current.Session["AxInlineGridEdit"] != null)
            {
                string[] fldValues = new string[] { };
                if (Fld.Value != null)
                {
                    fldValues = Fld.Value.Split(',');
                }

                if (fldValues.Length != 0 && Fld.Value != "")
                {
                    if (!Name.ToLower().StartsWith("axpfile_"))
                        grdAttHTML.Append("<div id='" + hlnkId + "attach' class='attach-files'>" + grdLinksInline + "</div>");
                    else
                        grdAttHTML.Append("<div id='axpFileAtt_hlnk_" + rowFrmNo + "attach' class='attach-files'>" + grdLinksInline + "</div>");
                }
                grdAttHTML.Append("</div></div>");
            }

        }

        return grdAttHTML.ToString();
    }

    private string GetGridReferHTML(FieldStruct Fld, bool IsGrid, string Name, int fldNo, string[] fldTlhw, string width)
    {
        string rowFrmNo = Name.Substring(Name.LastIndexOf("F") - 3);
        //Name  - axp_gridattach_2001F2
        string imgId = "grdRef_img_" + rowFrmNo;
        string hlnkId = "grdRef_hlnk_" + rowFrmNo;

        StringBuilder grdAttHTML = new StringBuilder();
        string hlnkSrc = string.Empty;
        if (!string.IsNullOrEmpty(Fld.Value))
        {
            Util.Util utilObj = new Util.Util();
            hlnkSrc = utilObj.ScriptsurlPath + "axpert//" + HttpContext.Current.Session["nsessionid"].ToString() + "//" + Fld.Value;
        }

        string strClass = "class=\"grdRefer handCur\"";
        if (Fld.freadonly)
            strClass = "";

        if (IsGrid)
        {

            grdAttHTML.Append("<input type=\"hidden\" id=\"" + Name + "\" " + strClass + "/>");
            grdAttHTML.Append("<span id=\"" + imgId + "\" tabindex=\"0\" title=\"" + Fld.fieldHint + "\" class=\"fa fa-paperclip\" onclick=\"ShowGridAttachPopUp(this);\" " + strClass + " />");
            if (hlnkSrc != string.Empty)
                grdAttHTML.Append("<a href=\"javascript:void(0)\" id=\"" + hlnkId + "\" tabindex=\"0\" " + strClass + " onclick=\"ShowGridAttLink('" + hlnkSrc + "')\">" + Fld.Value + "</a>");
            else
                grdAttHTML.Append("<a href=\"javascript:void(0)\" id=\"" + hlnkId + "\" tabindex=\"0\" " + strClass + " >" + Fld.Value + "</a>");
        }

        return grdAttHTML.ToString();
    }

    //Function to get the html for attachment for pdf files for digitial siganture
    private string GetFileUploadDSignHTML(FieldStruct Fld, bool IsGrid, string Name, int fldNo, string[] fldTlhw, string width)
    {
        string imgId = "img_" + Name;
        string hdnID = "hdn_" + Name;
        string printDocName = Fld.expr.Replace("{", "");
        printDocName = printDocName.Replace("}", "");
        StringBuilder upldDsignHTML = new StringBuilder();
        string strClass = "class=\"handCur disupldsign\"";
        if (Fld.freadonly)
            strClass = "";
        if (IsGrid)
        {
            upldDsignHTML.Append("<input type=\"hidden\" id=\"" + hdnID + "\"  value=\"" + printDocName + "\"/>");
            upldDsignHTML.Append("<input type=\"text\"   class='disupldsign'  id=\"" + Name + "\"/>");
            upldDsignHTML.Append("<span id=\"" + imgId + "\" tabindex=\"0\" title=\"" + Fld.fieldHint + "\" class=\"fa fa-paperclip\" onclick=\"ShowFileUplDSignPopUp(this);\" " + strClass + " />");

        }
        return upldDsignHTML.ToString();
    }



    //Function to get the html of an image field in the tstruct.
    private string GetImageHTML(FieldStruct Fld, bool IsGrid, string Name, int fldNo, string[] fldTlhw, string width, int fcwidth)
    {
        StringBuilder imgHTML = new StringBuilder();
        string hiddenName = Fld.name + "\" +nrno+\"";

        if (!string.IsNullOrEmpty(Fld.Value))
        {
            Util.Util utilObj = new Util.Util();
            Fld.Value = utilObj.ScriptsurlPath + "axpert//" + HttpContext.Current.Session["nsessionid"].ToString() + "//" + Fld.Value;
        }
        else
        {
            Fld.Value = (Name.ToLower().StartsWith("sig_")) ? "../AxpImages/signature.png" : "../AxpImages/upload.png";
            //Fld.Value = "../AxpImages/upload.png";
        }
        string divColsm = "";
        if (!IsGrid && fcwidth > 0)
        {
            divColsm = "col-sm-" + fcwidth + " ";
        }
        string strClass = (Name.ToLower().StartsWith("sig_")) ? "class=signaturePad" : "class=axpImg";
        if (Fld.freadonly)
        {
            strClass = "";
            //imgHTML.Append("<div class=\"" + divColsm + "divReadOnly\"><div class=\"dvImgClear\"><i id=\"del-" + Name + "\" class=\"glyphicon glyphicon-remove icon-arrows-remove\"></i> </div>");
            //imgHTML.Append("<div class=\"input-group divReadOnly\">");
        }
        else
        {
            //imgHTML.Append("<div class=\"" + divColsm + "divImgBorder\"><div class=\"dvImgClear\"><i id=\"del-" + Name + "\" class=\"glyphicon glyphicon-remove icon-arrows-remove\" onclick=\"ClearImageSrc(this);\"></i></div>");
            //imgHTML.Append("<div class=\"input-group\">");
        }

        if (IsGrid)
        {
            imgHTML.Append("<img id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" src=\"" + Fld.Value + "\" " + strClass + "  />");
            imgHTML.Append("<INPUT type=hidden name=\"" + Name + "\" id=\"" + Name + "\" /></div>");
        }
        else
        {
            //imgHTML.Append("<img id=\"" + Name + "\" title=\"" + Fld.fieldHint + "\" src=\"" + Fld.Value + "\" " + strClass + "  />");
            //imgHTML.Append("<INPUT type=\"hidden\" name=\"" + Name + "\" id=\"" + Name + "\" /></div>");

            if (Name.ToLower().StartsWith("sig_"))
            {
                imgHTML.Append("<div class=\"input-group input-group-sm flex-root mb-2 pe-4 divImgBorder signaturePad\"><div class=\"image-input image-input-outline w-100 dvImgClear\" data-kt-image-input=\"false\">");
                imgHTML.Append("<div class=\"shadow-sm w-100 h-100 imageFileUploadz d-flex\"><span class=\"material-icons material-icons-style material-icons-4x d-flex m-auto align-self-center cursor-pointer\">draw</span></div>");
                imgHTML.Append("<img id=\"" + Name + "\" class=\"profile-pic image-input-wrapper signatureInput w-100 h-100 p-3 position-absolute top-50 start-50 translate-middle bgi-size-contain of-contain d-none\" src=\"\">");
                imgHTML.Append("<input class=\"file-upload tstfldImagez d-none\" name=\"" + Name + "\" id=\"" + Name + "\" data-type=\"webImg\" type=\"file\" name=\"avatar\" accept=\"" + Constants.fileExtensionImage + "\">");
                imgHTML.Append("<input type=\"hidden\" name=\"avatar_remove\">");
                imgHTML.Append("<span class=\"delete-button d-flex btn btn-icon btn-circle btn-active-color-primary w-25px h-25px bg-white shadow-sm mt-n4 ms-auto d-none\" data-kt-image-input-action=\"remove\" data-bs-toggle=\"tooltip\" data-bs-dismiss=\"click\" title=\"Remove avatar\">");
                imgHTML.Append("<i id=\"del-" + Name + "\" onclick=\"ClearImageSrc(this);\" class=\"shadow-sm  w-25px h-25px bg-white btn btn-icon btn-circle btn-active-color-primary material-icons material-icons-style\">delete</i>");
                imgHTML.Append("</span>");
                imgHTML.Append("</div></div>");
            }
            else
            {
                imgHTML.Append("<div class=\"input-group input-group-sm flex-root mb-2 pe-4 divImgBorder\"><div class=\"image-input image-input-outline w-100 dvImgClear\" data-kt-image-input=\"true\" style=\"background-image: url(/assets/media/avatars/blank.png)\">");
                imgHTML.Append("<div class=\"shadow-sm w-100 h-100 imageFileUpload d-flex\"><span class=\"material-icons material-icons-style material-icons-4x d-flex m-auto align-self-center cursor-pointer\">file_upload</span></div>");
                imgHTML.Append("<img id=\"" + Name + "\" class=\"profile-pic image-input-wrapper w-100 h-100 p-3 position-absolute top-50 start-50 translate-middle bgi-size-contain of-contain d-none\" src=\"\">");
                imgHTML.Append("<span class=\"shadow-sm w-25px h-25px bg-white btn btn-icon btn-circle btn-active-color-primary material-icons material-icons-style position-absolute top-0 end-0 mt-n4 p-4 me-5 fldImageCamera\">photo_camera</span>");
                imgHTML.Append("<label class=\"upload-button btn btn-icon btn-sm btn-circle btn-active-color-primary w-25px h-25px bg-white shadow-sm\" data-kt-image-input-action=\"change\" data-bs-toggle=\"tooltip\" data-bs-dismiss=\"click\" title=\"Change avatar\">");
                imgHTML.Append("<i class=\"shadow-sm w-25px h-25px bg-white btn btn-icon btn-circle btn-active-color-primary material-icons material-icons-style\">edit</i>");
                imgHTML.Append("<input class=\"file-upload tstfldImage d-none\" name=\"" + Name + "\" id=\"" + Name + "\" data-type=\"webImg\" type=\"file\" name=\"avatar\" accept=\"" + Constants.fileExtensionImage + "\">");
                imgHTML.Append("<input type=\"hidden\" name=\"avatar_remove\">");
                imgHTML.Append("</label>");
                imgHTML.Append("<span class=\"delete-button d-flex btn btn-icon btn-circle btn-active-color-primary w-25px h-25px bg-white shadow-sm mt-n4 d-none\" data-kt-image-input-action=\"remove\" data-bs-toggle=\"tooltip\" data-bs-dismiss=\"click\" title=\"Remove avatar\">");
                imgHTML.Append("<i id=\"del-" + Name + "\" onclick=\"ClearImageSrc(this);\" class=\"shadow-sm  w-25px h-25px bg-white btn btn-icon btn-circle btn-active-color-primary material-icons material-icons-style\">delete</i>");
                imgHTML.Append("</span>");
                imgHTML.Append("</div></div>");
                //imgHTML.Append("</div>");
            }
        }

        return imgHTML.ToString();
    }

    //Function to get the html of an axpert button in the tstruct.
    private string GetAxpBtnHTML(FieldStruct field, bool isGrid, string name, string width)
    {
        StringBuilder btnHTML = new StringBuilder();
        string hiddenName = field.name + "\" +nrno+\"";
        if (isGrid)
        {
            btnHTML.Append("<a href=\"javascript:void(0)\" class=\"Grdlnk axpBtn\" id=\"" + name + "\" >" + field.caption + "</a>");
            if (IsFillGridCall)
            {
                btnHTML.Append("<textarea tabindex=\"-1\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  Grdlnk axpBtn\" maxlength=\"\"  data-style=\"\" data-type=\"axpBtn\" data-hidden=\"\" id=\"txt_axpBtn_" + name + "\" style=\"display: none;\"  readonly>" + field.caption + "</textarea>");
            }
        }
        else
        {
            btnHTML.Append("<a href=\"javascript:void(0)\"  class=\"Grdlnk axpBtn\" id=\"" + name + "\" >" + field.caption + "</a>");
        }
        return btnHTML.ToString();
    }

    private string GetGridButtonHtml(string dcNo)
    {
        StringBuilder buttonsHtml = new StringBuilder();

        bool allowMultiAdd = false;
        int axRowsLimit = 0;
        allowMultiAdd = AxRowLimit.TryGetValue("ax_rowlimit" + dcNo, out axRowsLimit);

        for (int i = 0; i < dcs.Count; i++)
        {
            DcStruct dcStr = (DcStruct)dcs[i];

            if (dcNo == dcStr.frameno.ToString())
            {
                string dcBoolean = string.Empty;
                if (dcStr.DcDefaultstate != "" && dcStr.DcDefaultstate.ToLower() == "collapse")
                    dcBoolean = " d-none ";
                bool addBtn = dcStr.isallowaddrows;
                buttonsHtml.Append("<div class=\"d-flex align-items-center flex-nowrap text-nowrap mb-1 newgridbtn gridIconBtns " + dcBoolean + "\">");

                if (dcStr.isallowaddrows)
                    buttonsHtml.Append("<a class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm\" id=\"gridAddBtn" + dcNo + "\" onclick=\"editTheRow(''," + dcNo + ",'',event)\" title=\"Add Row\"><span class=\"material-icons material-icons-style material-icons-3\">add</span></a>");

                if (dcStr.isallowdeletrows)
                    buttonsHtml.Append("<a href=\"javascript:void(0)\" id='clearThisDC" + dcNo + "' onclick=\"javascript:clearDataGrid('" + dcNo + "');\" class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm disabled\" title=\"Clear Data\"><span class=\"material-icons material-icons-style material-icons-3\">delete_outline</span></a>");

                if (isMobile)
                    buttonsHtml.Append("<a href=\"javascript:void(0)\" id='viewGrid" + dcNo + "' onclick=\"javascript:viewGridPopUp('" + dcNo + "');\" class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm lblViewGrid\" title=\"View Grid\"><span class=\"material-icons material-icons-style material-icons-3\">table_rows</span></a>");

                if (!isMobile)
                {
                    buttonsHtml.Append("<a href=\"javascript:void(0)\" id='exportGridToExcel" + dcNo + "' onclick=\"javascript:exportGridToExcel('" + dcNo + "');\" class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm\" title=\"Send to Excel\"><span class=\"material-icons material-icons-style material-icons-3\">table_chart</span></a>");
                    buttonsHtml.Append("<a href=\"javascript:void(0)\" id='ExcelGridimport" + dcNo + "' onclick=\"javascript:importExceltoGrid('" + dcNo + "');\" class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm\" title=\"Import Data from Excel to Grid\"><span class=\"material-icons material-icons-style material-icons-3\">table_view</span></a>");
                    buttonsHtml.Append("<a href=\"javascript:void(0)\" id='freezeGridColumns" + dcNo + "' onclick=\"javascript:FreezeGridCols('" + dcNo + "');\" class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm\" title=\"Freeze Grid Columns\"><span class=\"material-icons material-icons-style material-icons-3\">list_alt</span></a>");
                }

                ArrayList dcFsg = new ArrayList(fgs.Cast<FGStruct>().Where(f => f.fgtargetdc == ("dc" + dcNo)).ToList());

                if (dcFsg.Count == 1)
                {
                    FGStruct fg = ((FGStruct)dcFsg[0]);
                    FDR fastData = (FDR)HttpContext.Current.Session["FDR"];
                    string strOnClk = string.Empty;
                    if (fastData.IsFastFillGrid(this, fg.fgName, dcNo))
                        strOnClk = "onclick =\"javascript:FillGrid('" + dcNo + "','','" + fg.fgName + "',true);\"";
                    else
                        strOnClk = "onclick =\"javascript:FillGrid('" + dcNo + "','','" + fg.fgName + "');\"";
                    string fgCapt = fg.fgcaption == string.Empty ? fg.fgName : fg.fgcaption;
                    buttonsHtml.Append("<a href=\"javascript:void(0)\" " + strOnClk + " class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm\" id=\"fillgrid" + dcNo + "\" name=\"" + fg.fgName + "\" title=\"" + fgCapt + "\"><span class=\"material-icons material-icons-style material-icons-3\">view_module</span></a>");
                }
                else if (dcFsg.Count > 1)
                {
                    int frameNo = Convert.ToInt32(dcNo);
                    int cnt = 0;
                    buttonsHtml.Append("<div class=\"dropdown btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm\"><a id=\"fillgridList" + dcNo + "\" data-bs-toggle=\"dropdown\" aria-expanded=\"false\" title=\"Fill Grid List\"><span class='material-icons material-icons-style material-icons-3'>import_export</span></a><ul class=\"dropdown-menu\" aria-labelledby=\"fillgridList\">");
                    for (cnt = 0; cnt <= dcFsg.Count - 1; cnt++)
                    {
                        FGStruct fg = ((FGStruct)dcFsg[cnt]);
                        FDR fastData = (FDR)HttpContext.Current.Session["FDR"];
                        if (fastData.IsFastFillGrid(this, fg.fgName, dcNo))
                        {
                            buttonsHtml.Append("<li class=\"dropdown-item\" onclick=\"javascript:FillGrid('" + dcNo + "','','" + fg.fgName + "',true);\">");
                        }
                        else
                        {
                            buttonsHtml.Append("<li class=\"dropdown-item\" onclick=\"javascript:FillGrid('" + dcNo + "','','" + fg.fgName + "');\">");
                        }
                        string fgCapt = fg.fgcaption == string.Empty ? fg.fgName : fg.fgcaption;
                        buttonsHtml.Append("<a href=\"javascript:void(0)\" id='" + fg.fgName + "' type='button'  name='" + fg.fgName + "' title='" + fgCapt + "'>" + fgCapt + "</a></li>");
                    }
                    buttonsHtml.Append("</ul></div>");
                }

                for (int m = 0; m < gridHeaderScriptDcNo.Count; m++)
                {
                    if (dcNo == gridHeaderScriptDcNo[m].ToString())
                    {
                        buttonsHtml.Append(gridHeaderScriptButtons[m].ToString());
                    }
                }

                buttonsHtml.Append("</div>");
            }
        }
        return buttonsHtml.ToString();

    }

    private string GetFastAutoComHTML(FieldStruct fld, bool isGrid, string name, string fldReadOnly, string bgColor, bool isPicklist)
    {
        StringBuilder fastdataHTML = new StringBuilder();
        if (fld.fldMultiSelect != null && fld.fldMultiSelect != string.Empty)
        {
            fldCss = fldCss.Replace("form-control", "form-control fldmultiSelect");
            if (fld.refreshOnSave != "" && fld.refreshOnSave == "True")
                fldCss = fldCss.Replace("fldmultiSelect", "fldmultiSelect isrefreshsave");
            string dataTypeMs = "multigroupselect";
            if (IsFillGridCall)
                fastdataHTML.Append("<textarea tabindex=\"-1\" " + fldCss + " maxlength=\"" + fld.fldlength + "\" data-style=\"" + bgColor + "\" data-type=\"" + dataTypeMs + "\" data-sep=\"" + fld.fldMultiSelectSep + "\" data-hidden=\"\" id=\"" + name + "\" readonly=\"\">" + fld.Value + "</textarea>");
            else
                fastdataHTML.Append("<select id=\"" + name + "\" " + fldCss + " data-control=\"select2\" data-placeholder=\"Select an option\" data-allow-clear=\"true\" multiple data-selected=\"" + fld.Value + "\" data-type=\"" + dataTypeMs + "\" data-sep=\"" + fld.fldMultiSelectSep + "\"></select>");
            return fastdataHTML.ToString();
        }

        fldCss = fldCss.Replace("inputClass2", "fldFromSelect");
        if (fldCss.IndexOf("fldFromSelect") == -1)
            fldCss = fldCss.Replace("form-control", "form-control fldFromSelect");
        string dataType = "fromselect-pick";
        string refSelsrctid = string.Empty;
        refSelsrctid = GetSelectRefreshTid(fld.name);
        if (!isPicklist)
        {
            if (fld.refreshOnSave != "" && fld.refreshOnSave == "True")
                fldCss = fldCss.Replace("fldFromSelect", "fldFromSelect fastdll isrefreshsave");
            else
                fldCss = fldCss.Replace("fldFromSelect", "fldFromSelect fastdll");
            dataType = "fromselect-select";

        }
        else if (isPicklist)
        {
            if (fld.refreshOnSave != "" && fld.refreshOnSave == "True")
                fldCss = fldCss.Replace("fldFromSelect", "fldFromSelect isrefreshsave");
        }

        if (fld.name.ToLower().StartsWith("axpfilepath_") && fldCss.IndexOf("axpFilePathFld") == -1)
        {
            var endPart = name.Substring(12);
            fldCss = fldCss.Replace("fldFromSelect", "fldFromSelect axpFilePath_" + endPart + " axpFilePathFld ");
        }

        if (IsFillGridCall)
        {
            fastdataHTML.Append("<textarea tabindex=\"-1\" " + fldCss + " maxlength=\"" + fld.fldlength + "\" data-style=\"" + bgColor + "\" data-type=\"" + dataType + "\" data-hidden=\"\" id=\"" + name + "\" readonly=\"\">" + fld.Value + "</textarea>");
        }
        else
        {
            string refSelsrctDetails = "";
            if (refSelsrctid != "")
                refSelsrctDetails = refSelsrctid;
            if (isPicklist)
                fastdataHTML.Append("<select " + fldCss + " data-control=\"select2\" id=\"" + name + "\" data-placeholder=\"Select an option\" data-allow-clear=\"true\" data-refresh=\"false\" data-addoption=\"" + refSelsrctDetails + "\"></select>");
            else
                fastdataHTML.Append("<select " + fldCss + " data-control=\"select2\" id=\"" + name + "\" data-placeholder=\"Select an option\" data-allow-clear=\"true\" data-refresh=\"true\" data-addoption=\"" + refSelsrctDetails + "\"></select>");
        }
        return fastdataHTML.ToString();
    }

    private string GetCustLabelHTML(string fldName, int fldDcNo)
    {
        StringBuilder labelHtml = new StringBuilder();
        try
        {
            if (axdesignJObject.formLabel != null && axdesignJObject.formLabel.Count > 0)
            {
                string colFldWidth = "", colFldGridStackItem = "", colDivInputPadding = "";
                if (axdesignJObject.dcLayout != null && axdesignJObject.dcLayout != "" && axdesignJObject.dcLayout != "default")
                {
                    colFldWidth = "<div class=\"fld-wrap3 col-sm-12\">";
                    colFldGridStackItem = " colFldGridStackWidth ";
                    colDivInputPadding = " colDivInputPadding ";
                }

                var frLbl = axdesignJObject.formLabel.Where(elm => elm.dc == fldDcNo & elm.afterField == (fldName == "" ? "divDc" + fldDcNo : fldName)).ToList();
                if (frLbl != null && frLbl.Count > 0)
                {
                    foreach (var frLblVal in frLbl)
                    {
                        string style = "cursor: default;";
                        FieldStruct dummy = new FieldStruct();
                        dummy.visibility = false;
                        dummy.fldlength = 15;
                        dummy.fldFrameNo = fldDcNo;
                        if (frLblVal.hyperlinkJson != "")
                            dummy.ctype = "hyperlink";
                        else
                            dummy.ctype = "label";
                        dummy.datatype = "special";
                        dummy.name = frLblVal.name;

                        labelHtml.Append("<div class=\"  grid-stack-item " + colFldGridStackItem + "\"" + xywhGridStack(dummy) + ">");
                        labelHtml.Append("<div class=\"grid-stack-item-content\"></div>");
                        //labelHtml.Append("<div id=\"dv" + frLblVal.name.Replace(" ", "") + "\" class=\"labelcol inputs grid-stack-item-content form-group row formLabelParent " + colDivInputPadding + "\">");
                        labelHtml.Append("<div id=\"dv" + frLblVal.id + "\" class=\"labelcol inputs grid-stack-item-content form-group row formLabelParent " + colDivInputPadding + "\">");
                        labelHtml.Append(colFldWidth);
                        labelHtml.Append("<label class=\"gridstackCalc formLabel\" for=\"" + frLblVal.name.Replace(" ", "") + "\" data-id=\"" + frLblVal.id + "\" style=\"" + frLblVal.fontFamilly + "\"></label>");

                        if (frLblVal.hyperlinkJson != "")
                            labelHtml.Append("<a href=\"javascript:void(0)\" class=\"cursor-pointer\" style=\"" + frLblVal.fontFamilly + "\" id=\"" + frLblVal.id + "\" onclick='javascript:TstructLabelhyperlink(this);' data-param='" + frLblVal.hyperlinkJson + "'>" + frLblVal.name + "</a></div>");
                        else
                            labelHtml.Append("<font style=\"" + style + frLblVal.fontFamilly + "\" class=\"\">" + frLblVal.name + "</font></div>");
                        if (colFldWidth != "")
                            labelHtml.Append("</div>");
                        labelHtml.Append("</div>");
                    }
                }
            }
        }
        catch (Exception ex) { }
        return labelHtml.ToString();
    }
    #endregion

    #region LoadTabHTML

    public string GetTabFullHTML(int dcNo, int rowCnt, TStructData tstData, string isTab, string IsTabDis)
    {
        string tabHTML = string.Empty;
        string fillGriddcRowone = "";
        int dcIndNo = dcsPositionIndex.IndexOf(dcNo.ToString());
        if (IsDcFormatGrid(dcIndNo, dcNo))
        {

            dcOtherRows.Append(GetFormatDcHtml(dcIndNo, dcNo, rowCnt, tstData, isTab, IsTabDis));
        }
        else
        {

            //To create html from second row till the row count of the dc
            int serialNo = 1;
            for (int i = 1; i <= rowCnt; i++)
            {

                string rowNo = string.Empty;
                if (IsDcGrid(dcNo))
                {
                    if (i.ToString().Length == 1)
                        rowNo = "00" + i;
                    else if (i.ToString().Length == 2)
                        rowNo = "0" + i;
                    else
                        rowNo = i.ToString();
                }
                else
                {
                    rowNo = "000";
                }
                string rowHTML = string.Empty;
                string fillGridRowHTML = string.Empty;
                if (IsFillGridCall)
                {
                    if (((DcStruct)(dcs[dcIndNo])).DCHasDataRows)
                    {
                        rowHTML = GetFillGridDataFieldHtml(dcIndNo, dcNo, tstData, IsTabDis, rowNo, "", null, null);
                    }
                    if (i == rowCnt)
                    {
                        IsFillGridCall = false;
                        //FillGridRecons = true;
                        fillGridRowHTML = GetTabFieldHtml(dcIndNo, dcNo, tstData, IsTabDis, rowNo, "", null, null);
                        IsFillGridCall = true;
                        //FillGridRecons = false;
                    }
                }
                else
                {
                    rowHTML = GetTabFieldHtml(dcIndNo, dcNo, tstData, IsTabDis, rowNo, "", null, null);
                }

                //Below code will create the serial no and delete button in the row
                string dcRowone = "";

                if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
                {
                    if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "false")
                        dcRowone = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\">" + rowHTML + "</div>";
                    else
                        //delete grid row
                        //dcRowone = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\"><td class='gridtdclass d-none' style='display:none'><a href=\"javascript:void(0)\" id=\"del" + rowNo + "F" + dcNo + "\" class=\"rowdelete\" title=\"Delete row\"><img src=\"../axpimages/icons/16x16/delete-fade.png\" title=\"Delete Row\" alt=\"Delete row\" /></a></td>" + rowHTML + "</div>";
                        dcRowone = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\">" + rowHTML + "</div>";
                }
                else if (((DcStruct)(dcs[dcIndNo])).isgrid)
                {
                    string rowSrNoClass = string.Empty;
                    if (IsFillGridCall)
                    {
                        rowSrNoClass = "gridtdclass";
                    }
                    else
                    {
                        rowSrNoClass = "gridtdclass d-none";
                    }
                    if (IsFillGridCall)
                    {
                        if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "false")
                        {
                            if (((DcStruct)(dcs[dcIndNo])).DCHasDataRows)
                            {
                                dcRowone = "<tr  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " ><td class=\"text-center\"><span class=\"tem1\"><div class=\"form-check form-check-sm form-check-custom ms-2\"><input class=\"form-check-input border-gray-500 fgChk gridRowChk opacity-100\" type=\"checkbox\" name=\"grdchkItemTd" + dcNo + "\" id=\"grdchkItemTd" + rowNo + "F" + dcNo + "\" onclick=\"javascript:CheckboxGridRow(this," + dcNo + "," + rowNo + ",event);\"></div></span></td><td class='gridElement " + rowSrNoClass + "'><label id=\"lblSlNo" + rowNo + "F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">" + serialNo.ToString() + "</label></td>" + rowHTML + "</tr>";
                            }
                            if (i == rowCnt)
                            {
                                fillGriddcRowone = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\"><div class='gridElement gridtdclass d-none'><label id=\"lblSlNo" + rowNo + "F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">" + serialNo.ToString() + "</label></div>" + fillGridRowHTML + "</div>";
                            }
                        }
                        else
                        {
                            //delete grid row
                            if (((DcStruct)(dcs[dcIndNo])).DCHasDataRows)
                            {
                                dcRowone = "<tr  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " ><td class=\"text-center\"><span class=\"tem1\"><div class=\"form-check form-check-sm form-check-custom ms-2\"><input class=\"form-check-input border-gray-500 fgChk gridRowChk opacity-100\" type=\"checkbox\" name=\"grdchkItemTd" + dcNo + "\" id=\"grdchkItemTd" + rowNo + "F" + dcNo + "\" onclick=\"javascript:CheckboxGridRow(this," + dcNo + "," + rowNo + ",event);\"></div></span></td><td class='gridElement " + rowSrNoClass + "'><label id=\"lblSlNo" + rowNo + "F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">" + serialNo.ToString() + "</label></td>" + rowHTML + "</tr>";
                            }
                            if (i == rowCnt)
                            {
                                //fillGriddcRowone = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\"><div class='gridtdclass d-none'><a href=\"javascript:void(0)\" id=\"del" + rowNo + "F" + dcNo + "\" class=\"rowdelete\" title=\"Delete row\"><img src=\"../axpimages/icons/16x16/delete-fade.png\" title=\"Delete Row\" alt=\"Delete row\" /></a></div><div class='gridElement gridtdclass d-none'><label id=\"lblSlNo" + rowNo + "F" + dcNo + "\" class=\"slno\">" + serialNo.ToString() + "</label></div>" + fillGridRowHTML + "</div>";
                                fillGriddcRowone = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\"><div class='gridElement gridtdclass d-none'><label id=\"lblSlNo" + rowNo + "F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">" + serialNo.ToString() + "</label></div>" + fillGridRowHTML + "</div>";
                            }
                        }
                    }
                    else
                    {
                        if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "false")
                        {
                            dcRowone = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\"><div class='gridElement " + rowSrNoClass + "'><label id=\"lblSlNo" + rowNo + "F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">" + serialNo.ToString() + "</label></div>" + rowHTML + "</div>";
                        }
                        else
                        {
                            //delete grid row
                            dcRowone = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\"><div class='gridElement " + rowSrNoClass + "''><label id=\"lblSlNo" + rowNo + "F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">" + serialNo.ToString() + "</label></div>" + rowHTML + "</div>";
                        }
                    }
                }
                else
                {
                    dcRowone = rowHTML;
                    if (rowHTML == "" && fillGridRowHTML != "")
                        dcRowone = fillGridRowHTML;
                }
                dcOtherRows.Append(dcRowone);
                serialNo++;
            }
        }
        //int rowCount = rowCnt + 1;       
        tabHTML = GetFullTabDCHTML(dcIndNo, dcNo, dcOtherRows.ToString(), isTab, rowCnt.ToString(), fillGriddcRowone);

        return tabHTML;
    }

    //This function returns the dhtml and thier pop grid dc's html.
    public string GetTabDcHTML(int dcNo, TStructData tstDataObj, string IsTabDisabled)
    {
        int dcIndNo = dcsPositionIndex.IndexOf(dcNo.ToString());
        if (tstDataObj.recordID.ToString() != "0" && ((DcStruct)(dcs[dcIndNo])).isgrid)
        {
            IsFillGridCall = true;
        }
        string result = string.Empty;
        //To check if the tab dc contains any pop grid and store the popGrid dc number in subGridDcs.
        ArrayList subGridDcs = new ArrayList();
        for (int i = 0; i < popdcs.Count; i++)
        {
            if (dcNo == ((PopDcStruct)(popdcs[i])).pdcno)
            {
                subGridDcs.Add(((PopDcStruct)(popdcs[i])).frameno);
            }
        }

        StringBuilder popGridDcHTML = new StringBuilder();
        for (int j = 0; j < subGridDcs.Count; j++)
        {
            int rowCnt = GetRowCount(Convert.ToInt32(subGridDcs[j]), tstDataObj);
            if (popGridDcHTML.ToString() == "")
                popGridDcHTML.Append(subGridDcs[j] + "♣" + rowCnt + "*?*" + GetTabFullHTML(Convert.ToInt32(subGridDcs[j]), rowCnt, tstDataObj, "false", IsTabDisabled));
            else
                popGridDcHTML.Append("*Tab*" + subGridDcs[j] + "♣" + rowCnt + "*?*" + GetTabFullHTML(Convert.ToInt32(subGridDcs[j]), rowCnt, tstDataObj, "false", IsTabDisabled));
            dcOtherRows = new StringBuilder();
        }

        colCount = 0;
        //If HasDataRows=no flRowCnt should be 0 else should be total row count
        int flRowCnt = 0;
        if (IsFillGridCall)
        {
            if (((DcStruct)(dcs[dcIndNo])).DCHasDataRows)
                flRowCnt = GetRowCount(dcNo, tstDataObj);
        }
        else
            flRowCnt = GetRowCount(dcNo, tstDataObj);

        //Construct the result in the format of dcno♣rowCount + "*?*" + TabDcHTML + "*Tab*" + popGridDcHTMl + "*Tab*" + next pop grid dcs...        
        int rCnt = GetRowCount(dcNo, tstDataObj);

        string isTab = IsDcTab(dcNo);
        result = dcNo + "♣" + flRowCnt + "*?*" + GetTabFullHTML(dcNo, rCnt, tstDataObj, isTab, IsTabDisabled) + "*Tab*" + popGridDcHTML;
        dcOtherRows = new StringBuilder();
        if (tstDataObj.recordID.ToString() != "0" && ((DcStruct)(dcs[dcIndNo])).isgrid)
        {
            IsFillGridCall = false;
        }
        return result;
    }

    private string IsDcTab(int dcNo)
    {
        string isTab = "false";

        for (int i = 0; i < pagePositions.Count; i++)
        {
            if (pagePositions[i].ToString().Contains(","))
            {
                string[] tabContainers = pagePositions[i].ToString().Split(',');
                for (int j = 0; j < tabContainers.Length; j++)
                {
                    if (tabContainers[j].ToString() == dcNo.ToString())
                    {
                        isTab = "true";
                        break;
                    }
                }
                if (isTab == "true")
                    break;
            }
        }
        return isTab;
    }

    public int GetRowCount(int dcNo, TStructData tstData)
    {
        int rCnt = 0;
        if (tstData.dcRowCntVals != null)
        {
            for (int i = 0; i < tstData.dcRowCntVals.Count; i++)
            {

                string[] dcNumber = tstData.dcRowCntVals[i].ToString().Split('~');
                //dcNumber[0] = dcNumber[0].Replace("dc", "");
                string arrDcNo = dcNumber[0].Substring(2);
                if (arrDcNo == dcNo.ToString())
                {
                    rCnt = Convert.ToInt32(dcNumber[1]);
                    break;
                }
            }
        }

        if (rCnt == 0)
            rCnt = 1;

        return rCnt;
    }

    /// <summary>
    /// function to construct the field html for a particular dc. It doesnot contains the outer
    /// part of the DC like outer border, DC header, ADD, Fill.
    /// It iterates throug the complete fields, check whether it belongs the DC by checking the
    /// frame no. Then it constructs the HTML string for that field and appended to the DC HTML.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <seealso cref="GetFinalHtml"/>
    /// <returns> returns the field html. </returns>   
    private string GetTabFieldHtml(int dcIndNo, int dcNo, TStructData tstData, string IsTabDisabled, string rowNo, string keyColValue, ArrayList keyCols, ArrayList keyRowNos)
    {
        //NOTE: Assigning javascript functions for grid and non-grid fields's is done seperately,
        //as the grid field javascript functions are written into the hidden field through the variable gridHiddenHtml. 
        //The difference for grid functions is usage of \"+quo+\" instead of \".   
        //The hidden field is used to store the dummy row value, 
        //the value assigned to hidden field will be written within double quotes and hence we use \"+quo+\" to write a double quote in the HTML.
        string divDirection = "left";
        if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
        {
            divDirection = "right";
        }

        //Boolean isGrid = ((DcStruct)(dcs[dcNo - 1])).isgrid;
        Boolean isGrid = ((DcStruct)(dcs[dcIndNo])).isgrid;
        //DcStruct dc = (DcStruct)(dcs[dcNo - 1]);
        DcStruct dc = (DcStruct)(dcs[dcIndNo]);
        if (isGrid)
        {
            dcArrayNo++;
        }
        int fcwidth = 0;
        string colFldCaption = "", colFldWidth = "", colFldGridStackItem = "", colDivInputPadding = "";
        if (!isGrid && (axdesignJObject.dcLayout != null && axdesignJObject.dcLayout != "" && axdesignJObject.dcLayout != "default"))
        {
            fcwidth = int.Parse(axdesignJObject.fieldCaptionWidth);
            fcwidth = fcwidth / 10;
            colFldCaption = "col-sm-" + fcwidth + "";
            fcwidth = 12 - fcwidth;
            colFldWidth = "<div class=\"input-group col-sm-" + fcwidth + "\">";
            colFldGridStackItem = " colFldGridStackWidth ";
            colDivInputPadding = " colDivInputPadding ";
        }

        if (colFldWidth == "")
            colFldWidth = "<div class=\"input-group\">";

        dummyGridHeadHtml = new StringBuilder();
        gridDcColwidth = 0;
        gridHeadHtml = new StringBuilder();
        gridHiddenHtml = new StringBuilder();
        StringBuilder hiddenFldsHtml = new StringBuilder();
        Boolean isLastDcDate = false;
        StringBuilder fieldHtml = new StringBuilder();
        lastDcHeight = 0;
        int gridInSlackGridX = 0;
        int gridInSlackGridY = 0;
        int randomID = 10;
        string colSpan = string.Empty;
        string gridStackData = string.Empty;
        string lblBsClass = string.Empty;
        string lblBsIndex = string.Empty;
        string fldBsClass = string.Empty;
        int rndnumber = 0;
        Random randNum = new Random();
        rndnumber = randNum.Next(100000, 1000000);

        string dcFldRange = GetDcFieldRange(dcNo.ToString());
        string[] fldRange = dcFldRange.Split(',');

        string layoutstyle = string.Empty;
        if (HttpContext.Current.Session["layoutstyle"] != null)
        {
            layoutstyle = HttpContext.Current.Session["layoutstyle"].ToString();
        }
        if (fldRange.Length > 1)
        {
            int startIndex = Convert.ToInt32(fldRange[0]);
            int endIndex = Convert.ToInt32(fldRange[1]);
            DataRow ThisRow = tstData.DSGetRow(rowNo, dcNo);
            int top = -1;


            for (int i = startIndex; i <= endIndex; i++)
            {
                FieldStruct fld = (FieldStruct)flds[i];
                //As the following code is going to be called for each field,
                //its not made as method to avoid function call.
                string[] tlhw;
                if (fld.lbltlhw.ToString() != "")
                {
                    tlhw = fld.lbltlhw.ToString().Split(',');

                    tlhw[0] = (Convert.ToInt32(tlhw[0].ToString())).ToString();
                    tlhw[1] = (Convert.ToInt32(tlhw[1].ToString())).ToString();
                    tlhw[2] = (Convert.ToInt32(tlhw[2].ToString())).ToString();
                    tlhw[3] = (Convert.ToInt32(tlhw[3].ToString())).ToString();

                }
                else
                {
                    tlhw = ",,,".Split(','); // Hack: directly entering the data to the tlhw throws error
                    tlhw[0] = "";
                    tlhw[1] = "";
                    tlhw[2] = "";
                    tlhw[3] = "";
                }




                //this function is For Label Percentage
                string[] ptlhw;
                if (fld.plbltlhw.ToString() != "")
                {
                    ptlhw = fld.plbltlhw.ToString().Split(',');
                    if (!string.IsNullOrEmpty(ptlhw[3].ToString()))
                    {
                        float percentage = float.Parse(ptlhw[3].ToString());
                        // lblBsClass = ConvertFieldToLayout(layoutstyle, i);
                    }
                }

                //this function is for field Percentage


                string[] pfldTlhw;
                if (fld.pfldtlhw.ToString() != "")
                {
                    pfldTlhw = fld.pfldtlhw.ToString().Split(',');

                    //fldTlhw[0] = Math.Floor((Convert.ToInt32(fldTlhw[0].ToString()) * 1.2)).ToString();
                    //fldTlhw[1] = Math.Floor((Convert.ToInt32(fldTlhw[1].ToString()) * 1.0)).ToString();

                    if (!string.IsNullOrEmpty(pfldTlhw[3].ToString()))
                    {
                        float percentage = float.Parse(pfldTlhw[3].ToString());
                        //fldBsClass = findGridcol(percentage).ToString();
                    }

                }



                string[] fldTlhw;
                if (fld.fldtlhw.ToString() != "")
                {
                    fldTlhw = fld.fldtlhw.ToString().Split(',');

                    //fldTlhw[0] = Math.Floor((Convert.ToInt32(fldTlhw[0].ToString()) * 1.2)).ToString();
                    //fldTlhw[1] = Math.Floor((Convert.ToInt32(fldTlhw[1].ToString()) * 1.0)).ToString();



                }
                else
                {
                    fldTlhw = ",,,".Split(','); // Hack: directly entering the data to the tlhw throws error
                    fldTlhw[0] = "0";
                    fldTlhw[1] = "0";
                    fldTlhw[2] = "0";
                    fldTlhw[3] = "0";
                }

                if (fld.fldlength != 0)
                {

                    lblBsClass = bsClassByDatawidth(fld.fldlength, i, fld);
                    lblBsIndex = fld.fieldIndex.ToString();
                }

                if (!fld.visibility)
                    gridStackData = xywhGridStack(fld);

                string name = string.Empty;
                string hiddenName = string.Empty;
                string width = string.Empty;
                string fldValue = string.Empty;
                string comboValues = string.Empty;
                string idCol = string.Empty;
                string fldNAME = fld.name.ToUpper();
                string fdwidth = "0";
                //fdwidth = (fld.fldlength > 51 ? 400 : (fld.fldlength <= 15) ? 150 : (fld.fldlength * 8)).ToString();
                if (!fld.visibility)
                    fdwidth = xywhGridStack(fld, true);
                if (isGrid)
                {
                    if (rowNo != "")
                        name = fld.name + rowNo + "F" + fld.fldframeno;
                    else
                        name = fld.name + "001F" + fld.fldframeno;
                    hiddenName = fld.name + "\" +nrno+\"";
                    // for grid its column width
                    width = fld.cwid;
                    if (!fld.visibility)
                    {
                        if (width != "")
                            gridDcColwidth += Convert.ToInt32(width);
                        else
                            gridDcColwidth += 1;
                    }
                }
                else
                {
                    name = fld.name + "000F" + fld.fldframeno;
                    width = fldTlhw[3];
                }


                if (width == "")
                    width = "0";
                else
                    width = Convert.ToString(Convert.ToInt32(width) - 4);
                if (ThisRow != null)
                {
                    if (dc.IsFormatGrid && dc.KeyColumn == fld.name)
                    {
                        if (keyColValue != "")
                            ActiveKeyColValue = keyColValue;
                        else
                            ActiveKeyColValue = ThisRow[fld.name].ToString();
                    }
                    fldValue = ThisRow[fld.name].ToString();
                    if (fldValue.Contains("\""))
                        fldValue = fldValue.Replace("\"", "&quot;");
                    DataRow ThisFieldDropDown = tstData.DSGetDropdownRow(fld.name, rowNo);

                    if (ThisFieldDropDown == null)
                    {
                        ThisFieldDropDown = tstData.DSGetDropdownRow(fld.name, "1");
                        if (ThisFieldDropDown != null && ThisFieldDropDown["mr"].ToString() == "0") ThisFieldDropDown = null;

                    }

                    if (ThisFieldDropDown != null)
                        comboValues = ThisFieldDropDown["rowvals"].ToString();

                    if (comboValues != "")
                    {
                        if (fld.savenormal)
                            fld.fldIdCols = "1";
                        else
                            fld.fldIdCols = "0";

                        fld.fldComboValues = comboValues;
                    }
                    if ((fld.datatype == "Numeric") && (fld.applycomma == "True") && (fldValue != null) && (fldValue != ""))
                    {
                        if (fld.flddecimal > 0)
                        {
                            try
                            {
                                double newFldValue = Convert.ToDouble(fldValue);
                                //fldValue = newFldValue.ToString("#,###0.00", System.Globalization.CultureInfo.InvariantCulture);
                                string decmalFormat = "#,###0.";
                                for (int decNo = 0; decNo < fld.flddecimal; decNo++)
                                {
                                    decmalFormat = decmalFormat + "0";
                                }
                                fldValue = newFldValue.ToString(decmalFormat, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex) { }
                        }
                    }

                    fld.Value = fldValue;
                }
                else
                {
                    if (dc.IsFormatGrid && dc.KeyColumn == fld.name)
                    {
                        ActiveKeyColValue = fldValue;
                        fld.Value = fldValue;
                    }
                }


                if (fld.visibility)
                {
                    if (name.StartsWith("axp_recid") && fld.Value == "")
                        fld.Value = "0";
                    string cssAutoGen = string.Empty;
                    if (fld.moe == "AutoGenerate")
                        cssAutoGen = "class=\"autogen\"";
                    string ddetails = String.Empty;
                    //if (fld.tooltip != null && fld.tooltip != "")
                    //    ddetails = "<span class=\"material-icons material-icons-style align-middle material-icons-3 cursor-pointer spanddetails\" title=\"Display details\">info</span>";
                    if (name.StartsWith("axp_recid"))
                    {
                        fieldHtml.Append("<div class=\"gridElement d-none form-group\" id=\"dvGrid" + fld.name.Replace(" ", "") + "\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + name + "\" type=\"hidden\" value=\"0\" name=\"" + name + "\" value=\"" + fld.Value + "\" /></div></div>");
                        if (isGrid)
                        {
                            gridHiddenHtml.Append("<div class=\" grid-stack-item form-group\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + hiddenName + "\" type=hidden value=0 name=\"" + hiddenName + "\" /></div></div>");
                            gridHeadHtml.Append("<th style=\"display:none;\" id=\"th-" + fld.name + "\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead'>" + ddetails + "</div></th>");
                        }
                    }
                    else
                    {
                        if (fld.name.ToLower().StartsWith("axpfilepath_") && cssAutoGen == "")
                        {
                            var endPart = name.Substring(12);
                            cssAutoGen = "class=\"axpFilePath_" + endPart + " axpFilePathFld \"";
                        }
                        if (fld.moe == "Select")
                            fieldHtml.Append("<div class=\"gridElement form-group d-none\" id=\"dvGrid" + fld.name.Replace(" ", "") + "\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + name + "\" type=\"hidden\" name=\"" + name + "\" " + cssAutoGen + " value=\"" + fld.Value + "\" data-val=\"" + fld.moeval + "\"/></div></div>");
                        else
                            fieldHtml.Append("<div class=\"gridElement form-group d-none\" id=\"dvGrid" + fld.name.Replace(" ", "") + "\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + name + "\" type=\"hidden\" name=\"" + name + "\" " + cssAutoGen + " value=\"" + fld.Value + "\" /></div></div>");
                        if (isGrid)
                        {
                            gridHiddenHtml.Append("<div class=\" grid-stack-item form-group\"><div class=\"fldhdn" + i + "\"><INPUT id=\"" + hiddenName + "\" type=\"hidden\" name=\"" + hiddenName + "\" " + cssAutoGen + " /></div></div>");
                            gridHeadHtml.Append("<th style=\"display:none;\" id=\"th-" + fld.name + "\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead'>" + ddetails + "</div></th>");
                        }

                    }
                }
                else
                {
                    randomID++;
                    string popImg = string.Empty;
                    string popImgHidden = string.Empty;
                    int popwidth = 0;
                    for (int x = 0; x < popdcs.Count; x++)
                    {
                        if (dc.frameno == ((PopDcStruct)(popdcs[x])).pdcno)
                        {
                            if (fld.name.ToLower() == ((PopDcStruct)(popdcs[x])).popfield.ToLower())
                            {
                                string popRowNo = rowNo;
                                if (popRowNo == "") popRowNo = "001";

                                popImgHidden = "  <div alt='Open' class='subGrid glyphicon glyphicon-new-window icon-basic-elaboration-browser-plus' id='" + popRowNo + "F" + ((PopDcStruct)(popdcs[x])).frameno + "'></div>";
                                popImg = "  <div alt='Open' class='subGrid glyphicon glyphicon-new-window icon-basic-elaboration-browser-plus' id='" + popRowNo + "F" + ((PopDcStruct)(popdcs[x])).frameno + "'></div>";
                                popwidth = Convert.ToInt32(width) - 20;
                                break;
                            }
                        }
                    }


                    if (!isGrid)
                    {
                        if (fldTlhw[0] != "")
                            lastDcHeight = Convert.ToInt32(fldTlhw[0]) + Convert.ToInt32(fldTlhw[2]);
                        else
                            lastDcHeight = Convert.ToInt32(fldTlhw[2]);
                    }
                    bool isHyper = false;
                    int n = 0;
                    for (n = 0; n <= hlnks.Count - 1; n++)
                    {
                        if (fld.name == ((HLinkStruct)(hlnks[n])).hlsource.Substring(3))
                        {
                            isHyper = true;
                            break;
                        }
                    }
                    string designFontStyle = "";
                    string designHyperLink = "";
                    if (!isGrid)
                    {
                        if (axdesignJObject.buttonFieldFont != null && axdesignJObject.buttonFieldFont.Count > 0)
                        {
                            var frbtnfld = axdesignJObject.buttonFieldFont.Where(elm => elm.id == name).ToList();
                            if (frbtnfld != null && frbtnfld.Count > 0)
                            {
                                designFontStyle = frbtnfld[0].fontFamilly;
                                designHyperLink = frbtnfld[0].hyperlinkJson;
                            }
                        }
                    }

                    if (isGrid)
                    {
                        string ddetails = String.Empty;
                        //if (fld.tooltip != null && fld.tooltip != "")
                        //    ddetails = "<span class=\"material-icons material-icons-style align-middle material-icons-3 cursor-pointer spanddetails\" title=\"Click on any row in this column to know more about the data in the cell.\">info</span>";
                        string allowemptycss = "";
                        if (!fld.allowempty && fld.caption != "")
                            allowemptycss = " required ";
                        colCount++;
                        if (fld.caption.Contains("~"))
                        {
                            gridHeadHtml.Append("<th id=\"th-" + fld.name + "\" style=\"width:" + fdwidth + "px;\" class=\"wordBreak fw-boldest\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead " + allowemptycss + "'>");
                        }
                        else
                        {
                            gridHeadHtml.Append("<th id=\"th-" + fld.name + "\" style=\"width:" + fdwidth + "px;\" class=\"fw-boldest\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead " + allowemptycss + "'>");
                        }

                        if (isHyper)
                        {
                            gridHeadHtml.Append("<a href=\"javascript:void(0)\" class=\"cursor-pointer\" id=\"" + ((HLinkStruct)(hlnks[n])).hlsource.ToString() + "\" onclick='javascript:Tstructhyperlink(this);'>");
                        }
                        if (fldNAME.IndexOf("AXPBUTTON") == -1)
                        {
                            if (fld.caption.Contains("~"))
                                fld.caption = fld.caption.Replace("~", "\n");

                            gridHeadHtml.Append(fld.caption);
                        }
                        if (isHyper)
                        {
                            gridHeadHtml.Append("</a>");
                        }

                        // to handle the mandatory fields.
                        //if (!fld.allowempty && fld.caption != "")
                        //    gridHeadHtml.Append("<span class=\"allowempty\">*</span>");

                        //if (!string.IsNullOrEmpty(fld.fieldPurpose))
                        //    gridHeadHtml.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + fld.fieldPurpose + "\">help_outline</i></span>");
                        gridHeadHtml.Append(ddetails + "</div></th>");
                    }
                    else
                    {
                        if (fld.ctype.ToUpper() != "CHECK BOX")
                        {
                            fieldHtml.Append("<div class=' grid-stack-item" + colFldGridStackItem + "' " + gridStackData + " id=\"randomID_" + dc.frameno + randomID + "\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"labelcol inputs form-group row " + lblBsClass + " " + colDivInputPadding + " \" data-dataindex=" + lblBsIndex + ">");
                            if ((fld.allowempty && fld.caption != "") && (string.IsNullOrEmpty(fld.fieldPurpose)))
                                fieldHtml.Append("<div class=\"fld-wrap3 " + colFldCaption + "\" >");
                            else if (!fld.allowempty && fld.caption != "")
                                fieldHtml.Append("<div class=\"fld-wrap3 required " + colFldCaption + "\" >");
                            else
                                fieldHtml.Append("<div class=\"fld-wrap3 " + colFldCaption + "\" >");
                            if (isHyper)
                                fieldHtml.Append("<a href=\"javascript:void(0)\" class=\"cursor-pointer\" id=\"" + ((HLinkStruct)(hlnks[n])).hlsource.ToString() + "\" onclick='javascript:Tstructhyperlink(this);'>");
                            else if (designHyperLink != "")
                                fieldHtml.Append("<a href=\"javascript:void(0)\" class=\"cursor-pointer\" id=\"hylnk" + fld.name + "\" onclick='javascript:TstructLabelhyperlink(this);' data-param='" + designHyperLink + "'>");

                            if (designHyperLink != "")
                                fieldHtml.Append("<label class=\"form-label col-form-label pb-1 fw-boldest \" style=\"text-decoration:underline;cursor: pointer;" + designFontStyle + "\"  for=\"" + name + "\" >" + fld.caption + "</label >");
                            else
                                fieldHtml.Append("<label class=\"form-label col-form-label pb-1 fw-boldest \" style=\"" + designFontStyle + "\" for=\"" + name + "\">" + fld.caption + "</label >");

                            if (isHyper || designHyperLink != "")
                                fieldHtml.Append("</a>");

                            // to handle the mandatory fields.
                            //if (!fld.allowempty && fld.caption != "")
                            //    fieldHtml.Append("<span class=\"allowempty\">*</span>");
                            //if (!string.IsNullOrEmpty(fld.fieldPurpose))
                            //    fieldHtml.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + fld.fieldPurpose + "\">help_outline</i></span>");
                            fieldHtml.Append("</div>");
                        }
                        else if (fld.datatype.ToUpper() == "DATE/TIME")
                            fieldHtml.Append("<div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"  input-group grid-stack-item " + lblBsClass + "\"  data-dataindex=\"" + lblBsIndex + "\" " + gridStackData + "><div class=\"grid-stack-item-content ui-draggable-handle\"></div>");
                        else if (fld.ctype.ToUpper() == "CHECK BOX")
                        {
                            if (HttpContext.Current.Session["switchboxToCheckbox"] != null && HttpContext.Current.Session["switchboxToCheckbox"].ToString().ToLower() == "true")
                            {
                                fieldHtml.Append("<div class=' grid-stack-item" + colFldGridStackItem + "'  " + gridStackData + " id=\"randomID_" + dc.frameno + randomID + "\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"labelcol inputs checkbox form-group row " + lblBsClass + " " + colDivInputPadding + "  justify-content-start flex-nowrap flex-row-reverse align-items-center mt-7 \" data-dataindex=" + lblBsIndex + " >");
                                fieldHtml.Append("<div class=\"fld-wrap3 flex-shrink-1 ps-0 " + colFldCaption + /*(!columnModeEnabled ? "" : " d-none ")*/ "  " + "\"><label class=\"form-label col-form-label pb-1 fw-boldest\" style=\"cursor: default;\" for=\"" + name + "\">" + fld.caption + "</label></div>");
                            }
                            else
                            {
                                fieldHtml.Append("<div class=' grid-stack-item" + colFldGridStackItem + "'  " + gridStackData + " id=\"randomID_" + dc.frameno + randomID + "\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"labelcol inputs checkbox form-group row " + lblBsClass + " " + colDivInputPadding + " \" data-dataindex=" + lblBsIndex + " >");
                                fieldHtml.Append("<div class=\"fld-wrap3 " + colFldCaption + /*(!columnModeEnabled ? "" : " d-none ")*/ "  " + "\"><label class=\"form-label col-form-label pb-1 fw-boldest\" style=\"cursor: default;\" for=\"" + name + "\">" + fld.caption + "</label></div>");
                            }
                        }
                        else
                        {
                            fieldHtml.Append("<div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"labelcol inputs form-group row form  grid-stack-item \"   " + gridStackData + "><div class=\"grid-stack-item-content ui-draggable-handle\"></div>");
                        }
                    }

                    if (isGrid)
                    {
                        if (!fld.visibility)
                            gridStackData = xywhGridStack(fld);
                        //label normal
                        fieldHtml.Append("<div class=\"gridElement form-group grid-stack-item\"   " + gridStackData + "  id=\"dvGrid" + fld.name.Replace(" ", "") + "\" style=\"\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div class=\"grid-stack-item-content\">");
                        gridInSlackGridX += 4;
                        if (gridInSlackGridX % 12 == 0)
                        {
                            gridInSlackGridX = 0;
                            gridInSlackGridY += 1;
                        }
                        string[] moeValue;
                        if (fld.moeval != null)
                            moeValue = fld.moeval.ToString().Split(',');
                        else
                            moeValue = "".Split();
                        bool chkBox = false;
                        chkBox = IsGridCheckBox(moeValue);
                        if (!chkBox)
                        {

                            if ((fld.allowempty && fld.caption != "") && (string.IsNullOrEmpty(fld.fieldPurpose)))
                                fieldHtml.Append("<div class=\"fld-wrap3\" >");
                            else
                                fieldHtml.Append("<div class=\"fld-wrap3 required\">");
                            fieldHtml.Append("<label>" + fld.caption + "</label>");

                            //if (!string.IsNullOrEmpty(fld.fieldPurpose))
                            //    fieldHtml.Append("<span><i tabindex=\"-1\" data-bs-trigger=\"hover\" class=\"icon-arrows-question material-icons material-icons-style material-icons-4 align-middle ms-2 cursor-pointer\" id=\"ico_cl\" data-bs-toggle=\"tooltip\" data-bs-placement=\"right\" data-bs-dismiss=\"click\" data-bs-original-title=\"" + fld.fieldPurpose + "\">help_outline</i></span>");
                            fieldHtml.Append("</div>");//closing for fld-wrap2 and closing for fld-wrap1
                        }

                        if (popImg != "")
                        {
                            fieldHtml.Append("<div>");
                        }
                        else
                        {
                            fieldHtml.Append("<div >");
                        }
                        gridHiddenHtml.Append("<div class=\"col-md-4 col-lg-4 col-sm-4 gridElement form-group\"><div>");

                    }
                    else
                    {
                        if (fld.moe.ToUpper() == "SELECT" && fld.selectmode.ToUpper() == "FROM MASTER" && fld.editcombo == true)
                            fieldHtml.Append("<span id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"picklist  input-group \">");

                    }

                    if (popwidth > 0)
                        width = popwidth.ToString();
                    string addZeroForNumeric = string.Empty;
                    if (isGrid && ShowZeroForNumeric && (fld.datatype.ToUpper() == "NUMERIC"))
                        addZeroForNumeric = " value=0 ";

                    string fldCType = fld.ctype.ToUpper();
                    string fldMOE = fld.moe.ToUpper();

                    // generate html depends on the mode of entry, like text box, check box, radio button
                    switch (fldMOE)
                    {
                        // accept- simple input type which accepts the user input, mostly except image.
                        case "ACCEPT":
                            if (!isGrid && fld.datatype.ToUpper() != "IMAGE")
                                fieldHtml.Append(colFldWidth);
                            SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                            if (fldCType == "MEMO" || fld.datatype.ToLower() == "text" && fld.ctype.ToUpper() != "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "memotem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (!isGrid)
                                    width = Convert.ToString(Convert.ToInt32(width) - 10);

                                if (isGrid)
                                    gridHiddenHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                                fieldHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                            }
                            else if (fldCType == "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "checkbox", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                                fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                            }
                            else if (fld.datatype.ToUpper() == "IMAGE")
                            {
                                if (isGrid)
                                    gridHiddenHtml.Append(GetImageHTML(fld, isGrid, name, i, fldTlhw, width, fcwidth));
                                fieldHtml.Append(GetImageHTML(fld, isGrid, name, i, fldTlhw, width, fcwidth));
                            }
                            else if (fldNAME.ToUpper().IndexOf("AXPBUTTON") != -1)
                            {
                                if (isGrid)
                                {
                                    gridHiddenHtml.Append(GetAxpBtnHTML(fld, isGrid, name, width));
                                }
                                fieldHtml.Append(GetAxpBtnHTML(fld, isGrid, name, width));
                            }
                            else if (fld.datatype.ToUpper() == "DATE/TIME")
                            {

                                if (isGrid)
                                {
                                    width = (Convert.ToInt32(width) - 5).ToString();
                                    gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));

                                }
                                else
                                {
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                            }

                            else
                            {
                                if ((name.StartsWith("axp_gridattach")) || (fld.name.StartsWith("dc") && fld.name.ToLower().EndsWith("_image")) || (fld.name.ToLower().StartsWith("axpfile_") && isGrid))//(name.ToLower().StartsWith("dc" + dcNo + "_image")))
                                {
                                    string tmpValue = string.Empty;
                                    try
                                    {
                                        tmpValue = ThisRow["axpattach_filename" + fld.fldframeno.ToString()].ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        tmpValue = "";
                                    }
                                    if (tmpValue != "" && fld.Value != "")
                                    {
                                        string sfileEx = fld.Value.Substring(fld.Value.LastIndexOf("."));
                                        fld.Value = tmpValue + sfileEx;
                                    }

                                    if (isGrid)
                                        gridHiddenHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                    fieldHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                }
                                else
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                            }
                            break;
                        case "SELECT":
                            if (!isGrid)
                                fieldHtml.Append(colFldWidth);
                            string[] moeValue;
                            if (!isGrid)
                                width = Convert.ToString(Convert.ToInt32(width) + 5);

                            if (fld.moeval != null)
                                moeValue = fld.moeval.ToString().Split(',');
                            else
                                moeValue = "".Split();

                            if (fldCType == "CHECK LIST" && !isGrid)
                            {
                                SetCssClass(fld.freadonly, "multiFldChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckListHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                                fieldHtml.Append(GetCheckListHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                            }
                            else if (fldCType == "RADIO GROUP" && !isGrid)
                            {
                                SetCssClass(fld.freadonly, "multiFld", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (!isGrid)
                                {
                                    fieldHtml.Append(GetRadioGroupHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                                    gridHiddenHtml.Append(GetRadioGroupHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));

                                }

                            }
                            else if (fld.selectmode == string.Empty)
                            {
                                bool chkBox = false;
                                chkBox = IsGridCheckBox(moeValue);
                                SetCssClass(fld.freadonly, "combo", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                if (isGrid)
                                {
                                    if (chkBox)
                                    {
                                        SetCssClass(fld.freadonly, "gridChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                        width = Convert.ToString(Convert.ToInt32(width) - 10);
                                        gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                        fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));

                                    }
                                    else
                                    {
                                        if (isGrid)
                                            gridHiddenHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                        fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                    }
                                }
                                else
                                {
                                    fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                }
                            }
                            else if (fld.selectmode.ToUpper() == "FROM MASTER")
                            {
                                SetCssClass(fld.freadonly, "combo", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                if (!fld.editcombo)
                                {
                                    // Adding checkbox to GridDC                                       
                                    bool chkBox = false;
                                    chkBox = IsGridCheckBox(moeValue);

                                    if (isGrid)
                                    {
                                        if (chkBox)
                                        {
                                            SetCssClass(fld.freadonly, "gridChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                            gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                            fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                        }
                                        else
                                        {

                                            gridHiddenHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                            if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                            {
                                                fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                                fromListFlds += "," + fld.name;
                                            }
                                            else
                                                fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                        }
                                    }
                                    else
                                    {
                                        if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                        {
                                            fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                            fromListFlds += "," + fld.name;
                                        }
                                        else
                                            fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                    }
                                }
                                else
                                {
                                    SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);


                                    if (isGrid)
                                    {
                                        gridHiddenHtml.Append(GetPickListHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden, true));
                                    }

                                    fieldHtml.Append(GetPickListHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden, false));

                                }
                            }
                            else
                            {
                                SetCssClass(fld.freadonly, "combo", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                bool chkBox = false;
                                chkBox = IsGridCheckBox(moeValue);

                                if (isGrid)
                                {
                                    if (chkBox)
                                    {
                                        SetCssClass(fld.freadonly, "gridChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                        width = Convert.ToString(Convert.ToInt32(width) - 10);

                                        gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                        fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));

                                    }
                                    else
                                    {
                                        if (isGrid)
                                            gridHiddenHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                        if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                        {
                                            fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                            fromListFlds += "," + fld.name;
                                        }
                                        else
                                            fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                    }
                                }
                                else
                                {
                                    if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                    {
                                        fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                        fromListFlds += "," + fld.name;
                                    }
                                    else
                                        fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));

                                }
                            }
                            break;
                        case "AUTOGENERATE":
                            if (!isGrid)
                                fieldHtml.Append(colFldWidth);
                            SetCssClass(fld.freadonly, "auto", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);


                            if (isGrid)
                                gridHiddenHtml.Append(GetAutoGenFieldHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden));
                            fieldHtml.Append(GetAutoGenFieldHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden));

                            break;
                        case "FILL":
                            if (!isGrid)
                                fieldHtml.Append(colFldWidth);
                            SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                            if (fldCType == "MEMO" || fld.datatype.ToLower() == "text" && fld.ctype.ToUpper() != "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "memotem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (!isGrid)
                                    width = Convert.ToString(Convert.ToInt32(width) - 8);

                                if (isGrid)
                                    gridHiddenHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                                fieldHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));

                            }

                            else if (fldCType == "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "checkbox", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                                fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                            }

                            else
                            {
                                if (fld.name.ToLower().StartsWith("axpfile_") && isGrid)
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                    fieldHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                }
                                else
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                            }
                            break;
                        case "CALCULATE":
                            if (!isGrid)
                                fieldHtml.Append(colFldWidth);
                            SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                            if (fldCType == "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "checkbox", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));

                            }
                            else
                            {

                                if (isGrid)
                                    gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));

                            }
                            break;
                    }

                    if (isGrid)
                    {
                        gridHiddenHtml.Append("</div></div>");
                        fieldHtml.Append("</div></div></div>");

                        if (fldMOE == "SELECT" || (fldNAME.ToUpper().IndexOf("AXPBUTTON") != -1) || (popwidth > 0))
                        {
                            if (width != "")
                            {
                                if (popwidth > 0)
                                    width = fld.cwid;

                                width = (Convert.ToInt32(width) - 4).ToString();
                            }
                        }
                        dummyGridHeadHtml.Append("<TD class=agrow1 align=center ><INPUT type=\"text\" disabled  style=\"width:" + width + "px;height:0px;\" class=agrowGfrow onfocus=this.blur(); /></TD>");
                    }
                    else
                    {
                        //fieldHtml.Append(" </div>");
                        if (fld.moe.ToUpper() == "SELECT" && fld.selectmode.ToUpper() == "FROM MASTER" && fld.editcombo == true)
                            fieldHtml.Append("</span>");
                        fieldHtml.Append("</div></div>");
                        if (colFldWidth != "" && fld.datatype.ToUpper() != "IMAGE")
                            fieldHtml.Append("</div>");
                        fieldHtml.Append(GetCustLabelHTML(fld.name, fld.fldFrameNo));
                    }

                    if (isLastDcDate)
                        lastDcHeight = lastDcHeight + 75;
                }
            }
        }
        if (!isGrid)
            fieldHtml.Append(GetCustLabelHTML("", dcNo));
        if (isGrid && hiddenFldsHtml.ToString() != string.Empty) fieldHtml.Append("<td>" + hiddenFldsHtml.ToString() + "</td>");
        return fieldHtml.ToString();
    }





    /// <summary>
    /// function to construct the FillGridData html . It doesnot contains the outer
    /// part of the DC like outer border, DC header, ADD, Fill.
    /// It iterates throug the complete fields, check whether it belongs the DC by checking the
    /// frame no. Then it constructs the HTML string for that FillGrid and appended to the DC HTML.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <seealso cref="GetFinalHtml"/>
    /// <returns> returns the field html. </returns>
    private string GetFillGridDataFieldHtml(int dcIndNo, int dcNo, TStructData tstData, string IsTabDisabled, string rowNo, string keyColValue, ArrayList keyCols, ArrayList keyRowNos)
    {
        //NOTE: Assigning javascript functions for grid and non-grid fields's is done seperately,
        //as the grid field javascript functions are written into the hidden field through the variable gridHiddenHtml. 
        //The difference for grid functions is usage of \"+quo+\" instead of \".   
        //The hidden field is used to store the dummy row value, 
        //the value assigned to hidden field will be written within double quotes and hence we use \"+quo+\" to write a double quote in the HTML.
        string divDirection = "left";
        if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
        {
            divDirection = "right";
        }

        Boolean isGrid = ((DcStruct)(dcs[dcIndNo])).isgrid;
        DcStruct dc = (DcStruct)(dcs[dcIndNo]);
        if (isGrid)
        {
            dcArrayNo++;
        }
        dummyGridHeadHtml = new StringBuilder();
        gridDcColwidth = 0;
        gridHeadHtml = new StringBuilder();
        gridHiddenHtml = new StringBuilder();
        StringBuilder hiddenFldsHtml = new StringBuilder();
        Boolean isLastDcDate = false;
        StringBuilder fieldHtml = new StringBuilder();
        lastDcHeight = 0;
        int randomID = 10;
        string colSpan = string.Empty;
        string gridStackData = string.Empty;
        string lblBsClass = string.Empty;
        string lblBsIndex = string.Empty;
        string fldBsClass = string.Empty;
        int rndnumber = 0;
        Random randNum = new Random();
        rndnumber = randNum.Next(100000, 1000000);

        string dcFldRange = GetDcFieldRange(dcNo.ToString());
        string[] fldRange = dcFldRange.Split(',');

        string layoutstyle = string.Empty;
        if (HttpContext.Current.Session["layoutstyle"] != null)
        {
            layoutstyle = HttpContext.Current.Session["layoutstyle"].ToString();
        }
        if (fldRange.Length > 1)
        {
            int startIndex = Convert.ToInt32(fldRange[0]);
            int endIndex = Convert.ToInt32(fldRange[1]);
            DataRow ThisRow = tstData.DSGetRow(rowNo, dcNo);
            int top = -1;


            for (int i = startIndex; i <= endIndex; i++)
            {
                FieldStruct fld = (FieldStruct)flds[i];
                //As the following code is going to be called for each field,
                //its not made as method to avoid function call.
                string[] tlhw;
                if (fld.lbltlhw.ToString() != "")
                {
                    tlhw = fld.lbltlhw.ToString().Split(',');

                    tlhw[0] = (Convert.ToInt32(tlhw[0].ToString())).ToString();
                    tlhw[1] = (Convert.ToInt32(tlhw[1].ToString())).ToString();
                    tlhw[2] = (Convert.ToInt32(tlhw[2].ToString())).ToString();
                    tlhw[3] = (Convert.ToInt32(tlhw[3].ToString())).ToString();

                }
                else
                {
                    tlhw = ",,,".Split(','); // Hack: directly entering the data to the tlhw throws error
                    tlhw[0] = "";
                    tlhw[1] = "";
                    tlhw[2] = "";
                    tlhw[3] = "";
                }




                //this function is For Label Percentage
                string[] ptlhw;
                if (fld.plbltlhw.ToString() != "")
                {
                    ptlhw = fld.plbltlhw.ToString().Split(',');
                    if (!string.IsNullOrEmpty(ptlhw[3].ToString()))
                    {
                        float percentage = float.Parse(ptlhw[3].ToString());
                        // lblBsClass = ConvertFieldToLayout(layoutstyle, i);
                    }
                }

                //this function is for field Percentage


                string[] pfldTlhw;
                if (fld.pfldtlhw.ToString() != "")
                {
                    pfldTlhw = fld.pfldtlhw.ToString().Split(',');

                    //fldTlhw[0] = Math.Floor((Convert.ToInt32(fldTlhw[0].ToString()) * 1.2)).ToString();
                    //fldTlhw[1] = Math.Floor((Convert.ToInt32(fldTlhw[1].ToString()) * 1.0)).ToString();

                    if (!string.IsNullOrEmpty(pfldTlhw[3].ToString()))
                    {
                        float percentage = float.Parse(pfldTlhw[3].ToString());
                        //fldBsClass = findGridcol(percentage).ToString();
                    }

                }



                string[] fldTlhw;
                if (fld.fldtlhw.ToString() != "")
                {
                    fldTlhw = fld.fldtlhw.ToString().Split(',');

                    //fldTlhw[0] = Math.Floor((Convert.ToInt32(fldTlhw[0].ToString()) * 1.2)).ToString();
                    //fldTlhw[1] = Math.Floor((Convert.ToInt32(fldTlhw[1].ToString()) * 1.0)).ToString();



                }
                else
                {
                    fldTlhw = ",,,".Split(','); // Hack: directly entering the data to the tlhw throws error
                    fldTlhw[0] = "0";
                    fldTlhw[1] = "0";
                    fldTlhw[2] = "0";
                    fldTlhw[3] = "0";
                }

                if (fld.fldlength != 0)
                {

                    lblBsClass = bsClassByDatawidth(fld.fldlength, i, fld);
                    lblBsIndex = fld.fieldIndex.ToString();
                }

                if (!fld.visibility)
                    gridStackData = xywhGridStack(fld);

                string name = string.Empty;
                string hiddenName = string.Empty;
                string width = string.Empty;
                string fldValue = string.Empty;
                string comboValues = string.Empty;
                string idCol = string.Empty;
                string fldNAME = fld.name.ToUpper();
                string fdwidth = "0";
                //fdwidth = (fld.fldlength > 51 ? 400 : (fld.fldlength <= 15) ? 150 : (fld.fldlength * 8)).ToString();
                if (!fld.visibility)
                    fdwidth = xywhGridStack(fld, true);
                if (isGrid)
                {
                    if (rowNo != "")
                        name = fld.name + rowNo + "F" + fld.fldframeno;
                    else
                        name = fld.name + "001F" + fld.fldframeno;
                    hiddenName = fld.name + "\" +nrno+\"";
                    // for grid its column width
                    width = fld.cwid;
                    if (!fld.visibility)
                    {
                        if (width != "")
                            gridDcColwidth += Convert.ToInt32(width);
                        else
                            gridDcColwidth += 1;
                    }
                }
                else
                {
                    name = fld.name + "000F" + fld.fldframeno;
                    width = fldTlhw[3];
                }


                if (width == "")
                    width = "0";
                else
                    width = Convert.ToString(Convert.ToInt32(width) - 4);
                if (ThisRow != null)
                {
                    if (dc.IsFormatGrid && dc.KeyColumn == fld.name)
                    {
                        if (keyColValue != "")
                            ActiveKeyColValue = keyColValue;
                        else
                            ActiveKeyColValue = ThisRow[fld.name].ToString();
                    }
                    fldValue = ThisRow[fld.name].ToString();
                    if (fldValue.Contains("\""))
                        fldValue = fldValue.Replace("\"", "&quot;");
                    DataRow ThisFieldDropDown = tstData.DSGetDropdownRow(fld.name, rowNo);

                    if (ThisFieldDropDown == null)
                    {
                        ThisFieldDropDown = tstData.DSGetDropdownRow(fld.name, "1");
                        if (ThisFieldDropDown != null && ThisFieldDropDown["mr"].ToString() == "0") ThisFieldDropDown = null;

                    }

                    if (ThisFieldDropDown != null)
                        comboValues = ThisFieldDropDown["rowvals"].ToString();

                    if (comboValues != "")
                    {
                        if (fld.savenormal)
                            fld.fldIdCols = "1";
                        else
                            fld.fldIdCols = "0";

                        fld.fldComboValues = comboValues;
                    }
                    if ((fld.datatype == "Numeric") && (fld.applycomma == "True") && (fldValue != null) && (fldValue != ""))
                    {
                        if (fld.flddecimal > 0)
                        {
                            try
                            {
                                double newFldValue = Convert.ToDouble(fldValue);
                                //fldValue = newFldValue.ToString("#,###0.00", System.Globalization.CultureInfo.InvariantCulture);
                                string decmalFormat = "#,###0.";
                                for (int decNo = 0; decNo < fld.flddecimal; decNo++)
                                {
                                    decmalFormat = decmalFormat + "0";
                                }
                                fldValue = newFldValue.ToString(decmalFormat, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }

                    fld.Value = fldValue;
                }
                else
                {
                    if (dc.IsFormatGrid && dc.KeyColumn == fld.name)
                    {
                        ActiveKeyColValue = fldValue;
                        fld.Value = fldValue;
                    }
                }

                if (fld.visibility)
                {
                    if (name.StartsWith("axp_recid") && fld.Value == "")
                        fld.Value = "0";
                    string ddetails = String.Empty;
                    //if (fld.tooltip != null && fld.tooltip != "")
                    //    ddetails = "<span class=\"material-icons material-icons-style align-middle material-icons-3 cursor-pointer spanddetails\" title=\"Display details\">info</span>";
                    if (isGrid)
                    {
                        fieldHtml.Append("<td class=\"d-none form-group\"><label>" + fld.caption + "</label><textarea class=\"form-control w-100 border bg-transparent overflow-hidden resize-none \" data-style=\"height:0px;width:0px;\" maxlength=\"\" data-hidden=\"\"   id=\"" + name + "\" data-type=\"hidden\" name=\"" + name + "\" readonly>" + fld.Value + "</textarea></td>");
                        gridHeadHtml.Append("<th class=\"d-none form-group\" id=\"th-" + fld.name + "\"><label>" + fld.caption + "</label><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead'>" + ddetails + "</div></th>");
                    }
                    else
                        fieldHtml.Append("<td class=\"d-none form-group\"><textarea class=\"form-control w-100 border bg-transparent overflow-hidden resize-none \" data-hidden=\"\" maxlength=\"\" data-style=\"height:0px;width:0px;\" id=\"" + name + "\" data-type=\"hidden\" name=\"" + name + "\" readonly>" + fld.Value + "</textarea></td>");

                }
                else
                {
                    randomID++;
                    string popImg = string.Empty;
                    string popImgHidden = string.Empty;
                    int popwidth = 0;
                    for (int x = 0; x < popdcs.Count; x++)
                    {
                        if (dc.frameno == ((PopDcStruct)(popdcs[x])).pdcno)
                        {
                            if (fld.name.ToLower() == ((PopDcStruct)(popdcs[x])).popfield.ToLower())
                            {
                                string popRowNo = rowNo;
                                if (popRowNo == "") popRowNo = "001";


                                popImgHidden = "  <div alt=Open class='subGrid glyphicon glyphicon-new-window icon-basic-elaboration-browser-plus' id='" + popRowNo + "F" + ((PopDcStruct)(popdcs[x])).frameno + "'></div>";
                                popImg = "  <div alt=Open class='subGrid glyphicon glyphicon-new-window icon-basic-elaboration-browser-plus' id='" + popRowNo + "F" + ((PopDcStruct)(popdcs[x])).frameno + "'></div>";
                                popwidth = Convert.ToInt32(width) - 20;
                                break;
                            }
                        }
                    }


                    if (!isGrid)
                    {
                        if (fldTlhw[0] != "")
                            lastDcHeight = Convert.ToInt32(fldTlhw[0]) + Convert.ToInt32(fldTlhw[2]);
                        else
                            lastDcHeight = Convert.ToInt32(fldTlhw[2]);
                    }
                    bool isHyper = false;
                    int n = 0;
                    for (n = 0; n <= hlnks.Count - 1; n++)
                    {
                        if (fld.name == ((HLinkStruct)(hlnks[n])).hlsource.Substring(3))
                        {
                            isHyper = true;
                            break;
                        }
                    }

                    if (isGrid)
                    {
                        string ddetails = String.Empty;
                        //if (fld.tooltip != null && fld.tooltip != "")
                        //    ddetails = "<span class=\"material-icons material-icons-style align-middle material-icons-3 cursor-pointer spanddetails\" title=\"Click on any row in this column to know more about the data in the cell.\">info</span>";
                        string allowemptycss = "";
                        if (!fld.allowempty && fld.caption != "")
                            allowemptycss = " required ";
                        colCount++;
                        if (fld.caption.Contains("~"))
                        {
                            //   fieldHtml.Append("<label id=\"th-" + fld.name + "\"  \" class=\"wordBreak\"><div>");
                            gridHeadHtml.Append("<th id=\"th-" + fld.name + "\" style=\"width:" + fdwidth + "px;\" class=\"wordBreak fw-boldest\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead " + allowemptycss + "'>");
                        }
                        else
                        {
                            //    fieldHtml.Append("<label id=\"th-" + fld.name + "\" style=\"width:" + fdwidth + "px\" ><div>");
                            gridHeadHtml.Append("<th id=\"th-" + fld.name + "\" style=\"width:" + fdwidth + "px;\" class=\"fw-boldest\"><div id=\"th-" + fld.name + "-sizer\"></div><div class='thhead " + allowemptycss + "'>");
                        }
                        //if (!string.IsNullOrEmpty(fld.fieldPurpose))
                        //    fieldHtml.Append("<i tabindex=\"-1\" style=\"cursor: pointer; outline: none;\" data-trigger=\"focus\" class=\"fa fa-question-circle-o \" id=\"ico_cl\"  data-toggle=\"popover\"  data-content=\"" + fld.fieldPurpose + " \" data-placement=\"right\"></i>");
                        if (isHyper)
                        {
                            //      fieldHtml.Append("<label class=\"handCur\" id=\"" + ((HLinkStruct)(hlnks[n])).hlsource.ToString() + "\" onclick='javascript:Tstructhyperlink(this);'>");
                            gridHeadHtml.Append("<a href=\"javascript:void(0)\" class=\"handCur\" id=\"" + ((HLinkStruct)(hlnks[n])).hlsource.ToString() + "\" onclick='javascript:Tstructhyperlink(this);'>");
                        }
                        if (fldNAME.IndexOf("AXPBUTTON") == -1)
                        {
                            if (fld.caption.Contains("~"))
                                fld.caption = fld.caption.Replace("~", "\n");
                            //     fieldHtml.Append(fld.caption);
                            gridHeadHtml.Append(fld.caption);
                        }
                        if (isHyper)
                        {
                            //  fieldHtml.Append("</label>");
                            gridHeadHtml.Append("</a>");
                        }

                        // to handle the mandatory fields.
                        //if (!fld.allowempty && fld.caption != "")
                        //{
                        //    //     fieldHtml.Append("<label class=\"allowempty\">*</label>");
                        //    gridHeadHtml.Append("<span class=\"allowempty\">*</span>");
                        //}
                        //    fieldHtml.Append("</div></label>");
                        gridHeadHtml.Append(ddetails + "</div></th>");
                    }
                    else
                    {

                        if (fld.ctype.ToUpper() != "CHECK BOX")
                        {
                            fieldHtml.Append("<div class=' grid-stack-item'  " + gridStackData + " id=\"randomID_" + dc.frameno + randomID + "\"><span class=\"badge-grid-stack position-absolute top-0 end-0\">" + (randomID - 10) + "</span><div class=\"grid-stack-item-content ui-draggable-handle\"></div><div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"labelcol inputs form-group row " + lblBsClass + " \" data-dataindex=" + lblBsIndex + ">");
                            if (isHyper)
                                fieldHtml.Append("<a href=\"javascript:void(0)\"  class=\"cursor-pointer\" id=\"" + ((HLinkStruct)(hlnks[n])).hlsource.ToString() + "\" onclick='javascript:Tstructhyperlink(this);'>");
                            fieldHtml.Append("<label for=\"" + name + "\">" + fld.caption + "</label >");
                            //  fieldHtml.Append(string.Format("<label class='control-label  >{0}:</label>", fld.caption));

                            if (isHyper)
                                fieldHtml.Append("</a>");
                            //fieldHtml.Append("</font>");

                            // to handle the mandatory fields.
                            if (!fld.allowempty && fld.caption != "")
                                fieldHtml.Append("<span class=\"allowempty\">*</span>");
                            //fieldHtml.Append("</span>");
                        }
                        else if (fld.datatype.ToUpper() == "DATE/TIME")
                            fieldHtml.Append("<div id=\"dv" + fld.name.Replace(" ", "") + "\" data-dataindex=\"" + lblBsIndex + "\" class=\"  input-group grid-stack-item " + lblBsClass + "\" " + gridStackData + "><div class=\"grid-stack-item-content ui-draggable-handle\"></div>");
                        else
                        {
                            fieldHtml.Append("<div id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"labelcol inputs form  grid-stack-item \"  " + gridStackData + "><div class=\"grid-stack-item-content ui-draggable-handle\"></div>");
                        }
                    }

                    if (isGrid)
                    {
                        //we dont know
                        fieldHtml.Append("<td style=\"\"><div>");
                        gridHiddenHtml.Append("<div class=\"col-md-4 col-lg-4 col-sm-4 gridElement form-group\">");
                    }
                    else
                    {
                        if (fld.moe.ToUpper() == "SELECT" && fld.selectmode.ToUpper() == "FROM MASTER" && fld.editcombo == true)
                            fieldHtml.Append("<span id=\"dv" + fld.name.Replace(" ", "") + "\" class=\"picklist  input-group \">");

                    }

                    if (popwidth > 0)
                        width = popwidth.ToString();
                    string addZeroForNumeric = string.Empty;
                    if (isGrid && ShowZeroForNumeric && (fld.datatype.ToUpper() == "NUMERIC"))
                        addZeroForNumeric = " value=0 ";

                    string fldCType = fld.ctype.ToUpper();
                    string fldMOE = fld.moe.ToUpper();

                    // generate html depends on the mode of entry, like text box, check box, radio button
                    switch (fldMOE)
                    {
                        // accept- simple input type which accepts the user input, mostly except image.
                        case "ACCEPT":
                            SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                            if (fldCType == "MEMO" || fld.datatype.ToLower() == "text" && fld.ctype.ToUpper() != "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "memotem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (!isGrid)
                                    width = Convert.ToString(Convert.ToInt32(width) - 10);

                                if (isGrid)
                                    gridHiddenHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                                fieldHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                            }
                            else if (fldCType == "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "checkbox", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                                fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                            }
                            else if (fld.datatype.ToUpper() == "IMAGE")
                            {
                                if (isGrid)
                                    gridHiddenHtml.Append(GetImageHTML(fld, isGrid, name, i, fldTlhw, width, 0));
                                fieldHtml.Append(GetImageHTML(fld, isGrid, name, i, fldTlhw, width, 0));
                            }
                            else if (fldNAME.ToUpper().IndexOf("AXPBUTTON") != -1)
                            {
                                if (isGrid)
                                {
                                    gridHiddenHtml.Append(GetAxpBtnHTML(fld, isGrid, name, width));
                                }
                                fieldHtml.Append(GetAxpBtnHTML(fld, isGrid, name, width));
                            }
                            else if (fld.datatype.ToUpper() == "DATE/TIME")
                            {

                                if (isGrid)
                                {
                                    width = (Convert.ToInt32(width) - 5).ToString();
                                    gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));

                                }
                                else
                                {
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                            }

                            else
                            {
                                if ((name.StartsWith("axp_gridattach")) || (fld.name.StartsWith("dc") && fld.name.ToLower().EndsWith("_image")) || fld.name.ToLower().StartsWith("axpfile_"))//(name.ToLower().StartsWith("dc" + dcNo + "_image")))
                                {
                                    string tmpValue = string.Empty;
                                    try
                                    {
                                        tmpValue = ThisRow["axpattach_filename" + fld.fldframeno.ToString()].ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        tmpValue = "";
                                    }
                                    if (tmpValue != "" && fld.Value != "")
                                    {
                                        string sfileEx = fld.Value.Substring(fld.Value.LastIndexOf("."));
                                        fld.Value = tmpValue + sfileEx;
                                    }

                                    if (isGrid)
                                        gridHiddenHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                    fieldHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                }
                                else
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                            }
                            break;
                        case "SELECT":

                            string[] moeValue;
                            if (!isGrid)
                                width = Convert.ToString(Convert.ToInt32(width) + 5);

                            if (fld.moeval != null)
                                moeValue = fld.moeval.ToString().Split(',');
                            else
                                moeValue = "".Split();

                            if (fldCType == "CHECK LIST" && !isGrid)
                            {
                                SetCssClass(fld.freadonly, "multiFldChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckListHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                                fieldHtml.Append(GetCheckListHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                            }
                            else if (fldCType == "RADIO GROUP" && !isGrid)
                            {
                                SetCssClass(fld.freadonly, "multiFld", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (!isGrid)
                                {
                                    fieldHtml.Append(GetRadioGroupHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));
                                    gridHiddenHtml.Append(GetRadioGroupHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue));

                                }

                            }
                            else if (fld.selectmode == string.Empty)
                            {
                                bool chkBox = false;
                                chkBox = IsGridCheckBox(moeValue);
                                SetCssClass(fld.freadonly, "combo", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                if (isGrid)
                                {
                                    if (chkBox)
                                    {
                                        SetCssClass(fld.freadonly, "gridChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                        width = Convert.ToString(Convert.ToInt32(width) - 10);
                                        gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                        fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));

                                    }
                                    else
                                    {
                                        if (isGrid)
                                            gridHiddenHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                        fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                    }
                                }
                                else
                                {
                                    fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                }
                            }
                            else if (fld.selectmode.ToUpper() == "FROM MASTER")
                            {
                                SetCssClass(fld.freadonly, "combo", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                if (!fld.editcombo)
                                {
                                    // Adding checkbox to GridDC                                       
                                    bool chkBox = false;
                                    chkBox = IsGridCheckBox(moeValue);

                                    if (isGrid)
                                    {
                                        if (chkBox)
                                        {
                                            SetCssClass(fld.freadonly, "gridChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                            gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                            fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                        }
                                        else
                                        {

                                            gridHiddenHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                            if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                            {
                                                fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                                fromListFlds += "," + fld.name;
                                            }
                                            else
                                                fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                        }
                                    }
                                    else
                                    {
                                        if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                        {
                                            fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                            fromListFlds += "," + fld.name;
                                        }
                                        else
                                            fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                    }
                                }
                                else
                                {
                                    SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);


                                    if (isGrid)
                                    {
                                        gridHiddenHtml.Append(GetPickListHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden, true));
                                    }

                                    fieldHtml.Append(GetPickListHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden, false));

                                }
                            }
                            else
                            {
                                SetCssClass(fld.freadonly, "combo", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                bool chkBox = false;
                                chkBox = IsGridCheckBox(moeValue);

                                if (isGrid)
                                {
                                    if (chkBox)
                                    {
                                        SetCssClass(fld.freadonly, "gridChk", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                                        width = Convert.ToString(Convert.ToInt32(width) - 10);

                                        gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                        fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));

                                    }
                                    else
                                    {
                                        if (isGrid)
                                            gridHiddenHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                        if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                        {
                                            fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                            fromListFlds += "," + fld.name;
                                        }
                                        else
                                            fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));
                                    }
                                }
                                else
                                {
                                    if (moeValue.Length > 0 && (moeValue.Length != 1 && moeValue[0] == ""))
                                    {
                                        fieldHtml.Append(GetSelectHTML(fld, isGrid, name, fldNo, fldTlhw, width, moeValue, popImg, popImgHidden));
                                        fromListFlds += "," + fld.name;
                                    }
                                    else
                                        fieldHtml.Append(GetEmptySelectHTML(fld, isGrid, name, width, popImg, popImgHidden));

                                }
                            }
                            break;
                        case "AUTOGENERATE":

                            SetCssClass(fld.freadonly, "auto", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);


                            if (isGrid)
                                gridHiddenHtml.Append(GetAutoGenFieldHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden));
                            fieldHtml.Append(GetAutoGenFieldHTML(fld, isGrid, name, fldNo, fldTlhw, width, popImg, popImgHidden));

                            break;
                        case "FILL":
                            SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                            if (fldCType == "MEMO" || fld.datatype.ToLower() == "text" && fld.ctype.ToUpper() != "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "memotem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (!isGrid)
                                    width = Convert.ToString(Convert.ToInt32(width) - 8);

                                if (isGrid)
                                    gridHiddenHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));
                                fieldHtml.Append(GetMemoHTML(fld, isGrid, name, i, fldTlhw, width));

                            }

                            else if (fldCType == "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "checkbox", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                                fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, i, fldTlhw, width, fld.visibility, fld.moeval));
                            }

                            else
                            {
                                if (fld.name.ToLower().StartsWith("axpfile_") && isGrid)
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                    fieldHtml.Append(GetGridAttachHTML(fld, isGrid, name, fldNo, fldTlhw, width));
                                }
                                else
                                {
                                    if (isGrid)
                                        gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                    fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                }
                            }
                            break;
                        case "CALCULATE":
                            SetCssClass(fld.freadonly, "tem", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);

                            if (fldCType == "CHECK BOX")
                            {
                                SetCssClass(fld.freadonly, "checkbox", fld.datatype, fld.name, fldBsClass, fld.fldType, fld);
                                if (isGrid)
                                    gridHiddenHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));
                                fieldHtml.Append(GetCheckBoxHTML(fld, isGrid, name, fldNo, fldTlhw, width, fld.visibility, fld.moeval));

                            }
                            else
                            {

                                if (isGrid)
                                    gridHiddenHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));
                                fieldHtml.Append(GetInputHTML(fld, isGrid, name, fldNo, fldTlhw, width, addZeroForNumeric, popImg, popImgHidden));

                            }
                            break;
                    }

                    if (isGrid)
                    {
                        gridHiddenHtml.Append("</div></div>");
                        fieldHtml.Append("</div></td>");

                        if (fldMOE == "SELECT" || (fldNAME.ToUpper().IndexOf("AXPBUTTON") != -1) || (popwidth > 0))
                        {
                            if (width != "")
                            {
                                if (popwidth > 0)
                                    width = fld.cwid;

                                width = (Convert.ToInt32(width) - 4).ToString();
                            }
                        }
                        dummyGridHeadHtml.Append("<TD class=agrow1 align=center ><INPUT type=\"text\" disabled  style=\"width:" + width + "px;height:0px;\" class=agrowGfrow onfocus=this.blur(); /></TD>");
                    }
                    else
                    {
                        //fieldHtml.Append(" </div>");
                        if (fld.moe.ToUpper() == "SELECT" && fld.selectmode.ToUpper() == "FROM MASTER" && fld.editcombo == true)
                            fieldHtml.Append("</span>");
                        fieldHtml.Append("</div></td>");

                    }

                    if (isLastDcDate)
                        lastDcHeight = lastDcHeight + 75;
                }
            }
        }
        if (isGrid && hiddenFldsHtml.ToString() != string.Empty) fieldHtml.Append("<td>" + hiddenFldsHtml.ToString() + "</td>");

        if (isGrid)
        {
            for (int m = 0; m < gridScriptDcNo.Count; m++)
            {
                if (dcNo == Convert.ToInt32(gridScriptDcNo[m].ToString()))
                {
                    gridHeadHtml.Append(gridScriptButtons[m].ToString().Split('♠')[0]);
                    gridHiddenHtml.Append(gridScriptButtons[m].ToString().Split('♠')[1]);
                    fieldHtml.Append("<td>" + gridScriptButtons[m].ToString().Split('♠')[2] + "</td>");
                }
            }
        }
        return fieldHtml.ToString();
    }



    /// <summary>
    /// function to construct the dc html with field html.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <param name="fieldhtml"></param>
    /// <returns> completed field html.</returns>
    private string GetFullTabDCHTML(int dcIndNo, int dcNo, string fieldhtml, string isTab, string rowCnt, string fillGriddcRowone)
    {

        if (dcNo == 1) docHeight = 0;
        StringBuilder dcHtml = new StringBuilder();
        string dcFields1 = string.Empty;
        string ngBorder = string.Empty;
        StringBuilder customLabel = new StringBuilder();
        StringBuilder actionButton = new StringBuilder();
        StringBuilder defaultColsHtml = new StringBuilder();
        string divDirection = "left";
        string divCollapse = "right";
        int gridwidth = 0;

        if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
        {
            divDirection = "right";
            divCollapse = "left";

        }
        for (int n = 0; n < customLblDcNo.Count; n++)
        {
            if (dcNo == Convert.ToInt32(customLblDcNo[n].ToString()))
                customLabel.Append(customLabels[n].ToString());
        }

        for (int m = 0; m < pageActionDcNo.Count; m++)
        {
            if (dcNo == Convert.ToInt32(pageActionDcNo[m].ToString()))
                actionButton.Append(pageActionButtons[m].ToString());
        }
        var designMode = false;
        if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
        {
            designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
        }

        string dcBoolean = string.Empty;
        //if (!designMode && ((DcStruct)(dcs[dcNo - 1])).DcDefaultstate != "" && ((DcStruct)(dcs[dcNo - 1])).DcDefaultstate.ToLower() == "collapse")
        if (!designMode && ((DcStruct)(dcs[dcIndNo])).DcDefaultstate != "" && ((DcStruct)(dcs[dcIndNo])).DcDefaultstate.ToLower() == "collapse")
            dcBoolean = " d-none ";

        if (((DcStruct)(dcs[dcIndNo])).isgrid) //(((DcStruct)(dcs[dcNo - 1])).isgrid)
        {

            StringBuilder gridHdHtml = new StringBuilder();

            //gridHdHtml.Append("<div id='divDc" + dcNo + "' class=\"tab-content mainIframe " + (((DcStruct)(dcs[dcNo - 1])).isgrid ? "" : "grid-stack") + "\">");
            gridHdHtml.Append("<div id='divDc" + dcNo + "' class=\"tab-content mainIframe " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : "grid-stack") + "\">");

            //if (((DcStruct)(dcs[dcNo - 1])).isallowdeletrows.ToString().ToLower() == "true")
            if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "true")
            {
                xywhGridStack(new FieldStruct { visibility = false, fldFrameNo = dcNo, name = ("uniqueThHead" + dcNo) }, true);
                //if (((DcStruct)(dcs[dcNo - 1])).ispopgrid)
                if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
                {
                    defaultColsHtml.Append("<th id='uniqueThHead" + dcNo + "' class='thhead fw-boldest'><div>#</div></th>");
                    colCount++;
                }
                else
                {
                    defaultColsHtml.Append("<th id=\"uniqueThHead" + dcNo + "\" class='thhead fw-boldest'><div id=\"uniqueThHead" + dcNo + "-sizer\"></div><div class='thhead'>S.No</div></th>");
                    colCount = colCount++;
                }
            }
            else
            {
                //if (((DcStruct)(dcs[dcNo - 1])).ispopgrid)
                if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
                    defaultColsHtml.Append("");
                else
                {
                    xywhGridStack(new FieldStruct { visibility = false, fldFrameNo = dcNo, name = ("uniqueThHead" + dcNo) }, true);
                    defaultColsHtml.Append("<th id=\"uniqueThHead" + dcNo + "\" class='thhead fw-boldest'><div id=\"uniqueThHead" + dcNo + "-sizer\"></div><div class='thhead'>S.no</div></th>");
                    colCount++;
                }
            }
            int VisibleFlds = ((DcStruct)(dcs[dcIndNo])).visibleFldCount; // ((DcStruct)(dcs[dcNo - 1])).visibleFldCount;
            for (int k = 0; k < flds.Count; k++)
            {
                FieldStruct fld = ((FieldStruct)(flds[k]));
                if (fld.fldframeno < dcNo) continue;
                if (fld.fldframeno > dcNo) continue;// break;
                if (fld.fldframeno == dcNo)
                {
                    if (fld.visibility == false)
                    {
                        gridwidth += (fld.fldlength > 51 ? 400 : (fld.fldlength <= 15) ? 150 : (fld.fldlength * 8));
                    }
                }
            }
            //if (((DcStruct)(dcs[dcNo - 1])).isallowdeletrows.ToString().ToLower() == "true")
            if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "true")
                gridwidth = gridwidth + 80;
            else
                gridwidth = gridwidth + 40;//40 width for sl no column and 40 for delete button
            string dcBtns = GetGridButtonHtml(dcNo.ToString());
            colCount = 100;
            gridHdHtml.Append("<div id=\"containerDc\" class=\"tab-pane fade show active grid-icons\"><div class=\"card card-xl-stretch mb-1 mb-xl-2 border border-gray-300 rounded-0\"><div class=\"card-body px-3 pt-1 pb-3\">" + dcBtns);
            if (IsFillGridCall)
            {
                gridHdHtml.Append("<div id=\"gridheaddiv\"><span id=\"disgridhead" + dcNo + "\"></span></div><div class=\"clear\"></div>");
                string hideClass = "d-none";
                bool ishtmlhd = false;
                for (int m = 0; m < gridScriptDcNo.Count; m++)
                {
                    if (dcNo == Convert.ToInt32(gridScriptDcNo[m].ToString()))
                    {
                        ishtmlhd = true;
                        if (m == 0)
                            fillGriddcRowone = fillGriddcRowone.Substring(0, fillGriddcRowone.Length - 6);
                        fillGriddcRowone += gridScriptButtons[m].ToString().Split('♠')[1] + "</div>";
                    }
                }
                if (ishtmlhd)
                    fillGriddcRowone += "</div>";

                gridHdHtml.Append("<div class='row " + hideClass + "' id=\"wrapperForEditFields" + dcNo + "\">" + fillGriddcRowone + "<div class=\"col-sm-12 editLayoutFooter\"> <div class=\"GridHelpdiv\"><i class=\"fa fa-question-circle\" aria-hidden=\"true\"></i> <span>Use <b>Ctrl+Left arrow</b> &amp; <b>Ctrl+Right arrow</b> to navigate between record(s).</span></div><div><div class=\"btn-group previousNextEditButton\" role=\"group\"><button  title=\"Previous Record\" class=\"btn prevRec coldbtn disabled lblprevious\" disabled=\"disabled\" onclick=\"editThePreviousNextRow('','" + dcNo + "','');\">&lt; prev</button><button title=\"Next Record\" class=\"btn nextRec disabled coldbtn lblNext\" disabled=\"disabled\" onclick=\"editThePreviousNextRow('','" + dcNo + "','');\">Next &gt;</button></div><button  title=\"Save &amp; New Record\" class=\"btn hotbtn lblSaveNew\" id=\"newRecordBtn" + dcNo + "\" onclick=\"addTheValuesToGrid(" + dcNo + ",this)\">Save &amp; New</button></div></div></div>");
                //gridHdHtml.Append("</div>");
            }
            else
            {
                gridHdHtml.Append("<div id=\"gridheaddiv\"><span id=\"disgridhead" + dcNo + "\"></span></div><div class=\"clear\"></div>");
                string hideClass = "d-none";

                gridHdHtml.Append("<div class='row " + hideClass + "' id=\"wrapperForEditFields" + dcNo + "\">" + fieldhtml + "<div class=\"col-sm-12 editLayoutFooter\"> <div class=\"GridHelpdiv\"><i class=\"fa fa-question-circle\" aria-hidden=\"true\"></i> <span>Use <b>Ctrl+Left arrow</b> &amp; <b>Ctrl+Right arrow</b> to navigate between record(s).</span></div><div><div class=\"btn-group previousNextEditButton\" role=\"group\"><button title=\"Previous Record\" class=\"btn prevRec coldbtn disabled lblprevious\" disabled=\"disabled\" onclick=\"editThePreviousNextRow('','" + dcNo + "','');\">&lt; Prev</button><button title=\"Next Record\" class=\"btn nextRec disabled coldbtn lblNext\" disabled=\"disabled\" onclick=\"editThePreviousNextRow('','" + dcNo + "','');\">Next &gt;</button></div><button title=\"Save &amp; New Record\" class=\"btn hotbtn lblSaveNew\" id=\"newRecordBtn" + dcNo + "\" onclick=\"addTheValuesToGrid(" + dcNo + ",this)\">Save &amp; New</button></div></div></div>");
                //gridHdHtml.Append("</div>");
            }
            if (HttpContext.Current.Session["language"].ToString().ToLower() == "arabic")
            {
                gridHdHtml.Append("<div id=\"dvgridwrapper\"><div id=\"colScroll" + dcNo + "\" class=\"griddivColumn wrapperForGridData" + dcNo + " " + dcBoolean + "\">");
                gridHdHtml.Append("<table class='customSetupTableMN gridHeader g-1 table table-striped table-bordered mt-1 mb-0 border-gray-300 ' id=\"gridHd" + dcNo + "\"><thead><tr>");

            }
            else
            {
                gridHdHtml.Append("<div id=\"colScroll" + dcNo + "\" class=\"griddivColumn wrapperForGridData" + dcNo + " " + dcBoolean + "\">");
                gridHdHtml.Append("<table class='customSetupTableMN gridHeader g-1 table table-striped table-bordered mt-1 mb-0 border-gray-300 ' id=\"gridHd" + dcNo + "\"><thead><tr class=\"fw-bold fs-6 text-gray-800 border-bottom-2 border-gray-300\">");
            }
            xywhGridStack(new FieldStruct { visibility = false, fldFrameNo = dcNo, name = ("uniqueEditDeleteAct" + dcNo) }, true);
            //gridHdHtml.Append("<th id=\"uniqueEditDeleteAct" + dcNo + "\" class='editDeleteAct'></th>" + defaultColsHtml.ToString());
            gridHdHtml.Append("<th id =\"uniqueEditDeleteAct" + dcNo + "\" class=\"text-center fw-boldest\"><div class=\"form-check form-check-sm form-check-custom ms-2\"><input type=\"checkbox\" name=\"chkallgridrow" + dcNo + "\" class=\"form-check-input fgHdrChk gridHdrChk opacity-100\" id=\"chkallgridrow" + dcNo + "\" onclick=\"javascript:CheckAllGridRow(this, " + dcNo + ");\" disabled></div></th>" + defaultColsHtml.ToString());

            if (IsFillGridCall)
            {
                for (int m = 0; m < gridScriptDcNo.Count; m++)
                {
                    if (dcNo == Convert.ToInt32(gridScriptDcNo[m].ToString()))
                    {
                        gridHeadHtml.Append(gridScriptButtons[m].ToString().Split('♠')[0]);
                        //gridHiddenHtml.Append(gridScriptButtons[m].ToString().Split('♠')[1]);
                    }
                }
                gridHdHtml.Append(gridHeadHtml.ToString() + "</tr></thead><tbody>" + fieldhtml + "</tbody></table></div>");
            }
            else
            {
                gridHdHtml.Append(gridHeadHtml.ToString() + "</tr></thead><tbody></tbody></table></div>");
            }

            StringBuilder gridHtml = new StringBuilder();
            string gridButtons = string.Empty;

            int serialNo = Convert.ToInt32(rowCnt);
            string rowCountHtml = "<input type='hidden' id='hdnRCntDc" + dcNo + "'  value='" + rowCnt + "'/><input type='hidden' id='hdnSlNoCnt" + dcNo + "' value='" + serialNo.ToString() + "'/>";

            //if (((DcStruct)(dcs[dcNo - 1])).ispopgrid)
            if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
            {
                gridHdHtml.Append("<div id=\"contentScroll" + dcNo + "\" class=\"divGridContent\">");
                gridHdHtml.Append("<table class='subGridContent' id='gridDc" + dcNo + "'><tbody>");
            }
            else
            {
                gridHdHtml.Append("<div id=\"contentScroll" + dcNo + "\" class=\"divGridContent\">");
                gridHdHtml.Append("<table class='gridContent' id='gridDc" + dcNo + "'><tbody>");
            }


            gridHdHtml.Append("</tbody></table>");
            gridHdHtml.Append("</div></div></div>");

            //string gridOption = "<span class='gridoptions glyphicon material-icons' title='Grid Option'>more_vert</span>";
            string fullGridHtml1 = string.Empty;
            if (isTab == "true")
                fullGridHtml1 = "<div class='col-12 pb-2 dvdcframe' id=\"DivFrame" + dcNo + "\">" + gridHdHtml.ToString() + rowCountHtml + "</div>";
            else if (((DcStruct)(dcs[dcIndNo])).ispopgrid)//(((DcStruct)(dcs[dcNo - 1])).ispopgrid)
            {
                fullGridHtml1 = "<div>";
                //fullGridHtml1 += "<input type='hidden' id='popDcTitle" + dcNo + "' class='popDcTitle' value='" + ((DcStruct)(dcs[dcNo - 1])).caption + "' />";
                fullGridHtml1 += "<input type='hidden' id='popDcTitle" + dcNo + "' class='popDcTitle' value='" + ((DcStruct)(dcs[dcIndNo])).caption + "' />";
                fullGridHtml1 += gridHdHtml.ToString() + rowCountHtml + "</div>";
            }
            else if (isTab == "formatGrid")
            {
                fullGridHtml1 = "<div class='col-12 pb-2 dvdcframe' id=\"DivFrame" + dcNo + "\">";
                //fullGridHtml1 += "<div id=head" + dcNo + " class='dcTitle'" + divDirection + " ><a href=\"javascript:void(0)\" onclick='javascript:ShowDc(\"" + dcNo + "\");'><span data-type=\"hide\" title=\"Hide Dc\" class=\"material-icons\" id=dcButspan" + dcNo + " >keyboard_arrow_up</span></a><span class=\"frameCap fw-boldest fs-4 text-gray-900\" " + divDirection + " id=\"dcCaption" + dcNo + "\"><h3>" + ((DcStruct)(dcs[dcNo - 1])).caption + "</h3></span></div>";
                fullGridHtml1 += "<div id=head" + dcNo + " class='dcTitle'" + divDirection + " ><a href=\"javascript:void(0)\" onclick='javascript:ShowDc(\"" + dcNo + "\");'><span data-type=\"hide\" title=\"Hide Dc\" class=\"material-icons\" id=dcButspan" + dcNo + " >keyboard_arrow_up</span></a><span class=\"frameCap fw-boldest fs-4 text-gray-900\" " + divDirection + " id=\"dcCaption" + dcNo + "\"><h3>" + ((DcStruct)(dcs[dcIndNo])).caption + "</h3></span></div>";
                //fullGridHtml1 += "<div id=head" + dcNo + " class='dcTitle'" + divDirection + " ><span class=\"frameCap\" " + divDirection + " id=\"dcCaption" + dcNo + "\"><h3>" + ((DcStruct)(dcs[dcNo - 1])).caption + "</h3></span></div>";
                fullGridHtml1 += "<div id=head" + dcNo + " class='dcTitle'" + divDirection + " ><span class=\"frameCap\" " + divDirection + " id=\"dcCaption" + dcNo + "\"><h3>" + ((DcStruct)(dcs[dcIndNo])).caption + "</h3></span></div>";
                fullGridHtml1 += gridHdHtml.ToString() + rowCountHtml + "</div>";
            }
            else
            {
                fullGridHtml1 = "<div class='col-12 pb-2 dc dvdcframe' id=\"DivFrame" + dcNo + "\">";
                fullGridHtml1 += "<ul class=\"cursor-pointer nav nav-tabs mb-n2 border border-gray-300 rounded-0 bg-light\">";
                string dcBooleanHtml = string.Empty;
                //if (((DcStruct)(dcs[dcNo - 1])).DcBlean != "" && ((DcStruct)(dcs[dcNo - 1])).DcBlean.ToUpper() == "TRUE")
                if (((DcStruct)(dcs[dcIndNo])).DcBlean != "" && ((DcStruct)(dcs[dcIndNo])).DcBlean.ToUpper() == "TRUE")
                {
                    dcBooleanHtml = "<div class=\"form-check form-switch form-check-custom px-1 align-self-end my-auto \"><input type=\"checkbox\" id=\"dcBlean" + dcNo + "\" data-dcname=\"" + ((DcStruct)(dcs[dcIndNo])).name + "\" title=\"\" style=\"\" name=\"dcBlean" + dcNo + "\" class=\"form-check-input opacity-100 gridHeaderSwitch \" " + (dcBoolean == string.Empty ? "checked" : "") + "></div>";
                    //fullGridHtml1 += "<div class=\"nav-link fw-boldest d-flex p-4 shadow-sm active\">";
                    fullGridHtml1 += "<li id=head" + dcNo + " class=\"nav-item d-flex p-3 shadow-sm bg-white rounded-top\">" + dcBooleanHtml + "<a class=\"fs-4 nav-link fw-boldest text-gray-900 rounded-0 border-0 active\" data-bs-toggle=\"tab\" aria-current=\"page\" href=\"javascript:void(0);\" onclick='javascript:ShowDc(\"" + dcNo + "\");'>" + ((DcStruct)(dcs[dcIndNo])).caption + "</a></li></ul>";
                }
                else
                    fullGridHtml1 += "<li id=head" + dcNo + " class=\"nav-item d-flex p-3 shadow-sm bg-white rounded-top\"><a class=\"fw-bold fs-6 nav-link fw-boldest text-gray-800 rounded-0 border-0 active\" data-bs-toggle=\"tab\" aria-current=\"page\" href=\"javascript:void(0);\" onclick='javascript:ShowDc(\"" + dcNo + "\");'>" + ((DcStruct)(dcs[dcIndNo])).caption + "</a></li></ul>";

                fullGridHtml1 += gridHdHtml.ToString() + rowCountHtml;
            }
            dcHtml.Append(fullGridHtml1 + "<div class='clear'></div>");
            return "<div class='mainContent'>" + dcHtml.ToString() + "</div>";
        }
        else
        {
            // innermost div for all non grid dcs.
            // lastDcHeight is used if there is a calendar control in the last Dc, So this 
            // was affecting the dcheight to overcome this there is check where the the greater height isa set as dcheight
            if (dcNo == dcs.Count - 1)
            {
                if (((DcStruct)(dcs[dcIndNo])).dcHeight > lastDcHeight)
                {
                    lastDcHeight = ((DcStruct)(dcs[dcIndNo])).dcHeight;
                }
                ngBorder = "<div id=\"divDc" + dcNo + "\" class=\"row mainIframe " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : "grid-stack gx-0") + " " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : dcBoolean) + "\">";
            }
            else
            {
                ngBorder = "<div id=\"divDc" + dcNo + "\" class=\"row mainIframe " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : "grid-stack gx-0") + " " + (((DcStruct)(dcs[dcIndNo])).isgrid ? "" : dcBoolean) + "\"  >";
            }
            string ngFldHtml = ngBorder + fieldhtml + customLabel.ToString() + actionButton.ToString();
            if (isTab == "true")
            {
                if (dcNo > 1)
                {
                    dcHtml.Append("<div id=\"DivFrame" + dcNo + "\" class=\"col-12 pb-2 dvdcframe\"><div class='card card-xl-stretch mb-1 mb-xl-2 border border-gray-300 rounded-0'><div class='card-body px-3 pt-1 pb-3'>");
                    //dcHtml.Append(ngFldHtml + "</div></div></div><div class='clear'></div><div>" + ((DcStruct)(dcs[dcNo - 1])).purpose + "</div>");//muralli
                    dcHtml.Append(ngFldHtml + "</div></div></div><div class='clear'></div>");//muralli

                }
                else
                {
                    //TODO: the primary dc can also be a tabbed dc
                    dcHtml.Append("<div id=\"DivFrame" + dcNo + "\" class=\"col-12 pb-2 dvdcframe\"><div class='card card-xl-stretch mb-1 mb-xl-2 border border-gray-300 rounded-0'><div class='card-body px-3 pt-1 pb-3'>");
                    //dcHtml.Append(ngFldHtml + "</div></div></div><div class='clear'></div><div>" + ((DcStruct)(dcs[dcNo - 1])).purpose + "</div>");//muralli
                    dcHtml.Append(ngFldHtml + "</div></div></div><div class='clear'></div>");//muralli
                }
            }
            else
            {
                dcHtml.Append("<div id=\"DivFrame" + dcNo + "\" class=\"col-12 pb-2 dvdcframe\">");
                if (dcNo > 1)
                {
                    dcHtml.Append("<ul class=\"cursor-pointer nav nav-tabs mb-n2 border border-gray-300 rounded-0 bg-light\">");
                    string dcBooleanHtml = string.Empty;
                    if (((DcStruct)(dcs[dcIndNo])).DcBlean != "" && ((DcStruct)(dcs[dcIndNo])).DcBlean.ToUpper() == "TRUE")
                    {
                        dcBooleanHtml = "<div class=\"form-check form-switch form-check-custom px-1 align-self-end my-auto \"><input type=\"checkbox\" id=\"dcBlean" + dcNo + "\" data-dcname=\"" + ((DcStruct)(dcs[dcIndNo])).name + "\" title=\"\" style=\"\" name=\"dcBlean" + dcNo + "\" class=\"form-check-input opacity-100 gridHeaderSwitch \" " + (dcBoolean == string.Empty ? "checked" : "") + "></div>";
                        //dcHtml.Append("<div class=\"nav-link fw-boldest d-flex p-4 shadow-sm active\">");
                        dcHtml.Append("<li id=head" + dcNo + " class=\"nav-item d-flex p-3 shadow-sm bg-white rounded-top\">" + dcBooleanHtml + "<a class=\"fs-4 nav-link fw-boldest text-gray-900 rounded-0 border-0 active\" data-bs-toggle=\"tab\" aria-current=\"page\" href=\"javascript:void(0);\" onclick='javascript:ShowDc(\"" + dcNo + "\");'>" + ((DcStruct)(dcs[dcIndNo])).caption + "</a></li></ul>");
                    }
                    else
                        dcHtml.Append("<li id=head" + dcNo + " class=\"nav-item d-flex p-3 shadow-sm bg-white rounded-top\"><a class=\"fw-bold fs-6 nav-link fw-boldest text-gray-800 rounded-0 border-0 active\" data-bs-toggle=\"tab\" aria-current=\"page\" href=\"javascript:void(0);\" onclick='javascript:ShowDc(\"" + dcNo + "\");'>" + ((DcStruct)(dcs[dcIndNo])).caption + "</a></li></ul>");
                }
                if (flgEnblPurpose) //murali
                    dcHtml.Append(ngFldHtml + "</div><div class='clear'></div><div>" + ((DcStruct)(dcs[dcIndNo])).purpose + "</div>");//muralli
                else
                    dcHtml.Append(ngFldHtml + "</div><div class='clear'></div>");

            }
            dcHtml.Append("</div>");
            return dcHtml.ToString();
        }
    }



    #endregion

    #region General functions

    //Function to construct the rowno as per our standard [ 3 digit ] from the integer rowNo.
    public string GetRowNoHelper(int rowNo)
    {
        string rowNoStr = rowNo.ToString();
        if (rowNoStr.Length == 1)
            rowNoStr = "00" + rowNoStr;
        else if (rowNoStr.Length == 2)
            rowNoStr = "0" + rowNoStr;

        return rowNoStr;
    }

    /// <summary>
    /// Function to get the dc fields start index and end index from the field array.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <returns>start index and end index seperated by a comma</returns>
    public string GetDcFieldRange(string dcNo)
    {
        string startIndex = "0";
        string endIndex = "0";
        for (int i = 0; i < fldDcRange.Count; i++)
        {
            string[] fldRange = fldDcRange[i].ToString().Split('~');
            if (dcNo == fldRange[0].ToString())
            {
                string[] dcIndex = fldRange[1].Split(',');
                startIndex = dcIndex[0].ToString();
                endIndex = dcIndex[1].ToString();
            }
        }
        return startIndex + "," + endIndex;
    }

    /// <summary>
    /// Function to add decimal values to a string not containing decimal point
    /// </summary>
    /// <param name="value"></param>
    /// <param name="noOfDecimals"></param>
    /// <returns></returns>
    private string FormatDecimalValue(string value, int noOfDecimals)
    {
        string newValue = "";
        string suffixStr = "";
        for (int i = 0; i < noOfDecimals; i++)
        {
            suffixStr += "0";
        }
        if (value == "") value = "0";
        if (noOfDecimals > 0)
            newValue = value + "." + suffixStr;
        else
            newValue = value;
        return newValue;
    }

    #endregion

    #region FormatGridFunctions

    public ArrayList GetGroups(int dcIndNo, int dcNo, TStructData tstData)
    {
        //DcStruct dc = (DcStruct)(dcs[dcNo - 1]);
        DcStruct dc = (DcStruct)(dcs[dcIndNo]);
        if (dc.DispKeyColValues.Count > 0)
            return dc.DispKeyColValues;
        else if (dc.KeyColValues.Count > 0)
            return dc.KeyColValues;

        ArrayList DefaultGroups = new ArrayList();
        if (dc.Sql != "")
        {
            string result = ExecuteSql(dc.Sql, tstData, dcNo);
            DefaultGroups = GetGroupsFromResult(result, dcNo, dc.MapColumn);
        }
        return DefaultGroups;
    }


    /// <summary>
    /// Function to get the html of the format grid on load
    /// </summary>
    /// <param name="dcNo"></param>
    /// <param name="tstData"></param>
    /// <param name="rowCnt"></param>
    /// <param name="isTab"></param>
    /// <param name="isTabDis"></param>
    /// <returns></returns>
    public string GetDefaultFormatGridHtml(int dcIndNo, int dcNo, TStructData tstData, int rowCnt, string isTab, string isTabDis)
    {
        //DcStruct dc = (DcStruct)(dcs[dcNo - 1]);
        DcStruct dc = (DcStruct)(dcs[dcIndNo]);
        string dcHtml = string.Empty;

        ArrayList DefaultGroups = new ArrayList();
        if (dc.Action == string.Empty)
            DefaultGroups = GetGroups(dcIndNo, dcNo, tstData);
        string dcData = string.Empty;
        ArrayList groups = new ArrayList();
        if (DefaultGroups.Count > 0)
            groups.Add(DefaultGroups[0]);
        else
            groups.Add("");
        dcData = GetGroupRowHtml(dcIndNo, dcNo, groups, "001", tstData);
        string[] strDcHtml = dcData.Split('♣');
        dcHtml = strDcHtml[1].ToString();

        dcHtml = GetFullTabDCHTML(dcIndNo, dcNo, dcHtml, isTab, rowCnt.ToString(), "");
        return dcHtml;
    }

    /// <summary>
    /// Function to return the format grid html for the given rows which do not have sub groups.
    /// This function will be called only for groups from sql and list without action.
    /// </summary>
    /// <returns></returns>
    public string GetFormatHtmlForGroups(string selectedGroups, TStructData tstData, string isTab)
    {
        string dcData = string.Empty;
        string dcHtml = string.Empty;
        ArrayList groups = new ArrayList();
        string[] strGroups = selectedGroups.Split(',');
        int dcIndNo = dcsPositionIndex.IndexOf(dcNo.ToString());
        DcStruct dc = (DcStruct)(dcs[dcIndNo]);

        int dcRowCnt = tstData.GetDcRowCount(dcNo.ToString());

        for (int i = 0; i < strGroups.Length; i++)
        {
            dcRowCnt++;
            if (strGroups[i].ToString() != "")
            {
                groups.Add(strGroups[i].ToString());
                //tstData.AddInsertRowToFieldArray(dcNo, dcRowCnt);
                string rowNo = GetRowNoHelper(dcRowCnt);
                int idx = tstData.fieldControlName.IndexOf(dc.KeyColumn + rowNo + "F" + dcNo);
                if (idx != -1)
                    tstData.fldValues[idx] = strGroups[i].ToString();
            }
        }

        tstData.UpdateRowCount("dc" + dcNo.ToString(), "dc" + dcNo.ToString() + "~" + dcRowCnt.ToString());
        dcHtml = "*♠*" + GetTabDcHTML(dcNo, tstData, "false");
        return dcHtml;
    }

    /// <summary>
    /// Function to execute the sql for the format grid and return the groups returned by the query.
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public string ExecuteSql(string sql, TStructData tstData, int dcNo)
    {
        string result = string.Empty;
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        LogFile.Log logObj = new LogFile.Log();
        ArrayList tempArr = new ArrayList();
        string fldValueXml = string.Empty;
        string errorLog = logObj.CreateLog(" Get groups from query", HttpContext.Current.Session["nsessionid"].ToString(), "GetChoices-FormatGrid", "new");

        string query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' user='" + HttpContext.Current.Session["user"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' >";
        //It supports only non grid fields and single parameter
        if (sql.IndexOf(":") != -1)
        {
            string parameter = sql.Substring(sql.IndexOf(":") + 1);
            parameter = parameter.Trim();

            int idx = GetFieldIndex(parameter);
            FieldStruct fld = (FieldStruct)flds[idx];
            DataTable DCTable = tstData.dsDataSet.Tables["dc" + fld.fldframeno];
            DataRow dr = tstData.DSGetRow("000", fld.fldframeno);
            string value = string.Empty;
            if (dr != null)
                value = dr[parameter].ToString();

            if (fld.datatype.ToLower() != "numeric")
                value = "'" + value + "'";

            sql = sql.Replace(":" + parameter, value);
        }
        query += "<sql>" + sql + "</sql>";
        query += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        result = asbExt.CallGetChoiceWS(transId, query);
        return result;
    }

    /// <summary>
    /// Function to get the key column values from the result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="dcNo"></param>
    /// <returns></returns>
    private ArrayList GetGroupsFromResult(string result, int dcNo, string columnName)
    {
        //DcStruct dc = (DcStruct)(dcs[dcNo - 1]);
        ArrayList groups = new ArrayList();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(result);
        XmlNodeList groupNodes = default(XmlNodeList);
        groupNodes = xmlDoc.SelectNodes("//row");
        foreach (XmlNode rowNode in groupNodes)
        {
            foreach (XmlNode groupNode in rowNode.ChildNodes)
            {
                if (groupNode.Name.ToLower() == columnName.ToLower())
                    groups.Add(groupNode.InnerText);
            }
        }
        return groups;
    }

    /// <summary>
    /// Function to construct the Format grid html based on the keycolumn values.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <param name="rowCnt"></param>
    /// <param name="tstData"></param>
    /// <param name="isTab"></param>
    /// <param name="IsTabDis"></param>
    /// <returns></returns>
    public string GetFormatDcHtml(int dcIndNo, int dcNo, int rowCnt, TStructData tstData, string isTab, string IsTabDis)
    {
        //DcStruct dc = (DcStruct)(dcs[dcNo - 1]);
        DcStruct dc = (DcStruct)(dcs[dcIndNo]);
        StringBuilder dcRows = new StringBuilder();

        string priorKeyColValue = string.Empty;
        string currentKeyColValue = string.Empty;
        subTotColValues = new ArrayList();
        string dcHtml = string.Empty;
        string headerRowHtml = string.Empty;
        string rowHtml = string.Empty;
        string subTotalRowHtml = string.Empty;
        int serialNo = 0;
        string strDcNo = Convert.ToString(dcNo);
        string strRowCnt = Convert.ToString(rowCnt);

        ArrayList keyCols = new ArrayList();
        ArrayList keyRowNos = new ArrayList();

        tstData.GetKeyColValues(dcNo, keyCols, keyRowNos);

        for (int i = 0; i < keyCols.Count; i++)
        {
            string[] rows = keyRowNos[i].ToString().Split(',');
            serialNo = 0;
            for (int j = 0; j < rows.Length; j++)
            {
                int keyRowNo = Convert.ToInt32(rows[j].ToString());
                serialNo = serialNo + 1;
                string rowNo = GetRowNoHelper(keyRowNo);

                currentKeyColValue = tstData.DSGetRow(rowNo, Convert.ToInt32(strDcNo))[dc.KeyColumn].ToString();

                headerRowHtml = string.Empty;
                rowHtml = string.Empty;
                subTotalRowHtml = string.Empty;

                if (j == 0)
                {
                    headerRowHtml = GetHeaderRowHtml(dcIndNo, dcNo, currentKeyColValue);
                    headerRowHtml = GetRowHtml(dcIndNo, dcNo, rowNo, (j + 1).ToString(), headerRowHtml, "header", currentKeyColValue);
                    priorKeyColValue = currentKeyColValue;
                    dcRows.Append(headerRowHtml);
                }

                rowHtml = GetTabFieldHtml(dcIndNo, dcNo, tstData, IsTabDis, rowNo, currentKeyColValue, keyCols, keyRowNos);
                rowHtml = GetRowHtml(dcIndNo, dcNo, rowNo, serialNo.ToString(), rowHtml, "data", "");
                UpdateSubTotalValues(dc, rowNo, tstData);
                dcRows.Append(rowHtml);

                if (j == rows.Length - 1)
                {
                    subTotalRowHtml = GetSubTotalRowHtml(dcIndNo, dcNo, priorKeyColValue);
                    subTotalRowHtml = GetRowHtml(dcIndNo, dcNo, rowNo, (j + 1).ToString(), subTotalRowHtml, "subtotal", priorKeyColValue);
                    dcRows.Append(subTotalRowHtml);
                    subTotColValues = new ArrayList();
                }
            }
        }

        return dcRows.ToString();
    }



    /// <summary>
    /// Function to construct the row html for a given group/type. 
    /// </summary>
    /// <param name="dcNo"></param>
    /// <param name="KeyColValues"></param>
    /// <param name="rowNo"></param>
    /// <param name="tstData"></param>
    /// <returns></returns>
    public string GetGroupRowHtml(int dcIndNo, int dcNo, ArrayList groupValues, string rowNo, TStructData tstData)
    {
        string rowHtml = string.Empty;
        string rowIds = string.Empty;
        int rowCnt = Convert.ToInt32(rowNo);
        for (int i = 0; i < groupValues.Count; i++)
        {
            string curRowNo = GetRowNoHelper(rowCnt);
            string groupValue = groupValues[i].ToString();
            string headerHtml = GetHeaderRowHtml(dcIndNo, dcNo, groupValue);
            rowHtml += GetRowHtml(dcIndNo, dcNo, curRowNo, i.ToString(), headerHtml, "header", groupValue);

            string dataRowHtml = GetTabFieldHtml(dcIndNo, dcNo, tstData, "false", curRowNo, groupValue, null, null);
            rowHtml += GetRowHtml(dcIndNo, dcNo, curRowNo, "1", dataRowHtml, "data", "");
            DcStruct dc = (DcStruct)(dcs[dcIndNo]);
            UpdateSubTotalValues(dc, curRowNo, tstData);

            string stRowHtml = GetSubTotalRowHtml(dcIndNo, dcNo, groupValue);
            rowHtml += GetRowHtml(dcIndNo, dcNo, curRowNo, i.ToString(), stRowHtml, "subtotal", groupValue);
            if (rowIds == "")
                rowIds = curRowNo;
            else
                rowIds += "," + curRowNo;
            rowCnt++;
        }
        return rowIds + "♣" + rowHtml;
    }

    /// <summary>
    /// Function to calculate the sub total for all rows and update the sub total row.
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="rowNo"></param>
    /// <param name="tstData"></param>
    private void UpdateSubTotalValues(DcStruct dc, string rowNo, TStructData tstData)
    {
        if (subTotColValues.Count == 0 || subTotColValues.Count < dc.SubTotalColumns.Count)
        {
            subTotColValues.Clear();
            for (int j = 0; j < dc.SubTotalColumns.Count; j++)
            {
                subTotColValues.Add(0);
            }
        }
        for (int i = 0; i < dc.SubTotalColumns.Count; i++)
        {
            int fIndex = GetFieldIndex(dc.SubTotalColumns[i].ToString());
            int noOfDecimal = 0;
            string colValue = "0";
            if (fIndex != -1)
            {
                FieldStruct fld = (FieldStruct)flds[fIndex];
                noOfDecimal = fld.flddecimal;
                if (tstData.DSGetRow(rowNo, dc.frameno) != null)
                    colValue = tstData.DSGetRow(rowNo, dc.frameno)[fld.name].ToString();
            }

            if (colValue == string.Empty) colValue = "0";
            decimal oldTotal = Convert.ToDecimal(subTotColValues[i].ToString());

            if (oldTotal == 0)
            {
                if (colValue.Contains("."))
                    colValue = colValue.Substring(0, colValue.IndexOf(".") + 1 + noOfDecimal);
                else
                    colValue = FormatDecimalValue(colValue, noOfDecimal);
                subTotColValues[i] = colValue;
            }
            else
            {
                string newTotal = Convert.ToString(oldTotal + Convert.ToDecimal(colValue));
                if (newTotal.Contains("."))
                    newTotal = newTotal.Substring(0, newTotal.IndexOf(".") + 1 + noOfDecimal);
                else
                    newTotal = FormatDecimalValue(newTotal, noOfDecimal);
                subTotColValues[i] = newTotal;
            }
        }
    }

    /// <summary>
    /// Function to construct the sub total row html for the given group/type.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <param name="keyColValue"></param>
    /// <returns></returns>
    private string GetSubTotalRowHtml(int dcIndNo, int dcNo, string keyColValue)
    {
        string dcFldRange = GetDcFieldRange(dcNo.ToString());
        string[] fldRange = dcFldRange.Split(',');
        StringBuilder stRowHtml = new StringBuilder();
        //DcStruct dc = (DcStruct)(dcs[dcNo - 1]);
        DcStruct dc = (DcStruct)(dcs[dcIndNo]);
        if (fldRange.Length > 1)
        {
            int startIndex = Convert.ToInt32(fldRange[0]);
            int endIndex = Convert.ToInt32(fldRange[1]);

            for (int i = startIndex; i <= endIndex; i++)
            {
                FieldStruct fld = (FieldStruct)flds[i];
                int stIndx = dc.SubTotalColumns.IndexOf(fld.name);

                int indx = -1;
                if (dc.DisplayTotalCol == fld.name)
                    indx = 1;
                string hdnCount = "<input type=\"hidden\" id= 'hdn" + dcNo + keyColValue + "'" + "/>";

                if (stIndx != -1)
                {
                    string lblId = "lblSt" + fld.name + keyColValue + dcNo;
                    stRowHtml.Append("<td>" + GetLabelHtml(subTotColValues[stIndx].ToString(), dc.SubTotalStyle, lblId) + "</td>");
                }
                else if (indx != -1)
                {
                    stRowHtml.Append("<td>" + GetLabelHtml("Total", dc.SubTotalStyle, "") + hdnCount + "</td>");
                }
                else
                {
                    if (fld.cwid != "-1" && fld.visibility != true)
                        stRowHtml.Append("<td></td>");
                }
            }
        }

        return stRowHtml.ToString();
    }

    private string GetNewKeyColValue(int dcNo, string fieldValue, DcStruct dc)
    {
        int index = dc.KeyColValues.IndexOf(fieldValue);
        string newKeyColValue = fieldValue;
        if (index != -1 && dc.DispKeyColValues[index].ToString() != "")
            newKeyColValue = dc.DispKeyColValues[index].ToString();
        return newKeyColValue;
    }

    /// <summary>
    /// Function to construct the Header row html for the given row or type.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <param name="fieldValue"></param>
    /// <returns></returns>
    private string GetHeaderRowHtml(int dcIndNo, int dcNo, string fieldValue)
    {
        string dcFldRange = GetDcFieldRange(dcNo.ToString());
        string[] fldRange = dcFldRange.Split(',');
        StringBuilder headerRowHtml = new StringBuilder();
        //DcStruct dc = (DcStruct)(dcs[dcNo - 1]);
        DcStruct dc = (DcStruct)(dcs[dcIndNo]);
        if (fldRange.Length > 1)
        {
            int startIndex = Convert.ToInt32(fldRange[0]);
            int endIndex = Convert.ToInt32(fldRange[1]);

            for (int i = startIndex; i <= endIndex; i++)
            {
                FieldStruct fld = (FieldStruct)flds[i];
                if (fld.name == dc.DisplayKeyCol)
                {
                    string newValue = GetNewKeyColValue(dcNo, fieldValue, dc);
                    headerRowHtml.Append("<td>" + GetLabelHtml(newValue, dc.HeaderStyle, "") + "</td>");
                }
                else
                {
                    if (fld.visibility == true)
                        headerRowHtml.Append("<td width=\"" + fld.cwid + "px\"></td>");

                    if (fld.cwid != "-1" && fld.visibility != true)
                        headerRowHtml.Append("<td></td>");
                }
            }
        }
        return headerRowHtml.ToString();
    }

    /// <summary>
    /// Function to construct the label in the sub total row with the given styles.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="style"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private string GetLabelHtml(string value, string style, string id)
    {
        string labelHtml = string.Empty;
        string[] rowStyle = style.Split(',');
        string css = "style=\"";
        for (int i = 0; i < rowStyle.Length; i++)
        {
            string[] strCss = rowStyle[i].ToString().Split('=');

            if (strCss.Length > 1)
            {
                string styleName = strCss[0].ToString();
                string styleValue = strCss[1].ToString();

                if (styleValue != string.Empty)
                {
                    if (styleName == "fontname")
                    {
                        css += "font-family:" + styleValue + ";";
                    }
                    else if (styleName == "fontsize")
                    {
                        styleValue = Convert.ToString(Convert.ToInt32(styleValue) + 3);
                        css += "font-size:" + styleValue + "px;";
                    }
                    else if (styleName == "fontstyle")
                    {
                        if (styleValue == "b") css += "font-weight:bold;";
                        else if (styleValue == "u") css += "text-decoration:underline;";
                        else if (styleValue == "s") css += "text-decoration:overline;";
                        else if (styleValue == "i") css += "font-style:italic;";
                    }
                    else if (styleName == "fontcolor")
                    {
                        if (styleValue == "clBlue") styleValue = "blue";
                        css += "color:" + styleValue + ";";
                    }
                }
            }
        }
        css += "\"";
        if (id != "")
            //label normal
            labelHtml = "<label id=\"" + id + "\" " + css + " class=\"tem family\">" + value + "</label>";
        else
            //label normal
            labelHtml = "<label " + css + " class=\"tem family\">" + value + "</label>";
        return labelHtml;
    }

    /// <summary>
    /// Function to wrap the field html into the row html.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <param name="rowNo"></param>
    /// <param name="serialNo"></param>
    /// <param name="rowHTML"></param>
    /// <param name="rowType"></param>
    /// <param name="keyColValue"></param>
    /// <returns></returns>
    private string GetRowHtml(int dcIndNo, int dcNo, string rowNo, string serialNo, string rowHTML, string rowType, string keyColValue)
    {
        string dcRowHtml = "";
        if (rowType != "data")
        {
            string rowId = string.Empty;
            string addGroupBtn = string.Empty;
            string delGroupBtn = string.Empty;
            string style = string.Empty;
            if (rowType == "header")
            {
                rowId = "hdr-" + keyColValue + "F" + dcNo;
                style = "class=\"hdrRow\"";
                addGroupBtn = "<img id='addrow" + dcNo + "'  src='../axpimages/icons/16x16/add.png' class='handCur rowadd' alt='Add Row' title='Add Row' onclick=\"javascript:AddRow('" + dcNo + "','newRow' ,'" + keyColValue + "');\"/>";
                delGroupBtn = "<img src='../axpimages/icons/16x16/delete.png' class='handCursor' alt='Delete group' title='Delete group' onclick=\"javascript:DeleteGroup(this, '" + dcNo + "','" + keyColValue + "');\" />";
            }
            else
            {
                rowId = "subt-" + keyColValue + "F" + dcNo;
                style = "class=\"stRow\"";
            }


            //if (((DcStruct)(dcs[dcNo - 1])).ispopgrid)
            if (((DcStruct)(dcs[dcIndNo])).ispopgrid)
                dcRowHtml = "<div id=\"" + rowId + "\" " + style + "><td></td>" + rowHTML + "</div>";
            else if (((DcStruct)(dcs[dcIndNo])).isgrid)//(((DcStruct)(dcs[dcNo - 1])).isgrid)
            {
                //if (((DcStruct)(dcs[dcNo - 1])).isallowdeletrows.ToString().ToLower() == "false")
                if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "false")
                    dcRowHtml = "<div id=\"" + rowId + "\" " + style + "><td>" + addGroupBtn + "</td>" + rowHTML + "</div>";
                else
                    dcRowHtml = "<div id=\"" + rowId + "\" " + style + "><td>" + delGroupBtn + "</td><td>" + addGroupBtn + "</td>" + rowHTML + "</div>";
            }
            else
                dcRowHtml = rowHTML;
        }
        else
        {
            if (((DcStruct)(dcs[dcIndNo])).ispopgrid)//(((DcStruct)(dcs[dcNo - 1])).ispopgrid)
            {
                //if (((DcStruct)(dcs[dcNo - 1])).isallowdeletrows.ToString().ToLower() == "false")
                if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "false")
                    dcRowHtml = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\">" + rowHTML + "</div>";
                else
                    dcRowHtml = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\"><td><a href=\"javascript:void(0)\" id=\"del" + "001F" + dcNo + "\" title=\"Delete row\" class=\"rowdelete\"  /></td>" + rowHTML + "</div>";
            }
            else if (((DcStruct)(dcs[dcIndNo])).isgrid)//(((DcStruct)(dcs[dcNo - 1])).isgrid)
            {
                string rowSrNoClass = string.Empty;
                if (IsFillGridCall)
                {
                    rowSrNoClass = "gridtdclass d-none";
                }
                else
                {
                    rowSrNoClass = "gridtdclass d-none";
                }
                //if (((DcStruct)(dcs[dcNo - 1])).isallowdeletrows.ToString().ToLower() == "false")
                if (((DcStruct)(dcs[dcIndNo])).isallowdeletrows.ToString().ToLower() == "false")
                    dcRowHtml = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\"><div class='gridElement " + rowSrNoClass + "'><label id=\"lblSlNo" + rowNo + "F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">" + serialNo.ToString() + "</label></div>" + rowHTML + "</div>";
                else
                    //delete grid row
                    dcRowHtml = "<div  id=sp" + dcNo + "R" + rowNo + "F" + dcNo + " class=\"editWrapTr grid-stack\"><div class='gridElement " + rowSrNoClass + "'><label id=\"lblSlNo" + rowNo + "F" + dcNo + "\" class=\"form-control w-100 border bg-transparent overflow-hidden resize-none  slno\">" + serialNo.ToString() + "</label></div>" + rowHTML + "</div>";
            }
            else
                dcRowHtml = rowHTML;

        }
        return dcRowHtml;
    }

    /// <summary>
    /// Function to check if the dc has a  associated fill grid and returns the index. 
    /// </summary>
    /// <param name="dcNo"></param>
    /// <returns></returns>
    public int GetFillGridIndex(string dcNo)
    {
        int fgIdx = -1;
        for (int i = 0; i < fgs.Count; i++)
        {
            FGStruct fg = (FGStruct)fgs[i];
            if (fg.fgtargetdc == dcNo)
            {
                fgIdx = i;
                break;
            }
        }
        return fgIdx;
    }

    /// <summary>
    /// Function to check if the grid dc is format grid.
    /// </summary>
    /// <param name="dcNo"></param>
    /// <returns></returns>
    public bool IsDcFormatGrid(int curIndNo, int dcNo)
    {
        try
        {
            //DcStruct dc = (DcStruct)(dcs[dcNo - 1]);
            DcStruct dc = (DcStruct)(dcs[curIndNo]);
            return dc.IsFormatGrid;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


    protected string findGridcol(float percentage)
    {
        int col = (Convert.ToInt32(percentage / 8) - 1);
        int rem = Convert.ToInt32(percentage % 8);

        if (col == 0)
            return "col-lg-1";
        else if (col < 12)
            return (rem >= 5) ? ("col-lg-" + (col).ToString() + smBootstrapClass(col)) : ("col-lg-" + col.ToString() + smBootstrapClass(col));
        else
            return "col-lg-12";

    }

    protected string bsClassByDatawidth(int datawidth, int i, FieldStruct fld)
    {
        bool columnModeEnabled = axdesignJObject.dcLayout != null && axdesignJObject.dcLayout != "" && axdesignJObject.dcLayout != "default";

        if (fld.ctype.ToUpper() == "MEMO" || fld.datatype.ToLower() == "text")
        {
            return " w-100 agform " + (!columnModeEnabled ? " d-block " : "") + " grid-stack-item-content form-group fldindex" + i;
        }
        else if (fld.datatype.ToUpper() == "DATE/TIME")
        {
            return " w-100 agform " + (!columnModeEnabled ? " d-block " : "") + " grid-stack-item-content form-group fldindex" + i;
        }
        else if (fld.ctype.ToUpper() == "CHECK LIST" || fld.ctype.ToUpper() == "RADIO GROUP")
        {
            return " w-100 agform " + (!columnModeEnabled ? " d-block " : "") + " grid-stack-item-content form fldindex" + i;
        }
        else
        {
            int col = Convert.ToInt32(datawidth / 15) + 2;
            col = col - 2;
            return (datawidth <= 15) ? " w-100 agform " + (!columnModeEnabled ? " d-block " : "") + " grid-stack-item-content form-group fldindex" + i : " w-100 agform " + (!columnModeEnabled ? " d-block " : "") + " grid-stack-item-content form-group fldindex" + i;
        }
    }

    protected string xywhGridStack(FieldStruct fld = new FieldStruct(), bool isTable = false, int GridX = 0, int GridY = 0)
    {
        var designMode = false;

        if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
        {
            designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
        }
        int dcNo = fld.fldFrameNo;
        int dcIndNo = dcsPositionIndex.IndexOf(dcNo.ToString());
        bool isGrid = ((DcStruct)(dcs[dcIndNo])).isgrid;
        int x = 0;
        int y = 0;
        int w = 0;
        int minW = 2;
        int maxW = 0;
        int h = 0;
        int minH = 0;
        int maxH = 0;
        int autoPosition = 0;

        if (axdesignJObject == null)
        {
            axdesignJObject = new DesignObject();
            axdesignJObject.newDesign = true;
        }
        //try
        //{
        //    isFreshDesign = axdesignJObject.transid != null && axdesignJObject.transid == transId ? false : true;
        //}
        //catch (Exception ex)
        //{
        //    isFreshDesign = true;
        //}
        bool freshAutoArrange = false;
        string tableWidth = string.Empty;
        //if (isFreshDesign) {
        //    autoPosition = 1;
        //    tableWidth =  (fld.fldlength > 51 ? 400 : (fld.fldlength <= 15) ? 150 : (fld.fldlength * 8)).ToString();
        //}

        //if (fld.datatype == "special")
        //{
        //    freshAutoArrange = true;
        //}

        //if is not table pixels
        Dc curDc;
        string DcLayout = axdesignJObject.dcLayout;
        if (DcLayout == null || DcLayout == "default")
        {
            if (axdesignJObject.dcs == null)
            {
                axdesignJObject.dcs = new List<Dc>();
            }
            curDc = axdesignJObject.dcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString());
        }
        else
        {
            if (axdesignJObject.newdcs == null)
            {
                axdesignJObject.newdcs = new List<Dc>();
            }
            curDc = axdesignJObject.newdcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString());
        }
        try
        {
            if (curDc == null)
            {
                curDc = new Dc();
                curDc.dc_id = dcNo.ToString();
            }
        }
        catch (Exception ex)
        {
            //while (axdesignJObject.dcs.Count != dcNo)
            //{
            //    axdesignJObject.dcs.Add(new Dc());
            //    if (axdesignJObject.dcs.Count == dcNo)
            //    {
            //        axdesignJObject.dcs[axdesignJObject.dcs.Count - 1].dc_id = dcNo.ToString();
            //    }
            //}
        }
        curDc.isGrid = ((DcStruct)(dcs[dcIndNo])).isgrid ? "T" : "F";
        if (!isTable)
        {

            FieldsDesign fDsign = null;
            try
            {
                fDsign = curDc.fieldsDesign.FirstOrDefault(elm => elm.fld_id == fld.name);
                if (transId == "sect" && (fld.name == "filename" || fld.name == "css_js_src"))
                    fDsign.width = 36;
            }
            catch (Exception ex)
            {

            }

            //if (!isFreshDesign && fDsign != null)
            if (fDsign != null)
            {
                /*if (fDsign.visibility || designMode)
                {*/
                x = fDsign.x;
                y = fDsign.y;
                w = fDsign.width;
                h = fDsign.height;
                /*}else
                {
                    w = curDc.fieldsDesign.FirstOrDefault(elm => elm.fld_id == fld.name).width = 0;
                    h = curDc.fieldsDesign.FirstOrDefault(elm => elm.fld_id == fld.name).height = 0;
                }*/
            }
            else
            {
                //if (isFreshDesign && fDsign == null)
                if (fDsign == null)
                {
                    //autoPosition = 1;
                    //data-gs-x="2" data-gs-y="5" data-gs-width="1" data-gs-height="1" 
                    if (fld.ctype.ToUpper() == "MEMO" || fld.datatype.ToLower() == "text")
                    {
                        w = 15; h = 2;
                        //return " data-gs-width=\"5\" data-gs-height=\"1\" data-gs-min-height=\"2\" ";
                    }
                    else if (fld.datatype.ToUpper() == "DATE/TIME")
                    {
                        w = 9; h = 1;
                        //return " data-gs-width=\"3\" data-gs-height=\"1\" data-gs-max-height=\"1\" ";
                    }
                    else if ((fld.ctype.ToUpper() == "CHECK LIST" && !isGrid) || (fld.ctype.ToUpper() == "RADIO GROUP" && !isGrid))
                    {
                        w = 36; h = 1;
                        //return " data-gs-width=\"12\" data-gs-height=\"1\" data-gs-max-height=\"1\" ";
                    }
                    else if (fld.datatype.ToLower() == "image")
                    {
                        w = 10; h = 4;
                    }
                    else
                    {
                        int col = (Convert.ToInt32(fld.fldlength / 15) + 2) * 3;
                        if (col > 36) { col = 36; }
                        if (fld.fldlength <= 15)
                        {
                            w = 9; h = 1;
                        }
                        else
                        {
                            w = col; h = 1;
                        }
                        //return (fld.fldlength <= 15) ? " data-gs-width=\"3\" data-gs-height=\"1\" data-gs-max-height=\"1\" " : " data-gs-width=\"" + col + "\"  data-gs-max-height=\"1\" ";
                    }
                    if (!fld.visibility)
                    {

                        if (curDc.fieldsDesign == null)
                        {
                            curDc.fieldsDesign = new List<FieldsDesign>();
                        }

                        if (fld.datatype == "special" && fDsign == null && !axdesignJObject.newDesign && !isFreshDesign)
                        {
                            freshAutoArrange = true;
                        }



                        curDc.fieldsDesign.Add(new FieldsDesign());
                        int newX = 0;
                        int newY = 0;
                        if (freshAutoArrange || isGrid)
                        {

                            if (curDc.fieldsDesign.Count == 1)
                            {
                                newX = 0;
                                newY = 0;
                            }
                            else
                            {
                                newX = curDc.fieldsDesign[curDc.fieldsDesign.Count - 2].x + curDc.fieldsDesign[curDc.fieldsDesign.Count - 2].width;

                                int currentY = curDc.fieldsDesign[curDc.fieldsDesign.Count - 2].y;
                                if (newX + w > 36)
                                {
                                    newX = 0;
                                    int maxHeightFD = curDc.fieldsDesign.Where(i => i.y == currentY).Select(i => i.height).Max();
                                    newY = currentY + maxHeightFD;
                                }
                                else
                                {

                                    newY = currentY;

                                }

                            }

                        }
                        else
                        {
                            if (curDc.fieldsDesign.Count == 1)
                            {
                                newX = 0;
                                newY = 0;
                            }
                            else
                            {
                                newX = 0;
                                int currentY = curDc.fieldsDesign[curDc.fieldsDesign.Count - 2].y;
                                int maxHeightFD = curDc.fieldsDesign.Where(i => i.y == currentY).Select(i => i.height).Max();
                                newY = currentY + maxHeightFD;
                            }
                        }
                        x = curDc.fieldsDesign[curDc.fieldsDesign.Count - 1].x = newX;
                        y = curDc.fieldsDesign[curDc.fieldsDesign.Count - 1].y = newY;
                        curDc.fieldsDesign[curDc.fieldsDesign.Count - 1].width = w;
                        curDc.fieldsDesign[curDc.fieldsDesign.Count - 1].height = h;
                        //if (fld.ctype == "button" && fDsign == null && !axdesignJObject.newDesign && !isFreshDesign)
                        if (fld.ctype == "button" && fDsign == null && !axdesignJObject.newDesign)
                        {
                            curDc.fieldsDesign[curDc.fieldsDesign.Count - 1].visibility = false;
                        }
                        else
                        {
                            curDc.fieldsDesign[curDc.fieldsDesign.Count - 1].visibility = true;
                        }
                        curDc.fieldsDesign[curDc.fieldsDesign.Count - 1].fld_id = fld.name;
                    }
                }
            }

            //set min/max height
            if (fld.ctype.ToUpper() == "MEMO" && fld.fldType.ToLower() != "rich text")
            {
                minH = 2;
            }
            else if (fld.datatype.ToLower() == "text" && (fld.name.Contains("rtf_") || fld.name.Contains("rtfm_") || fld.fldType.ToLower() == "rich text"))
            {
                minH = 5;
            }
            else if (fld.datatype.ToLower() == "image")
            {
                minH = 2;
            }
            else if (fld.ctype.ToUpper() == "RADIO GROUP" && fld.type.ToLower() != "h" && !isGrid)//fld.ctype.ToUpper() == "CHECK LIST" || (
            {
                minH = 2;
            }
            else if (fld.ctype.ToUpper() == "RADIO GROUP" && fld.type.ToLower() == "h" && !isGrid)
            {
                minH = 1;
            }
            else if (fld.datatype.ToLower() == "text" && fld.ctype.ToLower() == "")
            {
                minH = 2;
            }
            else if (fld.ctype == "button")
            {
                minH = 1;
            }
            else
            {
                maxH = 1;
            }

            if (h < minH)
            {
                try
                {
                    h = minH;
                    curDc.fieldsDesign[curDc.fieldsDesign.Count - 1].height = h;
                }
                catch (Exception ex) { }
            }

            if (w < minW)
            {
                try
                {
                    w = minW;
                    curDc.fieldsDesign[curDc.fieldsDesign.Count - 1].width = w;
                }
                catch (Exception ex) { }
            }

            if (DcLayout == null || DcLayout == "default")
            {
                if (axdesignJObject.dcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString()) == null)
                {
                    axdesignJObject.dcs.Add(curDc);
                }
                else
                {
                    int index = axdesignJObject.dcs.IndexOf(axdesignJObject.dcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString()));
                    axdesignJObject.dcs[index] = curDc;
                }
            }
            else
            {
                if (axdesignJObject.newdcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString()) == null)
                {
                    axdesignJObject.newdcs.Add(curDc);
                }
                else
                {
                    int index = axdesignJObject.newdcs.IndexOf(axdesignJObject.newdcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString()));
                    axdesignJObject.newdcs[index] = curDc;
                }
            }

            return " " + (x >= 0 ? "data-gs-x=\"" + x + "\"" : "") + " " + (y >= 0 ? "data-gs-y=\"" + y + "\"" : "") + " " + (w > 0 ? "data-gs-width=\"" + w + "\"" : "") + " " + (minW > 0 ? "data-gs-min-width=\"" + minW + "\"" : "") + " " + (maxW > 0 ? "data-gs-max-width=\"" + maxW + "\"" : "") + " " + (h > 0 ? "data-gs-height=\"" + h + "\"" : "") + " " + (minH > 0 ? "data-gs-min-height=\"" + minH + "\"" : "") + " " + (maxH > 0 ? "data-gs-max-height=\"" + maxH + "\"" : "") + " " + ("data-gs-auto-position=\"" + autoPosition + "\"") + " ";
        }
        else
        {
            //if(tableWidth == "") {
            //    tableWidth = (fld.fldlength > 51 ? 400 : (fld.fldlength <= 15) ? 150 : (fld.fldlength * 8)).ToString();
            //}

            if (curDc.isGrid == "")
            {
                return "0";
            }


            TableDesign tDsign = null;
            try
            {
                if (curDc.tableDesign != null)
                {
                    tDsign = curDc.tableDesign.FirstOrDefault(elm => elm.fld_id == fld.name);
                }

            }
            catch (Exception ex)
            {

            }

            try
            {
                /*if (tDsign != null && !tDsign.visibility && !designMode)
                {
                    tableWidth = "0";
                }
                else */
                //if (!isFreshDesign && tDsign != null)


                if (curDc.tableDesign == null)
                {
                    curDc.tableDesign = new List<TableDesign>();
                }

                if (fld.datatype == "special" && tDsign == null && !axdesignJObject.newDesign && !isFreshDesign)
                {
                    freshAutoArrange = true;
                }



                curDc.tableDesign.Add(new TableDesign());


                if (tDsign != null)
                {
                    tableWidth = tDsign.width.ToString();
                }
                else
                {
                    if (!fld.visibility)
                    {

                        tableWidth = (fld.fldlength > 51 ? 400 : (fld.fldlength <= 15) ? 150 : (fld.fldlength * 8)).ToString();

                        curDc.tableDesign[curDc.tableDesign.Count - 1].width = Convert.ToInt32(tableWidth != "" ? tableWidth : "0");

                        if (fld.ctype == "button" && tDsign == null && !axdesignJObject.newDesign)
                        {
                            curDc.tableDesign[curDc.tableDesign.Count - 1].visibility = false;
                        }
                        else
                        {
                            curDc.tableDesign[curDc.tableDesign.Count - 1].visibility = true;
                        }
                        curDc.tableDesign[curDc.tableDesign.Count - 1].fld_id = fld.name;
                    }
                }
            }
            catch (Exception ex)
            {

            }


            if (DcLayout == null || DcLayout == "default")
            {
                if (axdesignJObject.dcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString()) == null)
                {
                    axdesignJObject.dcs.Add(curDc);
                }
                else
                {
                    int index = axdesignJObject.dcs.IndexOf(axdesignJObject.dcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString()));
                    axdesignJObject.dcs[index] = curDc;
                }
            }
            else
            {
                if (axdesignJObject.newdcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString()) == null)
                {
                    axdesignJObject.newdcs.Add(curDc);
                }
                else
                {
                    int index = axdesignJObject.newdcs.IndexOf(axdesignJObject.newdcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString()));
                    axdesignJObject.newdcs[index] = curDc;
                }
            }
            return tableWidth != "" ? tableWidth : "0";
            //return " " + 0 + " ";
            //return (fld.fldlength > 51 ? 400 : (fld.fldlength <= 15) ? 150 : (fld.fldlength * 8)).ToString();
        }
    }

    private string smBootstrapClass(int number)
    {
        if (number < 2)
        {
            return " ";
        }
        else
        {

            return " col-sm-" + (number + (number / 2));
        }
    }
    public string OldToNewDesignJSONParser(string oldjson, XmlDocument xmlDoc)
    {
        JObject json = JObject.Parse(oldjson);
        StringBuilder str = new StringBuilder();
        Boolean previouclass = false;
        Boolean cpMode = json["cpMode"] != null ? Convert.ToBoolean(json["cpMode"]) : true;
        char isgrid = 'F';
        string gridStretch = "false";
        str.Append("[");
        str.Append("{\"transid\":\"" + json["tstructName"] + "\",");
        str.Append("\"compressedMode\":" + cpMode.ToString().ToLower() + ",");
        str.Append("\"newDesign\":false,");
        str.Append("\"staticRunMode\":false,");
        str.Append("\"wizardDC\":false,");
        str.Append("\"selectedLayout\":null,");
        str.Append("\"selectedFontSize\":14,");
        str.Append("\"selectedControlHeight\":25,");
        str.Append("\"tstUpdatedOn\":\"" + tstUpdateOn + "\",");
        str.Append("\"dcLayout\":\"default\",");
        str.Append("\"formWidth\":\"100\",");
        str.Append("\"formAlignment\":\"default\",");
        str.Append("\"fieldCaptionWidth\":\"30\",");
        str.Append("\"formLabel\":\"[]\",");
        str.Append("\"buttonFieldFont\":\"[]\",");
        str.Append("\"dcs\": [");
        foreach (JObject dccontent in json["dcs"].Children<JObject>())
        {
            int yaxsis = 0;
            int xaxsis = 0;
            int xaxsisGrid = 0;
            int yaxsisGrid = 0;
            int imagewidth = 0;
            Boolean nextline = false;
            Boolean nextlineGrid = false;
            int thisDc = dccontent["id"].ToString().StartsWith("divDc") ? Convert.ToInt32(dccontent["id"].ToString().Substring(5)) : dccontent["id"].ToString().StartsWith("sp") ? Convert.ToInt32(dccontent["id"].ToString().Substring(dccontent["id"].ToString().LastIndexOf("F") + 1)) : dccontent["id"].ToString().StartsWith("#gridHd") ? Convert.ToInt32(dccontent["id"].ToString().Substring(7)) : 0;
            str.Append("{\"dc_id\": \"" + thisDc + "\",");
            str.Append("\"fieldsDesign\":[");
            var dcelementcontent = dccontent["elements"];
            Boolean isSpecialField = false;
            Boolean isCkeditor = false;
            foreach (var item in dcelementcontent.Children())
            {
                JArray j = new JArray();
                try
                {
                    j = (JArray)item;
                }
                catch (Exception ex)
                {

                }
                foreach (var item1 in j)
                {
                    JObject j1 = (JObject)item1;
                    var msgProperty = j1.Property("classes");
                    if (msgProperty != null && j1["classes"].ToString() == "row")
                    {
                        previouclass = true;
                    }
                    else
                    {
                        previouclass = false;
                        nextline = false;
                    }

                    var dcinnerelement = j1["elements"];
                    foreach (var iteminner in dcinnerelement.Children())
                    {
                        JArray jinner = new JArray();
                        try
                        {
                            jinner = (JArray)iteminner;
                        }
                        catch (Exception ex)
                        {
                            //jinner = JArray.Parse("[" + iteminner.ToString() + "]");
                        }
                        foreach (var iteminner1 in jinner)
                        {
                            JObject j1inner = (JObject)iteminner1;
                            string classname = ((Newtonsoft.Json.Linq.JValue)(j1inner["classes"])).Value.ToString();
                            if (classname != null && !(classname.Contains("grid-icons")) && !(classname.Contains("griddivColumn")))
                            {
                                System.Xml.XmlNodeList Memonode;
                                System.Xml.XmlNodeList checkboxnode;
                                System.Xml.XmlNodeList radiobuttonnode;
                                System.Xml.XmlNodeList image = null;
                                System.Xml.XmlNodeList ckeditor;
                                int heigntField = 1;
                                try
                                {
                                    Memonode = xmlDoc.SelectNodes("root/" + j1inner["id"].ToString().Substring(2) + "/a31[@ctype='Memo']");
                                    image = xmlDoc.SelectNodes("root/" + j1inner["id"].ToString().Substring(2) + "/a3");
                                    if (image.Count > 0 && image[0].InnerText.ToLower() == "image")
                                    {
                                        isSpecialField = true;
                                        if (j1inner["height"] != null)
                                        {
                                            int pxHeight = Convert.ToInt32(j1inner["height"].ToString().Replace("px", string.Empty));
                                            if (cpMode)
                                            {
                                                heigntField = Convert.ToInt32(Math.Round(Convert.ToDouble((pxHeight + 22 + 17) / 41)));
                                            }
                                            else
                                            {
                                                heigntField = Convert.ToInt32(Math.Round(Convert.ToDouble((pxHeight + 20 + 17) / 48)));
                                            }
                                        }
                                        else
                                        {
                                            heigntField = 2;
                                        }
                                    }
                                    checkboxnode = xmlDoc.SelectNodes("root/" + j1inner["id"].ToString().Substring(2) + "/a31[@ctype='Check list']");
                                    radiobuttonnode = xmlDoc.SelectNodes("root/" + j1inner["id"].ToString().Substring(2) + "/a31[@ctype='Radio group']");
                                    ckeditor = xmlDoc.SelectNodes("root/" + j1inner["id"].ToString().Substring(2) + "/a1");
                                    if (Memonode.Count > 0 || checkboxnode.Count > 0 || radiobuttonnode.Count > 0)
                                    {
                                        isSpecialField = true;
                                        heigntField = 2;
                                    }
                                    if (ckeditor.Count > 0 && (ckeditor[0].InnerText.ToLower().Contains("rtf_") || ckeditor[0].InnerText.ToLower().Contains("rtfm_")))
                                    {
                                        isCkeditor = true;
                                        heigntField = 5;
                                    }

                                    int leastMDValue = GetLeastMDValue(classname);

                                    if (image.Count > 0 && image[0].InnerText.ToLower() == "image")
                                    {
                                        imagewidth = leastMDValue * 3;
                                    }
                                    if (xaxsis + (leastMDValue * 3) > 36)
                                    {
                                        if (!isSpecialField)
                                        {
                                            yaxsis += 1;
                                            xaxsis = 0;
                                        }
                                        else if (isCkeditor)
                                        {
                                            yaxsis += 5;
                                            isCkeditor = false;
                                            isSpecialField = false;
                                        }
                                        else if (isSpecialField && image.Count > 0)
                                        {
                                            yaxsis += 1;
                                            xaxsis = imagewidth;

                                        }
                                        else if (isSpecialField)
                                        {
                                            yaxsis += 2;
                                            xaxsis = 0;
                                            isSpecialField = false;
                                        }
                                    }
                                    str.Append("{\"fld_id\":" + "\"" + j1inner["id"].ToString().Substring(2) + "\",");
                                    str.Append("\"x\":" + xaxsis + ",");
                                    str.Append("\"y\":" + yaxsis + ",");
                                    str.Append("\"width\":" + (leastMDValue * 3) + ",");
                                    str.Append("\"visibility\":true,");
                                    str.Append("\"height\":" + heigntField + "},");
                                    if (previouclass)
                                    {
                                        if (!isSpecialField)
                                        {
                                            yaxsis++;
                                        }
                                        else if (isCkeditor)
                                        {
                                            yaxsis += 5;
                                            isCkeditor = false;
                                            isSpecialField = false;
                                        }
                                        else if (isSpecialField)
                                        {
                                            yaxsis += 2;
                                            isSpecialField = false;
                                        }
                                        xaxsis = 0;
                                        nextline = true;
                                    }
                                    if (leastMDValue > 0 && !(nextline))
                                    {
                                        xaxsis += leastMDValue * 3;
                                    }
                                }
                                catch
                                {
                                }
                            }

                            //Grid Elment
                            else if (classname != null && classname.Contains("grid-icons") || classname.Contains("griddivColumn"))
                            {
                                try
                                {
                                    int mdcountTotal = 0;
                                    isgrid = 'T';
                                    var dcgridelement = j1inner["elements"];
                                    foreach (var itemgridelement in dcgridelement.Children())
                                    {

                                        JObject jgridinner = (JObject)itemgridelement;
                                        string classnameGrid = ((Newtonsoft.Json.Linq.JValue)(jgridinner["classes"])).Value.ToString();
                                        bool isSingleLength = classname.Length - (classname.LastIndexOf("col-md") + 7) == 1;

                                        int leastMDValue = GetLeastMDValue(classnameGrid);
                                        int heightfieldgrid = 1;
                                        mdcountTotal += leastMDValue;
                                        if (mdcountTotal > 12)
                                        {
                                            yaxsisGrid++;
                                            mdcountTotal = leastMDValue;
                                            xaxsisGrid = 0;
                                            nextlineGrid = true;
                                        }
                                        else
                                        {
                                            nextlineGrid = false;
                                        }

                                        str.Append("{\"fld_id\":" + "\"" + jgridinner["id"].ToString().Substring(6) + "\",");
                                        str.Append("\"x\":" + xaxsisGrid + ",");
                                        str.Append("\"y\":" + yaxsisGrid + ",");
                                        str.Append("\"width\":" + (leastMDValue * 3) + ",");
                                        if (jgridinner["isVisible"] != null)
                                        {
                                            str.Append("\"visibility\":" + jgridinner["isVisible"].ToString().ToLower() + ",");
                                        }
                                        else
                                        {
                                            str.Append("\"visibility\":true,");
                                        }
                                        str.Append("\"height\":" + heightfieldgrid + "},");
                                        if (leastMDValue > 0)
                                        {
                                            xaxsisGrid += leastMDValue * 3;
                                        }


                                    }
                                }
                                catch (Exception ex) { }
                            }
                            //Table head
                            if (classname != null && classname.Contains("griddivColumn"))
                            {

                                var dcTableElement = j1inner["elements"];
                                str.Append("],");
                                str.Append("\"tableDesign\":[");
                                foreach (var itemTableElement in dcTableElement.Children())
                                {
                                    JObject jTableinner = (JObject)itemTableElement;
                                    if (jTableinner["id"].ToString().StartsWith("uniqueEditDeleteAct") || jTableinner["id"].ToString().StartsWith("uniqueThHead"))
                                        str.Append("{\"fld_id\":" + "\"" + jTableinner["id"].ToString() + "\",");
                                    else
                                        str.Append("{\"fld_id\":" + "\"" + jTableinner["id"].ToString().Substring(3) + "\",");
                                    if (j1inner["gridStretch"] != null)
                                    {
                                        gridStretch = j1inner["gridStretch"].ToString();
                                    }
                                    if (jTableinner["visibility"] != null)
                                    {
                                        if (jTableinner["visibility"].ToString().ToLower() == "none")
                                            str.Append("\"visibility\":false,");
                                        else
                                            str.Append("\"visibility\":true,");
                                    }
                                    else
                                    {
                                        str.Append("\"visibility\":true,");
                                    }
                                    str.Append("\"width\" :" + "\"" + Math.Round(Convert.ToDouble(jTableinner["width"].ToString().Replace("px", string.Empty))) + "\"},");
                                }
                            }
                        }

                    }

                }
            }
            str.Append("],");
            if (isgrid == 'T')
            {
                str.Append("\"gridStretch\":" + gridStretch.ToString().ToLower() + ",");
            }
            str.Append("\"isGrid\":\"" + isgrid + "\"},");
        }
        str.Append("],");
        str.Append("\"newdcs\":[]");
        str.Append("}]");
        string finaljson = str.ToString().Replace(",]", "]");
        return finaljson;
    }

    public int GetLeastMDValue(string classNames)
    {
        string[] clsArray = classNames.Split(' ');
        ArrayList mdList = new ArrayList();
        foreach (var item in clsArray)
        {
            if (item.ToString().Contains("col-md"))
            {
                mdList.Add(item.ToString());
            }
        }
        ArrayList numbers = new ArrayList();
        foreach (string item in mdList)
        {
            numbers.Add(int.Parse(item.Substring(item.LastIndexOf('-') + 1)));
        }
        int min = 0;
        min = (int)numbers[0];
        foreach (int x in numbers)
        {
            if (min > x)
            {
                min = x;
            }
        }
        return min;
    }

    private string NodeApiCall(string APIName, string DATA)
    {
        string response = string.Empty;
        LogFile.Log logObj = new LogFile.Log();
        string URL = System.Configuration.ConfigurationManager.AppSettings["NodeAPI"] + APIName;
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.ContentLength = DATA.Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(DATA);
            requestWriter.Close();

            WebResponse webResponse = request.GetResponse();
            Stream webStream = webResponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(webStream);
            response = responseReader.ReadToEnd();
            logObj.CreateLog(APIName + " - " + response + "ApiName - " + APIName + "DataToApi - " + DATA, HttpContext.Current.Session["nsessionid"].ToString(), "GetPublishNodeApiCall", "new");
            responseReader.Close();
        }
        catch (Exception e)
        {
            logObj.CreateLog(APIName + " - " + response + " ApiName - " + APIName + " DataToApi - " + DATA + " Exception - " + e.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetPublishNodeApiCall", "new");

        }
        return response;
    }

    public string GetPublishNodeApiCall(string nodeAccessToken, string session_id, string utl, string userName, string appsesskey)
    {
        string DATA = "{\"session_id\":\"" + session_id + "\"" + ",\"utl\":\"" + utl + "\"" + ",\"userName\":\"" + userName + "\"" + ",\"authorization\":\"" + nodeAccessToken + "\"" + ",\"appSKey\":\"" + appsesskey + "\"" + ",\"transid\":\"" + transId.ToLower() + "\"" + ",\"module\":\"TSTRUCT\",\"type\":\"PUBLISH\"}";
        string response = NodeApiCall("getLayout", DATA);
        LogFile.Log logObj = new LogFile.Log();
        try
        {
            JObject JSONResponse = JObject.Parse(response);
            if (JSONResponse["status"].ToString().ToLower() == "true")
            {
                JToken outer = JToken.Parse(response);
                JArray inner = outer["metaData"].Value<JArray>();
                var list = new List<KeyValuePair<string, int>>();
                int contentplace = 0;
                int contentDesign_id = 0;
                int i = 0;
                foreach (JObject content in inner.Children<JObject>())
                {
                    foreach (JProperty prop in content.Properties())
                    {
                        list.Add(new KeyValuePair<string, int>(prop.Value.ToString(), i));
                        i++;
                    }
                }
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Key.ToLower() == "content")
                    {
                        contentplace = list[j].Value;
                    }
                    if (list[j].Key.ToLower() == "design_id")
                    {
                        contentDesign_id = list[j].Value;
                    }
                }

                if (((Newtonsoft.Json.Linq.JContainer)(JSONResponse["data"])).Count > 0)
                {
                    IsPublish = true;
                    JToken JtokenValue = JSONResponse["data"][0][contentplace];
                    Publish_id = JSONResponse["data"][0][contentDesign_id].ToString();
                    return JtokenValue.ToString();
                }

            }
            return string.Empty;

        }
        catch (Exception e)
        {
            logObj.CreateLog("Exception - " + e.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetPublishNodeApiCall", "new");
            return string.Empty;
        }
    }

    public string GetSaveNodeApiCall(string nodeAccessToken, string session_id, string utl, string userName, string appsesskey)
    {
        string DATA = "{\"session_id\":\"" + session_id + "\"" + ",\"utl\":\"" + utl + "\"" + ",\"userName\":\"" + userName + "\"" + ",\"authorization\":\"" + nodeAccessToken + "\"" + ",\"appSKey\":\"" + appsesskey + "\"" + ",\"transid\":\"" + transId.ToLower() + "\"" + ",\"module\":\"TSTRUCT\",\"type\":\"SAVE\"}";
        string response = NodeApiCall("getLayout", DATA);
        LogFile.Log logObj = new LogFile.Log();
        try
        {
            JObject JSONResponse = JObject.Parse(response);
            if (JSONResponse["status"].ToString().ToLower() == "true")
            {
                JToken outer = JToken.Parse(response);
                JArray inner = outer["metaData"].Value<JArray>();
                var list = new List<KeyValuePair<string, int>>();
                int contentplace = 0;
                int contentDesign_id = 0;
                int publishid = 0;
                int ispublishid = 0;
                int i = 0;
                foreach (JObject content in inner.Children<JObject>())
                {
                    foreach (JProperty prop in content.Properties())
                    {
                        list.Add(new KeyValuePair<string, int>(prop.Value.ToString(), i));
                        i++;
                    }
                }
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Key.ToLower() == "content")
                    {
                        contentplace = list[j].Value;
                    }
                    if (list[j].Key.ToLower() == "design_id")
                    {
                        contentDesign_id = list[j].Value;
                    }
                    if (list[j].Key.ToLower() == "parent_design_id")
                    {
                        publishid = list[j].Value;
                    }
                    if (list[j].Key.ToLower() == "is_publish")
                    {
                        ispublishid = list[j].Value;
                    }

                }

                if (((Newtonsoft.Json.Linq.JContainer)(JSONResponse["data"])).Count > 0)
                {

                    JToken JtokenValue = JSONResponse["data"][0][contentplace];
                    save_id = JSONResponse["data"][0][contentDesign_id].ToString();
                    Publish_id = JSONResponse["data"][0][publishid].ToString();
                    Is_Publish = JSONResponse["data"][0][ispublishid].ToString();
                    return JtokenValue.ToString();
                }
            }
            return string.Empty;

        }
        catch (Exception e)
        {
            logObj.CreateLog("Exception - " + e.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetPublishNodeApiCall", "new");
            return string.Empty;
        }
    }


    public void SetSaveNodeApiCall(string nodeAccessToken, string session_id, string utl, string userName, string appsesskey)
    {
        string DATA = "{\"session_id\":\"" + session_id + "\"" + ",\"utl\":\"" + utl + "\"" + ",\"userName\":\"" + userName + "\"" + ",\"authorization\":\"" + nodeAccessToken + "\"" + ",\"appSKey\":\"" + appsesskey + "\"" + ",\"transid\":\"" + transId.ToLower() + "\"" + ",\"content\":" + axdesignJson + ",\"type\":\"SAVE\",\"module\":\"TSTRUCT\"}";
        string response = NodeApiCall("setLayoutSave", DATA);
        LogFile.Log logObj = new LogFile.Log();
        try
        {
            JObject JSONResponse = JObject.Parse(response);
            save_id = JSONResponse["data"].ToString();
        }
        catch (Exception e)
        {
            logObj.CreateLog("Exception - " + e.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetPublishNodeApiCall", "new");
        }
    }

    public void SetPublishNodeApiCall(string nodeAccessToken, string session_id, string utl, string userName, string appsesskey)
    {
        string DATA = "{\"session_id\":\"" + session_id + "\"" + ",\"utl\":\"" + utl + "\"" + ",\"userName\":\"" + userName + "\"" + ",\"authorization\":\"" + nodeAccessToken + "\"" + ",\"appSKey\":\"" + appsesskey + "\"" + ",\"transid\":\"" + transId.ToLower() + "\"" + ",\"design_id\":\"" + save_id + "\"}";
        string response = NodeApiCall("setLayoutPublish", DATA);
        LogFile.Log logObj = new LogFile.Log();
        try
        {
            JObject JSONResponse = JObject.Parse(response);
            Publish_id = JSONResponse["data"].ToString();
        }
        catch (Exception e)
        {
            logObj.CreateLog("Exception - " + e.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetPublishNodeApiCall", "new");
        }
    }
    public string GetOldLayoutNodeApi(string nodeAccessToken, string session_id, string utl, string userName, string appsesskey)
    {
        string DATA = "{\"session_id\":\"" + session_id + "\"" + ",\"utl\":\"" + utl + "\"" + ",\"userName\":\"" + userName + "\"" + ",\"authorization\":\"" + nodeAccessToken + "\"" + ",\"appSKey\":\"" + appsesskey + "\"" + ",\"transid\":\"" + transId + "\"" + ",\"type\":\"axDesign\"}";
        string response = NodeApiCall("getOldLayout", DATA);
        LogFile.Log logObj = new LogFile.Log();
        try
        {
            JObject JSONResponse = JObject.Parse(response);
            if (JSONResponse["status"].ToString().ToLower() == "true")
            {
                JToken outer = JToken.Parse(response);
                JArray inner = outer["metaData"].Value<JArray>();
                var list = new List<KeyValuePair<string, int>>();
                int contentplace = 0;
                int i = 0;
                foreach (JObject content in inner.Children<JObject>())
                {
                    foreach (JProperty prop in content.Properties())
                    {
                        list.Add(new KeyValuePair<string, int>(prop.Value.ToString(), i));
                        i++;
                    }
                }
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Key.ToLower() == "value")
                    {
                        contentplace = list[j].Value;
                    }
                }

                if (((Newtonsoft.Json.Linq.JContainer)(JSONResponse["data"])).Count > 0)
                {
                    IsPublish = true;
                    JToken JtokenValue = JSONResponse["data"][0][contentplace];
                    return JtokenValue.ToString();
                }
            }
            return string.Empty;
        }
        catch (Exception e)
        {
            logObj.CreateLog("Exception - " + e.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetPublishNodeApiCall", "new");
            return string.Empty;
        }

    }

    public DesignObject getDesignObjectFromJson(string axdesignJson)
    {
        DesignObject axdesignJObject = new DesignObject();
        try
        {

            axdesignJObject = JsonConvert.DeserializeObject<DesignObject>(axdesignJson.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' }));
            //if (axdesignJObject.transid != transId)
            //{
            //    axdesignJObject = null;
            //}
            if (axdesignJObject.transid == null)
            {
                axdesignJObject.newDesign = true;
                isFreshDesign = true;
            }
            else
            {
                isFreshDesign = false;
            }
        }
        catch (Exception ex)
        {

        }
        return axdesignJObject;
    }

    private void getDesignDataPerf()
    {
        if (HttpContext.Current.Session["Axp_DesignJson"] != null && HttpContext.Current.Session["Axp_DesignJson"].ToString() != "")
        {
            axdesignJson = HttpContext.Current.Session["Axp_DesignJson"].ToString();
            HttpContext.Current.Session["Axp_DesignJson"] = null;
        }
        else if (HttpContext.Current.Session["Axp_OldDesignJson"] != null && HttpContext.Current.Session["Axp_OldDesignJson"].ToString() != "")
        {
            //Parsing old JSON to new JSON 
            try
            {
                axdesignJson = OldToNewDesignJSONParser(HttpContext.Current.Session["Axp_OldDesignJson"].ToString(), xmlDoc);
            }
            catch (Exception ex)
            {
                axdesignJson = string.Empty;
            }
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDW fdwObj = new FDW();
            string designKey = Constants.REDISTSTRUCTAXDESIGN;
            fdwObj.SaveInRedisServer(utilObj.GetRedisServerkey(designKey, transId), axdesignJson, designKey, schemaName);
        }
        axdesignJObject = getDesignObjectFromJson(axdesignJson);
        if (axdesignJObject == null)
        {
            axdesignJObject = new DesignObject();
            axdesignJObject.newDesign = true;
            isFreshDesign = true;
        }
    }

    private void getDesignData()
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
            nodeAccessToken = HttpContext.Current.Session["nodeAccessToken"].ToString();
        string appsesskey = HttpContext.Current.Session["AppSessionKey"].ToString();
        //Calling getLayout node api  for publish data on run mode

        if (HttpContext.Current.Session["Axp_DesignJson"] != null && HttpContext.Current.Session["Axp_DesignJson"].ToString() != "")
        {
            axdesignJson = HttpContext.Current.Session["Axp_DesignJson"].ToString();
            HttpContext.Current.Session["Axp_DesignJson"] = null;
        }

        //If no data yet saved and published then checking for old design
        //if (axdesignJson == string.Empty && !IsPublish && nodeAccessToken != string.Empty && HttpContext.Current.Session["Axp_OldDesign"] !=null  && HttpContext.Current.Session["Axp_OldDesign"].ToString() ==string.Empty)
        if (axdesignJson == string.Empty && HttpContext.Current.Session["Axp_OldDesign"] != null && HttpContext.Current.Session["Axp_OldDesign"].ToString() == string.Empty)
        {
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDW fdwObj = new FDW();
            //Calling getNewLayout for getting old JSON
            DBContext objdb = new DBContext();
            if (!designMode)
                axdesignJson = objdb.GetAxNewDesign(transId);
            if (axdesignJson == string.Empty)
            {
                //Calling getOldLayout for getting old JSON
                axdesignJson = objdb.GetAxOldDesign(transId); //GetOldLayoutNodeApi(nodeAccessToken, session_id, utl, userName, appsesskey);
                if (axdesignJson != string.Empty)
                {
                    //Parsing old JSON to new JSON 
                    try
                    {
                        axdesignJson = OldToNewDesignJSONParser(axdesignJson, xmlDoc);
                    }
                    catch (Exception ex)
                    {
                        axdesignJson = string.Empty;
                    }
                    if (axdesignJson != string.Empty)
                    {
                        string SavedId = objdb.SaveDesignJson(transId, axdesignJson);
                        if (SavedId != "")
                            objdb.PublishDesignJson(transId, SavedId);
                    }
                    string designKey = Constants.REDISTSTRUCTAXDESIGN;
                    fdwObj.SaveInRedisServer(utilObj.GetRedisServerkey(designKey, transId), axdesignJson, designKey, schemaName);
                    //string designTblKey = Constants.REDISTSTRUCTAXDESIGNTABLE;
                    //fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designTblKey, transId), "", false, schemaName);
                }
                else
                {
                    string axpOldDesignKey = Constants.AXOLDDESIGN;
                    fdwObj.SaveInRedisServer(utilObj.GetRedisServerkey(axpOldDesignKey, transId), "NoOldDesign", axpOldDesignKey, schemaName);
                }
            }
            else
            {
                string designKey = Constants.REDISTSTRUCTAXDESIGN;
                fdwObj.SaveInRedisServer(utilObj.GetRedisServerkey(designKey, transId), axdesignJson, designKey, schemaName);
                //string designTblKey = Constants.REDISTSTRUCTAXDESIGNTABLE;
                //fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designTblKey, transId), "", false, schemaName);
            }
        }
        axdesignJObject = getDesignObjectFromJson(axdesignJson);

        if (axdesignJObject == null)
        {
            axdesignJObject = new DesignObject();
            axdesignJObject.newDesign = true;
            isFreshDesign = true;
        }
    }
    #endregion


    #endregion



}
[Serializable()]
public class FieldsDesign
{
    public string fld_id { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public bool visibility { get; set; }
}
[Serializable()]
public class TableDesign
{
    public string fld_id { get; set; }
    public int width { get; set; }
    public bool visibility { get; set; }
}
[Serializable()]
public class Dc
{
    public string dc_id { get; set; }
    public string isGrid { get; set; }
    public bool gridStretch { get; set; }
    public List<FieldsDesign> fieldsDesign { get; set; }
    public List<TableDesign> tableDesign { get; set; }
}
[Serializable()]
public class DesignObject
{
    public string transid { get; set; }
    public bool compressedMode { get; set; }
    public bool newDesign { get; set; }
    public bool staticRunMode { get; set; }
    public bool wizardDC { get; set; }
    public string selectedLayout { get; set; }
    public string selectedFontSize { get; set; }
    public string selectedControlHeight { get; set; }
    public string tstUpdatedOn { get; set; }
    public string dcLayout { get; set; }
    public string formWidth { get; set; }
    public string formAlignment { get; set; }
    public string fieldCaptionWidth { get; set; }
    public List<FormLabel> formLabel { get; set; }
    public List<ButtonFieldFont> buttonFieldFont { get; set; }
    public List<Dc> dcs { get; set; }
    public List<Dc> newdcs { get; set; }
}
[Serializable()]
public class FormLabel
{
    public string ftype { get; set; }
    public string id { get; set; }
    public int dc { get; set; }
    public string afterField { get; set; }
    public string name { get; set; }
    public string fontFamilly { get; set; }
    public string fontFamillyCode { get; set; }
    public string hyperlinkJson { get; set; }
}
[Serializable()]
public class ButtonFieldFont
{
    public string ftype { get; set; }
    public string id { get; set; }
    public string fontFamilly { get; set; }
    public string fontFamillyCode { get; set; }
    public string hyperlinkJson { get; set; }
}
#endregion


