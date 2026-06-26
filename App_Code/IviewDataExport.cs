using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client.Impl;
using Saml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
public class IviewDataExport
{
    public string GetExcelFast(string _ivName, string _ivKey, string _params, string ivCaption, string dateformat)
    {
        LogFile.Log logobj = new LogFile.Log();
        if (HttpContext.Current.Session["username"] == null)
        {
            return "error:SESSION_TIMEOUT";
        }
        string exportExcelPath = "";
        if (HttpContext.Current.Session["AxConfigFileUploadPath"] != null)
        {
            exportExcelPath = HttpContext.Current.Session["AxConfigFileUploadPath"].ToString();
            exportExcelPath = exportExcelPath.Replace(@"\", "\\\\");
        }
        if (string.IsNullOrEmpty(exportExcelPath))
        {
            return "error: Upload file path is empty.";
        }
        logobj.CreateLog("Iview export to excel", HttpContext.Current.Session.SessionID, "IviewExport-" + _ivName, "new");
        string _userName = HttpContext.Current.Session["username"].ToString();
        string axConfigFileMapUser = "";
        string axConfigFileMapPwd = "";
        if (HttpContext.Current.Session["AxConfigFileMapUser"] != null)
            axConfigFileMapUser = HttpContext.Current.Session["AxConfigFileMapUser"].ToString();
        if (HttpContext.Current.Session["AxConfigFileMapPwd"] != null)
            axConfigFileMapPwd = HttpContext.Current.Session["AxConfigFileMapPwd"].ToString();

        IviewData objIview = (IviewData)HttpContext.Current.Session[_ivKey];
        string reportHFJson = objIview.reportHF;
        string headerJson = objIview.headerJSON;
        string ivDataResJson = objIview.ivDataRes;
        ivDataResJson = ivDataResJson.Split(new[] { "#$♥#" }, StringSplitOptions.None)[0];
        logobj.CreateLog("ivDataResJson:" + ivDataResJson, HttpContext.Current.Session.SessionID, "IviewExport-" + _ivName, "");
        JObject result = new JObject();
        result["reporthf"] = JObject.Parse(reportHFJson);
        result["metadata"] = JObject.Parse(headerJson);
        JObject dataObj = JObject.Parse(ivDataResJson);
        result["row"] = dataObj["data"]["row"];
        XmlDocument _xmlDoc = new XmlDocument();
        XmlElement root = _xmlDoc.CreateElement("data");
        _xmlDoc.AppendChild(root);
        XmlDocument tempDoc = JsonConvert.DeserializeXmlNode("{\"reporthf\":" + result["reporthf"].ToString() + "}", "data");
        XmlNode reporthfNode = _xmlDoc.ImportNode(tempDoc.DocumentElement.FirstChild, true); root.AppendChild(reporthfNode);
        tempDoc = JsonConvert.DeserializeXmlNode("{\"headrow\":" + result["metadata"].ToString() + "}", "data");
        XmlNode headrowNode = _xmlDoc.ImportNode(tempDoc.DocumentElement.FirstChild, true);
        root.AppendChild(headrowNode);
        JArray rows = (JArray)result["row"];
        foreach (JObject row in rows)
        {
            XmlElement rowElement = _xmlDoc.CreateElement("row");
            foreach (JProperty prop in row.Properties())
            {
                XmlElement col = _xmlDoc.CreateElement(prop.Name);
                col.InnerText = prop.Value.ToString();
                rowElement.AppendChild(col);
            }
            root.AppendChild(rowElement);
        }

        string ires = _xmlDoc.OuterXml;
        XDocument doc = XDocument.Parse(@ires);
        var elementsToRemove = doc.Descendants().Where(e => string.Equals(e.Name.LocalName, "axrowtype", StringComparison.OrdinalIgnoreCase) || string.Equals(e.Name.LocalName, "axp__font", StringComparison.OrdinalIgnoreCase) || string.Equals(e.Name.LocalName, "axp__color", StringComparison.OrdinalIgnoreCase) || string.Equals(e.Name.LocalName, "axrowoptions", StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var el in elementsToRemove)
            el.Remove();
        ires = doc.ToString();
        string outputPath = string.Empty;
        using (var wb = new XLWorkbook())
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ires))
                    return "IView Result Error";
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(ires);
                var headRow = xmlDoc.SelectSingleNode("//headrow");
                var dataRows = xmlDoc.SelectNodes("//row");
                var reportHF = xmlDoc.SelectNodes("//data/reporthf");
                if (headRow == null || dataRows == null)
                    return "Invalid XML structure";
                var columns = new List<ColDef>();
                var htmlCols = new HashSet<string>();
                foreach (XmlNode col in headRow.ChildNodes)
                {
                    if (col.Name == "axrowtype" || col.Name == "axp__font" || col.Name == "axp__color" || col.Name == "axrowoptions" || col.Name == "rowno" || col.Name == "pivotghead")
                        continue;
                    if (col.Name.StartsWith("html_"))
                        htmlCols.Add(col.Name);
                    bool hidden = col.Name.StartsWith("hide_") || (col.Attributes != null && col.Attributes["hide"] != null && col.Attributes["hide"].Value == "true");
                    string colAlign = "left";
                    if (col.Attributes != null && col.Attributes["align"] != null)
                    {
                        colAlign = col.Attributes["align"].Value;
                    }
                    string colType = "c";
                    if (col.Attributes != null && col.Attributes["type"] != null)
                    {
                        colType = col.Attributes["type"].Value;
                    }
                    columns.Add(new ColDef
                    {
                        Name = col.Name,
                        Caption = CleanExcelValue(
                            col.InnerXml.Replace("<hide>true</hide>", "")
                                        .Replace("<hide>false</hide>", "")
                        ),
                        Hidden = hidden,
                        Align = colAlign,
                        dType = colType
                    });
                }

                foreach (var c in columns)
                {
                    if (htmlCols.Contains(c.Name)) c.Hidden = false;
                    if (htmlCols.Contains("html_" + c.Name)) c.Hidden = true;
                }

                var visibleCols = columns.Where(c => !c.Hidden).ToList();
                int colCount = visibleCols.Count;

                var ws = wb.Worksheets.Add("Report");

                int rowIndex = 1;

                // ===== HEADER =====
                bool headerWritten = false;
                if (reportHF != null)
                {
                    foreach (XmlNode hf in reportHF)
                    {
                        var headerNode = hf.SelectSingleNode("header");
                        if (headerNode == null) continue;

                        foreach (XmlNode h in headerNode.ChildNodes)
                        {
                            XmlNode textNode = h.SelectSingleNode("text");
                            string caption = CleanExcelValue(textNode != null ? textNode.InnerText : "");

                            XmlNode alignNode = h.SelectSingleNode("header_aline");
                            string align = alignNode != null ? alignNode.InnerText : "@center";

                            if (string.IsNullOrWhiteSpace(caption))
                                continue;

                            var r = ws.Range(rowIndex, 1, rowIndex, colCount);
                            r.Merge();
                            r.Value = caption;

                            // EXACT OLD ALIGN LOGIC
                            if (align == "" || align == "@center")
                                r.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            else if (align == "@left")
                                r.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            else if (align == "@right")
                                r.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                            r.Style.Font.Bold = true;
                            r.Style.Font.FontColor = XLColor.Black;

                            rowIndex++;
                            headerWritten = true;
                        }
                    }
                }

                if (!headerWritten)
                {
                    var r = ws.Range(rowIndex, 1, rowIndex, colCount);
                    r.Merge();
                    r.Value = CleanExcelValue(ivCaption);
                    r.Style.Font.Bold = true;
                    r.Style.Font.FontSize = 14;
                    r.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rowIndex++;
                }
                string iviewParamsJsonCap = "{}";
                if (_params != "")
                {
                    var pairs = _params.Split(new[] { "♥" }, StringSplitOptions.None);
                    pairs[0] = pairs[0].Trim('"');
                    pairs[pairs.Length - 1] = pairs[pairs.Length - 1].Trim('"');
                    var dictionary = new Dictionary<string, string>();
                    foreach (var pair in pairs)
                    {
                        if (pair != "")
                        {
                            var keyValue = pair.Split(':');
                            if (keyValue.Length == 2)
                            {
                                string kyval = keyValue[1];
                                kyval = kyval.Replace("&grave;", "~").Replace("&amp;", "&");
                                dictionary[keyValue[0]] = kyval;
                            }
                        }
                    }
                    iviewParamsJsonCap = JsonConvert.SerializeObject(dictionary);
                }

                // ===== PARAMS =====
                if (!string.IsNullOrWhiteSpace(iviewParamsJsonCap) && iviewParamsJsonCap != "{}")
                {
                    var json = JObject.Parse(iviewParamsJsonCap);
                    foreach (var p in json)
                    {
                        var r = ws.Range(rowIndex, 1, rowIndex, colCount);
                        r.Merge();
                        r.Value = CleanExcelValue(p.Key) + ": " + CleanExcelValue(p.Value.ToString());
                        r.Style.Font.Bold = true;
                        rowIndex++;
                    }
                    rowIndex++;
                }

                // ===== PIVOT HEADERS =====
                var pivotNode = headRow.SelectSingleNode("pivotghead");

                if (pivotNode != null)
                {
                    var allCols = new List<string>();

                    foreach (XmlNode col in headRow.ChildNodes)
                        if (col.Name != "axrowtype" && col.Name != "axp__font" && col.Name != "axp__color" && col.Name != "axrowoptions" && col.Name != "rowno" && col.Name != "pivotghead")
                            allCols.Add(col.Name);

                    var indexMap = new Dictionary<int, int>();
                    int excelCol = 1;

                    for (int i = 0; i < allCols.Count; i++)
                        if (!columns.First(c => c.Name == allCols[i]).Hidden)
                            indexMap[i + 1] = excelCol++;

                    var pxml = new XmlDocument();
                    pxml.LoadXml("<pivot>" + pivotNode.InnerXml + "</pivot>");

                    foreach (XmlNode head in pxml.SelectNodes("//head"))
                    {
                        int sn = int.Parse(head.SelectSingleNode("snno") != null ? head.SelectSingleNode("snno").InnerText : "0");
                        int en = int.Parse(head.SelectSingleNode("enno") != null ? head.SelectSingleNode("enno").InnerText : "0");
                        string ghead = CleanExcelValue(head.SelectSingleNode("ghead") != null ? head.SelectSingleNode("ghead").InnerText : string.Empty);

                        if (string.IsNullOrWhiteSpace(ghead)) continue;

                        var cols = indexMap.Where(m => m.Key >= sn && m.Key <= en)
                                           .Select(m => m.Value).OrderBy(x => x).ToList();

                        if (!cols.Any()) continue;

                        var r = ws.Range(rowIndex, cols.First(), rowIndex, cols.Last());
                        r.Merge();
                        r.Value = ghead;

                        r.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        r.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        r.Style.Alignment.WrapText = true;
                        r.Style.Font.Bold = true;
                        r.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    rowIndex++;
                }

                // ===== COLUMN HEADERS =====
                int colIndex = 1;
                foreach (var c in visibleCols)
                {
                    var cell = ws.Cell(rowIndex, colIndex++);
                    cell.Value = c.Caption;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    string align = c.Align != null ? c.Align.Trim().ToLowerInvariant() : string.Empty;
                    switch (align)
                    {
                        case "right":
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            break;

                        case "center":
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            break;

                        default:
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            break;
                    }
                }

                int headerRowIndex = rowIndex;
                rowIndex++;

                // ===== DATA =====
                foreach (XmlNode row in dataRows)
                {
                    colIndex = 1;
                    foreach (var c in visibleCols)
                    {
                        string raw = CleanExcelValue(row.SelectSingleNode(c.Name) != null ? row.SelectSingleNode(c.Name).InnerText : string.Empty);
                        var cell = ws.Cell(rowIndex, colIndex++);
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        if (string.IsNullOrWhiteSpace(raw))
                        {
                            cell.Value = "";
                            continue;
                        }
                        //if (c.dType?.Equals("n", StringComparison.OrdinalIgnoreCase) == true)
                        if (string.Equals(c.dType, "n", StringComparison.OrdinalIgnoreCase))
                        {
                            bool bracketNeg = raw.StartsWith("(") && raw.EndsWith(")");
                            bool hasComma = raw.Contains(",");
                            bool hasDecimal = raw.Contains(".");
                            if (bracketNeg)
                                raw = raw.Substring(1, raw.Length - 2);
                            decimal dec;
                            if (decimal.TryParse(raw, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out dec))
                            {
                                if (bracketNeg)
                                    dec *= -1;
                                cell.Value = dec;
                                string format;
                                if (hasComma)
                                {
                                    format = hasDecimal ? "#,##0.################" : "#,##0";
                                }
                                else
                                {
                                    format = hasDecimal ? "0.################" : "0";
                                }
                                cell.Style.NumberFormat.Format = format;
                            }
                            else
                            {
                                cell.Value = raw;
                            }
                        }
                        else if (string.Equals(c.dType, "d", StringComparison.OrdinalIgnoreCase))
                        {
                            DateTime dt;
                            if (DateTime.TryParseExact(raw, dateformat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                            {
                                cell.Value = dt;
                                cell.Style.DateFormat.Format = dateformat;
                            }
                            else
                            {
                                cell.Value = raw;
                            }
                        }
                        else
                        {
                            cell.Value = raw;
                        }
                        switch ((c.Align ?? string.Empty).Trim().ToLowerInvariant())
                        {
                            case "right":
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                break;

                            case "center":
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                break;

                            default:
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                break;
                        }
                    }
                    rowIndex++;
                }

                // ===== FOOTER =====
                if (reportHF != null)
                {
                    foreach (XmlNode hf in reportHF)
                    {
                        var footer = hf.SelectSingleNode("footer");
                        if (footer == null) continue;

                        foreach (XmlNode f in footer.ChildNodes)
                        {
                            string caption = CleanExcelValue(f.SelectSingleNode("text") != null ? f.SelectSingleNode("text").InnerText : string.Empty);
                            string align = f.SelectSingleNode("footer_aline") != null ? f.SelectSingleNode("footer_aline").InnerText : "@center";

                            if (string.IsNullOrWhiteSpace(caption))
                                continue;

                            var r = ws.Range(rowIndex, 1, rowIndex, colCount);
                            r.Merge();
                            r.Value = caption;

                            // EXACT OLD ALIGN BEHAVIOR
                            if (align == "" || align == "@center")
                                r.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            else if (align == "@left")
                                r.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            else if (align == "@right")
                                r.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                            r.Style.Font.Bold = true;
                            r.Style.Font.FontColor = XLColor.Black;

                            rowIndex++;
                        }
                    }
                }

                ws.Columns().AdjustToContents();
                ws.SheetView.FreezeRows(headerRowIndex);

                string fileBase = ivCaption + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName = (string.IsNullOrWhiteSpace(fileBase) ? _ivName : fileBase) + ".xlsx";
                fileName = fileName.Replace("/", "_");
                string sid = HttpContext.Current.Session["nsessionid"].ToString();
                string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                string exportFolder = Path.Combine(scriptsPath, "Axpert", sid);
                if (!Directory.Exists(exportFolder))
                    Directory.CreateDirectory(exportFolder);
                outputPath = Path.Combine(exportFolder, fileName);
                wb.SaveAs(outputPath);
                Util.Util utilObj = new Util.Util();
                outputPath = utilObj.ScriptsurlPath + "axpert/" + sid + "/" + fileName;
                logobj.CreateLog("Iview export to excel: outputPath:" + outputPath, HttpContext.Current.Session.SessionID, "IviewExport-" + _ivName, "");
                wb.Dispose();
            }
            catch (Exception ex)
            {
                wb.Dispose();
                logobj.CreateLog("Iview export to excel:" + ex.Message, HttpContext.Current.Session.SessionID, "IviewExport-" + _ivName, "new");
            }
        }
        return outputPath;
    }
    public static void ExecuteWithOptionalShareAccess(string uncPath, string user, string pwd, Action action)
    {
        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pwd))
        {
            action();
            return;
        }
        string sharePath = GetSharePath(uncPath);
        NetworkCredential credentials = new NetworkCredential(user, pwd);
        using (new NetworkConnection(sharePath, credentials))
        {
            action();
        }
    }
    private static string GetSharePath(string fullPath)
    {
        if (!fullPath.StartsWith(@"\\"))
            throw new Exception("Invalid UNC path");

        string[] parts = fullPath.Split(
            new char[] { '\\' },
            StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
            throw new Exception("Invalid UNC format");

        return string.Format(@"\\{0}\{1}", parts[0], parts[1]);
    }
    protected static string CleanExcelValue(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "";

        string prev;
        do
        {
            prev = input;
            input = WebUtility.HtmlDecode(input);
        } while (input != prev);

        var doc = new HtmlDocument();
        doc.LoadHtml(input);

        // Remove MS Office conditional junk
        foreach (var c in doc.DocumentNode.SelectNodes("//comment()") ?? Enumerable.Empty<HtmlNode>())
            if (c.InnerHtml.Contains("if gte mso"))
                c.Remove();

        // Remove style/script
        foreach (var n in doc.DocumentNode.SelectNodes("//style|//script") ?? Enumerable.Empty<HtmlNode>())
            n.Remove();

        // br → newline
        foreach (var br in doc.DocumentNode.SelectNodes("//br") ?? Enumerable.Empty<HtmlNode>())
            br.ParentNode.ReplaceChild(doc.CreateTextNode("\n"), br);

        // a → keep visible text
        foreach (var a in doc.DocumentNode.SelectNodes("//a") ?? Enumerable.Empty<HtmlNode>())
            a.ParentNode.ReplaceChild(doc.CreateTextNode(a.InnerText), a);

        // strip formatting tags but keep content
        foreach (var n in doc.DocumentNode.SelectNodes("//span|//font|//b|//strong|//p|//li|//ul|//ol")
                 ?? Enumerable.Empty<HtmlNode>())
            n.ParentNode.ReplaceChild(doc.CreateTextNode(n.InnerText), n);

        string clean = doc.DocumentNode.InnerText;

        clean = clean.Replace("~", Environment.NewLine);
        clean = Regex.Replace(clean, @"\s+", " ").Trim();

        return clean;
    }
}

class ColDef
{
    public string Name;
    public string Caption;
    public bool Hidden;
    public string Align;
    public string dType;
}