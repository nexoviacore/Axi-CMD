<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Signin.aspx.cs" Inherits="Signin" %>

<%@ OutputCache Duration="1" Location="None" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert Sign in" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=appTitle%></title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
    <style>
        ::-ms-reveal {
            display: none;
        }
    </style>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" href="assets/media/logos/favicon.ico" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script>
        if (typeof localStorage != "undefined") {
            var customGS = "<link id=\"customGlobalStyles\" data-proj=\"\" href=\"\" rel=\"stylesheet\" />";
            document.write(customGS);
        }
        if (typeof localStorage != "undefined") {
            var customGS = "<link id=\"axGlobalThemeStyle\" data-themfld=\"\" href=\"\" rel=\"stylesheet\" />";
            document.write(customGS);
        }
    </script>
    <link rel="shortcut icon" href="../images/favicon.ico" />
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
    <meta http-equiv="EXPIRES" content="0" />
    <!-- <link href="../Css/Icons/icon.css" rel="stylesheet" /> -->
    <script>
        if (!('from' in Array)) {
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>

    <script src="../Js/jquery.browser.min.js" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/login.min.js?v=104"></script>
    <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>
    <script src="../Js/jsencrypt.js?v=1" type="text/javascript"></script>
    <script src="../Js/crypto-js.js?v=1" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script type="text/javascript">
        history.go(1);
        var cdt = new Date();
        let bst = cdt.getDate() + "-" + (cdt.getMonth() + 1) + "-" + cdt.getFullYear() + " " + cdt.getHours() + ":" + cdt.getMinutes() + ":" + cdt.getSeconds() + "." + cdt.getMilliseconds();
        let appTUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        var ispost = '<%=isPostback%>';
        var axThemeStyleFolder = '<%=axThemeStyleFolder%>';
        if (ispost == "false")
            localStorage.setItem("BST-" + appTUrl, bst);
        var gllangType = '<%=langType%>';
        var isUserLang = '<%=isUserLang%>';
        var isPowerBy = '<%=isPowerBy%>';
        var diFileInfo = '<%=strFileinfo%>';
        var hybridGUID = '<%=hybridGUID%>';
        var hybridDeviceId = '<%=hybridDeviceId%>';
        var keepMeAutoLogin = '<%=KeepMeAutoLogin%>';
        var keepMeAutoPwd = '<%=KeepMeAutoPwd%>';
        var KeepMeAutoLoginWeb = '<%=KeepMeAutoLoginWeb%>';
        var isMobile = isMobileDevice();
        var isOfficeSSO = '<%=isOfficeSSO%>';
        var oktaclientKey = '<%=oktaclientKey%>';
        var oktadomain = '<%=oktadomain%>';
        var office365clientKey = '<%=office365clientKey%>';
        var ssoredirecturl = '<%=ssoredirecturl%>';
        var ExecTraceOn = '<%=ExecTraceOn%>';
        var onlyssoKey = '<%=onlyssoKey%>';
        var multipleAccessCode = '<%=multipleAccessCode%>';
        var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        localStorage.removeItem('web_browser-' + projUrl);
        localStorage.removeItem('web_browser_dup-' + projUrl);
    </script>

    <script src="../Js/sso.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/msal.min.js" type="text/javascript"></script>
    <script src="../Js/okta-auth-js.min.js" type="text/javascript"></script>

    <noscript>
        <div>
            JavaScript is turned off in your web browser. Turn it on to take full advantage
            of this site, then refresh the page.
        </div>
    </noscript>

</head>
<body class="page-header-fixed login" id="main_body" runat="server" dir="<%=direction%>">
    <video id="bgvid" runat="server" playsinline="" autoplay="" muted="" loop="" class="d-none">
        <source src="" type="video/mp4" id="bgvidsource" runat="server" />
    </video>
    <form name="form2" method="post" action="mainnew.aspx" id="form2" defaultfocus="uname" class="form-vertical login-form" novalidate>
        <div>
            <%=strParams.ToString() %>
        </div>
    </form>
    <div class="row-fluid login-main card login-inner w-lg-500px m-auto">
        <div class="center-view">
            <div class="page-loader-wrapper" style="display: none">
                <div class="loader">
                    <div class="preloader">
                        <div class="spinner-layer pl-blue-grey">
                            <div class="circle-clipper left">
                                <div class="circle"></div>
                            </div>
                            <div class="circle-clipper right">
                                <div class="circle"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="center-view">
                <form id="form1" runat="server">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                        <Services>
                            <asp:ServiceReference Path="../WebService.asmx" />
                            <asp:ServiceReference Path="../CustomWebService.asmx" />
                        </Services>
                    </asp:ScriptManager>
                    <div id="SigninTemplate" class="position-fixed top-0 start-0 vw-100 vh-100 overflow-auto" runat="server">
                        <asp:Literal ID="LandPageTemplate" runat="server" Text=""></asp:Literal>
                    </div>

                    <asp:Panel runat="server" ID="panelSignin">
                        <div class="login-wrapper" runat="server" id="divPanelSignin" style="display: none">
                            <div class="w-lg-500px p-8 p-lg-12 mx-auto">
                                <div class="text-center mb-8">
                                    <div class="form-title">

                                        <img class="mb-2" src="../images/loginlogo.png" loading="lazy" />
                                        <div>
                                            <asp:Label ID="lblSignin" class="form-label fs-1 fw-boldest text-dark" runat="server" meta:resourcekey="lblSignin">Sign In</asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel runat="server" ID="panelUser">
                                    <div class="control-group" id="selectProj" runat="server">
                                        <div class="fv-row mb-8 fv-plugins-icon-container">
                                            <div class="d-flex flex-stack">
                                                <asp:Label ID="lblslctproj" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblslctproj">Select Project
                                                </asp:Label>
                                            </div>
                                            <select class="form-select form-select-solid m-wrap placeholder-no-fix" runat="server" data-control="select2" data-placeholder="Select Project" data-allow-clear="true" data-select2-id="select2-data-11-3n80" aria-hidden="true" onblur="GetProjLang();" id='axSelectProj' name="axSelectProj" tabindex="2">
                                            </select>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <div class="fv-row mb-8 fv-plugins-icon-container">
                                            <div class="d-flex flex-stack">
                                                <asp:Label ID="lblusername" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblusername">
                            User Name</asp:Label>
                                            </div>
                                            <input class="m-wrap placeholder-no-fix form-control form-control-solid" id="axUserName" tabindex="3" runat="server" type="text"
                                                autocomplete="off" placeholder="" name="axUserName" title="Username" required>
                                        </div>
                                    </div>

                                    <div class="hide control-group" id="axLangFld" runat="server">
                                        <div class="fv-row my-8 mb-4 fv-plugins-icon-container">
                                            <div class="input-icon left">
                                                <div class="d-flex flex-stack mb-1">
                                                    <asp:Label ID="lblslctlang" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblslctlang">Select Language</asp:Label>
                                                </div>
                                                <select class="form-select form-select-solid" data-control="select2" data-placeholder="Select Language" data-allow-clear="true" tabindex="3" id="axLanguage" name="axLanguage" runat="server" value=''></select>
                                                <div class="fv-plugins-message-container invalid-feedback"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="hide control-group" id="axLangFlddb" runat="server">
                                        <div class="fv-row my-8 mb-4 fv-plugins-icon-container">
                                            <div class="input-icon left">
                                                <div class="d-flex flex-stack mb-1">
                                                    <asp:Label ID="Label1" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblslctlang">Select Language</asp:Label>
                                                </div>
                                                <select class="form-select form-select-solid" data-control="select2" data-placeholder="Select Language" data-allow-clear="true" tabindex="3" id="axLanguagedb"></select>
                                                <div class="fv-plugins-message-container invalid-feedback"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <div class=" agform form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4" id="axstaysignin" runat="server" visible="false">
                                            <div class="controls my-2">
                                                <div class="input-icon left">
                                                    <input type="checkbox" id="signedin" runat="server" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px" tabindex="5" title="Keep me sign in?" />
                                                    <asp:Label runat="server" ID="lblstaysin" meta:resourcekey="lblstaysin" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="signedin">Keep me sign in?</asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="fv-plugins-message-container invalid-feedback"></div>

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

                                    <div class="form-actions d-flex flex-row flex-column-fluid">
                                        <div class="d-flex flex-row-fluid">
                                            <asp:Button runat="server" Text="Next" title="Next" TabIndex="6" ID="btnNext" class="btn btn-lg btn-primary mb-5 w-100" OnClick="btnNext_Click" OnClientClick="return chkNextForm();" />
                                        </div>
                                    </div>
                                    <div class="form-actions flex-row flex-column-fluid mb-8 d-none" id="divsso" runat="server">
                                        <div class="d-flex flex-row flex-column-fluid">
                                            <span class="form-label fs-6 text-gray-500">You can login using</span>
                                        </div>
                                        <div class="d-flex flex-row-auto mt-3">
                                            <button id="OktaBtn" runat="server" class="btn btn-icon btn-light-okta me-2 btn-sm" onclick="axOktaLogin();return false;" text="" visible="false" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Okta">
                                                <span class="svg-icon svg-icon-4">
                                                    <svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" viewBox="0 0 64 64">
                                                        <path d="M32 0C14.37 0 0 14.267 0 32s14.268 32 32 32 32-14.268 32-32S49.63 0 32 0zm0 48c-8.866 0-16-7.134-16-16s7.134-16 16-16 16 7.134 16 16-7.134 16-16 16z" fill="" />
                                                    </svg></span></button>

                                            <button id="Office365Btn" runat="server" class="btn btn-icon btn-light-office365 me-2 btn-sm" onclick="Office365Init();return false;" visible="false" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Office365">
                                                <span class="svg-icon svg-icon-4">
                                                    <svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="50" height="50" viewBox="0 0 48 48" style="height: 25px">
                                                        <g id="surface1">
                                                            <path <%--style={{fill: "#fff"}}--%> d="M 7 12 L 29 4 L 41 7 L 41 41 L 29 44 L 7 36 L 29 39 L 29 10 L 15 13 L 15 33 L 7 36 Z " fill="" />
                                                        </g></svg></span></button>

                                            <button id="GoogleBtn" class="btn btn-icon btn-light-google me-2 btn-sm " runat="server" onserverclick="GoogleBtn_Click" visible="false" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Google">
                                                <span class="svg-icon svg-icon-4">
                                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 488 512">
                                                        <path d="M488 261.8C488 403.3 391.1 504 248 504 110.8 504 0 393.2 0 256S110.8 8 248 8c66.8 0 123 24.5 166.3 64.9l-67.5 64.9C258.5 52.6 94.3 116.6 94.3 256c0 86.5 69.1 156.6 153.7 156.6 98.2 0 135-70.4 140.8-106.9H248v-85.3h236.1c2.3 12.7 3.9 24.9 3.9 41.4z" fill="" />
                                                    </svg></span></button>

                                            <button id="FacebookBtn" class="btn btn-icon btn-light-facebook me-2 btn-sm " runat="server" onserverclick="FacebookBtn_Click" visible="false" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Facebook">
                                                <span class="svg-icon svg-icon-4">
                                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512" width="24" height="24">
                                                        <path d="M279.14 288l14.22-92.66h-88.91v-60.13c0-25.35 12.42-50.06 52.24-50.06h40.42V6.26S260.43 0 225.36 0c-73.22 0-121.08 44.38-121.08 124.72v70.62H22.89V288h81.39v224h100.17V288z" fill=""></path></svg>
                                                </span>
                                            </button>

                                            <button id="OpenIdBtn" class="btn btn-icon btn-light-openid me-2 btn-sm " runat="server" onclick="OpenIdProject();" visible="false" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="OpenID">
                                                <span class="svg-icon svg-icon-4">
                                                    <svg xmlns="http://www.w3.org/2000/svg" viewbox="0 0 64 64" role="img" xmlns:xlink="http://www.w3.org/1999/xlink">
                                                        <path data-name="layer2"
                                                            d="M29.281 19.813C14.656 21.594 2 30.084 2 39.647c0 9.937 11.813 18.094 27.281 19.5v-5.719C18.875 52.116 11.094 46.4 11.094 39.647m0 0c0-6.469 8.625-12.147 18.188-13.741m9.093.094l1.594.147a28.171 28.171 0 0 1 9.392 3.245l-4.892 2.755L62 35.991l-1.219-13.032-4.7 2.645a49.477 49.477 0 0 0-17.709-5.792"
                                                            fill="none" stroke="#202020" stroke-linecap="round" stroke-miterlimit="10"
                                                            stroke-width="2" stroke-linejoin="round">
                                                        </path>
                                                        <path data-name="layer1" fill="none" stroke="#202020" stroke-linecap="round"
                                                            stroke-miterlimit="10" stroke-width="2" d="M29.281 7.959v51.094l9.094-4.219V3.553l-9.094 4.406z"
                                                            stroke-linejoin="round">
                                                        </path>
                                                    </svg>
                                                </span>
                                            </button>
                                            <button id="OpenIdBtnclick" class="d-none" runat="server" text="OpenId" onserverclick="OpenIdBtnclick_Click"></button>

                                            <%--<button id="WindowsBtn" class="btn btn-icon btn-light-windows me-2 btn-sm" runat="server" onclick="CheckWindowsBtn();" visible="false" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Windows">
                                                <span class="svg-icon svg-icon-4">
                                                    <svg xmlns="" width="64" height="64" shape-rendering="geometricPrecision" text-rendering="geometricPrecision" image-rendering="optimizeQuality" fill-rule="evenodd" clip-rule="evenodd" viewBox="0 0 640 640">
                                                        <path d="M.2 298.669L0 90.615l256.007-34.76v242.814H.201zM298.658 49.654L639.905-.012v298.681H298.657V49.654zM640 341.331l-.071 298.681L298.669 592V341.332h341.33zM255.983 586.543L.189 551.463v-210.18h255.794v245.26z" fill=""></path></svg></span></button>--%>
                                            <button id="WindowsBtn" class="btn btn-icon btn-light-windows me-2 btn-sm" runat="server" onclick="CheckWindowsBtnUser();" visible="false" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Windows">
                                                <span class="svg-icon svg-icon-4">
                                                    <svg xmlns="" width="64" height="64" shape-rendering="geometricPrecision" text-rendering="geometricPrecision" image-rendering="optimizeQuality" fill-rule="evenodd" clip-rule="evenodd" viewBox="0 0 640 640">
                                                        <path d="M.2 298.669L0 90.615l256.007-34.76v242.814H.201zM298.658 49.654L639.905-.012v298.681H298.657V49.654zM640 341.331l-.071 298.681L298.669 592V341.332h341.33zM255.983 586.543L.189 551.463v-210.18h255.794v245.26z" fill=""></path></svg></span></button>
                                            <%--<button id="WindowCloneBtn" class="d-none" runat="server" text="Windows" onserverclick="WindowsBtn_Click"></button>--%>
                                            <button id="WindowCloneBtnOld" class="d-none" runat="server" text="Windows" onserverclick="btnNext_Click"></button>

                                            <button id="SamlBtn" class="btn btn-icon btn-light-saml me-2 btn-sm " runat="server" onclick="chkSSOLogin();" onserverclick="SamlBtn_Click" visible="false" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="SAML">
                                                <span class="svg-icon svg-icon-4">
                                                    <svg version="1.0" xmlns="http://www.w3.org/2000/svg" width="24pt" height="24pt" viewBox="0 0 24 24">
                                                        <g transform="translate(0,24) scale(0.10,-0.1)">
                                                            <path d="M97 210 c-8 -19 -18 -46 -22 -60 -5 -21 -3 -20 14 8 32 49 55 41 119
-43 l23 -30 -15 30 c-8 17 -35 53 -59 80 l-45 50 -15 -35z"
                                                                fill="" />
                                                            <path d="M32 148 c-11 -29 -23 -68 -27 -85 -7 -37 0 -40 66 -26 l34 7 -29 6
c-45 8 -53 30 -33 91 22 70 14 75 -11 7z"
                                                                fill="" />
                                                            <path d="M165 105 c21 -55 19 -65 -19 -80 -19 -8 -46 -16 -58 -16 -19 -1 -20
-2 -4 -6 27 -7 136 18 136 31 0 6 -15 29 -32 51 -17 22 -27 31 -23 20z"
                                                                fill="" />
                                                        </g></svg>
                                                </span>
                                            </button>
                                        </div>
                                    </div>

                                    <div class="copyrightlabs">
                                        <div class="">
                                            <span id="dvCopyRight" runat="server" class="copyrightpara text-dark mb-2 fw-bolder">Powered by Axpert</span>
                                            <span id="axpertVer" class="text-dark mb-2 fw-bolder float-end" runat="server"></span>
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>

                                    <%--   <div class="create-account btn btn-icon btn-white  btn-active-primary  m-4  position-absolute top-0 end-0" id="axpertConfig" runat="server" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Axpert Configuration" visible="false" onclick="OpenNewConnection()">
                                        <span class="material-icons material-icons-style">settings</span>
                                    </div>--%>
                                </asp:Panel>

                                <asp:Literal ID="panelPwd" runat="server"></asp:Literal>

                                <asp:Literal ID="panelOtp" runat="server"></asp:Literal>

                                <asp:Button runat="server" Text="Login" title="Login" TabIndex="6" ID="btnSubmit" class="d-none" OnClientClick="return chkLoginFormNew(this);" OnClick="btnSubmit_Click" />
                                <asp:Button runat="server" Text="OTP Login" title="OTP Login" TabIndex="6" ID="btnOTPLogin" class="d-none" OnClick="btnOTPLogin_Click" />
                                <asp:Button runat="server" Text="Resend OTP" title="PWD OTP" TabIndex="6" ID="btnResendOtp" class="d-none" OnClick="btnResendOtp_Click" />
                                <asp:Button runat="server" Text="Windows SSO" title="Windows SSO" TabIndex="6" ID="WindowCloneBtn" class="d-none" OnClientClick="return chkLoginFormNew(this);" OnClick="WindowsBtn_Click" />
                            </div>
                        </div>
                        <input type="hidden" runat="server" name="hdnAxProjs" id="hdnAxProjs" />
                        <input type="hidden" runat="server" name="hdnMobDevice" id="hdnMobDevice" />
                        <input type="hidden" runat="server" name="hdnHybridGUID" id="hdnHybridGUID" />
                        <input type="hidden" runat="server" name="hdnHybridDeviceId" id="hdnHybridDeviceId" />
                        <input type="hidden" runat="server" name="hdnTimeZone" id="hdnTimeZone" />
                        <input type="hidden" runat="server" name="hdnProjName" id="hdnProjName" />
                        <input type="hidden" runat="server" name="hdnUserName" id="hdnUserName" />
                        <input type="hidden" runat="server" name="hdnProjLang" id="hdnProjLang" />
                        <input type="hidden" runat="server" name="hdnPuser" id="hdnPuser" />
                        <input type="hidden" runat="server" name="hdnOtpauth" id="hdnOtpauth" />
                        <input type="hidden" runat="server" name="hdnClientDt" id="hdnClientDt" />
                        <input type="hidden" runat="server" name="hdnWSSoBtn" id="hdnWSSoBtn" />
                        <asp:Label ID="lblCustomerror" runat="server" meta:resourcekey="lblCustomerror" Visible="false">Server error. Please try again.If the problem continues, please contact your administrator.</asp:Label>

                        <input type="hidden" id="browserElapsTime" runat="server" />
                        <input type="hidden" id="hdnLangs" runat="server" disabled="disabled" />
                        <input type="hidden" id="hdnAppTitle" runat="server" disabled="disabled" />
                        <input type="hidden" id="hdnLastOpenpage" runat="server" disabled="disabled" />
                        <input type="hidden" id="hdnbtforLogin" runat="server" />
                        <input type="hidden" id="hdnSrtforLogin" runat="server" />
                        <input type="hidden" id="hdnBwsrid" runat="server" />
                        <input type="hidden" id="hdnEncKey" runat="server" />
                        <asp:HiddenField ID="_antiforgery" runat="server" />
                    </asp:Panel>
                </form>
            </div>
        </div>
    </div>

    <form id="form3" class="d-none">
        <input type="hidden" runat="server" name="mobDevice" id="mobDevice" />
        <input type="hidden" runat="server" name="duplicateUser" id="duplicateUser" />
        <input type="hidden" runat="server" name="hbtforDupLogin" id="hbtforDupLogin" />
        <button type="submit" runat="server" title="Login" id="btnSubmitUser" class="hotbtn btn hide" />
    </form>


    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->
    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->
    <script src="../assets/scripts/app.min.js?v=1" type="text/javascript"></script>
    <script src="../assets/scripts/tasks.min.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL STYLES -->
    <!-- END PAGE LEVEL SCRIPTS -->
    <script type="text/javascript">
        jQuery(document).ready(function () {
            KTApp.init();
            App.init(); // initlayout and core plugins            
            $("#sidemenu-leftt").click();
        });
    </script>
</body>
</html>

