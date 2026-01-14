<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cancel.aspx.cs" Inherits="cancel" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Cancel Page" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Cancel Record</title>
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
    <link rel="Stylesheet" href="../Css/generic.min.css?v=10" type="text/css" id="generic" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script type="text/javascript" src="../Js/helper.min.js?v=172"></script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <link href="../Css/alerts.min.css" rel="stylesheet" />
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script type="text/javascript" src="../Js/tstruct.min.js?v=673"></script>
    <script type="text/javascript">

        var traceSplitChar = "♦";
        var traceSplitStr = "♦♦";
        var ErrLength = 7;
        var ErrStr = "<error>";
        maxLen = 150; // max number of characters allowed in the textbox
    </script>
    <script src="../Js/cancel.min.js?v=1" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
</head>
<body onload="GetFormDetails();">
    <form id="f1" runat="server" dir="<%=direction%>">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Path="../Js/tstruct.min.js?v=673" />
                </Scripts>
                <Services>
                    <asp:ServiceReference Path="../WebService.asmx" />
                </Services>
            </asp:ScriptManager>
        </div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblremarks" runat="server" meta:resourcekey="lblremarks" class="remarks">Remarks</asp:Label>
                    <input type="hidden" name="recordid000F0" value="0" />
                </td>
            </tr>
            <tr>
                <td>
                    <textarea rows="5" cols="60" id="comments" onkeyup="CheckMaxInput(this.form)" onblur="this.form.comments.value=FilterHTML(this.form.comments.value)"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblcharlft" runat="server" meta:resourcekey="lblcharlft" class="canTxt">Characters Left : </asp:Label>
                    <input type="text" name="remLen" class="remLen" disabled value="150" />
                    <input type="button" class="hotbtn btn" value="Cancel Record" onclick='javascript: CancelData();' />
                </td>
            </tr>
        </table>
    </form>
    <%=transDetails%>
</body>
</html>
