//using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
//using System.Net.PeerToPeer;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Diagnostics;
using System.Security.Policy;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net.Mime;
using System.Web.Routing;
using System.Xml;

public partial class aspx_AxPEG : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    //private static string CallWebAPI(string url, string method = "GET", string contentType = "application/json", string body = "", string calledFrom = "")
    //{
    //    string result = string.Empty;
    //    try
    //    {

    //        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
    //        httpRequest.Method = method;
    //        httpRequest.ContentType = contentType;
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
    //            Util.Util util = new Util.Util();
    //            using (WebResponse response = e.Response)
    //            {
    //                HttpWebResponse httpResponse = (HttpWebResponse)response;
    //                //bool isValidUser = false;
    //                //if (util.CheckValidLogin())
    //                //{
    //                //    isValidUser = true;
    //                if (calledFrom == "" && httpResponse.StatusCode == HttpStatusCode.Unauthorized)
    //                {
    //                    return RefreshSessionAndRecallAPI(url, method, contentType, body, calledFrom = "Error");
    //                }
    //                //}

    //                using (Stream data = response.GetResponseStream())
    //                using (var reader = new StreamReader(data))
    //                {
    //                    result = reader.ReadToEnd();
    //                }

    //                //if (isValidUser)
    //                //{
    //                if (calledFrom == "" && result.IndexOf("SessionId is not valid") > -1)
    //                {
    //                    return RefreshSessionAndRecallAPI(url, method, contentType, body, calledFrom = "Error");
    //                }
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
    //    var ARMSessionId = GetARMSessionId();
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

    //private static string GetARMSessionId()
    //{
    //    AnalyticsUtils _aUtils = new AnalyticsUtils();
    //    return _aUtils.ARMSessionId;
    //}

    [WebMethod]
    public static string GetActiveTasks()
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
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetActiveTasks";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString()
        };
        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetBulkActiveTasks(string processName, string taskType)
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
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetBulkActiveTasks";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            TaskType = taskType,
            ToUser = HttpContext.Current.Session["username"].ToString(),
            ProcessName = processName
        };
        
        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }


    [WebMethod]
    public static string AxDoTaskAction(string action, string taskId, string taskType, string statusReason, string statusText, string toUser)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;
        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();
        else
            return "Error in ARM connection.";
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMDoTaskAction";
        string password = MD5Hash(HttpContext.Current.Session["pwd"].ToString());
        var taskDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            TaskId = taskId,
            TaskType = taskType,
            Action = action,
            StatusReason = statusReason,
            StatusText = statusText,
            AppName = HttpContext.Current.Session["project"].ToString(),
            User = HttpContext.Current.Session["username"].ToString(),
            //password = password,
            returnTo = toUser,
            sendTo = toUser,
            toTask = ""
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(taskDetails));
        return tasks;
    }


    [WebMethod]
    public static string AxDoBulkAction(string action, string taskId, string taskType, string processName, string statusText)
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
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMDoBulkAction";
        string password = MD5Hash(HttpContext.Current.Session["pwd"].ToString());
        var taskDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            TaskId = taskId,
            taskType = taskType,
            action = action,
            //statusReason = statusReason,
            statusText = statusText,
            AppName = HttpContext.Current.Session["project"].ToString(),
            user = HttpContext.Current.Session["username"].ToString()
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(taskDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetProcess(string processName, string keyField, string keyValue)
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
        string processUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetProcessFlow";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ProcessName = processName,
            KeyField = keyField,
            KeyValue = keyValue,
            AppName = HttpContext.Current.Session["project"].ToString()
        };

        var tasks = _aUtils.CallWebAPI(processUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetProcessDefinition(string processName)
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
        string processUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetAxProcessDefinition";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ProcessName = processName,
            AppName = HttpContext.Current.Session["project"].ToString()
        };

        var tasks = _aUtils.CallWebAPI(processUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetProcessList(string processName)
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
        string processUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetAxProcessList";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ProcessName = processName,
            ToUser = HttpContext.Current.Session["username"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString()
        };

        var tasks = _aUtils.CallWebAPI(processUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetProcessTask(string taskId)
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
        string processUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetProcessTask";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            TaskId = taskId,
            ToUser = HttpContext.Current.Session["username"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString()
        };

        var tasks = _aUtils.CallWebAPI(processUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetProcessKeyValues(string processName)
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
        string processUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetProcessKeyValues";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ProcessName = processName,
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString()
        };

        var tasks = _aUtils.CallWebAPI(processUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }
    [WebMethod]
    public static string AxGetKeyValue(string processName, string recordId)
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
        string processUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetKeyValue";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ProcessName = processName,
            ToUser = HttpContext.Current.Session["user"].ToString(),
            RecordId = recordId,
            AppName = HttpContext.Current.Session["project"].ToString()
        };

        var tasks = _aUtils.CallWebAPI(processUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetDataSourcesData(string dataSource, Dictionary<string, string> dataSourceParams)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetDataSourcesData";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            DataSources = new List<string>() { dataSource },
            ToUser = HttpContext.Current.Session["user"].ToString(),
            SqlParams = dataSourceParams,
            AppName = HttpContext.Current.Session["project"].ToString()
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetTimelineData(string processName, string keyValue)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetTimelineData";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            ProcessName = processName,
            KeyValue = keyValue,
            AppName = HttpContext.Current.Session["project"].ToString()
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetCardsData(string processName, string taskName, string keyValue)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetProcessCardsData";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            ProcessName = processName,
            taskName = taskName,
            KeyValue = keyValue,
            AppName = HttpContext.Current.Session["project"].ToString(),
            SqlParams = GetGlobalParams(HttpContext.Current.Session["axGlobalVars"].ToString())
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxRefreshCardsData(string cardsIds, Dictionary<string, string> cardsParams)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetCardsDataByIds";
        var globalParams = GetGlobalParams(HttpContext.Current.Session["axGlobalVars"].ToString());
        foreach (KeyValuePair<string, string> kvp in cardsParams)
        {
            if (globalParams.ContainsKey(kvp.Key))
            {
                globalParams[kvp.Key] = kvp.Value;
            }
            else
            {
                globalParams.Add(kvp.Key, kvp.Value);
            }
        }

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            CardId = cardsIds,
            SqlParams = globalParams
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetSendToUsers(string taskId, string taskType, string keyValue, string taskName)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetSendToUsers";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            TaskId = taskId,
            TaskType = taskType,
            KeyValue = keyValue,
            TaskName = taskName
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetProcessUserType(string processName)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetProcessUserType";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            ProcessName = processName
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetNextTaskInProcess(string processName, string keyValue)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetNextTaskInProcess";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            ProcessName = processName,
            KeyValue = keyValue
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetEditableTask(string processName, string keyValue, string taskName, int indexNo)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        //string ARMSessionId = GetARMSessionId();
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;

        //string dataUrl = ConfigurationManager.AppSettings["ARM_URL"] + "/ARM_APIs/api/v1/ARMGetEditableTask";
        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();
        else
            return "Error in ARM connection.";
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetEditableTask";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            ProcessName = processName,
            KeyValue = keyValue,
            TaskName = taskName,
            IndexNo = indexNo
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetEditableForm(string processName, string keyValue, string recordId)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        //string ARMSessionId = GetARMSessionId();
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;

        //string dataUrl = ConfigurationManager.AppSettings["ARM_URL"] + "/ARM_APIs/api/v1/ARMGetEditableTask";
        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();
        else
            return "Error in ARM connection.";
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetEditableForm";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            ProcessName = processName,
            KeyValue = keyValue,
            RecordId = recordId
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetHomePageCards()
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        //string ARMSessionId = GetARMSessionId();
        string sessionId = HttpContext.Current.Session.SessionID;

        string fdKeyMenuData = Constants.REDISMENUDATA;
        string schemaName = string.Empty;
        if (HttpContext.Current.Session["dbuser"] != null)
            schemaName = HttpContext.Current.Session["dbuser"].ToString();
        FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
        string menuResult = "";
        Util.Util util = new Util.Util();
        if (fObj != null)
            menuResult = fObj.StringFromRedis(util.GetRedisServerkey(fdKeyMenuData, "Menu"), schemaName);

        if (string.IsNullOrEmpty(menuResult))
        {
            return "Error. Menu data is not available.";
        }

        JObject resultObject = new JObject();
        string _homepagecards = fObj.StringFromRedis(util.GetRedisServerkey(Constants.REDISHOMEPAGECARDS, ""), schemaName);
        if (_homepagecards != "")
        {
            resultObject = JObject.Parse(_homepagecards);
        }
        else
        {
            try
            {
                //string _sqlQuery = "select axhomeconfigid cardid,caption,pagecaption,displayicon,stransid,datasource,moreoption,colorcode,groupfolder,grppageid from axhomeconfig where active = 'T' order by disporder asc";
                string _sqlQuery = "select axhomeconfigid cardid,caption,pgname,displayicon,stransid,datasource,moreoption,colorcode,groupfolder,grppageid,carddesc,cardhide,html_editor_card,cardwidth,paneltypecnd,paneltype from axhomeconfig where cardhide!='T' order by disporder asc";
                ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
                string result = asbExt.ExecuteSQL("homecards", _sqlQuery, "JSON");
                if (result.IndexOf("{\"error\":") == -1)
                {
                    result = (JObject.Parse(result)["result"]["row"]).ToString();
                    if (result != "")
                    {
                        result = "{\"result\":{\"message\":\"success\",\"data\":" + result + ",\"success\":true}}";
                        resultObject = JObject.Parse(result);
                        if (result.ToString() != "" && resultObject["result"]["message"].ToString() == "success")
                        {
                            try
                            {
                                FDW fdwObj = new FDW();
                                fdwObj.SaveInRedisServer(fObj.MakeKeyName(Constants.REDISHOMEPAGECARDS, ""), result, "", schemaName);
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                    else
                    {
                        return "Error in Home Configuration.";
                    }
                }
                else
                {
                    return "Error in Home Configuration.";
                }
            }
            catch (Exception ex) { }
        }


        XDocument menuXmlDoc = XDocument.Parse(menuResult);

        if (resultObject["result"] != null)
        {
            JObject resultData = (JObject)resultObject["result"];
            if (resultData["data"] != null)
            {
                JArray dataArray = (JArray)resultData["data"];
                var filteredDataArray = new List<JObject>();


                foreach (var rowObject in dataArray.OfType<JObject>())
                {
                    if (rowObject["PANELTYPE"] != null || rowObject["paneltype"] != null)
                    {
                        JToken paneltypeToken = (JToken)rowObject["PANELTYPE"] == null ? rowObject["paneltype"] : rowObject["PANELTYPE"];
                        var panelType = paneltypeToken.ToString();

                        if (panelType == "Menu option")
                        {
                            if (rowObject["STRANSID"] != null || rowObject["stransid"] != null)
                            {
                                JToken strandidToken = (JToken)rowObject["STRANSID"] == null ? rowObject["stransid"] : rowObject["STRANSID"];
                                var strandidValue = strandidToken.ToString();
                                if (strandidValue.StartsWith("t"))
                                {
                                    strandidValue = "?transid=" + strandidValue.Substring(1) + "\"";
                                }
                                else if (strandidValue.StartsWith("i"))
                                {
                                    strandidValue = "?ivname=" + strandidValue.Substring(1) + "\"";
                                }
                                if (menuResult.Contains(strandidValue))
                                {
                                    filteredDataArray.Add(rowObject);
                                }
                            }
                        }
                        else if (panelType == "Menu folder")
                        {
                            JToken folderIdToken = (JToken)rowObject["PGNAME"] == null ? rowObject["pgname"] : rowObject["PGNAME"];
                            var folderId = folderIdToken.ToString();

                            JToken folderNameToken = (JToken)rowObject["CAPTION"] == null ? rowObject["caption"] : rowObject["CAPTION"];
                            var folderName = folderNameToken.ToString();

                            var parent = menuXmlDoc.Descendants()
                       .FirstOrDefault(p => (string)p.Attribute("oname") == folderId);

                            if (parent != null)
                            {
                                var childItems = parent.Descendants("child")
                                                       .Where(c => !string.IsNullOrEmpty((string)c.Attribute("target")));

                                foreach (var childItem in childItems)
                                {
                                    var tempRowObject = new JObject();
                                    tempRowObject.Add("cardid", childItem.Attribute("oname").Value);
                                    tempRowObject.Add("caption", childItem.Attribute("name").Value);
                                    tempRowObject.Add("groupfolder", folderName);
                                    tempRowObject.Add("grppageid", folderId);
                                    tempRowObject.Add("paneltype", panelType);
                                    tempRowObject.Add("target", childItem.Attribute("target").Value);
                                    filteredDataArray.Add(tempRowObject);
                                }
                            }

                        }
                        else if (panelType == "HTML panel")
                        {
                            filteredDataArray.Add(rowObject);
                        }
                    }
                }
                resultData["data"] = new JArray(filteredDataArray);
            }
        }
        return JsonConvert.SerializeObject(resultObject);
    }

    [WebMethod]
    public static string AxGetActiveListPageData(string processName, string keyValue, string taskId, string taskType)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetActiveListPageData";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            ProcessName = processName,
            KeyValue = keyValue,
            TaskId = taskId,
            TaskType = taskType
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }


    [WebMethod]
    public static string AxGetARMDataFromSQL(string sqlName, dynamic sqlParams)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        //string ARMSessionId = GetARMSessionId();
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;

        string dataUrl = "http://20.244.123.19/ARMTest/ARM_APIs/api/v1/ARMGetDataFromSQL";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            dataSource = sqlName,
            RefreshQueueName = "DataRefreshQueue",
            sqlParams = sqlParams
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    private static Dictionary<string, string> GetGlobalParams(string xml)
    {

        XElement element = XElement.Parse(xml);
        Dictionary<string, string> globalParams = new Dictionary<string, string>();

        // Loop through all the child elements of the root element and add them to the dictionary
        foreach (XElement child in element.Elements())
        {
            globalParams[child.Name.LocalName] = child.Value;
        }

        return globalParams;
    }

    [WebMethod]
    public static string ARMGetDropDownDataFromSQL(string dataSource, dynamic sqlParams)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }
        //string ARMSessionId = GetARMSessionId();
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;

        string dataUrl = "http://20.244.123.19/ARMTest/ARM_APIs/api/v1/ARMGetDropDownDataFromSQL";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            dataSource = dataSource,
            RefreshQueueName = "DataRefreshQueue",
            sqlParams = sqlParams
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    //[WebMethod]
    //public static string ARMPushJobsToQueue(string queueData, string queueName, string signalrClient = "", bool traces = false, int timespanDelay = 0)
    //{
    //    if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
    //    {
    //        return Constants.SESSIONTIMEOUT;
    //    }

    //    string dataUrl = "http://20.244.123.19/ARMTest/ARM_APIs/api/v1/ARMPushJobsToQueue";

    //    var processDetails = new
    //    {
    //        queuedata = queueData,
    //        queuename = queueName,
    //        trace = traces,
    //        timespandelay = timespanDelay,
    //        signalrclient = signalrClient
    //    };

    //    var tasks = CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
    //    return tasks;
    //}

    [WebMethod]
    public static string AxGetOptionalTask(string taskId)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetOptionalTask";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ToUser = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            TaskId = taskId
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }
    [WebMethod]
    public static object AxGetARMNotificationDetails()
    {
        //string ARMSessionId = GetARMSessionId();
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;

        }
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        if (ARMSessionId.StartsWith("Error"))
            return ARMSessionId;

        string sessionId = HttpContext.Current.Session.SessionID;
        var token = HttpContext.Current.Session["ARM_Token"].ToString();
        string ARM_Notification_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_Notification_URL"] != null)
            ARM_Notification_URL = HttpContext.Current.Session["ARM_Notification_URL"].ToString();
        else
            return "Error in ARM Notification Hub connection.";
        string url = ARM_Notification_URL;
        var result = new
        {
            token = token,
            url = url,
            sessionId = sessionId
        };
        return result;

    }

    [WebMethod]
    public static string AxDownload(string transid, string recordid, string processname, string keyvalue, string printformname, string outputfilename, string generatenew)
    {
        //string ARMSessionId = GetARMSessionId();
        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;
        string myjson = @"{
            ""_parameters"": [
                {
                    ""axdownload"": {
                        ""axpapp"": """ + HttpContext.Current.Session["project"].ToString() + @""",
                        ""username"":""" + HttpContext.Current.Session["username"].ToString() + @""",
						""trace"":""" + HttpContext.Current.Session["AxTrace"].ToString() + @""",
                        ""transid"": """ + transid + @""",
                        ""recordid"": """ + recordid + @""",
                        ""processname"": """ + processname + @""",
                        ""keyvalue"": """ + keyvalue + @""",
                        ""printformname"":""" + printformname + @""",
                        ""outputfilename"": """ + outputfilename + @""",
                        ""generatenew"": """ + generatenew + @"""
                    },
                       ""globalvars"": {},
                       ""uservars"": {}

                }
            ]
        }";

        var apiUrl = ConfigurationManager.AppSettings["scriptsUrlPath"].ToString() + "/ASBScriptRest.dll/datasnap/rest/TASBScriptRest/AxDownload";
        var tasks = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", myjson);
        try
        {
            JObject taskObject = JObject.Parse(tasks);
            string sourceFilePath = null;
            string fileName = "";
            JToken cmdvalToken = new JObject();
            JArray resultArray = taskObject["result"] as JArray;
            if (resultArray != null && resultArray.Count > 0)
            {
                JObject resultItem = resultArray[0] as JObject;
                if (resultItem != null)
                {
                    JArray commandArray = resultItem["command"] as JArray;
                    if (commandArray != null && commandArray.Count > 0)
                    {
                        JObject commandItem = commandArray[0] as JObject;
                        if (commandItem != null)
                        {
                            cmdvalToken = commandItem["cmdval"];
                            if (cmdvalToken != null)
                            {
                                sourceFilePath = cmdvalToken.Value<string>();
                                fileName = Path.GetFileName(sourceFilePath);
                                commandItem["cmdval"] = fileName;


                            }
                        }
                    }
                    else
                    {
                        JArray error = resultItem["error"] as JArray;
                        if (error != null && error.Count > 0)
                        {
                            return JsonConvert.SerializeObject(error);
                        }
                        else
                        {
                            return "Error encountered";
                        }
                    }
                }
            }

            if (sourceFilePath != null)
            {
                //Console.WriteLine("Source File Path: " + sourceFilePath);

                string updatedTasks = taskObject.ToString();
                var scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
                string destinationFolderPath = scriptsPath + "\\axpert\\" + sessionId;

                //if (fileName.ToLower().Contains(".pdf"))
                //    destinationFolderPath += "\\PDF";

                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);
                Directory.CreateDirectory(destinationFolderPath);
                File.Copy(sourceFilePath, destinationFilePath, true);
                //Console.WriteLine("File copied successfully.");
                return updatedTasks;
            }
            else
            {
                return "No Source File found";
            }

        }
        catch (Exception ex)
        {
            //Console.WriteLine(ex.Message);
            return ex.Message;
        }
    }

    [WebMethod]
    public static string AxGetNotifyHistory()
    {
        string notifyHistory = string.Empty;
        try
        {
            if (HttpContext.Current.Session["username"] != null)
            {
                string userName = HttpContext.Current.Session["username"].ToString();
                string schemaName = string.Empty;
                if (HttpContext.Current.Session["dbuser"] != null)
                    schemaName = HttpContext.Current.Session["dbuser"].ToString();
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                Util.Util util = new Util.Util();
                if (fObj != null)
                {
                    notifyHistory = fObj.StringFromRedis(util.GetRedisServerkey(Constants.AXNOTIFYHISTORY, userName), schemaName);
                    if (notifyHistory != "")
                    {
                        FDW fdwObj = new FDW();
                        fdwObj.Deletekey(util.GetRedisServerkey(Constants.AXNOTIFYHISTORY, userName));
                    }
                }

            }
        }
        catch (Exception ex)
        {
            notifyHistory = string.Empty;
        }
        return notifyHistory;
    }

    [WebMethod]
    public static string AxGetPendingActiveTasks(int pageNo, int pageSize)
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
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetPendingActiveTasks";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            PageSize = pageSize,
            PageNo = pageNo,
            GetCount = false,
            GetTaskDetails = false
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetCompletedTasks(int pageNo, int pageSize)
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
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetCompletedTasks";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            PageSize = pageSize,
            PageNo = pageNo,
            GetCount = false,
            GetTaskDetails = false
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxPEGGetTaskDetails(string processName, string taskType, string taskId, string keyValue)
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
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMPEGGetTaskDetails";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            ProcessName = processName,
            TaskType = taskType,
            TaskId = taskId,
            KeyValue = keyValue,
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetFilteredActiveTasks(string filterType, int pageNo, int pageSize, string fromuser, string processname, string fromdate, string todate, string searchtext)
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
        string tasksUrl = string.Empty;
        if (filterType == "pending")
            tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetFilteredActiveTasks";
        else
            tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetFilteredCompletedTasks";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            PageSize = pageSize,
            PageNo = pageNo,
            GetCount = false,
            GetTaskDetails = false,
            fromuser = fromuser,
            processname = processname,
            fromdate = fromdate,
            todate = todate,
            searchtext = searchtext
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetBulkApprovalCount()
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
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetBulkApprovalCount";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }


    [WebMethod]
    public static string AxGetApproveToTasks(string taskId, string processName, string keyValue, string taskName, string indexNo, string transId, string recordId)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetApproveToTasks";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AxUserName = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            ProcessName = processName,
            IndexNo = indexNo,
            TaskName = taskName,
            TransId = transId,
            RecordId = recordId,
            TaskId = taskId,
            KeyValue = keyValue
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetReturnToTasks(string taskId, string processName, string keyValue, string taskName, string indexNo, string transId, string recordId)
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
        string dataUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetReturnToTasks";

        var processDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AxUserName = HttpContext.Current.Session["user"].ToString(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            ProcessName = processName,
            IndexNo = indexNo,
            TaskName = taskName,
            TransId = transId,
            RecordId = recordId,
            TaskId = taskId,
            KeyValue = keyValue
        };

        var tasks = _aUtils.CallWebAPI(dataUrl, "POST", "application/json", JsonConvert.SerializeObject(processDetails));
        return tasks;
    }

    [WebMethod]
    public static string AxGetAllActiveTasks(int pageNo, int pageSize, string filter)
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
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMGetAllActiveTasks";

        var connectionDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = sessionId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            PageSize = pageSize,
            PageNo = pageNo,
            Filter = filter
        };
        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }


    [WebMethod(EnableSession = true)]
    public static string GetDataFromDataSource(string name, Dictionary<string, string> sqlParams, bool refreshCache)
    {
        return "GetDataFromDataSource WebMethod is deprecated. Use GetDataFromAxList WebMethod instead.";

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
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/GetDataFromDatasource";

        //AnalyticsUtils _aUtils = new AnalyticsUtils();
        var connectionDetails = new
        {
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ARMSessionId = ARMSessionId,
            DataSource = name,
            SqlParams = sqlParams,
            RefreshCache = refreshCache
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

    [WebMethod(EnableSession = true)]
    public static string GetDataFromAxList(List<string> adsNames, string keyField, string keyValue, Dictionary<string, string> sqlParams, bool refreshCache, Dictionary<string, object> props)
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
        string tasksUrl = ARM_URL + "/AxList/api/v1/AxList";

        //AnalyticsUtils _aUtils = new AnalyticsUtils();

        props = new Dictionary<string, object>(props, StringComparer.OrdinalIgnoreCase);

        var connectionDetails = new
        {
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            ARMSessionId = ARMSessionId,
            Action = "view",
            ADSNames = adsNames,
            KeyField = keyField,
            KeyValue = keyValue,
            SqlParams = sqlParams,
            RefreshCache = refreshCache,
            ADS = (props.ContainsKey("ADS") && !string.IsNullOrWhiteSpace(props["ADS"].ToString()) ? Convert.ToBoolean(props["ADS"]) : true),
            CachePermissions = (props.ContainsKey("CachePermissions") && !string.IsNullOrWhiteSpace(props["CachePermissions"].ToString()) ? Convert.ToBoolean(props["CachePermissions"]) : true),
            GetAllRecordsCount = (props.ContainsKey("GetAllRecordsCount") && !string.IsNullOrWhiteSpace(props["GetAllRecordsCount"].ToString()) ? Convert.ToBoolean(props["GetAllRecordsCount"]) : false),
            PageNo = (props.ContainsKey("PageNo") && !string.IsNullOrWhiteSpace(props["PageNo"].ToString()) ? Convert.ToInt32(props["PageNo"].ToString()) : 1),
            PageSize = (props.ContainsKey("PageSize") && !string.IsNullOrWhiteSpace(props["PageSize"].ToString()) ? Convert.ToInt32(props["PageSize"].ToString()) : 100),
            Sorting = (props.ContainsKey("Sorting") ? props["Sorting"] : null),
            Filters = (props.ContainsKey("Filters") ? props["Filters"] : null),
            AxClient_dateformat = (props.ContainsKey("axClient_dateformat") ? props["axClient_dateformat"] : null),
            Select_Columns = (props.ContainsKey("select_columns") ? props["select_columns"] : null),
            Aggregations = (props.ContainsKey("aggregations") ? props["aggregations"] : null),
            GroupBy_Columns = (props.ContainsKey("groupby_columns") ? props["groupby_columns"] : null)
        };

        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }

    [WebMethod(EnableSession = true)]
    public static string AxPushToQueue(string queueName, Dictionary<string, string> queueJson)
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        AnalyticsUtils _aUtils = new AnalyticsUtils();
        string ARMSessionId = _aUtils.ARMSessionId;
        string sessionId = HttpContext.Current.Session.SessionID;

        string ARM_URL = string.Empty;
        if (HttpContext.Current.Session["ARM_URL"] != null)
            ARM_URL = HttpContext.Current.Session["ARM_URL"].ToString();
        else
            return "Error in ARM connection.";
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/ARMPushToQueue";

        var connectionDetails = new
        {
            QueueName = queueName,
            QueueJson = queueJson,
            QueueData = queueName + "Data"
        };
        var tasks = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(connectionDetails));
        return tasks;
    }
}
