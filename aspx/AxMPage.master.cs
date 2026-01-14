using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AxMPage : System.Web.UI.MasterPage
{
    Util.Util util;
    public string direction = "ltr";
    public string langType = "en";
    public string language = string.Empty;
    public string glCulture = string.Empty;
    public string appName;
    public string appTitle;
    public string hybridGUID = "";
    public bool compressedMode = false;
    public string disableSplit = "false";
    public string axApp = string.Empty;
    public string userName = string.Empty;

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        if (Session["language"] != null)
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


    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["Project"] == null || Convert.ToString(HttpContext.Current.Session["Project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        else
        {
            Session["AxInternalRefresh"] = "true";
            language = Session["language"].ToString();
            glCulture = Session["ClientLocale"].ToString();
            axApp = HttpContext.Current.Session["Project"].ToString();
            util = new Util.Util();
            util.GetAxApps(axApp);
            if (Session["projTitle"] != null)
                appName = Session["projTitle"].ToString();
            else if (Session["AxAppTitle"] != null && Session["AxAppTitle"].ToString() != "")
                appName = Session["AxAppTitle"].ToString();
            appTitle = appName;
            hybridGUID = Session["hybridGUID"].ToString();
            if (Session["AxDisableSplit"] != null)
                disableSplit = Session["AxDisableSplit"].ToString();
            if (Session["user"] != null)
                userName = HttpContext.Current.Session["user"].ToString();
        }
    }

    private void SessExpires()
    {
        util = new Util.Util();
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }
}
