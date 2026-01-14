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
            if (!IsPostBack)
            {
                CheckDesignAccess();
                ViewState["ExImpTStructs"] = GetAllTStructs();
                CreateExportImportPanel();

                //if url contains transid value then select that tstruct in the tstruct dropdown & get all fields
                string transid = Request.QueryString["transid"];
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
    private void CreateExportImportPanel()
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
                ListItem lst = new ListItem();
                lst.Text = tstructCaption;
                lst.Value = tstructName;
                ddlImpTbl.Items.Add(lst);
            }
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

                    var colName = hdnColNames.Value.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    var colValue = hdnColValues.Value.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    for (int j = 0; j < colName.Count(); j++)
                    {
                        ddlColumnName.Items.Add(new ListItem(colName[j], colValue[j]));
                    }
                    string sFileName = upFileName.Value;
                    //if ((sFileName).IndexOf("xlsx") > -1)
                    //    ChkColNameInfile.Checked = true;
                    //string sFileName = upFileName.Value;

                    if (sFileName != string.Empty)
                    {
                        if ((sFileName).IndexOf("xlsx") > -1)
                            headerList.Clear();
                        headerList.AddRange(colValue);
                    }
                    if (ChkColNameInfile.Checked)
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

            // for (int i = 0; i < captionHeader.Length; i++)
            // {
            //      if (captionHeader[i].IndexOf("*") != -1)
            //     {
            //         captionHeader[i] = captionHeader[i].Replace("*", "");
            //     }
            //  }
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
                    //hdnHeadersWithTypes.Value = hdnHeadersWithTypes.Value.Replace("#", ",");
                    //hdnHeadersWithTypes.Value = hdnHeadersWithTypes.Value.Substring(0, hdnHeadersWithTypes.Value.Length - 1);
                    //string[] stringArray = hdnHeadersWithTypes.Value.Split('♠');

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
            //using (ExcelEngine excelEngine = new ExcelEngine())
            //{
            //    IApplication application = excelEngine.Excel;
            //   // application.DefaultVersion = ExcelVersion.Excel2016;

            //    //Reads input Excel stream as a workbook
            //    IWorkbook workbook = application.Workbooks.Open(File.OpenRead(Path.GetFullPath(filePath)));
            //    IWorksheet sheet = workbook.Worksheets[0];
            //    sheet.InsertRow(0, 1, ExcelInsertOptions.FormatAsBefore);
            //    sheet.ImportArray(cols.ToArray(), 0, 1, false);
            //}
            string FilenameEncrpt = filename;
            if (filename.IndexOf("&") > -1)
            {
                FilenameEncrpt = filename.Replace('&', '♠');
            }
            // hdnTemplateName.Value = filename;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Generate Template File", "window.open(\"openfile.aspx?fpath=" + FilenameEncrpt + "&Imp=t\", \"_self\");", true);

            // if (IsFileUploaded.Value == "1")
            //  btnColHeader_Click();
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
                dt = exceldt(sFileDir + sFileName);
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
            if (ChkColNameInfile.Checked.ToString().ToLower() == "true" || dt.Columns.Count == selectedFieldCount)
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

                    string[] array1 = hdnColValues.Value.Split(',');
                    string _output = Regex.Replace(excelColNames, @"\([^\)]*\)", "");
                    string[] array2 = _output.Split(',');
                    var matchedIndices = array2.Select(item => Array.IndexOf(array1, item)).Where(index => index != -1).ToArray();
                    if (ChkColNameInfile.Checked.ToString().ToLower() == "true" && matchedIndices.Length > 0)
                    {
                        hdnColNames.Value = excelColNames;
                        string output = Regex.Replace(excelColNames, @"\([^\)]*\)", "");
                        hdnColValues.Value = output;
                    }

                    string[] tstfld = new string[] { };
                    string[] colName = new string[] { };
                    colName = hdnColNames.Value.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    String tstructFldCaption = "";
                    for (int i = 0; i < colName.Length; i++)
                    {
                        //if (colName[i].IndexOf("*") != -1)
                        //{
                        //    tstructFldCaption += colName[i].Replace("*", "") + ",";
                        //}
                        //else
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

    //Once file has been uploaded using Generic Handler FileUploadHandler.ashx then it will come here to validate the file Rows & Columns       
    protected void UploadButton_Click(object sender, EventArgs e)
    {

        string checkHeader = hdnCheckHeader.Value;
        chkForIgnoreErr.Checked = true;
        string sid = Session["nsessionid"].ToString();
        string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        string sFileName = upFileName.Value;
        string sFileDir = ScriptsPath + "Axpert\\" + sid + "\\";
        if ((sFileName).IndexOf("xlsx") > -1)/*|| (sFileName).IndexOf("xls") > -1)*/
        {
            ddlSeparator.Visible = false;
            dt = exceldt(sFileDir + sFileName);
        }
        else
            dt = ReadCsvFile(sFileDir + sFileName);
        if (fileReadSuccess)
        {
            if ((dt.Columns.Count == 0 && dt.Rows.Count == 0) || (ChkColNameInfile.Checked && dt.Rows.Count == 0))
            {
                IsFileUploaded.Value = "";
                ScriptManager.RegisterStartupScript(updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning', 4036, 'client');uploadFileClickEvent(); uploadFileChangeEvent();$('#divProgress').hide();", true);
                hdnUploadFileWarnings.Value = "Empty";
                return;
            }
            if (ChkColNameInfile.Checked)
            {
                DataRow drr = dt.Rows[0];
                if ((sFileName).IndexOf("xlsx") == -1)
                {
                    dt.Rows.Remove(drr);
                    //  headerList.AddRange(dt.Rows[0].ItemArray);
                }
            }
            if (dt.Rows.Count > 0)
                dt = dt.Rows.Cast<DataRow>().Take(5).CopyToDataTable(); //to display only top 5 records in the Grid
            int selectedFieldCount = hdnColNames.Value.Split(',').Length;
            //allowing user to go to next step only if selected no of columns is equal to total no of columns in uploaded csv file              
            if (ChkColNameInfile.Checked || (!ChkColNameInfile.Checked && dt.Columns.Count == selectedFieldCount))//&& checkHeader!="true"
            {
                if (dt.Rows.Count > 0)
                {
                    // if ((sFileName).IndexOf("xlsx") > -1)
                    headerList.AddRange(dt.Columns.Cast<DataColumn>()
                             .Select(x => x.ColumnName)
                             .ToArray());
                    var row = dt.NewRow();
                    //row[0] = ((DataRow)headerList);
                    // DataRow drr1 = headerList;

                    IsFileUploaded.Value = "1";
                    hdnUploadFileWarnings.Value = "Success";
                    ScriptManager.RegisterStartupScript(updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "fileUploadSuccess();", true);
                    gridImpData.DataSource = dt;
                    gridImpData.DataBind();
                }
                else
                {
                    IsFileUploaded.Value = "";
                    hdnUploadFileWarnings.Value = "Empty";
                    ScriptManager.RegisterStartupScript(updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "showAlertDialog('warning', 4038, 'client');uploadFileClickEvent(); uploadFileChangeEvent();callParentNew('DoUtilitiesEvent(ImportData)','function');", true);
                }
            }
            else
            {
                IsFileUploaded.Value = "";
                string warningMsg = hdnUploadFileWarnings.Value == "NotEqualColumns" ? "showAlertDialog('error', 4039, 'client');" : "showAlertDialog('error', eval(callParent('lcm[310]')));";
                ScriptManager.RegisterStartupScript(updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", "" + warningMsg + "$(callParentNew('btn-close','class')).click();callParentNew('DoUtilitiesEvent(ImportData)','function');uploadFileClickEvent(); uploadFileChangeEvent();$('#divProgress').hide();", true);
            }
        }
        else
        {
            IsFileUploaded.Value = "";
            string warningMsg = hdnUploadFileWarnings.Value == "NotEqualColumns" ? "showAlertDialog('error', 4039);" : "showAlertDialog('error', eval(callParent('lcm[310]')));";
            ScriptManager.RegisterStartupScript(updatePnl2, typeof(UpdatePanel), "uploadAlertSuccessMessage", warningMsg + "$(callParentNew('btn-close','class')).click();callParentNew('DoUtilitiesEvent(ImportData)','function');", true);
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (sid == "")
        {
            SessExpires();
            return;
        }
        string uploadedFileName = CheckSpecialChars(upFileName.Value.ToString());
        int uploadedColumns = colheader.Value.Split(',').Count();
        int ignoredColumns = hdnIgnoredColumns.Value == "" ? 0 : Convert.ToInt32(hdnIgnoredColumns.Value.Split(',').Count());
        if (ignoredColumns != 0) //if any columns is ignored after field selection(either by uploaded file or by select column option) then create a new file with selected columns
        {
            string fname = DateTime.Now.ToString("ddMMyyymmssfffff") + uploadedFileName.Substring(uploadedFileName.LastIndexOf("."));
            string path = util.ScriptsPath + "Axpert\\" + sid;
            string oldFileName = path + "\\" + uploadedFileName;
            string newFileName = path + "\\" + fname;
            CreateCsvFileForSelectedColumns(oldFileName, newFileName);
            upFileName.Value = fname;
        }

        lblTest.Text = string.Empty;
        SummaryDwnld.Visible = false;
        string dirPath = util.ScriptsPath + "axpert\\" + sid;
        DirectoryInfo di = new DirectoryInfo(dirPath);
        //' Determine whether the directory exists.
        if (!di.Exists)
            di.Create();

        string qcsv = string.Empty;
        qcsv = "no";
        string ignoreErrors = string.Empty;
        ignoreErrors = "yes";
        if (chkForIgnoreErr.Checked)
            ignoreErrors = "no";

        string strMapFields = string.Empty, transId;
        string selText = ddlImpTbl.SelectedItem.Text;
        transId = ddlImpTbl.SelectedItem.Value;
        string strMapFieldsSelectedFiledNames = string.Empty;
        strMapFieldsSelectedFiledNames = colheader.Value;
        string errorLog = logobj.CreateLog("Call ImportData.", Session["nsessionid"].ToString(), "ImportData-" + selText + "", "new");
        string changedFilename = "";
        string changedOrignalFilename = "";
        if (upFileName.Value.Contains(".xlsx"))
            changedFilename = upFileName.Value.Remove(upFileName.Value.Length - 5);
        else
            changedFilename = upFileName.Value.Remove(upFileName.Value.Length - 4);
        string fileType = upFileName.Value.Substring(upFileName.Value.LastIndexOf("."));
        if (upFileName.Value.Contains(".xlsx"))
            changedOrignalFilename = hdnOriginalfileName.Value.Remove(hdnOriginalfileName.Value.Length - 5);
        else
            changedOrignalFilename = hdnOriginalfileName.Value.Remove(hdnOriginalfileName.Value.Length - 4);
        StringBuilder ipXml = new StringBuilder();
        ipXml.Append("<testroot sessionid='" + sid + "' axpapp='" + proj + "' transid='" + transId + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' type='" + fileType + "' ");
        ipXml.Append("trace='" + errorLog + "' scriptpath='" + util.ScriptsPath + "' filename='" + CheckSpecialChars(changedFilename + fileType) + "' sep='" + ddlSeparator.SelectedValue + "' ");
        if (hdnCheckToggleHeader.Value != null && hdnCheckToggleHeader.Value != "")
            ChkColNameInfile.Checked = Convert.ToBoolean(hdnCheckToggleHeader.Value);
        if (ChkColNameInfile.Checked)
        {
            ipXml.Append(" hline='hline' ");
            //colNameInFile = "true";
        }
        //group by column
        if (hdnGroupBy.Value != "NA")
            ipXml.Append(" group= '" + hdnGroupBy.Value + "' ");

        ipXml.Append(" validatedata = '" + ignoreErrors + "' ");
        ipXml.Append("qcsv='" + qcsv + "' mapinfile='" + ChkColNameInfile.Checked.ToString().ToLower() + "'>");
        ipXml.Append("<map>" + CheckSpecialChars(strMapFieldsSelectedFiledNames) + "</map>");
        string primaryKey = "";
        if (hdnPrimaryKey.Value == "" && hdnupdateField.Value != "")
        {
            primaryKey = hdnupdateField.Value.Split('(')[1].Substring(0, hdnupdateField.Value.Split('(')[1].Length - 1);
        }
        else
        {
            primaryKey = hdnPrimaryKey.Value;
        }
        if (chkForAllowUpdate.Checked || hdnupdateField.Value != "")
            ipXml.Append("<primaryfield>" + primaryKey + "</primaryfield >");

        try
        {
            if (Session["AxRulesImportXML"] != null && Session["AxRulesImportXML"].ToString() != "")
                ipXml.Append(Session["AxRulesImportXML"].ToString());
        }
        catch (Exception ex) { }

        ipXml.Append(Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</testroot>");

        string res = string.Empty;
        try
        {
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            string axpStructKey = Constants.AXCONFIGTSTRUCT;
            axpConfigStr = fObj.DataTableFromRedis(util.GetConfigCacheKey(axpStructKey, transId, "", Session["AxRole"].ToString(), "ALL"));
            int webserviceTimeout = Convert.ToInt32(axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "webservice timeout").Select(x => x.Field<string>("PROPSVAL")).FirstOrDefault());
            if (webserviceTimeout != 0)
                res = objWebServiceExt.CallImportData(ipXml.ToString(), webserviceTimeout);
            else
                res = objWebServiceExt.CallImportData(ipXml.ToString(), objWebServiceExt.asbUtils.Timeout);
            ParseResult(res, "import");
            hdnIgnoredColCount.Value = "0";
            hdnIgnoredColumns.Value = "";
        }
        catch (Exception ex)
        {
            Response.Redirect(util.errorString);
            lblTest.Text = "Error while importing.Please try again.";
            logobj.CreateLog(ex.Message, Session["nsessionid"].ToString(), "Import Data - Import", "", "true");
        }
        fileUploadComplete.Value = "1";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowDimmerClose", "callParentNew('closeFrame(); ','function');", true);
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

                                string[] array1 = hdnColValues.Value.Split(',');
                                string _output = Regex.Replace(line, @"\([^\)]*\)", "");
                                string[] array2 = _output.Split(',');
                                var matchedIndices = array2.Select(item => Array.IndexOf(array1, item)).Where(index => index != -1).ToArray();
                                if (ChkColNameInfile.Checked.ToString().ToLower() == "true" && matchedIndices.Length > 0)
                                {
                                    hdnColNames.Value = line;
                                    string output = Regex.Replace(line, @"\([^\)]*\)", "");
                                    hdnColValues.Value = output;
                                }

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
                                    //foreach (var item in mandatoryCols[0].Split(','))
                                    //    mandatoryFlds.Add(item);
                                    //foreach (var item in mandatoryCols[1].Split(','))
                                    //    mandatoryCap.Add(item);
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

    //If Tstruct selection has changed then get all fields & bind it to the Multiselect control using ASP.NET Repeator control    
    private void GetFields(string transid)
    {
        ddlGroupBy.Enabled = false;
        bool isTstructConDC = false;
        hdnGroupByColVal.Value = hdnGroupByColName.Value = "";
        ArrayList ddlFilterOpts = new ArrayList();
        dataupdatecheck.Checked = false;
        Session.Remove("AxRulesImportXML");

        //take the old transid session & update session id with new transid once get the Tstruct def re-assign the old session value
        var temp = Session["transid"];
        Session["transid"] = transid;
        TStructDef strObj = util.GetTstructDefObj("Get structure for", transid.Trim());
        Session["transid"] = temp;
        List<string> myCollection = new List<string>();
        // string combinedString1 = "";
        int oldDcNo = 0;
        strAutoComArray.Append("var arrItems = [");
        dictFlds = new Dictionary<string, string>();
        StringBuilder strDcFlds = new StringBuilder();
        StringBuilder visibleTstFlds = new StringBuilder();
        if (strObj != null)
        {
            GetAxRulesDef(strObj, transid);
        }
        for (int i = 0; i < strObj.flds.Count; i++)
        {
            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)strObj.flds[i];

            if (fld.fldframeno != oldDcNo)
            {
                TStructDef.DcStruct dc = (TStructDef.DcStruct)strObj.dcs[fld.fldframeno - 1];
                arrMapFlds.Add(dc.caption + " DC");
                arrDcData.Add(dc.caption + " DC");
                strAutoComArray.Append("\"" + dc.caption + " DC" + "\",");
                arrMapDcNos.Add(dc.frameno);
                arrAllDcsData.Add(dc.caption + " DC");
                arrMapIsDc.Add("T");
                if (dc.isgrid)
                    arrGridDcs.Add(dc.frameno);

                if (strDcFlds.ToString() != string.Empty)
                {
                    dictFlds.Add(oldDcNo.ToString(), strDcFlds.ToString().Substring(0, strDcFlds.ToString().Length - 1));
                    strDcFlds.Length = 0;
                }
                oldDcNo = dc.frameno;

                //enable group by option only if the Tstruct contains DC as Grid otherwise it will be disabled
                if (dc.isgrid && !isTstructConDC)
                {
                    isTstructConDC = true;
                    ddlGroupBy.Enabled = true;
                }
            }
            //string multiSelectfld = "";
            //if (fld.fldMultiSelect != null)
            //{
            //    if (fld.fldMultiSelectSep != "")
            //    {
            //        if (fld.fldMultiSelectSep != ",")
            //            multiSelectfld = fld.name + "~true~" + fld.fldMultiSelectSep;
            //        else
            //            multiSelectfld = fld.name + "~true~" + "@";
            //    }
            //}

            if (fld.savevalue && !fld.name.StartsWith("axp_recid") && fld.datatype.ToLower() != "image" && !fld.visibility && fld.moe != "AutoGenerate")
            {
                //if (fld.fldMultiSelect == null) {
                //    myCollection.Add(fld.name);
                //} else
                //    myCollection.Add(multiSelectfld);
                // combinedString1 = string.Join(",", myCollection.ToArray());
                // hdnCOLheaders.Value = combinedString1;
                arrMapFlds.Add(fld.caption + " (" + fld.name + ")");
                strAutoComArray.Append("\"" + fld.caption + " (" + fld.name + ")" + "\",");
                arrMapDcNos.Add(fld.fldframeno);
                //string datatype = fld.datatype;
                //headersWithTypes.Append(fld.name + "#" + datatype + "♠");
                //hdnHeadersWithTypes.Value = headersWithTypes.ToString();
                arrMapIsDc.Add("F");
                strDcFlds.Append(fld.caption + " (" + fld.name + ")" + ",");
                visibleTstFlds.Append(fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")" + ",");
                multiselect.Add(fld.name + "&&" + fld.caption + (fld.allowempty ? "" : "*") + "(" + fld.name + ")", fld.caption + "(" + fld.name + ")");
                ddlFilterOpts.Add(fld.caption + " (" + fld.name + ")");
                hdnTstructflds.Value = visibleTstFlds.ToString();
            }

            if (fld.fldframeno == 1 && fld.savevalue && !fld.visibility && fld.datatype.ToLower() != "image")
            {
                hdnGroupByColName.Value += fld.caption + ", ";
                hdnGroupByColVal.Value += fld.name + ", ";
            }
        }
        if (hdnGroupByColName.Value.Trim() != string.Empty)
        {
            hdnGroupByColVal.Value = hdnGroupByColVal.Value.Substring(0, hdnGroupByColVal.Value.Length - 2);
            hdnGroupByColName.Value = hdnGroupByColName.Value.Substring(0, hdnGroupByColName.Value.Length - 2);
        }

        if (strDcFlds.ToString() != string.Empty)
        {
            dictFlds.Add(oldDcNo.ToString(), strDcFlds.ToString().Substring(0, strDcFlds.ToString().Length - 1));
            strDcFlds.Length = 0;
            hdnLeftFlds.Value = visibleTstFlds.ToString().Substring(0, visibleTstFlds.ToString().Length - 1);
            visibleTstFlds.Length = 0;
        }
        strAutoComArray.Append("];");
        ViewState["arrDcData"] = arrDcData;
        ViewState["arrMapDcNos"] = arrMapDcNos;
        ViewState["dictFlds"] = dictFlds;
        ViewState["transid"] = transid;

        if (multiselect != null && multiselect.Count > 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "UpdateMandatoryFields", "updateMandatoryFieldsToSelection();", true);
            rptSelectFields.DataSource = multiselect.Keys;
            rptSelectFields.DataBind();
            rptTemplateFields.DataSource = "";
            rptTemplateFields.DataBind();
        }
        else
        {
            rptSelectFields.DataSource = "";
            rptSelectFields.DataBind();
            rptTemplateFields.DataSource = "";
            rptTemplateFields.DataBind();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertNoImportTstruct", "showAlertDialog(\"warning\",eval(callParent('lcm[260]')));", true);
        }
        if (axRuleDetails.Value != null && axRuleDetails.Value != "")
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "checkRules", "CheckAxRulesForImport();", true);
        }
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
                            //failedSummaryFileName = ddlImpTbl.SelectedItem.Text;
                            ////to replace special characters from the file name
                            //foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                            //    failedSummaryFileName = failedSummaryFileName.Replace(c, '_');

                            ////to replace space characters from the file name with underscore 
                            //foreach (char c in failedSummaryFileName.ToCharArray())
                            //    if (c == 32)
                            //        failedSummaryFileName = failedSummaryFileName.Replace(c, '_');

                            //File.WriteAllText(saveFailure + "\\" + failedSummaryFileName + "_Failed_Summary.csv", txttocsv.ToString()); //generate Failed summary report with [TstructName]_Failed_Summary.csv

                            //SummaryDwnld.Visible = true;
                            //SummaryDwnld.HRef = "openfile.aspx?fpath=" + failedSummaryFileName + "_Failed_Summary.csv&Imp=t";

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

    public DataTable exceldt(string filePath)
    {
        string sSheetName = null;
        string sConnection = null;
        DataTable dtTablesList = default(DataTable);
        OleDbCommand oleExcelCommand = default(OleDbCommand);
        OleDbDataReader oleExcelReader = default(OleDbDataReader);
        OleDbConnection oleExcelConnection = default(OleDbConnection);
        DataTable dt = new DataTable();

        // sConnection = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = '" + filePath + "';Extended Properties=\"Excel 8.0;HDR=YES;\"";        
        // sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0 xml; HDR=YES\""; 
        //if (!ChkColNameInfile.Checked)
        //    sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0; HDR=NO;IMEX=1\"";
        //else
        sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0; HDR=NO;IMEX=1\"";
        oleExcelConnection = new OleDbConnection(sConnection);
        oleExcelConnection.Open();

        dtTablesList = oleExcelConnection.GetSchema("Tables");

        if (dtTablesList.Rows.Count > 0)
        {
            sSheetName = dtTablesList.Rows[0]["TABLE_NAME"].ToString();
        }

        dtTablesList.Clear();
        dtTablesList.Dispose();

        if (!string.IsNullOrEmpty(sSheetName))
        {
            oleExcelCommand = oleExcelConnection.CreateCommand();
            oleExcelCommand.CommandText = "Select * From [" + sSheetName + "]";
            oleExcelCommand.CommandType = CommandType.Text;
            oleExcelReader = oleExcelCommand.ExecuteReader();

            dt.Load(oleExcelReader);
            //ChkColNameInfile.Checked = false;
            fileReadSuccess = true;
            oleExcelReader.Close();
        }
        oleExcelConnection.Close();
        return dt;
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
                Importnew.ddlSeparator.Visible = false;
                Importnew.dt = Importnew.exceldt(sFileDir + sFileName);
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
}
