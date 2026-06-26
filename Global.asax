<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {

        // Code that runs on application startup 
        SetLandingPages();
        Application["CustomisedIviews"] = "";
        Application["CustomIviewString"] = "";
        Application["axChangePwd"] = "true";
        Application["axChangePwdReDir"] = "false";
        Application["AxLoadFromCache"] = "";
        //ValidateIviewParamOnGo - if true, The iview parameters will be validated on click of go button in the iview, else will be validated on blur.
        Application["ValidateIviewParamOnGo"] = "false";

        //Global variables for Worflow configuration
        Application["AxOnApproveDisable"] = "false";
        Application["AxOnReturnSave"] = "false";
        Application["AxOnRejectSave"] = "false";
        Application["AxOnRejectDisable"] = "false";


        if (Application["ScriptsPath"] == null)
            SetAppVariables();
        SetAxCustomPath();
        GetAxApps();
        GetAxProps();
        GetAxAppSettings();
        //GetAxpString();// its not required for concurrect user support

        //SetServerLicense();

        //For Bundling and Minification Support
        BundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);

        if (HttpContext.Current.IsDebuggingEnabled)
        {
            System.Web.Optimization.BundleTable.EnableOptimizations = false;
        }
        else
        {
            System.Web.Optimization.BundleTable.EnableOptimizations = true;
        }

        //To forecefully enable Bundling and Minification
        //System.Web.Optimization.BundleTable.EnableOptimizations = true;

        //To disable Minification in Bundling and Minification
        // System.Web.Optimization.BundleTable.Bundles.ToList().ForEach(b => b.Transforms.Clear());

        //FDW objFDW = FDW.Instance;
        //string strProj = ConfigurationManager.AppSettings["proj"];
        //objFDW.Initialize(strProj);
        //GetConfigurationDetails(objFDW);
    }

    private static bool _isBaseUrlInitialized = false;
    private static readonly object _lock = new object();

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        if (!_isBaseUrlInitialized)
        {
            lock (_lock)
            {
                if (!_isBaseUrlInitialized)
                {
                    try
                    {
                        var context = System.Web.HttpContext.Current;
                        if (context != null && context.Request != null && context.Request.Url != null)
                        {
                            var url = context.Request.Url.GetLeftPart(UriPartial.Authority);
                            if (!string.IsNullOrWhiteSpace(url))
                            {
                                if (!url.EndsWith("/"))
                                    url += "/";
                                WebContextProvider.SetBaseUrl(url);
                                _isBaseUrlInitialized = true;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        if (WebContextProvider.GetBaseUrlCallback == null)
        {
            WebContextProvider.GetBaseUrlCallback = () =>
            {
                var ctx = System.Web.HttpContext.Current;
                if (ctx != null && ctx.Request != null && ctx.Request.Url != null)
                    return ctx.Request.Url.GetLeftPart(UriPartial.Authority) + "/";
                return WebContextProvider.GetBaseUrl();
            };
        }
    }



    private void GetAxAppSettings()
    {
        Util.Util uti = new Util.Util();
        string appsettings = uti.GetAxAppSettings();
        if (!string.IsNullOrEmpty(appsettings))
        {
            Application["AppSettingsIni"] = appsettings;
        }

    }

    private void GetAxpString()
    {

        ASB.WebService asb = new ASB.WebService();
        asb.GetAxpString();
    }

    #region Server Based lic

    private void SetServerLicense()
    {

        ASB.WebService asbWs = new ASB.WebService();
        asbWs.SetServerLicense();
    }

    #endregion    

    private void GetAxProps()
    {
        try
        {
            string contents = "";
            string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
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

                reader.Close();
                reader.Dispose();
            }
            Application["axProps"] = contents;
        }
        catch (Exception ex)
        {
            string axProps = "<axprops getfrom=\"\" setto=\"\"></axprops>";
            Application["axProps"] = axProps;

            LogFile.ErrorLog errLogObj = new LogFile.ErrorLog();
            errLogObj.LogException(ex, "Application_Error - Global.asax", ex.Message);
        }
    }

    private void GetAxApps()
    {
        string contents = "";
        string scriptsPath = string.Empty;
        string strProj = string.Empty;

        if (ConfigurationManager.AppSettings["proj"] != null && ConfigurationManager.AppSettings["proj"].ToString() != "")
            strProj = ConfigurationManager.AppSettings["proj"].ToString();
        scriptsPath = Application["scriptsPath"].ToString();
        Application["axdb"] = "Oracle";
        if (string.IsNullOrEmpty(strProj))
            return;
        string db = string.Empty;
        int dbIdx = -1;
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(scriptsPath);
        if (di.Exists)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(scriptsPath + "axapps.xml");
            bool startRead = false;
            string lastEmptyNode = string.Empty;
            while (reader.Read())
            {
                reader.MoveToContent();

                if (reader.NodeType == System.Xml.XmlNodeType.Element && reader.Name == strProj)
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
                    if (reader.NodeType == System.Xml.XmlNodeType.EndElement && reader.Name == strProj)
                    {
                        startRead = false;
                        contents += "</" + reader.Name + ">";
                        break;
                    }
                    else if (reader.NodeType == System.Xml.XmlNodeType.EndElement && startRead == true) //Display the end of the element.
                        contents += "</" + reader.Name + ">";
                }

            }
            reader.Close();
            reader.Dispose();
        }

        Application["axdb"] = db;
        Application["axApps"] = contents;

    }
    private void GetConfigurationDetails(FDW objFDW)
    {
        //Read all the files from Config folder
        try
        {
            string configFilePath = string.Empty;
            configFilePath = Server.MapPath("~/Config");
            System.IO.DirectoryInfo Files = new System.IO.DirectoryInfo(configFilePath);
            if (Files.Exists)
            {
                FDR fdrObj = new FDR();
                System.IO.FileInfo[] configFiles = Files.GetFiles();  // project names are added in application array
                AppConfiguration.AppConfiguration configObj = new AppConfiguration.AppConfiguration();
                for (int i = 0; i < configFiles.Length; i++)
                {
                    System.IO.FileInfo eachFile = configFiles[i];
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(configFilePath + "\\" + eachFile))
                    {
                        string configFileName = eachFile.Name;
                        configFileName = configFileName.Substring(0, configFileName.IndexOf("."));

                        if (configFileName == "") continue;

                        string configStr = string.Empty;
                        //configStr = fdrObj.StringFromRedis(Constants.CONFIGAPP_JSON_KEY, configFileName.ToLower());
                        var _jsoncontents = fdrObj.HashGetAllKey(Constants.AX_COMMON_APPSETTING_KEY, configFileName.ToLower());
                        if (_jsoncontents != null && _jsoncontents.ContainsKey(Constants.CONFIGAPP_JSON_KEY))
                        {
                            configStr = _jsoncontents[Constants.CONFIGAPP_JSON_KEY];
                        }
                        if (configStr == string.Empty)
                            configStr = reader.ReadToEnd();

                        //Checking for custom keys
                        string customDetails = GetCustomConfigDetails(configFileName);
                        try
                        {
                            bool resetConfig = false;
                            if (configStr.Trim() != String.Empty)
                            {
                                configStr = configObj.ValidateAppConfiguration(configStr, ref resetConfig);
                                if (resetConfig)
                                {
                                    reader.Dispose();
                                    configObj.SaveConfigFile(configFileName, configStr);
                                }
                            }
                            else
                            {
                                reader.Dispose();
                                configStr = configObj.SaveDefaultConfigFile(configFileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            reader.Dispose();
                            configStr = configObj.SaveDefaultConfigFile(configFileName);
                        }
                        finally
                        {
                            //objFDW.SaveInRedisServer(Constants.CONFIGAPP_JSON_KEY, configStr + customDetails, Constants.CONFIGAPP_JSON_KEY, configFileName.ToLower());
                            objFDW.HashSetKeyWithSchema(Constants.AX_COMMON_APPSETTING_KEY, Constants.CONFIGAPP_JSON_KEY, configStr + customDetails, configFileName.ToLower());
                        }
                    }
                }
            }

        }
        catch (Exception ex) { }
    }

    private string GetCustomConfigDetails(string proj)
    {
        try
        {
            string configFilePath = string.Empty;
            string customKeys = string.Empty;

            configFilePath = Server.MapPath("~/" + proj + "//custom.cfg");
            System.IO.FileInfo conFile = new System.IO.FileInfo(configFilePath);
            if (conFile.Exists)
            {
                //Check for custom config details inside the project folder
                using (System.IO.StreamReader reader = new System.IO.StreamReader(configFilePath))
                {
                    customKeys = reader.ReadToEnd();
                }
                return customKeys;
            }
        }
        catch (Exception ex) { }
        return string.Empty;

    }
    private void SetAxCustomPath()
    {

        string strProj = string.Empty;
        if (ConfigurationManager.AppSettings["proj"] != null)
            strProj = ConfigurationManager.AppSettings["proj"].ToString();
        string strLoginPath = "../aspx/signin.aspx";
        string strSignOutPath = "../aspx/signout.aspx";
        string strSessExpPath = "../aspx/sess.aspx";
        string strMainPath = "../aspx/mainnew.aspx";
        string filePath = string.Empty;

        try
        {
            filePath = Server.MapPath(strProj);
        }
        catch (Exception ex)
        {
            filePath = Server.MapPath(strProj);
        }
        if ((!string.IsNullOrEmpty(filePath)) && !string.IsNullOrEmpty(strProj))
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
            if ((di.Exists))
            {
                int i = 0;
                System.IO.FileInfo[] files = di.GetFiles();
                for (i = 0; i <= files.Length - 1; i++)
                {
                    if ((files[i].Name.ToLower() == "signin.aspx"))
                    {
                        strLoginPath = "../" + strProj + "/" + files[i].Name;
                    }
                    if ((files[i].Name.ToLower() == "signout.aspx"))
                    {
                        strSignOutPath = "../" + strProj + "/" + files[i].Name;
                    }
                    if ((files[i].Name.ToLower() == "sess.aspx"))
                    {
                        strSessExpPath = "../" + strProj + "/" + files[i].Name;
                    }
                    if ((files[i].Name.ToLower() == "main.aspx"))
                    {
                        strMainPath = "../" + strProj + "/" + files[i].Name;
                    }
                }
            }
        }
        if (ConfigurationManager.AppSettings["AxCLOUD"] != null && ConfigurationManager.AppSettings["AxCLOUD"].ToString() == "true")
            Application["LoginPath"] = strLoginPath;
        else
            Application["LoginPath"] = strLoginPath;
        Application["SignOutPath"] = strSignOutPath;
        Application["SessExpiryPath"] = strSessExpPath;
        Application["MainPath"] = strMainPath;
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        //Server.ClearError();
        //Response.Clear();
        //string path = string.Join("", HttpContext.Current.Request.Url.Segments, 0, Array.IndexOf(HttpContext.Current.Request.Url.Segments, "aspx/")) + "CusError/AxCustomError.aspx";
        //Response.Redirect(path);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("App Details:");
        try
        {
            sb.AppendLine("SessionId:" + Session.SessionID);
            if (Session["username"] != null)
                sb.AppendLine("UserName:" + Session["username"].ToString());
            else
                sb.AppendLine("UserName:");
            if (Session["Project"] != null)
                sb.AppendLine("Project Name:" + Session["Project"].ToString());
            else
                sb.AppendLine("Project Name:");
            sb.AppendLine("URL:" + HttpContext.Current.Request.Url.ToString());
        }
        catch (Exception exs)
        {
            sb.AppendLine("Exception:" + exs.Message);
        }

        LogFile.ErrorLog errLogObj = new LogFile.ErrorLog();
        Exception ex = Server.GetLastError();
        errLogObj.LogException(ex, "Application_Error - Global.asax", sb.ToString());

        Response.Redirect("~/CusError/AxCustomError.aspx");
    }




    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started 
        string strProj = string.Empty;
        Util.Util utilObj = new Util.Util();
        if (ConfigurationManager.AppSettings["proj"] != null && ConfigurationManager.AppSettings["proj"].ToString() != "")
            strProj = ConfigurationManager.AppSettings["proj"].ToString();
        if (Application["axApps"] != null)
            utilObj.GetDBConnection(strProj, Application["axApps"].ToString());


    }

    void Session_End(object sender, EventArgs e)
    {

        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }


    public void SetAppVariables()
    {
        string scriptsPath = string.Empty;
        string scriptsUrlPath = string.Empty;
        try
        {
            if (ConfigurationManager.AppSettings["scriptsUrlPath"] != null)
            {
                scriptsUrlPath = ConfigurationManager.AppSettings["scriptsUrlPath"].ToString();
                Application["scriptsUrlPath"] = scriptsUrlPath;
            }

            if (ConfigurationManager.AppSettings["ScriptsPath"] != null)
            {
                scriptsPath = ConfigurationManager.AppSettings["ScriptsPath"].ToString();
                Application["ScriptsPath"] = scriptsPath;
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("../CusError/AxCustomError.aspx");
        }
    }

    private void SetLandingPages()
    {
        //Application["default"] = "../aspx/Page.aspx";
        //Application["-dPage-"] = "../aspx/Page.aspx";
        //Application["Administrator"] = "../aspx/dashboard.aspx";
        //Application["Inventory"] = "../aspx/iview.aspx?ivname=stockrpt";
    }


</script>
