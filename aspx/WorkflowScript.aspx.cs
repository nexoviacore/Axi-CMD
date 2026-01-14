using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;


public partial class _WorkFlowScript : System.Web.UI.Page
{
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    Util.Util util = new Util.Util();
    LogFile.Log logobj = new LogFile.Log();
    Boolean sysErrorlog;
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
    string level = "";
    string lnkname = "";
    string sid = "";
    string[] identifys;
    string[] identifications;
    string[] Role;
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
    /// <summary>
    /// onload call the webservice to load the workflow related details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = util.toggleTheme();
        sysErrorlog = Convert.ToBoolean(Session["AxTrace"].ToString());
        string proj;
        try
        {
            proj = Session["project"].ToString();
        }
        catch
        { proj = ""; }

        if (proj == "")
        {
            String url = "sess.aspx";
            Response.Write("<script>");
            Response.Write("if(window.opener && !window.opener.closed){window.opener.parent.location.href='" + url + "';window.close();}else {parent.parent.location.href='" + url + "';}");
            Response.Write("</script>");

        }
        else
        {
            if (Request.UrlReferrer != null)
            {
                if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("main.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("workflowscript.aspx")))
                    Response.Redirect("../cusError/axcustomerror.aspx");
            }
            if (!IsPostBack)
            {
                ViewState["isedit"] = "false";
                sid = Session["nsessionid"].ToString();
                mvwTabs.ActiveViewIndex = 0;
                DataSet ds = new DataSet();
                string fileName = "workflow";
                string errorLog = logobj.CreateLog("LoadWorkFlowPage.", sid, fileName, "new");
                string wfload = "<root axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + sid + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace=" + '"' + errorLog + '"' + ">";
                wfload += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
                string res = "";
                try
                {
                    res = objWebServiceExt.CallLoadWorkFlowPageWS("", wfload);
                }
                catch (Exception exp)
                {
                    Response.Redirect(util.errorString);
                }

                if (res != string.Empty)
                {
                    if ((res.IndexOf(Constants.ERROR) != -1))
                    {
                        if (sysErrorlog)
                        {
                            logobj.CreateLog("Exception in LoadWorkFlow Service :--- " + res.ToString(), Session["nsessionid"].ToString(), "WorkFlow", "");
                        }
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
                GetParentandBranch();
                BindBlankGrid(grdTasks);
                identification = identify.Split(',');
                wfname = name.Split('~');
                wfname1 = name1.Split('~');
                tstname = tstructname.Split('~');
                tstname1 = tstructname1.Split('~');
                rolename = roles.Split('~');
                Binddata();
            }
            //btnAttach.Attributes.Add("OnClick", "validate(" + ddlTransaction.ClientID + "," + ddlField.ClientID + "," + ddlOperator.ClientID + "," + ddlValue1.ClientID + "," + ddlWorkFlow.ClientID + ")");
            btnDeleteWF.Attributes.Add("OnClick", "javascript:return " + "confirm('Are you sure want to delete this workflow')");
            TxtDays.Attributes.Add("onkeypress", "javascript:return CheckNumeric(event,'" + TxtDays.Text + "')");
            TxtHrs.Attributes.Add("onkeypress", "javascript:return CheckNumeric(event,'" + TxtHrs.Text + "')");
            //ddlParentGroup.Attributes.Add("onChange", "javascript:ClearGrdData()");
            //ddlSrcBranchName.Attributes.Add("onChange", "javascript:ClearGrdData()");
            //ddlDesBranchName.Attributes.Add("onChange", "javascript:ClearUserInfo()");
            Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>if (document.getElementById('SrcUser000F0'))document.getElementById('SrcUser000F0').value = document.getElementById('hdnSrcUser').value;</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "setDUser", "<script language=JavaScript>if (document.getElementById('DesUser000F0')) document.getElementById('DesUser000F0').value = document.getElementById('hdnDesUser').value;</script>");
            if (!IsPostBack)
            {
                FillModules();
            }
            DisableButtons();
        }

    }
    private void GetParentandBranch()
    {
        string iXml = string.Empty;
        iXml += "<sqlresultset axpapp='" + Session["project"] + "' sessionid=' " + sid + "' trace='" + Session["AxTrace"] + "' user='" + Session["user"] + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
        iXml += "<sql>select ParentName from vw_userbranch where username ='" + Session["user"] + "' group by ParentName</sql>";
        iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        string ires = string.Empty;
        try
        {
            ires = objWebServiceExt.CallGetChoiceWS("", iXml);
            if (ires.Contains("<error>") && ires.Contains(Constants.SESSIONEXPMSG))
            {
                Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG);
                return;
            }
        }
        catch (Exception ex)
        {
            Response.Redirect(util.errorString);
        }

        if (ires != "<error>" || ires.Contains("<error>") != true)
        {

            XmlDocument gcxmlDoc = new XmlDocument();
            XmlNodeList gcproductNodes;
            gcxmlDoc.LoadXml(ires);
            XmlNode gcproductNode;
            gcproductNodes = gcxmlDoc.SelectNodes("/sqlresultset/response/row");

            foreach (XmlNode gcproductNode_loopVariable in gcproductNodes)
            {
                gcproductNode = gcproductNode_loopVariable;
                string tmpFldName = gcproductNode.InnerText;
                if ((tmpFldName == string.Empty | tmpFldName == "*"))
                {
                    tmpFldName = "";
                }

                //ddlParentGroup.Items.Add(tmpFldName);
            }
        }
        else
        {
            Response.Redirect("./err.aspx");
        }
        FillBranchDropdown();

    }
    private void FillBranchDropdown()
    {
        // if dealer is empty do nothing. we should not be getting into this, however just a check
        //if ((ddlParentGroup.SelectedValue == string.Empty))
        //{
        //    return;
        //}

        //string sql = "";
        //sql = "select branchname from vw_userbranch where parentname ='" + CheckSpecialChars (ddlParentGroup .SelectedValue ) + "' and username ='" + Session["user"] + "'";

        //string iXml = "";
        //string sid = Session["nsessionid"].ToString();
        //string fileName = "GetChoices";
        //string errorLog = logobj.CreateLog("GetChoices.", sid, fileName, "new");

        //iXml += "<sqlresultset axpapp='" + Session["project"] + "' sessionid='" + sid + "' trace='" + errorLog + "' user='" + Session["user"] + "' transid=''>";
        //iXml += "<sql>" + sql + "</sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + "</sqlresultset>";
        //string res = string.Empty;
        //string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

        //try
        //{
        //    res = objWebServiceExt.CallGetChoiceWS("", iXml);
        //}
        //catch (Exception ex)
        //{         
        //    Response.Redirect(util.errorString);
        //}

        //XmlDocument xmlDoc = new XmlDocument();
        //XmlNodeList branchNodes;
        //XmlNode branch;

        //res = _xmlString + res;
        //xmlDoc.LoadXml(res);
        //branchNodes = xmlDoc.SelectNodes("/sqlresultset/response/row");

        //ddlSrcBranchName.Items.Clear();
        //ddlDesBranchName.Items.Clear();
        //foreach (XmlNode branchNode in branchNodes)
        //{
        //    branch = branchNode;
        //    string tmpFldName = branch.InnerText;
        //    if ((tmpFldName == string.Empty | tmpFldName == "*"))
        //    {
        //        tmpFldName = "";
        //    }

        //    ddlSrcBranchName.Items.Add(tmpFldName);
        //    ddlDesBranchName.Items.Add(tmpFldName);
        //}
    }
    private void FillModules()
    {
        string iXml = string.Empty;
        string fileName = "workflow-modules";
        string errorLog = logobj.CreateLog("fillmodules.", sid, fileName, "new");

        iXml += "<sql>select name,caption from axpages where levelno = 0 order by 2</sql>";
        string wfload = "<sqlresultset axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + sid + '"' + " appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace=" + '"' + errorLog + '"' + ">" + iXml;
        wfload += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";

        string ires_modules = string.Empty;
        try
        {
            ires_modules = objWebServiceExt.CallGetChoiceWS("", wfload);
            if (ires_modules.Contains("<error>") && ires_modules.Contains(Constants.SESSIONEXPMSG))
            {
                Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG);
                return;
            }
        }
        catch (Exception ex)
        {
            Response.Redirect(util.errorString);
        }

