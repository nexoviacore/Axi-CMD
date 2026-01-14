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


public partial class gridfileupload : System.Web.UI.Page
{
    string succ = string.Empty;
    string rowNo = string.Empty;
    Util.Util util = new Util.Util();
    long lMaxFileSize = 1000000;
    int attachmentSizeMB = 1;
    string atFName = string.Empty;
    //static string attTransId = string.Empty;
    //static string attFldName = string.Empty;
    public string direction = "ltr";
    LogFile.Log logObj = new LogFile.Log();
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
        util.IsValidSession();
        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = util.toggleTheme();
        if (Request.UrlReferrer != null)
        {
            if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("tstruct.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("gridfileupload.aspx")))
            {
                Response.Redirect("~/CusError/AxCustomError.aspx");
            }
        }

        if (Util.Util.CheckCrossScriptingInString(Request.QueryString.ToString()))
            Response.Redirect("~/CusError/AxCustomError.aspx");

        if (!IsPostBack)
        {
            hdnExeTrace.Value = "GridFileUpload page load started ♦ ";
            if (Request.QueryString["attFld"] != null)
            {
                if (!util.IsUserNameValid(Request.QueryString["attFld"].ToString()))
                    Response.Redirect(Constants.PARAMERR);
                hdnAttFld.Value = Request.QueryString["attFld"].ToString();
            }

            if (Request.QueryString["atFname"] != null)
                atFName = Request.QueryString["atFname"].ToString();

            if (Request.QueryString["dcNo"] != null)
                hdnDcNo.Value = Request.QueryString["dcNo"];
            if (Request.QueryString["attTransId"] != null)
                hdnattTransId.Value = Request.QueryString["attTransId"];
            if (Request.QueryString["attFldName"] != null)
                hdnattFldName.Value = Request.QueryString["attFldName"];
        }
        float maxSize = lMaxFileSize / 1000000;
        //lblTypeInfo.Text = "[Supported file types are word, Image and PDF]";

        //to get maximum attachment size from Config app
        if (Session["AxAttachmentSize"] != null)
            attachmentSizeMB = Convert.ToInt32(Session["AxAttachmentSize"]);
        lMaxFileSize = attachmentSizeMB * 1024 * 1024; //convert MB to Bytes
        fileuploadsts.Text = string.Format(GetLocalResourceObject("fileuploadsts.Text").ToString(), attachmentSizeMB); //replace filesize(in MB) using parameters
        lblfilecn.Text = string.Format(GetLocalResourceObject("lblfilecn.Text").ToString(), attachmentSizeMB);

    }
    public string GetGlobalAttachPath()
    {
        bool isLocalFolder = false;
        bool isRemoteFolder = false;
        string imagePath = string.Empty;
        string imageServer = string.Empty;
        string grdAttPath = string.Empty;
        string errorMessage = string.Empty;
        string mapUsername = string.Empty;
        string mapPassword = string.Empty;
        try
        {
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
        }
        catch (Exception ex)
        {
            logObj.CreateLog("Getting exception in GetGlobalAttachPath-" + ex.Message + "-- grdAttPath-" + grdAttPath, "GridAttachmentErrors\\" + HttpContext.Current.Session.SessionID, "GridFileupload-gblpath", "new", "true");
            grdAttPath = string.Empty;
        }
        return grdAttPath;
    }

