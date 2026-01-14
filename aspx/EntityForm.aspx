<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EntityForm.aspx.cs" Inherits="aspx_EntityForm" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Entity Management
    </title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
     <asp:PlaceHolder runat="server">
         <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
     </asp:PlaceHolder>
    <link rel="stylesheet" href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css" />
    <link rel="stylesheet" href="../css/EntityForm.min.css?v=11" />
      
</head>

<body id="Entitymanagement_Body" class="btextDir-<%=direction%> header-fixed header-tablet-and-mobile-fixed aside-fixed gradTheme" dir="<%=direction%>">
    <form id="form2" runat="server" enctype="multipart/form-data">

        <asp:PlaceHolder runat="server">
            <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server">
            <%:Scripts.Render("~/Js/DataPageJS") %>
        </asp:PlaceHolder>

         <div class="subres d-none">
             <%=tstJsArrays.ToString() %>
             <%=tstScript.ToString()%>
         </div>

        <asp:ScriptManager ID="ScriptManager" runat="server">
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
                <asp:ServiceReference Path="../CustomWebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <!--begin::Content-->
        <div class="content d-flex flex-column flex-column-fluid ">
            <!--begin::Container-->
            <div id="kt_content_container" class="" style="overflow: hidden;">
                <!--begin::Row-->

                <div class=" " id="Entitymanagement_Body-overalldiv">

                    <!--begin:::Col-->

                    <div id="Entity_management_Wrapper" class="row">


                        <div id="Entity_management_Left"
                            class="col-xl-12 col-md-12 d-flex flex-column flex-column-fluid vh-100 min-vh-100">
                            <div class="card card-flush h-lg-100  ">

                                <div class="card-header Page-title">

                                    <!--begin::Title-->
                                    <h3 class="card-title align-items-start flex-column" style="margin-bottom: 0px !important;">
                                        <span class="card-label fw-bold text-gray-900" id="EntityTitle"></span>

                                    </h3>
                                    <!--end::Title-->
                                    <!--begin::Toolbar-->
                                    <div class="card-toolbar">
                                        <div class="Tkts-toolbar-Right">
                                            <button type="button"  id="add-entity"
                                                class="btn btn-sm btn-icon btn-primary btn-active-primary btn-custom-border-radius" onclick="_entityForm.openNewTstruct(); return false;" data-toggle="tooltip" title="New">
                                                <span class="material-icons material-icons-style material-icons-2">add</span>
                                            </button>
                                            <button type="button"
                                                class="btn btn-sm btn-icon btn-primary btn-active-primary btn-custom-border-radius" onclick="return _entityForm.editEntity(); return false;" data-toggle="tooltip" title="Edit">
                                                <span class="material-icons material-icons-style material-icons-2">edit_note</span>
                                            </button>
                                            <button type="button"
                                                class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm" onclick="_entityForm.openEntityPage(); return false;" data-toggle="tooltip" title="View List">
                                                <span class="material-icons material-icons-style material-icons-2">format_list_bulleted</span>
                                            </button>
                                            <button type="button"
                                                class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm d-none" data-toggle="tooltip" title="Apply Filter(s)" onclick="openFilters(); return false;">
                                                <span class="material-icons material-icons-style material-icons-2">tune</span>
                                            </button>
                                            <button type="button"
                                                class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm" onclick="_entityForm.reloadEntityPage(); return false;" data-toggle="tooltip" title="Reload">
                                                <span class="material-icons material-icons-style material-icons-2">refresh</span>
                                            </button>
                                            

                                            <button type="button"
                                                class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm  tb-btn btn-sm" data-kt-menu-trigger="{default:'click', lg: 'hover'}" data-kt-menu-attach="parent" data-kt-menu-placement="bottom-end">
                                                <span
                                                    class="material-icons material-icons-style material-icons-2">format_color_fill
                                                </span>
                                            </button>


                                            <div id="selectThemes" class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-title-gray-700 menu-icon-gray-500 menu-active-bg menu-state-color fw-semibold py-4 fs-base w-150px" data-kt-menu="true" data-kt-element="theme-mode-menu">
                                                <!--begin::Menu item-->
                                                <div class="menu-item px-3 my-0">
                                                    <a href="#" class="menu-link px-3 py-2" data-kt-element="mode" data-target="lightTheme" data-kt-value="light">
                                                        <span class="material-icons material-icons-style material-icons-2">light_mode</span>
                                                        <span class="menu-title">Light
                                                        </span>
                                                    </a>
                                                </div>
                                                <!--end::Menu item-->

                                                <!--begin::Menu item-->
                                                <div class="menu-item px-3 my-0">
                                                    <a href="#" class="menu-link px-3 py-2" data-kt-element="mode" data-target="blackTheme">
                                                        <span class="material-icons material-icons-style material-icons-2">dark_mode</span>
                                                        <span class="menu-title">Dark
                                                        </span>
                                                    </a>
                                                </div>
                                                <!--end::Menu item-->

                                                <!--begin::Menu item-->
                                                <div class="menu-item px-3 my-0">
                                                    <a href="#" class="menu-link px-3 py-2 active" data-kt-element="mode" data-target="gradTheme" data-kt-value="system">
                                                        <span class="material-icons material-icons-style material-icons-2">palette</span>
                                                        <span class="menu-title">Gradient
                                                        </span>
                                                    </a>
                                                </div>

                                                <div class="menu-item px-3 my-0 d-none">
                                                    <a href="#" class="menu-link px-3 py-2" data-kt-element="mode" data-target="compactTheme" data-kt-value="system">
                                                        <span class="material-icons material-icons-style material-icons-2">view_compact</span>
                                                        <span class="menu-title">Pattern
                                                        </span>
                                                    </a>
                                                </div>
                                                <!--end::Menu item-->
                                            </div>
                                            <div style="float: right; padding-left: 3px; text-align:left">
                                                <button type="button" id="scriptBtns-btn"
                                                    class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm  tb-btn btn-sm d-none" data-kt-menu-trigger="{default:'click', lg: 'hover'}" data-kt-menu-attach="parent" data-kt-menu-placement="bottom-end">
                                                    <span
                                                        class="material-icons material-icons-style material-icons-2">apps
                                                    </span>
                                                </button>


                                                <div id="scriptBtns-container" class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-title-gray-700 menu-icon-gray-500 menu-active-bg menu-state-color fw-semibold py-4 fs-base w-250px" data-kt-menu="true" data-kt-element="theme-mode-menu">
                                                </div>
                                            </div>

                                            <button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm  tb-btn btn-sm" onclick="_entityForm.openConnectedDataConfiguration(); return false;" title="Data configuration">
                                                <span
                                                    class="material-icons material-icons-style material-icons-2">settings
                                                </span>
                                            </button>

                                        </div>
                                    </div>
                                    <!--end::Toolbar-->


                                </div>
                                <!--begin::Body-->

                                <div class="Tab_Header d-none">

                                    <ul id="dcTabs">
                                    </ul>
                                </div>




                                <div id="Project_Entity_form" class="card-body Project_items_Entity">
                                    <div  id="entityform-container">
                                    </div>
                                    <div id="noMoreRecordsMessage" class="no-more-records-message" style="display: none;">No more records</div>
                                </div>
                            </div>
                        </div>

                        <div id="Entity_management_Right"
                            class="col-xl-2 col-md-3 d-flex flex-column flex-column-fluid vh-100 min-vh-100 d-none">

                            <div class="card card-xl-stretch  flex-root h-1px  ">

                                
                                <div class="card-body  KPI_Section">

                                    <div class="card KC_Items">
                                        <div class="NO-KPI-Items" id="NO-KPI-Items">
                                                <img src="../images/icons/connected-data.png" alt="No configuration" />
                                                <h4>Data configuration not available.</h4>
                                                <p>Use 'Data configuration' option to configure</p>
                                            </div>
                                        <div class="KC_Items_Content d-none" id="KPI-2">

                                            <div class=" Invoice-content-wrap">
                                                <div class="row accordion"> 
                                                    <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap right-customhyperlinks d-none" >
                                                        <a href="#" class="Invoice-item accordion-header accordion-button fs-4 fw-semibold" data-bs-toggle="collapse" data-bs-target="#kt_accordion_connecteddata_hyperlinks" aria-expanded="false" aria-controls="kt_accordion_connecteddata_hyperlinks">
                                                            <div class="Invoice-icon">
                                                                <span class="material-icons material-icons-style material-icons-2">link</span>
                                                            </div>
                                                            <div class="Invoice-content">
                                                                <h6 class="subtitle">Links</h6>
                                                            </div>
                                                        </a>
                                                        <div id="kt_accordion_connecteddata_hyperlinks" class="accordion-collapse collapse show" aria-labelledby="kt_accordion_connecteddata_hyperlinks">
                                                            <div class="cc-metrics-panel">
                                                                <div class="cc-metrics-list">               
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap right-connectedpagesdata d-none">
                                                        <a href="#" class="Invoice-item accordion-header accordion-button fs-4 fw-semibold collapsed" data-bs-toggle="collapse" data-bs-target="#kt_accordion_connectedpagesdata" aria-expanded="false" aria-controls="kt_accordion_connectedpagesdata">
                                                            <div class="Invoice-icon">
                                                                <span class="material-icons material-icons-style material-icons-2">account_tree</span>
                                                            </div>
                                                            <div class="Invoice-content">
                                                                <h6 class="subtitle">Data Points</h6>
                                                            </div>
                                                        </a>
                                                        <div id="kt_accordion_connectedpagesdata" class="accordion-collapse collapse" aria-labelledby="kt_accordion_connectedpagesdata">
                                                            <div class="cc-metrics-panel">
                                                                <div class="cc-metrics-list">               
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                    <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap right-customdatasource d-none" >
                                                        <a href="#" class="Invoice-item accordion-header accordion-button fs-4 fw-semibold" data-bs-toggle="collapse" data-bs-target="#kt_accordion_connecteddata_datasource" aria-expanded="false" aria-controls="kt_accordion_connecteddata_datasource">
                                                            <div class="Invoice-icon">
                                                                <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                                                            </div>
                                                            <div class="Invoice-content">
                                                                <h6 class="subtitle">KPI Data</h6>
                                                            </div>
                                                        </a>
                                                        <div id="kt_accordion_connecteddata_datasource" class="accordion-collapse collapse show" aria-labelledby="kt_accordion_connecteddata_datasource">
                                                            <div class="cc-metrics-panel">
                                                                <div class="cc-metrics-list">               
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div> 

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>

                    </div>






                </div>

            </div>

            <div class="modal fade" id="fieldsModal">
                <div class="modal-dialog modal-md modal-dialog-scrollable">
                    <div class="modal-content">
                        <!-- Modal header -->
                        <div class="modal-header" style="padding: 1.75rem !important">
                            <h4 class="modal-title">Other Data - Fields Selection</h4>
                            <button type="button" title="Close" class=" btn btn-icon btn-danger  btn-active-primary shadow-sm tb-btn btn-sm" onclick="fieldsModelClose()">
                                <span class="material-icons material-icons-style material-icons-2">close</span>
                            </button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body" style="max-height: 65vh">
                            <input type="text" id="searchBar" class="form-control mb-3" placeholder="Search fields..." onkeyup="searchFields()">
                            <div class="container" id="dvModalFields">
                                <div class="container-fluid">
                                    <div class="table-responsive" id="fields-selection1"></div>
                                </div>
                            </div>

                        </div>

                        <div class="modal-footer" style="">
                            <div>
                                <button type="button" onclick="resetRelatedDataFields()" class="btn btn-white btn-sm">Reset</button>
                                <button type="button" onclick="applyRelatedDataFields()" class="btn btn-primary btn-sm">Apply</button>
                            </div>
                        </div>
                    </div>

                    <!-- Modal footer -->

                </div>
            </div>

            <div class="modal fade" id="displayFieldsModal">
                <div class="modal-dialog modal-md modal-dialog-scrollable">
                    <div class="modal-content">
                        <!-- Modal header -->
                        <div class="modal-header" style="padding: 1.75rem !important">
                            <h4 class="modal-title">Other Data - Display Fields Selection</h4>
                            <button type="button" title="Close" class=" btn btn-icon btn-danger  btn-active-primary shadow-sm tb-btn btn-sm" onclick="displayFieldsModelClose()">
                                <span class="material-icons material-icons-style material-icons-2">close</span>
                            </button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body" style="max-height: 65vh">
                            <div class="container" id="dvModalDisplayFields">
                                <div class="container-fluid">
                                    <div class="table-responsive" id="displayfields-selection"></div>
                                </div>
                            </div>

                        </div>

                        <div class="modal-footer" style="">
                            <div>
                                <button type="button" onclick="resetDisplayFields()" class="btn btn-white btn-sm">Reset</button>
                                <button type="button" onclick="applyDisplayFields()" class="btn btn-primary btn-sm">Apply</button>
                            </div>
                        </div>
                    </div>

                    <!-- Modal footer -->

                </div>
            </div>

            
            <div class="modal fade" id="connectedDataConfigModal">
                <div class="modal-dialog modal-dialog-scrollable modal-xl">
                    <div class="modal-content">
                        <!-- Modal header -->
                        <div class="modal-header" style="padding: 1.75rem !important">
                            <h4 class="modal-title">Other Data Configuration</h4>
                            <button type="button" title="Close" class=" btn btn-icon btn-danger  btn-active-primary shadow-sm tb-btn btn-sm" onclick="_entityForm.closeConnectedDataConfiguration()">
                                <span class="material-icons material-icons-style material-icons-2">close</span>
                            </button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body" style="max-height: 75vh">
                            <div class="container" id="dvModalConnectedData">
                                <div class="rounded border">
                                    <div class="hover-scroll-x">
                                        <div class="d-grid">
                                            <ul class="nav nav-tabs flex-nowrap text-nowrap" role="tablist">
                                                <li class="nav-item" role="presentation">
                                                    <a class="nav-link btn btn-active-light btn-color-gray-600 btn-active-color-primary rounded-bottom-0 d-none"
                                                        data-bs-toggle="tab" href="#kt_tab_pane_relateddata" aria-selected="true" role="tab">Related Data Fields selection</a>
                                                </li>
                                                <li class="nav-item" role="presentation">
                                                    <a class="nav-link btn btn-active-light btn-color-gray-600 btn-active-color-primary rounded-bottom-0 active"
                                                        data-bs-toggle="tab" href="#kt_tab_pane_connecteddatametrics" aria-selected="false" tabindex="-1" role="tab">Connected Data Metrics</a>
                                                </li>
                                                <li class="nav-item" role="presentation">
                                                    <a class="nav-link btn btn-active-light btn-color-gray-600 btn-active-color-primary rounded-bottom-0"
                                                        data-bs-toggle="tab" href="#kt_tab_pane_hyperlinks" aria-selected="false" tabindex="-1" role="tab">Hyperlinks</a>
                                                </li>                
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="tab-content" id="connecteddata-config-TabContent">
                                        <div class="tab-pane fade d-none" id="kt_tab_pane_relateddata" role="tabpanel">
                                            <div class="d-flex flex-column flex-md-row rounded border" id="relateddata-container">   
                                                    <div class="container-fluid">
                                                    <div class="table-responsive" id="fields-selection"></div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="tab-pane fade active show" id="kt_tab_pane_connecteddatametrics" role="tabpanel">
                                            <div class="d-flex flex-column flex-md-row rounded border">
                                                <ul class="nav nav-tabs nav-pills flex-row border-0 flex-md-column me-5 mb-3 mb-md-0 fs-6 min-w-lg-200px"
                                                    role="tablist"  id="connectedDataTabMenu">

                                                </ul>
                                                <div class="tab-content" id="connectedDataTabBody">

                                                </div>
                                            </div>
                                        </div>

                                        <div class="tab-pane fade" id="kt_tab_pane_hyperlinks" role="tabpanel">
                                            <div class="container">
                                                <div class="grid-container">                                      
                                                    <table class="table table-bordered table-hover" id="hyperlinkGrid">
                                                        <thead>
                                                            <tr>
                                                                <th>S.No</th>
                                                                <th>Hyperlink Name</th>
                                                                <th>Link</th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="hyperlinkGridBody">
                                                            <!-- Dynamic rows will be added here -->
                                                        </tbody>
                                                    </table>
            
                                                    <div class="mb-3">
                                                        <button id="addRowBtn" class="btn btn-primary">
                                                            <i class="material-icons align-middle">add_circle</i> Add Row
                                                        </button>                                                        
                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="modal-footer" style="">
                            <div>
                                <button type="button" onclick="_entityForm.resetConnectedDataConfig()" class="btn btn-white btn-sm">Reset</button>
                                <button type="button" onclick="_entityForm.applyConnectedDataConfig()" class="btn btn-primary btn-sm">Apply</button>
                            </div>
                        </div>
                    </div>

                    <!-- Modal footer -->

                </div>
            </div>


            <div class="modal fade" id="confirmationModal" tabindex="-1" aria-labelledby="confirmationModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="confirmationModalLabel">Confirm</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            Apply this change for?
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" id="btnAllUsers">All Users</button>
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" id="btnJustMyself">Myself</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <asp:HiddenField ID="hdnScriptsUrlpath" runat="server" />
        <asp:HiddenField ID="hdnEntityFormPageData" runat="server" />        
        <asp:HiddenField ID="hdnEntityFormPageMetaData" runat="server" />        
    </form>


    <!--end::Content-->
    <!--begin::Javascript-->
    <!--begin::Global Javascript Bundle(used by all pages)-->
    <!-- <script src="../assets/js/jquery-3.6.0.min.js"></script> -->

  
    <script>

        function ShowDimmer(status) {

            DimmerCalled = true;
            var dv = $("#waitDiv");

            if (dv.length > 0 && dv != undefined) {
                if (status == true) {

                    var currentfr = $("#middle1", parent.document);
                    if (currentfr) {
                        dv.width(currentfr.width());
                    }
                    dv.show();
                    document.onkeydown = function EatKeyPress() {
                        return false;
                    }
                } else {
                    dv.hide();
                    document.onkeydown = function EatKeyPress() {
                        if (DimmerCalled == true) {
                            return true;
                        }
                    }
                }
            } else {
                //TODO:Needs to be tested
                if (window.opener != undefined) {

                    dv = $("#waitDiv", window.opener.document);
                    if (dv.length > 0) {
                        if (status == true)
                            dv.show();
                        else
                            dv.hide();
                    }
                }
            }
            DimmerCalled = false;
        }


        $(document).ready(function () {


            $("button.rotate").click(function () {
                $(".task-activity-container").toggle();
            });


            var showChar = 450;  // How many characters are shown by default
            var ellipsestext = "...";
            var moretext = "Show more >";
            var lesstext = "Show less";


            $('.more').each(function () {
                var content = $(this).html();

                if (content.length > showChar) {

                    var c = content.substr(0, showChar);
                    var h = content.substr(showChar, content.length - showChar);

                    var html = c + '<span class="moreellipses">' + ellipsestext + '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelink">' + moretext + '</a></span>';

                    $(this).html(html);
                }

            });

            $(".morelink").click(function () {
                if ($(this).hasClass("less")) {
                    $(this).removeClass("less");
                    $(this).html(moretext);
                } else {
                    $(this).addClass("less");
                    $(this).html(lesstext);
                }
                $(this).parent().prev().toggle();
                $(this).prev().toggle();
                return false;
            });
        });

    </script>

    <!--end::Page Custom Javascript-->
    <!--end::Javascript-->
</body>
</html>
