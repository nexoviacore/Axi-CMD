using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class aspx_ToolbarManager : System.Web.UI.Page
{
    #region variables
    Util.Util util;
    LogFile.Log logobj = new LogFile.Log();
    public string direction = "ltr";
    public string langType = "en";
    static string axpIconpath = string.Empty;
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    ASB.WebService asbWebService = new ASB.WebService();
    public string toolbarData = string.Empty;
    public string structType = string.Empty;
    public string structName = string.Empty;
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
        if (Session["project"] == null || Session["axApps"] == null || Convert.ToString(Session["project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        getToolbar();
        string Iconpath = hdnIconPath.Value = util.GetAdvConfigs("icon path");
        if (Iconpath == null || Iconpath == string.Empty)
        {
            axpIconpath = HttpContext.Current.Application["ScriptsPath"].ToString() + "images\\user Icons";
            hdnIconPath.Value = HttpContext.Current.Application["scriptsUrlPath"].ToString() + "images/user Icons/";
        }
        else
        {
            axpIconpath = HttpContext.Current.Application["ScriptsPath"].ToString() + Iconpath;
            hdnIconPath.Value = HttpContext.Current.Application["scriptsUrlPath"].ToString() + Iconpath + "\\\\";
        }
        getUserIconList();

    }
    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }
    private void getToolbar()
    {
        structType = Request.QueryString["stype"];
        structName = Request.QueryString["name"];
        string result = string.Empty;
        result = asbWebService.callGetToolbarWS(structType, structName);
        if(result != string.Empty)
        {
        string errMsg = util.ParseJSonErrorNode(result);
          if (errMsg != string.Empty)
          {
            SessExpires();
          }
         else
         {
            toolbarData = result;
         }
       }
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
        catch(Exception ex)
        {

        }

    }

    protected void uploadIcon_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        if (uploadIcon.HasFile)
        {
            try
            {
                try
                {

                    DirectoryInfo di = new DirectoryInfo(axpIconpath);
                    //' Determine whether the directory exists.
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

    [WebMethod]
    public static object callSaveToolbarWS(string structType, string structName, string toolbarJSON)
    {
        // 
        FDW fdwObj = new FDW();
        Util.Util util = new Util.Util();
        ASB.WebService asbWeb = new ASB.WebService();
        try
        {
            string result = asbWeb.callSaveToolbarWS(structType, structName, toolbarJSON);
            string errMsg = util.ParseXmlErrorNode(result);
            if (errMsg != string.Empty)
            {
                if (errMsg == Constants.SESSIONERROR || errMsg == Constants.SESSIONEXPMSG || errMsg == Constants.SESSIONEXPMSG || errMsg == Constants.ERAUTHENTICATION)
                {
                    return util.SESSTIMEOUT;
                }
                else
                {
                    return new { status = "failure", msg = result };
                }
            }
            return new { status = "success", msg = result };

        }
        catch (Exception ex)
        {
            return new { status = "failure", msg = ex.Message };
        }
    }

}