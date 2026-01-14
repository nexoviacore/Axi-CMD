using AxInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

public partial class axinterface : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["project"] == null)
        {
            SessionExpired();
            return;
        }
    }

    public void SessionExpired()
    {
        Util.Util util = new Util.Util();
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

    [WebMethod]
    public static string GetGlobalVar()
    {
        string json = string.Empty;
        try
        {
            AxpertInterGlobalVar objGblData = new AxpertInterGlobalVar();
            json = objGblData.GetGlobalVars();
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Global Variables -" + ex.Message, sessID, "GetGlobalVar", "new");
        }
        return json;
    }

    [WebMethod]
    public static string TstructSaveData(string TransId, string RecordId = "0")
    {
        string json = string.Empty;
        try
        {
            if (HttpContext.Current.Session["AxInterData-" + TransId] != null)
            {
                DataSet ds = (DataSet)HttpContext.Current.Session["AxInterData-" + TransId];
                AxpertInterSaveData objSaveData = new AxpertInterSaveData(TransId, ds, RecordId);
                json = objSaveData.GenerateXMLStrucuture();
                if (json != string.Empty && json.IndexOf("\"save\": \"failure\"") == -1)
                {
                    HttpContext.Current.Session.Remove("AxInterData-" + TransId);
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Tstruct Save Data -" + ex.Message, sessID, "TstructSaveData", "new");
        }
        return json;
    }

    [WebMethod]
    public static string LoadDataJSON(string TransId, string RecordId)
    {
        string json = string.Empty;
        try
        {
            AxpertInterLoadData objLoadData = new AxpertInterLoadData(TransId, RecordId);
            json = objLoadData.LoadData();
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Load Data JSON -" + ex.Message, sessID, "LoadDataJSON", "new");
        }
        return json;
    }

    [WebMethod]
    public static string FormLoadData(string TransId, ArrayList SearchVars)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string json = string.Empty;
        try
        {
            AxpertInterDoFormLoad objLoadData = new AxpertInterDoFormLoad(TransId, SearchVars);
            json = objLoadData.DoFormLoad();
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Form Load Data -" + ex.Message, sessID, "FormLoadData", "new");
        }
        return json;
    }

    [WebMethod]
    public static string GetWidgetData(string widgetId)
    {
        string json = string.Empty;
        try
        {
            AxpertInterGetWidget objWData = new AxpertInterGetWidget(widgetId);
            json = objWData.GetWidgetData();
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Widget Data -" + ex.Message, sessID, "GetWidgetData", "new");
        }
        return json;
    }

    [WebMethod]
    public static string GetIViewData(string ivName)
    {
        string json = string.Empty;
        try
        {
            if (HttpContext.Current.Session["AxInterIvData-" + ivName] != null)
            {
                DataTable ivParams = new DataTable();
                ivParams = (DataTable)HttpContext.Current.Session["AxInterIvData-" + ivName];
                AxpertInterGetIview objIvData = new AxpertInterGetIview(ivName, ivParams);
                json = objIvData.GetIviewData();
                HttpContext.Current.Session.Remove("AxInterIvData-" + ivName);
            }
            else
            {
                DataTable ivParams = new DataTable();
                AxpertInterGetIview objIvData = new AxpertInterGetIview(ivName, ivParams);
                json = objIvData.GetIviewData();
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Iview Data -" + ex.Message, sessID, "GetIViewData", "new");
        }
        return json;
    }

    [WebMethod]
    public static string GetIViewParams(string IviewName)
    {
        string json = string.Empty;
        try
        {
            AxpertInterGetIviewParams objPData = new AxpertInterGetIviewParams(IviewName);
            json = objPData.GetIviewParamData();
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Iview Params -" + ex.Message, sessID, "GetIViewParams", "new");
        }
        return json;
    }

    [WebMethod]
    public static string GetCustomSql(string sqlNames, ArrayList sqlParams)
    {
        string json = string.Empty;
        try
        {
            AxpertInterGetCustomSql objWData = new AxpertInterGetCustomSql(sqlNames, sqlParams);
            json = objWData.GetCustomSqlData().ToString();
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Custom Sql -" + ex.Message, sessID, "GetCustomSql", "new");
        }
        return json;
    }

    [WebMethod]
    public static string GetValue(string TransId, string FieldName, string DcNo, string RowNo)
    {
        string json = string.Empty;
        try
        {
            int rowno = 0;
            if (RowNo != string.Empty)
            {
                rowno = int.Parse(RowNo);
            }
            if (HttpContext.Current.Session["AxInterData-" + TransId] != null)
            {
                DataSet ds = (DataSet)HttpContext.Current.Session["AxInterData-" + TransId];
                json = ds.Tables["DC" + DcNo].Rows[rowno][FieldName].ToString();
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Value -" + ex.Message, sessID, "GetValue", "new");
        }
        return json;
    }

    [WebMethod]
    public static string GetRowCount(string TransId, string DcNo)
    {
        string json = string.Empty;
        try
        {
            if (HttpContext.Current.Session["AxInterData-" + TransId] != null)
            {
                DataSet ds = (DataSet)HttpContext.Current.Session["AxInterData-" + TransId];
                json = ds.Tables["DC" + DcNo].Rows.Count.ToString();
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Row Count -" + ex.Message, sessID, "GetRowCount", "new");
        }
        return json;
    }

    [WebMethod]
    public static string GetRowData(string TransId, string DcNo, string RowNo)
    {
        string json = string.Empty;
        try
        {
            int rowno = 0;
            if (RowNo != string.Empty)
            {
                rowno = int.Parse(RowNo);
            }
            if (HttpContext.Current.Session["AxInterData-" + TransId] != null)
            {
                json = "{\"rowData\":[";
                string jsonVal = string.Empty;
                DataSet ds = (DataSet)HttpContext.Current.Session["AxInterData-" + TransId];
                foreach (DataColumn cols in ds.Tables["DC" + DcNo].Columns)
                {
                    jsonVal += "{\"" + cols.ColumnName + "\":\"" + ds.Tables["DC" + DcNo].Rows[rowno][cols.ColumnName] + "\"},";
                }
                if (jsonVal != string.Empty)
                    jsonVal = jsonVal.Remove(jsonVal.Length - 1, 1);
                json += jsonVal;
                json += "]}";
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Row Data -" + ex.Message, sessID, "GetRowData", "new");
        }
        return json;
    }

    [WebMethod]
    public static string SetValue(string TransId, string FieldName, string DcNo, string RowNo, string Value)
    {
        string json = string.Empty;
        try
        {
            int rowno = 0;
            if (RowNo != string.Empty)
            {
                rowno = int.Parse(RowNo);// > 0 ? int.Parse(RowNo) - 1 : 0;
            }
            if (HttpContext.Current.Session["AxInterData-" + TransId] != null)
            {
                DataSet objDs = (DataSet)HttpContext.Current.Session["AxInterData-" + TransId];
                if (objDs.Tables.Count > 0 && objDs.Tables["DC" + DcNo] != null && objDs.Tables["DC" + DcNo].Columns.Count > 0)
                {
                    DataTable dt = objDs.Tables["DC" + DcNo];
                    if (dt.Rows.Count == 0 && dt.Columns.Contains(FieldName))
                    {
                        dt.Rows.Add();
                        dt.Rows[rowno][FieldName] = Value;
                    }
                    else if (dt.Rows.Count < rowno + 1 && dt.Columns.Contains(FieldName))
                    {
                        dt.Rows.Add();
                        dt.Rows[rowno][FieldName] = Value;
                    }
                    else if (dt.Rows.Count == rowno + 1 && dt.Columns.Contains(FieldName))
                    {
                        dt.Rows[rowno][FieldName] = Value;
                    }
                    else
                    {
                        dt.Columns.Add(FieldName, typeof(String));
                        dt.Rows[rowno][FieldName] = Value;
                    }
                    objDs.Tables["DC" + DcNo].AcceptChanges();
                    HttpContext.Current.Session["AxInterData-" + TransId] = objDs;
                }
                else if (objDs.Tables["DC" + DcNo] == null)
                {
                    DataTable objDt = new DataTable("DC" + DcNo);
                    objDt.Columns.Add(FieldName, typeof(String));
                    objDt.Rows.Add(Value);
                    objDs.Tables.Add(objDt);
                    HttpContext.Current.Session["AxInterData-" + TransId] = objDs;
                }
                else
                {
                    DataTable dt = objDs.Tables["DC" + DcNo];
                    dt.Columns.Add(FieldName, typeof(String));
                    dt.Rows.Add(Value);
                    objDs.Tables.Add(dt);
                    HttpContext.Current.Session["AxInterData-" + TransId] = objDs;
                }
            }
            else
            {
                DataSet objDs = new DataSet();
                DataTable objDt = new DataTable("DC" + DcNo);
                objDt.Columns.Add(FieldName, typeof(String));
                objDt.Rows.Add(Value);
                objDs.Tables.Add(objDt);
                HttpContext.Current.Session["AxInterData-" + TransId] = objDs;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Set Value -" + ex.Message, sessID, "SetValue", "new");
        }
        return json;
    }

    [WebMethod]
    public static string ClearData(string TransId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string json = string.Empty;
        try
        {
            if (HttpContext.Current.Session["AxInterData-" + TransId] != null)
                HttpContext.Current.Session.Remove("AxInterData-" + TransId);
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Clear Data -" + ex.Message, sessID, "ClearData", "new");
        }
        return json;
    }

    [WebMethod]
    public static string LoadData(string TransId, string RecordId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string json = string.Empty;
        try
        {
            AxpertInterLoadData objLoadData = new AxpertInterLoadData(TransId, RecordId);
            json = objLoadData.LoadData();
            if (json != "" && json != Constants.SESSIONTIMEOUT)
            {
                DataSet objDs = new DataSet();
                var fldData = JsonConvert.DeserializeObject<ResultData>(json.ToString());
                DataTable objDt = new DataTable();
                foreach (var items in fldData.data)
                {
                    if (items.n != null)
                    {
                        if (items.n.StartsWith("DC"))
                        {
                            if (objDt != null && objDt.Rows.Count > 0)
                                objDs.Tables.Add(objDt);
                            objDt = new DataTable(items.n);
                        }
                        else
                        {
                            if (!objDt.Columns.Contains(items.n))
                                objDt.Columns.Add(items.n, typeof(String));
                            int rowno = int.Parse(items.r);
                            rowno = rowno == 0 ? 0 : rowno - 1;
                            if (objDt.Rows.Count == 0)
                                objDt.Rows.Add(items.v);
                            else if (objDt.Rows.Count == rowno)
                                objDt.Rows.Add(items.v);
                            else
                                objDt.Rows[rowno][items.n] = items.v;
                        }
                    }
                }
                objDs.Tables.Add(objDt);
                HttpContext.Current.Session["AxInterData-" + TransId] = objDs;
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Load Data -" + ex.Message, sessID, "LoadData", "new");
        }
        return json;
    }

    public class ResultData
    {
        public List<fieldValues> data { get; set; }
    }

    public class fieldValues
    {
        public string n { get; set; }
        public string v { get; set; }
        public string r { get; set; }
    }

    [WebMethod]
    public static string SetParamValue(string IViewName, ArrayList ParamName, ArrayList ParamValue)
    {
        string json = string.Empty;
        try
        {
            DataTable ivDt = new DataTable();
            foreach (string colName in ParamName)
            {
                ivDt.Columns.Add(colName);
            }
            int icl = 0;
            foreach (string colVal in ParamValue)
            {
                if (ivDt.Rows.Count == 0)
                    ivDt.Rows.Add(colVal);
                else
                    ivDt.Rows[0][icl] = colVal;
                icl++;
            }
            HttpContext.Current.Session["AxInterIvData-" + IViewName] = ivDt;
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Set Param Value -" + ex.Message, sessID, "SetParamValue", "new");
        }
        return json;
    }

    [WebMethod]
    public static string CallAction(string ActionName, string ActionType, string SName, bool IsScript, string IvSelectedRows)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string json = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            if (ActionType == "tstruct")
            {
                if (HttpContext.Current.Session["AxInterData-" + SName] != null)
                    ds = (DataSet)HttpContext.Current.Session["AxInterData-" + SName];
            }
            else
            {
                if (HttpContext.Current.Session["AxInterIvData-" + SName] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)HttpContext.Current.Session["AxInterIvData-" + SName];
                    ds.Tables.Add(dt);
                }
            }

            AxpertInterAction objData = new AxpertInterAction(ActionName, ActionType, SName, IsScript, ds, IvSelectedRows);
            json = objData.Action();
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Call Action -" + ex.Message, sessID, "CallAction", "new");
        }
        return json;
    }

    [WebMethod]
    public static string TstructDeleteRecord(string TransId, string RecordId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string json = string.Empty;
        try
        {
            if (HttpContext.Current.Session["AxInterData-" + TransId] != null)
            {
                // DataSet ds = (DataSet)HttpContext.Current.Session["AxInterData-" + TransId];
                AxpertInterDeleteRecord objDeleteRecord = new AxpertInterDeleteRecord(TransId, RecordId);
                json = objDeleteRecord.CallDeleteDataWS();
                if (json != string.Empty && json.IndexOf("\"msg\":\"Data deleted successfully\"") != -1)
                {
                    HttpContext.Current.Session.Remove("AxInterData-" + TransId);
                }
            }
        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Tstruct Delete Record -" + ex.Message, sessID, "TstructDeleteRecord", "new");
        }
        return json;
    }

    [WebMethod]
    public static string GetTstructRecordId(string TransId, ArrayList Datas)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string json = string.Empty;
        try
        {
            //if (HttpContext.Current.Session["AxInterData-" + TransId] != null)
            //{
            // DataSet ds = (DataSet)HttpContext.Current.Session["AxInterData-" + TransId];
            AxpertInterGetRecord objGetRecord = new AxpertInterGetRecord(TransId, Datas);
            json = objGetRecord.CallGetRecordWS();
            //if (json != string.Empty && json.IndexOf("\"msg\":\"Data deleted successfully\"") != -1)
            //{
            //    HttpContext.Current.Session.Remove("AxInterData-" + TransId);
            //}
            //}
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(json);
                json = JsonConvert.SerializeXmlNode(doc);
            }
            catch (Exception ex) { }

        }
        catch (Exception ex)
        {
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Get Tstruct Record -" + ex.Message, sessID, "GetTstructRecord", "new");
        }
        return json;
    }

    [WebMethod]
    public static string SaveToARMQueue(string jsonData, string TransId, string RecordId, string ChangedDcs)
    {
        string json = string.Empty;
        try
        {
            string response = string.Empty;
            string DATA = string.Empty;

            //string URL = System.Configuration.ConfigurationManager.AppSettings["ARM_URL"] + System.Configuration.ConfigurationManager.AppSettings["ARMPushToQueue_API"];
            string URL = String.Empty;
            if (HttpContext.Current.Session["ARM_URL"] != null && HttpContext.Current.Session["ARMPushToQueue_API"] != null)
                URL = HttpContext.Current.Session["ARM_URL"].ToString() + HttpContext.Current.Session["ARMPushToQueue_API"].ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            if (ChangedDcs != "")
            {
                ChangedDcs = ChangedDcs.Substring(0, ChangedDcs.Length - 1);
            }

            string appsesskey = HttpContext.Current.Session["AppSessionKey"].ToString();
            string globalvars = xmltojson(HttpContext.Current.Session["axGlobalVars"].ToString());
            string uservars = xmltojson(HttpContext.Current.Session["axUserVars"].ToString());
            string axapps = xmltojson(HttpContext.Current.Session["axApps"].ToString());
            jsonData = jsonData.Remove(jsonData.Length - 2);
            Util.Util util = new Util.Util();
            string pwd = util.MD5Hash(HttpContext.Current.Session["pwd"].ToString());
            DATA = "{\"_parameters\":[{\"savedata\":{\"axpapp\":\"" + HttpContext.Current.Session["project"].ToString() + "\",\"appsessionkey\":\"" + appsesskey + "\" " + ",\"transid\":\"" + TransId + "\",\"s\":\"" + HttpContext.Current.Session.SessionID + "\",\"username\":\"" + HttpContext.Current.Session["username"].ToString() + "\",\"password\":\"" + pwd + "\",\"changedrows\":{" + ChangedDcs + "},\"trace\":\"true\",\"recordid\":\"" + RecordId + "\",";
            DATA += jsonData + "}],\"globalvars\":{" + globalvars + "},\"uservars\":{" + uservars + "},\"axapps\":{" + axapps + "}";
            DATA += "}}]}";

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
            LogFile.Log logObj = new LogFile.Log();
            string sessID = Constants.GeneralLog;
            if (HttpContext.Current.Session != null)
                sessID = HttpContext.Current.Session.SessionID;
            logObj.CreateLog("Tstruct Save Data To ARMQ -" + ex.Message, sessID, "SaveToARMQueue", "new");
        }
        return json;
    }

    public static string xmltojson(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        string json = JsonConvert.SerializeXmlNode(doc);
        var parsed = JObject.Parse(json);
        var jtoken = ((Newtonsoft.Json.Linq.JContainer)parsed.First).First;
        return jtoken.ToString().Trim('{').Trim('}');
    }

    //private static string CallWebAPI(string url, string method = "GET", string contentType = "application/json", string body = "", string calledFrom = "")
    //{
    //    string result = string.Empty;
    //    try
    //    {
    //        if (url.ToLower().IndexOf("getactivetasks") > -1)
    //        {

    //        }
    //        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
    //        httpRequest.Method = method;
    //        httpRequest.ContentType = contentType;
    //        if (url.ToLower().IndexOf("https") == -1)
    //        {
    //            httpRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
    //        }
    //        if (HttpContext.Current.Session["ARM_Token"] != null && HttpContext.Current.Session["ARM_Token"].ToString() != string.Empty)
    //        {
    //            var token = HttpContext.Current.Session["ARM_Token"].ToString();
    //            httpRequest.Headers.Add("Authorization", "Bearer " + token);
    //        }

    //        using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
    //        {
    //            streamWriter.Write(body);
    //        }

    //        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
    //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //        {
    //            result = streamReader.ReadToEnd();
    //        }
    //    }
    //    catch (WebException e)
    //    {
    //        try
    //        {
    //            using (WebResponse response = e.Response)
    //            {
    //                HttpWebResponse httpResponse = (HttpWebResponse)response;
    //                Console.WriteLine("Error code: {0}", httpResponse.StatusCode);

    //                //if (calledFrom == "" && httpResponse.StatusCode == HttpStatusCode.Unauthorized)
    //                //{
    //                //    return RefreshSessionAndRecallAPI(url, method, contentType, body, calledFrom = "Error");
    //                //}

    //                using (Stream data = response.GetResponseStream())
    //                using (var reader = new StreamReader(data))
    //                {
    //                    result = reader.ReadToEnd();
    //                }

    //                //if (calledFrom == "" && result.IndexOf("SessionId is not valid") > -1)
    //                //{
    //                //    return RefreshSessionAndRecallAPI(url, method, contentType, body, calledFrom = "Error");
    //                //}
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            result = ex.Message;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        result = e.Message;
    //    }
    //    return result;
    //}

    //private static string RefreshSessionAndRecallAPI(string url, string method = "GET", string contentType = "application/json", string body = "", string calledFrom = "")
    //{
    //    HttpContext.Current.Session.Remove("ARM_SessionId");
    //    HttpContext.Current.Session.Remove("ARM_Token");
    //    AnalyticsUtils _aUtils = new AnalyticsUtils();
    //    string ARMSessionId = _aUtils.ARMSessionId;
    //    try
    //    {
    //        var jsonBody = JObject.Parse(body);
    //        if (jsonBody["ARMSessionId"] != null)
    //        {
    //            jsonBody["ARMSessionId"] = ARMSessionId;
    //            body = JsonConvert.SerializeObject(jsonBody);
    //        }
    //    }
    //    catch { }
    //    return CallWebAPI(url, method, contentType, body, calledFrom = "Error");
    //}

    private static string MD5Hash(string text)
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

    private static string GetARMSessionId()
    {
        try
        {
            var ARMSessionId = "";
            string sessionId = HttpContext.Current.Session.SessionID;
            if (HttpContext.Current.Session["ARM_SessionId"] == null)
            {
                //string privateKey = ConfigurationManager.AppSettings["ARM_PrivateKey"].ToString();
                string privateKey = String.Empty;
                if (HttpContext.Current.Session["ARM_PrivateKey"] != null)
                    privateKey = HttpContext.Current.Session["ARM_PrivateKey"].ToString();
                else
                    return "Error in ARM connection.";

                string hashedKey = MD5Hash(privateKey + sessionId);
                var axpertDetails = new
                {
                    user = HttpContext.Current.Session["user"].ToString(),
                    key = hashedKey,
                    AxSessionId = sessionId,
                    Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
                    AppName = HttpContext.Current.Session["project"].ToString()
                };
                string ARM_URL = string.Empty;
                if (HttpContext.Current.Session["ARM_URL"] != null)
                    ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();
                else
                    return "Error in ARM connection.";
                string connectionUrl = ARM_URL + "/api/v1/ARMConnectFromAxpert";
                AnalyticsUtils _aUtils = new AnalyticsUtils();
                var connectionResult = _aUtils.CallWebAPI(connectionUrl, "POST", "application/json", JsonConvert.SerializeObject(axpertDetails));

                var jObj = Newtonsoft.Json.Linq.JObject.Parse(connectionResult);
                if (jObj != null && jObj["result"] != null)
                {
                    if (Convert.ToBoolean(jObj["result"]["success"]))
                    {
                        ARMSessionId = jObj["result"]["connectionid"].ToString();
                        var token = jObj["result"]["token"].ToString();
                        HttpContext.Current.Session["ARM_SessionId"] = ARMSessionId;
                        HttpContext.Current.Session["ARM_Token"] = token;
                    }
                    else
                    {
                        return "Error in ARM connection.";
                    }
                }
            }
            else
            {
                ARMSessionId = HttpContext.Current.Session["ARM_SessionId"].ToString();
            }
            return ARMSessionId;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string CallDataSourceAPI(string apiPublicKey, Dictionary<string, string> sqlParams)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        //string ARMSessionId = GetARMSessionId();

        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;

        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();
        else
            return "Error in ARM connection.";
        string tasksUrl = ARM_URL + "/api/v1/ARMExecutePublishedAPI";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Project = HttpContext.Current.Session["project"].ToString(),
            PublicKey = apiPublicKey,
            UserName = HttpContext.Current.Session["username"].ToString(),
            GetSqlData = new
            {
                username = HttpContext.Current.Session["username"].ToString(),
                Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            },
            SqlParams = sqlParams
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

    [WebMethod(EnableSession = true)]
    public static string AxSetValue(string apiPublicKey, string fieldName, string dcNo, string rowNo, string value)
    {
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> data = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        if (HttpContext.Current.Session["AxAPISubmitData-" + apiPublicKey] != null)
        {
            data = (Dictionary<string, Dictionary<string, Dictionary<string, string>>>)HttpContext.Current.Session["AxAPISubmitData-" + apiPublicKey];
        }
        else { }

        if (!data.ContainsKey("dc" + dcNo))
        {
            data["dc" + dcNo] = new Dictionary<string, Dictionary<string, string>>();
        }

        // Check for RowNo and add if not present
        if (!data["dc" + dcNo].ContainsKey("row" + rowNo))
        {
            data["dc" + dcNo]["row" + rowNo] = new Dictionary<string, string>();
        }

        // Add or update the FieldName with the value
        data["dc" + dcNo]["row" + rowNo][fieldName] = value;
        HttpContext.Current.Session["AxAPISubmitData-" + apiPublicKey] = data;

        return "Added";
    }

    [WebMethod(EnableSession = true)]
    public static string CallSubmitDataAPI(string apiPublicKey, string recordId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        if (HttpContext.Current.Session["AxAPISubmitData-" + apiPublicKey] == null)
        {
            return "Error. Field data is not available for save.";
        }

        //string ARMSessionId = GetARMSessionId();
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;

        string sessionId = HttpContext.Current.Session.SessionID;

        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();
        else
            return "Error in ARM connection.";
        string tasksUrl = ARM_URL + "/api/v1/ARMExecutePublishedAPI";

        var data = new Dictionary<string, object>();
        data["mode"] = "new";
        data["keyvalue"] = "";
        data["recordid"] = recordId;

        var fieldData = (Dictionary<string, Dictionary<string, Dictionary<string, string>>>)HttpContext.Current.Session["AxAPISubmitData-" + apiPublicKey];
        foreach (var fData in fieldData)
        {
            data.Add(fData.Key, (object)fData.Value);
        }

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Project = HttpContext.Current.Session["project"].ToString(),
            PublicKey = apiPublicKey,
            UserName = HttpContext.Current.Session["username"].ToString(),
            SubmitData = new
            {
                username = HttpContext.Current.Session["username"].ToString(),
                Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
                keyfield = "",
                dataarray = new
                {
                    data = data
                }
            }
        };

        HttpContext.Current.Session.Remove("AxAPISubmitData-" + apiPublicKey);

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

    [WebMethod(EnableSession = true)]
    public static string CallAxScriptAPI(string script, string type, string name, string recordid, Dictionary<string, string> apiParams)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        //string ARMSessionId = GetARMSessionId();
        string sessionId = HttpContext.Current.Session.SessionID;

        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();
        else
            return "Error in ARM connection.";
        string tasksUrl = ARM_URL + "/api/v1/AxScript";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            Script = script,
            Type = type,
            Name = name,
            RecordId = recordid,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ApiParams = apiParams
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

}
