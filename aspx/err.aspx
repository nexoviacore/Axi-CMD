<%@ Page Language="VB" AutoEventWireup="false" CodeFile="err.aspx.vb" Inherits="err" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Error Page" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />

    <title>Error</title>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script type="text/javascript">
        var enableBackButton = false;
        var enableForwardButton = false;
    </script>
    <!-- <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" /> -->
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(If(direction = "ltr", "~/UI/axpertUI/ltrBundleCss", "~/UI/axpertUI/rtlBundleCss")) %>
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
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <%--custom alerts start--%>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>

    <script src="../Js/err.min.js?=4" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script type="text/javascript">
        var serverprocesstime = '<%=serverprocesstime%>';
        var requestProcess_logtime = '<%=requestProcess_logtime%>';
        $(document).ready(function (event) {
            if (serverprocesstime != '') {
                WireElapsTime(serverprocesstime, requestProcess_logtime);

                GetProcessTime();
                GetTotalElapsTime();
            }
        });
    </script>
    <script>
        if(parent.ShowDimmer){
            parent.ShowDimmer(false)
        }
    </script>
</head>
<body>
    <form dir="<%=direction%>" name="f1" id="form1" runat="server">
        <div class="d-flex justify-content-center align-items-center p-10 vh-100">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Path="../Js/helper.min.js?v=172" />
                </Scripts>
                <Services>
                    <asp:ServiceReference Path="../WebService.asmx" />
                    <asp:ServiceReference Path="../CustomWebService.asmx" />
                </Services>
            </asp:ScriptManager>

            <%If errMsg.Contains("Disconnected because you have logged into another session.") Then
                    Response.Write("<script>")
                    Response.Write("parent.parent.location.href='../aspx/sess.aspx';")
                    Response.Write("</script>")
                    Return
                ElseIf errMsg.Contains("Session Authentication failed...") Then
                    Response.Write("<script>")
                    Response.Write("parent.parent.location.href='../aspx/sess.aspx';")
                    Response.Write("</script>")
                    Return
                End If
            %>

            <h1 class="h1 me-3 my-0 align-top inline-block align-content-center">
                <% If errMsg <> "Releasing soon." Then%>
                <asp:Label ID="lblerror" runat="server" meta:resourcekey="lblerror">Oops!</asp:Label>
                <%End If %>
            </h1>

            <% If errMsg <> String.Empty And errMsg = "Releasing soon." Then%>
            <div class="inline-block align-middle ps-3 py-2 border-danger-- border-start--">
                <script type="text/javascript"> callParentNew("closeFrame()", "function"); </script>
                <h2 class="h1 me-3 my-0 align-top inline-block align-content-center lead--" id="desc"><%=errMsg%></h2>
            </div>
            <%else %>
            <div class="inline-block align-middle ps-3 py-2 border-danger border-start">
                <% If errMsg <> String.Empty Then%>
                <script type="text/javascript"> callParentNew("closeFrame()", "function"); </script>
                <asp:Label ID="lblerror1" runat="server" meta:resourcekey="lblerror1" CssClass="d-flex h4">Error occured due to the following reason:&nbsp;</asp:Label>
                <h2 class="my-0 font-weight-normal lead" id="desc"><%=errMsg%></h2>
                <%Else%>
                <h2 class="my-0 font-weight-normal lead" id="desc"><%=customError%></h2>
                <%End If%>
            </div>
            <%End If%>

            <%=enableBackForwButton%>
            <div id="dvGoBack" class="d-none">

                <span class="navLeft icon-arrows-left-double-32" onclick="javascript:BackForwardButtonClicked(&quot;back&quot;);"
                    id="goback" style="text-decoration: none; cursor: pointer; border: 0px;"
                    title="Click here to go back" />

            </div>
            <asp:Label ID="lblCustomerror" runat="server" meta:resourcekey="lblCustomerror" Visible="false">Server error. Please try again.If the problem continues, please contact your administrator.</asp:Label>
            <asp:Label ID="lblInvParams" runat="server" meta:resourcekey="lblInvParams" Visible="false">Invalid parameter</asp:Label>
            <asp:Label ID="lbleroccurred" runat="server" meta:resourcekey="lbleroccurred" Visible="false">Error occurred(2). Please try again or contact administrator.</asp:Label>
            <asp:Label ID="lblUnknownError" runat="server" meta:resourcekey="lblUnknownError" Visible="false">Unknown error. Please try again. If the problem continues, please contact your administrator.</asp:Label>
        </div>
    </form>
</body>
</html>
