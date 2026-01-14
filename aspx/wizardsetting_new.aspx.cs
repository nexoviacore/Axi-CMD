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
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using Newtonsoft.Json;

public partial class aspx_wizardsetting_new : System.Web.UI.Page
{
    public string DbObjDataXML = string.Empty;
    public string editorShow = "0";
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    ASB.WebService asbWebService = new ASB.WebService();
    public string strFillDepPName = string.Empty;
    public static String lsProj = "dwb";
    public ArrayList arrFillList = new ArrayList();
    public string direction = "ltr";
    public string langType = "en";
    DBquery DBQobj;
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
            ResetSessionTime();
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (IsPostBack) return;
            if (Request.QueryString["cId"] != null && Request.QueryString["cId"].ToString() != String.Empty)
            { fillRecord(); }
        }
        else
        {
            SessExpires();
        }
    }

    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static string getGlobVar()
    {
        Util.Util util = new Util.Util();
        String strGlobVar = "";
        String getGlobResult = "";
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;
        try
        {
            strGlobVar = util.GetGlobalVarString();
            String[] strGlobSplit = strGlobVar.Trim().Split(Convert.ToChar("="));
            if (strGlobSplit.Length > 0)
            {
                for (Int32 iCount = 0; iCount <= strGlobSplit.Length - 1; iCount++)
                {
                    String[] strGlobSplit1 = strGlobSplit[iCount].ToString().Split(Convert.ToChar("~"));
                    if (strGlobSplit1.Length > 0)
                    {
                       // for (Int32 jCount = 0; jCount <= strGlobSplit1.Length - 1; jCount++)
                        {
                            if (strGlobSplit1[0].ToString().Contains("Parameters[") == false)
                            {

                                if (strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "responsibilies" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "rolename" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "sesid" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "usergroup" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "project" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "groupno" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "userroles" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "pageaccess" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "transidlist" && strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", "").Trim().ToLower() != "appvartypes")
                                {
                                    row = new Dictionary<string, object>();
                                    row.Add("Name", strGlobSplit1[0].ToString().Replace(@"\", "").Replace("\"", ""));
                                    rows.Add(row);
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
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            return new JavaScriptSerializer().Serialize(new { response = "Success", Result = rows, Status = "1" });

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        { }
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            // ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }
    private void SessExpires()
    {
        string url = util.SESSEXPIRYPATH;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "SessExpiresMessage", "parent.parent.location.href='" + url + "'", true);
    }

    private void fillRecord()
    {
        //tbxeditPage.va
        try
        {
            //Get wizard Id
            String inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            String sqlQuery = string.Empty;
            String filename = "get-wizardSetting";
            LogFile.Log logobj = new LogFile.Log();
            String errorLog = logobj.CreateLog("Call GetData", HttpContext.Current.Session["nsessionid"].ToString(), "CallGetData-wizardSetting", "new", "true");
            String query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select mst_wizardsettingid,wizardtitle from mst_wizardsetting where mst_wizardsettingid='" + Request.QueryString["cId"].ToString() + "'";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            inputXML = query;
            logobj.CreateLog("Call to GetData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
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
                tbxeditPage.Value = dt.Rows[0][1].ToString();
                Session["pageT"] = dt.Rows[0][1].ToString();
            }
        }
        catch (Exception ex)
        {

        }
    }

    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static string returnTSNames()
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            Util.Util util = new Util.Util();
            LogFile.Log logobj = new LogFile.Log();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            String filename = "save-WizardSetting";
            string errorLog = logobj.CreateLog("Call GetData", HttpContext.Current.Session["nsessionid"].ToString(), "CallGetData-wizardSetting", "new", "true");
            string query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select a1.mst_WizardSettingid, a1.wizard_id,a1.wizardtitle, string_agg(b1.pagetitle, ',') as pagetitle,  string_agg(b1.selectiontype,',') as selectiontype, string_agg(b1.pagename,',') as pagename, string_agg(b1.pagecode,',') as pagecode, string_agg(b1.requiredparam,'~') as requiredparam from mst_WizardSetting a1 inner join mst_wizard_pages b1 on b1.mst_WizardSettingid = a1.mst_wizardsettingid where a1.wizardtitle = '" + HttpContext.Current.Session["pageT"].ToString() + "' group by a1.wizard_id,a1.wizardtitle, a1.mst_WizardSettingid order by a1.mst_WizardSettingid";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            inputXML = query;
            logobj.CreateLog("Call to GetData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            if (dt.Rows.Count > 0)
            {
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(dt.Rows.Count - 1); iCount++)
                {
                    row = new Dictionary<string, object>();
                    row.Add("Name", dt.Rows[iCount][3].ToString());
                    row.Add("Code", dt.Rows[iCount][6].ToString());
                    row.Add("CbType", dt.Rows[iCount][4].ToString());
                    row.Add("lsParam", dt.Rows[iCount][7].ToString());
                    rows.Add(row);
                }
            }
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            return new JavaScriptSerializer().Serialize(new { response = "Success", Result = rows, Status = "1" });
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static string callTstruct()
    {
        try
        {
            string inputXML = string.Empty;
            LogFile.Log logobj = new LogFile.Log();
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            Util.Util util = new Util.Util();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", HttpContext.Current.Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select caption,name as ntransid from tstructs";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables.Count > 1)
                {
                    dt = ds.Tables["row"];
                }
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(dt.Rows.Count - 1); iCount++)
                {
                    row = new Dictionary<string, object>();
                    row.Add("Caption", dt.Rows[iCount][0].ToString());
                    row.Add("Name", dt.Rows[iCount][1].ToString());
                    rows.Add(row);
                }
            }
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            return new JavaScriptSerializer().Serialize(new { response = "Success", Result = rows, Status = "1" });

            //return obDs1;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {

        }
    }
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static string callIviews()
    {
        try
        {
            string inputXML = string.Empty;
            LogFile.Log logobj = new LogFile.Log();
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            Util.Util util = new Util.Util();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", HttpContext.Current.Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select caption, name from iviews";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(dt.Rows.Count - 1); iCount++)
                {
                    row = new Dictionary<string, object>();
                    row.Add("Caption", dt.Rows[iCount][0].ToString());
                    row.Add("Name", dt.Rows[iCount][1].ToString());
                    rows.Add(row);
                }
            }
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            return new JavaScriptSerializer().Serialize(new { response = "Success", Result = rows, Status = "1" });

        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }
    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static string callPaymentGateWay()
    {
        try
        {
            string inputXML = string.Empty;
            LogFile.Log logobj = new LogFile.Log();
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            Util.Util util = new Util.Util();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", HttpContext.Current.Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select Paymentgateway_Name as caption, mst_paymentgatewayid from mst_paymentgateway";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(dt.Rows.Count - 1); iCount++)
                {
                    row = new Dictionary<string, object>();
                    row.Add("Caption", dt.Rows[iCount][0].ToString());
                    row.Add("Name", dt.Rows[iCount][1].ToString());
                    rows.Add(row);
                }
            }
            row = new Dictionary<string, object>();
            row.Add("Caption", "File Upload Page");
            row.Add("Name", "uploadfilepage.aspx");
            rows.Add(row);
            //row = new Dictionary<string, object>();
            //row.Add("Caption", "HTML Page");
            //row.Add("Name", "HTML Content");
            //rows.Add(row);

            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            return new JavaScriptSerializer().Serialize(new { response = "Success", Result = rows, Status = "1" });

        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }

    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static string callPageDtl()
    {
        try
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            row = new Dictionary<string, object>();
            row.Add("Caption", "Import Page");
            row.Add("Name", "importpage.aspx");
            rows.Add(row);
            row = new Dictionary<string, object>();
            row.Add("Caption", "Export Page");
            row.Add("Name", "exportpage.aspx");
            rows.Add(row);
            row = new Dictionary<string, object>();
            row.Add("Caption", "Widget page");
            row.Add("Name", "PageDesigner.aspx");
            rows.Add(row);
            row = new Dictionary<string, object>();
            row.Add("Caption", "HTML Page");
            row.Add("Name", "tstruct.aspx?transid=sect");
            rows.Add(row);

            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            return new JavaScriptSerializer().Serialize(new { response = "Success", Result = rows, Status = "1" });

        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }

    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static string getParamJson(String lsReqValue, String lsReqType)
    {
        try
        {
            String lsinfo = "";
            //lsinfo = "Action=Load";
            //lsinfo += ",Action=Open";
            if (lsReqType == "TS")
            {
                System.Text.StringBuilder obSb = new StringBuilder();
                string inputXML = string.Empty;
                LogFile.Log logobj = new LogFile.Log();
                ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
                Util.Util util = new Util.Util();
                String result = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                string sqlQuery = string.Empty;
                string errorLog = logobj.CreateLog("GetLoginActivity.", HttpContext.Current.Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
                string query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                sqlQuery = "select fname, datatype from axpflds where tstruct='" + lsReqValue + "'";
                sqlQuery = util.CheckSpecialChars(sqlQuery);
                query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



                //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
                //inputXML += "<sql>select caption,name as ntransid from tstructs</sql>";
                //inputXML += "</sqlresultset>";
                inputXML = query;
                result = asbExt.CallGetChoiceWS("", inputXML);
                DataSet dsPages = new DataSet();
                System.IO.StringReader sr = new System.IO.StringReader(result);
                dsPages.ReadXml(sr);
                ds = dsPages;
                String lsfields = "";
                if (ds.Tables.Count > 1)
                {
                    dt = ds.Tables["row"];
                }
                if (dt.Rows.Count > 0)
                {
                    for (Int16 iCount = 0; iCount <= Convert.ToInt16(dt.Rows.Count - 1); iCount++)
                    {
                        if (lsfields == String.Empty)
                            lsfields = lsReqValue + "." + dt.Rows[iCount][0].ToString();
                        else
                            lsfields += "," + lsReqValue + "." + dt.Rows[iCount][0].ToString();
                    }
                }
                lsinfo += "," + lsfields;
            }
            else if (lsReqType == "IV")
            {
                LogFile.Log logobj = new LogFile.Log();
                ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
                Util.Util util = new Util.Util();
                string ires = string.Empty;
                //if (cboType.SelectedItem.ToString() == "IVIEW")
                {
                    String iViewname = lsReqValue;
                    if (iViewname != "")
                    {
                        string fileName = "openiview-" + iViewname;
                        string errLog = logobj.CreateLog("Call to GetParams Web Service", HttpContext.Current.Session["nsessionid"].ToString(), fileName, "");
                        string iXml = string.Empty;
                        iXml = "<root name =\"" + iViewname + "\" axpapp = \"" + HttpContext.Current.Session["project"].ToString() + "\" sessionid = \"" + HttpContext.Current.Session["nsessionid"].ToString() + "\" appsessionkey=\"" + HttpContext.Current.Session["AppSessionKey"].ToString() + "\" username=\"" + HttpContext.Current.Session["username"].ToString() + "\" trace = \"" + errLog + "\"  >";
                        iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
                        IviewData objIview1 = new IviewData();
                        objIview1.WebServiceTimeout = asbExt.asbIview.Timeout;
                        ires = asbExt.CallGetParamsWS(iViewname, iXml, objIview1);
                        if (ires != string.Empty)
                        {
                            ires = ires.Split('♠')[1];
                        }
                        //Response.Write(ConstructParamsHtml1(ires));
                        //Response.End();
                    }
                }
                StringBuilder strJsArrays = new StringBuilder();
                StringBuilder strParamDetails = new StringBuilder();
                ArrayList iviewParamValues = new ArrayList();
                ArrayList iviewParams = new ArrayList();
                bool isSqlFld = false;
                string clientCulture = null;
                String lsParamVal = "";
                IviewData objIview = new IviewData();
                string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
                StringBuilder paramHtml = new StringBuilder();
                String result = ires;
                result = _xmlString + result;
                // logobj.CreateLog("Loading and setting parameters components", sid, fileName, string.Empty);
                string parameterName = string.Empty;
                string paramCaption = string.Empty;
                string paramType = string.Empty;
                string paramHidden = string.Empty;
                string paramMOE = string.Empty;
                string paramValue = string.Empty;
                string paramSql = string.Empty;
                string paramDepStr = string.Empty;
                Boolean unHideParams = false;
                string expr = string.Empty;
                string vExpr = string.Empty;
                int tabIndex = 0;

                XmlDocument xmlDoc = new XmlDocument();
                XmlNodeList productNodes = default(XmlNodeList);
                XmlNodeList baseDataNodes = default(XmlNodeList);
                //  StringBuilder strJsArrays = new StringBuilder();
                int iCnt = 0;
                int fldNo = 0;
                int dpCnt = 0;
                try
                {
                    xmlDoc.LoadXml(result);
                }
                catch (XmlException ex)
                {
                    // Response.Redirect(util.ERRPATH + ex.Message);
                }

                Regex reg = new Regex("[*'\",_&#^@]");
                productNodes = xmlDoc.SelectNodes("//root");
                bool showParam = false;
                if (productNodes.Count > 0)
                {
                    if (productNodes[0].Attributes["showparams"] != null && productNodes[0].Attributes["showparams"].Value != string.Empty)
                        showParam = Convert.ToBoolean(productNodes[0].Attributes["showparams"].Value);
                }

                Int16 tCount = 0;
                foreach (XmlNode productNode in productNodes)
                {
                    baseDataNodes = productNode.ChildNodes;
                    foreach (XmlNode baseDataNode in baseDataNodes)
                    {
                        tCount++;
                        if (baseDataNode.Attributes["cat"].Value == "params")
                        {
                            paramValue = string.Empty;
                            if (baseDataNode.Attributes["value"] != null)
                                paramValue = baseDataNode.Attributes["value"].Value;

                            iviewParamValues.Add(paramValue);

                            foreach (XmlNode tstNode in baseDataNode)
                            {
                                if (tstNode.Name == "a0")
                                {
                                    parameterName = tstNode.InnerText;
                                }
                                else if (tstNode.Name == "a2")
                                {
                                    paramCaption = tstNode.InnerText;
                                }
                                else if (tstNode.Name == "a4")
                                {
                                    paramType = tstNode.InnerText;
                                }
                            }
                        }

                        if (lsParamVal == String.Empty)
                            lsParamVal = lsReqValue + "." + parameterName;
                        else
                            lsParamVal += "," + lsReqValue + "." + parameterName;
                    }

                }


                lsinfo += "," + lsParamVal;

            }
            else if (lsReqType == "PG")
            { }
            return lsinfo;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static string setPageSave(String lsPagetitle, String lsDecsription, String lsPTitle, String lsSType, String lspCaption, String lsPName, String lsParameter)
    {
        try
        {
            String lsmsg = "";
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            LogFile.Log logobj = new LogFile.Log();
            Util.Util util = new Util.Util();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            String filename = "save-WizardSetting";
            String errorLog = "";
            string query = "";
            if (HttpContext.Current.Session["pageT"] != null && HttpContext.Current.Session["pageT"].ToString() != String.Empty)
            {
                //Get wizard Id
                inputXML = string.Empty;
                asbExt = new ASBExt.WebServiceExt();
                result = "";
                ds = new DataSet();
                dt = new DataTable();
                ds1 = new DataSet();
                dt1 = new DataTable();
                sqlQuery = string.Empty;
                filename = "get-wizardSetting";
                errorLog = logobj.CreateLog("Call GetData", HttpContext.Current.Session["nsessionid"].ToString(), "CallGetData-wizardSetting", "new", "true");
                query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                sqlQuery = "select mst_wizardsettingid,wizard_id from mst_wizardsetting where wizardtitle='" + HttpContext.Current.Session["pageT"].ToString() + "'";
                sqlQuery = util.CheckSpecialChars(sqlQuery);
                query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                inputXML = query;
                logobj.CreateLog("Call to GetData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
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
                    inputXML = string.Empty;
                    asbExt = new ASBExt.WebServiceExt();
                    result = "";
                    sqlQuery = string.Empty;
                    filename = "get-wizardSetting";
                    errorLog = logobj.CreateLog("Call GetData", HttpContext.Current.Session["nsessionid"].ToString(), "CallGetData-wizardSetting", "new", "true");
                    query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                    sqlQuery = "delete from mst_wizardsetting where mst_wizardsettingid='" + dt.Rows[0][0].ToString() + "'";
                    sqlQuery = util.CheckSpecialChars(sqlQuery);
                    query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    inputXML = query;
                    logobj.CreateLog("Call to GetData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
                    result = asbExt.CallGetChoiceWS("", inputXML);
                    if (result.ToLower() == "done")
                    {
                        inputXML = string.Empty;
                        asbExt = new ASBExt.WebServiceExt();
                        result = "";
                        sqlQuery = string.Empty;
                        filename = "get-wizardSetting";
                        errorLog = logobj.CreateLog("Call GetData", HttpContext.Current.Session["nsessionid"].ToString(), "CallGetData-wizardSetting", "new", "true");
                        query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                        sqlQuery = "delete from mst_wizard_pages where mst_wizardsettingid='" + dt.Rows[0][0].ToString() + "'";
                        sqlQuery = util.CheckSpecialChars(sqlQuery);
                        query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                        inputXML = query;
                        logobj.CreateLog("Call to GetData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
                        result = asbExt.CallGetChoiceWS("", inputXML);
                    }
                }
            }

            inputXML = string.Empty;
            asbExt = new ASBExt.WebServiceExt();
            result = "";
            ds = new DataSet();
            dt = new DataTable();
            ds1 = new DataSet();
            dt1 = new DataTable();
            sqlQuery = string.Empty;
            filename = "get-wizardSetting";
            errorLog = logobj.CreateLog("Call SaveDate", HttpContext.Current.Session["nsessionid"].ToString(), "CallSaveDate-WizardSetting", "new", "true");
            query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "INSERT INTO mst_wizardsetting(mst_wizardsettingid, cancel, sourceid, username, modifiedon, createdby, createdon, app_level, app_desc, wizard_id, wizardtitle, description, lsactive) VALUES(nextval('ax_wizard_publish_seq'), 'F', '0', '" + HttpContext.Current.Session["username"].ToString() + "', '" + DateTime.Now + "', '" + HttpContext.Current.Session["username"].ToString() + "', '" + DateTime.Now + "', '1', '1','w'||nextval('ax_wizard_id_seq'), '" + lsPagetitle + "', '" + lsDecsription + "', 'T')";
            //sqlQuery = "insert into axpages (name,caption, props, blobno,visible,type,ordno,levelno,updatedon, createdon, createdby,updatedby, pagetype) values ('" + tbxPageTitle.Text.Trim() + "','" + tbxPageTitle.Text + "', 'wizardpage.aspx?wizard_id=1', 1, 'T', 'p',61,0,'" + DateTime.Now + "','" + DateTime.Now + "','"+ HttpContext.Current.Session["username"].ToString() + "','" + HttpContext.Current.Session["username"].ToString() + "','web')";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            inputXML = query;
            logobj.CreateLog("Call to SaveData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
            result = asbExt.CallGetChoiceWS("", inputXML);
            if (result.ToLower() == "done")
            {
                //Get wizard Id
                inputXML = string.Empty;
                asbExt = new ASBExt.WebServiceExt();
                result = "";
                ds = new DataSet();
                dt = new DataTable();
                ds1 = new DataSet();
                dt1 = new DataTable();
                sqlQuery = string.Empty;
                filename = "get-wizardSetting";
                errorLog = logobj.CreateLog("Call GetData", HttpContext.Current.Session["nsessionid"].ToString(), "CallGetData-wizardSetting", "new", "true");
                query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                sqlQuery = "select mst_wizardsettingid,wizard_id from mst_wizardsetting where wizardtitle='" + lsPagetitle + "'";
                sqlQuery = util.CheckSpecialChars(sqlQuery);
                query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                inputXML = query;
                logobj.CreateLog("Call to GetData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
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
                    if (lsPTitle != String.Empty)
                    {
                        String[] strSess = lsPTitle.ToString().Split(Convert.ToChar(","));
                        String[] strSess1 = lsSType.ToString().Split(Convert.ToChar(","));
                        String[] strSess2 = lspCaption.ToString().Split(Convert.ToChar(","));
                        String[] strSess3 = lsPName.ToString().Split(Convert.ToChar(","));
                        String[] strSess4 = lsParameter.ToString().Split(Convert.ToChar("~"));
                        String lsHTML = "F";
                        String lsFReq = "T";
                        String lsTParty = "F";
                        String lsPActive = "F";
                        if (strSess.Length > 0)
                        {
                            for (Int16 iCount = 0; iCount <= Convert.ToInt16(strSess.Length - 1); iCount++)
                            {
                                //Insert Wizards Page 
                                inputXML = string.Empty;
                                asbExt = new ASBExt.WebServiceExt();
                                result = "";
                                //ds = new DataSet();
                                //dt = new DataTable();
                                //ds1 = new DataSet();
                                //dt1 = new DataTable();
                                sqlQuery = string.Empty;
                                filename = "save-WizardPage";
                                errorLog = logobj.CreateLog("Call SaveDate", HttpContext.Current.Session["nsessionid"].ToString(), "CallSaveData-WizardPage", "new", "true");
                                query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                                sqlQuery = "insert into mst_wizard_pages (mst_wizard_pagesid, mst_wizardsettingid, mst_wizard_pagesrow, pagetitle, selectiontype, pagename, pagecode, requiredparam, lshtml, lsframereq, lsthirdparty,lsPActive) values (nextval('ax_wizard_publish_seq'),'" + dt.Rows[0][0].ToString() + "','" + (iCount + 1).ToString() + "','" + strSess[iCount].ToString() + "', '" + strSess1[iCount].ToString() + "', '" + strSess2[iCount].ToString() + "', '" + strSess3[iCount].ToString() + "', '" + strSess4[iCount].ToString() + "','" + lsHTML + "','" + lsFReq + "','" + lsTParty + "','" + lsPActive + "')";
                                sqlQuery = util.CheckSpecialChars(sqlQuery);
                                query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                                inputXML = query;
                                logobj.CreateLog("Call to SaveData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
                                result = asbExt.CallGetChoiceWS("", inputXML);
                            }
                        }
                    }
                    //Insert Page menu 
                    inputXML = string.Empty;
                    asbExt = new ASBExt.WebServiceExt();
                    result = "";
                    //ds = new DataSet();
                    //dt = new DataTable();
                    //ds1 = new DataSet();
                    //dt1 = new DataTable();
                    sqlQuery = string.Empty;
                    filename = "save-AxPages";
                    errorLog = logobj.CreateLog("Call SaveDate", HttpContext.Current.Session["nsessionid"].ToString(), "CallSaveDate-AxPages", "new", "true");
                    query = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                    if (HttpContext.Current.Session["pageT"] != null && HttpContext.Current.Session["pageT"].ToString() != String.Empty)
                        sqlQuery = "update axpages set props='wizardpage.aspx?wizard_id=" + dt.Rows[0][1].ToString() + "',updatedon='" + DateTime.Now + "', updatedby ='" + HttpContext.Current.Session["username"].ToString() + "' where caption='" + HttpContext.Current.Session["pageT"].ToString() + "'";
                    else
                        sqlQuery = "insert into axpages (name,caption, props, blobno,visible,type,ordno,levelno,updatedon, createdon, createdby,updatedby, pagetype) values ('" + lsPagetitle.Trim().Replace(" ", "") + "','" + lsPagetitle + "', 'wizardpage.aspx?wizard_id=" + dt.Rows[0][1].ToString() + "', 1, 'T', 'p',61,0,'" + DateTime.Now + "','" + DateTime.Now + "','" + HttpContext.Current.Session["username"].ToString() + "','" + HttpContext.Current.Session["username"].ToString() + "','web')";
                    sqlQuery = util.CheckSpecialChars(sqlQuery);
                    query += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    inputXML = query;
                    logobj.CreateLog("Call to SaveData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
                    result = asbExt.CallGetChoiceWS("", inputXML);
                    if (result.ToLower() == "done")
                    { lsmsg = "Page published successfully."; }
                    else
                        lsmsg = "Page not published";
                }
            }
            return lsmsg;
        }
        catch (Exception ex)
        {
            return "";
        }
        finally { }
    }
}