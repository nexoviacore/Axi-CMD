<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OpenLogFile.aspx.vb" Inherits="OpenLogFile" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Open log File" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />     
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>    
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Open Log File</title>
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
    <%If EnableOldTheme = "true" Then%>
    <link href="../Css/genericOld.min.css" rel="stylesheet" type="text/css" id="generic" />
    <%Else%>
    <link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" id="Link1" />
    <%End If%>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <link id="themecss" type="text/css" rel="Stylesheet" href="" />
    <script src="../Js/OpenLogFile.min.js?v=4" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            //for focusing tab within the page
            setTimeout(function () {
                var elemntsToCheck = 'button[tabindex!="-1"],a[tabindex!="-1"],input[tabindex!="-1"],select[tabindex!="-1"],radio[tabindex!="-1"]';
                var inputs = $('#form1').find(elemntsToCheck).filter(':visible').not(':disabled');
                firstInput = inputs.first();
                lastInput = inputs.last();
                firstInput.addClass("firstFocusable");
                lastInput.addClass("lastFocusable");
                $(".firstFocusable").focus();
                $(".lastFocusable").on('keydown.tabRot', function (e) {
                    if ((e.which === 9 && !e.shiftKey)) {
                        e.preventDefault();
                        $(".firstFocusable").focus();
                    }
                });
                $(".firstFocusable").on('keydown.tabRot', function (e) {
                    if ((e.which === 9 && e.shiftKey)) {
                        e.preventDefault();
                        $(".lastFocusable").focus();
                    }
                });
            },500)
        })
    </script>
</head>

<body class="Family">
    <form id="form1" runat="server" dir="<%=direction%>">
        <div class="tstruct-content">
            <asp:GridView CellSpacing="-1"  ID="gvLogfiles" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                Font-Size="Small" GridLines="Both" AllowSorting="true" CssClass="dataGrid" 
                Width="100%" style= "margin-top: 50px">
                <RowStyle Height="5px" />
                <Columns>
                    <asp:TemplateField HeaderText="Trace files" ShowHeader="True"
                        ControlStyle-Width="80%">
                        <ItemStyle Font-Size="Small" HorizontalAlign="Left" VerticalAlign="Middle" Width="100%" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkfile" runat="server" CausesValidation="False" CommandName="Select"
                                CommandArgument='<%# Bind("link")%>' Text='<%# Bind("FileName")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
           
        </div>
        <asp:Label ID="lblNodata" runat="server" meta:resourcekey="lblNodata" Visible="false">No data found.</asp:Label>
    </form>
</body>
</html>
