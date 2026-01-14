<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlowDelegation.aspx.cs"
    Inherits="Workflow" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Work flow delegation"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <title>WorkFlow Delegation</title>
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
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>

    <script src="../Js/WF.min.js?v=5" type="text/javascript"></script>

    <script src="../Js/helper.min.js?v=172" type="text/javascript"></script>

    <script src="../Js/dimmingdiv.min.js?v=2" type="text/javascript"></script>

    <script src="../Js/actb.min.js?v=1" type="text/javascript"></script>

    <link id="themecss" type="text/css" rel="Stylesheet" href="" />
    <link rel="Stylesheet" href="../Css/generic.min.css?v=10" type="text/css" id="generic" />

    <script type="text/javascript">
        var wfLevels = new Array();
        var wfNames = new Array();
        var wfDbNames = new Array();
        var fldTypes = new Array();
        var LvlDetails = new Array();

        var X = 0;
        var Y = 0;

        $j(document).ready(function () {
            load(); 
            // ChangeTheme(window); adjustwin(window);
        });

    </script>
    <script src="../Js/Workflowdelegation.min.js?v=1"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <link href="../Css/WorkflowDelegation.min.css" rel="stylesheet" />

</head>
<body dir="<%=direction%>">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Js/WF.min.js?v=5" />
            </Scripts>
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div class="tstructcontent">
            <%= strFieldTypes.ToString() %>
            <div id="breadcrumb-panel">
                <div id='breadcrumb' class='icon-services left'>
                    <div class='icon-services left bcrumb'>
                        Admin >
                    </div>
                    <div>
                        <h3>
                            <asp:Label ID="lblwrkdelegate" runat="server" meta:resourcekey="lblwrkdelegate">Workflow Delegation</asp:Label></h3>
                    </div>
                </div>
            </div>
            <div class="tabBody" style="height: auto;">
                <asp:MultiView ID="mvwTabs" runat="server">
                    <asp:View ID="Delegation" runat="server">
                        <asp:Panel ID="pnlUser" runat="server" GroupingText="From User" CssClass="td">
                            <table>
                                <tr>
                                    <td class="style4">
                                        <asp:Label ID="lblSipfUnits" runat="server" meta:resourcekey="lblSipfUnits" Text="SIPF Unit"></asp:Label>
                                    </td>
                                    <td class="style5">
                                        <asp:DropDownList ID="ddlUnits" runat="server" Width="197px">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        <asp:Label ID="lblUser" runat="server" meta:resourcekey="lblUser" Text="User name"></asp:Label>
                                    </td>
                                    <td class="style5">
                                        <input type="text" name="SrcUser" id="SrcUser" maxlength="" style="width: 197px; height: "
                                            disabled onblur="javascript:SetSrcUserValue();" />&nbsp;<a><img src="../AxpImages/icons/search-back.jpg"
                                                id="" onclick="javascript:OpenPickList('ddlUnits','SrcUser~hdnSrcEmpId');" style="cursor: pointer; cursor: hand;"
                                                border="0" /></a>
                                        <input type="hidden" id="txtSrcUser" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGo" Text="Get Tasks" runat="server" OnClick="btnGo_Click" OnClientClick="SetSrcUserValue();" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlGrd" GroupingText="Tasks" CssClass="td" runat="server" Visible="false"
                            Height="250px" ScrollBars="Auto">
                            <asp:GridView CellSpacing="-1" ID="grdTasks" runat="server" AutoGenerateColumns="False" BorderColor="Gray"
                                BorderStyle="Solid" BorderWidth="1px" GridLines="None" Height="16px" Width="100%"
                                OnRowDataBound="grdTasks_RowDataBound" AllowSorting="True" OnSorting="grdTasks_Sorting">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top"
                                            Width="5%" HorizontalAlign="Center" />
                                        <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                        <HeaderTemplate>
                                            <input id="chkAll" type="checkbox" onclick="javascript: CheckAll();" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="checkbox" name="chkSelect" onclick="javascript: GetSelectedRecordValues()" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From" SortExpression="fromwhom">
                                        <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top"
                                            Width="10%" />
                                        <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblfrom" runat="server" Text='<%# Bind("fromwhom")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To" SortExpression="towhom">
                                        <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top"
                                            Width="10%" />
                                        <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblto" runat="server" Text='<%# Bind("towhom")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Message" SortExpression="msg">
                                        <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top"
                                            Width="75%" />
                                        <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblmsg" runat="server" Text='<%# Bind("msg") %>' target="" Font-Underline="true"
                                                ForeColor="Black"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="sname" ShowHeader="False" Visible="false">
                                        <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                        <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblsname" runat="server" Text='<%# Bind("sname")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="rid" ShowHeader="False" Visible="false">
                                        <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                        <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblrid" runat="server" Text='<%# Bind("rid")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cap" ShowHeader="False" Visible="false">
                                        <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                        <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblcap" runat="server" Text='<%# Bind("cap")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="delg" ShowHeader="False" Visible="false">
                                        <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                        <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lbldelg" runat="server" Text='<%# Bind("delg")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#497D9E" ForeColor="White" Height="25px" HorizontalAlign="Center" />
                                <FooterStyle BackColor="#497D9E" Height="25px" HorizontalAlign="Center" />
                                <AlternatingRowStyle BackColor="Transparent" />
                            </asp:GridView>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="Panel1" runat="server" GroupingText="To User" CssClass="td">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblUnit" runat="server" meta:resourcekey="lblUnit" Text="SIPF Unit"></asp:Label>
                                    </td>
                                    <td class="style5">
                                        <asp:DropDownList ID="ddlDelUnit" runat="server" Width="197px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                        <asp:Label ID="lblDestUser" runat="server" meta:resourcekey="lblDestUser" Text="User name"></asp:Label>
                                    </td>
                                    <td class="style5">
                                        <input type="text" id="DestUser" name="DestUser" disabled maxlength="" style="width: 197px; height: "
                                            onblur="javascript:SetDesUserValue();" />&nbsp;<a><img src="../AxpImages/icons/search-back.jpg"
                                                id="Img1" onclick="javascript:OpenPickList('ddlDelUnit','DestUser~hdnDesEmpId');"
                                                style="cursor: pointer; cursor: hand;" border="0" /></a>
                                        <input type="hidden" id="hdnDestUser" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSubmitTask" runat="server" OnClick="btnSubmitTask_Click" OnClientClick="javascript:return ValidateDupTasks();"
                                            Text="Assign" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />

                        <div>
                            <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                        <asp:HiddenField ID="hdnSelectedValues" runat="server" />
                        <asp:HiddenField ID="hdnSrcEmpId" runat="server" />
                        <asp:HiddenField ID="hdnDesEmpId" runat="server" />
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
        <%=strWorkflow.ToString() %>
    </form>
</body>
</html>
