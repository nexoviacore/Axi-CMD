<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="aspx_Roles" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Roles"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <title>Roles</title>
    <!-- ________ CSS __________ -->
    <link id="themecss" type="text/css" rel="Stylesheet" href="" />
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
    <link rel="Stylesheet" href="../Css/generic.min.css?v=10" type="text/css" id="generic" />
    <link href="../Css/Users.min.css?v=5" rel="stylesheet" type="text/css" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <!-- ________ JQUERY __________ -->

    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <!-- ________ JAVASCRIPT __________ -->

    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>

    <script src="../Js/actb.min.js?v=1" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/dimmingdiv.min.js?v=2" type="text/javascript"></script>

    <script src="../Js/umgmt.min.js?v=28" type="text/javascript"></script>
    <script src="../Js/jquery.msgBox.min.js?v=2" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../Css/msgBoxLight.min.css?v=5" />
    <script src="../Js/Roles.min.js?v=3" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
</head>
<!-- <body onload="load();adjustwin(window); ChangeTheme(window);" style="width: 100%; position: relative; margin: 0 auto;" onunload="CheckForUnsavedChangesRole();"> -->
<body onload="load();" style="width: 100%; position: relative; margin: 0 auto;" onunload="CheckForUnsavedChangesRole();">
    <%
        int docHt = 300;
        string tst_Scripts = "";
        tst_Scripts = tst_Scripts + "<script type='text/javascript' >function setDocht(){ adjustwin('" + docHt + "',window);}</script>";
        Response.Write(tst_Scripts);
    %><form id="form1" runat="server" dir="<%=direction%>">
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
                                <h3 style="padding: 10px">Roles</h3>
                            </div>
                            <div class="right" id="icons" style="padding-top: 4px;">
                                <asp:LinkButton ID="imgCopyRoles" runat="server" Style="cursor: hand" CssClass="lnk"
                                    OnClientClick="javascript:return countRolesChecked();"
                                    OnClick="imgCopyRoles_Click">Copy Role</asp:LinkButton>
                                &nbsp;                      
                            <asp:LinkButton ID="imgAddRole" runat="server" CssClass="lnk" OnClientClick="javascript:ShowDimmer(true);displayEditWindow('dvEditRole','Add/Edit Roles','show','Role');SetGoCall('Edit');adjustwin(window,600);"
                                OnClick="imgAddRole_Click" ToolTip="Add Role">Add Role</asp:LinkButton>
                                &nbsp;
                            <asp:LinkButton ID="imgDelRole" runat="server" ToolTip="Delete Role" CssClass="lnk"
                                OnClientClick="javascript:SetGoCall('Delete'); return ConfirmDel('grdRoLstRoles');"
                                OnClick="imgDelRole_Click">Remove</asp:LinkButton>
                                &nbsp;
                            <asp:LinkButton ID="imgListAllRoles" ToolTip="List All Roles" runat="server" CssClass="lnk"
                                OnClientClick="javascript:SetGoCall('Search');" OnClick="imgListAllRoles_Click">Clear</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                        <div id="dvErr1" runat="server" style="display: none; margin-top: 5px; visibility: hidden" class="errBdr">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left">

                                        <asp:HiddenField ID="roleErr" runat="server" />
                                        <asp:HiddenField ID="roleSuccess" runat="server" />

                                    </td>
                                    <td style="text-align: right">
                                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="../AxpImages/icons/close-button.png" BorderWidth="0"
                                            OnClientClick="javascript:HideErrDiv('dvErr1');" OnClick="imgListAllRoles_Click" />

                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="tableBody grd">
                            <div class="darkBdr" style="width: 100%; height: 33px">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: center; vertical-align: middle; width: 30%">
                                            <asp:RadioButtonList ID="rdBtnLstActRoles" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" TextAlign="Right" OnSelectedIndexChanged="rdBtnLstActRoles_SelectedIndexChanged">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem>Active</asp:ListItem>
                                                <asp:ListItem>InActive</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td></td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtRoSeRole" CssClass="Family1" runat="server" placeholder="Role Name" MaxLength="30" TextMode="SingleLine"></asp:TextBox>
                                                <asp:Button ID="btnSearch" runat="server" CssClass="hotbtn btn" ToolTip="Search Roles" OnClientClick="javascript:SetGoCall('Search');"
                                                    OnClick="btnRoSeListRoles_Click" Text="Search" />
                                            </div>
                                        </td>

                                    </tr>
                                </table>
                            </div>
                            <div style="text-align: center">
                                <asp:Label ID="lblNoRecordMsg" runat="server" Text="" CssClass="totrec" Visible="false"></asp:Label>
                            </div>
                            <div>
                                <asp:GridView CellSpacing="-1" ID="grdRoLstRoles" runat="server" AutoGenerateColumns="False" CssClass="gridData" Style="margin-top: 10px;">
                                    <HeaderStyle HorizontalAlign="Center" Height="25px" />
                                    <EmptyDataRowStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="chkRoLstCheck" runat="server" GroupName="rolesselect" OnClick="javascript:SelectSingleRadiobutton(this.id,grdRoLstRoles)" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="20px" />
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Role">
                                            <ItemTemplate>
                                                &nbsp;
                                                <asp:LinkButton ID="lnkRoLstEdit" CssClass="l2" runat="server" Text='<%# Eval("roles") %>'
                                                    OnClientClick="javascript:ShowBackgroundFade();SetGoCall('Edit');adjustwin(window,600);ShowDimmer(true);"
                                                    OnClick="lnkRoLstEdit_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="320px" HorizontalAlign="Center" />
                                            <ItemStyle Width="320px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Responsibilities">
                                            <ItemTemplate>
                                                &nbsp;
                                                <asp:Label ID="lblRoLstResp" runat="server" Text='<%# Eval("res") %>'></asp:Label>
                                                <asp:LinkButton ID="lnkRoLstMore" CssClass="l2" OnClientClick="javascript:SetGoCall('Edit');ShowBackgroundFade();adjustwin(window,600);"
                                                    runat="server" OnClick="lnkRoLstEdit_Click">more</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="600px" HorizontalAlign="Center" />
                                            <ItemStyle Width="600px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Active">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRoLstActive" runat="server" Text='<%# Eval("act") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div>
                                <asp:Label ID="recordsRole" runat="server" Text="" CssClass="totrec"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblpgCap" Text="Page No.  " runat="server" CssClass="totrec"></asp:Label>
                                <asp:DropDownList ID="lvPageRole" runat="server" AutoPostBack="true" onchange="javascript:SetGoCall('Search');adjustwin(window,600);"
                                    OnSelectedIndexChanged="lvPageRole_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="dvEditRole" style="z-index: 1" class="hide">
                            <div id="dvPopErrMsg" runat="server" class="errBdr" style="display: none; height: 30px;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="lblDisErrMsg" runat="server" class="erred"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <input type="button" value="Ok" onclick="javascript: HideErrDiv('dvPopErrMsg'); hiddenFloatingDiv('dvEditRole');" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblrole" runat="server" meta:resourcekey="lblrole">Role</asp:Label>
                                        <span class="req">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRoEditRole" runat="server" Style="font-size: 100%; font-weight: normal; font-style: inherit;" onchange="javascript:CapRoleChkBxEvent();"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span runat="server" id="RoEdActlbl">
                                            <asp:Label ID="lblactlbl" runat="server" meta:resourcekey="lblactlbl">Active</asp:Label></span>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkRoEditActive" runat="server" onchange="javascript:CapRoleChkBxEvent()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlCompnt" runat="server" Visible="false" GroupingText="Access to Components ">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 50%; text-align: center;">

                                                        <asp:CheckBox ID="chkBlog" runat="server" Text="Blog" />
                                                    </td>
                                                    <td style="width: 50%; text-align: center;">

                                                        <asp:CheckBox ID="chkChat" runat="server" Text="Chat" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div style="height: 330px;" id="Panel2" class="PopTd1">
                                            <div class="dvadminheader">
                                                <table>
                                                    <tr>
                                                        <td style="text-align: left; width: 150px;" class="dvadminsubheader">
                                                            <asp:Label ID="lblresponsibility" runat="server" meta:resourcekey="lblresponsibility">Responsibilities</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="dvResList" style="height: 290px; overflow-y: auto;">
                                                <asp:CheckBoxList ID="chkLstRoEd" runat="server">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"></td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <br />
                                <asp:Button ID="btnRoEdSave" CssClass="btnsavenew savecnclhtwt" runat="server" Text="Save" OnClientClick="javascript:SetGoCall('UpdateRole');ShowDimmer(true);return ValidateRole();"
                                    OnClick="btnRoEdSave_Click" />
                                <input type="button" id="btnRoEdCa" class="btncancel savecnclhtwt" value="Cancel" onclick="javascript: hiddenFloatingDiv('dvEditRole');" />
                            </div>
                        </div>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id='waitDiv' style='display: none;'>
                <div id='backgroundDiv' style='background: url(../Axpimages/loadingBars.gif) center center no-repeat rgba(255, 255, 255, 0.4); background-size: 135px;'>
                </div>
            </div>
            <br />
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

        <script type="text/javascript">        var hdnTree = document.getElementById("hdnTreeSelVal").value;</script>

    </form>
</body>
</html>
