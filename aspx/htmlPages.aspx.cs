using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;
using System.Configuration;
using System.Net;
using System.Xml;
using System.Collections;

public partial class aspx_htmlPages : System.Web.UI.Page
{
    public string direction = "ltr";
    public string langType = "en";
    Util.Util util;
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
        Util.Util util = new Util.Util();
        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }
        else if (!util.CheckValidLogin())
        {
            SessionExpired();
            return;
        }

        string pageId = string.Empty;
        string pageCaption = string.Empty;

        string path = string.Empty;
        string htmlFileName = string.Empty;
        string axpertWebUrl = string.Empty;
        string projectName = HttpContext.Current.Session["Project"].ToString();
        string schemaName = HttpContext.Current.Session["dbuser"].ToString();
        string sessionId = HttpContext.Current.Session.SessionID;
        string errorLog = string.Empty;

        string dbType = string.Empty;
        if (!string.IsNullOrEmpty(HttpContext.Current.Session["axdb"].ToString()))
            dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();

        LogFile.Log logobj = new LogFile.Log();
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();

        if (Request.QueryString != null && Request.QueryString["load"] != null)
        {
            pageId = Request.QueryString["load"].ToString();
        }

        if (Request.QueryString != null && Request.QueryString["loadcaption"] != null)
        {
            pageCaption = Request.QueryString["loadcaption"].ToString();
        }

        if (pageId != string.Empty)
        {
            string sqlResult = string.Empty;
            string sqlQuery = string.Empty;
            string userName = HttpContext.Current.Session["username"].ToString();

            errorLog = logobj.CreateLog("CallGetFileNames - Call Get HTML File Name Get Choices", sessionId, "HTMLPage", "new", "true");
            string inputXML = "<sqlresultset axpapp='" + projectName + "' sessionid='" + sessionId + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + userName + "' ><sql>";
            if (dbType.ToLower() == "oracle")
            {
                sqlQuery = "select replace(caption, ' ', '_') || '_' || replace(name, 'HP', '') || '.html' as htmlfilename from axpages where name='HP" + pageId + "'";
            }
            else // DbType is "ms sql" OR "postgresql" OR "postgre" OR "mariadb" OR "mysql"
            {
                sqlQuery = "select concat(replace(caption, ' ', '_'), '_', replace(name, 'HP', ''), '.html') as htmlfilename from axpages where name='HP" + pageId + "'";
            }
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            inputXML += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            logobj.CreateLog("Call Get HTML File Name Get Choices WS" + inputXML, sessionId, "CallGetFileName-HTMLPages-Ws", "");
            sqlResult = asbExt.CallGetChoiceWS("", inputXML);

            if (sqlResult == string.Empty || (sqlResult.StartsWith("<error>")) || (sqlResult.Contains("error")))
            {
                Response.Redirect("err.aspx?errmsg=" + util.ParseXmlErrorMsgNode(sqlResult));
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sqlResult);
                XmlNodeList xml = doc.SelectNodes("//row//HTMLFILENAME | //row//htmlfilename");

                string result = string.Empty;
                if (xml.Count > 0)
                {
                    result = xml[0].InnerText;
                }

                if (result.EndsWith(".html"))
                {
                    htmlFileName = result;
                }
            }
        }
        else if (pageCaption != string.Empty)
        {
            string sqlResult = string.Empty;
            string sqlQuery = string.Empty;
            string userName = HttpContext.Current.Session["username"].ToString();

            errorLog = logobj.CreateLog("CallGetFileNames - Call Get HTML File Name Get Choices", sessionId, "HTMLPage", "new", "true");
            string inputXML = "<sqlresultset axpapp='" + projectName + "' sessionid='" + sessionId + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + userName + "' ><sql>";
            if (dbType.ToLower() == "oracle")
            {
                sqlQuery = "select replace(caption, ' ', '_') || '_' || replace(name, 'HP', '') || '.html' as htmlfilename from axpages where lower(caption) = lower('" + pageCaption + "') and name like 'HP%'";
            }
            else // DbType is "ms sql" OR "postgresql" OR "postgre" OR "mariadb" OR "mysql"
            {
                sqlQuery = "select concat(replace(caption, ' ', '_'), '_', replace(name, 'HP', ''), '.html') as htmlfilename from axpages where lower(caption) = lower('" + pageCaption + "') and name like 'HP%'";
            }
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            inputXML += sqlQuery + " </sql>" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            logobj.CreateLog("Call Get HTML File Name Get Choices WS" + inputXML, sessionId, "CallGetFileName-HTMLPages-Ws", "");
            sqlResult = asbExt.CallGetChoiceWS("", inputXML);

            if (sqlResult == string.Empty || (sqlResult.StartsWith("<error>")) || (sqlResult.Contains("error")))
            {
                Response.Redirect("err.aspx?errmsg=" + util.ParseXmlErrorMsgNode(sqlResult));
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sqlResult);
                XmlNodeList xml = doc.SelectNodes("//row//HTMLFILENAME | //row//htmlfilename");

                string result = string.Empty;
                if (xml.Count > 0)
                {
                    result = xml[0].InnerText;
                }

                if (result.EndsWith(".html"))
                {
                    htmlFileName = result;
                }
            }
        }
        else
        {
            errorLog = logobj.CreateLog("CallGetFileNames - Call Get HTML Page's received information doesn't exist.", sessionId, "HTMLPage", "", "true");
            Response.Redirect("err.aspx?errmsg=HTML Page's received information doesn't exist.");
        }

        if (htmlFileName != string.Empty)
        {
            axpertWebUrl = "";// Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToLower().IndexOf("/aspx/") + 1);
            try
            {
                string extraParams = string.Empty;
                try
                {
                    extraParams = Request.Url.Query.Substring(Request.Url.Query.IndexOf("&"));

                    ArrayList targetList = new ArrayList();
                    string[] qs = extraParams.Split('&');
                    foreach (string param in qs)
                    {
                        if (param != string.Empty)
                        {
                            string[] paramList = param.Split('=');
                            if (paramList.Length == 2 && paramList[0] != string.Empty && paramList[0] != "hltype" && paramList[0] != "hdnbElapsTime")
                            {
                                targetList.Add(param);
                            }
                        }
                    }

                    extraParams = String.Join("&", targetList.ToArray());

                    if (extraParams != string.Empty)
                    {
                        extraParams = "&" + extraParams;
                    }
                }
                catch (Exception ex) { }

                //path = axpertWebUrl + projectName + "/HTMLPages/" + htmlFileName + "?v=" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "&load=" + pageId + extraParams;
                path = axpertWebUrl + projectName + "/HTMLPages/" + htmlFileName + "?v=" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "&load=" + pageId + extraParams;


                //HttpWebRequest request = WebRequest.Create(path) as HttpWebRequest;
                ////request.Method = "HEAD";
                //HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //HttpStatusCode status = response.StatusCode;

                //if (status.ToString() == "OK")
                FileInfo _htmlFile = new FileInfo(HttpContext.Current.Server.MapPath("~/" + projectName + "/HTMLPages/" + htmlFileName));
                if (_htmlFile.Exists)
                {
                    if (path != string.Empty)
                    {
                        //set params in redis
                        try
                        {
                            string userName = HttpContext.Current.Session["username"].ToString();

                            FDW fdwObj = new FDW();

                            fdwObj.ClearRedisServerDataByKey(util.GetRedisServerkey(Constants.HTMLPAGESQUERY, pageId, userName), "", false, schemaName);

                            if (extraParams != string.Empty)
                            {
                                fdwObj.SaveInRedisServer(util.GetRedisServerkey(Constants.HTMLPAGESQUERY, pageId, userName), extraParams, Constants.HTMLPAGESQUERY, schemaName);
                            }
                        }
                        catch (Exception ex)
                        { }

                        //redirect to file
                        try
                        {
                            Response.Write(@"<script language='javascript'> document.location.href='../" + path + "'; window.parent.callParentNew('closeFrame()','function');</script>");
                        }
                        catch (Exception ex)
                        {
                            errorLog = logobj.CreateLog("CallGetFileNames - Call Get HTML File loading exception for available file => " + ex.Message, sessionId, "HTMLPage", "", "true");
                            Response.Redirect("err.aspx?errmsg=Exception while loading this page.");
                        }
                    }
                }
                else
                {
                    errorLog = logobj.CreateLog("CallGetFileNames - Call Get HTML File is not found in the given path => " + path, sessionId, "HTMLPage", "", "true");
                    Response.Redirect("err.aspx?errmsg=File doesn't exist in the path.");
                }
            }
            catch (Exception ex)
            {
                errorLog = logobj.CreateLog("CallGetFileNames - Call Get HTML File loading exception => " + ex.Message, sessionId, "HTMLPage", "", "true");
                Response.Redirect("err.aspx?errmsg=Exception while loading this page.");
            }
        }
        else
        {
            errorLog = logobj.CreateLog("CallGetFileNames - Call Get HTML File name doesn't exist.", sessionId, "HTMLPage", "", "true");
            Response.Redirect("err.aspx?errmsg=File name doesn't exist");
        }
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }
}
