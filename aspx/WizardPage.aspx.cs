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

public partial class aspx_WizardPage : System.Web.UI.Page
{
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    ASB.WebService asbWebService = new ASB.WebService();
    public string strFillDepPName = string.Empty;
    public static String lsProj = "dwb";
    public ArrayList arrFillList = new ArrayList();
    public string direction = "ltr";
    public string langType = "en";
    public string sid = string.Empty;
    public string proj = string.Empty;
    public String lsIds = "";
    public String lsSteps = "";
    public Int32 lssCount = 0;
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
        if (Session["project"] != null)
        {
            proj = Session["project"].ToString();
            sid = HttpContext.Current.Session.SessionID;
            ResetSessionTime();
            if (IsPostBack) return;
            fillWizard();
        }
        else
        {
            SessExpires();
        }
    }
    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }
    private void SessExpires()
    {
        string url = util.SESSEXPIRYPATH;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "SessExpiresMessage", "parent.parent.location.href='" + url + "'", true);
    }

    private void fillWizard()
    {
        String strGlobVar = "";
        try
        {
            if (Request.QueryString["wizard_id"] != null && Request.QueryString["wizard_id"].ToString() != String.Empty)
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
                sqlQuery = "select a1.mst_WizardSettingid, a1.wizard_id,a1.wizardtitle, b1.pagetitle,  b1.selectiontype, b1.pagename, b1.pagecode, b1.requiredparam from mst_WizardSetting a1 inner join mst_wizard_pages b1 on b1.mst_WizardSettingid = a1.mst_wizardsettingid where a1.Wizard_id = '" + Request.QueryString["wizard_id"].ToString() + "' order by b1.mst_wizard_pagesrow";
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
                if (dt.Rows.Count > 0)
                {
                    StringBuilder obSb = new StringBuilder();
                    StringBuilder obSb1 = new StringBuilder();

                    obSb.Append("<div class='wizard-progress'>");
                    lssCount = dt.Rows.Count;
                    for (Int16 iCount = 0; iCount <= Convert.ToInt16(dt.Rows.Count - 1); iCount++)
                    {
                        String lsLink = "";
                        String ifrm = "";
                        if (lsIds == String.Empty)
                            lsIds = "Wizardset" + (iCount + 1).ToString();
                        else lsIds = lsIds + ", Wizardset" + (iCount + 1).ToString();

                        if (lsSteps == String.Empty)
                            lsSteps = dt.Rows[iCount][3].ToString();
                        else lsSteps = lsSteps + ", " + dt.Rows[iCount][3].ToString();
                        if (iCount == 0)
                            obSb.Append("<div data-objtype='WizardDataSetting' data-id='" + iCount.ToString() + "' data-target='Wizardset" + (iCount + 1).ToString() + "' class='step classic active'><a title = '" + dt.Rows[iCount][3].ToString() + "' class='stepName' href='javascript:void(0)'>" + dt.Rows[iCount][3].ToString() + "</a><div title='" + dt.Rows[iCount][3].ToString() + "' class='node'></div></div>");
                        else obSb.Append("<div data-objtype='WizardDataSetting' data-id='" + iCount.ToString() + "' data-target='Wizardset" + (iCount + 1).ToString() + "' class='step classic'><a title = '" + dt.Rows[iCount][3].ToString() + "' class='stepName' href='javascript:void(0)'>" + dt.Rows[iCount][3].ToString() + "</a><div title='" + dt.Rows[iCount][3].ToString() + "' class='node'></div></div>");
                        if (iCount == 0)
                            obSb1.Append("<div class='wizardContainer animated fadeIn' id='Wizardset" + (iCount + 1).ToString() + "'>");
                        else obSb1.Append("<div class='wizardContainer animated fadeIn' id='Wizardset" + (iCount + 1).ToString() + "' style='display:none;'>");
                        String lsParamDtl = "";
                        String lsParamVal = "";
                        String lsParamNm = "";
                        if (dt.Rows[iCount][7].ToString() != String.Empty)
                        {
                            String[] lsParamName = dt.Rows[iCount][7].ToString().Split(Convert.ToChar("="));
                            strGlobVar = util.GetGlobalVarString();
                            String[] strGlobSplit = strGlobVar.Trim().Split(Convert.ToChar("="));
                            if (strGlobSplit.Length > 0)
                            {
                                for (Int32 gCount = 0; gCount <= strGlobSplit.Length - 1; gCount++)
                                {
                                    String[] strGlobSplit1 = strGlobSplit[gCount].ToString().Split(Convert.ToChar("~"));
                                    if (strGlobSplit1.Length > 0)
                                    {
                                        // for (Int32 jCount = 0; jCount <= strGlobSplit1.Length - 1; jCount++)
                                        {
                                            if (strGlobSplit1[0].ToString().Contains("Parameters[") == false)
                                            {

                                                if (strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "responsibilies" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "rolename" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "sesid" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "usergroup" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "project" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "groupno" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "userroles" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "pageaccess" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "transidlist" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "appvartypes")
                                                {
                                                    if (strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() == lsParamName[0].ToString().ToLower())
                                                    {
                                                        String[] lsVal = strGlobSplit1[1].ToString().Split(Convert.ToChar(";"));
                                                        lsParamNm = lsParamName[0].ToString().ToLower();
                                                        lsParamVal = lsVal[0].ToString().Replace(@"\", "").Replace("\"", "").Trim();
                                                    }
                                                    //row = new Dictionary<string, object>();
                                                    //row.Add("Name", strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", ""));
                                                    //rows.Add(row);
                                                }
                                                //if (getGlobResult == String.Empty)
                                                //{
                                                //    getGlobResult = strGlobSplit1[jCount].ToString();
                                                //}
                                                //else
                                                //{
                                                //    getGlobResult = getGlobResult+ strGlobSplit1[jCount].ToString();
                                                //}
                                            }
                                        }
                                    }

                                }
                            }
                        }


                        if (dt.Rows[iCount][4].ToString() == "TSTRUCT")
                        {
                            if (lsParamNm != String.Empty && lsParamVal != String.Empty)
                                lsLink = "tstruct.aspx?transid=" + dt.Rows[iCount][6].ToString() + "&action=open&" + lsParamNm + "=" + lsParamVal;
                            else
                                lsLink = "tstruct.aspx?transid=" + dt.Rows[iCount][6].ToString();
                        }
                        else if (dt.Rows[iCount][4].ToString() == "IVIEW")
                            lsLink = "iview.aspx?ivname=" + dt.Rows[iCount][6].ToString();
                        else if (dt.Rows[iCount][4].ToString() == "CUSTOM PAGE")
                        {
                            if (dt.Rows[iCount][6].ToString().Contains("/download"))
                            {
                                lsLink = dt.Rows[iCount][6].ToString();
                            }
                            else
                            {
                                if (dt.Rows[iCount][6].ToString() == "Import Page")
                                {
                                    lsLink = "ImportNew.aspx";
                                }
                                else if (dt.Rows[iCount][6].ToString() == "Export Page")
                                {
                                    lsLink = "ExportNew.aspx";
                                }
                            }
                        }
                        else if (dt.Rows[iCount][4].ToString() == "PAYMENT GATEWAY")
                            lsLink = "ccavenue/dataFrom.aspx";
                        String cnt = "0";
                        if (dt.Rows.Count != (iCount + 1))
                            cnt = (iCount + 1).ToString();
                        ifrm = "<iframe id='frm" + (iCount + 1).ToString() + "' src='" + lsLink + "' title='Wizards Dashboard' onload='wizardLoadFunctions(this,\"" + cnt + "\");' class='card col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened' style='padding: 0px;' frameborder='0' scrolling='no' allowtransparency='True' width='100%' height='400px'></iframe>";
                        obSb1.Append("<section class='form-group col-md-12'>" + ifrm + "</section></div>");
                    }
                    obSb.Append("</div>");
                    wizardHeader.InnerHtml = obSb.ToString();
                    wizardBodyContent.InnerHtml = obSb1.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

}