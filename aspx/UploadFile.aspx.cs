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
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    Util.Util util = new Util.Util();
    protected void Page_Load(object sender, EventArgs e)
    {
        util.IsValidSession();
        foreach (string f in Request.Files.AllKeys)
        {
            HttpPostedFile file = Request.Files[f];
            string Proj = HttpContext.Current.Session["Project"].ToString();
            SessionIDManager manager1 = new SessionIDManager();
            string newID1 = manager1.CreateSessionID(Context);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string query = string.Empty;
            string result = string.Empty;
            string nextuser = string.Empty;
            string currentUserPath = string.Empty;

            string destinationpath = Request.QueryString["dpath"].ToString();
            string TransID = Request.QueryString["transid"].ToString();
            string OrderNo = Request.QueryString["orderno"].ToString();
            string Username = Request.QueryString["username"].ToString();
            string filename = Request.QueryString["filemname"].ToString();

            string inputXML = "<sqlresultset axpapp='" + Proj + "' sessionid='" + newID1 + "' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace='' ><sql>select USERNAME  from axdsigntrans where STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO ='" + (Convert.ToInt32(OrderNo) + 1).ToString() + "'</sql>";
            inputXML += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops>";
            inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            result = objWebServiceExt.CallGetChoiceWebService("", inputXML);
            if (result.Contains("<error>") && result.Contains(Constants.SESSIONEXPMSG))
            {
                Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG);
                return;
            }
            else if (result != string.Empty)
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
                            if (viewNode.Name.ToLower() == "username")
                                nextuser = viewNode.InnerText;
                        }
                    }
                }
                else
                {
                    nextuser = "final";
                }
            }
            currentUserPath = destinationpath + "\\" + Username;
            destinationpath = destinationpath + "\\" + nextuser;
            try
            {

                //delete orginal doc and add sign documnet to user folder
                File.Delete(currentUserPath + "\\" + file.FileName);
                file.SaveAs(currentUserPath + "\\" + file.FileName);
                //
                // If the directory doesn't exist, create it.
                if (!Directory.Exists(destinationpath))
                {
                    Directory.CreateDirectory(destinationpath);
                }
                file.SaveAs(destinationpath + "\\" + file.FileName);

                if (nextuser == "final")
                {

                    UpdatestatusLastuser(TransID, OrderNo, filename, newID1, Proj, result);
                }
                else
                {

                    UpdatestatusLastuser(TransID, OrderNo, filename, newID1, Proj, result);
                    UpdatestatusNextuser(TransID, OrderNo, filename, newID1, Proj, result);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == Constants.SESSIONEXPMSG)
                {
                    SessionExpired();
                    return;
                }
                else
                {
                    Response.Redirect(util.ERRPATH + ex.Message.Replace(Environment.NewLine, ""));
                }
            }
        }
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

    private void UpdatestatusNextuser(string TransID, string OrderNo, string filename, string newID1, string Proj, string result)
    {
        string inputXML2 = "<sqlresultset axpapp='" + Proj + "' sessionid='" + newID1 + "' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace='' ><sql>update axdsigntrans set STATUS=1 where STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO =" + (Convert.ToInt32(OrderNo) + 1) + "</sql>";
        inputXML2 += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops>";
        inputXML2 += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        result = objWebServiceExt.CallGetChoiceWebService(TransID, inputXML2);
        if (result.Contains(Constants.ERROR) == true)
        {
            result = result.Replace(Constants.ERROR, "");
            result = result.Replace("</error>", "");
            result = result.Replace("\n", "");
            throw (new Exception(result));
        }
    }

    private void UpdatestatusLastuser(string TransID, string OrderNo, string filename, string newID1, string Proj, string result)
    {
        string inputXML1 = "<sqlresultset axpapp='" + Proj + "' sessionid='" + newID1 + "' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace='' ><sql>update axdsigntrans set STATUS=2 where  STRANSID='" + TransID + "' AND DOCUMENTNAME='" + filename + "'AND ORDNO ='" + OrderNo + "'</sql>";
        inputXML1 += "<axprops><skin></skin><login></login><lastlogin></lastlogin><lastusername></lastusername><licenseurl></licenseurl><axhelp>false</axhelp></axprops>";
        inputXML1 += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        result = objWebServiceExt.CallGetChoiceWebService(TransID, inputXML1);
    }
}
