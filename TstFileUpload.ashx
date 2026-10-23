<%@ WebHandler Language="C#" Class="TstFileUpload" %>

using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Linq;
using Ionic.Zip;

public class TstFileUpload : IHttpHandler, IRequiresSessionState
{
    Util.Util util = new Util.Util();
    int attachmentSizeMB = 1;
    long lMaxFileSize = 1000000;
    string concatFileName = string.Empty;
    bool rncName = false;
    bool isExpExtend = true;

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
            string filePath = string.Empty;
            string fldNameId = string.Empty;
            string fldName = string.Empty;
            string dcNo = string.Empty;
            string FileExt = string.Empty;
            string FuTransid = string.Empty;
            string ProtectFile = string.Empty;
            string fileProtectPwd = string.Empty;
            if (context.Request.QueryString["thisFld"] != null)
                fldNameId = context.Request.QueryString["thisFld"];

            if (context.Request.QueryString["attFldName"] != null)
                fldName = context.Request.QueryString["attFldName"];

            if (context.Request.QueryString["filePath"] != null)
                filePath = context.Request.QueryString["filePath"];
            if (context.Request.QueryString["dcNo"] != null)
                dcNo = context.Request.QueryString["dcNo"];
            if (context.Request.QueryString["fileExt"] != null)
                FileExt = context.Request.QueryString["fileExt"];
            if (context.Request.QueryString["futransid"] != null)
                FuTransid = context.Request.QueryString["futransid"];

            if (context.Request.QueryString["protectFile"] != null)
                ProtectFile = context.Request.QueryString["protectFile"];

            if (context.Session["axp_uploadfileprotectedpwd"] != null)
                fileProtectPwd = context.Session["axp_uploadfileprotectedpwd"].ToString();

            //to get maximum attachment size from Config app
            if (context.Session["AxAttachmentSize"] != null)
                attachmentSizeMB = Convert.ToInt32(context.Session["AxAttachmentSize"]);
            lMaxFileSize = attachmentSizeMB * 1024 * 1024; //convert MB to Bytes

            filePath = GetDestFilePath(filePath, context);


            string authenticationStatus = string.Empty;
            if (filePath != string.Empty && util.GetAuthentication(ref authenticationStatus))
            {
                if (filePath != string.Empty && !filePath.EndsWith("\\"))
                    filePath += "\\";
                filePath = filePath.Replace("\\", "\\\\");
                DirectoryInfo di = new DirectoryInfo(filePath);
                //' Determine whether the directory exists.
                if (!di.Exists)
                {
                    try
                    {
                        di.Create();
                    }
                    catch (Exception ex)
                    {
                        context.Response.Write("error:File server path is empty / invalid.");
                        return;
                    }
                }
            }

