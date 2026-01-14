<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AxMain.aspx.cs" Inherits="AxMain" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Agile Labs" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
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
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <script src="../Js/thirdparty/jquery-resizable.min.js?v=2" type="text/javascript"></script>
    <title><%=appTitle%></title>
    <link rel="shortcut icon" href="../images/favicon.ico" />
    <script src="../Js/xmlToJson.js?v=2"></script>
    <script src="../Js/helper.min.js?v=172"></script>
    <script src="../assets/js/loadingoverlay.min.js?v=3" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/common.min.js?v=158"></script>
    <script src="../Js/process.min.js?v=344"></script>
    <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>
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
        var redisUtl = "<%=redisUtl%>";
        var currentThemeColor = "";
        var hdnpsid = '<%=hdnpsid%>';
        var nodeAccessToken;
        var AxUserRoles = '<%=AxRole%>';
        var prevMid1FrameUrl = "";
        var currAxpFrmUrl = "";
        var axMainPageReload = '<%=axMainPageReload%>';
        var compressedMode = false;//<%=compressedMode.ToString().ToLower()%>;
        var googleMapsApiKey = "<%=googleMapsApiKey%>";
        var hybridGUID = "<%=hybridGUID.ToString().ToLower()%>";
        var hybridDeviceId = "<%=hybridDeviceId.ToString().ToLower()%>";
        var ShowImaWidActionBtn ='<%=ShowImaWidActionBtn%>';
        var mainHomepagews ='<%=homepagews%>';
        var navigationshow='<%=navigationshow%>';
        var requestProcess_logtime ='<%=requestProcess_logtime%>';
        var serverprocesstime ='<%=serverprocesstime%>';
        var axpProjectCaption = '<%=axpProjectCaption%>';
        var axAttachmentSize = '<%=Session["AxAttachmentSize"] == null? "1" :(Session["AxAttachmentSize"].ToString()==""?"1":Session["AxAttachmentSize"].ToString())%>';
        var bundleCss = '<%=Styles.RenderFormat("{0},", direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
        var bundleJs = '<%=Scripts.RenderFormat("{0},", "~/UI/axpertUI/bundleJs").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
        var reportCss = '<%=Styles.RenderFormat("{0},", "~/Css/report").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
        var reportJs = '<%=Scripts.RenderFormat("{0},", "~/Js/report").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
        var aspBundleJs = '<%=Scripts.RenderFormat("{0},", "~/Js/aspJs").ToString().Replace("\r\n", "")%>'.split(",").filter(file => file).map(file => window.location.origin + file);
    </script>

    <script type="text/javascript" src="../Js/util.min.js?v=2"></script>
    <script type="text/javascript" src="../js/axmain.min.js?v=31"></script>
