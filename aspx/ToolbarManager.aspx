<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ToolbarManager.aspx.cs" Inherits="aspx_ToolbarManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="ToolbaarManger" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Toolbaar Manger</title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render("~/Css/ToolbarManager") %>
    </asp:PlaceHolder>
    <% if (direction == "rtl")
        { %>
    <link rel="stylesheet" href="../ThirdParty/bootstrap_rtl.min.css" type="text/css" />
    <% } %>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
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
        var toolbarData = '<%=toolbarData%>';
        var structType = '<%=structType%>';
        var structName = '<%=structName%>';
    </script>
</head>
<body class="Mainbody Family btextDir-<%=direction%>" dir="<%=direction%>">
    <form id="form1" runat="server">
        <asp:PlaceHolder runat="server">
            <%:Scripts.Render("~/Js/ToolbarManager") %>
        </asp:PlaceHolder>
        <div>
            <div id="dvToolbar">
                 <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <ajaxToolkit:AsyncFileUpload runat="server" CssClass="hide" ID="uploadIcon" ClientIDMode="AutoID" OnUploadedComplete="uploadIcon_UploadedComplete" OnClientUploadStarted="validateFile" OnClientUploadComplete="SetNodeIcon" onclientuploaderror="uploadError"></ajaxToolkit:AsyncFileUpload>
             
                <ul>
                    <li>
                        <a href="#" id="AddGrp" text="Add group" runat="server" class="coldbtn">Add group</a>
                    </li>
                    <li>
                        <a href="#" id="btnAdd" text="Add button" data-value="Add" runat="server" class="coldbtn">Add button</a>
                    </li>
                    <li>
                        <a href="#" id="btnEdit" text="Delete" data-value="Edit" runat="server" class="coldbtn">Edit</a>
                    </li>
                    <li>
                        <a href="#" id="btnDelete" text="Delete" runat="server" class="coldbtn">Delete</a>
                    </li>
                    <li>
                        <a href="#" id="chngicon" text="change icon"  runat="server" class="coldbtn">change icon</a>
                    </li>
                    <li>
                        <a href="#" id="btnSave" text="save" runat="server" class="coldbtn">Save</a>
                    </li>
                </ul>
            </div>
            <div id="buttonTree"></div>

        </div>
         <asp:HiddenField runat="server" ID="hdnUserIconList" />
            <asp:HiddenField runat="server" ID="hdnIconPath" />
    </form>
</body>
</html>
