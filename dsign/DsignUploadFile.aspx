<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DsignUploadFile.aspx.cs" Inherits="aspx_UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Css/button.css" rel="stylesheet" />
    <script src="../Js/jQueryUi/jquery-1.6.2.js" type="text/javascript"></script>
    <script src="../Js/noConflict.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseAttachPopup() {
            var filename = $j('#hdnOrginalfileName').val();
            var fldId = $j('#hdnAttachTextID').val();
            $j("#" + fldId, window.opener.document).val(filename);
            UpdateArray(fldId, filename);
            window.close();
        }


        function UpdateArray(fldName, fldValue) {
            var isAlreadyFound = false;
            for (var x = 0; x < window.opener.ChangedFields.length; x++) {

                var fName = window.opener.ChangedFields[x].toString();
                if (fName == fldName) {
                    if (fldValue == "***") {
                        window.opener.ChangedFields.splice(x, 1);
                        window.opener.ChangedFieldDbRowNo.splice(x, 1);
                        window.opener.ChangedFieldValues.splice(x, 1);
                        window.opener.ChangedFieldOldValues.splice(x, 1);
                    }
                    else {
                        window.opener.ChangedFieldOldValues[x] = window.opener.ChangedFieldValues[x].toString();
                        window.opener.ChangedFieldDbRowNo[x] = window.opener.ChangedFieldDbRowNo[x];
                        window.opener.ChangedFieldValues[x] = fldValue;
                    }
                    isAlreadyFound = true; // the field name is already found and updated.
                    break;
                }
            }

            if ((!isAlreadyFound) && (fldValue != "***")) {
                var fIndx = fldName.lastIndexOf("F");
                var rowNo = fldName.substring(fIndx - 3, fIndx);
                var dcNo = fldName.substring(fIndx + 1);
                var dbRowNo = parseInt(rowNo, 10);
                window.opener.ChangedFields.push(fldName);
                window.opener.ChangedFieldDbRowNo.push(dbRowNo);
                window.opener.ChangedFieldValues.push(fldValue);
                window.opener.ChangedFieldOldValues.push("");
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dcContent">

            <div class="clear"></div>
            <table width="100%" class="gridDatafile">
                <tr>
                    <td>Select a File :
                    </td>
                    <td>
                        <input id="inpPdfFile" accept="application/pdf" type="file" runat="server" />
                    </td>
                </tr>
            </table>
            <div class="clear"></div>
            <table width="100%" class="gridDatafile">
                <tr>
                    <td colspan="2">Select only Pdf file's.
                    </td>
                </tr>
                <tr></tr>
            </table>
            <asp:Label ID="fileuploadsts" Text="" runat="server" ForeColor="#DB2222"></asp:Label>

            <asp:HiddenField ID="hdnOrginalfileName" runat="server" />
            <asp:HiddenField ID="hdnAttachTextID" runat="server" />


        </div>
        <table width="100%" class="gridData">
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnAttach" class="hotbtn btn" runat="server" Text="Attach" OnClick="btnAttach_click" />&nbsp;
                     <input type="button" value="close" onclick="CloseAttachPopup();" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

