<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="aspx_ResetPassword" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Reset PAssword"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <link href="../Css/thirdparty/jquery-ui/1.12.1/jquery-ui.min.css" rel="stylesheet" />
    <link href="../Css/thirdparty/jquery-ui/1.12.1/jquery-ui.structure.min.css" rel="stylesheet" />
    <link href="../Css/thirdparty/jquery-ui/1.12.1/jquery-ui.theme.min.css" rel="stylesheet" />
    <link href="../Css/ResetPassword.min.css" rel="stylesheet" />
    <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" />
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
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <%--custom alerts start--%>
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/thirdparty/jquery-ui/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>

    <script src="../Js/md5.min.js" language="javascript" type="text/javascript"></script>

    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>

    <script src="../Js/user.min.js?v=20" type="text/javascript"></script>

    <link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" id="generic" />
    <link id="themecss" type="text/css" rel="Stylesheet" href="" />
    <script src="../Js/Resetpassword.min.js?v=3" type="text/javascript"></script>

    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <title>Reset Password</title>
</head>
<body class="dc" dir="<%=direction%>">

    <div>
        <form name="form1" runat="server" id="form2">
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </div>
            <div class="dcTitle">
                <center>
                    <asp:Label ID="lblAction" runat="server" meta:resourcekey="lblAction">Reset Password</asp:Label></center>
            </div>
            <div runat="server" class="tblMargin">
                <br />
                <div>
                    <table style="width: 350px">
                        <tr>
                            <td>
                                <asp:Label ID="lbluname" runat="server" meta:resourcekey="lbluname">User Name</asp:Label><font class="allowempty">* </font>
                            </td>
                            <td>
                                <asp:TextBox runat="server" placeholder="search user to reset password.." Style="width: 200px; height: 26px;" ID="userName"></asp:TextBox>
                                <img src="../AxpImages/icons/search-back.jpg" style="vertical-align: middle;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblpwd" runat="server" meta:resourcekey="lblpwd">Password</asp:Label><font class="allowempty">* </font>
                            </td>
                            <td>
                                <asp:TextBox TextMode="Password" runat="server" onblur="md5auth1(this);" Style="width: 200px; height: 26px;" name="np" autocomplete="off" ID="newPwd"></asp:TextBox>
                                <input runat="server" type="hidden" id="pwd000F0" name="pwd000F0" value="" />
                                <input runat="server" type="hidden" id="pwdlength" name="pwdlength" value="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcpwd" runat="server" meta:resourcekey="lblcpwd">Confirm Password</asp:Label><font class="allowempty">* </font>
                            </td>
                            <td>
                                <asp:TextBox TextMode="Password" runat="server" onblur="md5auth1(this);" Style="width: 200px; height: 26px;" name="cp" autocomplete="off" ID="confirmPwd"></asp:TextBox>
                                <input runat="server" type="hidden" name="cpwd000F0" id="cpwd000F0" value="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblmail" runat="server" meta:resourcekey="lblmail">Mail Id</asp:Label><font class="allowempty">* </font>
                            </td>
                            <td>
                                <asp:TextBox runat="server" autocomplete="off" ID="mailId" Style="width: 200px; height: 26px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblError" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                                <center>
                                    <asp:Button ID="save" class="hotbtn btn" Text="Save" OnClientClick=""
                                        runat="server" OnClick="save_Click" />
                                    <asp:Button ID="Clear" class="coldbtn btn" Text="Clear" runat="server"
                                        OnClientClick="" OnClick="Clear_Click" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <%=strFldArrays %>
        </form>
    </div>

</body>
</html>
