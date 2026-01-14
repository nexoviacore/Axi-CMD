<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Forgotpassword.aspx.cs" Inherits="aspx_Forgotpassword" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta name="description" content="Forgot Password">
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP">
    <meta name="author" content="Agile Labs">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title><%=appTitle%></title>


    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>

    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" href="../images/favicon.ico" />


    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>

    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>

    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>

    <script type="text/javascript" src="../Js/login.min.js?v=104"></script>
    <script src="../Js/ForgotPassword.min.js?v=15" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>

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
    <script>
        var gllangType = '<%=langType%>';
        var diFileInfo = '<%=strFileinfo%>';
        var isMobile = isMobileDevice();
    </script>
</head>

<body id="main_body" runat="server" dir="<%=direction%>">
    <video id="bgvid" runat="server" playsinline="" autoplay="" muted="" loop="" class="d-none">
        <source src="" type="video/mp4" id="bgvidsource" runat="server">
    </video>
    <noscript>
        <div class="center-view">
            <asp:Label ID="lblscript" runat="server" meta:resourcekey="lblscript">JavaScript is turned off in your web browser. Turn it on to take full advantage of this site, then refresh the page.</asp:Label>
        </div>
    </noscript>
    <%-- <div id="login-container">


        <div id="container-arrow">--%>
    <form id="Form1" runat="server" class="login-wrapper card login-inner w-lg-500px m-auto">
        <%--<div class="d-none">--%>

        <%--<div id="login-box" <%--class="card"--%>
        <%-- <div class="center-view">--%>
        <div class="w-lg-500px p-8 p-lg-12 mx-auto">
            <div class="text-center mb-8">
                <div class="form-title">
                    <img src="../images/loginlogo.png">
                    <div><span id="lblSignin" class="form-label fs-1 fw-boldest text-dark">Forgot password</span></div>
                    <%--<asp:Label ID="lblfrgotpwd" runat="server" meta:resourcekey="lblfrgotpwd">Forgot password</asp:Label>--%>
                </div>
            </div>
            <%--</div>--%>
            <%--<div>--%>
            <div class="toplbl" id="projlbl" runat="server">
            </div>
            <div class="control-group fields" id="selectProj1" runat="server">
                <%--<asp:Label ID="lblproj" runat="server" meta:resourcekey="lblproj" class="control-label visible-ie8 visible-ie9">
                            Select Project</asp:Label>--%>
                <div class="controls field-wrapper">
                    <div class="input-icon left">
                        <%--<i class="icon-desktop"></i>--%>
                        <input type='text' value='' id='axSelectProj' name="axSelectProj" runat="server" tabindex='2' placeholder='' class='m-wrap placeholder-no-fix new-search-input search-input' />
                        <div class="field-placeholder">

                            <asp:Label ID="lblproj" runat="server" meta:resourcekey="lblproj">
                            Select Project</asp:Label>
                        </div>
                        <i class="icon-cross" runat="server" title="Clear" onclick="$('#axSelectProj').val('').focus();"></i>
                        <i class="icon-chevron-down autoClickddl" runat="server" title="Select Project" onclick="$('#axSelectProj').autocomplete('search','').focus();"></i>
                    </div>
                </div>
            </div>
            <div class="control-group">
                <div class="fv-row mb-8 fv-plugins-icon-container">
                    <div class="input-icon left">
                        <asp:Label ID="lblusername" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblusername">
                            User Name</asp:Label>
                        <input class="m-wrap my-4  placeholder-no-fix form-control form-control-lg form-control-solid" id="txtName" runat="server" type="text"
                            autocomplete="off" placeholder="" name="uname" title="Username" required>
                        <%--<div class="field-placeholder">--%>

                        <%--</div>--%>
                    </div>
                </div>
            </div>
            <%-- <div class=" control-group fields">
                        <div class="controls field-wrapper">
                            <div class="input-icon left">
                                <%--<i class="icon-user"></i>--%>
            <%--<input type="text" id="txtName1" name="uname" runat="server" value="" tabindex="2"
                                    size="15" autocomplete="off" class="loginContr" placeholder="" required />
                                <div class="field-placeholder">
                                        
                                            <asp:Label ID="lblusername1" runat="server" meta:resourcekey="lblusername">
                            User Name</asp:Label>
                                    </div>
                            </div>
                        </div>
                    </div>--%>
            <div class="control-group">
                <div class="fv-row mb-8 fv-plugins-icon-container">
                    <div class="input-icon left">
                        <asp:Label ID="lblpwd" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblpwd">
                            Email</asp:Label>
                        <input type="text" id="txtMl" name="email" runat="server" value="" tabindex="2"
                            class="loginContr my-4 m-wrap placeholder-no-fix form-control form-control-lg form-control-solid" placeholder="" required>
                        <input type="text" id="txtMlOrg" name="email" runat="server" value="" tabindex="2"
                            class="loginContr my-4 m-wrap placeholder-no-fix form-control form-control-lg form-control-solid d-none" placeholder="" required>
                        <%--<div class="field-placeholder"></div>--%>
                    </div>
                </div>
            </div>

            <%if (ConfigurationManager.AppSettings["enableCaptcha"] != null && ConfigurationManager.AppSettings["enableCaptcha"].ToString() == "true")
                { %>
            <div class="control-group">
                <div class="fv-row mb-8 fv-plugins-icon-container">
                    <div class="d-flex flex-stack">
                        <asp:Label ID="lblcaptcha" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblcaptcha">
                                    Captcha
                        </asp:Label>
                    </div>
                    <BotDetect:WebFormsCaptcha runat="server" ID="DynamicCaptcha" UserInputID="CaptchaCodeTextBox" CodeStyle="Alphanumeric" SoundEnabled="false" CodeLength="7" AutoReloadExpiredCaptchas="true" AutoReloadTimeout="7200" />
                    <asp:TextBox ID="CaptchaCodeTextBox" runat="server" CssClass="m-wrap placeholder-no-fix form-control form-control-solid"></asp:TextBox>
                </div>
            </div>
            <%  } %>

            <div class="d-flex flex-row flex-column-fluid">
                <div class="d-flex flex-row-fluid">
                    <button type="submit" class="divbutton btn btn-lg btn-primary mb-5 w-100" id="btnSubmit" onclick="javascript: ForgotPwd(); return false;">
                        <span class="indicator-label">Send Password</span>
                        <span class="indicator-progress">Please wait...
                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                        </span>
                    </button>
                </div>
                <%--<div class="divbutton">
                        <input type="submit" title="Send password" id="btnSubmit1" value="Send password" class="hotbtn btn" tabindex="3"
                            onclick="javascript: ForgotPwd();" />
                    </div>--%>
                <div class="Backlink d-flex flex-row-fluid ms-5">
                    <!--<div class="d-flex flex-row-auto ms-2 mt-1">-->
                    <a href="#" onclick="OpenSigninPage();" class="btn btn-text-primary btn-active-light-primary btn-lg mb-5 w-100  "><span id="lbllogin" runat="server" meta:resourcekey="lbllogin" class="indicator-label ">Go Back To Sign In</span></a>
                </div>
                <%--<div class="Backlink">
                        <a onclick="OpenSigninPage();" href="javascript:void(0)">
                            <asp:Label ID="lbllogin1" runat="server" meta:resourcekey="lbllogin">Go back to login</asp:Label></a>
                    </div>--%>
            </div>
        </div>

        <label runat="server" id="lblMessage">
        </label>
        <asp:Label runat="server" Text="" ID="Label2" CssClass="lblMsg"></asp:Label>
        <%--</div>--%>
        <%-- </div>
                </div>--%>
        <%--</div>--%>
        <%-- </div>
    </div>--%>

        <input type="hidden" id="hdnLangs" runat="server" />
        <div>
            <asp:Button ID="btnReset" class="hotbtn btn" runat="server" OnClick="btnReset_Click" />
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
                <asp:ServiceReference Path="../CustomWebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <asp:HiddenField ID="hdnUsrName" runat="server" />
        <asp:HiddenField ID="hdnMl" runat="server" />
        <asp:HiddenField ID="hdnAxProjs" runat="server" />
        <asp:HiddenField ID="hdnProj" runat="server" />
        <asp:HiddenField ID="hdnActive" runat="server" />
        <asp:HiddenField ID="hdnNickname" runat="server" />

        <%--<label runat="server" id="Label1">
        </label>--%>
    </form>
</body>
</html>
