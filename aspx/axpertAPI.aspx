<%@ Page Language="C#" AutoEventWireup="true" CodeFile="axpertAPI.aspx.cs" Inherits="axpertAPI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="IView" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Axpert API</title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render("~/Css/axpertAPI") %>
    </asp:PlaceHolder>
    <% if (direction == "rtl")
        { %>
    <link rel="stylesheet" href="../ThirdParty/bootstrap_rtl.min.css" type="text/css" />
    <% } %>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
   <%-- <script>
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
    <script>
        var sessionID = '<%=sessionID%>';
    </script>
</head>
<body class="btextDir-<%=direction%>" dir="<%=direction%>">
    <form id="form1" runat="server">
        <asp:PlaceHolder runat="server">
            <%:Scripts.Render("~/Js/axpertAPI") %>
        </asp:PlaceHolder>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="main-api">
                    <div class="col-sm-4 col-lg-4 col-md-4">
                        <asp:DropDownList ID="ddlActions" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlActions_SelectedIndexChanged" AutoPostBack="True" onchange="selectedActionChanged()">
                            <asp:ListItem Selected="True">--Select Action--</asp:ListItem>
                            <asp:ListItem>Login</asp:ListItem>
                            <asp:ListItem>Submit data</asp:ListItem>
                            <asp:ListItem>Get data</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-4 col-lg-4 col-md-4">
                        <asp:DropDownList ID="ddlTstruct" CssClass="form-control" runat="server" AutoPostBack="True" Enabled="false" OnSelectedIndexChanged="ddlTstruct_SelectedIndexChanged" onchange="selectedTstructChanged()">
                            <asp:ListItem Selected="True">--Select Form--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-4 col-lg-4 col-md-4">
                        <asp:DropDownList ID="ddlIview" CssClass="form-control" runat="server" Enabled="false" onchange="selectedIviewChanged()">
                            <asp:ListItem Selected="True">--Select Iview--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-3 col-lg-3 col-md-3 hide">
                        <asp:DropDownList ID="ddlSQL" CssClass="form-control" runat="server" AutoPostBack="True" Enabled="false" OnSelectedIndexChanged="ddlSQL_SelectedIndexChanged" onchange="selectedSQLChanged()">
                            <asp:ListItem Selected="True">--Select SQL results--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-1 col-lg-1 col-md-1"><a id="btnAddSql" class="btn btn-primary hide" href="#" onclick="createPopup('tstruct.aspx?transid=t_csq&AxPop=true',true)">Add SQL</a> </div>

                    <div class="clearfix"></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="col-sm-12 col-lg-12 col-md-12">
            <div id="apiUrl" runat="server"></div>
        </div>
        <div class="col-sm-3 col-lg-3 col-md-3 hide">
            <asp:Label ID="lblParams" runat="server">Parameters</asp:Label>
            <div id="dvParams" runat="server">
                <asp:Table ID="loginParams" runat="server" CssClass="hide">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>Params</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Values</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell>axpapp</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>username</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>password</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>password</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table ID="sbmtdataParams" runat="server" CssClass="hide">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>Params</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Values</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell>axpapp</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>s(session ID)</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>transid</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>changedrows</asp:TableCell>
                        <asp:TableCell>JSON</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>recordid</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>recdata</asp:TableCell>
                        <asp:TableCell>JSON Array</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table ID="getIvewParams" runat="server" CssClass="hide">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>Params</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Values</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell>name</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>s(session ID)</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>axpapp</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>pageno</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>pagesize</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>sqlpagination</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>params</asp:TableCell>
                        <asp:TableCell>JSON</asp:TableCell>
                    </asp:TableRow>

                </asp:Table>
                <asp:Table ID="getChoiceParams" runat="server" CssClass="hide">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>Params</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Values</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell>axpapp</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>s(session ID)</asp:TableCell>
                        <asp:TableCell></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>sql</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>direct</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>params</asp:TableCell>
                        <asp:TableCell>string</asp:TableCell>
                    </asp:TableRow>

                </asp:Table>

            </div>
        </div>
        <div class="col-sm-12 col-lg-12 col-md-12">
            <asp:Label ID="lblRequest" runat="server">Sample Request JSON :</asp:Label>
            <div id="dvInputString" runat="server">
                <%--  <div class="dvName"><b>Request JSON :</b></div>
                <div class="dvContent"></div>--%>
            </div>
        </div>
        <div class="col-sm-12 col-lg-12 col-md-12">
            <asp:Label ID="lblResponse" runat="server">Sample Response JSON :</asp:Label>
            <div id="dvResponseString" runat="server">
                <%--<div class="dvName" ><b>On success :</b></div><div class="dvContent"></div>
                <div class="dvName" ><b>On failure :</b></div><div class="dvContent"></div>--%>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hdnParamIviewList" />
        <asp:HiddenField runat="server" ID="hdnScriptPath" />
    </form>


</body>
</html>
