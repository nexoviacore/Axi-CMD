<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoComplete.aspx.cs" Inherits="aspx_AutoComplete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" class="overflow-auto">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>

    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
    <link href="../UI/axpertUI/datatables.bundle.css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>
    <script src="../UI/axpertUI/datatables.bundle.js"></script>
    <script src="../Js/noConflict.min.js"></script>

    <script type="text/javascript" src="../Js/tstruct.min.js?v=673"></script>
    <script type="text/javascript" src="../Js/helper.min.js?v=172"></script>
    <script type="text/javascript" src="../Js/jsclient.min.js?v=118"></script>

    <script src="../Js/advSearch.min.js?v=27" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script type="text/javascript">
        var _isMulSel = '<%=_isMulSel%>';
        var IsFormDirty = false;
        var ChangedFields = new Array();
        var ChangedFieldValues = new Array();
        var DeletedDCRows = new Array();
        var fldNewNameArr = new Array();
        var fldNewDbRowNo = new Array();
        var fldNewValueArr = new Array();
        var AllFieldNames = new Array();
        var AllFieldValues = new Array();
        var trSelClick = true;
        var selectedValList = new Array();
        var unSelectedValList = new Array();
        var _isPageInfo = '<%=_isPageInfo%>';
        function loadParent(a) {
            if (trSelClick) {
                trSelClick = false;
                var gov = document.getElementById("<%=searchlist.ClientID%>");
                var grviewpno = document.getElementById("<%=pgno.ClientID%>");
                var pno = parseInt(grviewpno.value)
                let _ValNotSel = false;
                if (a == -1)
                    _ValNotSel = true;
                gov.selectedIndex = (pno * 10) + parseInt(a);
                var govlistval = "<%=searchlistval.ClientID%>";
                parent.isGrdEditDirty = true;
                SetSelectedValue(gov, govlistval, _ValNotSel);
                try {
                    var fname = document.form1.fname.value;
                    if (window.opener == null) {
                        var parentFillField = $j("#" + fname, parent.window.document);
                    } else {
                        var parentFillField = $j("#" + fname, window.opener.document);
                    }
                    parent.MainBlur(parentFillField);
                } catch (exp) { }
            }
        }

        function AddValuesToField() {
            if (typeof _isMulSel != "undefined" && _isMulSel == "true") {
                var govlistval = "<%=searchlistval.ClientID%>";
                parent.isGrdEditDirty = true;
                SetSelectedValueMul(selectedValList, unSelectedValList, govlistval);
                try {
                    var fname = document.form1.fname.value;
                    if (window.opener == null) {
                        var parentFillField = $j("#" + fname, parent.window.document);
                    } else {
                        var parentFillField = $j("#" + fname, window.opener.document);
                    }
                    parent.MainBlur(parentFillField);
                } catch (exp) { }
            } else {
                if (_selTrIndex != "NoChange")
                    loadParent(_selTrIndex);
            }
        }
    </script>
    <style>
        .noHover {
            pointer-events: none;
        }
    </style>
