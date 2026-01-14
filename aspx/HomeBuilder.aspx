<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomeBuilder.aspx.cs" Inherits="aspx_HomeBuilder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home Page Builder</title>
    <meta charset="utf-8"/> 
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>    
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <link href="../ThirdParty/materialize/css/materialize.min.css?v=3" rel="stylesheet" />
    <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" />
    <%--<script>
        if(typeof localStorage != "undefined"){
            var projUrl =  top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            var lsTimeStamp = localStorage["customGlobalStylesExist-" + projUrl]
            if(lsTimeStamp && lsTimeStamp != "false"){
                var appProjName = localStorage["projInfo-" + projUrl] || "";
                var customGS = "<link id=\"customGlobalStyles\" data-proj=\""+ appProjName +"\" href=\"../"+ appProjName +"/customGlobalStyles.css?v="+ lsTimeStamp +"\" rel=\"stylesheet\" />";
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
    <link href="../ThirdParty/scrollbar-plugin-master/jquery.mCustomScrollbar.css" rel="stylesheet" />
    <link href="../ThirdParty/Linearicons/Font/library/linearIcons.css" rel="stylesheet" />
    <link href="../ThirdParty/bgrins-spectrum/spectrum.css" rel="stylesheet" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css" rel="stylesheet" />
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <link href="../Css/thirdparty/font-awesome/4.6.3/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../Css/builder.min.css?v=29" rel="stylesheet" />
    <link href="../Css/thirdparty/intro.js-2.6.0/introjs.min.css" rel="stylesheet" />
    <link href="../Css/thirdparty/intro.js-2.6.0/themes/introjs-modern.css" rel="stylesheet" />
    <!-- <link href="" id="homeBuilderLink" rel="stylesheet" /> -->
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script>
        var userroles = '<%=Session["AxRole"] %>';
        userroles = userroles.split(",");
        var imageBase = '<%=homeBuliderImageaPath%>';
        var myTaskString = '<%=myTaskString%>';
        var totalWidgetCanAdd = '<%=Session["AxMaxNumOfWidgets"]%>' || 20;
        totalWidgetCanAdd = parseInt(totalWidgetCanAdd);
        var buildModeAccess = '<%=homeBulider%>';
        var globalVars = '<%=globalVars%>';
        buildModeAccess = buildModeAccess.split(',');
        var appsessionKey = '<%=appsessionKey%>';
        var dbType = '<%=Session["axdb"]%>';
    </script>
</head>
<body class="menu-<%=menuStyle %>">
    <div class="row" style="margin: 0px;">
        <!--   <div class="HPBmainTitle">
            <h5 class="left">Home Page Designer</h5>
            
        </div> -->
        <div id="HPBmainWrapper">
            <div class="col s2" id="HPBToolBox">
                <div class="hpbWrapper">
                    <div class="hpbHeaderTitle ">
                        <span class="icon-pencil-ruler"></span>
                        <span class="title">Object Browser</span>
                    </div>
                    <div class="toolBxPanelWrapper">
                        <div class="toolBxPanel">
                            <ul class="collection with-header">
                                <li class="collection-header">
                                    <span class="toolBxPanelSrchSpn">
                                        <input placeholder="Tstruct" id="HMTsSrch" class="toolBxPanelSrch">
                                        <span class="searchIcon icon-magnifier"></span>
                                    </span>
                                </li>
                                <ul id="toolBarLsttstruct" class="collectionListUl">
                                    <li class="collection-item noDataLi">
                                        <asp:Label ID="lblloading" runat="server" meta:resourcekey="lblloading">Loading...</asp:Label></li>
                                    <!-- <li data-target="tr1" data-type="tstruct" class="collection-item"></li> -->

                                </ul>
                            </ul>
                        </div>
                        <div class="toolBxPanel">
                            <ul class="collection with-header">
                                <li class="collection-header">
                                    <span class="toolBxPanelSrchSpn">
                                        <input placeholder="Iview" id="HBIVSrch" class="toolBxPanelSrch">
                                        <span class="searchIcon icon-magnifier"></span>
                                    </span>
                                </li>
                                <ul id="toolBarLstiview" class="collectionListUl">
                                    <li class="collection-item noDataLi">
                                        <asp:Label ID="lblnodatali" runat="server" meta:resourcekey="lblnodatali">Loading...</asp:Label></li>
                                    <!-- <li data-target="city" data-type="iview" class="collection-item">Purchase Order</li> -->

                                </ul>
                            </ul>
                        </div>
                        <div class="toolBxPanel">
                            <ul class="collection with-header">
                                <li class="collection-header">
                                    <span class="toolBxPanelSrchSpn">
                                        <input placeholder="Widgets" id="HBWSrch" class="toolBxPanelSrch">
                                        <span class="searchIcon icon-magnifier"></span>
                                    </span>
                                </li>
                                <ul id="toolBarLstwidget" class="collectionListUl">
                                    <li class="collection-item noDataLi">
                                        <asp:Label ID="lbllstwidget" runat="server" meta:resourcekey="lbllstwidget">Loading...</asp:Label></li>
                                </ul>
                            </ul>
                        </div>
                        <div class="toolBxPanel">
                            <ul class="collection with-header">
                                <li class="collection-header">
                                    <span class="toolBxPanelSrchSpn">
                                        <input placeholder="Custom Widget" id="HBCWSrch" class="toolBxPanelSrch">
                                        <span class="searchIcon icon-magnifier"></span>
                                    </span>
                                </li>
                                <ul class="collectionListUl customWidgetsUl">
                                    <li title="Image" data-type="Custom__img" data-target="C__img1" class="collection-item">Image</li>
                                    <li title="HTML" data-type="Custom__html" data-target="C__html1" class="collection-item">HTML</li>
                                    <li title="Static Text" data-type="Custom__txt" data-target="C__txt1" class="collection-item">Static Text</li>
                                    <li title="Sql Query" data-type="Custom__sql" data-target="C__sql1" class="collection-item">SQL Query</li>
                                    <li title="RSS Feed" data-type="Custom__rss" data-target="C__rss1" class="collection-item">RSS Feed</li>
                                    <li title="My Tasks" data-expand="myTsk" data-multiselect="false" data-type="Custom__mytsk" data-target="C__mytsk" class="collection-item">My Tasks</li>
                                </ul>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col s10" id="HPBdesignerCanvas">
                <div class="hpbWrapper">
                    <div class="hpbHeaderTitle ">
                        <span class="icon-window"></span>
                        <!-- <i class="material-icons">border_all</i> -->
                        <span class="title">Designer Canvas</span>
                        <span class="right upDwnBtnWrapper">
                            <button id="toggleMyPagesBtn" onclick="toggleMyPages(this)" title="My home pages" class="waves-effect waves-light btn-flat"><span class="icon-clipboard-user"></span></button>
                            <button title="Create New Tab" id="createNewPageModalBtn" data-target="createNewPageModal" class="waves-effect waves-light btn-flat"><span class="icon-plus"></span></button>
                            <button title="Save" id="saveDesign" onclick="ajaxCallObj.saveJsonData()" class="waves-effect waves-teal btn-flat"><span class="icon-check"></span></button>
                            <button title="Publish" id="publishDesign" onclick="ajaxCallObj.publishJsonData()" class="waves-effect waves-teal btn-flat"><span class="icon-upload2"></span></button>
                        </span>
                        <div class="clear"></div>
                    </div>
                    <ul class="tabs" id="HPBtabsHaeder" style="display: none;">
                        <!-- <li class="tab col s2"><a class="active" href="#test1">Tab 1</a></li> -->
                        <!-- <li class="tab col s2">
                            
                        </li> -->
                    </ul>
                    <!-- Modal Structure -->
                    <div id="createNewPageModal" class="modal modal-fixed-footer">
                        <div class="modal-content">
                            <!-- <h4>Modal Header</h4> -->
                            <div id="hpbPageRespSelector" class="ht100Per">
                                <h5>Create new page</h5>
                                <div class="ht100Per">
                                    <div class="row">
                                        <div class="input-field col s12">
                                            <input id="newTabName" type="text">
                                            <label for="newTabName">Name</label>
                                        </div>
                                        <div class="col s12">
                                            <span>
                                                <label for="isPrivateTab">Is Private : </label>
                                            </span>
                                            <span class="switch">
                                                <label>
                                                    No
                                                    <input id="addPageIsPrivate" type="checkbox">
                                                    <span class="lever"></span>Yes
                                                </label>
                                            </span>
                                        </div>
                                    </div>
                                    <br>
                                    <div class="row con" id="createTabRespSelector">
                                        <div class="col multiSelWrap">
                                            <!-- <label class="widgetRespLabel">Responsibility<sup>*</sup></label> -->
                                            <select name="from[]" id="multiselect" class="form-control1 browser-default rlmultiSlectFld" size="8" multiple="multiple">
                                                <!-- <option disabled value='No Roles'>No Roles</option> -->
                                                <option value='No'>Roles1</option>
                                                <option value='Noles'>Roles2</option>
                                                <option value='Nles'>Roles3</option>
                                                <option value='Nes'>Roles4</option>
                                                <option value='Ns'>Roles5</option>
                                            </select>
                                        </div>
                                        <div class="col multiSelWrapOpt" style="padding-left: 20px;">
                                            <button title="Select All" type="button" id="multiselect_rightAll" class="waves-effect btn-flat smallBtn">
                                                <i class="icon-chevron-right frstIcn"></i>
                                                <i class="icon-chevron-right scndIcn"></i>
                                            </button>
                                            <button title="Select" type="button" id="multiselect_rightSelected" class="waves-effect btn-flat smallBtn">
                                                <i class="icon-chevron-right"></i>
                                            </button>
                                            <button title="Remove" type="button" id="multiselect_leftSelected" class="waves-effect btn-flat smallBtn">
                                                <i class="icon-chevron-left"></i>
                                            </button>
                                            <button title="Remove All" type="button" id="multiselect_leftAll" class="waves-effect btn-flat smallBtn">
                                                <i class="icon-chevron-left frstIcn"></i>
                                                <i class="icon-chevron-left scndIcn"></i>
                                            </button>
                                        </div>
                                        <div class="col multiSelWrap">
                                            <select name="to[]" id="multiselect_to" class="form-control1 browser-default rlmultiSlectFld" size="8" multiple="multiple"></select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="display: none;" id="hpbPageTemplateSelector">
                                <h5>Please select a responsive template</h5>
                                <div class="templateWrapper">
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button title="Ok" onclick="addNewHomePage()" type="button" class="waves-effect btn themeButton cutsomColorbtn">Ok</button>
                            <button type="button" title="Cancel" style="margin-right: 10px;" class="cancelButton waves-effect btn modal-action modal-close">Cancel</button>
                        </div>
                    </div>
                    <!-- Modal Structure - END -->
                    <div id="hpbDsgnrcnvsWrapper">
                        <div id="sortable" class="row mainWidgetAddedWrapper">
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col s3 card hoverable scale-transition hide scale-out" id="propertySheet">
                <div class="hpbHeaderTitle themeDarkenBackground">
                    <span class="icon-paint-roller"></span>
                    <span class="title">Property Sheet</span>
                    <button title="Close" type="button" id="propertySrchCls" onclick="closeProprtySht();" class="btn-flat waves-effect btn-floating right"><i class="icon-cross2"></i></button>
                </div>
                <div id="propertySheetDataWrapper">
                    <div id="propertySearch">
                        <input placeholder="Search..." type="text" id="propertySearchFld" class="normalFld searchFld">
                        <span class="srchIcn icon-magnifier"></span>
                    </div>
                    <div class="posr" id="propTableContent">
                        <table class="bordered">
                            <tr>
                                <td class="subHeading themeTextColor" colspan="2"><span class="td-general">General</span> <span data-target="general" class="propShtDataToggleIcon icon-chevron-up"></span></td>
                            </tr>
                            <tr data-group="general">
                                <td id="ppsLcmp">Component</td>
                                <td id="prpShtComponent">Tstruct</td>
                            </tr>
                            <tr class="prpShtTitleRelated" data-group="general">
                                <td id="ppsLttl">Title</td>
                                <td>
                                    <input type="text" maxlength="50" onkeyup="homeJsonObj.updateValueOnKeyUp(this,'tl')" value="" class="normalInpFld themeMaterialInp" id="prpShtTitle">
                                </td>
                            </tr>
                            <tr id="prpShtHeadingSrc">
                                <td class="subHeading themeTextColor" colspan="2"><span class="td-src">Source</span> <span data-target="src" class="propShtDataToggleIcon icon-chevron-up"></span></td>
                            </tr>
                            <tr id="prpShtTargetTypeWrapper" data-group="src">
                                <td>Target Type</td>
                                <td>
                                    <select onchange="prpShtChangeTrgtType(this)" name="" id="prpShtTargetType">
                                        <option value="none">None</option>
                                        <option value="url">URL</option>
                                        <option value="tstruct">Tstruct</option>
                                        <option value="iview">Iview</option>
                                    </select>
                                </td>
                            </tr>
                            <tr id="prpShtRssFeedTime" class="commonCloseOnOpen" data-group="src">
                                <td>Refresh Time</td>
                                <td>
                                    <select onchange='homeJsonObj.updateDataInJson($(this).parents("#propertySheet").data("target"),"rsst",$(this).val())' name="">
                                        <option value="">None</option>
                                        <option value="2">2 Mins</option>
                                        <option value="5">5 Mins</option>
                                        <option value="8">8 Mins</option>
                                        <option value="10">10 Mins</option>
                                    </select>
                                </td>
                            </tr>
                            <tr id="prpShtCustomTargetWrapper" data-group="src">
                                <td>Target</td>
                                <td>
                                    <select name="" id="prpShtCustomTarget"></select>
                                </td>
                            </tr>
                            <tr id="prpShtTargetWrapper" data-group="src">
                                <td>Target</td>
                                <td>
                                    <input type="text" value="" class="normalInpFld themeMaterialInp" id="prpShtTarget">
                                </td>
                            </tr>
                            <tr id="prpShtTxtBxWrapper" data-group="src">
                                <td>Description</td>
                                <td>
                                    <input type="text" value="" data-key="dc" onfocus="createCustomTxtBx(this)" class="normalInpFld themeMaterialInp" id="prpShtTxtBx">
                                </td>
                            </tr>
                            <!--  <tr id="prpShtHtmlWrapper">
                                <td>HTML</td>
                                <td>
                                    <input type="text" value="" class="normalInpFld themeMaterialInp" id="prpShtHtml">
                                </td>
                            </tr> -->
                            <tr id="prpShtImgWrapper" data-group="src">
                                <td>Image</td>
                                <td>
                                    <div class="file-field input-field">
                                        <div class="btn fileupbtn themeButton">
                                            <span>...</span>
                                            <form id="frmUploader" enctype="multipart/form-data" action="'<%=nodeApiPath%>';" method="post">
                                                <input type="hidden" id="imageSessionId" name="session_id" value="123">
                                                <input type="hidden" id="imageUtl" name="utl" value="123">
                                                <input type="hidden" name="imagename" value="mani.jpg">
                                                <input type="hidden" name="username" id="imgUsername" />
                                                <input type="hidden" name="authorization" id="imgauthorization" />
                                                <input type="hidden" name="appSKey" id="imgappSKey" />
                                                <input type="file" name="imgUploader" id="imgUploader" />


                                            </form>
                                        </div>
                                        <div class="file-path-wrapper">
                                            <input class="file-path validate" type="text">
                                            <div class="progress" id="prpShtImgUpload">
                                                <div class="determinate" style="width: 0%"></div>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr data-group="src" class="commonCloseOnOpen" id="prpShtWidgetRefresherTr">
                                <td>Cache</td>
                                <td>
                                    <div class="switch">
                                        <label>
                                            No
                                            <input id="phpShtWidgetRefresher" onchange="prpShtToggleSwtch('cacheWidget',this)" type="checkbox">
                                            <span class="lever"></span>Yes
                                        </label>
                                    </div>
                                </td>
                            </tr>
                            <tr data-group="src" class="commonCloseOnOpen" id="prpShtWidgetRefresheTimerTr">
                                <td>Auto Refresh (Mins)</td>
                                <td>
                                    <input onkeyup="homeJsonObj.updateValueOnKeyUp(this,'cei')" type="number" max="999" value="" class="normalInpFld themeMaterialInp" id="prpShtWidgetRefresheTimer" />
                                </td>
                            </tr>
                            <tr>
                                <td class="subHeading themeTextColor" colspan="2"><span class="td-appr">Appearance</span> <span data-target="appr" class="propShtDataToggleIcon icon-chevron-up"></span></td>
                            </tr>
                            <!-- <tr>
                                <td>Font Size:</td>
                                <td>
                                    <input type="text" value="" class="normalInpFld themeMaterialInp" id="prpShtFont">
                                </td>
                            </tr> -->
                            <tr data-group="appr" id="prpShtTtlRgnTr">
                                <td>Title Region</td>
                                <td>
                                    <div class="switch">
                                        <label>
                                            Off
                                            <input id="prpShtTargetRgn" onchange="prpShtToggleSwtch('ttlRgn',this)" type="checkbox">
                                            <span class="lever"></span>On
                                        </label>
                                    </div>
                                </td>
                            </tr>
                            <tr data-group="appr" class="commonCloseOnOpen" id="prpShtImgRespTr">
                                <td>Image Responsive</td>
                                <td>
                                    <div class="switch">
                                        <label>
                                            Off
                                            <input id="prpShtRespimg" onchange="prpShtToggleSwtch('respImg',this)" type="checkbox">
                                            <span class="lever"></span>On
                                        </label>
                                    </div>
                                </td>
                            </tr>
                            <!-- <tr data-group="appr">
                                <td>Strech</td>
                                <td>
                                    <div class="switch">
                                        <label>
                                            Off
                                            <input onchange="prpShtToggleSwtch('strch',this)" type="checkbox">
                                            <span class="lever"></span> On
                                        </label>
                                    </div>
                                </td>
                            </tr> -->
                            <tr id="prpShtPageTemplateTr" class="commonCloseOnOpen" data-group="appr">
                                <td>Template</td>
                                <td>
                                    <!-- Dropdown Trigger -->
                                    <a class='dropdown-button dropDownBtnAsSelect' href='javascript:void(0)' data-activates='prpShtTmpFropDwn'>Drop Me! <span class="caret">?</span></a>
                                    <!-- Dropdown Structure -->
                                    <ul id='prpShtTmpFropDwn' class='dropdown-content'>
                                    </ul>
                                </td>
                            </tr>
                            <tr id="prpShtTitlePickerTr" class="prpShtTitleRelated" data-group="appr">
                                <td>Icon</td>
                                <td>
                                    <input type="text" value="" class="normalInpFld themeMaterialInp iconInpFld" id="prpShtIcon">
                                    <button type="button" onclick="toggleIcons()" class="iconSelector themeTextColor btn-flat waves-effect btn-floating right"><span id="iconSelectorSpn" class="icon-home"></span></button>
                                    <div id="iconWrapper">
                                    </div>
                                </td>
                            </tr>
                            <tr class="prpShtTitleRelated" id="prpShtIcnClrTr" data-group="appr">
                                <td>Icon Color</td>
                                <td>
                                    <div class="colorPickerWrapper">
                                        <input type="text" value="" class="normalInpFld themeMaterialInp colorPickerInp" id="prpShtIcnColor">
                                        <input type="text" value="" data-type="icc" class="normalInpFld themeMaterialInp colorPicker" id="">
                                    </div>
                                </td>
                            </tr>
                            <tr class="prpShtTitleRelated" id="prpShtTtlClrTr" data-group="appr">
                                <td>Title Color</td>
                                <td>
                                    <div class="colorPickerWrapper">
                                        <input type="text" value="" class="normalInpFld themeMaterialInp colorPickerInp" id="prpShtTtlColor">
                                        <input type="text" value="" data-type="tc" class="normalInpFld themeMaterialInp colorPicker" id="">
                                    </div>
                                </td>
                            </tr>
                            <tr class="prpShtTitleRelated" id="prpShtTtlBgClrTr" data-group="appr">
                                <td>Title Bg Color</td>
                                <td>
                                    <div class="colorPickerWrapper">
                                        <input type="text" value="" class="normalInpFld themeMaterialInp colorPickerInp" id="prpShtTtlBgColor">
                                        <input type="text" value="" data-type="tbc" class="normalInpFld themeMaterialInp colorPicker" id="">
                                    </div>
                                </td>
                            </tr>
                            <tr class="prpShtfloatingBtnGrp" id="prpShtFmBgClrTr" data-group="appr">
                                <td>Floating Menu Bg Color</td>
                                <td>
                                    <div class="colorPickerWrapper">
                                        <input type="text" value="" class="normalInpFld themeMaterialInp colorPickerInp" id="prpShtBtnBgColor">
                                        <input type="text" value="" data-type="fbc" class="normalInpFld themeMaterialInp colorPicker" id="">
                                    </div>
                                </td>
                            </tr>
                            <tr class="prpShtfloatingBtnGrp" id="prpShtFmTcTr" data-group="appr">
                                <td>Floating Menu Text Color</td>
                                <td>
                                    <div class="colorPickerWrapper">
                                        <input type="text" value="" class="normalInpFld themeMaterialInp colorPickerInp" id="prpShtBtnColor">
                                        <input type="text" value="" data-type="fc" class="normalInpFld themeMaterialInp colorPicker" id="">
                                    </div>
                                </td>
                            </tr>
                            <tr id="bdTxtClrTr" data-group="appr">
                                <td>Body text Color</td>
                                <td>
                                    <div class="colorPickerWrapper">
                                        <input type="text" value="" class="normalInpFld themeMaterialInp colorPickerInp" id="prpShtBodyTxtColor">
                                        <input type="text" value="" data-type="bc" class="normalInpFld themeMaterialInp colorPicker" id="">
                                    </div>
                                </td>
                            </tr>
                            <tr id="bdBgClrTr" data-group="appr">
                                <td>Body Bg Color</td>
                                <td>
                                    <div class="colorPickerWrapper">
                                        <input type="text" value="" class="normalInpFld themeMaterialInp colorPickerInp" id="prpShtBodyColor">
                                        <input type="text" value="" data-type="bbc" class="normalInpFld themeMaterialInp colorPicker" id="">
                                    </div>
                                </td>
                            </tr>
                            <tr data-group="appr" id="gradientPickerWrapper" class="commonCloseOnOpen">
                                <td>Gradient Color</td>
                                <td>
                                    <div id="gradientPicker" class="colorMe blue center-align">
                                        <a class="colorMeA white-text" data-color='blue' onclick="gradientClick('colorClick')" href="javascript:void(0)">blue  
                                        </a>
                                    </div>
                                    <div class="gradientPalletWrapper" style="display: none;">
                                        <div class="pallet">
                                            <div class="colorMe blue palletIns">
                                                <a title="Blue" onclick="gradientClick('colorPick','blue')" class="colorMeA" href="javascript:void(0)"></a>
                                            </div>
                                        </div>
                                        <div class="pallet">
                                            <div class="colorMe red palletIns">
                                                <a title="Red" onclick="gradientClick('colorPick','red')" class="colorMeA" href="javascript:void(0)"></a>
                                            </div>
                                        </div>
                                        <div class="pallet">
                                            <div class="colorMe yellow palletIns">
                                                <a title="Yellow" onclick="gradientClick('colorPick','yellow')" class="colorMeA" href="javascript:void(0)"></a>
                                            </div>
                                        </div>
                                        <div class="pallet">
                                            <div class="colorMe green palletIns">
                                                <a title="Green" onclick="gradientClick('colorPick','green')" class="colorMeA" href="javascript:void(0)"></a>
                                            </div>
                                        </div>
                                        <div class="pallet">
                                            <div class="colorMe purple palletIns">
                                                <a title="Purple" onclick="gradientClick('colorPick','purple')" class="colorMeA" href="javascript:void(0)"></a>
                                            </div>
                                        </div>
                                        <div class="pallet">
                                            <div class="colorMe multiBlue palletIns">
                                                <a title="Multiple Blue" onclick="gradientClick('colorPick','multiBlue')" class="colorMeA" href="javascript:void(0)"></a>
                                            </div>
                                        </div>
                                        <div class="pallet">
                                            <div class="colorMe multiLime palletIns">
                                                <a title="Multiple Lime" onclick="gradientClick('colorPick','multiLime')" class="colorMeA" href="javascript:void(0)"></a>
                                            </div>
                                        </div>


                                    </div>
                                </td>
                            </tr>
                            <tr id="prpShtAuthrzatn" class="commonCloseOnOpen">
                                <td class="subHeading themeTextColor" colspan="2"><span class="td-auth">Authorization</span> <span data-target="auth" class="propShtDataToggleIcon icon-chevron-up"></span></td>
                            </tr>
                            <tr data-group="auth" class="commonCloseOnOpen">
                                <td>Is Private</td>
                                <td>
                                    <div class="switch">
                                        <label>
                                            No
                                            <input id="prpShtIsPrivate" onchange="prpShtToggleSwtch('isPrivate',this)" type="checkbox">
                                            <span class="lever"></span>Yes
                                        </label>
                                    </div>
                                </td>
                            </tr>
                            <tr data-group="auth" id="prpShtRepTr" class="commonCloseOnOpen">
                                <td>Responsibility</td>
                                <td id="prpShtRolesWrappperTd">
                                    <div class="input-field col s12">
                                        <select multiple onchange='homeJsonObj.updateDataInJson($(this).parents("#propertySheet").data("target"),"rl",$(this).val())' id="prpShtRoles">
                                            <!-- <option selected value="0">All</option> -->
                                            <option disabled value="0">Please wait...</option>
                                        </select>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="subHeading themeTextColor" colspan="2"><span class="td-log">Activity Log</span> <span data-target="log" class="propShtDataToggleIcon icon-chevron-up"></span></td>
                            </tr>
                            <tr data-group="log" id="prpShtMdByTr">
                                <td>Modified By</td>
                                <td id="prpShtModifiedBy"></td>
                            </tr>
                            <tr data-group="log" id="prpShtMdyOnTr">
                                <td>Modified On</td>
                                <td id="prpShtModifiedOn"></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="customTxtAreaWrapperBg" style="display: none;">
        <div id="customTxtAreaWrapper">
            <!-- <button id="customTxtAreaWrapperCls" type="button" class="btn-flat waves-effect btn-floating right"><i class="icon-cross2"></i></button> -->
            <textarea tabindex="-1" name="" id="" cols="30" rows="10"></textarea>
            <div id="customTxtAreaFooter" style="display: none;" class="right-align">
                <button title="save" class="cutsomColorbtn themeButton save waves-effect waves-light btn"><span class="icon-check"></span></button>
                <button title="cancel" class="cancel waves-effect waves-light btn"><span class="icon-cross2"></span></button>
            </div>
        </div>
    </div>




    <script type="text/javascript" src="../Js/thirdparty/jquery/3.1.1/jquery.min.js"></script>
    <script src="../Js/noConflict.min.js?v=1"></script>
    <script type="text/javascript" src="../Js/common.min.js?v=158"></script>
    <script src="../Js/thirdparty/jquery-ui/jquery-ui.min.js"></script>
    <script src="../ThirdParty/jquery-mousewheel/jquery-mousewheel.min.js"></script>
    <script src="../ThirdParty/scrollbar-plugin-master/jquery.mCustomScrollbar.js"></script>
    <script src="../ThirdParty/materialize/js/materialize.min.js?v=11"></script>
    <%--//need to minified as plugin changed--%>
    <script src="../Js/jquery.browser.min.js" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>
    <script src="../Js/alerts.min.js?v=32"></script>
    <script src="../ThirdParty/Highcharts/highcharts.js"></script>
    <script src="../ThirdParty/Highcharts/highcharts-3d.js"></script>
    <script src="../ThirdParty/Highcharts/highcharts-more.js"></script>
    <script src="../ThirdParty/Highcharts/highcharts-exporting.js"></script>
    <script src="../Js/high-charts-functions.min.js?v=23"></script>
    <script src="../Js/multiselect.min.js"></script>
    <script src="../ThirdParty/ajaxForm.js"></script>
    <script src="../ThirdParty/bgrins-spectrum/spectrum.js"></script>
    <script src="../ThirdParty/DataTables-1.10.13/extensions/Extras/moment.min.js"></script>
    <script src="../Js/templateParser.min.js?v=12"></script>
    <script src="../Js/commonBuilder.min.js?v=48"></script>
    <script src="../Js/commonHome.min.js?v=25"></script>
    <script src="../Js/builder.min.js?v=64"></script>
    <script src="../Js/thirdparty/intro.js-2.6.0/intro.min.js"></script>
    <script src="../Js/homebuilder_intro.min.js?v=2"></script>
</body>
</html>