</head>
<body id="mainNewPageBody" onload="ChangeDir('<');"
    class="btextDir-<%=direction%> header-fixed header-tablet-and-mobile-fixed toolbar-enabled aside-fixed aside-default-enabled" dir="<%=direction%>" data-kt-aside-minimize="off" onunload="ClosePopUps();">

    <form id="form1" runat="server" class="h-100">
        <div id="divAuthError" class="d-flex justify-content-center align-items-center p-10 vh-100 d-none">
            <h1 class="h1 me-3 my-0 align-top inline-block align-content-center">
                <asp:Label ID="lblerror" runat="server" meta:resourcekey="lblerror">Error</asp:Label>
            </h1>
            <div class="inline-block align-middle ps-3 py-2 border-danger border-start">
                <label class="d-flex h4">Error occured due to the following reason:&nbsp;</label>
                <h2 class="my-0 font-weight-normal lead" id="desc"></h2>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
                <asp:ServiceReference Path="../CustomWebService.asmx" />
            </Services>
        </asp:ScriptManager>

        <div style="display: none;">
            <asp:HiddenField ID="hdParamsValues" runat="server" />
            <asp:HiddenField ID="hdnDisplayTxt" runat="server" />
            <asp:HiddenField ID="hdHomeUrl" runat="server" />
        </div>
        <div id="divtheme" style="display: none">
            <asp:Label ID="lbltheme" runat="server" Text=""></asp:Label>
            <asp:DropDownList ID="DropDownList1" runat="server">
            </asp:DropDownList>
            <asp:DropDownList ID="DropDownList2" runat="server">
            </asp:DropDownList>
            <%=regthmdata%>
        </div>

        <!--begin::Content-->
        <div class="content fs-6 d-flex flex-column flex-column-fluid px-3 pt-3 pb-2 h-100">
            <div class="btn btn-icon btn-sm position-absolute fw-bolder ms-4--- ms-n3 mt-n4 z-index-2 appBackBtn" onclick="prevbtn_click($(this))">
                <button type="button" class="btn btn-icon btn-sm ">
                    <span class="material-icons material-icons-style material-icons-2">arrow_back</span>
                </button>
            </div>
            <!-- #IFrames Panel @IMPORTANT@ -->
            <div class="splitter-wrapper flex-column-fluid h-100">
                <div class="panel-container main-panel-container flex-column-fluid row m-0 h-100">
                    <div class="panel-left panel-fisrt-part flex-column-fluid--- p-0 h-100">
                        <div class="panel-left-inner flex-column-fluid h-100">
                            <iframe id="middle1" name="middle1"
                                class="col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened flex-column-fluid h-100" src=""
                                style="padding: 0px;" frameborder="0"
                                allowtransparency="True" width="100%"></iframe>
                        </div>
                    </div>
                    <div class="splitter panel-splitter flex-column-fluid flex-row-auto cursor-col-resize w-unset h-100" id="drag-point" style="display: none;">
                    </div>
                    <div class="panel-right panel-second-part flex-column-fluid--- flex-root p-0 h-100" id="divaxpiframe"
                        style="display: none !important">
                        <div class="panel-right-inner flex-column-fluid h-100">
                            <iframe id="axpiframe" name="axpiframe"
                                class="col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened flex-column-fluid h-100" src=""
                                style="padding: 0px;" frameborder="0"
                                allowtransparency="True" width="100%" style="display: none;"></iframe>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /IFrames Panel -->
            <div class="cardsPageWrapper d-none">
                <div class="designer toolbar ms-5 pb-0">
                    <div class="d-flex flex-stack flex-wrap flex-sm-nowrap">
                        <!--begin::Info-->
                        <div class="d-flex flex-column align-items-start justify-content-center flex-wrap me-2">
                            <!--begin::Title-->
                            <h1 class="text-dark fw-boldest my-1 fs-2">Cards 
                                        <small class="text-muted fs-6 fw-normal ms-1"></small></h1>
                            <!--end::Title-->
                        </div>
                        <!--end::Info-->
                        <!--begin::Actions-->
                        <div class="d-flex align-items-center flex-nowrap text-nowrap py-1">
                            <a href="javascript:void(0);" class="editSaveCardDesign btn btn-icon btn-white btn-active-primary me-4" onclick="$.axpertUI.cardsPage.editSaveCardDesignToggle();">
                                <span class="material-icons material-icons-style">edit
                                </span>
                            </a>
                        </div>
                        <!--end::Actions-->
                    </div>
                </div>

                <ul class="cardsDesigner card list-group d-none mt-6"></ul>
                <div class="cardsPlot mb-8">
                </div>
            </div>
            <!-- </div> -->

            <div class="d-none toolbar ms-5 pb-0">
                <div class="container-fluid d-flex flex-stack flex-wrap flex-sm-nowrap">
                    <!--begin::Info-->
                    <div class="d-flex flex-column align-items-start justify-content-center flex-wrap me-2">
                        <!--begin::Title-->
                        <h1 class="text-dark fw-boldest my-1 fs-2">Cards 
                                <small class="text-muted fs-6 fw-normal ms-1"></small></h1>
                        <!--end::Title-->
                        <!--begin::Breadcrumb-->
                        <ul class="breadcrumb fw-bold fs-base my-1 d-none">
                            <li class="breadcrumb-item text-muted">
                                <a href="/craft/index.html" class="text-muted text-hover-primary">Home</a>
                            </li>
                            <li class="breadcrumb-item text-muted">Dashboards</li>
                            <li class="breadcrumb-item text-dark">Default</li>
                        </ul>
                        <!--end::Breadcrumb-->
                    </div>
                    <!--end::Info-->
                    <!--begin::Actions-->
                    <div class="d-flex align-items-center flex-nowrap text-nowrap py-1">
                        <a href="#" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary me-4" data-bs-toggle="modal" data-bs-target="#kt_modal_invite_friends">
                            <span class="material-icons material-icons-style">edit
                            </span>
                        </a>
                    </div>
                    <!--end::Actions-->
                </div>
            </div>

            <!--begin::Container-->
            <div class="d-none container">
                <!--begin::Row-->
                <div class="row">
                </div>
            </div>
            <!--end::Container-->

        </div>
        <!--end::Content-->
        <asp:HiddenField ID="hdKeepMeDefaultUrl" runat="server" />
        <asp:HiddenField ID="listviewAsDefault" Value="[]" runat="server" />
        <asp:HiddenField ID="listviewLoadFromSearch" Value="[]" runat="server" />

    </form>
</body>
</html>
