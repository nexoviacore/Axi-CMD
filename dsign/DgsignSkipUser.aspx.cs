using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class aspx_Dgsigndetails : System.Web.UI.Page
{
    ASBExt.WebServiceExt objwebservice = new ASBExt.WebServiceExt();
    Util.Util util = new Util.Util();
    LogFile.Log logobj = new LogFile.Log();
    public string Proj = string.Empty;
    public string sessionid = string.Empty;
    public string tstruct = string.Empty;
    public string transId = string.Empty;
    public string docName = string.Empty;
    public string docType = string.Empty;
    string fileName = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {

        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = util.toggleTheme();
        Proj = ConfigurationManager.AppSettings["proj"].ToString();
        sessionid = Session["nsessionid"].ToString();
        if (Request.QueryString["tstname"] != null)
            tstruct = Request.QueryString["tstname"].ToString();
        if (Request.QueryString["ttransid"] != null)
            transId = Request.QueryString["ttransid"].ToString();
        if (Request.QueryString["tdocname"] != null)
            docName = Request.QueryString["tdocname"].ToString();
        if (Request.QueryString["tdoctype"] != null)
            docType = Request.QueryString["tdoctype"].ToString();
        if (!IsPostBack)
            GetDocumentDetails();
    }

    private void GetDocumentDetails()
    {
        string result = string.Empty;
        string inputXML = string.Empty;
        fileName = "getdocdetails";
        try
        {
            string sql = "select stransid,doctype,documentname,username,ordno,case when status='0' then 'pending' when status ='1' then 'Currently pending' when status= -1 then'User Skipped' end status from axdsigntrans where status !='2'  and stransid='" + transId + "' and documentname='" + docName + "' and doctype='" + docType + "' order by ordno";
            inputXML = "<sqlresultset axpapp='" + Proj + "' sessionid='" + sessionid + "' direct='true' trace=''><sql>" + sql + "</sql>";
            inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallGetChoiceWebService("", inputXML);
            ConvertResultToDatatable(result);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Skipping user for signing :--- " + ex.Message.ToString(), sessionid, fileName, "");
        }
    }

    private void ConvertResultToDatatable(string result)
    {
        DataSet ds = new DataSet();
        if (result != string.Empty)
        {
            StringReader sr = new StringReader(result);
            ds.ReadXml(sr);
            if (ds.Tables.Contains("row"))
            {
                if (ds.Tables["row"].Rows.Count > 0)
                {
                    grvDsignFiles.DataSource = ds.Tables["row"];
                    grvDsignFiles.DataBind();

                }

            }
        }
    }

    protected void grvDsignFiles_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "skip")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grvDsignFiles.Rows[index];
            Label lblTransId = (Label)row.FindControl("lblstransid");
            Label lblDocName = (Label)row.FindControl("lbldocumentname");
            Label lblordno = (Label)row.FindControl("lblordno");
            Label lbldoctype = (Label)row.FindControl("lbldoctype");
            Label lblstatus = (Label)row.FindControl("status");
            UpdateStatusSkipppedUser(lblTransId.Text, lblDocName.Text, lblordno.Text, lbldoctype.Text);
        }
    }

    protected void grvDsignFiles_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblstatus = (Label)e.Row.FindControl("lblstatus");
            Label lblskippedstatus = (Label)e.Row.FindControl("lblskippedstatus");
            Button btnskip = (Button)e.Row.FindControl("btnskip");
            if (lblstatus.Text.ToLower() == "user skipped")
            {
                lblskippedstatus.Visible = true;
                btnskip.Visible = false;

            }
            else
            {
                btnskip.Visible = true;
                lblskippedstatus.Visible = false;
            }

            btnskip.Attributes.Add("onclick", "javascript:return " +
           "confirm('Are you sure you want to skip the user')");

        }
    }

    private void UpdateStatusSkipppedUser(string transID, string docName, string orderNo, string docType)
    {
        string result = string.Empty;
        string inputXML = string.Empty;
        string status = string.Empty;
        string skippedUserName = string.Empty;
        string nodeName = string.Empty;

        fileName = "updatingskippeduser";
        try
        {
            string sql = "<s1>select username,status from axdsigntrans  where stransid='" + transID + "' and documentname='" + docName + "' and ordno='" + orderNo + "' and doctype='" + docType + "'</s1>";
            sql += "<s2>update axdsigntrans set status =-1 where stransid='" + transID + "' and documentname='" + docName + "' and ordno='" + orderNo + "' and doctype='" + docType + "'</s2>";
            inputXML = "<sqlresultset axpapp='" + Proj + "' sessionid='" + sessionid + "' direct='true' trace=''><sql >" + sql + "</sql>";
            inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallMultiSQLExec(transID, inputXML);
            if (result != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                XmlNodeList rowNodes = default(XmlNodeList);
                rowNodes = xmlDoc.SelectNodes("//s1//response//row");
                if (rowNodes.Count > 0)
                {
                    foreach (XmlNode chNode in rowNodes)
                    {
                        foreach (XmlNode viewNode in chNode)
                        {
                            nodeName = viewNode.Name.ToLower();

                            if (nodeName == "status")
                                status = viewNode.InnerText;
                            else if (nodeName == "username")
                                skippedUserName = viewNode.InnerText;

                            if (status == "1")
                                GetNextUserLevel(skippedUserName, transID, docName, orderNo, docType);

                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowMessageBox", "ShowMessageBox('User Skipped Sucessfully.');", true);
                GetDocumentDetails();
            }

        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Skipping user for signing :--- " + ex.Message.ToString(), sessionid, fileName, "");
        }
    }

    private void GetNextUserLevel(string skippedUserName, string transID, string docName, string orderNo, string docType)
    {
        try
        {
            string result = string.Empty;
            string sql = string.Empty;
            string inputXML = string.Empty;
            string nextUserName = string.Empty;
            string nextOrderNo = string.Empty;
            string status = string.Empty;
            string isSendMail = string.Empty;
            string isSendSms = string.Empty;
            string nodeName = string.Empty;
            fileName = "getnextuserlevel";
            sql = "select sendmail,sendsms,status,username,ordno from axdsigntrans  where stransid='" + transID + "' and documentname='" + docName + "' and ordno >'" + orderNo + "' and doctype='" + docType + "'";
            inputXML = "<sqlresultset axpapp='" + Proj + "' sessionid='" + sessionid + "' direct='true' trace=''><sql>" + sql + "</sql>";
            inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallGetChoiceWebService(transID, inputXML);
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
                            nodeName = viewNode.Name.ToLower();
                            if (nodeName == "username")
                                nextUserName = viewNode.InnerText;
                            else if (nodeName == "status")
                                status = viewNode.InnerText;
                            else if (nodeName == "ordno")
                                nextOrderNo = viewNode.InnerText;
                            else if (nodeName == "sendmail")
                                isSendMail = viewNode.InnerText;
                            else if (nodeName == "sendsms")
                                isSendSms = viewNode.InnerText;

                        }
                        if (status != "-1")
                            break;

                    }
                    UpdateNextUserLevel(transID, docName, nextOrderNo, docType);
                    if (isSendMail.ToLower() == "y")
                        SendMailToUser(transID, docName, nextOrderNo);

                    if (isSendSms.ToLower() == "y")
                        SendSMSToUser(transID, docName, nextOrderNo);
                }
                else
                {

                    nextUserName = "final";
                }
                CopyFileToNexTUser(skippedUserName, nextUserName, transID, docName);
            }

        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Skipping user for signing :--- " + ex.Message.ToString(), sessionid, fileName, "");
        }
    }

    private void UpdateNextUserLevel(string transID, string docName, string nextOrderNo, string docType)
    {

        try
        {
            string sql = string.Empty;
            string inputXML = string.Empty;
            string result = string.Empty;
            fileName = "updatenxtuser";
            sql = "update axdsigntrans set status = 1 where stransid='" + transID + "' and documentname='" + docName + "' and ordno='" + nextOrderNo + "' and doctype='" + docType + "'";
            inputXML = "<sqlresultset axpapp='" + Proj + "' sessionid='" + sessionid + "' direct='true' trace=''><sql>" + sql + "</sql>";
            inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallGetChoiceWebService(transID, inputXML);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Skipping user for signing :--- " + ex.Message.ToString(), sessionid, fileName, "");
        }

    }

    private void CopyFileToNexTUser(string skippedUserName, string nextUserName, string transID, string docName)
    {
        string traceFileName = "copfiletouser";
        try
        {

            string scriptsPath = ConfigurationManager.AppSettings["Scriptspath"].ToString();
            string sourcePath = scriptsPath + "DigiSign\\" + transID + "\\" + skippedUserName;
            string DestinationPath = scriptsPath + "DigiSign\\" + transID + "\\" + nextUserName;

            if (!Directory.Exists(DestinationPath))
                Directory.CreateDirectory(DestinationPath);

            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    string fileName = System.IO.Path.GetFileName(s);
                    if (fileName.ToLower() == docName.ToLower())
                    {
                        string destFile = System.IO.Path.Combine(DestinationPath, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Skipping user for signing :--- " + ex.Message.ToString(), sessionid, traceFileName, "");
        }
    }


    #region mailevents
    private void SendMailToUser(string TransID, string filename, string nextOrderNo)
    {


        fileName = "sendingmail";
        try
        {
            string mailreult = string.Empty;
            StringBuilder inputXml = new StringBuilder();
            inputXml.Append("<root sname ='" + TransID + "' dsign='t'  direct='true' transid='" + TransID + "' filename='" + filename + "' ordno='" + nextOrderNo + "'  axpapp = '" + Proj + "' sessionid = '" + Session["nsessionid"].ToString() + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' stype=''  trace='' recordid='0'>");
            inputXml.Append("<sendmail><mailfrom></mailfrom><mailto></mailto><cc></cc><subject></subject><body></body>");
            inputXml.Append("<attach></attach><axpattach></axpattach><msoutlook>false</msoutlook><pruntime>false</pruntime></sendmail></root>");
            mailreult = objwebservice.CallAxpSendMailWS(TransID, inputXml.ToString());
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Skipping user for signing :--- " + ex.Message.ToString(), sessionid, fileName, "");
        }

    }
    #endregion

    #region smsevents
    private void SendSMSToUser(string TransID, string filename, string nextOrderNo)
    {
        fileName = "sendsms";
        try
        {
            string result;
            string mobileNo = string.Empty;
            string smsContents = string.Empty;
            string nodeName = string.Empty;
            string sql = "select mobileno ,smscontent  from axdsigntrans axt join axdsignmail axm on axt.axpmailid = axm.axpmailid where  STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO ='" + nextOrderNo + "'";
            string inputXML1 = "<sqlresultset axpapp='" + Proj + "' sessionid='" + sessionid + "' direct='true' trace='' ><sql>" + sql + "</sql>";
            inputXML1 += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallGetChoiceWebService("", inputXML1);
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
                            nodeName = viewNode.Name.ToLower();
                            if (nodeName == "mobileno")
                                mobileNo = viewNode.InnerText;

                            else if (nodeName == "smscontent")
                                smsContents = viewNode.InnerText;
                        }
                    }
                }
                AddSMSDetails(mobileNo, smsContents);
            }
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Skipping user for signing :--- " + ex.Message.ToString(), sessionid, fileName, "");

        }

    }
    private void AddSMSDetails(string mobileNo, string smsContents)
    {

        fileName = "addsmsdetails";
        try
        {
            string result = string.Empty;
            string sql = "insert into axsms(recordid,mobileno,msg,status)(SELECT AxSMS_Seq.NEXTVAL,'" + mobileNo + "','" + smsContents + "','0' from dual)";
            string inputXML1 = "<sqlresultset axpapp='" + Proj + "' sessionid='" + sessionid + "' direct='true' trace='' ><sql>" + sql + "</sql>";
            inputXML1 += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
            result = objwebservice.CallGetChoiceWebService("", inputXML1);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Skipping user for signing :--- " + ex.Message.ToString(), sessionid, fileName, "");

        }

    }
    #endregion


}