<%@ Page Language="VB" AutoEventWireup="false" CodeFile="srchComponent.aspx.vb" Inherits="srchComponent" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert Search Component" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>

    <title>Advanced Search</title>
    <link href="../Css/thirdparty/bootstrap/3.3.6/bootstrap.min.css" rel="stylesheet" />
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
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <%--custom alerts start--%>
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/thirdparty/bootstrap/3.3.6/bootstrap.min.js" type="text/javascript"></script>
    <link id="themecss" type="text/css" rel="Stylesheet" />
    <link href="../Css/GridTable.min.css?v=1" rel="stylesheet" />
    <script type="text/javascript" src="../Js/tstruct.min.js?v=673"></script>
    <script type="text/javascript" src="../Js/helper.min.js?v=172"></script>
    <script type="text/javascript" src="../Js/jsclient.min.js?v=118"></script>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>

    <script type="text/javascript">
        $j(document).ready(function () {
            ShowParentDimmer(false);
            // ChangeTheme(window);

        });
        var IsFormDirty = false;
        function ShowParentDimmer(status) {
            dv = $j("#waitDiv", window.opener.document);
            dv.hide();
            window.opener.document.onkeydown = function EatKeyPress() { if (DimmerCalled == true) { return true; } }
        }

        function ChangeDir(dir) {
            $j("#form1").attr("dir", dir);
        }

    </script>
    <style type="text/css">
        @media(max-width:516px) {
            #srchComponentTd .withWrapper {
                display: block;
                margin-top: 10px;
                margin-bottom: 10px;
            }

                #srchComponentTd .withWrapper label {
                    margin-right: 38px;
                }

                #srchComponentTd .withWrapper input {
                    width: 196px !important;
                }
        }


        @media(max-width:767px) {
            #searchoverlay table {
                width: 100%;
            }
        }

        #srchComponentTd .form-control {
            display: inline-block;
        }

        #waitDiv {
            height: DYNAMIC;
            left: 0px;
            position: absolute;
            top: 0px;
            visibility: visible;
            width: DYNAMIC;
            z-index: 100000;
        }

        #backgroundDiv {
            position: fixed;
            top: 0px;
            bottom: 0px;
            left: 0px;
            right: 0px;
            overflow: hidden;
            padding: 0;
            margin: 0;
            background-color: #d3d3d3;
            filter: alpha(opacity=50);
            /*opacity: 1;*/
            z-index: 999999;
        }
    </style>
