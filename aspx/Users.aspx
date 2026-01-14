<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="aspx_Users" EnableEventValidation="false" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Axpert file uploader" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>

    <title>Users</title>
    <!-- ________ CSS __________ -->

    <link id="themecss" type="text/css" rel="Stylesheet" href="" />
    <link rel="Stylesheet" href="../Css/generic.min.css?v=10" type="text/css" id="generic" />
    <link href="../Css/Users.min.css?v=5" rel="stylesheet" type="text/css" />
    <link href="../Css/msgBoxLight.min.css?v=4" rel="stylesheet" type="text/css" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
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
    <!-- ________ JQUERY __________ -->

    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>

    <!-- ________ JAVASCRIPT __________ -->
    <script src="../Js/JDate.min.js?v=3" type="text/javascript"></script>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>
    <script src="../Js/actb.min.js?v=1" type="text/javascript"></script>
    <script src="../Js/dimmingdiv.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/umgmt.min.js?v=28" type="text/javascript"></script>
    <script src="../Js/calendar.min.js?v=1" type="text/javascript"></script>
    <script src="../Js/md5.min.js" type="text/javascript"></script>
    <script src="../Js/jquery.msgBox.min.js?v=2" type="text/javascript"></script>
    <script type="text/javascript">
        Enableoldtheme = '<%=HttpContext.Current.Session["AxEnableOldTheme"].ToString() %>';
    </script>


</head>
<%
    int docHt = 300;
    string tst_Scripts = "";
    string global_Scripts = "";
    global_Scripts = "<script language='javascript' type='text/javascript' > var Parameters = new Array();" + strGlobalVar + " </script>";
    tst_Scripts = tst_Scripts + "<script language='javascript' type='text/javascript' >function setDocht(){ adjustwin('" + docHt + "',window);}</script>";
    Response.Write(tst_Scripts);
    Response.Write(global_Scripts);
