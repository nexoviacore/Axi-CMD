using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.UI;

/// <summary>
/// Summary description for IView
/// </summary>
public class IView
{
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    Util.Util util = new Util.Util();
    LogFile.Log logobj = new LogFile.Log();
    /// <summary>
    /// Public variables
    /// </summary>
    string proj = HttpContext.Current.Session["Project"].ToString();
    public string IvName = string.Empty;
    public int pageSize;
    public int pageNo;
    public bool IsDbPagination = false;
    public ArrayList HyperLinkColumns = new ArrayList();
    public string Caption = string.Empty;
    public string DateCulture = string.Empty;
    public int TotalPages;
    public enum HyperlinkMode { url, embeddedhtml };
    public ArrayList HiddenColumnNames = new ArrayList();
    public List<string> comboFillItems = new List<string>();
    public enum SortOrder { ASC, DESC };

    /// <summary>
    /// Private variables
    /// </summary>
    DataTable dtToolbarBtns = new DataTable();
    string Project = string.Empty;
    string Axapps = string.Empty;
    int CurrentPageNo;
    int totalRows;
    DataTable paramsDt = new DataTable();
    Dictionary<string, string> paramsDict = new Dictionary<string, string>();
    bool IsFirstTime = false;
    DataTable iviewDataTable = new DataTable();
    StringBuilder toolbarBtnsHtml = new StringBuilder();
    string AxRole = string.Empty;
    string sessionId = string.Empty;
    string user = string.Empty;
    bool validateParamOnGo = false;
    string strGlobalVar = string.Empty;
    bool trace = false;
    string errLog = string.Empty;
    string fileName = string.Empty;
    bool isCallWS = false;
    string title = string.Empty;
    string subTitle = string.Empty;
    string footer = string.Empty;
    string paramXml = string.Empty;
    string customText = string.Empty;

    /// <summary>
    /// Iview Constructor
    /// </summary>
    /// <param name="IViewName"></param>
    public IView(string IViewName)
    {
        if (HttpContext.Current.Application["ValidateIviewParamOnGo"] != null)
            validateParamOnGo = Convert.ToBoolean(HttpContext.Current.Application["ValidateIviewParamOnGo"].ToString());
        else
            validateParamOnGo = false;

        AxRole = HttpContext.Current.Session["AxRole"].ToString();
        AxRole = util.CheckSpecialChars(AxRole);
        proj = HttpContext.Current.Session["project"].ToString();
        proj = util.CheckSpecialChars(proj);
        sessionId = HttpContext.Current.Session["nsessionid"].ToString();
        sessionId = util.CheckSpecialChars(sessionId);
        user = HttpContext.Current.Session["user"].ToString();
        user = util.CheckSpecialChars(user);
        if (HttpContext.Current.Session["ClientLocale"] != null)
            DateCulture = Convert.ToString(HttpContext.Current.Session["ClientLocale"]);

        IvName = IViewName;

        if (string.IsNullOrEmpty(DateCulture))
            DateCulture = "en-gb";

        strGlobalVar = util.GetGlobalVarString();

        if (HttpContext.Current.Session["AxDbPagination"] != null && HttpContext.Current.Session["AxDbPagination"].ToString() == "")
            IsDbPagination = true;
        else if (HttpContext.Current.Session["AxDbPagination"] != null && HttpContext.Current.Session["AxDbPagination"].ToString() != "")
            bool.TryParse(HttpContext.Current.Session["AxDbPagination"].ToString(), out IsDbPagination);
        if (HttpContext.Current.Session["strTrace"] != null)
        {
            string strTrace = HttpContext.Current.Session["strTrace"].ToString();
            if (strTrace.ToLower() == "true")
                trace = true;
        }
        IsFirstTime = true;
        CurrentPageNo = 1;
    }

    /// <summary>
    /// Iview constructor with project param
    /// </summary>
    /// <param name="IViewName"></param>
    /// <param name="projectName"></param>
    public IView(string IViewName, string projectName) : this(IViewName)
    {
        proj = projectName;
    }

