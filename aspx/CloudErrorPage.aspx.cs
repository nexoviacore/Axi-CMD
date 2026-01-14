using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_CloudErrorPage : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    public string direction = "ltr";
    public string langType = "en";
    protected override void InitializeCulture()
    {
        if (Session["language"] != null || Application["LangSess"] != null)
        {
            string langProj = Session["language"] == null ? Application["LangSess"].ToString() : Session["language"].ToString();
            string dirLang = string.Empty;
            dirLang = util.SetCulture(langProj.ToUpper());
            if (!string.IsNullOrEmpty(dirLang))
            {
                direction = dirLang.Split('-')[0];
                langType = dirLang.Split('-')[1];
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["errmsg"] != null)
        {
            //  errno.InnerHtml = Request.QueryString["errmsg"].ToString();
            string error = Request.QueryString["errmsg"].ToString();
            if (error.ToLower() == "error")
                lblError.Visible = true;
            else
                lblSessionexp.Visible = true;
        }
    }
}
