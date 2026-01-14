<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Help.aspx.cs" Inherits="aspx_Help" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8"/> 
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>    
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content="Help" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <title>Help</title>
</head>

<body>
    <form id="form1" runat="server">
        <asp:Literal runat="server" ID="ltHTMLhelper"></asp:Literal>

        <script type="text/javascript">
            function resizeIFrameToFitContent(iFrame) {
               iFrame.height = document.documentElement.scrollHeight - 25;
            }
            window.addEventListener('DOMContentLoaded', function (e) {
                var iFrame = document.getElementById('docViewer');
                if(iFrame){
                    resizeIFrameToFitContent(iFrame);
                } 
            });
        </script>
    </form>
</body>
</html>
