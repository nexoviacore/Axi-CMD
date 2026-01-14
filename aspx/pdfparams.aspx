<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pdfparams.aspx.vb" Inherits="pdfparams"
    Debug="true" %>

<%@ Import Namespace="System.Xml" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="PDF Params"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <title>PDF Parameters</title>
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
    <%If EnableOldTheme = "true" Then%>
    <link href="../Css/genericOld.min.css" rel="stylesheet" type="text/css" id="generic" />
    <%Else%>
    <link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" id="Link1" />
    <%End If%>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <%--custom alerts start--%>
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script type="text/javascript" src="../Js/pdfparam.min.js?v=5"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <style type="text/css">
        .style1 {
            width: 153px;
        }
    </style>
</head>
<body class="Pagebody Family">
    <form id="form1" runat="server" dir="<%=direction%>">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Path="../Js/pdfparam.min.js?v=5" />
                </Scripts>
                <Services>
                    <asp:ServiceReference Path="../WebService.asmx" />
                </Services>
            </asp:ScriptManager>
        </div>
        <asp:HiddenField ID="hdnScriptspath" runat="server" />
    </form>
    <form name="f1" dir="<%=direction%>">
        <table style="width: 340px; height: 130px; border: 0px; padding: 1px; border-spacing: 1px;">
            <tr>
                <td style="text-align: center;" class="style1">
                    <div style="width: 127px;">
                        <center>
                            <span class="labelcap Family">Select PDF</span></center>
                    </div>
                </td>
                <td>
                    <div>
                        <select id="pdfFileName" name="fname" class="combotem Family">
                            <%=optstr %>
                        </select>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <center>
                        <input type="button" id="pdf" value="Open" onclick="javascript: CallPDFws();" />
                        <input id="Button1" type="button" value="Cancel" onclick="window.close();" /></center>
                </td>
            </tr>
        </table>
        <input type="hidden" value="<%=ptransid %>" name="transid" class="tem Family" disabled />
    </form>
</body>
</html>
