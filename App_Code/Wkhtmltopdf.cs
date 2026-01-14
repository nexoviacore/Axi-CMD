using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Hosting;

namespace Wkhtmltopdf
{
    public class PdfGenerator
    {
        public static string HtmlToPdf(string pdfOutputLocation, string outputFilenamePrefix, string urls, string type)
        {
            string[] options = null;
            string pdfHtmlToPdfExePath = string.Empty;
            if (HttpContext.Current.Session["AxPrintExePath"] != null)
                pdfHtmlToPdfExePath = HttpContext.Current.Session["AxPrintExePath"].ToString();
            string outputFilename = string.Empty;
            if (!string.IsNullOrEmpty(pdfHtmlToPdfExePath))
            {
                string urlsSeparatedBySpaces = string.Empty;
                try
                {
                    //Determine inputs
                    if ((urls == null) || (urls.Length == 0))
                        throw new Exception("No input URLs provided for HtmlToPdf");
                    else
                        urlsSeparatedBySpaces = urls; //Concatenate URLs

                    string outputFolder = pdfOutputLocation;
                    if (type == "iview")
                        outputFilename = outputFilenamePrefix + ".pdf";
                    else
                        outputFilename = outputFilenamePrefix + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_fff") + ".PDF"; // assemble destination PDF file name

                    var p = new System.Diagnostics.Process()
                    {
                        StartInfo =
                    {
                        FileName = pdfHtmlToPdfExePath,
                        Arguments = ((options == null) ? "" : String.Join(" ", options)) + " " + urlsSeparatedBySpaces + " " + outputFilename,
                        UseShellExecute = false, // needs to be false in order to redirect output
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
                        WorkingDirectory =outputFolder // HttpContext.Current.Server.MapPath(outputFolder)
            }
                    };

                    p.Start();

                    // read the output here...
                    //   var output = p.StandardOutput.ReadToEnd();
                    var errorOutput = p.StandardError.ReadToEnd();

                    // ...then wait n milliseconds for exit (as after exit, it can't read the output)
                    p.WaitForExit(60000);

                    // read the exit code, close process
                    int returnCode = p.ExitCode;
                    p.Close();

                    // if 0 or 2, it worked so return path of pdf
                    if ((returnCode == 0) || (returnCode == 2))
                        return outputFilename;
                    else
                        throw new Exception(errorOutput);
                }
                catch (Exception exc)
                {
                    if (type == "iview")
                        return outputFilename;
                    else
                        throw new Exception("Problem generating PDF from HTML, URLs: " + urlsSeparatedBySpaces + ", outputFilename: " + outputFilenamePrefix, exc);
                }
            }
            else
            {
                return "EXE path not defined";
            }
        }
    }
}