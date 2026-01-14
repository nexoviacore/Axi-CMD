using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using CacheMgr;
using System.Configuration;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using System.Web.UI;
/// <summary>
/// Summary description for Class1
/// </summary>
[Serializable]
public sealed class Custom
{
    static readonly Custom instance = new Custom();
    public ArrayList jsFiles = new ArrayList();
    public ArrayList jsGlobalFiles = new ArrayList();
    public ArrayList cssFiles = new ArrayList();
    public ArrayList cssGlobalFiles = new ArrayList();

    public ArrayList jsIviewFiles = new ArrayList();
    public ArrayList jsIviewGlobalFiles = new ArrayList();
    public ArrayList cssIviewFiles = new ArrayList();
    public ArrayList cssIviewGlobalFiles = new ArrayList();

    public ArrayList jsMainFiles = new ArrayList();    
    public ArrayList cssMainFiles = new ArrayList();    

    public ArrayList jsPageFiles = new ArrayList();
    public ArrayList cssPageFiles = new ArrayList();
    public ArrayList jsPageName = new ArrayList();
    public ArrayList cssPageName = new ArrayList();

    public ArrayList jsCustomizations = new ArrayList();

    public string projName = string.Empty;


    public string menuHtmlKey = string.Empty;
    public string menuDataKey = string.Empty;
    public string memtstruct = string.Empty;
    public string memIviewStruct = string.Empty;

    public string memIviewParam = string.Empty;

    enum PageListName
    {
        listIview,
        Responsibilities,
        WorkflowNew,
        widgetBuilder,
        dashBoard,
        PageDesigner,
        Signin
    }
    

    LogFile.Log logobj = new LogFile.Log();
    [NonSerialized]
    ASBCustom.CustomWebservice objCustomWebService;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit    
    static Custom()
    {

    }

    //Add the customizations here 
    Custom()
    {
        //Customization examples     

        //NOTE: for each transid add a transid.js file and transid.css file to the respective project folders under Custom folder,
        //Here the project folder name should be same as the 'proj' attribute value in the web.config file


        //For customizing the events throughout the project, add a js file in the folder and 
        //Call AddCustomLink function with the second parameter as true.
        //This javascript refernce will be added for all tstructs.
        //if the file is in js folder under the custom folder then the filepath will be "js/filename"
        //Eg: AddCustomLink("demo", "js/adjText");
        //Example : 1
        //AddCustomLink("*", "js/custom.js");

        //For customizations for a given tstruct which require css styles, should be added to transid.css file.
        //To add a reference to that css file, call the below function
        //Code : AddCustomLink(transid, filepath);
        //Eg: for adding a css file which contains css classes for the transaction "puOrd" 
        //Code : AddCustomLink("puOrd", filepath);

        //For customizing the css throyghout the project, add the css file in the css folder under the project folder
        //To add a reference to that css file, call the below function
        //Code : AddCustomLink("demo", filepath);
        //The first parameter should be "*", for adding the reference globally in all tstructs.
        //AddCustomLink("demo", "css/demo");      

        //For cusotmizing the iviews through css or js files call the below function
        //AddCustomLinkToIView("iviewname", "js/custom.js"); or AddCustomLinkToIView("*", "js/custom.js");
        //AddCustomLinkToIView("*", "js/customiview.js"); 
        //AddCustomLinkToIView("*", "css/custom.css");

        //AddCustomLinkToMainPage("js/customMenu.js");

        //For adding custom file for any pages use following links 
        //AddCustomLinkToPage contains two argument 1. javasrcipt/css file and the page name where want to add link ,with the help of enum (names as PageListName )pages can be added
        //AddCustomLinkToPage("js/HeadwayTstructIviewLoad.js", PageListName.listIview.ToString() + ".aspx");
        //AddCustomLinkToPage("js/HeadwayTstructIviewLoad.js", PageListName.Responsibilities.ToString() + ".aspx");
        //AddCustomLinkToPage("js/HeadwayTstructIviewLoad.js", PageListName.dashBoard.ToString() + ".aspx");
        //AddCustomLinkToPage("js/HeadwayTstructIviewLoad.js", PageListName.widgetBuilder.ToString() + ".aspx");
        //AddCustomLinkToPage("js/HeadwayTstructIviewLoad.js", PageListName.PageDesigner.ToString() + ".aspx");
        //AddCustomLinkToPage("js/HeadwayTstructIviewLoad.js", PageListName.WorkflowNew.ToString() + ".aspx");
        //AddCustomLinkToPage("css/HeadwayPages.css", PageListName.WorkflowNew.ToString() + ".aspx");
        //AddCustomLinkToPage("css/HeadwayPages.css", PageListName.dashBoard.ToString() + ".aspx");
    }


