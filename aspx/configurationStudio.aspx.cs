using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_configurationStudio : System.Web.UI.Page
{
    Util.Util util;
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
        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }
        else
        {
            if (!util.CheckValidLogin())
            {
                SessionExpired();
                return;
            }
            string _AxRole = string.Empty;
            if (Session["AxRole"] != null)
                _AxRole = Session["AxRole"].ToString();
            if (Session["username"] != null && Session["username"].ToString().ToLower() == "admin")
            {
                userActive.Visible = true;
            }
            else if (_AxRole == "admin" || _AxRole.Contains("admin,") || _AxRole.Contains(",admin"))
            {
                userActive.Visible = true;
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