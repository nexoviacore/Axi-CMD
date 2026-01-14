<%@ Page Language="C#" AutoEventWireup="true" CodeFile="iview.aspx.cs" Inherits="iview"
    EnableEventValidation="false" ValidateRequest="false" %>

<!DOCTYPE html>
<html>

<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="IView" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Iview</title>
    <!-- ________ CSS __________ -->
    <asp:PlaceHolder runat="server">
        <%:Styles.Render("~/Css/smartviews") %>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
    <!-- The Bundle CSS is loading after this file, which is why the iviewNewUi114.css link has been added here.  -->
    <link href="../Css/iviewNewUi114.css?v=3" rel="stylesheet" />
    <script>
        if (typeof localStorage != "undefined") {
            var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            var customStyleTimeStamp = localStorage["customIviewNewUiExist-" + projUrl];
            if (typeof customStyleTimeStamp != "undefined" && customStyleTimeStamp != null)
                customStyleTimeStamp = customStyleTimeStamp.replace(/[^a-zA-Z0-9_\-\/]/g, "");
            if (customStyleTimeStamp && customStyleTimeStamp != "false") {
                var appProjName = localStorage["projInfo-" + projUrl] || "";
                if (typeof appProjName != "undefined" && appProjName != null)
                    appProjName = appProjName.replace(/[^a-zA-Z0-9_\-\/]/g, "");
                var customReportStyle = "<link id=\"customIviewNewUi\" data-proj=\"" + appProjName + "\" href=\"../" + appProjName + "/customIviewNewUi.css?v=" + customStyleTimeStamp + "\" rel=\"stylesheet\" />";
                document.write(customReportStyle);
            }
        }
    </script>

    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <%--    <script>
        if (typeof localStorage != "undefined") {
            var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            var lsTimeStamp = localStorage["customGlobalStylesExist-" + projUrl]
            if (lsTimeStamp && lsTimeStamp != "false") {
                var appProjName = localStorage["projInfo-" + projUrl] || "";
                var customGS = "<link id=\"customGlobalStyles\" data-proj=\"" + appProjName + "\" href=\"../" + appProjName + "/customGlobalStyles.css?v=" + lsTimeStamp + "\" rel=\"stylesheet\" />";
                document.write(customGS);
            }
        }
    </script>
    <script>
        try {
            if (typeof localStorage != "undefined") {
                var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                var lsTimeStamp = localStorage["axGlobalThemeStyle-" + projUrl]
                if (lsTimeStamp && lsTimeStamp != "false") {
                    var axThemeFldr = localStorage["axThemeFldr-" + projUrl] || "";
                    var axCustomStyle = "<link id=\"axGlobalThemeStyle\" data-themfld=\"" + axThemeFldr + "\" href=\"../" + axThemeFldr + "/axGlobalThemeStyle.css?v=" + lsTimeStamp + "\" rel=\"stylesheet\" />";
                    document.write(axCustomStyle);
                }
            }
        } catch (ex) { }
    </script>--%>

    <script>
        (function () {
            if (typeof localStorage !== "undefined") {
                try {
                    let projUrl = top.window.location.href.toLowerCase().split("/aspx/")[0];
                    let lsTimeStamp = sanitizeInput(localStorage["customGlobalStylesExist-" + projUrl] || "");
                    let appProjName = sanitizeInput(localStorage["projInfo-" + projUrl] || "");
                    if (lsTimeStamp && lsTimeStamp !== "false" && appProjName) {
                        let linkElement = document.createElement("link");
                        linkElement.id = "customGlobalStyles";
                        linkElement.setAttribute("data-proj", appProjName);
                        linkElement.rel = "stylesheet";
                        let safeHref = encodeURI(`../${appProjName}/customGlobalStyles.css?v=${lsTimeStamp}`);
                        linkElement.href = safeHref;
                        document.head.appendChild(linkElement);
                    }

                    let themeTimeStamp = sanitizeInput(localStorage["axGlobalThemeStyle-" + projUrl] || "");
                    let axThemeFldr = sanitizeInput(localStorage["axThemeFldr-" + projUrl] || "");
                    if (themeTimeStamp && themeTimeStamp !== "false" && axThemeFldr) {
                        let themeLink = document.createElement("link");
                        themeLink.id = "axGlobalThemeStyle";
                        themeLink.setAttribute("data-themfld", axThemeFldr);
                        themeLink.rel = "stylesheet";
                        let safeHref = encodeURI(`../${axThemeFldr}/axGlobalThemeStyle.css?v=${themeTimeStamp}`);
                        themeLink.href = safeHref;
                        document.head.appendChild(themeLink);
                    }
                } catch (ex) {
                }
            }
            function sanitizeInput(input) {
                return input.replace(/[^a-zA-Z0-9_\-\/]/g, "");
            }
        })();
    </script>
    <!-- ________ JAVASCRIPT __________ -->

    <script type='text/javascript'>
        var gllang = '<%=langType%>';
        var parentArr = new Array();
        var typeArr = new Array();
        var depArr = new Array();
        var depParamArr = new Array();
        var hiddenArr = new Array();
        var Expressions = new Array();
        var exprSuggestions = new Array();
        var vExpressions = new Array();
        var paramType = new Array();
        var pCurArr = new Array();
        var sqlParamsArr = new Array();
        var columnDepArr = new Array();
        var ctrlType = "";
        var ctrlID = "";
        var childCtrlID = "";
        var oldChildCtrlID = "";
        var transid = "";
        var currFldValue = "";
        var Parameters = new Array();
        var AxMemParameters = new Array();
        var FNames = new Array();
        var fldNameArray = new Array();
        var fldValueArray = new Array();
        var fldDeletedArray = new Array();
        var IsNqIvTaskBtnCliked = false;
        var SaveWindow;
        var AxActiveField = "";
        var GridDispHead = false;
        var DCFrameNo = new Array();
        var TstructHasPop = false;
        var validateParamOnGo = false;
        var isChartsAvailable = true;
        var showMore = true;
        var isFromClearBtn = false;
        var IVIRCaption = "<%=breadCrumb.Replace("\"", "\\\"")%>";
        var printTitle = "<%=PrintTitle%>";
        var capturedButton1Submit = false;
        //below line will load the google charts

        var enableBackButton = false;
        var enableForwardButton = false;

        var allowRecPerPageChange = false;
        var lastSelRecPerPage = "";
        var hasBuildAccess = <%=hasBuildAccess.ToString().ToLower()%>;

        var requestProcess_logtime = '<%=requestProcess_logtime%>';
        var serverprocesstime = '<%=serverprocesstime%>';
        var rsaPublicKey = `<%=rsaPublicKey%>`.replace(/\\n/g, '\n');
    </script>

