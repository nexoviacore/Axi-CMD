using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_DeveloperStudio : System.Web.UI.Page
{
    Util.Util util;
    public string direction = "ltr";
    public string langType = "en";
    public string pname = "";
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
        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }
        else
        {
            if (Request.QueryString["ivtstName"] != null && Request.QueryString["ivtstName"].ToString() != "")
            {
                pname = Request.QueryString["ivtstName"].ToString();
            }
        }
    }
    public void SessionExpired()
    {
        Util.Util util = new Util.Util();
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }
}