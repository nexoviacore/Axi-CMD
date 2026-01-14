using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using Org.BouncyCastle.Ocsp;
using Newtonsoft.Json.Linq;
using System.Security;
using System.Web.Services;

public partial class aspx_cpwd : System.Web.UI.Page
{
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    public string filename = "ChangePassword";
    public string appName;
    public string logoutPath;
    public string direction = "ltr";
    public string enableBackForwButton = "";
    public string EnableOldTheme;
    public bool isCloud = false;
    public bool isCloudApp = false;
    public string signOutPath = "";
    public string remark = "";
    public string langType = "en";
    public string strFileinfo = string.Empty;
    public string PasswordprotectKEY = "";
    public string CpFirstMsg = "Existing Password needs to be changed for first time login";
    public string CpSecondMsg = "Your password is expired. You need to change the password.";
    public string CpThirdMsg = "Your password has been reset. You need to change your password.";
    public string CpErrormsg = "Please enter a valid password";

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

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (ConfigurationManager.AppSettings["isCloudApp"] != null)
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString());
        if (Request.QueryString["remark"] != null)
        {
            remark = Request.QueryString["remark"];
            if (remark != null & remark == "1" & Session["cpwdErr"] == null & hdncheckMsg.Value != "true")
                Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script>setTimeout(function () {showAlertDialog('info','" + CpFirstMsg + "',undefined,undefined,undefined,true);},10);</script>");
            if (remark != null & remark == "2" & Session["cpwdErr"] == null & hdncheckMsg.Value != "true")
                Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script>setTimeout(function () {showAlertDialog('info','" + CpSecondMsg + "',undefined,undefined,undefined,true);},10);</script>");
            if (remark != null & remark == "3" & Session["cpwdErr"] == null & hdncheckMsg.Value != "true")
                Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script>setTimeout(function () {showAlertDialog('info','" + CpThirdMsg + "',undefined,undefined,undefined,true);},10);</script>");

            if (IsPostBack && remark != "chpwd")
            {
                if (Session["project"] != null & Session["nsessionid"] != null)
                    util.RemoveLoggedUserDetails(Session["project"].ToString(), Session["nsessionid"].ToString(), Session["username"].ToString());
            }
        }
        if (Request.UrlReferrer != null)
        {
            if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("main.aspx") | Request.UrlReferrer.AbsolutePath.ToLower().Contains("mainnew.aspx") | Request.UrlReferrer.AbsolutePath.ToLower().Contains("cpwd.aspx") | Request.UrlReferrer.AbsolutePath.ToLower().Contains("signin.aspx") | Request.UrlReferrer.AbsolutePath.ToLower().Contains("login1.aspx") | Request.UrlReferrer.AbsolutePath.ToLower().Contains("logininter.aspx") | Request.UrlReferrer.AbsolutePath.ToLower().Contains("tstruct.aspx") | Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") | Request.UrlReferrer.AbsolutePath.ToLower().Contains("ParamsTstruct.aspx")))
                Response.Redirect("../CusError/AxCustomError.aspx");
        }

        try
        {
            if (Util.Util.CheckCrossScriptingInString(remark))
            {
                try
                {
                    Response.Redirect(Constants.LOGINPAGE, true);
                }
                catch (ThreadAbortException ex)
                {
                    Thread.ResetAbort();
                }
            }
        }
        catch (Exception ex)
        {
        }
        if (!IsPostBack)
        {
            if (Session["changepwdtemp"] != null)
                Session.Remove("changepwdtemp");
        }
        if (Session["AxEnableOldTheme"] != null)
            EnableOldTheme = Session["AxEnableOldTheme"].ToString().ToLower();
        ResetSessionTime();
        if (util.IsValidQueryString(Request.RawUrl) == false)
            Response.Redirect(util.ERRPATH + Constants.INVALIDURL);

        PasswordprotectKEY = util.GetAdvConfigs("Enforced Strong Password Policy").ToString().ToLower();
        hdnencryptkey.Value = PasswordprotectKEY;

        //New password settings for setting flags 
        if (PasswordprotectKEY != "true")
        {
            string _proj = Session["project"].ToString();
            string AxOTPAuth = util.GetOTPSettings(_proj, "AxOTPAuth");
            if (AxOTPAuth == "true")
            {
                FDR fdrObj = new FDR();
                string jsoncontents = fdrObj.StringFromRedis(Constants.AXPASSWORDPOL_CONN_KEY, Session["project"].ToString());
                if (jsoncontents != string.Empty)
                {
                    hdnpwdPolicy.Value = jsoncontents.Trim();
                    btnSumit.Text = "Next";
                }
            }
        }
        // End

        string username;
        string userid;
        if (Session["user"] != null)
        {
            userid = Session["user"].ToString();
            user000F0.Value = userid;
        }
        if (Session["AxNickName"] != null)
        {
            username = Session["AxNickName"].ToString();
            username000F0.Value = username;
        }
        if (Session["project"] == null)
        {
            string url;
            Response.Write("<script>" + Constants.vbCrLf);
            Response.Write(Constants.vbCrLf + "</script>");
        }
        else
        {
            if (Session["projTitle"] != null)
                appName = Session["projTitle"].ToString();
            else
                appName = Session["AxAppTitle"].ToString();

            logoutPath = util.SIGNOUTPATH;
            if (Session["language"].ToString() == "ARABIC")
                direction = "rtl";
            if (Session["cpwdErr"] != null & Session["cpwdErr"] != string.Empty)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script>showAlertDialog('warning','" + Session["cpwdErr"].ToString() + "');displayAlertMsgOnParent()</script>");
                Session["cpwdErr"] = string.Empty;
            }

            if (Session["minPwdChars"] != null)
                pwdlength.Value = Session["minPwdChars"].ToString();
            else
                pwdlength.Value = "0";

            bool isPwdAlphaNumeric = false;
            if (Session["IsPwdAlphaNumeric"] != null)
            {
                if (Session["IsPwdAlphaNumeric"].ToString().ToLower() == "t")
                    isPwdAlphaNumeric = true;
            }
            hdnalphanumeric.Value = isPwdAlphaNumeric.ToString();
        }
        if (Session["AxAxCLOUD"] != null)
            isCloud = Convert.ToBoolean(Session["AxAxCLOUD"].ToString());
        if ((isCloud))
            signOutPath = "../" + Session["domainName"];
        else
            signOutPath = util.SIGNOUTPATH;
        string folderPath = Server.MapPath("~/images/Custom");
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderPath);
        System.IO.FileInfo[] diFileinfo = di.GetFiles();
        var custommoblogoexist = "False";
        var customlogoexist = "False";
        var Ismobile = Request.Browser.IsMobileDevice;
        if (remark != "chpwd")
        {
            foreach (var drfile in diFileinfo)
            {
                if (drfile.Length > 0 && drfile.Name.Contains("homelogo_mob"))
                {
                    strFileinfo = drfile.Name;
                    break;
                }
            }
            foreach (var drfile in diFileinfo)
            {
                // Dim filename As String = drfile.ToString
                if (drfile.Length > 0 && drfile.Name.Contains("homelogo"))
                {
                    if (drfile.Name.Contains("mp4"))
                    {
                        bgvid.Attributes.CssStyle.Add("display", "block");
                        bgvidsource.Attributes.Add("src", "./../images/Custom/homelogo.mp4?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + "");
                        customlogoexist = "True";
                    }
                    else
                    {
                        main_body.Attributes.CssStyle.Add("background", "url(./../images/Custom/homelogo.jpg?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ") center center no-repeat fixed !important");
                        main_body.Attributes.CssStyle.Add("background-size", "cover !important");
                        customlogoexist = "True";
                    }
                }
            }
            if (customlogoexist == "False")
            {
                main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/login-img.png)");
                main_body.Attributes.CssStyle.Add("background-repeat", "no-repeat");
                main_body.Attributes.CssStyle.Add("background-attachment", "fixed");
                main_body.Attributes.CssStyle.Add("background-position", "bottom");
                main_body.Attributes.CssStyle.Add("background-size", "cover !important");
            }
        }
    }
    // To Reset the Session time in clientside
    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", true);
        }
    }

    protected void btnSubmit_Click(object sender, System.EventArgs e)
    {
        string ePwd = util.CheckSpecialChars(swee000F0.Value);
        checkSecurityVal(ePwd);
        string md5pwd = util.CheckSpecialChars(swc000F0.Value);
        checkSecurityVal(md5pwd);
        string npwdString = npwdHidden.Value;
        checkSecurityVal(npwdString);
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        string aesPwd = string.Empty;
        if ((md5pwd == ""))
        {
            md5pwd = npwdHiddenMd5.Value;
            checkSecurityVal(md5pwd);
        }
        try
        {
            aesPwd = objWebServiceExt.GetEncryptedValue(npwdString);
        }
        catch (Exception ex)
        {
        }
        try
        {
            if ((md5pwd == "" | aesPwd == ""))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "closepopup", "<script>parent.showAlertDialog('warning','" + CpErrormsg + "');</script>");
                return;
            }
        }
        catch (Exception ex)
        {
        }

        string _proj = Session["project"].ToString();
        string AxOTPAuth = util.GetOTPSettings(_proj, "AxOTPAuth");
        if (AxOTPAuth == "true" && PasswordprotectKEY != "true")
        {
            FDR fdrObj = new FDR();
            string jsoncontents = fdrObj.StringFromRedis(Constants.AXEMAILSMTP_CONN_KEY, _proj);
            if (jsoncontents != string.Empty && jsoncontents != "noemailsettings")
            {
                JObject _jsonAxEmail = JObject.Parse(jsoncontents);
                if (_jsonAxEmail["arm_url"] == null)
                {
                    string armUrlmsg = "ARM connection does not exist. Please contact the administrator.";
                    ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + armUrlmsg + "\");", true);
                    return;
                }
            }
            else if (jsoncontents == string.Empty)
            {
                jsoncontents = util.GetAxEmailSettings(_proj);
                if (jsoncontents != string.Empty && jsoncontents != "noemailsettings")
                {
                    JObject _jsonAxEmail = JObject.Parse(jsoncontents);
                    if (_jsonAxEmail["arm_url"] == null)
                    {
                        string armUrlmsg = "ARM connection does not exist. Please contact the administrator.";
                        ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + armUrlmsg + "\");", true);
                        return;
                    }
                }
                else
                {
                    string armUrlmsg = "Email settings not enabled. Please contact the administrator.";
                    ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + armUrlmsg + "\");", true);
                    return;
                }
            }
            else
            {
                string armUrlmsg = "Email settings not enabled. Please contact the administrator.";
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + armUrlmsg + "\");", true);
                return;
            }
            string axOTPAuthCahrs = util.GetOTPSettings(_proj, "AxOTPAuthCahrs");
            string axOTPAuthExpiry = util.GetOTPSettings(_proj, "AxOTPAuthExpiry");
            string _isotpNumeric = "true";
            ASB.WebService wsObj = new ASB.WebService();
            string _queueSuccess = wsObj.LoginOTPPushToQueue(_proj, Session["user"].ToString(), "", axOTPAuthExpiry, axOTPAuthCahrs, _isotpNumeric, jsoncontents);
            if (_queueSuccess.StartsWith("success♣"))
            {
                string _otpAuth = HttpContext.Current.Session["_otpAuth"].ToString();
                util.SaveOTPAuth(_otpAuth, Session["user"].ToString(), axOTPAuthExpiry, _proj);
                StringBuilder sb = new StringBuilder();
                if (remark != "chpwd")
                    sb.Append("<div class=\"w-lg-500px p-8 p-lg-12 mx-auto\" style=\"margin-top: -50px;\">");
                sb.Append("<div class=\"control-group\">");
                sb.Append("<div class=\"fv-row mb-4 fv-plugins-icon-container\"><div class=\"input-icon left\"><div class=\"d-flex flex-stack mb-2\"><asp:Label ID=\"lblotp\" class=\"form-label fw-boldest text-dark fs-6 mb-0\" runat=\"server\" meta:resourcekey=\"lblotp\">Enter OTP</asp:Label></div><input id=\"axOTPpwd\" runat=\"server\" class=\"m-wrap placeholder-no-fix form-control form-control-solid\" tabindex=\"1\" type=\"text\" autocomplete=\"off\" placeholder=\"\" maxlength=\"" + axOTPAuthCahrs + "\" name=\"axOTPpwd\" title=\"Enter OTP\" required /></div></div>");
                sb.Append("<div class=\"fv-row mb-4 fv-plugins-icon-container\"><div class=\"input-icon left\"><div class=\"d-flex flex-stack mb-2\"><asp:Label ID=\"lblotpexpiry\" class=\"form-label fw-boldest text-dark fs-6 mb-0\" runat=\"server\"></asp:Label></div></div></div>");
                sb.Append("<div class=\"form-actions d-flex flex-row flex-column-fluid\"><div class=\"d-flex flex-row-fluid justify-content-between\"><a href=\"javascript:void(0)\" tabindex=\"4\" id=\"btnBackLink\" class=\"text-gray-600 d-flex my-auto fs-4 mt-4\" onclick=\"backToPwdDiv()\"><span class=\"material-icons material-icons-style\">chevron_left</span>Back</a><a href=\"javascript:void(0)\" tabindex=\"3\" id=\"btnResendotp\" class=\"text-gray-600 d-flex my-auto fs-4 mt-4 d-none\" onclick=\"btnResendOTP();\">Resend OTP</a><input type=\"button\" value=\"Save\" title=\"Save\" TabIndex=\"2\" ID=\"btnSavePwd\" data-type=\"otp\" class=\"btn btn-lg btn-primary mb-5 w-50\" onclick=\"return chnPwdoptauth();\" /></div></div>");
                sb.Append("</div>");
                if (remark != "chpwd")
                    sb.Append("</div>");
                panelOtp.Text = sb.ToString();

                Session["changepwdtemp"] = aesPwd + "♣" + md5pwd + "♣" + ePwd;

                string _emailId = _queueSuccess.Split('♣')[2];
                string _mobileNo = _queueSuccess.Split('♣')[1];
                string _otpsuccmsg = string.Empty;
                if (_emailId != string.Empty && _mobileNo != string.Empty)
                {
                    _emailId = MaskEmail(_emailId);
                    _mobileNo = MaskMobileNumber(_mobileNo.Trim());
                    if (_mobileNo != string.Empty && _mobileNo.StartsWith("error:"))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + _mobileNo + "\");", true);
                        return;
                    }
                    else
                        _otpsuccmsg = "An OTP has been sent to your mobile number " + _mobileNo + " and email id " + _emailId + "";
                }
                else if (_emailId != string.Empty && _mobileNo == string.Empty)
                {
                    _emailId = MaskEmail(_emailId);
                    _otpsuccmsg = "An OTP has been sent to your email id " + _emailId + "";
                }
                else if (_emailId == string.Empty && _mobileNo != string.Empty)
                {
                    _mobileNo = MaskMobileNumber(_mobileNo.Trim());
                    if (_mobileNo != string.Empty && _mobileNo.StartsWith("error:"))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + _mobileNo + "\");", true);
                        return;
                    }
                    else
                        _otpsuccmsg = "An OTP has been sent to your mobile number " + _mobileNo + "";
                }
                string setIns = "javascript:chngPwdOtpExpires('" + axOTPAuthExpiry + "','" + _otpsuccmsg + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", setIns, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + _queueSuccess + "\");", true);
            }
        }
        else
            CallChangePwdWS(aesPwd, md5pwd, ePwd);

        //ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        //string errorLog;
        //errorLog = logobj.CreateLog("Call to Change password web service", Session["nsessionid"].ToString(), "ChangePwd", "new");
        //string result = string.Empty;
        //string changeByAdmin = "no";
        //string ixml = "<root axpapp='" + Session["project"].ToString() + "' loginuser='" + Session["user"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' user='" + Session["user"].ToString() + "' action=''>";
        //ixml += "<pwd>" + aesPwd + "</pwd><md5pwd>" + md5pwd + "</md5pwd><oldpwd>" + ePwd + "</oldpwd><changebyadmin>" + changeByAdmin + "</changebyadmin>";
        //ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";


        //try
        //{
        //    result = objWebServiceExt.CallChangePassword(ixml);
        //    if (result == "<error> Incorrect old password.</error>")
        //        result = "<error> Incorrect existing password.</error>";
        //}
        //catch (Exception ex)
        //{
        //}

        //string errMsg = string.Empty;
        //errMsg = util.ParseXmlErrorNode(result);

        //if (errMsg != string.Empty)
        //{
        //    if (errMsg.Contains(Constants.SESSIONERROR) | errMsg.Contains(Constants.SESSIONEXPMSG))
        //    {
        //        Session.RemoveAll();
        //        Session.Abandon();
        //        string url1;
        //        Response.Write("<script>" + Constants.vbCrLf);
        //        Response.Write(Constants.vbCrLf + "</script>");
        //    }
        //    else
        //    {
        //        Session["cpwdErr"] = errMsg;
        //        Response.Redirect("cpwd.aspx?remark=" + Request.QueryString["remark"]);
        //    }
        //}


        //if ((result.ToLower() == "done" | result.Contains("<success>")))
        //{
        //    if ((Request.QueryString["remark"] == "1") | (Request.QueryString["remark"] == "2") | (Request.QueryString["remark"] == "3"))
        //    {
        //        Session["validated"] = "True";

        //        System.Web.Configuration.SessionStateSection sessionStateSection = (System.Web.Configuration.SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
        //        string cookieName = sessionStateSection.CookieName;
        //        HttpCookie mycookie = new HttpCookie(cookieName);
        //        mycookie.Expires = DateTime.Now.AddDays(-1);
        //        HttpContext.Current.Response.Cookies.Add(mycookie);
        //        System.Web.HttpContext.Current.Session.Abandon();
        //        SessionIDManager manager = new SessionIDManager();
        //        manager.RemoveSessionID(System.Web.HttpContext.Current);

        //        Page.ClientScript.RegisterStartupScript(GetType(), "closepopup", "<script>parent.showAlertDialog('success','" + result + "'); setInterval(function(){window.location.href= '../aspx/SignIn.aspx';},1000)</script>");
        //    }
        //    else
        //    {
        //        dvMainPwd.Style.Add("display", "none");
        //        tabbody1.Style.Add("display", "none");

        //        Session["changeSuccess"] = result;
        //        Page.ClientScript.RegisterStartupScript(GetType(), "closepopup", "<script>closeDialog('" + result + "','../aspx/SignIn.aspx')</script>");
        //    }
        //}
        //else
        //{
        //    Session["cpwdErr"] = errMsg;
        //    Response.Redirect("cpwd.aspx?remark=" + Request.QueryString["remark"]);
        //}
    }

    protected void CallChangePwdWS(string aesPwd, string md5pwd, string ePwd)
    {
        try
        {
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            string errorLog;
            errorLog = logobj.CreateLog("Call to Change password web service", Session["nsessionid"].ToString(), "ChangePwd", "new");
            string result = string.Empty;
            string changeByAdmin = "no";
            string ixml = "<root axpapp='" + Session["project"].ToString() + "' loginuser='" + Session["user"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' user='" + Session["user"].ToString() + "' action=''>";
            ixml += "<pwd>" + aesPwd + "</pwd><md5pwd>" + md5pwd + "</md5pwd><oldpwd>" + ePwd + "</oldpwd><changebyadmin>" + changeByAdmin + "</changebyadmin>";
            ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";


            try
            {
                result = objWebServiceExt.CallChangePassword(ixml);
                if (result == "<error> Incorrect old password.</error>")
                    result = "<error> Incorrect existing password.</error>";
                else if (result == "<success>Password is changed successfully</success>")
                    result = "<success>Password is changed successfully. Please relogin with changed password</success>";
            }
            catch (Exception ex)
            {
            }

            string errMsg = string.Empty;
            errMsg = util.ParseXmlErrorNode(result);

            if (errMsg != string.Empty)
            {
                if (errMsg.Contains(Constants.SESSIONERROR) | errMsg.Contains(Constants.SESSIONEXPMSG))
                {
                    Session.RemoveAll();
                    Session.Abandon();
                    string url1;
                    Response.Write("<script>" + Constants.vbCrLf);
                    Response.Write(Constants.vbCrLf + "</script>");
                }
                else
                {
                    Session["cpwdErr"] = errMsg;
                    Response.Redirect("cpwd.aspx?remark=" + Request.QueryString["remark"]);
                }
            }


            if ((result.ToLower() == "done" | result.Contains("<success>")))
            {
                if ((Request.QueryString["remark"] == "1") | (Request.QueryString["remark"] == "2") | (Request.QueryString["remark"] == "3"))
                {
                    Session["validated"] = "True";

                    System.Web.Configuration.SessionStateSection sessionStateSection = (System.Web.Configuration.SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
                    string cookieName = sessionStateSection.CookieName;
                    HttpCookie mycookie = new HttpCookie(cookieName);
                    mycookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(mycookie);
                    System.Web.HttpContext.Current.Session.Abandon();
                    SessionIDManager manager = new SessionIDManager();
                    manager.RemoveSessionID(System.Web.HttpContext.Current);

                    Page.ClientScript.RegisterStartupScript(GetType(), "closepopup", "<script>parent.ShowDimmer(true); parent.showAlertDialog('success','" + result + "'); setTimeout(function(){window.location.href= '../aspx/SignIn.aspx';},200)</script>");
                }
                else
                {
                    dvMainPwd.Style.Add("display", "none");
                    tabbody1.Style.Add("display", "none");

                    Session["changeSuccess"] = result;
                    Page.ClientScript.RegisterStartupScript(GetType(), "closepopup", "<script>closeDialog('" + result + "','/aspx/SignIn.aspx')</script>");
                }
            }
            else
            {
                Session["cpwdErr"] = errMsg;
                Response.Redirect("cpwd.aspx?remark=" + Request.QueryString["remark"]);
            }
        }
        catch (Exception ex) { }
    }


    protected string MaskEmail(string email)
    {
        try
        {
            if (email != string.Empty)
            {
                int atIndex = email.IndexOf('@');
                if (atIndex > 0)
                {
                    string localPart = email.Substring(0, atIndex);
                    string domainPart = email.Substring(atIndex);

                    if (localPart.Length > 2)
                    {
                        string maskedLocalPart = localPart.Substring(0, 2) + new string('*', localPart.Length - 2);
                        return maskedLocalPart + domainPart;
                    }
                }
                return email;
            }
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("MaskEmail - EmailID- " + email + "\n\tError - " + ex.Message, HttpContext.Current.Session.SessionID, "Changepwd-MaskEmail", "new", "true");
            return string.Empty;
        }
    }
    protected string MaskMobileNumber(string mobileNo)
    {
        try
        {
            if (mobileNo != string.Empty)
            {
                if (mobileNo.Length >= 4)
                {
                    string lastFour = mobileNo.Substring(mobileNo.Length - 4);
                    if (!lastFour.All(char.IsDigit))
                        return "error: Invalid mobile number to send OTP : " + mobileNo;
                }

                int lastFourStartIndex = mobileNo.Length - 4;
                string maskedPart = new string('*', lastFourStartIndex - 3);
                return mobileNo.Substring(0, 3) + maskedPart + mobileNo.Substring(lastFourStartIndex);
            }
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("MaskMobileNumber - MobileNo- " + mobileNo + "\n\tError - " + ex.Message, HttpContext.Current.Session.SessionID, "Changepwd-MaskMobileNumber", "new", "true");
            return "error: Invalid mobile number to send OTP : " + mobileNo;
        }
    }

    public static string GetPwdAuthLang(string username, string proj)
    {
        string res = string.Empty;
        try
        {
            Util.Util utils = new Util.Util();
            string fdKeypwdOtpAuth = Constants.REDISPWDOTPAUTHLANG;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
            {
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            else
            {
                utils.GetAxApps(proj);
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            }
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
                fObj = new FDR();
            if (fObj != null)
                res = fObj.StringFromRedis(utils.GetRedisServerkey(fdKeypwdOtpAuth, username), schemaName);
        }
        catch (Exception ex)
        { }
        return res;
    }
    protected void checkSecurityVal(string _curValue)
    {
        try
        {
            if (Util.Util.CheckCrossScriptingInString(_curValue))
            {
                try
                {
                    Response.Redirect(Constants.LOGINPAGE, true);
                }
                catch (ThreadAbortException ex)
                {
                    Thread.ResetAbort();
                }
            }

            if (Util.Util.ContainsXSS(_curValue))
            {
                try
                {
                    Response.Redirect(Constants.LOGINPAGE, true);
                }
                catch (ThreadAbortException ex)
                {
                    Thread.ResetAbort();
                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                Response.Redirect(Constants.LOGINPAGE, true);
            }
            catch (ThreadAbortException tex)
            {
                Thread.ResetAbort();
            }
        }
    }

    protected void btnResendOtp_Click(object sender, EventArgs e)
    {
        string _proj = Session["project"].ToString();
        string AxOTPAuth = util.GetOTPSettings(_proj, "AxOTPAuth");
        if (AxOTPAuth == "true")
        {
            FDR fdrObj = new FDR();
            string jsoncontents = fdrObj.StringFromRedis(Constants.AXEMAILSMTP_CONN_KEY, _proj);
            if (jsoncontents != string.Empty && jsoncontents != "noemailsettings")
            {
                JObject _jsonAxEmail = JObject.Parse(jsoncontents);
                if (_jsonAxEmail["arm_url"] == null)
                {
                    string armUrlmsg = "ARM connection does not exist. Please contact the administrator.";
                    ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + armUrlmsg + "\");", true);
                    return;
                }
            }
            else if (jsoncontents == string.Empty)
            {
                jsoncontents = util.GetAxEmailSettings(_proj);
                if (jsoncontents != string.Empty && jsoncontents != "noemailsettings")
                {
                    JObject _jsonAxEmail = JObject.Parse(jsoncontents);
                    if (_jsonAxEmail["arm_url"] == null)
                    {
                        string armUrlmsg = "ARM connection does not exist. Please contact the administrator.";
                        ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + armUrlmsg + "\");", true);
                        return;
                    }
                }
                else
                {
                    string armUrlmsg = "Email settings not enabled. Please contact the administrator.";
                    ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + armUrlmsg + "\");", true);
                    return;
                }
            }
            else
            {
                string armUrlmsg = "Email settings not enabled. Please contact the administrator.";
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + armUrlmsg + "\");", true);
                return;
            }
            string axOTPAuthCahrs = util.GetOTPSettings(_proj, "AxOTPAuthCahrs");
            string axOTPAuthExpiry = util.GetOTPSettings(_proj, "AxOTPAuthExpiry");
            string _isotpNumeric = "true";
            ASB.WebService wsObj = new ASB.WebService();
            string _queueSuccess = wsObj.LoginOTPPushToQueue(_proj, Session["user"].ToString(), "", axOTPAuthExpiry, axOTPAuthCahrs, _isotpNumeric, jsoncontents);
            if (_queueSuccess.StartsWith("success♣"))
            {
                string _otpAuth = HttpContext.Current.Session["_otpAuth"].ToString();
                util.SaveOTPAuth(_otpAuth, Session["user"].ToString(), axOTPAuthExpiry, _proj);
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=\"control-group\">");
                sb.Append("<div class=\"fv-row mb-4 fv-plugins-icon-container\"><div class=\"input-icon left\"><div class=\"d-flex flex-stack mb-2\"><asp:Label ID=\"lblotp\" class=\"form-label fw-boldest text-dark fs-6 mb-0\" runat=\"server\" meta:resourcekey=\"lblotp\">Enter OTP</asp:Label></div><input id=\"axOTPpwd\" runat=\"server\" class=\"m-wrap placeholder-no-fix form-control form-control-solid\" tabindex=\"1\" type=\"text\" autocomplete=\"off\" placeholder=\"\" maxlength=\"" + axOTPAuthCahrs + "\" name=\"axOTPpwd\" title=\"Enter OTP\" required /></div></div>");
                sb.Append("<div class=\"fv-row mb-4 fv-plugins-icon-container\"><div class=\"input-icon left\"><div class=\"d-flex flex-stack mb-2\"><asp:Label ID=\"lblotpexpiry\" class=\"form-label fw-boldest text-dark fs-6 mb-0\" runat=\"server\"></asp:Label></div></div></div>");
                sb.Append("<div class=\"form-actions d-flex flex-row flex-column-fluid\"><div class=\"d-flex flex-row-fluid justify-content-between\"><a href=\"javascript:void(0)\" tabindex=\"4\" id=\"btnBackLink\" class=\"text-gray-600 d-flex my-auto fs-4 mt-4\" onclick=\"backToPwdDiv()\"><span class=\"material-icons material-icons-style\">chevron_left</span>Back</a><a href=\"javascript:void(0)\" tabindex=\"3\" id=\"btnResendotp\" class=\"text-gray-600 d-flex my-auto fs-4 mt-4 d-none\" onclick=\"btnResendOTP();\">Resend OTP</a><input type=\"button\" value=\"Save\" title=\"Save\" TabIndex=\"2\" ID=\"btnSavePwd\" data-type=\"otp\" class=\"btn btn-lg btn-primary mb-5 w-50\" onclick=\"return chnPwdoptauth();\" /></div></div>");
                sb.Append("</div>");
                panelOtp.Text = sb.ToString();

                string _emailId = _queueSuccess.Split('♣')[2];
                string _mobileNo = _queueSuccess.Split('♣')[1];
                string _otpsuccmsg = string.Empty;
                if (_emailId != string.Empty && _mobileNo != string.Empty)
                {
                    _emailId = MaskEmail(_emailId);
                    _mobileNo = MaskMobileNumber(_mobileNo);
                    if (_mobileNo != string.Empty && _mobileNo.StartsWith("error:"))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + _mobileNo + "\");", true);
                        return;
                    }
                    else
                        _otpsuccmsg = "An OTP has been sent to your mobile number " + _mobileNo + " and email id " + _emailId + "";
                }
                else if (_emailId != string.Empty && _mobileNo == string.Empty)
                {
                    _emailId = MaskEmail(_emailId);
                    _otpsuccmsg = "An OTP has been sent to your email id " + _emailId + "";
                }
                else if (_emailId == string.Empty && _mobileNo != string.Empty)
                {
                    _mobileNo = MaskMobileNumber(_mobileNo);
                    if (_mobileNo != string.Empty && _mobileNo.StartsWith("error:"))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"" + _mobileNo + "\");", true);
                        return;
                    }
                    else
                        _otpsuccmsg = "An OTP has been sent to your mobile number " + _mobileNo + "";
                }
                string setIns = "javascript:chngPwdOtpExpires('" + axOTPAuthExpiry + "','" + _otpsuccmsg + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", setIns, true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetOtpErrorMsg(\"Error occurred. Please try again or contact administrator.\");", true);
        }
    }

    [WebMethod]
    public static string chkOtppwd(string stsotpauth)
    {
        string res = string.Empty;
        try
        {
            if (Util.Util.CheckCrossScriptingInString(stsotpauth) || Util.Util.ContainsXSS(stsotpauth))
                throw new SecurityException("Invalid format.");

            if (HttpContext.Current.Session["user"] == null)
            {
                res = "InvalidUser";
                return res;
            }

            if (stsotpauth != null && stsotpauth != string.Empty)
            {
                Util.Util utils = new Util.Util();
                string _otpAuth = utils.GetOTPAuth(HttpContext.Current.Session["user"].ToString(), HttpContext.Current.Session["project"].ToString());
                if (_otpAuth != string.Empty)
                {
                    string[] _thisDetails = _otpAuth.Split('♣');
                    if (_thisDetails[0] == stsotpauth)
                    {
                        res = "success";
                    }
                    else
                    {
                        res = "InvalidOTP";
                    }
                }
                else
                {
                    res = "OTPexpired";
                }
            }
            else
            {
                res = "PleaseenterOTP";
            }
        }
        catch (Exception ex) { }

        return res;
    }

    protected void btnOTPpwdChange_Click(object sender, EventArgs e)
    {
        if (Session["changepwdtemp"] != null)
        {
            string[] changepwdtemp = Session["changepwdtemp"].ToString().Split('♣');
            Session.Remove("changepwdtemp");
            CallChangePwdWS(changepwdtemp[0], changepwdtemp[1], changepwdtemp[2]);
        }
    }
}