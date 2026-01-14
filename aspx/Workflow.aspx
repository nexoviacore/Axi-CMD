<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Workflow.aspx.cs" Inherits="Workflow" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Work flow"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <title>Work flow</title>
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
    <link href="../Css/alerts.min.css" rel="stylesheet" />
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/gen.min.js?v=14" type="text/javascript"></script>

    <script src="../Js/WF.min.js?v=5" type="text/javascript"></script>

    <script src="../Js/helper.min.js?v=172" type="text/javascript"></script>

    <script src="../Js/dimmingdiv.min.js?v=2" type="text/javascript"></script>

    <script src="../Js/actb.min.js?v=1" type="text/javascript"></script>

    <%--<link id="themecss" type="text/css" rel="Stylesheet" href="" />--%>
    <link href="../Css/globalFont.min.css" type="text/css" rel="stylesheet" />
    <link rel="Stylesheet" href="../Css/generic.min.css?v=10" type="text/css" id="generic" />

    <script type="text/javascript">
        var wfLevels = new Array();
        var wfNames = new Array();
        var wfDbNames = new Array();
        var fldTypes = new Array();
        var LvlDetails = new Array();

        var X = 0;
        var Y = 0;
    </script>


    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <style type="text/css">
        .style3 {
            width: 395px;
        }
    </style>
