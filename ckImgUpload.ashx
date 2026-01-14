<%@ WebHandler Language="C#" Class="ckImgUpload" %>

using System;
using System.Web;
using System.IO;
using System.Web.SessionState;

public class ckImgUpload : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        HttpPostedFile uploads = context.Request.Files["upload"];
        string CKEditorFuncNum = context.Request["CKEditorFuncNum"];
        string file = System.IO.Path.GetFileName(uploads.FileName);
        if (file.IndexOf(".jpg") > -1 || file.IndexOf(".jpeg") > -1 || file.IndexOf(".png") > -1 || file.IndexOf(".gif") > -1 || file.IndexOf(".bmp") > -1)
        {
            long lMaxFileSize = 1000000;
            if (uploads.ContentLength <= lMaxFileSize)
            {
                string fldName = context.Request.QueryString["fldName"];
                string transId = context.Request.QueryString["transId"];
                fldName = fldName.Substring(0, fldName.LastIndexOf("F") - 3);
                string lclPatch = context.Server.MapPath(".");
                string fileExe = DateTime.Now.ToString("ddMMyyymmssfffff");
                string fullPath = lclPatch + "\\filemanager\\" + transId + "\\" + fldName + "\\" + fileExe;
                string printUlr = string.Empty;
                try
                {
                    DirectoryInfo di = new DirectoryInfo(fullPath);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                uploads.SaveAs(fullPath + "\\" + file);
                string appUrl = HttpContext.Current.Request.Url.ToString();
                string[] appUrls = appUrl.Split(new[] { "ckImgUpload.ashx" }, StringSplitOptions.None);
                string url = appUrls[0] + "filemanager/" + transId + "/" + fldName + "/" + fileExe + "/" + file;
                context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
                context.Response.End();
            }
            else
            {
                context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", alert('File could not be uploaded. Filesize is more than 1MB.'));</script>");
                context.Response.End();
            }
        }
        else
        {
            context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", alert('Invalid file extension.'));</script>");
            context.Response.End();
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