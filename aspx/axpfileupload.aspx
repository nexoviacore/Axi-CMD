<%@ Page Language="C#" AutoEventWireup="true" CodeFile="axpfileupload.aspx.cs" Inherits="aspx_axpfileupload" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="File Upload" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Axp File Upload </title>

    <link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" id="generic" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/iFrameHandler.min.js"></script>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" type="text/css" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <link href="../ThirdParty/Linearicons/Font/library/linearIcons.css" rel="stylesheet" />

    <%--custom alerts start--%>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script src="../Js/tstruct.min.js?v=673" type="text/javascript"></script>

    <script src="../Js/axpFileUpload.min.js?v=9" type="text/javascript"></script>

    <link href="../Css/tstructNewUi.min.css?v=82" rel="stylesheet" />
    <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" type="text/css" />
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
<body dir='<%=direction%>'>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="dcContent">
            <div class="lablattactxt">
                <table class="gridDatauploadtxt">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblslctfilename" meta:resourcekey="lblslctfilename" runat="server">Please select a file and then click 'Attach' button.</asp:Label>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="clear"></div>
            <div>
                <div class="file-upload">
                    <div tabindex="0" class="file-select">
                        <div class="file-select-button" id="fileName">
                            <asp:Label ID="lblfilename" meta:resourcekey="lblfilename" runat="server">Choose File</asp:Label>
                        </div>
                        <div class="file-select-name" id="noFile">
                            <asp:Label ID="lblnofilename" meta:resourcekey="lblnofilename" runat="server">No file chosen...</asp:Label>
                        </div>
                        <input runat="server" type="file" tabindex="-1" name="filMyFile" id="filMyFile" accept="All Files/*" />
                    </div>
                </div>
            </div>
            <asp:Label meta:resourcekey="fileuploadsts" ID="fileuploadsts" Text="" runat="server"></asp:Label>
            <asp:HiddenField ID="upsts" Value="" runat="server" />
            <asp:HiddenField ID="fname" Value="" runat="server" />
            <asp:HiddenField ID="randNumber" Value="" runat="server" />
            <asp:HiddenField ID="hdnAttFld" Value="" runat="server" />
            <asp:HiddenField ID="hdnAttName" Value="" runat="server" />
            <asp:HiddenField ID="hdnFilePath" Value="" runat="server" />
            <asp:HiddenField ID="hdnType" Value="" runat="server" />
            <asp:HiddenField ID="hdnDcNo" Value="" runat="server" />
            <asp:HiddenField ID="hdnaxpFilePath" Value="" runat="server" />
            <asp:HiddenField ID="hdnaxpFileSavedPath" Value="" runat="server" />

            <asp:Label ID="lblFileUp" runat="server" meta:resourcekey="lblFileUp" Visible="false">File uploaded successfully!</asp:Label>
            <asp:Label ID="lblfilecn" runat="server" meta:resourcekey="lblfilecn" Visible="false"></asp:Label>
            <asp:Label ID="lblAnError" runat="server" meta:resourcekey="lblAnError" Visible="false">An Error Occured. Please Try Again!</asp:Label>
        </div>
        <div class="row">
            <asp:Button ID="cmdSend" class="hotbtn btn handCursor" runat="server" Text="Upload" OnClick="cmdSend_Click" OnClientClick="javascript: return AllowAttachements();" ToolTip="Upload" />&nbsp;
            <input name="close" type="button" id="close" value="Close" title="Close" class="coldbtn btn handCursor" onclick="closeUploadDialog();">
        </div>
    </form>


</body>
</html>
