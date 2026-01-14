using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml;
/// <summary>
/// Summary description for NewLogin
/// </summary>
[Serializable()]
public class LoginHelper
{
    public string axApps { get; set; }
    public string axProps { get; set; }
    public string AxCloudDb { get; set; }
    public string ipaddress { get; set; }
    public string rnd_key { get; set; }
    public string userDetails { get; set; }
    public string proj { get; set; }
    public string sid { get; set; }
    public string user { get; set; }
    public string isMobile { get; set; }
    public string timeZone { get; set; }
    public string hybridGUID { get; set; }
    public string hybridDeviceId { get; set; }
    public string hybridDefaultPage { get; set; }
    public string staySignedId { get; set; }
    public string privateSsoToken { get; set; }
    public string SsoName { get; set; }
    public string lic_redis { get; set; }
    public string lastOpenPage { get; set; }
    public string loggedBroserId { get; set; }
    public string diffTime { get; set; }
    public string oldsid { get; set; }
    public string SSOType { get; set; }

    public string clientLocale { get; set; }

    public string otpauth { get; set; }
    public string otpAuthCahrs { get; set; }
    public string otpAuthExpiry { get; set; }
    public string otpauthlogin { get; set; }
    public bool isSSO
    {
        get { return IsSSO; }
        set
        {
            IsSSO = value;
            if (isSSO)
            {
                privateSsoToken = "";
                SsoName = "";
            }
            else
            {
                privateSsoToken = string.Empty;
                SsoName = string.Empty;
            }
        }
    }

    private string Password;




    public string password
    {
        get { return Password; }
        set
        {
            Password = value;
            if (!string.IsNullOrEmpty(Password))
            {
                Random rand = new Random();
                rnd_key = rand.Next(1000, 9999).ToString();

                HashedPassword = MD5Hash(rnd_key + MD5Hash(Password));
            }
        }
    }

    public static string MD5Hash(string text)
    {
        MD5 md5 = new MD5CryptoServiceProvider();

        //compute hash from the bytes of text  
        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

        //get hash result after compute it  
        byte[] result = md5.Hash;

        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < result.Length; i++)
        {
            //change it into 2 hexadecimal digits  
            //for each byte  
            strBuilder.Append(result[i].ToString("x2"));
        }

