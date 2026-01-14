using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Web.Caching;
using System.Configuration;
using System.Data;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

namespace CacheMgr
{
    /// <summary>
    /// Summary description for CacheManager
    /// </summary>
    // TODO: Make this class as singleton
    [Serializable]
    public class CacheManager
    {

        LogFile.Log logobj = new LogFile.Log();
        Custom customobj = Custom.Instance;
        string errorLog = string.Empty;
        private string structHTML = string.Empty;
        public DesignObject axdesignJObcmg = new DesignObject();
        private string structScript = string.Empty;
        private string structBtns = string.Empty;
        private string taskBtns = string.Empty;
        private string structCaption = string.Empty;
        private string structName = string.Empty;
        private string strSubmitCancel = string.Empty;
        private string attachHTML = string.Empty;
        private string tstHeaderHTML = string.Empty;
        private string toolbarBtnIcons = string.Empty;
        private string modernToolbarBtnOpen = string.Empty;
        private Boolean IsTstObjNew = true;    // the value needs to be false in production. 
        private int NoOfDaysForExpiry = 1;
        Util.Util util = new Util.Util();
        Custom cusObj = Custom.Instance;
        public string tstCustomHTML = string.Empty;
        public string requestProcess_log = string.Empty;

        //static RedisServer fdwObj = RedisServer.Instance;
        public FDW fdwObj = new FDW();


        public string StructureHtml
        {
            get { return structHTML; }
            set { structHTML = value; }
        }

        public string TstHeaderHtml
        {
            get { return tstHeaderHTML; }
            set { tstHeaderHTML = value; }
        }

        public string ToolbarBtnIcons
        {
            get { return toolbarBtnIcons; }
            set { toolbarBtnIcons = value; }
        }

        public string ModernToolbarBtnOpen
        {
            get { return modernToolbarBtnOpen; }
            set { modernToolbarBtnOpen = value; }
        }

        public string AttachmentHtml
        {
            get { return attachHTML; }
            set { attachHTML = value; }
        }

        public string StructureScript
        {
            get { return structScript; }
            set { structScript = value; }
        }

        public string StructureBtns
        {
            get { return structBtns; }
            set { structBtns = value; }
        }

        public string TaskButtons
        {
            get { return taskBtns; }
            set { taskBtns = value; }
        }

        public string StructureCaption
        {
            get { return structCaption; }
            set { structCaption = value; }
        }

        public string StructureName
        {
            get { return structName; }
            set { structName = value; }
        }

        public string StructureSubmitCancel
        {
            get { return strSubmitCancel; }
            set { strSubmitCancel = value; }
        }

        public Boolean IsTstructNew
        {
            get { return IsTstObjNew; }
            set { IsTstObjNew = value; }
        }

        public CacheManager(string errorLog)
        {
            this.errorLog = errorLog;
        }

