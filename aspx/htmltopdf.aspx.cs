using CacheMgr;
using HtmlAgilityPack;
using iTextSharp.text;
//using Pechkin;
//using Pechkin.Synchronized;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class htmltopdf : System.Web.UI.Page
{
    static Util.Util util = new Util.Util();
    static TStructDef strObj = null;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string Getpdftohtml(string key, string fname)
    {
        string dtnow = DateTime.Now.ToString();
        HttpContext.Current.Session["LastUpdatedSess"] = dtnow;
        if (HttpContext.Current.Session["project"] == null)
        {
            string url = "session~" + util.SESSEXPIRYPATH;
            return url;
        }
        else
        {
            string pdfFileurl = string.Empty;
            if (key != null && fname != null)
            {
                TStructData tsData = (TStructData)HttpContext.Current.Session[key];
                string fileName = fname;
                pdfFileurl = GetHTML(tsData, fileName);
            }
            return pdfFileurl;
        }
    }

    public static string GetHTML(TStructData tsData, string fileName)
    {
        string pdfFileurl = string.Empty;
        try
        {
            string filePath = string.Empty;

            if (HttpContext.Current.Session["AxHtmlPath"] != null)
            {
                filePath = HttpContext.Current.Session["AxHtmlPath"].ToString();

                if (!filePath.Contains(":\\"))
                {
                    filePath = AppDomain.CurrentDomain.BaseDirectory + "CommonDir\\" + filePath;
                    if (!Directory.Exists(filePath))
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                }
            }
            if (!string.IsNullOrEmpty(filePath))
            {
                filePath = filePath + "\\" + fileName;
                StringBuilder htmlContent = ReadHtmlFile(filePath);
                if (htmlContent.Length != 0)
                {
                    string htmltopdfParams = string.Empty;
                    String[] htmlParams = new String[] { };
                    CacheManager cacheMgr = util.GetCacheObject();
                    strObj = util.GetStrObject(cacheMgr, tsData.transID);
                    htmltopdfParams = strObj.htmlToPDF;

                    if (!string.IsNullOrEmpty(htmltopdfParams))
                    {
                        int indLenth = fileName.Length + 1;
                        string indOf = fileName + "{";
                        if (!htmltopdfParams.Contains(indOf))
                        {
                            indOf = fileName;
                            indLenth = indLenth - 1;
                        }
                        string output = htmltopdfParams.Substring(htmltopdfParams.IndexOf(indOf) + indLenth);
                        if (!string.IsNullOrEmpty(output))
                            htmlParams = output.Substring(0, output.IndexOf('}')).Split(',');
                    }
                    else
                        return "error~seeion expired";
                    for (int i = 0; i < tsData.tstStrObj.dcs.Count; i++)
                    {
                        try
                        {
                            TStructDef.DcStruct dcStr = (TStructDef.DcStruct)tsData.tstStrObj.dcs[i];
                            if (dcStr.isgrid)
                            {
                                DataTable dt = tsData.dsDataSet.Tables[dcStr.name];
                                if (htmlContent.ToString().IndexOf("~gr" + dcStr.name) != -1)
                                {
                                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                    doc.LoadHtml(htmlContent.ToString());
                                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//tbody[@id=\"~gr" + dcStr.name + "\"]");
                                    if (nodes == null)
                                        nodes = doc.DocumentNode.SelectNodes("//div[@id=\"~gr" + dcStr.name + "\"]");// incase div
                                    foreach (HtmlNode node in nodes)
                                    {
                                        string orgTableRow = node.InnerHtml;
                                        string uptTableRow = string.Empty;
                                        foreach (DataRow dtRow in dt.Rows)
                                        {
                                            uptTableRow += orgTableRow;
                                            foreach (DataColumn dc in dt.Columns)
                                            {
                                                if (orgTableRow.IndexOf(":" + dc.Caption) != -1)
                                                    uptTableRow = uptTableRow.Replace(":" + dc.Caption, dtRow[dc].ToString());
                                            }
                                        }
                                        htmlContent = htmlContent.Replace(orgTableRow, uptTableRow);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        { }
                    }
                    foreach (var relParam in htmlParams)
                    {
                        try
                        {
                            int valDc = tsData.tstStrObj.GetFieldDc(relParam.ToString());
                            string relVal = tsData.GetFieldValue(valDc.ToString(), relParam.ToString());
                            relVal = relVal.Replace("<br>", "");
                            htmlContent = htmlContent.Replace(":" + relParam, relVal);
                        }
                        catch (Exception)
                        { }
                    }
                    htmlContent = htmlContent.Replace("color: rgb(0, 0, 0);", "");
                    pdfFileurl = GeneratePDF(htmlContent);
                }
                else
                {
                    pdfFileurl = "error~HTML Template not found!";
                }
            }
            else
            {
                pdfFileurl = "error~Template path not found!";
            }
        }
        catch (Exception ex)
        {
            pdfFileurl = "error~" + ex.Message;
        }
        return pdfFileurl;
    }

    public static StringBuilder ReadHtmlFile(string htmlFileNameWithPath)
    {
        StringBuilder storeContent = new StringBuilder();
        try
        {
            using (StreamReader htmlReader = new StreamReader(htmlFileNameWithPath))
            {
                string lineStr;
                while ((lineStr = htmlReader.ReadLine()) != null)
                {
                    storeContent.Append(lineStr);
                }
            }
        }
        catch (Exception)
        { }
        return storeContent;
    }

    public static string GeneratePDF(StringBuilder htmlContent)
    {
        string pdfFileurl = string.Empty;
        string printUlr = string.Empty;

        if (HttpContext.Current.Session["AxHtmlPath"] != null)
        {
            printUlr = HttpContext.Current.Session["AxHtmlPath"].ToString();

            if (!printUlr.Contains(":\\"))
            {
                printUlr = AppDomain.CurrentDomain.BaseDirectory + "CommonDir\\" + printUlr;
                if (!Directory.Exists(printUlr))
                {
                    Directory.CreateDirectory(printUlr);
                }
            }
        }
        if (!string.IsNullOrEmpty(printUlr))
        {
            if (htmlContent.ToString().IndexOf("/filemanager/") > -1)
            {
                string mpath = HttpContext.Current.Server.MapPath(".").Replace("\\aspx", "").Replace("\\", "/");//printUlr.Replace("\\", "/"); 
                htmlContent = htmlContent.Replace("/filemanager/", mpath + "/filemanager/");
            }
            string strHTML = htmlContent.ToString();
            bool IsPrintExe = true;
            if (HttpContext.Current.Session["AxIsPrintExe"] != null)
                IsPrintExe = bool.Parse(HttpContext.Current.Session["AxIsPrintExe"].ToString());
            if (IsPrintExe == true || IsPrintExe == false)// Actually this condition for to check to use exe or dll, so now we dont have dll 
            {
                string tempHtmlfile = printUlr + "\\printTemplates\\temp_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + ".html";
                FileStream fs = new FileStream(tempHtmlfile, FileMode.Create);
                StreamWriter swXLS = new StreamWriter(fs, Encoding.UTF8);
                string hdcontent = string.Empty, ftcontent = string.Empty;
                int hspacing = 0, fspacing = 0;
                bool hdpage = false, ftpage = false;
                string pdfUlr = HttpContext.Current.Server.MapPath("../downloads/");
                var fileName = "";
                if (strHTML.ToString().ToLower().IndexOf("<header>") > -1 && strHTML.ToString().ToLower().IndexOf("</header>") > -1)
                {
                    hdpage = true;
                    hdcontent = strHTML.ToString().Substring(strHTML.ToString().IndexOf("<header>") + 8, strHTML.ToString().IndexOf("</header>") - (strHTML.ToString().IndexOf("<header>") + 8));
                    strHTML = strHTML.ToString().Replace(hdcontent, "");
                    hspacing = hdcontent.Length / 100;
                    string data1 = File.ReadAllText(printUlr + "\\printTemplates\\header.html");
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(data1);
                    string headercontent = doc.DocumentNode.InnerHtml + hdcontent;
                    File.WriteAllText(printUlr + "\\printTemplates\\header_content.html", headercontent);
                }
                if (strHTML.ToString().ToLower().IndexOf("<footer>") > -1 && strHTML.ToString().ToLower().IndexOf("</footer>") > -1)
                {
                    ftpage = true;
                    ftcontent = strHTML.ToString().Substring(strHTML.ToString().IndexOf("<footer>") + 8, strHTML.ToString().IndexOf("</footer>") - (strHTML.ToString().IndexOf("<footer>") + 8));
                    strHTML = strHTML.ToString().Replace(ftcontent, "");
                    fspacing = ftcontent.Length / 100;
                    string data1 = File.ReadAllText(printUlr + "\\printTemplates\\footer.html");
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(data1);
                    string headercontent = doc.DocumentNode.InnerHtml;
                    headercontent = headercontent.Replace("<footer></footer>", ftcontent);
                    File.WriteAllText(printUlr + "\\printTemplates\\footer_content.html", headercontent);
                }
                swXLS.WriteLine(strHTML.ToString());
                swXLS.Close();
                if (hdpage && ftpage)
                    fileName = Wkhtmltopdf.PdfGenerator.HtmlToPdf(pdfUlr, "discharge", " --header-spacing " + hspacing + " --header-html " + printUlr + "\\printTemplates\\" + "header_content.html --footer-spacing " + fspacing + " --footer-html " + printUlr + "\\printTemplates\\" + "footer_content.html " + tempHtmlfile + "", "html");
                else if (hdpage)
                    fileName = Wkhtmltopdf.PdfGenerator.HtmlToPdf(pdfUlr, "discharge", " --header-spacing " + hspacing + " --header-html " + printUlr + "\\printTemplates\\" + "header_content.html --footer-html " + printUlr + "\\printTemplates\\" + "footer.html " + tempHtmlfile + "", "html");
                else if (ftpage)
                    fileName = Wkhtmltopdf.PdfGenerator.HtmlToPdf(pdfUlr, "discharge", " --header-spacing " + hspacing + " --header-html " + printUlr + "\\printTemplates\\" + "header.html --footer-spacing " + fspacing + " --footer-html " + printUlr + "\\printTemplates\\" + "footer_content.html " + tempHtmlfile + "", "html");
                else
                    fileName = Wkhtmltopdf.PdfGenerator.HtmlToPdf(pdfUlr, "discharge", " --header-spacing " + hspacing + " --header-html " + printUlr + "\\printTemplates\\" + "header.html --footer-html " + printUlr + "\\printTemplates\\" + "footer.html " + tempHtmlfile + "", "html");

                if (!string.IsNullOrEmpty(fileName))
                {

                    if (fileName != "EXE path not defined")
                    {
                        if (File.Exists(pdfUlr + fileName))
                        {
                            File.Delete(tempHtmlfile);
                            pdfFileurl = "../downloads/" + fileName;
                        }
                        if (File.Exists(printUlr + "\\printTemplates\\header_content.html"))
                            File.Delete(printUlr + "\\printTemplates\\header_content.html");
                        if (File.Exists(printUlr + "\\printTemplates\\footer_content.html"))
                            File.Delete(printUlr + "\\printTemplates\\footer_content.html");
                    }
                    else
                    {
                        pdfFileurl = "error~" + fileName;
                    }
                }
            }
            else
            {
                string pdfUlr = HttpContext.Current.Server.MapPath("../downloads/");
                string PrintMargins = "", PrintTitle = "", PrintTitleAlign = "";
                if (HttpContext.Current.Session["AxPrintMargins"] != null)
                    PrintMargins = HttpContext.Current.Session["AxPrintMargins"].ToString();
                if (HttpContext.Current.Session["AxPrintTitle"] != null)
                    PrintTitle = HttpContext.Current.Session["AxPrintTitle"].ToString();
                if (HttpContext.Current.Session["AxPrintTitleAlign"] != null)
                    PrintTitleAlign = HttpContext.Current.Session["AxPrintTitleAlign"].ToString();
                var fileName = htmltopdfdll("discharge", pdfUlr, htmlContent.ToString(), PrintMargins, PrintTitle, PrintTitleAlign);
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (fileName != "EXE path not defined")
                    {
                        if (File.Exists(pdfUlr + fileName))
                        {
                            pdfFileurl = "../downloads/" + fileName;
                        }
                    }
                    else
                    {
                        pdfFileurl = "error~" + fileName;
                    }
                }
            }
        }
        else
        {
            pdfFileurl = "error~HTML Template path not defined!";
        }
        return pdfFileurl;
    }

    [WebMethod]
    public static string DeletePrintPDF(string pdfPath)
    {
        if (File.Exists(HttpContext.Current.Server.MapPath(pdfPath)))
        {
            File.Delete(HttpContext.Current.Server.MapPath(pdfPath));
        }
        return "true";
    }


    public static string htmltopdfdll(string pdfName, string pdfUrl, string htmlText, string PrintMargins, string PrintTitle, string PrintTitleAlign)
    {
        //var globalConfig = new GlobalConfig();
        //globalConfig.SetPaperSize(PaperKind.Letter);

        //int left = 0, right = 0, top = 0, bottom = 0;
        //if (PrintMargins != "")
        //{
        //    try
        //    {
        //        left = Convert.ToInt32(PrintMargins.Split(',')[0]);
        //        right = Convert.ToInt32(PrintMargins.Split(',')[1]);
        //        top = Convert.ToInt32(PrintMargins.Split(',')[2]);
        //        bottom = Convert.ToInt32(PrintMargins.Split(',')[3]);
        //    }
        //    catch
        //    {
        //        left = 50; right = 50; top = 200; bottom = 100;
        //    }
        //}

        //globalConfig.SetMargins(new Margins(left, right, top, bottom));
        //var synchronizedPechkin = new SynchronizedPechkin(globalConfig);
        //var config = new ObjectConfig();
        //config.SetAllowLocalContent(true).SetPrintBackground(true);

        //if (PrintTitleAlign.ToLower() == "left")
        //    config.Header.SetLeftText(PrintTitle);
        //else if (PrintTitleAlign.ToLower() == "right")
        //    config.Header.SetRightText(PrintTitle);
        //else if (PrintTitleAlign.ToLower() == "center")
        //    config.Header.SetCenterText(PrintTitle);
        //if (PrintTitle != "")
        //    config.Header.SetLineSeparator(true);

        //config.Footer.SetLineSeparator(true);
        //config.Footer.SetRightText(@"Page [page] of [topage]");
        //var pdf = synchronizedPechkin.Convert(config, htmlText);
        //string outputFilename = pdfName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_fff") + ".PDF";
        //using (FileStream files = System.IO.File.Create(pdfUrl + outputFilename))
        //{
        //    files.Write(pdf, 0, pdf.Length);
        //}
        //return outputFilename;

        return "";
    }
}