</head>

<body class="content p-0 stay-page-loading btextDir-<%=direction%>" dir="<%=direction%>" id="<%=iName %>">
    <form id="form1" class="frmiview" runat="server" onclick="javascript:HideNqIvTaskList();">
        <div>
            <asp:PlaceHolder runat="server">
                <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server">
                <%:Scripts.Render("~/Js/smartviews") %>
            </asp:PlaceHolder>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
                <Services>
                    <asp:ServiceReference Path="../WebService.asmx" />
                </Services>
                <Services>
                    <asp:ServiceReference Path="../CustomWebService.asmx" />
                </Services>
            </asp:ScriptManager>
        </div>

        <div id="iviewFrame" runat="server">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divfile" class="d-none">
                        <table class="table tblrowbor">
                            <tr>
                                <td class="rowbor">
                                    <asp:Label ID="LabelFs" runat="server" meta:resourcekey="LabelFs" Font-Bold="True"
                                        ForeColor="#1e90ff">File Name:</asp:Label>
                                    <input id="filMyFile" accept="text/html" type="file" />
                                    <input type="button" onclick="javascript: CallAfterFileUpload();" id="cafterfload"
                                        value="load" name="cafterfload" />
                                </td>
                            </tr>
                            <tr class="txtrowbar">
                                <td class="rowbor">
                                    <asp:TextBox ID="TextBox2" Width="1" runat="server" Visible="true"
                                        BorderStyle="None" ForeColor="white" BackColor="white"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <%if (hidetoolbar == "false")
                        {%>
                    <asp:Label ID="lblTime" runat="server"></asp:Label>
                    <asp:Label ID="lblTimeClient" runat="server"></asp:Label>
                    <div class="listViewDc d-flex flex-column flex-column-fluid vh-100 min-vh-100 overflow-auto border border-gray-300 rounded">
                        <%=strBreadCrumbBtns%>
                        <div class="toolbar m-0 mb-5--- p-0 py-1 border--- border-gray-300--- border-bottom-0--- rounded-top--- bg-light">
                            <div id="breadcrumb-panel" class="container-fluid d-flex flex-stack flex-wrap flex-sm-nowrap ps-5 p-0">
                                <div id="breadcrumb" class="d-flex flex-column align-items-start justify-content-center flex-wrap me-2 <%=bread_direction%>">
                                    <%=strBreadCrumb.ToString()%>
                                </div>

                                <div id="ivInSearch">
                                    <div class="searchBox d-flex align-items-center flex-nowrap text-nowrap">
                                        <% if (iviewButtonStyle == "old")
                                            { %>

                                        <div id="dvRefreshParam" class="newRequestJson me-2" title="Refresh Report" data-bs-target="#iconsNew">
                                            <button id="dvRefreshParamIcon" type="submit" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 tb-btn">
                                                <span class="material-icons material-icons-style">refresh</span>
                                            </button>
                                        </div>

                                        <div class="searchBoxChildContainer input-group">
                                            <span class="icon input-group-text p-2"><span class="material-icons material-icons-style material-icons-2" id="idsearch">search</span></span>
                                            <input name="ivInSearchInput" type="search" id="ivInSearchInput" class="form-control form-control-sm" />
                                        </div>
                                        <% } %>
                                    </div>

                                    <% if (iviewButtonStyle != "old")
                                        { %>
                                    <div id="searchBar" class="toolbarOptionsParent">
                                        <div id='iconsNew' class="d-flex">
                                            <div id="pinnedsearchBar" class="d-flex align-items-center flex-nowrap text-nowrap d-none">
                                                <div id="pinnedicons" class="">
                                                    <ul id="pinnediconsUl" class="d-flex p-0 m-0"></ul>
                                                </div>
                                            </div>
                                            <div class="d-flex align-items-center flex-nowrap text-nowrap toolbarRightMenu">
                                                <div id="dvRefreshParam" class="newRequestJson" title="Refresh Report" data-bs-target="#iconsNew">
                                                    <button id="dvRefreshParamIcon" type="submit" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 tb-btn">
                                                        <span class="material-icons material-icons-style">refresh</span>
                                                    </button>
                                                </div>
                                                <div id="iconsNewSearch" title="Search" class="menu-dropdown newRequestJson" data-kt-menu-placement="bottom-end">
                                                    <a id="iconsNewSearchIcon" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 tb-btn" onclick="$('.searchBoxChildContainer').removeClass('d-none');$('#ivInSearchInput').focus();">
                                                        <span class="material-icons material-icons-style">search</span>
                                                    </a>
                                                </div>
                                                <div id="iconsNewNew" data-bs-toggle="tooltip" data-bs-placement="bottom" data-bs-dismiss="click" data-bs-original-title="New" class="newRequestJson">
                                                    <a id="iconsNewNewIcon" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 tb-btn">
                                                        <span class="material-icons material-icons-style">add</span>
                                                    </a>
                                                </div>
                                                <div id="iconsNewRemove" data-bs-toggle="tooltip" data-bs-placement="bottom" data-bs-dismiss="click" data-bs-original-title="Remove" class="newRequestJson">
                                                    <a id="iconsNewRemoveIcon" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 tb-btn">
                                                        <span class="material-icons material-icons-style">remove</span>
                                                    </a>
                                                </div>
                                                <div id="filterWrapper" class="newRequestJson">
                                                </div>
                                                <div id="iconsNewUtility" title="Utility" class="newRequestJson">
                                                    <span id="iconsNewUtilityIcon" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 tb-btn" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                                        <span class="material-icons material-icons-style">miscellaneous_services</span>
                                                    </span>
                                                    <ul id="iconsExportUl" class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-200px py-3" data-kt-menu="true">
                                                        <% if (iviewButtonStyle == "modern")
                                                            {%>
                                                        <div class="card">
                                                            <div class="card-body py-5">
                                                                <div class="mh-450px scroll-y me-n5 pe-5">
                                                                    <div class="row g-2">
                                                                        <% } %>
                                                                        <li id="iconsExportPrint" class="menu-item px-3">
                                                                            <a title="Print" class="menu-link px-3" href="javascript:void(0);">
                                                                                <div class="symbol symbol-25px symbol-circle me-5 dropdownIconUI">
                                                                                    <span class="symbol-label bg-primary text-white fw-normal fs-3 iconUITitle">P</span>
                                                                                </div>
                                                                                <span class="dropdownIconName">Print
                                                                                </span>
                                                                            </a>
                                                                        </li>
                                                                        <li id="iconsExportPdf" class="menu-item px-3">
                                                                            <a title="PDF" class="menu-link px-3" href="javascript:void(0);">
                                                                                <div class="symbol symbol-25px symbol-circle me-5 dropdownIconUI">
                                                                                    <span class="symbol-label bg-primary text-white fw-normal fs-3 iconUITitle">P</span>
                                                                                </div>
                                                                                <span class="dropdownIconName">PDF
                                                                                </span>
                                                                            </a>
                                                                        </li>
                                                                        <li id="iconsExportExcel" class="menu-item px-3">
                                                                            <a title="Excel" class="menu-link px-3" href="javascript:void(0);">
                                                                                <div class="symbol symbol-25px symbol-circle me-5 dropdownIconUI">
                                                                                    <span class="symbol-label bg-primary text-white fw-normal fs-3 iconUITitle">E</span>
                                                                                </div>
                                                                                <span class="dropdownIconName">Excel
                                                                                </span>
                                                                            </a>
                                                                        </li>
                                                                        <li id="iconsExportCSV" class="menu-item px-3">
                                                                            <a title="CSV" class="menu-link px-3" href="javascript:SetDatatableExport('csv');">
                                                                                <div class="symbol symbol-25px symbol-circle me-5 dropdownIconUI">
                                                                                    <span class="symbol-label bg-primary text-white fw-normal fs-3 iconUITitle">C</span>
                                                                                </div>
                                                                                <span class="dropdownIconName">CSV
                                                                                </span>
                                                                            </a>
                                                                        </li>
                                                                        <li id="iconsExportHTML" class="menu-item px-3">
                                                                            <a title="HTML" class="menu-link px-3" href="javascript:SetDatatableExport('html');">
                                                                                <div class="symbol symbol-25px symbol-circle me-5 dropdownIconUI">
                                                                                    <span class="symbol-label bg-primary text-white fw-normal fs-3 iconUITitle">H</span>
                                                                                </div>
                                                                                <span class="dropdownIconName">HTML
                                                                                </span>
                                                                            </a>
                                                                        </li>
                                                                        <li id="iconsExporJSON" class="menu-item px-3">
                                                                            <a title="JSON" class="menu-link px-3" href="javascript:SetDatatableExport('json');">
                                                                                <div class="symbol symbol-25px symbol-circle me-5 dropdownIconUI">
                                                                                    <span class="symbol-label bg-primary text-white fw-normal fs-3 iconUITitle">J</span>
                                                                                </div>
                                                                                <span class="dropdownIconName">JSON
                                                                                </span>
                                                                            </a>
                                                                        </li>
                                                                        <li id="iconsExportCopy" class="menu-item px-3">
                                                                            <a title="Copy" class="menu-link px-3" href="javascript:SetDatatableExport('copy');">
                                                                                <div class="symbol symbol-25px symbol-circle me-5 dropdownIconUI">
                                                                                    <span class="symbol-label bg-primary text-white fw-normal fs-3 iconUITitle">C</span>
                                                                                </div>
                                                                                <span class="dropdownIconName">Copy
                                                                                </span>
                                                                            </a>
                                                                        </li>
                                                                        <li id="iconsExportWord" class="menu-item px-3">
                                                                            <a title="Word" class="menu-link px-3" href="javascript:SetDatatableExport('word');">
                                                                                <div class="symbol symbol-25px symbol-circle me-5 dropdownIconUI">
                                                                                    <span class="symbol-label bg-primary text-white fw-normal fs-3 iconUITitle">W</span>
                                                                                </div>
                                                                                <span class="dropdownIconName">Word
                                                                                </span>
                                                                            </a>
                                                                        </li>
                                                                        <% if (iviewButtonStyle == "modern")
                                                                            {%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <% } %>
                                                    </ul>
                                                </div>
                                                <div id="iconsNewOption" title="Options" class="newRequestJson">
                                                    <span id="iconsNewOptionIcon" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 tb-btn" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                                        <span class="material-icons material-icons-style">apps</span>
                                                    </span>
                                                    <ul id="iconsUl" class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-200px py-3" data-kt-menu="true">
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <% } %>
                                </div>

                                <div class="w-100 menu menu-sub menu-sub-dropdown menu-column bg-transparent position-fixed show d-none searchBoxChildContainer" data-popper-placement="bottom-end" data-kt-menu="true">
                                    <div class="icon me-5 bg-white rounded-3">
                                        <span class="material-icons material-icons-style position-absolute top-50 translate-middle-y ms-3" id="idsearch">search
                                        </span>
                                        <%--<input class="form-control form-control-flush ps-12" type="search" id="ivInSearchInput" />--%>
                                        <div class="d-flex">
                                            <input id="ivInSearchInput" type="search" class="form-control form-control-flush ps-12 me-3 flex-grow-1"
                                                name="search" value="" />

                                            <button id="ivInSearchInputButton" class="btn btn-light fw-bold flex-shrink-0"
                                                data-target="ivInSearchInput">
                                                <span id="ivInSearchInputButtonLoader" class="spinner-border spinner-border-sm align-middle me-2 d-none"></span>
                                                Search with All Records ?
                                            </button>
                                        </div>
                                    </div>
                                </div>

                                <div id="dvActions" class="d-none" runat="server">
                                    <dl id="dlAction" class="ddlBtn">
                                        <dt><a href="javascript:void(0)" onclick="ddToggle(event);">
                                            <img src="../AxpImages/icons/task.png" alt="task" />
                                            <asp:Label ID="lblaction" runat="server" meta:resourcekey="lblaction">select
                                                    an Action</asp:Label>
                                        </a></dt>
                                        <dd>
                                            <ul id="ddActionBtns" runat="server">
                                            </ul>
                                        </dd>
                                    </dl>
                                </div>
                            </div>
                        </div>
                        <div id="ivContainer" class="container-fluid d-flex align-items-center flex-nowrap text-nowrap p-0">
                            <div>
                                <% if (iviewButtonStyle == "old")
                                    {%>
                                <div id="searchBar" class="toolbarOptionsParent">
                                    <div id="icons" class="d-flex">
                                        <ul id="iconsUl" class="d-flex p-0 my-0 scroll-x vw-100"></ul>
                                    </div>
                                </div>
                                <% } %>
                                <span class="paramFiletrs float-end" runat="server" id="divFilterCond"></span>
                                <div id="dvPages" runat="server" class="d-none">
                                    <asp:Label ID="records" runat="server" Text="" CssClass="totrec">
                                    </asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="pgCap" Text="Page No." runat="server" CssClass="totrec">
                                        </asp:Label>
                                    <asp:DropDownList ID="lvPage" runat="server" AutoPostBack="true"
                                        CssClass="combotem" onchange="javascript:ShowDimmer(true);">
                                    </asp:DropDownList>
                                    <asp:Label ID="pages" Text="" runat="server" CssClass="totrec">
                                    </asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblDbRecsCount" runat="server" CssClass="totrec">
                                        </asp:Label>

                                </div>

                                <div id="dvStatictime" runat="server">
                                    <asp:Label ID="lblRefresh" CssClass="colorButton"
                                        runat="server"></asp:Label>
                                </div>

                                <div runat="server" id="dvStagLoad" class="d-none">
                                    <a class="cursor-pointer" onclick="javascript:GetStagRecords('More');">
                                        <asp:Label ID="lblshow" runat="server" meta:resourcekey="lblshow">
                                                Show More</asp:Label>
                                    </a>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <a class="cursor-pointer" onclick="javascript:GetStagRecords('All');">
                                            <asp:Label ID="lblshowall" runat="server"
                                                meta:resourcekey="lblshowall">Show All</asp:Label>
                                        </a>&nbsp;&nbsp;
                                </div>
                                <div id="dvSqlPages" runat="server">
                                    <div id="nextPrevBtns" runat="server" class="nextPrevBtns">
                                        <asp:LinkButton ID="lnkPrev" runat="server" title="Prev"
                                            OnClientClick="return CheckDisabled('lnkPrev');" />
                                        <asp:LinkButton ID="lnkNext" runat="server" title="Next"
                                            OnClientClick="return CheckDisabled('lnkNext');" />
                                    </div>
                                    <% if (!requestJSON)
                                        { %>
                                    <%--duplicate rowsTxtCount--%>
                                    <div id="rowsTxtCount" class="d-flex flex-row flex-column-fluid gap-5">
                                        <div class="d-flex flex-row-auto w-auto flex-center ms-auto">
                                            <label id="lblCurPage" class="d-flex flex-row-auto w-auto flex-center totrec">
                                            </label>
                                        </div>
                                        <div class="d-flex flex-row-auto w-auto flex-center">
                                            <label id="lblNoOfRecs" class="d-flex flex-row-auto w-auto flex-center totrec">
                                            </label>
                                        </div>
                                        <div id="lnkShowAll" data-bs-toggle="tooltip" data-bs-placement="bottom" data-bs-dismiss="click" data-bs-original-title="Show All Records" class="d-flex flex-row-auto w-auto flex-center rounded shadow-sm cursor-pointer" onclick="getAllRecords();">
                                            <span class="material-icons material-icons-style material-icons-2">play_for_work</span>
                                        </div>
                                    </div>
                                    <% } %>
                                </div>
                                <div id="dvRowsPerPage" runat="server" class="d-none">
                                    <asp:Label ID="lbRecPerPage" meta:resourcekey="lbRecPerPage"
                                        runat="server">Rows Per
                                            Page</asp:Label>&nbsp;&nbsp;
                                        <asp:DropDownList ID="recPerPage" AutoPostBack="false" runat="server"
                                            onchange="onRecPerPageChange(event);ShowDimmer(true);">
                                            <asp:ListItem Text="50" Value="50" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                            <asp:ListItem Text="200" Value="200"></asp:ListItem>
                                            <asp:ListItem Text="500" Value="500"></asp:ListItem>
                                            <asp:ListItem Text="ALL" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>


                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                            <!--begin::Accordion Parameters-->
                            <div class="accordion rounded w-100 position-absolute z-index-1" id="accordion" runat="server">
                                <div id="myFiltersBody" runat="server" class="accordion-item border-0">

                                    <div id="Filterscollapse" class="accordion-collapse collapse" data-bs-parent="#accordion" runat="server">
                                        <div class="accordion-body tblresX w-100">
                                            <div class="d-flex position-absolute top-0 end-0 mt-1 me-1 dvFiltersClose" data-bs-toggle="tooltip" data-bs-placement="bottom" data-bs-dismiss="click" data-bs-original-title="Close" onclick="$('#Filterscollapse').collapse('hide');">
                                                <div class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm">
                                                    <span class="material-icons material-icons-style material-icons-3">close
                                                    </span>
                                                </div>
                                            </div>
                                            <div id="dvParamCont" runat="server">
                                                <%=paramHtml.ToString()%>
                                            </div>
                                            <div class="d-flex flex-row flex-column-fluid mt-4">
                                                <div class="d-flex flex-row-fluid flex-centerr">
                                                    <asp:Button ID="button1" runat="server" OnClientClick="Javascript:return ValidateOnSubmit();" CssClass="btn btn-primary d-inline-flex align-items-center shadow-sm me-2" Text="Search" ToolTip="Search" data-bs-toggle="collapse" />
                                                    &nbsp;
                                                        <asp:Button ID="button2" runat="server" CssClass="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm" OnClick="button2_Click" Text="Clear" ToolTip="Clear" OnClientClick="Javascript:ClearParamValues();" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <!--end::Accordion Parameters-->
                        </div>
                        <div class="d-flex ivcaptions" id="ivCap1" runat="server" visible="false">
                        </div>
                        <div class="d-flex mainTabs mx-1--- mt-2--- border-bottom border-gray-300">
                            <div class="btn btn-icon btn-sm btn-white btn-color-gray-500 btn-active-primary shadow-sm me-2 float-start scroller scroller-left d-none">
                                <span class="material-icons material-icons-style">chevron_left
                                </span>
                            </div>
                            <div class="btn btn-icon btn-sm btn-white btn-color-gray-500 btn-active-primary shadow-sm me-2 float-end scroller scroller-right d-none">
                                <span class="material-icons material-icons-style">chevron_right
                                </span>
                            </div>
                            <div id="dvViewtabs">
                                <ul class="cursor-pointer nav nav-tabs mb-n2 mt-2" id="viewTabs">
                                </ul>
                            </div>

                        </div>
                        <div class="card card-xl-stretch mb-1 mb-xl-2 shadow-sm--- border--- border-gray-300--- iviewCardContentWrapper flex-root h-1px mx-1---">
                            <div class="card-body pt-0--- p-2 m-1 mb-1 h-300px iviewTableWrapper">
                                <% if (requestJSON)
                                    { %>
                                <div class="d-flex flex-row flex-column-fluid pt-5--- pb-0--- pb-2 pageInformationNew gap-5">
                                    <%-- <div class="d-flex flex-row-fluid pageInformationLeft gap-10" style="display: inline-block; max-width: 100vw; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">--%>
                                    <div class="d-flex flex-row-fluid pageInformationLeft gap-10" style="width: 100%; max-width: 100%; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                        <div class="paramPillContent overflow-hidden--" style="display: inline-block; max-width: 100%; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                            <div id="dvSelectedFilters" class="badge badge-primary cursor-pointer d-none">
                                                <span id="FilterValues"></span>
                                            </div>
                                            <span class="ms-2 text-gray-800 tooltiptext"></span>
                                        </div>
                                        <div class="filterPillContent"></div>
                                    </div>
                                    <div id="rowsTxtCountNew" class="d-flex flex-row-auto w-auto flex-center pe-5 pageInformationRight">
                                        <div id="ivirAllCndtnPillsDiv" class="d-none pillPopover">
                                            <a title="" class="btn change-trigger" id="lnkallCndtnPill">
                                                <span class="material-icons material-icons-style">brush</span>
                                            </a>

                                            <div class="d-none" id="ivirAllCndtnPillsList">
                                            </div>
                                        </div>
                                        <div id="ivirChartPillsDiv" class="d-flex mx-2 d-none pillPopover">
                                            <a title="" class="btn btn-sm btn-icon btn-active-light-primary shadow-sm change-trigger" id="lnkChartPill" href="javascript:void(0);" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                                <span class="material-icons material-icons-style">insert_chart</span>
                                            </a>
                                            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-200px py-3" data-kt-menu="true" data-popper-placement="bottom-end" id="ivirChartPillsList">
                                            </div>
                                        </div>
                                        <% if (requestJSON == true)
                                            { %>
                                        <%--duplicate rowsTxtCount--%>
                                        <div id="rowsTxtCount" class="d-flex flex-row flex-column-fluid gap-5">
                                            <div class="d-flex flex-row-auto w-auto flex-center ms-auto">
                                                <label id="lblCurPage" class="d-flex flex-row-auto w-auto flex-center totrec">
                                                </label>
                                            </div>
                                            <div class="d-flex flex-row-auto w-auto flex-center">
                                                <label id="lblNoOfRecs" class="d-flex flex-row-auto w-auto flex-center totrec">
                                                </label>
                                            </div>
                                            <div id="lnkShowAll" data-bs-toggle="tooltip" data-bs-placement="bottom" data-bs-dismiss="click" data-bs-original-title="Show All Records" class="d-flex flex-row-auto w-auto flex-center rounded shadow-sm cursor-pointer" onclick="getAllRecords();">
                                                <span class="material-icons material-icons-style material-icons-2">play_for_work</span>
                                            </div>
                                        </div>
                                        <% } %>
                                    </div>

                                </div>
                                <hr class="d-none" />

                                <% } %>

                                <div class="myview d-none" id="divFilterView">
                                    <div class="myViewHeader">
                                        <h3>
                                            <asp:Label ID="lblview" runat="server" meta:resourcekey="lblview">Create View
                                            </asp:Label>
                                        </h3>
                                    </div>
                                    <div class="myviewname">
                                        <asp:Label ID="lblviewname" runat="server" CssClass="form-label fw-boldest" meta:resourcekey="lblviewname">Name
                                        </asp:Label>
                                    </div>
                                    <div class="myviewtext">
                                        <asp:TextBox ID="txtViewName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="myviewcancelbtn">
                                        <asp:Button ID="btnViewCancel" runat="server" CssClass="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2" OnClientClick="CloseViewPopup();"
                                            Text="Cancel" />
                                    </div>
                                </div>
                                <div class="dvContent">

                                    <%}
                                        else
                                        { %>

                                    <div>
                                        <%} %>
                                        <div class="sort d-none">
                                            <asp:Button ID="SortGrid" runat="server" />
                                            <asp:Button ID="FilterGrid" runat="server" />
                                            <asp:Button ID="HideColumn" runat="server" />


                                            <asp:HiddenField ID="hdnHideColName" runat="server" />
                                        </div>

                                        <asp:HyperLink ID="btnDownloadAll" runat="server" Text="Download Selected"
                                            Visible="true" Enabled="true" NavigateUrl="Javascript:DownloadSelectedFiles();"
                                            CssClass="d-none" />
                                        <div id="leftPanel" class="d-none" runat="server">
                                            <div id="paramCont" runat="server" class="wBdr Pagebody">
                                                <asp:HiddenField ID="hdnbElapsTimeGo" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnparamValues" runat="server" />
                                                <asp:HiddenField ID="rXml" runat="server" />
                                                <asp:HiddenField ID="param" runat="server" Value="" />
                                                <asp:HiddenField ID="paramxml" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnisIviewParamCache" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnGo" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnAct" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnIsPostBack" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnSRows" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnParamChngd" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnIvRefresh" runat="server" Value="" />
                                                <input type="hidden" name="recordid000F0" value="0" />
                                                <asp:HiddenField ID="hdnJsonData" runat="server" />
                                                <asp:HiddenField ID="hdnSortColName" runat="server" />
                                                <asp:HiddenField ID="hdnFilterColName" runat="server" />
                                                <asp:HiddenField ID="hdnFilterBy" runat="server" />
                                                <asp:HiddenField ID="hdnNoOfRecords" runat="server" />
                                                <asp:HiddenField ID="hdnGridColumns" runat="server" />
                                                <asp:HiddenField ID="hdnHideColumns" runat="server" />
                                                <table id="tblFilterCols">
                                                </table>
                                                <asp:HiddenField ID="hdnFilterChecked" runat="server" />
                                                <asp:HiddenField ID="hdnSortColumns" runat="server" />
                                                <asp:HiddenField ID="hdnMyViewName" runat="server" />
                                                <asp:HiddenField ID="hdnMyViewFilterCol" runat="server" />
                                                <asp:HiddenField ID="hdnIViewShow" runat="server" />
                                                <asp:HiddenField ID="hdnTotalIViewRec" runat="server" />
                                                <asp:HiddenField ID="hdnHasTotalRows" runat="server" />
                                                <asp:HiddenField ID="hdnHideShowCol" runat="server" />
                                                <asp:HiddenField ID="hdnHideShowFld" runat="server" />
                                                <asp:HiddenField ID="hdnIsParaVisible" runat="server" />
                                                <asp:HiddenField ID="hdnTotalRec" runat="server" />
                                                <asp:HiddenField ID="printRowsMaxLimitField" runat="server" />
                                                <asp:HiddenField ID="hdnShowParams" runat="server" />
                                                <asp:HiddenField ID="hdnIsDirectDB" runat="server" />
                                                <asp:HiddenField ID="hdnIsPerfXml" runat="server" Value="false" />
                                                <asp:HiddenField ID="hdnSelParamsAftrChWin" runat="server" />
                                                <asp:HiddenField ID="hdnAutoSplit" runat="server" />
                                                <asp:HiddenField ID="hdnDisableSplit" runat="server" />
                                                <asp:HiddenField ID="hdnWebServiceViewName" runat="server" />

                                                <asp:HiddenField ID="hdnListViewFieldsJSON" runat="server" />
                                                <asp:HiddenField ID="hdnLvSelectedCols" runat="server" />
                                                <asp:HiddenField ID="hdnLvSelectedHyperlink" runat="server" />
                                                <asp:HiddenField ID="hdnLvChangedStructure" runat="server" />

                                                <input type="hidden" id="cb_sactbu" name="cb_sactbu" />
                                            </div>
                                            <div>
                                                <h3 onclick="UpdateActiveArray(this);" id="ivSortHeader">
                                                    <asp:Label ID="lblsort" runat="server" meta:resourcekey="lblsort"> Sort
                                                    By</asp:Label>
                                                </h3>
                                                <div id="ivSortContent">
                                                    <ul id="sortIvByColumn">
                                                    </ul>
                                                </div>
                                            </div>
                                            <div>
                                                <h3 id="HideCol" onclick="UpdateActiveArray(this);">
                                                    <asp:Label ID="lblhidecolumn" runat="server"
                                                        meta:resourcekey="lblhidecolumn">Hide Column</asp:Label>
                                                </h3>
                                            </div>
                                        </div>
                                        <div id="contentPanelGridIview" runat="server">
                                            <div id="divcontainer" class="dvIvCont" runat="server">
                                                <div id="dvStatus" runat="server" class="error d-none">
                                                    <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                                                </div>
                                                <div id="dvPills">
                                                    <div id="pillsWrapper"></div>
                                                    <div id="ivirChartPills" class="text-center"></div>
                                                </div>

                                                <div class="tab-content border border-gray-300 rounded overflow-hidden">

                                                    <div id="dvData" runat="server">

                                                        <asp:Panel runat="server" ID="Panel1" Width="100%" CssClass="pnlgrid">
                                                            <div id="ivirMainChartWrapper">

                                                                <div id="ivirChartWrapper"></div>
                                                            </div>
                                                            <asp:GridView CellSpacing="-1" ID="GridView1" runat="server"
                                                                OnPreRender="GridView1_PreRender" AutoGenerateColumns="False"
                                                                CssClass="gridData">
                                                            </asp:GridView>
                                                            <div id="GridView2Wrapper" class="bg-white" runat="server" visible="false">
                                                            </div>
                                                            <table class="animationLoading animationLoadingHide">
                                                                <thead>
                                                                    <tr>
                                                                        <th></th>
                                                                        <th></th>
                                                                        <th></th>
                                                                        <th></th>
                                                                        <th></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                        <td></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </asp:Panel>

                                                        <div id="dvFilteredRowCount" runat="server">
                                                            <span>
                                                                <asp:Label ID="lblFilteredRowCount" runat="server"
                                                                    CssClass="totrec"></asp:Label>
                                                                <asp:Label ID="lblTotalNo" runat="server" Visible="false"
                                                                    meta:resourcekey="lblTotalNo">Total no. of filtered Rows :
                                                                </asp:Label>
                                                            </span>
                                                        </div>
                                                        <div id="divCustomAct" visible="false" runat="server" class="wBdr">
                                                            <div id="divActHeader"
                                                                class="dcHeaderBar">
                                                                <a href='javascript:ShowAndHideActDiv("dvActivityInfo");'>
                                                                    <span id="dcButspan3">
                                                                        <img id="imgAct" border="0" alt="show"
                                                                            src="../AxpImages/icons/16x16/expandwt.png" />
                                                                    </span>
                                                                </a>
                                                                <font class="heading">Recent
                                                            activities</font>
                                                            </div>
                                                            <div id="dvActivityInfo"
                                                                runat="server">
                                                                <center>
                                                                    <table>

                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <table>

                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table>
                                                                                                        <tbody>
                                                                                                            <tr>
                                                                                                                <td
                                                                                                                    colspan="2">
                                                                                                                    <asp:GridView
                                                                                                                        CellSpacing="-1"
                                                                                                                        ID="grvActivities"
                                                                                                                        runat="server"
                                                                                                                        BackColor="White"
                                                                                                                        CssClass="labelcap"
                                                                                                                        BorderStyle="Groove"
                                                                                                                        BorderWidth="2px"
                                                                                                                        CellPadding="4">
                                                                                                                        <RowStyle
                                                                                                                            BackColor="White"
                                                                                                                            BorderStyle="Groove"
                                                                                                                            BorderWidth="2px" />
                                                                                                                        <FooterStyle
                                                                                                                            BackColor="#99CCCC"
                                                                                                                            ForeColor="#003399" />
                                                                                                                        <PagerStyle
                                                                                                                            BackColor="#99CCCC"
                                                                                                                            ForeColor="#003399"
                                                                                                                            HorizontalAlign="Left" />
                                                                                                                        <SelectedRowStyle
                                                                                                                            BackColor="#009999"
                                                                                                                            Font-Bold="True"
                                                                                                                            ForeColor="#CCFF99" />
                                                                                                                        <HeaderStyle
                                                                                                                            BackColor="#c3d9ff"
                                                                                                                            Font-Bold="True" />
                                                                                                                    </asp:GridView>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </tbody>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </center>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="noRecccord" class="d-none" runat="server">
                                                <asp:Label ID="lblNodata" runat="server" meta:resourcekey="lblNodata">No
                                                data found.</asp:Label>
                                            </div>
                                        </div>

                                        <div id="dvCharts" class="d-none dvChart">
                                            <div class="dvChartCont">
                                                <a href="javascript:void(0)" onclick="javascript:CloseChart();">
                                                    <img alt="close" src="../AxpImages/icons/close-button.png" /></a>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span>
                                                                <asp:RadioButtonList ID="rbtnChartType" runat="server"
                                                                    RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="line" Selected="True">Line
                                                                    </asp:ListItem>
                                                                    <asp:ListItem Value="bar">Bar</asp:ListItem>
                                                                    <asp:ListItem Value="pie">Pie</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </span>
                                                        </td>
                                                        <td>
                                                            <a id="lnkChart" href="javascript:ShowChartPopUp();">View Full
                                                            Screen</a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Xaxis" runat="server" meta:resourcekey="Xaxis">
                                                            X-Axis</asp:Label>
                                                            <asp:DropDownList ID="ddlChartCol1"
                                                                runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Yaxis" runat="server" meta:resourcekey="Yaxis">
                                                            Y-Axis</asp:Label>
                                                            <asp:DropDownList ID="ddlChartCol2"
                                                                runat="server">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="btnGetChart1" runat="server"
                                                                Text="Go" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="chart_div">
                                            </div>
                                        </div>

                                        <asp:HiddenField ID="hdnParamHtml" runat="server" />
                                        <asp:HiddenField ID="hdnChartSize" runat="server" />
                                        <asp:HiddenField ID="hdnKey" runat="server" />
                                        <asp:HiddenField ID="hdnStagTableNo" runat="server" />
                                        <asp:HiddenField ID="hdnToggleDimmer" runat="server" Value="nothing" />
                                        <div id="bg" class="popup_bg">
                                        </div>
                                    </div>

                                    <div id="dvNoData" class="d-flex flex-column text-center h-100-- justify-content-center d-none">
                                        <span class="material-icons material-icons-style material-icons-5tx mx-auto text-gray-500" style="color: red !important;">report</span>
                                        <h3 class="fw-boldest">No Data Found</h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
            <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                <span class="loader"></span>
            </div>
        </div>
        <%=strJsArrays%>
        <%=tst_Scripts%>
        <%=enableBackForwButton%>

        <asp:Label ID="lblNodataServer" CssClass="d-none" runat="server" meta:resourcekey="lblNodataServer" Visible="false">No data found.
        </asp:Label>
        <asp:Label ID="lblTaskExcel" CssClass="d-none" runat="server" meta:resourcekey="lblTaskExcel">Excel
        </asp:Label>
        <asp:Label ID="lblTaskPDF" CssClass="d-none" runat="server" meta:resourcekey="lblTaskPDF">PDF</asp:Label>
    </form>
</body>

</html>
