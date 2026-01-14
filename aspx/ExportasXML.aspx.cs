using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Globalization;
using ASBExt;

public partial class ExportasXML : System.Web.UI.Page
{
    string filePath = string.Empty;
    string fileName = string.Empty;

    string sid = string.Empty;
    string proj = string.Empty;
    string user = string.Empty;
    string transId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["project"] == null)
        {
            SessionExpired();
        }
        else
        {
            SetGlobalVariables();

            string result = CallWebService();

            if (string.IsNullOrEmpty(result) || (result.StartsWith("<error>")))
            {
                result = result.Replace("<error>", "");
                result = result.Replace("</error>", "");
                Server.Transfer("err.aspx?errmsg=" + result);
            }
            else
            {
                SaveAsXMLFile(result);
                DownLoadXMLFile();
            }
        }
    }

    private void SetGlobalVariables()
    {
        transId = Convert.ToString(Request.QueryString["tid"]);
        proj = Session["project"].ToString();
        user = Session["user"].ToString();
        sid = Session["nsessionid"].ToString();
    }


    private string CallWebService()
    {
        Util.Util util = new Util.Util();
        LogFile.Log logobj = new LogFile.Log();
        WebServiceExt objExt = new WebServiceExt();

        string result = string.Empty;
        string sqlQuery = string.Empty;
        string inputXML = string.Empty;

        string branchname = Convert.ToString(Request.QueryString["branch"]);
        branchname = branchname.Replace("¿", "&amp;");
        string company = Convert.ToString(Request.QueryString["comp"]);
        company = company.Replace("¿", "&amp;");
        fileName = "Voucher";
        string voucherDate = Convert.ToString(Request.QueryString["vcd"]);
        sqlQuery = "<sql>select fn_TXML_GetVouchers( '" + company + "','" + branchname + "', to_date('" + voucherDate + "','dd/mm/yyyy')) as COLUMN1 from dual</sql>";

        string errorLog = logobj.CreateLog("Calling GetChoice function.", sid, "GetChoice-" + fileName, "new");

        inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sid + "' trace='" + errorLog + "' user='" + user + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
        inputXML += sqlQuery + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";

        try
        {
            result = objExt.CallGetChoiceWebService("", inputXML);
            result = result.Replace("&amp;", "&amp;amp;");
            DBContext obj = new DBContext();
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception while excuting GetChoice function" + ex.ToString(), sid, "GetChoice-" + fileName, "");
            Response.Redirect(util.errorString);
        }

        return result;
    }


    private void SaveAsXMLFile(string result)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(result);
        XmlNode xmlnode = xmlDoc.SelectSingleNode("//response/row/COLUMN1");

        if (string.IsNullOrEmpty(Convert.ToString(xmlnode)))
            return;

        XmlDocument xmlDoc1 = new XmlDocument();
        //xmlDoc1.LoadXml(xmlDoc.InnerXml);
        xmlDoc1.LoadXml(xmlnode.InnerText);

        string ScriptsPath = ConfigurationManager.AppSettings["ScriptsPath"];
        string dirPath = ScriptsPath + "Axpert\\" + sid;

        DirectoryInfo di = new DirectoryInfo(dirPath);

        // Determine whether the directory exists.
        if (!di.Exists)
        {
            // create the directory.
            di.Create();
        }

        filePath = dirPath + "\\" + fileName + ".xml";
        xmlDoc1.Save(filePath);
    }

    private void DownLoadXMLFile()
    {
        Boolean fileExists = false;
        if (filePath != "")
        {
            FileInfo files = new FileInfo(filePath);

            if (files.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + files.Name);
                Response.AddHeader("Content-Length", files.Length.ToString());
                Response.ContentType = "application/x-download";
                Response.WriteFile(files.FullName);
                Response.End();

                fileExists = true;
            }
        }

        if (!fileExists)
            Response.Write("<br/><center>File does not exist.</center>");
    }

    // function for handling session timeout.
    public void SessionExpired()
    {
        string url = "sess.aspx";
        Response.Write("<script language='javascript'>");
        Response.Write("if(window.opener && !window.opener.closed){window.opener.parent.location.href='" + url + "';window.close();}else {parent.parent.location.href='" + url + "';}");
        Response.Write("</script>");
    }
}
