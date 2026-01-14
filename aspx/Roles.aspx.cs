using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

public partial class aspx_Roles : System.Web.UI.Page
{
    public string proj = string.Empty;
    public string sid = string.Empty;
    public string trace = string.Empty;
    public string user = string.Empty;
    public string language = string.Empty;
    public string acScript = string.Empty;
    public string direction = "ltr";
    string errorLog = string.Empty;
    string txtFilter = string.Empty;
    ArrayList arrPages = new ArrayList();
    ArrayList arrForms = new ArrayList();
    ArrayList arrReports = new ArrayList();
    LogFile.Log logObj = new LogFile.Log();
    Util.Util util = new Util.Util();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    int totalRolerows = 0;
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
        util.IsValidSession();
        ResetSessionTime();
        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = util.toggleTheme();
        lblNoRecordMsg.Visible = false;

        Session["AllRolesForCurrentUser"] = null;
        if (Session["project"] == "" || Session["project"] == null)
        {
            SessExpires();
        }
        else
        {
            if (Request.UrlReferrer != null)
            {
                if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("mainnew.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("roles.aspx")))
                    Response.Redirect("../cusError/axcustomerror.aspx");
            }
            if (!IsPostBack)
            {
                GetGlobalVariables();
                BindRoles("", "");
                rdBtnLstActRoles.SelectedIndex = 0;
                Session["CurrentRes"] = "";
                ViewState["ResAction"] = "";
                ViewState["act"] = "";
            }
            else
            {
                SetGlobalVariables();
                if (hdnIsSearched.Value != "Go")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script language=JavaScript>HideDimmer();</script>");
                }
                dvErr1.Style.Add("display", "none");
                roleSuccess.Value = "";
                roleErr.Value = "";
            }
        }
        txtRoEditRole.Attributes.Add("onblur", "CapRoleChkBxEvent()");
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }

    private void GetGlobalVariables()
    {
        proj = Session["project"].ToString();
        ViewState["proj"] = proj;
        user = Session["user"].ToString();
        ViewState["user"] = user;
        sid = Session["nsessionid"].ToString();
        ViewState["sid"] = sid;
        language = Session["language"].ToString();
        ViewState["language"] = language;
        if (Session["language"].ToString() == "ARABIC")
        {
            direction = "rtl";
        }
    }

    private void SetGlobalVariables()
    {
        proj = ViewState["proj"].ToString();
        sid = ViewState["sid"].ToString();
        user = ViewState["user"].ToString();
        language = ViewState["language"].ToString();
    }

    #region # Roles #

    protected void imgAddRole_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = null;
        DataRowCollection drc;
        if (Session["AllResp"] != null)
        {
            ds = (DataSet)Session["AllResp"];
            dt = ds.Tables["row"];
        }
        else
        {
            dt = FillPickLists("Responsibilities");
            ds = dt.DataSet;
            Session["AllResp"] = ds;
        }

        if (dt == null)
            return;

        drc = dt.Rows;
        if (drc != null)
        {
            chkLstRoEd.Items.Clear();
            for (int i = 0; i < drc.Count; i++)
            {
                chkLstRoEd.Items.Add(drc[i]["CAPTION"].ToString());
                chkLstRoEd.Items[i].Attributes.Add("onClick", "javascript:CapRoleChkBxEvent()");
            }
            chkLstRoEd.Items.Insert(0, "Default");
        }
        //TODO: check for drc null condition
        //Added to disable all other roles if default is selected
        chkLstRoEd.Items[0].Attributes.Add("onClick", "javascript:EffectOtherCheckBoxes()");
        txtRoEditRole.Text = "";
        ViewState["RoleAction"] = "Add";
        txtRoEditRole.ReadOnly = false;
        chkRoEditActive.Checked = true;
    }

    protected void imgDelRole_Click(object sender, EventArgs e)
    {
        txtRoSeRole.Text = "";
        string SelRole = string.Empty;

        for (int i = 0; i < grdRoLstRoles.Rows.Count; i++)
        {
            LinkButton lblResp = (LinkButton)grdRoLstRoles.Rows[i].FindControl("lnkRoLstEdit");
            RadioButton Chk = (RadioButton)grdRoLstRoles.Rows[i].FindControl("chkRoLstCheck");
            if (Chk.Checked == true)
            {
                if (SelRole == string.Empty)
                    SelRole = lblResp.Text;
                else
                    SelRole += "," + lblResp.Text;
            }
        }

        errorLog = logObj.CreateLog("ProcessGroup", sid, "ProcessGroup-Delete", "new");
        string ixml = string.Empty;
        ixml = "<root axpapp='" + proj + "' action='remove' sessionid= '" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' usergroup='" + SelRole + "'>";
        ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        string res = string.Empty;
        //Call service
        res = objWebServiceExt.CallProcessGroupWS("Delete", ixml);

        if (res.Contains(Constants.ERROR) == true)
        {
            res = util.ParseXmlErrorNode(res);
            if (res == Constants.SESSIONERROR || res == Constants.SESSIONEXPMSG)
            {
                Session.RemoveAll();
                Session.Abandon();
                SessExpires();
                return;
            }
            dvErr1.Style.Add("display", "block");
            roleErr.Value = res;
        }
        else
        {
            txtRoSeRole.Text = "";
            BindRoles("", "");
            dvErr1.Style.Add("display", "block");
            roleSuccess.Value = "Role Deleted Successfully.";
        }
    }

    protected void lnkRoLstEdit_Click(object sender, EventArgs e)
    {
        ViewState["RoleAction"] = "Update";
        GridViewRow row = ((LinkButton)sender).Parent.Parent as GridViewRow;
        int rowIndx = row.RowIndex;

        LinkButton lnk = (LinkButton)grdRoLstRoles.Rows[rowIndx].FindControl("lnkRoLstEdit");
        Label lblActive = (Label)grdRoLstRoles.Rows[rowIndx].FindControl("lblRoLstActive");

        txtRoEditRole.Text = lnk.Text;
        txtRoEditRole.ReadOnly = true;
        if (lblActive.Text == "Y")
            chkRoEditActive.Checked = true;
        else
            chkRoEditActive.Checked = false;

        string result = string.Empty;
        string ixml = string.Empty;
        errorLog = logObj.CreateLog("GetUserGroupDetails", sid, "GetUserGroupDetails-RoleEdit", "new");
        ixml = "<root axpapp='" + proj + "' sessionid= '" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' usergroup='" + lnk.Text + "'>";
        ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        //Call service
        result = objWebServiceExt.CallGetUserGroupDetailsWS("RoleEdit", ixml);
        if (result.Contains(Constants.ERROR) == true)
        {
            result = util.ParseXmlErrorNode(result);
            if (result == Constants.SESSIONERROR || result == Constants.SESSIONEXPMSG)
            {
                Session.RemoveAll();
                Session.Abandon();
                SessExpires();
                return;
            }
            Response.Redirect("./err.aspx?errmsg=" + result);
        }
        else
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(result);
            int checkCount = 0;
            XmlNodeList nodes = default(XmlNodeList);
            nodes = xmldoc.GetElementsByTagName("root");
            chkLstRoEd.Items.Clear();
            for (int i = 0; i < nodes[0].ChildNodes.Count; i++)
            {
                string isChecked;
                isChecked = nodes[0].ChildNodes[i].Attributes["checked"].Value;
                chkLstRoEd.Items.Add(nodes[0].ChildNodes[i].InnerText);
                chkLstRoEd.Items[i].Attributes.Add("onClick", "javascript:CapRoleChkBxEvent()");
                if (isChecked == "true")
                {
                    chkLstRoEd.Items[i].Selected = true;
                    checkCount += 1;
                }
            }
            chkLstRoEd.Items.Insert(0, "Default");
            //Fill pick list grid grdRoEdResp chkLstRoEd
            //Refer Bug_24
            //Added to disable all Resp if default is selected
            chkLstRoEd.Items[0].Attributes.Add("onClick", "javascript:EffectOtherCheckBoxes()");
            if (checkCount == 0)
            {
                chkLstRoEd.Items[0].Selected = true;
            }
        }
    }

    protected void imgListAllRoles_Click(object sender, EventArgs e)
    {
        rdBtnLstActRoles.SelectedIndex = 0;
        txtRoSeRole.Text = "";
        BindRoles("", "");
    }

    private void BindRoles(string act, string Searchtext)
    {
        string SelRolePageNo = string.Empty;
        string Act = string.Empty;
        string ixml = string.Empty;
        if (lvPageRole.SelectedValue == "")
        {
            SelRolePageNo = "1";
        }
        else
        {
            SelRolePageNo = lvPageRole.SelectedValue;
        }
        if (act != "")
        {
            Act = act;
        }
        errorLog = logObj.CreateLog("GetRolesList", sid, "GetRolesList", "new");
        ixml = "<root axpapp='" + proj + "' sessionid= '" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' role ='" + Searchtext.Trim() + "' act='" + Act + "' res='' page='' struct='' pagesize='10' pageno='" + SelRolePageNo + "'>";
        ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        string result = string.Empty;
        //Call service
        result = objWebServiceExt.CallGetRolesListWS("Role", ixml);

        if ((result.Contains(Constants.ERROR) == true) || (result == ""))
        {
            result = util.ParseXmlErrorNode(result);
            if (result == Constants.SESSIONERROR || result == Constants.SESSIONEXPMSG)
            {
                Session.RemoveAll();
                Session.Abandon();
                SessExpires();
                return;
            }
            Response.Redirect("./err.aspx?errmsg=" + result);
        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            XmlNode rootNode = default(XmlNode);
            rootNode = xmlDoc.SelectSingleNode("root");


            XmlNodeList WrproductNodes = default(XmlNodeList);
            WrproductNodes = xmlDoc.GetElementsByTagName("row");
            if ((WrproductNodes.Count == 0))
            {
                dvErr1.Style.Add("display", "block");
                roleErr.Value = lblNodata.Text;// Constants.REC_NOT_FOUND;
                BindBlankGrid("Role", "");

            }
            else
            {
                dvErr1.Style.Add("display", "none");
                DataSet ds = new DataSet();
                System.IO.StringReader sr = new System.IO.StringReader(result);
                ds.ReadXml(sr);
                if (ds.Tables["row"].Rows.Count > 0)
                {
                    int tabIndx = 0;
                    if (ds.Tables[0].TableName == "row")
                        tabIndx = 0;
                    else
                        tabIndx = 1;

                    ds = SetAccessLngt(ds, tabIndx);
                    DataView dv2 = ds.Tables["row"].DefaultView;
                    grdRoLstRoles.DataSource = ds.Tables["row"];
                    grdRoLstRoles.DataBind();
                    AddMoreLink(ds, grdRoLstRoles, "lnkRoLstMore", tabIndx);
                    //If pgno = "1" Then
                    if (SelRolePageNo == "1")
                    {
                        if (rootNode.Attributes["totalrows"] != null)
                        {
                            totalRolerows = Convert.ToInt32(rootNode.Attributes["totalrows"].Value);
                        }
                        Session["totalRolerows"] = totalRolerows;
                    }
                    else
                    {
                        totalRolerows = Convert.ToInt32(Session["totalRolerows"]);
                    }

                    double pg = (int)totalRolerows / (int)grdRoLstRoles.PageSize;
                    int pg1 = (int)Math.Floor(pg);
                    if ((totalRolerows % grdRoLstRoles.PageSize) > 0)
                    {
                        pg1 += 1;
                    }
                    if (totalRolerows > 0)
                    {
                        recordsRole.Text = "Total no. of records : " + totalRolerows + " - " + "Pages : " + pg1;
                        lvPageRole.Visible = true;
                    }
                    else
                    {
                        lblpgCap.Visible = false;
                        lvPageRole.Visible = false;
                    }

                    int pgno = 0;
                    lvPageRole.Items.Clear();
                    for (pgno = 1; pgno <= pg1; pgno++)
                    {
                        lvPageRole.Items.Add(pgno.ToString());
                    }
                    lvPageRole.SelectedValue = SelRolePageNo;
                }
            }
        }
    }

    protected void btnRoEdSave_Click(object sender, EventArgs e)
    {
        txtRoSeRole.Text = "";
        string SelRes = string.Empty;
        for (int i = 0; i < chkLstRoEd.Items.Count; i++)
        {
            if (chkLstRoEd.Items[i].Selected == true)
            {
                if (SelRes == "")
                    SelRes = chkLstRoEd.Items[i].Text;
                else
                    SelRes += "," + chkLstRoEd.Items[i].Text;
            }
        }
        string strchkRoEditActive = string.Empty;
        if (chkRoEditActive.Checked)
        {
            strchkRoEditActive = "Y";
        }
        else
        {
            strchkRoEditActive = "N";
        }
        string action = string.Empty;

        if (ViewState["RoleAction"].ToString() == "Add")
            action = "add";
        else if (ViewState["RoleAction"].ToString() == "Copy")
            action = "add";
        else
            action = "update";

        errorLog = logObj.CreateLog("ProcessGroup", sid, "ProcessGroup-Save", "new");
        string ixml = string.Empty;
        ixml = "<root axpapp='" + proj + "' action='" + action + "' sessionid= '" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' usergroup='" + txtRoEditRole.Text.Trim() + "' blog='" + chkBlog.Checked + "' act='" + strchkRoEditActive + "' chat='" + chkChat.Checked + "'>";
        ixml += "<roles>" + SelRes + "</roles>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
        string res = string.Empty;
        //Call service
        res = objWebServiceExt.CallProcessGroupWS("Save", ixml);
        string msg = string.Empty;
        if (res == "<success>Usergroup already exists</success>")
        {
            roleErr.Value = "Role already exists. Please enter a new role.";
            chkLstRoEd.Items[0].Attributes.Add("onClick", "javascript:EffectOtherCheckBoxes()");
            return;
        }

        if (res.Contains(Constants.ERROR) == true)
        {
            res = util.ParseXmlErrorNode(res);
            if (res == Constants.SESSIONERROR || res == Constants.SESSIONEXPMSG)
            {
                Session.RemoveAll();
                Session.Abandon();
                SessExpires();
                return;
            }
            dvErr1.Style.Add("display", "none");
            chkLstRoEd.Items[0].Attributes.Add("onClick", "javascript:EffectOtherCheckBoxes()");
            roleErr.Value = res;
        }
        else
        {
            rdBtnLstActRoles.SelectedIndex = 0;
            BindRoles("", "");
            dvErr1.Style.Add("display", "none");
            if (msg != string.Empty)
                roleSuccess.Value = msg;
            else
            {
                if (ViewState["RoleAction"].ToString() == "Copy" && action == "add")
                {
                    if (res.Trim().Contains("<success>ORA-12899: value too large for column"))
                        roleErr.Value = "Role name cannot exceed 30 characters.";
                    else
                        roleSuccess.Value = "Role copied successfully.";
                }
                else if (action == "add")
                {
                    if (res.Trim().Contains("<success>ORA-12899: value too large for column"))
                        roleErr.Value = "Role name cannot exceed 30 characters.";
                    else
                        roleSuccess.Value = "Role added successfully.";
                }
                else if (action == "update")
                {
                    roleSuccess.Value = "Role updated successfully.";
                }
            }
        }
    }

    protected void rdBtnLstActRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        string act = "";
        lvPageRole.SelectedIndex = 0;
        if (rdBtnLstActRoles.SelectedIndex == 0)
            act = "";
        else if (rdBtnLstActRoles.SelectedIndex == 1)
            act = "Y";
        else
            act = "N";
        ViewState["act"] = act;
        txtRoSeRole.Text = "";
        lvPageRole.SelectedValue = "1";
        BindRoles(act, "");
    }

    protected void lvPageRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        var act = ViewState["act"].ToString();
        BindRoles(act, txtRoSeRole.Text);
    }

    protected void btnRoSeListRoles_Click(object sender, EventArgs e)
    {
        var act = ViewState["act"].ToString();
        lvPageRole.SelectedValue = "1";
        BindRoles(act, txtRoSeRole.Text);
    }

    protected void imgCopyRoles_Click(object sender, EventArgs e)
    {
        txtRoSeRole.Text = "";
        string SelRole = string.Empty;
        int checkedIndex = -1;
        for (int i = 0; i < grdRoLstRoles.Rows.Count; i++)
        {
            RadioButton Chk = (RadioButton)grdRoLstRoles.Rows[i].FindControl("chkRoLstCheck");
            if (Chk.Checked == true)
            {
                checkedIndex = i;
                break;
            }
        }
        ViewState["RoleAction"] = "Copy";
        LinkButton lnk = (LinkButton)grdRoLstRoles.Rows[checkedIndex].FindControl("lnkRoLstEdit");
        Label lblActive = (Label)grdRoLstRoles.Rows[checkedIndex].FindControl("lblRoLstActive");

        txtRoEditRole.Text = "";
        txtRoEditRole.ReadOnly = false;
        if (lblActive.Text == "Y")
            chkRoEditActive.Checked = true;
        else
            chkRoEditActive.Checked = false;

        string result = string.Empty;
        string ixml = string.Empty;
        errorLog = logObj.CreateLog("GetUserGroupDetails", sid, "GetUserGroupDetails-RoleEdit", "new");
        ixml = "<root axpapp='" + proj + "' sessionid= '" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' usergroup='" + lnk.Text + "'>";
        ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        //Call service
        result = objWebServiceExt.CallGetUserGroupDetailsWS("RoleEdit", ixml);
        if (result.Contains(Constants.ERROR) == true)
        {
            result = util.ParseXmlErrorNode(result);
            if (result == Constants.SESSIONERROR || result == Constants.SESSIONEXPMSG)
            {
                Session.RemoveAll();
                Session.Abandon();
                SessExpires();
                return;
            }
            Response.Redirect("./err.aspx?errmsg=" + result);
        }
        else
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(result);
            int checkCount = 0;
            XmlNodeList nodes = default(XmlNodeList);
            nodes = xmldoc.GetElementsByTagName("root");
            chkLstRoEd.Items.Clear();
            for (int i = 0; i < nodes[0].ChildNodes.Count; i++)
            {
                string isChecked;
                isChecked = nodes[0].ChildNodes[i].Attributes["checked"].Value;
                chkLstRoEd.Items.Add(nodes[0].ChildNodes[i].InnerText);
                chkLstRoEd.Items[i].Attributes.Add("onClick", "javascript:CapRoleChkBxEvent()");
                if (isChecked == "true")
                {
                    chkLstRoEd.Items[i].Selected = true;
                    checkCount += 1;
                }
            }
            chkLstRoEd.Items.Insert(0, "Default");
            chkLstRoEd.Items[0].Attributes.Add("onClick", "javascript:EffectOtherCheckBoxes()");
            if (checkCount == 0)
            {
                chkLstRoEd.Items[0].Selected = true;
            }
        }


    }

    #endregion

    #region # General Functions #

    /// <summary>
    /// Function to fill the picklist on page load.
    /// </summary>
    private DataTable FillPickLists(string CalledFrom)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        if (CalledFrom == "Responsibilities")
        {
            acScript += "<script language='javascript'>";
            //Fill Responsibilities
            string txt2 = "<sqlresultset axpapp='" + proj + "' sessionid='" + sid + "' trace='" + trace + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' >";
            txt2 += "<sql>select distinct au.rname as name,au.rname as caption from axuseraccess au order by au.rname</sql>";
            txt2 += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            dt = callGetChoices(txt2, "Responsibilities", ds, txtFilter);
            CreateScript(dt, "Responsibilities");
            acScript += "</script>";
        }
        return dt;
    }

    /// <summary>
    /// Function to fill the picklists based on the filter criteria.
    /// </summary>
    /// <param name="qry"></param>
    /// <param name="calledFrom"></param>
    /// <param name="ds"></param>
    /// <param name="filterTxt"></param>
    /// <returns></returns>
    private DataTable callGetChoices(string qry, string calledFrom, DataSet ds, string filterTxt)
    {

        DataSet dsCh = new DataSet();
        DataTable dt = new DataTable();

        string result = string.Empty;
        //Call service
        result = objWebServiceExt.CallGetChoiceWS("Role", qry);

        string errMsg = string.Empty;
        errMsg = util.ParseXmlErrorNode(result);

        if (errMsg != string.Empty)
        {
            if (errMsg == Constants.SESSIONERROR)
            {
                Session.RemoveAll();
                Session.Abandon();
                util.IFrameSessExpiry();
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
            XmlNodeList WrproductNodes = default(XmlNodeList);
            WrproductNodes = xmlDoc.GetElementsByTagName("row");
            if ((WrproductNodes.Count == 0))
            { }
            else
            {
                DataSet dsPages = new DataSet();
                System.IO.StringReader sr = new System.IO.StringReader(result);
                dsPages.ReadXml(sr);

                ds = dsPages;

                if (ds.Tables.Count > 1)
                {
                    dt = ds.Tables[2];
                }
            }
        }

        return dt;
    }

    /// <summary>
    /// Function to populate the javascript arrays for Auto complete.
    /// </summary>
    /// <param name="dv"></param>
    /// <param name="frm"></param>
    private void CreateScript(DataTable dt, string frm)
    {
        int i = 0;

        string acStructScript = string.Empty;
        string acRoles = string.Empty;
        string acRes = string.Empty;
        if (dt != null)
        {
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (frm == "Responsibilities")
                {
                    acScript += "arrRes[" + i + "]='" + dt.Rows[i][1].ToString() + "';";
                    acRes += "arrRes[" + i + "]='" + dt.Rows[i][1].ToString() + "';";
                }
            }
        }
    }

    private DataSet SetAccessLngt(DataSet ds, int tblIdx)
    {
        for (int i = 0; i < ds.Tables[tblIdx].Rows.Count; i++)
        {
            string strAccRgts = string.Empty;
            strAccRgts = ds.Tables[tblIdx].Rows[i][1].ToString();
            if (strAccRgts.Length > 85)
            {
                strAccRgts = strAccRgts.Substring(0, 80);
                strAccRgts = strAccRgts + "...";
                ds.Tables[tblIdx].Rows[i][1] = strAccRgts;
            }
        }
        return ds;
    }

    private void AddMoreLink(DataSet ds, GridView grd, string lnkName, int tblIdx)
    {
        for (int i = 0; i < ds.Tables[tblIdx].Rows.Count; i++)
        {
            string strAccRgts = string.Empty;
            strAccRgts = ds.Tables[tblIdx].Rows[i][1].ToString();
            LinkButton lnk = (LinkButton)grd.Rows[i].FindControl(lnkName);
            if (strAccRgts.Length > 80)
            {
                lnk.Visible = true;
            }
            else
            {
                lnk.Visible = false;
            }
        }
    }

    private void BindBlankGrid(string Name, string SourceCall)
    {
        if (Name == "Role")
        {
            grdRoLstRoles.DataSource = null;
            grdRoLstRoles.DataBind();
            recordsRole.Text = string.Empty;
            lblNoRecordMsg.Visible = true;
            lblNoRecordMsg.Text = lblNodata.Text;// Constants.REC_NOT_FOUND;
            lblpgCap.Text = string.Empty;
            lvPageRole.Visible = false;
        }
    }
    private string CheckSpecialChars(string str)
    {
        str = Regex.Replace(str, "&", "&amp;");
        str = Regex.Replace(str, "<", "&lt;");
        str = Regex.Replace(str, ">", "&gt;");
        str = Regex.Replace(str, "'", "&apos;");
        str = Regex.Replace(str, "\"", "&quot;");
        if (str == "dd/mm/yyyy")
            str = "";
        return str;
    }
    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }
    #endregion

}
