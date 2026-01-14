<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PluginCustomCode.aspx.cs" Inherits="PluginCustomCode" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Plugin Custom Code</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="../UI/axpertUI/style.bundle.css" />
    <link rel="stylesheet" href="../UI/axpertUI/plugins.bundle.css" />
    <link rel="stylesheet" href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css" />
    <link href="../css/datatables.min.css" rel="stylesheet">

    <link rel="stylesheet" href="../Css/codemirror.css">
    <link rel="stylesheet" href="../ThirdParty/codemirror/addon/hint/show-hint.css">
    <link rel="stylesheet" href="../ThirdParty/codemirror/addon/fold/foldgutter.css">
    <link rel="stylesheet" href="../ThirdParty/codemirror/addon/dialog/dialog.css">
    <link rel="stylesheet" href="../ThirdParty/codemirror/addon/search/matchesonscrollbar.css">

    <link rel="stylesheet" href="../Css/PluginCustomCode.min.css?v=2">

</head>

<body id="Entitymanagement_Body" class="header-fixed header-tablet-and-mobile-fixed aside-fixed gradTheme" runat="server">
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
                    <div id="Entity_Summary_Wrapper" class="row Entity_Summary_Wrapper">
                        <div class="card-header Page-title">
                            <!--begin::Title-->
                            <h3 class="card-title align-items-start flex-column col-md-3">
                                <span class="card-label fw-bold text-gray-900">Plugin Custom Code</span>
                            </h3>
                            <div class="d-flex flex-column flex-column-fluid col-md-7" style="overflow-x: auto;">
                                <div class="scroll-container">

                                    <div class="d-flex flex-column flex-column-fluid col-md-9 scrollable-menu">
                                        <ul id="dv_EntityContainer" class="nav nav-stretch nav-line-tabs nav-line-tabs-2x border-transparent fs-5 fw-bold dv_EntityContainer">
                                        </ul>
                                    </div>
                                </div>

                            </div>

                            <div class="card-toolbar ">
                                <div class="Tkts-toolbar-Right ">

                                    <div class="menu-item px-3 my-0" id="themeMenuItem" data-kt-menu-trigger="hover" data-kt-menu-placement="bottom">
                                        <a href="#" class="menu-link px-5 btn btn-primary me-2 mb-2">
                                            <span class="menu-title">Plugin custom code</span>
                                        </a>

                                        <div class="menu-sub menu-sub-dropdown menu-column menu-rounded menu-title-gray-700 menu-icon-gray-500 menu-active-bg menu-state-color fw-semibold py-4 fs-base w-250px"
                                            data-kt-menu="true" data-kt-element="theme-mode-menu">
                                            <div class="menu-item px-3 my-0">
                                                <a href="#" class="menu-link px-3 py-2" data-target="tstruct" onclick="_customPlugins.loadAddFilesFields('tstruct')">
                                                    <span class="material-icons material-icons-style material-icons-2">assignment</span>
                                                    <span class="menu-title">Customize Tstructs Forms</span>
                                                </a>
                                            </div>
                                            <div class="menu-item px-3 my-0">
                                                <a href="#" class="menu-link px-3 py-2" data-target="report" onclick="_customPlugins.loadAddFilesFields('report')">
                                                    <span class="material-icons material-icons-style material-icons-2">view_list</span>
                                                    <span class="menu-title">Customize Iview Reports</span>
                                                </a>
                                            </div>
                                            <div class="menu-item px-3 my-0">
                                                <a href="#" class="menu-link px-3 py-2" data-target="report" onclick="_customPlugins.loadAddFilesFields('datapage')">
                                                    <span class="material-icons material-icons-style material-icons-2">assignment</span>
                                                    <span class="menu-title">Customize Data Pages</span>
                                                </a>
                                            </div>
                                            <div class="menu-item px-3 my-0">
                                                <a href="#" class="menu-link px-3 py-2" data-target="report" onclick="_customPlugins.loadAddFilesFields('datalist')">
                                                    <span class="material-icons material-icons-style material-icons-2">view_list</span>
                                                    <span class="menu-title">Customize Data List</span>
                                                </a>
                                            </div>
                                            <div class="menu-item px-3 my-0">
                                                <a href="#" class="menu-link px-3 py-2" data-target="report" onclick="_customPlugins.loadAddFilesFields('htmlplugin')">
                                                    <span class="material-icons material-icons-style material-icons-2">html</span>
                                                    <span class="menu-title">Add HTML Plugins</span>
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div id="Entity_summary_Left"
                            class=" col-xl-3 col-md-3  d-flex flex-column flex-column-fluid   vh-100 min-vh-100 ">
                            <div class="card card-flush h-lg-100  ">

                                <div class="card-body">
                                    <div id="Data-Group-container">
                                        <div class="accordion filemanager-accordion accordion-icon-toggle" onclick="_customPlugins.openFileManager()">
                                            <div class="accordion-header">
                                                File Manager                                           
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>

                        <div id="Entity_summary_Right"
                            class="col-xl-9 col-md-9 d-flex flex-column flex-column-fluid vh-100 min-vh-100 d-none">

                            <div class="card card-xl-stretch  flex-root h-1px ">
                                <div id="Summary-Results" class="row card-body scroller">


                                    <div class="col-md-12 analytics-container">

                                        <div class="card-header">
                                            <div class="Chart-Header" id="analytics-chart-title">
                                            </div>
                                        </div>


                                        <div class="card-body Summary_Charts">
                                            <textarea id="codeEditor"></textarea>
                                        </div>
                                    </div>


                                </div>

                                <div class="card-footer-bottom file-footer d-none">
                                    <div class="rounded border p-5 d-flex flex-wrap gap-2">
                                        <a href="#" class="btn btn-primary w-75px" onclick="return _customPlugins.updateFile()">Save</a>
                                        <a href="#" class="btn btn-danger  w-75px" onclick="return _customPlugins.deleteFile()">Delete</a>
                                    </div>
                                </div>
                                <div class="card-footer-bottom plugin-footer d-none">
                                    <div class="rounded border p-5 d-flex flex-wrap gap-2">
                                        <a href="#" class="btn btn-primary w-75px" onclick="return _customPlugins.updatePlugin()">Save</a>
                                        <a href="#" class="btn btn-danger  w-75px" onclick="return _customPlugins.deletePlugin()">Delete</a>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="Entity_summary_Right_Empty"
                            class="col-xl-9 col-md-9 d-flex flex-column flex-column-fluid vh-100 min-vh-100">
                            <div class="card card-xl-stretch  flex-root h-1px ">

                                <div id="Summary-Results2" class="row card-body scroller">

                                    <div class="content d-flex flex-column flex-column-fluid fs-6" id="kt_content">
                                        <div class="container-xxl" id="kt_content_container2">
                                            <div class="card">
                                                <div class="card-body p-lg-17">
                                                    <div class="mb-18">
                                                        <div class="mb-10">
                                                            <div class="text-center mb-15">
                                                                <h3 class="fs-2hx text-dark mb-5">Personalize your application UI</h3>
                                                                <div class="fs-5 text-muted fw-semibold">
                                                                    Refer the following link for more samples / references: <a target="_blank" href="https://github.com/Agileaxpert/Axpert">Axpert Samples</a>
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
                        <div id="FileManager-container"
                            class="col-xl-9 col-md-9 d-flex flex-column flex-column-fluid vh-100 min-vh-100 d-none">
                            <div class="card card-xl-stretch  flex-root h-1px ">

                                <div class="container mt-4">
                                    <div class="card">
                                        <div class="card-body">
                                            <!-- Tabs -->
                                            <ul class="nav nav-tabs nav-stretch nav-line-tabs nav-line-tabs-2x border-transparent fs-5 fw-bold dv_EntityContainer" id="fileManagerTabs" role="tablist">
                                                <li class="nav-item" data-bs-toggle="tab" data-bs-target="#files"
                                                         role="tab" aria-controls="files" aria-selected="true">
                                                    <a class="nav-link active" id="files-tab" data-bs-toggle="tab" data-bs-target="#files"
                                                         role="tab" aria-controls="files" aria-selected="true">
                                                        Files
                                                    </a>
                                                </li>

                                                <li class="nav-item" data-bs-toggle="tab" data-bs-target="#new-folder"
                                                         role="tab" aria-controls="new-folder" aria-selected="false">
                                                    <a class="nav-link" id="new-folder-tab" data-bs-toggle="tab" data-bs-target="#new-folder"
                                                         role="tab" aria-controls="new-folder" aria-selected="false">
                                                        Create New Folder
                                                    </a>
                                                </li>
                                                <li class="nav-item" data-bs-toggle="tab" data-bs-target="#upload"
                                                        role="tab" aria-controls="upload" aria-selected="false">
                                                    <a class="nav-link" id="upload-tab" data-bs-toggle="tab" data-bs-target="#upload"
                                                        role="tab" aria-controls="upload" aria-selected="false">
                                                        Upload File
                                                    </a>
                                                </li>
                                            </ul>

                                            <!-- Tab Content -->
                                            <div class="tab-content" id="fileManagerTabContent">
                                                <!-- Files Tab -->
                                                <div class="tab-pane fade show active" id="files" role="tabpanel" aria-labelledby="files-tab">
                                                    <div class="accordion" id="folderAccordion">
                                                        <!-- Folders will be populated here -->
                                                    </div>
                                                </div>

                                                <!-- Create New Folder Tab -->
                                                <div class="tab-pane fade" id="new-folder" role="tabpanel" aria-labelledby="new-folder-tab">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="mb-3">
                                                                <label for="parentFolder" class="form-label">Parent Folder</label>
                                                                <select id="parentFolder" class="form-select">
                                                                    <!-- Folders will be populated here -->
                                                                </select>
                                                            </div>
                                                            <div class="mb-3">
                                                                <label for="newFolderName" class="form-label">New Folder Name</label>
                                                                <input type="text" id="newFolderName" class="form-control" placeholder="Enter folder name" />
                                                            </div>
                                                            <button type="button" id="createFolderBtn" class="btn btn-primary" onclick="_customPlugins.createFolder(); return false;">
                                                                Create Folder
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>

                                                <!-- Upload File Tab -->
                                                <div class="tab-pane fade" id="upload" role="tabpanel" aria-labelledby="upload-tab">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="mb-3">
                                                                <label for="uploadFolder" class="form-label">Select Folder</label>
                                                                <select id="uploadFolder" class="form-select">
                                                                    <!-- Folders will be populated here -->
                                                                </select>
                                                            </div>
                                                            <div class="mb-3">
                                                                <label for="fileUpload" class="form-label">Choose File</label>
                                                                <input type="file" id="fileUpload" class="form-control" />
                                                            </div>
                                                            <div class="mb-3">
                                                                <label for="customFileName" class="form-label">Custom Filename (optional)</label>
                                                                <input type="text" id="customFileName" class="form-control" placeholder="Enter custom filename" />
                                                            </div>
                                                            <button type="button" id="uploadBtn" class="btn btn-primary" onclick="_customPlugins.uploadFile(); return false;">
                                                                Upload File
                                                            </button>
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


        <asp:HiddenField ID="hdnAnalyticsPageLoadData" runat="server" />



        <div class="modal fade" id="filterModal">
            <div class="modal-dialog modal-sm modal-dialog-scrollable">
                <div class="modal-content">
                    <!-- Modal header -->
                    <div class="modal-header" style="padding: 1.75rem !important">
                        <h4 class="modal-title">Plugin Custom Code</h4>
                        <button type="button" title="Close"
                            class=" btn btn-icon btn-danger btn-active-primary shadow-sm tb-btn btn-sm"
                            onclick="_customPlugins.closeModalDialog()">
                            <span class="material-icons material-icons-style material-icons-2">close</span>
                        </button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">
                        <div class="container">
                            <div class="modal-body Entity_Summary_Wrapper" id="Add-Custom-Files" style="max-height: 65vh">


                                <div class="card">
                                    <div class="card-body px-6 py-2 mb-3">
                                        <div class="container" id="dvAddFiles">
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer d-flex flex-row justify-content-between w-100">

                        <div class="action_buttons d-inline-flex gap-2" style="margin-left: 90%;">

                            <button type="button" onclick="_customPlugins.addFile()" class="btn btn-primary">Add File</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnStructures" runat="server" />
    </form>



    <script type="text/javascript" src="../UI/axpertUI/plugins.bundle.js"></script>
    <script type="text/javascript" src="../UI/axpertUI/scripts.bundle.js"></script>
    <script type="text/javascript" src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js"></script>
    <script type="text/javascript" src="../Js/common.min.js?v=146"></script>
    <script type="text/javascript" src="../Js/alerts.min.js"></script>
    <script type="text/javascript" src="../Js/xmlToJson.js"></script>
    <script type="text/javascript" src="../Js/handlebars.min.js"></script>
    <script type="text/javascript" src="../ThirdParty/DataTables-1.10.13/extensions/Extras/moment.min.js"></script>

    <script src="../js/datatables.min.js"></script>

    <script type="text/javascript" src="../Js/codemirror/codemirror.js"></script>
    <script type="text/javascript" src="../Js/codemirror/mode/htmlmixed.js"></script>
    <script type="text/javascript" src="../Js/codemirror/mode/css.js"></script>
    <script type="text/javascript" src="../Js/codemirror/mode/javascript.js"></script>
    <script type="text/javascript" src="../Js/codemirror/mode/xml.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/edit/matchbrackets.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/edit/closebrackets.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/hint/show-hint.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/hint/html-hint.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/hint/css-hint.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/hint/javascript-hint.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/hint/xml-hint.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/fold/foldgutter.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/fold/foldcode.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/display/autorefresh.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/dialog/dialog.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/search/searchcursor.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/search/search.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/search/matchesonscrollbar.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/search/match-highlighter.js"></script>
    <script type="text/javascript" src="../ThirdParty/codemirror/addon/search/jump-to-line.js"></script>

    <script src="../js/Entity-common.min.js?v=14"></script>
    <script type="text/javascript" src="../Js/PluginCustomCode.min.js?v=4"></script>


</body>
</html>
