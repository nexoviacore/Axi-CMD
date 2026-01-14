<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ivtoivload.aspx.vb" Inherits="ivtoivload" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Load IView"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script type="text/javascript" src="../Js/thirdparty/jquery/3.1.1/jquery.min.js"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/common.min.js?v=158"></script>
    <title>Iview Load</title>
</head>

<body class="PageBody" onload="window.document.f1.submit();">

<form name="f1" method="post" action="iview.aspx?ivname=<%=iname %>" dir="<%=direction%>">
<input type="hidden" id="HdnName" value="ivtoivload"/>
<%=resXhtm%>
<%If Request.Browser.Browser = "Firefox" Then%>
    <script type="text/javascript">        window.document.f1.submit(); </script>
    <%End If%>
</form>

    
</body>
</html>
