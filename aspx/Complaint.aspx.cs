using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_Complaint : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    public string direction = "ltr";
    public string langType = "en";
    
    protected override void InitializeCulture()
    {
        if (Session["language"] != null)
        {
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
        string username = string.Empty;
        util.IsValidSession();
        ResetSessionTime();
        if (Session["project"] == null || Convert.ToString(Session["project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        //else
        //{
        //    if (!util.licencedValidSessionCheck())
        //    {
        //        HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG, false);
        //        return;
        //    }
        //}

        if (ConfigurationManager.AppSettings["CloudHomeAPI"] != null)
            CloudAPI.Value = ConfigurationManager.AppSettings["CloudHomeAPI"].ToString();

        if (Session["username"] != null )
        {
            UserName.Value = Session["username"].ToString();
        }

    }
    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }
    private static void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
    }
}