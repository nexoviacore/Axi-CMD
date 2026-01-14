using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Text;
using System.Text.RegularExpressions;

public partial class tstructhtml : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    string succ = string.Empty;
    public string direction = "ltr";
    public string langType = "en";
    public string strcvalue = string.Empty;
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
        util.IsValidSession();
        if (Session["project"] == null || Convert.ToString(Session["project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        if (!IsPostBack)
        {
            try
            {
                if (Request.QueryString["transid"] != null && Request.QueryString["transid"].ToString() != string.Empty)
                {
                    string transId = Request.QueryString["transid"].ToString();
                    LogFile.Log logobj = new LogFile.Log();
                    string errorLog = logobj.CreateLog("Get data from TstructHtml.", HttpContext.Current.Session["nsessionid"].ToString(), "GetdataHtml-" + transId, "new");
                    string sqlInput = "select cvalue from templates where iviewid='" + transId + "'";
                    sqlInput = util.CheckSpecialChars(sqlInput);
                    string iXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlInput + "</sql>";
                    iXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                    string recData = objWebServiceExt.CallGetChoiceWS("", iXML);
                    string errMsg = util.ParseXmlErrorNode(recData);
                    if (errMsg != string.Empty)
                    {
                    }
                    else if (recData != string.Empty)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(recData.ToString());
                        XmlNode node = doc.SelectSingleNode("sqlresultset//response//row//CVALUE | sqlresultset//response//row//cvalue");
                        if (node != null)
                            strcvalue = node.InnerText;
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
    private static void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
    }

    [WebMethod]
    public static string TstructSaveHtml(string transId, string tstCaption, string strInput)//, bool wizardMode)
    {
        string json = string.Empty;
        try
        {
            LogFile.Log logobj = new LogFile.Log();
            Util.Util util = new Util.Util();
            string errorLog = logobj.CreateLog("Save data from TstructHtml.", HttpContext.Current.Session["nsessionid"].ToString(), "SavedataHtml-" + transId, "new");
            string recId = "0";
            string sqlInput = "select templatesid from templates where iviewid='" + transId + "'";
            sqlInput = util.CheckSpecialChars(sqlInput);
            string iXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlInput + "</sql>";
            iXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            string recData = objWebServiceExt.CallGetChoiceWS("", iXML);
            string errMsg = util.ParseXmlErrorNode(recData);
            if (errMsg != string.Empty)
            {
                return json = "{\"message\":\"" + errMsg + "\"}*$*{\"result\":\"error\"}";
            }
            else if (recData != string.Empty)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(recData.ToString());
                XmlNode node = doc.SelectSingleNode("sqlresultset//response//row//TEMPLATESID | sqlresultset//response//row//templatesid");
                if (node != null)
                    recId = node.InnerText == string.Empty ? "0" : node.InnerText;
            }
            StringBuilder sb = new StringBuilder();
            string properties = "<Transaction axpapp=\"" + HttpContext.Current.Session["project"].ToString() + "\" imagefromdb=\"false\" afiles=\"\" trace=\"" + errorLog + "\" sessionid=\"" + HttpContext.Current.Session["nsessionid"].ToString() + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>{0}" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</Transaction>";
            sb.Append("<data transid=\"temps\" recordid=\"" + recId + "\">");
            sb.Append("<axp_recid1 rowno=\"001\">" + recId + "</axp_recid1>");
            sb.Append("<type rowno=\"001\">Tstruct</type>");
            sb.Append("<names rowno=\"001\">" + tstCaption + "(" + transId + ")</names>");
            sb.Append("<iviewid rowno=\"001\">" + transId + "</iviewid>");
            sb.Append("<elements rowno=\"001\">ALL</elements>");
            sb.Append("<cvalue rowno=\"001\">" + util.CheckSpecialChars(strInput) + "</cvalue>");
            sb.Append("<dupchk rowno=\"001\">" + transId + "ALL" + "</dupchk>");
            sb.Append("</data>");
            //ASBExt.WebServiceExt objWebServiceExtNew = new ASBExt.WebServiceExt();
            json = objWebServiceExt.CallSaveDataWS(transId, string.Format(properties, sb.ToString()), "");
            ClearCacheDesignKeys(transId);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Tstruct Save Html -" + ex.Message, sessID, "TstructSaveHtml", "new");
            json = ex.Message;
        }
        return json;
    }

    public static void ClearCacheDesignKeys(string Transid)
    {
        FDW fdwObj = new FDW();
        if (HttpContext.Current.Session["dbuser"] != null)
            fdwObj.Initialize(HttpContext.Current.Session["dbuser"].ToString());
        string fdKey = Constants.REDISTSTRUCT;
        string fdKeyMob = Constants.REDISTSTRUCTMOB;
        //string designKey = Constants.REDISTSTRUCTAXDESIGN;
        string designCustHtmlKey = Constants.REDISTSTRUCTAXCUSTHTML;
        string fdkey1 = Constants.AXPAGETITLE;
        string schemaName = string.Empty;
        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();
        Util.Util utilObj = new Util.Util();
        fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(fdkey1, Transid), "", false, schemaName);
        fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(fdKey, Transid), "", false, schemaName);
        fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(fdKeyMob, Transid), "", false, schemaName);
        //fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designKey, Transid), "", false, schemaName);
        fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designCustHtmlKey, Transid), "", false, schemaName);
    }


    public static string CheckSpecialChars(string str)
    {
        string defaultDateStr = "dd/mm/yyyy";
        //hack: The below line is used to make sure that the & in &amp; is not converted inadvertantly
        //      for other chars this scenario will not come as it does not contains the same char.
        str = Regex.Replace(str, "&amp;", "&");
        str = Regex.Replace(str, "&quot;", "“");
        str = Regex.Replace(str, "\n", "<br>");
        str = Regex.Replace(str, "&", "&amp;");
        str = Regex.Replace(str, "<", "&lt;");
        str = Regex.Replace(str, ">", "&gt;");
        str = Regex.Replace(str, "'", "&apos;");
        str = Regex.Replace(str, "\"", "&quot;");
        str = Regex.Replace(str, "’", "&apos;");
        str = Regex.Replace(str, "“", "&quot;");
        str = Regex.Replace(str, "”", "&quot;");
        str = Regex.Replace(str, "™", "&#8482;");
        str = Regex.Replace(str, "®", "&#174;");

        str = str.Replace((char)160, ' ');

        if (str == defaultDateStr)
            str = "";
        return str;
    }

}
