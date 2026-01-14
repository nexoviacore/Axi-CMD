<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArrangeMenu.aspx.cs" Inherits="aspx_ArrangeMenu" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="IView" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>ArrangeMenu</title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render("~/Css/ArrangeMenu") %>
    </asp:PlaceHolder>
    <% if (direction == "rtl")
        { %>
    <link rel="stylesheet" href="../ThirdParty/bootstrap_rtl.min.css" type="text/css" />
    <% } %>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script>
        var menuData = '<%=menuData%>';
    </script>
    <style>
        .card .card-header {
            display: flex;
            justify-content: space-between;
            align-items: stretch;
            flex-wrap: wrap;
            /*min-height: 70px;*/
            padding: 0 2.25rem;
            color: var(--bs-card-cap-color);
            background-color: var(--bs-card-cap-bg);
            border-bottom: 1px solid var(--bs-card-border-color);
        }

            .card .card-header .card-title {
                display: flex;
                align-items: center;
                margin: .5rem;
                margin-left: 0;
            }

                .card .card-header .card-title, .card .card-header .card-title .card-label {
                    font-weight: 500;
                    font-size: 24px !important;
                    color: var(--bs-text-gray-900);
                }

            .card .card-header .card-toolbar {
                display: flex;
                align-items: center;
                margin: .5rem 0;
                flex-wrap: wrap;
            }

        .btn > span {
            display: inline-flex;
            font-size: 1rem;
            padding-right: .35rem;
            vertical-align: middle;
        }

        a.btn {
            margin-right: 4px
        }
    </style>
</head>
<body class="btextDir-<%=direction%>" dir="<%=direction%>">
    <form id="form1" runat="server">
        <asp:PlaceHolder runat="server">
            <%:Scripts.Render("~/Js/ArrangeMenuJS") %>
        </asp:PlaceHolder>
        <div class="card">
            <div class="card-header">
                <h4 class="card-title mb-4">Arrange Menu
                    <div class="developerbreadcrumb-panel">
                        <%--<div class="developerbreadcrumb icon-services left">
                            <span class="developerbreadcrumbTitle">Arrange Menu</span>
                        </div>--%>
                        <div class="developerSearch">
                        </div>
                    </div>
                </h4>
                <div class="card-toolbar">
                    <a href="javascript:void(0);" id="lnkChngIcon" title="Change Icon" data-bs-toggle="tooltip" class="btn btn-sm btn-warning">Change Icon
                    </a>
                    <a href="javascript:void(0);" id="btnAdd" title="Add Folder" data-bs-toggle="tooltip" class="btn btn-sm btn-primary">Add Folder</a>
                    <a href="javascript:void(0);" id="btnDelete" title="Delete" data-bs-toggle="tooltip" class="btn btn-sm btn-danger">Delete</a>
                    <a href="javascript:void(0);" id="btnSave" title="Save" data-bs-toggle="tooltip" class="btn btn-sm btn-success">Save</a>
                </div>
            </div>
            <div class="card-body">
                <div class="amSearchWrapper">
                    <div class="col-lg-5 col-md-5 col-sm-5 amSearchInput">
                        <input id="amSearch" name="search" class="form-control" placeholder="Search..." autocomplete="off" />
                        <button id="amResetSearch">
                            <span class="material-icons">clear</span>
                        </button>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div id="arrangeMenu">
                </div>
                <div id="triggerPopover"></div>
                <div id="arngmnuFooter" class="hide">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <ajaxToolkit:AsyncFileUpload runat="server" ID="uploadIcon" ClientIDMode="AutoID" OnUploadedComplete="uploadIcon_Click" OnClientUploadStarted="validateFile" OnClientUploadComplete="SetNodeIcon" OnClientUploadError="uploadError"></ajaxToolkit:AsyncFileUpload>
                    <button type="button" id="lnkChngIcon" title="Change Icon" class="btn btn-default coldbtn">Change Icon</button>
                    <button type="button" id="btnAdd" title="Add Folder" class="btn btn-default coldbtn">Add Folder</button>
                    <button type="button" id="btnDelete" title="Delete" class="btn btn-default coldbtn">Delete</button>
                    <button type="button" id="btnSave" title="Save" class="btn btn-default coldbtn">Save</button>
                    <asp:HiddenField runat="server" ID="hdnUserIconList" />
                    <asp:HiddenField runat="server" ID="hdnIconPath" />
                </div>
            </div>
        </div>
        <%-- <div id='waitDiv'>
            <div id='backgroundDiv'>
            </div>
        </div>--%>
        <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
            <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                <span class="loader"></span>
            </div>
        </div>
    </form>
</body>
</html>
