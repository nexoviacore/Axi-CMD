using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AppSettings : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
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
        util.IsValidSession();
        if (Session["project"] == null || Session["axApps"] == null || Convert.ToString(Session["project"]) == string.Empty)
        {
            SessionExpired();
            return;
        }
        if (!IsPostBack)
            util.CheckUserSettings();
        //util.IsValidSession();
        //LoadUserAppSettings();
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string LoadUserAppSettings()
    {
        string result = String.Empty;
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string sql = string.Empty;
        try
        {
            sql = Constants.SQL_LOAD_USER_APPSETTINGS;
            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());
            }
            result = objCWbSer.GetChoicesCustomWS("", sql);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
        //string transID = string.Empty;
        //string resGetCh = "false";
        //string dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        //string sql = string.Empty;
        //try
        //{
        //    transID = HttpContext.Current.Session["transid"].ToString();

        //    sql = Constants.QUERY_GET_PRPS_STATUS;


        //    if (!string.IsNullOrEmpty(sql))
        //    {
        //        sql = sql.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());
        //        sql = sql.Replace("$TYPE$", "axPurpose");
        //        sql = sql.Replace("$TRANID$", transID);

        //    }

        //    string result = objCWbSer.GetChoices(HttpContext.Current.Session["transid"].ToString(), sql);
        //    if (result != string.Empty)
        //    {
        //        DataSet ds = new DataSet();
        //        StringReader sr = new StringReader(result);
        //        ds.ReadXml(sr);
        //        DataTable dt = ds.Tables["row"];
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            if (!string.IsNullOrEmpty(dt.Rows[0]["admn"].ToString()))
        //            {
        //                int adminValue = Convert.ToInt32(dt.Rows[0]["admn"]);
        //                int userValue = Convert.ToInt32(dt.Rows[0]["usr"]);
        //                if (adminValue > 0 && userValue > 0)
        //                    resGetCh = "true";
        //                else if (adminValue <= 0 && userValue > 0)
        //                    resGetCh = "true";
        //                else if (adminValue > 0 && userValue <= 0)
        //                    resGetCh = "false";
        //                else if (adminValue <= 0 && userValue <= 0)
        //                    resGetCh = "false";
        //            }
        //            else
        //                resGetCh = "false";
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    return resGetCh;
        //}
        //return resGetCh;
    }

    //[System.Web.Services.WebMethod(EnableSession = true)]
    [WebMethod]
    public static string SaveUserAppSettings(string ress)
    {
        //string x= Request.Form["data"];

        string result = String.Empty;
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string sql = string.Empty;
        try
        {
            sql = Constants.SQL_SAVE_USER_APPSETTINGS;
            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());
                sql = sql.Replace("$VALUE$", ress);
            }
            result = objCWbSer.GetChoicesCustomWS("", sql);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

}



