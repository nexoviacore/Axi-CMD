<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tsttable.aspx.cs" Inherits="tsttable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta name="description" content="Table" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Table</title>

    <asp:PlaceHolder runat="server">
        <%:Styles.Render("~/Css/form") %>
    </asp:PlaceHolder>
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
    <script type="text/javascript" src="../Js/iFrameHandler.min.js"></script>
    <script src="../Js/common.min.js?v=158"></script>
    <script src="../Js/tstructvars.min.js?v=94"></script>
    <script src="../Js/tstruct.min.js?v=673"></script>
    <script src="../Js/tstTable.min.js?v=34"></script>
</head>
<body dir="<%=direction%>">
    <form id="form1" runat="server">
        <div id="divTable">
        </div>
        <asp:HiddenField ID="hdnfieldId" Value="" runat="server" />
    </form>
</body>
</html>
