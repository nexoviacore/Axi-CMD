using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Web.UI.HtmlControls;


public partial class fileupload : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    string succ = string.Empty;
    public string direction = "ltr";
    public string langType = "en";
    long lMaxFileSize = 1048576;//1MB default
    int attachmentSizeMB = 1;

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
       
        util.IsValidSession();
        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = util.toggleTheme();
        if (Request.UrlReferrer != null)
        {
            if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("tstruct.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("fileupload.aspx")))
            {
                Response.Redirect("../cusError/axcustomerror.aspx");
            }
        }

        if (Session["project"] == null || Convert.ToString(Session["project"]) == string.Empty)
        {
            SessExpires();
            return;
        }

        if (!IsPostBack)
        {
            if (Session["language"].ToString() == "ARABIC")
            {
                direction = "rtl";
            }
            if (Request.QueryString["act"] != null)
            {
                if (!util.IsChar(Request.QueryString["act"].ToString()))
                    Response.Redirect(Constants.PARAMERR);

                hdnAction.Value = Request.QueryString["act"].ToString();
            }
        }
        //ClientScript.RegisterOnSubmitStatement(GetType(), "cli", "DoClientFunction()");

        //to get maximum attachment size from Config app
        if (Session["AxAttachmentSize"] != null)
            attachmentSizeMB = Convert.ToInt32(Session["AxAttachmentSize"]);
        lMaxFileSize = attachmentSizeMB*1024*1024; //convert MB to Bytes
        fileuploadsts.Text = string.Format(GetLocalResourceObject("fileuploadsts.Text").ToString(), attachmentSizeMB); //replace filesize(in MB) using parameters
        lblfilecn.Text = string.Format(GetLocalResourceObject("lblfilecn.Text").ToString(), attachmentSizeMB);
    }
    private static void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
    }

    protected void cmdSend_Click(object sender, EventArgs e)
    {
        if ((filMyFile.PostedFile != null) && (filMyFile.PostedFile.ContentLength > 0))
        {
            string sid = Session["nsessionid"].ToString();
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            try
            {

                DirectoryInfo di = new DirectoryInfo(ScriptsPath + "Axpert\\" + sid);
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
            catch (Exception ex)
            {
                throw ex;
            }

            string sFileName = System.IO.Path.GetFileName(filMyFile.PostedFile.FileName);
            string sFileDir = ScriptsPath + "Axpert\\" + sid + "\\";
           
            try
            {
                if (util.IsFileTypeValid(filMyFile.PostedFile))
                {
                    if (filMyFile.PostedFile.ContentLength <= lMaxFileSize)
                    {
                        if ((sFileDir + sFileName).Length > 260)//display warning message if file path exceeds 260 characters
                        {
                            fileuploadsts.Text = GetLocalResourceObject("lblFileSaveErr.Text").ToString();
                         
                            upsts.Value = "";
                        }
                        if (Constants.fileTypes.Contains(filMyFile.PostedFile.FileName.Substring(sFileName.LastIndexOf(".")).ToLower()) == false)
                        {
                            fileuploadsts.Text = "[Invalid File Extension]";
                            cmdSend.Enabled = false;
                        }
                        else
                        {
                            //Save File on disk
                            filMyFile.PostedFile.SaveAs(sFileDir + sFileName);

                            fileuploadsts.Text = "[" + lblFileUp.Text + "]";
                            fileuploadsts.ForeColor = System.Drawing.Color.Green;
                            upsts.Value = "Uploaded Successfully";

                            fname.Value = sFileName;
                            //Page.RegisterStartupScript("myScript", "<script language=JavaScript>alert('r + "');</script>"); 
                            //ClientScript.RegisterOnSubmitStatement(GetType(), "cli", "DoClientFunction()");
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "DoClientFunction", "DoClientFunction()", true);
                        }
                    }
                    else
                    {
                        fileuploadsts.Text = "";
                        upsts.Value = "";
                        fileuploadsts.Text = lblfilecn.Text;
                    }
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
