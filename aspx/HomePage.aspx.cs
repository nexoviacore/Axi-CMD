using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_HomePage : System.Web.UI.Page
{
    public string homeBuilder = string.Empty;
    public string globalVars = string.Empty;
    public string homeBuilderImagePath = string.Empty;
    public string appsessionKey = string.Empty;
    public string dbType = string.Empty;
    public string myTaskString = string.Empty;
    public bool isCloudApp = false;
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
        Util.Util util = new Util.Util();
        util.IsValidSession();
        ResetSessionTime();
        if (Session["project"] == null || Convert.ToString(Session["project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        //else
        //{
        //    if (!util.licencedValidSessionCheck())
        //    {
        //        HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG, false);
        //        return;
        //    }
        //}

        if (Session["AppSessionKey"] != null)
        {
            if (Session["lictype"] != null && Session["lictype"].ToString() == "limited")
                appsessionKey = "l~";
            else
                appsessionKey = "u~";
            appsessionKey += Session["AppSessionKey"].ToString();
        }

        dbType = Session["axdb"].ToString();

        globalVars = util.GetGlobalVarString();

        if (Session["AxHomeBuildAccess"] != null)
            homeBuilder = Session["AxHomeBuildAccess"].ToString();
        else
            homeBuilder = "default";
        if (ConfigurationManager.AppSettings["HomeBuilderImagePath"] != null)
            homeBuilderImagePath = ConfigurationManager.AppSettings["HomeBuilderImagePath"].ToString();
        //util.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri);

        if (Session["backForwBtnPressed"] != null && !Convert.ToBoolean(Session["backForwBtnPressed"]))
        {
            util.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri);
        }
        Session["backForwBtnPressed"] = false;
        if (ConfigurationManager.AppSettings["isCloudApp"] != null)
        {
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString()); ;
        }
        if (((dbType.ToLower() == "oracle") | (string.IsNullOrEmpty(dbType.ToLower()))))
        {
            myTaskString = "select fromwhom as reported_by,TO_CHAR(message) MESSAGE,  to_char(to_date(upddatetime, 'DD/MM/YYYY HH24:MI:SS'), 'DD/MM/YYYY') created_on, sname, recordid,to_char(dueby, 'DD/MM/YYYY') due_date from axtasks where towhom ='" + Session["user"] + "' and status=1 order by to_date(upddatetime, 'DD/MM/YYYY HH24:MI:SS') desc";
        }
        else if ((dbType.ToLower() == "mysql") | (dbType.ToLower() == "mariadb"))
        {
            myTaskString = "SELECT fromwhom,convert(message, char(1000)) MESSAGE,convert(date_format(upddatetime, '%d/%m/%Y %H:%i:%s'), char(50)) created_on,sname,recordid,convert(date_format(dueby, '%d/%m/%Y %H:%i:%s'), char(50)) due_date FROM axtasks WHERE towhom = '" + Session["user"] + "' AND status = 1 ORDER BY date_format(upddatetime, '%d/%m/%Y %H:%i:%s') DESC";
        }
        else if ((dbType.ToLower() == "ms sql"))
        {
            myTaskString = "select fromwhom, convert(nvarchar(70),(message)) MESSAGE, convert(varchar(10), upddatetime, 103) created_on, sname, recordid, convert(varchar(10), dueby, 103) due_date from axtasks where towhom = '" + Session["user"] + "' AND status = 1 ORDER BY convert(datetime, upddatetime) DESC";
        }
        else if ((dbType.ToLower() == "postgresql" || dbType.ToLower() == "postgre"))
        {
            myTaskString = "select fromwhom as reported_by,cast(message as text) MESSAGE,  to_char(to_date(upddatetime, 'DD/MM/YYYY HH24:MI:SS'), 'DD/MM/YYYY') created_on, sname, recordid,to_char(dueby, 'DD/MM/YYYY') due_date from axtasks where towhom = '" + Session["user"] + "' and status = 1 order by to_date(upddatetime, 'DD/MM/YYYY HH24:MI:SS') desc";
        }
        if (ConfigurationManager.AppSettings["EncryptionKey"] != null && ConfigurationManager.AppSettings["EncryptionIV"] != null && ConfigurationManager.AppSettings["EncryptionKey"] != string.Empty && ConfigurationManager.AppSettings["EncryptionIV"] != string.Empty)
        {
            string[] keyStr = ConfigurationManager.AppSettings["EncryptionKey"].ToString().Split(',');
            byte[] keyBytes = keyStr.Select(Byte.Parse).ToArray();
            string[] ivStr = ConfigurationManager.AppSettings["EncryptionIV"].ToString().Split(',');
            byte[] ivBytes = ivStr.Select(Byte.Parse).ToArray();
            byte[] encryptedUtl = util.EncryptStringToBytes_Aes(myTaskString + "^~)", keyBytes, ivBytes);
            myTaskString = BitConverter.ToString(encryptedUtl).Replace("-", string.Empty);
        }
        Page.ClientScript.RegisterStartupScript(GetType(), "set global var in homeBuilder", "<script>var isCloudApp = '" + isCloudApp.ToString() + "';var myTaskString= '" + myTaskString.ToString() + "';</script>");

    }
    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }
    private static void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
    }


}
