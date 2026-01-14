using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using System.Xml;

/// <summary>
/// Summary description for Intelliview
/// </summary>
/// 
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]

public class Intelliview : System.Web.Services.WebService
{
    [NonSerialized]
    private com.agile_labs.demo.DashboardApi dasboardApi = new com.agile_labs.demo.DashboardApi();
    string IVNXTSessionID;
    int IVNXTCompID = 0;
    string filtername = string.Empty;
    string reportname = string.Empty;
    string companyname = string.Empty;
    string username = string.Empty;
    string password = string.Empty;
    int reportid = 0;
    int filtertype = 0;
    LogFile.Log logobj = new LogFile.Log();
    [NonSerialized]
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();

    public string CompanyName
    {
        get { return companyname; }
        set { companyname = value; }
    }
    public string UserName
    {
        get { return username; }
        set { username = value; }
    }
    public string PassWord
    {
        get { return password; }
        set { password = value; }
    }
    public string ReportName
    {
        get { return reportname; }
        set { reportname = value; }
    }

    //instantiated on object creation.
    //Starting the intelliview Session
    private string StartSession()
    {
        string wsResult = string.Empty;
        try
        {
            wsResult = dasboardApi.ivnInitializeSession(companyname, username, password, 0, 1, 1, "English", " ");
        }
        catch (Exception e)
        {
            return e.Message;
        }
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(wsResult);
        var node = doc.SelectSingleNode("ResponseMessage");
        if (node.HasChildNodes)
        {
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                var child = node.ChildNodes;
                string nodeName = child[i].Name.ToString();
                if (nodeName == "Session")
                {
                    if (!string.IsNullOrEmpty(child[i].Attributes["Token"].Value))
                    {
                        Session["IVNXTSessionID"] = child[i].Attributes["Token"].Value;
                        IVNXTSessionID = Session["IVNXTSessionID"].ToString();
                    }
                }
                else if (nodeName == "Company")
                {
                    if (!string.IsNullOrEmpty(child[i].Attributes["ID"].Value))
                    {
                        Session["IVNXTCompID"] = int.Parse(child[i].Attributes["ID"].Value);
                        IVNXTCompID = int.Parse(Session["IVNXTCompID"].ToString());
                    }
                }
            }
            string reportDetails = GetReportDetails();
            XmlDocument repdetails = new XmlDocument();
            repdetails.LoadXml(reportDetails);
            //Session["reportId"] = repdetails.SelectSingleNode("/ResponseMessage/ReportDetailsList/Report").Attributes["ID"].Value;

            string filterDetails = GetFilterDetails();
            XmlDocument fildetails = new XmlDocument();
            //fildetails.LoadXml(filterDetails);
            Session["filtername"] = fildetails.SelectSingleNode("/ResponseMessage/ReportFilterDetails/Filter").Attributes["Name"].Value;
            Session["datamodelId"] = fildetails.SelectSingleNode("/ResponseMessage/ReportFilterDetails/Filter").Attributes["DataModelID"].Value;

            return "Sucess";
        }
        return "Failed";
    }

    /// <summary>Based on the input param get Report ID and send back the result</summary>
    /// <param name="reportName"> </param>
    /// return the Report id in the xml format
    private string GetReportDetails()
    {
        string wsResult = string.Empty;
        try
        {
            wsResult = dasboardApi.ivnGetReportDetails(IVNXTSessionID, IVNXTCompID, reportname);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return wsResult;
    }

    private string GetFilterDetails()
    {
        string wsResult = string.Empty;
        if (Session["reportId"] != null)
        {
            reportid = int.Parse(Session["reportId"].ToString());
            //filtertype = int.Parse(Session["FilterType"].ToString());

            try
            {
                wsResult = dasboardApi.ivnGetFilterDetails(IVNXTSessionID, IVNXTCompID, reportid, 2, true);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        return wsResult;
    }

    //Set Filters if defined for the report
    public void SetFilterValues(string result)
    {
        if (Session["filtername"] == null)
            return;



        string filtername = Session["filtername"].ToString();
        int datamodelid = int.Parse(Session["datamodelId"].ToString());
        reportid = int.Parse(Session["reportId"].ToString());
        string xml = "<DashboardFilters xmlns=\"http://tempuri.org/DashboardFilters.xsd\"><Filters><DataModelID>" + datamodelid + "</DataModelID><WidgetID /><Field>" + filtername + "</Field><Condition>" + result + "</Condition><Type>CHECKFILTER</Type></Filters></DashboardFilters>";
        try
        {
            string wsResult = dasboardApi.ivnSetFilterValues(IVNXTSessionID, IVNXTCompID, reportid, xml);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //Return intelliview url.
    public string ShowReport()
    {
        string wsResult = string.Empty;
        reportid = int.Parse(Session["reportId"].ToString());
        try
        {
            wsResult = dasboardApi.ivnShowReport(IVNXTSessionID, IVNXTCompID, reportid, "", "");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return wsResult;
    }

    public Intelliview(string ivName)
    {
        if (Session["iv_userName" + ivName] != null)
        {
            username = Session["iv_userName" + ivName].ToString();
            password = Session["iv_password" + ivName].ToString();
            companyname = Session["iv_companyName" + ivName].ToString();
            reportname = Session["iv_reportName" + ivName].ToString();
            if (Session["IVNXTCompID"] == null)
            {
                //  StartSession();
            }
            else
            {
                IVNXTSessionID = Session["IVNXTSessionID"].ToString();
                IVNXTCompID = int.Parse(Session["IVNXTCompID"].ToString());
            }
        }
    }
}
