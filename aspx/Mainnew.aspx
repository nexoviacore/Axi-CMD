<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Mainnew.aspx.cs" Inherits="aspx_Mainnew" EnableEventValidation="false" %>

<!DOCTYPE html>
<html>

<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert Tstruct" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <!-- <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script> -->

    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script src="../Js/thirdparty/jquery-ui/jquery-ui.min.js" type="text/javascript"></script>

    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>

    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>

    <script type="text/javascript" src="../Js/iFrameHandler.min.js"></script>

    <!-- <script>
        if(typeof localStorage != "undefined"){
            var projUrl =  top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            var lsTimeStamp = localStorage["customGlobalStylesExist-" + projUrl]
            if(lsTimeStamp && lsTimeStamp != "false"){
                var appProjName = localStorage["projInfo-" + projUrl] || "";
                var customGS = "<link id=\"customGlobalStyles\" data-proj=\""+ appProjName +"\" href=\"../"+ appProjName +"/customGlobalStyles.css?v="+ lsTimeStamp +"\" rel=\"stylesheet\" />";
                document.write(customGS);
            }
        }
    </script> -->
    <%--<script>
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
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <script src="../Js/thirdparty/jquery-resizable.min.js?v=2" type="text/javascript"></script>
    <title><%=appTitle%></title>
    <link rel="shortcut icon" href="../images/favicon.ico" />
    <script src="../Js/xmlToJson.js?v=2"></script>
    <script src="../Js/helper.min.js?v=175"></script>
    <script src="../assets/js/loadingoverlay.min.js?v=3" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/common.min.js?v=164"></script>
    <script src="../Js/process.min.js?v=357"></script>
    <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>
    <script src="../ThirdParty/QRcode/qrcode.js?v=1"></script>

    <script type="text/javascript">
        history.go(1); // disable the browser's back button
        var thmProj = "";
        var thmUser = "";
        var thmSid = "";
        var thmSelected = "";
        var helpOpen = false;
        var gl_language = "";
        var childWindowHandler = new Array(); //to handle child popups on main close
        var globalChange = false;
        var tstructPop = false; //to reload LView on updation of record using popup
        var listViewPage = "0"; //which page to refresh
        var clickedColumn = ""; //column name of which data has been clicked in LView
        var dataRowIndex = -1; //to know row index of data clicked
        var isSessionCleared = true; //to clear navigation session        
        var isParentIview = false; // to check if tstruct loaded from iview
        var homeHref = ""; // to save the iview url
        var disableNavigation = false;
        var AxIvFilterMinHeader = new Array(); // to push all the minimized pannel of iview and lview filter
        var isLockOnRead = false;
        var doPageUnload = true;
        var gllangauge = '<%=language%>';
        var gllangType = '<%=langType%>';
        var glCulture = '<%=glCulture%>';
        var gimportIsOpen = false;
        var uName = '<%= Session["user"] %>';
        var uOwner = '<%=Session["isowner"] %>'
        var AxHelpIview;
        var ivname;
        var cm = null;
        var nodeApi = "<%=utilNode%>";
        var utl = "<%=Utl%>";
        var redisUtl = "<%=redisUtl%>";
        //var currentThemeColor = '<%=themeColor%>';
        var currentThemeColor = "";
        var xmlMenuData = '<%=menuXmlData%>';
        var axMenuStyle = '<%=Session["AxMenuStyle"]==null?"":Session["AxMenuStyle"].ToString()%>';
        var axMenuCols = '<%=Session["AxMenuColumns"]==null?"":Session["AxMenuColumns"].ToString()%>';
        var axMenuPerView = '<%=Session["AxSubmenuPerView"]==null?"":Session["AxSubmenuPerView"].ToString()%>';
        var axMenuWordWrap = '<%=Session["AxMenuWordWrap"]==null?"": Session["AxMenuWordWrap"].ToString()%>';
        var axSubMenuCnt = '<%=Session["AxSubMenuCount"]==null?"": Session["AxSubMenuCount"].ToString()%>';
        var axDirSubMenuCnt = '<%=Session["AxDirectSubMenuCount"]==null?"": Session["AxDirectSubMenuCount"].ToString()%>';
        var axAttachmentSize = '<%=Session["AxAttachmentSize"] == null? "1" :(Session["AxAttachmentSize"].ToString()==""?"1":Session["AxAttachmentSize"].ToString())%>';
        var appsessionKey = '<%=appsessionKey%>';
        var hdnpsid = '<%=hdnpsid%>';
        var PeriodicRCKey = '<%=PeriodicRCKey%>';
        var nodeAccessToken;
        var PageBuilderAccess = '<%=PageBuilderAccess%>';
        PageBuilderAccess = PageBuilderAccess.split(',');
        var hasPageBuildAccess = false;
        var AxUserRoles = '<%=AxRole%>';
        var prevMid1FrameUrl = "";
        var currAxpFrmUrl = "";
        var GlobalSrchCondition = "Contains";
        var axMainPageReload = '<%=axMainPageReload%>';
        var axMenuReSize = '<%=axMenuReSize%>';
        var enableTemplatization = <%=templateText == String.Empty ? "false" : "true" %>;
        var axUserOptions =<%=axUserOptions%>;
        var compressedMode = true;//<%=compressedMode.ToString().ToLower()%>;
        var showMenu = <%=showMenu.ToString().ToLower()%>;
        var googleMapsApiKey = "<%=googleMapsApiKey%>";
        var privateSSO = "<%=privateSSO%>";
        var hybridGUID = "<%=hybridGUID.ToString().ToLower()%>";
        var hybridDeviceId = "<%=hybridDeviceId.ToString().ToLower()%>";
        var oktaclientKey = '<%=oktaclientKey%>';
        var office365clientKey = '<%=office365clientKey%>';
        var ssoredirecturl = '<%=ssoredirecturl%>';
        var ShowImaWidActionBtn ='<%=ShowImaWidActionBtn%>';
        var mainHomepagews ='<%=homepagews%>';
        var navigationshow ='<%=navigationshow%>';
        var localStorUser ='<%=localStorUser%>';
        var requestProcess_logtime ='<%=requestProcess_logtime%>';
        var serverprocesstime ='<%=serverprocesstime%>';
        var axpProjectCaption ='<%=axpProjectCaption%>';
        var homePageType = '<%=homePageType%>';
        var ExecTraceOn = '<%=ExecTraceOn%>';
        var bundleCss = '<%=Styles.RenderFormat("{0},", direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
        var bundleJs = '<%=Scripts.RenderFormat("{0},", "~/UI/axpertUI/bundleJs").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
        var reportCss = '<%=Styles.RenderFormat("{0},", "~/Css/report").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
        var reportJs = '<%=Scripts.RenderFormat("{0},", "~/Js/report").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
        var aspBundleJs = '<%=Scripts.RenderFormat("{0},", "~/Js/aspJs").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
        var pwdExpDaysAlert = '<%=pwdExpDays%>';
        var axpertstudioReact = '<%=axpertstudioReact%>';
        var axOldModelFlag = '<%=axOldModelFlag%>';
        var axpertDevOpt = '<%=axpertDevOpt%>';
        <%--var isAxiPlugin = '<%=isAxiPlugin%>';--%>
    </script>
    <script type="text/javascript" src="../Js/util.min.js?v=2"></script>
    <script src="../Js/sso.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/msal.min.js" type="text/javascript"></script>
    <script src="../Js/okta-auth-js.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/main.min.js?v=340"></script>
