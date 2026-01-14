<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AxCustomError.aspx.cs" Inherits="aspx_AxCustomError" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert Tstruct" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Custom Error</title>
    <%--<link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" />--%>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>

    <!-- Forcefully closing the Dimmer -->
    <script type="text/javascript">
        closeParentFrame();
    </script>
</head>
<body onload="closeParentFrame();" dir='<%=direction%>'>
    <form id="form1" runat="server">
        <div class="d-flex justify-content-center align-items-center p-10 vh-100">
            <h1 class="me-3 my-0 align-top inline-block align-content-center">Error</h1>
            <div class="inline-block align-middle ps-3 py-2 border-danger border-start">
                <asp:Label ID="lblOops" runat="server" meta:resourcekey="lblOops" CssClass="my-0 font-weight-normal lead"> Oops! there seems to be some problem. Please try later.</asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