    #region Public Functions
    /// <summary>
    /// This function will return param name and respective caption as a dictionary
    /// </summary>
    /// <returns>Dictionary with Param name and caption</returns>
    public Dictionary<string, string> GetParametersNameAndCaption()
    {
        string ires = GetParams();
        isCallWS = true;
        string errMsg = string.Empty;
        errMsg = util.ParseXmlErrorNode(ires);

        if (errMsg != string.Empty)
        {
            if (errMsg == Constants.SESSIONERROR)
            {
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();
            }
        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList productNodes = default(XmlNodeList);

            try
            {
                xmlDoc.LoadXml(ires);
            }
            catch (XmlException ex)
            {
            }
            paramsDict.Clear();
            productNodes = xmlDoc.SelectNodes("//root");
            if (productNodes.Count > 0)
            {
                for (int k = 0; k < productNodes[0].ChildNodes.Count; k++)
                {
                    string paramCaption = string.Empty;
                    string parameterName = string.Empty;
                    if (productNodes[0].ChildNodes[0].Attributes["cat"] != null && productNodes[0].ChildNodes[k].Attributes["cat"].Value == "params")
                    {
                        foreach (XmlNode xn in productNodes[0].ChildNodes[k].ChildNodes)
                        {
                            if (xn.Name == "a2")
                                paramCaption = xn.InnerText;
                            if (xn.Name == "a0")
                                parameterName = xn.InnerText;
                            if (!string.IsNullOrEmpty(parameterName) && !string.IsNullOrEmpty(paramCaption) && !paramsDict.Keys.Contains(parameterName))
                            {
                                paramsDict.Add(parameterName, paramCaption);
                                break;
                            }
                        }
                    }
                }
            }
        }


        return paramsDict;
    }

    /// <summary>
    /// This function will return details of Parameter
    /// </summary>
    /// <returns>datatable</returns>
    public DataTable GetParametersDetail()
    {
        string ires = GetParams();
        string errMsg = string.Empty;
        errMsg = util.ParseXmlErrorNode(ires);

        if (errMsg != string.Empty)
        {
            if (errMsg == Constants.SESSIONERROR)
            {
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();
            }
        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList productNodes = default(XmlNodeList);

            try
            {
                xmlDoc.LoadXml(ires);
            }
            catch (XmlException ex)
            {
                return paramsDt;
            }
            paramsDt = new DataTable();
            paramsDt.Columns.Add("ParamName");
            paramsDt.Columns.Add("ParamCaption");
            paramsDt.Columns.Add("DataType");
            paramsDt.Columns.Add("Expression");
            paramsDt.Columns.Add("ValidateExpression");
            paramsDt.Columns.Add("IsHidden");
            paramsDt.Columns.Add("paramMOE");
            paramsDt.Columns.Add("ParamSql");
            paramsDt.Columns.Add("ComboFill");
            paramsDt.Columns.Add("paramDepStr");
            productNodes = xmlDoc.SelectNodes("//root");

            for (int k = 0; k < productNodes[0].ChildNodes.Count; k++)
            {
                if (productNodes[0].ChildNodes[0].Attributes["cat"] != null && productNodes[0].ChildNodes[k].Attributes["cat"].Value == "params")
                {
                    paramsDt.Rows.Add();
                    foreach (XmlNode tstNode in productNodes[0].ChildNodes[k].ChildNodes)
                    {
                        if (tstNode.Name == "a2")
                            paramsDt.Rows[k]["ParamCaption"] = tstNode.InnerText;
                        else if (tstNode.Name == "a0")
                            paramsDt.Rows[k]["ParamName"] = tstNode.InnerText;
                        else if (tstNode.Name == "a4")
                            paramsDt.Rows[k]["DataType"] = tstNode.InnerText;
                        else if (tstNode.Name == "a6")
                            paramsDt.Rows[k]["Expression"] = tstNode.InnerText;
                        else if (tstNode.Name == "a10")
                            paramsDt.Rows[k]["ValidateExpression"] = tstNode.InnerText;
                        else if (tstNode.Name == "a21")
                            paramsDt.Rows[k]["IsHidden"] = tstNode.InnerText;
                        else if (tstNode.Name == "a13")
                            paramsDt.Rows[k]["paramMOE"] = tstNode.InnerText;
                        //else if (tstNode.Name == "a56")
                        //pvalue = tstNode.InnerText;
                        else if (tstNode.Name == "a11")
                            paramsDt.Rows[k]["ParamSql"] = tstNode.InnerText;
                        else if (tstNode.Name == "a15")
                        {
                            string paramDepStr = string.Empty;
                            foreach (XmlNode selNode in tstNode)
                            {
                                if (selNode.Name == "s")
                                {
                                    paramDepStr += selNode.InnerText.ToString() + ',';
                                }
                            }
                            paramsDt.Rows[k]["paramDepStr"] = paramDepStr.TrimEnd(',');
                        }
                        else if (tstNode.Name == "response")
                        {
                            string arrFillList = string.Empty;
                            foreach (XmlNode rowdNode in tstNode.ChildNodes)
                            {
                                if (rowdNode.ChildNodes[0] != null)
                                {
                                    if (rowdNode.ChildNodes[0].InnerText != "*")
                                    {
                                        string ddlValue = util.CheckSpecialChars(rowdNode.ChildNodes[0].InnerText);
                                        ddlValue = Regex.Replace(ddlValue, "&apos;", "&#39;");
                                        comboFillItems.Add(ddlValue);
                                        arrFillList += ddlValue + "¿";
                                    }
                                }

                            }
                            paramsDt.Rows[k]["ComboFill"] = arrFillList.TrimEnd('¿');
                        }
                    }
                }
            }
        }
        return paramsDt;
    }

