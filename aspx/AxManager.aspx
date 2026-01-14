<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AxManager.aspx.cs" Inherits="aspx_AxManager" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert Manager Page" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>AxManager</title>
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
    <link href="../Css/homepage.min.css" rel="stylesheet" />
    <link href="../Css/thirdparty/jquery-ui/1.12.1/jquery-ui.min.css" rel="stylesheet" />
    <link href="../Css/thirdparty/jquery-ui/1.12.1/jquery-ui.structure.min.css" rel="stylesheet" />
    <link href="../Css/thirdparty/jquery-ui/1.12.1/jquery-ui.theme.min.css" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/thirdparty/jquery-ui/jquery-ui.min.js" type="text/javascript"></script>

    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <%--custom alerts start--%>
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/AxManager.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>

</head>
<body dir='<%=direction%>'>
    <div class="container">
        <div id="content">
            <form name="form1" runat="server" id="form2">
                <div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </div>
                <h1>
                    <asp:Label ID="lblaxmanager" runat="server" meta:resourcekey="lblaxmanager">Axpert Connection Manager</asp:Label></h1>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <%-- <div>
				<label>Available Projects</label><asp:TextBox runat="server" placeholder="Select Available Project" ID="availableProjects"></asp:TextBox>
			</div>--%>
                            <div>
                                <asp:Label ID="lblproject" runat="server" meta:resourcekey="lblproject">Project Name</asp:Label><asp:TextBox runat="server" autocomplete="off" placeholder="Project Name" ID="projName"></asp:TextBox>
                            </div>
                            <div>
                                <asp:Label ID="lblDB" runat="server" meta:resourcekey="lblDB" Style="margin-left: 20px;">Database</asp:Label>
                                <asp:DropDownList ID="dBase" runat="server" Width="92%">
                                    <asp:ListItem Text="Oracle" Value="Oracle"></asp:ListItem>
                                    <asp:ListItem Text="MS SQL" Value="MS SQL"></asp:ListItem>
                                    <asp:ListItem Text="MySQL" Value="MySQL"></asp:ListItem>
                                    <%--<asp:ListItem Text="My SQL" Value="2"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:Label ID="lblconname" runat="server" meta:resourcekey="lblconname">Client Connection Name</asp:Label>
                                <asp:TextBox runat="server" autocomplete="off" placeholder="Client Connection Name" ID="clientConName"></asp:TextBox>
                            </div>
                            <div>
                                <asp:Label ID="lblusrname" runat="server" meta:resourcekey="lblusrname">DB User name</asp:Label>
                                <asp:TextBox runat="server" autocomplete="off" placeholder="DB User Name" ID="usrName"></asp:TextBox>
                            </div>
                        </div>
                        <div id="Div1" runat="server">
                            <asp:Button ID="save" Text="Save" OnClientClick="javascript:return ValidateAxManager();" CssClass="hotbtn btn" runat="server" OnClick="btnSave_Click" />
                            <asp:Button ID="submit" Text="Connect" OnClientClick="javascript:return ValidateAxManager();" CssClass="hotbtn btn" runat="server" OnClick="btnConnect_Click" />
                            <asp:Button ID="Clear" Text="Clear" runat="server" CssClass="coldbtn btn" OnClientClick="javascript:return ClearAxManagerData();" />
                            <asp:Button ID="delete" Text="Delete" runat="server" CssClass="coldbtn btn" OnClick="btnDelete_Click" OnClientClick="javascript:return ValidateAxManager(true);" />
                            <div style="display: none;">
                                <asp:Button ID="cnfSave" runat="server" OnClick="CnfButton_Click" />
                                <asp:Button ID="autofillProjDetails" runat="server" OnClick="autofillProjDetails_Click" />
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="sessionstatus" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="lblweb" runat="server" meta:resourcekey="lblweb"># Project Name can also be changed from web.config file.</asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:HiddenField ID="axAvailableProj" runat="server" />
            </form>
        </div>
    </div>
</body>
</html>