</head>
<body onload="ChangeDir('<%=direction%>');">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="../Js/tstruct.min.js?v=673" />
            </Scripts>
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div style="max-height: 600px;">
            <asp:HiddenField ID="fname" runat="server" />
            <div runat="server" id="searchoverlay" class="overlay">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="margin-top: 10px;" class="col-lg-12 col-sm-12 col-md-12">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td id="srchComponentTd">
                                    <div class="col-lg-5 col-sm-5 col-md-5">
                                        <% If Session("language").ToString() = "ARABIC" Then%>
                                        <asp:Label ID="lblwith" runat="server" meta:resourcekey="lblwith">
                                                    With</asp:Label>
                                        <%Else%>
                                        <asp:Label ID="lblsrchfor" runat="server" meta:resourcekey="lblsrchfor">
                                                    Search For</asp:Label>
                                        <%End If%>
                                        <asp:DropDownList ID="s1" CssClass="combotem Family form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-5 col-sm-5 col-md-5">
                                        <% If Session("language").ToString() = "ARABIC" Then%>
                                        <label>
                                            <asp:Label ID="lblsrch" runat="server" meta:resourcekey="lblsrch">
                                                        Search For</asp:Label>
                                        </label>
                                        <%Else%>

                                        <asp:Label ID="lblwth" runat="server" meta:resourcekey="lblwth">
                                                    With</asp:Label>
                                        <%End If%>

                                        <asp:TextBox ID="txtSearch" CssClass="tem Family form-control" runat="server" Text=""></asp:TextBox>

                                    </div>
                                    <asp:HiddenField ID="goval" runat="server" Value=""></asp:HiddenField>
                                    <div class="col-lg-2 col-sm-2 col-md-2">
                                        <label style="display: block; height: 20px;"></label>
                                        <asp:Button ID="btnsearch" CssClass="btn hotbtn" Text="Go" runat="server" />
                                    </div>
                                    <span style="display: none">
                                        <asp:ListBox ID="searchlist" Visible="true" CssClass=" form-control" runat="server" Height="0px" Width="0px"
                                            AutoPostBack="True"></asp:ListBox>
                                        <asp:ListBox ID="searchlistval" Visible="true" CssClass=" form-control" runat="server" Height="0px" Width="0px"
                                            AutoPostBack="True"></asp:ListBox>
                                        <asp:ListBox ID="lstValues" Visible="true" CssClass=" form-control" runat="server" Height="0px" Width="0px"
                                            AutoPostBack="True"></asp:ListBox>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ScrollBars="Auto" runat="server" ID="Panel1" Width="100%">
                            <div style="height: 5px;">
                            </div>
                            <div runat="server" id="dvSrchGrid">
                                <asp:GridView CellSpacing="-1" ID="GridView1" runat="server" CellPadding="2" GridLines="Vertical" Style="margin-top: 10px;"
                                    Width="100%" AllowSorting="false" RowStyle-Wrap="false" AutoGenerateColumns="false"
                                    PageSize="10" CssClass="gridData customSetupTableMN">
                                    <HeaderStyle />
                                    <AlternatingRowStyle />
                                </asp:GridView>
                            </div>
                            <table>
                                <tr>
                                    <td style="width: 20%">&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <div runat="server" id="dvMsg">
                                            <asp:Label ID="records" runat="server" Text="" CssClass="totrec"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <span style="display: none">
                                            <asp:TextBox ID="pgno" runat="server" Text="0" Visible="true" BorderStyle="None"
                                                ForeColor="white" Width="1" BackColor="white"></asp:TextBox>
                                        </span>
                                            <asp:Label ID="pgCap" meta:resourcekey="pgCap" Text="Page No. " Visible="false" runat="server" CssClass="totrec"></asp:Label>
                                            <asp:DropDownList ID="lvPage" runat="server" AutoPostBack="true" Visible="false"
                                                Width="40px">
                                            </asp:DropDownList>
                                            <asp:Label ID="pages" Text="" runat="server" CssClass="totrec" Visible="false"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <div id='waitDiv' style=''>
                            <div id='backgroundDiv' style='background: url(../Axpimages/loadingBars.gif) center center no-repeat rgba(255, 255, 255, 0.4); background-size: 135px;'>
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </div>
        <asp:Label ID="lblNodata" runat="server" meta:resourcekey="lblNodata" Visible="false">No data found.</asp:Label>
    </form>
    <script type="text/javascript">

        function loadParent(a) {
            var gov = document.getElementById("<%=searchlist.ClientID%>");
            var grviewpno = document.getElementById("<%=pgno.ClientID%>");
            var pno = parseInt(grviewpno.value)
            gov.selectedIndex = (pno * 10) + parseInt(a);
            var govlistval = "<%=searchlistval.ClientID%>";
            SetSelectedValue(gov, govlistval);

        }
        function LoadWorkflow(a) {
            var gov = document.getElementById("<%=searchlist.ClientID%>");
            var lstVal = document.getElementById('<%=lstValues.ClientID %>');
            var grviewpno = document.getElementById("<%=pgno.ClientID%>");
            var pno = parseInt(grviewpno.value)
            gov.selectedIndex = (pno * 10) + parseInt(a);
            var govlistval = "<%=searchlistval.ClientID%>";
            if (gov[gov.selectedIndex].value != -1 && lstVal[gov.selectedIndex].value != -1) {
                if (document.getElementById("fname")) {
                    var fname = document.getElementById("fname").value;
                    var ids = fname.split("~");
                    var result = gov[gov.selectedIndex].value;
                    var result1 = lstVal[gov.selectedIndex].value;
                    eval('window.opener.document.form1.' + ids[0] + '.value ="' + result + '";');
                    eval('window.opener.document.form1.' + ids[1] + '.value ="' + result1 + '";');
                }
            }
            window.close();
        }

        var ChangedFields = new Array();
        var ChangedFieldValues = new Array();
        var DeletedDCRows = new Array();
        var fldNewNameArr = new Array();
        var fldNewDbRowNo = new Array();
        var fldNewValueArr = new Array();
    </script>
    <script type="text/javascript">
        //  Fix: UpdatePanel Async Postbacks Slow - Fix from Microsoft for KB 2000262. 
        function disposeTree(sender, args) {
            var elements = args.get_panelsUpdating();
            for (var i = elements.length - 1; i >= 0; i--) {
                var element = elements[i];
                var allnodes = element.getElementsByTagName('*'),
                    length = allnodes.length;
                var nodes = new Array(length)
                for (var k = 0; k < length; k++) {
                    nodes[k] = allnodes[k];
                }
                for (var j = 0, l = nodes.length; j < l; j++) {
                    var node = nodes[j];
                    if (node.nodeType === 1) {
                        if (node.dispose && typeof (node.dispose) === "function") {
                            node.dispose();
                        }
                        else if (node.control && typeof (node.control.dispose) === "function") {
                            node.control.dispose();
                        }

                        var behaviors = node._behaviors;
                        if (behaviors) {
                            behaviors = Array.apply(null, behaviors);
                            for (var k = behaviors.length - 1; k >= 0; k--) {
                                behaviors[k].dispose();
                            }
                        }
                    }
                }
                element.innerHTML = "";
            }
        }


        Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(disposeTree);

    </script>
    <script src="../Js/common.min.js?v=158"></script>
</body>
</html>
