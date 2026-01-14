<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cpwd.aspx.cs" Inherits="aspx_cpwd" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert Tstruct" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Change Password</title>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <link rel="shortcut icon" href="../images/favicon.ico" />
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
    <style>
        ::-ms-reveal {
            display: none;
        }
    </style>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>

    <script src="../Js/jquery.browser.min.js" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>

    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script src="../Js/md5.min.js" type="text/javascript"></script>
    <script src="../Js/user.min.js?v=20" type="text/javascript"></script>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <script src="../Js/helper.min.js?v=172" type="text/javascript"></script>
    <script src="../Js/cpwd.min.js?v=23" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>
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

    <script type="text/javascript">        
        var cpwdRemark = "<%=Request.QueryString["remark"]%>";
        var diFileInfo = '<%=strFileinfo%>';
        var isMobile = isMobileDevice();
        $(document).ready(function () {
            <%if (Request.QueryString["remark"] != null)
        {  %>
            cpwdRemark = ("<%=Request.QueryString["remark"]%>");
            <%}%>
            $("#userid").val(document.getElementById('user000F0').value);
            $("#username").val(document.getElementById('username000F0').value);
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
        })
        parent.cpwdLoadExit = "true";
        var transid = 'pwda';
        var recordid = 0;
        var fldnameArr = new Array();
        var expr = new Array();
        var formcontrols = new Array();
        var patternName = new Array();
        var pattern = new Array();
        var AllowEmpty = new Array();
        var fNames = new Array();
        var gllangType = '<%=langType%>';
        fNames[0] = "uname";
        fNames[1] = "cepwd";
        fNames[2] = "epwd";
        fNames[3] = "pwd";
        fNames[4] = "cpwd";
        fNames[5] = "encpwd";
        var patterns = new Array();
        var isCloudApp = '<%=isCloudApp%>';
        <%if (remark != "chpwd")
        {%>
        var errorEnable = false;
        <%} %>
    </script>
</head>
<%
    string xHtm = string.Empty;
    string user = string.Empty;
    string sid = string.Empty;
    user = Session["user"].ToString();
    sid = Session["nsessionid"].ToString();
    xHtm = "<Script type='text/javascript'>";
    xHtm = xHtm + "function b() { a='" + Session["project"].ToString() + "';ba='" + user + "';c='pwda';d='" + sid + "';e='" + Session["AxRole"].ToString() + "'; f='" + (Session["trace"] == null ? "" : Session["trace"].ToString()) + "';SetTstProps(a,ba,c,d,e,f);}";
    xHtm = xHtm + "</script>";
    Response.Write(xHtm);
