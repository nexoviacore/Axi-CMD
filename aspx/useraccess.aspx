<%@ Page Language="VB" AutoEventWireup="false" CodeFile="useraccess.aspx.vb" Inherits="pvtpages_useraccess" ValidateRequest="false"  EnableEventValidation="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>User Access</title>

    <asp:PlaceHolder runat="server">
        <%:Styles.Render(If(direction = "ltr", "~/UI/axpertUI/ltrBundleCss", "~/UI/axpertUI/rtlBundleCss")) %>
    </asp:PlaceHolder>

    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script type="text/javascript" src="../Js/noConflict.min.js?v=1"></script>
    <script type="text/javascript" src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>
    <script type="text/javascript" src="../Js/common.min.js?v=158"></script>
    <script type="text/javascript" src="../Js/alerts.min.js?v=32"></script>
    <script type="text/javascript" src="../Js/helper.min.js?v=150"></script>
    <script type="text/javascript" src="../Js/useraccess.min.js?v=15"></script>
    <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>
    <script type="text/javascript" src="../ThirdParty/DataTables-1.10.13/media/js/jquery.dataTables.min.js"></script>

    <script>
        var transid = '<%=Request.QueryString("transid")%>'
    </script>

    <%--<script>
        if (typeof localStorage != "undefined") {
            var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            var lsTimeStamp = localStorage["customGlobalStylesExist-" + projUrl]
            if (lsTimeStamp && lsTimeStamp != "false") {
                var appProjName = localStorage["projInfo-" + projUrl] || "";
                var customGS = "<link id=\"customGlobalStyles\" data-proj=\"" + appProjName + "\" href=\"../" + appProjName + "/customGlobalStyles.css?v=" + lsTimeStamp + "\" rel=\"stylesheet\" />";
                document.write(customGS);
            }
        }
    </script>
    <script>
        try {
            if (typeof localStorage != "undefined") {
                var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                var lsTimeStamp = localStorage["axGlobalThemeStyle-" + projUrl]
                if (lsTimeStamp && lsTimeStamp != "false") {
                    var axThemeFldr = localStorage["axThemeFldr-" + projUrl] || "";
                    var axCustomStyle = "<link id=\"axGlobalThemeStyle\" data-themfld=\"" + axThemeFldr + "\" href=\"../" + axThemeFldr + "/axGlobalThemeStyle.css?v=" + lsTimeStamp + "\" rel=\"stylesheet\" />";
                    document.write(axCustomStyle);
                }
            }
        } catch (ex) { }
    </script>--%>
    <script>
            (function () {
                if (typeof localStorage !== "undefined") {
                    try {
                        let projUrl = top.window.location.href.toLowerCase().split("/aspx/")[0];
                        let lsTimeStamp = sanitizeInput(localStorage["customGlobalStylesExist-" + projUrl] || "");
                        let appProjName = sanitizeInput(localStorage["projInfo-" + projUrl] || "");
                        if (lsTimeStamp && lsTimeStamp !== "false" && appProjName) {
                            let linkElement = document.createElement("link");
                            linkElement.id = "customGlobalStyles";
                            linkElement.setAttribute("data-proj", appProjName);
                            linkElement.rel = "stylesheet";
                            let safeHref = encodeURI(`../${appProjName}/customGlobalStyles.css?v=${lsTimeStamp}`);
                            linkElement.href = safeHref;
                            document.head.appendChild(linkElement);
                        }

                        let themeTimeStamp = sanitizeInput(localStorage["axGlobalThemeStyle-" + projUrl] || "");
                        let axThemeFldr = sanitizeInput(localStorage["axThemeFldr-" + projUrl] || "");
                        if (themeTimeStamp && themeTimeStamp !== "false" && axThemeFldr) {
                            let themeLink = document.createElement("link");
                            themeLink.id = "axGlobalThemeStyle";
                            themeLink.setAttribute("data-themfld", axThemeFldr);
                            themeLink.rel = "stylesheet";
                            let safeHref = encodeURI(`../${axThemeFldr}/axGlobalThemeStyle.css?v=${themeTimeStamp}`);
                            themeLink.href = safeHref;
                            document.head.appendChild(themeLink);
                        }
                    } catch (ex) {
                    }
                }
                function sanitizeInput(input) {
                    return input.replace(/[^a-zA-Z0-9_\-\/]/g, "");
                }
            })();
    </script>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
