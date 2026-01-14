<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ParamsTstruct.aspx.cs" Inherits="ParamsTstruct" %>


<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Axpert Tstruct" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>ParamsTstruct</title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render("~/Css/form") %>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
    <script>
        if (typeof localStorage != "undefined") {
            var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            var customStyleTimeStamp = localStorage["customTstructNewUiExist-" + projUrl];
            if (typeof customStyleTimeStamp != "undefined" && customStyleTimeStamp != null)
                customStyleTimeStamp = customStyleTimeStamp.replace(/[^a-zA-Z0-9_\-\/]/g, "");
            if (customStyleTimeStamp && customStyleTimeStamp != "false") {
                var appProjName = localStorage["projInfo-" + projUrl] || "";
                if (typeof appProjName != "undefined" && appProjName != null)
                    appProjName = appProjName.replace(/[^a-zA-Z0-9_\-\/]/g, "");
                var customFormStyle = "<link id=\"customTstructNewUi\" data-proj=\"" + appProjName + "\" href=\"../" + appProjName + "/customTstructNewUi.css?v=" + customStyleTimeStamp + "\" rel=\"stylesheet\" />";
                document.write(customFormStyle);
            }
        }
    </script>

    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
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
    <script type="text/javascript">
        var mode = "form";
        //variables used for new picklist control
        var totalPLRows = 0;
        var pageSize = 10;
        var curPageNo = 1;
        var noOfPLPages = 0;
        var isTstPop = '<%=isTstPop%>';
        var appsessionKey = '<%=appsessionKey%>';
        var firstInput;
        var lastInput;
        var FetchPickListRows = '<%=FetchPickListRows%>';
        //variables used for storing images in folders        
        var imgNames = new Array();
        var imgSrc = new Array();
        var axInlineGridEdit = '<%=Session["AxInlineGridEdit"]%>' == 'true';
        var enableBackButton = false;
        var enableForwardButton = false;
        var AxwizardType = '<%=axWizardType%>';
        var wizardLoadRecord = '<%=wizardLoadRecord%>';
        var tracePath = '<%=traceLog%>';
        var gloDraft = '<%=tstDraftsScript%>';
        var AxpTstButtonStyle = '<%=AxpTstButtonStyle%>';
        var btnfooterlist = "<%=btnfooterlist%>";
        var btnfooteropenlist = "<%=btnfooteropenlist%>";
        var requestProcess_logtime = '<%=requestProcess_logtime%>';
        var serverprocesstime = '<%=serverprocesstime%>';
        //Temporary code
        var pageLogTime = '<%=pageLogTime%>';
        var formLogTime = '<%=formLogTime%>';
        //End Temporary code
    </script>
    <%--<script src="../Js/ParamsTstruct.min.js?v=3" type="text/javascript"></script>--%>
