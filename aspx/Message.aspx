<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Message.aspx.vb" Inherits="Message" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8"/>    
    <meta name="description" content="Messages"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>    
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>ToDo List</title>
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
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <link href="../Css/Icons/icon.css" rel="stylesheet" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />


    <%--custom alerts end--%>
    <script type="text/javascript">
        var enableBackButton = false;
        var enableForwardButton = false;
    </script>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <script src="../Js/thirdparty/bootstrap/3.3.6/bootstrap.min.js" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <link href="../AssetsNew/css/style.min.css?v=3" rel="stylesheet" />
    <link href="../Css/GridTable.min.css?v=1" rel="stylesheet" />
    <link href="../Css/TstructNew.min.css?v=91" rel="stylesheet" />
    <link href="../App_Themes/Gray/Stylesheet.min.css?v=23" rel="stylesheet" />
    <link id="themecss" type="text/css" href="" rel="stylesheet" />
    <script src="../Js/Message.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
</head>
<body onload="ChangeDir('<%=direction%>');">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Path="../Js/helper.min.js?v=172" />
                </Scripts>
                <Services>
                    <asp:ServiceReference Path="../WebService.asmx" />
                    <asp:ServiceReference Path="../CustomWebService.asmx" />
                </Services>
            </asp:ScriptManager>
            <div class="content">

                <% If lang = "ARABIC" Then%>
                <div id="backforwrdbuttons" class="hide backbutton" style="float: right;">
                    <%Else%>
                    <div id="backforwrdbuttons" class="hide backbutton" style="float: left; padding: 5px;">
                        <%End If%>
                        <span onclick="javascript:BackForwardButtonClicked('back');" class='navLeft icon-arrows-left-double-32 handCur' id="goback" style="border: 0px;" title="Click here to go back"></span>
                    </div>
                    <div id="breadcrumb-panel">
                        <div id='breadcrumb' class="icon-services">
                            <% If lang = "ARABIC" Then%>
                            <div class="icon-services bcrumb h3">
                                <asp:Label ID="lblmytask" runat="server" meta:resourcekey="lblmytask">My Tasks</asp:Label></div>
                            <%Else%>
                            <div class="icon-services bcrumb h3 left" style="margin-top: 0px; padding: 10px; padding-left: 40px; font-size: 18px; cursor: default !important">
                                <asp:Label runat="server" meta:resourcekey="lblmytask">My Tasks</asp:Label></div>
                            <%End If%>
                        </div>
                    </div>
                    <div id="searDiv" style="height: 100%; width: 100%;" runat="server">
                        <div class="form-group col-lg-5 col-sm-5 col-md-5">
                            <asp:Label CssClass="ToDoLblText Family" ID="sfor" runat="server" Text="Search for :"></asp:Label>
                            <asp:DropDownList ID="sf_Cb" runat="server" CssClass="form-control">
                                <asp:ListItem Value="From"></asp:ListItem>
                                <asp:ListItem Value="Message"></asp:ListItem>
                                <asp:ListItem Value="Date"></asp:ListItem>
                                <asp:ListItem Value="Duedate"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group col-lg-5 col-sm-5 col-md-5">
                            <asp:Label ID="swithl" runat="server" CssClass="ToDoLblText Family" Text="With"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="swith" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-12 col-sm-12 col-md-2 msg">
                            <label style="display: block; height: 13px;"></label>
                            <asp:Button ID="sbut" runat="server" Text="Search" CssClass="btn hotbtn" />
                            <asp:Button ID="clear" runat="server" Text="Clear" CssClass="coldbtn btn" />

                        </div>
                    </div>
                    <div class="dashboard column1" style="padding: 10px;">
                        <asp:Label ID="lblMsg" CssClass="lblMsg Family" Style="height: calc(100vh - 140px); display: table-cell; vertical-align: middle; width: 100vw; font-size: 26px; color: rgb(100, 100, 100); text-align: center; box-sizing: border-box;" runat="server" Visible="False"></asp:Label>
                        <asp:GridView CellSpacing="-1" ID="gvMessages" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="true" CssClass="gridData customSetupTableMN pointerclass" Width="100%">
                            <RowStyle Height="5px" />
                            <Columns>
                                <asp:TemplateField HeaderText="From">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFrom" runat="server" Text='<%# Bind("From")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Message" ShowHeader="True">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50%" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkMessage" runat="server" CausesValidation="False" CommandName="Select"
                                            CommandArgument='<%# Bind("link")%>' Text='<%# Bind("Message")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date" ShowHeader="False">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" Width="15%" />
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTime" runat="server" Text='<%# Bind("Time")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtHrs" runat="server" Width="64px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Due Date" ShowHeader="False">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" Width="15%" />
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDue" runat="server" Text='<%# Bind("Due")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle Height="25px" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="Transparent" />
                        </asp:GridView>
                    </div>
                    <%=tst_Scripts%><%=enableBackForwButton%>
            &nbsp;
                </div>
            </div>
            <asp:Label ID="lblNodata" runat="server" meta:resourcekey="lblNodata" Visible="false">No data found.</asp:Label>

            <div class="DivMessage">
            </div>
    </form>


</body>
</html>
