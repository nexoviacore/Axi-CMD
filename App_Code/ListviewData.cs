using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;

/// <summary>
/// Summary description for ListviewData
/// </summary>
/// 
[Serializable()]
public class ListviewData
{
    public ListviewData()
    {
        //
        // TODO: Add constructor logic here
        //
        LvKey = string.Empty;
        LName = string.Empty;
        HeadRowXML = string.Empty;
        SortColumnName = string.Empty;
        SortColumn = string.Empty;
        SortDir = string.Empty;
        Mode = string.Empty;
        ViewList = string.Empty;
        Views = string.Empty;
        LviewCaption = string.Empty;
        TaskList = string.Empty;
        MenuBreadcrumb = string.Empty;
        Tid = string.Empty;
        CustomView = string.Empty;
        CustViewTransId = string.Empty;
        FilterText = string.Empty;
        FilterXml = string.Empty;
        Listres = string.Empty;

        HeadName = new ArrayList();
        CustViews = new ArrayList();
        DefaultfldName = new ArrayList();
        ColWidth = new ArrayList();
        ColType = new ArrayList();
        ColHide = new ArrayList();
        ColDec = new ArrayList();
        ColHlink = new ArrayList();
        ColMap = new ArrayList();
        CancelFlag = new ArrayList();
        ColFld = new ArrayList();
        HiddenCols = new ArrayList();
        ShowHiddengridCols = new ArrayList();
        colAlign = new ArrayList();
        DtListData = new DataTable();
        DtCurrentdata = new DataTable();

        IsPerfXml = false;
        IsWordWithHtml = false;
        IviewParamString = string.Empty;
        ReportHdrs = new ArrayList();
        ReportHdrStyles = new ArrayList();
        ReportFtrs = new ArrayList();
        ReportFtrStyles = new ArrayList();
        IsIviewStagLoad = false;
    }

    #region Private Variables

    private string lvKey;
    private string lName;
    private string toolBarBtns;
    private string gridPageSize;
    private string gridPageNo;
    private string ires;
    private string filterCondition;
    private string headRowXML;
    private string sortColumnName;
    private string sortColumn;
    private string sortDir;
    private string mode;
    private string viewList;
    private string views;
    private string lviewCaption;
    private string taskList;
    private string menuBreadcrumb;
    private string tid;
    private string filterText;

    private bool isPerfXml;

    private string customView;
    private string custViewTransId;

    private int lv_noofpages;

    private ArrayList custViews;
    private ArrayList headName;
    private ArrayList defaultfldName;
    private ArrayList colWidth;
    private ArrayList colType;
    private ArrayList colHide;
    private ArrayList colDec;
    private ArrayList colHlink;
    private ArrayList colMap;
    private ArrayList colFld;
    private ArrayList cancelFlag;
    private ArrayList yesNoColumns;
    private ArrayList showHiddenCols;
    private ArrayList showHiddengridCols;
    private ArrayList hiddenCols;
    private ArrayList colAlign;
    private DataTable dtListData;
    private DataTable dtCurrentdata;
    private ArrayList reportHdrs;
    private ArrayList reportHdrStyles;
    private ArrayList reportFtrs;
    private ArrayList reportFtrStyles;
    private string iviewParamString;

    private string filterXml;
    private string listres;

    private bool isDirectDBcall;
    /* Word*/
    private bool isWordWithHtml;
    private string iviewCaption;
    private bool isIviewStagLoad;

    #endregion

    #region Public Methods

    public string LName
    {
        get { return lName; }
        set { lName = value; }
    }

    public string FilterText
    {
        get { return filterText; }
        set { filterText = value; }
    }
    public ArrayList HeadName
    {
        get { return headName; }
        set { headName = value; }
    }

    public ArrayList DefaultfldName
    {
        get { return defaultfldName; }
        set { defaultfldName = value; }
    }

    public ArrayList ColWidth
    {
        get { return colWidth; }
        set { colWidth = value; }
    }

    public ArrayList ColType
    {
        get { return colType; }
        set { colType = value; }
    }

    public ArrayList ColHide
    {
        get { return colHide; }
        set { colHide = value; }
    }

    public ArrayList ColDec
    {
        get { return colDec; }
        set { colDec = value; }
    }

    public ArrayList ColHlink
    {
        get { return colHlink; }
        set { colHlink = value; }
    }

    public ArrayList ColMap
    {
        get { return colMap; }
        set { colMap = value; }
    }

