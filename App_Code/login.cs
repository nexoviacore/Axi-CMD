using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.IO;
using System.Web.Caching;
using System.Collections;
using System.Text;
using CacheMgr;
using System.Xml;
using System.Diagnostics;
using System.Data;
using System.Linq;
using Axpert_Object;
using System.Threading;
using System.Web.SessionState;
using System.Web.UI;
/// <summary>
/// Summary description for login
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class login : System.Web.Services.WebService
{

    ASBMenu.ASBMenuservice asbMenu = new ASBMenu.ASBMenuservice();
    public login()
    {

    }
    [WebMethod(EnableSession = true)]
    public string LoginSignature(string username, string password, string seed)
    {
        SessionIDManager manager = new SessionIDManager();
        string newID = manager.CreateSessionID(Context);
        string ProjectName = HttpContext.Current.Session["Project"].ToString();

        string inputxml = "<login ip='" + HttpContext.Current.Request.UserHostAddress + "' other=''  seed='" + seed + "'  axpapp='" + ProjectName + "' sessionid='" + newID + "' username='" + username + "' password='" + password + "' url='' direct='t' trace=''  macid='' licfile=''>";
        inputxml += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp></axhelp></axprops></login>";
        string result = "";
        //result = asbMenu.AppLogin(inputxml);
        if (result.StartsWith(Constants.ERROR) || result.Contains(Constants.ERROR))
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(result);
            string msg = string.Empty;
            XmlNode errorNode = xmldoc.SelectSingleNode("/error");
            if (result.Contains("\n"))
                result = result.Replace("\n", "");
            foreach (XmlNode msgNode in errorNode)
            {
                if (msgNode.Name == "msg")
                {
                    msg = msgNode.InnerText;
                    break;
                }
            }

            if (msg == string.Empty && errorNode.InnerText != string.Empty)
                msg = errorNode.InnerText;
            result = msg;
        }
        else
        {
            string scriptsPath = Application["ScriptsPath"].ToString();
            string ScriptsurlPath = Application["ScriptsurlPath"].ToString();
            int DgSignRefereshTime = Convert.ToInt32(Session["AxDgSignRefreshTime"].ToString());

            result = "Success," + scriptsPath + "," + ScriptsurlPath + "," + DgSignRefereshTime;
        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    public string GetPDFFilesForSign(string username)
    {
        string result = string.Empty;
        SessionIDManager manager1 = new SessionIDManager();
        string newID1 = manager1.CreateSessionID(Context);
        string Proj = HttpContext.Current.Session["Project"].ToString();
        string inputXML = "<sqlresultset axpapp='" + Proj + "' sessionid='" + newID1 + "' direct='true' trace='' ><sql>select * from axdsigntrans  where username ='" + username + "' and  STATUS ='1' </sql>";
        inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops></sqlresultset>";
        result = asbMenu.GetChoices(inputXML);
        return result;
    }
}
