<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CloudErrorPage.aspx.cs" Inherits="aspx_CloudErrorPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Cloud Error Page" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Custom Error</title>
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
    <link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <!-- Forcefully closing the Dimmer -->
    <script type="text/javascript">
        //window.parent.closeFrame();
    </script>
</head>
<body onload="window.parent.closeFrame();" dir='<%=direction%>'>
    <form id="form1" runat="server">
        <div>
            <h3 class="error err">
                <asp:Label ID="lblprob" runat="server" meta:resourcekey="lblprob">Oops! there seems to be some problem. Please try later.</asp:Label></h3>
            <div>
                <asp:Label ID="lblerrorno" runat="server" meta:resourcekey="lblerrorno">ErrorNo:</asp:Label><div runat="server" id="errno">
                    <asp:Label ID="lblSessionexp" runat="server" meta:resourcekey="lblSessionexp" Visible="false"> Disconnected because you have logged into another session.</asp:Label>
                    <asp:Label ID="lblError" runat="server" meta:resourcekey="lblError" Visible="false"> Error</asp:Label>
                </div>
            </div>
            <h5 runat="server" id="errdtls">
                <asp:Label ID="lblcontact" runat="server" meta:resourcekey="lblcontact">Please try again or contact Agile Cloud Team.</asp:Label></h5>
        </div>
    </form>
</body>
</html>