</head>

<body id="mainNewPageBody" onload="ChangeDir('<');"
    class="btextDir-<%=direction%> header-fixed header-tablet-and-mobile-fixed toolbar-enabled aside-fixed aside-default-enabled" dir="<%=direction%>" data-kt-aside-minimize="off" onunload="ClosePopUps();">
    <asp:Literal ID="templateStage" runat="server" Text=""></asp:Literal>
    <%--<form id="form1" runat="server" onclick="javascript:HideHelp();">--%>
    <form id="form1" runat="server" onclick="javascript:HideHelp();">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
                <asp:ServiceReference Path="../CustomWebService.asmx" />
            </Services>
        </asp:ScriptManager>

        <div id="myPopover" class="d-none" runat="server">
        </div>
        <div id="draftmessagedv" class="tooltip" style="display: none">
            <%=draftsMessage.Value%>
        </div>
        <div class="clear">
        </div>
        <div id="top-selectBox" class="right" style="width: 143px; display: none">
            <div>
                &nbsp;&nbsp;&nbsp;<img alt="print docs" src="../AxpImages/icons/16x16/printer.png" /><a id="ankPrint"
                    style="vertical-align: top;" class="handCur" onclick="javascript:DisplayPrintDocs();">
                    <asp:Label ID="lbldocs" runat="server" meta:resourcekey="lbldocs">PrintDocs(0-0)</asp:Label>
                </a>
            </div>
        </div>
        <!-- the menu's css class to used to get the menu control and handled at main.js.-->
        <div id="dvAppearance">
            <div id="divAppearanceLeft">
            </div>
        </div>
        <div>
            <!-- hdParamsValues for getting global variable value-->
            <div style="display: none;">
                <asp:HiddenField ID="hdParamsValues" runat="server" />
                <asp:HiddenField ID="hdnDisplayTxt" runat="server" />
                <asp:Button ID="btnSetParams" runat="server" OnClientClick="SetUnload('false');"
                    OnClick="btnSetParams_Click" />
            </div>

            <asp:HiddenField ID="hdnTraceValue" runat="server" />
            <asp:HiddenField ID="hdnAction" runat="server" />
            <asp:HiddenField ID="draftsMessage" runat="server" />
            <asp:HiddenField ID="hdnAxGloRecId" runat="server" />
            <div id="AxGloVarHTML" runat="server" style="display: none" />
            <div id="AxGloVarPopOverHTML" runat="server" style="display: none" />
            <div id="divtheme" style="display: none">
                <asp:Label ID="lbltheme" runat="server" Text=""></asp:Label>
                <asp:DropDownList ID="DropDownList1" runat="server">
                </asp:DropDownList>
                <asp:DropDownList ID="DropDownList2" runat="server">
                </asp:DropDownList>
                <%=regthmdata%>
                <%=printInterval %>
                <%=draftScript %>
            </div>
            <div id="mobileMenuWrapper" style="display: none;"></div>
            <div runat="server" class="info d-none" id="dvMessage">
                <table style='margin: 0px; width: 100%; height: 100%; vertical-align: middle;'>
                    <tr>
                        <td>Your password will expire in
                            <%=pwdExpDays %>
                            days. Click <a onclick="javascript:showChangePasswordDialog()" target="middle1">here</a> to
                            change your password.
                        </td>
                        <td style="float: right">
                            <img src="../AxpImages/icons/16x16/delete.png" class="handCur" alt="delete"
                                onclick="HideInfoDiv();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvPrintDocs">
            </div>
            <div class="wrapper---">
                <%if (templateText == String.Empty)
                    {%>
                <header class="main-header">
                    <!-- Logo -->

                    <!-- Header Navbar -->
                    <%if (HttpContext.Current.Session["AxCloudDB"] != null)
                        { %>
                    <nav class="navbar navbar-static-top cloud-header">
                        <%}
                            else
                            { %>
                        <nav class="navbar navbar-static-top">
                            <%} %>
                            <!-- Sidebar toggle button-->
                            <div class="header-left">
                                <a href="javascript:void(0)" class="slide-window" data-toggle="push-menu" role="button">
                                    <i class="fa fa-list" id="menuCollapse" title="Collapse Menu"></i>
                                    <span class="sr-only">Toggle navigation</span>
                                </a>
                                <div class="logo-main">
                                    <a class="brand" href="javascript:void(0)" tabindex="1" title="<%=appName %>"
                                        onclick="LoadIframe('loadhomepage');">
                                        <%if (HttpContext.Current.Session["AxCloudDB"] != null)
                                            { %>
                                        <img class="left" src="../assets/img/agile-biz.png" style="height: 41px;"
                                            alt="logo" onclick="LoadIframe('loadhomepage');" />
                                        <%}
                                            else%>
                                        <%if (IsCustomLogoAvail)
                                            {
                                                var ver = DateTime.Now.ToString("yyyyMMddHHmmss"); %>
                                        <img class="left" src="../images/Custom/logo.png?v=<%=ver %>"
                                            style="width: 48px;" alt="logo" onclick="LoadIframe('loadhomepage');" />
                                        <%}
                                            else %>

                                        <%if (IsLogoAvail)
                                            { %>
                                        <img class="left" src="../assets/img/logo.png" style="width: 48px; height: 32px;"
                                            alt="logo" onclick="LoadIframe('loadhomepage');" />
                                        <%} %>
                                        <%if (HttpContext.Current.Session["AxCloudDB"] == null)
                                            { %>
                                        <span
                                            style="margin-top: 3px; color: #fff; font-weight: normal; font-size: 15px; line-height: 35px; margin-left: 5px;"
                                            onclick="LoadIframe('loadhomepage');" class="titlename">
                                            <%if (axProjectTile == string.Empty)
                                                { %>
                                            <%=appName %>
                                            <%}
                                                else
                                                {%>
                                            Axpert
                                            <%} %>
                                        </span>
                                        <%} %>
                                    </a>
                                    <%if (Session["MobileView"] != null && Session["MobileView"].ToString() == "True")
                                        {%>
                                    <span class="mobile-search fa fa-search"></span>

                                    <%--  <span class="logout-main"><a id="li_Logout2" href="javascript:void(0);" class="link"
                                        data-toggle="tooltip" title="" data-original-title="Logout"
                                        onclick="javascript:signout('../aspx/signout.aspx');return false;"><i
                                            class="fa fa-power-off"></i></a></span>--%>
                                    <div class="clearfix"></div>
                                    <div class="mobile-search-inner" style="display: none;">
                                        <form>
                                            <div id="nav-searchbox">
                                                <input id="globalSearchinp" type="text" class="form-control"
                                                    placeholder="Type for search..." autofocus>
                                            </div>
                                        </form>
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                            <%if (Session["MobileView"] != null && Session["MobileView"].ToString() == "False")
                                {%>
                            <div class="header-mid">
                                <%--<form>--%>
                                <div id="nav-searchbox" class="globalSrch">
                                    <input id="globalSearchinp" type="text" class="form-control"
                                        placeholder="Type for search...">
                                    <i id="gloSrchSpin" class="icon-arrows-rotate rorate-icon d-none"></i>
                                    <i id="gloCondIcon" class="icon-arrows-compress fa fa-link search-icon-chain"></i>
                                    <i class="icon-basic-magnifier fa fa-search search-home"></i>
                                </div>
                                <%--</form>--%>
                            </div>
                            <%} %>
                            <div class="header-right">
                                <%if (HttpContext.Current.Session["AxCloudDB"] != null)
                                    { %>

                                <a class="cloudbrand" href="javascript:void(0)" tabindex="1" title="<%=appName %>">
                                    <span class="cloudtitle" onclick="LoadIframe('loadhomepage');" class="titlename">
                                        <%=appName %>
                                    </span>

                                </a>

                                <%} %>
                                <div class="navbar-custom-menu">
                                    <div><span class="hidden-xs wel-msg">Welcome <%=Session["AxNickName"]%></span></div>
                                    <ul class="nav navbar-nav">
                                        <!-- User Account -->
                                        <li class="dropdown user user-menu">

                                            <ul class="dropdown-menu scale-up">
                                                <!-- User image -->
                                                <li class="user-header">
                                                    <%--<img src="img/Axpert_Icon.ico" class="img-responsive" alt="User Image">--%>
                                                    <p>
                                                        MultiPurpose Themes
                                                        <small>Member since April . 2016</small>
                                                    </p>
                                                </li>
                                                <!-- Menu Body -->
                                                <li class="user-body">
                                                    <div class="row">
                                                        <div class="col-xs-4 text-center">
                                                            <a href="javascript:void(0)">Followers</a>
                                                        </div>
                                                        <div class="col-xs-4 text-center">
                                                            <a href="javascript:void(0)">Friends</a>
                                                        </div>
                                                        <div class="col-xs-4 text-center">
                                                            <a href="javascript:void(0)">Sales</a>
                                                        </div>
                                                    </div>
                                                    <!-- /.row -->
                                                </li>
                                                <!-- Menu Footer-->
                                                <li class="user-footer">
                                                    <div class="pull-left">
                                                        <a href="javascript:void(0)"
                                                            class="btn btn-default btn-flat">Profile</a>
                                                    </div>
                                                    <div class="pull-right">
                                                        <a href="javascript:void(0)"
                                                            class="btn btn-default btn-flat">Sign out</a>
                                                    </div>
                                                </li>
                                            </ul>
                                        </li>
                                        <!-- Messages-->
                                        <li class="dropdown messages-menu axpSettings" title="Home">
                                            <a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown">
                                                <i class="icon-basic-home " onclick="resetLeftMenu();"></i>

                                            </a>
                                        </li>
                                        <li class="dropdown messages-menu noBg panel-control axpSettings"
                                            title="Select Theme">
                                            <a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown">
                                                <i id="themeIcon" class='cog icon-software-paintbrush iconforom'
                                                    style="cursor: pointer; width: initial; height: initial;"></i>
                                            </a>
                                        </li>

                                        <li id="Div1" class="globalParamsWrapper dropdown messages-menu" runat="server"
                                            title="Global Parameters">
                                            <a id="editAxGlo2" runat="server" class="noBg" tabindex="-1"
                                                data-container="body" data-toggle="popover" data-placement="bottom"
                                                data-content="" data-trigger="hover" data-popover-content="#myPopover">
                                                <i class="icon-software-pencil" style="position: relative;"
                                                    tabindex="8"></i>
                                            </a>
                                        </li>
                                        <!-- Notifications -->
                                        <li class="dropdown notifications-menu axpSettings" title="Utilities">
                                            <a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown">
                                                <i class="icon-basic-settings"></i>

                                            </a>
                                            <ul class="dropdown-menu scale-up">
                                                <ul class="head-list">
                                                    <li class="commonSettings" visible="false" id="li_Responsibilities1"
                                                        onclick="DoUtilitiesEvent('Responsibilities');" runat="server"
                                                        title="Responsiblity"><a href="javascript:void(0)"><i
                                                            class="icon-tasks icon-basic-webpage-txt"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblRespons" runat="server"
                                                                meta:resourcekey="lblRespons"> Responsibility
                                                            </asp:Label>
                                                        </a></li>
                                                    <li id="li_WorkFlow1" title="WorkFlow"
                                                        onclick="javascript:LoadIframe('workflownew.aspx')"
                                                        runat="server" visible="true"><a href="javascript:void(0)"><i
                                                            class="icon-map-marker icon-basic-elaboration-browser-refresh"></i>
                                                            <asp:Label ID="lblWorkFlow" runat="server"
                                                                meta:resourcekey="lblWorkFlow"> WorkFlow</asp:Label>
                                                        </a></li>
                                                    <li id="li_ImportData1" runat="server"
                                                        class="commonSettings modal-dialog-events"
                                                        onclick="DoUtilitiesEvent('ImportData');" title="Import Data">
                                                        <a href="javascript:void(0)"><i
                                                            class="glyphicon glyphicon-import icon-basic-upload"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblImportData" runat="server"
                                                                meta:resourcekey="lblImportData"> Import Data
                                                            </asp:Label>
                                                        </a>

                                                    </li>
                                                    <li id="li_ExportData1" runat="server"
                                                        class="commonSettings modal-dialog-events"
                                                        onclick="DoUtilitiesEvent('ExportData');" title="Export Data">
                                                        <a href="javascript:void(0)"><i
                                                            class="glyphicon glyphicon-export icon-basic-download"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblExportData" runat="server"
                                                                meta:resourcekey="lblExportData"> Export Data
                                                            </asp:Label>
                                                        </a>
                                                    </li>

                                                    <li id="li_ImportHistory1" runat="server" class="commonSettings"
                                                        onclick="DoUtilitiesEvent('ImportHistory');"
                                                        title="Import History">
                                                        <a href="javascript:void(0)"><i
                                                            class="glyphicon glyphicon-tasks icon-basic-clockwise"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblImpHistory" runat="server"
                                                                meta:resourcekey="lblImpHistory"> Import History
                                                            </asp:Label>
                                                        </a>
                                                    </li>
                                                    <li class="adminSettings modal-dialog-events"
                                                        onclick="DoUtilitiesEvent('InMemoryDB');" title="In-Memory DB">
                                                        <a href="javascript:void(0)"><i
                                                            class="glyphicon glyphicon-tasks icon-basic-anticlockwise"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblFastUtility" runat="server"
                                                                meta:resourcekey="lblFastUtility"> In-Memory DB
                                                            </asp:Label>
                                                        </a>
                                                    </li>
                                                    <li id="li_WidgetBuilder1" class="commonSettings"
                                                        onclick="DoUtilitiesEvent('WidgetBuilder');"
                                                        title="Widget Builder" visible="false" runat="server">
                                                        <a href="javascript:void(0)"><i
                                                            class="glyphicon icon-basic-settings"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblWidBuilder" runat="server"
                                                                meta:resourcekey="lblWidBuilder"> Widget Builder
                                                            </asp:Label>
                                                        </a>
                                                    </li>
                                                    <li class="commonSettings liDashBoardIcon"
                                                        onclick="ToogleLeftMenu('dashboardPanel', 'true','Dashboard','')"
                                                        title="Dashboard">
                                                        <a href="javascript:void(0)"><i id="dashBoardIcon"
                                                            class="icon-ecommerce-graph-increase"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblDashboard" runat="server"
                                                                meta:resourcekey="lblDashboard"> Dashboard </asp:Label>
                                                        </a>


                                                    </li>
                                                    <li id="pageBuilderBtn" class="noBg commonSettings"
                                                        onclick="LoadIframe('PageDesigner.aspx')" tabindex="-1"
                                                        title="Page Builder">
                                                        <a href="javascript:void(0)"><i id="pageBuilderIcon"
                                                            class="icon-basic-sheet-multiple "
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblpagebuilder" runat="server"
                                                                meta:resourcekey="lblpagebuilder"> Page Builder
                                                            </asp:Label>
                                                        </a>
                                                    </li>


                                                    <%--  <li id="exeiddownload" onclick="LoadIframe('AppDownload.aspx')"
                                                        title="Download" style="display: none;">
                                                        <a href="javascript:void(0)"><i
                                                            class="glyphicon glyphicon-download-alt icon-basic-elaboration-browser-download"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblDownload" runat="server"
                                                                meta:resourcekey="lblDownload"> Download </asp:Label>
                                                        </a>
                                                    </li>--%>

                                                    <li class="commonSettings modal-dialog-events" id="axpertLogs1"
                                                        runat="server" onclick="DoUtilitiesEvent('AxpertLogs');"
                                                        title="Axpert Logs" style="display: none;">
                                                        <a href="javascript:void(0)"><i
                                                            class="glyphicon icon-basic-todolist-pencil"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblAxpertLogs1" runat="server"
                                                                meta:resourcekey="lblAxpertLogs"> Axpert Logs
                                                            </asp:Label>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </ul>
                                        </li>
                                        <!-- Tasks -->
                                        <li class="dropdown tasks-menu axpSettings" title="Settings">
                                            <a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown">
                                                <i class="icon-cog icon-basic-gear"></i>

                                            </a>
                                            <ul class="dropdown-menu scale-up">
                                                <ul class="head-list">
                                                    <li onclick="javascript:showChangePasswordDialog()"
                                                        id="changepassword" runat="server">
                                                        <a href="javascript:void(0)" class="media add-tooltip"
                                                            data-title="Used space : 95%" data-container="body"
                                                            data-placement="bottom" data-original-title=""
                                                            title="Change Password">
                                                            <i class="icon-basic-lock"></i>
                                                            <asp:Label ID="Label5" runat="server"
                                                                meta:resourcekey="lblChangePassword"> Change Password
                                                            </asp:Label>
                                                        </a>
                                                    </li>
                                                    <li id="li_Trace1" runat="server" visible="false"
                                                        style="height: 30px;" title="Trace file">
                                                        <a href="javascript:void(0)"
                                                            class="modal-dialog-events-settings" style="height: 30px;"
                                                            onclick="javascript:showTraceFileDialog();">
                                                            <i class="icon-basic-geolocalize-01"></i>
                                                            <asp:Label ID="Label1" runat="server"
                                                                meta:resourcekey="lblTracefile"> Trace file </asp:Label>
                                                        </a>

                                                        <%if (traceValue == true)
                                                            {%>
                                                        <span onclick="javascript:ToggleTrace(true);">
                                                            <label class="switchtoggel_cls">
                                                                <input type="checkbox" checked><span id="imgTogTrace"
                                                                    class="slider round"
                                                                    title="Trace On"></span></label>
                                                        </span>
                                                        <%}
                                                            else
                                                            {%>
                                                        <span onclick="javascript:ToggleTrace(true);">
                                                            <label class="switchtoggel_cls">
                                                                <input type="checkbox"><span id="imgTogTrace"
                                                                    class="slider round"
                                                                    title="Trace Off"></span></label>
                                                        </span>
                                                        <% }
                                                        %>
                                                    </li>
                                                    <li id="li3" class="modal-dialog-events-settings"
                                                        onclick="javascript:showAbout();" runat="server" title="About">
                                                        <a href="javascript:void(0)"><i class="icon-arrows-info"></i>
                                                            <asp:Label ID="Label2" runat="server"
                                                                meta:resourcekey="lblabout">About</asp:Label>
                                                        </a>

                                                    </li>
                                                    <li class="commonSettings modal-dialog-events" id="configApp1"
                                                        runat="server" onclick="DoUtilitiesEvent('ConfigApp');"
                                                        title="Global Settings">
                                                        <a href="javascript:void(0)"><i
                                                            class="glyphicon icon-arrows-squares"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="lblConfigApp1" runat="server"
                                                                meta:resourcekey="lblConfigApp"> Global Settings
                                                            </asp:Label>
                                                        </a>
                                                    </li>
                                                    <li class="commonSettings modal-dialog-events" id="AppConfig"
                                                        runat="server"
                                                        onclick="LoadIframe('tstruct.aspx?transid=axstc');"
                                                        title="Developer Options">
                                                        <a href="javascript:void(0)"><i
                                                            class="glyphicon icon-software-pathfinder-exclude"
                                                            style="margin-right: 5px;"></i>
                                                            <asp:Label ID="Label3" runat="server"
                                                                meta:resourcekey="lblAppConfig">Developer Options
                                                            </asp:Label>
                                                        </a>
                                                    </li>
                                                    <li id="li_Logout"
                                                        onclick="javascript:signout('<%=signOutPath %>');"
                                                        title="Logout">
                                                        <a class="media" href="javascript:void(0)">
                                                            <i class="icon-basic-key"></i>
                                                            <asp:Label ID="Label4" runat="server"
                                                                meta:resourcekey="lblLogout">Logout</asp:Label>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </ul>
                                        </li>
                                        <li id="qrCodeBtnWrapper" class="dropdown messages-menu axpSettings">

                                            <a href="javascript:void(0)"
                                                class="dropdown-toggle noBg qrcoderelnoBg qrcoderel"
                                                data-toggle="dropdown" id="qrCode" onclick="createMobileQRcode()"
                                                title="Mobile QR">
                                                <i class="fa fa-qrcode qrcoderel"></i>

                                            </a>


                                        </li>
                                        <li id="complaintLI" class="dropdown messages-menu axpSettings"
                                            title="Report An Issue" runat="server" visible="false">
                                            <a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown">
                                                <i class="icon-basic-sheet-pen"
                                                    onclick="DoUtilitiesEvent('Complaint');"></i>

                                            </a>
                                        </li>
                                        <li class="dropdown messages-menu axpSettings" title="Home" id="liLicInfo"
                                            runat="server" visible="false">
                                            <a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown"
                                                id="aLicinfo" runat="server" title="License Infomation">
                                                <div class="halfway-fab teal darken-1 pulse">
                                                    <i class="fa fa-info"></i>
                                                </div>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </nav>
                </header>
                <div id="customizer" class="panel-closed text-center">
                    <div id="options">
                        <div class="options-head">
                            <asp:Label ID="lblthemeclt" runat="server" meta:resourcekey="lblthemeclt">Theme Switcher
                            </asp:Label>
                        </div>
                        <div class="options-box clearfix">
                            <ul class="color-scheme clearfix">
                                <li><a id="blue" class="blue themeEvent" title="Blue"></a></li>
                                <li><a id="green" class="green themeEvent" title="Green"></a></li>
                                <li><a id="red" class="red themeEvent" title="Red"></a></li>
                                <li><a id="indigo" class="indigo themeEvent" title="Purple"></a></li>
                                <li><a id="orange" class="orange themeEvent" title="Orange"></a></li>
                                <li><a id="deepindigo" class="deepindigo themeEvent" title="Indigo"></a></li>
                                <li><a id="gray" class="gray themeEvent" title="Default"></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <aside class="main-sidebar" style="display: none;">
                    <!-- sidebar-->
                    <ul class="header-list">
                        <li class="refresh hisItem" title="Show History List"><i class="fa fa-angle-down"></i>
                            <div class="selection-main">
                                <ul class="histList item-list">
                                </ul>
                            </div>
                        </li>
                        <li class="refresh" title="Go Forward"><i class="fa fa-chevron-circle-right linkNext linkGray"
                            data-index="" data-url=""></i></li>
                        <li class="refresh" title="Go Back"><i class="fa fa-chevron-circle-left linkPrev linkGray"
                            data-index="" data-url=""></i></li>
                        <li class="refresh" onclick="refreshMenu();" title="Refresh Menu"><i class="fa fa-refresh"></i>
                        </li>
                    </ul>
                    <section class="sidebar" style="height: auto;">
                        <!-- Sidebar user panel -->

                        <!-- sidebar menu: -->
                        <div class="slide-main-menu">
                            <ul class="sidebar-menu" data-widget="tree" id="mainnav-menu">
                            </ul>
                        </div>
                    </section>
                    <!-- /.sidebar -->
                </aside>
                <!-- Content Wrapper. Contains page content -->
                <div class="content-wrapper">
                    <%if (axProjectTile != string.Empty)
                        { %>
                    <span class="axCustomPTitle" title='<%=axProjectTile %>'>
                        <%=axProjectTile %>
                    </span>
                    <%}%>
                    <!-- Main content -->
                    <section class="mybreadcram">
                        <div class="row">
                            <div class="col-lg-3 col-md-3">
                            </div>
                            <div class="button-splitter">
                                <%if (Session["MobileView"] != null && Session["MobileView"].ToString() == "False")
                                    {%>
                                <div class="split-btn d-none" onclick="assocateIframe();splitvertical();" title="Vertical Split" style="transform: rotate(90deg);"><i class="fa fa-sort"></i></div>
                                <div class="split-btn-vertical d-none" onclick="assocateIframe();splithorizantal();" title="Horizontal Split"><i class="fa fa-sort"></i></div>
                                <%--<div class="split-vertical"><i class="fa fa-arrows-h"></i></div>--%>
                                <%} %>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12" style="padding: 0px;">
                            <div class="splitter-wrapper">
                                <div class="panel-container main-panel-container">
                                    <div class="panel-left panel-fisrt-part">
                                        <div class="panel-left-inner">
                                            <iframe id="middle1" name="middle1" frameborder="0" scrolling="no" class="searchOpened"
                                                allowtransparency="True" width="100%"></iframe>
                                        </div>
                                    </div>
                                    <div class="splitter panel-splitter" id="drag-point" style="display: none;">
                                    </div>
                                    <div class="panel-right panel-second-part" id="divaxpiframe" style="display: none !important">
                                        <div class="panel-right-inner">
                                            <iframe id="axpiframe" name="axpiframe" frameborder="0" runat="server" scrolling="no"
                                                class="searchOpened" allowtransparency="True" width="100%"
                                                style="display: none;"></iframe>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                </div>

                <%} %>
            </div>
            <%if (templateText == String.Empty)
                {%>
            <div class="sidebar-footer">
                <!-- item-->

                <!-- item-->
                <%--<a href="" class="link" data-toggle="tooltip" title="" data-original-title="Logout" onclick="javascript:signout('../aspx/signout.aspx');"><i class="fa fa-power-off"></i></a>--%>
            </div>
            <footer class="main-footer">
                Powered by Axpert
            </footer>
            <%} %>
        </section>
        <!-- /.content -->
        </div>
        <!-- /.content-wrapper -->


        <!-- /.control-sidebar -->
        <!-- Add the sidebar's background. This div must be placed immediately after the control sidebar -->
        <div class="control-sidebar-bg"></div>
        <!-- <div id="mainQRcodeWrapper"></div> -->
        </div>

        <asp:HiddenField ID="NodeApiCall" runat="server" />
        <asp:HiddenField ID="hdndashBoardIcon" runat="server" Value="f" />
        <asp:HiddenField ID="hdnmessagesIcon" runat="server" Value="f" />
        <asp:HiddenField ID="hdHomeUrl" runat="server" />
        <asp:HiddenField ID="hdKeepMeDefaultUrl" runat="server" />

        <asp:HiddenField ID="cardsData" runat="server" />
        <asp:HiddenField ID="cardsDesign" runat="server" />

        <asp:HiddenField ID="listviewAsDefault" Value="[]" runat="server" />
        <asp:HiddenField ID="listviewLoadFromSearch" Value="[]" runat="server" />

        <asp:HiddenField ID="enableListviewExtraHyperlinks" Value="" runat="server" />

        <asp:HiddenField ID="_antiforgery" runat="server" />

        <asp:HiddenField ID="hdnOpenNewtabSite" runat="server" />

        <div id="waitDiv" class="page-loader rounded-2">
            <div class="loader-box-wrapper d-flex">
                <span class="loader"></span>
                <span class="page-loader-text ps-3 boldest fs-5 py-5">Welcome</span>
            </div>
        </div>
    </form>
    <form name="clwindow" dir="<%=direction%>">
        <input type="hidden" name="clwin" value="" />
    </form>
    <form method="post" action="mainnew.aspx">
        <%=strParamsRF.ToString() %>
        <button style="display: none;" id="btnResubmit"></button>
    </form>
</body>

</html>
