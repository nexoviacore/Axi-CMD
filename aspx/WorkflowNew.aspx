<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowNew.aspx.cs" Inherits="_WorkFlowNew" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert Workflow" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Workflow</title>
    <link rel="Stylesheet" href="../Css/generic.min.css?v=10" type="text/css" id="generic" />
    <link id="themecss" type="text/css" rel="Stylesheet" href="" />
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <%--custom alerts start--%>
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/dimmingdiv.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/WF.min.js?v=5" type="text/javascript"></script>

    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <script src="../Js/thirdparty/jquery-ui/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>

    <link href="../Css/WF.min.css?v=3" rel="stylesheet" type="text/css" />
    <%--<link href="../Css/workflowNew.min.css?v=10" rel="stylesheet" type="text/css" />--%>
    <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" type="text/css" />
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
    <script type="text/javascript">

        var fldTypes = new Array();
        var wfTransid = '<%=transid%>';
        var workflowId = '<%=workflowId%>';
    </script>
    <script src="../Js/WorkFlow.min.js?v=5" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>

    <style type="text/css">
        .style3 {
            width: 395px;
        }

        label, input[type="checkbox"] {
            cursor: pointer;
        }

        .chkdiv {
            margin-top: -18px;
            margin-left: 19px;
        }
    </style>
