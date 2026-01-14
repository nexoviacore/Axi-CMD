<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fileupload.aspx.cs" Inherits="fileupload" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="File Upload" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>File Upload</title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction=="ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss" ) %>
    </asp:PlaceHolder>
    <!-- <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" /> -->
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
    <!-- <link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" id="generic" /> -->
    <link id="themecss" type="text/css" rel="Stylesheet" href="" />
    <link href="../Css/fileupload.min.css?v=3" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <%--<script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>--%>
    <script src="../Js/iFrameHandler.min.js"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <%--custom alerts start--%>
    <!-- <link href="../Css/animate.min.css" rel="stylesheet" /> -->
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <script src="../Js/fileUpload.js?v=22" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>


</head>
<body dir='<%=direction%>'>

    <form id="form1" runat="server" enctype="multipart/form-data">

        <div class="dcContent d-flex w-100 flex-column mb-5">
            <div class="lablattactxt">
                <table style="width: 100%" class="gridDatauploadtxt">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblslctfilename" CssClass="form-label col-form-label pb-1 fw-boldest" meta:resourcekey="lblslctfilename" runat="server">Please select a file and then click 'Attach' button.</asp:Label>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="clear"></div>
            <div>
                <div class="file-upload">
                    <div tabindex="0" class="file-select">
                        <div class="file-select-button" id="fileName">
                            <asp:Label ID="lblfilename" CssClass="form-label col-form-label pb-1 fw-boldest" meta:resourcekey="lblfilename" runat="server">Choose File</asp:Label>
                        </div>
                        <div class="file-select-name" id="noFile">
                            <asp:Label ID="lblnofilename" CssClass="form-label col-form-label pb-1 fw-boldest" meta:resourcekey="lblnofilename" runat="server">No file chosen...</asp:Label>
                        </div>
                        <input tabindex="-1" runat="server" type="file" name="filMyFile" id="filMyFile" accept="All Files/*" multiple="multiple" />
                    </div>
                </div>
            </div>
            <asp:Label ID="fileuploadsts" CssClass="form-label col-form-label fw-boldest text-danger" meta:resourcekey="fileuploadsts" Text="" runat="server" ForeColor="#DB2222"></asp:Label>
            <asp:HiddenField ID="upsts" Value="" runat="server" />
            <asp:HiddenField ID="fname" Value="" runat="server" />
            <asp:HiddenField ID="hdnAction" Value="" runat="server" />

            <asp:Label ID="lblFileUp" CssClass="form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblFileUp" Visible="false">File uploaded successfully!</asp:Label>
            <asp:Label ID="lblfilecn" CssClass="form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblfilecn" Visible="false"></asp:Label>
            <asp:Label ID="lblAnError" CssClass="form-label col-form-label pb-1 fw-boldest" runat="server" meta:resourcekey="lblAnError" Visible="false">An Error Occured. Please Try Again!</asp:Label>
        </div>
        <div class="d-flex">
            <asp:Button ID="cmdSend" class="btn btn-primary d-inline-flex align-items-center shadow-sm me-2 handCur" title="Attach" runat="server" Text="Attach" OnClick="cmdSend_Click" />&nbsp;
            <input name="close" type="button" id="close" value="Close" title="Close" class="coldbtn btn handCursor d-none" onclick="closeUploadDialog();">
        </div>
    </form>

</body>
</html>
