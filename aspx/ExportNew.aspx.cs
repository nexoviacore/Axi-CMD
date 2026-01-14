using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Collections;
using CacheMgr;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class aspx_ExportNew : System.Web.UI.Page
{
    LogFile.Log logobj = new LogFile.Log();
    string action = string.Empty;
    StringBuilder strAutoComArray = new StringBuilder();
    ArrayList arrTstructs = new ArrayList();
    ArrayList arrMapFlds = new ArrayList();
    ArrayList arrMapDcNos = new ArrayList();
    ArrayList arrMapIsDc = new ArrayList();
    ArrayList arrDcData = new ArrayList();
    Dictionary<string, string> dictFlds = new Dictionary<string, string>();
    ArrayList arrGridDcs = new ArrayList();
    ArrayList arrAllDcsData = new ArrayList();
    Dictionary<string, string> multiselect = new Dictionary<string, string>();
    Util.Util util = new Util.Util();
    LogFile.Log logObj = new LogFile.Log();
    public StringBuilder strFldArrays = new StringBuilder();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();

    string proj = string.Empty;
    string sid = string.Empty;
    string transid = string.Empty;
    public string direction = "ltr";
    public string langType = "en";
    bool isValidSession = false;
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
        util.IsValidSession();
        if (Session["project"] == null)
        {
            SessExpires();
            return;
        }
        else if (!util.CheckValidLogin())
        {
            SessExpires();
            return;
        }
        ResetSessionTime();
        GetGlobalVariables();
        HtmlLink Link = FindControl("generic") as HtmlLink;
        if (Request.UrlReferrer != null)
        {
            if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("mainnew.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("exportnew.aspx")))
                Response.Redirect("../aspx/CloudErrorPage.aspx?errmsg=error");
        }

        if (!IsPostBack)
        {
            CheckDesignAccess();
            if (Request.QueryString["action"] != null)
            {
                if (!util.IsChar(Request.QueryString["action"].ToString()))
                    Response.Redirect(Constants.PARAMERR);
                action = Request.QueryString["action"].ToString();
                //Get the Transactions to fill
                ViewState["ExImpTStructs"] = GetAllTStructs();
                //Session["ExImpTStructs"] = GetAllTStructs();
                if (action == "export")
                {
                    AddTstructs(ddlExTstruct);
                }
            }
        }
        if (ViewState["arrDcData"] != null)
            arrDcData = (ArrayList)ViewState["arrDcData"];
        if (ViewState["arrMapDcNos"] != null)
            arrMapDcNos = (ArrayList)ViewState["arrMapDcNos"];
        if (ViewState["dictFlds"] != null)
            dictFlds = (Dictionary<string, string>)ViewState["dictFlds"];
        if (ViewState["transid"] != null)
            transid = (string)ViewState["transid"];


        if (ConfigurationManager.AppSettings["isCloudApp"] != null)
        {
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString()); ;
        }
        Page.ClientScript.RegisterStartupScript(GetType(), "set global var in iview", "<script>var isCloudApp = '" + isCloudApp.ToString() + "';</script>");
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }

    //general
    private void GetTStructs()
    {
        ddlExTstruct.Items.Add(new ListItem(Constants.EMPTYOPTION, "NA"));
        ViewState["menubreadcrumb"] = "";
        //string strv = Session["ExImpTStructs"].ToString();
        string strv = ViewState["ExImpTStructs"].ToString();
        XmlDocument xmlDoc1 = new XmlDocument();
        xmlDoc1.LoadXml(strv);

        XmlNodeList xmlChild = xmlDoc1.GetElementsByTagName("row");
        string tstructName = string.Empty, tstructCaption = string.Empty;
        for (int i = 0; i <= xmlChild.Count - 1; i++)
        {
            tstructName = xmlChild[i].ChildNodes.Item(0).InnerText.Trim();
            tstructCaption = xmlChild[i].ChildNodes.Item(1).InnerText.Trim();
            arrTstructs.Add(tstructCaption + " $" + tstructName + "$");
        }
    }

    private void AddTstructs(DropDownList ddl)
    {
        ddlExTstruct.Items.Add(new ListItem(Constants.EMPTYOPTION, "NA"));
        //if (Session["ExImpTStructs"] == null)
        //    Session["ExImpTStructs"] = GetAllTStructs();
        //string strv = Session["ExImpTStructs"].ToString();
        if (ViewState["ExImpTStructs"] == null)
            ViewState["ExImpTStructs"] = GetAllTStructs();
        string strv = ViewState["ExImpTStructs"].ToString();
        XmlDocument xmlDoc1 = new XmlDocument();
        xmlDoc1.LoadXml(strv);

        XmlNodeList xmlChild = xmlDoc1.GetElementsByTagName("row");
        string tstructName = string.Empty, tstructCaption = string.Empty;
        if (xmlChild.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertNoTstruct", "showAlertDialog(\"warning\",eval(callParent('lcm[259]')));", true);
        }
        else
        {
            for (int i = 0; i < xmlChild.Count; i++)
            {
                tstructName = xmlChild[i].ChildNodes.Item(0).InnerText.Trim();
                tstructCaption = xmlChild[i].ChildNodes.Item(1).InnerText.Trim();
                ListItem lst = new ListItem();
                lst.Text = tstructCaption;
                lst.Value = tstructName;
                ddl.Items.Add(lst);
            }
        }
    }

    /// <summary>
    /// If Tstruct selection has changed then get all fields & bind it to the Multiselect control using ASP.NET Repeator control
    /// </summary>
    /// <param name="transid"></param>
    private void GetFields(string transid)
    {
        hdnDateFields.Value = string.Empty;
        ListItemCollection lstFilterOpts = new ListItemCollection();
        //take the old transid session & update session id with new transid once get the Tstruct def re-assign the old session value
        var temp = Session["transid"];
        Session["transid"] = transid;
        TStructDef strObj = util.GetTstructDefObj("Get structure for", transid.Trim());
        Session["transid"] = temp;
        bool dateFieldExists = false;
        int oldDcNo = 0;
        strAutoComArray.Append("var arrItems = [");
        dictFlds = new Dictionary<string, string>();
        StringBuilder strDcFlds = new StringBuilder();
        for (int i = 0; i < strObj.flds.Count; i++)
        {
            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)strObj.flds[i];

            if (fld.fldframeno != oldDcNo)
            {
                TStructDef.DcStruct dc = (TStructDef.DcStruct)strObj.dcs[fld.fldframeno - 1];
                arrMapFlds.Add(dc.caption + " DC");
                arrDcData.Add(dc.caption + " DC");
                strAutoComArray.Append("\"" + dc.caption + " DC" + "\",");
                arrMapDcNos.Add(dc.frameno);
                arrAllDcsData.Add(dc.caption + " DC");
                arrMapIsDc.Add("T");
                if (dc.isgrid)
                    arrGridDcs.Add(dc.frameno);

                if (strDcFlds.ToString() != string.Empty)
                {
                    dictFlds.Add(oldDcNo.ToString(), strDcFlds.ToString().Substring(0, strDcFlds.ToString().Length - 1));
                    strDcFlds.Length = 0;
                }
                oldDcNo = dc.frameno;
            }

            if (fld.savevalue && !fld.name.StartsWith("axp_recid") && fld.datatype.ToLower() != "image" && !fld.visibility)
            {
                arrMapFlds.Add(fld.caption + " (" + fld.name + ")");
                strAutoComArray.Append("\"" + fld.caption + " (" + fld.name + ")" + "\",");
                arrMapDcNos.Add(fld.fldframeno);
                arrMapIsDc.Add("F");
                strDcFlds.Append(fld.caption + " (" + fld.name + ")" + ",");
                // multiselect.Add(fld.name + "&&" + fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")", fld.caption + "(" + fld.name + ")");
                multiselect.Add(fld.caption + " (" + fld.name + ")", fld.caption + "(" + fld.name + ")");
                lstFilterOpts.Add(new ListItem(fld.caption, fld.name));//+ "(" + fld.name + ")"

                if (fld.datatype == "Date/Time")
                {
                    hdnDateFields.Value += fld.name + ",";
                    if (!dateFieldExists)
                        dateFieldExists = true;
                }
            }


        }

        if (dateFieldExists)
            hdnDateFields.Value = hdnDateFields.Value.Substring(0, hdnDateFields.Value.Length - 1);
        else
            hdnDateFields.Value = string.Empty;

        if (strDcFlds.ToString() != string.Empty)
        {
            dictFlds.Add(oldDcNo.ToString(), strDcFlds.ToString().Substring(0, strDcFlds.ToString().Length - 1));
            strDcFlds.Length = 0;
        }
        strAutoComArray.Append("];");

        ViewState["arrDcData"] = arrDcData;
        ViewState["arrMapDcNos"] = arrMapDcNos;
        ViewState["dictFlds"] = dictFlds;
        ViewState["transid"] = transid;
        if (multiselect != null && multiselect.Count > 0)
        {
            rptSelectFields.DataSource = multiselect.Keys;
            rptSelectFields.DataBind();
        }
        else
        {
            rptSelectFields.DataSource = "";
            rptSelectFields.DataBind();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertImportTstruct", "showAlertDialog(\"warning\",eval(callParent('lcm[260]')));", true);
        }
        ddlFilter.DataSource = lstFilterOpts;
        ddlFilter.DataTextField = "Text";
        ddlFilter.DataValueField = "Value";
        ddlFilter.DataBind();
        ddlFilter.Items.Insert(0, new ListItem(Constants.EMPTYOPTION, "NA"));
        ddlFilter.SelectedIndex = 0;
    }

    //private void ConstructFldStr(string fieldName)
    //{
    //    strFldArrays.Append("<script type='text/javascript'>");
    //    for (int i = 0; i < arrMapFlds.Count; i++)
    //    {
    //        strFldArrays.Append("arrFlds[\"" + i + "\"]=" + "\"" + arrMapFlds[i].ToString() + "\";");
    //        strFldArrays.Append("arrFldDcNo[\"" + i + "\"]=" + "\"" + arrMapDcNos[i].ToString() + "\";");
    //        strFldArrays.Append("arrIsDc[\"" + i + "\"]=" + "\"" + arrMapIsDc[i].ToString() + "\";");
    //    }

    //    for (int j = 0; j < arrGridDcs.Count; j++)
    //    {
    //        strFldArrays.Append("arrGridDcs[\"" + j + "\"]=" + "\"" + arrGridDcs[j].ToString() + "\";");
    //    }

    //    for (int k = 0; k < arrAllDcsData.Count; k++)
    //    {
    //        strFldArrays.Append("arrAllDcsData[\"" + k + "\"]=" + "\"" + arrAllDcsData[k].ToString() + "\";");
    //    }
    //    strFldArrays.Append(strAutoComArray.ToString() + "availableTags = arrItems;");

    //    strFldArrays.Append("</script>");
    //}

    private string CheckSpecialChars(string str)
    {
        //hack: The below line is used to make sure that the & in &amp; is not converted inadvertantly
        //      for other chars this scenario will not come as it does not contains the same char.
        str = Regex.Replace(str, "&amp;", "&");
        str = Regex.Replace(str, "&quot;", "“");
        str = Regex.Replace(str, "\n", "<br>");
        str = Regex.Replace(str, "&", "&amp;");
        str = Regex.Replace(str, "<", "&lt;");
        str = Regex.Replace(str, ">", "&gt;");
        str = Regex.Replace(str, "'", "&apos;");
        str = Regex.Replace(str, "\"", "&quot;");
        str = Regex.Replace(str, "’", "&apos;");
        str = Regex.Replace(str, "“", "&quot;");
        str = Regex.Replace(str, "”", "&quot;");
        str = Regex.Replace(str, "™", "&#8482;");
        str = Regex.Replace(str, "®", "&#174;");

        str = str.Replace((char)160, ' ');

        return str;
    }

    private void SessExpires()
    {
        string url = util.SESSEXPIRYPATH;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "SessExpiresMessage", "parent.parent.location.href='" + url + "'", true);
    }

    private void GetGlobalVariables()
    {
        proj = Session["project"].ToString();
        proj = util.CheckSpecialChars(proj);
        sid = Session["nsessionid"].ToString();
        sid = util.CheckSpecialChars(sid);
    }

    //Export
    protected void ddlExTstruct_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selText = ddlExTstruct.SelectedItem.Text;
        if (selText != Constants.EMPTYOPTION)
        {
            if (selText != string.Empty)
            {
                transid = ddlExTstruct.SelectedValue;
                GetFields(transid);
            }
        }
        else
        {
            rptSelectFields.DataSource = "";
            rptSelectFields.DataBind();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        lnkExpFile.NavigateUrl = "";
        lnkExpFile.Text = "";
        lblSuccess.Text = "";

        string qcsv = string.Empty, filename = string.Empty, fileType = string.Empty, strMapFields = string.Empty;
        if (chkExWithQuotes.Checked)
            qcsv = "yes";
        else
            qcsv = "no";

        fileType = ddlExFileType.SelectedValue;
        strMapFields = hdnColValues.Value;
        filename = txtExpFileName.Text.Trim();
        //to replace special characters from the file name
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            filename = filename.Replace(c, '_');

        //to replace space characters from the file name with underscore 
        foreach (char c in filename.ToCharArray())
            if (c == 32)
                filename = filename.Replace(c, '_');

        string errorLog = logObj.CreateLog("Call ExportData.", Session["nsessionid"].ToString(), "ExportData-" + transid + "", "new");
        StringBuilder ipXml = new StringBuilder();
        ipXml.Append("<testroot sessionid='" + sid + "' axpapp='" + proj + "' transid='" + transid.Trim() + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' type='" + fileType + "' ");
        ipXml.Append("trace='" + errorLog + "' scriptpath='" + util.ScriptsPath + "' filename='" + filename + "' ");
        ipXml.Append("sep='" + CheckSpecialChars(ddlExSeparator.SelectedValue) + "' qcsv='" + qcsv + "' withheader='" + chkWithHeader.Checked.ToString().ToLower() + "'>");
        ipXml.Append("<map>" + CheckSpecialChars(strMapFields) + "</map>");
        ipXml.Append(hdnCondString.Value + Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</testroot>");
        string res = string.Empty;
        try
        {
            res = objWebServiceExt.CallExportData(ipXml.ToString());
        }
        catch (Exception ex)
        {
            Response.Redirect(util.errorString);
        }
        ParseResult(res, "export");
    }

    private void ParseResult(string result, string calledFrom)
    {
        if (result != string.Empty)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(result);

                XmlNode root = xmlDoc.ChildNodes[0];
                string fileName = string.Empty;
                string filePath = string.Empty;
                string err = string.Empty;
                string expCount = string.Empty;
                //sample <result>
                //<filename>test.txtText</filename><filepath>C:\inetpub\wwwroot\scriptsperf\32kkfe2kqamm3d55mpl4tq55\</filepath><expcount>6</expcount></result>
                foreach (XmlNode chNode in root)
                {
                    if (chNode.Name == "filename")
                        fileName = chNode.InnerText;
                    else if (chNode.Name == "filepath")
                        filePath = chNode.InnerText;
                    else if (chNode.Name == "imperr" || chNode.Name == "experr")
                        err = chNode.InnerText;
                    else if (chNode.Name == "expcount")
                        expCount = chNode.InnerText;
                }


                filePath = filePath + fileName;

                if (err == Constants.SESSIONERROR || err == Constants.SESSIONEXPMSG)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SessExpiresMessage", "parent.parent.location.href='../aspx/CloudErrorPage.aspx?errmsg=" + Constants.SESSIONERROR + "'", true);
                }
                else if (err != string.Empty && fileName == string.Empty)
                {
                    if (calledFrom == "export")
                    {
                        if (err.Contains(Constants.SESSIONEXPMSG))
                        {
                            Session.RemoveAll();
                            Session.Abandon();
                            SessExpires();
                        }
                        else
                        {
                            lnkExpFile.Visible = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FileAlertErrorMessage", "showAlertDialog(\"warning\",\"" + err + "\");$(\"#pSucc\").attr(\"class\",\"hide\");", true);
                        }
                    }
                }
                else if (err != string.Empty)
                {
                    lnkExpFile.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FileAlertErrorMessage", "showAlertDialog('warning', 4032, 'client');$(\"#pSucc\").attr(\"class\",\"hide\");", true);
                }
                else
                {

                    if (expCount != "" && expCount != "0")
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(filePath);

                        //set appropriate headers
                        if (file.Exists)
                        {
                            if (calledFrom == "export")
                            {
                                string alertMessg = string.Empty, msg = string.Empty;
                                if (err != string.Empty)
                                    alertMessg = "$('#lblSuccess').html(eval(callParent('lcm[237]')) + " + expCount + "+'<br/>' +eval(callParent('lcm[236]'))+" + err + ");";
                                else
                                    alertMessg = "$('#lblSuccess').text(eval(callParent('lcm[237]')) + " + expCount + ");";
                                lnkExpFile.NavigateUrl = "openfile.aspx?fpath=" + fileName + "&Imp=t";
                                string fileDownloadicon = "file_download";
                                string fileDownloadiconmsg = "Download";
                                lnkExpFile.Text = "<span class=\"material-icons\" id=\"ico\">" + fileDownloadicon + "</span>" + fileDownloadiconmsg + " ";
                                //"<span class=\"glyphicon glyphicon-download-alt icon-basic-download\" id=\"ico\"></span> Download";
                                lnkExpFile.Visible = true;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "FileAlertErrorMessage", alertMessg + "$(\"#pSucc\").attr(\"class\",\"\");", true);
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "FileAlertErrorMessage", "showAlertDialog('success',\"" + alertMessg +"\");", true);
                            }
                        }
                        else
                        {

                            lnkExpFile.Visible = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FileAlertErrorMessage", "showAlertDialog('warning', 4032, 'client');", true);//$(\"#pSucc\").attr(\"class\",\"hide\");wizardTabFocus('wizardPrevbtn', 'wizardCompbtn');

                        }
                    }
                    else
                    {
                        lnkExpFile.Visible = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FileAlertErrorMessage", "showAlertDialog('warning', 5004, 'client');", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "FileAlertErrorMessage", "showAlertDialog(\"warning\",4033,\"client\");$(\"#pSucc\").attr(\"class\",\"hide\");wizardTabFocus('wizardPrevbtn', 'wizardCompbtn');", true);
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "StopDimmer", "ShowDimmer(false);", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SessExpiresMessage", "parent.parent.location.href='../aspx/CloudErrorPage.aspx?errmsg=" + ex.Message + "'", true);
                //Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "/aspx/CloudErrorPage.aspx?errmsg=" + ex.Message);
            }

        }
    }

    public string GetAllTStructs()
    {
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string username = Session["user"].ToString();
        string query = string.Format(Constants.IMPEXP_GETTSTRUCTS, username);
        string result = objCWbSer.GetChoicesCustomWS("", query);
        if (result.Contains(Constants.SESSIONEXPMSG))
        {
            SessExpires();
        }
        return result;
    }

    private void CheckDesignAccess()
    {
        try
        {
            if (Session["axDesign"] == null)
            {
                Session["axDesign"] = "false";
                string user = Session["user"].ToString();
                if (HttpContext.Current.Session["AxResponsibilities"] != null && HttpContext.Current.Session["AxDesignerAccess"] != null)
                {
                    if (user.ToLower() == "admin")
                        Session["axDesign"] = "true";
                    else
                    {
                        string[] arrAxResp = HttpContext.Current.Session["AxResponsibilities"].ToString().ToLower().Split(',');
                        string[] arrAxDesignerResp = HttpContext.Current.Session["AxDesignerAccess"].ToString().ToLower().Split(',');
                        foreach (string designerResp in arrAxDesignerResp)
                        {
                            if (arrAxResp.Contains(designerResp))
                            {
                                Session["axDesign"] = "true";
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Import data - CheckDesignAccess -" + ex.Message, HttpContext.Current.Session.SessionID, "CheckDesignAccess", "new");
        }
    }
}