</head>
<body dir='<%=direction%>' onload="" style="margin: 0px 2px 0px 2px; height: auto;"
    class="Family">
    <form id="form1" runat="server" style="height: auto;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Js/WF.min.js?v=5" />
            </Scripts>
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <%= strFieldTypes.ToString() %>

        <div id="breadcrumb-panel" style="display: none;">
            <div id="breadcrumb" class="<%=direction == "ltr"?"left":"right" %> icon-services">
                <div class="<%=direction == "ltr"?"left":"right" %> icon-services bcrumb pd5">
                    <div>
                        <h3 id="headerWorkflow" style="color: #000000; position: relative; bottom: 12px; font-size: 20px !important; left: -20px; font-family: Roboto, sans-serif">Workflow</h3>
                    </div>
                </div>
            </div>
            <div id="dvActions" style="float: right; display: none;">
                <dl id="dlAction" class="ddlBtn">
                    <dt><a href="javascript:void(0)" onclick="ddToggle(event);">
                        <img src="../AxpImages/icons/task.png" alt="task"><span id="lblaction">select an Action</span></a></dt>
                    <dd>
                        <ul id="ddActionBtns">
                        </ul>
                    </dd>
                </dl>
            </div>
        </div>
        <div class="workFlowTable">
            <table align="center" width="100%">
                <tr>
                    <td style="width: 100%">
                        <asp:Menu ID="menuTabs" runat="server" CssClass="menuTabs" OnMenuItemClick="menuTabs_MenuItemClick"
                            Orientation="Horizontal" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="SelectedMenuTab"
                            StaticSubMenuIndent="36px">
                            <Items>
                                <asp:MenuItem Selected="True" Text="Create WorkFlow" Value="0"></asp:MenuItem>
                                <asp:MenuItem Text="Workflow Delegation" Value="1"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                        <div class="tabBody customtabBody">
                            <asp:MultiView ID="mvwTabs" runat="server">
                                <asp:View ID="vwWFcreate" runat="server">
                                    <table style="width: 100%">
                                        <tr>
                                            <td class="dvLeft">
                                                <h2 class="listForm"><i class="material-icons">assignment</i><span>List Of Forms</span></h2>
                                                <div class="dvLeft1 dvLeftInner">
                                                    <asp:TreeView ID="tvWorkflow" runat="server" OnSelectedNodeChanged="tvWorkflow_SelectedNodeChanged">
                                                        <SelectedNodeStyle CssClass="GridHead" />
                                                        <RootNodeStyle CssClass="RootNodeCss" />
                                                    </asp:TreeView>
                                                    <div style="display: none">
                                                        <asp:Button ID="btnTreeClick" runat="server" Text="refresh" OnClick="btnTreeClick_Click" />
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="dvRight">
                                                <div class="dvRightMain">
                                                    <div id="dvShowAll" runat="server" style="display: none">


                                                        <asp:Button ID="btnNew" Width="150px" CssClass="btnwfsavenew" runat="server" OnClick="btnNewWF_Click" Text="New Workflow" />

                                                        <asp:TextBox ID="txtWFDetails" runat="server" TextMode="MultiLine" Width="98%" Rows="20" ReadOnly="true" CssClass="textarea"></asp:TextBox>
                                                    </div>
                                                    <div id="dvWfDetails" runat="server" style="display: none" class="dvRight1">
                                                        <asp:UpdatePanel runat="server">
                                                          
                                                            <ContentTemplate>
                                                                <asp:Panel GroupingText="WorkFlow" CssClass="td" ID="pnlWF" runat="server">
                                                                    <table width="100%" align="center">
                                                                        <tr class="td">
                                                                            <td style="width: 5%">
                                                                                <asp:Label ID="lblWorkFlow" runat="server" meta:resourcekey="lblWorkFlow" Text="Name "></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="TxtWorkFlow" runat="server" Width="175px"></asp:TextBox>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Button ID="btnNewWF" Visible="false" runat="server" Text="New" Width="60px"
                                                                                    OnClick="btnNewWF_Click" />
                                                                                &nbsp;
                                                                        <asp:Button ID="btnDeleteWF" Enabled="false" runat="server" Text="Delete"
                                                                            CssClass="btnwfcancel coldbtn" OnClick="btnDeleteWF_Click" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="font-size: small; font-weight: normal; font-style: normal"
                                                                                colspan="3">
                                                                                <div id="dvAddCond" runat="server">
                                                                                    <a href="javascript:OpenCondPopUp();">
                                                                                        <asp:Label ID="lbladdcond" runat="server" meta:resourcekey="lbladdcond">Click here to add a condition</asp:Label></a>
                                                                                </div>
                                                                                <div id="dvDispCond" runat="server" style="display: none">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <span>
                                                                                                    <asp:Label ID="lblcondclt" runat="server" meta:resourcekey="lblcondclt">Condition :</asp:Label></span><asp:Label ID="lblCond" runat="server">
                                                                                                    </asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Button ID="btnEditCond" Text="Edit" OnClientClick="javascript:EditCondPopUp();" runat="server" />
                                                                                                <asp:Button ID="btnDeleteCond" Text="Delete" runat="server" OnClientClick="javascript:ClearCond();"
                                                                                                    OnClick="btnDeleteCond_Click" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:HiddenField ID="hdnCondDetails" runat="server" />
                                                                                </div>
                                                                                <br />

                                                                                <asp:DropDownList ID="ddlwfnamelist" Visible="false" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                    <div style="display: none">
                                                                        <asp:DropDownList ID="ddlFldTypes" runat="server">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hdnLvlValues" runat="server" />
                                                                    </div>
                                                                </asp:Panel>
                                                            <%--</ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>--%>
                                                                <asp:Panel GroupingText="Roles" CssClass="td" ID="pnlRoles" runat="server">
                                                                    <div id="dvShowRoles" runat="server">
                                                                        <table width="100%" align="center">
                                                                            <tr>
                                                                            <td align="left">&nbsp;<%--<asp:Button ID="btnAddRow" runat="server" OnClientClick="javascript:ShowNewRole();"
                                                                             Text=" +" Width="50px" Visible="false" />--%><i class="material-icons" id="btnAddRole" onclick="javascript: ShowNewRole();">add</i></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <asp:GridView ID="gvWorkFlow" runat="server" EnableViewState="false" AutoGenerateColumns="False" BorderColor="Gray" onclick="javascript:adjustwin(window);" onchange="javascript:ActionChanged()"
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
                                                                                                    <asp:CheckBox ID="chkMandatory" runat="server" Enabled="false" />
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
                                                                                                    <asp:TextBox ID="TxtDays" MaxLength="2" onkeypress="javascript:return CheckNumeric(event,this.value);"
                                                                                                        Text='<%# Bind("WFdays")%>' runat="server" Width="64px"></asp:TextBox>
                                                                                                </EditItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Max Hours" ShowHeader="False">
                                                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblMaxHours" runat="server" Text='<%# Bind("WFhrs")%>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <EditItemTemplate>
                                                                                                    <asp:TextBox ID="TxtHrs" Text='<%# Bind("WFhrs")%>' MaxLength="2" onkeypress="javascript:return CheckNumeric(event,this.value);"
                                                                                                        runat="server" Width="64px"></asp:TextBox>
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
                                                                                                    <asp:DropDownList ID="ddlAction" runat="server" Enabled="true">
                                                                                                    </asp:DropDownList>
                                                                                                </EditItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:CommandField HeaderStyle-BorderColor="Black" ItemStyle-BorderColor="Black" ItemStyle-BorderStyle="Solid"
                                                                                                ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
                                                                                                ItemStyle-Width="5%" ShowEditButton="False">
                                                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                                            </asp:CommandField>
                                                                                            <asp:CommandField HeaderStyle-BorderColor="Black" ItemStyle-BorderColor="Black" ItemStyle-BorderStyle="Solid"
                                                                                                ItemStyle-BorderWidth="1px" ItemStyle-VerticalAlign="Top" ItemStyle-Width="5%"
                                                                                                ShowCancelButton="False" ShowDeleteButton="True">
                                                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top" />
                                                                                            </asp:CommandField>
                                                                                          <%--  <asp:TemplateField HeaderText="Insert Rows" ShowHeader="True">
                                                                                                <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" VerticalAlign="Top"
                                                                                                    Width="8%" />
                                                                                                <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="8%" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkinsert" runat="server" CommandName="Insert" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                                                        Text="Insert"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>--%>
                                                                                        </Columns>
                                                                                        <HeaderStyle CssClass="GridHead" Height="25px" HorizontalAlign="Center" />
                                                                                        <AlternatingRowStyle BackColor="Transparent" />
                                                                                    </asp:GridView>
                                                                                </td>

                                                                                <asp:HiddenField ID="ActionSelectedValue" runat="server" />
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="dvAddRole" style="display: none">
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
                                                                                        <asp:Label ID="lblapprove" runat="server" meta:resourcekey="lblapprove">Is Approval Mandatory :</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="ChkMandatory" Checked='<%# Bind("WFMandatory")%>' Enabled="false" runat="server" />
                                                                                    </td>
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
                                                                                <tr visible="false">
                                                                                    <td align="right"></td>
                                                                                    <td align="left">
                                                                                        <asp:Button ID="btnCreateWF" CssClass="btnwfsavenew" runat="server" OnClick="Button1_Click" Text="OK" Width="60px" />
                                                                                        &nbsp;
                                                                            <asp:Button ID="btnCancelWF" CssClass="btnwfcancel" runat="server" Text="Cancel" OnClick="btnCancelWF_Click" />
                                                                                    </td>
                                                                                    <td></td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </div>
                                                                </asp:Panel>
                                                                <div>
                                                                    <asp:CheckBox ID="chkedit" runat="server" Checked="false" Text="Allow edit after approval" />
                                                                    <asp:CheckBox ID="chkcancel" runat="server" Checked="false" Text="Allow cancel after approval" />

                                                                </div>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="bottom" align="left">

                                                                            <asp:CheckBox ID="chkpredefinedcomments" runat="server" Checked="false" Text="Enable predefined comments for workflow " />
                                                                            <div id="divpdcomment" class="chkdiv" runat="server" style="display: none;">
                                                                                <a href="javascript:AddPreComments();"><label>Enable predefined comments for workflow</label></a>
                                                                            </div>
                                                                        </td>
                                                                        <td valign="bottom" align="right">
                                                                        <asp:Button ID="btnSubmit" CssClass="btnwfsavenew coldbtn" Text="Submit" runat="server" OnClick="btnSubmit_Click"
                                                                             />
                                                                            &nbsp;<asp:Button ID="btnCancel" CssClass="btnwfcancel coldbtn" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                                                            &nbsp;<asp:Button ID="btnNotify" runat="server" Visible="false" Text="Notification"
                                                                                OnClick="btnNotify_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:Panel Visible="false" GroupingText="Notification" CssClass="td" ID="pnlNotify"
                                                            runat="server">
                                                        <table style="width:100% align:"center">
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
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="margin-top: 5px; margin-bottom: 5px;">
                                        <%--                            <input type="button" value="Submit" onclick="SaveSubType();" />--%>
                                        <asp:HiddenField ID="hdnSubTypeCond" runat="server" />
                                        <asp:HiddenField ID="hdnCondTxt" runat="server" />
                                        <asp:HiddenField ID="hdnDisplayCond" runat="server" />
                                    </div>
                                    <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </asp:View>

                                <asp:View ID="Delegation" runat="server">
                                    <asp:Panel ID="pnlUser" runat="server" CssClass="td" GroupingText="From User">
                                        <table>
                                            <tr>
                                                <td class="style4">&nbsp;
                                                </td>
                                                <td class="style5">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style4">
                                                    <asp:Label ID="lblSrcUser" runat="server" meta:resourcekey="lblSrcUser" Text="User name"></asp:Label>
                                                </td>
                                                <td class="style5">
                                                    <input id="SrcUser000F0" maxlength="" name="SrcUser000F0" onblur="javascript:SetSrcFromUserValue();"
                                                        style="width: 197px; height: " type="text" />&nbsp;<a>
                                                            <img id="img~SrcUser000F0" border="0" onclick="javascript:OpenPickList('ddlParentGroup','ddlSrcBranchName','SrcUser000F0~hdnSrcEmpId');"
                                                                src="../AxpImages/icons/search-back.jpg" style="cursor: pointer;" />
                                                        </a>
                                                    <input id="hdnSrcUser" runat="server" type="hidden" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnGo" CssClass="btnwfsavenew" runat="server" OnClick="btnGo_Click" OnClientClick="SetSrcFromUserValue();"
                                                        Text="Get Tasks" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="pnlGrd" GroupingText="Tasks" CssClass="td" runat="server" Height="200px"
                                        ScrollBars="Auto">
                                        <asp:GridView CellSpacing="-1" ID="grdTasks" runat="server" AutoGenerateColumns="False" BorderColor="Gray"
                                            BorderStyle="Solid" BorderWidth="1px" GridLines="None" Height="16px" Width="100%"
                                            CssClass="Grid" OnRowDataBound="grdTasks_RowDataBound" AllowSorting="True" OnSorting="grdTasks_Sorting">
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
                                                <td class="style4">&nbsp;
                                                </td>
                                                <td class="style5">&nbsp;
                                                </td>
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
                                                    <asp:Button ID="btnSubmitTask" CssClass="btnwfsavenew" runat="server" OnClick="btnSubmitTask_Click" OnClientClick="javascript:return ValidateDupTasks();"
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
        <div id="dvCondition" class="dvBg dvLevel bdr" style="display: none">
            <div id="filterDiv" class="">
                <table id="tblpanel" cellspacing="0" cellpadding="3" style="margin-left: 7.1em">
                    <tr>
                        <td id="lvfld">
                            <asp:DropDownList ID="ddlFilter" CssClass="lblTxt" runat="server" Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td id="filcond">
                            <asp:DropDownList ID="ddlFilcond" CssClass="lblTxt" runat="server" onchange="javascript:CheckFilterCond('-1');">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Equal to" Value="="></asp:ListItem>
                                <asp:ListItem Text="Not Equal to" Value="!="></asp:ListItem>
                                <asp:ListItem Text="Less Than" Value="<"></asp:ListItem>
                                <asp:ListItem Text="Greater Than" Value=">"></asp:ListItem>
                                <asp:ListItem Text="Less Than or Equal to" Value="<="></asp:ListItem>
                                <asp:ListItem Text="Greater Than or Equal to" Value=">="></asp:ListItem>
                                <asp:ListItem Text="Between" Value="between"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="filval11">
                            <asp:TextBox ID="txtFilter" runat="server" Width="160px" CssClass="lblTxt" Text=""></asp:TextBox>
                        </td>
                        <td id="filval12">
                            <asp:TextBox ID="filVal2" runat="server" Width="160px" CssClass="lblTxt"
                                Text="" disabled></asp:TextBox>
                            <asp:HiddenField ID="goval" runat="server" Value=""></asp:HiddenField>
                            <asp:HiddenField ID="FilterRowCount" Value="0" runat="server" />
                            <asp:HiddenField ID="hdnAction" runat="server" />
                            <asp:HiddenField ID="hdnFilterXml" runat="server" />
                            <asp:HiddenField ID="hdnValidate" runat="server" />
                        </td>
                        <td>
                            <img src="../AxpImages/icons/16x16/add.png" class="curHand" id="btnAddNew" alt="Add condition"
                                onclick="javascript:AddNewCondition();" />
                        </td>
                    </tr>
                </table>
                <div id="dvNewrow" style="position: relative; top: 3%; height: auto; display: block">
                    <table id="tblFilter">
                    </table>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
