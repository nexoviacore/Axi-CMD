using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Text;
//using Word.Api.Interfaces;
//using Word.W2004;
//using Word.W2004.Elements;
//using Word.W2004.Elements.TableElements;
//using Word.W2004.Style;
//using WFont = Word.W2004.Style.Font;
//using WImage = Word.W2004.Elements.Image;
using System.Drawing;
using System.Xml;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using FontSize = DocumentFormat.OpenXml.Wordprocessing.FontSize;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using System.Web.Services;
using System.Globalization;

public partial class aspx_WordView : System.Web.UI.Page
{
    IviewData objIview = new IviewData();
    Util.Util objUtil = new Util.Util();
    ArrayList colHide = new ArrayList();
    ArrayList colHead = new ArrayList();
    ArrayList htmlColumns = new ArrayList();
    ArrayList arrColType = new ArrayList();
    ArrayList colType = new ArrayList();
    ArrayList arrSubhead = new ArrayList();
    Util.Util util = new Util.Util();

    ArrayList hdrFont = new ArrayList();
    ArrayList hdrAlign = new ArrayList();

    ArrayList hdrFooterName = new ArrayList();
    ArrayList hdrFooterAlign = new ArrayList();
    ArrayList hdrFooterFont = new ArrayList();
    bool hideProjeName = false;
    string headerText = string.Empty;
    ArrayList axhiddencols = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        util.IsValidSession();
        ResetSessionTime();
        if (Session["project"] == null)
        {
            SessExpires();
        }
        else
        {
            if (!util.CheckValidLogin())
            {
                SessExpires();
                return;
            }
            if (Session["AxShowAppTitle"] != null && Session["AxShowAppTitle"].ToString().ToLower() == "true")
            {
                if (Session["AxAppTitle"] != null && Session["AxAppTitle"] != string.Empty)
                    headerText = Session["AxAppTitle"].ToString();
                else if (Session["projTitle"] != null)
                    headerText = Session["projTitle"].ToString();
            }
            if (Session["AxWordConfigs"] == null)
            {
                util.GetAxExportConfig();
            }
            ExportToWord();
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

    public void CreateDocumentNew(DataTable dt)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(stream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                Body body = mainPart.Document.AppendChild(new Body());

                SectionProperties sectionProps = new SectionProperties();
                Stack<HeaderReference> headerReferences = new Stack<HeaderReference>();
                int fontSize = 12;
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();
                Header header = new Header();
                if (!hideProjeName && headerText.ToString() != "")
                {
                    Paragraph headerParagraph = CreateHeaderParagraph(headerText.ToString(), fontSize * 2, JustificationValues.Center);
                    header.Append(headerParagraph);
                }
                if (objIview.ReportHdrs.Count > 0)
                {
                    for (int i = 0; i < objIview.ReportHdrs.Count; i++)
                    {
                        JustificationValues justification = JustificationValues.Center;
                        string _alignment = hdrAlign[i].ToString();
                        if (_alignment == "@left")
                            justification = JustificationValues.Left;
                        else if (_alignment == "@right")
                            justification = JustificationValues.Right;

                        Paragraph headerParagraph = CreateHeaderParagraph(StringReplaceSpecialChar(objIview.ReportHdrs[i].ToString()), fontSize * 2, justification);
                        header.Append(headerParagraph);
                    }
                }
                else
                {
                    Paragraph headerParagraph = CreateHeaderParagraph(objIview.IviewCaption, fontSize * 2, JustificationValues.Center);
                    header.Append(headerParagraph);
                }
                headerPart.Header = header;
                headerPart.Header.Save();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                headerReferences.Push(headerReference);
                while (headerReferences.Count > 0)
                {
                    sectionProps.Append(headerReferences.Pop());
                }

                Stack<FooterReference> footerRefeesrences = new Stack<FooterReference>();
                FooterPart footerPart = mainPart.AddNewPart<FooterPart>();
                Footer footer = new Footer();
                if (hdrFooterName.Count > 0)
                {
                    for (int i = 0; i < hdrFooterName.Count; i++)
                    {
                        Paragraph footerParagraph = new Paragraph();
                        Run _footerRun = new Run();
                        RunProperties _runFooterProperties = new RunProperties();
                        Bold _footerbold = new Bold();
                        _runFooterProperties.Append(_footerbold);
                        FontSize footerfontSizeElement = new FontSize() { Val = (fontSize * 2).ToString() };
                        _runFooterProperties.Append(footerfontSizeElement);
                        Color footercolor = new Color() { Val = "000000" }; // Hex color code for black
                        _runFooterProperties.Append(footercolor);
                        _footerRun.Append(_runFooterProperties);
                        Text _footerText = new Text(hdrFooterName[i].ToString());
                        _footerRun.Append(_footerText);
                        ParagraphProperties _paragraphProperties = new ParagraphProperties();
                        string _alignment = hdrFooterAlign[i].ToString();
                        Justification _justification;

                        switch (_alignment)
                        {
                            case "":
                            case "@center":
                                _justification = new Justification() { Val = JustificationValues.Center };
                                break;
                            case "@left":
                                _justification = new Justification() { Val = JustificationValues.Left };
                                break;
                            case "@right":
                                _justification = new Justification() { Val = JustificationValues.Right };
                                break;
                            default:
                                _justification = new Justification() { Val = JustificationValues.Left }; // Default to Left
                                break;
                        }
                        SpacingBetweenLines _spacing = new SpacingBetweenLines()
                        {
                            After = "0",
                            Before = "0",
                            Line = "0", // Adjust to control the line spacing
                            LineRule = LineSpacingRuleValues.Exact
                        };
                        _paragraphProperties.Append(_justification);
                        _paragraphProperties.Append(_spacing);
                        footerParagraph.Append(_paragraphProperties);
                        footerParagraph.Append(_footerRun);
                        footer.Append(footerParagraph);

                    }
                }
                if (!string.IsNullOrEmpty(objIview.IviewFooter))
                {
                    objIview.IsWordWithHtml = false;
                    Paragraph blankParagraph = new Paragraph();
                    Run blankRun = new Run();
                    Text blankText = new Text(" ");
                    blankRun.Append(blankText);
                    blankParagraph.Append(blankRun);
                    footer.Append(blankParagraph);
                    string[] footerText = objIview.IviewFooter.Split('|');
                    foreach (var text in footerText)
                    {
                        Paragraph footerParagraph = new Paragraph();
                        Run _footerRun = new Run();
                        RunProperties _runFooterProperties = new RunProperties();
                        Bold _footerbold = new Bold();
                        _runFooterProperties.Append(_footerbold);
                        FontSize footerfontSizeElement = new FontSize() { Val = (fontSize * 2).ToString() };
                        _runFooterProperties.Append(footerfontSizeElement);
                        Color footercolor = new Color() { Val = "000000" };
                        _runFooterProperties.Append(footercolor);
                        _footerRun.Append(_runFooterProperties);
                        Text _footerText = new Text(StringReplaceSpecialChar(text));
                        _footerRun.Append(_footerText);
                        ParagraphProperties _paragraphProperties = new ParagraphProperties();
                        Justification _justification = new Justification() { Val = JustificationValues.Center };
                        SpacingBetweenLines _spacing = new SpacingBetweenLines()
                        {
                            After = "0",
                            Before = "0",
                            Line = "0", // Adjust to control the line spacing
                            LineRule = LineSpacingRuleValues.Exact
                        };
                        _paragraphProperties.Append(_justification);
                        _paragraphProperties.Append(_spacing);
                        footerParagraph.Append(_paragraphProperties);
                        footerParagraph.Append(_footerRun);
                        footer.Append(footerParagraph);
                    }
                }
                footerPart.Footer = footer;

                Footer footer2 = new Footer();
                string _currentUser = string.Empty;
                if (HttpContext.Current.Session["username"] != null && HttpContext.Current.Session["username"].ToString() != "")
                {
                    _currentUser = HttpContext.Current.Session["username"].ToString();
                }
                if (_currentUser != string.Empty)
                {
                    string PrintedBy = "Printed by: " + _currentUser + " and Printed On: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    Paragraph footerParagraph = new Paragraph();
                    Run _footerRun = new Run();
                    Text _footerText = new Text(PrintedBy);
                    _footerRun.Append(_footerText);
                    ParagraphProperties _paragraphProperties = new ParagraphProperties();
                    Justification _justification = new Justification() { Val = JustificationValues.Right };
                    SpacingBetweenLines _spacing = new SpacingBetweenLines()
                    {
                        After = "0",
                        Before = "0",
                        Line = "0",
                        LineRule = LineSpacingRuleValues.Exact
                    };
                    _paragraphProperties.Append(_justification);
                    _paragraphProperties.Append(_spacing);
                    footerParagraph.Append(_paragraphProperties);
                    footerParagraph.Append(_footerRun);
                    footer2.Append(footerParagraph);
                }

                footerPart.Footer = footer2;
                footerPart.Footer.Save();
                FooterReference footerReference = new FooterReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(footerPart) };
                footerRefeesrences.Push(footerReference);
                while (footerRefeesrences.Count > 0)
                {
                    sectionProps.Append(footerRefeesrences.Pop());
                }
                if (HttpContext.Current.Session["AxWordOrientation"] != null && HttpContext.Current.Session["AxWordOrientation"].ToString() != "")
                {
                    if (HttpContext.Current.Session["AxWordOrientation"].ToString().ToLower() == "landscape")
                    {
                        PageSize pageSize = new PageSize()
                        {
                            Width = 16838,  // 11.69 inches = 297 mm (A4 width in landscape) in twips
                            Height = 11906, // 8.27 inches = 210 mm (A4 height) in twips
                            Orient = PageOrientationValues.Landscape
                        };
                        sectionProps.Append(pageSize);
                    }
                    else
                    {
                        PageSize pageSize = new PageSize()
                        {
                            Orient = PageOrientationValues.Portrait
                        };
                        sectionProps.Append(pageSize);
                    }
                }
                else
                {
                    PageSize pageSize = new PageSize()
                    {
                        Orient = PageOrientationValues.Portrait
                    };
                    sectionProps.Append(pageSize);
                }
                if (HttpContext.Current.Session["AxWordMargin"] != null && HttpContext.Current.Session["AxWordMargin"].ToString() != "")
                {
                    string wordMargins = HttpContext.Current.Session["AxWordMargin"].ToString();
                    Dictionary<string, string> dimensions = JsonConvert.DeserializeObject<Dictionary<string, string>>(wordMargins);
                    int top = 567;
                    int bottom = 567;
                    int left = 567;
                    int right = 567;
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
                    PageMargin margins = new PageMargin()
                    {
                        Top = top,
                        Bottom = bottom,
                        Left = new UInt32Value((uint)left),
                        Right = new UInt32Value((uint)right),
                        Header = new UInt32Value((uint)top),
                        Footer = new UInt32Value((uint)bottom)
                    };
                    sectionProps.Append(margins);
                }
                else
                {
                    PageMargin margins = new PageMargin()
                    {
                        Top = 567,//1 cm
                        Bottom = 567,
                        Left = 567,
                        Right = 567,
                        Header = 567,
                        Footer = 567
                    };
                    sectionProps.Append(margins);
                }

                body.Append(sectionProps);

                string iName = string.Empty;
                if (Session["ivname"] != null)
                    iName = Session["ivname"].ToString();
                else if (Request.QueryString["ivname"] != null)
                    iName = Request.QueryString["ivname"].ToString();

                string ivParams = string.Empty;
                if (Request.QueryString["ivParamCaption"] != null)
                    ivParams = Request.QueryString["ivParamCaption"].ToString();

                ArrayList _paramList = new ArrayList();
                if (!string.IsNullOrEmpty(ivParams))
                {
                    StringBuilder sbPVal = new StringBuilder();
                    string[] pairs = ivParams.Split('♥');
                    foreach (var pair in pairs)
                    {
                        if (!string.IsNullOrEmpty(pair))
                        {
                            string[] keyValue = pair.Split(':');
                            string kyval = keyValue[1].Replace("&grave;", "~");
                            kyval = kyval.Replace("&amp;", "&");
                            _paramList.Add(keyValue[0] + ": " + kyval);
                        }
                    }
                }

                foreach (string pval in _paramList)
                {
                    Paragraph paragraph = new Paragraph();
                    ParagraphProperties paragraphProperties = new ParagraphProperties();
                    Justification justification = new Justification() { Val = JustificationValues.Left };
                    SpacingBetweenLines spacing = new SpacingBetweenLines()
                    {
                        Before = "0", // No space before the paragraph
                        After = "0",  // No space after the paragraph
                        Line = "240", // Single line spacing (12pt)
                        LineRule = LineSpacingRuleValues.Exact
                    };
                    paragraphProperties.Append(justification);
                    paragraphProperties.Append(spacing);
                    paragraph.Append(paragraphProperties);
                    Run run = new Run();
                    RunProperties runProperties = new RunProperties();
                    FontSize fontSizeElement = new FontSize() { Val = "22" }; // Font size (11pt)
                    runProperties.Append(fontSizeElement);
                    Color color = new Color() { Val = "000000" }; // Font color (black)
                    runProperties.Append(color);
                    run.Append(runProperties);
                    Text text = new Text(pval);
                    run.Append(text);
                    paragraph.Append(run);
                    mainPart.Document.Body.Append(paragraph);
                }

                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                    )
                );
                TableLayout tableLayout = new TableLayout() { Type = TableLayoutValues.Autofit };
                //TableWidth tableWidth = new TableWidth() { Type = TableWidthUnitValues.Auto };
                //TableWidth tableWidth = new TableWidth() { Type = TableWidthUnitValues.Dxa, Width = "5000" };
                //props.Append(tableWidth);
                props.Append(tableLayout);

                TableCellMarginDefault tableCellMarginDefault = new TableCellMarginDefault(
                    new TopMargin { Width = "50", Type = TableWidthUnitValues.Dxa },
                    new BottomMargin { Width = "50", Type = TableWidthUnitValues.Dxa }, // Changed to positive value
                    new LeftMargin { Width = "50", Type = TableWidthUnitValues.Dxa },
                    new RightMargin { Width = "50", Type = TableWidthUnitValues.Dxa }
                );
                props.Append(tableCellMarginDefault);
                table.AppendChild<TableProperties>(props);

                if (dt.Rows[0].Table.Columns.Contains("rowdata_Id"))
                    dt.Columns.Remove("rowdata_Id");
                if (dt.Rows[0].Table.Columns.Contains("axrowtype"))
                    dt.Columns.Remove("axrowtype");
                if (dt.Rows[0].Table.Columns.Contains("axp__font"))
                    dt.Columns.Remove("axp__font");
                if (dt.Rows[0].Table.Columns.Contains("axp__color"))
                    dt.Columns.Remove("axp__color");
                if (dt.Rows[0].Table.Columns.Contains("axrowoptions"))
                    dt.Columns.Remove("axrowoptions");
                TableStrings ts = new TableStrings(12);
                CellString cs = new CellString(12);
                TableRow headerRow = new TableRow();
                int ih = -1;
                foreach (DataColumn column in dt.Columns)
                {
                    ih++;
                    if (column.ToString() != colHead[ih].ToString() && colHide[ih].ToString() == "true")
                        continue;
                    else if (column.ToString() == colHead[ih].ToString() && colHide[ih].ToString() == "true")
                        continue;
                    else if (axhiddencols.IndexOf(column.ToString()) > -1)// (axhiddencols.IndexOf(column) > -1)
                        continue;
                    string colName = colHead[ih].ToString();
                    if (colName == "Sr. No.")
                        colName = "Sr. No.";
                    Paragraph headerRowParagraph = new Paragraph();
                    Run headerRun = new Run();
                    RunProperties runProperties = new RunProperties();
                    Bold bold = new Bold();
                    runProperties.Append(bold);
                    headerRun.Append(runProperties);
                    headerRun.Append(new Text(StringReplaceSpecialChar(colName)));
                    ParagraphProperties paragraphProperties = new ParagraphProperties();
                    Justification justification = new Justification() { Val = JustificationValues.Center };
                    paragraphProperties.Append(justification);
                    headerRowParagraph.Append(paragraphProperties);
                    headerRowParagraph.Append(headerRun);
                    TableCell headerCell = new TableCell(headerRowParagraph);
                    TableCellProperties cellProperties = new TableCellProperties();
                    DocumentFormat.OpenXml.Wordprocessing.Shading cellShading = new DocumentFormat.OpenXml.Wordprocessing.Shading()
                    {
                        Color = "auto",
                        Fill = "D9D9D9",
                        Val = ShadingPatternValues.Clear
                    };
                    cellProperties.Append(cellShading);
                    headerCell.Append(cellProperties);
                    headerRow.Append(headerCell);
                }
                table.Append(headerRow);
                int srNoIndex = -1;
                int cellIndex = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TableRow dataRow = new TableRow();
                    bool isGTRow = false;
                    bool addGrandTotal = false;
                    DataRow dr = dt.Rows[i];
                    string rowString = string.Empty;
                    int isFirstCol = 0;
                    for (int c = 0; c < dt.Rows[i].Table.Columns.Count; c++)
                    {
                        if (dt.Rows[0].Table.Columns[c].ToString() != colHead[c].ToString() && colHide[c].ToString() == "true")
                            continue;
                        else if (dt.Rows[0].Table.Columns[c].ToString() == colHead[c].ToString() && colHide[c].ToString() == "true")
                            continue;
                        else if (axhiddencols.IndexOf(dt.Rows[i].Table.Columns[c].ToString()) > -1)
                            continue;
                        string colValue = StringReplaceSpecialCharNew(dr[dt.Rows[i].Table.Columns[c]].ToString());
                        //if (arrSubhead.Count > 0 && int.Parse(arrSubhead[0].ToString()) == i)
                        //{
                        //    if (cellIndex == -1)
                        //        for (int a = 0; a < dt.Rows[i].Table.Columns.Count; a++)
                        //            if (dr[dt.Rows[i].Table.Columns[a]].ToString() == "")
                        //                continue;
                        //            else
                        //            {
                        //                cellIndex = a;
                        //                break;
                        //            }

                        //    string cellText = dr[dt.Rows[i].Table.Columns[cellIndex]].ToString();
                        //    string cleanedText = StringReplaceSpecialChar(cellText);
                        //    TableCell dataCell = new TableCell(new Paragraph(new Run(new Text(cleanedText))));
                        //    dataRow.Append(dataCell);
                        //    arrSubhead.Remove(arrSubhead[c]);
                        //    break;
                        //}
                        //else
                        //{
                        bool isBoldComplete = false;
                        if (colValue == "Grand Total" && srNoIndex != -1)
                        {
                            addGrandTotal = true;
                            colValue = "";
                            isGTRow = true;
                        }
                        if ((addGrandTotal) && (colValue == "") && (isFirstCol != 0) && (srNoIndex != -1))
                        {
                            colValue = "Grand Total";
                            addGrandTotal = false;
                            isBoldComplete = true;
                            Text cellText = new Text(!string.IsNullOrEmpty(colValue) ? colValue : "-");
                            Run run = new Run();
                            RunProperties runProperties = new RunProperties();
                            Bold bold = new Bold();
                            runProperties.Append(bold);
                            run.Append(runProperties);
                            run.Append(cellText);
                            Paragraph paragraph = new Paragraph(run);
                            TableCell dataCell = new TableCell(paragraph);
                            dataRow.Append(dataCell);
                        }
                        if (!isBoldComplete)
                        {
                            if (isGTRow)
                            {
                                Text cellText = new Text(!string.IsNullOrEmpty(colValue) ? colValue : "-");
                                Run run = new Run();
                                RunProperties runProperties = new RunProperties();
                                Bold bold = new Bold();
                                runProperties.Append(bold);
                                run.Append(runProperties);
                                run.Append(cellText);
                                Paragraph paragraph = new Paragraph(run);
                                TableCell dataCell = new TableCell(paragraph);
                                dataRow.Append(dataCell);
                            }
                            else
                            {
                                string[] breakTags = new[] { "\n", "<br>", "<br/>", "</br>" };
                                if (breakTags.Any(tag => colValue.Contains(tag)))
                                {
                                    TableCell dataCell = new TableCell();
                                    var paragraph = new Paragraph();
                                    string[] lines = colValue.Split(new[] { "\n", "<br>", "<br/>", "</br>" }, StringSplitOptions.None);
                                    foreach (var line in lines)
                                    {
                                        if (!string.IsNullOrWhiteSpace(line))
                                        {
                                            paragraph.Append(new Run(new Text(line.Trim())));
                                            paragraph.Append(new Run(new Break()));
                                        }
                                    }
                                    if (paragraph.Elements<Run>().Any())
                                    {
                                        var lastRun = paragraph.Elements<Run>().Last();
                                        if (lastRun.Elements<Break>().Any())
                                        {
                                            lastRun.Remove();
                                        }
                                    }
                                    dataCell.Append(paragraph);
                                    dataRow.Append(dataCell);
                                }
                                else
                                {
                                    TableCell dataCell = new TableCell(new Paragraph(new Run(new Text(!string.IsNullOrEmpty(colValue) ? colValue : "-"))));
                                    dataRow.Append(dataCell);
                                }
                            }
                        }
                        //}
                        isFirstCol = isFirstCol + 1;
                    }
                    table.Append(dataRow);
                }
                body.Append(table);
                mainPart.Document.Save();
            }
            string fileName = objIview.IviewCaption.Replace(",", "");
            fileName = fileName.Replace(" ", "_");

            // Output the document to the browser
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".doc");
            HttpContext.Current.Response.BinaryWrite(stream.ToArray());
            HttpContext.Current.Response.End();
        }
    }
    Paragraph CreateHeaderParagraph(string text, int fontSize, JustificationValues justification)
    {
        Paragraph headerParagraph = new Paragraph();

        // Run for the text
        Run _headerRun = new Run();
        RunProperties _runProperties = new RunProperties();
        Bold _bold = new Bold();
        _runProperties.Append(_bold);
        FontSize fontSizeElement = new FontSize() { Val = fontSize.ToString() };
        _runProperties.Append(fontSizeElement);
        Color color = new Color() { Val = "000000" }; // Black color
        _runProperties.Append(color);
        _headerRun.Append(_runProperties);
        _headerRun.Append(new Text(text));

        ParagraphProperties _paragraphProperties = new ParagraphProperties();
        _paragraphProperties.Append(new Justification() { Val = justification });
        SpacingBetweenLines _spacing = new SpacingBetweenLines()
        {
            After = "0", // No space after the paragraph
            Before = "0", // No space before the paragraph
            //Line = "240", // Single line spacing (12pt)
            //LineRule = LineSpacingRuleValues.Exact,
        };
        _paragraphProperties.Append(_spacing);
        //ContextualSpacing contextualSpacing = new ContextualSpacing() { Val = true };
        //_paragraphProperties.Append(contextualSpacing);

        // Append properties and run to the paragraph
        headerParagraph.Append(_paragraphProperties);
        headerParagraph.Append(_headerRun);

        return headerParagraph;
    }

    protected static string StringReplaceSpecialChar(string value)
    {
        value = parseHTML(value); ;
        if (value.Contains("<!--[if gte mso 9]>"))
        {
            value = value.Replace("<!--[if gte mso 9]>", "ô");
            int totalCount = value.Count(x => x == 'ô');
            for (int i = 0; i < totalCount; i++)
            {
                int startIndex = value.IndexOf("ô");
                int endIndex = value.IndexOf("<![endif]-->", startIndex);
                value = value.Remove(startIndex, (endIndex - startIndex) + 12);
            }
        }

        if (value.Contains("<!--[if gte mso 10]>"))
        {
            value = value.Replace("<!--[if gte mso 10]>", "├");
            int totalCount = value.Count(x => x == '├');
            for (int i = 0; i < totalCount; i++)
            {
                int startIndex = value.IndexOf("├");
                int endIndex = value.IndexOf("<![endif]-->", startIndex);
                value = value.Remove(startIndex, (endIndex - startIndex) + 12);
            }
        }

        if (value.Contains("<style>"))
        {
            value = value.Replace("<style>", "☻");
            int totalCount = value.Count(x => x == '☻');
            for (int i = 0; i < totalCount; i++)
            {
                int startIndex = value.IndexOf("☻");
                int endIndex = value.IndexOf("</style>", startIndex);
                value = value.Remove(startIndex, (endIndex - startIndex) + 8);
            }
        }
        if (value.Contains("<a "))
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(value);
            var aTags = doc.DocumentNode.SelectNodes("//a");
            if (aTags != null)
            {
                foreach (var aTag in aTags)
                {
                    var spanTag = aTag.SelectSingleNode(".//span");
                    if (spanTag == null)
                    {
                        string innerText = aTag.InnerText.Trim();
                        aTag.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(innerText), aTag);
                    }
                    else
                    {
                        aTag.Remove();
                    }
                }
            }
            string result = doc.DocumentNode.InnerHtml;
            value = result.Trim();
        }

        if (value.Contains("<font "))
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(value);
            string combinedContent = string.Empty;
            foreach (var node in doc.DocumentNode.ChildNodes)
            {
                if (node.Name.Equals("font", StringComparison.OrdinalIgnoreCase))
                {
                    combinedContent += node.InnerText.Trim() + " ";
                }
                else if (!string.IsNullOrWhiteSpace(node.InnerText))
                {
                    combinedContent += node.InnerText.Trim() + " ";
                }
            }
            value = combinedContent.Trim();
        }

        value = value.Replace("<o:p>", "");
        value = value.Replace("</o:p>", "");

        value = value.Replace("<![endif]-->", " ");
        value = value.Replace("<!--[if !supportLists]-->", " ");

        value = value.Replace("<span>", " ");
        value = value.Replace("</span>", " ");

        value = value.Replace("<strong>", "");
        value = value.Replace("</strong>", "");

        value = value.Replace("<b>", "");
        value = value.Replace("</b>", "");

        value = value.Replace("<p>", "");
        value = value.Replace("</p>", "");

        value = value.Replace("<ul>", "");
        value = value.Replace("</ul>", "");

        value = value.Replace("<li>", " ");
        value = value.Replace("</li>", "");

        value = value.Replace("<ol>", "");
        value = value.Replace("</ol>", "");
        value = value.Replace("\n", "");
        value = value.Replace("</br>", "");
        value = value.Replace("<br/>", "");
        value = value.Replace("<br>", "");
        value = value.Replace("paraLine", "");
        value = value.Replace("&nbsp;", " ");
        value = value.Replace("&zwj;", "");
        value = value.Replace("&#39;", "'");
        value = value.Replace("&rsquo;", "'");
        value = value.Replace("&rdquo;", "\"");
        value = value.Replace("&ldquo;", "\"");
        value = value.Replace("&ndash", "-");
        value = value.Replace("&ndash;", "-");
        //value = value.Replace("&", "&amp;");
        value = value.Replace("!--[endif]--", "");
        return value;
    }

    protected static string StringReplaceSpecialCharNew(string value)
    {
        value = parseHTML(value); ;
        if (value.Contains("<!--[if gte mso 9]>"))
        {
            value = value.Replace("<!--[if gte mso 9]>", "ô");
            int totalCount = value.Count(x => x == 'ô');
            for (int i = 0; i < totalCount; i++)
            {
                int startIndex = value.IndexOf("ô");
                int endIndex = value.IndexOf("<![endif]-->", startIndex);
                value = value.Remove(startIndex, (endIndex - startIndex) + 12);
            }
        }

        if (value.Contains("<!--[if gte mso 10]>"))
        {
            value = value.Replace("<!--[if gte mso 10]>", "├");
            int totalCount = value.Count(x => x == '├');
            for (int i = 0; i < totalCount; i++)
            {
                int startIndex = value.IndexOf("├");
                int endIndex = value.IndexOf("<![endif]-->", startIndex);
                value = value.Remove(startIndex, (endIndex - startIndex) + 12);
            }
        }

        if (value.Contains("<style>"))
        {
            value = value.Replace("<style>", "☻");
            int totalCount = value.Count(x => x == '☻');
            for (int i = 0; i < totalCount; i++)
            {
                int startIndex = value.IndexOf("☻");
                int endIndex = value.IndexOf("</style>", startIndex);
                value = value.Remove(startIndex, (endIndex - startIndex) + 8);
            }
        }
        if (value.Contains("<a "))
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(value);
            var aTags = doc.DocumentNode.SelectNodes("//a");
            if (aTags != null)
            {
                foreach (var aTag in aTags)
                {
                    var spanTag = aTag.SelectSingleNode(".//span");
                    if (spanTag == null)
                    {
                        string innerText = aTag.InnerText.Trim();
                        aTag.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(innerText), aTag);
                    }
                    else
                    {
                        aTag.Remove();
                    }
                }
            }
            string result = doc.DocumentNode.InnerHtml;
            value = result.Trim();
        }

        if (value.Contains("<font "))
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(value);
            string combinedContent = string.Empty;
            foreach (var node in doc.DocumentNode.ChildNodes)
            {
                if (node.Name.Equals("font", StringComparison.OrdinalIgnoreCase))
                {
                    combinedContent += node.InnerText.Trim() + " ";
                }
                else if (!string.IsNullOrWhiteSpace(node.InnerText))
                {
                    combinedContent += node.InnerText.Trim() + " ";
                }
            }
            value = combinedContent.Trim();
        }

        value = value.Replace("<o:p>", "");
        value = value.Replace("</o:p>", "");

        value = value.Replace("<![endif]-->", " ");
        value = value.Replace("<!--[if !supportLists]-->", " ");

        value = value.Replace("<span>", " ");
        value = value.Replace("</span>", " ");

        value = value.Replace("<strong>", "");
        value = value.Replace("</strong>", "");

        value = value.Replace("<b>", "");
        value = value.Replace("</b>", "");

        value = value.Replace("<p>", "");
        value = value.Replace("</p>", "");

        value = value.Replace("<ul>", "");
        value = value.Replace("</ul>", "");

        value = value.Replace("<li>", " ");
        value = value.Replace("</li>", "");

        value = value.Replace("<ol>", "");
        value = value.Replace("</ol>", "");
        value = value.Replace("paraLine", "");
        value = value.Replace("&nbsp;", " ");
        value = value.Replace("&zwj;", "");
        value = value.Replace("&#39;", "'");
        value = value.Replace("&rsquo;", "'");
        value = value.Replace("&rdquo;", "\"");
        value = value.Replace("&ldquo;", "\"");
        value = value.Replace("&ndash", "-");
        value = value.Replace("&ndash;", "-");
        value = value.Replace("!--[endif]--", "");
        return value;
    }

    private static string parseHTML(string html)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);
        HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//div | //ul | //p | //table | //ol | //span | //strong | //b");
        using (StringWriter sw = new StringWriter())
        {
            if (htmlNodes != null && htmlNodes.Count > 0)
                foreach (var node in htmlNodes)
                    if (node.HasAttributes)
                        node.Attributes.RemoveAll();

            doc.Save(sw);
            return sw.ToString();
        }
    }

    public void SessExpires()
    {
        Util.Util utilObj = new Util.Util();
        string url = utilObj.SESSEXPIRYPATH;
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }

    public void GetDataTypeOfAColumn(string value)
    {
        if (colType[colHead.IndexOf(value)].ToString() == "c")
            arrColType.Add("<w:jc w:val=\"left\"/>");
        else if (colType[colHead.IndexOf(value)].ToString() == "n")
            arrColType.Add("<w:jc w:val=\"right\"/>");
        else if (colType[colHead.IndexOf(value)].ToString() == "d")
            arrColType.Add("<w:jc w:val=\"center\"/>");
    }

    public void ExportToWord()
    {
        string Ikey = string.Empty;
        if (Session["ivKey"] != null)
            Ikey = Session["ivKey"].ToString();
        else if (Request.QueryString["ivKey"] != null)
            Ikey = Request.QueryString["ivKey"].ToString();
        objIview = (IviewData)Session[Ikey];
        objIview.ShowHiddengridCols = new ArrayList();
        objIview.ReportHdrs = new ArrayList();

        string sid = HttpContext.Current.Session["nsessionid"].ToString();

        DataTable dt = new DataTable();
        //dt = objIview.DtCurrentdata.Clone();

        if (objIview.IsIviewStagLoad)
        {

            for (int rCnt = 0; rCnt < objIview.DtCurrentdata.Rows.Count; rCnt++)
            {
                dt.ImportRow(objIview.DtCurrentdata.Rows[rCnt]);
            }
            //colHide = (ArrayList)objIview.ColHide.Clone();
        }
        else
        {
            dt = GetFullData();
        }

        int dtcount = dt.Columns.Count;
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].ColumnName.Contains("html_"))
                {
                    colHide[j] = "false";
                    string oldColName = dt.Columns[j].ColumnName.Replace("html_", "");

                    // to hide the columns if column is hidden and column with prefix as html_
                    if (objIview.ShowHideCols != null)
                    {
                        if (objIview.ShowHideCols.IndexOf(oldColName) > -1)
                            colHide[j] = "true";
                    }
                }
            }

            int count = 0;
            foreach (DataRow item in dt.Rows)
            {
                foreach (var cell in item.ItemArray)
                {
                    if (cell.ToString().ToLower() == "subhead")
                        arrSubhead.Add(count);
                }
                count++;
            }
            if (dt.Columns.Count < colHide.Count)
            {
                colHide.RemoveAt(1);
                colHide.RemoveAt(1);

                colHead.RemoveAt(1);
                colHead.RemoveAt(1);

                colType.RemoveAt(1);
                colType.RemoveAt(1);
            }

            if (dt.Columns.Count == colHide.Count)
            {
                for (int i = colHide.Count - 1; i >= 0; i--)
                    if (colHide[i].ToString() == "true" || dt.Columns[i].ColumnName == "axrowtype" || dt.Columns[i].ColumnName == "rowno" || dt.Columns[i].ColumnName == "axp__font" || dt.Columns[i].ColumnName == "axp__color" || dt.Columns[i].ColumnName == "axrowoptions")
                        //if hidden column are used in hide column 
                        if (objIview.ShowHiddengridCols != null)
                            if (objIview.ShowHiddengridCols.IndexOf(dt.Columns[i].ColumnName) > -1)
                            {
                                // To remove the normal column if column cantains html_column and is visible
                                int indx = -1;
                                string removeColumn = string.Empty;
                                if (dt.Columns[i].ColumnName.Contains("html_"))
                                {
                                    //do nothing
                                }
                                else
                                {
                                    removeColumn = "html_" + dt.Columns[i].ColumnName;
                                }
                            }
                            else
                            {
                                dt.Columns.RemoveAt(i);
                                colHead.RemoveAt(i);
                                colHide.RemoveAt(i);
                                colType.RemoveAt(i);
                            }
                        else
                        {
                            dt.Columns.RemoveAt(i);
                            colHead.RemoveAt(i);
                            colHide.RemoveAt(i);
                            colType.RemoveAt(i);
                        }
            }


            for (int hIdx = dt.Columns.Count - 1; hIdx >= 0; hIdx--)
            {
                int cIdx = -1, sHIdx = -1;
                cIdx = colHide.IndexOf(dt.Columns[hIdx].ColumnName);

                //condition for removeing hide columns which are checked in gridview 
                if (objIview.ShowHideCols != null)
                    sHIdx = objIview.ShowHideCols.IndexOf(dt.Columns[hIdx].ColumnName);

                if ((cIdx != -1) || (sHIdx != -1))
                {
                    dt.Columns.RemoveAt(hIdx);
                    colHead.RemoveAt(hIdx);
                }
            }
            //CreateDocument(dt);
            CreateDocumentNew(dt);
        }
    }

    public DataTable GetFullData()
    {
        DataTable dtData = new DataTable();
        string paramvalues = string.Empty;
        string pXml = string.Empty;
        try
        {
            DataSet ds = CallWebService();

            dtData = ds.Tables["row"].Copy();
        }
        catch (Exception Ex) { }
        return dtData;
    }
    public string GetParameterXML(string paramvalues)
    {

        string str = string.Empty;
        string nXml = string.Empty;

        str = paramvalues;
        string[] strp = objUtil.AxSplit1(str, "~");
        int indx = 0;

        for (indx = 0; indx <= strp.Length - 1; indx++)
        {
            if (!string.IsNullOrEmpty(strp[indx]))
            {
                string[] arrparam = strp[indx].ToString().Split(',');
                string pName = objUtil.CheckSpecialChars(arrparam[0].ToString());
                string pValue = objUtil.CheckSpecialChars(arrparam[1].ToString());

                if (pValue.Contains("`") == true)
                {
                    pValue = pValue.Replace("`", ",");
                }
                nXml = nXml + "<" + pName + ">";
                nXml = nXml + pValue;
                nXml = nXml + "</" + pName + ">";
            }
        }
        return nXml;
    }
    private DataSet CallWebService()
    {
        string iName = string.Empty;
        string sortCol = string.Empty;
        string sortOrd = string.Empty;
        string filterCol = string.Empty;
        string filterColVal = string.Empty;
        string filterValue1 = string.Empty;
        string filterOpr = string.Empty;
        string filename = string.Empty;
        string sid = string.Empty;
        string nXml = string.Empty;
        string errorLog = string.Empty;
        LogFile.Log logobj = new LogFile.Log();

        if (Session["ivname"] != null)
            iName = Session["ivname"].ToString();
        else if (Request.QueryString["ivname"] != null)
            iName = Request.QueryString["ivname"].ToString();
        if (Session["sOrder"] != null)
            sortOrd = Session["sOrder"].ToString();
        if (Session["sColumn"] != null)
            sortCol = Session["sColumn"].ToString();
        if (Session["fcolopr"] != null)
            filterOpr = Session["fcolopr"].ToString();
        if (Session["fCol"] != null)
            filterCol = Session["fCol"].ToString();
        if (Session["fColVal"] != null)
            filterColVal = Session["fColVal"].ToString();
        if (Session["fColVal2"] != null)
            filterColVal = Session["fColVal2"].ToString();
        string typeIvOrLv = "";
        if (Request.QueryString["typeIvOrLv"] != null)
            typeIvOrLv = Request.QueryString["typeIvOrLv"].ToString();

        string ivParams = string.Empty;
        if (Session["AxIvExportParams-" + iName] != null)
            ivParams = Session["AxIvExportParams-" + iName].ToString();

        filename = "GetLView-" + iName;
        sid = Session["nsessionid"].ToString();

        string curRecord = "";
        if (Request.QueryString["curRecord"] != null)
        {
            curRecord = Request.QueryString["curRecord"].ToString();
        }

        if (ivParams != string.Empty)
        {
            StringBuilder sbPVal = new StringBuilder();
            string[] pairs = ivParams.Split('~');
            foreach (var pair in pairs)
            {
                string[] keyValue = pair.Split('♠');
                string kyval = keyValue[1];
                kyval = kyval.Replace("&grave;", "~");
                kyval = kyval.Replace("&amp;", "&");
                XElement element = new XElement(keyValue[0], kyval);
                sbPVal.Append(element.ToString());
                if (keyValue[0] != "" && keyValue[0].ToLower().ToString() == "axhiddencolumn")
                {
                    foreach (string _thiscolName in kyval.Split(','))
                        axhiddencols.Add(_thiscolName);
                }
            }
            ivParams = sbPVal.ToString();
        }

        errorLog = logobj.CreateLog("Getting Iview Data.", sid, filename, "new");
        string _thisList = string.Empty;
        if (typeIvOrLv == "true")
            _thisList = " purpose=\"list\" ";
        if (curRecord == "")
            nXml = "<root " + _thisList + " headercached='false' name ='" + iName + "' axpapp ='" + Session["project"].ToString() + "' sessionid = '" + sid + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errorLog + "' pageno='0' firsttime='yes' sorder='" + sortOrd + "' scol='" + sortCol + "' fcolopr='" + filterOpr + "' fcolnm='" + filterCol + "' fcolval1='" + filterColVal + "' fcolval2='" + filterValue1 + "'><params> ";
        else
            nXml = "<root " + _thisList + " headercached='false' name ='" + iName + "' axpapp ='" + Session["project"].ToString() + "' sessionid = '" + sid + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errorLog + "' pageno='1' pagesize='" + curRecord + "' firsttime='yes' sqlpagination='true' getrowcount='false' gettotalrows='false' smartview='true'><params> ";
        nXml = nXml + ivParams;// objIview.iviewParams.IviewParamString;
        nXml = nXml + "</params>";
        nXml = nXml + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

        string ires = string.Empty;
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        try
        {
            objIview.requestJSON = true;
            ires = objWebServiceExt.CallGetIViewWS(iName, nXml, "", objIview);
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
        //DataSet ds1 = SetIviewComponents(ires);
        SetHeaderFooters(ires);
        dynamic jsonObject = JsonConvert.DeserializeObject(ires);
        DataSet ds1 = ConvertToDataSet(jsonObject);
        return ds1;

    }

    public DataSet ConvertToDataSet(dynamic jsonObject)
    {
        DataSet dataSet = new DataSet();
        JObject jsonObjectNew = jsonObject.data.headrow;
        foreach (var property in jsonObjectNew.Properties())
        {
            string propertyName = property.Name;
            JToken hideToken = property.Value.SelectToken("@hide");
            JToken typeToken = property.Value.SelectToken("@type");
            JToken textToken = property.Value.SelectToken("#text");
            string hideValue = hideToken != null ? hideToken.ToString() : null;
            string typeValue = typeToken != null ? typeToken.ToString() : null;
            string textValue = textToken != null ? textToken.ToString() : null;
            if (propertyName == "rowno")
            {
                colHide.Add(hideValue);
                colHead.Add("Sr. No.");
                colType.Add("c");
            }
            else if (propertyName != "pivotghead")
            {
                colHide.Add(hideValue);
                colHead.Add(textValue);
                colType.Add(typeValue);
            }
        }


        // Convert rows to DataTable
        DataTable rowsTable = new DataTable("row");

        // Add columns dynamically
        foreach (var row in jsonObject.data.row)
        {
            foreach (JProperty property in row)
            {
                if (!rowsTable.Columns.Contains(property.Name))
                {
                    rowsTable.Columns.Add(new DataColumn(property.Name, typeof(string)));
                }
            }
        }

        // Add rows dynamically
        foreach (var row in jsonObject.data.row)
        {
            DataRow dataRow = rowsTable.NewRow();
            foreach (JProperty property in row)
            {
                dataRow[property.Name] = property.Value.ToString();
            }
            rowsTable.Rows.Add(dataRow);
        }
        dataSet.Tables.Add(rowsTable);

        return dataSet;
    }

    public void SetHeaderFooters(string sJson)
    {
        JObject jsonObject = JObject.Parse(sJson);
        JObject reporthf = (JObject)jsonObject["data"]["reporthf"];

        try
        {
            string hideprojname = reporthf["hideprojname"].ToString();
            if (hideprojname != null && hideprojname.ToString() == "@t")
            {
                hideProjeName = true;
            }
        }
        catch (Exception ex) { }

        JObject header = (JObject)reporthf["header"];
        JObject footer = (JObject)reporthf["footer"];
        if (header != null)
        {
            foreach (var item in header)
            {
                objIview.ReportHdrs.Add((string)item.Value["text"]);
                hdrFont.Add((string)item.Value["font"]);
                if (item.Value["header_aline"] != null)
                {
                    hdrAlign.Add((string)item.Value["header_aline"]);
                }
                else if (hdrAlign.Count > 0 && hdrAlign[0] != null)
                {
                    hdrAlign.Add(hdrAlign[0]);
                }
                else
                {
                    hdrAlign.Add("@center");
                }
            }
        }
        if (footer != null)
        {
            foreach (var item in footer)
            {
                hdrFooterName.Add((string)item.Value["text"]);
                hdrFooterFont.Add((string)item.Value["font"]);
                if (item.Value["footer_aline"] != null)
                {
                    hdrFooterAlign.Add((string)item.Value["footer_aline"]);
                }
                else if (hdrFooterAlign.Count > 0 && hdrFooterAlign[0] != null)
                {
                    hdrFooterAlign.Add(hdrFooterAlign[0]);
                }
                else
                {
                    hdrFooterAlign.Add("@center");
                }
            }
        }
    }
    public DataSet SetIviewComponents(string sXml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNodeList xmlNList;
        XmlNodeList cbaseDataNodes;
        XmlNodeList baseDataNodes;
        XmlNodeList baseChildList;
        DataSet ds = new DataSet();
        StringWriter sw = new StringWriter();

        xmlDoc.LoadXml(sXml);
        baseDataNodes = xmlDoc.SelectNodes("//headrow");

        foreach (XmlNode baseDataNode in baseDataNodes)
        {
            baseChildList = baseDataNode.ChildNodes;
            foreach (XmlNode baseChildNode in baseChildList)
            {
                //if (baseChildNode.Attributes["hide"] != null)
                //{
                if (baseChildNode.Name != "pivotghead")
                {
                }
                //if (baseChildNode.Name == "axrowtype" || baseChildNode.Name == "axp__font")
                //{
                //    continue;
                //}
                if (baseChildNode.Name == "rowno")
                {
                    colHide.Add(baseChildNode.Attributes["hide"].Value);
                    colHead.Add("Sr. No.");
                    colType.Add("c");
                }
                else
                {
                    if (baseChildNode.Name != "pivotghead")
                    {

                        colHide.Add(baseChildNode.Attributes["hide"].Value);
                        colHead.Add(baseChildNode.InnerText);
                        colType.Add(baseChildNode.Attributes["type"].Value);
                    }
                }
                //}

            }
        }

        xmlNList = xmlDoc.SelectNodes("//data/comps");
        objIview.ReportHdrs.Clear();

        try
        {
            foreach (XmlNode compNode in xmlNList)
            {
                cbaseDataNodes = compNode.ChildNodes;
                foreach (XmlNode cbaseDataNode in cbaseDataNodes)
                {
                    string sNodeName = cbaseDataNode.Name;

                    if (sNodeName.Equals("x__head"))
                        objIview.IviewCaption = cbaseDataNode.Attributes["caption"].Value;
                    else
                    {
                        if (sNodeName.Equals("header"))
                        {
                            if (cbaseDataNode.HasChildNodes)
                            {
                                for (int i = 0; i < cbaseDataNode.ChildNodes.Count; i++)
                                {
                                    objIview.ReportHdrs.Add(cbaseDataNode.ChildNodes[i].InnerText);
                                }
                            }
                        }
                    }
                }

            }

            XmlTextWriter xSw = new XmlTextWriter(sw);
            xmlDoc.WriteTo(xSw);
            string nXML = sw.ToString();
            StringReader sr = new StringReader(nXML);
            ds.ReadXml(sr);
        }
        catch (Exception Ex) { }
        return ds;
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    private string GetReportHeaders(IviewData objIview)
    {
        StringBuilder strHeaders = new StringBuilder();
        for (int i = 1; i < objIview.ReportHdrs.Count; i++)
        {
            strHeaders.Append("<div> <h3 align=center><span >");
            strHeaders.Append("<font style='color:black;font-size:13px'>");
            strHeaders.Append(objIview.ReportHdrs[i].ToString() + "</font></span> </h3></div>");
        }
        return strHeaders.ToString();
    }

    private string GetSubHeadings(ArrayList subHeads)
    {
        StringBuilder strSubHeads = new StringBuilder();
        for (int i = 0; i < subHeads.Count; i++)
        {
            strSubHeads.Append("<div> <h3 align=center><span style=");
            strSubHeads.Append(HtmlTextWriter.DoubleQuoteChar);
            strSubHeads.Append("font-weight:bold; font-size:8px; font-family:'Segoe UI'; color: #81040a;");
            strSubHeads.Append(HtmlTextWriter.DoubleQuoteChar);
            strSubHeads.Append(">" + subHeads[i].ToString() + "</span> </h3></div>");
        }
        return strSubHeads.ToString();
    }

    [WebMethod]
    public static string ExportToWordMobile(string ivKey, string ivname, string ivtype, string axpCache, string _params, string _typeIvOrLv, string _curRecord, string _ivParamCaption)
    {
        string _fileName = string.Empty;
        IviewData objIview = new IviewData();
        ArrayList colHide = new ArrayList();
        ArrayList colHead = new ArrayList();
        ArrayList htmlColumns = new ArrayList();
        ArrayList arrColType = new ArrayList();
        ArrayList colType = new ArrayList();
        ArrayList arrSubhead = new ArrayList();
        Util.Util util = new Util.Util();
        ArrayList axhiddencols = new ArrayList();
        bool hideProjeName = false;
        ArrayList hdrFont = new ArrayList();
        ArrayList hdrAlign = new ArrayList();

        ArrayList hdrFooterName = new ArrayList();
        ArrayList hdrFooterAlign = new ArrayList();
        ArrayList hdrFooterFont = new ArrayList();
        string headerText = string.Empty;

        string Ikey = string.Empty;
        if (HttpContext.Current.Session["ivKey"] != null)
            Ikey = HttpContext.Current.Session["ivKey"].ToString();
        else if (ivKey != null)
            Ikey = ivKey.ToString();
        objIview = (IviewData)HttpContext.Current.Session[Ikey];
        objIview.ShowHiddengridCols = new ArrayList();
        objIview.ReportHdrs = new ArrayList();

        string sid = HttpContext.Current.Session["nsessionid"].ToString();

        if (HttpContext.Current.Session["AxWordConfigs"] == null)
        {
            util.GetAxExportConfig();
        }

        DataTable dt = new DataTable();
        if (objIview.IsIviewStagLoad)
        {

            for (int rCnt = 0; rCnt < objIview.DtCurrentdata.Rows.Count; rCnt++)
            {
                dt.ImportRow(objIview.DtCurrentdata.Rows[rCnt]);
            }
        }
        else
        {
            DataTable dtData = new DataTable();
            string paramvalues = string.Empty;
            string pXml = string.Empty;
            try
            {
                string iName = string.Empty;
                string sortCol = string.Empty;
                string sortOrd = string.Empty;
                string filterCol = string.Empty;
                string filterColVal = string.Empty;
                string filterValue1 = string.Empty;
                string filterOpr = string.Empty;
                string filename = string.Empty;
                string nXml = string.Empty;
                string errorLog = string.Empty;
                LogFile.Log logobj = new LogFile.Log();

                if (HttpContext.Current.Session["ivname"] != null)
                    iName = HttpContext.Current.Session["ivname"].ToString();
                else if (ivname != null)
                    iName = ivname.ToString();
                if (HttpContext.Current.Session["sOrder"] != null)
                    sortOrd = HttpContext.Current.Session["sOrder"].ToString();
                if (HttpContext.Current.Session["sColumn"] != null)
                    sortCol = HttpContext.Current.Session["sColumn"].ToString();
                if (HttpContext.Current.Session["fcolopr"] != null)
                    filterOpr = HttpContext.Current.Session["fcolopr"].ToString();
                if (HttpContext.Current.Session["fCol"] != null)
                    filterCol = HttpContext.Current.Session["fCol"].ToString();
                if (HttpContext.Current.Session["fColVal"] != null)
                    filterColVal = HttpContext.Current.Session["fColVal"].ToString();
                if (HttpContext.Current.Session["fColVal2"] != null)
                    filterColVal = HttpContext.Current.Session["fColVal2"].ToString();
                string typeIvOrLv = "";
                if (_typeIvOrLv != null)
                    typeIvOrLv = _typeIvOrLv.ToString();

                string ivParams = string.Empty;
                if (HttpContext.Current.Session["AxIvExportParams-" + iName] != null)
                    ivParams = HttpContext.Current.Session["AxIvExportParams-" + iName].ToString();

                filename = "GetLView-" + iName;
                sid = HttpContext.Current.Session["nsessionid"].ToString();

                string curRecord = "";
                if (_curRecord != null)
                {
                    curRecord = _curRecord.ToString();
                }

                if (ivParams != string.Empty)
                {
                    StringBuilder sbPVal = new StringBuilder();
                    string[] pairs = ivParams.Split('~');
                    foreach (var pair in pairs)
                    {
                        string[] keyValue = pair.Split('♠');
                        string kyval = keyValue[1];
                        kyval = kyval.Replace("&grave;", "~");
                        kyval = kyval.Replace("&amp;", "&");
                        XElement element = new XElement(keyValue[0], kyval);
                        sbPVal.Append(element.ToString());
                        if (keyValue[0] != "" && keyValue[0].ToLower().ToString() == "axhiddencolumn")
                        {
                            foreach (string _thiscolName in kyval.Split(','))
                                axhiddencols.Add(_thiscolName);
                        }
                    }
                    ivParams = sbPVal.ToString();
                }
                errorLog = logobj.CreateLog("Getting Iview Data.", sid, filename, "new");
                string _thisList = string.Empty;
                if (typeIvOrLv == "true")
                    _thisList = " purpose=\"list\" ";
                if (curRecord == "")
                    nXml = "<root " + _thisList + " headercached='false' name ='" + iName + "' axpapp ='" + HttpContext.Current.Session["project"].ToString() + "' sessionid = '" + sid + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errorLog + "' pageno='0' firsttime='yes' sorder='" + sortOrd + "' scol='" + sortCol + "' fcolopr='" + filterOpr + "' fcolnm='" + filterCol + "' fcolval1='" + filterColVal + "' fcolval2='" + filterValue1 + "'><params> ";
                else
                    nXml = "<root " + _thisList + " headercached='false' name ='" + iName + "' axpapp ='" + HttpContext.Current.Session["project"].ToString() + "' sessionid = '" + sid + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errorLog + "' pageno='1' pagesize='" + curRecord + "' firsttime='yes' sqlpagination='true' getrowcount='false' gettotalrows='false' smartview='true'><params> ";
                nXml = nXml + ivParams;// objIview.iviewParams.IviewParamString;
                nXml = nXml + "</params>";
                nXml = nXml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

                string ires = string.Empty;
                ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                try
                {
                    objIview.requestJSON = true;
                    ires = objWebServiceExt.CallGetIViewWS(iName, nXml, "", objIview);
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
                JObject jsonObject = JObject.Parse(ires);
                JObject reporthf = (JObject)jsonObject["data"]["reporthf"];

                try
                {
                    string hideprojname = reporthf["hideprojname"].ToString();
                    if (hideprojname != null && hideprojname.ToString() == "@t")
                    {
                        hideProjeName = true;
                    }
                }
                catch (Exception ex) { }

                JObject header = (JObject)reporthf["header"];
                JObject footer = (JObject)reporthf["footer"];
                if (header != null)
                {
                    foreach (var item in header)
                    {
                        objIview.ReportHdrs.Add((string)item.Value["text"]);
                        hdrFont.Add((string)item.Value["font"]);
                        if (item.Value["header_aline"] != null)
                        {
                            hdrAlign.Add((string)item.Value["header_aline"]);
                        }
                        else if (hdrAlign.Count > 0 && hdrAlign[0] != null)
                        {
                            hdrAlign.Add(hdrAlign[0]);
                        }
                        else
                        {
                            hdrAlign.Add("@center");
                        }
                    }
                }
                if (footer != null)
                {
                    foreach (var item in footer)
                    {
                        hdrFooterName.Add((string)item.Value["text"]);
                        hdrFooterFont.Add((string)item.Value["font"]);
                        if (item.Value["footer_aline"] != null)
                        {
                            hdrFooterAlign.Add((string)item.Value["footer_aline"]);
                        }
                        else if (hdrFooterAlign.Count > 0 && hdrFooterAlign[0] != null)
                        {
                            hdrFooterAlign.Add(hdrFooterAlign[0]);
                        }
                        else
                        {
                            hdrFooterAlign.Add("@center");
                        }
                    }
                }

                dynamic _jsonObject = JsonConvert.DeserializeObject(ires);

                DataSet dataSet = new DataSet();
                JObject jsonObjectNew = _jsonObject.data.headrow;
                foreach (var property in jsonObjectNew.Properties())
                {
                    string propertyName = property.Name;
                    JToken hideToken = property.Value.SelectToken("@hide");
                    JToken typeToken = property.Value.SelectToken("@type");
                    JToken textToken = property.Value.SelectToken("#text");
                    string hideValue = hideToken != null ? hideToken.ToString() : null;
                    string typeValue = typeToken != null ? typeToken.ToString() : null;
                    string textValue = textToken != null ? textToken.ToString() : null;
                    if (propertyName == "rowno")
                    {
                        colHide.Add(hideValue);
                        colHead.Add("Sr. No.");
                        colType.Add("c");
                    }
                    else if (propertyName != "pivotghead")
                    {
                        colHide.Add(hideValue);
                        colHead.Add(textValue);
                        colType.Add(typeValue);
                    }
                }


                // Convert rows to DataTable
                DataTable rowsTable = new DataTable("row");

                // Add columns dynamically
                foreach (var row in _jsonObject.data.row)
                {
                    foreach (JProperty property in row)
                    {
                        if (!rowsTable.Columns.Contains(property.Name))
                        {
                            rowsTable.Columns.Add(new DataColumn(property.Name, typeof(string)));
                        }
                    }
                }

                // Add rows dynamically
                foreach (var row in _jsonObject.data.row)
                {
                    DataRow dataRow = rowsTable.NewRow();
                    foreach (JProperty property in row)
                    {
                        dataRow[property.Name] = property.Value.ToString();
                    }
                    rowsTable.Rows.Add(dataRow);
                }
                dataSet.Tables.Add(rowsTable);

                dt = dataSet.Tables["row"].Copy();
            }
            catch (Exception Ex) { }
        }

        int dtcount = dt.Columns.Count;
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].ColumnName.Contains("html_"))
                {
                    colHide[j] = "false";
                    string oldColName = dt.Columns[j].ColumnName.Replace("html_", "");

                    // to hide the columns if column is hidden and column with prefix as html_
                    if (objIview.ShowHideCols != null)
                    {
                        if (objIview.ShowHideCols.IndexOf(oldColName) > -1)
                            colHide[j] = "true";
                    }
                }
            }

            int count = 0;
            foreach (DataRow item in dt.Rows)
            {
                foreach (var cell in item.ItemArray)
                {
                    if (cell.ToString().ToLower() == "subhead")
                        arrSubhead.Add(count);
                }
                count++;
            }
            if (dt.Columns.Count < colHide.Count)
            {
                colHide.RemoveAt(1);
                colHide.RemoveAt(1);

                colHead.RemoveAt(1);
                colHead.RemoveAt(1);

                colType.RemoveAt(1);
                colType.RemoveAt(1);
            }

            if (dt.Columns.Count == colHide.Count)
            {
                for (int i = colHide.Count - 1; i >= 0; i--)
                    if (colHide[i].ToString() == "true" || dt.Columns[i].ColumnName == "axrowtype" || dt.Columns[i].ColumnName == "rowno" || dt.Columns[i].ColumnName == "axp__font" || dt.Columns[i].ColumnName == "axp__color" || dt.Columns[i].ColumnName == "axrowoptions")
                        //if hidden column are used in hide column 
                        if (objIview.ShowHiddengridCols != null)
                            if (objIview.ShowHiddengridCols.IndexOf(dt.Columns[i].ColumnName) > -1)
                            {
                                // To remove the normal column if column cantains html_column and is visible
                                int indx = -1;
                                string removeColumn = string.Empty;
                                if (dt.Columns[i].ColumnName.Contains("html_"))
                                {
                                    //do nothing
                                }
                                else
                                {
                                    removeColumn = "html_" + dt.Columns[i].ColumnName;
                                }
                            }
                            else
                            {
                                dt.Columns.RemoveAt(i);
                                colHead.RemoveAt(i);
                                colHide.RemoveAt(i);
                                colType.RemoveAt(i);
                            }
                        else
                        {
                            dt.Columns.RemoveAt(i);
                            colHead.RemoveAt(i);
                            colHide.RemoveAt(i);
                            colType.RemoveAt(i);
                        }
            }


            for (int hIdx = dt.Columns.Count - 1; hIdx >= 0; hIdx--)
            {
                int cIdx = -1, sHIdx = -1;
                cIdx = colHide.IndexOf(dt.Columns[hIdx].ColumnName);

                //condition for removeing hide columns which are checked in gridview 
                if (objIview.ShowHideCols != null)
                    sHIdx = objIview.ShowHideCols.IndexOf(dt.Columns[hIdx].ColumnName);

                if ((cIdx != -1) || (sHIdx != -1))
                {
                    dt.Columns.RemoveAt(hIdx);
                    colHead.RemoveAt(hIdx);
                }
            }
            //string _fileName = CreateDocumentMobile(dt, objIview, ivname, _ivParamCaption);

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(stream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                    {
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        SectionProperties sectionProps = new SectionProperties();
                        Stack<HeaderReference> headerReferences = new Stack<HeaderReference>();
                        int fontSize = 12;
                        HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();
                        Header header = new Header();
                        if (!hideProjeName && headerText.ToString() != "")
                        {
                            Paragraph headerParagraph = CreateHeaderParagraphMobile(headerText.ToString(), fontSize * 2, JustificationValues.Center);
                            header.Append(headerParagraph);
                        }
                        if (objIview.ReportHdrs.Count > 0)
                        {
                            for (int i = 0; i < objIview.ReportHdrs.Count; i++)
                            {
                                JustificationValues justification = JustificationValues.Center;
                                string _alignment = hdrAlign[i].ToString();
                                if (_alignment == "@left")
                                    justification = JustificationValues.Left;
                                else if (_alignment == "@right")
                                    justification = JustificationValues.Right;

                                Paragraph headerParagraph = CreateHeaderParagraphMobile(StringReplaceSpecialChar(objIview.ReportHdrs[i].ToString()), fontSize * 2, justification);
                                header.Append(headerParagraph);
                            }
                        }
                        else
                        {
                            Paragraph headerParagraph = CreateHeaderParagraphMobile(objIview.IviewCaption, fontSize * 2, JustificationValues.Center);
                            header.Append(headerParagraph);
                        }
                        headerPart.Header = header;
                        headerPart.Header.Save();
                        HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                        headerReferences.Push(headerReference);
                        while (headerReferences.Count > 0)
                        {
                            sectionProps.Append(headerReferences.Pop());
                        }

                        Stack<FooterReference> footerRefeesrences = new Stack<FooterReference>();
                        FooterPart footerPart = mainPart.AddNewPart<FooterPart>();
                        Footer footer = new Footer();
                        if (hdrFooterName.Count > 0)
                        {
                            for (int i = 0; i < hdrFooterName.Count; i++)
                            {
                                Paragraph footerParagraph = new Paragraph();
                                Run _footerRun = new Run();
                                RunProperties _runFooterProperties = new RunProperties();
                                Bold _footerbold = new Bold();
                                _runFooterProperties.Append(_footerbold);
                                FontSize footerfontSizeElement = new FontSize() { Val = (fontSize * 2).ToString() };
                                _runFooterProperties.Append(footerfontSizeElement);
                                Color footercolor = new Color() { Val = "000000" }; // Hex color code for black
                                _runFooterProperties.Append(footercolor);
                                _footerRun.Append(_runFooterProperties);
                                Text _footerText = new Text(hdrFooterName[i].ToString());
                                _footerRun.Append(_footerText);
                                ParagraphProperties _paragraphProperties = new ParagraphProperties();
                                string _alignment = hdrFooterAlign[i].ToString();
                                Justification _justification;

                                switch (_alignment)
                                {
                                    case "":
                                    case "@center":
                                        _justification = new Justification() { Val = JustificationValues.Center };
                                        break;
                                    case "@left":
                                        _justification = new Justification() { Val = JustificationValues.Left };
                                        break;
                                    case "@right":
                                        _justification = new Justification() { Val = JustificationValues.Right };
                                        break;
                                    default:
                                        _justification = new Justification() { Val = JustificationValues.Left }; // Default to Left
                                        break;
                                }
                                SpacingBetweenLines _spacing = new SpacingBetweenLines()
                                {
                                    After = "0",
                                    Before = "0",
                                    Line = "0", // Adjust to control the line spacing
                                    LineRule = LineSpacingRuleValues.Exact
                                };
                                _paragraphProperties.Append(_justification);
                                _paragraphProperties.Append(_spacing);
                                footerParagraph.Append(_paragraphProperties);
                                footerParagraph.Append(_footerRun);
                                footer.Append(footerParagraph);

                            }
                        }
                        if (!string.IsNullOrEmpty(objIview.IviewFooter))
                        {
                            objIview.IsWordWithHtml = false;
                            Paragraph blankParagraph = new Paragraph();
                            Run blankRun = new Run();
                            Text blankText = new Text(" ");
                            blankRun.Append(blankText);
                            blankParagraph.Append(blankRun);
                            footer.Append(blankParagraph);
                            string[] footerText = objIview.IviewFooter.Split('|');
                            foreach (var text in footerText)
                            {
                                Paragraph footerParagraph = new Paragraph();
                                Run _footerRun = new Run();
                                RunProperties _runFooterProperties = new RunProperties();
                                Bold _footerbold = new Bold();
                                _runFooterProperties.Append(_footerbold);
                                FontSize footerfontSizeElement = new FontSize() { Val = (fontSize * 2).ToString() };
                                _runFooterProperties.Append(footerfontSizeElement);
                                Color footercolor = new Color() { Val = "000000" };
                                _runFooterProperties.Append(footercolor);
                                _footerRun.Append(_runFooterProperties);
                                Text _footerText = new Text(StringReplaceSpecialChar(text));
                                _footerRun.Append(_footerText);
                                ParagraphProperties _paragraphProperties = new ParagraphProperties();
                                Justification _justification = new Justification() { Val = JustificationValues.Center };
                                SpacingBetweenLines _spacing = new SpacingBetweenLines()
                                {
                                    After = "0",
                                    Before = "0",
                                    Line = "0", // Adjust to control the line spacing
                                    LineRule = LineSpacingRuleValues.Exact
                                };
                                _paragraphProperties.Append(_justification);
                                _paragraphProperties.Append(_spacing);
                                footerParagraph.Append(_paragraphProperties);
                                footerParagraph.Append(_footerRun);
                                footer.Append(footerParagraph);
                            }
                        }
                        footerPart.Footer = footer;

                        Footer footer2 = new Footer();
                        string _currentUser = string.Empty;
                        if (HttpContext.Current.Session["username"] != null && HttpContext.Current.Session["username"].ToString() != "")
                        {
                            _currentUser = HttpContext.Current.Session["username"].ToString();
                        }
                        if (_currentUser != string.Empty)
                        {
                            string PrintedBy = "Printed by: " + _currentUser + " and Printed On: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            Paragraph footerParagraph = new Paragraph();
                            Run _footerRun = new Run();
                            Text _footerText = new Text(PrintedBy);
                            _footerRun.Append(_footerText);
                            ParagraphProperties _paragraphProperties = new ParagraphProperties();
                            Justification _justification = new Justification() { Val = JustificationValues.Right };
                            SpacingBetweenLines _spacing = new SpacingBetweenLines()
                            {
                                After = "0",
                                Before = "0",
                                Line = "0",
                                LineRule = LineSpacingRuleValues.Exact
                            };
                            _paragraphProperties.Append(_justification);
                            _paragraphProperties.Append(_spacing);
                            footerParagraph.Append(_paragraphProperties);
                            footerParagraph.Append(_footerRun);
                            footer2.Append(footerParagraph);
                        }

                        footerPart.Footer = footer2;

                        footerPart.Footer.Save();
                        FooterReference footerReference = new FooterReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(footerPart) };
                        footerRefeesrences.Push(footerReference);
                        while (footerRefeesrences.Count > 0)
                        {
                            sectionProps.Append(footerRefeesrences.Pop());
                        }
                        if (HttpContext.Current.Session["AxWordOrientation"] != null && HttpContext.Current.Session["AxWordOrientation"].ToString() != "")
                        {
                            if (HttpContext.Current.Session["AxWordOrientation"].ToString().ToLower() == "landscape")
                            {
                                PageSize pageSize = new PageSize()
                                {
                                    Width = 16838,  // 11.69 inches = 297 mm (A4 width in landscape) in twips
                                    Height = 11906, // 8.27 inches = 210 mm (A4 height) in twips
                                    Orient = PageOrientationValues.Landscape
                                };
                                sectionProps.Append(pageSize);
                            }
                            else
                            {
                                PageSize pageSize = new PageSize()
                                {
                                    Orient = PageOrientationValues.Portrait
                                };
                                sectionProps.Append(pageSize);
                            }
                        }
                        else
                        {
                            PageSize pageSize = new PageSize()
                            {
                                Orient = PageOrientationValues.Portrait
                            };
                            sectionProps.Append(pageSize);
                        }
                        if (HttpContext.Current.Session["AxWordMargin"] != null && HttpContext.Current.Session["AxWordMargin"].ToString() != "")
                        {
                            string wordMargins = HttpContext.Current.Session["AxWordMargin"].ToString();
                            Dictionary<string, string> dimensions = JsonConvert.DeserializeObject<Dictionary<string, string>>(wordMargins);
                            int top = 567;
                            int bottom = 567;
                            int left = 567;
                            int right = 567;
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
                            PageMargin margins = new PageMargin()
                            {
                                Top = top,
                                Bottom = bottom,
                                Left = new UInt32Value((uint)left),
                                Right = new UInt32Value((uint)right),
                                Header = new UInt32Value((uint)top),
                                Footer = new UInt32Value((uint)bottom)
                            };
                            sectionProps.Append(margins);
                        }
                        else
                        {
                            PageMargin margins = new PageMargin()
                            {
                                Top = 567,//1 cm
                                Bottom = 567,
                                Left = 567,
                                Right = 567,
                                Header = 567,
                                Footer = 567
                            };
                            sectionProps.Append(margins);
                        }

                        body.Append(sectionProps);
                        string iName = string.Empty;
                        if (HttpContext.Current.Session["ivname"] != null)
                            iName = HttpContext.Current.Session["ivname"].ToString();
                        else if (ivname != null)
                            iName = ivname.ToString();
                        string ivParams = string.Empty;
                        if (_ivParamCaption != null)
                            ivParams = _ivParamCaption.ToString();
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

                        foreach (string pval in _paramList)
                        {
                            Paragraph paragraph = new Paragraph();
                            ParagraphProperties paragraphProperties = new ParagraphProperties();
                            Justification justification = new Justification() { Val = JustificationValues.Left };
                            SpacingBetweenLines spacing = new SpacingBetweenLines()
                            {
                                Before = "0", // No space before the paragraph
                                After = "0",  // No space after the paragraph
                                Line = "240", // Single line spacing (12pt)
                                LineRule = LineSpacingRuleValues.Exact
                            };
                            paragraphProperties.Append(justification);
                            paragraphProperties.Append(spacing);
                            paragraph.Append(paragraphProperties);
                            Run run = new Run();
                            RunProperties runProperties = new RunProperties();
                            FontSize fontSizeElement = new FontSize() { Val = "22" }; // Font size (11pt)
                            runProperties.Append(fontSizeElement);
                            Color color = new Color() { Val = "000000" }; // Font color (black)
                            runProperties.Append(color);
                            run.Append(runProperties);
                            Text text = new Text(pval);
                            run.Append(text);
                            paragraph.Append(run);
                            mainPart.Document.Body.Append(paragraph);
                        }

                        Table table = new Table();
                        TableProperties props = new TableProperties(
                            new TableBorders(
                                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                                new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                            )
                        );
                        TableLayout tableLayout = new TableLayout() { Type = TableLayoutValues.Autofit };
                        //TableWidth tableWidth = new TableWidth() { Type = TableWidthUnitValues.Auto };
                        //props.Append(tableWidth);
                        props.Append(tableLayout);

                        TableCellMarginDefault tableCellMarginDefault = new TableCellMarginDefault(
                            new TopMargin { Width = "50", Type = TableWidthUnitValues.Dxa },
                            new BottomMargin { Width = "50", Type = TableWidthUnitValues.Dxa }, // Changed to positive value
                            new LeftMargin { Width = "50", Type = TableWidthUnitValues.Dxa },
                            new RightMargin { Width = "50", Type = TableWidthUnitValues.Dxa }
                        );
                        props.Append(tableCellMarginDefault);
                        table.AppendChild<TableProperties>(props);

                        if (dt.Rows[0].Table.Columns.Contains("rowdata_Id"))
                            dt.Columns.Remove("rowdata_Id");
                        if (dt.Rows[0].Table.Columns.Contains("axrowtype"))
                            dt.Columns.Remove("axrowtype");
                        if (dt.Rows[0].Table.Columns.Contains("axp__font"))
                            dt.Columns.Remove("axp__font");
                        if (dt.Rows[0].Table.Columns.Contains("axp__color"))
                            dt.Columns.Remove("axp__color");
                        if (dt.Rows[0].Table.Columns.Contains("axrowoptions"))
                            dt.Columns.Remove("axrowoptions");
                        TableStrings ts = new TableStrings(12);
                        CellString cs = new CellString(12);
                        TableRow headerRow = new TableRow();
                        int ih = -1;
                        foreach (DataColumn column in dt.Columns)
                        {
                            ih++;
                            if (column.ToString() != colHead[ih].ToString() && colHide[ih].ToString() == "true")
                                continue;
                            else if (column.ToString() == colHead[ih].ToString() && colHide[ih].ToString() == "true")
                                continue;
                            else if (axhiddencols.IndexOf(column.ToString()) > -1)// (axhiddencols.IndexOf(column) > -1)
                                continue;
                            string colName = colHead[ih].ToString();
                            if (colName == "Sr. No.")
                                colName = "Sr. No.";
                            Paragraph headerRowParagraph = new Paragraph();
                            Run headerRun = new Run();
                            RunProperties runProperties = new RunProperties();
                            Bold bold = new Bold();
                            runProperties.Append(bold);
                            headerRun.Append(runProperties);
                            headerRun.Append(new Text(StringReplaceSpecialChar(colName)));
                            ParagraphProperties paragraphProperties = new ParagraphProperties();
                            Justification justification = new Justification() { Val = JustificationValues.Center };
                            paragraphProperties.Append(justification);
                            headerRowParagraph.Append(paragraphProperties);
                            headerRowParagraph.Append(headerRun);
                            TableCell headerCell = new TableCell(headerRowParagraph);
                            TableCellProperties cellProperties = new TableCellProperties();
                            DocumentFormat.OpenXml.Wordprocessing.Shading cellShading = new DocumentFormat.OpenXml.Wordprocessing.Shading()
                            {
                                Color = "auto",
                                Fill = "D9D9D9",
                                Val = ShadingPatternValues.Clear
                            };
                            cellProperties.Append(cellShading);
                            headerCell.Append(cellProperties);
                            headerRow.Append(headerCell);
                        }
                        table.Append(headerRow);
                        int srNoIndex = -1;
                        int cellIndex = -1;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TableRow dataRow = new TableRow();
                            bool isGTRow = false;
                            bool addGrandTotal = false;
                            DataRow dr = dt.Rows[i];
                            string rowString = string.Empty;
                            int isFirstCol = 0;
                            for (int c = 0; c < dt.Rows[i].Table.Columns.Count; c++)
                            {
                                if (dt.Rows[0].Table.Columns[c].ToString() != colHead[c].ToString() && colHide[c].ToString() == "true")
                                    continue;
                                else if (dt.Rows[0].Table.Columns[c].ToString() == colHead[c].ToString() && colHide[c].ToString() == "true")
                                    continue;
                                else if (axhiddencols.IndexOf(dt.Rows[i].Table.Columns[c].ToString()) > -1)
                                    continue;
                                string colValue = StringReplaceSpecialCharNew(dr[dt.Rows[i].Table.Columns[c]].ToString());
                                //if (arrSubhead.Count > 0 && int.Parse(arrSubhead[0].ToString()) == i)
                                //{
                                //    if (cellIndex == -1)
                                //        for (int a = 0; a < dt.Rows[i].Table.Columns.Count; a++)
                                //            if (dr[dt.Rows[i].Table.Columns[a]].ToString() == "")
                                //                continue;
                                //            else
                                //            {
                                //                cellIndex = a;
                                //                break;
                                //            }

                                //    string cellText = dr[dt.Rows[i].Table.Columns[cellIndex]].ToString();
                                //    string cleanedText = StringReplaceSpecialChar(cellText);
                                //    TableCell dataCell = new TableCell(new Paragraph(new Run(new Text(cleanedText))));
                                //    dataRow.Append(dataCell);
                                //    arrSubhead.Remove(arrSubhead[c]);
                                //    break;
                                //}
                                //else
                                //{
                                bool isBoldComplete = false;
                                if (colValue == "Grand Total" && srNoIndex != -1)
                                {
                                    addGrandTotal = true;
                                    colValue = "";
                                    isGTRow = true;
                                }
                                if ((addGrandTotal) && (colValue == "") && (isFirstCol != 0) && (srNoIndex != -1))
                                {
                                    colValue = "Grand Total";
                                    addGrandTotal = false;
                                    isBoldComplete = true;
                                    Text cellText = new Text(!string.IsNullOrEmpty(colValue) ? colValue : "-");
                                    Run run = new Run();
                                    RunProperties runProperties = new RunProperties();
                                    Bold bold = new Bold();
                                    runProperties.Append(bold);
                                    run.Append(runProperties);
                                    run.Append(cellText);
                                    Paragraph paragraph = new Paragraph(run);
                                    TableCell dataCell = new TableCell(paragraph);
                                    dataRow.Append(dataCell);
                                }
                                if (!isBoldComplete)
                                {
                                    if (isGTRow)
                                    {
                                        Text cellText = new Text(!string.IsNullOrEmpty(colValue) ? colValue : "-");
                                        Run run = new Run();
                                        RunProperties runProperties = new RunProperties();
                                        Bold bold = new Bold();
                                        runProperties.Append(bold);
                                        run.Append(runProperties);
                                        run.Append(cellText);
                                        Paragraph paragraph = new Paragraph(run);
                                        TableCell dataCell = new TableCell(paragraph);
                                        dataRow.Append(dataCell);
                                    }
                                    else
                                    {
                                        string[] breakTags = new[] { "\n", "<br>", "<br/>", "</br>" };
                                        if (breakTags.Any(tag => colValue.Contains(tag)))
                                        {
                                            TableCell dataCell = new TableCell();
                                            var paragraph = new Paragraph();
                                            string[] lines = colValue.Split(new[] { "\n", "<br>", "<br/>", "</br>" }, StringSplitOptions.None);
                                            foreach (var line in lines)
                                            {
                                                if (!string.IsNullOrWhiteSpace(line))
                                                {
                                                    paragraph.Append(new Run(new Text(line.Trim())));
                                                    paragraph.Append(new Run(new Break()));
                                                }
                                            }
                                            if (paragraph.Elements<Run>().Any())
                                            {
                                                var lastRun = paragraph.Elements<Run>().Last();
                                                if (lastRun.Elements<Break>().Any())
                                                {
                                                    lastRun.Remove();
                                                }
                                            }
                                            dataCell.Append(paragraph);
                                            dataRow.Append(dataCell);
                                        }
                                        else
                                        {
                                            TableCell dataCell = new TableCell(new Paragraph(new Run(new Text(!string.IsNullOrEmpty(colValue) ? colValue : "-"))));
                                            dataRow.Append(dataCell);
                                        }
                                    }
                                }
                                //}
                                isFirstCol = isFirstCol + 1;
                            }
                            table.Append(dataRow);
                        }
                        body.Append(table);
                        mainPart.Document.Save();
                    }


                    _fileName = objIview.IviewCaption.Replace(",", "");
                    _fileName = _fileName.Replace(" ", "_");
                    string strPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                    strPath = strPath + @"Axpert\" + sid + @"\";
                    string fullPath = Path.Combine(strPath, _fileName + ".doc");
                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.WriteTo(fileStream);
                    }
                    _fileName = _fileName + ".doc";
                }
            }
            catch (Exception ex) { }
            return _fileName;
        }
        else
            return "error";
    }

    // Helper Method to Create a Paragraph with Specific Properties
    static Paragraph CreateHeaderParagraphMobile(string text, int fontSize, JustificationValues justification)
    {
        Paragraph headerParagraph = new Paragraph();

        // Run for the text
        Run _headerRun = new Run();
        RunProperties _runProperties = new RunProperties();
        Bold _bold = new Bold();
        _runProperties.Append(_bold);
        FontSize fontSizeElement = new FontSize() { Val = fontSize.ToString() };
        _runProperties.Append(fontSizeElement);
        Color color = new Color() { Val = "000000" }; // Black color
        _runProperties.Append(color);
        _headerRun.Append(_runProperties);
        _headerRun.Append(new Text(text));

        ParagraphProperties _paragraphProperties = new ParagraphProperties();
        _paragraphProperties.Append(new Justification() { Val = justification });
        SpacingBetweenLines _spacing = new SpacingBetweenLines()
        {
            After = "0", // No space after the paragraph
            Before = "0", // No space before the paragraph
            //Line = "240", // Single line spacing (12pt)
            //LineRule = LineSpacingRuleValues.Exact,
        };
        _paragraphProperties.Append(_spacing);
        //ContextualSpacing contextualSpacing = new ContextualSpacing() { Val = true };
        //_paragraphProperties.Append(contextualSpacing);

        // Append properties and run to the paragraph
        headerParagraph.Append(_paragraphProperties);
        headerParagraph.Append(_headerRun);

        return headerParagraph;
    }
}