</head>
<body onload="LoadWF();">
    <form id="form1" runat="server" dir="<%=direction%>">
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
                <div id='breadcrumb' class="icon-services left">
                    <h3>Define Workflow</h3>
                </div>
                <div class="right" id="icons">
                </div>
            </div>
            <div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>

                        <asp:Panel ID="pnlWF" CssClass="dc" runat="server">
                            <div class="dcContent">
                                <center>
                                    <table width="40%">

                                        <tr>
                                            <td align="left" style="width: 25%"> 
                                                <asp:Label ID="lblWorkFlow" runat="server" meta:resourcekey="lblWorkFlow" Text="Document"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList AutoPostBack="true" ID="ddlTst" runat="server" Width="225px" CssClass="lblTxt"
                                                    onchange="javascript:ClearLevels();" OnSelectedIndexChanged="ddlTst_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="TxtWorkFlow" runat="server" Visible="False" Width="175px"></asp:TextBox>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 25%">
                                                <asp:Label ID="lblSubType" runat="server" meta:resourcekey="lblSubType" Text="Sub Type"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList AutoPostBack="true" ID="ddlSubType" runat="server" Width="225px"
                                                    CssClass="lblTxt" onchange="javascript:SetList();" OnSelectedIndexChanged="ddlSubType_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <input id="btnSubType" type="button" onclick="javascript: ShowCondDiv(); adjustwin(window);"
                                                    value="Add" />
                                                <input id="btnEditSubType" type="button" onclick="javascript: ShowCondDiv(); EditCond(); adjustwin(window);"
                                                    value="Edit" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblCond" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>

                                            <asp:Label ID="lblSubTypeText" runat="server"></asp:Label>

                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div id="dvCondition" class="dvBg dvLevel bdr" style="display: none">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 80%">&nbsp;<asp:TextBox runat="server" Style="width: 320px;" ID="subtypeName" onclick="ClearEntryText();"
                                        value="Enter a sub type name" />
                                    </td>
                                    <td align="right" style="margin-top: 0px;">
                                        <div style="float: right">
                                            <img src='../AxpImages/icons/close-button.png' alt='Close' onclick="javascript:HideDiv('dvCondition');" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="margin-top: 3px; margin-bottom: 3px; margin-left: 2px;">
                                <asp:Label ID="lblcatogorised" runat="server" meta:resourcekey="lblcatogorised">
                                    All transactions that satisfy the below condition will be categorised under this
                                subtype.
                                </asp:Label>
                            </div>
                            <div id="filterDiv" class="">
                                <table id="tblpanel" cellspacing="0" cellpadding="3">

                                    <tr>
                                        <td id="lvfld">
                                            <asp:DropDownList ID="ddlFilter" CssClass="lblTxt" runat="server" Width="120px">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="filcond">
                                            <asp:DropDownList ID="ddlFilcond" CssClass="lblTxt" runat="server" onclick="javascript:CheckFilterCond('-1');">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Equal To" Value="="></asp:ListItem>
                                                <asp:ListItem Text="Not Equal To" Value="!="></asp:ListItem>
                                                <asp:ListItem Text="Containing" Value="contains"></asp:ListItem>
                                                <asp:ListItem Text="Not Containing" Value="not contains"></asp:ListItem>
                                                <asp:ListItem Text="<" Value="lessthan"></asp:ListItem>
                                                <asp:ListItem Text=">" Value="greaterthan"></asp:ListItem>
                                                <asp:ListItem Text="<= " Value="lessthan equal"></asp:ListItem>
                                                <asp:ListItem Text=">=" Value="greaterthan equal"></asp:ListItem>
                                                <asp:ListItem Text="Between" Value="between"></asp:ListItem>
                                                <asp:ListItem Text="Start with" Value="start with"></asp:ListItem>
                                                <asp:ListItem Text="End with" Value="end with"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td id="filval11">
                                            <asp:TextBox ID="txtFilter" runat="server" Width="160px" CssClass="lblTxt" Text=""></asp:TextBox>
                                        </td>
                                        <td id="filval12">
                                            <asp:TextBox ID="filVal2" Enabled="false" runat="server" Width="160px" CssClass="lblTxt"
                                                Text=""></asp:TextBox>
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
                            <div style="margin-top: 5px; margin-bottom: 5px;">
                                <%--                            <input type="button" value="Submit" onclick="SaveSubType();" />--%>
                                <asp:HiddenField ID="hdnSubTypeCond" runat="server" />
                                <asp:HiddenField ID="hdnCondTxt" runat="server" />
                            </div>
                        </div>
                        <div style="display: none">
                            <asp:DropDownList ID="ddlFldTypes" runat="server">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdnLvlValues" runat="server" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="clear">
                </div>
                <asp:Panel ID="pnlLevels" runat="server" CssClass="dc">
                    <div class="dcTitle">
                        <asp:Label ID="lbllevels" runat="server" meta:resourcekey="lbllevels">
                        Levels</asp:Label>
                    </div>
                    <div id="dvLevels" runat="server" class="dcContent">
                    </div>
                    &nbsp;
                <a href="javascript:AddNewLevel();adjustwin(window);" id="lnkAddNewLevel"><asp:Label ID="lbladdlevels" runat="server" meta:resourcekey="lbladdlevels">
                        Add Levels</asp:Label></a>
                    <asp:HiddenField runat="server" ID="hdnLevelCnt" />
                </asp:Panel>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="windowcontent2" class="Hidden">
                            <table style="width: 100%">
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:HiddenField ID="hdnLoadSubType" runat="server" />
                        <asp:HiddenField ID="hdnSubTypeDet" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="display: none">
                    <asp:DropDownList ID="ddlTransactionId" runat="server">
                    </asp:DropDownList>
                    <asp:ListBox ID="ddlRole" runat="server" Height="100px" Width="175px" SelectionMode="Multiple"
                        onchange="javascript:CheckMandatory(this)"></asp:ListBox>
                    <asp:DropDownList ID="ddlwfnamelist" Visible="false" runat="server">
                    </asp:DropDownList>
                    <asp:Button ID="btnLoadWorkflow" runat="server" OnClick="btnLoadWorkflow_Click" /><asp:HiddenField
                        ID="hdnWfName" runat="server" />
                    <asp:DropDownList ID="ddlRoles" runat="server">
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnListWf" Value="" runat="server" />
                </div>
                <br />
                <div>
                    <center>
                        <asp:Button runat="server" ID="btnSave" Text="Save" OnClientClick="return GetRoleData();"
                            OnClick="btnSave_Click" /></center>
                </div>
                <div id='waitDiv' style='display: none;'>
                    <div id='backgroundDiv' style='background: url(../Axpimages/loadingBars.gif) center center no-repeat rgba(255, 255, 255, 0.4); background-size: 135px;'>
                    </div>
                </div>
                <br />
                <br />
                <asp:HiddenField ID="hdnWrkLevel" runat="server" />
                <asp:HiddenField ID="hdnDisplayLevels" runat="server" />
            </div>
        </div>
        <%=strWorkflow.ToString() %>
    </form>
</body>
</html>
