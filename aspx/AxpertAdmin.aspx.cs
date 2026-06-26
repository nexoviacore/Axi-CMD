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
using System.Net;
using System.Security;
using RabbitMQ.Client.Impl;
using iTextSharp.text;
using System.Security.Policy;
using System.Collections.Concurrent;

public partial class AxpertAdmin : System.Web.UI.Page
{
    public string appTitle = "Axpert Admin Settings";
    public static string jsonText = string.Empty;
    //public string existingJsonARM = string.Empty;
    //public string existingJsonFile = string.Empty;
    public string existingAppSettings = "{}";
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
        try
        {
            AntiforgeryChecker.Check(this, _antiforgery);
        }
        catch (Exception ex)
        {
            Response.Redirect("~/CusError/AxCustomError.aspx");
        }
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
                else if (HttpContext.Current.Session != null && HttpContext.Current.Session["AxpertAdUser"] == null)
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
            //RMQueueproj.DataSource = lst;
            armExtResource.DataSource = lst;
            lstconnection.DataBind();
            axSelectProj.DataBind();
            armproj.DataBind();
            fileproj.DataBind();
            //RMQueueproj.DataBind();
            armExtResource.DataBind();
            lstRconnection.DataSource = lst;
            lstRconnection.DataBind();
            selLicDomain.DataSource = lst;
            selLicDomain.DataBind();
            lstStudioRconnection.DataSource = lst;
            lstStudioRconnection.DataBind();

