<%@ WebHandler Language="C#" Class="HeaderFileUpload" %>

using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Linq;
using Ionic.Zip;

public class HeaderFileUpload : IHttpHandler, IRequiresSessionState
{
    Util.Util util = new Util.Util();
    int attachmentSizeMB = 1;
    long lMaxFileSize = 1000000;
    string scriptsPath = string.Empty;
    string scriptsUrlPath = string.Empty;
    string sid = string.Empty;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request.Files.Count > 0)
        {
            HttpPostedFile file = context.Request.Files[0];
            if (Constants.disallowedMimeTypes.Contains(file.ContentType.ToLower()))
            {
                context.Response.Write("error:Invalid File Content.");
                return;
            }
            string fname = file.FileName;
            string act = string.Empty;
            string transId = string.Empty;
            string delFile = string.Empty;
            string AxtstAFSDB = "false";
            string FileExt = string.Empty;
            string ProtectFile = string.Empty;
            string fileProtectPwd = string.Empty;
            if (context.Request.QueryString["act"] != null)
                act = context.Request.QueryString["act"];

            if (context.Request.QueryString["transid"] != null)
                transId = context.Request.QueryString["transid"];

            if (context.Request.QueryString["delFile"] != null)
                delFile = context.Request.QueryString["delFile"];

            if (context.Request.QueryString["AxtstAFSDB"] != null)
                AxtstAFSDB = context.Request.QueryString["AxtstAFSDB"];

            if (context.Request.QueryString["fileExt"] != null)
                FileExt = context.Request.QueryString["fileExt"];

            if (context.Request.QueryString["protectFile"] != null)
                ProtectFile = context.Request.QueryString["protectFile"];

            if (context.Session["axp_uploadfileprotectedpwd"] != null)
                fileProtectPwd = context.Session["axp_uploadfileprotectedpwd"].ToString();

            if (context.Session["nsessionid"] != null)
                sid = context.Session["nsessionid"].ToString();
            else
            {
                context.Response.Write("error:sessionexpired");
                return;
            }
            scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            scriptsUrlPath = HttpContext.Current.Application["ScriptsurlPath"].ToString();
            //to get maximum attachment size from Config app
            if (context.Session["AxAttachmentSize"] != null)
                attachmentSizeMB = Convert.ToInt32(context.Session["AxAttachmentSize"]);
            lMaxFileSize = attachmentSizeMB * 1024 * 1024; //convert MB to Bytes

            string attachDirPath = "";
            bool attachDir = false;
            if (HttpContext.Current.Session["AxAttachFilePath"] != null)
                attachDirPath = HttpContext.Current.Session["AxAttachFilePath"].ToString();

            if (HttpContext.Current.Session["AxpAttachmentPathGbl"] != null)
                attachDirPath = HttpContext.Current.Session["AxpAttachmentPathGbl"].ToString();

            if (attachDirPath != string.Empty)
                attachDir = true;

            try
            {
                if (attachDir)
                {
                    DirectoryInfo di = new DirectoryInfo(scriptsPath + "Axpert\\" + sid + "\\tstHFile-" + transId);
                    //' Determine whether the directory exists.
                    if (di.Exists)
                    {

                    }
                    else
                    {
                        // create the directory.
                        di.Create();
                    }
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(scriptsPath + "Axpert\\" + sid);
                    //' Determine whether the directory exists.
                    if (di.Exists)
                    {

                    }
                    else
                    {
                        // create the directory.
                        di.Create();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string sFileDir = String.Empty;
            if (attachDir)
                sFileDir = scriptsPath + "Axpert\\" + sid + "\\tstHFile-" + transId + "\\";
            else
                sFileDir = scriptsPath + "Axpert\\" + sid + "\\";
            if ((file != null) && (file.ContentLength > 0))
            {
                try
                {
                    if (util.IsFileTypeValid(file))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string json = js.Serialize(Constants.fileTypes);
                        string[] notallowfileTypes = Constants.notallowfileTypes;
                        string fileNameLower = file.FileName.ToLower();
                        string jsonVideos = "";
                        if (context.Session["TstAllowVideoAtta-" + transId] != null && context.Session["TstAllowVideoAtta-" + transId].ToString() != "")
                        {
                            string isstrVideoFile = context.Session["TstAllowVideoAtta-" + transId].ToString();
                            var videoExtList = isstrVideoFile.Split(',').Select(x => "." + x.Trim()).ToList();
                            jsonVideos = js.Serialize(videoExtList);
                        }
                        string jsonSpecificFiles = "";
                        if (context.Session["TstAllowSpecificAtta-" + transId] != null && context.Session["TstAllowSpecificAtta-" + transId].ToString() != "")
                        {
                            string isstrSpecificFile = context.Session["TstAllowSpecificAtta-" + transId].ToString();
                            var specificExtList = isstrSpecificFile.Split(',').Select(x => "." + x.Trim()).ToList();
                            jsonSpecificFiles = js.Serialize(specificExtList);
                        }
                        if (file.ContentLength <= lMaxFileSize)
                        {
                            if ((sFileDir + fname).Length > 260)//display warning message if file path exceeds 260 characters
                            {
                                context.Response.Write("error:Too many characters in the filename");
                                return;
                            }
                            if (FileExt != string.Empty && FileExt.Contains(file.FileName.Substring(fname.LastIndexOf(".") + 1).ToLower()) == false)
                            {
                                context.Response.Write("error:Selected file type not allowed in this form as per the setting.");
                                return;
                            }
                            else if (json.Contains(file.FileName.Substring(fname.LastIndexOf(".")).ToLower()) == false && ((jsonVideos == "" || jsonVideos.Contains(file.FileName.Substring(fname.LastIndexOf(".")).ToLower()) == false) && (jsonSpecificFiles == "" || jsonSpecificFiles.Contains(file.FileName.Substring(fname.LastIndexOf(".")).ToLower()) == false)))
                            {
                                context.Response.Write("error:Invalid File Extension");
                                return;
                            }
                            else if (notallowfileTypes.Any(ext => fileNameLower.Contains(ext)))
                            {
                                context.Response.Write("error:Invalid File Extension.");
                                return;
                            }
                            else if (file.FileName.Contains(","))
                            {
                                context.Response.Write("error:Character ',' restricted in uploading File Names. Please rename and upload.");
                                return;
                            }
                            else
                            {
                                string orgZipFileName = fname;
                                if (fileProtectPwd != string.Empty && ProtectFile == "true")
                                    orgZipFileName = Path.GetFileNameWithoutExtension(fname) + ".zip";
                                bool _isFileExist = true;
                                if (delFile != string.Empty)
                                {
                                    string[] _delFile = delFile.Split('♦');
                                    foreach (string _sfile in _delFile)
                                    {
                                        if (_sfile == orgZipFileName)
                                        {
                                            _isFileExist = false;
                                            continue;
                                        }
                                    }
                                }
                                if (_isFileExist && File.Exists(sFileDir + orgZipFileName))
                                {
                                    context.Response.Write("error:File already exists, please rename and upload again!");
                                    return;
                                }

                                if (fileProtectPwd != string.Empty && ProtectFile == "true")
                                {
                                    if (attachDir)
                                        context.Session["tstHeaderAttach"] = "true";
                                    else
                                        context.Session["tstHeaderAttachDbFile"] = context.Session["tstHeaderAttachDbFile"] + "," + orgZipFileName;
                                    file.SaveAs(sFileDir + fname);
                                    string orgFileName = Path.GetFileNameWithoutExtension(fname) + ".zip";
                                    string zipFilePath = Path.Combine(sFileDir, orgFileName);
                                    using (ZipFile zip = new ZipFile())
                                    {
                                        zip.Password = fileProtectPwd;
                                        zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                                        zip.AddFile(sFileDir + "\\" + fname, "");
                                        zip.Save(zipFilePath);
                                    }
                                    File.Delete(sFileDir + "\\" + fname);
                                    context.Response.Write("success:Uploaded Successfully~fileName:" + orgFileName);
                                    if (AxtstAFSDB == "true" && attachDir)
                                    {
                                        try
                                        {
                                            string _scriptPath = scriptsPath + "Axpert\\" + sid + "\\";
                                            file.SaveAs(_scriptPath + fname);
                                            string axpertzipFilePath = Path.Combine(_scriptPath, orgFileName);
                                            using (ZipFile zip = new ZipFile())
                                            {
                                                zip.Password = fileProtectPwd;
                                                zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                                                zip.AddFile(_scriptPath + "\\" + fname, "");
                                                zip.Save(axpertzipFilePath);
                                            }
                                            File.Delete(_scriptPath + "\\" + fname);
                                        }
                                        catch (Exception ex) { }
                                    }
                                }
                                else
                                {
                                    if (attachDir)
                                        context.Session["tstHeaderAttach"] = "true";
                                    else
                                        context.Session["tstHeaderAttachDbFile"] = context.Session["tstHeaderAttachDbFile"] + "," + fname;
                                    //Save File on disk
                                    file.SaveAs(sFileDir + fname);
                                    context.Response.Write("success:Uploaded Successfully");
                                    if (AxtstAFSDB == "true" && attachDir)
                                    {
                                        try
                                        {
                                            string _scriptPath = scriptsPath + "Axpert\\" + sid + "\\";
                                            file.SaveAs(_scriptPath + fname);
                                        }
                                        catch (Exception ex) { }
                                    }
                                }
                            }
                        }
                        else
                        {
                            context.Response.Write("error:File could not be uploaded. Filesize is more than " + lMaxFileSize + " MB");
                            return;
                        }
                    }
                }
                catch (Exception ex)//in case of an error
                {
                    context.Response.Write("error:An Error Occured. Please Try Again!");
                }
            }
            else
            {
                context.Response.Write("error:File could not be uploaded. Filesize is 0 KB");
            }
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
