using System;
using System.Web;
using System.Web.UI;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    protected void Page_Load(object sender, EventArgs e)
    {
        string loginPath = util.LOGINPATH;
        loginPath = loginPath.Substring(3);
        string axManager = util.AXMANAGERPATH.Substring(3);
        if (!IsPostBack)
        {
            if (Session["Project"] != null)
            {
                if (Session["Project"].ToString() != "" || util.CheckForAvailableProjects().IndexOf(',') > 0)
                    Response.Redirect(loginPath);
                else
                    Response.Redirect(axManager);
            }
            else
                Response.Redirect(loginPath);
        }
    }
}
