<%@ Page Language="VB" AutoEventWireup="false" CodeFile="signout.aspx.vb" Inherits="signout"
    Debug="true" %>

<!DOCTYPE html>

<html>
<head runat="server">

    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta name="description" content="Axpert Signout" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title><%=appTitle%></title>
    <link rel="shortcut icon" href="../images/favicon.ico" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(If(direction = "ltr", "~/UI/axpertUI/ltrBundleCss", "~/UI/axpertUI/rtlBundleCss")) %>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158"></script>
    <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>
    <script type="text/javascript" src="../Js/alerts.min.js?v=32"></script>
    <script>
        let appSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        localStorage.setItem('logged_in-' + appSessUrl, 'no');
        let instanceName = localStorage.getItem("instanceName-" + appSessUrl);
        localStorage.removeItem("unlmtUsername-" + appSessUrl);
        var diFileInfo = '<%=strFileinfo%>';
        var isMobile = isMobileDevice();
        $(document).ready(function () {
            if (typeof instanceName != "undefined" && instanceName != null) {
                let hrefVal = $("#lnkLogin").attr('href');
                $("#lnkLogin").attr("href", hrefVal + "?" + instanceName);
            }
            $("#lnkLogin").prop('title', lcm[389]);
            if (isMobileDevice()) {
                let custommoblogoexist = false;
                if (diFileInfo != "") {
                    $j("#main_body").removeAttr("style");
                    var imageUrl = "./../images/Custom/" + diFileInfo;
                    $j("#main_body").css({ "background-image": "url(" + imageUrl + ")", "background-size": "cover", "height": "100vh" });
                    custommoblogoexist = true;
                }
                if (custommoblogoexist == false) {
                    $j("#main_body").removeAttr("style");
                    var imageUrl = "./../AxpImages/login-img.png";
                    $j("#main_body").css({ "background-image": "url(" + imageUrl + ")", "background-size": "cover", "height": "100vh" });
                }

            }

            if (typeof localStorage != "undefined") {
                var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                var proj = localStorage["projInfo-" + projUrl] || "";
                if (typeof proj != "undefined" && proj != null)
                    proj = proj.replace(/[^a-zA-Z0-9_\-\/]/g, "");
                setProjectImages(proj);

                localStorage.removeItem('web_browser-' + projUrl);
                localStorage.removeItem('web_browser_dup-' + projUrl);
            }
        });

        function setProjectImages(proj) {
            var webBgImage = "../AxpImages/login-img.png";
            var mobBgImage = "../AxpImages/login-img.png";
            var webBgImageDiv = $("body");
            var mobBgImageDiv = $("body");

            if (proj) {
                getProjectAppLogo(proj, async = true, function (success) {
                    if (success !== null && success !== void 0 && success.d) {
                        var _JSON$parse = JSON.parse(success.d),
                            webbg = _JSON$parse.webbg,
                            mobbg = _JSON$parse.mobbg;

                        if (webbg && !mobbg) {
                            mobbg = webbg;
                        }

                        if (!isMobile) {
                            webBgImageDiv.css("background", "url(".concat(webbg ? "".concat(webbg, "?v=").concat(new Date().getTime()) : webBgImage, ") ").concat(webbg ? "no-repeat center center fixed" : "no-repeat fixed bottom")).css("background-size", "cover");
                        } else {
                            mobBgImageDiv.css("background", "url(".concat(mobbg ? "".concat(mobbg, "?v=").concat(new Date().getTime()) : mobBgImage, "?v=").concat(new Date().getTime(), ") ").concat(mobbg ? "no-repeat center center fixed" : "no-repeat fixed bottom")).css("background-size", "cover");
                        }
                    } else {
                        if (!isMobile) {
                            webBgImageDiv.css("background", "url(".concat(webBgImage, ") no-repeat fixed bottom")).css("background-size", "cover");
                        } else {
                            mobBgImageDiv.css("background", "url(".concat(mobBgImage, "?v=").concat(new Date().getTime(), ") no-repeat fixed bottom ")).css("background-size", "cover");
                        }
                    }
                }, function (error) {
                    if (!isMobile) {
                        webBgImageDiv.css("background", "url(".concat(webBgImage, ") no-repeat fixed bottom")).css("background-size", "cover");
                    } else {
                        mobBgImageDiv.css("background", "url(".concat(mobBgImage, "?v=").concat(new Date().getTime(), ") no-repeat fixed bottom")).css("background-size", "cover");
                    }
                });
            } else {
                if (!isMobile) {
                    webBgImageDiv.css("background", "url(".concat(webBgImage, ") no-repeat fixed bottom")).css("background-size", "cover");
                } else {
                    mobBgImageDiv.css("background", "url(".concat(mobBgImage, "?v=").concat(new Date().getTime(), ") no-repeat fixed bottom")).css("background-size", "cover");
                }
            }
        }
    </script>
</head>

<body id="main_body" runat="server" dir="<%=direction%>">
    <video id="bgvid" runat="server" playsinline="" autoplay="" muted="" loop="" class="d-none">
        <source src="" type="video/mp4" id="bgvidsource" runat="server" class="d-none">
    </video>
    <form id="form1" runat="server" class="card w-lg-500px m-auto min-h-250px">
        <div id="info-box" class="flex-column-auto card-body w-lg-500px p-8 p-lg-12 m-auto">
            <h1 class="text-dark fs-2x mb-5 text-center">
                <asp:Label ID="lblMsg" class="d-flex min-w-200px" meta:resourcekey="lblMsg" runat="server">You have successfully signed out.</asp:Label>
                <%--<asp:Label ID="lblLoggedout" runat="server" meta:resourcekey="lblLoggedout" Visible="false">You have successfully signed out.</asp:Label>--%>
                <asp:Label ID="lblCustomerror" runat="server" meta:resourcekey="lblCustomerror" Visible="false">Server error. Please try again.If the problem continues, please contact your administrator.</asp:Label>
            </h1>
            <div class="d-flex fw-bold fs-4 text-gray-600">
                <a href="<%=loginStr %>" class="btn btn-lg btn-primary mx-auto w-50" id="lnkLogin">Sign In</a>
            </div>
        </div>

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
    </form>
</body>
</html>