            axpDevOptions.DataSource = lst;
            axpDevOptions.DataBind();
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
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

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
                if (jsonApp["appsettings"] != null && jsonApp["appsettings"][selCon] != null && jsonApp["appsettings"][selCon].Type == JTokenType.Object)
                {
                    JObject projectObj = (JObject)jsonApp["appsettings"];
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
                    //fdwObj.DeleteAllKeys(selCon + "-" + Constants.AXAPPS_XML_KEY, selCon);

                    //fdwObj.DeleteAllKeys(selCon + "-" + Constants.AXARM_CONN_KEY, selCon);
                    //fdwObj.DeleteAllKeys(selCon + "-" + Constants.AXARM_IntRes_CONN_KEY, selCon);
                    //fdwObj.DeleteAllKeys(selCon + "-" + Constants.AXFileServer_CONN_KEY, selCon);
                    //fdwObj.DeleteAllKeys(selCon + "-" + Constants.AXSSO_CONN_KEY, selCon);
                    //fdwObj.DeleteAllKeys(selCon + "-" + Constants.AXARM_ExtRes_CONN_KEY, selCon);
                    fdwObj.HashDeleteAllkey(Constants.AX_COMMON_APPSETTING_KEY, selCon);
                    HttpContext.Current.Session.Remove("AppAllSettingsKey-" + selCon);
                }
                catch (Exception ex) { }
                Response.Redirect("AxpertAdmin.aspx?auth=" + Request.QueryString["auth"].ToString());
            }
        }
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

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
            if ((dbtype.ToLower() == "postgre" || dbtype.ToLower() == "postgresql") && driver.ToLower() == "ado")
            {
                string _odbcConn = util.GetPostgreODBCConnection(dbconName);
                if (_odbcConn != "")
                    userName = _odbcConn.Split(';').First(x => x.StartsWith("Username=", StringComparison.OrdinalIgnoreCase)).Split('=')[1];
                else
                    userName = dbconName;
                pwd = "";
                xml += "<dbuser>" + userName + "</dbuser>";
                xml += "<pwd></pwd>";
            }
            else
            {
                xml += "<dbuser>" + userName + "</dbuser>";
                xml += "<pwd>" + pwd + "</pwd>";
            }
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
                //fdwObj.DeleteAllKeys(NewConName + "-" + Constants.AXAPPS_XML_KEY, NewConName);
                fdwObj.HashDeleteAllkey(Constants.AX_COMMON_APPSETTING_KEY, NewConName);
                HttpContext.Current.Session.Remove("AppAllSettingsKey-" + NewConName);
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

    [WebMethod(EnableSession = true)]
    public static string AppTestConnection(string ddldbtype, string ddldbversion, string ddldriver, string txtccname, string txtusername, string txtPassword, string txtpostgresodbc, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(ddldbtype) || Util.Util.CheckCrossScriptingInString(ddldbversion) || Util.Util.CheckCrossScriptingInString(ddldriver) || Util.Util.CheckCrossScriptingInString(txtccname) || Util.Util.CheckCrossScriptingInString(txtusername) || Util.Util.CheckCrossScriptingInString(txtPassword) || Util.Util.CheckCrossScriptingInString(txtpostgresodbc))
            throw new SecurityException("Invalid format.");

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
            if ((dbtype.ToLower() == "postgre" || dbtype.ToLower() == "postgresql") && driver.ToLower() == "ado" && txtpostgresodbc == "true")
            {
                userName = dbconName;
                jsonData += ",\"dbuser\":\"" + dbconName + "\"";
                jsonData += ",\"pwd\":\"\"";
            }
            else
            {
                jsonData += ",\"dbuser\":\"" + userName.Replace(@"\", "\\\\") + "\"";
                jsonData += ",\"pwd\":\"" + pwd + "\"";
            }
            jsonData += "}";
            string axpapp = userName;
            if (axpapp.Contains("\\"))
                axpapp = axpapp.Split('\\')[0];
            string trace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            string inputJson = "{\"_parameters\":[{\"getdbconnection\":{\"axpapp\":\"" + axpapp + "\",\"trace\":\"" + trace + "\"},\"" + axpapp + "\":" + jsonData + "}]}";

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

    protected void btndbpwb_Click(object sender, EventArgs e)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");
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
            bool _txtpostgresodbc = txtpostgresodbc.Checked;
            string txtpostgreodbc = "false";
            if (_txtpostgresodbc)
                txtpostgreodbc = "true";
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
            if ((dbtype.ToLower() == "postgre" || dbtype.ToLower() == "postgresql") && driver.ToLower() == "ado" && txtpostgreodbc == "true")
            {
                string _odbcConn = util.GetPostgreODBCConnection(dbconName);
                if (_odbcConn != "")
                    userName = _odbcConn.Split(';').First(x => x.StartsWith("Username=", StringComparison.OrdinalIgnoreCase)).Split('=')[1];
                else
                    userName = dbconName;
                jsonData += ",\"dbuser\":\"" + dbconName + "\"";
                jsonData += ",\"pwd\":\"\"";
            }
            else
            {
                jsonData += ",\"dbuser\":\"" + userName.Replace(@"\", "\\\\") + "\"";
                jsonData += ",\"pwd\":\"" + pwd + "\"";
            }
            jsonData += "}";
            string axpapp = userName;
            if (axpapp.Contains("\\"))
                axpapp = axpapp.Split('\\')[0];
            string trace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
            string inputJson = "{\"_parameters\":[{\"setdbpassword\":{\"axpapp\":\"" + axpapp + "\",\"newpwd\":\"" + newpwd + "\",\"trace\":\"" + trace + "\"},\"" + axpapp + "\":" + jsonData + "}]}";

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

    private static readonly ConcurrentDictionary<string, LoginAttemptInfo> LoginAttempts = new ConcurrentDictionary<string, LoginAttemptInfo>();
    private static readonly ConcurrentDictionary<string, RateLimitInfo> RateLimits = new ConcurrentDictionary<string, RateLimitInfo>();
    [WebMethod(EnableSession = true)]
    public static string UserAuthentication(string AuthUsername, string AuthPwd, string csrfToken)
    {
        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(AuthUsername) || Util.Util.CheckCrossScriptingInString(AuthPwd))
        {
            throw new SecurityException("Invalid format.");
        }
        string result = string.Empty;
        try
        {
            if (string.IsNullOrWhiteSpace(AuthUsername) || string.IsNullOrWhiteSpace(AuthPwd))
            {
                return result;
            }
            string ipAddress = HttpContext.Current.Request.UserHostAddress ?? "UNKNOWN";
            RateLimitInfo rateInfo = RateLimits.GetOrAdd(ipAddress,
                k => new RateLimitInfo
                {
                    Count = 0,
                    WindowStart = DateTime.UtcNow
                });
            lock (rateInfo)
            {
                if ((DateTime.UtcNow - rateInfo.WindowStart).TotalMinutes >= 1)
                {
                    rateInfo.Count = 0;
                    rateInfo.WindowStart = DateTime.UtcNow;
                }
                rateInfo.Count++;
                if (rateInfo.Count > 10)
                {
                    throw new SecurityException("Too many login attempts. Please try again later.");
                }
            }
            string userKey = AuthUsername.Trim().ToLowerInvariant();
            LoginAttemptInfo loginInfo = LoginAttempts.GetOrAdd(userKey, k => new LoginAttemptInfo());
            if (loginInfo.LockedUntil.HasValue && loginInfo.LockedUntil.Value > DateTime.UtcNow)
            {
                throw new SecurityException("Account is temporarily locked. Please try again later.");
            }
            Util.Util objutil = new Util.Util();
            string iniFilePath = HttpContext.Current.Server.MapPath("~/ConfigAuthentication.ini");
            if (!File.Exists(iniFilePath))
                return result;
            string inifile = File.ReadAllText(iniFilePath);
            if (string.IsNullOrWhiteSpace(inifile))
                return result;
            dynamic json = JsonConvert.DeserializeObject(inifile.Replace("\r\n", string.Empty));
            bool authenticated = false;
            foreach (var chlAuthUser in json)
            {
                string storedUser = Convert.ToString(chlAuthUser.uname.Value);
                string storedPwd = Convert.ToString(chlAuthUser.pwd.Value);
                if (string.Equals(storedUser, AuthUsername, StringComparison.OrdinalIgnoreCase) && storedPwd == AuthPwd)
                {
                    authenticated = true;
                    break;
                }
            }
            if (authenticated)
            {
                loginInfo.FailedAttempts = 0;
                loginInfo.LockedUntil = null;
                HttpContext.Current.Session["AxpertAdUser"] = HttpContext.Current.Session.SessionID;
                result = objutil.encrtptDecryptAES(HttpContext.Current.Session.SessionID);
            }
            else
            {
                loginInfo.FailedAttempts++;
                if (loginInfo.FailedAttempts >= 5)
                {
                    loginInfo.LockedUntil = DateTime.UtcNow.AddMinutes(10);
                }
            }
        }
        catch (Exception ex)
        {
            result = "error:" + ex.Message;
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("User Authentication: " + ex.Message, HttpContext.Current.Session.SessionID, "UserAuthentication", "new", "true");
        }
        return result;
    }

    protected void btnRedisOk_Click(object sender, EventArgs e)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

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
            HttpContext.Current.Session.Remove("AppAllSettingsKey-" + RConName);
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
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

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

    [WebMethod(EnableSession = true)]
    public static string RedisTestConnection(string rHost, string rPort, string rPwd, string axwConn, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(rHost) || Util.Util.CheckCrossScriptingInString(rPort) || Util.Util.CheckCrossScriptingInString(rPwd) || Util.Util.CheckCrossScriptingInString(axwConn))
            throw new SecurityException("Invalid format.");

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

    [WebMethod(EnableSession = true)]
    public static string VerifyARMSettings(string proj, string aUrl, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(proj) || Util.Util.CheckCrossScriptingInString(aUrl))
            throw new SecurityException("Invalid format.");

        if (!IsValidArmUrl(aUrl))
            throw new SecurityException("Invalid ARM URL");

        if (string.IsNullOrWhiteSpace(proj))
            return "Error: Project cannot be left empty";

        if (string.IsNullOrWhiteSpace(aUrl))
            return "Error: ARM URL cannot be left empty";

        try
        {
            string URL = aUrl.TrimEnd('/') + "/ARM_APIs/api/v1/ValidateAppSettings";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";

            var _inputJson = "{\"appname\":\"" + proj + "\"}";
            request.ContentLength = _inputJson.Length;

            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(_inputJson);
            requestWriter.Close();

            WebResponse webResponse = request.GetResponse();
            Stream webStream = webResponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(webStream);
            string response = responseReader.ReadToEnd();
            return response;
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string ARMConnection(string aKey, string aUrl, string aScriptsUrl, string aPeg, string proj, string aExpiryMinutes, string aNotificationURL, string aNotificationExpiryHours, string aNotificationMaxPerUser, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(aKey) || Util.Util.CheckCrossScriptingInString(aUrl) || Util.Util.CheckCrossScriptingInString(aScriptsUrl) || Util.Util.CheckCrossScriptingInString(aPeg) || Util.Util.CheckCrossScriptingInString(proj) || Util.Util.CheckCrossScriptingInString(aExpiryMinutes) || Util.Util.CheckCrossScriptingInString(aNotificationURL) || Util.Util.CheckCrossScriptingInString(aNotificationExpiryHours) || Util.Util.CheckCrossScriptingInString(aNotificationMaxPerUser))
            throw new SecurityException("Invalid format.");

        if (!IsValidArmUrl(aUrl))
            throw new SecurityException("Invalid ARM URL");

        if (!IsValidArmUrl(aScriptsUrl))
            throw new SecurityException("Invalid Scripts URL");

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
            aRMQapiUrl = armaUrl + "ARM_APIs/api/v1/ARMPushToQueue";
            aSignalRapiUrl = aNotificationURL + "/api/v1/SendSignalR";
            AxFCMSendMsgURL = armaUrl + "ARM_APIs/api/v1/SendFCMNotification";
            AxRapidSaveURL = armaScriptsUrl + "ASBRapidSaveRest.dll/datasnap/rest/TASBRapidSaveRest/RapidSave";
            axpegemailactionurl = armaUrl + "ARM_APIs/api/v1/ARMMailTaskAction";
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
            //fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_CONN_KEY, proj);
            //fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_IntRes_CONN_KEY, proj);
            fdwObj.HashDeleteAllkey(Constants.AX_COMMON_APPSETTING_KEY, proj);
            HttpContext.Current.Session.Remove("AppAllSettingsKey-" + proj);
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


    [WebMethod(EnableSession = true)]
    public static string DelARMConnectionWs(string proj, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(proj))
            throw new SecurityException("Invalid format.");

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
            //fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_CONN_KEY, proj);
            //fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_IntRes_CONN_KEY, proj);
            fdwObj.HashDeleteAllkey(Constants.AX_COMMON_APPSETTING_KEY, proj);
            HttpContext.Current.Session.Remove("AppAllSettingsKey-" + proj);
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

    [WebMethod(EnableSession = true)]
    public static string FileConnection(string fUpload, string fDownload, string proj, string fMapUsername, string fMapPwd, string AxAttachSize, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(fUpload) || Util.Util.CheckCrossScriptingInString(fDownload) || Util.Util.CheckCrossScriptingInString(proj) || Util.Util.CheckCrossScriptingInString(fMapUsername) || Util.Util.CheckCrossScriptingInString(fMapPwd) || Util.Util.CheckCrossScriptingInString(AxAttachSize))
            throw new SecurityException("Invalid format.");

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
                { "FileServerMapPwd", fMapPwd },
                { "AxAttachmentSize", AxAttachSize }
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
            //fdwObj.DeleteAllKeys(proj + "-" + Constants.AXFileServer_CONN_KEY, proj);
            //fdwObj.HashDeletekeyNew(Constants.AX_COMMON_APPSETTING_KEY, Constants.AXFileServer_CONN_KEY, proj);
            fdwObj.HashDeleteAllkey(Constants.AX_COMMON_APPSETTING_KEY, proj);
            HttpContext.Current.Session.Remove("AppAllSettingsKey-" + proj);
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

    [WebMethod(EnableSession = true)]
    public static string DelFileConnectionWs(string proj, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(proj))
            throw new SecurityException("Invalid format.");

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
            //fdwObj.DeleteAllKeys(proj + "-" + Constants.AXFileServer_CONN_KEY, proj);
            //fdwObj.HashDeletekeyNew(Constants.AX_COMMON_APPSETTING_KEY, Constants.AXFileServer_CONN_KEY, proj);
            fdwObj.HashDeleteAllkey(Constants.AX_COMMON_APPSETTING_KEY, proj);
            HttpContext.Current.Session.Remove("AppAllSettingsKey-" + proj);
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

    [WebMethod(EnableSession = true)]
    public static string LicDomainConnection(string domainName, string proj, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(domainName) || Util.Util.CheckCrossScriptingInString(proj))
            throw new SecurityException("Invalid format.");

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


    [WebMethod(EnableSession = true)]
    public static string DelLicDomainConnection(string proj, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(proj))
            throw new SecurityException("Invalid format.");

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
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

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
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

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

    [WebMethod(EnableSession = true)]
    public static string RedisTestConnectionStudio(string rHost, string rPort, string rPwd, string axsConn, string studioUrl, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(rHost) || Util.Util.CheckCrossScriptingInString(rPort) || Util.Util.CheckCrossScriptingInString(rPwd) || Util.Util.CheckCrossScriptingInString(axsConn) || Util.Util.CheckCrossScriptingInString(studioUrl))
            throw new SecurityException("Invalid format.");
        if (!IsValidArmUrl(studioUrl))
            throw new SecurityException("Invalid ARM URL");

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


    [WebMethod(EnableSession = true)]
    public static string RMQueueConnection(string aRMQHost, string aRMQPort, string aRMQUser, string aRMQPwd, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(aRMQHost) || Util.Util.CheckCrossScriptingInString(aRMQPort) || Util.Util.CheckCrossScriptingInString(aRMQUser) || Util.Util.CheckCrossScriptingInString(aRMQPwd))
            throw new SecurityException("Invalid format.");

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


    [WebMethod(EnableSession = true)]
    public static string DelRMQueueConnectionWs()
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

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

    [WebMethod(EnableSession = true)]
    public static string SaveExternalResWs(string proj, string NamedUrls, string NamedSftp, string NamedFileServers, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(proj) || Util.Util.CheckCrossScriptingInString(NamedUrls) || Util.Util.CheckCrossScriptingInString(NamedSftp) || Util.Util.CheckCrossScriptingInString(NamedFileServers))
            throw new SecurityException("Invalid format.");

        string result = "";
        try
        {
            DelExternalResWs(proj, csrfToken);
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
            //fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_ExtRes_CONN_KEY, proj);
            //fdwObj.HashDeletekeyNew(Constants.AX_COMMON_APPSETTING_KEY, Constants.AXARM_ExtRes_CONN_KEY, proj);
            fdwObj.HashDeleteAllkey(Constants.AX_COMMON_APPSETTING_KEY, proj);
            HttpContext.Current.Session.Remove("AppAllSettingsKey-" + proj);
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


    [WebMethod(EnableSession = true)]
    public static string DelExternalResWs(string proj, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(proj))
            throw new SecurityException("Invalid format.");

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
            //fdwObj.DeleteAllKeys(proj + "-" + Constants.AXARM_ExtRes_CONN_KEY, proj);
            //fdwObj.HashDeletekeyNew(Constants.AX_COMMON_APPSETTING_KEY, Constants.AXARM_ExtRes_CONN_KEY, proj);
            fdwObj.HashDeleteAllkey(Constants.AX_COMMON_APPSETTING_KEY, proj);
            HttpContext.Current.Session.Remove("AppAllSettingsKey-" + proj);
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

    private static bool IsValidArmUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        url = url.Trim();

        Uri uri;
        if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            return false;

        if (uri.Scheme != Uri.UriSchemeHttp &&
            uri.Scheme != Uri.UriSchemeHttps)
            return false;

        if (string.IsNullOrWhiteSpace(uri.Host))
            return false;

        if (url.Contains("<") ||
            url.Contains(">") ||
            url.Contains("\"") ||
            url.Contains("'"))
            return false;

        string decoded =
            HttpUtility.UrlDecode(url).ToLower();

        if (decoded.Contains("javascript:") ||
            decoded.Contains("data:") ||
            decoded.Contains("file:") ||
            decoded.Contains("<script"))
            return false;

        return true;
    }

    [WebMethod(EnableSession = true)]
    public static string SaveAxpertDevOptionsWs(string proj, string devOpt, string csrfToken)
    {
        var session = HttpContext.Current.Session;
        if (session == null || session["AxpertAdUser"] == null)
            throw new UnauthorizedAccessException("Session expired");

        if (HttpContext.Current.Session["AntiforgeryToken"] == null || csrfToken != HttpContext.Current.Session["AntiforgeryToken"].ToString())
        {
            throw new SecurityException("CSRF Attack Detected!");
        }
        if (Util.Util.CheckCrossScriptingInString(proj) || Util.Util.CheckCrossScriptingInString(devOpt))
            throw new SecurityException("Invalid format.");

        string result = "";
        try
        {
            string project = proj;
            var propertiesDict = new Dictionary<string, object>
            {
                { "Options", devOpt }
            };
            var propData = new Dictionary<string, object> {
                { "AxpDevOptsMenu", propertiesDict }
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
            string _ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
            string filePath = @" " + _ScriptsPath + "";
            string directoryPath = Path.GetDirectoryName(filePath);
            FileInfo Filefi = new FileInfo(_ScriptsPath);
            if (!Filefi.Exists)
            {
                File.WriteAllText(_ScriptsPath, jsonString);
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
            fdwObj.HashDeleteAllkey(Constants.AX_COMMON_APPSETTING_KEY, proj);
            HttpContext.Current.Session.Remove("AppAllSettingsKey-" + proj);
        }
        catch (Exception ex) { }
        string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
        string SaveddevOpt = @" " + ScriptsPath + "";
        FileInfo Filefiarm = new FileInfo(ScriptsPath);
        try
        {
            if (Filefiarm.Exists)
            {
                string existingJson = File.ReadAllText(SaveddevOpt);
                existingJson = JsonConvert.SerializeObject(existingJson);
                result = existingJson;
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

public class LoginAttemptInfo
{
    public int FailedAttempts { get; set; }
    public DateTime? LockedUntil { get; set; }
}

public class RateLimitInfo
{
    public int Count { get; set; }
    public DateTime WindowStart { get; set; }
}