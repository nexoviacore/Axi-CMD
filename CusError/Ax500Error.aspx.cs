using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_Ax500Error : System.Web.UI.Page
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

    }
}