        if (ires_modules != "<error>" || ires_modules.Contains("<error>") != true)
        {

            XmlDocument gcxmlDoc = new XmlDocument();
            XmlNodeList gcproductNodes;
            XmlNodeList gcbaseDataNodes;
            gcxmlDoc.LoadXml(ires_modules);

            gcproductNodes = gcxmlDoc.SelectNodes("/sqlresultset/response/row");
            ddlModules.Items.Add("");
            foreach (XmlNode gcproductNode in gcproductNodes)
            {
                gcbaseDataNodes = gcproductNode.ChildNodes;
                string namVal = string.Empty;
                string capVal = string.Empty;
                foreach (XmlNode gcbaseDataNode in gcbaseDataNodes)
                {
                    string gcbaseDataname = gcbaseDataNode.Name;
                    if (gcbaseDataname.ToLower() == "caption")
                    {
                        capVal = gcbaseDataNode.InnerText;
                    }
                    else
                    {
                        namVal = gcbaseDataNode.InnerText;
                    }
                    if ((capVal != "") && (namVal != ""))
                    {
                        if (gcbaseDataNode.InnerText != "*")
                        {
                            ddlModules.Items.Add(new ListItem(capVal, namVal));
                            capVal = string.Empty;
                            namVal = string.Empty;
                        }
                    }
                }
            }
        }
        else
        {
            Response.Redirect("./err.aspx");
        }
    }


    private void DisableButtons()
    {
        if (gvWorkFlow.Rows.Count <= 0)
        {
            btnSubmit.Enabled = false;
        }
        else
        {
            btnSubmit.Enabled = true;
        }
    }

    /// <summary>
    /// function to bind the data related to workflow initially. 
    /// </summary>
    private void Binddata()
    {
        for (int m = 0; m < wfname.Length; m++)
        {
            ddlWorkFlowList.Items.Add(wfname[m].ToString());
            ddlWorkFlow.Items.Add(wfname[m].ToString());
            ddlwfnamelist.Items.Add(wfname1[m].ToString());
        }
        for (int n = 0; n < tstname.Length; n++)
        {
            ddlTransaction.Items.Add(tstname[n].ToString());
            ddlTransactionId.Items.Add(tstname1[n].ToString());
            //ddlTransactionmc.Items.Add(tstname[n].ToString());
            //ddlTransactionmcId.Items.Add(tstname1[n].ToString()); 
        }
        for (int j = 0; j < rolename.Length; j++)
        {
            if (rolename[j].ToString() != "")
            {
                ddlRole.Items.Add(rolename[j].ToString());
            }
        }
        for (int l = 0; l < identification.Length; l++)
        {
            lstidentification.Items.Add(identification[l].ToString());
        }
    }

    protected void menuTabs_MenuItemClick(object sender, MenuEventArgs e)
    {
        RefreshData();
        this.mvwTabs.ActiveViewIndex = Int32.Parse(menuTabs.SelectedValue);
    }

    /// <summary>
    /// function to reset the data.
    /// </summary>
    private void RefreshData()
    {
        TxtWorkFlow.Visible = false;
        btnAddRow.Visible = false;
        gvWorkFlow.Columns.Clear();
        gvWorkFlowAttach.Columns.Clear();
        Session["myWorkflow"] = null;
        Session["myTransaction"] = null;
        Session["myNotify"] = null;
        Session["myNotifyDetails"] = null;
        gvWorkFlow.Visible = false;
        gvWorkFlowAttach.Visible = false;
        gvUserNotify.Visible = false;
        lblWorkFlow.Enabled = true;
        ddlWorkFlowList.Visible = true;
        ddlWorkFlow.SelectedIndex = 0;
        ddlWorkFlowList.SelectedIndex = 0;
        ddlTransaction.SelectedIndex = 0;
        ddlValue1.Text = string.Empty;
        ddlValue2.Text = string.Empty;
        ddlField.SelectedIndex = 0;
        ddlOperator.SelectedIndex = 0;
        btnNotify.Visible = false;
        pnlNotify.Visible = false;
        btnDeleteWF.Enabled = false;
        btnNewWF.Enabled = true;
        pnlwfdetails.Visible = false;
        ddlActions.SelectedIndex = 0;
        DisableButtons();
        //btnSubmit.Attributes.Add("OnClick", "javascript:RefreshPage()");
    }

    /// <summary>
    /// All the events related to workflow like create new, edit , update and delete workflow.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region WorkFlow
    protected void Button1_Click(object sender, EventArgs e)
    {
        string Errmsg = verifydata();
        if (String.IsNullOrEmpty(Errmsg))
        {
            ViewState["insert"] = 0;
            AddNewData();
            BindWorkFlowGridview();
            mvwTabs.ActiveViewIndex = 0;
            clearData();
            DisableButtons();
        }
        else
        {
            Messagebox(0, Errmsg);
        }
    }



    private string verifydata()
    {
        bool Isok = false;
        string errmsg = string.Empty;

        if (ddlRole.SelectedValue == "")
            errmsg += " Select atleast one role. " + "\\n";

        for (int n = 0; n < ChkListRights.Items.Count - 1; n++)
        {
            if (ChkListRights.Items[n].Selected)
            {
                Isok = true;
                break;
            }
        }

        if (!Isok)
            errmsg += " Assign atleast one right" + "\\n";

        if (TxtHrs.Text != "")
        {
            if (Int32.Parse(TxtHrs.Text.ToString()) > 23)
                errmsg += " Hours cannot be more than 24" + "\\n";

            ddlActions.Enabled = true;
        }

        return errmsg;
    }

    private void clearData()
    {
        ddlRole.SelectedIndex = -1;
        ddlActions.SelectedIndex = -1;
        TxtDays.Text = "";
        TxtHrs.Text = "";
        ChkMandatory.Checked = false;
        lstidentification.ClearSelection();
        for (int i = 0; i < ChkListRights.Items.Count; i++)
        {
            ChkListRights.Items[i].Selected = false;
        }
        for (int j = 0; j < ddlActions.Items.Count; j++)
        {
            ddlActions.Items[j].Selected = false;
        }
    }

    protected void gvWorkFlow_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ListBox lstRole = (ListBox)e.Row.FindControl("lstRoleEdit");
            if (lstRole != null)
            {
                foreach (ListItem item in ddlRole.Items) { if (item.Text.ToString() != "") lstRole.Items.Add(item); }
                foreach (ListItem li in lstRole.Items) { li.Selected = false; }


                if ((ViewState["Role"] != "") && (ViewState["Role"] != null))
                {
                    Role = ViewState["Role"].ToString().Split(',');
                    foreach (ListItem li in lstRole.Items)
                    {
                        if (Array.IndexOf(Role, li.Value) > -1)
                            li.Selected = true;
                    }

                    ViewState["Role"] = "";
                }
            }

            DropDownList ddlActionEdit = (DropDownList)e.Row.FindControl("ddlAction");
            if (ddlActionEdit != null)
            {
                foreach (ListItem item in ddlActions.Items)
                {
                    ddlActionEdit.Items.Add(item);
                }

                if (ViewState["Action"] != "")
                {
                    string action;
                    action = ViewState["Action"].ToString();
                    ddlActionEdit.SelectedValue = action;
                }
            }

            ListBox lst1 = (ListBox)e.Row.FindControl("lstIdentify");
            if (lst1 != null)
                foreach (ListItem item in lstidentification.Items) { lst1.Items.Add(item); }

        }
    }

    protected void gvWorkFlow_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Insert")
        {
            int index = Int32.Parse(e.CommandArgument.ToString());
            ViewState["insert"] = 1;
            ViewState["insertat"] = index;
            AddNewData();
            BindWorkFlowGridview();
        }

    }
    protected void gvWorkFlow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = (DataTable)Session["myWorkflow"];
        DataRow dr = dt.Rows[e.RowIndex];
        dt.Rows.Remove(dr);
        gvWorkFlow.EditIndex = -1;
        BindWorkFlowGridview();
        DisableButtons();
    }
    protected void gvWorkFlow_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {

    }
    protected void gvWorkFlow_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string txtapprove = DataBinder.Eval(e.Row.DataItem, "WFApprove").ToString();
            string txtreject = DataBinder.Eval(e.Row.DataItem, "WFReject").ToString();
            string txtreview = DataBinder.Eval(e.Row.DataItem, "WFReview").ToString();
            string txtreturn = DataBinder.Eval(e.Row.DataItem, "WFReturn").ToString();
            string mndtry = DataBinder.Eval(e.Row.DataItem, "WFMandatory").ToString();

            CheckBox chkApprove = (CheckBox)e.Row.FindControl("chkApprove");
            CheckBox chkReject = (CheckBox)e.Row.FindControl("chkReject");
            CheckBox chkReview = (CheckBox)e.Row.FindControl("chkReview");
            CheckBox chkReturn = (CheckBox)e.Row.FindControl("chkReturn");
            CheckBox chkMandatory = (CheckBox)e.Row.FindControl("chkMandatory");

            if (e.Row.RowIndex == gvWorkFlow.EditIndex)
            {
                ListBox lstRole = (ListBox)e.Row.FindControl("lstRoleEdit");
                lstRole.Attributes.Add("onchange", "javascript:CheckRole(" + e.Row.RowIndex + ");");

                TextBox txtDays = (TextBox)e.Row.FindControl("TxtDays");
                txtDays.Attributes.Add("onblur", "javascript:CheckActions(" + e.Row.RowIndex + ");");

                TextBox txtHrs = (TextBox)e.Row.FindControl("TxtHrs");
                txtHrs.Attributes.Add("onblur", "javascript:CheckActions(" + e.Row.RowIndex + ");");
            }

            if (mndtry.ToLower() == "y")
                chkMandatory.Checked = true;
            else
                chkMandatory.Checked = false;

            if (txtapprove.ToLower() == "true")
                chkApprove.Checked = true;
            else
                chkApprove.Checked = false;

            if (txtreject.ToLower() == "true")
                chkReject.Checked = true;
            else
                chkReject.Checked = false;

            if (txtreview.ToLower() == "true")
                chkReview.Checked = true;
            else
                chkReview.Checked = false;

            if (txtreturn.ToLower() == "true")
                chkReturn.Checked = true;
            else
                chkReturn.Checked = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // reference the Delete Link Command Column.
            if (gvWorkFlow.EditIndex < 0)
            {
                LinkButton db = (LinkButton)e.Row.Cells[11].Controls[0];
                db.OnClientClick = string.Format("return confirm('Are you sure want to delete the Role?');");
            }
        }

    }

    protected void btndisplay_Click(object sender, EventArgs e)
    {

    }
    protected void gvEditWF_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            mvwTabs.ActiveViewIndex = 2;
            menuTabs.Items[2].Selected = true;
        }
    }

    protected void btnAddRow_Click(object sender, EventArgs e)
    {
        clearData();
        mvwTabs.ActiveViewIndex = 1;
    }

    public void BindWorkFlowGridview()
    {
        // BINDING THE Gridview1 FROM SESSION            
        if (Session["myWorkflow"] != null)
        {
            DataTable dt = (DataTable)Session["myWorkflow"];
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                gvWorkFlow.Visible = true;
                gvWorkFlow.DataSource = dt;
                gvWorkFlow.DataBind();
            }
            else
            {
                gvWorkFlow.Visible = false;
            }
        }
    }

    protected void gvWorkFlow_RowEditing(object sender, GridViewEditEventArgs e)
    {
        ViewState["isedit"] = "false";
        gvWorkFlow.EditIndex = e.NewEditIndex;
        string identify = string.Empty;
        string action = string.Empty;
        ViewState["Role"] = null;
        ViewState["identify"] = null;
        ViewState["Action"] = null;
        int intRow = (int)e.NewEditIndex;
        Label lblIdentify = (Label)gvWorkFlow.Rows[intRow].FindControl("lblIdentify");
        if (lblIdentify != null)
        {
            identify = lblIdentify.Text;
            if (identify != "")
            {
                ViewState["identify"] = identify;
            }
        }

        Label lblRole = (Label)gvWorkFlow.Rows[intRow].FindControl("lblRole");
        if (lblRole != null)
        {
            identify = lblRole.Text;
            if (identify != "")
            {
                ViewState["Role"] = identify;
            }
        }

        Label lblAction = (Label)gvWorkFlow.Rows[intRow].FindControl("lblAction");
        if (lblAction != null)
        {
            action = lblAction.Text;
            if (action != null)
            {
                ViewState["Action"] = action;
            }
        }

        BindWorkFlowGridview();
        ViewState["isedit"] = "edit";
    }

    protected void gvWorkFlow_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        ViewState["isedit"] = "false";

        ListBox lstRoleEdit = (ListBox)gvWorkFlow.Rows[e.RowIndex].FindControl("lstRoleEdit");
        ListBox lstidentify = (ListBox)gvWorkFlow.Rows[e.RowIndex].FindControl("lstIdentify");
        TextBox TextBoxWithDays = (TextBox)gvWorkFlow.Rows[e.RowIndex].FindControl("TxtDays");
        TextBox TextBoxWithHrs = (TextBox)gvWorkFlow.Rows[e.RowIndex].FindControl("TxtHrs");
        CheckBox chkApprove = (CheckBox)gvWorkFlow.Rows[e.RowIndex].FindControl("chkApprove");
        CheckBox chkReject = (CheckBox)gvWorkFlow.Rows[e.RowIndex].FindControl("chkReject");
        CheckBox chkReview = (CheckBox)gvWorkFlow.Rows[e.RowIndex].FindControl("chkReview");
        CheckBox chkReturn = (CheckBox)gvWorkFlow.Rows[e.RowIndex].FindControl("chkReturn");
        DropDownList ddlAction = (DropDownList)gvWorkFlow.Rows[e.RowIndex].FindControl("ddlAction");
        CheckBox chkMandatory = (CheckBox)gvWorkFlow.Rows[e.RowIndex].FindControl("chkMandatory");
        ///string Newname = ddlname.SelectedItem.Text;
        string NewDays = TextBoxWithDays.Text.ToString();

        string NewHrs = TextBoxWithHrs.Text.ToString();
        string actions = ddlAction.SelectedItem.Text.ToString();
        string Approve = "";
        string Reject = "";
        string Review = "";
        string Return = "";
        string mndtry = "";
        if (chkApprove.Checked)
            Approve = "true";
        if (chkReject.Checked)
            Reject = "true";
        if (chkReview.Checked)
            Review = "true";
        if (chkReturn.Checked)
            Return = "true";
        if (chkMandatory.Checked)
            mndtry = "Y";
        else
            mndtry = "N";

        string identify = "";

        int counts = 0;
        for (int g = 0; g < lstidentify.Items.Count; g++)
        {
            if (lstidentify.Items[g].Selected)
            {
                if (counts == 0)
                {
                    identify = lstidentify.Items[g].Text;
                    counts++;
                }
                else
                {
                    identify += "," + lstidentify.Items[g].Text;
                }
            }
        }

        string Role = "";
        int countrole = 0;
        for (int g = 0; g < lstRoleEdit.Items.Count; g++)
        {
            if (lstRoleEdit.Items[g].Selected)
            {
                if (countrole == 0)
                {
                    Role = lstRoleEdit.Items[g].Text;
                    countrole++;
                }
                else
                {
                    Role += "," + lstRoleEdit.Items[g].Text;
                }
            }
        }


        DataTable dt = (DataTable)Session["myWorkflow"];
        DataRow dr = dt.Rows[e.RowIndex];

        dr["WFname"] = Role;
        dr["WFdays"] = NewDays;
        dr["WFhrs"] = NewHrs;
        dr["WFApprove"] = Approve;
        dr["WFReject"] = Reject;
        dr["WFReview"] = Review;
        dr["WFReturn"] = Return;
        dr["identify"] = identify;
        dr["WFAction"] = actions;
        dr["WFMandatory"] = mndtry;

        dr.AcceptChanges();
        Session["myWorkflow"] = dt;
        gvWorkFlow.EditIndex = -1;
        BindWorkFlowGridview();
    }

    public void AddNewData()
    {
        // ADDING DATA AND STORING THE DATATABLE INTO THE SESSION            
        DataTable dt = new DataTable();
        if (Session["myWorkflow"] != null)
        {
            dt = (DataTable)Session["myWorkflow"];
        }
        else
        {
            dt.Columns.Add("WFname");
            dt.Columns.Add("WFdays");
            dt.Columns.Add("WFhrs");
            dt.Columns.Add("WFApprove");
            dt.Columns.Add("WFReject");
            dt.Columns.Add("WFReview");
            dt.Columns.Add("WFReturn");
            dt.Columns.Add("identify");
            dt.Columns.Add("WFAction");
            dt.Columns.Add("WFMandatory");
        }

        DataRow drow = dt.NewRow();
        if (ViewState["insert"].ToString() == "1")
        {
            drow["WFname"] = "";
            drow["WFdays"] = "";
            drow["WFhrs"] = "";
            drow["WFApprove"] = false;
            drow["WFReject"] = false;
            drow["WFReview"] = false;
            drow["WFReturn"] = false;
            drow["identify"] = "";
            drow["WFAction"] = "";
            drow["WFMandatory"] = false;
        }
        else
        {
            if (ddlRole.SelectedIndex > -1)
                drow["WFname"] = ddlRole.SelectedItem.Text.ToString();

            if (ddlActions.SelectedIndex > -1)
                drow["WFAction"] = ddlActions.SelectedItem.Text.ToString();

            string rolenames = "";
            bool isSelected;
            for (int i = 0; i < ddlRole.Items.Count; i++)
            {
                isSelected = ddlRole.Items[i].Selected;
                if (isSelected)
                    rolenames += ddlRole.Items[i].Value + ",";
            }

            if (rolenames.Length - 1 == rolenames.LastIndexOf(","))
            {
                rolenames = rolenames.Substring(0, rolenames.Length - 1);
            }

            drow["WFname"] = rolenames;

            drow["WFdays"] = TxtDays.Text.ToString();
            drow["WFhrs"] = TxtHrs.Text.ToString();
            if (ChkListRights.Items[0].Selected)
                drow["WFApprove"] = true;
            if (ChkListRights.Items[1].Selected)
                drow["WFReject"] = true;
            if (ChkListRights.Items[2].Selected)
                drow["WFReview"] = true;
            if (ChkListRights.Items[3].Selected)
                drow["WFReturn"] = true;
            if (ChkMandatory.Checked)
                drow["WFMandatory"] = "Y";

            for (int k = 0; k < lstidentification.Items.Count; k++)
            {
                if (lstidentification.Items[k].Selected)
                {
                    if (drow["identify"] == null || drow["identify"] == "")
                        drow["identify"] = lstidentification.Items[k].Text;
                    else
                        drow["identify"] += "," + lstidentification.Items[k].Text;
                }
            }
        }

        if (ViewState["insert"].ToString() == "1")
        {
            int pos = Int32.Parse(ViewState["insertat"].ToString());
            int Idx = pos;
            dt.Rows.InsertAt(drow, Idx);
            dt.AcceptChanges();
            ViewState["insert"] = 0;
        }
        else
        {
            dt.Rows.Add(drow);
        }

        Session["myWorkflow"] = dt;

    }
    protected void gvWorkFlow_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvWorkFlow.EditIndex = -1;
        BindWorkFlowGridview();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        RefreshData();
        DisableButtons();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (ViewState["isedit"].ToString() == "false")
        {
            bool issubmit = false;
            if (TxtWorkFlow.Visible == false)
            {
                if (ddlWorkFlowList.SelectedIndex > 0)
                    issubmit = true;
            }
            else
            {
                if (TxtWorkFlow.Text != "")
                    issubmit = true;
            }

            if (issubmit)
            {
                //Session["myWorkflow"] = null;
                string role = "";
                string approve = "";
                string Reject = "";
                string Review = "";
                string Return = "";
                string Days = "";
                string Hrs = "";
                string action = "";
                string wfxml = "";
                int levelcount = 0;
                string level = "";
                string mndtry = "";

                foreach (GridViewRow dr in gvWorkFlow.Rows)
                {
                    CheckBox chkApprove = (CheckBox)dr.FindControl("chkApprove");
                    CheckBox chkReject = (CheckBox)dr.FindControl("chkReject");
                    CheckBox chkReview = (CheckBox)dr.FindControl("chkReview");
                    CheckBox chkReturn = (CheckBox)dr.FindControl("chkReturn");
                    Label lblRole = (Label)dr.FindControl("lblRole");
                    Label lblDays = (Label)dr.FindControl("lblMaxDays");
                    Label lblHrs = (Label)dr.FindControl("lblMaxHours");
                    Label lblIdentify = (Label)dr.FindControl("lblIdentify");
                    Label lblAction = (Label)dr.FindControl("lblAction");
                    CheckBox chkMandatory = (CheckBox)dr.FindControl("chkMandatory");

                    role = lblRole.Text;
                    Days = lblDays.Text;
                    Hrs = lblHrs.Text;
                    action = lblAction.Text;

                    if (chkMandatory.Checked)
                        mndtry = "Y";
                    else
                        mndtry = "N";

                    if (chkApprove.Checked)
                        approve = "y";
                    else
                        approve = "n";

                    if (chkReject.Checked)
                        Reject = "y";
                    else
                        Reject = "n";

                    if (chkReview.Checked)
                        Review = "y";
                    else
                        Review = "n";

                    if (chkReturn.Checked)
                        Return = "y";
                    else
                        Return = "n";


                    levelcount += 1;
                    level = "Level" + levelcount;
                    wfxml = wfxml + "<" + level + " role=" + '"' + role + '"' + ">";
                    wfxml += "<actions" + " approve=" + '"' + approve + '"' + " reject=" + '"' + Reject + '"' + " review=" + '"' + Review + '"' + " return=" + '"' + Return + '"' + "/>";
                    wfxml += "<identification>" + lblIdentify.Text + "</identification>";
                    wfxml += "<days>" + Days + "</days>";
                    wfxml += "<hrs>" + Hrs + "</hrs>";
                    wfxml += "<action>" + action + "</action>";
                    wfxml += "<mndtry>" + mndtry + "</mndtry>";
                    wfxml += "</" + level + ">";
                }

                string finalxml = "";
                string caption = "";
                string fileName = "workflow";
                string errorLog = logobj.CreateLog("SaveWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
                string checkDup = "checkduplicate='true'";
                if (ViewState["IsEditMode"] != null && ViewState["IsEditMode"].ToString() == "true")
                {
                    checkDup = "checkduplicate='false'";

                }

                if (TxtWorkFlow.Visible == false)
                {
                    caption = ddlWorkFlowList.SelectedItem.Text;
                    finalxml += "<root  wfname=" + '"' + ddlwfnamelist.Items[ddlWorkFlowList.SelectedIndex].Text + '"' + " wfcaption=" + '"' + caption + '"' + " axpapp=" + '"' + Session["project"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " sessionid=" + '"' + Session["nsessionid"] + '"' + " trace=" + '"' + errorLog + '"' + " version='v1' " + checkDup + ">";
                }
                else
                {
                    caption = TxtWorkFlow.Text;
                    finalxml += "<root  wfcaption=" + '"' + caption + '"' + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace=" + '"' + errorLog + '"' + " version='v1' " + checkDup + ">";
                }
                finalxml += "<workxml><root maxlevel=" + '"' + levelcount + '"' + ">" + wfxml + "</root></workxml>";
                finalxml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";

                string Err = Validategv();
                if (string.IsNullOrEmpty(Err))
                {
                    string submit = "";
                    try
                    {
                        submit = objWebServiceExt.CallSaveWorkflow("", finalxml);
                    }
                    catch (Exception exp)
                    {
                        Response.Redirect(util.errorString);
                    }
                    ViewState["IsEditMode"] = "false";
                    RefreshData();
                    Response.Redirect("./WorkFlowscript.aspx");
                }
                else
                {
                    Messagebox(0, Err);
                }
            }
            else
            {
                Messagebox(4027, "");
            }
        }
        else
        {
            Messagebox(4028, "");
            ViewState["isedit"] = "false";
        }
    }

    private string Validategv()
    {
        string errmsg = "";

        int indx = gvWorkFlow.Rows.Count;
        CheckBox chkApprove = (CheckBox)gvWorkFlow.Rows[indx - 1].FindControl("chkApprove");
        if (chkApprove.Checked)
        {

        }
        else
        {
            errmsg += "No approve for last level. Cannot save." + "\\n";
            return errmsg;
        }

        if (indx > 1)
        {
            foreach (GridViewRow dr in gvWorkFlow.Rows)
            {
                if (dr.RowIndex != gvWorkFlow.Rows.Count - 1)
                {
                    Label lblRole = (Label)dr.FindControl("lblRole");
                    CheckBox chkApprove1 = (CheckBox)dr.FindControl("chkApprove");
                    CheckBox chkReject = (CheckBox)dr.FindControl("chkReject");
                    CheckBox chkReview = (CheckBox)dr.FindControl("chkReview");
                    CheckBox chkReturn = (CheckBox)dr.FindControl("chkReturn");
                    if ((!chkApprove1.Checked) && (!chkReview.Checked) && (!chkReturn.Checked))
                    {
                        if (chkReject.Checked)
                        {
                            string level = "level" + dr.RowIndex + " has only reject, Do you want to continue?";
                            btnSubmit.Attributes.Add("OnClick", "javascript:return confirm('" + level + "');");
                        }
                    }

                    if ((!chkApprove1.Checked) && (!chkReview.Checked) && (!chkReturn.Checked) && (!chkReject.Checked))
                    {
                        errmsg += "No rights for level" + dr.RowIndex + ". Cannot Save." + "\\n";
                        return errmsg;
                    }

                    if (lblRole.Text == "")
                    {
                        errmsg += "No role for level" + dr.RowIndex + ". Cannot Save." + "\\n";
                        return errmsg;
                    }
                }
            }
        }

        return errmsg;
    }

    protected void btnDeleteWF_Click(object sender, EventArgs e)
    {
        DeleteWorkflow();
        DisableButtons();
    }

    protected void DeleteWorkflow()
    {

        string workid = "";
        string workcap = "";
        string removexml = "";
        string res = "";
        string fileName = "workflow";
        string errorLog = logobj.CreateLog("RemoveWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
        if (ddlWorkFlowList.SelectedIndex > 0)
        {
            workid = ddlwfnamelist.Items[ddlWorkFlowList.SelectedIndex].Text;
            workcap = ddlWorkFlow.Items[ddlWorkFlowList.SelectedIndex].Text;
            removexml += "<root workid=" + '"' + workid + '"' + " workcap=" + '"' + workcap + '"' + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace=" + '"' + errorLog + '"' + ">";
            removexml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
            try
            {
                res = objWebServiceExt.CallRemoveWorkflow(removexml);
            }
            catch (Exception exp)
            {
                Response.Redirect(util.errorString);
            }

            int index = res.IndexOf("Removed");
            if (index != -1)
            {
                RefreshData();
                Response.Redirect("./WorkFlowscript.aspx");

            }
            else
            {
                Messagebox(0, res);
            }
        }
        else
        {
            Messagebox(4029, "");
        }

    }

    protected void menuParent_MenuItemClick(object sender, MenuEventArgs e)
    {
        Session["myWorkflow"] = null;
        // this.mvParent.ActiveViewIndex = Int32.Parse(menuParent.SelectedValue);
    }
    protected void btnNewWF_Click(object sender, EventArgs e)
    {
        //lblWorkFlowName.Visible = true;
        TxtWorkFlow.Visible = true;
        TxtWorkFlow.Text = "";
        btnAddRow.Visible = true;
        Session["myWorkflow"] = null;
        Session["myTransaction"] = null;
        Session["myNotify"] = null;
        gvWorkFlow.Columns.Clear();
        gvWorkFlow.Visible = false;
        gvUserNotify.Columns.Clear();
        gvUserNotify.Visible = false;
        btnNotify.Visible = false;
        pnlNotify.Visible = false;
        ddlWorkFlowList.Visible = false;
        ddlRole.SelectedIndex = -1;
        btnDeleteWF.Enabled = false;
        btnNewWF.Enabled = false;
        DisableButtons();
    }

    protected void ddlWorkFlowList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWorkFlowList.SelectedIndex > 0)
        {
            string fileName = "workflow";
            string errorLog = logobj.CreateLog("LoadWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
            Session["myWorkflow"] = null;
            string wfloadonchange = "<root wfname=" + '"' + ddlwfnamelist.Items[ddlWorkFlowList.SelectedIndex].Text + '"' + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace =" + '"' + errorLog + '"' + ">";
            wfloadonchange += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
            string res = string.Empty;
            try
            {
                res = objWebServiceExt.CallLoadWorkflow("", wfloadonchange);
            }
            catch (Exception exp)
            {
                Response.Redirect(util.errorString);
            }
            DataSet dsStore = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(res);
            dsStore.ReadXml(sr);


            ViewState["IsEditMode"] = "true";

            foreach (DataRow docRow in dsStore.Tables[1].Rows)
            {
                DataTable dt = new DataTable();
                if (Session["myWorkflow"] != null)
                {
                    dt = (DataTable)Session["myWorkflow"];
                }
                else
                {
                    dt.Columns.Add("WFname");
                    dt.Columns.Add("WFdays");
                    dt.Columns.Add("WFhrs");
                    dt.Columns.Add("WFApprove");
                    dt.Columns.Add("WFReject");
                    dt.Columns.Add("WFReview");
                    dt.Columns.Add("WFReturn");
                    dt.Columns.Add("identify");
                    dt.Columns.Add("WFAction");
                    dt.Columns.Add("WFMandatory");
                }
                DataRow drow = dt.NewRow();
                drow["WFname"] = docRow["name"].ToString();
                if (docRow.Table.Columns.Contains("days"))
                    drow["WFdays"] = docRow["days"].ToString();
                else
                    drow["WFdays"] = "";
                if (docRow.Table.Columns.Contains("hrs"))
                    drow["WFhrs"] = docRow["hrs"].ToString();
                else
                    drow["WFhrs"] = "";
                drow["WFApprove"] = docRow["approve"].ToString();
                drow["WFReject"] = docRow["reject"].ToString();
                drow["WFReview"] = docRow["review"].ToString();
                drow["WFReturn"] = docRow["return"].ToString();
                drow["identify"] = docRow["identify"].ToString();
                if (docRow.Table.Columns.Contains("mndtry"))
                    drow["WFMandatory"] = docRow["mndtry"].ToString();
                else
                    drow["WFMandatory"] = "N";

                if (docRow.Table.Columns.Contains("action"))
                    drow["WFAction"] = docRow["action"].ToString();
                else
                    drow["WFAction"] = "";


                dt.Rows.Add(drow);
                Session["myWorkflow"] = dt;
            }

            gvWorkFlow.EditIndex = -1;
            BindWorkFlowGridview();
            btnAddRow.Visible = true;
            btnNotify.Visible = false;
            btnDeleteWF.Enabled = true;
            gvUserNotify.Columns.Clear();
            gvUserNotify.Visible = false;
            pnlNotify.Visible = false;
            btnNewWF.Enabled = false;
        }
        else
        {
            RefreshData();
        }
        DisableButtons();
    }
    #endregion


    #region Notification
    protected void btnNotify_Click(object sender, EventArgs e)
    {
        getdata();
        gvUserNotify.Visible = true;
        pnlNotify.Visible = true;
    }

    private void getdata()
    {
        Session["myNotify"] = null;
        foreach (GridViewRow dr in gvWorkFlow.Rows)
        {
            CheckBox chkApprove = (CheckBox)dr.FindControl("chkApprove");
            CheckBox chkReject = (CheckBox)dr.FindControl("chkReject");
            CheckBox chkReview = (CheckBox)dr.FindControl("chkReview");
            CheckBox chkReturn = (CheckBox)dr.FindControl("chkReturn");
            Label lblRole = (Label)dr.FindControl("lblRole");
            DataTable dt = new DataTable();
            if (Session["myNotify"] != null)
            {
                dt = (DataTable)Session["myNotify"];
            }
            else
            {
                dt.Columns.Add("name");
                dt.Columns.Add("Approve");
                dt.Columns.Add("Reject");
                dt.Columns.Add("Review");
                dt.Columns.Add("Return");
            }
            DataRow drow = dt.NewRow();

            drow["name"] = lblRole.Text;
            if (chkApprove.Checked)
                drow["Approve"] = true;
            else
                drow["Approve"] = false;

            if (chkReject.Checked)
                drow["Reject"] = true;
            else
                drow["Reject"] = false;

            if (chkReview.Checked)
                drow["Review"] = true;
            else
                drow["Review"] = false;

            if (chkReturn.Checked)
                drow["Return"] = true;
            else
                drow["Return"] = false;

            dt.Rows.Add(drow);
            Session["myNotify"] = dt;
        }

        gvUserNotify.EditIndex = -1;
        BindNotifyGridview();
    }

    public void BindNotifyGridview()
    {
        // BINDING THE Gridview1 FROM SESSION            
        if (Session["myNotify"] != null)
        {
            DataTable dt = (DataTable)Session["myNotify"];
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                gvUserNotify.Visible = true;
                gvUserNotify.DataSource = dt;
                gvUserNotify.DataBind();
            }
            else
            {
                gvUserNotify.Visible = false;
            }
        }
    }
    protected void gvUserNotify_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string txtapprove = DataBinder.Eval(e.Row.DataItem, "Approve").ToString();
            string txtreject = DataBinder.Eval(e.Row.DataItem, "Reject").ToString();
            string txtreview = DataBinder.Eval(e.Row.DataItem, "Review").ToString();
            string txtreturn = DataBinder.Eval(e.Row.DataItem, "Return").ToString();
            LinkButton lnkApprove = (LinkButton)e.Row.FindControl("lnkApprove");
            LinkButton lnkReject = (LinkButton)e.Row.FindControl("lnkReject");
            LinkButton lnkReview = (LinkButton)e.Row.FindControl("lnkReview");
            LinkButton lnkReturn = (LinkButton)e.Row.FindControl("lnkReturn");
            lnkApprove.CommandArgument = "On Approve";
            lnkReject.CommandArgument = "On Reject";
            lnkReview.CommandArgument = "On Review";
            lnkReturn.CommandArgument = "On Return";
            if (txtapprove.ToLower() == "true")
                lnkApprove.Text = "Click Here";
            else
                lnkApprove.Text = "";


            if (txtreject.ToLower() == "true")
                lnkReject.Text = "Click Here";
            else
                lnkReject.Text = "";

            if (txtreview.ToLower() == "true")
                lnkReview.Text = "Click Here";
            else
                lnkReview.Text = "";

            if (txtreturn.ToLower() == "true")
                lnkReturn.Text = "Click Here";
            else
                lnkReturn.Text = "";
        }
    }

    protected void gvUserNotify_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView gvTemp = (GridView)sender;
        string gvUniqueID = gvTemp.UniqueID;
        if (e.CommandName == "Click")
        {
            lnkname = e.CommandArgument.ToString();
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
            level = ((Label)gvTemp.Rows[row.RowIndex].FindControl("lbllevel")).Text;
            txtEvent.Text = lnkname.ToString();
            SetData();
            mvwTabs.ActiveViewIndex = 3;
        }
    }

    private void SetData()
    {
        bool isfound = false;
        foreach (GridViewRow dr in gvWorkFlow.Rows)
        {
            Label lblRole = (Label)dr.FindControl("lblRole");
            ChkListNotify.Items.Add(lblRole.Text.ToString());
        }
        DataSet dsNotifylist1 = new DataSet();
        string notifyxml = "<root level=" + '"' + level + '"' + " wfname=" + '"' + ddlwfnamelist.Items[ddlWorkFlowList.SelectedIndex].Text + '"' + " axpapp=" + '"' + Session["project"] + '"' + " appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' sessionid=" + '"' + Session["nsessionid"] + '"' + " trace=" + '"' + "false" + '"' + ">";
        notifyxml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
        string strnotify = string.Empty;
        try
        {
            strnotify = objWebServiceExt.CallViewCommentsWS("", notifyxml);
        }
        catch (Exception exp)
        {
            Response.Redirect(util.errorString);
        }

        System.IO.StringReader sr = new System.IO.StringReader(strnotify);
        dsNotifylist1.ReadXml(sr);
        foreach (DataRow docRow in dsNotifylist1.Tables[0].Rows)
        {
            if ((txtEvent.Text == docRow["event"].ToString()) && (level == docRow["level"].ToString()))
            {
                txtEmail.Text = docRow["email"].ToString();
                rbtnlistsend.SelectedIndex = rbtnlistsend.Items.IndexOf(rbtnlistsend.Items.FindByValue(docRow["type"].ToString()));
                txtSubject.Text = docRow["subject"].ToString();
                txtBody.Text = docRow["body"].ToString();
                foreach (DataColumn col in dsNotifylist1.Tables[0].Columns)
                {
                    if (col.ColumnName == "role")
                    {
                        isfound = true;
                    }
                }
                if (isfound)
                {
                    for (int i = 0; i < ChkListNotify.Items.Count; i++)
                    {
                        if (ChkListNotify.Items[i].Text == docRow["role"].ToString())
                            ChkListNotify.Items[i].Selected = true;
                    }
                }
                else
                {
                    foreach (DataRow docRow1 in dsNotifylist1.Tables[1].Rows)
                    {
                        for (int i = 0; i < ChkListNotify.Items.Count; i++)
                        {
                            if (ChkListNotify.Items[i].Text == docRow1[0].ToString())
                                ChkListNotify.Items[i].Selected = true;
                        }
                    }
                }
            }
        }

    }

    protected void btnSaveNotify_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancelNotify_Click(object sender, EventArgs e)
    {
        mvwTabs.ActiveViewIndex = 0;
        txtBody.Text = "";
        txtEmail.Text = "";
        txtEvent.Text = "";
        txtSubject.Text = "";
        ChkListNotify.Items.Clear();
        for (int n = 0; n < rbtnlistsend.Items.Count; n++)
        {
            rbtnlistsend.Items[n].Selected = false;
        }

    }
    #endregion

    /// <summary>
    /// All the events related to workflow and transaction, like attaching, detaching workflows with transaction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Transaction
    protected void ddlWorkFlow_SelectedIndexChanged(object sender, EventArgs e)
    {
        string fileName = "workflow";
        string errorLog = logobj.CreateLog("LoadWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
        Session["myTransaction"] = null;
        string wfloadonchange1 = "<root wfname=" + '"' + ddlwfnamelist.Items[ddlWorkFlow.SelectedIndex].Text + '"' + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace =" + '"' + errorLog + '"' + ">";
        wfloadonchange1 += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
        string res1 = "";
        try
        {
            res1 = objWebServiceExt.CallLoadWorkflow("", wfloadonchange1);
        }
        catch (Exception exp)
        {
            Response.Redirect(util.errorString);
        }

        DataSet dsStore1 = new DataSet();
        System.IO.StringReader sr1 = new System.IO.StringReader(res1);
        dsStore1.ReadXml(sr1);
        if (ddlWorkFlow.SelectedIndex > 0)
        {
            foreach (DataRow docRow in dsStore1.Tables[1].Rows)
            {
                DataTable dt = new DataTable();
                if (Session["myTransaction"] != null)
                {
                    dt = (DataTable)Session["myTransaction"];
                }
                else
                {
                    dt.Columns.Add("WFname");
                    dt.Columns.Add("WFdays");
                    dt.Columns.Add("WFhrs");
                    dt.Columns.Add("WFApprove");
                    dt.Columns.Add("WFReject");
                    dt.Columns.Add("WFReview");
                    dt.Columns.Add("WFReturn");
                    dt.Columns.Add("identify");
                    dt.Columns.Add("WFAction");
                    dt.Columns.Add("WFMandatory");
                }
                DataRow drow = dt.NewRow();
                drow["WFname"] = docRow["name"].ToString();
                if (docRow.Table.Columns.Contains("days"))
                    drow["WFdays"] = docRow["days"].ToString();
                else
                    drow["WFdays"] = "";
                if (docRow.Table.Columns.Contains("hrs"))
                    drow["WFhrs"] = docRow["hrs"].ToString();
                else
                    drow["WFhrs"] = "";
                drow["WFApprove"] = docRow["approve"].ToString();
                drow["WFReject"] = docRow["reject"].ToString();
                drow["WFReview"] = docRow["review"].ToString();
                drow["WFReturn"] = docRow["return"].ToString();
                drow["identify"] = docRow["identify"].ToString();
                if (docRow.Table.Columns.Contains("mndtry"))
                    drow["WFMandatory"] = docRow["mndtry"].ToString();
                else
                    drow["WFMandatory"] = "N";

                if (docRow.Table.Columns.Contains("action"))
                    drow["WFAction"] = docRow["action"].ToString();
                else
                    drow["WFAction"] = "";

                dt.Rows.Add(drow);
                Session["myTransaction"] = dt;
            }

            gvWorkFlowAttach.EditIndex = -1;
            BindTransactGridview();
            pnlwfdetails.Visible = true;
        }
        else
        {
            gvWorkFlowAttach.Visible = false;
            pnlwfdetails.Visible = false;

        }
    }


    public void BindTransactGridview()
    {
        // BINDING THE Gridview1 FROM SESSION            
        if (Session["myTransaction"] != null)
        {
            DataTable dt = (DataTable)Session["myTransaction"];
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                gvWorkFlowAttach.Visible = true;
                gvWorkFlowAttach.DataSource = dt;
                gvWorkFlowAttach.DataBind();
            }
            else
            {
                gvWorkFlowAttach.Visible = false;
            }
        }
    }

    protected void gvWorkFlowAttach_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string txtapprove = DataBinder.Eval(e.Row.DataItem, "WFApprove").ToString();
            string txtreject = DataBinder.Eval(e.Row.DataItem, "WFReject").ToString();
            string txtreview = DataBinder.Eval(e.Row.DataItem, "WFReview").ToString();
            string txtreturn = DataBinder.Eval(e.Row.DataItem, "WFReturn").ToString();
            string mndtry = DataBinder.Eval(e.Row.DataItem, "WFMandatory").ToString();

            CheckBox chkApprove = (CheckBox)e.Row.FindControl("chkApprove");
            CheckBox chkReject = (CheckBox)e.Row.FindControl("chkReject");
            CheckBox chkReview = (CheckBox)e.Row.FindControl("chkReview");
            CheckBox chkReturn = (CheckBox)e.Row.FindControl("chkReturn");
            CheckBox chkMandatory = (CheckBox)e.Row.FindControl("chkMandatory");

            if (mndtry.ToLower() == "y")
                chkMandatory.Checked = true;
            else
                chkMandatory.Checked = false;

            if (txtapprove.ToLower() == "true")
                chkApprove.Checked = true;
            else
                chkApprove.Checked = false;

            if (txtreject.ToLower() == "true")
                chkReject.Checked = true;
            else
                chkReject.Checked = false;

            if (txtreview.ToLower() == "true")
                chkReview.Checked = true;
            else
                chkReview.Checked = false;

            if (txtreturn.ToLower() == "true")
                chkReturn.Checked = true;
            else
                chkReturn.Checked = false;
        }
    }

    protected void btnAttach_Click(object sender, EventArgs e)
    {
        if (ddlWorkFlow.SelectedIndex > 0)
        {
            string wfattachxml = "";
            string fileName = "workflow";
            string errorLog = logobj.CreateLog("AttachWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
            wfattachxml += "<root axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace=" + '"' + errorLog + '"';

            // the following code is commented as now only the values are entered. 
            // axpert handles the fld name with :, where in web its plain field name. so this is a hack
            // for removing the :, the : will be added while attaching the workflow. see fn  btnAttach_Click 
            String tmpFldValue = ddlValue2.Text;
            //if (tmpFldValue.Length > 0)
            //    tmpFldValue = ':' + tmpFldValue;
            //else
            //    tmpFldValue = "";

            wfattachxml += " transid=" + '"' + ddlTransactionId.Items[ddlTransaction.SelectedIndex].Text + '"' + " workid=" + '"' + ddlwfnamelist.Items[ddlWorkFlow.SelectedIndex].Text + '"' + " fname=" + '"' + ddlField.SelectedItem.Text + '"' + " opr=" + '"' + ddlOperator.SelectedItem.Text + '"' + " fval1=" + '"' + ddlValue1.Text + '"' + " fval2=" + '"' + tmpFldValue + '"' + ">";
            wfattachxml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
            string attachresult = "";
            try
            {
                attachresult = objWebServiceExt.CallAttachWorkflow("", wfattachxml);
            }
            catch (Exception exp)
            {
                Response.Redirect(util.errorString);
            }

            if (attachresult != string.Empty)
            {
                if ((attachresult.IndexOf(Constants.ERROR) == -1))
                {
                    RefreshData();
                }
                else
                {
                    if (sysErrorlog)
                    {
                        logobj.CreateLog("Exception in AttachWorkFlow Service :--- " + attachresult.ToString(), Session["nsessionid"].ToString(), "WorkFlow", "");
                    }
                    Response.Redirect(util.errorString);
                }
            }

        }
        else
        {
            if (ddlTransaction.SelectedIndex > 0)
                Messagebox(4030, "");
            else
                Messagebox(4031, "");
        }

    }

    protected void ddlTransaction_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTransaction.SelectedIndex > 0)
        {
            string fileName = "workflow";
            string errorLog = logobj.CreateLog("GetAttachWFDetails.", Session["nsessionid"].ToString(), fileName, "");
            Session["myTransaction"] = null;
            DataSet dsStore2 = new DataSet();
            string wftransaction = "<root transid=" + '"' + ddlTransactionId.Items[ddlTransaction.SelectedIndex].Text + '"' + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " trace=" + '"' + errorLog + '"' + ">";
            wftransaction += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
            string s = string.Empty;
            try
            {
                s = objWebServiceExt.CallGetAttachWFDetails("", wftransaction);
            }
            catch (Exception exp)
            {
                Response.Redirect(util.errorString);
            }


            XmlDocument loadxmlDoc = new XmlDocument();
            loadxmlDoc.LoadXml(s);
            string field = "";
            string workid = "";
            string workcaption = "";
            string[] fields;
            string condtext = "";
            string[] condtexts;
            XmlNodeList WrproductNodes = default(XmlNodeList);
            WrproductNodes = loadxmlDoc.GetElementsByTagName("root");
            foreach (XmlNode WrproductNode in WrproductNodes)
            {
                field = WrproductNode.Attributes["fields"].Value.ToString();
            }
            fields = field.Split(',');

            ddlField.Items.Clear();
            ddlValue1.Text = string.Empty;
            ddlValue2.Text = string.Empty;

            ddlField.Items.Add("");

            for (int k = 0; k < fields.Length - 1; k++)
            {
                ddlField.Items.Add(fields[k].ToString());
            }

            foreach (XmlNode WrproductNode in WrproductNodes)
            {
                if (WrproductNode.Attributes.Count > 1)
                {
                    workid = WrproductNode.Attributes["workid"].Value.ToString();
                    workcaption = WrproductNode.Attributes["caption"].Value.ToString();
                    condtext = WrproductNode.Attributes["condtext"].Value.ToString();

                    condtexts = condtext.Split(' ');
                    if (condtexts.Length > 2)
                    {
                        ddlField.SelectedIndex = ddlField.Items.IndexOf(ddlField.Items.FindByValue(condtexts[0].ToString()));
                        ddlOperator.SelectedIndex = ddlOperator.Items.IndexOf(ddlOperator.Items.FindByValue(condtexts[1].ToString()));

                        // currently only the values are passed. so the following code is commentd. 
                        // axpert handles the fld name with :, where in web its plain field name. so this is a hack
                        // for removing the :, the : will be added while attaching the workflow. see fn  btnAttach_Click 
                        String tmpFldValue = condtexts[2].ToString();
                        if (tmpFldValue != String.Empty)
                        {
                            string[] tmpFld = tmpFldValue.Split(',');
                            if (tmpFld.Length < 2)
                            {
                                ddlValue1.Text = tmpFld[0];
                            }
                            else
                            {
                                ddlValue1.Text = tmpFld[0];
                                ddlValue2.Text = tmpFld[1];
                            }
                        }
                    }

                    ddlWorkFlow.SelectedIndex = ddlWorkFlow.Items.IndexOf(ddlWorkFlow.Items.FindByValue(workcaption));

                    if (ddlWorkFlow.SelectedIndex > 0)
                    {

                        string wfloadonchange1 = "<root wfname=" + '"' + ddlwfnamelist.Items[ddlWorkFlow.SelectedIndex].Text + '"' + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace=" + '"' + errorLog + '"' + ">";
                        wfloadonchange1 += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
                        string res1 = "";
                        try
                        {
                            res1 = objWebServiceExt.CallLoadWorkflow("", wfloadonchange1);
                        }
                        catch (Exception exp)
                        {
                            Response.Redirect(util.errorString);
                        }

                        DataSet dsStore1 = new DataSet();
                        System.IO.StringReader sr1 = new System.IO.StringReader(res1);
                        dsStore1.ReadXml(sr1);
                        foreach (DataRow docRow in dsStore1.Tables[1].Rows)
                        {
                            DataTable dt = new DataTable();
                            if (Session["myTransaction"] != null)
                            {
                                dt = (DataTable)Session["myTransaction"];
                            }
                            else
                            {
                                dt.Columns.Add("WFname");
                                dt.Columns.Add("WFdays");
                                dt.Columns.Add("WFhrs");
                                dt.Columns.Add("WFApprove");
                                dt.Columns.Add("WFReject");
                                dt.Columns.Add("WFReview");
                                dt.Columns.Add("WFReturn");
                                dt.Columns.Add("identify");
                                dt.Columns.Add("WFAction");
                                dt.Columns.Add("WFMandatory");
                            }
                            DataRow drow = dt.NewRow();
                            drow["WFname"] = docRow["name"].ToString();
                            if (docRow.Table.Columns.Contains("days"))
                                drow["WFdays"] = docRow["days"].ToString();
                            else
                                drow["WFdays"] = "";
                            if (docRow.Table.Columns.Contains("hrs"))
                                drow["WFhrs"] = docRow["hrs"].ToString();
                            else
                                drow["WFhrs"] = "";
                            drow["WFApprove"] = docRow["approve"].ToString();
                            drow["WFReject"] = docRow["reject"].ToString();
                            drow["WFReview"] = docRow["review"].ToString();
                            drow["WFReturn"] = docRow["return"].ToString();
                            drow["identify"] = docRow["identify"].ToString();
                            drow["WFAction"] = docRow["action"].ToString();
                            if (docRow.Table.Columns.Contains("mndtry"))
                                drow["WFMandatory"] = docRow["mndtry"].ToString();
                            else
                                drow["WFMandatory"] = "N";

                            dt.Rows.Add(drow);
                            Session["myTransaction"] = dt;
                        }

                        gvWorkFlowAttach.EditIndex = -1;
                        BindTransactGridview();
                        pnlwfdetails.Visible = true;
                    }

                    else
                    {
                        gvWorkFlowAttach.Columns.Clear();
                        gvWorkFlowAttach.Visible = false;
                        Session["myTransaction"] = null;
                        pnlwfdetails.Visible = false;

                    }

                }
                else
                {
                    gvWorkFlowAttach.Columns.Clear();
                    gvWorkFlowAttach.Visible = false;
                    Session["myTransaction"] = null;
                    pnlwfdetails.Visible = false;
                    ddlField.SelectedIndex = 0;
                    ddlOperator.SelectedIndex = 0;
                    ddlValue1.Text = string.Empty;
                    ddlValue2.Text = string.Empty;
                    ddlWorkFlow.SelectedIndex = 0;
                }
            }
        }
    }
    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sXml = null;

        string fileName = "wftstructs";
        sid = Session["nsessionid"].ToString();
        string errorLog = logobj.CreateLog("Fillmodules.", sid, fileName, "new");


        string iXml = "<root axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + sid + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace=" + '"' + errorLog + '"' + " head=\"" + ddlModules.SelectedValue + "\" level=\"0\"" + ">";
        iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
        string ires_menu = "";
        try
        {
            ires_menu = objWebServiceExt.CallGetModulewiseTstruct("", iXml);
        }
        catch (Exception ex)
        {
            Response.Redirect(util.errorString);
        }

        XmlDocument loadxmlDoc = new XmlDocument();
        loadxmlDoc.LoadXml(ires_menu);

        tstructname = string.Empty; tstructname1 = string.Empty;
        XmlNodeList WrproductNodes = default(XmlNodeList);
        XmlNodeList WfchildNodes = default(XmlNodeList);
        WrproductNodes = loadxmlDoc.GetElementsByTagName("root");
        if ((WrproductNodes.Count == 0))
        {

        }
        else
        {
            ddlTransactionmc.Items.Clear();
            mConditions.Value = "";
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

        tstname = tstructname.Split('~');
        tstname1 = tstructname1.Split('~');
        ddlTransactionmc.Items.Clear();
        ddlTransactionmcId.Items.Clear();
        ddlTransactionmc.Items.Add("");
        ddlTransactionmcId.Items.Add("");
        for (int n = 0; n < tstname.Length; n++)
        {
            if (tstname[n].ToString() != "")
            {
                ddlTransactionmc.Items.Add(tstname[n].ToString());
                ddlTransactionmcId.Items.Add(tstname1[n].ToString());
            }
        }

    }


    protected void btnCancelAttach_Click(object sender, EventArgs e)
    {
        RefreshData();
    }

    protected void btnDetach_Click(object sender, EventArgs e)
    {

    }

    protected void ddlTransactionmc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTransactionmc.SelectedIndex > 0)
        {
            string fileName = "workflow";
            string errorLog = logobj.CreateLog("GetAttachWFDetails.", Session["nsessionid"].ToString(), fileName, "");
            Session["myTransaction"] = null;
            string wftransaction = "<root transid=" + '"' + ddlTransactionmcId.Items[ddlTransactionmc.SelectedIndex].Text + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " trace=" + '"' + errorLog + '"' + ">";
            wftransaction += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
            string s = string.Empty;
            try
            {
                s = objWebServiceExt.CallMultiCondAttachWFDetails("", wftransaction);
            }
            catch (Exception exp)
            {
                Response.Redirect(util.errorString);
            }

            XmlDocument loadxmlDoc = new XmlDocument();
            loadxmlDoc.LoadXml(s);
            string mcond = "";
            XmlNodeList WrproductNodes = default(XmlNodeList);
            WrproductNodes = loadxmlDoc.GetElementsByTagName("conditions");
            foreach (XmlNode WrproductNode in WrproductNodes)
            {
                for (int mc = 0; mc < WrproductNode.ChildNodes.Count; mc++)
                {
                    mcond += WrproductNode.ChildNodes[mc].InnerText + "\r\n";
                }
            }
            mConditions.Value = mcond;
        }
    }
    protected void btnAttachmc_Click(object sender, EventArgs e)
    {
        wfmConderr.Text = string.Empty;
        string wfattachxml = "";
        string fileName = "workflow-multicond";
        string errorLog = logobj.CreateLog("AttachWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
        string multiConditions = mConditions.Value;
        multiConditions = CheckSpecialChars(multiConditions);
        multiConditions = " <a>" + multiConditions + "</a>";
        multiConditions = multiConditions.Replace("\r\n", "</a><a>");
        wfattachxml += "<root axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace=" + '"' + errorLog + '"';
        wfattachxml += " transid=" + '"' + ddlTransactionmcId.Items[ddlTransactionmc.SelectedIndex].Text + '"' + " >";
        wfattachxml += "<conditions>" + multiConditions + "</conditions>";
        wfattachxml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
        string attachresult = "";
        try
        {
            attachresult = objWebServiceExt.CallAttachMultiCondWF("", wfattachxml);
        }
        catch (Exception exp)
        {
            Response.Redirect(util.errorString);
        }

        if (attachresult != string.Empty)
        {
            if ((attachresult.IndexOf(Constants.ERROR) == -1))
            {
                ddlTransactionmc.SelectedIndex = 0;
                mConditions.Value = string.Empty;
                wfmConderr.Text = string.Empty;
            }
            else
            {
                if (sysErrorlog)
                {
                    logobj.CreateLog("Exception in AttachWorkFlow Service :--- " + attachresult.ToString(), Session["nsessionid"].ToString(), "WorkFlow", "");
                }
                string eMsg = attachresult.ToString().Substring(7, attachresult.Length - 9);
                wfmConderr.Text = eMsg;
            }
        }

    }

    protected void btnCancelAttachmc_Click(object sender, EventArgs e)
    {
        ddlTransactionmc.SelectedIndex = 0;
        mConditions.Value = string.Empty;
        wfmConderr.Text = string.Empty;
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

    #endregion


    protected void Messagebox(int Msg, string message)
    {
        if (message != "")
            Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script language=JavaScript>showAlertDialog('info','" + message + "');</script>");
        else
            Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script language=JavaScript>showAlertDialog('info'," + Msg + ",'client');</script>");

    }
    protected void btnCancelWF_Click(object sender, EventArgs e)
    {
        mvwTabs.ActiveViewIndex = 0;
    }

    #region Workflow Delegation
    private void BindBlankGrid(GridView grd)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fromwhom");
        dt.Columns.Add("towhom");
        dt.Columns.Add("msg");
        dt.Columns.Add("sname");
        dt.Columns.Add("rid");
        dt.Columns.Add("cap");
        dt.Columns.Add("delg");

        DataRow drow = dt.NewRow();
        drow["fromwhom"] = "";
        drow["towhom"] = "";
        drow["msg"] = "";
        drow["sname"] = "";
        drow["rid"] = "";
        drow["cap"] = "";
        drow["delg"] = "";
        dt.Rows.Add(drow);
        Session["dtTasks"] = dt;
        grdTasks.DataSource = dt;
        grdTasks.DataBind();
    }
    protected void ddlParentGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBlankGrid(grdTasks);
        FillBranchDropdown();
    }
    protected void ddlSrcBranchName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBlankGrid(grdTasks);
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (hdnSrcUser.Value != "")
        {
            lblErrMsg.Text = "";
            string iXml;
            string fileName = "GetChoices-getgridDtls";
            string errorLog = logobj.CreateLog("Call to GetChoices Web Service.", Session["nsessionid"].ToString(), fileName, "");

            bool uservalid = util.IsUserNameValid(hdnSrcUser.Value);
            if (!uservalid)
            {
                Messagebox(4020, "");

            }
            else

            {

                iXml = "<sqlresultset axpapp='" + Session["project"] + "' transid='' user='" + hdnSrcUser.Value + "' sessionid='" + Session["nsessionid"] + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace='" + errorLog + "'>";
                iXml = iXml + "<sql>select a.fromwhom,a.towhom,convert(nvarchar(70),(a.message)) msg,a.sname, a.recordid as rid,b.caption as cap,'F' as delg from axtasks a,tstructs b where  a.status = 1  and a.towhom = '" + hdnSrcUser.Value + "' and a.sname = b.name and b.blobno = 1</sql>";
                iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                string ires = string.Empty;
                try
                {
                    ires = objWebServiceExt.CallGetChoiceWS("", iXml);
                    if (ires.Contains("<error>") && ires.Contains(Constants.SESSIONEXPMSG))
                    {
                        Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect(util.errorString);
                }

                XmlDocument loadxmlDoc = new XmlDocument();
                XmlNodeList rootNode;
                XmlNodeList chldNodes;
                loadxmlDoc.LoadXml(ires);

                rootNode = loadxmlDoc.GetElementsByTagName("response");
                chldNodes = rootNode[0].ChildNodes;

                if (ires != "<error>" && chldNodes.Count > 0)
                {
                    DataSet dsStore1 = new DataSet();
                    System.IO.StringReader sr1 = new System.IO.StringReader(ires);
                    dsStore1.ReadXml(sr1);
                    DataTable dt = new DataTable();

                    dt.Columns.Add("fromwhom");
                    dt.Columns.Add("towhom");
                    dt.Columns.Add("msg");
                    dt.Columns.Add("sname");
                    dt.Columns.Add("rid");
                    dt.Columns.Add("cap");
                    dt.Columns.Add("delg");

                    foreach (DataRow docRow in dsStore1.Tables[2].Rows)
                    {
                        DataRow drow = dt.NewRow();
                        drow["fromwhom"] = "" + docRow["fromwhom"].ToString();
                        drow["towhom"] = "" + docRow["towhom"].ToString();
                        drow["msg"] = "" + docRow["msg"].ToString();
                        drow["sname"] = "" + docRow["sname"].ToString();
                        drow["rid"] = "" + docRow["rid"].ToString();
                        drow["cap"] = "" + docRow["cap"].ToString();
                        drow["delg"] = "" + docRow["delg"].ToString();
                        dt.Rows.Add(drow);
                    }
                    Session["dtTasks"] = dt;
                    grdTasks.DataSource = dt;
                    grdTasks.DataBind();
                    Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>document.getElementById('SrcUser000F0').value = document.getElementById('hdnSrcUser').value;</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "setDUser", "<script language=JavaScript> document.getElementById('hdnDesUser').value = document.getElementById('DesUser000F0').value;</script>");
                }
                else
                {
                    hdnSrcUser.Value = "";
                    hdnDesUser.Value = "";

                    BindBlankGrid(grdTasks);
                    Messagebox(4016, "");
                }
            }


        }
        else
        {
            BindBlankGrid(grdTasks);
            Messagebox(4021, "");
        }

    }
    protected void grdTasks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFrom = (Label)e.Row.Cells[1].FindControl("lblfrom");
            Label lblto = (Label)e.Row.Cells[2].FindControl("lblto");
            Label lblDelg = (Label)e.Row.Cells[7].FindControl("lbldelg");
            LinkButton lblMsg = (LinkButton)e.Row.Cells[3].FindControl("lblmsg");
            Label lblRecid = (Label)e.Row.Cells[5].FindControl("lblrid");
            Label lblSname = (Label)e.Row.Cells[4].FindControl("lblsname");
            if (lblDelg.Text == "F")
            {
                lblFrom.Style.Add("font-weight", "bold");
                lblto.Style.Add("font-weight", "bold");
                lblMsg.Style.Add("font-weight", "bold");
            }
            else
            {
                lblFrom.Style.Add("font-weight", "normal");
                lblto.Style.Add("font-weight", "normal");
                lblMsg.Style.Add("font-weight", "normal");
            }
            string URL = "./tstruct.aspx?transid=" + lblSname.Text + "&recordid=" + lblRecid.Text + "";
            lblMsg.Attributes.Add("OnClick", "javascript:return LoadPopPage('" + URL + "');");
        }
    }
    protected void btnSubmitTask_Click(object sender, EventArgs e)
    {
        bool isAssigned = false;

        if (hdnDesUser.Value != "" && hdnSrcUser.Value != "")
        {
            if (hdnDesUser.Value.ToString().ToLower() == hdnSrcUser.Value.ToString().ToLower())
            {
                hdnDesUser.Value = "";
                Messagebox(4017, "");
            }
            else
            {
                bool uservalid = util.IsUserNameValid(hdnDesUser.Value);
                if (!uservalid)
                {
                    Messagebox(4020, "");
                }
                else
                {

                    string iXml = string.Empty;
                    string fileName1 = "GetChoices-CheckUser";
                    string errorLog1 = logobj.CreateLog("Call to GetChoices Web Service.", Session["nsessionid"].ToString(), fileName1, "");
                    iXml = "<sqlresultset axpapp='" + Session["project"] + "' transid='' user='" + hdnSrcUser.Value + "' sessionid='" + Session["nsessionid"] + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace='" + errorLog1 + "'>";
                    iXml += "<sql>select count(distinct(a.username)) from axusers a  inner join vw_userbranch b on a.axusersid =b.axusersid   where lower(a.username) =lower('" + hdnDesUser.Value + "') and a.active ='T' </sql>";
                    iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    string ires1 = string.Empty;
                    try
                    {
                        ires1 = objWebServiceExt.CallGetChoiceWS("", iXml);
                        if (ires1.Contains("<error>") && ires1.Contains(Constants.SESSIONEXPMSG))
                        {
                            Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect(util.errorString);
                    }

                    XmlDocument loadxmlDoc = new XmlDocument();
                    XmlNodeList rootNode;
                    XmlNodeList chldNodes;
                    loadxmlDoc.LoadXml(ires1);
                    string count = string.Empty;
                    rootNode = loadxmlDoc.GetElementsByTagName("response");
                    chldNodes = rootNode[0].ChildNodes;
                    foreach (XmlNode WrproductNode in chldNodes)
                    {
                        count = WrproductNode.InnerText;
                    }
                    if (count != "0")
                    {
                        string ixml;
                        string fileName = "SaveDelegatedTasks";
                        string errorLog = logobj.CreateLog("Call to SaveDelegatedTasks Web Service.", Session["nsessionid"].ToString(), fileName, "");
                        string chkFinal = "f";
                        if (chkFinalApp.Checked)
                            chkFinal = "t";
                        ixml = "<root axpapp='" + Session["project"] + "' sessionid='" + Session["nsessionid"] + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' uname='" + hdnSrcUser.Value + "' duname='" + hdnDesUser.Value + "' finalapp='" + chkFinal + "'><tasks>";

                        char[] Delimeter = { ',' };
                        string[] SelectedValues = hdnSelectedValues.Value.Split(Delimeter);

                        if (hdnSelectedValues.Value != "" && grdTasks.Rows.Count > 0)
                        {
                            for (int i = 0; i < SelectedValues.Length; i++)
                            {
                                Label lblsname = (Label)grdTasks.Rows[Convert.ToInt32(SelectedValues[i]) - 1].FindControl("lblsname");
                                Label lblcap = (Label)grdTasks.Rows[Convert.ToInt32(SelectedValues[i]) - 1].FindControl("lblcap");
                                Label lblrid = (Label)grdTasks.Rows[Convert.ToInt32(SelectedValues[i]) - 1].FindControl("lblrid");

                                ixml += "<row>";
                                ixml += "<sname>" + lblsname.Text + "</sname>";
                                ixml += "<rid>" + lblrid.Text + "</rid>";
                                ixml += "<cap>" + lblcap.Text + "</cap>";
                                ixml += "</row>";

                            }
                            ixml += "</tasks>";
                            ixml += "<tmpflds>";
                            //ixml += "<m_dealername>" + CheckSpecialChars(ddlParentGroup.SelectedValue) + "</m_dealername>";
                            //ixml += "<m_branchname>" + CheckSpecialChars(ddlDesBranchName.SelectedValue) + "</m_branchname>";
                            ixml += "</tmpflds>" + Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
                            string ires = string.Empty;
                            try
                            {
                                ires = objWebServiceExt.CallSaveDelegatedTasks("", ixml);
                            }
                            catch (Exception ex)
                            {
                                Response.Redirect(util.errorString);
                            }

                            if (ires == "<error>" || ires.Contains("<error>") == true)
                            {
                                Response.Redirect("./err.aspx");
                            }
                            else
                            {
                                BindBlankGrid(grdTasks);
                                Messagebox(4018, "");
                                isAssigned = true;
                                //ddlParentGroup.ClearSelection();
                                FillBranchDropdown();
                                hdnSelectedValues.Value = "";
                                hdnSrcUser.Value = "";
                                hdnDesUser.Value = "";
                            }
                        }
                        else
                        {
                            Messagebox(4022, "");
                        }
                    }
                    else
                    {
                        hdnDesUser.Value = "";
                        Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>document.getElementById('SrcUser000F0').value = document.getElementById('hdnSrcUser').value;</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>document.getElementById('DesUser000F0').value = '';</script>");
                        Messagebox(4022, "");
                    }

                }


            }
        }
        else
        {
            if (hdnSrcUser.Value == "")
            {
                BindBlankGrid(grdTasks);
            }

        }

        if (isAssigned != true)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>document.getElementById('SrcUser000F0').value = document.getElementById('hdnSrcUser').value;</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "setDUser", "<script language=JavaScript> document.getElementById('DesUser000F0').value = document.getElementById('hdnDesUser').value;</script>");
        }
    }
    protected void grdTasks_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtTasks = new DataTable();
        dtTasks = (DataTable)Session["dtTasks"];

        if (Convert.ToString(ViewState["SortDir"]) == "ASC")
        {
            dtTasks.DefaultView.Sort = e.SortExpression + " " + "DESC";
            ViewState["SortDir"] = "DESC";
        }
        else
        {
            dtTasks.DefaultView.Sort = e.SortExpression + " " + "ASC";
            ViewState["SortDir"] = "ASC";
        }

        Session["dtTasks"] = dtTasks;
        grdTasks.DataSource = dtTasks.DefaultView;
        grdTasks.DataBind();
        Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>document.getElementById('SrcUser000F0').value = document.getElementById('hdnSrcUser').value;</script>");
        Page.ClientScript.RegisterStartupScript(GetType(), "setDUser", "<script language=JavaScript> document.getElementById('DesUser000F0').value = document.getElementById('hdnDesUser').value;</script>");
    }
    #endregion
}
