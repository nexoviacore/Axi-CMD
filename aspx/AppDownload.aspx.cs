using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class aspx_AppDownload : System.Web.UI.Page
{
    string GETSHEMA_API_DATA = "{0}";
    string GETSHEMA_API_LABEL = "appid";
    string GETSHEMA_API_URL = "api/ValidateAppUser/GetSchemaName/";
    public string direction = "ltr";
    Util.Util util = new Util.Util();

    public string langType = "en";
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

        Util.Util util = new Util.Util();
        util.IsValidSession();
        ResetSessionTime();
        if (Session["project"] == null || Convert.ToString(Session["project"]) == string.Empty)
        {
            SessExpires();
            return;
        }

        if (!IsPostBack)
            GetFileList();
    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }

    private static void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
    }
    protected void download_click(object sender, EventArgs e)
    {
        if (hiddenfilenames.Value != null && hiddenfilenames.Value != "")
        {
            string dirPath = ConfigurationManager.AppSettings["DumpPath"] + Session["LISTID"] + "\\";
            string[] files = hiddenfilenames.Value.ToString().Split('`');
            files = files.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (files.Length == 1 && Directory.Exists(dirPath))
            {
                if (Directory.Exists(dirPath))
                {
                    DirectoryInfo di = new DirectoryInfo(dirPath);
                    FileInfo[] EXEFiles = di.GetFiles(files[0]);
                    if (EXEFiles.Length == 1)
                    {
                        panel1.Visible = false;
                        download.Visible = true;
                        requestExeFile(EXEFiles[0].Name);
                    }
                }
            }
        }
    }
    protected void requestExeFile(string fileName)
    {
        string configPath = string.Empty;
        if (HttpContext.Current.Session["LISTID"] != null && ConfigurationManager.AppSettings["DumpPath"] != null && HttpContext.Current.Session["APPID"] != null)
        {
            string dirPath = ConfigurationManager.AppSettings["DumpPath"] + HttpContext.Current.Session["LISTID"] + "\\";
            if (Directory.Exists(dirPath))
            {
                string filePath = dirPath + fileName;

                try
                {
                    configPath = dirPath + HttpContext.Current.Session["APPID"].ToString();
                    string configFile = CreateConfigFile(configPath);
                    string readmeFile = dirPath + "\\Installer_Readme.txt";
                    string[] fileList = new string[] { filePath, configFile, readmeFile };
                    Download_ZIP(fileList);
                }
                catch (Exception ex) { }
                finally
                {
                    ClearFolder(configPath);
                }
            }
        }
    }
    private void ClearFolder(string FolderName)
    {
        if (!string.IsNullOrEmpty(FolderName) && Directory.Exists(FolderName))
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            dir.Delete();
        }
    }
    private string CreateConfigFile(string path)
    {
        string fileName = "config.ini";
        string schemaName = getSchemaName();
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if (Directory.Exists(path))
        {
            TextWriter tw = new StreamWriter(path + "\\" + fileName);
            tw.WriteLine("[Cloud]");
            tw.WriteLine(ConfigurationManager.AppSettings["MasterDataAPI"].ToString());
            tw.WriteLine(ConfigurationManager.AppSettings["PublishAPI"].ToString());
            tw.WriteLine("AppId=" + HttpContext.Current.Session["APPID"].ToString());
            tw.WriteLine("cloudschema=" + (string.IsNullOrEmpty(schemaName) ? "NotFound" : schemaName));
            tw.Close();
        }
        return path + "\\" + fileName;
    }
    private string getSchemaName_old()
    {
        if (HttpContext.Current.Session["axconstr"] != null)
        {
            //schemaName = HttpContext.Current.Session["axconstr"].ToString().Split(';')[1].Split('=')[1];
            string connStr = HttpContext.Current.Session["axconstr"].ToString();
            string[] connParams = connStr.Split(';');
            foreach (string str in connParams)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] attr = str.Split('=');
                    if (attr.Length > 0)
                    {
                        if (attr[0].ToString().ToLower() == "user id")
                        {
                            return attr[1];
                        }
                    }
                }
            }
        }
        return string.Empty;
    }
    private string getSchemaName()
    {
        string schemaName = string.Empty;
        if (HttpContext.Current.Session["APPID"] != null)
        {
            string jsonResult = string.Empty;
            try
            {
                jsonResult = this.ConsumePostApi(HttpContext.Current.Session["APPID"].ToString(), GETSHEMA_API_LABEL, GETSHEMA_API_URL);
            }
            catch (Exception Ex)
            { }
            if (!string.IsNullOrEmpty(jsonResult))
            {
                GenResult gres = JsonConvert.DeserializeObject<GenResult>(jsonResult);
                schemaName = gres.result;
            }
        }
        return schemaName;
    }
    protected void requestExeFile_1(string fileName)
    {
        if (HttpContext.Current.Session["LISTID"] != null && ConfigurationManager.AppSettings["DumpPath"] != null && HttpContext.Current.Session["APPID"] != null)
        {
            string dirPath = ConfigurationManager.AppSettings["DumpPath"] + HttpContext.Current.Session["LISTID"] + "\\";
            if (Directory.Exists(dirPath))
            {
                string filePath = dirPath + fileName;
                if (File.Exists(filePath))
                {
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + HttpContext.Current.Session["APPID"] + ".exe"); // replace a.html with your filename
                    Response.WriteFile(@"" + dirPath + fileName); //use your file path here.
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
    protected void GetFileList()
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder filechklist = new StringBuilder();
        List<FileVersionInfo> objFileList = new List<FileVersionInfo>();
        if (Session["LISTID"] != null && ConfigurationManager.AppSettings["DumpPath"] != null)
        {
            string dirPath = ConfigurationManager.AppSettings["DumpPath"] + Session["LISTID"] + "\\";

            if (Directory.Exists(dirPath))
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                FileInfo[] EXEFiles = di.GetFiles("*.exe");

                if (EXEFiles.Length > 0)
                {
                    hdnSelExe.Value = EXEFiles[0].ToString();
                    if (EXEFiles.Length > 0)
                    {
                        sb.Append("<table class=\"table table-striped\"><thead><tr><th>Sl.no</th><th>FileName</th><th>Version</th><th>Operating System</th><th>FileSize</th><th>Date Published</th> </tr> </thead><tbody>");
                        for (int i = 0; i < EXEFiles.Length; i++)
                        {
                            FileVersionInfo fd = FileVersionInfo.GetVersionInfo(EXEFiles[i].FullName);
                            sb.Append("<tr>");
                            sb.Append("<td>" + (i + 1) + "</td>");
                            sb.Append("<td>" + EXEFiles[i].Name + "</td>");
                            sb.Append("<td>" + fd.ProductVersion + "</td>");
                            sb.Append("<td>" + "32bit" + "</td>");
                            sb.Append("<td>" + Math.Round((EXEFiles[i].Length / 1048576.00), 1) + "MB" + "</td>");
                            sb.Append("<td>" + EXEFiles[i].LastWriteTime + "</td>");
                            sb.Append("</tr>");

                            filechklist.Append(" <label class=\"radio-inline\">");
                            filechklist.Append("<input value = \"" + EXEFiles[i] + "\" type=\"radio\" checked=\"true\" name=\"versionSelection\">" + EXEFiles[i]);
                            filechklist.Append("</label>");

                        }
                        sb.Append("</tbody></table>");

                        if (EXEFiles.Length > 0)
                        {
                            string chklistHtml = "<div class=\"radioBoxDwnloadWrapper\">" + filechklist.ToString();
                            chklistHtml += "<div class=\"dwnBtnWrapper\"><a  class=\"hotbtn btn\"  onclick=\"OpenDownload();\">Download</a></div></div>";
                            downloadattch.InnerHtml = chklistHtml;
                        }

                        FileDetails.InnerHtml = sb.ToString();

                    }
                }
            }
        }
    }
    private void Download_ZIP(string[] files)
    {
        ZipFile zip = new ZipFile();
        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        zip.AddDirectoryByName(HttpContext.Current.Session["APPID"].ToString());
        foreach (string file in files)
        {
            zip.AddFile(file, HttpContext.Current.Session["APPID"].ToString());
        }
        Response.Clear();
        Response.BufferOutput = false;
        string zipName = String.Format("App_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        Response.ContentType = "application/zip";
        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
        zip.Save(Response.OutputStream);
        Response.Flush();
    }



    #region WEB API

    public string ConsumePostApi(string apiData, string apiLabel, string apiUrl)
    {
        string[] splitLabels = apiLabel.Split('/');
        string[] splitDatas = apiData.Split('/');
        string result = string.Empty;
        try
        {
            string baseURL = string.Empty;
            try
            {
                baseURL = ConfigurationManager.AppSettings["CloudHomeAPI"].ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            string json = "";
            if (splitLabels.Length == splitDatas.Length)
            {
                json += "{";
                for (int i = 0; i < splitLabels.Length; i++)
                {
                    json += "\"" + splitLabels[i] + "\":\"" + splitDatas[i] + "\"";
                    if (i < (splitLabels.Length) - 1)
                    {
                        json += ",";
                    }
                }
                json += "}";
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL + apiUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();


            if (((int)Convert.ToInt32(httpResponse.StatusCode) >= 200) && ((int)Convert.ToInt32(httpResponse.StatusCode) <= 299))
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd().ToString();
                }
            }
            else
            {
                Exception ex = new Exception();
                throw ex;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }
    #endregion



}




public class GenResult
{
    public string result { get; set; }
    public string code { get; set; }
}