    public ArrayList CancelFlag
    {
        get { return cancelFlag; }
        set { cancelFlag = value; }
    }
    public ArrayList ColFld
    {
        get { return colFld; }
        set { colFld = value; }
    }

    public ArrayList ColAlign
    {
        get { return colAlign; }
        set { colAlign = value; }
    }
    public string ToolBarBtns
    {
        get { return toolBarBtns; }
        set { toolBarBtns = value; }
    }

    public string GridPageSize
    {
        get { return gridPageSize; }
        set { gridPageSize = value; }
    }

    public string GridPageNo
    {
        get { return gridPageNo; }
        set { gridPageNo = value; }
    }

    public ArrayList YesNoColumns
    {
        get { return yesNoColumns; }
        set { yesNoColumns = value; }
    }

    public string Ires
    {
        get { return ires; }
        set { ires = value; }
    }

    public ArrayList ShowHiddenCols
    {
        get { return showHiddenCols; }
        set { showHiddenCols = value; }
    }

    public ArrayList HiddenCols
    {
        get { return hiddenCols; }
        set { hiddenCols = value; }
    }
    public ArrayList CustViews
    {
        get { return custViews; }
        set { custViews = value; }
    }

    public string FilterCondition
    {
        get { return filterCondition; }
        set { filterCondition = value; }
    }
    public string HeadRowXML
    {
        get { return headRowXML; }
        set { headRowXML = value; }
    }

    public DataTable DtListData
    {
        get { return dtListData; }
        set { dtListData = value; }
    }

    public ArrayList ShowHiddengridCols
    {
        get { return showHiddengridCols; }
        set { showHiddengridCols = value; }
    }

    public ArrayList ReportHdrs
    {
        get { return reportHdrs; }
        set { reportHdrs = value; }
    }

    public ArrayList ReportHdrStyles
    {
        get { return reportHdrStyles; }
        set { reportHdrStyles = value; }
    }
    public ArrayList ReportFtrs
    {
        get { return reportFtrs; }
        set { reportFtrs = value; }
    }
    public ArrayList ReportFtrStyles
    {
        get { return reportFtrStyles; }
        set { reportFtrStyles = value; }
    }
    public string SortColumnName
    {
        get { return sortColumnName; }
        set { sortColumnName = value; }
    }

    public string SortColumn
    {
        get { return sortColumn; }
        set { sortColumn = value; }
    }

    public string SortDir
    {
        get { return sortDir; }
        set { sortDir = value; }
    }

    public bool IsPerfXml
    {
        get { return isPerfXml; }
        set { isPerfXml = value; }
    }
    public string Mode
    {
        get { return mode; }
        set { mode = value; }
    }

    public string LviewCaption
    {
        get { return lviewCaption; }
        set { lviewCaption = value; }
    }

    public string TaskList
    {
        get { return taskList; }
        set { taskList = value; }
    }

    public string MenuBreadcrumb
    {
        get { return menuBreadcrumb; }
        set { menuBreadcrumb = value; }
    }

    public string Tid
    {
        get { return tid; }
        set { tid = value; }
    }

    public string CustViewTransId
    {
        get { return custViewTransId; }
        set { custViewTransId = value; }
    }

    public string CustomView
    {
        get { return customView; }
        set { customView = value; }
    }

    public int Lv_noofpages
    {
        get { return lv_noofpages; }
        set { lv_noofpages = value; }
    }

    public string ViewList
    {
        get { return viewList; }
        set { viewList = value; }
    }

    public string Views
    {
        get { return views; }
        set { views = value; }
    }

    public string LvKey
    {
        get { return lvKey; }
        set { lvKey = value; }
    }

    public string FilterXml
    {
        get { return filterXml; }
        set { filterXml = value; }
    }

    public string Listres
    {
        get { return listres; }
        set { listres = value; }
    }


    public bool IsDirectDBcall
    {
        get { return isDirectDBcall; }
        set { isDirectDBcall = value; }
    }
    public bool IsWordWithHtml
    {
        get { return isWordWithHtml; }
        set { isWordWithHtml = value; }
    }
    public DataTable DtCurrentdata
    {
        get { return dtCurrentdata; }
        set { dtCurrentdata = value; }
    }
    public string IviewParamString
    {
        get { return iviewParamString; }
        set { iviewParamString = value; }
    }

    public string IviewCaption
    {
        get { return iviewCaption; }
        set { iviewCaption = value; }
    }
    public bool IsIviewStagLoad
    {
        get { return isIviewStagLoad; }
        set { isIviewStagLoad = value; }
    }

    public string exportVerticalAlign = "middle";

    #endregion


}

   

