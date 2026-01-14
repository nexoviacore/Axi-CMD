<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Analytics.aspx.cs" Inherits="aspx_Analytics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Axpert Analytics</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="../UI/axpertUI/style.bundle.css" />
    <link rel="stylesheet" href="../UI/axpertUI/plugins.bundle.css" />
    <link rel="stylesheet" href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css" />
    <link rel="stylesheet" href="../css/analytics.min.css?v=4" />
</head>

<body id="Entitymanagement_Body" class="btextDir-<%=direction%> header-fixed header-tablet-and-mobile-fixed aside-fixed gradTheme"  dir="<%=direction%>">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
                <asp:ServiceReference Path="../CustomWebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <!--begin::Content-->
        <div class="content d-flex flex-column flex-column-fluid " style="padding: 0px !important">
            <!--begin::Container-->
            <div id="kt_content_container" class="">
                <!--begin::Row-->
                <div id="Entity_Summary_Body-overalldiv" class="Entity_Summary">
                    <!--begin:::Col-->
                    <div id="Entity_Summary_Wrapper" class="row">
                        <div class="card-header Page-title">
                            <!--begin::Title-->
                            <h3 class="card-title align-items-start flex-column col-md-1">
                                <span class="card-label fw-bold text-gray-900">Analytics</span>
                            </h3>
                            <div class="d-flex flex-column flex-column-fluid col-md-9" style="overflow-x: auto;">
                                <div class="scroll-container">
                                    <div class="scroll-btn left-btn" onclick="scrollLeft()">

                                        <span class="material-icons material-icons-style material-icons-2">chevron_left </span>
                                    </div>

                                    <div class="d-flex flex-column flex-column-fluid col-md-9 scrollable-menu">
                                        <ul id="dv_EntityContainer" class="nav nav-stretch nav-line-tabs nav-line-tabs-2x border-transparent fs-5 fw-bold">
                                            <!-- Dynamic items will be appended here -->
                                        </ul>
                                    </div>

                                    <div class="scroll-btn right-btn" onclick="scrollRight()">
                                        <span class="material-icons material-icons-style material-icons-2">chevron_right </span>
                                    </div>
                                </div>

                            </div>



                            <!--end::Title-->
                            <!--begin::Toolbar-->
                            <div class="card-toolbar ">
                                <div class="Tkts-toolbar-Right ">
                                    <button id="btn_Add_Entity" type="button" onclick="openEntitySelection()" title="Add Entities"
                                        class="btn btn-sm btn-icon btn-primary btn-active-primary btn-custom-border-radius">
                                        <span class="material-icons material-icons-style material-icons-2">add</span>
                                    </button>
                                    <button id="btn_selectfields" type="button" onclick="openFieldSelection()" title="Configure fields"
                                        class="btn btn-sm btn-icon btn-custom-border-radius">
                                        <span class="material-icons material-icons-style material-icons-2">settings </span>
                                    </button>
                                    <!-- Button to Apply Filters -->
                                    <button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm d-none"
                                        data-toggle="tooltip" title="Apply Filter(s)" onclick="openFilters(); return false;">
                                        <span class="material-icons material-icons-style material-icons-2">tune</span>
                                    </button>
                                </div>
                            </div>
                            <!--end::Toolbar-->
                        </div>



                        <!-- <div class="Chart-Header">
                            <div id="analytics-chart-title" class="col-md-12  analytics-container">
                                <div class="direction-btns ">
                                    <button id="" type="button" onclick="moveselection('left')" class="btn btn-sm btn-icon btn-primary btn-active-primary btn-custom-border-radius">
                                        <span class="material-icons material-icons-style material-icons-2">arrow_back</span>
                                    </button>
                                    <button type="button" id="" onclick="moveselection('right')" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm">
                                        <span class="material-icons material-icons-style material-icons-2">arrow_forward</span>
                                    </button>
                                    <button type="button" id="" onclick="moveselection('up')" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm">
                                        <span class="material-icons material-icons-style material-icons-2">arrow_upward</span>
                                    </button>
                                    <button type="button" id="" onclick="moveselection('down')" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm  tb-btn btn-sm">
                                        <span class="material-icons material-icons-style material-icons-2">arrow_downward</span>
                                    </button>
                                </div>
                                <div id="chart-title" class="d-inline"></div>
                                <div class="d-inline chart-selections">
                                    <button type="button" onclick="chartSelectionClick(event,this)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm bg-success" chart_type="pie">
                                        <span class="material-icons material-icons-style material-icons-2">pie_chart</span>
                                    </button>
                                    <button type="button" onclick="chartSelectionClick(event,this)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm" chart_type="column">
                                        <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                                    </button>
                                    <button type="button" onclick="chartSelectionClick(event,this)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm" chart_type="bar">
                                        <span class="material-icons material-icons-style material-icons-2" style="writing-mode: vertical-rl;">bar_chart</span>
                                    </button>
                                   
                                </div>

                            </div>

                        </div> -->



                        <div id="Entity_summary_Left"
                            class=" col-xl-2 col-md-2  d-flex flex-column flex-column-fluid   vh-100 min-vh-100 ">
                            <div class="card card-flush h-lg-100  ">

                                <!--begin::Body-->
                                <div class="card-body">
                                    <div id="Data-Group-container">
                                    </div>
                                </div>
                                <!-- <div class="card-footer d-none">
                                <div class="Data-Group_Items">
                                    <a href="">
                                        <div class="d-flex ">
                                            <div class="symbol symbol-40px symbol-circle me-5">
                                                <div class="symbol-label bgs1 bg-success">
                                                    <span class="material-icons material-icons-style material-icons-2">add</span>
                                                </div>
                                            </div>
                                            <div class="d-flex flex-column">
                                                <span class="Data-Group-name">Add New View</span>
                                                <span class="Data-Group-label">Reports
                                                </span>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                            </div> -->

                            </div>
                        </div>

                        <div id="Entity_summary_Right"
                            class="col-xl-10 col-md-10 d-flex flex-column flex-column-fluid vh-100 min-vh-100">

                            <div class="card card-xl-stretch  flex-root h-1px ">

                                <!--begin::Body-->
                                <!-- <div id="" class="row m-auto" style="justify-content: flex-start !important;">
                                    <h3></h3>
                                </div> -->
                                <div id="Summary-Results" class="row card-body scroller">


                                    <div class="col-md-8 analytics-container">
                                        <!-- Analytics Chart Title and Selection Buttons -->
                                        <div class="card-header">
                                            <div class="Chart-Header" id="analytics-chart-title">
                                                <!-- Direction Buttons and Chart Title Centered -->
                                                <div class="direction-btns d-flex align-items-center justify-content-between">
                                                    <!-- Left Button -->
                                                    <a href="javascript:void(0);" onclick="moveselection('left')" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm">
                                                        <span class="material-icons material-icons-style material-icons-2">arrow_back</span>
                                                    </a>

                                                    <!-- Chart Title in the Center -->
                                                    <div id="chart-title" class="mx-2 text-center"></div>

                                                    <!-- Right Button -->
                                                    <a href="javascript:void(0);" onclick="moveselection('right')" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm">
                                                        <span class="material-icons material-icons-style material-icons-2">arrow_forward</span>
                                                    </a>
                                                </div>

                                                <!-- Chart Type Selection Buttons with Grouping -->
                                                <div class="d-inline chart-selections mt-3">
                                                    <div class="btn-group">
                                                        <button class="btn btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                                                            <span class="material-icons material-icons-style material-icons-2 chart-icon">pie_chart</span>
                                                            <span class="chart-name">Pie </span>
                                                            <!-- Chart name will be updated dynamically -->
                                                        </button>


                                                        <ul class="dropdown-menu">
                                                            <li>
                                                                <a href="javascript:void(0);" onclick="chartSelectionClick(event, this)" class="dropdown-item menu-link" chart_type="pie">
                                                                    <span class="material-icons material-icons-style material-icons-2">pie_chart</span>
                                                                    <span class="chart-name">Pie </span>
                                                                </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:void(0);" onclick="chartSelectionClick(event, this)" class="dropdown-item menu-link" chart_type="column">
                                                                    <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                                                                    <span class="chart-name">Column </span>
                                                                </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:void(0);" onclick="chartSelectionClick(event, this)" class="dropdown-item menu-link" chart_type="bar">
                                                                    <span class="material-icons material-icons-style material-icons-2" style="writing-mode: vertical-rl;">bar_chart</span>
                                                                    <span class="chart-name">Bar </span>
                                                                </a>
                                                            </li>
                                                        </ul>


                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="card-body Summary_Charts">



                                            <!-- Card Content -->
                                            <div class="card Summary_Charts_Items">
                                                <div class="card-header collapsible cursor-pointer rotate collapsed d-none"
                                                    data-bs-toggle="collapse" aria-expanded="false" data-bs-target="#KPI-3">
                                                    <h3 class="card-title" id="chartTitle">KPI Title</h3>
                                                    <div class="card-toolbar rotate-180">
                                                        <a href="#" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm d-none">
                                                            <span class="material-icons material-icons-style material-icons-2">visibility_off</span>
                                                        </a>
                                                        <a href="#" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm d-none"
                                                            data-kt-menu-trigger="{default:'click', lg: 'hover'}"
                                                            data-kt-menu-attach="parent" data-kt-menu-placement="bottom-end">
                                                            <span class="material-icons material-icons-style material-icons-2">more_vert</span>
                                                        </a>
                                                        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-title-gray-700 menu-icon-gray-500 menu-active-bg menu-state-color fw-semibold py-4 fs-base w-150px" data-kt-menu="true" data-kt-element="theme-mode-menu">
                                                            <div class="menu-item px-3 my-0">
                                                                <a href="#" class="menu-link px-3 py-2" data-kt-element="mode" data-kt-value="pie">
                                                                    <span class="material-icons material-icons-style material-icons-2">pie_chart</span>
                                                                    <span class="menu-title">Pie Chart</span>
                                                                </a>
                                                            </div>
                                                            <div class="menu-item px-3 my-0">
                                                                <a href="#" class="menu-link px-3 py-2" data-kt-element="mode" data-kt-value="column">
                                                                    <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                                                                    <span class="menu-title">Bar Chart</span>
                                                                </a>
                                                            </div>
                                                            <div class="menu-item px-3 my-0">
                                                                <a href="#" class="menu-link px-3 py-2" data-kt-element="mode" data-kt-value="line">
                                                                    <span class="material-icons material-icons-style material-icons-2">show_chart</span>
                                                                    <span class="menu-title">Line Chart</span>
                                                                </a>
                                                            </div>
                                                        </div>
                                                        <span class="material-icons material-icons-style material-icons-2">expand_circle_down</span>
                                                    </div>
                                                </div>
                                                <div class="collapse Summary_Charts_Items_Content show" id="KPI-3">
                                                    <div class="cardsPlot mb-8" id="Homepage_CardsList">
                                                        <div class="" id="Homepage_CardsList_Wrapper"></div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>




                                    <div class="col-md-4 analytics-container">
                                        <div class="card KC_Items">
                                            <div class="collapse KC_Items_Content show" id="KPI-2">
                                                <div class=" Invoice-content-wrap">
                                                    <div class="row">
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- <p class="nodata">No data found.</p> -->
                                </div>
                                <div class="card-footer-bottom">
                                    <div id="Aggregation_Wrapper" class="card-header row" style="justify-content: flex-start !important;">
                                    </div>

                                </div>
                            </div>
                        </div>


                    </div>
                </div>
                <!--end:::Col-->
                <!--begin:::Col-->
                <!--end::Container-->
            </div>

            <!-- Bootstrap Modal HTML -->
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

            <div class="modal fade" id="entityModal">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Add Entities</h4>
                            <div class="search-container">
                                <div class="position-relative d-flex align-items-center">
                                    <span class="material-icons material-icons-style material-icons-2 material-icons-lg-3 cursor-default position-absolute top-50 translate-middle-y ms-0 ms-lg-4 text-gray-500">search</span>
                                    <input id="searchEntity" type="text" class="search-input form-control form-control-transparent ps-8" name="search" value="" placeholder="Search Entities..." data-kt-search-element="input" />
                                </div>
                            </div>
                            <button type="button" title="Close" class="btn btn-icon btn-danger btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm" onclick="entityModelClose()">
                                <span class="material-icons material-icons-style material-icons-2">close</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="HomePageCards mb-8 col-md-12">
                                <div class="card">
                                    <div class="card-body px-6 py-2 mb-3">

                                        <div id="selectedEntitiesContainer" class="row d-flex" style="margin-bottom: 20px;"></div>
                                        <label id="pendingLabel" class="pending-name">Entities To Add</label>
                                        <div id="entityDataContainer" class="row d-flex"></div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="modal-footer">
                            <button type="button" onclick="resetSelection()" class="btn btn-white btn-sm shadow-sm">Reset</button>
                            <button type="button" onclick="saveSelectedEntities()" class="btn btn-primary btn-sm shadow-sm">Apply</button>
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
                        <h4 class="modal-title">Fields Selection</h4>
                        <button type="button" title="Close" class=" btn btn-icon btn-danger btn-active-primary shadow-sm tb-btn btn-sm" onclick="fieldsModelClose()">
                            <span class="material-icons material-icons-style material-icons-2">close</span>
                        </button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body" style="max-height: 65vh">
                        <div class="container" id="dvModalFields">
                            <div class="container-fluid">
                                <div class="table-responsive">
                                    <table class="table table-hover">
                                        <thead class="thead-dark">
                                            <tr>
                                                <th>
                                                    <input type="checkbox" id="check-all" /></th>
                                                <th>Field Name</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                                <div class="table-responsive" id="fields-selection"></div>
                            </div>


                        </div>

                    </div>

                    <div class="modal-footer" style="">
                        <div>
                            <button type="button" onclick="resetFields()" class="btn btn-white btn-sm">Reset</button>
                            <button type="button" onclick="applyFields()" class="btn btn-primary btn-sm">Apply</button>
                        </div>
                    </div>
                </div>

                <!-- Modal footer -->

            </div>
        </div>
        </div>

        <!-- Modal for Filters -->
        <div class="modal fade" id="filterModal">
            <div class="modal-dialog modal-lg modal-dialog-scrollable">
                <div class="modal-content">
                    <!-- Modal header -->
                    <div class="modal-header" style="padding: 1.75rem !important">
                        <h4 class="modal-title">Filters</h4>
                        <button type="button" title="Close"
                            class=" btn btn-icon btn-danger btn-active-primary shadow-sm tb-btn btn-sm"
                            onclick="filterModelClose()">
                            <span class="material-icons material-icons-style material-icons-2">close</span>
                        </button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body" style="max-height: 65vh">
                        <div class="container" id="dvModalFilter"></div>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer d-flex flex-row justify-content-between w-100">
                        <div class="filter_group_checkbox_input_wrapper mt-2 d-flex align-items-center gap-5"
                            data-type="checkbox" style="flex-basis: 75%;">
                            <div class="filter_group_checkbox_wrapper d-inline-flex gap-2">
                                <label for="filter_group_checkbox" style="flex-basis: max-content;">
                                    Save
                                    filters with a custom name ?</label>
                                <input type="checkbox" name="filter_group_checkbox" id="filterGroupCheckbox"
                                    style="flex-basis: fit-content;"
                                    onchange="$('#filterGroupName').prop('disabled', !this.checked);">
                            </div>

                            <input type="text" name="filterGroupName" id="filterGroupName" class="col-md-7"
                                disabled>
                        </div>
                        <div class="action_buttons d-inline-flex gap-2">
                            <button type="button" onclick="resetFilters()"
                                class="btn btn-white btn-sm">
                                Reset</button>
                            <!-- Add an ID to the button -->
                            <button type="button" id="applyFilterButton" class="btn btn-primary">Apply</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdnAnalyticsPageLoadData" runat="server" />
    </form>

    <!--end::Content-->
    <!--begin::Javascript-->
    <!--begin::Global Javascript Bundle(used by all pages)-->
    <!-- <script src="../assets/js/jquery-3.6.0.min.js"></script> -->

    <script type="text/javascript" src="../UI/axpertUI/plugins.bundle.js"></script>
    <script type="text/javascript" src="../UI/axpertUI/scripts.bundle.js"></script>
    <script type="text/javascript" src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js"></script>
    <script type="text/javascript" src="../Js/common.min.js"></script>
    <script type="text/javascript" src="../Js/alerts.min.js"></script>
    <script type="text/javascript" src="../Js/xmlToJson.js"></script>
    <script type="text/javascript" src="../ThirdParty/DataTables-1.10.13/extensions/Extras/moment.min.js"></script>
    <script src="../js/Entity-Filter.min.js?v=8"></script>
    <script src="../js/Entity-common.min.js?v=14"></script>
    <script src="../js/Analytics.min.js?v=11"></script>


    <!--end::Page Custom Javascript-->
    <!--end::Javascript-->
</body>




</html>

