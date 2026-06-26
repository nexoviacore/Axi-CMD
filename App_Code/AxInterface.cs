using System.Collections.Generic;
using System.Web;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Data;

namespace AxInterface
{
    public class tstAttachment
    {
        public string fieldName { get; set; }
        public string fileName { get; set; }
        public string attachment { get; set; }
    }

    public class AxpertInterSaveData
    {
        public StringBuilder sb = new StringBuilder();
        DataSet objDs = new DataSet();
        public bool hasAttachments { get; set; }
        public List<tstAttachment> attachments = new List<tstAttachment>();
        public string transID = string.Empty;
        public string recordId = "0";
        LogFile.Log logobj = new LogFile.Log();
        Util.Util util = new Util.Util();

        public string GenerateXMLStrucuture()
        {
            string errorLog = logobj.CreateLog("Save data from AxInterface page.", HttpContext.Current.Session["nsessionid"].ToString(), "Savedata-" + transID, "new");
            string properties = "<Transaction axpapp=\"" + HttpContext.Current.Session["project"].ToString() + "\" afiles=\"\" trace=\"" + errorLog + "\" sessionid=\"" + HttpContext.Current.Session["nsessionid"].ToString() + "\" appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>{♦♠♣♥}" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</Transaction>";
            sb.Append("<data transid=\"" + transID + "\" recordid=\""+ recordId + "\">");

            foreach (var dts in objDs.Tables)
            {
                DataTable dt = (DataTable)dts;
                string dcNumber = dt.TableName;
                dcNumber = dcNumber.Replace("dc", "");
                int row = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string rowNumber = row.ToString().Length == 1 ? "00" + row : row.ToString().Length == 2 ? "0" + row : row.ToString();
                    sb.Append("<axp_recid" + dcNumber + " rowno=\"" + rowNumber + "\">"+ recordId + "</axp_recid" + dcNumber + ">");
                    foreach (DataColumn cols in dt.Columns)
                    {
                        sb.Append("<" + cols.ColumnName + " rowno=\"" + rowNumber + "\">" + util.CheckSpecialChars(dt.Rows[i][cols.ColumnName].ToString().Replace("^^dq", "\"").Replace(";bkslh", @"\")) + "</" + cols.ColumnName + ">");
                    }
                    row++;
                }
            }
            sb.Append("</data>");
            //return CallSaveDataWS(transID, string.Format(properties, sb.ToString()));
            return CallSaveDataWS(transID, properties.Replace("{♦♠♣♥}", sb.ToString()));
        }

        private string CallSaveDataWS(string transId, string saveDataInput)
        {
            string json = string.Empty;
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            json = objWebServiceExt.CallSaveDataWS(transId, saveDataInput, "");
            //requestProcess_logtime += loadRes.Split('♠')[0];
            json = json.Split('♠')[1];
            return json;
        }

        private void CreateAttachmentGridDCXML(string dcNumber, JArray value)
        {
            int row = 1;
            foreach (JObject item in value)
            {
                sb.Append("<axp_recid" + dcNumber + " rowno=\"00" + row + "\">0</axp_recid" + dcNumber + ">");

                var field = item.Properties().FirstOrDefault(p => p.Name.Contains("axp_gridattach"));
                var documentName = item.Properties().FirstOrDefault(p => p.Name.Equals("docname"));
                var fileName = item.Properties().FirstOrDefault(p => p.Name.Equals("filename"));
                var isMandatory = item.Properties().FirstOrDefault(p => p.Name.Equals("manitoryflag"));

                sb.Append("<" + field.Name + " rowno=\"00" + row + "\">" + fileName.Value.ToString() + "</" + field.Name + ">");
                sb.Append("<" + documentName.Name + " rowno=\"00" + row + "\">" + documentName.Value.ToString() + "</" + documentName.Name + ">");
                sb.Append("<" + fileName.Name + " rowno=\"00" + row + "\">" + fileName.Value.ToString() + "</" + fileName.Name + ">");
                sb.Append("<" + isMandatory.Name + " rowno=\"00" + row + "\">" + isMandatory.Value.ToString() + "</" + isMandatory.Name + ">");

                attachments.Add(new tstAttachment
                {
                    attachment = field.Value.ToString(),
                    fieldName = field.Name,
                    fileName = fileName.Value.ToString()
                });
                row++;
            }
        }

        public AxpertInterSaveData(string transId, DataSet ds, string RecordId)
        {
            transID = transId;
            objDs = ds;
            recordId = RecordId;
        }
    }

    public class AxpertInterGetCustomSql
    {
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        LogFile.Log logobj = new LogFile.Log();
        Util.Util util = new Util.Util();
        static string sqlnames = string.Empty;
        static ArrayList sqlparams = new ArrayList();

        public JObject GetCustomSqlData()
        {
            JObject getwidgetdetails = new JObject();
            string errorLog = logobj.CreateLog("Get Custom Sql Data", HttpContext.Current.Session["nsessionid"].ToString(), "GetCustomSqlData", "new");
            try
            {
                string result = string.Empty;

                string getPageInfo = string.Empty;
                getPageInfo = " sqlnames='" + sqlnames + "' ";
                string inputXml = "<sqlresultset " + getPageInfo + " axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                inputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                result = asbExt.CallGetCustomSqlWS(inputXml);
                if (result != string.Empty)
                {
                    dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(result);
                    if(jsonObj.error == null) { 
                        getwidgetdetails = JObject.Parse(jsonObj["getwidgetdetails"].ToString());
                    }else
                    {
                        getwidgetdetails["error"] = JObject.Parse(result);
                    }
                }
            }
            catch (Exception ex)
            {
                string sessID = Constants.GeneralLog;
                if (HttpContext.Current.Session != null)
                    sessID = HttpContext.Current.Session.SessionID;
                logobj.CreateLog("GetCustomSqlData exception -" + ex.Message, sessID, "GetCustomSqlData-exception", "new");
            }
            return getwidgetdetails;
        }

        public AxpertInterGetCustomSql(string sqlNames, ArrayList sqlParams)
        {
            sqlnames = sqlNames;
            sqlparams = sqlParams;
        }
    }

    public class AxpertInterGetWidget
    {
        LogFile.Log logobj = new LogFile.Log();
        static string widgetid = string.Empty;
        public string GetWidgetData()
        {
            string wgData = string.Empty;
            string sqlInput = "SELECT WIDGET_ID,WIDGET_TYPE,SUB_TYPE,TITLE,SQLTEXT,TABLETEXT,ATTRIBUTES,CHARTTYPE from AX_WIDGET where widget_id=" + widgetid;
            string errorLog = logobj.CreateLog("Get Wideget from AxInterface page.", HttpContext.Current.Session["nsessionid"].ToString(), "GetWideget-GetChoice", "new");
            string iXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlInput + "</sql>";
            iXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            wgData = objWebServiceExt.CallGetChoiceWS("GetWidget", iXML);
            if (wgData != string.Empty)
            {
                wgData = GetWidgetQueryData(wgData);
            }
            return wgData;
        }

        private string GetWidgetQueryData(string wgData)
        {
            string wgResult = string.Empty;
            string qdata = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(wgData.ToString());
            XmlNode node = doc.SelectSingleNode("sqlresultset/response/row/SQLTEXT");
            if (node.InnerText != "")
            {
                string sqlQuery = node.InnerText.TrimStart();
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
                sqlQuery = CheckSpecialChars(sqlQuery);
                string errorLog = logobj.CreateLog("Get Wideget Query from AxInterface page.", HttpContext.Current.Session["nsessionid"].ToString(), "GEtWidgetQuery-GetChoice", "new");
                string iXML = "<sqlresultset axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><sql>" + sqlQuery + "</sql>";
                iXML += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
                ASBExt.WebServiceExt objWebServiceExtNew = new ASBExt.WebServiceExt();
                qdata = objWebServiceExtNew.CallGetChoiceWS("GetWidgetQuery", iXML);
                XmlDocument docQuery = new XmlDocument();
                docQuery.LoadXml(qdata.ToString());
                XmlNodeList queryResponse = docQuery.SelectNodes("sqlresultset");
                XmlDocument queryDocs = new XmlDocument();
                queryDocs.LoadXml(queryResponse[0].InnerXml);
                qdata = JsonConvert.SerializeXmlNode(queryDocs);
            }
            wgResult = "{data: " + wgResult + ", queryData: " + qdata + "}";
            return wgResult;
        }

        private string CheckSpecialChars(string str)
        {
            str = Regex.Replace(str, "&amp;", "&");
            str = Regex.Replace(str, "&quot;", "“");
            //str = Regex.Replace(str, "\n", "<br>");
            str = Regex.Replace(str, "&", "&amp;");
            str = Regex.Replace(str, "<", "&lt;");
            str = Regex.Replace(str, ">", "&gt;");
            str = Regex.Replace(str, "'", "&apos;");
            str = Regex.Replace(str, "\"", "&quot;");
            str = Regex.Replace(str, "’", "&apos;");
            str = Regex.Replace(str, "“", "&quot;");
            str = Regex.Replace(str, "”", "&quot;");
            str = Regex.Replace(str, "™", "&#8482;");
            str = Regex.Replace(str, "®", "&#174;");
            str = str.Replace((char)160, ' ');
            string defaultDateStr = "dd/mm/yyyy";
            if (str == defaultDateStr)
                str = "";
            return str;
        }

        public AxpertInterGetWidget(string widgetId)
        {
            widgetid = widgetId;
        }
    }

    public class AxpertInterGetIview
    {
        LogFile.Log logobj = new LogFile.Log();
        static string ivname = string.Empty;
        DataTable ivparams = new DataTable();
        Util.Util util = new Util.Util();

        public string GetIviewData()
        {
            string ivData = string.Empty;
            string pXml = string.Empty;
            string recsperpage = "0";
            string pageno = "0";

            for (int i = 0; i < ivparams.Rows.Count; i++)
            {
                for (int j = 0; j < ivparams.Columns.Count; j++)
                {
                    string nodeName = ivparams.Columns[j].ColumnName;
                    string nodeVal = ivparams.Rows[i][j].ToString();
                    pXml = pXml + "<" + nodeName + ">";
                    pXml = pXml + util.CheckSpecialChars(nodeVal.ToString().Replace("^^dq", "\"").Replace(";bkslh", @"\"));
                    pXml = pXml + "</" + nodeName + ">";
                }
            }
            string errorLog = logobj.CreateLog("Get Iview Data from AxInterface page.", HttpContext.Current.Session["nsessionid"].ToString(), "GetIview-" + ivname, "new");
            string iXml = "<root name ='" + ivname + "' axpapp = '" + HttpContext.Current.Session["project"].ToString() + "' sessionid = '" + HttpContext.Current.Session["nsessionid"].ToString() + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' trace = '" + errorLog + "' pageno='" + pageno + "' pagesize='" + recsperpage.ToString() + "' firsttime='" + (int.Parse(pageno) < 2 ? "yes" : "no") + "' sqlpagination='false' getrowcount='false' gettotalrows='false' smartview='true'><params>" + pXml + "</params>";
            iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root> ";
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            IviewData objIview = new IviewData();
            ivData = objWebServiceExt.CallGetIViewWS(ivname, iXml, "", objIview);
            ivData = ivData.Split('♠')[1];
            return ivData;
        }

        public AxpertInterGetIview(string ivName, DataTable ivParams)
        {
            ivname = ivName;
            ivparams = ivParams;
        }
    }

    public class AxpertInterGetIviewParams
    {
        LogFile.Log logobj = new LogFile.Log();
        static string ivname = string.Empty;

        public string GetIviewParamData()
        {
            Util.Util util = new Util.Util();
            string paramData = string.Empty;
            string errorLog = logobj.CreateLog("Get Iview Data from AxInterface page.", HttpContext.Current.Session["nsessionid"].ToString(), "GetIview-" + ivname, "new");
            string iXml = "<root name =\"" + ivname + "\" axpapp = \"" + HttpContext.Current.Session["project"].ToString() + "\" sessionid = \"" + HttpContext.Current.Session["nsessionid"].ToString() + "\" appsessionkey=\"" + HttpContext.Current.Session["AppSessionKey"].ToString() + "\" username=\"" + HttpContext.Current.Session["username"].ToString() + "\" trace = \"" + errorLog + "\" firsttime='false'>";
            iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root> ";
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            IviewData objIview = new IviewData();
            string ivResult = objWebServiceExt.CallGetParamsWS(ivname, iXml, objIview);
            if (ivResult != string.Empty)
            {
                ivResult = ivResult.Split('♠')[1];
                if (ivResult.StartsWith("<"))
                    ivResult = util.ReplaceFirstOccurrence(ivResult, "#$#", "#$♥#");
                string[] splitRes = ivResult.Split(new[] { "#$♥#" }, StringSplitOptions.None);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(splitRes[0].ToString());
                //paramData = JsonConvert.SerializeXmlNode(doc);
                paramData = "{\"params\":[";
                string jsonNode = string.Empty;
                foreach (XmlNode node in doc.SelectSingleNode("root").ChildNodes)
                {
                    jsonNode += "{\"" + node.Name + "\":\"" + node.Attributes["value"].InnerText + "\"},";
                }
                if (jsonNode != string.Empty)
                    jsonNode = jsonNode.Remove(jsonNode.Length - 1, 1);
                paramData += jsonNode;
                paramData += "]}";
            }
            return paramData;
        }

        public AxpertInterGetIviewParams(string ivName)
        {
            ivname = ivName;
        }
    }

    public class AxpertInterGlobalVar
    {
        public string GetGlobalVars()
        {
            string json = string.Empty;
            json = "{\"globalVars\":[";
            if (HttpContext.Current.Session["axGlobalVars"] != null)
            {
                string jsonNode = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(HttpContext.Current.Session["axGlobalVars"].ToString());
                foreach (XmlNode node in xmlDoc.SelectSingleNode("globalvars").ChildNodes)
                {
                    if (node.Name.ToLower() != "responsibilies" && node.Name.ToLower() != "rolename" && node.Name.ToLower() != "sesid" && node.Name.ToLower() != "usergroup" && node.Name.ToLower() != "project" && node.Name.ToLower() != "groupno" && node.Name.ToLower() != "userroles" && node.Name.ToLower() != "pageaccess" && node.Name.ToLower() != "transidlist" && node.Name.ToLower() != "appvartypes" && node.Name.ToLower() != "axpdbdirpath")
                    {
                        string NodeVal = node.InnerText.Replace(@"\", "\\\\");
                        jsonNode += "{\"" + node.Name + "\":\"" + NodeVal + "\"},";
                    }
                }
                if (jsonNode != string.Empty)
                    jsonNode = jsonNode.Remove(jsonNode.Length - 1, 1);
                json += jsonNode;
            }
            json += "]}";
            return json;
        }
    }

    public class AxpertInterLoadData
    {
        LogFile.Log logobj = new LogFile.Log();
        string transId = string.Empty;
        string recId = string.Empty;

        public string LoadData()
        {
            string json = string.Empty;
            string InputXml = "";
            InputXml = InputXml + "<root ispegedit='false' imagefromdb='false' axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session.SessionID + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' transid='" + transId + "' recordid='" + recId + "' dcname='' trace=''>";
            InputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
            InputXml += "</root>";
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            json = objWebServiceExt.CallLoadDataWS(transId, InputXml, "", "", "");
            if (json != string.Empty)
            {
                json = json.Split('♠')[1];
                string[] jsonSplit = json.Split(new[] { "*$*" }, StringSplitOptions.None);
                json = jsonSplit[1];

                // used logic for reference from LoadStructure method in tstruct.aspx.cs
                json = json.Trim();
                json = json.Replace("\\n", "");
                json = json.Replace("\\", "\\\\");
            }
            return json;
        }

        public AxpertInterLoadData(string TransId, string RecordId)
        {
            transId = TransId;
            recId = RecordId;
        }
    }

    public class AxpertInterDoFormLoad
    {
        LogFile.Log logobj = new LogFile.Log();
        string transId = string.Empty;
        ArrayList variables = new ArrayList();
        Util.Util util = new Util.Util();

        public string DoFormLoad()
        {
            string json = string.Empty;

            string pXml = string.Empty;
            foreach (var parVal in variables)
            {
                string nodeName = parVal.ToString().Split('=')[0];
                string nodeVal = parVal.ToString().Split('=')[1];
                pXml = pXml + "<" + nodeName + ">";
                pXml = pXml + util.CheckSpecialChars(nodeVal.ToString().Replace("^^dq", "\"").Replace(";bkslh", @"\"));
                pXml = pXml + "</" + nodeName + ">";
            }

            string InputXml = "";
            InputXml = InputXml + "<root act='load' imagefromdb='false' axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session.SessionID + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' transid='" + transId + "' recordid='0' dcname='' trace=''>";
            InputXml += pXml;
            InputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
            InputXml += "</root>";
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            json = objWebServiceExt.CallDoFormLoadWS(transId, InputXml, "");
            if (json != string.Empty)
            {
                json = json.Split('♠')[1];
                string[] jsonSplit = json.Split(new[] { "*$*" }, StringSplitOptions.None);
                json = jsonSplit[1];

                // used logic for reference from LoadStructure method in tstruct.aspx.cs
                json = json.Trim();
                json = json.Replace("\\n", "");
                json = json.Replace("\\", "\\\\");
            }
            return json;
        }

        public AxpertInterDoFormLoad(string TransId, ArrayList Variables)
        {
            transId = TransId;
            variables = Variables;
        }
    }

    public class AxpertInterAction
    {
        LogFile.Log logobj = new LogFile.Log();
        string Actionname = string.Empty;
        string Actiontype = string.Empty;
        bool isScript = false;
        string Sname = string.Empty;
        string IvSelectedRows = string.Empty;
        DataSet dset = new DataSet();
        Util.Util util = new Util.Util();

        public string Action()
        {
            string json = string.Empty;

            string InputXml = "";
            if (Actiontype == "tstruct")
            {
                InputXml = "<root imagefromdb='false' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' scriptpath='" + HttpContext.Current.Application["ScriptsPath"].ToString() + "' axpapp='" + HttpContext.Current.Session["project"].ToString() + "' trace='' recordid='0' fno='0' afiles='' dcname='' actrow='0' sessionid='" + HttpContext.Current.Session.SessionID + "' stype='tstructs' sname='" + Sname + "' actname='" + Actionname + "' __file='' options='true' dsignstatus=''>";
                InputXml += "<varlist>";
                foreach (var dts in dset.Tables)
                {
                    DataTable dt = (DataTable)dts;
                    string dcNumber = dt.TableName;
                    dcNumber = dcNumber.Replace("dc", "");
                    int row = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string rowNumber = row.ToString().Length == 1 ? "00" + row : row.ToString().Length == 2 ? "0" + row : row.ToString();
                        InputXml += "<row>";
                        foreach (DataColumn cols in dt.Columns)
                        {
                            InputXml += "<" + cols.ColumnName + " rowno=\"" + rowNumber + "\">" + util.CheckSpecialChars(dt.Rows[i][cols.ColumnName].ToString().Replace("^^dq", "\"").Replace(";bkslh", @"\")) + "</" + cols.ColumnName + ">";
                        }
                        InputXml += "</row>";
                        row++;
                    }
                }
                InputXml += "</varlist>";
            }
            else
            {
                InputXml = "<root scriptpath='" + HttpContext.Current.Application["ScriptsPath"].ToString() + "' axpapp='" + HttpContext.Current.Session["project"].ToString() + "' trace='' sessionid='" + HttpContext.Current.Session.SessionID + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' stype='iviews' sname='" + Sname + "' actname='" + Actionname + "' username='" + HttpContext.Current.Session["username"].ToString() + "'>";
                InputXml += "<params>";
                foreach (var dts in dset.Tables)
                {
                    DataTable dt = (DataTable)dts;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        InputXml += "<row>";
                        foreach (DataColumn cols in dt.Columns)
                        {
                            InputXml += "<" + cols.ColumnName + ">" + util.CheckSpecialChars(dt.Rows[i][cols.ColumnName].ToString().Replace("^^dq", "\"").Replace(";bkslh", @"\")) + "</" + cols.ColumnName + ">";
                        }
                        InputXml += "</row>";
                    }
                }
                InputXml += "</params>";
                InputXml += "<varlist>" + IvSelectedRows + "</varlist>";
            }
            InputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString();
            InputXml += "</root>";
            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();

            if (isScript)
            {
                json = objWebServiceExt.callRemoteDoScriptWS("", InputXml, "", 100000);
            }
            else
            {
                json = objWebServiceExt.CallRemoteDoActionWS("", InputXml, "", 100000);
            }
            
            json = json.Split('♠')[1];
            return json;
        }

        public AxpertInterAction(string ActionName, string ActionType, string SName, bool isscript, DataSet ds, string ivSelectedRows)
        {
            Actionname = ActionName;
            Actiontype = ActionType;
            isScript = isscript;
            Sname = SName;
            dset = ds;
            IvSelectedRows = ivSelectedRows;
        }
    }

    public class AxpertInterDeleteRecord
    {
        public string transId = string.Empty;
        public string recordId = string.Empty;
        LogFile.Log logobj = new LogFile.Log();
        Util.Util util = new Util.Util();

        public AxpertInterDeleteRecord(string TransId, string RecordId)
        {
            transId = TransId;
            recordId = RecordId;
        }

        public string GenerateDeleteInputXML()
        {
            string errorLog = logobj.CreateLog("Delete Record from AxInterface page.", HttpContext.Current.Session["nsessionid"].ToString(), "DeleteRecord-" + transId, "new");

            string properties = "<Transaction axpapp='" + HttpContext.Current.Session["project"].ToString() + "' trace='" + errorLog + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' transid='" + transId + "' recordid='" + recordId + "' action='delete' allowCancel='true' >" + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</Transaction>";

            return properties;
        }

        public string CallDeleteDataWS()
        {
            string json = string.Empty;

            string deleteXML = GenerateDeleteInputXML();            

            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            json = objWebServiceExt.CallDeleteDataWS(transId, deleteXML);
            return json;
        }
    }

    public class AxpertInterGetRecord
    {
        public string transId = string.Empty;
        public ArrayList datas = new ArrayList();
        LogFile.Log logobj = new LogFile.Log();
        Util.Util util = new Util.Util();

        public AxpertInterGetRecord(string TransId, ArrayList Datas)
        {
            transId = TransId;
            datas = Datas;
        }

        public string GeneratGetRecordInputXML()
        {
            string errorLog = logobj.CreateLog("Get Record from AxInterface page.", HttpContext.Current.Session["nsessionid"].ToString(), "GetRecord-" + transId, "new");

            string xml = "<root axpapp='" + HttpContext.Current.Session["project"].ToString() + "' sessionid='" + HttpContext.Current.Session["nsessionid"].ToString() + "' trace='" + errorLog + "' transid='" + transId + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "'><values>";

            foreach (Dictionary<string, object> data in datas)
            {
                List<string> keyList = new List<string>(data.Keys);

                foreach (string key in keyList)
                {
                    string pName = util.CheckUrlSpecialChars(key);
                    string pValue = util.CheckUrlSpecialChars(data[key].ToString());

                    xml = xml + "<" + pName + ">" + pValue + "</" + pName + ">";
                }
            }

            xml = xml + "</values>";

            xml = xml + HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";

            return xml;
        }

        public string CallGetRecordWS()
        {
            string json = string.Empty;

            string getRecXml = GeneratGetRecordInputXML();

            ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
            json = objWebServiceExt.CallGetRecordIdWS(transId, getRecXml);
            return json;
        }
    }
}
