using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class aspx_ImageUpload : System.Web.UI.Page
{
    string scriptsPath = string.Empty;
    string scriptsUrlPath = string.Empty;
    string sid = string.Empty;
    public string fieldName = string.Empty;
    public string isAxpImagePath = "false";
    public string AxpCameraOption = "false";
    public bool fileUpload1 = false;
    public string direction = "ltr";
    Util.Util util = new Util.Util();
    public string langType = "en";
    long lMaxFileSize = 1000000;
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
        //util.IsValidAxpertSession();
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
        scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        scriptsUrlPath = HttpContext.Current.Application["ScriptsurlPath"].ToString();
        if (Session["nsessionid"] != null)
            sid = Session["nsessionid"].ToString();
        else
        {
            SessExpires();
            return;
        }
        if (util.IsValidQueryString(Request.RawUrl) == false)
        {
            HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.INVALIDURL);
        }
        fieldName = Request.QueryString["fldname"].ToString();
        if (Request.QueryString["isAxpImagePath"] != null)
        {
            isAxpImagePath = Request.QueryString["isAxpImagePath"].ToString();
        }
        if (Request.QueryString["AxpCameraOption"] != null)
        {
            AxpCameraOption = Request.QueryString["AxpCameraOption"].ToString().ToLower();
        }
        //to get maximum attachment size from Config app
        if (Session["AxAttachmentSize"] != null)
            attachmentSizeMB = Convert.ToInt32(Session["AxAttachmentSize"]);
        lMaxFileSize = attachmentSizeMB * 1024 * 1024; //convert MB to Bytes
        //fileuploadsts.Text = string.Format(GetLocalResourceObject("fileuploadsts.Text").ToString(), attachmentSizeMB); //replace filesize(in MB) using parameters
        lblfilesize.Text = string.Format(GetLocalResourceObject("lblfilesize.Text").ToString(), attachmentSizeMB);
    }
    protected void btnCapture_Click(object sender, EventArgs e)
    {
        string base64 = mydata.Value;
        string[] substr = base64.Split(',');

        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(substr[1])))
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string filePath = "";
            using (Bitmap bm2 = new Bitmap(ms))
            {

                int fIdx = fieldName.LastIndexOf("F");
                string captureName = "";
                string tmpFldName = "";
                if (fIdx != -1)
                {
                    captureName = timeStamp + fieldName.Substring(0, fIdx - 3) + ".jpg";
                    tmpFldName = fieldName.Substring(0, fIdx - 3);
                }
                DirectoryInfo di = new DirectoryInfo(scriptsPath + "axpert\\" + sid + "\\" + tmpFldName);
                //The folder path for uploading the image. 
                if (tmpFldName.StartsWith(Constants.IMGPrefix))
                {
                    filepathna.Text = scriptsUrlPath + "axpert/" + sid + "/" + tmpFldName + "/" + System.Uri.EscapeDataString(captureName);
                    filePath = scriptsPath + "axpert\\" + sid + "\\" + tmpFldName + "\\" + captureName;
                }
                //The folder path for uploading the image - AxpimagePath. 
                else if (isAxpImagePath == "true")
                {
                    filepathna.Text = scriptsUrlPath + "axpert/" + sid + "/" + tmpFldName + "/" + System.Uri.EscapeDataString(captureName);
                    filePath = scriptsPath + "axpert\\" + sid + "\\" + tmpFldName + "\\" + captureName;
                }

                else
                {
                    di = new DirectoryInfo(scriptsPath + "axpert\\" + sid);
                    filepathna.Text = scriptsUrlPath + "axpert/" + sid + "/" + System.Uri.EscapeDataString(captureName);
                    filePath = scriptsPath + "axpert\\" + sid + "\\" + captureName;
                }


                if (!di.Exists)
                    di.Create();
                fname.Text = captureName;
                bm2.Save(filePath);
                ClientScript.RegisterStartupScript(GetType(), "name", "<script language=\"javascript\"> CloseWindow('" + fieldName + "')</script>");

                //Page.ClientScript.RegisterStartupScript(GetType(), "name", "CloseWindow('" + fieldName + "')");
            }

        }
    }

    //protected void btnUpload_Click(object sender, EventArgs e)
    //{
    //    if (filMyFile.PostedFile.ContentLength > 0)
    //    {
    //        HttpPostedFile uploadedImgFile;
    //        uploadedImgFile = filMyFile.PostedFile;
    //        int fileLength = 0;
    //        fileLength = uploadedImgFile.ContentLength;
    //        string imageFileName = "";
    //        string filePath = "";
    //        if (fileLength > 0 && fileLength < lMaxFileSize)
    //        {
    //            Byte[] fileData = new Byte[fileLength];
    //            uploadedImgFile.InputStream.Read(fileData, 0, fileLength);
    //            imageFileName = Path.GetFileName(uploadedImgFile.FileName);
    //            int fIdx = fieldName.LastIndexOf("F");
    //            string tmpFldName = "";
    //            if (fIdx != -1)
    //            {
    //                tmpFldName = fieldName.Substring(0, fIdx - 3);
    //            }
    //            DirectoryInfo di = new DirectoryInfo(scriptsPath + "axpert\\" + sid + "\\" + tmpFldName);


    //            //The folder path for uploading the image. 
    //            if (tmpFldName.StartsWith(Constants.IMGPrefix))
    //            {
    //                filepathna.Text = scriptsUrlPath + "axpert/" + sid + "/" + tmpFldName + "/" + System.Uri.EscapeDataString(imageFileName);
    //                filePath = scriptsPath + "axpert\\" + sid + "\\" + tmpFldName + "\\" + imageFileName;
    //            }
    //            //The folder path for uploading the image - AxpimagePath. 
    //            else if (isAxpImagePath == "true")
    //            {
    //                filepathna.Text = scriptsUrlPath + "axpert/" + sid + "/" + tmpFldName + "/" + System.Uri.EscapeDataString(imageFileName);
    //                filePath = scriptsPath + "axpert\\" + sid + "\\" + tmpFldName + "\\" + imageFileName;
    //            }

    //            else
    //            {
    //                di = new DirectoryInfo(scriptsPath + "axpert\\" + sid);
    //                filepathna.Text = scriptsUrlPath + "axpert/" + sid + "/" + System.Uri.EscapeDataString(imageFileName);
    //                filePath = scriptsPath + "axpert\\" + sid + "\\" + imageFileName;
    //            }
    //            if (!di.Exists)
    //                di.Create();
    //            fname.Text = imageFileName;
    //            //check for file content
    //            string fileType = uploadedImgFile.ContentType;
    //            if (util.IsFileTypeValid(uploadedImgFile))
    //            {
    //                string[] imageTypes = new string[] { "image/gif", "image/pjpeg", "image/jpg", "image/pjpg", "image/jpeg", "image/png", "image/bmp", "image/tiff", "image/tif" };
    //                if (imageTypes.Contains(uploadedImgFile.ContentType))
    //                {
    //                    WriteToFile(filePath, ref fileData);
    //                    fileuploadsts.Text = "[File uploaded successfully!]";
    //                    fileuploadsts.ForeColor = System.Drawing.Color.Green;
    //                    ClientScript.RegisterStartupScript(GetType(), "name", "<script language=\"javascript\"> CloseWindow('" + fieldName + "')</script>");
    //                }
    //                else
    //                {
    //                    fileuploadsts.Text = "[" + lblfilecn.Text + "]";
    //                    fileuploadsts.ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2222");
    //                }
    //            }
    //            else
    //            {
    //                fileuploadsts.Text = "[" + lblfilecn.Text + "]";
    //                fileuploadsts.ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2222");
    //            }
    //        }
    //        else
    //        {
    //            fileuploadsts.Text = "[" + string.Format(GetLocalResourceObject("lblfilesize.Text").ToString(), attachmentSizeMB) + "]";
    //            fileuploadsts.ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2222");
    //        }
    //    }
    //}


    // Writes file to current folder
    private void WriteToFile(string strPath, ref byte[] Buffer)
    {
        // Create a file
        FileStream newFile = new FileStream(strPath, FileMode.Create);
        // Write data to the file
        newFile.Write(Buffer, 0, Buffer.Length);
        //Close file
        newFile.Close();
    }

    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");
    }
}