        /// <summary>
        /// Function to get the TStruct definition object from Cache.
        /// If it exist in cache then check if the structure was changed, if it has not changed then get it from cache.
        /// else get the structure from the db.
        /// </summary>
        /// <param name="Proj">Project name</param>
        /// <param name="Sid">Session Id</param>
        /// <param name="User">User name</param>
        /// <param name="TransId">Transaction Id</param>
        /// <param name="AxRole">Role</param>
        /// <returns>struct def object.</returns>
        public TStructDef GetStructDef(string projectName, string sessionID, string userName, string transId, string AxRole)
        {

            TStructDef strObj;
            string fdKey = Constants.REDISTSTRUCT;
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                fdKey = Constants.REDISTSTRUCTMOB;
            string newTid = transId;
            if (transId.EndsWith("-design"))
            {
                transId = transId.Substring(0, transId.IndexOf("-"));
                fdKey = Constants.REDISTSTRUCTDESIGN;
                newTid = transId + "-design";
            }
            string fileName = "opentstruct-" + transId;
            string language = HttpContext.Current.Session["language"].ToString();


            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            logobj.CreateLog("Get Struct from Cache Manager. User: " + userName + " Role: " + AxRole, sessionID, fileName, "");
            strObj = (TStructDef)fObj.TstructDefFromRedis(util.GetRedisServerkey(fdKey, transId));
            if (strObj != null && !IsStructureUpdated(strObj, transId, sessionID, projectName, userName))
            {
                //TODO- Fastdata               
                Boolean designMode = false;
                if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
                    designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
                if (!designMode)
                {
                    if (strObj.IsObjCustomHtml)
                    {
                        string designHtmlKey = Constants.REDISTSTRUCTAXCUSTHTML;
                        string designCustHtmlJson = fObj.StringFromRedis(util.GetRedisServerkey(designHtmlKey, transId));
                        if (designCustHtmlJson == "")
                        {
                            if (strObj.axdesignJson == "")
                            {
                                string designKey = Constants.REDISTSTRUCTAXDESIGN;
                                string designJson = fObj.StringFromRedis(util.GetRedisServerkey(designKey, transId));
                                HttpContext.Current.Session["Axp_DesignJson"] = designJson;
                            }
                            else
                                HttpContext.Current.Session["Axp_DesignJson"] = strObj.axdesignJson;
                        }
                        else
                        {
                            HttpContext.Current.Session["IsCustomHtml"] = "true";
                            tstCustomHTML = designCustHtmlJson;
                        }
                    }
                    else
                    {
                        if (strObj.axdesignJson == "")
                        {
                            string designKey = Constants.REDISTSTRUCTAXDESIGN;
                            string designJson = fObj.StringFromRedis(util.GetRedisServerkey(designKey, transId));
                            HttpContext.Current.Session["Axp_DesignJson"] = designJson;
                        }
                        else
                            HttpContext.Current.Session["Axp_DesignJson"] = strObj.axdesignJson;
                    }
                }
                strObj.IsObjFromCache = true;
                logobj.CreateLog("Getting Structure from Redis " + userName + " Role: " + AxRole, sessionID, fileName, "");
            }
            else
            {
                strObj = DuplicateStructDef(newTid, AxRole, projectName, userName, sessionID, fileName, userName);
                if (strObj == null)
                {
                    strObj = GetStructDefFromDb(newTid, AxRole, projectName, userName, sessionID);
                    logobj.CreateLog("Getting Structure from DB " + userName + " Role: " + AxRole, sessionID, fileName, "");
                }
            }
            if (HttpContext.Current.Session["IsCustomHtml"] != null && HttpContext.Current.Session["IsCustomHtml"].ToString() == "true")
            {
                strObj.IsObjCustomHtml = true;
                HttpContext.Current.Session["IsCustomHtml"] = string.Empty;
            }
            return strObj;

        }

