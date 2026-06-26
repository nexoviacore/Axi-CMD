using Newtonsoft.Json.Linq;
using Saml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Workflownotification : System.Web.UI.Page
{
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    ASB.WebService asbWebService = new ASB.WebService();
    ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    ArrayList globalVariables = new ArrayList();
    public string direction = "ltr";
    public string langType = "en";
    string transId = "";
    string recordId = "";
    string actName = "";
    public string saveBtnName = "Save";
    public string actNameComment = "";
    string workflowId = "";
    string lno = "";
    string elno = "";
    string comments = "";
    bool isChanged = false;
    string proj = "";
    string username = "";

    public static string ssoredirecturl = string.Empty;

    public string oktaclientKey = string.Empty;
    public string oktasecretKey = string.Empty;
    public string oktadomain = string.Empty;

    public string isOfficeSSO = string.Empty;
    public string office365clientKey = string.Empty;
    public string office365secretKey = string.Empty;

    public static string googleclientKey = string.Empty;
    public static string googlesecretKey = string.Empty;

    public static string fbclientKey = string.Empty;
    public static string fbsecretKey = string.Empty;

    public string onlyssoKey = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["enc"] != null)
        {
            string details = Decrypt(Request.QueryString["enc"]);
            string[] workFlowDtls = details.Split(',');
            proj = workFlowDtls[0];
            transId = workFlowDtls[1];
            recordId = workFlowDtls[2];
            workflowId = workFlowDtls[3];
            lno = workFlowDtls[4];
            elno = workFlowDtls[5];
            actName = workFlowDtls[6];
            isAuthorized.Value = workFlowDtls[7];
            if (isAuthorized.Value == "F")
                username = workFlowDtls[8];
            if (actName == "approve")
            {
                actNameComment = "Approved";
                saveBtnName = "Approve";
            }
            else if (actName == "return")
            {
                actNameComment = "Returned";
                saveBtnName = "Return";
            }
            else if (actName == "reject")
            {
                actNameComment = "Rejected";
                saveBtnName = "Reject";
            }
            else if (actName == "review")
            {
                actNameComment = "Reviewed";
                saveBtnName = "Review";
            }
        }

        if (!IsPostBack)
        {
            SetAxpertLogo();
            if (Session["LoginWith"] != null)
            {
                switch (Session["LoginWith"].ToString())
                {
                    case "google":
                        FetchUserSocialDetail("google");
                        break;
                    case "facebook":
                        FetchUserSocialDetail("facebook");
                        break;
                    case "saml":
                        FetchUserSocialDetail("saml");
                        break;
                }
            }
            else if (Request.QueryString["ssotype"] != null)
            {
                if (Request.QueryString["ssotype"] == "office365")
                {
                    Session["LoginWith"] = "office365";
                    FetchUserSocialDetail("office365", Request.QueryString["code"].ToString());
                }
                else if (Request.QueryString["ssotype"] == "okta")
                {
                    Session["LoginWith"] = "okta";
                    FetchUserSocialDetail("okta", Request.QueryString["code"].ToString());
                }
            }
            else
            {
                if (isAuthorized.Value != "F")
                    SSOButtonsEnable(proj);
            }
        }
    }

    protected void SSOButtonsEnable(string _strProj)
    {
        try
        {
            if (_strProj != string.Empty)
            {
                //FDR fdrObj = new FDR();
                //string SSOJsoncontent = fdrObj.StringFromRedis(Constants.AXSSO_CONN_KEY, _strProj);
                FDR fdrObj = new FDR();
                string SSOJsoncontent = fdrObj.HashGetKeyWithSchema(Constants.AX_COMMON_APPSETTING_KEY, Constants.AXSSO_CONN_KEY, _strProj);
                if (SSOJsoncontent == string.Empty)
                {
                    string ScriptsPath = ConfigurationManager.AppSettings["ScriptsPath"].ToString();
                    FileInfo fi = new FileInfo(ScriptsPath + "\\AppSettings.ini");
                    if (fi.Exists)
                    {
                        string scriptsPathAxFile = ScriptsPath + "\\AppSettings.ini";
                        string filePatharm = @" " + scriptsPathAxFile + "";
                        string existingJsonAxFile = File.ReadAllText(filePatharm);
                        JObject jsonAxFile = JObject.Parse(existingJsonAxFile);
                        if (jsonAxFile["appsettings"] != null && jsonAxFile["appsettings"][_strProj] != null && jsonAxFile["appsettings"][_strProj]["SSO"] != null)
                        {
                            SSOJsoncontent = jsonAxFile["appsettings"][_strProj]["SSO"].ToString();
                            enableSSOBtnsKeys(SSOJsoncontent);

                            FDW fdwObj = new FDW();
                            //fdwObj.SaveInRedisServer(Constants.AXSSO_CONN_KEY, SSOJsoncontent, Constants.AXSSO_CONN_KEY, _strProj);
                            fdwObj.HashSetKeyWithSchema(Constants.AX_COMMON_APPSETTING_KEY, Constants.AXSSO_CONN_KEY, SSOJsoncontent, _strProj);
                        }
                        else
                        {
                            FDW fdwObj = new FDW();
                            //fdwObj.SaveInRedisServer(Constants.AXSSO_CONN_KEY, "nossoconnection", Constants.AXSSO_CONN_KEY, _strProj);
                            fdwObj.HashSetKeyWithSchema(Constants.AX_COMMON_APPSETTING_KEY, Constants.AXSSO_CONN_KEY, "nossoconnection", _strProj);
                        }
                    }
                    else
                    {
                        FDW fdwObj = new FDW();
                        //fdwObj.SaveInRedisServer(Constants.AXSSO_CONN_KEY, "nossoconnection", Constants.AXSSO_CONN_KEY, _strProj);
                        fdwObj.HashSetKeyWithSchema(Constants.AX_COMMON_APPSETTING_KEY, Constants.AXSSO_CONN_KEY, "nossoconnection", _strProj);
                    }
                }
                else if (SSOJsoncontent != "nossoconnection")
                {
                    enableSSOBtnsKeys(SSOJsoncontent);
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("Exception in GetAxConfigFileServer - util.cs-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxConfigFileServer", "new");
        }
    }
    protected void enableSSOBtnsKeys(string SSOJsoncontent)
    {
        JObject config = JObject.Parse(SSOJsoncontent);
        foreach (var section in config)
        {
            if (section.Key == "windows")
            {
                JObject sectionObject = section.Value as JObject;
                if (sectionObject != null)
                {
                    foreach (JProperty property in sectionObject.Properties())
                    {
                        switch (property.Name)
                        {
                            case "ssoType":
                                WindowsBtn.Visible = true;
                                divsso.Attributes["class"] = divsso.Attributes["class"].Replace("d-none", "").Trim();
                                break;
                            case "ssoWinDomain":
                                Session["windDomain"] = property.Value.ToString();
                                break; ;
                            case "onlysso":
                                if (onlyssoKey == "")
                                    onlyssoKey = property.Value.ToString() == "true" ? "windows" : "";
                                break;
                        }
                    }
                }
            }
            else if (section.Key == "saml")
            {
                JObject sectionObject = section.Value as JObject;
                if (sectionObject != null)
                {
                    foreach (JProperty property in sectionObject.Properties())
                    {
                        switch (property.Name)
                        {
                            case "ssoType":
                                SamlBtn.Visible = true;
                                divsso.Attributes["class"] = divsso.Attributes["class"].Replace("d-none", "").Trim();
                                break;
                            case "SamlPartnerIdP":
                                Session["SamlPartnerIdP"] = property.Value.ToString();
                                break;
                            case "SamlIdentifier":
                                Session["SamlIdentifier"] = property.Value.ToString();
                                break;
                            case "SamlCertificate":
                                Session["SamlCertificate"] = property.Value.ToString();
                                break;
                            case "SamlRedirectUrl":
                                ssoredirecturl = property.Value.ToString();
                                Session["SamlRedirectUrl"] = property.Value.ToString();
                                break;
                            case "onlysso":
                                if (onlyssoKey == "")
                                    onlyssoKey = property.Value.ToString() == "true" ? "saml" : "";
                                break;
                        }
                    }
                }
            }
            else if (section.Key == "office365")
            {
                JObject sectionObject = section.Value as JObject;
                if (sectionObject != null)
                {
                    foreach (JProperty property in sectionObject.Properties())
                    {
                        switch (property.Name)
                        {
                            case "ssoType":
                                Office365Btn.Visible = true;
                                divsso.Attributes["class"] = divsso.Attributes["class"].Replace("d-none", "").Trim();
                                break;
                            case "of365clientkey":
                                office365clientKey = property.Value.ToString();
                                break;
                            case "of365secretkey":
                                office365secretKey = property.Value.ToString();
                                break;
                            case "of365redirecturl":
                                ssoredirecturl = property.Value.ToString();
                                break;
                            case "onlysso":
                                if (onlyssoKey == "")
                                    onlyssoKey = property.Value.ToString() == "true" ? "office365" : "";
                                break;
                        }
                    }
                }
            }
            else if (section.Key == "okta")
            {
                JObject sectionObject = section.Value as JObject;
                if (sectionObject != null)
                {
                    foreach (JProperty property in sectionObject.Properties())
                    {
                        switch (property.Name)
                        {
                            case "ssoType":
                                OktaBtn.Visible = true;
                                divsso.Attributes["class"] = divsso.Attributes["class"].Replace("d-none", "").Trim();
                                break;
                            case "oktaclientkey":
                                oktaclientKey = property.Value.ToString();
                                break;
                            case "oktasecretkey":
                                oktasecretKey = property.Value.ToString();
                                break;
                            case "oktadomain":
                                oktadomain = property.Value.ToString();
                                break;
                            case "oktaredirecturl":
                                ssoredirecturl = property.Value.ToString();
                                break;
                            case "onlysso":
                                if (onlyssoKey == "")
                                    onlyssoKey = property.Value.ToString() == "true" ? "okta" : "";
                                break;
                        }
                    }
                }
            }
            else if (section.Key == "google")
            {
                JObject sectionObject = section.Value as JObject;
                if (sectionObject != null)
                {
                    foreach (JProperty property in sectionObject.Properties())
                    {
                        switch (property.Name)
                        {
                            case "ssoType":
                                GoogleBtn.Visible = true;
                                divsso.Attributes["class"] = divsso.Attributes["class"].Replace("d-none", "").Trim();
                                break;
                            case "googleclientkey":
                                googleclientKey = property.Value.ToString();
                                break;
                            case "googlesecretkey":
                                googlesecretKey = property.Value.ToString();
                                break;
                            case "googleredirecturl":
                                ssoredirecturl = property.Value.ToString();
                                break;
                            case "onlysso":
                                if (onlyssoKey == "")
                                    onlyssoKey = property.Value.ToString() == "true" ? "google" : "";
                                break;
                        }
                    }
                }
            }
            else if (section.Key == "facebook")
            {
                JObject sectionObject = section.Value as JObject;
                if (sectionObject != null)
                {
                    foreach (JProperty property in sectionObject.Properties())
                    {
                        switch (property.Name)
                        {
                            case "ssoType":
                                FacebookBtn.Visible = true;
                                divsso.Attributes["class"] = divsso.Attributes["class"].Replace("d-none", "").Trim();
                                break;
                            case "fbclientkey":
                                fbclientKey = property.Value.ToString();
                                break;
                            case "fbsecretkey":
                                fbsecretKey = property.Value.ToString();
                                break;
                            case "fbredirecturl":
                                ssoredirecturl = property.Value.ToString();
                                break;
                            case "onlysso":
                                if (onlyssoKey == "")
                                    onlyssoKey = property.Value.ToString() == "true" ? "facebook" : "";
                                break;
                        }
                    }
                }
            }
        }
    }

    public string getlicDetails()
    {
        var result = asbExt.CallAxpString();
        var macid = "";
        var licfile = "";

        if (result != string.Empty)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(result);
            }
            catch (Exception ex)
            {
            }
            XmlNode resultNode = null;


            resultNode = xmlDoc.SelectSingleNode("//result");
            if (resultNode != null)
            {
                foreach (XmlNode childNode in resultNode.ChildNodes)
                {
                    if (childNode.Name == "macid")
                        macid = childNode.InnerText;
                    else if (childNode.Name == "licfile")
                        licfile = childNode.InnerText;
                }
            }


        }
        var licDetails = " macid='" + macid + "' licfile='" + licfile + "'";
        return licDetails;
    }

    private string GetBrowserDetails()
    {
        System.Web.HttpBrowserCapabilities browser = Request.Browser;
        string userDetails = browser.Type + "¿" + browser.Browser + "¿"
            + browser.Version + "¿" + browser.MajorVersion + "¿"
            + browser.MinorVersion + "¿" + browser.Platform + "¿"
            + Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];

        if (userDetails.Length > 200)
            userDetails = userDetails.Substring(0, 200);

        return userDetails;
    }

    private string GetAxProps()
    {
        string contents = "";
        string scriptsPath = ConfigurationManager.AppSettings["ScriptsPath"].ToString();
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(scriptsPath);

        if (di.Exists)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(scriptsPath + "axprops.xml");

            while (reader.Read())
            {
                reader.MoveToContent();
                if (reader.NodeType == System.Xml.XmlNodeType.Element)
                    contents += "<" + reader.Name + ">";
                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                    contents += reader.Value + "";
                else if (reader.NodeType == System.Xml.XmlNodeType.EndElement) //Display the end of the element.
                    contents += "</" + reader.Name + ">";

            }
        }
        return contents;
    }

    private string GetAxApps(string project)
    {
        string contents = "";
        string scriptsPath = string.Empty;

        if (project == "" && ConfigurationManager.AppSettings["proj"] != null && ConfigurationManager.AppSettings["proj"].ToString() != "")
            project = ConfigurationManager.AppSettings["proj"].ToString();
        scriptsPath = ConfigurationManager.AppSettings["ScriptsPath"].ToString();
        Application["axdb"] = "Oracle";
        if (string.IsNullOrEmpty(project))
            return "";
        string db = string.Empty;
        int dbIdx = -1;
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(scriptsPath);
        if (di.Exists)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(scriptsPath + "axapps.xml");
            bool startRead = false;
            string lastEmptyNode = string.Empty;
            using (reader)
            {
                while (reader.Read())
                {
                    reader.MoveToContent();

                    if (reader.NodeType == System.Xml.XmlNodeType.Element && reader.Name == project)
                    {
                        startRead = true;
                        contents += "<" + reader.Name + ">";
                    }
                    else if (reader.NodeType == System.Xml.XmlNodeType.Element && startRead == true)
                    {
                        if (reader.Name == "db")
                        {
                            dbIdx = 0;
                        }

                        contents += "<" + reader.Name + ">";
                        if (reader.IsEmptyElement)
                        {
                            lastEmptyNode = reader.Name;
                            contents += "</" + reader.Name + ">";
                        }
                    }

                    if (reader.NodeType == System.Xml.XmlNodeType.Text && startRead == true)
                    {
                        if (dbIdx != -1)
                        {
                            db = reader.Value;
                            dbIdx = -1;
                        }
                        contents += reader.Value + "";
                    }
                    else
                    {
                        if (reader.Name == lastEmptyNode)
                        {
                            lastEmptyNode = string.Empty;
                            continue;
                        }
                        if (reader.NodeType == System.Xml.XmlNodeType.EndElement && reader.Name == project)
                        {
                            startRead = false;
                            contents += "</" + reader.Name + ">";
                            break;
                        }
                        else if (reader.NodeType == System.Xml.XmlNodeType.EndElement && startRead == true) //Display the end of the element.
                            contents += "</" + reader.Name + ">";
                    }

                }
            }

        }

        if (contents != string.Empty)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(contents);
            XmlNodeList dbuser = xmlDoc.GetElementsByTagName("dbuser");
            string appPWD = GetDBPasswordService(dbuser[0].InnerText);
            if (xmlDoc.DocumentElement.SelectSingleNode("pwd") == null)
            {
                XmlElement pwd = xmlDoc.CreateElement("pwd");
                xmlDoc.DocumentElement.AppendChild(pwd);
            }
            xmlDoc.DocumentElement.SelectSingleNode("pwd").InnerText = appPWD;
            contents = xmlDoc.OuterXml;
        }

        return contents;
    }

    public string GetDBPasswordService(string username)
    {
        string pwd = "";
        LogFile.Log logobj = new LogFile.Log();
        string ScriptsPath = ConfigurationManager.AppSettings["ScriptsPath"].ToString();
        string fileName = ScriptsPath + username + ".pwd";
        try
        {
            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    String encryptedPWD = sr.ReadToEnd();
                    pwd = encryptedPWD.Trim();
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in getting password :--- " + ex.Message.ToString(), "", "GetDBPasswordService", "New");
        }
        return pwd;
    }

    protected void submit_Click(object sender, EventArgs e)
    {
        string result;
        username = axUserName.Value;
        string licDetails = getlicDetails();
        string userDetails = GetBrowserDetails();
        var axApps = GetAxApps(proj);
        var axProps = GetAxProps();
        Session.Abandon();
        var sid = GenerateNewSessionID();

        Random rand = new Random();
        var seed = rand.Next(1000, 9999).ToString();
        var pwrd = MD5Hash(seed + MD5Hash(axPassword.Value));
        var errlog = "";
        if (ConfigurationManager.AppSettings["LoginTrace"] != null)
        {
            string loginTrace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            if (loginTrace.ToLower() == "true")
                errlog = logobj.CreateLog("Call to WorkFlow Notification Login Web Service", sid, "wflogin", "", "true");
            else
                errlog = logobj.CreateLog("Call to WorkFlow Notification  Web Service", sid, "wflogin", "");
        }
        var lang_attr = "";
        var sXml = "<login ip='' other='" + userDetails + "'  seed='" + seed + "'  axpapp='" + proj + "' sessionid='" + sid + "' username='" + username + "' password='" + pwrd + "' url='' direct='t' trace='" + errlog + "' " + lang_attr + licDetails + ">";
        sXml = sXml + axApps + axProps + "</login>";
        result = objWebServiceExt.CallLoginWS("main", sXml);
        if (result != "")
            result = result.Split('♠')[1];
        if (result.StartsWith(Constants.ERROR) || result.Contains(Constants.ERROR))
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(result);
            string msg = string.Empty;
            XmlNode errorNode = xmldoc.SelectSingleNode("/error");
            if (result.Contains("\n"))
                result = result.Replace("\n", "");

            foreach (XmlNode msgNode in errorNode)
            {
                if (msgNode.Name == "msg")
                {
                    msg = msgNode.InnerText;
                    break;
                }
            }

            if (msg == string.Empty && errorNode.InnerText != string.Empty)
                msg = errorNode.InnerText;
            if (msg.ToLower().Contains("ora-"))
                msg = "Error occurred(2). Please try again or contact administrator.";
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('error','" + msg + "'); </script>", false);
        }
        else if (result.StartsWith("<result") || result.Contains("<result"))
        {
            ParseLoginResult(result);
            ViewState["AppSessionKey"] = HttpContext.Current.Session["AppSessionKey"].ToString();
            ViewState["axGlobalVars"] = HttpContext.Current.Session["axGlobalVars"].ToString();
            ViewState["axUserVars"] = HttpContext.Current.Session["axUserVars"].ToString();
            ViewState["wfaUserName"] = username;

            string inputXML = string.Empty;
            string resultCho = string.Empty;
            try
            {
                inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sid + "' direct='true' trace='" + "true" + "' appsessionkey='" + ViewState["AppSessionKey"] + "' username='" + username + "' >";
                inputXML += "<sql>select status from axtasks where towhom='" + username + "' and sname='" + transId + "' and recordid=" + recordId + " and app_level=" + lno + " order by upddatetime desc</sql>";
                inputXML += axApps + axProps + ViewState["axGlobalVars"] + ViewState["axUserVars"] + "</sqlresultset>";
                resultCho = asbExt.CallGetChoiceWS("", inputXML);
                if (resultCho.StartsWith(Constants.ERROR) || resultCho.Contains(Constants.ERROR))
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(resultCho);
                    string msg = string.Empty;
                    XmlNode errorNode = xmldoc.SelectSingleNode("/error");
                    if (resultCho.Contains("\n"))
                        resultCho = resultCho.Replace("\n", "");

                    foreach (XmlNode msgNode in errorNode)
                    {
                        if (msgNode.Name == "msg")
                        {
                            msg = msgNode.InnerText;
                            break;
                        }
                    }

                    if (msg == string.Empty && errorNode.InnerText != string.Empty)
                        msg = errorNode.InnerText;
                    if (msg.ToLower().Contains("ora-"))
                        msg = "Error occurred(2). Please try again or contact administrator.";
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('error','" + msg + "'); </script>", false);
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(resultCho);
                    XmlNode resultNode = xmlDoc.SelectSingleNode("/sqlresultset/response/row");
                    if (resultNode == null)
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('error','Does not have the access to " + actName + "'); </script>", false);
                    else
                    {
                        foreach (XmlNode childNode in resultNode.ChildNodes)
                        {
                            if (childNode.InnerText == "0")
                            {
                                string strMsg = "This record was " + (actName.ToString().ToLower() == "approve" ? "approved" : actName + "ed") + " by other user";
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('warning','" + strMsg + "'); </script>", false);
                            }
                            else if (childNode.InnerText == "2")
                            {
                                string strMsg = "This workflow was canceled by the user";
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('warning','" + strMsg + "'); </script>", false);
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> createModal(); </script>", false);
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
    }

    private string GenerateNewSessionID()
    {
        SessionIDManager manager = new SessionIDManager();

        string newID = manager.CreateSessionID(Context);
        bool redirected = false;
        bool isAdded = false;
        manager.SaveSessionID(Context, newID, out redirected, out isAdded);
        return newID;
    }

    protected void comment_Click(object sender, EventArgs e)
    {
        var sid = "";
        sid = Session.SessionID;
        var axApps = GetAxApps(proj);
        var axProps = GetAxProps();
        var comments = comment.Value;
        if (comments != "")
        {
            string errlog = string.Empty;
            if (ConfigurationManager.AppSettings["LoginTrace"] != null)
            {
                string loginTrace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
                if (loginTrace.ToLower() == "true")
                    errlog = logobj.CreateLog("Call to WorkFlow Notification WorkFlowActionWS", sid, "WorkFlowAction", "", "true");
                else
                    errlog = logobj.CreateLog("Call to WorkFlow Notification WorkFlowActionWS", sid, "WorkFlowAction", "");
            }
            isChanged = true;
            username = username == "" ? ViewState["wfaUserName"].ToString() : username;
            var inputXml = string.Empty;
            if (isAuthorized.Value == "F")
                inputXml = "<root axpapp='" + proj + "' trace='" + errlog + "'  sessionid='" + sid + "' transid='" + transId + "'  recordid='" + recordId + "' actname='" + actName + "' comments='" + comments + "' appsessionkey='" + ViewState["AppSessionKey"] + "' changed='" + isChanged + "' lno='" + lno + "' elno='" + elno + "' wfmailauth='" + isAuthorized.Value + "' usrname='" + username + "'>";
            else
                inputXml = "<root axpapp='" + proj + "' trace='" + errlog + "'  sessionid='" + sid + "' transid='" + transId + "'  recordid='" + recordId + "' actname='" + actName + "' comments='" + comments + "' appsessionkey='" + ViewState["AppSessionKey"] + "' changed='" + isChanged + "' lno='" + lno + "' elno='" + elno + "' wfmailauth='" + isAuthorized.Value + "' username='" + username + "'>";
            inputXml = inputXml + ViewState["axGlobalVars"] + ViewState["axUserVars"] + axApps + axProps + "</root>";

            var workflowResult = objWebServiceExt.CallWorkFlowActionWS(transId, inputXml);
            if (workflowResult.StartsWith(Constants.ERROR) || workflowResult.Contains(Constants.ERROR))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(workflowResult);
                string msg = string.Empty;
                XmlNode errorNode = xmldoc.SelectSingleNode("/error");
                if (workflowResult.Contains("\n"))
                    workflowResult = workflowResult.Replace("\n", "");

                foreach (XmlNode msgNode in errorNode)
                {
                    if (msgNode.Name == "msg")
                    {
                        msg = msgNode.InnerText;
                        break;
                    }
                }

                if (msg == string.Empty && errorNode.InnerText != string.Empty)
                    msg = errorNode.InnerText;
                if (msg.ToLower().Contains("ora-"))
                    msg = "Error occurred(2). Please try again or contact administrator.";
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('error','" + msg.Replace("'", "") + "'); </script>", false);
            }
            else if (workflowResult == "done")
            {
                string action;
                if (actName == "approve")
                {
                    action = actName + "d";
                }
                else
                {
                    action = actName + "ed";
                }
                string msg = "Workflow " + action + " successfully.";
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "SetWFAErrorMsg('success','" + msg + "');", true);
            }

        }
    }

    private string md5(string sPassword)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(sPassword);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();
    }


    public static string Decrypt(string inp)
    {
        string codes64 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/!@#$%^&*(){}|[];:<>?,.";
        int i, a, x, b;
        string result = "";
        a = 0;
        b = 0;
        for (i = 0; i < inp.Length; i++)
        {

            x = (codes64.IndexOf(inp[i]));
            if (x >= 0)
            {
                b = b * 64 + x;
                a = a + 6;
                if (a >= 8)
                {
                    a = a - 8;
                    x = b >> a;
                    b = b % (1 << a);
                    x = x % 256;
                    result = result + Convert.ToChar(x);
                }
            }
            else
            {
                System.Environment.Exit(1);
            }
        }
        return result;
    }

    private void ParseLoginResult(string result)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(result);

        XmlNode resultNode = xmlDoc.SelectSingleNode("/result");
        foreach (XmlNode childNode in resultNode.ChildNodes)
        {
            if (childNode.Name == "globalvars")
            {
                HttpContext.Current.Session["axGlobalVars"] = "<globalvars>" + childNode.InnerXml + "</globalvars>";
                bool axp_timezone = false;
                foreach (XmlNode xmlNode in childNode.ChildNodes)
                {
                    if (xmlNode.Name.ToLower() == "axp_timezone" && xmlNode.InnerText != "")
                    {
                        axp_timezone = true;
                        if (childNode.SelectSingleNode("timediff") != null)
                        {
                            childNode.SelectSingleNode("timediff").InnerText = "";
                        }
                    }
                }
                if (axp_timezone)
                {
                    HttpContext.Current.Session["axGlobalVars"] = "<globalvars>" + childNode.InnerXml + "</globalvars>";
                }
            }
            else if (childNode.Name == "uservars")
            {
                HttpContext.Current.Session["axUserVars"] = "<uservars>" + childNode.InnerXml + "</uservars>";
            }
        }
    }

    public static string MD5Hash(string text)
    {
        MD5 md5 = new MD5CryptoServiceProvider();

        //compute hash from the bytes of text  
        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

        //get hash result after compute it  
        byte[] result = md5.Hash;

        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < result.Length; i++)
        {
            //change it into 2 hexadecimal digits  
            //for each byte  
            strBuilder.Append(result[i].ToString("x2"));
        }

        return strBuilder.ToString();
    }


    //SSO Authentication 
    protected void GoogleBtn_Click(object sender, EventArgs e)
    {
        GetSocialCredentials("google");
    }

    protected void FacebookBtn_Click(object sender, EventArgs e)
    {
        GetSocialCredentials("facebook");
    }

    protected void WindowsBtn_Click(object sender, EventArgs e)
    {
        if (Session["windDomain"] != null && Session["windDomain"].ToString() != "")
        {
            string domainName = Session["windDomain"].ToString();
            string[] dname = domainName.Split('.');
            string adPath = string.Empty;
            if (dname.Length == 2)
                adPath = "LDAP://" + domainName + "/DC=" + dname[0] + ",DC=" + dname[1] + "";
            else if (dname.Length == 3)
                adPath = "LDAP://" + domainName + "/DC=" + dname[0] + ",DC=" + dname[1] + ",DC=" + dname[2] + "";
            LdapAuthentication adAuth = new LdapAuthentication(adPath);
            try
            {
                Session["LoginWith"] = "windows";
                if (true == adAuth.IsAuthenticated(dname[0], axUserName.Value, axPassword.Value))
                {
                    string SSOKey = adAuth._guid;
                    string groups = adAuth.GetGroups();
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, axUserName.Value, DateTime.Now, DateTime.Now.AddMinutes(60), false, groups);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);
                    CallLoginWS(true, axUserName.Value, proj, SSOKey, "windows");
                }
                else
                {
                    string loginPath = Application["LoginPath"].ToString();
                    string queryProj = string.Empty;
                    if (Session["queryProj"] != null)
                        queryProj = Session["queryProj"].ToString();
                    string msg = "Authentication failed, check username and password.";
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('error','" + msg + "'); </script>", false);
                    //ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetLoginErrorMsg('" + msg + "');", true);
                }
                Session.Remove("LoginWith");
            }
            catch (Exception ex)
            {
                Session.Remove("LoginWith");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('error','" + ex.Message.ToString().Replace("\r\n", "") + "'); </script>", false);
                //ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetLoginErrorMsg('" + ex.Message.ToString().Replace("\r\n", "") + "');", true);
                return;
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetLoginErrorMsg('Authentication failed, check windows doamin name.');", true);
            return;
        }
    }

    private void GetSocialCredentials(String provider)
    {
        ISSO ssoObject = SSO.ObjectHelper(provider);
        Session["LoginWith"] = provider;
        if (provider == "google")
        {
            ssoObject.clientID = googleclientKey;
            ssoObject.clientSecret = googlesecretKey;
            ssoObject.redirectUrl = ssoredirecturl + "aspx/signin.aspx";
        }
        else if (provider == "facebook")
        {
            ssoObject.clientID = fbclientKey;
            ssoObject.clientSecret = fbsecretKey;
            ssoObject.redirectUrl = ssoredirecturl + "aspx/signin.aspx";
        }
        Response.Redirect(ssoObject.GetRequestURL());
    }

    private void FetchUserSocialDetail(string provider, string SSOIdUser = "")
    {
        try
        {
            if (provider == "google")
            {
                var url = Request.Url.Query;
                if (!string.IsNullOrEmpty(url))
                {
                    string queryString = url.ToString();
                    string[] words = queryString.Split('=');
                    string code = words[1];
                    if (!string.IsNullOrEmpty(code))
                    {
                        Google.Request request = new Google.Request();
                        request.clientID = googleclientKey;
                        request.clientSecret = googlesecretKey;
                        request.redirectUrl = ssoredirecturl + "aspx/Workflownotification.aspx";
                        string parameters = request.GetParameters(code);
                        string response = MakeWebRequest("https://accounts.google.com/o/oauth2/token", "POST", "application/x-www-form-urlencoded", parameters);
                        Google.Token tokenInfo = new JavaScriptSerializer().Deserialize<Google.Token>(response);

                        if (tokenInfo != null)
                        {
                            if (!string.IsNullOrEmpty(tokenInfo.access_token))
                            {
                                var googleInfo = MakeWebRequest("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + tokenInfo.access_token, "GET");
                                Google.Info profile = new JavaScriptSerializer().Deserialize<Google.Info>(googleInfo);
                                CallLoginWS(true, profile.email, proj, profile.id, "ga");
                            }
                        }
                    }
                }
                Session.Remove("LoginWith");
            }
            else if (provider == "facebook")
            {
                if (Request["code"] != null)
                {
                    Facebook.Request request = new Facebook.Request();
                    request.clientID = fbclientKey;
                    request.clientSecret = fbsecretKey;
                    request.redirectUrl = ssoredirecturl + "aspx/Workflownotification.aspx";
                    string url = request.GetParameters(Request["code"]);
                    string tokenResponse = MakeWebRequest(url, "GET");
                    var tokenInfo = new JavaScriptSerializer().Deserialize<Facebook.Token>(tokenResponse);
                    var facebookInfoJson = MakeWebRequest(request.AccessUrl + tokenInfo.access_token, "GET");
                    Facebook.Info objUser = new JavaScriptSerializer().Deserialize<Facebook.Info>(facebookInfoJson);
                    CallLoginWS(true, objUser.email, proj, tokenInfo.access_token, "fb");
                }
                Session.Remove("LoginWith");
            }
            else if (provider == "office365")
            {
                string officeDetail = SSOIdUser;
                if (officeDetail != string.Empty)
                {
                    string[] officeDetails = officeDetail.Split(new[] { "*$*" }, StringSplitOptions.None);
                    CallLoginWS(true, officeDetails[1].ToString(), proj, officeDetails[0].ToString(), "of365");
                }
                Session.Remove("LoginWith");
            }
            else if (provider == "okta")
            {
                string officeDetail = SSOIdUser;
                if (officeDetail != string.Empty)
                {
                    string[] officeDetails = officeDetail.Split(new[] { "*$*" }, StringSplitOptions.None);
                    CallLoginWS(true, officeDetails[1].ToString(), proj, officeDetails[0].ToString(), "ot");
                }
                Session.Remove("LoginWith");
            }
            else if (provider == "saml")
            {
                try
                {
                    string qstr = util.encrtptDecryptAES(Request.QueryString["res"], false);
                    string[] resQstr = qstr.Split('&');
                    string samlDetail = string.Empty;
                    string samlProj = string.Empty;
                    foreach (var resVar in resQstr)
                    {
                        string[] resStr = resVar.Split('=');
                        if (resStr[0] == "Project")
                            samlProj = resStr[1];
                        else if (resStr[0] == "code")
                            samlDetail = resStr[1];
                    }
                    if (samlDetail != string.Empty)
                    {
                        string[] samlDetails = samlDetail.Split(new[] { "*$*" }, StringSplitOptions.None);
                        CallLoginWS(true, samlDetails[1].ToString(), samlProj, samlDetails[0].ToString(), "saml");
                    }
                }
                catch (Exception ex) { }
                Session.Remove("LoginWith");
            }
        }
        catch (Exception ex)
        {
            Session.Remove("LoginWith");
            Response.Redirect("error.aspx");
        }
    }

    /// <summary>
    /// Calling 3rd party web apis. 
    /// </summary>
    /// <param name="destinationUrl"></param>
    /// <param name="methodName"></param>
    /// <param name="requestJSON"></param>
    /// <returns></returns>
    public string MakeWebRequest(string destinationUrl, string methodName, string contentType = "", string requestJSON = "")
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            request.Method = methodName;
            if (methodName == "POST")
            {
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestJSON);
                request.ContentType = contentType;
                request.ContentLength = bytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            return null;
        }
        catch (WebException webEx)
        {
            return webEx.Message;
        }
    }

    protected void CallLoginWS(bool isSSO, string UserName, string strProj, string ssoKey, string ssoName)
    {
        string result;
        username = UserName;
        proj = strProj;
        string licDetails = getlicDetails();
        string userDetails = GetBrowserDetails();
        var axApps = GetAxApps(strProj);
        var axProps = GetAxProps();
        Session.Abandon();
        var sid = GenerateNewSessionID();

        Random rand = new Random();
        string rnd_key = rand.Next(1000, 9999).ToString();
        var errlog = "";
        if (ConfigurationManager.AppSettings["LoginTrace"] != null)
        {
            string loginTrace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            if (loginTrace.ToLower() == "true")
                errlog = logobj.CreateLog("Call to WorkFlow Notification Login Web Service", sid, "wflogin", "", "true");
            else
                errlog = logobj.CreateLog("Call to WorkFlow Notification  Web Service", sid, "wflogin", "");
        }
        var lang_attr = "";
        var sXml = "<login singleloginkey=\"" + ssoKey + "\" ssoname=\"" + ssoName + "\" ip='' other='" + userDetails + "' seed='" + rnd_key + "' axpapp='" + strProj + "' sessionid='" + sid + "' username='" + UserName + "' password='' url='' direct='t' trace='" + errlog + "' " + lang_attr + licDetails + ">";
        sXml = sXml + axApps + axProps + "</login>";
        result = objWebServiceExt.CallLoginWS("main", sXml);
        if (result != "")
            result = result.Split('♠')[1];
        if (result.StartsWith(Constants.ERROR) || result.Contains(Constants.ERROR))
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(result);
            string msg = string.Empty;
            XmlNode errorNode = xmldoc.SelectSingleNode("/error");
            if (result.Contains("\n"))
                result = result.Replace("\n", "");

            foreach (XmlNode msgNode in errorNode)
            {
                if (msgNode.Name == "msg")
                {
                    msg = msgNode.InnerText;
                    break;
                }
            }

            if (msg == string.Empty && errorNode.InnerText != string.Empty)
                msg = errorNode.InnerText;
            if (msg.ToLower().Contains("ora-"))
                msg = "Error occurred(2). Please try again or contact administrator.";
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('error','" + msg + "'); </script>", false);
        }
        else if (result.StartsWith("<result") || result.Contains("<result"))
        {
            ParseLoginResult(result);
            ViewState["AppSessionKey"] = HttpContext.Current.Session["AppSessionKey"].ToString();
            ViewState["axGlobalVars"] = HttpContext.Current.Session["axGlobalVars"].ToString();
            ViewState["axUserVars"] = HttpContext.Current.Session["axUserVars"].ToString();
            ViewState["wfaUserName"] = username;


            string inputXML = string.Empty;
            string resultCho = string.Empty;
            try
            {
                inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sid + "' direct='true' trace='" + "true" + "' appsessionkey='" + ViewState["AppSessionKey"] + "' username='" + username + "' >";
                inputXML += "<sql>select status from axtasks where towhom='" + username + "' and sname='" + transId + "' and recordid=" + recordId + " and app_level=" + lno + " order by upddatetime desc</sql>";
                inputXML += axApps + axProps + ViewState["axGlobalVars"] + ViewState["axUserVars"] + "</sqlresultset>";
                resultCho = asbExt.CallGetChoiceWS("", inputXML);
                if (resultCho.StartsWith(Constants.ERROR) || resultCho.Contains(Constants.ERROR))
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(resultCho);
                    string msg = string.Empty;
                    XmlNode errorNode = xmldoc.SelectSingleNode("/error");
                    if (resultCho.Contains("\n"))
                        resultCho = resultCho.Replace("\n", "");

                    foreach (XmlNode msgNode in errorNode)
                    {
                        if (msgNode.Name == "msg")
                        {
                            msg = msgNode.InnerText;
                            break;
                        }
                    }

                    if (msg == string.Empty && errorNode.InnerText != string.Empty)
                        msg = errorNode.InnerText;
                    if (msg.ToLower().Contains("ora-"))
                        msg = "Error occurred(2). Please try again or contact administrator.";
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('error','" + msg + "'); </script>", false);
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(resultCho);
                    XmlNode resultNode = xmlDoc.SelectSingleNode("/sqlresultset/response/row");
                    if (resultNode == null)
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('error','Does not have the access to " + actName + "'); </script>", false);
                    else
                    {
                        foreach (XmlNode childNode in resultNode.ChildNodes)
                        {
                            if (childNode.InnerText == "0")
                            {
                                string strMsg = "This record was " + (actName.ToString().ToLower() == "approve" ? "approved" : actName + "ed") + " by other user";
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('warning','" + strMsg + "'); </script>", false);
                            }
                            else if (childNode.InnerText == "2")
                            {
                                string strMsg = "This workflow was canceled by the user";
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> SetWFAErrorMsg('warning','" + strMsg + "'); </script>", false);
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Workflow", "<script type='text/javascript'> createModal(); </script>", false);
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
    }

    protected void SetAxpertLogo()
    {
        string folderPath = Server.MapPath("~/images/Custom");
        DirectoryInfo di = new DirectoryInfo(folderPath);
        FileInfo[] diFileinfo = di.GetFiles();
        var customlogoexist = "False";
        foreach (var drfile in diFileinfo)
        {
            if (drfile.Length > 0 && drfile.Name.Contains("homelogo"))
            {
                main_body.Attributes.CssStyle.Add("background", "url(./../images/Custom/" + drfile.Name + "?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ") no-repeat center center fixed !important ");
                main_body.Attributes.CssStyle.Add("background-size", "cover !important");
                main_body.Attributes.CssStyle.Add("height", "100vh !important");
                customlogoexist = "True";
                break;
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

    protected void SamlBtn_Click(object sender, EventArgs e)
    {
        try
        {
            Session["LoginWith"] = "saml";
            Session["Project"] = proj;

            //TODO: specify the SAML provider url here, aka "Endpoint"
            var samlEndpoint = Session["SamlPartnerIdP"].ToString();// ConfigurationManager.AppSettings["SamlPartnerIdP"].ToString();
            string returnUrl = Session["SamlRedirectUrl"].ToString();// ConfigurationManager.AppSettings["ssoredirecturl"].ToString();
            returnUrl += "aspx/samlresponse.aspx?Project=" + Session["Project"].ToString() + "&fromwf=true&enc=" + Request.QueryString["enc"];
            string SamlIdentifier = Session["SamlIdentifier"].ToString();// ConfigurationManager.AppSettings["SamlIdentifier"].ToString();
            var request = new AuthRequest(SamlIdentifier, returnUrl);

            Response.Redirect(request.GetRedirectUrl(samlEndpoint));
        }
        catch (Exception ex)
        {
            Session.Remove("LoginWith");
            ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SetLoginErrorMsg('" + ex.Message.ToString().Replace("\r\n", "") + "');", true);
            return;
        }
    }
}
