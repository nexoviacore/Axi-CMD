using BotDetect.Drawing;
using CacheMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
public partial class aspx_AutoComplete : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    static LogFile.Log logobj = new LogFile.Log();
    string fldname = string.Empty; string tid = string.Empty; string activeRow = string.Empty; string frameNo = string.Empty;
    string tstKey = string.Empty; string parStr = string.Empty; string subStr = string.Empty; string idcol = string.Empty; string name = string.Empty;
    string srchTxt = string.Empty; string fieldnm = string.Empty; string pdc = string.Empty; string prow = string.Empty;
    string _fldValue = string.Empty;
    public string _isMulSel = "false";
    int pageNo = 1;
    public string EnableOldTheme = "";
    ArrayList arrHiddenCols = new ArrayList();
    ArrayList srchColsDataType = new ArrayList();
    ArrayList arrFldCapCols = new ArrayList();
    ArrayList selFildValues = new ArrayList();
    ArrayList arrHiddenFillCols = new ArrayList();
    TStructData tstData;
    public string direction = "ltr";
    public string langType = "en";
    public string _isFilterVal = "";
    public string _isPageInfo = "";
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
        if (Session["AxEnableOldTheme"] != null)
            EnableOldTheme = Session["AxEnableOldTheme"].ToString();
        //For cloud applications
        util.IsValidSession();
        ResetSessionTime();
        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }
        else
        {
            if (!IsPostBack)
            {
                if (util.IsValidQueryString(Request.RawUrl) == false)
                    HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.INVALIDURL);
                GetQueryStringVals();

                tstData = (TStructData)Session[tstKey];
                int idx = tstData.tstStrObj.GetFieldIndex(fieldnm);
                if (idx != -1)
                {
                    TStructDef.FieldStruct fld = (TStructDef.FieldStruct)tstData.tstStrObj.flds[idx];
                    divTitle.InnerText = fld.caption;
                }
                else
                    divTitle.InnerText = fieldnm;

                BindSearchFlds(idx);
                CallWebservice();
                fname.Value = fldname;
            }
            else
                SetQueryStringVals();
        }
        searchlist.Attributes.Add("onchange", "SetSelectedValue(this,'" + searchlistval.ClientID + "');");
    }

    private void SetQueryStringVals()
    {
        tid = ViewState["tid"].ToString();
        tstKey = ViewState["key"].ToString();
        activeRow = ViewState["activeRow"].ToString();
        fieldnm = ViewState["field"].ToString();
        frameNo = ViewState["frameno"].ToString();
        parStr = ViewState["parStr"].ToString();
        subStr = ViewState["subStr"].ToString();

        int idx = -1;
        idx = parStr.IndexOf("~");
        if ((!string.IsNullOrEmpty(parStr) & idx != -1))
        {
            pdc = parStr.Substring(0, idx);
            prow = parStr.Substring(idx + 1);
        }
    }

    private void BindSearchFlds(int fldIdx)
    {
        ArrayList srchCols = new ArrayList();

        if (Request.QueryString["isFldddl"] != null && Request.QueryString["isFldddl"].ToString() == "true")
        {
            if (fldIdx != -1)
            {
                TStructDef.FieldStruct fld = (TStructDef.FieldStruct)tstData.tstStrObj.flds[fldIdx];
                string srFldDataType = fld.datatype;
                ddlSearchFld.Items.Add(fieldnm);
                ddlDataType.Items.Add(srFldDataType);
            }
        }
        else
        {
            srchCols = GetSearchCols(fieldnm, fldIdx);
            int k = 0;

            for (k = 0; k < srchCols.Count; k++)
            {
                if ((!string.IsNullOrEmpty(srchCols[k].ToString())))
                {
                    ddlSearchFld.Items.Add(srchCols[k].ToString());
                    if (srchColsDataType[k].ToString() == "")
                        UpdateFieldDataType(srchCols[k].ToString(), k);

                    ddlDataType.Items.Add(srchColsDataType[k].ToString());
                }
            }
        }
        ddlSearchFld.SelectedIndex = 0;
        if (ddlSearchFld.Items.Count <= 1)
            ddlSearchFld.Enabled = false;
        else
            ddlSearchFld.Enabled = true;
    }

    private void UpdateFieldDataType(string flName, int fldIndex)
    {
        TStructData tstDataNew = (TStructData)Session[tstKey];
        int idx = tstDataNew.tstStrObj.GetFieldIndex(flName);
        if (idx != -1)
        {
            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)tstData.tstStrObj.flds[idx];
            srchColsDataType[fldIndex] = fld.datatype;
        }
    }

    private ArrayList GetSearchCols(string field, int idx)
    {

        if (idx != -1)
        {
            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)tstData.tstStrObj.flds[idx];
            srchColsDataType = fld.AdvSrchFldsType;
            return fld.AdvSrchFlds;
        }

        XmlDocument xmlDoc = new XmlDocument();
        ArrayList a26Array = new ArrayList();
        ArrayList a63Array = new ArrayList();

        ArrayList a26ArrayType = new ArrayList();
        ArrayList a63ArrayType = new ArrayList();

        string fldCaption = string.Empty;
        string dataType = string.Empty;
        string fldName = string.Empty;
        xmlDoc.LoadXml(tstData.tstStrObj.structRes);
        XmlNodeList tstBaseDataNodes = default(XmlNodeList);
        tstBaseDataNodes = xmlDoc.SelectNodes("//root");

        foreach (XmlNode tstBaseDataNode in tstBaseDataNodes)
        {
            XmlNodeList tstChildDataNodes = null;
            tstChildDataNodes = tstBaseDataNode.ChildNodes;

            foreach (XmlNode tstChildDataNode in tstChildDataNodes)
            {
                if ((tstChildDataNode.Attributes["cat"] != null))
                {
                    if (tstChildDataNode.Attributes["cat"].Value.ToString() == "field")
                    {
                        foreach (XmlNode fldNode in tstChildDataNode)
                        {
                            if (fldNode.Name == "a2")
                            {
                                fldCaption = fldNode.InnerText;
                            }
                            if (fldNode.Name == "a3")
                            {
                                dataType = fldNode.InnerText;
                            }
                            else if (fldNode.Name == "a1")
                            {
                                fldName = fldNode.InnerText;
                            }
                            else if ((fldNode.Name == "a7"))
                            {
                                foreach (XmlNode fldDetail in fldNode)
                                {
                                    if (fldDetail.Name == "a26")
                                    {
                                        if ((fldDetail.Attributes["stype"] != null))
                                        {
                                            if (fldDetail.Attributes["stype"].Value == "tstruct" | fldDetail.Attributes["stype"].Value == "sql")
                                            {
                                                if (fldName == field)
                                                {
                                                    a26Array.Add(fldCaption);
                                                    a26ArrayType.Add(dataType);
                                                }
                                            }
                                        }

                                    }
                                    else if (fldDetail.Name == "a63")
                                    {
                                        foreach (XmlNode cnNode in fldDetail)
                                        {
                                            if ((cnNode.Attributes["cn"] != null))
                                            {
                                                if (fldName == field)
                                                {
                                                    a63Array.Add(cnNode.Attributes["cn"].Value);
                                                    a63ArrayType.Add("");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if ((a63Array.Count == 0))
        {
            srchColsDataType = a26ArrayType;
            return a26Array;
        }
        else
        {
            srchColsDataType = a63ArrayType;
            return a63Array;
        }
    }

    public void btnGo_Click(object sender, EventArgs e)
    {
        txtSrchText.Text = "";
        CallWebservice();
    }

    public void btnClear_Click(object sender, EventArgs e)
    {
        ddlSearchFld.SelectedIndex = 0;
        ddlCondition.SelectedIndex = 0;
        txtfilter.Text = "";
        txtfilter1.Text = "";
        txtSrchText.Text = "";
        pageNo = 1;
        CallWebservice();
    }

    public void btnPriv_Click(object sender, EventArgs e)
    {
        txtSrchText.Text = "";
        if (int.Parse(ViewState["pageno"].ToString()) > 1)
            pageNo = int.Parse(ViewState["pageno"].ToString()) - 1;
        CallWebservice();
    }

    public void btnNext_Click(object sender, EventArgs e)
    {
        txtSrchText.Text = "";
        pageNo = int.Parse(ViewState["pageno"].ToString()) + 1;
        CallWebservice();
    }

    private void CallWebservice()
    {
        //string srchText = txtfilter.Text.Trim();
        //string srchTextBetween = txtfilter1.Text.Trim();
        string srchText = txtfilter.Text;
        _isFilterVal = srchText;
        string srchTextBetween = txtfilter1.Text;
        string condition = ddlCondition.SelectedValue;
        ViewState["pageno"] = pageNo;
        string result = string.Empty;
        try
        {
            TStructData tstData = (TStructData)Session[tstKey];
            if (tstData == null)
                result = "";
            else
            {
                ArrayList dArr = new ArrayList();
                string errorLog = logobj.CreateLog("Calling Webservice", Session["nsessionid"].ToString(), "FastGetFieldChoices" + tid, "new");
                string sqlcolumn = "false";
                if (Session["showSqlColumnsAdvSearch"] != null && Session["showSqlColumnsAdvSearch"].ToString() != "" && Session["showSqlColumnsAdvSearch"].ToString() == "All")
                    sqlcolumn = "true";
                else if (Session["showSqlColumnsAdvSearch"] != null && Session["showSqlColumnsAdvSearch"].ToString() != "" && Session["showSqlColumnsAdvSearch"].ToString() != "All")
                {
                    string _advSearchDevopt = Session["showSqlColumnsAdvSearch"].ToString();
                    string[] _ArrAdvSearchDevopt = _advSearchDevopt.Split(',');
                    var _lstArr = _ArrAdvSearchDevopt.AsEnumerable().Where(x => x == fieldnm).ToList();
                    if (_lstArr.Count > 0)
                        sqlcolumn = "true";
                    else
                        sqlcolumn = "false";
                }

                string iXml = "<sqlresultset sqlcolumn='" + sqlcolumn + "' axpapp='" + Session["project"] + "' cond='" + CheckSpecialChars(condition) + "' value='" + srchText + "' value1='" + srchTextBetween + "' sessionid='" + Session["nsessionid"] + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' field='" + fieldnm + "' sqlfield='" + ddlSearchFld.SelectedItem + "' activerow='" + activeRow + "' frameno='" + frameNo + "' pdc='" + pdc + "' prow='" + prow + "' trace='" + errorLog + "' transid='" + tid + "' pageno='" + pageNo + "' pagesize='100'>";
                //Changed from GetPicklistInputXml to GetFieldValueXml since all grid dependents were not being passed
                tstData.GetFieldValueXml(dArr, dArr, dArr, dArr, "-1", "LoadTab", "NG", tstData.tstStrObj.GetFieldDc(fieldnm).ToString());
                //tstData.GetPicklistInputXml(dArr, dArr, dArr, dArr, fieldnm, frameNo, parStr, subStr, activeRow);
                iXml += tstData.fieldValueXml + tstData.memVarsData + Session["axapps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString() + "</sqlresultset>";
                ASB.WebService objWeb = new ASB.WebService();
                TStructDef strObj = tstData.tstStrObj;
                string ires = strObj.structRes;
                result = objWeb.GetFieldChoices(tid, iXml, ires, tstKey);
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Autocomplete.aspx.cs, CallWebservice function-" + ex.Message, Session["nsessionid"].ToString(), "FastGetFieldChoices" + tid, "");
        }

        if (result != "")
            ParseResult(result);
        else
        {
            progressArea.Visible = true;
            dvSearch.Visible = false;
            dvFooter.Visible = false;
        }
    }

    private void ParseResult(string result)
    {
        try
        {
            string errMsg = string.Empty;
            errMsg = util.ParseXmlErrorNode(result);

            if (errMsg != string.Empty)
            {
                if (errMsg == Constants.SESSIONERROR)
                    SessionExpired();
                else
                    Response.Redirect(util.ERRPATH + errMsg);
            }

            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.LoadXml(result);

            XmlNodeList compNodes = default(XmlNodeList);
            compNodes = xmlDoc1.SelectNodes("//sqlresultset/response");
            int totalRows = 0;
            foreach (XmlNode cNode in compNodes)
            {
                //Getting total if pageno=1 else pick it from session                    
                XmlNode tnode = cNode.Attributes["totalrows"];
                if (tnode == null)
                {
                    totalRows = 0;
                }
                else
                {
                    if (pageNo == 1)
                    {
                        totalRows = Convert.ToInt32(tnode.Value);
                        Session["pl_noofpages"] = totalRows;
                    }
                    else
                    {
                        totalRows = Convert.ToInt32(Session["pl_noofpages"]);
                    }

                    cNode.Attributes.RemoveNamedItem("totalrows");
                    break; // TODO: might not be correct. Was : Exit For   
                }
            }

            BindDataGrid(result, totalRows, pageNo.ToString(), xmlDoc1);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Autocomplete.aspx.cs, ParseResult function-" + ex.Message, Session["nsessionid"].ToString(), "ParseResult", "new");
        }
    }

    private void BindDataGrid(string result, int totRows, string pageno, XmlDocument xmlDoc)
    {
        try
        {
            if (totRows == 0)
            {
                GridView1.Columns.Clear();
                GridView1.DataSource = new string[] { };
                GridView1.DataBind();
                progressArea.Visible = true;
                dvSearch.Visible = false;
                dvFooter.Visible = false;
            }
            else
            {
                GridView1.Columns.Clear();
                GridView1.DataSource = new string[] { };
                GridView1.DataBind();
                progressArea.Visible = false;
                XmlNode resultNode = default(XmlNode);
                resultNode = xmlDoc.GetElementsByTagName("sqlresultset")[0]; string map = "";
                if (((resultNode.Attributes["idcol"] != null)))
                    idcol = resultNode.Attributes["idcol"].Value;
                if (((resultNode.Attributes["map"] != null)))
                    map = resultNode.Attributes["map"].Value;
                if (((resultNode.Attributes["colmap"] != null)))
                    ViewState["colmap"] = resultNode.Attributes["colmap"].Value;
                else
                    ViewState["colmap"] = "";
                int _PageSize = 0;
                if (resultNode.Attributes["pagesize"] != null)
                    _PageSize = int.Parse(resultNode.Attributes["pagesize"].Value);

                searchlist.Items.Clear();
                searchlistval.Items.Clear();
                SetItemsInHdnFlds(result, totRows, pageno);

                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1.LoadXml(result);

                XmlNode cNode = default(XmlNode);
                cNode = xmlDoc1.SelectSingleNode("//response");

                XmlNodeList rowNodes = default(XmlNodeList);
                rowNodes = cNode.ChildNodes;
                int j = 0;
                foreach (XmlNode chNode in rowNodes)
                {
                    foreach (XmlNode chhNode in chNode)
                    {
                        if (chhNode.Attributes.Count > 0)
                        {
                            if ((j == 0 & (chhNode.Attributes["w"].Value == "0")))
                            {
                                arrHiddenCols.Add(chhNode.Name);
                            }
                            if (chhNode.Attributes["cap"] != null && j == 0)
                            {
                                arrFldCapCols.Add(chhNode.Attributes["cap"].Value);
                            }

                            if (chhNode.Attributes["hidden"] != null && j == 0)
                            {
                                arrHiddenFillCols.Add(chhNode.Attributes["hidden"].Value);
                            }
                            chhNode.Attributes.RemoveAll();
                        }
                    }
                    j = j + 1;
                }

                string totrow = cNode.Attributes["totalrows"].Value;
                if (pageNo == 1)
                    ViewState["totRows"] = totrow;
                if (pageNo > 1 && ViewState["totRows"].ToString() != "")
                    totrow = ViewState["totRows"].ToString();

                if (totrow != "0")
                {
                    cNode.Attributes.RemoveAll();

                    StringWriter sw = new StringWriter();
                    XmlTextWriter xw = new XmlTextWriter(sw);
                    cNode.WriteTo(xw);
                    string res = sw.ToString();
                    DataSet ds = new DataSet();
                    System.IO.StringReader sr = new System.IO.StringReader(res);
                    ds.ReadXml(sr);
                    int i = 1;
                    int colCount = 0;

                    //if (_isMulSel == "true")
                    //{
                    BoundField bfields = new BoundField();
                    bfields.DataField = "";
                    bfields.HeaderText = "mulChk";
                    GridView1.Columns.Add(bfields);
                    //}


                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        BoundField bfield = new BoundField();
                        //if (i == 0 && _isMulSel == "true")
                        //{
                        //    i++;
                        //}
                        //'initialiae the data field value
                        bfield.DataField = dc.ColumnName;
                        //'initialise the header text value
                        if (arrFldCapCols[colCount].ToString() == "")
                            bfield.HeaderText = dc.ColumnName;
                        else
                            bfield.HeaderText = arrFldCapCols[colCount].ToString();
                        //' add newly created columns to gridview
                        GridView1.Columns.Add(bfield);
                        colCount++;
                        if (arrHiddenCols.IndexOf(GridView1.Columns[i].HeaderText) != -1)
                        {
                            GridView1.Columns[i].Visible = false;
                        }
                    }

                    GridView1.DataSource = ds.Tables[0];
                    GridView1.DataBind();

                    int pagSize = _PageSize > 100 ? _PageSize : (_PageSize == 0 ? int.Parse(totrow) : 100);
                    int startPg = 1; int endPg = pagSize;
                    if (int.Parse(totrow) < pagSize) endPg = int.Parse(totrow);
                    if (pageNo > 1)
                    {
                        startPg = (pagSize * (pageNo - 1)) + 1;
                        if (int.Parse(totrow) < (pagSize * pageNo))
                            endPg = int.Parse(totrow);
                        else
                            endPg = (startPg + pagSize) - 1;
                    }
                    if (pageNo <= 1)
                    {
                        var strCssClass = btnPriv.Attributes["class"];
                        if (strCssClass.IndexOf("disabled") == -1)
                            btnPriv.Attributes["class"] = strCssClass + " disabled";
                    }
                    else
                    {
                        btnPriv.Attributes.Add("class", btnPriv.Attributes["class"].Replace("disabled", ""));
                    }
                    if ((pagSize * pageNo) >= int.Parse(totrow))
                    {
                        var strCssClass = btnNextclk.Attributes["class"];
                        if (strCssClass.IndexOf("disabled") == -1)
                            btnNextclk.Attributes["class"] = strCssClass + " disabled";
                    }
                    else
                    {
                        btnNextclk.Attributes.Add("class", btnNextclk.Attributes["class"].Replace("disabled", ""));
                    }
                    lblstart.Text = startPg.ToString();
                    lblend.Text = endPg.ToString();
                    lbltotrec.Text = totrow;
                    dvSearch.Visible = true;
                    dvFooter.Visible = true;
                    if (_isFilterVal == "")
                        _isPageInfo = _PageSize + "♠" + totrow + "♠" + startPg;
                    else
                        _isPageInfo = "";
                }
                else
                {
                    progressArea.Visible = true;
                    dvSearch.Visible = false;
                    dvFooter.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Autocomplete.aspx.cs, BindDataGrid function-" + ex.Message, Session["nsessionid"].ToString(), "BindDataGrid", "new");
        }
    }

    private void SetItemsInHdnFlds(string a, int totRows, string pageno)
    {
        try
        {
            DataSet ds = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(a);
            ds.ReadXml(sr);

            if ((ds.Tables.Count <= 1))
            {
                // Message();
            }
            else
            {
                string value = "";
                XmlDocument loadxmlDoc = new XmlDocument();
                XmlNodeList WrproductNodes = default(XmlNodeList);

                loadxmlDoc.LoadXml(a);

                WrproductNodes = loadxmlDoc.GetElementsByTagName("response");
                if ((WrproductNodes.Count == 0))
                {
                }
                else
                {
                    foreach (XmlNode WrproductNode in WrproductNodes)
                    {
                        if (((WrproductNode.Attributes["value"] != null)))
                        {
                            value = WrproductNode.Attributes["value"].Value;
                        }
                    }
                }

                string fields = "";
                XmlNodeList getnodes = default(XmlNodeList);
                getnodes = loadxmlDoc.GetElementsByTagName("sqlresultset");

                string map = "";

                foreach (XmlNode getnode in getnodes)
                {
                    if (((getnode.Attributes["idcol"] != null)))
                    {
                        idcol = getnode.Attributes["idcol"].Value;
                    }
                    if (((getnode.Attributes["map"] != null)))
                    {
                        map = getnode.Attributes["map"].Value;
                    }
                    if (((getnode.Attributes["colmap"] != null)))
                    {
                        ViewState["colmap"] = getnode.Attributes["colmap"].Value;
                    }
                    else
                    {
                        ViewState["colmap"] = "";
                    }
                }

                string mapstr = "";
                int j = 0;
                if ((!string.IsNullOrEmpty(map)))
                {
                    dynamic mapfield = map.Split(',');

                    XmlNodeList mapnodes = default(XmlNodeList);
                    XmlNodeList childmapnodes = default(XmlNodeList);
                    mapnodes = loadxmlDoc.GetElementsByTagName("row");
                    foreach (XmlNode mapnode in mapnodes)
                    {
                        childmapnodes = mapnode.ChildNodes;
                        foreach (XmlNode childmapnode in childmapnodes)
                        {
                            for (j = 0; j <= mapfield.Length - 1; j++)
                            {
                                dynamic mpfld = mapfield[j].ToString();
                                dynamic mpfldArr = mpfld.Split('=');
                                if (childmapnode.Name.ToString().ToLower() == mpfldArr[0].ToString().ToLower())
                                {
                                    mapstr += "~" + mpfldArr[1].ToString() + "***" + childmapnode.InnerText.ToString();
                                }
                            }
                        }
                        searchlistval.Items.Add(mapstr.ToString());
                        mapstr = "";
                    }
                }

                int wcount = 0;
                int i = 0;
                if ((value != "--"))
                {
                    XmlNodeList rownodes = default(XmlNodeList);
                    XmlNodeList childnodes = default(XmlNodeList);
                    rownodes = loadxmlDoc.GetElementsByTagName("row");
                    wcount = rownodes.Count;
                    foreach (XmlNode rownode in rownodes)
                    {
                        childnodes = rownode.ChildNodes;
                        foreach (XmlNode chilnode in childnodes)
                        {
                            i = i + 1;
                            if (((idcol == "yes")))
                            {
                                if ((i == 2))
                                {
                                    name = name + "¿" + chilnode.InnerText.ToString();
                                }
                                else if (i == 1)
                                {
                                    if (name == string.Empty)
                                    {
                                        name = chilnode.InnerText.ToString();
                                    }
                                    else
                                    {
                                        name += "~" + chilnode.InnerText.ToString();
                                    }
                                }
                            }
                            else
                            {
                                if ((i == 1))
                                {
                                    if (name == string.Empty)
                                    {
                                        name = chilnode.InnerText.ToString();
                                    }
                                    else
                                    {
                                        name += "~" + chilnode.InnerText.ToString();
                                    }
                                }
                            }
                        }
                        i = 0;
                    }

                    dynamic wfname = name.Split('~');
                    for (int m = 0; m <= wfname.Length - 1; m++)
                    {
                        searchlist.Visible = true;
                        searchlist.Items.Add(wfname[m].ToString());
                    }

                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Autocomplete.aspx.cs, SetItemsInHdnFlds function-" + ex.Message, Session["nsessionid"].ToString(), "SetItemsInHdnFlds", "new");
        }
    }

    private void GetQueryStringVals()
    {
        //"./srchComponent.aspx?search=" + x + "&fldname=" + fname1 + "&transid=" + tst + "&activeRow=" + AxActiveRowNo + "&frameno=" + fldFrameNo +
        //"&key=" + objKey + "&parStr=" + parStr + "&subStr=" + subStr;
        if (Request.QueryString["fldname"] != null)
            fldname = Request.QueryString["fldname"];
        if (Request.QueryString["transid"] != null)
        {
            tid = Request.QueryString["transid"];
            ViewState["tid"] = tid;
        }
        if (Request.QueryString["activeRow"] != null)
        {
            activeRow = Request.QueryString["activeRow"];
            ViewState["activeRow"] = activeRow;
        }
        if (Request.QueryString["frameno"] != null)
        {
            frameNo = Request.QueryString["frameno"];
            ViewState["frameno"] = frameNo;
        }
        if (Request.QueryString["key"] != null)
        {
            tstKey = Request.QueryString["key"];
            ViewState["key"] = tstKey;
        }
        if (Request.QueryString["parStr"] != null)
        {
            parStr = Request.QueryString["parStr"];
            ViewState["parStr"] = parStr;
        }
        if (Request.QueryString["subStr"] != null)
        {
            subStr = Request.QueryString["subStr"];
            ViewState["subStr"] = subStr;
        }
        if (Request.QueryString["srchTxt"] != null)
            srchTxt = Request.QueryString["srchTxt"];

        if (Request.QueryString["selFldVal"] != null)
            _fldValue = Request.QueryString["selFldVal"];
        if (_fldValue != string.Empty)
        {
            string[] strArrVal = _fldValue.Split('♠');
            foreach (string _val in strArrVal)
            {
                selFildValues.Add(_val);
            }
        }

        //if (Request.QueryString["isFldddl"] != null && Request.QueryString["isFldddl"].ToString() == "true")
        //{
        //    divFilter.Visible = false;
        //}

        if (Request.QueryString["isMultSel"] != null && Request.QueryString["isMultSel"].ToString() == "true")
        {
            _isMulSel = "true";
        }
        else
        {
            _isMulSel = "false";
        }



        if (fldname.LastIndexOf("F") != -1)
        {
            fieldnm = fldname.Substring(0, fldname.LastIndexOf("F") - 3);
            ViewState["field"] = fieldnm;
        }


        if (!util.IsSearchFieldNameValid(fldname) || !util.IsNumber(activeRow) || !util.IsNumber(frameNo) || !util.IsAlphaNumUnd(tstKey) || !util.IsAlphaNum(parStr) || !util.IsAlphaNum(subStr))
            Response.Redirect(Constants.PARAMERR);

    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", " callParentNew(\"ResetSession()\",\"function\");", true);
        }
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("if(window.opener && !window.opener.closed){window.opener.parent.location.href='" + url + "';window.close();}else {parent.parent.location.href='" + url + "';}");
        Response.Write("</script>");
    }

    private string CheckSpecialChars(string str)
    {
        if (str != null)
        {
            str = Regex.Replace(str, "amp;", "&");
            str = Regex.Replace(str, "hash;", "#");
            str = Regex.Replace(str, "&", "&amp;");
            str = Regex.Replace(str, "<", "&lt;");
            str = Regex.Replace(str, ">", "&gt;");
            str = Regex.Replace(str, '"'.ToString(), "&quot;");
            str = Regex.Replace(str, "'", "&apos;");
        }
        return str;
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int p = GridView1.PageIndex;
            DataRowView drv = default(DataRowView);
            drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (drv != null)
                {
                    string catName = e.Row.RowIndex.ToString();
                    //if (_isMulSel != "true")
                    //{
                    //    e.Row.Attributes["onclick"] = "loadParent('" + catName + "');";
                    //    e.Row.Attributes["tabindex"] = "0";
                    //}
                    //else
                    //{
                    //    e.Row.Attributes["onclick"] = "SetchkAdsVal('" + catName + "');";
                    //    e.Row.Attributes["tabindex"] = "0";
                    //}
                    if (_isMulSel != "true")
                    {
                        e.Row.Attributes["onclick"] = "SetchkAdsVal('" + catName + "');";
                        e.Row.Attributes["tabindex"] = "0";
                    }
                    else
                    {
                        e.Row.Attributes["onclick"] = "SetchkAdsValMul('" + catName + "');";
                        e.Row.Attributes["tabindex"] = "0";
                    }
                    int n = 0;
                    for (n = 0; n <= e.Row.Cells.Count - 1; n++)
                    {
                        if (e.Row.Cells[n].Text == "*")
                        {
                            e.Row.Cells[n].Text = "";
                        }
                    }
                }
                if (selFildValues.Count > 0 && (selFildValues.IndexOf(e.Row.Cells[0].Text) > -1 || selFildValues.IndexOf(e.Row.Cells[1].Text) > -1))
                {
                    e.Row.CssClass = "tableStripedFix bg-secondary";
                }
            }
            //for NOWRAP in IE
            int m = 0;
            for (m = 0; m <= e.Row.Cells.Count - 1; m++)
            {
                if (e.Row.Cells[m].Text.Length > 0)
                {
                    //if (_isMulSel == "true" && e.Row.RowType == DataControlRowType.Header && e.Row.Cells[m].Text == "mulChk")
                    if (e.Row.RowType == DataControlRowType.Header && e.Row.Cells[m].Text == "mulChk")
                    {
                        e.Row.Cells[m].Text = "";
                        e.Row.Cells[m].CssClass += " capitalizedText bg-white position-sticky top-0 w-15px";
                    }
                    else
                    {
                        if (e.Row.RowType == DataControlRowType.Header && e.Row.Cells[m].Text == "&nbsp;")
                            e.Row.Cells[m].CssClass += "no-sort";
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            //e.Row.Cells[m].Text = e.Row.Cells[m].Text.ToLower().Replace("_", " ");
                            e.Row.Cells[m].Text = e.Row.Cells[m].Text.Replace("_", " ");
                            e.Row.Cells[m].CssClass += " capitalizedText bg-white position-sticky top-0";
                        }
                        if (e.Row.Cells[m].Text != string.Empty && e.Row.Cells[m].Text.EndsWith("*"))
                        {
                            e.Row.Cells[m].Text = e.Row.Cells[m].Text.Remove(e.Row.Cells[m].Text.ToString().Length-1);
                            e.Row.Cells[m].Text = "<nobr class=\"required\">" + e.Row.Cells[m].Text + "</nobr>";
                        }
                        else
                        {
                            e.Row.Cells[m].Text = "<nobr>" + e.Row.Cells[m].Text + "</nobr>";
                        }
                    }
                }

                if (m > 0 && arrHiddenFillCols[m - 1] != null && arrHiddenFillCols[m - 1].ToString() == "T")
                {
                    e.Row.Cells[m].CssClass += " d-none";
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Autocomplete.aspx.cs, RowDataBound function-" + ex.Message, Session["nsessionid"].ToString(), "RowDataBound", "new");
        }
    }

    protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        DataTable dtOrders = new DataTable();
        dtOrders = (DataTable)Session["order"];
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = dtOrders.DefaultView;
        GridView1.DataBind();
    }

}
