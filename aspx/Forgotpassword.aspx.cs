using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.IO;

using System.Configuration;

using BotDetect;
using BotDetect.Web;
using Newtonsoft.Json.Linq;

public partial class aspx_Forgotpassword : System.Web.UI.Page
{
    public string appName;
    public string appTitle;
    public string errMsg;
    public string successMsg;
    public string proj = string.Empty;
    public string EnableOldTheme;
    string loginTrace = "false";
    string usrMob = string.Empty;
    Util.Util util;
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    Util.Util utilObj = new Util.Util();
    LogFile.Log logobj = new LogFile.Log();
    public string direction = "ltr";
    public string strFileinfo = string.Empty;
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
            FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/Js/lang/content-" + langType + ".js"));
            if (!(filcustom.Exists))
            {
                direction = "ltr";
                langType = "en";
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        main_body.Attributes.Add("class", direction + " signout_cls");
        util = new Util.Util();
        if (Request.UrlReferrer != null)
        {
            if (Request.UrlReferrer.AbsolutePath.ToLower().Contains("signin.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("forgotpassword.aspx"))
            {

            }
            else
            {
                Response.Redirect("../CusError/Axcustomerror.aspx");
            }
        }


        if (ConfigurationManager.AppSettings["LoginTrace"] != null)
        {
            loginTrace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
        }

        if ((!string.IsNullOrEmpty(errMsg)))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showAlertDialog(\"error\",\"" + errMsg + "\");", true);
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallHideAlertDialog", "hideAlertDialog(\"\");", true);
        }

        //commeting this as per rajendra's requirement
        if (Request.QueryString["proj"] != null)
        {
            if (Request.QueryString["proj"] != "undefined" && Request.QueryString["proj"] != "Select Project")
            {
                proj = Convert.ToString(Request.QueryString["proj"]);

                selectProj1.Style.Add("display", "none");
                projlbl.Style.Add("display", "none");
                //if (!utilObj.IsAlphaNum(proj))
                //    Response.Redirect(Constants.PARAMERR);
            }
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["proj"] != null)
            {
                if (Request.QueryString["proj"] != "undefined" && Request.QueryString["proj"].ToString() != "" && Request.QueryString["proj"].ToString().ToLower() != "select project")
                {
                    proj = Convert.ToString(Request.QueryString["proj"]);
                    hdnProj.Value = proj;
                }
            }

            if (ConfigurationManager.AppSettings["proj"] != null && ConfigurationManager.AppSettings["proj"] != String.Empty)
            {
                proj = ConfigurationManager.AppSettings["proj"].ToString();
                hdnAxProjs.Value = proj;
                selectProj1.Style.Add("display", "none");
                projlbl.Style.Add("display", "none");
            }
            else
                hdnAxProjs.Value = utilObj.CheckForAvailableProjects();
            EnableCaptcha(proj);
        }

        if (Session["projTitle"] != null)
        {
            appTitle = Session["projTitle"].ToString();
        }
        else
        {
            if (Session["AxAppTitle"] != null)
                appTitle = Session["AxAppTitle"].ToString();
            else
                appTitle = GetCurrAppTitle(proj);
        }
        appName = appTitle;

        if ((!string.IsNullOrEmpty(successMsg)))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showAlertDialog(\"success\",\"" + successMsg + "\");", true);
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallHideAlertDialog", "hideAlertDialog(\"\");", true);
        }
        Random random = new Random();
        int i = random.Next(1, 9999);
        ViewState["RandNo"] = i.ToString();
        string folderPath = Server.MapPath("~/images/Custom");
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderPath);
        var diFileinfo = di.GetFiles();
        bool Ismobile = Request.Browser.IsMobileDevice;
        string custommoblogoexist = "false";
        string customlogoexist = "false";

        //    foreach (var file in diFileinfo)
        //    {
        //        if (file.Length > 0 && file.Name.Contains("homelogo_mob"))
        //        {
        //            strFileinfo = file.Name;
        //return;
        //        }
        //    }
        //if (Ismobile)
        //{
        //    foreach (var file in diFileinfo)
        //    {
        //        if (file.Length > 0 && file.Name.Contains("homelogo_mob"))
        //        {
        //            main_body.Attributes.CssStyle.Add("background", "url(./../images/Custom/" + file.Name + "?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ") no-repeat center center fixed !important");
        //            main_body.Attributes.CssStyle.Add("background-size", "cover !important");
        //            main_body.Attributes.CssStyle.Add("height", "100vh !important");
        //            custommoblogoexist = "True";
        //            return;
        //        }
        //    }
        //    if (custommoblogoexist == "false" && Ismobile)
        //    {
        //        //main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/homelogo_mob.jpg) no-repeat");
        //        main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/login-img.png) no-repeat");
        //        main_body.Attributes.CssStyle.Add("background-size", "cover !important");
        //        main_body.Attributes.CssStyle.Add("height", "100vh !important");
        //    }
        //}
        //else
        //{
        //foreach (var file in diFileinfo)
        //{
        //    if (file.Length > 0 && file.Name.Contains("homelogo"))
        //    {
        //        if (file.Name.Contains("mp4"))
        //        {
        //            main_body.Attributes.CssStyle.Add("background", "");
        //            bgvid.Attributes.CssStyle.Add("display", "block");
        //            bgvidsource.Attributes.Add("src", "./../images/Custom/homelogo.mp4?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + "");
        //            customlogoexist = "True";
        //            return;
        //        }
        //        else
        //        {
        //            main_body.Attributes.CssStyle.Add("background", "url(./../images/Custom/" + file.Name + "?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ") no-repeat center center fixed !important");
        //            main_body.Attributes.CssStyle.Add("background-size", "cover !important");
        //            main_body.Attributes.CssStyle.Add("height", "100vh !important");
        //            customlogoexist = "True";
        //            return;
        //        }
        //    }
        //}
        if (customlogoexist == "false")
        {
            //main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/homelogo.jpg) center center no-repeat fixed !important");
            //main_body.Attributes.CssStyle.Add("background-size", "cover !important");
            main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/login-img.png)");
            main_body.Attributes.CssStyle.Add("background-repeat", "no-repeat");
            main_body.Attributes.CssStyle.Add("background-attachment", "fixed");
            main_body.Attributes.CssStyle.Add("background-position", "bottom");
            main_body.Attributes.CssStyle.Add("background-size", "cover !important");
        }
        //}
    }


    protected void btnReset_Click(object sender, System.EventArgs e)
    {
        if (hdnUsrName.Value.ToLower() == "admin")
        {
            errMsg = "Cannot change admin password.";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showAlertDialog(\"error\",4034,\"client\");", true);
            return;
        }

        //if (IsPostBack && ConfigurationManager.AppSettings["enableCaptcha"] != null && ConfigurationManager.AppSettings["enableCaptcha"].ToString() == "true")
        if (IsPostBack && Session["captchaenabledFlagForgot"] != null && Session["captchaenabledFlagForgot"].ToString() == "true")
        {
            bool allowChange = DynamicCaptcha.Validate();

            if (!allowChange)
            {
                errMsg = "Wrong Captcha, Please Try Again.";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showAlertDialog(\"error\", \"" + errMsg + "\");", true);
                return;
            }
        }

        // string pwd = Convert.ToString(ViewState["RandNo"]);
        Random r = new Random();
        int randNum = r.Next(1000000);
        string sixDigitPwd = randNum.ToString("D6");
        string otpPassword = sixDigitPwd;
        string aesPwd = string.Empty;
        aesPwd = objWebServiceExt.GetEncryptedValue(sixDigitPwd);
        MD5 md5Hash = MD5.Create();
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sixDigitPwd));
        string msgTosend = string.Empty;

        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
            sBuilder.Append(data[i].ToString("x2"));
        sixDigitPwd = sBuilder.ToString();

        if (hdnActive.Value.ToLower() == "f")
        {
            string activeErrorMsg = "Mail cannot be sent as user is not active anymore";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showAlertDialog(\"error\", \"" + activeErrorMsg + "\");", true);
            return;
        }

        if (hdnNickname.Value != "" || hdnNickname.Value != String.Empty)
            msgTosend = hdnNickname.Value;
        else
            msgTosend = hdnUsrName.Value;



        string result = string.Empty;
        string inputQuery = string.Empty;
        string sid = Session.SessionID;
        hdnAxProjs.Value = utilObj.CheckForAvailableProjects();

        if (proj == string.Empty)
        {
            proj = hdnProj.Value;
        }

        util.GetAxApps(proj);

        string axApps = Convert.ToString(HttpContext.Current.Session["axApps"]);
        string axProps = Convert.ToString(Application["axProps"]);

        string errlog = string.Empty;
        if (loginTrace.ToLower() == "true")
            errlog = logobj.CreateLog("Call to Change Password Web Service", sid, "Forgot Password", "", "true");
        else
            errlog = logobj.CreateLog("Call to Change Password Web Service", sid, "Forgot Password", "");

        StringBuilder strContent = new StringBuilder();

        strContent.Append("<usermailid>" + hdnMl.Value + "</usermailid><subject>Reset Password</subject><body>");
        strContent.Append("<l0>Hi " + msgTosend + ",</l0>");
        strContent.Append("<l01>" + proj + "</l01>");
        strContent.Append("<l1>Your password has been changed " + otpPassword + " as per your request</l1>");
        strContent.Append("<l2>Please login immediately and change the password of your choice for security</l2>");
        strContent.Append("<l3>Regards</l3><l4>Support Team.</l4></body>");
        strContent.Append("<pwd>" + aesPwd + "</pwd>");
        strContent.Append("<md5pwd>" + sixDigitPwd + "</md5pwd><changebyadmin>yes</changebyadmin>");

        string inputXML = "<root trace='" + errlog + "' direct='t' axpapp='" + proj + "' username='" + hdnUsrName.Value + "' service='f' user='" + hdnUsrName.Value + "'>" + strContent.ToString() +
            axApps + axProps + "</root>";

        try
        {
            result = objWebServiceExt.CallChangePassword(inputXML);
            errlog = logobj.CreateLog("Call to Change Password Web Service" + result + "inputxml" + inputXML, sid, "Forgot Password", "", "true");
        }
        catch (Exception ex)
        {
            CallExceptionErrorPage("Send mail", ex.Message.ToString(), "Sendmail-");
        }

        if (result != string.Empty)
        {
            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.LoadXml(result);
            if (xmlDoc1.DocumentElement.Name == "error")
            {
                errMsg = result = xmlDoc1.InnerText;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showAlertDialog(\"error\",\"" + errMsg + "\");", true);
            }
            else if (xmlDoc1.DocumentElement.Name == "success")
            {
                //SMS logic is disabled from mobile and will be handeled from webservice in future.
                //GetUserMobile();
                //if (usrMob != string.Empty || usrMob != "")
                //{
                //    if (utilObj.IsNumber(usrMob))
                //        UpdatePwdbySMS();
                //}
                successMsg = result = xmlDoc1.InnerText;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showAlertDialog(\"success\",\"" + successMsg + "\");OpenSigninPagetimeset()", true);
            }
        }

        //  System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "afterreset", "AddProjs();", true);
    }
    private void CallExceptionErrorPage(string serviceName, string exception, string fileName)
    {
        logobj.CreateLog("Exception in " + serviceName + " Service :--- " + exception, HttpContext.Current.Session.SessionID, fileName, "");
    }

    [System.Web.Services.WebMethod]
    public string GetCurrAppTitle(string name)
    {
        string projTitle = string.Empty;
        projTitle = utilObj.GetConfigAttrValue(name, "AxAppTitle");
        return projTitle;
    }

    private void GetUserMobile()
    {
        string result = string.Empty;
        string sid = HttpContext.Current.Session.SessionID;
        string sql = string.Empty;
        string inputXML = string.Empty;
        sql = "select mobile from axusers where username = '" + hdnUsrName.Value + "'";
        inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sid + "' direct='true' trace=''><sql>" + sql + "</sql>";
        inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
        try
        {
            result = objWebServiceExt.CallGetChoiceWebService("", inputXML);
        }
        catch (Exception ex)
        {
            logobj.CreateLog(ex.Message, Session.SessionID, "GetUserMobile_" + sid, "new", "true");
        }
        if (result != string.Empty)
        {
            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.LoadXml(result);
            XmlNodeList nodes = xmlDoc1.DocumentElement.SelectNodes("response/row");
            if (nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.HasChildNodes)
                    {
                        if ((node.ChildNodes[0].Name.ToLower() == "mobile"))
                            usrMob = node.ChildNodes[0].InnerText;
                    }
                }
            }

        }
    }

    private void UpdatePwdbySMS()
    {
        string result = string.Empty;
        string sid = HttpContext.Current.Session.SessionID;
        string sql = string.Empty;
        string inputXML = string.Empty;
        //Entry based on standard AXSMS table structure
        sql = "insert into axsms values('" + usrMob + "','Hi, Your password has been changed " + Convert.ToString(ViewState["RandNo"]) + " as per your request. Please login immediately and change the password of your choice for security. Regards, Support Team.','0','','')";
        inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sid + "' direct='true' trace=''><sql>" + sql + "</sql>";
        inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
        try
        {
            result = objWebServiceExt.CallGetChoiceWebService("", inputXML);
        }
        catch (Exception ex)
        {
            logobj.CreateLog(ex.Message, Session.SessionID, "UpdatePwdbySMS_" + sid, "new", "true");
        }
    }

    protected void EnableCaptcha(string _proj)
    {
        try
        {
            string captcha = util.GetAllSettings(_proj, Constants.AXPCAPTCHA_CONN_KEY);
            if (captcha != "" && captcha != "nooptions")
            {
                JObject captchaJson = JObject.Parse(captcha);
                if (captchaJson["CaptchaEnabled"] != null && captchaJson["CaptchaEnabled"].ToString() == "true")
                {
                    HttpContext.Current.Session["captchaenabledFlagForgot"] = "true";
                    pnlCaptcha.Visible = true;
                    DynamicCaptcha.CodeLength = Convert.ToInt32(captchaJson["CaptchaLength"].ToString());
                    DynamicCaptcha.CodeStyle = (BotDetect.CodeStyle)Enum.Parse(typeof(BotDetect.CodeStyle), captchaJson["CaptchaStyle"].ToString(), true);
                }
                else
                {
                    pnlCaptcha.Visible = false;
                    HttpContext.Current.Session["captchaenabledFlagForgot"] = "false";
                }
            }
            else
            {
                pnlCaptcha.Visible = false;
                HttpContext.Current.Session["captchaenabledFlagForgot"] = "false";
            }
        }
        catch (Exception ex) { }
    }
}
