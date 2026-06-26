using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class tsttable : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
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
        if (Session["project"] == null)
        {
            SessExpires();
            return;
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["fldId"] != null)
            {
                hdnfieldId.Value = Request.QueryString["fldId"].ToString();
            }
        }
    }
    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    [WebMethod]
    public static string GetSqlFillData(string tstDId, string fldName)
    {
        string strData = string.Empty;
        try
        {
            Util.Util utils = new Util.Util();
            string strSql = string.Empty;
            TStructData tstData = (TStructData)HttpContext.Current.Session[tstDId];
            if (tstData != null)
            {
                TStructDef strObj = tstData.tstStrObj;
                fldName = fldName.Substring(0, fldName.LastIndexOf("F") - 3);
                int dfIndx = strObj.GetFieldIndex(fldName);
                TStructDef.FieldStruct fld = (TStructDef.FieldStruct)strObj.flds[dfIndx];
                strSql = fld.tabletypsql;

                ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                strSql = utils.CheckSpecialChars(strSql);
                string inputXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + strSql + "</sql>";
                inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                string result = objWebServiceExt.CallGetChoiceWS("", inputXML);
                if (result != "")
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(result);
                    XmlNodeList globNode = xmlDoc.SelectNodes("//row");
                    strData = JsonConvert.SerializeObject(globNode);
                }
                else
                    strData = "";
            }
            else
                strData = "";
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Call to Getchoices Web Service input json " + ex.Message, HttpContext.Current.Session.SessionID, "GetSqlFillData", string.Empty);
        }
        return strData;
    }

    [WebMethod]
    public static string GetstrTableJson(string descrName, string tstDId, string fldName)
    {
        string strData = string.Empty;
        string strDataSql = string.Empty;
        try
        {
            Util.Util utils = new Util.Util();
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            string inputXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>select tjson from axp_tabledescriptor where dname='" + descrName + "'</sql>";
            inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            string result = objWebServiceExt.CallGetChoiceWS("", inputXML);
            if (result != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                XmlNodeList globNode = xmlDoc.SelectNodes("//row");
                //strData = JsonConvert.SerializeObject(globNode[0].InnerText);
                dynamic tblJson = JsonConvert.DeserializeObject(globNode[0].InnerText);
                if (tblJson.props.sql != null && tblJson.props.sql.Value != "")
                {
                    string tabletypsql = tblJson.props.sql.Value;
                    tblJson.props.sql = "sqlDescr";
                    strData = JsonConvert.SerializeObject(tblJson);
                    tabletypsql = utils.CheckSpecialChars(tabletypsql);
                    string inputXMLSql = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + tabletypsql + "</sql>";
                    inputXMLSql += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    string resultSql = objWebServiceExt.CallGetChoiceWS("", inputXMLSql);
                    if (resultSql != "")
                    {
                        XmlDocument xmlDocSql = new XmlDocument();
                        xmlDocSql.LoadXml(resultSql);
                        XmlNodeList globNodeSql = xmlDocSql.SelectNodes("//row");
                        strDataSql = JsonConvert.SerializeObject(globNodeSql);
                    }
                    else
                        strDataSql = "";
                }
                else
                {
                    strData = JsonConvert.SerializeObject(tblJson);
                }
            }
            else
                strData = "";
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Call to Getchoices Web Service input json " + ex.Message, HttpContext.Current.Session.SessionID, "GetstrTableJson", string.Empty);
        }
        return strData + "♥" + strDataSql;
    }

    [WebMethod]
    public static string GetDBProcTableJson(string procName, string tstDId, string fldName, ArrayList ChangedFields, ArrayList ChangedFieldDbRowNo, ArrayList ChangedFieldValues, ArrayList DeletedDCRows, string tfldDc, string tfldRowNo)
    {
        string strData = string.Empty;
        string strDataSql = string.Empty;
        try
        {
            Util.Util utils = new Util.Util();
            string prName = procName.Split('@')[1];
            if (prName.IndexOf(":") > 0)
            {
                XmlDocument xmlDocgbl = new XmlDocument();
                xmlDocgbl.LoadXml(HttpContext.Current.Session["axGlobalVars"].ToString());
                string blgAppVar = string.Empty;
                if (xmlDocgbl.SelectSingleNode("globalvars/appvartypes") != null)
                    blgAppVar = xmlDocgbl.SelectSingleNode("globalvars/appvartypes").InnerXml;
                foreach (XmlNode parms in xmlDocgbl.ChildNodes[0].ChildNodes)
                {
                    if (Regex.Match(prName, String.Format(@":\b{0}\b", parms.Name.ToString()), RegexOptions.IgnoreCase).Success)
                    {
                        prName = prName.Replace(":" + parms.Name, "'" + parms.InnerXml + "'");
                    }
                }
                if (prName.IndexOf(":") > 0)
                {
                    string fldXML = string.Empty;
                    TStructData tstData = (TStructData)HttpContext.Current.Session[tstDId];
                    if (tstData != null)
                    {
                        fldName = fldName.Substring(0, fldName.LastIndexOf("F") - 3);
                        tstData.GetFieldValueXml(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, "-1", "AutoComplete", "NG", tstData.tstStrObj.GetFieldDc(fldName).ToString());
                        fldXML = tstData.fieldValueXml;
                    }
                    if (fldXML != string.Empty)
                    {
                        TStructDef tstStrObj = tstData.tstStrObj;
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml("<fldXML>" + fldXML + "</fldXML>");
                        foreach (XmlNode parms in xmlDoc.ChildNodes[0].ChildNodes)
                        {
                            if (Regex.Match(prName, String.Format(@":\b{0}\b", parms.Name.ToString()), RegexOptions.IgnoreCase).Success)
                            {
                                int fldidxs = tstStrObj.GetFieldIndex(parms.Name);
                                TStructDef.FieldStruct flds = (TStructDef.FieldStruct)tstStrObj.flds[fldidxs];
                                bool isDcGrid = false;
                                isDcGrid = tstData.IsDcGrid(flds.fldframeno.ToString(), tstStrObj);
                                if (isDcGrid == true)
                                {
                                    if (tfldDc == flds.fldframeno.ToString() && int.Parse(parms.Attributes["rowno"].Value) == int.Parse(tfldRowNo))
                                        prName = prName.Replace(":" + parms.Name, "'" + parms.InnerXml + "'");
                                    else if (tfldDc != flds.fldframeno.ToString())
                                        prName = prName.Replace(":" + parms.Name, "'" + parms.InnerXml + "'");
                                }
                                else if (isDcGrid == false)
                                {
                                    prName = prName.Replace(":" + parms.Name, "'" + parms.InnerXml + "'");
                                }
                            }
                        }
                    }
                }
            }
            if (HttpContext.Current.Session["axdb"] != null && HttpContext.Current.Session["axdb"].ToString().ToLower() == "oracle")
                prName = "select " + prName + " FROM dual";
            else
                prName = "select " + prName;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            string result = asbExt.ExecuteSQL("", prName);
            if (result != "")
            {
                result = result.Replace("\\r\\n", "");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                XmlNodeList globNode = xmlDoc.SelectNodes("//row");
                dynamic tblJson = JsonConvert.DeserializeObject(globNode[0].InnerText);
                if (tblJson.props.sql != null && tblJson.props.sql.Value != "")
                {
                    string tabletypsql = tblJson.props.sql.Value;
                    tblJson.props.sql = "sqlDescr";
                    strData = JsonConvert.SerializeObject(tblJson);
                    tabletypsql = utils.CheckSpecialChars(tabletypsql);
                    string inputXMLSql = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + tabletypsql + "</sql>";
                    inputXMLSql += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    string resultSql = asbExt.CallGetChoiceWS("", inputXMLSql);
                    if (resultSql != "")
                    {
                        XmlDocument xmlDocSql = new XmlDocument();
                        xmlDocSql.LoadXml(resultSql);
                        XmlNodeList globNodeSql = xmlDocSql.SelectNodes("//row");
                        strDataSql = JsonConvert.SerializeObject(globNodeSql);
                    }
                    else
                        strDataSql = "";
                }
                else
                {
                    strData = JsonConvert.SerializeObject(tblJson);
                }
            }
            else
                strData = "";
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Call to Getchoices Web Service input json " + ex.Message, HttpContext.Current.Session.SessionID, "GetDBProcTableJson", string.Empty);
        }
        return strData + "♥" + strDataSql;
    }
}