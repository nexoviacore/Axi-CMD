<%@ Page Language="C#" AutoEventWireup="true" CodeFile="iviewAutoComplete.aspx.cs" Inherits="aspx_iviewAutoComplete" %>

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

    <script type="text/javascript" src="../Js/alerts.min.js?v=32"></script>
    <script type="text/javascript" src="../Js/helper.min.js?v=172"></script>

    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script src="../Js/iviewAutoComplete.min.js?v=4" type="text/javascript"></script>
    <script type="text/javascript">
        var fldname = "<%=fldname.ToString()%>";        
    </script>   
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
        <div class="toolbar m-0 ms-5 p-0 py-1 d-none">
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
                            <asp:DropDownList ID="ddlSearchFld" CssClass="form-control" runat="server" autofocus>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container me-2 d-none">
                        <div class="dropdown">
                            <asp:DropDownList ID="ddlDataType" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container me-2">
                        <div class="dropdown">
                            <asp:DropDownList ID="ddlCondition" CssClass="form-control" runat="server">
                                <asp:ListItem Text="-- Select Condition --" Value=""></asp:ListItem>
                                <asp:ListItem Text="Equal To" Value="="></asp:ListItem>
                                <asp:ListItem Text="Not Equal To" Value="!="></asp:ListItem>
                                <asp:ListItem Text="Starts With" Value="starts with"></asp:ListItem>
                                <asp:ListItem Text="Ends With" Value="ends with"></asp:ListItem>
                                <asp:ListItem Text="Contains" Value="contains"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container me-2">
                        <div class="dropdown profile-pic">
                            <asp:TextBox ID="txtfilter" runat="server" class="form-control" placeholder="filter value" TabIndex=" 0"></asp:TextBox>
                            <div class="edit">
                                <i class="glyphicon glyphicon-remove clearico" title="clear"></i>
                            </div>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container me-2 d-none" id="txtfltbetween">
                        <div class="dropdown profile-pic">
                            <asp:TextBox ID="txtfilter1" runat="server" class="form-control" placeholder="filter value" TabIndex="0"></asp:TextBox>
                            <div class="edit">
                                <i class="glyphicon glyphicon-remove clearico" title="clear"></i>
                            </div>
                        </div>
                    </div>

                    <div class="fv-row mb-8 fv-plugins-icon-container">
                        <a class="btn btn-sm btn-icon btn-light-primary me-2 shadow-sm p-6" title="filter" onclick="callserverbtn('btnsearch');"><span class="material-icons material-icons-style material-icons-3">filter_alt</span></a>

                        <a class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm p-6" title="clear filter" onclick="callserverbtn('btnclear');"><span class="material-icons material-icons-style material-icons-3">clear</span></a>
                        <asp:Button runat="server" class="btn btn-default btn-sm filterico glyphicon glyphicon-filter d-none" ID="btnsearch" title="filter" OnClick="btnGo_Click" TabIndex="0"></asp:Button>
                        <asp:Button runat="server" class="btn btn-default btn-sm filterico glyphicon glyphicon-filter d-none" ID="btnclear" title="filter" OnClick="btnClear_Click" TabIndex="0"></asp:Button>                       
                    </div>
                </div>
                <div class="control-group">
                    <div class="btnsearch">
                        <div class="input-group profile-pic" id="dvSearch" runat="server" visible="false">
                            <asp:TextBox ID="txtSrchText" runat="server" class="search form-control" placeholder="search..."></asp:TextBox>         
                            <div class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary p-6 input-group-text rounded-0 border border-1 border-gray-400">
                                <span class="material-icons material-icons-style material-icons-3 clearico" title="clear">clear</span>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="control-group">
                    <asp:GridView ID="GridView1" runat="server" CellPadding="2" GridLines="Vertical" Style="margin-top: 10px;"
                        Width="100%" AllowSorting="false" RowStyle-Wrap="false" AutoGenerateColumns="false"
                        PageSize="50" CssClass="gridData ivirMainGrid dataTable table table-striped table-hover border cursor-pointer" OnRowDataBound="GridView1_RowDataBound" border="0">
                        <HeaderStyle />
                        <AlternatingRowStyle />
                    </asp:GridView>
                    <div id="dvFooter" runat="server" class="d-flex flex-row" visible="false">
                        <div class="py-3">
                            <asp:Label ID="lblshow" runat="server" meta:resourcekey="lblshow">Showing</asp:Label>
                            <asp:Label ID="lblstart" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblto" runat="server" meta:resourcekey="lblto">to</asp:Label>
                            <asp:Label ID="lblend" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblof" runat="server" meta:resourcekey="lblof">of</asp:Label>
                            <asp:Label ID="lbltotrec" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblentries" runat="server" meta:resourcekey="lblentries">entries</asp:Label>
                        </div>
                        <div class="ms-auto d-none">
                            <a class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm disabled" runat="server" id="btnPriv" title="previous" onclick="callserverbtn('btnprevi');"><span id="spanPriv" runat="server" class="material-icons material-icons-style material-icons-3">arrow_back_ios</span></a>
                            <a class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm disabled" runat="server" id="btnNextclk" title="next" onclick="callserverbtn('btnnext');"><span id="spanNextclk" runat="server" class="material-icons material-icons-style material-icons-3">arrow_forward_ios</span></a>
                            <asp:Button runat="server" class="btn btn-default btn-sm d-none" ID="btnprevi" title="previous" OnClick="btnPriv_Click" TabIndex="0"></asp:Button>
                            <asp:Button runat="server" class="btn btn-default btn-sm d-none" ID="btnnext" title="next" OnClick="btnNext_Click" TabIndex="0"></asp:Button>

                        </div>
                    </div>
                </div>

                <div class="control-group" id="progressArea" runat="server" visible="true">
                    <div>
                        <asp:Label ID="lblnodata" runat="server" meta:resourcekey="lblnodata"> No data found.</asp:Label>
                    </div>
                </div>

            </div>
        </div>
        <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
            <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                <span class="loader"></span>
            </div>
        </div>
    </form>
</body>
</html>
