using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class cancel : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    public string transDetails = "";
    public string direction = "ltr";
    public string langType = "en";
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
        if (Session["language"].ToString() == "ARABIC")
        {
            direction = "rtl";
        }
        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = util.toggleTheme();
        if (Request.UrlReferrer != null)
        {
            if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("tstruct.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("cancel.aspx")))
            {
                Response.Redirect("../cusError/axcustomerror.aspx");
            }
        }
        string transId = Request.QueryString["transid"];
        string rid = Request.QueryString["rid"];
        transDetails = "<script type='text/javascript'>";
        transDetails += "var recordid = '" + rid + "';var gl_language = '" + Session["language"].ToString() + "';";
        transDetails += "function GetFormDetails() { var a = '" + Session["project"] + "';var b='" + Session["user"] + "';var c='" + transId + "';var d='" + Session["nsessionid"] + "';var e = '" + Session["AxRole"] + "';var f='" + Session["AxTrace"] + "';SetTstProps(a,b,c,d,e,f);}";
        transDetails += "</script>";
    }
}
