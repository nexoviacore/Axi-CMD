using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dwb : System.Web.UI.Page
{
    public string SQLHintObj = string.Empty;
    ASBExt.WebServiceExt asbEx = new ASBExt.WebServiceExt();
    DBquery DBQobj;
    Util.Util utils = new Util.Util();
    public string nodeAccessToken = string.Empty;
    public string userResps = string.Empty;
    public string restdllPath = string.Empty;
    public string nodeAPI = string.Empty;
    public string userroles = string.Empty;
    public string redisutl = string.Empty;
    public string sid = string.Empty;
    public string utl = string.Empty;//Session["utl"]
    public string hasPageBuildAccess = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["Project"] == null || Convert.ToString(HttpContext.Current.Session["Project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        getSQLHintObj();
        if(HttpContext.Current.Session["nodeAccessToken"] != null && HttpContext.Current.Session["nodeAccessToken"].ToString() != "")
            nodeAccessToken = HttpContext.Current.Session["nodeAccessToken"].ToString();
        if (HttpContext.Current.Session["AxResponsibilities"] != null && HttpContext.Current.Session["AxResponsibilities"].ToString() != "")
            userResps = HttpContext.Current.Session["AxResponsibilities"].ToString();
        if (ConfigurationManager.AppSettings["RestDllPath"] != null && ConfigurationManager.AppSettings["RestDllPath"].ToString() != "")
            restdllPath = ConfigurationManager.AppSettings["RestDllPath"].ToString();
        if (ConfigurationManager.AppSettings["NodeAPI"] != null && ConfigurationManager.AppSettings["NodeAPI"].ToString() != "")
            nodeAPI = ConfigurationManager.AppSettings["NodeAPI"].ToString();
        if (HttpContext.Current.Session["AxRole"] != null && HttpContext.Current.Session["AxRole"].ToString() != "")
            userroles = HttpContext.Current.Session["AxRole"].ToString();
        if (HttpContext.Current.Session["RedisCacheIP"] != null && HttpContext.Current.Session["RedisCacheIP"].ToString() != "")
            redisutl = HttpContext.Current.Session["RedisCacheIP"].ToString();
        if (HttpContext.Current.Session["nsessionid"] != null && HttpContext.Current.Session["nsessionid"].ToString() != "")
            sid = HttpContext.Current.Session["nsessionid"].ToString();
        if (HttpContext.Current.Session["utl"] != null && HttpContext.Current.Session["utl"].ToString() != "")
            utl = HttpContext.Current.Session["utl"].ToString();//Session["utl"]
        if (HttpContext.Current.Session["hasPageBuildAccess"] != null && HttpContext.Current.Session["hasPageBuildAccess"].ToString() != "")
            hasPageBuildAccess = HttpContext.Current.Session["hasPageBuildAccess"].ToString();
    }
    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }
    private void getSQLHintObj()
    {
        DBQobj = new DBquery();
        string query = string.Empty;
        string result = string.Empty;
        try
        {
            query = DBQobj.getSQLqueryString("", "getTblClmnHint");
            result = asbEx.ExecuteSQL("", query, "JSON");
            string errrMsg = utils.ParseJSonErrorNode(result);
            if(errrMsg == string.Empty)
            {
                SQLHintObj = result;
            }
             

        }
        catch (Exception ex)
        {

        }
    }

}
