#region NameSpaces
using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml;
using ASBExt;
using System.Linq;

#endregion

namespace Axpert_Object
{
    //This class contains methods and properties for Login and Menu
    public class AxpertDotNet
    {
        //Objects
        Util.Util util = new Util.Util();
        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        LogFile.Log logobj = new LogFile.Log();

        #region "attributes"

        string sid = string.Empty;
        string user = string.Empty;
        string AxRole = string.Empty;
        string dtCulture = string.Empty;
        Boolean trace = false;

        //Build firstTimeLogin manage workflow 
        byte BFMW = 0;

        #endregion

        #region "Properties"

        string appName = string.Empty;
        string appTitle = string.Empty;
        string scriptsPath = string.Empty;
        string signOutPath = string.Empty;
        string loginPath = string.Empty;
        string axApps = string.Empty;
        string axProps = string.Empty;
        string projectName = string.Empty;
        string theme = string.Empty;
        ArrayList globalVariables = new ArrayList();
        ArrayList userVariables = new ArrayList();

        public string ApplicationName
        {
            get { return appName; }
            set { appName = value; }
        }

        public string ApplicationTitle
        {
            get { return appTitle; }
            set { appTitle = value; }
        }

        public string ScriptsPath
        {
            get { return scriptsPath; }
            set { scriptsPath = value; }
        }

        public string SignOutPath
        {
            get { return signOutPath; }
            set { signOutPath = value; }
        }

        public string LoginPath
        {
            get { return loginPath; }
            set { loginPath = value; }
        }

        public string AxApps
        {
            get { return axApps; }
            set { axApps = value; }
        }

        public string AxProps
        {
            get { return axProps; }
            set { axProps = value; }
        }

        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }



        #endregion

        //Constructor
        public AxpertDotNet()
        {
            scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            signOutPath = util.SIGNOUTPATH;
            loginPath = HttpContext.Current.Application["LoginPath"].ToString();
            if (HttpContext.Current.Application["AxGlobalApps"] != null)
                axApps = HttpContext.Current.Application["AxGlobalApps"].ToString();
            axProps = HttpContext.Current.Application["axProps"].ToString();
        }



        #region "Private"

        //Add a params details to the DataTable
        private DataTable AddRow(DataTable dt, string parent, string child, string target, string level)
        {
            DataRow dr = dt.NewRow();
            dr["pages"] = parent;
            dr["tstruct"] = child;
            dr["target"] = target;
            dr["level"] = level;
            dt.Rows.Add(dr);
            return dt;
        }

        private void SetDateCulture()
        {
            dtCulture = HttpContext.Current.Request.UserLanguages[0];
            if (dtCulture.ToLower() == "en")
                dtCulture = "en-us";
            HttpContext.Current.Session["ClientLocale"] = dtCulture;
            HttpContext.Current.Session["language"] = "English";
        }

        private void SetUserAccessProps(string result)
        {//Build, firstTimeLogin,Users,Workflow,import,export                 

            HttpContext.Current.Session["Build"] = result.Substring(0, 1);

            if (result.Substring(0, 1) == "T" || HttpContext.Current.Session["AxRole"].ToString() == "default_")
            {

                HttpContext.Current.Session["traceStatus"] = "T";
                util.sysErrorlog = true;
                logobj.errorlog = "true";

            }
            else
            {
                HttpContext.Current.Session["AxTrace"] = "false";
            }

            if (result.Substring(2, 1) == "T")
            {

                HttpContext.Current.Session["userAccessEnabled"] = "true";
            }

            if (result.Substring(3, 1) == "T")
            {
                HttpContext.Current.Session["workflowEnabled"] = "true";
            }

            if (result.Length > 4)
            {
                bool import = false;
                bool export = false;
                if (result.Substring(4, 1) == "T")
                {
                    import = true;
                    HttpContext.Current.Session["ImportAccess"] = "true";
                }
                if (result.Substring(5, 1) == "T")
                {
                    export = true;
                    HttpContext.Current.Session["ExportAccess"] = "true";
                }

                if (import || export)
                {

                    HttpContext.Current.Session["HistoryAccess"] = "true";
                }
            }

        }

