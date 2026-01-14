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
using OfficeOpenXml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saml;
using System.Net;
using System.Threading;

//using Bytescout.Spreadsheet;

public partial class aspx_ImportNew : System.Web.UI.Page
{
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    ASB.WebService asbWebService = new ASB.WebService();
    StringBuilder strMenuHtml1 = new StringBuilder();
    public string sid = string.Empty;
    public string proj = string.Empty;
    public string sheetcount = string.Empty;
    public string formQueue = string.Empty;
    public string formQueueVal = string.Empty;
    string part1StringheaderName = string.Empty;
    string part2StringheaderValues = string.Empty;
    List<DataTable> dataTables = new List<DataTable>();
    string Flname = string.Empty;
    public string formDcCount = string.Empty;
    ArrayList ddlColumnNames = new ArrayList();
    Dictionary<string, string> multiselect = new Dictionary<string, string>();
    StringBuilder headersWithTypes = new StringBuilder();
    ArrayList arrTstructs = new ArrayList();
    ArrayList headerList = new ArrayList();
    ArrayList arrMapFlds = new ArrayList();
    ArrayList arrDcData = new ArrayList();
    ArrayList arrMapDcNos = new ArrayList();
    ArrayList arrAllDcsData = new ArrayList();
    ArrayList arrMapIsDc = new ArrayList();
    ArrayList arrGridDcs = new ArrayList();
    string SelectVal = string.Empty;
    string transid = string.Empty;
    StringBuilder strAutoComArray = new StringBuilder();
    Dictionary<string, string> dictFlds = new Dictionary<string, string>();
    DataTable dt = new DataTable();
    bool isValidSession = false;
    public string direction = "ltr";
    public string langType = "en";
    public bool isCloudApp = false;
    public bool fileReadSuccess = false;
    ArrayList ignoredCols = new ArrayList();
    Dictionary<string, string> multiselectTemplate = new Dictionary<string, string>();
    DataTable axpConfigStr = new DataTable();
    bool allExist = false;
    List<string> uniqueNamesSheet = new List<string>();
    string uniqueNamesStringSheet = string.Empty;
    string uniqueNamesStringheaders = string.Empty;
    string sheetSequence = string.Empty;

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
            if (!util.CheckValidLogin())
            {
                SessExpires();
                return;
            }
            proj = Session["project"].ToString();
            sid = HttpContext.Current.Session.SessionID;
            ResetSessionTime();
            if (!IsPostBack)
            {
                CheckDesignAccess();
                ViewState["ExImpTStructs"] = GetAllTStructs();
                CreateExportImportPanel();
                GETimpnames();


                //if url contains transid value then select that tstruct in the tstruct dropdown & get all fields
                string transid = Request.QueryString["transid"];
                string axmsgid = Request.QueryString["axmsgid"];
                // hdnaxmsgid.Value = "1355440000012";//"1355880000012";
                if (axmsgid != null)//1286660000000
                {
                    hdnaxmsgid.Value = axmsgid;//axmsgid;//"1286660000000";// axmsgid;
                }

                if (transid != null)
                {
                    ddlImpTbl.SelectedValue = transid;
                    hdnTransid.Value = transid.ToString();
                    GetFields(transid);
                }
            }
            if (ConfigurationManager.AppSettings["isCloudApp"] != null)
                isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"].ToString()); ;
            Page.ClientScript.RegisterStartupScript(GetType(), "set global var in iview", "<script>var isCloudApp = '" + isCloudApp.ToString() + "';</script>");
        }
        else
        {
            SessExpires();
        }
    }

    //Get all Tstruct list & bind it to dropdownlist
    public void CreateExportImportPanel()
    {
        ddlImpTbl.Items.Add(new ListItem(Constants.EMPTYOPTION, "NA"));
        string strv = ViewState["ExImpTStructs"].ToString();
        XmlDocument xmlDoc1 = new XmlDocument();
        xmlDoc1.LoadXml(strv);

        XmlNodeList xmlChild = xmlDoc1.GetElementsByTagName("row");
        string tstructName = string.Empty, tstructCaption = string.Empty;
        if (xmlChild.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertNoTstruct", "showAlertDialog(\"warning\",eval(callParent('lcm[259]')));", true);
        }
        else
        {
            for (int i = 0; i < xmlChild.Count; i++)
            {
                tstructName = xmlChild[i].ChildNodes.Item(0).InnerText.Trim();
                tstructCaption = xmlChild[i].ChildNodes.Item(1).InnerText.Trim();
                formQueue += tstructCaption.ToString() + ",";

                formQueueVal += tstructName.ToString() + ",";

                ListItem lst = new ListItem();
                lst.Text = tstructCaption;
                lst.Value = tstructName;
                ddlImpTbl.Items.Add(lst);
            }
            formQueue = formQueue.Remove(formQueue.Length - 1);
            hdnformQueue.Value = formQueue;
            formQueueVal = formQueueVal.Remove(formQueueVal.Length - 1);
            hdnformQueueVal.Value = formQueueVal;
        }
    }


    protected void ddlImpTbl_SelectedIndexChanged(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadingStatus", "ShowDimmer(true);", true);
        string selText = ddlImpTbl.SelectedItem.Text;

        //to replace special characters from the file name(Tstruct caption)
        string filename = selText;
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            filename = filename.Replace(c, '_');

        //to replace space characters from the file name with underscore 
        foreach (char c in filename.ToCharArray())
            if (c == 32)
                filename = filename.Replace(c, '_');

        hdnTemplateName.Value = filename;

        if (selText != Constants.EMPTYOPTION)
        {
            if (selText != string.Empty)
            {
                transid = ddlImpTbl.SelectedValue;
                hdnTransid.Value = transid;
                GetFields(transid);
            }
        }
        else
        {
            rptSelectFields.DataSource = "";
            rptSelectFields.DataBind();
            rptTemplateFields.DataSource = "";
            rptTemplateFields.DataBind();
        }
        hdnColNames.Value = hdnColValues.Value = "";
        IsFileUploaded.Value = "";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadingStatus", "ShowDimmer(false);", true);
    }

    protected void gridImpData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int igCol = 0;
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    DropDownList ddlColumnName = new DropDownList();
                    ddlColumnName.ID = "ddlColumnName" + i;
                    var colName = hdnFieldnamesExcelnew.Value.Split(',');
                    for (int j = 0; j < colName.Count(); j++)
                    {
                        ddlColumnName.Items.Add(new ListItem(colName[j], colName[j]));
                    }
                    string sFileName = upFileName.Value;

                    if (sFileName != string.Empty)
                    {
                        if ((sFileName).IndexOf("xlsx") > -1)
                            headerList.Clear();
                        headerList.AddRange(colName);
                    }
                    if (ChkColNameInfile.Checked == false)
                    {
                        if (ddlColumnName.Items.FindByValue(headerList[i].ToString()) != null)
                            ddlColumnName.Items.FindByValue(headerList[i].ToString()).Selected = true;
                        else if (ddlColumnName.Items.FindByText(headerList[i].ToString()) != null)
                            ddlColumnName.Items.FindByText(headerList[i].ToString()).Selected = true;
                        else
                        {
                            ddlColumnName.Items.FindByText(ignoredCols[igCol++].ToString()).Selected = true;
                        }
                    }
                    else
                        ddlColumnName.SelectedIndex = i;
                    e.Row.Cells[i].Controls.Add(ddlColumnName);
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Import Data - Griddatabound", "", "true");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CreateTemplate", "showAlertDialog(\"warning\",eval(callParent('lcm[262]')));", true);
        }
    }

    //Template(.csv) creation based on the Tstruct fields
    protected void btnCreateTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string scriptsPath = string.Empty, sessionId = string.Empty, filename = string.Empty;

            if (Application["ScriptsPath"] != null)
                scriptsPath = Application["ScriptsPath"].ToString();

            if (Session["nsessionid"] != null)
                sessionId = Session["nsessionid"].ToString();

            filename = hdnTemplateName.Value;

            filePath = scriptsPath + @"Axpert\" + sessionId;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //filePath += @"\" + filename + ".csv";
            filePath += @"\" + filename;
            StringBuilder sb = new StringBuilder();

            //if same column name exists in the tstruct then append that column with id
            string[] captionHeader = hdnColNames.Value.Split(',').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
            string[] nameHeader = hdnColValues.Value.Split(',').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
            ArrayList cols = new ArrayList();
            ArrayList dupCol = new ArrayList();
            for (int i = 0; i < captionHeader.Length; i++)
            {
                for (int j = i; j < captionHeader.Length - 1; j++)
                {
                    if (captionHeader[i] == captionHeader[j + 1])
                    {
                        dupCol.Add(captionHeader[i]); //get all duplicate column names
                        break;
                    }
                }
            }

            for (int i = 0; i < nameHeader.Length; i++)
            {
                if (!dupCol.Contains(nameHeader[i])) // if column contains duplicate then append it with id
                    cols.Add(captionHeader[i]);
                else
                    cols.Add(captionHeader[i] + "(" + captionHeader[i] + ")");
            }

            if (filename.Contains(".xlsx"))
            {
                //using (var workbook = new XLWorkbook())
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var existingSheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Sheet1");

                    if (existingSheet != null)
                    {
                        package.Workbook.Worksheets.Delete(existingSheet);
                    }
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    for (int i = 0; i < cols.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = cols[i];
                        worksheet.Column(i + 1).Style.Numberformat.Format = "@";
                    }
                    try
                    {
                        package.Save();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                sb.AppendLine(string.Join(ddlSeparator.SelectedValue, cols.ToArray()));
                File.WriteAllText(filePath, sb.ToString());
            }
            string FilenameEncrpt = filename;
            if (filename.IndexOf("&") > -1)
            {
                FilenameEncrpt = filename.Replace('&', '♠');
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Generate Template File", "window.open(\"openfile.aspx?fpath=" + FilenameEncrpt + "&Imp=t\", \"_self\");", true);
        }
        catch (Exception ex)
        {
            logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Import Data - CreateTemplate", "", "true");
            ScriptManager.RegisterStartupScript(updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog(\"warning\",eval(callParent('lcm[261]')))", true);
        }
    }

    static bool ArraysEquals(String[] A, String[] B)
    {
        bool result = (A.Length == B.Length);
        if (result)
        {
            foreach (String X in A)
            {
                if (!B.Contains(X))
                {
                    return false;
                }
            }
        }
        return result;
    }

    public void btnColHeader_Click(object sender, EventArgs e)
    {
        string sid = Session["nsessionid"].ToString();
        string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        string sFileName = upFileName.Value;
        string sFileDir = ScriptsPath + "Axpert\\" + sid + "\\";
        if (sFileName != string.Empty)
        {
            if ((sFileName).IndexOf(".xlsx") > -1)/*|| (sFileName).IndexOf("xls") > -1)*/
                //  dt = exceldt(sFileDir + sFileName);
                GetExcelDataTables(sFileDir + sFileName);
            else
                dt = ReadCsvFile(sFileDir + sFileName);
            string excelColNames = "";
            if (dt.Rows.Count > 0 && (sFileName).IndexOf(".csv") > -1)
            {
                DataRow drr = dt.Rows[0];
                object[] objectArray = drr.ItemArray;
                // Convert the object array to a string array
                string[] stringArray = Array.ConvertAll(objectArray, x => x.ToString());
                excelColNames = "";
                // Output the string array
                foreach (string value in stringArray)
                {
                    excelColNames += value.ToString() + ",";
                }
                excelColNames = excelColNames.Remove(excelColNames.Length - 1);
            }
            int selectedFieldCount = hdnColNames.Value.Split(',').Length;
            //allowing user to go to next step only if selected no of columns is equal to total no of columns in uploaded csv file              
            if (dt.Columns.Count == selectedFieldCount)
            {
                if (dt.Rows.Count > 0)
                {
                    if (sFileName.IndexOf(".xlsx") > -1)
                    {
                        headerList.AddRange(dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray());
                        var row = dt.NewRow();
                        foreach (var s in headerList)
                        {
                            excelColNames += s.ToString() + ",";
                        }
                        excelColNames = excelColNames.Remove(excelColNames.Length - 1);
                    }
                    if (excelColNames.Length > 0)
                    {
                        if (excelColNames.Contains("F1"))
                        {
                            DataRow drr = dt.Rows[0];
                            if ((sFileName).IndexOf(".xlsx") != -1)
                            {
                                // Convert the first row to an object array
                                object[] objectArray = drr.ItemArray;
                                // Convert the object array to a string array
                                string[] stringArray = Array.ConvertAll(objectArray, x => x.ToString());
                                excelColNames = "";
                                // Output the string array
                                foreach (string value in stringArray)
                                {
                                    excelColNames += value.ToString() + ",";
                                }
                                excelColNames = excelColNames.Remove(excelColNames.Length - 1);
                                if (ChkColNameInfile.Checked)
                                    dt.Rows.Remove(drr);
                            }
                        }
                    }
                    string[] excelheader = excelColNames.ToString().Split(',');
                    string[] tstfld = new string[] { };
                    string[] colName = new string[] { };
                    colName = hdnColNames.Value.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    String tstructFldCaption = "";
                    for (int i = 0; i < colName.Length; i++)
                    {
                        tstructFldCaption += colName[i] + ",";
                    }
                    tstructFldCaption = tstructFldCaption.Remove(tstructFldCaption.Length - 1);
                    tstfld = tstructFldCaption.ToString().Split(',');
                    if (ArraysEquals(excelheader, tstfld))
                    {
                        if (ChkColNameInfile.Checked.ToString().ToLower() == "false")
                        {
                            ChkColNameInfile.Checked = true;
                            hdnCheckToggleHeader.Value = "true";
                            DataRow drr = dt.Rows[0];
                            dt.Rows.Remove(drr);
                            if (dt.Rows.Count > 0)
                                dt = dt.Rows.Cast<DataRow>().Take(5).CopyToDataTable();
                        }
                        else
                        {
                            hdnCheckToggleHeader.Value = "true";
                            DataRow drr = dt.Rows[0];
                            if ((sFileName).IndexOf(".xlsx") == -1)
                            {
                                dt.Rows.Remove(drr);
                            }
                            if (dt.Rows.Count > 0)
                                dt = dt.Rows.Cast<DataRow>().Take(5).CopyToDataTable();
                        }
                        IsFileUploaded.Value = "1";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowDimmerClose", "addChkbxsToGrdColumns();", true);
                        gridImpData.DataSource = dt;
                        gridImpData.DataBind();
                    }
                    else
                    {
                        if (ChkColNameInfile.Checked.ToString().ToLower() == "true")
                        {
                            string warningMsg = hdnUploadFileWarnings.Value == "NotEqualHeaders" ? "showAlertDialog('error', 4039);" : "showAlertDialog('error', eval(callParent('lcm[529]')));";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "uploadAlertSuccessMessage", warningMsg + "$('#btnPrev').click();", true);
                        }
                        else
                        {
                            IsFileUploaded.Value = "1";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowDimmerClose", "addChkbxsToGrdColumns();", true);
                            if (dt.Rows.Count > 0)
                                dt = dt.Rows.Cast<DataRow>().Take(5).CopyToDataTable();
                            gridImpData.DataSource = dt;
                            gridImpData.DataBind();
                        }

                    }

                }
            }
        }
    }

    public string GetHeadersAsString(DataTable dataTable)
    {
        string headersString = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName));
        if (headersString.Contains("F1"))
        {
            DataRow drr = dataTable.Rows[0];
            object[] objectArray = drr.ItemArray;
            // Convert the object array to a string array
            string[] stringArray = Array.ConvertAll(objectArray, x => x.ToString());
            headersString = "";
            // Output the string array
            foreach (string value in stringArray)
            {
                headersString += value.ToString() + ",";
            }
            headersString = headersString.Remove(headersString.Length - 1);
        }

        // Split the string by commas
        string[] partsheader = headersString.Split(',');

        // Lists to hold the two parts
        List<string> part1List = new List<string>();
        List<string> part2List = new List<string>();

        // Process each part
        foreach (string part in partsheader)
        {
            // Extract the text before and inside the parentheses
            int openParenIndex = part.IndexOf('(');
            int closeParenIndex = part.IndexOf(')');

            if (openParenIndex != -1 && closeParenIndex != -1)
            {
                string part1 = part.Substring(0, openParenIndex).Trim();
                string part2 = part.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1).Trim();

                part1List.Add(part1);
                part2List.Add(part2);
            }
        }

        // Join the lists into strings
        part1StringheaderName = string.Join(",", part1List);
        part2StringheaderValues = string.Join(",", part2List);

        return headersString;
    }
    public void GETimpnames()
    {

        LogFile.Log logobj = new LogFile.Log();
        Util.Util util = new Util.Util();
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();

        string result = string.Empty;
        string errorLog = logobj.CreateLog("Call SaveDate", HttpContext.Current.Session["nsessionid"].ToString(), "CallGETiMPDATA", "new", "true");
        string inputXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
        string sqlQuery = "select impname from aximpdata;";
        sqlQuery = util.CheckSpecialChars(sqlQuery);
        inputXML += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        logobj.CreateLog("Call to SaveData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), "save-IMPDATA", "");
        result = asbExt.CallGetChoiceWS("", inputXML);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(result);
        XmlNodeList impnameNodes = xmlDoc.GetElementsByTagName("impname");

        // Create a list to hold the values
        List<string> impnameValues = new List<string>();

        // Iterate over the nodes and add their values to the list
        foreach (XmlNode node in impnameNodes)
        {
            impnameValues.Add(node.InnerText);
        }

        // Convert the list to an array
        string[] impnameArray = impnameValues.ToArray();
        DropDownList2.Items.Clear(); // Clear existing items
        foreach (string impname in impnameArray)
        {
            DropDownList2.Items.Add(new ListItem(impname, impname));
        }

    }


    [WebMethod]
    public static string GETtabledata(string tempname)
    {

        LogFile.Log logobj = new LogFile.Log();
        Util.Util util = new Util.Util();
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        //string tempname = hdnaxmsgid.Value;
        string result = string.Empty;
        string errorLog = logobj.CreateLog("Call SaveDate", HttpContext.Current.Session["nsessionid"].ToString(), "CallGETiMPDATA", "new", "true");
        string inputXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
        string sqlQuery = "select requestpayload from axactivemessages where taskid ='" + tempname + "'"; //"select requestpayload from axactivemessages where taskid ='1286660000000'";//
        sqlQuery = util.CheckSpecialChars(sqlQuery);
        inputXML += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        logobj.CreateLog("Call to SaveData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), "save-IMPDATA", "");
        result = asbExt.CallGetChoiceWS("", inputXML);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(result);
        string databaseType = HttpContext.Current.Session["axdb"].ToString().ToLower();
        XmlNode requestPayloadNode;
        if (databaseType != "" && databaseType == "oracle")
            requestPayloadNode = xmlDoc.SelectSingleNode("//REQUESTPAYLOAD");
        else
            requestPayloadNode = xmlDoc.SelectSingleNode("//requestpayload");
        return requestPayloadNode != null ? requestPayloadNode.InnerText : string.Empty;
    }

    [WebMethod]
    public static string GETdata(string tempname)
    {

        LogFile.Log logobj = new LogFile.Log();
        Util.Util util = new Util.Util();
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();

        string result = string.Empty;
        string errorLog = logobj.CreateLog("Call SaveDate", HttpContext.Current.Session["nsessionid"].ToString(), "CallGETiMPDATA", "new", "true");
        string inputXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
        string sqlQuery = "select impdata from aximpdata where impname ='" + tempname + "';";
        sqlQuery = util.CheckSpecialChars(sqlQuery);
        inputXML += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        logobj.CreateLog("Call to SaveData Web Service" + inputXML, HttpContext.Current.Session["nsessionid"].ToString(), "save-IMPDATA", "");
        result = asbExt.CallGetChoiceWS("", inputXML);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(result);
        string impdata = "";
        impdata = xmlDoc.SelectSingleNode("//response/row/impdata").InnerText;
        JObject jsonObject = JObject.Parse(impdata);
        string impdatartn = jsonObject["rapidxlimport"]["formname"] + "~" + jsonObject["rapidxlimport"]["dcname"] + "~" + jsonObject["rapidxlimport"]["primaryfield"];//jsonObject["rapidxlimport"]["dcname"]
        return impdatartn;

    }
    //Once file has been uploaded using Generic Handler FileUploadHandler.ashx then it will come here to validate the file Rows & Columns       
    public void UploadButton_Click(object sender, EventArgs e)
    {
        string checkHeader = hdnCheckHeader.Value;
        chkForIgnoreErr.Checked = true;
        string sid = Session["nsessionid"].ToString();
        string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        string sFileName = upFileName.Value;
        string fname = sFileName.Split('-')[0];
        aspx_ImportNew Importnew = new aspx_ImportNew();
        Importnew.Flname = sFileName;
        string sFileDir = ScriptsPath + "Axpert\\" + sid + "\\";
        if ((sFileName).IndexOf("xlsx") > -1)/*|| (sFileName).IndexOf("xls") > -1)*/
        {
            ddlSeparator.Visible = false;
            //dt = exceldt(sFileDir + sFileName);
            List<DataTable> dataTables = GetExcelDataTables(sFileDir + sFileName);
            hdnSheetCount.Value = dataTables.Count().ToString();
            sheetcount = dataTables.Count().ToString();
            string resultSheet = ""; // Initialize an empty string

            // Loop through the count and build the string
            for (int i = 1; i <= dataTables.Count(); i++)
            {
                resultSheet += "sheet" + i; // Add the sheet name

                // Add a comma if it's not the last sheet
                if (i < dataTables.Count())
                {
                    resultSheet += ",";
                }
            }


            List<object[]> firstRows = new List<object[]>();
            foreach (DataTable table in dataTables)
            {
                //if (table.Rows.Count > 0) // Check if the table has rows
                // {
                //object[] firstRow = table.Rows[0].ItemArray;
                string[] headers = table.Columns.Cast<DataColumn>()
                                    .Select(column => column.ColumnName)
                                    .ToArray();
                firstRows.Add(headers);
                // }
            }

            if (dataTables.Count >= 1) // Check if there are at least two DataTables in the list
            {
                DataTable sheet = dataTables[0]; // Access the second DataTable (index 1)
                string headersString = GetHeadersAsString(sheet);
                string[] headersxl = headersString.Split(',');
                string[] formhdrs = hdnFieldnamesExcelnew.Value.Split(',');
                allExist = headersxl.Any(header => formhdrs.Contains(header));
                if (allExist)
                {
                    hdnxlHasHeader.Value = "true";
                }
                else
                {
                    hdnxlHasHeader.Value = "false";
                }
            }


            IsFileUploaded.Value = "1";
            if (dataTables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertErrorMessage", "ShowImportError('Empty file can not be import. Please check and try again.');", true);
                return;
            }
            DataRow drr = dataTables[0].Rows[0];
            //dataTables[0].Rows.Remove(drr);
            hdnRowCount.Value = drr.Table.Rows.Count.ToString();
            // gridImpData.DataBind();
            if (hdnmulForm.Value == "false")
            {
                gridImpData.DataSource = dataTables[0];
                gridImpData.DataBind();
            }
            hdnUploadFileWarnings.Value = "Success";
            string firstRowsJson = JsonConvert.SerializeObject(firstRows);
            string script = string.Format("fileUploadSuccess(); generateHTML('{0}', {1});countSheets('{2}', '{3}');", resultSheet, firstRowsJson, sheetcount, fname); //"fileUploadSuccess();generateHTML(" + resultSheet + " +',' + " + firstRowsJson + ");";
                                                                                                                                                                      //string script = $"fileUploadSuccess(); countSheets({sheetcount}, '{fname}');";

            ScriptManager.RegisterStartupScript(updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", script, true);
        }
    }

    static DataSet ReadExcelUsingOleDb(string filePath)
    {
        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0; HDR=NO;IMEX=1\"";

        using (OleDbConnection connection = new OleDbConnection(connectionString))
        {
            connection.Open();

            // Get the sheet names
            DataTable sheets = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            DataSet dataSet = new DataSet();

            foreach (DataRow row in sheets.Rows)
            {
                string sheetName = row["TABLE_NAME"].ToString();

                // Read data from each sheet
                string query = "SELECT * FROM [" + sheetName + "]";
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                {
                    DataTable table = new DataTable { TableName = sheetName };
                    adapter.Fill(table);
                    dataSet.Tables.Add(table);
                }
            }

            return dataSet;
        }
    }

    static void UpdateSheetNames(string filePath, string[] sheetNames)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        FileInfo fileInfo = new FileInfo(filePath);

        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            // Get all worksheets
            var worksheets = package.Workbook.Worksheets;

            // Ensure the array size matches the number of sheets
            if (sheetNames.Length != worksheets.Count)
                throw new InvalidOperationException("The number of sheet names provided does not match the number of sheets in the file.");

            for (int i = 0; i < worksheets.Count; i++)
            {
                worksheets[i].Name = sheetNames[i]; // Rename each sheet
            }

            // Save changes to the file
            package.Save();
        }
    }

    private int GetSheetKey(int sheetIndex)
    {
        return sheetIndex; // Key is equal to the sheet's index
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        string sid = HttpContext.Current.Session["nsessionid"].ToString();
        string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        string sFileName = upFileName.Value.ToString();
        string sFileDir = ScriptsPath + "Axpert\\" + sid + "\\";
        string sourceFilePath = sFileDir + sFileName;


        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // Array to hold sheet names
        string[] sheetNames;
        List<string> uniqueHeaders = new List<string>();
        //HashSet<string> uniqueHeaders = new HashSet<string>();
        using (var package = new ExcelPackage(new FileInfo(sourceFilePath)))
        {
            // Extract sheet names
            sheetNames = package.Workbook.Worksheets
                .Select(ws => ws.Name)
                .ToArray();



            foreach (var worksheet in package.Workbook.Worksheets)
            {
                if (worksheet.ToString().IndexOf("dc1") != -1)
                {
                    // Get the number of columns in the sheet
                    int columnCount = worksheet.Dimension.End.Column;

                    // Read the first row (header row) of the sheet
                    var headerCell = worksheet.Cells[1, 1].Text.Trim(); // Read the first cell (first column of the first row)

                    // Add the header to the HashSet (automatically handles duplicates)
                    uniqueHeaders.Add(headerCell);
                }
            }
        }
        sheetSequence = string.Join(", ", sheetNames);
        hdnsheetSequence.Value = sheetSequence;
        if (hdnmulForm.Value == "true")
        {
            // Extract unique names before the ~ character
            uniqueNamesSheet = sheetNames
                .Select(name => name.Split('~')[0]) // Split by '~' and take the first part
                .Distinct() // Ensure uniqueness
                .ToList();

            uniqueNamesStringSheet = string.Join("$$", uniqueNamesSheet);
            hdnuniqueNamesStringSheet.Value = uniqueNamesStringSheet;
            string[] headersArray = new string[uniqueHeaders.Count];
            uniqueHeaders.CopyTo(headersArray);
            uniqueNamesStringheaders = string.Join("$$", headersArray);
            hdnuniqueNamesStringheaders.Value = uniqueNamesStringheaders;
        }

        string filePath = "";
        string scriptsPath = string.Empty, sessionId = string.Empty, filename = string.Empty;

        if (Application["ScriptsPath"] != null)
            scriptsPath = Application["ScriptsPath"].ToString();

        if (Session["nsessionid"] != null)
            sessionId = Session["nsessionid"].ToString();

        filename = hdnTemplateName.Value;

        filePath = scriptsPath + @"Axpert\" + sessionId;
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        string strProj = HttpContext.Current.Session["project"].ToString();
        string jsoncontents = string.Empty;
        string networkPath = string.Empty;
        // Path to the network directory
        try
        {
            FDR fdrObj = new FDR();
            jsoncontents = fdrObj.StringFromRedis(Constants.AXFileServer_CONN_KEY, strProj);
            if (jsoncontents == string.Empty || jsoncontents == "nofileserverconnection")
            {
                //alert();
                ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertErrorMessage", "ShowImportError('File Upload Virtual Path is empty, Please verify and add the required path information through AxpertAdmin Settings.');", true);
                return;
            }
            else
            {
                JObject _jsonAxFile = JObject.Parse(jsoncontents);
                if (_jsonAxFile["FileUploadPath"] != null)
                    HttpContext.Current.Session["AxConfigFileUploadPath"] = _jsonAxFile["FileUploadPath"].ToString();
                if (_jsonAxFile["FileDownloadPath"] != null)
                    HttpContext.Current.Session["AxConfigFileDownloadPath"] = _jsonAxFile["FileDownloadPath"].ToString();
            }
            networkPath = HttpContext.Current.Session["AxConfigFileUploadPath"].ToString();
            if (!Directory.Exists(networkPath))
            {
                ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertErrorMessage", "ShowImportError('File Upload Virtual Path specified in AxpertAdmin Settings not exist. Please verify and correct it.');", true);
                return;
            }
        }
        catch (Exception ex) { }

        string transid = hdnsltForm.Value;
        string dcs = hdnsltDC.Value;
        string tempName = hdnsltTemp.Value;
        string formFull = hdnsltFormFull.Value;
        string pKey = hdnsltPkey.Value;
        string[] dcArray = dcs.Split(',');
        if (dcArray.Length > 0 && dcArray[0] == "selectall")
        {
            dcArray = dcArray.Skip(1).ToArray(); // Skip the first element and convert back to array
        }
        // Create a new array with the desired format
        string[] updatedSheetNames = new string[dcArray.Length];
        for (int i = 0; i < dcArray.Length; i++)
        {
            updatedSheetNames[i] = transid + "~" + dcArray[i];
        }

        string sheetHeadersJSON = Request.Form["hdndheetheaders"];
        JObject jsonObjects = JObject.Parse(sheetHeadersJSON);
        var keysToRemove = new List<string>();
        foreach (var property in jsonObjects.Properties())
        {
            if (property.Name == "NaN")
            {
                keysToRemove.Add(property.Name);
            }
        }

        // Remove the "NaN" keys
        foreach (var key in keysToRemove)
        {
            jsonObjects.Remove(key);
        }
        sheetHeadersJSON = jsonObjects.ToString();

        // Get the count of properties in the JSON object
        int propertyCount = jsonObjects.Count;
        int sheetNumber = int.Parse(hdnSheetnumber.Value);

        if (!string.IsNullOrEmpty(sheetHeadersJSON) && propertyCount == sheetNumber && hdnxlHasHeader.Value != "false")
        {
            // Deserialize the JSON string into a Dictionary
            var sheetHeaders = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, List<string>>>(sheetHeadersJSON);

            // Define the path to the Excel file (make sure this path is correct)
            //string filePath = Server.MapPath("~/path/to/excel/folder/yourfile.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Open the Excel file using EPPlus
            using (var package = new ExcelPackage(new FileInfo(sourceFilePath)))
            {
                // Loop through the sheets in the Excel file
                for (int sheetIndex = 0; sheetIndex <= package.Workbook.Worksheets.Count - 1; sheetIndex++)
                {
                    var sheet = package.Workbook.Worksheets[sheetIndex];
                    int sheetKey = GetSheetKey(sheetIndex + 1); // Get sheet key based on its index

                    if (sheetHeaders.ContainsKey(sheetKey))
                    {
                        // Get the new headers for this sheet
                        var newHeaders = sheetHeaders[sheetKey];
                        for (int i = 0; i < newHeaders.Count; i++)
                        {
                            sheet.Cells[1, i + 1].Value = newHeaders[i]; // Set header values in the first row
                        }
                    }
                }

                // Save the modified Excel file
                package.Save();
            }

            string destinationFilePath = Path.Combine(networkPath, Path.GetFileName(sourceFilePath));

            // Copy the file
            File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
        }
        else
        {
            DataSet dataSet = ReadExcelUsingOleDb(sourceFilePath);
            //if (!Directory.Exists(networkPath))
            //{
            //    //Console.WriteLine("Destination folder does not exist.");
            //    ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertErrorMessage", "ShowImportError('Upload file path folder does not exist, please check and try again.');", true);
            //    return;
            //}

            // Define destination file path
            string destinationFilePath = Path.Combine(networkPath, Path.GetFileName(sourceFilePath));

            // Copy the file
            File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
        }
        string rediscachedtls = util.GetRedisConnDetails(Session["project"].ToString());
        string redisIp = HttpContext.Current.Session["RedisCacheIP"].ToString();
        string redisPass = HttpContext.Current.Session["RedisCachePwd"].ToString();
        if (redisPass != "")
            redisPass = util.EncryptPWD(redisPass);
        string redisIPonly = redisIp.Split(':')[0];
        string redisPort = redisIp.Split(':')[1];
        string projectname = HttpContext.Current.Session["project"].ToString();
        string userName = HttpContext.Current.Session["username"].ToString();
        AxpertRestAPIToken axpertToken = new AxpertRestAPIToken(userName);
        //string mapCols ="";
        if (propertyCount == sheetNumber)
            hdnUpdateMapcolumns.Value = "true";

        ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertSuccessMessage", "GetImpData(\"" + redisIPonly + "\",\"" + redisPort + "\",\"" + redisPass + "\",\"" + projectname + "\",\"" + axpertToken.token + "\",\"" + axpertToken.seed + "\",\"" + axpertToken.userAuthKey + "\",\"" + HttpContext.Current.Session["AxTrace"].ToString() + "\");", true);
    }

    //Create a new file with selected columns if any columns has ignored
    public void CreateCsvFileForSelectedColumns(string filePath, string newFileName)
    {
        using (var reader = new StreamReader(filePath))
        {
            StringBuilder sb = new StringBuilder();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(Convert.ToChar(ddlSeparator.SelectedValue));
                var cols = hdnIgnoredColumns.Value.Split(',');
                ArrayList selectedColumns = new ArrayList();
                for (int i = 0; i < values.Count(); i++)
                {
                    string colIndex = (i + 1).ToString();
                    if (!cols.Contains(colIndex))
                    {
                        selectedColumns.Add(values[i]);
                    }
                }
                sb.AppendLine(string.Join(ddlSeparator.SelectedValue, selectedColumns.ToArray()));
            }
            File.WriteAllText(newFileName, sb.ToString());
        }
    }

    //Read the uploaded CSV file & returns result as a DataTable    
    public DataTable ReadCsvFile(string filePath)
    {
        DataTable dataTable = new DataTable();
        try
        {
            bool firstRow = true;
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line != string.Empty)
                        {
                            string[] values = line.Split(Convert.ToChar(ddlSeparator.SelectedValue));
                            if (firstRow)
                            {
                                if (values.Count() != 1 && !line.Contains(ddlSeparator.SelectedValue))
                                {
                                    fileReadSuccess = false;
                                    IsFileUploaded.Value = "";
                                    dataTable.Rows.Clear();
                                    dataTable.Columns.Clear();
                                    hdnUploadFileWarnings.Value = "InvalidFileFormat";
                                    return dataTable;
                                }
                                firstRow = false;
                                string[] captionHeader = hdnColNames.Value.Split(',');
                                string[] nameHeader = hdnColValues.Value.Split(',');

                                for (int i = 0; i < nameHeader.Length; i++)
                                {
                                    //if same column name exists in the tstruct then append that column with id
                                    nameHeader[i] = nameHeader[i].Trim().Replace("\n", "");
                                    captionHeader[i] = captionHeader[i].Trim().Replace("\n", "");
                                    if (!dataTable.Columns.Contains(nameHeader[i]))
                                        dataTable.Columns.Add(nameHeader[i]);
                                    else
                                        dataTable.Columns.Add(nameHeader[i] + "(" + captionHeader[i] + ")");
                                }
                                string[] captions = hdnColNames.Value.Replace(", ", ",").Split(',');

                                string[] mandatoryCols = hdnMandatoryFields.Value.Replace(", ", ",").Split('#');
                                ArrayList mandatoryFlds = new ArrayList();
                                ArrayList mandatoryCap = new ArrayList();
                                if (mandatoryCols.Count() > 0)
                                {
                                    var flds = mandatoryCols[0].Split(',');
                                    var caps = mandatoryCols[1].Split(',');
                                    for (int i = 0; i < caps.Length; i++)
                                    {
                                        mandatoryFlds.Add(flds[i]);
                                        mandatoryCap.Add(caps[i]);
                                    }
                                }

                                for (int i = 0; i < captions.Count(); i++)
                                {
                                    if (mandatoryCols.Count() > 0)
                                    {
                                        if (!values.Contains(captions[i]))
                                        {
                                            if (!mandatoryFlds.Contains(captions[i]) || !mandatoryCap.Contains(captions[i]))
                                                ignoredCols.Add(captions[i]);
                                        }
                                    }
                                    else if (!values.Contains(captions[i]))
                                        ignoredCols.Add(captions[i]);
                                }
                            }

                            DataRow dr = dataTable.NewRow();
                            int mandatoryColCount = int.Parse(hdnMandatoryColCount.Value);
                            if (dataupdatecheck.Checked != true)
                            {
                                if (mandatoryColCount <= values.Length && values.Length <= dataTable.Columns.Count)
                                {//if each row contains same total no of header columns
                                    for (int i = 0; i < values.Length; i++)
                                        dr[i] = values[i];
                                    dataTable.Rows.Add(dr);
                                    fileReadSuccess = true;
                                }
                                else
                                {//if any of the row does not contain the expected number of columns
                                    IsFileUploaded.Value = "";
                                    ScriptManager.RegisterStartupScript(updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('error', 4039, 'client');uploadFileClickEvent(); uploadFileChangeEvent();$('#divProgress').hide();", true);
                                    hdnUploadFileWarnings.Value = "NotEqualColumns";
                                    dataTable.Rows.Clear();
                                    dataTable.Columns.Clear();
                                    fileReadSuccess = false;
                                    break;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < values.Length; i++)
                                    dr[i] = values[i];
                                dataTable.Rows.Add(dr);
                                fileReadSuccess = true;
                            }
                            if (values.Length < dataTable.Columns.Count)
                            {
                                hdnIgnoredColCount.Value = values.Length.ToString();
                            }
                        }
                    }
                }
            }
            return dataTable;
        }
        catch (Exception ex)
        {
            logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Import Data - ReadCSVfile", "", "true");
            dataTable.Rows.Clear();
            dataTable.Columns.Clear();
            return dataTable;
        }
    }

    [WebMethod]
    public static string GetSheets(string sheetname, string filename, string xlhasheader)
    {

        aspx_ImportNew Importnew = new aspx_ImportNew();
        string sid = HttpContext.Current.Session["nsessionid"].ToString();
        string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        string sFileDir = ScriptsPath + "Axpert\\" + sid + "\\";
        int sheetnames = Convert.ToInt32(sheetname);
        List<DataTable> dataTabless;
        if (xlhasheader != "" && xlhasheader == "false")
        {
            dataTabless = GetExcelDataTablesNoHDR(sFileDir + filename);
        }
        else
        {
            dataTabless = GetExcelDataTables(sFileDir + filename);
        }
        DataTable sheet = dataTabless[Convert.ToInt32(sheetname)]; // Access the second DataTable (index 1)
        string headersString = Importnew.GetHeadersAsString(sheet);
        var result = new
        {
            Headers = headersString,
            Data = JsonConvert.SerializeObject(dataTabless[sheetnames])
        };

        return JsonConvert.SerializeObject(result);
    }


    [WebMethod]
    public static int GetDcs(string transid)
    {
        Util.Util util = new Util.Util();
        TStructDef strObj = util.GetTstructDefObj("Get structure for", transid.Trim());
        return strObj.dcs.Count;

    }

    [WebMethod]
    public static string GenerateExcel(string transid, string[][] arraysByDC)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            // Dynamically create sheets based on arraysByDC
            for (int i = 0; i < arraysByDC.Length; i++)
            {
                int numbersheet = i + 1;
                var worksheet = package.Workbook.Worksheets.Add(transid + "~" + ("dc" + numbersheet).ToString());
                var headers = arraysByDC[i];
                for (int j = 0; j < headers.Length; j++)
                {
                    worksheet.Cells[1, j + 1].Value = headers[j];
                    worksheet.Column(j + 1).Style.Numberformat.Format = "@";
                }
            }
            using (var memoryStream = new MemoryStream())
            {
                package.SaveAs(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                // Convert the byte array to a Base64 string
                return Convert.ToBase64String(fileBytes);
            }
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        // File path stored in a string variable
        string filePath = hdnExcelfilepath.Value;//@"E:\importexcel\Download\empdtimportdata.xlsx";
        string fileName = hdnExcelfileName.Value;
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        Response.TransmitFile(filePath);
        Response.End();
    }


    [WebMethod]
    public static string DownloadExcelFile(string filepathxl)
    {
        return filepathxl;
    }

    [WebMethod]
    public static string btnCreate(string filename, string nongridarray, string gridarray)
    {
        string FilenameEncrpt = "";
        try
        {
            string filePath = "";
            string scriptsPath = string.Empty, sessionId = string.Empty; //filename = string.Empty;

            if (System.Configuration.ConfigurationManager.AppSettings["ScriptsPath"] != null)
                scriptsPath = System.Configuration.ConfigurationManager.AppSettings["ScriptsPath"].ToString();

            if (HttpContext.Current.Session["nsessionid"] != null)
                sessionId = HttpContext.Current.Session["nsessionid"].ToString();

            //filename = hdnTemplateName.Value;

            filePath = scriptsPath + @"Axpert\" + sessionId;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //filePath += @"\" + filename + ".csv";
            filePath += @"\" + filename;
            StringBuilder sb = new StringBuilder();
            ArrayList cols = new ArrayList();
            ArrayList dupCol = new ArrayList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Example arrays, replace with your actual arrays
                string[] nongridarrays = nongridarray.Split(',');
                string[] gridarrays = gridarray.Split(',');

                // Array containing DC numbers, replace with your actual array
                string[] dcno = { "DC1", "DC2" };
                var existingSheet1 = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "SheetDC1");
                if (existingSheet1 != null)
                {
                    package.Workbook.Worksheets.Delete(existingSheet1);
                }

                var existingSheet2 = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "SheetDC2");
                if (existingSheet2 != null)
                {
                    package.Workbook.Worksheets.Delete(existingSheet2);
                }
                // Create sheets dynamically based on dcno array
                for (int i = 0; i < dcno.Length; i++)
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet" + dcno[i].ToString()); // Add a sheet with the DC number as the name
                    if (i == 0)
                    {
                        // Populate the first sheet with nongridarray headers
                        for (int j = 0; j < nongridarrays.Length; j++)
                        {
                            worksheet.Cells[1, j + 1].Value = nongridarrays[j]; // Write each header to the first row
                        }
                    }
                    else if (i == 1)
                    {
                        // Populate the second sheet with gridarray headers
                        for (int j = 0; j < gridarrays.Length; j++)
                        {
                            worksheet.Cells[1, j + 1].Value = gridarrays[j]; // Write each header to the first row
                        }
                    }
                }

                // Save the Excel package
                package.Save();
            }
            FilenameEncrpt = filename;
            if (filename.IndexOf("&") > -1)
            {
                FilenameEncrpt = filename.Replace('&', '♠');
            }
        }
        catch (Exception ex)
        {
        }
        return filename;
    }
    //If Tstruct selection has changed then get all fields & bind it to the Multiselect control using ASP.NET Repeator control 
    [WebMethod]
    public static string GetFields(string transid)
    {
        Util.Util util = new Util.Util();
        TStructDef strObj = util.GetTstructDefObj("Get structure for", transid.Trim());
        //Session["transid"] = temp;
        List<string> myCollection = new List<string>();
        // string combinedString1 = "";
        int oldDcNo = 0;
        StringBuilder strDcFlds = new StringBuilder();

        Dictionary<string, string> multiselect = new Dictionary<string, string>();
        StringBuilder visibleTstFlds = new StringBuilder();
        StringBuilder visibleTstFldsGridDc = new StringBuilder();
        StringBuilder visibleTstFldsNonGriddc = new StringBuilder();
        string gridDc = "false";
        for (int i = 0; i < strObj.flds.Count; i++)
        {
            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)strObj.flds[i];

            if (fld.fldframeno != oldDcNo)
            {
                TStructDef.DcStruct dc = (TStructDef.DcStruct)strObj.dcs[fld.fldframeno - 1];
                if (dc.isgrid)
                {
                    gridDc = "true";
                    // visibleTstFldsGridDc.Append(dc.dcPList + "♠").ToString();
                    if (fld.savevalue && !fld.name.StartsWith("axp_recid") && fld.datatype.ToLower() != "image" && !fld.visibility && fld.moe != "AutoGenerate")
                    {
                        visibleTstFldsGridDc.Append(fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")" + "♠" + fld.fldframeno + ",");
                    }
                }
                else
                {
                    //visibleTstFlds.Append(dc.name + "false" + "♠");
                    if (fld.savevalue && !fld.name.StartsWith("axp_recid") && fld.datatype.ToLower() != "image" && !fld.visibility)//&& fld.moe != "AutoGenerate"
                    {
                        visibleTstFldsNonGriddc.Append(fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")" + "♠" + fld.fldframeno + ",");
                        multiselect.Add(fld.name + "&&" + fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")", fld.caption + "(" + fld.name + ")");
                    }

                }
            }
            if (fld.savevalue && !fld.name.StartsWith("axp_recid") && fld.datatype.ToLower() != "image" && !fld.visibility)//&& fld.moe != "AutoGenerate"
            {
                visibleTstFlds.Append(fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")" + ",");
            }
        }

        visibleTstFlds.Append("♠" + gridDc);
        var response = new
        {
            // Multiselect = multiselect,
            VisibleTstFlds = visibleTstFlds,
            visibleTstFldsGridDc = visibleTstFldsGridDc,
            visibleTstFldsNonGriddc = visibleTstFldsNonGriddc
        };

        return JsonConvert.SerializeObject(response);
        //return visibleTstFlds.ToString();
    }

    #region
    private void GetAxRulesDef(TStructDef strObj, string transId)
    {
        try
        {
            string ScriptOnSubmitXML = string.Empty;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            string strAxRulesUser = string.Empty;
            string conRuleUser = Constants.AXRULESDEFUserRole;
            strAxRulesUser = fObj.StringFromRedis(util.GetRedisServerkey(conRuleUser, transId), schemaName);
            if (strAxRulesUser != string.Empty && strAxRulesUser != "" && strAxRulesUser != "<axrulesdef></axrulesdef>")// User role wise rules 
            {
                int ruledefNo = 0;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strAxRulesUser);

                XmlNode axruleDefChildNodes = default(XmlNode);
                axruleDefChildNodes = xmlDoc.DocumentElement.SelectSingleNode("//axrulesdef"); //ruleDefNodes.ChildNodes;
                StringBuilder ruledeflist = new StringBuilder();
                StringBuilder ruledeflistFormCont = new StringBuilder();
                string ScriptOnLoad = "false", ScriptOnSubmit = "false", formcontrol = "false", formcontrolparents = "false";
                foreach (XmlNode rulesNode in axruleDefChildNodes)
                {
                    foreach (XmlNode rulesChildNode in rulesNode)
                    {
                        if (rulesChildNode.Name.ToLower() == "scriptonload")
                        {
                            string strSOL = "";
                            if (rulesChildNode.InnerText != "")
                            {
                                ScriptOnLoad = "true";
                                strSOL = rulesChildNode.InnerText.Replace("\n", "♥").Replace("^", "♥");
                            }
                            //ruledeflist.Append("var AxRDScriptOnLoad[" + ruledefNo + "] = " + "\"" + strSOL + "\";");
                            ruledeflist.Append(strSOL + '♠');
                        }
                        else if (rulesChildNode.Name.ToLower() == "onsubmit")
                        {
                            if (rulesChildNode.InnerText != "")
                            {
                                ScriptOnSubmit = "true";
                                ScriptOnSubmitXML = rulesChildNode.InnerText.Replace("^", "\n");
                            }
                        }
                        else if (rulesChildNode.Name.ToLower() == "formcontrolparent")
                        {
                            if (rulesChildNode.InnerText != "")
                                formcontrolparents = "true";
                            //ruledeflist.Append("var AxRDFormControlParent[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                        }
                        else if (rulesChildNode.Name.ToLower() == "formcontrol")
                        {
                            string strFc = "";
                            if (rulesChildNode.InnerText != "")
                            {
                                formcontrol = "true";
                                strFc = rulesChildNode.InnerText.Replace("\n", "♥").Replace("^", "♥");
                            }
                            //ruledeflist.Append("var AxRDFormControl[" + ruledefNo + "] = " + "\"" + strFc + "\";");
                            ruledeflistFormCont.Append(strFc + '♠');
                        }
                    }
                    ruledefNo++;
                }
                string strRulesDefEngin = ScriptOnLoad + "~" + ScriptOnSubmit + "~" + formcontrol + "~" + formcontrolparents;
                //string jsRuleDefArray = ruledeflist.ToString() + "var AxRulesEngine=\"" + strRulesDefEngin + "\";";
                axRuleDetails.Value = strRulesDefEngin;
                axRuleScript.Value = ruledeflistFormCont.ToString();
                axRuleOnSubmit.Value = ruledeflist.ToString();
            }
            else if (strAxRulesUser == string.Empty || strAxRulesUser == "")//If user role wise is empty rule then need to check in full rule key
            {
                string strAxRules = string.Empty;
                string conRule = Constants.AXRULESDEF;
                strAxRules = fObj.StringFromRedis(util.GetRedisServerkey(conRule, transId), schemaName);

                if (strAxRules != string.Empty && strAxRules != "" && strAxRules != "<axrulesdef></axrulesdef>")
                {
                    StringBuilder ruledeflist = new StringBuilder();
                    StringBuilder ruledeflistFormCont = new StringBuilder();
                    int ruledefNo = 0;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(strAxRules);

                    XmlNode axruleDefChildNodes = default(XmlNode);
                    axruleDefChildNodes = xmlDoc.DocumentElement.SelectSingleNode("//axrulesdef"); //ruleDefNodes.ChildNodes;

                    StringBuilder sbUserRules = new StringBuilder();
                    string ScriptOnLoad = "false", ScriptOnSubmit = "false", formcontrol = "false", formcontrolparents = "false";
                    foreach (XmlNode rulesNode in axruleDefChildNodes)
                    {
                        XmlDocument xmlDocNode = new XmlDocument();
                        xmlDocNode.LoadXml(rulesNode.OuterXml);
                        XmlNode axruleDefNodes = default(XmlNode);
                        axruleDefNodes = xmlDocNode.SelectSingleNode("//uroles");
                        bool isRolesMath = false;
                        if (axruleDefNodes.InnerText != "")
                        {
                            string[] rdefRoles = axruleDefNodes.InnerText.Split(',');
                            string userRoles = Session["AxRole"].ToString();
                            string[] userRolesList = userRoles.Split(',');
                            foreach (var rdrName in rdefRoles)
                            {
                                int index = Array.IndexOf(userRolesList, rdrName);
                                if (index != -1 || (rdrName != "" && rdrName.ToLower() == "default"))
                                {
                                    isRolesMath = true;
                                    break;
                                }
                            }
                        }
                        if (isRolesMath)
                        {
                            sbUserRules.Append(rulesNode.OuterXml);

                            foreach (XmlNode rulesChildNode in rulesNode)
                            {
                                if (rulesChildNode.Name.ToLower() == "scriptonload")
                                {
                                    string strSOL = "";
                                    if (rulesChildNode.InnerText != "")
                                    {
                                        ScriptOnLoad = "true";
                                        strSOL = rulesChildNode.InnerText.Replace("\n", "♥").Replace("^", "♥");
                                    }
                                    //ruledeflist.Append("var AxRDScriptOnLoad[" + ruledefNo + "] = " + "\"" + strSOL + "\";");
                                    ruledeflist.Append(strSOL + '♠');
                                }
                                else if (rulesChildNode.Name.ToLower() == "onsubmit")
                                {
                                    if (rulesChildNode.InnerText != "")
                                    {
                                        ScriptOnSubmit = "true";
                                        ScriptOnSubmitXML = rulesChildNode.InnerText.Replace("^", "\n");
                                    }
                                }
                                else if (rulesChildNode.Name.ToLower() == "formcontrolparent")
                                {
                                    if (rulesChildNode.InnerText != "")
                                        formcontrolparents = "true";
                                    //ruledeflist.Append("var AxRDFormControlParent[" + ruledefNo + "] = " + "\"" + rulesChildNode.InnerText + "\";");
                                }
                                else if (rulesChildNode.Name.ToLower() == "formcontrol")
                                {
                                    string strFc = "";
                                    if (rulesChildNode.InnerText != "")
                                    {
                                        formcontrol = "true";
                                        strFc = rulesChildNode.InnerText.Replace("\n", "♥").Replace("^", "♥");
                                    }
                                    //ruledeflist.Append("var AxRDFormControl[" + ruledefNo + "] = " + "\"" + strFc + "\";");
                                    ruledeflistFormCont.Append(strFc + '♠');
                                }
                            }
                            ruledefNo++;
                        }
                    }

                    try
                    {
                        FDW fdwObj = new FDW();
                        string conRuleuser = Constants.AXRULESDEFUserRole;
                        fdwObj.SaveInRedisServer(util.GetRedisServerkey(conRuleuser, transId), "<axrulesdef>" + sbUserRules + "</axrulesdef>", Constants.AXRULESDEFUserRole, schemaName);
                    }
                    catch (Exception ex)
                    { }

                    string strRulesDefEngin = ScriptOnLoad + "~" + ScriptOnSubmit + "~" + formcontrol + "~" + formcontrolparents;
                    //string jsRuleDefArray = ruledeflist.ToString() + "var AxRulesEngine=\"" + strRulesDefEngin + "\";";
                    axRuleDetails.Value = strRulesDefEngin;
                    axRuleScript.Value = ruledeflistFormCont.ToString();
                    axRuleOnSubmit.Value = ruledeflist.ToString();
                }
                else
                {
                    axRuleDetails.Value = "";
                    axRuleScript.Value = "";
                    axRuleOnSubmit.Value = "";
                }
            }
        }
        catch (Exception ex)
        { }
    }
    #endregion

    private string CheckSpecialChars(string str)
    {
        //hack: The below line is used to make sure that the & in &amp; is not converted inadvertantly
        //      for other chars this scenario will not come as it does not contains the same char.
        str = Regex.Replace(str, "&amp;", "&");
        str = Regex.Replace(str, "&quot;", "“");
        str = Regex.Replace(str, "\n", "<br>");
        str = Regex.Replace(str, "&", "&amp;");
        str = Regex.Replace(str, "<", "&lt;");
        str = Regex.Replace(str, ">", "&gt;");
        str = Regex.Replace(str, "'", "&apos;");
        str = Regex.Replace(str, "\"", "&quot;");
        str = Regex.Replace(str, "’", "&apos;");
        str = Regex.Replace(str, "“", "&quot;");
        str = Regex.Replace(str, "”", "&quot;");
        str = Regex.Replace(str, "™", "&#8482;");
        str = Regex.Replace(str, "®", "&#174;");

        str = str.Replace((char)160, ' ');

        return str;
    }

    //To display the Imported summary in Table format & create a link to download Import summary file
    private void ParseResult(string result, string calledFrom)
    {
        SummaryDwnld.Visible = false;
        if (result != string.Empty)
        {
            if (result.Contains(Constants.SESSIONEXPMSG) || result.Contains(Constants.ERAUTHENTICATION))
                SessExpires();
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(result);
                }
                catch (Exception ex)
                {
                    logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Import Data - ParseResult", "", "true");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SessExpiresMessage", "parent.parent.location.href='" + util.ERRPATH + "'", true);
                }

                XmlNode root = xmlDoc.ChildNodes[0];
                string fileName = string.Empty;
                string filePath = string.Empty;
                string err = string.Empty;
                string expCount = string.Empty;
                string failedSummaryFileName = string.Empty;
                //sample <result>
                //<filename>test.txtText</filename><filepath>C:\inetpub\wwwroot\scriptsperf\32kkfe2kqamm3d55mpl4tq55\</filepath><expcount>6</expcount></result>
                foreach (XmlNode chNode in root)
                {
                    if (chNode.Name == "filename")
                        fileName = chNode.InnerText;
                    else if (chNode.Name == "filepath")
                        filePath = chNode.InnerText;
                    else if (chNode.Name == "imperr" || chNode.Name == "errorrows")
                        err += chNode.InnerText;
                    else if (chNode.Name == "expcount")
                        expCount = chNode.InnerText;
                    else if (chNode.Name == "errorfilename")
                        failedSummaryFileName = chNode.InnerText;
                }

                string saveFailure = filePath;
                filePath = filePath + fileName;
                if (filePath != string.Empty)
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(filePath);

                    string text = string.Empty;
                    //set appropriate headers
                    if (file.Exists)
                    {
                        //string failedSummaryFileName = string.Empty;
                        text = System.IO.File.ReadAllText(filePath);
                        summaryText.InnerText = text;

                        int NumberOfRecordsInFile = 0;
                        int NumberOfRecordsImp = 0;
                        int NumberOfRecordsNotImp = 0;
                        int added = 0, updated = 0;
                        string line;
                        using (StreamReader file1 = new StreamReader(filePath))
                        {
                            while ((line = file1.ReadLine()) != null)
                            {
                                if (line.Contains("No of Transactions from File :") || line.Contains("No of Records from File :"))
                                {
                                    string NumberOfRecords = line;
                                    NumberOfRecordsInFile = NumberOfRecordsInFile + Convert.ToInt32(NumberOfRecords.Substring(NumberOfRecords.IndexOf(':') + 1).Trim());
                                }
                                if (line.Contains("No of Transactions imported :") || line.Contains("No of Records imported :"))
                                {
                                    string NumberOfRecordsImpLine = line;
                                    NumberOfRecordsImp = NumberOfRecordsImp + Convert.ToInt32(NumberOfRecordsImpLine.Substring(NumberOfRecordsImpLine.IndexOf(':') + 1).Trim());
                                    added = Convert.ToInt32(NumberOfRecordsImpLine.Substring(NumberOfRecordsImpLine.IndexOf(':') + 1).Trim());
                                }
                                if (line.Contains("No of Transactions Updated :") || line.Contains("No of Records Updated :"))
                                {
                                    string NumberOfRecordsUpdatedLine = line;
                                    NumberOfRecordsImp = NumberOfRecordsImp + Convert.ToInt32(NumberOfRecordsUpdatedLine.Substring(NumberOfRecordsUpdatedLine.IndexOf(':') + 1).Trim());
                                    updated = Convert.ToInt32(NumberOfRecordsUpdatedLine.Substring(NumberOfRecordsUpdatedLine.IndexOf(':') + 1).Trim());
                                }
                                if (line.Contains("No of Transactions not imported :") || line.Contains("No of Records not imported :"))
                                {
                                    string NumberOfRecordsNotImpLine = line;
                                    NumberOfRecordsNotImp = NumberOfRecordsNotImp + Convert.ToInt32(NumberOfRecordsNotImpLine.Substring(NumberOfRecordsNotImpLine.IndexOf(':') + 1).Trim());
                                }
                                if (line.Contains("No of Error Transactions (not Validated) :") || line.Contains("No of Error Records (not Validated) :"))
                                {
                                    string NumberOfRecordsNotImpLine = line;
                                    NumberOfRecordsNotImp = NumberOfRecordsNotImp + Convert.ToInt32(NumberOfRecordsNotImpLine.Substring(NumberOfRecordsNotImpLine.IndexOf(':') + 1).Trim());
                                }
                                if (line.Contains("No of Transactions Validated :") || line.Contains("No of Records Validated :"))
                                {
                                    string NumberOfRecordsNotImpLine = line;
                                    NumberOfRecordsNotImp = NumberOfRecordsNotImp + Convert.ToInt32(NumberOfRecordsNotImpLine.Substring(NumberOfRecordsNotImpLine.IndexOf(':') + 1).Trim());
                                }
                            }
                        }

                        StringBuilder sbreport = new StringBuilder();
                        sbreport.Append("<table class='table gridData'  style='width:100%;text-align:center;margin:auto'>");
                        sbreport.Append("<thead><tr><th style='width:40%;' id='thSummFileName'></th><th id='thSummRecords' style='width:24%;'></th><th id='thSummAdded' style='width:12%;'></th><th id='thSummUpdated' style='width:12%;'></th><th style='width:12%;' id='thSummFailed'></th></tr></thead>");
                        sbreport.Append("<tr>");
                        sbreport.Append("<td style=\"text-align:left;\">" + uploadFileName.Value);
                        sbreport.Append("</td>");
                        sbreport.Append("<td>" + NumberOfRecordsInFile);
                        sbreport.Append("</td>");
                        sbreport.Append("<td>" + added);
                        sbreport.Append("</td>");
                        sbreport.Append("<td>" + updated);
                        sbreport.Append("</td>");
                        //sbreport.Append("<td>" + NumberOfRecordsImp);
                        //sbreport.Append("</td>");
                        sbreport.Append("<td>" + NumberOfRecordsNotImp);
                        sbreport.Append("</td>");
                        sbreport.Append("</tr>");
                        sbreport.Append("</table>");
                        ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "setFailedSummaryColumnHeadings", "setFailedSummaryColumnHeadings()", true);
                        if (NumberOfRecordsNotImp != 0)
                        {
                            string[] lines = System.IO.File.ReadAllLines(filePath);
                            string flag = string.Empty;
                            StringBuilder builder = new StringBuilder();

                            for (int i = 0; i < lines.Count(); i++)
                            {
                                var temp = lines[i].ToString().Split('\t');
                                if (lines[i].Contains("Error at Transactions no : "))
                                {
                                    builder.AppendLine(string.Join(",", lines[i].Replace("Error at Transactions no : ", "")));
                                }
                                else if (lines[i].Contains("Row No : "))
                                {
                                    builder.AppendLine(string.Join(",", lines[i].Replace("Row No : ", "")));
                                }
                                else if (lines[i].Contains("Field : "))
                                {
                                    builder.AppendLine(string.Join(",", lines[i].Replace("Field : ", "")));
                                }
                                else if (lines[i].Contains("Value : "))
                                {
                                    builder.AppendLine(string.Join(",", lines[i].Replace("Value : ", "")));
                                }
                                else if (lines[i].Contains("Error Message : "))
                                {
                                    builder.AppendLine(string.Join(",", lines[i].Replace("Error Message : ", "")));
                                }
                            }

                            string[] textToCsv = builder.ToString().Split('\n');
                            StringBuilder txttocsv = new StringBuilder();
                            txttocsv.AppendLine("Error at record no,Row No,Field,Value,Reason");
                            string oneLine = string.Empty;
                            for (int i = 0; i < textToCsv.Count(); i++)
                            {
                                oneLine += textToCsv[i].Replace('\r', ',');
                                if ((i + 1) % 5 == 0)
                                {
                                    txttocsv.AppendLine(oneLine);
                                    oneLine = string.Empty;
                                }
                            }
                            SummaryDwnld.Visible = true;
                            if (failedSummaryFileName != string.Empty)
                            {
                                SummaryDwnld.HRef = "openfile.aspx?fpath=" + failedSummaryFileName + "&Imp=t";
                            }
                            else
                            {
                                SummaryDwnld.Attributes["class"] = "btn btn-light-primary noHover";
                            }
                        }
                        lblTest.Visible = true;
                        lblTest.Text = sbreport.ToString();

                        if (NumberOfRecordsImp > 0 && NumberOfRecordsNotImp == 0)
                        {
                            ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('success', 4045, 'client');wizardTabFocus('wizardPrevbtn', 'wizardCompbtn');", true);
                        }
                        else if (NumberOfRecordsImp > 0 && NumberOfRecordsNotImp > 0)
                        {
                            ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning', 4046, 'client');wizardTabFocus('SummaryDwnld', 'wizardCompbtn');", true);
                        }
                        else if (NumberOfRecordsImp == 0 && NumberOfRecordsNotImp > 0)
                        {
                            ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning', 4047, 'client');wizardTabFocus('SummaryDwnld', 'wizardCompbtn');", true);
                        }
                        else if (NumberOfRecordsImp == 0 && NumberOfRecordsNotImp == 0 && err != string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog(\"warning\",\"" + err.ToString() + "\");wizardTabFocus('wizardPrevbtn', 'wizardCompbtn');", true);
                        }
                        ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "StopDimmer", "ShowDimmer(false);", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning', 4047, 'client');wizardTabFocus('wizardPrevbtn', 'wizardCompbtn');", true);
                        SummaryDwnld.Visible = false;
                        lblTest.Text = "";
                        lblTest.Text = "Error while importing.";
                        ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "StopDimmer", "ShowDimmer(false);", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning', 4047, 'client');wizardTabFocus('wizardPrevbtn', 'wizardCompbtn');", true);
                    lblTest.Text = "Error while importing<br/>" + err;
                    ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "StopDimmer", "ShowDimmer(false);", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning', 4047, 'client');wizardTabFocus('wizardPrevbtn', 'wizardCompbtn');", true);
            lblTest.Text = "Error while importing";
            ScriptManager.RegisterStartupScript(updatePln3, typeof(UpdatePanel), "StopDimmer", "ShowDimmer(false);", true);
        }
    }

    private void RemoveUploadedFile()
    {
        try
        {
            string sessionPath = util.ScriptsPath + "axpert\\" + sid;
            string filename = string.Empty;
            DirectoryInfo di = new DirectoryInfo(sessionPath);
            //' Determine whether the directory exists.
            if (di.Exists)
            {
                filename = sessionPath + "\\" + upFileName.Value;
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }
        catch (Exception ex)
        {
            lblTest.Text = "Error while importing.Please try again.";
            logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Import Data - RemoveUploadedFile", "", "true");
        }
    }

    private void SessExpires()
    {
        if (IsFileUploaded.Value == "1")
            RemoveUploadedFile();
        string url = util.SESSEXPIRYPATH;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "SessExpiresMessage", "parent.parent.location.href='" + url + "'", true);
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }

    //to get all Tstructs based on username
    public string GetAllTStructs()
    {
        //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
        ASB.WebService objCWbSer = new ASB.WebService();
        string username = Session["user"].ToString();
        string query = string.Format(Constants.IMPEXP_GETTSTRUCTS, username);
        string result = objCWbSer.GetChoicesCustomWS("", query);
        if (result.Contains(Constants.SESSIONEXPMSG))
        {
            SessExpires();
        }
        return result;
    }

    static List<DataTable> GetExcelDataTables(string filepath)
    {
        List<DataTable> dataTables = new List<DataTable>();

        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + filepath + "\";Extended Properties=\"Excel 12.0; HDR=YES;IMEX=1\"";
        try
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    string sheetName = schemaRow["TABLE_NAME"].ToString();

                    // Normalize sheet name (handle quoted names)
                    if (sheetName.StartsWith("'") && sheetName.EndsWith("'"))
                        sheetName = sheetName.Substring(1, sheetName.Length - 2);

                    if (!sheetName.EndsWith("$"))
                        continue;

                    string query = string.Format("SELECT * FROM [{0}]", sheetName);

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable(sheetName.TrimEnd('$'));
                        bool schemaLoaded = false;
                        int rowCount = 0;

                        // Force-load schema even if no data
                        DataTable schema = reader.GetSchemaTable();
                        if (schema != null)
                        {
                            foreach (DataRow col in schema.Rows)
                            {
                                string colName = col["ColumnName"].ToString();
                                Type dataType = (Type)col["DataType"];
                                dt.Columns.Add(colName, dataType);
                            }
                            schemaLoaded = true;
                        }

                        while (reader.Read() && rowCount < 10)
                        {
                            DataRow newRow = dt.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                newRow[i] = reader.IsDBNull(i) ? "" : reader.GetValue(i);

                            }
                            dt.Rows.Add(newRow);
                            rowCount++;
                        }

                        // Add DataTable even if only headers exist
                        if (dt.Columns.Count > 0)
                            dataTables.Add(dt);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Optionally log ex.Message
        }

        return dataTables;
    }


    static List<DataTable> GetExcelDataTablesNoHDR(string filepath)
    {
        List<DataTable> dataTables = new List<DataTable>();

        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + filepath + "\";Extended Properties=\"Excel 12.0; HDR=NO;IMEX=1\"";
        try
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    string sheetName = schemaRow["TABLE_NAME"].ToString();
                    // Normalize sheet name (handle quoted names)
                    if (sheetName.StartsWith("'") && sheetName.EndsWith("'"))
                    {
                        sheetName = sheetName.Substring(1, sheetName.Length - 2);
                    }

                    // Only process worksheet tables ending with $
                    if (!sheetName.EndsWith("$"))
                        continue;
                    // Query using valid worksheet name
                    string query = string.Format("SELECT * FROM [{0}]", sheetName); //"SELECT * FROM [" + sheetName + "$]";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable(sheetName.TrimEnd('$'));
                        bool schemaLoaded = false;
                        int rowCount = 0;

                        while (reader.Read() && rowCount < 10)
                        {
                            if (!schemaLoaded)
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                                }
                                schemaLoaded = true;
                            }

                            DataRow newRow = dt.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                newRow[i] = reader.IsDBNull(i) ? "" : reader.GetValue(i);
                            }
                            dt.Rows.Add(newRow);
                            rowCount++;
                        }

                        dataTables.Add(dt);
                    }
                }
            }
        }
        catch (Exception ex) { }

        return dataTables;
    }

    static void UpdateSheetNamesWithOleDb(string sourceFilePath, string[] updatedSheetNames, string destinationFilePath)
    {
        try
        {
            // Ensure network directory exists
            string networkDirectory = Path.GetDirectoryName(destinationFilePath);
            if (!Directory.Exists(networkDirectory))
            {
                Console.WriteLine("Network directory does not exist.");
                return;
            }

            // Read the original file
            string sourceConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + sourceFilePath + "\";Extended Properties=\"Excel 12.0; HDR=NO;IMEX=1\"";
            DataSet dataSet = new DataSet();

            using (OleDbConnection sourceConnection = new OleDbConnection(sourceConnectionString))
            {
                sourceConnection.Open();
                DataTable schemaTable = sourceConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow row in schemaTable.Rows)
                {
                    string sheetName = row["TABLE_NAME"].ToString();

                    string query = "SELECT * FROM [" + sheetName + "]";
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, sourceConnection))
                    {
                        DataTable dataTable = new DataTable(sheetName.TrimEnd('$'));
                        adapter.Fill(dataTable);
                        dataSet.Tables.Add(dataTable);
                    }
                }
            }

            // Write to the new file with updated sheet names
            string destinationConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + networkDirectory + "\";Extended Properties=\"Excel 12.0; HDR=NO;IMEX=1\"";
            //string destinationConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + destinationFilePath + "\";Extended Properties=\"Excel 12.0; HDR=NO;IMEX=1\"";
            using (OleDbConnection destinationConnection = new OleDbConnection(destinationConnectionString))
            {
                destinationConnection.Open();
                for (int i = 0; i < dataSet.Tables.Count && i < updatedSheetNames.Length; i++)
                {
                    DataTable table = dataSet.Tables[i];
                    string createTableQuery = GenerateCreateTableQuery(updatedSheetNames[i], table);
                    using (OleDbCommand createCommand = new OleDbCommand(createTableQuery, destinationConnection))
                    {
                        createCommand.ExecuteNonQuery();
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        string insertQuery = GenerateInsertQuery(updatedSheetNames[i], table, row);
                        using (OleDbCommand insertCommand = new OleDbCommand(insertQuery, destinationConnection))
                        {
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            // Console.WriteLine($"File saved successfully to {destinationFilePath}");
        }
        catch (Exception ex)
        {
            // Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }
    static string GenerateCreateTableQuery(string sheetName, DataTable table)
    {
        // Build the column definitions manually
        string columns = "";
        foreach (DataColumn col in table.Columns)
        {
            columns += "[" + col.ColumnName + "] TEXT, ";
        }

        // Remove the trailing comma and space
        if (columns.Length > 2)
        {
            columns = columns.Substring(0, columns.Length - 2);
        }

        return "CREATE TABLE [" + sheetName + "] (" + columns + ")";
    }

    static string GenerateInsertQuery(string sheetName, DataTable table, DataRow row)
    {
        // Build the column names manually
        string columns = "";
        foreach (DataColumn col in table.Columns)
        {
            columns += "[" + col.ColumnName + "], ";
        }

        // Remove the trailing comma and space
        if (columns.Length > 2)
        {
            columns = columns.Substring(0, columns.Length - 2);
        }

        // Build the values manually
        string values = "";
        foreach (object value in row.ItemArray)
        {
            string sanitizedValue = value != null
                ? value.ToString().Replace("'", "''")
                : ""; // Handle nulls gracefully
            values += "'" + sanitizedValue + "', ";
        }

        // Remove the trailing comma and space
        if (values.Length > 2)
        {
            values = values.Substring(0, values.Length - 2);
        }

        return "INSERT INTO [" + sheetName + "] (" + columns + ") VALUES (" + values + ")";
    }



    public void exceldt(string filePath)
    {
        string sSheetName = null;
        string sConnection = null;
        DataTable dtTablesList = default(DataTable);
        OleDbCommand oleExcelCommand = default(OleDbCommand);
        OleDbDataReader oleExcelReader = default(OleDbDataReader);
        OleDbConnection oleExcelConnection = default(OleDbConnection);
        DataTable dt = new DataTable();
        sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0; HDR=NO;IMEX=1\"";
        using (OleDbConnection connection = new OleDbConnection(sConnection))
        {
            connection.Open();

            // Get the list of sheet names in the Excel file
            DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            // Iterate through each sheet and create a DataTable for each
            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                string sheetName = schemaRow["TABLE_NAME"].ToString();

                // Remove the trailing $ character from the sheet name
                if (sheetName.EndsWith("$"))
                {
                    sheetName = sheetName.Substring(0, sheetName.Length - 1);
                }

                // Select all data from the current sheet
                string query = "SELECT * FROM [{sheetName}$]";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    DataTable dataTable = new DataTable(sheetName);
                    adapter.Fill(dataTable);

                    // You can now use the 'dataTable' object containing the data from each sheet
                    Console.WriteLine("DataTable created for sheet '{sheetName}' with {dataTable.Rows.Count} rows.");
                }
            }
        }
    }

    private void CheckDesignAccess()
    {
        try
        {
            if (Session["axDesign"] == null)
            {
                Session["axDesign"] = "false";
                string user = Session["user"].ToString();
                if (HttpContext.Current.Session["AxResponsibilities"] != null && HttpContext.Current.Session["AxDesignerAccess"] != null)
                {
                    if (user.ToLower() == "admin")
                        Session["axDesign"] = "true";
                    else
                    {
                        string[] arrAxResp = HttpContext.Current.Session["AxResponsibilities"].ToString().ToLower().Split(',');
                        string[] arrAxDesignerResp = HttpContext.Current.Session["AxDesignerAccess"].ToString().ToLower().Split(',');
                        foreach (string designerResp in arrAxDesignerResp)
                        {
                            if (arrAxResp.Contains(designerResp))
                            {
                                Session["axDesign"] = "true";
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Import data - CheckDesignAccess -" + ex.Message, HttpContext.Current.Session.SessionID, "CheckDesignAccess", "new");
        }
    }

    [WebMethod]
    public static string GetTemplates(string transid)
    {
        try
        {
            ASBExt.WebServiceExt asbEx = new ASBExt.WebServiceExt();
            string sqlQuery = Constants.IMP_GETTEMPLATES;
            sqlQuery = sqlQuery.Replace("$STRANSID$", transid);
            string result = string.Empty;
            result = asbEx.ExecuteSQL("", sqlQuery, "JSON");
            return result;
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("ImportData's_GetTemplates -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in ImportData's_GetTemplates", "new");
            return string.Empty;
        }
    }

    [WebMethod]
    public static void FileuplaodValidation()
    {
        try
        {
            aspx_ImportNew Importnew = new aspx_ImportNew();
            //string checkHeader = Importnew.hdnCheckHeader.Value;
            // Importnew.chkForIgnoreErr.Checked = true;
            string sid = HttpContext.Current.Session["nsessionid"].ToString();
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            string sFileName = Importnew.upFileName.Value;
            string sFileDir = ScriptsPath + "Axpert\\" + sid + "\\";
            if ((sFileName).IndexOf("xlsx") > -1)/*|| (sFileName).IndexOf("xls") > -1)*/
            {
                //Importnew.ddlSeparator.Visible = false;
                //Importnew.dt = Importnew.exceldt(sFileDir + sFileName);
                List<DataTable> dataTables = GetExcelDataTables(sFileDir + sFileName);
            }
            else
                Importnew.dt = Importnew.ReadCsvFile(sFileDir + sFileName);
            if (Importnew.fileReadSuccess)
            {
                if ((Importnew.dt.Columns.Count == 0 && Importnew.dt.Rows.Count == 0) || (Importnew.ChkColNameInfile.Checked && Importnew.dt.Rows.Count == 0))
                {
                    Importnew.IsFileUploaded.Value = "";
                    ScriptManager.RegisterStartupScript(Importnew.updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning', 4036, 'client');uploadFileClickEvent(); uploadFileChangeEvent();$('#divProgress').hide();", true);
                    Importnew.hdnUploadFileWarnings.Value = "Empty";
                    return;
                }
                if (Importnew.ChkColNameInfile.Checked)
                {
                    DataRow drr = Importnew.dt.Rows[0];
                    if ((sFileName).IndexOf(".xlsx") == -1)
                    {
                        Importnew.dt.Rows.Remove(drr);
                        //  headerList.AddRange(dt.Rows[0].ItemArray);
                    }


                }
                if (Importnew.dt.Rows.Count > 0)
                    Importnew.dt = Importnew.dt.Rows.Cast<DataRow>().Take(5).CopyToDataTable(); //to display only top 5 records in the Grid
                int selectedFieldCount = Importnew.hdnColNames.Value.Split(',').Length;
                //allowing user to go to next step only if selected no of columns is equal to total no of columns in uploaded csv file              
                if (Importnew.dt.Columns.Count == selectedFieldCount)//&& checkHeader!="true"
                {

                    if (Importnew.dt.Rows.Count > 0)
                    {
                        // if ((sFileName).IndexOf("xlsx") > -1)
                        Importnew.headerList.AddRange(Importnew.dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray());
                        var row = Importnew.dt.NewRow();
                        //row[0] = ((DataRow)headerList);
                        // DataRow drr1 = headerList;

                        Importnew.IsFileUploaded.Value = "1";
                        Importnew.hdnUploadFileWarnings.Value = "Success";
                        ScriptManager.RegisterStartupScript(Importnew.updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "fileUploadSuccess();", true);
                        Importnew.gridImpData.DataSource = Importnew.dt;
                        Importnew.gridImpData.DataBind();
                    }
                    else
                    {
                        Importnew.IsFileUploaded.Value = "";
                        Importnew.hdnUploadFileWarnings.Value = "Empty";
                        ScriptManager.RegisterStartupScript(Importnew.updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning', 4038, 'client');uploadFileClickEvent(); uploadFileChangeEvent();callParentNew('DoUtilitiesEvent('ImportData'); ','function');", true);
                    }
                }
                else
                {
                    Importnew.IsFileUploaded.Value = "";
                    string warningMsg = Importnew.hdnUploadFileWarnings.Value == "NotEqualColumns" ? "showAlertDialog('error', 4039, 'client');" : "showAlertDialog('error', eval(callParent('lcm[310]')));";
                    ScriptManager.RegisterStartupScript(Importnew.updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "" + warningMsg + "uploadFileClickEvent(); uploadFileChangeEvent();$('#divProgress').hide();callParentNew('DoUtilitiesEvent('ImportData'); ','function');", true);//"callParentNew('closeFrame(); ','function');",
                }
            }
            else
            {
                Importnew.IsFileUploaded.Value = "";
                string warningMsg = Importnew.hdnUploadFileWarnings.Value == "NotEqualColumns" ? "showAlertDialog('error', 4039);" : "showAlertDialog('error', eval(callParent('lcm[310]')));";
                ScriptManager.RegisterStartupScript(Importnew.updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", warningMsg + "$('#divProgress').hide();", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    [WebMethod]
    public static string deleteTemplate(string transid, string tempname)
    {
        try
        {
            //ASBCustom.CustomWebservice cwsObj = new ASBCustom.CustomWebservice();
            ASB.WebService cwsObj = new ASB.WebService();
            string sqlQuery = Constants.IMP_DELTEMPLATES;
            sqlQuery = sqlQuery.Replace("$STRANSID$", transid);
            sqlQuery = sqlQuery.Replace("$TEMPLATENAME$", tempname);
            string result = cwsObj.GetChoicesCustomWS("", sqlQuery);

            if (result.Contains(Constants.ERROR) || result.Contains(Constants.SESSIONEXPMSG) || result.Contains(Constants.ERAUTHENTICATION))
            {
                return "";
            }
            else if (result == "done")
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logobj = new LogFile.Log();
            logobj.CreateLog("ImportData's_DeleteTemplate -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in ImportData's_DeleteTemplate", "new");
        }
        return "";
    }

    [WebMethod]
    public static string AxRuleSetMandatory(ArrayList AxAllowEmpty)
    {
        string res = string.Empty;
        try
        {
            if (AxAllowEmpty.Count > 0)
            {
                string axrulefldXMl = string.Empty;
                StringBuilder allowEmpty = new StringBuilder();
                foreach (string _val in AxAllowEmpty)
                {
                    string[] _aeFld = _val.Split('~');
                    allowEmpty.Append("<" + _aeFld[0] + ">" + _aeFld[1] + "</" + _aeFld[0] + ">");
                }
                axrulefldXMl = "<axrules>";
                axrulefldXMl += "<allowempty>" + allowEmpty.ToString() + "</allowempty>";
                axrulefldXMl += "</axrules>";
                HttpContext.Current.Session["AxRulesImportXML"] = axrulefldXMl;
                res = "done";
            }
            else
                res = string.Empty;
        }
        catch (Exception ex)
        {
            res = string.Empty;
        }
        return res;
    }

    [WebMethod]
    public static string PushToImportQueue(string queueData, string transid)
    {
        string response = string.Empty;
        try
        {
            string URL = String.Empty;
            string ArmScriptURL = string.Empty;
            if (HttpContext.Current.Session["ARM_URL"] != null)
                URL = HttpContext.Current.Session["ARM_URL"].ToString();
            if (HttpContext.Current.Session["ARM_Scripts_URL"] != null)
                ArmScriptURL = HttpContext.Current.Session["ARM_Scripts_URL"].ToString();

            if (URL == string.Empty || ArmScriptURL == string.Empty)
                return "RMQError: ARM Connection is not defined";

            transid = transid.Substring(transid.IndexOf("(") + 1).TrimEnd(')');
            string _RKey = transid + "-gblImport-" + HttpContext.Current.Session["username"].ToString() + "-AxConvertXLtoJSONQueue";
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            string armExportExcell = fdrObj.ReadStringKey(_RKey);
            if (armExportExcell != string.Empty && armExportExcell == "requestinprocess")
            {
                response = "keyexist";
                return response;
            }

            queueData = queueData.Replace("$SESSIONID$", HttpContext.Current.Session.SessionID);
            queueData = queueData.Replace("$USERNAME$", HttpContext.Current.Session["username"].ToString());
            queueData = queueData.Replace("$SIGNALRURL$", URL.TrimEnd('/') + "/api/v1/SendSignalR");
            queueData = queueData.Replace("$ARMSCRIPTURL$", ArmScriptURL.TrimEnd('/') + "/ASBRapidSaveRest.dll/datasnap/rest/TASBRapidSaveRest/ConvertXLToJSON");
            if (queueData.EndsWith("}"))
                queueData = queueData.Substring(0, queueData.Length - 1);

            string globalVars = HttpContext.Current.Session["axGlobalVars"].ToString();
            globalVars = globalVars.Replace(@"\", "\\\\");
            string _axapps = HttpContext.Current.Session["axApps"].ToString();
            _axapps = _axapps.Replace(@"\", "\\\\");

            queueData += ",\"axprops\":\"" + HttpContext.Current.Application["axProps"].ToString() + "\",\"axapps\":\"" + _axapps + "\",\"globalvars\":\"" + globalVars + "\",\"uservars\":\"" + HttpContext.Current.Session["axUserVars"].ToString() + "\"}";

            string pushToQueueURL = URL.TrimEnd('/') + "/api/v1/ARMPushToQueue";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pushToQueueURL);
            request.Method = "POST";
            request.ContentType = "application/json";

            string saveDetails = JsonConvert.SerializeObject(new
            {
                queuename = "AxConvertXLtoJSONQueue",
                queuedata = queueData
            });

            bool isRMQExist = false;
            ASB.WebService asbWebService = new ASB.WebService();
            string rmqstatus = asbWebService.CheckRabbitMqStatus("AxConvertXLtoJSONQueue");
            if (rmqstatus == "OK")
            {
                string axrapidimportsave = asbWebService.CheckRabbitMqStatus("axrapidimportsave");
                if (axrapidimportsave == "OK")
                    isRMQExist = true;
                else if (axrapidimportsave != "Queue does not exist or queue name incorrect")
                    return "RMQError:" + axrapidimportsave;
            }
            else if (rmqstatus != "Queue does not exist or queue name incorrect")
                return "RMQError:" + rmqstatus;

            request.ContentLength = saveDetails.Length;

            using (StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII))
            {
                requestWriter.Write(saveDetails);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();

                if (!isRMQExist)
                {
                    string _rmqstatus = asbWebService.CheckRabbitMqStatus("AxConvertXLtoJSONQueue");
                    if (_rmqstatus != "OK")
                        return "RMQError:" + _rmqstatus;
                    else
                    {
                        int retries = 5;
                        int delayMs = 2000;
                        string rapidImportStatus = "";
                        for (int i = 0; i < retries; i++)
                        {
                            rapidImportStatus = asbWebService.CheckRabbitMqStatus("axrapidimportsave");
                            if (rapidImportStatus == "OK")
                                return response;
                            Thread.Sleep(delayMs);
                        }
                        return "RMQError:" + rapidImportStatus;
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(ex.Message);
                return "Error:" + ex.Message;
            }
        }
        catch (Exception ex)
        {
            response = "Error:" + ex.Message;
        }
        return response;
    }

    [WebMethod]
    public static string GetFieldsNew(string transid, string _selectedDcs)
    {
        Util.Util util = new Util.Util();
        TStructDef strObj = util.GetTstructDefObj("Get structure for", transid.Trim());
        List<string> myCollection = new List<string>();

        var selectedDcNumbers = string.IsNullOrWhiteSpace(_selectedDcs) ? new int[0] : _selectedDcs.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x.Trim())).ToArray();

        int oldDcNo = 0;
        StringBuilder strDcFlds = new StringBuilder();

        Dictionary<string, string> multiselect = new Dictionary<string, string>();
        StringBuilder visibleTstFlds = new StringBuilder();
        StringBuilder visibleTstFldsGridDc = new StringBuilder();
        StringBuilder visibleTstFldsNonGriddc = new StringBuilder();
        string gridDc = "false";
        for (int i = 0; i < strObj.flds.Count; i++)
        {
            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)strObj.flds[i];

            if (fld.fldframeno != oldDcNo && selectedDcNumbers.Contains(fld.fldframeno))
            {
                TStructDef.DcStruct dc = (TStructDef.DcStruct)strObj.dcs[fld.fldframeno - 1];
                if (dc.isgrid)
                {
                    gridDc = "true";
                    // visibleTstFldsGridDc.Append(dc.dcPList + "♠").ToString();
                    if (fld.savevalue && !fld.name.StartsWith("axp_recid") && fld.datatype.ToLower() != "image" && !fld.visibility && fld.moe != "AutoGenerate")
                    {
                        visibleTstFldsGridDc.Append(fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")" + "♠" + fld.fldframeno + ",");
                    }
                }
                else
                {
                    //visibleTstFlds.Append(dc.name + "false" + "♠");
                    if (fld.savevalue && !fld.name.StartsWith("axp_recid") && fld.datatype.ToLower() != "image" && !fld.visibility)//&& fld.moe != "AutoGenerate"
                    {
                        visibleTstFldsNonGriddc.Append(fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")" + "♠" + fld.fldframeno + ",");
                        multiselect.Add(fld.name + "&&" + fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")", fld.caption + "(" + fld.name + ")");
                    }

                }
            }
            if (selectedDcNumbers.Contains(fld.fldframeno))
            {
                if (fld.savevalue && !fld.name.StartsWith("axp_recid") && fld.datatype.ToLower() != "image" && !fld.visibility)//&& fld.moe != "AutoGenerate"
                {
                    visibleTstFlds.Append(fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")" + ",");
                }
            }
        }

        visibleTstFlds.Append("♠" + gridDc);
        var response = new
        {
            // Multiselect = multiselect,
            VisibleTstFlds = visibleTstFlds,
            visibleTstFldsGridDc = visibleTstFldsGridDc,
            visibleTstFldsNonGriddc = visibleTstFldsNonGriddc
        };

        return JsonConvert.SerializeObject(response);
        //return visibleTstFlds.ToString();
    }
}

