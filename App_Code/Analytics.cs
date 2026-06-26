using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Analytics
{
    List<string> analyticsProperties = new List<string> { "XAXISFIELDS", "YAXISFIELDS", "CHARTTYPE" };
    string ARM_URL = string.Empty;
    string ARMSessionId = string.Empty;
    AnalyticsUtils _aUtils;

    public Analytics() {
        _aUtils = new AnalyticsUtils();
        ARM_URL = _aUtils.ARM_URL;
        ARMSessionId = _aUtils.ARMSessionId;
    }

    public string GetAnalyticsEntityData(string page = "", string transId = "")
    {        
        string apiUrl = ARM_URL + "/AxList/api/v1/GetAnalyticsEntityData";

        var inputJson = new
        {
            Page = page,
            ARMSessionId = ARMSessionId,
            AxSessionId = HttpContext.Current.Session.SessionID,
            TransId = transId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            Roles = HttpContext.Current.Session["AxRole"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString(),
            Language = HttpContext.Current.Session["language"].ToString(),
            PropertiesList = analyticsProperties
        };

        var analyticsData = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
        return analyticsData;
    }

    public string SetAnalyticsData(string page, string transId, Dictionary<string, string> properties, bool allUsers)
    {       
        string apiUrl = ARM_URL + "/ARM_APIs/api/v1/SetAnalyticsData";

        var inputJson = new
        {
            Page = page,
            ARMSessionId = ARMSessionId,
            AxSessionId = HttpContext.Current.Session.SessionID,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            TransId = transId,
            Properties = properties,
            All = allUsers,
            Language = HttpContext.Current.Session["language"].ToString(),
            Roles = HttpContext.Current.Session["AxRole"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString()
        };

        var result = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
        return result;
    }

    public string GetAnalyticsData(string page, string transId, List<string> propertiesList)
    {        
        string apiUrl = ARM_URL + "/ARM_APIs/api/v1/GetAnalyticsData";

        var inputJson = new
        {
            Page = page,
            ARMSessionId = ARMSessionId,
            AxSessionId = HttpContext.Current.Session.SessionID,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            TransId = transId,
            PropertiesList = propertiesList,
            Language = HttpContext.Current.Session["language"].ToString(),
            Roles = HttpContext.Current.Session["AxRole"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString()
        };

        var result = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
        return result;
    }

    public string GetAnalyticsChartsData(string page, string transId, string aggField, string aggTransId, string groupField, string groupTransId, string aggFunc)
    {
        string apiUrl = ARM_URL + "/AxList/api/v1/GetAnalyticsChartsData";

        var inputJson = new
        {
            Page = page,
            ARMSessionId = ARMSessionId,
            AxSessionId = HttpContext.Current.Session.SessionID,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            TransId = transId,
            ChartMetaData = new List<object>
            { 
                new {
                    Aggfield = aggField,
                    Groupfield = groupField,
                    AggTransId = aggTransId,
                    Grouptransid = groupTransId,
                    AggFunc = aggFunc
                }
            },
            Roles = HttpContext.Current.Session["AxRole"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString(),
            Language = HttpContext.Current.Session["language"].ToString()
            //,
            //ViewFilters = _aUtils.GetViewFilters(new List<string> { transId }),
            //GlobalParams = _aUtils.GetGlobalParams()
        };

        var result = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
        return result;
    }

    public string GetAnalyticsMultipleChartsData(string page, string transId, List<object> charts)
    {
        string apiUrl = ARM_URL + "/AxList/api/v1/GetAnalyticsChartsData";

        var inputJson = new
        {
            Page = page,
            ARMSessionId = ARMSessionId,
            AxSessionId = HttpContext.Current.Session.SessionID,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            TransId = transId,
            ChartMetaData = charts,
            Roles = HttpContext.Current.Session["AxRole"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString(),
            Language = HttpContext.Current.Session["language"].ToString()
            //,
            //ViewFilters = _aUtils.GetViewFilters(new List<string> { transId }),
            //GlobalParams = _aUtils.GetGlobalParams()
        };

        var result = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
        return result;
    }
    
    public string GetEntityDropDownData(string transId, string fldId, string searchText)
    {
        string apiUrl = ARM_URL + "/AxList/api/v1/GetEntityDropdownValues";

        var inputJson = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = HttpContext.Current.Session.SessionID,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            TransId = transId,
            FieldId = fldId,
            SearchText = searchText,
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString(),
            Language = HttpContext.Current.Session["language"].ToString()
        };

        var result = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
        return result;
    }

    public string GetEntityList(string selectedEntites = "")
    {
        string tasksUrl = ARM_URL + "/ARM_APIs/api/v1/GetEntityList";

        var entityDetails = new
        {
            ARMSessionId = ARMSessionId,
            AxSessionId = HttpContext.Current.Session.SessionID,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            Roles = HttpContext.Current.Session["AxRole"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString(),
            Language = HttpContext.Current.Session["language"].ToString(),
            EntityName = selectedEntites
        };

        var entities = _aUtils.CallWebAPI(tasksUrl, "POST", "application/json", JsonConvert.SerializeObject(entityDetails));
        return entities;

    }


}