        private void SetSessionVariables()
        {
            HttpContext.Current.Session["project"] = projectName;
            HttpContext.Current.Session["nsessionid"] = sid;
            HttpContext.Current.Session["AxRole"] = AxRole;
            HttpContext.Current.Session["user"] = user;
            HttpContext.Current.Session["validated"] = "True";
            HttpContext.Current.Session["themeColor"] = theme;

        }

        private bool IsResultXmlFormat(string result)
        {
            if (result.TrimStart().StartsWith("<") && result.TrimEnd().EndsWith(">"))
                return true;
            else
                return false;
        }

        //Process the login result and set the global variables 
        private void ProcessResult(string result)
        {

            if (IsResultXmlFormat(result))
            {
                SetSessionVariables();
                ParseLoginResult(result);
            }
            else
            {
                string[] themeres = result.Split(',');
                if ((themeres.Length > 1))
                {
                    theme = themeres[0].ToString();
                }
                else
                {
                    theme = "Gray";
                    HttpContext.Current.Session["themeColor"] = "Gray";
                }

                string[] splitout = result.Split('~');
                AxRole = splitout[1].ToString().Substring(0, splitout[1].ToString().Length - 1);
                string[] reg_gv = result.Split('#');
                HttpContext.Current.Session["globalvarstring"] = string.Empty;
                StringBuilder paramlist = new StringBuilder();
                int paramno = 0;
                if (reg_gv.Length > 0)
                {
                    string global_vars = reg_gv[1].ToString();
                    string[] glo_Split = global_vars.Split('^');
                    string globalApp_vars = glo_Split[0].ToString();
                    string globalUsr_vars = null;
                    if (glo_Split.Length > 1)
                    {
                        globalUsr_vars = glo_Split[1].ToString();
                    }
                    else
                    {
                        globalUsr_vars = "";
                    }


                    string[] gloApp_Split = globalApp_vars.Split('~');
                    HttpContext.Current.Session["paramstring"] = global_vars;
                    if (gloApp_Split.Length > 0)
                    {
                        int glo_inx = 0;
                        for (glo_inx = 1; glo_inx <= gloApp_Split.Length - 1; glo_inx++)
                        {
                            int indx = gloApp_Split[glo_inx].ToString().IndexOf("=");
                            string leftreg_glovar = gloApp_Split[glo_inx].ToString().Substring(0, indx);
                            string rightreg_glovar = gloApp_Split[glo_inx].Substring(indx + 1);
                            if (!Constants.globalVarPathVaribles.Contains(leftreg_glovar.ToLower()))
                            {
                                paramlist.Append("Parameters[" + paramno + "] = " + "\"" + leftreg_glovar + "~" + rightreg_glovar + "\";  ");
                                paramno = paramno + 1;
                            }
                        }
                    }

                    string[] gloUsr_Split = globalUsr_vars.Split('~');
                    if (gloUsr_Split.Length > 0)
                    {
                        int glo_inx1 = 0;
                        for (glo_inx1 = 1; glo_inx1 <= gloUsr_Split.Length - 1; glo_inx1++)
                        {
                            int indx1 = gloUsr_Split[glo_inx1].ToString().IndexOf("=");
                            string leftreg_glovar1 = gloUsr_Split[glo_inx1].ToString().Substring(0, indx1);
                            string rightreg_glovar1 = gloUsr_Split[glo_inx1].ToString().Substring(indx1 + 1);
                            if (!Constants.globalVarPathVaribles.Contains(leftreg_glovar1.ToLower()))
                            {
                                paramlist.Append("Parameters[" + paramno + "] = " + "\"" + leftreg_glovar1 + "~" + rightreg_glovar1 + "\";  ");
                                paramno = paramno + 1;
                            }
                        }
                    }

                }
                SetDateCulture();
                if ((paramlist.ToString() != string.Empty))
                {
                    paramlist.Append("Parameters[" + paramno + "] = " + "\"Culture~" + dtCulture + "\";  ");
                    HttpContext.Current.Session["globalvarstring"] = paramlist.ToString();
                }

                SetSessionVariables();

                SetUserAccessProps(splitout[0].ToString().Split(',')[1]);
            }
        }

