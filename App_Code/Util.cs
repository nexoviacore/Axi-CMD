
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections;
using System.Web.Caching;
using Newtonsoft.Json.Linq;
using CacheMgr;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using ASBExt;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.Web.Script.Serialization;
using Tools;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Threading;
using System.Globalization;
using System.Data.Odbc;
using StackExchange.Redis;
using Microsoft.Win32;

namespace Util
{
    /// <summary>
    /// Summary description for Util
    /// </summary>
    [Serializable()]
    public class Util
    {
        public string errorString = "../aspx/err.aspx?errmsg=Unknown error. Please try again. If the problem continues, please contact your administrator.";
        public string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        public string draftsPath = string.Empty;
        public string enableOldTheme = "false";
        public string CachePath = string.Empty;
        public string ScriptsurlPath = HttpContext.Current.Application["ScriptsurlPath"].ToString();
        public string[] InvalidFileTypes = new string[] { "dll", "exe", "bat" };

        //Global application constants
        public string LOGINPATH = Convert.ToString(HttpContext.Current.Application["LoginPath"]);
        public string SIGNOUTPATH = Convert.ToString(HttpContext.Current.Application["SignOutPath"]);
        public string SESSEXPIRYPATH = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        public string MAINPATH = Convert.ToString(HttpContext.Current.Application["MainPath"]);
        public string IviewWrap = "true";
        public Boolean BreadCrumb = true;
        public string ERRPATH = "../aspx/err.aspx?errmsg=";
        public string ACERRPATH = "../aspx/CloudErrorPage.aspx?errmsg=";
        public string SESSTIMEOUT = "SESSION_TIMEOUT";
        public string AXMANAGERPATH = "../aspx/axmanager.aspx";
        public Boolean sysErrorlog = false;
        public const int capacity = 10;
        List<string> allUrls = new List<string>(capacity);
        List<string> allurlIdx = new List<string>(capacity);
        [NonSerialized]
        public ConnectionMultiplexer redisLic;
        public void LogErrorToFile(string str)
        {

        }