            if (filePath == string.Empty)
            {
                context.Response.Write("error:File server path is empty / invalid.");
            }
            else
            {
                HttpPostedFile httpAttFile = file;
                if ((httpAttFile != null) && (httpAttFile.ContentLength > 0))
                {
                    string thisFileName = Path.GetFileName(httpAttFile.FileName);
                    string Ext = thisFileName.Substring(thisFileName.LastIndexOf("."));
                    //hdnType.Value = Ext;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(Constants.fileTypes);

                    string[] notallowfileTypes = Constants.notallowfileTypes;
                    string jsonVideos = "";
                    if (context.Session["TstAllowVideoAtta-" + FuTransid] != null && context.Session["TstAllowVideoAtta-" + FuTransid].ToString() != "")
                    {
                        string isstrVideoFile = context.Session["TstAllowVideoAtta-" + FuTransid].ToString();
                        var videoExtList = isstrVideoFile.Split(',').Select(x => "." + x.Trim()).ToList();
                        jsonVideos = js.Serialize(videoExtList);
                    }
                    string jsonSpecificFiles = "";
                    if (context.Session["TstAllowSpecificAtta-" + FuTransid] != null && context.Session["TstAllowSpecificAtta-" + FuTransid].ToString() != "")
                    {
                        string isstrSpecificFile = context.Session["TstAllowSpecificAtta-" + FuTransid].ToString();
                        var specificExtList = isstrSpecificFile.Split(',').Select(x => "." + x.Trim()).ToList();
                        jsonSpecificFiles = js.Serialize(specificExtList);
                    }
                    try
                    {
                        string fileNameLower = thisFileName.ToLower();
                        if (!util.IsFileTypeValid(httpAttFile))
                        {
                            context.Response.Write("error:Invalid File.");
                            return;
                        }
                        else if (httpAttFile.ContentLength > lMaxFileSize)
                        {
                            context.Response.Write("error:File could not be uploaded. Filesize is more than " + lMaxFileSize + " MB");
                            return;
                        }
                        else if ((filePath + thisFileName).Length > 260)//display warning message if file path exceeds 260 characters
                        {
                            context.Response.Write("error:Too many characters in the filename");
                            return;
                        }
                        else if (FileExt != string.Empty && FileExt.Contains(httpAttFile.FileName.Substring(thisFileName.LastIndexOf(".") + 1).ToLower()) == false)
                        {
                            context.Response.Write("error:Selected file type not allowed in this form as per the setting.");
                            return;
                        }
                        else if (json.Contains(httpAttFile.FileName.Substring(thisFileName.LastIndexOf(".")).ToLower()) == false && ((jsonVideos == "" || jsonVideos.Contains(httpAttFile.FileName.Substring(thisFileName.LastIndexOf(".")).ToLower()) == false) && (jsonSpecificFiles == "" || jsonSpecificFiles.Contains(httpAttFile.FileName.Substring(thisFileName.LastIndexOf(".")).ToLower()) == false)))
                        {
                            context.Response.Write("error:Invalid File Extension.");
                            return;
                        }
                        else if (notallowfileTypes.Any(ext => fileNameLower.Contains(ext)))
                        {
                            context.Response.Write("error:Invalid File Extension.");
                            return;
                        }
                        else if (httpAttFile.FileName.Contains(","))
                        {
                            context.Response.Write("error:Character ',' restricted in uploading File Names. Please rename and upload.");
                            return;
                        }
                        else
                        {
                            if (fileProtectPwd != string.Empty && ProtectFile == "true")
                            {
                                string orgZipFileName = Path.GetFileNameWithoutExtension(concatFileName + thisFileName) + ".zip";
                                string fullPath = filePath + orgZipFileName;// concatFileName + thisFileName;
                                if (isExpExtend == true)
                                {
                                    if (File.Exists(fullPath))
                                    {
                                        context.Response.Write("error:File already exists, please rename and upload again!");
                                        return;
                                    }
                                }
                                ArrayList arListExist = new ArrayList();
                                string attFldName = fldName;
                                if (context.Session["AxpAttFileServer"] != null)
                                    arListExist = (ArrayList)context.Session["AxpAttFileServer"];
                                if (arListExist.IndexOf(attFldName + "~" + fullPath) == -1)
                                {
                                    arListExist.Add(attFldName + "~" + fullPath);
                                    context.Session["AxpAttFileServer"] = arListExist;
                                }
                                //Save File on disk
                                httpAttFile.SaveAs(filePath + concatFileName + thisFileName);

                                string zipFilePath = Path.Combine(filePath, orgZipFileName);
                                using (ZipFile zip = new ZipFile())
                                {
                                    zip.Password = fileProtectPwd;
                                    zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                                    zip.AddFile(filePath + concatFileName + thisFileName, "");
                                    zip.Save(zipFilePath);
                                }
                                File.Delete(filePath + concatFileName + thisFileName);
                                context.Response.Write("success:File uploaded successfully~fileName:" + orgZipFileName + "~filepath:" + filePath + orgZipFileName + "♠" + orgZipFileName);
                            }
                            else
                            {
                                string fullPath = filePath + concatFileName + thisFileName;
                                if (isExpExtend == true)
                                {
                                    if (File.Exists(fullPath))
                                    {
                                        context.Response.Write("error:File already exists, please rename and upload again!");
                                        return;
                                    }
                                }
                                ArrayList arListExist = new ArrayList();
                                string attFldName = fldName;
                                if (context.Session["AxpAttFileServer"] != null)
                                    arListExist = (ArrayList)context.Session["AxpAttFileServer"];
                                if (arListExist.IndexOf(attFldName + "~" + fullPath) == -1)
                                {
                                    arListExist.Add(attFldName + "~" + fullPath);
                                    context.Session["AxpAttFileServer"] = arListExist;
                                }
                                //Save File on disk
                                httpAttFile.SaveAs(filePath + concatFileName + thisFileName);
                                context.Response.Write("success:File uploaded successfully~filepath:" + filePath + concatFileName + thisFileName + "♠" + concatFileName + thisFileName);
                            }
                        }
                    }
                    catch (Exception ex)//in case of an error
                    {
                        context.Response.Write("success:An Error Occured. Please Try Again!");
                    }
                }
                else
                {
                    context.Response.Write("error:File could not be uploaded. Filesize is 0 KB");
                }
            }
        }
        else
            context.Response.Write("error");
    }

    public string GetGlobalAttachPath(HttpContext context)
    {
        bool isLocalFolder = false;
        bool isRemoteFolder = false;
        string imagePath = string.Empty;
        string imageServer = string.Empty;
        string grdAttPath = string.Empty;
        string errorMessage = string.Empty;
        string mapUsername = string.Empty;
        string mapPassword = string.Empty;

        if (context.Session["AxpImageServerGbl"] != null)
        {
            imageServer = context.Session["AxpImageServerGbl"].ToString();
            imageServer = imageServer.Replace(";bkslh", @"\");
        }
        if (context.Session["AxpImagePathGbl"] != null)
        {
            imagePath = context.Session["AxpImagePathGbl"].ToString();
            imagePath = imagePath.Replace(";bkslh", @"\");

            if (imagePath.IndexOf(":") > -1)
                isLocalFolder = true;
            else if (imagePath.IndexOf(@"\") > -1)
                isRemoteFolder = true;
        }

        if (imagePath != string.Empty)
        {
            if (isLocalFolder || isRemoteFolder)
                grdAttPath = imagePath;
            else
                grdAttPath = imageServer + @"\" + imagePath;
        }
        else if (imageServer != string.Empty)
        {
            grdAttPath = imageServer;
        }
        else //If the global variables AxpimageServer and AxpImagePath is not defined
        {
            if (context.Session["AxGridAttachPath"] != null)
            {
                grdAttPath = context.Session["AxGridAttachPath"].ToString();
            }
        }
        return grdAttPath;
    }

    public string GetDestFilePath(string filePath, HttpContext context)
    {
        string destFilePath = string.Empty;
        try
        {
            string sid = string.Empty;
            if (context.Session["nsessionid"] != null)
                sid = context.Session["nsessionid"].ToString();
            string AttPath = string.Empty;
            if (filePath != "")// AxpFilePath_ field value 
            {
                destFilePath = AttPath = filePath;
                if (AttPath.EndsWith("*"))
                {
                    destFilePath = AttPath.Substring(0, AttPath.LastIndexOf('\\'));
                    concatFileName = AttPath.Substring(AttPath.LastIndexOf("\\") + 1).Replace("*", "");
                    if (concatFileName == string.Empty)
                    {
                        isExpExtend = false;
                        rncName = true;
                        Random rand = new Random();
                        string rnd_key = rand.Next(100000, 999999).ToString();
                        concatFileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + rnd_key;
                    }
                }
            }
            else // Global var file server path 
            {
                isExpExtend = false;
                destFilePath = GetGlobalAttachPath(context);
                if (destFilePath == "" && context.Session["AxConfigFileUploadPath"] != null && context.Session["AxConfigFileUploadPath"].ToString() != "")
                    destFilePath = context.Session["AxConfigFileUploadPath"].ToString();
            }
        }
        catch (Exception ex)
        {
            destFilePath = string.Empty;
            throw ex;
        }
        return destFilePath;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
