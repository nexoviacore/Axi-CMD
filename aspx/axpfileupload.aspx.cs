using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.Services;
using System.Collections;

public partial class aspx_axpfileupload : System.Web.UI.Page
{
    string succ = string.Empty;
    string concatFileName = string.Empty;
    bool isExpExtend = true;
    bool rncName = false;
    Util.Util util = new Util.Util();
    long lMaxFileSize = 1000000;
    int attachmentSizeMB = 1;
    public string direction = "ltr";
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
        if (Session["project"] == null)
        {
            SessExpires();
            return;
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["attFld"] != null)
            {
                if (!util.IsUserNameValid(Request.QueryString["attFld"].ToString()))
                    Response.Redirect(Constants.PARAMERR);
                hdnAttFld.Value = Request.QueryString["attFld"].ToString();
            }

            if (Request.QueryString["dcNo"] != null)
                hdnDcNo.Value = Request.QueryString["dcNo"];

            if (Request.QueryString["attFldName"] != null)
                hdnAttName.Value = Request.QueryString["attFldName"];
        }

        //to get maximum attachment size from Config app
        if (Session["AxAttachmentSize"] != null)
            attachmentSizeMB = Convert.ToInt32(Session["AxAttachmentSize"]);
        lMaxFileSize = attachmentSizeMB * 1024 * 1024; //convert MB to Bytes
        fileuploadsts.Text = string.Format(GetLocalResourceObject("fileuploadsts.Text").ToString(), attachmentSizeMB); //replace filesize(in MB) using parameters
        lblfilecn.Text = string.Format(GetLocalResourceObject("lblfilecn.Text").ToString(), attachmentSizeMB);

    }

    protected string GetGlobalAttachPath()
    {
        bool isLocalFolder = false;
        bool isRemoteFolder = false;
        string imagePath = string.Empty;
        string imageServer = string.Empty;
        string grdAttPath = string.Empty;
        string errorMessage = string.Empty;
        string mapUsername = string.Empty;
        string mapPassword = string.Empty;

        if (HttpContext.Current.Session["AxpImageServerGbl"] != null)
        {
            imageServer = HttpContext.Current.Session["AxpImageServerGbl"].ToString();
            imageServer = imageServer.Replace(";bkslh", @"\");
        }
        if (HttpContext.Current.Session["AxpImagePathGbl"] != null)
        {
            imagePath = HttpContext.Current.Session["AxpImagePathGbl"].ToString();
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
            if (HttpContext.Current.Session["AxGridAttachPath"] != null)
            {
                grdAttPath = HttpContext.Current.Session["AxGridAttachPath"].ToString();
            }
        }
        return grdAttPath;
    }

    protected string GetDestFilePath()
    {
        string destFilePath = string.Empty;
        try
        {
            string sid = string.Empty;
            if (Session["nsessionid"] != null)
                sid = Session["nsessionid"].ToString();
            string AttPath = string.Empty;
            if (hdnaxpFilePath.Value != "")// AxpFilePath_ field value 
            {
                destFilePath = AttPath = hdnaxpFilePath.Value;
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
                destFilePath = GetGlobalAttachPath();
            }

            string authenticationStatus = string.Empty;
            if (destFilePath != string.Empty && util.GetAuthentication(ref authenticationStatus))
            {
                if (destFilePath != string.Empty && !destFilePath.EndsWith("\\"))
                    destFilePath += "\\";
                destFilePath = destFilePath.Replace("\\", "\\\\");
                hdnFilePath.Value = destFilePath;
                DirectoryInfo di = new DirectoryInfo(destFilePath);
                //' Determine whether the directory exists.
                if (!di.Exists)
                    di.Create();
            }
        }
        catch (Exception ex)
        {
            destFilePath = string.Empty;
            throw ex;
        }
        return destFilePath;
    }

    protected void cmdSend_Click(object sender, EventArgs e)
    {
        string destFilePath = GetDestFilePath();
        if (destFilePath == string.Empty)
        {
            fileuploadsts.Text = "[" + GetLocalResourceObject("lblPath.Text").ToString() + "]";
            fileuploadsts.ForeColor = System.Drawing.Color.Red;
            upsts.Value = "";
            cmdSend.Enabled = false;
        }
        else
        {
            HttpFileCollection httpAttFiles = Request.Files;
            for (int i = 0; i < httpAttFiles.Count; i++)
            {
                HttpPostedFile httpAttFile = httpAttFiles[i];
                if ((httpAttFile != null) && (httpAttFile.ContentLength > 0))
                {
                    string thisFileName = Path.GetFileName(httpAttFile.FileName);
                    string Ext = thisFileName.Substring(thisFileName.LastIndexOf("."));
                    hdnType.Value = Ext;

                    try
                    {
                        if (!util.IsFileTypeValid(httpAttFile))
                        {
                            upsts.Value = "";
                            fileuploadsts.Text = "[" + GetLocalResourceObject("lblInvFile.Text").ToString() + "]";
                            fileuploadsts.ForeColor = System.Drawing.Color.Red;
                            cmdSend.Enabled = false;
                            break;
                        }
                        else if (httpAttFile.ContentLength > lMaxFileSize)
                        {
                            upsts.Value = "";
                            fileuploadsts.Text = "[" + lblfilecn.Text + "]";
                            fileuploadsts.ForeColor = System.Drawing.Color.Red;
                            cmdSend.Enabled = false;
                            break;
                        }
                        else if ((destFilePath + thisFileName).Length > 260)//display warning message if file path exceeds 260 characters
                        {
                            fileuploadsts.Text = "[" + GetLocalResourceObject("lblFileSaveErr.Text").ToString() + "]";
                            fileuploadsts.ForeColor = System.Drawing.Color.Red;
                            upsts.Value = "";
                            cmdSend.Enabled = false;
                            break;
                        }
                        else if (Constants.fileTypes.Contains(httpAttFile.FileName.Substring(thisFileName.LastIndexOf(".")).ToLower()) == false)
                        {
                            fileuploadsts.Text = "[" + GetLocalResourceObject("lblInvFileExt.Text").ToString() + "]";
                            fileuploadsts.ForeColor = System.Drawing.Color.Red;
                            cmdSend.Enabled = false;
                            break;
                        }
                        else
                        {
                            if (hdnAttName.Value == "")
                                break;

                            string fullPath = destFilePath + concatFileName + thisFileName;
                            if (isExpExtend == true)
                            {
                                if (File.Exists(fullPath))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:CheckIsFileExists();", true);
                                    cmdSend.Enabled = false;
                                    return;
                                }
                            }
                            ArrayList arListExist = new ArrayList();
                            string attFldName = hdnAttName.Value;
                            if (Session["AxpAttFileServer"] != null)
                                arListExist = (ArrayList)Session["AxpAttFileServer"];
                            if (arListExist.IndexOf(attFldName + "~" + fullPath) == -1)
                            {
                                arListExist.Add(attFldName + "~" + fullPath);
                                Session["AxpAttFileServer"] = arListExist;
                            }
                            //Save File on disk
                            httpAttFile.SaveAs(destFilePath + concatFileName + thisFileName);
                            hdnaxpFileSavedPath.Value = destFilePath + concatFileName + thisFileName;
                            fileuploadsts.Text = "[" + lblFileUp.Text + "]";
                            fileuploadsts.ForeColor = System.Drawing.Color.Green;
                            upsts.Value = "Uploaded Successfully";

                            if (rncName)
                            {
                                randNumber.Value = "true";
                                fname.Value = concatFileName + thisFileName;
                            }
                            else
                                fname.Value = thisFileName;
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
    }

    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }
}