        public Util()
        {
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["AxTrace"] != null && HttpContext.Current.Session["AxTrace"].ToString() != "")
                    sysErrorlog = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"]);
                if (HttpContext.Current.Session["AxEnableOldTheme"] != null)
                    enableOldTheme = HttpContext.Current.Session["AxEnableOldTheme"].ToString();
                //if (HttpContext.Current.Session["AxIviewcelltextwrap"] != null)
                //    IviewWrap = HttpContext.Current.Session["AxIviewcelltextwrap"].ToString();
                if (HttpContext.Current.Session["AxBreadCrumb"] != null)
                    BreadCrumb = Convert.ToBoolean(HttpContext.Current.Session["AxBreadCrumb"]);
            }
        }


        public string GetToken()
        {
            string token = "";
            try
            {
                AxpertToken.Token objToken = new AxpertToken.Token();
                string oldToken = "";
                if (HttpContext.Current.Session["axpToken"] != null)
                    oldToken = HttpContext.Current.Session["axpToken"].ToString();
                token = objToken.GetToken(oldToken);
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Exception in GetToken- util.cs-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exc-GetToken", "new");
            }
            //return "11711311101132110113111411321171134118113111411311115113";
            return token;
        }

        #region Validation

        /// <summary>
        /// Function to validate TransactionId length lesser or equal to 5 and allow only alphanumerics
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// 

        public bool IsTransIdValid(string str)
        {
            if (str == null) return false;
            if (IsUserNameValid(str) && (str.Length <= 5))
                return true;
            else
                return false;
        }

        public bool IsValidIvName(string str)
        {
            if (str == null) return false;
            if (IsUserNameValid(str) && (str.Length <= 8))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Function to validate alphanumerics numbers
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// 

        public bool IsAlphaNum(string str)
        {
            if (str == null) return false;
            Regex regexitem = new Regex("^[a-zA-Z0-9 ]*$");
            if (regexitem.IsMatch(str))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Function to validate alphanumerics numbers and underscore(_)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// 

        public bool IsAlphaNumUnd(string str)
        {
            if (str == null) return false;
            Regex regexitem = new Regex("^[a-zA-Z0-9 _]*$");
            if (regexitem.IsMatch(str))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Function to validate rowid  allow only numerics
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        /// 
        public bool IsNumber(string number)
        {
            if (number == null) return false;
            Regex regexItem = new Regex("^[0-9]*$");
            if (regexItem.IsMatch(number))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Function to validate string to  allow only alphabets
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        /// 
        public bool IsChar(string str)
        {
            if (str == null) return false;
            Regex regexItem = new Regex("^[a-zA-z]*$");
            if (regexItem.IsMatch(str))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Function to validate and allow only valid docname
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        /// 
        public bool IsDocName(string DocName)
        {
            if (DocName == null) return true;
            Regex regexItem = new Regex("^[*?<>:|]+$");
            if (regexItem.IsMatch(DocName))
                return false;
            else
                return true;
        }



        /// <summary>
        /// Function to validate string to allow only alphanumeric and underscore
        /// </summary>
        /// <param name="number">string</param>
        /// <returns></returns>
        /// 
        public bool IsUserNameValid(string Str)
        {
            if (Str == null) return false;
            if (Str.Contains('@'))
            {
                return Regex.IsMatch(Str, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            }
            else
            {
                Regex regexItem = new Regex("^[a-zA-Z0-9_.]*$");
                if (regexItem.IsMatch(Str))
                    return true;
                else
                    return false;
            }
        }
        public bool IsSearchFieldNameValid(string Str)
        {
            if (Str == null) return false;
            Regex regexItem = new Regex("^[a-zA-Z~0-9_.]*$");
            if (regexItem.IsMatch(Str))
                return true;
            else
                return false;
        }

        public bool IsPwdValid(string str)
        {
            if (str == null) return true;
            Regex regexItem = new Regex("^[a-zA-Z0-9-_.!@#$]*$");
            if (regexItem.IsMatch(str))
                return false;
            else
                return true;
        }

        public bool IsHashValid(string str)
        {
            if (str == null) return false;
            Regex regexItem = new Regex("^[0-9]*$");
            if (regexItem.IsMatch(str))
                return true;
            else
                return false;
        }
        public bool IsProjectValid(string str)
        {
            if (str == null) return false;
            Regex regexItem = new Regex("^[a-zA-Z0-9_,. ]*$");
            if (regexItem.IsMatch(str))
                return true;
            else
                return false;
        }

        /// <summary>
        /// To get get error msg as json
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>Json string</returns>
        public string ErrorMsgToJson(string str)
        {
            string jsnnode = "{ \"error\": [{ \"msg\": \"" + str + "\"}] }";
            return jsnnode;
        }

        public bool IsParamArrayList(ArrayList arr)
        {
            return (arr != null && arr.GetType() == typeof(ArrayList) && arr.Count >= 0);
        }

        public bool IsParamArrayList(ArrayList arr1, ArrayList arr2, ArrayList arr3, ArrayList arr4)
        {
            if ((arr1 != null && arr1.GetType() == typeof(ArrayList) && arr1.Count >= 0) &&
                (arr2 != null && arr2.GetType() == typeof(ArrayList) && arr2.Count >= 0) &&
                (arr3 != null && arr3.GetType() == typeof(ArrayList) && arr3.Count >= 0) &&
                (arr4 != null && arr4.GetType() == typeof(ArrayList) && arr4.Count >= 0))
                return true;
            else
                return false;
        }

        #endregion

        /// <summary>
        /// Function to check for special characters in given str and replace them with standard constants.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string CheckSpecialChars(string str)
        {
            if (str == null)
                str = "";
            str = Regex.Replace(str, "&", "&amp;");
            str = Regex.Replace(str, "<", "&lt;");
            str = Regex.Replace(str, ">", "&gt;");
            str = Regex.Replace(str, "'", "&apos;");
            str = Regex.Replace(str, "\"", "&quot;");
            string delimited = @"\\";
            str = Regex.Replace(str, delimited, ";bkslh");
            return str;
        }

        public string AdditionalSpecialCharsCheck(string str)
        {
            if (str == null)
                str = "";

            str = Regex.Replace(str, "&apos;", "&#39;");
            str = Regex.Replace(str, ";bkslh", "&#92;");
            str = Regex.Replace(str, "&amp;#94;", "^");
            str = Regex.Replace(str, "&amp;#64;", "@");
            str = Regex.Replace(str, "&amp;grave;", "~");
            str = Regex.Replace(str, "&grave;", "~");
            return str;
        }

        public string ReverseCheckSpecialChars(string str)
        {
            if (str == null)
                str = "";
            str = Regex.Replace(str, "&amp;", "&");
            str = Regex.Replace(str, "&lt;", "<");
            str = Regex.Replace(str, "&gt;", ">");
            str = Regex.Replace(str, "&apos;", "'");
            str = Regex.Replace(str, "&quot;", "\"");
            str = Regex.Replace(str, "&nbsp;", " ");
            return str;
        }

        /// <summary>
        /// This function replaces special characters in sql query, since singlequote is handled differently
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ReverseCheckSpecialCharsInQuery(string str)
        {
            if (str == null)
                str = "";
            str = Regex.Replace(str, "&amp;", "&");
            str = Regex.Replace(str, "&lt;", "<");
            str = Regex.Replace(str, "&gt;", ">");
            str = Regex.Replace(str, "&apos;", "''");
            str = Regex.Replace(str, "&quot;", "\"");
            return str;
        }

        public string CheckSplChrInputXML(string str)
        {
            if (str != null && str != "")
            {
                if (!str.StartsWith("<"))
                {
                    int strSpChar = str.Split('<')[0].Length;
                    str = str.Substring(strSpChar, str.Length - strSpChar);
                    str = str.TrimStart();
                }
                if (!str.EndsWith(">"))
                {
                    int endSpChar = str.Split('>').Last().Length;
                    str = str.Substring(0, str.Length - endSpChar);
                    str = str.TrimEnd();
                }
            }
            return str;
        }

        public string ReplaceSqlInjChar(string str)
        {
            if (str == null)
                str = "";
            return str.Replace("'", "''");
        }

        public bool IsNullOrEmpty(String value)
        {
            return (value == null || value.Length == 0);
        }

        public string loginFormValues()
        {
            StringBuilder sbLoginFrmVals = new StringBuilder();
            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                sbLoginFrmVals.Append("<input type=hidden name=" + HttpContext.Current.Request.Form.Keys[i].ToString() + " value=" + HttpContext.Current.Request.Form[i].ToString() + ">");
            }
            return sbLoginFrmVals.ToString();
        }

        public string toggleTheme()
        {
            string href = string.Empty;
            if (HttpContext.Current.Session["AxEnableOldTheme"] != null)
                enableOldTheme = HttpContext.Current.Session["AxEnableOldTheme"].ToString().ToLower();
            if (enableOldTheme == "true")
                href = @"~\css\genericOld.min.css";
            else
                href = @"~\css\generic.css";

            return href;

        }

        /// <summary>
        /// Function to check for special characters in given str and replace them with standard constants.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string CheckSpecialCharsSaveAs(string str)
        {
            if (str == null)
                str = "";
            str = Regex.Replace(str, "&quot;", "\"");
            str = Regex.Replace(str, "&apos;", "'");

            str = Regex.Replace(str, "&", "&amp;");
            str = Regex.Replace(str, "<", "&lt;");
            str = Regex.Replace(str, ">", "&gt;");
            str = Regex.Replace(str, "'", "&apos;");
            str = Regex.Replace(str, "\"", "&quot;");

            return str;
        }

        public string CheckSpecialCharacterPurpose(string str)
        {
            if (str == null)
                str = "";
            str = Regex.Replace(str, "&quot;", "\"");
            str = Regex.Replace(str, "&apos;", "'");

            str = Regex.Replace(str, "&", "&amp;");
            str = Regex.Replace(str, "<", "&lt;");
            str = Regex.Replace(str, ">", "&gt;");
            str = Regex.Replace(str, "'", "&apos;");
            str = Regex.Replace(str, "\"", "&quot;");
            str = Regex.Replace(str, @"\\", "&#92;");
            str = Regex.Replace(str, "/", "&#47;");
            str = Regex.Replace(str, ",", "&#44;");
            str = Regex.Replace(str, @"\*", "&#42;");
            str = Regex.Replace(str, "-", "&macr;");
            str = Regex.Replace(str, ":", "&#58;");
            str = Regex.Replace(str, @"\.", "&#46;");

            return str;
        }

        public Array AxSplit(string str, string delimiter)
        {
            str = str.Replace(delimiter, "♣");
            string[] splitArr = str.Split('♣');
            return splitArr;
        }

        public string[] AxSplit1(string str, string delimiter)
        {
            str = str.Replace(delimiter, "♣");
            string[] splitArr = str.Split('♣');
            return splitArr;
        }

        public string GetNextSet(int setNo, int noOfPages)
        {
            string str = "";
            for (int i = 1; i <= 10; i++)
            {
                int j = ((setNo - 1) * 10) + i;
                if (j <= noOfPages)
                {
                    if (str == "")
                        str += j.ToString();
                    else
                        str += "," + j.ToString();
                }
            }
            return str;
        }

        /// <summary>
        /// Returns true if the file type is not any of the InvalidFileTypes.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>  
        public bool IsFileTypeValid(HttpPostedFile file)
        {
            bool isValid = true;
            string fileName = Path.GetFileName(file.FileName);
            string[] fileExtnStr = fileName.Split('.');

            if (file.ContentType == "application/x-msdownload")
            {
                string fileExtn = fileExtnStr[fileExtnStr.Length - 1].ToString();
                if (fileExtn == "dll" || fileExtn == "exe" || fileExtn == "bat" || fileExtn == "reg" || fileExtn == "com")
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// Loops through the application cache objects and deletes them.
        /// </summary>
        /// <returns></returns>
        public string ClearApplicationCache()
        {
            string msg = "Cache cleared.";
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            try
            {
                // delete every key from cache
                for (int i = 0; i < keys.Count; i++)
                {
                    HttpRuntime.Cache.Remove(keys[i]);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        public bool fBrowserIsMobile()
        {
            if (HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                var u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();

                if (u.Contains("mobile") || u.Contains("Mobile"))
                    return true;
            }

            return false;
        }

        public string GetClientDateString(string clientCulture, string dt, bool isUnProcessed = false)
        {
            if (dt == string.Empty)
                return dt;
            // as the default format is en-gb, do nothing. so handling only the us culture
            // the last value, year, also contains the time. 
            if (clientCulture.ToLower().ToString() == "en-us")
            {
                string[] tmpDateArr = dt.Split(' ');
                string[] tmpDateArr2 = tmpDateArr[0].Split('/');
                if (tmpDateArr2.Length > 2)
                {
                    if (isUnProcessed && tmpDateArr2[0].Length == 4)
                    {
                        dt = tmpDateArr2[2] + "/" + tmpDateArr2[0] + "/" + tmpDateArr2[1];
                    }
                    else
                    {
                        dt = tmpDateArr2[1] + "/" + tmpDateArr2[0] + "/" + tmpDateArr2[2];
                    }
                }
            }
            else if (clientCulture.ToLower().ToString() == "en-gb")
            {
                string[] tmpDateArr = dt.Split(' ');
                string[] tmpDateArr2 = tmpDateArr[0].Split('-');
                if (tmpDateArr2.Length > 2)
                {
                    //dt = tmpDateArr2[0] + "/" + tmpDateArr2[1] + "/" + tmpDateArr2[2];
                    if (isUnProcessed && tmpDateArr2[0].Length == 4)
                    {
                        dt = tmpDateArr2[2] + "/" + tmpDateArr2[1] + "/" + tmpDateArr2[0];
                    }
                    else
                    {
                        dt = tmpDateArr[0];
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Function returns the error message if the error node is there, else returns empty.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public string ParseJSonErrorNode(string result, bool throwEx = true)
        {
            result = result.Replace("^^dq", "\'");
            string returnStr = result.Replace("*$*", "¿");
            string[] newResult = returnStr.Split('¿');
            for (int i = 0; i < newResult.Length; i++)
            {
                JArray msg = null;
                try
                {
                    JObject message = JObject.Parse(newResult[i].ToString());
                    msg = (JArray)message["error"];
                }
                catch (Exception ex)
                {
                    if (throwEx)
                    {
                        throw ex;
                    }
                }

                if (msg != null)
                {
                    string saveResult = msg[0].SelectToken("msg").ToString();
                    saveResult = saveResult.Remove(0, 1);
                    saveResult = saveResult.Remove(saveResult.Length - 1, 1);
                    return saveResult;
                }
            }
            return "";
        }


        /// <summary>
        /// Function to return the random number
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public int GenereateRndNo()
        {
            Random rNo = new Random();
            int n;
            n = rNo.Next(1111111, 9999999);
            return n;
        }

        /// <summary>
        /// Function to generate the key for storing the tstruct data object in session.
        /// </summary>
        /// <param name="transid"></param>
        /// <returns></returns>
        public string GetTstDataId(string transid)
        {
            return transid + "_" + GenereateRndNo();
        }

        public string GetIviewId(string ivName)
        {
            return ivName + "_" + GenereateRndNo();
        }

        public string GetLviewId(string lvName)
        {
            return lvName + "_" + GenereateRndNo();
        }
        /// <summary>
        /// Function to check if the result contains error node
        /// If the result contains error, return the error message
        /// else return empty.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public string ParseXmlErrorNode(string result)
        {
            string errMsg = string.Empty;

            if (result.StartsWith(Constants.ERROR))
            {
                result = result.Replace(Constants.ERROR, "");
                result = result.Replace(Constants.ERRORCLOSE, "");
                if (result == string.Empty)
                    result = "Unknown error.";
                errMsg = result;
            }
            return errMsg;
        }


        public string ParseXmlErrorMsgNode(string result)
        {
            string errMsg = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            if (xmlDoc.ChildNodes[0] != null)
            {
                if (xmlDoc.ChildNodes[0].Name == "error")
                {
                    foreach (XmlNode childNode in xmlDoc.ChildNodes[0])
                    {
                        if (childNode.Name == "msg")
                        {
                            return childNode.InnerText;
                        }
                    }

                    errMsg = xmlDoc.ChildNodes[0].InnerText;
                }
            }

            return errMsg;
        }

        /// <summary>
        /// Function to replace the browser generated html tags in the result.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public string ReplaceTextAreaChars(string result, string src)
        {
            //The below lines are commented as the result from GetIView is called and it replaces the symbols with
            //result = Regex.Replace(result, "&lt;", "<");
            //result = Regex.Replace(result, "&gt;", ">");
            result = Regex.Replace(result, "<B>", "");
            result = Regex.Replace(result, "</B>", "");
            if (src == "pdf")
            {
                result = Regex.Replace(result, "<BR>", "\n");
            }
            else
            {
                result = Regex.Replace(result, "<BR>", " ");
                result = Regex.Replace(result, "</BR>", " ");
                result = Regex.Replace(result, "<BR/>", " ");
            }
            return result;
        }

        /// <summary>
        /// Function to check for the image with the given image name under the axpimages folder.
        /// Returns true, if the image is foun, else returns false.
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public bool IsImageAvailable(string imageName, string imgType = "")
        {
            bool imageExists = false;
            FileInfo fil;
            if (imgType == "icon")
            {
                string Iconpath = GetAdvConfigs("icon path");
                string name = imageName.Substring(imageName.LastIndexOf("/") + 1);
                if (Iconpath == null || Iconpath == string.Empty)
                {
                    Iconpath = HttpContext.Current.Application["ScriptsPath"].ToString() + "images\\user Icons\\" + name;
                }
                else
                {
                    Iconpath = HttpContext.Current.Application["ScriptsPath"].ToString() + Iconpath + "\\" + name;
                }
                //  string path = HttpContext.Current.Application["ScriptsPath"].ToString() + imageName.Split(new[] { "localhost" },StringSplitOptions.None)[1];
                fil = new FileInfo(Iconpath);

            }
            else
                fil = new FileInfo(HttpContext.Current.Server.MapPath("../Axpimages\\" + imageName));
            if (fil.Exists)
                imageExists = true;

            return imageExists;
        }
        /// <summary>
        /// Function to check the langauge for arabic .
        /// </summary>
        /// <returns></returns>
        public string GetLngAttribute()
        {
            string language = string.Empty;
            if (HttpContext.Current.Session["language"] != null)
                language = HttpContext.Current.Session["language"].ToString();

            string lang_at = string.Empty;
            if (language != "ENGLISH")
            {
                lang_at = " lang=\"" + language + "\"";
            }

            return lang_at;
        }


        public bool IsLogoExists()
        {
            bool isLogo = false;
            FileInfo fil = new FileInfo(HttpContext.Current.Server.MapPath("../assets/img/logo.png"));
            if (fil.Exists)
                isLogo = true;
            return isLogo;
        }

        public void IFrameSessExpiry()
        {
            string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
            HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
            HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
            HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
        }
        public string GetDupUserInfo(string pName, string uName)
        {
            string lstUser = string.Empty;
            try
            {
                if (HttpContext.Current.Session["dbuser"] == null)
                    GetAxApps(pName);
                FDW fdwObj = new FDW(pName);
                bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
                if (isRedisConnected)
                {
                    FDR fObj = new FDR(pName);
                    lstUser = fObj.HashGetKey(Constants.AXLOGGEDUSER, uName);
                }
                else
                {
                    if (HttpContext.Current.Application[pName + Constants.AXLOGGEDUSER + uName] != null)
                        lstUser = HttpContext.Current.Application[pName + Constants.AXLOGGEDUSER + uName].ToString();
                }
            }
            catch (Exception ex)
            { }
            return lstUser;
        }
        public void SetDupUserInfo(string pName, string uName, string strVal)
        {
            try
            {
                FDW fdwObj = new FDW();
                bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
                if (isRedisConnected)
                    fdwObj.HashSetKey(Constants.AXLOGGEDUSER, uName, strVal);
                else
                    HttpContext.Current.Application[pName + Constants.AXLOGGEDUSER + uName] = strVal;
            }
            catch (Exception ex)
            { }
        }

        public string GetCustomsGroupButtons(string idd, string callertype)
        {
            string result = string.Empty;
            string groupButtons = string.Empty;
            //ASBCustom.CustomWebservice objWeb = new ASBCustom.CustomWebservice();
            ASB.WebService objWeb = new ASB.WebService();
            try
            {
                string sqlStr = String.Empty;
                if (callertype == "iview")
                {
                    sqlStr = Constants.GET_IV_GROUPED_BUTTONS.Replace(Constants.VAR_IVNAME, idd);
                }
                //result = objWeb.GetChoices(idd, sqlStr);
                result = objWeb.GetChoicesCustomWS(idd, sqlStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (result.Contains(Constants.ERROR) == true)
            {
                result = result.Replace(Constants.ERROR, "");
                result = result.Replace("</error>", "");
                result = result.Replace("\n", "");
                //TODO: Exception logging to be done
            }
            if (result != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(result);
                }
                catch (Exception ex)
                {
                    //TODO: exception logging to be done
                }
                XmlNode resultNode = null;


                resultNode = xmlDoc.SelectSingleNode("//row");
                if (resultNode != null)
                {
                    foreach (XmlNode childNode in resultNode.ChildNodes)
                    {
                        if (childNode.Name.ToLower() == "cvalue")
                        {
                            groupButtons = childNode.InnerText;
                            break;
                        }
                    }
                }
            }
            return groupButtons;

        }

        #region "Public function"
        public TStructDef GetTstructDefObj(string errorLog, string transid)
        {
            TStructDef strObj = null;
            LogFile.Log logobj = new LogFile.Log();
            try
            {
                CacheManager cacheMgr = new CacheManager(errorLog);
                strObj = cacheMgr.GetStructDef(HttpContext.Current.Session["project"].ToString(), HttpContext.Current.Session["nsessionid"].ToString(), HttpContext.Current.Session["user"].ToString(), transid, HttpContext.Current.Session["AxRole"].ToString());
            }
            catch (Exception ex)
            {
                if (sysErrorlog)
                    logobj.CreateLog("Exception in GetStructure Service :--- " + ex.Message.ToString(), HttpContext.Current.Session["nsessionid"].ToString(), "GetStructDef" + transid, "");
                HttpContext.Current.Server.Transfer(errorString);
            }
            return strObj;
        }

        public void UploadFiles(string f, string sid)
        {
            string axpert = "Axpert\\";
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            //Create a Folder
            try
            {
                DirectoryInfo di = new DirectoryInfo(ScriptsPath + axpert + sid);

                //' Determine whether the directory exists.
                if (di.Exists)
                {

                }
                else
                {
                    // create the directory.
                    di.Create();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string[] files = new string[21];
            if ((f.IndexOf(",") != -1))
            {
                files = f.Split(',');
            }
            else
            {
                files[0] = f;
            }

            int i = 0;
            if ((files.Length > 0))
            {

                for (i = 0; i <= files.Length - 1; i++)
                {

                    if (((files[i] != null)))
                    {
                        string outputPath = null;
                        System.IO.BinaryReader brReader = default(System.IO.BinaryReader);
                        System.IO.BinaryWriter brWriter = default(System.IO.BinaryWriter);

                        f = files[i].ToString();
                        string filename = "";
                        int index = f.LastIndexOf("\\");
                        if ((index != -1))
                        {
                            //if the file value is a path location then copy the file to session script folder
                            filename = f.Substring(index + 1);
                            outputPath = ScriptsPath + axpert + sid + "\\" + filename;
                            //remove if file exists in the session script folder & copy again the file from the path to session script folder
                            if (File.Exists(outputPath))
                                File.Delete(outputPath);

                            if (File.Exists(f))
                                File.Copy(f, outputPath);
                        }
                        else
                        {
                            filename = f;
                            outputPath = ScriptsPath + axpert + sid + "\\" + filename;
                            //' Create a file
                            f = ScriptsPath + axpert + sid + "\\" + f;
                            if (!File.Exists(outputPath))
                            {
                                FileStream input = new FileStream(outputPath, FileMode.Open, FileAccess.Read);
                                FileStream output = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                                int Iindexer = 0;
                                int FileLen = 0;

                                brReader = new System.IO.BinaryReader(input);
                                brWriter = new System.IO.BinaryWriter(output);
                                int bufsize = 30000;
                                // this is buffer size
                                int readcount = 0;
                                int bsize = 0;

                                Iindexer = 0;
                                FileInfo fileInfo = new FileInfo(f);

                                FileLen = Convert.ToInt32(fileInfo.Length);
                                while ((readcount < FileLen))
                                {
                                    if (bufsize < FileLen - readcount)
                                    {
                                        bsize = bufsize;
                                    }
                                    else
                                    {
                                        bsize = FileLen - readcount;
                                    }
                                    byte[] buffer = new byte[bsize];

                                    brReader.Read(buffer, Iindexer, bsize);
                                    brWriter.Write(buffer, Iindexer, bsize);

                                    readcount = readcount + bsize;
                                }

                                brReader.Close();
                                brWriter.Close();
                            }
                        }

                    }
                }
                files = null;
            }
        }

        public void RefreshField()
        {

        }

        public void RefreshDc()
        {

        }

        public void KillSession()
        {
            if (HttpContext.Current.Session.Contents.Count > 0)
            {
                HttpContext.Current.Session.Contents.Clear();
            }
            System.Web.Configuration.SessionStateSection sessionStateSection = (System.Web.Configuration.SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
            string cookieName = sessionStateSection.CookieName;

            HttpCookie mycookie = new HttpCookie(cookieName);
            mycookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(mycookie);
            HttpContext.Current.Session.Abandon();
        }

        public string CheckUrlSpecialChars(string value)
        {
            value = value.Replace("%", "%25");
            value = value.Replace("#", "%23");
            value = value.Replace("&", "%26");
            value = value.Replace("'", "%27");
            value = value.Replace("\"", "%22");
            value = value.Replace("+", "%2b");
            value = value.Replace("<", "%3C");
            value = value.Replace("\\", "%5C");
            value = value.Replace(" ", "%20");
            return value;
        }

        public string CheckReverseUrlSpecialChars(string value)
        {
            value = value.Replace("%25", "%");
            value = value.Replace("%23", "#");
            value = value.Replace("%26", "&");
            value = value.Replace("%27", "'");
            value = value.Replace("%22", "\"");
            value = value.Replace("%2b", "+");
            value = value.Replace("%3C", "<");
            value = value.Replace("%3E", ">");
            value = value.Replace("%5C", "\\");
            value = value.Replace("%20", " ");
            return value;
        }

        internal void ClearAxpStructConfigInRedis(string key, string transid, string cfgRoles, string type, string pageName)
        {
            FDW fdwObj = new FDW();
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            if (type == "general")
            {
                string configKey = "axGenConfigs-";
                ArrayList allConfigKeys = fdrObj.GetAllKeys(configKey);
                for (int i = 0; i < allConfigKeys.Count; i++)
                {
                    string keyName = allConfigKeys[i].ToString();
                    fdwObj.DeleteAllKeys(keyName, schemaName);
                }
                string configNoKey = "-axNoGenConfigs-";
                ArrayList allNoDataConfigKeys = fdrObj.GetAllKeys(configNoKey);
                for (int i = 0; i < allNoDataConfigKeys.Count; i++)
                {
                    string keyName = allNoDataConfigKeys[i].ToString();
                    fdwObj.DeleteAllKeys(keyName, schemaName);
                }
            }
            else if (type == "tstruct")
            {
                if (pageName != "ALL Forms")
                {
                    //string configKey = pageName + "-axconfigstruct-" + cfgRoles + '-';
                    string configKey = pageName + "-axconfigstruct-";
                    ArrayList allConfigKeys = fdrObj.GetAllKeys(configKey);
                    for (int i = 0; i < allConfigKeys.Count; i++)
                    {
                        string keyName = allConfigKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, schemaName);
                    }

                    //string axpConfigTable = pageName + "-tstNoConfigs-" + cfgRoles + '-';
                    string axpConfigTable = pageName + "-tstNoConfigs-";
                    ArrayList allConfigTblKeys = fdrObj.GetAllKeys(axpConfigTable);
                    for (int i = 0; i < allConfigTblKeys.Count; i++)
                    {
                        string keyName = allConfigTblKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, schemaName);
                    }
                }
                else
                {
                    //string configKey = "-axconfigstruct-" + cfgRoles + '-';
                    string configKey = "-axconfigstruct-";
                    ArrayList allConfigKeys = fdrObj.GetAllKeys(configKey);
                    for (int i = 0; i < allConfigKeys.Count; i++)
                    {
                        //remove these keys in REDIS
                        string keyName = allConfigKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, schemaName);
                    }

                    //string axpConfigTable = "-tstNoConfigs-" + cfgRoles + '-';
                    string axpConfigTable = "-tstNoConfigs-";
                    ArrayList allConfigTblKeys = fdrObj.GetAllKeys(axpConfigTable);
                    for (int i = 0; i < allConfigTblKeys.Count; i++)
                    {
                        //remove these keys in REDIS
                        string keyName = allConfigTblKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, schemaName);
                    }
                }
            }
            else if (type == "iview")
            {
                if (pageName != "ALL Reports")
                {
                    //string configKey = pageName + "-axconfigstruct-" + cfgRoles + '-';
                    string configKey = pageName + "-axivconfigstruct-";
                    ArrayList allConfigKeys = fdrObj.GetAllKeys(configKey);
                    for (int i = 0; i < allConfigKeys.Count; i++)
                    {
                        string keyName = allConfigKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, schemaName);
                    }

                    //string axpConfigTable = pageName + "-tstNoConfigs-" + cfgRoles + '-';
                    string axpConfigTable = pageName + "-ivNoConfigs-";
                    ArrayList allConfigTblKeys = fdrObj.GetAllKeys(axpConfigTable);
                    for (int i = 0; i < allConfigTblKeys.Count; i++)
                    {
                        string keyName = allConfigTblKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, schemaName);
                    }
                }
                else
                {
                    //string configKey = "-axconfigstruct-" + cfgRoles + '-';
                    string configKey = "-axivconfigstruct-";
                    ArrayList allConfigKeys = fdrObj.GetAllKeys(configKey);
                    for (int i = 0; i < allConfigKeys.Count; i++)
                    {
                        //remove these keys in REDIS
                        string keyName = allConfigKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, schemaName);
                    }

                    //string axpConfigTable = "-tstNoConfigs-" + cfgRoles + '-';
                    string axpConfigTable = "-ivNoConfigs-";
                    ArrayList allConfigTblKeys = fdrObj.GetAllKeys(axpConfigTable);
                    for (int i = 0; i < allConfigTblKeys.Count; i++)
                    {
                        //remove these keys in REDIS
                        string keyName = allConfigTblKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, schemaName);
                    }
                }
            }
        }

        public string ReplaceUrlSpecialChars(string value)
        {
            value = value.Replace("%25", "%");
            value = value.Replace("%23", "#");
            value = value.Replace("%26", "&");
            value = value.Replace("%27", "'");
            value = value.Replace("%22", "\"");
            value = value.Replace("%2b", "+");
            return value;
        }

        #endregion

        #region Print Docs

        /// <summary>
        /// Function to construct the html of the print docs from the array in session.
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        public string GetPrintDocsHTML(ArrayList docs)
        {
            StringBuilder docsHtml = new StringBuilder();

            if (docs.Count == 0)
            {
                docsHtml.Append("<div id=\"dvPrintMsg\" class=\"info\" style=\"width: 500px;\">");
                docsHtml.Append("No Documents to print.</div>");
            }
            else
            {

                docsHtml.Append("<div id=\"dvPrintMsg\" class=\"info\" style=\"width: 500px;\">Your document is being generated, you may close this window now. ");
                docsHtml.Append("To know the status click on the 'PrintDocs' link at the top rigth corner of the page</div>");

                docsHtml.Append("<div style=\"height:190px;overflow-y:auto;\" >");
                docsHtml.Append("<table class='gridData' style=\"margin-top:5px;\"><thead><tr><th width=\"20px\"></th><th width=\"100px\"> Page </th><th width=\"250px\"> Document </th><th width=\"120px\"> Requested On </th><th width=\"60px\">Status</th></tr></thead>");
                for (int i = 0; i < docs.Count; i++)
                {
                    int idx = docs[i].ToString().IndexOf("♣");
                    int idx1 = docs[i].ToString().IndexOf("¿");
                    int idx2 = docs[i].ToString().IndexOf("~");
                    string status = docs[i].ToString().Substring(idx2 + 1);
                    string fileName = docs[i].ToString().Substring(0, idx);
                    docsHtml.Append("<tr id=\"docRow" + i.ToString() + "\">");
                    docsHtml.Append("<td><a id=\"delDoc" + i.ToString() + "\" alt=\"Delete row\" title=\"Delete row\" class=\"rowdelete\"  onclick=\"javascript:RemoveDoc('docRow" + i.ToString() + "','" + fileName + "');\"/></td>");
                    docsHtml.Append("<td>" + docs[i].ToString().Substring(idx + 1, idx1 - (idx + 1)) + "</td>");
                    docsHtml.Append("<td>" + fileName + "</td>");
                    docsHtml.Append("<td>" + docs[i].ToString().Substring(idx1 + 1, idx2 - (idx1 + 1)) + "</td>");
                    if (status == Constants.PROGRESS)
                        docsHtml.Append("<td>" + Constants.PROGRESS + "</td>");
                    else
                        docsHtml.Append("<td><a onclick=\"javascript:OpenDoc('" + fileName + "');\">Open</a></td></tr>");
                }
                docsHtml.Append("</table></div>");
            }
            return docsHtml.ToString();
        }

        /// <summary>
        /// Function to delete the document from session array on click of delete in the print docs pop up. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="docs"></param>
        /// <returns></returns>
        public string DeleteDocFromSession(string fileName, ArrayList docs)
        {
            for (int i = docs.Count - 1; i >= 0; i--)
            {
                int idx = docs[i].ToString().IndexOf("♣");
                string docName = docs[i].ToString().Substring(0, idx);
                if (docName == fileName)
                {
                    docs.RemoveAt(i);
                    break;
                }
            }
            if (HttpContext.Current.Session["project"] == null)
                return SESSTIMEOUT;
            else
            {
                HttpContext.Current.Session["printingDocs"] = docs;
                return "done";
            }
        }

        /// <summary>
        /// Function to create the document name from the axp_printfield and date
        /// </summary>
        /// <param name="selectedDoc"></param>
        /// <param name="tid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDocName(string selectedDoc, string tid, string key)
        {
            string dbFileName = string.Empty;
            string docPrefix = string.Empty;
            TStructData tstData = (TStructData)HttpContext.Current.Session[key];
            int dotIdx = selectedDoc.LastIndexOf(".");
            string fileName = selectedDoc.Substring(0, dotIdx);
            string ext = selectedDoc.Substring(dotIdx);

            if (tstData != null)
            {
                int idx = tstData.GetFldIndex(1, "axprintfile", 0);
                if (idx != -1)
                    docPrefix = tstData.GetFieldValue("1", "axprintfile");
            }

            if (docPrefix == string.Empty)
                docPrefix = DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_";

            TStructDef strObj = GetTstructDefObj("Get structure for", tid);

            //The format of th filename will be  the "document name + axprint field value + transid 
            dbFileName = fileName + "_" + docPrefix + "_" + tid + ext + "♣" + strObj.tstCaption + "¿" + DateTime.Now.ToString();
            if (CheckDupPrintDoc(fileName + "_" + docPrefix + "_" + tid + ext))
            {
                string dateStr = Convert.ToString(DateTime.Now.Hour) + Convert.ToString(DateTime.Now.Minute) + Convert.ToString(DateTime.Now.Second);
                dbFileName = fileName + "_" + docPrefix + "_" + dateStr + tid + ext + "♣" + strObj.tstCaption + "¿" + DateTime.Now.ToString();
            }

            return dbFileName;
        }

        /// <summary>
        /// Function to check if the document name is already available in session.
        /// </summary>
        /// <param name="printDocName"></param>
        /// <returns></returns>
        private bool CheckDupPrintDoc(string printDocName)
        {
            string docName = string.Empty;
            ArrayList printDocs;
            if (HttpContext.Current.Session["printingDocs"] != null)
                printDocs = (ArrayList)HttpContext.Current.Session["printingDocs"];
            else
                printDocs = new ArrayList();

            for (int i = 0; i < printDocs.Count; i++)
            {
                docName = printDocs[i].ToString();
                int idx = docName.IndexOf("♣");
                docName = docName.ToString().Substring(0, idx);
                if (docName == printDocName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Function to get the print docs from the session
        /// </summary>
        /// <returns></returns>
        public ArrayList GetDocsFromSession()
        {
            ArrayList printDocs;
            if (HttpContext.Current.Session["printingDocs"] != null)
                printDocs = (ArrayList)HttpContext.Current.Session["printingDocs"];
            else
                printDocs = new ArrayList();

            return printDocs;
        }

        /// <summary>
        /// Function to get the number of files ready to open and files in progress.
        /// </summary>
        /// <returns></returns>
        public string GetPrintCount(ArrayList printDocs)
        {
            int inProgressCnt = 0;
            int doneCnt = 0;
            string result = string.Empty;
            for (int i = 0; i < printDocs.Count; i++)
            {
                string status = printDocs[i].ToString().Substring(printDocs[i].ToString().LastIndexOf("~") + 1);
                if (status == Constants.PROGRESS)
                    inProgressCnt++;
                else
                    doneCnt++;
            }
            result = "PrintDocs(" + Convert.ToString(doneCnt) + "-" + Convert.ToString(printDocs.Count) + ")";
            return result;
        }

        /// <summary>
        /// Function to get the status of created documents from the total documents in queue for printing.
        /// </summary>
        /// <returns></returns>
        public string GetPrintDocStatus()
        {
            if (HttpContext.Current.Session["nsessionid"] == null)
                return SESSTIMEOUT;
            string filePath = ScriptsPath + "axpert\\" + HttpContext.Current.Session["nsessionid"].ToString();
            DirectoryInfo di = new DirectoryInfo(filePath);
            ArrayList printDocs = GetDocsFromSession();
            int cnt = 0;
            if (di.Exists)
            {
                FileInfo[] fileEntries = di.GetFiles();
                for (int i = 0; i < fileEntries.Length; i++)
                {
                    FileInfo file = (FileInfo)fileEntries[i];
                    for (int j = 0; j < printDocs.Count; j++)
                    {
                        int idx = printDocs[j].ToString().IndexOf("♣");
                        string docName = printDocs[j].ToString().Substring(0, idx);
                        if (docName == file.Name)
                        {
                            int idx1 = printDocs[j].ToString().LastIndexOf("~");
                            printDocs[j] = printDocs[j].ToString().Substring(0, idx1 + 1) + "done";
                            cnt++;
                        }
                    }
                }
            }
            HttpContext.Current.Session["printingDocs"] = printDocs;
            return "PrintDocs(" + cnt.ToString() + "-" + Convert.ToString(printDocs.Count) + ")";
        }

        #endregion

        #region Save As Draft

        /// <summary>
        /// Function to save the file as draft in the axpert\drafts\user folder
        /// </summary>
        /// <param name="transId">transid</param>
        /// <param name="Key">transid + 7 digits unique key</param>
        /// <param name="username">user</param>
        public bool SaveAsDraft(string transId, string Key, string username, string customDraftName)
        {
            bool res = false;
            string rKey = transId + "-" + username + Constants.UNIQUE_DRAFT_KEY_PHRASE + customDraftName + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            LogFile.Log logObj = new LogFile.Log();
            var redisDraftkey = transId + "-" + username + Constants.UNIQUE_DRAFT_KEY_PHRASE + customDraftName;

            try
            {
                TStructData tstData = (TStructData)HttpContext.Current.Session[Key];
                FDW fdwObj = new FDW();
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                var redisDraftkeys = fdrObj.GetAllKeys(schemaName + "-" + redisDraftkey);
                if (redisDraftkeys.Count > 0)
                {
                    fdwObj.DeleteAllKeys(redisDraftkeys[0].ToString(), schemaName);
                    fdwObj.SaveInRedisServer(rKey, tstData, "", schemaName);

                }
                else
                {
                    fdwObj.SaveInRedisServer(rKey, tstData, "", schemaName);
                }
                res = true;
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception while saving draft" + ex.StackTrace, HttpContext.Current.Session["nsessionid"].ToString(), "Exception", "new");
                throw ex;
            }
            return res;
        }

        public bool SaveAsDraftNew(string transId, string Key, string username, string customDraftName)
        {
            bool res = false;
            string rKey = string.Empty;
            if (HttpContext.Current.Session["TstAvoidmultipledraft-" + transId] != null && HttpContext.Current.Session["TstAvoidmultipledraft-" + transId].ToString() == "true")
                rKey = transId + "-" + username + Constants.UNIQUE_DRAFT_KEY_PHRASE + customDraftName;
            else
                rKey = transId + "-" + username + Constants.UNIQUE_DRAFT_KEY_PHRASE + customDraftName + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            LogFile.Log logObj = new LogFile.Log();
            //var redisDraftkey = transId + "-" + username + Constants.UNIQUE_DRAFT_KEY_PHRASE + customDraftName;

            try
            {
                TStructData tstData = (TStructData)HttpContext.Current.Session[Key];
                FDW fdwObj = new FDW();
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                fdwObj.SaveInRedisServer(rKey, tstData, "", schemaName);
                res = true;
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception while saving draft" + ex.StackTrace, HttpContext.Current.Session["nsessionid"].ToString(), "Exception", "new");
                throw ex;
            }
            return res;
        }

        public static void DeletedraftRediskeys(string key)
        {
            try
            {
                if (HttpContext.Current.Session["tstautosaveDraftt"] != null && HttpContext.Current.Session["tstautosaveDraftt"].ToString() == "true")
                {
                    FDW fdwObj = new FDW();
                    FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                    var draftrediskey = (key + "-" + HttpContext.Current.Session["user"].ToString() + Constants.UNIQUE_DRAFT_KEY_PHRASE);
                    var draftallkeys = fObj.GetAllKeys(HttpContext.Current.Session["dbuser"].ToString() + "-" + draftrediskey);
                    if (draftallkeys.Count > 0)
                    {

                        string schemaName = string.Empty;
                        if (HttpContext.Current.Session["dbuser"] != null)
                            schemaName = HttpContext.Current.Session["dbuser"].ToString();
                        fdwObj.DeleteAllKeys(draftallkeys[0].ToString(), schemaName);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void DeletedraftRediskeysNew(string key, string keyNames)
        {
            try
            {
                if (keyNames != string.Empty)
                {
                    FDW fdwObj = new FDW();
                    FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                    foreach (var thisKey in keyNames.Split('♥'))
                    {
                        var draftallkeys = fObj.GetAllKeys(thisKey);
                        if (draftallkeys.Count > 0)
                        {

                            string schemaName = string.Empty;
                            if (HttpContext.Current.Session["dbuser"] != null)
                                schemaName = HttpContext.Current.Session["dbuser"].ToString();
                            fdwObj.DeleteAllKeys(draftallkeys[0].ToString(), schemaName);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void PopOutOldDrafts(int retainCnt, string pattern)
        {
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            ArrayList allDraftKeys = fdrObj.GetAllKeys(pattern);
            allDraftKeys = GetSortedDraftKeys(allDraftKeys);
            FDW fdwObj = new FDW();
            for (int i = retainCnt; i < allDraftKeys.Count; i++)
            {
                //remove these keys in REDIS
                string keyName = allDraftKeys[i].ToString();
                fdwObj.DeleteAllKeys(keyName);
            }
        }

        public ArrayList GetSortedDraftKeys(ArrayList keys)
        {
            ArrayList iKeys = keys;
            Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();
            if (keys.Count > 0)
            {
                try
                {
                    foreach (string key in keys)
                    {
                        string dtStr = key.Substring(key.LastIndexOf('-') + 1, 14);
                        DateTime dt = DateTime.ParseExact(dtStr, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture);
                        dic.Add(key, dt);
                    }
                    dic = dic.OrderByDescending(u => u.Value).ToDictionary(z => z.Key, y => y.Value);
                    if (dic.Count > 0)
                    {
                        keys.Clear();
                        foreach (KeyValuePair<string, DateTime> kvp in dic)
                        {
                            keys.Add(kvp.Key);
                        }
                    }
                }
                catch (Exception ex) { keys = iKeys; }
            }

            return keys;
        }


        public string GetDraftsMarkUp(string transid, string userid, string caption)
        {
            StringBuilder sb = new StringBuilder("");
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            ArrayList lstKeys = fdrObj.GetAllKeys(transid + "-" + userid + Constants.UNIQUE_DRAFT_KEY_PHRASE);
            sb.Append("<ul class=\"dropdown-menu draftUL\">");
            lstKeys = GetSortedDraftKeys(lstKeys);
            if (lstKeys.Count > 0)
            {
                int index = 1;
                foreach (string key in lstKeys)
                {
                    string dtStr = key.Substring(key.LastIndexOf('-') + 1, 14);
                    DateTime dt = DateTime.ParseExact(dtStr, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture);
                    try
                    {
                        int crLen = (key.LastIndexOf('-') - 5) - key.IndexOf(Constants.UNIQUE_DRAFT_KEY_PHRASE);
                        caption = key.Substring(key.IndexOf(Constants.UNIQUE_DRAFT_KEY_PHRASE) + 5, crLen);
                    }
                    catch (Exception ex)
                    { }
                    caption = caption.Replace('♠', '-');
                    sb.Append("<li title=\"" + caption + " - Created On " + dt.ToString() + " \"><a href=\"#\" onclick=\"loadSavedDraft('" + key.ToString() + "')\">" + "" + " " + (caption.Length > 20 ? caption.Substring(0, 19) : caption) + "...</a></li>");
                    index++;
                }
            }
            else
            { sb.Append("<li><a href=\"#\">No Drafts</a></li>"); }
            sb.Append("</ul>");
            return sb.ToString();
        }

        public string GetDraftsMarkUpNew(string transid, string userid, string caption)
        {
            StringBuilder sb = new StringBuilder("");
            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            ArrayList lstKeys = fdrObj.GetAllKeys(transid + "-" + userid + Constants.UNIQUE_DRAFT_KEY_PHRASE);
            if (HttpContext.Current.Session["TstAvoidmultipledraft-" + transid] != null && HttpContext.Current.Session["TstAvoidmultipledraft-" + transid].ToString() == "true")
            {
                sb.Append("<ul style=\"list-style:none;padding:0;\">");
                lstKeys = GetSortedDraftKeys(lstKeys);
                if (lstKeys.Count > 0)
                {
                    sb.Append("<li style=\"display:flex;align-items:center;\"><input type=\"checkbox\" id=\"selectAllDrafts\" onclick=\"toggleAllDrafts(this)\" style=\"margin-right:6px;\" /> <strong>Select All</strong></li>");
                    int index = 1;
                    foreach (string key in lstKeys)
                    {
                        int idx = key.LastIndexOf("-sad-");
                        if (idx >= 0)
                        {
                            caption = key.Substring(idx + 5);
                        }
                        caption = caption.Replace('♠', '-');
                        sb.Append("<li style=\"display:flex;align-items:center;\" title=\"" + caption + " \"><input type=\"checkbox\" class=\"draft-delete-checkbox\" value=\"\" data-keyid='" + key.ToString() + "' style=\"margin-right:6px;\"/> <a href=\"#\" onclick=\"loadSavedDraftNew('" + key.ToString() + "')\">" + caption + " </a></li>");
                        index++;
                    }
                }
                else
                {
                    sb.Append("<li><a href=\"#\">No Drafts</a></li>");
                }
                sb.Append("</ul>");
            }
            else
            {
                sb.Append("<ul style=\"list-style:none;padding:0;\">");
                lstKeys = GetSortedDraftKeys(lstKeys);
                if (lstKeys.Count > 0)
                {
                    sb.Append("<li style=\"display:flex;align-items:center;\"><input type=\"checkbox\" id=\"selectAllDrafts\" onclick=\"toggleAllDrafts(this)\" style=\"margin-right:6px;\" /> <strong>Select All</strong></li>");
                    int index = 1;
                    foreach (string key in lstKeys)
                    {
                        string dtStr = key.Substring(key.LastIndexOf('-') + 1, 14);
                        DateTime dt = DateTime.ParseExact(dtStr, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture);
                        try
                        {
                            int crLen = (key.LastIndexOf('-') - 5) - key.IndexOf(Constants.UNIQUE_DRAFT_KEY_PHRASE);
                            caption = key.Substring(key.IndexOf(Constants.UNIQUE_DRAFT_KEY_PHRASE) + 5, crLen);
                        }
                        catch (Exception ex)
                        { }
                        caption = caption.Replace('♠', '-');
                        sb.Append("<li style=\"display:flex;align-items:center;\" title=\"" + caption + " - Created On " + dt.ToString() + " \"><input type=\"checkbox\" class=\"draft-delete-checkbox\" value=\"\" data-keyid='" + key.ToString() + "' style=\"margin-right:6px;\"/> <a href=\"#\" onclick=\"loadSavedDraftNew('" + key.ToString() + "')\">" + caption + " - Created On " + dt.ToString() + "</a></li>");
                        index++;
                    }
                }
                else
                {
                    sb.Append("<li><a href=\"#\">No Drafts</a></li>");
                }
                sb.Append("</ul>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Function to construct the html to display the list of drafts from the drafts folder.
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="captions"></param>
        /// <returns></returns>
        public string CreateDraftsHtml(ArrayList fileNames, ArrayList captions)
        {
            //filename will be in the format of transidkey~createdtime
            StringBuilder draftsHtml = new StringBuilder();
            if (fileNames.Count == 0)
            {
                draftsHtml.Append("<div id=\"dvPrintMsg\" class=\"info\" style=\"width:'100%';line-height:20px;\">");
                draftsHtml.Append("No Drafts available.</div>");
            }
            else
            {
                draftsHtml.Append("<div class=\"dcTitle\"><center>Drafts</center></div>");
                draftsHtml.Append("<span style=\"float:right;margin-top:-28px;\"><input type=\"button\" onclick=\"javascript:DeleteDraft('')\" value=\"Delete All\"/></span>");
                draftsHtml.Append("<div style=\"height:300px;overflow-y:auto;\" >");
                draftsHtml.Append("<table class='gridData' style=\"margin-top:5px;width:100%;\"><thead><tr><th></th><th> Transaction name </th><th> Created On </th><th> Saved Data </th></tr></thead>");
                for (int i = 0; i < fileNames.Count; i++)
                {
                    int idx1 = fileNames[i].ToString().IndexOf("-");
                    int idx2 = fileNames[i].ToString().IndexOf("~");
                    if (idx1 == -1 || idx2 == -1)
                        continue;
                    string fileStr = fileNames[i].ToString();
                    string displayName = captions[i].ToString();
                    string createdOn = fileStr.Substring(idx2 + 1);
                    string transid = fileStr.Substring(idx1 + 1, idx2 - idx1 - 1);
                    string userName = HttpContext.Current.Session["user"].ToString();

                    Serializer serializer = new Serializer();
                    string fileName = draftsPath + "axpert\\drafts\\" + userName + "\\" + transid + "\\" + fileStr;
                    string savedData = string.Empty;
                    try
                    {
                        var data = serializer.DeSerializeObject(fileName);

                        for (int j = 0; j < data.tstStrObj.flds.Count; j++)
                        {
                            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)data.tstStrObj.flds[j];
                            if (fld.fldframeno != 1)
                                break;
                            if (!fld.visibility && !fld.name.StartsWith("axp_recid"))
                                savedData += data.GetFieldValue("1", fld.name) + ",";
                            if (savedData.Length > 150)
                            {
                                savedData = savedData.Substring(0, 150) + "...";
                                break;
                            }
                        }
                        savedData = savedData.TrimEnd(',');
                    }
                    catch { }

                    draftsHtml.Append("<tr id=\"docRow" + i.ToString() + "\">");
                    draftsHtml.Append("<td><a id=\"delDraft" + i.ToString() + "\" alt=\"Delete draft\" title=\"Delete draft\" class=\"rowdelete\"  onclick=\"javascript:DeleteDraft('" + fileNames[i].ToString() + "');\"/></td>");
                    draftsHtml.Append("<td><a href=\"javascript:LoadDraft('" + fileNames[i].ToString() + "')\">" + displayName + "</a></td>");
                    draftsHtml.Append("<td>" + createdOn + "</td>");
                    draftsHtml.Append("<td>" + savedData + "</td>");
                }
                draftsHtml.Append("</table></div>");
            }
            return draftsHtml.ToString();
        }

        /// <summary>
        /// Function to get the comma seperated transids for passing in the query.
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        public string GetTransIds(ArrayList fileNames)
        {
            StringBuilder strTransids = new StringBuilder();
            for (int i = 0; i < fileNames.Count; i++)
            {
                string[] strFile = fileNames[i].ToString().Split('-');
                if (strTransids.ToString() == string.Empty && strFile[1].ToString().IndexOf('~') > -1)
                    strTransids.Append("'" + strFile[1].Split('~')[0].ToString() + "'");
                else
                    strTransids.Append("," + "'" + strFile[1].Split('~')[0].ToString() + "'");
            }
            return strTransids.ToString();
        }

        /// <summary>
        /// Function to get the filenames from the drafts directory.
        /// </summary>
        /// <returns></returns>
        public ArrayList GetDraftNames()
        {
            ArrayList files = new ArrayList();
            string userName = HttpContext.Current.Session["user"].ToString();

            string draftPath = draftsPath + "axpert\\drafts\\" + userName + "\\";
            DirectoryInfo di = new DirectoryInfo(draftPath);
            if (!di.Exists)
                return files;
            List<FileInfo> fileEntries = new List<FileInfo>();
            foreach (var p in di.GetDirectories())
            {
                for (int i = 0; i < p.GetFiles().Length; i++)
                    fileEntries.Add(p.GetFiles()[i]);
            }
            files = new ArrayList(fileEntries.OrderByDescending(f => f.CreationTime).Select(f => f.Name).ToList());
            return files;
        }

        /// <summary>
        /// Function to delete the given draft from the folder and update the draft arrays in session.
        /// </summary>
        /// <param name="draftId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string DeleteDraft(string draftId, string userName)
        {
            string draftPath = draftsPath + "axpert\\drafts\\" + userName + "\\";
            try
            {
                //To delete all
                if (draftId == string.Empty)
                {
                    DirectoryInfo draftDirectory = new DirectoryInfo(draftPath);
                    draftDirectory.Delete(true);
                }
                //to delete a file
                else if (draftId.IndexOf('-') != -1 && draftId.IndexOf('~') != -1)
                {
                    draftPath += draftId.Split('~')[0].Split('-')[1] + "\\";
                    DirectoryInfo draftDirectory = new DirectoryInfo(draftPath);
                    draftDirectory.Delete(true);
                }
                //to delete after save
                else if (draftId.IndexOf('~') == -1)
                {
                    draftPath += draftId.Split('-')[1] + "\\";
                    DirectoryInfo draftDirectory = new DirectoryInfo(draftPath);
                    draftDirectory.Delete(true);
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            UpdateDraftArrays();
            ArrayList fileNames = (ArrayList)HttpContext.Current.Session["draftFileNames"];
            ArrayList captions = (ArrayList)HttpContext.Current.Session["draftCaptions"];
            return CreateDraftsHtml(fileNames, captions);
        }

        /// <summary>
        /// Function which updates the drafts array in session with newly added or deleted drafts.
        /// </summary>
        /// <param name="draftId">transid-key</param>
        public void UpdateDraftArrays()
        {
            ArrayList fileNames = new ArrayList();
            ArrayList captions = new ArrayList();

            fileNames = GetDraftNames();
            if (fileNames.Count > 0)
            {
                string transIds = GetTransIds(fileNames);
                if (transIds != string.Empty)
                {
                    LogFile.Log logobj = new LogFile.Log();
                    string errorLog = logobj.CreateLog("Get Tstruct captions for Drafts", HttpContext.Current.Session["nsessionid"].ToString(), "GetDraftsCaption", "new");
                    GetTstCaptions(transIds, errorLog, fileNames, captions);
                }
            }

            HttpContext.Current.Session["draftFileNames"] = fileNames;
            HttpContext.Current.Session["draftCaptions"] = captions;
        }

        public void GetTstCaptions(string transIds, string errorLog, ArrayList fileNames, ArrayList captions)
        {

            string iXml = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
            iXml += "<sql>select name,caption from tstructs where name in (" + transIds + ") and blobno = 1</sql>";
            iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            ArrayList tstCaptions = new ArrayList();
            WebServiceExt objExt = new WebServiceExt();
            string result = objExt.CallGetChoiceWS("", iXml);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlNodes = default(XmlNodeList);
            ArrayList arrDDlItems = new ArrayList();
            ArrayList names = new ArrayList();

            xmlDoc.LoadXml(result.ToString());
            xmlNodes = xmlDoc.SelectNodes("//response/row");

            foreach (XmlNode node in xmlNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name.ToLower() == "name")
                        names.Add(childNode.InnerText);
                    else
                        tstCaptions.Add(childNode.InnerText);
                }
            }

            string tmpTransIds = string.Empty;

            for (int i = 0; i < fileNames.Count; i++)
            {
                int idx1 = fileNames[i].ToString().IndexOf("-");
                int idx2 = fileNames[i].ToString().IndexOf("~");
                if (idx1 == -1 || idx2 == -1)
                    continue;
                string transid = fileNames[i].ToString().Substring(idx1 + 1, idx2 - idx1 - 1);

                int idx = names.IndexOf(transid);
                if (idx != -1)
                {
                    captions.Add(tstCaptions[idx].ToString());
                }
                else
                {
                    if (tmpTransIds == string.Empty)
                        tmpTransIds = transid;
                    else
                        tmpTransIds += "," + transid;
                }
            }

            string[] strTransids = tmpTransIds.Split(',');
            for (int k = 0; k < strTransids.Length; k++)
            {
                for (int j = 0; j < fileNames.Count; j++)
                {
                    int indx = fileNames[j].ToString().IndexOf("-");
                    int idx2 = fileNames[j].ToString().IndexOf("~");
                    if (indx == -1 || idx2 == -1)
                        continue;
                    string transid = fileNames[j].ToString().Substring(indx + 1, idx2 - indx - 1);
                    if (transid == strTransids[k].ToString())
                        fileNames.RemoveAt(j);
                }
            }
        }

        #endregion

        #region Datacache

        public void SaveInDataCache(string transId, string Key)
        {
            TStructData tstData = (TStructData)HttpContext.Current.Session[Key];
            Serializer serializer = new Serializer();
            string dirName = CachePath + "datacache\\" + transId + "\\" + tstData.recordid;
            string fileName = dirName + "\\" + tstData.recordid + "-dataobj";

            DirectoryInfo di = new DirectoryInfo(dirName);
            LogFile.Log logObj = new LogFile.Log();

            try
            {
                if (!di.Exists) di.Create();
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception while creating data obj in cache" + ex.StackTrace + "------Path-" + fileName, HttpContext.Current.Session["nsessionid"].ToString(), "Exception", "new");
            }

            try
            {
                serializer.SerializeObject(fileName, tstData);
            }
            catch (Exception ex)
            {
                logObj.CreateLog("Exception while creating data obj in cache" + ex.StackTrace + "------Path-" + fileName, HttpContext.Current.Session["nsessionid"].ToString(), "Exception", "new");
                throw ex;
            }
        }

        public void WriteToFile(string dirPath, string filename, string data)
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);
            //' Determine whether the directory exists.
            if (!di.Exists)
            {
                di.Create();
            }

            string filepath = dirPath + "\\" + filename + ".txt";

            StreamWriter sw = default(StreamWriter);
            sw = new StreamWriter(filepath, false);
            //True for appending
            sw.WriteLine(data);
            //Close the file.
            sw.Flush();
            sw.Close();
        }

        public void WriteToFileExt(string dirPath, string filename, string data)
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);
            //' Determine whether the directory exists.
            if (!di.Exists)
            {
                di.Create();
            }

            string filepath = dirPath + "\\" + filename + ".txt";
            bool fileExists = false;
            StreamWriter sw = default(StreamWriter);
            if (File.Exists(filepath))
            {
                fileExists = true;
            }
            using (sw = new StreamWriter(filepath, true))
            {
                if (!fileExists)
                {
                    sw.WriteLine("Sid,tid,rid,Service,startTime,cl1,wt1,asb,wt2,cl2,ASBTot,ASBDB");
                }
                //True for appending
                sw.WriteLine(data);
                //Close the file.
                sw.Flush();
                sw.Close();
            }
        }

        /// Function to read data from a text file
        public string ReadFromFile(string dirPath, string filename)
        {
            string result = string.Empty;
            string filepath = dirPath + "\\" + filename + ".txt";
            System.IO.StreamReader sr = default(System.IO.StreamReader);
            if (File.Exists(filepath))
            {
                sr = System.IO.File.OpenText(filepath);
                result = sr.ReadToEnd();
                sr.Close();
            }
            return result;
        }

        /// Function to read data from a file.
        public string ReadFromFile(string filePath)
        {
            string result = string.Empty;
            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(filePath);
                if (file.Exists)
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
                    {
                        result = reader.ReadToEnd();
                    }
                    return result;
                }
            }
            catch (Exception ex) { }
            return result;
        }

        /// <summary>
        /// Function to delete the records stored in cache from listview when multiple records are selected and deleted.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="recIds"></param>
        /// <param name="transId"></param>
        public void DeleteRecordsFromCache(string result, string recIds, string transId)
        {
            string[] strRecIds = recIds.Split(',');
            for (int i = 0; i < strRecIds.Length; i++)
            {
                if (strRecIds[i] != string.Empty)
                {
                    if (IsFileInCache(transId, strRecIds[i].ToString()))
                        DeleteFromCache(result, strRecIds[i].ToString(), transId);
                }
            }
        }

        public Boolean IsFileInCache(string transid, string recId)
        {
            bool isFileCached = false;
            DirectoryInfo di = new DirectoryInfo(CachePath + "datacache\\" + transid + "\\" + recId);
            //The getfiles length is checked with 3 as all the three files should be available in cache.
            if (di.Exists && di.GetFiles().Length == 3)
                isFileCached = true;
            return isFileCached;
        }

        public void DeleteFromCache(string result, string recId, string transId)
        {
            string msg = ParseMessageNode(result);
            //TODO: message node contains comma seperated messages for delete. for example - done, done
            //Weather to use this or get it fixed in web service.
            msg = msg.ToLower();
            if (msg == "done" || msg == "data deleted successfully" || msg.Contains("done"))
            {
                DirectoryInfo di = new DirectoryInfo(CachePath + "datacache\\" + transId + "\\" + recId);
                //di.delete(true) will recurssively delete all files and folders inside the directory.
                if (di.Exists)
                    di.Delete(true);
            }
        }

        private string ParseMessageNode(string result)
        {
            string msgStr = string.Empty;
            JArray msgNode = null;
            try
            {
                JObject tstData = JObject.Parse(result);
                msgNode = (JArray)tstData["message"];
            }
            catch (Exception ex)
            {

            }
            if (msgNode != null)
            {
                for (int i = 0; i < msgNode.Count; i++)
                {
                    Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(msgNode[i].ToString());

                    for (int k = 0; k < values.Count; k++)
                    {
                        if (values.ContainsKey("msg"))
                            msgStr += values["msg"];

                    }
                }

            }
            return msgStr;
        }

        public Boolean IsTstructCached(string transid)
        {
            Boolean isTstCached = false;

            if (!string.IsNullOrEmpty(HttpContext.Current.Application["AxLoadFromCache"].ToString()))
            {
                string transIds = HttpContext.Current.Application["AxLoadFromCache"].ToString();
                if (transIds == "*") isTstCached = true;

                string[] strTst = transIds.Split(',');
                for (int i = 0; i < strTst.Length; i++)
                {
                    if (strTst[i].ToString() == transid)
                    {
                        isTstCached = true;
                        break;
                    }
                }
            }
            return isTstCached;
        }

        #endregion

        #region TStruct Navigation

        public string GetNavigationRecord(string nxtprv, string recordId, string transactionId, string columnName, int dataRowIndex, bool isParentIview)
        {

            if (isParentIview)
            {
                if (!string.IsNullOrEmpty(columnName) && HttpContext.Current.Session["iNavigationInfoTable"] != null && dataRowIndex >= 0)
                    return GetNavigationRecordByTable(isParentIview, (DataTable)HttpContext.Current.Session["iNavigationInfoTable"], nxtprv, dataRowIndex, columnName);
                else
                    return string.Empty;
            }
            else
            {
                if (!string.IsNullOrEmpty(columnName) && HttpContext.Current.Session["navigationInfoTable"] != null
                    && dataRowIndex >= 0 && columnName != "recordid" && (HttpContext.Current.Session["recordTransId"] == null
                    || HttpContext.Current.Session["recordTransId"].ToString() != transactionId))
                {
                    return GetNavigationRecordByTable(isParentIview, (DataTable)HttpContext.Current.Session["navigationInfoTable"], nxtprv, dataRowIndex, columnName);
                }
                else if (HttpContext.Current.Session["lstRecordIds"] != null && HttpContext.Current.Session["recordTransId"] != null
                    && HttpContext.Current.Session["recordTransId"].ToString() == transactionId)
                {
                    List<string> lstRecordIds = (List<string>)HttpContext.Current.Session["lstRecordIds"];
                    int indxRecordId = lstRecordIds.IndexOf(recordId);
                    if (indxRecordId == -1)
                        return recordId;

                    else if (indxRecordId == 0 && nxtprv == "prev")
                        return "This is the first record in the page.";

                    else if (indxRecordId == lstRecordIds.Count - 1 && nxtprv == "next")
                        return "This is the last record in the page.";

                    if (nxtprv == "prev")
                        return lstRecordIds[indxRecordId - 1].ToString();
                    else
                        return lstRecordIds[indxRecordId + 1].ToString();
                }
                else if (HttpContext.Current.Session["lstRecordIds"] != null &&
                    HttpContext.Current.Session["recordTransId"].ToString() != transactionId)
                {
                    return recordId;
                }
                else
                {
                    return recordId;
                }
            }
        }

        public string GetNavigationRecordByTable(bool isParentIview, DataTable navigationInfo, string nxtprv, int dataRowIndex, string columnName)
        {
            string data = string.Empty;
            if (nxtprv == "next" && navigationInfo.Rows.Count == dataRowIndex + 1)
                return "This is the last record in the page.";
            else if (nxtprv == "prev" && dataRowIndex == 0)
                return "This is the first record in the page.";

            int i = dataRowIndex;
            //return next querystring if querystring in datatable is empty
            if (nxtprv == "next")
            {
                while (string.IsNullOrEmpty(data))
                {
                    //If querystring of last record is empty
                    if (navigationInfo.Rows.Count <= i + 1)
                        return "This is the last record in the page.";
                    data = navigationInfo.Rows[i + 1][columnName].ToString();
                    i++;
                }
            }
            else
            {
                //return the record for which querystring is not empty
                while (string.IsNullOrEmpty(data))
                {
                    //If query string of 1st record is empty
                    if (i <= 0)
                        return "This is the first record in the page.";
                    data = navigationInfo.Rows[i - 1][columnName].ToString();
                    i--;
                }
            }
            return data + "*¿*" + i;
        }


        public bool IsValidQueryString(string url, bool skipBracket = false)
        {
            string[] invalidtypes = null;

            if (skipBracket)
            {
                invalidtypes = new string[] { "/>" };
            }
            else
            {
                invalidtypes = new string[] { "/>", "[", "]" };
            }

            url = CheckReverseUrlSpecialChars(url);
            bool IsValid = true;
            foreach (string a in invalidtypes)
            {
                if (url.Contains(a))
                {
                    IsValid = false;
                    break;
                }
            }
            return IsValid;
        }




        #endregion

        #region Back and Forward button
        /// <summary>
        /// This function is to add the loaded page url to the session variable to make Back and Forward buttons working.
        /// </summary>
        public void UpdateNavigateUrl(string url)
        {
            //int urlIndex = -1;
            //string result = string.Empty;
            //if (HttpContext.Current.Session["allUrls"] != null)
            //    allUrls = (List<string>)HttpContext.Current.Session["allUrls"];
            //if (HttpContext.Current.Session["urlIndex"] != null)
            //    urlIndex = (int)HttpContext.Current.Session["urlIndex"];


            //if (allUrls.Count >= capacity)
            //    allUrls.RemoveAt(0);

            //bool fromHyperLink = false;
            //if (HttpContext.Current.Session["AxFromHypLink"] != null)
            //{
            //    if (HttpContext.Current.Session["AxFromHypLink"].ToString().ToLower() == "true")
            //    {
            //        fromHyperLink = true;
            //        HttpContext.Current.Session["AxFromHypLink"] = "false";
            //    }
            //}

            //bool tstSaveUrl = false;
            //if (HttpContext.Current.Session["axp_IsSaveUrl"] != null)
            //{
            //    if (HttpContext.Current.Session["axp_IsSaveUrl"].ToString() == "true")
            //    {
            //        tstSaveUrl = true;
            //        HttpContext.Current.Session["axp_IsSaveUrl"] = "false";
            //    }
            //}

            //int idx = url.IndexOf("?");
            //int idx1 = url.IndexOf("&");
            //string subStr = string.Empty;
            //string strSamePage = string.Empty;
            //string strLastPage = string.Empty;
            //if (idx != -1)
            //{
            //    try
            //    {
            //        if (url.Contains("transid="))
            //            subStr = url.Substring(url.IndexOf("transid=")).Split('=')[1].Split('&')[0];
            //        else if (url.Contains("ivname="))
            //            subStr = url.Substring(url.IndexOf("ivname=")).Split('=')[1].Split('&')[0];
            //        else if (idx1 != -1)
            //            subStr = url.Substring(idx, idx1 - idx);
            //    }
            //    catch { }

            //    strSamePage = url.Split('?')[0].ToString();
            //}

            //string strOldPageName = String.Empty;
            //if (allUrls.Count > 0 && allUrls[allUrls.Count - 1].Split('♣').Last().Split('=').Length > 1)
            //{
            //    try
            //    {
            //        string lastURL = allUrls[allUrls.Count - 1].Split('♣').Last();
            //        if (lastURL.Contains("transid="))
            //            strOldPageName = lastURL.Substring(lastURL.IndexOf("transid=")).Split('=')[1].Split('&')[0];
            //        else if (lastURL.Contains("ivname="))
            //            strOldPageName = lastURL.Substring(lastURL.IndexOf("ivname=")).Split('=')[1].Split('&')[0];
            //        else
            //            strOldPageName = lastURL.Split('=')[1].Split('&')[0].ToString();

            //        strLastPage = lastURL.Split('?')[0].ToString();
            //    }
            //    catch { }
            //}

            //if (allUrls.Count() == 0 || ((strOldPageName != String.Empty && subStr != String.Empty && strOldPageName != subStr) || (allUrls[allUrls.Count - 1].IndexOf(strSamePage) == -1 || allUrls[allUrls.Count - 1].IndexOf(strSamePage) == 0)))
            //{
            //    bool hasUrl = false;
            //    if ((urlIndex != -1 && allUrls.Count > 0) && (strSamePage == strLastPage & !string.IsNullOrEmpty(subStr) && allUrls[allUrls.Count - 1].ToString().Contains(subStr) || allUrls[allUrls.Count - 1].ToString() == url))
            //        hasUrl = true;

            //    if (fromHyperLink || hasUrl)
            //    {
            //        if (fromHyperLink)
            //            url = AppendNavUrl(allUrls, url, hasUrl);
            //        if (allUrls.Count != 0)
            //        {
            //            if (url.Contains("iview.aspx") || url.Contains("ivtoivload.aspx"))
            //            {
            //                allUrls[allUrls.Count - 1] = CheckByIviewNameThenAppend(allUrls, url);
            //            }
            //            else
            //            {
            //                allUrls[allUrls.Count - 1] = url;
            //            }
            //        }
            //        else
            //            allUrls.Add(url);
            //        urlIndex = allUrls.Count - 1;
            //    }
            //    else
            //    {
            //        if (hasUrl || tstSaveUrl)
            //            allUrls[allUrls.Count - 1] = url;
            //        else
            //            allUrls.Add(url);
            //        urlIndex = allUrls.Count - 1;
            //    }

            //    HttpContext.Current.Session["urlIndex"] = urlIndex;
            //    HttpContext.Current.Session["allUrls"] = allUrls;
            //    HttpContext.Current.Session["enableBackButton"] = true;
            //    HttpContext.Current.Session["enableForwardButton"] = true;
            //}
        }
        private string CheckByIviewNameThenAppend(List<string> allUrls, string url)
        {
            string newUrl = "";
            string oldUrl = string.Empty;
            if (allUrls.Count > 0)
            {
                oldUrl = allUrls[allUrls.Count - 1].ToString();

                string[] oldUrls = oldUrl.Split('♣');
                if (oldUrls.Length > 1)
                {

                    string oldd = GetUrlIviewName(oldUrls[oldUrls.Length - 1]);
                    string neww = GetUrlIviewName(url);
                    if (oldd == neww)
                    {
                        oldUrls[oldUrls.Length - 1] = url;

                        for (int i = 0; i < oldUrls.Length; i++)
                        {
                            if (newUrl == string.Empty)
                                newUrl = oldUrls[i].ToString();
                            else
                                newUrl += "♣" + oldUrls[i].ToString();
                        }
                    }
                    else if (allUrls[allUrls.Count - 1].Contains("ivtoivload.aspx?ivname=" + neww))
                    {
                        newUrl = url;
                    }
                    else
                    {
                        if (url.Contains(oldUrl))
                            newUrl = url;
                        else
                            newUrl = oldUrl + "♣" + url;
                    }

                }
                else
                {
                    newUrl = url;
                }
            }

            return newUrl;
        }

        private string GetUrlIviewName(string mainUrl)
        {
            string[] oldUrls = mainUrl.Split('♣');
            int startIndex = oldUrls.Last().IndexOf("ivname=") + 7;
            int endIndex = oldUrls.Last().IndexOf("&", startIndex);
            if (endIndex == -1) endIndex = oldUrls.Last().Length;
            string ivname = oldUrls.Last().Substring(startIndex, endIndex - startIndex);
            return ivname;
        }

        private string AppendNavUrl(List<string> allUrls, string url, bool hasUrl)
        {
            string newUrl = "";
            string oldUrl = string.Empty;
            if (allUrls.Count > 0)
                oldUrl = allUrls[allUrls.Count - 1].ToString();
            if (hasUrl)
            {
                //if the new url is the same page as the last item, then update the last item and return the newly constructed url
                string[] oldUrls = oldUrl.Split('♣');
                oldUrls[oldUrls.Length - 1] = newUrl;

                for (int i = 0; i < oldUrls.Length - 1; i++)
                {
                    if (newUrl == string.Empty)
                        newUrl = oldUrls[i].ToString();
                    else
                        newUrl += "♣" + oldUrls[i].ToString();
                }
            }
            else
            {
                newUrl = oldUrl + "♣" + url;
            }
            return newUrl;

        }
        #endregion


        public void ClearSession()
        {

            HttpContext.Current.Session["lstRecordIds"] = null;
            HttpContext.Current.Session["recordTransId"] = null;
            HttpContext.Current.Session["navigationInfoTable"] = null;
            HttpContext.Current.Session["iNavigationInfoTable"] = null;
            HttpContext.Current.Session["currentPageNo"] = null;
            //LV Navigation Details
            ArrayList sessKeysDel = new ArrayList();
            try
            {
                var sessKeys = HttpContext.Current.Session.Keys;
                foreach (string SessionVariable in sessKeys)
                {
                    if (SessionVariable.StartsWith("lvRecordListing-"))
                    {
                        sessKeysDel.Add(SessionVariable);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            foreach (string SessionVariable in sessKeysDel)
            {
                HttpContext.Current.Session[SessionVariable] = null;
            }
            HttpContext.Current.Session["lvPageNo"] = null;
            HttpContext.Current.Session["lvPageSize"] = null;
            HttpContext.Current.Session["lvRecPos"] = null;
            HttpContext.Current.Session["lvTotalRows"] = null;
            HttpContext.Current.Session["lvRecordKey"] = null;


        }

        #region axapps
        public string CheckForAvailableProjects()
        {
            string axAvailProjs = string.Empty;
            try
            {
                FileInfo fi = new FileInfo(ScriptsPath + "\\axapps.xml");
                if (fi.Exists)
                {
                    XDocument doc;
                    FileStream file = new FileStream(ScriptsPath + "\\axapps.xml", FileMode.Open, FileAccess.ReadWrite);
                    using (StreamReader reader = new StreamReader(file))
                    {
                        doc = XDocument.Load(reader);
                        reader.Close();
                        if (doc.Element("connections").Elements().Count() >= 1)
                        {
                            axAvailProjs += "Select Project,";
                            foreach (string s in doc.Element("connections").Elements().Select(s => s.Name.LocalName).ToList())
                                axAvailProjs += s + ",";
                            file.Close();
                            return axAvailProjs.TrimEnd(',');
                        }
                    }
                    file.Close();
                    file.Dispose();
                }
                return axAvailProjs;
            }
            catch (Exception ex)
            {
                return axAvailProjs;
            }
        }

        public string ServiceInputSpecialChars(string value)
        {
            value = value.Replace("&", "&amp;");
            value = value.Replace("%", "&perc;");
            value = value.Replace(">", "&gt;");
            value = value.Replace("<", "&lt;");
            value = value.Replace("=", "&equalto;");
            //value = value.Replace("\"", "&quot;");
            return value;
        }

        public void GetAxApps(string strProj)
        {
            string xmlcontents = string.Empty;
            string contents = string.Empty;
            HttpContext.Current.Session["axdb"] = "Oracle";
            string db = string.Empty;
            if (strProj != string.Empty)
            {
                FDR fdrObj = new FDR(strProj);
                xmlcontents = fdrObj.StringFromRedis(Constants.AXAPPS_XML_KEY, strProj);
                if (xmlcontents == string.Empty)
                {
                    FileInfo fi = new FileInfo(ScriptsPath + "\\axapps.xml");
                    if (fi.Exists)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(ScriptsPath + "\\axapps.xml");
                        XmlNodeList pNode = doc.SelectNodes("/connections/" + strProj);
                        xmlcontents = pNode[0].OuterXml;
                        updateAppSettingsini(strProj, xmlcontents);
                        FDW fdwObj = new FDW(strProj);
                        fdwObj.SaveInRedisServer(Constants.AXAPPS_XML_KEY, xmlcontents, Constants.AXAPPS_XML_KEY, strProj);
                    }
                }
            }

            if (xmlcontents != string.Empty)
            {
                int dbIdx = -1;
                System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(new System.IO.StringReader(xmlcontents));
                bool startRead = false;
                string lastEmptyNode = string.Empty;
                while (reader.Read())
                {
                    reader.MoveToContent();
                    if (reader.NodeType == System.Xml.XmlNodeType.Element && reader.Name == strProj)
                    {
                        startRead = true;
                        contents += "<" + reader.Name + ">";
                    }
                    else if (reader.NodeType == System.Xml.XmlNodeType.Element && startRead == true)
                    {
                        if (reader.Name == "db")
                        {
                            dbIdx = 0;
                        }
                        contents += "<" + reader.Name + ">";
                        if (reader.IsEmptyElement)
                        {
                            lastEmptyNode = reader.Name;
                            contents += "</" + reader.Name + ">";
                        }
                    }

                    if (reader.NodeType == System.Xml.XmlNodeType.Text && startRead == true)
                    {
                        if (dbIdx != -1)
                        {
                            db = reader.Value;
                            dbIdx = -1;
                        }
                        contents += reader.Value + "";
                    }
                    else
                    {
                        if (reader.Name == lastEmptyNode)
                        {
                            lastEmptyNode = string.Empty;
                            continue;
                        }
                        if (reader.NodeType == System.Xml.XmlNodeType.EndElement && reader.Name == strProj)
                        {
                            startRead = false;
                            contents += "</" + reader.Name + ">";
                            break;
                        }
                        else if (reader.NodeType == System.Xml.XmlNodeType.EndElement && startRead == true) //Display the end of the element.
                            contents += "</" + reader.Name + ">";
                    }

                }
                reader.Close();
            }

            HttpContext.Current.Session["axdb"] = db;
            if (contents != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(contents);
                XmlNodeList dbuser = xmlDoc.GetElementsByTagName("dbuser");
                XmlNodeList dbtype = xmlDoc.GetElementsByTagName("db");
                string appPWD = string.Empty;
                if (dbtype[0].InnerText.ToLower() == "postgresql" || dbtype[0].InnerText.ToLower() == "postgre")
                {
                    appPWD = dbuser[0].InnerText;
                    appPWD = appPWD.Split('\\')[0];
                    appPWD = GetDBPasswordService(appPWD);
                }
                else
                    appPWD = GetDBPasswordService(dbuser[0].InnerText);
                if (xmlDoc.DocumentElement.SelectSingleNode("pwd") == null)
                {
                    XmlElement pwd = xmlDoc.CreateElement("pwd");
                    xmlDoc.DocumentElement.AppendChild(pwd);
                }
                xmlDoc.DocumentElement.SelectSingleNode("pwd").InnerText = appPWD;
                contents = xmlDoc.OuterXml;
            }
            HttpContext.Current.Session["axApps"] = contents;
            GetDBConnection(strProj, contents);
        }

        public string GetDBPasswordService(string username)
        {
            string pwd = "";
            LogFile.Log logobj = new LogFile.Log();

            string fileName = ScriptsPath + username + ".pwd";
            try
            {
                if (File.Exists(fileName))
                {
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        String encryptedPWD = sr.ReadToEnd();
                        pwd = encryptedPWD.Trim();
                        sr.Close();
                        sr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Exception in getting password :--- " + ex.Message.ToString(), "", "GetDBPasswordService", "New");
            }
            return pwd;
        }


        public void GetDBConnection(string strProj, string contents)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable(strProj);
            dataTable.Columns.Add("type", typeof(string));
            dataTable.Columns.Add("db", typeof(string));
            dataTable.Columns.Add("version", typeof(string));
            dataTable.Columns.Add("driver", typeof(string));
            dataTable.Columns.Add("dbcon", typeof(string));
            dataTable.Columns.Add("dbuser", typeof(string));
            dataTable.Columns.Add("structurl", typeof(string));
            dataTable.Columns.Add("dataurl", typeof(string));
            dataTable.Columns.Add("pwd", typeof(string));
            dataSet.Tables.Add(dataTable);
            System.IO.StringReader xmlSR = new System.IO.StringReader(contents);
            dataSet.ReadXml(xmlSR, XmlReadMode.IgnoreSchema);
            string dbuser = dataSet.Tables[0].Rows[0]["dbuser"].ToString();
            string dbtype = dataSet.Tables[0].Rows[0]["db"].ToString();
            string pwd = dataSet.Tables[0].Rows[0]["pwd"].ToString();
            string Server = dataSet.Tables[0].Rows[0]["dbcon"].ToString();
            string database = dataSet.Tables[0].Rows[0]["dbuser"].ToString();
            string version = dataSet.Tables[0].Rows[0]["version"].ToString().Trim();

            string dbuserALC = dbuser;
            string cs = "";

            if (dbtype.ToLower() != "postgresql" && dbtype.ToLower() != "postgre")
                pwd = GetDBPassword(database);

            if (dbtype.ToLower() == "ms sql" || dbtype.ToLower() == "mssql")
            {
                if (version.ToUpper() == "ABOVE 2012")
                    Server = GetServerIpAndPort(Server, dbuser, pwd);
                //Server = GetODBCDataSourceName(Server, dbuser, pwd);
                cs = @"Server=" + Server + "; Database=" + database + "; User Id=" + dbuser + "; Password=" + pwd + ";";
                dbuserALC = dbuser;
            }
            if (dbtype.ToLower() == "oracle")
            {
                cs = @"Data Source=" + Server + ";User Id=" + dbuser + ";Password=" + pwd + ";Pooling=False;";
                dbuserALC = dbuser;
            }
            if (dbtype.ToLower() == "mysql" || dbtype.ToLower() == "mariadb")
            {
                if (Server.Contains(":"))
                {
                    //Default port for mysql or mariadb is 3306. if it was changed we need to pass the port no seperately in the Conn Str. 
                    string serverPort = Server.Substring(Server.IndexOf(':') + 1);
                    cs = @"Server=" + Server.Substring(0, Server.IndexOf(':')) + "; Port=" + serverPort + "; Database=" + database + ";Uid=" + dbuser + ";Pwd=" + pwd + ";";
                }
                else
                {

                    cs = @"Server=" + Server + ";Database=" + database + ";Uid=" + dbuser + ";Pwd=" + pwd + ";";
                }
                dbuserALC = dbuser;
            }
            if (dbtype.ToLower() == "postgresql" || dbtype.ToLower() == "postgre")
            {
                if (Server.Contains(":"))
                {
                    if (dbuser.Contains("\\") || database.Contains("\\"))
                    {

                        string[] userDtls = dbuser.Split('\\');
                        string[] databaseDtls = database.Split('\\');
                        if (userDtls.Length > 1 && userDtls[1] != "")
                            if ((userDtls.Length > 1 && userDtls[1] != "") && (databaseDtls.Length > 1 && databaseDtls[1] != ""))
                            {
                                dbuser = userDtls[0];
                                database = databaseDtls[1];
                            }
                            else
                            {
                                dbuser = userDtls[0];
                                database = databaseDtls[0];
                            }
                        dbuserALC = dbuser + "~" + database;
                    }
                    else
                    {
                        database = "axpertdb";
                        dbuserALC = dbuser;
                    }
                    //Default port for postgres is 5432. if it was changed we need to pass the port no seperately in the Conn Str. 
                    pwd = GetDBPassword(dbuser);
                    string serverPort = Server.Substring(Server.IndexOf(':') + 1);
                    cs = @"Server=" + Server.Substring(0, Server.IndexOf(':')) + "; Port=" + serverPort + "; Database=" + database + ";Uid=" + dbuser + ";Pwd=" + pwd + ";Pooling=false;";
                    dbuser = dbuser + "~" + database;
                }
                else
                {
                    //int start_index = dbuser.IndexOf('\\');
                    //start_index = start_index + 1;
                    //dbuser = dbuser.Substring(start_index, dbuser.Length-1);
                    if (dbuser.Contains("\\") || database.Contains("\\"))
                    {
                        string[] userDtls = dbuser.Split('\\');
                        string[] databaseDtls = database.Split('\\');
                        if ((userDtls.Length > 1 && userDtls[1] != "") && (databaseDtls.Length > 1 && databaseDtls[1] != ""))
                        {
                            dbuser = userDtls[0];
                            database = databaseDtls[1];
                        }
                        else
                        {
                            dbuser = userDtls[0];
                            database = databaseDtls[0];
                        }
                        dbuserALC = dbuser + "~" + database;
                    }
                    else
                    {
                        database = "axpertdb";
                        dbuserALC = dbuser;
                    }
                    pwd = GetDBPassword(dbuser);
                    cs = @"Server=" + Server + ";Database=" + database + ";Uid=" + dbuser + ";Pwd=" + pwd + ";Pooling=false;";
                    dbuser = dbuser + "~" + database;
                }
            }

            HttpContext.Current.Session["dbuser"] = dbuser;
            HttpContext.Current.Session["axconstr"] = cs;
            HttpContext.Current.Session["axdb"] = dbtype;
            HttpContext.Current.Session["dbuserALC"] = dbuserALC;
        }

        public string GetODBCDataSourceName(string serverName, string uid, string pwd)
        {
            string connStr = "DSN=" + serverName + ";Uid=" + uid + ";Pwd=" + pwd + ";";
            string dataSourceName = String.Empty;
            using (OdbcConnection odbcConn = new OdbcConnection(connStr))
            {
                try
                {
                    odbcConn.Open();
                    dataSourceName = odbcConn.DataSource.ToString();
                }
                catch (Exception e)
                {
                    return serverName;
                }
                finally
                {
                    odbcConn.Close();
                    odbcConn.Dispose();
                }
            }

            if (!string.IsNullOrEmpty(dataSourceName))
                return dataSourceName;
            else
                return serverName;
        }

        public static string GetServerIpAndPort(string serverName, string uid, string pwd)
        {
            string connStr = "DSN=" + serverName + ";Uid=" + uid + ";Pwd=" + pwd + ";";
            string dataSourceName = string.Empty;

            using (OdbcConnection odbcConn = new OdbcConnection(connStr))
            {
                try
                {
                    odbcConn.Open();
                    try
                    {
                        using (OdbcCommand cmd = odbcConn.CreateCommand())
                        {
                            cmd.CommandText = @"
                            SELECT local_net_address, local_tcp_port 
                            FROM sys.dm_exec_connections 
                            WHERE session_id = @@SPID";

                            using (OdbcDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string ip = reader["local_net_address"].ToString();
                                    string port = reader["local_tcp_port"].ToString();
                                    dataSourceName = ip + "\\" + serverName + "," + port;
                                }
                            }
                        }
                    }
                    catch
                    {
                        string ds = GetServerFromDsn(serverName);
                        dataSourceName = !string.IsNullOrEmpty(ds) ? ds : serverName;
                    }
                }
                catch
                {
                    return serverName;
                }
                finally
                {
                    odbcConn.Close();
                    odbcConn.Dispose();
                }
            }

            return !string.IsNullOrEmpty(dataSourceName) ? dataSourceName : serverName;
        }

        private static string GetServerFromDsn(string dsnName)
        {
            string result = string.Empty;
            string[] keys = new string[]
            {
        @"SOFTWARE\ODBC\ODBC.INI\" + dsnName,
        @"SOFTWARE\WOW6432Node\ODBC\ODBC.INI\" + dsnName
            };

            foreach (string key in keys)
            {
                using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(key))
                {
                    if (regKey != null)
                    {
                        object server = regKey.GetValue("Server");
                        if (server != null)
                        {
                            result = server.ToString();
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public void updateAppSettingsini(string ConName, string axappsnodes)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(axappsnodes);
                string dbtype = string.Empty, driver = string.Empty, dbverno = string.Empty, dbconName = string.Empty, _userName = string.Empty, pwd = string.Empty;
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    if (node.Name == "db")
                        dbtype = node.InnerText.Trim();
                    else if (node.Name == "driver")
                        driver = node.InnerText.Trim();
                    else if (node.Name == "version")
                        dbverno = node.InnerText.Trim();
                    else if (node.Name == "dbcon")
                        dbconName = node.InnerText.Trim();
                    else if (node.Name == "dbuser")
                        _userName = node.InnerText.Trim();
                    else if (node.Name == "pwd")
                        pwd = node.InnerText.Trim();
                }
                var propertiesDict = new Dictionary<string, object>
                {
                { "type", "db" },
                { "structurl", "" },
                { "db", dbtype },
                { "driver", driver },
                { "version", dbverno },
                { "dbcon", dbconName },
                { "dbuser", _userName },
                { "pwd", pwd },
                { "dataurl", "" }
                };
                var propData = new Dictionary<string, object> {
                { ConName, propertiesDict }
                };
                var mainDict = new Dictionary<string, object>
                {
                { "appconnections", propData }
                };
                string jsonString = JsonConvert.SerializeObject(mainDict);
                string ScriptsPathApp = HttpContext.Current.Application["ScriptsPath"].ToString() + "AppSettings.ini";
                string filePath = @" " + ScriptsPathApp + "";
                string directoryPath = Path.GetDirectoryName(filePath);
                FileInfo Filefi = new FileInfo(ScriptsPathApp);
                if (!Filefi.Exists)
                {
                    File.WriteAllText(ScriptsPathApp, jsonString);
                }
                else
                {
                    string existingJson = File.ReadAllText(filePath);
                    if (existingJson != "")
                    {
                        JObject json = JObject.Parse(existingJson);
                        JObject newData = JObject.Parse(jsonString);
                        json.Merge(newData, new JsonMergeSettings
                        {
                            MergeArrayHandling = MergeArrayHandling.Union
                        });
                        File.WriteAllText(filePath, json.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion


        #region Filter Grid Data

        /// <summary>
        /// To get visible column and data of grid as JSon
        /// </summary>
        /// <param name="dt">DataTable that is binded with grid</param>
        /// <param name="headName">Visible Column Name</param>
        /// <param name="colFld">Column Name in datatable</param>
        /// <returns>Json string</returns>
        public string DataTableToJson(DataTable dt, ArrayList headName, ArrayList colFld, ArrayList colHide, Array rowsToExclude, string typeColumn, string ColsToFilter, int pageSize, out int noOfRows)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string[] colToInclude = new string[] { };
            ArrayList arrFilterCols = new ArrayList();
            int colCnt = 0;
            if (ColsToFilter != string.Empty)
            {
                colToInclude = ColsToFilter.ToLower().Split(',');
                for (int i = 0; i < colToInclude.Length; i++)
                {
                    arrFilterCols.Add(colToInclude[i]);
                }
            }
            int rowsCount = 0;
            if (dt != null)
            {
                StringBuilder distVals = new StringBuilder();
                distVals.Append("{");
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    //Below condition allows only columns in axp_filtercolumns or first 5 visible columns
                    if (arrFilterCols.Count == 0)
                    {
                        if (colCnt >= 5)
                            break;
                    }
                    else if (arrFilterCols.IndexOf(dt.Columns[k].ColumnName) == -1)
                    {
                        continue;
                    }

                    List<string> colData = new List<string>();
                    DataColumn col = dt.Columns[k];
                    if (col.ToString() == "rowno" || col.ToString() == "axrowtype")
                        continue;

                    foreach (DataRow dr in dt.Rows)
                    {

                        if (dr.Table.Columns.Contains(typeColumn))
                        {
                            if (Array.IndexOf(rowsToExclude, dr[typeColumn]) > -1)
                                continue;
                        }
                        colData.Add(dr[col].ToString());
                    }
                    var q = from x in colData
                            group x by x into g
                            let count = g.Count()
                            where g.Count() <= pageSize
                            orderby g.Key ascending
                            select new { Value = g.Key, Count = count };
                    rowsCount = colData.Count;
                    try
                    {
                        distVals.Append("\"" + headName[colFld.IndexOf(col.ColumnName.ToLower())].ToString().Replace("~", " ") + "\":" +
                            serializer.Serialize(q));
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Redirect(ERRPATH + ex.Message);
                    }
                    distVals.Append(",");
                    colCnt++;
                }
                noOfRows = rowsCount;
                return distVals.ToString().TrimEnd(',') + "}";
            }
            noOfRows = rowsCount;
            return string.Empty;
        }


        #endregion

        private string ConvertPerfDate(string oldDate)
        {
            string[] tempdt = null;
            tempdt = oldDate.Split('-');
            string formatDate = string.Empty;

            if (tempdt.Length == 3)
            {
                string dd = null;
                string MM = null;
                string yyyy = null;

                string[] year = null;
                year = tempdt[0].ToString().Split(' ');
                yyyy = year[0];
                if (yyyy.Length == 2)
                {
                    yyyy = "1900";
                }
                MM = tempdt[1].ToString();
                if (int.Parse(MM) > 0 & int.Parse(MM) <= 12)
                {
                    if (MM.Length == 1)
                    {
                        MM = "0" + dd;
                    }
                }
                else
                {
                    MM = "01";
                }
                dd = tempdt[2].ToString();
                if (int.Parse(dd) > 0 & int.Parse(dd) <= 31)
                {
                    if ((dd.Length == 1))
                    {
                        dd = "0" + dd;
                    }
                }
                else
                {
                    dd = "01";
                }

                //Dim newDt As Date
                //  string newDt = new DateTime(int.Parse(yyyy), int.Parse(MM), int.Parse(dd)).ToString();
                string newDt = new DateTime(int.Parse(yyyy), int.Parse(MM), int.Parse(dd)).ToShortDateString();
                formatDate = newDt;
            }
            else
            {
                formatDate = oldDate;
            }
            return formatDate;
        }

        public void SaveConfigFile(string proj, string configStr, bool isBackupFile = false)
        {
            try
            {
                if (proj == "") return;
                string path = string.Empty;
                if (isBackupFile)
                    path = HttpContext.Current.Server.MapPath("~/Config/Previous Version Files");
                else
                    path = HttpContext.Current.Server.MapPath("~/Config");

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                path = path + "\\" + proj + ".cfg";
                if (!File.Exists(path))
                    System.IO.File.Create(path).Close();
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path, false))
                {
                    writer.WriteLine(configStr);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable GetSerialNoForPerfXml(DataTable dt, string columnName)
        {
            if (!string.IsNullOrEmpty(columnName))
            {
                string srNoColumnName = columnName;
                if (dt.Columns.Contains(srNoColumnName))
                {
                    for (int count = 0; count < dt.Rows.Count; count++)
                    {
                        dt.Rows[count][srNoColumnName] = count + 1;

                    }
                }
            }
            return dt;
        }


        public string GetConfigAttrValue(string proj, string attrName, string lang = "")
        {
            string strLang = string.Empty;
            try
            {
                string configStr = string.Empty;
                if (HttpContext.Current.Session["AxStrConfig-" + proj] == null || HttpContext.Current.Session["AxStrConfig-" + proj].ToString() == string.Empty)
                {
                    configStr = GetConfigAppJSON(proj);
                    if (configStr != "" && configStr.StartsWith("Error:"))
                        return configStr;
                    HttpContext.Current.Session["AxStrConfig-" + proj] = configStr;
                }
                else
                {
                    configStr = HttpContext.Current.Session["AxStrConfig-" + proj].ToString();
                }
                if (configStr != string.Empty)
                {
                    JArray configNode = null;
                    JArray configLangNode = null;
                    JObject objConfig = JObject.Parse(configStr);
                    configNode = (JArray)objConfig["configStr"];

                    if (configNode != null)
                    {
                        foreach (JObject content in configNode.Children<JObject>())
                        {
                            foreach (JProperty prop in content.Properties())
                            {
                                if (prop.Name == attrName)
                                {
                                    return prop.Value.ToString();
                                }
                            }
                        }
                    }

                    configLangNode = (JArray)objConfig["configLangKeys"];
                    if (configLangNode != null)
                    {
                        string langCode = "_" + lang.Substring(0, 3).ToUpper();

                        attrName = attrName + langCode;

                        foreach (JObject content in configLangNode.Children<JObject>())
                        {
                            foreach (JProperty prop in content.Properties())
                            {
                                if (prop.Name.ToUpper() == attrName.ToUpper())
                                {
                                    return prop.Value.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetConfigAttrValue function -" + ex.Message, "GetProjectArray", "GetConfigAttrValue", "new", "true");
                return strLang;
            }
            return strLang;
        }

        /// <summary>
        /// Updating the default Configuration details in Session
        /// </summary>
        /// <param name="configstr"></param>
        public void UpdateConfiginSession(string configstr, bool setConfigArray = false)
        {
            try
            {
                if (configstr != string.Empty)
                {
                    Dictionary<string, string> savedConfig = new Dictionary<string, string>();

                    JArray configNode = null;
                    JArray configLangNode = null;

                    JObject objConfig = JObject.Parse(configstr);
                    configNode = (JArray)objConfig["configStr"];
                    configLangNode = (JArray)objConfig["configLangKeys"];

                    if (configNode != null)
                    {
                        foreach (JObject content in configNode.Children<JObject>())
                        {
                            foreach (JProperty prop in content.Properties())
                            {
                                string propName = prop.Name.ToUpper();
                                if (propName == "AXIMPEXPTEMPTPATH" || propName == "AXIMAGEPATH" || propName == "AXATTACHFILEPATH"
                                    || propName == "AXGRIDATTACHPATH" || propName == "AXPRINTEXEPATH" || propName == "AXHTMLPATH")
                                {
                                    HttpContext.Current.Session[prop.Name] = prop.Value.ToString().Replace("||", "\\");
                                    if (setConfigArray)
                                        savedConfig.Add(prop.Name, prop.Value.ToString().Replace("||", "\\"));
                                }
                                else
                                {
                                    HttpContext.Current.Session[prop.Name] = prop.Value.ToString();
                                    if (setConfigArray)
                                        savedConfig.Add(prop.Name, prop.Value.ToString());
                                }
                            }
                        }
                    }

                    string lang = String.Empty;
                    if (HttpContext.Current.Session["language"] != null && HttpContext.Current.Session["language"].ToString() != String.Empty)
                    {
                        lang = HttpContext.Current.Session["language"].ToString();
                    }
                    else if (HttpContext.Current.Request.Form["language"] != null && HttpContext.Current.Request.Form["language"].ToString() != String.Empty)
                    {
                        lang = HttpContext.Current.Request.Form["language"].ToString();
                    }

                    if (lang != "")
                    {
                        string langCode = "_" + lang.Substring(0, 3).ToUpper();
                        if (configLangNode != null)
                        {
                            foreach (JObject content in configLangNode.Children<JObject>())
                            {
                                foreach (JProperty prop in content.Properties())
                                {
                                    if (prop.Name.ToUpper().EndsWith(langCode))
                                        HttpContext.Current.Session[prop.Name.Substring(0, prop.Name.IndexOf(langCode))] = prop.Value.ToString();

                                    if (setConfigArray)
                                        savedConfig.Add(prop.Name, prop.Value.ToString());
                                }
                            }
                        }
                    }

                    if (setConfigArray)
                        HttpContext.Current.Session["ConfigArray"] = savedConfig;
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog(ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Log-in UpdateConfiginSession", "new");

                if (setConfigArray)
                    UpdateConfiginSession(configstr, false);
                else
                    return;
            }
        }

        public string SaveDefaultConfigFile(string proj)
        {
            SaveConfigFile(proj, Constants.DEFAULT_CONFIGSTR);
            return Constants.DEFAULT_CONFIGSTR;
        }

        public string CallIntelliviewService(string ivName)
        {
            string link = string.Empty;
            Intelliview iv = new Intelliview(ivName);
            if (HttpContext.Current.Session["iv_filterValues" + ivName] != null)
            {
                string filterValue = HttpContext.Current.Session["iv_filterValues" + ivName].ToString();
                //if filters are defined for this Report
                if (filterValue != "*" || !string.IsNullOrEmpty(filterValue))
                {
                    //sample  userDepartments = "'Agriculture Department','PWD'";
                    iv.SetFilterValues(filterValue);
                }
                //if no filters are defined for this Report
                link = iv.ShowReport();
            }

            return link;
        }

        #region Agilecloud

        /// <summary>
        /// Function To check whether the request is coming From MainCloud Application or not
        /// </summary>
        public string GetAppAndUserInfo(string id, string ipAddr)
        {
            string result = string.Empty;
            Connection cs = new Connection();
            result = cs.GetDetails(id, ipAddr);
            return result;
        }


        /// <summary>
        /// Function to Get the application details.using these details have to Update the session variables and construct axapps and axprops To run Cloud Application 
        /// </summary>
        public string GetCloudGlobalVariables()
        {
            string response = string.Empty;
            string ixml = string.Empty;
            if (HttpContext.Current.Session["project"] != null)
            {
                LogFile.Log logobj = new LogFile.Log();
                string projName = HttpContext.Current.Session["project"].ToString();
                string errorLog = logobj.CreateLog("Calling Get New global variables", HttpContext.Current.Session.SessionID, "GetNewGloVars-Cloud", "new", "true");
                if (HttpContext.Current.Session["AxRole"] != null)//HttpContext.Current.Session["AxRole"]
                {
                    HttpContext.Current.Session["USER_EMAILID"] = HttpContext.Current.Session["username"].ToString();
                    if (HttpContext.Current.Session["AxRole"].ToString().Contains("default"))
                        ixml += "<root axpapp=\"" + projName + "\" sessionid=\"" + HttpContext.Current.Session.SessionID + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' user=\"" + HttpContext.Current.Session["USER_EMAILID"] + "\" password=\"\" sso=\"true\" url=\"\" direct=\"t\" trace=\"" + errorLog + "\"  inssession=\"true\" userroles=\"" + HttpContext.Current.Session["AxRole"] + "\" >";
                    else
                        ixml += "<root axpapp=\"" + projName + "\" sessionid=\"" + HttpContext.Current.Session.SessionID + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' user=\"" + HttpContext.Current.Session["USER_EMAILID"] + "\" password=\"\" sso=\"true\" url=\"\" direct=\"t\" trace=\"" + errorLog + "\"  inssession=\"true\" usergroup=\"\" userroles=\"\" >";
                }
                ixml += HttpContext.Current.Session["axApps"].ToString();
                ixml += HttpContext.Current.Application["axProps"].ToString();// + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
                ixml += "</root>";
                response = string.Empty;
                try
                {
                    WebServiceExt webServiceExt = new WebServiceExt();
                    response = webServiceExt.CallGetNewGlobalVarsWS("empdt", ixml);
                }
                catch (Exception ex)
                {
                    response = "Error!" + ex;
                    return response;
                }
                if (response != string.Empty)
                {
                    string[] result = response.Split('#');
                    if (result.Length > 1)
                    {
                        string[] glovars = result[1].ToString().Split('~');
                        foreach (string str in glovars)
                        {
                            int idx = str.IndexOf("=");
                            if (str != "" && str.IndexOf("appsessionkey=") > -1)
                            {
                                response = response.Replace("~" + str, "");
                                HttpContext.Current.Session["AppSessionKey"] = str.Split('=')[1];
                            }
                            else if (str != "" && str.Contains("AXGLO_USER") == false)
                                HttpContext.Current.Session[str.Substring(0, idx)] = str.Substring(idx + 1);
                        }
                    }
                }
                else
                {
                    response = "Error_While_Loading_app";
                    return response;
                }
            }
            else
            {
                response = "Error_App_Details_Not_Found";
                return response;
            }
            return response;
        }

        public string GetCloudGlobalVariables(string AppDetails)
        {
            string connString = string.Empty;
            string appXml = string.Empty;

            string response = string.Empty;

            Connection connObj = new Connection();
            // 'Data Source=orcl254;User Id=ERP_218;Password=log;'
            if (AppDetails != null && !string.IsNullOrEmpty(AppDetails))
            {
                LogFile.Log logobj = new LogFile.Log();
                string projName = string.Empty;
                string schemaName = string.Empty;
                string[] appDbDetails = AppDetails.ToString().Split(';');   //Appdb Format:   DataSource=orcl254; UId =cloudtest; Password=log;             
                foreach (string appStr1 in appDbDetails)
                {
                    string appStr = appStr1.Replace(" ", "");
                    if (appStr.ToLower().Contains("uid") || appStr.ToLower().Contains("user id") || appStr.ToLower().Contains("userid"))
                    {
                        schemaName = appStr.Substring(appStr.IndexOf("=") + 1);
                    }
                    else if ((appStr.ToLower().Contains("data source")) || (appStr.ToLower().Contains("datasource")))
                    {
                        connString = appStr.Substring(appStr.IndexOf("=") + 1);
                    }
                }
                string ixml = string.Empty;
                projName = schemaName;
                string errorLog = logobj.CreateLog("Calling Get New global variables", HttpContext.Current.Session.SessionID, "GetNewGloVars-Cloud", "new", "true");
                if (HttpContext.Current.Session["ROLE"] != null)
                {
                    if (HttpContext.Current.Session["ROLE"].ToString().Contains("default"))
                        ixml += "<root axpapp=\"" + projName + "\" sessionid=\"" + HttpContext.Current.Session.SessionID + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' user=\"" + HttpContext.Current.Session["USER_EMAILID"] + "\" password=\"\" sso=\"true\" url=\"\" direct=\"t\" trace=\"" + errorLog + "\"  inssession=\"true\" userroles=\"" + HttpContext.Current.Session["ROLE"] + "\" >";
                    else
                        ixml += "<root axpapp=\"" + projName + "\" sessionid=\"" + HttpContext.Current.Session.SessionID + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' user=\"" + HttpContext.Current.Session["USER_EMAILID"] + "\" password=\"\" sso=\"true\" url=\"\" direct=\"t\" trace=\"" + errorLog + "\"  inssession=\"true\" usergroup=\"\" userroles=\"\" >";
                }

                ConstructAxApps(projName, connString);


                //Code to execute login event fastdata sets for cloud user

                // onApp event datatables will be created in redis for cloud user.

                if (HttpContext.Current.Session["dbuser"] != null)// cloud application fast data 
                {
                    FDW fObj = new FDW(projName);
                    fObj.Initialize(projName);
                }
                ConstructAxProps(projName);
                ixml += HttpContext.Current.Session["axApps"];
                ixml += HttpContext.Current.Session["axProps"] + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
                ixml += "</root>";
                response = string.Empty;
                try
                {
                    WebServiceExt webServiceExt = new WebServiceExt();
                    response = webServiceExt.CallGetNewGlobalVarsWS("empdt", ixml);    //response Format:Done#~axp_imagepath=D: \Program files\Axpert\img\~axp_gridattach=D:\Program files\Axpert\img\~axp_testexpr=0~responsibilies=testing~rolename=~ax_evalcopy=F^
                }
                catch (Exception ex)
                {
                    response = "Error!" + ex;
                    return response;
                }
                if (response != string.Empty)
                {
                    string[] result = response.Split('#');
                    if (result.Length > 1)
                    {
                        string[] glovars = result[1].ToString().Split('~');
                        foreach (string str in glovars)
                        {
                            int idx = str.IndexOf("=");
                            if (str != "" && str.Contains("AXGLO_USER") == false)
                                HttpContext.Current.Session[str.Substring(0, idx)] = str.Substring(idx + 1);
                        }
                    }
                }
                else
                {
                    response = "Error_While_Loading_app";
                    return response;
                }
                SetDefaultAttributes(projName);
                UpdateSessionVars(projName);
            }
            else
            {
                response = "Error_App_Details_Not_Found";
                return response;
            }
            return response;
        }

        public void ConstructAxApps(string projName, string connString)
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
            HttpContext.Current.Session["axApps"] = axapps;

            GetDBConnection(projName, axapps);
        }

        /// <summary>
        /// Function To Create Session Variable axProps.These Details are required to call the Web Service(GetNewGlobalVariable)
        /// </summary>
        public void ConstructAxProps(string projName)
        {
            string axProps = "<axprops getfrom=\"\" setto=\"\">";
            axProps += "<lastlogin>" + projName + "</lastlogin>";
            axProps += "<oradatestring>dd/mm/yyyy hh24:mi:ss</oradatestring>";
            axProps += "<crlocation></crlocation>";
            axProps += "<lastusername>" + HttpContext.Current.Session["USER_EMAILID"] + "</lastusername>";
            axProps += "<login>local</login>";
            axProps += "<skin>Black</skin>";
            axProps += "<axhelp>true</axhelp>";
            axProps += "</axprops>";
            HttpContext.Current.Session["axProps"] = axProps;
        }

        /// <summary>
        /// Function To create the Config Files Then Read the default configration from the config file. 
        /// </summary>
        private void LoadAppConfiguration(string[] projNames, string proj)
        {

            //Here Need To Write the code for writing the config details and reading the details from config files 
        }

        /// <summary>
        /// Function to create default session Variables.these Variables Used Across application
        /// </summary>  
        public void SetDefaultAttributes(string projName)
        {
            if (projName != string.Empty)
            {
                UpdateConfiginSession(Constants.DEFAULT_CONFIGSTR);
            }
        }

        private void UpdateSessionVars(string projName)
        {

            //HttpContext.Current.Session["proj"] = projName;
            HttpContext.Current.Session["project"] = projName;
            //HttpContext.Current.Session["sid"] = HttpContext.Current.Session.SessionID;
            HttpContext.Current.Session["nsessionid"] = HttpContext.Current.Session.SessionID;
            HttpContext.Current.Session["AxRole"] = HttpContext.Current.Session["ROLE"].ToString();
            if (HttpContext.Current.Session["USER_EMAILID"] != null)
            {
                HttpContext.Current.Session["user"] = HttpContext.Current.Session["USER_EMAILID"].ToString();
            }
            else
            {
                HttpContext.Current.Session["user"] = "";
            }

            HttpContext.Current.Session["language"] = "ENGLISH";
            HttpContext.Current.Session["validated"] = "true";

        }


        #region WEB API
        #region Validate Identical password Post API string Formats and Urls
        public static string GETSHEMA_API_DATA = "{0}";
        public static string GETSHEMA_API_LABEL = "userid";
        public static string GETSHEMA_API_URL = "api/ValidateAppUser/GetSchemaName/";
        #endregion



        public string ConsumeApi(string apiURL, string baseaddress)
        {
            WebRequest req = WebRequest.Create(baseaddress + apiURL);
            req.Method = "GET";
            //req.Headers.Add("key");
            req.ContentType = "application/json; charset=utf-8";

            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();

            StreamReader re = new StreamReader(stream);

            string json = re.ReadToEnd();
            return json;
        }
        public string ConsumePostApi(string apiData, string apiLabel, string apiUrl)
        {
            string[] splitLabels = apiLabel.Split('/');
            string[] splitDatas = apiData.Split('/');
            string result = string.Empty;
            try
            {
                string baseURL = string.Empty;
                try
                {
                    baseURL = ConfigurationManager.AppSettings["CloudHomeAPI"].ToString();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                string json = "";
                if (splitLabels.Length == splitDatas.Length)
                {
                    json += "{";
                    for (int i = 0; i < splitLabels.Length; i++)
                    {
                        json += "\"" + splitLabels[i] + "\":\"" + splitDatas[i] + "\"";
                        if (i < (splitLabels.Length) - 1)
                        {
                            json += ",";
                        }
                    }
                    json += "}";
                }

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL + apiUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();


                if (((int)Convert.ToInt32(httpResponse.StatusCode) >= 200) && ((int)Convert.ToInt32(httpResponse.StatusCode) <= 299))
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd().ToString();
                    }
                }
                else
                {
                    Exception ex = new Exception();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion


        public void IsValidSession()
        {
            if (ConfigurationManager.AppSettings["isCloudApp"] != null && ConfigurationManager.AppSettings["isCloudApp"] == "true")
            {
                try
                {
                    if (HttpContext.Current.Session["USERID"] != null && HttpContext.Current.Session["CLOUDSESSID"] != null && ConfigurationManager.AppSettings["CloudHomeAPI"] != null)
                    {
                        string cloudHomeUrl = string.Empty;
                        string APIString = string.Format(Constants.CLOUD_HOME_API, HttpContext.Current.Session["USERID"].ToString(), HttpContext.Current.Session["CLOUDSESSID"].ToString());
                        string jsonResult = ConsumeApi(APIString, ConfigurationManager.AppSettings["CloudHomeAPI"].ToString());
                        AgcSession objAgcSession = JsonConvert.DeserializeObject<AgcSession>(jsonResult);
                        if (objAgcSession != null)
                        {
                            if (!string.IsNullOrEmpty(objAgcSession.valid_session))
                            {
                                if (ConfigurationManager.AppSettings["CloudHomeURL"] != null)
                                    cloudHomeUrl = ConfigurationManager.AppSettings["CloudHomeURL"].ToString();
                                if (objAgcSession.valid_session.ToLower() == "invalid")
                                    //HttpContext.Current.Response.Redirect(ACERRPATH + "Invalid Session - Someone has created and using the new session with same user credentials.");
                                    //TestAlert("Invalid Session - Someone has created and using the new session with same user credentials");
                                    SignOutFromCloud();

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Log Exception
                }
            }
        }


        //public void IsValidAxpertSession()
        //{
        //    if (ConfigurationManager.AppSettings["isCloudApp"] == null || ConfigurationManager.AppSettings["isCloudApp"] == "false")
        //    {
        //        ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
        //        string result = string.Empty;
        //        try
        //        {
        //            if (HttpContext.Current.Session["nsessionid"] != null && HttpContext.Current.Session["project"] != null)
        //            {
        //                string iXml = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"] + "' trace='" + "test" + "' transid='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
        //                iXml += "<sql>select 1 as a from dual</sql>";//It is a dummy query, just to call GetChoices
        //                iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
        //                result = objWebServiceExt.CallGetChoiceWS("", iXml);
        //                if (string.IsNullOrEmpty(result) || (result.StartsWith("<error>")))
        //                    HttpContext.Current.Response.Write("<script>top.location='sess.aspx';parent.location='sess.aspx';</script>");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //Log Exception
        //        }
        //    }
        //}

        public static void SendAlert(string status, string message, string functionality)
        {

            string sMessage = "showAlertDialog('" + status + "', '" + message + "','','', '" + functionality + "');";

            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;

                if (ScriptManager.GetCurrent(p) != null)
                {
                    ScriptManager.RegisterStartupScript(p, typeof(Page), "Message", sMessage, true);
                }
                else
                {
                    p.ClientScript.RegisterStartupScript(typeof(Page), "Message", sMessage, true);
                }
            }
        }


        public void delALLNotificiationKeyfromRedis()
        {
            try
            {
                string schemaName = string.Empty;
                string userName = string.Empty;
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                FDW fdwObj = new FDW();
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                if (HttpContext.Current.Session["username"] != null)
                    userName = HttpContext.Current.Session["username"].ToString();
                string NotifyKey = schemaName + "-" + userName + Constants.NOTIFICATION_PHASE;
                ArrayList NotifyKeys = fdrObj.GetAllKeys(NotifyKey);
                if (NotifyKeys.Count > 0)
                {
                    foreach (string item in NotifyKeys)
                    {

                        fdwObj.DeleteAllKeys(item, schemaName);
                    }
                }
            }
            catch (Exception ex) { }
        }


        public static void SignOutFromCloud()
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                if (ScriptManager.GetCurrent(p) != null)
                {
                    ScriptManager.RegisterStartupScript(p, typeof(Page), "cloudsignout", "CloudSignout();", true);
                }
                else
                {
                    p.ClientScript.RegisterStartupScript(typeof(Page), "Messageso", "CloudSignout();", true);
                }
            }
        }




        #endregion

        public bool GetAuthentication(ref string authenticationStatus)
        {
            string mapUsername = string.Empty,
                   mapPassword = string.Empty,
                   domain = string.Empty,
                   mapServer = string.Empty,
                   isLinuxServer = string.Empty;

            if (HttpContext.Current.Session["AxpImageServerGbl"] != null)
                mapServer = HttpContext.Current.Session["AxpImageServerGbl"].ToString();

            if (HttpContext.Current.Session["axp_domain"] != null)
                domain = HttpContext.Current.Session["axp_domain"].ToString();

            if (HttpContext.Current.Session["axp_mapusername"] != null)
                mapUsername = HttpContext.Current.Session["axp_mapusername"].ToString();

            if (HttpContext.Current.Session["axp_linuxserver"] != null)
                isLinuxServer = HttpContext.Current.Session["axp_linuxserver"].ToString();

            mapUsername = Regex.Replace(mapUsername, ";bkslh", "\\");

            string[] stlitUser = mapUsername.Split('\\');

            if (stlitUser.Length == 2)
            {
                if (domain == string.Empty)
                {
                    domain = stlitUser[0];
                }
                mapUsername = stlitUser[1];
            }

            if (HttpContext.Current.Session["axp_mappwd"] != null)
                mapPassword = HttpContext.Current.Session["axp_mappwd"].ToString();

            if (mapUsername != string.Empty && mapPassword != string.Empty)
            {
                try
                {
                    if (domain != "" && isLinuxServer != "true")
                    {
                        using (new Impersonator(mapUsername, domain, mapPassword))
                        {
                            authenticationStatus = "success";
                            return true;
                        }
                    }
                    else if (domain != "" && isLinuxServer == "true")
                    {
                        var credentials = new NetworkCredential(mapUsername, mapPassword);
                        using (new NetworkConnection(domain, credentials))
                        {
                            authenticationStatus = "success";
                            return true;
                        }
                    }
                    else
                    {
                        LogFile.Log logobj = new LogFile.Log();
                        authenticationStatus = "Domain name required for file server authentication";
                        string errorLog = logobj.CreateLog(authenticationStatus, HttpContext.Current.Session.SessionID, "Attachments-Impersonator", "new");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    LogFile.Log logobj = new LogFile.Log();
                    authenticationStatus = ex.Message;
                    string errorLog = logobj.CreateLog(ex.Message, HttpContext.Current.Session.SessionID, "Attachments-Impersonator", "new");
                    return false;
                }
            }
            return true;
        }

        //The 'Decrypt Password' logics are written based on the logics from Delphi codeset.
        public string DecryptPWD(string insId)
        {
            int i = 0;
            string pwd = String.Empty;
            string cpuId = String.Empty;

            Int32.TryParse(insId.Substring(0, 4), out i);
            insId = insId.Substring(4);
            i = i * 4;
            cpuId = insId.Substring(0, i);
            insId = insId.Substring(i);
            i = insId.Length;
            insId = insId.Substring(0, i - 3);
            pwd = GetPassword(insId, cpuId);

            return pwd;
        }

        public string GetPassword(string dtid, string mid)
        {
            int len, len1, i, k;
            string s = String.Empty, s1 = String.Empty, s2 = String.Empty, dbid = String.Empty;

            s = mid;
            len = s.Length;
            while (s != "")
            {
                s1 = s.Substring(0, 4);
                Int32.TryParse(s1, out k);
                //s2 = Strings.Chr(k).ToString();
                s2 = (Encoding.GetEncoding(1252).GetString(new byte[] { (byte)k })[0]).ToString(); //to get ASCII character for any number, works for any CultureInfo languages
                dbid = dbid + s2;
                s = s.Substring(4);
            }

            len = dbid.Length;
            len1 = dtid.Length;

            if (len1 < len)
            {
                for (i = len1; i < len; i++)
                {
                    dtid = dtid + '0';
                }
            }

            for (i = 0; i < len; i++)
            {
                s = s + Strings.Chr(Strings.Asc(dbid[i]) - Strings.Asc(dtid[i])).ToString();
            }

            return s;
        }

        public string GetDBPassword(string username)
        {
            string pwd = "log";
            LogFile.Log logobj = new LogFile.Log();

            string fileName = ScriptsPath + username + ".pwd";
            try
            {
                if (File.Exists(fileName))
                {
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        String encryptedPWD = sr.ReadToEnd();
                        pwd = DecryptPWD(encryptedPWD.Trim());
                        sr.Close();
                        sr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Exception in getting password :--- " + ex.Message.ToString(), "", "GetDBPassword", "New");
            }
            return pwd;
        }

        //The 'Encrypt Password' logics are written based on the logics from Delphi codeset.
        public string EncryptPWD(string pwd)
        {
            string enPwd = string.Empty;
            string insid = pwd;
            string dtid = GetTimeId();

            int i = dtid.Length;
            string s = dtid.Substring(0, dtid.Length - 4);

            insid = EncodeDBID(s, insid);
            enPwd = insid + dtid;
            return enPwd;
        }

        private string GetTimeId()
        {
            string res = string.Empty;
            string s = string.Empty, s1 = string.Empty;
            int i;
            string dtime = "01020345060708";
            i = int.Parse(dtime.Substring(0, 2));
            s = (i + 31).ToString();
            s = s + (int.Parse(dtime.Substring(2, 2)) + i + 13);
            s = s + (int.Parse(dtime.Substring(4, 4)) * i);
            s = s + dtime.Substring(8, 2) + dtime.Substring(10, 2) + dtime.Substring(12, 2);
            i = s.Length;
            s1 = "00" + i;
            res = s + s1;
            return res;
        }

        private string EncodeDBID(string dtid, string dbid)
        {
            string Result = string.Empty;
            int len = dbid.Length;
            int len1 = dtid.Length;
            string s = string.Empty, s1 = string.Empty;
            if (len1 < len)
            {
                for (int i = len1; i < len; i++)
                {
                    dtid = dtid + '0';
                }
            }
            ArrayList arr = new ArrayList();
            for (int i = 0; i < len; i++)
            {
                arr.Add((Encoding.ASCII.GetBytes(dbid[i].ToString())[0] + dtid[i]));
            }

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].ToString().Length == 4)
                    s1 = s1 + arr[i];
                else if (arr[i].ToString().Length == 3)
                    s1 = s1 + ("0" + arr[i]);
                else if (arr[i].ToString().Length == 2)
                    s1 = s1 + ("00" + arr[i]);
            }
            int i1 = dbid.Length;
            if (i1.ToString().Length == 1)
                s = "000" + i1;
            else if (i1.ToString().Length == 2)
                s = "00" + i1;
            else if (i1.ToString().Length == 3)
                s = "0" + i1;
            else if (i1.ToString().Length == 4)
                s = i1.ToString();
            Result = s + s1;
            return Result;
        }

        public string GetSchemaName()
        {
            string schemaName = "Global";
            if (System.Web.HttpContext.Current.Session != null)
            {
                string strApps = System.Web.HttpContext.Current.Session["axApps"].ToString();
                //if(strApps.IndexOf("<?xml") == -1)
                //{
                //    strApps = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + strApps;
                //}
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(strApps);
                XmlNode dbUserNode = xml.SelectSingleNode("//dbuser");
                if (dbUserNode != null)
                {
                    schemaName = dbUserNode.InnerText;
                }
            }

            return schemaName;
        }

        public void ClearFDFldSession()
        {
            try
            {
                if (HttpContext.Current.Session["fd-HugeFlds"] != null)
                {
                    HttpContext.Current.Session["fdFldData-" + HttpContext.Current.Session["fd-HugeFlds"] + ""] = null;
                    HttpContext.Current.Session["fd-fm-" + HttpContext.Current.Session["fd-HugeFlds"] + ""] = null;
                    HttpContext.Current.Session["fd-HugeFlds"] = null;
                }
            }
            catch (Exception e)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("Fast Data Field session clear -" + e.Message, HttpContext.Current.Session.SessionID, "ClearFDFldSession", "new");
            }
        }

        public void CopyFiles(string sourcePath, string destPath, string srcFileName)
        {
            try
            {
                System.IO.BinaryReader brReader = default(System.IO.BinaryReader);
                System.IO.BinaryWriter brWriter = default(System.IO.BinaryWriter);

                DirectoryInfo di = new DirectoryInfo(sourcePath);
                DirectoryInfo diNew = new DirectoryInfo(destPath);

                if (!diNew.Exists)
                    diNew.Create();

                if (!di.Exists)
                    di.Create();

                if (di.Exists)
                {
                    string strFile = sourcePath + "\\" + srcFileName;
                    string strDest = destPath + srcFileName;
                    FileStream input = null;
                    try
                    {
                        input = new FileStream(strFile, FileMode.Open, FileAccess.Read);
                    }
                    catch (FileNotFoundException ex)
                    {

                    }
                    if (input != null)
                    {
                        FileStream output = new FileStream(strDest, FileMode.Create, FileAccess.Write);
                        brReader = new System.IO.BinaryReader(input);
                        brWriter = new System.IO.BinaryWriter(output);
                        int bufsize = 30000;
                        // this is buffer size
                        int readcount = 0;
                        int bsize = 0;

                        int indexer = 0;
                        FileInfo fileInfo = new FileInfo(strFile);

                        int FileLen = Convert.ToInt32(fileInfo.Length);
                        while ((readcount < FileLen))
                        {
                            if (bufsize < FileLen - readcount)
                            {
                                bsize = bufsize;
                            }
                            else
                            {
                                bsize = FileLen - readcount;
                            }
                            byte[] buffer = new byte[bsize];

                            brReader.Read(buffer, indexer, bsize);
                            brWriter.Write(buffer, indexer, bsize);

                            readcount = readcount + bsize;
                        }

                        brReader.Close();
                        brWriter.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string CopyFilesPDF(string sourcePath, string destPath, string srcFileName)
        {
            string destFilename = string.Empty;
            try
            {
                System.IO.BinaryReader brReader = default(System.IO.BinaryReader);
                System.IO.BinaryWriter brWriter = default(System.IO.BinaryWriter);

                DirectoryInfo di = new DirectoryInfo(sourcePath);
                DirectoryInfo diNew = new DirectoryInfo(destPath);

                if (!diNew.Exists)
                    diNew.Create();

                if (!di.Exists)
                    di.Create();

                if (di.Exists)
                {
                    string strFile = sourcePath + "\\" + srcFileName;
                    if (srcFileName.IndexOf(".") != -1)
                        destFilename = srcFileName.Split('.')[0] + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_fff") + "." + srcFileName.Split('.')[1];
                    else
                        destFilename = srcFileName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_fff") + ".pdf";
                    string strDest = destPath + destFilename;
                    FileStream input = null;
                    try
                    {
                        input = new FileStream(strFile, FileMode.Open, FileAccess.Read);
                    }
                    catch (FileNotFoundException ex)
                    {

                    }
                    if (input != null)
                    {
                        FileStream output = new FileStream(strDest, FileMode.Create, FileAccess.Write);
                        brReader = new System.IO.BinaryReader(input);
                        brWriter = new System.IO.BinaryWriter(output);
                        int bufsize = 30000;
                        // this is buffer size
                        int readcount = 0;
                        int bsize = 0;

                        int indexer = 0;
                        FileInfo fileInfo = new FileInfo(strFile);

                        int FileLen = Convert.ToInt32(fileInfo.Length);
                        while ((readcount < FileLen))
                        {
                            if (bufsize < FileLen - readcount)
                            {
                                bsize = bufsize;
                            }
                            else
                            {
                                bsize = FileLen - readcount;
                            }
                            byte[] buffer = new byte[bsize];

                            brReader.Read(buffer, indexer, bsize);
                            brWriter.Write(buffer, indexer, bsize);

                            readcount = readcount + bsize;
                        }

                        brReader.Close();
                        brWriter.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return destFilename;
        }

        public CacheManager GetCacheObject()
        {
            CacheManager cacheMgr = null;
            string errorLog = string.Empty;
            try
            {
                cacheMgr = new CacheManager(errorLog);
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("Get Cache Object-" + ex.Message, HttpContext.Current.Session.SessionID, "GetCacheObject", "new");
            }
            return cacheMgr;
        }

        public TStructDef GetStrObject(CacheManager cacheMgr, string transId)
        {
            TStructDef strObj = null;
            // cachemanager and TStructDef objects throw exceptions
            try
            {
                string language = HttpContext.Current.Session["language"].ToString();
                strObj = cacheMgr.GetStructDef(HttpContext.Current.Session["project"].ToString(), HttpContext.Current.Session["nsessionid"].ToString(), HttpContext.Current.Session["user"].ToString(), transId, HttpContext.Current.Session["AxRole"].ToString());
            }
            catch (Exception ex)
            {
                if (ex.Message == Constants.SESSIONEXPMSG)
                {
                    //SessionExpired();
                    return null;
                }
                else
                {
                    // Response.Redirect(util.ERRPATH + ex.Message.Replace(Environment.NewLine, ""));
                }
            }


            return strObj;
        }

        public byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }


        public string encrtptDecryptAES(string data, bool encrypt = true)
        {
            string returnString = string.Empty;
            try
            {
                if (ConfigurationManager.AppSettings["EncryptionKey"] != null && ConfigurationManager.AppSettings["EncryptionIV"] != null && ConfigurationManager.AppSettings["EncryptionKey"] != string.Empty && ConfigurationManager.AppSettings["EncryptionIV"] != string.Empty)
                {
                    string[] keyStr = ConfigurationManager.AppSettings["EncryptionKey"].ToString().Split(',');
                    byte[] keyBytes = keyStr.Select(Byte.Parse).ToArray();
                    string[] ivStr = ConfigurationManager.AppSettings["EncryptionIV"].ToString().Split(',');
                    byte[] ivBytes = ivStr.Select(Byte.Parse).ToArray();
                    if (encrypt)
                    {
                        byte[] encryptedString = EncryptStringToBytes_Aes(data, keyBytes, ivBytes);
                        returnString = BitConverter.ToString(encryptedString);
                    }
                    else
                    {
                        byte[] encryptedByte = data.Split('-').Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToArray();
                        returnString = DecryptStringFromBytes_Aes(encryptedByte, keyBytes, ivBytes);
                    }
                }
            }
            catch (Exception ex) { }
            return returnString;
        }

        #region RedisServerKeys
        public string GetRedisServerkey(string type, string transId, string user = "ALL")
        {
            string AxRole = string.Empty;
            string lang = string.Empty;
            string key = string.Empty;
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["AxRoleRedis"] != null)
                    AxRole = HttpContext.Current.Session["AxRoleRedis"].ToString();
                else if (HttpContext.Current.Session["AxRole"] != null)
                {
                    AxRole = HttpContext.Current.Session["AxRole"].ToString();
                    AxRole = GetAxRoleRedis(AxRole);
                }

                if (HttpContext.Current.Session["language"] != null)
                    lang = HttpContext.Current.Session["language"].ToString().Substring(0, 3);
            }
            switch (type)
            {
                case Constants.CACMENU:
                    key = "General-" + AxRole + '-' + lang + "2";
                    break;
                case Constants.REDISMENUDATA:
                    key = "General-MenuData-" + AxRole + '-' + lang;
                    break;
                case Constants.REDISCARDSDATA:
                    key = "General-CardsData-" + transId + '-' + lang;
                    break;
                case Constants.REDISTSTRUCT:
                    key = transId + '-' + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTMOB:
                    key = transId + "-Mob-" + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTTABLE:
                    key = transId + "-DcTable" + '-' + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTDOFORM:
                    key = transId + "-DoForm" + '-' + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTDESIGN:
                    key = transId + "-dsgn" + '-' + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTAXCUSTHTML:
                    key = transId + "-axTstCustomHtml" + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTAXDESIGN:
                    key = transId + "-axDesign" + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTALL:
                    key = transId + '-';
                    break;
                case Constants.REDISTSTRUCTDTLS:
                    key = transId + "-TSTDTLS" + '-' + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTDTLSMOB:
                    key = transId + "-TSTDTLSMOB" + '-' + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTDESIGNDTLS:
                    key = transId + "-dsgnTSTDTLS" + '-' + AxRole + '-' + lang;
                    break;
                case Constants.IVIEWSTRUCT:
                    key = transId + "-StructXml-" + AxRole + '-' + lang;
                    break;
                case Constants.IVIEWPARAM:
                    key = transId + "-getparam-" + AxRole + '-' + lang;
                    break;
                case Constants.AXCONFIGURATIONS:
                    key = transId + "-axconfigstruct-" + AxRole + '-' + lang;
                    break;
                case Constants.AXCONFIGURATIONTABLE:
                    key = transId + "-axpConfigTable-" + AxRole + '-' + lang;
                    break;
                case Constants.AXOLDDESIGN:
                    key = transId + "-axOldDesign-" + lang;
                    break;
                case Constants.AXPAGETITLE:
                    key = "axpagetitle-" + lang;
                    break;
                case Constants.AXHYBRIDINFO:
                    key = transId + "-axgetHybridInfo";
                    break;
                case Constants.AXHYBRIDWEIGHTSCALEINFO:
                    key = transId + "-axHybridWeightScaleInfo";
                    break;
                case Constants.AXHYBRIDNOTIFIINFO:
                    key = transId + "-axgetHybridNotifiInfo";
                    break;
                case Constants.RedisIviewObj:
                    key = transId + "-iv-" + AxRole + '-' + lang;
                    break;
                case Constants.RedisListviewObj:
                    key = transId + "-lv-" + AxRole + '-' + lang;
                    break;
                case Constants.RedisIviewBuilderObj:
                    key = transId + "-ivBuild-" + AxRole + '-' + lang;
                    break;
                case Constants.RedisIviewSettings:
                    key = transId + "-ivSettings-" + AxRole + '-' + user + '-' + lang;
                    break;
                case Constants.RedisListviewSettings:
                    key = transId + "-lvSettings-" + AxRole + '-' + user + '-' + lang;
                    break;
                case Constants.RedisIviewBuilderSettingsObj:
                    key = transId + "-ivBuildSettings-" + AxRole + '-' + lang;
                    break;
                case Constants.RedisOldIviewSettings:
                    key = "ivSettings-" + AxRole + '-' + user + '-' + lang;
                    break;
                case Constants.RedisOldListviewSettings:
                    key = "lvSettings-" + AxRole + '-' + user + '-' + lang;
                    break;
                case Constants.RedisOldIviewTemplates:
                    key = transId + "-ivTemplates-" + AxRole + '-' + user + '-' + lang;
                    break;
                case Constants.RedisOldListviewTemplates:
                    key = transId + "-lvTemplates-" + AxRole + '-' + user + '-' + lang;
                    break;
                case Constants.IviewNavigationData:
                    key = transId + "-ivNavParams-" + lang;
                    break;
                case Constants.REDISGLOBALVARS:
                    key = "GlobalVars-" + user;
                    break;
                case Constants.REDISAXUSEROPTIONS:
                    key = "axUserOptions-" + user;
                    break;
                case Constants.REDISLVRECORDLISTING:
                    key = "lvRecordListing-" + user;
                    break;
                case Constants.ListViewFieldsInfo:
                    key = transId + "-LstFldInfo" + '-' + AxRole + '-' + lang;
                    break;
                case Constants.AXRULESDEF:
                    key = transId + "-AxRulesDef-" + lang;//key = transId + "-AxRulesDef-" + AxRole + '-' + lang;
                    break;
                case Constants.AXRULESDEFUserRole:
                    key = transId + "-AxRulesDefUserRole-" + AxRole + '-' + lang;
                    break;
                case Constants.DBMEMVARSFORMLOAD:
                    key = "axdbmemvars-" + user + "-" + transId;
                    break;
                case Constants.CONFIGDATAVARSFORMLOAD:
                    key = transId + "-axconfigdatavars" + '-' + lang;
                    break;
                case Constants.HTMLPAGESQUERY:
                    key = transId + "-HTMLPAGESQUERY-" + user + '-' + lang;
                    break;
                case Constants.AXVALERRORCODE:
                    key = transId + "-AxValErrorCode-" + AxRole + '-' + lang;
                    break;
                case Constants.REDISLASTLOGGEDIN:
                    key = "LastLoggedInInfo-" + transId;
                    break;
                case Constants.REDISPROFILEPIC:
                    key = "ProfilePic-" + transId;
                    break;
                case Constants.AXRULESDEFBTNCMSG:
                    key = transId + "-AxRulesDefBtnConMsg-" + lang;
                    break;
                case Constants.AXRULESDEFBTNCONMSGUSERROLE:
                    key = transId + "-AxRBtnConMsgUserRole-" + AxRole + '-' + lang;
                    break;
                case Constants.AXNOTIFYHISTORY:
                    key = "AxNotifyHistory-" + transId;
                    break;
                case Constants.REDISCARDSDATADASHBOARD:
                    key = "General-DashBoardCardsData-" + transId + '-' + lang;
                    break;
                case Constants.REDISHOMEPAGECARDS:
                    key = "General-" + type + '-' + lang;
                    break;
                case Constants.REDISOTPAUTH:
                    key = transId + "-otpauth";
                    break;
                case Constants.REDISPWDOTPAUTHLANG:
                    key = transId + "-pwdotpauthlang";
                    break;
                case Constants.REDISFREEZEGRDCOLS:
                    key = transId + "-FreezeGridCols-" + user;
                    break;
                case Constants.REDISTSTHTMLLS:
                    key = transId + "-tstHtmlLs-" + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTHTMLLSMob:
                    key = transId + "-tstHtmlLsMob-" + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTHTMLLSGLOBALVARS:
                    key = transId + "-tstHtmlLsGlbVars" + '-' + AxRole + '-' + lang;
                    break;
                case Constants.REDISTSTRUCTDOPTIME:
                    key = transId + "-TstflDopTime-" + user + "-" + lang;
                    break;
                case Constants.REDISTSTFLDFROMAPI:
                    key = transId + "-TstFldFromApi-" + user + "-" + AxRole + "-" + lang;
                    break;
                case Constants.REDISTSTCUSTOMJSCSS:
                    key = transId + "-TstCustomJsCss-" + AxRole + "-" + lang;
                    break;
                case Constants.REDISARMPERMISSION:
                    key = transId + "-" + Constants.REDISARMPERMISSION + "-" + user;
                    break;
                case Constants.REDISARMMETADATA:
                    key = transId + "-" + Constants.REDISARMMETADATA + "-" + user;
                    break;
                case Constants.REDISARMCONNECTEDDATAMETADATA:
                    key = "*" + transId.Substring(1) + "*" + Constants.REDISARMCONNECTEDDATAMETADATA + "*" + user;
                    break;
                case Constants.REDISARMCONNECTEDDATAPERMISSION:
                    key = "*" + transId.Substring(1) + "*" + Constants.REDISARMPERMISSION + "*" + user;
                    break;
                case Constants.REDISARMAXADSSQL:
                    key = "ARM-" + Constants.REDISARMAXADSSQL + "-" + transId;
                    break;
                case Constants.REDISARMAXADSDATA:
                    key = "ARM-" + Constants.REDISARMAXADSDATA + "-" + transId + "-";
                    break;
                case Constants.REDISARMAXCARDSLIST:
                    key = "ARM-" + Constants.REDISARMAXCARDSLIST;
                    break;
                case Constants.REDISBULKUSERACTIVATE:
                    key = Constants.REDISBULKUSERACTIVATE + "-" + transId;
                    break;
                default:
                    key = "General-" + AxRole + '-' + lang;
                    break;
            }
            return key;
        }

        public string GetRedisServerFieldkey(string transId, string type, string fldName, int fldIndex)
        {
            string AxRole = string.Empty;
            string lang = string.Empty;
            string Iviewname = string.Empty;
            string key = string.Empty;
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["AxRoleRedis"] != null)
                    AxRole = HttpContext.Current.Session["AxRoleRedis"].ToString();
                else if (HttpContext.Current.Session["AxRole"] != null)
                {
                    AxRole = HttpContext.Current.Session["AxRole"].ToString();
                    AxRole = GetAxRoleRedis(AxRole);
                }

                if (HttpContext.Current.Session["language"] != null)
                    lang = HttpContext.Current.Session["language"].ToString().Substring(0, 3);

                if (HttpContext.Current.Session["iName"] != null)
                    Iviewname = HttpContext.Current.Session["iName"].ToString();
            }
            switch (type)
            {

                case Constants.DEPFLDARRAY:
                    key = transId + '-' + fldName + "-array-" + AxRole + '-' + lang;
                    break;
                case Constants.FIELDNAME:
                    key = transId + '-' + fldName + "-" + AxRole + '-' + lang;
                    break;
                case Constants.FIELDINDEX:
                    key = transId + '-' + fldName + "-" + fldIndex + "-" + AxRole + '-' + lang;
                    break;
                case Constants.FORMLOADARRAY:
                    key = transId + "-FormLoadArray-" + lang;
                    break;
                case Constants.FORMLOADINDEX:
                    key = transId + "-FormLoadIndex-" + fldIndex + "-" + lang;
                    break;
                case Constants.FIELDPARAMNAMES:
                    key = transId + '-' + type + "-" + AxRole + '-' + lang;
                    break;
                default:
                    key = transId + '-' + type + "-" + AxRole + '-' + lang;
                    break;
            }
            return key;

        }

        public string GetConfigCacheKey(string type, string transId, string ivName, string roles, string username)
        {
            string AxRole = string.Empty;
            string lang = string.Empty;
            string key = string.Empty;
            if (roles != string.Empty)
            {
                var LstRoles = roles.Split(',').OrderBy(x => x).ToList();
                AxRole = string.Join(",", LstRoles);
            }
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["language"] != null)
                    lang = HttpContext.Current.Session["language"].ToString().Substring(0, 3);
            }
            switch (type)
            {
                case Constants.AXCONFIGGENERAL:
                    key = "General-axGenConfigs-" + AxRole + "-" + username + "-" + lang;
                    break;
                case Constants.AXCONFIGTSTRUCT:
                    key = transId + "-axconfigstruct-" + AxRole + "-" + username + "-" + lang;
                    break;
                case Constants.AXCONFIGIVIEW:
                    key = ivName + "-axivconfigstruct-" + AxRole + "-" + username + "-" + lang;
                    break;
                default:
                    key = "General-" + AxRole + '-' + lang;
                    break;
            }
            return key;
        }
        public string GetNoDataConfigCacheKey(string type, string transId, string ivName, string roles, string username)
        {
            string AxRole = string.Empty;
            string lang = string.Empty;
            string key = string.Empty;
            if (roles != string.Empty)
            {
                var LstRoles = roles.Split(',').OrderBy(x => x).ToList();
                AxRole = string.Join(",", LstRoles);
            }
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["language"] != null)
                    lang = HttpContext.Current.Session["language"].ToString().Substring(0, 3);
            }
            switch (type)
            {
                case Constants.AXNODATACONFIGGENERAL:
                    key = "General-axNoGenConfigs-" + AxRole + "-" + username + "-" + lang;
                    break;
                case Constants.AXNODATACONFIGTSTRUCT:
                    key = transId + "-tstNoConfigs-" + AxRole + "-" + username + "-" + lang;
                    break;
                case Constants.AXNODATACONFIGIVIEW:
                    key = ivName + "-ivNoConfigs-" + AxRole + "-" + username + "-" + lang;
                    break;
                default:
                    key = "General-" + AxRole + '-' + lang;
                    break;
            }
            return key;
        }

        private string GetAxRoleRedis(string AxRole)
        {
            string axroles = string.Empty;
            try
            {
                var LstRoles = AxRole.Split(',').OrderBy(x => x).ToList();
                axroles = string.Join(",", LstRoles);
                HttpContext.Current.Session["AxRoleRedis"] = axroles;
                return axroles;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["AxRoleRedis"] = AxRole;
                return AxRole;
            }
        }

        public string GetFastDataDSName(DataSet dtDefin, string dsName)
        {
            if (dtDefin == null)
            {
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                var memFastDataDef = fdrObj.DSasJsonFromRedis(Constants.RS_FD_DEFINITION);
                dtDefin = memFastDataDef;
            }
            string newDsName = string.Empty;
            foreach (DataRow row in dtDefin.Tables[Constants.FD_DT_DEFINITION].Rows)
            {
                string strDsName = row["DataSetName"].ToString();
                string strSqlQuery = row["SQLText"].ToString();
                string strLoadEvent = row["LoadEvent"].ToString().ToLower();
                if (!IsNullOrEmpty(strDsName) && !IsNullOrEmpty(strSqlQuery) && !IsNullOrEmpty(strLoadEvent) && dsName == strDsName)
                {
                    DataTable dtName = new DataTable(strDsName);
                    try
                    {
                        if (strLoadEvent == "onlogin")
                        {
                            newDsName = strDsName + LoginSuffixAndParamsToSQL(row)[0];
                        }
                        else
                            newDsName = dsName;
                        break;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return newDsName;
        }

        public bool ParseString(string str)
        {
            Int32 intValue;
            Int64 bigintValue;
            Double doubleValue;
            Decimal decimalValue;
            DateTime dateValue;

            if (Int32.TryParse(str, out intValue))
                return false;
            else if (Int64.TryParse(str, out bigintValue))
                return false;
            else if (Double.TryParse(str, out doubleValue))
                return false;
            else if (Decimal.TryParse(str, out decimalValue))
                return false;
            else if (DateTime.TryParse(str, out dateValue))
                return true;
            else return true;

        }

        public string[] LoginSuffixAndParamsToSQL(DataRow row)
        {
            //NOte: opType = suffix/params
            string[] returnString = new string[2];
            try
            {
                returnString[1] = row["SQLText"].ToString();
                string[] Parameters = row["Params"].ToString().Split(',');

                foreach (string par in Parameters)
                {
                    String[] SeparatePar = par.Split(':');

                    if (!String.IsNullOrEmpty(HttpContext.Current.Session[SeparatePar[1].Trim()].ToString()))
                    {
                        string setValue = HttpContext.Current.Session[SeparatePar[1].Trim()].ToString();
                        setValue = ReplaceSqlInjChar(setValue);
                        if (ParseString(setValue))
                        {
                            setValue = "'" + setValue + "'";
                        }
                        returnString[0] += "-" + HttpContext.Current.Session[SeparatePar[1].Trim()].ToString();
                        returnString[1] = returnString[1].Replace(":" + SeparatePar[1].Trim(), setValue);
                    }
                    else
                    {
                        returnString[1] = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            { }
            return returnString;
        }

        #endregion

        public string SetCulture(string language)
        {
            string direction = string.Empty;
            switch (language)
            {
                case "ARABIC":
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-AE");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-AE");
                    direction = "rtl-ar";
                    break;
                case "FRENCH":
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-CA");
                    direction = "ltr-fr";
                    break;
                case "HINDI":
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("hi-IN");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("hi-IN");
                    direction = "ltr-hi";
                    break;
                default:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
                    direction = "ltr-en";
                    break;
            }
            return direction;
        }

        public void CheckUserSettings()
        {
            string userResult = String.Empty;
            string sql = String.Empty;
            //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
            ASB.WebService objWeb = new ASB.WebService();
            try
            {
                sql = Constants.SQL_CHECK_USER;
                sql = sql.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());

                userResult = objWeb.GetChoicesCustomWS("", sql);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(userResult);
                var isUserExists = doc.SelectNodes("//row//USERNAME");
                if (isUserExists.Count == 0)
                {
                    sql = Constants.SQL_INSERT_USER_APPSETTINGS;
                    sql = sql.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());
                    sql = sql.Replace("$VALUE$", Constants.DEFAULT_USER_APPSETTINGS);
                    userResult = objWeb.GetChoicesCustomWS("", sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UtlEncode(string Utl)
        {
            if (Utl != String.Empty)
            {
                Utl += "^~)";
                try
                {
                    if (ConfigurationManager.AppSettings["EncryptionKey"] != null && ConfigurationManager.AppSettings["EncryptionIV"] != null && ConfigurationManager.AppSettings["EncryptionKey"] != string.Empty && ConfigurationManager.AppSettings["EncryptionIV"] != string.Empty)
                    {
                        string[] keyStr = ConfigurationManager.AppSettings["EncryptionKey"].ToString().Split(',');
                        byte[] keyBytes = keyStr.Select(Byte.Parse).ToArray();
                        string[] ivStr = ConfigurationManager.AppSettings["EncryptionIV"].ToString().Split(',');
                        byte[] ivBytes = ivStr.Select(Byte.Parse).ToArray();
                        byte[] encryptedUtl = EncryptStringToBytes_Aes(Utl, keyBytes, ivBytes);
                        Utl = BitConverter.ToString(encryptedUtl).Replace("-", string.Empty);
                    }
                }
                catch (Exception Ex)
                { }
            }
            return Utl;
        }

        public void TempAttaServerFiles()
        {
            if (HttpContext.Current.Session["attGridFileServer"] != null && HttpContext.Current.Session["attGridFileServer"].ToString() != string.Empty)
            {
                string attServerFiles = HttpContext.Current.Session["attGridFileServer"].ToString();
                string[] lstServerFiles = attServerFiles.Split('♦');
                string authenticationStatus = string.Empty;
                if (lstServerFiles.Length > 0 && GetAuthentication(ref authenticationStatus))
                {
                    foreach (var lstFile in lstServerFiles)
                    {
                        if (lstFile != string.Empty)
                        {
                            string filePath = lstFile.Split('~')[1];
                            try
                            {
                                File.Delete(filePath);
                            }
                            catch (Exception) { }
                        }
                    }
                }
                HttpContext.Current.Session["attGridFileServer"] = "";
            }

            if (HttpContext.Current.Session["tstGridDbFiles"] != null && HttpContext.Current.Session["tstGridDbFiles"].ToString() != string.Empty)
            {
                string _attGrFileNames = HttpContext.Current.Session["tstGridDbFiles"].ToString();
                HttpContext.Current.Session["tstGridDbFiles"] = "";
                string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                string sessionid = HttpContext.Current.Session["nsessionid"] != null ? HttpContext.Current.Session["nsessionid"].ToString() : "";
                if (sessionid != "")
                {
                    string sFileDir = scriptsPath + "Axpert\\" + sessionid + "\\";
                    foreach (var lstFile in _attGrFileNames.Split(','))
                    {
                        if (lstFile != "")
                        {
                            string filePath = sFileDir + lstFile;
                            try
                            {
                                File.Delete(filePath);
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }

            if (HttpContext.Current.Session["AxpAttFileServer"] != null)
            {
                ArrayList attServerFiles = (ArrayList)HttpContext.Current.Session["AxpAttFileServer"];
                string authenticationStatus = string.Empty;
                if (attServerFiles.Count > 0 && GetAuthentication(ref authenticationStatus))
                {
                    foreach (var lstFile in attServerFiles)
                    {
                        string filePath = lstFile.ToString().Split('~')[1];
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch (Exception) { }
                    }
                }
                HttpContext.Current.Session["AxpAttFileServer"] = null;
            }

            try
            {
                if (HttpContext.Current.Session["tstHeaderAttach"] != null && HttpContext.Current.Session["tstHeaderAttach"].ToString() == "true")
                {
                    HttpContext.Current.Session["tstHeaderAttach"] = "false";
                    string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                    string sessionid = HttpContext.Current.Session["nsessionid"] != null ? HttpContext.Current.Session["nsessionid"].ToString() : "";
                    if (sessionid != "")
                    {
                        string sFileDir = scriptsPath + "Axpert\\" + sessionid + "\\";
                        DirectoryInfo div = new DirectoryInfo(sFileDir);
                        foreach (DirectoryInfo Folder in div.GetDirectories())
                        {
                            try
                            {
                                if (Folder.Name.StartsWith("tstHFile-"))
                                    Folder.Delete(true);
                            }
                            catch
                            { }
                        }
                    }
                }

                if (HttpContext.Current.Session["tstHeaderAttachDbFile"] != null && HttpContext.Current.Session["tstHeaderAttachDbFile"].ToString() != "")
                {
                    string _attFileNames = HttpContext.Current.Session["tstHeaderAttachDbFile"].ToString();
                    HttpContext.Current.Session["tstHeaderAttachDbFile"] = "";
                    string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                    string sessionid = HttpContext.Current.Session["nsessionid"] != null ? HttpContext.Current.Session["nsessionid"].ToString() : "";
                    if (sessionid != "")
                    {
                        string sFileDir = scriptsPath + "Axpert\\" + sessionid + "\\";
                        foreach (var lstFile in _attFileNames.Split(','))
                        {
                            if (lstFile != "")
                            {
                                string filePath = sFileDir + lstFile;
                                try
                                {
                                    File.Delete(filePath);
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void ClearUserIviewData()
        {
            try
            {
                string user = HttpContext.Current.Session["user"].ToString();
                user = CheckSpecialChars(user);

                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                {
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                }

                FDW fdwObj = new FDW();

                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

                ///////////////////////   

                string keyAccess = Constants.RedisIvData;

                string keyPattern = fObj.MakeKeyName(keyAccess, "*", user, "*", -1);

                ArrayList keyList = fObj.GetPrefixedKeys(keyPattern, true, string.Empty, false);

                fdwObj.DeleteKeys(keyList);

                ///////////////////////

                keyAccess = Constants.RedisLvData;

                keyPattern = fObj.MakeKeyName(keyAccess, "*", user, "*", -1);

                keyList = fObj.GetPrefixedKeys(keyPattern, true, string.Empty, false);

                fdwObj.DeleteKeys(keyList);

                /////////////////////// 
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Exception in ClearUserIviewData-" + ex.Message, "ClearUserIviewData", "ClearUserIviewData", "new");
            }
        }

        public void RemoveLoggedUserDetails(string PrevProj, string sessId, string uName)
        {
            try
            {
                if (!string.IsNullOrEmpty(PrevProj) && !string.IsNullOrEmpty(sessId))
                {
                    string loggedUserList = GetDupUserInfo(PrevProj, uName);
                    if (loggedUserList != null && loggedUserList != "" && loggedUserList.IndexOf("♦" + sessId + "♣") != -1)
                    {
                        try
                        {
                            FDW fdwObj = new FDW(PrevProj);
                            bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
                            if (isRedisConnected)
                            {
                                fdwObj.HashDeletekey(Constants.AXLOGGEDUSER, uName);
                                fdwObj.HashDeletekey(Constants.AXLOGGEDUSER + "-ARM", uName);
                                if (HttpContext.Current.Session["ARM_SessionID"] != null && HttpContext.Current.Session["ARM_SessionID"].ToString() != "")
                                {
                                    fdwObj.ClearRedisServerDataByKey(HttpContext.Current.Session["ARM_SessionID"].ToString(), "", true);
                                    fdwObj.ClearRedisServerDataByKey(HttpContext.Current.Session["ARM_SessionID"].ToString() + "-VIEWFILTERS", "", true);
                                }
                            }
                            else
                                HttpContext.Current.Application[PrevProj + Constants.AXLOGGEDUSER + uName] = "";
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public bool CheckLoggedUserDetails(string PrevProj, string sessId, string uName)
        {
            if (!string.IsNullOrEmpty(PrevProj) && !string.IsNullOrEmpty(sessId))
            {
                string loggedUserList = GetDupUserInfo(PrevProj, uName);
                if (loggedUserList != null && loggedUserList != "" && loggedUserList.IndexOf("♦" + sessId + "♣") != -1)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public void AddKeyOnRefreshSave(string sessCacheKey)
        {
            var frSavekeys = new List<string>();
            if (HttpContext.Current.Session["ac-frsave-key"] != null)
            {
                frSavekeys = (List<string>)HttpContext.Current.Session["ac-frsave-key"];
                frSavekeys.Add(sessCacheKey);
                HttpContext.Current.Session["ac-frsave-key"] = frSavekeys;
            }
            else
            {
                frSavekeys.Add(sessCacheKey);
                HttpContext.Current.Session["ac-frsave-key"] = frSavekeys;
            }
        }

        public void DeleteKeyOnRefreshSave()
        {
            var frSavekeys = new List<string>();
            if (HttpContext.Current.Session["ac-frsave-key"] != null)
            {
                frSavekeys = (List<string>)HttpContext.Current.Session["ac-frsave-key"];
                for (int i = 0; i < frSavekeys.Count; i++)
                {
                    HttpContext.Current.Session.Remove(frSavekeys[i].ToString());
                }
            }
            HttpContext.Current.Session.Remove("ac-frsave-key");
            try
            {
                var lstAutKeys = HttpContext.Current.Session.Keys.Cast<string>().Where(x => x.StartsWith("autocomplete♦")).ToList();
                if (lstAutKeys.Count > 0)
                {
                    foreach (string _key in lstAutKeys)
                        HttpContext.Current.Session.Remove(_key);
                }
            }
            catch (Exception ex) { }
        }


        public string GetAxRelations(string TransId)
        {
            string AxRelKeys = string.Empty;
            try
            {
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                DataTable dt = fdrObj.DataTableFromRedis(Constants.RS_AXRELATIONS);
                string projName = HttpContext.Current.Session["dbuser"].ToString();
                if (dt != null && dt.Rows.Count > 0)
                {
                    var arkeys = dt.AsEnumerable().Where(x => x.Field<string>("mstruct") == TransId).Select(r => new { dstruct = r.Field<string>("dstruct"), dfield = r.Field<string>("dfield") }).Distinct().ToList();
                    foreach (var dd in arkeys)
                    {
                        AxRelKeys += projName + "-" + dd.dstruct + "-" + dd.dfield + "*";
                    }
                    if (AxRelKeys != string.Empty)
                        AxRelKeys = AxRelKeys.Remove(AxRelKeys.Length - 1);
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Exception in GetAxRelations-Util function-" + ex.Message, "AxRelations", "GetAxRelations", "new");
            }
            return AxRelKeys;
        }

        /// <summary>
        /// Function to check if the field axp_real_time_cache contains True then auto refresh will work in web
        /// </summary>
        /// <param name="transid"></param>
        /// <param name="tstDef"></param>
        /// <param name="tstData"></param>
        /// <returns></returns>
        public bool isRealTimeCacEnabled(string transid, TStructDef tstDef)
        {
            if (tstDef == null)
            {
                tstDef = GetTstructDefObj("From Listview", transid);
            }

            bool isRTCacheEnabled = false;
            int fldIdx = tstDef.GetFieldIndex("axp_real_time_cache");
            if (fldIdx != -1)
            {
                TStructDef.FieldStruct fld = (TStructDef.FieldStruct)tstDef.flds[fldIdx];
                if (fld.expr.ToLower() == "{t}")
                    isRTCacheEnabled = true;
            }
            return isRTCacheEnabled;
        }

        public string GetIpAddress()
        {
            string ipaddress = "";
            try
            {
                if (HttpContext.Current.Session["clientIpAddress"] != null)
                {
                    return HttpContext.Current.Session["clientIpAddress"].ToString();
                }

                ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ipaddress == "" || ipaddress == null)
                    ipaddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (ipaddress == "::1" || ipaddress == "127.0.0.1") //if application is running locally(Client and Server both are the same machine) ie, from Visual Studio, the IP Address will show as 127.0.0.1 or ::1 then take client local ip address
                {
                    string strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                    foreach (IPAddress IP in ipEntry.AddressList)
                    {
                        if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipaddress = Convert.ToString(IP);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ipaddress = "";
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Fetching client IP address", HttpContext.Current.Session["nsessionid"].ToString(), "login", "true", ex.Message);
            }
            return ipaddress;
        }
        /// <summary>
        /// Function to check for Script tag in given str and replace them with standard constants.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string CheckScriptTag(string str)
        {
            if (str == null)
                str = "";
            str = Regex.Replace(str, "<script>", "&ltscript&gt");
            str = Regex.Replace(str, "</script>", "&lt/script&gt");
            return str;
        }

        public string GetConfigAppJSON(string proj)
        {
            string configStr = string.Empty;
            try
            {
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fdrObj == null)
                    fdrObj = new FDR(proj);
                if (fdrObj.redisIpLocalHost == "localhost")
                {
                    return "Error: Please use the proper IP Address for Redis Host.";
                }
                configStr = fdrObj.StringFromRedis(Constants.CONFIGAPP_JSON_KEY, proj.ToLower());
                if (configStr == "")
                {
                    configStr = ReadFromFile(HttpContext.Current.Server.MapPath("~/Config/") + proj + ".cfg");
                    if (configStr == "")
                        configStr = SaveDefaultConfigFile(proj);

                    if (proj != String.Empty)
                    {
                        FDW fdwObj = new FDW(proj);
                        fdwObj.SaveInRedisServer(Constants.CONFIGAPP_JSON_KEY, configStr, Constants.CONFIGAPP_JSON_KEY, proj.ToLower());
                    }
                }
            }
            catch (Exception ex) { }
            return configStr;
        }

        public string GetAdvConfigs(string ConfType, string structureType = "", string structureName = "")
        {
            string stsPage = string.Empty;
            try
            {
                if (HttpContext.Current.Session["AdvConfigsGeneral"] == null)
                {
                    string schemaName = string.Empty;
                    if (HttpContext.Current.Session["dbuser"] != null)
                        schemaName = HttpContext.Current.Session["dbuser"].ToString();
                    FDW fdwObj = new FDW();
                    bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
                    if (isRedisConnected)
                    {
                        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                        string axconfigKey = Constants.AXCONFIGGENERAL;
                        DataTable configData = fObj.DataTableFromRedis(GetConfigCacheKey(axconfigKey, "", "", "ALL", "ALL"));
                        if (configData != null && configData.Rows.Count > 0)
                        {
                            HttpContext.Current.Session["AdvConfigsGeneral"] = configData;
                            stsPage = GetGenConfigString(configData, ConfType, structureType, structureName);
                        }
                        else
                        {
                            string axconfigNoKey = Constants.AXNODATACONFIGGENERAL;
                            string nodata = fObj.StringFromRedis(GetNoDataConfigCacheKey(axconfigNoKey, "", "", "ALL", "ALL"));
                            if (nodata == "NoData")
                            {
                                HttpContext.Current.Session["AdvConfigsGeneral"] = "NoData";
                                return stsPage;
                            }
                            else
                            {
                                DataTable dt = GetGenConfigsFromDB(schemaName);
                                HttpContext.Current.Session["AdvConfigsGeneral"] = dt;
                                stsPage = GetGenConfigString(dt, ConfType, structureType, structureName);
                            }
                        }
                    }
                    else
                    {
                        DataTable dt = GetGenConfigsFromDB(schemaName);
                        HttpContext.Current.Session["AdvConfigsGeneral"] = dt;
                        stsPage = GetGenConfigString(dt, ConfType, structureType, structureName);
                    }
                }
                else
                {
                    if (HttpContext.Current.Session["AdvConfigsGeneral"].ToString() == "NoData")
                        return stsPage;
                    else
                    {
                        DataTable dt = (DataTable)HttpContext.Current.Session["AdvConfigsGeneral"];
                        stsPage = GetGenConfigString(dt, ConfType, structureType, structureName);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return stsPage;
        }
        public DataTable getAllGenConfigsData()
        {
            DataTable configData = new DataTable();
            try
            {
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                string axconfigKey = Constants.AXCONFIGGENERAL;
                configData = fObj.DataTableFromRedis(GetConfigCacheKey(axconfigKey, "", "", "ALL", "ALL"));
                if (configData != null && configData.Rows.Count > 0)
                {
                    configData = GetGenConfigsFromDB(schemaName);
                }
            }
            catch (Exception ex) { }

            return configData;
        }
        private DataTable GetGenConfigsFromDB(string schemaName)
        {
            DataTable dt = new DataTable();
            try
            {
                DBContext objDbCont = new DBContext();
                dt = objDbCont.GetDataTableInline(Constants.SQL_GET_AXPGENCONFIGS);
                FDW fdwObj = new FDW();
                bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
                if (isRedisConnected)
                {
                    string axconfigKey = Constants.AXCONFIGGENERAL;
                    string axconfigNoKey = Constants.AXNODATACONFIGGENERAL;
                    if (dt != null && dt.Rows.Count > 0)
                        fdwObj.SaveInRedisServerDT(GetConfigCacheKey(axconfigKey, "", "", "ALL", "ALL"), dt, axconfigKey, schemaName);
                    else
                        fdwObj.SaveInRedisServer(GetNoDataConfigCacheKey(axconfigNoKey, "", "", "ALL", "ALL"), "NoData", axconfigNoKey, schemaName);
                }
                return dt;
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Get General ConfigsFromDB - " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "GetGenConfigsFromDB", "", "true");
                string _db_type = HttpContext.Current.Session["axdb"].ToString();
                if (_db_type.ToLower() == "oracle")
                    HttpContext.Current.Response.Redirect(ERRPATH + "oracle DB error:" + ex.Message);
                return dt;
            }
        }
        private string GetGenConfigString(DataTable dt, string ConfType, string structureType, string structureName)
        {
            string stsValue = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                string AxRole = string.Empty;
                if (HttpContext.Current.Session["AxRole"] != null)
                    AxRole = HttpContext.Current.Session["AxRole"].ToString();
                ConfType = ConfType.ToLower();
                if (ConfType == "landing structure")
                {
                    AxRole = "♦" + AxRole.Replace(",", "♦") + "♦";
                    var stsValueNew = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType && AxRole.Contains("♦" + x.Field<string>("USERROLES") + "♦")).Select(x => (x.Field<string>("STYPE") == null ? x.Field<string>("PROPSVAL") : x.Field<string>("STYPE")) + "♦" + x.Field<string>("STRUCTNAME") + "♦" + x.Field<string>("PROPVALUE2")).ToList();
                    if (stsValueNew.Count > 0)
                    {
                        stsValue = stsValueNew[0];
                    }
                    if (stsValue == string.Empty)
                        stsValue = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType && x.Field<string>("USERROLES") == "ALL").Select(x => (x.Field<string>("STYPE") == null ? x.Field<string>("PROPSVAL") : x.Field<string>("STYPE")) + "♦" + x.Field<string>("STRUCTNAME") + "♦" + x.Field<string>("PROPVALUE2")).First().ToString();
                }
                else if (ConfType == "main page reload")
                    stsValue = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType && x.Field<string>("PROPSVAL") == "true").Select(x => x.Field<string>("STRUCTNAME")).First().ToString();
                else if (ConfType == "google maps api key")
                    stsValue = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType).Select(x => x.Field<string>("PROPVALUE2")).First().ToString();
                else if (ConfType == "load old model views"/* || ConfType == "smartviews cache time"*/)
                {
                    AxRole = "♦" + AxRole.Replace(",", "♦") + "♦";
                    try
                    {
                        stsValue = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType && (AxRole.Contains("♦" + x.Field<string>("USERROLES") + "♦") || x.Field<string>("USERROLES") == "ALL") && x.Field<string>("STYPE").ToLower() == structureType && (x.Field<string>("STRUCTNAME") == structureType || x.Field<string>("STRUCTNAME").StartsWith("ALL "))).Select(x => (x.Field<string>("PROPSVAL"))).ToList().First().ToString();
                    }
                    catch (Exception ex) { }
                }
                else if (ConfType == "listview as default")
                {
                    AxRole = "♦" + AxRole.Replace(",", "♦") + "♦";
                    try
                    {
                        var dataRows = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType && (AxRole.Contains("♦" + x.Field<string>("USERROLES") + "♦") || x.Field<string>("USERROLES") == "ALL") && x.Field<string>("STYPE").ToLower() == structureType);
                        DataTable filteredDt = dataRows.Any() ? dataRows.CopyToDataTable() : new DataTable();

                        stsValue = JsonConvert.SerializeObject(filteredDt);
                    }
                    catch (Exception ex) { }
                }
                else if (ConfType == "listview as default from search")
                {
                    AxRole = "♦" + AxRole.Replace(",", "♦") + "♦";
                    try
                    {
                        var dataRows = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType && (AxRole.Contains("♦" + x.Field<string>("USERROLES") + "♦") || x.Field<string>("USERROLES") == "ALL") && x.Field<string>("STYPE").ToLower() == structureType);
                        DataTable filteredDt = dataRows.Any() ? dataRows.CopyToDataTable() : new DataTable();

                        stsValue = JsonConvert.SerializeObject(filteredDt);
                    }
                    catch (Exception ex) { }
                }
                else if (ConfType == "user manual")
                    stsValue = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType).Select(x => x.Field<string>("PROPSVAL") + "♦" + x.Field<string>("PROPVALUE2")).First().ToString();
                else if (ConfType == "icon path")
                    stsValue = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType).Select(x => x.Field<string>("PROPVALUE2")).First().ToString();
                else
                    stsValue = dt.AsEnumerable().Where(x => x.Field<string>("ASPROPS").ToLower() == ConfType).Select(x => x.Field<string>("PROPSVAL")).First().ToString();
            }
            return stsValue;
        }

        //private static bool CheckHTMLTags(string value)
        //{
        //    Regex pattern = new Regex("[¿|♦♠♣♥]|[\n]{1}");
        //    string afterAppSymbols = pattern.Replace(value, "");

        //    bool containsHTML = ((afterAppSymbols != HttpUtility.HtmlEncode(afterAppSymbols) || (afterAppSymbols != HttpUtility.HtmlDecode(afterAppSymbols))));
        //    if (!containsHTML)
        //        return false;
        //    else
        //        return true;
        //}

        public static bool CheckCrossScriptingInString(string value)
        {
            //if (ConfigurationManager.AppSettings["doXSSCheck"] == null || string.IsNullOrEmpty(ConfigurationManager.AppSettings["doXSSCheck"].ToString()) || ConfigurationManager.AppSettings["doXSSCheck"].ToLower() != "true")
            //    return false;

            if (!string.IsNullOrEmpty(value))
            {

                List<string> tgs = new List<string>() { "<applet", "<body", "<embed", "<frame", "<script", "<html", "<iframe", "<img", "<style", "<layer", "<link", "<ilayer", "<meta", "<object", "alert(", "<svg", "<xss", "confirm(", "prompt(", "<prompt", "expression(", "onerror", "scriptlet", "setTimeout(", "/vbs", "eval(", ".href", ".location.", "document.", "onchange", "onclick", "onmouseover", "onmouseout", "onkeydown", "onload", "<h1>", "<h2>", "<h3>", "<h4>", "<h5>", "<h6>", "</h1>", "</h2>", "</h3>", "</h4>", "</h5>", "</h6>", "<marquee" };
                //string temp = Regex.Replace(value.ToLower(), @"\s", "");
                string temp = value.ToLower();
                // Replace & to acceptable html encode character. Thne replace it with & to bypass the htmltag method
                //Replace & to some other character
                // string beforesign = temp.Replace("&", "♦");
                // if (CheckHTMLTags(beforesign))
                //     return true;
                //beforesign = temp.Replace("♦", "&");
                //Replace the character back to &

                if (tgs.FirstOrDefault(s => (temp.IndexOf(s.ToLower()) > -1 || temp.Contains(s.ToLower()))) != null)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool ContainsXSS(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            string decodedValue = MultiLevelDecode(value);
            decodedValue = decodedValue.ToLower();
            List<string> dangerousTags = new List<string>
        {
            "<script", "alert(", "expression(", "onerror", "onload", "eval(", "setTimeout(", "document.", "window.",
            "javascript:", "vbscript:", "confirm(", "prompt(", "background:", "scriptlet", "<iframe", "<object", "<embed",
            "<meta", "<link", "<style", "onmouseover", "onmouseout", "onfocus", "onblur", "onkeydown", "onkeyup",
            "<img", ".href", ".location.", "setInterval(", "createElement(", "innerHTML", "outerHTML", "tostring:alert", "{tostring", "constructor.constructor", "alert", "function(", "newfunction", "=>alert"
        };
            string xssPattern = @"(<script.*?>|javascript:|on\w+=|alert\(|eval\(|document\.|window\.|setTimeout\(|setInterval\(|innerHTML|outerHTML)";
            if (dangerousTags.Exists(tag => decodedValue.Contains(tag)))
            {
                return true;
            }
            if (Regex.IsMatch(decodedValue, xssPattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }
        private static string MultiLevelDecode(string input)
        {
            string decoded = HttpUtility.UrlDecode(input);
            decoded = HttpUtility.HtmlDecode(decoded);
            decoded = Regex.Replace(decoded, @"\\x([0-9A-Fa-f]{2})", match =>
            {
                return ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString();
            });
            return decoded;
        }

        public string ReplaceBlockedTags(string inputData)
        {
            try
            {
                inputData = inputData.Replace("&lt;gmi ", "&lt;img ");
                inputData = inputData.Replace("&lt;1h", "&lt;h1");
                inputData = inputData.Replace("&lt;2h", "&lt;h2");
                inputData = inputData.Replace("&lt;3h", "&lt;h3");
                inputData = inputData.Replace("&lt;4h", "&lt;h4");
                inputData = inputData.Replace("&lt;5h", "&lt;h5");
                inputData = inputData.Replace("&lt;6h", "&lt;h6");

                inputData = inputData.Replace("&lt;/1h&gt", "&lt;/h1&gt");
                inputData = inputData.Replace("&lt;/2h&gt", "&lt;/h2&gt");
                inputData = inputData.Replace("&lt;/3h&gt", "&lt;/h3&gt");
                inputData = inputData.Replace("&lt;/4h&gt", "&lt;/h4&gt");
                inputData = inputData.Replace("&lt;/5h&gt", "&lt;/h5&gt");
                inputData = inputData.Replace("&lt;/6h&gt", "&lt;/h6&gt");
                return inputData;
            }
            catch (Exception ex)
            {
                return inputData;
            }
        }



        #region Formload Data Cache
        public object GetFormLoadKey(string transId, string flGlobalVarNode)
        {
            string flKey = string.Empty, devOptionKey = string.Empty, matchflGlobalVar = string.Empty;
            try
            {
                int keyIndex = -1;
                DataTable dtConfig = GetStructConfig(transId);
                string cacheTime = GetConfigSettings(transId, dtConfig);
                if (cacheTime != "none")
                {
                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                    flKey = fdrObj.MakeKeyName(Constants.FORMLOADRES, transId);
                    devOptionKey = cacheTime;
                    if (flGlobalVarNode != string.Empty)
                    {
                        flGlobalVarNode = flGlobalVarNode.Replace(";bkslh", "\\");
                        var keyList = flGlobalVarNode.Split('¿').ToList();
                        keyIndex = fdrObj.MakeVarKeyName(keyList);
                        if (keyIndex != -1)
                            matchflGlobalVar = keyList[keyIndex];
                    }
                }
                else
                    flKey = "none";
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetFormLoadKey -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in GetFormLoadKey", "new");
            }
            return new { flKey = flKey, flGlobalVars = flGlobalVarNode, devOption = devOptionKey, matchflGlobalVar = matchflGlobalVar };
        }

        public string GetFormLoadData(string transId, dynamic flGblExistingKeys)
        {
            string result = string.Empty;
            try
            {
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                string flKey = flGblExistingKeys.GetType().GetProperty("flKey").GetValue(flGblExistingKeys, null);
                string devOption = flGblExistingKeys.GetType().GetProperty("devOption").GetValue(flGblExistingKeys, null);
                string matchflGlobalVar = flGblExistingKeys.GetType().GetProperty("matchflGlobalVar").GetValue(flGblExistingKeys, null);
                if (matchflGlobalVar != string.Empty)
                {
                    string delimited = @"\\";
                    matchflGlobalVar = Regex.Replace(matchflGlobalVar, delimited, ";bkslh");
                }
                if (matchflGlobalVar != string.Empty && devOption == string.Empty)
                {
                    string keyDate = matchflGlobalVar.Split('♦').Last();
                    string sysDate = DateTime.Now.ToString("dd/MM/yyyy");
                    if (keyDate == sysDate)
                        result = fdrObj.HashGetKey(flKey, matchflGlobalVar);
                }
                else if (matchflGlobalVar != string.Empty && devOption != string.Empty)
                {
                    int cacheMins = 0;
                    if (devOption.ToLower().IndexOf("min") != -1)
                    {
                        string mints = devOption.Replace("min", "").Replace("Min", "");
                        cacheMins = int.Parse(mints.TrimEnd());
                    }
                    else
                    {
                        string hours = devOption.Replace("hour", "").Replace("Hour", "");
                        cacheMins = int.Parse(hours.TrimEnd()) * 60;
                    }

                    string keyDate = matchflGlobalVar.Split('♦').Last();
                    string sysDate = DateTime.Now.ToString("dd/MM/yyyy");
                    double sysTime = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")).TotalMinutes;
                    double keyTime = TimeSpan.Parse(DateTime.Parse(keyDate).ToString("HH:mm")).TotalMinutes;
                    keyDate = DateTime.Parse(keyDate).ToString("dd/MM/yyyy");
                    double elapsTime = sysTime - keyTime;
                    if (keyDate == sysDate && elapsTime <= cacheMins)
                        result = fdrObj.HashGetKey(flKey, matchflGlobalVar);
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetFormLoadData -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in GetFormLoadData", "new");
            }
            return result;
        }

        public string SetFormLoadData(string loadRes, string transId, dynamic flGblExistingKeys)
        {
            string flgvValue = string.Empty;
            try
            {
                string flKey = "";
                string flGlobalVars = "";
                string devOption = "";
                string matchflGlobalVar = "";
                if (flGblExistingKeys.ToString() != "")
                {
                    flKey = flGblExistingKeys.GetType().GetProperty("flKey").GetValue(flGblExistingKeys, null);
                    flGlobalVars = flGblExistingKeys.GetType().GetProperty("flGlobalVars").GetValue(flGblExistingKeys, null);
                    devOption = flGblExistingKeys.GetType().GetProperty("devOption").GetValue(flGblExistingKeys, null);
                    matchflGlobalVar = flGblExistingKeys.GetType().GetProperty("matchflGlobalVar").GetValue(flGblExistingKeys, null);
                }

                string stsGlobal = "";
                if (loadRes != "true" && loadRes != "false")
                {
                    string stsResult = loadRes.Replace("*$*", "¿");
                    stsResult = stsResult.Split('¿').Last();
                    stsGlobal = ParseJSonResultNode(stsResult);
                }
                else
                {
                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                    flKey = fdrObj.MakeKeyName(Constants.FORMLOADRES, transId);
                    stsGlobal = transId;
                }
                string strflKeys = string.Empty, strSignleKey = string.Empty;
                if (flGlobalVars == string.Empty)
                {
                    if (devOption != string.Empty)
                        strflKeys = stsGlobal + "♦" + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    else
                        strflKeys = stsGlobal + "♦" + DateTime.Now.ToString("dd/MM/yyyy");
                    strSignleKey = strflKeys;
                }
                else
                {
                    if (matchflGlobalVar == string.Empty)
                    {
                        if (devOption != string.Empty)
                        {
                            strSignleKey = stsGlobal + "♦" + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            strflKeys = flGlobalVars + "¿" + strSignleKey;
                        }
                        else
                        {
                            strSignleKey = stsGlobal + "♦" + DateTime.Now.ToString("dd/MM/yyyy");
                            strflKeys = flGlobalVars + "¿" + strSignleKey;
                        }
                    }
                    else
                    {
                        string replKey = string.Empty;

                        try
                        {
                            FDW fdwObjNew = new FDW();
                            fdwObjNew.HashDeletekey(flKey, matchflGlobalVar);
                        }
                        catch (Exception exd) { }
                        string existingKey = matchflGlobalVar.Substring(0, matchflGlobalVar.LastIndexOf('♦'));
                        if (devOption != string.Empty)
                            replKey = existingKey + "♦" + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        else
                            replKey = existingKey + "♦" + DateTime.Now.ToString("dd/MM/yyyy");
                        strSignleKey = replKey;
                        strflKeys = flGlobalVars.Replace(matchflGlobalVar, replKey);
                    }
                }
                flgvValue = strflKeys;
                FDW fdwObj = new FDW();
                HttpContext.Current.Session["formloadsubkey-" + transId] = strSignleKey;
                fdwObj.HashSetKey(flKey, strSignleKey, loadRes);

                try
                {
                    string schemaName = string.Empty;
                    if (HttpContext.Current.Session["dbuser"] != null)
                        schemaName = HttpContext.Current.Session["dbuser"].ToString();
                    string fldKeyDop = Constants.REDISTSTRUCTDOPTIME;
                    fdwObj.SaveInRedisServer(GetRedisServerkey(fldKeyDop, transId, stsGlobal), strSignleKey, Constants.REDISTSTRUCT, schemaName);
                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("SetFormLoadData -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in SetFormLoadData", "new");
            }
            return flgvValue;
        }

        public string ParseJSonResultNode(string result)
        {
            string globVars = string.Empty;
            try
            {
                string delimited = @"\\";
                string dfResultparams = Regex.Replace(result, delimited, ";bkslh");
                var pickData = JsonConvert.DeserializeObject<globalVar>(dfResultparams);
                if (pickData != null && pickData.globalVars != null && pickData.globalVars.Count() > 0)
                {
                    globVars = string.Join("♦", pickData.globalVars.Select(i => i.n + ":" + i.v));
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("ParseJSonResultNode -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in ParseJSonResultNode", "new");
            }
            return globVars;
        }

        private class globalVar
        {
            public List<globalVars> globalVars { get; set; }
        }

        private class globalVars
        {
            public string n { get; set; }
            public string v { get; set; }
        }

        public string GetConfigSettings(string transid, DataTable axpConfigStr)
        {
            string cacheTime = string.Empty;
            if (axpConfigStr == null)
            {
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                axpConfigStr = fObj.DataTableFromRedis(GetConfigCacheKey(Constants.AXCONFIGTSTRUCT, transid, "", "ALL", "ALL"));
            }
            if (axpConfigStr != null && axpConfigStr.Rows.Count > 0)
            {
                var cacheTimes = axpConfigStr.AsEnumerable().Where(x => x.Field<string>("PROPS") == "FormLoad" && x.Field<string>("STYPE") == "Tstruct" && x.Field<string>("STRUCTNAME") == transid).Select(y => y.Field<string>("PROPSVAL")).ToList();
                if (cacheTimes.Count > 0)
                    cacheTime = cacheTimes[0].ToString();
            }
            return cacheTime;
        }

        public DataTable GetStructConfig(string transId)
        {
            DataTable dtconfig = new DataTable();
            try
            {
                string axpStructKey = Constants.AXCONFIGTSTRUCT;
                if (HttpContext.Current.Session["AxDtConfigs"] != null)
                {
                    dtconfig = (DataTable)HttpContext.Current.Session["AxDtConfigs"];
                    HttpContext.Current.Session.Remove("AxDtConfigs");
                }
                if (dtconfig.Rows.Count == 0)
                {
                    FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                    dtconfig = fObj.DataTableFromRedis(GetConfigCacheKey(axpStructKey, transId, "", HttpContext.Current.Session["AxRole"].ToString(), "ALL"));
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                string sessID = Constants.GeneralLog;
                if (HttpContext.Current.Session != null)
                    sessID = HttpContext.Current.Session.SessionID;
                logObj.CreateLog("GetStructConfig -" + ex.Message, sessID, "GetStructConfig", "new");
            }
            return dtconfig;
        }

        #endregion

        public string ReplaceImagePath(string loadXml)
        {
            try
            {
                if (HttpContext.Current.Session["AxpSaveImageDb"] != null && HttpContext.Current.Session["AxpSaveImageDb"].ToString() == "true")
                {
                    if (HttpContext.Current.Session["AxpImagePathGbl"] != null && HttpContext.Current.Session["AxpImagePathGbl"].ToString() != "")
                    {
                        string aximageNodeVal = string.Empty, aximageNode = string.Empty;
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(loadXml);
                        XmlNode inputNode = xmlDoc.SelectSingleNode("/root/globalvars");
                        foreach (XmlNode xmlNode in inputNode.ChildNodes)
                        {
                            if (xmlNode.Name.ToLower() == "axpimagepath")
                            {
                                aximageNodeVal = "<" + xmlNode.Name + ">" + HttpContext.Current.Session["AxpImagePathGbl"].ToString() + "</" + xmlNode.Name + ">";
                                aximageNode = "<" + xmlNode.Name + "></" + xmlNode.Name + ">";
                                break;
                            }
                        }
                        loadXml = loadXml.Replace(aximageNodeVal, aximageNode);
                    }
                }
                return loadXml;
            }
            catch (Exception)
            {
                return loadXml;
            }
        }

        public string GetGlobalVarString()
        {
            string GlobalVarString = string.Empty;
            try
            {
                string fdKeyData = Constants.REDISGLOBALVARS;
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();

                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                    fObj = new FDR();
                GlobalVarString = fObj.StringFromRedis(GetRedisServerkey(fdKeyData, "GlobalVars", HttpContext.Current.Session["username"].ToString()), schemaName);
            }
            catch (Exception)
            { }
            if (GlobalVarString == string.Empty && HttpContext.Current.Session["globalvarstring"] != null)
                GlobalVarString = HttpContext.Current.Session["globalvarstring"].ToString();
            return GlobalVarString;
        }

        public void SetGlobalVarString(string GlobalVarString)
        {
            try
            {
                string fdKeyData = Constants.REDISGLOBALVARS;
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDW fdwObj = new FDW();
                bool IsCache = fdwObj.SaveInRedisServer(GetRedisServerkey(fdKeyData, "GlobalVars", HttpContext.Current.Session["username"].ToString()), GlobalVarString, Constants.REDISGLOBALVARS, schemaName);
                if (IsCache == false)
                    HttpContext.Current.Session["globalvarstring"] = GlobalVarString;
            }
            catch (Exception)
            {
                HttpContext.Current.Session["globalvarstring"] = GlobalVarString;
            }
        }

        public Dictionary<int, string> GetlvRecordList(string transid)
        {
            Dictionary<int, string> lvRecordListing = new Dictionary<int, string>();
            try
            {
                string fdKeyData = Constants.REDISLVRECORDLISTING;
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();

                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                    fObj = new FDR();
                var lvRecordListings = fObj.ObjectJsonFromRedis(GetRedisServerkey(fdKeyData, "lvRecordListing", HttpContext.Current.Session["username"].ToString()) + "-" + transid, schemaName);
                if (lvRecordListings == null && HttpContext.Current.Session["lvRecordListing-" + transid] != null)
                    lvRecordListing = (Dictionary<int, string>)HttpContext.Current.Session["lvRecordListing-" + transid];
                else if (lvRecordListings != null)
                    lvRecordListing = (Dictionary<int, string>)lvRecordListings;
            }
            catch (Exception)
            {
                if (HttpContext.Current.Session["lvRecordListing-" + transid] != null)
                    lvRecordListing = (Dictionary<int, string>)HttpContext.Current.Session["lvRecordListing-" + transid];
            }
            return lvRecordListing;
        }

        public void SetlvRecordList(string transid, Dictionary<int, string> lvRecordListing)
        {
            try
            {
                string fdKeyData = Constants.REDISLVRECORDLISTING;
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDW fdwObj = new FDW();
                bool IsCache = fdwObj.SaveInRedisServer(GetRedisServerkey(fdKeyData, "lvRecordListing", HttpContext.Current.Session["username"].ToString()) + "-" + transid, lvRecordListing, Constants.REDISLVRECORDLISTING, schemaName);
                if (IsCache == false)
                    HttpContext.Current.Session["lvRecordListing-" + transid] = lvRecordListing;
            }
            catch (Exception)
            {
                HttpContext.Current.Session["lvRecordListing-" + transid] = lvRecordListing;
            }
        }

        public void DeleteUnwantedKeys()
        {
            try
            {
                string userName = string.Empty;
                if (HttpContext.Current.Session["username"] == null)
                    return;
                else
                    userName = HttpContext.Current.Session["username"].ToString();
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] == null)
                    return;
                else
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDW fdwObj = new FDW();
                ArrayList list = new ArrayList();
                list.Add(schemaName + "-" + Constants.REDISGLOBALVARS + "-" + userName);
                list.Add(schemaName + "-" + Constants.REDISAXUSEROPTIONS + "-" + userName);
                fdwObj.DeleteKeys(list);

                string fdKeyData = Constants.REDISLVRECORDLISTING;
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                    fObj = new FDR();
                var lvRecordListings = fObj.GetWildCardKeyNames(GetRedisServerkey(fdKeyData, "lvRecordListing", HttpContext.Current.Session["username"].ToString()) + "-");
                fdwObj.DeleteKeys(lvRecordListings);

                string fdData = Constants.DBMEMVARSFORMLOAD;
                var dbVarKeys = fObj.GetWildCardKeyNames(GetRedisServerkey(fdData, "", HttpContext.Current.Session["username"].ToString()));
                fdwObj.DeleteKeys(dbVarKeys);
            }
            catch (Exception ex)
            { }
        }

        public void RemovelvListPageLoad()
        {
            try
            {
                string userName = string.Empty;
                if (HttpContext.Current.Session["username"] == null)
                    return;
                else
                    userName = HttpContext.Current.Session["username"].ToString();
                FDW fdwObj = new FDW();
                string fdKeyData = Constants.REDISLVRECORDLISTING;

                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                    fObj = new FDR();
                var lvRecordListings = fObj.GetWildCardKeyNames(GetRedisServerkey(fdKeyData, "lvRecordListing", HttpContext.Current.Session["username"].ToString()) + "-");
                fdwObj.DeleteKeys(lvRecordListings);
            }
            catch (Exception)
            { }
        }

        #region Axpert license server based
        public string GetServerLicDetails(string redisLicIp, string redisLicPass)
        {
            string redisConn = string.Empty;
            try
            {
                if (!redisLicIp.StartsWith("127.0.0.1:"))
                {
                    redisConn = "keyNotMatch";
                    return redisConn;
                }

                if (redisLic == null)
                {
                    ConfigurationOptions
                        configLic = new ConfigurationOptions
                        {
                            SyncTimeout = int.MaxValue,
                            KeepAlive = 60,
                            Password = redisLicPass,
                            AbortOnConnectFail = false,
                            AllowAdmin = true,
                        };
                    configLic.EndPoints.Add(redisLicIp);

                    try
                    {
                        redisLic = ConnectionMultiplexer.Connect(configLic);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (redisLic.IsConnected)
                {
                    IDatabase cacheClient = redisLic.GetDatabase();
                    string key = "licstring";
                    if (cacheClient.KeyExists(key))
                        redisConn = "keyExists";
                    else
                        redisConn = "keyNotExists";
                }
                else
                    redisConn = "notConnected";
            }
            catch (Exception) { }
            return redisConn;
        }
        #endregion

        public void DeleteTstIvObject(string TstIvKey = "")
        {
            try
            {
                if (HttpContext.Current.Session["isDupTab"] != null && HttpContext.Current.Session["isDupTab"].ToString().StartsWith("true-"))
                {
                    string _thisDupTab = HttpContext.Current.Session["isDupTab"].ToString();
                    _thisDupTab = _thisDupTab.Split('-')[1];
                    if (HttpContext.Current.Session["tstivobjkey-duptab-" + _thisDupTab] != null && HttpContext.Current.Session["tstivobjkey-duptab-" + _thisDupTab].ToString() != "")
                    {
                        string tstivobjkey = HttpContext.Current.Session["tstivobjkey-duptab-" + _thisDupTab].ToString();
                        string[] lstKeys = tstivobjkey.Split(',');
                        var item = lstKeys.Where(x => x.StartsWith(TstIvKey + "_")).ToList();
                        if (TstIvKey != "" && item.Count > 0)//To remove from the TstIvKey to next keys
                        {
                            string delKeys = string.Empty;
                            int keyInd = Array.IndexOf(lstKeys, item[0]);
                            for (int i = keyInd; i < lstKeys.Length; i++)
                            {
                                HttpContext.Current.Session.Remove(lstKeys[i]);
                                if (delKeys == string.Empty)
                                    delKeys = lstKeys[i];
                                else
                                    delKeys += "," + lstKeys[i];
                            }
                            tstivobjkey = tstivobjkey.Replace(delKeys, "").TrimEnd(',');
                            HttpContext.Current.Session["tstivobjkey-duptab-" + _thisDupTab] = tstivobjkey;
                        }
                        else//To remove all the keys 
                        {
                            foreach (var key in lstKeys)
                            {
                                HttpContext.Current.Session.Remove(key);
                            }
                            HttpContext.Current.Session.Remove("tstivobjkey-duptab-" + _thisDupTab);
                        }
                    }
                }
                else if (HttpContext.Current.Session["tstivobjkey"] != null && HttpContext.Current.Session["tstivobjkey"].ToString() != "")
                {
                    string tstivobjkey = HttpContext.Current.Session["tstivobjkey"].ToString();
                    string[] lstKeys = tstivobjkey.Split(',');
                    var item = lstKeys.Where(x => x.StartsWith(TstIvKey + "_")).ToList();
                    if (TstIvKey != "" && item.Count > 0)//To remove from the TstIvKey to next keys
                    {
                        string delKeys = string.Empty;
                        int keyInd = Array.IndexOf(lstKeys, item[0]);
                        for (int i = keyInd; i < lstKeys.Length; i++)
                        {
                            HttpContext.Current.Session.Remove(lstKeys[i]);
                            if (delKeys == string.Empty)
                                delKeys = lstKeys[i];
                            else
                                delKeys += "," + lstKeys[i];
                        }
                        tstivobjkey = tstivobjkey.Replace(delKeys, "").TrimEnd(',');
                        HttpContext.Current.Session["tstivobjkey"] = tstivobjkey;
                    }
                    else//To remove all the keys 
                    {
                        foreach (var key in lstKeys)
                        {
                            HttpContext.Current.Session.Remove(key);
                        }
                        HttpContext.Current.Session.Remove("tstivobjkey");
                    }

                    //if (TstIvKey != "" && HttpContext.Current.Session["dbmemvars_" + TstIvKey] != null)
                    //    HttpContext.Current.Session.Remove("dbmemvars_" + TstIvKey);
                }
            }
            catch (Exception ex) { }
        }

        public void ClearCachedObject(string objectType)
        {
            try
            {
                FDR fdrObj;
                if (HttpContext.Current.Session["FDR"] != null)
                    fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                else
                    fdrObj = new FDR();

                FDW fdwObj = new FDW();

                string project = string.Empty;
                string AxRole = string.Empty;

                project = HttpContext.Current.Session["project"].ToString();
                AxRole = HttpContext.Current.Session["AxRole"].ToString();

                switch (objectType)
                {
                    case "iview":
                        ArrayList arrKeys = fdrObj.GetAllKeys(project + "-*-iv-");
                        foreach (string key in arrKeys)
                        {
                            fdwObj.ClearRedisServerDataByKey(key, String.Empty, true);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Exception in ClearCachedObject - " + objectType + " - util.cs -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "ClearCachedObject", "new");
            }
        }

        public void SaveExecutionText()
        {
            if (ConfigurationManager.AppSettings["ExecutionTrace"] != null && ConfigurationManager.AppSettings["ExecutionTrace"].ToString() != string.Empty && ConfigurationManager.AppSettings["ExecutionTrace"].ToString() == "true")
            {
                string ExecutionLongText = string.Empty;
                FDW fdwObj = new FDW();
                string userName = string.Empty;
                if (HttpContext.Current.Session["username"] != null)
                    userName = HttpContext.Current.Session["username"].ToString();
                string sessId = HttpContext.Current.Session.SessionID;
                try
                {
                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                    string ExecTraceKey = "Executiontrace-" + userName + "-" + sessId;
                    ExecutionLongText = fdrObj.ReadKey(ExecTraceKey);

                }
                catch (Exception ex) { }
                try
                {
                    DBContext obj = new DBContext();
                    obj.SaveExecutionTrace(ExecutionLongText);

                    string ExecTraceKey = "Executiontrace-" + userName + "-" + sessId;
                    fdwObj.Deletekey(ExecTraceKey);
                }
                catch (Exception ex)
                {
                    string ExecTraceKey = "Executiontrace-" + userName + "-" + sessId;
                    fdwObj.Deletekey(ExecTraceKey);
                }
            }
            else
            {
                try
                {
                    FDW fdwObj = new FDW();
                    string userName = string.Empty;
                    if (HttpContext.Current.Session["username"] != null)
                        userName = HttpContext.Current.Session["username"].ToString();
                    string sessId = HttpContext.Current.Session.SessionID;
                    string ExecTraceKey = "Executiontrace-" + userName + "-" + sessId;
                    fdwObj.Deletekey(ExecTraceKey);
                }
                catch (Exception ex)
                { }
            }
        }

        public void SaveKeepAliveWebDetails(string currPageUrl)
        {
            try
            {
                string ipad = GetIpAddress();
                ipad = ipad.Replace(".", "1");
                string brOwner = HttpContext.Current.Session["loggedBroserId"].ToString();
                string urlDomain = System.Web.HttpContext.Current.Request.Url.ToString();
                if (urlDomain.ToLower().IndexOf("/webservice.asmx/") > -1)
                    urlDomain = urlDomain.Substring(0, urlDomain.ToLower().IndexOf("/webservice.asmx/"));
                else
                    urlDomain = urlDomain.Substring(0, urlDomain.ToLower().IndexOf("/aspx/mainnew.aspx"));

                ipad += "-" + brOwner + "-" + urlDomain;

                string sessionexpirydays = string.Empty;
                if (HttpContext.Current.Session["AxSessionExpiryDays"] != null && HttpContext.Current.Session["AxSessionExpiryDays"].ToString() != "")
                    sessionexpirydays = HttpContext.Current.Session["AxSessionExpiryDays"].ToString();
                if (sessionexpirydays != "0")
                {
                    FDW fdwObj = new FDW();
                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                    string userName = string.Empty;
                    string pwd = string.Empty;
                    if (HttpContext.Current.Session["username"] != null)
                        userName = HttpContext.Current.Session["username"].ToString();
                    if (HttpContext.Current.Session["pwd"] != null)
                        pwd = HttpContext.Current.Session["pwd"].ToString();

                    string kaProj = HttpContext.Current.Session["project"].ToString();
                    string kalanguage = HttpContext.Current.Session["language"].ToString();
                    string ssotype = string.Empty;
                    if (HttpContext.Current.Session["isSSOLogin"] != null && HttpContext.Current.Session["isSSOLogin"].ToString() == "True")
                        ssotype = HttpContext.Current.Session["SSOLoginType"].ToString();
                    string RsKey = fdrObj.MakeKeyName(Constants.REDISKEEPWEBINFO, ipad) + "-" + userName;

                    string KeepMeDetails = fdrObj.ReadKeyNoSchema(RsKey);
                    if (KeepMeDetails != string.Empty)
                    {
                        string[] KeepMeSavedInfo = KeepMeDetails.Split('~');
                        if (KeepMeSavedInfo[0] != kaProj || KeepMeSavedInfo[1] != userName || KeepMeSavedInfo[2] != pwd || KeepMeSavedInfo[3] != kalanguage || KeepMeSavedInfo[4] != urlDomain || KeepMeSavedInfo[5] != sessionexpirydays || KeepMeSavedInfo[6] != currPageUrl)
                        {
                            string datajson = kaProj + "~" + userName + "~" + pwd + "~" + kalanguage + "~" + urlDomain + "~" + sessionexpirydays + "~" + currPageUrl + "~" + ssotype;
                            int expiryTime = int.Parse(sessionexpirydays) * 24 * 60;
                            fdwObj.WriteKeyNoSchema(RsKey, datajson, expiryTime);
                        }
                    }
                    else
                    {
                        string datajson = kaProj + "~" + userName + "~" + pwd + "~" + kalanguage + "~" + urlDomain + "~" + sessionexpirydays + "~" + currPageUrl + "~" + ssotype;
                        int expiryTime = int.Parse(sessionexpirydays) * 24 * 60;
                        fdwObj.WriteKeyNoSchema(RsKey, datajson, expiryTime);
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void DeleteKeepAliveWebKey()
        {
            try
            {
                if (HttpContext.Current.Session["KeepMeWebExpiry"] == null)
                {
                    string ipad = GetIpAddress();
                    ipad = ipad.Replace(".", "1");
                    string brOwner = HttpContext.Current.Session["loggedBroserId"].ToString();
                    string urlDomain = System.Web.HttpContext.Current.Request.Url.ToString();
                    if (urlDomain.ToLower().IndexOf("/webservice.asmx/") > -1)
                        urlDomain = urlDomain.Substring(0, urlDomain.ToLower().IndexOf("/webservice.asmx/"));
                    else
                        urlDomain = urlDomain.Substring(0, urlDomain.ToLower().IndexOf("/aspx/mainnew.aspx"));

                    ipad += "-" + brOwner + "-" + urlDomain;

                    FDW fdwObj = new FDW();
                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                    ArrayList lstKeys = new ArrayList();

                    string userName = string.Empty;
                    if (HttpContext.Current.Session["username"] != null)
                        userName = HttpContext.Current.Session["username"].ToString();
                    string RsKey = fdrObj.MakeKeyName(Constants.REDISKEEPWEBINFO, ipad) + "-" + userName;

                    lstKeys.Add(RsKey);
                    fdwObj.DeleteKeys(lstKeys);
                }

                if (HttpContext.Current.Session["KeepMeHybridExpiry"] == null && HttpContext.Current.Session["hybridDeviceId"] != null && HttpContext.Current.Session["hybridDeviceId"].ToString() != "")
                {
                    string hgDevId = HttpContext.Current.Session["hybridDeviceId"].ToString();
                    FDW fdwObj = new FDW();
                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                    ArrayList lstKeys = new ArrayList();
                    lstKeys.Add(fdrObj.MakeKeyName(Constants.REDISHYBRIDINFO, hgDevId));
                    fdwObj.DeleteKeys(lstKeys);
                }
            }
            catch (Exception ex) { }
        }

        public string getSqlParameters(string sql)
        {
            string returnParams = string.Empty;

            ArrayList paramss = new ArrayList();

            string pattern1 = @"(:)+[a-zA-Z0-9]{1,}";

            Regex rgx1 = new Regex(pattern1);

            string pattern2 = @"(\{(?:\[??[^\[]*?\}))*";

            Regex rgx2 = new Regex(pattern2);

            foreach (Match match in rgx1.Matches(sql))
            {
                string replaceStr = Regex.Replace(match.Value, @"[:\s]", "");

                if (match.Value != "" && match.Value.IndexOf("::") != 0 && !paramss.Contains(replaceStr))
                {
                    paramss.Add(replaceStr);
                }
            }

            foreach (Match match in rgx2.Matches(sql))
            {
                string replaceStr = Regex.Replace(match.Value, @"[{\s]", "");
                replaceStr = Regex.Replace(replaceStr, @"[}\s]", "");

                if (match.Value != "" && !paramss.Contains(replaceStr) && !replaceStr.StartsWith("dynamicfilter"))
                {
                    paramss.Add(replaceStr);
                }
            }

            //foreach (string par in paramss)
            //{
            //    //Console.WriteLine(par);
            //}

            return string.Join(",", paramss.ToArray());
        }

        public string getExpiredCache(string json, JObject identifiers)
        {
            //identifiers = {
            //    "id": "axp_cardsid",
            //    "cache": "cachedata",
            //    "cachedTime": "cachedTime",
            //    "refreshAfter": "autorefresh"
            //}
            string dbType = string.Empty;
            if (HttpContext.Current.Session["axdb"] != null && HttpContext.Current.Session["axdb"].ToString() != string.Empty)
            {
                dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
            }

            string result = string.Empty;

            ArrayList expiredList = new ArrayList();

            JArray jsonArray = new JArray();

            try
            {
                jsonArray = JArray.Parse(json);
            }
            catch (Exception ex) { }

            if (jsonArray.Count > 0)
            {
                int ind = -1;
                foreach (JObject obj in jsonArray)
                {
                    if (dbType.ToLower() == "oracle" && obj[identifiers["id"].ToString()] == null && obj[identifiers["id"].ToString().ToUpper()] != null)
                    {
                        identifiers["id"] = identifiers["id"].ToString().ToUpper();
                        identifiers["cache"] = identifiers["cache"].ToString().ToUpper();
                        //identifiers["cachedTime"] = identifiers["cachedTime"].ToString().ToUpper();
                        identifiers["refreshAfter"] = identifiers["refreshAfter"].ToString().ToUpper();
                    }

                    ind++;
                    if (obj != null && obj[identifiers["cachedTime"].ToString()] != null && obj[identifiers["cache"].ToString()] != null && obj[identifiers["cache"].ToString()].ToString() == "true" && obj[identifiers["refreshAfter"].ToString()] != null && obj[identifiers["refreshAfter"].ToString()].ToString() != "" && obj[identifiers["refreshAfter"].ToString()].ToString() != "0")
                    {
                        long oldAddedTime = long.Parse(DateTime.ParseExact(obj[identifiers["cachedTime"].ToString()].ToString(), "ddMMyyyyHHmm", null).AddMinutes(int.Parse(obj[identifiers["refreshAfter"].ToString()].ToString())).ToString("ddMMyyyyHHmm"));
                        long latestTime = long.Parse(DateTime.Now.ToString("ddMMyyyyHHmm"));
                        if (oldAddedTime <= latestTime)
                        {
                            expiredList.Add(obj[identifiers["id"].ToString()]);
                        }
                    }
                    else if (obj != null && (obj[identifiers["cachedTime"].ToString()] == null || obj[identifiers["cachedTime"].ToString()].ToString() == ""))
                    {
                        if (obj["isDummy"] != null && obj["isDummy"].ToString() == "True")
                        {
                            //Do nothing
                        }
                        else
                            expiredList.Add(obj[identifiers["id"].ToString()]);
                    }
                    else if ((obj[identifiers["cache"].ToString().ToString()] != null && obj[identifiers["cache"].ToString()].ToString() == "false") || (obj[identifiers["refreshAfter"].ToString()] != null && obj[identifiers["refreshAfter"].ToString()].ToString() != "" && obj[identifiers["refreshAfter"].ToString()].ToString() == "0"))
                    {
                        expiredList.Add(obj[identifiers["id"].ToString()]);
                    }
                }
            }

            result = string.Join(",", expiredList.ToArray());

            return result;
        }

        public void CreateParamaterArray()
        {
            StringBuilder paramlist = new StringBuilder();
            string value = string.Empty;
            int paramCnt = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(HttpContext.Current.Session["axGlobalVars"].ToString());
            XmlNode globNode = xmlDoc.SelectSingleNode("/globalvars");
            foreach (XmlNode xmlNode in globNode.ChildNodes)
            {
                string strVar = xmlNode.Name;
                value = CheckSpecialChars(xmlNode.InnerText);
                if (!Constants.globalVarPathVaribles.Contains(strVar.ToLower()))
                {
                    paramlist.Append("Parameters[" + paramCnt + "] = " + "\"" + strVar + "~" + value + "\";  ");
                    paramCnt++;
                }
                if (strVar.ToString() == "project" && value.Contains(','))
                    HttpContext.Current.Session[strVar.ToString()] = value.Split(',')[1];
                else
                    HttpContext.Current.Session[strVar.ToString()] = value;
            }
            Boolean useCulture = false;
            string dtCulture = HttpContext.Current.Request.UserLanguages[0];
            if (HttpContext.Current.Session["AxUSCulture"] != null)
                useCulture = Convert.ToBoolean(HttpContext.Current.Session["AxUSCulture"].ToString());
            if (!useCulture)
                dtCulture = "en-gb";
            else
            {
                if (dtCulture.ToLower() == "en" || dtCulture.ToLower() == "en-us")
                    dtCulture = "en-us";
            }
            paramlist.Append("Parameters[" + paramCnt + "] = " + "\"username~" + HttpContext.Current.Session["user"].ToString() + "\";  ");
            paramlist.Append("Parameters[" + (paramCnt + 1) + "] = " + "\"Culture~" + dtCulture + "\";  ");

            HttpContext.Current.Session["ClientLocale"] = dtCulture;
            if (paramlist.ToString() != string.Empty)
                SetGlobalVarString(paramlist.ToString());
        }

        public string GetDBMemVarsXML(string _thisTid)
        {
            string dbmemvarsXML = string.Empty;
            try
            {
                if (HttpContext.Current.Session["forms_transids"] != null && HttpContext.Current.Session["forms_transids"].ToString() != "")
                {
                    string[] dbVarformloadList = HttpContext.Current.Session["forms_transids"].ToString().Split(',');
                    var isDbVarExist = dbVarformloadList.AsEnumerable().Where(x => x == _thisTid).ToList();
                    if (isDbVarExist.Count > 0)
                    {
                        string schemaname = string.Empty;
                        if (HttpContext.Current.Session["dbuser"] != null)
                            schemaname = HttpContext.Current.Session["dbuser"].ToString();
                        string user = HttpContext.Current.Session["user"].ToString();
                        string fdKey = Constants.DBMEMVARSFORMLOAD;
                        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                        if (fObj != null)
                            dbmemvarsXML = fObj.StringFromRedis(GetRedisServerkey(fdKey, _thisTid, user), schemaname);
                        if (dbmemvarsXML == String.Empty && HttpContext.Current.Session["dbmemvars_" + _thisTid] != null && HttpContext.Current.Session["dbmemvars_" + _thisTid].ToString() != "")
                            dbmemvarsXML = HttpContext.Current.Session["dbmemvars_" + _thisTid].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (HttpContext.Current.Session["dbmemvars_" + _thisTid] != null && HttpContext.Current.Session["dbmemvars_" + _thisTid].ToString() != "")
                    dbmemvarsXML = HttpContext.Current.Session["dbmemvars_" + _thisTid].ToString();
                else
                    dbmemvarsXML = string.Empty;
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetDBMemVariables XML  - " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetDBMemVarsXML-" + _thisTid, "new");
            }
            return dbmemvarsXML;
        }

        public string GetAxRuleParams(string sqlQuery)
        {
            string paramValues = string.Empty;
            string sqlQueryNew = sqlQuery;
            try
            {
                string globalVar = HttpContext.Current.Session["axGlobalVars"].ToString();
                XmlDocument xmlDocgbl = new XmlDocument();
                xmlDocgbl.LoadXml(globalVar);
                string blgAppVar = string.Empty;
                if (xmlDocgbl.SelectSingleNode("globalvars/appvartypes") != null)
                    blgAppVar = xmlDocgbl.SelectSingleNode("globalvars/appvartypes").InnerXml;
                foreach (XmlNode parms in xmlDocgbl.ChildNodes[0].ChildNodes)
                {
                    if (Regex.Match(sqlQuery, String.Format(@":\b{0}\b", parms.Name.ToString()), RegexOptions.IgnoreCase).Success && parms.Name != "appvartypes")
                    {
                        paramValues += parms.Name + ":" + parms.InnerXml + "~";
                        sqlQueryNew = sqlQueryNew.Replace(@":" + parms.Name, "'" + parms.InnerXml + "'");
                    }
                }
            }
            catch (Exception ex) { }
            return paramValues + "♠" + sqlQueryNew;
        }

        public string GetConfigDataVarsXML(string _thisTid)
        {
            string cdVarsXML = string.Empty;
            try
            {
                if (HttpContext.Current.Session["configparam_transids"] != null && HttpContext.Current.Session["configparam_transids"].ToString() != "")
                {
                    string[] dbVarformloadList = HttpContext.Current.Session["configparam_transids"].ToString().Split(',');
                    var isDbVarExist = dbVarformloadList.AsEnumerable().Where(x => x == _thisTid).ToList();
                    if (isDbVarExist.Count > 0)
                    {
                        string schemaname = string.Empty;
                        if (HttpContext.Current.Session["dbuser"] != null)
                            schemaname = HttpContext.Current.Session["dbuser"].ToString();
                        string user = HttpContext.Current.Session["user"].ToString();
                        string fdKey = Constants.CONFIGDATAVARSFORMLOAD;
                        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                        if (fObj != null)
                            cdVarsXML = fObj.StringFromRedis(GetRedisServerkey(fdKey, _thisTid), schemaname);
                        if (cdVarsXML == String.Empty && HttpContext.Current.Session["configdata_" + _thisTid] != null && HttpContext.Current.Session["configdata_" + _thisTid].ToString() != "")
                            cdVarsXML = HttpContext.Current.Session["configdata_" + _thisTid].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (HttpContext.Current.Session["configdata_" + _thisTid] != null && HttpContext.Current.Session["configdata_" + _thisTid].ToString() != "")
                    cdVarsXML = HttpContext.Current.Session["configdata_" + _thisTid].ToString();
                else
                    cdVarsXML = string.Empty;
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetConfigDataVarsXML XML  - " + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetConfigDataVarsXML-" + _thisTid, "new");
            }
            return cdVarsXML;
        }
        public string GetUserLevelLangFromDB(string username)
        {
            string axulang = string.Empty;
            string sqlQuery = string.Empty;
            try
            {
                if (HttpContext.Current.Session["AxDirectDBException"] != null)
                    HttpContext.Current.Session.Remove("AxDirectDBException");
                DBContext objDbCont = new DBContext();
                DataTable dt = new DataTable();
                sqlQuery = Constants.SQL_GET_AXUSERLANG;
                sqlQuery = sqlQuery.Replace("$USERNAME$", username);
                dt = objDbCont.GetDataTableInline(sqlQuery);
                if (dt != null && dt.Rows.Count > 0)
                {
                    axulang = dt.Rows[0]["AXLANG"].ToString();
                    axulang += "♣" + dt.Rows[0]["PWDAUTH"].ToString();
                    axulang += "♣" + dt.Rows[0]["OTPAUTH"].ToString();
                    axulang += "♣" + dt.Rows[0]["ACTIVE"].ToString();
                    axulang += "♣" + dt.Rows[0]["EMAIL"].ToString();
                    axulang += "♣" + dt.Rows[0]["MOBILE"].ToString();
                }
            }
            catch (Exception ex)
            {
                string _dberror = string.Empty;
                if (HttpContext.Current.Session["AxDirectDBException"] != null)
                {
                    _dberror = "error:" + HttpContext.Current.Session["AxDirectDBException"].ToString();
                    HttpContext.Current.Session.Remove("AxDirectDBException");
                }
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetUserLevelLangFromDB: Query: " + sqlQuery + " Exception:" + ex.Message + " DB Error:" + _dberror, HttpContext.Current.Session.SessionID, "GetUserLevelLangFromDB-exception", "new", "true");
                axulang = _dberror;
            }
            return axulang;
        }

        #region GetAxvalErrorcodes from Redis
        public string GetAxvalErrorcode(string transId)
        {
            string AxErroCodes = string.Empty;
            try
            {
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                string conErrCode = Constants.AXVALERRORCODE;
                AxErroCodes = fObj.StringFromRedis(GetRedisServerkey(conErrCode, transId), schemaName);
            }
            catch (Exception ex)
            {
                AxErroCodes = string.Empty;
            }
            return AxErroCodes;
        }
        #endregion

        public string GetRedisConnDetails(string thisProj)
        {
            string rcDetails = string.Empty;
            try
            {

                string projName = string.Empty;
                if (!string.IsNullOrEmpty(thisProj))
                    projName = thisProj;
                else if (HttpContext.Current.Session["project"] != null)
                {
                    projName = HttpContext.Current.Session["project"].ToString();
                }
                if (HttpContext.Current.Application["AppSettingsIni"] != null)
                {
                    JObject redisObj = JObject.Parse(HttpContext.Current.Application["AppSettingsIni"].ToString());
                    if (redisObj["appsettings"] != null && redisObj["appsettings"][projName] != null && redisObj["appsettings"][projName]["Redis"] != null)
                    {
                        string redisIp = string.Empty;
                        string redisPort = string.Empty;
                        string redisPass = string.Empty;

                        JObject connDetails = redisObj["appsettings"][projName]["Redis"] as JObject;
                        if (connDetails != null)
                        {
                            redisIp = connDetails.Value<string>("host") ?? string.Empty;
                            if (redisIp != "" && redisIp == "localhost")
                                return "localhost";
                            redisPort = connDetails.Value<string>("port") ?? string.Empty;
                            redisPass = connDetails.Value<string>("pwd") ?? string.Empty;

                            if (!string.IsNullOrEmpty(redisPass))
                            {
                                redisPass = DecryptPWD(redisPass);
                            }
                        }

                        if (!string.IsNullOrEmpty(redisIp))
                        {
                            redisIp += ":" + redisPort;
                            rcDetails = redisIp + "♣" + redisPass;

                            if (HttpContext.Current.Session != null && HttpContext.Current.Session["project"] != null)
                            {
                                HttpContext.Current.Session["RedisCacheIP"] = redisIp;
                                HttpContext.Current.Session["RedisCachePwd"] = redisPass;
                            }
                        }
                    }
                    else
                    {
                        if (HttpContext.Current.Session != null && HttpContext.Current.Session["project"] != null)
                        {
                            HttpContext.Current.Session.Remove("RedisCacheIP");
                            HttpContext.Current.Session.Remove("RedisCachePwd");
                        }
                    }
                }
                else
                {
                    string jsonPath = Path.Combine(ScriptsPath, "AppSettings.ini");
                    FileInfo fi = new FileInfo(jsonPath);
                    if (fi.Exists)
                    {
                        string jsonText = File.ReadAllText(jsonPath);
                        HttpContext.Current.Application["AppSettingsIni"] = jsonText;
                        JObject redisObj = JObject.Parse(jsonText);
                        if (redisObj["appsettings"] != null && redisObj["appsettings"][projName] != null && redisObj["appsettings"][projName]["Redis"] != null)
                        {
                            string redisIp = string.Empty;
                            string redisPort = string.Empty;
                            string redisPass = string.Empty;

                            JObject connDetails = redisObj["appsettings"][projName]["Redis"] as JObject;
                            if (connDetails != null)
                            {
                                redisIp = connDetails.Value<string>("host") ?? string.Empty;
                                if (redisIp != "" && redisIp == "localhost")
                                    return "localhost";
                                redisPort = connDetails.Value<string>("port") ?? string.Empty;
                                redisPass = connDetails.Value<string>("pwd") ?? string.Empty;

                                if (!string.IsNullOrEmpty(redisPass))
                                {
                                    redisPass = DecryptPWD(redisPass);
                                }
                            }

                            if (!string.IsNullOrEmpty(redisIp))
                            {
                                redisIp += ":" + redisPort;
                                rcDetails = redisIp + "♣" + redisPass;

                                if (HttpContext.Current.Session != null)
                                {
                                    HttpContext.Current.Session["RedisCacheIP"] = redisIp;
                                    HttpContext.Current.Session["RedisCachePwd"] = redisPass;
                                }
                            }
                        }
                        else
                        {
                            if (HttpContext.Current.Session != null)
                            {
                                HttpContext.Current.Session.Remove("RedisCacheIP");
                                HttpContext.Current.Session.Remove("RedisCachePwd");
                            }
                        }
                    }
                    else
                    {
                        if (HttpContext.Current.Session != null)
                        {
                            HttpContext.Current.Session.Remove("RedisCacheIP");
                            HttpContext.Current.Session.Remove("RedisCachePwd");
                        }
                    }
                }

                //}
                //else
                //{
                //    if (HttpContext.Current.Session != null)
                //    {
                //        HttpContext.Current.Session.Remove("RedisCacheIP");
                //        HttpContext.Current.Session.Remove("RedisCachePwd");
                //    }
                //}
            }
            catch (Exception ex)
            {
                rcDetails = string.Empty;
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetRedisConnDetails  - " + ex.Message, HttpContext.Current.Session != null ? HttpContext.Current.Session.SessionID : "", "RedisConnDetails", "new");
            }
            return rcDetails;
        }

        public string GetAxpLicRedisConnDetails()
        {
            string rcDetails = string.Empty;
            try
            {
                string rConnName = string.Empty;
                if (ConfigurationManager.AppSettings["axpLicRedisConnection"] != null && ConfigurationManager.AppSettings["axpLicRedisConnection"].ToString() != "")
                {
                    rConnName = ConfigurationManager.AppSettings["axpLicRedisConnection"].ToString();
                }
                if (!string.IsNullOrEmpty(rConnName))
                {
                    string jsonFile = Path.Combine(ScriptsPath, "AppSettings.ini");
                    FileInfo fi = new FileInfo(jsonFile);
                    if (fi.Exists)
                    {
                        string jsonText = File.ReadAllText(jsonFile);
                        JObject jObj = JObject.Parse(jsonText);

                        JObject connections = jObj["redisconnections"] as JObject;
                        if (connections != null && connections[rConnName] != null)
                        {
                            string redisIp = string.Empty;
                            string redisPort = string.Empty;
                            string redisPass = string.Empty;

                            JObject connDetails = connections[rConnName] as JObject;
                            if (connDetails != null)
                            {
                                redisIp = connDetails.Value<string>("host") ?? string.Empty;
                                redisPort = connDetails.Value<string>("port") ?? string.Empty;
                                redisPass = connDetails.Value<string>("pwd") ?? string.Empty;

                                if (!string.IsNullOrEmpty(redisPass))
                                {
                                    redisPass = DecryptPWD(redisPass);
                                }
                            }

                            if (!string.IsNullOrEmpty(redisIp))
                            {
                                redisIp += ":" + redisPort;
                                rcDetails = redisIp + "♣" + redisPass;

                                if (HttpContext.Current.Session != null)
                                {
                                    HttpContext.Current.Session["axpLic_RedisIP"] = redisIp;
                                    HttpContext.Current.Session["axpLic_RedisPwd"] = redisPass;
                                }
                            }
                        }
                        else
                        {
                            if (HttpContext.Current.Session != null)
                            {
                                HttpContext.Current.Session.Remove("axpLic_RedisIP");
                                HttpContext.Current.Session.Remove("axpLic_RedisPwd");
                            }
                        }
                    }
                }
                else
                {
                    if (HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session.Remove("axpLic_RedisIP");
                        HttpContext.Current.Session.Remove("axpLic_RedisPwd");
                    }
                }
            }
            catch (Exception ex)
            {
                rcDetails = string.Empty;
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetAxpLicRedisConnDetails  - " + ex.Message, HttpContext.Current.Session != null ? HttpContext.Current.Session.SessionID : "", "AxpLicRedisConnDetails", "new");
            }
            return rcDetails;
        }

        public string MD5Hash(string text)
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
        public void GetAxARMConnection(string strProj)
        {
            string jsoncontents = string.Empty;
            if (strProj != string.Empty)
            {
                try
                {
                    FDR fdrObj = new FDR();
                    jsoncontents = fdrObj.StringFromRedis(Constants.AXARM_CONN_KEY, strProj);
                    if (jsoncontents == string.Empty)
                    {
                        FileInfo fi = new FileInfo(ScriptsPath + "\\AppSettings.ini");
                        if (fi.Exists)
                        {
                            string scriptsPathARM = ScriptsPath + "\\AppSettings.ini";
                            string filePatharm = @" " + scriptsPathARM + "";
                            string existingJsonARM = File.ReadAllText(filePatharm);
                            JObject jsonARM = JObject.Parse(existingJsonARM);
                            if (jsonARM["appsettings"] != null && jsonARM["appsettings"][strProj] != null && jsonARM["appsettings"][strProj]["ARM"] != null)
                            {
                                jsoncontents = jsonARM["appsettings"][strProj]["ARM"].ToString();
                                try
                                {
                                    JObject _jsonARM = JObject.Parse(jsoncontents);
                                    if (_jsonARM["ARM_PrivateKey"] != null)
                                        HttpContext.Current.Session["ARM_PrivateKey"] = _jsonARM["ARM_PrivateKey"].ToString();
                                    if (_jsonARM["ARM_URL"] != null)
                                        HttpContext.Current.Session["ARM_URL"] = _jsonARM["ARM_URL"].ToString();
                                    if (_jsonARM["ARM_Notification_URL"] != null)
                                        HttpContext.Current.Session["ARM_Notification_URL"] = _jsonARM["ARM_Notification_URL"].ToString();
                                    if (_jsonARM["ARM_Scripts_URL"] != null)
                                        HttpContext.Current.Session["ARM_Scripts_URL"] = _jsonARM["ARM_Scripts_URL"].ToString();
                                    HttpContext.Current.Session["ARMPushToQueue_API"] = "/api/v1/ARMPushToQueue";
                                    if (_jsonARM["PEG"] != null)
                                        HttpContext.Current.Session["ARMPEG"] = _jsonARM["PEG"].ToString();
                                }
                                catch (Exception ex)
                                {
                                    LogFile.Log logobj = new LogFile.Log();
                                    logobj.CreateLog("Exception in GetAxARMConnection while reading from file - util.cs- jsoncontents:" + jsoncontents + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxARMConn-ex", "new");
                                }

                                FDW fdwObj = new FDW(strProj);
                                fdwObj.SaveInRedisServer(Constants.AXARM_CONN_KEY, jsoncontents, Constants.AXARM_CONN_KEY, strProj);
                            }
                            else
                            {
                                FDW fdwObj = new FDW(strProj);
                                fdwObj.SaveInRedisServer(Constants.AXARM_CONN_KEY, "noarmconnection", Constants.AXARM_CONN_KEY, strProj);
                            }
                        }
                        else
                        {
                            FDW fdwObj = new FDW(strProj);
                            fdwObj.SaveInRedisServer(Constants.AXARM_CONN_KEY, "noarmconnection", Constants.AXARM_CONN_KEY, strProj);
                        }
                    }
                    else if (jsoncontents != "noarmconnection")
                    {
                        try
                        {
                            JObject _jsonARM = JObject.Parse(jsoncontents);
                            if (_jsonARM["ARM_PrivateKey"] != null)
                                HttpContext.Current.Session["ARM_PrivateKey"] = _jsonARM["ARM_PrivateKey"].ToString();
                            if (_jsonARM["ARM_URL"] != null)
                                HttpContext.Current.Session["ARM_URL"] = _jsonARM["ARM_URL"].ToString();
                            if (_jsonARM["ARM_Notification_URL"] != null)
                                HttpContext.Current.Session["ARM_Notification_URL"] = _jsonARM["ARM_Notification_URL"].ToString();
                            if (_jsonARM["ARM_Scripts_URL"] != null)
                                HttpContext.Current.Session["ARM_Scripts_URL"] = _jsonARM["ARM_Scripts_URL"].ToString();
                            HttpContext.Current.Session["ARMPushToQueue_API"] = "/api/v1/ARMPushToQueue";
                            if (_jsonARM["PEG"] != null)
                                HttpContext.Current.Session["ARMPEG"] = _jsonARM["PEG"].ToString();
                        }
                        catch (Exception ex)
                        {
                            LogFile.Log logobj = new LogFile.Log();
                            logobj.CreateLog("Exception in GetAxARMConnection while reading from cache - util.cs- jsoncontents:" + jsoncontents + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxARMConn-Exception", "new");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFile.Log logobj = new LogFile.Log();
                    logobj.CreateLog("Exception in GetAxARMConnection - util.cs-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxARMConnection", "new");
                }

                GetAxRMQSettings(strProj);

                GetAxARMInternalResources(strProj);

                GetAxARMExternalResources(strProj);
            }
        }
        public string GetAxARMInternalResources(string strProj)
        {
            string jsoncontents = string.Empty;
            if (strProj != string.Empty)
            {
                try
                {
                    FDR fdrObj = new FDR();
                    jsoncontents = fdrObj.StringFromRedis(Constants.AXARM_IntRes_CONN_KEY, strProj);
                    if (jsoncontents == string.Empty)
                    {
                        FileInfo fi = new FileInfo(ScriptsPath + "\\AppSettings.ini");
                        if (fi.Exists)
                        {
                            string scriptsPathARM = ScriptsPath + "\\AppSettings.ini";
                            string filePatharm = @" " + scriptsPathARM + "";
                            string existingJsonARM = File.ReadAllText(filePatharm);
                            JObject jsonARM = JObject.Parse(existingJsonARM);
                            if (jsonARM["appsettings"] != null && jsonARM["appsettings"][strProj] != null && jsonARM["appsettings"][strProj]["InternalResources"] != null)
                            {
                                jsoncontents = jsonARM["appsettings"][strProj]["InternalResources"].ToString();
                                try
                                {
                                    JObject _jsonExtRes = JObject.Parse(jsoncontents);
                                    if (_jsonExtRes["AxRMQAPIURL"] != null)
                                        HttpContext.Current.Session["RMQueueAPIURL"] = _jsonExtRes["AxRMQAPIURL"].ToString();
                                    if (_jsonExtRes["AxSignalRapiURL"] != null)
                                        HttpContext.Current.Session["SignalRAPIURL"] = _jsonExtRes["AxSignalRapiURL"].ToString();
                                    if (_jsonExtRes["AxFCMSendMsgURL"] != null)
                                        HttpContext.Current.Session["FCMSendMsgURL"] = _jsonExtRes["AxFCMSendMsgURL"].ToString();
                                    if (_jsonExtRes["AxRapidSaveURL"] != null)
                                        HttpContext.Current.Session["RapidSaveURL"] = _jsonExtRes["AxRapidSaveURL"].ToString();
                                    if (_jsonExtRes["axpegemailactionurl"] != null)
                                        HttpContext.Current.Session["pegemailactionurl"] = _jsonExtRes["axpegemailactionurl"].ToString();
                                    if (_jsonExtRes["AxScriptsAPIURL"] != null)
                                        HttpContext.Current.Session["ScriptsAPIURL"] = _jsonExtRes["AxScriptsAPIURL"].ToString();
                                }
                                catch (Exception ex)
                                {
                                    LogFile.Log logobj = new LogFile.Log();
                                    logobj.CreateLog("Exception in GetAxARMInternalResources while reading from file - util.cs- jsoncontents:" + jsoncontents + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxARMInternalResources-ex", "new");
                                }

                                FDW fdwObj = new FDW(strProj);
                                fdwObj.SaveInRedisServer(Constants.AXARM_IntRes_CONN_KEY, jsoncontents, Constants.AXARM_IntRes_CONN_KEY, strProj);
                            }
                            else
                            {
                                FDW fdwObj = new FDW(strProj);
                                fdwObj.SaveInRedisServer(Constants.AXARM_IntRes_CONN_KEY, "noarmconnection", Constants.AXARM_IntRes_CONN_KEY, strProj);
                            }
                        }
                        else
                        {
                            FDW fdwObj = new FDW(strProj);
                            fdwObj.SaveInRedisServer(Constants.AXARM_IntRes_CONN_KEY, "noarmconnection", Constants.AXARM_IntRes_CONN_KEY, strProj);
                        }
                    }
                    else if (jsoncontents != "noarmconnection")
                    {
                        try
                        {
                            JObject _jsonExtRes = JObject.Parse(jsoncontents);
                            if (_jsonExtRes["AxRMQAPIURL"] != null)
                                HttpContext.Current.Session["RMQueueAPIURL"] = _jsonExtRes["AxRMQAPIURL"].ToString();
                            if (_jsonExtRes["AxSignalRapiURL"] != null)
                                HttpContext.Current.Session["SignalRAPIURL"] = _jsonExtRes["AxSignalRapiURL"].ToString();
                            if (_jsonExtRes["AxFCMSendMsgURL"] != null)
                                HttpContext.Current.Session["FCMSendMsgURL"] = _jsonExtRes["AxFCMSendMsgURL"].ToString();
                            if (_jsonExtRes["AxRapidSaveURL"] != null)
                                HttpContext.Current.Session["RapidSaveURL"] = _jsonExtRes["AxRapidSaveURL"].ToString();
                            if (_jsonExtRes["axpegemailactionurl"] != null)
                                HttpContext.Current.Session["pegemailactionurl"] = _jsonExtRes["axpegemailactionurl"].ToString();
                            if (_jsonExtRes["AxScriptsAPIURL"] != null)
                                HttpContext.Current.Session["ScriptsAPIURL"] = _jsonExtRes["AxScriptsAPIURL"].ToString();
                        }
                        catch (Exception ex)
                        {
                            LogFile.Log logobj = new LogFile.Log();
                            logobj.CreateLog("Exception in GetAxARMInternalResources while reading from cache - util.cs- jsoncontents:" + jsoncontents + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxARMInternalResources-Exception", "new");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFile.Log logobj = new LogFile.Log();
                    logobj.CreateLog("Exception in GetAxARMInternalResources - util.cs-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxARMInternalResources", "new");
                }
            }
            return jsoncontents;
        }

        public string GetAxARMExternalResources(string strProj)
        {
            string jsoncontents = string.Empty;
            if (strProj != string.Empty)
            {
                try
                {
                    FDR fdrObj = new FDR();
                    jsoncontents = fdrObj.StringFromRedis(Constants.AXARM_ExtRes_CONN_KEY, strProj);
                    if (jsoncontents == string.Empty)
                    {
                        FileInfo fi = new FileInfo(ScriptsPath + "\\AppSettings.ini");
                        if (fi.Exists)
                        {
                            string scriptsPathARM = ScriptsPath + "\\AppSettings.ini";
                            string filePatharm = @" " + scriptsPathARM + "";
                            string existingJsonARM = File.ReadAllText(filePatharm);
                            JObject jsonARM = JObject.Parse(existingJsonARM);
                            if (jsonARM["appsettings"] != null && jsonARM["appsettings"][strProj] != null && jsonARM["appsettings"][strProj]["ExternalResources"] != null)
                            {
                                jsoncontents = jsonARM["appsettings"][strProj]["ExternalResources"].ToString();

                                FDW fdwObj = new FDW(strProj);
                                fdwObj.SaveInRedisServer(Constants.AXARM_ExtRes_CONN_KEY, jsoncontents, Constants.AXARM_ExtRes_CONN_KEY, strProj);
                            }
                            else
                            {
                                FDW fdwObj = new FDW(strProj);
                                fdwObj.SaveInRedisServer(Constants.AXARM_ExtRes_CONN_KEY, "noarmconnection", Constants.AXARM_ExtRes_CONN_KEY, strProj);
                            }
                        }
                        else
                        {
                            FDW fdwObj = new FDW(strProj);
                            fdwObj.SaveInRedisServer(Constants.AXARM_ExtRes_CONN_KEY, "noarmconnection", Constants.AXARM_ExtRes_CONN_KEY, strProj);
                        }
                    }
                    else if (jsoncontents != "noarmconnection")
                    {
                        return jsoncontents;
                    }
                }
                catch (Exception ex)
                {
                    LogFile.Log logobj = new LogFile.Log();
                    logobj.CreateLog("Exception in GetAxARMExternalResources - util.cs-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxARMExternalResources", "new");
                }
            }
            return jsoncontents;
        }

        public string GetAxRMQSettings(string strProj)
        {
            string jsoncontents = string.Empty;
            if (strProj != string.Empty)
            {
                try
                {
                    if (HttpContext.Current.Application["AppSettingsIni"] != null)
                    {
                        JObject redisObj = JObject.Parse(HttpContext.Current.Application["AppSettingsIni"].ToString());
                        if (redisObj["rmqsettings"] != null)
                        {
                            JObject _jsonRMQ = JObject.Parse(redisObj["rmqsettings"].ToString());
                            jsoncontents = "amqp://" + _jsonRMQ["RMQueueUser"].ToString() + ":" + _jsonRMQ["RMQueuePassword"].ToString() + "@" + _jsonRMQ["RMQueueHost"].ToString() + ":" + _jsonRMQ["RMQueuePort"].ToString();
                            HttpContext.Current.Session["AxRabbitMQSet"] = jsoncontents;
                        }
                    }
                    if (jsoncontents == string.Empty)
                    {
                        FileInfo fi = new FileInfo(ScriptsPath + "\\AppSettings.ini");
                        if (fi.Exists)
                        {
                            string scriptsPathARM = ScriptsPath + "\\AppSettings.ini";
                            string filePatharm = @" " + scriptsPathARM + "";
                            string existingJsonARM = File.ReadAllText(filePatharm);
                            JObject jsonARM = JObject.Parse(existingJsonARM);
                            if (jsonARM["rmqsettings"] != null)
                            {
                                jsoncontents = jsonARM["rmqsettings"].ToString();
                                JObject _jsonRMQ = JObject.Parse(jsoncontents);
                                jsoncontents = "amqp://" + _jsonRMQ["RMQueueUser"].ToString() + ":" + _jsonRMQ["RMQueuePassword"].ToString() + "@" + _jsonRMQ["RMQueueHost"].ToString() + ":" + _jsonRMQ["RMQueuePort"].ToString();
                                HttpContext.Current.Session["AxRabbitMQSet"] = jsoncontents;
                            }
                            else
                            {
                                HttpContext.Current.Session["AxRabbitMQSet"] = "noRMQSettings";
                            }
                        }
                        else
                        {
                            HttpContext.Current.Session["AxRabbitMQSet"] = "noRMQSettings";
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFile.Log logobj = new LogFile.Log();
                    logobj.CreateLog("Exception in GetAxRMQSettings - util.cs-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxRMQSettings", "new");
                }
            }
            return jsoncontents;
        }

        public void GetAxConfigFileServer(string strProj)
        {
            string jsoncontents = string.Empty;
            if (strProj != string.Empty)
            {
                try
                {
                    FDR fdrObj = new FDR();
                    jsoncontents = fdrObj.StringFromRedis(Constants.AXFileServer_CONN_KEY, strProj);
                    if (jsoncontents == string.Empty)
                    {
                        FileInfo fi = new FileInfo(ScriptsPath + "\\AppSettings.ini");
                        if (fi.Exists)
                        {
                            string scriptsPathAxFile = ScriptsPath + "\\AppSettings.ini";
                            string filePatharm = @" " + scriptsPathAxFile + "";
                            string existingJsonAxFile = File.ReadAllText(filePatharm);
                            JObject jsonAxFile = JObject.Parse(existingJsonAxFile);
                            if (jsonAxFile["appsettings"] != null && jsonAxFile["appsettings"][strProj] != null && jsonAxFile["appsettings"][strProj]["FileConfig"] != null)
                            {
                                jsoncontents = jsonAxFile["appsettings"][strProj]["FileConfig"].ToString();
                                try
                                {
                                    JObject _jsonAxFile = JObject.Parse(jsoncontents);
                                    if (_jsonAxFile["FileUploadPath"] != null)
                                        HttpContext.Current.Session["AxConfigFileUploadPath"] = _jsonAxFile["FileUploadPath"].ToString();
                                    if (_jsonAxFile["FileDownloadPath"] != null)
                                        HttpContext.Current.Session["AxConfigFileDownloadPath"] = _jsonAxFile["FileDownloadPath"].ToString();
                                    if (_jsonAxFile["FileServerMapUsername"] != null)
                                        HttpContext.Current.Session["AxConfigFileMapUser"] = _jsonAxFile["FileServerMapUsername"].ToString();
                                    if (_jsonAxFile["FileServerMapPwd"] != null)
                                        HttpContext.Current.Session["AxConfigFileMapPwd"] = _jsonAxFile["FileServerMapPwd"].ToString();
                                }
                                catch (Exception ex)
                                {
                                    LogFile.Log logobj = new LogFile.Log();
                                    logobj.CreateLog("Exception in GetAxConfigFileServer while reading from file - util.cs- jsoncontents:" + jsoncontents + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxConfigFileServer-ex", "new");
                                }

                                FDW fdwObj = new FDW(strProj);
                                fdwObj.SaveInRedisServer(Constants.AXFileServer_CONN_KEY, jsoncontents, Constants.AXFileServer_CONN_KEY, strProj);
                            }
                            else
                            {
                                FDW fdwObj = new FDW(strProj);
                                fdwObj.SaveInRedisServer(Constants.AXFileServer_CONN_KEY, "nofileserverconnection", Constants.AXFileServer_CONN_KEY, strProj);
                            }
                        }
                        else
                        {
                            FDW fdwObj = new FDW(strProj);
                            fdwObj.SaveInRedisServer(Constants.AXFileServer_CONN_KEY, "nofileserverconnection", Constants.AXFileServer_CONN_KEY, strProj);
                        }
                    }
                    else if (jsoncontents != "nofileserverconnection")
                    {
                        try
                        {
                            JObject _jsonAxFile = JObject.Parse(jsoncontents);
                            if (_jsonAxFile["FileUploadPath"] != null)
                                HttpContext.Current.Session["AxConfigFileUploadPath"] = _jsonAxFile["FileUploadPath"].ToString();
                            if (_jsonAxFile["FileDownloadPath"] != null)
                                HttpContext.Current.Session["AxConfigFileDownloadPath"] = _jsonAxFile["FileDownloadPath"].ToString();
                            if (_jsonAxFile["FileServerMapUsername"] != null)
                                HttpContext.Current.Session["AxConfigFileMapUser"] = _jsonAxFile["FileServerMapUsername"].ToString();
                            if (_jsonAxFile["FileServerMapPwd"] != null)
                                HttpContext.Current.Session["AxConfigFileMapPwd"] = _jsonAxFile["FileServerMapPwd"].ToString();
                        }
                        catch (Exception ex)
                        {
                            LogFile.Log logobj = new LogFile.Log();
                            logobj.CreateLog("Exception in GetAxConfigFileServer while reading from cache - util.cs- jsoncontents:" + jsoncontents + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxConfigFileServer-Exception", "new");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFile.Log logobj = new LogFile.Log();
                    logobj.CreateLog("Exception in GetAxConfigFileServer - util.cs-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxConfigFileServer", "new");
                }
            }
        }

        public void SaveOTPAuth(string _otp, string _uname, string otpAuthExpiry, string project)
        {
            try
            {
                string fdKeyOtpAuth = Constants.REDISOTPAUTH;
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDW fdwObj = new FDW(project);
                fdwObj.WriteKeyAutoExpireInSec(GetRedisServerkey(fdKeyOtpAuth, _uname), _otp, int.Parse(otpAuthExpiry) * 60);
            }
            catch (Exception ex)
            { }
        }

        public string GetOTPAuth(string _uname, string _proj)
        {
            string _otpAuth = string.Empty;
            try
            {
                string fdKeyOtpAuth = Constants.REDISOTPAUTH;
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                    fObj = new FDR(_proj);
                if (fObj != null)
                    _otpAuth = fObj.StringFromRedis(GetRedisServerkey(fdKeyOtpAuth, _uname), schemaName);
            }
            catch (Exception ex)
            { }
            return _otpAuth;
        }

        public string GetReGenTstDataObj(string _Key, string _result)
        {
            string objtstDatakey = string.Empty;
            try
            {
                string _thistrId = "";
                if (_result != "" && !_result.StartsWith("{"))
                {
                    _thistrId = _result;
                    _result = "";
                }
                else if (_result != "" && _result.StartsWith("{"))
                {
                    string[] splitRes = _result.Split(new[] { "♠*$" }, StringSplitOptions.None);
                    _thistrId = splitRes[1];
                    _result = splitRes[0];
                }
                TStructData tstData = (TStructData)HttpContext.Current.Session[_Key];
                if (tstData == null)
                {
                    CacheManager cacheMgr = GetCacheObject();
                    TStructDef strObj = GetStrObject(cacheMgr, _thistrId);
                    string _transId = _thistrId;
                    TStructData strDataObj = null;
                    string recId = "0";
                    if (_result != "" && _result.StartsWith("{"))
                    {
                        JObject _tstData = JObject.Parse(_result);
                        foreach (var _dtNode in _tstData["data"])
                        {
                            if (_dtNode["n"].ToString() == "axp_recid1")
                            {
                                recId = _dtNode["v"].ToString();
                                break;
                            }
                        }
                    }
                    strDataObj = new TStructData(_result, _transId, recId, strObj);
                    strDataObj.transid = _transId.ToString();
                    strDataObj.recordid = recId;
                    objtstDatakey = GetTstDataId(_transId);
                    HttpContext.Current.Session.Add(objtstDatakey, strDataObj);
                    tstIvObjUpdate(objtstDatakey);
                }
                else
                {
                    TStructDef strObj = tstData.tstStrObj;
                    string _transId = tstData.transID;
                    TStructData strDataObj = null;
                    string recId = "0";
                    if (_result != "" && _result.StartsWith("{"))
                    {
                        JObject _tstData = JObject.Parse(_result);
                        foreach (var _dtNode in _tstData["data"])
                        {
                            if (_dtNode["n"].ToString() == "axp_recid1")
                            {
                                recId = _dtNode["v"].ToString();
                                break;
                            }
                        }
                    }
                    strDataObj = new TStructData(_result, _transId, recId, strObj);
                    strDataObj.transid = _transId.ToString();
                    strDataObj.recordid = recId;
                    objtstDatakey = GetTstDataId(_transId);
                    HttpContext.Current.Session.Add(objtstDatakey, strDataObj);
                    tstIvObjUpdate(objtstDatakey);
                }
            }
            catch (Exception ex)
            {
                objtstDatakey = "";
            }
            return objtstDatakey;
        }

        public void tstIvObjUpdate(string NewKey)
        {
            if (HttpContext.Current.Session["isDupTab"] != null && HttpContext.Current.Session["isDupTab"].ToString().StartsWith("true-"))
            {
                string _thisDupTab = HttpContext.Current.Session["isDupTab"].ToString();
                _thisDupTab = _thisDupTab.Split('-')[1];
                HttpContext.Current.Session["isDupTab"] = "false";
                if (HttpContext.Current.Session["tstivobjkey-duptab-" + _thisDupTab] != null && HttpContext.Current.Session["tstivobjkey-duptab-" + _thisDupTab].ToString() != string.Empty)
                    HttpContext.Current.Session["tstivobjkey-duptab-" + _thisDupTab] = HttpContext.Current.Session["tstivobjkey-duptab-" + _thisDupTab].ToString() + "," + NewKey;
                else
                    HttpContext.Current.Session["tstivobjkey-duptab-" + _thisDupTab] = NewKey;
            }
            else
            {
                if (HttpContext.Current.Session["tstivobjkey"] != null && HttpContext.Current.Session["tstivobjkey"].ToString() != string.Empty)
                    HttpContext.Current.Session["tstivobjkey"] = HttpContext.Current.Session["tstivobjkey"].ToString() + "," + NewKey;
                else
                    HttpContext.Current.Session["tstivobjkey"] = NewKey;
            }
        }

        public object GetTstHtmllsKey(string transId, string flGlobalVarNode)
        {
            string flKey = string.Empty, devOptionKey = string.Empty, matchflGlobalVar = string.Empty;
            try
            {
                int keyIndex = -1;
                DataTable dtConfig = GetStructConfig(transId);
                string cacheTime = GetConfigSettings(transId, dtConfig);
                if (cacheTime != "none")
                {
                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                    flKey = fdrObj.MakeKeyName(Constants.REDISTSTHTMLLS, transId);
                    devOptionKey = cacheTime;
                    if (flGlobalVarNode != string.Empty)
                    {
                        flGlobalVarNode = flGlobalVarNode.Replace(";bkslh", "\\");
                        var keyList = flGlobalVarNode.Split('¿').ToList();
                        keyIndex = fdrObj.MakeVarKeyName(keyList);
                        if (keyIndex != -1)
                            matchflGlobalVar = keyList[keyIndex];
                    }
                }
                else
                    flKey = "none";
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetTstHtmlFormLoadKey -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in GetTstHtmlFormLoadKey", "new");
            }
            return new { flKey = flKey, flGlobalVars = flGlobalVarNode, devOption = devOptionKey, matchflGlobalVar = matchflGlobalVar };
        }

        public string SetTstHtmlLs(string loadRes, string transId, dynamic flGblExistingKeys, string usedVars)
        {
            string flgvValue = string.Empty;
            try
            {
                string flKey = flGblExistingKeys.GetType().GetProperty("flKey").GetValue(flGblExistingKeys, null);
                string flGlobalVars = flGblExistingKeys.GetType().GetProperty("flGlobalVars").GetValue(flGblExistingKeys, null);
                string devOption = flGblExistingKeys.GetType().GetProperty("devOption").GetValue(flGblExistingKeys, null);
                string matchflGlobalVar = flGblExistingKeys.GetType().GetProperty("matchflGlobalVar").GetValue(flGblExistingKeys, null);

                string stsGlobal = usedVars;// ParseJSonResultNode(usedVars);
                string strflKeys = string.Empty, strSignleKey = string.Empty;
                if (flGlobalVars == string.Empty)
                {
                    if (devOption != string.Empty)
                        strflKeys = stsGlobal + "♦" + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    else
                        strflKeys = stsGlobal + "♦" + DateTime.Now.ToString("dd/MM/yyyy");
                    strSignleKey = strflKeys;
                }
                else
                {
                    if (matchflGlobalVar == string.Empty)
                    {
                        if (devOption != string.Empty)
                        {
                            strSignleKey = stsGlobal + "♦" + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            strflKeys = flGlobalVars + "¿" + strSignleKey;
                        }
                        else
                        {
                            strSignleKey = stsGlobal + "♦" + DateTime.Now.ToString("dd/MM/yyyy");
                            strflKeys = flGlobalVars + "¿" + strSignleKey;
                        }
                    }
                    else
                    {
                        string replKey = string.Empty;

                        try
                        {
                            FDW fdwObjNew = new FDW();
                            fdwObjNew.HashDeletekey(flKey, matchflGlobalVar);
                        }
                        catch (Exception exd) { }
                        string existingKey = "";
                        if (matchflGlobalVar.LastIndexOf('♦') > -1)
                            existingKey = matchflGlobalVar.Substring(0, matchflGlobalVar.LastIndexOf('♦'));
                        else
                            existingKey = matchflGlobalVar;
                        if (devOption != string.Empty)
                            replKey = existingKey + "♦" + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        else
                            replKey = existingKey + "♦" + DateTime.Now.ToString("dd/MM/yyyy");

                        strSignleKey = replKey;
                        strflKeys = flGlobalVars.Replace(matchflGlobalVar, replKey);
                    }
                }
                flgvValue = strflKeys;
                FDW fdwObj = new FDW();
                fdwObj.HashSetKey(flKey, strSignleKey, loadRes);
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("SetTstHtmlLs -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in SetTstHtmlLs", "new");
            }
            return flgvValue;
        }

        public string ReplaceFirstOccurrence(string source, string find, string replace)
        {
            int place = source.IndexOf(find);
            if (place == -1)
            {
                return source;
            }
            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public void GetAxExportConfig()
        {
            string jsoncontents = string.Empty;
            try
            {
                jsoncontents = ReadFromFile(HttpContext.Current.Server.MapPath("../ExportConfig.ini"));
                if (jsoncontents != string.Empty)
                {
                    JObject _jsonWord = JObject.Parse(jsoncontents);
                    if (_jsonWord["Word"] != null)
                    {
                        try
                        {
                            HttpContext.Current.Session["AxWordConfigs"] = "true";
                            if (_jsonWord["Word"]["margin"] != null)
                                HttpContext.Current.Session["AxWordMargin"] = _jsonWord["Word"]["margin"].ToString();
                            if (_jsonWord["Word"]["orientation"] != null)
                                HttpContext.Current.Session["AxWordOrientation"] = _jsonWord["Word"]["orientation"].ToString();
                        }
                        catch (Exception ex)
                        {
                            LogFile.Log logobj = new LogFile.Log();
                            logobj.CreateLog("Exception in GetAxExportConfig while reading from file - util.cs- jsoncontents:" + jsoncontents + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxExportConfig-ex", "new");
                        }
                    }
                    else
                    {
                        HttpContext.Current.Session["AxWordConfigs"] = "false";
                    }

                    if (_jsonWord["PDF"] != null)
                    {
                        try
                        {
                            HttpContext.Current.Session["AxPDFConfigs"] = "true";
                            if (_jsonWord["PDF"]["margin"] != null)
                                HttpContext.Current.Session["AxPDFMargin"] = _jsonWord["PDF"]["margin"].ToString();
                            if (_jsonWord["PDF"]["orientation"] != null)
                                HttpContext.Current.Session["AxPDFOrientation"] = _jsonWord["PDF"]["orientation"].ToString();
                        }
                        catch (Exception ex)
                        {
                            LogFile.Log logobj = new LogFile.Log();
                            logobj.CreateLog("Exception in GetAxExportConfig while reading from file - util.cs- jsoncontents:" + jsoncontents + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxExportConfig-ex", "new");
                        }
                    }
                    else
                    {
                        HttpContext.Current.Session["AxPDFConfigs"] = "false";
                    }
                }
                else
                {
                    HttpContext.Current.Session["AxWordConfigs"] = "false";
                    HttpContext.Current.Session["AxPDFConfigs"] = "false";
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Exception in GetAxExportConfig - util.cs-" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetAxExportConfig", "new");
            }
        }

        public void DeleteCachedDataKeys(string keyType, string keyInfo = "")
        {
            try
            {
                FDW fdwObj = new FDW();
                if (HttpContext.Current.Session["dbuser"] != null)
                    fdwObj.Initialize(HttpContext.Current.Session["dbuser"].ToString());
                string _schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    _schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                {
                    fObj = new FDR();
                    HttpContext.Current.Session["FDR"] = fObj;
                }

                if (keyType == "tstruct")
                {
                    string Transid = string.Empty;
                    if (keyInfo != string.Empty && keyInfo.StartsWith("<"))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(keyInfo);
                        XmlNode tNode = xmlDoc.SelectSingleNode("//params");
                        Transid = tNode["tstname"].InnerText;
                    }
                    else if (keyInfo != string.Empty)
                        Transid = keyInfo;

                    string tKey = _schemaName + "-" + Transid + "-*";
                    ArrayList tstCachedKeys = fObj.GetAllKeys(tKey);
                    for (int i = 0; i < tstCachedKeys.Count; i++)
                    {
                        string keyName = tstCachedKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, _schemaName);
                    }
                }
                else if (keyType == "iview")
                {
                    string IviewName = string.Empty;
                    if (keyInfo != string.Empty && keyInfo.StartsWith("<"))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(keyInfo);
                        XmlNode tNode = xmlDoc.SelectSingleNode("//varlist/row");
                        IviewName = tNode["iname"].InnerText;
                    }
                    else if (keyInfo != string.Empty)
                        IviewName = keyInfo;

                    string ivKey = _schemaName + "-" + IviewName + "-*";
                    ArrayList ivCachedKeys = fObj.GetAllKeys(ivKey);
                    for (int i = 0; i < ivCachedKeys.Count; i++)
                    {
                        string keyName = ivCachedKeys[i].ToString();
                        fdwObj.DeleteAllKeys(keyName, _schemaName);
                    }
                }
                else
                {
                    // For other keys
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Exception in DeleteCachedDataKeys - util.cs -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "DeleteCachedDataKeys", "new");
            }
        }

        public string VirtualFilePathUpdate()
        {
            string virtualdownloadpath = string.Empty;
            try
            {
                if (HttpContext.Current.Session["AxConfigFileUploadPath"] != null)
                    virtualdownloadpath += "<VirtualUploadFilePath>" + HttpContext.Current.Session["AxConfigFileUploadPath"].ToString() + "</VirtualUploadFilePath>";
                if (HttpContext.Current.Session["AxConfigFileDownloadPath"] != null)
                    virtualdownloadpath += "<VirtualDownloadFilePath>" + HttpContext.Current.Session["AxConfigFileDownloadPath"].ToString() + "</VirtualDownloadFilePath>";
                if (HttpContext.Current.Session["AxConfigFileMapUser"] != null)
                    virtualdownloadpath += "<VirtualFilePathMapUser>" + HttpContext.Current.Session["AxConfigFileMapUser"].ToString() + "</VirtualFilePathMapUser>";
                if (HttpContext.Current.Session["AxConfigFileMapPwd"] != null)
                    virtualdownloadpath += "<VirtualFilePathMapPwd>" + HttpContext.Current.Session["AxConfigFileMapPwd"].ToString() + "</VirtualFilePathMapPwd>";
            }
            catch (Exception ex) { }
            return virtualdownloadpath;
        }

        public string GetAxEmailSettings(string _proj)
        {
            string result = string.Empty;
            FileInfo _emailfi = new FileInfo(ScriptsPath + "\\AppSettings.ini");
            if (_emailfi.Exists)
            {
                string scriptsPathAxEmail = ScriptsPath + "\\AppSettings.ini";
                string filePatharm = @" " + scriptsPathAxEmail + "";
                string existingJsonAxEmail = File.ReadAllText(filePatharm);
                JObject jsonAxEmail = JObject.Parse(existingJsonAxEmail);
                if (jsonAxEmail["appsettings"] != null && jsonAxEmail["appsettings"][_proj] != null && jsonAxEmail["appsettings"][_proj]["EmailSettings"] != null)
                {
                    string jsoncontents = jsonAxEmail["appsettings"][_proj]["EmailSettings"].ToString();
                    result = jsoncontents;
                    FDW fdwObj = new FDW(_proj);
                    fdwObj.SaveInRedisServer(Constants.AXEMAILSMTP_CONN_KEY, jsoncontents, Constants.AXEMAILSMTP_CONN_KEY, _proj);
                }
                else
                {
                    FDW fdwObj = new FDW(_proj);
                    fdwObj.SaveInRedisServer(Constants.AXEMAILSMTP_CONN_KEY, "noemailsettings", Constants.AXEMAILSMTP_CONN_KEY, _proj);
                    result = string.Empty;
                }
            }
            else
            {
                FDW fdwObj = new FDW(_proj);
                fdwObj.SaveInRedisServer(Constants.AXEMAILSMTP_CONN_KEY, "noemailsettings", Constants.AXEMAILSMTP_CONN_KEY, _proj);
                result = string.Empty;
            }
            return result;
        }

        public string GetOTPSettings(string proj, string attrName)
        {
            string strOtp = string.Empty;
            try
            {
                string AxOTPSettings = string.Empty;
                if (HttpContext.Current.Session["AxOTPSettings-" + proj] == null || HttpContext.Current.Session["AxOTPSettings-" + proj].ToString() == string.Empty)
                {
                    AxOTPSettings = GetAxEmailSettings(proj);
                    HttpContext.Current.Session["AxOTPSettings-" + proj] = AxOTPSettings;
                }
                else
                {
                    AxOTPSettings = HttpContext.Current.Session["AxOTPSettings-" + proj].ToString();
                }
                if (AxOTPSettings != string.Empty && AxOTPSettings != "noemailsettings")
                {
                    JArray configNode = null;
                    JObject objConfig = JObject.Parse(AxOTPSettings);
                    if (objConfig[attrName] != null)
                        strOtp = objConfig[attrName].ToString();

                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetOTPSettings function -" + ex.Message, "GetOTPSettings", "GetOTPSettings", "new", "true");
                return strOtp;
            }
            return strOtp;
        }

        public bool CheckValidLogin()
        {
            try
            {
                string _nsessId = HttpContext.Current.Session["nsessionid"].ToString();
                string _projName = HttpContext.Current.Session["project"].ToString();
                string _username = HttpContext.Current.Session["username"].ToString();
                string isValidLogin = GetDupUserInfo(_projName, _username);
                if (isValidLogin != null && isValidLogin != "" && isValidLogin.IndexOf(_username + "♦" + _nsessId + "♣") == -1)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetAxAppSettings()
        {
            string jsRes = string.Empty;
            try
            {
                string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                string jsonPath = Path.Combine(scriptsPath, "AppSettings.ini");
                FileInfo fi = new FileInfo(jsonPath);
                if (fi.Exists)
                {
                    jsRes = File.ReadAllText(jsonPath);
                }
            }
            catch (Exception ex)
            { }
            return jsRes;
        }

        public string GetRedisConnDetailsStudio()
        {
            string rcDetails = string.Empty;
            try
            {
                string projName = string.Empty;
                if (HttpContext.Current.Session["project"] != null)
                    projName = HttpContext.Current.Session["project"].ToString();

                if (projName == string.Empty)
                    return rcDetails;

                if (HttpContext.Current.Application["AppSettingsIni"] != null)
                {
                    JObject redisObj = JObject.Parse(HttpContext.Current.Application["AppSettingsIni"].ToString());
                    if (redisObj["appsettings"] != null && redisObj["appsettings"][projName] != null && redisObj["appsettings"][projName]["AxStudioRedis"] != null)
                    {
                        string redisIp = string.Empty;
                        string redisPort = string.Empty;
                        string redisPass = string.Empty;
                        string studiourl = string.Empty;

                        JObject connDetails = redisObj["appsettings"][projName]["AxStudioRedis"] as JObject;
                        if (connDetails != null)
                        {
                            redisIp = connDetails.Value<string>("host") ?? string.Empty;
                            if (redisIp != "" && redisIp == "localhost")
                                return "localhost";
                            redisPort = connDetails.Value<string>("port") ?? string.Empty;
                            redisPass = connDetails.Value<string>("pwd") ?? string.Empty;
                            studiourl = connDetails.Value<string>("studiourl") ?? string.Empty;

                            if (!string.IsNullOrEmpty(redisPass))
                            {
                                redisPass = DecryptPWD(redisPass);
                            }
                        }

                        if (!string.IsNullOrEmpty(redisIp))
                        {
                            redisIp += ":" + redisPort;
                            rcDetails = redisIp + "♣" + redisPass;

                            if (HttpContext.Current.Session != null && HttpContext.Current.Session["project"] != null)
                            {
                                HttpContext.Current.Session["axDevStudioRedisIP"] = redisIp;
                                HttpContext.Current.Session["axDevStudioRedisPass"] = redisPass;
                                HttpContext.Current.Session["axpertstudioReact"] = studiourl;
                            }
                        }
                    }
                    else
                    {
                        if (HttpContext.Current.Session != null && HttpContext.Current.Session["project"] != null)
                        {
                            HttpContext.Current.Session.Remove("axDevStudioRedisIP");
                            HttpContext.Current.Session.Remove("axDevStudioRedisPass");
                            HttpContext.Current.Session.Remove("axpertstudioReact");
                        }
                    }
                }
                else
                {
                    string jsonPath = Path.Combine(ScriptsPath, "AppSettings.ini");
                    FileInfo fi = new FileInfo(jsonPath);
                    if (fi.Exists)
                    {
                        string jsonText = File.ReadAllText(jsonPath);
                        HttpContext.Current.Application["AppSettingsIni"] = jsonText;
                        JObject redisObj = JObject.Parse(jsonText);
                        if (redisObj["appsettings"] != null && redisObj["appsettings"][projName] != null && redisObj["appsettings"][projName]["AxStudioRedis"] != null)
                        {
                            string redisIp = string.Empty;
                            string redisPort = string.Empty;
                            string redisPass = string.Empty;
                            string studiourl = string.Empty;

                            JObject connDetails = redisObj["appsettings"][projName]["AxStudioRedis"] as JObject;
                            if (connDetails != null)
                            {
                                redisIp = connDetails.Value<string>("host") ?? string.Empty;
                                if (redisIp != "" && redisIp == "localhost")
                                    return "localhost";
                                redisPort = connDetails.Value<string>("port") ?? string.Empty;
                                redisPass = connDetails.Value<string>("pwd") ?? string.Empty;
                                studiourl = connDetails.Value<string>("studiourl") ?? string.Empty;

                                if (!string.IsNullOrEmpty(redisPass))
                                {
                                    redisPass = DecryptPWD(redisPass);
                                }
                            }

                            if (!string.IsNullOrEmpty(redisIp))
                            {
                                redisIp += ":" + redisPort;
                                rcDetails = redisIp + "♣" + redisPass;

                                if (HttpContext.Current.Session != null)
                                {
                                    HttpContext.Current.Session["axDevStudioRedisIP"] = redisIp;
                                    HttpContext.Current.Session["axDevStudioRedisPass"] = redisPass;
                                    HttpContext.Current.Session["axpertstudioReact"] = studiourl;
                                }
                            }
                        }
                        else
                        {
                            if (HttpContext.Current.Session != null)
                            {
                                HttpContext.Current.Session.Remove("axDevStudioRedisIP");
                                HttpContext.Current.Session.Remove("axDevStudioRedisPass");
                                HttpContext.Current.Session.Remove("axpertstudioReact");
                            }
                        }
                    }
                    else
                    {
                        if (HttpContext.Current.Session != null)
                        {
                            HttpContext.Current.Session.Remove("axDevStudioRedisIP");
                            HttpContext.Current.Session.Remove("axDevStudioRedisPass");
                            HttpContext.Current.Session.Remove("axpertstudioReact");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rcDetails = string.Empty;
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetRedisConnDetailsStudio  - " + ex.Message, HttpContext.Current.Session != null ? HttpContext.Current.Session.SessionID : "", "GetRedisConnDetailsStudio", "new");
            }
            return rcDetails;
        }

        public string GetGblVarsFromIni()
        {
            string gblIniVars = string.Empty;
            try
            {
                if (HttpContext.Current.Session["RMQueueAPIURL"] != null)
                    gblIniVars += "<AxRMQAPIURL>" + HttpContext.Current.Session["RMQueueAPIURL"].ToString() + "</AxRMQAPIURL>";
                if (HttpContext.Current.Session["SignalRAPIURL"] != null)
                    gblIniVars += "<AxSignalRapiURL>" + HttpContext.Current.Session["SignalRAPIURL"].ToString() + "</AxSignalRapiURL>";
                if (HttpContext.Current.Session["FCMSendMsgURL"] != null)
                    gblIniVars += "<AxFCMSendMsgURL>" + HttpContext.Current.Session["FCMSendMsgURL"].ToString() + "</AxFCMSendMsgURL>";
                if (HttpContext.Current.Session["RapidSaveURL"] != null)
                    gblIniVars += "<AxRapidSaveURL>" + HttpContext.Current.Session["RapidSaveURL"].ToString() + "</AxRapidSaveURL>";
                if (HttpContext.Current.Session["pegemailactionurl"] != null)
                    gblIniVars += "<axpegemailactionurl>" + HttpContext.Current.Session["pegemailactionurl"].ToString() + "</axpegemailactionurl>";
                if (HttpContext.Current.Session["ScriptsAPIURL"] != null)
                    gblIniVars += "<AxScriptsAPIURL>" + HttpContext.Current.Session["ScriptsAPIURL"].ToString() + "</AxScriptsAPIURL>";
            }
            catch (Exception ex) { }
            return gblIniVars;
        }
    }

    /// <summary>
    /// The IComparer class used to declare the type comparision for the sort in array.
    /// </summary>
    public class CustomComparer : IComparer
    {
        Comparer _comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);

        int IComparer.Compare(object x, object y)
        {
            // Convert string comparisons to int
            return _comparer.Compare(Convert.ToInt32(x), Convert.ToInt32(y));
        }
    }



    public class AgcSession
    {
        public string result { get; set; }
        public string code { get; set; }
        public string valid_session { get; set; }
    }
    public class GenResult
    {
        public string result { get; set; }
        public string code { get; set; }
    }



}