%>
<body id="main_body" runat="server" dir="<%=direction%>">
    <%if (remark != "chpwd")
        {  %>
    <video id="bgvid" runat="server" playsinline="" autoplay="" muted="" loop="" class="d-none">
        <source src="" type="video/mp4" id="bgvidsource" runat="server">
    </video>
    <%} %>
    <%if (remark != "chpwd")
        {  %>
    <div class="login card login-inner w-lg-500px m-auto">
        <div class="login-main">
            <div class="center-view">
                <%} %>
                <div class="Family btextDir-<%=direction%>" dir="<%=direction%>">
                    <div <% if (remark != "chpwd")
                        {%>
                        class="" <%}%>>
                        <form id="form1" runat="server" class="Pagebody" dir="<%=direction%>">
                            <div>
                                <asp:ScriptManager ID="ScriptManager1" runat="server">
                                    <Scripts>
                                        <asp:ScriptReference Path="../Js/gen.min.js?v=14" />
                                        <asp:ScriptReference Path="../Js/tstruct.min.js?v=673" />
                                    </Scripts>
                                    <Services>
                                        <asp:ServiceReference Path="../WebService.asmx" />
                                    </Services>
                                </asp:ScriptManager>
                            </div>

                            <%if (remark == "chpwd")
                                {%>
                            <div id="dvMainPwd" class="d-none" runat="server">
                                <div id="breadcrumb-panel">
                                    <div id='breadcrumb' class='icon-services left'>
                                        <div class='icon-services left bcrumb pd10'>
                                        </div>
                                    </div>
                                    <div id='icons' class="right" runat="server">
                                        <asp:ImageButton runat="server" CssClass="d-none" AlternateText="Save" ID="btnSubmit" OnClientClick="javascript:return ValidatePassword(this);"
                                            ImageUrl="../AxpImages/save.png" />
                                        &nbsp;&nbsp;
                                    </div>
                                </div>
                            </div>
                            <%}%>
                            <div>
                                <div>
                                    <%if (remark != "chpwd")
                                        { %>

                                    <div class="w-lg-500px p-8 p-lg-12 mx-auto">
                                        <%--<div class="card login-inner">--%>
                                        <%}%>

                                        <%if (remark != "chpwd")
                                            { %>

                                        <div class="form-title">
                                            <div class="text-center mb-8">
                                                <img class="mb-2" src="../images/loginlogo.png" loading="lazy" />
                                                <div>
                                                    <asp:Label ID="spnCpwdHeading" class="form-label fs-1 fw-boldest text-dark" runat="server">Sign In</asp:Label>
                                                </div>
                                            </div>
                                            <div id="dvMgs" class="d-none">
                                            </div>
                                            <%} %>
                                            <div class="dvuserid mb-5">
                                                <%if (remark == "chpwd")
                                                    { %>
                                                <asp:Label ID="lbluserid" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lbluserid" for="email">
                            Username</asp:Label>

                                                <input type="text" id="userid" name="user000F0" runat="server" readonly value="" onfocus="this.value=this.value;" tabindex=""
                                                    class="loginContr m-wrap my-1 placeholder-no-fix form-control form-control-lg form-control-solid bg-secondary" placeholder="" autofocus autocomplete="off">
                                                <input type="hidden" runat="server" name="user000F0" id="user000F0" value="" />
                                                <%} %>
                                            </div>
                                            <div class="dvusername mb-5">
                                                <%if (remark == "chpwd")
                                                    { %>
                                                <asp:Label ID="lblusername" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblusername" for="email">
                            Username</asp:Label>

                                                <input type="text" id="username" name="username000F0" runat="server" readonly value="" onfocus="this.value=this.value;" tabindex=""
                                                    class="loginContr m-wrap my-1 placeholder-no-fix form-control form-control-lg form-control-solid bg-secondary" placeholder="" autofocus autocomplete="off">
                                                <input type="hidden" runat="server" name="username000F0" id="username000F0" value="" />
                                                <%} %>
                                            </div>
                                            <div class="<%if (remark == "chpwd")
                                                { %>chpwd-position <%}
                                                else
                                                { %> firsttime-chpwd-position <%} %> form-horizontal"
                                                id="dvChngPwd">
                                                <div id="dvCp">
                                                    <div>
                                                        <input type="hidden" name="recordid000F0" value="0" />
                                                        <input type="hidden" name="html_transid000F0" value="pwda" /><input type="hidden"
                                                            name="rn000F0" value="<%=Session["user"]%>" />
                                                    </div>
                                                    <div class="control-group" id="Epwddiv">
                                                        <div class="fv-row mb-8 fv-plugins-icon-container">
                                                            <%if (remark != "chpwd")
                                                                {  %>
                                                            <div class="field-placeholder">
                                                                <asp:Label ID="Label1" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblexistingpwd" for="email">
                            Existing Password</asp:Label>
                                                                <%}%>
                                                                <%if (remark == "chpwd")
                                                                    { %>
                                                                <asp:Label ID="lblexistingpwd" class="form-label fs-6 fw-boldest text-dark" runat="server" meta:resourcekey="lblexistingpwd" for="email">
                            Existing Password</asp:Label>
                                                                <%} %>
                                                                <div class="input-group input-group-sm">
                                                                    <input type="password" id="existingPwd" name="swe000F0" runat="server" value="" onfocus="this.value=this.value;"
                                                                        class="loginContr m-wrap my-1 placeholder-no-fix form-control form-control-lg form-control-solid" placeholder="" onblur="document.getElementById('swee000F0').value=(this.value);" autofocus autocomplete="off">
                                                                    <span class="input-group-text" style="border: none;height: 46px;margin-top: 3px;margin-bottom: 3px;"><i toggle="#existingPwd" class="material-icons cursor-pointer fs-4 toggle-password">visibility_off</i></span>
                                                                </div>
                                                                <input type="hidden" runat="server" name="swee000F0" id="swee000F0" value="" />

                                                                <%if (remark != "chpwd")
                                                                    {  %>
                                                            </div>
                                                        </div>
                                                        <%} %>
                                                    </div>
                                                </div>
                                                <%-- </div>
                        </div>--%>

                                                <div class="control-group" id="Npwddiv">
                                                    <div class="fv-row mb-8 fv-plugins-icon-container">
                                                        <div class="input-icon left">
                                                            <%if (remark != "chpwd")
                                                                { %>
                                                            <div class="field-placeholder">
                                                                <asp:Label ID="Label2" runat="server" meta:resourcekey="lblnewpwd" class="form-label fs-6 fw-boldest text-dark" for="email">New Password</asp:Label>
                                                                <%} %>
                                                                <%if (remark == "chpwd")
                                                                    { %>
                                                                <asp:Label ID="lblnewpwd" runat="server" meta:resourcekey="lblnewpwd" class="form-label fs-6 fw-boldest text-dark" for="email">New Password</asp:Label>
                                                                <%} %>
                                                                <div class="<%if (remark != "chpwd")
                                                                    {%> controls field-wrapper <%}
                                                                    else
                                                                    { %> col-lg-7 <%} %>">
                                                                    <%if (remark != "chpwd")
                                                                        { %>
                                                                    <div class="input-icon left">
                                                                        <%} %>
                                                                        <div class="input-group input-group-sm">
                                                                            <input id="newPwd" type="password" value="" name="swn" onfocus="this.value = this.value;" tabindex="0"
                                                                                class="m-wrap my-1  placeholder-no-fix form-control form-control-lg form-control-solid <%if (remark != "chpwd")
                                                                                { %> border-left <%} %>"
                                                                                onblur="document.getElementById('sw000F0').value=md5authNew(this.value);" autocomplete="off" />
                                                                            <span class="input-group-text" style="border: none;height: 46px;margin-top: 3px;margin-bottom: 3px;"><i toggle="#newPwd" class="material-icons cursor-pointer fs-4 toggle-password">visibility_off</i></span>
                                                                        </div>
                                                                        <input runat="server" type="hidden" id="pwdlength" name="swlength" value="" />
                                                                        <input runat="server" type="hidden" id="sw000F0" name="sw000F0" value="" />
                                                                        <%if (remark != "chpwd")
                                                                            { %>
                                                                    </div>
                                                                </div>
                                                                <%} %>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group mb-5" id="Region">
                                                    <%if (remark != "chpwd")
                                                        { %>
                                                    <div class="field-placeholder">
                                                        <asp:Label ID="Label3" runat="server" meta:resourcekey="lblconfirmpwd" class="form-label fs-6 fw-boldest text-dark" for="email">Confirm Password<span class="allowempty">* </span></asp:Label>
                                                        <%} %>
                                                        <%if (remark == "chpwd")
                                                            { %>
                                                        <asp:Label ID="lblconfirmpwd" runat="server" meta:resourcekey="lblconfirmpwd" class="form-label fs-6 fw-boldest text-dark" for="email">Confirm Password<span class="allowempty">* </span></asp:Label>
                                                        <%}%>
                                                        <div class="<%if (remark != "chpwd")
                                                            { %> controls field-wrapper <%}
                                                            else
                                                            { %> col-lg-7 <%} %>">
                                                            <%if (remark != "chpwd")
                                                                { %>
                                                            <div class="input-icon left">
                                                                <%} %>
                                                                <div class="input-group input-group-sm">
                                                                    <input id="confirmPwd" type="password" value="" name="swc" onblur="document.getElementById('swc000F0').value=md5authNew(this.value);" onfocus="this.value=this.value;"
                                                                        class="m-wrap my-1  placeholder-no-fix form-control form-control-lg form-control-solid <%if (remark != "chpwd")
                                                                        { %> border-left <%} %>"
                                                                        autocomplete="off">
                                                                    <span class="input-group-text" style="border: none;height: 46px;margin-top: 3px;margin-bottom: 3px;"><i toggle="#confirmPwd" class="material-icons cursor-pointer fs-4 toggle-password">visibility_off</i></span>
                                                                </div>
                                                                <input runat="server" type="hidden" name="swc000F0" id="swc000F0" value="" />
                                                                <input runat="server" type="hidden" name="npwdHidden" id="npwdHidden" value="" />
                                                                <input runat="server" type="hidden" name="checkMsg" id="hdncheckMsg" value="" />
                                                                <input runat="server" type="hidden" name="npwdHiddenMd5" id="npwdHiddenMd5" value="" />
                                                                <input runat="server" type="hidden" name="alphanumeric" id="hdnalphanumeric" value="" />
                                                                <input runat="server" type="hidden" name="encryptkey" id="hdnencryptkey" value="" />
                                                                <input runat="server" type="hidden" name="pwdPolicy" id="hdnpwdPolicy" value="" />
                                                                <%-- <%If remark <> "chpwd" %>
                                                                <div class="field-placeholder">
                                                                    <asp:Label ID="Label3" runat="server" meta:resourcekey="lblconfirmpwd" class="form-label fs-6 fw-boldest text-dark" for="email">Confirm Password<span class="allowempty">* </span></asp:Label>
                                                                    <%End If %>--%>
                                                                <%if (remark != "chpwd")
                                                                    { %>
                                                            </div>
                                                        </div>
                                                        <%} %>
                                                    </div>
                                                </div>

                                                <div class="form-actions d-flex flex-row flex-column-fluid my-1">
                                                    <div class="d-flex flex-row-fluid my-2">
                                                        <asp:Button runat="server" ID="btnSumit" OnClientClick="javascript:return ValidatePassword(this);"
                                                            Text="Save" class="divbutton btn btn-lg btn-primary mb-5 w-100" OnClick="btnSubmit_Click" title="Save" />
                                                        <asp:Button runat="server" Text="OTP Password" title="OTP Password" TabIndex="6" ID="btnOTPpwdChange" class="d-none" OnClick="btnOTPpwdChange_Click" />
                                                        <asp:Button runat="server" Text="Resend OTP" title="PWD OTP" TabIndex="6" ID="btnResendOtp" class="d-none" OnClick="btnResendOtp_Click" />
                                                        <% if (remark == "chpwd")
                                                            {%>
                                                        <button type="button" class="btn btn-text-primary btn-active-light-primary btn-lg mb-5 w-100" onclick="parent.closeModalDialog()" id="btnClose" title="Cancel">Cancel</button>
                                                        <%} %>
                                                    </div>
                                                </div>
                                                <% if (remark != "chpwd")
                                                    {%>
                                                <div class="copyrightlabs">
                                                    <div class="col-lg-12">
                                                        <span><span id="axpertVer" class="fw-bolder text-dark mb-2 float-end" runat="server"></span></span>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                </div>
                                                <%} %>
                                            </div>
                                        </div>
                                        <%if (remark != "chpwd")
                                            {%>
                                    </div>
                                </div>
                                <%}%>

                                <asp:Literal ID="panelOtp" runat="server"></asp:Literal>
                            </div>
                    </div>
                    <div id="wBdr" class="wBdr">
                        <div id="tabbody1" class="dcContent" runat="server">
                            <div id="tab-body-new:1" class="dcContent">
                                <div id="1">
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--end-->
                    <%if (Request.Browser.Browser == "Firefox")
                        {%>
                    <script type="text/javascript">        b();</script>
                    <%}%>
                            </form>
                </div>
            </div>
            <%if (remark != "chpwd")
                { %>
        </div>
    </div>
    </div>
    </div>
    <%} %>
</body>
</html>
