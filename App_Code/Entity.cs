using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

public class Entity
{
    List<string> entityProperties = new List<string> { "FIELDS", "FILTERS", "KEYFIELD", "CHARTS", "RIGHTPANEL", "MODIFICATIONFIELDS", "CONFIG","THEME" };
    string ARM_URL = string.Empty;
    string ARMSessionId = string.Empty;
    AnalyticsUtils _aUtils;

    public Entity()
    {
        _aUtils = new AnalyticsUtils();
        ARM_URL = _aUtils.ARM_URL;
        ARMSessionId = _aUtils.ARMSessionId;
    }

    public string GetEntityListPageLoadData(string page, string transId, int pageNo, int pageSize, List<Dictionary<string, Object>> filters, bool onlyData = true)
    {
        string apiUrl = ARM_URL + "/AxList/api/v1/GetEntityListPageLoadData";
        
        var inputJson = new
        {
            Page = page,
            ARMSessionId = ARMSessionId,
            OnlyData = onlyData,
            AxSessionId = HttpContext.Current.Session.SessionID,
            TransId = transId,
            Trace = _aUtils.GetTraceFlag(),
            AppName = HttpContext.Current.Session["project"].ToString(),
            Roles = HttpContext.Current.Session["AxRole"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString(),
            Language = HttpContext.Current.Session["language"].ToString(),
            PropertiesList = entityProperties,
            PageNo = pageNo,
            PageSize = pageSize,
            //ViewFilters = _aUtils.GetViewFilters(new List<string> { transId }),
            //GlobalParams = _aUtils.GetGlobalParams(),
            Filters = filters,
            CalledFromDataList = true
        };

        var analyticsData = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
        return analyticsData;
    }

    public string GetEntityEditableFlag(string page, string transId, string recordId, string action)
    {
        string apiUrl = ARM_URL + "/ARM_APIs/api/v1/AxGetEditableFlag";

        var inputJson = new
        {
            //Page = page,
            ARMSessionId = ARMSessionId,
            //AxSessionId = HttpContext.Current.Session.SessionID,
            TransId = transId,
            Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
            AppName = HttpContext.Current.Session["project"].ToString(),
            Roles = HttpContext.Current.Session["AxRole"].ToString(),
            UserName = HttpContext.Current.Session["username"].ToString(),
            SchemaName = HttpContext.Current.Session["dbuser"].ToString(),
            Language = HttpContext.Current.Session["language"].ToString(),
            //RecordId = Convert.ToInt64(recordId),
            KeyField = "recordid",
            KeyValue = recordId,
            Action = action
            //,
            //ViewFilters = _aUtils.GetViewFilters(new List<string> { transId }),
            //GlobalParams = _aUtils.GetGlobalParams()

        };

        var entityFlag = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
        return entityFlag;
    }
}
