<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageUpload.aspx.cs" Inherits="aspx_ImageUpload" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Image Upload" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Image Upload</title>

    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
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

    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>

    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>    
    <script type="text/javascript" src="../Js/iFrameHandler.min.js"></script>
    <script src="../Js/ImageUpload.min.js?v=14" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>

</head>
<body dir='<%=direction%>' class="btextDir-<%=direction%>">
    <form id="form1" runat="server" method="post" enctype="multipart/form-data" dir="<%=direction%>">
        <div class="container p-0">
            <div id="dvNavigate" class="form-group">
                <span id="divCamera" class="col-lg-6 d-none">
                    <label class="radio-inline">
                        <input type="radio" name="imageRadio" checked="true" id="rdbImgCamera" runat="server" data-rdb-value="Camera">Camera
                    </label>
                </span>
            </div>
            <div class="dcContent col-12 pb-5">
                <div id="dvCam" class="form-group">
                    <canvas id="canvasid" hidden="hidden"></canvas>
                    <img id="imgCanvas" src="" runat="server" alt="" hidden="hidden" />
                    <input type="hidden" runat="server" id="mydata" value="hidden" />
                    <video id="videoElement" class="vh-100 vw-100 mvh-100 max-vw-100" autoplay></video>
                    <div class="w-100 position-fixed bottom-0">
                        <asp:Button ID="btnCapture" runat="server" CssClass="btn btn-white btn-icon shadow-sm material-icons material-icons-style material-icons-4x m-auto d-flex " disabled="true" OnClientClick="snap();"
                            Text="camera" data-bs-toggle="tooltip" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Capture" OnClick="btnCapture_Click" />
                    </div>
                    <div class="position-fixed bottom-0 bg-white rounded-circle shadow-sm">
                        <button id="flip-btn" data-bs-toggle="tooltip" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Flip Camera" class="btn btn-white btn-icon shadow-sm material-icons material-icons-style material-icons-1 m-5">flip</button>
                    </div>
                    <asp:TextBox ID="filepathna" runat="server" Visible="true" class="d-none"></asp:TextBox>
                    <asp:TextBox ID="fname" runat="server" Visible="true" class="d-none"></asp:TextBox>
                </div>
                <input runat="server" type="hidden" id="imgUploadLimit" value="4000000" />

                <asp:Label ID="lblfilecn" runat="server" meta:resourcekey="lblfilecn" Visible="false">Image could not be uploaded. Invalid FileType</asp:Label>
                <asp:Label ID="lblfilesize" runat="server" meta:resourcekey="lblfilesize" Visible="false"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