        /// <summary>
        /// Function to get the struct def object from the data base.
        /// </summary>
        /// <param name="TransId">Transaction Id</param>
        /// <param name="AxRole">Role</param>
        /// <param name="strObj">Struct def object</param>
        /// <returns>struct def object.</returns>
        public TStructDef GetStructDefFromDb(string transId, string AxRole, string proj, string axpUser, string sessionID)
        {

            string fdKey = Constants.REDISTSTRUCT;
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                fdKey = Constants.REDISTSTRUCTMOB;
            if (transId.EndsWith("-design"))
            {
                transId = transId.Substring(0, transId.IndexOf("-"));
                fdKey = Constants.REDISTSTRUCTDESIGN;
            }
            string res = string.Empty, fileName = "opentstruct-" + transId;
            string iXml = string.Empty;
            string language = HttpContext.Current.Session["language"].ToString();
            logobj.CreateLog("    Calling webservice to get Structure from DB", sessionID, fileName, "");
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            bool isDesignMode = false;
            if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
                isDesignMode = bool.Parse(HttpContext.Current.Session[transId + "IsDesignMode"].ToString());
            string axrulescached = CheckAxRulesCached(transId);
            string IsFromProcess = HttpContext.Current.Session["IsFromProcess"] != null ? HttpContext.Current.Session["IsFromProcess"].ToString() : "false";
            iXml = iXml + "<root fromprocess='" + IsFromProcess + "' axpapp='" + proj + "' axrulescached='" + axrulescached + "' sessionid='" + sessionID + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace='" + errorLog + "' axdesign='false' axdesigndefault='" + isDesignMode.ToString().ToLower() + "' ><sname>tstructs</sname><name>" + transId + "</name><caption></caption>";
            iXml = iXml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

            //Call service
            res = objWebServiceExt.CallGetStructureWS(transId, iXml);
            requestProcess_log = res.Split('♠')[0];
            res = res.Split('♠')[1];
            if (res.Contains(Constants.ERROR) == true)
            {
                res = res.Replace(Constants.ERROR, "");
                res = res.Replace("</error>", "");
                res = res.Replace("\n", "");
                throw (new Exception(requestProcess_log + "♠" + res));
            }
            else
            {
                //GetTstruct Custom details
                DataTable srchCols = new DataTable();
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(res);
                    string customDetails = string.Empty;
                    XmlNodeList customDetailsXML = xmlDoc.GetElementsByTagName("axpconfigs_" + transId);
                    if (customDetailsXML[0] != null)
                    {
                        customDetails = customDetailsXML[0].OuterXml;
                        res = res.Replace(customDetails, "");
                    }
                    srchCols = GetTstructCustoms(transId, customDetails);
                    XmlNodeList pdfListNode = xmlDoc.GetElementsByTagName(transId + "_pdflist");
                    if (pdfListNode[0] != null)
                    {
                        HttpContext.Current.Session[transId + "_pdflist"] = pdfListNode[0].OuterXml;
                        res = res.Replace(pdfListNode[0].OuterXml, "");
                    }

                    XmlNodeList AxRulesListNode = xmlDoc.GetElementsByTagName("axrulesdef");
                    if (AxRulesListNode[0] != null)
                    {
                        SetAxRulesDef(AxRulesListNode[0].OuterXml, transId);
                        res = res.Replace(AxRulesListNode[0].OuterXml, "");
                    }
                    else if (axrulescached != "T")
                    {
                        SetAxRulesDef("<axrulesdef></axrulesdef>", transId);
                    }

                    XmlNodeList AxRulesListNode_btn = xmlDoc.GetElementsByTagName("axrulesdef_btnmsgs");
                    if (AxRulesListNode_btn[0] != null)
                    {
                        SetAxRulesDef_Btn(AxRulesListNode_btn[0].OuterXml, transId);
                        res = res.Replace(AxRulesListNode_btn[0].OuterXml, "");
                    }


                    XmlNodeList AxvalErrorcode = xmlDoc.GetElementsByTagName("axvalerrorcode");
                    if (AxvalErrorcode[0] != null)
                    {
                        SetAxvalErrorCode(AxvalErrorcode[0].OuterXml, transId);
                        res = res.Replace(AxvalErrorcode[0].OuterXml, "");
                    }


                    XmlNodeList formglobalvars = xmlDoc.GetElementsByTagName("formglobalvars");
                    if (formglobalvars[0] != null)
                    {
                        SetTstHtmllsGlobalVars(formglobalvars[0].InnerXml, transId);
                        res = res.Replace(formglobalvars[0].OuterXml, "");
                    }
                    else
                    {
                        SetTstHtmllsGlobalVars("novars", transId);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                res = GetDesignConfigXML(res, transId, schemaName);

                DataSet dsRapidDef = new DataSet();

                //Since the Rapid is disabled temporarily, following 'GetRapidDefinition' function is commented out.
                //if (transId.ToLower() != "axglo" && (HttpContext.Current.Session["AxIsPerfCode"] != null && HttpContext.Current.Session["AxIsPerfCode"].ToString().ToLower() == "true"))
                //{
                //    dsRapidDef = GetRapidDefinition(transId); 
                //}

                TStructDef strObj = new TStructDef(res, srchCols, dsRapidDef);

                //dt_RapidDependents is holding very huge dataTable which is not useful after creating tstructdef object and throwing exception while saving to redis so clearing rapid related datatables before saving to redis
                strObj.dt_RapidDependents = null;
                strObj.dt_RapidDependentsTemp = null;
                strObj.dt_RapidFormControl = null;

                if (fdKey == Constants.REDISTSTRUCTDESIGN)
                {
                    strObj.IsDesignMode = true;
                }

                //DMS Integration
                try
                {
                    strObj = customobj.AxAfterGetStruct(res, transId, strObj);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                strObj.IsObjFromCache = false;
                strObj.structRes = res;
                IsTstObjNew = true;

                try
                {
                    string dcs = JsonConvert.SerializeObject(strObj.dcs);
                    string flds = JsonConvert.SerializeObject(strObj.flds);

                    JObject finalObject = new JObject();
                    finalObject["dcs"] = JArray.Parse(dcs);
                    finalObject["flds"] = JArray.Parse(flds);

                    string saveString = finalObject.ToString();

                    string fdListViewFieldsInfoKey = Constants.ListViewFieldsInfo;
                    if (!fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdListViewFieldsInfoKey, transId), saveString, fdListViewFieldsInfoKey, schemaName))
                    {
                        HttpContext.Current.Session[transId + "_" + fdListViewFieldsInfoKey] = saveString;
                    }
                }
                catch (Exception ex) { }



                //TODO: REDIS
                //To store primary DC recId while loading the tstruct using Iview
                //Can avoid GetRecordId service call from second time load tstruct with same recid 
                if (strObj.dcTable.Count > 0 && strObj.dcTable[0].ToString() != "")
                {
                    string fdKeydcTable = Constants.REDISTSTRUCTTABLE;
                    fdwObj.SaveInRedisServer(util.GetRedisServerkey(fdKeydcTable, transId), strObj.dcTable[0], Constants.REDISTSTRUCTTABLE, schemaName);
                }
                logobj.CreateLog("    Adding the structure to cache. Updated time-" + strObj.tstUpdateOn, sessionID, "GetCacheDetails", "");
                logobj.CreateLog("    Returning the structure.", sessionID, fileName, "");
                return strObj;
            }
        }

