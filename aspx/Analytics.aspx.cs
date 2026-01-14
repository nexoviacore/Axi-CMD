using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class aspx_Analytics : System.Web.UI.Page
{
    #region Variable Declaration
    Util.Util util;
    public string proj = string.Empty;
    public string sid = string.Empty;
    public string language = string.Empty;
    public string trace = string.Empty;
    public string user = string.Empty;
    public string entityName = string.Empty;
    public string selectedEntities = string.Empty;
    public string entityMetaDataJson = string.Empty;
    public string allEntitiesList = string.Empty;
    public string selectedEntitiesList = string.Empty;
    public string selectedAnalyticsFieldsList = string.Empty;
    public string langType = "en";
    public string direction = "ltr";
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
        util = new Util.Util();
        util.IsValidSession();

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
            Analytics analytics = new Analytics();
            hdnAnalyticsPageLoadData.Value = analytics.GetAnalyticsEntityData("Analytics");
        }
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

    [WebMethod]
    public static string GetEntityListWS()
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        Analytics analytics = new Analytics();
        return analytics.GetEntityList();
    }

    [WebMethod]
    public static string SetAnalyticsDataWS(string page, string transId, Dictionary<string, string> properties, bool allUsers)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        Analytics analytics = new Analytics();
        var result = analytics.SetAnalyticsData(page, transId, properties, allUsers);
        return result;
    }

    [WebMethod]
    public static string GetAnalyticsDataWS(string page, string transId, List<string> propertiesList)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        Analytics analytics = new Analytics();
        var result = analytics.GetAnalyticsData(page, transId, propertiesList);
        return result;
    }

    [WebMethod]
    public static string GetAnalyticsEntityWS(string page, string transId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        Analytics analytics = new Analytics();
        var result = analytics.GetAnalyticsEntityData(page, transId);
        return result;
    }

    [WebMethod]
    public static string GetAnalyticsChartsDataWS(string page, string transId, string aggField, string aggTransId, string groupField, string groupTransId, string aggFunc)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        Analytics analytics = new Analytics();
        var result = analytics.GetAnalyticsChartsData(page, transId, aggField, aggTransId, groupField, groupTransId, aggFunc);
        return result;
    }

    [WebMethod]
    public static string GetAnalyticsMultipleChartsDataWS(string page, string transId, List<object> charts)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        Analytics analytics = new Analytics();
        var result = analytics.GetAnalyticsMultipleChartsData(page, transId, charts);
        return result;
    }

    [WebMethod]
    public static string GetEntityDropDownDataWS(string transId, string fldId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        Analytics analytics = new Analytics();
        var result = analytics.GetEntityDropDownData(transId, fldId);
        return result;
    }
}
