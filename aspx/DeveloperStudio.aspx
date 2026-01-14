<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeveloperStudio.aspx.cs" Inherits="aspx_DeveloperStudio" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Axpert Developer Studio</title>
    <script>
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
    </script>
    <script type="text/javascript">
        var _pageName = '<%=pname%>';
    </script>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
</head>
<body class="header-fixed header-tablet-and-mobile-fixed toolbar-enabled aside-fixed aside-default-enabled page-loading" id="dvdeveloperstudio" data-kt-aside-minimize="off" dir="<%=direction%>">
    <div class="adminMain d-flex flex-column flex-root">
        <div class="adminMainInner page d-flex flex-row flex-column-fluid position-relative">
            <div class="d-flex flex-column flex-row-fluid w-100 h-100">
                <div class="content fs-6 d-flex flex-column flex-column-fluid px-3 pt-5 pb-2 adminRightGap">
                    <div class="splitter-wrapper flex-column-fluid h-100">
                        <div class="panel-container main-panel-container flex-column-fluid h-100">
                            <div class="panel-left panel-fisrt-part flex-column-fluid h-100">
                                <div class="panel-left-inner flex-column-fluid h-100">
                                    <iframe id="axpiframeac" name="axpiframeac" class="col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened flex-column-fluid h-100" style="padding: 0px;" frameborder="0" allowtransparency="True" width="100%"></iframe>
                                </div>
                                <div class="splitter panel-splitter flex-column-fluid h-100" id="drag-point" style="display: none;">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
                <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                    <span class="loader"></span>
                </div>
            </div>
        </div>
    </div>

    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>

    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script src="../Js/helper.min.js?v=172" type="text/javascript"></script>
    <script src="../js/developerStudio.min.js?v=1" type="text/javascript"></script>
    <script type="text/javascript">
        $(callParentNew("appBackBtn", "class")).hide();
    </script>
    <form id="form1" runat="server">
    </form>
</body>
</html>

