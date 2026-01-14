using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Packaging;
using System.Configuration;
using System.Security.AccessControl;
using Ionic.Zip;

public partial class aspx_OpenFiles : System.Web.UI.Page
{
    Util.Util objUtil = new Util.Util();
    string EnableOldTheme = string.Empty;
    string strfilename = string.Empty;
    string sid = string.Empty;
    string strFiles = string.Empty;
    string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
    string compressCheck = string.Empty;
    string dir = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        objUtil.IsValidSession();

        if (Session["project"] == null)
        {
            SessionExpired();
        }

        if (HttpContext.Current.Session["AxEnableOldTheme"] != null)
            EnableOldTheme = HttpContext.Current.Session["AxEnableOldTheme"].ToString();

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Session["nsessionid"].ToString()))
            {
                if (Request.QueryString["alPath"] != null)
                {
                    string alPath = Request.QueryString["alPath"].ToString();
                    DownloadAxLang(alPath);
                }
                else
                {
                    sid = Session["nsessionid"].ToString();

                    if (Request.QueryString["fpath"] != null)
                    {
                        strFiles = Request.QueryString["fpath"].ToString();

                    }
                    if (Request.QueryString["docid"].ToString() != null)
                    {
                        ViewState["Docid"] = Request.QueryString["docid"].ToString();
                    }
                    if (Request.QueryString["compress"].ToString() != null)
                    {
                        compressCheck = Request.QueryString["compress"].ToString();
                    }
                    if (Request.QueryString["path"].ToString() != null)
                    {
                        ViewState["savePath"] = Request.QueryString["path"].ToString();
                    }
                    if (Request.QueryString["dirName"].ToString() != null || Request.QueryString["dirName"].ToString() != "undefined")
                    {
                        dir = Request.QueryString["dirName"].ToString();
                        dir = dir.Replace(";bkslh", "\\");
                    }
                    ViewState["dir"] = dir;
                    if (compressCheck.ToString().ToLower().Equals("true"))
                        btnDownloadZip.Visible = true;
                    else
                        btnDownloadZip.Visible = false;

                    if (ViewState["savePath"].ToString() == "")
                        btnSaveFiles.Visible = false;
                    else
                        btnSaveFiles.Visible = true;

                    DataTable dtFiles = new DataTable();

                    string[] sSentFiles = strFiles.Split('|');

                    DirectoryInfo di = new DirectoryInfo(ScriptsPath + "\\Axpert\\" + sid + "\\" + ViewState["dir"].ToString() + "\\");

                    if (di.Exists)
                    {
                        FileInfo[] files = di.GetFiles();

                        foreach (System.IO.FileInfo fi in files)
                        {
                            for (int i = 0; i < sSentFiles.Length; i++)
                            {
                                if (fi.Name == sSentFiles[i].ToString())
                                {
                                    BindDataGrid(sSentFiles[i], dtFiles);
                                    break;
                                }
                            }
                        }
                    }

                    gvFiles.DataSource = dtFiles;
                    gvFiles.DataBind();
                }
            }
            else
            {
                SessionExpired();
            }

        }
    }

    /// <summary>
    /// function for handling session timeout.
    /// </summary>
    public void SessionExpired()
    {
        string url = objUtil.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

    private void BindDataGrid(string sFilename, DataTable dtFiles)
    {
        if (!dtFiles.Columns.Contains("FileName"))
            dtFiles.Columns.Add("FileName");
        if (!dtFiles.Columns.Contains("link"))
            dtFiles.Columns.Add("link");

        DataRow dr = dtFiles.NewRow();
        dr["Filename"] = sFilename;
        dr["link"] = sFilename;
        dtFiles.Rows.Add(dr);

    }

    protected void gvFiles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
            string link = e.CommandArgument.ToString();
            OpenFiles(link);
        }
    }

    private void OpenFiles(string linkname)
    {

        string sOutputPath = ScriptsPath + "Axpert\\" + Session["nsessionid"].ToString() + "\\" + ViewState["dir"].ToString() + linkname;
        FileInfo sFile = new FileInfo(sOutputPath);
        if (sFile.Exists)
        {
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + sFile.Name);
            Response.AddHeader("Content-Length", sFile.Length.ToString());
            Response.ContentType = "application/x-download";
            Response.WriteFile(sFile.FullName);
            Response.End();
        }
        else
            Response.Write("This file does not exist.");

    }
    protected void btnDownloadZip_Click(object sender, EventArgs e)
    {
        SaveAsZip();

        string filePath;
        if (ViewState["savePath"].ToString() != string.Empty)
            filePath = ViewState["savePath"].ToString() + ViewState["Docid"].ToString() + ".zip";
        else
            filePath = ScriptsPath + "Axpert\\" + Session["nsessionid"].ToString() + "\\" + ViewState["dir"].ToString() + "\\" + ViewState["Docid"].ToString() + ".zip";

        System.IO.FileInfo sfile = new System.IO.FileInfo(filePath);
        if (sfile.Exists)
        {
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + sfile.Name);
            Response.AddHeader("Content-Length", sfile.Length.ToString());
            Response.ContentType = "application/x-zip-compressed";
            Response.WriteFile(sfile.FullName);
            Response.End();

        }
        else
            Response.Write("This file does not exist.");

    }

    private void SaveAsZip()
    {
        try
        {
            string sid = Session["nsessionid"].ToString();
            string zipPath;

            string folderPath = ScriptsPath + "Axpert\\" + sid + "\\" + ViewState["dir"].ToString() + "\\";
            if (ViewState["savePath"].ToString() != string.Empty)
                zipPath = ViewState["savePath"].ToString() + ViewState["Docid"].ToString() + ".zip";
            else
                zipPath = folderPath + ViewState["Docid"].ToString() + ".zip";

            //create a new one 
            Package zip = ZipPackage.Open(zipPath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);

            //Add all files in the folder:
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderPath);
            System.IO.FileInfo[] diFileinfo = di.GetFiles();

            foreach (System.IO.FileInfo drfile in diFileinfo)
            {
                if (drfile.ToString().IndexOf(".pdf") > -1)
                    AddToArchive(zip, ScriptsPath + "Axpert\\" + sid + "\\" + ViewState["dir"].ToString() + "\\" + drfile.ToString());

            }
            zip.Close();

        }
        catch (Exception) { }

    }

    private void AddToArchive(Package zip, string fileToAdd)
    {
        try
        {
            //Replae spaces with an underscore (_) 
            string uriFileName = fileToAdd.Replace(" ", "_");

            //A Uri always starts with a forward slash "/" 
            string zipUri = String.Concat("/", System.IO.Path.GetFileName(uriFileName));

            Uri partUri = new Uri(zipUri, UriKind.Relative);
            string contentType = System.Net.Mime.MediaTypeNames.Application.Zip;

            //The PackagePart contains the information: 
            // Where to extract the file when it's extracted (partUri) 
            // The type of content stream (MIME type):  (contentType) 
            //The type of compression:  (CompressionOption.Normal)   
            PackagePart pkgPart = zip.CreatePart(partUri, contentType, CompressionOption.Normal);

            //Read all of the bytes from the file to add to the zip file 
            Byte[] bites = File.ReadAllBytes(fileToAdd);

            //Compress and write the bytes to the zip file 
            pkgPart.GetStream().Write(bites, 0, bites.Length);
        }
        catch (Exception) { }
    }
    protected void btnSaveFiles_Click(object sender, EventArgs e)
    {
        try
        {
            string currentPath = ScriptsPath + "Axpert\\" + Session["nsessionid"].ToString() + "\\" + ViewState["dir"].ToString() + "\\";
            string filePath = ViewState["savePath"].ToString() + ViewState["Docid"].ToString() + "\\";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(currentPath);
            System.IO.FileInfo[] diFileinfo = di.GetFiles();

            foreach (System.IO.FileInfo drfile in diFileinfo)
            {
                if (drfile.ToString() != ViewState["Docid"].ToString() + ".zip")
                {
                    File.Copy(currentPath + drfile.ToString(), filePath + drfile.ToString(), true);
                }
            }
            Response.Write("<script>showAlertDialog('success',4013,'client');</script>");
            Response.Write("<script>window.close();</script>");
            Response.End();
        }

        catch (Exception) { }
    }

    public void DownloadAxLang(string fpath)
    {
        try
        {
            if (!string.IsNullOrEmpty(fpath))
            {
                fpath = Path.Combine(fpath, "download");
                DirectoryInfo di = new DirectoryInfo(fpath);

                if (di.Exists)
                {
                    FileInfo[] files = di.GetFiles();

                    if (files.Length > 0)
                    {
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                            zip.AddDirectoryByName("AxLanguages");

                            foreach (FileInfo sFile in files)
                            {
                                switch (sFile.Name)
                                {
                                    case "tstruct.xlsx":
                                    case "iviews.xlsx":
                                    case "axpages.xlsx":
                                    case "errorcodes.xlsx":
                                    case "cards.xlsx":
                                        zip.AddFile(sFile.FullName, "AxLanguages");
                                        break;
                                }
                            }
                            Response.Clear();
                            Response.ContentType = "application/zip";
                            string zipName = String.Format("AxLanguages_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                            Response.AddHeader("content-disposition", "attachment; filename=" + zipName);

                            using (var zipStream = new MemoryStream())
                            {
                                zip.Save(zipStream);
                                zipStream.Position = 0;
                                zipStream.CopyTo(Response.OutputStream);
                            }

                            Response.Flush();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            Response.Close();
        }
    }
}
