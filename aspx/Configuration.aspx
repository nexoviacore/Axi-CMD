<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Configuration.aspx.cs" Inherits="aspx_Configuration" Debug="true" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Configuration" />
    <meta name="keywords" content="Agile" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Configuration</title>

    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
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
    <link href="../Css/multi-select.min.css" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script type="text/javascript">
        var enableBackButton = false;
        var enableForwardButton = false;
        var lockPage = '<%=lockPage%>';
    </script>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>

    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <script>
        var AxApiUrl = '<%=apiUrl%>';
    </script>
    <script src="../Js/Configuration.min.js?v=96"></script>
    <script src="../Js/AppSettings.min.js?v=12" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script>
        var attchmentLimit = '<%=Session["AxAttachmentSize"]%>' == '' ? '1' : '<%=Session["AxAttachmentSize"]%>'
        callParentNew("axAttachmentSize=", attchmentLimit);
        var appsessionKey = '<%=appsessionKey%>';
        // to update error message timeout in config app after saving
        var errMessage = '<%=Session["AxErrorMsg"]%>' == '' ? 'false' : '<%=Session["AxErrorMsg"]%>'
        callParentNew("errorEnable=", errMessage);

        var errTimeout = '<%=Session["AxErrorMsgTimeout"]%>' == '' ? '0' : '<%=Convert.ToInt32(Session["AxErrorMsgTimeout"]) * 1000%>'
        callParentNew("errorTimeout=", errTimeout);
        var _projName = '<%=proj%>';
    </script>
