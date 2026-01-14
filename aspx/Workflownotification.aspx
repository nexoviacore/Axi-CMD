<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Workflownotification.aspx.cs" Inherits="Workflownotification" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert Sign in" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Axpert WorkFlow Notification</title>
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
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>

    <script type="text/javascript">
        var alertsTimeout = 3000;

        var isOfficeSSO = '<%=isOfficeSSO%>';
        var oktaclientKey = '<%=oktaclientKey%>';
        var oktadomain = '<%=oktadomain%>';
        var office365clientKey = '<%=office365clientKey%>';
        var ssoredirecturl = '<%=ssoredirecturl%>';
        var isMobile = isMobileDevice();
        var isPowerBy = '';
        var wfDefaultComm = '<%=actNameComment%>';
        var onlyssoKey = '<%=onlyssoKey%>';
        var saveBtnName = '<%=saveBtnName%>';
    </script>

    <script src="../Js/sso.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/msal.min.js" type="text/javascript"></script>
    <script src="../Js/okta-auth-js.min.js" type="text/javascript"></script>
    <script src="../Js/workflowNotification.min.js?v=9"></script>
</head>
<body class="page-header-fixed login" id="main_body" runat="server" dir="<%=direction%>">
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
                <form runat="server">
                    <div class="lpanel">
                        <asp:ScriptManager runat="server">
                        </asp:ScriptManager>
                        <asp:Panel runat="server" ID="panelSignin">
                            <div class="login-wrapper" runat="server" id="divPanelSignin">
                                <div class="w-lg-500px p-8 p-lg-12 mx-auto">
                                    <div class="text-center mb-8">
                                        <div class="form-title">
                                            <img class="mb-2" src="../images/loginlogo.png" loading="lazy" />
                                            <div>
                                                <asp:Label ID="lblSignin" class="form-label fs-1 fw-boldest text-dark" runat="server" meta:resourcekey="lblSignin">Sign In</asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="fv-row mb-8 fv-plugins-icon-container">
                                            <div class="d-flex flex-stack">
                                                <asp:Label ID="lblusername" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblusername">User Name</asp:Label>
                                            </div>
                                            <input class="m-wrap placeholder-no-fix form-control form-control-solid" id="axUserName" tabindex="0" runat="server" type="text"
                                                autocomplete="off" placeholder="" name="axUserName" title="User Name" required>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="fv-row mb-8 fv-plugins-icon-container">
                                            <div class="d-flex flex-stack">
                                                <asp:Label ID="lblpwd" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblpwd">Password</asp:Label>
                                            </div>
                                            <input class="m-wrap placeholder-no-fix form-control form-control-solid" id="axPassword" tabindex="0" runat="server" type="password"
                                                autocomplete="off" placeholder="" name="axPassword" title="Password" required>
                                        </div>
                                    </div>
                                    <div class="form-actions">
                                        <asp:UpdatePanel ID="updSubmit" runat="server">
                                            <ContentTemplate>
                                                <div class="form-actions d-flex flex-row flex-column-fluid">
                                                    <div class="d-flex flex-row-fluid">
                                                        <asp:Button runat="server" Text="Login" title="Login" TabIndex="6" ID="submit" class="btn btn-lg btn-primary mb-5 w-100" OnClick="submit_Click" OnClientClick="return wfchkNextForm();" />
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

                                                        <button id="WindowsBtn" class="btn btn-icon btn-light-windows me-2 btn-sm" runat="server" onclick="CheckWindowsBtnUser();" visible="false" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-dismiss="click" data-bs-trigger="hover" data-bs-original-title="Windows">
                                                            <span class="svg-icon svg-icon-4">
                                                                <svg xmlns="" width="64" height="64" shape-rendering="geometricPrecision" text-rendering="geometricPrecision" image-rendering="optimizeQuality" fill-rule="evenodd" clip-rule="evenodd" viewBox="0 0 640 640">
                                                                    <path d="M.2 298.669L0 90.615l256.007-34.76v242.814H.201zM298.658 49.654L639.905-.012v298.681H298.657V49.654zM640 341.331l-.071 298.681L298.669 592V341.332h341.33zM255.983 586.543L.189 551.463v-210.18h255.794v245.26z" fill=""></path></svg></span></button>

                                                        <button id="WindowCloneBtnOld" class="d-none" runat="server" text="Windows" onserverclick="submit_Click"></button>

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
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:HiddenField ID="hdnPwd" runat="server" />
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                    </div>

                    <div id="workflowComments" class="modal" data-backdrop="static" data-keyboard="false" data-easein="expandIn" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-target="#consumergoods2">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Comments</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-group">
                                        <textarea class="form-control" rows="10" id="comment" runat="server"></textarea>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="commentSave" class="btn btn-primary hotbtndynamic" OnClientClick="return CommentsValidate();" OnClick="comment_Click" Text="Save" title="Save" runat="server" />
                                    <%--<asp:Label ID="commentCancel" class="btn btn-default coldbtndynamic" data-dismiss="modal" aria-hidden="true" onclick="resetActions()" data-placement="bottom" title="Clear" runat="server" Text="Clear"></asp:Label>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="isAuthorized" runat="server" />
                    <asp:HiddenField ID="hdnComment" runat="server" />
                </form>
            </div>
        </div>
    </div>
</body>
</html>
