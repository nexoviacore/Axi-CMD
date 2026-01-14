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
using ASBExt;
using System.Text.RegularExpressions;
using Ionic.Zip;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using AxInterface;
using System.Net.Mail;
using Microsoft.Ajax.Utilities;
using BotDetect;
using System.Security.Cryptography;
using System.Xml.Linq;
using RabbitMQ.Client;

namespace ASB
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        Util.Util utils = new Util.Util();
        public string strFillDepPName = string.Empty;
        public ArrayList arrFillList = new ArrayList();
        AxpertDotNet ads = new AxpertDotNet();
        public string axpert = "Axpert\\";
        public const string DATA = "data";
        LogFile.Log logobj = new LogFile.Log();
        Util.Util utilObj = new Util.Util();
        CacheMgr.CacheManager cm = new CacheMgr.CacheManager(string.Empty);
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        string project = string.Empty;
        string user = string.Empty;
        string AxRole = string.Empty;
        string sessionid = string.Empty;
        string transid = string.Empty;
        string ivname = string.Empty;
        public const int capacity = 10;
        List<string> allUrls = new List<string>(capacity);
        List<string> allUrlsIdx = new List<string>(capacity);
        ExecTrace ObjExecTr = ExecTrace.Instance;

        //Since the change theme is done in javascript, on postback the selected theme has to be saved in session.
        //Hence this function acts as a wrapper to update the theme in session and continue to update the db.
        [WebMethod(EnableSession = true)]
        public string UpdateTheme(string theme, string trace)
        {

            string inputXML = string.Empty;
            string result = string.Empty;

            if (trace != "♦♦GetChoicestoUpdateAppearance♦")
                return utilObj.ErrorMsgToJson("Invalid trace value.");

            bool themevalid = utilObj.IsChar(theme);
            if (!themevalid)
            {
                return utilObj.ErrorMsgToJson("Theme Invalid");
            }
            else
            {
                if (HttpContext.Current.Session["project"] == null)
                    return utilObj.SESSTIMEOUT;
                HttpContext.Current.Session["themeColor"] = theme;
                GetGlobalVars();
                inputXML = "<sqlresultset axpapp='" + project + "' transid='" + "" + "' sessionid='" + sessionid + "' trace='" + trace + "' >";
                inputXML += "<sql>Update axusers SET webskin='" + theme + "' where username='" + user + "'</sql>";
                inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                string iXML = GetTraceString(inputXML);
                if (iXML != string.Empty) inputXML = iXML;
                result = asbExt.CallGetChoiceWebService("", inputXML);
                return result;
            }

        }

        [WebMethod(EnableSession = true)]
        public string SaveDataXML(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, ArrayList deletedFldArrayValues, string files, string rid, string delRows, string changedRows, string key, string axRulesFlds = "", string axPegApprovalSave = "", string resTstHtmlLS = "", string isLoadFromDraft = "")
        {
            string requestProcess_logtime = string.Empty;
            ObjExecTr.SetCurrentTime();
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;

            if (tstData.transID != "temps")
            {
                if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
                {
                    return Constants.MALICIOUSNPUTDETECTED;
                }
            }

            string result = string.Empty;
            DateTime eTime1 = DateTime.Now;

            Util.Util.DeletedraftRediskeys(tstData.transid);
            result = tstData.CallSaveDataWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, deletedFldArrayValues, files, rid, delRows, changedRows, axRulesFlds, axPegApprovalSave, isLoadFromDraft);
            requestProcess_logtime += result.Split('♠')[0];
            result = result.Split('♠')[1];
            DateTime sTime2 = DateTime.Now;
            if (!result.StartsWith(Constants.ERROR))
            {
                if (tstData.transid.ToLower() == "axglo")
                    Session["AxInternalRefresh"] = "true";
                if (tstData.IsDraftObj && HttpContext.Current.Session["AxEnableDraftSave"] != null && HttpContext.Current.Session["AxEnableDraftSave"].ToString() == "true")
                {
                    user = HttpContext.Current.Session["user"].ToString();
                    utilObj.DeleteDraft(user + "-" + tstData.transid, user);
                }
            }
            DateTime eTime2 = DateTime.Now;

            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));

            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", result);
            string serverprocesstime = ObjExecTr.TotalServerElapsTime();
            result = serverprocesstime + "*♠*" + requestProcess_logtime + "*♠*" + result;
            if (_tstKeyChanged != "")
            {
                result = _tstKeyChanged + "♠*♠" + result;
            }
            return result;
        }

        [WebMethod()]
        public void RemoveNewFiles(string f, string sid)
        {
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();

            if ((!string.IsNullOrEmpty(f)))
            {
                //Create a Folder
                DirectoryInfo di = new DirectoryInfo(ScriptsPath + axpert + sid);
                //' Determine whether the directory exists.
                if (di.Exists)
                {
                    string outputPath = null;
                    outputPath = ScriptsPath + axpert + sid + "\\" + f;
                    //' Delete a file
                    File.Delete(outputPath);
                }
            }
        }

        [WebMethod(EnableSession = true)]
        private string GetTraceString(string currentXml)
        {
            string traceVal = string.Empty;
            string errlog = string.Empty;
            traceVal = currentXml;

            if (currentXml.Contains("_#") == true)
            {
                currentXml = currentXml.Replace("_#", "~");
            }

            string filename = string.Empty;
            int sindex = currentXml.IndexOf("♦♦");
            int eindex = currentXml.LastIndexOf("♦");

            if (sindex != -1 && eindex != -1)
            {
                filename = currentXml.Substring(sindex + 2, eindex - sindex - 2);
                sessionid = HttpContext.Current.Session["nsessionid"].ToString();
                errlog = logobj.CreateLog("Executing Action", sessionid, filename, "new");
                traceVal = currentXml.Replace("♦♦" + filename + "♦", errlog);
            }

            if (traceVal.Contains("~") == true)
            {
                traceVal = traceVal.Replace("~", "_#");
            }
            if (traceVal != null && traceVal != "")// Reddy
            {
                if (traceVal.TrimStart().StartsWith("<root"))
                {
                    traceVal = traceVal.Replace("<root", "<root appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'");
                }
                else if (traceVal.TrimStart().StartsWith("<sqlresultset"))
                {
                    traceVal = traceVal.Replace("<sqlresultset", "<sqlresultset appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'");
                }
            }
            return traceVal;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteDataXML(string rid, string s, string key, bool allowCancel = true, string resTstHtmlLS = "")
        {
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if ((!utilObj.IsNumber(rid)) || (!utilObj.IsAlphaNumUnd(key)))// IsAlphaNum
                return "{\"error\":[{\"msg\":\"Error: Invalid parameter value.\"}]}";
            else if (!allowCancel)
            {
                sessionid = HttpContext.Current.Session["nsessionid"].ToString();
                logobj.CreateLog("Delete Data: This transaction cannot be deleted. In workflow, 'Allow cancel after approval' is 'false'.", sessionid, "Delete-" + key, "new");
                return "{\"error\":[{\"msg\":\"This transaction cannot be deleted.\"}]}";
            }
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string result = string.Empty;
            //s = s + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</Transaction>";
            DateTime eTime1 = DateTime.Now;
            result = tstData.CallDeleteDataWS(s);
            //if (result.ToLower().Contains("success") && utilObj.isRealTimeCacEnabled(transid, tstData.tstStrObj))
            //    result = result + "*#*" + utilObj.GetAxRelations(tstData.transid);
            DateTime sTime2 = DateTime.Now;
            if (utilObj.IsFileInCache(tstData.transid, rid))
                utilObj.DeleteFromCache(result, rid, tstData.transid);
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteRowWS(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string ArrActionLog, string s, string key, string dcNo, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            if (tstData == null)
                return Constants.DUPLICATESESS;
            sessionid = HttpContext.Current.Session["nsessionid"].ToString();
            string result = string.Empty;
            logobj.CreateActionLog(ArrActionLog, sessionid, "ActionLog", "");
            string errorLog = logobj.CreateLog("Delete row webservice", sessionid, "DeleteRow-" + tstData.transid, "new");
            result = tstData.CallDeleteRowWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, s, dcNo);
            //TStructDef tstObj = utilObj.GetTstructDefObj("GetStructure", tstData.transid);
            tstData.JsonToArray(result, tstData.tstStrObj, "", "DeleteRow");
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteRowPerfWS(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string ArrActionLog, string s, string key, string dcNo, ArrayList DeletedRowNos, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;

            if (tstData.transID != "temps")
            {
                if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
                {
                    return Constants.MALICIOUSNPUTDETECTED;
                }
            }

            sessionid = HttpContext.Current.Session["nsessionid"].ToString();
            string result = string.Empty;
            logobj.CreateActionLog(ArrActionLog, sessionid, "ActionLog", "");
            string errorLog = logobj.CreateLog("Delete row webservice", sessionid, "DeleteRow-" + tstData.transid, "new");
            result = tstData.CallDeleteRowPerfWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, s, dcNo, DeletedRowNos);
            tstData.JsonToArray(result, tstData.tstStrObj, "", "DeleteRow");
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string AddRowWS(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string s, string dcNo, string key, string resTstHtmlLS = "")
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            sessionid = HttpContext.Current.Session["nsessionid"].ToString();
            string result = string.Empty;
            string errorLog = logobj.CreateLog("Add row webservice", sessionid, "AddRow-" + tstData.transid, "new");
            result = tstData.CallAddRowWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, s, dcNo);
            //TStructDef tstObj = utilObj.GetTstructDefObj("GetStructure", tstData.transid);
            tstData.JsonToArray(result, tstData.tstStrObj, "", "AddRow");
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string AddRowPerfWS(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string s, string dcNo, string key, bool isFromScript = false, string resTstHtmlLS = "")
        {
            string requestProcess_logtime = string.Empty;
            ObjExecTr.SetCurrentTime();
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            sessionid = HttpContext.Current.Session["nsessionid"].ToString();
            string result = string.Empty;
            string errorLog = logobj.CreateLog("Add row webservice", sessionid, "AddRow-" + tstData.transid, "new");
            result = tstData.CallAddRowPerfWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, s, dcNo);
            requestProcess_logtime += result.Split('♠')[0];
            result = result.Split('♠')[1];
            //TStructDef tstObj = utilObj.GetTstructDefObj("GetStructure", tstData.transid);
            bool isValidRow = true;
            try
            {
                if (isFromScript && result != "")
                {
                    string jsonResult = result.Replace("*$*", "♦");
                    jsonResult = jsonResult.Replace("\\", ";bkslh");
                    string[] resval = jsonResult.Split('♦');
                    for (int j = 0; j < resval.Length; j++)
                    {
                        JArray Data = null;
                        try
                        {
                            JObject tData = JObject.Parse(resval[j]);
                            Data = (JArray)tData["data"];
                        }
                        catch (Exception exe) { }

                        for (int m = 0; m < Data.Count; m++)
                        {
                            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(Data[m].ToString());
                            string fName = values["n"];
                            string fValue = values["v"];
                            //if (fName.ToLower() == "validrow" + dcNo && fValue != "" && fValue.ToLower() == "f")
                            if (fName.ToLower() == "validrow" + dcNo && fValue != "" && fValue.ToLower() != "t")
                            {
                                isValidRow = false;
                                if (fValue.ToLower() == "f")
                                    result = "Not a valid row!";
                                else
                                    result = "Notavalidrow♦" + fValue;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isValidRow = true;
            }
            if (isValidRow)
                tstData.JsonToArray(result, tstData.tstStrObj, "", "AddRow");

            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", result);
            string serverprocesstime = ObjExecTr.TotalServerElapsTime();
            result = serverprocesstime + "*♠*" + requestProcess_logtime + "*♠*" + result;
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string CreatePDF(string s, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            //s = s + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            string result = string.Empty;
            result = tstData.CallCreatePDFWS(s);
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string CreateFastReportPDF(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string s, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            tstData.GetFieldValueXml(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, "-1", "false", "ALL", "");
            s += "<varlist><row>" + tstData.fldValueXml + tstData.memVarsData + "</row></varlist>";
            //s += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            string result = string.Empty;
            result = tstData.CallCreateFastPDFWS(s);
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string CallActionWS(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, ArrayList deletedFldArrayValues, string ArrActionLog, string visibleDcs, string s, string f, string source, string delRows, string changedRows, string key, string files = "", bool isScript = false, string axRulesFlds = "", bool axRulesScript = false, string resTstHtmlLS = "", string isLoadFromDraft = "")
        {
            string requestProcess_logtime = string.Empty;
            ObjExecTr.SetCurrentTime();
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            if (source != "tsect")
            {
                if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
                {
                    return Constants.MALICIOUSNPUTDETECTED;
                }
            }
            if (source == "tsect")
                source = "t";

            string result = string.Empty;
            string ires = string.Empty;
            string filename = string.Empty;
            IviewData objIview = new IviewData();
            GetGlobalVars();

            if (!utilObj.IsAlphaNumUnd(key))
                return utilObj.ErrorMsgToJson("Invalid parameter.");

            if (!utilObj.IsParamArrayList(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray))
                return utilObj.ErrorMsgToJson("Invalid parameter.");

            if (source != "t" && source != "i")
                return utilObj.ErrorMsgToJson("Invalid parameter.");

            if (source != "t")
            {
                string news = GetTraceString(s);
                if (news != string.Empty)
                {
                    if (news.Contains("_#") && news.Contains("</params><varlist>"))
                    {
                        string[] splitNews = news.Split(new[] { "</params><varlist>" }, StringSplitOptions.None);

                        if (splitNews[1] != string.Empty)
                        {
                            splitNews[1] = splitNews[1].Replace("_#", "~");

                            news = String.Join("</params><varlist>", splitNews);
                        }
                    }

                    s = news;
                }
                string data = string.Empty;

                if ((!string.IsNullOrEmpty(f)))
                {
                    if (File.Exists(f))
                        data = File.ReadAllText(f);

                    utilObj.UploadFiles(f, sessionid);
                }
                if (key != "")
                {
                    objIview = (IviewData)HttpContext.Current.Session[key];
                    if (objIview == null)
                        return Constants.DUPLICATESESS;
                }
                //ires = objIview.StructureXml;
                s = s + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
                if (isScript)
                {
                    filename = "Script-" + transid;
                    logobj.CreateLog("Call to RemoteDoScript Web Service", sessionid, filename, string.Empty);
                    result = asbExt.callRemoteDoScriptWS("", s, ires, asbExt.asbScript.Timeout);
                }
                else
                {
                    filename = "Action-" + transid;
                    logobj.CreateLog("Call to RemoteDoAction Web Service", sessionid, filename, string.Empty);
                    result = asbExt.CallRemoteDoActionWS("", s, ires, asbExt.asbAction.Timeout);
                }

                requestProcess_logtime += result.Split('♠')[0];
                result = result.Split('♠')[1];
                logobj.CreateLog("End Time : " + DateTime.Now.ToString(), sessionid, filename, string.Empty);

                requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", result);
                string serverprocesstime = ObjExecTr.TotalServerElapsTime();
                result = serverprocesstime + "*♠*" + requestProcess_logtime + "*♠*" + result;
                return result;
            }
            else
            {
                string data = string.Empty;
                if ((!string.IsNullOrEmpty(f)))
                {
                    if (File.Exists(f))
                        data = File.ReadAllText(f);

                    utilObj.UploadFiles(f, sessionid);
                }
                string _tstKeyChanged = "";
                if (resTstHtmlLS != "")
                {
                    utilObj.DeleteTstIvObject(key.Split('_')[0]);
                    key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                    _tstKeyChanged = key;
                }
                TStructData tstData = (TStructData)Session[key];
                if (tstData == null)
                    return Constants.DUPLICATESESS;

                if (tstData != null)
                {
                    Util.Util.DeletedraftRediskeys(tstData.transid);
                    logobj.CreateActionLog(ArrActionLog, sessionid, "ActionLog", "");
                    string calledFrom = ArrActionLog.StartsWith("CallAction-axpbutton_") ? "axpbutton" : "false";
                    result = tstData.CallActionWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, deletedFldArrayValues, s, f, source, delRows, changedRows, calledFrom: calledFrom, files: new StringBuilder(files), isScript: isScript, axRulesFlds: axRulesFlds, axRulesScript: axRulesScript, isLoadFromDraft: isLoadFromDraft);
                    requestProcess_logtime += result.Split('♠')[0];
                    result = result.Split('♠')[1];
                    tstData.JsonToArray(result, tstData.tstStrObj, "", "Action");
                    //string AxRelKeys = string.Empty;
                    //if (Session["AxRelKeys"] != null)
                    //{
                    //    AxRelKeys = "*#*" + Session["AxRelKeys"].ToString();
                    //    Session["AxRelKeys"] = null;
                    //}
                    //result = result + "*♠*" + tstData.GetVisibleFormatGridsHtml(tstData.tstStrObj, visibleDcs) + AxRelKeys;
                    result = result + "*♠*" + tstData.GetVisibleFormatGridsHtml(tstData.tstStrObj, visibleDcs);
                }
                else
                    result = "{\"error\":[{\"msg\":\"Error: Please reload the page and try again.\"}]}";

                requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", result);
                string serverprocesstime = ObjExecTr.TotalServerElapsTime();
                result = serverprocesstime + "*♠*" + requestProcess_logtime + "*♠*" + result;
                if (_tstKeyChanged != "")
                    result = _tstKeyChanged + "♠*♠" + result;
                return result;
            }
        }

        [WebMethod(EnableSession = true)]
        public string ViewAttachment(string s, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            sessionid = HttpContext.Current.Session["nsessionid"].ToString();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            transid = tstData.transid;
            string errorLog = logobj.CreateLog("Get Structure object from cache for view attachment.", sessionid, "GetStructXml-", "new");
            s = GetTraceString(s);
            //TStructDef tstObj = utilObj.GetTstructDefObj(errorLog, transid);
            TStructDef tstObj = tstData.tstStrObj;
            string result = string.Empty;
            s = s + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            result = tstData.CallViewAttachWS(s, tstObj.structRes);
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string RemoveAttachment(string s, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            sessionid = HttpContext.Current.Session["nsessionid"].ToString();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            if (transid == "")
                transid = tstData.transid;
            string result = string.Empty;
            //s = s + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            string errorLog = logobj.CreateLog("Get Structure object from cache for view attachment.", sessionid, "GetStructXml-", "new");
            //TStructDef tstObj = utilObj.GetTstructDefObj(errorLog, transid);
            TStructDef tstObj = tstData.tstStrObj;
            result = tstData.CallRemoveAttachWS(s, tstObj.structRes);
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        //NOTE: webservice needs to be released in new dll
        [WebMethod(EnableSession = true)]
        public string DeleteIviewRow(string recIds, string transid, string s)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string news = GetTraceString(s);
            if (news != string.Empty) s = news;
            string result = string.Empty;
            s = s + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            result = asbExt.CallDeleteIVRowWS("", s);
            //if (result.ToLower().Contains("done") && utilObj.isRealTimeCacEnabled(transid, null))
            //    result = result + "*#*" + utilObj.GetAxRelations(transid);
            utilObj.DeleteRecordsFromCache(result, recIds, transid);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetDepFlds(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string ArrActionLog, string visibleDcs, string s, string tid, string key, string frameNo, string rowNo, string fieldName, string subGridInfo, string parentInfo, string resTstHtmlLS)
        {
            string requestProcess_logtime = string.Empty;
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string result = string.Empty;
            GetGlobalVars();
            logobj.CreateActionLog(ArrActionLog, sessionid, "ActionLog", "");
            tstData.SetSubGridRows(subGridInfo);
            tstData.AxActiveDc = frameNo;
            DateTime eTime1 = DateTime.Now;
            result = tstData.CallGetDepFldWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, s, frameNo, rowNo, fieldName, subGridInfo, parentInfo);
            requestProcess_logtime += result.Split('♠')[0];
            result = result.Split('♠')[1];
            DateTime sTime2 = DateTime.Now;
            //TStructDef tstObj = utilObj.GetTstructDefObj("Get structure object", tid);
            TStructDef tstObj = tstData.tstStrObj;
            if (result != "")
            {
                try
                {
                    if (tstObj.IsDcGrid(Convert.ToInt32(frameNo)))
                        tstData.JsonToArray(result, tstObj, "", "GetDepPerf");
                    else
                        tstData.JsonToArray(result, tstObj, "", "GetDep");
                }
                catch (Exception ex)
                { }
            }
            tstData.AxActiveDc = string.Empty;
            result = result + "*♠*" + tstData.GetVisibleFormatGridsHtml(tstObj, visibleDcs);
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));

            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", result);
            string serverprocesstime = ObjExecTr.TotalServerElapsTime();
            result = serverprocesstime + "*♠*" + requestProcess_logtime + "*♠*" + result;
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetDepFldsPerf(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, ArrayList RegVarFldList, string ArrActionLog, string visibleDcs, string s, string tid, string key, string frameNo, string rowNo, string perfDepFldName, string fieldName, string subGridInfo, string parentInfo, ArrayList acceptExpressionFlds, string resTstHtmlLS)
        {
            string requestProcess_logtime = string.Empty;
            ObjExecTr.SetCurrentTime();
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string result = string.Empty;
            GetGlobalVars();
            logobj.CreateActionLog(ArrActionLog, sessionid, "ActionLog", "");
            tstData.SetSubGridRows(subGridInfo);
            tstData.AxActiveDc = frameNo;
            DateTime eTime1 = DateTime.Now;
            result = tstData.CallGetDepFldPerfWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, RegVarFldList, s, frameNo, rowNo, perfDepFldName, fieldName, subGridInfo, parentInfo, acceptExpressionFlds);
            requestProcess_logtime += result.Split('♠')[0];
            result = result.Split('♠')[1];
            DateTime sTime2 = DateTime.Now;
            //TStructDef tstObj = utilObj.GetTstructDefObj("Get structure object", tid);
            TStructDef tstObj = tstData.tstStrObj;
            if (result != "")
            {
                try
                {
                    if (tstObj.IsDcGrid(Convert.ToInt32(frameNo)))
                        tstData.JsonToArray(result, tstObj, "", "GetDepPerf");
                    else
                        tstData.JsonToArray(result, tstObj, "", "GetDep");
                }
                catch (Exception ex)
                { }
            }
            tstData.AxActiveDc = string.Empty;
            result = result + "*♠*" + tstData.GetVisibleFormatGridsHtml(tstObj, visibleDcs);
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));

            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", result);
            string serverprocesstime = ObjExecTr.TotalServerElapsTime();
            result = serverprocesstime + "*♠*" + requestProcess_logtime + "*♠*" + result;
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }


        [WebMethod(EnableSession = true)]
        public string GetRapidDepFlds(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string ArrActionLog, string visibleDcs, string s, string tid, string key, string frameNo, string rowNo, string fieldName, string subGridInfo, string parentInfo, string resTstHtmlLS)
        {
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string result = string.Empty;
            GetGlobalVars();
            tstData.SetSubGridRows(subGridInfo);
            tstData.AxActiveDc = frameNo;
            DateTime eTime1 = DateTime.Now;
            result = tstData.CallGetRapidDepFldWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, s, frameNo, rowNo, fieldName, subGridInfo, parentInfo);
            DateTime sTime2 = DateTime.Now;
            TStructDef tstObj = tstData.tstStrObj;
            if (result != "")
            {
                try
                {
                    if (tstObj.IsDcGrid(Convert.ToInt32(frameNo)))
                        tstData.JsonToArray(result, tstObj, "", "GetDepPerf");
                    else
                        tstData.JsonToArray(result, tstObj, "", "GetDep");
                }
                catch (Exception ex)
                { }
            }
            tstData.AxActiveDc = string.Empty;
            result = result + "*♠*" + tstData.GetVisibleFormatGridsHtml(tstObj, visibleDcs);
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }


        [WebMethod(EnableSession = true)]
        public string GetSearchResult(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string fldName, string fldValue, string pageNo, string pageSize, string key, string frameNo, string activerow, string parentInfo, string subGridInfo, string includeDcs, string resTstHtmlLS = "")
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();
            string result = "";
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            transid = tstData.transid;
            string errorLog = logobj.CreateLog("Get Structure object from cache for picklist.", sessionid, "PickList-" + transid, "new");
            errorLog = logobj.CreateLog("Get picklist search result.", sessionid, "GetPickListData-" + transid, "new");
            string activePRow = string.Empty;
            string activePDc = string.Empty;
            if (parentInfo != string.Empty)
            {
                int idx = parentInfo.IndexOf("~");
                if (idx != -1)
                {
                    activePDc = parentInfo.Substring(0, idx);
                    activePRow = parentInfo.Substring(idx + 1);
                }
            }
            string iXml = string.Empty;
            iXml += "<sqlresultset activerow='" + activerow + "' pdc='" + activePDc + "' prow='" + activePRow + "' frameno='" + frameNo + "' axpapp='" + project + "' value='" + fldValue + "' sessionid= '" + sessionid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' field='" + fldName + "'";
            iXml += " sqlfield='' trace='" + errorLog + "' transid='" + transid + "'  pageno='" + pageNo + "' ";
            iXml += "pagesize='" + pageSize + "'>";
            result = tstData.GetPickListResult(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, iXml, tstData.tstStrObj.structRes, fldName, frameNo, parentInfo, subGridInfo, activerow, includeDcs);
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string CallLoadDcData(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string ArrActionLog, string iXml, string dcNo, Boolean isFillGrid, string isDisable, string key, string resTstHtmlLS = "")
        {
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            string result = string.Empty;
            string tabHTML = string.Empty;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            transid = tstData.transid;
            GetGlobalVars();
            logobj.CreateActionLog(ArrActionLog, sessionid, "ActionLog", "");
            string errorLog = logobj.CreateLog("Get Structure object from cache for dc info.", sessionid, "GetTabData-" + dcNo, "new");
            //TStructDef strObj = utilObj.GetTstructDefObj(errorLog, transid);
            TStructDef strObj = tstData.tstStrObj;
            string isRapidCall = "false";
            if ((Session["RapidTsTruct"] != null && Session["RapidTsTruct"].ToString() == "true") && (HttpContext.Current.Session["AxIsPerfCode"] != null && HttpContext.Current.Session["AxIsPerfCode"].ToString().ToLower() == "true"))
                isRapidCall = "true";
            DateTime eTime1 = DateTime.Now;
            string isDraft = string.IsNullOrEmpty(tstData.TabDcStatus) ? "no" : tstData.GetCurrTabStatus(dcNo);
            Boolean designMode = false;
            if (HttpContext.Current.Session[transid + "IsDesignMode"] != null && HttpContext.Current.Session[transid + "IsDesignMode"].ToString() != string.Empty)
                designMode = Convert.ToBoolean(HttpContext.Current.Session[transid + "IsDesignMode"]);
            if (isDraft == "yes")
            {
                result = tstData.GetTabDCJson(dcNo);
            }
            else if (isRapidCall != "true" && !designMode)
                result = tstData.LoadComboValues(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, iXml, dcNo);
            else
                tstData.GetFieldValueXml(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, "-1", "LoadTab", "NG", dcNo);
            DateTime sTime2 = DateTime.Now;
            if (result != "")
                tstData.JsonToArray(result, strObj, dcNo, "LoadTab");

            //The return string will contain the MasterRow fields, json string for dependent dc's of the tab dc(excluding the json for tab dc and its sub grid if any),
            //and the html for tab dc and its sub grid html if any.    
            //AJH000011 //if (isFillGrid)
            strObj.IsFillGridCall = true;

            string temptabHTML = strObj.GetTabDcHTML(Convert.ToInt32(dcNo), tstData, isDisable);

            FDW fdwObj = new FDW();
            bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;

            if (designMode && HttpContext.Current.Session["Axp_DesignJson"] != null && HttpContext.Current.Session["Axp_DesignJson"].ToString() != "")
                strObj.axdesignJObject = tstData.tstStrObj.getDesignObjectFromJson(Session["Axp_DesignJson"].ToString());

            string tempDcDesignJson = string.Empty;
            try
            {
                //tempDcDesignJson = strObj.axdesignJson.
                Dc curDc = tstData.tstStrObj.axdesignJObject.dcs.FirstOrDefault(elm => elm.dc_id == dcNo.ToString());
                tempDcDesignJson = new JavaScriptSerializer().Serialize(curDc);
            }
            catch (Exception ex)
            {

            }
            string tabjson = "";
            bool isDcGrid = tstData.IsDcGrid(dcNo, strObj);
            if ((tstData.recordid != "0" || result != "") && isDcGrid)
            {
                tabjson = tstData.tabJson.ToString();
                tabHTML = tstData.AxDepArrays.ToString() + "*♦*" + tstData.GetMasterRowFlds() + "*♠*" + tabjson + "*♠*" + temptabHTML + "*♠*" + tempDcDesignJson;
            }
            else
                tabHTML = tstData.AxDepArrays.ToString() + "*♦*" + tstData.GetMasterRowFlds() + "*♠*" + tabjson + "*♠*" + temptabHTML + "*♠*" + tempDcDesignJson + "*♠*~false";
            //if (isFillGrid)
            strObj.IsFillGridCall = false;

            //to update design json from tab to object
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();

            string fdKey = Constants.REDISTSTRUCT;
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                fdKey = Constants.REDISTSTRUCTMOB;
            //FDW fdwObj = new FDW();
            fdwObj.SaveInRedisServer(utilObj.GetRedisServerkey(fdKey, transid), strObj, Constants.REDISTSTRUCT, schemaName);
            tstData.tstStrObj = strObj;
            Session[key] = (TStructData)tstData;

            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));
            if (_tstKeyChanged != "")
                tabHTML = _tstKeyChanged + "♠*♠" + tabHTML;
            return tabHTML;
        }

        [WebMethod(EnableSession = true)]
        public string GetFillGridData(string paramXml, ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string dcNo, string fgName, string transid, string src, string key, string exprResult, string resTstHtmlLS)
        {
            string requestProcess_logtime = string.Empty;
            ObjExecTr.SetCurrentTime();
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            string result = string.Empty;
            GetGlobalVars();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            TStructDef strObj = tstData.tstStrObj;
            string errorLog = logobj.CreateLog("Get FillGird values result.", sessionid, "GetFillGridValues-" + transid, "new");
            string iXml = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' trace='" + errorLog + "' frameno='" + dcNo + "' fgname='" + fgName + "' transid='" + transid + "'>";
            iXml += paramXml;

            result = tstData.CallMsFillGrid(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, dcNo, iXml, paramXml, fgName);
            requestProcess_logtime += result.Split('♠')[0];
            result = result.Split('♠')[1];
            if (result != string.Empty && !result.StartsWith(Constants.ERROR))
            {
                result = tstData.ConstructFillGrid(result, dcNo, fgName, strObj, tstData, src, exprResult);
            }

            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", result);
            string serverprocesstime = ObjExecTr.TotalServerElapsTime();
            result = serverprocesstime + "*♠♠*" + requestProcess_logtime + "*♠♠*" + result;
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetFastFillGridData(string dcNo, string fgName, string transid, string src, string key, string exprResult, string resTstHtmlLS)
        {
            string requestProcess_logtime = string.Empty;
            ObjExecTr.SetCurrentTime();
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string result = string.Empty;
            GetGlobalVars();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            TStructDef strObj = tstData.tstStrObj;
            string errorLog = logobj.CreateLog("Get FillGird values result.", sessionid, "GetFillGridValues-" + transid, "new");
            string iXml = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' trace='" + errorLog + "' frameno='" + dcNo + "' fgname='" + fgName + "' transid='" + transid + "'>";

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            //result = fObj.GetFastFillGridXML(utilObj.GetTstructDefObj("GetFastAjaxData", tstData.transid), fgName, dcNo);
            result = fObj.GetFastFillGridXML(tstData.tstStrObj, fgName, dcNo);
            if (result != string.Empty && !result.StartsWith(Constants.ERROR))
            {
                result = tstData.ConstructFillGrid(result, dcNo, fgName, strObj, tstData, src, exprResult);
            }

            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", result);
            string serverprocesstime = ObjExecTr.TotalServerElapsTime();
            result = serverprocesstime + "*♠*" + requestProcess_logtime + "*♠*" + result;
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string ExecuteFormatFill(string transid, string frameNo, string selection, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            GetGlobalVars();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            //TStructDef tstObj = utilObj.GetTstructDefObj("GetStructure", transid);
            TStructDef tstObj = tstData.tstStrObj;
            int dcIndNo = tstObj.dcsPositionIndex.IndexOf(frameNo.ToString());
            TStructDef.DcStruct dc = (TStructDef.DcStruct)tstObj.dcs[dcIndNo];
            string fileName = "GetFormatFillGrid" + transid;
            string dcHTML = string.Empty;
            string errorLog = logobj.CreateLog("GetFormatFillGrid.", sessionid, fileName, "new");
            dcHTML = tstObj.GetFormatHtmlForGroups(selection, tstData, "false");
            if (_tstKeyChanged != "")
                dcHTML = _tstKeyChanged + "♠*♠" + dcHTML;
            return dcHTML;
        }

        [WebMethod(EnableSession = true)]
        public string ExecuteAction(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string ArrActionLog, string rid, string transid, string frameNo, string s, string selection, string delRows, string changedRows, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            //TStructDef tstObj = utilObj.GetTstructDefObj("GetStructure", transid);
            TStructDef tstObj = tstData.tstStrObj;
            int dcIndNo = tstObj.dcsPositionIndex.IndexOf(frameNo.ToString());
            TStructDef.DcStruct dc = (TStructDef.DcStruct)tstObj.dcs[dcIndNo];
            string fileName = "GetFormatFillGrid" + transid;
            logobj.CreateActionLog(ArrActionLog, sessionid, "ActionLog", "");
            string errorLog = logobj.CreateLog("GetFormatFillGrid.", sessionid, fileName, "new");
            string actions = dc.Action;
            string dcHTML = string.Empty;
            ArrayList deletedFldArrayValues = new ArrayList();
            if (actions != "")
            {
                string[] strActions = actions.Split(',');
                string result = string.Empty;
                transid = tstData.transid;

                for (int i = 0; i < strActions.Length; i++)
                {
                    if (strActions[i].ToString() == string.Empty)
                        continue;

                    string Ixml = "";
                    Ixml = "<root axpapp='" + project + "' trace='" + errorLog + "' recordid='" + rid + "' selection='" + selection + "'  fno='" + frameNo + "' actrow='0' sessionid='" + sessionid + "'  stype='tstructs' sname='" + transid + "' actname='" + strActions[i].ToString() + "' __file='" + fileName + "' options='true'><varlist><row>";
                    Ixml += s;
                    result = tstData.CallActionWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, deletedFldArrayValues, Ixml, "", "t", delRows, changedRows, frameNo: frameNo);

                    if (result != "")
                        tstData.JsonToArray(result, tstObj, frameNo, "ExecAction");
                    tstData.oldRowNos.Clear();
                    tstData.newRowNos.Clear();
                }
                dcHTML = tstData.tabJson.ToString() + "*♠*" + tstObj.GetTabDcHTML(Convert.ToInt32(frameNo), tstData, "false");
            }
            if (_tstKeyChanged != "")
                dcHTML = _tstKeyChanged + "♠*♠" + dcHTML;
            return dcHTML;
        }

        [WebMethod(EnableSession = true)]
        public string DoGetFillGrid(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string frameNo, string fgName, string s, string key, string resTstHtmlLS)
        {
            string requestProcess_logtime = string.Empty;
            ObjExecTr.SetCurrentTime();
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();

            string dcHTML = string.Empty;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            transid = tstData.transid;

            string fileName = "DoFillGrid-" + transid;
            string errorLog = logobj.CreateLog("GetFillGridValues.", sessionid, fileName, "new");
            string Ixml = "";
            Ixml = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' frameno='" + frameNo + "' fgname='" + fgName + "' transid='" + transid + "'>";
            Ixml += s;

            TStructDef strObj = tstData.tstStrObj;
            string result = string.Empty;
            DateTime eTime1 = DateTime.Now;
            result = tstData.CallGetFillGridWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, frameNo, Ixml, fgName);
            requestProcess_logtime += result.Split('♠')[0];
            result = result.Split('♠')[1];
            DateTime sTime2 = DateTime.Now;
            if (result != "")
                tstData.JsonToArray(result, strObj, frameNo, "FillGridMS");

            strObj.IsFillGridCall = true;
            if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True" && Session["AxInlineGridEdit"].ToString() == "false")
            {
                dcHTML = result;// + "*♠*" + strObj.GetTabDcHTML(Convert.ToInt32(frameNo), tstData, "false");
            }
            else
                dcHTML = tstData.tabJson.ToString() + "*♠*" + strObj.GetTabDcHTML(Convert.ToInt32(frameNo), tstData, "false");
            strObj.IsFillGridCall = false;
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));

            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", dcHTML);
            string serverprocesstime = ObjExecTr.TotalServerElapsTime();
            dcHTML = serverprocesstime + "*♠♠*" + requestProcess_logtime + "*♠♠*" + dcHTML;
            if (_tstKeyChanged != "")
                dcHTML = _tstKeyChanged + "♠*♠" + dcHTML;
            return dcHTML;
        }

        [WebMethod(EnableSession = true)]

        public string DoGetFillGridNonMS(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string frameNo, string fgName, string key, string resTstHtmlLS)
        {
            string requestProcess_logtime = string.Empty;
            ObjExecTr.SetCurrentTime();
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string fileName = "DoFillGridAutoValues" + tstData.transid;
            string errorLog = logobj.CreateLog("DoFillGridValues.", sessionid, fileName, "new");
            string iXml = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' frameno='" + frameNo + "' fgname='" + fgName + "' transid='" + tstData.transid + "'><GridList></GridList>";
            string result = string.Empty;
            transid = tstData.transid;
            DateTime eTime1 = DateTime.Now;
            result = tstData.CallGetFillGridWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, frameNo, iXml, fgName);
            requestProcess_logtime += result.Split('♠')[0];
            result = result.Split('♠')[1];
            DateTime sTime2 = DateTime.Now;
            TStructDef strObj = tstData.tstStrObj;
            if (result != "")
                tstData.JsonToArray(result, strObj, frameNo, "FillGridNMS");
            strObj.IsFillGridCall = true;
            if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True" && Session["AxInlineGridEdit"].ToString() == "false")
            {
                //do nothing
            }
            else
                result = tstData.tabJson.ToString() + "*♠*" + strObj.GetTabDcHTML(Convert.ToInt32(frameNo), tstData, "false");
            strObj.IsFillGridCall = false;
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));

            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", result);
            string serverprocesstime = ObjExecTr.TotalServerElapsTime();
            result = serverprocesstime + "*♠♠*" + requestProcess_logtime + "*♠♠*" + result;
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetSubGridDropdown(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string frameNo, string key, string recordid, int parRowNo, string parDcNo, int popRowNo, string resTstHtmlLS)
        {
            string result = string.Empty;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            transid = tstData.transid;
            string errorLog = logobj.CreateLog("Calling GetSubGrid dropdown.", sessionid, "GetSubGridDDL", "new");
            string iXml = "<sqlresultset axpapp='" + project + "' transid='" + tstData.transid + "' recordid='" + recordid + "' dcname='dc" + frameNo + "' sessionid='" + sessionid + "' trace='" + errorLog + "' activerow='1' frameno ='" + frameNo + "' pardc='" + parDcNo + "' pdc='" + parDcNo + "' prow='" + parRowNo + "'>";
            result = tstData.GetSubGridCombos(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, iXml, frameNo, parDcNo, popRowNo, parRowNo);
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }



        //[WebMethod()]
        //public string SaveTstXml(string message)
        //{
        //    string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();

        //    return "Hi" + message;
        //}





        [WebMethod(EnableSession = true)]
        public string RefreshDc(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string frameNo, string key, string recordid, string includeDcs, string resTstHtmlLS)
        {
            string result = string.Empty;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            transid = tstData.transid;
            string errorLog = logobj.CreateLog("Calling Refresh Dc.", sessionid, "RefreshDc-" + frameNo, "new");
            string iXml = "<sqlresultset axpapp='" + project + "' transid='" + tstData.transid + "' recordid='" + recordid + "' dcname='dc" + frameNo + "' sessionid='" + sessionid + "' trace='" + errorLog + "' >";
            result = tstData.CallRefreshDc(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, iXml, frameNo, includeDcs);
            tstData.JsonToArray(result, tstData.tstStrObj, "", "RefreshDc");
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string ProcessWFActionChanges(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string files, string rid, string inputXml, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            inputXml = GetTraceString(inputXml);
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string result = string.Empty;
            if (tstData != null)
            {
                result = tstData.CallWorkFlowActionWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, files, rid, inputXml);
            }
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string ProcessWFAction(string s)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string news = GetTraceString(s);
            if (news != string.Empty) s = news;
            string result = null;
            s = s + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            try
            {
                XElement root = XElement.Parse(s);
                string _transId = (root.Attribute("transid") != null) ? root.Attribute("transid").Value : string.Empty;
                if (HttpContext.Current.Session["TstDataValidateWFAction-" + _transId] != null && HttpContext.Current.Session["TstDataValidateWFAction-" + _transId].ToString() != string.Empty)
                {
                    string formval = HttpContext.Current.Session["TstDataValidateWFAction-" + _transId].ToString();
                    s = s.Replace("<root ", "<root formval='" + formval + "' ");
                }
            }
            catch (Exception) { }
            result = asbExt.CallWorkFlowActionWS("", s);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetDependParamsValues(string s, string IVkey)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            IviewData objIview = new IviewData();
            string iStructure = string.Empty;
            try
            {
                objIview = (IviewData)HttpContext.Current.Session[IVkey];
                if (objIview == null)
                    return Constants.DUPLICATESESS;
                iStructure = objIview.StructureXml;
            }
            catch (Exception ex)
            {
                iStructure = string.Empty;
            }
            string news = GetTraceString(s);
            if (news != string.Empty) s = news;
            s = s + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            s = s.Replace("&amp;grave;", "~");
            string result = null;
            //Call service
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            result = objWebServiceExt.CallGetDependParamsValuesWS(transid, s, iStructure, objIview.WebServiceTimeout);
            UpdateFilterParamXML(result, objIview.IName);
            return result;
        }

        private void UpdateFilterParamXML(string result, string iName)
        {
            Dictionary<string, string> CurDepFields = new Dictionary<string, string>();
            Dictionary<string, string> PreDepFields = new Dictionary<string, string>();

            try
            {
                if (Session["FilterParamResXML" + iName] != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(result);
                    string[] depParams = xmlDoc.DocumentElement.Attributes["depparams"].Value.ToString().Split(',');

                    foreach (var depParam in depParams)
                    {
                        XmlNode field = xmlDoc.SelectSingleNode("//" + depParam + "/response");
                        if (field != null)
                            CurDepFields.Add(depParam, field.InnerXml);
                    }
                    if (Session["UpdateParamsCollection"] == null)
                    {
                        Session["UpdateParamsCollection"] = CurDepFields;
                    }
                    else
                    {
                        PreDepFields = (Dictionary<string, string>)Session["UpdateParamsCollection"];
                        //PreDepFields.Union(CurDepFields); ;
                        foreach (KeyValuePair<string, string> kvp in CurDepFields)
                        {
                            PreDepFields[kvp.Key] = kvp.Value;
                        }
                        Session["UpdateParamsCollection"] = PreDepFields;
                    }
                }
            }
            catch (Exception ex)
            {
                result = "error:" + ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public string ClearCache()
        {
            string result = string.Empty;
            result = utilObj.ClearApplicationCache();
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetAddGroupsHtml(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string dcNo, string transid, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            //TStructDef strObj = utilObj.GetTstructDefObj("GetAddGroupHtml", transid);
            TStructDef strObj = tstData.tstStrObj;
            int dcIndNo = strObj.dcsPositionIndex.IndexOf(dcNo.ToString());
            TStructDef.DcStruct dc = (TStructDef.DcStruct)strObj.dcs[dcIndNo];
            string fgHTML = string.Empty;
            tstData.GetFieldValueXml(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, "-1", "false", "ALL", "");
            string mapCol = string.Empty;

            if (dc.Action != "")
            {
                string result = strObj.ExecuteSql(dc.Sql, tstData, Convert.ToInt32(dcNo));
                fgHTML = tstData.ConstructFormatFillGrid(result, strObj, dcNo, "fill");
            }
            else if (dc.KeyColValues.Count > 0 || dc.Sql != "")
            {
                ArrayList groups = new ArrayList();
                //int dcIndNo = strObj.dcsPositionIndex.IndexOf(dcNo.ToString());
                groups = strObj.GetGroups(dcIndNo, Convert.ToInt32(dcNo), tstData);
                string colWidth = strObj.GetColumnWidth(dcNo, dc.KeyColumn);

                if (dc.MapParentCol != string.Empty)
                    mapCol = dc.MapParentCol;
                else
                    mapCol = dc.KeyColumn;

                string headRow = "<th>" + dc.KeyColumn + "</th>";
                string rowHtml = "<tr>";
                for (int i = 0; i < groups.Count; i++)
                {
                    if (tstData.IsDupKeyColValue(mapCol, dcNo, groups[i].ToString()))
                    {
                        rowHtml += "<tr><td width='15'><span class=tem1><input class='fgChk' type=checkbox disabled='disabled' name='chkItem' " + i + "  value=\"" + groups[i].ToString() + "\" onclick='javascript:ChkHdrCheckbox();' >";
                    }
                    else
                        rowHtml += "<tr><td width='15'><span class=tem1><input class='fgChk' type=checkbox name='chkItem' " + i + "  value=\"" + groups[i].ToString() + "\" onclick='javascript:ChkHdrCheckbox();' >";

                    rowHtml += "</span></td><td class='fgData'>" + groups[i].ToString() + "</td></tr>";
                }
                fgHTML = "<div style=\"width:100%;max-height:300px;overflow-x:auto;overflow-y:scroll;padding-top:10px\"><table id='tblFillGrid" + dcNo + "' class='gridData'><thead><tr><th width=\"15\"><input type=checkbox name='chkall' class='fgHdrChk' id='chkall' onclick=\"javascript:CheckAll(this);\" ></th>" + headRow + "</tr></thead><tbody>" + rowHtml + "</tbody></table>";
                fgHTML += "<div style='text-align:center;'><input type='button' class='button' value='Ok' onclick=\"javascript:ProcessAddGroup('" + dcNo + "')\"/></div></div>";
            }
            if (_tstKeyChanged != "")
                fgHTML = _tstKeyChanged + "♠*♠" + fgHTML;
            return fgHTML;
        }

        [WebMethod(EnableSession = true)]
        public string GetNewGroupsHtml(int dcNo, string transid, string selectedGroups, string lastRowNo, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            //TStructDef strObj = utilObj.GetTstructDefObj("GetAddGroupHtml", transid);
            TStructDef strObj = tstData.tstStrObj;
            ArrayList groups = new ArrayList();
            string[] strGroups = selectedGroups.Split('¿');
            for (int i = 0; i < strGroups.Length; i++)
            {
                groups.Add(strGroups[i].ToString());
            }
            int dcIndNo = strObj.dcsPositionIndex.IndexOf(dcNo.ToString());
            string grpHtml = strObj.GetGroupRowHtml(dcIndNo, dcNo, groups, lastRowNo, tstData);
            if (_tstKeyChanged != "")
                grpHtml = _tstKeyChanged + "♠*♠" + grpHtml;
            return grpHtml;
        }

        [WebMethod(EnableSession = true)]
        public void DeleteTstDataObj(string key)
        {
            HttpContext.Current.Session.Remove(key);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        [WebMethod(EnableSession = true)]
        public void DeleteIviewDataObj(string key)
        {
            HttpContext.Current.Session.Remove(key);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [WebMethod(EnableSession = true)]
        public string GetSessionValue(string key)
        {
            string returnString = string.Empty;
            try
            {
                returnString = HttpContext.Current.Session[key].ToString();
            }
            catch (Exception ex) { }
            return returnString;
        }

        [WebMethod(EnableSession = true)]
        public string CreateFastReportDoc(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string tid, string selectedDoc, string rid, string key, string resTstHtmlLS)
        {
            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string docName = rid + selectedDoc;
            string iXml = string.Empty; string errorLog = string.Empty;
            errorLog = logobj.CreateLog("Calling CreateFastReport", sessionid, "CreateFastReportWord-" + tid, "new");
            iXml = "<root scriptspath='" + utilObj.ScriptsPath + "' axpapp='" + project + "' sessionid='" + sessionid + "' formname='" + selectedDoc + "' filename='" + docName + "' transid='" + tid + "' recordid='" + rid + "' trace='" + errorLog + "'><varlist><row>";

            tstData.GetFieldValueXml(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, "-1", "false", "ALL", "");
            iXml += tstData.fldValueXml + tstData.memVarsData + "</row></varlist>";

            //iXml = iXml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            string result = string.Empty;
            result = tstData.CallCreateFastPDFWS(iXml);
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string AddPrintDoc(string tid, string selectedDoc, string rid, string key, string docType, string resTstHtmlLS)
        {
            string result = string.Empty;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            bool tidvalid = utilObj.IsTransIdValid(tid);
            bool ridvalid = utilObj.IsNumber(rid);
            bool docnamevalid = utilObj.IsDocName(selectedDoc);
            if (!tidvalid || !ridvalid || !docnamevalid)
            {
                string errMsg = string.Empty;
                if (!tidvalid)
                    errMsg = "Invalid Transaction Id";
                else if (!ridvalid)
                    errMsg = "Invalid Record Id";
                else if (!docnamevalid)
                    errMsg = "Invalid Document Name";

                return utilObj.ErrorMsgToJson(errMsg);
            }
            else
            {
                GetGlobalVars();
                string errorLog = string.Empty;

                errorLog = logobj.CreateLog("Adding Print tasks", sessionid, "InsertPrintJob-" + tid, "new");
                string iXml = string.Empty;

                string docName = utilObj.GetDocName(selectedDoc, tid, key);
                string[] docNameStr = docName.Split('♣');
                string printDocName = docNameStr[0].ToString().Substring(0, docNameStr[0].ToString().IndexOf("."));
                string type = string.Empty;
                if (docType == "doc")
                    type = "A";
                else
                    type = "F";

                iXml = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' trace='" + errorLog + "' transid='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                //The print doc will be stored as 'print doc name + "_" + axprintfield value + "_" + transid ' and will be stored at 'axpert' folder in the scripts path.
                iXml += "<sql> Insert into Axprinting values('" + tid + "','" + rid + "','" + selectedDoc + "','" + utilObj.ScriptsPath + "axpert\\" + sessionid + "','" + printDocName + "','InProgress','" + type + "')</sql>";
                iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                string ires = string.Empty;
                ires = asbExt.CallGetChoiceWS(tid, iXml);
                ArrayList printDocs;
                if (ires == "done")
                {
                    if (Session["printingDocs"] != null)
                        printDocs = (ArrayList)Session["printingDocs"];
                    else
                        printDocs = new ArrayList();

                    //The file name format to store in session - "filename_axprintfile/datetime_tid♣tstcaption¿requested on~status "
                    printDocs.Add(docName + "~" + Constants.PROGRESS);
                    Session["printingDocs"] = printDocs;
                }
                if (_tstKeyChanged != "")
                    result = _tstKeyChanged + "♠*♠" + result;
                return result;
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetPrintDocsHtml()
        {
            ArrayList printDocs;
            string result = string.Empty;
            string res = utilObj.GetPrintDocStatus();
            if (res == utilObj.SESSTIMEOUT)
                return res;
            printDocs = utilObj.GetDocsFromSession();
            result = utilObj.GetPrintCount(printDocs) + "♣" + utilObj.GetPrintDocsHTML(printDocs);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string RefreshPrintCount()
        {
            ArrayList printDocs;
            string result = string.Empty;
            printDocs = utilObj.GetDocsFromSession();
            result = utilObj.GetPrintCount(printDocs);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string RemoveDoc(string fileName)
        {
            string result = string.Empty;
            ArrayList printDocs = utilObj.GetDocsFromSession();
            string status = utilObj.DeleteDocFromSession(fileName, printDocs);
            if (status == utilObj.SESSTIMEOUT)
                return status;
            GetGlobalVars();
            string tid = fileName.Substring(fileName.LastIndexOf("_") + 1, fileName.IndexOf(".") - (fileName.LastIndexOf("_") + 1));
            string errorLog = logobj.CreateLog("Get choices to delete print doc", sessionid, "DeletePrintDoc-" + tid, "new");

            bool tidvalid = utilObj.IsTransIdValid(tid);
            bool filenamevalid = utilObj.IsDocName(fileName);
            if ((!tidvalid) || (!filenamevalid))
            {
                string errmsg = string.Empty;
                if (!tidvalid)
                    errmsg = "Invalid TransistationID ";
                else if (!filenamevalid)
                    errmsg = "Invalid FileName";
                return utilObj.ErrorMsgToJson(errmsg);
            }


            else
            {
                string iXml = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' trace='" + errorLog + "' transid='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                //The print doc will be stored as 'print doc name + "_" + axprintfield value + "_" + transid ' and will be stored at 'axpert' folder in the scripts path.
                iXml += "<sql> delete from Axprinting where transid='" + tid + "' and filename='" + fileName + "'</sql>";
                iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                //The result from CallGetChoice is ignored since it deletes if records are aavailable.
                result = asbExt.CallGetChoiceWS(tid, iXml);
                result = utilObj.GetPrintCount(printDocs);
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public ArrayList GetPrintDocs(string tid)
        {
            GetGlobalVars();
            ArrayList printDocs = new ArrayList();
            string result = string.Empty;
            string errorLog = string.Empty;
            string newTid = null;
            newTid = "t" + tid.ToLower();

            string dbTimesql = null;
            errorLog = logobj.CreateLog("Get Print Docs", sessionid, "GetPrintDocs-" + tid, "new");
            //dbTimesql = "<sqlresultset axpapp='" + project + "' sessionid= '" + sessionid + "' trace='" + errorLog + "'><sql>select distinct caption,name,printdoctype from prints where lower(sname) =  '" + newTid + "'  and blobno=1 order by caption</sql>";
            //dbTimesql += Session["axApps"].ToString() + Application["axProps"].ToString() + "</sqlresultset>";
            string dbTimeres = null;

            dbTimesql = "<root sname='" + tid + "' axpapp='" + project + "' sessionid='" + sessionid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' stype='tstructs' trace='" + errorLog + "'>";
            dbTimesql += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

            dbTimeres = asbExt.CallGetPrintDocList(tid, dbTimesql);
            dbTimeres.ToUpper();
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlNodes = default(XmlNodeList);
            ArrayList arrDDlItems = new ArrayList();
            printDocs.Add("-- Select --♣-- Select --");
            xmlDoc.LoadXml(dbTimeres.ToString());
            xmlNodes = xmlDoc.SelectNodes("//root");

            foreach (XmlNode productNode in xmlNodes[0])
            {
                if (productNode.Name != "transid")
                {
                    if (productNode.Attributes["source"] != null)
                        printDocs.Add(productNode.Attributes["source"].Value + "♣" + productNode.ChildNodes[0].InnerText);
                }
            }
            return printDocs;

        }

        [WebMethod(EnableSession = true)]
        public string UpdateDocStatus()
        {
            return utilObj.GetPrintDocStatus();
        }

        [WebMethod(EnableSession = true)]
        public string UpdateTraceStatus(string status)
        {
            HttpContext.Current.Session["AxTrace"] = status;
            return "done";
        }

        [WebMethod(EnableSession = true)]
        public string AppendRowToDataObj(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string ArrActionLog, string key, string resTstHtmlLS)
        {
            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            string result = string.Empty;
            GetGlobalVars();
            logobj.CreateActionLog(ArrActionLog, sessionid, "ActionLog", "");
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            tstData.GetFieldValueXml(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, "-1", "UpdateNewRow", "ALL", "");
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        private void GetGlobalVars()
        {
            project = HttpContext.Current.Session["project"].ToString();
            sessionid = HttpContext.Current.Session["nsessionid"].ToString();
            user = HttpContext.Current.Session["user"].ToString();
            AxRole = HttpContext.Current.Session["AxRole"].ToString();
        }

        #region Save As Draft
        [WebMethod(EnableSession = true)]
        public string SaveAsDraft(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string transid, string key, string tabDCStatusStr, string resTstHtmlLS)
        {
            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();
            string result = string.Empty;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            tstData.UpdateDataList(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray);
            tstData.TabDcStatus = tabDCStatusStr;
            int fldIdx = tstData.tstStrObj.GetFieldIndex("axp_draft_name");
            string custVal = string.Empty;
            if (custVal == string.Empty)
                custVal = tstData.tstStrObj.tstCaption;

            //custVal = custVal.Replace('-', '♠');
            try
            {
                Session["DraftHTML"] = Constants.DRAFT_REFRESH;
                if (!utilObj.SaveAsDraft(transid, key, user, custVal))
                    result = "error:";
            }
            catch (Exception ex)
            {
                result = "error:" + ex.Message;
            }
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string SaveAsDraftNew(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string transid, string key, string tabDCStatusStr, string resTstHtmlLS)
        {
            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();
            string result = string.Empty;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            tstData.UpdateDataList(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray);
            tstData.TabDcStatus = tabDCStatusStr;
            int fldIdx = tstData.tstStrObj.GetFieldIndex("axp_draft_name");
            string custVal = string.Empty;
            if (custVal == string.Empty)
                custVal = tstData.tstStrObj.tstCaption;

            //custVal = custVal.Replace('-', '♠');
            try
            {
                Session["DraftHTML"] = Constants.DRAFT_REFRESH;
                if (!utilObj.SaveAsDraftNew(transid, key, user, custVal))
                    result = "error:Error while save draft, please try again.";
            }
            catch (Exception ex)
            {
                result = "error:" + ex.Message;
            }
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetDraftsFromRedis(string transid, string caption)
        {
            string drafts = string.Empty;
            string userId = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
            {
                userId = HttpContext.Current.Session["username"].ToString();
                try
                {
                    drafts = utilObj.GetDraftsMarkUp(transid, userId, caption);
                }
                catch (Exception ex)
                {
                    drafts = Constants.EX_DRAFT_HTML;
                }
            }
            else
            {
                drafts = Constants.EX_DRAFT_HTML;
            }
            return drafts;
        }

        [WebMethod(EnableSession = true)]
        public string GetDraftsFromRedisNew(string transid, string caption)
        {
            string drafts = string.Empty;
            string userId = string.Empty;
            if (HttpContext.Current.Session["username"] != null)
            {
                userId = HttpContext.Current.Session["username"].ToString();
                try
                {
                    drafts = utilObj.GetDraftsMarkUpNew(transid, userId, caption);
                }
                catch (Exception ex)
                {
                    drafts = Constants.EX_DRAFT_HTML;
                }
            }
            else
            {
                drafts = Constants.EX_DRAFT_HTML;
            }
            return drafts;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteDraft(string draftId)
        {
            GetGlobalVars();
            string result = string.Empty;
            result = utilObj.DeleteDraft(draftId, user);
            return result;
        }
        #endregion

        //AddToDataCache
        [WebMethod(EnableSession = true)]
        public string AddToDataCache(string html, string key, string jsArrays, string recId, string resTstHtmlLS)
        {
            GetGlobalVars();
            string result = string.Empty;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            tstData.recordid = recId;
            tstData.recordID = recId;
            tstData.ires = string.Empty;
            tstData.result = string.Empty;
            tstData.tabJson = new StringBuilder();
            HttpContext.Current.Session[key] = tstData;
            string dirPath = utilObj.CachePath + "datacache\\" + tstData.transid + "\\" + recId;
            try
            {
                utilObj.SaveInDataCache(tstData.transid, key);
                utilObj.WriteToFile(dirPath, recId + "-html", html);
                utilObj.WriteToFile(dirPath, recId + "-js", jsArrays);
                result = recId;
            }
            catch (Exception ex)
            {
                result = "error:" + ex.Message;
                logobj.CreateLog(ex.StackTrace.ToString(), sessionid, "Exception-SaveHtml" + transid, "");
            }
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string RefreshField(string frameNo, string rowNo, string fieldName)
        {
            string result = string.Empty;
            //Needs to be implemented.
            return result;
        }
        #region TStruct Navigation
        [WebMethod(EnableSession = true)]
        public string GetRecord(string nxtprv, string recordId, string transactionId, string columnName, int dataRowIndex, bool isParentIview)
        {
            return utilObj.GetNavigationRecord(nxtprv, recordId, transactionId, columnName, dataRowIndex, isParentIview);
        }

        [WebMethod(EnableSession = true)]
        public bool SessionHasValue(string transactionId, int dataRowIndex, string columnName, bool isParentIview)
        {
            if (isParentIview)
            {
                if (dataRowIndex >= 0 && !string.IsNullOrEmpty(columnName) && Session["iNavigationInfoTable"] != null)
                    return true;
                else
                    return false;
            }
            else
            {
                if (dataRowIndex >= 0 && !string.IsNullOrEmpty(columnName) && columnName != "recordid" && Session["navigationInfoTable"] != null)
                    return true;
                else if (Session["lstRecordIds"] != null && Session["recordTransId"] != null && Session["recordTransId"].ToString() == transactionId)
                    return true;
                else
                    return false;
            }
        }

        [WebMethod(EnableSession = true)]
        public void ClearNavigationSession()
        {
            utilObj.ClearSession();
        }
        #endregion
        #region Back Forward Button
        [WebMethod(EnableSession = true)]
        public string NavigateBackForwardButton(string buttonName)
        {
            if ((buttonName.GetType() != typeof(string)) || (!(buttonName == "back" || buttonName == "forward")))
                return String.Empty;

            string rul = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            //if (!(rul.Contains("iview.aspx") || rul.Contains("tstructdesign.aspx") || rul.Contains("listiview.aspx") || rul.Contains("ivtoivload.aspx") || rul.Contains("tstruct.aspx") || rul.Contains("ivtstload.aspx") || rul.Contains("err.aspx") || rul.Contains("cpwd.aspx") || rul.Contains("ParamsTstruct.aspx")))
            //return string.Empty;

            utilObj.ClearSession();
            int urlIndex = -1;
            if (Session["allUrls"] != null)
                allUrls = (List<string>)Session["allUrls"];

            if (Session["urlIndex"] != null)
                urlIndex = (int)Session["urlIndex"];

            bool fromHyperLink = false;
            if (HttpContext.Current.Session["AxFromHypLink"] != null)
            {
                if (HttpContext.Current.Session["AxFromHypLink"].ToString() == "true")
                {
                    fromHyperLink = true;
                    HttpContext.Current.Session["AxFromHypLink"] = "false";
                }
            }
            Session["enableBackButton"] = true;
            Session["enableForwardButton"] = true;
            Session["backForwBtnPressed"] = true;

            if (urlIndex != -1 && allUrls.Count > urlIndex)
            {
                string newUrl = string.Empty;
                //If the url is a iview with hyperlink urls
                if (allUrls[urlIndex].ToString().IndexOf('♣') != -1)
                {
                    newUrl = GetPrevNavUrl(urlIndex);
                    Session["allUrls"] = allUrls;
                }
                else
                {
                    if (buttonName == "back" && !fromHyperLink)
                        urlIndex = urlIndex - 1;
                    else if (buttonName == "forward")
                        urlIndex = urlIndex + 1;
                    if (urlIndex != -1)
                    {
                        if (allUrls[urlIndex].ToString().IndexOf('♣') != -1)
                        {
                            newUrl = GetPrevNavUrl1(urlIndex);
                            if (allUrls[urlIndex + 1].ToString().Contains("tstructdesign.aspx") || allUrls[urlIndex + 1].ToString().Contains("listIview.aspx") || allUrls[urlIndex + 1].ToString().Contains("axp_IsSaveUrl=true") || allUrls[urlIndex + 1].ToString().Contains("tstruct.aspx"))
                            {
                                allUrls.RemoveAt(urlIndex + 1);
                                Session["allUrls"] = allUrls;
                            }
                        }
                        else
                        {
                            if (allUrls.Count() <= urlIndex + 1 && fromHyperLink)
                            {
                                urlIndex = urlIndex - 1;
                                newUrl = allUrls[urlIndex].ToString();
                                allUrls.RemoveAt(urlIndex);
                            }
                            else
                            {
                                newUrl = allUrls[urlIndex].ToString();
                                allUrls.RemoveAt(urlIndex + 1);
                            }
                            Session["allUrls"] = allUrls;
                        }
                        Session["urlIndex"] = urlIndex;
                    }
                }
                return newUrl;
            }
            return string.Empty;
        }

        private string GetPrevNavUrl1(int urlIndex)
        {
            string newUrl = string.Empty;
            string[] newUrls = allUrls[urlIndex].Split('♣');
            newUrl = newUrls[newUrls.Length - 1].ToString();
            return newUrl;
        }

        private string GetPrevNavUrl(int urlIndex)
        {
            string newUrl = string.Empty;
            string[] newUrls = allUrls[urlIndex].Split('♣');
            string updUrl = string.Empty;

            for (int i = 0; i < newUrls.Length - 1; i++)
            {
                if (i == newUrls.Length - 2)
                {
                    newUrl = newUrls[i].ToString();

                }
                if (updUrl == string.Empty)
                    updUrl = newUrls[i].ToString();
                else
                    updUrl += "♣" + newUrls[i].ToString();

            }
            allUrls[urlIndex] = updUrl;
            return newUrl;
        }
        #endregion


        [WebMethod(EnableSession = true)]
        public void CreateTimeLog(string clientStartTime, string clientTime1, string clientTime2, string asbTot, string asbDb, string serviceName, string dataKey, string resTstHtmlLS)
        {
            GetGlobalVars();
            string path = utilObj.ScriptsPath + "\\log\\" + sessionid;
            string serverTime = string.Empty;
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(dataKey.Split('_')[0]);
                dataKey = utilObj.GetReGenTstDataObj(dataKey, resTstHtmlLS);
            }
            TStructData tstData = (TStructData)Session[dataKey];
            if (tstData != null)
                serverTime = tstData.strServerTime;
            //Sid,tid,rid,Service,start time,cl1,wt1,asb,wt2 ,cl2, ASBTot, ASBDB
            string logData = sessionid + "," + tstData.transID + "," + tstData.recordid + "," + serviceName + "," + clientStartTime + "," + clientTime1 + "," + serverTime + "," + clientTime2 + "," + asbTot + "," + asbDb;

            utilObj.WriteToFileExt(path, "Timetaken", logData);
        }

        // iview caching : on unload deletes all existing cached iview files.
        [WebMethod(EnableSession = true)]
        public void DeleteIviewCacheFiles(string iName)
        {
            if (Session["project"] == null) return;
            string ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            sessionid = HttpContext.Current.Session["nsessionid"].ToString();
            string path = ScriptsPath + "axpert\\" + sessionid + "\\" + iName;
            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists)
            {
                try
                {
                    //utilObj.DeleteIviewFiles(path);
                }
                catch (Exception ex)
                {
                    logobj.CreateLog("Exception while Creating session id folder in DeleteIviewCacheFiles" + ex.StackTrace + "------Path-" + ScriptsPath, HttpContext.Current.Session["nsessionid"].ToString(), "Exception", "new");
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public string TestMailSettings(string smptpHost, string smptpPort, string smptpUser, string smptpPassword)
        {
            string errorLog = logobj.CreateLog("Smptp Mail Settings", sessionid, "SmptpMailsettings", "new");
            string result = string.Empty;
            try
            {
                string iXml = string.Empty;
                string tstType = "tstruct";
                string tstName = "Axpert Properties";
                string authMail = "false";
                string userMailfrom = string.Empty;
                if (HttpContext.Current.Session["AxEmailId"] != null && HttpContext.Current.Session["AxEmailId"].ToString() != string.Empty)
                    userMailfrom = HttpContext.Current.Session["AxEmailId"].ToString();
                if (userMailfrom != string.Empty && userMailfrom != "")
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(HttpContext.Current.Session["axGlobalVars"].ToString());
                    if (xDoc != null)
                    {
                        var auth = xDoc.SelectSingleNode("transparent_mailauth");
                        if (auth != null)
                        {

                            authMail = auth.Value.ToString();

                        }

                    }
                    iXml = "<root axpapp='" + HttpContext.Current.Session["project"] + "' sessionid='" + Session["nsessionid"].ToString() + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' host='" + smptpHost + "' portno='" + smptpPort + "' userid='" + smptpUser + "' pwd='" + smptpPassword + "' stype='" + tstType + "' sname='" + tstName + "' mailfrom='" + userMailfrom + "' transparent_mailauth='" + authMail + "'> ";
                    iXml += "</root>";

                    result = asbExt.CallTestMailSettings(iXml);
                }
                else result = "<error>Logged in user email is not set</error>";
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public void UnlockTStructRecord(string transId, string recordId, bool unlockByAdmin = false)
        {
            GetGlobalVars();
            Session["axp_lockonrecid"] = null;
            string errorLog = logobj.CreateLog("Unlocking the record", sessionid, "UnlockRecord", "new");
            string iXml = string.Empty;
            iXml = "<root axpapp='" + project + "' sessionid='" + sessionid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' transid='" + transId + "' " + (unlockByAdmin ? "unlockby='superuser'" : "") + " recordid='" + recordId + "'>";
            iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            string result = string.Empty;
            result = asbExt.CallUnlockTStructRecord(iXml);
        }

        [WebMethod(EnableSession = true)]
        public void SignOut(string keeuUrlPath = "")
        {
            try
            {
                GetGlobalVars();
            }
            catch (Exception ex)
            { }
            try
            {
                if (project != "")
                {
                    if (keeuUrlPath != "" && keeuUrlPath != "false")
                    {
                        try
                        {
                            if (keeuUrlPath == "mainnew")
                            {
                                keeuUrlPath = "";
                            }
                            utilObj.SaveKeepAliveWebDetails(keeuUrlPath);
                        }
                        catch (Exception ex) { }
                    }
                    else if (keeuUrlPath == "false")
                    {
                        if (HttpContext.Current.Session["KeepMeWebExpiry"] != null)
                            HttpContext.Current.Session.Remove("KeepMeWebExpiry");
                    }
                    CallUnlockTstructRecord();
                    string AxCloudDB = string.Empty;
                    if (Session["AxCloudDB"] != null)
                        AxCloudDB = Session["AxCloudDB"].ToString();
                    string errorLog = logobj.CreateLog("Calling Signout ws", sessionid, "Signout", "new");
                    string iXml = string.Empty;
                    string Svrlic_redis = string.Empty;
                    if (Session["Svrlic_redis"] != null)
                        Svrlic_redis = Session["Svrlic_redis"].ToString();
                    //iXml = "<root axpapp='" + project + "' sessionid='" + sessionid + "' " + Svrlic_redis + " trace='" + errorLog + "'>";
                    //if (Session["axApps"] != null)
                    //    iXml += Session["axApps"].ToString() + Application["axProps"].ToString();
                    //else if (project != "" && Application[project + "-axApps"] != null)
                    //    iXml += Application[project + "-axApps"].ToString() + Application["axProps"].ToString();
                    //iXml += "</root>";
                    utilObj.RemoveLoggedUserDetails(project, sessionid, Session["username"].ToString());
                    utilObj.DeleteKeepAliveWebKey();
                    utilObj.SaveExecutionText();
                    utilObj.DeleteUnwantedKeys();
                    utilObj.TempAttaServerFiles();
                    utilObj.ClearUserIviewData();
                    string result = string.Empty;
                    utilObj.delALLNotificiationKeyfromRedis();
                    //result = asbExt.CallLogoutWS("Signout", iXml);
                    ////utilObj.RemoveLoggedUserDetails(project, sessionid);

                    string _axApps = string.Empty;
                    string _axProps = string.Empty;
                    if (Session["axApps"] != null)
                    {
                        _axApps = Session["axApps"].ToString();
                        _axProps = Application["axProps"].ToString();
                    }
                    else if (project != "" && Application[project + "-axApps"] != null)
                    {
                        _axApps = Application[project + "-axApps"].ToString();
                        _axProps = Application["axProps"].ToString();
                    }
                    if (errorLog != "")
                        errorLog = errorLog.Replace("\\", "\\\\");
                    _axApps = _axApps.Replace(@"\", "\\\\");
                    string _UserName = string.Empty;
                    if (Session["username"] != null)
                        _UserName = Session["username"].ToString();
                    string signOutJson = "{\"logout\":{\"axpapp\":\"" + project + "\",\"username\":\"" + _UserName + "\",\"sessionid\":\"" + sessionid + "\"," + Svrlic_redis + " \"trace\":\"" + errorLog + "\"},\"axapps\":\"" + _axApps + "\",\"axprops\":\"" + _axProps + "\"}";
                    result = ALCClient.CallALCClientLogout(_UserName, signOutJson);
                    string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                    scriptsPath = scriptsPath + "Axpert\\" + sessionid;
                    if (Directory.Exists(scriptsPath) && sessionid != "")
                    {
                        try
                        {
                            Directory.Delete(scriptsPath, true);
                        }
                        catch (Exception Ex)
                        {
                            //Do nothing
                        }
                    }
                    if (AxCloudDB != string.Empty)
                        Application["AxCloudDB"] = AxCloudDB;
                }
            }
            catch (Exception ex)
            {
                utilObj.KillSession();
            }
        }

        private void CallUnlockTstructRecord()
        {
            if (Session["axp_lockonrecid"] != null && (Session["axp_isLockOnRead"] != null && Session["axp_isLockOnRead"].ToString() == "true"))
            {
                string errorLog = logobj.CreateLog("Calling Unlock Tstruct ws", sessionid, "Unlock", "new");
                try
                {
                    string[] recLockDetails = Session["axp_lockonrecid"].ToString().Split('~');
                    logobj.CreateLog("Transid=" + recLockDetails[0].ToString() + ", Recid=" + recLockDetails[1].ToString(), sessionid, "Unlock", "");
                    UnlockTStructRecord(recLockDetails[0].ToString(), recLockDetails[1].ToString());
                }
                catch (Exception ex)
                {
                    logobj.CreateLog("Exception in Unlock tstruct-" + ex.Message, sessionid, "Unlock", "");
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetRecordId(string transId, string param)
        {
            param = "<values>" + param + "</values>";
            GetGlobalVars();
            string errorLog = logobj.CreateLog("Calling get recordid ws", sessionid, "getrecid", "new");
            string iXml = string.Empty;
            iXml = "<root axpapp='" + project + "' sessionid='" + sessionid + "' trace='" + errorLog + "' transid='" + transId + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
            iXml += param + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            string result = string.Empty;

            result = asbExt.CallGetRecordIdWS(transId, iXml);
            if (result != null)
                result = result.Split('♠')[1];
            return result;
        }


        [WebMethod(EnableSession = true)]
        public void SaveIntUser(string name, string org, string email, string mob, string partner)
        {
            string result = string.Empty;
            string qry = null;
            qry = "insert into intusers values('" + name + "','" + org + "','" + email + "','" + mob + "','" + partner + "')";
            result = ads.GetChoices(qry, false);
            if (result != "done")
            {

            }

        }

        [WebMethod(EnableSession = true)]
        public string SetActionIvParamsForTstLoad(string url, string paramStr, string iviewName)
        {
            HttpContext.Current.Session["AxIvParamsForTstLoad-" + iviewName] = paramStr;
            return url;
        }

        [WebMethod(EnableSession = true)]
        public string SetActionIvParams(string url, string paramStr, string iviewName)
        {
            //HttpContext.Current.Session["AxActionParams" + iviewName] = paramStr;
            // HttpContext.Current.Session["IviewNavigationData-" + iviewName] = paramStr;
            this.SetIviewNavigationData(paramStr, iviewName);
            return url;
        }

        [WebMethod(EnableSession = true)]
        public object SetExportParams(string url, string paramStr, string iviewName)
        {
            HttpContext.Current.Session["AxIvExportParams-" + iviewName] = paramStr;
            return new { url, paramStr, iviewName };
        }

        [WebMethod(EnableSession = true)]
        public bool SetIviewNavigationData(string paramStr, string iviewName)
        {
            bool returnSuccess = false;
            string username = HttpContext.Current.Session["username"].ToString();
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            Util.Util util = new Util.Util();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
            {
                fObj = new FDR();
                HttpContext.Current.Session["FDR"] = fObj;
            }

            JObject jObj = new JObject();
            bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
            if (isRedisConnected)
            {
                try
                {
                    jObj = JObject.Parse(fObj.StringFromRedis(util.GetRedisServerkey(Constants.IviewNavigationData, iviewName), schemaName));
                }
                catch (Exception ex)
                { }
            }

            FDW objFDW = new FDW();
            if (isRedisConnected)
            {
                jObj[username] = paramStr;

                returnSuccess = objFDW.SaveInRedisServerDT(util.GetRedisServerkey(Constants.IviewNavigationData, iviewName), jObj, "", schemaName);
            }
            else
            {
                HttpContext.Current.Session["IviewNavigationData-" + iviewName] = paramStr;
                returnSuccess = true;
            }


            return returnSuccess;
        }


        [WebMethod(EnableSession = true)]
        public string GetIviewNavigationData(string iviewName)
        {
            string returnString = string.Empty;
            string username = HttpContext.Current.Session["username"].ToString();
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            Util.Util util = new Util.Util();
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            if (fObj == null)
            {
                fObj = new FDR();
                HttpContext.Current.Session["FDR"] = fObj;
            }
            bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
            if (isRedisConnected)
            {
                //returnString = fObj.StringFromRedis(util.GetRedisServerkey(Constants.IviewNavigationData, iviewName), schemaName);
                JObject jObj = new JObject();
                try
                {
                    jObj = JObject.Parse(fObj.StringFromRedis(util.GetRedisServerkey(Constants.IviewNavigationData, iviewName), schemaName));

                    returnString = jObj[username].ToString();
                }
                catch (Exception ex)
                { }
            }
            else if (HttpContext.Current.Session["IviewNavigationData-" + iviewName] != null)
            {
                returnString = HttpContext.Current.Session["IviewNavigationData-" + iviewName].ToString();
            }

            //if (returnString == string.Empty) {
            //    returnString = "{}";
            //}

            return returnString;
        }



        [WebMethod(EnableSession = true)]
        public void GetAxpString()
        {
            string result = string.Empty;
            string trace = "false";
            string errorLog = string.Empty;
            if (ConfigurationManager.AppSettings["LoginTrace"] != null)
                trace = ConfigurationManager.AppSettings["LoginTrace"].ToString().ToLower();

            errorLog = logobj.CreateLog("Calling GetAxpString-", "logAxpString", "GetAxpString", "new", trace);
            result = asbExt.CallAxpString();
            logobj.CreateLog(result, "logAxpString", "GetAxpString", "", trace);
            if (result != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(result);
                }
                catch (Exception ex)
                {
                }
                XmlNode resultNode = null;


                resultNode = xmlDoc.SelectSingleNode("//result");
                if (resultNode != null)
                {
                    foreach (XmlNode childNode in resultNode.ChildNodes)
                    {
                        if (childNode.Name == "macid")
                            Application["AxpString1"] = childNode.InnerText;
                        else if (childNode.Name == "licfile")
                            Application["AxpString2"] = childNode.InnerText;
                    }
                }

                XmlNode errNode = null;

                errNode = xmlDoc.SelectSingleNode("//e");
                if (errNode != null)
                    Application["AxpString1"] = errNode.OuterXml;
            }
        }

        public void SetServerLicense()
        {
            try
            {
                string redisIp = string.Empty;
                string redisPwd = string.Empty;
                string scrPath = string.Empty;
                if (HttpContext.Current.Application["ScriptsPath"] != null)
                    scrPath = HttpContext.Current.Application["ScriptsPath"].ToString();

                //if (ConfigurationManager.AppSettings["axpLic_RedisIp"] != null)
                //    redisIp = ConfigurationManager.AppSettings["axpLic_RedisIp"].ToString();

                //if (ConfigurationManager.AppSettings["axpLic_RedisPass"] != null)
                //    redisPwd = ConfigurationManager.AppSettings["axpLic_RedisPass"].ToString();

                if (HttpContext.Current.Session != null && HttpContext.Current.Session["axpLic_RedisIP"] != null && HttpContext.Current.Session["axpLic_RedisIP"].ToString() != "")
                {
                    redisIp = HttpContext.Current.Session["axpLic_RedisIP"].ToString();
                    if (HttpContext.Current.Session["axpLic_RedisPwd"] != null && HttpContext.Current.Session["axpLic_RedisPwd"].ToString() != "")
                        redisPwd = HttpContext.Current.Session["axpLic_RedisPwd"].ToString();
                }
                else
                {
                    string rcDetails = utilObj.GetAxpLicRedisConnDetails();
                    if (rcDetails != "")
                    {
                        redisIp = rcDetails.Split('♣')[0];
                        redisPwd = rcDetails.Split('♣')[1];
                    }
                }

                if (redisIp != string.Empty)
                {
                    string rlicConn = utils.GetServerLicDetails(redisIp, redisPwd);
                    switch (rlicConn)
                    {
                        case "notConnected":
                            logobj.CreateLog("SetServerLicense: Redis Connection details for Axpert license is not proper. Please contact your support person.", "", "SetServerLicense", "new", "true");
                            break;
                        case "keyNotMatch":
                            logobj.CreateLog("SetServerLicense: Redis IP for Axpert license should be set as 127.0.0.1. Please contact your support person.", "", "SetServerLicense", "new", "true");
                            break;
                        case "keyNotExists":
                            string[] filePaths = Directory.GetFiles(@scrPath, "*-axpertsrvr.lic");
                            if (filePaths.Count() > 0)
                            {
                                string licFileName = filePaths[0].Split('\\').Last();
                                string result = string.Empty;
                                if (redisPwd != string.Empty)
                                    redisPwd = utils.EncryptPWD(redisPwd);
                                string strInpt = redisIp + "~" + redisPwd + "," + licFileName;
                                result = asbExt.CallServerLicense(strInpt);
                                if (result != "done")
                                    logobj.CreateLog("SetServerLicense: Result:" + result, "", "SetServerLicense", "new", "true");
                            }
                            else
                                logobj.CreateLog("SetServerLicense: Server license file not exists in the script folder. please activate server license or if already activated copy lic file in current script folder.", "", "SetServerLicense", "new", "true");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("SetServerLicense -" + ex.Message, "", "SetServerLicense", "new", "true");
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetParams(string iViewname)
        {
            if (iViewname != "")
            {
                WebServiceExt objWebServiceExt = new WebServiceExt();
                string fileName = "openiview-" + iViewname;
                string errLog = logobj.CreateLog("Call to GetParams Web Service", Session["nsessionid"].ToString(), fileName, "");
                string iXml = string.Empty;
                iXml = "<root name =\"" + iViewname + "\" axpapp = \"" + Session["project"].ToString() + "\" sessionid = \"" + Session["nsessionid"].ToString() + "\" appsessionkey=\"" + HttpContext.Current.Session["AppSessionKey"].ToString() + "\" username=\"" + HttpContext.Current.Session["username"].ToString() + "\" trace = \"" + errLog + "\"  >";
                iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
                string ires = string.Empty;
                IviewData objIview = new IviewData();
                objIview.WebServiceTimeout = objWebServiceExt.asbIview.Timeout;
                ires = objWebServiceExt.CallGetParamsWS(iViewname, iXml, objIview);
                if (ires != null)
                    ires = ires.Split('♠')[1];
                if (ires == "" || ires.Contains("Disconnected because you have logged into another session.") || ires.Contains("error"))
                {
                    return ires;
                }
                return ConstructParamsHtml(ires);
            }
            else
                return "invalid Iview";
            //return ires;
        }

        [WebMethod(EnableSession = true)]
        public string GetContactInfo()
        {
            string result = string.Empty;
            if (HttpContext.Current.Session["project"] != null)
            {
                result = Session["m_contactinfo"].ToString();
            }
            return result;
        }


        private string ConstructParamsHtml(string result)
        {

            StringBuilder strJsArrays = new StringBuilder();
            StringBuilder strParamDetails = new StringBuilder();
            ArrayList iviewParamValues = new ArrayList();
            ArrayList iviewParams = new ArrayList();
            bool isSqlFld = false;
            string clientCulture = null;

            IviewData objIview = new IviewData();
            string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            StringBuilder paramHtml = new StringBuilder();
            result = _xmlString + result;
            // logobj.CreateLog("Loading and setting parameters components", sid, fileName, string.Empty);
            string parameterName = string.Empty;
            string paramCaption = string.Empty;
            string paramType = string.Empty;
            string paramHidden = string.Empty;
            string paramMOE = string.Empty;
            string paramValue = string.Empty;
            string paramSql = string.Empty;
            string paramDepStr = string.Empty;
            Boolean unHideParams = false;
            string expr = string.Empty;
            string vExpr = string.Empty;
            int tabIndex = 0;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList productNodes = default(XmlNodeList);
            XmlNodeList baseDataNodes = default(XmlNodeList);
            //  StringBuilder strJsArrays = new StringBuilder();
            int iCnt = 0;
            int fldNo = 0;
            int dpCnt = 0;
            try
            {
                xmlDoc.LoadXml(result);
            }
            catch (XmlException ex)
            {
                // Response.Redirect(util.ERRPATH + ex.Message);
            }

            productNodes = xmlDoc.SelectNodes("//root");
            bool showParam = false;
            if (productNodes[0].Attributes["showparams"] != null && productNodes[0].Attributes["showparams"].Value != string.Empty)
                showParam = Convert.ToBoolean(productNodes[0].Attributes["showparams"].Value);



            string ivCaption = xmlDoc.SelectSingleNode("root").Attributes["caption"].Value;
            ivCaption = ivCaption.Replace("&&", "&");
            string lblHeading = ivCaption;
            objIview.IviewCaption = ivCaption;
            bool paramsBound = true;
            string Paramsids = string.Empty;
            //string Paramsids = "<div id='paramsValue' style='display:none'>";
            foreach (XmlNode productNode in productNodes)
            {
                baseDataNodes = productNode.ChildNodes;
                paramHtml.Append("<table id=\"ivParamTable\" style='width:100%;border-spacing:0px;padding:10px;border:0px' ><tr style=\"height:15px;\">");
                foreach (XmlNode baseDataNode in baseDataNodes)
                {
                    if (baseDataNode.Attributes["cat"].Value == "params")
                    {
                        paramValue = string.Empty;
                        if (baseDataNode.Attributes["value"] != null)
                            paramValue = baseDataNode.Attributes["value"].Value;

                        iviewParamValues.Add(paramValue);

                        foreach (XmlNode tstNode in baseDataNode)
                        {
                            if (tstNode.Name == "a0")
                            {
                                parameterName = tstNode.InnerText;
                                iviewParams.Add(parameterName);
                            }
                            else if (tstNode.Name == "a2")
                            {
                                paramCaption = tstNode.InnerText;
                                strParamDetails.Append(paramCaption + ",");
                                if (paramCaption == "axp_refresh" && objIview.iviewParams != null)
                                    objIview.iviewParams.Axp_refresh = "true";

                            }
                            else if (tstNode.Name == "a4")
                            {
                                paramType = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a6")
                            {
                                expr = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a10")
                            {
                                vExpr = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a21")
                            {
                                paramHidden = tstNode.InnerText;
                                strParamDetails.Append(paramHidden + "~");
                            }
                            else if (tstNode.Name == "a13")
                            {
                                paramMOE = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a56")
                            {
                                //pvalue = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a11")
                            {
                                paramSql = tstNode.InnerText;
                            }
                            else if (tstNode.Name == "a15")
                            {
                                foreach (XmlNode selNode in tstNode)
                                {
                                    if (selNode.Name == "s")
                                    {
                                        paramDepStr = selNode.InnerText.ToString();
                                    }
                                }
                            }
                            else if (tstNode.Name == "response")
                            {
                                ComboFill(tstNode);
                                isSqlFld = true;
                            }

                        }
                    }
                    if (paramType == "Date/Time" && clientCulture.ToLower() == "en-us")
                    {
                        paramValue = utilObj.GetClientDateString(clientCulture, paramValue);

                        if (paramMOE.ToLower() == "select" && arrFillList.Count > 0)
                        {
                            for (int y = 0; y <= arrFillList.Count - 1; y++)
                                arrFillList[y] = utilObj.GetClientDateString(clientCulture, arrFillList[y].ToString());
                        }
                    }

                    //construction of parameters.
                    strJsArrays.Append("parentArr[" + fldNo + "]='" + parameterName + "';typeArr[" + fldNo + "]='" + paramMOE + "';paramType[" + fldNo + "]='" + paramType + "';depArr[" + fldNo + "]='" + paramDepStr + "';hiddenArr[" + fldNo + "]='" + paramHidden + "';Expressions[" + fldNo + "]='" + expr + "';pCurArr[" + fldNo + "]= '" + paramValue + "';vExpressions[" + fldNo + "]='" + vExpr + "'; ");

                    //string paramsString = "parentArr[" + fldNo + "],typeArr["+ fldNo +"], ";
                    //strJsArrays.Append("saveIviewParams(typeArr[" + fldNo + "] )";
                    paramHidden = paramHidden.ToLower();
                    if (paramHidden == "false" && objIview.iviewParams != null)
                    {
                        objIview.iviewParams.IsParameterExist = true;

                        if (paramsBound && paramValue == string.Empty)
                        {
                            if (paramMOE.ToLower() == "select")
                            {
                                if (arrFillList.Count > 0 && arrFillList[0].ToString() != string.Empty)
                                    paramsBound = true;
                                else
                                    paramsBound = false;
                            }
                            else
                                paramsBound = false;
                        }

                        tabIndex = tabIndex + 1;
                        unHideParams = true;
                        string CallValidateExpr = string.Empty;
                        if (!string.IsNullOrEmpty(vExpr))
                        {
                            if (paramMOE.ToLower() == "accept")
                            {
                                if (paramType == "Date/Time")
                                    CallValidateExpr = "onChange=\"FillDependents('" + parameterName + "');\" ";
                                else
                                    CallValidateExpr = "onBlur=\"FillDependents('" + parameterName + "');\" ";

                            }
                            else if ((paramMOE.ToLower() == "select") | (paramMOE.ToLower() == "pick list"))
                            {
                                CallValidateExpr = "onChange=\"FillDependents('" + parameterName + "');\" ";
                            }
                        }
                        else
                        {
                            if (paramMOE.ToLower() == "accept")
                            {
                                CallValidateExpr = "onblur=\"FillDependents('" + parameterName + "');\" ";
                            }
                            else if ((paramMOE.ToLower() == "select") | (paramMOE.ToLower() == "pick list"))
                            {
                                CallValidateExpr = "onChange=\"FillDependents('" + parameterName + "');\" ";
                            }
                        }

                        if ((paramMOE.ToLower() == "accept") & (paramType == "Date/Time") & (!string.IsNullOrEmpty(expr)))
                        {
                            if (expr.ToLower() != "date()")
                            {
                                paramHtml.Append("<td class=paramtd1 style='width:50%'><font class=cap>" + paramCaption + "</font></td><td class=paramtd2 style='width:50%'><input type=text id='" + parameterName + "' name='" + parameterName + "' value='" + paramValue + "' class='tem Family date' style='width:80%' TabIndex='" + tabIndex + "' ></td>");
                                paramHtml.Append("</tr><tr>");
                            }
                            else
                            {
                                paramHtml.Append("<td class=paramtd1 style='width:50%'><font class=cap>" + paramCaption + "</font></td><td class=paramtd2 style='width:50%'><input type=text id='" + parameterName + "' name='" + parameterName + "' value='" + paramValue + "' class='tem Family date' style='width:80%' TabIndex='" + tabIndex + "' " + CallValidateExpr + "></td>");
                                paramHtml.Append("</tr><tr>");
                            }
                        }
                        else if ((paramMOE.ToLower() == "accept") & (paramType == "Date/Time") & (string.IsNullOrEmpty(expr)))
                        {
                            if (paramDepStr != string.Empty)
                            {
                                paramHtml.Append("<td class=paramtd1 style='width:50%'><font class=cap>" + paramCaption + "</font></td><td class=paramtd2 style='width:50%'><input type=text id='" + parameterName + "' name='" + parameterName + "' value='" + paramValue + "' class='tem Family date' style='width:80%' TabIndex='" + tabIndex + "' " + CallValidateExpr + "></td>");
                                paramHtml.Append("</tr><tr>");
                            }
                            else
                            {
                                paramHtml.Append("<td class=paramtd1 style='width:50%'><font class=cap>" + paramCaption + "</font></td><td class=paramtd2 style='width:50%'><input type=text id='" + parameterName + "' name='" + parameterName + "' value='" + paramValue + "' class='tem Family date' style='width:80%' TabIndex='" + tabIndex + "'></td>");
                                paramHtml.Append("</tr><tr>");
                            }
                        }
                        else if ((paramMOE.ToLower() == "accept") & (!string.IsNullOrEmpty(expr)))
                        {
                            paramHtml.Append("<td class=paramtd1 style='width:50%'><font class=cap>" + paramCaption + "</font></td><td class=paramtd2 style='width:50%'><input type=text id='" + parameterName + "' name='" + parameterName + "' value='" + paramValue + "' class='tem Family' style='width:80%'  TabIndex='" + tabIndex + "' " + CallValidateExpr + "></td>");
                            paramHtml.Append("</tr><tr>");
                        }
                        else if ((paramMOE.ToLower() == "accept"))
                        {
                            paramHtml.Append("<td class=paramtd1 style='width:50%'><font class=cap>" + paramCaption + "</font></td><td class=paramtd2 style='width:50%'><input type=text id='" + parameterName + "' name='" + parameterName + "' TabIndex='" + tabIndex + "' value='" + paramValue + "' style='width:80%' class='tem Family' " + CallValidateExpr + "></td>");
                            paramHtml.Append("</tr><tr>");
                        }
                        else if ((paramMOE.ToLower() == "pick list"))
                        {
                            paramHtml.Append("<td class=paramtd1 style='width:50%'><font class=cap>" + paramCaption + "</font></td><td class=paramtd2 style='width:50%'><input type=text id='" + parameterName + "' name='" + parameterName + "' value='" + paramValue + "'  class='tem Family'" + CallValidateExpr + " style='width:80%' >&nbsp;<a><img align='middle' src=\"../AxpImages/pickList.png\" id=\"img~" + parameterName + "\"  style=\"cursor:hand;vertical-align: middle;border:0px;\" /></a></td>");
                            paramHtml.Append("</tr><tr>");
                            arrFillList.Clear();
                        }
                        else if ((paramMOE.ToLower() == "select") & isSqlFld == true)
                        {
                            paramHtml.Append("<td class=paramtd1 style='width:50%'><font class=cap>" + paramCaption + "</font></td><td class=paramtd2 style='width:50%'><select TabIndex='" + tabIndex + "' id='" + parameterName + "' name='" + parameterName + "'  class='combotem Family' style='width:85%' " + CallValidateExpr + ">");
                            // onmouseover=\"showHideTooltip(this.id);\"
                            int i = 0;
                            if (arrFillList.Count == 0)
                            {
                                paramHtml.Append("<option selected> </option>");
                            }
                            for (i = 0; i <= arrFillList.Count - 1; i++)
                            {
                                if (arrFillList[i].ToString() == paramValue)
                                {
                                    paramHtml.Append("<option selected value='" + arrFillList[i].ToString() + "'>" + arrFillList[i].ToString() + "</option>");
                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        paramHtml.Append("<option value=''></option>");
                                        paramHtml.Append("<option value='" + arrFillList[i].ToString() + "'>" + arrFillList[i].ToString() + "</option>");
                                        strFillDepPName = strFillDepPName + parameterName + "¿";
                                    }
                                    else
                                    {
                                        paramHtml.Append("<option value='" + arrFillList[i].ToString() + "'>" + arrFillList[i].ToString() + "</option>");
                                    }
                                }
                            }
                            paramHtml.Append("</select></td>");
                            paramHtml.Append("</tr><tr>");
                            arrFillList.Clear();
                            isSqlFld = false;
                        }
                        else if (paramMOE.ToLower() == "select" & isSqlFld == false)
                        {
                            paramHtml.Append("<td class=paramtd1 ><font class=cap>" + paramCaption + "</font></td><td class=paramtd2><select id='" + parameterName + "' name='" + parameterName + "' TabIndex='" + tabIndex + "'  class='combotem Family' style=width:85%;  " + CallValidateExpr + "></select></td>");
                            paramHtml.Append("</tr><tr>");
                        }
                        else if (((paramMOE.ToLower() == "multi select") & (isSqlFld == true)))
                        {
                            paramHtml.Append("<td  class=paramtd1 valign=top ><font class=cap>" + paramCaption + "</font></td><td class=paramtd2><select id='" + parameterName + "' name='" + parameterName + "' TabIndex='" + tabIndex + "' class='combotem Family' style=width:85%;height:100px;  multiple=\"multiple\"  " + CallValidateExpr + ">");
                            int i = 0;
                            for (i = 0; i <= arrFillList.Count - 1; i++)
                            {
                                if (arrFillList[i].ToString() == paramValue)
                                {
                                    paramHtml.Append("<option selected value='" + arrFillList[i].ToString() + "'>" + arrFillList[i].ToString() + "</option>");
                                }
                                else
                                {
                                    paramHtml.Append("<option value='" + arrFillList[i].ToString() + "'>" + arrFillList[i].ToString() + "</option>");
                                }
                            }
                            paramHtml.Append("</select></td>");
                            paramHtml.Append("</tr><tr>");
                            arrFillList.Clear();
                            isSqlFld = false;
                        }
                        else if (((paramMOE.ToLower() == "multi select") & (isSqlFld == false)))
                        {
                            paramHtml.Append("<td  class=paramtd1 valign=top ><font class=cap>" + paramCaption + "</font></td><td class=paramtd2><select id='" + parameterName + "' name='" + parameterName + "' TabIndex='" + tabIndex + "' class='combotem Family' style=width:85%;height:100px;  multiple=\"multiple\"  " + CallValidateExpr + ">");
                            paramHtml.Append("</select></td>");
                            paramHtml.Append("</tr><tr>");
                        }

                        iCnt = iCnt + 1;
                        if (CallValidateExpr != string.Empty && paramDepStr != string.Empty)
                        {
                            strJsArrays.Append("depParamArr[" + dpCnt + "]='" + parameterName + "';");
                            dpCnt++;
                        }
                        CallValidateExpr = string.Empty;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(paramValue))
                        {
                            paramHtml.Append("<input type=hidden id='" + parameterName + "' name='" + parameterName + "' value='" + paramValue + "'>");
                        }
                        else
                        {
                            paramHtml.Append("<input type=hidden id='" + parameterName + "' name='" + parameterName + "' value=''>");
                        }


                    }
                    fldNo = fldNo + 1;
                    paramDepStr = string.Empty;
                    if (iCnt == 2)
                    {
                        iCnt = 0;
                        paramHtml.Append("</tr><tr>");

                    }
                    Paramsids += parameterName + ",";

                }

            }
            //Paramsids += "</div>";
            //Session["paramDetails"] = strParamDetails.ToString();
            //Session["FillDepPName"] = strFillDepPName;

            paramHtml.Append("¥");
            //paramHtml.Append(strJsArrays);
            paramHtml.Append(Paramsids);
            return paramHtml.ToString() + "ƒ" + strJsArrays.ToString();


        }


        [WebMethod(EnableSession = true)]
        public void ComboFill(XmlNode baseDataNode)
        {
            foreach (XmlNode rowdNode in baseDataNode.ChildNodes)
            {
                if (rowdNode.ChildNodes[0] != null)
                {
                    if (rowdNode.ChildNodes[0].InnerText != "*")
                    {
                        string ddlValue = utils.CheckSpecialChars(rowdNode.ChildNodes[0].InnerText);

                        ddlValue = Regex.Replace(ddlValue, "&apos;", "&#39;");
                        arrFillList.Add(ddlValue);
                    }
                }

            }
        }

        [WebMethod(EnableSession = true)]
        public string GetIviewRecordCount(string IvName, string Parameters, bool isListView)
        {
            string iXml = string.Empty;
            string errlog = string.Empty;
            string result = string.Empty;
            string fileName = "GetIviewRecordCount-" + IvName;
            string paramXml;
            paramXml = ConstructParamXml(Parameters);
            string sessionId = string.Empty;
            sessionId = HttpContext.Current.Session.SessionID;
            errlog = logobj.CreateLog("GetIviewRecordCount ", sessionId, fileName, "");

            string purposeString = string.Empty;

            if (isListView)
            {
                purposeString = " purpose=\"list\" ";
            }

            iXml = "<root " + purposeString + " name ='" + IvName + "' axpapp = '" + Session["project"].ToString() + "' sessionid = '" + Session["nsessionid"].ToString() + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errlog + "' pageno='1' pagesize='1000' firsttime='yes' sqlpagination='true' getrowcount='true' gettotalrows='true'><params>" + paramXml + "</params>";
            //}

            iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root> ";

            //Call service
            result = asbExt.CallGetRecordCount(IvName, iXml, "");
            return result;
        }

        public string GetFilterData(string IvName, string Parameters)
        {
            string iXml = string.Empty;
            string errlog = string.Empty;
            string result = string.Empty;

            string title = string.Empty;
            string subTitle = string.Empty;
            string customText = string.Empty;
            string footer = string.Empty;
            DataSet dst = new DataSet();
            string fileName = "GetIViewFromObject-" + IvName;
            string paramXml;


            paramXml = ConstructParamXml(Parameters);

            iXml = "<root name ='" + IvName + "' axpapp = '" + Session["project"].ToString() + "' sessionid = '" + Session["nsessionid"].ToString() + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errlog + "' pageno='" + 1 + "' pagesize='" + 10 + "' firsttime='" + "yes" + "' sqlpagination='" + "true" + "'><params>" + paramXml + "</params>";
            //}

            iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root> ";

            //Call service
            IviewData objIview = new IviewData();
            objIview.WebServiceTimeout = asbExt.asbIview.Timeout;
            result = asbExt.CallGetIViewWS(IvName, iXml, "", objIview);
            if (result != null)
                result = result.Split('♠')[1];



            string errMsg = utils.ParseXmlErrorNode(result);

            if (errMsg != string.Empty)
            {
                return dst.GetXml();
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNodeList XmlNodeList = default(XmlNodeList);

                try
                {
                    xmlDoc.LoadXml(result);
                }
                catch (Exception ex)
                {
                    // if (trace)
                    // {
                    //  logobj.CreateLog("Exception in GetIview Service :--- " + ex.Message.ToString(), sessionId, fileName, "");
                    // }
                    return null;
                }
                //GetSubCaptions(xmlDoc);
                //CreateHeaderRow(xmlDoc, pageNo.ToString());
                XmlNodeList = xmlDoc.SelectNodes("//headrow");
                int recCount = Convert.ToInt32(XmlNodeList[0].Attributes["reccount"].Value);
                //remove all other nodes other than data and call binddata
                XmlNode rnode = xmlDoc.SelectSingleNode("//headrow/pivotghead");
                if (rnode != null)
                    rnode.ParentNode.RemoveChild(rnode);


                DataTable dt = new DataTable();

                dt.Columns.Add("title");
                dt.Columns.Add("subtitle");
                dt.Columns.Add("text");
                //GetCustomString();
                DataRow dr = dt.NewRow();
                XmlNodeList parentNodes = default(XmlNodeList);
                parentNodes = XmlNodeList[0].ChildNodes;
                for (int count = 0; count <= parentNodes.Count - 1; count++)
                {
                    foreach (XmlNode chldNode in parentNodes)
                    {
                        if (chldNode.Name == title)
                        {
                            dr["title"] = title;
                        }
                        else if (chldNode.Name == subTitle)
                        {
                            dr["subtitle"] = subTitle;
                        }
                        else if (chldNode.Name == customText)
                        {
                            dr["text"] = customText;
                        }

                    }
                }
                dt.Rows.Add(dr);

                XmlNode headRow = xmlDoc.SelectSingleNode("//headrow");
                headRow.ParentNode.RemoveChild(headRow);

                XmlNode cNode = xmlDoc.SelectSingleNode("//comps");
                cNode.ParentNode.RemoveChild(cNode);
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);

                try
                {
                    xmlDoc.WriteTo(xw);
                }
                catch (Exception ex)
                {
                    //if (trace)
                    // {
                    //    logobj.CreateLog("Exception in GetIview Service :--- " + ex.Message.ToString(), sessionId, fileName, "");
                    //}
                    return null;
                }
                //Bind the dataset
                string nXml = string.Empty;
                nXml = sw.ToString();
                // assign XML Val to Properite
                //IvResult = nXml;
                StringReader sr = new StringReader(nXml);
                dst.ReadXml(sr);
                //dt.TableName = "Head";
                //dst.Tables.Add(dt);
            }
            return dst.GetXml();
        }

        public string ConstructParamXml(string Parameters)
        {
            string pXml = string.Empty;
            string str = string.Empty;
            str = Parameters;
            string[] strp = utils.AxSplit1(str, "¿");
            int i = 0;

            for (i = 0; i <= strp.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(strp[i]))
                {
                    string[] arrparam = strp[i].ToString().Split('~');
                    arrparam[0] = utils.CheckSpecialChars(arrparam[0].ToString());
                    arrparam[1] = utils.CheckSpecialChars(arrparam[1].ToString());
                    if (arrparam[1].Contains("&amp;grave;") == true)
                    {
                        arrparam[1] = arrparam[1].ToString().Replace("&amp;grave;", "~");
                    }
                    pXml = pXml + "<" + arrparam[0].ToString() + ">";
                    pXml = pXml + arrparam[1].ToString();
                    pXml = pXml + "</" + arrparam[0].ToString() + ">";

                    //arrparam[1] = arrparam[1].ToString().Replace(",", "`");

                }

            }
            return pXml;
        }



        [WebMethod(EnableSession = true)]
        public string GetNewsfeed()
        {
            string result = string.Empty;
            if (HttpContext.Current.Session["project"] != null)
            {
                if (Session["NewsFeeds"] != null)
                {
                    result = Session["NewsFeeds"].ToString();
                }
                else
                {

                    string project = HttpContext.Current.Session["project"].ToString();
                    string sessionid = HttpContext.Current.Session["nsessionid"].ToString();
                    string user = HttpContext.Current.Session["user"].ToString();
                    string AxRole = HttpContext.Current.Session["AxRole"].ToString();
                    string errorLog = string.Empty;

                    errorLog = logobj.CreateLog("Get Newsfeed", sessionid, "newsfeed", "new");
                    string iXml = string.Empty;
                    string tid = string.Empty;
                    string sql = string.Empty;
                    //string sql = "select msg, dte, msgtype from CMIS.get_livefeedvalues(:usr, getdate()) order by dte desc";

                    if (Session["AxLatestNews"] != null)
                    {
                        sql = Session["AxLatestNews"].ToString();
                    }
                    if (sql.Contains('#'))
                    {
                        sql = sql.Substring(sql.IndexOf('#') + 1);
                    }

                    string replaceStr = "'" + Session["user"].ToString() + "'";
                    //parameter with usr,uname,user is replaced.
                    sql = sql.Replace(":usr", replaceStr);
                    sql = sql.Replace(":user", replaceStr);
                    sql = sql.Replace(":uname", replaceStr);

                    iXml = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' trace='" + errorLog + "' transid='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                    iXml += "<sql>" + sql + "</sql>";
                    iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    result = asbExt.CallGetChoiceWS(tid, iXml);
                    Session["NewsFeeds"] = result;
                }
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetLiveFeeds()
        {
            string result = string.Empty;
            if (HttpContext.Current.Session["project"] != null)
            {
                if (Session["EventReminders"] != null)
                {
                    result = Session["EventReminders"].ToString();
                }
                else
                {
                    string project = HttpContext.Current.Session["project"].ToString();
                    string sessionid = HttpContext.Current.Session["nsessionid"].ToString();
                    string user = HttpContext.Current.Session["user"].ToString();
                    string AxRole = HttpContext.Current.Session["AxRole"].ToString();
                    string errorLog = string.Empty;

                    errorLog = logobj.CreateLog("Get EventReminders", sessionid, "eventreminders", "new");
                    string iXml = string.Empty;
                    string tid = string.Empty;
                    string sql = "";


                    //string sql = "select msg, dte, msgtype from CMIS.get_livefeedvalues(:usr, getdate()) order by dte desc";
                    if (Session["AxLiveFeeds"] != null)
                    {
                        sql = Session["AxLiveFeeds"].ToString();
                    }
                    if (sql.Contains('#'))
                    {
                        sql = sql.Substring(sql.IndexOf('#') + 1);
                    }
                    string replaceStr = "'" + Session["user"].ToString() + "'";
                    //parameter with usr,uname,user is replaced.
                    sql = sql.Replace(":usr", replaceStr);
                    sql = sql.Replace(":user", replaceStr);
                    sql = sql.Replace(":uname", replaceStr);

                    iXml = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' trace='" + errorLog + "' transid='' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                    iXml += "<sql>" + sql + "</sql>";
                    iXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    result = asbExt.CallGetChoiceWS(tid, iXml);
                    Session["EventReminders"] = result;
                }
            }
            return result;
        }



        [WebMethod(EnableSession = true)]
        public string GetFilePathForIviewAttachment(string proj, string tid, string fldname, string filename, string attachtype, string onlyRecordId, string onlyFileName, bool isDbAttachment, bool onlygetPath = false)
        {
            string result = "";
            string authenticationStatus = string.Empty;
            //if (!string.IsNullOrEmpty(proj) && !string.IsNullOrEmpty(tid) && !string.IsNullOrEmpty(filename))
            //proj is removed to map path same as in SaveImageToFolder in tstructdata.cs
            if (!string.IsNullOrEmpty(tid) && !string.IsNullOrEmpty(filename))
            {
                if (Session["nsessionid"] == null || string.IsNullOrEmpty(Session["nsessionid"].ToString()))
                    return utilObj.SESSTIMEOUT;
                string path = string.Empty;//concatenate the path for the file.

                if (!isDbAttachment)
                {
                    if (attachtype == "fullpath")
                    {
                        filename = utilObj.CheckReverseUrlSpecialChars(filename);
                        string splitter = Regex.Match("\\/", @"\\|\/").ToString();
                        if (splitter.Length > 0)
                        {
                            List<string> splittedArr = filename.Split(splitter.ToCharArray()[0]).ToList();
                            if (splittedArr.Count > 0)
                            {
                                //path = splittedArr
                                filename = splittedArr[splittedArr.Count - 1];
                                splittedArr.RemoveAt(splittedArr.Count - 1);
                                path = String.Join(splitter, splittedArr.ToArray()) + splitter;

                            }
                        }
                    }
                    else if (attachtype == "header" && HttpContext.Current.Session["AxpAttachmentPathGbl"] != null)
                    {
                        string tempAttachFilePath = HttpContext.Current.Session["AxpAttachmentPathGbl"].ToString();
                        if (!tempAttachFilePath.Contains(":\\") && !tempAttachFilePath.Contains("\\\\"))
                        {
                            //tempAttachFilePath = AppDomain.CurrentDomain.BaseDirectory + "CommonDir\\" + tempAttachFilePath;
                            tempAttachFilePath = HttpContext.Current.Application["ScriptsPath"].ToString() + axpert + Session["nsessionid"] + "\\" + tempAttachFilePath;
                            if (!Directory.Exists(tempAttachFilePath))
                            {
                                Directory.CreateDirectory(tempAttachFilePath);
                            }
                        }
                        path = tempAttachFilePath + "\\" + tid + "\\";
                    }
                    else if (HttpContext.Current.Session["AxpImagePathGbl"] != null)
                    {
                        string tempGridAttachPath = HttpContext.Current.Session["AxpImagePathGbl"].ToString();
                        if (!tempGridAttachPath.Contains(":\\") && !tempGridAttachPath.Contains("\\\\"))
                        {
                            //tempGridAttachPath = AppDomain.CurrentDomain.BaseDirectory + "CommonDir\\" + tempGridAttachPath;
                            tempGridAttachPath = HttpContext.Current.Application["ScriptsPath"].ToString() + axpert + Session["nsessionid"] + "\\" + tempGridAttachPath;
                            if (!Directory.Exists(tempGridAttachPath))
                            {
                                Directory.CreateDirectory(tempGridAttachPath);
                            }
                        }
                        path = tempGridAttachPath + "\\" + tid + "\\" + fldname + "\\";
                    }
                }

                string dPath = string.Empty;
                //string recordID = filename.Split('-')[0];
                //string fileList = filename.Replace(recordID + "-", "");

                if (path != string.Empty && !isDbAttachment && utilObj.GetAuthentication(ref authenticationStatus))
                {
                    //string resolvedFilePath = path + filename;
                    foreach (string file in filename.Split(','))
                    {
                        string internalFileName = "";
                        //filename = recordID + "-" + file;
                        DirectoryInfo di = new DirectoryInfo(path);
                        if (!di.Exists)
                            di.Create();

                        FileInfo[] fileEntries = null;

                        if (attachtype == "fullpath" || attachtype == "gridattach")
                        {
                            fileEntries = di.GetFiles(file);
                        }
                        else if (attachtype == "image")
                        {
                            fileEntries = di.GetFiles(file + ".*");
                        }
                        else if (attachtype == "header")
                        {
                            fileEntries = di.GetFiles(file + "-*");
                        }

                        if (fileEntries != null && fileEntries.Length > 0)
                        {
                            if (attachtype == "fullpath" || attachtype == "gridattach")
                            {
                                internalFileName = file;
                                FileInfo dirFile = (FileInfo)fileEntries[0];

                                result += genericFileDownloader(dirFile, internalFileName, path, ref dPath, filename, attachtype, onlygetPath);
                            }
                            else if (attachtype == "image" || attachtype == "header")
                            {
                                for (int i = 0; i < fileEntries.Length; i++)
                                {
                                    internalFileName = file;
                                    FileInfo dirFile = (FileInfo)fileEntries[i];

                                    result += genericFileDownloader(dirFile, internalFileName, path, ref dPath, filename, attachtype, onlygetPath);
                                }
                            }
                        }

                    }
                    if (!onlygetPath && attachtype == "header")
                    {
                        try
                        {
                            DirectoryInfo di = new DirectoryInfo(dPath);
                            if (!di.Exists)
                                di.Create();

                            FileInfo[] fileEntries = di.GetFiles();
                            //if (fileEntries.Length > 1)
                            //{
                            using (ZipFile zipFile = new ZipFile(Encoding.UTF8)) //to handle file name characters in all languages(UTF8)
                            {
                                try
                                {
                                    zipFile.AddDirectory(dPath);
                                    zipFile.Save(dPath.Substring(0, dPath.Length - 1) + ".zip");
                                    result = dPath.Substring(0, dPath.Length - 1) + ".zip";
                                }
                                catch (Exception)
                                {
                                    //result = "Error: Failure while processing Zipping file.";
                                }
                            }
                            //}
                        }
                        catch (Exception ex) { }
                    }
                }
                else if (isDbAttachment)
                {
                    string inputXML = string.Empty;

                    inputXML = "<root axpapp=\"" + proj + "\"  sessionid=\"" + Session["nsessionid"].ToString() + "\"  filename=\"" + onlyFileName + "\" transid=\"" + tid /*+ fldname*/ + "\"  recordid=\"" + onlyRecordId + "\"   trace=\"♦♦GetAttachedFile♦\">";

                    string errorLog = logobj.CreateLog("Get Structure object from cache for view attachment.", sessionid, "GetStructXml-", "new");
                    inputXML = GetTraceString(inputXML);

                    inputXML = inputXML + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

                    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();

                    result = objWebServiceExt.CallViewAttachmentsWS(tid, inputXML, "");

                    JObject resultJSON = new JObject();

                    try
                    {
                        resultJSON = JObject.Parse(result);
                        resultJSON["basePath"] = utilObj.ScriptsurlPath + "Axpert/" + Session["nsessionid"].ToString() + "/";

                        result = resultJSON.ToString();
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else
                return "Error: Invalid Param Values.";

            if (!onlygetPath && result == "" || result == "♠")
            {
                if (authenticationStatus == "" || authenticationStatus == "success")
                {
                    if (attachtype == "header")
                    {
                        result = "Error: There is no attachment to download for this record." + "♠";
                    }
                    else
                    {
                        result = String.Format("Error: File '{0}' is not available.", filename) + "♠";
                    }
                }
                else
                {
                    result = String.Format("Error: File '{0}' is not accessible => {1}.", filename, authenticationStatus) + "♠";
                }
            }

            return result.Trim('♠');
        }

        [WebMethod(EnableSession = true)]
        public string GetAxpFilePathForListviewAttachment(string filepath, string filename)
        {
            string result = "";
            string authenticationStatus = string.Empty;
            if (!string.IsNullOrEmpty(filename))
            {
                if (Session["nsessionid"] == null || string.IsNullOrEmpty(Session["nsessionid"].ToString()))
                    return utilObj.SESSTIMEOUT;
                string path = string.Empty;//concatenate the path for the file.

                if (filepath == string.Empty)
                {
                    if (HttpContext.Current.Session["AxpImagePathGbl"] != null)
                    {
                        string tempGridAttachPath = HttpContext.Current.Session["AxpImagePathGbl"].ToString();
                        if (!tempGridAttachPath.Contains(":\\") && !tempGridAttachPath.Contains("\\\\"))
                        {
                            tempGridAttachPath = HttpContext.Current.Application["ScriptsPath"].ToString() + axpert + Session["nsessionid"] + "\\" + tempGridAttachPath;
                            if (!Directory.Exists(tempGridAttachPath))
                            {
                                Directory.CreateDirectory(tempGridAttachPath);
                            }
                        }
                        filepath = tempGridAttachPath + "\\\\";
                    }
                }

                path = filepath;
                filename = utilObj.CheckReverseUrlSpecialChars(filename);

                string dPath = string.Empty;

                if (utilObj.GetAuthentication(ref authenticationStatus))
                {
                    foreach (string file in filename.Split(','))
                    {
                        string internalFileName = "";
                        DirectoryInfo di = new DirectoryInfo(path);
                        if (!di.Exists)
                            di.Create();

                        FileInfo[] fileEntries = di.GetFiles(file);

                        if (fileEntries != null && fileEntries.Length > 0)
                        {
                            internalFileName = file;
                            FileInfo dirFile = (FileInfo)fileEntries[0];

                            result += genericFileDownloader(dirFile, internalFileName, path, ref dPath, filename);
                        }
                    }
                }
            }
            else
                return "Error: Invalid Param Values.";

            if (result == "" || result == "♠")
            {
                if (authenticationStatus != "" && authenticationStatus != "success")
                {
                    result = String.Format("Error: File '{0}' is not available.", filename) + "♠";
                }
                else
                {
                    result = String.Format("Error: File '{0}' is not accessible => {1}.", filename, authenticationStatus) + "♠";
                }
            }

            return result.Trim('♠');
        }

        private string genericFileDownloader(FileInfo dirFile, string internalFileName, string path, ref string dPath, string filename, string attachtype = "", bool onlygetPath = false)
        {
            string result = string.Empty;

            if (dirFile.Name.StartsWith(internalFileName + "."))
            {
                internalFileName = dirFile.Name;
            }
            else if (attachtype == "header" && dirFile.Name.StartsWith(internalFileName + "-"))
            {
                internalFileName = dirFile.Name;
            }
            //else if(attachtype == "gridattach" && i > 0)
            //{
            //    break;
            //}



            if (System.IO.Directory.Exists(path) && System.IO.File.Exists(path + internalFileName))
            {

                dPath = utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\";
                //if (filename.Contains(".docx"))
                //{
                //    dPath += "Docs" + "\\";
                //}
                if (!onlygetPath && attachtype == "header")
                {
                    dPath += filename + "\\";
                }
                try
                {
                    if (!System.IO.Directory.Exists(dPath))
                        System.IO.Directory.CreateDirectory(dPath);
                    System.IO.File.Copy(path + internalFileName, dPath + internalFileName, true);

                    if (internalFileName != string.Empty && internalFileName.EndsWith(".zip"))
                    {
                        string unzipFile = string.Empty;
                        string fileProtectPwd = string.Empty;
                        if (HttpContext.Current.Session["axp_uploadfileprotectedpwd"] != null)
                            fileProtectPwd = HttpContext.Current.Session["axp_uploadfileprotectedpwd"].ToString();
                        if (fileProtectPwd != string.Empty)
                        {
                            string zipFilePath = Path.Combine(dPath, internalFileName);
                            using (ZipFile zip = ZipFile.Read(zipFilePath))
                            {
                                zip.Password = fileProtectPwd;
                                foreach (ZipEntry e in zip)
                                {
                                    e.Extract(dPath, ExtractExistingFileAction.OverwriteSilently);
                                    unzipFile = e.FileName;
                                }
                            }
                            result += utilObj.ScriptsurlPath + "Axpert/" + Session["nsessionid"].ToString() + "/" + unzipFile + "♠";
                        }
                        else
                            result += utilObj.ScriptsurlPath + "Axpert/" + Session["nsessionid"].ToString() + "/" + internalFileName + "♠";
                    }
                    else
                        result += utilObj.ScriptsurlPath + "Axpert/" + Session["nsessionid"].ToString() + "/" + internalFileName + "♠";
                }
                catch (Exception ex)
                {
                    result += String.Format("Error: Failure while processing the file '{0}'.", internalFileName) + "♠";
                }
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetHelpText(string tid)
        {
            GetGlobalVars();
            string errorLog = string.Empty;
            string helpRes = string.Empty;
            string getHelpsql = string.Empty;
            string fileName = string.Empty;

            try
            {
                if (Session["nsessionid"] == null || string.IsNullOrEmpty(Session["nsessionid"].ToString()))
                    return utilObj.SESSTIMEOUT;

                ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();

                getHelpsql = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>select rtf_hint from AxTstructHint where tid='" + tid + "'</sql>";
                getHelpsql += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                fileName = "LoadHint-" + tid;
                logobj.CreateLog("Getting Help Text.", sessionid, fileName, "");
                //Call service
                helpRes = objWebServiceExt.CallGetChoiceWS(tid, getHelpsql);

                if (helpRes.Contains(Constants.ERROR) == true)
                {
                    helpRes = helpRes.Replace(Constants.ERROR, "");
                    helpRes = helpRes.Replace("</error>", "");
                    helpRes = helpRes.Replace("\n", "");
                    throw (new Exception(helpRes));
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
            string hintText = string.Empty;
            if (helpRes != string.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(helpRes);
                }
                catch (Exception ex)
                {
                }
                XmlNode resultNode = null;


                resultNode = xmlDoc.SelectSingleNode("//row");
                if (resultNode != null)
                {
                    foreach (XmlNode childNode in resultNode.ChildNodes)
                    {
                        if (childNode.Name.ToLower() == "rtf_hint")
                            hintText = childNode.InnerText;
                    }
                }
            }
            return hintText;
        }

        [WebMethod(EnableSession = true)]
        public string GetZipPathForIviewAttachment(string[] proj, string[] tids, string[] rowids, string[] filenames)
        {
            string result = string.Empty;
            string dPath = string.Empty;
            string gridAttachPath = string.Empty;
            string AttachFilePath = string.Empty;
            if (Session["nsessionid"] == null)
                return "Error: Your Session has expired";
            if (HttpContext.Current.Session["AxGridAttachPath"] != null && HttpContext.Current.Session["AxAttachFilePath"] != null)
            {

                gridAttachPath = HttpContext.Current.Session["AxGridAttachPath"].ToString();
                AttachFilePath = HttpContext.Current.Session["AxAttachFilePath"].ToString();

                if (!gridAttachPath.Contains(":\\") && !gridAttachPath.Contains("\\\\"))
                {
                    gridAttachPath = AppDomain.CurrentDomain.BaseDirectory + "CommonDir\\" + gridAttachPath;
                    if (!Directory.Exists(gridAttachPath))
                    {
                        Directory.CreateDirectory(gridAttachPath);
                    }
                }

                if (!AttachFilePath.Contains(":\\") && !AttachFilePath.Contains("\\\\"))
                {
                    AttachFilePath = AppDomain.CurrentDomain.BaseDirectory + "CommonDir\\" + AttachFilePath;
                    if (!Directory.Exists(AttachFilePath))
                    {
                        Directory.CreateDirectory(AttachFilePath);
                    }
                }
            }
            else
                return "Error: Failure while processing the file.";

            if (!CheckDestinationFolder(utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString()))
                result = "Error: While Creating folder";
            if (proj.Length > 0 && proj.Length == tids.Length && proj.Length == filenames.Length)
            {
                try
                {
                    dPath = utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\ZipDownloads\\";
                    for (int i = 0; i < proj.Length; i++)
                    {
                        if (!utilObj.IsNullOrEmpty(proj[i]) && !string.IsNullOrEmpty(tids[i].ToString()) && !string.IsNullOrEmpty(filenames[i].ToString()))
                        {
                            string path = string.Empty;//concatenate the path for the file. 
                            path = !string.IsNullOrEmpty(rowids[i]) ? gridAttachPath + "\\" + proj[i] + "\\" + tids[i] + "\\" + rowids[i] + "\\" : AttachFilePath + "\\" + proj[i] + "\\" + tids[i] + "\\";

                            string recordID = filenames[i].Split('-')[0];
                            string fileList = filenames[i].Replace(recordID + "-", "");

                            foreach (string file in fileList.Split(','))
                            {
                                string filename = recordID + "-" + file;

                                if (System.IO.File.Exists(path + filename))
                                    System.IO.File.Copy(path + filename, dPath + filename, true);
                            }
                        }
                        else
                            return "Error: Invalid Param Values.";
                    }
                }
                catch (Exception ex)
                {
                    return "Error: Failure while processing the file.";
                }
            }
            else
                return "Error: Failure while processing the file.";

            using (ZipFile zipFile = new ZipFile(Encoding.UTF8)) //to handle file name characters in all languages(UTF8)
            {
                try
                {
                    zipFile.AddDirectory(utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\ZipDownloads");
                    zipFile.Save(utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\ZipDownloads\\SelectedFiles.zip");
                    result = utilObj.ScriptsurlPath + "Axpert/" + Session["nsessionid"].ToString() + "/ZipDownloads/SelectedFiles.zip";
                }
                catch (Exception)
                {
                    result = "Error: Failure while processing Zipping file.";
                }
            }
            return result;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public string GetLanguage(string proj, string source)
        {
            string strLang = "ENGLISH";
            if (source == "Login")
            {
                strLang = utilObj.GetConfigAttrValue(proj, "AxLanguages");
            }
            else
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string inputXML = string.Empty;
                string result = string.Empty;


                //Call service
                try
                {
                    string errorLog = string.Empty;
                    errorLog = logobj.CreateLog("Get GetLanguage", sessionid, "GetLanguage-getchoice", "new");
                    inputXML = "<sqlresultset axpapp='" + proj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' >";
                    inputXML += "<sql>select distinct LANGNAME from axlangsource</sql>";
                    inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    result = asbExt.CallGetChoiceWS("", inputXML);

                }
                catch (Exception ex) { }
                string errMsg = string.Empty;
                errMsg = utilObj.ParseXmlErrorNode(result);

                if (errMsg != string.Empty)
                {
                    if (errMsg == Constants.SESSIONERROR)
                    {
                        Session.RemoveAll();
                        Session.Abandon();
                        utilObj.IFrameSessExpiry();
                    }
                }
                else if (result != string.Empty)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(result);
                    XmlNodeList WrproductNodes = default(XmlNodeList);
                    WrproductNodes = xmlDoc.GetElementsByTagName("row");
                    if ((WrproductNodes.Count == 0))
                    { }
                    else
                    {
                        DataSet dsPages = new DataSet();
                        System.IO.StringReader sr = new System.IO.StringReader(result);
                        dsPages.ReadXml(sr);

                        ds = dsPages;

                        if (ds.Tables.Count > 1)
                        {
                            dt = ds.Tables["row"];
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    for (int idx = 0; idx < dt.Rows.Count; idx++)
                    {
                        strLang += "," + dt.Rows[idx][0].ToString();
                    }
                }
            }

            return strLang;
        }

        private bool CheckDestinationFolder(string path)
        {
            try
            {
                if (!System.IO.Directory.Exists(path + "\\ZipDownloads"))
                    System.IO.Directory.CreateDirectory(path + "\\ZipDownloads");
                else
                {
                    System.IO.Directory.Delete(path + "\\ZipDownloads", true);
                    System.IO.Directory.CreateDirectory(path + "\\ZipDownloads");
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public string GetRoles()
        {
            try
            {
                string inputXML = string.Empty;
                logobj.CreateLog("GetRolesforDefaultLandingPage", Session["nsessionid"].ToString(), "GetRolesList", "Configuration");
                inputXML = "<root axpapp='" + Session["Project"].ToString() + "' sessionid= '" + Session["nsessionid"].ToString() + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='' role ='' act='' res='' page='' struct='' pagesize='10' pageno='1'>";
                inputXML += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

                string result = string.Empty;
                //Call service
                result = asbExt.CallGetRolesListWS("Role", inputXML);
                if ((!result.Contains(Constants.ERROR) == true) || (result != ""))
                {
                    return result;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }




        [System.Web.Services.WebMethod(EnableSession = true)]
        public string GetChoices(string tStrType, string value)
        {
            string sqlQuery = string.Empty;
            string resGetCh = string.Empty;

            if (tStrType == "design" && HttpContext.Current.Session["axdb"].ToString().ToLower() == "oracle")
                sqlQuery = Constants.ORCL_QRY_PRPS_DSGN;
            else if (tStrType == "design" && HttpContext.Current.Session["axdb"].ToString().ToLower() == "ms sql")
                sqlQuery = Constants.SQL_QRY_PRPS_DSGN;
            else if (tStrType == "normal" && HttpContext.Current.Session["axdb"].ToString().ToLower() == "oracle")
                sqlQuery = Constants.ORCL_QRY_PRPS_USER;
            else if (tStrType == "normal" && HttpContext.Current.Session["axdb"].ToString().ToLower() == "ms sql")
                sqlQuery = Constants.SQL_QRY_PRPS_USER;

            //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
            ASB.WebService objCWbSer = new ASB.WebService();
            try
            {
                if (!string.IsNullOrEmpty(sqlQuery))
                {
                    sqlQuery = sqlQuery.Replace("$USERID$", HttpContext.Current.Session["user"].ToString());
                    sqlQuery = sqlQuery.Replace("$TRANID$", HttpContext.Current.Session["transid"].ToString());
                    sqlQuery = sqlQuery.Replace("$VALUE$", value);
                    sqlQuery = sqlQuery.Replace("$TYPE$", "axPurpose");
                }

                string result = objCWbSer.GetChoicesCustomWS(HttpContext.Current.Session["transid"].ToString(), sqlQuery);
            }
            catch (Exception ex)
            {
                return resGetCh;
            }
            return resGetCh;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public string GetPrpLblStatus()
        {
            string resGetCh = "false";
            string dbType = HttpContext.Current.Session["axdb"].ToString().ToLower();
            //ASBCustom.CustomWebservice objCWbSer = new ASBCustom.CustomWebservice();
            ASB.WebService objCWbSer = new ASB.WebService();
            try
            {
                string sql = "Select ( CASE WHEN (Select Value  from axpCloudDevSettings where ID='" + HttpContext.Current.Session["user"] + "' and type=\'axPurpose\') IS NULL THEN 0 else (Select Value  from axpCloudDevSettings where ID='" + HttpContext.Current.Session["user"] + "' and type=\'axPurpose\') end ) AS admn,(CASE WHEN (Select Value  from axpCloudUserSettings where USERID='" + HttpContext.Current.Session["user"] + "' and \'axPurpose\') IS NULL THEN 0 else (Select Value  from axpCloudUserSettings where USERID='" + HttpContext.Current.Session["user"] + "' and type=\'axPurpose\')end ) AS usr " + (dbType == "oracle" ? "from dual" : "");
                if (!string.IsNullOrEmpty(sql))
                    sql = sql.Replace("$USERID", HttpContext.Current.Session["user"].ToString());

                string result = objCWbSer.GetChoicesCustomWS(HttpContext.Current.Session["transid"].ToString(), sql);
                if (result != string.Empty)
                {
                    DataSet ds = new DataSet();
                    StringReader sr = new StringReader(result);
                    ds.ReadXml(sr);
                    DataTable dt = ds.Tables["row"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0]["admn"].ToString()))
                        {
                            int adminValue = Convert.ToInt32(dt.Rows[0]["admn"]);
                            int userValue = Convert.ToInt32(dt.Rows[0]["usr"]);
                            if (adminValue > 0 && userValue > 0)
                                resGetCh = "true";
                            else if (adminValue <= 0 && userValue > 0)
                                resGetCh = "true";
                            else if (adminValue > 0 && userValue <= 0)
                                resGetCh = "false";
                            else if (adminValue <= 0 && userValue <= 0)
                                resGetCh = "false";
                        }
                        else
                            resGetCh = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                return resGetCh;
            }
            return resGetCh;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public string GetFastDepFlds(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string ArrActionLog, string visibleDcs, string str, string tid, string key, string frameNo, string rowNo, string changeFieldName, string subGridInfo, string parentInfo, string fieldName, string resTstHtmlLS)
        {
            DateTime stDate = DateTime.Now;
            string result = string.Empty;
            string _tstKeyChanged = "";
            try
            {
                if (HttpContext.Current.Session["project"] == null)
                    return utilObj.SESSTIMEOUT;

                if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
                {
                    return Constants.MALICIOUSNPUTDETECTED;
                }

                DateTime sTime1 = DateTime.Now;
                if (resTstHtmlLS != "")
                {
                    utilObj.DeleteTstIvObject(key.Split('_')[0]);
                    key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                    _tstKeyChanged = key;
                }
                TStructData tstData = (TStructData)Session[key];
                if (tstData == null)
                    return Constants.DUPLICATESESS;
                tstData.UpdateDataList(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray);
                //FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                //result = fObj.GetDepFastDataJson(fieldName, tid, tstData, rowNo);
                DateTime eTime1 = DateTime.Now;
                DateTime sTime2 = DateTime.Now;
                tstData.AxActiveDc = string.Empty;
                DateTime eTime2 = DateTime.Now;
                if (tstData.logTimeTaken)
                    tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Exception in GetFastDepFlds-" + ex.Message, sessionid, "GetFastDep", "new");
            }
            DateTime edTime = DateTime.Now;
            logobj.CreateLog("Timetaken in GetFastDepFlds- Field" + fieldName + "-" + stDate.Subtract(edTime).TotalMilliseconds.ToString(), sessionid, "LogTimeTaken", "");

            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        public string GetRapidAutoComplete(string tstDataId, string fldName, string fldValue, ArrayList ChangedFields, ArrayList ChangedFieldDbRowNo, ArrayList ChangedFieldValues, ArrayList DeletedDCRows, string pageData, string fastdll, string fldNameAc, string refreshAC, TStructData tstData)
        {
            //pageNo,pagesize, frameno, activerow, parinfo subinfo, includedcs
            string[] pgData = pageData.Split('~');
            string errorLog = logobj.CreateLog("Autocomplete- GetRapidAutoComplete.", sessionid, "Rapid_Autocomplete-" + fldName, "new");
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join("", ChangedFieldValues.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            string result = string.Empty;
            try
            {
                transid = tstData.transid;
                string activePRow = string.Empty; string activePDc = string.Empty;

                if (pageData != string.Empty)
                {
                    int idx = pageData.IndexOf("♠");
                    if (idx != -1)
                    {
                        activePDc = pageData.Substring(0, idx);
                        if (!string.IsNullOrEmpty(activePDc) && activePDc != "")
                            activePDc = activePDc.Split('~').Last();
                        activePRow = pageData.Substring(idx + 1);
                        if (!string.IsNullOrEmpty(activePRow) && activePRow != "")
                            activePRow = activePRow.Split('~').First();
                    }
                }

                StringBuilder iXml = new StringBuilder();
                string token = utilObj.GetToken();
                HttpContext.Current.Session["axpToken"] = token;
                iXml.Append("<sqlresultset token='" + token + "' activerow='" + pgData[3] + "' pdc='" + activePDc + "' prow='" + activePRow + "' frameno='" + pgData[2] + "' axpapp='" + project + "' value='" + fldValue + "' sessionid= '" + sessionid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' field='" + fldName + "'");
                iXml.Append(" sqlfield='' trace='" + errorLog + "' transid='" + transid + "' pageno='" + pgData[0] + "' ");
                iXml.Append("pagesize='" + pgData[1] + "'>");
                iXml.Append(tstData.GetRapidFieldValueXml(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, pgData[2], pgData[3], fldName));
                iXml.Append(tstData.fieldValueXml + tstData.memVarsData);
                iXml.Append(HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString());
                iXml.Append("</sqlresultset>");
                result = asbExt.CallRapidSearchWS(transid, iXml.ToString());
                if (fastdll == "True" && result != "" && utilObj.ParseJSonErrorNode(result) == "")
                    tstData.AddToFasDDl(fldNameAc, result);
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Exception while calling Autocomplete- field-" + fldName, sessionid, "Autocomp-Exc", "new");
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public bool SetSavedAndPublishedID(string transId, string designJSON, string savedID = "", string publishedID = "", string ispublish = "N")
        {
            bool saved = false;
            bool update = false;

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            FDW fdwObj = new FDW();
            string fdKey = Constants.REDISTSTRUCT;
            string fdKeyMob = Constants.REDISTSTRUCTMOB;
            string designKey = Constants.REDISTSTRUCTAXDESIGN;
            string designCustHtmlKey = Constants.REDISTSTRUCTAXCUSTHTML;
            //string designTblKey = Constants.REDISTSTRUCTAXDESIGNTABLE;
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            //fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designTblKey, transId), "", false, schemaName);
            fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(fdKey, transId), "", false, schemaName);
            fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(fdKeyMob, transId), "", false, schemaName);
            saved = fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designKey, transId), "", false, schemaName);
            fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(designCustHtmlKey, transId), "", false, schemaName);
            return saved;
        }

        public string GetdllAutoComplete(string tstDataId, string fldName, string fldValue, ArrayList ChangedFields, ArrayList ChangedFieldDbRowNo, ArrayList ChangedFieldValues, ArrayList DeletedDCRows, string pageData, string fastdll, string fldNameAc, string refreshAC, string pickArrow, string parentFlds, string rfSave, string IsApiFld, string tblSourceParams, string isTstHtmlLs, string ddlFldSqlParams, string ddlSqlPNames)
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            string strJson = string.Empty;
            string requestProcess_logtime = string.Empty;
            GetGlobalVars();
            string errorLog = logobj.CreateLog("Autocomplete- GetdllAutoComplete.", sessionid, "autocomplete-" + fldName, "new");
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            string strRequest = ObjExecTr.RequestProcessTime("Request");
            DateTime startkst = DateTime.Now;

            if (Util.Util.CheckCrossScriptingInString(String.Join("", ChangedFieldValues.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            if (fldValue != "" && Util.Util.CheckCrossScriptingInString(fldValue))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            strRequest += ObjExecTr.KernelProcessTime(startkst, "After CheckCrossScriptingInString called ", "", "");
            startkst = DateTime.Now;

            string _tstKeyChanged = "";
            if (isTstHtmlLs != "")
            {
                strRequest += ObjExecTr.KernelProcessTime(startkst, "Before DeleteTstIvObject called ", "", "");
                startkst = DateTime.Now;

                utilObj.DeleteTstIvObject(tstDataId.Split('_')[0]);
                tstDataId = utilObj.GetReGenTstDataObj(tstDataId, isTstHtmlLs);
                _tstKeyChanged = tstDataId;

                strRequest += ObjExecTr.KernelProcessTime(startkst, "After DeleteTstIvObject called ", "", "");
                startkst = DateTime.Now;
            }
            string parentFldNames = string.Empty;
            string result = string.Empty;
            TStructData tstData = (TStructData)Session[tstDataId];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            strRequest += ObjExecTr.KernelProcessTime(startkst, "After tstData object taken from session called ", "", "");
            startkst = DateTime.Now;
            string isRapidCall = "false";
            //if ((Session["RapidTsTruct"] != null && Session["RapidTsTruct"].ToString() == "true") && (HttpContext.Current.Session["AxIsPerfCode"] != null && HttpContext.Current.Session["AxIsPerfCode"].ToString().ToLower() == "true") && tstData.transid.ToLower() != "axglo")
            //    isRapidCall = "true";

            if (isRapidCall == "true")//Rapid tstruct 
            {
                //result = GetRapidAutoComplete(tstDataId, fldName, fldValue, ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, pageData, fastdll, fldNameAc, refreshAC, tstData);
            }
            else
            {
                //pageNo,pagesize, frameno, activerow, parinfo subinfo, includedcs
                string[] pgData = pageData.Split('~');
                string activeRow = pgData[3];
                int fetchRCount = 1000;
                if (Session["PickListFetchSize"] != null && Session["PickListFetchSize"].ToString() != "")
                    fetchRCount = int.Parse(Session["PickListFetchSize"].ToString());
                int pgNumber = int.Parse(pgData[0]);
                if (fastdll == "True")
                    fldValue = ""; //Making value empty if the field is normal select 
                else
                    pgData[1] = fetchRCount.ToString();//If the field is picklist set row count is 1000.
                transid = tstData.transid;
                strRequest += ObjExecTr.KernelProcessTime(startkst, "Before GetFieldValueXml dataset update called ", "", "");
                startkst = DateTime.Now;

                tstData.GetFieldValueXml(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, "-1", "AutoComplete", "NG", tstData.tstStrObj.GetFieldDc(fldName).ToString());
                string paramiXml = string.Empty;
                strRequest += ObjExecTr.KernelProcessTime(startkst, "After GetFieldValueXml dataset update called ", "", "");
                startkst = DateTime.Now;
                if (ddlFldSqlParams == "1")
                {
                    string SessAutKey = "autocomplete♦" + transid + "-" + fldName + "-" + parentFlds;
                    if (Session[SessAutKey] != null && int.Parse(activeRow) == 0)
                    {
                        paramiXml = Session[SessAutKey].ToString();
                        parentFlds = paramiXml.Split('♠')[1];
                        parentFldNames = paramiXml.Split('♠')[2];
                        paramiXml = paramiXml.Split('♠')[0];

                        strRequest += ObjExecTr.KernelProcessTime(startkst, "After autocomplete Session for parameters called ", "", "");
                        startkst = DateTime.Now;
                    }
                    else
                    {
                        var lstAutKeys = Session.Keys.Cast<string>().Where(x => x.StartsWith("autocomplete♦")).ToList();
                        if (lstAutKeys.Count > 0)
                            Session.Remove(lstAutKeys[0]);

                        strRequest += ObjExecTr.KernelProcessTime(startkst, "After autocomplete Session not available and clear previous session called ", "", "");
                        startkst = DateTime.Now;

                        if (ddlSqlPNames == "")
                        {
                            string fldkey = utilObj.GetRedisServerFieldkey(tstData.transid, Constants.FIELDPARAMNAMES, fldName, 0);
                            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                            string _fldParamsfromRD = fObj.HashGetKey(fldkey, fldName);
                            if (_fldParamsfromRD != string.Empty)
                            {
                                paramiXml = tstData.GetFieldSqlParamsVars(fldName, _fldParamsfromRD, tstData.fieldValueXml, HttpContext.Current.Session["axGlobalVars"].ToString(), HttpContext.Current.Session["axUserVars"].ToString(), int.Parse(activeRow), parentFlds, tblSourceParams);
                                parentFlds = paramiXml.Split('♠')[1];
                                parentFldNames = paramiXml.Split('♠')[2];
                                paramiXml = paramiXml.Split('♠')[0];

                                strRequest += ObjExecTr.KernelProcessTime(startkst, "After ddlSqlPNames is empty and taken from redis called ", "", "");
                                startkst = DateTime.Now;
                            }
                            else
                            {
                                paramiXml = tstData.GetFieldSqlQuery(fldName, tstData.fieldValueXml, HttpContext.Current.Session["axGlobalVars"].ToString(), HttpContext.Current.Session["axUserVars"].ToString(), int.Parse(activeRow), parentFlds, tblSourceParams);
                                parentFlds = paramiXml.Split('♠')[1];
                                parentFldNames = paramiXml.Split('♠')[2];
                                paramiXml = paramiXml.Split('♠')[0];

                                strRequest += ObjExecTr.KernelProcessTime(startkst, "After ddlSqlPNames is empty and called old GetFieldSqlQuery called ", "", "");
                                startkst = DateTime.Now;
                            }
                        }
                        else
                        {
                            paramiXml = tstData.GetFieldSqlParamsVars(fldName, ddlSqlPNames, tstData.fieldValueXml, HttpContext.Current.Session["axGlobalVars"].ToString(), HttpContext.Current.Session["axUserVars"].ToString(), int.Parse(activeRow), parentFlds, tblSourceParams);
                            parentFlds = paramiXml.Split('♠')[1];
                            parentFldNames = paramiXml.Split('♠')[2];
                            paramiXml = paramiXml.Split('♠')[0];

                            strRequest += ObjExecTr.KernelProcessTime(startkst, "After ddlSqlPNames is not empty and GetFieldSqlParamsVars called ", "", "");
                            startkst = DateTime.Now;
                        }
                    }
                }

                bool isCache = false;
                try
                {
                    FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                    if (fastdll == "True" && rfSave != "True" && refreshAC == "False")//Fetching data from redis for combobox field based on bound parents, if parents got changed with these parents will call the db then cached same.
                    {
                        //string strRequest = ObjExecTr.RequestProcessTime("Request");
                        strRequest += ObjExecTr.KernelProcessTime(startkst, "Server side process before data ", "", "");
                        DateTime kst = DateTime.Now;
                        string fldkey = utilObj.GetRedisServerFieldkey(tstData.transid, Constants.FIELDNAME, fldName, 0);
                        string dllData = fObj.HashGetKey(fldkey, parentFlds);
                        logobj.CreateLog("Data from Redis.-" + dllData, sessionid, "autocomplete-" + fldName, "");
                        if (dllData != string.Empty && refreshAC == "False" && pickArrow == "False")
                        {
                            result = dllData;
                            isCache = true;
                            if (_tstKeyChanged != "")
                                result = _tstKeyChanged + "♣*♣" + result;
                            result = result + "♠" + strRequest + ObjExecTr.KernelProcessTime(kst, "GetSearchResult-fromRedis", "", result);
                            return result;
                        }
                    }
                    else if (fastdll == "True" && rfSave == "True" && refreshAC == "False")//Fetching data from session for combobox+refreshonsave filed.
                    {
                        //string strRequest = ObjExecTr.RequestProcessTime("Request");
                        strRequest += ObjExecTr.KernelProcessTime(startkst, "Server side process before data ", "", "");
                        DateTime kst = DateTime.Now;
                        string sessCacheKey = transid + "-" + fldName + "-" + parentFlds;
                        if (Session[sessCacheKey] != null)
                        {
                            result = Session[sessCacheKey].ToString();
                            isCache = true;
                            if (_tstKeyChanged != "")
                                result = _tstKeyChanged + "♣*♣" + result;
                            result = result + "♠" + strRequest + ObjExecTr.KernelProcessTime(kst, "GetSearchResult-fromSession", "", result);
                            return result;
                        }
                    }
                    else if (fastdll == "False" && fldValue == "")//Fetching data from session for picklist field and input vaue is empty(that means user clicked on ENTER key, ARROW key or field arrow icon).
                    {
                        //string strRequest = ObjExecTr.RequestProcessTime("Request");
                        strRequest += ObjExecTr.KernelProcessTime(startkst, "Server side process before data ", "", "");
                        DateTime kst = DateTime.Now;
                        string sessCacheKey = transid + "-" + fldName + "-" + parentFlds + "-empty";
                        if (Session[sessCacheKey] != null)
                        {
                            result = Session[sessCacheKey].ToString();
                            var pickData = JsonConvert.DeserializeObject<pickdatalist>(result.ToString());
                            var rcount = pickData.pickdata[0].rcount;
                            var fname = pickData.pickdata[1].fname;
                            var dfname = pickData.pickdata[2].dfname;
                            var data = pickData.pickdata[3].data;
                            int rows = 50;
                            int totalRecords = int.Parse(rcount) < fetchRCount ? int.Parse(rcount) : fetchRCount;
                            int totalPages = totalRecords / rows;
                            int perPage = pgNumber > totalPages ? totalRecords % rows : rows;
                            string json = new JavaScriptSerializer().Serialize(data.GetRange(50 * (pgNumber - 1), perPage));
                            result = "{\"pickdata\":[{\"rcount\":\"" + rcount + "\"},{\"fname\":\"" + fname + "\"},{\"dfname\":\"" + dfname + "\"},{\"data\":" + json + "}]}";
                            isCache = true;
                            if (_tstKeyChanged != "")
                                result = _tstKeyChanged + "♣*♣" + result;
                            result = result + "♠" + strRequest + ObjExecTr.KernelProcessTime(kst, "GetSearchResult-fromSession", "", result);
                            return result;
                        }
                    }
                    else if (fastdll == "False" && fldValue.Length < 2)//Fetching data from session for Picklist field if the input is 1 or 2 character. that means filterig empty picklist session values if the same session is empty user will get this message "Please enter minimum of 2 character to search".
                    {
                        result = "{\"pickdata\":[{\"rcount\":\"0\"},{\"fname\":\"" + fldName + "\"},{\"data\":[{\"i\":\"Please enter minimum of 2 characters to search\",\"v\":\"\",\"d\":\"\"}]}]}";
                        if (_tstKeyChanged != "")
                            result = _tstKeyChanged + "♣*♣" + result;
                        return result;
                    }
                    else if (fastdll == "False" && fldValue.Length >= 2)//Fetching data from session for Picklist field and input values is >=2 characters, if not match the pattern of input will delete the existing session key and will create new session key with matched data.
                    {
                        //string strRequest = ObjExecTr.RequestProcessTime("Request");
                        strRequest += ObjExecTr.KernelProcessTime(startkst, "Server side process before data ", "", "");
                        DateTime kst = DateTime.Now;
                        string SessPickKey = transid + "-" + fldName + "-" + parentFlds + "-♦";
                        if (Session[SessPickKey] != null)
                        {

                            bool isPickCache = false, issvChanged = false;
                            string SCacheKey = transid + "-" + fldName + "-" + parentFlds + "-♦" + fldValue;
                            string SessPickKeyValue = Session[SessPickKey].ToString();
                            if (SessPickKeyValue == SCacheKey)
                                isPickCache = true;
                            else if (fldValue.Contains(SessPickKeyValue.Split('♦')[1]))//(fldValue.StartsWith(SessPickKeyValue.Split('♦')[1]))
                            {
                                issvChanged = true;
                                isPickCache = true;
                            }
                            else
                            {
                                Session.Remove(SessPickKeyValue);
                                Session.Remove(SessPickKey);
                            }
                            if (isPickCache)
                            {
                                result = Session[SessPickKeyValue].ToString();
                                var pickData = JsonConvert.DeserializeObject<pickdatalist>(result.ToString());
                                var tolrcount = long.Parse(pickData.pickdata[0].rcount);
                                var fname = pickData.pickdata[1].fname;
                                var dfname = pickData.pickdata[2].dfname;
                                var data = pickData.pickdata[3].data;
                                string spcfldValue = string.Empty;
                                if (issvChanged == true)
                                {
                                    spcfldValue = fldValue.Replace(SessPickKeyValue, "");
                                    var splFldVals = spcfldValue.Split('%');
                                    for (int j = 0; j < splFldVals.Length; j++)
                                    {
                                        var splFldVal = splFldVals[j].Split('_');
                                        for (int k = 0; k < splFldVal.Length; k++)
                                        {
                                            data = data.AsEnumerable().Where(x => x.i.ToLower().Contains(splFldVal[k].ToLower())).ToList();
                                        }
                                    }
                                }
                                int rcount = data.Count();
                                if (tolrcount == fetchRCount && rcount == 0 && issvChanged == true)
                                {
                                    Session.Remove(SessPickKeyValue);
                                    Session.Remove(SessPickKey);
                                }
                                else if (tolrcount == fetchRCount && issvChanged == true && (spcfldValue.Contains("%") || spcfldValue.Contains("_")))
                                {
                                    Session.Remove(SessPickKeyValue);
                                    Session.Remove(SessPickKey);
                                }
                                else
                                {
                                    int rows = 50;
                                    int totalRecords = rcount < fetchRCount ? rcount : fetchRCount;
                                    int totalPages = totalRecords / rows;
                                    int perPage = pgNumber > totalPages ? totalRecords % rows : rows;
                                    string json = new JavaScriptSerializer().Serialize(data.GetRange(50 * (pgNumber - 1), perPage));
                                    result = "{\"pickdata\":[{\"rcount\":\"" + rcount + "\"},{\"fname\":\"" + fname + "\"},{\"dfname\":\"" + dfname + "\"},{\"data\":" + json + "}]}";
                                    isCache = true;
                                    if (_tstKeyChanged != "")
                                        result = _tstKeyChanged + "♣*♣" + result;
                                    result = result + "♠" + strRequest + ObjExecTr.KernelProcessTime(kst, "GetSearchResult-fromSession", "", result);
                                    return result;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                if (IsApiFld == "true")
                {
                    result = GetDataFromApi(fldName, tstData.tstStrObj, tstData.fieldValueXml);
                }
                else
                {
                    logobj.CreateLog("AutoComplete from service.", sessionid, "autocomplete-" + fldName, "");
                    string activePRow = string.Empty; string activePDc = string.Empty;
                    if (pageData != string.Empty)
                    {
                        int idx = pageData.IndexOf("♠");
                        if (idx != -1)
                        {
                            activePDc = pageData.Substring(0, idx);
                            if (!string.IsNullOrEmpty(activePDc) && activePDc != "")
                                activePDc = activePDc.Split('~').Last();
                            activePRow = pageData.Substring(idx + 1);
                            if (!string.IsNullOrEmpty(activePRow) && activePRow != "")
                                activePRow = activePRow.Split('~').First();
                        }
                    }
                    string spfldValue = utilObj.CheckSpecialChars(fldValue);
                    string iXml = string.Empty;
                    string aucSearchPattern = "contains";
                    if (HttpContext.Current.Session["AxpautoCompPatt"] != null && HttpContext.Current.Session["AxpautoCompPatt"].ToString() != "")
                        aucSearchPattern = HttpContext.Current.Session["AxpautoCompPatt"].ToString();
                    iXml += "<sqlresultset activerow='" + pgData[3] + "' pdc='" + activePDc + "' prow='" + activePRow + "' frameno='" + pgData[2] + "' axpapp='" + project + "' value='" + spfldValue + "' sessionid= '" + sessionid + "' field='" + fldName + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'";
                    iXml += " sqlfield='' trace='" + errorLog + "' transid='" + transid + "' pageno='" + pgData[0] + "' cond='" + aucSearchPattern + "'";//cond='starts with'
                    iXml += " pagesize='" + pgData[1] + "'>";
                    logobj.CreateLog("Input xml-" + iXml, sessionid, "autocomplete-" + fldName, "");

                    if (ddlFldSqlParams == "0")
                    {
                        iXml += tstData.GetFieldSqlNonParams(fldName);

                        iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString();
                        iXml += "</sqlresultset>";
                        iXml = utilObj.CheckSplChrInputXML(iXml);
                    }
                    else
                    {
                        iXml += paramiXml;
                        iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString();
                        iXml += "</sqlresultset>";
                        iXml = utilObj.CheckSplChrInputXML(iXml);
                    }

                    string strRequestWs = ObjExecTr.KernelProcessTime(startkst, "Server side process before data ", "", "");
                    DateTime kst = DateTime.Now;

                    result = asbExt.CallAutoGetSearchNewWS(transid, iXml, tstData.tstStrObj.structRes);
                    requestProcess_logtime = strRequest + strRequestWs + result.Split('♠')[0];
                    result = result.Split('♠')[1];
                }
                logobj.CreateLog("AutoComplete from CallAutoGetSearchNewWS.-" + result, sessionid, "autocomplete-" + fldName, "");

                if (utils.ParseJSonErrorNode(result) != "")//To check is there error node.
                {
                    if (_tstKeyChanged != "")
                        result = _tstKeyChanged + "♣*♣" + result;
                    return result;
                }
                try
                {
                    if (ParseJSonResultNode(result))// To check is result is empty or not.
                    {
                        if (IsApiFld != "true")
                        {
                            if (fastdll == "True" && rfSave != "True" && isCache == false && result != "" && pickArrow == "False")//Storing combobox values in redis with parent field key pair.
                            {
                                string schemaName = string.Empty;
                                if (HttpContext.Current.Session["dbuser"] != null)
                                    schemaName = HttpContext.Current.Session["dbuser"].ToString();

                                FDW fdwObj = new FDW();
                                string fldkey = utilObj.GetRedisServerFieldkey(tstData.transid, Constants.FIELDNAME, fldName, 0);
                                fdwObj.HashSetKey(fldkey, parentFlds, result);
                            }
                            else if (fastdll == "True" && rfSave == "True" && isCache == false && result != "" && pickArrow == "False")//Saving combobox+refreshonsave field values in session with parent field key pair.
                            {
                                string sessCacheKey = transid + "-" + fldName + "-" + parentFlds;
                                Session[sessCacheKey] = result;
                                utilObj.AddKeyOnRefreshSave(sessCacheKey);
                            }
                            else if (fastdll == "False" && fldValue == "" && isCache == false && result != "" && pickArrow == "False")//Saving picklist field values in session with parent field key pair if the input value is empty.
                            {
                                string sessCacheKey = transid + "-" + fldName + "-" + parentFlds + "-empty";
                                Session[sessCacheKey] = result;
                                if (rfSave == "True")
                                    utilObj.AddKeyOnRefreshSave(sessCacheKey);
                                var pickData = JsonConvert.DeserializeObject<pickdatalist>(result.ToString());
                                var rcount = pickData.pickdata[0].rcount;
                                var fname = pickData.pickdata[1].fname;
                                var dfname = pickData.pickdata[2].dfname;
                                var data = pickData.pickdata[3].data;
                                int rows = 50;
                                int totalRecords = int.Parse(rcount) < fetchRCount ? int.Parse(rcount) : fetchRCount;
                                int totalPages = totalRecords / rows;
                                int perPage = pgNumber > totalPages ? totalRecords % rows : rows;
                                string json = new JavaScriptSerializer().Serialize(data.GetRange(50 * (pgNumber - 1), perPage));
                                result = "{\"pickdata\":[{\"rcount\":\"" + rcount + "\"},{\"fname\":\"" + fname + "\"},{\"dfname\":\"" + dfname + "\"},{\"data\":" + json + "}]}";
                            }
                            else if (fastdll == "False" && fldValue != "" && isCache == false && result != "" && pickArrow == "False")//Saving picklist field values in session with parent field key pair if the input value lenght is greater than or equal to(>=) 3 characters.
                            {
                                string SessCacheKey = transid + "-" + fldName + "-" + parentFlds + "-♦" + fldValue;
                                string SessPickKey = transid + "-" + fldName + "-" + parentFlds + "-♦";
                                Session[SessCacheKey] = result;
                                Session[SessPickKey] = SessCacheKey;
                                if (rfSave == "True")
                                {
                                    utilObj.AddKeyOnRefreshSave(SessCacheKey);
                                    utilObj.AddKeyOnRefreshSave(SessPickKey);
                                }
                                var pickData = JsonConvert.DeserializeObject<pickdatalist>(result.ToString());
                                var rcount = pickData.pickdata[0].rcount;
                                var fname = pickData.pickdata[1].fname;
                                var dfname = pickData.pickdata[2].dfname;
                                var data = pickData.pickdata[3].data;
                                int rows = 50;
                                int totalRecords = int.Parse(rcount) < fetchRCount ? int.Parse(rcount) : fetchRCount;
                                int totalPages = totalRecords / rows;
                                int perPage = pgNumber > totalPages ? totalRecords % rows : rows;
                                string json = new JavaScriptSerializer().Serialize(data.GetRange(50 * (pgNumber - 1), perPage));
                                result = "{\"pickdata\":[{\"rcount\":\"" + rcount + "\"},{\"fname\":\"" + fname + "\"},{\"dfname\":\"" + dfname + "\"},{\"data\":" + json + "}]}";
                            }

                            if (parentFldNames != string.Empty)
                            {
                                FDW fdwObj = new FDW();
                                string fldkey = utilObj.GetRedisServerFieldkey(tstData.transid, Constants.FIELDPARAMNAMES, fldName, 0);
                                fdwObj.HashSetKey(fldkey, fldName, parentFldNames);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            result = result + "♥" + parentFldNames + "♠" + requestProcess_logtime;
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♣*♣" + result;
            return result;
        }

        public bool ParseJSonResultNode(string result)
        {
            var pickData = JsonConvert.DeserializeObject<pickdatalist>(result.ToString());
            if (pickData.pickdata != null && pickData.pickdata.Count() > 0)
                return true;
            else
                return false;
        }

        public class pickdatalist
        {
            public List<pickdata> pickdata { get; set; }
        }

        public class pickdata
        {
            public string rcount { get; set; }
            public string fname { get; set; }
            public string dfname { get; set; }
            public List<fieldValues> data { get; set; }
        }

        public class fieldValues
        {
            public string i { get; set; }
            public string v { get; set; }
            public string d { get; set; }

        }

        public string GetDataFromApi(string fldName, TStructDef tstStrObj, string fieldValueXml)
        {
            string result = string.Empty;
            int fldidx = tstStrObj.GetFieldIndex(fldName);
            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)tstStrObj.flds[fldidx];

            string api = string.Empty;
            string mapname = string.Empty;
            string apiType = string.Empty;
            string contType = string.Empty;
            string contBody = string.Empty;

            string _apiname = fld.fieldSelAPIName;
            if (_apiname != null && _apiname != string.Empty)
            {
                try
                {
                    string _apiData = GetFromAPIInfo(_apiname, fldName, tstStrObj.transId);
                    JArray returnArray = new JArray();
                    JObject resultRows = JObject.Parse(_apiData);
                    if (_apiData != string.Empty)
                    {
                        JArray _apiEle = (JArray)resultRows["result"]["row"];
                        foreach (JObject row in _apiEle)
                        {
                            JObject modifiedRow = new JObject();
                            foreach (var property in row.Properties())
                            {
                                if (property.Name.ToLower() == "execapiurl")
                                    api = property.Value.ToString();
                                if (property.Name.ToLower() == "apiresponsetype")
                                    apiType = property.Value.ToString();
                                if (property.Name.ToLower() == "apiresponseformat")
                                    contType = property.Value.ToString();
                                if (property.Name.ToLower() == "execapirequeststring")
                                    contBody = property.Value.ToString();
                            }
                        }
                    }

                    string apiParams = fld.fieldSelAPIParams;
                    string[] strDetails = apiParams.Split('♠');
                    mapname = strDetails[0];
                }
                catch (Exception ex) { }
            }
            else
            {
                api = fld.fieldSelAPI;
                string apiParams = fld.fieldSelAPIParams;
                string[] strDetails = apiParams.Split('♠');
                mapname = strDetails[0];
                apiType = strDetails[1];
                contType = strDetails[2];
                contBody = strDetails[5];
            }
            if (api == string.Empty)
            {
                return "";
            }

            ArrayList fromApiName = new ArrayList();
            ArrayList fromApiExp = new ArrayList();
            ArrayList dfnames = tstStrObj.fldAcceptFromApi;

            if (apiType == "Axpert")
            {
                if (Session["ARM_Scripts_URL"] != null && Session["ARM_Scripts_URL"].ToString() != "")
                {
                    string _restdllpath = Session["ARM_Scripts_URL"].ToString();
                    if (_restdllpath.Substring(_restdllpath.Length - 1) != "/")
                        _restdllpath += "/";
                    string[] _apiPath = api.Split(new[] { "ASBTStructRest.dll" }, StringSplitOptions.None);
                    if (_apiPath.Length == 1 && _apiPath[0].IndexOf("ASBMenuRest.dll/") > -1)
                    {
                        string[] _apiPathTst = api.Split(new[] { "ASBMenuRest.dll" }, StringSplitOptions.None);
                        api = _restdllpath + "ASBMenuRest.dll" + _apiPathTst[1];
                    }
                    else
                        api = _restdllpath + "ASBTStructRest.dll" + _apiPath[1];
                }
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);

            string DATA = string.Empty;
            if (apiType == "Axpert")
            {
                request.Method = "POST";
                request.ContentType = "application/" + contType;
                DATA = contBody;// strDetails[5];
                DATA = ParseAxpertApiParams(DATA, "sqlparams", fieldValueXml, tstStrObj);
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();
            }
            else
            {
                request.Method = "GET";
                request.ContentType = "application/" + contType;
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                result = responseReader.ReadToEnd();
                Console.Out.WriteLine(result);
                responseReader.Close();
                //apiType = "external";
                //result = "{\"result\":[{\"fname\": \"username\"}, {\"dfname\": \"fpbuild^ftest2^fatest\"}, {\"data\": [{\"i\": \"test1\",\"d\": \"a^b^c\"}, {\"i\": \"test2\",\"d\": \"e^f^g\"}, {\"i\": \"test3\",\"d\": \"h^i^j\"}]}]}";
                if (result != string.Empty && apiType.ToLower() == "external")
                {
                    try
                    {
                        if (dfnames.Count > 0)
                        {
                            foreach (var dfN in dfnames)
                            {
                                if (dfN.ToString().Split('^').Length > 2)
                                {
                                    string faName = dfN.ToString().Split('^')[0];
                                    string isFromApi = dfN.ToString().Split('^')[1];
                                    string fromapiExp = dfN.ToString().Split('^')[2];
                                    if (isFromApi == "true" && fromapiExp != "" && fromapiExp.Split('~').Length > 0)
                                    {
                                        if (fromapiExp.Split('~')[0] == fldName)
                                        {
                                            fromApiName.Add(faName);
                                            if (mapname != "" && fromapiExp.Split('~').Length > 1)
                                                fromApiExp.Add(fromapiExp.Split('~')[1]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        fromApiName = new ArrayList();
                        fromApiExp = new ArrayList();
                    }
                    result = ApiResultParser(result, contType, mapname, fldName, fromApiName, fromApiExp);
                }
                else if (result != string.Empty)
                {
                    JObject obj = JObject.Parse(result);
                    result = obj["result"].First.ToString();
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("AutoComplete from API.-" + ex.Message, sessionid, "FromApi-" + fldName, "new");
            }
            return result;
        }

        public string GetFromAPIInfo(string _apiname, string _fieldName, string _transid)
        {
            string _apiData = string.Empty;
            try
            {
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                _apiData = fdrObj.StringFromRedis(utilObj.GetRedisServerkey(Constants.REDISTSTFLDFROMAPI, _transid, _fieldName + "-" + _apiname));
                if (_apiData == string.Empty)
                {
                    string sqlInput = "select execapidefname,execapiurl,apiresponsetype,apiresponseformat,execapirequeststring from EXECUTEAPIDEF where execapidefname = '" + _apiname + "'";
                    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                    _apiData = asbExt.ExecuteSQL("", sqlInput, "JSON");
                    if (_apiData != string.Empty)
                    {
                        FDW fdwObj = new FDW();
                        fdwObj.SaveInRedisServer(utilObj.GetRedisServerkey(Constants.REDISTSTFLDFROMAPI, _transid, _fieldName + "-" + _apiname), _apiData, Constants.REDISTSTFLDFROMAPI, schemaName);
                    }
                }
            }
            catch (Exception ex) { }
            return _apiData;
        }

        private string ApiResultParser(string response, string contType, string mapname, string fldName, ArrayList fromApiName, ArrayList fromApiExp)
        {
            string result = string.Empty;
            try
            {
                if (contType.ToLower() == "json")
                {
                    JObject obj = JObject.Parse(response);
                    if (obj.ToString().Contains("\"dfname\":"))
                    {
                        result = obj.ToString();
                    }
                    else
                    {
                        string json = string.Empty;
                        int ic = 0;
                        if (mapname != "")
                        {
                            foreach (var attNode in obj["data"])
                            {
                                string strValue = attNode[mapname].ToString();
                                if (fromApiName.Count > 0 && fromApiExp.Count > 0)
                                {
                                    string fromApiExpVal = "";
                                    foreach (var fax in fromApiExp)
                                    {
                                        fromApiExpVal += attNode[fax].ToString() + "^";
                                    }
                                    json += "{\"i\":\"" + strValue + "\",\"v\":\"\",\"d\":\"" + fromApiExpVal.TrimEnd('^') + "\"},";
                                }
                                else
                                    json += "{\"i\":\"" + strValue + "\",\"v\":\"\"},";
                                ic++;
                            }
                        }
                        else
                        {
                            //JObject ooBj = JObject.Parse("{\"AED\": \"United Arab Emirates Dirham\",\"AFN\": \"Afghan Afghani\",\"ALL\": \"Albanian Lek\",\"AMD\": \"Armenian Dram\",\"ANG\": \"Netherlands Antillean Guilder\"}");                       
                            //Considering first position of json 
                            foreach (var kval in obj)
                            {
                                if (fromApiName.Count > 0)
                                    json += "{\"i\":\"" + kval.Key + "\",\"v\":\"\",\"d\":\"" + kval.Value.ToString() + "\"},";
                                else
                                    json += "{\"i\":\"" + kval.Key + "\",\"v\":\"\"},";
                                ic++;
                            }
                        }
                        result = "{\"pickdata\":[{\"rcount\":\"" + ic + "\"},{\"fname\":\"" + fldName + "\"},{\"dfname\":\"" + string.Join("^", fromApiName.ToArray()) + "\"},{\"data\":[" + json.TrimEnd(',') + "]}]}";
                    }
                }
                else if (contType == "xml")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(response);
                    XmlNodeList elemList = doc.SelectNodes("//root/data/element");
                    string json = string.Empty;
                    int ic = 0;
                    foreach (XmlNode attNode in elemList)
                    {
                        foreach (XmlNode chlNode in attNode.ChildNodes)
                        {
                            if (chlNode.Name == mapname)
                            {
                                json += "{\"i\":\"" + chlNode.InnerText + "\",\"v\":\"\"},";
                                ic++;
                                break;
                            }
                        }

                    }
                    result = "{\"pickdata\":[{\"rcount\":\"" + ic + "\"},{\"fname\":\"" + fldName + "\"},{\"dfname\":\"\"},{\"data\":[" + json.TrimEnd(',') + "]}]}";
                }
                else if (contType == "csvl")
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }

        private string ParseAxpertApiParams(string RequestData, string paramsNodeName, string fieldValueXml, TStructDef tstStrObj, bool isAxAutoIntelli = false)
        {
            string parseData = string.Empty;
            string paramKey = string.Empty;
            try
            {
                JObject obj = JObject.Parse(RequestData);
                string json = string.Empty;

                string fldValues = "<fldxml>" + fieldValueXml + "</fldxml>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(fldValues);

                string globalvars = HttpContext.Current.Session["axGlobalVars"].ToString();
                XmlDocument xmlDocgbl = new XmlDocument();
                xmlDocgbl.LoadXml(globalvars);

                string blgAppVar = string.Empty;
                if (xmlDocgbl.SelectSingleNode("globalvars/appvartypes") != null)
                    blgAppVar = xmlDocgbl.SelectSingleNode("globalvars/appvartypes").InnerXml;

                string uservars = HttpContext.Current.Session["axUserVars"].ToString();
                XmlDocument xmlDocul = new XmlDocument();
                xmlDocul.LoadXml(uservars);

                foreach (JProperty attNode in obj["_parameters"][0][paramsNodeName])
                {
                    if (attNode.Name == "recordid" && xmlDoc.SelectSingleNode("/fldxml/axp_recid1") != null)
                    {
                        string propVal = xmlDoc.SelectSingleNode("/fldxml/axp_recid1").InnerText;
                        RequestData = RequestData.Replace("\"" + attNode.Name + "\":\"\"", "\"" + attNode.Name + "\":\"" + propVal + "~n\"");
                        if (isAxAutoIntelli)
                            paramKey += attNode.Name + ":" + propVal + "♣";
                    }
                    else if (xmlDoc.SelectSingleNode("/fldxml/" + attNode.Name) != null && tstStrObj != null)
                    {
                        string propVal = xmlDoc.SelectSingleNode("/fldxml/" + attNode.Name).InnerText;
                        int fldidxs = tstStrObj.GetFieldIndex(attNode.Name);
                        TStructDef.FieldStruct flds = (TStructDef.FieldStruct)tstStrObj.flds[fldidxs];
                        string VarTypes = "c";
                        if (flds.datatype == "Character" || flds.datatype == "Text")
                            VarTypes = "c";
                        else if (flds.datatype == "Numeric")
                            VarTypes = "n";
                        else if (flds.datatype == "Date/Time")
                            VarTypes = "d";
                        RequestData = RequestData.Replace("\"" + attNode.Name + "\":\"\"", "\"" + attNode.Name + "\":\"" + propVal + "~" + VarTypes + "\"");
                        if (isAxAutoIntelli)
                            paramKey += attNode.Name + ":" + propVal + "♣";
                    }
                    else if (xmlDocgbl.SelectSingleNode("/globalvars/" + attNode.Name) != null)
                    {
                        string propVal = xmlDocgbl.SelectSingleNode("/globalvars/" + attNode.Name).InnerText;
                        string VarTypes = "c";
                        int gblIcnt = 1;
                        foreach (XmlNode parms in xmlDocgbl.ChildNodes[0].ChildNodes)
                        {
                            if (parms.Name == attNode.Name)
                            {
                                if (blgAppVar.Length >= gblIcnt)
                                    VarTypes = blgAppVar[gblIcnt - 1].ToString();
                                else if (blgAppVar.Length < gblIcnt)
                                    VarTypes = "c";
                                break;
                            }
                            gblIcnt++;
                        }
                        RequestData = RequestData.Replace("\"" + attNode.Name + "\":\"\"", "\"" + attNode.Name + "\":\"" + propVal + "~" + VarTypes + "\"");
                        if (isAxAutoIntelli)
                            paramKey += attNode.Name + ":" + propVal + "♣";
                    }
                    else if (xmlDocul.SelectSingleNode("/uservars/" + attNode.Name) != null)
                    {
                        string propVal = xmlDocul.SelectSingleNode("/uservars/" + attNode.Name).InnerText;
                        RequestData = RequestData.Replace("\"" + attNode.Name + "\":\"\"", "\"" + attNode.Name + "\":\"" + propVal + "~c\"");
                        if (isAxAutoIntelli)
                            paramKey += attNode.Name + ":" + propVal + "♣";
                    }
                }
                if (isAxAutoIntelli)
                    parseData = RequestData + '♠' + paramKey;
                else
                    parseData = RequestData;
            }
            catch (Exception ex)
            {
                parseData = RequestData;
            }
            return parseData;
        }

        public string GetFieldChoices(string transid, string inputXml, string strucXml, string key)
        {
            string result = string.Empty;
            result = asbExt.CallFastFieldChoicesWS(transid, inputXml, strucXml);
            return result;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public bool SetListViewDictionary(string[] listviewRecords, int viewRecordKey, string transId)
        {
            Dictionary<int, string> lvRecordListing = new Dictionary<int, string>();
            int index = 0;
            foreach (string navPath in listviewRecords)
            {
                lvRecordListing.Add(++index, navPath);
            }
            utils.SetlvRecordList(transId, lvRecordListing);
            Session["lvRecordKey"] = viewRecordKey;
            Session["OpenedRecord-" + transId] = viewRecordKey;
            return true;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public string GetListViewNext(string transId, string key, string resTstHtmlLS)
        {
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "" && HttpContext.Current.Session[key] == null)
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string recId = tstData.recordID;
            Dictionary<int, string> lvRecordListing = new Dictionary<int, string>();
            lvRecordListing = utils.GetlvRecordList(transId);
            string returnQuery = string.Empty;
            int recordKey = 0;
            try
            {
                recordKey = Convert.ToInt32(Session["lvRecordKey"]);

                KeyValuePair<int, string> recordkeyValuePair = new KeyValuePair<int, string>();

                while ((recordkeyValuePair.Key == 0 || recordkeyValuePair.Value == "") && lvRecordListing.Count > recordKey)
                {
                    recordKey++;
                    recordkeyValuePair = lvRecordListing.SingleOrDefault(x => x.Key == recordKey && x.Value != null);
                }
                Session["lvRecordKey"] = recordKey;

                returnQuery = recordkeyValuePair.Value;
                if (returnQuery == null)
                {
                    returnQuery = string.Empty;
                }
            }
            catch (Exception ex) { }

            if (string.IsNullOrEmpty(returnQuery))
                return "";

            if (returnQuery.ToLower().IndexOf("tstruct.aspx") > -1)
            {
                if (_tstKeyChanged != "")
                    returnQuery = _tstKeyChanged + "♠*♠" + returnQuery;
                return returnQuery;
            }
            else
            {
                string pageType = "";
                if (recordKey == 1)
                    pageType = "first";
                else if (recordKey == lvRecordListing.Count)
                    pageType = "last";
                else
                    pageType = "middle";

                if (_tstKeyChanged != "")
                    return _tstKeyChanged + "♠*♠" + "tstruct.aspx?transid=" + transId + "&recordid=" + returnQuery + "&hltype=load&torecid=false&recPos=" + returnQuery.ToString() + "&curPage=1&pageType=" + pageType + "&openerIV=" + transId + "&isIV=false&isDupTab=false";
                else
                    return "tstruct.aspx?transid=" + transId + "&recordid=" + returnQuery + "&hltype=load&torecid=false&recPos=" + recordKey.ToString() + "&curPage=1&pageType=" + pageType + "&openerIV=" + transId + "&isIV=false&isDupTab=false";
            }

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public string GetListViewPrev(string transId, string key, string resTstHtmlLS)
        {
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "" && HttpContext.Current.Session[key] == null)
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string recId = tstData.recordID;
            Dictionary<int, string> lvRecordListing = new Dictionary<int, string>();
            lvRecordListing = utils.GetlvRecordList(transId);
            string returnQuery = string.Empty;
            int recordKey = 0;
            try
            {

                recordKey = Convert.ToInt32(Session["lvRecordKey"]);

                KeyValuePair<int, string> recordkeyValuePair = new KeyValuePair<int, string>();

                while ((recordkeyValuePair.Key == 0 || recordkeyValuePair.Value == "") && recordKey > 1)
                {
                    recordKey--;
                    recordkeyValuePair = lvRecordListing.SingleOrDefault(x => x.Key == recordKey && x.Value != null);
                }
                Session["lvRecordKey"] = recordKey;

                returnQuery = recordkeyValuePair.Value;
                if (returnQuery == null)
                {
                    returnQuery = string.Empty;
                }
            }
            catch (Exception ex) { }

            if (string.IsNullOrEmpty(returnQuery))
                return "";

            if (returnQuery.ToLower().IndexOf("tstruct.aspx") > -1)
            {
                if (_tstKeyChanged != "")
                    returnQuery = _tstKeyChanged + "♠*♠" + returnQuery;
                return returnQuery;
            }
            else
            {
                string pageType = "";
                if (recordKey == 1)
                    pageType = "first";
                else if (recordKey == lvRecordListing.Count)
                    pageType = "last";
                else
                    pageType = "middle";

                if (_tstKeyChanged != "")
                    return _tstKeyChanged + "♠*♠" + "tstruct.aspx?transid=" + transId + "&recordid=" + returnQuery + "&hltype=load&torecid=false&recPos=" + returnQuery.ToString() + "&curPage=1&pageType=" + pageType + "&openerIV=" + transId + "&isIV=false&isDupTab=false";
                else
                    return "tstruct.aspx?transid=" + transId + "&recordid=" + returnQuery + "&hltype=load&torecid=false&recPos=" + recordKey.ToString() + "&curPage=1&pageType=" + pageType + "&openerIV=" + transId + "&isIV=false&isDupTab=false";
            }

        }

        public string GetListViewRecordsDictionary(string transId, string type, int pageNo)
        {
            string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            Dictionary<int, string> lvRecordListing = new Dictionary<int, string>();
            lvRecordListing = utils.GetlvRecordList(transId);

            string ires = string.Empty;
            string iXml = string.Empty;
            string fileName = "GetNextRecords-" + transId;
            string errorLog = logobj.CreateLog("GetNextRecords.", Session.SessionID.ToString(), fileName, "new");
            iXml = "<root name =\"" + transId + "\"  custviewname=\"" + transId + "\" axpapp = \"" + Session["Project"].ToString() + "\" sessionid = \"" + Session.SessionID.ToString() + "\" appsessionkey=\"" + HttpContext.Current.Session["AppSessionKey"].ToString() + "\" username=\"" + HttpContext.Current.Session["username"].ToString() + "\" trace = \"" + errorLog + "\" list=\"true\"  pageno=\"" + pageNo + "\" pagesize=\"" + Session["lvPageSize"].ToString() + "\" ><params>";
            iXml = iXml + "</params>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            ires = asbExt.CallGetLViewWS(transId, iXml, asbExt.asbIview.Timeout);


            string errMsg = string.Empty;
            errMsg = utils.ParseXmlErrorNode(ires);

            if (errMsg != string.Empty)
            {
                if (errMsg == Constants.SESSIONERROR)
                {
                    Session.RemoveAll();
                    Session.Abandon();
                    return "sess.aspx";
                }
                else
                {
                    return "err.aspx?errmsg=" + errMsg;
                }
            }
            else
            {
                logobj.CreateLog("Setting Lview components", Session.SessionID.ToString(), fileName, "");
                ires = _xmlString + ires;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(ires);

                //Remove headrow
                XmlNode rnode = default(XmlNode);
                rnode = xmlDoc.SelectSingleNode("//headrow");
                if (rnode != null)
                    rnode.ParentNode.RemoveChild(rnode);

                //Remove Comps
                XmlNode cNode = default(XmlNode);
                cNode = xmlDoc.SelectSingleNode("//comps");
                cNode.ParentNode.RemoveChild(cNode);

                XmlNode vNode = default(XmlNode);
                vNode = xmlDoc.SelectSingleNode("//customizeview");
                if (vNode != null)
                    vNode.ParentNode.RemoveChild(vNode);

                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                xmlDoc.WriteTo(xw);

                string nXml = null;
                nXml = sw.ToString();
                if (nXml != "<?xml version=\"1.0\" encoding=\"utf-8\"?><data></data>")
                {
                    ArrayList colType = new ArrayList();
                    DataSet ds = new DataSet();
                    StringReader sr = new StringReader(nXml);
                    try
                    {
                        ds.ReadXml(sr);
                    }
                    catch (Exception ex)
                    {
                        //CloseDimmerJS();
                        //Response.Redirect(util.ERRPATH + ex.Message);
                        return "err.aspx?errmsg=" + ex.Message;
                    }
                    int n = 0;
                    n = 0;
                    DataSet ds1 = new DataSet();
                    ds1 = ds.Clone();

                    int rno = 0;
                    rno = 0;
                    foreach (DataRow dr1 in ds.Tables[0].Rows)
                    {


                        ds1.Tables[0].ImportRow(dr1);
                        int recPos = (Convert.ToInt32(ds1.Tables[0].Rows[rno]["rowno"]) + ((Convert.ToInt32(pageNo) - 1) * Convert.ToInt32(Session["lvPageSize"].ToString())));

                        lvRecordListing.Add(recPos, ds1.Tables[0].Rows[rno]["recordid"].ToString());
                        rno = rno + 1;
                    }
                    utils.SetlvRecordList(transId, lvRecordListing);
                }
                return "";
            }

        }

        //For AutoFill on configuration page of ddlsource under globalization tab
        [WebMethod(EnableSession = true)]
        public string FormAutoComplete(string FldValue, string fltValue, string ddlLanguage)
        {
            string jsonstring = string.Empty;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/aspx/App_LocalResources"));
                var files = dir.GetFiles("*.aspx.resx");
                List<string> PageList = new List<string>();
                jsonstring = "[";
                foreach (var file in files)
                {
                    string statusfile = FileExistInternal(file.Name.Remove(file.Name.Length - 10), ddlLanguage);
                    if (file.Name.Remove(file.Name.Length - 10).ToLower().Contains(fltValue.ToLower()) || fltValue == "")
                    {
                        jsonstring += "{\"item\":\"" + file.Name.Remove(file.Name.Length - 10) + "\",\"status\":\"" + statusfile + "\"},";
                    }

                }
                if (jsonstring != "[")
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
                jsonstring += "]";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return jsonstring;
        }

        //Draw all content of a page in form of textarea on configuration page
        [WebMethod(EnableSession = true)]
        public string FileXml(string ddlsource, string ddlLanguage)
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                StreamReader SR;
                int count = 0;
                string ln = GetCultureInfo(ddlLanguage);
                string fname = ddlsource + ".aspx" + '.' + ln + ".resx";
                List<String> sbtarget = new List<String>();
                if (File.Exists(Server.MapPath("~/aspx/App_LocalResources/" + fname)))
                {
                    SR = File.OpenText(Server.MapPath("~/aspx/App_LocalResources/" + fname));
                    string strval = SR.ReadToEnd();
                    SR.Close();
                    if (!string.IsNullOrEmpty(strval))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(strval);
                        XmlNodeList resulttarget = xmlDoc.SelectNodes("/root/data");
                        foreach (XmlNode node in resulttarget)
                        {
                            sbtarget.Add(node.ChildNodes[1].InnerText);
                        }
                    }
                }

                SR = File.OpenText(Server.MapPath("~/aspx/App_LocalResources/" + ddlsource + ".aspx.resx"));
                string str = SR.ReadToEnd();
                SR.Close();
                if (!string.IsNullOrEmpty(str))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(str);
                    XmlNodeList result = xmlDoc.SelectNodes("/root/data");

                    if (sbtarget.Count <= 0)
                    {
                        foreach (XmlNode node in result)
                        {
                            count++;
                            sbContent.Append("<div class=\"row configformone\" runat=\"server\"><div class=\"col-sm-6\" runat=\"server\"><div class=\"row configformone\" runat=\"server\"><div class=\"col-sm-1\" runat=\"server\"><label class=\"rowserial\"> " + count + "</label></div><div class=\"col-sm-11\" runat=\"server\"><textarea id =\"" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"  name =\"" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"  class=\"form-control \" readonly=\"true\" title=\"" + node.ChildNodes[1].InnerText + "\" placeholder=\"" + node.ChildNodes[1].InnerText + "\" style=\"height: 30px\" value=\"" + node.ChildNodes[1].InnerText + "\">" + node.ChildNodes[1].InnerText + "</textarea></div></div></div><div runat =\"server\" class=\"col-sm-6\"><textarea id=\"txt" + node.Attributes[0].Value.ToString().Split('.')[0] + "\" name=\"txt" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"  class=\"form-control targettext \" style=\"height: 30px\" name=\"txt" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"></textarea></div><div class=\"col-sm-1\"></div></div>");
                        }
                    }
                    else
                    {
                        int i = 0;
                        foreach (XmlNode node in result)
                        {
                            count++;
                            if (i < sbtarget.Count)
                            {
                                sbContent.Append("<div class=\"row configformone\" runat=\"server\"><div class=\"col-sm-6\" runat=\"server\"><div class=\"row configformone\" runat=\"server\"><div class=\"col-sm-1\" runat=\"server\"><label class=\"rowserial\"> " + count + "</label></div><div class=\"col-sm-11\" runat=\"server\"><textarea id =\"" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"  name =\"" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"  class=\"form-control \" readonly=\"true\" title=\"" + node.ChildNodes[1].InnerText + "\" placeholder=\"" + node.ChildNodes[1].InnerText + "\" style=\"height: 30px\" value=\"" + node.ChildNodes[1].InnerText + "\">" + node.ChildNodes[1].InnerText + "</textarea></div></div></div><div runat =\"server\" class=\"col-sm-6\"><textarea id=\"txt" + node.Attributes[0].Value.ToString().Split('.')[0] + "\" name=\"txt" + node.Attributes[0].Value.ToString().Split('.')[0] + "\" class=\"form-control targettext \" style=\"height: 30px\" name=\"txt" + node.Attributes[0].Value.ToString().Split('.')[0] + "\" placeholder=\"" + sbtarget[i] + "\">" + sbtarget[i] + "</textarea></div><div class=\"col-sm-1\"></div></div>");
                                i++;
                            }
                            else
                            {
                                sbContent.Append("<div class=\"row configformone\" runat=\"server\"><div class=\"col-sm-6\" runat=\"server\"><div class=\"row configformone\" runat=\"server\"><div class=\"col-sm-1\" runat=\"server\"><label class=\"rowserial\"> " + count + "</label></div><div class=\"col-sm-11\" runat=\"server\"><textarea id =\"" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"  name =\"" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"  class=\"form-control \" readonly=\"true\" title=\"" + node.ChildNodes[1].InnerText + "\" placeholder=\"" + node.ChildNodes[1].InnerText + "\" style=\"height: 30px\" value=\"" + node.ChildNodes[1].InnerText + "\">" + node.ChildNodes[1].InnerText + "</textarea></div></div></div><div runat =\"server\" class=\"col-sm-6\"><textarea id=\"txt" + node.Attributes[0].Value.ToString().Split('.')[0] + "\" name=\"txt" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"  class=\"form-control targettext \" style=\"height: 30px\" name=\"txt" + node.Attributes[0].Value.ToString().Split('.')[0] + "\"></textarea></div><div class=\"col-sm-1\"></div></div>");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return sbContent.ToString();
        }

        //Check Is file before exist or note on configuration page for 

        public string FileExistInternal(string filename, string ddlLanguage)
        {
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/aspx/App_LocalResources"));
            try
            {
                string language = GetCultureInfo(ddlLanguage);
                var file = filename + ".aspx." + language + ".resx";
                if (File.Exists(Server.MapPath("~/aspx/App_LocalResources/" + file)))
                {
                    return "yes";
                }
                return "No";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public Boolean SaveLanFileData(string ddlSource, string ddlLanguage, string textvalue)
        {

            StreamReader SR;
            try
            {
                SR = File.OpenText(Server.MapPath("~/aspx/App_LocalResources/" + ddlSource + ".aspx.resx"));
                string str = SR.ReadToEnd();

                string[] dataval = textvalue.Split('♠');
                int lenghtdata = dataval.Length - 1;
                SR.Close();
                if (!string.IsNullOrEmpty(str))
                {

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(str);
                    XmlNodeList result = xmlDoc.SelectNodes("/root/data");
                    int i = 0;
                    string textval = string.Empty;
                    foreach (XmlNode node in result)
                    {
                        node.InnerXml = "<value>" + dataval[i] + "</value>";
                        i++;
                    }
                    string language = GetCultureInfo(ddlLanguage);
                    string filename = ddlSource;
                    filename = filename + ".aspx" + '.' + language + ".resx";
                    xmlDoc.Save(Server.MapPath("~/aspx/App_LocalResources/" + filename + ""));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string GetCultureInfo(string language)
        {
            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            for (int i = 0; i <= 732; i++)
            {
                if (cinfo[i].DisplayName.ToLower() == language.ToLower())
                {
                    return cinfo[i].Name;
                }
            }
            return string.Empty;
        }
        [WebMethod(EnableSession = true)]
        public string AddSessionPair(string key, string val)
        {
            HttpContext.Current.Session[key] = val;
            return "Session Added";
        }
        [WebMethod(EnableSession = true)]
        public string CompareLastUpdated()
        {
            if (System.Web.HttpContext.Current.Session["LastUpdatedSess"] != null)
            {
                DateTime lastUpdatedTime = DateTime.Parse(HttpContext.Current.Session["LastUpdatedSess"].ToString());
                DateTime currentTime = DateTime.Now;
                int deferenceTime = (int)(currentTime - lastUpdatedTime).TotalMinutes;
                return deferenceTime.ToString();
            }
            else
                return "Session does not exist";
        }

        [WebMethod(EnableSession = true)]
        public string KeepSignLastUpdated(int AliveTime, string currPageUrl)
        {
            try
            {
                if (HttpContext.Current.Session["LastUpdatedSess"] != null)
                {
                    if (Session["hybridDeviceId"] != null && Session["hybridDeviceId"].ToString() != "")
                    {
                        Session["KeepMeHybridExpiry"] = "true";
                        string devId = Session["hybridDeviceId"].ToString();
                        string hyProj = HttpContext.Current.Session["project"].ToString();
                        string devStr = "&deviceid=" + devId + "&projname=" + hyProj;
                        return devStr;
                    }
                    else
                    {
                        Session["KeepMeWebExpiry"] = "true";
                        utils.SaveKeepAliveWebDetails(currPageUrl);
                        return "web";
                    }
                }
                else
                    return "false";
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        [WebMethod(EnableSession = true)]
        public object GetSession(string key)
        {
            var session = HttpContext.Current.Session;

            if (session != null && key == "nsessionid" && session["nsessionid"] != null)
                return session["nsessionid"];
            else if (session != null && key == "nsessionid")
                return "Session does not exist";

            if (session != null && key == "appsessionkey" && session[key] != null)
            {
                string appsessKey = (session["lictype"] != null && session["lictype"].ToString() == "limited") ? "l~" : "u~";
                return appsessKey + session[key];
            }
            else if (session != null && session[key] != null)
            {
                return session[key];
            }
            else
                return "Session does not exist";
        }

        //Method to Set a value to a session or append value to a existing session with a delimiter
        [WebMethod(EnableSession = true)]
        public string SetSession(string key, string val, bool appendVal = false, string delimiter = "")
        {
            if (!appendVal || System.Web.HttpContext.Current.Session[key] == null || System.Web.HttpContext.Current.Session[key].ToString() == "")
            {
                System.Web.HttpContext.Current.Session[key] = val;
                return "Session Added";
            }
            else
            {
                System.Web.HttpContext.Current.Session[key] = System.Web.HttpContext.Current.Session[key].ToString() + delimiter + val;
                return "Value appended to Session";
            }
        }

        [WebMethod(EnableSession = true)]
        public string SetNodeApineeds(string key, string val)
        {
            System.Web.HttpContext.Current.Session[key] = val;
            return "added";
        }

        //Method to get the new tab page url
        [WebMethod(EnableSession = true)]
        public string GetNewTabPageSess(string key)
        {
            string pageUrl = String.Empty;
            if (!(System.Web.HttpContext.Current.Session[key] == null || System.Web.HttpContext.Current.Session[key].ToString() == ""))
            {
                pageUrl = System.Web.HttpContext.Current.Session[key].ToString();

                if (pageUrl.Contains('♠'))
                    System.Web.HttpContext.Current.Session[key] = pageUrl.Substring(pageUrl.IndexOf('♠') + 1);
                else
                    System.Web.HttpContext.Current.Session[key] = null;

                pageUrl = pageUrl.Split('♠')[0];
            }
            return pageUrl;
        }

        [WebMethod(EnableSession = true)]
        public string CallRestWS(string jsonData, string webServiceName)
        {
            string paramsNode = "";
            string[] resSplit = jsonData.Split(new[] { "*$*" }, StringSplitOptions.None);

            jsonData = resSplit[0];

            if (resSplit.Length > 1)
            {
                paramsNode = resSplit[1];
            }
            string _adafFieldsList = string.Empty;
            if (webServiceName.StartsWith("ValidateExpression#"))
            {
                _adafFieldsList = webServiceName.Split('#')[1];
                webServiceName = webServiceName.Split('#')[0];
            }

            string errlog = logobj.CreateLog("", HttpContext.Current.Session["nsessionid"].ToString(), webServiceName, "");
            if (Session["AxTrace"] != null && Session["AxTrace"].ToString() == "true")
            {
                string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                jsonData = jsonData.Replace("\"trace\":\"\"", "\"trace\":\"" + errlog.Replace("\\", "\\\\") + "\"");
                jsonData = jsonData.Replace("\"scriptpath\":\"\"", "\"scriptpath\":\"" + scriptsPath.Replace("\\", "\\\\") + "\"");
            }
            string response = string.Empty;
            string DATA = string.Empty;
            string URL = GetArmScriptURL() + "ASBDefineRest.dll/datasnap/rest/TASBDefineRest/" + webServiceName + "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            if (webServiceName.ToLower() == "savestruct")
            {
                string appsesskey = Session["AppSessionKey"].ToString();
                string globalvars = xmltojson(Session["axGlobalVars"].ToString());
                string uservars = xmltojson(Session["axUserVars"].ToString());
                string axapps = xmltojson(Session["axApps"].ToString());
                jsonData = jsonData.Remove(jsonData.Length - 2);
                DATA = jsonData + ",\"appsessionkey\":\" " + appsesskey + " \" " + ",\"globalvars\":{" + globalvars + "},\"uservars\":{" + uservars + "},\"axapps\":{" + axapps + "}}}";//appsessionkey
            }
            else
            {
                //DATA = jsonData;
                string appsesskey = Session["AppSessionKey"].ToString();
                string axprops = HttpContext.Current.Application["axProps"].ToString();
                axprops = xmltojson(axprops);
                string axapps = xmltojson(Session["AxApps"].ToString());
                string globalvar = xmltojson(Session["axGlobalVars"].ToString());
                string axappsNode = Session["project"].ToString();
                jsonData = jsonData.Substring(0, jsonData.Length - 4);
                if (_adafFieldsList == string.Empty)
                    DATA = jsonData + ",\"appsessionkey\":\"" + appsesskey + "\"},\"" + axappsNode + "\":{" + axapps + "},\"axprops\":{" + axprops + "},\"globalvars\":{" + globalvar + "}" + (paramsNode != string.Empty ? "," + paramsNode + "" : "") + "}]}";
                else
                {
                    string[] _userJson = _adafFieldsList.Split(',');
                    string _userVarResult = "{\n" + string.Join(",\n", _userJson.Select(x => string.Format("  \"{0}\": \"\"", x))) + "\n}";
                    DATA = jsonData + ",\"appsessionkey\":\"" + appsesskey + "\"},\"" + axappsNode + "\":{" + axapps + "},\"axprops\":{" + axprops + "},\"globalvars\":{" + globalvar + "},\"uservars\":" + _userVarResult + "" + (paramsNode != string.Empty ? "," + paramsNode + "" : "") + "}]}";
                }
            }
            try
            {
                DATA = utilObj.ServiceInputSpecialChars(DATA);
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
                return response;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
                return response;
            }

        }

        public string xmltojson(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc);
            var parsed = JObject.Parse(json);
            var jtoken = ((Newtonsoft.Json.Linq.JContainer)parsed.First).First;
            return jtoken.ToString().Trim('{').Trim('}');
        }

        //Method to Unlock ConfigApp Page
        [WebMethod(EnableSession = true)]
        public string UnlockConfigApp(bool forceUnlock = false)
        {
            string sessID = string.Empty;
            if (HttpContext.Current.Session["nsessionid"] != null)
                sessID = HttpContext.Current.Session["nsessionid"].ToString();

            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();

            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            string lockedBy = fdrObj.StringFromRedis(Constants.CONFIG_LOCK_KEY);
            string[] lockDetails = lockedBy.Split('♦');

            if ((lockDetails.Length == 4 && lockDetails[2] == sessID) || forceUnlock)
            {
                FDW fdwObj = new FDW();
                fdwObj.Initialize(schemaName);
                fdwObj.ClearRedisServerDataByKey(Constants.CONFIG_LOCK_KEY, Constants.CONFIG_LOCK_KEY, false, schemaName);
            }

            return "Unlocked Config App.";
        }

        //Method to take assocaited tstruct from Iview
        [WebMethod(EnableSession = true)]
        public string GetAssociatedTstruct(string key)
        {

            IviewData iviewObj = (IviewData)HttpContext.Current.Session[key];
            return iviewObj.AssociatedTStruct;
        }

        //Method to Refresh Menu

        [WebMethod(EnableSession = true)]
        public string RefreshMenu()
        {
            string sXml = string.Empty;
            string result1 = string.Empty;
            string lang_at = "";
            string errlog = logobj.CreateLog("Getting Menu", sessionid, "GetMultiMenu", "new");
            sXml = sXml + "<root menucached='false' axpapp='" + HttpContext.Current.Session["project"] + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errlog + "' mname =\"\" " + lang_at + " appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'";
            // if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
            //     sXml = sXml + " mobdev='yes'";
            sXml = sXml + "> ";
            sXml = sXml + Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString();
            sXml = sXml + "</root>";
            result1 = asbExt.CallGetMultiLevelMenuWS("main", sXml);
            if (result1 != string.Empty)
            {
                result1 = result1.Split('♠')[1];
                result1 = Regex.Replace(result1, ";fwdslh", "/");
                result1 = Regex.Replace(result1, ";hpn", "-");
                result1 = Regex.Replace(result1, ";bkslh", "\\");
                result1 = Regex.Replace(result1, ";eql", "=");
                result1 = Regex.Replace(result1, ";qmrk", "?");

                string fdKeyMenuData = Constants.REDISMENUDATA;
                string schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                FDW fdwObj = new FDW();
                fdwObj.SaveInRedisServer(utilObj.GetRedisServerkey(fdKeyMenuData, "Menu"), result1, Constants.REDISMENUDATA, schemaName);

                result1 = result1.Replace(@"\", ";bkslh");
                result1 = Regex.Replace(result1, "'", "&apos;");
                result1 = Regex.Replace(result1, "&quot;", " ");
            }
            return result1;
        }

        [WebMethod(EnableSession = true)]
        public string CreateLoginKey(string proj, string userName, string pwd, string seed, string language, string mobileDevice, string hybridGUID, string guid)
        {
            Cache cacObj = new Cache();
            HttpContext.Current.Cache.Insert(guid, proj + "~" + userName + "~" + pwd + "~" + seed + "~" + language + "~" + mobileDevice + "~" + hybridGUID);
            return "Session Added";
        }

        [WebMethod(EnableSession = true)]
        public bool FlushAllKeys()
        {
            bool clearRedis = false;
            FDR fObj = null;
            try
            {
                fObj = (FDR)Session["FDR"];
            }
            catch (Exception ex)
            {
                Session["FDR"] = null;
            }
            if (fObj == null)
            {
                fObj = new FDR();
                Session["FDR"] = fObj;
            }
            try
            {
                clearRedis = fObj.FlushAllRedisKeys();
            }
            catch (Exception ex) { }
            return clearRedis;
        }

        [WebMethod(EnableSession = true)]
        public void SetHybridInfo(dynamic hybridinfo)
        {
            string result = JsonConvert.SerializeObject(hybridinfo);
            var parsedResult = JObject.Parse(result);
            string strGUID = parsedResult.SelectToken("GUID").Value<string>().ToLower();
            FDR fObj = null;
            try
            {
                fObj = (FDR)Session["FDR"];
            }
            catch (Exception ex)
            {
                Session["FDR"] = null;
            }
            if (fObj == null)
            {
                fObj = new FDR();
                Session["FDR"] = fObj;
            }
            string locKey = Constants.AXHYBRIDINFO;
            Util.Util util;
            util = new Util.Util();
            FDW fdwObj = new FDW();
            fdwObj.SaveInRedisServer(util.GetRedisServerkey(locKey, strGUID), result, Constants.AXHYBRIDINFO, "♠");
            //var existingvalues = fObj.StringFromRedis(util.GetRedisServerkey(locKey, strGUID), "♠");
        }

        [WebMethod(EnableSession = true)]
        public string GetRedisString(string key, string id, string user = "ALL")
        {
            string redisValues = "";
            Util.Util util;
            util = new Util.Util();
            FDR fObj = null;
            string GUID = string.Empty;
            string schemaName = string.Empty;
            try
            {
                fObj = (FDR)Session["FDR"];
            }
            catch (Exception ex)
            {
                Session["FDR"] = null;
            }
            if (fObj == null)
            {
                fObj = new FDR();
                Session["FDR"] = fObj;
            }
            if (key == "hybridinfo")
            {
                key = Constants.AXHYBRIDINFO;
                schemaName = "♠";
            }
            else if (key == Constants.AXHYBRIDWEIGHTSCALEINFO)
            {
                schemaName = "♠";
            }
            try
            {
                redisValues = fObj.StringFromRedis(util.GetRedisServerkey(key, id, user), schemaName).ToString();
                if (key == Constants.AXHYBRIDWEIGHTSCALEINFO && redisValues != "")
                {
                    FDW fdwObj = new FDW();
                    fdwObj.ClearRedisServerDataByKey(util.GetRedisServerkey(key, id, user), "", false, schemaName);
                }
            }
            catch (Exception ex) { }

            return redisValues;
        }

        [WebMethod(EnableSession = true)]
        public void SetHybridNotifiInfo(dynamic notification_info)
        {
            string result = JsonConvert.SerializeObject(notification_info);
            var parsedResult = JObject.Parse(result);
            string strGUID = parsedResult.SelectToken("GUID").Value<string>().ToLower();
            FDR fObj = null;
            try
            {
                fObj = (FDR)Session["FDR"];
            }
            catch (Exception ex)
            {
                Session["FDR"] = null;
            }
            if (fObj == null)
            {
                fObj = new FDR();
                Session["FDR"] = fObj;
            }
            string notifiKey = Constants.AXHYBRIDNOTIFIINFO;
            Util.Util util;
            util = new Util.Util();
            FDW fdwObj = new FDW();
            fdwObj.SaveInRedisServer(util.GetRedisServerkey(notifiKey, strGUID), result, Constants.AXHYBRIDNOTIFIINFO, "♠");
            //logobj.CreateLog("SetHybridNotifiInfo-" + notification_info + " --strGUID--" + strGUID + "--result--" + result, sessionid, "SetHybridNotifiInfo", "new", "true");
        }

        [WebMethod(EnableSession = true)]
        public string GetHybridNotifiInfo(string id, string status)
        {
            string redisValues = "";
            Util.Util util;
            util = new Util.Util();
            FDR fObj = null;
            string GUID = string.Empty;
            string schemaName = string.Empty;
            try
            {
                fObj = (FDR)Session["FDR"];
            }
            catch (Exception ex)
            {
                Session["FDR"] = null;
            }
            if (fObj == null)
            {
                fObj = new FDR();
                Session["FDR"] = fObj;
            }
            string notifiKey = Constants.AXHYBRIDNOTIFIINFO;
            schemaName = "♠";
            try
            {
                redisValues = fObj.StringFromRedis(util.GetRedisServerkey(notifiKey, id), schemaName).ToString();
            }
            catch (Exception ex)
            {
                logobj.CreateLog("GetHybridNotifiInfo- --redisvalues--" + redisValues + " --Id--" + id + "--Exception--" + ex.Message, sessionid, "GetHybridNotifiInfo", "new");
                return "Redis error";
            }

            if (redisValues != "")
            {
                try
                {
                    dynamic mbNotify = JsonConvert.DeserializeObject(redisValues);
                    DBContext obj = new DBContext();
                    obj.SaveMobileNotification(mbNotify.GUID.Value, mbNotify.firebase_id.Value, mbNotify.imei_no.Value, status);
                    logobj.CreateLog("GetHybridNotifiInfo- --redisvalues--" + redisValues + " --Id--" + id + "--mbNotify--" + mbNotify + "--guid--" + mbNotify.GUID.Value + "--firebase_id--" + mbNotify.firebase_id.Value + "--imei_no--" + mbNotify.imei_no.Value, sessionid, "GetHybridNotifiInfoLog", "new");
                }
                catch (Exception ex)
                {
                    logobj.CreateLog("GetHybridNotifiInfo-" + redisValues + " --Id--" + id + "--Ex:" + ex.Message, sessionid, "GetHybridNotifiInfo", "new");
                    return "DB error";
                }
            }

            return "done";
        }

        [WebMethod(EnableSession = true)]
        public string ResetFormLoadCache(string transid)
        {
            string result = string.Empty;
            string schemaName = string.Empty;
            if (Session["SessResetFormLoad"] != null)
            {
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                var sKey = Session["SessResetFormLoad"].ToString().Split('¿').ToList();
                FDR fObj = (FDR)Session["FDR"];
                string flArray = utilObj.GetRedisServerFieldkey(transid, Constants.FORMLOADARRAY, "", 0);
                string strflKeys = fObj.StringFromRedis(flArray);
                var keyList = strflKeys.Split('¿').ToList();
                FDW fdwObj = new FDW();
                fdwObj.Initialize(schemaName);
                if (int.Parse(sKey[0]) != keyList.Count() - 1)
                {
                    string keyDelete = keyList[int.Parse(sKey[0])];
                    strflKeys = strflKeys.Replace(keyDelete, "deleted");
                    fdwObj.SaveInRedisServer(flArray, strflKeys, "", schemaName);
                }
                else if (int.Parse(sKey[0]) == keyList.Count() - 1 && keyList.Count() > 1)
                {
                    string keyDelete = keyList[int.Parse(sKey[0])];
                    strflKeys = strflKeys.Replace("¿" + keyDelete, "");
                    fdwObj.SaveInRedisServer(flArray, strflKeys, "", schemaName);
                }
                else if (keyList.Count() == 1)
                    fdwObj.ClearRedisServerDataByKey(flArray, "", false, schemaName);
                fdwObj.ClearRedisServerDataByKey(sKey[1], "", false, schemaName);
                string fdKey = Constants.REDISTSTRUCTDOFORM;
                fdwObj.ClearRedisServerDataByKey(utilObj.GetRedisServerkey(fdKey, transid), "", false, schemaName);
                result = "done";
            }
            return result;
        }

        public string GetMultiSelectValues(string tstDataId, string fldName, string fldValue, ArrayList ChangedFields, ArrayList ChangedFieldDbRowNo, ArrayList ChangedFieldValues, ArrayList DeletedDCRows, string fldNameMs, string refreshMs, string parentFlds, string rfSave, string pageData, string isTstHtmlLs)
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            string result = string.Empty;
            string iXml = string.Empty;
            GetGlobalVars();
            string[] pgData = pageData.Split('~');
            string activeRow = pgData[3];
            string errorLog = logobj.CreateLog("MultiSelect- GetMultiSelectValues.", sessionid, "MultiSelect-" + fldName, "new");
            string _tstKeyChanged = "";
            if (isTstHtmlLs != "")
            {
                utilObj.DeleteTstIvObject(tstDataId.Split('_')[0]);
                tstDataId = utilObj.GetReGenTstDataObj(tstDataId, isTstHtmlLs);
                _tstKeyChanged = tstDataId;
            }
            TStructData tstData = (TStructData)Session[tstDataId];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string activePRow = string.Empty; string activePDc = string.Empty;
            if (pageData != string.Empty)
            {
                int idx = pageData.IndexOf("♠");
                if (idx != -1)
                {
                    activePDc = pageData.Substring(0, idx);
                    if (!string.IsNullOrEmpty(activePDc) && activePDc != "")
                        activePDc = activePDc.Split('~').Last();
                    activePRow = pageData.Substring(idx + 1);
                    if (!string.IsNullOrEmpty(activePRow) && activePRow != "")
                        activePRow = activePRow.Split('~').First();
                }
            }
            iXml += "<sqlresultset activerow='" + pgData[3] + "' pdc='" + activePDc + "' prow='" + activePRow + "' frameno='" + pgData[2] + "' axpapp='" + project + "' value='' sessionid= '" + sessionid + "' field='" + fldName + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'";
            iXml += " sqlfield='' trace='" + errorLog + "' transid='" + tstData.transid + "' pageno='" + pgData[0] + "' cond='contains'";
            iXml += " pagesize='" + pgData[1] + "'>";
            tstData.GetFieldValueXml(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, "-1", "AutoComplete", "NG", tstData.tstStrObj.GetFieldDc(fldName).ToString());
            string SessAutKey = "MultiSelect♦" + tstData.transid + "-" + fldName + "-" + parentFlds;
            if (Session[SessAutKey] != null)
                iXml += Session[SessAutKey].ToString();
            else
            {
                var lstAutKeys = Session.Keys.Cast<string>().Where(x => x.StartsWith("MultiSelect♦")).ToList();
                if (lstAutKeys.Count > 0)
                    Session.Remove(lstAutKeys[0]);
                iXml += tstData.GetFieldSqlQuery(fldName, tstData.fieldValueXml, HttpContext.Current.Session["axGlobalVars"].ToString(), HttpContext.Current.Session["axUserVars"].ToString(), int.Parse(activeRow), parentFlds);
            }
            parentFlds = iXml.Split('♠')[1];
            iXml = iXml.Split('♠')[0];
            //string dbmemvarsXML = utilObj.GetDBMemVarsXML(tstData.transid);
            //string cdVarsXML = utilObj.GetConfigDataVarsXML(tstData.transid);
            iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString();
            iXml += "</sqlresultset>";
            iXml = utilObj.CheckSplChrInputXML(iXml);
            logobj.CreateLog("Input xml-" + iXml, sessionid, "MultiSelect-" + fldName, "");

            string strParent = string.Empty;
            bool isCache = false;
            try
            {
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (rfSave != "True" && refreshMs == "False")
                {
                    string fldkey = utilObj.GetRedisServerFieldkey(tstData.transid, Constants.FIELDNAME, fldName, 0);
                    string dllData = fObj.HashGetKey(fldkey, parentFlds);
                    logobj.CreateLog("Data from Redis.-" + dllData, sessionid, "MultiSelect-" + fldName, "");
                    if (dllData != string.Empty && refreshMs == "False")
                    {
                        result = dllData;
                        isCache = true;
                        if (_tstKeyChanged != "")
                            result = _tstKeyChanged + "♣*♣" + result;
                        return result;
                    }
                }
                else if (rfSave == "True" && refreshMs == "False")
                {
                    string sessCacheKey = tstData.transid + "-" + fldName + "-" + parentFlds;
                    if (Session[sessCacheKey] != null)
                    {
                        result = Session[sessCacheKey].ToString();
                        isCache = true;
                        if (_tstKeyChanged != "")
                            result = _tstKeyChanged + "♣*♣" + result;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("MultiSelect from GetMultiSelectValues.-" + ex.Message, sessionid, "MultiSelect-" + fldName, "");
            }

            result = asbExt.CallGetMultiSelectValues(tstData.transid, iXml, tstData.tstStrObj.structRes);
            logobj.CreateLog("MultiSelect from GetMultiSelectValues.-" + result, sessionid, "MultiSelect-" + fldName, "");

            try
            {
                if (rfSave != "True" && isCache == false && result != "")
                {
                    string schemaName = string.Empty;
                    if (HttpContext.Current.Session["dbuser"] != null)
                        schemaName = HttpContext.Current.Session["dbuser"].ToString();

                    FDW fdwObj = new FDW();
                    string fldkey = utilObj.GetRedisServerFieldkey(tstData.transid, Constants.FIELDNAME, fldName, 0);
                    fdwObj.HashSetKey(fldkey, parentFlds, result);
                }
                else if (rfSave == "True" && isCache == false && result != "")
                {
                    string sessCacheKey = tstData.transid + "-" + fldName + "-" + parentFlds;
                    Session[sessCacheKey] = result;
                    utilObj.AddKeyOnRefreshSave(sessCacheKey);
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("MultiSelect from GetMultiSelectValues.-" + ex.Message, sessionid, "MultiSelect-cache-" + fldName, "");
            }
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♣*♣" + result;
            return result;
        }


        [WebMethod(EnableSession = true)]
        public string SaveSmartViews(string iView, string key, string userSmartViewJSON, string uname)
        {
            string result = string.Empty;
            //string structurePlusAccessControl = string.Empty;
            IviewData objIview = new IviewData();
            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            if (key != "")
            {
                try
                {
                    //objIview = new IviewData();
                    objIview = (IviewData)HttpContext.Current.Session[key];
                    if (objIview == null)
                        return Constants.DUPLICATESESS;
                    //if (!objIview.requestJSON)
                    //{
                    //    structurePlusAccessControl = objIview.StructureXml;
                    //}
                    //else if (objIview.StructureXml != string.Empty)
                    //{
                    //    structurePlusAccessControl = objIview.AccessControlXml + "#$#" + objIview.StructureXml;
                    //}
                }
                catch (Exception ex)
                {
                    //structurePlusAccessControl = string.Empty;
                }
            }


            string trace = "false";
            // string uname = (HttpContext.Current.Session["build"].ToString() == "T") ? "ALL": user ;


            GetGlobalVars();
            string filename = "SaveSmartViews-" + iView;
            trace = logobj.CreateLog("Call to SaveSmartViews Web Service", sessionid, filename, string.Empty);


            //string errorLog = string.Empty;
            //if (ConfigurationManager.AppSettings["LoginTrace"] != null)
            //    trace = ConfigurationManager.AppSettings["LoginTrace"].ToString().ToLower();

            string inputXML = "<root name='" + iView + "' axpapp='" + project + "' sessionid='" + sessionid + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + user + "' applyto='" + uname + "' trace='" + trace + "'><config_data>" + userSmartViewJSON + "</config_data>";


            inputXML = inputXML + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";


            result = asbExt.SaveSmartViews(iView, inputXML, objIview.WebServiceTimeout);

            string error = string.Empty;


            error = utilObj.ParseXmlErrorNode(result);

            logobj.CreateLog("End Time : " + DateTime.Now.ToString(), sessionid, filename, string.Empty);

            if (error != "")
            {
                return error;
            }
            else
            {
                try
                {
                    if (uname == "ALL")
                    {
                        uname = uname.ToLower();
                    }


                    Session[key] = objIview;

                    FDW fdwObj = new FDW();
                    //string fdKey = Constants.RedisIviewObj;
                    string fdSettingsKey = Constants.RedisIviewSettings;
                    if (objIview.purposeString != "")
                    {
                        //fdKey = Constants.RedisListviewObj;
                        fdSettingsKey = Constants.RedisListviewSettings;
                    }
                    //fdwObj.SaveInRedisServer(utilObj.GetRedisServerkey(fdKey, objIview.IName), objIview, fdKey, schemaName);

                    FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

                    fdwObj.SaveInRedisServer(fObj.MakeKeyName(fdSettingsKey, objIview.IName, uname), userSmartViewJSON, fdSettingsKey, schemaName);
                }
                catch (Exception ex) { }
                return result;
            }
        }

        [WebMethod(EnableSession = true)]
        public void RemoveUnwantedAxpFiles()
        {
            try
            {
                if (HttpContext.Current.Session["AxpAttFileServer"] != null)
                {
                    ArrayList attServerFiles = (ArrayList)HttpContext.Current.Session["AxpAttFileServer"];
                    string authenticationStatus = string.Empty;
                    if (attServerFiles.Count > 0 && utils.GetAuthentication(ref authenticationStatus))
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
            }
            catch (Exception) { }
        }

        [WebMethod(EnableSession = true)]
        public string LoadAxpFileToScript(string filePath, string fileName)
        {
            string copied = "false";
            try
            {
                string PrefixFilename = string.Empty;
                string sessionid = HttpContext.Current.Session["nsessionid"].ToString();
                string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                string scrPath = scriptsPath + "axpert\\" + sessionid + "\\";
                if (!File.Exists(scrPath + fileName))
                {
                    if (filePath == string.Empty)
                    {
                        if (HttpContext.Current.Session["grdAttPath"] != null)
                            filePath = HttpContext.Current.Session["grdAttPath"].ToString();
                        else
                        {
                            GetGlobalAttachPath();
                            filePath = HttpContext.Current.Session["grdAttPath"].ToString();
                        }
                        if (filePath == "" && Session["AxConfigFileUploadPath"] != null && Session["AxConfigFileUploadPath"].ToString() != "")
                            filePath = Session["AxConfigFileUploadPath"].ToString();
                    }
                    else if (filePath != string.Empty && filePath.EndsWith("*"))
                    {
                        string axpFilePath = filePath.Substring(0, filePath.LastIndexOf('\\')) + "\\";
                        PrefixFilename = filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("*", "");
                        filePath = axpFilePath;
                    }
                    string unzipFileName = CopyFiles(filePath, scrPath, PrefixFilename + fileName, fileName, "true");
                    if (unzipFileName != string.Empty)
                    {
                        copied = "true:" + unzipFileName;
                    }
                    else
                        copied = "true";
                }
                else if (fileName != string.Empty && fileName.EndsWith(".zip"))
                {
                    string unzipFile = string.Empty;
                    string fileProtectPwd = string.Empty;
                    if (HttpContext.Current.Session["axp_uploadfileprotectedpwd"] != null)
                        fileProtectPwd = HttpContext.Current.Session["axp_uploadfileprotectedpwd"].ToString();
                    if (fileProtectPwd != string.Empty)
                    {
                        string zipFilePath = Path.Combine(scrPath, fileName);
                        using (ZipFile zip = ZipFile.Read(zipFilePath))
                        {
                            zip.Password = fileProtectPwd;
                            foreach (ZipEntry e in zip)
                            {
                                e.Extract(scrPath, ExtractExistingFileAction.OverwriteSilently);
                                unzipFile = e.FileName;
                            }
                        }
                    }
                    if (unzipFile != string.Empty)
                    {
                        copied = "true:" + unzipFile;
                    }
                }
            }
            catch (Exception) { }
            return copied;
        }

        [WebMethod(EnableSession = true)]
        public string IsAxpFileExists(string filePath)
        {
            string fileExist = string.Empty;
            try
            {
                if (File.Exists(filePath))
                    fileExist = "true";
                else
                    fileExist = "false";
            }
            catch (Exception)
            {
                fileExist = "error";
            }
            return fileExist;
        }

        [WebMethod(EnableSession = true)]
        public string IsGridAttFileExists(string filePath)
        {
            string fileExist = string.Empty;
            try
            {
                if (File.Exists(filePath))
                    fileExist = "true";
                else
                    fileExist = "false";
            }
            catch (Exception)
            {
                fileExist = "error";
            }
            return fileExist;
        }

        [WebMethod(EnableSession = true)]
        public string AxpFileMoveToNewFolder(string axpOldPath, string axpNewPath, string axpFileList)
        {
            string copied = "false";
            try
            {
                if (axpOldPath == string.Empty)
                {
                    if (HttpContext.Current.Session["grdAttPath"] != null)
                        axpOldPath = HttpContext.Current.Session["grdAttPath"].ToString();
                    else
                    {
                        GetGlobalAttachPath();
                        axpOldPath = HttpContext.Current.Session["grdAttPath"].ToString();
                    }
                }
                foreach (var flname in axpFileList.Split(','))
                {
                    CopyFiles(axpOldPath, axpNewPath, flname, flname);
                    if (File.Exists(axpOldPath + "\\" + flname))
                    {
                        File.Delete(axpOldPath + "\\" + flname);
                    }
                }
                copied = "true";
            }
            catch (Exception) { }
            return copied;
        }

        [WebMethod(EnableSession = true)]
        public string GetTstructFieldsForListView(string transId)
        {
            string returnString = string.Empty;

            if (HttpContext.Current.Session["project"] == null)
            {
                return null;
            }
            try
            {

                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();

                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];

                string fdListViewFieldsInfoKey = Constants.ListViewFieldsInfo;

                string loadString = fObj.StringFromRedis(utilObj.GetRedisServerkey(fdListViewFieldsInfoKey, transId), schemaName);
                if (loadString == string.Empty)
                {
                    loadString = HttpContext.Current.Session[transId + "_" + fdListViewFieldsInfoKey].ToString();
                }
                returnString = loadString;
            }
            catch (Exception ex)
            { }

            return returnString;
        }

        [WebMethod(EnableSession = true)]
        public string GetCardsData()
        {
            string returnString = string.Empty;

            if (HttpContext.Current.Session["project"] == null)
            {
                return null;
            }

            LogFile.Log logobj = new LogFile.Log();
            string cardId = string.Empty;

            try
            {
                string wgData = string.Empty;
                //string sqlInput = "select axp_cardsid,cardname,cardtype,charttype,chartjson,cardicon, width, height,cachedata,autorefresh,sql_editor_cardsql as cardsql,orderno,accessstring, pagename, pagedesc, cardbgclr, axpfile_imgcard, htype, htransid, hcaption from axp_cards where accessstring='' or lower(accessstring) like '%all%' or lower(accessstring) like concat('%', '" + System.Web.HttpContext.Current.Session["userroles"].ToString() + "', '%') order by orderno";

                string[] roles = System.Web.HttpContext.Current.Session["userroles"].ToString().Split(',');

                string roleString = string.Join(" or ", roles.Select(role =>
                   {
                       return ("lower(accessstring) like '%," + role + ",%'");
                   }
                ));

                string sqlInput = "select axp_cardsid,cardname,cardtype,charttype,chartjson,cardicon,pagename,pagedesc,cardbgclr,width,height,cachedata,autorefresh,sql_editor_cardsql as cardsql,orderno,accessstring,htransid,htype,hcaption,axpfile_imgcard,html_editor_card,calendarstransid from axp_cards where " + roleString + " order by orderno";

                ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                wgData = asbExt.ExecuteSQL("", sqlInput, "JSON");

                JArray returnArray = new JArray();

                JObject resultRows = JObject.Parse(wgData);

                if (wgData != string.Empty)
                {
                    try
                    {

                        string qdata = string.Empty;

                        JObject wgResult = new JObject();

                        JArray cards = (JArray)resultRows["result"]["row"];

                        foreach (JObject card in cards)
                        {

                            string node = card["cardsql"].ToString();

                            if (node != "")
                            {
                                string sqlQuery = node.Trim();
                                if (sqlQuery.Contains(":"))// to check any of tstruct fields & global Vars
                                {
                                    XmlDocument docGbl = new XmlDocument();
                                    docGbl.LoadXml(HttpContext.Current.Session["axGlobalVars"].ToString());
                                    foreach (XmlNode gblNode in docGbl.DocumentElement.ChildNodes)
                                    {
                                        if (Regex.Match(sqlQuery, String.Format(@":\b{0}\b", gblNode.Name), RegexOptions.IgnoreCase).Success)
                                            sqlQuery = sqlQuery.Replace(":" + gblNode.Name, "'" + gblNode.InnerText + "'");
                                    }
                                    if (Regex.Match(sqlQuery, String.Format(@":\b{0}\b", "username"), RegexOptions.IgnoreCase).Success)
                                        sqlQuery = sqlQuery.Replace(":username", "'" + HttpContext.Current.Session["username"].ToString() + "'");
                                }
                                sqlQuery = utilObj.CheckSpecialChars(sqlQuery);

                                qdata = asbExt.ExecuteSQL("", sqlQuery, "JSON"); ;


                                //XmlDocument docQuery = new XmlDocument();
                                //XmlNodeList queryResponse = docQuery.SelectNodes("sqlresultset");
                                //XmlDocument queryDocs = new XmlDocument();
                                //try
                                //{
                                //    docQuery.LoadXml(qdata.ToString());
                                //    queryDocs.LoadXml(queryResponse[0].InnerXml);
                                //    qdata = JsonConvert.SerializeXmlNode(queryDocs);
                                //}
                                //catch (Exception ex)
                                //{
                                //    qdata = (new JObject()).ToString();
                                //}
                                try
                                {
                                    node = (JObject.Parse(qdata)["result"]).ToString();
                                }
                                catch (Exception ex)
                                {
                                    node = "";
                                }

                            }

                            try
                            {
                                card["cardsql"] = JObject.Parse(node.ToString());
                            }
                            catch (Exception ex)
                            {
                                card["cardsql"] = "";
                            }
                            returnArray.Add(card);
                        }

                    }
                    catch (Exception ex)
                    { }
                }


                returnString = returnArray.ToString();
            }
            catch (Exception ex)
            { }

            return returnString;
        }

        [WebMethod(EnableSession = true)]
        public string saveCardsDesign(string design)
        {
            string result = string.Empty;

            if (HttpContext.Current.Session["project"] == null)
            {
                return null;
            }

            design = design.Replace("'", "''");
            string sqlInput = "update axusers Set carddetails = '" + design + "' where username='" + HttpContext.Current.Session["username"].ToString() + "'";

            //ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            //result = asbExt.ExecuteSQL("", sqlInput, "JSON");

            //DBContext obj = new DBContext();
            //obj.ExecuteSqlQueryInline(sqlInput);

            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            result = asbExt.ExecuteSQL("", sqlInput, "JSON");

            if (result.Contains("success"))
            {
                result = "true";
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public string refreshCards(string cards)
        {
            string result = string.Empty;

            if (HttpContext.Current.Session["project"] == null)
            {
                return null;
            }

            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();

            string sid = Session["nsessionid"].ToString();
            sid = utilObj.CheckSpecialChars(sid);

            string errlog = logobj.CreateLog("Refresh Cards", sid, "RefreshCards", "");

            string sXml = string.Empty;

            sXml = sXml + "<root cardids='" + cards + "' axpapp='" + Session["project"].ToString() + "' sessionid='" + sid + "' trace='" + errlog + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "'";
            sXml = sXml + "> ";
            sXml = sXml + Session["axApps"].ToString() + Application["axProps"].ToString() + Session["axGlobalVars"].ToString() + Session["axUserVars"].ToString();
            sXml = sXml + "</root>";

            result = objWebServiceExt.GetIndividualCardDetails("main", sXml);

            return result;
        }

        protected string CopyFiles(string sourcePath, string destPath, string srcFileName, string desFileName, string fromaxpfileclick = "")
        {
            string destFileName = string.Empty;
            string authenticationStatus = string.Empty;
            if (utils.GetAuthentication(ref authenticationStatus))
            {
                DirectoryInfo sPath = new DirectoryInfo(sourcePath);
                DirectoryInfo dPath = new DirectoryInfo(destPath);

                if (!dPath.Exists)
                    dPath.Create();

                if (sPath.Exists)
                {
                    BinaryReader brReader = default(BinaryReader);
                    BinaryWriter brWriter = default(BinaryWriter);

                    string strFile = sourcePath + "\\" + srcFileName;
                    string strDest = destPath + "\\" + desFileName;
                    FileStream input = null;
                    try
                    {
                        input = new FileStream(strFile, FileMode.Open, FileAccess.Read);
                    }
                    catch (FileNotFoundException ex) { }
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
                        brReader.Dispose();
                        brWriter.Dispose();
                        output.Dispose();
                        input.Dispose();
                    }

                    if (fromaxpfileclick == "true")
                    {
                        string fileProtectPwd = string.Empty;
                        if (HttpContext.Current.Session["axp_uploadfileprotectedpwd"] != null)
                            fileProtectPwd = HttpContext.Current.Session["axp_uploadfileprotectedpwd"].ToString();
                        if (fileProtectPwd != string.Empty && desFileName.EndsWith(".zip"))
                        {
                            string zipFilePath = Path.Combine(destPath, desFileName);
                            using (ZipFile zip = ZipFile.Read(zipFilePath))
                            {
                                zip.Password = fileProtectPwd;
                                foreach (ZipEntry e in zip)
                                {
                                    e.Extract(destPath, ExtractExistingFileAction.OverwriteSilently);
                                    destFileName = e.FileName;
                                }
                            }
                            if (File.Exists(zipFilePath))
                            {
                                File.Delete(zipFilePath);
                            }
                        }
                    }
                }
            }
            return destFileName;
        }

        protected void GetGlobalAttachPath()
        {
            bool isLocalFolder = false;
            bool isRemoteFolder = false;
            string imagePath = string.Empty;
            string imageServer = string.Empty;
            string grdAttPath = string.Empty;
            string errorMessage = string.Empty;
            string mapUsername = string.Empty;
            string mapPassword = string.Empty;

            if (HttpContext.Current.Session["AxpImageServerGbl"] != null)
            {
                imageServer = HttpContext.Current.Session["AxpImageServerGbl"].ToString();
                imageServer = imageServer.Replace(";bkslh", @"\");
            }
            if (HttpContext.Current.Session["AxpImagePathGbl"] != null)
            {
                imagePath = HttpContext.Current.Session["AxpImagePathGbl"].ToString();
                imagePath = imagePath.Replace(";bkslh", @"\");

                if (imagePath.IndexOf(":") > -1)
                    isLocalFolder = true;
                else
                    if (imagePath.IndexOf(@"\") > -1)
                    isRemoteFolder = true;
            }

            if (imagePath != string.Empty)
            {
                if (isLocalFolder || isRemoteFolder)
                    grdAttPath = imagePath;
                else
                    grdAttPath = imageServer + @"\" + imagePath;
            }
            else if (imageServer != string.Empty)
            {
                grdAttPath = imageServer;
            }
            else
            {
                if (HttpContext.Current.Session["AxGridAttachPath"] != null)
                {
                    grdAttPath = HttpContext.Current.Session["AxGridAttachPath"].ToString();
                }
            }
            HttpContext.Current.Session["grdAttPath"] = grdAttPath;
        }
        public string saveArrangeMenuData(string menuXml, string changedNodes, string actionSequence, string changedVisibility)
        {
            string result = string.Empty;
            if (HttpContext.Current.Session["project"] == null)
                return utils.SESSTIMEOUT;
            string proj = HttpContext.Current.Session["project"].ToString();
            string sessionid = HttpContext.Current.Session["nsessionid"].ToString();
            string errLog = logobj.CreateLog("ArrangeMenu- callRearrangeMenuWS.", sessionid, "ArrangeMenu-" + proj, "new");
            string InputXml = "<root scriptpath='" + HttpContext.Current.Application["ScriptsPath"].ToString() + "' axpapp=\"" + proj + "\"  trace=\"" + errLog + "\" sessionid=\"" + sessionid + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' changedNodes=\"" + changedNodes + "\" actionsequence=\"" + actionSequence + "\" changedVisibility=\"" + changedVisibility + "\" >";
            InputXml += menuXml;
            InputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString();
            InputXml += HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
            InputXml += "</root>";
            result = asbExt.callRearrangeMenuWS(InputXml);
            string errMsg = utils.ParseXmlErrorNode(result);
            if (errMsg != string.Empty)
            {
                if (errMsg == Constants.SESSIONERROR || errMsg == Constants.SESSIONEXPMSG || errMsg == Constants.SESSIONEXPMSG || errMsg == Constants.ERAUTHENTICATION)
                {
                    return utils.SESSTIMEOUT;
                }
                else
                {
                    return errMsg;
                }
            }
            return result;
        }
        public string callGetToolbarWS(string stype, string name)
        {
            string result = string.Empty;
            string errLog = logobj.CreateLog("ToolbarManager- callGetToolbarWS.", System.Web.HttpContext.Current.Session["nsessionid"].ToString(), "ToolbarManager-" + System.Web.HttpContext.Current.Session["project"].ToString(), "new");
            if (HttpContext.Current.Session["project"] == null)
                return utils.SESSTIMEOUT;
            string proj = HttpContext.Current.Session["project"].ToString();
            string InputXml = "<root name =\"" + name + "\"  stype = \"" + stype + "\" axpapp = \"" + System.Web.HttpContext.Current.Session["project"].ToString() + "\" sessionid = \"" + System.Web.HttpContext.Current.Session["nsessionid"].ToString() + "\" appsessionkey = '" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username = \"" + HttpContext.Current.Session["username"].ToString() + "\"  trace = '' responseformat = 'json' ></root >";
            result = asbExt.GetToolBar(InputXml);
            return result;
        }
        public string callSaveToolbarWS(string stype, string name, string toolbarJSON)
        {
            string result = string.Empty;
            string errLog = logobj.CreateLog("ArrangeMenu- callRearrangeMenuWS.", HttpContext.Current.Session["nsessionid"].ToString(), "ArrangeMenu-" + System.Web.HttpContext.Current.Session["project"].ToString(), "new");
            if (HttpContext.Current.Session["project"] == null)
                return utils.SESSTIMEOUT;
            string proj = HttpContext.Current.Session["project"].ToString();
            string InputXml = "<root name =\"" + name + "\"  stype = \"" + stype + "\" axpapp = \"" + System.Web.HttpContext.Current.Session["project"].ToString() + "\" sessionid = \"" + System.Web.HttpContext.Current.Session["nsessionid"].ToString() + "\" appsessionkey = '" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username = \"" + HttpContext.Current.Session["username"].ToString() + "\"  trace = ''>";
            InputXml += " <toolbar_data>" + toolbarJSON + "</toolbar_data></root>";
            result = asbExt.saveToolbar(InputXml);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string SendNotificationFromFirebase(string guId, string isForceCall)
        {
            string result = "-1";
            try
            {
                string firebase_id = string.Empty;
                if (guId == "")
                    return "";
                string redisValues = "";
                Util.Util util;
                util = new Util.Util();
                FDR fObj = null;
                string GUID = string.Empty;
                string schemaName = string.Empty;
                try
                {
                    fObj = (FDR)Session["FDR"];
                }
                catch (Exception ex)
                {
                    Session["FDR"] = null;
                }
                if (fObj == null)
                {
                    fObj = new FDR();
                    Session["FDR"] = fObj;
                }
                string notifiKey = Constants.AXHYBRIDNOTIFIINFO;
                schemaName = "♠";
                try
                {
                    redisValues = fObj.StringFromRedis(util.GetRedisServerkey(notifiKey, guId), schemaName).ToString();
                    dynamic mbNotify = JsonConvert.DeserializeObject(redisValues);
                    firebase_id = mbNotify.firebase_id.Value;
                }
                catch (Exception ex)
                {
                    return "Redis error";
                }

                string webAddr = "";
                if (Session["ARM_URL"] != null && Session["ARM_URL"].ToString() != "")
                    webAddr = Session["ARM_URL"].ToString() + "/api/v1/SendFCMNotification";
                else if (Session["AxFCMSendMsgURL"] != null && Session["AxFCMSendMsgURL"].ToString() != "")
                    webAddr = Session["AxFCMSendMsgURL"].ToString();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"message\":{\"token\":[\"" + firebase_id + "\"],\"data\": {\"isaxp_locationdata\":\"true\"}}}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                if (isForceCall == "true")
                {
                    Session["SendNotificationForceCall"] = "true";
                }
                else
                {
                    Session["SendNotificationForceCall"] = "false";
                }
            }
            catch (Exception ex)
            {
                Session["SendNotificationForceCall"] = "false";
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetNewGlobalVars(string lang)
        {
            string sXml = string.Empty;
            string result = string.Empty;
            string errlogapp = logobj.CreateLog("Calling Get New global variables", HttpContext.Current.Session["nsessionid"].ToString(), "GetNewGlobalVars", "");
            string iXml = "<root " + lang + " scriptpath='" + HttpContext.Current.Application["ScriptsPath"].ToString() + "' axpapp='" + HttpContext.Current.Session["project"] + "' trace='" + errlogapp + "' sessionid='" + HttpContext.Current.Session["nsessionid"] + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' user='" + HttpContext.Current.Session["username"].ToString() + "'>";
            iXml += HttpContext.Current.Session["axApps"].ToString();
            iXml += HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            WebServiceExt objWebServiceExt = new WebServiceExt();
            result = objWebServiceExt.CallGetNewGlobalVarsWS("refreshparams", iXml);
            if (result != string.Empty && !result.Contains("\"error\""))
            {
                ParseGlobalVarsResult(result);
                result = "done";
            }
            else
                result = "Please try again later.";
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string CallRestGetChoice(string jsonData, string projectname, string username)
        {
            string sqlQuery = Constants.SQL_GET_EMAIL;
            sqlQuery = sqlQuery.Replace("$USERNAME$", username);
            string Sessionid = Session.SessionID;
            string response = string.Empty;
            string DATA = string.Empty;
            utils.GetAxApps(projectname);
            string webServiceNames = "GetChoices";
            string axapps = xmltojson(Session["axApps"].ToString());
            string axprops = HttpContext.Current.Application["axProps"].ToString();
            axprops = xmltojson(axprops);
            string URL = GetArmScriptURL() + "ASBMenuRest.dll/datasnap/rest/TASBMenuRest/" + webServiceNames + "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            jsonData = jsonData.Substring(0, jsonData.Length - 2);
            jsonData = jsonData + "," + "\"sql\" :\"" + sqlQuery + "\"";
            jsonData = jsonData + "," + "\"sessionid\" :\"" + Session.SessionID + "\"";
            jsonData = jsonData + "," + "\"username\" :\"" + username + "\"}";
            //DATA = jsonData +","+ "\"" + webServiceName + "\":{" + axapps + "},\"axprops\":{" + axprops + "},\"globalvars\":\"\",\"uservars\":\"\"}";
            DATA = jsonData + "," + "\"" + projectname + "\":{" + axapps + "},\"axprops\":{" + axprops + "}}";

            try
            {
                //DATA = utilObj.ServiceInputSpecialChars(DATA);

                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();

                return response;
            }
            catch (Exception e)
            {
                LogFile.Log logObj = new LogFile.Log(); //hwayetntwam14k1ztv0pbpxy

                logobj.CreateLog("Call to Getchoices Web Service input json " + e.Message, Sessionid, "CallRestGetChoice", string.Empty);
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
                return response;
            }
        }

        private void ParseGlobalVarsResult(string result)
        {
            ArrayList globalVariables = new ArrayList();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            XmlNode childNode = xmlDoc.SelectSingleNode("/result/globalvars");

            string axGridAttachPath = string.Empty, axAttachmentpath = string.Empty;
            if (HttpContext.Current.Session["AxGridAttachPath"] != null && HttpContext.Current.Session["AxGridAttachPath"].ToString() != "")
            {
                if (childNode.SelectSingleNode("AXPIMAGEPATH") != null)
                {
                    childNode.SelectSingleNode("AXPIMAGEPATH").InnerText = HttpContext.Current.Session["AxGridAttachPath"].ToString();
                    axGridAttachPath = "";
                }
                else
                {
                    axGridAttachPath = HttpContext.Current.Session["AxGridAttachPath"].ToString();
                    axGridAttachPath = "<AXPIMAGEPATH>" + axGridAttachPath + "</AXPIMAGEPATH>";
                }
            }
            if (HttpContext.Current.Session["AxAttachFilePath"] != null && HttpContext.Current.Session["AxAttachFilePath"].ToString() != "")
            {
                if (childNode.SelectSingleNode("AXPATTACHMENTPATH") != null)
                {
                    childNode.SelectSingleNode("AXPATTACHMENTPATH").InnerText = HttpContext.Current.Session["AxAttachFilePath"].ToString();
                    axAttachmentpath = "";
                }
                else
                {
                    axAttachmentpath = HttpContext.Current.Session["AxAttachFilePath"].ToString();
                    axAttachmentpath = "<AXPATTACHMENTPATH>" + axAttachmentpath + "</AXPATTACHMENTPATH>";
                }
            }

            try
            {
                if (Session["AxTimezoneVariation"] != null && Session["AxTimezoneVariation"].ToString() == "false")
                {
                    if (childNode.SelectSingleNode("timediff") != null)
                    {
                        childNode.SelectSingleNode("timediff").InnerText = "0";
                    }
                }
            }
            catch (Exception ex) { }
            bool axp_timezone = false;
            string virtualdownloadpath = utilObj.VirtualFilePathUpdate();
            string glbVarsFromIni = utilObj.GetGblVarsFromIni();
            HttpContext.Current.Session["axGlobalVars"] = "<globalvars>" + childNode.InnerXml + axGridAttachPath + axAttachmentpath + virtualdownloadpath + glbVarsFromIni + "</globalvars>";
            string defProject = string.Empty;
            if (HttpContext.Current.Session["project"] != null)
                defProject = HttpContext.Current.Session["project"].ToString();
            foreach (XmlNode xmlNode in childNode.ChildNodes)
            {
                if (xmlNode.Name.ToLower() == "axp_timezone" && xmlNode.InnerText != "")
                {
                    axp_timezone = true;
                    if (childNode.SelectSingleNode("timediff") != null)
                    {
                        childNode.SelectSingleNode("timediff").InnerText = "";
                    }
                }
                if (xmlNode.Name.ToLower() == "axp_lockonread" && (xmlNode.InnerText.ToLower() == "t" || xmlNode.InnerText.ToLower() == "true"))
                    HttpContext.Current.Session["axp_isLockOnRead"] = "true";
                globalVariables.Add(xmlNode.Name + "=" + xmlNode.InnerText);

                if (xmlNode.Name.ToLower() == "axglo_hide")
                    HttpContext.Current.Session["AxGloHideShow"] = xmlNode.InnerText;
                if (xmlNode.Name.ToLower() == "axglo_recordid")
                    HttpContext.Current.Session["AxGloRecId"] = xmlNode.InnerText;
                if (xmlNode.Name.ToLower() == "rolename")
                    HttpContext.Current.Session["AxRole"] = xmlNode.InnerText;
                if (xmlNode.Name.ToLower() == "responsibilies")
                    HttpContext.Current.Session["AxResponsibilities"] = xmlNode.InnerText;

                if (xmlNode.Name.ToLower() == "axpimagepath")
                    HttpContext.Current.Session["AxpImagePathGbl"] = xmlNode.InnerText;
                if (xmlNode.Name.ToLower() == "axpimageserver")
                    HttpContext.Current.Session["AxpImageServerGbl"] = xmlNode.InnerText;
                if (xmlNode.Name.ToLower() == "axpattachmentpath")
                    HttpContext.Current.Session["AxpAttachmentPathGbl"] = xmlNode.InnerText;
            }

            if (axp_timezone)
            {
                HttpContext.Current.Session["axGlobalVars"] = "<globalvars>" + childNode.InnerXml + axGridAttachPath + axAttachmentpath + virtualdownloadpath + glbVarsFromIni + "</globalvars>";
            }

            if (globalVariables.Count > 0)
            {
                CreateParamaterArray(globalVariables);
            }
            HttpContext.Current.Session["project"] = defProject;
        }

        private void CreateParamaterArray(ArrayList globalVariables)
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
                value = utils.CheckSpecialChars(strVar[1].ToString());
                if (!Constants.globalVarPathVaribles.Contains(strVar[0].ToLower()))
                {
                    paramlist.Append("Parameters[" + i + "] = " + "\"" + strVar[0] + "~" + value + "\";  ");
                    i++;
                }
                if (strVar[0].ToString() == "project" && value.Contains(','))
                    HttpContext.Current.Session[strVar[0].ToString()] = value.Split(',')[1];
                else
                    HttpContext.Current.Session[strVar[0].ToString()] = value;
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
            paramCnt = i;
            paramlist.Append("Parameters[" + paramCnt + "] = " + "\"username~" + HttpContext.Current.Session["user"].ToString() + "\";  ");
            paramlist.Append("Parameters[" + (paramCnt + 1) + "] = " + "\"Culture~" + dtCulture + "\";  ");

            HttpContext.Current.Session["ClientLocale"] = dtCulture;
            if (paramlist.ToString() != string.Empty)
                utils.SetGlobalVarString(paramlist.ToString());
        }

        [WebMethod(EnableSession = true)]
        public string switchLanguage(string lang)
        {
            string langAttribute = "";

            langAttribute = " lang='" + lang + "' ";

            HttpContext.Current.Session["language"] = lang.ToLower();
            HttpContext.Current.Session["AxInternalRefresh"] = "true";
            HttpContext.Current.Session["isLanguageRefresh"] = "true";

            return GetNewGlobalVars(langAttribute);
        }

        [WebMethod(EnableSession = true)]
        public string GetAcceptDataFromAPI(string tstDataId, string fldname, string apiInfo, string resTstHtmlLS)
        {
            string result = string.Empty;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            try
            {
                if (resTstHtmlLS != "")
                {
                    utilObj.DeleteTstIvObject(tstDataId.Split('_')[0]);
                    tstDataId = utilObj.GetReGenTstDataObj(tstDataId, resTstHtmlLS);
                    _tstKeyChanged = tstDataId;
                }
                TStructData tstData = (TStructData)Session[tstDataId];
                if (tstData == null)
                    return Constants.DUPLICATESESS;
                TStructDef tstStrObj = tstData.tstStrObj;
                int fldidx = tstStrObj.GetFieldIndex(fldname);
                TStructDef.FieldStruct fld = (TStructDef.FieldStruct)tstStrObj.flds[fldidx];
                //string api = fld.fieldSelAPI;
                //string apiParams = fld.fieldSelAPIParams;
                //string[] strDetails = apiParams.Split('♠');
                //string apiType = strDetails[1];
                //string contType = strDetails[2];

                string api = string.Empty;
                string mapname = string.Empty;
                string apiType = string.Empty;
                string contType = string.Empty;
                string contBody = string.Empty;

                string _apiname = fld.fieldSelAPIName;
                if (_apiname != null && _apiname != string.Empty)
                {
                    try
                    {
                        string _apiData = GetFromAPIInfo(_apiname, fldname, tstStrObj.transId);
                        JArray returnArray = new JArray();
                        JObject resultRows = JObject.Parse(_apiData);
                        if (_apiData != string.Empty)
                        {
                            JArray _apiEle = (JArray)resultRows["result"]["row"];
                            foreach (JObject row in _apiEle)
                            {
                                JObject modifiedRow = new JObject();
                                foreach (var property in row.Properties())
                                {
                                    if (property.Name.ToLower() == "execapiurl")
                                        api = property.Value.ToString();
                                    if (property.Name.ToLower() == "apiresponsetype")
                                        apiType = property.Value.ToString();
                                    if (property.Name.ToLower() == "apiresponseformat")
                                        contType = property.Value.ToString();
                                    if (property.Name.ToLower() == "execapirequeststring")
                                        contBody = property.Value.ToString();
                                }
                            }
                        }

                        string apiParams = fld.fieldSelAPIParams;
                        string[] strDetails = apiParams.Split('♠');
                        mapname = strDetails[0];
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    api = fld.fieldSelAPI;
                    string apiParams = fld.fieldSelAPIParams;
                    string[] strDetails = apiParams.Split('♠');
                    mapname = strDetails[0];
                    apiType = strDetails[1];
                    contType = strDetails[2];
                    contBody = strDetails[5];
                }
                if (api == string.Empty)
                {
                    return "";
                }



                if (apiType == "Axpert")
                {
                    if (Session["ARM_Scripts_URL"] != null && Session["ARM_Scripts_URL"].ToString() != "")
                    {
                        string _restdllpath = Session["ARM_Scripts_URL"].ToString();
                        if (_restdllpath.Substring(_restdllpath.Length - 1) != "/")
                            _restdllpath += "/";
                        string[] _apiPath = api.Split(new[] { "ASBMenuRest.dll" }, StringSplitOptions.None);
                        api = _restdllpath + "ASBMenuRest.dll" + _apiPath[1];
                    }
                }


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);

                string DATA = string.Empty;
                if (apiType == "Axpert")
                {
                    request.Method = "POST";
                    request.ContentType = "application/" + contType;
                    DATA = contBody;// strDetails[5];
                    DATA = ParseAxpertApiParams(DATA, "sqlparams", tstData.fieldValueXml, tstStrObj);
                    request.ContentLength = DATA.Length;
                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                    requestWriter.Write(DATA);
                    requestWriter.Close();
                }
                else
                {
                    request.Method = "GET";
                    request.ContentType = "application/" + contType;
                }
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                result = responseReader.ReadToEnd();
                Console.Out.WriteLine(result);
                responseReader.Close();
                if (result != string.Empty)
                {
                    var obj = JObject.Parse(result);
                    foreach (var attNode in obj["result"][0]["result"]["row"])
                    {
                        result = attNode.First.Last.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Get Accept field value from API.-" + ex.Message, sessionid, "AcceptApi-" + fldname, "new");
            }
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string CallGetDBConnectionWS(string jsonData)
        {
            string response = string.Empty;
            string DATA = string.Empty;
            string URL = GetArmScriptURL() + "ASBMenuRest.dll/datasnap/rest/TASBMenuRest/GetDBConnection";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";

            try
            {
                DATA = utilObj.ServiceInputSpecialChars(jsonData);

                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
                return response;
            }
            catch (Exception e)
            {
                LogFile.Log logObj = new LogFile.Log();
                logobj.CreateLog("Call to GetDBConnection Service input json " + jsonData + " URL:" + URL + " Error:" + e.Message, Session.SessionID, "CallGetDBConnection", "new", "true");
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
                return response;
            }

        }

        [WebMethod(EnableSession = true)]
        public string CallActivateLicenseWS(string xmlData)
        {
            string result = string.Empty;
            WebServiceExt objWebServiceExt = new WebServiceExt();
            result = objWebServiceExt.CallActivateLicenseWS(xmlData);

            return result;
        }

        [WebMethod(EnableSession = true)]
        public string CallGetLicenseInfoWS(string jsonData)
        {
            string response = string.Empty;
            string DATA = string.Empty;
            string URL = GetArmScriptURL() + "ASBMenuRest.dll/datasnap/rest/TASBMenuRest/GetLicenseInfo";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";

            try
            {
                DATA = utilObj.ServiceInputSpecialChars(jsonData);

                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
                return response;
            }
            catch (Exception e)
            {
                LogFile.Log logObj = new LogFile.Log();
                logobj.CreateLog("Call to GetLicenseInfo Service input json " + jsonData + " URL:" + URL + " Error:" + e.Message, Session.SessionID, "CallGetLicenseInfo", "new", "true");
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
                return response;
            }

        }

        [WebMethod(EnableSession = true)]
        public string CallSetDBPasswordWS(string jsonData)
        {
            string response = string.Empty;
            string DATA = string.Empty;
            string URL = GetArmScriptURL() + "ASBMenuRest.dll/datasnap/rest/TASBMenuRest/SetDBPassword";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";

            try
            {
                DATA = utilObj.ServiceInputSpecialChars(jsonData);

                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
                return response;
            }
            catch (Exception e)
            {
                LogFile.Log logObj = new LogFile.Log();
                logobj.CreateLog("Call to SetDBPassword Service input json " + jsonData + " URL:" + URL + " Error:" + e.Message, Session.SessionID, "CallSetDBPassword", "new", "true");
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
                return response;
            }

        }

        [WebMethod(EnableSession = true)]
        public string GetProjectAppLogo(string projName)
        {
            JObject returnObject = new JObject();

            string logoPath = string.Empty;
            string webBgPath = string.Empty;
            string mobBgPath = string.Empty;

            if (projName != string.Empty)
            {
                try
                {
                    string logoDir = "~/" + projName + "";

                    var dirPath = HttpContext.Current.Server.MapPath(logoDir);

                    DirectoryInfo dir = new DirectoryInfo(dirPath);


                    var logos = dir.GetFiles("axAppLogo.*");
                    if (logos.Count() > 0 && logos[0].Name != string.Empty)
                    {
                        logoPath = "../" + projName + "/" + logos[0].Name + "";
                    }

                    var webBgs = dir.GetFiles("axAppWebBG.*");
                    if (webBgs.Count() > 0 && webBgs[0].Name != string.Empty)
                    {
                        webBgPath = "../" + projName + "/" + webBgs[0].Name + "";
                    }

                    var mobBgs = dir.GetFiles("axAppMobBG.*");
                    if (mobBgs.Count() > 0 && mobBgs[0].Name != string.Empty)
                    {
                        mobBgPath = "../" + projName + "/" + mobBgs[0].Name + "";
                    }
                }
                catch (Exception ex)
                {
                }
            }

            returnObject["logo"] = logoPath;
            returnObject["webbg"] = webBgPath;
            returnObject["mobbg"] = mobBgPath;

            return returnObject.ToString();
        }

        [WebMethod(EnableSession = true)]
        public string NotifyHybridForWeightScale(string guId)
        {
            string result = string.Empty;
            string redisValues = "";
            string firebase_id = string.Empty;
            string webAddr = "";
            try
            {
                LogFile.Log logObj = new LogFile.Log();
                if (guId == "")
                {
                    logObj.CreateLog("NotifyHybridForWeightScale- --GUID is empty.", Session.SessionID, "NotifyHybridForWeightScale", "new", "true");
                    return "";
                }

                Util.Util util = new Util.Util();
                FDR fObj = null;
                string GUID = string.Empty;
                string schemaName = string.Empty;
                try
                {
                    fObj = (FDR)Session["FDR"];
                }
                catch (Exception ex)
                {
                    Session["FDR"] = null;
                }
                if (fObj == null)
                {
                    fObj = new FDR();
                    Session["FDR"] = fObj;
                }
                string notificationKey = Constants.AXHYBRIDNOTIFIINFO;
                schemaName = "♠";
                try
                {
                    redisValues = fObj.StringFromRedis(util.GetRedisServerkey(notificationKey, guId), schemaName).ToString();
                    dynamic mbNotify = JsonConvert.DeserializeObject(redisValues);
                    firebase_id = mbNotify.firebase_id.Value;
                }
                catch (Exception ex)
                {
                    logObj.CreateLog("NotifyHybridForWeightScale- --redisValues-- " + redisValues + "--firebase_id--" + firebase_id + " --GUID--" + guId + " --Exception Message--" + ex.Message, Session.SessionID, "NotifyHybridForWeightScale", "new", "true");
                    return "Redis error";
                }

                if (Session["ARM_URL"] != null && Session["ARM_URL"].ToString() != "")
                    webAddr = Session["ARM_URL"].ToString() + "/api/v1/SendFCMNotification";
                else if (Session["AxFCMSendMsgURL"] != null && Session["AxFCMSendMsgURL"].ToString() != "")
                    webAddr = Session["AxFCMSendMsgURL"].ToString();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"message\":{\"token\":[\"" + firebase_id + "\"],\"data\": {\"isaxp_weightscaledata\":\"true\"}}}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                logObj.CreateLog("NotifyHybridForWeightScale- --redisValues-- " + redisValues + "--firebase_id--" + firebase_id + " --GUID--" + guId + " --webAddr--" + webAddr + "--result--" + result, Session.SessionID, "NotifyHybridForWeightScale", "new", "true");
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("NotifyHybridForWeightScale- --redisValues-- " + redisValues + "--firebase_id--" + firebase_id + " --GUID--" + guId + " --webAddr--" + webAddr + "--result--" + result + " --Exception Message--" + ex.Message, Session.SessionID, "NotifyHybridForWeightScale", "new", "true");
            }
            result = "success";
            return result;
        }

        [WebMethod(EnableSession = true)]
        public void SetHybridWeightScaleInfo(dynamic weightScaleInfo)
        {
            Util.Util util = new Util.Util();
            LogFile.Log logObj = new LogFile.Log();

            string result = JsonConvert.SerializeObject(weightScaleInfo);
            var parsedResult = JObject.Parse(result);
            string strGUID = parsedResult.SelectToken("GUID").Value<string>().ToLower();

            try
            {
                FDR fObj = null;
                try
                {
                    fObj = (FDR)Session["FDR"];
                }
                catch (Exception ex)
                {
                    Session["FDR"] = null;
                }
                if (fObj == null)
                {
                    fObj = new FDR();
                    Session["FDR"] = fObj;
                }
                string weightScaleKey = Constants.AXHYBRIDWEIGHTSCALEINFO;

                FDW fdwObj = new FDW();
                fdwObj.SaveInRedisServer(util.GetRedisServerkey(weightScaleKey, strGUID), result, Constants.AXHYBRIDWEIGHTSCALEINFO, "♠");
                logObj.CreateLog("SetHybridWeightScaleInfo-" + weightScaleInfo + " --strGUID--" + strGUID + "--result--" + result, Session.SessionID, "SetHybridWeightScaleInfo", "new", "true");
            }
            catch (Exception ex)
            {
                logObj.CreateLog("SetHybridWeightScaleInfo-" + weightScaleInfo + " --strGUID--" + strGUID + "--result--" + result + " --Exception Message--" + ex.Message, Session.SessionID, "SetHybridWeightScaleInfo", "new", "true");
            }
        }

        [WebMethod(EnableSession = true)]
        public string NotifyCloseWeightScalePort(string guId)
        {
            string result = string.Empty;
            string redisValues = "";
            string firebase_id = string.Empty;
            string webAddr = "";
            try
            {
                if (guId == "")
                    return "";

                Util.Util util = new Util.Util();
                FDR fObj = null;
                string GUID = string.Empty;
                string schemaName = string.Empty;
                try
                {
                    fObj = (FDR)Session["FDR"];
                }
                catch (Exception ex)
                {
                    Session["FDR"] = null;
                }
                if (fObj == null)
                {
                    fObj = new FDR();
                    Session["FDR"] = fObj;
                }
                string notificationKey = Constants.AXHYBRIDNOTIFIINFO;
                schemaName = "♠";
                try
                {
                    redisValues = fObj.StringFromRedis(util.GetRedisServerkey(notificationKey, guId), schemaName).ToString();
                    dynamic mbNotify = JsonConvert.DeserializeObject(redisValues);
                    firebase_id = mbNotify.firebase_id.Value;
                }
                catch (Exception ex)
                {
                    return "Redis error";
                }
                if (Session["ARM_URL"] != null && Session["ARM_URL"].ToString() != "")
                    webAddr = Session["ARM_URL"].ToString() + "/api/v1/SendFCMNotification";
                else if (Session["AxFCMSendMsgURL"] != null && Session["AxFCMSendMsgURL"].ToString() != "")
                    webAddr = Session["AxFCMSendMsgURL"].ToString();

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add("Authorization:key=" + firebase_server_key);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"message\":{\"token\":[\"" + firebase_id + "\"],\"data\": {\"isaxp_weightscaleclose\":\"true\"}}}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("NotifyCloseWeightScalePort- --redisValues-- " + redisValues + "--firebase_id--" + firebase_id + " --GUID--" + guId + " --webAddr--" + webAddr + "--result--" + result + " --Exception Message--" + ex.Message, Session.SessionID, "NotifyCloseWeightScalePort", "new", "true");
            }
            return "";
        }

        [WebMethod(EnableSession = true)]
        public string CallApiDefinations(string api = "", string type = "axpert", string cacheInfo = "[]")
        {
            if (HttpContext.Current.Session["Project"] == null || Convert.ToString(HttpContext.Current.Session["Project"]) == string.Empty)
            {
                return utils.SESSTIMEOUT;
            }

            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            LogFile.Log logobj = new LogFile.Log();

            ArrayList nonCachedApi = new ArrayList();
            string baseName = string.Empty;
            string apiNameKeyPrefix = string.Empty;

            try
            {
                baseName = JArray.Parse(cacheInfo)[0]["id"].ToString();
            }
            catch (Exception ex)
            { }

            apiNameKeyPrefix = type.ToUpper() + "-" + (baseName != string.Empty ? baseName + "-" : "");

            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();

            string returnString = string.Empty;

            bool cacheMeightExist = false;
            try
            {
                cacheMeightExist = utilObj.getExpiredCache(cacheInfo, JObject.Parse("{\"id\": \"id\", \"cache\": \"cachedata\", \"cachedTime\": \"cachedTime\", \"refreshAfter\": \"autorefresh\"}")) == string.Empty;
            }
            catch (Exception ex)
            { }

            JObject returnObject = new JObject();

            if (api == string.Empty)
            {
                return returnObject.ToString();
            }

            if (cacheMeightExist)
            {
                string[] apiArr = api.Split(',');

                foreach (string apiName in apiArr)
                {
                    if (apiName == string.Empty)
                    {
                        continue;
                    }

                    string redisData = fObj.StringFromRedis(fObj.MakeKeyName(Constants.REDISAPIKEYS, apiNameKeyPrefix + apiName), schemaName);

                    if (redisData != string.Empty)
                    {
                        try
                        {
                            returnObject[apiName] = JToken.Parse(redisData);
                            //nonCachedApi.Add(apiName);
                        }
                        catch (Exception ex)
                        {
                            returnObject[apiName] = redisData;
                            //nonCachedApi.Add(apiName);
                        }
                    }
                }
            }

            if (type.ToLower() == "axpert")
            {

                try
                {
                    string wgData = string.Empty;

                    //string[] roles = System.Web.HttpContext.Current.Session["userroles"].ToString().Split(',');

                    string apiNameString = string.Join(", ", api.Split(',').Where(apiName =>
                    {
                        return returnObject[apiName] == null || returnObject[apiName].ToString() == string.Empty;
                    }).Select(apiName =>
                    {
                        return ("'" + apiName + "'");
                    }));

                    if (apiNameString == string.Empty)
                    {
                        return returnObject.ToString();
                    }

                    string sqlInput = "select a.execapidefname, a.execapiurl, a.apiresponsetype, a.execapimethod, a.apiresponseformat, a.execapirequeststring from executeapidef a where a.execapidefname in (" + apiNameString + ")";

                    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                    wgData = asbExt.ExecuteSQL("", sqlInput, "JSON");



                    JObject resultRows = JObject.Parse(wgData);

                    if (wgData != string.Empty)
                    {
                        try
                        {

                            string qdata = string.Empty;

                            JObject wgResult = new JObject();

                            JArray apiInfos = (JArray)resultRows["result"]["row"];

                            foreach (JObject apiResp in apiInfos)
                            {
                                try
                                {
                                    string requestName = apiResp["execapidefname"].ToString();
                                    string requestUrl = apiResp["execapiurl"].ToString();
                                    string requestMethod = apiResp["execapimethod"].ToString();
                                    string requestData = apiResp["execapirequeststring"].ToString();
                                    string responseType = apiResp["apiresponsetype"].ToString();
                                    string responseFormat = apiResp["apiresponseformat"].ToString();

                                    string apiResponse = string.Empty;

                                    if (responseType.ToLower() == "axpert" || requestMethod.ToLower() == "post")
                                    {
                                        apiResponse = GetDataFromPostAPI(requestUrl, responseFormat, requestData);
                                    }
                                    else
                                    {
                                        apiResponse = GetDataFromGetAPI(requestUrl, responseType, requestMethod);
                                    }

                                    if (requestName != string.Empty)
                                    {
                                        try
                                        {
                                            returnObject[requestName] = JToken.Parse(apiResponse);
                                            nonCachedApi.Add(requestName);
                                        }
                                        catch (Exception ex)
                                        {
                                            returnObject[requestName] = apiResponse;
                                            nonCachedApi.Add(requestName);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                        }
                        catch (Exception ex)
                        { }
                    }


                }
                catch (Exception ex)
                { }
            }
            else if (type.ToLower() == "sql")
            {
                try
                {
                    string apiNameString = string.Join(",", api.Split(',').Where(apiName =>
                    {
                        return returnObject[apiName] == null || returnObject[apiName].ToString() == string.Empty;
                    }));

                    if (apiNameString == string.Empty)
                    {
                        return returnObject.ToString();
                    }

                    AxpertInterGetCustomSql objWData = new AxpertInterGetCustomSql(apiNameString, new ArrayList());
                    string apiResponses = objWData.GetCustomSqlData().ToString();

                    if (apiResponses != string.Empty)
                    {
                        try
                        {
                            JObject apiResponsesObject = JObject.Parse(apiResponses);

                            foreach (var apiResponse in apiResponsesObject)
                            {
                                returnObject[apiResponse.Key] = apiResponse.Value;
                                if (apiResponse.Key != "error")
                                {
                                    nonCachedApi.Add(apiResponse.Key);
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            FDW fdwObj = new FDW();
            bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;

            if (isRedisConnected)
            {
                foreach (string cachingApi in nonCachedApi)
                {
                    try
                    {
                        if (returnObject[cachingApi] != null)
                        {
                            bool saved = fdwObj.SaveInRedisServer(fObj.MakeKeyName(Constants.REDISAPIKEYS, apiNameKeyPrefix + cachingApi), returnObject[cachingApi].ToString(), "", schemaName);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
            }

            returnString = returnObject.ToString();

            return returnString;
        }

        [WebMethod(EnableSession = true)]
        public string GetDataFromGetAPI(string ApiUrl, string ApiMethodType, string ApiRespType)
        {
            string result = string.Empty;
            ApiMethodType = ApiMethodType.ToUpper();
            string DATA = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);
            request.Method = "GET";
            request.ContentType = "application/" + ApiRespType;

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                result = responseReader.ReadToEnd();
                Console.Out.WriteLine(result);
                responseReader.Close();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetDataFromPostAPI(string ApiUrl, string ApiRespType, string ApiData)
        {
            string result = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);

            //string DATA = string.Empty;
            //if (apiType == "Axpert")
            //{
            request.Method = "POST";
            request.ContentType = "application/" + ApiRespType;
            //DATA = strDetails[5];
            ApiData = ParseAxpertApiParams(ApiData, "sqlparams", "", null);
            request.ContentLength = ApiData.Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(ApiData);
            requestWriter.Close();
            //}

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                result = responseReader.ReadToEnd();
                Console.Out.WriteLine(result);
                responseReader.Close();

                //if (result != string.Empty && apiType.ToLower() == "external")
                //{
                //    result = ApiResultParser(result, contType, mapname, fldName, fromApiName, fromApiExp);
                //}
                //else if (result != string.Empty)
                //{

                if (result != string.Empty && ApiRespType.ToLower() == "xml")
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(result);

                        result = JsonConvert.SerializeXmlNode(doc);

                    }
                    catch (Exception ex) { }
                }

                try
                {
                    JObject obj = JObject.Parse(result);
                    result = obj["result"].First.ToString();
                }
                catch (Exception ex) { }

                //}
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }


        [WebMethod(EnableSession = true)]
        public string CallExecuteScriptAPI(string apifrom, string tstDataId, string apiButtonName, string apiInfo, string resTstHtmlLS)
        {
            string result = string.Empty;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            try
            {
                if (apifrom == "t")
                {
                    if (resTstHtmlLS != "")
                    {
                        utilObj.DeleteTstIvObject(tstDataId.Split('_')[0]);
                        tstDataId = utilObj.GetReGenTstDataObj(tstDataId, resTstHtmlLS);
                        _tstKeyChanged = tstDataId;
                    }
                    TStructData tstData = (TStructData)Session[tstDataId];
                    if (tstData == null)
                        return Constants.DUPLICATESESS;
                    TStructDef tstStrObj = tstData.tstStrObj;
                    int fldidx = tstStrObj.GetButtonIndex(apiButtonName);
                    TStructDef.ButtonStruct btn = (TStructDef.ButtonStruct)tstStrObj.btns[fldidx];
                    string api = btn.btnapi.url;
                    string apiType = btn.btnapi.restype;
                    string contType = btn.btnapi.resformat;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);

                    string DATA = string.Empty;
                    if (apiType == "Axpert")
                    {
                        request.Method = "POST";
                        request.ContentType = "application/" + contType;
                        DATA = btn.btnapi.reqstr;
                        DATA = ParseAxpertApiParams(DATA, "apiparams", tstData.fieldValueXml, tstStrObj);
                        request.ContentLength = DATA.Length;
                        StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                        requestWriter.Write(DATA);
                        requestWriter.Close();
                    }
                    else
                    {
                        request.Method = "GET";
                        request.ContentType = "application/" + contType;
                    }
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    result = responseReader.ReadToEnd();
                    Console.Out.WriteLine(result);
                    responseReader.Close();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Call ExecuteScript APi.-" + ex.Message, sessionid, "ExecuteScriptAPi-" + apiButtonName, "new", "true");
            }
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetAxpertCunfigInfo(string additionalInfo)
        {
            string axpertdeveloperUrl = string.Empty;
            try
            {
                if (ConfigurationManager.AppSettings["axpertdeveloper"] != null)
                {
                    string adDetails = HttpContext.Current.Session["project"].ToString();
                    adDetails += "♦" + HttpContext.Current.Session["user"].ToString();
                    adDetails += "♦" + HttpContext.Current.Session["pwd"].ToString();
                    adDetails += "♦" + HttpContext.Current.Session["language"].ToString();
                    adDetails += "♦" + HttpContext.Current.Session["hybridGUID"].ToString();
                    adDetails += "♦" + HttpContext.Current.Session["AxAppTitle"].ToString();
                    adDetails += "♦" + HttpContext.Current.Session["userDetails"].ToString();
                    if (additionalInfo != "")
                        adDetails += "♦" + additionalInfo;
                    axpertdeveloperUrl = ConfigurationManager.AppSettings["axpertdeveloper"].ToString();
                    adDetails = utils.encrtptDecryptAES(adDetails);
                    axpertdeveloperUrl += "aspx/dwbsignin.aspx?adInfo=" + adDetails;
                }
            }
            catch (Exception ex)
            {
                axpertdeveloperUrl = string.Empty;
            }
            return axpertdeveloperUrl;
        }

        [WebMethod(EnableSession = true)]
        public string GetInternalSSOToken(string _thisProj)
        {
            string InterSSOToken = string.Empty;
            if (_thisProj != string.Empty)
            {
                try
                {
                    string _thisUser = string.Empty;
                    if (HttpContext.Current.Session["username"] != null && HttpContext.Current.Session["username"].ToString() != "")
                        _thisUser = HttpContext.Current.Session["username"].ToString();
                    else
                        return "";
                    string response = string.Empty;
                    string DATA = string.Empty;
                    string URL = GetArmScriptURL() + "ASBDefineRest.dll/datasnap/rest/TASBDefineRest/GetInternalSSOKey";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    DATA = "{\"_parameters\":[{\"getssokey\":{\"axpapp\": \"" + _thisProj + "\",\"username\":\"" + _thisUser + "\"}}]}";
                    request.ContentLength = DATA.Length;
                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                    requestWriter.Write(DATA);
                    requestWriter.Close();

                    try
                    {
                        WebResponse webResponse = request.GetResponse();
                        Stream webStream = webResponse.GetResponseStream();
                        StreamReader responseReader = new StreamReader(webStream);
                        response = responseReader.ReadToEnd();
                        //Console.Out.WriteLine(response);
                        responseReader.Close();

                        try
                        {
                            JArray jData = null;
                            JObject tData = JObject.Parse(response);
                            jData = (JArray)tData["result"];
                            string authKey = jData[0]["result"][0]["key"].ToString();
                            InterSSOToken = "proj=" + _thisProj + "&username=" + _thisUser + "&uKey=" + authKey + "&lang=" + Session["language"].ToString();

                            /// 
                            /// 
                            ///

                            InterSSOToken = utilObj.encrtptDecryptAES(InterSSOToken);
                            //This URL is hardcoded for the understanding purpose which need to be constructed for the production instant.
                            InterSSOToken = "http://demo.agile-labs.com/aspx/signin.aspx?InternalSSO=" + InterSSOToken;
                        }
                        catch (Exception exe)
                        {
                            InterSSOToken = string.Empty;
                        }
                    }
                    catch (Exception e)
                    {
                        InterSSOToken = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    InterSSOToken = string.Empty;
                }
            }
            return InterSSOToken;
        }

        [WebMethod(EnableSession = true)]
        public string ImportToQueueAPI(string jsonData, string transid)
        {
            string response = string.Empty;
            try
            {
                string DATA = string.Empty;
                string URL = ConfigurationManager.AppSettings["ImportBrokerURL"] + "/import";// "http://172.16.0.85:3000/import";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                string appsesskey = Session["AppSessionKey"].ToString();
                string globalvars = xmltojson(Session["axGlobalVars"].ToString());
                string uservars = xmltojson(Session["axUserVars"].ToString());
                string axapps = xmltojson(Session["axApps"].ToString());
                string axprops = HttpContext.Current.Application["axProps"].ToString();
                axprops = xmltojson(axprops);
                jsonData = jsonData.Substring(0, jsonData.Length - 2);
                jsonData = jsonData + "," + "\"transid\" :\"" + transid + "\"";
                jsonData = jsonData + "," + "\"sessionid\" :\"" + Session.SessionID + "\"";
                jsonData = jsonData + "," + "\"username\" :\"" + Session["username"].ToString() + "\"";
                DATA = jsonData + "," + "\"" + HttpContext.Current.Session["project"].ToString() + "\":{" + axapps + "},\"axprops\":{" + axprops + "}}";
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();
                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    return response;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("ImportToQueueAPI -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "ImportToQueueAPI-" + transid, "new");
                return "Error: While Import to Queue";
            }
        }

        [WebMethod(EnableSession = true)]
        public string SaveTransactionRestAPI(string jsonData, string transId, string recordId, string changedDcs, string deledtedRows, string hfiles, string tstDataId, ArrayList deletedFldArrayValues, string resTstHtmlLS)
        {
            string response = string.Empty;
            string DATA = string.Empty;
            string URL = String.Empty;
            if (HttpContext.Current.Session["ARM_URL"] != null && HttpContext.Current.Session["ARMPushToQueue_API"] != null)
                URL = HttpContext.Current.Session["ARM_URL"].ToString() + HttpContext.Current.Session["ARMPushToQueue_API"].ToString();

            bool isRMQExist = false;
            string rmqstatus = CheckRabbitMqStatus("RapidSaveQueue");
            if (rmqstatus == "OK")
                isRMQExist = true;
            else if (rmqstatus != "Queue does not exist or queue name incorrect")
                return "RMQError:" + rmqstatus;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            if (changedDcs != "")
            {
                changedDcs = changedDcs.Substring(0, changedDcs.Length - 1);
            }

            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(tstDataId.Split('_')[0]);
                tstDataId = utilObj.GetReGenTstDataObj(tstDataId, resTstHtmlLS);
                _tstKeyChanged = tstDataId;
            }
            TStructData tstData = (TStructData)Session[tstDataId];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            string deledtedRowsJson = string.Empty;
            if (deledtedRows != "")
            {
                deledtedRowsJson = tstData.GetDelRowsNode(deledtedRows);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(deledtedRowsJson);
                deledtedRows = JsonConvert.SerializeXmlNode(doc);
                deledtedRows = deledtedRows.Remove(deledtedRows.IndexOf("{"), 1);
                deledtedRows = deledtedRows.Remove(deledtedRows.LastIndexOf("}"), 1);
                deledtedRows += ",";
            }

            string appsesskey = Session["AppSessionKey"].ToString();
            string globalvars = xmltojson(Session["axGlobalVars"].ToString());
            string uservars = xmltojson(Session["axUserVars"].ToString());
            string axapps = xmltojson(Session["axApps"].ToString());
            jsonData = jsonData.Remove(jsonData.Length - 2);
            string pwd = utilObj.MD5Hash(Session["pwd"].ToString());
            string axprops = HttpContext.Current.Application["axProps"].ToString();
            axprops = xmltojson(axprops);
            string axappsNode = HttpContext.Current.Session["project"].ToString();
            DATA = "{\"_parameters\":[{\"savedata\":{\"cachedsave\":\"true\",\"axpapp\":\"" + HttpContext.Current.Session["project"].ToString() + "\",\"appsessionkey\":\"" + appsesskey + "\" " + ",\"transid\":\"" + transId + "\",\"s\":\"" + Session.SessionID + "\",\"username\":\"" + Session["username"].ToString() + "\",\"password\":\"" + pwd + "\",\"afiles\":\"" + hfiles + "\",\"changedrows\":{" + changedDcs + "},\"trace\":\"" + HttpContext.Current.Session["AxTrace"].ToString() + "\",\"recordid\":\"" + recordId + "\"," + deledtedRows + "";
            //DATA += jsonData + "}],\"globalvars\":{" + globalvars + "},\"uservars\":{" + uservars + "},\"axapps\":{" + axapps + "}";
            //DATA += "}}]}";
            DATA += jsonData + "}]},\"" + axappsNode + "\":{" + axapps + "},\"axprops\":{" + axprops + "},\"globalvars\":{" + globalvars + "},\"uservars\":{" + uservars + "}";
            DATA += "}]}";

            if (tstData.tstStrObj.FEncryptFlag.Count > 0)
            {
                DATA = CachedSaveFldDataEncrypt(DATA, tstData.tstStrObj.FEncryptFlag);
            }

            var saveDetails = "{\"queuename\":\"RapidSaveQueue\",\"queuedata\":" + JsonConvert.SerializeObject(DATA) + "}";
            request.ContentLength = saveDetails.Length;

            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(saveDetails);
            //requestWriter.Write(DATA);
            requestWriter.Close();

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
                if (!isRMQExist)
                {
                    string _rmqstatus = CheckRabbitMqStatus("ARMExportQueue");
                    if (_rmqstatus != "OK")
                        return "RMQError:" + _rmqstatus;
                }
                try
                {
                    if (response != "" && response.IndexOf("\"success\":true") > -1)
                    {
                        if (deletedFldArrayValues.Count > 0)
                            DeleteCachedSaveFiles(deletedFldArrayValues, tstData);
                        else
                            HttpContext.Current.Session["AxpAttFileServer"] = null;
                    }
                }
                catch (Exception ex) { }
                if (_tstKeyChanged != "")
                    response = _tstKeyChanged + "♠*♠" + response;
                return response;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
                if (_tstKeyChanged != "")
                    response = _tstKeyChanged + "♠*♠" + response;
                return response;
            }
        }

        [WebMethod(EnableSession = true)]
        public string ExportARMPushToQueue(string iviewName, string iviewParams, string ivParamCaption, string isListView, string IVIRCaption, string dtFormat)
        {
            string response = string.Empty;
            if (HttpContext.Current.Session["project"] == null || Session["nsessionid"] == null)
                return utilObj.SESSTIMEOUT;
            string DATA = string.Empty;
            string URL = String.Empty;
            string ArmScriptURL = String.Empty;
            string axConfigFilePath = String.Empty;
            if (HttpContext.Current.Session["ARM_URL"] != null && HttpContext.Current.Session["ARMPushToQueue_API"] != null)
                URL = HttpContext.Current.Session["ARM_URL"].ToString() + HttpContext.Current.Session["ARMPushToQueue_API"].ToString();
            if (HttpContext.Current.Session["ARM_Scripts_URL"] != null)
                ArmScriptURL = HttpContext.Current.Session["ARM_Scripts_URL"].ToString();

            if (URL == string.Empty || ArmScriptURL == string.Empty)
                return "error:ARM Connection defined";

            bool isRMQExist = false;
            string rmqstatus = CheckRabbitMqStatus("ARMExportQueue");
            if (rmqstatus == "OK")
                isRMQExist = true;
            else if (rmqstatus != "Queue does not exist or queue name incorrect")
                return "RMQError:" + rmqstatus;

            if (HttpContext.Current.Session["AxConfigFileUploadPath"] != null)
            {
                axConfigFilePath = HttpContext.Current.Session["AxConfigFileUploadPath"].ToString();
                axConfigFilePath = axConfigFilePath.Replace(@"\", "\\\\");
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            string iviewParamsJson = "{}";
            if (iviewParams != "")
            {
                var pairs = iviewParams.Split(new[] { "~" }, StringSplitOptions.None);
                pairs[0] = pairs[0].Trim('"');
                pairs[pairs.Length - 1] = pairs[pairs.Length - 1].Trim('"');
                var dictionary = new Dictionary<string, string>();
                foreach (var pair in pairs)
                {
                    var keyValue = pair.Split('♠');
                    if (keyValue.Length == 2)
                    {
                        string kyval = keyValue[1];
                        kyval = kyval.Replace("&grave;", "~").Replace("&amp;", "&");
                        dictionary[keyValue[0]] = kyval;
                    }
                }
                iviewParamsJson = JsonConvert.SerializeObject(dictionary);
            }

            string iviewParamsJsonCap = "{}";
            if (ivParamCaption != "")
            {
                var pairs = ivParamCaption.Split(new[] { "♥" }, StringSplitOptions.None);
                pairs[0] = pairs[0].Trim('"');
                pairs[pairs.Length - 1] = pairs[pairs.Length - 1].Trim('"');
                var dictionary = new Dictionary<string, string>();
                foreach (var pair in pairs)
                {
                    if (pair != "")
                    {
                        var keyValue = pair.Split(':');
                        if (keyValue.Length == 2)
                        {
                            string kyval = keyValue[1];
                            kyval = kyval.Replace("&grave;", "~").Replace("&amp;", "&");
                            dictionary[keyValue[0]] = kyval;
                        }
                    }
                }
                iviewParamsJsonCap = JsonConvert.SerializeObject(dictionary);
            }

            string purpose = string.Empty;
            if (isListView == "True")
                purpose = "\"purpose\":\"list\",\"pageno\":\"0\",\"pagesize\":\"0\",";
            else
                purpose = "\"pageno\":\"0\",\"pagesize\":\"0\",";
            string globalVars = HttpContext.Current.Session["axGlobalVars"].ToString();
            globalVars = globalVars.Replace(@"\", "\\\\");
            string _axapps = HttpContext.Current.Session["axApps"].ToString();
            _axapps = _axapps.Replace(@"\", "\\\\");
            DATA = "{\"getreport\":{\"trace\":\"" + HttpContext.Current.Session["AxTrace"].ToString() + "\"," + purpose + "\"metadata\":\"true\",\"fetchhidcol\":\"false\",\"_dateformat\":\"" + dtFormat + "\",\"ivcaption\":\"" + IVIRCaption + "\",\"sqlpagination\":\"false\",\"params\":" + iviewParamsJson + ",\"ivParamCaption\":" + iviewParamsJsonCap + ",\"project\":\"" + HttpContext.Current.Session["project"].ToString() + "\",\"name\":\"" + iviewName + "\",\"token\":\"d152232e23ff6523b8104196ff1b6015\",\"seed\":\"211269\",\"userauthkey\":\"" + Session["username"].ToString() + "\",\"s\":\"" + Session.SessionID + "\",\"username\":\"" + Session["username"].ToString() + "\",\"arm_url\":\"" + HttpContext.Current.Session["ARM_URL"].ToString() + "\",\"ARMScriptURL\":\"" + ArmScriptURL + "\",\"axConfigFilePath\":\"" + axConfigFilePath + "\"},\"axprops\":\"" + HttpContext.Current.Application["axProps"].ToString() + "\",\"axapps\":\"" + _axapps + "\",\"globalvars\":\"" + globalVars + "\",\"uservars\":\"" + HttpContext.Current.Session["axUserVars"].ToString() + "\"}";
            var saveDetails = "{\"queuename\":\"ARMExportQueue\",\"queuedata\":" + JsonConvert.SerializeObject(DATA) + ",\"timespandelay\":\"0\"}";
            request.ContentLength = saveDetails.Length;

            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(saveDetails);
            requestWriter.Close();

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
                if (!isRMQExist)
                {
                    string _rmqstatus = CheckRabbitMqStatus("ARMExportQueue");
                    if (_rmqstatus != "OK")
                        return "RMQError:" + _rmqstatus;
                    else
                        return response;
                }
                else
                    return response;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
                return "error:" + e.Message;
            }
        }


        [WebMethod(EnableSession = true)]
        public string ARMExportPushToQueue(string transid, string type, string output, string page, string dateformat, string fields, string entityName)
        {
            string response = string.Empty;

            if (HttpContext.Current.Session["project"] == null || HttpContext.Current.Session["nsessionid"] == null)
                return "Session timeout";

            string URL = String.Empty;
            string ArmScriptURL = string.Empty;
            string axConfigFilePath = string.Empty;
            string armurl = HttpContext.Current.Session["ARM_URL"].ToString();
            string apiURL = HttpContext.Current.Session["ARM_URL"].ToString() + "/api/v1/GetEntityListPageLoadData";


            if (HttpContext.Current.Session["ARM_URL"] != null && HttpContext.Current.Session["ARMPushToQueue_API"] != null)
                URL = HttpContext.Current.Session["ARM_URL"].ToString() + HttpContext.Current.Session["ARMPushToQueue_API"].ToString();
            if (HttpContext.Current.Session["ARM_Scripts_URL"] != null)
                ArmScriptURL = HttpContext.Current.Session["ARM_Scripts_URL"].ToString();

            if (URL == string.Empty || ArmScriptURL == string.Empty)
                return "Error: ARM Connection is not defined";

            if (HttpContext.Current.Session["AxConfigFileUploadPath"] != null)
            {
                axConfigFilePath = HttpContext.Current.Session["AxConfigFileUploadPath"].ToString();
                axConfigFilePath = axConfigFilePath.Replace(@"\", "\\\\");
            }

            if (HttpContext.Current.Session["ARM_Token"] == null || HttpContext.Current.Session["ARM_Token"].ToString() == "")
                return "Error: ARM Token is not defined";

            bool isRMQExist = false;
            string rmqstatus = CheckRabbitMqStatus("ARMExportQueue");
            if (rmqstatus == "OK")
                isRMQExist = true;
            else if (rmqstatus != "Queue does not exist or queue name incorrect")
                return "RMQError:" + rmqstatus;

            AnalyticsUtils _aUtils = new AnalyticsUtils();
            string armSessionId = _aUtils.ARMSessionId;
            string roles = HttpContext.Current.Session["AxRole"].ToString();
            string sessionId = HttpContext.Current.Session.SessionID;
            string token = HttpContext.Current.Session["ARM_Token"].ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";

            string DATA = JsonConvert.SerializeObject(new
            {
                Page = page,
                ARMSessionId = armSessionId,
                AxSessionId = sessionId,
                Trace = HttpContext.Current.Session["AxTrace"],
                AppName = HttpContext.Current.Session["project"].ToString(),
                Roles = roles,
                UserName = HttpContext.Current.Session["username"].ToString(),
                TransId = transid,
                Type = type,
                Output = output,
                ARMUrl = armurl,
                APIURL = apiURL,
                DateFormat = dateformat,
                ARMScriptURL = ArmScriptURL,
                AxConfigFilePath = axConfigFilePath,
                Token = token,
                EntityName = entityName,
                Fields = fields
            });

            string saveDetails = JsonConvert.SerializeObject(new
            {
                queuename = "ARMExportQueue",
                queuedata = DATA

            });

            request.ContentLength = saveDetails.Length;

            using (StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII))
            {
                requestWriter.Write(saveDetails);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
                if (!isRMQExist)
                {
                    string _rmqstatus = CheckRabbitMqStatus("ARMExportQueue");
                    if (_rmqstatus != "OK")
                        return "RMQError:" + _rmqstatus;
                    else
                        return response;
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(ex.Message);
                return "error:" + ex.Message;
            }
        }
        [WebMethod(EnableSession = true)]
        public string checkRedisKeyARMExport(string iviewName, string iviewParams)
        {
            string res = string.Empty;
            if (HttpContext.Current.Session["project"] == null || Session["nsessionid"] == null)
                return utilObj.SESSTIMEOUT;
            string _RKey = string.Empty;
            if (iviewParams != "")
                _RKey = iviewName + "-ivarmexportexcel-" + HttpContext.Current.Session["username"].ToString() + "-" + iviewParams;
            else
                _RKey = iviewName + "-ivarmexportexcel-" + HttpContext.Current.Session["username"].ToString();

            FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
            string armExportExcell = fdrObj.ReadStringKey(_RKey);
            if (armExportExcell != string.Empty && armExportExcell == "requestinprocess")
            {
                //return "warning:Previous request still in process. Please wait till you get the notification.";
                res = "keyexist";
            }
            else
            {
                res = "nokeyexist";
            }
            return res;
        }
        protected void DeleteCachedSaveFiles(ArrayList deletedFldArrayValues, TStructData tstData)
        {
            try
            {
                if (deletedFldArrayValues.Count > 0)
                {
                    tstData.GetGlobalAttachPath();
                    TStructDef strObj = tstData.tstStrObj;
                    ArrayList lstAttFileServer = new ArrayList();
                    if (HttpContext.Current.Session["AxpAttFileServer"] != null)
                        lstAttFileServer = (ArrayList)HttpContext.Current.Session["AxpAttFileServer"];
                    foreach (var delFile in deletedFldArrayValues)
                    {
                        string[] dltfnameval = delFile.ToString().Split('~');
                        string dlFName = dltfnameval[0].Substring(0, dltfnameval[0].LastIndexOf("F") - 3);
                        string dlRow = dltfnameval[0].Substring(dltfnameval[0].LastIndexOf("F") - 3, 3);
                        var dlFullname = lstAttFileServer.Cast<string>().Where(i => i.StartsWith(dlFName + "~") && i.EndsWith(dltfnameval[1])).ToArray();
                        // TO delete file from folder and remove from list which are deleted before save the transaction
                        if (dlFullname.Count() > 0)
                        {
                            lstAttFileServer.Remove(dlFullname[0]);
                            string dlFullPath = dlFullname[0].Replace(dlFName + "~", "");
                            RemoveDeletedAxpFileFromFolder(dlFullPath);
                        }
                        else
                        {
                            // To delete file from folder after load transaction and delete files                        
                            var axpfilepath_ = strObj.AxpFileUploadFields.Cast<string>().Where(i => i.ToLower().StartsWith("axpfilepath") && i.EndsWith("_" + dlFName.ToString().Substring(8))).ToArray();
                            int idx = strObj.GetFieldIndex(dlFName.ToString());
                            TStructDef.FieldStruct fld = (TStructDef.FieldStruct)strObj.flds[idx];
                            int axpFldDcNo = fld.fldframeno;
                            DataTable dcTable = tstData.dsDataSet.Tables["dc" + axpFldDcNo];
                            bool isDcGrid = false;
                            isDcGrid = tstData.IsDcGrid(axpFldDcNo.ToString(), strObj);
                            int rNo = 0;
                            if (isDcGrid)
                                rNo = int.Parse(dlRow) - 1;
                            else
                                rNo = int.Parse(dlRow);
                            string axpFPath = string.Empty;
                            if (dcTable.Rows.Count > 0)
                                axpFPath = dcTable.Rows[rNo][axpfilepath_[0]].ToString();
                            if (axpFPath == string.Empty)
                                axpFPath = HttpContext.Current.Session["grdAttPath"].ToString();
                            if (axpFPath == string.Empty && HttpContext.Current.Session["AxConfigFileUploadPath"] != null && HttpContext.Current.Session["AxConfigFileUploadPath"].ToString() != "")
                                axpFPath = HttpContext.Current.Session["AxConfigFileUploadPath"].ToString();
                            if (axpFPath.EndsWith("*"))
                            {
                                string axpFilePath = axpFPath.Substring(0, axpFPath.LastIndexOf('\\')) + "\\";
                                string PrefixFilename = axpFPath.Substring(axpFPath.LastIndexOf("\\") + 1).Replace("*", "");
                                string axpFileName = dltfnameval[1];
                                if (PrefixFilename != string.Empty)
                                    axpFileName = PrefixFilename + axpFileName;
                                if (axpFilePath != "" && !axpFilePath.EndsWith("\\"))
                                    axpFilePath += "\\";
                                RemoveDeletedAxpFileFromFolder(axpFilePath + axpFileName);
                            }
                            else
                            {
                                if (axpFPath != "" && !axpFPath.EndsWith("\\"))
                                    axpFPath += "\\";
                                RemoveDeletedAxpFileFromFolder(axpFPath + dltfnameval[1]);
                            }
                        }
                    }
                }
                HttpContext.Current.Session["AxpAttFileServer"] = null;
            }
            catch (Exception ex) { }
        }
        protected void RemoveDeletedAxpFileFromFolder(string fullPath)
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Error while RemoveDeletedAxpFileFromFolder-" + ex.Message, sessionid, "RemoveDeletedAxpFileCachedSave", "new");
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetResponsibilityDetails(string responsibility)
        {
            GetGlobalVars();
            string errorLog = logobj.CreateLog("Calling GetResponsibilityDetails Service", sessionid, "GetRespDetails", "new");
            string ixml = "<root axpapp='" + project + "' sessionid= '" + sessionid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' role='" + responsibility + "'>";
            ixml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            string result = asbExt.CallGetAccessRightsDetailsWS("Resp", ixml);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string EditResponsibilityDetails(string action, string responsibility, string saveXML, string isTstructSel, string isIviewSel)
        {
            GetGlobalVars();
            string fileName = "EditResponsibilityDetails", accessRgtXml = string.Empty, res = string.Empty;
            string errorLog = logobj.CreateLog("Loading Structure.", sessionid, fileName, "new");
            accessRgtXml = "<root axpapp='" + project + "' trace='" + errorLog + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' action='" + action + "'  role='" + responsibility + "' act=''  sessionid= '" + sessionid + "'>";
            if (isTstructSel != string.Empty)
                saveXML += "<Tstructs  parent=''>Tstructs</Tstructs>";
            if (isIviewSel != string.Empty)
                saveXML += "<Iviews  parent=''>Iviews</Iviews>";
            accessRgtXml += saveXML;
            accessRgtXml += Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            res = asbExt.CallProcessRoleWS("Resp", accessRgtXml);
            return res;
        }

        //Iview : download multiple attachmemts in zip format.
        [WebMethod(EnableSession = true)]
        public string GetZipPathForIviewAttachmentNew(string filepathjson)
        {
            JObject jsonOBJ = JObject.Parse(filepathjson);
            string result = string.Empty;
            string destPath = utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\ZipDownloads";
            if (HttpContext.Current.Session["project"] == null || Session["nsessionid"] == null)
                return utilObj.SESSTIMEOUT;
            try
            {
                if (!System.IO.Directory.Exists(destPath))
                    System.IO.Directory.CreateDirectory(destPath);
                else
                {
                    System.IO.Directory.Delete(destPath, true);
                    System.IO.Directory.CreateDirectory(destPath);
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetZipPathForIviewAttachmentNew -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in GetZipPathForIviewAttachmentNew", "new");

                return "Error: While Creating folder";

            }

            try
            {
                destPath = destPath + "\\";
                foreach (var x in jsonOBJ)
                {
                    string folderName = x.Key;
                    var filePathArray = x.Value;
                    string prefix = string.Empty;
                    string suffix = string.Empty;
                    System.IO.Directory.CreateDirectory(destPath + folderName);
                    foreach (string file in filePathArray)
                    {
                        string filepath = file;
                        if (file.IndexOf("?") != -1)
                        {
                            prefix = file.Split('?')[1].Split('&')[0];
                            suffix = file.Split('?')[1].Split('&')[1];
                            filepath = file.Split('?')[0];
                        }
                        string finaldestPath = destPath + folderName + "\\" + prefix + Path.GetFileNameWithoutExtension(filepath) + suffix + Path.GetExtension(filepath);

                        if (System.IO.File.Exists(filepath))
                            if (System.IO.File.Exists(finaldestPath))
                                System.IO.File.Copy(filepath, destPath + folderName + "\\" + Path.GetFileNameWithoutExtension(finaldestPath) + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(finaldestPath), true);
                            else
                                System.IO.File.Copy(filepath, destPath + folderName + "\\" + Path.GetFileName(finaldestPath), true);
                    }
                    using (ZipFile zipFile = new ZipFile(Encoding.UTF8)) //to handle file name characters in all languages(UTF8)
                    {
                        zipFile.AddDirectory(utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\ZipDownloads\\" + folderName);
                        zipFile.Save(utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\ZipDownloads\\" + folderName + ".zip");
                        if (result == string.Empty)
                            result = utilObj.ScriptsurlPath + "Axpert/" + Session["nsessionid"].ToString() + "/ZipDownloads/" + folderName + ".zip";
                        else
                            result += "," + utilObj.ScriptsurlPath + "Axpert/" + Session["nsessionid"].ToString() + "/ZipDownloads/" + folderName + ".zip";
                    }
                    System.IO.Directory.Delete(utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\ZipDownloads\\" + folderName, true);
                }

                if (result.IndexOf(",") > -1)
                    using (ZipFile zipFile = new ZipFile(Encoding.UTF8)) //to handle file name characters in all languages(UTF8)
                    {
                        zipFile.AddDirectory(utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\ZipDownloads");
                        zipFile.Save(utilObj.ScriptsPath + "axpert\\" + Session["nsessionid"].ToString() + "\\ZipDownloads\\SelectedFiles.zip");
                        result = utilObj.ScriptsurlPath + "Axpert/" + Session["nsessionid"].ToString() + "/ZipDownloads/SelectedFiles.zip";
                    }
            }
            catch (Exception ex)
            {
                result = "Error: Failure while processing the files.";

                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("GetZipPathForIviewAttachmentNew -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "Exception in GetZipPathForIviewAttachmentNew", "new");
            }

            return result;
        }

        private string GetUserPasswordForPEG()
        {
            if (HttpContext.Current.Session["mailapproval"] != null && HttpContext.Current.Session["mailapproval"].ToString() == "true")
            {
                string sessionId = HttpContext.Current.Session["nsessionid"].ToString();
                string result = "";
                string sqlQuery = "select password from axusers where username = @user";
                sqlQuery = sqlQuery.Replace("@user", "'" + HttpContext.Current.Session["username"].ToString() + "'");
                string inputXML = string.Empty;
                inputXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + sessionId + "' trace='' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlQuery + "</sql>";
                inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                result = objWebServiceExt.CallGetChoiceWebService(transid, inputXML);
                if (result != "")
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(result);
                    XmlNodeList xmlNodes = default(XmlNodeList);
                    xmlNodes = xmlDoc.SelectNodes("//sqlresultset/response/row/password");
                    result = xmlNodes[0].InnerXml;
                    return result;
                }
                else
                    return "";
            }
            else
            {
                return utilObj.MD5Hash(Session["pwd"].ToString());
            }
        }

        [WebMethod(EnableSession = true)]
        public string AmdApproveWS(string jsonData, string comments, string reasons, string isAmendment, string AxTstIsPEG)
        {
            string response = string.Empty;
            try
            {
                if (HttpContext.Current.Session["project"] == null)
                    return utilObj.SESSTIMEOUT;
                string DATA = string.Empty;
                string URL = GetArmScriptURL() + "ASBPegRest.dll/datasnap/rest/TASBPeg/AxApprove";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                string pwd = GetUserPasswordForPEG();
                string[] _amdData = jsonData.Split('♦');
                string globalvars = xmltojson(Session["axGlobalVars"].ToString());
                string uservars = xmltojson(Session["axUserVars"].ToString());
                string axprops = HttpContext.Current.Application["axProps"].ToString();
                axprops = xmltojson(axprops);
                string axapps = xmltojson(Session["AxApps"].ToString());
                string axappsProj = HttpContext.Current.Session["project"].ToString();
                if (isAmendment == "true")
                {
                    AxTstIsPEG = AxTstIsPEG == "" ? "false" : AxTstIsPEG;
                    DATA = @"{
                     ""axapprove"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""recordid"": """ + _amdData[6] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""approved"",
                     ""ispeg"":""" + AxTstIsPEG + @""",
                     ""isamendment"":""true"",
		             ""statustext"" : """ + comments + @""",
		             ""statusreason"" : """ + reasons + @""",
                     ""userdata"": { }
                },
                     """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": { " + globalvars + @"},
                     ""uservars"": { " + uservars + @"}
            }";
                }
                else
                {
                    DATA = @"{
                     ""axapprove"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""approved"",
                     ""ispeg"":""true"",
		             ""statustext"" : """ + comments + @""",
		              ""statusreason"" : """ + reasons + @""",
		             ""userdata"": {}
	                 },
                     """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";
                }
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    return response;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                    return response;
                }
            }
            catch (Exception ex) { }
            return response;
        }

        [WebMethod(EnableSession = true)]
        public string AmdReturnWS(string jsonData, string comments, string reasons, string retLevel, string isAmendment)
        {
            string response = string.Empty;
            try
            {
                if (HttpContext.Current.Session["project"] == null)
                    return utilObj.SESSTIMEOUT;
                string DATA = string.Empty;
                string URL = GetArmScriptURL() + "ASBPegRest.dll/datasnap/rest/TASBPeg/AxReturn";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                string pwd = GetUserPasswordForPEG();
                string[] _amdData = jsonData.Split('♦');
                string globalvars = xmltojson(Session["axGlobalVars"].ToString());
                string uservars = xmltojson(Session["axUserVars"].ToString());
                string axprops = HttpContext.Current.Application["axProps"].ToString();
                axprops = xmltojson(axprops);
                string axapps = xmltojson(Session["AxApps"].ToString());
                string axappsProj = HttpContext.Current.Session["project"].ToString();
                if (isAmendment == "true")
                {
                    DATA = @"{
                     ""axreturn"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""recordid"": """ + _amdData[6] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""returned"",
                     ""isamendment"":""true"",
		             ""statustext"" : """ + comments + @""",
		             ""statusreason"" : """ + reasons + @""",
		             ""userdata"": {}
	                 },
                     """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";
                }
                else
                {
                    DATA = @"{
                     ""axreturn"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""returned"",
                     ""ispeg"":""true"",
		             ""statustext"" : """ + comments + @""",
		             ""statusreason"" : """ + reasons + @""",
                     ""returnto"": """ + retLevel + @""",
		             ""userdata"": {}
	                 },
                     """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";
                }
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    return response;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                    return response;
                }
            }
            catch (Exception ex) { }
            return response;
        }

        [WebMethod(EnableSession = true)]
        public string AmdRejectWS(string jsonData, string comments, string reasons, string isAmendment)
        {
            string response = string.Empty;
            try
            {
                if (HttpContext.Current.Session["project"] == null)
                    return utilObj.SESSTIMEOUT;
                string DATA = string.Empty;
                string URL = GetArmScriptURL() + "ASBPegRest.dll/datasnap/rest/TASBPeg/AxReject";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                string pwd = GetUserPasswordForPEG();
                string[] _amdData = jsonData.Split('♦');
                string globalvars = xmltojson(Session["axGlobalVars"].ToString());
                string uservars = xmltojson(Session["axUserVars"].ToString());
                string axprops = HttpContext.Current.Application["axProps"].ToString();
                axprops = xmltojson(axprops);
                string axapps = xmltojson(Session["AxApps"].ToString());
                string axappsProj = HttpContext.Current.Session["project"].ToString();
                if (isAmendment == "true")
                {
                    DATA = @"{
                     ""axreject"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""recordid"": """ + _amdData[6] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""rejected"",
                     ""isamendment"":""true"",
		             ""statustext"" : """ + comments + @""",
		             ""statusreason"" : """ + reasons + @""",
		             ""userdata"": {}
	                 },
                    """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";
                }
                else
                {
                    DATA = @"{
                     ""axreject"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""rejected"",
                     ""ispeg"":""true"",
		             ""statustext"" : """ + comments + @""",
		             ""statusreason"" : """ + reasons + @""",
		             ""userdata"": {}
	                 },
                    """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";
                }
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    return response;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                    return response;
                }
            }
            catch (Exception ex) { }
            return response;
        }

        [WebMethod(EnableSession = true)]
        public string PegWithdrawWS(string jsonData, string comments, string reasons, string isAmendment)
        {
            string response = string.Empty;
            try
            {
                if (HttpContext.Current.Session["project"] == null)
                    return utilObj.SESSTIMEOUT;
                string DATA = string.Empty;
                string URL = GetArmScriptURL() + "ASBPegRest.dll/datasnap/rest/TASBPeg/AxWithdraw";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                string pwd = GetUserPasswordForPEG();
                string[] _amdData = jsonData.Split('♦');
                string globalvars = xmltojson(Session["axGlobalVars"].ToString());
                string uservars = xmltojson(Session["axUserVars"].ToString());
                string axprops = HttpContext.Current.Application["axProps"].ToString();
                axprops = xmltojson(axprops);
                string axapps = xmltojson(Session["AxApps"].ToString());
                string axappsProj = HttpContext.Current.Session["project"].ToString();
                if (isAmendment == "true")
                {
                    DATA = @"{
                     ""axwithdraw"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""recordid"": """ + _amdData[6] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""withdrawn"",
                     ""isamendment"":""true"",
		             ""statustext"" : """ + comments + @""",
		             ""statusreason"" : """ + reasons + @""",
		             ""userdata"": {}
	                 },
                    """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";
                }
                else
                {
                    DATA = @"{
                     ""axwithdraw"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""withdrawn"",
                     ""ispeg"":""true"",
		             ""statustext"" : """ + comments + @""",
		             ""statusreason"" : """ + reasons + @""",
		             ""userdata"": {}
	                 },
                    """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";
                }
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    return response;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                    return response;
                }
            }
            catch (Exception ex) { }
            return response;
        }

        [WebMethod(EnableSession = true)]
        public string PegCheckWS(string jsonData, string comments, string reasons)
        {
            string response = string.Empty;
            try
            {
                if (HttpContext.Current.Session["project"] == null)
                    return utilObj.SESSTIMEOUT;
                string DATA = string.Empty;
                string URL = GetArmScriptURL() + "ASBPegRest.dll/datasnap/rest/TASBPeg/AxCheck";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                string pwd = GetUserPasswordForPEG();
                string[] _amdData = jsonData.Split('♦');
                string globalvars = xmltojson(Session["axGlobalVars"].ToString());
                string uservars = xmltojson(Session["axUserVars"].ToString());
                string axprops = HttpContext.Current.Application["axProps"].ToString();
                axprops = xmltojson(axprops);
                string axapps = xmltojson(Session["AxApps"].ToString());
                string axappsProj = HttpContext.Current.Session["project"].ToString();
                DATA = @"{
                     ""axcheck"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""checked"",
                     ""ispeg"":""true"",
		             ""statustext"" : """ + comments + @""",
		              ""statusreason"" : """ + reasons + @""",
		             ""userdata"": {}
	                 },
                     """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";

                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    return response;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                    return response;
                }
            }
            catch (Exception ex) { }
            return response;
        }

        [WebMethod(EnableSession = true)]
        public string PegReCallWS(string jsonData, string comments, string reasons, string isAmendment)
        {
            string response = string.Empty;
            try
            {
                if (HttpContext.Current.Session["project"] == null)
                    return utilObj.SESSTIMEOUT;
                string DATA = string.Empty;
                string URL = GetArmScriptURL() + "ASBPegRest.dll/datasnap/rest/TASBPeg/AxRecall";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                string pwd = GetUserPasswordForPEG();
                string[] _amdData = jsonData.Split('♦');
                string globalvars = xmltojson(Session["axGlobalVars"].ToString());
                string uservars = xmltojson(Session["axUserVars"].ToString());
                string axprops = HttpContext.Current.Application["axProps"].ToString();
                axprops = xmltojson(axprops);
                string axapps = xmltojson(Session["AxApps"].ToString());
                string axappsProj = HttpContext.Current.Session["project"].ToString();
                if (isAmendment == "true")
                {
                    DATA = @"{
                     ""axrecall"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""recordid"": """ + _amdData[6] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""recalled"",
                     ""isamendment"":""true"",
		             ""statustext"" : """ + comments + @""",
		             ""statusreason"" : """ + reasons + @""",
		             ""userdata"": {}
	                 },
                    """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";
                }
                else
                {
                    DATA = @"{
                     ""axrecall"": {
                     ""axpapp"":""" + HttpContext.Current.Session["project"].ToString() + @""", 
		             ""username"": """ + HttpContext.Current.Session["username"].ToString() + @""",
		             ""password"": """ + pwd + @""",
		             ""seed"": """",
		             ""trace"": """ + HttpContext.Current.Session["AxTrace"] + @""",
		             ""transid"": """ + _amdData[5] + @""",
		             ""taskid"": """ + _amdData[3] + @""",
		             ""taskname"": """ + _amdData[4] + @""",
		             ""processname"": """ + _amdData[2] + @""",
		             ""keyfield"": """ + _amdData[0] + @""",
		             ""keyvalue"":""" + _amdData[1] + @""",
		             ""status"": ""recalled"",
                     ""ispeg"":""true"",
		             ""statustext"" : """ + comments + @""",
		             ""statusreason"" : """ + reasons + @""",
		             ""userdata"": {}
	                 },
                     """ + axappsProj + @""":{" + axapps + @"},
                     ""axprops"":{ " + axprops + @"},
	                 ""globalvars"": {" + globalvars + @"},
                     ""uservars"": {" + uservars + @"}
                     }";
                }
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    return response;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                    return response;
                }
            }
            catch (Exception ex) { }
            return response;
        }


        [WebMethod(EnableSession = true)]
        public string GetAutoIntelliFromAPI(string tstDataId, ArrayList ChangedFields, ArrayList ChangedFieldDbRowNo, ArrayList ChangedFieldValues, ArrayList DeletedDCRows, string fldname, string searchText, string sqlCols, string resTstHtmlLS)
        {
            string result = string.Empty;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            try
            {
                if (resTstHtmlLS != "")
                {
                    utilObj.DeleteTstIvObject(tstDataId.Split('_')[0]);
                    tstDataId = utilObj.GetReGenTstDataObj(tstDataId, resTstHtmlLS);
                    _tstKeyChanged = tstDataId;
                }
                TStructData tstData = (TStructData)Session[tstDataId];
                if (tstData == null)
                    return Constants.DUPLICATESESS;
                TStructDef tstStrObj = tstData.tstStrObj;
                int fldidx = tstStrObj.GetFieldIndex(fldname);
                TStructDef.FieldStruct fld = (TStructDef.FieldStruct)tstStrObj.flds[fldidx];
                string _transId = tstData.transid;
                tstData.GetFieldValueXml(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, "-1", "AutoComplete", "NG", tstData.tstStrObj.GetFieldDc(fldname).ToString());

                //string api = fld.fieldSelAPI;
                //string apiParams = fld.fieldSelAPIParams;
                //string[] strDetails = apiParams.Split('♠');
                //string apiType = strDetails[1];
                //string contType = strDetails[2];

                string api = string.Empty;
                string mapname = string.Empty;
                string apiType = string.Empty;
                string contType = string.Empty;
                string contBody = string.Empty;

                string _apiname = fld.fieldSelAPIName;
                if (_apiname != null && _apiname != string.Empty)
                {
                    try
                    {
                        string _apiData = GetFromAPIInfo(_apiname, fldname, tstStrObj.transId);
                        JArray returnArray = new JArray();
                        JObject resultRows = JObject.Parse(_apiData);
                        if (_apiData != string.Empty)
                        {
                            JArray _apiEle = (JArray)resultRows["result"]["row"];
                            foreach (JObject row in _apiEle)
                            {
                                JObject modifiedRow = new JObject();
                                foreach (var property in row.Properties())
                                {
                                    if (property.Name.ToLower() == "execapiurl")
                                        api = property.Value.ToString();
                                    if (property.Name.ToLower() == "apiresponsetype")
                                        apiType = property.Value.ToString();
                                    if (property.Name.ToLower() == "apiresponseformat")
                                        contType = property.Value.ToString();
                                    if (property.Name.ToLower() == "execapirequeststring")
                                        contBody = property.Value.ToString();
                                }
                            }
                        }

                        string apiParams = fld.fieldSelAPIParams;
                        string[] strDetails = apiParams.Split('♠');
                        mapname = strDetails[0];
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    api = fld.fieldSelAPI;
                    string apiParams = fld.fieldSelAPIParams;
                    string[] strDetails = apiParams.Split('♠');
                    mapname = strDetails[0];
                    apiType = strDetails[1];
                    contType = strDetails[2];
                    contBody = strDetails[5];
                }
                if (api == string.Empty)
                {
                    return "";
                }

                string strParamVal = string.Empty;
                if (apiType == "Axpert")
                {
                    string DATA = string.Empty;
                    DATA = contBody;// strDetails[5];
                    string _RestDllPath = GetArmScriptURL();
                    if ((tstData.transid == "ad_fp" || tstData.transid == "a__fn" || tstData.transid == "ad_pn") && _RestDllPath != null)
                    {
                        try
                        {
                            string _restPath = _RestDllPath;
                            api = _restPath + "ASBMenuRest.dll/datasnap/rest/TASBMenuRest/GetSqldata";

                            DATA = DATA.Replace("\n", "");
                            JObject obj = JObject.Parse(DATA);
                            string _axapps = obj["_parameters"][0]["getsqldata"]["axpapp"].ToString();
                            DATA = DATA.Replace("\"axpapp\":\"" + _axapps + "\"", "\"axpapp\":\"" + Session["project"].ToString() + "\"");
                            string ddd = xmltojson(Session["axApps"].ToString());
                            string _thisAppCon = obj["_parameters"][0][_axapps].ToString().Replace("\r\n", "");
                            DATA = DATA.Replace(_thisAppCon, "{" + ddd + "}");
                            DATA = DATA.Replace("\"" + _axapps + "\":", "\"" + Session["project"].ToString() + "\":");
                        }
                        catch (Exception ex)
                        {
                            api = fld.fieldSelAPI;
                            DATA = contBody;// strDetails[5];
                        }
                    }
                    DATA = ParseAxpertApiParams(DATA, "sqlparams", tstData.fieldValueXml, tstStrObj, true);
                    if (DATA != string.Empty && DATA.Split('♠').Length > 1)
                        strParamVal = DATA.Split('♠')[1];
                    DATA = DATA.Split('♠')[0];

                    if (sqlCols != "" && sqlCols == "t")
                    {
                        DATA = DATA.Replace("\"trace\":", "\"sqlcols\":\"t\",\"trace\":");
                    }

                    FDW fdwObj = new FDW();
                    FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                    string RsKey = string.Empty;
                    if (sqlCols != "" && sqlCols == "t")
                        RsKey = fdrObj.MakeKeyName(Constants.REDISAXAUTOINTELLI, _transId) + "-" + fldname + "-sqlcols";
                    else
                        RsKey = fdrObj.MakeKeyName(Constants.REDISAXAUTOINTELLI, _transId) + "-" + fldname + "-" + strParamVal;
                    result = fdrObj.ReadKey(RsKey);
                    if (result == string.Empty)
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);

                        request.Method = "POST";
                        request.ContentType = "application/" + contType;
                        request.ContentLength = DATA.Length;
                        StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                        requestWriter.Write(DATA);
                        requestWriter.Close();
                        WebResponse webResponse = request.GetResponse();
                        Stream webStream = webResponse.GetResponseStream();
                        StreamReader responseReader = new StreamReader(webStream);
                        result = responseReader.ReadToEnd();
                        Console.Out.WriteLine(result);
                        responseReader.Close();
                        if (result != string.Empty)
                        {
                            var obj = JObject.Parse(result);
                            result = obj["result"][0]["result"].ToString();
                            if (apiType == "Axpert" && result != string.Empty && result != "{\r\n  \"row\": []\r\n}")
                            {
                                try
                                {
                                    int cHour = DateTime.Now.Hour;
                                    int expiryTime = (24 - cHour) * 60;
                                    fdwObj.WriteKeyAutoExpire(RsKey, result, expiryTime);
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                }
                else
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
                    request.Method = "GET";
                    request.ContentType = "application/" + contType;
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    result = responseReader.ReadToEnd();
                    Console.Out.WriteLine(result);
                    responseReader.Close();
                    if (result != string.Empty)
                    {
                        var obj = JObject.Parse(result);
                        result = obj["result"][0]["result"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Get auto intelligence field value from API.-" + ex.Message, sessionid, "GetAutoIntelliFromAPI-" + fldname, "new");
            }
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string GetCachedSaveNotificationJSON(string transid, string axmsgid)
        {
            string result = string.Empty;
            try
            {
                if (HttpContext.Current.Session["nsessionid"] != null)
                {
                    string sessionId = HttpContext.Current.Session["nsessionid"].ToString();

                    //string sqlQuery = "select displaycontent from axactivemessages a where taskid=:activemessageid and transid=:transid order by eventdatetime desc";
                    string sqlQuery = "select requestpayload from axactivemessages a where taskid=:activemessageid and transid=:transid order by eventdatetime desc";
                    sqlQuery = sqlQuery.Replace(":activemessageid", "'" + axmsgid + "'");
                    sqlQuery = sqlQuery.Replace(":transid", "'" + transid + "'");
                    string errorLog = logobj.CreateLog("Get Cached Save Notification JSON", sessionId, "GetCachedSaveNotificationJSON-" + transid + "", "new");
                    string inputXML = string.Empty;
                    inputXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + sessionId + "' trace='" + errorLog + "' direct='true' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlQuery + "</sql>";
                    inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                    result = objWebServiceExt.CallGetChoiceWebService(transid, inputXML);
                    if (result != "")
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(result);
                        XmlNodeList xmlNodes = default(XmlNodeList);
                        //xmlNodes = xmlDoc.SelectNodes("//sqlresultset/response/row/requestpayload");
                        xmlNodes = xmlDoc.SelectNodes("//sqlresultset/response/row/*[translate(local-name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='requestpayload']");
                        result = xmlNodes[0].InnerXml;
                        return result;
                    }
                    else
                        return "nodata";
                }
                else
                    result = "error";
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Get Cached Save Notification JSON.-axmsgid:" + axmsgid + " Exception:" + ex.Message, sessionid, "GetCachedSaveNotificationJSON-" + transid, "new");
                result = "error";
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public void SaveAxNotifyHistory(string _AxNotifyHistory)
        {
            try
            {
                if (HttpContext.Current.Session["username"] != null)
                {
                    string userName = HttpContext.Current.Session["username"].ToString();
                    FDW fdwObj = new FDW();
                    fdwObj.WriteKeyAutoExpire(utilObj.GetRedisServerkey(Constants.AXNOTIFYHISTORY, userName), _AxNotifyHistory, 24 * 60);
                }
            }
            catch (Exception ex) { }
        }

        public string GetArmScriptURL()
        {
            string ArmRestDllPath = string.Empty;
            try
            {
                if (Session["ARM_Scripts_URL"] != null && Session["ARM_Scripts_URL"].ToString() != "")
                {
                    ArmRestDllPath = Session["ARM_Scripts_URL"].ToString();
                    if (ArmRestDllPath.Substring(ArmRestDllPath.Length - 1) != "/")
                        ArmRestDllPath += "/";
                }
                else if (ConfigurationManager.AppSettings["RestDllPath"] != null)
                    ArmRestDllPath = ConfigurationManager.AppSettings["RestDllPath"].ToString();
            }
            catch (Exception ex)
            {
            }
            return ArmRestDllPath;
        }

        [WebMethod(EnableSession = true)]
        public string SaveFillgridResize(string transid, string fillgridName, string resizeVal)
        {
            string res = string.Empty;
            try
            {
                DBContext obj = new DBContext();
                res = obj.SaveFillgridResize(transid, fillgridName, resizeVal);
            }
            catch (Exception ex)
            {
                res = "Error while saving.";
            }
            return res;
        }

        //CustomWebService methods
        [WebMethod(EnableSession = true)]
        public string CustomFunction()
        {
            string result = string.Empty;
            result = Session["project"].ToString();
            return result;
        }
        [WebMethod(EnableSession = true)]
        public string SetIvparams(string param)
        {
            string result = "done";
            Session["iviewcustomparams"] = param;
            return result;
        }
        [WebMethod(EnableSession = true)]
        public string GetChoicesCustomWS(string transid, string sqlQuery)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string errorLog = logobj.CreateLog("Call GetChoices", Session["nsessionid"].ToString(), "CallGetChoices-" + transid + "", "new");
            string inputXML = string.Empty;
            inputXML = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>" + sqlQuery + "</sql>";
            inputXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            string result = asbExt.CallGetChoiceWS(transid, inputXML);
            return result;
        }



        [WebMethod(EnableSession = true)]
        public string CreateFastReportPDFCustomWS(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string s, string key, string resTstHtmlLS)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            tstData.GetFieldValueXml(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, "-1", "false", "ALL", "");
            s += "<varlist><row>" + tstData.fldValueXml + tstData.memVarsData + "</row></varlist>";
            s += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            string result = string.Empty;
            result = tstData.CallCreateFastPDFWS(s);
            if (_tstKeyChanged != "")
                result = _tstKeyChanged + "♠*♠" + result;
            return result;
        }


        [WebMethod()]
        public string HelloWorld()
        {
            string result = string.Empty;

            return "Hi Hello";
        }


        public string CheckDMSIntegration(string structid)
        {
            string dmsSql = string.Empty;
            string res = string.Empty;
            string errorLog = string.Empty;

            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            /*  dmsSql = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "'><sql>select  url,add_view,dc, fieldnames from tomni  where dmsid = '" + structid + "' </sql>";
              dmsSql += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + "</sqlresultset>";
              res = objWebServiceExt.CallGetChoiceWS("", dmsSql);
              if (res.Contains(Constants.ERROR) == true)
              {
                  res = res.Replace(Constants.ERROR, "");
                  res = res.Replace("</error>", "");
                  res = res.Replace("\n", "");
                  throw (new Exception(res));
              }*/
            return res;


        }

        [WebMethod(EnableSession = true)]
        public string SendMail(string toAddress, string msgBody, string subject, string userMail)
        {

            string status = "Failure";
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                NetworkCredential credential = (NetworkCredential)smtp.Credentials;

                string from = credential.UserName;
                string host = smtp.Host;
                int port = smtp.Port;
                bool enableSsl = smtp.EnableSsl;
                string user = credential.UserName;
                string password = credential.Password;

                message.From = new MailAddress(from);
                message.To.Add(new MailAddress(toAddress));
                message.CC.Add(from + "," + userMail);
                message.Subject = subject;
                message.IsBodyHtml = false; //to make message body as html  
                message.Body = msgBody;
                smtp.Port = port;
                smtp.Host = host; //for gmail host  
                smtp.EnableSsl = enableSsl;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(user, password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                status = "Success";
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("Send Mail -\n\tError - " + ex.Message, HttpContext.Current.Session.SessionID, "Send Mail", "");
            }
            return status;
        }

        [WebMethod(EnableSession = true)]
        public string LoadExportFileToScript(string filePath, string fileName)
        {
            string copied = string.Empty;
            try
            {
                string PrefixFilename = string.Empty;
                string sessionid = HttpContext.Current.Session["nsessionid"].ToString();
                string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                string scrPath = scriptsPath + "axpert\\" + sessionid + "\\Exports\\";

                if (File.Exists(scrPath + fileName))
                {
                    File.Delete(scrPath + fileName);
                }
                CopyFiles(filePath, scrPath, fileName, fileName);
                if (File.Exists(scrPath + fileName))
                {
                    int lastUnderscoreIndex = fileName.LastIndexOf('_');
                    if (lastUnderscoreIndex != -1)
                    {
                        int lastPeriodIndex = fileName.LastIndexOf('.');
                        string namePart = fileName.Substring(0, lastUnderscoreIndex);
                        string extensionPart = fileName.Substring(lastPeriodIndex);
                        string _reNameFile = namePart + extensionPart;
                        File.Delete(scrPath + _reNameFile);
                        File.Move(scrPath + fileName, scrPath + _reNameFile);
                        copied = utilObj.ScriptsurlPath + "axpert/" + sessionid + "/Exports/" + _reNameFile;
                    }
                    else
                        copied = utilObj.ScriptsurlPath + "axpert/" + sessionid + "/Exports/" + fileName;
                }
                else
                    copied = utilObj.ScriptsurlPath + "axpert/" + sessionid + "/Exports/" + fileName;
            }
            catch (Exception) { }
            return copied;
        }

        [WebMethod(EnableSession = true)]
        public string GenerateSignalRNotification(string messageType, string message)
        {
            string response = string.Empty;
            if (HttpContext.Current.Session["project"] == null || Session["nsessionid"] == null)
                return utilObj.SESSTIMEOUT;
            string DATA = string.Empty;
            string URL = String.Empty;
            if (HttpContext.Current.Session["ARM_Notification_URL"] != null)
                URL = HttpContext.Current.Session["ARM_Notification_URL"].ToString() + "/api/v1/SendSignalR";

            if (URL == string.Empty)
                return "error:ARM Notification URL not defined";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            var saveDetails = "{\"project\":\"" + HttpContext.Current.Session["project"] + "\",\"UserId\":\"" + HttpContext.Current.Session["user"].ToString() + "\",\"Message\":\"[{\\\"notifytype\\\":\\\"" + messageType + "\\\",\\\"type\\\":\\\"" + messageType + "\\\",\\\"icon\\\":\\\"\\\",\\\"title\\\":\\\"" + messageType + "\\\",\\\"message\\\":\\\"" + message + "\\\",\\\"dt\\\":\\\"" + DateTime.Now.ToString() + "\\\",\\\"link\\\":{\\\"t\\\":\\\"e\\\",\\\"name\\\":\\\"\\\",\\\"p\\\":\\\"\\\",\\\"act\\\":\\\"\\\",\\\"axmsgid\\\":\\\"\\\"}}]\"}";
            request.ContentLength = saveDetails.Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(saveDetails);
            requestWriter.Close();

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                Console.Out.WriteLine(response);
                responseReader.Close();
                return response;
            }
            catch (Exception e)
            {
                return "error:" + e.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetExcelImportValues(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, string frameNo, string s, string key, string resTstHtmlLS, string isDataExist)
        {
            string requestProcess_logtime = string.Empty;
            ObjExecTr.SetCurrentTime();
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;

            if (Util.Util.CheckCrossScriptingInString(String.Join(",", fldValueArray.ToArray())))
            {
                return Constants.MALICIOUSNPUTDETECTED;
            }

            GetGlobalVars();

            string dcHTML = string.Empty;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;
            transid = tstData.transid;

            string fileName = "GetExcelImportValues-" + transid;
            string errorLog = logobj.CreateLog("GetExcelImportValues.", sessionid, fileName, "new");
            string Ixml = "";
            Ixml = "<sqlresultset axpapp='" + project + "' sessionid='" + sessionid + "' appsessionkey='" + Session["AppSessionKey"].ToString() + "' username='" + Session["username"].ToString() + "' trace='" + errorLog + "' frameno='" + frameNo + "' transid='" + transid + "'>";
            Ixml += s;

            TStructDef strObj = tstData.tstStrObj;
            string result = string.Empty;
            DateTime eTime1 = DateTime.Now;
            result = tstData.CallGetExcelImportValuesWS(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, frameNo, Ixml, isDataExist);
            requestProcess_logtime += result.Split('♠')[0];
            result = result.Split('♠')[1];
            DateTime sTime2 = DateTime.Now;
            if (result != "")
                tstData.JsonToArray(result, strObj, frameNo, "FillGridMS", "ExcelImport");

            strObj.IsFillGridCall = true;
            dcHTML = tstData.tabJson.ToString() + "*♠*" + strObj.GetTabDcHTML(Convert.ToInt32(frameNo), tstData, "false");
            strObj.IsFillGridCall = false;
            DateTime eTime2 = DateTime.Now;
            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));

            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response", dcHTML);
            string serverprocesstime = ObjExecTr.TotalServerElapsTime();
            dcHTML = serverprocesstime + "*♠♠*" + requestProcess_logtime + "*♠♠*" + dcHTML;
            if (_tstKeyChanged != "")
                dcHTML = _tstKeyChanged + "♠*♠" + dcHTML;
            return dcHTML;
        }

        [WebMethod(EnableSession = true)]
        public string UserActivation(string _Username, string _UserType, string _IsActive)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string res = string.Empty;
            try
            {
                _IsActive = "T";
                string schemaName = HttpContext.Current.Session["dbuserALC"].ToString();
                //LogFile.Log _logObj = new LogFile.Log();
                //_logObj.CreateLog("User Activate details:_Username:" + _Username + " _UserType:" + _UserType + " _IsActive:" + _IsActive + " schemaName:" + schemaName, HttpContext.Current.Session.SessionID, "UserActivate", "");
                res = ALCClient.CallALCClient("Activate", _Username, _UserType, _IsActive, schemaName);
                if (!res.StartsWith("Error:") && !res.Contains("The operation has timed out"))
                {
                    var outerObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
                    string authKey = outerObject["AuthKey"];
                    string userKey = outerObject["UserKey"];
                    DBContext obj = new DBContext();
                    string _res = obj.UpdateALCUserInfo(_Username, _IsActive, authKey, userKey);
                    if (_res == "0" || _res.StartsWith("Error:"))
                        res = _res;
                    else
                        res = "User activated successfully";
                }
                else
                {
                    LogFile.Log logObj = new LogFile.Log();
                    logObj.CreateLog("User Activation -Error - " + res, HttpContext.Current.Session.SessionID, "UserActivation", "");
                    if (res.Contains("{\"Message\":"))
                    {
                        res = res.Replace("Error: 400 - Bad Request,  Error response body: ", "");
                        JObject jsonObject = JObject.Parse(res);
                        res = jsonObject["Message"].ToString();
                    }
                    else
                        res = "Error: Error while activating user, please try again";
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("User Activation -Error:" + res + " in try - " + ex.Message, HttpContext.Current.Session.SessionID, "UserActivation", "");
                res = "Error: Error while activating user, please try again";
            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string UserDeactivate(string _Username, string _UserType, string _IsActive)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string res = string.Empty;
            try
            {
                if (_IsActive.ToLower() == "t")
                {
                    _IsActive = "F";
                    string schemaName = HttpContext.Current.Session["dbuserALC"].ToString();

                    //LogFile.Log _logObj = new LogFile.Log();
                    //_logObj.CreateLog("User Deactivate details:_Username:" + _Username + " _UserType:" + _UserType + " _IsActive:" + _IsActive + " schemaName:" + schemaName, HttpContext.Current.Session.SessionID, "UserDeactivate", "");

                    res = ALCClient.CallALCClient("Deactivate", _Username, _UserType, _IsActive, schemaName);
                    if (!res.StartsWith("Error:") && !res.Contains("The operation has timed out"))
                    {
                        var outerObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
                        string authKey = "";
                        string userKey = "";
                        DBContext obj = new DBContext();
                        string _res = obj.UpdateALCUserInfo(_Username, _IsActive, authKey, userKey);
                        if (_res == "0" || _res.StartsWith("Error:"))
                            res = _res;
                        else
                            res = "User deactivated successfully";
                    }
                    else
                    {
                        LogFile.Log logObj = new LogFile.Log();
                        logObj.CreateLog("User Deactivate -Error - " + res, HttpContext.Current.Session.SessionID, "UserDeactivate", "");
                        if (res.Contains("{\"Message\":"))
                        {
                            res = res.Replace("Error: 400 - Bad Request,  Error response body: ", "");
                            JObject jsonObject = JObject.Parse(res);
                            res = jsonObject["Message"].ToString();
                        }
                        else
                            res = "Error: Error while deactivating user, please try again";
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("User Deactivate -Error:" + res + " in try - " + ex.Message, HttpContext.Current.Session.SessionID, "UserDeactivate", "");
                res = "Error: Error while deactivating user, please try again";
            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string UserUpdate(string _Username, string _UserType, string _IsActive, string _prevUserType, string _isValidityOver)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string res = string.Empty;
            try
            {
                string schemaName = HttpContext.Current.Session["dbuserALC"].ToString();
                //LogFile.Log _logObj = new LogFile.Log();
                //_logObj.CreateLog("User Update details:_Username:" + _Username + " _UserType:" + _UserType + " _IsActive:" + _IsActive + " _prevUserType:" + _prevUserType + " _isValidityOver:" + _isValidityOver + " schemaName:" + schemaName, HttpContext.Current.Session.SessionID, "UserUpdate", "");
                res = ALCClient.CallALCClientUpdate("Update", _Username, _UserType, _IsActive, _prevUserType, _isValidityOver, schemaName);
                if (!res.StartsWith("Error:") && !res.Contains("The operation has timed out"))
                {
                    var outerObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
                    string authKey = outerObject["AuthKey"];
                    string userKey = outerObject["UserKey"];
                    DBContext obj = new DBContext();
                    string _res = obj.UpdateALCUserInfo(_Username, _IsActive, authKey, userKey);
                    if (_res == "0" || _res.StartsWith("Error:"))
                        res = _res;
                    else
                        res = "User updated successfully";
                }
                else
                {
                    LogFile.Log logObj = new LogFile.Log();
                    logObj.CreateLog("User Update -Error - " + res, HttpContext.Current.Session.SessionID, "UserUpdate", "");
                    if (res.Contains("{\"Message\":"))
                    {
                        res = res.Replace("Error: 400 - Bad Request,  Error response body: ", "");
                        JObject jsonObject = JObject.Parse(res);
                        res = jsonObject["Message"].ToString();
                    }
                    else
                        res = "Error: Error while updating user, please try again";
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("User Update -Error:" + res + " in try - " + ex.Message, HttpContext.Current.Session.SessionID, "UserUpdate", "");
                res = "Error: Error while updating user, please try again";
            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string GetUserKeyInfo(string _Username, string _userKey, string _authkey)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string res = string.Empty;
            try
            {
                res = ALCClient.CallALCClientInfo(_Username, _userKey, _authkey);
                if (res.StartsWith("error:"))
                {
                    LogFile.Log logObj = new LogFile.Log();
                    logObj.CreateLog("User Update -Error - " + res, HttpContext.Current.Session.SessionID, "GetUserKeyInfo", "");
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("User Update -Error:" + res + " in try - " + ex.Message, HttpContext.Current.Session.SessionID, "GetUserKeyInfo", "");
            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public ArrayList ListTemplates(string fileType)
        {
            try
            {
                string templateRepository = HttpRuntime.AppDomainAppPath + "AxpTemplates\\";
                DirectoryInfo di = new DirectoryInfo(templateRepository);
                FileInfo[] templateFiles = di.GetFiles("*.html");
                ArrayList allFiles = new ArrayList();
                for (int i = 0; i < templateFiles.Length; i++)
                {
                    FileInfo objFile = (FileInfo)templateFiles[i];
                    allFiles.Add(objFile.Name);
                }
                return allFiles;
            }
            catch (Exception ex)
            { }
            return null;
        }

        [WebMethod(EnableSession = true)]
        public string tstAddFieldReactWS(string _fieldXML)
        {
            string results = string.Empty;
            try
            {
                string ttransid = string.Empty;
                var emptyJSON = "{\"name\":\"\",\"oldName\":\"\",\"caption\":\"\",\"oldCaption\":\"\",\"fieldType\":\"\",\"customDataType\":\"NA\",\"dataType\":\"\",\"dclabel\":\"\",\"dcName\":\"\",\"dcNo\":\"\",\"dcCaption\":\"\",\"width\":1,\"saveValue\":\"true\",\"decimal\":0,\"dbDecimal\":0,\"MOE\":\"Accept\",\"acceptChoice\":\"sql\",\"defSrcField\":\"\",\"defSrcFieldDDN\":\"\",\"defTable\":\"\",\"expression\":\"\",\"source\":\"\",\"defList\":\"\",\"DSForm\":\"\",\"DSFormCaption\":\"\",\"DSFormDDN\":\"\",\"DSField\":\"\",\"DSFieldCaption\":\"\",\"DSFieldDDN\":\"\",\"DSTable\":\"\",\"SqlText\":\"\",\"srcField\":\"\",\"srcTable\":\"\",\"srcForm\":\"\",\"srcFormDDN\":\"\",\"dataSource\":\"\",\"dataSourceDDN\":\"\",\"masterForm\":\"\",\"masterFormCaption\":\"\",\"masterFormDDN\":\"\",\"masterField\":\"\",\"masterFieldCaption\":\"\",\"masterFieldDDN\":\"\",\"fillMaster\":\"\",\"fillMasterCaption\":\"\",\"fillMasterDDN\":\"\",\"masterSqlText\":\"\",\"fillSource\":\"\",\"fillSourceDDN\":\"\",\"suggestive\":\"false\",\"refreshOnSave\":\"false\",\"saveNomralized\":\"false\",\"autoSelect\":\"true\",\"comboBox\":\"false\",\"apiNameSql\":\"\",\"apiNameSqlDDN\":\"\",\"apiNameMO\":\"\",\"apiNameMODDN\":\"\",\"apiURL\":\"\",\"apiReqStr\":\"\",\"apiRespType\":\"\",\"apiRespFormat\":\"\",\"mapFieldApi\":\"\",\"fillFrmApiSrc\":\"\",\"fillFrmApiSrcCap\":\"\",\"fillFrmApiSrcDDN\":\"\",\"intelligence\":\"false\",\"defSeqTable\":[],\"sequence\":\"\",\"validateExp\":\"\",\"allowEmpty\":\"true\",\"allowDuplicate\":\"true\",\"onlyPositive\":\"false\",\"clientValidation\":\"false\",\"hidden\":\"false\",\"readOnly\":\"false\",\"applyComma\":\"false\",\"setCarry\":\"false\",\"tabstop\":\"true\",\"fieldDataEncrypt\":\"false\",\"hideInSearch\":\"false\",\"PWAlphaNum\":\"false\",\"masking\":\"No Mask\",\"maskChar\":\"X\",\"maskRoles\":\"\",\"lastCharMask\":0,\"firstCharMask\":0,\"pattern\":\"\",\"displayDetails\":\"\",\"separator\":\"\",\"pwdChar\":\"\",\"pwdLength\":0,\"pwdExpiry\":0,\"radiotype\":\"\",\"applyRule\":\"false\",\"encrypted\":\"false\",\"runtimeFld\":\"false\",\"runtimecriteria\":\"false\",\"purpose\":\"\",\"ptableid\":\"\",\"firstcolvalue\":\"\",\"tablename\":\"\"}";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(_fieldXML);
                XmlNode stIdNode = xmlDoc.SelectSingleNode("//root/varlist/row/isparam");
                if (stIdNode != null && stIdNode.InnerText == "T")
                {
                    try
                    {
                        JObject jsonObject = JObject.Parse(emptyJSON);
                        XmlNodeList rowNodes = xmlDoc.SelectNodes("//root/varlist/row");
                        if (rowNodes != null)
                        {
                            foreach (XmlNode rowNode in rowNodes)
                            {
                                foreach (XmlNode childNode in rowNode.ChildNodes)
                                {
                                    string fieldName = childNode.Name;
                                    string fieldValue = childNode.InnerText;
                                    if (fieldName == "ttransid")
                                        ttransid = fieldValue;
                                    if (fieldName == "vpname")
                                        jsonObject["name"] = fieldValue;
                                    if (fieldName == "oldpcaption")
                                        jsonObject["oldCaption"] = fieldValue;
                                    if (fieldName == "pcaption")
                                        jsonObject["caption"] = fieldValue;
                                    if (fieldName == "pdatatype")
                                        jsonObject["dataType"] = fieldValue;
                                    if (fieldName == "modeofentry")
                                        jsonObject["MOE"] = fieldValue;
                                    if (fieldName == "modeofentry")
                                    {
                                        jsonObject["MOE"] = fieldValue;
                                        if (fieldValue.ToLower() == "select")
                                            jsonObject["source"] = "select from sql";
                                    }
                                    if (fieldName == "customdatatype")
                                        jsonObject["fieldType"] = fieldValue;
                                    if (fieldName == "display")
                                        jsonObject["hidden"] = fieldValue == "F" ? "false" : "true";
                                    if (fieldName == "readonly")
                                        jsonObject["readOnly"] = fieldValue == "F" ? "false" : "true";
                                    if (fieldName == "datawidth")
                                        jsonObject["width"] = fieldValue;
                                    if (fieldName == "SQL_editor_psql")
                                    {
                                        string _SqlText = fieldValue;
                                        if (_SqlText != string.Empty)
                                        {
                                            _SqlText = Regex.Replace(_SqlText, @"(\s+|<br\s*/?>)", " ", RegexOptions.IgnoreCase);
                                        }
                                        jsonObject["SqlText"] = _SqlText;
                                    }
                                    if (fieldName == "dcselect")
                                    {
                                        jsonObject["dclabel"] = fieldValue;
                                        string _DcName = fieldValue.Split('(')[1].Split(')')[0];
                                        string _dcNumber = _DcName.Replace("dc", "");
                                        jsonObject["dcName"] = _DcName;
                                        jsonObject["dcNo"] = _dcNumber;
                                        jsonObject["dcCaption"] = _DcName;
                                    }

                                    if (fieldName == "masterdl")
                                        jsonObject["fillMaster"] = fieldValue;
                                    if (fieldName == "v_sql")
                                    {
                                        string _v_sqlText = fieldValue;
                                        if (_v_sqlText != string.Empty)
                                        {
                                            _v_sqlText = Regex.Replace(_v_sqlText, @"(\s+|<br\s*/?>)", " ", RegexOptions.IgnoreCase);
                                        }
                                        jsonObject["masterSqlText"] = _v_sqlText;
                                    }
                                    if (fieldName == "source")
                                        jsonObject["fillSource"] = fieldValue;
                                }
                            }
                        }
                        results = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented).Replace("\r\n ", "").Replace("\r\n", "");
                        string globalvars = Session["axGlobalVars"].ToString();
                        string uservars = Session["axUserVars"].ToString();
                        string axapps = Session["axApps"].ToString();
                        string axprops = HttpContext.Current.Application["axProps"].ToString();
                        globalvars = globalvars.Replace(@"\", "\\\\");
                        string errorLog = string.Empty;
                        errorLog = logobj.CreateLog("Calling addTstfieldtoXml:", sessionid, "addTstfieldtoXml-" + transid, "new");
                        bool isTrace = false;
                        if (errorLog != "")
                            isTrace = true;
                        var _jsonObject = new
                        {
                            userName = HttpContext.Current.Session["user"].ToString(),
                            projName = HttpContext.Current.Session["project"].ToString(),
                            trace = isTrace,
                            structName = ttransid,
                            sessionid = HttpContext.Current.Session["nsessionid"].ToString(),
                            axApps = axapps,
                            axProps = axprops,
                            axGlobalvars = globalvars,
                            axUservars = uservars,
                            fldPropsJSON = results
                        };

                        string inputJson = JsonConvert.SerializeObject(_jsonObject, Newtonsoft.Json.Formatting.None);

                        string axpertstudioReact = string.Empty;
                        if (Session["axpertstudioReact"] != null)
                            axpertstudioReact = Session["axpertstudioReact"].ToString();

                        string api = axpertstudioReact + "General/addTstfieldtoXml";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
                        request.Method = "POST";
                        request.ContentType = "application/JSON";
                        request.Headers["isruntime"] = "true";
                        request.ContentLength = inputJson.Length;
                        StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                        requestWriter.Write(inputJson);
                        requestWriter.Close();

                        WebResponse webResponse = request.GetResponse();
                        Stream webStream = webResponse.GetResponseStream();
                        StreamReader responseReader = new StreamReader(webStream);
                        results = responseReader.ReadToEnd();
                        Console.Out.WriteLine(results);
                        responseReader.Close();
                        utilObj.DeleteCachedDataKeys("tstruct", "axglo");
                    }
                    catch (Exception ex)
                    {
                        LogFile.Log logObj = new LogFile.Log();
                        logObj.CreateLog("Tstruct AddFieldReactWS -Error - " + ex.Message, HttpContext.Current.Session.SessionID, "AddFieldReactWS", "");
                    }
                }
                else
                    results = "";
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("Tstruct AddFieldReactWS -Error - " + ex.Message, HttpContext.Current.Session.SessionID, "AddFieldReactWS2", "");
            }
            return results;
        }

        [WebMethod(EnableSession = true)]
        public string AddFormRuntimeWS(string additionalInfo)
        {
            string res = string.Empty;
            try
            {
                if (additionalInfo != "" && additionalInfo != null)
                {
                    string[] _addInfo = additionalInfo.Split('~');
                    string _defaultJSON = "{\"name\":\"" + _addInfo[1] + "\",\"caption\":\"" + _addInfo[2] + "\",\"schema\":\"\",\"menuaccess\":true,\"pform\":\"yes\",\"saveCtrlFld\":\"\",\"saveCtrlFldCaption\":\"\",\"saveCtrlFldsDDN\":\"\",\"dltCtrlFld\": \"\",\"dltCtrlFldCaption\":\"\",\"dltCtrlFldsDDN\":\"\",\"tracking\":false,\"trackby\":\"\",\"trackusrs\": \"\",\"trackUsrsDDN\":[],\"trackfldOption\":\"\",\"trackFlds\":\"\",\"trackFldCaps\":\"\",\"trackFldsDDN\":[],\"allowconfig\":false,\"enablecache\":false,\"enablewf\":false,\"allowattch\":false,\"enableListVW\":true,\"purpose\":\"\"}";
                    string _runtimeInfo = string.Empty;
                    string _formType = _addInfo[3] == "1" ? "simple" : (_addInfo[3] == "2" ? "header-details" : "header-details-footer");
                    if (Session["isRuntimeFormKeys"] != null && Session["isRuntimeFormKeys"].ToString() != "")
                        _runtimeInfo = "{\"formtype\":\"" + _formType + "\"}";
                    else
                    {
                        string globalvars = Session["axGlobalVars"].ToString();
                        string uservars = Session["axUserVars"].ToString();
                        string axapps = Session["axApps"].ToString();
                        string axprops = HttpContext.Current.Application["axProps"].ToString();
                        globalvars = globalvars.Replace(@"\", "\\\\");
                        _runtimeInfo = "{\"formtype\":\"" + _formType + "\",\"axapps\":\"" + axapps + "\",\"axprops\":\"" + axprops + "\",\"axglobalvars\":\"" + globalvars + "\",\"axuservars\":\"" + uservars + "\"}";
                    }

                    string errorLog = string.Empty;
                    errorLog = logobj.CreateLog("Calling addFormRuntime:", sessionid, "addFormRuntime-" + _addInfo[1], "new");
                    bool isTrace = false;
                    if (errorLog != "")
                        isTrace = true;
                    var _jsonObject = new
                    {
                        mode = "new",
                        projName = HttpContext.Current.Session["project"].ToString(),
                        userName = HttpContext.Current.Session["user"].ToString(),
                        sessionID = HttpContext.Current.Session["nsessionid"].ToString(),
                        trace = isTrace,
                        structName = _addInfo[1],
                        structCaption = _addInfo[2],
                        structType = "tstruct",
                        isComp = false,
                        compJSON = "{}",
                        propvalJSON = _defaultJSON,
                        runtimeInfo = _runtimeInfo

                    };
                    string inputJson = JsonConvert.SerializeObject(_jsonObject, Newtonsoft.Json.Formatting.None);
                    Session["isRuntimeFormKeys"] = "true";

                    string axpertstudioReact = string.Empty;
                    if (Session["axpertstudioReact"] != null)
                        axpertstudioReact = Session["axpertstudioReact"].ToString();

                    string api = axpertstudioReact + "SaveFormDataRuntime";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
                    request.Method = "POST";
                    request.ContentType = "application/JSON";
                    request.Headers["isruntime"] = "true";
                    request.ContentLength = inputJson.Length;
                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                    requestWriter.Write(inputJson);
                    requestWriter.Close();

                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    res = responseReader.ReadToEnd();
                    responseReader.Close();
                }
            }
            catch (Exception ex)
            {
                res = string.Empty;
            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string AddFieldRuntimeWS(string transid, string formCap, string fieldJson, string fldMode, string newform, string formInfo)
        {
            string res = string.Empty;
            try
            {
                if (newform != "" && newform == "T")
                {
                    string _addform = AddFormRuntimeWS(formInfo);
                    JObject obj = JObject.Parse(_addform);
                    if (obj["success"].ToString() == "False")
                    {
                        res = "addform:" + _addform;
                        return res;
                    }
                }
                string _runtimeInfo = string.Empty;
                if (Session["isRuntimeFormKeys"] != null && Session["isRuntimeFormKeys"].ToString() != "")
                    _runtimeInfo = "{}";
                else
                {
                    string globalvars = Session["axGlobalVars"].ToString();
                    string uservars = Session["axUserVars"].ToString();
                    string axapps = Session["axApps"].ToString();
                    string axprops = HttpContext.Current.Application["axProps"].ToString();
                    globalvars = globalvars.Replace(@"\", "\\\\");
                    _runtimeInfo = "{\"axapps\":\"" + axapps + "\",\"axprops\":\"" + axprops + "\",\"axglobalvars\":\"" + globalvars + "\",\"axuservars\":\"" + uservars + "\"}";
                }

                string errorLog = string.Empty;
                errorLog = logobj.CreateLog("Calling AddFieldRuntimeWS:", sessionid, "AddFieldRuntimeWS-" + transid, "new");
                bool isTrace = false;
                if (errorLog != "")
                    isTrace = true;
                var _jsonObject = new
                {
                    mode = fldMode,
                    projName = HttpContext.Current.Session["project"].ToString(),
                    userName = HttpContext.Current.Session["user"].ToString(),
                    sessionID = HttpContext.Current.Session["nsessionid"].ToString(),
                    trace = isTrace,
                    structName = transid,
                    structCaption = formCap,
                    structType = "tstruct",
                    isComp = true,
                    compJSON = "{}",
                    propvalJSON = fieldJson,
                    runtimeInfo = _runtimeInfo

                };
                string inputJson = JsonConvert.SerializeObject(_jsonObject, Newtonsoft.Json.Formatting.None);
                Session["isRuntimeFormKeys"] = "true";

                string axpertstudioReact = string.Empty;
                if (Session["axpertstudioReact"] != null)
                    axpertstudioReact = Session["axpertstudioReact"].ToString();

                string api = axpertstudioReact + "SaveFormDataRuntime";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
                request.Method = "POST";
                request.ContentType = "application/JSON";
                request.Headers["isruntime"] = "true";
                request.ContentLength = inputJson.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(inputJson);
                requestWriter.Close();

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                res = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch (Exception ex)
            {
                res = string.Empty;
            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteFieldRuntimeWS(string transid, string fieldname, string dcname)
        {
            string res = string.Empty;
            try
            {
                string _runtimeInfo = string.Empty;
                if (Session["isRuntimeFormKeys"] != null && Session["isRuntimeFormKeys"].ToString() != "")
                    _runtimeInfo = "{}";
                else
                {
                    string globalvars = Session["axGlobalVars"].ToString();
                    string uservars = Session["axUserVars"].ToString();
                    string axapps = Session["axApps"].ToString();
                    string axprops = HttpContext.Current.Application["axProps"].ToString();
                    globalvars = globalvars.Replace(@"\", "\\\\");
                    _runtimeInfo = "{\"axapps\":\"" + axapps + "\",\"axprops\":\"" + axprops + "\",\"axglobalvars\":\"" + globalvars + "\",\"axuservars\":\"" + uservars + "\"}";
                }

                string errorLog = string.Empty;
                errorLog = logobj.CreateLog("Calling DeleteFieldRuntimeWS:", sessionid, "DeleteFieldRuntimeWS-" + transid, "new");
                bool isTrace = false;
                if (errorLog != "")
                    isTrace = true;
                isTrace = true;
                var _jsonObject = new
                {
                    projName = HttpContext.Current.Session["project"].ToString(),
                    userName = HttpContext.Current.Session["user"].ToString(),
                    sessionID = HttpContext.Current.Session["nsessionid"].ToString(),
                    trace = isTrace,
                    structName = transid,
                    StructType = "tstruct",
                    CompName = fieldname,
                    CompType = "field",
                    ParentName = dcname,
                    runtimeInfo = _runtimeInfo
                };
                string inputJson = JsonConvert.SerializeObject(_jsonObject, Newtonsoft.Json.Formatting.None);
                Session["isRuntimeFormKeys"] = "true";

                string axpertstudioReact = string.Empty;
                if (Session["axpertstudioReact"] != null)
                    axpertstudioReact = Session["axpertstudioReact"].ToString();

                string api = axpertstudioReact + "DeleteDataRunTime";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
                request.Method = "POST";
                request.ContentType = "application/JSON";
                request.Headers["isruntime"] = "true";
                request.ContentLength = inputJson.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(inputJson);
                requestWriter.Close();

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                res = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch (Exception ex)
            {
                res = string.Empty;
            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string tstDeleteFieldReactWS(string _fieldXML)
        {
            string results = string.Empty;
            try
            {
                string ttransid = string.Empty;
                string dFieldName = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(_fieldXML);
                XmlNode stIdNode = xmlDoc.SelectSingleNode("//root/varlist/row/isparam");
                if (stIdNode != null && stIdNode.InnerText == "T")
                {
                    try
                    {
                        XmlNode ttransidNode = xmlDoc.SelectSingleNode("//root/varlist/row/ttransid");
                        if (ttransidNode != null)
                            ttransid = ttransidNode.InnerText;
                        XmlNode vpnameNode = xmlDoc.SelectSingleNode("//root/varlist/row/vpname");
                        if (vpnameNode != null)
                            dFieldName = vpnameNode.InnerText;
                        string globalvars = Session["axGlobalVars"].ToString();
                        string uservars = Session["axUserVars"].ToString();
                        string axapps = Session["axApps"].ToString();
                        string axprops = HttpContext.Current.Application["axProps"].ToString();
                        globalvars = globalvars.Replace(@"\", "\\\\");
                        string errorLog = string.Empty;
                        errorLog = logobj.CreateLog("Calling tstDeleteFieldReactWS:", sessionid, "tstDeleteFieldReactWS-" + ttransid, "new");
                        bool isTrace = !string.IsNullOrEmpty(errorLog);
                        var _jsonObject = new
                        {
                            userName = HttpContext.Current.Session["user"].ToString(),
                            projName = HttpContext.Current.Session["project"].ToString(),
                            trace = isTrace,
                            structName = ttransid,
                            fldName = dFieldName,
                            sessionid = HttpContext.Current.Session["nsessionid"].ToString(),
                            axApps = axapps,
                            axProps = axprops,
                            axGlobalvars = globalvars,
                            axUservars = uservars
                        };
                        var serializerSettings = new JsonSerializerSettings
                        {
                            Formatting = Newtonsoft.Json.Formatting.None,
                            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                            {
                                NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
                            }
                        };
                        string inputJson = JsonConvert.SerializeObject(_jsonObject, serializerSettings);
                        string axpertstudioReact = string.Empty;
                        if (Session["axpertstudioReact"] != null)
                            axpertstudioReact = Session["axpertstudioReact"].ToString();

                        string api = axpertstudioReact + "General/deleteTstfield";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
                        request.Method = "POST";
                        request.ContentType = "application/JSON";
                        request.Headers["isruntime"] = "true";
                        request.ContentLength = inputJson.Length;
                        StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                        requestWriter.Write(inputJson);
                        requestWriter.Close();

                        WebResponse webResponse = request.GetResponse();
                        Stream webStream = webResponse.GetResponseStream();
                        StreamReader responseReader = new StreamReader(webStream);
                        results = responseReader.ReadToEnd();
                        Console.Out.WriteLine(results);
                        responseReader.Close();

                        utilObj.DeleteCachedDataKeys("tstruct", "axglo");
                    }
                    catch (Exception ex)
                    {
                        LogFile.Log logObj = new LogFile.Log();
                        logObj.CreateLog("Tstruct tstDeleteFieldReactWS -Error - " + ex.Message, HttpContext.Current.Session.SessionID, "tstDeleteFieldReactWS", "");
                    }
                }
                else
                    results = "";
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("Tstruct tstDeleteFieldReactWS -Error - " + ex.Message, HttpContext.Current.Session.SessionID, "tstDeleteFieldReactWS2", "");
            }
            return results;
        }

        [WebMethod(EnableSession = true)]
        public string openDevStudioRedis(string ptype)
        {
            string res = string.Empty;
            try
            {
                DevFDW devfdwObj = new DevFDW();// DevFDW.DevInstance;
                bool added = devfdwObj.writeSessionHash(ptype);
                if (added)
                    res = "done";
            }
            catch (Exception ex) { }
            return res;
        }
        [WebMethod(EnableSession = true)]
        public string isDevStudioOpenedRedis(string ptype)
        {
            string res = "defSiteNoteOpend";
            try
            {
                DevFDW devfdwObj = new DevFDW();
                res = devfdwObj.HashGetKey(ptype);
                if (res != null && res != "")
                    res = "defSiteOpend";
                else
                    res = "defSiteNoteOpend";
            }
            catch (Exception ex)
            {
                res = "defSiteNoteOpend";
            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string getSQLHintObj()
        {

            DBquery DBQobj = new DBquery();
            WebServiceExt asbEx = new WebServiceExt();
            string query = string.Empty;
            string result = string.Empty;
            try
            {
                query = DBQobj.getSQLqueryString("", "getTblClmnHint");
                result = asbEx.ExecuteSQL("", query, "JSON");
                string errrMsg = utils.ParseJSonErrorNode(result);
                if (errrMsg == string.Empty)
                    return result;
                else
                    return string.Empty;

            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Exception in getSQLHintObj - util.cs -" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "getSQLHintObj", "new");
                return string.Empty;
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetPreviewInfo(string pName)
        {
            string axpertWebUrl = string.Empty;
            try
            {
                JObject adDetails = new JObject();

                adDetails["project"] = HttpContext.Current.Session["project"].ToString();
                adDetails["username"] = HttpContext.Current.Session["user"].ToString();
                adDetails["lang"] = HttpContext.Current.Session["language"].ToString();
                adDetails["roles"] = HttpContext.Current.Session["userroles"].ToString();
                adDetails["hguid"] = HttpContext.Current.Session["hybridGUID"].ToString();
                adDetails["pName"] = pName;

                var appurl = Convert.ToString(System.Web.HttpContext.Current.Request.Url);
                Uri uri = new Uri(appurl);
                string baseUrl = string.Format("{0}://{1}", uri.Scheme, uri.Authority);
                string[] pathSegments = uri.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (pathSegments.Length > 0)
                {
                    baseUrl = string.Format("{0}/{1}", baseUrl, pathSegments[0]);
                }
                axpertWebUrl = baseUrl;
                axpertWebUrl += "/aspx/AxMainAuth.aspx?encAuth=" + utils.encrtptDecryptAES(adDetails.ToString(Newtonsoft.Json.Formatting.None));
            }
            catch (Exception ex)
            {
                axpertWebUrl = string.Empty;
            }
            return axpertWebUrl;
        }

        [WebMethod(EnableSession = true)]
        public string GetCardsWithData(bool isHomePage, bool refreshData)
        {
            string result = String.Empty;
            try
            {
                AnalyticsUtils _aUtils = new AnalyticsUtils();
                string apiUrl = _aUtils.ARM_URL + "/api/v1/GetCardsWithData";

                var inputJson = new
                {
                    ARMSessionId = _aUtils.ARMSessionId,
                    AxSessionId = HttpContext.Current.Session.SessionID,
                    Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
                    AppName = HttpContext.Current.Session["project"].ToString(),
                    Roles = HttpContext.Current.Session["AxRole"].ToString(),
                    UserName = HttpContext.Current.Session["username"].ToString(),
                    SchemaName = HttpContext.Current.Session["dbuser"].ToString(),
                    Language = HttpContext.Current.Session["language"].ToString(),
                    HomePageCards = isHomePage,
                    RefreshData = refreshData
                    //,GlobalParams = _aUtils.GetGlobalParams()
                };

                var cardsData = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));

                if (cardsData != "")
                {
                    try
                    {
                        var cardDataJson = JObject.Parse(cardsData)["result"];
                        if (cardDataJson["data"] != null && !string.IsNullOrEmpty(cardDataJson["data"].ToString()))
                        {
                            cardsData = cardDataJson["data"].ToString();
                        }
                        else if (cardDataJson["statuscode"] != null && cardDataJson["statuscode"].ToString() == "500")
                        {
                            LogFile.Log logobj = new LogFile.Log();
                            logobj.CreateLog("Error in GetCardsWithData: " + cardsData, HttpContext.Current.Session["nsessionid"].ToString(), "GetCardsWithData", "new", "true");
                            cardsData = "";
                        }
                        else if (cardDataJson["success"] != null && cardDataJson["success"].ToString().ToLower() == "false")
                        {
                            LogFile.Log logobj = new LogFile.Log();
                            logobj.CreateLog("Error in GetCardsWithData: " + cardsData, HttpContext.Current.Session["nsessionid"].ToString(), "GetCardsWithData", "new", "true");

                            if (cardDataJson["message"] != null)
                            {
                                cardsData = cardDataJson["message"].ToString();
                            }
                            else
                                cardsData = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        LogFile.Log logobj = new LogFile.Log();
                        logobj.CreateLog("Error in GetCardsWithData: " + cardsData + " Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetCardsWithData", "new", "true");
                        cardsData = "";
                    }
                }
                return cardsData;
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Error in GetCardsWithData: Exception:" + ex.Message, HttpContext.Current.Session["nsessionid"].ToString(), "GetCardsWithData", "new", "true");
                return "";
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetDataFromExternalAPI(string ApiUrl, string ApiMethodType, string ApiRespType)
        {
            string result = string.Empty;
            ApiMethodType = ApiMethodType.ToUpper();
            string DATA = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);
            request.Method = ApiMethodType;
            request.ContentType = "application/" + ApiRespType;

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                result = responseReader.ReadToEnd();
                Console.Out.WriteLine(result);
                responseReader.Close();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }


        [WebMethod(EnableSession = true)]
        public string AxpStrDataEncrypt(string value)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string res = string.Empty;
            try
            {
                res = AXPEncryptData.CallEncryptData(value);
            }
            catch (Exception ex)
            { }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string AxpStrDataDeEncrypt(string value)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string res = string.Empty;
            try
            {
                res = AXPEncryptData.CallDecryptData(value);
            }
            catch (Exception ex)
            { }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string AxpFieldDataEncrypt(string inputJson)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string res = string.Empty;
            try
            {
                res = AXPEncryptData.CallEncryptDataJson(inputJson);
            }
            catch (Exception ex)
            { }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string AxpFieldDataDecrypt(string inputJson, ArrayList encryptFldList, string isFromIview = "false")
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string res = string.Empty;
            try
            {
                inputJson = inputJson.Replace(@"\", "\\\\");
                string[] parts = inputJson.Split(new[] { "*$*" }, StringSplitOptions.RemoveEmptyEntries);
                string jsonData = parts.FirstOrDefault(p =>
                {
                    try
                    {
                        JObject obj = JObject.Parse(p);
                        return obj["data"] != null;
                    }
                    catch
                    {
                        return false;
                    }
                });
                if (string.IsNullOrEmpty(jsonData))
                    return inputJson;
                if (isFromIview == "false")
                {
                    JObject jsonObject = JObject.Parse(jsonData);
                    JArray dataArray = (JArray)jsonObject["data"];
                    var filteredItems = dataArray
                        .Where(item => encryptFldList.Contains((string)item["n"]))
                        .ToList();
                    string resultJson = JsonConvert.SerializeObject(filteredItems, Newtonsoft.Json.Formatting.Indented);

                    string decryptedJson = AXPEncryptData.CallDecryptDataJson(resultJson);

                    JArray decryptedArray = JArray.Parse(decryptedJson);
                    foreach (JObject decryptedItem in decryptedArray)
                    {
                        string fieldName = (string)decryptedItem["n"];
                        string rowNumber = (string)decryptedItem["r"];
                        JObject fullItem = dataArray
                            .FirstOrDefault(d => (string)d["n"] == fieldName && (string)d["r"] == rowNumber) as JObject;
                        if (fullItem != null)
                        {
                            fullItem["v"] = decryptedItem["v"];
                        }
                    }
                    res = inputJson.Replace(jsonData, jsonObject.ToString(Newtonsoft.Json.Formatting.None));
                }
                else
                {
                    JObject jsonObject = JObject.Parse(jsonData);
                    JArray rowArray = (JArray)jsonObject["data"]["row"];
                    JArray itemsToDecrypt = new JArray();
                    foreach (JObject row in rowArray)
                    {
                        string rowno = (string)row["rowno"];
                        foreach (string fld in encryptFldList)
                        {
                            if (row[fld] != null)
                            {
                                JObject obj = new JObject();
                                obj.Add("n", fld);
                                obj.Add("r", rowno);
                                obj.Add("v", row[fld]);
                                itemsToDecrypt.Add(obj);
                            }
                        }
                    }
                    string encryptedJson = JsonConvert.SerializeObject(itemsToDecrypt);
                    string decryptedJson = AXPEncryptData.CallDecryptDataJson(encryptedJson);
                    JArray decryptedArray = JArray.Parse(decryptedJson);
                    foreach (JObject decryptedItem in decryptedArray)
                    {
                        string fieldName = (string)decryptedItem["n"];
                        string rowNumber = (string)decryptedItem["r"];
                        string decryptedValue = (string)decryptedItem["v"];
                        JObject matchingRow = rowArray
                            .FirstOrDefault(r => (string)r["rowno"] == rowNumber) as JObject;
                        if (matchingRow != null)
                        {
                            matchingRow[fieldName] = decryptedValue;
                        }
                    }
                    res = jsonObject.ToString(Newtonsoft.Json.Formatting.None);
                }

                res = res.Replace(@"\\", @"\");
            }
            catch (Exception ex)
            { }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string CachedSaveFldDataEncrypt(string inputJson, ArrayList encryptFldList)
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string res = string.Empty;
            try
            {
                JObject jsonObject = JObject.Parse(inputJson);
                JArray parameters = (JArray)jsonObject["_parameters"];
                JObject savedata = (JObject)parameters[0]["savedata"];
                JArray recdata = (JArray)savedata["recdata"];
                List<JObject> extractedFields = new List<JObject>();
                foreach (JObject record in recdata)
                {
                    foreach (JProperty property in record.Properties())
                    {
                        JArray rows = (JArray)property.Value;
                        foreach (JObject row in rows)
                        {
                            string rowNumber = (string)row["rowno"];
                            JObject columns = (JObject)row["columns"];

                            foreach (object fieldObj in encryptFldList)
                            {
                                string field = fieldObj.ToString();
                                if (columns.Property(field) != null)
                                {
                                    extractedFields.Add(new JObject { { "axp_recid", property.Name.Replace("axp_recid", "") }, { "n", field }, { "r", rowNumber }, { "v", columns[field] } });
                                }
                            }
                        }
                    }
                }
                string resultJson = JsonConvert.SerializeObject(extractedFields, Newtonsoft.Json.Formatting.Indented);
                string _decRes = AXPEncryptData.CallEncryptDataJson(resultJson);
                JArray decryptedArray = JArray.Parse(_decRes);
                foreach (JObject decryptedItem in decryptedArray)
                {
                    string recid = "axp_recid" + (string)decryptedItem["axp_recid"];
                    string fieldName = (string)decryptedItem["n"];
                    string rowNumber = (string)decryptedItem["r"];
                    string encryptedValue = (string)decryptedItem["v"];
                    foreach (JObject record in recdata)
                    {
                        if (record.Property(recid) != null)
                        {
                            JArray rowArray = (JArray)record[recid];
                            foreach (JObject row in rowArray)
                            {
                                if ((string)row["rowno"] == rowNumber)
                                {
                                    JObject columns = (JObject)row["columns"];
                                    if (columns.Property(fieldName) != null)
                                    {
                                        columns[fieldName] = columns[fieldName].ToString() + "*$*" + encryptedValue;
                                    }
                                }
                            }
                        }
                    }
                }
                res = jsonObject.ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            { }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string CheckFormIsLocked(string ptype, string structName, string formComponentsFlag)
        {
            string res = "notlocked";
            try
            {
                if (formComponentsFlag != "")
                {
                    string[] _formComps = formComponentsFlag.Split('~');
                    DeleteLockedKey(_formComps[0], _formComps[1]);
                }
                DevFDW devfdwObj = new DevFDW();
                res = devfdwObj.HashGetKeyLocked(ptype, structName);
                if (res == null || res == "")
                    res = "notlocked";
                else
                    res = res.Split('♠')[0];
            }
            catch (Exception ex)
            {
                res = "notlocked";
            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteLockedKey(string ptype, string structName)
        {
            bool res = false;
            try
            {
                DevFDW devfdwObj = new DevFDW();
                res = devfdwObj.HashDeletekeyLocked(ptype, structName);
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

        public void addFieldDynamically(string inputXml)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(inputXml);
                XmlNode tftransid = xmlDoc.SelectSingleNode("//root/varlist/row/tftransid");
                XmlNode tfcaption = xmlDoc.SelectSingleNode("//root/varlist/row/tfcaption");
                XmlNode dc2fldprops = xmlDoc.SelectSingleNode("//root/varlist/row/dc2fldprops");
                XmlNode dc3fldprops = xmlDoc.SelectSingleNode("//root/varlist/row/dc3fldprops");
                AddFieldRuntimeWS(tftransid.InnerText, tfcaption.InnerText, dc2fldprops.InnerText, "new", "", "");

                AddFieldRuntimeWS(tftransid.InnerText, tfcaption.InnerText, dc3fldprops.InnerText, "new", "", "");
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("Tstruct addFieldDynamically -Error - " + ex.Message, HttpContext.Current.Session.SessionID, "addFieldDynamically", "");
            }
        }
        public string LoginOTPPushToQueue(string _proj, string _uname, string _loginRes, string otpAuthExpiry, string axOTPAuthCahrs, string _isotpNumeric, string _JsonSmtp)
        {
            string response = string.Empty;
            try
            {
                string DATA = string.Empty;
                string URL = String.Empty;
                string _emailSubject = string.Empty;
                string _emailBody = string.Empty;
                string _smsMesg = string.Empty;
                if (_JsonSmtp != string.Empty)
                {
                    if (_JsonSmtp != "noemailsettings")
                    {
                        JObject _jsonAxEmail = JObject.Parse(_JsonSmtp);
                        if (_jsonAxEmail["arm_url"] != null)
                        {
                            URL = _jsonAxEmail["arm_url"].ToString();
                            _emailSubject = _jsonAxEmail["emailsubject"].ToString();
                            _emailBody = _jsonAxEmail["emailbody"].ToString();
                            _smsMesg = _jsonAxEmail["smscontent"].ToString();
                        }
                        else
                            return "ARM connection does not exist. Please contact the administrator.";
                    }
                    else
                    {
                        return "Email settings does not exist. Please contact the administrator.";
                    }
                }
                //else
                //{
                //    FDR fdrObj = new FDR();
                //    string jsoncontents = fdrObj.StringFromRedis(Constants.AXEMAILSMTP_CONN_KEY, _proj);
                //    if (jsoncontents == string.Empty)
                //    {
                //        FileInfo _emailfi = new FileInfo(utilObj.ScriptsPath + "\\emailsettings.ini");
                //        if (_emailfi.Exists)
                //        {
                //            string scriptsPathAxEmail = utilObj.ScriptsPath + "\\emailsettings.ini";
                //            string filePatharm = @" " + scriptsPathAxEmail + "";
                //            string existingJsonAxEmail = File.ReadAllText(filePatharm);
                //            JObject jsonAxEmail = JObject.Parse(existingJsonAxEmail);
                //            if (jsonAxEmail[_proj] != null)
                //            {
                //                jsoncontents = jsonAxEmail[_proj].ToString();
                //                JObject _jsonAxEmail = JObject.Parse(jsoncontents);
                //                if (_jsonAxEmail["arm_url"] != null)
                //                    URL = _jsonAxEmail["arm_url"].ToString();
                //                else
                //                    return "ARM connection does not exist. Please contact the administrator.";
                //                FDW fdwObj = new FDW();
                //                fdwObj.SaveInRedisServer(Constants.AXEMAILSMTP_CONN_KEY, jsoncontents, Constants.AXEMAILSMTP_CONN_KEY, _proj);
                //            }
                //            else
                //            {
                //                FDW fdwObj = new FDW();
                //                fdwObj.SaveInRedisServer(Constants.AXEMAILSMTP_CONN_KEY, "noemailsettings", Constants.AXEMAILSMTP_CONN_KEY, _proj);
                //                return "Email settings does not exist. Please contact the administrator.";
                //            }
                //        }
                //        else
                //        {
                //            FDW fdwObj = new FDW();
                //            fdwObj.SaveInRedisServer(Constants.AXEMAILSMTP_CONN_KEY, "noemailsettings", Constants.AXEMAILSMTP_CONN_KEY, _proj);
                //            return "Email settings does not exist. Please contact the administrator.";
                //        }
                //    }
                //    else if (jsoncontents != "noemailsettings")
                //    {
                //        JObject _jsonAxEmail = JObject.Parse(jsoncontents);
                //        if (_jsonAxEmail["arm_url"] != null)
                //            URL = _jsonAxEmail["arm_url"].ToString();
                //        else
                //            return "ARM connection does not exist. Please contact the administrator.";
                //    }
                //    else
                //    {
                //        return "Email settings does not exist. Please contact the administrator.";
                //    }
                //}

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";

                string _otp = string.Empty;
                if (_isotpNumeric == "true")
                {
                    const string digits = "0123456789";
                    int otpLength = int.Parse(axOTPAuthCahrs);
                    var otpBuilder = new StringBuilder();
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        byte[] buffer = new byte[1];
                        while (otpBuilder.Length < otpLength)
                        {
                            rng.GetBytes(buffer);
                            int randomValue = buffer[0] % digits.Length;
                            if (buffer[0] < digits.Length * (256 / digits.Length))
                            {
                                otpBuilder.Append(digits[randomValue]);
                            }
                        }
                    }
                    _otp = otpBuilder.ToString();
                    HttpContext.Current.Session["_otpAuth"] = _otp;
                }
                else
                {
                    const string digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    int otpLength = int.Parse(axOTPAuthCahrs);
                    var otpBuilder = new StringBuilder();
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        byte[] buffer = new byte[1];
                        while (otpBuilder.Length < otpLength)
                        {
                            rng.GetBytes(buffer);
                            int randomValue = buffer[0] % digits.Length;
                            if (buffer[0] < digits.Length * (256 / digits.Length))
                            {
                                otpBuilder.Append(digits[randomValue]);
                            }
                        }
                    }
                    _otp = otpBuilder.ToString();
                    HttpContext.Current.Session["_otpAuth"] = _otp;
                }

                string _emailid = string.Empty, _mobileNo = string.Empty;
                if (_loginRes != "")
                {
                    var match = Regex.Match(_loginRes, "email_id\\s*=\\s*\"([^\"]*)\"");
                    if (match.Success)
                        _emailid = match.Groups[1].Value;

                    var matchMob = Regex.Match(_loginRes, "mobileno\\s*=\\s*\"([^\"]*)\"");
                    if (matchMob.Success)
                        _mobileNo = matchMob.Groups[1].Value;
                }
                else
                {
                    if (Session["userEmail_id"] != null)
                        _emailid = Session["userEmail_id"].ToString();
                    if (Session["userMobileno"] != null)
                        _mobileNo = Session["userMobileno"].ToString();
                }

                string _esredisKey = _proj + "-" + Constants.AXEMAILSMTP_CONN_KEY;
                string redisIp = string.Empty, redisPort = string.Empty, redisPwd = string.Empty;
                if (HttpContext.Current.Session["RedisCacheIP"] != null)
                {
                    redisIp = HttpContext.Current.Session["RedisCacheIP"].ToString();
                    redisPort = redisIp.Split(':')[1];
                    redisIp = redisIp.Split(':')[0];
                }
                if (HttpContext.Current.Session["RedisCachePwd"] != null)
                {
                    redisPwd = HttpContext.Current.Session["RedisCachePwd"].ToString();
                }

                if (HttpContext.Current.Session["RedisCacheIP"] == null)
                {
                    string rcDetails = utilObj.GetRedisConnDetails(_proj);
                    if (rcDetails != "")
                    {
                        redisIp = rcDetails.Split('♣')[0];
                        redisPort = redisIp.Split(':')[1];
                        redisIp = redisIp.Split(':')[0];
                        redisPwd = rcDetails.Split('♣')[1];
                    }
                }

                if (_emailSubject == string.Empty)
                    _emailSubject = "Axpert OTP Authentication";
                if (_emailBody == string.Empty)
                    _emailBody = "Dear " + _uname + ",<br>One Time Password(OTP) for Axpert login is " + _otp + " <br>Regards,<br>Support Team.";
                else
                {
                    _emailBody = _emailBody.Replace("{username}", _uname);
                    _emailBody = _emailBody.Replace("{otp}", _otp);
                    _emailBody = _emailBody.Replace("{otpexpirytime}", otpAuthExpiry);
                }
                if (_smsMesg == string.Empty)
                    _smsMesg = "Dear " + _uname + ",\\nOne Time Password(OTP) for Axpert login is " + _otp + "";
                else
                {
                    _smsMesg = _smsMesg.Replace("<br>", "\\n");
                    _smsMesg = _smsMesg.Replace("{username}", _uname);
                    _smsMesg = _smsMesg.Replace("{otp}", _otp);
                    _smsMesg = _smsMesg.Replace("{otpexpirytime}", otpAuthExpiry);
                }
                int _otpAuthExpiry = int.Parse(otpAuthExpiry) * 60;
                DATA = "{\"email\":{\"to\":[\"" + _emailid + "\"],\"subject\": \"" + _emailSubject + "\",\"body\":[\"" + _emailBody + "\"]},\"sms\":{\"mobileno\":[\"" + _mobileNo + "\"],\"message\": [\"" + _smsMesg + "\"],\"otp\": \"" + _otp + "\"},\"redis\":{\"redisserver\":\"" + redisIp + "\",\"redisport\":\"" + redisPort + "\",\"redispwd\":\"" + redisPwd + "\",\"emailconfigkey\":\"" + _esredisKey + "\"}}";
                var saveDetails = "{\"queuename\":\"LoginOTPQueue\",\"queuedata\":" + JsonConvert.SerializeObject(DATA) + ",\"msgexpirysecs\":\"" + _otpAuthExpiry + "\"}";
                request.ContentLength = saveDetails.Length;

                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(saveDetails);
                requestWriter.Close();

                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    try
                    {
                        if (response != "" && response.IndexOf("\"success\":true") > -1)
                        {
                            response = "success♣" + _mobileNo + "♣" + _emailid;
                        }
                    }
                    catch (Exception ex) { }
                    return response;
                }
                catch (Exception e)
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogFile.Log logobj = new LogFile.Log();
                logobj.CreateLog("Exception in LoginOTPPushToQueue - util.cs-" + ex.Message, Session.SessionID, "LoginOTPPushToQueue", "new");
                response = ex.Message;
                return response;
            }
        }

        [WebMethod(EnableSession = true)]
        public string MultiFrameUpdateDataXML(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, ArrayList deletedFldArrayValues, string files, string rid, string delRows, string changedRows, string key, string resTstHtmlLS)
        {
            ObjExecTr.SetCurrentTime();
            DateTime sTime1 = DateTime.Now;
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string _tstKeyChanged = "";
            if (resTstHtmlLS != "")
            {
                utilObj.DeleteTstIvObject(key.Split('_')[0]);
                key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                _tstKeyChanged = key;
            }
            TStructData tstData = (TStructData)Session[key];
            if (tstData == null)
                return Constants.DUPLICATESESS;

            string result = string.Empty;
            DateTime eTime1 = DateTime.Now;

            Util.Util.DeletedraftRediskeys(tstData.transid);
            result = tstData.CallMultiFrameDataUpdate(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, deletedFldArrayValues, files, rid, delRows, changedRows);
            DateTime sTime2 = DateTime.Now;
            if (!result.StartsWith(Constants.ERROR))
            {
                if (tstData.IsDraftObj && HttpContext.Current.Session["AxEnableDraftSave"] != null && HttpContext.Current.Session["AxEnableDraftSave"].ToString() == "true")
                {
                    user = HttpContext.Current.Session["user"].ToString();
                    utilObj.DeleteDraft(user + "-" + tstData.transid, user);
                }
            }
            DateTime eTime2 = DateTime.Now;

            if (tstData.logTimeTaken)
                tstData.strServerTime = (tstData.webTime1 + (eTime1.Subtract(sTime1).TotalMilliseconds)) + "," + tstData.asbTime + "," + (tstData.webTime2 + (eTime2.Subtract(sTime2).TotalMilliseconds));
            if (_tstKeyChanged != "")
            {
                result = _tstKeyChanged + "♠*♠" + result;
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string MultiFrameDataPushToQueue(string multiframeJsonData)
        {
            string result = string.Empty;
            try
            {
                string response = string.Empty;
                string DATA = string.Empty;
                string URL = String.Empty;
                if (HttpContext.Current.Session["ARM_URL"] != null && HttpContext.Current.Session["ARMPushToQueue_API"] != null)
                    URL = HttpContext.Current.Session["ARM_URL"].ToString() + HttpContext.Current.Session["ARMPushToQueue_API"].ToString();
                string appsesskey = Session["AppSessionKey"].ToString();
                string globalvars = xmltojson(Session["axGlobalVars"].ToString());
                string uservars = xmltojson(Session["axUserVars"].ToString());
                string axapps = xmltojson(Session["axApps"].ToString());
                string pwd = utilObj.MD5Hash(Session["pwd"].ToString());
                string axprops = HttpContext.Current.Application["axProps"].ToString();
                axprops = xmltojson(axprops);
                string axappsNode = HttpContext.Current.Session["project"].ToString();
                JObject _obj = JObject.Parse(multiframeJsonData);
                foreach (KeyValuePair<string, JToken> formData in _obj)
                {
                    string formName = formData.Key;
                    JObject formContent = formData.Value as JObject;
                    if (formContent != null)
                    {
                        string tstDataId = formContent["tstDataId"] != null ? formContent["tstDataId"].ToString() : "";
                        string changedDcs = formContent["changedDcs"] != null ? formContent["changedDcs"].ToString() : "";
                        string recordid = formContent["recordid"] != null ? formContent["recordid"].ToString() : "";
                        string deledtedRows = formContent["delRows"] != null ? formContent["delRows"].ToString() : "";
                        string hfiles = formContent["files"] != null ? formContent["files"].ToString() : "";

                        JToken recdataNode = formContent["recdata"];
                        string recdataJsonString = recdataNode.ToString(Newtonsoft.Json.Formatting.None);
                        string jsonData = "\"recdata\":" + recdataJsonString;

                        TStructData tstData = (TStructData)Session[tstDataId];
                        if (tstData == null)
                            return Constants.DUPLICATESESS;
                        string _transId = tstData.transid;

                        string deledtedRowsJson = string.Empty;
                        if (deledtedRows != "")
                        {
                            deledtedRowsJson = tstData.GetDelRowsNode(deledtedRows);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(deledtedRowsJson);
                            deledtedRows = JsonConvert.SerializeXmlNode(doc);
                            deledtedRows = deledtedRows.Remove(deledtedRows.IndexOf("{"), 1);
                            deledtedRows = deledtedRows.Remove(deledtedRows.LastIndexOf("}"), 1);
                            deledtedRows += ",";
                        }
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        if (changedDcs == "{}")
                            DATA = "{\"_parameters\":[{\"savedata\":{\"cachedsave\":\"true\",\"axpapp\":\"" + HttpContext.Current.Session["project"].ToString() + "\",\"appsessionkey\":\"" + appsesskey + "\" " + ",\"transid\":\"" + _transId + "\",\"s\":\"" + Session.SessionID + "\",\"username\":\"" + Session["username"].ToString() + "\",\"password\":\"" + pwd + "\",\"afiles\":\"" + hfiles + "\",\"changedrows\":{},\"trace\":\"" + HttpContext.Current.Session["AxTrace"].ToString() + "\",\"recordid\":\"" + recordid + "\"," + deledtedRows + "";
                        else
                            DATA = "{\"_parameters\":[{\"savedata\":{\"cachedsave\":\"true\",\"axpapp\":\"" + HttpContext.Current.Session["project"].ToString() + "\",\"appsessionkey\":\"" + appsesskey + "\" " + ",\"transid\":\"" + _transId + "\",\"s\":\"" + Session.SessionID + "\",\"username\":\"" + Session["username"].ToString() + "\",\"password\":\"" + pwd + "\",\"afiles\":\"" + hfiles + "\",\"changedrows\":" + changedDcs + ",\"trace\":\"" + HttpContext.Current.Session["AxTrace"].ToString() + "\",\"recordid\":\"" + recordid + "\"," + deledtedRows + "";
                        DATA += jsonData + "},\"" + axappsNode + "\":{" + axapps + "},\"axprops\":{" + axprops + "},\"globalvars\":{" + globalvars + "},\"uservars\":{" + uservars + "}";
                        DATA += "}]}";
                        if (tstData.tstStrObj.FEncryptFlag.Count > 0)
                        {
                            DATA = CachedSaveFldDataEncrypt(DATA, tstData.tstStrObj.FEncryptFlag);
                        }
                        var saveDetails = "{\"queuename\":\"RapidSaveQueue\",\"queuedata\":" + JsonConvert.SerializeObject(DATA) + "}";
                        request.ContentLength = saveDetails.Length;
                        StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                        requestWriter.Write(saveDetails);
                        requestWriter.Close();
                        try
                        {
                            WebResponse webResponse = request.GetResponse();
                            Stream webStream = webResponse.GetResponseStream();
                            StreamReader responseReader = new StreamReader(webStream);
                            response = responseReader.ReadToEnd();
                            responseReader.Close();
                            try
                            {
                                HttpContext.Current.Session["AxpAttFileServer-" + _transId] = null;
                            }
                            catch (Exception ex) { }
                            result = response;
                        }
                        catch (Exception e)
                        {
                            result = response;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string LoadAndkeepTstructAPI(string tformId, string mtstDataId)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(mtstDataId) && !string.IsNullOrEmpty(tformId))
                {
                    TStructData tstData = (TStructData)HttpContext.Current.Session[mtstDataId];
                    if (tstData == null)
                        return Constants.DUPLICATESESS;
                    TStructDef strObj = tstData.tstStrObj;
                    ArrayList formCommonSessList = new ArrayList();
                    try
                    {
                        string consttstCommonSessList = "savedraftKeyCreatedtime,tstHeaderAttachDbFileOnLoad,tstHeaderAttachDbFile,StructureHtml,ToolbarBtnIcons,ModernToolbarBtnOpen,Axp_DesignJson,axp_IsSaveUrl,AxDtConfigs,AxpFldMultiSelect,AxpFldMultiSelSp,AxpautoCompPatt,AxpSaveImageDb,PickListFetchSize,showSqlColumnsAdvSearch,switchboxToCheckbox,AxtstAFSDB,AxHypTstRefresh,openerIV,IsFromProcess,AxRuleOnSubmit,lvRecordKey,IsCustomHtml,Axp_OldDesignJson,AxpAttFileServer,attGridFileServer,tstGridDbFiles,tstHeaderAttach";
                        foreach (var sessVar in consttstCommonSessList.Split(','))
                        {
                            if (Session[sessVar] != null)
                                formCommonSessList.Add(sessVar + "♦♣" + Session[sessVar].ToString());
                        }
                    }
                    catch (Exception ex) { }
                    strObj.tstCommonSessList = formCommonSessList;
                    tstData.tstStrObj = strObj;
                    Session.Add("MultiFrame_" + tformId, tstData);
                    result = "success";
                }
                else
                {
                    result = "error";
                }
            }
            catch (Exception ex)
            {
                result = "error";
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string ActivateTstructAPI(string tformId, string mtstDataId)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(tformId) && !string.IsNullOrEmpty(mtstDataId))
                {
                    TStructData tstData = (TStructData)HttpContext.Current.Session["MultiFrame_" + tformId];
                    if (tstData == null)
                        return Constants.DUPLICATESESS;
                    TStructDef strObj = tstData.tstStrObj;
                    ArrayList formCommonSessList = strObj.tstCommonSessList;
                    try
                    {
                        foreach (string sessVar in formCommonSessList)
                        {
                            string[] sessVarVal = sessVar.Split(new string[] { "♦♣" }, StringSplitOptions.None);
                            Session[sessVarVal[0]] = sessVarVal[1];
                        }
                    }
                    catch (Exception ex) { }
                    Session.Add(mtstDataId, tstData);
                    result = "success";
                }
                else
                {
                    result = "error";
                }
            }
            catch (Exception ex)
            {
                result = "error";
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public string BulkUserActivation()
        {
            if (HttpContext.Current.Session["project"] == null)
                return utilObj.SESSTIMEOUT;
            string jobId = Guid.NewGuid().ToString();
            try
            {
                string schemaName = HttpContext.Current.Session["dbuserALC"].ToString();
                DBContext obj = new DBContext();
                DataTable userList = obj.GetDataTableInline(Constants.SQL_Bulk_User_Activation);
                if (userList == null || userList.Rows.Count == 0)
                    return "Error: No users found for activation.";
                FDW fdwObj = new FDW();
                fdwObj.WriteKeyAutoExpire(utilObj.GetRedisServerkey(Constants.REDISBULKUSERACTIVATE, jobId), "Queued", 60);
                string strProj = HttpContext.Current.Session["project"].ToString();
                string _dbuser = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    _dbuser = HttpContext.Current.Session["dbuser"].ToString();
                string rcDetails = utilObj.GetRedisConnDetails(strProj);

                string _db_type = HttpContext.Current.Session["axdb"].ToString();
                string __connectionString = HttpContext.Current.Session["axconstr"].ToString();

                ThreadPool.QueueUserWorkItem(state =>
                {
                    BulkUserActivate objss = new BulkUserActivate();
                    objss.ProcessBulkActivation(jobId, userList, schemaName, _dbuser, rcDetails, _db_type, __connectionString);
                });
                return "JobQueued:" + jobId;
            }
            catch (Exception ex)
            {
                FDW fdwObj = new FDW();
                fdwObj.Deletekey(utilObj.GetRedisServerkey(Constants.REDISBULKUSERACTIVATE, jobId));

                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("BulkUserActivation Error: " + ex.Message, HttpContext.Current.Session.SessionID, "BulkUserActivation", "new");
                return "Error: Unable to start activation job.";
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetBulkActivationStatus(string jobId)
        {
            try
            {
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                var status = fdrObj.StringFromRedis(utilObj.GetRedisServerkey(Constants.REDISBULKUSERACTIVATE, jobId));
                string statusMsg = string.IsNullOrEmpty(status) ? "Pending" : status.ToString();
                if (statusMsg.StartsWith("Completed.") || statusMsg.StartsWith("Error:") || statusMsg.StartsWith("Pending"))
                {
                    FDW fdwObj = new FDW();
                    fdwObj.Deletekey(utilObj.GetRedisServerkey(Constants.REDISBULKUSERACTIVATE, jobId));
                }
                return statusMsg;
            }
            catch (Exception ex)
            {
                FDW fdwObj = new FDW();
                fdwObj.Deletekey(utilObj.GetRedisServerkey(Constants.REDISBULKUSERACTIVATE, jobId));
                return "Error:" + ex.Message;
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetInActiveUserCount()
        {
            try
            {
                DBContext obj = new DBContext();
                DataTable userList = obj.GetDataTableInline(Constants.SQL_InActive_User_Count);
                int count = 0;
                if (userList != null && userList.Rows.Count > 0)
                    count = Convert.ToInt32(userList.Rows[0]["InactiveCount"]);
                return count.ToString();
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("GetInActiveUserCount Error: " + ex.Message, HttpContext.Current.Session.SessionID, "GetInActiveUserCount", "new");
                return "Error: " + ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public string ExecuteScriptPushtoQueue(ArrayList fldArray, ArrayList fldDbRowNo, ArrayList fldValueArray, ArrayList fldDeletedArray, ArrayList deletedFldArrayValues, string ArrActionLog, string visibleDcs, string s, string delRows, string changedRows, string key, string resTstHtmlLS, string tid, string rid, string _scripName, string _scriptCaption, string _queueName, string tstructCaption)
        {
            string res = string.Empty;
            try
            {
                string response = string.Empty;
                if (HttpContext.Current.Session["project"] == null || Session["nsessionid"] == null)
                    return utilObj.SESSTIMEOUT;
                string _RKey = tid + "-execScript-" + HttpContext.Current.Session["username"].ToString() + "-" + _queueName;
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                string armExportExcell = fdrObj.ReadStringKey(_RKey);
                if (armExportExcell != string.Empty && armExportExcell == "requestinprocess")
                {
                    res = "keyexist";
                    return res;
                }

                bool isRMQExist = false;
                string rmqstatus = CheckRabbitMqStatus(_queueName);
                if (rmqstatus == "OK")
                    isRMQExist = true;
                else if (rmqstatus != "Queue does not exist or queue name incorrect")
                    return "RMQError:" + rmqstatus;

                string _tstKeyChanged = "";
                if (resTstHtmlLS != "")
                {
                    utilObj.DeleteTstIvObject(key.Split('_')[0]);
                    key = utilObj.GetReGenTstDataObj(key, resTstHtmlLS);
                    _tstKeyChanged = key;
                }
                TStructData tstData = (TStructData)Session[key];
                if (tstData == null)
                    return Constants.DUPLICATESESS;
                string resultsss = string.Empty;
                if (tstData != null)
                {
                    resultsss = tstData.GetVarListForScript(tstData, fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, deletedFldArrayValues, s, delRows, changedRows);

                }
                string DATA = string.Empty;
                string URL = String.Empty;
                string ArmScriptURL = String.Empty;
                if (HttpContext.Current.Session["ARM_URL"] != null && HttpContext.Current.Session["ARMPushToQueue_API"] != null)
                    URL = HttpContext.Current.Session["ARM_URL"].ToString() + HttpContext.Current.Session["ARMPushToQueue_API"].ToString();
                if (HttpContext.Current.Session["ARM_Scripts_URL"] != null)
                    ArmScriptURL = HttpContext.Current.Session["ARM_Scripts_URL"].ToString();

                if (URL == string.Empty || ArmScriptURL == string.Empty)
                    return "error:ARM Connection defined";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";

                string globalVars = HttpContext.Current.Session["axGlobalVars"].ToString();
                globalVars = globalVars.Replace(@"\", "\\\\");
                string _axapps = HttpContext.Current.Session["axApps"].ToString();
                _axapps = _axapps.Replace(@"\", "\\\\");

                //DATA = "{\"executescript\":{\"trace\":\"" + HttpContext.Current.Session["AxTrace"].ToString() + "\",\"project\":\"" + HttpContext.Current.Session["project"].ToString() + "\",\"s\":\"" + Session.SessionID + "\",\"username\":\"" + Session["username"].ToString() + "\",\"script\":\"" + _scripName + "\",\"type\":\"form\",\"name\":\"" + tid + "\",\"rid\":\"" + rid + "\",\"pageCaption\":\"" + tstructCaption + "\"},\"varlist\":{\"row\":{\"axp_recid1\":{\"_rowno\":\"001\",\"__text\":\"1185220000000\"},\"flda\":{\"_rowno\":\"001\",\"__text\":\"admin\"},\"fldb\":{\"_rowno\":\"001\",\"__text\":\"ARMAxpertJobsQueueLocal\"},\"apibody\":{\"_rowno\":\"001\"},\"queueName\":{\"_rowno\":\"001\"}}},\"axprops\":\"" + HttpContext.Current.Application["axProps"].ToString() + "\",\"axapps\":\"" + _axapps + "\",\"globalvars\":\"" + globalVars + "\",\"uservars\":\"" + HttpContext.Current.Session["axUserVars"].ToString() + "\"}";
                DATA = "{\"executescript\":{\"trace\":\"" + HttpContext.Current.Session["AxTrace"].ToString() + "\",\"project\":\"" + HttpContext.Current.Session["project"].ToString() + "\",\"s\":\"" + Session.SessionID + "\",\"username\":\"" + Session["username"].ToString() + "\",\"script\":\"" + _scripName + "\",\"scriptcaption\":\"" + _scriptCaption + "\",\"scriptqueuename\":\"" + _queueName + "\",\"type\":\"form\",\"name\":\"" + tid + "\",\"rid\":\"" + rid + "\",\"pageCaption\":\"" + tstructCaption + "\"}," + resultsss.Replace("\r\n", "") + ",\"axprops\":\"" + HttpContext.Current.Application["axProps"].ToString() + "\",\"axapps\":\"" + _axapps + "\",\"globalvars\":\"" + globalVars + "\",\"uservars\":\"" + HttpContext.Current.Session["axUserVars"].ToString() + "\"}";

                var saveDetails = "{\"queuename\":\"" + _queueName + "\",\"queuedata\":" + JsonConvert.SerializeObject(DATA) + ",\"timespandelay\":\"0\"}";

                request.ContentLength = saveDetails.Length;

                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(saveDetails);
                requestWriter.Close();

                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    if (!isRMQExist)
                    {
                        string _rmqstatus = CheckRabbitMqStatus(_queueName);
                        if (_rmqstatus != "OK")
                            return "RMQError:" + _rmqstatus;
                        else
                            return response;
                    }
                    else
                        return response;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                    return "error:" + e.Message;
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string ExecuteScriptPushtoQueueIview(string _scripName, string _scriptCaption, string _queueName, string iviewName, string iviewCaption, string ivXml)
        {
            string res = string.Empty;
            try
            {
                string response = string.Empty;
                if (HttpContext.Current.Session["project"] == null || Session["nsessionid"] == null)
                    return utilObj.SESSTIMEOUT;
                string _RKey = iviewName + "-execScript-" + HttpContext.Current.Session["username"].ToString() + "-" + _queueName;
                FDR fdrObj = (FDR)HttpContext.Current.Session["FDR"];
                string armExportExcell = fdrObj.ReadStringKey(_RKey);
                if (armExportExcell != string.Empty && armExportExcell == "requestinprocess")
                {
                    res = "keyexist";
                    return res;
                }

                bool isRMQExist = false;
                string rmqstatus = CheckRabbitMqStatus(_queueName);
                if (rmqstatus == "OK")
                    isRMQExist = true;
                else if (rmqstatus != "Queue does not exist or queue name incorrect")
                    return "RMQError:" + rmqstatus;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(ivXml);
                XmlNode paramsObj = xmlDoc.SelectSingleNode("//params");
                if (paramsObj == null)
                    return "{}";
                string paramjson = JsonConvert.SerializeXmlNode(paramsObj, Newtonsoft.Json.Formatting.None, false);
                JObject fullParamObj = JObject.Parse(paramjson);
                JToken paramsObject = fullParamObj["params"];
                JObject Paramsoutput = new JObject();
                Paramsoutput["params"] = paramsObject;
                string _params = JsonConvert.SerializeObject(Paramsoutput, Newtonsoft.Json.Formatting.Indented);
                if (_params.StartsWith("{"))
                    _params = _params.Substring(1);
                if (_params.EndsWith("}"))
                    _params = _params.Substring(0, _params.Length - 1);

                XmlNode varlist = xmlDoc.SelectSingleNode("//varlist");
                if (varlist == null)
                    return "{}";
                string json = JsonConvert.SerializeXmlNode(varlist, Newtonsoft.Json.Formatting.None, false);
                JObject fullObj = JObject.Parse(json);
                JToken varlistObject = fullObj["varlist"];
                JObject output = new JObject();
                output["varlist"] = varlistObject;
                string _varlist = JsonConvert.SerializeObject(output, Newtonsoft.Json.Formatting.Indented);
                if (_varlist.StartsWith("{"))
                    _varlist = _varlist.Substring(1);
                if (_varlist.EndsWith("}"))
                    _varlist = _varlist.Substring(0, _varlist.Length - 1);

                string DATA = string.Empty;
                string URL = String.Empty;
                string ArmScriptURL = String.Empty;
                if (HttpContext.Current.Session["ARM_URL"] != null && HttpContext.Current.Session["ARMPushToQueue_API"] != null)
                    URL = HttpContext.Current.Session["ARM_URL"].ToString() + HttpContext.Current.Session["ARMPushToQueue_API"].ToString();
                if (HttpContext.Current.Session["ARM_Scripts_URL"] != null)
                    ArmScriptURL = HttpContext.Current.Session["ARM_Scripts_URL"].ToString();

                if (URL == string.Empty || ArmScriptURL == string.Empty)
                    return "error:ARM Connection defined";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";

                string globalVars = HttpContext.Current.Session["axGlobalVars"].ToString();
                globalVars = globalVars.Replace(@"\", "\\\\");
                string _axapps = HttpContext.Current.Session["axApps"].ToString();
                _axapps = _axapps.Replace(@"\", "\\\\");

                DATA = "{\"executescript\":{\"trace\":\"" + HttpContext.Current.Session["AxTrace"].ToString() + "\",\"project\":\"" + HttpContext.Current.Session["project"].ToString() + "\",\"s\":\"" + Session.SessionID + "\",\"username\":\"" + Session["username"].ToString() + "\",\"script\":\"" + _scripName + "\",\"scriptcaption\":\"" + _scriptCaption + "\",\"scriptqueuename\":\"" + _queueName + "\",\"type\":\"report\",\"name\":\"" + iviewName + "\",\"pageCaption\":\"" + iviewCaption + "\"}," + _params.Replace("\r\n", "") + "," + _varlist.Replace("\r\n", "") + ",\"axprops\":\"" + HttpContext.Current.Application["axProps"].ToString() + "\",\"axapps\":\"" + _axapps + "\",\"globalvars\":\"" + globalVars + "\",\"uservars\":\"" + HttpContext.Current.Session["axUserVars"].ToString() + "\"}";

                var saveDetails = "{\"queuename\":\"" + _queueName + "\",\"queuedata\":" + JsonConvert.SerializeObject(DATA) + ",\"timespandelay\":\"0\"}";

                request.ContentLength = saveDetails.Length;

                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(saveDetails);
                requestWriter.Close();

                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    if (!isRMQExist)
                    {
                        string _rmqstatus = CheckRabbitMqStatus(_queueName);
                        if (_rmqstatus != "OK")
                            return "RMQError:" + _rmqstatus;
                        else
                            return response;
                    }
                    else
                        return response;
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                    return "error:" + e.Message;
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        [WebMethod(EnableSession = true)]
        public string CheckRabbitMqStatus(string queueName)
        {
            string amqpUrl = string.Empty;
            try
            {
                if (HttpContext.Current.Session["project"] == null || Session["nsessionid"] == null)
                    return utilObj.SESSTIMEOUT;
                amqpUrl = HttpContext.Current.Session["AxRabbitMQSet"].ToString();
                LogFile.Log logObj = new LogFile.Log();
                if (amqpUrl == "noRMQSettings")
                {
                    logObj.CreateLog("CheckRabbitMqStatus queueName:" + queueName + " amqpUrl: " + amqpUrl, HttpContext.Current.Session.SessionID, "CheckRabbitMqStatus", "new");
                    return "RabbitMQ Server Setting not found, please contact application Administrator.";
                }
                var result = CheckConsumerStatus(amqpUrl, queueName);
                switch (result)
                {
                    case QueueStatus.ServerDown:
                        return "RMQueue Server is DOWN or not reachable, please contact Administrator.";

                    case QueueStatus.InvalidSettings:
                        return "RMQueue Settings are incorrect, please contact Administrator.";

                    case QueueStatus.QueueNotFound:
                        return "Queue does not exist or queue name incorrect";

                    case QueueStatus.NoConsumer:
                        return "Required Consumer Service is not running, please contact Administrator.";

                    case QueueStatus.ConsumerActive:
                        return "OK";
                }
                logObj.CreateLog("CheckRabbitMqStatus queueName:" + queueName + " amqpUrl: " + amqpUrl + " result:" + result, HttpContext.Current.Session.SessionID, "CheckRabbitMqStatus", "new");
                return "error: " + result;
            }
            catch (Exception ex)
            {
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("CheckRabbitMqStatus exception queueName:" + queueName + " amqpUrl: " + amqpUrl + " Eexception: " + ex.Message, HttpContext.Current.Session.SessionID, "CheckRabbitMqStatus", "new");
                return "error: Exception-" + ex.Message;
            }
        }

        public static QueueStatus CheckConsumerStatus(string amqpUrl, string queueName)
        {
            try
            {
                var factory = new ConnectionFactory();
                factory.Uri = new Uri(amqpUrl);

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    try
                    {
                        channel.QueueDeclarePassive(queueName);
                    }
                    catch (RabbitMQ.Client.Exceptions.OperationInterruptedException ex)
                    {
                        if (ex.ShutdownReason.ReplyCode == 404)
                            return QueueStatus.QueueNotFound;
                        return QueueStatus.InvalidSettings;
                    }
                    try
                    {
                        string tag = channel.BasicConsume(
                            queue: queueName,
                            autoAck: false,
                            consumer: new DummyConsumer(channel),
                            exclusive: true
                        );

                        channel.BasicCancel(tag);
                        return QueueStatus.NoConsumer;
                    }
                    catch (RabbitMQ.Client.Exceptions.OperationInterruptedException)
                    {
                        return QueueStatus.ConsumerActive;
                    }
                }
            }
            catch
            {
                return QueueStatus.ServerDown;
            }
        }
    }
}
public class DummyConsumer : DefaultBasicConsumer
{
    public DummyConsumer(IModel model) : base(model) { }
}

public enum QueueStatus
{
    ServerDown,
    InvalidSettings,
    QueueNotFound,
    NoConsumer,
    ConsumerActive
}
