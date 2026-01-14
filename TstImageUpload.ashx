<%@ WebHandler Language="C#" Class="TstImageUpload" %>

using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Linq;

public class TstImageUpload : IHttpHandler, IRequiresSessionState
{
    public string fieldName = string.Empty;
    public string isAxpImagePath = "false";
    public string tsttrid = string.Empty;
    long lMaxFileSize = 1000000;
    int attachmentSizeMB = 1;
    string scriptsPath = string.Empty;
    string scriptsUrlPath = string.Empty;
    string sid = string.Empty;
    Util.Util util = new Util.Util();
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request.Files.Count > 0)
        {
            HttpPostedFile filMyFile = context.Request.Files[0];
            if (Constants.disallowedMimeTypes.Contains(filMyFile.ContentType.ToLower()))
            {
                context.Response.Write("error:Image could not be uploaded. Invalid File Content.");
                return;
            }
            if (context.Request.QueryString["fldname"] != null)
            {
                fieldName = context.Request.QueryString["fldname"];
            }
            if (context.Request.QueryString["isAxpImagePath"] != null)
            {
                isAxpImagePath = context.Request.QueryString["isAxpImagePath"];
            }
            if (context.Request.QueryString["transid"] != null)
            {
                tsttrid = context.Request.QueryString["transid"];
            }

            scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            scriptsUrlPath = HttpContext.Current.Application["ScriptsurlPath"].ToString();
            if (context.Session["nsessionid"] != null)
                sid = context.Session["nsessionid"].ToString();
            else
            {
                context.Response.Write("error:sessionexpired");
                return;
            }
            if (context.Session["AxAttachmentSize"] != null)
                attachmentSizeMB = Convert.ToInt32(context.Session["AxAttachmentSize"]);
            lMaxFileSize = attachmentSizeMB * 1024 * 1024; //convert MB to Bytes

            if (filMyFile.ContentLength > 0)
            {
                int fileLength = 0;
                fileLength = filMyFile.ContentLength;
                string imageFileName = "";
                string filePath = "";
                if (fileLength > 0 && fileLength < lMaxFileSize)
                {
                    Byte[] fileData = new Byte[fileLength];
                    filMyFile.InputStream.Read(fileData, 0, fileLength);
                    imageFileName = Path.GetFileName(filMyFile.FileName);
                    int fIdx = fieldName.LastIndexOf("F");
                    string tmpFldName = "";
                    if (fIdx != -1)
                    {
                        tmpFldName = fieldName.Substring(0, fIdx - 3);
                    }
                    DirectoryInfo di = new DirectoryInfo(scriptsPath + "axpert\\" + sid + "\\" + tmpFldName);


                    //The folder path for uploading the image. 
                    if (tmpFldName.StartsWith(Constants.IMGPrefix))
                    {
                        //filepathna.Text = scriptsUrlPath + "axpert/" + sid + "/" + tmpFldName + "/" + System.Uri.EscapeDataString(imageFileName);
                        filePath = scriptsPath + "axpert\\" + sid + "\\" + tmpFldName + "\\" + imageFileName;
                    }
                    //The folder path for uploading the image - AxpimagePath. 
                    else if (isAxpImagePath == "true")
                    {
                        //filepathna.Text = scriptsUrlPath + "axpert/" + sid + "/" + tmpFldName + "/" + System.Uri.EscapeDataString(imageFileName);
                        filePath = scriptsPath + "axpert\\" + sid + "\\" + tmpFldName + "\\" + imageFileName;
                    }

                    else
                    {
                        di = new DirectoryInfo(scriptsPath + "axpert\\" + sid);
                        //filepathna.Text = scriptsUrlPath + "axpert/" + sid + "/" + System.Uri.EscapeDataString(imageFileName);
                        filePath = scriptsPath + "axpert\\" + sid + "\\" + imageFileName;
                    }
                    if (!di.Exists)
                        di.Create();
                    //fname.Text = imageFileName;
                    //check for file content
                    string fileType = filMyFile.ContentType;
                    if (util.IsFileTypeValid(filMyFile))
                    {
                        string[] imageTypes = new string[] { "image/gif", "image/pjpeg", "image/jpg", "image/pjpg", "image/jpeg", "image/png", "image/bmp", "image/tiff", "image/tif" };
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string json = js.Serialize(imageTypes);
                        if (json.Contains(filMyFile.ContentType))
                        {
                            WriteToFile(filePath, ref fileData);
                            try
                            {
                                if (context.Session["AxtstAFSDB"] != null && context.Session["AxtstAFSDB"].ToString() == "true")
                                {
                                    di = new DirectoryInfo(scriptsPath + "axpert\\" + sid);
                                    filePath = scriptsPath + "axpert\\" + sid + "\\" + imageFileName;
                                    if (!di.Exists)
                                        di.Create();
                                    WriteToFile(filePath, ref fileData);
                                }
                            }
                            catch (Exception ex) { }
                            context.Response.Write("success:File uploaded successfully!");
                            if (tsttrid != string.Empty && tsttrid == "a__na")
                            {
                                try
                                {
                                    string thisProj = string.Empty;
                                    if (context.Session["Project"] != null)
                                    {
                                        thisProj = context.Session["Project"].ToString();
                                        string dirPath = context.Server.MapPath("~/images/news/" + thisProj);
                                        if (!Directory.Exists(dirPath))
                                        {
                                            Directory.CreateDirectory(dirPath);
                                        }
                                        string savePath = Path.Combine(dirPath, imageFileName);
                                        WriteToFile(savePath, ref fileData);
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }
                        }
                        else
                        {
                            context.Response.Write("error:Image could not be uploaded. Invalid FileType");
                        }
                    }
                    else
                    {
                        context.Response.Write("error:Image could not be uploaded. Invalid FileType");
                    }
                }
                else
                {
                    context.Response.Write("error:Image could not be uploaded. Filesize is more than " + attachmentSizeMB + " MB");
                }
            }
        }
        else
            context.Response.Write("error");
    }

    public void WriteToFile(string strPath, ref byte[] Buffer)
    {
        // Create a file
        FileStream newFile = new FileStream(strPath, FileMode.Create);
        // Write data to the file
        newFile.Write(Buffer, 0, Buffer.Length);
        //Close file
        newFile.Close();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
