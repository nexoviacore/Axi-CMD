<%@ Page Language="VB" AutoEventWireup="false" CodeFile="useracciview.aspx.vb" Inherits="useracciview" ValidateRequest="false" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert file user access" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <title>User Access Iview</title>
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

</head>
<body class="Family btextDir -<%=direction%>" dir="<%=direction%>">
    <form id="form1" runat="server" dir="<%=direction%>">

        <asp:PlaceHolder runat="server">
            <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
        </asp:PlaceHolder>
        <div class="form-horizontal">
            <div runat="server" class="success d-none" id="dvMessage">
            </div>
            <div class="view-content">
                <asp:ScriptManager runat="server" />
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="content" id="ViewControl">
                            <ul class="nav nav-tabs nav-pills border-0 me-5 mb-3 mb-md-0 fs-6" role="tablist">
                                <li class="nav-item w-50 me-0 mb-md-2" role="presentation">
                                    <a class="nav-link w-100 btn btn-flex btn-active-light active" href="#Buttons" data-bs-toggle="tab" id="aButtons">Buttons</a>
                                </li>
                            </ul>
                            <div class="tab-content fs-5">
                                <div class="tab-pane fade show active" id="Buttons" role="tabpanel">
                                    <div id="but_div" runat="server" class="userTbl">
                                    </div>
                                    <asp:HiddenField ID="but_xml" runat="server" />
                                </div>
                            </div>
                        </div>
                        <asp:Menu ID="Menu1" Width="150px" runat="server" Orientation="Horizontal" RenderingMode="List" IncludeStyleBlock="false" StaticMenuStyle-CssClass="nav nav-tabs" DynamicMenuStyle-CssClass="dropdown-menu" StaticSelectedStyle-CssClass="active" Visible="false">
                            <Items>
                                <asp:MenuItem Selected="true" Text="View Control" Value="0"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                        <br />
                        <!-- Tab for View Control -->
                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0" Visible="false">
                            <asp:View ID="vc" runat="server">
                                <asp:Menu ID="vc_menu" Width="100px" runat="server" Orientation="Horizontal" RenderingMode="List" IncludeStyleBlock="false" StaticMenuStyle-CssClass="nav nav-tabs" DynamicMenuStyle-CssClass="dropdown-menu" StaticSelectedStyle-CssClass="active">
                                    <Items>
                                        <asp:MenuItem Text="Buttons" Selected="true" Value="2"></asp:MenuItem>
                                    </Items>
                                </asp:Menu>
                            </asp:View>
                        </asp:MultiView>
                        <!-- View Control Inner Items -->
                        <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
                            <!-- Buttons -->
                            <asp:View ID="but_view" runat="server">
                                <div class="h-auto d-none">
                                    <table class="table table-sm table-row-bordered w-100">
                                        <tr class="align-top">
                                            <td class="border-top-0 text-left"></td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                        </asp:MultiView>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="dialog-footer float-end">
            <div class="<%=direction %>">
                <asp:Button runat="server" ID="save" Text="Save" CssClass="btn btn-primary btn-sm" OnClientClick="ShowDimmer(true);" />
                <input type="button" id="btnClose" name="Close" value="Close" class="btn btn-light btn-sm shadow-sm" onclick="closeWindow()" />
            </div>
        </div>
        <div id="ls" runat="server">
        </div>

        <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
            <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                <span class="loader"></span>
            </div>
        </div>
        
        <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
        <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
        <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
        <script src="../Js/helper.min.js?v=150" type="text/javascript"></script>
        <script src="../Js/useracciview.min.js?v=14" type="text/javascript"></script>
        <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
        <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>

        <script>
            var isCloudApp = '<%=isCloudApp%>'
        </script>
    </form>
</body>
</html>