        /// <summary>
        /// Function to parse the login result xml and set the access properties and global variables
        /// </summary>
        /// <param name="result"></param>
        private void ParseLoginResult(string result)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);

            XmlNode resultNode = xmlDoc.SelectSingleNode("/result");
            if (resultNode.Attributes["theme"] != null)
                theme = resultNode.Attributes["theme"].Value;
            else
                theme = "Blue";

            HttpContext.Current.Session["themeColor"] = theme;

            if (resultNode.Attributes["role"] != null)
            {
                AxRole = resultNode.Attributes["role"].Value;
                HttpContext.Current.Session["AxRole"] = resultNode.Attributes["role"].Value;
            }

            foreach (XmlNode childNode in resultNode.ChildNodes)
            {
                if (childNode.Name == "globalvars")
                {
                    HttpContext.Current.Session["axGlobalVars"] = "<globalvars>" + childNode.InnerXml + "</globalvars>";
                    foreach (XmlNode xmlNode in childNode.ChildNodes)
                        globalVariables.Add(xmlNode.Name + "=" + xmlNode.InnerText);
                }
                else if (childNode.Name == "uservars")
                {
                    HttpContext.Current.Session["axUserVars"] = "<uservars>" + childNode.InnerXml + "</uservars>";
                    foreach (XmlNode xmlNode in childNode.ChildNodes)
                        userVariables.Add(xmlNode.Name + "=" + xmlNode.InnerText);
                }
                else if (childNode.Name == "access")
                {
                    SetUserAccess(childNode);
                }
                else if (childNode.Name == "pwdconf")
                {
                    foreach (XmlNode xmlNode in childNode.ChildNodes)
                    {
                        if (xmlNode.Name == "minpwdchars")
                        {
                            HttpContext.Current.Session["minPwdChars"] = xmlNode.InnerText;
                        }
                        else if (xmlNode.Name == "pwdalphanumeric")
                        {
                            HttpContext.Current.Session["IsPwdAlphaNumeric"] = xmlNode.InnerText;
                        }
                    }
                }

            }

            if (globalVariables.Count > 0 || userVariables.Count > 0)
            {
                CreateParamaterArray();
            }
        }

        private void CreateParamaterArray()
        {
            StringBuilder paramlist = new StringBuilder();
            string value = string.Empty;
            int paramCnt = 0;

            //for (int i = 0; i < globalVariables.Count; i++)
            //{
            int i = 0;
            foreach (string item in globalVariables)
            {
                string[] strVar = item.ToString().Split('=');
                value = CheckSpecialChars(strVar[1].ToString());
                if (!Constants.globalVarPathVaribles.Contains(strVar[0].ToLower()))
                {
                    paramlist.Append("Parameters[" + i + "] = " + "\"" + strVar[0] + "~" + value + "\";  ");
                    i++;
                }
            }
            for (int j = 0; j < userVariables.Count; j++)
            {
                string[] strVar = userVariables[j].ToString().Split('=');
                value = CheckSpecialChars(strVar[1].ToString());
                paramlist.Append("Parameters[" + j + "] = " + "\"" + strVar[0] + "~" + value + "\";  ");
            }

            HttpContext.Current.Session["globalVarsArray"] = globalVariables;
            HttpContext.Current.Session["userVarsArray"] = userVariables;

            string dtCulture = HttpContext.Current.Request.UserLanguages[0];
            //If useCulture flag is false , we are not checking the browser culture.
            //If it's true ,we are setting the culture based on the client browser language.
            bool useCulture = false;
            if (!useCulture)
                dtCulture = "en-gb";
            else
            {
                if (dtCulture.ToLower() == "en")
                    dtCulture = "en-us";
            }
            paramCnt = i + userVariables.Count;
            paramlist.Append("Parameters[" + paramCnt + "] = " + "\"username~" + user + "\";  ");
            paramlist.Append("Parameters[" + (paramCnt + 1) + "] = " + "\"Culture~" + dtCulture + "\";  ");

            HttpContext.Current.Session["ClientLocale"] = dtCulture;

            if (paramlist.ToString() != string.Empty)
            {
                HttpContext.Current.Session["globalvarstring"] = paramlist.ToString();
            }
        }

        private string CheckSpecialChars(string str)
        {
            str = Regex.Replace(str, "&", "&amp;");
            str = Regex.Replace(str, "<", "&lt;");
            str = Regex.Replace(str, ">", "&gt;");
            str = Regex.Replace(str, "'", "&apos;");
            str = Regex.Replace(str, "\"", "&quot;");
            string delimited = @"\\";

            str = Regex.Replace(str, delimited, ";bkslh");

            return str;
        }

        /// <summary>
        /// The user access properties will be set for managing the application.
        /// </summary>
        /// <param name="childNode"></param>
        private void SetUserAccess(XmlNode childNode)
        {
            bool import = false;
            bool export = false;
            HttpContext.Current.Session["Build"] = "F";
            HttpContext.Current.Session["forcePwdChange"] = "0";
            HttpContext.Current.Session["userAccessEnabled"] = false;
            HttpContext.Current.Session["workflowEnabled"] = false;
            HttpContext.Current.Session["ImportAccess"] = false;
            HttpContext.Current.Session["ExportAccess"] = false;
            HttpContext.Current.Session["pwdExpiresIn"] = "0";

            foreach (XmlNode xmlNode in childNode.ChildNodes)
            {
                if (xmlNode.Name == "build" && xmlNode.InnerText == "T")
                {
                    HttpContext.Current.Session["Build"] = xmlNode.InnerText;
                }
                else if (xmlNode.Name == "forcepwdchange")
                {
                    HttpContext.Current.Session["forcePwdChange"] = xmlNode.InnerText;
                }
                else if (xmlNode.Name == "users" && xmlNode.InnerText == "T")
                {
                    HttpContext.Current.Session["userAccessEnabled"] = true;
                }
                else if (xmlNode.Name == "workflow" && xmlNode.InnerText == "T")
                {
                    HttpContext.Current.Session["workflowEnabled"] = true;
                }
                else if (xmlNode.Name == "import" && xmlNode.InnerText == "T")
                {
                    import = true;
                    HttpContext.Current.Session["ImportAccess"] = import;
                }
                else if (xmlNode.Name == "export" && xmlNode.InnerText == "T")
                {
                    export = true;
                    HttpContext.Current.Session["ExportAccess"] = import;
                }
                else if (xmlNode.Name == "sessionexists")
                {
                    // not in use - To display a message to the user that the previous session is expired.
                }
                else if (xmlNode.Name == "pwdexpiresin")
                {
                    HttpContext.Current.Session["pwdExpiresIn"] = xmlNode.InnerText;
                }
            }
            if (import || export)
            {
                HttpContext.Current.Session["HistoryAccess"] = true;
            }
        }

        private void ClearVars()
        {
            this.user = string.Empty;
            this.trace = false;
            this.sid = string.Empty;
            HttpContext.Current.Session["AxRole"] = string.Empty;
            HttpContext.Current.Session["project"] = string.Empty;
            HttpContext.Current.Session["nsessionid"] = string.Empty;
            HttpContext.Current.Session["user"] = string.Empty;
        }

        //Check the special characters
        private string CheckMenuSpecialChars(string str)
        {
            str = Regex.Replace(str, ";fwdslh", "/");
            str = Regex.Replace(str, ";hpn", "-");
            str = Regex.Replace(str, ";bkslh", "\\");
            str = Regex.Replace(str, ";eql", "=");
            str = Regex.Replace(str, ";qmrk", "?");
            return str;
        }

        public void GetSchemaDetails(string schemaName, string loginName)
        {
            //Using get choices get the schema details and construct the axapps as given below
            //<test>
            //<type>db</type>
            //<db>Oracle</db>                      //<db>MS SQL</db>        
            //<driver>dbx</driver>                 //<driver>ado</driver>
            //<dbcon>orcl</dbcon>
            //<dbuser>test</dbuser>
            //<structurl></structurl><pwd></pwd><dataurl></dataurl></test>
            string dbType = string.Empty;
            string dbcon = string.Empty;
            string dbuser = string.Empty;
            string dbpwd = string.Empty;
            //proj = ConfigurationManager.AppSettings["proj"].ToString();
            string proj = HttpContext.Current.Session["Project"].ToString();

            string errorLog = "";
            string iXml = "<sqlresultset axpapp='" + proj + "' sessionid='" + HttpContext.Current.Session.SessionID + "' direct='true' trace='" + errorLog + "'>";
            //iXml += "<sql>select dbase,dbhost,dbpwd from memberschema where schemaname ='" + schemaName + "' and agiledomain ='" + loginName + "'</sql>";
            iXml += "<sql>select dbase,dbhost,dbpwd from memberschema where schemaname ='" + schemaName + "'</sql>";
            iXml += HttpContext.Current.Application["AxGlobalApps"].ToString() + HttpContext.Current.Application["axProps"].ToString();
            if (HttpContext.Current.Session["axGlobalVars"] != null)
                iXml += HttpContext.Current.Session["axGlobalVars"].ToString();
            if (HttpContext.Current.Session["axUserVars"] != null)
                iXml += HttpContext.Current.Session["axUserVars"].ToString();
            iXml += "</sqlresultset>";

            WebServiceExt objExt = new WebServiceExt();
            string result = objExt.CallGetChoiceWS("", iXml);
            if (result.Contains(Constants.ERROR) == true)
            {
                result = result.Replace(Constants.ERROR, "");
                result = result.Replace("</error>", "");
                result = result.Replace("\n", "");
                throw (new Exception(result));
            }

            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlNodes = default(XmlNodeList);
            StringBuilder strSchemas = new StringBuilder();
            xmlDoc.LoadXml(result);
            xmlNodes = xmlDoc.SelectNodes("//response/row");

            foreach (XmlNode dbNode in xmlNodes[0])
            {
                string dbNodeName = dbNode.Name.ToLower();
                if (dbNodeName == "dbase")
                    dbType = dbNode.InnerText.ToLower();
                else if (dbNodeName == "dbhost")
                    dbcon = dbNode.InnerText;
                else if (dbNodeName == "dbpwd")
                    dbpwd = dbNode.InnerText;
            }

            //Parse and construct the axapps xml        
            StringBuilder strAxApps = new StringBuilder();
            strAxApps.Append("<" + proj + ">");
            strAxApps.Append("<type>db</type>");
            if (dbType == "oracle")
                strAxApps.Append("<db>Oracle</db><driver>dbx</driver>");
            else if (dbType == "ms sql")
                strAxApps.Append("<db>MS SQL</db><driver>ado</driver>");
            // strAxApps.Append("<dbcon>" + dbcon + "</dbcon>");
            strAxApps.Append("<dbcon>" + dbcon + "</dbcon>");
            strAxApps.Append("<dbuser>" + schemaName + "</dbuser>");
            strAxApps.Append("<pwd>" + dbpwd + "</pwd>");
            strAxApps.Append("</" + proj + ">");

            HttpContext.Current.Session["axApps"] = strAxApps.ToString();
            HttpContext.Current.Session["project"] = proj;
            //HttpContext.Current.Application["axApps"] = strAxApps.ToString();
        }

        #endregion

        #region "Public Methods"


        public string Login(string userName, string pwd, string schemaName, string loginName, string ipAddress, string seed)
        {
            string result = string.Empty;
            bool schemavalid = util.IsAlphaNum(schemaName);
            if (!schemavalid)
            {
                result = "<errornode>Schema invalid<errornode>";
                return result;
            }
            else
            {
                WebServiceExt objWebServiceExt = new WebServiceExt();
                GetSchemaDetails(schemaName, loginName);


                string iXml = string.Empty;
                axApps = HttpContext.Current.Session["axApps"].ToString();
                axProps = HttpContext.Current.Application["axProps"].ToString();
                string proj = HttpContext.Current.Session["Project"].ToString();
                sid = HttpContext.Current.Session.SessionID;

                string language = HttpContext.Current.Request.Form["language"];
                if (string.IsNullOrEmpty(language))
                    language = "ENGLISH";

                if (!string.IsNullOrEmpty(language))
                    HttpContext.Current.Session["language"] = language;
                else
                    HttpContext.Current.Session["language"] = string.Empty;
                //if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
                //  direction = "rtl";


                if (!string.IsNullOrEmpty(pwd))
                {
                    string errorLog = logobj.CreateLog("Login cloud", sid, "Login-", "new");
                    string lang_attr = string.Empty;
                    lang_attr = " lang=''";
                    iXml = "<login ip='" + ipAddress + "' other=''  seed='" + seed + "'  axpapp='" + proj + "' sessionid='" + sid + "' username='" + userName + "' password='" + pwd + "' url='' direct='t' trace='" + errorLog + "' " + lang_attr + ">";
                    iXml = iXml + axApps + axProps + "</login>";

                    try
                    {
                        result = objWebServiceExt.CallLoginWS("main", iXml);
                    }
                    catch (Exception ex)
                    {
                        string strErrMsg = string.Empty;
                        if (ex.Message.Length > 50)
                        {
                            strErrMsg = ex.Message.Substring(0, 50);
                            strErrMsg += "...";
                        }
                        else
                        {
                            strErrMsg = ex.Message;
                        }
                    }

                    user = userName;


                    if (result.Substring(0, 7) != Constants.ERROR)
                    {
                        //Store the basic seesion values
                        ProcessResult(result);

                        HttpContext.Current.Session["domainName"] = loginName;
                        result = "success";
                    }
                    else
                        ClearVars();

                }
            }

            return result;
        }

        /// <summary>
        /// Check the user credentials and return the success or error message
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="trace"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public string Login(string userName, string pwd, Boolean trace, string sessionId)
        {
            if (userName == string.Empty || pwd == string.Empty)
                return "<error>User name or password is empty</error>";

            this.user = userName;
            this.trace = trace;
            this.sid = sessionId;

            string iXml = string.Empty;
            string errlog = string.Empty;
            string result = string.Empty;
            string fileName = string.Empty;

            string language = HttpContext.Current.Request.Form["language"];
            if (string.IsNullOrEmpty(language))
                language = "ENGLISH";

            if (!string.IsNullOrEmpty(language))
                HttpContext.Current.Session["language"] = language;
            else
                HttpContext.Current.Session["language"] = string.Empty;
            //if (HttpContext.Current.Session["language"].ToString() == "ARABIC")
            //  direction = "rtl";

            //login code
            if (trace)
            {
                fileName = "Login";
                errlog = logobj.CreateLog("Call to Login Web Service", sid, fileName, "");
            }

            iXml = "<login ip=\"" + HttpContext.Current.Request.UserHostAddress + "\" seed=\"\"  axpapp=\"" + ProjectName + "\" sessionid=\"" + sid + "\" username=\"" + user + "\" password=\"" + pwd + "\" url=\"\" direct=\"t\" trace=\"" + errlog + "\">";
            iXml += axApps + axProps + "</login>";

            try
            {
                result = objWebServiceExt.CallLoginWS("main", iXml);
            }
            catch (Exception ex)
            {
                string strErrMsg = ex.Message;

                if ((strErrMsg.Length > 50))
                {
                    strErrMsg = ex.Message.Substring(0, 50);
                    strErrMsg += "...";
                }
                //  ClearVars();
                return Constants.ERROR + strErrMsg + Constants.ERRORCLOSE;
            }

            if (result.Substring(0, 7) != Constants.ERROR)
            {
                //Store the basic seesion values
                ProcessResult(result);
                result = "success";
            }
            else
                ClearVars();

            return result;
        }


        /// <summary>
        /// Returns  dataset containing the menu information
        /// </summary>
        /// <returns></returns>
        public DataSet GetMenu()
        {
            DataSet ds = new DataSet();

            string iXml = string.Empty;
            string errlog = string.Empty;
            string result = string.Empty;
            string fileName = string.Empty;

            if (trace)
            {
                fileName = "GetMultiLevelMenu";
                errlog = logobj.CreateLog("Call to GetMultiLevelMenu Web Service", sid, fileName, "");
                errlog = logobj.CreateLog("Getting Menu", sid, fileName, "");
            }

            iXml = "<root axpapp=\"" + projectName + "\" sessionid=\"" + sid + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trafdsce=\"" + errlog + "\" mname =\"\">";
            iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString();
            if (HttpContext.Current.Session["axGlobalVars"] != null)
                iXml += HttpContext.Current.Session["axGlobalVars"].ToString();
            if (HttpContext.Current.Session["axUserVars"] != null)
                iXml += HttpContext.Current.Session["axUserVars"].ToString();
            iXml += "</root>";
            try
            {
                result = objWebServiceExt.CallGetMultiLevelMenuWS("main", iXml);
            }
            catch (Exception ex)
            {
                if (trace)
                {
                    logobj.CreateLog("Exception in GetIview Service :--- " + ex.Message.ToString(), sid, fileName, "");
                }
                return null;
            }
            result = CheckMenuSpecialChars(result);
            HttpContext.Current.Session["MenuData"] = result;
            if ((result.Length == 0) || (result.Substring(1, 7) == Constants.ERROR))
            {
                return null;
            }

            string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            result = _xmlString + result;
            string parentName = string.Empty;
            string parentTarget = string.Empty;
            string parentLevel = string.Empty;
            string childName = string.Empty;
            string childTarget = string.Empty;
            string childLevel = string.Empty;
            if (trace)
                logobj.CreateLog("Binding menu for received xml", sid, "login", "");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);

            DataTable dt = new DataTable();

            dt.Columns.Add("pages");
            dt.Columns.Add("tstruct");
            dt.Columns.Add("target");
            dt.Columns.Add("level");

            string max = "0";
            XmlNodeList RootNodes = default(XmlNodeList);
            RootNodes = xmlDoc.SelectNodes("/root");
            // To get the maximum depth value at root node     
            foreach (XmlNode RootNode in RootNodes)
            {
                max = RootNode.Attributes["max"].Value;
            }
            XmlNodeList parentNodes = default(XmlNodeList);
            try
            {
                parentNodes = RootNodes[0].ChildNodes;
                foreach (XmlNode menuchldNode in parentNodes)
                {
                    //Parent Node
                    parentName = menuchldNode.Attributes["name"].Value;
                    parentTarget = menuchldNode.Attributes["target"].Value;
                    parentLevel = menuchldNode.Attributes["level"].Value;

                    dt = AddRow(dt, parentName, "", parentTarget, parentLevel);

                    XmlNodeList zeroChldNode = default(XmlNodeList);
                    zeroChldNode = menuchldNode.ChildNodes;

                    foreach (XmlNode firstChildNodes in zeroChldNode)
                    {
                        //First child Node
                        childName = firstChildNodes.Attributes["name"].Value;
                        childTarget = firstChildNodes.Attributes["target"].Value;
                        childLevel = firstChildNodes.Attributes["level"].Value;

                        dt = AddRow(dt, "", childName, childTarget, childLevel);
                        foreach (XmlNode secondChldNode in firstChildNodes)
                        {
                            //Second child Node
                            childName = secondChldNode.Attributes["name"].Value;
                            childTarget = secondChldNode.Attributes["target"].Value;
                            childLevel = secondChldNode.Attributes["level"].Value;

                            dt = AddRow(dt, "", childName, childTarget, childLevel);
                            foreach (XmlNode thirdChildNodes in secondChldNode)
                            {
                                //Third child Node
                                childName = thirdChildNodes.Attributes["name"].Value;
                                childTarget = thirdChildNodes.Attributes["target"].Value;
                                childLevel = thirdChildNodes.Attributes["level"].Value;

                                dt = AddRow(dt, "", childName, childTarget, childLevel);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            ds.Tables.Add(dt);

            return ds;
        }

        #endregion

        #region "Get Choice"

        /// <summary>
        /// Function to return the result of the given query in xml format
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isSessionActive"></param>
        /// <returns></returns>
        public string GetChoices(string query, bool isSessionActive)
        {
            string result = string.Empty;
            string proj = HttpContext.Current.Session["Project"].ToString();

            string errorLog = string.Empty;
            errorLog = logobj.CreateLog("GetChoices webservice", "1", "GetSchemas-", "new");
            string inputXML = "";
            if (isSessionActive)
            {
                inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + HttpContext.Current.Session.SessionID + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                inputXML += "<sql>" + query + "</sql>";
                inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            }
            else
            {
                inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + HttpContext.Current.Session.SessionID + "' direct='true' trace='" + errorLog + "'>";
                inputXML += "<sql>" + query + "</sql></sqlresultset>";
            }


            try
            {
                result = objWebServiceExt.CallGetChoiceWebService("", inputXML);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        #endregion

    }
}
