<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DgsignSkipUser.aspx.cs" Inherits="aspx_Dgsigndetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../App_Themes/Gray/Stylesheet.min.css?v=23" rel="stylesheet" />
    <link href="../Css/genericold.min.css" rel="stylesheet" type="text/css" id="generic" />
    <script src="../Js/jQueryUi/jquery-1.6.2.js" type="text/javascript"></script>
    <script src="../Js/noConflict.js" type="text/javascript"></script>
    <%--custom alerts start--%>
    <link href="../Css/animate.css" rel="stylesheet" />
    <link href="../Css/alerts.css" rel="stylesheet" />
    <script src="../Js/alerts.min.js?v=33" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/jquery.msgBox.min.js?v=2" type="text/javascript"></script>
    <script type="text/javascript">
        function ShowMessageBox(msg) {
            //  $j.msgBox({ content: msg, type: "info" }, 100);
            showAlertDialog("info", msg);
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="breadcrumb-panel">
            <div class="icon-services bcrumb h3 center">Users to Skip Signing</div>

        </div>
        <div>
            <asp:GridView ID="grvDsignFiles" OnRowDataBound="grvDsignFiles_OnRowDataBound" AutoGenerateColumns="false" CssClass="gridData" runat="server" OnRowCommand="grvDsignFiles_OnRowCommand">
                <Columns>
                    <asp:TemplateField Visible="false" HeaderText="TransID">
                        <ItemTemplate>
                            <asp:Label ID="lblstransid" runat="server" Text='<%#Eval("stransid") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Document Name">
                        <ItemTemplate>
                            <asp:Label ID="lbldocumentname" runat="server" Text='<%#Eval("documentname") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="User Name">
                        <ItemTemplate>
                            <asp:Label ID="lblusername" runat="server" Text='<%#Eval("username") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Order No">
                        <ItemTemplate>
                            <asp:Label ID="lblordno" runat="server" Text='<%#Eval("ordno") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblstatus" runat="server" Text='<%#Eval("status") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" HeaderText="doctype">
                        <ItemTemplate>
                            <asp:Label ID="lbldoctype" runat="server" Text='<%#Eval("doctype") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Skip">
                        <ItemTemplate>
                            <asp:Button ID="btnskip" runat="server"
                                CommandName="Skip" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="Skip"></asp:Button>
                            <asp:Label ID="lblskippedstatus" Visible="false" runat="server" Text="User Skipped" />

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>
    </form>
    <p>
    </p>
</body>
</html>
