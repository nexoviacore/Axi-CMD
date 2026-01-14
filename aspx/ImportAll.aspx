<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportALL.aspx.cs" Inherits="aspx_ImportNew" EnableEventValidation="false" %>

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

    <script type="text/javascript">
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
        var sheetcount = '<%=sheetcount%>';
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
    </script>
    <style>
        .noHover {
            pointer-events: none;
        }
    </style>
</head>

<body dir='<%=direction%>' class="btextDir-<%=direction%> iframeScrollFix content p-0 fs-6">
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
        <asp:HiddenField ID="hdnSheetCount" runat="server" />
        <asp:HiddenField ID="hdnformQueue" runat="server" />
        <asp:HiddenField ID="hdnformQueueVal" runat="server" />
        <div class="stepper stepper-pills card bg-transparent border-0 h-100" id="kt_stepper_example_clickable" data-kt-stepper="true">
            <!--begin::Nav-->
            <div class="card-header d-block px-0 py-5 bg-transparent border-0 mx-20">
                <div class="stepper-nav bg-white rounded-2 flex-center flex-wrap">

                    <%--                                        <div class="stepper-item mx-2 my-4 current" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">1</span>
                        </div>
                        <div class="stepper-label">
                            <h3 class="stepper-title">Start</h3>
                        </div>
                    </div>--%>
                    <!--begin::Step 1-->
                    <div class="stepper-item mx-2 my-4 current" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <%--<div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">1</span>
                        </div>--%>
                        <%--<div class="stepper-label">
                            <h3 class="stepper-title">Upload File </h3>
                        </div>--%>
                    </div>
                    <!--end::Step 1-->

                    <!--begin::Step 2-->
                    <%--                    <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">2</span>
                        </div>
                        <div class="stepper-label">
                            <h3 class="stepper-title">Map Excel Sheets</h3>
                        </div>
                    </div>--%>
                    <!--end::Step 2-->

                    <!--begin::Step 3-->
                    <%--                    <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">3</span>
                        </div>
                        <div class="stepper-label">
                            <h3 class="stepper-title">File Import Status</h3>
                        </div>
                    </div>--%>
                    <!--end::Step 3-->

                    <!--begin::Step 4-->
                    <%-- <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">5</span>
                        </div>
                        <div class="stepper-label">
                            <h3 class="stepper-title">Schedule</h3>
                        </div>
                    </div>--%>
                    <!--end::Step 4-->
                    <!--begin::Step 5-->
                    <%--                    <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <div class="stepper-line w-40px"></div>
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">6</span>
                        </div>
                        <div class="stepper-label">
                            <h3 class="stepper-title">Summary</h3>
                        </div>
                    </div>--%>
                    <!--end::Step 5-->
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
                                    <div id="axstaysignin6" class="">
                                 <div class="controls">
                                                                                 <div id="axstaysigninforms" class="form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4">
                                                <div class="controlsforms">
                                                    <div class="input-icon left d-flex justify-content-center customclscol">
                                                        <input name="signedin" type="checkbox" id="mulForm" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" title="Multiple Form"   meta:resourcekey="lblFileHeaders" runat="server">
                                                        <span id="Span4" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="mulForm" runat="server">Multiple Form</span>
                                                        <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip" id="headertip" data-bs-original-title="Enable this if you want to upload multiform only." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                                    </div>
                                                </div>
                                            </div>
                                     <button id="btnSaveTemplateXl" type="button" class="btn btn-light-primary shadow-sm col-md-3 ms-3 mt-n2" onclick="saveTemplateXl();">
                            Download Template                       
                        </button>
                                  <%--         <button id="btnDownloadxl" type="button" class="btn btn-light-primary shadow-sm d-none">
                            Download Uploaded Excel file                      
                        </button>--%>
                                            <asp:Button ID="btnDownloadxl" runat="server" Text="Click here to download Excel and view the exceptions" OnClick="btnDownload_Click" CssClass="btn btn-light-primary shadow-sm my-3 d-none" />
                                            <%--                                   <div class="input-icon left d-flex justify-content-center customclscol">
                                       <input name="signedin" type="checkbox" id="Checkbox4" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" title="File contains Headers"  checked="checked" meta:resourcekey="lblFileHeaders" runat="server">
                                        <span id="Span4" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="ChkColNameInfile" runat="server">Process data through Queue</span>
                                         <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip" id="headertip1" data-bs-original-title="Enable this if you want to Process data through Queue." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                            </div>--%>
                                        </div>
                                    </div>
                                    <section class="col-md-12  d-flex justify-content-center">
                                        <asp:UpdatePanel ID="UpdatePanel1" class="w-100" runat="server" UpdateMode="conditional">
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
                                                    <asp:Label ID="Label1" Text="" runat="server" ForeColor="#DB2222"></asp:Label>
                                                    <input type="button" name="name" value="Upload" class=" btn d-none" id="btnFileUpload" />
                                                    <asp:Button runat="server" ID="Button1" class="btn  btn-primary d-none" Text="Upload" OnClick="UploadButton_Click" />
                                                </div>
                                                <div id="axstaysignin" class="form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4 d-none">
                                                    <div class="controls">
                                                        <div class="input-icon left d-flex justify-content-center customclscol">
                                                            <input name="signedin" type="checkbox" id="Checkbox1" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" title="File contains Headers" checked="checked" meta:resourcekey="lblFileHeaders" runat="server">
                                                            <span id="Span1" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="ChkColNameInfile" runat="server">File contains Headers</span>
                                                            <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip" id="headertip" data-bs-original-title="Enable this after uploading the file only." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </section>
                                    <section class="form-group col-md-12 my-4 d-none">
                                        <div class="form-group mb-4">
                                            <label for="projectName" class="form-label col-form-label pb-1 fw-boldest mb-4">Enter Template Name</label>
                                            <input type="text" class="form-control" id="projectName" value="">
                                        </div>
                                    </section>
                                    <section class="form-group col-md-12 my-4 d-none">
                                        <asp:Label runat="server" AssociatedControlID="DropDownList2" class="form-label col-form-label pb-1 fw-boldest required">
                                            <asp:Label ID="Label4" runat="server" meta:resourcekey="lblImptbl" Text="Select Existing Import Name"></asp:Label>
                                        </asp:Label>
                                        <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl1" data-bs-content="Select a Import Name" data-bs-placement="right">help_outline</i>
                                        <asp:DropDownList runat="server" ID="DropDownList2" CssClass="form-select forValidation selectpicker" data-live-search="true" data-size="6" AutoPostBack="true" placeholder="Select Form" OnSelectedIndexChanged="ddlImpTbl_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </section>
                                    <section class="form-group col-md-12 mb-4 d-none">
                                        <asp:Label runat="server" AssociatedControlID="ddlImpTbl" class="form-label col-form-label pb-1 fw-boldest required">
                                            <asp:Label ID="lblImptbl" runat="server" meta:resourcekey="lblImptbl" Text="Select Form"></asp:Label>
                                        </asp:Label>
                                        <i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl1" data-bs-content="Select a Existing ImportName from which the data needs to be imported." data-bs-placement="right">help_outline</i>
                                        <asp:DropDownList runat="server" ID="ddlImpTbl" CssClass="form-select forValidation selectpicker" data-live-search="true" data-size="6" AutoPostBack="true" placeholder="Select Form" OnSelectedIndexChanged="ddlImpTbl_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </section>

                                    <section class="form-group col-md-12 mb-4 d-none">
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
                                    <div id="dataupdatediv" class="form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4 d-none">
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
                                    <section class="form-group col-md-12 d-none">
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
                                   <%-- <button id="openTableButton" class="btn btn-primary my-3" onclick="openTableInBSModal()">Open Table in Modal</button>--%>
                                    <div class="my-3" id="xlnum" runat="server"></div>
                                    <button id="btngridxl" type="button" class="btn btn-light-primary shadow-sm my-3 d-none" onclick="openTableInBSModal()">
                                        Map Coloumns                     
                                    </button>
                                    <div class="" id="checksheets"></div>
                                    <section class="my-4">
