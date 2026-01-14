<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportNew.aspx.cs" Inherits="aspx_ImportNew" EnableEventValidation="false" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Import" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <title>Import Data</title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction=="ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss" ) %>
    </asp:PlaceHolder>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <%--<script type="text/javascript">
        if (typeof localStorage != "undefined") {
            var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            var lsTimeStamp = localStorage["customGlobalStylesExist-" + projUrl]
            if (lsTimeStamp && lsTimeStamp != "false") {
                var appProjName = localStorage["projInfo-" + projUrl] || "";
                var customGS = "<link id=\"customGlobalStyles\" data-proj=\"" + appProjName + "\" href=\"../" + appProjName + "/customGlobalStyles.css?v=" + lsTimeStamp + "\" rel=\"stylesheet\" />";
                document.write(customGS);
            }
        }

        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
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
    <script type="text/javascript">
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <style>
        .noHover {
            pointer-events: none;
        }
    </style>
</head>

<body dir='<%=direction%>' class="btextDir-<%=direction%> iframeScrollFix content p-0 fs-6" id="dvimportnew">
    <form id="form1" runat="server">
        <asp:PlaceHolder runat="server">
            <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
        </asp:PlaceHolder>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
                <asp:ServiceReference Path="../CustomWebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <asp:HiddenField ID="axRuleDetails" runat="server" />
        <asp:HiddenField ID="axRuleScript" runat="server" />
        <asp:HiddenField ID="axRuleOnSubmit" runat="server" />
        <div class="stepper stepper-pills card bg-transparent border-0 h-100" id="kt_stepper_example_clickable" data-kt-stepper="true">
            <!--begin::Nav-->
            <div class="card-header d-block px-0 py-5 bg-transparent border-0 mx-20">
                <div class="stepper-nav bg-white rounded-2 flex-center flex-wrap">
                    <!--begin::Step 1-->
                    <div class="stepper-item mx-2 my-4 current" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">1</span>
                        </div>
                        <div class="stepper-label">
                            <h3 class="stepper-title">Data Search </h3>
                        </div>
                    </div>
                    <!--end::Step 1-->

                    <!--begin::Step 2-->
                    <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">2</span>
                        </div>
                        <div class="stepper-label">
                            <h3 class="stepper-title">Upload</h3>
                        </div>
                    </div>
                    <!--end::Step 2-->

                    <!--begin::Step 3-->
                    <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">3</span>
                        </div>
                        <div class="stepper-label">
                            <h3 class="stepper-title">Edit</h3>
                        </div>
                    </div>
                    <!--end::Step 3-->

                    <!--begin::Step 4-->
                    <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">4</span>
                        </div>
                        <div class="stepper-label">
                            <h3 class="stepper-title">Summary</h3>
                        </div>
                    </div>
                    <!--end::Step 4-->
                </div>
            </div>
            <div class="card-body min-h-100px mx-20 overflow-auto pt-0">
                <div class="form w-100 w-lg-700px mx-auto" novalidate="novalidate" id="kt_stepper_example_basic_form">
                    <!--begin::Group-->
                    <div class="mb-5">
                        <div class="flex-column current" data-kt-stepper-element="content">
                            <%--Step 1--%>
                            <div id="wizardBodyContent">
                                <!-- Widget Data Search - begins -->
                                <div id="imWizardDataSearch">
                                    <section class="form-group col-md-12 mb-4">
                                        <asp:Label runat="server" AssociatedControlID="ddlImpTbl" class="form-label col-form-label pb-1 fw-boldest required">
                                            <asp:Label ID="lblImptbl" runat="server" meta:resourcekey="lblImptbl" Text="Select Form"></asp:Label>
                                        </asp:Label>
                                        <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl1" data-bs-content="Select a TStruct from which the data needs to be imported." data-bs-placement="right">help_outline</i>
                                        <asp:DropDownList runat="server" ID="ddlImpTbl" CssClass="form-select forValidation selectpicker" data-live-search="true" data-size="6" AutoPostBack="true" placeholder="Select Form" OnSelectedIndexChanged="ddlImpTbl_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </section>

                                    <section class="form-group col-md-12 mb-4">
                                        <asp:Label runat="server" AssociatedControlID="ddlImpTemplate" class="form-label col-form-label pb-1 fw-boldest">
                                            <asp:Label ID="lblImpTemplate" runat="server" meta:resourcekey="lblImpTemplate" Text="Select Template">
                                            </asp:Label>
                                        </asp:Label>
                                        <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl0" data-bs-content="Select a template for the form selected to be imported." data-bs-placement="right">help_outline</i>
                                        <div class="d-flex flex-row">
                                            <asp:DropDownList runat="server" ID="ddlImpTemplate" CssClass="form-select forValidation selectpicker" data-live-search="true" data-size="6" AutoPostBack="false">
                                            </asp:DropDownList>
                                            <div id="ddlImpTempDel" onclick="deleteTemplate();" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm ms-2 p-6" title="Delete Template">
                                                <span class="material-icons">delete</span>
                                            </div>
                                        </div>
                                    </section>
                                    <div id="dataupdatediv" class="form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4">
                                        <div class="controls">
                                            <div class="input-icon left d-flex justify-content-center customclscol">
                                                <input name="dataupdate" type="checkbox" id="dataupdatecheck" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" title="Data Update" meta:resourcekey="lblDataUpdate" runat="server">
                                                <span id="DataUpdate" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="ChkColNameInfile" runat="server">Data Update</span>
                                                <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip" id="headertipuptdata" data-bs-original-title="Enable this only if you want to update existing data." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                            </div>
                                        </div>
                                    </div>
                                    <section class="form-group col-md-12 mb-4 d-none" id="dataupdate">
                                        <asp:Label runat="server" AssociatedControlID="tstFlds" class="form-label col-form-label pb-1 fw-boldest">
                                            <asp:Label ID="lblImpPrimary" runat="server" meta:resourcekey="lblImpPrimary" Text="Select Primary Field">
                                            </asp:Label>
                                        </asp:Label>
                                        <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl11" data-bs-content="Select a field you want to update." data-bs-placement="right">help_outline</i>
                                        <div class="d-flex flex-row" id="primaryfld">
                                            <asp:DropDownList runat="server" ID="tstFlds" CssClass="form-select forValidation selectpicker" data-live-search="true" data-size="6" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </div>
                                    </section>

                                    <div id="axstaysignin" class="form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4">
                                        <div class="controls">
                                            <div class="input-icon left d-flex justify-content-center customclscol">
                                                <input name="signedin" type="checkbox" id="ChkColNameInfile" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" title="File contains Headers" checked="checked" meta:resourcekey="lblFileHeaders" runat="server">
                                                <span id="lblstaysin" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="ChkColNameInfile" runat="server">File contains Headers</span>
                                                <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip" id="headertip" data-bs-original-title="Enable this after uploading the file only." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                            </div>
                                        </div>
                                    </div>

                                    <section class="form-group col-md-12">
                                        <label class="form-label col-form-label pb-1 fw-boldest required" for="txtExFields" onclick="$('.ms-selectable ul.ms-list').focus();">
                                            <asp:Label ID="lbltxtim" runat="server" meta:resourcekey="lbltxtim" Text="Fields">
                                            </asp:Label>
                                        </label>
                                        <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl2" data-bs-content="Select the field(s) to be included in the import template." data-bs-placement="right">help_outline</i>
                                        <table class="table table-borderless w-100">
                                            <tr class="d-flex flex-row-auto flex-center">
                                                <td class="d-block d-sm-table-cell col-12 col-sm-5 p-0">
                                                    <select id="mSelectLeft" name="from[]" class="multiselect form-control scroll-x" size="9" multiple="multiple" data-right="#mSelectRight" data-right-all="#right_All_1" data-right-selected="#right_Selected_1" data-left-all="#left_All_1" data-left-selected="#left_Selected_1">
                                                        <asp:Repeater ID="rptSelectFields" runat="server">
                                                            <ItemTemplate>
                                                                <option value='<%# Container.DataItem.ToString().Substring(0,Container.DataItem.ToString().IndexOf("&&")) %>'>
                                                                    <%# Container.DataItem.ToString().Substring(Container.DataItem.ToString().IndexOf("&&")+2)%>
                                                                </option>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </select>
                                                </td>
                                                <td class="d-block d-sm-table-cell col-12 col-sm-2 py-0">
                                                    <%--button icons are updating based on the lang selection <,>, <<,>>--%>
                                                    <div class="text-center py-1">
                                                        <a href="javascript:void(0);" id="right_All_1" title="Select All" class="btn btn-sm btn-icon btn-active-primary shadow-sm p-5">
                                                            <span class="material-icons material-icons-style">keyboard_double_arrow_right</span>
                                                        </a>
                                                    </div>
                                                    <div class="text-center py-1">
                                                        <a href="javascript:void(0);" id="right_Selected_1" title="Select" class="btn btn-sm btn-icon btn-active-primary shadow-sm p-5">
                                                            <span class="material-icons material-icons-style">keyboard_arrow_right</span>
                                                        </a>
                                                    </div>
                                                    <div class="text-center py-1">
                                                        <a href="javascript:void(0);" id="left_Selected_1" title="Unselect" class="btn btn-sm btn-icon btn-active-primary shadow-sm p-5">
                                                            <span class="material-icons material-icons-style">keyboard_arrow_left</span>
                                                        </a>
                                                    </div>
                                                    <div class="text-center py-1">
                                                        <a href="javascript:void(0);" id="left_All_1" title="Unselect All" class="btn btn-sm btn-icon btn-active-primary shadow-sm p-5">
                                                            <span class="material-icons material-icons-style">keyboard_double_arrow_left</span>
                                                        </a>
                                                    </div>
                                                    <div class="text-center py-1">
                                                        <a href="javascript:void(0);" id="reorder" title="Re-order" class="btn btn-sm btn-icon btn-active-primary shadow-sm p-5">
                                                            <span class="material-icons material-icons-style">low_priority</span>
                                                        </a>
                                                    </div>
                                                </td>
                                                <td class="d-block d-sm-table-cell col-12 col-sm-5 p-0">
                                                    <select name="to[]" id="mSelectRight" class="multiselect form-control scroll-x" size="9" multiple="multiple">
                                                        <asp:Repeater ID="rptTemplateFields" runat="server">
                                                            <ItemTemplate>
                                                                <option value='<%# Container.DataItem.ToString().Substring(0,Container.DataItem.ToString().IndexOf("&&")) %>'>
                                                                    <%# Container.DataItem.ToString().Substring(Container.DataItem.ToString().IndexOf("&&")+2)%>
                                                                </option>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </select>
                                                </td>
                                            </tr>
                                        </table>
                                    </section>

                                    <section class="col-md-4 col-sm-12 upload-section d-flex justify-content-center py-4" style="padding-left: 10px;">
                                        <div class="btn btn-icon btn-active-light-primary w-30px h-30px w-md-40px h-md-40px position-relative" data-kt-menu-trigger="click" data-kt-menu-attach="parent" data-kt-menu-placement="bottom" data-kt-menu-flip="top-end">
                                            <button class="btn btn-primary dropdown-toggle" id="lnkExpTemp" type="button" data-bs-toggle="dropdown" aria-expanded="true">
                                                Download Data Template
            <span class="caret"></span>
                                            </button>
                                            <span class="material-icons material-icons-style material-icons-2 align-middle ms-2" tabindex="0" id="datetip" data-bs-toggle="tooltip" data-bs-original-title="1. Date should be in DD/MM/YYYY or MM/DD/YYYY format <br> 2. Make sure Excel Data Columns should not have Datatype as 'Custom' (recommended Datatype is 'Text') especially when Excel files don't have Column Headers." data-bs-placement="bottom" data-bs-html="true" data-bs-dismiss="click">help_outline</span>
                                        </div>
                                        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg menu-state-primary fw-bold py-4 fs-6 w-200px" data-kt-menu="true">
                                            <div id="exceldiv" class="menu-item px-3">
                                                <a href="javascript:void(0)" id="excel1" class="menu-link">Excel</a>
                                            </div>
                                            <div id="csvdiv" class="menu-item px-3">
                                                <a href="javascript:void(0)" id="CSV1" class="menu-link">CSV</a>
                                            </div>
                                        </div>

                                        <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2 d-none" data-bs-toggle="popover" id="icocl3" data-bs-content="Template should be available in .CSV format" data-bs-placement="right">help_outline</i>
                                        <br />
                                        <asp:Label ID="lblTemplateNtAvalble" runat="server" Text="" class="lblleft" ForeColor="#cf4444" Visible="false"></asp:Label>
                                    </section>

                                    <asp:UpdatePanel runat="server" ID="plnUpdate1">
                                        <ContentTemplate>
                                            <asp:Button Text="text" class="btn btn-primary btn-icon d-none" runat="server" ID="btnCreateTemplate" OnClick="btnCreateTemplate_Click" />
                                            <asp:Button Text="text" class="btn btn-primary btn-icon d-none" runat="server" ID="ColHeaderClick" OnClick="btnColHeader_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:HiddenField ID="hdnMandatoryColCount" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnMandatoryFields" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnSelectedColumnCount" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnColValues" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnColNames" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnTransid" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnLeftFlds" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnCheckHeaders" runat="server" Value="" />
                                </div>
                            </div>
                        </div>

                        <div class="flex-column" data-kt-stepper-element="content">
                            <%--Step 2--%>
                            <div class="row" id="imWizardUpload">
                                <span class="customMessage fileUploadErrorMessage"></span>
                                <%--<section class="col-md-4 col-sm-12 upload-section d-flex justify-content-center py-4">
                                    <div class="btn btn-icon btn-active-light-primary w-30px h-30px w-md-40px h-md-40px position-relative" data-kt-menu-trigger="click" data-kt-menu-attach="parent" data-kt-menu-placement="bottom" data-kt-menu-flip="top-end">
                                        <button class="btn btn-primary dropdown-toggle" id="lnkExpTemp" type="button" data-bs-toggle="dropdown" aria-expanded="true">
                                            Download Data Template
                                            <span class="caret"></span>
                                        </button>
                                        <span class="material-icons material-icons-style material-icons-2 align-middle ms-2" tabindex="0" id="datetip" data-bs-toggle="tooltip" data-bs-original-title="1. Date should be in DD/MM/YYYY or MM/DD/YYYY format <br> 2. Make sure Excel Data Columns should not have Datatype as 'Custom' (recommended Datatype is 'Text') especially when Excel files don't have Column Headers." data-bs-placement="bottom" data-bs-html="true" data-bs-dismiss="click">help_outline</span>
                                    </div>
                                    <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg menu-state-primary fw-bold py-4 fs-6 w-200px" data-kt-menu="true">
                                        <div id="exceldiv" class="menu-item px-3">
                                            <a href="#" id="excel1" class="menu-link">Excel</a>
                                        </div>
                                        <div id="csvdiv" class="menu-item px-3">
                                            <a href="#" id="CSV1" class="menu-link">CSV</a>
                                        </div>
                                    </div>

                                    <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl3" data-bs-content="Template should be available in .CSV format" data-bs-placement="right">help_outline</i>
                                    <br />
                                    <asp:Label ID="lblTemplateNtAvalble" runat="server" Text="" class="lblleft" ForeColor="#cf4444" Visible="false"></asp:Label>
                                </section>--%>

                                <section class="col-md-5 col-sm-12 d-flex justify-content-center py-4">
                                    <div class="btn btn-icon btn-secondary w-30px h-30px w-md-40px h-md-40px position-relative bg-white cursor-default">
                                        <button class="btn btn-secondary dropdown-toggle shadow-sm" id="btnDwnloadDtSrc" type="button" disabled>
                                            Download Data Source
                                            <span class="caret"></span>
                                        </button>
                                    </div>
                                </section>

                                <section class="col-md-3 col-sm-12 d-flex justify-content-center py-4">
                                    <a id="btnImpHistory" href="javascript:void(0);" runat="server" class="btn btn-light-primary shadow-sm" tabindex="0" visible="true" title="Import History" onclick="importHistory();">
                                        <asp:Label ID="lblImpHistory" runat="server" meta:resourcekey="lblImpHistory">Import History</asp:Label>
                                    </a>
                                </section>

                                <section class="col-md-12  d-flex justify-content-center">
                                    <asp:UpdatePanel ID="updatePnl2" class="w-100" runat="server" UpdateMode="conditional">
                                        <ContentTemplate>
                                            <div class="my-4 file-upload ">
                                                <div class="form-group form-control">
                                                    <div id="dropzone_AxpFileImport" class="dropzone dropzone-queue min-h-1px border-0 px-3 py-3">
                                                        <div class="d-flex flex-row-auto flex-center dropzone-panel mb-lg-0 m-0">
                                                            <a class="dropzone-select fs-7">
                                                                <span class="material-icons material-icons-style material-icons-2 float-start mx-2">upload_file</span>
                                                                Drop files here or click to upload
                                                            </a>
                                                            <span class="material-icons material-icons-style material-icons-2 float-end ms-4 fileuploadmore d-none" data-bs-toggle="popover" data-bs-sanitize="false" data-bs-placement="bottom" data-bs-html="true">more</span>
                                                            <a class="dropzone-remove-all btn btn-sm btn-light-primary d-none">Remove All</a>
                                                        </div>
                                                        <div class="dropzone-items wm-200px d-none">
                                                            <div class="dropzone-item" style="display: none">
                                                                <div class="dropzone-file">
                                                                    <div class="dropzone-filename" title="some_image_file_name.jpg">
                                                                        <span data-dz-name>some_image_file_name.jpg</span>
                                                                    </div>
                                                                    <div class="dropzone-error" data-dz-errormessage>
                                                                    </div>
                                                                </div>
                                                                <div class="dropzone-progress d-none">
                                                                    <div class="progress">
                                                                        <div class="progress-bar bg-primary" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0" data-dz-uploadprogress>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="dropzone-toolbar">
                                                                    <span class="dropzone-delete" data-dz-remove>
                                                                        <span class="material-icons material-icons-style material-icons-2 float-end dropzoneItemDelete">clear</span>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <asp:Label ID="fileuploadsts" Text="" runat="server" ForeColor="#DB2222"></asp:Label>
                                                <input type="button" name="name" value="Upload" class=" btn d-none" id="btnFileUpload" />
                                                <asp:Button runat="server" ID="UploadButton" class="btn  btn-primary d-none" Text="Upload" OnClick="UploadButton_Click" />
                                            </div>
                                            <%-- <div id="axstaysignin" class="form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4">
                                                <div class="controls">
                                                    <div class="input-icon left d-flex justify-content-center customclscol">
                                                        <input name="signedin" type="checkbox" id="ChkColNameInfile" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" title="File contains Headers"  checked="checked" meta:resourcekey="lblFileHeaders" runat="server">
                                                        <span id="lblstaysin" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="ChkColNameInfile" runat="server">File contains Headers</span>
                                                        <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip" id="headertip" data-bs-original-title="Enable this after uploading the file only." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </section>

                                <section class="col-xs-12">
                                    <div class="col-xs-12 customclscol">
                                        <div class="form-group upload-progress">
                                            <div id="divProgress" class="progress d-none">
                                                <div id="divProgressBar" class="progress-bar progress-bar-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </section>

                                <div class="form-group col-md-12 row g-3 align-items-center my-2">
                                    <div class="col-sm-6">
                                        <asp:Label class="form-label fw-boldest text-dark fs-6 mb-2" runat="server" ID="lblseparator" meta:resourcekey="lblseparator" AssociatedControlID="ddlSeparator">
                                        </asp:Label>
                                        <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl6" data-bs-content="Select character used for separating columns in the data file." data-bs-placement="right">help_outline</i>
                                        <asp:DropDownList ID="ddlSeparator" CssClass="form-select" runat="server">
                                            <asp:ListItem Value="," Selected="True">Comma [ , ]</asp:ListItem>
                                            <asp:ListItem Value=";">Semicolon [ ; ]</asp:ListItem>
                                            <asp:ListItem Value="|">Pipe [ | ]</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-6 customclscol">
                                        <asp:Label runat="server" class="form-label fw-boldest text-dark fs-6 mb-2" ID="lblimgroupby" meta:resourcekey="lblimgroupby" AssociatedControlID="ddlGroupBy">
                                        </asp:Label>
                                        <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl7" data-bs-content="Form contains data grid, please select column with unique values." data-bs-placement="right">help_outline</i>
                                        <asp:DropDownList ID="ddlGroupBy" CssClass="form-select" runat="server" placeholder="Please select GroupBy Condition">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="flex-column" data-kt-stepper-element="content">
                            <%--Step 3--%>
                            <div id="imWizardEdit">
                                <div class="d-flex justify-content-between mb-5">
                                    <div id="axstaysignin1" class="form-check form-switch form-check-custom form-check-solid">
                                        <div class="controls">
                                            <input type="checkbox" id="chkForIgnoreErr" runat="server" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px align-middle" checked="checked" />
                                            <asp:Label ID="lblForIgnoreErr" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6" AssociatedControlID="chkForIgnoreErr" meta:resourcekey="lblForIgnoreErr" runat="server"> Ignore errors</asp:Label>
                                            <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="tooltip" id="icocl4" data-bs-original-title="Check this to ignore errors in the rows during file upload." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                        </div>
                                    </div>
                                    <div id="axstaysignin2" class="form-check form-switch form-check-custom form-check-solid d-none">
                                        <div class="controls">
                                            <input type="checkbox" id="chkForAllowUpdate" runat="server" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px align-middle" onclick="javascript: ChkAllowUpdate();" />
                                            <asp:Label ID="lblAllowUpdate" AssociatedControlID="chkForAllowUpdate" runat="server" CssClass="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6"> Allow Update </asp:Label>
                                            <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="tooltip" id="icocl5" data-bs-original-title="If allow update is checked,the rows will be updated using the primary key." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                        </div>
                                    </div>
                                    <div class="d-flex justify-content-between my-1">
                                        <asp:Label ID="lblprimarycolmn" runat="server" meta:resourcekey="lblprimarycolmn" CssClass="form-label col-form-label pb-1 fw-boldest d-none">Primary Column
                                        </asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlPrimaryKey" CssClass="form-select form-select-sm w-100px">
                                        </asp:DropDownList>
                                        <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle my-2 me-n7 ms-3" data-bs-toggle="tooltip" data-bs-original-title="Please select a Primary key column to allow update." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                        <asp:HiddenField ID="hdnPrimaryKey" runat="server" Value="" />
                                    </div>
                                </div>

                                <section>
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="importrecord">
                                                <asp:Label ID="lblrecords" CssClass="form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" runat="server" meta:resourcekey="lblrecords">
                                                    Top 5 records
                                                </asp:Label>
                                            </div>
                                            <hr class="text-gray-500" />
                                            <div class="importrecordtable">
                                                <asp:GridView CellSpacing="-1" ID="gridImpData" runat="server" Visible="true" OnRowDataBound="gridImpData_RowDataBound" CssClass="table w-100" BorderStyle="None">
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </section>

                                <section class="col-sm-12">
                                    <asp:HiddenField ID="hdnIgnoredColumns" runat="server" Value="" />
                                    <asp:HiddenField ID="colheader" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnCOLheaders" runat="server" Value="" />
                                </section>
                            </div>
                        </div>

                        <div class="flex-column" data-kt-stepper-element="content">
                            <%--Step 4--%>
                            <div class="d-flex justify-content-center" id="imWizardSummary">
                                <asp:Label runat="server" ID="lblPleaseWait" Visible="false"></asp:Label>
                                <asp:UpdatePanel ID="updatePln3" runat="server">
                                    <ContentTemplate>
                                        <textarea name="text" id="summaryText" runat="server" rows="11" class="resize:none d-none" cols="70"></textarea>
                                        <div class="form-group col-md-12">
                                            <h3>
                                                <asp:Label ID="lblimportsum" runat="server" meta:resourcekey="lblimportsum">Import Summary</asp:Label>
                                            </h3>
                                            <asp:Label runat="server" ID="lblTest"></asp:Label>
                                            <br />
                                            <a id="SummaryDwnld" runat="server" class="btn btn-primary" tabindex="0" visible="false" title="Download Failed Data">
                                                <asp:Label ID="lbldwnldsum" runat="server" meta:resourcekey="lbldwnldsum">Download Failed Data</asp:Label>
                                            </a>
                                            <a id="btnHome" href="javascript:void(0);" runat="server" class="btn btn-light-primary d-inline-flex align-items-center shadow-sm me-2 float-end" tabindex="0" visible="true" title="Back to Data Search" onclick="$(callParentNew('btn-close','class')).click(),callParentNew('DoUtilitiesEvent(ImportData)','function');">Back to Data Search
                                            </a>
                                        </div>
                                        <asp:HiddenField ID="hdnIgnoredColCount" runat="server" Value="0" />
                                        <asp:HiddenField ID="uploadFileName" runat="server" Value="" />
                                        <asp:HiddenField ID="uploadIviewName" runat="server" Value="" />
                                        <asp:HiddenField ID="upFileName" runat="server" Value="" />
                                        <asp:HiddenField ID="IsFileUploaded" runat="server" Value="" />
                                        <asp:HiddenField ID="fileUploadComplete" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdnTemplateName" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnUploadFileWarnings" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnGroupBy" runat="server" Value="NA" />
                                        <asp:HiddenField ID="hdnGroupByColName" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnGroupByColVal" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnCheckHeader" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnOriginalfileName" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnCheckToggleHeader" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnFileHeaderCheck" runat="server" Value="false" />
                                        <asp:HiddenField ID="hdnHeadersWithTypes" runat="server" Value="false" />
                                        <asp:HiddenField ID="hdnTstructflds" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnupdateField" runat="server" Value="" />
                                        <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" CssClass="cloudButton btn btn-primary d-none" OnClientClick="callParentNew('loadFrame();','function');" />
                                        <br />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnImport" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>
                    <!--end::Group-->
                </div>
            </div>

            <div class="card-footer">
                <div class="d-flex d-flex justify-content-end mx-2">
                    <div class="me-2">
                        <button id="btnPrev" type="button" runat="server" class="btn btn-white btn-color-gray-700 btn-active-primary shadow-sm" data-kt-stepper-action="previous">
                            Back
                        </button>
                    </div>

                    <div>
                        <button id="btnSubmit" type="button" class="btn btn-primary shadow-sm" data-kt-stepper-action="submit">
                            <span class="indicator-label">Submit</span>
                            <span class="indicator-progress">Please wait... 
                                <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                            </span>
                        </button>

                        <button id="btnSaveTemplate" type="button" class="btn btn-light-primary shadow-sm" onclick="saveTemplate();">
                            Save Template & Continue                       
                        </button>

                        <button id="btnContinue" type="button" class="btn btn-primary shadow-sm" data-kt-stepper-action="next">
                            Continue
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
            <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                <span class="loader"></span>
            </div>
        </div>

        <script type="text/javascript" src="../Js/thirdparty/jquery-ui/jquery-ui.min.js"></script>
        <script type="text/javascript" src="../Js/noConflict.min.js?v=1"></script>
        <script type="text/javascript" src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>
        <script type="text/javascript" src="../Js/jquery.multi-select.min.js"></script>
        <script type="text/javascript" src="../Js/alerts.min.js?v=32"></script>
        <script type="text/javascript" src="../Js/helper.min.js?v=172"></script>
        <script type="text/javascript" src="../Js/jsclient.min.js?v=118"></script>
        <script type="text/javascript" src="../Js/common.min.js?v=158"></script>
        <script type="text/javascript" src="../Js/AxInterface.min.js?v=16"></script>
        <script type="text/javascript" src="../Js/multiselect.min.js"></script>
        <script type="text/javascript" src="../Js/import.min.js?v=51"></script>
        <script type="text/javascript">
            var proj = '<%=proj%>';
            var sid = '<%=sid%>';
            var userName = '<%=Session["user"]%>';
        </script>

    </form>
</body>

</html>
