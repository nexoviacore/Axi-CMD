using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.IO;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.UI.HtmlControls;

public partial class Workflow : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    LogFile.Log logobj = new LogFile.Log();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    public StringBuilder strWorkflow = new StringBuilder();
    public StringBuilder strFieldTypes = new StringBuilder();
    string proj = string.Empty;
    string sid = string.Empty;
    string user = string.Empty;
    string language = string.Empty;
    string identify = "";
    string[] identification;
    string name = "";
    string[] wfname;
    string name1 = "";
    string[] wfname1;
    string tstructname = "";
    string[] tstname;
    string tstructname1 = "";
    string[] tstname1;
    string[] rolename;
    public StringBuilder strJs = new StringBuilder();
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
        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = util.toggleTheme();

        if (Session["project"] == null)
        {
            String url = util.SESSEXPIRYPATH;
            Response.Write("<script>");
            Response.Write("parent.parent.location.href='" + url + "';");
            Response.Write("</script>");
        }
        else
        {
            if (Request.UrlReferrer != null)
            {
                if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("main.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("workflow.aspx")))
                    Response.Redirect("../cusError/axcustomerror.aspx");
            }
            SetGlobalVariables();

            if (!IsPostBack)
            {
                LoadWorkflowData();
            }
        }
    }

    protected void grdWfList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataView dv = new DataView();
        DataSet ds = new DataSet();
        ds = (DataSet)Session["dsWf"];
    }

    protected void ddlTst_SelectedIndexChanged(object sender, EventArgs e)
    {
        //get the columns and fill.
        GetFilterColumns();
        GetSubTypes();
        lblCond.Text = "";
        this.ClientScript.RegisterStartupScript(GetType(), "showdimmer", "<script type=\"text/javascript\" language=\"javascript\">ShowDimmer(false);</script>");
    }

    private void GetSubTypes()
    {
        string strQuery = "";
        string fileName = "GetSubType";
        string errorLog = logobj.CreateLog("LoadWorkFlowPage.", sid, fileName, "new");
        string transId = ddlTst.SelectedValue;
        strQuery = "select subtype from AxAttachWorkflow where transid='" + transId + "'";
        string inputXml = "<sqlresultset axpapp='" + proj + "' sessionid='" + sid + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
        inputXml += "<sql>" + strQuery + "</sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";

        string res = string.Empty;
        //Call service
        res = objWebServiceExt.CallGetChoiceWS(transId, inputXml);
        if (res.Contains("<error>") && res.Contains(Constants.SESSIONEXPMSG))
        {
            Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG);
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();
        XmlNodeList sTypeNodes = default(XmlNodeList);
        xmlDoc.LoadXml(res);

        sTypeNodes = xmlDoc.SelectNodes("/sqlresultset/response/row");
        ddlSubType.Items.Clear();
        ddlSubType.Items.Add("");
        hdnLoadSubType.Value = "";
        hdnSubTypeDet.Value = "";

        for (int i = 0; i < sTypeNodes.Count; i++)
        {
            string subType = "";
            foreach (XmlNode subNode in sTypeNodes[i].ChildNodes)
            {
                if (subNode.Name.ToLower() == "subtype")
                {
                    subType = subNode.InnerText;
                    break;
                }
            }

            ddlSubType.Items.Add(subType);
            if (sTypeNodes.Count == 1 && subType == "none")
            {
                ddlSubType.SelectedIndex = 1;
                GetWorkflowDetails(ddlTst.SelectedValue, ddlSubType.SelectedValue);
                hdnLoadSubType.Value = "true";
                hdnSubTypeDet.Value = hdnLvlValues.Value;
            }
        }
    }

    protected void btnSubType_Click(object sender, EventArgs e)
    {

    }

    protected void btnLoadWorkflow_Click(object sender, EventArgs e)
    {
        //if (txtWorkflowName.Text == hdnWfName.Value)
        //    return;
        //string fileName = "workflow";
        //string errorLog = logobj.CreateLog("LoadWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
        //Session["myWorkflow"] = null;
        //hdnWfName.Value = txtWorkflowName.Text;
        //string wfloadonchange = "<root wfname=" + '"' + txtWorkflowName.Text + '"' + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " trace=" + '"' + errorLog + '"' + "></root>";
        //string res = "";
        //try
        //{
        //    res = a.LoadWorkFlow(wfloadonchange);
        //}
        //catch (Exception exp)
        //{
        //    if (sysErrorlog)
        //    {
        //        logobj.CreateLog("Exception in LoadWorkFlow Service :--- " + exp.Message.ToString(), Session["nsessionid"].ToString(), "WorkFlow", "");
        //    }

        //    util.LogErrorToFile("Exception in LoadWorkFlow Service :--- " + exp.Message.ToString());
        //    Response.Redirect(util.errorString);
        //}
    }

    private void GetFilterColumns()
    {

        ddlFilter.Items.Clear();
        ddlFilter.Items.Add("");
        ArrayList columnName = new ArrayList();
        ArrayList columnCaption = new ArrayList();
        ArrayList columnDataType = new ArrayList();
        //get the columns and display
        string transid = "";
        transid = ddlTst.SelectedValue;
        if (transid != "")
        {
            string iXml = string.Empty;
            string fileName = "workflow";
            string errorLog = logobj.CreateLog("SaveWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
            string result = string.Empty;
            //Call service
            result = util.GetTstructDefObj(errorLog, transid).structRes;

            string errMsg = string.Empty;
            errMsg = util.ParseXmlErrorNode(result);

            if (errMsg != string.Empty)
            {
                if (errMsg == Constants.SESSIONERROR)
                {
                    Session.RemoveAll();
                    Session.Abandon();
                    SessExpires();
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
                XmlNodeList tstBaseDataNodes = default(XmlNodeList);
                tstBaseDataNodes = xmlDoc.SelectNodes("//root");
                foreach (XmlNode tstBaseDataNode in tstBaseDataNodes)
                {

                    XmlNodeList tstChildDataNodes = default(XmlNodeList);
                    tstChildDataNodes = tstBaseDataNode.ChildNodes;

                    foreach (XmlNode tstChildDataNode in tstChildDataNodes)
                    {
                        if (tstChildDataNode.Attributes["cat"].Value.ToString() == "field")
                        {
                            foreach (XmlNode fldNode in tstChildDataNode)
                            {
                                if (fldNode.Name == "a1")
                                {
                                    columnName.Add(fldNode.InnerText);
                                }
                                if (fldNode.Name == "a2")
                                {
                                    columnCaption.Add(fldNode.InnerText);
                                }
                                if (fldNode.Name == "a3")
                                {
                                    columnDataType.Add(fldNode.InnerText);
                                }
                            }
                        }
                    }
                }
                //fill the filter details 
                FillColumns(columnName, columnCaption, columnDataType);
            }
        }
        else
        {
            //clear levels
        }
    }

    private void SessExpires()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    private void FillColumns(ArrayList names, ArrayList captions, ArrayList dataTypes)
    {
        strFieldTypes.Append("<script type='text/javascript' language='javascript'>");
        ddlFldTypes.Items.Add("");
        for (int i = 0; i < names.Count; i++)
        {
            if (captions[i].ToString() != "")
            {
                ListItem lst = new ListItem();
                lst.Text = captions[i].ToString();
                lst.Value = names[i].ToString();
                ddlFilter.Items.Add(lst);
                ddlFldTypes.Items.Add(dataTypes[i].ToString());
                strFieldTypes.Append("fldTypes[" + i + "]='" + dataTypes[i].ToString() + "';");
            }
        }
        strFieldTypes.Append("</script>");
    }

    private void LoadWorkflowData()
    {
        DataSet ds = new DataSet();
        string fileName = "workflow";
        string errorLog = logobj.CreateLog("LoadWorkFlowPage.", sid, fileName, "new");
        string wfload = "<root axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + sid + '"' + " trace=" + '"' + errorLog + '"' + " appsessionkey= '" + Session["AppSessionKey"].ToString() + "' username= '" + Session["username"].ToString() + "'>";
        wfload = wfload + Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";

        string res = string.Empty;
        //Call service
        res = objWebServiceExt.CallLoadWorkFlowPageWS(fileName, wfload);

        if (res != string.Empty)
        {
            if ((res.IndexOf(Constants.ERROR) != -1))
            {

                Response.Redirect(util.errorString);
            }
        }

        XmlDocument loadxmlDoc = new XmlDocument();
        loadxmlDoc.LoadXml(res);
        string roles = "";
        XmlNodeList WrproductNodes = default(XmlNodeList);
        XmlNodeList WfchildNodes = default(XmlNodeList);
        WrproductNodes = loadxmlDoc.GetElementsByTagName("tstructs");
        if ((WrproductNodes.Count == 0))
        {

        }
        else
        {
            foreach (XmlNode WrproductNode in WrproductNodes)
            {
                WfchildNodes = WrproductNode.ChildNodes;
                foreach (XmlNode WrchildNode in WfchildNodes)
                {
                    tstructname += "~" + WrchildNode.InnerText.ToString();
                    tstructname1 += "~" + WrchildNode.Name.ToString();
                }
            }
        }

        WrproductNodes = loadxmlDoc.GetElementsByTagName("workflows");
        if ((WrproductNodes.Count == 0))
        {

        }
        else
        {
            foreach (XmlNode WrproductNode in WrproductNodes)
            {
                WfchildNodes = WrproductNode.ChildNodes;
                foreach (XmlNode WrchildNode in WfchildNodes)
                {
                    name += "~" + WrchildNode.InnerText.ToString();
                    name1 += "~" + WrchildNode.Name.ToString();
                }
            }
        }

        WrproductNodes = loadxmlDoc.GetElementsByTagName("roles");
        if ((WrproductNodes.Count == 0))
        {

        }
        else
        {
            foreach (XmlNode WrproductNode in WrproductNodes)
            {
                WfchildNodes = WrproductNode.ChildNodes;
                foreach (XmlNode WrchildNode in WfchildNodes)
                {
                    roles += "~" + WrchildNode.InnerText.ToString();
                }
            }
        }


        WrproductNodes = loadxmlDoc.GetElementsByTagName("identification");
        if ((WrproductNodes.Count == 0))
        {

        }
        else
        {
            foreach (XmlNode WrproductNode in WrproductNodes)
            {
                WfchildNodes = WrproductNode.ChildNodes;
                foreach (XmlNode WrchildNode in WfchildNodes)
                {
                    identify = WrchildNode.InnerText.ToString();
                }
            }
        }

        identification = identify.Split(',');
        wfname = name.Split('~');
        wfname1 = name1.Split('~');
        tstname = tstructname.Split('~');
        tstname1 = tstructname1.Split('~');
        rolename = roles.Split('~');
        Binddata();
    }

    private void Binddata()
    {
        strWorkflow.Append("<script language='javascript'>");
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("wfName");

        for (int m = 0; m < wfname.Length; m++)
        {
            DataRow dr = dt.NewRow();
            dr["wfName"] = wfname[m].ToString();
            dt.Rows.Add(dr);
            strWorkflow.Append("wfDbNames[" + m + "]='" + wfname[m].ToString() + "';");
            //ddlWorkFlowList.Items.Add(wfname[m].ToString());            
            ddlwfnamelist.Items.Add(wfname1[m].ToString());
        }
        ds.Tables.Add(dt);
        Session["dsWf"] = ds;

        strWorkflow.Append("var txt =document.getElementById('txtWorkflowName');");
        strWorkflow.Append("if(txt)new actb(txt, wfDbNames);");
        strWorkflow.Append("</script>");

        for (int i = 0; i < tstname.Length; i++)
        {
            ListItem item = new ListItem();
            item.Text = tstname[i].ToString();
            item.Value = tstname1[i].ToString();
            ddlTst.Items.Add(item);
            ddlTransactionId.Items.Add(item.Value);
        }

        for (int j = 0; j < rolename.Length; j++)
        {
            if (rolename[j].ToString() != "")
            {
                ddlRole.Items.Add(rolename[j].ToString());
                ddlRoles.Items.Add(rolename[j].ToString());
            }
        }
    }

    private void SetGlobalVariables()
    {
        proj = Session["project"].ToString();
        proj = CheckSpecialChars(proj);
        sid = Session["nsessionid"].ToString();
        sid = CheckSpecialChars(sid);
        user = Session["user"].ToString();
        user = CheckSpecialChars(user);
        language = Session["language"].ToString();
        if (Session["language"].ToString() == "ARABIC")
        {
            direction = "rtl";
        }

    }

    private string CheckSpecialChars(string str)
    {
        str = Regex.Replace(str, "&", "&amp;");
        str = Regex.Replace(str, "<", "&lt;");
        str = Regex.Replace(str, ">", "&gt;");
        str = Regex.Replace(str, "'", "&apos;");
        str = Regex.Replace(str, "\"", "&quot;");

        return str;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (hdnValidate.Value != "false")
        {
            hdnSubTypeCond.Value = CheckSpecialChars(hdnSubTypeCond.Value);
            if (subtypeName.Text == "Enter a sub type name")
                subtypeName.Text = "none";
            if (ddlSubType.SelectedValue != "")
                subtypeName.Text = ddlSubType.SelectedValue;
            string inputXml = "";
            string fileName = "workflow";
            string errorLog = logobj.CreateLog("SaveAttachWorkFlow.", Session["nsessionid"].ToString(), fileName, "new");
            inputXml += "<root axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace=" + '"' + errorLog + '"';
            inputXml += " transid=" + '"' + ddlTst.SelectedValue + '"' + " stype=" + '"' + subtypeName.Text + '"' + " scond=" + '"' + hdnSubTypeCond.Value + '"' + " condtxt=" + '"' + hdnCondTxt.Value + '"' + ">";

            if (hdnWrkLevel.Value != "")
            {

                string[] levels = hdnWrkLevel.Value.Split('♣');
                inputXml += "<workxml maxlevel='" + (levels.Length).ToString() + "'>";
                for (int i = 0; i < levels.Length; i++)
                {
                    string[] levelDetails = levels[i].ToString().Split('¿');
                    string reject = ""; string returnVal = "";
                    if (levelDetails[1].ToString() == "true")
                        reject = "y";
                    if (levelDetails[2].ToString() == "true")
                        returnVal = "y";
                    inputXml += "<Level" + (i + 1) + " role='" + levelDetails[0].ToString() + "'>";
                    inputXml += "<actions reject='" + reject + "' return='" + returnVal + "' approve='y' review='y'/>";
                    inputXml += "<identification>" + levelDetails[3].ToString() + "</identification>";
                    inputXml += "</Level" + (i + 1) + ">";

                }
                inputXml += "</workxml>";
            }
            inputXml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";

            string result = string.Empty;
            //Call service
            result = objWebServiceExt.CallSaveAndAttachWorkflowWS(fileName, inputXml);
            if (result == "done")
                Response.Redirect("workflow.aspx");
        }
        ddlTst.SelectedIndex = 0;
    }

    private void GetWorkflowDetails(string transid, string subtype)
    {
        string fileName = "workflow";
        string errorLog = logobj.CreateLog("LoadWorkFlowPage.", sid, fileName, "new");
        string wfload = "<root axpapp=" + '"' + Session["project"] + '"' + " transid=" + '"' + ddlTst.SelectedValue + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' stype=" + '"' + ddlSubType.SelectedValue + '"' + " sessionid=" + '"' + sid + '"' + " trace=" + '"' + errorLog + '"' + ">";
        wfload = wfload + Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";

        string res = string.Empty;
        //Call service
        res = objWebServiceExt.CallLoadWFWS(fileName, wfload);

        if (res != "")
        {
            string errMsg = string.Empty;
            errMsg = util.ParseXmlErrorNode(res);

            if (errMsg != string.Empty)
            {
                if (errMsg == Constants.SESSIONERROR)
                {
                    Session.RemoveAll();
                    Session.Abandon();
                    SessExpires();
                }
                else
                {
                    Response.Redirect(util.ERRPATH + errMsg);
                }
            }
            else
            {
                //strJs.Append("<script type='text/javascript'>");
                XmlDocument loadxmlDoc = new XmlDocument();
                loadxmlDoc.LoadXml(res);

                XmlNodeList root = loadxmlDoc.GetElementsByTagName("root");
                if (root[0].Attributes["scond"] != null)
                    lblCond.Text = root[0].Attributes["scond"].Value;

                if (root[0].Attributes["condtxt"] != null)
                    hdnCondTxt.Value = root[0].Attributes["condtxt"].Value;

                XmlNodeList levels = loadxmlDoc.GetElementsByTagName("Level");

                for (int i = 0; i < levels.Count; i++)
                {

                    XmlNodeList lvlDetails = levels[i].ChildNodes;
                    string identity = "";
                    string role = "";
                    string canReturn = "";
                    string canReject = "";

                    if (levels[i].Attributes["role"] != null)
                        role = levels[i].Attributes["role"].Value;

                    for (int j = 0; j < lvlDetails.Count; j++)
                    {
                        if (lvlDetails[j].Name == "identify")
                        {
                            identity = lvlDetails[j].InnerText;
                        }
                        else if (lvlDetails[j].Name == "actions")
                        {
                            if (lvlDetails[j].Attributes["reject"] != null)
                                canReject = lvlDetails[j].Attributes["reject"].Value;

                            if (lvlDetails[j].Attributes["return"] != null)
                                canReturn = lvlDetails[j].Attributes["return"].Value;

                        }
                        else if (lvlDetails[j].Name == "return")
                        {
                            canReturn = lvlDetails[j].InnerText;
                        }
                    }
                    //strJs.Append("wfLevels[" + i + "]='" + i.ToString() + "';");
                    // strJs.Append("LvlDetails[" + i + "]='" + identity + "¿" + role + "¿" + canReject + "¿" + canReturn + "';");
                    strJs.Append(identity + "¿" + role + "¿" + canReject + "¿" + canReturn);
                    strJs.Append("♣");

                }
                //strJs.Append("</script>");
                hdnListWf.Value = "true";
                hdnLvlValues.Value = strJs.ToString();
            }
        }
    }

    protected void ddlSubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetWorkflowDetails(ddlTst.SelectedValue, ddlSubType.SelectedValue);
    }

    private void AddNewLevel(int rCnt)
    {
        StringBuilder str = new StringBuilder();
        string dvId = "dvLevel" + rCnt;
        str.Append("<div id='dvLevel" + rCnt + "' class='wfLevel'><div class='leftPane'><table style='width:30%'><tr><td><select id='ddlUser" + rCnt + "' class='lblTxt'>");
        str.Append("<option>All users</option><option>Reporting to user</option>");
        str.Append("<option>All users in this branch</option><option>All users in this department</option>");
        str.Append("<option>All users in this region</option></select></td><td><label id='lblRole" + rCnt + "'>Role</label><select id='ddlRole" + rCnt + "' class='lblTxt'>");
        str.Append("<option></option></select></td></tr><tr><td><input type='checkbox' id='chkReturn" + rCnt + "' />");
        str.Append("<label>User can return the document</label></td></tr><tr><td><input type='checkbox' id='chkReject" + rCnt + "' />");
        str.Append("<label>User can reject the document</label></td></tr>");
        str.Append("</table></div>");
        str.Append("<div class='rightPane'><a id='lnkInsert" + rCnt + "' class='handCur lnk' href=\"javascript:InsertNewLevel('" + rCnt + "', '" + (rCnt + 1) + "');adjustwin(window);\">Insert</a>&nbsp;&nbsp;");
        str.Append("<a class='handCur lnk' href=\"javascript:RemoveLevel('" + dvId + "', '" + rCnt + "');\">Remove</a> &nbsp;");
        str.Append("<img src='../AxpImages/icons/close-button.png' alt='Close' onclick=\"javascript:HideDiv('" + dvId + "');\" /></div></div>");


        if (dvLevels.InnerHtml == "")
            dvLevels.InnerHtml = str.ToString();
        else
            dvLevels.InnerHtml += str.ToString();
    }


    private string CreateLevelHtml(int rowNo)
    {
        string dvId = "dvLevel" + rowNo;
        int newRCnt = rowNo + 1;
        StringBuilder str = new StringBuilder();
        str.Append("<div id='dvLevel" + rowNo + "' class='wfLevel'><div class='leftPane'><table style='width:30%'><tr><td><select id='ddlUser" + rowNo + "' class='lblTxt'>");
        str.Append("<option>All users</option><option>Reporting to user</option>");
        str.Append("<option>All users in this branch</option><option>All users in this department</option>");
        str.Append("<option>All users in this region</option></select></td><td><label id='lblRole" + rowNo + "'>Role</label><select id='ddlRole" + rowNo + "' class='lblTxt'>");
        str.Append("<option></option></select></td></tr><tr><td><input type='checkbox' id='chkReturn" + rowNo + "' />");
        str.Append("<label>User can return the document</label></td></tr><tr><td><input type='checkbox' id='chkReject" + rowNo + "' />");
        str.Append("<label>User can reject the document</label></td></tr>");
        str.Append("</table></div>");
        str.Append("<div class='rightPane'><a id='lnkInsert" + rowNo + "' class='handCur lnk' href=\"javascript:InsertNewLevel('" + rowNo + "', '" + newRCnt + "');adjustwin(window);\">Insert</a>&nbsp;&nbsp;");
        str.Append("<a class='handCur lnk' href=\"javascript:RemoveLevel('" + dvId + "', '" + rowNo + "');\">Remove</a> &nbsp;");
        str.Append("<img src='../AxpImages/icons/close-button.png' alt='Close' onclick=\"javascript:HideDiv('" + dvId + "');\" /></div></div>");
        return str.ToString();
    }
}
