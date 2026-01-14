<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ivpicklist.aspx.vb" Inherits="ivpicklist" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Pick List" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>IView Picklist</title>

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
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script type="text/javascript">
        var srchFld = "";
    </script>

</head>

<body dir="<%=direction%>">
    <form id="form1" runat="server" dir="<%=direction%>">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Path="../Js/iview.min.js?v=403" />
                </Scripts>
                <Services>
                    <asp:ServiceReference Path="../WebService.asmx" />
                </Services>
            </asp:ScriptManager>
        </div>
        <div id="dvPickList" runat="server">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="d-none">
                        <asp:ListBox ID="searchlist" Visible="true" runat="server" Height="0px" Width="0px"
                            AutoPostBack="True"></asp:ListBox>
                        <asp:ListBox ID="searchlistval" Visible="true" runat="server" Height="0px" Width="0px"
                            AutoPostBack="True"></asp:ListBox>
                        <asp:ListBox ID="lstValues" Visible="true" runat="server" Height="0px" Width="0px"
                            AutoPostBack="True"></asp:ListBox>
                    </div>
                    <div class="d-flex flex-column">
                        <asp:Panel runat="server" ID="Panel1">
                            <asp:GridView CellSpacing="-1" ID="GridView1" runat="server" CellPadding="2" GridLines="Vertical" AllowSorting="false" RowStyle-Wrap="false" AutoGenerateColumns="false" PageSize="10" CssClass="table table-bordered table-responsive table-hover w-100 text-dark Grid">
                                <Columns>
                                </Columns>
                                <HeaderStyle CssClass="text-dark GridHead" />
                                <AlternatingRowStyle CssClass="GridAltPage" />
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="Panel2" CssClass="d-flex flex-row-auto w-auto justify-content-end">
                            <asp:Label ID="records" runat="server" Text="" CssClass="totrec"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="pgCap" Text="Page no : " runat="server" Visible="false" CssClass="totrec">
                            </asp:Label>
                            <asp:DropDownList ID="lvPage" runat="server" AutoPostBack="true" Visible="false">
                            </asp:DropDownList>
                        </asp:Panel>
                        <asp:Label ID="lblErrMsg" runat="server" CssClass="form-label fw-boldest text-danger p-5  seartem" Visible="false"></asp:Label>

                        <asp:TextBox ID="pgno" runat="server" Text="0" CssClass="form-control-sm"></asp:TextBox>

                    </div>
                    <div class="d-none">
                        <asp:Button ID="btnTemp" runat="server" />
                    </div>
                    <asp:HiddenField ID="hdnIViewData2" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:HiddenField ID="paramXml" runat="server" />
            <asp:HiddenField ID="srchFld" runat="server" />
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div id="progressArea">
                        <asp:Label ID="lblgetdata" meta:resourcekey="lblgetdata" runat="server"> Getting the data,
                            please wait...</asp:Label>
                        <asp:Image ID="LoadingImage" runat="server" ImageUrl="../AxpImages/icons/5-1.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <asp:Label ID="lblNodata" runat="server" meta:resourcekey="lblNodata" Visible="false">No data found.</asp:Label>
    </form>


    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script type="text/javascript" src="../Js/noConflict.min.js?v=1"></script>
    <script type="text/javascript" src="../Js/alerts.min.js?v=32"></script>
    <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>
    <script type="text/javascript" src="../Js/common.min.js?v=158"></script>
    <script type="text/javascript" src="../Js/ivpicklist.min.js?v=13"></script>
    <script type="text/javascript" src="../Js/JDate.min.js?v=3"></script>
</body>

</html>