</head>
<body dir="<%=direction%>" class="container p-0">
    <form id="form1" runat="server">
        <asp:HiddenField ID="fname" runat="server" />
        <span style="display: none">
            <asp:ListBox ID="searchlist" Visible="true" runat="server" Height="0px" Width="0px"
                AutoPostBack="True"></asp:ListBox>
            <asp:ListBox ID="searchlistval" Visible="true" runat="server" Height="0px" Width="0px"
                AutoPostBack="True"></asp:ListBox>
            <asp:ListBox ID="lstValues" Visible="true" runat="server" Height="0px" Width="0px"
                AutoPostBack="True"></asp:ListBox>

            <asp:TextBox ID="pgno" runat="server" Text="0" Visible="true" BorderStyle="None"
                ForeColor="white" Width="1" BackColor="white"></asp:TextBox>
        </span>
        <div class="toolbar m-0 ms-5 p-0 py-1">
            <div class="container-fluid p-0 d-flex flex-stack flex-wrap flex-sm-nowrap">
                <div class="d-flex flex-column align-items-start justify-content-center flex-wrap me-2" id="breadcrumb-panel">
                    <h1 class="text-dark fw-boldest my-1 fs-2">
                        <div id="divTitle" runat="server"></div>
                        <small class="text-muted fs-6 fw-normal ms-1"></small></h1>
                </div>
            </div>
        </div>
        <div id="main" class="row">
            <div class="col-12 pb-5 ">
                <div class="control-group input-group" runat="server" id="divFilter">
                    <div class="fv-row mb-8 fv-plugins-icon-container me-2">
                        <div class="dropdown">
                            <asp:DropDownList ID="ddlSearchFld" CssClass="form-control input-sm selectPaddingFix" runat="server" autofocus>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container me-2 d-none">
                        <div class="dropdown">
                            <asp:DropDownList ID="ddlDataType" CssClass="form-control input-sm selectPaddingFix" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container me-2">
                        <div class="dropdown">
                            <asp:DropDownList ID="ddlCondition" CssClass="form-control input-sm selectPaddingFix" runat="server">
                                <asp:ListItem Text="-- Select Condition --" Value=""></asp:ListItem>
                                <asp:ListItem Text="Equal To" Value="="></asp:ListItem>
                                <asp:ListItem Text="Not Equal To" Value="!="></asp:ListItem>
                              <%--  <asp:ListItem Text="Less Than" Value="<"></asp:ListItem>
                                <asp:ListItem Text="Less Than or Equal To" Value="<="></asp:ListItem>
                                <asp:ListItem Text="Greater Than" Value=">"></asp:ListItem>
                                <asp:ListItem Text="Greater Than or Equal To" Value=">="></asp:ListItem>--%>
                                <asp:ListItem Text="Starts With" Value="starts with"></asp:ListItem>
                                <asp:ListItem Text="Ends With" Value="ends with"></asp:ListItem>
                                <asp:ListItem Text="Contains" Value="contains"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container me-2">
                        <div class="dropdown profile-pic">
                            <asp:TextBox ID="txtfilter" runat="server" class="form-control input-sm selectPaddingFix" placeholder="filter value" TabIndex=" 0"></asp:TextBox>
                            <div class="edit">
                                <i class="glyphicon glyphicon-remove clearico" title="clear"></i>
                            </div>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container me-2 d-none" id="txtfltbetween">
                        <div class="dropdown profile-pic">
                            <asp:TextBox ID="txtfilter1" runat="server" class="form-control input-sm selectPaddingFix" placeholder="filter value" TabIndex="0"></asp:TextBox>
                            <div class="edit">
                                <i class="glyphicon glyphicon-remove clearico" title="clear"></i>
                            </div>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container">
                        <a id="btnFilterSearch" class="btn btn-icon btn-primary me-2 shadow-sm" title="filter" onclick="callserverbtn('btnsearch');"><span class="material-icons material-icons-style material-icons-3">filter_alt</span></a>

                        <a id="btnFilterClear" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm" title="clear filter" onclick="callserverbtn('btnclear');"><span class="material-icons material-icons-style material-icons-3">clear</span></a>

                        <%-- <a class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm disabled" runat="server" id="btnPriv" title="previous" onclick="callserverbtn('btnprevi');"><span id="spanPriv" runat="server" class="material-icons material-icons-style material-icons-3">arrow_back_ios</span></a>

                        <a class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm disabled" runat="server" id="btnNextclk" title="next" onclick="callserverbtn('btnnext');"><span id="spanNextclk" runat="server" class="material-icons material-icons-style material-icons-3">arrow_forward_ios</span></a>--%>

                        <asp:Button runat="server" class="btn btn-default btn-sm filterico glyphicon glyphicon-filter " Style="display: none" ID="btnsearch" title="filter" OnClick="btnGo_Click" TabIndex="0"></asp:Button>
                        <asp:Button runat="server" class="btn btn-default btn-sm filterico glyphicon glyphicon-filter" Style="display: none" ID="btnclear" title="filter" OnClick="btnClear_Click" TabIndex="0"></asp:Button>
                        <%--  <asp:Button runat="server" class="btn btn-default btn-sm filterico glyphicon glyphicon-chevron-left glyphsetting" Style="display: none" ID="btnprevi" title="previous" OnClick="btnPriv_Click" TabIndex="0"></asp:Button>
                        <asp:Button runat="server" class="btn btn-default btn-sm filterico glyphicon glyphicon-chevron-right glyphsetting" Style="display: none" ID="btnnext" title="next" OnClick="btnNext_Click" TabIndex="0"></asp:Button>--%>
                    </div>
                </div>
                <div class="control-group">
                    <div class="btnsearch">
                        <div class="form-group profile-pic" id="dvSearch" runat="server">
                            <asp:TextBox ID="txtSrchText" runat="server" class="search form-control" placeholder="search..."></asp:TextBox>
                            <div class="nsearch">
                                <i class="glyphicon glyphicon-search" title="search"></i>
                            </div>
                            <div class="edit ">
                                <i class="glyphicon glyphicon-remove clearico" title="clear"></i>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="control-group">
                    <asp:GridView ID="GridView1" runat="server" CellPadding="2" GridLines="Vertical" Style="margin-top: 10px;"
                        Width="100%" AllowSorting="false" RowStyle-Wrap="false" AutoGenerateColumns="false"
                        PageSize="100" CssClass="gridData ivirMainGrid dataTable table table-striped table-hover border cursor-pointer" OnRowDataBound="GridView1_RowDataBound" border="0">
                        <HeaderStyle />
                        <AlternatingRowStyle />
                    </asp:GridView>
                    <div id="dvFooter" runat="server" class="d-flex flex-row">
                        <div class="py-3">
                            <asp:Label ID="lblshow" runat="server" meta:resourcekey="lblshow">Showing</asp:Label>
                            <asp:Label ID="lblstart" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblto" runat="server" meta:resourcekey="lblto">to</asp:Label>
                            <asp:Label ID="lblend" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblof" runat="server" meta:resourcekey="lblof">of</asp:Label>
                            <asp:Label ID="lbltotrec" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblentries" runat="server" meta:resourcekey="lblentries">entries</asp:Label>
                        </div>
                        <div class="ms-auto">
                            <a class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm disabled" runat="server" id="btnPriv" title="previous" onclick="callserverbtn('btnprevi');"><span id="spanPriv" runat="server" class="material-icons material-icons-style material-icons-3">arrow_back_ios</span></a>
                            <a class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm disabled" runat="server" id="btnNextclk" title="next" onclick="callserverbtn('btnnext');"><span id="spanNextclk" runat="server" class="material-icons material-icons-style material-icons-3">arrow_forward_ios</span></a>
                            <asp:Button runat="server" class="btn btn-default btn-sm filterico glyphicon glyphicon-chevron-left glyphsetting" Style="display: none" ID="btnprevi" title="previous" OnClick="btnPriv_Click" TabIndex="0"></asp:Button>
                            <asp:Button runat="server" class="btn btn-default btn-sm filterico glyphicon glyphicon-chevron-right glyphsetting" Style="display: none" ID="btnnext" title="next" OnClick="btnNext_Click" TabIndex="0"></asp:Button>

                        </div>
                    </div>
                </div>

                <div class="control-group" id="progressArea" runat="server" visible="false">
                    <div>
                        <asp:Label ID="lblnodata" runat="server" meta:resourcekey="lblnodata"> No data found.</asp:Label>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