        public TStructDef DuplicateStructDef(string transId, string AxRole, string proj, string axpUser, string sessionID, string fileName, string userName)
        {
            string fdKey = Constants.REDISTSTRUCTMOB;
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                fdKey = Constants.REDISTSTRUCT;
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            TStructDef strObj;
            strObj = (TStructDef)fObj.TstructDefFromRedis(util.GetRedisServerkey(fdKey, transId));
            if (strObj != null)
            {
                string res = strObj.structRes;
                string customDetails = string.Empty;
                {
                    customDetails = "<axpconfigs_" + transId + "><configname>searchcols</configname><cvalue>" + strObj.srchCols + "</cvalue>";
                    customDetails += "<configname>groupbtns</configname><cvalue>" + strObj.grpActBtns + "</cvalue>";
                    customDetails += "<configname>Printform</configname><cvalue>" + strObj.htmlToPDF + "</cvalue>";
                    customDetails += "<configname>refreshselect</configname><cvalue>" + strObj.refreshSelect + "</cvalue></axpconfigs_" + transId + ">";
                }
                DataTable srchCols = new DataTable();
                srchCols = GetTstructCustoms(transId, customDetails);
                Boolean designMode = false;
                if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
                    designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
                if (!designMode)
                {

                    string designHtmlKey = Constants.REDISTSTRUCTAXCUSTHTML;
                    string designCustHtmlJson = fObj.StringFromRedis(util.GetRedisServerkey(designHtmlKey, transId));
                    if (designCustHtmlJson == "")
                    {
                        string designKey = Constants.REDISTSTRUCTAXDESIGN;
                        string designJson = fObj.StringFromRedis(util.GetRedisServerkey(designKey, transId));
                        HttpContext.Current.Session["Axp_DesignJson"] = designJson;
                    }
                    else
                    {
                        HttpContext.Current.Session["IsCustomHtml"] = "true";
                        tstCustomHTML = designCustHtmlJson;
                    }
                }
                DataSet dsRapidDef = new DataSet();
                TStructDef strObjNew = new TStructDef(res, srchCols, dsRapidDef);
                logobj.CreateLog("Getting Structure from Redis " + userName + " Role: " + AxRole, sessionID, fileName, "");
                return strObjNew;
            }
            else
                return strObj;
        }

