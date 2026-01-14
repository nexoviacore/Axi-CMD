using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class aspx_AddEditResponsibility : System.Web.UI.Page
{
    public string proj = string.Empty;
    public string sid = string.Empty;
    public string trace = string.Empty;
    public string user = string.Empty;
    public string language = string.Empty;
    public string acScript = string.Empty;
    public string direction = "ltr";
    public string langType = "en";
    string errorLog = string.Empty;
    string txtFilter = string.Empty;
    ArrayList arrPages = new ArrayList();
    ArrayList arrForms = new ArrayList();
    ArrayList arrReports = new ArrayList();
    LogFile.Log logObj = new LogFile.Log();
    Util.Util util = new Util.Util();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    int totalResrows = 0;
    string currentRes = string.Empty, ixml = string.Empty;
    string actionType = string.Empty, result = string.Empty;
    public bool isCloudApp = false;

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
        ResetSessionTime();
        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }
        GetGlobalVariables();
        if (!Page.IsPostBack)
        {
            var qryStr = Request.QueryString;
            if (qryStr["action"] != null)
                actionType = qryStr["action"].ToString();
            if (util.IsValidQueryString(Request.RawUrl) == false)
                HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.INVALIDURL);

            if (actionType == "add")
            {
                hdnAction.Value = "Add";
                AddResponsibility();
            }
            else if (actionType == "edit")
            {
                hdnAction.Value = "Update";
                if (qryStr["name"] != null)
                {
                    currentRes = qryStr["name"].ToString();
                    EditResponsibility();
                }
            }
            else
            {
                currentRes = qryStr["name"].ToString();
                CopyResponsibility();
            }
        }

        if (ConfigurationManager.AppSettings["isCloudApp"] != null)
        {
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString()); ;
        }
        Page.ClientScript.RegisterStartupScript(GetType(), "set global var in iview", "<script>var isCloudApp = '" + isCloudApp.ToString() + "';</script>");
    }

    private void AddResponsibility()
    {
        ViewState["ResAction"] = "AddRes";
        Session["CurrentRes"] = "default";
    }

    private void EditResponsibility()
    {
        ViewState["ResAction"] = "UpdateRes";
        txtReEditResp.Text = currentRes;
        txtReEditResp.ReadOnly = true;
    }

    private void CopyResponsibility()
    {
        ViewState["ResAction"] = "CopyRes";
    }

    private void SessionExpired()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", true);
        }
    }

    private void GetGlobalVariables()
    {
        proj = Session["project"].ToString();
        ViewState["proj"] = proj;
        user = Session["user"].ToString();
        ViewState["user"] = user;
        sid = Session["nsessionid"].ToString();
        ViewState["sid"] = sid;
        language = Session["language"].ToString();
        ViewState["language"] = language;
        if (Session["language"].ToString() == "ARABIC")
        {
            direction = "rtl";
        }
    }

}
