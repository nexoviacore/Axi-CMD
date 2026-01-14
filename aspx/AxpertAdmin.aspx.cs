using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using StackExchange.Redis;

public partial class AxpertAdmin : System.Web.UI.Page
{
    public string appTitle = "Axpert Admin Settings";
    public static string jsonText = string.Empty;
    //public string existingJsonARM = string.Empty;
    //public string existingJsonFile = string.Empty;
    public string existingAppSettings = "{}";
    public string existingJsonSSO = string.Empty;
    public string selProj = string.Empty;
    public string direction = "ltr";
    public string langType = "en";
    Util.Util util = new Util.Util();
    public string isLicExist = "true";
    public string authPopup = "false";
    public string strudioScriptUrlMesg = string.Empty;
    //[NonSerialized]
    public static ConfigurationOptions Rconfig;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ConfigurationManager.AppSettings["EnableAxpertConfiguration"] == null || ConfigurationManager.AppSettings["EnableAxpertConfiguration"].ToString() == "false")
            {
                Response.Redirect("signin.aspx", true);
            }
            else if (ConfigurationManager.AppSettings["EnableAxpertConfiguration"] != null && ConfigurationManager.AppSettings["EnableAxpertConfiguration"].ToString() == "true" && Request.QueryString["auth"] == null)
            {
                PanelAuthenticate.Visible = true;
                panelewConnection.Visible = false;
                authPopup = "true";
                main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/login-img.png)");
                main_body.Attributes.CssStyle.Add("background-repeat", "no-repeat");
                main_body.Attributes.CssStyle.Add("background-attachment", "fixed");
                main_body.Attributes.CssStyle.Add("background-position", "bottom");
                main_body.Attributes.CssStyle.Add("background-size", "cover !important");
                return;
            }
            else if (ConfigurationManager.AppSettings["EnableAxpertConfiguration"] != null && ConfigurationManager.AppSettings["EnableAxpertConfiguration"].ToString() == "true" && Request.QueryString["auth"] != null)
            {
                main_body.Style.Remove("background");
                configbody.Attributes["class"] = "row-fluid";
                //configbody.Attributes["class"] = configbody.Attributes["class"].Replace("row-fluid m-autoss1", "row-fluid");

                string authInfo = util.encrtptDecryptAES(Request.QueryString["auth"], false);
                if (Session.SessionID != authInfo)
                {
                    PanelAuthenticate.Visible = true;
                    panelewConnection.Visible = false;

                    authPopup = "true";
                    return;
                }
            }

            string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            string appSettingsPath = Path.Combine(scriptsPath, "AppSettings.ini");
            FileInfo fileInfo = new FileInfo(appSettingsPath);
            try
            {
                if (fileInfo.Exists)
                {
                    existingAppSettings = File.ReadAllText(appSettingsPath);
                }
                else
                {
                    createAppSettingsFirstTime();
                    Util.Util uti = new Util.Util();
                    string _appsettings = uti.GetAxAppSettings();
                    if (!string.IsNullOrEmpty(_appsettings))
                    {
                        HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
                    }
                    Response.Redirect("AxpertAdmin.aspx?auth=" + Request.QueryString["auth"].ToString());
                }
            }
            catch (Exception ex)
            {
                existingAppSettings = "{}";
            }

            jsonText = string.Empty;
            // SetAxpertLogo();
            if (Request.QueryString["proj"] != null)
            {
                hdnselecproj.Value = Request.QueryString["proj"].ToString();
                selProj = Request.QueryString["proj"].ToString();
            }
            List<string> lst = new List<string>();
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            FileInfo fi = new FileInfo(ScriptsPath + "\\axapps.xml");
            if (fi.Exists)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ScriptsPath + "\\axapps.xml");
                try
                {
                    XmlNodeList pNode = doc.SelectSingleNode("/connections").ChildNodes;
                    foreach (XmlNode nodename in pNode)
                    {
                        lst.Add(nodename.Name);
                    }
                }
                catch (Exception ex)
                { }
                jsonText = doc.OuterXml;
                jsonText = util.CheckSpecialChars(jsonText);
            }
            lstconnection.DataSource = lst;
            axSelectProj.DataSource = lst;
            armproj.DataSource = lst;
            fileproj.DataSource = lst;
            ssoProj.DataSource = lst;
            //RMQueueproj.DataSource = lst;
            armExtResource.DataSource = lst;
            lstconnection.DataBind();
            axSelectProj.DataBind();
            armproj.DataBind();
            fileproj.DataBind();
            ssoProj.DataBind();
            //RMQueueproj.DataBind();
            armExtResource.DataBind();
            lstRconnection.DataSource = lst;
            lstRconnection.DataBind();
            selLicDomain.DataSource = lst;
            selLicDomain.DataBind();
            lstStudioRconnection.DataSource = lst;
            lstStudioRconnection.DataBind();

            //IsLicExists();
        }
    }

    private void createAppSettingsFirstTime()
    {
        List<string> lst = new List<string>();
        try
        {
            JObject finalJson = new JObject();
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            FileInfo fi = new FileInfo(ScriptsPath + "\\axapps.xml");
            if (fi.Exists)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ScriptsPath + "\\axapps.xml");
                JObject appConnections = new JObject();
                foreach (XmlNode connNode in doc.DocumentElement.ChildNodes)
                {
                    string connName = connNode.Name;
                    lst.Add(connName);
                    JObject connDetails = new JObject();
                    foreach (XmlNode child in connNode.ChildNodes)
                    {
                        connDetails[child.Name] = child.InnerText.Trim();
                    }
                    if (connDetails["dbuser"] != null)
                    {
                        string dbUser = connDetails["dbuser"].ToString();
                        connDetails["dbuser"] = dbUser; // no Replace needed
                    }
                    appConnections[connName] = connDetails;
                }
                finalJson.Add("appconnections", appConnections);

            }

            string jsonOutput = JsonConvert.SerializeObject(finalJson, Newtonsoft.Json.Formatting.Indented);
            string ScriptsPathApp = ScriptsPath + "AppSettings.ini";
            string filePath = ScriptsPathApp;
            FileInfo Filefi = new FileInfo(filePath);
            if (!Filefi.Exists)
            {
                File.WriteAllText(filePath, jsonOutput);
            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(existingJson))
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(jsonOutput);

                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });

                    File.WriteAllText(filePath, json.ToString(Newtonsoft.Json.Formatting.Indented));
                }
            }

        }
        catch (Exception ex) { }

        try
        {
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            string ssoFile = Path.Combine(ScriptsPath, "ssoinfoconfig.ini");

            if (File.Exists(ssoFile))
            {
                JObject ssoData = JObject.Parse(File.ReadAllText(ssoFile));
                string appSettingsFile = Path.Combine(ScriptsPath, "AppSettings.ini");
                JObject _finalJson = File.Exists(appSettingsFile) ? JObject.Parse(File.ReadAllText(appSettingsFile)) : new JObject();
                if (_finalJson["appsettings"] == null)
                    _finalJson["appsettings"] = new JObject();
                JObject appSettings = (JObject)_finalJson["appsettings"];
                foreach (var project in ssoData.Properties())
                {
                    string projName = project.Name;
                    JObject projSSO = (JObject)project.Value;
                    if (appSettings[projName] == null)
                        appSettings[projName] = new JObject();
                    JObject projSettings = (JObject)appSettings[projName];
                    projSettings["SSO"] = projSSO;
                }
                string _jsonOutput = JsonConvert.SerializeObject(_finalJson, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(appSettingsFile, _jsonOutput);
            }
        }
        catch (Exception ex) { }

        try
        {
            string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            string emailFile = Path.Combine(scriptsPath, "emailsettings.ini");
            if (File.Exists(emailFile))
            {
                JObject emailData = JObject.Parse(File.ReadAllText(emailFile));
                string appSettingsFile = Path.Combine(scriptsPath, "AppSettings.ini");
                JObject finalJson = File.Exists(appSettingsFile) ? JObject.Parse(File.ReadAllText(appSettingsFile)) : new JObject();
                if (finalJson["appsettings"] == null)
                    finalJson["appsettings"] = new JObject();
                JObject appSettings = (JObject)finalJson["appsettings"];
                foreach (var project in emailData.Properties())
                {
                    string projName = project.Name;
                    JObject projEmail = (JObject)project.Value;
                    if (appSettings[projName] == null)
                        appSettings[projName] = new JObject();
                    JObject projSettings = (JObject)appSettings[projName];
                    projSettings["EmailSettings"] = projEmail;
                }
                string jsonOutput = JsonConvert.SerializeObject(finalJson, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(appSettingsFile, jsonOutput);
            }
        }
        catch (Exception ex) { }

        try
        {
            string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            string ARMFile = Path.Combine(scriptsPath, "armconfig.ini");
            if (File.Exists(ARMFile))
            {
                string webScriptsPath = "";
                if (ConfigurationManager.AppSettings["ScriptsPath"] != null && ConfigurationManager.AppSettings["ScriptsPath"].ToString() != string.Empty)
                    webScriptsPath = ConfigurationManager.AppSettings["ScriptsPath"].ToString();
                string scriptsUrlPath = "";
                if (ConfigurationManager.AppSettings["scriptsUrlPath"] != null && ConfigurationManager.AppSettings["scriptsUrlPath"].ToString() != string.Empty)
                    scriptsUrlPath = ConfigurationManager.AppSettings["scriptsUrlPath"].ToString();

                JObject ARMData = JObject.Parse(File.ReadAllText(ARMFile));
                string appSettingsFile = Path.Combine(scriptsPath, "AppSettings.ini");
                JObject finalJson = File.Exists(appSettingsFile) ? JObject.Parse(File.ReadAllText(appSettingsFile)) : new JObject();
                if (finalJson["appsettings"] == null)
                    finalJson["appsettings"] = new JObject();
                JObject appSettings = (JObject)finalJson["appsettings"];
                foreach (var project in ARMData.Properties())
                {
                    string projName = project.Name;
                    JObject projARM = (JObject)project.Value;
                    projARM["ScriptsPath"] = webScriptsPath;
                    projARM["scriptsUrlPath"] = scriptsUrlPath;
                    if (appSettings[projName] == null)
                        appSettings[projName] = new JObject();
                    JObject projSettings = (JObject)appSettings[projName];
                    projSettings["ARM"] = projARM;
                }
                string jsonOutput = JsonConvert.SerializeObject(finalJson, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(appSettingsFile, jsonOutput);
            }
        }
        catch (Exception ex) { }

        try
        {
            string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            string fileconfig = Path.Combine(scriptsPath, "fileconfig.ini");
            if (File.Exists(fileconfig))
            {
                JObject fileconfigData = JObject.Parse(File.ReadAllText(fileconfig));
                string appSettingsFile = Path.Combine(scriptsPath, "AppSettings.ini");
                JObject finalJson = File.Exists(appSettingsFile) ? JObject.Parse(File.ReadAllText(appSettingsFile)) : new JObject();
                if (finalJson["appsettings"] == null)
                    finalJson["appsettings"] = new JObject();
                JObject appSettings = (JObject)finalJson["appsettings"];
                foreach (var project in fileconfigData.Properties())
                {
                    string projName = project.Name;
                    JObject projfileconfig = (JObject)project.Value;
                    if (appSettings[projName] == null)
                        appSettings[projName] = new JObject();
                    JObject projSettings = (JObject)appSettings[projName];
                    projSettings["FileConfig"] = projfileconfig;
                }
                string jsonOutput = JsonConvert.SerializeObject(finalJson, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(appSettingsFile, jsonOutput);
            }
        }
        catch (Exception ex) { }


        try
        {
            JObject finalJson = new JObject();
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            string redisFilePath = Path.Combine(ScriptsPath, "redisconns.xml");
            FileInfo redisfi = new FileInfo(redisFilePath);
            if (redisfi.Exists)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(redisFilePath);
                XmlNode root = doc.DocumentElement;
                JObject finalJsonredis = new JObject();
                if (root != null && root.HasChildNodes)
                {
                    XmlNode firstConnNode = root.FirstChild;
                    JObject connDetails = new JObject();
                    foreach (XmlNode child in firstConnNode.ChildNodes)
                    {
                        connDetails[child.Name] = child.InnerText.Trim();
                    }
                    finalJsonredis["Redis"] = connDetails;
                }
                foreach (var appcon in lst)
                {
                    finalJson[appcon] = finalJsonredis;
                }

                string appSettingsFile = Path.Combine(ScriptsPath, "AppSettings.ini");
                JObject appsettingJson = File.Exists(appSettingsFile)
                    ? JObject.Parse(File.ReadAllText(appSettingsFile))
                    : new JObject();
                if (appsettingJson["appsettings"] == null)
                    appsettingJson["appsettings"] = new JObject();
                JObject appSettings = (JObject)appsettingJson["appsettings"];
                foreach (var project in finalJson.Properties())
                {
                    string projName = project.Name;
                    JObject projfileconfig = (JObject)project.Value;
                    if (appSettings[projName] == null)
                    {
                        appSettings[projName] = projfileconfig;
                    }
                    else
                    {
                        ((JObject)appSettings[projName]).Merge(projfileconfig, new JsonMergeSettings
                        {
                            MergeArrayHandling = MergeArrayHandling.Union,
                            MergeNullValueHandling = MergeNullValueHandling.Ignore
                        });
                    }
                }
                string jsonOutput = JsonConvert.SerializeObject(appsettingJson, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(appSettingsFile, jsonOutput);
            }

        }
        catch (Exception ex) { }
    }

    protected void IsLicExists()
    {
        try
        {
            string lic_redis = string.Empty;
            string lic_file = string.Empty;
            bool licExist = false;
            string redisLicDetails = GetServerLicDetails();
            if (redisLicDetails != string.Empty && !redisLicDetails.StartsWith("error:"))
            {
                lic_redis = redisLicDetails.Split('=')[1].ToString().Trim('\'');
                licExist = true;
            }
            else
            {
                string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                string[] files = System.IO.Directory.GetFiles(ScriptsPath, "*.lic");
                if (files.Count() > 0)
                {
                    lic_file = files[0].Split('\\').Last();
                    licExist = true;
                }
            }
            if (licExist == false)
                isLicExist = "false";
            else
            {
                string trace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
                string inputJson = "{\"_parameters\":[{\"getlicenseinfo\":{\"trace\":\"" + trace + "\"},\"licdetails\":{\"lic_redis\":\"" + lic_redis + "\",\"lic_file\":\"" + lic_file + "\"}}]}";

                ASB.WebService objws = new ASB.WebService();
                string res = objws.CallGetLicenseInfoWS(inputJson);
                var details = JObject.Parse(res);
                string licno = details["result"][0]["result"]["licno"].ToString();
                string licExpiry = details["result"][0]["result"]["expiry"].ToString();
                if (licExpiry == "Perpetual")
                    lbllicExpiry.InnerText = licExpiry;
                else
                    lbllicExpiry.InnerText = "Expires on " + licExpiry;
                if (licno == "templic")
                {
                    lblerkey.Text = "Trial version";
                    txtlicappkey.Visible = false;
                }
                else
                    lblerkey.Text = "Registration key:";
                txtlicappkey.Text = licno;
                txtlicappkey.ReadOnly = true;
                txtlicofflinekey.Text = licno;
                txtlicofflinekey.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("IsLicExists:" + ex.Message, Session.SessionID, "IsLicExists", "new", "true");
        }
    }

    protected void SetAxpertLogo()
    {
        main_body.Attributes.Add("Dir", direction);
        string folderPath = Server.MapPath("~/images/Custom");
        DirectoryInfo di = new DirectoryInfo(folderPath);
        FileInfo[] diFileinfo = di.GetFiles();
        var customlogoexist = "False";

        foreach (var drfile in diFileinfo)
        {
            if (drfile.Length > 0 && drfile.Name.Contains("homelogo"))
            {
                if (drfile.Name.Contains("mp4"))
                {
                    main_body.Attributes.CssStyle.Add("background", "");
                    customlogoexist = "True";
                    break;
                }
                else
                {
                    main_body.Attributes.CssStyle.Add("background", "url(./../images/Custom/" + drfile.Name + "?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ") no-repeat center center fixed !important ");
                    main_body.Attributes.CssStyle.Add("background-size", "cover !important");
                    main_body.Attributes.CssStyle.Add("height", "100vh !important");
                    customlogoexist = "True";
                    break;
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

    private static string GetServerLicDetails()
    {
        string licdetails = string.Empty;
        try
        {
            Util.Util objutil = new Util.Util();
            string redisIp = string.Empty;
            string redisPwd = string.Empty;
            //if (ConfigurationManager.AppSettings["axpLic_RedisIp"] != null)
            //    redisIp = ConfigurationManager.AppSettings["axpLic_RedisIp"].ToString();

            //if (ConfigurationManager.AppSettings["axpLic_RedisPass"] != null)
            //    redisPwd = ConfigurationManager.AppSettings["axpLic_RedisPass"].ToString();
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["axpLic_RedisIP"] != null && HttpContext.Current.Session["axpLic_RedisIP"].ToString() != "")
            {
                redisIp = HttpContext.Current.Session["axpLic_RedisIP"].ToString();
                if (HttpContext.Current.Session["axpLic_RedisPwd"] != null && HttpContext.Current.Session["axpLic_RedisPwd"].ToString() != "")
                    redisPwd = HttpContext.Current.Session["axpLic_RedisPwd"].ToString();
            }
            else
            {
                string rcDetails = objutil.GetAxpLicRedisConnDetails();
                if (rcDetails != "")
                {
                    redisIp = rcDetails.Split('♣')[0];
                    redisPwd = rcDetails.Split('♣')[1];
                }
            }

            if (redisIp != string.Empty)
            {
                string rlicConn = objutil.GetServerLicDetails(redisIp, redisPwd);
                switch (rlicConn)
                {
                    case "notConnected":
                        licdetails = "error:Redis Connection details for Axpert license is not proper. Please contact your support person.";
                        break;
                    case "keyNotExists":
                        licdetails = "error:Server seems to be not licensed. Please contact your support person.";
                        break;
                    case "keyNotMatch":
                        licdetails = "error:Redis IP for Axpert license should be set as 127.0.0.1. Please contact your support person.";
                        break;
                    case "keyExists":
                        if (redisPwd != string.Empty)
                            redisPwd = objutil.EncryptPWD(redisPwd);
                        licdetails = "lic_redis='" + redisIp + "~" + redisPwd + "'";
                        break;
                }
            }
            else
                licdetails = string.Empty;
        }
        catch (Exception ex)
        {
        }
        return licdetails;
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        int slIndx = lstconnection.SelectedIndex;
        if (slIndx != -1)
        {
            bool isConnDel = false;
            string selCon = lstconnection.Items[slIndx].Value;
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            FileInfo fi = new FileInfo(ScriptsPath + "\\axapps.xml");
            if (fi.Exists)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ScriptsPath + "\\axapps.xml");
                XmlNode node = doc.SelectSingleNode("/connections/" + selCon);
                node.ParentNode.RemoveChild(node);
                doc.Save(ScriptsPath + "\\axapps.xml");
                isConnDel = true;
            }

            string scriptsPathApp = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = scriptsPathApp;
            string directoryPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryPath))
            {
                string existingJsonApp = File.ReadAllText(filePath);
                JObject jsonApp = JObject.Parse(existingJsonApp);
                if (jsonApp["appconnections"] != null && jsonApp["appconnections"][selCon] != null && jsonApp["appconnections"][selCon].Type == JTokenType.Object)
                {
                    JObject projectObj = (JObject)jsonApp["appconnections"];
                    if (projectObj[selCon] != null)
                    {
                        projectObj.Property(selCon).Remove();
                        string modifiedJsonApp = jsonApp.ToString();
                        File.WriteAllText(filePath, modifiedJsonApp);
                    }
                }
            }
            if (isConnDel)
            {
                try
                {
                    FDW fdwObj = new FDW(selCon);
                    fdwObj.DeleteAllKeys(selCon + "-" + Constants.AXAPPS_XML_KEY, selCon);
                }
                catch (Exception ex) { }
                Response.Redirect("AxpertAdmin.aspx?auth=" + Request.QueryString["auth"].ToString());
            }
        }
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        string NewConName = string.Empty;
        try
        {
            string connectionType = ddlIsNewConnection.SelectedValue;
            NewConName = txtNewConName.Text;
            string dbtype = ddldbtype.Value;
            string dbverno = ddldbversion.Value;
            string driver = ddldriver.SelectedValue;
            string dbconName = txtccname.Text;
            string userName = txtusername.Text;
            string pwd = txtPassword.Text;
            txtPassword.Text = "";
            if (pwd == "")
                pwd = "log";
            pwd = util.EncryptPWD(pwd);
            string xml = "<" + NewConName + ">";
            xml += "<type>db</type>";
            xml += "<structurl/>";
            xml += "<db>" + dbtype + "</db>";
            xml += "<driver>" + driver + "</driver>";
            xml += "<version>" + dbverno + "</version>";
            xml += "<dbcon>" + dbconName + "</dbcon>";
            xml += "<dbuser>" + userName + "</dbuser>";
            xml += "<pwd>" + pwd + "</pwd>";
            xml += "<dataurl/>";
            xml += "</" + NewConName + ">";

            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            FileInfo fi = new FileInfo(ScriptsPath + "\\axapps.xml");
            if (fi.Exists)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlDocument docAxapp = new XmlDocument();
                docAxapp.Load(ScriptsPath + "\\axapps.xml");
                if (connectionType == "new")
                {
                    bool isConnExists = false;
                    XmlNodeList pNode = docAxapp.SelectNodes("/connections");
                    foreach (XmlNode nodeName in pNode)
                    {
                        foreach (XmlNode nodeN in nodeName.ChildNodes)
                        {
                            if (nodeN.Name.ToString().ToLower() == NewConName.ToLower())
                            {
                                isConnExists = true;
                                break;
                            }
                        }
                    }
                    if (isConnExists == false)
                    {
                        XmlNode newBook = docAxapp.ImportNode(doc.DocumentElement, true);
                        docAxapp.DocumentElement.AppendChild(newBook);
                        docAxapp.Save(ScriptsPath + "\\axapps.xml");

                        List<string> lst = new List<string>();
                        XmlDocument docList = new XmlDocument();
                        docList.Load(ScriptsPath + "\\axapps.xml");
                        try
                        {
                            XmlNodeList ConNode = docList.SelectSingleNode("/connections").ChildNodes;
                            foreach (XmlNode nodename in ConNode)
                            {
                                lst.Add(nodename.Name);
                            }
                        }
                        catch (Exception ex)
                        { }
                        jsonText = docAxapp.OuterXml;
                        jsonText = util.CheckSpecialChars(jsonText);

                        lstconnection.DataSource = lst;
                        lstconnection.DataBind();
                        selProj = NewConName;
                        ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SuccApplyConnection('success','" + NewConName + "');", true);
                    }
                    else
                        ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SuccApplyConnection('failed','" + NewConName + "');", true);
                }
                else
                {
                    XmlNode node = docAxapp.SelectSingleNode("/connections/" + NewConName);
                    node.ParentNode.RemoveChild(node);
                    //docAxapp.Save(ScriptsPath + "\\axapps.xml");

                    XmlNode newBook = docAxapp.ImportNode(doc.DocumentElement, true);
                    docAxapp.DocumentElement.AppendChild(newBook);
                    docAxapp.Save(ScriptsPath + "\\axapps.xml");

                    List<string> lst = new List<string>();
                    XmlDocument docList = new XmlDocument();
                    docList.Load(ScriptsPath + "\\axapps.xml");
                    try
                    {
                        XmlNodeList ConNode = docList.SelectSingleNode("/connections").ChildNodes;
                        foreach (XmlNode nodename in ConNode)
                        {
                            lst.Add(nodename.Name);
                        }
                    }
                    catch (Exception ex)
                    { }
                    jsonText = docAxapp.OuterXml;
                    jsonText = util.CheckSpecialChars(jsonText);

                    lstconnection.DataSource = lst;
                    lstconnection.DataBind();
                    selProj = NewConName;
                    ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SuccApplyConnection('success','" + NewConName + "');", true);
                }
            }
            else
            {
                xml = "<connections>" + xml + "</connections>";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                doc.Save(ScriptsPath + "/axapps.xml");

                List<string> lst = new List<string>();
                XmlDocument docList = new XmlDocument();
                docList.Load(ScriptsPath + "\\axapps.xml");
                try
                {
                    XmlNodeList pNode = docList.SelectSingleNode("/connections").ChildNodes;
                    foreach (XmlNode nodename in pNode)
                    {
                        lst.Add(nodename.Name);
                    }
                }
                catch (Exception ex)
                { }
                jsonText = doc.OuterXml;
                jsonText = util.CheckSpecialChars(jsonText);

                lstconnection.DataSource = lst;
                lstconnection.DataBind();
                selProj = NewConName;
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SuccApplyConnection('success','" + NewConName + "');", true);
            }

            var propertiesDict = new Dictionary<string, object>
            {
                { "type", "db" },
                { "structurl", "" },
                { "db", dbtype },
                { "driver", driver },
                { "version", dbverno },
                { "dbcon", dbconName },
                { "dbuser", userName },
                { "pwd", pwd },
                { "dataurl", "" }
            };
            var propData = new Dictionary<string, object> {
                { NewConName, propertiesDict }
            };
            var mainDict = new Dictionary<string, object>
            {
                { "appconnections", propData }
            };
            string jsonString = JsonConvert.SerializeObject(mainDict);
            string ScriptsPathApp = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = @" " + ScriptsPathApp + "";
            string directoryPath = Path.GetDirectoryName(filePath);
            FileInfo Filefi = new FileInfo(ScriptsPathApp);
            if (!Filefi.Exists)
            {
                File.WriteAllText(ScriptsPathApp, jsonString);
            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (existingJson != "")
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(jsonString);
                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    File.WriteAllText(filePath, json.ToString());
                }
            }
            try
            {
                FDW fdwObj = new FDW(NewConName);
                fdwObj.DeleteAllKeys(NewConName + "-" + Constants.AXAPPS_XML_KEY, NewConName);
            }
            catch (Exception ex) { }
            try
            {
                existingAppSettings = File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                existingAppSettings = "{}";
            }
            string _appsettings = util.GetAxAppSettings();
            if (!string.IsNullOrEmpty(_appsettings))
            {
                Application["AppSettingsIni"] = _appsettings;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Apply Connect Ok button:" + ex.Message, HttpContext.Current.Session.SessionID, "applyconnectok", "new", "true");
        }
    }

    [WebMethod]
    public static string AppTestConnection(string ddldbtype, string ddldbversion, string ddldriver, string txtccname, string txtusername, string txtPassword)
    {
        string result = string.Empty;
        Util.Util objutil = new Util.Util();
        try
        {
            string dbtype = ddldbtype;
            string dbverno = ddldbversion;
            string driver = ddldriver;
            string dbconName = txtccname;
            string userName = txtusername;
            string pwd = txtPassword;
            if (pwd == "")
                pwd = "log";
            pwd = objutil.EncryptPWD(pwd);

            string jsonData = "{";
            jsonData += "\"type\":\"db\"";
            jsonData += ",\"db\":\"" + dbtype + "\"";
            jsonData += ",\"version\":\"" + dbverno + "\"";
            jsonData += ",\"driver\":\"" + driver + "\"";
            jsonData += ",\"dbcon\":\"" + dbconName + "\"";
            //jsonData += ",\"structurl\":\"\"";
            //jsonData += ",\"dataurl\":\"\"";
            jsonData += ",\"dbuser\":\"" + userName.Replace(@"\", "\\\\") + "\"";
            jsonData += ",\"pwd\":\"" + pwd + "\"";
            jsonData += "}";
            string axpapp = userName;
            if (axpapp.Contains("\\"))
                axpapp = axpapp.Split('\\')[0];
            // GetDbConnection axapp licdetails 

            string lic_redis = string.Empty;
            string lic_file = string.Empty;
            string redisLicDetails = GetServerLicDetails();
            if (redisLicDetails != string.Empty && !redisLicDetails.StartsWith("error:"))
                lic_redis = redisLicDetails.Split('=')[1].ToString().Trim('\'');
            else
            {
                string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                string[] files = System.IO.Directory.GetFiles(ScriptsPath, "*.lic");
                if (files.Count() > 0)
                {
                    lic_file = files[0].Split('\\').Last();
                }
            }
            string trace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            string inputJson = "{\"_parameters\":[{\"getdbconnection\":{\"axpapp\":\"" + axpapp + "\",\"trace\":\"" + trace + "\"},\"" + axpapp + "\":" + jsonData + ",\"licdetails\":{\"lic_redis\":\"" + lic_redis + "\",\"lic_file\":\"" + lic_file + "\"}}]}";

            ASB.WebService objws = new ASB.WebService();
            result = objws.CallGetDBConnectionWS(inputJson);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Test Connection:" + ex.Message, HttpContext.Current.Session.SessionID, "AppTestConnection", "new", "true");
        }
        return result;
    }

    protected void btnActivate_Click(object sender, EventArgs e)
    {
        try
        {
            string userName = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).HostName;
            if (userName.IndexOf(".") > -1)
                userName = userName.Split('.')[0];
            string licappkey = txtlicappkey.Text;
            string trace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            string inputXMl = "<root username=\"" + userName + "\" licno=\"" + licappkey + "\" offline=\"no\" trace=\"" + trace + "\"></root>";
            //LogFile.Log logObj = new LogFile.Log();
            //logObj.CreateLog("Activate Input:" + inputXMl, HttpContext.Current.Session.SessionID, "ActivateInput", "new", "true");
            ASB.WebService objws = new ASB.WebService();
            string result = objws.CallActivateLicenseWS(inputXMl);
            if (result != "")
            {
                XmlDocument docXMl = new XmlDocument();
                docXMl.LoadXml(result);
                string strText = docXMl.LastChild.InnerText;
                string strType = docXMl.LastChild.Name == "result" ? "success" : docXMl.LastChild.Name;
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:LicActivationSucc('" + strType + "','" + strText.TrimEnd() + "');", true);
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:LicActivationSucc('error','Something went wrong please try again later..');", true);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Activate button:" + ex.Message, HttpContext.Current.Session.SessionID, "ActivateLic", "new", "true");
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            string userName = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).HostName;
            if (userName.IndexOf(".") > -1)
                userName = userName.Split('.')[0];
            string licappkey = txtlicappkey.Text;
            string trace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            string inputXMl = "<root username=\"" + userName + "\" licno=\"" + licappkey + "\" lictype=\"F\" offline=\"no\" trace=\"" + trace + "\"></root>";
            ASB.WebService objws = new ASB.WebService();
            string result = objws.CallActivateLicenseWS(inputXMl);
            if (result != "")
            {
                XmlDocument docXMl = new XmlDocument();
                docXMl.LoadXml(result);
                string strText = docXMl.LastChild.InnerText;
                string strType = docXMl.LastChild.Name == "result" ? "success" : docXMl.LastChild.Name;
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:LicActivationSucc('" + strType + "','" + strText.TrimEnd() + "');", true);
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:LicActivationSucc('error','Something went wrong please try again later..');", true);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Refresh button:" + ex.Message, HttpContext.Current.Session.SessionID, "RefreshLic", "new", "true");
        }
    }

    protected void btnTrial_Click(object sender, EventArgs e)
    {
        try
        {
            string userName = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).HostName;
            if (userName.IndexOf(".") > -1)
                userName = userName.Split('.')[0];
            string licappkey = txtlicappkey.Text;
            string trace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            string inputXMl = "<root username=\"" + userName + "\" orgname=\"from web site\" licno=\"trial\" lictype=\"E\" offline=\"no\" trace=\"" + trace + "\"></root>";
            ASB.WebService objws = new ASB.WebService();
            string result = objws.CallActivateLicenseWS(inputXMl);
            if (result != "")
            {
                XmlDocument docXMl = new XmlDocument();
                docXMl.LoadXml(result);
                string strText = docXMl.LastChild.InnerText;
                string strType = docXMl.LastChild.Name == "result" ? "success" : docXMl.LastChild.Name;
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:LicActivationSucc('" + strType + "','" + strText.TrimEnd() + "');", true);
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:LicActivationSucc('error','Something went wrong please try again later..');", true);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Activate trial button:" + ex.Message, HttpContext.Current.Session.SessionID, "ActivateTrialLic", "new", "true");
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            string userName = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).HostName;
            if (userName.IndexOf(".") > -1)
                userName = userName.Split('.')[0];
            string licappkey = txtlicofflinekey.Text;
            string trace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            string inputXMl = "<root username=\"" + userName + "\" licno=\"" + licappkey + "\" offline=\"yes\" trace=\"" + trace + "\"></root>";
            ASB.WebService objws = new ASB.WebService();
            string result = objws.CallActivateLicenseWS(inputXMl);
            if (result != "")
            {
                XmlDocument docXMl = new XmlDocument();
                docXMl.LoadXml(result);
                string strText = docXMl.LastChild.InnerText;
                string strType = docXMl.LastChild.Name == "result" ? "success" : docXMl.LastChild.Name;
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:LicDownloadSucc('" + strType + "','" + strText.TrimEnd() + "');", true);
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:LicDownloadSucc('error','Something went wrong please try again later..');", true);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("DownLoad button:" + ex.Message, HttpContext.Current.Session.SessionID, "DownloadReg", "new", "true");
        }
    }

    protected void btndownloadfile_Click(object sender, EventArgs e)
    {
        string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        string filePath = scriptsPath + "\\Axpert.reg";
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.WriteFile(file.FullName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();
        }
    }

    protected void btnFileUpload_Click(object sender, EventArgs e)
    {
        try
        {
            string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            HttpFileCollection httpAttFiles = Request.Files;
            for (int i = 0; i < httpAttFiles.Count; i++)
            {
                HttpPostedFile httpAttFile = httpAttFiles[i];
                if ((httpAttFile != null) && (httpAttFile.ContentLength > 0))
                {
                    string thisFileName = Path.GetFileName(httpAttFile.FileName);
                    httpAttFile.SaveAs(scriptsPath + thisFileName);
                    ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:LicActivationSucc('success','Lic file uploaded successfully.');", true);
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Upload button:" + ex.Message, HttpContext.Current.Session.SessionID, "UploadLic", "new", "true");
        }
    }

    protected void btndbpwb_Click(object sender, EventArgs e)
    {
        string result = string.Empty;
        Util.Util objutil = new Util.Util();
        try
        {
            string dbtype = ddldbtype.Value;
            string dbverno = ddldbversion.Value;
            string driver = ddldriver.SelectedValue;
            string dbconName = txtccname.Text;
            string userName = txtusername.Text;
            string pwd = txtPassword.Text;
            string newpwd = txtNewPassword.Text;
            txtPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            if (pwd == "")
                pwd = "log";
            pwd = objutil.EncryptPWD(pwd);

            newpwd = objutil.EncryptPWD(newpwd);

            string jsonData = "{";
            jsonData += "\"type\":\"db\"";
            jsonData += ",\"db\":\"" + dbtype + "\"";
            jsonData += ",\"version\":\"" + dbverno + "\"";
            jsonData += ",\"driver\":\"" + driver + "\"";
            jsonData += ",\"dbcon\":\"" + dbconName + "\"";
            //jsonData += ",\"structurl\":\"\"";
            //jsonData += ",\"dataurl\":\"\"";
            jsonData += ",\"dbuser\":\"" + userName.Replace(@"\", "\\\\") + "\"";
            jsonData += ",\"pwd\":\"" + pwd + "\"";
            jsonData += "}";
            string axpapp = userName;
            if (axpapp.Contains("\\"))
                axpapp = axpapp.Split('\\')[0];
            // GetDbConnection axapp licdetails 

            string lic_redis = string.Empty;
            string lic_file = string.Empty;
            string redisLicDetails = GetServerLicDetails();
            if (redisLicDetails != string.Empty && !redisLicDetails.StartsWith("error:"))
                lic_redis = redisLicDetails.Split('=')[1].ToString().Trim('\'');
            else
            {
                string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                string[] files = System.IO.Directory.GetFiles(ScriptsPath, "*.lic");
                if (files.Count() > 0)
                {
                    lic_file = files[0].Split('\\').Last();
                }
            }
            string trace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            string inputJson = "{\"_parameters\":[{\"setdbpassword\":{\"axpapp\":\"" + axpapp + "\",\"newpwd\":\"" + newpwd + "\",\"trace\":\"" + trace + "\"},\"" + axpapp + "\":" + jsonData + ",\"licdetails\":{\"lic_redis\":\"" + lic_redis + "\",\"lic_file\":\"" + lic_file + "\"}}]}";

            ASB.WebService objws = new ASB.WebService();
            string succMsg = objws.CallSetDBPasswordWS(inputJson);
            if (succMsg != "")
            {
                var details = JObject.Parse(succMsg);
                string changed = details["result"][0]["result"].ToString();
                if (changed == "true")
                    succMsg = "Password changed successfully.";
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SuccPasswordChange('success','" + succMsg + "');", true);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("change password button:" + ex.Message, HttpContext.Current.Session.SessionID, "ChangePasswordDb", "new", "true");
        }
    }

    [WebMethod]
    public static string UserAuthentication(string AuthUsername, string AuthPwd)
    {
        string result = string.Empty;
        try
        {
            if (AuthUsername != "" && AuthPwd != "")
            {
                Util.Util objutil = new Util.Util();
                string inifile = File.ReadAllText(HttpContext.Current.Server.MapPath("~/ConfigAuthentication.ini"));
                if (inifile != null && inifile != "")
                {
                    dynamic json = JsonConvert.DeserializeObject(inifile.Replace("\r\n", ""));
                    foreach (var chlAuthUser in json)
                    {
                        if (chlAuthUser.uname.Value == AuthUsername && chlAuthUser.pwd.Value == AuthPwd)
                        {
                            result = objutil.encrtptDecryptAES(HttpContext.Current.Session.SessionID);
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("User Authentication:" + ex.Message, HttpContext.Current.Session.SessionID, "UserAuthentication", "new", "true");
        }
        return result;
    }


    protected void btnRedisOk_Click(object sender, EventArgs e)
    {
        try
        {
            string connectionType = ddlIsRedisNewConnection.SelectedValue;
            string RConName = txtRedisNewConn.Text;
            string rHostName = txtrhotname.Text;
            string rPort = txtrport.Text;
            string rPwd = txtrpwd.Text;

            if (rHostName != "" && rHostName.ToLower() == "localhost")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SuccRedisConnection('error','Please use the proper IP Address for Redis Host.');", true);
                return;
            }

            if (rPwd != "")
                rPwd = util.EncryptPWD(rPwd);
            var propertiesDict = new Dictionary<string, object>
            {
                { "host", rHostName },
                { "port", rPort },
                { "pwd", rPwd }
            };
            var propData = new Dictionary<string, object> {
                { "Redis", propertiesDict }
            };
            var appDict = new Dictionary<string, object>
            {
                { RConName, propData }
            };
            var mainDict = new Dictionary<string, object>
            {
                { "appsettings", appDict }
            };
            string jsonString = JsonConvert.SerializeObject(mainDict);
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = @" " + ScriptsPath + "";
            string directoryPath = Path.GetDirectoryName(filePath);
            FileInfo Filefi = new FileInfo(ScriptsPath);
            if (!Filefi.Exists)
            {
                File.WriteAllText(ScriptsPath, jsonString);

            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (existingJson != "")
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(jsonString);
                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    File.WriteAllText(filePath, json.ToString());
                }
            }

            string scriptsPathredis = HttpContext.Current.Application["ScriptsPath"].ToString();
            string appSettingsPath = Path.Combine(scriptsPathredis, "AppSettings.ini");
            FileInfo fileInfo = new FileInfo(ScriptsPath);
            try
            {
                if (fileInfo.Exists)
                {
                    existingAppSettings = File.ReadAllText(appSettingsPath);
                }
                else
                    existingAppSettings = "{}";
            }
            catch (Exception ex)
            {
                existingAppSettings = "{}";
            }
            string _appsettings = util.GetAxAppSettings();
            if (!string.IsNullOrEmpty(_appsettings))
            {
                Application["AppSettingsIni"] = _appsettings;
            }

            ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SuccRedisConnection('success','" + RConName + "');", true);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Apply Connect Ok button:" + ex.Message, HttpContext.Current.Session.SessionID, "applyconnectRedisok", "new", "true");
        }
    }

    protected void btnRedisdelete_Click(object sender, EventArgs e)
    {
        int slIndx = lstRconnection.SelectedIndex;
        if (slIndx != -1)
        {
            string selCon = lstRconnection.Items[slIndx].Value;
            string scriptsPathRedis = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = scriptsPathRedis;
            string directoryPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryPath))
            {
                string existingJsonRedis = File.ReadAllText(filePath);
                JObject jsonRedis = JObject.Parse(existingJsonRedis);
                if (jsonRedis["appsettings"] != null && jsonRedis["appsettings"][selCon] != null && jsonRedis["appsettings"][selCon].Type == JTokenType.Object)
                {
                    JObject projectObj = (JObject)jsonRedis["appsettings"][selCon];
                    if (projectObj["Redis"] != null)
                    {
                        projectObj.Property("Redis").Remove();
                        string modifiedJsonFileConfig = jsonRedis.ToString();
                        File.WriteAllText(filePath, modifiedJsonFileConfig);
                    }
                }
                string _appsettings = util.GetAxAppSettings();
                if (!string.IsNullOrEmpty(_appsettings))
                {
                    Application["AppSettingsIni"] = _appsettings;
                }
                Response.Redirect("AxpertAdmin.aspx?auth=" + Request.QueryString["auth"].ToString());
            }
        }
    }

    public static ConnectionMultiplexer RedisConnect(string rhost, string rport, string rpwd, bool GetServerInfo = false)
    {
        try
        {
            string redisIP = rhost + ":" + rport;
            if (Rconfig == null)
            {
                HashSet<string> redisCommands = new HashSet<string>
                {
                    "CLUSTER",
                    "PING", "ECHO", "CLIENT",
                    "SUBSCRIBE", "UNSUBSCRIBE", "NULL"
                };

                if (!GetServerInfo)
                {
                    redisCommands.Add("INFO");
                    redisCommands.Add("CONFIG");
                }
                Rconfig = new ConfigurationOptions
                {
                    SyncTimeout = 10,//int.MaxValue,
                    KeepAlive = 10,
                    Password = rpwd,
                    AbortOnConnectFail = false,
                    AllowAdmin = true,
                    CommandMap = CommandMap.Create(redisCommands, available: false)
                };
                if (redisIP != "")
                {
                    foreach (var rIP in redisIP.Split(','))
                    {
                        Rconfig.EndPoints.Add(rIP);
                    }
                }
            }
            if (redisIP != "")
            {
                ConnectionMultiplexer rredis = ConnectionMultiplexer.Connect(Rconfig);
                return rredis;
            }
        }
        catch (Exception ex)
        {
            //schemaNameKey = string.Empty;
            //logObj.CreateLog("Redis Server Constructor(RedisServer), Message:" + ex.Message, GetSessionId(), "RedisServer", "new"); return null;
        }
        return null;
    }

    [WebMethod]
    public static string RedisTestConnection(string rHost, string rPort, string rPwd, string axwConn)
    {
        string result = string.Empty;
        try
        {
            if (rHost != "" && rHost.ToLower() == "localhost")
            {
                result = "false:Please use the proper IP Address for Redis Host.";
                return result;
            }
            if (axwConn == "")
            {
                result = "false:Please select Connection Name.";
                return result;
            }
            else
            {
                string awRport = string.Empty;
                string selCon = axwConn;
                string scriptsPathRedis = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
                string filePath = scriptsPathRedis;
                string directoryPath = Path.GetDirectoryName(filePath);
                if (Directory.Exists(directoryPath))
                {
                    string existingJsonRedis = File.ReadAllText(filePath);
                    JObject jsonRedis = JObject.Parse(existingJsonRedis);
                    if (jsonRedis["appsettings"] != null && jsonRedis["appsettings"][selCon] != null && jsonRedis["appsettings"][selCon].Type == JTokenType.Object)
                    {
                        JObject projectObj = (JObject)jsonRedis["appsettings"][selCon];
                        if (projectObj["AxStudioRedis"] != null)
                        {
                            awRport = jsonRedis["appsettings"][selCon]["AxStudioRedis"]["port"].ToString();
                        }
                    }
                }
                if (awRport != string.Empty && awRport == rPort)
                {
                    result = "false:Port should not be same as AxpertStudio Redis Connection port.";
                    return result;
                }
            }

            Rconfig = null;
            var rredis = RedisConnect(rHost, rPort, "");
            if (rredis != null && !rredis.IsConnected && rPwd != "")
            {
                RedisClose(rredis);
                Rconfig = null;
                var rredisnew = RedisConnect(rHost, rPort, rPwd);
                if (rredisnew != null && rredisnew.IsConnected)
                    result = "true:yes";
                else
                    result = "false";
            }
            else if (rredis != null && rredis.IsConnected)
            {
                result = "true:no";
            }
            else
                result = "false";
            RedisClose(rredis);
            Rconfig = null;
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Test Redis Connect button:" + ex.Message, HttpContext.Current.Session.SessionID, "RedisTestConnection", "new", "true");
        }
        return result;
    }

    private static void RedisClose(ConnectionMultiplexer redis)
    {
        if (redis != null)
            redis.Close(false);
    }
    [WebMethod]
    public static string ARMConnection(string aKey, string aUrl, string aScriptsUrl, string aPeg, string proj, string aExpiryMinutes, string aClientSSO, string aNotificationURL, string aNotificationExpiryHours, string aNotificationMaxPerUser)
    {
        string result = "";
        try
        {
            string project = proj;
            string webScriptsPath = "";
            if (ConfigurationManager.AppSettings["ScriptsPath"] != null && ConfigurationManager.AppSettings["ScriptsPath"].ToString() != string.Empty)
                webScriptsPath = ConfigurationManager.AppSettings["ScriptsPath"].ToString();
            string scriptsUrlPath = "";
            if (ConfigurationManager.AppSettings["scriptsUrlPath"] != null && ConfigurationManager.AppSettings["scriptsUrlPath"].ToString() != string.Empty)
                scriptsUrlPath = ConfigurationManager.AppSettings["scriptsUrlPath"].ToString();
            string aRMQapiUrl = string.Empty, aSignalRapiUrl = string.Empty, AxFCMSendMsgURL = string.Empty, AxRapidSaveURL = string.Empty, axpegemailactionurl = string.Empty, AxScriptsAPIURL = string.Empty;
            aUrl = aUrl.TrimEnd();
            aNotificationURL = aNotificationURL.TrimEnd();

            if (aNotificationURL.EndsWith("/"))
                aNotificationURL = aNotificationURL.Substring(0, aNotificationURL.Length - 1);

            if (aUrl.EndsWith("/"))
                aUrl = aUrl.Substring(0, aUrl.Length - 1);

            aScriptsUrl = aScriptsUrl.TrimEnd();
            string armaUrl = aUrl.TrimEnd();
            string armaScriptsUrl = aScriptsUrl.TrimEnd();
            if (!armaUrl.EndsWith("/"))
                armaUrl = armaUrl + "/";
            if (!armaScriptsUrl.EndsWith("/"))
                armaScriptsUrl = armaScriptsUrl + "/";
            aRMQapiUrl = armaUrl + "api/v1/ARMPushToQueue";
            aSignalRapiUrl = aNotificationURL + "/api/v1/SendSignalR";
            AxFCMSendMsgURL = armaUrl + "api/v1/SendFCMNotification";
            AxRapidSaveURL = armaScriptsUrl + "ASBRapidSaveRest.dll/datasnap/rest/TASBRapidSaveRest/RapidSave";
            axpegemailactionurl = armaUrl + "api/v1/ARMMailTaskAction";
            AxScriptsAPIURL = armaScriptsUrl + "ASBScriptRest.dll/datasnap/rest/TASBScriptRest/scriptsapi";

            var propertiesDict = new Dictionary<string, object>
            {
                { "ARM_PrivateKey", aKey },
                { "ARM_URL", aUrl },
                { "ARM_Scripts_URL", aScriptsUrl },
                { "PEG", aPeg },
                {"ScriptsPath",webScriptsPath },
                {"scriptsUrlPath",scriptsUrlPath },
                {"ExpiryMinutes",aExpiryMinutes },
                {"ClientSSO",aClientSSO },

                {"ARM_Notification_URL",aNotificationURL },
                {"ARM_Notification_ExpiryHours",aNotificationExpiryHours },
                {"ARM_Notification_MaxPerUser",aNotificationMaxPerUser }
            };
            var propData = new Dictionary<string, object> {
                { "ARM", propertiesDict }
            };
            var appDict = new Dictionary<string, object>
            {
                { project, propData }
            };
            var mainDict = new Dictionary<string, object>
            {
                { "appsettings", appDict }
            };
            string jsonString = JsonConvert.SerializeObject(mainDict);
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = @" " + ScriptsPath + "";
            string directoryPath = Path.GetDirectoryName(filePath);
            FileInfo Filefi = new FileInfo(ScriptsPath);
            if (!Filefi.Exists)
            {
                File.WriteAllText(ScriptsPath, jsonString);

            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (existingJson != "")
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(jsonString);
                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    File.WriteAllText(filePath, json.ToString());
                }
            }


            var internalResources = new Dictionary<string, object>
            {
                {"AxRMQAPIURL",aRMQapiUrl },
                {"AxSignalRapiURL",aSignalRapiUrl },
                {"AxFCMSendMsgURL",AxFCMSendMsgURL },
                {"AxRapidSaveURL",AxRapidSaveURL },
                {"axpegemailactionurl",axpegemailactionurl },
                {"AxScriptsAPIURL",AxScriptsAPIURL }
            };
            var internalResourcesData = new Dictionary<string, object> {
                { "InternalResources", internalResources }
            };
            var internalResourcesDict = new Dictionary<string, object>
            {
                { project, internalResourcesData }
            };
            var internalResourcesmainDict = new Dictionary<string, object>
            {
                { "appsettings", internalResourcesDict }
            };
            string _jsonString = JsonConvert.SerializeObject(internalResourcesmainDict);
            if (!Filefi.Exists)
            {
                File.WriteAllText(ScriptsPath, _jsonString);
            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (existingJson != "")
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(_jsonString);
                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    File.WriteAllText(filePath, json.ToString());
                }
            }

            Util.Util uti = new Util.Util();
            string _appsettings = uti.GetAxAppSettings();
            if (!string.IsNullOrEmpty(_appsettings))
            {
                HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
            }
        }
        catch (Exception ex)
        {
        }

        try
        {
            FDW fdwObj = new FDW(proj);
            fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_CONN_KEY, proj);
            fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_IntRes_CONN_KEY, proj);
        }
        catch (Exception ex) { }
        string ScriptsPathARM = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePatharm = @" " + ScriptsPathARM + "";
        string directoryPatharm = Path.GetDirectoryName(filePatharm);
        FileInfo Filefiarm = new FileInfo(ScriptsPathARM);
        try
        {
            if (Filefiarm.Exists)
            {
                string existingJsonARM = File.ReadAllText(filePatharm);
                existingJsonARM = JsonConvert.SerializeObject(existingJsonARM);
                result = existingJsonARM;
            }

        }
        catch (Exception ex) { }
        return result;
    }


    [WebMethod]
    public static string DelARMConnectionWs(string proj)
    {
        string result = "";
        try
        {
            string scriptsPathARM = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = scriptsPathARM;
            string directoryPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryPath))
            {
                string existingJsonARM = File.ReadAllText(filePath);
                JObject jsonARM = JObject.Parse(existingJsonARM);
                if (jsonARM["appsettings"] != null && jsonARM["appsettings"][proj] != null && jsonARM["appsettings"][proj].Type == JTokenType.Object)
                {
                    JObject projectObj = (JObject)jsonARM["appsettings"][proj];
                    if (projectObj["ARM"] != null)
                    {
                        projectObj.Property("ARM").Remove();
                        string modifiedJsonARM = jsonARM.ToString();
                        File.WriteAllText(filePath, modifiedJsonARM);
                    }
                }
                Util.Util uti = new Util.Util();
                string _appsettings = uti.GetAxAppSettings();
                if (!string.IsNullOrEmpty(_appsettings))
                {
                    HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
                }
            }
        }
        catch (Exception ex)
        {

        }
        try
        {
            FDW fdwObj = new FDW(proj);
            fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_CONN_KEY, proj);
            fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_IntRes_CONN_KEY, proj);
        }
        catch (Exception ex) { }
        string ScriptsPathARMdel = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePatharmdel = @" " + ScriptsPathARMdel + "";
        string directoryPatharmdel = Path.GetDirectoryName(filePatharmdel);
        FileInfo Filefiarmdel = new FileInfo(ScriptsPathARMdel);
        try
        {
            if (Filefiarmdel.Exists)
            {
                string existingJsonARM = File.ReadAllText(filePatharmdel);
                existingJsonARM = JsonConvert.SerializeObject(existingJsonARM);
                result = existingJsonARM;
            }

        }
        catch (Exception ex) { }
        return result;
    }

    [WebMethod]
    public static string FileConnection(string fUpload, string fDownload, string proj, string fMapUsername, string fMapPwd)
    {
        string result = "";
        try
        {
            string fileUploadPath = fUpload;
            if (fileUploadPath != "" && !fileUploadPath.EndsWith("\\"))
                fileUploadPath += "\\";
            string fileDownloadPath = fDownload;
            if (fileDownloadPath != "" && !fileDownloadPath.EndsWith("\\"))
                fileDownloadPath += "\\";
            string project = proj;
            var propertiesDict = new Dictionary<string, object>
            {
                { "FileUploadPath", fileUploadPath },
                { "FileDownloadPath", fileDownloadPath },
                { "FileServerMapUsername", fMapUsername },
                { "FileServerMapPwd", fMapPwd }
            };
            var propData = new Dictionary<string, object> {
                { "FileConfig", propertiesDict }
            };
            var appDict = new Dictionary<string, object>
            {
                { project, propData }
            };
            var mainDict = new Dictionary<string, object>
            {
                { "appsettings", appDict }
            };
            string jsonString = JsonConvert.SerializeObject(mainDict);
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = @" " + ScriptsPath + "";
            string directoryPath = Path.GetDirectoryName(filePath);
            FileInfo Filefi = new FileInfo(ScriptsPath);
            if (!Filefi.Exists)
            {
                File.WriteAllText(ScriptsPath, jsonString);

            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (existingJson != "")
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(jsonString);
                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    File.WriteAllText(filePath, json.ToString());
                }
            }
            Util.Util uti = new Util.Util();
            string _appsettings = uti.GetAxAppSettings();
            if (!string.IsNullOrEmpty(_appsettings))
            {
                HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
            }
        }
        catch (Exception ex)
        {

        }
        try
        {
            FDW fdwObj = new FDW(proj);
            fdwObj.DeleteAllKeys(proj + "-" + Constants.AXFileServer_CONN_KEY, proj);
        }
        catch (Exception ex) { }
        string ScriptsPathfile = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePathfile = @" " + ScriptsPathfile + "";
        string directoryPatharm = Path.GetDirectoryName(filePathfile);
        FileInfo Filefifilee = new FileInfo(ScriptsPathfile);
        try
        {
            if (Filefifilee.Exists)
            {
                string existingJsonFile = File.ReadAllText(filePathfile);
                existingJsonFile = JsonConvert.SerializeObject(existingJsonFile);
                result = existingJsonFile;
            }

        }
        catch (Exception ex) { }
        return result;
    }

    [WebMethod]
    public static string DelFileConnectionWs(string proj)
    {
        string result = "";
        try
        {
            string ScriptsPathFile = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = ScriptsPathFile;
            string directoryPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryPath))
            {
                string existingJsonFileConfig = File.ReadAllText(filePath);
                JObject jsonFileConfig = JObject.Parse(existingJsonFileConfig);
                if (jsonFileConfig["appsettings"] != null && jsonFileConfig["appsettings"][proj] != null && jsonFileConfig["appsettings"][proj].Type == JTokenType.Object)
                {
                    JObject projectObj = (JObject)jsonFileConfig["appsettings"][proj];
                    if (projectObj["FileConfig"] != null)
                    {
                        projectObj.Property("FileConfig").Remove();
                        string modifiedJsonFileConfig = jsonFileConfig.ToString();
                        File.WriteAllText(filePath, modifiedJsonFileConfig);
                    }
                }
                Util.Util uti = new Util.Util();
                string _appsettings = uti.GetAxAppSettings();
                if (!string.IsNullOrEmpty(_appsettings))
                {
                    HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
                }
            }
        }
        catch (Exception ex)
        {

        }
        try
        {
            FDW fdwObj = new FDW(proj);
            fdwObj.DeleteAllKeys(proj + "-" + Constants.AXFileServer_CONN_KEY, proj);
        }
        catch (Exception ex) { }

        string ScriptsPathfiledel = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePathfiledel = @" " + ScriptsPathfiledel + "";
        string directoryPatharmdel = Path.GetDirectoryName(filePathfiledel);
        FileInfo Filefifileedel = new FileInfo(ScriptsPathfiledel);
        try
        {
            if (Filefifileedel.Exists)
            {
                string existingJsonFile = File.ReadAllText(filePathfiledel);
                existingJsonFile = JsonConvert.SerializeObject(existingJsonFile);
                result = existingJsonFile;
            }

        }
        catch (Exception ex) { }
        return result;
    }

    [WebMethod]
    public static string SaveSSOConnection(object requestJson, string ssoType, string ssoProj)
    {
        string result = "";
        try
        {
            if (requestJson != null && !string.IsNullOrEmpty(ssoType) && !string.IsNullOrEmpty(ssoProj))
            {
                JObject newSSOData = JObject.FromObject(requestJson);
                string scriptsPathSSo = HttpContext.Current.Application["ScriptsPath"].ToString() + "ssoinfoconfig.ini";
                string _filePathSSo = Path.Combine(scriptsPathSSo);
                JObject existingData = new JObject();
                if (File.Exists(_filePathSSo))
                {
                    string existingJsonSSo = File.ReadAllText(_filePathSSo);
                    if (!string.IsNullOrWhiteSpace(existingJsonSSo))
                    {
                        existingData = JsonConvert.DeserializeObject<JObject>(existingJsonSSo) ?? new JObject();
                    }
                }
                if (existingData[ssoProj] == null || !existingData[ssoProj].HasValues)
                {
                    existingData[ssoProj] = new JObject();
                }
            ((JObject)existingData[ssoProj])[ssoType] = newSSOData;
                string updatedJson = JsonConvert.SerializeObject(existingData, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(_filePathSSo, updatedJson);
            }
        }
        catch (Exception ex)
        {

        }
        try
        {
            FDW fdwObj = new FDW(ssoProj);
            fdwObj.DeleteAllKeys(ssoProj + "-" + Constants.AXSSO_CONN_KEY, ssoProj);
        }
        catch (Exception ex) { }
        string ScriptsPathSSo = HttpContext.Current.Application["ScriptsPath"].ToString() + "ssoinfoconfig.ini";
        string filePathSSo = @" " + ScriptsPathSSo + "";
        FileInfo FileSSOfile = new FileInfo(ScriptsPathSSo);
        try
        {
            if (FileSSOfile.Exists)
            {
                string existingJsonSSo = File.ReadAllText(filePathSSo);
                existingJsonSSo = JsonConvert.SerializeObject(existingJsonSSo);
                result = existingJsonSSo;
            }
        }
        catch (Exception ex) { }
        return result;
    }

    [WebMethod]
    public static string SaveUpdateSSOConnection(object requestJson, string ssoType, string ssoProj, object updatereqJson, string updateSsoType)
    {
        string result = "";
        try
        {
            if (requestJson != null && !string.IsNullOrEmpty(ssoType) && !string.IsNullOrEmpty(ssoProj))
            {
                JObject newSSOData = JObject.FromObject(requestJson);
                string scriptsPathSSo = HttpContext.Current.Application["ScriptsPath"].ToString() + "ssoinfoconfig.ini";
                string _filePathSSo = Path.Combine(scriptsPathSSo);
                JObject existingData = new JObject();
                if (File.Exists(_filePathSSo))
                {
                    string existingJsonSSo = File.ReadAllText(_filePathSSo);
                    if (!string.IsNullOrWhiteSpace(existingJsonSSo))
                    {
                        existingData = JsonConvert.DeserializeObject<JObject>(existingJsonSSo) ?? new JObject();
                    }
                }
                if (existingData[ssoProj] == null || !existingData[ssoProj].HasValues)
                {
                    existingData[ssoProj] = new JObject();
                }
            ((JObject)existingData[ssoProj])[ssoType] = newSSOData;
                string updatedJson = JsonConvert.SerializeObject(existingData, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(_filePathSSo, updatedJson);

                if (updatereqJson != null && updateSsoType != string.Empty)
                {
                    JObject updateSSOData = JObject.FromObject(updatereqJson);
                    var ssoProp = updateSSOData.Properties().FirstOrDefault();
                    if (ssoProp != null)
                    {
                        string dynamicType = ssoProp.Name;
                        JObject updateSsoValue = (JObject)ssoProp.Value;
                        ((JObject)existingData[ssoProj])[dynamicType] = updateSsoValue;
                    }

                    string _updatedJson = JsonConvert.SerializeObject(existingData, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(_filePathSSo, _updatedJson);
                }
            }
        }
        catch (Exception ex)
        {

        }
        try
        {
            FDW fdwObj = new FDW(ssoProj);
            fdwObj.DeleteAllKeys(ssoProj + "-" + Constants.AXSSO_CONN_KEY, ssoProj);
        }
        catch (Exception ex) { }
        string ScriptsPathSSo = HttpContext.Current.Application["ScriptsPath"].ToString() + "ssoinfoconfig.ini";
        string filePathSSo = @" " + ScriptsPathSSo + "";
        FileInfo FileSSOfile = new FileInfo(ScriptsPathSSo);
        try
        {
            if (FileSSOfile.Exists)
            {
                string existingJsonSSo = File.ReadAllText(filePathSSo);
                existingJsonSSo = JsonConvert.SerializeObject(existingJsonSSo);
                result = existingJsonSSo;
            }
        }
        catch (Exception ex) { }
        return result;
    }

    [WebMethod]
    public static string SSoConDelete(string ssoProj)
    {
        string scriptsPathSSo = HttpContext.Current.Application["ScriptsPath"].ToString() + "ssoinfoconfig.ini";
        string filePathSSo = Path.Combine(scriptsPathSSo);

        if (File.Exists(filePathSSo))
        {
            string existingJson = File.ReadAllText(filePathSSo);
            JObject existingData = JsonConvert.DeserializeObject<JObject>(existingJson) ?? new JObject();
            if (existingData[ssoProj] != null)
            {
                existingData.Remove(ssoProj);
                File.WriteAllText(filePathSSo, JsonConvert.SerializeObject(existingData, Newtonsoft.Json.Formatting.Indented));
                try
                {
                    FDW fdwObj = new FDW(ssoProj);
                    fdwObj.DeleteAllKeys(ssoProj + "-" + Constants.AXSSO_CONN_KEY, ssoProj);
                }
                catch (Exception ex) { }
                return "deleted";
            }
            else
                return "notdeleted";
        }
        else
            return "notdeleted";
    }
    [WebMethod]
    public static string SSoTypeDelete(string ssoProj, string ssoType)
    {
        string scriptsPathSSo = HttpContext.Current.Application["ScriptsPath"].ToString() + "ssoinfoconfig.ini";
        string filePathSSo = Path.Combine(scriptsPathSSo);
        if (File.Exists(filePathSSo))
        {
            string existingJson = File.ReadAllText(filePathSSo);
            JObject existingData = JsonConvert.DeserializeObject<JObject>(existingJson) ?? new JObject();
            if (existingData[ssoProj] != null)
            {
                JObject projectData = (JObject)existingData[ssoProj];
                if (projectData[ssoType] != null)
                {
                    projectData.Remove(ssoType);
                    File.WriteAllText(filePathSSo, JsonConvert.SerializeObject(existingData, Newtonsoft.Json.Formatting.Indented));

                    try
                    {
                        FDW fdwObj = new FDW(ssoProj);
                        fdwObj.DeleteAllKeys(ssoProj + "-" + Constants.AXSSO_CONN_KEY, ssoProj);
                    }
                    catch (Exception ex) { }
                    return "deleted";
                }
                else
                    return "notdeleted";
            }
            else
                return "notdeleted";
        }
        else
            return "notdeleted";
    }


    [WebMethod]
    public static string LicDomainConnection(string domainName, string proj)
    {
        string result = "";
        try
        {
            string _domainName = domainName;
            string project = proj;
            var propertiesDict = new Dictionary<string, object>
            {
                { "domainname", domainName }
            };
            var propData = new Dictionary<string, object> {
                { "licdomain", propertiesDict }
            };
            var appDict = new Dictionary<string, object>
            {
                { project, propData }
            };
            var mainDict = new Dictionary<string, object>
            {
                { "appsettings", appDict }
            };
            string jsonString = JsonConvert.SerializeObject(mainDict);
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = @" " + ScriptsPath + "";
            string directoryPath = Path.GetDirectoryName(filePath);
            FileInfo Filefi = new FileInfo(ScriptsPath);
            if (!Filefi.Exists)
            {
                File.WriteAllText(ScriptsPath, jsonString);

            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (existingJson != "")
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(jsonString);
                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    File.WriteAllText(filePath, json.ToString());
                }
            }
            Util.Util uti = new Util.Util();
            string _appsettings = uti.GetAxAppSettings();
            if (!string.IsNullOrEmpty(_appsettings))
            {
                HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
            }
        }
        catch (Exception ex)
        {
        }

        string ScriptsPathLic = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePathLic = @" " + ScriptsPathLic + "";
        FileInfo Filefiarm = new FileInfo(ScriptsPathLic);
        try
        {
            if (Filefiarm.Exists)
            {
                string existingJsonLic = File.ReadAllText(filePathLic);
                existingJsonLic = JsonConvert.SerializeObject(existingJsonLic);
                result = existingJsonLic;
            }

        }
        catch (Exception ex) { }
        return result;
    }


    [WebMethod]
    public static string DelLicDomainConnection(string proj)
    {
        string result = "";
        try
        {
            string scriptsPathLic = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = scriptsPathLic;
            string directoryPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryPath))
            {
                string existingJsonLic = File.ReadAllText(filePath);
                JObject jsonLic = JObject.Parse(existingJsonLic);
                if (jsonLic["appsettings"] != null && jsonLic["appsettings"][proj] != null && jsonLic["appsettings"][proj].Type == JTokenType.Object)
                {
                    JObject projectObj = (JObject)jsonLic["appsettings"][proj];
                    if (projectObj["licdomain"] != null)
                    {
                        projectObj.Property("licdomain").Remove();
                        string modifiedJsonARM = jsonLic.ToString();
                        File.WriteAllText(filePath, modifiedJsonARM);
                    }
                }
                Util.Util uti = new Util.Util();
                string _appsettings = uti.GetAxAppSettings();
                if (!string.IsNullOrEmpty(_appsettings))
                {
                    HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
                }
            }
        }
        catch (Exception ex)
        {

        }
        string ScriptsPathLicdel = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePathLicdel = @" " + ScriptsPathLicdel + "";
        FileInfo FilefiLicdel = new FileInfo(ScriptsPathLicdel);
        try
        {
            if (FilefiLicdel.Exists)
            {
                string existingJsonLic = File.ReadAllText(filePathLicdel);
                existingJsonLic = JsonConvert.SerializeObject(existingJsonLic);
                result = existingJsonLic;
            }

        }
        catch (Exception ex) { }
        return result;
    }

    protected void btnRedisdeleteStudio_Click(object sender, EventArgs e)
    {
        int slIndx = lstStudioRconnection.SelectedIndex;
        if (slIndx != -1)
        {
            string selCon = lstStudioRconnection.Items[slIndx].Value;
            string scriptsPathRedis = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = scriptsPathRedis;
            string directoryPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryPath))
            {
                string existingJsonRedis = File.ReadAllText(filePath);
                JObject jsonRedis = JObject.Parse(existingJsonRedis);
                if (jsonRedis["appsettings"] != null && jsonRedis["appsettings"][selCon] != null && jsonRedis["appsettings"][selCon].Type == JTokenType.Object)
                {
                    JObject projectObj = (JObject)jsonRedis["appsettings"][selCon];
                    if (projectObj["AxStudioRedis"] != null)
                    {
                        projectObj.Property("AxStudioRedis").Remove();
                        string modifiedJsonFileConfig = jsonRedis.ToString();
                        File.WriteAllText(filePath, modifiedJsonFileConfig);
                    }
                }
                string _appsettings = util.GetAxAppSettings();
                if (!string.IsNullOrEmpty(_appsettings))
                {
                    Application["AppSettingsIni"] = _appsettings;
                }
                Response.Redirect("AxpertAdmin.aspx?auth=" + Request.QueryString["auth"].ToString());
            }
        }
    }

    protected void btnRedisOkStudio_Click(object sender, EventArgs e)
    {
        try
        {
            string connectionType = ddlIsRedisNewConnectionStudio.SelectedValue;
            string RConName = txtRedisNewConnStudio.Text;
            string rHostName = txtrhotnameStudio.Text;
            string rPort = txtrportStudio.Text;
            string rPwd = txtrpwdStudio.Text;
            string strudioUrl = txtStudioUrl.Text;
            string studioScriptPath = txtStudioScriptPath.Text;
            string studioURLPath = txtStudioURLPath.Text;

            if (rHostName != "" && rHostName.ToLower() == "localhost")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SuccRedisConnection('error','Please use the proper IP Address for Redis Host.');", true);
                return;
            }

            if (rPwd != "")
                rPwd = util.EncryptPWD(rPwd);
            var propertiesDict = new Dictionary<string, object>
            {
                { "host", rHostName },
                { "port", rPort },
                { "pwd", rPwd },
                { "studiourl",strudioUrl},
                { "studioScriptPath",studioScriptPath},
                { "studioURLPath",studioURLPath}
            };
            var propData = new Dictionary<string, object> {
                { "AxStudioRedis", propertiesDict }
            };
            var appDict = new Dictionary<string, object>
            {
                { RConName, propData }
            };
            var mainDict = new Dictionary<string, object>
            {
                { "appsettings", appDict }
            };
            string jsonString = JsonConvert.SerializeObject(mainDict);
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = @" " + ScriptsPath + "";
            string directoryPath = Path.GetDirectoryName(filePath);
            FileInfo Filefi = new FileInfo(ScriptsPath);
            if (!Filefi.Exists)
            {
                File.WriteAllText(ScriptsPath, jsonString);

            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (existingJson != "")
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(jsonString);
                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    File.WriteAllText(filePath, json.ToString());
                }
            }

            try
            {
                string ScriptsPathJson = HttpContext.Current.Application["ScriptsPath"].ToString() + "appsettings.json";
                string filePathJson = @" " + ScriptsPathJson + "";
                studioScriptPath = studioScriptPath.Replace(@"\", "\\\\");
                if (!studioURLPath.EndsWith("/"))
                    studioURLPath += "/";
                string _Json = "{\"Logging\":{\"LogLevel\":{\"Default\":\"Information\",\"Microsoft.AspNetCore\":\"Warning\"}},\"AppSettings\":{\"ScriptsPath\":\"" + studioScriptPath + "\",\"ScriptURLPath\":\"" + studioURLPath + "\",\"ASBIView.ASBIViewservice\":\"" + studioURLPath + "ASBIView.dll/soap/ASBIView\",\"ASBAction.ASBActionservice\":\"" + studioURLPath + "ASBAction.dll/soap/ASBAction\",\"ASBDefine.ASBDefineservice\":\"" + studioURLPath + "ASBDefine.dll/soap/Asbdefine\"},\"AllowedHosts\":\"*\"}";
                JObject _Ojson = JObject.Parse(_Json);
                File.WriteAllText(filePathJson, _Ojson.ToString());
            }
            catch (Exception ex) { }


            string scriptsPathredis = HttpContext.Current.Application["ScriptsPath"].ToString();
            string appSettingsPath = Path.Combine(scriptsPathredis, "AppSettings.ini");
            FileInfo fileInfo = new FileInfo(ScriptsPath);
            try
            {
                if (fileInfo.Exists)
                {
                    existingAppSettings = File.ReadAllText(appSettingsPath);
                }
                else
                    existingAppSettings = "{}";
            }
            catch (Exception ex)
            {
                existingAppSettings = "{}";
            }
            string _appsettings = util.GetAxAppSettings();
            if (!string.IsNullOrEmpty(_appsettings))
            {
                Application["AppSettingsIni"] = _appsettings;
            }


            strudioScriptUrlMesg = strudioUrl;

            ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:SuccStudioRedisConnection('success','" + strudioUrl + "');", true);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Apply Connect Ok button:" + ex.Message, HttpContext.Current.Session.SessionID, "applyconnectRedisokStudio", "new", "true");
        }
    }

    [WebMethod]
    public static string RedisTestConnectionStudio(string rHost, string rPort, string rPwd, string axsConn, string studioUrl)
    {
        string result = string.Empty;
        try
        {
            if (rHost != "" && rHost.ToLower() == "localhost")
            {
                result = "false:Please use the proper IP Address for Redis Host.";
                return result;
            }

            if (axsConn == "")
            {
                result = "false:Please select Connection Name.";
                return result;
            }
            else
            {
                string awRport = string.Empty;
                string selCon = axsConn;
                string scriptsPathRedis = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
                string filePath = scriptsPathRedis;
                string directoryPath = Path.GetDirectoryName(filePath);
                if (Directory.Exists(directoryPath))
                {
                    string existingJsonRedis = File.ReadAllText(filePath);
                    JObject jsonRedis = JObject.Parse(existingJsonRedis);
                    if (jsonRedis["appsettings"] != null && jsonRedis["appsettings"][selCon] != null && jsonRedis["appsettings"][selCon].Type == JTokenType.Object)
                    {
                        JObject projectObj = (JObject)jsonRedis["appsettings"][selCon];
                        if (projectObj["Redis"] != null)
                        {
                            awRport = jsonRedis["appsettings"][selCon]["Redis"]["port"].ToString();
                        }
                    }
                }
                if (awRport != string.Empty && awRport == rPort)
                {
                    result = "false:Port should not be same as AxpertWeb Redis Connection port.";
                    return result;
                }
            }

            Rconfig = null;
            var rredis = RedisConnect(rHost, rPort, "");
            if (rredis != null && !rredis.IsConnected && rPwd != "")
            {
                RedisClose(rredis);
                Rconfig = null;
                var rredisnew = RedisConnect(rHost, rPort, rPwd);
                if (rredisnew != null && rredisnew.IsConnected)
                    result = "true:yes";
                else
                    result = "false";
            }
            else if (rredis != null && rredis.IsConnected)
            {
                result = "true:no";
            }
            else
                result = "false";
            RedisClose(rredis);
            Rconfig = null;
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Test Redis Connect button:" + ex.Message, HttpContext.Current.Session.SessionID, "RedisTestConnection", "new", "true");
        }
        return result;
    }


    [WebMethod]
    public static string RMQueueConnection(string aRMQHost, string aRMQPort, string aRMQUser, string aRMQPwd)
    {
        string result = "";
        try
        {
            var propertiesDict = new Dictionary<string, object>
            {
                {"RMQueueHost",aRMQHost },
                {"RMQueuePort",aRMQPort },
                {"RMQueueUser",aRMQUser },
                {"RMQueuePassword",aRMQPwd }
            };
            var mainDict = new Dictionary<string, object>
            {
                { "rmqsettings", propertiesDict }
            };
            string jsonString = JsonConvert.SerializeObject(mainDict);
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = @" " + ScriptsPath + "";
            string directoryPath = Path.GetDirectoryName(filePath);
            FileInfo Filefi = new FileInfo(ScriptsPath);
            if (!Filefi.Exists)
            {
                File.WriteAllText(ScriptsPath, jsonString);

            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (existingJson != "")
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(jsonString);
                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    File.WriteAllText(filePath, json.ToString());
                }
            }
            Util.Util uti = new Util.Util();
            string _appsettings = uti.GetAxAppSettings();
            if (!string.IsNullOrEmpty(_appsettings))
            {
                HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
            }
        }
        catch (Exception ex)
        {
        }

        string ScriptsPathARM = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePatharm = @" " + ScriptsPathARM + "";
        string directoryPatharm = Path.GetDirectoryName(filePatharm);
        FileInfo Filefiarm = new FileInfo(ScriptsPathARM);
        try
        {
            if (Filefiarm.Exists)
            {
                string existingJsonARM = File.ReadAllText(filePatharm);
                existingJsonARM = JsonConvert.SerializeObject(existingJsonARM);
                result = existingJsonARM;
            }
        }
        catch (Exception ex) { }
        return result;
    }


    [WebMethod]
    public static string DelRMQueueConnectionWs()
    {
        string result = "";
        try
        {
            string scriptsPathARM = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = scriptsPathARM;
            string directoryPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryPath))
            {
                string existingJsonARM = File.ReadAllText(filePath);
                JObject jsonARM = JObject.Parse(existingJsonARM);
                if (jsonARM["rmqsettings"] != null)
                {
                    jsonARM.Remove("rmqsettings");
                    File.WriteAllText(filePath, jsonARM.ToString());
                }
                Util.Util uti = new Util.Util();
                string _appsettings = uti.GetAxAppSettings();
                if (!string.IsNullOrEmpty(_appsettings))
                {
                    HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
                }
            }
        }
        catch (Exception ex)
        {

        }
        string ScriptsPathARMdel = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePatharmdel = @" " + ScriptsPathARMdel + "";
        string directoryPatharmdel = Path.GetDirectoryName(filePatharmdel);
        FileInfo Filefiarmdel = new FileInfo(ScriptsPathARMdel);
        try
        {
            if (Filefiarmdel.Exists)
            {
                string existingJsonARM = File.ReadAllText(filePatharmdel);
                existingJsonARM = JsonConvert.SerializeObject(existingJsonARM);
                result = existingJsonARM;
            }

        }
        catch (Exception ex) { }
        return result;
    }

    [WebMethod]
    public static string SaveExternalResWs(string proj, string NamedUrls, string NamedSftp, string NamedFileServers)
    {
        string result = "";
        try
        {
            DelExternalResWs(proj);
            var urls = JsonConvert.DeserializeObject<List<NamedUrlItem>>(NamedUrls);
            var sftp = JsonConvert.DeserializeObject<List<NamedSftpItem>>(NamedSftp);
            var fileServers = JsonConvert.DeserializeObject<List<NamedFileServerItem>>(NamedFileServers);

            var urlDict = urls.ToDictionary(x => x.Name, x => (object)x.Url);
            var sftpDict = sftp.ToDictionary(x => x.Name, x => (object)x.SFTP);
            var fileServerDict = fileServers.ToDictionary(x => x.Name, x => (object)x.FileServer);

            string project = proj;
            var propertiesDict = new Dictionary<string, object>
            {
                { "NamedURLS", urlDict },
                { "NamedSFTP", sftpDict },
                { "NamedFileServer", fileServerDict }
            };
            var propData = new Dictionary<string, object> {
                { "ExternalResources", propertiesDict }
            };
            var appDict = new Dictionary<string, object>
            {
                { project, propData }
            };
            var mainDict = new Dictionary<string, object>
            {
                { "appsettings", appDict }
            };
            string jsonString = JsonConvert.SerializeObject(mainDict);
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = @" " + ScriptsPath + "";
            string directoryPath = Path.GetDirectoryName(filePath);
            FileInfo Filefi = new FileInfo(ScriptsPath);
            if (!Filefi.Exists)
            {
                File.WriteAllText(ScriptsPath, jsonString);
            }
            else
            {
                string existingJson = File.ReadAllText(filePath);
                if (existingJson != "")
                {
                    JObject json = JObject.Parse(existingJson);
                    JObject newData = JObject.Parse(jsonString);
                    json.Merge(newData, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    File.WriteAllText(filePath, json.ToString());
                }
            }
            Util.Util uti = new Util.Util();
            string _appsettings = uti.GetAxAppSettings();
            if (!string.IsNullOrEmpty(_appsettings))
            {
                HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
            }
        }
        catch (Exception ex)
        {
        }

        try
        {
            FDW fdwObj = new FDW(proj);
            fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_ExtRes_CONN_KEY, proj);
        }
        catch (Exception ex) { }
        string ScriptsPathARM = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePatharm = @" " + ScriptsPathARM + "";
        string directoryPatharm = Path.GetDirectoryName(filePatharm);
        FileInfo Filefiarm = new FileInfo(ScriptsPathARM);
        try
        {
            if (Filefiarm.Exists)
            {
                string existingJsonARM = File.ReadAllText(filePatharm);
                existingJsonARM = JsonConvert.SerializeObject(existingJsonARM);
                result = existingJsonARM;
            }

        }
        catch (Exception ex) { }
        return result;
    }


    [WebMethod]
    public static string DelExternalResWs(string proj)
    {
        string result = "";
        try
        {
            string scriptsPathARM = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = scriptsPathARM;
            string directoryPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryPath))
            {
                string existingJsonARM = File.ReadAllText(filePath);
                JObject jsonARM = JObject.Parse(existingJsonARM);
                if (jsonARM["appsettings"] != null && jsonARM["appsettings"][proj] != null && jsonARM["appsettings"][proj].Type == JTokenType.Object)
                {
                    JObject projectObj = (JObject)jsonARM["appsettings"][proj];
                    if (projectObj["ExternalResources"] != null)
                    {
                        projectObj.Property("ExternalResources").Remove();
                        string modifiedJsonARM = jsonARM.ToString();
                        File.WriteAllText(filePath, modifiedJsonARM);
                    }
                }
                Util.Util uti = new Util.Util();
                string _appsettings = uti.GetAxAppSettings();
                if (!string.IsNullOrEmpty(_appsettings))
                {
                    HttpContext.Current.Application["AppSettingsIni"] = _appsettings;
                }
            }
        }
        catch (Exception ex)
        {

        }
        try
        {
            FDW fdwObj = new FDW(proj);
            fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_ExtRes_CONN_KEY, proj);
        }
        catch (Exception ex) { }
        string ScriptsPathARMdel = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string filePatharmdel = @" " + ScriptsPathARMdel + "";
        string directoryPatharmdel = Path.GetDirectoryName(filePatharmdel);
        FileInfo Filefiarmdel = new FileInfo(ScriptsPathARMdel);
        try
        {
            if (Filefiarmdel.Exists)
            {
                string existingJsonARM = File.ReadAllText(filePatharmdel);
                existingJsonARM = JsonConvert.SerializeObject(existingJsonARM);
                result = existingJsonARM;
            }

        }
        catch (Exception ex) { }
        return result;
    }
}

public class NamedUrlItem
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public class NamedSftpItem
{
    public string Name { get; set; }
    public string SFTP { get; set; }
}

public class NamedFileServerItem
{
    public string Name { get; set; }
    public string FileServer { get; set; }
}
