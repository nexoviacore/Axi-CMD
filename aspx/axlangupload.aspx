<%@ Page Language="C#" AutoEventWireup="true" CodeFile="axlangupload.aspx.cs" Inherits="aspx_axlangupload" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="File Upload" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Axp File Upload </title>

    <link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" id="generic" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/iFrameHandler.min.js"></script>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" type="text/css" />
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <link href="../ThirdParty/Linearicons/Font/library/linearIcons.css" rel="stylesheet" />

    <%--custom alerts start--%>
    <script src="../Js/alerts.min.js?v=30" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/gen.min.js?v=13" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script src="../Js/tstruct.min.js?v=673" type="text/javascript"></script>

    <script src="../Js/axlangupload.min.js?v=9" type="text/javascript"></script>

    <link href="../Css/tstructNewUi.min.css?v=60" rel="stylesheet" />
    <link href="../Css/globalStyles.min.css?v=35" rel="stylesheet" type="text/css" />

</head>
<body dir='<%=direction%>'>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="dcContent">
            <div class="lablattactxt">
                <table class="gridDatauploadtxt">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblslctfilename" runat="server">Please select a '<%=upFileType %>' and then click 'Upload' button.</asp:Label>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="clear"></div>
            <div>
                <div class="file-upload">
                    <div tabindex="0" class="file-select">
                        <div class="file-select-button" id="fileName">
                            <asp:Label ID="lblfilename" runat="server">Choose File</asp:Label>
                        </div>
                        <div class="file-select-name" id="noFile">
                            <asp:Label ID="lblnofilename" runat="server">No file chosen...</asp:Label>
                        </div>
                        <input runat="server" type="file" tabindex="-1" name="filMyFile" id="filMyFile" accept=".xlsx" />
                    </div>
                </div>
            </div>
            <asp:Label ID="fileuploadsts" Text="" runat="server"></asp:Label>
            <asp:HiddenField ID="hdnFilePath" Value="" runat="server" />
            <asp:HiddenField ID="hdnUpFileType" Value="" runat="server" />
            <asp:HiddenField ID="upsts" Value="" runat="server" />
            <asp:Label ID="lblFileUp" runat="server" Visible="false">File uploaded successfully!</asp:Label>
            <asp:Label ID="lblfilecn" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblAnError" runat="server" Visible="false">An Error Occured. Please Try Again!</asp:Label>
        </div>
        <div class="row">
            <asp:Button ID="cmdSend" class="hotbtn btn handCursor" runat="server" Text="Upload" OnClick="cmdSend_Click" OnClientClick="javascript: return AllowAttachements();" ToolTip="Upload" />&nbsp;
            <input name="close" type="button" id="close" value="Close" title="Close" class="coldbtn btn handCursor" onclick="closeUploadDialog();">
        </div>
    </form>


</body>
</html>
