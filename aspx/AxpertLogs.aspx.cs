using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;

public partial class aspx_Logs : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    public string direction = "ltr";
    public string langType = "en";

    protected override void InitializeCulture()
    {
        if (Request.Form["language"] != null || Session["language"] != null)
        {
            string langProj = Request.Form["language"] == null ? Session["language"].ToString() : Request.Form["language"];
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

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetConfigLogsHtml()
    {
        StringBuilder strLogs = new StringBuilder();

        try
        {
            string sqlSelectLog = Constants.SQL_GET_AXPWEBLOGS.Replace("$Type$", "Config Changes");
            DBContext obj = new DBContext();
            DataTable dtConfigLog = new DataTable();
            try
            {
                dtConfigLog = obj.GetDataTableInline(sqlSelectLog);
            }
            catch { }

            strLogs.Append("<table id='tblConfigLog' class='gridData'><thead><tr><th></th>");

            //Getting Table header
            strLogs.Append("<th>Modified by</th>");
            strLogs.Append("<th>Modified on</th>");
            strLogs.Append("<th>Other Info</th>");
            strLogs.Append("<th>Changes</th>");
            strLogs.Append("</tr></thead><tbody>");

            //Getting Table row-wise data
            Dictionary<string, string> rowData = new Dictionary<string, string>();
            foreach (DataRow dr in dtConfigLog.Rows)
            {
                rowData.Clear();
                strLogs.Append("<tr><td class='details-control'><span style='vertical-align:middle' title='Show Details' class='icon-arrows-down'></span></td>");

                for (int i = 0; i < dtConfigLog.Columns.Count; i++)
                {
                    rowData.Add(dtConfigLog.Columns[i].ColumnName.ToUpper(), Convert.ToString(dr.ItemArray[i]));
                }

                strLogs.Append("<td>" + (rowData.ContainsKey("USERNAME") ? rowData["USERNAME"] : "") + "</td>");
                strLogs.Append("<td>" + (rowData.ContainsKey("LOGTIME") ? rowData["LOGTIME"] : "") + "</td>");
                strLogs.Append("<td>" + (rowData.ContainsKey("SESSIONID") ? "<b>Session ID : </b>" + rowData["SESSIONID"] : "") + (rowData.ContainsKey("IPADDRESS") ? "<br><b>IP : </b>" + rowData["IPADDRESS"] : "") + "</td>");
                strLogs.Append("<td>" + (rowData.ContainsKey("LOGDETAILS") ? GetLogDetails(rowData["LOGDETAILS"]) : "") + "</td>");
                strLogs.Append("</tr>");
            }
            strLogs.Append("</tbody></table>");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return strLogs.ToString();
    }

    private static string GetLogDetails(string strDetails)
    {
        try
        {
            JArray configNode = null;
            JObject objConfig = JObject.Parse(strDetails);
            configNode = (JArray)objConfig["changes"];

            StringBuilder strChanges = new StringBuilder();

            if (configNode != null)
            {
                foreach (JObject content in configNode.Children<JObject>())
                {
                    string propName = content["key"].ToString().ToUpper();
                    if ((propName == "AXIMPEXPTEMPTPATH" || propName == "AXIMAGEPATH" || propName == "AXATTACHFILEPATH" || propName == "AXGRIDATTACHPATH" || propName == "AXPRINTEXEPATH" || propName == "AXHTMLPATH"))
                    {
                        strChanges.Append("♦" + GetConfigFldLabels(content["key"].ToString()) + "♣" + content["oldVal"].ToString().Replace("||", "\\") + "♣" + content["newVal"].ToString().Replace("||", "\\"));
                    }
                    else
                        strChanges.Append("♦" + GetConfigFldLabels(content["key"].ToString()) + "♣" + content["oldVal"].ToString() + "♣" + content["newVal"].ToString());
                }

            }
            return strChanges.ToString().Trim('♦');
        }
        catch { }
        return string.Empty;
    }

    public  static string GetConfigFldLabels(string key)
    {
        string lang = "";
        if ((key.StartsWith("AxAppTitle") || key.StartsWith("AxCopyRightText") || key.StartsWith("AxPrintTitle")) && key.Split('_').Length == 2)
        {
            lang = key.Split('_')[1];
            key = key.Split('_')[0];
        }

        switch (key)
        {
            case "AxEnableCards":
                return "Enable Cards";
            case "AxCPWDOnLogin":
                return "Allow Change Password on first time login";
            case "AxSessionExtend":
                return "Session Auto Extend";
            case "AxAlertTimeout":
                return "Alerts Timeout in seconds";
            case "AxWizardType":
                return "Wizard Type";
            case "AxLanguages":
                return "Application Languages";
            case "AxDevInstance":
                return "Development Instance";
            case "AutoSavePublish":
                return "Save and Auto Publish";
            case "AxDisplayAutoGenVal":
                return "Show Auto generated Field Value";
            case "AxIsPerfCode":
                return "Enable AutoComplete for Select and Picklist fields";
            case "AxInlineGridEdit":
                return "Grid Data Edit";
            case "AxAttachmentSize":
                return "Attachments Size";
            case "AxImagePath":
                return "Image field storage path";
            case "AxAttachFilePath":
                return "Form Attachment storage path";
            case "AxGridAttachPath":
                return "Grid Attachment storage path";
            case "AxDrafts":
                return "Save As Draft";
            case "AxAutoPurge":
                return "Auto Purge Drafts";
            case "AxMaxDraftsCount":
                return "Max Drafts Count";
            case "AxDesignerAccess":
                return "Enable Design Mode";
            case "AxPrintExePath":
                return "Print Server Path(wktohtmlpdf)";
            case "AxHtmlPath":
                return "Template path(HTML)";
            case "AxIviewDataWSRows":
                return "Iview Data Rows Cache Size";
            case "AxIviewcelltextwrap":
                return "Column word wrap";
            case "AxDefaultPageSize":
                return "Rows per page";
            case "AxShowAppTitle":
                return "Show App Title in downloads";
            case "AxHomeBuildAccess":
                return "Home builder access";
            case "AxMaxNumOfWidgets":
                return "Maximum number of widgets in home builder";
            case "AxAppTitle":
                return "App Title (" + lang + ")";
            case "AxCopyRightText":
                return "Copyright Text (" + lang + ")";
            case "AxPrintTitle":
                return "Print Title (" + lang + ")";
            case "AxMenuStyle":
                return "Menu Style";
            case "AxMenuColumns":
                return "Columns";
            case "AxSubmenuPerView":
                return "Submenu per view";
            case "AxMenuWordWrap":
                return "Word Wrap";
            case "AxSubMenuCount":
                return "SubMenu count";
            case "AxDirectSubMenuCount":
                return "Direct SubMenu count";
        }
        return key;
    }
}