    /// <summary>
    /// Struct for DMS integration Document Mgmt Sysytem - RAJCOMP
    /// </summary>
    [Serializable]
    public struct DMSStruct
    {
        string dmsUrl;
        string dmsAddorView;
        string dmsDcNo;
        string dmsFieldsDtl;

        public string dmsurl
        {
            get { return dmsUrl; }
            set { dmsUrl = value; }
        }

        public string dmsaddorView
        {
            get { return dmsAddorView; }
            set { dmsAddorView = value; }
        }

        public string dmsdcNo
        {
            get { return dmsDcNo; }
            set { dmsDcNo = value; }
        }

        public string dmsfieldsDtl
        {
            get { return dmsFieldsDtl; }
            set { dmsFieldsDtl = value; }
        }

    }
    /// <summary>
    /// Function to add html to the CustomDiv in the tstruct page
    /// </summary>
    /// <returns></returns>
    public string GetCustomDivHtml()
    {
        return string.Empty;
    }

    public static Custom Instance
    {
        get
        {
            return instance;
        }
    }

    #region Private Methods

    //Function to add the javascript file link to the tstruct page.
    private void AddCustomLink(string transID, string fileName)
    {
        if (fileName.IndexOf(".") != -1)
        {
            string fileExtn = fileName.Substring(fileName.LastIndexOf(".") + 1);
            fileExtn = fileExtn.ToLower();
            if (fileExtn.StartsWith("js"))
            {
                if (transID == "*")
                    jsGlobalFiles.Add(fileName);
                else
                    jsFiles.Add(transID + "¿" + fileName);
            }
            else if (fileExtn.StartsWith("css"))
            {
                if (transID == "*")
                    cssGlobalFiles.Add(fileName);
                else
                    cssFiles.Add(transID + "¿" + fileName);
            }
        }
    }

    private void AddCustomLinkToIView(string ivName, string fileName)
    {
        if (fileName.IndexOf(".") != -1)
        {
            string fileExtn = fileName.Substring(fileName.LastIndexOf(".") + 1);
            fileExtn = fileExtn.ToLower();
            if (fileExtn.StartsWith("js"))
            {
                if (ivName == "*")
                    jsIviewGlobalFiles.Add(fileName);
                else
                    jsIviewFiles.Add(ivName + "¿" + fileName);
            }
            else if (fileExtn.StartsWith("css"))
            {
                if (ivName == "*")
                    cssIviewGlobalFiles.Add(fileName);
                else
                    cssIviewFiles.Add(ivName + "¿" + fileName);
            }
        }
    }

