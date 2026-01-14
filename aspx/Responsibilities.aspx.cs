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
using System.Web.Services;
using System.Linq;

public partial class aspx_Responsibilities : System.Web.UI.Page
{
    static public string proj = string.Empty;
    static public string sid = string.Empty;
    static public string trace = string.Empty;
    static public string user = string.Empty;
    public string language = string.Empty;
    public string acScript = string.Empty;
    public string direction = "ltr";
    public string enableBackForwButton = string.Empty;
    static string errorLog = string.Empty;
    string txtFilter = string.Empty;
    public bool isTstPop = false;
    ArrayList arrPages = new ArrayList();
    ArrayList arrForms = new ArrayList();
    ArrayList arrReports = new ArrayList();
    static LogFile.Log logObj = new LogFile.Log();
    static Util.Util util = new Util.Util();
    static ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    int totalResrows = 0;
    public string langType = "en";
    public bool isCloudApp = false;
    public static bool enableBackButton = false;
    public static bool enableForwardButton = false;
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
        //util.IsValidAxpertSession();
        ResetSessionTime();
        lblNoRecordMsg.Visible = false;
        Session["AllResp"] = null;
        if (Session["project"] == "" || Session["project"] == null)
        {
            SessExpires();
        }
        else
        {
            if (!util.CheckValidLogin())
            {
                SessExpires();
                return;
            }
            //if (!util.licencedValidSessionCheck())
            //{
            //    HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG, false);
            //    return;
            //}
            GetGlobalVariables();
            if (!IsPostBack)
            {
                Session["CurrentRes"] = "";
                ViewState["ResAction"] = "";
                ViewState["act"] = "";
            }
            else
            {
            }

            if (Session["backForwBtnPressed"] != null && !Convert.ToBoolean(Session["backForwBtnPressed"]))
            {
                util.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri);
            }
            Session["backForwBtnPressed"] = false;
            enableBackButton = Convert.ToBoolean(Session["enableBackButton"]);
            enableForwardButton = Convert.ToBoolean(Session["enableForwardButton"]);

        }
        if (ConfigurationManager.AppSettings["isCloudApp"] != null)
        {
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString()); ;
        }
        AddCustomLinks();
        Page.ClientScript.RegisterStartupScript(GetType(), "set global var in iview", "<script>var isCloudApp = '" + isCloudApp.ToString() + "';parent.parent.$.LoadingOverlay('hide', true);</script>");
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            //ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", true);
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

    #region # Responsibilities #

    //protected void btnReSearch_Click(object sender, EventArgs e)
    //{
    //    string act = ViewState["act"].ToString();
    //    lvPage.SelectedValue = "1";
    //    BindGrids(txtReSeResp.Text, "", "", act);
    //}

    //public void BindGrids(string res, string page, string struc, string act)
    //{
    //    string SelResPageNo = string.Empty;
    //    string ixml = string.Empty;
    //    if (lvPage.SelectedValue == "")
    //        SelResPageNo = "1";
    //    else
    //        SelResPageNo = lvPage.SelectedValue;

    //    errorLog = logObj.CreateLog("Calling GetResponsibilityList Service", sid, "GetResponsibilities", "new");
    //    ixml = "<root axpapp='" + proj + "' sessionid= '" + sid + "' trace='" + errorLog + "' act='" + act + "' res='" + res + "' page='" + page + "' struct='" + struc + "' pagesize='0' pageno='0'>";
    //    ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + "</root>";
    //    string result = string.Empty;

    //    //Call service
    //    result = objWebServiceExt.CallGetResponsibilityListWS("Responsibilities", ixml);

    //    if (string.IsNullOrEmpty(result))
    //    {
    //        dvErr.Style.Add("display", "block");
    //        errorMessage.Value = lblNodata.Text;// Constants.REC_NOT_FOUND ;
    //        BindBlankGrid("Res", "");
    //    }
    //    else if (result.Contains(Constants.ERROR) == true)
    //    {
    //        result = util.ParseXmlErrorNode(result);
    //        if (result == Constants.SESSIONERROR || result == Constants.SESSIONEXPMSG)
    //        {
    //            Session.RemoveAll();
    //            Session.Abandon();
    //            SessExpires();
    //            return;
    //        }
    //        Response.Redirect("./err.aspx?errmsg=" + result);
    //    }
    //    else
    //    {
    //        XmlDocument xmlDoc = new XmlDocument();
    //        xmlDoc.LoadXml(result);
    //        XmlNode rootNode = default(XmlNode);
    //        rootNode = xmlDoc.SelectSingleNode("root");

    //        XmlNodeList WrproductNodes = default(XmlNodeList);
    //        WrproductNodes = xmlDoc.GetElementsByTagName("row");
    //        if ((WrproductNodes.Count == 0))
    //        {
    //            dvErr.Style.Add("display", "block");
    //            errorMessage.Value = lblNodata.Text;// Constants.REC_NOT_FOUND;
    //            BindBlankGrid("Res", "");
    //        }
    //        else
    //        {
    //            DataSet ds = new DataSet();
    //            System.IO.StringReader sr = new System.IO.StringReader(result);
    //            ds.ReadXml(sr);
    //            if (ds.Tables["row"].Rows.Count > 0)
    //            {
    //                int Tabindx = 0;
    //                if (ds.Tables[0].TableName == "row")
    //                    Tabindx = 0;
    //                else
    //                    Tabindx = 1;

    //                ds = SetAccessLngt(ds, Tabindx);
    //                DataView dv = ds.Tables["row"].DefaultView;
    //                grdReList.DataSource = ds.Tables["row"];
    //                grdReList.DataBind();
    //                AddMoreLink(ds, grdReList, "lnkReLstMore", Tabindx);

    //                //If pgno = "1" Then
    //                if (SelResPageNo == "1")
    //                {
    //                    if (rootNode.Attributes["totalrows"] != null)
    //                        totalResrows = Convert.ToInt32(rootNode.Attributes["totalrows"].Value);
    //                    Session["totalResrows"] = totalResrows;
    //                }
    //                else
    //                {
    //                    totalResrows = Convert.ToInt32(Session["totalResrows"]);
    //                }

    //                double pg = (int)totalResrows / (int)grdReList.PageSize;
    //                int pg1 = (int)Math.Floor(pg);
    //                if ((totalResrows % grdReList.PageSize) > 0)
    //                    pg1 += 1;

    //                if (totalResrows > 0)
    //                {
    //                    records.Text = "Total no. of records : " + totalResrows + " - " + "Pages : " + pg1;
    //                    lvPage.Visible = true;
    //                }
    //                else
    //                {
    //                    pgCap.Visible = false;
    //                    lvPage.Visible = false;
    //                }

    //                int pgno = 0;
    //                lvPage.Items.Clear();
    //                for (pgno = 1; pgno <= pg1; pgno++)
    //                {
    //                    lvPage.Items.Add(pgno.ToString());
    //                }
    //                lvPage.SelectedValue = SelResPageNo;
    //            }
    //        }
    //    }
    //}

    //protected void imgListAllRes_Click(object sender, EventArgs e)
    //{
    //    rdbtnLstResActive.SelectedIndex = 0;
    //    txtReSeResp.Text = "";
    //    BindGrids("", "", "", "");
    //}

    //protected void imgAddNewRes_Click(object sender, EventArgs e)
    //{
    //    txtReSeResp.Text = "";
    //    ViewState["ResAction"] = "AddRes";
    //    ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "addResponsibility", "parent.displayBootstrapModalDialog('Add Responsibility', 'md', '430px', true, '../aspx/AddEditResponsibility.aspx?action=add',true);ShowDimmer(true);", true);
    //}

    ///// <summary>
    ///// Function to display Details of multiple responsibilities.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void lnkReLstEdit_Click(object sender, EventArgs e)
    //{
    //    string result = string.Empty;
    //    bool respStatus = false;
    //    ViewState["ResAction"] = "Update";
    //    GridViewRow row = ((LinkButton)sender).Parent.Parent as GridViewRow;
    //    int rowIndx = row.RowIndex;
    //    LinkButton lblResp = (LinkButton)grdReList.Rows[rowIndx].FindControl("lnkReLstEdit");
    //    Label lblActive = (Label)grdReList.Rows[rowIndx].FindControl("lblReLstActive");

    //    if (Session["CurrentRes"] == null)
    //        Session["CurrentRes"] = "";

    //    Session["CurrentRes"] = lblResp.Text;
    //    if (lblActive.Text == "Y")
    //        respStatus = true;
    //    else
    //        respStatus = false;

    //    ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "editResponsibility", "parent.displayBootstrapModalDialog('Edit Responsibility', 'md', '430px', true, '../aspx/AddEditResponsibility.aspx?CurrentResStatus=" + respStatus + "&action=edit',true);ShowDimmer(true);", true);
    //}

    /// <summary>
    /// function for replacing the special characters in a given string.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <remarks></remarks>
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


    ///// <summary>
    ///// Function to Delete Responsibilities.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void imdDelRes_Click(object sender, EventArgs e)
    //{
    //    string resName = string.Empty;
    //    int i = 0;
    //    Boolean isChecked = false;
    //    for (i = 0; i < grdReList.Rows.Count; i++)
    //    {
    //        LinkButton lblResp = (LinkButton)grdReList.Rows[i].FindControl("lnkReLstEdit");
    //        CheckBox Chk = (CheckBox)grdReList.Rows[i].FindControl("chkReLstCheck");
    //        if (Chk.Checked == true)
    //        {
    //            isChecked = true;
    //            if (resName == string.Empty)
    //                resName = lblResp.Text;
    //            else
    //                resName += "," + lblResp.Text;
    //        }
    //    }
    //    if (isChecked == true)
    //    {
    //        txtReSeResp.Text = "";
    //        string fileName = "ProcessRole-Delete Responsibility";
    //        errorLog = logObj.CreateLog("Loading Structure.", sid, fileName, "new");
    //        string accessRgtXml = string.Empty;
    //        accessRgtXml = "<root axpapp='" + proj + "' trace='" + errorLog + "' action='remove'  role='" + resName + "' sessionid= '" + sid + "'>";
    //        string res = string.Empty;
    //        accessRgtXml += Session["axApps"].ToString() + Application["axProps"].ToString() + "</root>";

    //        //Call service
    //        res = objWebServiceExt.CallProcessRoleWS("Resp", accessRgtXml);

    //        if (res.Contains(Constants.ERROR) == true)
    //        {
    //            res = util.ParseXmlErrorNode(res);
    //            if (res == Constants.SESSIONERROR || res == Constants.SESSIONEXPMSG)
    //            {
    //                Session.RemoveAll();
    //                Session.Abandon();
    //                return;
    //            }
    //        }
    //        else if (res.Contains("success"))
    //        {
    //            if (res.Contains("assigned"))
    //            {
    //                res = res.Replace(Constants.SUCCESS, "");
    //                res = res.Replace(Constants.SUCCESSCLOSE, "");
    //                ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning',\"" + res + "\")", true);
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('success','Responsibility deleted successfully')", true);
    //            }
    //        }
    //        BindGrids(txtReSeResp.Text, "", "", "");
    //    }
    //}

    //protected void btnSearchNode_Click(object sender, EventArgs e)
    //{
    //    ClearTreeSelection(treeEditRes);
    //    treeEditRes.CollapseAll();

    //    if (txtSearchNode.Text != "")
    //    {
    //        int i = 0;
    //        Boolean nodeFound = false;
    //        for (i = 0; i < treeEditRes.Nodes.Count; i++)
    //        {
    //            if (treeEditRes.Nodes[i].Text.ToLower() == txtSearchNode.Text)
    //            {
    //                treeEditRes.Focus();
    //                treeEditRes.Nodes[i].Selected = true;
    //                treeEditRes.Nodes[i].Select();
    //                nodeFound = true;
    //                break;
    //            }
    //            else
    //            {
    //                if (nodeFound == false)
    //                {
    //                    treeEditRes.Nodes[i].Expand();

    //                    if (treeEditRes.Nodes[i].ChildNodes.Count > 0)
    //                    {
    //                        int j = 0;
    //                        TreeNodeCollection chldNodes = treeEditRes.Nodes[i].ChildNodes;
    //                        for (j = 0; j < chldNodes.Count; j++)
    //                        {
    //                            if (chldNodes[j].Text.ToLower() == txtSearchNode.Text.ToLower())
    //                            {
    //                                treeEditRes.Focus();
    //                                chldNodes[j].Selected = true;
    //                                chldNodes[j].Select();
    //                                nodeFound = true;
    //                                break;
    //                            }
    //                            else
    //                            {
    //                                chldNodes[j].Expand();
    //                                if (chldNodes[j].ChildNodes.Count > 0 && nodeFound == false)
    //                                {
    //                                    TreeNodeCollection leafNodes = chldNodes[j].ChildNodes;
    //                                    int k = 0;
    //                                    for (k = 0; k < leafNodes.Count; k++)
    //                                    {
    //                                        if (leafNodes[k].Text.ToLower() == txtSearchNode.Text)
    //                                        {
    //                                            treeEditRes.Focus();
    //                                            leafNodes[k].Selected = true;
    //                                            leafNodes[k].Select();

    //                                            nodeFound = true;
    //                                            break;
    //                                        }
    //                                    }

    //                                }
    //                            }
    //                            if (nodeFound == true)
    //                                break;
    //                        }
    //                    }
    //                }
    //            }
    //            if (nodeFound == true)
    //                break;
    //        }

    //        if (nodeFound == false)
    //        {
    //            errorMessage.Value = "Access Right not found.";
    //            treeEditRes.CollapseAll();
    //        }
    //        else
    //        {
    //            lblDisErrMsg.Text = "";
    //            dvPopErrMsg.Style.Add("display", "none");
    //        }
    //    }
    //    else
    //    {
    //        dvPopErrMsg.Style.Add("display", "block");
    //        lblDisErrMsg.Text = "Please enter the Access Right to be searched.";
    //        errorMessage.Value = "Please enter the Access Right to be searched.";
    //    }
    //}

    //protected void rdbtnLstResActive_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    totalResrows = Convert.ToInt32(Session["totalResrows"]);
    //    string act = "";
    //    lvPage.SelectedIndex = 0;
    //    if (rdbtnLstResActive.SelectedIndex == 0)
    //        act = "";
    //    else if (rdbtnLstResActive.SelectedIndex == 1)
    //        act = "Y";
    //    else
    //        act = "N";
    //    ViewState["act"] = act;
    //    lvPage.SelectedValue = "1";
    //    txtReSeResp.Text = "";
    //    BindGrids("", "", "", act);
    //}

    //private void ClearTreeSelection(TreeView tree)
    //{
    //    for (int i = 0; i < tree.Nodes.Count; i++)
    //    {
    //        if (tree.Nodes[i].Selected == true)
    //            tree.Nodes[i].Selected = false;
    //        if (tree.Nodes[i].ChildNodes.Count > 0)
    //        {
    //            TreeNodeCollection chldNodes = tree.Nodes[i].ChildNodes;
    //            for (int j = 0; j < chldNodes.Count; j++)
    //            {
    //                if (chldNodes[j].Selected == true)
    //                    chldNodes[j].Selected = false;

    //                if (chldNodes[j].ChildNodes.Count > 0)
    //                {
    //                    TreeNodeCollection leafNodes = chldNodes[j].ChildNodes;
    //                    for (int k = 0; k < leafNodes.Count; k++)
    //                    {
    //                        if (leafNodes[k].Selected == true)
    //                            leafNodes[k].Selected = false;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}


    #endregion
    #region # General Functions #
    private DataSet SetAccessLngt(DataSet ds, int tblIdx)
    {
        for (int i = 0; i < ds.Tables[tblIdx].Rows.Count; i++)
        {
            string strAccRgts = ds.Tables[tblIdx].Rows[i][1].ToString().Trim();
            bool bigResp = false;
            if (strAccRgts.Length > 80)
            {
                strAccRgts = strAccRgts.Substring(0, 80);
                bigResp = true;
            }
            if (strAccRgts.EndsWith(","))
                strAccRgts = strAccRgts.Substring(0, strAccRgts.Length - 1);
            if (bigResp)
                strAccRgts += "...";
            ds.Tables[tblIdx].Rows[i][1] = strAccRgts;
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
                lnk.Visible = true;
            else
                lnk.Visible = false;
        }
    }

    //private void BindBlankGrid(string Name, string SourceCall)
    //{
    //    if (Name == "Res")
    //    {
    //        grdReList.DataSource = null;
    //        grdReList.DataBind();
    //        lblNoRecordMsg.Visible = true;
    //        lblNoRecordMsg.Text = errorMessage.Value;
    //        records.Text = string.Empty;
    //        pgCap.Text = string.Empty;
    //        lvPage.Visible = false;
    //    }
    //}

    /// <summary>
    /// Function to Copy Responsibility.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void imgCopyRes_Click(object sender, EventArgs e)
    //{
    //    string resName = string.Empty;
    //    int checkedResp = -1;
    //    for (int i = 0; i < grdReList.Rows.Count; i++)
    //    {
    //        CheckBox Chk = (CheckBox) grdReList.Rows[i].FindControl("chkReLstCheck");
    //        if (Chk.Checked == true)
    //        {
    //            checkedResp = i;
    //            break;
    //        }
    //    }
    //    if (checkedResp != -1)
    //    {
    //        string result = string.Empty;
    //        LinkButton lblResp = (LinkButton) grdReList.Rows[checkedResp].FindControl("lnkReLstEdit");
    //        ViewState["ResAction"] = "Copy";
    //        Session["CurrentRes"] = lblResp.Text;
    //        ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "copyResponsibility", "parent.parent.displayBootstrapModalDialog('Copy Responsibility', 'md', '430px', true, '../aspx/AddEditResponsibility.aspx?CurrentResStatus=" + lblResp.Text + "&action=copy',true);ShowDimmer(true);", true);
    //    }
    //}

    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    #endregion

    //#region # Paging for Gridviews #
    //protected void lvPage_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string act = ViewState["act"].ToString();
    //    BindGrids(txtReSeResp.Text, "", "", act);
    //}
    //#endregion


    //protected void btnRefreshGrid_Click(object sender, EventArgs e)
    //{
    //    BindGrids("", "", "", "");
    //}

    class Responsibility
    {
        public string Name { get; set; }
        public string AccessRights { get; set; }
        public string Status { get; set; }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static object BindGridData(string act)
    {
        string SelResPageNo = string.Empty;
        string ixml = string.Empty;
        string response = string.Empty, result = string.Empty;

        errorLog = logObj.CreateLog("Calling GetResponsibilityList Service", sid, "GetResponsibilities", "new");
        ixml = "<root axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid= '" + HttpContext.Current.Session["nsessionid"].ToString() + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace='" + errorLog + "' act='" + act + "' res='' page='' struct='' pagesize='0' pageno='0'>";
        ixml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        //Call service
        response = objWebServiceExt.CallGetResponsibilityListWS("Responsibilities", ixml);

        if (response.Contains(Constants.ERROR) == true)
        {
            if (response.Contains(Constants.SESSIONERROR) || response.Contains(Constants.SESSIONEXPMSG))
                result = "SessionExpiry";
            else
                result = "Error";
        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);
            XmlNode rootNode = default(XmlNode);
            rootNode = xmlDoc.SelectSingleNode("root");

            XmlNodeList WrproductNodes = default(XmlNodeList);
            WrproductNodes = xmlDoc.GetElementsByTagName("row");
            if ((WrproductNodes.Count == 0))
            {
                return new List<Responsibility>();
            }
            else
            {
                DataSet ds = new DataSet();
                System.IO.StringReader sr = new System.IO.StringReader(response);
                ds.ReadXml(sr);
                if (ds.Tables["row"].Rows.Count > 0)
                {
                    int Tabindx = 0;
                    if (ds.Tables[0].TableName == "row")
                        Tabindx = 0;
                    else
                        Tabindx = 1;
                    var empList = ds.Tables[0].AsEnumerable().Select(dataRow => new Responsibility
                    {
                        Name = "<a style='cursor:pointer' onclick=\"ShowDimmer(true);BootstrapDialogResponsibiltyShow('../aspx/AddEditResponsibility.aspx?status=" + (dataRow.Field<string>("act") == "Y" ? "true" : "false") + "&action=edit&name=" + dataRow.Field<string>("res") + "', true)\" class='edit-resp' data-status='" + (dataRow.Field<string>("act") == "Y" ? "true" : "false") + "'>" + dataRow.Field<string>("res") + "</a>",
                        AccessRights = (dataRow.Field<string>("ar").Length > 1 ? dataRow.Field<string>("ar").Substring(0, dataRow.Field<string>("ar").Length - 2) : dataRow.Field<string>("ar"))
                        //AccessRights = dataRow.Field<string>("ar").Length > 150 ? dataRow.Field<string>("ar").Substring(0, 150) + "..." : (dataRow.Field<string>("ar").Length > 1 ? dataRow.Field<string>("ar").Substring(0, dataRow.Field<string>("ar").Length - 2) : dataRow.Field<string>("ar")),
                        //Status = dataRow.Field<string>("act") == "Y" ? "Yes" : "No"
                    }).ToList();
                    return empList;
                }
            }
        }
        return result;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static object DeleteResponsibilities(string selectedResp)
    {
        //string selectedResp = string.Empty;
        string result = string.Empty;
        string fileName = "ProcessRole-Delete Responsibility";
        errorLog = logObj.CreateLog("Loading Structure.", sid, fileName, "new");
        string accessRgtXml = string.Empty;
        accessRgtXml = "<root axpapp= '" + HttpContext.Current.Session["project"].ToString() + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace='" + errorLog + "' action='remove'  role='" + selectedResp + "' sessionid= '" + HttpContext.Current.Session["nsessionid"].ToString() + "'>";
        string res = string.Empty;
        accessRgtXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        //Call service
        res = objWebServiceExt.CallProcessRoleWS("Resp", accessRgtXml);
        if (res.Contains(Constants.ERROR) == true)
        {
            res = util.ParseXmlErrorNode(res);
            if (res.Contains(Constants.SESSIONERROR) || res.Contains(Constants.SESSIONEXPMSG))
                result = "SessionExpiry";
            else
                result = "Error";
        }
        else if (res.Contains("success"))
        {
            if (res.Contains("assigned"))
                result = res;
            else
            {
                result = "Success";
            }
        }
        return result;
    }
    private void AddCustomLinks()
    {
        Custom cusObj = Custom.Instance;
        string projName = HttpContext.Current.Session["Project"].ToString();
        for (int i = 0; i < cusObj.jsPageName.Count; i++)
        {
            string fileName = string.Empty;
            if (cusObj.jsPageName[i].ToString() == "Responsibilities.aspx")
            {
                fileName = cusObj.jsPageFiles[i].ToString();
                FileInfo filtcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/" + fileName));
                if (filtcustom.Exists)
                {

                    HtmlGenericControl js = new HtmlGenericControl("script");
                    js.Attributes["type"] = "text/javascript";
                    string path = "../" + projName + "/" + fileName + "?v=" + filtcustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                    js.Attributes["src"] = path;
                    ScriptManager1.Controls.Add(js);
                }
            }
        }

        for (int j = 0; j < cusObj.cssPageName.Count; j++)
        {
            string fileName = string.Empty;
            if (cusObj.cssPageName[j].ToString() == "Responsibilities.aspx")
            {
                fileName = cusObj.cssPageFiles[j].ToString();
                FileInfo filtcsscustom = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projName + "/" + fileName));
                if (filtcsscustom.Exists)
                {
                    HtmlGenericControl css = new HtmlGenericControl("link");
                    css.Attributes["type"] = "text/css";
                    css.Attributes["rel"] = "stylesheet";
                    string path = "../" + projName + "/" + fileName + "?v=" + filtcsscustom.LastWriteTime.ToString("MMddyyyyHHmmss");
                    css.Attributes["href"] = path;
                    ScriptManager1.Controls.Add(css);
                }
            }
        }
    }
}
