using PdfSharp.Pdf.Content.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_axlangupload : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    public string direction = "ltr";
    public string langType = "en";
    public string upFileType = "";
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
        if (Session["project"] == null)
        {
            SessExpires();
            return;
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["fpath"] != null)
            {
                hdnFilePath.Value = Request.QueryString["fpath"].ToString();
            }
            if (Request.QueryString["upfiletype"] != null)
            {
                hdnUpFileType.Value = Request.QueryString["upfiletype"].ToString();
                upFileType = hdnUpFileType.Value + ".xlsx";
            }
        }
    }

    protected void cmdSend_Click(object sender, EventArgs e)
    {
        HttpFileCollection httpAttFiles = Request.Files;
        for (int i = 0; i < httpAttFiles.Count; i++)
        {
            HttpPostedFile httpAttFile = httpAttFiles[i];
            if ((httpAttFile != null) && (httpAttFile.ContentLength > 0))
            {
                string axLPath = hdnFilePath.Value;
                if (axLPath != string.Empty)
                    axLPath += "\\upload\\";
                axLPath = axLPath.Replace("\\", "\\\\");
                try
                {
                    hdnFilePath.Value = axLPath;
                    DirectoryInfo di = new DirectoryInfo(axLPath);
                    if (!di.Exists)
                        di.Create();
                }
                catch (Exception ex)
                {
                    fileuploadsts.Text = lblAnError.Text;
                    continue;
                }

                string thisFileName = Path.GetFileName(httpAttFile.FileName);
                string Ext = thisFileName.Substring(thisFileName.LastIndexOf("."));

                try
                {
                    if (!util.IsFileTypeValid(httpAttFile))
                    {
                        fileuploadsts.Text = "[Invalid File.]";
                        fileuploadsts.ForeColor = System.Drawing.Color.Red;
                        cmdSend.Enabled = false;
                        break;
                    }
                    else if (Constants.fileTypes.Contains(httpAttFile.FileName.Substring(thisFileName.LastIndexOf(".")).ToLower()) == false)
                    {
                        fileuploadsts.Text = "[Invalid File Extension.]";
                        fileuploadsts.ForeColor = System.Drawing.Color.Red;
                        cmdSend.Enabled = false;
                        break;
                    }
                    else if ((hdnUpFileType.Value + ".xlsx").ToLower() != thisFileName.ToString().ToLower())
                    {
                        fileuploadsts.Text = "[Invalid File.]";
                        fileuploadsts.ForeColor = System.Drawing.Color.Red;
                        cmdSend.Enabled = false;
                        break;
                    }
                    else
                    {
                        //Save File on disk
                        httpAttFile.SaveAs(axLPath + thisFileName);

                        fileuploadsts.Text = "[" + lblFileUp.Text + "]";
                        fileuploadsts.ForeColor = System.Drawing.Color.Green;
                        upsts.Value = "Uploaded Successfully";

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "DoClientFunction", "DoClientFunction()", true);
                    }
                }
                catch (Exception ex)//in case of an error
                {
                    fileuploadsts.Text = lblAnError.Text;
                    upsts.Value = fileuploadsts.Text;
                }
            }
        }
    }

    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }
}