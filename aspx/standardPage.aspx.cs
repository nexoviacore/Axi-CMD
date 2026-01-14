using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_standardPage : System.Web.UI.Page
{
    #region Variable Declaration
    Util.Util util;
    public string direction = "ltr";
    public string langType = "en";
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

    }

    [WebMethod]
    public static object GetStandardPages(string name)
    {
        string returnString = string.Empty;

        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        string result = "";
        try
        {
            result = asbExt.ExecuteSQL("", Constants.SQL_GET_SP.Replace(Constants.VAR_NAME, name), "JSON");

            JToken res = JToken.Parse(result);

            if (res["error"] != null ) {
                return new { status = "error", result = res["error"]["msg"].ToString() };
            }

            return new { status = "success", result = result };
        }
        catch (Exception ex)
        {
            return new { status = "failure", result = Constants.CUSTOMERROR };
        }
    }
}