using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using AjaxControlToolkit;
using System.Configuration;
using Util;

public partial class aspx_ArrangeMenu : System.Web.UI.Page
{

    #region variable

    public string menuData = string.Empty;
    public string direction = "ltr";
    Util.Util util;
    public string langType = "en";
    LogFile.Log logobj = new LogFile.Log();
    FDW fdwObj = new FDW();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    static string axpIconpath = string.Empty;


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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["Project"] == null || Convert.ToString(HttpContext.Current.Session["Project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        else if (!util.CheckValidLogin())
        {
            SessExpires();
            return;
        }
        string Iconpath = hdnIconPath.Value = util.GetAdvConfigs("icon path");
        if (Iconpath == null || Iconpath == string.Empty)
        {
            axpIconpath = HttpContext.Current.Application["ScriptsPath"].ToString() + "images\\user Icons";
            hdnIconPath.Value = HttpContext.Current.Application["scriptsUrlPath"].ToString() + "images/user Icons/";
        }
        else
        {
            axpIconpath = HttpContext.Current.Application["ScriptsPath"].ToString() + Iconpath;
            hdnIconPath.Value = HttpContext.Current.Application["scriptsUrlPath"].ToString() + Iconpath + "/";
        }
        GetMenuData();
        getUserIconList();

    }
    private void GetMenuData()
    {
        string result1 = string.Empty;
        string err = string.Empty;
        string lang_at = "";
        string loginTrace = "false";
        if (Session["language"] != null && Session["language"].ToString().ToUpper() != "ENGLISH")
            lang_at = " lang=\"" + Session["language"].ToString() + "\"";
        string sid = Session["nsessionid"].ToString();
        sid = CheckSpecialChars(sid);
        string errMsg = string.Empty;
        try
        {
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            string sXml = string.Empty;
            string errlog = logobj.CreateLog("Getting Menu", sid, "GetMultiMenu", "new");
            if (loginTrace.ToLower() == "true")
                errlog = logobj.CreateLog("Getting Menu", sid, "GetMultiMenu", "new", "true");
            sXml = sXml + "<root arrangemenu=\"true\" axpapp='" + Session["project"].ToString() + "' sessionid='" + sid + "' trace='" + errlog + "' mname =\"\" " + lang_at + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'";
            sXml = sXml + "> ";
            sXml = sXml + Session["AxApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString();
            sXml = sXml + "</root>";

            result1 = objWebServiceExt.CallGetMultiLevelMenuWS("main", sXml);
            string[] splittedResult = result1.Split(new[] { "*$*" }, StringSplitOptions.None);
            if (splittedResult.Length > 0)
            {
                result1 = splittedResult[0];
                result1 = result1.Split('♠')[1];
            }
            result1 = Regex.Replace(result1, ";fwdslh", "/");
            result1 = Regex.Replace(result1, ";hpn", "-");
            result1 = Regex.Replace(result1, ";bkslh", "\\");
            result1 = Regex.Replace(result1, ";eql", "=");
            result1 = Regex.Replace(result1, ";qmrk", "?");
        }
        catch (Exception ex)
        {
            logobj.CreateLog("GetMenuData --ArrangeMenu " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetMenuData-exception", "new");
        }

        if (errMsg != string.Empty)
        {
            SessExpires();
        }
        else
        {
            menuData = result1;
            menuData = Regex.Replace(menuData, "'", "&apos;");
            menuData = Regex.Replace(menuData, "&quot;", " ");
        }
    }
    private string CheckSpecialChars(string str)
    {
        str = Regex.Replace(str, "&", "&amp;");
        str = Regex.Replace(str, "<", "&lt;");
        str = Regex.Replace(str, ">", "&gt;");
        str = Regex.Replace(str, "'", "&apos;");
        str = Regex.Replace(str, "\"", "&quot;");
        string delimited = @"\\";
        str = Regex.Replace(str, delimited, ";bkslh");
        return str;
    }
    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }
    private void getUserIconList()
    {
        try
        {
            hdnUserIconList.Value = "";
            DirectoryInfo dir = new DirectoryInfo(axpIconpath);
            if (dir.Exists)
            {
                FileInfo[] file = dir.GetFiles();
                foreach (FileInfo file2 in file)
                {
                    if (file2.Extension == ".jpg" || file2.Extension == ".jpeg" || file2.Extension == ".png" || file2.Extension == ".JPG" || file2.Extension == ".PNG")
                    {
                        hdnUserIconList.Value += file2.Name + ",";
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void uploadIcon_Click(object sender, EventArgs e)
    {
        if (uploadIcon.HasFile)
        {
            try
            {
                try
                {

                    DirectoryInfo di = new DirectoryInfo(axpIconpath);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                string iconName = uploadIcon.FileName;
                uploadIcon.PostedFile.SaveAs(axpIconpath + "\\" + iconName);
                hdnUserIconList.Value += iconName + ",";
            }
            catch (Exception ex)
            {

            }

        }
    }

    [WebMethod]
    public static object SaveTreeView(string key, string changedNodes, string actionSequence, string changedVisibility)
    {
        string fdKeyMenuData = Constants.REDISMENUDATA;
        string schemaName = string.Empty;
        FDW fdwObj = new FDW();
        Util.Util util = new Util.Util();
        ASB.WebService asbWeb = new ASB.WebService();
        try
        {
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            string result = asbWeb.saveArrangeMenuData(key, changedNodes, actionSequence, changedVisibility);
            if (result.ToLower() == "done")
            {
                bool IsMenuCache = fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKeyMenuData, "Menu"), key, Constants.REDISMENUDATA, schemaName);
                if (IsMenuCache == false)
                    HttpContext.Current.Session["MenuData"] = key;
                return new { status = "success", msg = "" };
            }
            else
            {
                return new { status = "failure", msg = result };
            }

        }
        catch (Exception ex)
        {
            return new { status = "failure", msg = ex.Message };
        }
    }

    [WebMethod]
    public static string deleteIconImage(string fileName)
    {
        string status = "Failure";
        try
        {
            if (File.Exists(axpIconpath + "\\" + fileName))
            {
                File.Delete(axpIconpath + "\\" + fileName);
                status = "Success";
            }
        }
        catch (Exception ex)
        {
            status = "Failure";
        }
        return status;
    }
}
