<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddEditResponsibility.aspx.cs" Inherits="aspx_AddEditResponsibility" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>User Access</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction=="ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss" ) %>
    </asp:PlaceHolder>

    <link rel="stylesheet" href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" />
    <link rel="stylesheet" href="../ThirdParty/fancytree-master/src/skin-material/ui.fancytree.css" />

    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
</head>

<body class="ER-Body btextDir-<%=direction%>" dir="<%=direction%>">
    <form id="form1" runat="server">
        <div>
            <asp:PlaceHolder runat="server">
                <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
            </asp:PlaceHolder>
            <asp:ScriptManager runat="server">
            </asp:ScriptManager>
           
            <asp:HiddenField ID="hdnTreeSelVal" runat="server" />

            <div id="dvEditResp" class="container-fluid">
                <div class="row py-3 border-bottom border-gray-200 pe-6" id="top-section">
                    <div class="col-md-6 d-flex" id="editResLabel">
                        <h1 class="text-dark fw-boldest my-1 fs-2 page-caption">Access control for responsibility</h1>
                    </div>
                    <div class="col-md-6 d-flex gap-2 invisible--" id="newRes">
                        <label class="form-label col-form-label required fs-5">Responsibility</label>
                        <span class="form-group w-100 my-auto">
                            <asp:TextBox ID="txtReEditResp" runat="server" MaxLength="50" CssClass="form-control form-control-sm my-auto align-middle" placeholder="Responsibility Name"></asp:TextBox>
                        </span>
                    </div>

                    <div class="col-md-2 d-flex gap-3 justify-content-end--">
                        <label class="form-label col-form-label-- fw-bolder fs-6 my-auto">Show selection only</label>
                        <label class="form-check form-switch form-check-solid-- my-auto toggle-switch-small">
                            <input type="checkbox" id="toggleButtonSmall" class="form-check-input w-45px h-20px" />
                        </label>
                    </div>

                    <div class="col-md-4 my-auto">
                        <div class="form-group">
                            <div id="newMenuSearch" class="search-form d-flex">
                                <input type="text" value="" id="amSearch" runat="server" placeholder="Search..." class="new-search-input search-input ui-autocomplete-input form-control form-control-sm rounded-1--" autocomplete="off" />
                                <span title="Clear" id="GSclearBtn" class="close material-icons material-icons-style d-none" tabindex="0">close</span>
                                <button type="button" id="globalSearchBtn" class="search-button p-0 bg-white border-0 d-none">
                                    <span class="material-icons material-icons-style">search</span>
                                </button>

                                <span id="searchclear" class="material-icons material-icons-style d-none" title="Clear">search</span>
                                <span id="searchPrevPages" class="material-icons material-icons-style d-none" title="Clear">expand_less</span>
                                <span id="searchNextPages" class="material-icons material-icons-style d-none" title="Clear">expand_more</span>
                                <label id="lblSearchRecMsg" class="d-none"></label>

                            </div>
                        </div>
                    </div>

                </div>
                <div class="accessright row px-0 h-100 overflow-hidden border-bottom">
                    <div class="treenode-content col-md-6 col-sm-12 h-450px-- d-flex border-start border-end border-gray-300 pb-10" id="containr" style="height: calc(100vh - 105px);">
                        <div class="container px-0">
                            <div class="row ER-header text-center border-bottom border-gray-200">
                                <label class="col-md-7 form-label col-form-label fw-boldest">Pages</label>
                                <label class="col-md-2 form-label col-form-label fw-boldest">Menu Access</label>
                                <label class="col-md-3 form-label col-form-label fw-boldest">Structure Access</label>
                            </div>
                            <div id="contain" class="row ER-TreeContainer h-100">
                            </div>
                        </div>
                    </div>
                    <div id="iviewpage" class="treenode-content col-md-6 col-sm-12 h-450px-- border-start border-end border-gray-300" style="height: calc(100vh - 105px);">
                        <div id="user_access" class="accessrightcontrol text-center">
                            <label class="form-label col-form-label fw-bolder">User Access</label>
                            <div class="useraccess-nodata d-flex h-400px card card-stretch align-items-center justify-content-center border-top border-gray-200 rounded-0">
                                <img src="../images/useraccess.png" class="h-100px" />
                                <p class="mt-4">
                                    Select the left side items to configure the                                   
                                    <br />
                                    access and controls
                               
                                </p>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="bottom-section border-top-- py-2 position-absolute bottom-0 end-5">
                    <button type="button" class="btn btn-primary btn-sm float-end" id="save_btn">Save</button>
                </div>

                <asp:HiddenField ID="hdnAction" runat="server" />
            </div>
        </div>

        <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
            <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                <span class="loader"></span>
            </div>
        </div>


        <script type="text/javascript" src="../Js/noConflict.min.js?v=1"></script>
        <script type="text/javascript" src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>
        <script type="text/javascript" src="../Js/common.min.js?v=158"></script>
        <script type="text/javascript" src="../Js/alerts.min.js?v=32"></script>
        <script type="text/javascript" src="../Js/umgmt.min.js?v=28"></script>
        <script type="text/javascript" src="../Js/lang/content-<%=langType%>.js?v=64"></script>
        <script type="text/javascript" src="../ThirdParty/fancytree-master/dist/jquery.fancytree-all-deps.min.js?v=1"></script>
        <script type="text/javascript" src="../ThirdParty/fancytree-master/src/jquery-ui-dependencies/jquery.fancytree.ui-deps.js"></script>
        <script type="text/javascript" src="../ThirdParty/fancytree-master/src/jquery.fancytree.js"></script>
        <script type="text/javascript" src="../ThirdParty/fancytree-master/src/jquery.fancytree.dnd5.js"></script>
        <script type="text/javascript" src="../ThirdParty/fancytree-master/src/jquery.fancytree.edit.js"></script>
        <script type="text/javascript" src="../ThirdParty/fancytree-master/src/jquery.fancytree.glyph.js"></script>
    </form>
</body>
</html>
