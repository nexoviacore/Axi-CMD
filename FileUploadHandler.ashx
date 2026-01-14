<%@ WebHandler Language="C#" Class="FileUploadHandler" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Web.SessionState;
public class FileUploadHandler : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.Files.Count > 0)
        {
            HttpFileCollection files = context.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                string fname = file.FileName;
                string sid = context.Session["nsessionid"].ToString();
                string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                context.Response.ContentType = "text/plain";
                try
                {
                    // Create directory if not exists
                    string directoryPath = Path.Combine(ScriptsPath, "Axpert", sid);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("Directory creation error: " + ex.Message);
                    return;
                }

                // File handling
                try
                {
                    string fileExt = Path.GetExtension(fname);
                    string baseFileName = Path.GetFileNameWithoutExtension(fname);
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string newFileName = timestamp + "-" + baseFileName + fileExt;
                    string savePath = Path.Combine(ScriptsPath, "Axpert", sid, newFileName);

                    // Increase File Size Limit (Up to 10MB)
                    long maxFileSize = 10 * 1024 * 1024; // 10MB

                    if (file.ContentLength > maxFileSize)
                    {
                        context.Response.Write("File size exceeds 10MB.");
                        return;
                    }

                    // Save File Efficiently Using Buffered Streams
                    using (FileStream fs = new FileStream(savePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[81920]; // 80KB buffer
                        int bytesRead;
                        using (Stream inputStream = file.InputStream)
                        {
                            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fs.Write(buffer, 0, bytesRead);
                            }
                        }
                    }

                    context.Response.Write("File Uploaded successfully &&" + newFileName);
                }
                catch (Exception ex)
                {
                    context.Response.Write("Error: " + ex.Message);
                }
            }
        }
        else
        {
            context.Response.Write("An Error Occured. Please Try Again!");
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
