<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adminconsole.aspx.cs" Inherits="aspx_adminconsole" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Widget</title>

    <%--<script>
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
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
</head>
<body class="header-fixed header-tablet-and-mobile-fixed toolbar-enabled aside-fixed aside-default-enabled page-loading" id="dvadminconsole" data-kt-aside-minimize="off" dir="<%=direction%>">

    <div class="adminMain d-flex flex-column flex-root">
        <div class="adminMainInner page d-flex flex-row flex-column-fluid position-relative">
            <!-- <div class="aside aside-default aside-hoverable rounded--- mt-20 mt-lg-0" data-kt-drawer="true" data-kt-drawer-name="aside" data-kt-drawer-activate="{default: true, lg: false}" data-kt-drawer-overlay="true" data-kt-drawer-width="{default:'200px', '300px': '250px'}" data-kt-drawer-direction="start" data-kt-drawer-toggle="#kt_aside_toggle"> -->
            <div class="aside aside-default aside-hoverable" data-kt-drawer="true" data-kt-drawer-name="aside" data-kt-drawer-activate="{default: true, lg: false}" data-kt-drawer-overlay="true" data-kt-drawer-width="{default:'200px', '300px': '250px'}" data-kt-drawer-direction="start" data-kt-drawer-toggle="#kt_aside_toggle">
                <div class="adminMainLeft aside-menu flex-column-fluid">
                    <!-- <div class="adminLeftMenu menu menu-column menu-fit menu-rounded menu-title-gray-600 menu-icon-gray-400 menu-state-primary menu-state-icon-primary menu-state-bullet-primary menu-arrow-gray-500 fw-bold fs-5 my-5 mt-lg-5 mb-lg-0" data-kt-menu="true">
                        <div class="pt-lg-20 menu-fit hover-scroll-y hover-scroll-x me-lg-n5 pe-lg-5" data-kt-scroll="true" data-kt-scroll-activate="{default: false, lg: true}" data-kt-scroll-height="auto" data-kt-scroll-wrappers="#kt_aside_menu" data-kt-scroll-offset="20px"> -->
                    <div class="adminLeftMenu menu menu-column menu-fit menu-rounded menu-title-gray-400 menu-icon-gray-300 menu-state-white menu-state-icon-warning menu-state-bullet-warning menu-hover-bg-dark menu-arrow-gray-400 fw-light fs-6 my-5 mt-lg-2 mb-lg-0" data-kt-menu="true">
                        <div class="pt-lg-20 menu-fit menu-link-no-padding hover-scroll-y hover-scroll-x ms-lg-n5--- me-lg-n10--- ps-lg-2 pe-lg-3 dark-scroll" data-kt-scroll="true" data-kt-scroll-activate="{default: false, lg: true}" data-kt-scroll-height="auto" data-kt-scroll-wrappers="#kt_aside_menu" data-kt-scroll-offset="20px">
                            <div class="menu-item">
                                <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("ivtoivload.aspx?ivname=axroles");callParentNew("closeFrame();","function");'>
                                    <span class="menu-icon">
                                        <%--<a href="javascript:void(0);">--%>
                                        <i class="setting-icon material-icons">history</i>
                                    </span>
                                    <span class="setting-caption menu-title">Roles</span>
                                    <%--<div class="clearfix"></div>--%>
                                    <%--</a>--%>

                                </a>
                            </div>

                            <div class="menu-item">
                                <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("ivtoivload.aspx?ivname=response");callParentNew("closeFrame();","function");'>
                                    <%--<a href="javascript:void(0);">--%>
                                    <span class="menu-icon">
                                        <i class="setting-icon material-icons">how_to_reg</i>
                                    </span>
                                    <span class="setting-caption menu-title">Responsibilities</span>
                                    <%--<div class="clearfix"></div>--%>
                                    <%-- </a>--%>
                                </a>
                            </div>
                            <%-- <li onclick="LoadIframeac('udPage.html');">
                                    <a href="javascript:void(0);">
                                        <i class="setting-icon material-icons">post_add</i>
                                        <span class="setting-caption">Task Console</span>
                                        <div class="clearfix"></div>
                                    </a>
                                </li>--%>
                            <div class="menu-item">
                                <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("ivtoivload.aspx?ivname=axusers");callParentNew("closeFrame();","function");'>
                                    <%--<a href="javascript:void(0);">--%>
                                    <span class="menu-icon">
                                        <i class="setting-icon material-icons">supervisor_account</i>
                                    </span>
                                    <span class="setting-caption menu-title">Users</span>
                                    <%--<div class="clearfix"></div>
                                    </a>--%>
                                </a>
                            </div>
                            <div class="menu-item">
                                <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("ivtoivload.aspx?ivname=inmemdb");callParentNew("closeFrame();","function");'>
                                    <%--<a href="javascript:void(0);">--%>
                                    <span class="menu-icon">
                                        <i class="setting-icon material-icons">playlist_add_check</i>
                                    </span>
                                    <span class="setting-caption menu-title">Mem DB Console</span>
                                    <%-- <div class="clearfix"></div>
                                    </a>--%>
                                </a>
                            </div>
                            <div data-kt-menu-trigger="click" class="menu-item menu-accordion">
                                <span class="menu-link">
                                    <span class="menu-icon">
                                        <!--begin::Svg Icon | path: icons/duotune/general/gen025.svg-->
                                        <i class="setting-icon material-icons">search</i>
                                        <!--end::Svg Icon-->
                                    </span>
                                    <span class="menu-title">Jobs Console</span>
                                    <span class="menu-arrow"></span>
                                </span>
                                <div class="menu-sub menu-sub-accordion menu-active-bg">
                                    <div class="menu-item">
                                        <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("ivtoivload.aspx?ivname=axscrlog");callParentNew("closeFrame();","function");' href="javascript:void(0);">
                                            <span class="d-flex mx-2">
                                                <i class="setting-icon material-icons">code</i>
                                            </span>
                                            <span class="menu-title mx-2">Script Job</span>
                                        </a>
                                    </div>
                                    <%--<li>--%>
                                    <%-- <a href="javascript:void(0);">
                                        <i class="setting-icon material-icons">search</i>
                                        <span class="setting-caption">Jobs Console</span>
                                        <div class="clearfix"></div>
                                    </a>--%>
                                    <%--<ul class="devsubmenu">--%>
                                    <%--<li onclick="LoadIframeac('ivtoivload.aspx?ivname=axscrlog');">
                                            <a href="javascript:void(0);">
                                                <i class="setting-icon material-icons">code</i>
                                                <span class="setting-caption">Script Job</span>
                                                <div class="clearfix"></div>
                                            </a>
                                        </li>--%>
                                    <div class="menu-item">
                                        <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("ivtoivload.aspx?ivname=aximplog");callParentNew("closeFrame();","function");'>
                                            <%--<a href="javascript:void(0);">--%>
                                            <span class="d-flex mx-2">
                                                <i class="setting-icon material-icons">vertical_align_bottom</i>
                                            </span>
                                            <span class="menu-title mx-2">Import jobs</span>
                                            <%--<div class="clearfix"></div>--%>
                                            <%--</a>--%>
                                        </a>
                                    </div>
                                    <%--<li onclick="LoadIframeac('ivtoivload.aspx?ivname=axexplog');">
                                            <a href="javascript:void(0);">
                                                <i class="setting-icon material-icons">merge_type</i>
                                                <span class="setting-caption">Export jobs</span>
                                                <div class="clearfix"></div>
                                            </a>
                                        </li>--%>
                                    <div class="menu-item">
                                        <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("ivtoivload.aspx?ivname=emaillog");callParentNew("closeFrame();","function");'>
                                            <%--<a href="javascript:void(0);">--%>
                                            <span class="d-flex mx-2">
                                                <i class="setting-icon material-icons">mail_outline</i>
                                            </span>
                                            <span class="menu-title mx-2">Email Jobs</span>

                                        </a>
                                    </div>
                                    <%--<li onclick="LoadIframeac('ivtoivload.aspx?ivname=printlog');">
                                            <a href="javascript:void(0);">
                                                <i class="setting-icon material-icons">print</i>
                                                <span class="setting-caption">Print jobs</span>
                                                <div class="clearfix"></div>
                                            </a>
                                        </li>--%>
                                    <div class="menu-item">
                                        <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("ivtoivload.aspx?ivname=axapilog");callParentNew("closeFrame();","function");'>
                                            <%--<a href="javascript:void(0);">--%>
                                            <span class="d-flex mx-2">
                                                <i class="setting-icon material-icons">check_circle</i>
                                            </span>
                                            <span class="menu-title mx-2">API jobs</span>
                                            <%--<div class="clearfix"></div>--%>
                                            <%--</a>--%>
                                        </a>
                                    </div>
                                    <%--</ul>--%>
                                    <%--</li>--%>
                                </div>
                                <%-- <li onclick="LoadIframeac('iview.aspx?ivname=auditlog');">
                                    <a href="javascript:void(0);">
                                        <i class="setting-icon material-icons">assessment</i>
                                        <span class="setting-caption">Usage audit</span>
                                        <div class="clearfix"></div>
                                    </a>
                                </li>--%>
                            </div>
                            <div class="menu-item d-none">
                                <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("WorkflowNew.aspx");callParentNew("closeFrame();","function");'>
                                    <%--<a href="javascript:void(0);">--%>
                                    <span class="menu-icon">
                                        <i class="setting-icon material-icons">account_tree</i>
                                    </span>
                                    <span class="setting-caption menu-title">WorkFlow</span>
                                    <%-- <div class="clearfix"></div>
                                    </a>--%>
                                </a>
                            </div>
                            <div class="menu-item">
                                <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("configuration.aspx");callParentNew("closeFrame();","function");'>
                                    <%--<a href="javascript:void(0);">displayBootstrapModalDialog('Global Settings', 'lg', '430px', true, '../aspx/Configuration.aspx', true, false, function () { UnlockConfigApp({ forceUnlock: false }); });--%>
                                    <span class="menu-icon">
                                        <i class="setting-icon material-icons">settings_applications</i>
                                    </span>
                                    <span class="setting-caption menu-title">Settings</span>
                                    <%--  <div class="clearfix"></div>
                                    </a>--%>
                                </a>
                            </div>
                            <%-- <li onclick="LoadIframeac('WorkflowNew.aspx');">
                                    <a href="javascript:void(0);">
                                        <i class="setting-icon material-icons">account_tree</i>
                                        <span class="setting-caption">WorkFlow Delegation</span>
                                        <div class="clearfix"></div>
                                    </a>
                                </li>--%>

                            <div class="menu-item">
                                <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("tstruct.aspx?act=load&transid=ad_pr&axpdef_axpertpropsid=1");callParentNew("closeFrame();","function");'>
                                    <%--<a href="javascript:void(0);">--%>
                                    <span class="menu-icon">
                                        <i class="setting-icon material-icons">assignment</i>
                                    </span>
                                    <span class="setting-caption menu-title">Application Properties</span>
                                    <%--<div class="clearfix"></div>
                                            </a>--%>
                                </a>
                            </div>
                            <%--   <div class="menu-item">
                                <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("ivtoivload.aspx?ivname=axlangs");callParentNew("closeFrame();","function");'>
                                    <span class="menu-icon">
                                        <i class="setting-icon material-icons">language</i>
                                    </span>
                                    <span class="setting-caption menu-title">Axpert languages</span>
                                </a>
                            </div>
                            <div class="menu-item">
                                <a class="menu-link" onclick='callParentNew("loadFrame();","function");LoadIframeac("iview.aspx?ivname=ad_ve&tstcaption=Validations Error Code Setup");callParentNew("closeFrame();","function");'>
                                    <span class="menu-icon">
                                        <i class="setting-icon material-icons">report_gmailerrorred</i>
                                    </span>
                                    <span class="setting-caption menu-title">Error Codes</span>
                                </a>
                            </div>--%>
                        </div>
                    </div>
                </div>
                <%--</ul>--%>
            </div>
            <%--</div>--%>
            <%--</div>--%>
            <div class="wrapper d-flex flex-column flex-row-fluid w-100 h-100">
                <div id="kt_header" class="header" data-kt-sticky="true" data-kt-sticky-name="header" data-kt-sticky-offset="{default: '0px', lg: '0px'}" data-kt-sticky-animation="false">
                    <!--begin::Container-->
                    <div class="container-fluid d-flex align-items-stretch justify-content-between">
                        <%--container-fluid--%>
                        <!--begin::Logo bar-->
                        <div class="d-flex align-items-center flex-grow-1 flex-lg-grow-0">
                            <!--begin::Aside Toggle-->
                            <div class="d-flex align-items-center d-lg-none">
                                <div class="btn btn-icon btn-active-color-primary ms-n2 me-1" id="kt_aside_toggle">
                                    <!--begin::Svg Icon | path: icons/duotune/abstract/abs015.svg-->
                                    <span class="material-icons material-icons-style material-icons-1">menu
                                    </span>
                                    <!--end::Svg Icon-->
                                </div>
                            </div>
                            <!--end::Aside Toggle-->
                            <!--begin::Aside toggler-->
                            <div class="btn btn-icon w-auto ps-0 btn-active-color-primary d-none d-lg-inline-flex me-2 me-lg-5 active---" data-kt-toggle="true" data-kt-toggle-state="active" data-kt-toggle-target="body" data-kt-toggle-name="aside-minimize">
                                <span class="material-icons material-icons-style material-icons-2 rotate-180">menu
                                </span>
                            </div>
                            <!--end::Aside toggler-->
                            <h1 class="text-dark fw-bold m-2 fs-2">Administration Console</h1>
                        </div>
                        <!--end::Logo bar-->

                    </div>
                    <!--end::Container-->
                </div>
                <div class="content fs-6 d-flex flex-column flex-column-fluid px-3 pt-5 pb-2 adminRightGap">
                    <!-- <div class="btn btn-icon btn-sm position-absolute ms-n20 mt-2 fw-bolder z-index-2 transform-270 appBackBtn" onclick="prevbtn_click($(this))">
                        <span class="btn btn-flex btn-white btn-active-light-primary h-30px shadow-sm fs-6 mt-7 pt-4 pb-3 px-4 rounded-top-0" title="" data-bs-toggle="tooltip-disable" data-bs-placement="left" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-html="true" data-bs-sanitize="false" data-bs-original-title='<button type="button" class="btn btn-icon btn-sm btn-outline btn-outline-dashed btn-outline-primary btn-bg-white btn-active-primary"><i class="material-icons material-icons-style material-icons-2">arrow_back</i></button>'>
                            <span>back</span>
                        </span>
                    </div> -->
                    <div class="btn btn-icon btn-sm position-absolute fw-bolder ms-4--- ms-n3 mt-n7 z-index-2 appBackBtn" onclick="prevbtn_click($(this))">
                        <button type="button" class="btn btn-icon btn-sm ">
                            <span class="material-icons material-icons-style material-icons-2">arrow_back</span></button>
                    </div>
                    <div class="splitter-wrapper flex-column-fluid h-100">
                        <div class="panel-container main-panel-container flex-column-fluid h-100">
                            <div class="panel-left panel-fisrt-part flex-column-fluid h-100">
                                <div class="panel-left-inner flex-column-fluid h-100">
                                    <iframe id="axpiframeac" name="axpiframeac" class="col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened flex-column-fluid h-100" style="padding: 0px;" frameborder="0" allowtransparency="True" width="100%"></iframe>
                                </div>
                                <div class="splitter panel-splitter flex-column-fluid h-100" id="drag-point" style="display: none;">
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div class="adminMainRight">--%>

                    <%--</div>--%>
                </div>
            </div>
            <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
                <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                    <span class="loader"></span>
                </div>
            </div>
            <%--<div class="clearfix"></div>--%>
        </div>
    </div>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>

    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script src="../Js/helper.min.js?v=172" type="text/javascript"></script>
    <script src="../Thirdparty/newMaterialUI/plugins/node-waves/waves.min.js"></script>
    <script src="../js/adminconsole.min.js?v=10" type="text/javascript"></script>
    <script type="text/javascript">
        $(callParentNew("appBackBtn", "class")).hide();
    </script>
    <form id="form1" runat="server">
    </form>
</body>
</html>