        private string GetDesignConfigXML(string rXML, string transId, string schemaName)
        {
            string res = string.Empty;
            try
            {
                Boolean designMode = false;
                if (HttpContext.Current.Session[transId + "IsDesignMode"] != null && HttpContext.Current.Session[transId + "IsDesignMode"].ToString() != string.Empty)
                    designMode = Convert.ToBoolean(HttpContext.Current.Session[transId + "IsDesignMode"]);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rXML);
                XmlNodeList newdesign = xmlDoc.GetElementsByTagName("axnewdesign");
                if (newdesign[0] != null)
                {
                    if (!designMode)
                    {
                        string designKey = Constants.REDISTSTRUCTAXDESIGN;
                        HttpContext.Current.Session["Axp_DesignJson"] = newdesign[0].InnerXml.Replace("]</CONTENT>", "]").Replace("<CONTENT>[", "[").Replace("]</content>", "]").Replace("<content>[", "[");
                        fdwObj.SaveInRedisServer(util.GetRedisServerkey(designKey, transId), HttpContext.Current.Session["Axp_DesignJson"].ToString(), designKey, schemaName);
                    }
                    rXML = rXML.Replace(newdesign[0].OuterXml, "");
                }
                else
                {
                    XmlNodeList olddesign = xmlDoc.GetElementsByTagName("axolddesign");
                    if (olddesign[0] != null)
                    {
                        if (!designMode)
                            HttpContext.Current.Session["Axp_OldDesignJson"] = olddesign[0].InnerXml.Replace("]</CONTENT>", "]").Replace("<CONTENT>[", "[").Replace("]</content>", "]").Replace("<content>[", "[").Replace("</VALUE>", "").Replace("<VALUE>", "");
                        rXML = rXML.Replace(olddesign[0].OuterXml, "");
                    }
                }
                XmlNodeList customhtml = xmlDoc.GetElementsByTagName("customhtml");
                if (customhtml[0] != null)
                {
                    DataTable dtHtml = new DataTable();
                    DBContext objData = new DBContext();
                    string queryHtml = "Select names,cvalue from templates where iviewid = '" + transId + "'";
                    dtHtml = objData.GetDataTableInline(queryHtml);
                    if (dtHtml != null && dtHtml.Rows.Count > 0)
                    {
                        if (dtHtml.Rows[0]["cvalue"].ToString() != string.Empty)
                        {
                            HttpContext.Current.Session["IsCustomHtml"] = "true";
                            tstCustomHTML = dtHtml.Rows[0]["cvalue"].ToString();
                            tstCustomHTML = tstCustomHTML.Replace("*@*", "@");
                            string designHtmlKey = Constants.REDISTSTRUCTAXCUSTHTML;
                            fdwObj.SaveInRedisServer(util.GetRedisServerkey(designHtmlKey, transId), tstCustomHTML, designHtmlKey, schemaName);
                        }
                    }
                }

                XmlNodeList axconfig = xmlDoc.GetElementsByTagName("axconfig");
                if (axconfig[0] != null)
                {
                    string axpStructKey = Constants.AXCONFIGTSTRUCT;
                    string AxRole = HttpContext.Current.Session["AxRole"].ToString();
                    DataSet ds = new DataSet();
                    string ddddd = axconfig[0].OuterXml;
                    StringReader sr = new StringReader(ddddd);
                    XmlTextReader reader = new XmlTextReader(sr);
                    ds.ReadXml(reader);
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    HttpContext.Current.Session["AxDtConfigs"] = dt;
                    fdwObj.SaveInRedisServerDT(util.GetConfigCacheKey(axpStructKey, transId, "", AxRole, "ALL"), dt, axpStructKey, schemaName);
                    rXML = rXML.Replace(axconfig[0].OuterXml, "");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var lstMultiFlds = dt.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "multi select" && x.Field<string>("PROPSVAL").ToLower() == "true")
                         .Select(x => x.Field<string>("SFIELD")).ToList();
                        if (lstMultiFlds.Count > 0)
                            HttpContext.Current.Session["AxpFldMultiSelect"] = lstMultiFlds;
                        var lstMultiFldsp = dt.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "multi select" && x.Field<string>("PROPSVAL").ToLower() == "true")
                        .Select(x => x.Field<string>("SFIELD") + "♦" + x.Field<string>("PROPVALUE2")).ToList();
                        if (lstMultiFldsp.Count > 0)
                            HttpContext.Current.Session["AxpFldMultiSelSp"] = lstMultiFldsp;

                        var buttonstyle = dt.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "tstruct button style").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                        if (buttonstyle.Count > 0)
                            HttpContext.Current.Session["AxpTstButtonStyleDef"] = buttonstyle[0].val.ToLower();

