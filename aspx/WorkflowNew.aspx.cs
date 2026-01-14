using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.IO;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;


public partial class _WorkFlowNew : System.Web.UI.Page
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
    ArrayList tstNames = new ArrayList();
    ArrayList tstCaptions = new ArrayList();
    ArrayList tstWorkflows = new ArrayList();
    public StringBuilder strFieldTypes = new StringBuilder();
    string[] tstname;
    string tstructname1 = "";
    string[] tstname1;
    string[] rolename;
    string level = "";
    string lnkname = "";
    string sid = "";
    //string[] identifys;
    //string[] identifications;
    string[] Role;
    public string transid = string.Empty;
    public string direction = "ltr";

    /// <summary>
    /// onload call the webservice to load the workflow related details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    public string prevTransId = string.Empty;
    public string prevWfName = string.Empty;
    public string prevWfId = string.Empty;
    public string workflowId = string.Empty;
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
        divpdcomment.Style["display"] = "none";
        chkpredefinedcomments.Text = "Enable predefined comments for workflow ";
        if (!IsPostBack && Request.QueryString["prevTransId"] != null)
        {
            prevTransId = Request.QueryString["prevTransId"].ToString();
            if (Request.QueryString["prevWfName"] != null)
                prevWfName = Request.QueryString["prevWfName"].ToString();
        }

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

        if (proj == "" || Session["nsessionid"] == null)
        {
            String url = "sess.aspx";
            Response.Write("<script>");
            Response.Write("if(window.opener && !window.opener.closed){window.opener.parent.location.href='" + url + "';window.close();}else {parent.parent.location.href='" + url + "';}");
            Response.Write("</script>");

        }
        else
        {
            //if (Request.UrlReferrer != null)
            //{
            //    if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("main.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("workflownew.aspx")))
            //        Response.Redirect("../cusError/axcustomerror.aspx");
            //}
            if (!IsPostBack)
            {
                ViewState["isedit"] = "false";
                sid = Session["nsessionid"].ToString();
                mvwTabs.ActiveViewIndex = 0;
                //mvwTabs.ActiveViewIndex = 1;
                OnLoad();
                if (Session["wftstruct"] != null)
                {
                    if (Session["wftstruct"].ToString() != string.Empty)
                    {
                        TreeNode node = tvWorkflow.FindNode(Session["wftstruct"].ToString());
                        if (node != null)
                        {
                            tvWorkflow.CollapseAll();
                            node.Select();
                            node.Expand();
                            if (node.Depth == 0)
                                GetWorkflowsForTst(node.Value);
                            else
                                GetWorkflowsForTst(node.Parent.Value);
                        }
                        Session["wftstruct"] = "";
                    }
                }
            }
            else
            {
                if (hdnDisplayCond.Value != string.Empty)
                {
                    lblCond.Text = hdnDisplayCond.Value;
                    dvDispCond.Style.Add("display", "block");
                    dvAddCond.Style.Add("display", "none");
                }
                if (tvWorkflow.SelectedNode != null)
                {
                    if (tvWorkflow.SelectedNode.Depth == 1)
                        transid = tvWorkflow.SelectedNode.Parent.Value;
                    else
                        transid = tvWorkflow.SelectedNode.Value;
                }
            }

            btnDeleteWF.Attributes.Add("OnClick", "javascript:return " + "confirm('Do you want to delete this workflow?')");
            TxtDays.Attributes.Add("onkeypress", "javascript:return CheckNumeric(event,'" + TxtDays.Text + "')");
            TxtHrs.Attributes.Add("onkeypress", "javascript:return CheckNumeric(event,'" + TxtHrs.Text + "')");
            Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>if (document.getElementById('SrcUser000F0'))document.getElementById('SrcUser000F0').value = document.getElementById('hdnSrcUser').value;</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "setDUser", "<script language=JavaScript>if (document.getElementById('DesUser000F0')) document.getElementById('DesUser000F0').value = document.getElementById('hdnDesUser').value;</script>");

            DisableButtons();
            if (prevWfId != string.Empty)
            {
                string workflowName = prevWfId;
                prevWfId = string.Empty;
                workflowId = prevWfId;
                dvWfDetails.Style.Add("display", "block");
                dvShowAll.Style.Add("display", "none");
                TxtWorkFlow.Text = workflowName;
                gvWorkFlow.Visible = true;
                transid = prevTransId;
                GetWorkflowDetails(workflowName);
            }
        }
    }


    private void OnLoad()
    {
        DataSet ds = new DataSet();
        string fileName = "workflow-Load";
        string errorLog = logobj.CreateLog("LoadWorkFlowPage.", sid, fileName, "new");
        string wfload = "<root axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + sid + '"' + " trace=" + '"' + errorLog + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' version='v2'>";
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
                    tstCaptions.Add(WrchildNode.InnerText);
                    tstructname1 += "~" + WrchildNode.Name.ToString();
                    tstNames.Add(WrchildNode.Name);
                    if (!string.IsNullOrEmpty(WrchildNode.Attributes["wfname"].Value))
                    {
                        tstWorkflows.Add(WrchildNode.Attributes["wfname"].Value);
                    }
                    else
                    {
                        tstWorkflows.Add("");
                    }
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
        //GetParentandBranch();
        BindBlankGrid(grdTasks);
        identification = identify.Split(',');
        wfname = name.Split('~');
        wfname1 = name1.Split('~');
        tstname = tstructname.Split('~');
        tstname1 = tstructname1.Split('~');
        rolename = roles.Split('~');
        Binddata();
        BindTree();
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
        //btnAddRow.Visible = false;
        gvWorkFlow.Columns.Clear();
        //gvWorkFlowAttach.Columns.Clear();
        Session["myWorkflow"] = null;
        Session["myTransaction"] = null;
        Session["myNotify"] = null;
        Session["myNotifyDetails"] = null;
        gvWorkFlow.Visible = false;
        //gvWorkFlowAttach.Visible = false;
        gvUserNotify.Visible = false;
        lblWorkFlow.Enabled = true;
        // ddlWorkFlowList.Visible = true;
        //ddlWorkFlow.SelectedIndex = 0;
        // ddlWorkFlowList.SelectedIndex = 0;
        //ddlTransaction.SelectedIndex = 0;
        // ddlValue1.Text = string.Empty;
        // ddlValue2.Text = string.Empty;
        // ddlField.SelectedIndex = 0;
        // ddlOperator.SelectedIndex = 0;
        btnNotify.Visible = false;
        pnlNotify.Visible = false;
        btnDeleteWF.Enabled = false;
        btnNewWF.Enabled = true;
        // pnlwfdetails.Visible = false;
        ddlActions.SelectedIndex = 0;
        DisableButtons();
        hdnSubTypeCond.Value = "";
        hdnCondTxt.Value = "";
        hdnDisplayCond.Value = "";
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

                if ((ViewState["Role"] != "") && (ViewState["Role"] != null))// if (ViewState["Action"] != "")
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
        if (dt != null && dt.Rows.Count == 1 && e.RowIndex == 0)
        {
            Messagebox(4023, "");
            return;
        }
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
                db.OnClientClick = string.Format("return confirm('Do you want to delete the Role?');");
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
        // mvwTabs.ActiveViewIndex = 1;
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
        if (intRow > gvWorkFlow.Rows.Count && gvWorkFlow.Rows.Count > 0)
        {

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
        string actions = ActionSelectedValue.Value;
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

            if (!util.IsNumber(TxtDays.Text))
                Messagebox(4024, "");
            if (TxtHrs.Text != "")
            {
                int hrs = Convert.ToInt32(TxtHrs.Text);
                if (!(util.IsNumber(TxtHrs.Text) && (hrs > 0 && hrs < 24)))
                    Messagebox(4025, "");
            }
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
            drow["identify"] = drow["identify"].ToString().TrimStart(',');
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
        BindWorkFlowGridview();
        if (ViewState["isedit"].ToString() == "false")
        {
            bool issubmit = false;

            if (TxtWorkFlow.Text != "")
                issubmit = true;

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

                string isCancel = "f";
                string isEdit = "f";
                string isPredc = "f";

                if (chkcancel.Checked)
                    isCancel = "t";

                if (chkedit.Checked)
                    isEdit = "t";

                if (chkpredefinedcomments.Checked)
                    isPredc = "t";

                string finalxml = "";
                string caption = "";
                string fileName = "workflow-save";
                string errorLog = logobj.CreateLog("SaveWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
                bool checkduplicate = false;
                caption = TxtWorkFlow.Text;
                if (tvWorkflow.SelectedNode.Text != caption)
                    checkduplicate = true;
                string wkId = string.Empty;
                if (tvWorkflow.SelectedNode.Depth == 0)
                    wkId = "";
                else
                    wkId = tvWorkflow.SelectedNode.Value;

                string cond = string.Empty;
                cond = hdnSubTypeCond.Value;
                cond = CheckSpecialChars(cond);
                finalxml += "<root transid='" + transid + "'  wfname='" + wkId + "' wfcaption=" + '"' + caption + '"' + " axpapp=" + '"' + Session["project"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " sessionid=" + '"' + Session["nsessionid"] + '"' + " trace=" + '"' + errorLog + '"' + " version='v2' cond=" + '"' + cond + '"' + " condstr=" + '"' + hdnDisplayCond.Value + '"' + " cancel='" + isCancel + "' edit='" + isEdit + "' pdcomments='" + isPredc + "' checkduplicate='" + checkduplicate + "'>";

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
                    if (submit.Contains(Constants.ERROR))
                    {
                        submit = submit.Replace(Constants.ERROR, "");
                        submit = submit.Replace(Constants.ERRORCLOSE, "");
                        Messagebox(0, submit);
                        return;
                    }
                    RefreshData();
                    if (tvWorkflow.SelectedNode.Depth == 1)
                        Session["wftstruct"] = tvWorkflow.SelectedNode.Parent.Value;
                    else
                        Session["wftstruct"] = tvWorkflow.SelectedNode.Value;
                    if (submit == "done")
                        Messagebox(4026, "");
                    Response.Redirect("./WorkFlowNew.aspx");
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

        hdnSubTypeCond.Value = string.Empty;
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


    protected void DeleteWorkflow()
    {
        string workid = "";
        string workcap = "";
        string removexml = "";
        string res = "";
        string fileName = "workflow";
        string errorLog = logobj.CreateLog("RemoveWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
        if (TxtWorkFlow.Text != string.Empty)
        {
            workid = tvWorkflow.SelectedNode.Value;
            workcap = TxtWorkFlow.Text;
            removexml += "<root workid=" + '"' + workid + '"' + " workcap=" + '"' + workcap + '"' + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace=" + '"' + errorLog + '"' + " version='v2'>";
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
                if (tvWorkflow.SelectedNode.Depth == 1)
                    Session["wftstruct"] = tvWorkflow.SelectedNode.Parent.Value;
                else
                    Session["wftstruct"] = tvWorkflow.SelectedNode.Value;
                Response.Redirect("./WorkFlownew.aspx");

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
        hdnCondTxt.Value = string.Empty;
        hdnCondDetails.Value = string.Empty;
        hdnDisplayCond.Value = string.Empty;
        hdnSubTypeCond.Value = string.Empty;
        lblCond.Text = "";
        dvAddCond.Style.Add("display", "block");
        dvDispCond.Style.Add("display", "none");
        //lblWorkFlowName.Visible = true;
        TxtWorkFlow.Visible = true;
        TxtWorkFlow.Text = "";
        //btnAddRow.Visible = true;
        Session["myWorkflow"] = null;
        Session["myTransaction"] = null;
        Session["myNotify"] = null;
        gvWorkFlow.Columns.Clear();
        gvWorkFlow.Visible = false;
        gvUserNotify.Columns.Clear();
        gvUserNotify.Visible = false;
        btnNotify.Visible = false;
        pnlNotify.Visible = false;
        //ddlWorkFlowList.Visible = false;
        ddlRole.SelectedIndex = -1;
        btnDeleteWF.Enabled = false;
        btnNewWF.Enabled = false;
        DisableButtons();
        dvWfDetails.Style.Add("display", "block");
        dvShowAll.Style.Add("display", "none");
        chkcancel.Checked = false;
        chkedit.Checked = false;
        chkpredefinedcomments.Checked = false;
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
            //txtEvent.Text = lnkname.ToString();
            //SetData();
            mvwTabs.ActiveViewIndex = 3;
        }
    }



    protected void btnSaveNotify_Click(object sender, EventArgs e)
    {

    }

    #endregion

    /// <summary>
    /// All the events related to workflow and transaction, like attaching, detaching workflows with transaction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Transaction



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




    protected void btnCancelAttach_Click(object sender, EventArgs e)
    {
        RefreshData();
    }

    protected void btnDetach_Click(object sender, EventArgs e)
    {

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

    protected void btnDeleteWF_Click(object sender, EventArgs e)
    {
        DeleteWorkflow();
        DisableButtons();
    }

    protected void Messagebox(int Msg, string message)
    {
        if (message != "")
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "myrest", "<script type='text/javascript'> showAlertDialog('info','" + message + "'); </script>", false);
        else
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "myrest", "<script type='text/javascript'> showAlertDialog('info'," + Msg + ",'client'); </script>", false);
    }
    protected void btnCancelWF_Click(object sender, EventArgs e)
    {
        mvwTabs.ActiveViewIndex = 0;
    }

    protected void btnTreeClick_Click(object sender, EventArgs e)
    {
        GetWorkflowsForTst(tvWorkflow.SelectedNode.Value);
        dvShowAll.Style.Add("display", "block");
        dvWfDetails.Style.Add("display", "none");
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
        //FillBranchDropdown();
    }
    protected void ddlSrcBranchName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBlankGrid(grdTasks);
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (hdnSrcUser.Value != "")
        {

            bool uservalid = util.IsUserNameValid(hdnSrcUser.Value);
            if (!uservalid)
            {
                Messagebox(4020, "");
            }
            else
            {


                lblErrMsg.Text = "";
                string iXml;
                string fileName = "GetChoices-getgridDtls";
                string errorLog = logobj.CreateLog("Call to GetChoices Web Service.", Session["nsessionid"].ToString(), fileName, "");



                iXml = "<sqlresultset axpapp='" + Session["project"] + "' transid='' user='" + hdnSrcUser.Value + "' sessionid='" + Session["nsessionid"] + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                iXml = iXml + "<sql>select a.fromwhom,a.towhom,a.message msg,a.sname, a.recordid as rid,b.caption as cap,'F' as delg from axtasks a,tstructs b where  a.status = 1  and a.towhom = '" + hdnSrcUser.Value + "' and a.sname = b.name and b.blobno = 1</sql>";
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
                        //Earlier the message was trimmed in the sql, which is now done in the dt.
                        //drow["msg"] = "" + docRow["msg"].ToString().Substring(0,150);
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
            string URL = "./tstruct.aspx?transid=" + lblSname.Text + "&recordid=" + lblRecid.Text + "" + "&openerIV="+lblSname.Text+"&isIV=false";
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
                    iXml = "<sqlresultset axpapp='" + Session["project"] + "' transid='' user='" + hdnSrcUser.Value + "' sessionid='" + Session["nsessionid"] + "' trace='" + errorLog1 + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
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
                        string chkFinal = "f";
                        if (chkFinalApp.Checked)
                            chkFinal = "t";
                        string ixml;
                        string fileName = "SaveDelegatedTasks";
                        string errorLog = logobj.CreateLog("Call to SaveDelegatedTasks Web Service.", Session["nsessionid"].ToString(), fileName, "new");
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
                                ires = ires.Replace("<error>", "");
                                ires = ires.Replace("</error>", "");
                                Response.Redirect("./err.aspx?errmsg=" + ires);
                            }
                            else
                            {
                                BindBlankGrid(grdTasks);
                                Messagebox(4018, "");
                                isAssigned = true;
                                //ddlParentGroup.ClearSelection();
                                //FillBranchDropdown();
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
    protected void tvWorkflow_SelectedNodeChanged(object sender, EventArgs e)
    {
        tvWorkflow.CollapseAll();
        if (tvWorkflow.SelectedNode.Depth == 1)
        {
            dvWfDetails.Style.Add("display", "block");
            dvShowAll.Style.Add("display", "none");
            TxtWorkFlow.Text = tvWorkflow.SelectedNode.Text;
            gvWorkFlow.Visible = true;
            transid = tvWorkflow.SelectedNode.Parent.Value;
            GetWorkflowDetails(tvWorkflow.SelectedNode.Value);
            tvWorkflow.SelectedNode.Parent.Expand();
        }
        else if (tvWorkflow.SelectedNode.Depth == 0)
        {
            transid = tvWorkflow.SelectedNode.Value;
            dvWfDetails.Style.Add("display", "none");
            dvShowAll.Style.Add("display", "block");
            GetWorkflowsForTst(tvWorkflow.SelectedNode.Value);
            //Display all the workflows and their conditions
            tvWorkflow.SelectedNode.Expand();
        }
        GetFilterColumns();


    }

    private void BindTree()
    {
        int wficount = 0;
        for (int i = 0; i < tstNames.Count; i++)
        {
            TreeNode node = new TreeNode(tstCaptions[i].ToString() + "(" + tstNames[i].ToString() + ")");
            node.Value = tstNames[i].ToString();
            //tstWorkflows will be in the format - wf1~caption1,wf2~caption2
            if (tstWorkflows[i].ToString() != string.Empty)
            {
                string[] workFlows = tstWorkflows[i].ToString().Split(',');

                for (int j = 0; j < workFlows.Length; j++)
                {
                    if (workFlows[j].ToString() != string.Empty)
                    {
                        string[] wfDetails = workFlows[j].ToString().Split('~');
                        if (prevWfName != string.Empty && prevWfName == wfDetails[1].ToString())
                        {
                            prevWfId = wfDetails[0].ToString();
                            wficount = i;
                        }
                        TreeNode childNode = new TreeNode(wfDetails[1].ToString());
                        childNode.Value = wfDetails[0].ToString();
                        node.ChildNodes.Add(childNode);
                    }
                }
            }
            tvWorkflow.Nodes.Add(node);
        }
        tvWorkflow.CollapseAll();
        if (tvWorkflow.Nodes.Count > 0)
        {
            tvWorkflow.Nodes[wficount].Select();
            tvWorkflow.Nodes[wficount].Expand();
            GetWorkflowsForTst(tvWorkflow.Nodes[wficount].Value);
            transid = tvWorkflow.Nodes[wficount].Value;
        }
        GetFilterColumns();
        dvShowAll.Style.Add("display", "block");
        dvWfDetails.Style.Add("display", "none");
    }

    private void GetWorkflowDetails(string workflowName)
    {
        workflowId = workflowName;
        divpdcomment.Style["display"] = "block";
        chkpredefinedcomments.Text = "";
        string fileName = "workflow-loadwf";
        string errorLog = logobj.CreateLog("LoadWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
        Session["myWorkflow"] = null;
        string wfloadonchange = "<root wfname=" + '"' + workflowName + '"' + " axpapp=" + '"' + Session["project"] + '"' + " appsessionkey = '" + Session["AppSessionKey"].ToString() + "' username = '" + Session["username"].ToString() + "'" + " sessionid = " + '"' + Session["nsessionid"] + '"' + " trace = " + '"' + errorLog + '"' + " version = 'v2' > ";
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
        XmlDocument loadxmlDoc = new XmlDocument();
        loadxmlDoc.LoadXml(res);


        XmlNode root = loadxmlDoc.GetElementsByTagName("root")[0].ChildNodes[0];

        if (root.Attributes["cancel"] != null)
        {
            if (root.Attributes["cancel"].Value.ToLower() == "t")
                chkcancel.Checked = true;
            else
                chkcancel.Checked = false;

        }

        if (root.Attributes["WEdit"] != null)
        {
            if (root.Attributes["WEdit"].Value.ToLower() == "t")
                chkedit.Checked = true;
            else
                chkedit.Checked = false;

        }

        if (loadxmlDoc.GetElementsByTagName("root")[0].Attributes["pdcomments"] != null)
        {
            if (loadxmlDoc.GetElementsByTagName("root")[0].Attributes["pdcomments"].Value.ToLower() == "t")
                chkpredefinedcomments.Checked = true;
            else
                chkpredefinedcomments.Checked = false;

        }


        XmlNodeList rootNode = loadxmlDoc.GetElementsByTagName("root")[0].ChildNodes[0].ChildNodes;
        StringBuilder strCond = new StringBuilder();
        foreach (XmlNode condNode in rootNode)
        {
            if (condNode.Name != "condtxt") continue;
            XmlNodeList listNode = condNode.ChildNodes;
            string strNodeValue = string.Empty;
            string joinStr = string.Empty;
            int cnt = 0;
            foreach (XmlNode iNode in listNode)
            {
                cnt = cnt + 1;
                //if (strCond.ToString() != string.Empty)
                //    strCond.Append("|");
                foreach (XmlNode cNode in iNode.ChildNodes)
                {
                    strNodeValue = cNode.InnerText;
                    if (cNode.Name == "j")
                    {
                        if (strNodeValue.ToLower() == "or" || strNodeValue.ToLower() == "and")
                        {
                            strCond.Append("^|" + strNodeValue + "^");
                        }

                    }
                    else
                    {
                        if (cNode.Name == "f")
                        {
                            if (strNodeValue != string.Empty)
                                strNodeValue = strNodeValue.Substring(strNodeValue.IndexOf("(")).Replace(" )", "").Replace("( ", "");
                            strCond.Append(strNodeValue);
                        }
                        else
                        {
                            //strNodeValue.Substring(16).Replace(" )","").Replace("( ","");
                            if (strCond.ToString() != string.Empty)
                                strCond.Append("^" + strNodeValue);
                            else
                                strCond.Append(strNodeValue);
                        }
                    }
                }
            }
        }

        hdnCondTxt.Value = strCond.ToString();

        DataSet dsStore = new DataSet();
        System.IO.StringReader sr = new System.IO.StringReader(res);
        dsStore.ReadXml(sr);
        string condText = string.Empty;
        string cond = string.Empty;
        foreach (DataRow docRow in dsStore.Tables["workflow"].Rows)
        {
            condText = docRow["condstr"].ToString();
            cond = docRow["cond"].ToString();
            if (condText == string.Empty)
            {
                dvAddCond.Style.Add("display", "block");
                dvDispCond.Style.Add("display", "none");
            }
            else
            {
                dvDispCond.Style.Add("display", "block");
                dvAddCond.Style.Add("display", "none");
                lblCond.Text = condText;
                hdnDisplayCond.Value = condText;
            }
        }

        if (cond != string.Empty)
            hdnSubTypeCond.Value = cond;
        if (dsStore.Tables["Level"] != null)
        {
            foreach (DataRow docRow in dsStore.Tables["Level"].Rows)
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
        }
        gvWorkFlow.EditIndex = -1;
        BindWorkFlowGridview();
        //btnAddRow.Visible = true;
        btnNotify.Visible = false;
        btnDeleteWF.Enabled = true;
        gvUserNotify.Columns.Clear();
        gvUserNotify.Visible = false;
        pnlNotify.Visible = false;
        btnNewWF.Enabled = false;
        DisableButtons();
    }

    private void GetWorkflowsForTst(string transId)
    {
        sid = Session["nsessionid"].ToString();
        string fileName = "GetWorkflowsConditions";
        string errorLog = logobj.CreateLog("GetWorkflowsForTst.", sid, fileName, "new");
        string wfload = "<root axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + sid + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace=" + '"' + errorLog + '"' + " transid=" + '"' + transId + '"' + ">";
        wfload += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
        string res = "";
        try
        {
            res = objWebServiceExt.CallGetWorkflowsForTst(transId, wfload);
        }
        catch (Exception exp)
        {
            Response.Redirect(util.errorString);
        }
        XmlDocument loadxmlDoc = new XmlDocument();
        loadxmlDoc.LoadXml(res);
        string roles = "";
        XmlNodeList WrproductNodes = default(XmlNodeList);
        XmlNodeList WfchildNodes = default(XmlNodeList);
        WrproductNodes = loadxmlDoc.GetElementsByTagName("response");

        StringBuilder strDetails = new StringBuilder();

        DataTable dt = new DataTable();
        dt.Columns.Add("Name");
        dt.Columns.Add("WorkflowID");
        dt.Columns.Add("Condition");
        foreach (XmlNode WrproductNode in WrproductNodes)
        {
            WfchildNodes = WrproductNode.ChildNodes;
            foreach (XmlNode WrchildNode in WfchildNodes)
            {
                string role = string.Empty;
                string caption = string.Empty;

                if (WrchildNode.Attributes["wfroles"] != null)
                    role = WrchildNode.Attributes["wfroles"].Value;

                if (WrchildNode.Attributes["wfcaption"] != null)
                    caption = WrchildNode.Attributes["wfcaption"].Value;

                if (WrchildNode.InnerText == string.Empty)
                    strDetails.Append("if( no condition ) then");
                else
                    strDetails.Append("if( " + WrchildNode.InnerText + " ) then");

                strDetails.Append(Environment.NewLine);
                strDetails.Append("      " + caption + "(" + role + ")");
                strDetails.Append(Environment.NewLine);
            }
        }
        txtWFDetails.Text = strDetails.ToString();

    }

    private void GetFilterColumns()
    {

        ddlFilter.Items.Clear();
        ddlFilter.Items.Add("");
        ArrayList columnName = new ArrayList();
        ArrayList columnCaption = new ArrayList();
        ArrayList columnDataType = new ArrayList();
        //get the columns and display        
        if (transid != "")
        {
            string iXml = string.Empty;
            string fileName = "workflow";
            string errorLog = logobj.CreateLog("SaveWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
            string result = string.Empty;
            //Call service
            if (util.GetTstructDefObj(errorLog, transid) != null)
                result = util.GetTstructDefObj(errorLog, transid).structRes;

            string errMsg = string.Empty;
            errMsg = util.ParseXmlErrorNode(result);

            if (errMsg != string.Empty)
            {

                Response.Redirect(util.ERRPATH + errMsg);

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

    private void FillColumns(ArrayList names, ArrayList captions, ArrayList dataTypes)
    {
        strFieldTypes.Append("<script type='text/javascript' language='javascript'>");
        ddlFldTypes.Items.Clear();
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
    protected void btnDeleteCond_Click(object sender, EventArgs e)
    {
        string fileName = "DeleteCondition-" + tvWorkflow.SelectedNode.Value;
        string errorLog = logobj.CreateLog("LoadWorkFlow.", Session["nsessionid"].ToString(), fileName, "");
        Session["myWorkflow"] = null;
        string wfloadonchange = "<root workid=" + '"' + tvWorkflow.SelectedNode.Value + '"' + " axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + Session["nsessionid"] + '"' + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'" + " trace=" + '"' + errorLog + '"' + " version='v2'>";
        wfloadonchange += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
        string res = string.Empty;
        try
        {
            res = objWebServiceExt.CallDeleteWFCondition("", wfloadonchange);
        }
        catch (Exception exp)
        {
            Response.Redirect(util.errorString);
        }
        if (res == "done")
        {
            hdnSubTypeCond.Value = "";
            hdnDisplayCond.Value = "";
            hdnCondTxt.Value = "";
            dvDispCond.Style.Add("display", "none");
            dvAddCond.Style.Add("display", "block");
        }
    }
    }

