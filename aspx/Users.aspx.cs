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

public partial class aspx_Users : System.Web.UI.Page
{
    public string proj = string.Empty;
    public string sid = string.Empty;
    public string trace = string.Empty;
    public string user = string.Empty;
    public string acScript = string.Empty;
    public string strGlobalVar = string.Empty;
    string errorLog = string.Empty;
    string txtFilter = string.Empty;
    public string direction = "ltr";

    ArrayList arrPages = new ArrayList();
    ArrayList arrForms = new ArrayList();
    ArrayList arrReports = new ArrayList();
    LogFile.Log logObj = new LogFile.Log();
    Util.Util util = new Util.Util();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    string clientCulture = string.Empty;
    int totalUserrows = 0;
    bool isPwdAlphaNumeric = false;

    public bool checkBfNav = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        util.IsValidSession();
        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = util.toggleTheme();
        lblNoRecordMsg.Visible = false;

        if (Session["project"] == "" || Session["project"] == null)
        {
            SessExpires();
        }
        else
        {
            if (Request.UrlReferrer != null)
            {
                if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("main.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("users.aspx")))
                    Response.Redirect("../cusError/axcustomerror.aspx");
            }
            pwdlength.Value = Session["minPwdChars"].ToString();

            clientCulture = Convert.ToString(Session["ClientLocale"]);
            if (!IsPostBack)
            {
                GetGlobalVariables();
                BindUsers("", "");
                rdbtnListActUsers.SelectedIndex = 0;
                Session["CurrentRes"] = "";
                ViewState["ResAction"] = "";
                ViewState["act"] = "";
                if (Session["IsPwdAlphaNumeric"].ToString().ToLower() == "t")
                    isPwdAlphaNumeric = true;

                Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script language=JavaScript>isPwdAlphaNumeric = " + isPwdAlphaNumeric.ToString().ToLower() + "</script>");
            }
            else
            {
                SetGlobalVariables();
                if (hdnIsSearched.Value != "Go")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script language=JavaScript>HideDimmer();</script>");
                }
                dvErr2.Style.Add("display", "none");
                userErr.Value = "";
                userSuccess.Value = "";
                Session["selectedUser"] = null;
                if (!(String.IsNullOrEmpty(txtUsAdsw.Text.Trim())))
                {
                    txtUsAdsw.Attributes["value"] = txtUsAdsw.Text;
                }
                if (!(String.IsNullOrEmpty(txtUsAdConsw.Text.Trim())))
                {
                    txtUsAdConsw.Attributes["value"] = txtUsAdConsw.Text;
                }
            }
        }
        AddEvents();
    }
    private void GetGlobalVariables()
    {
        proj = Session["project"].ToString();
        ViewState["proj"] = proj;
        user = Session["user"].ToString();
        ViewState["user"] = user;
        sid = Session["nsessionid"].ToString();
        ViewState["sid"] = sid;
        strGlobalVar = util.GetGlobalVarString();
        if (Session["language"].ToString() == "ARABIC")
        {
            direction = "rtl";
        }
    }
    private void AddEvents()
    {
        txtUsAdName.Attributes.Add("onblur", "CapUserChangeEvent()");
        txtUsAdsw.Attributes.Add("onblur", "CapUserChangeEvent(),md5auth1(this);");
        txtUsAdConsw.Attributes.Add("onblur", "CapUserChangeEvent()");
        pwdExpiryDays.Attributes.Add("onblur", "CapUserChangeEvent(),ValidatePswdExpDays()");
        txtUsAdEmail.Attributes.Add("onblur", "CapUserChangeEvent()");
        SrcUser000F0.Attributes.Add("onblur", "CapUserChangeEvent()");

    }

    private void SetGlobalVariables()
    {
        proj = ViewState["proj"].ToString();
        sid = ViewState["sid"].ToString();
        user = ViewState["user"].ToString();
    }

    #region # Users #

    protected DataTable GetRolesForUsers()
    {
        DataTable dTable = null;
        //Added to avoid server call for getting the user role each time
        if (ViewState["AllRolesForCurrentUser"] != null)
        {
            DataSet ds = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(ViewState["AllRolesForCurrentUser"].ToString());
            ds.ReadXml(sr);
            dTable = ds.Tables[2];
            return dTable;
        }

        string txt3 = "<sqlresultset axpapp='" + proj + "' sessionid='" + sid + "' trace='" + trace + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' >";
        txt3 += "<sql>select AG.userroles as name, AG.groupname as USERGROUP from axusergroups AG order by AG.groupname</sql>";
        txt3 += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        string result = string.Empty;
        //Call service
        result = objWebServiceExt.CallGetChoiceWS("user", txt3);
        ViewState["AllRolesForCurrentUser"] = result;
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
            DataSet ds = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            ds.ReadXml(sr);
            dTable = ds.Tables[2];
        }
        return dTable;
    }

    protected void lnkAddUser_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("STARTDATE");
        dt.Columns.Add("ENDDATE");
        dt.Columns.Add("USERGROUP");
        for (int i = 0; i < grdUsRoles.Rows.Count; i++)
        {
            TextBox lblStrDt = (TextBox)grdUsRoles.Rows[i].FindControl("txtUsStrDt");
            TextBox lblEndDt = (TextBox)grdUsRoles.Rows[i].FindControl("txtUsEndDt");
            DropDownList lblRole = (DropDownList)grdUsRoles.Rows[i].FindControl("ddlUsRole");
            DataRow dr1 = dt.NewRow();
            dr1["STARTDATE"] = lblStrDt.Text;
            dr1["ENDDATE"] = lblEndDt.Text;
            dr1["USERGROUP"] = lblRole.SelectedValue;
            if (lblRole.SelectedValue == "")
            {
                string index = (i + 1).ToString();
                userErr.Value = "Please select a Role in row " + index;
                return;
            }
            dt.Rows.Add(dr1);
        }
        DataRow dr = dt.NewRow();
        dr["STARTDATE"] = clientCulture.ToLower() == "en-gb" ? DateTime.Today.ToString("dd/MM/yyyy") : DateTime.Today.ToString("MM/dd/yyyy");
        dr["ENDDATE"] = "";
        dr["USERGROUP"] = "";
        dt.Rows.Add(dr);
        ds.Tables.Add(dt);
        Session["UserRoles"] = ds.Tables[0];
        grdUsRoles.DataSource = ds;
        grdUsRoles.DataBind();
    }

    protected void imgAddNewUs_Click(object sender, EventArgs e)
    {
        txtUsAdsw.Attributes["value"] = string.Empty;
        txtUsAdConsw.Attributes["value"] = string.Empty;
        dvErr2.Style.Add("display", "none");
        ViewState["Useraction"] = "add";
        BindBlankGrid("Users", "Adduser");
        txtUsAdName.Text = "";
        txtUsAdName.ReadOnly = false;
        txtUsAdsw.Text = "";
        txtUsAdConsw.Text = "";
        chkUsAdAct.Checked = true;
        txtUsAdEmail.Text = "";
        pwdExpiryDays.Text = "";
        //Add Empty row
        ViewState["NewUser"] = "true";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("STARTDATE");
        dt.Columns.Add("ENDDATE");
        dt.Columns.Add("USERGROUP");
        DataRow dr = dt.NewRow();
        dr["STARTDATE"] = clientCulture.ToLower() == "en-gb" ? DateTime.Today.ToString("dd/MM/yyyy") : DateTime.Today.ToString("MM/dd/yyyy");
        dr["ENDDATE"] = "";
        dr["USERGROUP"] = "";
        dt.Rows.Add(dr);
        ds.Tables.Add(dt);
        grdUsRoles.DataSource = ds;
        grdUsRoles.DataBind();

    }

    protected void imdDelUser_Click(object sender, EventArgs e)
    {
        txtUsSeUser.Text = "";
        Boolean isChkd = false;
        string Selusers = string.Empty; string res = string.Empty;
        for (int i = 0; i < grdUsLstUsers.Rows.Count; i++)
        {
            RadioButton chkUsCheck = (RadioButton)grdUsLstUsers.Rows[i].FindControl("chkUsLstCheck");
            LinkButton lnk = (LinkButton)grdUsLstUsers.Rows[i].FindControl("lnkUsName");
            user = Session["user"].ToString();

            if (chkUsCheck.Checked == true)
            {
                isChkd = true;
                if (Selusers == "")
                    Selusers = lnk.Text;
                else
                    Selusers += "," + lnk.Text;
                if (lnk.Text == user)
                {
                    userErr.Value = "Logged in user cannot be deleted.";
                    return;
                }
            }
        }

        if (isChkd == true)
        {
            string fileName = "ProcessUsers";
            string errorLog = logObj.CreateLog("Save or delete users.", sid, fileName, "new");
            string ixml = string.Empty;
            ixml = "<root axpapp='" + proj + "' loginuser='" + user + "'  sessionid='" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' user='" + Selusers + "' action='remove'>";
            ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
            //Call service
            res = objWebServiceExt.CallProcessUserWS("user", ixml);

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
                dvErr2.Style.Add("display", "block");
                userErr.Value = res;
            }
            else
            {
                txtUsSeUser.Text = "";
                BindUsers("", "");
                dvErr2.Style.Add("display", "block");
                userSuccess.Value = "User Deleted Successfully.";
            }
        }
    }

    protected void imgListAllUser_Click(object sender, EventArgs e)
    {
        txtUsSeUser.Text = "";
        rdbtnListActUsers.SelectedIndex = 0;
        lvPageUser.SelectedValue = "1";
        txtUsAdsw.Attributes["value"] = string.Empty;
        txtUsAdConsw.Attributes["value"] = string.Empty;
        BindUsers("", "");
    }

    protected void lnkUsName_Click(object sender, EventArgs e)
    {
        txtUsAdsw.Attributes["value"] = string.Empty;
        txtUsAdConsw.Attributes["value"] = string.Empty;
        GridViewRow row = ((LinkButton)sender).Parent.Parent as GridViewRow;
        int rowIndx = row.RowIndex;
        string res = string.Empty;
        ViewState["Useraction"] = "update";
        ViewState["NewUser"] = "false";
        LinkButton lnk = (LinkButton)grdUsLstUsers.Rows[rowIndx].FindControl("lnkUsName");

        string fileName = "GetUserDetails";
        string errorLog = logObj.CreateLog("Get User Details.", sid, fileName, "new");
        string ixml = string.Empty;
        ixml = "<root axpapp='" + proj + "' loginuser='" + user + "'  sessionid='" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' user='" + lnk.Text + "'>";
        ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
        //Call service
        res = objWebServiceExt.CallGetUserDetailsWS("user", ixml);

        if (res != "")
        {
            DataSet ds = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(res);
            ds.ReadXml(sr);
            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    DataTable dTable = ds.Tables[2];
                    grdUsRoles.DataSource = ds.Tables[2];
                    Session["SelUserRole"] = dTable;
                    Session["UserRoles"] = dTable;
                    try
                    {
                        grdUsRoles.DataBind();
                    }
                    catch { userErr.Value = "Role of this user has been deleted externally."; }
                }
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(res);
            XmlNode rootNode = default(XmlNode);
            rootNode = xmlDoc.SelectSingleNode("root");
            string uname = ""; string mailid = "";
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
            {
                if (rootNode.ChildNodes[i].Name == "uname")
                {
                    uname = rootNode.ChildNodes[i].InnerText;
                    txtUsAdName.Text = uname;
                    txtUsAdName.ReadOnly = true;
                    Session["selectedUser"] = uname;
                }

                else if (rootNode.ChildNodes[i].Name == "mailid")
                {
                    mailid = rootNode.ChildNodes[i].InnerText;
                    txtUsAdEmail.Text = mailid;
                }

                else if (rootNode.ChildNodes[i].Name == "build")
                {
                    if (rootNode.ChildNodes[i].InnerText == "T")
                        chkAvailBuild.Checked = true;
                    else
                        chkAvailBuild.Checked = false;
                }
                else if (rootNode.ChildNodes[i].Name == "manage")
                {
                    if (rootNode.ChildNodes[i].InnerText == "T")
                        chkShowUsers.Checked = true;
                    else
                        chkShowUsers.Checked = false;
                }
                else if (rootNode.ChildNodes[i].Name == "axpertmanager")
                {
                    if (rootNode.ChildNodes[i].InnerText == "T")
                        chkAvailAxManager.Checked = true;
                    else
                        chkAvailAxManager.Checked = false;
                }
                else if (rootNode.ChildNodes[i].Name == "send")
                {
                    if (rootNode.ChildNodes[i].InnerText == "T")
                        chkAvailSync.Checked = true;
                    else
                        chkAvailSync.Checked = false;
                }
                else if (rootNode.ChildNodes[i].Name == "workflow")
                {
                    if (rootNode.ChildNodes[i].InnerText == "T")
                        chkShowWorkflow.Checked = true;
                    else
                        chkShowWorkflow.Checked = false;
                }
                else if (rootNode.ChildNodes[i].Name == "act")
                {
                    if (rootNode.ChildNodes[i].InnerText == "Y" || rootNode.ChildNodes[i].InnerText == "" || rootNode.ChildNodes[i].InnerText == "T")
                        chkUsAdAct.Checked = true;
                    else
                        chkUsAdAct.Checked = false;
                }
                else if (rootNode.ChildNodes[i].Name == "imp")
                {
                    if (rootNode.ChildNodes[i].InnerText == "T")
                        chkAvailImport.Checked = true;
                    else
                        chkAvailImport.Checked = false;
                }
                else if (rootNode.ChildNodes[i].Name == "exp")
                {
                    if (rootNode.ChildNodes[i].InnerText == "T")
                        chkAvailExport.Checked = true;
                    else
                        chkAvailExport.Checked = false;
                }
                else if (rootNode.ChildNodes[i].Name == "reportingto")
                {
                    SrcUser000F0.Text = rootNode.ChildNodes[i].InnerText;
                }
                else if (rootNode.ChildNodes[i].Name.ToLower() == "pwdexpdays")
                {
                    pwdExpiryDays.Text = rootNode.ChildNodes[i].InnerText;
                }
            }
            txtUsAdsw.Text = "*****";
            txtUsAdConsw.Text = "*****";
        }


    }

    protected void btnUsAdSave_Click(object sender, EventArgs e)
    {
        Boolean valPwd = true;
        Boolean valUg = true;
        Boolean valUn = true; string Newpwd = ""; string ConPwd = "";
        string userGrp = "<ug>"; string res = string.Empty;
        string action = string.Empty;
        if (ViewState["Useraction"] != null && ViewState["Useraction"].ToString() == "update")
            action = "update";
        else
            action = "add";
        if (txtUsAdName.Text == "")
        {
            dvErr2.Style.Add("display", "none");
            userErr.Value = "Please enter the user name.";
            valUn = false;
            return;
        }
        else if (txtUsAdsw.Text == "" || txtUsAdConsw.Text == "")
        {
            if (action != "update")
            {
                dvErr2.Style.Add("display", "none");
                userErr.Value = "Please enter the password.";
                valPwd = false;
                return;
            }
        }

        else if (txtUsAdsw.Text != txtUsAdConsw.Text)
        {
            dvErr2.Style.Add("display", "none");
            userErr.Value = "The passwords entered do not match, Please retype the passwords.";
            valPwd = false;
            return;
        }

        else if (txtUsAdEmail.Text == "" || txtUsAdEmail.Text == "")
        {
            dvErr2.Style.Add("display", "none");
            userErr.Value = "Please enter the email.";
            valPwd = false;
            return;
        }
        if (txtUsAdName.Text == user && checkBoxDefaultValue.Value.ToLower() == "false")
        {
            userErr.Value = "Logged in user cannot be deactivated.";
            return;
        }
        int pwdExpDays = 0;
        if (pwdExpiryDays.Text != string.Empty && !int.TryParse(pwdExpiryDays.Text, out pwdExpDays))
        {
            userErr.Value = "Password expiry days should be an integer.";
            return;
        }

        if (ViewState["Useraction"] != null && ViewState["Useraction"].ToString() == "update")
            action = "update";
        else
            action = "add";


        for (int i = 0; i < grdUsRoles.Rows.Count; i++)
        {
            TextBox lblStrDt = (TextBox)grdUsRoles.Rows[i].FindControl("txtUsStrDt");
            TextBox lblEndDt = (TextBox)grdUsRoles.Rows[i].FindControl("txtUsEndDt");
            DropDownList lblRole = (DropDownList)grdUsRoles.Rows[i].FindControl("ddlUsRole");
            string startDate = lblStrDt.Text;
            string endDate = lblEndDt.Text;
            if (!(string.IsNullOrEmpty(clientCulture)))
            {
                string rowIndex = Convert.ToString(i + 1);
                if (lblRole.Text == "")
                {
                    userErr.Value = "Please select a Role in row " + rowIndex;
                    return;
                }
                if ((startDate == string.Empty) || (startDate == "dd/mm/yyyy") || (startDate == "mm/dd/yyyy"))
                {
                    userErr.Value = "Start date cannot be left empty in row " + rowIndex;
                    return;
                }
                if ((endDate == "dd/mm/yyyy") || (endDate == "mm/dd/yyyy"))
                    endDate = "";
                if (!string.IsNullOrEmpty(endDate) && clientCulture.ToLower() != "en-us")
                {
                    try
                    {
                        DateTime stDt = DateTime.ParseExact(startDate, @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime enDt = DateTime.ParseExact(endDate, @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (enDt < stDt)
                        {
                            userErr.Value = "Start date cannot be greater than End date in row " + rowIndex;
                            lblStrDt.Text = "";
                            lblEndDt.Text = "";
                            return;
                        }
                    }
                    catch { }
                }
                else if (!string.IsNullOrEmpty(endDate) && clientCulture.ToLower() == "en-us")
                {
                    try
                    {
                        DateTime stDt = DateTime.ParseExact(startDate, @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime enDt = DateTime.ParseExact(endDate, @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (enDt < stDt)
                        {
                            userErr.Value = "Start date cannot be greater than End date in row " + rowIndex;
                            lblStrDt.Text = "";
                            lblEndDt.Text = "";
                            return;
                        }
                    }
                    catch { }
                }
                if (clientCulture.ToLower() == "en-us")
                {
                    if (startDate != string.Empty)
                    {
                        string[] dateStr = startDate.Split('/');
                        startDate = dateStr[1] + "/" + dateStr[0] + "/" + dateStr[2];
                    }
                    if (endDate != string.Empty)
                    {
                        string[] dateStr1 = endDate.Split('/');
                        endDate = dateStr1[1] + "/" + dateStr1[0] + "/" + dateStr1[2];
                    }
                }
            }
            if (userGrp.Contains(lblRole.SelectedValue))
            {
                userErr.Value = "User Role Already Exists Try Again with Different Role Name ";

                return;

            }
            userGrp += "<row><USERGROUP>" + lblRole.SelectedValue + "</USERGROUP><STARTDATE>" + startDate + "</STARTDATE><ENDDATE>" + endDate + "</ENDDATE></row>";
        }
        userGrp += "</ug>";

        string act = "";
        //checkBoxDefaultValue is a hidden field which is added to keep the value of chkUsAdAct
        //if (chkUsAdAct.Checked == true)
        if (checkBoxDefaultValue.Value.ToLower() == "true")
            act = "T";
        else
            act = "F";

        if (txtUsAdsw.Text == "*****" && txtUsAdConsw.Text == "*****")
        {
            Newpwd = "";
            ConPwd = "";
        }
        else
        {
            Newpwd = txtUsAdsw.Text;
            ConPwd = txtUsAdConsw.Text;
        }
        string workFlow = string.Empty;
        if (chkShowWorkflow.Checked == true)
            workFlow = "T";
        else
            workFlow = "F";

        string userAccess = string.Empty;
        if (chkShowUsers.Checked == true)
            userAccess = "T";
        else
            userAccess = "F";

        string axManager = string.Empty;
        if (chkAvailAxManager.Checked == true)
            axManager = "T";
        else
            axManager = "F";

        string buildMode = string.Empty;
        if (chkAvailBuild.Checked == true)
            buildMode = "T";
        else
            buildMode = "F";

        string sync = string.Empty;
        if (chkAvailSync.Checked == true)
            sync = "T";
        else
            sync = "F";

        string imp = "F";
        string exp = "F";
        if (chkAvailImport.Checked)
            imp = "T";
        if (chkAvailExport.Checked)
            exp = "T";

        string reportingTo = SrcUser000F0.Text;

        string changeByAdmin = "no";
        if (Session["user"].ToString() == "admin" && txtUsAdName.Text != "admin")
            changeByAdmin = "yes";

        errorLog = logObj.CreateLog("saving user", sid, "SaveUser", "new");
        string inputXml = string.Empty;
        //+ "<changebyadmin>" + changeByAdmin + "</changebyadmin>" 
        inputXml = "<root axpapp='" + proj + "' loginuser='" + user + "' changebyadmin='" + changeByAdmin + "'  sessionid='" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' user='" + txtUsAdName.Text + "' action='" + action + "'>";
        inputXml += "<act>" + act + "</act>" + "<pwdexpdays>" + pwdExpDays.ToString() + "</pwdexpdays>" + "<uname>" + txtUsAdName.Text + "</uname><pwd>" + sw000F0.Value + "</pwd><cpwd>" + sw000F0.Value + "</cpwd><mailid>" + txtUsAdEmail.Text + "</mailid><build>" + buildMode + "</build><workflow>" + workFlow + "</workflow><imp>" + imp + "</imp><exp>" + exp + "</exp><manage>" + userAccess + "</manage><axpertmanager>" + axManager + "</axpertmanager><send>" + sync + "</send><reportingto>" + reportingTo + "</reportingto>";
        inputXml += userGrp + Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";

        if (txtUsAdsw.Text != txtUsAdConsw.Text)
        {
            dvErr2.Style.Add("display", "none");
            userErr.Value = "The passwords entered do not match, Please retype the passwords.";
            valPwd = false;
        }
        else if (txtUsAdEmail.Text == "" || txtUsAdEmail.Text == "")
        {
            dvErr2.Style.Add("display", "none");
            userErr.Value = "Please enter the email.";
            valPwd = false;
        }
        else if (txtUsAdsw.Text == "" || txtUsAdConsw.Text == "")
        {
            if (action != "update")
            {
                dvErr2.Style.Add("display", "none");
                userErr.Value = "Please enter the password.";
                valPwd = false;
            }
        }

        if (grdUsRoles.Rows.Count == 0)
        {
            dvErr2.Style.Add("display", "none");
            userErr.Value = "Please add a role.";
            valUg = false;
        }
        if (txtUsAdName.Text == "")
        {
            dvErr2.Style.Add("display", "none");
            userErr.Value = "Please enter the user name.";
            valUn = false;
        }
        if (valPwd == true && valUg == true && valUn == true)
        {
            //Call service
            res = objWebServiceExt.CallProcessUserWS("user", inputXml);

            Session["selectedUser"] = null;
            if (!(res.StartsWith("<success>")))
            {
                dvErr2.Style.Add("display", "none");
                userErr.Value = res;
                return;

            }
            else
            {
                txtUsSeUser.Text = "";
                BindBlankGrid("Users", action);
                rdbtnListActUsers.SelectedIndex = 0;
                BindUsers("", "");
                dvErr2.Style.Add("display", "none");
                if (action == "add")
                { userSuccess.Value = "User added successfully."; }
                else if (action == "update")
                { userSuccess.Value = "User updated successfully."; }
                txtUsAdsw.Attributes["value"] = string.Empty;
                txtUsAdConsw.Attributes["value"] = string.Empty;
            }
        }
    }

    private void BindUsers(string act, string SearchText)
    {
        string SelUserPageNo = string.Empty;
        string ixml = string.Empty;
        if (lvPageUser.SelectedValue == "")
            SelUserPageNo = "1";
        else
            SelUserPageNo = lvPageUser.SelectedValue;

        errorLog = logObj.CreateLog("GetUsersList", sid, "GetUsersList", "new");
        ixml = "<root axpapp='" + proj + "' user='" + user + "' sessionid= '" + sid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' act='" + act + "' res='' page='' struct='' role='' suser='" + SearchText + "' pagesize='10' pageno='" + SelUserPageNo + "'>";
        ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";

        string result = string.Empty;
        //Call service
        result = objWebServiceExt.CallGetUsersListWS("user", ixml);
        result = result.Replace("<act>Y</act>", "<act>T</act>");
        result = result.Replace("<act>N</act>", "<act>F</act>");

        if (string.IsNullOrEmpty(result))
        {
            dvErr2.Style.Add("display", "block");
            userErr.Value = lblNodata.Text;
            BindBlankGrid("Users", "");

        }
        else if (result.Contains(Constants.ERROR) == true)
        {
            result = util.ParseXmlErrorNode(result);
            if (result == Constants.SESSIONERROR || result == Constants.SESSIONEXPMSG)
            {
                Session.RemoveAll();
                Session.Abandon();
                SessExpires();
                return;
            }
            dvErr2.Style.Add("display", "block");
            userErr.Value = result;
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
                dvErr2.Style.Add("display", "block");
                userErr.Value = lblNodata.Text;
                BindBlankGrid("Users", "");

            }
            else
            {
                dvErr2.Style.Add("display", "none");
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
                    DataTable dTable2 = ds.Tables["row"];
                    grdUsLstUsers.DataSource = ds.Tables["row"];
                    grdUsLstUsers.DataBind();
                    AddMoreLink(ds, grdUsLstUsers, "lnlUsLstMore", tabIndx);
                    //If pgno = "1" Then
                    if (SelUserPageNo == "1")
                    {
                        if (rootNode.Attributes["totalrows"] != null)
                        {
                            totalUserrows = Convert.ToInt32(rootNode.Attributes["totalrows"].Value);
                        }
                        Session["totalUserrows"] = totalUserrows;
                    }
                    else
                    {
                        totalUserrows = Convert.ToInt32(Session["totalUserrows"]);
                    }

                    double pg = (int)totalUserrows / (int)grdUsLstUsers.PageSize;
                    int pg1 = (int)Math.Floor(pg);
                    if ((totalUserrows % grdUsLstUsers.PageSize) > 0)
                    {
                        pg1 += 1;
                    }
                    if (totalUserrows > 0)
                    {
                        recordsUser.Text = "Total no. of records : " + totalUserrows + " - " + "Pages : " + pg1;
                        lvPageUser.Visible = true;
                    }
                    else
                    {
                        lblpgCap1.Visible = false;
                        lvPageUser.Visible = false;
                    }

                    int pgno = 0;
                    lvPageUser.Items.Clear();
                    for (pgno = 1; pgno <= pg1; pgno++)
                    {
                        lvPageUser.Items.Add(pgno.ToString());
                    }
                    lvPageUser.SelectedValue = SelUserPageNo;
                }
            }
        }
    }

    protected void imgDelUserRole_Click(object sender, EventArgs e)
    {
        SelectedUserRoles();
        DataView dv = new DataView();
        if (Session["UserRoles"] != null)
            dv.Table = (DataTable)Session["UserRoles"];
        else if (ViewState["Useraction"] != null && ViewState["Useraction"].ToString() == "update")
            dv.Table = (DataTable)Session["SelUserRole"];

        if (dv.Table == null || dv.Table.Rows.Count == 1)
        {
            userErr.Value = "This row cannot be deleted.";
            return;
        }
        GridViewRow row = ((ImageButton)sender).Parent.Parent as GridViewRow;
        int rowIndx = row.RowIndex;
        if (dv.Table.Rows.Count > 0)
        {
            dv.Delete(rowIndx);
        }
        else
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("STARTDATE");
            dt.Columns.Add("ENDDATE");
            dt.Columns.Add("USERGROUP");
            ds.Tables.Add(dt);
            dv = ds.Tables[0].DefaultView;
        }
        grdUsRoles.DataSource = dv;
        grdUsRoles.DataBind();
    }

    protected void btnUsSeUsers_Click(object sender, EventArgs e)
    {
        string act = ViewState["act"].ToString();
        lvPageUser.SelectedValue = "1";
        BindUsers(act, txtUsSeUser.Text);
    }

    protected void rdbtnListActUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        string act = "";
        lvPageUser.SelectedIndex = 0;
        if (rdbtnListActUsers.SelectedIndex == 0)
            act = "";
        else if (rdbtnListActUsers.SelectedIndex == 1)
            act = "T";
        else
            act = "F";
        ViewState["act"] = act;
        txtUsSeUser.Text = "";
        lvPageUser.SelectedValue = "1";
        BindUsers(act, "");

    }

    protected void lvPageUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        string act = ViewState["act"].ToString();
        BindUsers(act, txtUsSeUser.Text);
    }

    protected void grdUsRoles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtStrDt = (TextBox)e.Row.FindControl("txtUsStrDt");
            txtStrDt.Attributes.Add("onblur", "javascript:ValidateDate(" + e.Row.RowIndex + ",2);");
            TextBox txtEndDt = (TextBox)e.Row.FindControl("txtUsEndDt");
            txtEndDt.Attributes.Add("onblur", "javascript:ValidateDate(" + e.Row.RowIndex + ",3);");

            clientCulture = Convert.ToString(Session["ClientLocale"]);
            if (!(string.IsNullOrEmpty(clientCulture)))
            {
                if (txtStrDt.Text == "")
                    txtStrDt.Text = clientCulture.ToLower() == "en-gb" ? "dd/mm/yyyy" : "mm/dd/yyyy";
                else if (ViewState["Useraction"].ToString() == "update")
                    txtStrDt.Text = util.GetClientDateString(clientCulture, txtStrDt.Text);
                if (txtEndDt.Text == "")
                    txtEndDt.Text = clientCulture.ToLower() == "en-gb" ? "dd/mm/yyyy" : "mm/dd/yyyy";
                else if (ViewState["Useraction"].ToString() == "update")
                    txtEndDt.Text = util.GetClientDateString(clientCulture, txtEndDt.Text);
            }
        }
    }

    protected void grdUsRoles_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlUsRole");
            //    //Fill the role dropdownlist
            DataTable dTable;
            SetGlobalVariables();
            dTable = GetRolesForUsers();
            ddl.Items.Add("");
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                ddl.Items.Add(dTable.Rows[i][1].ToString());
            }

            DataTable dTable2 = (DataTable)Session["UserRoles"];
            string IsNewUser = string.Empty;
            if (ViewState["NewUser"] != null)
            {
                IsNewUser = ViewState["NewUser"].ToString();
            }

            if (Session["SelUserRole"] != null && dTable2.Rows.Count > e.Row.RowIndex && IsNewUser != "true")
            {
                ddl.SelectedValue = dTable2.Rows[e.Row.RowIndex]["USERGROUP"].ToString();
            }
            else if (Session["UserRoles"] != null)
            {
                DataTable dv1 = (DataTable)Session["UserRoles"];
                if (dv1.Rows.Count > 0)
                    ddl.SelectedValue = dv1.Rows[e.Row.RowIndex]["USERGROUP"].ToString();
            }
            else
            {
                ddl.SelectedValue = "";
            }
        }
    }

    protected void ddlUs_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedUserRoles();
    }

    public void SelectedUserRoles()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("STARTDATE");
        dt.Columns.Add("ENDDATE");
        dt.Columns.Add("USERGROUP");
        for (int i = 0; i < grdUsRoles.Rows.Count; i++)
        {
            TextBox lblStrDt = (TextBox)grdUsRoles.Rows[i].FindControl("txtUsStrDt");
            TextBox lblEndDt = (TextBox)grdUsRoles.Rows[i].FindControl("txtUsEndDt");
            DropDownList lblRole = (DropDownList)grdUsRoles.Rows[i].FindControl("ddlUsRole");
            DataRow dr1 = dt.NewRow();
            dr1["STARTDATE"] = lblStrDt.Text;
            dr1["ENDDATE"] = lblEndDt.Text;
            dr1["USERGROUP"] = lblRole.SelectedValue;
            dt.Rows.Add(dr1);
        }
        ds.Tables.Add(dt);
        Session["UserRoles"] = ds.Tables[0];
        Session["SelUserRole"] = ds.Tables[0];
        grdUsRoles.DataSource = ds;
        grdUsRoles.DataBind();
    }

    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    #region # General Functions #
    #region # Paging for Gridviews #



    #endregion

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
                lnk.Visible = true;
            else
                lnk.Visible = false;
        }
    }

    private void BindBlankGrid(string Name, string SourceCall)
    {
        if (Name == "Users")
        {
            grdUsRoles.DataSource = null;
            grdUsRoles.DataBind();
            if (SourceCall == "")
            {
                grdUsLstUsers.DataSource = null;
                grdUsLstUsers.DataBind();
                recordsUser.Text = string.Empty;
                lblpgCap1.Text = string.Empty;
                lblNoRecordMsg.Visible = true;
                lblNoRecordMsg.Text = lblNodata.Text;
            }
            txtUsAdName.Text = string.Empty;
            txtUsAdEmail.Text = string.Empty;
            chkUsAdAct.Checked = false;
            txtUsAdsw.Text = string.Empty;
            txtUsAdConsw.Text = string.Empty;
            pwdExpiryDays.Text = string.Empty;
            txtUsAdsw.Attributes["value"] = string.Empty;
            txtUsAdConsw.Attributes["value"] = string.Empty;
            lblpgCap1.Visible = false;
            lvPageUser.Visible = false;


        }
    }

    #endregion
    #endregion
}
