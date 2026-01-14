<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportNew.aspx.cs" ValidateRequest="false" Inherits="aspx_ExportNew" EnableEventValidation="false" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Export Data" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
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
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
   
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script src="../Js/jquery.multi-select.min.js" type="text/javascript"></script>
    
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    
    <script src="../Js/helper.min.js?v=172" type="text/javascript"></script>
    <script src="../Js/ExportImport.min.js?v=34" type="text/javascript"></script>
    <script src="../Js/multiselect.min.js" type="text/javascript"></script>
    

    <script type="text/javascript">

        var arrFlds = new Array();
        var arrFldDcNo = new Array();
        var arrIsDc = new Array();
        var arrGridDcs = new Array();
        var arrAllDcsData = new Array();
        var clcikOnSumbmit = true;
        var AxwizardType = '<%=Session["AxWizardType"]%>';

    </script>

</head>
<body dir='<%=direction%>' class="btextDir-<%=direction%> content p-0 fs-6 page-loading" id="dvexportnew">
    <form id="form1" runat="server">

        <div class="stepper stepper-pills card bg-transparent border-0 h-100 first" id="kt_stepper_example_clickable" data-kt-stepper="true">
            <!--begin::Nav-->
            <div class="card-header d-block px-0 py-5 bg-transparent border-0 mx-20">
                <div class="stepper-nav bg-white rounded-2 flex-center flex-wrap">
                    <!--begin::Step 1-->
                    <div class="stepper-item mx-2 my-4 current" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <!--begin::Line-->
                        <div class="stepper-line w-40px"></div>
                        <!--end::Line-->

                        <!--begin::Icon-->
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">1</span>
                        </div>
                        <!--end::Icon-->

                        <!--begin::Label-->
                        <div class="stepper-label">
                            <h3 class="stepper-title">Data Search
                            </h3>

                            <div class="stepper-desc">
                                <%--Description--%>
                            </div>
                        </div>
                        <!--end::Label-->
                    </div>
                    <!--end::Step 1-->

                    <!--begin::Step 2-->
                    <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <!--begin::Line-->
                        <div class="stepper-line w-40px"></div>
                        <!--end::Line-->

                        <!--begin::Icon-->
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">2</span>
                        </div>
                        <!--begin::Icon-->

                        <!--begin::Label-->
                        <div class="stepper-label">
                            <h3 class="stepper-title">Filter
                            </h3>

                            <div class="stepper-desc">
                                <%--Description--%>
                            </div>
                        </div>
                        <!--end::Label-->
                    </div>
                    <!--end::Step 2-->

                    <!--begin::Step 3-->
                    <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <!--begin::Line-->
                        <div class="stepper-line w-40px"></div>
                        <!--end::Line-->

                        <!--begin::Icon-->
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">3</span>
                        </div>
                        <!--begin::Icon-->

                        <!--begin::Label-->
                        <div class="stepper-label">
                            <h3 class="stepper-title">Export
                            </h3>

                            <div class="stepper-desc">
                                <%--Description--%>
                            </div>
                        </div>
                        <!--end::Label-->
                    </div>
                    <!--end::Step 3-->

                    <!--begin::Step 4-->
                    <div class="stepper-item mx-2 my-4 pending" data-kt-stepper-element="nav" data-kt-stepper-action="step">
                        <!--begin::Line-->
                        <div class="stepper-line w-40px"></div>
                        <!--end::Line-->

                        <!--begin::Icon-->
                        <div class="stepper-icon w-40px h-40px">
                            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
                            <span class="stepper-number">4</span>
                        </div>
                        <!--begin::Icon-->

                        <!--begin::Label-->
                        <div class="stepper-label">
                            <h3 class="stepper-title">Complete
                            </h3>

                            <div class="stepper-desc">
                                <%--Description--%>
                            </div>
                        </div>
                        <!--end::Label-->
                    </div>
                    <!--end::Step 4-->
                </div>
            </div>
            <!--end::Nav-->

            <!--begin::Form-->
            <div class="card card-body h-400px mx-20 overflow-auto">
                <div class="form w-lg-700px mx-auto" novalidate="novalidate" id="kt_stepper_example_basic_form">
                    <!--begin::Group-->
                    <div class="mb-5">
                        <!--begin::Step 1-->
                        <div class="flex-column current" data-kt-stepper-element="content" id="exWizardDataSearch">
                            <%--div class="wizardContainer wizardValidation animated fadeIn" id="exWizardDataSearch">--%>
                            <section class="form-group col-md-12 mb-4">
                                <asp:Label runat="server" class="selectalign form-label col-form-label pb-1 fw-boldest required" AssociatedControlID="ddlExTstruct">
                                    <asp:Label runat="server" ID="Label1" meta:resourcekey="lblextstruct" Text="Select Form" /><span class="allowempty d-none">*</span>
                                </asp:Label>
                                <i tabindex="0" data-trigger="focus" class="icon-arrows-question" data-toggle="popover" id="icocl1" data-content="Select the form, from which you want to export." data-placement="right"></i>
                                <asp:DropDownList runat="server" ID="ddlExTstruct" CssClass="form-select forValidation selectpicker" data-live-search="true" data-size="6" onchange="checkForValuesChanged(this)" placeholder="Select Form" AutoPostBack="true" OnSelectedIndexChanged="ddlExTstruct_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="customErrorMessage selectPageErrorMessage"></span>
                            </section>
                            <section class="form-group col-md-12">
                                <label for="txtExFields" onclick="$('.ms-selectable ul.ms-list').focus();" class="my-4 required">
                                    <asp:Label ID="Label2" runat="server" meta:resourcekey="lbltxtex" class="form-label col-form-label pb-1 fw-boldest" Text="Fields"></asp:Label><span class="allowempty d-none">*</span>
                                </label>
                                <i tabindex="0" data-trigger="focus" class="icon-arrows-question" data-toggle="popover" id="icocl2" data-content="Select the field(s) to be included in the export template." data-placement="right"></i>
                                <table class="table table-borderless w-100">
                                    <tr class="d-flex flex-row-auto flex-center">
                                        <td class="d-block d-sm-table-cell col-12 col-sm-5 p-0">
                                            <select id="mSelectLeft" name="from[]" class="multiselect form-control scroll-x" size="8" multiple="multiple" data-right="#mSelectRight" data-right-all="#right_All_1" data-right-selected="#right_Selected_1" data-left-all="#left_All_1" data-left-selected="#left_Selected_1">
                                                <asp:Repeater ID="rptSelectFields" runat="server">
                                                    <ItemTemplate>
                                                        <option value='<%# Container.DataItem.ToString()%>'><%# Container.DataItem.ToString()%></option>
                                                    </ItemTemplate><%--.Substring(0,Container.DataItem.ToString().IndexOf(" ("))--%>
                                                </asp:Repeater>
                                            </select>
                                        </td>
                                        <td class="d-block d-sm-table-cell col-12 col-sm-2 py-0">
                                            <%--button icons are updating based on the lang selection <, >, <<, >>--%>
                                            <div class="text-center py-1"><a href="javascript:void(0);" id="right_All_1" title="Select All" class="btn btn-sm btn-icon btn-active-primary shadow-sm"><span class="material-icons material-icons-style">keyboard_double_arrow_right</span></a></div>
                                            <div class="text-center py-1"><a href="javascript:void(0);" id="right_Selected_1" title="Select" class="btn btn-sm btn-icon btn-active-primary shadow-sm"><span class="material-icons material-icons-style">keyboard_arrow_right</span></a></div>
                                            <div class="text-center py-1"><a href="javascript:void(0);" id="left_Selected_1" title="Unselect" class="btn btn-sm btn-icon btn-active-primary shadow-sm"><span class="material-icons material-icons-style">keyboard_arrow_left</span></a></div>
                                            <div class="text-center py-1"><a href="javascript:void(0);" id="left_All_1" title="Unselect All" class="btn btn-sm btn-icon btn-active-primary shadow-sm"><span class="material-icons material-icons-style">keyboard_double_arrow_left</span></a></div>
                                        </td>
                                        <td class="d-block d-sm-table-cell col-12 col-sm-5 p-0">
                                            <select name="to[]" id="mSelectRight" class="multiselect form-control scroll-x" size="8" multiple="multiple"></select>
                                        </td>
                                    </tr>
                                </table>
                            </section>
                            <asp:HiddenField ID="hdnColValues" runat="server" Value="" />
                            <asp:HiddenField ID="hdnColNames" runat="server" Value="" />
                            <span class="customErrorMessagee SelectFieldErrorMessage"></span>
                        </div>
                        <%--</div>--%>
                        <!--begin::Step 1-->

                        <!--begin::Step 1-->
                        <div class="flex-column pending" data-kt-stepper-element="content">
                            <div class="wizardContainer animated fadeIn" id="exWizardFilter">
                                <h2 class="con">
                                    <asp:Label ID="lblcond" runat="server" meta:resourcekey="lblcond">Conditions</asp:Label></h2>
                                <section id="dvCondition" class="dvBg dvLevel bdr">
                                    <section id="filtersection">
                                        <div id="tblpanel">
                                            <div class="configRightFormFields row g-3 align-items-center mb-4 my-4" id="lvfld">
                                                <div class="col-md-4 col-sm-12">
                                                    <asp:Label ID="lblPassword" class="form-label col-form-label pb-1 fw-boldest" runat="server">Field</asp:Label>
                                                </div>
                                                <div class="col-md-8 col-sm-12">
                                                    <asp:DropDownList ID="ddlFilter" CssClass="lblTxt form-select" data-control="select2" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="clearfix"></div>
                                            </div>
                                            <%--  <tr>
                                    <th>
                                        <asp:Label ID="lbfield" runat="server" meta:resourcekey="lbfield">Field</asp:Label></th>
                                    <th>
                                        <asp:Label ID="lblcond1" runat="server" meta:resourcekey="lblcond1">Condition</asp:Label></th>
                                    <th>
                                        <asp:Label ID="lblvalue" runat="server" meta:resourcekey="lblvalue">Value</asp:Label></th>
                                    <th></th>
                                    <th></th>
                                </tr>
                                <tr>--%>
                                            <%--<td id="lvfld1">
                                        <asp:DropDownList ID="ddlFilter" CssClass="lblTxt form-select"  data-control="select2" runat="server">
                                        </asp:DropDownList>
                                    </td>--%>
                                            <div class="configRightFormFields row g-3 align-items-center mb-4" id="filcond">
                                                <div class="col-md-4 col-sm-12">
                                                    <asp:Label ID="Label3" class="form-label col-form-label pb-1 fw-boldest" runat="server">Condition</asp:Label>
                                                </div>
                                                <div class="col-md-8 col-sm-12">
                                                    <asp:DropDownList ID="ddlFilcond" placeholder="Please select a Condition" CssClass="lblTxt form-select" data-control="select2" runat="server" onchange="javascript:CheckFilterCond('-1');">
                                                        <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="Equal to" Value="="></asp:ListItem>
                                                        <asp:ListItem Text="Not Equal to" Value="!="></asp:ListItem>
                                                        <asp:ListItem Text="Less Than" Value="<"></asp:ListItem>
                                                        <asp:ListItem Text="Greater Than" Value=">"></asp:ListItem>
                                                        <asp:ListItem Text="Less Than or Equal to" Value="<="></asp:ListItem>
                                                        <asp:ListItem Text="Greater Than or Equal to" Value=">="></asp:ListItem>
                                                        <asp:ListItem Text="Between" Value="between"></asp:ListItem>
                                                        <asp:ListItem Text="Starts With" Value="Starts With"></asp:ListItem>
                                                        <asp:ListItem Text="Ends With" Value="Ends With"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="clearfix"></div>
                                            </div>
                                            <%-- <td id="filcond">
                                        <asp:DropDownList ID="ddlFilcond" placeholder="Please select a Condition" CssClass="lblTxt form-select" data-control="select2" runat="server" onchange="javascript:CheckFilterCond('-1');">
                                            <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Equal to" Value="="></asp:ListItem>
                                            <asp:ListItem Text="Not Equal to" Value="!="></asp:ListItem>
                                            <asp:ListItem Text="Less Than" Value="<"></asp:ListItem>
                                            <asp:ListItem Text="Greater Than" Value=">"></asp:ListItem>
                                            <asp:ListItem Text="Less Than or Equal to" Value="<="></asp:ListItem>
                                            <asp:ListItem Text="Greater Than or Equal to" Value=">="></asp:ListItem>
                                            <asp:ListItem Text="Between" Value="between"></asp:ListItem>
                                            <asp:ListItem Text="Starts With" Value="Starts With"></asp:ListItem>
                                            <asp:ListItem Text="Ends With" Value="Ends With"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>--%>
                                            <div class="configRightFormFields row g-3 align-items-center mb-4" id="filval11">
                                                <div class="col-md-4 col-sm-12">
                                                    <asp:Label ID="Label4" class="form-label col-form-label pb-1 fw-boldest" runat="server">Value</asp:Label>
                                                </div>
                                                <div class="col-md-8 col-sm-12">
                                                    <asp:TextBox ID="txtFilter" runat="server" CssClass="lblTxt m-wrap placeholder-no-fix form-control" MaxLength="1024" Text=""></asp:TextBox>
                                                </div>
                                                <div class="clearfix"></div>
                                            </div>
                                            <%--<td id="filval11">
                                        <asp:TextBox ID="txtFilter" Width="100px" runat="server" CssClass="lblTxt form-control" MaxLength="1024" Text=""></asp:TextBox>
                                    </td>--%>
                                            <%-- <td id="filval12">
                                        <asp:TextBox ID="filVal2" runat="server" Width="100px" CssClass="lblTxt form-control" MaxLength="1024" Text="" disabled="disabled"></asp:TextBox>--%>
                                            <div class="configRightFormFields row g-3 align-items-center mb-4" id="filval12">
                                                <div class="col-md-4 col-sm-12">
                                                    <asp:Label ID="Label5" class="form-label col-form-label pb-1 fw-boldest" runat="server">Value</asp:Label>
                                                </div>
                                                <div class="col-md-8 col-sm-12">
                                                    <asp:TextBox ID="filVal2" runat="server" disabled="disabled" Text="" CssClass="lblTxt m-wrap placeholder-no-fix form-control" MaxLength="1024"></asp:TextBox>
                                                </div>
                                                <div class="clearfix"></div>

                                                <asp:HiddenField ID="goval" runat="server" Value=""></asp:HiddenField>
                                                <asp:HiddenField ID="FilterRowCount" Value="0" runat="server" />
                                                <asp:HiddenField ID="hdnAction" runat="server" />
                                                <asp:HiddenField ID="hdnFilterXml" runat="server" />
                                                <asp:HiddenField ID="hdnValidate" runat="server" />
                                                <asp:HiddenField ID="hdnDateFields" runat="server" />
                                            </div>
                                            <%-- </td>--%>
                                            <div class="configRightFormFields row g-3 align-items-center mb-4">
                                                <a class="curHand btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2" tabindex="0" href="javascript:AddNewCondition();"
                                                    id="btnAddNew"><span class="material-icons material-icons-style">add
                                                    </span></a>
                                                <a class="curHand btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm" href="javascript:ClearFirstRow();"
                                                    id="clearCond" tabindex="0"><span class="material-icons material-icons-style">remove
                                                    </span></a>
                                            </div>
                                            <%-- </tr>--%>
                                        </div>
                                        <section id="dvNewrow">
                                            <table id="tblFilter">
                                            </table>
                                        </section>
                                    </section>
                                </section>
                            </div>
                        </div>
                        <!--begin::Step 1-->

                        <!--begin::Step 1-->
                        <div class="flex-column pending" data-kt-stepper-element="content">
                            <div class="wizardContainer animated fadeIn" id="exWizardExport">
                                <div class="configRightFormFields row g-3 align-items-center mb-4 my-4">
                                    <div class="col-md-4 col-sm-12">
                                        <asp:Label ID="lblexfiletype" runat="server" class="lblleft form-label col-form-label pb-1 fw-boldest" meta:resourcekey="lblexfiletype" AssociatedControlID="ddlExFileType">
                                            File Type<span class="allowempty">*</span>
                                        </asp:Label>
                                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question helptxt" data-toggle="popover" id="icocl3" data-content="Select the file format to which the data to be exported." data-placement="right"></i>
                                    </div>
                                    <div class="col-md-8 col-sm-12">
                                        <asp:DropDownList ID="ddlExFileType" CssClass="form-select" runat="server" onchange="javascript:EnableSeparator()">
                                            <asp:ListItem Value="Text">Text</asp:ListItem>
                                            <asp:ListItem Value="CSV">CSV</asp:ListItem>
                                            <%--<asp:ListItem Value="Excel">Excel</asp:ListItem>--%>
                                        </asp:DropDownList>
                                        <span class="customErrorMessage SelectFielTypeErrorMessage"></span>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <%--                                     <div class="clearfix"></div>
                    <section class="form-group col-md-6">
                       
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question helptxt" data-toggle="popover" id="icocl3" data-content="Select the file format to which the data to be exported." data-placement="right"></i>
                        <asp:DropDownList ID="ddlExFileType" CssClass="form-control" runat="server" onchange="javascript:EnableSeparator()">
                            <asp:ListItem Value="Text">Text</asp:ListItem>
                            <asp:ListItem Value="CSV">CSV</asp:ListItem>
                            <%--<asp:ListItem Value="Excel">Excel</asp:ListItem>--%>
                                <%--</asp:DropDownList>
                        <span class="customErrorMessage SelectFielTypeErrorMessage"></span>
                    </section>--%>
                                <div class="configRightFormFields row g-3 align-items-center mb-4">
                                    <div class="col-md-4 col-sm-12">
                                        <asp:Label ID="lblexseparator" runat="server" class="lblleft form-label col-form-label pb-1 fw-boldest" meta:resourcekey="lblexseparator" AssociatedControlID="ddlExSeparator">
                                            Seperator<span class="allowempty">*</span>
                                        </asp:Label>
                                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question helptxt" data-toggle="popover" id="icocl4" data-content="Select the type of seperator used to separate the column values on each line." data-placement="right"></i>
                                    </div>
                                    <div class="col-md-8 col-sm-12">
                                        <asp:DropDownList ID="ddlExSeparator" CssClass="form-select" runat="server" disabled="disabled">
                                            <asp:ListItem Value="">-- Select --</asp:ListItem>
                                            <asp:ListItem Value="," Selected="True">Comma(,)</asp:ListItem>
                                            <asp:ListItem Value=";">Semicolon(;)</asp:ListItem>
                                            <asp:ListItem Value="|">Pipe(|)</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="configRightFormFields row g-3 align-items-center mb-4">
                                    <div class="col-md-4 col-sm-12">
                                        <asp:Label ID="lblexpfilename" meta:resourcekey="lblexpfilename" runat="server" class="lblleft form-label col-form-label pb-1 fw-boldest" AssociatedControlID="txtExpFileName">
                                            File Name<span class="allowempty">*</span>
                                        </asp:Label>
                                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question helptxt" data-toggle="popover" id="icocl5" data-content="Please specify the file name to which data will be exported.By default the file name will same as the selected form name." data-placement="right"></i>
                                    </div>
                                    <div class="col-md-8 col-sm-12">
                                        <asp:TextBox ID="txtExpFileName" CssClass="form-control checkForSpecialCharacters" MaxLength="250" onblur="javascript:DisChks()" runat="server"></asp:TextBox>
                                        <span class="customErrorMessage FielNameToExportErrorMessage"></span>

                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <%--<section class="form-group col-md-6">
                        <asp:Label runat="server" class="lblleft" ID="lblexseparator" meta:resourcekey="lblexseparator" AssociatedControlID="ddlExSeparator">
                                            Seperator
                        </asp:Label>
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question helptxt" data-toggle="popover" id="icocl4" data-content="Select the type of seperator used to separate the column values on each line." data-placement="right"></i>

                        
                    </section>
                    <section class="form-group col-md-12">
                        <asp:Label ID="lblexpfilename" meta:resourcekey="lblexpfilename" runat="server" class="lblleft form-label col-form-label pb-1 fw-boldest" AssociatedControlID="txtExpFileName">
                                            File Name<span class="allowempty">*</span>
                        </asp:Label>
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question helptxt" data-toggle="popover" id="icocl5" data-content="Please specify the file name to which data will be exported.By default the file name will same as the selected form name." data-placement="right"></i>

                        <asp:TextBox ID="txtExpFileName" CssClass="form-control checkForSpecialCharacters" MaxLength="250" onblur="javascript:DisChks()" runat="server"></asp:TextBox>
                        <span class="customErrorMessage FielNameToExportErrorMessage"></span>
                    </section>--%>
                                <div class="configRightFormFields row g-3 align-items-center mb-4 app form-check form-switch form-check-solid">
                                    <%-- <input name="signedin" type="checkbox" id="chkWithHeader" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px" title="With Header">
                                            <label id="lblstaysin" class="form-check-label form-label col-form-label pb-1 fw-boldest fw-bolder text-dark fs-6 mb-0" for="chkWithHeader">With Header</label>
                        <asp:Label runat="server" class="checkbox-inline form-label col-form-label pb-1 fw-boldest d-none" AssociatedControlID="chkWithHeader">
                        <asp:CheckBox ID="chkWithHeader1" CssClass="d-none" runat="server" Text="With Header" meta:resourcekey="lblWithHeader" />
                            </asp:Label>--%>

                                    <asp:Label runat="server" class="form-label col-form-label pb-1 fw-boldest" AssociatedControlID="chkWithHeader">
                                        <input id="chkWithHeader" type="checkbox" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px" runat="server" text="With Header" meta:resourcekey="lblWithHeader" />With Header
                                    </asp:Label>
                                    <i tabindex="0" data-trigger="focus" class="icon-arrows-question" data-toggle="popover" id="icocl6" data-content="If you wish to export the data with column names, check mark this option." data-placement="right"></i>
                                    <%--<<br />--%>
                                    <asp:Label runat="server" class="form-label col-form-label pb-1 fw-boldest" AssociatedControlID="chkExWithQuotes">
                                        <input id="chkExWithQuotes" type="checkbox" class="m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2" onclick="javascript: SetExType();" runat="server" meta:resourcekey="lblWithQuotes" />With Quotes  <span tabindex="0"
                                            class="material-icons material-icons-style material-icons-2 align-middle ms-2 my-3" data-bs-toggle="tooltip"
                                            id="headertip"
                                            data-bs-original-title="Enable this if data to be exported contains comma seperator"
                                            data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>
                                    </asp:Label>

                                </div>
                            </div>
                        </div>
                        <!--begin::Step 1-->

                        <!--begin::Step 1-->
                        <div class="flex-column pending" data-kt-stepper-element="content">
                            <div class="wizardContainer animated fadeIn" id="exWizardComplete">
                                <asp:ScriptManager runat="server" ID="sm">
                                </asp:ScriptManager>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnExport" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <br />
                                        <div class="container d-flex justify-content-center">
                                            <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                                        </div>
                                        <br />
                                        <div class="d-flex justify-content-center">
                                            <asp:HyperLink ID="lnkExpFile" CssClass="btn btn-primary d-inline-flex align-items-center shadow-sm me-2" runat="server" Visible="false" ToolTip="Download"></asp:HyperLink>
                                            <section class="btnExport">
                                                <asp:Button ID="btnExport" CssClass="btn-primary btn btn-info-full d-none" runat="server" Text="Download" OnClick="btnExport_Click" />
                                            </section>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </div>
                        <!--begin::Step 1-->
                    </div>
                    <!--end::Group-->


                </div>
            </div>
            <!--end::Form-->

            <div class="card-footer ">
                <%--bg-transparent border-0 px-0 py-2--%>
                <!--begin::Actions-->
                <div class="d-flex d-flex justify-content-end mx-2">
                    <!--begin::Wrapper-->
                    <div class="me-2">
                        <button type="button" class="btn btn-white btn-color-gray-700 btn-active-primary shadow-sm" data-kt-stepper-action="previous">
                            Back
                        </button>
                    </div>
                    <!--end::Wrapper-->

                    <!--begin::Wrapper-->
                    <div>
                        <button type="button" class="btn btn-primary shadow-sm" data-kt-stepper-action="submit">
                            <span class="indicator-label">Submit
                            </span>
                            <span class="indicator-progress">Please wait... <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                            </span>
                        </button>

                        <button type="button" class="btn btn-primary shadow-sm" data-kt-stepper-action="next">
                            Continue
                        </button>
                    </div>
                    <!--end::Wrapper-->
                </div>
                <!--end::Actions-->
            </div>
        </div>
        <!-- Widget Work flow - begins -->
        <div id="wizardWrappper" class="d-none">
            <!-- Widget Header menus - begins -->
            <div id="wizardHeader">
            </div>
            <!-- Widget Header menus - end -->

            <div id="wizardBodyContent">
                <!-- Widget Data Search - begins -->
                <%-- <div class="wizardContainer wizardValidation animated fadeIn" id="exWizardDataSearch">
                    <section class="form-group col-md-12">
                        <asp:Label runat="server" class="selectalign" AssociatedControlID="ddlExTstruct">
                            <asp:Label runat="server" ID="lblextstruct" meta:resourcekey="lblextstruct" Text="Select Form" /><span class="allowempty">*</span>
                        </asp:Label>
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question" data-toggle="popover" id="icocl1" data-content="Select the form, from which you want to export." data-placement="right"></i>
                        <asp:DropDownList runat="server" ID="ddlExTstruct" CssClass="form-control forValidation selectpicker" data-live-search="true" data-size="6" onchange="checkForValuesChanged(this)" AutoPostBack="true" OnSelectedIndexChanged="ddlExTstruct_SelectedIndexChanged">
                        </asp:DropDownList>
                        <span class="customErrorMessage selectPageErrorMessage"></span>
                    </section>
                    <section class="form-group col-md-12">
                        <label for="txtExFields" onclick="$('.ms-selectable ul.ms-list').focus();">
                            <asp:Label ID="lbltxtex" runat="server" meta:resourcekey="lbltxtex" Text="Fields"></asp:Label><span class="allowempty">*</span>
                        </label>
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question" data-toggle="popover" id="icocl2" data-content="Select the field(s) to be included in the export template." data-placement="right"></i>
                        <table>
                            <tr>
                                <td>
                                    <select id="mSelectLeft" name="from[]" class="multiselect form-control" size="8" multiple="multiple" data-right="#mSelectRight" data-right-all="#right_All_1" data-right-selected="#right_Selected_1" data-left-all="#left_All_1" data-left-selected="#left_Selected_1">
                                        <asp:Repeater ID="rptSelectFields" runat="server">
                                            <ItemTemplate>
                                                <option value='<%# Container.DataItem.ToString()%>'><%# Container.DataItem.ToString().Substring(0,Container.DataItem.ToString().IndexOf(" ("))%></option>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                </td>
                                <td>
                                    <%--button icons are updating based on the lang selection <, >, <<, >>--%>
                <%--<div><a href="" id="right_All_1" title="Select All" class="fa colorButton select-button"></a></div>
                                    <div><a href="" id="right_Selected_1" title="Select" class="fa colorButton select-button"></a></div>
                                    <div><a href="" id="left_Selected_1" title="Unselect" class="fa colorButton select-button"></a></div>
                                    <div><a href="" id="left_All_1" title="Unselect All" class="fa colorButton select-button"></a></div>
                                </td>
                                <td>
                                    <select name="to[]" id="mSelectRight" class="multiselect form-control" size="8" multiple="multiple"></select>
                                </td>
                            </tr>
                        </table>
                    </section>
                    <asp:HiddenField ID="hdnColValues" runat="server" Value="" />
                    <asp:HiddenField ID="hdnColNames" runat="server" Value="" />
                    <span class="customErrorMessagee SelectFieldErrorMessage"></span>
                </div>--%>
                <!-- Widget Data Search - end -->

                <!-- Widget Data Filter - begins -->
                <%--<div class="wizardContainer animated fadeIn" id="exWizardFilter">
                    <h2 class="con">
                        <asp:Label ID="lblcond" runat="server" meta:resourcekey="lblcond">Conditions</asp:Label></h2>
                    <section id="dvCondition" class="dvBg dvLevel bdr">
                        <section id="filtersection">
                            <table id="tblpanel">
                                <tr>
                                    <th>
                                        <asp:Label ID="lbfield" runat="server" meta:resourcekey="lbfield">Field</asp:Label></th>
                                    <th>
                                        <asp:Label ID="lblcond1" runat="server" meta:resourcekey="lblcond1">Condition</asp:Label></th>
                                    <th>
                                        <asp:Label ID="lblvalue" runat="server" meta:resourcekey="lblvalue">Value</asp:Label></th>
                                    <th></th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td id="lvfld">
                                        <asp:DropDownList ID="ddlFilter" CssClass="lblTxt form-control" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="filcond">
                                        <asp:DropDownList ID="ddlFilcond" CssClass="lblTxt form-control" runat="server" onchange="javascript:CheckFilterCond('-1');">
                                            <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Equal to" Value="="></asp:ListItem>
                                            <asp:ListItem Text="Not Equal to" Value="!="></asp:ListItem>
                                            <asp:ListItem Text="Less Than" Value="<"></asp:ListItem>
                                            <asp:ListItem Text="Greater Than" Value=">"></asp:ListItem>
                                            <asp:ListItem Text="Less Than or Equal to" Value="<="></asp:ListItem>
                                            <asp:ListItem Text="Greater Than or Equal to" Value=">="></asp:ListItem>
                                            <asp:ListItem Text="Between" Value="between"></asp:ListItem>
                                            <asp:ListItem Text="Starts With" Value="Starts With"></asp:ListItem>
                                            <asp:ListItem Text="Ends With" Value="Ends With"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td id="filval11">
                                        <asp:TextBox ID="txtFilter" Width="100px" runat="server" CssClass="lblTxt form-control" MaxLength="1024" Text=""></asp:TextBox>
                                    </td>
                                    <td id="filval12">
                                        <asp:TextBox ID="filVal2" runat="server" Width="100px" CssClass="lblTxt form-control" MaxLength="1024" Text="" disabled="disabled"></asp:TextBox>
                                        <asp:HiddenField ID="goval" runat="server" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="FilterRowCount" Value="0" runat="server" />
                                        <asp:HiddenField ID="hdnAction" runat="server" />
                                        <asp:HiddenField ID="hdnFilterXml" runat="server" />
                                        <asp:HiddenField ID="hdnValidate" runat="server" />
                                        <asp:HiddenField ID="hdnDateFields" runat="server" />
                                    </td>
                                    <td>
                                        <a class="curHand icon-arrows-circle-plus" tabindex="0" href="javascript:AddNewCondition();"
                                            id="btnAddNew"></a>
                                        <a class="curHand icon-arrows-circle-minus" href="javascript:ClearFirstRow();"
                                            id="clearCond" tabindex="0"></a>
                                    </td>
                                </tr>
                            </table>
                            <section id="dvNewrow">
                                <table id="tblFilter">
                                </table>
                            </section>
                        </section>
                    </section>
                </div>--%>
                <!-- Widget Data Filter - end -->

                <!-- Widget Data Export - begins -->
                <%--<div class="wizardContainer animated fadeIn" id="exWizardExport">
                    <section class="form-group col-md-6">
                        <asp:Label ID="lblexfiletype" runat="server" class="lblleft" meta:resourcekey="lblexfiletype" AssociatedControlID="ddlExFileType">
                                            File Type<span class="allowempty">*</span>
                        </asp:Label>
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question helptxt" data-toggle="popover" id="icocl3" data-content="Select the file format to which the data to be exported." data-placement="right"></i>
                        <asp:DropDownList ID="ddlExFileType" CssClass="form-control" runat="server" onchange="javascript:EnableSeparator()">
                            <asp:ListItem Value="Text">Text</asp:ListItem>
                            <asp:ListItem Value="CSV">CSV</asp:ListItem>
                            <%--<asp:ListItem Value="Excel">Excel</asp:ListItem>--%>
                <%-- </asp:DropDownList>
                        <span class="customErrorMessage SelectFielTypeErrorMessage"></span>
                    </section>
                    <section class="form-group col-md-6">
                        <asp:Label runat="server" class="lblleft" ID="lblexseparator" meta:resourcekey="lblexseparator" AssociatedControlID="ddlExSeparator">
                                            Seperator
                        </asp:Label>
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question helptxt" data-toggle="popover" id="icocl4" data-content="Select the type of seperator used to separate the column values on each line." data-placement="right"></i>

                        <asp:DropDownList ID="ddlExSeparator" CssClass="form-control" runat="server" disabled="disabled">
                            <asp:ListItem Value="">-- Select --</asp:ListItem>
                            <asp:ListItem Value="," Selected="True">Comma(,)</asp:ListItem>
                            <asp:ListItem Value=";">Semicolon(;)</asp:ListItem>
                            <asp:ListItem Value="|">Pipe(|)</asp:ListItem>
                        </asp:DropDownList>
                    </section>
                    <section class="form-group col-md-12">
                        <asp:Label ID="lblexpfilename" meta:resourcekey="lblexpfilename" runat="server" class="lblleft" AssociatedControlID="txtExpFileName">
                                            File Name<span class="allowempty">*</span>
                        </asp:Label>
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question helptxt" data-toggle="popover" id="icocl5" data-content="Please specify the file name to which data will be exported.By default the file name will same as the selected form name." data-placement="right"></i>

                        <asp:TextBox ID="txtExpFileName" CssClass="form-control checkForSpecialCharacters" MaxLength="250" onblur="javascript:DisChks()" runat="server"></asp:TextBox>
                        <span class="customErrorMessage FielNameToExportErrorMessage"></span>
                    </section>
                    <section class="form-group col-md-12 app">
                        <asp:Label runat="server" class="checkbox-inline" AssociatedControlID="chkWithHeader">
                            <asp:CheckBox ID="chkWithHeader" runat="server" Text="With Header" meta:resourcekey="lblWithHeader" />
                        </asp:Label>
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question" data-toggle="popover" id="icocl6" data-content="If you wish to export the data with column names, check mark this option." data-placement="right"></i>
                        <br />
                        <asp:Label runat="server" class="checkbox-inline" AssociatedControlID="chkExWithQuotes">
                            <asp:CheckBox ID="chkExWithQuotes" onclick="javascript:SetExType();" runat="server" meta:resourcekey="lblWithQuotes" />
                        </asp:Label>
                        <i tabindex="0" data-trigger="focus" class="icon-arrows-question" data-toggle="popover" id="icocl7" data-content="Use this option to specify how to identify text strings in a column. The default value is double quotation marks." data-placement="right"></i>
                    </section>
                </div>--%>
                <!-- Widget Data Export - end -->

                <!-- Widget Complete flow - begins -->
                <%--<div class="wizardContainer animated fadeIn" id="exWizardComplete">
                    <asp:ScriptManager runat="server" ID="sm">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnExport" />
                        </Triggers>
                        <ContentTemplate>
                            <br />
                            <div class="container">
                                <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                            </div>
                            <br />
                            <asp:HyperLink ID="lnkExpFile" CssClass="hotbtn btn" runat="server" Visible="false" ToolTip="Download"></asp:HyperLink>
                            <section class="btnExport">
                                <asp:Button ID="btnExport" CssClass="hotbtn btn btn-info-full" runat="server" Text="Export" OnClick="btnExport_Click" />
                            </section>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>--%>
                <!-- Widget Complete flow - end -->

            </div>
        </div>
        <div id="wizardFooter" class="wizard-footer d-none">
            <div class="pull-left">
                <button onclick="expWizardObj.checkClick(this,'prev')" title="Previous" type="button" id="wizardPrevbtn" class="btn hotbtn prev-step">&lt; Prev</button>
            </div>
            <div class="pull-right">
                <button id="wizardCancelbtn" onclick="closeWindow()" type="button" title="Cancel" class="coldbtn btn btn-info-full ">Cancel</button>
                <button onclick="expWizardObj.checkClick(this,'next')" data-validator="" title="Next" id="wizardNextbtn" type="button" class="hotbtn btn ">Next &gt;</button>
                <button type="button" id="wizardCompbtn" class="hotbtn btn  " title="Done">Done</button>
            </div>
        </div>
        <!-- Widget work flow - end -->
        <div id="waitDiv1" class="page-loader rounded-2 bg-radial-gradient">
            <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                <span class="loader"></span>
            </div>
        </div>
        <div id='waitDiv' class="hide">
            <div id='backgroundDiv'>
            </div>
        </div>

        <asp:HiddenField ID="hdnCondString" runat="server" />
        <script>
            // Stepper lement
            //var element = document.querySelector("#kt_stepper_example_clickable");

            //// Initialize Stepper
            //var stepper = new KTStepper(element);

            //// Handle navigation click
            //stepper.on("kt.stepper.click", function (stepper) {
            //	stepper.goTo(stepper.getClickedStepIndex()); // go to clicked step
            //});

            //// Handle next step
            //    stepper.on("kt.stepper.next", function (stepper) {


            //        if (stepper.getCurrentStepIndex() == 1) {
            //          var selecCondition =  ValSelectCondition();
            //          if (selecCondition) {
            //              return true;
            //          }
            //          else {

            //              stepper.goNext();
            //          }

            //        }
            //        //else

            //        if (stepper.getCurrentStepIndex() == 2) {

            //            var resultForNxtClick = SaveCondition();
            //            if (resultForNxtClick) {
            //            //    $("#ddlExFileType").focus();
            //            //    return true;
            //                stepper.goNext();
            //            }
            //            else {
            //            //    return false;
            //               stepper.goNext();
            //            }
            //            //stepper.goNext();
            //        }

            //    //var resultForNxtClick = SaveCondition();
            //    if (stepper.getCurrentStepIndex() == 3) {
            //        var temp = "";
            //        $("#mSelectRight option").each(function () {
            //            temp += $(this).val() + ", "
            //        })
            //        temp = temp.substring(0, temp.length - 2);
            //        $('#hdnColValues').val(temp);
            //        var result = ClickExportBtn();
            //        if (result) {
            //            $("#btnExport").click();

            //        }
            //    }
            //   // stepper.goNext();// go next step
            //});

            //// Handle previous step
            //stepper.on("kt.stepper.previous", function (stepper) {
            //	stepper.goPrevious(); // go previous step
            //});
        </script>
    </form>
</body>
</html>