%>
<!-- <body onload="load();adjustwin(window,700); ChangeTheme(window);" onunload="CheckForUnsavedChangesUser();"> -->
<body onload="load();" onunload="CheckForUnsavedChangesUser();">
    <form id="form1" runat="server" dir="<%=direction%>">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Js/umgmt.min.js?v=28" />
            </Scripts>
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div id="wBdr">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="divcontainer">
                        <div id="breadcrumb-panel">
                            <div id='breadcrumb' class="icon-services left">
                                <h3 style="padding: 10px">Users</h3>
                            </div>
                            <div class="right" id="icons" style="padding-top: 4px;">
                                <asp:LinkButton ID="imgAddUser" runat="server" CssClass="lnk" OnClientClick="javascript:ShowDimmer(true);SetGoCall('AddUser');adjustwin(window,700);"
                                    OnClick="imgAddNewUs_Click" ToolTip="Add User">Add User</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="imgDelUser" runat="server" ToolTip="Delete User" CssClass="lnk"
                                    OnClientClick="javascript:SetGoCall('Delete'); return ConfirmDel('grdUsLstUsers');"
                                    OnClick="imdDelUser_Click">Remove</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="imgListUsers" ToolTip="List All Users" runat="server" CssClass="lnk"
                                    OnClientClick="javascript:SetGoCall('Search');" OnClick="imgListAllUser_Click">Clear</asp:LinkButton>
                            </div>
                        </div>
                        <div id="dvErr2" runat="server" style="display: none; margin-top: 5px; visibility: hidden" class="errBdr">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align:left">
                                        <asp:HiddenField ID="userErr" runat="server" />
                                        <asp:HiddenField ID="userSuccess" runat="server" />
                                    </td>
                                    <td style="text-align:left">
                                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="../AxpImages/icons/close-button.png" BorderWidth="0"
                                            OnClientClick="javascript:HideErrDiv('dvErr2');" OnClick="btnUsSeUsers_Click" />



                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dvSucc2" runat="server" style="display: none; margin-top: 5px;" class="errBdr">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align:left">
                                        <asp:Label ID="lblSuccMsg2" runat="server" CssClass="erred"></asp:Label>
                                    </td>
                                    <td style="text-align:left">
                                        <a onclick="javascript:HideErrDiv('dvSucc2');" style="cursor: hand;">
                                            <img alt="Close" title="Close" src="../AxpImages/icons/close-button.png" border="0"></a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="tableBody grd">
                            <div class="darkBdr" style="height: 33px;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align:left;vertical-align:middle;width:30%;">
                                            <asp:RadioButtonList ID="rdbtnListActUsers" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" TextAlign="Right" OnSelectedIndexChanged="rdbtnListActUsers_SelectedIndexChanged">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem>Active</asp:ListItem>
                                                <asp:ListItem>InActive</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td></td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtUsSeUser" runat="server" placeholder="User Name"
                                                    CssClass="Family1"></asp:TextBox>
                                                <asp:Button ID="btnSearch" runat="server" CssClass="hotbtn btn" ToolTip="Search User" Text="Search" OnClick="btnUsSeUsers_Click"
                                                    OnClientClick="javascript:SetGoCall('Search');" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <center>
                                <asp:Label ID="lblNoRecordMsg" runat="server" Text="" CssClass="totrec" Visible="false"></asp:Label>
                            </center>
                            <div id="dvGrid" runat="server">

                                <asp:GridView CellSpacing="-1"  ID="grdUsLstUsers" runat="server" AutoGenerateColumns="False" CssClass="gridData" Style="margin-top: 10px;">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="20px" />
                                            <ItemStyle Width="20px" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="chkUsLstCheck" runat="server" GroupName="userselect" OnClick="javascript:SelectSingleRadiobutton(this.id,grdUsLstUsers)" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Name" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="200px" />
                                            <ItemStyle Width="200px" />
                                            <ItemTemplate>
                                                &nbsp;
                                            <asp:LinkButton ID="lnkUsName" CssClass="l2" runat="server" Text='<%# Eval("users") %>'
                                                OnClientClick="javascript:SetGoCall('Edit');adjustwin(window,700);ShowDimmer(true);"
                                                OnClick="lnkUsName_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="400px" />
                                            <ItemStyle Width="400px" />
                                            <ItemTemplate>
                                                &nbsp;
                                            <asp:Label ID="lblUsLstEmail" runat="server" Text='<%# Eval("email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Roles" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="400px" />
                                            <ItemStyle Width="400px" />
                                            <ItemTemplate>
                                                &nbsp;
                                            <asp:Label ID="lblUsLstRoles" runat="server" Text='<%# Eval("roles") %>'></asp:Label>
                                                <asp:LinkButton ID="lnlUsLstMore" CssClass="l2" runat="server" OnClientClick="javascript:ShowBackgroundFade();SetGoCall('Edit');adjustwin(window,700);">more</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="60px" />
                                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblUsLstActive" runat="server" Text='<%# Eval("act") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                            <div id="dvInfo">
                                <asp:Label ID="recordsUser" runat="server" Text="" CssClass="totrec"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblpgCap1" Text="Page No.  " runat="server" CssClass="totrec"></asp:Label>
                                <asp:DropDownList ID="lvPageUser" runat="server" AutoPostBack="true" onchange="javascript:SetGoCall('Search');adjustwin(window,700);"
                                    OnSelectedIndexChanged="lvPageUser_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="dvAddEditUser" class="hide">
                            <div id="dvPopErrMsg" runat="server" class="errBdr" style="display: none; height: 30px;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align:left;">
                                            <asp:Label ID="lblDisErrMsg" runat="server" class="erred"></asp:Label>
                                        </td>
                                        <td style="text-align:right;">
                                            <input type="button" value="Ok" onclick="javascript: HideErrDiv('dvPopErrMsg'); hiddenFloatingDiv('dvAddEditUser');" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table style="width: 99%; text-align: left; margin-left: 2px; margin-top: 2px;" class="genBdr">
                                <tr>
                                    <td>Name <font class="req">*</font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUsAdName" runat="server" Style="font-size: 100%; font-weight: normal; font-style: inherit;" TextMode="SingleLine" Width="140px" MaxLength="30" onchange="javascript:CapUserChangeEvent()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Password <font class="req">*</font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUsAdsw" runat="server" TextMode="Password" Width="140px" autocomplete="off"></asp:TextBox>
                                        <input runat="server" type="hidden" id="sw000F0" name="sw000F0" value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Confirm Password <font class="req">*</font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUsAdConsw" runat="server" TextMode="Password" Width="140px" autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Password Expiry Days <font class="req"></font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="pwdExpiryDays" Text="0" runat="server" Style="font-size: 100%; font-weight: normal; font-style: inherit; font-family: inherit;" TextMode="SingleLine" Width="140px" MaxLength="30"></asp:TextBox>
                                        <asp:HiddenField ID="pwdlength" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Active
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkUsAdAct" runat="server" onclick="javascript:UpdateHiddenFieldValue();CapUserChangeEvent();" />
                                    </td>
                                    <asp:HiddenField runat="server" ID="checkBoxDefaultValue" Value="True" />
                                </tr>
                                <tr>
                                    <td>Email <font class="req">*</font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUsAdEmail" runat="server"
                                            TextMode="SingleLine" Width="224px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Reporting To <font class="req"></font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SrcUser000F0" runat="server" onblur="javascript:SetSrcFromUserValue();" onchange="javascript:CapUserChangeEvent()"
                                            TextMode="SingleLine" Width="224px"></asp:TextBox>
                                        &nbsp;<a><img src="../AxpImages/icons/search-back.jpg" alt="search user"
                                            id="" onclick="javascript:OpenPickList('ddlParentGroup','ddlSrcBranchName','SrcUser000F0~hdnSrcEmpId');" style="zoom: .6; vertical-align: middle; pointer; cursor: hand;"
                                            border="0" /></a>
                                        <asp:HiddenField ID="hdnSrcUser" runat="server" />
                                        <asp:HiddenField ID="hdnSrcEmpId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlShowBtns" runat="server" GroupingText="Access to">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:CheckBox ID="chkAvailBuild" Style="float: left;" runat="server" Text="Build*" onclick="javascript:CapUserChangeEvent()" /></center>
                                                    </td>
                                                    <td>
                                                        <center>
                                                            <asp:CheckBox ID="chkShowWorkflow" Style="float: left;" runat="server" Text="Workflow" onclick="javascript:CapUserChangeEvent()" /></center>
                                                    </td>
                                                    <td>
                                                        <center>
                                                            <asp:CheckBox ID="chkAvailAxManager" Style="float: left;" runat="server" Text="Axpert Manager*" onclick="javascript:CapUserChangeEvent()" /></center>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:CheckBox ID="chkAvailSync" Style="float: left;" runat="server" Text="Synchronize*" onclick="javascript:CapUserChangeEvent()" /></center>
                                                    </td>
                                                    <td>
                                                        <center>
                                                            <asp:CheckBox ID="chkAvailImport" Style="float: left;" runat="server" Text="Import" onclick="javascript:CapUserChangeEvent()" /></center>
                                                    </td>
                                                    <td>
                                                        <center>
                                                            <asp:CheckBox ID="chkAvailExport" Style="float: left;" runat="server" Text="Export" onclick="javascript:CapUserChangeEvent()" /></center>
                                                    </td>
                                                    <td>
                                                        <center>
                                                            <asp:CheckBox ID="chkShowUsers" Style="float: left;" runat="server" Text="Users" Checked="false" onclick="javascript:CapUserChangeEvent()" /></center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:LinkButton ID="lnkAddUser" Style="margin-left: 4px;" OnClientClick="javascript:handleError();SetGoCall('AddRole');CapUserChangeEvent()" runat="server" OnClick="lnkAddUser_Click"><img src="../axpimages/icons/16x16/add.png" alt="Add User" /></asp:LinkButton>
                            <br />
                            <div id="divUsRoles" style="height: 110px; border: #bbb 1px solid; overflow-y: auto;">
                                <asp:GridView CellSpacing="-1"  ID="grdUsRoles" runat="server" AutoGenerateColumns="false" Style="margin-left: 4px;" CellPadding="1"
                                     CssClass="gridData" OnRowCreated="grdUsRoles_RowCreated" OnRowDataBound="grdUsRoles_RowDataBound">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgDelUserRole" runat="server" ImageUrl="~/AxpImages/icons/16x16/delete.png"
                                                    OnClientClick="javascript:SetGoCall('DeleteUserRole');CapUserChangeEvent();" OnClick="imgDelUserRole_Click"
                                                    ToolTip="Delete Role" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Role" HeaderStyle-Width="200px" ItemStyle-Width="200px"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlUsRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUs_SelectedIndexChanged" onchange="javascript:CapUserChangeEvent();SetGoCall('AddRole');">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="180px" ItemStyle-Width="180px"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUsStrDt" runat="server" Width="80" onfocus="javascript:showCalendarControl(this);"
                                                    onchange="javascript:CapUserChangeEvent()" Text='<%# Eval("STARTDATE") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="180px" ItemStyle-Width="180px"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUsEndDt" runat="server" Width="80" onfocus="javascript:showCalendarControl(this);"
                                                    onchange="javascript:CapUserChangeEvent()" Text='<%# Eval("ENDDATE") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <div>
                                <center>
                                    <asp:Button ID="btnUsAdSave" runat="server" Text="Save" OnClick="btnUsAdSave_Click"
                                        CssClass="btnsavenew savecnclhtwt" OnClientClick="javascript:SetGoCall('SaveUser');ShowDimmer(true);return ValidateUser();" />
                                    <input type="button" class="btncancel savecnclhtwt" id="btnUsAdCancel" onclick="javascript: hiddenFloatingDiv('dvAddEditUser', 'close');"
                                        value="Cancel" />
                                </center>
                                <div style="margin-left: 20px;">* This feature is only available in Axpert.</div>
                            </div>
                        </div>

                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
            <div id='waitDiv' style='display: none;'>
                <div id='backgroundDiv' style='background: url(../Axpimages/loadingBars.gif) center center no-repeat rgba(255, 255, 255, 0.4); background-size: 135px;'>
                </div>
            </div>
            <div class="container2">
                <div class="tblfoot">
                </div>

            </div>
        </div>
        <asp:Label ID="lblNodata" runat="server" meta:resourcekey="lblNodata" Visible="false">No data found.</asp:Label>
        <asp:HiddenField ID="hdnIsSearched" runat="server" />
        <asp:HiddenField ID="hdnTreeSelVal" runat="server" />
        <asp:HiddenField ID="hdnStatus" runat="server" />

        <%=acScript%>
        <script src="../Js/user.min.js?v=20" type="text/javascript"></script>

        <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    </form>
</body>
</html>