</head>
<body id="UserAccess_Content" class="Family btextDir-<%=direction%>" dir="<%=direction%>">
    <form id="form1" runat="server">
        <div class="content form-horizontal">
            <div class="view-content">
                <asp:ScriptManager runat="server" />
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <ul class="nav nav-tabs nav-pills border-0 me-5 mb-3 mb-md-0 fs-6" role="tablist">
                            <li id="liViewControl" class="nav-item w-50 me-0 mb-md-2" role="presentation">
                                <a href="#ViewControl" data-bs-toggle="tab" id="aViewControl" class="nav-link w-100 btn btn-flex btn-active-light active">View Control</a>
                            </li>
                            <li id="liTransactionControl" class="nav-item w-50 me-0 mb-md-2 d-none" role="presentation">
                                <a href="#TransactionControl" data-bs-toggle="tab" id="aTransactionControl" class="nav-link w-100 btn btn-flex btn-active-light">Transaction Control</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane fade show active" id="ViewControl" role="tabpanel">
                                <ul class="nav nav-tabs nav-line-tabs mb-5 fs-6" role="tablist">
                                    <li class="nav-item" role="presentation"><a class="nav-link active" href="#DCs" data-bs-toggle="tab" id="aDCs">DCs</a></li>
                                    <li class="nav-item" role="presentation"><a class="nav-link" href="#Fields" data-bs-toggle="tab" id="aFields">Fields</a></li>
                                    <li class="nav-item" role="presentation"><a class="nav-link" href="#Buttons" data-bs-toggle="tab" id="aButtons">Buttons</a></li>
                                    <li class="nav-item" role="presentation"><a class="nav-link" href="#ListviewButtons" data-bs-toggle="tab" id="aListViewButtons">Listview Buttons</a></li>
                                </ul>
                                <div class="tab-content UserAccess-Table fs-5">
                                    <div class="tab-pane fade show active" id="DCs" role="tabpanel">
                                        <div id="dc_div" runat="server" class="userTbl">
                                        </div>
                                        <asp:HiddenField ID="dc_xml" runat="server"></asp:HiddenField>
                                    </div>
                                    <div class="tab-pane fade" id="Fields" role="tabpanel">
                                        <div id="fields_div" runat="server" class="userTbl">
                                        </div>
                                        <asp:HiddenField ID="fld_xml" runat="server"></asp:HiddenField>
                                    </div>
                                    <div class="tab-pane fade" id="Buttons" role="tabpanel">
                                        <div id="but_div" runat="server" class="userTbl">
                                        </div>
                                        <asp:HiddenField ID="but_xml" runat="server" />
                                    </div>
                                    <div class="tab-pane fade" id="ListviewButtons" role="tabpanel">
                                        <div id="lv_div" runat="server" class="userTbl">
                                        </div>
                                        <asp:HiddenField ID="lv_xml" runat="server"></asp:HiddenField>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade d-none" id="TransactionControl" role="tabpanel">
                                <div class="container">
                                    <a id="tcadd" class="btn btn-icon btn-sm btn-light float-end" title="Add" href="javascript:void(0);">
                                        <span class="material-icons material-icons-style text-info">add</span>
                                    </a>
                                    <br />
                                    <table id="tblTC" class="table table-sm table-row-bordered w-100 gridData fs-5" cellspacing="0">
                                        <thead>
                                            <tr class="col">
                                                <th class="col-7 text-center">Expression</th>
                                                <th class="col-1">View</th>
                                                <th class="col-1">Edit</th>
                                                <th class="col-1">Delete</th>
                                                <th class="col-2"></th>
                                            </tr>
                                        </thead>
                                    </table>
                                    <div id="tc_expr" runat="server" visible="false" class="h-280px">
                                        <asp:GridView ID="tc_expr_gv" runat="server" AutoGenerateColumns="False" CssClass="gridData" AutoGenerateDeleteButton="true" AutoGenerateEditButton="true">
                                            
                                        </asp:GridView>
                                    </div>

                                    <div id="tc_flds" runat="server" class="h-280px fs-5 d-none">
                                        <div class="mb-3">
                                            <div class="input-group-- mb-3 d-flex">
                                                <label class="form-label col-form-label col-3" id="lblFieldColumn">Select Field/Column</label>
                                                <asp:DropDownList ID="tc_fld_cb" runat="server" class="form-control select2-ddl">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="input-group-- mb-3 d-flex">
                                                <label class="form-label col-form-label col-3" id="lblOperator">Operator</label>
                                                <asp:DropDownList ID="tc_opr_cb" runat="server" AutoPostBack="false" class="form-control select2-ddl">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Text="Equal to"></asp:ListItem>
                                                    <asp:ListItem Text="Not Equal to"></asp:ListItem>
                                                    <asp:ListItem Text="Less Than"></asp:ListItem>
                                                    <asp:ListItem Text="Less Than or Equal to"></asp:ListItem>
                                                    <asp:ListItem Text="Greater Than"></asp:ListItem>
                                                    <asp:ListItem Text="Greater Than or Equal to"></asp:ListItem>
                                                    <asp:ListItem Text="Is Empty"></asp:ListItem>
                                                    <asp:ListItem Text="Between"></asp:ListItem>
                                                    <asp:ListItem Text="Contains"></asp:ListItem>
                                                    <asp:ListItem Text="Not Contains"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="input-group-- mb-3 d-flex">
                                                <label class="form-label col-form-label col-3" id="lblValues">Values</label>
                                                <asp:DropDownList ID="tc_values_cb" runat="server" class="form-control select2-ddl">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="input-group-- mb-3 d-flex justify-content-between">
                                                <label class="form-label col-form-label col-3" id="lblValuesBtw">Values</label>
                                                <asp:DropDownList ID="tc_betvalues_cb" runat="server" Enabled="false" class="form-control select2-ddl">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="d-flex mb-3 justify-content-between">
                                                <div class="d-flex gap-2">
                                                    <asp:CheckBox ID="tc_view_cb" runat="server" CssClass="w-15px h-15px my-auto" Checked="true" />
                                                    <label for="tc_view_cb" class="form-label col-form-label">View</label>
                                                </div>
                                                <div class="d-flex gap-2">
                                                    <asp:CheckBox ID="tc_edit_cb" runat="server" CssClass="w-15px h-15px my-auto" Checked="true" />
                                                    <label for="tc_edit_cb" class="form-label col-form-label">Edit</label>
                                                </div>
                                                <div class="d-flex gap-2">
                                                    <asp:CheckBox ID="tc_delete_cb" runat="server" CssClass="w-15px h-15px my-auto" Checked="true" />
                                                    <label for="tc_delete_cb" class="form-label col-form-label">Delete</label>
                                                </div>
                                            </div>
                                            <div class="d-flex mb-3 gap-2" id="divTCActions">
                                                <asp:HiddenField ID="editmode" Value="" runat="server" />
                                                <input type="button" id="btnTCOK" name="name" value="Ok" class="btn btn-primary btn-sm" onclick="validateTransactionControl()" />
                                                <input type="button" id="btnTCCancel" name="name" value="Cancel" class="btn btn-white btn-sm shadow-sm" onclick="cancelTransactionControl()" />

                                                <asp:Button ID="tc_ok" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClientClick="return validateTransactionControl()" Visible="false" />
                                                <asp:Button ID="tc_cancel" runat="server" Text="Cancel" CssClass="btn btn-sm btn-white shadow-sm" OnClick="tc_cancel_Click" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <asp:HiddenField runat="server" ID="hdnFormChanges" Value="false" />
                        <asp:HiddenField runat="server" ID="hdnUsrAccessRes" Value="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="d-flex gap-3 float-end dialog-footer">
            <asp:Button runat="server" ID="save" Text="Save" CssClass="btn btn-primary btn-sm" AutoPostBack="true"/>
            <input type="button" id="btnClose" name="Close" value="Close" class="btn btn-white btn-sm shadow-sm" onclick="closeWindow()" />
        </div>

        <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
            <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                <span class="loader"></span>
            </div>
        </div>

    </form>


</body>
</html>