    private void AddCustomLinkToMainPage(string fileName)
    {
        if (fileName.IndexOf(".") != -1)
        {
            string fileExtn = fileName.Substring(fileName.LastIndexOf(".") + 1);
            fileExtn = fileExtn.ToLower();
            if (fileExtn.StartsWith("js"))
                jsMainFiles.Add(fileName);            
            else if (fileExtn.StartsWith("css"))
                cssMainFiles.Add(fileName);            
        }
    }
    private void AddCustomLinkToPage(string fileName,string pageName)
    {
        if (fileName.IndexOf(".") != -1)
        {
            string fileExtn = fileName.Substring(fileName.LastIndexOf(".") + 1);
            fileExtn = fileExtn.ToLower();
            if (fileExtn.StartsWith("js"))
            {
                jsPageName.Add(pageName);
                jsPageFiles.Add(fileName);
            }
            else if (fileExtn.StartsWith("css"))
            {
                cssPageName.Add(pageName);
                cssPageFiles.Add(fileName);
            }
        }
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// function to get the field's ID.
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public string GetFieldID(string fieldName)
    {
        string fieldID = "-1";
        CacheManager cacheMgr = GetCacheObject();
        TStructDef strObj = GetStrObject(cacheMgr);
        if (strObj != null)
        {
            fieldID = strObj.GetFieldID(fieldName);
        }
        return fieldID;
    }

    /// <summary>
    /// Function to get the field's index from the tstruct def object.
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public int GetFieldIndex(string fieldName)
    {
        int fieldIndex = -1;
        CacheManager cacheMgr = GetCacheObject();
        TStructDef strObj = GetStrObject(cacheMgr);
        if (strObj != null)
        {
            fieldIndex = strObj.GetFieldIndex(fieldName);
        }
        return fieldIndex;
    }


    #endregion

    #region GetCacheObject
    private CacheManager GetCacheObject()
    {
        string sid = HttpContext.Current.Session["nsessionid"].ToString();
        string transId = HttpContext.Current.Session["transid"].ToString();
        string fileName = "opentstruct-" + transId;
        string errorLog = logobj.CreateLog("Loading Structure.", sid, fileName, "new");

        CacheManager cacheMgr = null;

        try
        {
            cacheMgr = new CacheManager(errorLog);
        }
        catch (Exception ex)
        {

        }
        return cacheMgr;
    }
    #endregion

    #region GetStrObject
    private TStructDef GetStrObject(CacheManager cacheMgr)
    {
        string proj = HttpContext.Current.Session["project"].ToString();
        string sid = HttpContext.Current.Session["nsessionid"].ToString();
        string user = HttpContext.Current.Session["user"].ToString();
        string transId = HttpContext.Current.Session["transid"].ToString();
        string AxRole = HttpContext.Current.Session["AxRole"].ToString();

        TStructDef strObj = null;
        try
        {
            strObj = cacheMgr.GetStructDef(proj, sid, user, transId, AxRole);
        }
        catch (Exception ex)
        {

        }
        return strObj;
    }

    #endregion

    #region Customised Functions

    //UpdateSqlDepGridDcs

    public string UpdateSqlDepGridDcs(string transid, string fldName, string depDcs)
    {
        string result = depDcs;
        //if (transid == "obill" && fldName == "billtype")
        //    if (result == "")
        //        depDcs = "2";
        //    else
        //        depDcs += ",2";
        //result = depDcs;
        return result;
    }

    public string UpdateSqlGridDependents(string transid, string fldName, string strDepFlds)
    {
        string result = strDepFlds;
        //if (transid == "obill" && fldName == "billtype")
        //{
        //    if (result == "")
        //        strDepFlds = "hidden_payamount";
        //    else
        //        strDepFlds += ",hidden_payamount";
        //    result = strDepFlds;

        //}
        return result;
    }


    //This function can be used to refresh a grid field which has a grid parent field.
    public string UpdateSqlGridParents(string transid, string fldName, string strParentFlds)
    {
        string result = strParentFlds;
        if (strParentFlds == null)
            result = "";
        //if (transid == "prset" && fldName == "sla_product")
        //{
        //    if (result.Trim() == "")
        //        strParentFlds = "ver_product";
        //    else
        //        strParentFlds += ",ver_product";
        //    result = strParentFlds;
        //}
        return result;
    }
    //customization for DMS Integration

    public TStructDef AxAfterGetStruct(string res, string transId, TStructDef tstructObj)
    {
        //checking for custom actions and updating the tstruct definition
        return tstructObj;
    }


    public string AxGetCustomIviewBtns(string iName, string defaultBut)
    {
        return "";
    }
    public ArrayList AxGetCustomTstBtns(TStructDef strObj)
    {
        ArrayList dmsBtnHtml = new ArrayList();
        return dmsBtnHtml;
    }
    public string axBeforeSearch(string transId, string pagesize)
    {
        return pagesize;
    }
    public DataSet axAfterSearch(string transId, DataSet ds, string searchVal, string fldId)
    {
        return ds;
    }

    //To add custom menu item

    public string AddCustomMenuItem()
    {
        return string.Empty;
    }

    public void AxAfterAttSave(string transId, string sId, string recId)
    {

    }
    public string AxBeforeAddTaskTitle(string hint)
    {

        return hint;
    }


    public string AxGetCustomMenu(string url)
    {
        return url;
    }
    /// <summary>
    /// This method is achieve SSO
    /// </summary>
    /// <param name="page"></param>
    /// <param name="type"></param>
    public void AxCustomPageLoad(Page page, Type type)
    {
        
    }

    /// <summary>
    /// <para>Takes Schema Name(s) as input and the returns the Project Name(s) from the axapps.xml. <br />
    /// Returns null if no match found or on exception.  <br />
    /// e.g. CheckForAvailableProjects("schema1","schema2") </para>
    /// </summary>
    /// <param name="schemaNames"></param>
    /// <returns></returns>
    public string GetProjects(params string[] schemaNames)
    {
        return string.Empty;
    }

    /// <summary>
    /// Custom Iview data call before BindDataGrid()
    /// </summary>
    /// <param name="receivedXML">received iview data</param>
    /// <param name="iviewName">iviewName</param>
    /// <author>Abhishek</author>
    public string AxBeforeIViewBindDataGrid(string receivedXML, string iviewName)
    {
        return receivedXML;
    }
    #endregion


}
