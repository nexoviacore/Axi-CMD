<%@ WebHandler Language="C#" Class="ProjectImageUploadHandler" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Web.SessionState;
using System.Linq;
public class ProjectImageUploadHandler : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        if (context.Request.Files.Count > 0)
        {
            HttpPostedFile file = context.Request.Files[0];
            string fname = file.FileName;

            string projName = string.Empty;

            if (context.Request.QueryString["proj"] != null)
            {
                projName = context.Request.QueryString["proj"];
            }

            string uploadType = string.Empty;

            if (context.Request.QueryString["type"] != null)
            {
                uploadType = context.Request.QueryString["type"];
            }

            string dirPath = HttpContext.Current.Server.MapPath("~/" + projName + "");

            try
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                if (!di.Exists)
                {
                    di.Create();
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            long lMaxFileSize = 1000000;
            string[] fileTypes = { ".bmp", ".jpg", ".jpeg", ".png", ".gif" };
            string extension = Path.GetExtension(fname);
            try
            {
                if (file.ContentLength <= lMaxFileSize && fileTypes.Contains(extension.ToLower()))
                {
                    //Save File on disk
                    string sFileName = "axAppLogo";

                    switch (uploadType) {
                        case "logo":
                            sFileName = "axAppLogo";
                            break;
                        case "webbg":
                            sFileName = "axAppWebBG";
                            break;
                        case "mobbg":
                            sFileName = "axAppMobBG";
                            break;
                    }

                    DirectoryInfo di = new DirectoryInfo(dirPath);

                    FileInfo[] dfile = di.GetFiles(sFileName + ".*");
                    if (dfile.Count() > 0) {
                        foreach (FileInfo filee in dfile) {
                            File.Delete(dirPath + "\\" + filee.Name);
                        }
                    }

                    file.SaveAs(dirPath + "\\" + sFileName + extension);
                    context.Response.Write("Image Uploaded successfully");
                }
                else
                {
                    context.Response.Write("Image could not be uploaded. Filesize is more than 1MB or unsupported file is uploaded.");
                }
            }
            catch (Exception ex)//in case of an error
            {
                context.Response.Write("An error occured. Please Try Again!");
            }
            // }
        }
        else {
            string projName = string.Empty;

            if (context.Request.QueryString["proj"] != null)
            {
                projName = context.Request.QueryString["proj"];
            }

            string dirPath = HttpContext.Current.Server.MapPath("~/" + projName + "");

            try
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                if (!di.Exists)
                {
                    // create the directory.
                    di.Create();
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }


            try
            {
                string handlerType = string.Empty;

                if (context.Request.QueryString["handlertype"] != null)
                {
                    handlerType = context.Request.QueryString["handlertype"];
                }

                if (handlerType == "delete")
                {
                    string uploadType = string.Empty;

                    if (context.Request.QueryString["type"] != null)
                    {
                        uploadType = context.Request.QueryString["type"];
                    }

                    string sFileName = "axAppLogo";

                    switch (uploadType) {
                        case "logo":
                            sFileName = "axAppLogo";
                            break;
                        case "webbg":
                            sFileName = "axAppWebBG";
                            break;
                        case "mobbg":
                            sFileName = "axAppMobBG";
                            break;
                    }

                    DirectoryInfo di = new DirectoryInfo(dirPath);

                    FileInfo[] dfile = di.GetFiles(sFileName + ".*");
                    if (dfile.Count() > 0) {
                        foreach (FileInfo filee in dfile) {
                            File.Delete(dirPath + "\\" + filee.Name);
                        }
                    }

                    //file.SaveAs(dirPath + "\\" + sFileName + extension);
                    context.Response.Write("Image Deleted successfully");
                }
                else {
                    context.Response.Write("Please select logo to upload.");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("An error occured. Please Try Again!");
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