</head>
<body onload="ChangeDir('<%=direction%>');" id="<%=transId %>" class="content d-flex flex-column flex-column-fluid fs-6 p-0" dir="<%=direction%>">
    <div id="dvlayout" class="d-flex flex-column mvh-100--- min-vh-100 w-100 position-absolute" runat="server">
        <div class="toolbar m-0 ms-5--- p-0 py-1 flex-shrink-0--- sticky-top bg-light border border-gray-300 rounded-top">
            <div class="container-fluid p-0 d-flex flex-stack flex-wrap flex-sm-nowrap ps-5 p-0">
                <div class="d-flex flex-column align-items-start justify-content-center flex-wrap me-2" id="breadcrumb-panel">
                    <%=tstHeader %>
                </div>
                <div class="d-flex align-items-center flex-nowrap text-nowrap">
                    <%if (AxpTstButtonStyle != "" && AxpTstButtonStyle != "old")
                        { %>
                    <%if (!isTstCustomHtml)
                        { %>
                    <div class="toolbarRightMenu">

                        <%=modernToolbarBtnHtml.ToString() %>

                        <span class="align-items-center">
                            <button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm menu-dropdown" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                <span class="material-icons material-icons-style">apps</span>
                            </button>
                            <%if (AxpTstButtonStyle == "classic")
                                { %>
                            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3" data-kt-menu="true" style="z-index: 106; position: fixed; inset: 0px auto auto 0px; margin: 0px; transform: translate(131px, 52px);" data-popper-placement="bottom-end">
                                <%=toolbarBtnHtml.ToString() %>
                            </div>
                            <% }
                                else
                                {%>
                            <div class="menu menu-sub menu-sub-dropdown menu-column w-100 w-sm-350px " data-kt-menu="true" style="z-index: 105; position: fixed; inset: 0px auto auto 0px; margin: 0px; transform: translate(919px, 90px);" data-popper-placement="bottom-end">
                                <div class="card">
                                    <div class="card-body py-5">
                                        <div class="mh-450px scroll-y me-n5 pe-5">
                                            <div class="row g-2">
                                                <%=toolbarBtnHtml.ToString() %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <% } %>
                        </span>
                    </div>
                    <% }
                        else
                        {%>
                    <%= toolbarBtnHtml.ToString()%>
                    <% } %>
                    <% }
                        else
                        {%>
                    <div id='searchBar' class="toolbarOptionsParent">
                        <div id="icons" class="right">
                            <ul>
                                <%if (!isTstCustomHtml)
                                    { %>
                                <div id="tstToolBarBtn" runat="server">
                                    <div id="tstIcons" runat="server">
                                        <%= toolbarBtnHtml.ToString() + designModeBtnHtml.ToString() %>
                                    </div>
                                    <li id="dvRefreshFromLoad" runat="server" visible="false">
                                        <span>
                                            <button type="submit" title="Refresh FormLoad" onclick="ResetFormLoadCache();">
                                                <i class="fa fa-refresh"></i>
                                            </button>
                                        </span>
                                    </li>
                                </div>
                                <% }
                                    else
                                    {%>
                                <%= toolbarBtnHtml.ToString()%>
                                <% } %>
                            </ul>
                        </div>
                    </div>
                    <% } %>
                </div>

                <div id="designModeToolbar" class="d-none">
                    <div id="dvToolbar" runat="server" class="d-flex align-items-center flex-nowrap text-nowrap mb-1">
                        <a title="TStruct Design Properties" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm toolbarIcons" id="tstDsignProp" onclick="openProprtySht('tstructPS')"><span class="material-icons material-icons-style material-icons-3">settings</span></a>
                        <a title="DC Layouts" runat="server" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm toolbarIcons" id="tstDesigDcLayouts" onclick="javascript:DesignFormDcLayouts();"><span class="material-icons material-icons-style material-icons-3">view_quilt</span></a>
                        <a title="Reset Design" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm toolbarIcons" id="A7" onclick="javascript:ResetButtonClickedWeb();"><span class="material-icons material-icons-style material-icons-3">restart_alt</span></a>
                        <a title="Save" runat="server" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm toolbarIcons" id="saveDesign" onclick="javascript:SaveDesignerJSONWeb();"><span class="material-icons material-icons-style material-icons-3">save</span></a>
                        <a title="Publish" runat="server" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm toolbarIcons" id="PublishDesign" onclick="javascript:PublishDesignerJSONWeb();"><span class="material-icons material-icons-style material-icons-3">publish</span></a>
                        <a title="Save" runat="server" visible="false" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm toolbarIcons" id="PublishSaveDesign" onclick="javascript:SavePublishDesignerJSONWeb();"><span class="material-icons material-icons-style material-icons-3">published_with_changes</span></a>
                        <a title="Run" runat="server" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm toolbarIcons" id="RunMode" onclick="javascript:goToRenderMode();"><span class="material-icons material-icons-style material-icons-3">play_circle</span></a>
                        <a title="HTML" runat="server" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm toolbarIcons" id="tstructHtml" onclick="javascript:customTstHtml();"><span class="material-icons material-icons-style material-icons-3">code</span></a>
                    </div>
                </div>
            </div>
        </div>


        <div id="pagebdy" class="container p-0 overflow-auto---" style="max-width: inherit;">
            <div id="heightframe" runat="server">
                <div id="formContainer" class="col-xl-12">
                    <form id="form1" runat="server" enctype="multipart/form-data">
                        <div>
                            <asp:PlaceHolder runat="server">
                                <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server">
                                <%:Scripts.Render("~/Js/formBasic") %>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server">
                                <%:Scripts.Render("~/Js/formInit") %>
                            </asp:PlaceHolder>
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                                <Services>
                                    <asp:ServiceReference Path="../WebService.asmx" />
                                    <asp:ServiceReference Path="../CustomWebService.asmx" />
                                </Services>
                            </asp:ScriptManager>
                        </div>
                        <asp:HiddenField ID="hdnDataObjId" runat="server" />
                        <asp:HiddenField ID="hdnScriptspath" runat="server" />
                        <asp:HiddenField ID="hdnAxIsPerfCode" runat="server" />
                        <asp:HiddenField ID="SaveID" runat="server" />
                        <asp:HiddenField ID="hdnDraftName" Value="false" runat="server" />
                        <asp:HiddenField ID="hdnFldAlgnProp" Value="" runat="server" />



                        <asp:HiddenField ID="PublishID" runat="server" />
                        <asp:HiddenField ID="isAxpImagePathHidden" runat="server" />
                        <asp:HiddenField ID="IsPublish" runat="server" />
                        <div id="Wrapperpropsheet" class="d-none">
                            <div class='col s3 card hoverable scale-transition scale-out' id='propertySheet'>
                                <div class='hpbHeaderTitle'>
                                    <span class='icon-paint-roller'></span>
                                    <span class='title'>Property Sheet</span>
                                    <button title='Save' type='button' onclick='updateField();' id='updateWidget' class='btn-flat waves-effect btn-floating pull-right '><span class='icon-arrows-check'></span></button>
                                    <div class='clear'></div>
                                </div>
                                <div id='propertySheetDataWrapper'>
                                    <div class='clear'></div>
                                    <div id='propertySearch' class="d-none">
                                        <input placeholder='Search...' type='text' id='propertySearchFld' class='form-control normalFld searchFld'>
                                        <span class='srchIcn icon-magnifier'></span>
                                    </div>
                                    <div class='posr' id='propTableContent'>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="addFieldPsWrapper" class="d-none">
                            <table id="addFieldPS" class='bordered' data-parent="addFieldPsWrapper" data-title="Add Field">
                                <tr>
                                    <td class='subHeading' colspan='2'>General <span data-target='general' class='propShtDataToggleIcon icon-arrows-down'></span></td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Name<sup>*</sup></td>
                                    <td>
                                        <input id='fldName' class='form-control' type='text'></td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Caption<sup>*</sup></td>
                                    <td>
                                        <input id='fldCaption' class='form-control' type='text'></td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Data Type</td>
                                    <td>
                                        <select onchange='moeChanger(this)' class='form-control' id='seldataType'>
                                            <option>Character</option>
                                            <option>Numeric</option>
                                            <option>Date/Time</option>
                                            <option>Image</option>
                                            <option>Text</option>
                                        </select></td>
                                </tr>
                                <tr data-group='general'>
                                    <td>DC</td>
                                    <td>
                                        <select runat='server' class='form-control' id='seldc'></select></td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Mode of entry</td>
                                    <td>
                                        <select class='form-control' id='selmoe'>
                                            <option value='Accept'>Accept</option>
                                            <option value='Select'>Select</option>
                                        </select></td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Width</td>
                                    <td>
                                        <input class='form-control' maxlength='4' id='fldWidth' value='10' type='text'></td>
                                </tr>
                                <tr class='decimalFld notSearchable' data-group='general'>
                                    <td>Decimal</td>
                                    <td>
                                        <input id='fldDecimal' maxlength='2' class='form-control' value='0' type='text'>
                                    </td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Visible</td>
                                    <td>
                                        <select class='form-control' id='selvisible'>
                                            <option value='T'>Yes</option>
                                            <option value='F'>No</option>

                                        </select></td>
                                </tr>
                                <tr>
                                    <td class='subHeading' colspan='2'>Appearance <span data-target='appr' class='propShtDataToggleIcon icon-arrows-down'></span></td>
                                </tr>
                                <tr data-group='appr'>
                                    <td>Normalized</td>
                                    <td>
                                        <select class='form-control' id='selNormalized'>
                                            <option value='F'>No</option>
                                            <option value='T'>Yes</option>
                                        </select>

                                    </td>
                                </tr>
                                <tr data-group='appr'>
                                    <td>Read Only</td>
                                    <td>
                                        <select id='fldReadOnly' class='form-control'>
                                            <option value='F'>No</option>
                                            <option value='T'>Yes</option>
                                        </select></td>
                                </tr>
                                <tr>
                                    <td class='subHeading' colspan='2'>Source <span data-target='src' class='propShtDataToggleIcon icon-arrows-down'></span></td>

                                </tr>
                                <tr data-group='src'>
                                    <td>SQL</td>
                                    <td>
                                        <input onfocus='createSqlWindow()' id='sqlSource' class='form-control' type='text'></td>
                                </tr>
                                <tr>
                                    <td class='subHeading' colspan='2'>Validation <span data-target='valid' class='propShtDataToggleIcon icon-arrows-down'></span></td>
                                </tr>
                                <tr data-group='valid'>
                                    <td>Allow Empty</td>
                                    <td>
                                        <select id='selAlwEmpty' class='form-control'>
                                            <option value='T'>Yes</option>
                                            <option value='F'>No</option>
                                        </select></td>
                                </tr>
                                <tr data-group='valid'>
                                    <td>Allow Duplicate</td>
                                    <td>
                                        <select id='selAlwDup' class='form-control'>
                                            <option value='T'>Yes</option>
                                            <option value='F'>No</option>

                                        </select></td>
                                </tr>
                            </table>
                        </div>

                        <div id="tstructPsWrapper" class="d-none">
                            <table id="tstructPS" class='table table-bordered mt-6 bordered' data-parent="tstructPsWrapper" data-title="TStruct Design Properties" data-save="false">
                                <tr>
                                    <td class='subHeading' colspan='2'>Tstruct <span data-target='general' class='propShtDataToggleIcon icon-arrows-down'></span></td>
                                </tr>
                                <tr class="d-none" data-group='general'>
                                    <td>Compress Mode<sup>*</sup></td>
                                    <td>
                                        <div class="switch" onclick="toggleCompressedMode('s')">
                                            <a href="javascript:void(0)" class="swtchDummyAnchr">
                                                <input class="tgl tgl-ios" name="optDirectDb" id="ckbCompressedMode" type="checkbox">
                                                <label class="tgl-btn togglecustom toggle_btn" for="ckbCompressedMode" id="lblckbCompressedMode"></label>
                                            </a>
                                        </div>
                                    </td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Static Run Mode<sup>*</sup></td>
                                    <td>
                                        <div class="form-check form-switch form-check-custom float-end" onclick="toggleStaticRunMode()">
                                            <input class="form-check-input" type="checkbox" id="ckbStaticRunMode" />
                                            <label class="form-check-label" for="ckbStaticRunMode" id="lblckbStaticRunMode"></label>
                                        </div>
                                    </td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Wizard DC<sup>*</sup></td>
                                    <td>
                                        <div class="form-check form-switch form-check-custom float-end" onclick="toggleWizardDCOption()">
                                            <input class="form-check-input" type="checkbox" id="ckbWizardDC" />
                                            <label class="form-check-label" for="ckbWizardDC" id="lblckbWizardDC"></label>
                                        </div>
                                    </td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Layout<sup>*</sup></td>
                                    <td>
                                        <select id="designLayoutSelector" class="form-control" onchange="changeDesignLayout()" data-no-clear="true">
                                            <option value="default">Default</option>
                                            <option value="tabbed">Tabbed</option>
                                            <!--<option value="tile">Tile</option>-->
                                            <option value="double">Header-Tabbed-Footer</option>
                                            <option value="single">Header-Tabbed</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Font Size<sup>*</sup></td>
                                    <td>
                                        <label for="designForntSizeSelector" style="font-weight: 400;">
                                            Font-Size (px):
                                            <output id="designForntSizeValue" style="display: inline-block; font-weight: 500;">0</output></label>
                                        <input type="range" max="30" min="14" step="2" class="form-range" id="designForntSizeSelector" oninput="designForntSizeValue.value = this.value" />
                                    </td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Control Height <sup>*</sup></td>
                                    <td>
                                        <label for="designControlHeightSelector" style="font-weight: 400;">
                                            Control Height (px):
                                            <output id="designControlHeightValue" style="display: inline-block; font-weight: 500;">0</output></label>
                                        <input type="range" max="32" min="24" step="2" class="form-range" id="designControlHeightSelector" oninput="designControlHeightValue.value = this.value" />
                                    </td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Form Width <sup>*</sup></td>
                                    <td>
                                        <label for="designFormWidthSelector" style="font-weight: 400;">
                                            Form Width (%):
                                            <output id="designFormWidthValue" style="display: inline-block; font-weight: 500;">0</output></label>
                                        <input type="range" max="100" min="50" step="10" class="form-range" id="designFormWidthSelector" oninput="designFormWidthValue.value = this.value" />
                                    </td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Form Alignment: <sup>*</sup></td>
                                    <td>
                                        <select id="designFormAlignmentSelector" class="form-control" data-no-clear="true">
                                            <option value="default">Default</option>
                                            <option value="center">Center</option>
                                            <option value="right">Right</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr data-group='general'>
                                    <td>Field Caption Width <sup>*</sup></td>
                                    <td>
                                        <label for="designFieldCaptionWidthSelector" style="font-weight: 400;">
                                            Caption Width (%):
                                            <output id="designFieldCaptionWidthValue" style="display: inline-block; font-weight: 500;">0</output></label>
                                        <input type="range" max="50" min="10" step="10" class="form-range" id="designFieldCaptionWidthSelector" oninput="designFieldCaptionWidthValue.value = this.value" />
                                    </td>
                                </tr>
                            </table>
                        </div>


                        <%--div to be shown as search dialog--%>
                        <div id="searchFields" class="d-none">
                            <input type="text" id="srchDynamicText" class="AxSearchField" onkeyup="FindTstructString(this.value);" />&nbsp;&nbsp;&nbsp;&nbsp;<label id="searchResCount"></label>
                            <button type="button" id="SearchPrevBtn" onclick="MovePrev();" class="icon-arrows-up"></button>
                            <button type="button" id="SearchNextBtn" onclick="MoveNext();" class="icon-arrows-down"></button>
                            <button type="button" onclick="UnSelectSearchItem('close');" class="icon-arrows-remove"></button>
                        </div>
                        <%--div to be shown as search dialog On Multi Select--%>

                        <div class="accordion rounded w-100 position-absolute z-index-1 d-none h-100">
                            <div id="searchoverrelay" class="accordion-bodyz shadow card mx-1">
                                <div class="card-body">
                                    <div class="d-flex flex-row-auto">
                                        <h4>
                                            <span id="lblheadsrch">Search</span>
                                        </h4>
                                        <div class="d-flex position-absolute top-0 end-0 mt-2 me-4">
                                            <a class="btn btn-sm btn-icon btn-active-primary shadow-sm" href="javascript:Closediv();">
                                                <span class="material-icons material-icons-style">clear</span>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="accordion-collapse collapse show" viewstatemode="Enabled" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="d-flex flex-column">
                                                    <div class="d-flex flex-row-auto gap-5">
                                                        <asp:DropDownList ID="ddlSearch" CssClass="form-select selectPaddingFix" runat="server">
                                                        </asp:DropDownList>

                                                        <% if (langauge == "ARABIC")
                                                            {%>
                                                        <asp:Label ID="lblsrch" runat="server" meta:resourcekey="lblsrch" class="form-label col-form-label pb-1 fw-boldest">
                                                        Search For
                                                        </asp:Label>
                                                        <% }
                                                            else
                                                            { %>
                                                        <asp:Label ID="lblwth" runat="server" CssClass="form-label col-form-label pb-1 fw-boldest" meta:resourcekey="lblwth">
                                                        With
                                                        </asp:Label>
                                                        <%} %>

                                                        <asp:TextBox ID="searstr" CssClass="tem Family form-control" runat="server" value=""></asp:TextBox>
                                                        <asp:HiddenField ID="goval" runat="server" Value=""></asp:HiddenField>

                                                        <input id="Button1" class="btn btn-sm btn-primary" type="button" name="go" value="Go" runat="server" onclick="javascript: valid_submit(); setDocht();" />
                                                        <a id="btnViewColumn" class="btn btn-sm btn-icon btn-active-primary shadow-sm p-7" name="View Columns" title="View Columns" href="javascript:void(0)" onclick="javascript:SearchViewColumns();">
                                                            <span class="material-icons material-icons-style">view_module</span>
                                                        </a>

                                                        <div class="buttgo d-none">
                                                            &nbsp;<asp:Button CssClass="searchHeadbar" ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                                                            <asp:HiddenField ID="hdnHtml" runat="server" Value=""></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnFilename" runat="server" Value=""></asp:HiddenField>
                                                            <asp:Button CssClass="searchHeadbar" ID="btnHtml" runat="server" OnClick="btnHtml_Click" />
                                                            <asp:HiddenField ID="hdnSearchStr" runat="server" Value=""></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnbElapsTimeGo" runat="server" Value=""></asp:HiddenField>
                                                        </div>
                                                    </div>

                                                    <div id="srchcontent" class="py-5 d-none" runat="server">
                                                        <asp:Panel ID="Panel1" runat="server">
                                                            <asp:GridView CellSpacing="-1" ID="grdSearchRes" runat="server" AllowSorting="false"
                                                                AutoGenerateColumns="false" CellPadding="2" CssClass="gridData table table-bordered"
                                                                GridLines="Vertical" OnPageIndexChanging="grdSearchRes_PageIndexChanging"
                                                                OnRowDataBound="grdSearchRes_RowDataBound" PageSize="10" RowStyle-Wrap="false" BorderStyle="None">
                                                            </asp:GridView>
                                                            <div class="d-flex flex-row flex-column-fuildz gap-2 flex-centerz">
                                                                <div class="d-flex">
                                                                    <asp:Label ID="records" runat="server" Text="" CssClass="h5 my-3"></asp:Label>
                                                                </div>
                                                                <div class="d-flex m-auto"></div>
                                                                <div class="d-flex">
                                                                    <asp:Label ID="pgCap" runat="server" CssClass="h5 my-3" Text="Page No."
                                                                        Visible="false"></asp:Label>
                                                                    <asp:DropDownList ID="lvPage" runat="server" AutoPostBack="true"
                                                                        onchange="javascript:Pagination();" Visible="false" CssClass="form-select form-select-sm w-100px">
                                                                    </asp:DropDownList>
                                                                    <asp:Label ID="pages" Text="" runat="server" CssClass="totrec h5 my-3"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-backdrop cursor-pointer fade show z-index-n1" onclick="javascript:Closediv();"></div>
                        </div>
                        <div class="pdfOverlay d-none">
                            <div id="dvPDFDocList">
                                <div id="dvPdfDDl">
                                </div>
                            </div>
                        </div>

                        <div runat="server" class="success d-none" id="dvMessage">
                        </div>
                        <div id="workflowoverlay" runat="server" class="overlay d-none">
                            <div class="d-none">
                                <p class="left">
                                    <asp:TextBox ID="txtCommentWF" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                                </p>
                                <p class="left workflow-buttons">
                                    <input type="button" id="btntabapprove" onclick="CheckFields(this);" value="Approve" class="hotbtn btn" />
                                    <input type="button" id="btntabreject" onclick="CheckFields(this);" value="Reject" class="coldbtn btn" />
                                    <input type="button" id="btntabreview" onclick="CheckFields(this);" value="Approve & Forward" class="hotbtn btn" />
                                    <input type="button" id="btntabreturn" onclick="CheckFields(this);" value="Return" class="coldbtn btn" />
                                    <input type="button" id="btntabcomments" onclick="javascript: ViewComments();" value="View comments" class="coldbtn btn" />
                                </p>
                                <div class="clear">
                                </div>
                                <div class="wfsuccess">
                                    <asp:Label ID="lblStatus" runat="server" class="wkfText" Text="Reviewed by"></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnWfLno" />
                                    <asp:HiddenField runat="server" ID="hdnWfELno" />
                                </div>
                            </div>

                            <%--New Workflow UI--%>
                            <div id="main" class="card d-flex flex-row-auto border border-dashed border-gray-500 px-4 py-1 mt-2 bg-white mb-3 rounded-2 bg-light-primary">
                                <div class="d-flex flex-row-auto gap-3 ">
                                    <div id="stratWrkf" class="ApprovalBar workflowdisplayinline d-none" data-content="Open" data-placement="bottom">
                                    </div>
                                    <!--Your Select -->
                                    <div class="dropbox ">
                                        <div class="wfselectbox d-none">
                                            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3" data-kt-menu="true" data-popper-placement="bottom-end" id="selectbox">
                                            </div>
                                        </div>

                                        <div id="consumergoods2" class="d-none">
                                            <div class="form-group">
                                                <asp:TextBox TextMode="multiline" Rows="5" ID="comment" MaxLength="4000" runat="server" CssClass="form-control" autocomplete="off" onkeyup="LimtCharacters(this,250,'message');" />
                                                <asp:Label runat="server" ID="message" ClientIDMode="Static" AssociatedControlID="comment"></asp:Label>
                                                <p id="lblreject" class="customErrorMessage"></p>
                                            </div>
                                        </div>

                                        <div id="tblWrk" class="form-group d-none">
                                        </div>
                                    </div>

                                    <div class="icobtn workflowdisplayinline ms-auto">
                                        <asp:Label ID="btnViewCommts" runat="server" class="commentbtn btn btn-sm btn-icon btn-active-light-primary shadow-sm me-2" type="button" onclick="getComntWf(true)">
                                            <span class="material-icons material-icons-style">history_toggle_off</span>
                                        </asp:Label>
                                    </div>

                                    <div id="wfpdcommentmodal" class="d-none">
                                        <div class="form-group">
                                            <div id="wfpdcomments" class="form-group" style="max-height: 300px; overflow: auto;">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--End New Workflow UI--%>
                        </div>
                        <div id="file" class="d-none">
                            <table class="tblfile">
                                <tr>
                                    <td class="rowbor">
                                        <asp:Label ID="LabelFs" runat="server" meta:resourcekey="LabelFs" Font-Bold="True" ForeColor="#1e90ff">File Name:</asp:Label>
                                        <input id="filMyFile" accept="text/html" type="file" />
                                        <input type="button" onclick="javascript: CallAfterFileUploadAction();" id="cafterfload"
                                            name="cafterfload" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowbor">
                                        <asp:TextBox ID="TextBox2" runat="server" Width="1" Visible="true" BorderStyle="None"
                                            ForeColor="white" BackColor="white"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <input type="hidden" id="attachfname" />
                                <input type="button" onclick="javascript: callAfterFileAttach();" id="afterfattach"
                                    name="afterfattach" />
                            </div>
                        </div>
                        <asp:HiddenField ID="hdnScriptsUrlpath" runat="server" />
                        <asp:HiddenField ID="hdnShowAutoGenFldValue" runat="server" />
                        <asp:HiddenField ID="HdnAxAdvPickSearch" runat="server" Value="false" />
                        <asp:HiddenField ID="designHidden" runat="server" />
                        <div id="CustomDiv" runat="server" class="d-none">
                        </div>
                        <div id="dvFormatDc" runat="server" class="d-none">
                        </div>
                        <asp:HiddenField ID="hdnCompMode" runat="server" Value="false" />
                        <asp:Label ID="lblTaskBtn" CssClass="d-none" runat="server" meta:resourcekey="lblTaskBtn">Tasks</asp:Label>
                        <asp:HiddenField ID="hdnTabHtml" runat="server" />
                    </form>
                </div>

                <div id="wBdr" class="tstructcontent " runat="server">
                </div>
            </div>

            <input type="hidden" id="hdnActionName" />
            <asp:Label ID="lblNodata" runat="server" meta:resourcekey="lblNodata" Visible="false">No data found.</asp:Label>

            <div class="subres d-none">
                <%=tstJsArrays.ToString() %>
                <%=tstVars.ToString()%>
                <%=tstTabScript.ToString()%>
                <%=tstScript.ToString()%>
                <%=enableBackForwButton%>
                <%=tstDraftsScript.ToString()%>
            </div>
            <!--The below div is used to display the picklist dropdown -->

            <div id="dvFillGrid" style="">
            </div>
        </div>

        <%if (!isMobileView || (isMobileView && wizardClass == ""))
            { %>
        <div class="footer d-flex flex-lg-column content p-1 pb-4 pt-0 flex-shrink-0--- position-sticky bottom-0 mt-auto tstructMainBottomFooter">
            <div class="d-flex flex-column flex-md-row ms-auto">
                <% if (rid != "0" && Request.QueryString["recPos"] != null && Request.QueryString["recPos"] != "" && Request.QueryString["recPos"] != "null" && Request.QueryString["recordid"] != null)
                    { %>
                <div class="text-dark tstructBottomLeftButton d-flex align-items-center">
                    <div class="btn btn-icon btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2 lnkPrev"><a id="lnkPrev" href="javascript:void(0)" onclick="lnkPrevClick();" title="prev transaction"><span class="material-icons material-icons-style">chevron_left</span></a></div>
                    <div class="btn btn-icon btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2 lnkNext"><a id="lnkNext" href="javascript:void(0)" onclick="lnkNextClick();" title="next transaction"><span class="material-icons material-icons-style">chevron_right</span></a></div>
                </div>
                <%} %>
                <%if (!isTstCustomHtml)
                    { %>
                <div class="text-dark BottomToolbarBar">
                    <a href="javascript:void(0)" id="ftbtn_iSave" title="save" onclick='javascript:FormSubmit();' runat="server" class="btn btn-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm d-none"><span class="material-icons">save</span>Submit</a>
                    <a href="javascript:void(0)" id="ftbtn_iDiscard" title="discard" onclick="javascript:discardLodaData();" runat="server" class="btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm d-none"><span class="material-icons">clear</span>Discard</a>
                    <a href="javascript:void(0)" id="ftbtn_iNew" title="new" onclick='javascript:NewTstruct();' runat="server" class="btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm d-none"><span class="material-icons">add_circle_outline</span>New</a>
                    <a href="javascript:void(0)" id="ftbtn_iRemove" title="delete" onclick='javascript:DeleteTstruct();' runat="server" class="btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm d-none"><span class="material-icons">remove</span>Remove</a>
                    <%--<a href="javascript:void(0)" id="ftbtn_iList" title="listview" runat="server" class="btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm d-none"><span class="material-icons">format_list_bulleted</span>List</a>--%>
                    <%= modernFooterActBtnHtml.ToString() %>
                    <div class="clearfix"></div>
                </div>
                <%}
                    else
                    {%>
                <%= modernToolbarBtnHtml.ToString() %>
                <% } %>
            </div>
        </div>
        <%}%>
    </div>
    <div id="reloaddiv" class="d-none">
        <span class="AVerror">Server is unable to process your request. Please retry...</span>
    </div>
    <script src="../Js/ParamsTstruct.min.js?v=6" type="text/javascript"></script>
</body>
</html>
