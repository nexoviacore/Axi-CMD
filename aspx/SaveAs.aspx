<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SaveAs.aspx.vb" Inherits="SaveAs" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Save As"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <title>Save As</title>
    <!-- ________ CSS __________ -->
    <%If EnableOldTheme = "true" Then%>
    <link href="../Css/genericOld.min.css" rel="stylesheet" type="text/css" id="generic" />
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
    <%Else%>
    <link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" id="Link1" />
    <%End If%>
    <!-- ________ JQUERY __________ -->
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>

    <%--custom alerts start--%>
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>
    <!-- ________ JAVASCRIPT __________ -->

    <script type="text/javascript" src="../Js/iview.min.js?v=403"></script>

    <script src="../Js/helper.min.js?v=172" type="text/javascript"></script>
    <script src="../Js/SaveAs.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <link href="../Css/SaveAs.min.css" rel="stylesheet" />
</head>

<body class="divcontainer">
    <form id="form1" runat="server" dir="<%=direction%>">
        <%

            Dim tid As String
            tid = Request.QueryString("tid").ToString()
            param1 = Request.QueryString("param").ToString()
            'recid = Request.QueryString("recid").ToString()  var rid='" & recid & "';
            param.Value = param1.ToString()
            Response.Write("<script type=""text/javascript"" language=""javascript"">")
            Response.Write("var proj = '" & Session("project") & "';var user='" & user & "';var sid='" & sid & "';var ivna='';var trace='" & trace & "';var axTheme='" + Session("Thmcolor") + "'")
            Response.Write("</script>")

        %>
        <div style="text-align: center;">
            <div style="height: 50px; margin-top: 10px;">

                <asp:DropDownList ID="ddloptions" runat="server" CssClass="combotem Family">
                    <asp:ListItem>Save As</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>HTML</asp:ListItem>
                    <%-- <asp:ListItem>CSV</asp:ListItem>--%>
                    <%-- <asp:ListItem>XML</asp:ListItem>--%>
                </asp:DropDownList>

                <br />
            </div>
            <div style='text-align: center;'>
                <asp:Button ID="btnSaveAs" runat="server" Height="23px" Text="Ok" />
                &nbsp;<input id="Button1" type="button" value="Close" onclick="window.close();" />
            </div>
            <div>
                <asp:Label ForeColor="Red" ID="lblErr" runat="server"></asp:Label>
            </div>
        </div>
        <input id="ivtype" type="hidden" name="ivtype" />
        <input id="param" type="hidden" name="param" runat="server" />

    </form>
</body>

</html>