<%--                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                        <div class="importrecord d-none">
                                            <asp:Label ID="Label5" CssClass="form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" runat="server" meta:resourcekey="lblrecords">
                                                    Top 5 records
                                            </asp:Label>
                                        </div>--%>
                                        <div id="UpdateGridWrapper" class ="d-none">
                                        <asp:UpdatePanel ID="UpdateGrid" runat="server">
                                            <ContentTemplate>

                                                <hr class="text-gray-500 d-none" id="txtgrey" />
                                                <div class="importrecordtable">
                                                    <asp:GridView CellSpacing="-1" ID="gridImpData" runat="server" Visible="true" OnRowDataBound="gridImpData_RowDataBound" CssClass="table w-100" BorderStyle="None">
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                            </div>
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
                                <div id="dropdownSheetContainer"></div>
                                <%--<section class ="my-4">
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                       <div class="importrecord">
                                                <asp:Label ID="lblrecords" CssClass="form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" runat="server" meta:resourcekey="lblrecords">
                                                    Top 5 records
                                                </asp:Label>
                                            </div>
                                    <asp:UpdatePanel id="UpdateGrid" runat="server">
                                        <ContentTemplate>
                                     
                                            <hr class="text-gray-500" />
                                            <div class="importrecordtable">
                                                <asp:GridView CellSpacing="-1" ID="gridImpData" runat="server" Visible="true" OnRowDataBound="gridImpData_RowDataBound" CssClass="table w-100" BorderStyle="None">
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </section>--%>
                                <div id="groupbyes"></div>
                                <button type="submit" class="btn btn-primary d-none">Save</button>

                                <span class="customMessage fileUploadErrorMessage"></span>
                                <section class="col-md-4 col-sm-12 upload-section d-flex justify-content-center py-4 d-none">
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
                                </section>

                                <section class="col-md-5 col-sm-12 d-flex justify-content-center py-4 d-none">
                                    <div class="btn btn-icon btn-secondary w-30px h-30px w-md-40px h-md-40px position-relative bg-white cursor-default">
                                        <button class="btn btn-secondary dropdown-toggle shadow-sm" id="btnDwnloadDtSrc" type="button" disabled>
                                            Download Data Source
                                            <span class="caret"></span>
                                        </button>
                                    </div>
                                </section>

                                <section class="col-md-3 col-sm-12 d-flex justify-content-center py-4 d-none">
                                    <a id="btnImpHistory" href="javascript:void(0);" runat="server" class="btn btn-light-primary shadow-sm" tabindex="0" visible="true" title="Import History" onclick="importHistory();">
                                        <asp:Label ID="lblImpHistory" runat="server" meta:resourcekey="lblImpHistory">Import History</asp:Label>
                                    </a>
                                </section>

                                <section class="col-md-12  d-flex justify-content-center d-none">
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
                                            <div id="axstaysignin" class="form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4">
                                                <div class="controls">
                                                    <div class="input-icon left d-flex justify-content-center customclscol">
                                                        <input name="signedin" type="checkbox" id="ChkColNameInfile" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" title="File contains Headers"  checked="checked" meta:resourcekey="lblFileHeaders" runat="server">
                                                        <span id="lblstaysin" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="ChkColNameInfile" runat="server">File contains Headers</span>
                                                        <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip" id="headertip" data-bs-original-title="Enable this after uploading the file only." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </section>

                                <section class="col-xs-12 d-none">
                                    <div class="col-xs-12 customclscol">
                                        <div class="form-group upload-progress">
                                            <div id="divProgress" class="progress d-none">
                                                <div id="divProgressBar" class="progress-bar progress-bar-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </section>

                                <div class="form-group col-md-12 row g-3 align-items-center my-2 d-none">
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
                            <div id="imWizardEdit" <%-- class ="d-none"--%>>
                                <%--<button id="openTableButton" class="btn btn-primary" onclick="openTableInBSModal()">Open Table in Modal</button>--%>
                                <div class="controls d-none mb-6 row d-flex thread-slider d-none">
                                 <asp:Label runat="server" class="form-label col-form-label pb-1 fw-boldest col-2">
                                            <asp:Label ID="Label3" runat="server" meta:resourcekey="lblImptbl" Text="Thread Count"></asp:Label>
                                        </asp:Label>
                                     <div class="col-10 d-flex gap-5">
                                    <output for="slider" id="sliderValue1" class="fw-bolder fs-2 my-auto">1</output>
                                <input type="range" min="1" max="10" value="1" step="1" id="slider" class="form-range my-auto">
                                 <output for="slider" id="sliderValue2" class="fw-bolder fs-2 my-auto">10</output>
                                    </div></div>
                                 <div class="controls mb-6 d-none">
                                        <asp:Label runat="server" class="form-label col-form-label pb-1 fw-boldest">
                                            <asp:Label ID="Label2" runat="server" Text="Process Mode"></asp:Label>
                                        </asp:Label>
                                        <%--<i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl6" data-bs-content="Select character used for separating columns in the data file." data-bs-placement="right">help_outline</i>--%>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-select" runat="server">
                                            <asp:ListItem Value="Process with Error" Selected="True">Process with Error</asp:ListItem>
                                            <asp:ListItem Value="Only Validate Data">Only Validate Data</asp:ListItem>
                                            <asp:ListItem Value="Stop on Error">Stop on Error</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                             <div id="axstaysignin4" class="form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-6 d-none">
                                 <div class="controls">
                                   <div class="input-icon left d-flex justify-content-center customclscol">
                                       <input name="signedin" type="checkbox" id="Checkbox2" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" title="File contains Headers"  checked="checked" meta:resourcekey="lblFileHeaders" runat="server">
                                        <span id="Span2" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="ChkColNameInfile" runat="server">Ignore Field Exceptions</span>
                                         <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip" id="headertip" data-bs-original-title="Enable this after uploading the file only." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                            </div>
                                               </div>
                                            </div>
                                <div id="axstaysignin5" class="form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-6 d-none">
                                 <div class="controls">
                                   <div class="input-icon left d-flex justify-content-center customclscol">
                                       <input name="signedin" type="checkbox" id="Checkbox3" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" title="File contains Headers"  checked="checked" meta:resourcekey="lblFileHeaders" runat="server">
                                        <span id="Span3" class="form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" for="ChkColNameInfile" runat="server">Append Data</span>
                                         <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip" id="headertip" data-bs-original-title="Enable this after uploading the file only." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                            </div>
                                               </div>
                                            </div>
                                <div class="d-flex justify-content-between mb-5 d-none">
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
                                    <div class="d-flex justify-content-between my-1 d-none">
                                        <asp:Label ID="lblprimarycolmn" runat="server" meta:resourcekey="lblprimarycolmn" CssClass="form-label col-form-label pb-1 fw-boldest d-none">Primary Column
                                        </asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlPrimaryKey" CssClass="form-select form-select-sm w-100px">
                                        </asp:DropDownList>
                                        <span tabindex="0" class="material-icons material-icons-style material-icons-2 align-middle my-2 me-n7 ms-3" data-bs-toggle="tooltip" data-bs-original-title="Please select a Primary key column to allow update." data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                        <asp:HiddenField ID="hdnPrimaryKey" runat="server" Value="" />
                                    </div>
                                </div>

