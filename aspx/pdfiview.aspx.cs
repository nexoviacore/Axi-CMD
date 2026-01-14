using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Saml;
using System.Xml.Linq;
using Word.W2004.Style;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Web.Services;
using DocumentFormat.OpenXml.Packaging;
using System.Globalization;

public partial class pdfiview : System.Web.UI.Page
{
    private string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
    private LogFile.Log logobj = new LogFile.Log();
    private Util.Util util = new Util.Util();
    private ArrayList colHide = new ArrayList();
    private ArrayList colFld = new ArrayList();
    private ArrayList colWidth = new ArrayList();
    private ArrayList colHead = new ArrayList();
    private ArrayList ivhead = new ArrayList();
    private ArrayList arrLstParams = new ArrayList();
    private ArrayList arrLstParamVal = new ArrayList();
    private string iname;
    private string user;
    private string sid;
    private string ivtype;
    private string proj;
    private string paramVal;
    private string iViewCap;
    private string paramXml = "";
    private string typeIvOrLv = string.Empty;
    private string curRecord = string.Empty;
    private bool hideProjeName = false;
    private ArrayList pivotGroupHeaderNames = new ArrayList();
    private ArrayList pivotStartCol = new ArrayList();
    private ArrayList pivotEndCol = new ArrayList();
    private ArrayList htmlColumns = new ArrayList();
    IviewData objIview = new IviewData();
    public StringBuilder sb = new StringBuilder();

    ArrayList hdrFont = new ArrayList();
    ArrayList hdrAlign = new ArrayList();

