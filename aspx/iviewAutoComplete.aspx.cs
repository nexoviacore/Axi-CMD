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
public partial class aspx_iviewAutoComplete : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    static LogFile.Log logobj = new LogFile.Log();
    public string fldname = string.Empty; string fldcaption = string.Empty; string tid = string.Empty; string activeRow = string.Empty; string frameNo = string.Empty;
    string tstKey = string.Empty; string parStr = string.Empty; string subStr = string.Empty; string idcol = string.Empty; string name = string.Empty;
    string srchTxt = string.Empty; string fieldnm = string.Empty; string pdc = string.Empty; string prow = string.Empty;
    string depParamVal = string.Empty;
    string dF = string.Empty;
    int pageNo = 1;
    public string EnableOldTheme = "";
    ArrayList arrHiddenCols = new ArrayList();
    ArrayList srchColsDataType = new ArrayList();
    TStructData tstData;
    public string direction = "ltr";
    public string langType = "en";
    public string isDropdown = "false";
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

                //CallWebservice();
                fname.Value = fldname;
            }
            else
                SetQueryStringVals();
        }
        searchlist.Attributes.Add("onchange", "SetSelectedValue(this,'" + searchlistval.ClientID + "');");
    }

    private void SetQueryStringVals()
    {
        fldname = ViewState["fldname"].ToString();
        tid = ViewState["tid"].ToString();
        tstKey = ViewState["key"].ToString();


        depParamVal = ViewState["params"].ToString();
        dF = ViewState["dF"].ToString();
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
        //CallWebservice();
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
        string srchText = txtfilter.Text;
        string srchTextBetween = txtfilter1.Text;
        string condition = ddlCondition.SelectedValue;
        ViewState["pageno"] = pageNo;
        string result = string.Empty;
        try
        {
            IviewData objIview = (IviewData)Session[tstKey];
            if (objIview == null)
            {
                result = "";
            }
            else
            {
                Util.Util utilGlo = new Util.Util();
                string pickDepParams = string.Empty;
                try
                {
                    if (depParamVal != "")
                    {
                        string[] pdParamStr = depParamVal.Split('¿');
                        for (int i = 0; i < pdParamStr.Count(); i++)
                        {
                            string pdNameVal = pdParamStr[i];
                            if (pdNameVal != "")
                            {
                                string[] pdNames = pdNameVal.Split('~');
                                pickDepParams = pickDepParams + "<" + pdNames[0] + ">";
                                pickDepParams = pickDepParams + utilGlo.CheckSpecialChars(pdNames[1]);
                                pickDepParams = pickDepParams + "</" + pdNames[0] + ">";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                if (condition == string.Empty)
                {
                    condition = ddlCondition.SelectedValue = "contains";
                }
                else if (condition == "=")
                {
                    condition = "equal to";
                }
                else if (condition == "!=")
                {
                    condition = "not equal to";
                }

                string dfCol = string.Empty;

                if (ddlSearchFld.Items.Count > 0)
                {
                    if (ViewState["dF"] != null & ViewState["dF"].ToString() != string.Empty)
                    {
                        dfCol = ddlSearchFld.Items[ddlSearchFld.SelectedIndex].Value;
                    }
                }

                srchText = utilGlo.CheckSpecialChars(srchText);
                string iXml = string.Empty;
                string filename = "iviewpicklist-" + tid;
                LogFile.Log logobj = new LogFile.Log();
                ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
                string errlog = logobj.CreateLog("", HttpContext.Current.Session["nsessionid"].ToString(), filename, "");
                if (isDropdown == "false")
                {
                    //int _pagesize = 50;
                    //if (dF == "")
                    //{
                        pageNo = 0;
                        int _pagesize = 0;
                    //}
                    iXml = iXml + "<sqlresultset axpapp=\"" + HttpContext.Current.Session["project"] + "\" cond=\"" + CheckSpecialChars(condition) + "\" value=\"" + srchText + "\" sessionid= \"" + HttpContext.Current.Session["nsessionid"] + "\" pname =\"" + fldname + "\" trace=\"" + errlog + "\" user=\"" + HttpContext.Current.Session["user"] + "\" ivname=\"" + tid + "\" pageno =\"" + pageNo + "\" pagesize=\"" + _pagesize + "\" dfcolname='" + dfCol + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                    if (pickDepParams != string.Empty)
                        iXml = iXml + "<varlist>" + pickDepParams + "</varlist>";
                    iXml = iXml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
                    iXml = iXml + "</sqlresultset>";

                    result = asbExt.CallGetParamChoicesWS(tid, iXml, objIview.StructureXml);

                }
                else
                {
                    try
                    {
                        IviewParams objIviewParam = (IviewParams)Session[tstKey + "_param"];
                        string _paramXML = objIviewParam.ParamXML;
                        XmlDocument xmlDoc1 = new XmlDocument();
                        xmlDoc1.LoadXml(_paramXML);
                        XmlNodeList compNodes = default(XmlNodeList);
                        compNodes = xmlDoc1.SelectNodes("/root/" + fldname + "/response");
                        if (compNodes[0] != null)
                        {
                            int totRows = compNodes[0].ChildNodes.Count;
                            result = "<sqlresultset>" + compNodes[0].OuterXml + "</sqlresultset>";
                            result = result.Replace("<response>", "<response totalrows='" + totRows + "'>");
                        }
                        else
                        {
                            iXml = iXml + "<sqlresultset axpapp=\"" + HttpContext.Current.Session["project"] + "\" cond=\"" + CheckSpecialChars(condition) + "\" value=\"" + srchText + "\" sessionid= \"" + HttpContext.Current.Session["nsessionid"] + "\" pname =\"" + fldname + "\" trace=\"" + errlog + "\" user=\"" + HttpContext.Current.Session["user"] + "\" ivname=\"" + tid + "\" pageno =\"0\" pagesize=\"0\" dfcolname='" + dfCol + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                            if (pickDepParams != string.Empty)
                                iXml = iXml + "<varlist>" + pickDepParams + "</varlist>";
                            iXml = iXml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
                            iXml = iXml + "</sqlresultset>";
                            result = asbExt.CallGetParamChoicesWS(tid, iXml, objIview.StructureXml);
                        }
                    }
                    catch (Exception ex)
                    {
                        result = string.Empty;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in iviewAutocomplete.aspx.cs, CallWebservice function-" + ex.Message, Session["nsessionid"].ToString(), "CallGetParamChoicesWS" + tid, "");
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
                    else if (pageNo == 0 && tnode != null)
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
            logobj.CreateLog("Exception in iviewAutocomplete.aspx.cs, ParseResult function-" + ex.Message, Session["nsessionid"].ToString(), "ParseResult", "new");
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
                            chhNode.Attributes.RemoveAll();
                        }
                    }
                    j = j + 1;
                }

                //string totrow = cNode.Attributes["totalrows"].Value;
                string totrow = totRows.ToString();
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
                    int i = 0;
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        BoundField bfield = new BoundField();
                        //'initialiae the data field value
                        bfield.DataField = dc.ColumnName;
                        //'initialise the header text value                        
                        bfield.HeaderText = dc.ColumnName;
                        //' add newly created columns to gridview
                        GridView1.Columns.Add(bfield);
                        if (arrHiddenCols.IndexOf(GridView1.Columns[i].HeaderText) != -1)
                        {
                            GridView1.Columns[i].Visible = false;
                        }
                    }

                    GridView1.DataSource = ds.Tables[0];
                    GridView1.DataBind();

                    //int pagSize = 50;
                    //int totalPages = (int.Parse(totrow) + pagSize - 1) / pagSize;
                    //int startEntry = (pageNo - 1) * pagSize + 1;
                    //int endEntry = Math.Min(pageNo * pagSize, int.Parse(totrow));
                    //if (pageNo <= 1)
                    //{
                    //    var strCssClass = btnPriv.Attributes["class"];
                    //    if (strCssClass.IndexOf("disabled") == -1)
                    //        btnPriv.Attributes["class"] = strCssClass + " disabled";
                    //}
                    //else
                    //{
                    //    btnPriv.Attributes.Add("class", btnPriv.Attributes["class"].Replace("disabled", ""));
                    //}
                    //if (pageNo == totalPages)
                    //{
                    //    var strCssClass = btnNextclk.Attributes["class"];
                    //    if (strCssClass.IndexOf("disabled") == -1)
                    //        btnNextclk.Attributes["class"] = strCssClass + " disabled";
                    //}
                    //else
                    //{
                    //    btnNextclk.Attributes.Add("class", btnNextclk.Attributes["class"].Replace("disabled", ""));
                    //}
                    lblstart.Text = "1";
                    lblend.Text = totrow;
                    lbltotrec.Text = totrow;
                    dvSearch.Visible = true;
                    dvFooter.Visible = true;
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
            logobj.CreateLog("Exception in iviewAutocomplete.aspx.cs, BindDataGrid function-" + ex.Message, Session["nsessionid"].ToString(), "BindDataGrid", "new");
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
            logobj.CreateLog("Exception in iviewAutocomplete.aspx.cs, SetItemsInHdnFlds function-" + ex.Message, Session["nsessionid"].ToString(), "SetItemsInHdnFlds", "new");
        }
    }

    private void GetQueryStringVals()
    {
        if (Request.QueryString["fldname"] != null)
        {
            fldname = Request.QueryString["fldname"];
            ViewState["fldname"] = fldname;
        }
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

        if (Request.QueryString["params"] != null)
        {
            depParamVal = Request.QueryString["params"];
            ViewState["params"] = depParamVal;
        }

        if (Request.QueryString["dF"] != null)
        {
            dF = Request.QueryString["dF"];
            ViewState["dF"] = dF;

            if (dF != string.Empty)
            {
                foreach (string dd in dF.Split('|'))
                {
                    try
                    {
                        ddlSearchFld.Items.Add(new ListItem(dd.Split('~')[1] == null ? dd.Split('~')[0] : dd.Split('~')[1], dd.Split('~')[0]));
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else
            {
                if (Request.QueryString["fldcaption"] != null)
                {
                    fldcaption = Request.QueryString["fldcaption"];
                    ViewState["fldcaption"] = fldcaption;
                    ddlSearchFld.Items.Add(new ListItem(fldcaption, fldcaption));
                }               
            }
        }

        if (Request.QueryString["isSelect"] != null)
        {
            isDropdown = Request.QueryString["isSelect"];
        }
        else
        {
            isDropdown = "false";
        }

        //if (Request.QueryString["isFldddl"] != null && Request.QueryString["isFldddl"].ToString() == "true")
        //{
        //    divFilter.Visible = false;
        //}

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

                    e.Row.Attributes["onclick"] = "loadParent('" + catName + "');";
                    e.Row.Attributes["tabindex"] = "0";

                    int n = 0;
                    for (n = 0; n <= e.Row.Cells.Count - 1; n++)
                    {
                        if (e.Row.Cells[n].Text == "*")
                        {
                            e.Row.Cells[n].Text = "";
                        }
                    }
                }
            }
            //for NOWRAP in IE
            int m = 0;
            for (m = 0; m <= e.Row.Cells.Count - 1; m++)
            {
                if (e.Row.Cells[m].Text.Length > 0)
                {
                    if (e.Row.RowType == DataControlRowType.Header && e.Row.Cells[m].Text == "&nbsp;")
                        e.Row.Cells[m].CssClass += "no-sort";
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[m].Text = e.Row.Cells[m].Text.ToLower().Replace("_", " ");
                        e.Row.Cells[m].CssClass += " capitalizedText";
                    }
                    e.Row.Cells[m].Text = "<nobr>" + e.Row.Cells[m].Text + "</nobr>";
                }
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in iviewAutocomplete.aspx.cs, RowDataBound function-" + ex.Message, Session["nsessionid"].ToString(), "RowDataBound", "new");
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