<%--                                <section class ="d-none">
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
                                </section>--%>

                                <section class="col-sm-12">
                                    <asp:HiddenField ID="hdnIgnoredColumns" runat="server" Value="" />
                                    <asp:HiddenField ID="colheader" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnCOLheaders" runat="server" Value="" />
                                </section>
                            </div>
                        </div>

                        <div class="flex-column" data-kt-stepper-element="content">
                            <%--Step 4--%>
                            <div id="dropdownsContainerd" class ="d-none"></div>
                                                                <div class="form-group">
    <label for="jobSchedule" class ="form-label col-form-label pb-1 fw-boldest mb-4">Job Schedule</label>
    <select id="jobSchedule" class="form-control">
        <option value="everyday">Every day</option>
        <option value="everyweek">Every week</option>
        <option value="everymonth">Every month</option>
        <option value="everyquarter">Every quarter</option>
        <option value="custom">Custom</option>
    </select>
</div>
<div id="weekdayGroup" class="form-group" style="display: none;">
    <label for="weekday" class ="form-label col-form-label pb-1 fw-boldest mb-4">Weekday</label>
    <select id="weekday" class="form-control">
        <option value="sunday">Sunday</option>
        <option value="monday">Monday</option>
        <option value="tuesday">Tuesday</option>
        <option value="wednesday">Wednesday</option>
        <option value="thursday">Thursday</option>
        <option value="friday">Friday</option>
        <option value="saturday">Saturday</option>
    </select>
