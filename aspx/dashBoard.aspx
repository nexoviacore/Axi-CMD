<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashBoard.aspx.cs" Inherits="aspx_dashBoard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Dashboard</title>
    <meta charset="utf-8"/>
    <meta name="description" content="Axpert file uploader"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>    
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />

    <link href="../ThirdParty/materialize/css/materialize.min.css?v=3" rel="stylesheet" />
    <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" />
    <%--<script>
        if(typeof localStorage != "undefined"){
            var projUrl =  top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            var lsTimeStamp = localStorage["customGlobalStylesExist-" + projUrl]
            if(lsTimeStamp && lsTimeStamp != "false"){
                var appProjName = localStorage["projInfo-" + projUrl] || "";
                var customGS = "<link id=\"customGlobalStyles\" data-proj=\""+ appProjName +"\" href=\"../"+ appProjName +"/customGlobalStyles.css?v="+ lsTimeStamp +"\" rel=\"stylesheet\" />";
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
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <link href="../Css/cloudGrid.min.css" rel="stylesheet" />
    <link href="../Css/Icons/icon.css" rel="stylesheet" />
    <link href="../AssetsNew/css/style.min.css?v=3" rel="stylesheet" />
    <link href="../AssetsNew/css/dashboard.min.css?v=10" rel="stylesheet" />
    <link href="../Css/newMenu.min.css?v=22" rel="stylesheet" />
    <link href="../ThirdParty/Linearicons/Font/library/linearIcons.css" rel="stylesheet" />
    <link href="../ThirdParty/scrollbar-plugin-master/jquery.mCustomScrollbar.css" rel="stylesheet" />
    <link id="themecss" type="text/css" href="" rel="stylesheet" />
    <style type="text/css">
        .highcharts-contextmenu > .highcharts-menu > hr {
            margin-top: 0px;
            margin-bottom: 0px;
            border: 0;
            border-top: 1px solid #eee;
        }

        .maxHeight {
            height: 300px;
        }
    </style>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script>
        var globalVars = '<%=globalVars%>';
        var appsessionKey = '<%=appsessionKey%>';
        var userResps = parent.userResp;
    </script>

</head>
<body onload="ChangeDir('<%=direction%>');" class="btextDir-<%=direction%>" dir="<%=direction%>">
    <div id="linksdialog" title="Add Quick Link">
    </div>
    <div id="linkEditDialog" title="Edit Quick Link">
    </div>
    <form id="form1" runat="server">
        <div class="content" style="display: none">
            <% if (langs == "ARABIC")
                {%>
            <div id="backforwrdbuttons" class="backbutton hide" style="float: right;">
                <%}%>
                <%else
                    {%>
                <div id="backforwrdbuttons" class="backbutton hide" style="float: left; padding: 5px;">
                    <%}%>
                </div>
                <div id="breadcrumb-panel">
                    <div id='breadcrumb' class="icon-services">
                        <% if (langs == "ARABIC")
                            {%>
                        <div class="icon-services bcrumb h3"></div>
                        <%}%>
                        <%else
                            {%>
                        <div class="icon-services bcrumb h3 left" style="margin-top: 0px; color: black; padding: 10px; font-size: 18px; padding-left: 40px; cursor: default !important"></div>
                        <%}%>
                    </div>
                </div>
            </div>
        </div>

        <div class="DivMessage">
            <div style="padding: 0px;" class="container-fluid">
                <div class="row" style="margin: 0px;">
                    <div class="col s12" style="padding: 0px;">
                        <div class="hpbWrapper">
                            <div class="hpbHeaderTitle dashBoardTitle">
                                <i class="icon-chart-growth" style="cursor: default !important;"></i>
                                <span class="title">
                                    <asp:Label ID="lbldashbrd" runat="server" meta:resourcekey="lbldashbrd">DashBoard</asp:Label></span>
                            </div>
                            <div id="HPBmainWrapper">
                                <div class="col s2" id="HPBToolBox">
                                    <div class="hpbWrapper">
                                        <div class="hpbHeaderTitle dashBoardTitle">
                                            <i class="icon-funnel" style="cursor: default !important;"></i>
                                            <span class="title">
                                                <asp:Label ID="lblfltr" runat="server" meta:resourcekey="lblfltr">Filter</asp:Label></span>
                                            <button type="button" title="Close ToolBox" id="collapseDBTB" class="btn-flat collapseOpenBtns waves-effect btn-floating right dashboard-main"><span class="icon-chevron-left-circle"></span></button>
                                        </div>
                                        <div class="toolBxPanelWrapper">
                                            <div class="toolBxPanel" id="toolBxPanel">
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col s10" id="HPBdesignerCanvas">
                                    <div class="row widgetrow">
                                        <div class="col s12" style="padding-right: 0px; padding-left: 3px;">
                                            <div id="openDBTBWrapper" style="display: none;">
                                                <div class="hpbHeaderTitle dashBoardTitle" style="padding: 3px 5px;">
                                                    <%--<i class="icon-compass"></i>--%>
                                                    <button type="button" title="Open ToolBox" id="openDBTB" class="btn-flat collapseOpenBtns waves-effect btn-floating"><span class="icon-chevron-right-circle"></span></button>
                                                </div>
                                            </div>
                                            <ul class="tabs listUlDb">
                                            </ul>
                                        </div>
                                        <div class="rightPartODshBrd">
                                            <div class="rpdbContainer">
                                                <div id="divDetailsLinks">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/thirdparty/jquery-ui/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../ThirdParty/materialize/js/materialize.min.js?v=10"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script src="../ThirdParty/Highcharts/highcharts.js"></script>
    <script src="../ThirdParty/Highcharts/highcharts-3d.js"></script>
    <script src="../ThirdParty/Highcharts/highcharts-more.js"></script>
    <script src="../ThirdParty/Highcharts/highcharts-exporting.js"></script>

    <script src="../Js/high-charts-functions.min.js?v=23"></script>
    <script src="../Js/dashboardchart.min.js?v=20" type="text/javascript"></script>
    <script src="../Js/dashBoard.min.js?v=29" type="text/javascript"></script>
    <script src="../assets/js/loadingoverlay.min.js?v=3"></script>
    <script src="../ThirdParty/scrollbar-plugin-master/jquery.mCustomScrollbar.js"></script>
</body>
</html>
