using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class aspx_ResetPassword : System.Web.UI.Page
{
    public StringBuilder strFldArrays = new StringBuilder();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    LogFile.Log logObj = new LogFile.Log();
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
        ResetSessionTime();
        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = util.toggleTheme();

        if (Session["project"] == null || Session["project"] == string.Empty)
        {
            SessExpires();
        }
        else
        {
            pwdlength.Value = Session["minPwdChars"].ToString();
            string res = GetLockedUsers();
            bool isPwdAlphaNumeric = false;
            if (Session["IsPwdAlphaNumeric"].ToString().ToLower() == "t")
                isPwdAlphaNumeric = true;

            Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script>isPwdAlphaNumeric=" + isPwdAlphaNumeric.ToString() + "</script>");
        }
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }

    private string GetLockedUsers()
    {
        createScriptForUsers();
        return string.Empty;

    }

    private void createScriptForUsers()
    {
        string errorLog = logObj.CreateLog("Calling GetChoices to get the locked users-", Session["nsessionid"].ToString(), "GetChoices", "new");
        string qry = "select username,email from axusers  where logintry >" + Session["minPwdChars"].ToString();
        string txt2 = "<sqlresultset axpapp='" + Session["project"].ToString() + "' transid='' sessionid='" + Session["nsessionid"] + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' >";
        txt2 += qry;
        txt2 += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        ArrayList users = callGetChoices(txt2, "resetPwd");
        strFldArrays.Append("<script type='text/javascript'>");
        StringBuilder strAutoComArray = new StringBuilder();
        strAutoComArray.Append("var arrItems = [");
        for (int i = 0; i < users.Count; i++)
        {
            strFldArrays.Append("arrFlds[\"" + i + "\"]=" + "\"" + users[i].ToString() + "\";");
            strAutoComArray.Append("\"" + users[i].ToString() + "\",");
        }
        strAutoComArray.Append("];");
        strFldArrays.Append(strAutoComArray.ToString() + "availableTags = arrItems;");
    }

    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    private ArrayList callGetChoices(string qry, string calledFrom)
    {

        ArrayList users = new ArrayList();

        string result = string.Empty;
        //Call service        
        result = objWebServiceExt.CallGetChoiceWS("ResetUserPwd", qry);

        string errMsg = string.Empty;
        errMsg = util.ParseXmlErrorNode(result);

        if (errMsg != string.Empty)
        {
            if (errMsg == Constants.SESSIONERROR)
            {
                Session.RemoveAll();
                Session.Abandon();
                util.IFrameSessExpiry();
            }
            else
            {
                Response.Redirect(util.ERRPATH + errMsg);
            }
        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            XmlNodeList WrproductNodes = default(XmlNodeList);
            WrproductNodes = xmlDoc.GetElementsByTagName("row");
            if ((WrproductNodes.Count == 0))
            { }
            else
            {
                for (int i = 0; i < WrproductNodes.Count; i++)
                {

                }
            }
        }

        return users;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        string errorLog = logObj.CreateLog("Calling get choices to get the locked users", Session["nsessionid"].ToString(), "ResetPwd", "new");
        string iXml = "<root axpapp='" + Session["project"].ToString() + "' loginuser='" + Session["user"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' user='" + Session["user"].ToString() + "' action=''>";
        iXml += "<pwd>" + newPwd.Text + "</pwd><oldpwd>" + "" + "</oldpwd><changebyadmin>yes</changebyadmin>";
        iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";

        string result = string.Empty;
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            result = objWebServiceExt.CallChangePassword(iXml);
            xmlDoc.LoadXml(result);
        }
        catch (Exception ex)
        { }

        XmlNode errNode = xmlDoc.SelectSingleNode("error");
        if (errNode != null)
        {
            userName.Text = string.Empty;
            mailId.Text = string.Empty;
            lblError.Text = errNode.InnerText;
        }

        XmlNode succNode = xmlDoc.SelectSingleNode("success");
        if (succNode != null)
        {
            userName.Text = string.Empty;
            mailId.Text = string.Empty;
            Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script>showAlertDialog('success','" + succNode.InnerText + "');</script>");
        }
    }
    protected void Clear_Click(object sender, EventArgs e)
    {
        userName.Text = string.Empty;
        mailId.Text = string.Empty;
    }
}
