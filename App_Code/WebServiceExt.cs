using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Web.Configuration;
using System.Data;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ASBExt
{
    /// <summary>
    /// Summary description for WebServiceExt
    /// </summary>
    [Serializable]
    public class WebServiceExt
    {
        public ASBMenu.ASBMenuservice asbMenu = new ASBMenu.ASBMenuservice();
        public ASBAction.ASBActionservice asbAction = new ASBAction.ASBActionservice();
        public ASBTStruct.ASBTStructservice asbTStruct = new ASBTStruct.ASBTStructservice();
        public ASBRapidTStruct.ASBRapidTStructservice asbRTstruct = new ASBRapidTStruct.ASBRapidTStructservice();
        public ASBIView.ASBIViewservice asbIview = new ASBIView.ASBIViewservice();
        public ASBUser.ASBUserservice asbUser = new ASBUser.ASBUserservice();
        public ASBDefine.ASBDefineservice asbDefine = new ASBDefine.ASBDefineservice();
        public ASBUtils.ASBUtilsservice asbUtils = new ASBUtils.ASBUtilsservice();
        public ASBScript.ASBScriptservice asbScript = new ASBScript.ASBScriptservice();

        LogFile.Log logobj = new LogFile.Log();
        Util.Util utilObj = new Util.Util();
        ExecTrace ObjExecTr = ExecTrace.Instance;
        bool attachDir = false;

        /// <summary>Based on the input param get recordid and send back the result </summary>
        /// <param name="paramXml"> </param>
        /// return the record id in the xml format
        public string CallGetParamChoicesWS(string transId, string paramXml, string structureXML = "")
        {
            string result = string.Empty;

            try
            {
                result = asbIview.GetParamChoices(paramXml, structureXML);
                result = GetAppSessionKey("CallGetParamChoicesWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetParamChoices", ex.Message.ToString(), "GetParamChoices-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the input param get recordid and send back the result </summary>
        /// <param name="paramXml"> </param>
        /// return the record id in the xml format
        public string CallGetRecordIdWS(string transId, string paramXml)
        {
            string result = string.Empty;

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbTStruct.GetRecordId(paramXml);
                result = GetAppSessionKey("CallGetRecordIdWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "GetRecordId", paramXml, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("GetRecordId", ex.Message.ToString(), "GetRecordId-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the transid and record id it will give the comments </summary>
        /// <param name="comments"> </param>
        /// return the comments details in the xml format
        public string CallViewCommentsWS(string transId, string comments)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.ViewComments(comments);
                result = GetAppSessionKey("CallViewCommentsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("ViewComments", ex.Message.ToString(), "ViewComments-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the transid and record id it will give the comments </summary>
        /// <param name="comments"> </param>
        /// return the comments details in the xml format
        public string CallGetChoiceWebService(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbMenu.GetChoices(inputXML);
                result = GetAppSessionKey("CallGetChoiceWebService", result);
                if (result.Contains(Constants.SESSIONEXPMSG))
                {
                    HttpContext.Current.Response.Redirect(utilObj.ERRPATH + Constants.SESSIONEXPMSG);
                }
                else if (result.Contains(Constants.ERROR))
                {
                    HttpContext.Current.Response.Redirect(utilObj.ERRPATH + Constants.ERAUTHENTICATION);
                }
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("ViewComments", ex.Message.ToString(), "ViewComments-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the input query it will give the result</summary>
        /// <param name="inputQuery"> </param>
        /// return the result in the xml format
        public string CallGetChoiceWS(string transId, string inputXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(inputXML);
            XmlNode sqlNode = doc.SelectSingleNode("//sql");
            string inputQuery = sqlNode.InnerText;
            string result = string.Empty;

            try
            {
                if (ConfigurationManager.AppSettings["isCloudApp"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["isCloudApp"]))
                {
                    DBContext objDB = new DBContext();
                    if (inputQuery == string.Empty || inputQuery == "")
                        return "<sqlresultset><response/></sqlresultset>";
                    DataSet ds = new DataSet();

                    logobj.CreateLog("Calling Direct DB - GetChoice : " + inputQuery, HttpContext.Current.Session["nsessionid"].ToString(), "DirectDB-GetChoice", string.Empty);
                    ds = objDB.GetChoices(inputQuery, HttpContext.Current.Session["nsessionid"].ToString());
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString() != string.Empty)
                            result = ds.Tables[0].Rows[0][0].ToString();
                        else
                            result = "<sqlresultset><response/></sqlresultset>";
                    }
                    else
                        result = "<sqlresultset><response value=\"--\"/></sqlresultset>";

                }
                else
                {
                    //logobj.CreateLog("Calling Service - GetChoice : " + inputQuery, HttpContext.Current.Session["nsessionid"].ToString(), "Service-GetChoice", string.Empty);
                    result = asbMenu.GetChoices(inputXML);
                    result = GetAppSessionKey("CallGetChoiceWS", result);
                }
                if (result.Contains(Constants.SESSIONEXPMSG))
                {
                    HttpContext.Current.Response.Redirect(utilObj.ERRPATH + Constants.SESSIONEXPMSG);
                }
                else if (result.Contains(Constants.ERAUTHENTICATION))// (result.Contains(Constants.ERROR))
                {
                    HttpContext.Current.Response.Redirect(utilObj.ERRPATH + Constants.ERAUTHENTICATION);
                }
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetChoices", ex.Message.ToString(), "GetChoices-" + transId + "");
                result = "<error>" + ex.Message + "</error>";
            }
            return result;
        }

        /// <summary>Based on the inputxml to get Params details for both iview and listview</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the xml format</returns>
        public string CallGetParamsWS(string transId, string inputXML, IviewData objIview)
        {
            string result = string.Empty;

            int tempWebServiceTimeout = asbIview.Timeout;
            asbIview.Timeout = objIview.WebServiceTimeout;

            int tempWebScriptTimeout = HttpContext.Current.Server.ScriptTimeout;
            HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(objIview.WebServiceTimeout / 1000);

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;

                if (!objIview.requestJSON)
                {
                    result = asbIview.GetParams(inputXML);
                }
                else
                {
                    string accessControlPlusStructure = string.Empty;
                    if (objIview.StructureXml != string.Empty)
                    {
                        accessControlPlusStructure = objIview.AccessControlXml + "#$♥#" + objIview.StructureXml;
                    }
                    result = asbIview.GetParamsNew(inputXML, accessControlPlusStructure);
                }
                result = GetAppSessionKey("CallGetParamsWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "GetParams", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("GetParams", ex.Message.ToString(), "GetParams-" + transId + "");
            }

            asbIview.Timeout = tempWebServiceTimeout;

            HttpContext.Current.Server.ScriptTimeout = tempWebScriptTimeout;

            return result;
        }

        /// <summary>Based on the inputxml to get listview details</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the xml format</returns>
        public string CallGetLViewWS(string transId, string inputXML, int webServiceTimeout)
        {
            string result = string.Empty;

            int tempWebServiceTimeout = asbIview.Timeout;
            asbIview.Timeout = webServiceTimeout;

            int tempWebScriptTimeout = HttpContext.Current.Server.ScriptTimeout;
            HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(webServiceTimeout / 1000);

            try
            {
                result = asbIview.GetLView(inputXML);
                result = GetAppSessionKey("CallGetLViewWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetLView", ex.Message.ToString(), "GetLView-" + transId + "");
            }

            asbIview.Timeout = tempWebServiceTimeout;

            HttpContext.Current.Server.ScriptTimeout = tempWebScriptTimeout;

            return result;
        }

        /// <summary>Based on the inputxml to get iview details</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the xml format</returns>
        public string CallGetIViewWS(string transId, string inputXML, string iSTructure, IviewData objIview)
        {
            string result = string.Empty;

            int tempWebServiceTimeout = asbIview.Timeout;

            string dataId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "");

            string redisIP = string.Empty;

            string redisPort = string.Empty;

            string redisPass = string.Empty;


            if (HttpContext.Current.Session["RedisCacheIP"] != null && HttpContext.Current.Session["RedisCacheIP"].ToString() != "")
            {
                string[] redisIpPort = HttpContext.Current.Session["RedisCacheIP"].ToString().Split(':');
                if (redisIpPort.Length > 0)
                {
                    redisIP = redisIpPort[0];
                }
                if (redisIpPort.Length > 1)
                {
                    redisPort = redisIpPort[1];
                }
            }

            if (HttpContext.Current.Session["RedisCachePwd"] != null && HttpContext.Current.Session["RedisCachePwd"].ToString() != "")
            {
                redisPass = HttpContext.Current.Session["RedisCachePwd"].ToString();
            }

            //WebConfigurationManager.AppSettings["redisPass"];
            if (objIview.WebServiceTimeout != 0)
            {
                // objIview.WebServiceTimeout = 1;
                // var webServiceTimeoutsecs = 1;
                var webServiceTimeoutsecs = objIview.WebServiceTimeout / 1000;
                if (objIview.notifyTimeout != string.Empty && inputXML != string.Empty)
                {
                    // inputXML = inputXML.Replace("<root", "<root timeout='" + webServiceTimeoutsecs + "'");
                    inputXML = inputXML.Replace("<root", "<root timeout='" + webServiceTimeoutsecs + "'");
                    inputXML = inputXML.Replace("<root", "<root redisserver='" + redisIP + "'");
                    inputXML = inputXML.Replace("<root", "<root redisportno='" + redisPort + "'");
                    inputXML = inputXML.Replace("<root", "<root redispwd='" + redisPass + "'");
                    inputXML = inputXML.Replace("<root", "<root loadtype='" + dataId + "'");
                    inputXML = inputXML.Replace("<root", "<root loaddata='" + transId + "'");
                }
            }


            asbIview.Timeout = objIview.WebServiceTimeout;

            int tempWebScriptTimeout = HttpContext.Current.Server.ScriptTimeout;

            // HttpContext.Current.Server.ScriptTimeout = 1;
            HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(objIview.WebServiceTimeout / 1000);

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;

                if (!objIview.requestJSON)
                {
                    result = asbIview.GetIView(inputXML, iSTructure);
                }
                else
                {
                    string accessControlPlusStructure = string.Empty;
                    if (iSTructure != string.Empty)
                    {
                        accessControlPlusStructure = objIview.AccessControlXml + "#$♥#" + iSTructure;
                    }
                    result = asbIview.GetIViewNew(inputXML, accessControlPlusStructure);
                }

                result = GetAppSessionKey("CallGetIViewWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "GetIView", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().IndexOf("operation has timed out") != -1)
                {
                    JObject response = new JObject();
                    response["message"] = "webservice timeout";
                    response["dataId"] = dataId;
                    result = response.ToString();
                }
                else
                {
                    result = "<error>" + ex.Message.ToString();
                    CallExceptionErrorPage("GetIView", ex.Message.ToString(), "GetIView-" + transId + "");
                }
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
            }

            asbIview.Timeout = tempWebServiceTimeout;

            HttpContext.Current.Server.ScriptTimeout = tempWebScriptTimeout;

            return result;
        }

        /// <summary>Based on the inputxml to get iview RecordCount</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the xml format</returns>
        public string CallGetRecordCount(string iName, string inputXML, string iSTructure)
        {
            string result = string.Empty;

            try
            {
                result = asbIview.GetIView(inputXML, iSTructure);
                result = GetAppSessionKey("CallGetIViewWS", result);
            }
            catch (Exception ex)
            {
                result = "<error>" + ex.Message.ToString();
                CallExceptionErrorPage("GetIViewRecordCount", ex.Message.ToString(), "GetIViewRecordCount-" + iName + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to send the mail</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the string format (ex :for success-'done' )</returns>
        public string CallAxpSendMailWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbAction.AxpSendMail(inputXML);
                result = GetAppSessionKey("CallAxpSendMailWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("AxpSendMail", ex.Message.ToString(), "AxpSendMail-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the TODO list</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the XML format</returns>
        public string CallGetDocListWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbAction.GetDocList(inputXML);
                result = GetAppSessionKey("CallGetDocListWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetDocList", ex.Message.ToString(), "GetDocList-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to export the details in the EXCEL foramt</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the XML format</returns>
        public string CallSaveAsExcelWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbIview.SaveAsExcel(inputXML);
                result = GetAppSessionKey("CallSaveAsExcelWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("SaveAsExcel", ex.Message.ToString(), "SaveAsExcel-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the action details</summary>
        /// <param1 name="inputXML"> </param1><param2>current transaction structure details</param2>
        /// <returns >return the result in the JSON format</returns>
        public string CallRemoteDoActionWS(string transId, string inputXML, string ires, int webServiceTimeout)
        {
            string result = string.Empty;

            string redisIP = string.Empty;

            string redisPort = string.Empty;

            string redisPass = string.Empty;


            if (HttpContext.Current.Session["RedisCacheIP"] != null && HttpContext.Current.Session["RedisCacheIP"].ToString() != "")
            {
                string[] redisIpPort = HttpContext.Current.Session["RedisCacheIP"].ToString().Split(':');
                if (redisIpPort.Length > 0)
                {
                    redisIP = redisIpPort[0];
                }
                if (redisIpPort.Length > 1)
                {
                    redisPort = redisIpPort[1];
                }
            }

            if (HttpContext.Current.Session["RedisCachePwd"] != null && HttpContext.Current.Session["RedisCachePwd"].ToString() != "")
            {
                redisPass = HttpContext.Current.Session["RedisCachePwd"].ToString();
            }

            if (webServiceTimeout != 0)
            {
                string notifyTimeout = string.Empty;

                try
                {
                    notifyTimeout = utilObj.GetAdvConfigs("notification time interval");
                }
                catch (Exception ex)
                { }

                //webServiceTimeout = 10;
                var webServiceTimeoutsecs = webServiceTimeout / 1000;

                if (notifyTimeout != string.Empty && inputXML != string.Empty)
                {
                    inputXML = inputXML.Replace("<root", "<root timeout='" + webServiceTimeoutsecs + "'");
                    inputXML = inputXML.Replace("<root", "<root redispwd='" + redisPass + "'");
                    inputXML = inputXML.Replace("<root", "<root redisserver ='" + redisIP + "'");
                    inputXML = inputXML.Replace("<root", "<root redisportno ='" + redisPort + "'");
                }

            }
            int tempWebServiceTimeout = asbAction.Timeout;
            asbAction.Timeout = webServiceTimeout;

            int tempWebScriptTimeout = HttpContext.Current.Server.ScriptTimeout;
            HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(webServiceTimeout / 1000);

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbAction.RemoteDoAction(inputXML, ires);

                //strRequest += "Service Result:" + result + " ♦ ";

                result = GetAppSessionKey("CallRemoteDoActionWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "DoAction", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().IndexOf("operation has timed out") != -1)
                {
                    //result = "This proces taking time is more than expected. You will get a notification once completed";
                    //result = "{\"result\":[{\"action\": \"retry\"}]}*$*{\"error\":[{\"msg\":\"This proces taking time is more than expected. You will get a notification once completed\"}]}";
                    result = "{\"error\":[{\"msg\":\"This proces taking time is more than expected. You will get a notification once completed\"}]}";
                }
                else
                    CallExceptionErrorPage("RemoteDoAction", ex.Message.ToString(), "RemoteDoAction-" + transId + "");
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
            }

            asbAction.Timeout = tempWebServiceTimeout;

            HttpContext.Current.Server.ScriptTimeout = tempWebScriptTimeout;

            return result;
        }

        /// <summary>Based on the inputxml to get the menu details</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the XML format</returns>
        public string CallGetNewGlobalVarsWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbMenu.GetNewGlobalVars(inputXML);
                result = GetAppSessionKey("CallGetNewGlobalVarsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetNewGlobalVars", ex.Message.ToString(), "GetNewGlobalVars-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the menu details</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the XML format</returns>
        public string CallGetMultiLevelMenuWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbMenu.GetMultiLevelMenu(inputXML);
                result = GetAppSessionKey("CallGetMultiLevelMenuWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "Menu", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("GetMultiLevelMenu", ex.Message.ToString(), "GetMultiLevelMenu-" + transId + "");
            }
            return result;
        }


        public string CallContinueCurrentSessionWS(string inputXML)
        {
            string result = string.Empty;
            try
            {
                result = asbMenu.ContinueCurrentSession(inputXML);
                result = GetAppSessionKey("CallContinueCurrentSessionWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("ContinueCurrentSessionWS", ex.Message.ToString(), "CallContinueCurrentSessionWS");
            }
            return result;
        }

        public string GetIndividualCardDetails(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbMenu.GetIndividualCardDetails(inputXML);
                result = GetAppSessionKey("GetIndividualCardDetails", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetIndividualCardDetails", ex.Message.ToString(), "GetIndividualCardDetails-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the login user details</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the string format</returns>
        public string CallLoginWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbMenu.Login(inputXML);
                result = GetAppSessionKey("CallLoginWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "Login", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                if (utilObj.sysErrorlog)
                {
                    logobj.CreateLog("Exception in Login Service :--- " + ex.Message.ToString(), HttpContext.Current.Session["nsessionid"].ToString(), "Login", "");
                }
                throw ex;
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the pdf file list</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the xml format</returns>
        public string CallGetPDFListWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                // result = asbAction.GetPDFList(inputXML);
                result = GetAppSessionKey("CallGetPDFListWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetPDFList", ex.Message.ToString(), "GetPDFList-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputXML to get the print form path details</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the string format</returns>
        public string CallCreatePrintFormWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbAction.CreatePrintForm(inputXML);
                result = GetAppSessionKey("CallCreatePrintFormWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CreatePrintForm", ex.Message.ToString(), "CreatePrintForm-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the details of Responsibility list.</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the XML format</returns>
        public string CallGetResponsibilityListWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.GetResponsibilityList(inputXML);
                result = GetAppSessionKey("CallGetResponsibilityListWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetResponsibilityList", ex.Message.ToString(), "GetResponsibilityList-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the details of Access Rights Details list for responsbility screen.</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the XML format</returns>
        public string CallGetAccessRightsDetailsWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.GetAccessRightsDetails(inputXML);
                result = GetAppSessionKey("CallGetAccessRightsDetailsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetAccessRightsDetails", ex.Message.ToString(), "GetAccessRightsDetails-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the details of Process Role list for responsbility screen.</summary>
        /// <param name="inputXML"> </param>
        /// <returns >return the result in the string format (Ex:for success-'success') </returns>
        public string CallProcessRoleWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.ProcessRole(inputXML);
                result = GetAppSessionKey("CallProcessRoleWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("ProcessRole", ex.Message.ToString(), "ProcessRole-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to save/delete the Process Group list for Role screen.</summary>
        /// <param name="inputXML"> </param>
        /// <returns >Service return the result in the string format (Ex:for success-'success') </returns>
        public string CallProcessGroupWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.ProcessGroup(inputXML);
                result = GetAppSessionKey("CallProcessGroupWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("ProcessGroup", ex.Message.ToString(), "ProcessGroup-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the Roles List for Role screen.</summary>
        /// <param name="inputXML"> </param>
        /// <returns >Service return the result in the xml format </returns>
        public string CallGetRolesListWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.GetRolesList(inputXML);
                result = GetAppSessionKey("CallGetRolesListWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetRolesList", ex.Message.ToString(), "GetRolesList");
            }
            return result;
        }

        /// <summary>Based on the inputxml to get the Roles List for Role screen.</summary>
        /// <param name="inputXML"> </param>
        /// <returns >Service return the result in the xml format </returns>
        public string CallGetUserGroupDetailsWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.GetUserGroupDetails(inputXML);
                result = GetAppSessionKey("CallGetUserGroupDetailsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetUserGroupDetails", ex.Message.ToString(), "GetUserGroupDetails-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to Logout from the current session(calling from sess/signout page)</summary>
        /// <param name="inputXML"> </param>
        /// <returns ></returns>
        public string CallLogoutWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbMenu.Logout(inputXML);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Logout", ex.Message.ToString(), "Logout-" + transId + "");
            }
            utilObj.KillSession();
            return result;
        }

        /// <summary>Based on the inputxml to Logout from the current session(calling from sess/signout page). this is for dont kill session</summary>
        /// <param name="inputXML"> </param>
        /// <returns ></returns>
        public string CallLogoutNewWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbMenu.Logout(inputXML);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Logout", ex.Message.ToString(), "Logout-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to Get Field Choices details</summary>
        /// <param1 name="inputXML"> </param1><param2>current transaction structure details</param2>
        /// <returns >return the result in the xml format</returns>
        public string CallGetFieldChoicesWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.GetFieldChoices(inputXML, structure);
                result = GetAppSessionKey("CallGetFieldChoicesWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetFieldChoices", ex.Message.ToString(), "GetFieldChoices-" + transId + "");
            }
            return result;
        }


        /// <summary>Based on the inputxml to Get existing record details</summary>
        /// <param1 name="inputXML"> </param1><param2>current transaction structure details</param2>
        /// <returns >return the result in the joson format</returns>
        public string CallLoadDataWS(string transId, string inputXML, string structure, string recId, string project)
        {
            string result = string.Empty;


            string isPerfCode = HttpContext.Current.Session["AxIsPerfCode"] != null ? HttpContext.Current.Session["AxIsPerfCode"].ToString() : "false";

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                if (isPerfCode == "true")
                    result = asbTStruct.FastLoadData(inputXML, structure);
                else
                    result = asbTStruct.LoadData(inputXML, structure);
                result = GetAppSessionKey("CallLoadDataWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "LoadData", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("LoadData", ex.Message.ToString(), "LoadData-" + transId + "");
            }
            return result;
        }

        public string CallLoadDataFromHtml(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.LoadDataFromHTML(inputXML, structure);
                result = GetAppSessionKey("CallLoadDataFromHtml", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("LoadDataFromHtml", ex.Message.ToString(), "LoadDataFromHtml-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to Get new record details</summary>
        /// <param1 name="inputXML"> </param1><param2>current transaction structure details</param2>
        /// <returns >return the result in the joson format</returns>
        public string CallDoFormLoadWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;
            string isPerfCode = HttpContext.Current.Session["AxIsPerfCode"] != null ? HttpContext.Current.Session["AxIsPerfCode"].ToString() : "false";
            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                if (isPerfCode == "true")
                    result = asbTStruct.FastDoFormLoad(inputXML, structure); // result = asbTStruct.FastFormLoad(inputXML, structure);//
                else
                    result = asbTStruct.DoFormLoad(inputXML, structure);
                result = GetAppSessionKey("CallDoFormLoadWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "FormLoad", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("DoFormLoad", ex.Message.ToString(), "DoFormLoad-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to Get new record details</summary>
        /// <param1 name="inputXML"> </param1><param2>current transaction structure details</param2>
        /// <returns >return the result in the joson format</returns>
        public string CallRapidDoFormLoadWS(string transId, string inputXML)
        {
            string result = string.Empty;
            try
            {
                result = asbRTstruct.RapidDoFormLoad(inputXML);
                result = GetAppSessionKey("CallRapidDoFormLoadWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("DoFormLoad", ex.Message.ToString(), "DoFormLoad-" + transId + "");
            }
            return result;
        }

        public string CallGetSearchValWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbTStruct.GetSearchVal(inputXML, structure);
                result = GetAppSessionKey("CallGetSearchValWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "GetSearchVal", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("GetSearchVal", ex.Message.ToString(), "GetSearchVal-" + transId + "");
            }
            return result;
        }

        public string CallGetUserAccessTstDetailsWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.GetUserAccessTstDetails(inputXML);
                result = GetAppSessionKey("CallGetUserAccessTstDetailsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetUserAccessTstDetails", ex.Message.ToString(), "GetUserAccessTstDetails-" + transId + "");
            }
            return result;
        }

        public string CallSaveAccessTstDetailsWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.SaveAccessTstDetails(inputXML);
                result = GetAppSessionKey("CallSaveAccessTstDetailsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("SaveAccessTstDetails", ex.Message.ToString(), "SaveAccessTstDetails-" + transId + "");
            }
            return result;
        }

        public string CallGetUserAccessIviewDetailsWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.GetUserAccessIviewDetails(inputXML);
                result = GetAppSessionKey("CallGetUserAccessIviewDetailsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetUserAccessIviewDetails", ex.Message.ToString(), "GetUserAccessIviewDetails-" + transId + "");
            }
            return result;
        }

        public string CallSaveAccessIviewDetailsWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.SaveAccessIviewDetails(inputXML);
                result = GetAppSessionKey("CallSaveAccessIviewDetailsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("SaveAccessIviewDetails", ex.Message.ToString(), "SaveAccessIviewDetails-" + transId + "");
            }
            return result;
        }

        public string CallProcessUserWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.ProcessUser(inputXML);
                result = GetAppSessionKey("CallProcessUserWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("ProcessUser", ex.Message.ToString(), "ProcessUser-" + transId + "");
            }
            return result;
        }

        public string CallGetUserDetailsWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.GetUserDetails(inputXML);
                result = GetAppSessionKey("CallGetUserDetailsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetUserDetails", ex.Message.ToString(), "GetUserDetails-" + transId + "");
            }
            return result;
        }

        public string CallGetUsersListWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.GetUsersList(inputXML);
                result = GetAppSessionKey("CallGetUsersListWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetUsersList", ex.Message.ToString(), "GetUsersList-" + transId + "");
            }
            return result;
        }

        public string CallGetHistoryDataWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.GetHistoryData(inputXML, structure);
                result = GetAppSessionKey("CallGetHistoryDataWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetHistoryData", ex.Message.ToString(), "GetHistoryData-" + transId + "");
            }
            return result;
        }

        public string CallLoadWorkFlowPageWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.LoadWorkFlowPage(inputXML);
                result = GetAppSessionKey("CallLoadWorkFlowPageWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("LoadWorkFlowPage", ex.Message.ToString(), "LoadWorkFlowPage-" + transId + "");
            }
            return result;
        }

        public string CallGetWorkflowsForTst(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.LoadWorkFlowCondition(inputXML);
                result = GetAppSessionKey("CallGetWorkflowsForTst", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("LoadWorkFlowCondition", ex.Message.ToString(), "LoadWorkFlowCondition-" + transId + "");
            }
            return result;
        }

        public string CallSaveAndAttachWorkflowWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.SaveAndAttachWorkflow(inputXML);
                result = GetAppSessionKey("CallSaveAndAttachWorkflowWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("SaveAndAttachWorkflow", ex.Message.ToString(), "SaveAndAttachWorkflow-" + transId + "");
            }
            return result;
        }

        public string CallLoadWFWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.LoadWF(inputXML);
                result = GetAppSessionKey("CallLoadWFWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("LoadWF", ex.Message.ToString(), "LoadWF-" + transId + "");
            }
            return result;
        }

        public string CallLoadWorkflow(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.LoadWorkFlow(inputXML);
                result = GetAppSessionKey("CallLoadWorkflow", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("LoadWF", ex.Message.ToString(), "LoadWF-" + transId + "");
            }
            return result;
        }

        public string CallDeleteWFCondition(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.RemoveWorkFlowCondition(inputXML);
                result = GetAppSessionKey("CallDeleteWFCondition", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("RemoveWFCond", ex.Message.ToString(), "RemovwWFCond-" + transId + "");
            }
            return result;
        }

        public string CallAttachWorkflow(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.AttachWorkFlow(inputXML);
                result = GetAppSessionKey("CallAttachWorkflow", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("AttachWF", ex.Message.ToString(), "AttachWF-" + transId + "");
            }
            return result;
        }

        public string CallGetModulewiseTstruct(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.GetModuleWiseTStruct(inputXML);
                result = GetAppSessionKey("CallGetModulewiseTstruct", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetModuleWiseTstruct", ex.Message.ToString(), "GetModuleWiseTstruct-" + transId + "");
            }
            return result;
        }

        public string CallMultiCondAttachWFDetails(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.GetMultiCondAttachWFDetails(inputXML);
                result = GetAppSessionKey("CallMultiCondAttachWFDetails", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetMultiCondAttachWFDetails", ex.Message.ToString(), "GetMultiCondAttachWFDetails-" + transId + "");
            }
            return result;
        }

        public string CallAttachMultiCondWF(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.AttachMultiCondWorkFlow(inputXML);
                result = GetAppSessionKey("CallAttachMultiCondWF", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("AttachMultiCondWorkFlow", ex.Message.ToString(), "AttachMultiCondWorkFlow-" + transId + "");
            }
            return result;
        }

        public string CallSaveDelegatedTasks(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.SaveDelegatedTasks(inputXML);
                result = GetAppSessionKey("CallSaveDelegatedTasks", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("SaveDelegatedTasks", ex.Message.ToString(), "SaveDelegatedTasks-" + transId + "");
            }
            return result;
        }

        public string CallSaveWorkflow(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.SaveWorkFlow(inputXML);
                result = GetAppSessionKey("CallSaveWorkflow", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("SaveWorkFlow", ex.Message.ToString(), "SaveWorkFlow-" + transId + "");
            }
            return result;
        }

        public string CallGetAttachWFDetails(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.GetAttachWFDetails(inputXML);
                result = GetAppSessionKey("CallGetAttachWFDetails", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetAttachWFDetails", ex.Message.ToString(), "GetAttachWFDetails-" + transId + "");
            }
            return result;
        }

        public string CallGetFillGridValuesWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbTStruct.GetFillGridValues(inputXML, structure);
                result = GetAppSessionKey("CallGetFillGridValuesWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "GetFillGridValues", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("GetFillGridValues", ex.Message.ToString(), "GetFillGridValues-" + transId + "");
            }
            return result;
        }

        public string CallRapidDoFillGridValuesWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbRTstruct.RapidDoFillGridValues(inputXML);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("RapidDoFillGridValues", ex.Message.ToString(), "RapidDoFillGridValues-" + transId + "");
            }
            return result;
        }

        public string CallSaveDataWS(string transId, string inputXML, string structure, int webServiceTimeout = 0)
        {
            string result = string.Empty;
            int tempWebServiceTimeout = 0;
            int tempWebScriptTimeout = 0;
            if (webServiceTimeout >= 0)
            {
                tempWebServiceTimeout = asbTStruct.Timeout;
                asbTStruct.Timeout = webServiceTimeout;
                tempWebScriptTimeout = HttpContext.Current.Server.ScriptTimeout;
                HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(webServiceTimeout / 1000);
            }
            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbTStruct.SaveData(inputXML, structure);
                result = GetAppSessionKey("CallSaveDataWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "SaveData", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("SaveData", ex.Message.ToString(), "SaveData-" + transId + "");
            }
            if (webServiceTimeout >= 0)
            {
                asbTStruct.Timeout = tempWebServiceTimeout;
                HttpContext.Current.Server.ScriptTimeout = tempWebScriptTimeout;
            }
            return result;
        }

        public string CallWorkFlowActionWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.WorkFlowAction(inputXML);
                result = GetAppSessionKey("CallWorkFlowActionWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("WorkFlowAction", ex.Message.ToString(), "WorkFlowAction-" + transId + "");
            }
            return result;
        }

        public string CallGetDepentendFieldValuesWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;
            string isPerfCode = HttpContext.Current.Session["AxIsPerfCode"] != null ? HttpContext.Current.Session["AxIsPerfCode"].ToString() : "false";
            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                if (isPerfCode == "true")
                    result = asbTStruct.FastGetDepentendFieldValues(inputXML, structure);
                else
                    result = asbTStruct.GetDepentendFieldValues(inputXML, structure);

                //strRequest += "Service Result:" + result + " ♦ ";

                result = GetAppSessionKey("CallGetDepentendFieldValuesWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "GetDepentendFieldValues", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("GetDepentendFieldValues", ex.Message.ToString(), "GetDepentendFieldValues-" + transId + "");
            }
            return result;
        }

        public string CallGetRapidDepFldVals(string transId, string inputXml)
        {
            string result = string.Empty;
            try
            {
                result = asbRTstruct.RapidGetDepentendFieldValues(inputXml);
                result = GetAppSessionKey("CallGetRapidDepFldVals", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Exception in GetRapidDepentendFieldValues", ex.Message.ToString(), "ExcRapidDepentendFieldValues-" + transId + "");
            }
            return result;
        }

        public string CallDoFillGridValuesWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;
            string isPerfCode = HttpContext.Current.Session["AxIsPerfCode"] != null ? HttpContext.Current.Session["AxIsPerfCode"].ToString() : "false";
            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                if (isPerfCode == "true")
                    result = asbTStruct.FastDoFillGridValues(inputXML, structure);
                else
                    result = asbTStruct.DoFillGridValues(inputXML, structure);
                result = GetAppSessionKey("CallDoFillGridValuesWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "FastDoFillGridValues", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("DoFillGridValues", ex.Message.ToString(), "DoFillGridValues-" + transId + "");
            }
            return result;
        }

        public string CallDoExcelImportValuesWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;
            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbTStruct.DoExcelImportValues(inputXML, structure);
                result = GetAppSessionKey("CallDoExcelImportValuesWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "DoExcelImportValues", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("DoExcelImportValues", ex.Message.ToString(), "DoExcelImportValues-" + transId + "");
            }
            return result;
        }

        public string CallDeleteDataWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.DeleteData(inputXML);
                result = GetAppSessionKey("CallDeleteDataWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("DeleteData", ex.Message.ToString(), "Exception-" + transId + "");
            }
            return result;
        }

        public string CallDeleteRowWS(string transId, string inputXML, string sXml)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.RefreshGridDependents(inputXML, sXml);
                result = GetAppSessionKey("CallDeleteRowWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("DeleteData", ex.Message.ToString(), "Exception-" + transId + "");
            }
            return result;
        }

        public string CallDeleteIVRowWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.DeleteRows(inputXML);
                result = GetAppSessionKey("CallDeleteIVRowWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("DeleteIvData", ex.Message.ToString(), "Exception-" + transId + "");
            }
            return result;
        }

        public string CallAddRowWS(string transId, string inputXML, string sXml)
        {
            string result = string.Empty;

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbTStruct.AddGridRowValues(inputXML, sXml);

                //strRequest += "Input XML:" + inputXML.Replace("<", "&lt;").Replace(">", "&gt;") + " ♦♦ Service Result:" + result + " ♦ ";

                result = GetAppSessionKey("CallAddRowWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "AddGridRowValues", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("DeleteData", ex.Message.ToString(), "Exception-" + transId + "");
            }
            return result;
        }

        public string CallCreatePDFWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbAction.CreatePDF(inputXML);
                result = GetAppSessionKey("CallCreatePDFWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CreatePDF", ex.Message.ToString(), "CreatePDF-" + transId + "");
            }
            return result;
        }

        public string CallCreateFastPDFWS(string transId, string inputXML, string sXml)
        {
            string result = string.Empty;

            try
            {
                result = asbAction.CreateFastreport(inputXML, sXml);
                result = GetAppSessionKey("CallCreateFastPDFWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CreatePDF", ex.Message.ToString(), "CreatePDF-" + transId + "");
            }
            return result;
        }

        public string CallGetPrintDocList(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbAction.GetPrintDocList(inputXML);
                result = GetAppSessionKey("CallGetPrintDocList", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CreatePDF", ex.Message.ToString(), "CreatePDF-" + transId + "");
            }
            return result;
        }

        public string CallViewAttachmentsWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.ViewAtachments(inputXML, structure);
                result = GetAppSessionKey("CallViewAttachmentsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("ViewAtachments", ex.Message.ToString(), "ViewAtachments-" + transId + "");
            }
            return result;
        }

        public string CallRemoveAttachmentsWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.RemoveAtachments(inputXML, structure);
                result = GetAppSessionKey("CallRemoveAttachmentsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("RemoveAtachments", ex.Message.ToString(), "RemoveAtachments-" + transId + "");
            }
            return result;
        }

        public string CallRemoveWorkflow(string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.RemoveWorkFlow(inputXML);
                result = GetAppSessionKey("CallRemoveWorkflow", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("RemoveAtachments", ex.Message.ToString(), "RemoveWorkflow-");
            }
            return result;
        }

        public string CallLoadDCCombosWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;
            string isPerfCode = HttpContext.Current.Session["AxIsPerfCode"] != null ? HttpContext.Current.Session["AxIsPerfCode"].ToString() : "false";
            try
            {
                if (isPerfCode == "true")
                    result = asbTStruct.FastLoadDCCombos(inputXML, structure);
                else
                    result = asbTStruct.LoadDCCombos(inputXML, structure);
                result = GetAppSessionKey("CallLoadDCCombosWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("LoadDCCombos", ex.Message.ToString(), "LoadDCCombos-" + transId + "");
            }
            return result;
        }

        public string CallGetSubGridCCombosWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.GetSubGridDropDown(inputXML, structure);
                result = GetAppSessionKey("CallGetSubGridCCombosWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetSubGridDropDown", ex.Message.ToString(), "GetSubGridDropdown-" + transId + "");
            }
            return result;
        }

        public string CallGetSearchResultWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.GetSearchResult(inputXML, structure);
                result = GetAppSessionKey("CallGetSearchResultWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetSearchResult", ex.Message.ToString(), "GetSearchResult-" + transId + "");
            }
            return result;
        }

        public string CallGetDependParamsValuesWS(string transId, string inputXML, string iStructure, int webServiceTimeout)
        {
            string result = string.Empty;

            int tempWebServiceTimeout = asbIview.Timeout;
            asbIview.Timeout = webServiceTimeout;

            int tempWebScriptTimeout = HttpContext.Current.Server.ScriptTimeout;
            HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(webServiceTimeout / 1000);

            try
            {
                result = asbIview.GetDependParamsValues(inputXML, iStructure);
                result = GetAppSessionKey("CallGetDependParamsValuesWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetDependParamsValues", ex.Message.ToString(), "GetDependParamsValues-" + transId + "");
            }

            asbIview.Timeout = tempWebServiceTimeout;

            HttpContext.Current.Server.ScriptTimeout = tempWebScriptTimeout;

            return result;
        }

        public string CallGetStructureWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbDefine.GetStructure(inputXML);
                result = GetAppSessionKey("CallGetStructureWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "GetStructure", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("GetStructure", ex.Message.ToString(), "GetStructure-" + transId + "");
            }
            return result;
        }

        private void CallExceptionErrorPage(string serviceName, string exception, string fileName)
        {
            if (utilObj.sysErrorlog)
                logobj.CreateLog("Exception in " + serviceName + " Service :--- " + exception, HttpContext.Current.Session["nsessionid"].ToString(), fileName, "");
            else
                logobj.CreateLog("Exception in " + serviceName + " Service :--- " + exception, HttpContext.Current.Session["nsessionid"].ToString(), fileName, "new", "true");
            //HttpContext.Current.Server.Transfer(utilObj.errorString);
        }

        public string CallRefreshDc(string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.RefreshDC(inputXML, structure);
                result = GetAppSessionKey("CallRefreshDc", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("RefreshDc", ex.Message.ToString(), "RefreshDc-");
            }
            return result;
        }

        public string SaveCustomizeView(string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbIview.SaveCustomizeView(inputXML);
                result = GetAppSessionKey("SaveCustomizeView", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("SaveCustomizeView", ex.Message.ToString(), "SaveCustomizeView-");
            }
            return result;
        }

        public string DeleteCustomizeView(string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbIview.DeleteCustomizeView(inputXML);
                result = GetAppSessionKey("DeleteCustomizeView", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("DeleteCustomizeView", ex.Message.ToString(), "DeleteCustomizeView-");
            }
            return result;
        }

        public string GetCustomizeViews(string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbIview.GetCustomizeViews(inputXML);
                result = GetAppSessionKey("GetCustomizeViews", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetCustomizeView", ex.Message.ToString(), "GetCustomizeView-");
            }
            return result;
        }

        public string CallExportData(string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUtils.ExportData(inputXML);
                result = GetAppSessionKey("CallExportData", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Call Export Data", ex.Message.ToString(), "ExportData-");
            }
            return result;
        }

        public string CallImportData(string inputXML, int webServiceTimeout)
        {
            string result = string.Empty;
            int tempWebServiceTimeout = asbUtils.Timeout;
            asbUtils.Timeout = webServiceTimeout;
            int tempWebScriptTimeout = HttpContext.Current.Server.ScriptTimeout;
            HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(webServiceTimeout / 1000);
            try
            {
                result = asbUtils.ImportData(inputXML);
                result = GetAppSessionKey("CallImportData", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Call Import Data", ex.Message.ToString(), "ImportData-");
            }
            asbUtils.Timeout = tempWebServiceTimeout;
            HttpContext.Current.Server.ScriptTimeout = tempWebScriptTimeout;
            return result;
        }

        public string CallChangePassword(string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.ChangePassword(inputXML);
                result = GetAppSessionKey("CallChangePassword", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Call Change Password", ex.Message.ToString(), "ChangePwd-");
            }
            return result;
        }
        public string CallTestMailSettings(string inputXML)
        {

            string result = string.Empty;

            try
            {
                result = asbAction.ValidateMailSettings(inputXML);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Call smptp mailsettings", ex.Message.ToString(), "smptpmailsetting-");
            }
            return result;
        }

        public string CallUnlockTStructRecord(string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.UnlockTstructsRecord(inputXML);
                result = GetAppSessionKey("CallUnlockTStructRecord", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Call Unlock record", ex.Message.ToString(), "UnlockRec-");
            }
            return result;
        }

        public string CallAxpString()
        {
            string result = string.Empty;

            try
            {
                //result = asbMenu.GetAxpString();
                result = GetAppSessionKey("CallAxpString", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Call AxpString ID", ex.Message.ToString(), "GetAxpString-");
            }
            return result;
        }

        public string CallServerLicense(string strDetails)
        {
            string result = string.Empty;

            try
            {
                result = asbMenu.SetServerLicense(strDetails);
                //result = GetAppSessionKey("CallServerLicense", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("Call ServerLicense", ex.Message.ToString(), "CallServerLicense-");
            }
            return result;
        }

        /// <summary>Based on the inputxml to Get Field Choices details</summary>
        /// <param1 name="inputXML"> </param1><param2>current transaction structure details</param2>
        /// <returns >return the result in the xml format</returns>
        public string CallRapidSearchWS(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbRTstruct.RapidGetSearchResult(inputXML);
                result = GetAppSessionKey("CallRapidSearchWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetRapidSearchWS", ex.Message.ToString(), "GetRapidSearchWS-" + transId + "");
            }
            return result;
        }

        /// <summary>Based on the inputxml to Get Field Choices details</summary>
        /// <param1 name="inputXML"> </param1><param2>current transaction structure details</param2>
        /// <returns >return the result in the xml format</returns>
        public string CallFastSearchWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.FastGetSearchResult(inputXML, structure);  // result = asbTStruct.FastSearchResult(inputXML, structure);//
                result = GetAppSessionKey("CallFastSearchWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallGetFastSearchWS", ex.Message.ToString(), "CallGetFastSearchWS-" + transId + "");
            }
            return result;
        }
        public string CallAutoGetSearchWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.AutoGetSearchResult(inputXML, structure);  // result = asbTStruct.FastSearchResult(inputXML, structure);//
                result = GetAppSessionKey("CallAutoGetSearchWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallAutoGetSearchWS", ex.Message.ToString() + "-inputXML-" + inputXML, "CallAutoGetSearchWS-" + transId + "");
            }
            return result;
        }

        public string CallGetMultiSelectValues(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.GetMultiSelectValues(inputXML, structure);
                result = GetAppSessionKey("CallGetMultiSelectValues", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallGetMultiSelectValues", ex.Message.ToString() + "-inputXML-" + inputXML, "CallGetMultiSelectValues-" + transId + "");
            }
            return result;
        }

        public string CallAutoGetSearchNewWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbTStruct.AutoGetSearchResultNew(inputXML, structure);
                result = GetAppSessionKey("CallAutoGetSearchNewWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "GetSearchResult", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
                CallExceptionErrorPage("CallAutoGetSearchNewWS", ex.Message.ToString() + "-inputXML-" + inputXML, "CallAutoGetSearchNewWS-" + transId + "");
            }
            return result;
        }

        public string CallFastFieldChoicesWS(string transId, string inputXML, string structure)
        {
            string result = string.Empty;

            try
            {
                result = asbTStruct.FastFieldChoices(inputXML, structure);
                result = GetAppSessionKey("CallFastFieldChoicesWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetFieldChoices", ex.Message.ToString(), "GetFieldChoices-" + transId + "");
            }
            return result;
        }

        public string CallMultiSQLExec(string transId, string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbMenu.MultiSQLExec(inputXML);
                result = GetAppSessionKey("CallMultiSQLExec", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("MultiSQLExec", ex.Message.ToString(), "MultiSQLExec-" + transId + "");
            }
            return result;
        }
        public String GetEncryptedValue(string password)
        {
            string result = string.Empty;

            try
            {
                result = asbUser.GetEncryptedValue(password);
            }
            catch (Exception ex)
            {
            }
            return result;
        }


        public string CallGetHomePageDetailsWS(string inputXML)
        {
            string result = string.Empty;
            try
            {
                result = asbMenu.GetHomePageDetails(inputXML);
                result = GetAppSessionKey("CallGetHomePageDetailsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallGetHomePageDetailsWS", ex.Message.ToString(), "CallGetHomePageDetailsWS");
            }
            return result;
        }

        public string CallGetCustomSqlWS(string inputXML)
        {
            string result = string.Empty;
            try
            {
                result = asbMenu.GetCustomSqlResult(inputXML);
                result = GetAppSessionKey("CallGetCustomSqlWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallGetCustomSqlWS", ex.Message.ToString(), "CallGetCustomSqlWS");
            }
            return result;
        }

        public string CallGetIndividualWidgetDetailsWS(string inputXML)
        {
            string result = string.Empty;
            try
            {
                result = asbMenu.GetIndividualWidgetDetails(inputXML);
                result = GetAppSessionKey("CallGetIndividualWidgetDetailsWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallGetIndividualWidgetDetailsWS", ex.Message.ToString(), "CallGetIndividualWidgetDetailsWS");
            }
            return result;
        }


        public string GetAppSessionKey(string funcName, string result)
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToShortTimeString();
            try
            {
                if (result.TrimStart().StartsWith("<") && result.TrimEnd().EndsWith(">"))//XML
                {
                    string[] splitRes = result.Split(new[] { "#$#" }, StringSplitOptions.None);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(splitRes[0]);
                    if (xmlDoc.DocumentElement.Attributes["appsessionkey"] != null)
                    {
                        string AppSessKey = xmlDoc.DocumentElement.Attributes["appsessionkey"].Value;
                        if (AppSessKey != null && AppSessKey != "")
                            HttpContext.Current.Session["AppSessionKey"] = AppSessKey;
                        xmlDoc.DocumentElement.RemoveAttribute("appsessionkey");
                        splitRes[0] = xmlDoc.DocumentElement.OuterXml;
                    }
                    result = string.Join("#$#", splitRes);
                }
                else if (result.TrimStart().StartsWith("{")) //Json
                {
                    string[] orgResult = result.Split(new[] { "#$#" }, StringSplitOptions.None);
                    string appJsonKey = orgResult[0];
                    if (!string.IsNullOrEmpty(appJsonKey) && appJsonKey.Contains("appsessionkey"))
                    {
                        var AppJsonValue = JObject.Parse(appJsonKey)["appsessionkey"].Select(el => new { value = (string)el["value"] }).ToList();
                        HttpContext.Current.Session["AppSessionKey"] = AppJsonValue[0].value;
                        result = string.Join("#$#", orgResult.Skip(1).ToArray());
                    }
                }
                else if (result.TrimStart().StartsWith("<"))//XML+JSON
                {
                    string splitter = "*$*";

                    if (result.IndexOf(splitter) == -1)
                    {
                        splitter = "#$#";
                    }

                    string[] splitRes = result.Split(new[] { splitter }, StringSplitOptions.None);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(splitRes[0]);
                    if (xmlDoc.DocumentElement.Attributes["appsessionkey"] != null)
                    {
                        string AppSessKey = xmlDoc.DocumentElement.Attributes["appsessionkey"].Value;
                        if (AppSessKey != null && AppSessKey != "")
                            HttpContext.Current.Session["AppSessionKey"] = AppSessKey;
                        xmlDoc.DocumentElement.RemoveAttribute("appsessionkey");
                        splitRes[0] = xmlDoc.DocumentElement.OuterXml;
                    }
                    result = string.Join(splitter, splitRes);
                }
                return result;
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("GetAppSessionKey", ex.Message.ToString(), "GetAppSessionKey-" + funcName + "");
                return result;
            }
        }


        public string SaveSmartViews(string iView, string inputXML, int webServiceTimeout)
        {
            string result = string.Empty;


            int tempWebServiceTimeout = asbIview.Timeout;
            asbIview.Timeout = webServiceTimeout;


            int tempWebScriptTimeout = HttpContext.Current.Server.ScriptTimeout;
            HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(webServiceTimeout / 1000);


            try
            {
                result = asbIview.SaveSmartViews(inputXML);
                result = GetAppSessionKey("CallSaveSmartViews", result);
            }
            catch (Exception ex)
            {
                result = "<error>" + ex.Message.ToString();
                CallExceptionErrorPage("SaveSmartViews", ex.Message.ToString(), "SaveSmartViews-" + iView + "");
            }


            asbIview.Timeout = tempWebServiceTimeout;


            HttpContext.Current.Server.ScriptTimeout = tempWebScriptTimeout;


            return result;
        }

        public string ExecuteSQL(string transid, string sqlQuery, string format = "")
        {
            string result = string.Empty;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string errorLog = logobj.CreateLog("Call ExecuteSQ", HttpContext.Current.Session["nsessionid"].ToString(), "CallExecuteSQL-" + transid + "", "");
            string inputXML = string.Empty;
            inputXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' responseformat='" + format + "'  ><sql>" + sqlQuery + "</sql>";
            inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            try
            {
                result = asbMenu.ExecuteSQL(inputXML);
                result = GetAppSessionKey("CallExecuteSQL", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("ExecuteSQL", ex.Message.ToString(), "ExecuteSQL-" + transid + "");
                result = "{ \"error\":" + ex.Message + " }";

            }

            return result;
        }

        public string callRearrangeMenuWS(string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.RearrangeMenu(inputXML);
                result = GetAppSessionKey("CallRearrangeMenuWS", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("callArrangeMenuWS", ex.Message.ToString(), "callArrangeMenuWS");
            }
            return result;
        }

        public string GetToolBar(string inputXML)
        {
            string result = string.Empty;

            try
            {
                result = asbDefine.GetToolBar(inputXML);
                result = GetAppSessionKey("CallGetToolBar", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallGetToolBar", ex.Message.ToString(), "CallGetToolBar");
            }
            return result;

        }
        public string saveToolbar(string inputXML)
        {
            string result = string.Empty;

            try
            {

                result = asbDefine.SaveToolBar(inputXML);
                result = GetAppSessionKey("CallGetToolBar", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallGetToolBar", ex.Message.ToString(), "CallGetToolBar");
            }
            return result;

        }

        public string callRemoteDoScriptWS(string transId, string inputXML, string ires, int webServiceTimeout)
        {
            string result = string.Empty;

            string redisIP = string.Empty;

            string redisPort = string.Empty;

            string redisPass = string.Empty;


            if (HttpContext.Current.Session["RedisCacheIP"] != null && HttpContext.Current.Session["RedisCacheIP"].ToString() != "")
            {
                string[] redisIpPort = HttpContext.Current.Session["RedisCacheIP"].ToString().Split(':');
                if (redisIpPort.Length > 0)
                {
                    redisIP = redisIpPort[0];
                }
                if (redisIpPort.Length > 1)
                {
                    redisPort = redisIpPort[1];
                }
            }

            if (HttpContext.Current.Session["RedisCachePwd"] != null && HttpContext.Current.Session["RedisCachePwd"].ToString() != "")
            {
                redisPass = HttpContext.Current.Session["RedisCachePwd"].ToString();
            }

            if (webServiceTimeout != 0)
            {
                string notifyTimeout = string.Empty;

                try
                {
                    notifyTimeout = utilObj.GetAdvConfigs("notification time interval");
                }
                catch (Exception ex)
                { }

                //webServiceTimeout = 10;
                var webServiceTimeoutsecs = webServiceTimeout / 1000;

                if (notifyTimeout != string.Empty && inputXML != string.Empty)
                {
                    inputXML = inputXML.Replace("<root", "<root timeout='" + webServiceTimeoutsecs + "'");
                    inputXML = inputXML.Replace("<root", "<root redispwd='" + redisPass + "'");
                    inputXML = inputXML.Replace("<root", "<root redisserver ='" + redisIP + "'");
                    inputXML = inputXML.Replace("<root", "<root redisportno ='" + redisPort + "'");
                }

            }
            int tempWebServiceTimeout = asbScript.Timeout;
            asbScript.Timeout = webServiceTimeout;

            int tempWebScriptTimeout = HttpContext.Current.Server.ScriptTimeout;
            HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(webServiceTimeout / 1000);

            try
            {
                string strRequest = ObjExecTr.RequestProcessTime("Request");
                DateTime kst = DateTime.Now;
                result = asbScript.RemoteDoScript(inputXML, ires);

                if (result != "" && result.Contains("#*#") && result.StartsWith("clearcache="))
                    result = ClearCacheonScriptSave(result);

                result = GetAppSessionKey("CallRemoteDoScriptWS", result);
                result = strRequest + ObjExecTr.KernelProcessTime(kst, "RemoteDoScript", inputXML, result) + "♠" + result;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().IndexOf("operation has timed out") != -1)
                {
                    //result = "This proces taking time is more than expected. You will get a notification once completed";
                    result = "{\"error\":[{\"msg\":\"This proces taking time is more than expected. You will get a notification once completed\"}]}";
                }
                else
                    CallExceptionErrorPage("RemoteDoScript", ex.Message.ToString(), "RemoteDoScript-" + transId + "");
                result = ObjExecTr.ResponseErrorMsg("Kernel - " + ex.Message) + "♠" + result;
            }

            asbScript.Timeout = tempWebServiceTimeout;

            HttpContext.Current.Server.ScriptTimeout = tempWebScriptTimeout;

            return result;
        }

        private string ClearCacheonScriptSave(string scriptRes)
        {
            string _thisResult = string.Empty;
            try
            {
                string returnStr = scriptRes.Replace("#*#", "¿");
                string[] newResult = returnStr.Split('¿');
                _thisResult = newResult[1];

                string cacheList = newResult[0];
                cacheList = cacheList.Replace("clearcache=", "");
                Util.Util util = new Util.Util();
                FDW fdwObj = new FDW();
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                    fObj = new FDR();
                foreach (var _thisName in cacheList.Split(','))
                {
                    string[] _thisVar = _thisName.Split('~');
                    if (_thisVar[0] == "t")
                    {
                        string fdData = Constants.REDISTSTRUCTALL;
                        var dbVarKeys = fObj.GetWildCardKeyNames(util.GetRedisServerkey(fdData, _thisVar[1]));
                        fdwObj.DeleteKeys(dbVarKeys);

                        string fdMemVar = Constants.DBMEMVARSFORMLOAD;
                        var dbMemVarKeys = fObj.GetWildCardKeyNames(util.GetRedisServerkey(fdMemVar, _thisVar[1], "*"));
                        fdwObj.DeleteKeys(dbMemVarKeys);
                    }
                    else if (_thisVar[0] == "i")
                    {
                        var dbIVarKeys = fObj.GetWildCardKeyNames(_thisVar[1]);
                        fdwObj.DeleteKeys(dbIVarKeys);

                        var dblvDataKeys = fObj.GetWildCardKeyNames(_thisVar[1]);
                        fdwObj.DeleteKeys(dblvDataKeys);
                    }
                }
            }
            catch (Exception ex)
            {
                _thisResult = scriptRes;
            }
            return _thisResult;
        }

        public string CallActivateLicenseWS(string inputXML)
        {
            string result = string.Empty;
            try
            {
                result = asbMenu.ActivateLicense(inputXML);
                //result = GetAppSessionKey("CallGetToolBar", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallActivateLicenseWS", ex.Message.ToString(), "CallActivateLicenseWS");
            }
            return result;

        }

        public string CallAxMemVarAvailable(string transId, string inputXML)
        {
            string result = string.Empty;
            try
            {
                result = asbTStruct.GetAxMemLoadVars(inputXML);
                result = GetAppSessionKey("CallAxMemVarAvailable", result);
            }
            catch (Exception ex)
            {
                CallExceptionErrorPage("CallAxMemVarAvailable", ex.Message.ToString(), "CallAxMemVarAvailable");
            }
            return result;
        }

        public string CallOpenSessionWS(string inputXML)
        {
            string result = string.Empty;
            try
            {
                result = asbMenu.OpenSessionForFlutter(inputXML);
                result = GetAppSessionKey("CallOpenSessionWS", result);
            }
            catch (Exception ex)
            {
                if (utilObj.sysErrorlog)
                {
                    logobj.CreateLog("Exception in Open Session Service :--- " + ex.Message.ToString(), HttpContext.Current.Session["nsessionid"].ToString(), "OpenSession", "");
                }
                throw ex;
            }
            return result;
        }
    }
}
