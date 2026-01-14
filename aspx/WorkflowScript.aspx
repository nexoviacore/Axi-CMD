<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowScript.aspx.cs" Inherits="_WorkFlowScript" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Axpert Tstruct"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <title>Workflow Scripts</title>
    <link rel="Stylesheet" href="../Css/generic.min.css?v=10" type="text/css" id="generic" />
    <link id="themecss" type="text/css" rel="Stylesheet" href="" />
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
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/WF.min.js?v=5" type="text/javascript"></script>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <link rel="Stylesheet" href="../Css/WF.min.css?v=3" type="text/css" />
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
    <script src="../Js/WorkFlowScript.min.js?v=3" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <style type="text/css">
        .style3 {
            width: 395px;
        }
    </style>
</head>
<!-- <body onload="ChangeTheme();adjustwin(window);" style="margin: 0px 2px 0px 2px; height: auto;" -->
<body onload="" style="margin: 0px 2px 0px 2px; height: auto;"
    class="Family">
    <form id="form1" runat="server" style="height: auto;">
        <div style="overflow-x: auto; overflow-y: hidden;">
            <table align="center" width="100%">
                <tr>
                    <td style="width: 100%">
                        <asp:Menu ID="menuTabs" runat="server" CssClass="menuTabs" OnMenuItemClick="menuTabs_MenuItemClick"
                            Orientation="Horizontal" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="SelectedMenuTab"
                            StaticSubMenuIndent="36px">
                            <Items>
                                <asp:MenuItem Selected="True" Text="Create WorkFlow" Value="0"></asp:MenuItem>
                                <asp:MenuItem Text="Attach to Transaction" Value="4"></asp:MenuItem>
                                <asp:MenuItem Text="Workflow Delegation" Value="5"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                        <div class="tabBody" style="height: auto;">
                            <asp:MultiView ID="mvwTabs" runat="server">
                                <asp:View ID="vwWFcreate" runat="server">
                                    <asp:Panel GroupingText="WorkFlow" CssClass="td" ID="pnlWF" runat="server">
                                        <table width="100%" align="center">
                                            <tr class="td">
                                                <td align="right" style="width: 25%">
                                                    <asp:Label ID="lblWorkFlow" runat="server" meta:resourcekey="lblWorkFlow" Text="WorkFlow :"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="true" ID="ddlWorkFlowList" runat="server" Width="225px"
                                                        OnSelectedIndexChanged="ddlWorkFlowList_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="TxtWorkFlow" runat="server" Visible="False" Width="175px"></asp:TextBox>
                                                </td>
                                                <td align="left" style="width: 25%">
                                                    <asp:Button ID="btnNewWF" runat="server" Text="New" Width="60px" OnClick="btnNewWF_Click" />
                                                    &nbsp;
                                                <asp:Button ID="btnDeleteWF" Enabled="false" runat="server" Text="Delete" Width="60px"
                                                    OnClick="btnDeleteWF_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="font-size: small; font-weight: normal; font-style: normal"></td>
                                                <td class="style3">
                                                    <asp:DropDownList ID="ddlwfnamelist" Visible="false" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel GroupingText="Roles" CssClass="td" ID="pnlRoles" runat="server">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="left">&nbsp;<asp:Button ID="btnAddRow" runat="server" OnClick="btnAddRow_Click" Text=" +"
                                                    Width="50px" Visible="False" Font-Bold="True" Font-Size="Medium" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top">
                                                    <asp:GridView CellSpacing="-1" ID="gvWorkFlow" runat="server" AutoGenerateColumns="False" BorderColor="Gray"
                                                        CssClass="Grid" BorderStyle="Solid" BorderWidth="1px" GridLines="None" Height="16px"
                                                        OnRowCancelingEdit="gvWorkFlow_RowCancelingEdit" OnRowCommand="gvWorkFlow_RowCommand"
                                                        OnRowDataBound="gvWorkFlow_RowDataBound" OnRowDeleted="gvWorkFlow_RowDeleted"
                                                        OnRowDeleting="gvWorkFlow_RowDeleting" OnRowEditing="gvWorkFlow_RowEditing" OnRowUpdating="gvWorkFlow_RowUpdating"
                                                        Width="100%" OnRowCreated="gvWorkFlow_RowCreated">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Role">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top"
                                                                    Width="30%" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRole" runat="server" Text='<%# Bind("WFname")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:ListBox SelectionMode="Multiple" ID="lstRoleEdit" runat="server"></asp:ListBox>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="IsMandatory" ShowHeader="False">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMandatory" runat="server" Enabled="false" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="chkMandatory" runat="server" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approve" ShowHeader="False">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkApprove" runat="server" Enabled="false" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="chkApprove" runat="server" Enabled="true" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Reject" ShowHeader="False">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkReject" runat="server" Enabled="false" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="chkReject" runat="server" Enabled="true" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Forward" ShowHeader="False">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkReview" runat="server" Enabled="false" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="chkReview" runat="server" Enabled="true" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Return" ShowHeader="False">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkReturn" runat="server" Enabled="false" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="chkReturn" runat="server" Enabled="true" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Max Days" ShowHeader="False">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMaxDays" runat="server" Text='<%# Bind("WFdays")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TxtDays" MaxLength="2" onkeypress="javascript:return CheckNumeric(event,this.value);" Text='<%# Bind("WFdays")%>' runat="server" Width="64px"></asp:TextBox>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Max Hours" ShowHeader="False">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMaxHours" runat="server" Text='<%# Bind("WFhrs")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TxtHrs" Text='<%# Bind("WFhrs")%>' MaxLength="2" onkeypress="javascript:return CheckNumeric(event,this.value);" runat="server" Width="64px"></asp:TextBox>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Identification" ShowHeader="False">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIdentify" runat="server" Text='<%# Bind("identify")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:ListBox SelectionMode="Multiple" ID="lstIdentify" runat="server"></asp:ListBox>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action" ShowHeader="False">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAction" runat="server" Text='<%# Bind("WFAction")%>' Enabled="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlAction" runat="server" AutoPostBack="true" Enabled="true">
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField HeaderStyle-BorderColor="Black" ItemStyle-BorderColor="Black" ItemStyle-BorderStyle="Solid"
                                                                ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
                                                                ItemStyle-Width="5%" ShowEditButton="True">
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                            </asp:CommandField>
                                                            <asp:CommandField HeaderStyle-BorderColor="Black" ItemStyle-BorderColor="Black" ItemStyle-BorderStyle="Solid"
                                                                ItemStyle-BorderWidth="1px" ItemStyle-VerticalAlign="Top" ItemStyle-Width="5%"
                                                                ShowCancelButton="False" ShowDeleteButton="True">
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Insert Rows" ShowHeader="True">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" Width="8%" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="8%" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkinsert" runat="server" CommandName="Insert" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                        Text="Insert"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="GridHead" Height="25px" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle BackColor="Transparent" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <table width="100%">
                                        <tr>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td valign="bottom" align="center">
                                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click"
                                                    Style="height: 26px;" />
                                                &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                                &nbsp;<asp:Button ID="btnNotify" runat="server" Visible="false" Text="Notification"
                                                    OnClick="btnNotify_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel Visible="false" GroupingText="Notification" CssClass="td" ID="pnlNotify"
                                        runat="server">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" valign="top" class="td">
                                                    <asp:GridView CellSpacing="-1" ID="gvUserNotify" runat="server" AutoGenerateColumns="False" BorderColor="Gray"
                                                        CssClass="Grid" BorderStyle="Solid" BorderWidth="1px" Visible="false" GridLines="None"
                                                        Height="16px" Width="100%" OnRowCommand="gvUserNotify_RowCommand" OnRowDataBound="gvUserNotify_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Role">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbllevel" runat="server" Text='<%# Bind("name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="On Approve">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkApprove" runat="server" CommandName="Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="On Reject">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkReject" runat="server" CommandName="Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="On Review">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkReview" runat="server" CommandName="Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="On Return">
                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkReturn" runat="server" CommandName="Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="GridHead" Height="25px" HorizontalAlign="Center" />
                                                        <FooterStyle CssClass="GridHead" Height="25px" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle BackColor="Transparent" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:View>
                                <asp:View ID="vwAdd" runat="server">
                                    <asp:Panel GroupingText="Role Details" CssClass="td" ID="pnlRoleDetails" runat="server">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblrole" runat="server" meta:resourcekey="lblrole">Role :</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="ddlRole" runat="server" Height="100px" Width="175px" SelectionMode="Multiple"
                                                        onchange="javascript:CheckMandatory(this)"></asp:ListBox>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblapprove" runat="server" meta:resourcekey="lblapprove">Is Approval Mandatory :</asp:Label></td>
                                                <td>
                                                    <asp:CheckBox ID="ChkMandatory" Checked='<%# Bind("WFMandatory")%>' runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td align="right"></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblrights" runat="server" meta:resourcekey="lblrights">Rights :</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBoxList ID="ChkListRights" runat="server" CellSpacing="5" Height="25px"
                                                        RepeatDirection="Horizontal" Width="135px">
                                                        <asp:ListItem>Approve</asp:ListItem>
                                                        <asp:ListItem>Reject</asp:ListItem>
                                                        <asp:ListItem>Forward</asp:ListItem>
                                                        <asp:ListItem>Return</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td align="right"></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblidentity" runat="server" meta:resourcekey="lblidentity">Identification :</asp:Label>
                                                </td>
                                                <td align="left" valign="middle">
                                                    <asp:ListBox SelectionMode="Multiple" ID="lstidentification" runat="server" Width="135px"
                                                        Height="50px"></asp:ListBox>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblmaxtime" runat="server" meta:resourcekey="lblmaxtime">Max Time for Response :</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TxtDays" MaxLength="2" runat="server" Width="40px" onblur="javascript:CheckAction();"></asp:TextBox>
                                                    &nbsp;Days&nbsp;&nbsp;&nbsp;
                                                <asp:TextBox ID="TxtHrs" runat="server" Width="40px" MaxLength="2" onblur="javascript:CheckAction();"></asp:TextBox>
                                                    &nbsp;Hours&nbsp;&nbsp;&nbsp; Action
                                                <asp:DropDownList Width="200px" ID="ddlActions" runat="server" Enabled="false">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem>Approve</asp:ListItem>
                                                    <asp:ListItem>Forward</asp:ListItem>
                                                    <asp:ListItem>Reject</asp:ListItem>
                                                    <asp:ListItem>Return</asp:ListItem>
                                                </asp:DropDownList>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">&nbsp;
                                                </td>
                                                <td align="left">&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right"></td>
                                                <td align="left">
                                                    <asp:Button ID="btnCreateWF" runat="server" OnClick="Button1_Click" Text="OK" Width="60px" />
                                                    &nbsp;
                                                <asp:Button ID="btnCancelWF" runat="server" Text="Cancel" Width="60px" OnClick="btnCancelWF_Click" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:View>
                                <asp:View ID="vwAttachTransaction" runat="server">
                                    <table width="100%" align="center">
                                        <tr>
                                            <td align="center" class="td">
                                                <asp:Panel GroupingText="Transaction" ID="pnlTransaction" runat="server">
                                                    <table style="border-style: none; border-color: inherit; border-width: medium; margin-bottom: 0px;"
                                                        align="center" width="100%">
                                                        <%--<tr>
                                                        <td align="right" class="td" style="width: 25%">
                                                            Module :
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlModules" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"
                                                                Style="margin-left: 0px" Width="300px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>--%>
                                                        <tr>
                                                            <td align="right" class="td" style="width: 25%">
                                                                <asp:Label ID="lbltransact" runat="server" meta:resourcekey="lbltransact">Transaction :</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlTransaction" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTransaction_SelectedIndexChanged"
                                                                    Style="margin-left: 0px" Width="300px">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlTransactionId" Visible="false" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" class="td">
                                                <asp:Panel GroupingText="Condition" ID="pnlCondition" runat="server">
                                                    <table width="100%" class="td" align="center">
                                                        <tr>
                                                            <td align="left" style="width: 25%">Field
                                                            </td>
                                                            <td align="left" style="width: 25%">Operator
                                                            </td>
                                                            <td align="left" style="width: 25%">Value1
                                                            </td>
                                                            <td align="left" style="width: 25%">Value2
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 25%">
                                                                <asp:DropDownList Width="175px" ID="ddlField" runat="server">
                                                                    <asp:ListItem></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:DropDownList Width="175px" ID="ddlOperator" runat="server">
                                                                    <asp:ListItem></asp:ListItem>
                                                                    <asp:ListItem>Equal_To</asp:ListItem>
                                                                    <asp:ListItem>Not_Equal_To</asp:ListItem>
                                                                    <asp:ListItem>Less_than</asp:ListItem>
                                                                    <asp:ListItem>Less_than_or_Equal_To</asp:ListItem>
                                                                    <asp:ListItem>Greater_than</asp:ListItem>
                                                                    <asp:ListItem>Greater_than_or_Equal_To</asp:ListItem>
                                                                    <asp:ListItem>Between</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:TextBox Width="175px" ID="ddlValue1" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:TextBox Width="175px" ID="ddlValue2" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" class="td">
                                                <asp:Panel GroupingText="WorkFlow" ID="pnlWorkflow" runat="server">
                                                    <table width="100%" align="center">
                                                        <tr>
                                                            <td align="right" style="width: 25%" class="td">WorkFlow :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList Width="175px" ID="ddlWorkFlow" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlWorkFlow_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left"></td>
                                                            <td align="left"></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel Visible="false" GroupingText="Details" ID="pnlwfdetails" runat="server">
                                                    <table width="100%" align="center">
                                                        <tr>
                                                            <td align="center" class="td">
                                                                <asp:GridView CellSpacing="-1" ID="gvWorkFlowAttach" runat="server" AutoGenerateColumns="False" BorderColor="Gray"
                                                                    CssClass="Grid" BorderStyle="Solid" BorderWidth="1px" GridLines="None" Height="16px"
                                                                    Width="100%" OnRowDataBound="gvWorkFlowAttach_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Role">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top"
                                                                                Width="30%" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRole" runat="server" Text='<%# Bind("WFname")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="IsMandatory" ShowHeader="False">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkMandatory" runat="server" Enabled="false" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Approve" ShowHeader="False">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkApprove" runat="server" Enabled="false" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Reject" ShowHeader="False">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkReject" runat="server" Enabled="false" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Review" ShowHeader="False">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkReview" runat="server" Enabled="false" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Return" ShowHeader="False">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkReturn" runat="server" Enabled="false" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Max Days" ShowHeader="False">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblMaxDays" runat="server" Text='<%# Bind("WFdays")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Max Hours" ShowHeader="False">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblMaxHours" runat="server" Text='<%# Bind("WFhrs")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Identification" ShowHeader="False">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblIdentify" runat="server" Text='<%# Bind("identify")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action" ShowHeader="False">
                                                                            <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                            <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAction" runat="server" Text='<%# Bind("WFAction") %>' Enabled="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="GridHead" ForeColor="White" Height="25px" HorizontalAlign="Center" />
                                                                    <FooterStyle CssClass="GridHead" Height="25px" HorizontalAlign="Center" />
                                                                    <AlternatingRowStyle BackColor="Transparent" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnAttach" runat="server" Text="Attach" Width="60px" OnClick="btnAttach_Click" />&nbsp;
                                            <asp:Button ID="btnCancelAttach" runat="server" Text="Cancel" Width="60px" OnClick="btnCancelAttach_Click" />&nbsp;
                                            <asp:Button ID="btnDetach" runat="server" Text="Detach" Width="60px" OnClick="btnDetach_Click"
                                                Visible="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="vwEnterNotify" runat="server">
                                    <table width="100%" align="center">
                                        <tr>
                                            <td></td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="font-size: small; font-weight: normal; font-style: normal">
                                                <asp:Label ID="lblEvent" runat="server" meta:resourcekey="lblEvent" Text="Event :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEvent" Enabled="false" runat="server" Width="188px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="font-size: small; font-weight: normal; font-style: normal">
                                                <asp:Label ID="lblTo" runat="server" meta:resourcekey="lblTo" Text="To :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBoxList ID="ChkListNotify" runat="server">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="font-size: small; font-weight: normal; font-style: normal">
                                                <asp:Label ID="lbltype" runat="server" meta:resourcekey="lbltype" Text="Send Type :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rbtnlistsend" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem>Internal</asp:ListItem>
                                                    <asp:ListItem>External</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="font-size: small; font-weight: normal; font-style: normal">
                                                <asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" Text="Email :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" Width="399px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="font-size: small; font-weight: normal; font-style: normal">
                                                <asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" Text="Subject :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSubject" runat="server" Width="399px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="font-size: small; font-weight: normal; font-style: normal">
                                                <asp:Label ID="lblAttach" runat="server" meta:resourcekey="lblAttach" Text="Attach :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="FileUpload1" runat="server" Width="395px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="font-size: small; font-weight: normal; font-style: normal">
                                                <asp:Label ID="lblBody" runat="server" meta:resourcekey="lblBody" Text="Body :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBody" TextMode="MultiLine" runat="server" Height="100px" Width="395px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="font-size: small; font-weight: normal; font-style: normal">&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="font-size: small; font-weight: normal; font-style: normal">&nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSaveNotify" runat="server" OnClick="btnSaveNotify_Click" Text="Save"
                                                    Width="60px" />
                                                &nbsp;<asp:Button ID="btnCancelNotify" runat="server" OnClick="btnCancelNotify_Click"
                                                    Text="Cancel" Width="60px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="vwAttachmulticond" runat="server">
                                    <table width="100%" align="center">
                                        <tr>
                                            <td align="center" class="td">
                                                <asp:Panel GroupingText="Transaction" ID="Panel1" runat="server">
                                                    <table style="border-style: none; border-color: inherit; border-width: medium; margin-bottom: 0px;"
                                                        align="center" width="100%">
                                                        <tr>
                                                            <td align="right" class="td" style="width: 25%">Module :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlModules" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"
                                                                    Style="margin-left: 0px" Width="300px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="td" style="width: 25%">Transaction :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlTransactionmc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTransactionmc_SelectedIndexChanged"
                                                                    Style="margin-left: 0px" Width="300px">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlTransactionmcId" Visible="false" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" class="td">
                                                <asp:Panel GroupingText="Rules" ID="Panel2" runat="server">
                                                    <table style="border-style: none; border-color: inherit; border-width: medium; margin-bottom: 0px;"
                                                        align="center" width="100%">
                                                        <tr>
                                                            <td align="left" class="td">
                                                                <textarea runat="server" id="mConditions" rows="15" cols="70">
                                                        </textarea>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <asp:Label ID="wfmConderr" runat="server" Text="" ForeColor="#DB2222" Font-Size="10px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnAttachmc" runat="server" Text="Attach" Width="60px" OnClick="btnAttachmc_Click" />&nbsp;
                                            <asp:Button ID="btnCancelAttachmc" runat="server" Text="Cancel" Width="60px" OnClick="btnCancelAttachmc_Click" />&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="Delegation" runat="server">
                                    <asp:Panel ID="pnlUser" runat="server" CssClass="td" GroupingText="From User">
                                        <table>
                                            <tr>
                                                <td class="style4">&nbsp;</td>
                                                <td class="style5">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style4">
                                                    <asp:Label ID="lblSrcUser" runat="server" meta:resourcekey="lblSrcUser" Text="User name"></asp:Label>
                                                </td>
                                                <td class="style5">
                                                    <input id="SrcUser000F0" maxlength="" name="SrcUser000F0"
                                                        onblur="javascript:SetSrcFromUserValue();" style="width: 197px; height: "
                                                        type="text" />&nbsp;<a>
                                                            <img id="img~SrcUser000F0" border="0"
                                                                onclick="javascript:OpenPickList('ddlParentGroup','ddlSrcBranchName','SrcUser000F0~hdnSrcEmpId');"
                                                                src="../AxpImages/icons/search-back.jpg" style="cursor: pointer;" />
                                                        </a>
                                                    <input id="hdnSrcUser" runat="server" type="hidden" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click"
                                                        OnClientClick="SetSrcFromUserValue();" Text="Get Tasks" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="pnlGrd" GroupingText="Tasks" CssClass="td" runat="server"
                                        Height="200px" ScrollBars="Auto">
                                        <asp:GridView CellSpacing="-1" ID="grdTasks" runat="server" AutoGenerateColumns="False" BorderColor="Gray"
                                            BorderStyle="Solid" BorderWidth="1px" GridLines="None" Height="16px" Width="100%" CssClass="Grid"
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
                                                <asp:TemplateField HeaderText="To" ShowHeader="False" Visible="false">
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
                                            <FooterStyle CssClass="GridHead" Height="25px" HorizontalAlign="Center" />
                                            <AlternatingRowStyle BackColor="Transparent" />
                                            <HeaderStyle CssClass="GridHead" Height="25px" HorizontalAlign="Center" />
                                        </asp:GridView>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="Panel3" runat="server" GroupingText="To User" CssClass="td">
                                        <table>
                                            <tr>
                                                <td class="style4">&nbsp;</td>
                                                <td class="style5">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style4">
                                                    <asp:Label ID="lblDesUser000F0" runat="server" meta:resourcekey="lblDesUser000F0" Text="User name"></asp:Label>
                                                </td>
                                                <td class="style5">
                                                    <input type="text" id="DesUser000F0" name="DesUser000F0" maxlength="" style="width: 197px; height: "
                                                        onblur="javascript:SetDesUserValue();" />&nbsp;<a><img src="../AxpImages/icons/search-back.jpg"
                                                            id="Img1" onclick="javascript:OpenPickList('ddlParentGroup','ddlDesBranchName','DesUser000F0~hdnDesEmpId');"
                                                            style="cursor: pointer;" border="0" /></a>
                                                    <input type="hidden" id="hdnDesUser" runat="server" />
                                                    <asp:CheckBox ID="chkFinalApp" Checked="true" runat="server" Text="Final Approval/Rejection" />

                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSubmitTask" runat="server" OnClick="btnSubmitTask_Click" OnClientClick="javascript:return ValidateDupTasks();"
                                                        Text="Assign" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <div style="width: 100%">
                                        <center>
                                        </center>
                                    </div>
                                    <div>
                                        <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                    <asp:HiddenField ID="hdnSelectedValues" runat="server" />
                                    <asp:HiddenField ID="hdnSrcEmpId" runat="server" />
                                    <asp:HiddenField ID="hdnDesEmpId" runat="server" />
                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