</div>
<div id="startFromGroup" class="form-group">
    <label for="startFrom" class ="form-label col-form-label pb-1 fw-boldest mb-4">Start From</label>
    <input type="date" id="startFrom" class="form-control">
</div>

<div id="startTimeGroup" class="form-group">
    <label for="startTime" class ="form-label col-form-label pb-1 fw-boldest mb-4">Start Time</label>
    <input type="time" id="startTime" class="form-control">
</div>



<div id="daysGroup" class="form-group" style="display: none;">
    <label for="days" class ="form-label col-form-label pb-1 fw-boldest mb-4">Day</label>
    <select id="days" class="form-control">
    </select>
</div>

<div id="noOfMinutesGroup" class="form-group" style="display: none;">
    <label for="noOfMinutes" class ="form-label col-form-label pb-1 fw-boldest mb-4">No of Minutes</label>
    <input type="number" id="noOfMinutes" class="form-control">
</div>
        <div class="form-group mb-4">
            <label for="projectName" class ="form-label col-form-label pb-1 fw-boldest mb-4">Import Folder</label>
            <input type="text" class="form-control" id="projectName" value="">
        </div>
                            
                            <div class="d-flex justify-content-center d-none" id="imWizardSummary">
                                
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
                                        <asp:HiddenField ID="hdnexcelHeaderName" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnexcelHeaderValues" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnsltForm" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnsltDC" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnsltTemp" runat="server" Value="" />
                                         <asp:HiddenField ID="hdnsltFormFull" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnsltPkey" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnRowCount" runat="server" Value="" />
                                        <asp:HiddenField ID="hdndheetheaders" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnaxmsgid" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnExcelfilepath" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnExcelfileName" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnFieldnamesExcelnew" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnxlHasHeader" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnmulForm" runat="server" Value="false" />
                                        <asp:HiddenField ID="hdnuniqueNamesStringSheet" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnuniqueNamesStringheaders" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnsheetSequence" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnSheetnumber" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnUpdateMapcolumns" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnFname" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnFnameKey" runat="server" Value="" />
                                        <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" OnClientClick="getHeadersForSheet();" CssClass="cloudButton btn btn-primary d-none" />
                                        <asp:Button ID="btnxlrest" runat="server" Text="Import" CssClass="cloudButton btn btn-primary d-none" OnClientClick="GetImpData();" />
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
                        <button id="btnPrev" type="button" runat ="server" class="btn btn-white btn-color-gray-700 btn-active-primary shadow-sm" data-kt-stepper-action="previous">
                            Back
                        </button>
                    </div>

                    <div>
                        <button id="btnSubmit" type="button" class="btn btn-primary shadow-sm d-none" data-kt-stepper-action="submit">
                            <span class="indicator-label">Submit</span>
                            <span class="indicator-progress">Please wait... 
                                <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                            </span>
                        </button>

                        <button id="btnSaveTemplate" type="button" class="btn btn-light-primary shadow-sm d-none" onclick="saveTemplate();">
                            Save Template & Continue                       
                        </button>

                        <button id="btnContinue" type="button" class="btn btn-primary shadow-sm" data-kt-stepper-action="next">
                            Submit
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
        <script type="text/javascript" src="../Js/helper.min.js?v=158"></script>
        <%--<script type="text/javascript" src="../Js/jsclient.min.js?v=103"></script>--%>
        <script type="text/javascript" src="../Js/common.min.js?v=158"></script>
<%--        <script type="text/javascript" src="../Js/AxInterface.min.js?v=11"></script>
        <script type="text/javascript" src="../Js/multiselect.min.js"></script>--%>
        <script type="text/javascript" src="../Js/importall.min.js?v=15"></script>
        <script type="text/javascript">
            var proj = '<%=proj%>';
            var sid = '<%=sid%>';
            var userName = '<%=Session["user"]%>';
        </script>

    </form>
</body>

</html>