    ArrayList hdrFooterName = new ArrayList();
    ArrayList hdrFooterAlign = new ArrayList();
    ArrayList hdrFooterFont = new ArrayList();
    ArrayList axhiddencols = new ArrayList();
    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["project"].ToString() == "")
            sess_expires();
        else
        {
            if (!util.CheckValidLogin())
            {
                sess_expires();
                return;
            }
            if (Request.UrlReferrer != null)
            {
            }
            string Ikey = string.Empty;
            if (Request.QueryString["ivKey"] != null)
                Ikey = Request.QueryString["ivKey"];
            if (!IsPostBack)
            {
                iname = Request.QueryString["ivname"];

                if (iname != null)
                {
                    if (!util.IsValidIvName(iname))
                        Response.Redirect(Constants.PARAMERR);
                }
                user = Session["user"].ToString();
                sid = Session["nsessionid"].ToString();
                ivtype = Request.QueryString["ivtype"];

                if (ivtype != null)
                {
                    if (ivtype == null & !util.IsChar(ivtype))
                        Response.Redirect(Constants.PARAMERR);
                }

                if (Request.QueryString["axpCache"] != null)
                {
                    paramVal = Session["AxIvExportParams-" + iname].ToString();
                    Session["AxIvExportParams-" + iname] = "";
                }
                else
                    paramVal = Request.QueryString["params"].ToString();

                if (iname == null & Session["ivname"] != null/* TODO Change to default(_) if this is not a reference type */ )
                    iname = Session["ivname"].ToString();
                if (paramVal == null & Session["params"] != null/* TODO Change to default(_) if this is not a reference type */ )
                    paramVal = Session["params"].ToString();
                if (ivtype == null & Session["ivtype"] != null/* TODO Change to default(_) if this is not a reference type */ )
                    ivtype = Session["ivtype"].ToString();

                objIview = (IviewData)Session[Ikey];
                objIview.ReportHdrs = new ArrayList();
                ViewState["ivtype"] = ivtype;
                ViewState["params"] = paramVal;
                proj = Session["project"].ToString();
                ViewState["iname"] = iname;
                ViewState["ivtype"] = ivtype;
                ViewState["proj"] = proj;
                ViewState["user"] = user;
                ViewState["sid"] = sid;

                if (Request.QueryString["typeIvOrLv"] != null)
                    typeIvOrLv = Request.QueryString["typeIvOrLv"].ToString();
                ViewState["typeIvOrLv"] = typeIvOrLv;

                if (Request.QueryString["curRecord"] != null)
                    curRecord = Request.QueryString["curRecord"].ToString();
                ViewState["curRecord"] = curRecord;
            }
            else
            {
                user = ViewState["user"].ToString();
                iname = ViewState["iname"].ToString();
                ivtype = ViewState["ivtype"].ToString();
                proj = ViewState["proj"].ToString();
                sid = ViewState["sid"].ToString();
                paramVal = ViewState["params"].ToString();
                ivtype = ViewState["ivtype"].ToString();
                typeIvOrLv = ViewState["typeIvOrLv"].ToString();
                curRecord = ViewState["curRecord"].ToString();
            }
            string str = GetPDF();
            Response.Buffer = true;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            System.IO.FileInfo files = new System.IO.FileInfo(str);
            Response.AddHeader("Content-disposition", "attachment;filename=" + files.Name);
            Response.ContentType = "application/pdf";
            Response.WriteFile(files.FullName);
            Response.End();
        }
    }

    private Cell AlignCell(Cell cellAttr, string AlignStr)
    {
        if (AlignStr == "" | AlignStr == "Left")
            cellAttr.HorizontalAlignment = Element.ALIGN_LEFT;
        else if (AlignStr == "Right")
            cellAttr.HorizontalAlignment = Element.ALIGN_RIGHT;
        else if (AlignStr == "Center")
            cellAttr.HorizontalAlignment = Element.ALIGN_CENTER;

        return cellAttr;
    }

    private void GetParam(string param)
    {
        int k;
        string tem1 = string.Empty;
        string[] arrParams;
        string[] arrNoOfParams;
        string @params = string.Empty;

        if (paramVal != null)
        {
            if (paramVal.Contains("~"))
            {
                arrNoOfParams = paramVal.Split('~');
                int i;
                for (i = 0; i <= arrNoOfParams.Length - 1; i++)
                {
                    arrParams = arrNoOfParams[i].Split('♠');
                    arrLstParams.Insert(i, arrParams[0]);
                    if (arrParams[1].Contains("`") == true)
                        arrParams[1] = arrParams[1].Replace("`", "~");

                    arrLstParamVal.Insert(i, arrParams[1]);
                }
            }
            else
            {
                arrParams = paramVal.Split('♠');
                for (k = 0; k <= arrParams.Length - 1; k++)
                {
                    if (arrParams.Length > k + 1)
                    {
                        arrLstParams.Insert(k, arrParams[0]);
                        if (arrParams[1].Contains("`") == true)
                            arrParams[1] = arrParams[1].Replace("`", ",");

                        arrLstParamVal.Insert(k, arrParams[1]);
                    }
                }
            }
        }

        int j;
        for (j = 0; j <= arrLstParams.Count - 1; j++)
        {
            if (arrLstParams.Count > 0)
            {
                tem1 = arrLstParams[j].ToString();
                if (arrLstParams.Count > 0)
                {
                    paramVal = arrLstParamVal[j].ToString().Replace("&grave;", "~");
                    paramVal = arrLstParamVal[j].ToString().Replace("&amp;", "&");
                }
                else
                    paramVal = "";
            }

            if (param != "")
            {
                paramXml = paramXml + "<" + tem1.ToString() + ">";
                //paramXml = paramXml + util.CheckSpecialCharsSaveAs(util.ReplaceUrlSpecialChars(paramVal));

                string kyval = util.CheckSpecialCharsSaveAs(util.ReplaceUrlSpecialChars(paramVal));
                kyval = System.Net.WebUtility.HtmlDecode(kyval);
                kyval = kyval.Replace("&grave;", "~");
                kyval = kyval.Replace("&amp;", "&");
                paramXml = paramXml + kyval;

                paramXml = paramXml + "</" + tem1 + ">";

                @params = @params + "&" + tem1 + "=";
                @params = @params + paramVal;

                if (tem1 != "" && tem1.ToLower().ToString() == "axhiddencolumn")
                {
                    foreach (string _thiscolName in paramVal.Split(','))
                        axhiddencols.Add(_thiscolName);
                }
            }
            else
                paramXml = "<params>";
        }
    }

    public void sess_expires()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language=\"javascript\">" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }
    public string ConvertDatatableToXML(System.Data.DataTable dt)
    {
        MemoryStream str = new MemoryStream();
        dt.TableName = "row";
        dt.WriteXml(str, true);
        str.Seek(0, SeekOrigin.Begin);
        StreamReader sr = new StreamReader(str);
        string xmlstr;
        xmlstr = sr.ReadToEnd().Replace("DocumentElement", "data");
        return (xmlstr);
    }

    private string GetPDF()
    {
        string isList;
        if (ivtype == "listview" | ivtype == "lview")
            isList = "true";
        else
            isList = "false";

        GetParam(paramVal);

        string sortCol = string.Empty;
        string sortOrd = string.Empty;
        string filterCol = string.Empty;
        string filterColVal = string.Empty;
        string filterValue1 = string.Empty;
        string filterOpr = string.Empty;

        if (Session["sOrder"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (Session["sOrder"].ToString() != "")
                sortOrd = Session["sOrder"].ToString();
        }

        if (Session["sColumn"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (Session["sColumn"].ToString() != "")
                sortCol = Session["sColumn"].ToString();
        }

        if (Session["fcolopr"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (Session["fcolopr"].ToString() != "")
                filterOpr = Session["fcolopr"].ToString();
        }

        if (Session["fCol"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (Session["fCol"].ToString() != "")
                filterCol = Session["fCol"].ToString();
        }

        if (Session["fColVal"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (Session["fColVal"].ToString() != "")
                filterColVal = Session["fColVal"].ToString();
        }

        if (Session["fColVal2"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (Session["fColVal2"].ToString() != "")
                filterValue1 = Session["fColVal2"].ToString();
        }

        string fileName = "GetLView" + iname;
        string errorLog = logobj.CreateLog("GetLView.", sid, fileName, "new");
        string custXML = string.Empty;
        string iXml;
        string _thisList = string.Empty;
        if (typeIvOrLv == "true")
            _thisList = " purpose=\"list\" ";
        if (curRecord == "")
            iXml = "<root " + _thisList + " headercached='false' name ='" + iname + "' axpapp ='" + Session["project"].ToString() + "' sessionid = '" + sid + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errorLog + "' pageno='0' firsttime='yes' sorder='" + sortOrd + "' scol='" + sortCol + "' fcolopr='" + filterOpr + "' fcolnm='" + filterCol + "' fcolval1='" + filterColVal + "' fcolval2='" + filterValue1 + "'><params> ";
        else
            iXml = "<root " + _thisList + " headercached='false' name ='" + iname + "' axpapp ='" + Session["project"].ToString() + "' sessionid = '" + sid + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errorLog + "' pageno='1' pagesize='" + curRecord + "' firsttime='yes' sqlpagination='true' getrowcount='false' gettotalrows='false' smartview='true'><params> ";
        iXml = iXml + paramXml;
        iXml = iXml + "</params>";
        iXml = iXml + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        string ires = string.Empty;
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        try
        {
            objIview.requestJSON = true;
            ires = objWebServiceExt.CallGetIViewWS(iname, iXml, "", objIview);
            if (ires != null)
            {
                ires = ires.Split('♠')[1];

                if (ires.StartsWith("<"))
                    ires = util.ReplaceFirstOccurrence(ires, "#$#", "#$♥#");
                string[] resultSplitter = ires.Split(new[] { "#$♥#" }, StringSplitOptions.None);
                ires = resultSplitter[0];
            }
        }
        catch (Exception Ex)
        {
        }

        string strErrMsg = string.Empty;
        strErrMsg = util.ParseXmlErrorNode(ires);

        if (strErrMsg != string.Empty)
        {
            strErrMsg = strErrMsg.Replace("\r", "").Replace("\n", "");
            if (strErrMsg == Constants.SESSIONERROR)
            {
                Session.RemoveAll();
                Session.Abandon();
                string url1;
                url1 = util.SESSEXPIRYPATH;
                Response.Write("<script>" + Constants.vbCrLf);
                Response.Write("parent.parent.location.href='" + url1 + "';");
                Response.Write(Constants.vbCrLf + "</script>");
            }
            else
                Response.Redirect(util.ERRPATH + strErrMsg);
        }
        else
        {
            if (ires != string.Empty)
            {
                JObject jsonObject = JObject.Parse(ires);
                JObject data = (JObject)jsonObject["data"];
                XDocument xmlDocument = new XDocument();
                XElement root = new XElement("data");
                XElement dataElement = JsonConvert.DeserializeXNode(data.ToString(), "data").Root;
                root.Add(dataElement.Elements());
                xmlDocument.Add(root);
                ires = xmlDocument.ToString();

                XmlDocument xmlDocNew = new XmlDocument();
                xmlDocNew.LoadXml(ires);
                string datarows = "";
                string totalrows = "";
                string reccount = "";
                XmlNodeList nodesToRemove = xmlDocNew.SelectNodes("//reccount | //totalrows | //datarows");
                foreach (XmlNode node in nodesToRemove)
                {
                    if (node.Name == "datarows")
                        datarows = node.InnerText;
                    if (node.Name == "totalrows")
                        totalrows = node.InnerText;
                    if (node.Name == "reccount")
                        reccount = node.InnerText;

                    node.ParentNode.RemoveChild(node);
                }
                XmlNode metadataNode = xmlDocNew.SelectSingleNode("//metadata");
                if (metadataNode != null)
                {
                    XmlElement newElement = xmlDocNew.CreateElement("headrow");
                    foreach (XmlAttribute attribute in metadataNode.Attributes)
                    {
                        newElement.Attributes.Append((XmlAttribute)attribute.CloneNode(true));
                    }
                    foreach (XmlNode childNode in metadataNode.ChildNodes)
                    {
                        newElement.AppendChild(childNode.CloneNode(true));
                    }
                    metadataNode.ParentNode.ReplaceChild(newElement, metadataNode);
                }

                ires = xmlDocNew.OuterXml;
                XDocument doc = XDocument.Parse(@ires);
                //doc.Descendants("axrowtype").Remove();
                //doc.Descendants("axp__font").Remove();
                var elementsToRemove = doc.Descendants().Where(e =>
        string.Equals(e.Name.LocalName, "axrowtype", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(e.Name.LocalName, "axp__font", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(e.Name.LocalName, "axp__color", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(e.Name.LocalName, "axrowoptions", StringComparison.OrdinalIgnoreCase)).ToList();
                foreach (var el in elementsToRemove)
                    el.Remove();
                ires = doc.ToString();
            }

            ires = _xmlString + ires;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ires);

            pivotGroupHeaderNames.Clear();
            pivotStartCol.Clear();
            pivotEndCol.Clear();

            XmlNodeList productNodes;
            XmlNodeList baseDataNodes;
            productNodes = xmlDoc.SelectNodes("//headrow");
            int hcolNos;
            hcolNos = 0;

            foreach (XmlNode productNode in productNodes)
            {
                baseDataNodes = productNode.ChildNodes;
                int iCount = -1;
                foreach (XmlNode baseDataNode in baseDataNodes)
                {
                    iCount += 1;
                    colFld.Add(baseDataNode.Name);
                    colHead.Add(baseDataNode.InnerText);
                    if (baseDataNode.Attributes["width"] != null)
                        colWidth.Add(baseDataNode.Attributes["width"].Value);
                    else
                        colWidth.Add("0");

                    if (baseDataNode.Name == "rowno")
                    {
                        if (ivtype == "listview" | ivtype == "lview")
                            colHide.Add("false");
                        else
                            colHide.Add("true");
                    }
                    else
                    {
                        if (baseDataNode.Name.StartsWith("html_"))
                            htmlColumns.Add(baseDataNode.Name);

                        if (baseDataNode.Attributes["hide"] != null)
                        {
                            if (baseDataNode.Name.StartsWith("hide_"))
                                colHide.Add("true");
                            else
                                colHide.Add(baseDataNode.Attributes["hide"].Value);
                        }
                        if (baseDataNode.Name == "pivotghead")
                        {
                            XmlNodeList finalNodelist;
                            foreach (XmlNode base2node in baseDataNode)
                            {
                                finalNodelist = base2node.ChildNodes;
                                foreach (XmlNode finalNode in finalNodelist)
                                {
                                    if (finalNode.Name == "sn")
                                        pivotStartCol.Add(finalNode.InnerText);
                                    else if (finalNode.Name == "ghead")
                                        pivotGroupHeaderNames.Add(finalNode.InnerText);
                                    else if (finalNode.Name == "en")
                                        pivotEndCol.Add(finalNode.InnerText);
                                }
                            }
                            colHide.Add("true");
                        }

                        hcolNos = hcolNos + 1;
                    }
                }
            }

            var htIdx = 0;
            var cIdx = 0;
            for (htIdx = 0; htIdx <= htmlColumns.Count - 1; htIdx++)
            {
                for (cIdx = 0; cIdx <= colFld.Count - 1; cIdx++)
                {
                    string htmlColName = htmlColumns[htIdx].ToString();
                    if (colFld[cIdx] == htmlColName)
                        colHide[cIdx] = "false";
                    else if (colFld[cIdx] == htmlColName.Replace("html_", ""))
                        colHide[cIdx] = "true";
                }
            }

            // to remove attributes from headrow
            XmlNodeList productNodes2;
            XmlNodeList baseDataNodes2;
            productNodes2 = xmlDoc.SelectNodes("//headrow");

            foreach (XmlNode productNode2 in productNodes2)
            {
                baseDataNodes2 = productNode2.ChildNodes;
                foreach (XmlNode baseDataNode2 in baseDataNodes2)
                    baseDataNode2.Attributes.RemoveAll();
            }

            XmlNodeList compNodes;
            XmlNodeList cbaseDataNodes;
            compNodes = xmlDoc.SelectNodes("//data/reporthf");
            foreach (XmlNode compNode in compNodes)
            {
                cbaseDataNodes = compNode.ChildNodes;
                foreach (XmlNode cbaseDataNode in cbaseDataNodes)
                {
                    if (cbaseDataNode.Name == "hideprojname")
                    {
                        string hideprojname = cbaseDataNode.InnerText;
                        if (hideprojname != "" && hideprojname == "@t")
                        {
                            hideProjeName = true;
                        }
                    }
                    else if (Strings.Mid(cbaseDataNode.Name, 1, 7) == "x__head")
                    {
                        iViewCap = cbaseDataNode.Attributes["caption"].Value;
                    }
                    else if (Strings.Mid(cbaseDataNode.Name, 1, 3) == "lbl")
                    {
                        iViewCap = cbaseDataNode.Attributes["caption"].Value;
                    }
                    else if (cbaseDataNode.Name == "header")
                    {
                        foreach (XmlNode childHeaders in cbaseDataNode)
                        {
                            string xmlContent = childHeaders.OuterXml;
                            XmlDocument _xmlDoc = new XmlDocument();
                            _xmlDoc.LoadXml(xmlContent);
                            XmlNode textNode = _xmlDoc.SelectSingleNode("//text");
                            XmlNode textNodefont = _xmlDoc.SelectSingleNode("//font");
                            if (textNodefont != null)
                                hdrFont.Add(textNodefont.InnerText);
                            else
                                hdrFont.Add("");
                            XmlNode textNodeheader_aline = _xmlDoc.SelectSingleNode("//header_aline");
                            if (textNodeheader_aline != null)
                                hdrAlign.Add(textNodeheader_aline.InnerText);
                            else
                                hdrAlign.Add("");
                            objIview.ReportHdrs.Add(textNode.InnerText.Trim());
                        }
                    }
                    if (cbaseDataNode.Name == "footer")
                    {
                        foreach (XmlNode childFooters in cbaseDataNode)
                        {
                            string xmlContent = childFooters.OuterXml;
                            XmlDocument _xmlDoc = new XmlDocument();
                            _xmlDoc.LoadXml(xmlContent);
                            XmlNode textNode = _xmlDoc.SelectSingleNode("//text");
                            hdrFooterName.Add(textNode.InnerText);
                            XmlNode textNodefont = _xmlDoc.SelectSingleNode("//font");
                            if (textNodefont != null)
                                hdrFooterFont.Add(textNodefont.InnerText);
                            else
                                hdrFooterFont.Add("");
                            XmlNode textNodeheader_aline = _xmlDoc.SelectSingleNode("//footer_aline");
                            if (textNodeheader_aline != null)
                                hdrFooterAlign.Add(textNodeheader_aline.InnerText);
                            else
                                hdrFooterAlign.Add(hdrFooterAlign[0]);
                        }
                    }
                }
            }


            // Remove Comps
            XmlNode cNode;
            cNode = xmlDoc.SelectSingleNode("//comps");
            if (cNode != null)
                cNode.ParentNode.RemoveChild(cNode);

            // remove hidden fields in headrow
            int hrep;
            // change  later try new code to remove this for
            for (hrep = 1; hrep <= hcolNos; hrep++)
            {
                XmlNodeList productNodes3;
                XmlNodeList baseDataNodes3;
                productNodes3 = xmlDoc.SelectNodes("//headrow");

                int hidx;

                foreach (XmlNode productNode3 in productNodes3)
                {
                    baseDataNodes3 = productNode3.ChildNodes;
                    foreach (XmlNode baseDataNode3 in baseDataNodes3)
                    {
                        // Response.Write(baseDataNode1.Name & "<br>")
                        for (hidx = 0; hidx <= colFld.Count - 1; hidx++)
                        {
                            if (colFld[hidx] == baseDataNode3.Name)
                            {
                                try
                                {
                                    if (colHide[hidx].ToString() == "true")
                                        baseDataNode3.ParentNode.RemoveChild(baseDataNode3);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
            }

            // remove hidden fields
            int rep;
            // change  later try new code to remove this for
            for (rep = 1; rep <= hcolNos; rep++)
            {
                XmlNodeList productNodes1;
                XmlNodeList baseDataNodes1;
                productNodes1 = xmlDoc.SelectNodes("//row");

                int idx;
                foreach (XmlNode productNode1 in productNodes1)
                {
                    baseDataNodes1 = productNode1.ChildNodes;
                    foreach (XmlNode baseDataNode1 in baseDataNodes1)
                    {
                        // Response.Write(baseDataNode1.Name & "<br>")
                        for (idx = 0; idx <= colFld.Count - 1; idx++)
                        {
                            try
                            {
                                if (colFld[idx].ToString().ToLower() == baseDataNode1.Name.ToLower())
                                {
                                    if (colHide[idx].ToString() == "true")
                                        baseDataNode1.ParentNode.RemoveChild(baseDataNode1);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }

            // PDF creation starts
            // find no of visible columns

            int viscols;
            viscols = 0;
            int g;
            if (ivtype == "listview" | ivtype == "lview" | ivtype.ToLower() == "iview")
            {
                for (g = 0; g <= colHide.Count - 1; g++)
                {
                    if (colHide[g].ToString() == "false")
                    {
                        if (axhiddencols.IndexOf(colFld[g].ToString()) == -1)
                            viscols = viscols + 1;
                    }
                }
            }
            else
                for (g = 1; g <= colHide.Count - 1; g++)
                {
                    if (colHide[g].ToString() == "false")
                    {
                        if (axhiddencols.IndexOf(colFld[g].ToString()) == -1)
                            viscols = viscols + 1;
                    }
                }

            int[] widthscell = new int[viscols - 1 + 1];
            int clcont = 0;
            if (ivtype == "listview" | ivtype == "lview" | ivtype.ToLower() == "iview")
            {
                for (g = 0; g <= colHide.Count - 1; g++)
                {
                    if (colHide[g].ToString() == "false")
                    {
                        if (colFld.Count > 0)
                        {
                            if (axhiddencols.IndexOf(colFld[g].ToString()) > -1)
                                continue;
                            if (colFld[g].ToString() == "rowno")
                            {
                                widthscell[clcont] = 50;
                                clcont = clcont + 1;
                            }
                            else
                            {
                                // widthscell(clcont) = objIview.ColWidth(g)
                                if (colWidth != null)
                                    widthscell[clcont] = int.Parse(colWidth[colFld.IndexOf(colFld[g])].ToString());
                                else
                                    widthscell[clcont] = int.Parse(colWidth[g].ToString());
                                clcont = clcont + 1;
                            }
                        }
                        else if (colWidth.Count > 0)
                        {
                            if (axhiddencols.IndexOf(colFld[g].ToString()) > -1)
                                continue;
                            if (colFld[g].ToString() == "rowno")
                            {
                                widthscell[clcont] = 50;
                                clcont = clcont + 1;
                            }
                            else
                            {
                                widthscell[clcont] = int.Parse(colWidth[g].ToString());
                                clcont = clcont + 1;
                            }
                        }
                    }
                }
            }
            else
                for (g = 1; g <= colHide.Count - 1; g++)
                {
                    if (colHide[g].ToString() == "false")
                    {
                        if (axhiddencols.IndexOf(colFld[g].ToString()) == -1)
                            continue;
                        if (colWidth != null)
                            widthscell[clcont] = int.Parse(colWidth[colFld.IndexOf(colFld[g])].ToString());
                        else
                            widthscell[clcont] = int.Parse(colWidth[g].ToString());
                        clcont = clcont + 1;
                    }
                }

            iTextSharp.text.Table htab;
            if (viscols == 0)
                viscols = 1;
            htab = new iTextSharp.text.Table(viscols);
            htab.WidthPercentage = 100;
            htab.Border = 1;
            htab.Cellspacing = 1;
            htab.Cellpadding = 1;
            htab.SetWidths(widthscell);

            int inc;
            int colSpan;
            int totColSpan = 0;
            for (inc = 0; inc <= pivotGroupHeaderNames.Count - 1; inc++)
            {
                string hn;
                colSpan = System.Convert.ToInt32(pivotEndCol[inc].ToString()) - System.Convert.ToInt32(pivotStartCol[inc].ToString());
                hn = pivotGroupHeaderNames[inc].ToString();
                try
                {
                    string fontPath = GetFontPathBasedOnLanguage(hn);
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));
                    
                    hn = util.ReplaceTextAreaChars(hn, "pdf");
                    hn = StripHtmlTagsMobile(hn);

                    Cell cellAttribute = new Cell(new Paragraph(hn, universalFont));

                    cellAttribute.Colspan = colSpan;
                    cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                    htab.AddCell(cellAttribute);
                }
                catch (Exception)
                {
                    Cell cellAttribute = new Cell(new Phrase(hn, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                    cellAttribute.Colspan = colSpan;
                    cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                    htab.AddCell(cellAttribute);
                }
                totColSpan += colSpan;
            }

            XmlNodeList hproductNodes;
            XmlNodeList hbaseDataNodes;
            int tmpColCount = 0;
            hproductNodes = xmlDoc.SelectNodes("/data/headrow");
            foreach (XmlNode hproductNode in hproductNodes)
            {
                hbaseDataNodes = hproductNode.ChildNodes;
                tmpColCount = hbaseDataNodes.Count;
                Response.Write(hbaseDataNodes.Count);
                foreach (XmlNode hbaseDataNode in hbaseDataNodes)
                {
                    string hn;
                    if (hbaseDataNode.Name == "rowno")
                    {
                        if (ivtype == "listview" | ivtype == "lview")
                        {
                            hn = "Sr. No.";
                            try
                            {
                                string fontPath = GetFontPathBasedOnLanguage(hn);
                                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                                iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                                hn = util.ReplaceTextAreaChars(hn, "pdf");
                                hn = StripHtmlTags(hn);

                                Cell cellAttribute = new Cell(new Paragraph(hn, universalFont));

                                cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                                htab.AddCell(cellAttribute);
                            }
                            catch (Exception)
                            {
                                Cell cellAttribute = new Cell(new Phrase(hn, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                                cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                                htab.AddCell(cellAttribute);
                            }

                        }
                    }
                    else
                    {
                        hn = hbaseDataNode.InnerText;
                        if (axhiddencols.IndexOf(hn) > -1)
                        {
                            tmpColCount--;
                            continue;
                        }
                        if (hn == "axp_slno")
                            hn = "Sr. No.";
                        if ((hn.IndexOf("~") != -1))
                            hn = hn.Replace("~", "\n");
                        try
                        {
                            string fontPath = GetFontPathBasedOnLanguage(hn);
                            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                            hn = util.ReplaceTextAreaChars(hn, "pdf");
                            hn = StripHtmlTags(hn);

                            Cell cellAttribute = new Cell(new Paragraph(hn, universalFont));

                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                            if (htab != null)
                                htab.AddCell(cellAttribute);
                        }
                        catch (Exception)
                        {
                            Cell cellAttribute = new Cell(new Phrase(hn, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                            if (htab != null)
                                htab.AddCell(cellAttribute);
                        }
                    }
                }
            }

            // Get Data
            XmlNodeList dproductNodes;
            XmlNodeList dbaseDataNodes;
            string AlignStr = string.Empty;
            dproductNodes = xmlDoc.SelectNodes("/data/row");
            foreach (XmlNode dproductNode in dproductNodes)
            {
                dbaseDataNodes = dproductNode.ChildNodes;
                int i = 1;

                foreach (XmlNode dbaseDataNode in dbaseDataNodes)
                {
                    //if (colFld != null)
                    //{
                    //    var indx = colFld.IndexOf(dbaseDataNode.Name);
                    //    if (indx > -1)
                    //        AlignStr = ColAlign[indx].ToString();
                    //}
                    string strCellText = string.Empty;
                    if (dbaseDataNode.Name == "rowno")
                    {
                        if (ivtype == "listview" | ivtype == "lview")
                        {
                            strCellText = dbaseDataNode.InnerText.ToString();
                            strCellText = util.ReplaceTextAreaChars(strCellText, "pdf");
                            strCellText = StripHtmlTags(strCellText);
                            try
                            {
                                string fontPath = GetFontPathBasedOnLanguage(strCellText);
                                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                                iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1f));

                                Cell cellAttribute = new Cell(new Paragraph(strCellText, universalFont));

                                cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                                if (htab != null)
                                    htab.AddCell(cellAttribute);
                            }
                            catch (Exception)
                            {
                                Cell cellAttribute = new iTextSharp.text.Cell(new Paragraph(strCellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1F))));
                                cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                                if (htab != null)
                                    htab.AddCell(cellAttribute);
                            }
                        }
                        else
                            continue;
                    }
                    else
                    {
                        if (axhiddencols.IndexOf(dbaseDataNode.Name) > -1)
                            continue;
                        strCellText = dbaseDataNode.InnerText.ToString();
                        strCellText = util.ReplaceTextAreaChars(strCellText, "pdf");
                        strCellText = StripHtmlTags(strCellText);
                        try
                        {
                            string fontPath = GetFontPathBasedOnLanguage(strCellText);
                            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1f));


                            Cell cellAttribute = new Cell(new Paragraph(strCellText, universalFont));

                            AlignCell(cellAttribute, AlignStr);
                            if (htab != null)
                                htab.AddCell(cellAttribute);
                        }
                        catch (Exception ex)
                        {
                            Cell cellAttribute = new iTextSharp.text.Cell(new Paragraph(strCellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1F))));
                            AlignCell(cellAttribute, AlignStr);
                            if (htab != null)
                                htab.AddCell(cellAttribute);
                        }
                    }
                    i = i + 1;
                }
                for (i = i; i <= tmpColCount; i++)
                {
                    if (htab != null)
                        htab.AddCell(new iTextSharp.text.Cell(new Paragraph("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1F)))));
                }
            }

            string ivParams = string.Empty;
            if (Request.QueryString["ivParamCaption"] != null)
                ivParams = Request.QueryString["ivParamCaption"].ToString();
            ArrayList _paramList = new ArrayList();
            if (ivParams != string.Empty)
            {
                StringBuilder sbPVal = new StringBuilder();
                string[] pairs = ivParams.Split('♥');
                foreach (var pair in pairs)
                {
                    if (pair != "")
                    {
                        string[] keyValue = pair.Split(':');
                        string kyval = keyValue[1];
                        kyval = kyval.Replace("&grave;", "~");
                        kyval = kyval.Replace("&amp;", "&");
                        _paramList.Add(keyValue[0] + ": " + kyval);
                    }
                }
            }
            // for adding the header in the PDF
            iTextSharp.text.Table Hdrtab;
            Hdrtab = new iTextSharp.text.Table(1);
            Hdrtab.WidthPercentage = 100;
            Hdrtab.TableFitsPage = true;
            Hdrtab.Border = 0;
            Hdrtab.Cellspacing = 1;
            Hdrtab.Cellpadding = 1;

            if (Session["AxShowAppTitle"] != null && Session["AxShowAppTitle"].ToString().ToLower() == "true")
            {
                string headerText = "";
                if (Session["AxAppTitle"] != null && Session["AxAppTitle"] != string.Empty)
                    headerText = Session["AxAppTitle"].ToString();
                else if (Session["projTitle"] != null)
                    headerText = Session["projTitle"].ToString();

                if (!hideProjeName && headerText != string.Empty)
                {
                    try
                    {
                        string fontPath = GetFontPathBasedOnLanguage(headerText);
                        BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                        Cell cellCapAttribute = new Cell(new Paragraph(headerText, universalFont));

                        cellCapAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        Hdrtab.AddCell(cellCapAttribute);
                    }
                    catch (Exception ex)
                    {
                        Cell cellCapAttribute = new Cell(new Paragraph(headerText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                        cellCapAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        Hdrtab.AddCell(cellCapAttribute);
                    }
                }
            }

            if (objIview.ReportHdrs.Count > 0)
            {
                int iv;
                for (iv = 0; iv <= objIview.ReportHdrs.Count - 1; iv += 1)
                {
                    string _alignment = hdrAlign[iv].ToString();
                    string _font = hdrFont[iv].ToString();

                    try
                    {
                        string fontPath = GetFontPathBasedOnLanguage(objIview.ReportHdrs[iv].ToString());
                        BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                        Cell cellAttribute = new Cell(new Paragraph(objIview.ReportHdrs[iv].ToString(), universalFont));

                        if (_alignment == "" || _alignment == "@center")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        else if (_alignment == "@left")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                        else if (_alignment == "@right")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Hdrtab.AddCell(cellAttribute);
                    }
                    catch (Exception ex)
                    {
                        Cell cellAttribute = new Cell(new Phrase(objIview.ReportHdrs[iv].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                        if (_alignment == "" || _alignment == "@center")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        else if (_alignment == "@left")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                        else if (_alignment == "@right")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Hdrtab.AddCell(cellAttribute);
                    }
                }
            }
            else if (iViewCap != null & iViewCap != string.Empty)
            {
                try
                {
                    string fontPath = GetFontPathBasedOnLanguage(iViewCap);
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                    Cell cellCapAttribute = new Cell(new Paragraph(iViewCap, universalFont));

                    cellCapAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                    Hdrtab.AddCell(cellCapAttribute);
                }
                catch (Exception ex)
                {
                    Cell cellCapAttribute = new Cell(new Paragraph(iViewCap, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                    cellCapAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                    Hdrtab.AddCell(cellCapAttribute);
                }
            }

            foreach (string pval in _paramList)
            {
                try
                {
                    string fontPath = GetFontPathBasedOnLanguage(pval);
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                    Cell cellAttribute = new Cell(new Paragraph(pval, universalFont));

                    cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                    Hdrtab.AddCell(cellAttribute);
                }
                catch (Exception ex)
                {
                    Cell cellAttribute = new Cell(new Paragraph(pval, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                    cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                    Hdrtab.AddCell(cellAttribute);
                }
            }
            if (htab != null)
                Hdrtab.InsertTable(htab);
            if (hdrFooterName.Count > 0)
            {
                int iv;
                for (iv = 0; iv <= hdrFooterName.Count - 1; iv += 1)
                {
                    string _alignment = hdrFooterAlign[iv].ToString();
                    string _font = hdrFooterFont[iv].ToString();

                    try
                    {
                        string fontPath = GetFontPathBasedOnLanguage(hdrFooterName[iv].ToString());
                        BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                        Cell cellAttribute = new Cell(new Paragraph(hdrFooterName[iv].ToString(), universalFont));

                        if (_alignment == "" || _alignment == "@center")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        else if (_alignment == "@left")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                        else if (_alignment == "@right")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Hdrtab.AddCell(cellAttribute);
                    }
                    catch (Exception ex)
                    {
                        Cell cellAttribute = new Cell(new Phrase(hdrFooterName[iv].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                        if (_alignment == "" || _alignment == "@center")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        else if (_alignment == "@left")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                        else if (_alignment == "@right")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Hdrtab.AddCell(cellAttribute);
                    }
                }
            }
            string _currentUser = string.Empty;
            if (HttpContext.Current.Session["username"] != null && HttpContext.Current.Session["username"].ToString() != "")
            {
                _currentUser = HttpContext.Current.Session["username"].ToString();
            }
            if (_currentUser != string.Empty)
            {
                string PrintedBy = "Printed by: " + _currentUser + " and Printed On: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                Cell cellAttribute = new Cell(new Phrase(PrintedBy, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 6, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                Hdrtab.AddCell(cellAttribute);
            }

            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            ScriptsPath += @"Axpert\";
            DirectoryInfo di = new DirectoryInfo(ScriptsPath + sid);
            if (di.Exists)
            {
            }
            else
                di.Create();

            string strPath;
            if (HttpContext.Current.Session["AxPDFConfigs"] == null)
            {
                util.GetAxExportConfig();
            }
            iTextSharp.text.Document mydocument;
            string _fileName = objIview.IviewCaption.Replace(",", "");
            _fileName = _fileName.Replace(" ", "_");
            if (HttpContext.Current.Session["AxPDFMargin"] != null && HttpContext.Current.Session["AxPDFMargin"].ToString() != "")
            {
                int top = 54;
                int bottom = 54;
                int left = 36;
                int right = 36;
                string pdfMargins = HttpContext.Current.Session["AxPDFMargin"].ToString();
                Dictionary<string, string> dimensions = JsonConvert.DeserializeObject<Dictionary<string, string>>(pdfMargins);
                foreach (var entry in dimensions)
                {
                    if (entry.Key == "top")
                        top = top * int.Parse(entry.Value);
                    if (entry.Key == "bottom")
                        bottom = bottom * int.Parse(entry.Value);
                    if (entry.Key == "left")
                        left = left * int.Parse(entry.Value);
                    if (entry.Key == "right")
                        right = right * int.Parse(entry.Value);
                }
                bool isLandscape = false;
                if (HttpContext.Current.Session["AxPDFOrientation"] != null && HttpContext.Current.Session["AxPDFOrientation"].ToString().ToLower() == "landscape")
                {
                    isLandscape = true;
                }
                Rectangle pageSize = isLandscape ? PageSize.A4.Rotate() : PageSize.A4;
                mydocument = new iTextSharp.text.Document(pageSize, left, right, top, bottom);
            }
            else
                mydocument = new iTextSharp.text.Document();
            strPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            if (iViewCap != null && iViewCap != "")
                strPath = strPath + @"Axpert\" + sid + @"\" + iViewCap + ".pdf";
            else if (_fileName != string.Empty)
                strPath = strPath + @"Axpert\" + sid + @"\" + _fileName + ".pdf";
            else
                strPath = strPath + @"Axpert\" + sid + @"\view.pdf";
            PdfWriter.GetInstance(mydocument, new FileStream(strPath, FileMode.Create));
            mydocument.Open();
            mydocument.Add(Hdrtab);
            mydocument.Close();
            return strPath;
        }
        return "";
    }

    private static readonly string fontsFolder = HttpContext.Current.Server.MapPath("~/fonts/");
    private static readonly Regex ArabicRegex = new Regex(@"[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\uFB50-\uFDFF\uFE70-\uFEFF\uFDFD]", RegexOptions.Compiled);
    private static readonly Regex HindiRegex = new Regex(@"[\u0900-\u097F]", RegexOptions.Compiled);
    private static readonly Regex TamilRegex = new Regex(@"[\u0B80-\u0BFF]", RegexOptions.Compiled);
    private static readonly Regex TeluguRegex = new Regex(@"[\u0C00-\u0C7F]", RegexOptions.Compiled);
    private static readonly Regex KannadaRegex = new Regex(@"[\u0C80-\u0CFF]", RegexOptions.Compiled);
    private static readonly Regex MalayalamRegex = new Regex(@"[\u0D00-\u0D7F]", RegexOptions.Compiled);
    private static readonly Regex GurmukhiRegex = new Regex(@"[\u0A00-\u0A7F]", RegexOptions.Compiled);
    private static readonly Regex GujaratiRegex = new Regex(@"[\u0A80-\u0AFF]", RegexOptions.Compiled);
    private static readonly Regex BengaliRegex = new Regex(@"[\u0980-\u09FF]", RegexOptions.Compiled);
    private static readonly Regex OriyaRegex = new Regex(@"[\u0B00-\u0B7F]", RegexOptions.Compiled);
    private static readonly Regex SinhalaRegex = new Regex(@"[\u0D80-\u0DFF]", RegexOptions.Compiled);
    private static readonly Regex CjkRegex = new Regex(@"[\u4E00-\u9FFF]", RegexOptions.Compiled);

    public static string GetFontPathBasedOnLanguage(string input)
    {
        if (string.IsNullOrEmpty(input))
            return GetDefaultFontPath();

        if (ArabicRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansArabic-Regular.ttf");

        if (HindiRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansDevanagari-Regular.ttf");

        if (TamilRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansTamil-Regular.ttf");

        if (TeluguRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansTelugu-Regular.ttf");

        if (KannadaRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansKannada-Regular.ttf");

        if (MalayalamRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansMalayalam-Regular.ttf");

        if (GurmukhiRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansGurmukhi-Regular.ttf");

        if (GujaratiRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansGujarati-Regular.ttf");

        if (BengaliRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansBengali-Regular.ttf");

        if (OriyaRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansOriya-Regular.ttf");

        if (SinhalaRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansSinhala-Regular.ttf");

        if (CjkRegex.IsMatch(input))
            return Path.Combine(fontsFolder, "NotoSansCJKsc-Regular.otf");

        return GetDefaultFontPath(); // fallback
    }
    private static string GetDefaultFontPath()
    {
        return Path.Combine(fontsFolder, "NotoSans-Regular.ttf");
    }
    public string StripHtmlTags(string input)
    {
        // Use a regular expression to remove all HTML tags
        return Regex.Replace(input, "<.*?>", String.Empty);
    }

    [WebMethod]
    public static string GeneratePDFMobile(string ivKey, string ivname, string ivtype, string axpCache, string _params, string _typeIvOrLv, string _curRecord, string _ivParamCaption)
    {
        if (HttpContext.Current.Session["project"].ToString() == "")
        {
            return "error";
        }
        else
        {
            Util.Util util = new Util.Util();
            IviewData objIview = new IviewData();
            string Ikey = ivKey;
            string iname = ivname;
            if (iname != null)
            {
                if (!util.IsValidIvName(iname))
                    return "error";
            }
            string user = HttpContext.Current.Session["user"].ToString();
            string sid = HttpContext.Current.Session["nsessionid"].ToString();
            if (ivtype != null)
            {
                if (ivtype == null & !util.IsChar(ivtype))
                    return "error";
            }
            string paramVal = string.Empty;
            if (axpCache != null)
            {
                paramVal = HttpContext.Current.Session["AxIvExportParams-" + iname].ToString();
                HttpContext.Current.Session["AxIvExportParams-" + iname] = "";
            }
            else
                paramVal = _params.ToString();

            if (iname == null & HttpContext.Current.Session["ivname"] != null/* TODO Change to default(_) if this is not a reference type */ )
                iname = HttpContext.Current.Session["ivname"].ToString();
            if (paramVal == null & HttpContext.Current.Session["params"] != null/* TODO Change to default(_) if this is not a reference type */ )
                paramVal = HttpContext.Current.Session["params"].ToString();
            if (ivtype == null & HttpContext.Current.Session["ivtype"] != null/* TODO Change to default(_) if this is not a reference type */ )
                ivtype = HttpContext.Current.Session["ivtype"].ToString();

            objIview = (IviewData)HttpContext.Current.Session[Ikey];
            objIview.ReportHdrs = new ArrayList();
            //ViewState["ivtype"] = ivtype;
            //ViewState["params"] = paramVal;
            string proj = HttpContext.Current.Session["project"].ToString();
            //ViewState["iname"] = iname;
            //ViewState["ivtype"] = ivtype;
            //ViewState["proj"] = proj;
            //ViewState["user"] = user;
            //ViewState["sid"] = sid;

            string typeIvOrLv = _typeIvOrLv.ToString();

            string curRecord = _curRecord.ToString();

            string str = GetPDFMobile(ivKey, ivname, ivtype, axpCache, paramVal, _typeIvOrLv, _curRecord, objIview, _ivParamCaption);
            //HttpContext.Current.Response.Buffer = true;
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.ClearHeaders();
            System.IO.FileInfo files = new System.IO.FileInfo(str);
            //HttpContext.Current.Response.AddHeader("Content-disposition", "attachment;filename=" + files.Name);
            //HttpContext.Current.Response.ContentType = "application/pdf";
            //HttpContext.Current.Response.WriteFile(files.FullName);
            //HttpContext.Current.Response.End();
            return files.Name;
        }
    }

    private static string GetPDFMobile(string ivKey, string ivname, string ivtype, string axpCache, string paramVal, string _typeIvOrLv, string _curRecord, IviewData objIview, string _ivParamCaption)
    {
        string isList;
        if (ivtype == "listview" | ivtype == "lview")
            isList = "true";
        else
            isList = "false";
        string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
        Util.Util util = new Util.Util();
        ArrayList colHide = new ArrayList();
        ArrayList colFld = new ArrayList();
        ArrayList colWidth = new ArrayList();
        ArrayList colHead = new ArrayList();
        ArrayList ivhead = new ArrayList();
        ArrayList arrLstParams = new ArrayList();
        ArrayList arrLstParamVal = new ArrayList();

        ArrayList pivotGroupHeaderNames = new ArrayList();
        ArrayList pivotStartCol = new ArrayList();
        ArrayList pivotEndCol = new ArrayList();
        ArrayList htmlColumns = new ArrayList();
        bool hideProjeName = false;
        string iViewCap = string.Empty;

        ArrayList hdrFont = new ArrayList();
        ArrayList hdrAlign = new ArrayList();

        ArrayList hdrFooterName = new ArrayList();
        ArrayList hdrFooterAlign = new ArrayList();
        ArrayList hdrFooterFont = new ArrayList();
        ArrayList axhiddencols = new ArrayList();

        string paramXml = GetParamMobile(paramVal, paramVal);

        if (paramXml != string.Empty)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<params>" + paramXml + "</params>");
            XmlNode axHiddenColumnNode = null;
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                if (string.Equals(node.Name, "axhiddencolumn", StringComparison.OrdinalIgnoreCase))
                {
                    axHiddenColumnNode = node;
                    break;
                }
            }
            if (axHiddenColumnNode != null)
            {
                if (axHiddenColumnNode.InnerText != "")
                {
                    foreach (string _thiscolName in axHiddenColumnNode.InnerText.Split(','))
                        axhiddencols.Add(_thiscolName);
                }
            }
        }

        string sortCol = string.Empty;
        string sortOrd = string.Empty;
        string filterCol = string.Empty;
        string filterColVal = string.Empty;
        string filterValue1 = string.Empty;
        string filterOpr = string.Empty;

        if (HttpContext.Current.Session["sOrder"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (HttpContext.Current.Session["sOrder"].ToString() != "")
                sortOrd = HttpContext.Current.Session["sOrder"].ToString();
        }

        if (HttpContext.Current.Session["sColumn"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (HttpContext.Current.Session["sColumn"].ToString() != "")
                sortCol = HttpContext.Current.Session["sColumn"].ToString();
        }

        if (HttpContext.Current.Session["fcolopr"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (HttpContext.Current.Session["fcolopr"].ToString() != "")
                filterOpr = HttpContext.Current.Session["fcolopr"].ToString();
        }

        if (HttpContext.Current.Session["fCol"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (HttpContext.Current.Session["fCol"].ToString() != "")
                filterCol = HttpContext.Current.Session["fCol"].ToString();
        }

        if (HttpContext.Current.Session["fColVal"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (HttpContext.Current.Session["fColVal"].ToString() != "")
                filterColVal = HttpContext.Current.Session["fColVal"].ToString();
        }

        if (HttpContext.Current.Session["fColVal2"] != null/* TODO Change to default(_) if this is not a reference type */ )
        {
            if (HttpContext.Current.Session["fColVal2"].ToString() != "")
                filterValue1 = HttpContext.Current.Session["fColVal2"].ToString();
        }
        string iname = ivname;
        string fileName = "GetLView" + ivname;
        LogFile.Log logobj = new LogFile.Log();
        string sid = HttpContext.Current.Session["nsessionid"].ToString();
        string errorLog = logobj.CreateLog("GetLView.", sid, fileName, "new");
        string custXML = string.Empty;
        string iXml;
        string _thisList = string.Empty;
        if (_typeIvOrLv == "true")
            _thisList = " purpose=\"list\" ";
        if (_curRecord == "")
            iXml = "<root " + _thisList + " headercached='false' name ='" + iname + "' axpapp ='" + HttpContext.Current.Session["project"].ToString() + "' sessionid = '" + sid + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errorLog + "' pageno='0' firsttime='yes' sorder='" + sortOrd + "' scol='" + sortCol + "' fcolopr='" + filterOpr + "' fcolnm='" + filterCol + "' fcolval1='" + filterColVal + "' fcolval2='" + filterValue1 + "'><params> ";
        else
            iXml = "<root " + _thisList + " headercached='false' name ='" + iname + "' axpapp ='" + HttpContext.Current.Session["project"].ToString() + "' sessionid = '" + sid + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errorLog + "' pageno='1' pagesize='" + _curRecord + "' firsttime='yes' sqlpagination='true' getrowcount='false' gettotalrows='false' smartview='true'><params> ";
        iXml = iXml + paramXml;
        iXml = iXml + "</params>";
        iXml = iXml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        string ires = string.Empty;
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        try
        {
            objIview.requestJSON = true;
            ires = objWebServiceExt.CallGetIViewWS(iname, iXml, "", objIview);
            if (ires != null)
            {
                ires = ires.Split('♠')[1];

                if (ires.StartsWith("<"))
                    ires = util.ReplaceFirstOccurrence(ires, "#$#", "#$♥#");
                string[] resultSplitter = ires.Split(new[] { "#$♥#" }, StringSplitOptions.None);
                ires = resultSplitter[0];
            }
        }
        catch (Exception Ex)
        {
        }

        string strErrMsg = string.Empty;
        strErrMsg = util.ParseXmlErrorNode(ires);

        if (strErrMsg != string.Empty)
        {
            strErrMsg = strErrMsg.Replace("\r", "").Replace("\n", "");
            if (strErrMsg == Constants.SESSIONERROR)
            {
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();
                return "error";
            }
            else
                return "error";
        }
        else
        {
            if (ires != string.Empty)
            {
                JObject jsonObject = JObject.Parse(ires);
                JObject data = (JObject)jsonObject["data"];
                XDocument xmlDocument = new XDocument();
                XElement root = new XElement("data");
                XElement dataElement = JsonConvert.DeserializeXNode(data.ToString(), "data").Root;
                root.Add(dataElement.Elements());
                xmlDocument.Add(root);
                ires = xmlDocument.ToString();

                XmlDocument xmlDocNew = new XmlDocument();
                xmlDocNew.LoadXml(ires);
                string datarows = "";
                string totalrows = "";
                string reccount = "";
                XmlNodeList nodesToRemove = xmlDocNew.SelectNodes("//reccount | //totalrows | //datarows");
                foreach (XmlNode node in nodesToRemove)
                {
                    if (node.Name == "datarows")
                        datarows = node.InnerText;
                    if (node.Name == "totalrows")
                        totalrows = node.InnerText;
                    if (node.Name == "reccount")
                        reccount = node.InnerText;

                    node.ParentNode.RemoveChild(node);
                }
                XmlNode metadataNode = xmlDocNew.SelectSingleNode("//metadata");
                if (metadataNode != null)
                {
                    XmlElement newElement = xmlDocNew.CreateElement("headrow");
                    foreach (XmlAttribute attribute in metadataNode.Attributes)
                    {
                        newElement.Attributes.Append((XmlAttribute)attribute.CloneNode(true));
                    }
                    foreach (XmlNode childNode in metadataNode.ChildNodes)
                    {
                        newElement.AppendChild(childNode.CloneNode(true));
                    }
                    metadataNode.ParentNode.ReplaceChild(newElement, metadataNode);
                }

                ires = xmlDocNew.OuterXml;
                XDocument doc = XDocument.Parse(@ires);
                //doc.Descendants("axrowtype").Remove();
                //doc.Descendants("axp__font").Remove();
                var elementsToRemove = doc.Descendants().Where(e =>
       string.Equals(e.Name.LocalName, "axrowtype", StringComparison.OrdinalIgnoreCase) ||
       string.Equals(e.Name.LocalName, "axp__font", StringComparison.OrdinalIgnoreCase) ||
       string.Equals(e.Name.LocalName, "axp__color", StringComparison.OrdinalIgnoreCase) ||
       string.Equals(e.Name.LocalName, "axrowoptions", StringComparison.OrdinalIgnoreCase)).ToList();
                foreach (var el in elementsToRemove)
                    el.Remove();
                ires = doc.ToString();
            }

            ires = _xmlString + ires;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ires);

            pivotGroupHeaderNames.Clear();
            pivotStartCol.Clear();
            pivotEndCol.Clear();

            XmlNodeList productNodes;
            XmlNodeList baseDataNodes;
            productNodes = xmlDoc.SelectNodes("//headrow");
            int hcolNos;
            hcolNos = 0;

            foreach (XmlNode productNode in productNodes)
            {
                baseDataNodes = productNode.ChildNodes;
                int iCount = -1;
                foreach (XmlNode baseDataNode in baseDataNodes)
                {
                    iCount += 1;
                    colFld.Add(baseDataNode.Name);
                    colHead.Add(baseDataNode.InnerText);
                    if (baseDataNode.Attributes["width"] != null)
                        colWidth.Add(baseDataNode.Attributes["width"].Value);
                    else
                        colWidth.Add("0");

                    if (baseDataNode.Name == "rowno")
                    {
                        if (ivtype == "listview" | ivtype == "lview")
                            colHide.Add("false");
                        else
                            colHide.Add("true");
                    }
                    else
                    {
                        if (baseDataNode.Name.StartsWith("html_"))
                            htmlColumns.Add(baseDataNode.Name);

                        if (baseDataNode.Attributes["hide"] != null)
                        {
                            if (baseDataNode.Name.StartsWith("hide_"))
                                colHide.Add("true");
                            else
                                colHide.Add(baseDataNode.Attributes["hide"].Value);
                        }
                        if (baseDataNode.Name == "pivotghead")
                        {
                            XmlNodeList finalNodelist;
                            foreach (XmlNode base2node in baseDataNode)
                            {
                                finalNodelist = base2node.ChildNodes;
                                foreach (XmlNode finalNode in finalNodelist)
                                {
                                    if (finalNode.Name == "sn")
                                        pivotStartCol.Add(finalNode.InnerText);
                                    else if (finalNode.Name == "ghead")
                                        pivotGroupHeaderNames.Add(finalNode.InnerText);
                                    else if (finalNode.Name == "en")
                                        pivotEndCol.Add(finalNode.InnerText);
                                }
                            }
                            colHide.Add("true");
                        }

                        hcolNos = hcolNos + 1;
                    }
                }
            }

            var htIdx = 0;
            var cIdx = 0;
            for (htIdx = 0; htIdx <= htmlColumns.Count - 1; htIdx++)
            {
                for (cIdx = 0; cIdx <= colFld.Count - 1; cIdx++)
                {
                    string htmlColName = htmlColumns[htIdx].ToString();
                    if (colFld[cIdx] == htmlColName)
                        colHide[cIdx] = "false";
                    else if (colFld[cIdx] == htmlColName.Replace("html_", ""))
                        colHide[cIdx] = "true";
                }
            }

            // to remove attributes from headrow
            XmlNodeList productNodes2;
            XmlNodeList baseDataNodes2;
            productNodes2 = xmlDoc.SelectNodes("//headrow");

            foreach (XmlNode productNode2 in productNodes2)
            {
                baseDataNodes2 = productNode2.ChildNodes;
                foreach (XmlNode baseDataNode2 in baseDataNodes2)
                    baseDataNode2.Attributes.RemoveAll();
            }

            XmlNodeList compNodes;
            XmlNodeList cbaseDataNodes;
            compNodes = xmlDoc.SelectNodes("//data/reporthf");
            foreach (XmlNode compNode in compNodes)
            {
                cbaseDataNodes = compNode.ChildNodes;
                foreach (XmlNode cbaseDataNode in cbaseDataNodes)
                {
                    if (cbaseDataNode.Name == "hideprojname")
                    {
                        string hideprojname = cbaseDataNode.InnerText;
                        if (hideprojname != "" && hideprojname == "@t")
                        {
                            hideProjeName = true;
                        }
                    }
                    else if (Strings.Mid(cbaseDataNode.Name, 1, 7) == "x__head")
                    {
                        iViewCap = cbaseDataNode.Attributes["caption"].Value;
                    }
                    else if (Strings.Mid(cbaseDataNode.Name, 1, 3) == "lbl")
                    {
                        iViewCap = cbaseDataNode.Attributes["caption"].Value;
                    }
                    else if (cbaseDataNode.Name == "header")
                    {
                        foreach (XmlNode childHeaders in cbaseDataNode)
                        {
                            string xmlContent = childHeaders.OuterXml;
                            XmlDocument _xmlDoc = new XmlDocument();
                            _xmlDoc.LoadXml(xmlContent);
                            XmlNode textNode = _xmlDoc.SelectSingleNode("//text");
                            XmlNode textNodefont = _xmlDoc.SelectSingleNode("//font");
                            if (textNodefont != null)
                                hdrFont.Add(textNodefont.InnerText);
                            else
                                hdrFont.Add("");
                            XmlNode textNodeheader_aline = _xmlDoc.SelectSingleNode("//header_aline");
                            if (textNodeheader_aline != null)
                                hdrAlign.Add(textNodeheader_aline.InnerText);
                            else
                                hdrAlign.Add("");
                            objIview.ReportHdrs.Add(textNode.InnerText.Trim());
                        }
                    }
                    if (cbaseDataNode.Name == "footer")
                    {
                        foreach (XmlNode childFooters in cbaseDataNode)
                        {
                            string xmlContent = childFooters.OuterXml;
                            XmlDocument _xmlDoc = new XmlDocument();
                            _xmlDoc.LoadXml(xmlContent);
                            XmlNode textNode = _xmlDoc.SelectSingleNode("//text");
                            hdrFooterName.Add(textNode.InnerText);
                            XmlNode textNodefont = _xmlDoc.SelectSingleNode("//font");
                            if (textNodefont != null)
                                hdrFooterFont.Add(textNodefont.InnerText);
                            else
                                hdrFooterFont.Add("");
                            XmlNode textNodeheader_aline = _xmlDoc.SelectSingleNode("//footer_aline");
                            if (textNodeheader_aline != null)
                                hdrFooterAlign.Add(textNodeheader_aline.InnerText);
                            else
                                hdrFooterAlign.Add(hdrFooterAlign[0]);
                        }
                    }
                }
            }


            // Remove Comps
            XmlNode cNode;
            cNode = xmlDoc.SelectSingleNode("//comps");
            if (cNode != null)
                cNode.ParentNode.RemoveChild(cNode);

            // remove hidden fields in headrow
            int hrep;
            // change  later try new code to remove this for
            for (hrep = 1; hrep <= hcolNos; hrep++)
            {
                XmlNodeList productNodes3;
                XmlNodeList baseDataNodes3;
                productNodes3 = xmlDoc.SelectNodes("//headrow");

                int hidx;

                foreach (XmlNode productNode3 in productNodes3)
                {
                    baseDataNodes3 = productNode3.ChildNodes;
                    foreach (XmlNode baseDataNode3 in baseDataNodes3)
                    {
                        for (hidx = 0; hidx <= colFld.Count - 1; hidx++)
                        {
                            if (colFld[hidx] == baseDataNode3.Name)
                            {
                                try
                                {
                                    if (colHide[hidx].ToString() == "true")
                                        baseDataNode3.ParentNode.RemoveChild(baseDataNode3);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
            }

            // remove hidden fields
            int rep;
            // change  later try new code to remove this for
            for (rep = 1; rep <= hcolNos; rep++)
            {
                XmlNodeList productNodes1;
                XmlNodeList baseDataNodes1;
                productNodes1 = xmlDoc.SelectNodes("//row");

                int idx;
                foreach (XmlNode productNode1 in productNodes1)
                {
                    baseDataNodes1 = productNode1.ChildNodes;
                    foreach (XmlNode baseDataNode1 in baseDataNodes1)
                    {
                        for (idx = 0; idx <= colFld.Count - 1; idx++)
                        {
                            try
                            {
                                if (colFld[idx].ToString().ToLower() == baseDataNode1.Name.ToLower())
                                {
                                    if (colHide[idx].ToString() == "true")
                                        baseDataNode1.ParentNode.RemoveChild(baseDataNode1);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }

            // PDF creation starts
            // find no of visible columns

            int viscols;
            viscols = 0;
            int g;
            if (ivtype == "listview" | ivtype == "lview" | ivtype.ToLower() == "iview")
            {
                for (g = 0; g <= colHide.Count - 1; g++)
                {
                    if (colHide[g].ToString() == "false")
                    {
                        if (axhiddencols.IndexOf(colFld[g].ToString()) == -1)
                            viscols = viscols + 1;
                    }
                }
            }
            else
                for (g = 1; g <= colHide.Count - 1; g++)
                {
                    if (colHide[g].ToString() == "false")
                    {
                        if (axhiddencols.IndexOf(colFld[g].ToString()) == -1)
                            viscols = viscols + 1;
                    }
                }

            int[] widthscell = new int[viscols - 1 + 1];
            int clcont = 0;
            if (ivtype == "listview" | ivtype == "lview" | ivtype.ToLower() == "iview")
            {
                for (g = 0; g <= colHide.Count - 1; g++)
                {
                    if (colHide[g].ToString() == "false")
                    {
                        if (colFld.Count > 0)
                        {
                            if (axhiddencols.IndexOf(colFld[g].ToString()) > -1)
                                continue;
                            if (colFld[g].ToString() == "rowno")
                            {
                                widthscell[clcont] = 50;
                                clcont = clcont + 1;
                            }
                            else
                            {
                                if (colWidth != null)
                                    widthscell[clcont] = int.Parse(colWidth[colFld.IndexOf(colFld[g])].ToString());
                                else
                                    widthscell[clcont] = int.Parse(colWidth[g].ToString());
                                clcont = clcont + 1;
                            }
                        }
                        else if (colWidth.Count > 0)
                        {
                            if (axhiddencols.IndexOf(colFld[g].ToString()) > -1)
                                continue;
                            if (colFld[g].ToString() == "rowno")
                            {
                                widthscell[clcont] = 50;
                                clcont = clcont + 1;
                            }
                            else
                            {
                                widthscell[clcont] = int.Parse(colWidth[g].ToString());
                                clcont = clcont + 1;
                            }
                        }
                    }
                }
            }
            else
                for (g = 1; g <= colHide.Count - 1; g++)
                {
                    if (colHide[g].ToString() == "false")
                    {
                        if (axhiddencols.IndexOf(colFld[g].ToString()) == -1)
                            continue;
                        if (colWidth != null)
                            widthscell[clcont] = int.Parse(colWidth[colFld.IndexOf(colFld[g])].ToString());
                        else
                            widthscell[clcont] = int.Parse(colWidth[g].ToString());
                        clcont = clcont + 1;
                    }
                }

            iTextSharp.text.Table htab;
            if (viscols == 0)
                viscols = 1;
            htab = new iTextSharp.text.Table(viscols);
            htab.WidthPercentage = 100;
            htab.Border = 1;
            htab.Cellspacing = 1;
            htab.Cellpadding = 1;
            htab.SetWidths(widthscell);

            int inc;
            int colSpan;
            int totColSpan = 0;
            for (inc = 0; inc <= pivotGroupHeaderNames.Count - 1; inc++)
            {
                string hn;
                colSpan = System.Convert.ToInt32(pivotEndCol[inc].ToString()) - System.Convert.ToInt32(pivotStartCol[inc].ToString());
                hn = pivotGroupHeaderNames[inc].ToString();
                try
                {
                    string fontPath = GetFontPathBasedOnLanguage(hn);
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                    hn = util.ReplaceTextAreaChars(hn, "pdf");
                    hn = StripHtmlTagsMobile(hn);

                    Cell cellAttribute = new Cell(new Paragraph(hn, universalFont));

                    cellAttribute.Colspan = colSpan;
                    cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                    htab.AddCell(cellAttribute);
                }
                catch (Exception)
                {
                    Cell cellAttribute = new Cell(new Phrase(hn, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                    cellAttribute.Colspan = colSpan;
                    cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                    htab.AddCell(cellAttribute);
                }
                totColSpan += colSpan;
            }

            XmlNodeList hproductNodes;
            XmlNodeList hbaseDataNodes;
            int tmpColCount = 0;
            hproductNodes = xmlDoc.SelectNodes("/data/headrow");
            foreach (XmlNode hproductNode in hproductNodes)
            {
                hbaseDataNodes = hproductNode.ChildNodes;
                tmpColCount = hbaseDataNodes.Count;
                //Response.Write(hbaseDataNodes.Count);
                foreach (XmlNode hbaseDataNode in hbaseDataNodes)
                {
                    string hn;
                    if (hbaseDataNode.Name == "rowno")
                    {
                        if (ivtype == "listview" | ivtype == "lview")
                        {
                            hn = "Sr. No.";
                            try
                            {
                                string fontPath = GetFontPathBasedOnLanguage(hn);
                                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                                iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                                hn = util.ReplaceTextAreaChars(hn, "pdf");
                                hn = StripHtmlTagsMobile(hn);

                                Cell cellAttribute = new Cell(new Paragraph(hn, universalFont));

                                cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                                htab.AddCell(cellAttribute);
                            }
                            catch (Exception)
                            {
                                Cell cellAttribute = new Cell(new Phrase(hn, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                                cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                                htab.AddCell(cellAttribute);
                            }
                        }
                    }
                    else
                    {
                        hn = hbaseDataNode.InnerText;
                        if (axhiddencols.IndexOf(hn) > -1)
                        {
                            tmpColCount--;
                            continue;
                        }
                        if (hn == "axp_slno")
                            hn = "Sr. No.";
                        if ((hn.IndexOf("~") != -1))
                            hn = hn.Replace("~", "\n");

                        try
                        {
                            string fontPath = GetFontPathBasedOnLanguage(hn);
                            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                            hn = util.ReplaceTextAreaChars(hn, "pdf");
                            hn = StripHtmlTagsMobile(hn);

                            Cell cellAttribute = new Cell(new Paragraph(hn, universalFont));

                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                            if (htab != null)
                                htab.AddCell(cellAttribute);
                        }
                        catch (Exception)
                        {
                            Cell cellAttribute = new Cell(new Phrase(hn, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                            if (htab != null)
                                htab.AddCell(cellAttribute);
                        }
                    }
                }
            }

            // Get Data
            XmlNodeList dproductNodes;
            XmlNodeList dbaseDataNodes;
            string AlignStr = string.Empty;
            dproductNodes = xmlDoc.SelectNodes("/data/row");
            foreach (XmlNode dproductNode in dproductNodes)
            {
                dbaseDataNodes = dproductNode.ChildNodes;
                int i = 1;

                foreach (XmlNode dbaseDataNode in dbaseDataNodes)
                {
                    string strCellText = string.Empty;
                    if (dbaseDataNode.Name == "rowno")
                    {
                        if (ivtype == "listview" | ivtype == "lview")
                        {
                            strCellText = dbaseDataNode.InnerText.ToString();
                            strCellText = util.ReplaceTextAreaChars(strCellText, "pdf");
                            strCellText = StripHtmlTagsMobile(strCellText);

                            try
                            {
                                string fontPath = GetFontPathBasedOnLanguage(strCellText);
                                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                                iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1f));

                                Cell cellAttribute = new Cell(new Paragraph(strCellText, universalFont));

                                cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                                if (htab != null)
                                    htab.AddCell(cellAttribute);
                            }
                            catch (Exception)
                            {
                                Cell cellAttribute = new iTextSharp.text.Cell(new Paragraph(strCellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1F))));
                                cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                                if (htab != null)
                                    htab.AddCell(cellAttribute);
                            }
                        }
                        else
                            continue;
                    }
                    else
                    {
                        if (axhiddencols.IndexOf(dbaseDataNode.Name) > -1)
                            continue;
                        strCellText = dbaseDataNode.InnerText.ToString();
                        strCellText = util.ReplaceTextAreaChars(strCellText, "pdf");
                        strCellText = StripHtmlTagsMobile(strCellText);

                        try
                        {
                            string fontPath = GetFontPathBasedOnLanguage(strCellText);
                            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1f));

                            Cell cellAttribute = new Cell(new Paragraph(strCellText, universalFont));

                            AlignCellMobile(cellAttribute, AlignStr);
                            if (htab != null)
                                htab.AddCell(cellAttribute);
                        }
                        catch (Exception)
                        {
                            Cell cellAttribute = new iTextSharp.text.Cell(new Paragraph(strCellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1F))));
                            AlignCellMobile(cellAttribute, AlignStr);
                            if (htab != null)
                                htab.AddCell(cellAttribute);
                        }
                    }
                    i = i + 1;
                }
                for (i = i; i <= tmpColCount; i++)
                {
                    if (htab != null)
                        htab.AddCell(new iTextSharp.text.Cell(new Paragraph("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL, new GrayColor(0.1F)))));
                }
            }

            string ivParams = string.Empty;
            if (_ivParamCaption != null)
                ivParams = _ivParamCaption;
            ArrayList _paramList = new ArrayList();
            if (ivParams != string.Empty)
            {
                StringBuilder sbPVal = new StringBuilder();
                string[] pairs = ivParams.Split('♥');
                foreach (var pair in pairs)
                {
                    if (pair != "")
                    {
                        string[] keyValue = pair.Split(':');
                        string kyval = keyValue[1];
                        kyval = kyval.Replace("&grave;", "~");
                        kyval = kyval.Replace("&amp;", "&");
                        _paramList.Add(keyValue[0] + ": " + kyval);
                    }
                }
            }
            // for adding the header in the PDF
            iTextSharp.text.Table Hdrtab;
            Hdrtab = new iTextSharp.text.Table(1);
            Hdrtab.WidthPercentage = 100;
            Hdrtab.TableFitsPage = true;
            Hdrtab.Border = 0;
            Hdrtab.Cellspacing = 1;
            Hdrtab.Cellpadding = 1;

            if (HttpContext.Current.Session["AxShowAppTitle"] != null && HttpContext.Current.Session["AxShowAppTitle"].ToString().ToLower() == "true")
            {
                string headerText = "";
                if (HttpContext.Current.Session["AxAppTitle"] != null && HttpContext.Current.Session["AxAppTitle"] != string.Empty)
                    headerText = HttpContext.Current.Session["AxAppTitle"].ToString();
                else if (HttpContext.Current.Session["projTitle"] != null)
                    headerText = HttpContext.Current.Session["projTitle"].ToString();

                if (!hideProjeName && headerText != string.Empty)
                {
                    Cell cellCapAttribute = new Cell(new Paragraph(headerText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                    cellCapAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                    Hdrtab.AddCell(cellCapAttribute);
                }
            }

            if (objIview.ReportHdrs.Count > 0)
            {
                int iv;
                for (iv = 0; iv <= objIview.ReportHdrs.Count - 1; iv += 1)
                {
                    string _alignment = hdrAlign[iv].ToString();
                    string _font = hdrFont[iv].ToString();

                    try
                    {
                        string fontPath = GetFontPathBasedOnLanguage(objIview.ReportHdrs[iv].ToString());
                        BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                        Cell cellAttribute = new Cell(new Paragraph(objIview.ReportHdrs[iv].ToString(), universalFont));

                        if (_alignment == "" || _alignment == "@center")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        else if (_alignment == "@left")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                        else if (_alignment == "@right")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Hdrtab.AddCell(cellAttribute);
                    }
                    catch (Exception)
                    {
                        Cell cellAttribute = new Cell(new Phrase(objIview.ReportHdrs[iv].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                        if (_alignment == "" || _alignment == "@center")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        else if (_alignment == "@left")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                        else if (_alignment == "@right")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Hdrtab.AddCell(cellAttribute);
                    }
                }
            }
            else if (iViewCap != null & iViewCap != string.Empty)
            {
                Cell cellCapAttribute = new Cell(new Paragraph(iViewCap, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                cellCapAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                Hdrtab.AddCell(cellCapAttribute);
            }

            foreach (string pval in _paramList)
            {
                Cell cellAttribute = new Cell(new Paragraph(pval, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                Hdrtab.AddCell(cellAttribute);
            }
            if (htab != null)
                Hdrtab.InsertTable(htab);
            if (hdrFooterName.Count > 0)
            {
                int iv;
                for (iv = 0; iv <= hdrFooterName.Count - 1; iv += 1)
                {
                    string _alignment = hdrFooterAlign[iv].ToString();
                    string _font = hdrFooterFont[iv].ToString();

                    try
                    {
                        string fontPath = GetFontPathBasedOnLanguage(objIview.ReportHdrs[iv].ToString());
                        BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font universalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1f));

                        Cell cellAttribute = new Cell(new Paragraph(objIview.ReportHdrs[iv].ToString(), universalFont));

                        if (_alignment == "" || _alignment == "@center")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        else if (_alignment == "@left")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                        else if (_alignment == "@right")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Hdrtab.AddCell(cellAttribute);
                    }
                    catch (Exception)
                    {
                        Cell cellAttribute = new Cell(new Phrase(hdrFooterName[iv].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                        if (_alignment == "" || _alignment == "@center")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_CENTER;
                        else if (_alignment == "@left")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_LEFT;
                        else if (_alignment == "@right")
                            cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Hdrtab.AddCell(cellAttribute);
                    }
                }
            }

            string _currentUser = string.Empty;
            if (HttpContext.Current.Session["username"] != null && HttpContext.Current.Session["username"].ToString() != "")
            {
                _currentUser = HttpContext.Current.Session["username"].ToString();
            }
            if (_currentUser != string.Empty)
            {
                string PrintedBy = "Printed by: " + _currentUser + " and Printed On: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                Cell cellAttribute = new Cell(new Phrase(PrintedBy, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 6, iTextSharp.text.Font.BOLD, new GrayColor(0.1F))));
                cellAttribute.HorizontalAlignment = Element.ALIGN_RIGHT;
                Hdrtab.AddCell(cellAttribute);
            }

            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            ScriptsPath += @"Axpert\";
            DirectoryInfo di = new DirectoryInfo(ScriptsPath + sid);
            if (di.Exists)
            {
            }
            else
                di.Create();

            string strPath;

            if (HttpContext.Current.Session["AxPDFConfigs"] == null)
            {
                util.GetAxExportConfig();
            }
            iTextSharp.text.Document mydocument;
            if (HttpContext.Current.Session["AxPDFMargin"] != null && HttpContext.Current.Session["AxPDFMargin"].ToString() != "")
            {
                int top = 54;
                int bottom = 54;
                int left = 36;
                int right = 36;
                string pdfMargins = HttpContext.Current.Session["AxPDFMargin"].ToString();
                Dictionary<string, string> dimensions = JsonConvert.DeserializeObject<Dictionary<string, string>>(pdfMargins);
                foreach (var entry in dimensions)
                {
                    if (entry.Key == "top")
                        top = top * int.Parse(entry.Value);
                    if (entry.Key == "bottom")
                        bottom = bottom * int.Parse(entry.Value);
                    if (entry.Key == "left")
                        left = left * int.Parse(entry.Value);
                    if (entry.Key == "right")
                        right = right * int.Parse(entry.Value);
                }
                bool isLandscape = false;
                if (HttpContext.Current.Session["AxPDFOrientation"] != null && HttpContext.Current.Session["AxPDFOrientation"].ToString().ToLower() == "landscape")
                {
                    isLandscape = true;
                }
                Rectangle pageSize = isLandscape ? PageSize.A4.Rotate() : PageSize.A4;
                mydocument = new iTextSharp.text.Document(pageSize, left, right, top, bottom);
            }
            else
                mydocument = new iTextSharp.text.Document();
            string _fileName = objIview.IviewCaption.Replace(",", "");
            _fileName = _fileName.Replace(" ", "_");

            strPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            if (iViewCap != null && iViewCap != "")
                strPath = strPath + @"Axpert\" + sid + @"\" + iViewCap + ".pdf";
            else if (_fileName != string.Empty)
                strPath = strPath + @"Axpert\" + sid + @"\" + _fileName + ".pdf";
            else
                strPath = strPath + @"Axpert\" + sid + @"\view.pdf";
            PdfWriter.GetInstance(mydocument, new FileStream(strPath, FileMode.Create));
            mydocument.Open();
            mydocument.Add(Hdrtab);
            mydocument.Close();
            return strPath;
        }
    }

    private static string GetParamMobile(string param, string paramVal)
    {
        int k;
        string tem1 = string.Empty;
        string[] arrParams;
        string[] arrNoOfParams;
        string @params = string.Empty;
        ArrayList arrLstParams = new ArrayList();
        ArrayList arrLstParamVal = new ArrayList();
        string paramXml = string.Empty;
        Util.Util util = new Util.Util();
        if (paramVal != null)
        {
            if (paramVal.Contains("~"))
            {
                arrNoOfParams = paramVal.Split('~');
                int i;
                for (i = 0; i <= arrNoOfParams.Length - 1; i++)
                {
                    arrParams = arrNoOfParams[i].Split('♠');
                    arrLstParams.Insert(i, arrParams[0]);
                    if (arrParams[1].Contains("`") == true)
                        arrParams[1] = arrParams[1].Replace("`", "~");

                    arrLstParamVal.Insert(i, arrParams[1]);
                }
            }
            else
            {
                arrParams = paramVal.Split('♠');
                for (k = 0; k <= arrParams.Length - 1; k++)
                {
                    if (arrParams.Length > k + 1)
                    {
                        arrLstParams.Insert(k, arrParams[0]);
                        if (arrParams[1].Contains("`") == true)
                            arrParams[1] = arrParams[1].Replace("`", ",");

                        arrLstParamVal.Insert(k, arrParams[1]);
                    }
                }
            }
        }

        int j;
        for (j = 0; j <= arrLstParams.Count - 1; j++)
        {
            if (arrLstParams.Count > 0)
            {
                tem1 = arrLstParams[j].ToString();
                if (arrLstParams.Count > 0)
                {
                    paramVal = arrLstParamVal[j].ToString().Replace("&grave;", "~");
                    paramVal = arrLstParamVal[j].ToString().Replace("&amp;", "&");
                }
                else
                    paramVal = "";
            }

            if (param != "")
            {
                paramXml = paramXml + "<" + tem1.ToString() + ">";
                //paramXml = paramXml + util.CheckSpecialCharsSaveAs(util.ReplaceUrlSpecialChars(paramVal));

                string kyval = util.CheckSpecialCharsSaveAs(util.ReplaceUrlSpecialChars(paramVal));
                kyval = System.Net.WebUtility.HtmlDecode(kyval);
                kyval = kyval.Replace("&grave;", "~");
                kyval = kyval.Replace("&amp;", "&");
                paramXml = paramXml + kyval;

                paramXml = paramXml + "</" + tem1 + ">";

                @params = @params + "&" + tem1 + "=";
                @params = @params + paramVal;

                //if (tem1 != "" && tem1.ToLower().ToString() == "axhiddencolumn")
                //{
                //    foreach (string _thiscolName in paramVal.Split(','))
                //        axhiddencols.Add(_thiscolName);
                //}
            }
            else
                paramXml = "<params>";
        }
        return paramXml;
    }

    public static string StripHtmlTagsMobile(string input)
    {
        // Use a regular expression to remove all HTML tags
        return Regex.Replace(input, "<.*?>", String.Empty);
    }

    private static Cell AlignCellMobile(Cell cellAttr, string AlignStr)
    {
        if (AlignStr == "" | AlignStr == "Left")
            cellAttr.HorizontalAlignment = Element.ALIGN_LEFT;
        else if (AlignStr == "Right")
            cellAttr.HorizontalAlignment = Element.ALIGN_RIGHT;
        else if (AlignStr == "Center")
            cellAttr.HorizontalAlignment = Element.ALIGN_CENTER;

        return cellAttr;
    }
}