                        var labelscreateddesktop = dt.AsEnumerable().Where(x => x.Field<string>("PROPS").ToLower() == "labels created in desktop hide in runtime").Select(x => new { val = x.Field<string>("PROPSVAL") }).ToList();
                        if (labelscreateddesktop.Count > 0)
                            HttpContext.Current.Session["AxpLblCreatedDesktop-" + transId] = labelscreateddesktop[0].val.ToLower();
                    }
                    else
                    {
                        HttpContext.Current.Session["AxpFldMultiSelect"] = null;
                        HttpContext.Current.Session["AxpFldMultiSelSp"] = null;
                        HttpContext.Current.Session.Remove("AxpTstButtonStyleDef");
                        HttpContext.Current.Session.Remove("AxpLblCreatedDesktop-" + transId);
                    }
                }
                res = rXML;
                return res;
            }
            catch (Exception ex)
            {
                return res = rXML;
            }
        }

        private DataSet GetRapidDefinition(string transId)
        {
            DataSet dsDef = new DataSet();

            try
            {
                DBContext objDB = new DBContext();
                dsDef = objDB.GetRapidDefData(Constants.GET_RAPID_DEF_SP, transId);
                logobj.CreateLog("    Data from Rapid definition.-" + dsDef, HttpContext.Current.Session.SessionID, "RapidDefinition-" + transId, "");
            }
            catch (Exception ex)
            {
                logobj.CreateLog("Exception in GetRapidDefinition-" + ex.Message, HttpContext.Current.Session.SessionID, "ExcRapidDefinition-" + transId, "new");
                //throw ex;
            }

            return dsDef;
        }

        private DataTable GetTstructCustoms(string transId, string cutResult)
        {
            string cutTable = "axpconfigs_" + transId;
            string srchCols = string.Empty;
            DataSet ds = new DataSet();
            if (cutResult != null || cutResult != "")
            {
                if (cutResult.Contains(Constants.ERROR) == true)
                {
                    return new DataTable();
                }
                if (cutResult != string.Empty)
                {
                    try
                    {
                        //More than 1 configuration cutResult XML is not loading as expected dataset so added below line. 
                        cutResult = "<root>" + cutResult.Replace("</CVALUE><CONFIGNAME>", "</CVALUE></" + cutTable + "><" + cutTable + "><CONFIGNAME>").Replace("</cvalue><configname>", "</cvalue></" + cutTable + "><" + cutTable + "><configname>") + "</root>";
                        StringReader sr = new StringReader(cutResult);
                        XmlTextReader reader = new XmlTextReader(sr);
                        ds.ReadXml(reader);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return ds.Tables[cutTable];
            }
            else
            {
                return new DataTable();
            }

            #region Before concurrent user 
            //string result = string.Empty;
            //string srchCols = string.Empty;
            //DataSet ds = new DataSet();
            //ASBCustom.CustomWebservice objWeb = new ASBCustom.CustomWebservice();
            //try
            //{
            //    string sqlStr = Constants.GET_TST_CUSTOM_ACTIONS.Replace(Constants.VAR_TRANSID, transId);
            //    result = objWeb.GetChoices(transId, sqlStr);
            //}
            //catch (Exception ex)
            //{
            //    logobj.CreateLog("Exception in GetTstructCustoms-" + ex.Message, HttpContext.Current.Session.SessionID, "GetTstructCustoms", "");
            //    throw ex;
            //}

            //if (result.Contains(Constants.ERROR) == true)
            //{
            //    return new DataTable();
            //}
            //if (result != string.Empty)
            //{
            //    try
            //    {
            //        StringReader sr = new StringReader(result);
            //        XmlTextReader reader = new XmlTextReader(sr);
            //        ds.ReadXml(reader);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            //return ds.Tables["row"];
            #endregion
        }

        /// <summary>
        /// Function to check if the stucture definition was changed in the database by comparing the updatedOn and the updated date stored in the struct def object.
        /// </summary>
        /// <param name="strObj">Struct def object</param>
        /// <param name="TransId">Transaction Id</param>
        /// <returns>True if the structure was updated else false.</returns>
        public Boolean IsStructureUpdated(TStructDef strObj, string transId, string sessionID, string proj, string axpUser)
        {
            string isDevInstance = string.Empty;
            if (HttpContext.Current.Session["AxDevInstance"] != null)
                isDevInstance = HttpContext.Current.Session["AxDevInstance"].ToString();
            if (isDevInstance == "true")
            {
                System.Globalization.CultureInfo culture = default(System.Globalization.CultureInfo);
                culture = new System.Globalization.CultureInfo("en-GB");

                string dbTimeSql = string.Empty, dbTimeRes = string.Empty, fileName = string.Empty;
                ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
                if (transId.ToLower().Equals("axglo"))
                    dbTimeSql = "<sqlresultset axpapp='" + proj + "' sessionid='" + sessionID + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>select updatedon upd from tstructs where lower(name) ='" + transId.ToLower() + "' and blobno=1</sql>";
                else
                    dbTimeSql = "<sqlresultset axpapp='" + proj + "' sessionid='" + sessionID + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>select updatedon upd from tstructs where name ='" + transId + "' and blobno=1</sql>";

                dbTimeSql += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                fileName = "opentstruct-" + transId;
                logobj.CreateLog("Calling DB to check structure is updated.", sessionID, fileName, "");
                //Call service
                dbTimeRes = objWebServiceExt.CallGetChoiceWS(transId, dbTimeSql);

                if (dbTimeRes.Contains(Constants.ERROR) == true)
                {
                    dbTimeRes = dbTimeRes.Replace(Constants.ERROR, "");
                    dbTimeRes = dbTimeRes.Replace("</error>", "");
                    dbTimeRes = dbTimeRes.Replace("\n", "");
                    throw (new Exception(dbTimeRes));
                }
                else
                {
                    string dbResult = dbTimeRes.ToUpper();
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNode dbUpdatedtime = null;
                    xmlDoc.LoadXml(dbResult);
                    dbUpdatedtime = xmlDoc.SelectSingleNode("//RESPONSE/ROW/UPD");

                    string dbUpdDate = dbUpdatedtime.InnerText;
                    DateTime dbDate;
                    if (HttpContext.Current.Session["axdb"] != null)
                    {
                        if (HttpContext.Current.Session["axdb"].ToString().ToLower() == "ms sql")
                            culture = new System.Globalization.CultureInfo("en-US");
                    }


                    try
                    {
                        dbDate = DateTime.Parse(dbUpdDate, culture);
                    }
                    catch (System.FormatException ex)
                    {

                        dbDate = DateTime.Parse(dbUpdDate);
                    }

                    DateTime cacheTime;
                    string objTime = strObj.tstUpdateOn;
                    if (object.ReferenceEquals(objTime, ""))
                        objTime = "01/01/2000 00:00:01 AM";

                    try
                    {
                        cacheTime = DateTime.Parse(objTime, culture);
                    }
                    catch (System.FormatException ex)
                    {
                        cacheTime = DateTime.Parse(objTime);
                    }

                    logobj.CreateLog("    Cache time is " + cacheTime + ", DB time is " + dbDate + ", transid-" + transId, sessionID, "GetCacheDetails", "");
                    return dbDate > cacheTime;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Function to get the Structure HTML from cache.
        /// </summary>
        /// <param name="transId">Transaction Id</param>
        /// <param name="AxRole">Role</param>
        public void GetStructureHTML(string transId, string AxRole, string sessionID, string language)
        {
            string cachName = string.Empty, fileName = string.Empty;
            string fdKey = Constants.REDISTSTRUCTDTLS;
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                fdKey = Constants.REDISTSTRUCTDTLSMOB;
            if (transId.EndsWith("-design"))
            {
                transId = transId.Substring(0, transId.IndexOf("-"));
                fdKey = Constants.REDISTSTRUCTDESIGNDTLS;
            }
            fileName = "opentstruct-" + transId;
            logobj.CreateLog("    Getting structure from Cache.", sessionID, fileName, "");
            string proj = HttpContext.Current.Session["project"].ToString();
            string cacheKey = (util.GetRedisServerkey(fdKey, transId));
            FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
            DataTable dt = fObj.DTasJsonFromRedis(cacheKey);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    structHTML = dt.Rows[0][Constants.structHTML].ToString();
                    tstHeaderHTML = dt.Rows[0][Constants.tstHeaderHTML].ToString();
                    toolbarBtnIcons = dt.Rows[0][Constants.toolbarBtnIcons].ToString();
                    modernToolbarBtnOpen = dt.Rows[0][Constants.modernToolbarBtnOpen].ToString();
                    attachHTML = dt.Rows[0][Constants.attachHTML].ToString();
                    taskBtns = dt.Rows[0][Constants.taskBtns].ToString();
                    structScript = dt.Rows[0][Constants.structScript].ToString();
                    structBtns = dt.Rows[0][Constants.structBtns].ToString();
                    structCaption = dt.Rows[0][Constants.structCaption].ToString();
                    strSubmitCancel = dt.Rows[0][Constants.strSubmitCancel].ToString();
                    axdesignJObcmg = getDesignObjectFromJson(dt.Rows[0][Constants.strDesign].ToString());
                    if (axdesignJObcmg == null)
                    {
                        axdesignJObcmg = new DesignObject();
                        axdesignJObcmg.newDesign = true;
                        //isFreshDesign = true;
                    }
                }
            }
        }



        public DesignObject getDesignObjectFromJson(string axdesignJson)
        {
            DesignObject axdesignJObject = new DesignObject();
            try
            {

                axdesignJObject = JsonConvert.DeserializeObject<DesignObject>(axdesignJson.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' }));
                if (axdesignJObject.transid == null)
                {
                    axdesignJObject.newDesign = true;
                }
            }
            catch (Exception ex)
            {

            }
            return axdesignJObject;
        }

        /// <summary>
        /// Function to store the Structure HTML into cache.
        /// </summary>
        /// <param name="transId">Transaction Id</param>
        /// <param name="AxRole">Role</param>
        public void SetStructureHTML(string transId, string AxRole, string language)
        {

            string fdKey = Constants.REDISTSTRUCTDTLS;
            if (HttpContext.Current.Session["MobileView"] != null && HttpContext.Current.Session["MobileView"].ToString() == "True")
                fdKey = Constants.REDISTSTRUCTDTLSMOB;
            if (transId.EndsWith("-design"))
            {
                transId = transId.Substring(0, transId.IndexOf("-"));
                fdKey = Constants.REDISTSTRUCTDESIGNDTLS;
            }
            string proj = HttpContext.Current.Session["project"].ToString();
            string cacheKey = (util.GetRedisServerkey(fdKey, transId));
            string cachName = string.Empty;

            DataTable dt = new DataTable();
            dt.Columns.Add(Constants.structHTML);
            dt.Columns.Add(Constants.tstHeaderHTML);
            dt.Columns.Add(Constants.toolbarBtnIcons);
            dt.Columns.Add(Constants.modernToolbarBtnOpen);
            dt.Columns.Add(Constants.attachHTML);
            dt.Columns.Add(Constants.taskBtns);
            dt.Columns.Add(Constants.structScript);
            dt.Columns.Add(Constants.structBtns);
            dt.Columns.Add(Constants.structCaption);
            dt.Columns.Add(Constants.strSubmitCancel);
            dt.Columns.Add(Constants.strDesign);

            DataRow dr = dt.NewRow();
            dr[Constants.structHTML] = structHTML;
            dr[Constants.tstHeaderHTML] = tstHeaderHTML;
            dr[Constants.toolbarBtnIcons] = toolbarBtnIcons;
            dr[Constants.modernToolbarBtnOpen] = modernToolbarBtnOpen;
            dr[Constants.attachHTML] = attachHTML;
            dr[Constants.taskBtns] = taskBtns;
            dr[Constants.structScript] = structScript;
            dr[Constants.structBtns] = structBtns;
            dr[Constants.structCaption] = structCaption;
            dr[Constants.strSubmitCancel] = strSubmitCancel;
            dr[Constants.strDesign] = Newtonsoft.Json.JsonConvert.SerializeObject(axdesignJObcmg);
            dt.Rows.Add(dr);

            string schemaName = string.Empty;
            if (HttpContext.Current.Session["dbuser"] != null)
                schemaName = HttpContext.Current.Session["dbuser"].ToString();
            try
            {
                bool isSaved = fdwObj.SaveInRedisServerDT(cacheKey, dt, Constants.REDISTSTRUCT, schemaName);
                if (isSaved == false)
                {
                    HttpContext.Current.Session["StructureHtml"] = structHTML;
                    HttpContext.Current.Session["ToolbarBtnIcons"] = toolbarBtnIcons;
                    HttpContext.Current.Session["ModernToolbarBtnOpen"] = modernToolbarBtnOpen;
                }
            }
            catch (Exception)
            {
                HttpContext.Current.Session["StructureHtml"] = structHTML;
                HttpContext.Current.Session["ToolbarBtnIcons"] = toolbarBtnIcons;
                HttpContext.Current.Session["ModernToolbarBtnOpen"] = modernToolbarBtnOpen;
            }
        }

        private void SetAxRulesDef(string ruleDef, string transId)
        {
            try
            {
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                string conRule = Constants.AXRULESDEF;
                fdwObj.SaveInRedisServer(util.GetRedisServerkey(conRule, transId), ruleDef, Constants.AXRULESDEF, schemaName);
            }
            catch (Exception ex)
            { }
        }

        private string CheckAxRulesCached(string transId)
        {
            string isCached = "F";
            try
            {
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                string strAxRules = string.Empty;
                string conRule = Constants.AXRULESDEF;
                strAxRules = fObj.StringFromRedis(util.GetRedisServerkey(conRule, transId), schemaName);
                if (strAxRules == string.Empty || strAxRules == "")
                    isCached = "F";
                else if (strAxRules == "<axrulesdef></axrulesdef>")
                    isCached = "N";
                else
                    isCached = "T";
            }
            catch (Exception ex)
            { }
            return isCached;
        }

        private void SetAxvalErrorCode(string axValErrCode, string transId)
        {
            try
            {
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                string conErrCode = Constants.AXVALERRORCODE;
                fdwObj.SaveInRedisServer(util.GetRedisServerkey(conErrCode, transId), axValErrCode, Constants.AXVALERRORCODE, schemaName);
            }
            catch (Exception ex)
            { }
        }

        private void SetAxRulesDef_Btn(string ruleDefBtn, string transId)
        {
            try
            {
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                string conRuleBtn = Constants.AXRULESDEFBTNCMSG;
                fdwObj.SaveInRedisServer(util.GetRedisServerkey(conRuleBtn, transId), ruleDefBtn, Constants.AXRULESDEFBTNCMSG, schemaName);
            }
            catch (Exception ex)
            { }
        }

        private void SetTstHtmllsGlobalVars(string _thisFormGlobalVars, string transId)
        {
            try
            {
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                string conRuleBtn = Constants.REDISTSTHTMLLSGLOBALVARS;
                bool isSaved = fdwObj.SaveInRedisServer(util.GetRedisServerkey(conRuleBtn, transId), _thisFormGlobalVars, Constants.REDISTSTHTMLLSGLOBALVARS, schemaName);
                if (!isSaved)
                    HttpContext.Current.Session["tstHtmlLsGlbVars-" + transId] = _thisFormGlobalVars;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["tstHtmlLsGlbVars-" + transId] = _thisFormGlobalVars;
            }
        }
    }
}
