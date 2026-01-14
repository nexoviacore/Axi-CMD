using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.Web.Services;
using CacheMgr;
using ClosedXML.Excel;
using System.Data.OleDb;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using Npgsql;

public partial class aspx_updateWizard : System.Web.UI.Page
{
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    ASB.WebService asbWebService = new ASB.WebService();
    public string strFillDepPName = string.Empty;
    public static String lsProj = "dwb";
    public ArrayList arrFillList = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        btnDelete.Click += BtnDelete_Click;
        if (IsPostBack) return;
        BindGrid();
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.Form["chk"] != null)
            {
                String[] lsstr = Request.Form["chk"].ToString().Split(Convert.ToChar(","));
                String result = "";
                if (lsstr.Length > 0)
                {
                    for (Int16 iCount = 0; iCount <= Convert.ToInt16(lsstr.Length - 1); iCount++)
                    {
                        string inputXML = string.Empty;
                        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
                        string sqlQuery = string.Empty;
                        String filename = "save-WizardSetting";
                        string errorLog = logobj.CreateLog("Call GetData", Session["nsessionid"].ToString(), "CallGetData-wizardSetting", "new", "true");
                        string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                        sqlQuery = "delete from mst_wizardsetting where mst_wizardsettingid='" + lsstr[iCount].ToString() + "'";
                        sqlQuery = util.CheckSpecialChars(sqlQuery);
                        query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                        inputXML = query;
                        logobj.CreateLog("Call to GetData Web Service" + inputXML, Session["nsessionid"].ToString(), filename, "");
                        result = asbExt.CallGetChoiceWS("", inputXML);
                        //Delete Wizard pages
                        inputXML = string.Empty;
                        sqlQuery = string.Empty;
                        filename = "save-WizardSetting";
                        errorLog = logobj.CreateLog("Call GetData", Session["nsessionid"].ToString(), "CallGetData-wizardSetting", "new", "true");
                        query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                        sqlQuery = "delete from mst_wizard_pages where mst_wizardsettingid='" + lsstr[iCount].ToString() + "'";
                        sqlQuery = util.CheckSpecialChars(sqlQuery);
                        query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                        inputXML = query;
                        logobj.CreateLog("Call to GetData Web Service" + inputXML, Session["nsessionid"].ToString(), filename, "");
                        result = asbExt.CallGetChoiceWS("", inputXML);
                    }
                }
                if (result.ToLower() == "done")
                { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Wizard deleted successfully.');", true); }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Wizard not Deleted!');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select any record!');", true);
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindGrid()
    {
        string inputXML = string.Empty;
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        String result = "";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        string sqlQuery = string.Empty;
        String filename = "save-WizardSetting";
        string errorLog = logobj.CreateLog("Call GetData", Session["nsessionid"].ToString(), "CallGetData-wizardSetting", "new", "true");
        string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
        sqlQuery = "select mst_wizardsettingid,wizard_id, wizardTitle from mst_wizardsetting";
        sqlQuery = util.CheckSpecialChars(sqlQuery);
        query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        inputXML = query;
        logobj.CreateLog("Call to GetData Web Service" + inputXML, Session["nsessionid"].ToString(), filename, "");
        result = asbExt.CallGetChoiceWS("", inputXML);
        DataSet dsPages = new DataSet();
        System.IO.StringReader sr = new System.IO.StringReader(result);
        dsPages.ReadXml(sr);
        ds = dsPages;

        if (ds.Tables.Count > 1)
        {
            dt = ds.Tables["row"];
        }

        gdWizard.DataSource = dt;
        gdWizard.DataBind();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdWizard.PageIndex = e.NewPageIndex;
        this.BindGrid();
    }
}