</head>
<body dir='<%=direction%>' class="btextDir-<%=direction%> content p-0 fs-6">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
                <asp:ServiceReference Path="../CustomWebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div class="maincontainer">
            <!--tab navigation starts from here -->
            <ul class="cursor-pointer nav nav-tabs mb-n2 d-none">
                <li class="nav-item" id="liApplication2">
                    <a class="nav-link fw-boldest shadow-sm fs-6 text-gray-800 p-4 active" runat="server" data-menu-id="menu1Header" data-div-id="home" id="tabApplication2" href="#home" aria-controls="home" aria-selected="true">Application</a>
                </li>
                <li class="nav-item" id="liForms2">
                    <a class="nav-link fw-boldest shadow-sm fs-6 text-gray-800 p-4" runat="server" id="tabForms2" data-toggle="tab" data-div-id="menu1" data-menu-id="menu2Header" href="#menu1" aria-controls="menu1" aria-selected="false">Forms</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link fw-boldest shadow-sm fs-6 text-gray-800 p-4" runat="server" id="liReports2" data-toggle="tab" data-div-id="menu2" data-menu-id="menu3Header" href="#menu2" aria-controls="menu2" aria-selected="false">Reports</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link fw-boldest shadow-sm fs-6 text-gray-800 p-4" runat="server" id="liGlobe2" data-toggle="tab" data-menu-id="menu5Header" data-div-id="menu4" href="#menu4" aria-controls="menu4" aria-selected="false">Globalization</a>
                </li>
            </ul>
            <nav class="d-none">
                <div class=" nav rounded-pill flex-center bg-white overflow-auto flex-nowrap p-2 my-2 confignav" id="nav-tab" role="tablist">
                    <button class="flex-equal nav-link fw-boldest btn btn-active-light btn-color-gray-500 btn-active-color-gray-700 py-2 px-4 fs-6 fw-bold active" runat="server" data-menu-id="menu1Header" data-div-id="home" id="liApplication" data-bs-toggle="tab" data-bs-target="#home" type="button" role="tab" aria-controls="home" aria-selected="true">Application</button>
                    <asp:Label ID="lbltabapp" class="d-none" runat="server" meta:resourcekey="lbltabapp">Application</asp:Label>
                    <button class="flex-equal nav-link fw-boldest btn btn-active-light btn-color-gray-500 btn-active-color-gray-700 py-2 px-4 fs-6 fw-bold" runat="server" id="liForms" data-div-id="menu1" data-menu-id="menu2Header" data-bs-toggle="tab" data-bs-target="#menu1" type="button" role="tab" aria-controls="menu1" aria-selected="false">Forms</button>
                    <asp:Label ID="lblforms" class="d-none" runat="server" meta:resourcekey="lblforms">Forms</asp:Label>
                    <button class="flex-equal nav-link fw-boldest btn btn-active-light btn-color-gray-500 btn-active-color-gray-700 py-2 px-4 fs-6 fw-bold" runat="server" id="liReports" data-div-id="menu2" data-menu-id="menu3Header" data-bs-toggle="tab" data-bs-target="#menu2" type="button" role="tab" aria-controls="menu2" aria-selected="false">Reports</button>
                    <asp:Label ID="lblreport" class="d-none" runat="server" meta:resourcekey="lblreport">Reports</asp:Label>
                    <button class="flex-equal nav-link fw-boldest btn btn-active-light btn-color-gray-500 btn-active-color-gray-700 py-2 px-4 fs-6 fw-bold d-none" runat="server" data-menu-id="menu4Header" data-div-id="menu3" id="liDeveloper" data-bs-toggle="tab" data-bs-target="#menu3" type="button" role="tab" aria-controls="menu3" aria-selected="false">Page Builder</button>
                    <asp:Label ID="lbldeveloper" class="d-none" runat="server" meta:resourcekey="lbldeveloper">Page Builder</asp:Label>
                    <button class="flex-equal nav-link fw-boldest btn btn-active-light btn-color-gray-500 btn-active-color-gray-700 py-2 px-4 fs-6 fw-bold" runat="server" id="liGlobe" data-menu-id="menu5Header" data-div-id="menu4" data-bs-toggle="tab" data-bs-target="#menu4" type="button" role="tab" aria-controls="menu4" aria-selected="false">Globalization</button>
                    <asp:Label ID="lblGlobe" class="d-none" runat="server" meta:resourcekey="lblGlobe">Globalization</asp:Label>
                    <button class="flex-equal nav-link fw-boldest btn btn-active-light btn-color-gray-500 btn-active-color-gray-700 py-2 px-4 fs-6 fw-bold" runat="server" id="liUserSettings" data-menu-id="menu6Header" data-div-id="menu5" data-bs-toggle="tab" data-bs-target="#menu5" type="button" role="tab" aria-controls="menu5" aria-selected="false" style="display: none">User</button>
                    <asp:Label ID="lbluserset" class="d-none" runat="server" meta:resourcekey="lbluserset">User</asp:Label>
                    <button class="d-none flex-equal nav-link fw-boldest btn btn-active-light btn-color-gray-500 btn-active-color-gray-700 py-2 px-4 fs-6 fw-bold" id="liMenuSet" data-menu-id="menu7Header" runat="server" data-div-id="menu7" data-bs-toggle="tab" data-bs-target="#menu7" type="button" role="tab" aria-controls="menu7" aria-selected="false">Menu</button>
                    <asp:Label ID="Label1" class="d-none" runat="server" meta:resourcekey="lblmenuSet">Menu</asp:Label>
                </div>
            </nav>
            <ul class="cursor-pointer nav nav-tabs mb-n2 confignav d-none">
                <li class="active" id="liApplication1" data-menu-id="menu1Header" data-div-id="home"><a data-toggle="tab" href="#home" id="tabApplication" runat="server">
                    <asp:Label ID="lbltabapp1" runat="server" meta:resourcekey="lbltabapp">Application</asp:Label></a></li>
                <li id="liForms1" data-menu-id="menu2Header" data-div-id="menu1"><a data-toggle="tab" href="#menu1" id="tabForms" runat="server">
                    <asp:Label ID="lblforms1" runat="server" meta:resourcekey="lblforms">Forms</asp:Label></a></li>
                <li id="liReports1" data-menu-id="menu3Header" data-div-id="menu2"><a data-toggle="tab" href="#menu2" id="tabReports" runat="server">
                    <asp:Label ID="lblreport1" runat="server" meta:resourcekey="lblreport">Reports</asp:Label></a></li>
                <li id="liDeveloper1" class="d-none" data-menu-id="menu4Header" data-div-id="menu3" style="display: none;"><a data-toggle="tab" href="#menu3" id="tabDeveloper" runat="server">
                    <asp:Label ID="lbldeveloper1" class="d-none" runat="server" meta:resourcekey="lbldeveloper">Page Builder</asp:Label></a></li>
                <li id="liGlobe1" data-menu-id="menu5Header" data-div-id="menu4"><a data-toggle="tab" href="#menu4" id="tabGlobe" runat="server">
                    <asp:Label ID="lblGlobe1" runat="server" meta:resourcekey="lblGlobe">Globalization</asp:Label></a></li>
                <li id="liUserSettings1" data-menu-id="menu6Header" data-div-id="       " class="" runat="server" style="display: none"><a data-toggle="tab" href="#menu5">
                    <asp:Label ID="lbluserset1" runat="server" meta:resourcekey="lbluserset">User</asp:Label></a></li>
                <li id="liMenuSet1" class="d-none" data-menu-id="menu7Header" data-div-id="menu7" class=""><a data-toggle="tab" href="#menu7" id="tabMenuSet" runat="server">
                    <asp:Label ID="Label11" runat="server" meta:resourcekey="lblmenuSet">Menu</asp:Label></a></li>
            </ul>
            <div class="tab-content scrolldiv" id="nav-tabContent">
                <div id="home" runat="server" class="tab-pane fade show active configheight" role="tabpanel" aria-labelledby="nav-home-tab">
                    <!--wrapper content is toggele space -->
                    <div class="">
                        <div class="" id="accordion" role="tablist" aria-multiselectable="true">
                            <div class="card panel panel-default configpanel my-4 shadow-sm mx-2">
                                <div class=" card-header align-items-center panel-heading active configheading" role="tab">
                                    <div class="card-title">
                                        <h1 class="panel-title">
                                            <asp:Label ID="lblSettings" runat="server" meta:resourcekey="lblSettings">Settings</asp:Label>
                                        </h1>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default configpanel card shadow-sm mx-2">

                                <div class=" card-header align-items-center panel-heading active configheading" role="tab" id="headingOne">
                                    <div class="card-title">
                                        <h2 class="panel-title">
                                            <asp:Label ID="lblapplication" runat="server" meta:resourcekey="lblapplication">Application</asp:Label>
                                        </h2>
                                    </div>
                                </div>
                                <%--  <div class ="card-body">--%>
                                <div id="collapseOne" class="card-body" role="tabpanel" aria-labelledby="headingOne">
                                    <div class="panel-body">
                                        <!--Form control starts here-->
                                        <div class="row configformone">
                                            <div class="col-sm-12">
                                                <div class="form-group  d-none">
                                                    <asp:Label ID="lbltrace" for="txtTrace" runat="server" meta:resourcekey="lbltrace">Enable Trace</asp:Label>
                                                    <div class="radio rdopadding">
                                                        <label>
                                                            <input type="radio" name="optTrace" value="true"><asp:Label ID="lblopttrace" runat="server" meta:resourcekey="lblopttrace"> Yes</asp:Label></label>
                                                    </div>
                                                    <div class="radio">
                                                        <label>
                                                            <input type="radio" name="optTrace" value="false"><asp:Label ID="lblopttrace1" runat="server" meta:resourcekey="lblopttrace1">No</asp:Label></label>
                                                    </div>
                                                    <asp:HiddenField ID="hdnTrace" runat="server" />
                                                </div>
                                                <div class="form-group  d-none">
                                                    <div>
                                                        <asp:Label ID="lblmenu" runat="server" meta:resourcekey="lblmenu">Menu Style</asp:Label>
                                                        <div class="radio rdopadding">
                                                            <label>
                                                                <input type="radio" name="optMenu" value="false"><asp:Label ID="lbloptmenu" runat="server" meta:resourcekey="lbloptmenu">Classic</asp:Label></label>
                                                        </div>
                                                        <div class="radio">
                                                            <label>
                                                                <input type="radio" name="optMenu" value="true"><asp:Label ID="lbloptmenu1" runat="server" meta:resourcekey="lbloptmenu1">Modern</asp:Label></label>
                                                        </div>
                                                        <asp:HiddenField ID="hdnMenu" runat="server" />
                                                    </div>
                                                </div>

                                                <div class="form-group row my-2">
                                                    <div class="col-md-6 align-items-center justify-content-end">
                                                        <span id="lblenablebreadcrumb" class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lblenablebreadcrumb">Enable Breadcrumb</span>
                                                    </div>
                                                    <div class="col-md-6 d-flex align-items-center">
                                                        <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                            <input class="form-check-input w-45px h-25px" name="optOptBcrum" id="optBcrum" type="checkbox">
                                                            <label class="" for="optBcrum" id="lblOptBcrum"></label>
                                                        </a>
                                                    </div>
                                                    <asp:HiddenField ID="hdnBreadcrumb" runat="server" />
                                                </div>

                                                <div class="form-group row my-2 d-none">
                                                    <div class="col-md-6 align-items-center justify-content-end">
                                                        <span id="lblenableCards" class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lblenableCards" for="enable">Enable Homepage as Cards</span>
                                                    </div>
                                                    <div class="col-md-6 d-flex align-items-center">
                                                        <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                            <input class="form-check-input w-45px h-25px" name="optCards" id="enableCardsText" type="checkbox">
                                                            <label class="" for="enableCardsText" id="lblenablCardsText"></label>
                                                        </a>
                                                    </div>
                                                    <asp:HiddenField ID="hdnEnableCards" runat="server" />
                                                </div>

                                                <div class="form-group" id="divtally" runat="server" style="display: none">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lbltally" CssClass="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lbltally" for="enable">Enable Export to Tally</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid ms-auto m-0 ps-0 py-4">
                                                        <input class="m-wrap placeholder-no-fix form-check-input h-25px w-45px" name="optTally" id="enabletallyText" type="checkbox" />
                                                        <label class="" for="enabletallyText" id="lblenabletallyText"></label>
                                                    </a>
                                                    <asp:HiddenField ID="hdnTallyExport" runat="server" />
                                                </div>
                                                <div class="form-group row my-2">
                                                    <div class="col-md-6 align-items-center justify-content-end">
                                                        <span id="lblcpwd" class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lblcpwd" for="enable">Allow Change Password on first time login</span>
                                                    </div>
                                                    <div class="col-md-6 d-flex align-items-center">
                                                        <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                            <input class="form-check-input w-45px h-25px" name="optCPWD" id="enablelblcpwdText" type="checkbox">
                                                            <label class="" for="enablelblcpwdText" id="lblenablelblcpwdText"></label>
                                                        </a>
                                                    </div>
                                                    <asp:HiddenField ID="hdnCPOFL" runat="server" />
                                                </div>

                                                <div class="form-group row my-2 d-none">
                                                    <div class="col-md-6 align-items-center justify-content-end">
                                                        <span id="lblotpauth" class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lblotpauth" for="enable">OTP Authentication</span>
                                                    </div>
                                                    <div class="col-md-6 d-flex align-items-center">
                                                        <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                            <input class="form-check-input w-45px h-25px" name="optSessExt" id="enablelblotpauth" type="checkbox">
                                                            <label class="" for="enablelblsessText" id="lblenablelblotpauth"></label>
                                                        </a>
                                                    </div>
                                                    <asp:HiddenField ID="hdnotpauth" runat="server" />
                                                </div>

                                                <div class="d-none" id="otpauthproperty">
                                                    <div class="form-group row my-2">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblotpauthchars" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblotpauthchars" for="usr">OTP Authentication Characters</asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <select id="ddlotpauthcahrs" class="form-select" data-control="select2" runat="server" data-placeholder="OTP Authentication Characters">
                                                                <option value="3">3</option>
                                                                <option value="4">4</option>
                                                                <option value="5">5</option>
                                                                <option value="6">6</option>
                                                                <option value="7">7</option>
                                                                <option value="8">8</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div class="form-group row my-2">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblotpauthexpiry" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblotpauthexpiry" for="usr">OTP Authentication Expiry Time</asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <select id="ddlotpauthexpiry" class="form-select" data-control="select2" runat="server" data-placeholder="OTP Authentication Expiry Time">
                                                                <option value="1">1 min</option>
                                                                <option value="2">2 mins</option>
                                                                <option value="3">3 mins</option>
                                                                <option value="4">4 mins</option>
                                                                <option value="5">5 mins</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-group d-none">
                                                    <asp:Label ID="lblhelpview" runat="server" meta:resourcekey="lblhelpview" for="helpIview">Help Page</asp:Label>
                                                    <div>
                                                        <select class="form-control customconfiginput" id="ddlHelpIview" runat="server">
                                                            <option value=""></option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row my-2 d-none">
                                                    <div class="col-md-6 align-items-center justify-content-end">
                                                        <asp:Label ID="lblsess" CssClass="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lblsess" for="enable">Session Auto Extend</asp:Label>
                                                    </div>
                                                    <div class="col-md-6 d-flex align-items-center">
                                                        <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                            <input class="form-check-input w-45px h-25px" name="optSessExt" id="enablelblsessText" type="checkbox">
                                                            <label class="" for="enablelblsessText" id="lblenablelblsessText"></label>
                                                        </a>
                                                    </div>
                                                    <asp:HiddenField ID="hdnSessExt" runat="server" />
                                                </div>
                                                <div class="form-group row my-2 d-none">
                                                    <div class="col-md-6 align-items-center justify-content-end">
                                                        <asp:Label ID="lbltimezone" CssClass="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lbltimezone" for="enable">Allow Timezone Variation</asp:Label>
                                                    </div>
                                                    <div class="col-md-6 d-flex align-items-center">
                                                        <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                            <input class="form-check-input w-45px h-25px" name="allowTimezone" id="allowTimezone" type="checkbox">
                                                            <label class="" for="allowTimezone" id="lblallowTimezone"></label>
                                                        </a>
                                                    </div>
                                                    <asp:HiddenField ID="hdnAllowTimeZone" runat="server" />
                                                </div>

                                                <div class="form-group" style="display: none;">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lblDisableSplit" runat="server" meta:resourcekey="lblDisableSplit" for="enable">Disable Split</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                        <input class="tgl tgl-ios" name="optSessExt" id="enablelSplit" type="checkbox" />
                                                        <label class="tgl-btn togglecustom toggle_btn" for="enablelSplit" id="lblenablelSplit"></label>
                                                    </a>
                                                    <asp:HiddenField ID="hdnDisableSplit" runat="server" />
                                                </div>
                                                <div class="form-group row my-2 d-none">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblGlobalSrchTxt" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblGlobalSrchTxt" for="lblGlobalSrchTxt">Global Search Fetch Limit</asp:Label>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <input type="number" id="txtGlobalSrchLimit" maxlength="6" class="form-control placeholder-no-fix" runat="server" />
                                                    </div>
                                                </div>
                                                <%--to show enable error message and timeout--%>
                                                <div class="form-group" style="display: none;">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lblerr" runat="server" meta:resourcekey="lblenableErrormsg" for="enable">Hide Error Messages</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                        <input class="tgl tgl-ios" name="opterrormsg" id="enableErrorMsgText" type="checkbox" />
                                                        <label class="tgl-btn togglecustom toggle_btn" for="enableErrorMsgText" id="lblenableErrormsgText"></label>
                                                    </a>
                                                    <asp:HiddenField ID="hdnEnableErrorMsg" runat="server" />
                                                </div>
                                                <div class="form-group" id="divErrorTimeOut" style="display: none;">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lblErrorTimeout" runat="server" meta:resourcekey="lblErrorTimeout" for="txtErrorTimeout"></asp:Label></span>
                                                    <input type="number" maxlength="2" class="form-control customconfiginput" id="txtErrorTimeout" runat="server" />
                                                </div>
                                                <div class="form-group" style="display: none;">
                                                    <span class="setTitle">
                                                        <asp:Label ID="Label5" runat="server" meta:resourcekey="lblusr" for="enable">Enable Submit & Cancel                                                        buttons</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                        <input class="tgl tgl-ios" name="opterrormsg" id="showSubmitCancel1" type="checkbox" />
                                                        <label class="tgl-btn togglecustom toggle_btn" for="showSubmitCancel1" id="lblenableBtnText"></label>
                                                    </a>
                                                    <asp:HiddenField ID="showSubmitCancel" runat="server" />
                                                </div>
                                                <div class="form-group row my-2">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSessionExpiryDays" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblSessionExpiryDays" for="txtSessionExpiryDays">Application Session Expiry in Days</asp:Label>
                                                        <span class="material-icons material-icons-style align-middle material-icons-3 cursor-pointer" title="This is the value used in stay signed in functionality">info</span>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <input type="number" maxlength="2" min="1" max="10" id="txtSessionExpiryDays" class="form-control placeholder-no-fix" runat="server" />
                                                    </div>
                                                </div>

                                                <div class="form-group row my-2">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAlertTimeout" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblAlertTimeout" for="txtAlertTimeout">Alerts Timeout</asp:Label>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <input type="number" id="txtAlertTimeout" class="form-control placeholder-no-fix" runat="server" />
                                                    </div>
                                                </div>

                                                <div class="form-group row my-2">
                                                    <div class="col-md-6 align-items-center justify-content-end">
                                                        <span id="lblDevInstance" class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lblDevInstance" for="enable">Development Instance</span>
                                                    </div>
                                                    <div class="col-md-6 d-flex align-items-center">
                                                        <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                            <input class="form-check-input w-45px h-25px" id="enableDevInstance" type="checkbox">
                                                            <label class="tgl-btn togglecustom" for="enableDevInstance" id="lblenableDevInstance"></label>
                                                        </a>
                                                    </div>
                                                    <asp:HiddenField ID="hdnDevInstance" runat="server" />
                                                </div>

                                                <div class="form-group row my-2">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAttachmentSize" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblAttachmentSize">Max Attachment Size in MB</asp:Label>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <select id="ddlAttachmentSize" class="form-select my-2--" data-control="select2" runat="server" data-placeholder="Select Attachment Size">
                                                            <option value="1" selected="selected">1</option>
                                                            <option value="2">2</option>
                                                            <option value="3">3</option>
                                                            <option value="4">4</option>
                                                            <option value="5">5</option>
                                                            <option value="6">6</option>
                                                            <option value="7">7</option>
                                                            <option value="8">8</option>
                                                            <option value="9">9</option>
                                                            <option value="10">10</option>
                                                            <option value="20">20</option>
                                                            <option value="40">40</option>
                                                            <option value="50">50</option>
                                                            <option value="70">70</option>
                                                            <option value="250">250</option>
                                                            <option value="500">500</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row my-2">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblapptitle" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblapptitle">Application Title</asp:Label>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <input type="text" runat="server" class="m-wrap my-4-- placeholder-no-fix form-control " id="txtAppTitle" maxlength="100" />
                                                    </div>
                                                </div>

                                                <div class="form-group row my-10">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblapplogo" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblapplogo">Application Logo</asp:Label>
                                                    </div>
                                                    <div class="col-md-6 avatar-wrapper">
                                                        <div class="image-input image-input-outline" data-kt-image-input="true" style="background-image: url()">
                                                            <img class="profile-pic image-input-wrapper w-70px h-70px" src="../images/loginlogo.png" />
                                                            <label class="btn btn-icon btn-circle btn-active-color-primary w-25px h-25px bg-white shadow"
                                                                data-kt-image-input-action="change"
                                                                data-bs-toggle="tooltip"
                                                                data-bs-dismiss="click"
                                                                title="Change avatar">
                                                                <i class="material-icons">edit</i>
                                                                <input class="file-upload hide" id="UploadAppLogoImg" data-type="logo" type="file" name="avatar" accept=".png, .jpg, .jpeg" />
                                                                <input type="hidden" name="avatar_remove" />
                                                            </label>
                                                            <span class="delete-button d-flex btn btn-icon btn-circle btn-active-color-primary w-25px h-25px bg-white shadow "
                                                                data-kt-image-input-action="remove"
                                                                data-bs-toggle="tooltip"
                                                                data-bs-dismiss="click"
                                                                title="Remove avatar">
                                                                <i class="material-icons">delete</i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-group  d-none">
                                                    <asp:Label ID="lblactsess" runat="server" meta:resourcekey="lblactsess" for="enable">Display Active sessions in Login page</asp:Label>
                                                    <div class="radio rdopadding">
                                                        <label>
                                                            <input type="radio" name="optActSess" value="true"><asp:Label ID="lbloptactsess" runat="server" meta:resourcekey="lblactsess">Yes</asp:Label></label>
                                                    </div>
                                                    <div class="radio">
                                                        <label>
                                                            <input type="radio" name="optActSess" value="false"><asp:Label ID="lbloptactsess1" runat="server" meta:resourcekey="lbloptactsess1">No</asp:Label></label>
                                                    </div>
                                                    <asp:HiddenField ID="hdnActSess" runat="server" />
                                                </div>

                                                <div class="form-group row my-2 d-none">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblWizardType" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblWizardType" for="usr">Wizard Type</asp:Label>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <select id="ddlWizardType" class="form-select" data-control="select2" runat="server" data-placeholder="Select Wizard Type">
                                                            <option value="classic">Classic</option>
                                                            <option value="modern">Modern</option>
                                                        </select>
                                                    </div>
                                                </div>

                                                <div class="form-group" style="display: none">
                                                    <asp:Label ID="lblimptmp" runat="server" meta:resourcekey="lblimptmp" for="enable">Import Template Path</asp:Label>
                                                    <input type="text" runat="server" class="form-control" id="txtImpEmpTmpPath" maxlength="255" />

                                                </div>


                                                <div class="form-group  d-none">
                                                    <asp:Label ID="lblUploadAppImg" runat="server" meta:resourcekey="lblUploadAppImg" for="enable">Background Image/Video</asp:Label>
                                                    <span class="file-upload">
                                                        <span class="file-select" tabindex="0" id="spnFileSelect">
                                                            <span class="file-select-button" id="fileName">
                                                                <asp:Label ID="lblfilename" runat="server" meta:resourcekey="lblfilename">Choose File</asp:Label></span>
                                                            <span class="file-select-name" id="noFile" style="width: 50%">
                                                                <asp:Label ID="lblnofile" runat="server" meta:resourcekey="lblnofile">No file chosen...</asp:Label></span>

                                                            <asp:FileUpload runat="server" ID="UploadAppImg" Style="margin-top: -1px;" TabIndex="-1" />
                                                        </span>
                                                    </span>
                                                </div>
                                                <div class="form-group  d-none">
                                                    <asp:Label ID="lblUploadMobAppImage" runat="server" meta:resourcekey="lblUploadMobAppImage" for="enable">Mobile Background Image</asp:Label>
                                                    <span class="file-upload">
                                                        <span class="file-select" tabindex="0" id="spnFileSelect2">
                                                            <span class="file-select-button" id="fileName2">
                                                                <asp:Label ID="lblfilename2" runat="server" meta:resourcekey="lblfilename">Choose File</asp:Label></span>
                                                            <span class="file-select-name" id="noFile2" style="width: 50%">
                                                                <asp:Label ID="lblnofile2" runat="server" meta:resourcekey="lblnofile">No file chosen...</asp:Label></span>
                                                            <asp:FileUpload runat="server" ID="UploadAppMobImg" Style="margin-top: -1px;" TabIndex="-1" />
                                                        </span>
                                                    </span>
                                                </div>
                                                <div class="form-group" style="display: none;">
                                                    <asp:Label ID="lblUploadAppLogoImg" runat="server" meta:resourcekey="lblUploadAppLogoImg" for="enable">Logo</asp:Label>
                                                    <span class="file-upload">
                                                        <span class="file-select" tabindex="0" id="spnFileSelect3">
                                                            <span class="file-select-button" id="fileName3">
                                                                <asp:Label ID="lblfilename3" runat="server" meta:resourcekey="lblfilename">Choose File</asp:Label></span>
                                                            <span class="file-select-name" id="noFile3" style="width: 50%">
                                                                <asp:Label ID="lblnofile3" runat="server" meta:resourcekey="lblnofile">No file chosen...</asp:Label></span>
                                                            <asp:FileUpload runat="server" ID="UploadAppLogoImg" Style="margin-top: -1px;" TabIndex="-1" />
                                                        </span>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <!--ends here -->
                                    </div>
                                    <%--</div>--%>
                                </div>
                                <div class="card-header align-items-center panel-heading  active configheading" role="tab" id="headingTwo">
                                    <div class="card-title">
                                        <h2 class="panel-title">
                                            <asp:Label ID="lblglobal" runat="server" meta:resourcekey="lblglobal">Supported Languages</asp:Label>
                                        </h2>
                                    </div>
                                </div>
                                <div id="collapseTwo" class="card-body" role="tabpanel" aria-labelledby="headingTwo">
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <span class="setTitle">
                                                <asp:Label ID="lbllanguage" runat="server" meta:resourcekey="lbllanguage" for="sel1" Visible="false">Application Languages</asp:Label>

                                            </span>
                                            <div id="dvUserLeveLangs" class="form-group row my-2">
                                                <div class="col-md-6 align-items-center justify-content-end">
                                                    <span class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0">Apply user level language</span>
                                                </div>
                                                <div class="col-md-6 d-flex align-items-center">
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                        <input class="form-check-input w-45px h-25px" name="optUserLang" id="optUserLang" type="checkbox">
                                                    </a>
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hdnUserLevelLang" runat="server" />
                                            <div id="dvLangs" class="form-group row my-2">
                                                <div class="col-md-6 align-items-center justify-content-end">
                                                    <span class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0">Select Languages</span>
                                                </div>
                                                <div class="col-md-6 d-flex align-items-center multiSelWrap">
                                                    <%-- <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                        <input class="form-check-input w-45px h-25px" name="optLang" value="english" type="checkbox">
                                                    </a>--%>

                                                    <select id="langMultiselect" class="form-select" data-control="select2" multiple>
                                                        <option value="english">English</option>
                                                    </select>

                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hdnLanguage" runat="server" />
                                            <div id="dvLangInputsNew" runat="server">
                                            </div>
                                        </div>

                                        <div class="form-group row my-2" style="display: none">
                                            <div class="col-md-6 align-items-center justify-content-end">
                                                <asp:Label ID="lbldate" runat="server" meta:resourcekey="lbldate" for="enable">Date Format Culture</asp:Label>
                                            </div>
                                            <div class="col-md-6 d-flex align-items-center">
                                                <div class="radio rdopadding">
                                                    <label class="form-check-label form-label col-form-label pb-1 fw-boldest">
                                                        <input type="radio" name="optCulture" value="true"><asp:Label ID="lbloptculture" runat="server" meta:resourcekey="lbloptculture">US</asp:Label></label>
                                                </div>
                                                <div class="radio">
                                                    <label class="form-check-label form-label col-form-label pb-1 fw-boldest">
                                                        <input type="radio" name="optCulture" value="false"><asp:Label ID="lbloptculture1" runat="server" meta:resourcekey="lbloptculture1">German</asp:Label></label>
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hdnCulture" runat="server" />
                                        </div>

                                        <div class="form-group row my-2 d-none">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblLangSelect" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblLangSelect">Application Language</asp:Label>
                                            </div>
                                            <div class="col-md-6">
                                                <select id="langSelect" class="form-select my-4--" data-control="select2" runat="server" data-placeholder="Select Language">
                                                </select>
                                            </div>
                                        </div>


                                        <div class="col-sm-6 d-none">
                                            <div class="form-group">
                                                <asp:Label ID="lblcopyright" class="form-check-label form-label col-form-label pb-1 fw-boldest" for="txtCopyRightText" runat="server" meta:resourcekey="lblcopyright">App Copyright Text</asp:Label>
                                                <input type="text" runat="server" class="m-wrap my-4 placeholder-no-fix form-control " id="txtCopyRightText" maxlength="255" />
                                            </div>
                                        </div>
                                        <div class="form-group row my-2 d-none">
                                            <div class="col-md-6">
                                                <asp:Label ID="lbltitle" class="form-check-label form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lbltitle" for="usr">Form Print Title</asp:Label>
                                            </div>
                                            <div class="col-md-6">
                                                <input type="text" class="m-wrap my-4-- placeholder-no-fix form-control " id="txtPrintTitle" runat="server" maxlength="255" />
                                            </div>
                                        </div>
                                        <div id="dvLangInputs" runat="server">
                                        </div>
                                        <input type="hidden" id="hdnLangKeys" runat="server" />
                                        <input type="hidden" id="hdnLangVals" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div id="collapseeight" class="" role="tabpanel" aria-labelledby="headingseven">
                                <div class="panel-body">
                                    <div class="row configformone" id="glLangMainWrapper">
                                        <div class="col-sm-6">
                                            <div class="form-group ui-widget glLangSrcFld d-none" id="lblddlLanguage">
                                                <asp:Label ID="lbllang" runat="server" meta:resourcekey="lbllang">Target Language</asp:Label>
                                                <div class="ddlbtn">
                                                    <input type="text" onclick="createLangAutoComp()" id="ddlLanguage" value="" text="" runat="server" placeholder="Select Target language" autocomplete="off" class="customfontico fldAutocomplete combotem Family form-control  fastdll ui-autocomplete-input" />
                                                    <div class="autoclear autoinputtxtclear clearico"><i class="fa fa-times" title="clear" onclick="document.getElementById('ddlLanguage').value = ''"></i></div>
                                                    <div class="edit"><i class="fa fa-chevron-down autoClickddl" title="select" onclick="createLangAutoComp();" data-clk="selectddlSource"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                        <input type="hidden" id="hdnAxLangSrc" runat="server" />
                                        <div class="col-sm-6">
                                            <div class="form-group ui-widget glLangSrcFld d-none" id="lblddlsource">
                                                <asp:Label ID="lblsource" runat="server" meta:resourcekey="lblsource" For="ddlSource">Form</asp:Label>
                                                <div class="ddlbtn">
                                                    <input onclick="createSrcAutoComp()" type="text" id="ddlSource" placeholder="Select Form" runat="server" class="customfontico fldAutocomplete combotem Family fldA form-control  fastdll ui-autocomplete-input" autocomplete="off" />
                                                    <div class="autoclear autoinputtxtclear clearico"><i class="fa fa-times" id="clrddlSource" title="clear" onclick="document.getElementById('ddlSource').value = ''"></i></div>
                                                    <div class="edit"><i class="fa fa-chevron-down autoClickddl" id="sid" onclick="createSrcAutoComp();" title="select" data-clk="ddlSource"></i></div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div id="dvContent" runat="server"></div>
                                </div>
                            </div>

                            <div class="card panel panel-default configpanel my-4 shadow-sm mx-2 d-none">
                                <div class="card-header align-items-center panel-heading active configheading" role="tab" id="headingTwo">
                                    <h4 class="panel-title m-4--">
                                        <asp:Label ID="lblformsdiv" runat="server" meta:resourcekey="lblformsdiv">Forms</asp:Label>
                                    </h4>
                                </div>
                                <div id="collapseThree" class="card-body" role="tabpanel" aria-labelledby="headingTwo">
                                    <div class="panel-body">

                                        <div class="row configformone">
                                            <div class="col-sm-12">
                                                <%-- <div class="form-group row my-2">
                                                    <div class="col-md-6 align-items-center justify-content-end">
                                                        <span id="lblDevInstance" class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lblDevInstance" for="enable">Development Instance</span>
                                                    </div>
                                                    <div class="col-md-6 d-flex align-items-center">
                                                        <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch form-check-solid m-0 ps-0-- py-4">
                                                            <input class="form-check-input w-45px h-25px" id="enableDevInstance" type="checkbox">
                                                            <label class="tgl-btn togglecustom" for="enableDevInstance" id="lblenableDevInstance"></label>
                                                        </a>
                                                    </div>
                                                    <asp:HiddenField ID="hdnDevInstance" runat="server" />
                                                </div>--%>

                                                <div class="form-group" style="display: none">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lblSaveAndPublish" runat="server" meta:resourcekey="lblSaveAndPublish" for="enable">Save and Auto Publish</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                        <input class="tgl tgl-ios" id="chkAutoSavePublish" type="checkbox" />
                                                        <label class="tgl-btn togglecustom toggle_btn me-2" for="chkAutoSavePublish" id="lblchkAutoSavePublish"></label>
                                                    </a>
                                                    <asp:HiddenField ID="hdnAutoSavePublish" runat="server" />
                                                </div>
                                                <div class="form-group" style="display: none">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lblautognr" runat="server" meta:resourcekey="lblautognr" for="usr">Show Auto generated Field Value</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                        <input class="tgl tgl-ios" name="optAutoGen" id="enableautognrText" type="checkbox" />
                                                        <label class="tgl-btn togglecustom toggle_btn" for="enableautognrText" id="lblenableautognrText"></label>
                                                    </a>
                                                    <asp:HiddenField ID="hdnAutoGen" runat="server" />
                                                </div>
                                                <div class="form-group" style="display: none">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lblslct" runat="server" meta:resourcekey="lblslct" for="usr">Enable AutoComplete for Select and Picklist fields</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                        <input class="tgl tgl-ios" name="optPerf" id="enablelblslctText" type="checkbox" />
                                                        <label class="tgl-btn togglecustom toggle_btn" for="enablelblslctText" id="lblenablelblslctText"></label>
                                                    </a>
                                                    <asp:HiddenField ID="hdnIsPerfCode" runat="server" />
                                                </div>
                                                <div class="form-group">
                                                    <div class="form-inline" style="display: none">
                                                        <div class="form-group">
                                                            <asp:Label ID="lblInlineGridEditTitle" runat="server" meta:resourcekey="lblInlineGridEdit" for="usr">Grid Data Edit</asp:Label>
                                                        </div>
                                                        <div class="form-group" style="margin-left: 10px;">
                                                            <select id="ddlInlineGrid" class="form-control" runat="server">
                                                                <option value="popup">Popup Dialog</option>
                                                                <option value="inline">Inline</option>
                                                            </select>
                                                        </div>
                                                        <asp:HiddenField ID="hdnInlineGridEdit" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="form-group" style="display: none">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lbloptdcgrid" runat="server" meta:resourcekey="lbloptdcgrid" for="optDcGrid">Grid DC Pop Up Visible On Save</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                        <input class="tgl tgl-ios" name="optDcGrid" id="enableoptdcgridText" type="checkbox" />
                                                        <label class="tgl-btn togglecustom toggle_btn" for="enableoptdcgridText" id="lblenableoptdcgridText"></label>
                                                    </a>
                                                    <asp:HiddenField ID="hdnDcGridOnSave" runat="server" />
                                                </div>


                                                <div class="form-group" runat="server" visible="false">
                                                    <asp:Label ID="lblimgfld" runat="server" meta:resourcekey="lblimgfld" for="txtImagePath">Image field storage path</asp:Label>
                                                    <input type="text" class="form-control" runat="server" id="txtImagePath" maxlength="255" />
                                                </div>
                                                <div class="form-group" runat="server" visible="false">
                                                    <asp:Label ID="lblattachpath" runat="server" meta:resourcekey="lblattachpath" for="txtAttachPath">Form Attachment storage path</asp:Label>
                                                    <input type="text" class="form-control" runat="server" id="txtAttachPath" maxlength="255" />
                                                </div>
                                                <div class="form-group" runat="server" visible="false">
                                                    <asp:Label ID="lblgridattachpath" runat="server" meta:resourcekey="lblgridattachpath" for="txtGridAttachPath">Grid Attachment storage path</asp:Label>
                                                    <input type="text" class="form-control" runat="server" id="txtGridAttachPath" maxlength="255" />
                                                </div>
                                                <div class="form-group" style="display: none;">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lblEnableDraft" runat="server" meta:resourcekey="lblEnableDraft" for="enable">Save As Draft</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                        <input class="tgl tgl-ios" name="optDirectDb" id="ckbEnabledDraft" type="checkbox" />
                                                        <label class="tgl-btn togglecustom toggle_btn" for="ckbEnabledDraft" id="lblEnableDraftText"></label>
                                                    </a>
                                                    <asp:HiddenField ID="hdnEnableDrafts" runat="server" />
                                                </div>

                                                <div class="form-group" style="display: none;">
                                                    <span class="setTitle">
                                                        <asp:Label ID="lblAutoPurge" runat="server" meta:resourcekey="lblAutoPurge" for="enable">Auto Purge Drafts</asp:Label></span>
                                                    <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                        <input class="tgl tgl-ios" name="optDirectDb" id="ckbAutoPurge" type="checkbox" />
                                                        <label class="tgl-btn togglecustom toggle_btn" for="ckbAutoPurge" id="lblAutoPurgeText"></label>
                                                    </a>
                                                    <asp:HiddenField ID="hdnAutoPurge" runat="server" />
                                                </div>

                                                <div class="form-group" style="display: none;">
                                                    <asp:Label ID="lblMaxDraftsCount" runat="server" meta:resourcekey="lblMaxDraftsCount" for="txtMaxDraftsCount">Max Drafts Count</asp:Label>
                                                    <input type="number" placeholder="1 to 99" class="form-control" runat="server" id="txtMaxDraftsCount" onchange="handleChange(this);" maxlength="99" min="1" max="99" />
                                                </div>

                                                <div class="form-group" style="display: none;">
                                                    <asp:Label ID="lblDesignMode" runat="server" meta:resourcekey="lblDesignMode" for="designerResp">Enable Design Mode (Please select the responsibilities) </asp:Label>
                                                    <div class="col multiSelWrap">
                                                        <select id="designerResp" class="form-control1 browser-default rlmultiSlectFld d-none" size="8" multiple="multiple">
                                                        </select>
                                                    </div>
                                                    <asp:HiddenField ID="hdnDesignerResp" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="card panel panel-default configpanel my-4 shadow-sm mx-2 d-none">
                                <div class="card-header align-items-center panel-heading  active configheading" role="tab" id="headingTwo">
                                    <div class="card-title">
                                        <h4 class="panel-title">
                                            <asp:Label ID="lbluiconfig" runat="server" meta:resourcekey="lbluiconfig">UI Configuration</asp:Label>

                                        </h4>
                                    </div>
                                </div>
                                <div id="collapseTwo" class="card-body" role="tabpanel" aria-labelledby="headingTwo">
                                    <div class="dbUIWrapper card-body row w-lg-1000px p-8 p-lg-12 mx-auto">
                                        <div class="dbUIBody">
                                            <div class="row">
                                                <%--<div class="col-sm-4">
                                                    <div class="dbUIImgTitle p-2">
                                                        <h4>Application Logo</h4>
                                                    </div>
                                                    <div class="avatar-wrapper">
                                                        <div class="image-input image-input-outline" data-kt-image-input="true" style="background-image: url()">
                                                            <img class="profile-pic image-input-wrapper w-70px h-70px" src="../images/loginlogo.png" />
                                                            <label class="btn btn-icon btn-circle btn-active-color-primary w-25px h-25px bg-white shadow"
                                                                data-kt-image-input-action="change"
                                                                data-bs-toggle="tooltip"
                                                                data-bs-dismiss="click"
                                                                title="Change avatar">
                                                                <i class="material-icons">edit</i>
                                                                <input class="file-upload hide" id="UploadAppLogoImg" data-type="logo" type="file" name="avatar" accept=".png, .jpg, .jpeg" />
                                                                <input type="hidden" name="avatar_remove" />
                                                            </label>
                                                            <span class="delete-button d-flex btn btn-icon btn-circle btn-active-color-primary w-25px h-25px bg-white shadow "
                                                                data-kt-image-input-action="remove"
                                                                data-bs-toggle="tooltip"
                                                                data-bs-dismiss="click"
                                                                title="Remove avatar">
                                                                <i class="material-icons">delete</i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                <div class="col-sm-4">
                                                    <div class="dbUIImgTitle p-2 ">
                                                        <h4>Web Project Background</h4>
                                                    </div>
                                                    <div class="image-input image-input-outline" data-kt-image-input="true" style="background-image: url(/assets/media/svg/avatars/blank.svg)">
                                                        <img class="profile-pic image-input-wrapper w-250px h-150px" src="../AxpImages/loginlogo.png" />
                                                        <label class="btn btn-icon btn-circle btn-active-color-primary w-25px h-25px bg-white shadow"
                                                            data-kt-image-input-action="change"
                                                            data-bs-toggle="tooltip"
                                                            data-bs-dismiss="click"
                                                            title="Change avatar">
                                                            <i class="material-icons">edit</i>
                                                            <input class="file-upload d-none" id="UploadWebBgImg" data-type="webbg" type="file" name="avatar" accept=".png, .jpg, .jpeg" />
                                                            <input type="hidden" name="avatar_remove" />
                                                        </label>
                                                        <span class="delete-button  d-flex btn btn-icon btn-circle btn-active-color-primary w-25px h-25px bg-white shadow"
                                                            data-kt-image-input-action="remove"
                                                            data-bs-toggle="tooltip"
                                                            data-bs-dismiss="click"
                                                            title="Remove avatar">
                                                            <i class="material-icons">delete</i>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <div class="dbUIImgTitle p-2">
                                                        <h4>Mobile Project Background</h4>
                                                    </div>
                                                    <div class="avatar-wrapper">
                                                        <div class="image-input image-input-outline" data-kt-image-input="true" style="background-image: url(/assets/media/avatars/blank.png)">
                                                            <img class="profile-pic image-input-wrapper w-250px h-150px" src="../AxpImages/loginlogo.png" />
                                                            <label class=" upload-button btn btn-icon btn-sm btn-circle btn-active-color-primary w-25px h-25px bg-white shadow"
                                                                data-kt-image-input-action="change"
                                                                data-bs-toggle="tooltip"
                                                                data-bs-dismiss="click"
                                                                title="Change avatar">
                                                                <i class="material-icons">edit</i>

                                                                <!--begin::Inputs-->
                                                                <input class="file-upload d-none" id="UploadMobBgImg" data-type="mobbg" type="file" name="avatar" accept=".png, .jpg, .jpeg" />
                                                                <input type="hidden" name="avatar_remove" />
                                                                <!--end::Inputs-->
                                                            </label>
                                                            <span class="delete-button  d-flex btn btn-icon btn-circle btn-active-color-primary w-25px h-25px bg-white shadow"
                                                                data-kt-image-input-action="remove"
                                                                data-bs-toggle="tooltip"
                                                                data-bs-dismiss="click"
                                                                title="Remove avatar">
                                                                <i class="material-icons">delete</i>
                                                            </span>
                                                            <!--end::Remove button-->
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="menu1" runat="server" class="tab-pane fade show active configheight">
                    <div>

                        <div class="panel panel-default configpanel" style="display: none;">
                            <div class="card-header align-items-center panel-heading  active configheading" role="tab" id="headingfour">
                                <h4 class="panel-title">
                                    <a class="collapsed" role="button" data-toggle="collapse" href="#collapsefour" aria-expanded="true" aria-controls="collapsefour" title="Hide"></a>
                                    <asp:Label ID="lblprint" runat="server" meta:resourcekey="lblprint">Print</asp:Label>

                                </h4>
                            </div>
                            <div id="collapsefour" class="" role="tabpanel" aria-labelledby="headingfour">
                                <div class="panel-body">


                                    <div class="row configformone">
                                        <div class="col-sm-6">

                                            <div class="form-group" id="divprint32" runat="server" style="display: none;">
                                                <asp:Label ID="lblprint32" runat="server" meta:resourcekey="lblprint32" for="usr">Print for 32bit/64bit</asp:Label>
                                                <div class="radio rdopadding">
                                                    <label>
                                                        <input type="radio" name="optexe" value="true"><asp:Label ID="lbloptprint32" runat="server" meta:resourcekey="lbloptprint32">32-bit</asp:Label></label>
                                                </div>
                                                <div class="radio">
                                                    <label>
                                                        <input type="radio" name="optexe" value="false"><asp:Label ID="lbloptprint64" runat="server" meta:resourcekey="lbloptprint64">64-bit</asp:Label></label>
                                                </div>
                                                <asp:HiddenField ID="hdnIsPrintExe" runat="server" />
                                            </div>

                                            <div class="form-group" id="dv32bit">
                                                <asp:Label ID="lblexepath" runat="server" meta:resourcekey="lblexepath" for="usr">Print Server Path(wktohtmlpdf)</asp:Label>
                                                <input type="text" class="form-control" id="txtExePath" runat="server" maxlength="255" />
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lblhtmlpath" runat="server" meta:resourcekey="lblhtmlpath" for="usr">Template path (HTML)</asp:Label>
                                                <input type="text" class="form-control" id="txtPrintPath" runat="server" maxlength="255" />
                                            </div>
                                            <div class="form-group" id="divmargins" runat="server" style="display: none;">
                                                <asp:Label ID="lblmargins" runat="server" meta:resourcekey="lblmargins" for="usr">Print Margins - Top, Right, Bottom, Left</asp:Label>
                                                <div class="printlabel">
                                                    <input type="text" maxlength="3" class="form-control cofigdirection" id="pmtop" onblur="return ValidateNumeric(this);" />
                                                    <input type="text" maxlength="3" class="form-control cofigdirection" id="pmright" onblur="return ValidateNumeric(this);" />
                                                    <input type="text" maxlength="3" class="form-control cofigdirection" id="pmbottom" onblur="return ValidateNumeric(this);" />
                                                    <input type="text" maxlength="3" class="form-control cofigdirection" id="pmleft" onblur="return ValidateNumeric(this);" />
                                                    <asp:HiddenField ID="hdnPrintMargins" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group" id="divtitlealign" runat="server" style="display: none;">
                                                <asp:Label ID="lbltitlealign" runat="server" meta:resourcekey="lbltitlealign" for="usr">Title Alignment</asp:Label>
                                                <select class="form-control customconfiginput" id="ddlTitAlignment" runat="server">
                                                    <option>Left</option>
                                                    <option>Center</option>
                                                    <option>Right</option>
                                                </select>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <!--menu2 begins  here -->
                <div id="menu2" runat="server" class="tab-pane fade configheight d-none">
                    <div>
                        <div class="panel panel-default configpanel card shadow-sm mx-2">
                            <div class="card-header align-items-center panel-heading  active configheading" role="tab" id="headingfive">
                                <div class="card-title">
                                    <h4 class="panel-title">
                                        <asp:Label ID="lblgeneralc5" runat="server" meta:resourcekey="lblgeneralc5">General</asp:Label>
                                    </h4>
                                </div>
                            </div>
                            <div id="collapsefive" class="card-body" role="tabpanel" aria-labelledby="headingfive">
                                <div class="panel-body">
                                    <div class="row configformone">
                                        <div class="col-sm-12">
                                            <div class="form-group  d-none">
                                                <span class="setTitle">
                                                    <asp:Label ID="lblGetIviewRowCountTitle" runat="server" meta:resourcekey="lblGetIviewRowCountTitle" for="chkGetIviewRowCount">Get Iview Row Count</asp:Label></span>
                                                <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                    <input class="tgl tgl-ios" name="getIviewRowCount" id="chkGetIviewRowCount" type="checkbox" />
                                                    <label class="tgl-btn togglecustom toggle_btn" for="chkGetIviewRowCount" id="lblGetIviewRowCount"></label>
                                                </a>
                                                <asp:HiddenField ID="hdnGetIviewRowCount" runat="server" />
                                            </div>

                                            <div class="form-group  d-none">
                                                <span class="setTitle">
                                                    <asp:Label ID="lblIviewDataWSRowsTitle" runat="server" meta:resourcekey="lblIviewDataWSRowsTitle" for="txtIviewDataWSRows">Iview Records to Cache with every Web Service Call</asp:Label></span>
                                                <input class="form-control" runat="server" name="iviewDataWSRows" id="txtIviewDataWSRows" maxlength="5" type="number" />
                                            </div>


                                            <div class="form-group  d-none">
                                                <asp:Label ID="lblgrpdup" runat="server" meta:resourcekey="lblgrpdup" for="usr">Group duplicate value</asp:Label>
                                                <input type="text" class="form-control customconfiginput" id="txtMergeRows" runat="server">
                                            </div>
                                            <div class="form-group  d-none">
                                                <asp:Label ID="lblmaxprint" runat="server" meta:resourcekey="lblmaxprint" for="usr">Maximum number of rows to print</asp:Label>
                                                <input type="number" maxlength="5" class="form-control customconfiginput" id="txtMaxRowsToPrint" runat="server" onblur="return ValidateNumeric(this);" />
                                            </div>
                                            <div class="form-group  d-none">
                                                <asp:Label ID="lbldbpag" runat="server" meta:resourcekey="lbldbpag" for="usr">Enable DB pagination</asp:Label>
                                                <div class="radio rdopadding">
                                                    <label>
                                                        <input type="radio" name="optDbP" value="true"><asp:Label ID="lbloptdbpag" runat="server" meta:resourcekey="lbloptdbpag">Yes</asp:Label></label>
                                                </div>
                                                <div class="radio">
                                                    <label>
                                                        <input type="radio" name="optDbP" value="false"><asp:Label ID="lbloptdbpag1" runat="server" meta:resourcekey="lbloptdbpag1">No</asp:Label></label>
                                                </div>
                                                <asp:HiddenField ID="hdnDbPagination" runat="server" />
                                            </div>

                                            <div class="form-group  d-none">
                                                <span class="setTitle">
                                                    <asp:Label ID="lbltextwrapc5" runat="server" meta:resourcekey="lbltextwrapc5" for="usr">Column word wrap</asp:Label></span>
                                                <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                    <input class="tgl tgl-ios" name="optWrap" id="enabletextwrapc5Text" type="checkbox" />
                                                    <label class="tgl-btn togglecustom toggle_btn" for="enabletextwrapc5Text" id="lblenabletextwrapc5Text"></label>
                                                </a>
                                                <asp:HiddenField ID="hdnTextWrap" runat="server" />
                                            </div>

                                            <div class="form-group  d-none">
                                                <asp:Label ID="lblrowpage" runat="server" meta:resourcekey="lblrowpage" for="usr">Rows per page</asp:Label>
                                                <input type="text" maxlength="5" class="form-control customconfiginput" id="txtPageRowCount" runat="server" onblur="return ValidateNumeric(this);" />
                                            </div>
                                            <div class="form-group form-switch form-check form-check-custom form-check-solid col-md-6 d-none">
                                                <span class="setTitle">
                                                    <asp:Label ID="lbltitledwn" class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lbltitledwn" for="usr">Show App Title in downloads</asp:Label></span>
                                                <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid ms-auto m-0 ps-0 py-4">
                                                    <input class="form-check-input ms-3 w-45px h-25px" name="optTitlePrint" id="enabletitledwn" type="checkbox" />
                                                    <label class="" for="enabletitledwn" id="lblenabletitledwn"></label>
                                                </a>
                                                <asp:HiddenField ID="hdnShowAppTitle" runat="server" />
                                            </div>

                                            <div class="form-group form-switch form-check form-check-custom form-check-solid col-md-6 d-none">
                                                <span class="setTitle">
                                                    <asp:Label ID="lblstripedreport" class="form-check-label form-label col-form-label pb-1 fw-boldest ms-0" runat="server" meta:resourcekey="lblstripedreport" for="usr">Enable Striped Report</asp:Label></span>
                                                <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid ms-auto m-0 ps-0 py-4">
                                                    <input class="form-check-input ms-3 w-45px h-25px" name="optstripedreport" id="enablestripedreport" type="checkbox" />
                                                    <label class="" for="enabletitledwn" id="lblenablestripedreport"></label>
                                                </a>
                                                <asp:HiddenField ID="hdnShowstripedreport" runat="server" />
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!--menu3 begins here -->
                <div id="menu3" runat="server" class="tab-pane fade configheight d-none">
                    <div>
                        <div class="panel panel-default configpanel card shadow-sm mx-2">
                            <div class="card-header align-items-center panel-heading  active configheading" role="tab" id="headingsix">
                                <div class="card-title">
                                    <h4 class="panel-title">
                                        <asp:Label ID="lblgeneralc6" runat="server" meta:resourcekey="lblgeneralc6">General </asp:Label>
                                    </h4>
                                </div>
                            </div>
                            <div id="collapsesix" class="card-body" role="tabpanel" aria-labelledby="headingsix">
                                <div class="panel-body">
                                    <div class="row configformone">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <asp:Label ID="lblgeneralsel1" runat="server" meta:resourcekey="lblgeneralsel1">Page builder access (Please select the responsibilities) </asp:Label>
                                                <div class="col multiSelWrap">
                                                    <select id="homeBuildResp" class="form-control1 browser-default rlmultiSlectFld d-none" size="8" multiple="multiple">
                                                    </select>
                                                </div>
                                                <asp:HiddenField ID="hdnHomeBuildResp" runat="server" />
                                            </div>
                                            <asp:HiddenField ID="hdnHomeBuildRoles" runat="server" />
                                            <div class="form-group">
                                                <asp:Label ID="lblgeneralsel2" runat="server" meta:resourcekey="lblgeneralsel2" for="usr">Maximum number of widgets in page builder</asp:Label>
                                                <input type="text" maxlength="3" class="form-control customconfiginput" id="txtMaxNumOfWidgets" runat="server" onblur="return ValidateNumeric(this);" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!--menu4 begins here -->

                <!--menu5 begins here -->
                <div id="menu5" runat="server" class="tab-pane fade show active configheight" style="display: none;">
                    <div>
                        <div class="panel panel-default configpanel">
                            <div class="panel-heading  active configheading" role="tab" id="headingseven">
                                <h4 class="panel-title">
                                    <a class="collapsed" id="menu6Header" role="button" data-toggle="collapse" href="#collapseseven" aria-expanded="true" aria-controls="collapseseven" title="Hide"></a>
                                    <asp:Label ID="lblForm" runat="server" meta:resourcekey="lblForm">Form</asp:Label>

                                </h4>
                            </div>

                            <div id="collapseseven" class="" role="tabpanel" aria-labelledby="headingseven">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <span class="setTitle">
                                            <asp:Label ID="lblsettitle" runat="server" meta:resourcekey="lblsettitle">Show alert before row navigation in form Grid</asp:Label></span>
                                        <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                            <input class="appSettingsChkBx tgl tgl-ios" id="sabrnig" type="checkbox" />
                                            <label class="tgl-btn togglecustom toggle_btn" for="sabrnig"></label>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="menu7" runat="server" class="tab-pane fade configheight d-none">
                    <div class="card-header align-items-center panel-heading active configheading" role="tab">
                        <div class="card-title">
                            <h4 class="panel-title">
                                <asp:Label ID="Label2" runat="server" meta:resourcekey="lblgeneral">General</asp:Label>
                            </h4>
                        </div>
                    </div>
                    <div id="menuGenralItems" class="" role="tabpanel" aria-labelledby="headingseven">
                        <div class="panel-body">
                            <div id="">
                                <div class="col-sm-6 col-md-6">
                                    <div class="form-group">
                                        <span>
                                            <asp:Label runat="server" meta:resourcekey="lblaxMenuStyleSel">Menu Style</asp:Label>
                                        </span>
                                        <label></label>
                                        <div class="col multiSelWrap">
                                            <select id="axMenuStyleSel" class="form-control" runat="Server">
                                                <option value="default">Default</option>
                                                <option value="classic">Classic</option>
                                                <option value="custom">Custom</option>
                                            </select>
                                        </div>
                                    </div>

                                </div>

                                <div id="axCustomMenuInfo">
                                    <div class="col-sm-6 col-md-6">
                                        <div class="form-group">
                                            <span>
                                                <asp:Label runat="server" meta:resourcekey="lblaxMenuColCount">Columns</asp:Label>
                                            </span>
                                            <label></label>
                                            <div class="col multiSelWrap">
                                                <select id="axMenuColCount" class="form-control" runat="Server">
                                                    <option value="1">1</option>
                                                    <option value="2">2</option>
                                                    <option value="3">3</option>
                                                    <option value="4">4</option>
                                                </select>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-6">
                                        <div class="form-group">
                                            <span>
                                                <asp:Label runat="server" ID="lblaxMenusubCntPerView" meta:resourcekey="lblaxMenusubCntPerView">Submenu per view</asp:Label>
                                            </span>
                                            <label></label>
                                            <div class="col multiSelWrap">
                                                <input type="number" max="99" min="1" id="axMenusubCntPerView" class="form-control" runat="Server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-6">
                                        <div class="panel-body">
                                            <div class="form-group">
                                                <span class="setTitle">
                                                    <asp:Label ID="lblaxMenuWrdWrap" runat="server" meta:resourcekey="lblaxMenuWrdWrap">Word Wrap</asp:Label></span>
                                                <a href="javascript:void(0)" class="swtchDummyAnchr form-check form-switch  form-check-solid m-2">
                                                    <input class="tgl tgl-ios" id="axMenuWrapText" type="checkbox" />
                                                    <label class="tgl-btn togglecustom toggle_btn" for="axMenuWrapText" id="lblaxMenuWrapText"></label>
                                                </a>
                                                <asp:HiddenField ID="hdnMenuWrapText" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <span>
                                                <asp:Label runat="server" ID="lblSubMenuCount" meta:resourcekey="lblSubMenuCount"> SubMenu count </asp:Label></span>

                                            <div class="radio">
                                                <label>
                                                    <input type="radio" name="axMenuSubmenuCnt" value="all"><asp:Label runat="server" ID="lblall" meta:resourcekey="lblall">All</asp:Label></label>
                                            </div>
                                            <div class="radio">
                                                <label>
                                                    <input type="radio" name="axMenuSubmenuCnt" value="lim"><asp:Label runat="server" ID="Label4" meta:resourcekey="lblLimited">Limited</asp:Label></label>
                                                <input class="form-control menuInlineInpFld" type="number" min="1" max="99" id="txtSubMenuCnt" />
                                            </div>
                                            <asp:HiddenField ID="hdnSubMenuCount" runat="server" />
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <span>
                                                <asp:Label ID="lblDirSubMenuCount" runat="server" meta:resourcekey="lblDirSubMenuCount">Direct subMenu count</asp:Label>
                                            </span>

                                            <div class="radio">
                                                <label>
                                                    <input type="radio" name="axMenuDirSubmenuCnt" value="all"><asp:Label runat="server" ID="Label3" meta:resourcekey="lblall">All</asp:Label></label>
                                            </div>
                                            <div class="radio">
                                                <label>
                                                    <input type="radio" name="axMenuDirSubmenuCnt" value="lim"><asp:Label runat="server" ID="lblLimited" meta:resourcekey="lblLimited">Limited</asp:Label></label>
                                                <input class="form-control menuInlineInpFld" type="number" min="1" max="99" id="txtDirSubMenuCnt" />
                                            </div>
                                            <asp:HiddenField ID="hdnDirSubMenuCount" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id='waitDiv' style='display: none;'>
                    <div id='backgroundDiv' style='background: url(../Axpimages/loadingBars.gif) center center no-repeat rgba(255, 255, 255, 0.843137); background-size: 135px;'>
                    </div>
                </div>
            </div>
            <div class="Config-cont m-2">
                <i class="fa fa-question-circle" aria-hidden="true"></i><span class="form-label fs-6 fw-bolder text-dark">Changes to global settings do not take effect until you log out and log back into the application.</span>
                <div class="pull-right" style="padding-top: 10px;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" title="Save" OnClientClick="if (!CheckRequiredFields()) { return false;}; GetConfigVals();saveAppSettings();GetTextsdata('','');" OnClick="btnSave_Click" class=" btn-primary btn btn-active-primary shadow-sm btn handCursor allow-enter-key  me-2" />
                    <input id="btncancel" type="button" runat="server" value="Cancel" title="Cancel" style="display: none;" class="btn btn-white btn-active-primary shadow-sm handCursor" />
                    <input id="btnRestore" type="button" runat="server" value="Restore Defaults" title="Restore Defaults" onclick="loadDefaultValues(); EnableEdit();" class="btn btn-white btn-active-primary shadow-sm  handCursor allow-enter-key" />
                </div>
                <input type="hidden" name="hdnRoleLandingPage" runat="server" id="hdnRoleName" />
                <input id="hdnLandingPages" type="hidden" name="hdnLandingPages" runat="server" />
            </div>
        </div>
        <asp:HiddenField ID="hdnUserSettings" runat="server" />
    </form>
    <script src="../Js/jquery.multi-select.min.js" type="text/javascript"></script>

    <script>
        $("#designerResp").multiSelect();
        $("#homeBuildResp").multiSelect();
    </script>
</body>
</html>