    /// <summary>
    /// To return IView data as datatable
    /// </summary>
    /// <param name="iName">IView name</param>
    /// <param name="pageNo">To load page number</param>
    /// <param name="recsPerPage">Records per page to be displayed</param>
    /// <returns></returns>
    public DataSet GetData(int pageNo, int recsPerPage)
    {
        string iXml = string.Empty;
        string errlog = string.Empty;
        string result = string.Empty;

        title = string.Empty;
        subTitle = string.Empty;
        customText = string.Empty;
        footer = string.Empty;
        DataSet dst = new DataSet();
        fileName = "GetIViewFromObject-" + IvName;

        if (trace)
        {
            errlog = logobj.CreateLog("Call to GetIView Web Service for page no " + pageNo, sessionId, fileName, "");
            logobj.CreateLog("Start Time " + DateTime.Now.ToString(), sessionId, fileName, "");
        }
        if (sessionId == "")
        {
            sessionId = HttpContext.Current.Session.SessionID;
        }
        if (IsDbPagination == true && pageNo.ToString() != "1" && HttpContext.Current.Session["iv_noofpages"] != null)
        {
            int.TryParse(HttpContext.Current.Session["iv_noofpages"].ToString(), out totalRows);
            iXml = "<root name ='" + IvName + "' axpapp = '" + proj + "' sessionid = '" + sessionId + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errlog + "' pageno='" + pageNo + "' pagesize='" + recsPerPage.ToString() + "' firsttime='" + "yes" + "' sqlpagination='" + IsDbPagination.ToString().ToLower() + "' totalrows='" + totalRows + "'><params>" + paramXml + "</params>";
        }
        else
        {
            iXml = "<root name ='" + IvName + "' axpapp = '" + proj + "' sessionid = '" + sessionId + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errlog + "' pageno='" + pageNo + "' pagesize='" + recsPerPage.ToString() + "' firsttime='" + "yes" + "' sqlpagination='" + IsDbPagination.ToString().ToLower() + "'><params>" + paramXml + "</params>";
        }

        iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root> ";

        //Call service
        IviewData objIview = new IviewData();
        objIview.WebServiceTimeout = objWebServiceExt.asbMenu.Timeout;
        result = objWebServiceExt.CallGetIViewWS(IvName, iXml, "", objIview);

        string errMsg = util.ParseXmlErrorNode(result);

        if (errMsg != string.Empty)
        {
            return dst;
        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList XmlNodeList = default(XmlNodeList);

            try
            {
                xmlDoc.LoadXml(result);
            }
            catch (Exception ex)
            {
                if (trace)
                {
                    logobj.CreateLog("Exception in GetIview Service :--- " + ex.Message.ToString(), sessionId, fileName, "");
                }
                return null;
            }
            //GetSubCaptions(xmlDoc);
            //CreateHeaderRow(xmlDoc, pageNo.ToString());
            XmlNodeList = xmlDoc.SelectNodes("//headrow");
            int recCount = Convert.ToInt32(XmlNodeList[0].Attributes["reccount"].Value);
            //remove all other nodes other than data and call binddata
            XmlNode rnode = xmlDoc.SelectSingleNode("//headrow/pivotghead");
            if (rnode != null)
                rnode.ParentNode.RemoveChild(rnode);


            DataTable dt = new DataTable();

            dt.Columns.Add("title");
            dt.Columns.Add("subtitle");
            dt.Columns.Add("text");
            //GetCustomString();
            DataRow dr = dt.NewRow();
            XmlNodeList parentNodes = default(XmlNodeList);
            parentNodes = XmlNodeList[0].ChildNodes;
            for (int count = 0; count <= parentNodes.Count - 1; count++)
            {
                foreach (XmlNode chldNode in parentNodes)
                {
                    if (chldNode.Name == title)
                    {
                        dr["title"] = title;
                    }
                    else if (chldNode.Name == subTitle)
                    {
                        dr["subtitle"] = subTitle;
                    }
                    else if (chldNode.Name == customText)
                    {
                        dr["text"] = customText;
                    }

                }
            }
            dt.Rows.Add(dr);

            XmlNode headRow = xmlDoc.SelectSingleNode("//headrow");
            headRow.ParentNode.RemoveChild(headRow);

            XmlNode cNode = xmlDoc.SelectSingleNode("//comps");
            cNode.ParentNode.RemoveChild(cNode);
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);

            try
            {
                xmlDoc.WriteTo(xw);
            }
            catch (Exception ex)
            {
                if (trace)
                {
                    logobj.CreateLog("Exception in GetIview Service :--- " + ex.Message.ToString(), sessionId, fileName, "");
                }
                return null;
            }
            //Bind the dataset
            string nXml = string.Empty;
            nXml = sw.ToString();
            // assign XML Val to Properite
            //IvResult = nXml;
            StringReader sr = new StringReader(nXml);
            dst.ReadXml(sr);
            //dt.TableName = "Head";
            //dst.Tables.Add(dt);
        }
        return dst;
    }

    /// <summary>
    /// To call getdata with param
    /// </summary>
    /// <param name="pName"></param>
    /// <param name="pValue"></param>
    /// <param name="delemeter"></param>
    public void AssignParameter(ArrayList pName, ArrayList pValue, string delemeter)
    {
        if (pName != null && pValue != null)
        {
            for (int i = 0; i < pName.Count; i++)
            {
                if (pName[i].ToString().IndexOf(delemeter) == -1 && pValue[i].ToString().IndexOf(delemeter) == -1)
                {
                    paramXml = paramXml + "<" + pName[i].ToString() + ">";
                    paramXml = paramXml + pValue[i].ToString();
                    paramXml = paramXml + "</" + pName[i].ToString() + ">";
                }
                else
                {
                    string[] pNames = pName[i].ToString().Split(delemeter.ToCharArray());
                    string[] pValues = pValue[i].ToString().Split(delemeter.ToCharArray());
                    for (int k = 0; k < pNames.Count(); k++)
                    {
                        paramXml = paramXml + "<" + pNames[k].ToString() + ">";
                        paramXml = paramXml + pValues[k].ToString();
                        paramXml = paramXml + "</" + pNames[k].ToString() + ">";
                    }
                }
            }
        }
    }

    #endregion

    #region Private Functions

    /// <summary>
    /// This function will make a web service call to return parameters as xml
    /// </summary>
    /// <returns></returns>
    private string GetParams()
    {
        fileName = "openiview-" + IvName;
        errLog = logobj.CreateLog("Call to GetParams Web Service", sessionId, fileName, "");
        string iXml = string.Empty;
        iXml = "<root name =\"" + IvName + "\" axpapp = \"" + proj + "\" sessionid = \"" + sessionId + "\" appsessionkey=\"" + HttpContext.Current.Session["AppSessionKey"].ToString() + "\" username=\"" + HttpContext.Current.Session["username"].ToString() + "\" trace = \"" + errLog + "\"  >";
        iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
        string ires = string.Empty;
        isCallWS = true;
        IviewData objIview = new IviewData();
        objIview.WebServiceTimeout = objWebServiceExt.asbIview.Timeout;
        ires = objWebServiceExt.CallGetParamsWS(IvName, iXml, objIview);
        if (ires != null)
            ires = ires.Split('♠')[1];
        return ires;
    }

    #endregion
}
