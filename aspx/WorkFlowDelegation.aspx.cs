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
    string role = string.Empty;
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
    string axApps = string.Empty;
    string axProps = string.Empty;
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
        axApps = Session["axApps"].ToString();
        axProps = Application["axProps"].ToString();

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
            Response.Write("parent.parent.location.href='" + url + "';");
            Response.Write("</script>");

        }
        else
        {
            //if (Request.UrlReferrer != null)
            //{
            //    if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("main.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("workflow.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("workflowdelegation.aspx")))
            //        Response.Redirect("../cusError/axcustomerror.aspx");
            //}
            if (!IsPostBack)
            {
                sid = Session["nsessionid"].ToString();
                mvwTabs.ActiveViewIndex = 0;
                DataSet ds = new DataSet();
                string wfload = "<root axpapp=" + '"' + Session["project"] + '"' + " sessionid=" + '"' + sid + '"' + " trace=" + '"' + Session["AxTrace"] + '"' + " appsessionkey= '" + Session["AppSessionKey"].ToString() + "' username= '" + Session["username"].ToString() + "'>";
                wfload = wfload + axProps + axApps + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</root>";
                string res = "";
                XmlDocument loadxmlDoc = new XmlDocument();

                res = objWebServiceExt.CallLoadWorkFlowPageWS("workflow", wfload);
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


                GetUnits();

                identification = identify.Split(',');
                wfname = name.Split('~');
                wfname1 = name1.Split('~');
                tstname = tstructname.Split('~');
                tstname1 = tstructname1.Split('~');
                rolename = roles.Split('~');
            }

            ddlUnits.Attributes.Add("onChange", "javascript:ClearGrdData()");
            ddlDelUnit.Attributes.Add("onChange", "javascript:ClearUserInfo()");
        }

    }

    private void GetUnits()
    {
        string iXml = string.Empty;

        iXml += "<sqlresultset axpapp='" + Session["project"] + "' sessionid=' " + sid + "' trace='" + Session["AxTrace"] + "' user='" + Session["user"] + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
        iXml += "<sql>SELECT distinct ORGANIZATION,ORG_CODE FROM V_USERORGANIZATION order by ORGANIZATION</sql>";
        iXml += axProps + axApps + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        string ires_OrgUnit = string.Empty;
        ires_OrgUnit = objWebServiceExt.CallGetChoiceWS("workflow", iXml);

        if (ires_OrgUnit != "<error>" || ires_OrgUnit.Contains("<error>") != true)
        {

            XmlDocument gcxmlDoc = new XmlDocument();
            XmlNodeList gcproductNodes;
            XmlNodeList gcbaseDataNodes;
            gcxmlDoc.LoadXml(ires_OrgUnit);

            gcproductNodes = gcxmlDoc.SelectNodes("/sqlresultset/response/row");

            string ounit = "";
            string orgunit = "";
            string[] orgunits1;
            string[] orgunits;

            foreach (XmlNode gcproductNode in gcproductNodes)
            {
                gcbaseDataNodes = gcproductNode.ChildNodes;
                foreach (XmlNode gcbaseDataNode in gcbaseDataNodes)
                {
                    if (gcbaseDataNode.InnerText != "*")
                    {
                        orgunit += "~" + gcbaseDataNode.InnerText;
                    }
                }
                //To differentiate between each row prefix # to the combination of Organization and org_Code.
                ounit += "#" + orgunit;
                orgunit = "";
            }


            orgunits1 = ounit.Split('#');
            int o_inx = 0;

            for (o_inx = 1; o_inx <= orgunits1.Length - 1; o_inx++)
            {
                orgunits = orgunits1[o_inx].ToString().Split('~');
                ddlUnits.Items.Add(orgunits[1].ToString());
                ddlUnits.Items[o_inx - 1].Value = orgunits[2].ToString();
                ddlDelUnit.Items.Add(orgunits[1].ToString());
                ddlDelUnit.Items[o_inx - 1].Value = orgunits[2].ToString();
            }
            if (((Session["orgunit"] != null)))
            {
                ddlUnits.SelectedIndex = ddlUnits.Items.IndexOf(ddlUnits.Items.FindByValue(Session["orgunit"].ToString()));
            }

        }
        else if (ires_OrgUnit.Contains("<error>") && ires_OrgUnit.Contains(Constants.SESSIONEXPMSG))
        {
            Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG);
            return;
        }
        else
        {
            Response.Redirect("./err.aspx");
        }


    }

    protected void Messagebox(int Msg)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "myrest", "<script language=JavaScript>showAlertDialog('info'," + Msg + ",'client');</script>");
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (txtSrcUser.Value != "")
        {
            lblErrMsg.Text = "";
            string iXml;

            iXml = "<root axpapp='" + Session["project"] + "' uname='" + txtSrcUser.Value + "' sessionid='" + Session["nsessionid"] + "' trace='" + Session["AxTrace"] + "' >";
            iXml += axProps + axApps + "</root>";
            string ires = string.Empty;
            //ires = objWebServiceExt.CallGetUserTasksWS("workflow", iXml);
            XmlDocument loadxmlDoc = new XmlDocument();
            XmlNodeList rootNode;
            XmlNodeList chldNodes;
            loadxmlDoc.LoadXml(ires);
            rootNode = loadxmlDoc.SelectNodes("//root");
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

                foreach (DataRow docRow in dsStore1.Tables[0].Rows)
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
                pnlGrd.Visible = true;
                Session["dtTasks"] = dt;
                grdTasks.DataSource = dt;
                grdTasks.DataBind();
                Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>document.getElementById('SrcUser').value = document.getElementById('txtSrcUser').value;</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "setDUser", "<script language=JavaScript> document.getElementById('hdnDestUser').value = document.getElementById('DestUser').value;</script>");
            }
            else
            {
                BindBlankGrid(grdTasks);
                Messagebox(4016);
            }
        }
        else
        {
            pnlGrd.Visible = false;
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

            lblMsg.PostBackUrl = "./loadtstruct.aspx?transid=" + lblSname.Text + "&recordid=" + lblRecid.Text + "";
        }
    }
    private void BindBlankGrid(GridView grd)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fromwhom");
        dt.Columns.Add("towhoms");
        dt.Columns.Add("msg");
        dt.Columns.Add("sname");
        dt.Columns.Add("rid");
        dt.Columns.Add("cap");
        dt.Columns.Add("delg");
        grdTasks.DataSource = dt;
        grdTasks.DataBind();
    }
    protected void btnSubmitTask_Click(object sender, EventArgs e)
    {
        bool isAssigned = false;

        if (hdnDestUser.Value != "" && txtSrcUser.Value != "")
        {
            if (hdnSrcEmpId.Value == hdnDesEmpId.Value)
            {
                Messagebox(4017);
            }
            else
            {
                string ixml;
                string errlog = logobj.CreateLog("SaveDelegatedTasks", Session["nsessionid"].ToString(), "SaveDelegatedTasks", "new");
                ixml = "<root axpapp='" + Session["project"] + "' sessionid='" + Session["nsessionid"] + "' trace='" + errlog + "' uname='" + txtSrcUser.Value + "' duname='" + hdnDestUser.Value + "'><tasks>";

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
                    ixml += axProps + axApps + "</root>";

                    string ires = string.Empty;
                    //ires = objWebServiceExt.CallSaveDelegatedTasksWS("workflow", ixml);

                    if (ires == "<error>" || ires.Contains("<error>") == true)
                    {
                        Response.Redirect("./err.aspx");
                    }
                    else
                    {
                        BindBlankGrid(grdTasks);
                        Messagebox(4018);
                        pnlGrd.Visible = false;
                        isAssigned = true;
                        ddlUnits.ClearSelection();
                        ddlDelUnit.ClearSelection();
                        hdnSelectedValues.Value = "";
                        txtSrcUser.Value = "";
                        hdnDestUser.Value = "";
                    }
                }
                else
                {
                    Messagebox(4019);
                }
            }
        }
        else
        {
            if (txtSrcUser.Value == "")
            {
                BindBlankGrid(grdTasks);
            }

        }

        if (isAssigned != true)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>document.getElementById('SrcUser').value = document.getElementById('txtSrcUser').value;</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "setDUser", "<script language=JavaScript> document.getElementById('DestUser').value = document.getElementById('hdnDestUser').value;</script>");
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
        Page.ClientScript.RegisterStartupScript(GetType(), "setUser", "<script language=JavaScript>document.getElementById('SrcUser').value = document.getElementById('txtSrcUser').value;</script>");
        Page.ClientScript.RegisterStartupScript(GetType(), "setDUser", "<script language=JavaScript> document.getElementById('DestUser').value = document.getElementById('hdnDestUser').value;</script>");
    }
}
