<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DsignLogin.aspx.cs" Inherits="aspx_DsignLogin" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body onload="window.document.form1.submit();">
    <form name="form1" method="post" action="../aspx/SigninInter.aspx">
    <div>
    <%=strParams.ToString() %>
    </div>
        <%if (Request.Browser.Browser == "Firefox"){%>
    <script type="text/javascript">  window.document.form1.submit(); </script>
    <%}%>
    </form>
</body>
</html>