        return strBuilder.ToString();
    }

    private string HashedPassword { get; set; }
    public string pwd { get; set; }
    public string errlog { get; set; }
    public string lang_attr { get; set; }
    public string licDetails { get; set; }
    public string result { get; set; }
    public string loginTrace { get; set; }
    private string loginXml { get; set; }
    public string selectedLanguage { get; set; }
    private bool IsSSO;
    public string ExecTraceOn { get; set; }

    public Dictionary<string, string> sessions = new Dictionary<string, string>();


    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    public LoginHelper()
    {

    }

    public LoginHelper(string projectName, string browserDetails)
    {

        proj = projectName;
        util.GetAxApps(projectName);
        axApps = HttpContext.Current.Session["axApps"].ToString();
        userDetails = browserDetails;
        if (HttpContext.Current.Session["AxCloudDB"] != null)
            AxCloudDb = HttpContext.Current.Session["AxCloudDB"].ToString();
        ipaddress = util.GetIpAddress();

        axProps = HttpContext.Current.Application["axProps"].ToString();
        oldsid = HttpContext.Current.Session.SessionID;
        System.Web.Configuration.SessionStateSection sessionStateSection = (System.Web.Configuration.SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
        string cookieName = sessionStateSection.CookieName;

        HttpCookie mycookie = new HttpCookie(cookieName);
        mycookie.Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Response.Cookies.Add(mycookie);
        System.Web.HttpContext.Current.Session.Abandon();

        SessionIDManager manager = new SessionIDManager();
        manager.RemoveSessionID(System.Web.HttpContext.Current);
        var newId = manager.CreateSessionID(System.Web.HttpContext.Current);
        var isRedirected = true;
        var isAdded = true;
        manager.SaveSessionID(System.Web.HttpContext.Current, newId, out isRedirected, out isAdded);
        sid = newId;
        loginTrace = ConfigurationManager.AppSettings["LoginTrace"].ToString();
    }

    private void GetLoginXml()
    {
        if (loginTrace.ToLower() == "true")
            errlog = logobj.CreateLog("Call to Login Web Service", sid, "login", "", "true");
        else
            errlog = logobj.CreateLog("Call to Login Web Service", sid, "login", "");
        if (selectedLanguage == string.Empty)
            selectedLanguage = "English";

        string lang_at = "";
        if (selectedLanguage != null && selectedLanguage.ToUpper() != "ENGLISH")
            lang_at = " lang=\"" + selectedLanguage + "\"";
        string scriptsPath = "";
        if (ConfigurationManager.AppSettings["scriptsUrlPath"] != null)
            scriptsPath = ConfigurationManager.AppSettings["scriptsUrlPath"].ToString();
        string isInternalSSO = string.Empty;
        if (SsoName != "" && SsoName == "InternalSSO")
        {
            isInternalSSO = " internalsso=\"yes\" appsessionkey=\"" + privateSsoToken + "\" ";
            privateSsoToken = "";
            SsoName = "";
            isSSO = false;
        }
        string _otpauth = string.Empty;
        StringBuilder strContent = new StringBuilder();
        if (otpauth == "true")
        {
            _otpauth = " otpauth='t' ";

            //string _otp = util.GenerateOTPAuth(proj, user, otpAuthCahrs, otpAuthExpiry);
            string _otp = string.Empty;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            for (int i = 0; i < int.Parse(otpAuthCahrs); i++)
            {
                //_otp += random.Next(0, 10); // Generates a random digit (0-9)
                _otp += chars[random.Next(chars.Length)];
            }
            HttpContext.Current.Session["_otpAuth"] = _otp;
            strContent.Append("<usermailid></usermailid><subject>Axpert OTP Authentication</subject><body>");
            strContent.Append("<l0>Dear " + user + ",</l0>");
            strContent.Append("<l01>" + proj + "</l01>");
            strContent.Append("<l1>One Time Password(OTP) for Axpert login is " + _otp + "</l1>");
            //strContent.Append("<l2></l2>");
            strContent.Append("<l3>Regards</l3><l4>Support Team.</l4></body>");
        }
        else
        {
            _otpauth = " otpauth='f' ";
        }

        if (isSSO)
        {
            if (rnd_key == string.Empty)
            {
                Random rand = new Random();
                rnd_key = rand.Next(1000, 9999).ToString();
            }
            loginXml = "<login " + lang_at + isInternalSSO + _otpauth + " singleloginkey=\"" + privateSsoToken + "\" ssoname=\"" + SsoName + "\" " + lic_redis + " clouddb ='" + AxCloudDb + "' ip='" + ipaddress + "' other='" + userDetails + "'  seed='" + rnd_key + "'  axpapp='" + proj + "' sessionid='" + sid + "' username='" + user + "' password='' url='' direct='t' scriptpath='" + scriptsPath + "' axp_clientlocale='" + clientLocale + "' trace='" + errlog + "' " + lang_attr + licDetails + ">" + strContent.ToString() + axApps + axProps + "</login>";
        }
        else
        {
            loginXml = "<login " + lang_at + isInternalSSO + _otpauth + " clouddb='" + AxCloudDb + "' " + lic_redis + " ip='" + ipaddress + "' other='" + userDetails + "' timediff='" + diffTime + "' seed='" + rnd_key + "'  axpapp='" + proj + "' sessionid='" + sid + "' username='" + user + "' password='" + HashedPassword + "' url='' direct='t' scriptpath='" + scriptsPath + "' axp_clientlocale='" + clientLocale + "' trace='" + errlog + "' " + lang_attr + licDetails + ">" + strContent.ToString() + axApps + axProps + "</login>";
        }
    }


    public void CallLoginService()
    {
        //GetLoginXml();
        //ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        //result = objWebServiceExt.CallLoginWS("main", loginXml);       

        GetLoginJSON();
        ExecTrace ObjExecTr = ExecTrace.Instance;
        string strRequest = ObjExecTr.RequestProcessTime("Request");
        DateTime kst = DateTime.Now;
        result = ALCClient.CallALCClientLogin(ExecTraceOn, loginXml);
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        if (ExecTraceOn == "true")
        {
            strRequest += result.Split('♠')[0];
            result = result.Split('♠')[1];
        }
        result = objWebServiceExt.GetAppSessionKey("CallLoginWS", result);
        result = strRequest + ObjExecTr.KernelProcessTime(kst, "Login", loginXml, result) + "♠" + result;
    }

    private void GetLoginJSON()
    {
        if (loginTrace.ToLower() == "true")
            errlog = logobj.CreateLog("Call to Login Web Service", sid, "login", "", "true");
        else
            errlog = logobj.CreateLog("Call to Login Web Service", sid, "login", "");
        if (selectedLanguage == string.Empty)
            selectedLanguage = "English";

        string lang_at = "";
        if (selectedLanguage != null && selectedLanguage.ToUpper() != "ENGLISH")
            lang_at = " \"lang\":\"" + selectedLanguage + "\",";
        string scriptsPath = "";
        if (ConfigurationManager.AppSettings["scriptsUrlPath"] != null)
            scriptsPath = ConfigurationManager.AppSettings["scriptsUrlPath"].ToString();
        string isInternalSSO = string.Empty;
        if (SsoName != "" && SsoName == "InternalSSO")
        {
            isInternalSSO = " \"internalsso\":\"yes\",\"appsessionkey\":\"" + privateSsoToken + "\", ";
            privateSsoToken = "";
            SsoName = "";
            isSSO = false;
        }
        string _otpauth = string.Empty;
        string Otpmail = string.Empty;
        StringBuilder strContent = new StringBuilder();
        if (otpauth == "true")
        {
            _otpauth = " \"otpauth\":\"t\", ";
            string _otp = string.Empty;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            for (int i = 0; i < int.Parse(otpAuthCahrs); i++)
            {
                _otp += chars[random.Next(chars.Length)];
            }
            HttpContext.Current.Session["_otpAuth"] = _otp;
            strContent.Append("<otpmail><usermailid></usermailid><subject>Axpert OTP Authentication</subject><body>");
            strContent.Append("<l0>Dear " + user + ",</l0>");
            strContent.Append("<l01>" + proj + "</l01>");
            strContent.Append("<l1>One Time Password(OTP) for Axpert login is " + _otp + "</l1>");
            strContent.Append("<l3>Regards</l3><l4>Support Team.</l4></body></otpmail>");

            //strContent.Append(" \"usermailid\":\"\",\"subject\":\"Axpert OTP Authentication\",\"body\":{\"l0\":\"Dear " + user + "\",\"l01\":\"" + proj + "\",\"l1\":\"One Time Password(OTP) for Axpert login is " + _otp + "\",\"l3\":\"Regards\",\"l4\":\"Support Team.\"},");
            Otpmail = " \"otpmail\":\"" + strContent.ToString() + "\",";
        }
        else
        {
            _otpauth = " \"otpauth\":\"f\", ";
        }

        if (errlog != "")
            errlog = errlog.Replace("\\", "\\\\");

        axApps = axApps.Replace(@"\", "\\\\");
        if (isSSO)
        {
            if (rnd_key == string.Empty)
            {
                Random rand = new Random();
                rnd_key = rand.Next(1000, 9999).ToString();
            }
            //loginXml = "{\"_parameters\":[{\"loginwithuserkey\":{\"axpapp\":\"" + proj + "\"," + lang_at + isInternalSSO + _otpauth + lic_redis + " \"singleloginkey\":\"" + privateSsoToken + "\",\"ssoname\":\"" + SsoName + "\",\"clouddb\":\"" + AxCloudDb + "\",\"ip\":\"" + ipaddress + "\",\"timediff\":\"" + diffTime + "\",\"sessionid\":\"" + sid + "\",\"username\":\"" + user + "\",\"password\":\"\",\"seed\":\"" + rnd_key + "\",\"other\": \"" + userDetails + "\",\"trace\":\"" + errlog + "\",\"url\":\"\",\"direct\": \"t\",\"scriptpath\":\"" + scriptsPath + "\",\"axp_clientlocale\": \"" + clientLocale + "\"}," + strContent.ToString() + "\"axapps\":\"" + axApps + "\",\"axprops\":\"" + axProps + "\"}]}";
            loginXml = "{\"loginwithuserkey\":{\"axpapp\":\"" + proj + "\"," + lang_at + isInternalSSO + _otpauth + lic_redis + " \"singleloginkey\":\"" + privateSsoToken + "\",\"ssoname\":\"" + SsoName + "\",\"clouddb\":\"" + AxCloudDb + "\",\"ip\":\"" + ipaddress + "\",\"timediff\":\"" + diffTime + "\",\"sessionid\":\"" + sid + "\",\"username\":\"" + user + "\",\"password\":\"\",\"seed\":\"" + rnd_key + "\",\"other\": \"" + userDetails + "\",\"trace\":\"" + errlog + "\",\"url\":\"\",\"direct\": \"t\",\"scriptpath\":\"" + scriptsPath + "\",\"axp_clientlocale\": \"" + clientLocale + "\"}," + Otpmail + "\"axapps\":\"" + axApps + "\",\"axprops\":\"" + axProps + "\"}";
        }
        else
        {
            //loginXml = "{\"_parameters\":[{\"loginwithuserkey\":{\"axpapp\":\"" + proj + "\"," + lang_at + isInternalSSO + _otpauth + lic_redis + " \"clouddb\":\"" + AxCloudDb + "\",\"ip\":\"" + ipaddress + "\",\"timediff\":\"" + diffTime + "\",\"sessionid\":\"" + sid + "\",\"username\":\"" + user + "\",\"password\":\"" + HashedPassword + "\",\"seed\":\"" + rnd_key + "\",\"other\": \"" + userDetails + "\",\"trace\":\"" + errlog + "\",\"url\":\"\",\"direct\": \"t\",\"scriptpath\":\"" + scriptsPath + "\",\"axp_clientlocale\": \"" + clientLocale + "\"}," + strContent.ToString() + "\"axapps\":\"" + axApps + "\",\"axprops\":\"" + axProps + "\"}]}";
            loginXml = "{\"loginwithuserkey\":{\"axpapp\":\"" + proj + "\"," + lang_at + isInternalSSO + _otpauth + lic_redis + " \"clouddb\":\"" + AxCloudDb + "\",\"ip\":\"" + ipaddress + "\",\"timediff\":\"" + diffTime + "\",\"sessionid\":\"" + sid + "\",\"username\":\"" + user + "\",\"password\":\"" + HashedPassword + "\",\"seed\":\"" + rnd_key + "\",\"other\": \"" + userDetails + "\",\"trace\":\"" + errlog + "\",\"url\":\"\",\"direct\": \"t\",\"scriptpath\":\"" + scriptsPath + "\",\"axp_clientlocale\": \"" + clientLocale + "\"}," + Otpmail + "\"axapps\":\"" + axApps + "\",\"axprops\":\"" + axProps + "\"}";
        }
    }

    private string xmltojson(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        string json = JsonConvert.SerializeXmlNode(doc);
        var parsed = JObject.Parse(json);
        var jtoken = ((JContainer)parsed.First).First;
        return jtoken.ToString().Trim('{').Trim('}');
    }
}
