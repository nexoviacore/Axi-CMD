using ASBExt;
using CacheMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.SessionState;

/// <summary>
/// Summary description for CloudService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class CloudService : System.Web.Services.WebService
{

    Util.Util util = new Util.Util();
    LogFile.Log logobj = new LogFile.Log();
    public CloudService()
    {
    }
    public void InitFastDataObj()
    {
        try
        {
            if (Session["FDR"] == null)
            {
                FDR fObj = new FDR();
                fObj.schemaNameKey = Session["dbuser"].ToString();
                Session["FDR"] = fObj;
            }
        }
        catch (Exception ex)
        {

        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public string HelloWorld()
    {
        return "Hello Cloud";
    }

    [WebMethod(EnableSession = true)]
    public string SaveTstructApi(string project, string AxRole, string tid, string user, string ConnString, string fldArray1, string fldDbRowNo1, string fldValueArray1, string fldDeletedArray1, string Rid)
    {
        string result = string.Empty;
        SessionIDManager Manager = new SessionIDManager();
        string sessionid = Manager.CreateSessionID(Context);
        try
        {
            ArrayList fldArray = new ArrayList(fldArray1.Split('`'));
            ArrayList fldDbRowNo = new ArrayList(fldDbRowNo1.Split('`'));
            ArrayList fldValueArray = new ArrayList(fldValueArray1.Split('`'));
            ArrayList fldDeletedArray = new ArrayList(fldDeletedArray1.Split('`'));

            fldDeletedArray.Remove("");
            string rid = Rid;
            string files = "" + tid;
            string errlog = string.Empty;
            string filename = "saveAPI-" + tid + user;
            errlog = logobj.CreateLog("Saving Data", sessionid, filename, "new");

            UpdateSessionVars(project, sessionid, user, AxRole, tid);
            GetSessionID(project, ConnString, sessionid, user, AxRole);
            util.GetDBConnection(project, HttpContext.Current.Session["axApps"].ToString());
            InitFastDataObj();
            CacheManager cacheMgr = GetCacheObject();
            TStructDef strObj = GetStrObject(cacheMgr, project, sessionid, user, tid, AxRole);
            TStructData tstData = new TStructData("", tid, rid, strObj);
            if (rid != "0")
            {
                tstData.recordid = "";
            }
            string serviceInputXml = string.Empty;

            tstData.GetFieldValueXml(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, "-1", "true", "ALL", "");
            serviceInputXml = "<Transaction axpapp=\"" + project + "\" afiles=\"\" trace=\"" + errlog + "\" sessionid=\"" + sessionid + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><data  transid=\"" + tid + "\"   recordid=\"" + rid + "\"> ";

            logobj.CreateLog("Calling Save Tsruct Api", sessionid, filename, "new");
            serviceInputXml += tstData.fldValueXml + "</data>";
            serviceInputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Session["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
            serviceInputXml += "</Transaction>";
            string ires = strObj.structRes;
            //Call service
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            DateTime asStart = DateTime.Now;
            result = objWebServiceExt.CallSaveDataWS(tid, serviceInputXml, ires);
            DateTime asEnd = DateTime.Now;
            return result;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Save Tstruct Api: " + ex.Message + "", sessionid, "SaveTstructApi", "new");
            result = "Error!" + ex.Message;
            return result;
        }
    }

    #region GetCacheObject
    private CacheManager GetCacheObject()
    {
        CacheManager cacheMgr = null;
        string errorLog = string.Empty;

        try
        {
            cacheMgr = new CacheManager(errorLog);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Redirect(util.ERRPATH + ex.Message);
        }

        if (cacheMgr == null)
            HttpContext.Current.Response.Redirect(util.ERRPATH + "Server error. Please try again later");

        return cacheMgr;
    }
    #endregion

    #region GetStrObject
    private TStructDef GetStrObject(CacheManager cacheMgr, string proj, string sid, string user, string tid, string AxRole)
    {
        TStructDef strObj = null;
        // cachemanager and TStructDef objects throw exceptions
        try
        {
            string language = HttpContext.Current.Session["language"].ToString();
            strObj = cacheMgr.GetStructDef(proj, sid, user, tid, AxRole);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in GetStrObject-" + ex.Message, sid, "GetStrObject-" + tid, "new");
            if (ex.Message == Constants.SESSIONEXPMSG)
            {

            }
            else
            {
                HttpContext.Current.Response.Redirect(util.ERRPATH + ex.Message.Replace(Environment.NewLine, ""));
            }
        }

        if (strObj == null)
            HttpContext.Current.Response.Redirect(util.ERRPATH + "Server error. Please try again later");


        return strObj;
    }
    #endregion

    [WebMethod(EnableSession = true)]
    public string GetCompanyDetails(string proj, string tid, string AxRole, string ConnString, string user, string appID)
    {
        string sessionId = "";

        string result = string.Empty;
        if (HttpContext.Current.Session != null)
            sessionId = HttpContext.Current.Session.SessionID;
        try
        {
            ConstructAxApps(proj, ConnString);
            ConstructAxProps(proj, user);

            //string sqlQuery = "select a.companyid,a.companyname,a.companyaddress,b.country_name as country,a.localcurrency,a.basecurrency,b.countryid,a.fyrsmonth from company a,country b where a.COUNTRY=b.COUNTRYID and a.username='" + user + "' and a.appid="+int.Parse(appID);
            string sqlQuery = "select a.companyid,a.companyname,a.companyaddress,b.country_name as country,a.localcurrency,a.basecurrency,b.countryid,a.fyrsmonth,a.appid from company a,country b where a.COUNTRY=b.COUNTRYID ";
            //HttpContext.Current.Session["trace"] = "true";
            string errorLog = logobj.CreateLog("Call GetChoices API", sessionId, "CallGetChoicesAPI-" + tid + "", "new");
            string inputXML = string.Empty;
            inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sessionId + "' trace='" + errorLog + "' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlQuery + "</sql>";
            inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            string glovars = GetSessionID(proj, ConnString, sessionId, user, AxRole);
            glovars = ParseGlobalVars(glovars);
            result = GetSqlData(tid, inputXML) + "`" + glovars;
            return result;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Get Company Details: " + ex.Message + "", sessionId, "GetCompanyDetails", "new");
            result = "Error!" + ex.Message;
            return result;
        }
    }

    private string ParseGlobalVars(string result)
    {
        string[] aryGloVar = result.Split('~');
        string res = string.Empty;
        foreach (var gloVar in aryGloVar)
        {
            string[] tempVar = gloVar.Split('=');
            if (tempVar[0].ToString() == "isbranchmandatory")
            {
                res = tempVar[0] + "~" + tempVar[1];
            }
        }
        return res;
    }


    [WebMethod(EnableSession = true)]
    public string GetBranchDetails(string proj, string tid, string AxRole, string ConnString, string companyid, string user, string appID)
    {
        string sessionId = "";
        string result = string.Empty;
        if (HttpContext.Current.Session != null)
            sessionId = HttpContext.Current.Session.SessionID;
        try
        {
            ConstructAxApps(proj, ConnString);
            ConstructAxProps(proj, user);

            //string sqlQuery = "select a.COMPANYID,a.COMPANYNAME,b.STATE_NAME,b.STATEID,c.BRANCHNAME,c.BRANCHID, c.BRANCHADDRESS,d.COUNTRY_NAME, d.countryid,c.LOCATIONTYPE,c.ISSTOCKLOC,c.ACTIVE  from company a,state b, branch c,country d where a.username='" + user + "' and a.companyid='" + companyid + "'  and a.companyid=c.companyid and c.country=d.countryid and c.state=b.stateid  and c.appid=" + int.Parse(appID);
            string sqlQuery = "select a.COMPANYID,a.COMPANYNAME,b.STATE_NAME,b.STATEID,c.BRANCHNAME,c.BRANCHID, c.BRANCHADDRESS,d.COUNTRY_NAME, d.countryid,c.LOCATIONTYPE,c.ISSTOCKLOC,c.ACTIVE  from company a,state b, branch c,country d where a.companyid='" + companyid + "'  and a.companyid=c.companyid and c.country=d.countryid and c.state=b.stateid(+) ";
            //HttpContext.Current.Session["trace"] = "true";
            string errorLog = logobj.CreateLog("Call GetChoices API", sessionId, "CallGetChoicesAPI-" + tid + "", "new");
            string inputXML = string.Empty;
            inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sessionId + "' trace='" + errorLog + "' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlQuery + "</sql>";
            inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            result = GetSqlData(tid, inputXML);
            return result;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Get Branch Details: " + ex.Message + "", sessionId, "GetBranchDetails", "new");
            result = "Error!" + ex.Message;
            return result;
        }
    }

    public string GetSqlData(string tid, string inputXML)
    {
        string result = string.Empty;
        try
        {
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            result = objWebServiceExt.CallGetChoiceWebService(tid, inputXML);
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Get Sql Data: " + ex.Message + "", tid, "GetSqlData", "new");
            result = "Error!" + ex.Message;
            return result;
        }
        return result;
    }

    public string GetSessionID(string projName, string ConnString, string sessionId, string user, string AxRole)
    {
        string ixml = string.Empty;
        string errorLog = logobj.CreateLog("Calling Get New global variables", sessionId, "GetNewGloVars-Cloud", "new", "true");
        ixml += "<root axpapp=\"" + projName + "\" sessionid=\"" + sessionId + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' user=\"" + user + "\" password=\"\" sso=\"true\" url=\"\" direct=\"t\" trace=\"" + errorLog + "\"  inssession=\"true\" userroles=\"" + AxRole + "\">";
        ConstructAxApps(projName, ConnString);
        ConstructAxProps(projName, user);
        ixml += HttpContext.Current.Session["axApps"];
        ixml += HttpContext.Current.Session["axProps"];
        ixml += HttpContext.Current.Session["axGlobalVars"];
        ixml += HttpContext.Current.Session["axUserVars"];
        ixml += "</root>";
        string response = string.Empty;
        try
        {
            WebServiceExt webServiceExt = new WebServiceExt();
            response = webServiceExt.CallGetNewGlobalVarsWS("empdt", ixml);    //response Format:Done#~axp_imagepath=D: \Program files\Axpert\img\~axp_gridattach=D:\Program files\Axpert\img\~axp_testexpr=0~responsibilies=testing~rolename=~ax_evalcopy=F^
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Get SessionID: " + ex.Message + "", sessionId, "GetSessionID", "new");
            response = "Error!" + ex.Message;
            return response;
        }
        return response;
    }

    public string ConstructAxApps(string projName, string connString)
    {
        string axapps = string.Empty;
        axapps += "<" + projName + ">";
        axapps += "<type>db</type>";
        axapps += "<structurl></structurl>";
        axapps += "<db>Oracle</db>";
        axapps += "<driver>dbx</driver>";
        axapps += "<dbcon>" + connString + "</dbcon>";
        axapps += "<dbuser>" + projName + "</dbuser>";
        axapps += "<pwd></pwd>";
        axapps += "</" + projName + ">";
        HttpContext.Current.Session["axApps"] = axapps.ToString();
        return axapps;
    }

    public string ConstructAxProps(string projName, string user)
    {
        string axProps = "<axprops getfrom=\"\" setto=\"\">";
        axProps += "<lastlogin>" + projName + "</lastlogin>";
        axProps += "<oradatestring>dd/mm/yyyy hh24:mi:ss</oradatestring>";
        axProps += "<crlocation></crlocation>";
        axProps += "<lastusername>" + user + "</lastusername>";
        axProps += "<login>local</login>";
        axProps += "<skin>Black</skin>";
        axProps += "<axhelp>true</axhelp>";
        axProps += "</axprops>";
        HttpContext.Current.Session["axProps"] = axProps.ToString();
        return axProps;
    }

    private void UpdateSessionVars(string projName, string sid, string user, string AxRole, string tid)
    {
        HttpContext.Current.Session["project"] = projName;
        //HttpContext.Current.Session["proj"] = projName;
        //HttpContext.Current.Session["sid"] = sid;
        HttpContext.Current.Session["nsessionid"] = sid;
        HttpContext.Current.Session["language"] = "ENGLISH";
        HttpContext.Current.Session["validated"] = "true";
        HttpContext.Current.Session["AxEnableOldTheme"] = "false";
        HttpContext.Current.Session["user"] = user;
        HttpContext.Current.Session["AxRole"] = AxRole;
        HttpContext.Current.Session["transid"] = tid;

    }

    [WebMethod(EnableSession = true)]
    public string DeleteTstructApi(string project, string AxRole, string tid, string user, string ConnString, string Rid)
    {
        string result = string.Empty;
        SessionIDManager Manager = new SessionIDManager();
        string sessionid = Manager.CreateSessionID(Context);
        try
        {
            string files = "" + tid;
            string errlog = string.Empty;
            DateTime sTime1 = DateTime.Now;
            UpdateSessionVars(project, sessionid, user, AxRole, tid);
            GetSessionID(project, ConnString, sessionid, user, AxRole);
            InitFastDataObj();
            CacheManager cacheMgr = GetCacheObject();
            TStructDef strObj = GetStrObject(cacheMgr, project, sessionid, user, tid, AxRole);
            TStructData tstData = new TStructData("", tid, Rid, strObj);
            if (!util.IsNumber(Rid))
                return "{\"error\":[{\"msg\":\"Error: Invalid parameter value.\"}]}";

            if (Rid == "0")
            {
                return "{\"error\":[{\"msg\":\"Error: New Record Cannot Be deleted.\"}]}";
            }
            else if (strObj.tstCancelled.ToLower() == "true")
            {
                return "{\"error\":[{\"msg\":\"Error: Cancelled record cannot be deleted.\"}]}";
            }
            string xml = "<Transaction axpapp=\"" + project + "\" transid=\"" + tid + "\" appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' recordid=\"" + Rid + "\" action=\"delete\" trace=\"♦♦" + files + "♦\" sessionid=\"" + sessionid + "\">";
            xml = xml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</Transaction>";

            DateTime eTime1 = DateTime.Now;
            result = tstData.CallDeleteDataWS(xml);
            DateTime sTime2 = DateTime.Now;
            if (util.IsFileInCache(tstData.transid, Rid))
                util.DeleteFromCache(result, Rid, tstData.transid);
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));
            return result;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Delete Tstruct Api: " + ex.Message + "", sessionid, "DeleteTstructApi", "new");
            result = "Error!" + ex.Message;
            return result;
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetUserDetails(string proj, string tid, string AxRole, string ConnString, string user, string Appid)
    {
        string result = string.Empty;
        string sessionId = "";
        if (HttpContext.Current.Session != null)
            sessionId = HttpContext.Current.Session.SessionID;
        try
        {
            ConstructAxApps(proj, ConnString);
            ConstructAxProps(proj, user);
            //string sqlQuery = "select distinct a.axusersid as axp_recid1,a.username as axuser,a.email,a.nickname,r.axusergroupg from axusers a,(SELECT axusersid,wm_concat(r.axusergroup) as axusergroupg FROM  axuserlevelgroups r GROUP BY axusersid)r where r.axusersid=a.axusersid order by a.username asc";
            string sqlQuery = "select distinct a.axusersid as axp_recid1,a.username as axuser,a.email,a.nickname,r.axusergroupg,a.actflag from axusers a,(SELECT axusersid,wm_concat(r.axusergroup) as axusergroupg FROM  axuserlevelgroups r GROUP BY axusersid)r where r.axusersid=a.axusersid and a.appid=" + int.Parse(Appid) + "  order by a.username asc";
            //HttpContext.Current.Session["trace"] = "true";
            string errorLog = logobj.CreateLog("Call GetChoices API", sessionId, "CallGetChoicesAPI-" + tid + "", "new");
            string inputXML = string.Empty;
            inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sessionId + "' trace='" + errorLog + "' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlQuery + "</sql>";
            inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            result = GetSqlData(tid, inputXML);
            return result;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Get User Details: " + ex.Message + "", sessionId, "GetUserDetails", "new");
            result = "Error!" + ex.Message;
            return result;
        }
    }

    [WebMethod(EnableSession = true)]
    public DataSet GetSingleUserDetails(string proj, string tid, string AxRole, string ConnString, string user, string rid)
    {
        string result = string.Empty;
        SessionIDManager Manager = new SessionIDManager();
        string sessionid = Manager.CreateSessionID(Context);
        DataSet ds = new DataSet();
        try
        {
            string filename = "saveAPI-" + tid;
            UpdateSessionVars(proj, sessionid, user, AxRole, tid);
            GetSessionID(proj, ConnString, sessionid, user, AxRole);
            util.GetDBConnection(proj, HttpContext.Current.Session["axApps"].ToString());
            InitFastDataObj();
            CacheManager cacheMgr = GetCacheObject();
            TStructDef strObj = GetStrObject(cacheMgr, proj, sessionid, user, tid, AxRole);
            string visibleDCs = string.Empty;
            visibleDCs = strObj.GetVisibleDCs();
            string loadXml = string.Empty;
            string errorLog = logobj.CreateLog("Loading Structure.", sessionid, filename, "new");
            loadXml = loadXml + "<root axpapp='" + proj + "' sessionid='" + sessionid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' transid='" + tid + "' recordid='" + rid + "' dcname='' trace='" + errorLog + "'>";
            loadXml += Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString();
            loadXml += "</root>";
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            result = objWebServiceExt.CallLoadDataWS(tid, loadXml, strObj.structRes, rid, proj);
            result = result.Trim();
            result = result.Replace("\\n", "");
            result = result.Replace("\\", ";bkslh");

            TStructData strDataObj = new TStructData(result, tid, rid, strObj);

            ds.Tables.Add("DC1");
            ds.Tables["DC1"].Merge(strDataObj.dsDataSet.Tables["DC1"]);
            ds.Tables.Add("DC2");
            ds.Tables["DC2"].Merge(strDataObj.dsDataSet.Tables["DC2"]);
            ds.Tables.Add("DC3");
            if (strDataObj.dsDataSet.Tables["DC3"].Rows[0]["axp_recid3"].ToString() != "")
                ds.Tables["DC3"].Merge(strDataObj.dsDataSet.Tables["DC3"]);
            else
                ds.Tables["DC3"].Merge(strDataObj.dsDataSet.Tables["DC3"].Clone());
            return ds;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Get Single User Details: " + ex.Message + "", sessionid, "GetSingleUserDetails", "new");
            return ds;
        }
    }

    [WebMethod(EnableSession = true)]
    public DataSet GetUserDetailsCB(string proj, string tid, string AxRole, string ConnString, string user)
    {
        string sessionId = "";
        string result = string.Empty;
        if (HttpContext.Current.Session != null)
            sessionId = HttpContext.Current.Session.SessionID;
        DataSet ds = new DataSet();
        try
        {
            ConstructAxApps(proj, ConnString);
            ConstructAxProps(proj, user);

            string sqlQuery = "select c.companyid,c.companyname,b.branchid,b.branchname,b.companyid as brncompanyid from company c LEFT OUTER JOIN branch b on c.companyid=b.companyid";
            //HttpContext.Current.Session["trace"] = "true";
            string errorLog = logobj.CreateLog("Call GetChoices API", sessionId, "CallGetChoicesAPI-" + tid + "", "new");
            string inputXML = string.Empty;
            inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sessionId + "' trace='" + errorLog + "' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlQuery + "</sql>";
            inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            result = GetSqlData(tid, inputXML);

            StringReader stringReader = new StringReader(result);
            ds.ReadXml(stringReader);
            var dtcom = ds.Tables["row"].AsEnumerable().Select(r => new
            {
                COMPANYID = Convert.ToInt64(r.Field<string>("COMPANYID")),
                COMPANYNAME = r.Field<string>("COMPANYNAME")
            }).Distinct().ToList();

            ds.Tables.Add("company");
            ds.Tables["company"].Merge(dtConvert.ToDataTable(dtcom));
            return ds;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Get User Details Company & Branch: " + ex.Message + "", sessionId, "GetUserDetailsCB", "new");
            result = "Error!" + ex.Message;
            return ds;
        }
    }

    [WebMethod(EnableSession = true)]
    public string DeleteTstructApiUser(string project, string AxRole, string tid, string user, string ConnString, string Rid)
    {
        string result = string.Empty;
        SessionIDManager Manager = new SessionIDManager();
        string sessionid = Manager.CreateSessionID(Context);
        try
        {
            string files = "" + tid;
            DateTime sTime1 = DateTime.Now;
            UpdateSessionVars(project, sessionid, user, AxRole, tid);
            GetSessionID(project, ConnString, sessionid, user, AxRole);
            InitFastDataObj();
            CacheManager cacheMgr = GetCacheObject();
            TStructDef strObj = GetStrObject(cacheMgr, project, sessionid, user, tid, AxRole);
            TStructData tstData = new TStructData("", tid, Rid, strObj);
            if (!util.IsNumber(Rid))
                return "{\"error\":[{\"msg\":\"Error: Invalid parameter value.\"}]}";

            if (Rid == "0")
            {
                return "{\"error\":[{\"msg\":\"Error: New Record Cannot Be deleted.\"}]}";
            }
            else if (strObj.tstCancelled.ToLower() == "true")
            {
                return "{\"error\":[{\"msg\":\"Error: Cancelled record cannot be deleted.\"}]}";
            }
            string xml = "<Transaction axpapp=\"" + project + "\" transid=\"" + tid + "\" appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' recordid=\"" + Rid + "\" action=\"delete\" trace=\"♦♦" + files + "♦\" sessionid=\"" + sessionid + "\">";
            xml = xml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</Transaction>";

            DateTime eTime1 = DateTime.Now;
            result = tstData.CallDeleteDataWS(xml);
            DateTime sTime2 = DateTime.Now;
            if (util.IsFileInCache(tstData.transid, Rid))
                util.DeleteFromCache(result, Rid, tstData.transid);
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));
            return result;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Delete Tstruct Api User: " + ex.Message + "", sessionid, "DeleteTstructApiUser", "new");
            result = "Error!" + ex.Message;
            return result;
        }
    }

    [WebMethod(EnableSession = true)]
    public string SaveTstructApiUser(string project, string AxRole, string tid, string user, string ConnString,
        string fldArray1, string fldDbRowNo1, string fldValueArray1, string fldDeletedArray1, string Rid, string delRows)
    {
        string result = string.Empty;
        SessionIDManager Manager = new SessionIDManager();
        string sessionid = Manager.CreateSessionID(Context);
        string serviceInputXml = string.Empty;
        try
        {
            ArrayList fldArray = new ArrayList(fldArray1.Split('`'));
            ArrayList fldDbRowNo = new ArrayList(fldDbRowNo1.Split('`'));
            ArrayList fldValueArray = new ArrayList(fldValueArray1.Split('`'));
            ArrayList fldDeletedArray = new ArrayList(fldDeletedArray1.Split('`'));
            fldDeletedArray.Remove("");
            string errlog = string.Empty;
            string filename = "saveAPI-" + tid;

            errlog = logobj.CreateLog("Saving Data", sessionid, filename, "new");
            UpdateSessionVars(project, sessionid, user, AxRole, tid);
            GetSessionID(project, ConnString, sessionid, user, AxRole);
            util.GetDBConnection(project, HttpContext.Current.Session["axApps"].ToString());
            InitFastDataObj();
            CacheManager cacheMgr = GetCacheObject();
            TStructDef strObj = GetStrObject(cacheMgr, project, sessionid, user, tid, AxRole);
            TStructData tstData = new TStructData("", tid, Rid, strObj);

            tstData.GetFieldValueXml(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, "-1", "true", "ALL", "");
            serviceInputXml = "<Transaction axpapp=\"" + project + "\" afiles=\"\" trace=\"" + errlog + "\" sessionid=\"" + sessionid + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><data  transid=\"" + tid + "\"   recordid=\"" + Rid + "\"> ";

            logobj.CreateLog("Calling Save Tsruct Api", sessionid, filename, "");
            serviceInputXml += tstData.fldValueXml + "</data>";

            if (Rid != "0")
            {
                serviceInputXml += delRows;
                serviceInputXml += "<changedrows><dc2>*</dc2><dc3>*</dc3></changedrows>";
            }

            serviceInputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Session["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
            serviceInputXml += "</Transaction>";
            string ires = strObj.structRes;
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            DateTime asStart = DateTime.Now;
            result = objWebServiceExt.CallSaveDataWS(tid, serviceInputXml, ires);
            return result;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Save Tstruct Api User: " + ex.Message + "", sessionid, "SaveTstructApiUser", "new");
            logobj.CreateLog("Input xml: " + serviceInputXml + "", sessionid, "SaveTstructApiUser", "");
            logobj.CreateLog("stack trace: " + ex.StackTrace + "", sessionid, "SaveTstructApiUser", "");
            result = "Error!" + ex.Message;
            return result;
        }
    }
    [WebMethod(EnableSession = true)]
    public string GetUserRoles(string proj, string tid, string ConnString, string user)
    {
        string sessionId = "";
        string result = string.Empty;
        if (HttpContext.Current.Session != null)
            sessionId = HttpContext.Current.Session.SessionID;
        DataSet ds = new DataSet();
        try
        {
            ConstructAxApps(proj, ConnString);
            ConstructAxProps(proj, user);

            string sqlQuery = "select groupno,groupname,userroles,roledesc from axusergroups";
            //HttpContext.Current.Session["trace"] = "true";
            string errorLog = logobj.CreateLog("Call GetChoices API", sessionId, "CallGetChoicesAPI-" + tid + "", "new");
            string inputXML = string.Empty;
            inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + sessionId + "' trace='" + errorLog + "' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlQuery + "</sql>";
            inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            result = GetSqlData(tid, inputXML);
            return result;
        }
        catch (Exception ex)
        {
            logobj.CreateLog("Exception in Get User Roles: " + ex.Message + "", sessionId, "GetUserRoles", "new");
            result = "Error!" + ex.Message;
            return result;
        }
    }
}

public static class dtConvert
{
    public static DataTable ToDataTable<T>(this IList<T> data)
    {
        PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        for (int i = 0; i < props.Count; i++)
        {
            PropertyDescriptor prop = props[i];
            table.Columns.Add(prop.Name, prop.PropertyType);
        }
        object[] values = new object[props.Count];
        foreach (T item in data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = props[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        return table;
    }
}