    protected void cmdSend_Click(object sender, EventArgs e)
    {
        HttpFileCollection httpAttFiles = Request.Files;
        string fNames = string.Empty;
        string sid = string.Empty;

        for (int i = 0; i < httpAttFiles.Count; i++)
        {
            HttpPostedFile httpAttFile = httpAttFiles[i];

            if ((httpAttFile != null) && (httpAttFile.ContentLength > 0))
            {
                int idx = hdnAttFld.Value.LastIndexOf("F");
                rowNo = hdnAttFld.Value.Substring(idx - 3, 3);
                if (Session["nsessionid"] != null)
                    sid = Session["nsessionid"].ToString();
                else
                {
                    SessExpires();
                    return;
                }
                bool isFileServer = false;
                string destFilePath = string.Empty;
                bool isLocalPath = false;
                string grdAttPath = string.Empty;

                string attFldName = hdnattFldName.Value;
                string attTransId = hdnattTransId.Value;

                if (!attFldName.StartsWith("axp_nga_"))
                {
                    if (hdnImagePath.Value != "")
                    {
                        if (HttpContext.Current.Session["AxpImageServerGbl"] != null)
                        {
                            grdAttPath = HttpContext.Current.Session["AxpImageServerGbl"].ToString();
                            grdAttPath = grdAttPath.Replace(";bkslh", @"\");
                        }
                        if (hdnImagePath.Value != string.Empty && hdnImagePath.Value.Contains(":"))
                            grdAttPath = hdnImagePath.Value;
                        else if (grdAttPath != string.Empty)
                            grdAttPath = grdAttPath + "\\" + hdnImagePath.Value;
                        destFilePath = grdAttPath;
                        isLocalPath = true;
                    }
                    else
                    {
                        destFilePath = GetGlobalAttachPath();
                    }
                }
                string authenticationStatus = string.Empty;
                if (destFilePath != string.Empty && util.GetAuthentication(ref authenticationStatus) && !attFldName.StartsWith("axp_nga_"))
                {
                    destFilePath = destFilePath.Replace("\\", "\\\\");

                    if (!isLocalPath)
                    {
                        if (attFldName.ToLower() == "axp_gridattach_" + hdnDcNo.Value)
                            destFilePath += attTransId + "\\\\" + HttpContext.Current.Session["username"].ToString() + "\\\\" + attFldName + "\\\\";
                        else
                            destFilePath += attTransId + "\\\\" + attFldName + "\\\\";
                    }
                    try
                    {
                        hdnFilePath.Value = destFilePath;
                        DirectoryInfo di = new DirectoryInfo(destFilePath);
                        //' Determine whether the directory exists.
                        if (!di.Exists)
                            di.Create();
                        isFileServer = true;
                    }
                    catch (Exception ex)
                    {
                        logObj.CreateLog("Getting exception in destFilePath Directory-" + ex.Message + "-- destFilePath-" + destFilePath, "GridAttachmentErrors\\" + HttpContext.Current.Session.SessionID, "GridFileupload-exception", "new", "true");
                        isFileServer = false;
                        destFilePath = HttpContext.Current.Application["scriptspath"].ToString() + "axpert\\" + sid;
                        try
                        {
                            hdnFilePath.Value = HttpContext.Current.Application["scriptsurlpath"].ToString() + "axpert//" + sid + "//";
                            DirectoryInfo di = new DirectoryInfo(destFilePath);
                            //' determine whether the directory exists.
                            if (!di.Exists)
                                di.Create();

                        }
                        catch (Exception exc)
                        {
                            logObj.CreateLog("Getting exception in Axpert folder Directory-" + exc.Message + "-- destFilePath-" + destFilePath, "GridAttachmentErrors\\" + HttpContext.Current.Session.SessionID, "GridFileupload-axpert", "new", "true");
                            throw exc;
                        }
                        //throw ex;
                    }
                }
                else
                {
                    destFilePath = HttpContext.Current.Application["scriptspath"].ToString() + "axpert\\" + sid;
                    try
                    {
                        hdnFilePath.Value = HttpContext.Current.Application["scriptsurlpath"].ToString() + "axpert//" + sid + "//";
                        DirectoryInfo di = new DirectoryInfo(destFilePath);
                        //' determine whether the directory exists.
                        if (!di.Exists)
                            di.Create();

                    }
                    catch (Exception ex)
                    {
                        logObj.CreateLog("Getting exception in Axpert folder Directory outer conditions -" + ex.Message, "GridAttachmentErrors\\" + HttpContext.Current.Session.SessionID, "GridFileupload-axpertouter", "new", "true");
                        throw ex;
                    }
                }

                string sFileName = System.IO.Path.GetFileName(httpAttFile.FileName);
                string Ext = sFileName.Substring(sFileName.LastIndexOf("."));
                sFileName = sFileName.Substring(0, sFileName.LastIndexOf("."));
                hdnType.Value = Ext;
                sFileName = sFileName + Ext;
                try
                {
                    if (util.IsFileTypeValid(httpAttFile))
                    {
                        if (httpAttFile.ContentLength <= lMaxFileSize)
                        {
                            if ((destFilePath + sFileName).Length > 260)//display warning message if file path exceeds 260 characters
                            {
                                fileuploadsts.Text = GetLocalResourceObject("lblFileSaveErr.Text").ToString();
                                upsts.Value = "";
                                cmdSend.Enabled = false;
                                string execTrace = hdnExeTrace.Value;
                                hdnExeTrace.Value = execTrace + fileuploadsts.Text + " ♦ ";
                                break;
                            }
                            if (Constants.fileTypes.Contains(httpAttFile.FileName.Substring(sFileName.LastIndexOf(".")).ToLower()) == false)
                            {
                                fileuploadsts.Text = "[Invalid File Extension]";
                                cmdSend.Enabled = false;
                                string execTrace = hdnExeTrace.Value;
                                hdnExeTrace.Value = execTrace + fileuploadsts.Text + " ♦ ";
                                break;
                            }
                            else
                            {
                                if (isFileServer)
                                {
                                    if (attFldName.ToLower() == "axp_gridattach_" + hdnDcNo.Value)
                                    {
                                        string fullPath = destFilePath + HttpContext.Current.Session["username"].ToString() + "-" + sFileName;
                                        string attFldNameRow = attFldName + int.Parse(rowNo);
                                        Session["attGridFileServer"] = (Session["attGridFileServer"] != null && Session["attGridFileServer"].ToString() != string.Empty) ? (Session["attGridFileServer"].ToString() + "♦" + attFldNameRow + "~" + fullPath) : (attFldNameRow + "~" + fullPath);
                                        //Save File on disk
                                        httpAttFile.SaveAs(destFilePath + HttpContext.Current.Session["username"].ToString() + "-" + sFileName);//+ rowNo
                                        hdnAxGridAttSavedPath.Value = destFilePath + HttpContext.Current.Session["username"].ToString() + "-" + sFileName;
                                    }
                                    else
                                    {
                                        string fullPath = destFilePath + HttpContext.Current.Session["username"].ToString() + "-" + sFileName;
                                        string attFldNameRow = attFldName + int.Parse(rowNo);
                                        Session["attGridFileServer"] = (Session["attGridFileServer"] != null && Session["attGridFileServer"].ToString() != string.Empty) ? (Session["attGridFileServer"].ToString() + "♦" + attFldNameRow + "~" + fullPath) : (attFldNameRow + "~" + fullPath);
                                        //Save File on disk
                                        httpAttFile.SaveAs(destFilePath + HttpContext.Current.Session["username"].ToString() + "-" + sFileName);//+ rowNo
                                        hdnAxGridAttSavedPath.Value = destFilePath + HttpContext.Current.Session["username"].ToString() + "-" + sFileName;
                                    }
                                }
                                else
                                {
                                    //Save File on disk
                                    httpAttFile.SaveAs(destFilePath + "\\" + sFileName);//+ rowNo
                                    hdnAxGridAttSavedPath.Value = destFilePath + "\\" + sFileName;
                                }
                                fileuploadsts.Text = "[" + lblFileUp.Text + "]";
                                fileuploadsts.ForeColor = System.Drawing.Color.Green;
                                upsts.Value = "Uploaded Successfully";

                                string execTrace = hdnExeTrace.Value;
                                hdnExeTrace.Value = execTrace + " Uploaded Successfully ♦ Uploaded File Path:" + hdnAxGridAttSavedPath.Value + "♦";

                                if (fname.Value != "")
                                    fname.Value = fname.Value + ',' + sFileName;
                                else
                                    fname.Value = sFileName;// + rowNo

                                Page.ClientScript.RegisterStartupScript(this.GetType(), "DoClientFunction", "DoClientFunction()", true);
                            }
                        }
                        else
                        {
                            upsts.Value = "";
                            fileuploadsts.Text = "[" + lblfilecn.Text + "]";
                            string execTrace = hdnExeTrace.Value;
                            hdnExeTrace.Value = execTrace + lblfilecn.Text + "♦";
                        }
                    }
                }
                catch (Exception ex)//in case of an error
                {
                    logObj.CreateLog("Getting exception in file validation or save-" + ex.Message, "GridAttachmentErrors\\" + HttpContext.Current.Session.SessionID, "GridFileupload-save", "new", "true");
                    fileuploadsts.Text = lblAnError.Text;
                    upsts.Value = fileuploadsts.Text;
                    string execTrace = hdnExeTrace.Value;
                    hdnExeTrace.Value = execTrace + lblAnError.Text + " ♦ Exception:" + ex.Message + "♦";
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
