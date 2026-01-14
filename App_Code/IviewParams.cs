using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for IviewParams
/// </summary>
/// 
[Serializable()]
public class IviewParams
{

    //public bool isObjFromCache = false;
    public bool showParam = false;
    public string strJsArrays = string.Empty;
    public ArrayList iviewParams = new ArrayList();
    public ArrayList iviewParamValues = new ArrayList();
    public ArrayList arrVisibleParamList = new ArrayList();
    public bool paramsBound = true;
    public string axpResp = "false";
    //param string for cached data = "";
    public string paramCacheString = "";

    private bool paramsExist;
    public bool ParamsExist
    {
        get { return paramsExist; }
        set { paramsExist = value; }
    }

    private ArrayList paramNames;
    public ArrayList ParamNames
    {
        get { return paramNames; }
        set { paramNames = value; }
    }

    private ArrayList paramCaption;
    public ArrayList ParamCaption
    {
        get { return paramCaption; }
        set { paramCaption = value; }
    }

    private ArrayList paramValsOnLoad;
    public ArrayList ParamValsOnLoad
    {
        get { return paramValsOnLoad; }
        set { paramValsOnLoad = value; }
    }

    private ArrayList paramChangedVals;
    public ArrayList ParamChangedVals
    {
        get { return paramChangedVals; }
        set { paramChangedVals = value; }
    }

    private ArrayList paramNameType;
    public ArrayList ParamNameType
    {
        get { return paramNameType; }
        set { paramNameType = value; }
    }

    private bool forceDisableCache = false;
    public bool ForceDisableCache
    {
        get { return forceDisableCache; }
        set { forceDisableCache = value; }
    }

    private bool noVisibleParam = true;
    public bool NoVisibleParam
    {
        get { return noVisibleParam; }
        set { noVisibleParam = value; }
    }

    private ArrayList arrParamType;
    public ArrayList ArrParamType
    {
        get { return arrParamType; }
        set { arrParamType = value; }
    }

    private bool isParameterExist = false;
    public bool IsParameterExist
    {
        get { return isParameterExist; }
        set { isParameterExist = value; }
    }

    private StringBuilder paramHtml;
    public StringBuilder ParamHtml
    {
        get { return paramHtml; }
        set { paramHtml = value; }
    }

    private string axp_refresh;
    public string Axp_refresh
    {
        get { return axp_refresh; }
        set { axp_refresh = value; }
    }

    private string paramXml = string.Empty;
    public string ParamXML
    {
        get { return paramXml; }
        set { paramXml = value; }
    }

    private string iviewParamString;
    public string IviewParamString
    {
        get { return iviewParamString; }
        set { iviewParamString = value; }
    }


    //private bool retainIviewParams = false;
    //public bool RetainIviewParams
    //{
    //    get { return retainIviewParams; }
    //    set { retainIviewParams = value; }
    //}

    private bool isFilterEnabled;
    public bool IsFilterEnabled
    {
        get { return isFilterEnabled; }
        set { isFilterEnabled = value; }
    }

    private bool isIviewParamcache = true;
    public bool isIviewParamCache
    {
        get { return isIviewParamcache; }
        set { isIviewParamcache = value; }
    }

    private bool isIviewParamClearcache = false;
    public bool isIviewParamClearCache
    {
        get { return isIviewParamClearcache; }
        set { isIviewParamClearcache = value; }
    }

    public IviewParams()
    {
        //
        // TODO: Add constructor logic here
        //
        paramNames = new ArrayList();
        paramCaption = new ArrayList();
        paramNameType = new ArrayList();
        paramValsOnLoad = new ArrayList();
        paramChangedVals = new ArrayList();

        ArrParamType = new ArrayList();


    }
}
