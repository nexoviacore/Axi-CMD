<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppSettings.aspx.cs" Inherits="AppSettings" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="App Settings" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>App Settings</title>
    <link href="../Css/thirdparty/bootstrap/3.3.6/bootstrap.min.css" rel="stylesheet" />
    <link href="../Css/thirdparty/font-awesome/4.6.3/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" />
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
    <link href="../Css/AppSettings.min.css?v=1" rel="stylesheet" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <link href="../App_Themes/Gray/Stylesheet.min.css?v=23" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script type="text/javascript" src="../Js/thirdparty/jquery/3.1.1/jquery.min.js"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/thirdparty/bootstrap/3.3.6/bootstrap.min.js" type="text/javascript"></script>
    <script src="../Js/AppSettings.min.js?v=12" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <link id="themecss" type="text/css" href="" rel="stylesheet" />
</head>
<body dir='<%=direction%>'>
    <div id="breadcrumb-panel">
        <div id="breadcrumb" class="icon-services">
            <div class="icon-services bcrumb h3 left" style="margin-top: 0px; padding: 10px; padding-left: 40px; font-size: 18px; cursor: default !important">Application Settings</div>
        </div>
    </div>
    <div class="container">

        <div id="main">
            <div class="container">
                <div class="col-xs-10 col-xs-offset-1 col-md-10 col-sm-10  col-sm-offset-1 col-md-offset-1 appsetting">
                    <div class="workfile">
                        <asp:Label ID="lblslct" runat="server" meta:resourcekey="lblslct" for="sel1">Select Setting</asp:Label>
                        <div class="form-group">
                            <select id="appSettingSlct" class="form-control">
                                <option value="">-Select-</option>
                                <option value="tstruct">Form
                                </option>
                                <%--<option value="Iview">Iview</option>
                                <option value="Common">Common</option>--%>
                            </select>
                        </div>
                        <div class="tstruct box">
                            <div class="row">
                                <div class="tg-list">
                                    <div class="tg-list-item">
                                        <span class="setTitle">
                                            <asp:Label ID="lblalert" runat="server" meta:resourcekey="lblalert">Show alert before row navigation in form Grid.</asp:Label></span>
                                        <input class="appSettingsChkBx tgl tgl-ios" id="sabrnig" type="checkbox" />
                                        <label class="tgl-btn" for="sabrnig"></label>
                                    </div>
                                    <%-- <div class="tg-list-item">
                                        <span class="setTitle">Some testing variable.</span>
                                        <input class="appSettingsChkBx tgl tgl-ios" id="stv" type="checkbox" />
                                        <label class="tgl-btn" for="stv"></label>
                                    </div>--%>
                                    <button type="button" class="btn hotbtn" onclick="saveAppSettings();">Save</button>
                                    <button type="button" class="btn coldbtn" onclick="cancelAppSettings();">Cancel</button>
                                </div>
                            </div>
                        </div>
                        <div class="Iview box">
                            <div class="row">
                                <div class="pull-left">
                                    <asp:Label ID="lblon" runat="server" meta:resourcekey="lblon">
                                    Turn on the feature switch</asp:Label>
                                    <input type="checkbox" checked data-toggle="toggle" data-size="mini">
                                </div>
                                <div class="pull-right">
                                    <asp:Label ID="lbloff" runat="server" meta:resourcekey="lbloff">

                                    Turn of the feature switch</asp:Label>
                                    <input type="checkbox" checked data-toggle="toggle" data-size="mini">
                                </div>
                            </div>
                            <div class="row">
                                <div class="pull-right">
                                    <button class="hotbtntiny btn">Save</button>
                                    <button class="coldbtntiny btn">Cancel</button>
                                </div>
                            </div>
                        </div>
                        <div class="Common box">
                            <div class="row">
                                <div class="pull-left">
                                    <asp:Label ID="lblon1" runat="server" meta:resourcekey="lblon1">
                                    Turn on the feature switch</asp:Label>
                                    <input type="checkbox" checked data-toggle="toggle" data-size="mini">
                                </div>
                                <div class="pull-right">
                                    <asp:Label ID="lbloff1" runat="server" meta:resourcekey="lbloff1">
                                    Turn off the feature switch</asp:Label>
                                    <input type="checkbox" checked data-toggle="toggle" data-size="mini">
                                </div>
                            </div>
                            <div class="row">
                                <div class="pull-right">
                                    <button class="hotbtntiny btn">Save</button>
                                    <button class="coldbtntiny btn">Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <%--<div class="tg-list">
            <div class="tg-list-item">
                <span class="setTitle">Show alert before row navigation in form Grid.</span>
                <input class="appSettingsChkBx tgl tgl-ios" id="sabrnig" type="checkbox" />
                <label class="tgl-btn" for="sabrnig"></label>
            </div>
            <div class="tg-list-item">
                <span class="setTitle">Some testing variable.</span>
                <input class="appSettingsChkBx tgl tgl-ios" id="stv" type="checkbox" />
                <label class="tgl-btn" for="stv"></label>
            </div>
            <button type="button" class="btn hotbtn" onclick="saveAppSettings();">Save</button>
            <button type="button" class="btn coldbtn" onclick="cancelAppSettings();">Cancel</button>
        </div>--%>
    </div>

</body>
</html>
