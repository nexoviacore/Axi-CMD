using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Collections;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Web.Services;
using Newtonsoft.Json.Linq;
using System.Globalization;

public partial class aspx_Configuration : System.Web.UI.Page
{
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    ASB.WebService objWeb = new ASB.WebService();
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    public string strlng = "ENGLISH";
    public string appsessionKey = string.Empty;
    public string apiUrl = string.Empty;
    public string enableBackForwButton = string.Empty;
    public bool isTstPop = false; bool bLoginTrace = false,
            bDisplayAutoGenFields = false, bDbPagination = false,
            bDisplayUsersCount = false, bEnableOldTheme = false, bShowSubmitCancel = false,
            bIviewWrap = false, bBreadCrumb = false, bLogTimeTaken = false, bRedirectChangePwd = false;
    public string direction = "ltr";
    public string language = "ENGLISH";
    public string langCode = String.Empty;
    public string langType = "en";
    public bool isCloudApp = false;
    public string advancedConfig = string.Empty;
    public string userName = string.Empty;
    public string proj = string.Empty;
    public string sessId = string.Empty;
    public bool lockPage = false;
    FDR fdrObj;
    FDW fdwObj = new FDW();
    bool logChanges = false;
    string savedConfigStr = string.Empty;
    Dictionary<string, string> savedConfig = new Dictionary<string, string>();
    string schemaName = string.Empty;
    string ipaddress = string.Empty;

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
        bool bRestore = false;
        util.IsValidSession();

        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }
        else if (!util.CheckValidLogin())
        {
            SessionExpired();
            return;
        }

        SetPageVariables();

        if (!IsPostBack)
        {
            util.CheckUserSettings();
            if (Request.QueryString["reStoreDefault"] != null)
                bRestore = Convert.ToBoolean(Request.QueryString["reStoreDefault"]);

            LoadDefaultAttributes(bRestore);
            LoadLanguages();
            //LoadLangConfigKeys();
            LoadLangConfigKeysNew(hdnLanguage.Value);
            SetDefaultLangKeys();
            string result = GetAxLangSourceVals();
            if (result.Length > 0)
            {
                hdnAxLangSrc.Value = result;
            }
            //else
            //{
            //    menu6.Style.Add("display", "none");
            //}
        }

        SetPageUI();
    }

    /// <summary>
    /// function for handling session timeout.
    /// </summary>
    /// <remarks></remarks>
    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

    private void SetPageUI()
    {
        if (ConfigurationManager.AppSettings["advancedConfig"] != null)
        {
            advancedConfig = ConfigurationManager.AppSettings["advancedConfig"].ToString();
            if (advancedConfig == "true")
            {
                divmargins.Style.Add("display", "block");
                divtitlealign.Style.Add("display", "block");
                divprint32.Style.Add("display", "block");
                divtally.Style.Add("display", "block");
            }
        }

        if (Session["Build"] != null && Convert.ToString(Session["Build"]) == "T")
        {
            liUserSettings.Attributes["class"] = "";
            //menu4.Attributes["class"] = "tab-pane fade configheight";
            btnRestore.Visible = true;
        }
        else
        {
            tabApplication.Style.Add("display", "none");
            tabForms.Style.Add("display", "none");
            tabReports.Style.Add("display", "none");
            tabDeveloper.Style.Add("display", "none");
            tabGlobe.Style.Add("display", "none");
            tabMenuSet.Style.Add("display", "none");
            home.Style.Add("display", "none");
            menu1.Style.Add("display", "none");
            menu2.Style.Add("display", "none");
            menu3.Style.Add("display", "none");
            //menu4.Style.Add("display", "none");
            menu7.Style.Add("display", "none");
            liUserSettings.Attributes["class"] = "active";
            menu5.Attributes["class"] = "tab-pane fade in active configheight";
            btnRestore.Visible = false;
        }
    }

    private void SetPageVariables()
    {
        if (Session["AppSessionKey"] != null)
        {
            appsessionKey = Session["AppSessionKey"].ToString();
        }

        if (Session["language"] != null && Session["language"].ToString() != "")
        {
            language = Session["language"].ToString().ToUpper();
            langCode = "_" + language.Substring(0, 3).ToUpper();
        }

        if (HttpContext.Current.Session["username"] != null)
            userName = HttpContext.Current.Session["username"].ToString();

        if (HttpContext.Current.Session["Project"] != null)
            proj = HttpContext.Current.Session["Project"].ToString();

        if (HttpContext.Current.Session["nsessionid"] != null)
            sessId = HttpContext.Current.Session["nsessionid"].ToString();

        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();

        savedConfigStr = util.ReadFromFile(Server.MapPath("~/Config/") + proj + ".cfg");

        ipaddress = util.GetIpAddress();

        fdrObj = (FDR)HttpContext.Current.Session["FDR"];

        // string lockedBy = fdrObj.StringFromRedis(Constants.CONFIG_LOCK_KEY);
        // CheckPageLock(lockedBy);

        if (ConfigurationManager.AppSettings["AxpertWebLogs"] != null && ConfigurationManager.AppSettings["AxpertWebLogs"].ToString() == "true" && !lockPage)
            logChanges = true;

        //Updating the latest Config values in Session, so that it will be set to the config app page fields.
        if (savedConfigStr != string.Empty)
        {
            util.UpdateConfiginSession(savedConfigStr, logChanges);

            if (logChanges && HttpContext.Current.Session["ConfigArray"] != null)
                savedConfig = (Dictionary<string, string>)HttpContext.Current.Session["ConfigArray"];
        }

        if (ConfigurationManager.AppSettings["NodeAPI"] != null)
            apiUrl = ConfigurationManager.AppSettings["NodeAPI"].ToString();

        if (ConfigurationManager.AppSettings["isCloudApp"] != null)
        {
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString()); ;
        }
        Page.ClientScript.RegisterStartupScript(GetType(), "set global var in iview", "<script>var isCloudApp = '" + isCloudApp.ToString() + "';</script>");
    }

    private void CheckPageLock(string lockedBy)
    {
        bool acquireLock = false;
        string[] lockDetails = lockedBy.Split('♦');
        if (lockDetails.Length == 4 && lockDetails[2] != sessId)
        {
            DateTime lockedOn = Convert.ToDateTime(lockDetails[1]);
            bool lockExpired = lockedOn.AddMinutes(10) < DateTime.Now;
            if (!lockExpired)
            {
                lockPage = true;
                // Page.ClientScript.RegisterStartupScript(GetType(), "LockConfigPage", "showAlertDialog(\"error\",\"This page is locked by '" + lockDetails[0] + "', IP: " + lockDetails[3] + "\");", true);
            }
            else
                acquireLock = true;
        }
        else
            acquireLock = true;

        if (acquireLock)
            fdwObj.SaveInRedisServer(Constants.CONFIG_LOCK_KEY, userName + "♦" + DateTime.Now + "♦" + sessId + "♦" + ipaddress, Constants.CONFIG_LOCK_KEY, schemaName);
    }

    private string ReplacePathChar(string path)
    {
        return path.Replace("\\", "||");
    }

    private void TrimBackSlashforPathFields()
    {
        if (txtImpEmpTmpPath.Value != String.Empty && txtImpEmpTmpPath.Value.EndsWith("\\"))
        {
            txtImpEmpTmpPath.Value = txtImpEmpTmpPath.Value.Substring(0, txtImpEmpTmpPath.Value.Length - 1);
        }

        if (txtImagePath.Value != String.Empty && txtImagePath.Value.EndsWith("\\"))
        {
            txtImagePath.Value = txtImagePath.Value.Substring(0, txtImagePath.Value.Length - 1);
        }

        if (txtAttachPath.Value != String.Empty && txtAttachPath.Value.EndsWith("\\"))
        {
            txtAttachPath.Value = txtAttachPath.Value.Substring(0, txtAttachPath.Value.Length - 1);
        }

        if (txtGridAttachPath.Value != String.Empty && txtGridAttachPath.Value.EndsWith("\\"))
        {
            txtGridAttachPath.Value = txtGridAttachPath.Value.Substring(0, txtGridAttachPath.Value.Length - 1);
        }

        if (txtExePath.Value != String.Empty && txtExePath.Value.EndsWith("\\"))
        {
            txtExePath.Value = txtExePath.Value.Substring(0, txtExePath.Value.Length - 1);
        }

        if (txtPrintPath.Value != String.Empty && txtPrintPath.Value.EndsWith("\\"))
        {
            txtPrintPath.Value = txtPrintPath.Value.Substring(0, txtPrintPath.Value.Length - 1);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Session["Project"] == null)
        {
            return;
        }

        //string lockedBy = fdrObj.StringFromRedis(Constants.CONFIG_LOCK_KEY);
        //string[] lockDetails = lockedBy.Split('♦');

        //if (lockDetails.Length == 4 && lockDetails[2] != sessId)
        //{
        //    lockPage = true;
        //    Page.ClientScript.RegisterStartupScript(GetType(), "LockConfigPageOnSave", "showAlertDialog(\"error\",\"Could not save. This page is locked by '" + lockDetails[0] + "', IP: " + lockDetails[3] + "\");", true);
        //    return;
        //}

        string dirPath = Server.MapPath("~/images/Custom");
        if (UploadAppImg.PostedFile.FileName != null && UploadAppImg.PostedFile.FileName != "")
        {
            try
            {
                if (File.Exists(dirPath + "/homelogo.mp4"))
                {
                    DirectoryInfo dir = new DirectoryInfo(dirPath);
                    var file = dir.GetFiles("*homelogo.*");
                    File.Delete(dirPath + "/" + file[0].Name);
                }
                else
                {
                    DirectoryInfo dir = new DirectoryInfo(dirPath);
                    var file = dir.GetFiles("*homelogo.*");
                    if (file.Count() > 0)
                        File.Delete(dirPath + "/" + file[0].Name);
                }

                if (UploadAppImg.PostedFile.ContentType == "video/mp4")
                {

                    UploadAppImg.PostedFile.SaveAs(dirPath + "/homelogo.mp4");
                }
                else
                {
                    string ImgNameExt = UploadAppImg.PostedFile.FileName.Split('.').Last();
                    UploadAppImg.PostedFile.SaveAs(dirPath + "/homelogo." + ImgNameExt);
                }
            }
            catch (Exception ex)
            {

            }
        }
        if (UploadAppLogoImg.PostedFile.FileName != null && UploadAppLogoImg.PostedFile.FileName != "")
        {
            //string name = UploadAppLogoImg.PostedFile.FileName;
            try
            {
                // UploadAppLogoImg.PostedFile.SaveAs(dirPath + "/logo.png");
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                var file = dir.GetFiles("logo.*");
                if (file.Count() > 0)
                    File.Delete(dirPath + "/" + file[0].Name);

                //string name = UploadAppLogoImg.PostedFile.FileName.Split('.').Last();
                UploadAppLogoImg.PostedFile.SaveAs(dirPath + "/logo.png");
            }
            catch (Exception ex)
            {

            }
        }
        if (UploadAppMobImg.PostedFile.FileName != null && UploadAppMobImg.PostedFile.FileName != "")
        {
            // string name = UploadAppMobImg.PostedFile.FileName;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                var file = dir.GetFiles("*homelogo_mob.*");
                if (file.Count() > 0)
                    File.Delete(dirPath + "/" + file[0].Name);

                string name = UploadAppMobImg.PostedFile.FileName.Split('.').Last();
                UploadAppMobImg.PostedFile.SaveAs(dirPath + "/homelogo_mob.jpg");
            }
            catch (Exception ex)
            {

            }
        }

        StringBuilder newConfig = new StringBuilder();
        TrimBackSlashforPathFields();
        //NOTE: Any change in the below json should also be updated in the 'Constants DEFAULT_CONFIGSTR'
        newConfig.Append("{\"configStr\": [");
        //Application level settings        
        newConfig.Append("{\"AxLoginTrace\": \"" + hdnTrace.Value + "\"},");
        //configStr.Append("{\"AxMenuStyle\": \"" + hdnMenu.Value + "\"},");//Other value will be Modern
        newConfig.Append("{\"AxBreadCrumb\": \"" + hdnBreadcrumb.Value + "\"},");
        newConfig.Append("{\"AxEnableCards\": \"" + hdnEnableCards.Value + "\"},");
        newConfig.Append("{\"AxExportTallyTid\": \"" + hdnTallyExport.Value + "\"},");
        newConfig.Append("{\"AxCPWDOnLogin\": \"" + hdnCPOFL.Value + "\"},");
        newConfig.Append("{\"AxHelpIview\": \"" + ddlHelpIview.Value + "\"},");
        newConfig.Append("{\"AxSessionExtend\": \"" + hdnSessExt.Value + "\"},");
        newConfig.Append("{\"AxTimezoneVariation\": \"" + hdnAllowTimeZone.Value + "\"},");
        newConfig.Append("{\"AxDisableSplit\": \"" + hdnDisableSplit.Value + "\"},");
        newConfig.Append("{\"AxShowLoggedinUsersCount\": \"" + hdnActSess.Value + "\"},");
        newConfig.Append("{\"AxImpExpTemptPath\": \"" + ReplacePathChar(txtImpEmpTmpPath.Value) + "\"},");
        newConfig.Append("{\"AxGlobalSrchLimit\": \"" + txtGlobalSrchLimit.Value + "\"},");
        newConfig.Append("{\"AxAlertTimeout\": \"" + txtAlertTimeout.Value + "\"},");
        newConfig.Append("{\"AxSessionExpiryDays\": \"" + txtSessionExpiryDays.Value + "\"},");

        //error message timeout
        newConfig.Append("{\"AxErrorMsg\": \"" + hdnEnableErrorMsg.Value + "\"},");
        newConfig.Append("{\"AxErrorMsgTimeout\": \"" + txtErrorTimeout.Value + "\"},");

        newConfig.Append("{\"AxWizardType\": \"" + ddlWizardType.Value + "\"},");

        newConfig.Append("{\"AxOTPAuth\": \"" + hdnotpauth.Value + "\"},");
        newConfig.Append("{\"AxOTPAuthCahrs\": \"" + ddlotpauthcahrs.Value + "\"},");
        newConfig.Append("{\"AxOTPAuthExpiry\": \"" + ddlotpauthexpiry.Value + "\"},");

        ////Application globalization
        newConfig.Append("{\"AxLanguages\": \"" + hdnLanguage.Value + "\"},");
        newConfig.Append("{\"AxUSCulture\": \"" + hdnCulture.Value + "\"},");
        newConfig.Append("{\"AxUserLevelLang\": \"" + hdnUserLevelLang.Value + "\"},");
        //Form level settings
        newConfig.Append("{\"AxDevInstance\": \"" + hdnDevInstance.Value + "\"},");
        newConfig.Append("{\"AxDisplayAutoGenVal\": \"" + hdnAutoGen.Value + "\"},");
        newConfig.Append("{\"AxIsPerfCode\": \"" + hdnIsPerfCode.Value + "\"},");
        newConfig.Append("{\"AxDcGridOnSave\": \"" + hdnDcGridOnSave.Value + "\"},");
        newConfig.Append("{\"AxImagePath\": \"" + ReplacePathChar(txtImagePath.Value) + "\"},");
        newConfig.Append("{\"AxAttachFilePath\": \"" + ReplacePathChar(txtAttachPath.Value) + "\"},");
        newConfig.Append("{\"AxGridAttachPath\": \"" + ReplacePathChar(txtGridAttachPath.Value) + "\"},");
        newConfig.Append("{\"AxMaxDraftsCount\": \"" + txtMaxDraftsCount.Value + "\"},");

        newConfig.Append("{\"AxAttachmentSize\": \"" + ddlAttachmentSize.Value + "\"},");
        newConfig.Append("{\"AxInlineGridEdit\": \"" + (ddlInlineGrid.Value == "inline" ? "true" : "false") + "\"},");
        newConfig.Append("{\"AxShowSubmitCancel\": \"" + showSubmitCancel.Value + "\"},");
        newConfig.Append("{\"AxDesignerAccess\": \"" + hdnDesignerResp.Value + "\"},");
        newConfig.Append("{\"AxDrafts\": \"" + hdnEnableDrafts.Value + "\"},");
        newConfig.Append("{\"AxAutoPurge\": \"" + hdnAutoPurge.Value + "\"},");
        newConfig.Append("{\"AutoSavePublish\" : \"" + hdnAutoSavePublish.Value + "\"},");
        ////Print settings
        newConfig.Append("{\"AxIsPrintExe\": \"" + hdnIsPrintExe.Value + "\"},");
        newConfig.Append("{\"AxPrintExePath\": \"" + ReplacePathChar(txtExePath.Value) + "\"},");
        newConfig.Append("{\"AxHtmlPath\": \"" + ReplacePathChar(txtPrintPath.Value) + "\"},");
        newConfig.Append("{\"AxPrintMargins\": \"" + hdnPrintMargins.Value + "\"},");
        newConfig.Append("{\"AxPrintTitleAlign\": \"" + ddlTitAlignment.Value + "\"},");
        //Report level settings
        newConfig.Append("{\"AxIviewcelltextwrap\": \"" + hdnTextWrap.Value + "\"},");
        newConfig.Append("{\"AxMergeRowIviews\": \"" + txtMergeRows.Value + "\"},");
        newConfig.Append("{\"AxPrintRowsMaxLimit\": \"" + txtMaxRowsToPrint.Value + "\"},");
        newConfig.Append("{\"AxDbPagination\": \"" + hdnDbPagination.Value + "\"},");
        newConfig.Append("{\"AxDefaultPageSize\": \"" + txtPageRowCount.Value + "\"},");
        newConfig.Append("{\"AxShowAppTitle\": \"" + hdnShowAppTitle.Value + "\"},");
        newConfig.Append("{\"AxShowStripedReport\": \"" + hdnShowstripedreport.Value + "\"},");
        newConfig.Append("{\"AxGetIviewRowCount\": \"" + hdnGetIviewRowCount.Value + "\"},");
        newConfig.Append("{\"AxIviewDataWSRows\": \"" + txtIviewDataWSRows.Value + "\"},");
        //Home page setting
        newConfig.Append("{\"AxHomeBuildAccess\": \"" + hdnHomeBuildResp.Value + "\"},");
        newConfig.Append("{\"AxMaxNumOfWidgets\": \"" + txtMaxNumOfWidgets.Value + "\"},");
        //Menu setting
        newConfig.Append("{\"AxMenuStyle\": \"" + axMenuStyleSel.Value + "\"},");
        newConfig.Append("{\"AxMenuColumns\": \"" + axMenuColCount.Value + "\"},");
        newConfig.Append("{\"AxSubmenuPerView\": \"" + axMenusubCntPerView.Value + "\"},");
        newConfig.Append("{\"AxMenuWordWrap\": \"" + hdnMenuWrapText.Value + "\"},");
        newConfig.Append("{\"AxSubMenuCount\": \"" + hdnSubMenuCount.Value + "\"},");
        newConfig.Append("{\"AxDirectSubMenuCount\": \"" + hdnDirSubMenuCount.Value + "\"}");

        newConfig.Append("]");
        newConfig.Append(SetLangConfigKeys());
        newConfig.Append("}");//End of json        

        try
        {
            FDW fdwObj = new FDW();
            fdwObj.SaveInRedisServer(Constants.CONFIGAPP_JSON_KEY, newConfig.ToString(), Constants.CONFIGAPP_JSON_KEY, proj.ToLower());
            util.SaveConfigFile(proj, newConfig.ToString());
            util.SaveConfigFile(proj, savedConfigStr, true);

            UpdateApplication(newConfig);
            Application["-dPage-"] = "../aspx/Page.aspx";
            SaveUserAppSettings();

            if (logChanges)
            {
                string strChanges = GetConfigChanges(savedConfigStr, newConfig.ToString());
                if (strChanges != string.Empty && strChanges != "{\"changes\":[]}")
                {
                    //DataTable dtConfigLog = util.GetConfigLogDT();
                    string sqlInsertLog = Constants.SQL_SET_AXPWEBLOGS;
                    sqlInsertLog = sqlInsertLog.Replace("$Username$", userName);
                    sqlInsertLog = sqlInsertLog.Replace("$Logtime$", DateTime.Now.ToString());
                    sqlInsertLog = sqlInsertLog.Replace("$Type$", "Config Changes");
                    sqlInsertLog = sqlInsertLog.Replace("$Sessionid$", sessId);
                    sqlInsertLog = sqlInsertLog.Replace("$Ipaddress$", ipaddress);
                    sqlInsertLog = sqlInsertLog.Replace("$Logdetails$", strChanges);
                    DBContext obj = new DBContext();
                    obj.ExecuteSqlQueryInline(sqlInsertLog);
                }
            }

            savedConfigStr = newConfig.ToString();
            //LoadLangConfigKeys();
            LoadLangConfigKeysNew(hdnLanguage.Value);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Save", "showSuccess();", true);
        }
        catch (System.IO.IOException ex)
        {
            string errlog = logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Configuration", "", "true");
            Response.Redirect("../CusError/AxCustomError.aspx");
        }

    }

    private void GetRoleLandingPages()
    {
        string[] lndPages = hdnLandingPages.Value.Split('~');
        string[] lndRoles = hdnRoleName.Value.Split('~');

        if (lndPages.Length > 0)
        {
            for (int i = 0; i < lndPages.Length; i++)
            {
                if (lndPages[i] != string.Empty)
                    Application[lndRoles[i]] = lndPages[i];
            }
        }
    }
    private void SetDefaultLangKeys()
    {
        String strKeys = String.Empty;
        string strAppTitle = String.Empty;
        if (Session["projTitle"] != null && Session["projTitle"].ToString() != String.Empty)
            strAppTitle = Session["projTitle"].ToString();
        strKeys = "AxAppTitle:" + strAppTitle + "♦AxCopyRightText:Powered by Axpert♦AxPrintTitle:";
        hdnLangKeys.Value = strKeys;
    }

    private void LoadLangConfigKeys()
    {
        try
        {
            if (savedConfigStr != string.Empty)
            {
                JArray configLangNode = null;

                JObject objConfig = JObject.Parse(savedConfigStr);
                configLangNode = (JArray)objConfig["configLangKeys"];

                StringBuilder divHTML = new StringBuilder();

                if (configLangNode != null)
                {
                    foreach (JObject content in configLangNode.Children<JObject>())
                    {
                        foreach (JProperty prop in content.Properties())
                        {
                            string propName = prop.Name.ToString().ToUpper();
                            if (propName.StartsWith("AXAPPTITLE") && prop.Value.ToString() == String.Empty && Session["projTitle"] != null && Session["projTitle"].ToString() != String.Empty)
                            {
                                divHTML.Append("<input type='hidden' id='" + prop.Name.ToString() + "' name='" + prop.Name.ToString().Substring(prop.Name.ToString().Length - 3) + "' value='" + HttpContext.Current.Session["projTitle"].ToString() + "' />");
                            }
                            else
                                divHTML.Append("<input type='hidden' id='" + prop.Name.ToString() + "' name='" + prop.Name.ToString().Substring(prop.Name.ToString().Length - 3) + "' value='" + prop.Value.ToString() + "' />");
                        }
                    }
                }
                dvLangInputs.InnerHtml = divHTML.ToString();
                dvLangInputsNew.InnerHtml = divHTML.ToString();
            }
        }
        catch (System.IO.IOException ex)
        {
            string errlog = logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Configuration - LoadLangConfigKeys", "new", "true");
            Response.Redirect("../CusError/AxCustomError.aspx");
        }
    }

    private void LoadLangConfigKeysNew(string _strlng)
    {
        try
        {
            if (savedConfigStr != string.Empty)
            {
                JArray configLangNode = null;

                JObject objConfig = JObject.Parse(savedConfigStr);
                configLangNode = (JArray)objConfig["configLangKeys"];

                StringBuilder divHTML = new StringBuilder();

                if (configLangNode != null)
                {
                    foreach (JObject content in configLangNode.Children<JObject>())
                    {
                        foreach (JProperty prop in content.Properties())
                        {
                            string propName = prop.Name.ToString().ToUpper();
                            if (propName.StartsWith("AXAPPTITLE") && prop.Value.ToString() == String.Empty && Session["projTitle"] != null && Session["projTitle"].ToString() != String.Empty)
                            {
                                divHTML.Append("<input type='hidden' id='" + prop.Name.ToString() + "' name='" + prop.Name.ToString().Substring(prop.Name.ToString().Length - 3) + "' value='" + HttpContext.Current.Session["projTitle"].ToString() + "' />");
                            }
                            else if (propName != "AXAPPTITLE_ENG" && propName.StartsWith("AXAPPTITLE_") && propName != "AXAPPTITLE_")
                            {
                                string _first3chars = prop.Name.ToString().Substring(prop.Name.ToString().Length - 3);
                                var _theseLangs = _strlng.Split(',');
                                var filteredLanguage = _theseLangs.FirstOrDefault(lang => lang.StartsWith(_first3chars, StringComparison.OrdinalIgnoreCase));
                                if (filteredLanguage != null)
                                    divHTML.Append("<div class=\"form-group row my-2\" id=\"dvtitle" + prop.Name.ToString().Substring(prop.Name.ToString().Length - 3) + "\"><div class=\"col-md-6\"><span id=\"lbl" + propName + "\" class=\"form-check-label form-label col-form-label pb-1 fw-boldest\"> Application Title " + filteredLanguage.ToString() + " <i tabindex=\"-1\" style=\"cursor: pointer; outline: none;\" class=\"icon-arrows-question\" id=\"ico_cl\" data-toggle=\"popover\" data-content=\"Enter Application Title to overwrite the existing title.\" data-placement=\"right\" data-original-title=\"\" title=\"\" data-bs-original-title=\"\"></i></span></div><div class=\"col-md-6\"><input name=\"" + prop.Name.ToString().Substring(prop.Name.ToString().Length - 3) + "\" type=\"text\" id=\"" + prop.Name.ToString() + "\" class=\"m-wrap my-4-- placeholder-no-fix form-control \" maxlength=\"100\" value=\"" + prop.Value.ToString() + "\"></div></div>");
                                else
                                    divHTML.Append("<div class=\"form-group row my-2 d-none\" id=\"dvtitle" + prop.Name.ToString().Substring(prop.Name.ToString().Length - 3) + "\"><div class=\"col-md-6\"><span id=\"lbl" + propName + "\" class=\"form-check-label form-label col-form-label pb-1 fw-boldest\"> Application Title <i tabindex=\"-1\" style=\"cursor: pointer; outline: none;\" class=\"icon-arrows-question\" id=\"ico_cl\" data-toggle=\"popover\" data-content=\"Enter Application Title to overwrite the existing title.\" data-placement=\"right\" data-original-title=\"\" title=\"\" data-bs-original-title=\"\"></i></span></div><div class=\"col-md-6\"><input name=\"" + prop.Name.ToString().Substring(prop.Name.ToString().Length - 3) + "\" type=\"text\" id=\"" + prop.Name.ToString() + "\" class=\"m-wrap my-4-- placeholder-no-fix form-control \" maxlength=\"100\" value=\"" + prop.Value.ToString() + "\"></div></div>");
                            }
                            else
                                divHTML.Append("<input type='hidden' id='" + prop.Name.ToString() + "' name='" + prop.Name.ToString().Substring(prop.Name.ToString().Length - 3) + "' value='" + prop.Value.ToString() + "' />");
                        }
                    }
                }
                dvLangInputsNew.InnerHtml = divHTML.ToString();
            }
        }
        catch (System.IO.IOException ex)
        {
            string errlog = logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Configuration - LoadLangConfigKeys", "new", "true");
            Response.Redirect("../CusError/AxCustomError.aspx");
        }
    }

    private string SetLangConfigKeys()
    {
        string[] arrLangKeys = hdnLangVals.Value.Split('♦');
        StringBuilder configLangKeys = new StringBuilder();
        bool isNew = true;
        configLangKeys.Append(",\"configLangKeys\": [");
        foreach (string strKey in arrLangKeys)
        {
            if (isNew)
            {
                configLangKeys.Append("{\"" + strKey.Split(':')[0].ToString() + "\": \"" + strKey.Split(':')[1].ToString() + "\"}");
                isNew = false;
            }
            else
                configLangKeys.Append(",{\"" + strKey.Split(':')[0].ToString() + "\": \"" + strKey.Split(':')[1].ToString() + "\"}");
        }
        configLangKeys.Append("]");
        return configLangKeys.ToString();
    }

    private void UpdateApplication(StringBuilder createstrText)
    {
        try
        {
            Session["AxLoginTrace"] = hdnTrace.Value;
            Session["AxBreadCrumb"] = hdnBreadcrumb.Value;
            Session["AxEnableCards"] = hdnEnableCards.Value;
            Session["AxDrafts"] = hdnEnableDrafts.Value;
            Session["AxAutoPurge"] = hdnAutoPurge.Value;
            Session["AxExportTallyTid"] = hdnTallyExport.Value;
            Session["AxCPWDOnLogin"] = hdnCPOFL.Value;
            Session["AxHelpIview"] = ddlHelpIview.Value;
            Session["AxSessionExtend"] = hdnSessExt.Value;
            Session["AxTimezoneVariation"] = hdnAllowTimeZone.Value;
            Session["AxDisableSplit"] = hdnDisableSplit.Value;
            Session["AxShowLoggedinUsersCount"] = hdnActSess.Value;
            Session["AxImpExpTemptPath"] = txtImpEmpTmpPath.Value;
            Session["AxGlobalSrchLimit"] = txtGlobalSrchLimit.Value;
            Session["AxAlertTimeout"] = txtAlertTimeout.Value;
            Session["AxSessionExpiryDays"] = txtSessionExpiryDays.Value;
            // enable error messages
            Session["AxErrorMsg"] = hdnEnableErrorMsg.Value;
            Session["AxErrorMsgTimeout"] = txtErrorTimeout.Value;
            Session["AxWizardType"] = ddlWizardType.Value;

            Session["AxOTPAuth"] = hdnotpauth.Value;
            Session["AxOTPAuthCahrs"] = ddlotpauthcahrs.Value;
            Session["AxOTPAuthExpiry"] = ddlotpauthexpiry.Value;

            ////Application globalization
            Session["AxLanguages"] = hdnLanguage.Value;
            Session["AxUSCulture"] = hdnCulture.Value;
            Session["AxUserLevelLang"] = hdnUserLevelLang.Value;
            //Form level settings
            Session["AxDevInstance"] = hdnDevInstance.Value;
            Session["AxDisplayAutoGenVal"] = hdnAutoGen.Value;
            Session["AxIsPerfCode"] = hdnIsPerfCode.Value;
            Session["AxDcGridOnSave"] = hdnDcGridOnSave.Value;
            Session["AxImagePath"] = txtImagePath.Value;
            Session["AxAttachFilePath"] = txtAttachPath.Value;
            Session["AxGridAttachPath"] = txtGridAttachPath.Value;
            Session["AxMaxDraftsCount"] = txtMaxDraftsCount.Value;
            Session["AxAttachmentSize"] = ddlAttachmentSize.Value;
            Session["AxInlineGridEdit"] = (ddlInlineGrid.Value == "inline" ? "true" : "false");
            Session["AxShowSubmitCancel"] = showSubmitCancel.Value;
            Session["AxDesignerAccess"] = hdnDesignerResp.Value;
            Session["AutoSavePublish"] = hdnAutoSavePublish.Value;
            ////Print settings
            Session["AxIsPrintExe"] = hdnIsPrintExe.Value;
            Session["AxPrintExePath"] = txtExePath.Value.Replace("\\", "\\\\");
            Session["AxHtmlPath"] = txtPrintPath.Value.Replace("\\", "\\\\");
            Session["AxPrintMargins"] = hdnPrintMargins.Value;
            Session["AxPrintTitleAlign"] = ddlTitAlignment.Value;
            //Report level settings
            Session["AxIviewcelltextwrap"] = hdnTextWrap.Value;
            Session["AxMergeRowIviews"] = txtMergeRows.Value;
            Session["AxPrintRowsMaxLimit"] = txtMaxRowsToPrint.Value;
            Session["AxDbPagination"] = hdnDbPagination.Value;
            Session["AxDefaultPageSize"] = txtPageRowCount.Value;
            Session["AxShowAppTitle"] = hdnShowAppTitle.Value;
            Session["AxGetIviewRowCount"] = hdnGetIviewRowCount.Value;
            Session["AxIviewDataWSRows"] = txtIviewDataWSRows.Value;
            //Home page setting
            Session["AxHomeBuildAccess"] = hdnHomeBuildResp.Value;
            Session["AxMaxNumOfWidgets"] = txtMaxNumOfWidgets.Value;
            //Menu setting
            Session["AxMenuStyle"] = axMenuStyleSel.Value;
            Session["AxMenuColumns"] = axMenuColCount.Value;
            Session["AxSubmenuPerView"] = axMenusubCntPerView.Value;
            Session["AxMenuWordWrap"] = hdnMenuWrapText.Value;
            Session["AxSubMenuCount"] = hdnSubMenuCount.Value;
            Session["AxDirectSubMenuCount"] = hdnDirSubMenuCount.Value;
        }
        catch (Exception ex) { }
    }
    protected void btnRestore_Click()
    {
        LoadDefaultAttributes(true);
    }

    public void LoadDefaultAttributes(bool bRestore)
    {
        string path = string.Empty;
        if (Session["Project"] == null)
            Response.Redirect(util.SESSEXPIRYPATH);
        path = Server.MapPath("~/Config") + "\\" + proj + ".cfg";
        FileInfo sConfigPath = new FileInfo(path);
        if (!sConfigPath.Exists || bRestore)
        {
            //NOTE: Any change in the below Default values should also be updated in the 'Constants DEFAULT_CONFIGSTR'
            if (Session["projTitle"] != null)
                txtAppTitle.Value = Session["projTitle"].ToString();
            //Application settings
            hdnTrace.Value = "false";
            hdnBreadcrumb.Value = "false";
            hdnEnableCards.Value = "false";
            hdnEnableDrafts.Value = "false";
            hdnAutoPurge.Value = "true";
            hdnTallyExport.Value = "";
            hdnCPOFL.Value = "true";
            ddlHelpIview.SelectedIndex = 0;
            hdnSessExt.Value = "true";
            hdnActSess.Value = "false";
            txtImpEmpTmpPath.Value = "";
            txtAlertTimeout.Value = "3000";
            txtSessionExpiryDays.Value = "5";
            txtGlobalSrchLimit.Value = "5000";
            //enable error settings
            hdnEnableErrorMsg.Value = "false";
            txtErrorTimeout.Value = "0";
            ddlWizardType.SelectedIndex = 0;

            hdnotpauth.Value = "false";
            ddlotpauthcahrs.SelectedIndex = 0;
            ddlotpauthexpiry.SelectedIndex = 0;

            ////Application globalization
            hdnLanguage.Value = "english";
            hdnUserLevelLang.Value = "false";
            hdnCulture.Value = "false";
            //Form level settings
            hdnDevInstance.Value = "false";
            hdnAutoGen.Value = "false";
            hdnIsPerfCode.Value = "true";
            hdnDcGridOnSave.Value = "true";
            txtImagePath.Value = "";
            txtAttachPath.Value = "";
            txtGridAttachPath.Value = "";
            txtMaxDraftsCount.Value = "1";
            showSubmitCancel.Value = "false";
            hdnDesignerResp.Value = "designer";
            hdnAutoSavePublish.Value = "false";
            ////Print settings
            hdnIsPrintExe.Value = "true";
            txtExePath.Value = "";
            txtPrintPath.Value = "";
            hdnPrintMargins.Value = "50,50,200,100";
            ddlTitAlignment.SelectedIndex = 1;
            //Report level settings
            hdnTextWrap.Value = "true";
            txtMergeRows.Value = "";
            txtMaxRowsToPrint.Value = "10000";
            hdnDbPagination.Value = "true";
            txtPageRowCount.Value = "30";
            hdnShowAppTitle.Value = "true";
            hdnShowstripedreport.Value = "false";
            hdnInlineGridEdit.Value = "true";
            ddlInlineGrid.Value = "inline";
            hdnGetIviewRowCount.Value = "false";
            txtIviewDataWSRows.Value = "1000";
            //Home page setting
            hdnHomeBuildResp.Value = "default";
            txtMaxNumOfWidgets.Value = "99";
            //LoadLanghdnIsPerfCodeuages();
            //Menu Settings
            axMenuStyleSel.SelectedIndex = 0;
            axMenuColCount.SelectedIndex = 2;
            axMenusubCntPerView.Value = "6";
            hdnMenuWrapText.Value = "false";
            hdnSubMenuCount.Value = "4";
            hdnDirSubMenuCount.Value = "6";
        }
        else
        {
            GetSavedConfiguration();
        }
        //if (!bRestore)
        //    ScriptManager.RegisterStartupScript(this, this.btnRestore.GetType(), "ReStore", "disableElements();", true);
    }

    private void GetSavedConfiguration()
    {

        if (HttpContext.Current.Session["nsessionid"] != null)
        {
            try
            {
                //Application settings
                hdnTrace.Value = Session["AxLoginTrace"].ToString().ToLower();
                hdnBreadcrumb.Value = Session["AxBreadCrumb"].ToString().ToLower();
                hdnEnableCards.Value = Session["AxEnableCards"].ToString().ToLower();
                hdnEnableDrafts.Value = Session["AxDrafts"].ToString().ToLower();
                hdnAutoPurge.Value = Session["AxAutoPurge"].ToString().ToLower();
                hdnTallyExport.Value = Session["AxExportTallyTid"].ToString().ToLower();
                hdnCPOFL.Value = Session["AxCPWDOnLogin"].ToString().ToLower();
                ddlHelpIview.Value = Session["AxHelpIview"].ToString();
                hdnSessExt.Value = Session["AxSessionExtend"].ToString().ToLower();
                hdnAllowTimeZone.Value = Session["AxTimezoneVariation"].ToString().ToLower();
                hdnDisableSplit.Value = Session["AxDisableSplit"].ToString().ToLower();
                hdnActSess.Value = Session["AxShowLoggedinUsersCount"].ToString().ToLower();
                txtImpEmpTmpPath.Value = Session["AxImpExpTemptPath"].ToString();
                txtAlertTimeout.Value = Session["AxAlertTimeout"].ToString();
                txtSessionExpiryDays.Value = Session["AxSessionExpiryDays"].ToString();

                hdnotpauth.Value = Session["AxOTPAuth"].ToString().ToLower();
                ddlotpauthcahrs.Value = Session["AxOTPAuthCahrs"].ToString();
                ddlotpauthexpiry.Value = Session["AxOTPAuthExpiry"].ToString();

                if (Session["AxGlobalSrchLimit"] != null)
                    txtGlobalSrchLimit.Value = Session["AxGlobalSrchLimit"].ToString();
                else
                    txtGlobalSrchLimit.Value = "5000";
                //enable error timeout
                if (Session["AxErrorMsg"] == null && Session["AxErrorMsgTimeout"] == null)
                {
                    hdnEnableErrorMsg.Value = "false";
                    txtErrorTimeout.Value = "0";
                }
                else
                {
                    hdnEnableErrorMsg.Value = Session["AxErrorMsg"].ToString();
                    txtErrorTimeout.Value = Session["AxErrorMsgTimeout"].ToString();
                }

                ddlWizardType.Value = Session["AxWizardType"].ToString();
                ////Application globalization
                hdnLanguage.Value = Session["AxLanguages"].ToString().ToLower();
                hdnCulture.Value = Session["AxUSCulture"].ToString().ToLower();
                hdnUserLevelLang.Value = Session["AxUserLevelLang"].ToString().ToLower();
                //Form level settings
                hdnDevInstance.Value = Session["AxDevInstance"].ToString().ToLower();
                hdnAutoGen.Value = Session["AxDisplayAutoGenVal"].ToString().ToLower();
                hdnIsPerfCode.Value = Session["AxIsPerfCode"].ToString().ToLower();
                hdnDcGridOnSave.Value = Session["AxDcGridOnSave"].ToString().ToLower();
                txtImagePath.Value = Session["AxImagePath"].ToString();
                txtAttachPath.Value = Session["AxAttachFilePath"].ToString();
                txtGridAttachPath.Value = Session["AxGridAttachPath"].ToString();
                txtMaxDraftsCount.Value = Session["AxMaxDraftsCount"].ToString();

                if (Session["AxInlineGridEdit"] == null)
                {
                    ddlInlineGrid.Value = "inline";
                    Session["AxInlineGridEdit"] = "true";
                }
                else
                    ddlInlineGrid.Value = Session["AxInlineGridEdit"].ToString() == "false" ? "popup" : "inline";

                showSubmitCancel.Value = Session["AxShowSubmitCancel"].ToString();
                hdnDesignerResp.Value = Session["AxDesignerAccess"].ToString();
                hdnAutoSavePublish.Value = Session["AutoSavePublish"] != null && Session["AutoSavePublish"].ToString() != "" ? Session["AutoSavePublish"].ToString().ToLower() : "false";
                ////Print settings
                hdnIsPrintExe.Value = Session["AxIsPrintExe"].ToString().ToLower();
                txtExePath.Value = Session["AxPrintExePath"].ToString().Replace("\\\\", "\\");
                txtPrintPath.Value = Session["AxHtmlPath"].ToString().Replace("\\\\", "\\");
                hdnPrintMargins.Value = Session["AxPrintMargins"].ToString();
                ddlTitAlignment.Value = Session["AxPrintTitleAlign"].ToString();
                //Report level settings
                hdnTextWrap.Value = Session["AxIviewcelltextwrap"].ToString().ToLower();
                txtMergeRows.Value = Session["AxMergeRowIviews"].ToString();
                txtMaxRowsToPrint.Value = Session["AxPrintRowsMaxLimit"].ToString();
                hdnDbPagination.Value = Session["AxDbPagination"].ToString().ToLower();
                txtPageRowCount.Value = Session["AxDefaultPageSize"].ToString();
                hdnShowAppTitle.Value = Session["AxShowAppTitle"].ToString().ToLower();
                hdnShowstripedreport.Value = Session["AxShowStripedReport"].ToString().ToLower();
                hdnGetIviewRowCount.Value = Session["AxGetIviewRowCount"] != null ? Session["AxGetIviewRowCount"].ToString().ToLower() : "false";
                txtIviewDataWSRows.Value = (Session["AxIviewDataWSRows"] != null && Session["AxIviewDataWSRows"].ToString() != string.Empty) ? Session["AxIviewDataWSRows"].ToString() : "1000";
                //Home page setting
                hdnHomeBuildResp.Value = Session["AxHomeBuildAccess"].ToString();
                txtMaxNumOfWidgets.Value = Session["AxMaxNumOfWidgets"].ToString();
                //Menu setting
                axMenuStyleSel.Value = Session["AxMenuStyle"].ToString();
                axMenuColCount.Value = Session["AxMenuColumns"].ToString();
                axMenusubCntPerView.Value = Session["AxSubmenuPerView"].ToString();
                hdnMenuWrapText.Value = Session["AxMenuWordWrap"].ToString();
                hdnSubMenuCount.Value = Session["AxSubMenuCount"].ToString();
                hdnDirSubMenuCount.Value = Session["AxDirectSubMenuCount"].ToString();

                if (Session["AxAttachmentSize"] == null)
                {
                    ddlAttachmentSize.Value = "1";
                    Session["AxAttachmentSize"] = "1";
                }
                else
                    ddlAttachmentSize.Value = Session["AxAttachmentSize"].ToString();
            }
            catch (Exception ex)
            {
                logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Configuration-Exc", "new");
            }
        }
    }

    private void LoadLanguages()
    {
        strlng = objWeb.GetLanguage(proj, "Config");
        //txtlanguage.Value = strlng;
    }

    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string LoadUserAppSettings()
    {
        string result = String.Empty;
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string sql = string.Empty;
        try
        {
            sql = Constants.SQL_LOAD_USER_APPSETTINGS;
            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());
            }
            result = objCWbSer.GetChoicesCustomWS("", sql);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }

    private void SaveUserAppSettings()
    {
        string ress = hdnUserSettings.Value;
        string result = String.Empty;
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string sql = string.Empty;
        try
        {
            sql = Constants.SQL_SAVE_USER_APPSETTINGS;
            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());
                sql = sql.Replace("$VALUE$", ress);
            }
            result = objCWbSer.GetChoicesCustomWS("", sql);
        }
        catch (Exception ex)
        {
            string errlog = logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Configuration - SaveUserAppSettings", "", "true");
            Response.Redirect("../CusError/AxCustomError.aspx");
        }
    }
    public static string Getcultureinfo(string language)
    {
        CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
        for (int i = 0; i <= 732; i++)
        {
            if (cinfo[i].DisplayName == language)
            {
                return cinfo[i].Name;
            }
        }
        return string.Empty;
    }

    private string GetAxLangSourceVals()
    {
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string result = objCWbSer.GetChoicesCustomWS("", Constants.SQL_GET_AXLANGSOURCE);
        XmlDocument doc = new XmlDocument();
        if (result != "SESSION_TIMEOUT")
        {
            doc.LoadXml(result.ToLower());
            string AxLang = string.Empty;
            XmlNodeList xml = doc.SelectNodes("//row//langname");
            if (xml.Count != 0)
            {
                int i = 0;
                foreach (XmlNode node in xml)
                {
                    AxLang += xml[i].InnerText + ",";
                    i++;
                }
                if (AxLang.Length > 0)
                {
                    AxLang = AxLang.Substring(0, AxLang.Length - 1);
                }

            }
            else
            {
                AxLang = "";
            }
            return AxLang;

        }
        return string.Empty;
    }

    public string GetConfigChanges(string savedConfigStr, string newConfigStr)
    {

        JArray oldConfigNode = null;
        JObject objOldConfig = JObject.Parse(savedConfigStr);
        oldConfigNode = (JArray)objOldConfig["configStr"];

        JArray newConfigNode = null;
        JObject objNewConfig = JObject.Parse(newConfigStr);
        newConfigNode = (JArray)objNewConfig["configStr"];

        StringBuilder strChanges = new StringBuilder();
        strChanges.Append("{\"changes\":[");

        StringBuilder strFldChanges = new StringBuilder();

        if (oldConfigNode != null && newConfigNode != null)
        {
            strFldChanges.Append(GetChangedFields(savedConfigStr, oldConfigNode, newConfigNode));
        }

        oldConfigNode = (JArray)objOldConfig["configLangKeys"];
        newConfigNode = (JArray)objNewConfig["configLangKeys"];

        if (oldConfigNode != null && newConfigNode != null)
        {
            strFldChanges.Append(GetChangedFields(savedConfigStr, oldConfigNode, newConfigNode));
        }

        strChanges.Append(strFldChanges.ToString().Trim(','));
        strChanges.Append("]}");

        return strChanges.ToString();
    }

    public string GetChangedFields(string savedConfigStr, JArray oldConfigNode, JArray newConfigNode)
    {
        StringBuilder strChanges = new StringBuilder();
        foreach (JObject content in newConfigNode.Children<JObject>())
        {
            foreach (JProperty prop in content.Properties())
            {
                if (savedConfig.ContainsKey(prop.Name) && Convert.ToString(savedConfig[prop.Name]) != prop.Value.ToString())
                {
                    string propName = prop.Name.ToUpper();
                    if ((propName == "AXIMPEXPTEMPTPATH" || propName == "AXIMAGEPATH" || propName == "AXATTACHFILEPATH" || propName == "AXGRIDATTACHPATH" || propName == "AXPRINTEXEPATH" || propName == "AXHTMLPATH"))
                    {
                        if (Convert.ToString(savedConfig[prop.Name]) != prop.Value.ToString().Replace("||", "\\"))
                            strChanges.Append("{\"key\":\"" + prop.Name + "\",\"oldVal\":\"" + savedConfig[prop.Name].Replace("\\", "||") + "\",\"newVal\":\"" + prop.Value.ToString().Replace("\\", "||") + "\"},");
                    }
                    else
                        strChanges.Append("{\"key\":\"" + prop.Name + "\",\"oldVal\":\"" + savedConfig[prop.Name] + "\",\"newVal\":\"" + prop.Value.ToString() + "\"},");
                }
                else if (!savedConfig.ContainsKey(prop.Name))
                {
                    strChanges.Append("{\"key\":\"" + prop.Name + "\",\"oldVal\":\"\",\"newVal\":\"" + prop.Value.ToString() + "\"},");
                }
            }
        }
        return strChanges.ToString();
    }

    [WebMethod]
    public static void SesstingExceptionLog(string errorMsgs)
    {
        try
        {
            string _userName = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
                _userName = HttpContext.Current.Session["username"].ToString();
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("UserName:" + _userName + " Exception:" + errorMsgs, "ApplicationError/" + HttpContext.Current.Session["nsessionid"].ToString(), "SesstingExceptionLog", "", "true");
        }
        catch (Exception ex) { }
    }
}
