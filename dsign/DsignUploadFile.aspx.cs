using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Web.SessionState;
using System.Data;
using System.Xml;
public partial class aspx_UploadFile : System.Web.UI.Page
{

    #region globalvariables
    ASBExt.WebServiceExt objwebservice = new ASBExt.WebServiceExt();
    public string printDocName = string.Empty;
    public string attachTxtID = string.Empty;
    public string Proj = string.Empty;
    public string newID = string.Empty;
    public string Username = string.Empty;
    public string password = string.Empty;
    public string seed = string.Empty;
    public string comments = string.Empty;
    #endregion

    #region pageevents
    protected void Page_Load(object sender, EventArgs e)
    {

        Proj = ConfigurationManager.AppSettings["proj"].ToString();

        newID = "abcdefghijk";

        //if condtion will excuete when user upload the doc for signing
        try
        {
            if ((Request.QueryString["pdocname"] != null) && (Request.QueryString["attachtxtid"] != null))
            {
                printDocName = Request.QueryString["pdocname"].ToString();
                attachTxtID = Request.QueryString["attachtxtid"].ToString();
                hdnAttachTextID.Value = attachTxtID;
            }
            else
            {
                foreach (string f in Request.Files.AllKeys)
                {
                    HttpPostedFile file = Request.Files[f];
                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();
                    string query = string.Empty;
                    string result = string.Empty;
                    string nextuser = string.Empty;
                    string currentUserPath = string.Empty;
                    string nextOrderNo = string.Empty;
                    string destinationpath = Request.QueryString["dpath"].ToString();
                    string TransID = Request.QueryString["transid"].ToString();
                    string OrderNo = Request.QueryString["orderno"].ToString();
                    string filename = Request.QueryString["filemname"].ToString();
                    Username = Request.QueryString["username"].ToString();
                    password = Request.QueryString["password"].ToString();
                    seed = Request.QueryString["seed"].ToString();
                    comments = Request.QueryString["comments"].ToString();

                    //sql to get next user name for signing
                    string sql = "select status,ordno,username  from axdsigntrans where STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO >'" + OrderNo + "' order by ordno";
                    string inputXML = "<sqlresultset axpapp='" + Proj + "' sessionid='" + newID + "' direct='true' trace='' ><sql>" + sql + "</sql>";
                    inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
                    result = objwebservice.CallGetChoiceWebService(TransID, inputXML);

                    if (result != string.Empty)
                    {
                        string status = string.Empty;


                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(result);
                        XmlNodeList rowNodes = default(XmlNodeList);
                        rowNodes = xmlDoc.SelectNodes("//row");
                        if (rowNodes.Count > 0)
                        {
                            foreach (XmlNode chNode in rowNodes)
                            {
                                foreach (XmlNode viewNode in chNode)
                                {

                                    if (viewNode.Name.ToLower() == "status")
                                        status = viewNode.InnerText;

                                    if (viewNode.Name.ToLower() == "ordno")
                                        nextOrderNo = viewNode.InnerText;


                                    if (viewNode.Name.ToLower() == "username")
                                        nextuser = viewNode.InnerText;
                                }

                                if (status != "-1")
                                    break;
                                // OrderNo = orderNo;

                            }
                        }
                        else
                        {
                            nextuser = "final";
                        }


                    }
                    currentUserPath = destinationpath + "\\" + Username;
                    destinationpath = destinationpath + "\\" + nextuser;

                    //delete orginal doc and add sign documnet to user folder
                    File.Delete(currentUserPath + "\\" + file.FileName);
                    file.SaveAs(currentUserPath + "\\" + file.FileName);

                    // If the directory doesn't exist, create it.
                    if (!Directory.Exists(destinationpath))
                        Directory.CreateDirectory(destinationpath);

                    //save the file to next user level
                    file.SaveAs(destinationpath + "\\" + file.FileName);

                    if (nextuser == "final")
                    {
                        UpdatestatusLastuser(TransID, OrderNo, filename, newID, Proj, result);
                    }
                    else
                    {
                        UpdatestatusNextuser(TransID, OrderNo, nextOrderNo, filename, newID, Proj, result);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    #endregion

    #region Updatemethods
    private void UpdatestatusNextuser(string TransID, string OrderNo, string nextOrderNo, string filename, string newID, string Proj, string result)
    {
        try
        {
            string isSendMail = string.Empty;
            string isSemdSms = string.Empty;
            //int nextOrderNo = (Convert.ToInt32(OrderNo));
            string sql = string.Empty;
            string inputXML = string.Empty;
            //update comments and status for current user
            sql += "<s1>update axdsigntrans set STATUS=2,COMMENTS= '" + comments + "'  where  STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO ='" + OrderNo + "'</s1>";
            //update status for next user
            sql += "<s2>update axdsigntrans set STATUS=1 where STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO =" + nextOrderNo + "</s2>";
            //check mail and sms flag for next user
            sql += "<s3>select sendmail,sendsms from axdsigntrans where STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO =" + nextOrderNo + "</s3>";

            inputXML = "<sqlresultset axpapp='" + Proj + "' sessionid='" + newID + "' direct='true' trace='' ><sql>" + sql + "</sql>";
            inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallMultiSQLExec(TransID, inputXML);

            if (result != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                XmlNodeList rowNodes = default(XmlNodeList);
                rowNodes = xmlDoc.SelectNodes("//s3//response//row");
                if (rowNodes.Count > 0)
                {
                    foreach (XmlNode chNode in rowNodes)
                    {
                        foreach (XmlNode viewNode in chNode)
                        {
                            if (viewNode.Name.ToLower() == "sendmail")
                                isSendMail = viewNode.InnerText;

                            if (viewNode.Name.ToLower() == "sendsms")
                                isSemdSms = viewNode.InnerText;
                        }
                    }
                }
            }
            if (isSendMail.ToLower() == "y")
                SendMailToUser(TransID, filename, nextOrderNo);

            if (isSemdSms.ToLower() == "y")
                SendSMSToUser(TransID, filename, nextOrderNo);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void UpdatestatusLastuser(string TransID, string OrderNo, string filename, string newID, string Proj, string result)
    {
        try
        {
            string inputXML1 = "<sqlresultset axpapp='" + Proj + "' sessionid='" + newID + "' direct='true' trace='' ><sql>update axdsigntrans set STATUS=2,COMMENTS= '" + comments + "'  where  STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO ='" + OrderNo + "'</sql>";
            inputXML1 += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallGetChoiceWebService(TransID, inputXML1);
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    #endregion

    #region fileuploadevents
    protected void btnAttach_click(object sender, EventArgs e)
    {
        try
        {
            FileInfo fi = new FileInfo(inpPdfFile.PostedFile.FileName);
            string fileext = fi.Extension;
            if (fileext.ToLower() == ".pdf")
            {
                if ((inpPdfFile.PostedFile != null) && (inpPdfFile.PostedFile.ContentLength > 0))
                {
                    string sid = Session["nsessionid"].ToString();
                    string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                    hdnOrginalfileName.Value = System.IO.Path.GetFileName(inpPdfFile.PostedFile.FileName);
                    string sFileName = printDocName;
                    string sFileDir = ScriptsPath + "Axpert\\" + sid + "\\";
                    long lMaxFileSize = 1000000;
                    DirectoryInfo di = new DirectoryInfo(ScriptsPath + "Axpert\\" + sid);
                    if (!di.Exists)
                        di.Create();
                    if (inpPdfFile.PostedFile.ContentLength <= lMaxFileSize)
                    {
                        inpPdfFile.PostedFile.SaveAs(sFileDir + sFileName);
                        fileuploadsts.Text = "Uploaded Successfully. Please click Close button now.";
                        fileuploadsts.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        fileuploadsts.Text = "File could not be uploaded. Filesize is more than 1MB.";
                    }

                }
            }
            else
            {
                fileuploadsts.Text = "Please Select only Pdf Files!";

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #endregion

    #region mailevents
    private void SendMailToUser(string TransID, string filename, string nextOrderNo)
    {

        try
        {
            string mailreult = string.Empty;
            string inputXml = string.Empty;
            inputXml += "<root sname ='" + TransID + "' dsign='t' username='" + Username + "' password='" + password + "' seed='" + seed + "'  direct='true' transid='" + TransID + "' filename='" + filename + "' ordno='" + nextOrderNo + "'  axpapp = '" + Proj + "' sessionid = '" + newID + "' stype=''  trace='' recordid='0'>";
            inputXml += "<sendmail>";
            inputXml += "<mailfrom></mailfrom>";
            inputXml += "<mailto></mailto>";
            inputXml += "<cc></cc>";
            inputXml += "<subject></subject>";
            inputXml += "<body></body>";
            inputXml += "<attach></attach>";
            inputXml += "<axpattach></axpattach>";
            inputXml += "<msoutlook>false</msoutlook>";
            inputXml += "<pruntime>false</pruntime>";
            inputXml += "</sendmail></root>";
            mailreult = objwebservice.CallAxpSendMailWS(TransID, inputXml);
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    #endregion

    #region smsevents
    private void SendSMSToUser(string TransID, string filename, string nextOrderNo)
    {
        try
        {
            string result;
            string mobileNo = string.Empty;
            string smsContents = string.Empty;
            string sql = "select mobileno ,smscontent  from axdsigntrans axt join axdsignmail axm on axt.axpmailid = axm.axpmailid where  STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO ='" + nextOrderNo + "'";
            string inputXML1 = "<sqlresultset axpapp='" + Proj + "' sessionid='" + newID + "' direct='true' trace='' ><sql>" + sql + "</sql>";
            inputXML1 += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallGetChoiceWebService(TransID, inputXML1);
            if (result != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                XmlNodeList rowNodes = default(XmlNodeList);
                rowNodes = xmlDoc.SelectNodes("//row");
                if (rowNodes.Count > 0)
                {
                    foreach (XmlNode chNode in rowNodes)
                    {
                        foreach (XmlNode viewNode in chNode)
                        {
                            if (viewNode.Name.ToLower() == "mobileno")
                                mobileNo = viewNode.InnerText;

                            if (viewNode.Name.ToLower() == "smscontent")
                                smsContents = viewNode.InnerText;
                        }
                    }
                }
                AddSMSDetails(mobileNo, smsContents);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    private void AddSMSDetails(string mobileNo, string smsContents)
    {
        try
        {
            string result = string.Empty;
            string sql = "insert into axsms(recordid,mobileno,msg,status)(SELECT AxSMS_Seq.NEXTVAL,'" + mobileNo + "','" + smsContents + "','0' from dual)";
            string inputXML1 = "<sqlresultset axpapp='" + Proj + "' sessionid='" + newID + "' direct='true' trace='' ><sql>" + sql + "</sql>";
            inputXML1 += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallGetChoiceWebService("", inputXML1);
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    #endregion


}
