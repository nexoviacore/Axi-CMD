<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AboutUs.aspx.cs" Inherits="aspx_AboutUs" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="About Us" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>About Axpert</title>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <link href="../Css/thirdparty/bootstrap/3.3.6/bootstrap.min.css" rel="stylesheet" />
    <script src="../Js/thirdparty/bootstrap/3.3.6/bootstrap.min.js"></script>
    <script src="../Js/thirdparty/jquery-ui/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <link href="../Css/thirdparty/jquery-ui/1.12.1/jquery-ui.min.css" rel="stylesheet" />
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
    <script src="../Js/common.min.js?v=158"></script>
    <link href="../Css/aboutus.min.css?v=3" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            modalHeader = eval(callParent("divModalHeader", "id") + ".getElementById('divModalHeader')");
            modalHeader.innerText = eval(callParent('lcm[387]'));
            $("#btnClose").prop("title", eval(callParent('lcm[249]')));
            $(this).parent("#btnClose").focus();
        }

        )
    </script>
    <style>
        .btextDir-rtl .modal-header button#btnModalClose {
            float: left !important;
        }
    </style>
</head>

<body class="btextDir-<%=direction%>" dir="<%=direction%>" id="dvaboutus">

    <div class="container versionInfo">
        <div class="about-us ">
            <div class=" versionclass">
                <asp:Label ID="lblVer" runat="server" meta:resourcekey="lblVer" class="control-label  castLbl " for="version">Version: </asp:Label>
                <asp:Label Text="" ID="Labelvers" runat="server" />
                <asp:Label class="fontclass" Text="" ID="lblVersion" runat="server" />
                <asp:Label class="fontclass" Text="" ID="lblSubversion" runat="server" />
            </div>
        </div>
        <div id="divproject" runat="server">
            <asp:Label ID="lblproj" runat="server" meta:resourcekey="lblproj" class="control-label castLbl" for="version">Project: </asp:Label>
            <asp:Label class="fontclass" Text="" ID="divprojcontent" runat="server" />
        </div>
        <div runat="server">
            <asp:Label ID="lblappsrvr" runat="server" meta:resourcekey="lblappsrvr" class="control-label castLbl" for="version">Application Server:</asp:Label>
            <asp:Label class="fontclass" Text="IIS" ID="Label13" runat="server" />
        </div>
        <div runat="server">
            <asp:Label ID="lbldb" runat="server" meta:resourcekey="lbldb" class="control-label castLbl" for="version">Database: </asp:Label>
            <asp:Label class="fontclass" Text="" ID="divdbconent" runat="server" />
        </div>
        <div runat="server">
            <asp:Label ID="lbldc" runat="server" meta:resourcekey="lbldc" class="control-label castLbl" for="version">Data Caching: </asp:Label>
            <asp:Label class="fontclass" Text="" ID="divdatacontent" runat="server" />
        </div>
        <div class="relclass" id="divRelDate" runat="server" visible="false">
            <asp:Label ID="lblrd" runat="server" meta:resourcekey="lblrd" class="control-label castLbl" for="version">Release Date:</asp:Label>
            <asp:Label class="fontclass" Text="" ID="lblRelDate" runat="server" />
        </div>
        <div id="divDesc" runat="server" visible="false">
            <asp:Label ID="lbldes" runat="server" meta:resourcekey="lbldes" class="control-label castLbl" for="version">Description: </asp:Label>
            <div class="col-lg-4">
                <div class="fontclass" id="divDescContent" runat="server" />
            </div>
        </div>
        <div id="divFeat" runat="server" visible="false">
            <asp:Label ID="lblfeat" runat="server" meta:resourcekey="lblfeat" class="control-label castLbl" for="version">Features: </asp:Label>
            <div class="col-lg-4">
                <div class="fontclass" id="divFeaturesContent" runat="server" />
            </div>
        </div>
        <div id="divEnhan" runat="server" visible="false">
            <asp:Label ID="lblenhan" runat="server" meta:resourcekey="lblenhan" class="control-label castLbl" for="version">Enhancements: </asp:Label>
            <div class="col-lg-4">
                <div class="fontclass" id="divEnhanContent" runat="server" />
            </div>
        </div>
        <%--   <div id="divBug" runat="server" visible="false">
            <asp:Label ID="lblbf" runat="server" meta:resourcekey="lblbf" class="control-label castLbl" for="version">Bugs Fixed: </asp:Label>
        </div>
        <div class="col-lg-4">
            <div class="fontclass" id="divBugContent" runat="server" />
        </div>--%>

        <button type="button" id="btnClose" class="btn" onclick="parent.closeModalDialog()" title=""></button>

    </div>
</body>
</html>
