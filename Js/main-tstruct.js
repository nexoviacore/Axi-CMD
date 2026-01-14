// global variables
var isMozilla;
var objDiv = null;
var originalDivHTML = "";
var DivID = "";
var over = false;
var addHeader = "";
var fillGridDatatbl = "";
var shouldFaceUser = false;
var flipCamera = false;
var gridDummyRows = false;
var gridDummyRowVal = new Array;
var AxpGridForm = "popup";
var fldmultiSelectdep = false;
var depNotBoundFld = "";
var axplatlongFldName = "";
var axpfilepathold = "";
var axpScanBarFldFocus = "";
var axpScriptaddrowres = "";
var axpScriptIsAddrow = true;
var dcLayoutType = "default";
var isDcLayoutSelected = false;
var formLabelJSON = [];
var buttonFieldFontJSON = [];
var regVarFldExp = new Array();
var select2EventType = "";
var select2IsOpened = false;
var select2IsFocused = false;
var setDesProp = {};
var forceRowedit = false;
var isGridFileUploadOnLoad = false;
var wizardHidenDcNos = [];
var gridRowEditOnLoad = false;
var AxRulesDefValidation = "false";
var AxRulesDefFilter = "false";
var AxRuesDefFormcontrol = "false";
var AxRulesDefComputescript = "false";
var AxRulesDefAllowdup = "false";
var AxRulesDefAllowEmpty = "false";
var AxRulesDefIsAppli = "false";

var AxRulesDefScriptOnLoad = "false";
var AxRulesDefOnSubmit = "false";
var AxRuesDefScriptFormcontrol = "false";
var AxRuesDefScriptFCP = "false";

var AxpDcstateVal = "";
var AxAllowEmptyFlds = new Array;
var isScriptFormLoad = "false";
var callGetTab = false;
var AxFldExpOnAddRow = "";
var AxpFillDepFieldsClient = "";
var AxFldBlured = "";
var AxFldBlurFromSelect = "";
var AxDoPegApprovalSave = "";
var AxPegFinalApproval = "false";
var AxAmendmentReadOnly = "false";
var AxPegAmdreadOnly = "false";
var isAddRowWsCalled = "false";
var isExcelImpDelWS = "false";
var isPegApproveConfirm = "";
var AxPegLevelNo = "";
var AxSetFldCaption = new Array();
var AxDiscardNxtPrevFc = new Array();
var MultiFillgridSldIndex = new Array();
var addFieldReloaduri = "";
var fldsHideOnPage = "false";
var iststddlcheck = false;
var interExterResourcesData = "";
var dynamicMobileResolution = function () {
    if ($(".grid-stack").hasClass("dynamicRunMode")) {
        if ((window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth) <=
            renderGridOptions.minWidth) {
            $(".grid-stack").addClass("grid-stack-one-column-mode");
        } else if ($(".tileLayoutDiv").length == 0) {
            $(".grid-stack").removeClass("grid-stack-one-column-mode");
        }
    }
}

$j(document).ready(function () {
    if (typeof isServerDummyPost != "undefined" && isServerDummyPost == "true") {
        isServerDummyPost = 'false';
        return;
    }
    if (typeof isTstPostBackVal == "undefined" || isTstPostBackVal == "")
        WireElapsTime(serverprocesstime, requestProcess_logtime);
    AxpGridForm = AxpGridFormCols != "" ? AxpGridFormCols.split("♠")[0] : "popup";
    if (theMode == "design") {
        $(callParentNew("splitIcon", "id")).css({
            "display": ""
        }).removeClass("hide");
        $(callParentNew("spiltHeading", "class")).css({
            "display": ""
        }).removeClass("hide");
    }
    fldsHideOnPage = "false";
    AxIsTstructLocked = false
    tstReadOnly = false;
    isDepForceCallOnExp = "false";
    typeof commonReadyTasks == 'function' ? commonReadyTasks() : "";
    if (typeof isTstPostBackVal != "undefined" && isTstPostBackVal != "") {
        var resval = isTstPostBackVal.split("*$*");
        var wBdrHtml = resval[0];
        $("#wBdr").html("");
        $("#wBdr").prepend(wBdrHtml);
        var toolBarHtml = resval[1];
        //if ((typeof isWizardTstruct == "undefined" || isWizardTstruct == false) && (typeof IsObjCustomHtml == "undefined" || IsObjCustomHtml == "False")) {
        if ((typeof isWizardTstruct == "undefined" || (typeof isWizardTstruct != "undefined" && isWizardTstruct == false)) && (typeof IsObjCustomHtml == "undefined" || (typeof IsObjCustomHtml != "undefined" && IsObjCustomHtml == "False"))) {
            $("#tstIcons").html("");
            $("#tstIcons").prepend(toolBarHtml);
            $("#iconsNewOptionIcon").after("");
            $("#iconsNewOptionIcon").after(toolBarHtml);
            $('.toolbarRightMenu .menu:not(.dvbtnAppsDraft)').html("")
            $('.toolbarRightMenu .menu:not(.dvbtnAppsDraft)').append(toolBarHtml);
            $('.toolbarRightMenu a.btn,.toolbarRightMenu span button').removeClass('disabled');
            $('.toolbarRightMenu a.btn,.toolbarRightMenu span button').removeClass('d-none');
            $('.toolbarRightMenu a.btn,.toolbarRightMenu span button').removeAttr("style").css({ "pointer-events": "auto" });
            $('.toolbarRightMenu a.btn,.toolbarRightMenu span button').removeAttr('disabled');
            var modenFooter = resval[2];
            if (modenFooter != '') {
                //$("#tstModernOpenIcons").html("");
                //$("#tstModernOpenIcons").prepend(modenFooter.split('♦')[0]);
                btnfooteropenlist = modenFooter.split('♦')[1];
                if (btnfooteropenlist == "") {
                    AdditionalRunTimeMsg("Tstruct loaded from LS/cache and save & new button empty. modenFooter:" + modenFooter.toString());
                }
                if (btnfooteropenlist.indexOf('<li>') > -1)
                    btnfooteropenlist = '';
            } else {
                AdditionalRunTimeMsg("Tstruct loaded from LS/cache and save & new button empty. modenFooter else:" + modenFooter.toString());
            }
        } else if (typeof isWizardTstruct != "undefined" && isWizardTstruct != false) {
            AdditionalRunTimeMsg("Tstruct loaded from LS/cache and save & new button empty. isWizardTstruct:" + isWizardTstruct);
        } else if (typeof IsObjCustomHtml != "undefined" && IsObjCustomHtml != "False") {
            AdditionalRunTimeMsg("Tstruct loaded from LS/cache and save & new button empty. IsObjCustomHtml:" + IsObjCustomHtml);
        }
    }
    if (typeof tstConfigurations != "undefined" && tstConfigurations.config !== undefined && tstConfigurations.config.length > 0) {

        //const buttonStyleVal = tstConfigurations.config.find(item => item.props?.trim().toLowerCase() === "tstruct button style")?.propsval;

        let _finalJson = [];
        try {
            tstConfigurations.config.forEach(function (val) {
                var ret = {};
                $.map(val, function (value, key) {
                    ret[key.toLowerCase()] = value;
                });
                _finalJson.push(ret);
            });
        } catch (ex) { }
        const buttonStyleVal = _finalJson.find(item => item.props?.trim().toLowerCase() === "tstruct button style")?.propsval;
        if (typeof buttonStyleVal != "undefined" && buttonStyleVal != AxpTstButtonStyle)
            AxpTstButtonStyle = buttonStyleVal;
        if (AxpTstButtonStyle == "modern") {
            $(".menu.menu-sub.menu-sub-dropdown:not(.dvbtnAppsDraft)").css("flex-flow", "wrap");
        }
    }
    if (typeof AxpTstButtonStyle != "undefined" && AxpTstButtonStyle == "old") {
        $("#breadcrumb-panel").removeClass("justify-content-center");
        $("#breadcrumb-panel").parent().removeClass("flex-stack").addClass("flex-column");
        $("#breadcrumb-panel").next().removeClass("align-items-center");
        $("#searchBar").addClass("vw-100 overflow-scroll");
        $("#searchBar").find("ul").addClass("px-0 my-0");

        try {
            KTMenu?.init();

            var menuEl = document.querySelector("#tstIcons .menu");
            if (KTMenu.getInstance(menuEl)) {
                menuEl.classList.remove('initialized');
            }
           
            menuEl.on("kt.menu.dropdown.show", (item) => {
                $(item).find(".menu-link:not(.menu-sub .menu-link)").removeClass("text-gray-600").addClass("text-white");
            });
            menuEl.on("kt.menu.dropdown.hide", (item) => {
                $(item).find(".menu-link:not(.menu-sub .menu-link)").removeClass("text-white").addClass("text-gray-600");
            });
        } catch (error) { }
    }
    $("#btnSaveTst").prop({
        'value': callParentNew('lcm')[392],
        'title': eval(callParent('lcm[392]'))
    });
    $("#New").prop({
        'value': callParentNew('lcm')[393],
        'title': eval(callParent('lcm[393]'))
    });
    SetFormDirty(false);
    AxpIviewDisableSplit = AxpIviewDisableSplit.toLowerCase();
    compressedMode = appGlobalVarsObject._CONSTANTS.compressedMode;
    if (!isMobile && isTstPop.toLowerCase() == "false") {
        if (AxpIviewDisableSplit != undefined && AxpIviewDisableSplit == "false") {
            $(callParentNew("split-btn", "class")).css({
                "display": ""
            }).removeClass("hide");
            $(callParentNew("split-btn-vertical", "class")).css({
                "display": ""
            }).removeClass("hide");
        } else if (AxpIviewDisableSplit === "true") {
            $(callParentNew("splitIcon", "id")).addClass("hide");
            $(callParentNew("spiltHeading", "class")).addClass("hide");
            // $(callParentNew("split-btn-vertical", "class")).addClass("hide");
        } else if (AxpIviewDisableSplit === "") {
            $(callParentNew("splitIcon", "id")).css({
                "display": ""
            }).removeClass("hide");
            $(callParentNew("spiltHeading", "class")).css({
                "display": ""
            }).removeClass("hide");
        }
    } else {
        $(callParentNew("split-btn", "class")).addClass("hide");
        $(callParentNew("split-btn-vertical", "class")).addClass("hide");
    }

    if (AxpIsAutoSplit.toLowerCase() == "true" && callParentNew("isTstructSplited") === false) {
        callParentNew(`assocateIframe(true)`, 'function');
    }
    var glangType = eval(callParent('gllangType'));
    if (glangType === "ar") {
        //        $('head').append('<link rel="stylesheet" href="../ThirdParty/bootstrap_rtl.min.css" type="text/css" />');
        $('[data-toggle=popover]').each(function () {
            $(this).data('placement', 'left');
        });
    }


    if (typeof AxRulesEngine != "undefined" && AxRulesEngine != "") {
        let axruleList = AxRulesEngine.split('~');
        //AxRulesDefValidation = axruleList[0];
        //AxRulesDefFilter = axruleList[1];
        //AxRuesDefFormcontrol = axruleList[2];
        //AxRulesDefComputescript = axruleList[3];
        //AxRulesDefAllowdup = axruleList[4];
        //AxRulesDefAllowEmpty = axruleList[5];
        //AxRulesDefIsAppli = axruleList[6];

        AxRulesDefScriptOnLoad = axruleList[0];
        AxRulesDefOnSubmit = axruleList[1];
        AxRuesDefScriptFormcontrol = axruleList[2];
        AxRuesDefScriptFCP = axruleList[3];
    }


    $ == undefined ? $ = $j : "";
    try {
        if (DCIsPopGrid.indexOf("True") > -1) {
            window.location.href = "err.aspx?errmsg=Axpertweb 11.0 popgrid (subgrid) is not supporting, please change the definition and try again..";
            return;
        }
        AxBeforeTstLoad();
    } catch (ex) {
        $j('div#wBdr').show();
        ShowDimmer(false);
    }
    var t0 = performance.now();
    AxStartTime = new Date();
    $(document).on("click", "#icons,#btnSaveTst,.BottomToolbarBar a", function (e) {
        if (actionCallFlag != actionCallbackFlag) {
            e.stopPropagation;
            e.preventDefault();
            $("#icons,#btnSaveTst,.BottomToolbarBar a").css({
                "pointer-events": "none"
            });
            return;
        }
    })
    try {       
        hideDiscardButton();
        switchTstructMode();
        GetFormDetails();
        loadingNew();
        LoadGridScript();
        AxAutoGenDeps = GetDependentArray();
        AddVisTabDcsInArray();
        OnTstructLoad();
        SetFieldSetCarryValue();
        AxpFileFields();
        if (typeof isTstPostBackVal != "undefined" && isTstPostBackVal != "") {
            isTstPostBackVal = "";
            recordid = $("#axp_recid1000F1").val();
            $j("#recordid000F0").val($("#axp_recid1000F1").val());
        }
        //if (typeof isWizardTstruct != "undefined" && isWizardTstruct) {
        // $("#wizardFormContainer").append($("#formContainer").detach());
        // $("#wizardFormContainer").append($("#attachment-overlay").detach());
        //}
        showSaveNewbtns();
        axDeveloperStudioTstToolbar();
    } catch (ex) { 
        $j('div#wBdr').show();
        ShowDimmer(false);
    }
    if (gl_language == "ARABIC")
        $j("#pagebdy").css("direction", "rtl");
    SetGridBtnAccess();
    if (mode != "design") {
        try {
            LoadEvents();
        } catch (ex) { }
        //readOnlyFldddDiv();
    }

    $(window).resize(dynamicMobileResolution);

    setTimeout(function () {
        swicthCompressMode();
    }, 0);

    CheckCustFooter();
    CheckCustomTStSave();
    SetAutoCompAccess("");
    if (typeof focusAfterSaveOnLoad != "undefined" && focusAfterSaveOnLoad != "")
        SetFocusAfterSaveOnLoad();
    else
        FocusOnFirstField("form");
    SetFocusFormControl();
    try {
        if (transid == "axpwf")
            tstwfcomments();

        if (transid == "ad_ur")
            btn17onclick = function () {
                let roleName = $("#axusergroup000F1").val();
                if (roleName == "" || roleName == null || typeof roleName == "undefined") {
                    showAlertDialog("warning", "Please enter role name to give menu access.");
                    return;
                }

                parent.OpenResponsibiltyFromRolesPage(`AddEditResponsibility.aspx?status=true&action=edit&name=${roleName}`);

            }


        AxAfterTstLoad();
    } catch (ex) { }
    //DisplaySearchDlg();
    if (AxIsTstructLocked) {
        LockTstruct();
    }
    jQuery.browser = {};
    (function () {
        jQuery.browser.msie = false;
        jQuery.browser.version = 0;
        if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
            jQuery.browser.msie = true;
            jQuery.browser.version = RegExp.$1;
        }
    })();


    if ($j(".clsPrps").length > 0) {
        $j("#tgPurpose").css("display", "inline-block");
    } else {
        $j("#tgPurpose").css("display", "none");
    }
    $('[data-toggle="popover"]').popover({
        container: 'body'
    });
    checkSuccessAxpertMsg();
    if (eval(callParent('isPrintPDFClick')))
        $j(".printhtmltopdf").click();

    if (eval(callParent('isSaveAndPrintClick'))) {
        var isSPValues = [];
        isSPValues = eval(callParent('isSavePrintValues'));
        OpenPdfFile(isSPValues[0], isSPValues[1], isSPValues[2], isSPValues[3], "");
        eval(callParent('isSaveAndPrintClick') + "= false");
        eval(callParent('isSavePrintValues') + "=[]");
    }

    makeFieldInitCap();
    var t1 = performance.now();
    //console.log("Call to form load took " + (t1 - t0) + " milliseconds.");

    $(window).scroll(function () {
        // $('#ui-datepicker-div').hide();
        KTMenu.hideDropdowns();
    });
    //$("#editUpdateButton2").click(function (e) {
    //    $('#ui-datepicker-div').hide(); // This is the preferred method.
    //});

    //if the tstruct field is axp-url (ie, field name starts with axp_url_) then make textbox hidden & make label has hyperlink
    $("[data-axp-url]").each(function () {
        $(this).after('<br/>')
        inputId = $(this).attr("for");
        text = $(this).text();
        $(this).text('');
        $("#" + inputId).attr("type", "hidden");
        urlValue = $("#" + inputId).val();
        url = $("#" + inputId).val() == "" ? "javascript:void(0);" : "window.open('" + urlValue + "','newwindow','width=600,height=500')"; //open link in window only for tstruct edit
        $("#" + inputId).after("<a style='cursor:pointer' onclick=" + url + ">" + text + "</a>");
    });

    if (typeof btnfooterlist != 'undefined' && btnfooterlist != '') {
        $('#ftbtn_iList').attr('onClick', btnfooterlist);
    }

    //if (typeof btnfooteropenlist != 'undefined' && btnfooteropenlist != "" && (typeof IsObjCustomHtml == "undefined" || IsObjCustomHtml == "False")) {
    //    //$("#dvFooter").hide();
    //    $.each(btnfooteropenlist.split(','), function (i, val) {
    //        if (val != "") {
    //            let _btnId = val.split('~')[0];
    //            if (_btnId != "New" && _btnId != "Save") {
    //                $("#" + _btnId).removeClass("d-none");
    //                if (val.split('~').length > 1) {
    //                    let _btnDefId = val.split('~')[1];
    //                    $("#" + _btnId).attr('data-id', _btnDefId);
    //                }
    //            } else {
    //                if (val.split('~').length > 1) {
    //                    let _btnDefId = val.split('~')[1];
    //                    $("#ftbtn_i" + _btnId).attr('data-id', _btnDefId);
    //                }
    //            }
    //        }
    //    });
    //    //if (recordid != "0")
    //    //    $("#ftbtn_iSave").removeAttr("class").addClass("btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm");
    //}

    if (typeof dvRefreshFromLoadModern != "undefined" && dvRefreshFromLoadModern == "true") {
        $("#dvRefreshFromLoadModern").removeClass("d-none");
        /* $("#dvRefreshFromLoadModern").show();*/
    }

    if (typeof AxpTstButtonStyle != "undefined" && AxpTstButtonStyle == "classic" && $("#btnAppsHeader").next(".menu").find(".menu-item:not(.d-none)").length == 0)
        $("#btnAppsHeader").addClass("d-none");
    //else if (typeof AxpTstButtonStyle != "undefined" && AxpTstButtonStyle == "modern" && $("#btnAppsHeader").next(".menu").find(".card .row .col-4:not(.d-none)").length == 0)
        //$("#btnAppsHeader").addClass("d-none");
             

    if (typeof hideToolBar != "undefined" && hideToolBar && $("#design").length == 0 && theMode != "design") {
        var srchBarHt = 28; //Search bar height
        $("#searchBar").hide();
        $(".dvheightframe").first().css("top", "0px");
        if (typeof isWizardTstruct != "undefined" && isWizardTstruct)
            $("#wizardBodyContent").height($("#wizardBodyContent").height() + srchBarHt);
        else {
            srchBarHt = srchBarHt + 5 + 8; //Search bar height + Padding + Margin
            $("#pagebdy").height($("#pagebdy").height() + srchBarHt);
            $(".dvheightframe").first().attr("style", $(".dvheightframe").first().attr("style") + ";height:" + ($(".dvheightframe").first().height() + srchBarHt) + "px !important;")
        }
    }

    var curFrameObj = $(window.frameElement);
    if (typeof curFrameObj.attr("id") != "undefined" && (curFrameObj.attr("id") === "axpiframe" || curFrameObj.attr("id") === "homePageDynamicFrame" || curFrameObj.attr("id").startsWith("homePageFrame"))) {
        iframeindex = 1;
        $j("#goback").hide();
    }
    if (callParentNew("IsfieldaddInDesignMode")) {
        callParentNew("IsfieldaddInDesignMode=", false)
        $('.grid-stack').addClass('dirty');
    }

    //GenTstHtmlLocalStorage();

    if (isMobile && mobileCardLayout == "none") {
        $("#wizardHeader").addClass("mobileHeader");
        $.each(DCIsGrid, function (i, data) {
            if (data === "True") {
                $("#wrapperForEditFields" + (i + 1)).removeClass("d-none");
                $("#wrapperForEditFields" + (i + 1)).addClass("mobilewrapperForEditFields");
                if ($(".wrapperForGridData" + (i + 1) + " table tbody tr").length == 0 && !axInlineGridEdit)
                    $("#colScroll" + (i + 1)).hide();
                if (axInlineGridEdit)
                    $(".editWrapTr").hide();
                $(".editLayoutFooter").hide();
                $(`#DivFrame${i + 1} .newgridbtn`).addClass("d-none");
                let gridButton = $(`#DivFrame${i + 1} .newgridbtn>ul`).html();
                $(`#wrapperForEditFields${i + 1}`).append(`<div class="clearfix"></div><div class="grdButtons btnMobile d-none"><ul class="left">${gridButton}</ul></div>`);
                $(`#wrapperForEditFields${i + 1} .btnMobile`).find(`#viewGrid${i + 1}`).addClass("d-none");
            }
        });
    } else if (isMobile && axInlineGridEdit && AxpGridForm != "form") {
        $.each(DCIsGrid, function (i, data) {
            $(".newgridbtn.gridIconBtns").find(`#viewGrid${i + 1}`).addClass('d-none');
        });
    }

    $(document).off('keyup keydown', '.select2.select2-container');
    $(document).on('keyup keydown', `.select2.select2-container`, function (e) {
        if (e.key != "undefined" && e.key.toLowerCase() == 'c' && e.ctrlKey) {
            if ($(e.currentTarget).parent().find('select.fldFromSelect').length > 0 && $(e.currentTarget).parent().find('select.fldFromSelect').attr('disabled') && $(e.currentTarget).parent().find('select.fldFromSelect').hasClass('flddis')) {
                try {
                    if ($(e.currentTarget).parent().find('select.fldFromSelect').val() != null && $(e.currentTarget).parent().find('select.fldFromSelect').val() != "") {
                        //navigator.clipboard.writeText($(e.currentTarget).parent().find('select.fldFromSelect').val());
                        if (typeof navigator.clipboard != "undefined")
                            navigator.clipboard.writeText($(e.currentTarget).parent().find('select.fldFromSelect').val());
                        else {
                            const textarea = document.createElement('textarea');
                            textarea.value = $(e.currentTarget).parent().find('select.fldFromSelect').val();
                            document.body.prepend(textarea);
                            textarea.select();
                            try {
                                document.execCommand('copy');
                            } catch (err) {
                                console.log(err);
                            } finally {
                                textarea.remove();
                            }
                        }
                    }
                    else
                        navigator.clipboard.writeText("");
                } catch (ex) {
                    navigator.clipboard.writeText("");
                }
            } else if ($(e.currentTarget).parent().find('select.fldFromSelect').length > 0 && $(e.currentTarget).parent().find('select.fldFromSelect').attr('disabled')) {
                try {
                    if ($(e.currentTarget).parent().find('select.fldFromSelect').val() != null && $(e.currentTarget).parent().find('select.fldFromSelect').val() != "") {
                        //navigator.clipboard.writeText($(e.currentTarget).parent().find('select.fldFromSelect').val());
                        if (typeof navigator.clipboard != "undefined")
                            navigator.clipboard.writeText($(e.currentTarget).parent().find('select.fldFromSelect').val());
                        else {
                            const textarea = document.createElement('textarea');
                            textarea.value = $(e.currentTarget).parent().find('select.fldFromSelect').val();
                            document.body.prepend(textarea);
                            textarea.select();
                            try {
                                document.execCommand('copy');
                            } catch (err) {
                                console.log(err);
                            } finally {
                                textarea.remove();
                            }
                        }
                    }
                    else
                        navigator.clipboard.writeText("");
                } catch (ex) {
                    navigator.clipboard.writeText("");
                }
            }
        }
    });

    $(document).off('focus', '.select2.select2-container');
    $(document).on('focus', '.select2.select2-container', function (e) {
        var isOriginalEvent = e.originalEvent; // don't re-open on closing focus event
        var isSingleSelect = $(this).find(".select2-selection--single").length > 0; // multi-select will pass focus to input
        if (isOriginalEvent && isSingleSelect) {
            if (select2EventType == "open") {
                select2EventType = "";
                if (select2IsFocused == true && select2IsOpened)
                    $(this).siblings('select.fldFromSelect').select2('close');
                select2IsOpened = false;
                select2IsFocused = false;
            } else {
                if (select2IsOpened)
                    select2EventType = "click";
                else {
                    select2IsFocused = true;
                    select2EventType = "tab";
                }
            }
            //if (recordid == "0")
            //    $(this).siblings('select:enabled').select2('open');
            if (recordid == "0") {
                let _thisId = $(this).parent().find("select.form-select").attr("id");
                if (typeof _thisId != "undefined" && _thisId != "") {
                    let _fldName = GetFieldsName(_thisId);
                    let _fldInd = GetFieldIndex(_fldName);
                    let _fldAEmpty = GetFieldProp(_fldInd, 'allowEmpty');
                    let _thidVal = GetFieldValueNew(_thisId);
                    if (_fldAEmpty == "F" && _thidVal == "" && (isAddRowWsCalled == "true" || isAddRowWsCalled == "false")) {
                        $(this).siblings('select:enabled').select2('open');
                    }
                    else {
                        try {
                            if ($("#" + _thisId).val() != null && $("#" + _thisId).val() != "" && !$("#" + _thisId).is(':disabled'))
                                ShowTooltip(_fldName, $("#" + _thisId));
                        } catch (ex) { }
                    }
                }
            }
        }
    });

    $(document).off('click', '.select2.select2-container');
    $(document).on('click', '.select2.select2-container', function (e) {
        var isOriginalEvent = e.originalEvent; // don't re-open on closing focus event
        var isSingleSelect = $(this).find(".select2-selection--single").length > 0; // multi-select will pass focus to input
        if (isOriginalEvent && isSingleSelect) {
            select2EventType = "click";
            $(this).siblings('select:enabled').select2('open');
        }
    });

    /** * @description Focus next/Prev non-grid fields on Tab/Shift+Tab along with "tabStop" field property */
    $j("input:not([id=searstr],[class=AxAddRows],[class=AxSearchField]),select:not([id=selectbox]),textarea:not(#txtCommentWF):not(.labelInp),input[type=checkbox],li.dropdown-chose,.select2.select2-container").bind("keydown", function (e) {
        var keyCode = e.keyCode || e.which;
        var tabFldId = $(this);
        if (keyCode == 9 && !e.shiftKey) {
            var curFldTabOrder = $(this).closest("[class*=fldindex]").data("dataindex");
            let TabFldDc;
            let _curMultselFld = "";
            if (typeof $(this).attr("id") != "undefined")
                TabFldDc = $(this).attr("id").substring($(this).attr("id").lastIndexOf("F") + 1, $(this).attr("id").length);
            else if ($(this).closest("div").find('select').length > 0) {
                TabFldDc = $(this).closest("div").find('select').attr('id').substring($(this).closest("div").find('select').attr('id').lastIndexOf("F") + 1, $(this).closest("div").find('select').attr('id').length);
                _curMultselFld = $(this).closest("div").find('select').attr('id');
            } else if ($(this).hasClass("tokenSelectAll"))
                TabFldDc = $(this).closest("div").attr("name").substring($(this).closest("div").attr("name").lastIndexOf("F") + 1, $(this).closest("div").attr("name").length);
            var listDivTabOrder = [];
            $(this).closest("#divDc" + TabFldDc).find(".grid-stack-item").find("[class*=fldindex]").each(function (ind, val) {
                var fName = GetFieldsName($(this).find("input:not([id=searstr],[class=AxAddRows],[class=AxSearchField]),select:not([id=selectbox]),textarea:not(#txtCommentWF):not(.labelInp),input[type=checkbox]").attr("id"));
                var fldIndex = $j.inArray(fName, FNames);
                if ($(this).css('display') != "none" && GetFieldProp(fldIndex, "tabStop") != "F" && fldIndex > -1 && FFieldReadOnly[fldIndex] != "True")
                    listDivTabOrder.push($(this).data("dataindex"));
            });
            listDivTabOrder.sort(function (a, b) {
                return a - b
            });
            $.each(listDivTabOrder, function (i, indTabOr) {
                if (curFldTabOrder < indTabOr) {
                    var NextFldId = $j(tabFldId).closest("#divDc" + TabFldDc).find("[data-dataindex=" + indTabOr + "]").find("input:not([id=searstr],[class=AxAddRows],[class=AxSearchField]),select:not([id=selectbox]),textarea:not(#txtCommentWF):not(.labelInp),input[type=checkbox]");
                    if (typeof $("#" + $(NextFldId).attr("id")).attr("disabled") == "undefined" && typeof $("#" + $(NextFldId).attr("id") + ":not(.tstOnlyTime,.tstOnlyTime24hours)").attr("readonly") == "undefined") {
                        if (!$("#" + $(NextFldId).attr("id")).hasClass("axpImg") && !$("#" + $(NextFldId).attr("id")).hasClass("fldmultiSelect")) {
                            if ($(NextFldId).length == 4)
                                return false;
                            if (!$("#" + $(NextFldId).attr("id")).hasClass('fldFromSelect') && !$("#" + $(NextFldId).attr("id")).hasClass('multiFldChk') && !$("#" + $(NextFldId).attr("id")).hasClass('multiFldChklist')) {
                                if ($("#" + $(NextFldId).attr("id")).hasClass('gridstackCalc') && $("#" + $(NextFldId).attr("id")).siblings("#cke_" + $(NextFldId).attr("id")).length > 0) {
                                    $("#" + $(NextFldId).attr("id")).focus();
                                    return true;
                                }
                                else if ($("#" + $(NextFldId).attr("id")).hasClass('profile-pic')) {
                                    $("#" + $(NextFldId).attr("id")).focus();
                                    return false;
                                }
                                else {
                                    if (_curMultselFld != "") {
                                        if ($("#" + _curMultselFld).hasClass('multiFldChk')) {
                                            $("#" + _curMultselFld).select2('close');
                                            _curMultselFld = "";
                                            setTimeout(function () {
                                                $("#" + $(NextFldId).attr("id")).focus().select();
                                                e.preventDefault();
                                                return false;
                                            }, 0);
                                        } else {
                                            $("#" + $(NextFldId).attr("id")).focus().select();
                                            e.preventDefault();
                                            return false;
                                        }
                                    } else {
                                        $("#" + $(NextFldId).attr("id")).focus().select();
                                        e.preventDefault();
                                        return false;
                                    }
                                }
                            } else {
                                $("#" + $(NextFldId).attr("id")).focus();
                                return false;
                            }
                        } else if ($("#" + $(NextFldId).attr("id")).hasClass("fldmultiSelect")) {
                            if ($(NextFldId).length == 4)
                                return false;
                            $("#" + $(NextFldId).attr("id")).focus().select();
                            return false;
                        }
                    } else if (listDivTabOrder.length - 1 == i) {
                        $(".tstructMainBottomFooter a:visible:eq(0)").focus();
                    }
                } else if (curFldTabOrder == indTabOr && listDivTabOrder.length - 1 == i) {
                    let _NextDcFind = tabFldId.parents('[id^="DivFrame"]').siblings('[id^="DivFrame"]:not(.d-none)').add(tabFldId.parents('[id^="DivFrame"]').siblings('.col-12').find('.tab-pane.active [id^="DivFrame"]:not(.d-none)')).filter(':first');
                    if (_NextDcFind.length > 0)
                        $(tabFldId.parents('[id^="DivFrame"]').siblings('[id^="DivFrame"]:not(.d-none)').add(tabFldId.parents('[id^="DivFrame"]').siblings('.col-12').find('.tab-pane.active [id^="DivFrame"]:not(.d-none)')).filter(':first').find('a')[0]).focus();
                    else
                        $(".tstructMainBottomFooter a:visible:eq(0)").focus();
                    return false;
                }
            });
        } else if (keyCode == 9 && e.shiftKey) {
            if ($(this).hasClass("select2-search__field")) {
                isSelectedValFocus = false;
                return;
            }
            var curFldTabOrder = $(this).closest("[class*=fldindex]").data("dataindex");
            let TabFldDc;
            if (typeof $(this).attr("id") != "undefined")
                TabFldDc = $(this).attr("id").substring($(this).attr("id").lastIndexOf("F") + 1, $(this).attr("id").length);
            else if ($(this).closest("div").find('select').length > 0)
                TabFldDc = $(this).closest("div").find('select').attr('id').substring($(this).closest("div").find('select').attr('id').lastIndexOf("F") + 1, $(this).closest("div").find('select').attr('id').length);
            else if ($(this).hasClass("tokenSelectAll"))
                TabFldDc = $(this).closest("div").attr("name").substring($(this).closest("div").attr("name").lastIndexOf("F") + 1, $(this).closest("div").attr("name").length);
            var listDivTabOrder = [];
            $(this).closest("#divDc" + TabFldDc).find(".grid-stack-item").find("[class*=fldindex]").each(function (ind, val) {
                var fName = GetFieldsName($(this).find("input:not([id=searstr],[class=AxAddRows],[class=AxSearchField]),select:not([id=selectbox]),textarea:not(#txtCommentWF):not(.labelInp),input[type=checkbox]").attr("id"));
                var fldIndex = $j.inArray(fName, FNames);
                if ($(this).css('display') != "none" && GetFieldProp(fldIndex, "tabStop") != "F" && fldIndex > -1 && FFieldReadOnly[fldIndex] != "True")
                    listDivTabOrder.push($(this).data("dataindex"));
            });
            listDivTabOrder.sort(function (a, b) {
                return a - b
            }).reverse();
            $.each(listDivTabOrder, function (i, indTabOr) {
                if (curFldTabOrder > indTabOr) {
                    var NextFldId = $j(tabFldId).closest("#divDc" + TabFldDc).find("[data-dataindex=" + indTabOr + "]").find("input:not([id=searstr],[class=AxAddRows],[class=AxSearchField]),select:not([id=selectbox]),textarea:not(#txtCommentWF):not(.labelInp),input[type=checkbox]");
                    if (typeof $("#" + $(NextFldId).attr("id")).attr("disabled") == "undefined" && typeof $("#" + $(NextFldId).attr("id") + ":not(.tstOnlyTime,.tstOnlyTime24hours)").attr("readonly") == "undefined") {
                        if (!$("#" + $(NextFldId).attr("id")).hasClass("axpImg") && !$("#" + $(NextFldId).attr("id")).hasClass("fldmultiSelect")) {
                            if ($(NextFldId).length == 4)
                                return false;
                            if (!$("#" + $(NextFldId).attr("id")).hasClass('fldFromSelect') && !$("#" + $(NextFldId).attr("id")).hasClass('multiFldChk') && !$("#" + $(NextFldId).attr("id")).hasClass('multiFldChklist')) {
                                if ($("#" + $(NextFldId).attr("id")).hasClass('gridstackCalc') && $("#" + $(NextFldId).attr("id")).siblings("#cke_" + $(NextFldId).attr("id")).length > 0) {
                                    $("#" + $(NextFldId).attr("id")).focus();
                                    return true;
                                }
                                else if ($("#" + $(NextFldId).attr("id")).hasClass('profile-pic')) {
                                    $("#" + $(NextFldId).attr("id")).focus();
                                    return false;
                                }
                                else {
                                    $("#" + $(NextFldId).attr("id")).focus().select();
                                    e.preventDefault();
                                    return false;
                                }
                            }
                            else {
                                $("#" + $(NextFldId).attr("id")).select();
                                return true;
                            }
                        } else if ($("#" + $(NextFldId).attr("id")).hasClass("fldmultiSelect")) {
                            if ($(NextFldId).length == 4)
                                return false;
                            $("#" + $(NextFldId).attr("id")).focus().select();
                            return false;
                        }
                    } else if (listDivTabOrder.length - 1 == i) {
                        $(".tstructMainBottomFooter a:visible:eq(0)").focus();
                    }
                }
            });
        }
    });

    setTimeout(function () {
        if (typeof isWizardTstruct != "undefined" && isWizardTstruct) {
            var mainDiv = $('#wizardTstructWrapper').outerHeight(true);
            var toolBarOH = $('#toolBarBtns').outerHeight() - 41; // 41 is the default haight for first row
            var heighFrame = $("#wizardBodyFooterWrapper").outerHeight(true);
            var wizardHeader = $('#wizardHeader').outerHeight(true);
            // $('#wizardBodyContent').css({
            //     height: `calc(100vh - ${(mainDiv + toolBarOH + wizardHeader) - (heighFrame + wizardHeader)}px)`
            // });
        } else {
            var heighFrame = $("#heightframe").outerHeight(true);
            var mainDiv = $('#dvlayout').outerHeight(true);
            var footer = $('.tstructMainBottomFooter').outerHeight(true);
            if (transid == 'axglo') {
                // $('#heightframe').css({
                //     height: `calc(100vh - 95px)`
                // });
            } else {
                //$('#heightframe').css({ height: `calc(100vh - ${(mainDiv - (heighFrame - footer))}px)` });
                if ($("body").hasClass("formLayoutWidth")) {
                    let wBdrOH = $("#wBdr").outerHeight(true);
                    if (wBdrOH < heighFrame)
                        $('.tstructMainBottomFooter').css({
                            bottom: `${heighFrame - (wBdrOH + footer)}px`
                        });
                    else
                        $('.tstructMainBottomFooter').css({
                            bottom: `-${footer}px`
                        });
                }
                // $('#wBdr').css({ height: `calc(100vh - ${(mainDiv - (heighFrame - footer))}px)` });
                if (!isMobile && $('#DivFrame1').outerHeight() < $('#heightframe').outerHeight()) {
                    if ($('#tabsCont2 .dvdcframe').length > 0) {
                        var mainHeight = $('#tabsCont2 .dvdcframe').height() + $('#myTab').outerHeight(true) + ($('#heightframe').outerHeight(true) - $('#DivFrame1').outerHeight(true) - $('#tabsCont2').parents('.wrapper').outerHeight(true) - 20);
                        $('#tabsCont2 .dvdcframe').css({
                            'min-height': mainHeight
                        });
                    }
                    $.each(DCIsGrid, function (dcIndex) {
                        DCIsGrid[dcIndex] == "False" && dcIndex != 0 ? $("#DivFrame" + (dcIndex + 1)).addClass("nonGridDcTab") : "";
                    });
                }
            }
        }
    }, 50);
    // to get latitude longitude value

    if (theMode != "design" && isMobile && ($.inArray("axp_latlong", FNames) != -1)) {
        var llIndex = ($.inArray("axp_latlong", FNames));
        axplatlongFldName = FNames[llIndex];
        callParentNew("LocationInfoAPI()", "function");
    }

    if (theMode != "design" && ($.inArray("latitude", FNames) != -1) && ($.inArray("longitude", FNames) != -1)) {
        var counter = 0;
        var intervalID = setInterval(() => {
            counter++;
            if (counter == 10) {
                clearInterval(intervalID);
            }

            var fldlatitude = `latitude000F${GetDcNo("latitude")}`;
            var fldlongitude = `longitude000F${GetDcNo("longitude")}`;

            if ($(`#${fldlatitude}`).val() == "" && $(`#${fldlongitude}`).val() == "") {

                if (getLocation() === true) {
                    clearInterval(intervalID);
                }
            }
        }, 1000);
    }

    // $(".dropdown-mul").off('click.dropdown-mula');
    // $(".dropdown-mul").on('click.dropdown-mula', function (event) {
    //     if (typeof fldmultiSelectdep == "undefined" || (typeof fldmultiSelectdep != "undefined" && fldmultiSelectdep == false))
    //         MultiGroupSelectClk(event, this);
    // });

    if (breadCrumbStr) {
        //$("#breadcrumb-panel span").removeClass("tstivtitle");
        //$("#breadcrumb-panel .bcrumb .menuBreadCrumb").length > 0 && $("#breadcrumb-panel .bcrumb .menuBreadCrumb").remove();
        //$("#breadcrumb-panel .bcrumb").append('<span class="menuBreadCrumb"><span class="breadCrumbCaption">' + breadCrumbStr + '</span>' + $("#breadcrumb-panel span.tstivCaption").text() + '</span>');
        let brcList = "";
        breadCrumbStr.split(" > ").forEach(function (item) {
            if (item != "")
                brcList += "<li class=\"breadcrumb-item text-muted\">" + item + "</li>";
        });
        if ($("#breadcrumb-panel ul.breadcrumb").length > 0)
            $("#breadcrumb-panel ul.breadcrumb").remove();
        $("#breadcrumb-panel").append(`<ul class="breadcrumb fw-bold fs-base my-1">` + brcList + `<li class="breadcrumb-item text-dark">` + $("#breadcrumb-panel h1").text() + `</li></ul>`);
    }

    if (isMobile) {
        $('#breadcrumb-panel').attr({
            'data-toggle': 'popover',
            'data-placement': 'bottom',
            'data-content': "",
        });

        $('#breadcrumb-panel[data-toggle=popover]').popover({
            content: $(".menuBreadCrumb").length > 1 ? $(".menuBreadCrumb").text() : $(".tstivCaption ").text(),
            container: 'body'
        });
    }
    if (axInlineGridEdit) {
        $('.griddivColumn').addClass('gridFixedHeader').css({ "overflow": "auto" });
    }
    if (typeof gridFixedHeader == "undefined" || gridFixedHeader == "true") {
        //$('.griddivColumn ').addClass('gridFixedHeader').css({ "overflow": "auto", "max-height": "calc(100vh - 130px)" });
        (isMobile && AxpGridForm == "form") ? "" : $('.griddivColumn').addClass('gridFixedHeader').css({ "max-height": "calc(100vh - 130px)" });
        $(".gridFixedHeader table thead tr th").css({ "background": "#fff", "position": "sticky", "top": "0" });
        $(".gridFixedHeader table tfoot tr td").css({ "background": "#fff", "position": "sticky", "bottom": "0" });
    }

    if (typeof gridFixedHeader != "undefined" && (gridFixedHeader == "" || gridFixedHeader == "false") && $(".gridFixedHeader table tfoot").length > 0) {
        (isMobile && AxpGridForm == "form") ? "" : $('.griddivColumn').addClass('gridFixedHeader').css({ "max-height": "calc(100vh - 130px)" });
        $(".gridFixedHeader table tfoot tr td").css({ "background": "#fff", "position": "sticky", "bottom": "0" });
    }

    hideacoptions();
    $("input.fldFromSelect").off('click');
    $("input.fldFromSelect").on('click', function () {
        if (isMobile) {
            let fldId = $(this).attr("id");
            if (fldId != "" && $("#" + fldId).hasClass("fldFromSelect"))
                $("#" + fldId).removeClass("fldFromSelect");
        }
    });

    if (appGlobalVarsObject._CONSTANTS.compressedMode) {
        appGlobalVarsObject.methods.toggleCompressModeUI($('body'));
    }

    $(".toolbarRightMenu .menu").find('.menu-sub').addClass("mh-300px scroll-y");

    $(".nav.nav-tabs").each((ind, tab) => {
        $(tab).find(".nav-item a.nav-link").length == 1 && $(tab).find(".nav-item a.nav-link").removeClass("active").addClass("bg-white");
    });

    //setTimeout(function () {
    //    swicthCompressMode();
    //}, 0);

    // if (callParentNew("isDWB")) {
    //     $("#breadcrumb-panel, #design").hide();
    // }

    $(".fldCustTableIcon").off('click.fldCustTableIcona');
    $(".fldCustTableIcon").on('click.fldCustTableIcona', function (event) {
        FieldTypeTable(event, this);
    });
    $(".fldCustTable ").off('keydown.fldtblA');
    $(".fldCustTable ").on("keydown.fldtblA", function (event) {
        if (event.keyCode == 13) {
            FieldTypeTable(event, this);
        }
    });
    if (typeof $("input[type='password']") != "undefined" && $("input[type='password']").length > 0) {
        var style = document.createElement('style');
        style.innerHTML = `input[type="password"]::-ms-reveal, 
    input[type="password"]::-ms-clear {
        display: none !important;
    }`;
        document.head.appendChild(style);
    }

    $j(document).on("click", ".toggle-password", function () {
        //$(".toggle-password").click(function () {

        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $($(this).attr("toggle"));
        if (input.attr("type") == "password") {
            input.attr("type", "text");
            $(this).text("visibility");
            $(this).css("color", "#999");
        } else {
            input.attr("type", "password");
            $(this).text("visibility_off");
            $(this).css("color", "#999");
        }
    });

    $('#searchBar').on('click', 'ul.dropdown-menu [data-toggle=dropdown]', function (event) {
        event.preventDefault();
        event.stopPropagation();
        $(this).parent().siblings().removeClass('open');
        $(this).parent().toggleClass('open');
    });
    if (isMobile) {
        $('body').addClass('isMobile');
        $("#design").hide();
        $(".tst-search").addClass("flex-column");
    }

    if (true || AxpTstButtonStyle != "old") {
        $("#iconsNew .right").off("click.subMenu", '.dropdown-submenu a').on("click.subMenu", '.dropdown-submenu a', function (e) {
            var $not = $(this).next('ul');
            $.each($("#iconsUl ul").not($not), function (i, val) {
                if ($(val).is(':visible')) {
                    $(val).hide();
                }
            });
            $(this).next('ul').toggle();
            //  $(this).find('i').toggle();

            e.stopPropagation();
            e.preventDefault();
        });

        $('#iconsNew .right.dropdown').off("click.menu").on("click.menu", function (e) {
            if (isMobile) {
                $('.pinIcon, .iconUIPin').hide();
            }
            if ($(this).hasClass("open")) {
                $(this).find(".dropdown-submenu ul.dropdown-menu").hide();
            }
        });

    }
    $(document).off("dblclick", ".customSetupTableMN tbody tr td div.edit-mode-content textarea:not([disabled])").on("dblclick", ".customSetupTableMN tbody tr td div.edit-mode-content textarea:not([disabled])", function (event) {
        let _grdRowTextarea = $(this);
        const currentText = _grdRowTextarea.val();
        let _html = `<div><div class="input-group input-group-sm"><textarea id="txtgrdRowTxtarea" title="" name="txtgrdRowTxtarea" class="tem Family memofam form-control" style="height: 150px;">${currentText}</textarea></div></div>`;
        let myModal = new BSModal("modalIdGrdTxtAreapopup", "Edit Textarea", _html,
            () => {
            },
            () => {
            }
        );
        myModal.modalFooter.innerHTML = "";
        let buttonOkHTML = `
            <button id="btngrdtareaCancel" class="btn btn-secondary">Cancel</button>
            <button id="btngrdtareaOk" class="btn btn-primary">OK</button>
        `;
        myModal.modalFooter.innerHTML = buttonOkHTML;
        $(document).off("click", "#btngrdtareaOk").on("click", "#btngrdtareaOk", function () {
            const newText = $("#txtgrdRowTxtarea").val();
            _grdRowTextarea.val(newText);
            myModal.close();
            MainBlur($(_grdRowTextarea));
        });
        $(document).off("click", "#btngrdtareaCancel").on("click", "#btngrdtareaCancel", function () {
            myModal.close();
        });
    });

    GetProcessTime();
    GetTotalElapsTime();
    $(".modernButtonOptions #iconsUl > li.dropdown-submenu").on('click', function (e) {
        if ($("#iconsUl > li:visible").index(this) % 3 == 0) {
            $(this).find('ul').css({
                'left': '0px'
            })
        } else if ($("#iconsUl > li:visible").index(this) % 3 == 1) {
            $(this).find('ul').css({
                'left': '-110px'
            })
        } else if ($("#iconsUl > li:visible").index(this) % 3 == 2) {
            $(this).find('ul').css({
                'left': '-223px'
            })
        }
    });
    barCodeScannerInit();

    try {
        document.querySelectorAll("#btnAppPegMore").forEach((item) => {
            KTMenu.getInstance(item).on("kt.menu.dropdown.show", function (item) {
                if (isPegApproveConfirm == "axapprove") {
                    isPegApproveConfirm = "";
                    return;
                }
                let _axPegJson = AxPegJSON;
                if (typeof _axPegJson[0].data.cmsg_appcheck != "undefined" && _axPegJson[0].data.cmsg_appcheck != "") {
                    if (!axPegConfirmation("Approve", _axPegJson[0].data.cmsg_appcheck)) {
                        KTMenu.getInstance(item)?.hide();
                        return;
                    } else {
                        setTimeout(function () {
                            document.querySelector(`textarea[id="txtPegComments"]`).focus();
                        }, 0);
                    }
                }
            });
        });

        document.querySelectorAll("#btnReturnPegMore").forEach((item) => {
            KTMenu.getInstance(item).on("kt.menu.dropdown.show", function (item) {
                if (isPegApproveConfirm == "axreturn") {
                    isPegApproveConfirm = "";
                    return;
                }
                let _axPegJson = AxPegJSON;
                if (typeof _axPegJson[0].data.cmsg_return != "undefined" && _axPegJson[0].data.cmsg_return != "") {
                    if (!axPegConfirmation("Return", _axPegJson[0].data.cmsg_return)) {
                        KTMenu.getInstance(item)?.hide();
                        return;
                    } else {
                        setTimeout(function () {
                            document.querySelector(`textarea[id="txtPegReturnComments"]`).focus();
                        }, 0);
                    }
                }
            });
        });

        document.querySelectorAll("#btnRejectPegMore").forEach((item) => {
            KTMenu.getInstance(item).on("kt.menu.dropdown.show", function (item) {
                if (isPegApproveConfirm == "axreject") {
                    isPegApproveConfirm = "";
                    return;
                }
                let _axPegJson = AxPegJSON;
                if (typeof _axPegJson[0].data.cmsg_reject != "undefined" && _axPegJson[0].data.cmsg_reject != "") {
                    if (!axPegConfirmation("Reject", _axPegJson[0].data.cmsg_reject)) {
                        KTMenu.getInstance(item)?.hide();
                        return;
                    } else {
                        setTimeout(function () {
                            document.querySelector(`textarea[id="txtPegRejectComments"]`).focus();
                        }, 0);
                    }
                }
            });
        });
    } catch (ex) { }

    if (theMode != 'design' && !isMobile)
        GenTstHtmlLocalStorage();

    if (typeof recordid != "undefined" && recordid != "" && recordid != "0")
        CheckTransReadOnly();

    if (typeof isTstPop != "undefined" && isTstPop == 'True')
        $("#btnClearCacheReloadForm").addClass('d-none');
    if (typeof recordid != "undefined" && recordid != "" && recordid != "0")
        $("#btnClearCacheReloadForm").addClass('d-none');
    if (isMobile) {
        $("#dvlayout").find(".d-flex.align-items-center.flex-nowrap.text-nowrap").addClass('ms-auto');

        $("#dvlayout").find(".toolbarRightMenu").css({
            'position': 'relative',
            'right': '0px !important'
        });
        $("#dvlayout").find(".toolbarRightMenu").removeClass("toolbarRightMenu");
        var design_Form = designObj[0];

        if (typeof design_Form != "undefined" && typeof design_Form.formAlignment != "undefined" && design_Form.formAlignment == 'right') {
            var element = document.getElementById('dvlayout');
            element.classList.remove('position-absolute');
            element.classList.add('position-auto');
        }
    }
    tabedDcSliderHtml();
    $('#lblddfooter').hover(function () {
        var tooltipText = typeof $(this).attr('title') != "undefined" ? $(this).attr('title') : $(this).attr('data-title');
        $(this).attr('data-title', tooltipText).removeAttr('title');
        var tooltip = $('<div class="tooltipDdetails">').text(tooltipText);
        $(this).append(tooltip);
        var position = $(this).position();
        var elementWidth = $(this).outerWidth();
        var tooltipHeight = tooltip.outerHeight();
        tooltip.css({
            position: 'absolute',
            top: position.top - tooltipHeight - 10, // Adjust as needed
            left: position.left,
            backgroundColor: '#e4e6ef',
            color: 'black',
            padding: '5px',
            borderRadius: '3px',
            zIndex: 999999
        }).fadeIn();
    }, function () {
        $('.tooltipDdetails').remove();
    });

    try {
        if (typeof parent.axProcessObj != "undefined") {
            parent.$("#Process_Flow").scrollTop(0);
            parent.ShowDimmer(false);
        }
    } catch (ex) { }    

    setExtraBtnTooltip();
});

function GenTstHtmlLocalStorage() {
    try {       
        let _thsiifId = window.frameElement.id;
        if (_thsiifId == 'middle1' || _thsiifId.startsWith("axMultiiframe_")) {
            if (window.frameElement.src.indexOf('ivtstload.aspx') > -1)
                callParentNew("lastLoadtstId=", 'fromiview');
            else
                callParentNew("lastLoadtstId=", window.frameElement.contentWindow.jQuery('#form1').attr('action'));
        }        
        callParentNew("updateAppLinkObj")?.(window.frameElement.src, 0, window?.frameElement?.id == "axpiframe", { ...window?.frameElement?.dataset });
        if (_thsiifId == 'middle1' || _thsiifId.startsWith("axMultiiframe_")) {
            if (recordid != "" && recordid != "0" && $(".tstructMainBottomFooter .tstructBottomLeftButton").length == 0 && loadRecordFromSearch == false) {
                if (typeof AxDiscardNxtPrevFc != "undefined" && AxDiscardNxtPrevFc.length > 0) {
                    let lnkprev = 'lnkprev~';
                    let filteredPrev = AxDiscardNxtPrevFc.filter(item => {
                        return item.toLowerCase().startsWith(lnkprev.toLowerCase()) && item.slice(lnkprev.length).toLowerCase() === item.slice(lnkprev.length);
                    });
                    let lnkprevClass = "";
                    if (typeof filteredPrev != "undefined" && filteredPrev.length > 0) {
                        let _thisEle = filteredPrev[0].split('~')[1];
                        if (_thisEle == "hide")
                            lnkprevClass = "d-none";
                    }

                    let lnknext = 'lnknext~';
                    let filteredNxt = AxDiscardNxtPrevFc.filter(item => {
                        return item.toLowerCase().startsWith(lnknext.toLowerCase()) && item.slice(lnknext.length).toLowerCase() === item.slice(lnknext.length);
                    });

                    let lnknextClass = "";
                    if (typeof filteredNxt != "undefined" && filteredNxt.length > 0) {
                        let _thisEle = filteredNxt[0].split('~')[1];
                        if (_thisEle == "hide")
                            lnknextClass = "d-none";
                    }
                    $(".tstructMainBottomFooter div:eq(0)").prepend(`<div class="text-dark tstructBottomLeftButton d-flex align-items-center">
    <a class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2 lnkPrev ${lnkprevClass}" id="lnkPrev" href="javascript:void(0)" onclick="lnkPrevClick();" title="Previous Transction"><span class="material-icons material-icons-style">chevron_left</span></a>
    <a class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2 lnkNext ${lnknextClass}" id="lnkNext" href="javascript:void(0)" onclick="lnkNextClick();" title="Next Transaction"><span class="material-icons material-icons-style">chevron_right</span></a>
</div>`);
                } else {
                    $(".tstructMainBottomFooter div:eq(0)").prepend(`<div class="text-dark tstructBottomLeftButton d-flex align-items-center">
    <a class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2 lnkPrev" id="lnkPrev" href="javascript:void(0)" onclick="lnkPrevClick();" title="Previous Transction"><span class="material-icons material-icons-style">chevron_left</span></a>
    <a class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2 lnkNext" id="lnkNext" href="javascript:void(0)" onclick="lnkNextClick();" title="Next Transaction"><span class="material-icons material-icons-style">chevron_right</span></a>
</div>`);
                }
            } else if (recordid == "0" && $(".tstructMainBottomFooter .tstructBottomLeftButton").length > 0) {
                $(".tstructMainBottomFooter .tstructBottomLeftButton").remove();
            } else if ($(".tstructMainBottomFooter .tstructBottomLeftButton").length > 0) {
                if (typeof AxDiscardNxtPrevFc != "undefined" && AxDiscardNxtPrevFc.length > 0) {
                    let lnkprev = 'lnkprev~';
                    let filteredPrev = AxDiscardNxtPrevFc.filter(item => {
                        return item.toLowerCase().startsWith(lnkprev.toLowerCase()) && item.slice(lnkprev.length).toLowerCase() === item.slice(lnkprev.length);
                    });
                    let lnkprevClass = "";
                    if (typeof filteredPrev != "undefined" && filteredPrev.length > 0) {
                        let _thisEle = filteredPrev[0].split('~')[1];
                        if (_thisEle == "hide")
                            lnkprevClass = "d-none";
                    }

                    $(".tstructMainBottomFooter #lnkPrev").addClass(lnkprevClass);

                    let lnknext = 'lnknext~';
                    let filteredNxt = AxDiscardNxtPrevFc.filter(item => {
                        return item.toLowerCase().startsWith(lnknext.toLowerCase()) && item.slice(lnknext.length).toLowerCase() === item.slice(lnknext.length);
                    });

                    let lnknextClass = "";
                    if (typeof filteredNxt != "undefined" && filteredNxt.length > 0) {
                        let _thisEle = filteredNxt[0].split('~')[1];
                        if (_thisEle == "hide")
                            lnknextClass = "d-none";
                    }

                    $(".tstructMainBottomFooter #lnkNext").addClass(lnknextClass);
                }
            }
        }

        if (_thsiifId != 'middle1' && !_thsiifId.startsWith("axMultiiframe_"))
            $("#btnClearCacheReloadForm").addClass('d-none');

        loadRecordFromSearch = false;
        if (typeof AxDiscardNxtPrevFc != "undefined" && AxDiscardNxtPrevFc.length > 0) {
            let lnkprev = 'discard~';
            let filteredPrev = AxDiscardNxtPrevFc.filter(item => {
                return item.toLowerCase().startsWith(lnkprev.toLowerCase()) && item.slice(lnkprev.length).toLowerCase() === item.slice(lnkprev.length);
            });
            if (typeof filteredPrev != "undefined" && filteredPrev.length > 0) {
                let _thisEle = filteredPrev[0].split('~')[1];
                if (_thisEle != "hide")
                    hideDiscardButton();
            } else
                hideDiscardButton();
        } else {
            hideDiscardButton();
        }
        showSaveNewbtns();

        if (typeof transid != "undefined" && transid == "ad_af") {
            $("#ftbtn_iNew").addClass("d-none");
            $("#ftbtn_iDiscard").addClass("d-none");
        }

        if (isServerSide == 'true' && (_thsiifId == 'middle1' || _thsiifId.startsWith("axMultiiframe_"))) {
            try {
                let _thisCKEditor = false;
                for (var instanceName in CKEDITOR.instances) {
                    if (CKEDITOR.instances.hasOwnProperty(instanceName)) {
                        _thisCKEditor = true;
                        break;
                    }
                }
                if (_thisCKEditor) {
                    setTimeout(function () {
                        setTimeout(function () {
                            StoreTstHmtlLoad();
                        }, 100);
                    }, 0);
                    return;
                }
            } catch (ex) { }
            setTimeout(function () {
                setTimeout(function () {
                    StoreTstHmtlLoad();
                }, 100);
            }, 0);
        } else {
            isServerSide = 'false';
            $("#hdnTstHtmLs").val('');
        }
    } catch (ex) { }
}

function StoreTstHmtlLoad() {
    let _thsiifId = window.frameElement.id;
    if (_thsiifId.startsWith("axMultiiframe_"))
        callParentNew(`SetTstFrameMultiFrame(${_thsiifId})`, "function");
    else
        callParentNew("SetTstFrame()", "function");

    try {
        callParentNew("isdummyLoad=", "");
        let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        let _formLoadRes = $("#hdnTstHtmLs").val();
        _formLoadRes += "♦♠♣♥" + $('.subres').html();
        _formLoadRes = _formLoadRes.replace(/var\s+LoadResult\s*=\s*(['"`])[\s\S]*?\1\s*;/gi, "var LoadResult = '';");
        $("#hdnTstHtmLs").val('');
        var serializedContent = new XMLSerializer().serializeToString(callParentNew("originalContent"));
        var _finalTstHtml = serializedContent + '♠♠♠' + _formLoadRes;
        try {
            let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + transid + "-" + appSUrl + "♥)", "function");
            if (_thisKey.length > 0) {
                for (const val of _thisKey) {
                    localStorage.removeItem(val);
                }
            }
            var _time = new Date();
            var _localTime = _time.getTime();
            localStorage.setItem("tstHtml♠" + transid + "-" + appSUrl + "♥" + _localTime, _finalTstHtml);
            StoreLsTstHtml(transid, _finalTstHtml);
        } catch (ex) {
            if (ex.message.indexOf('exceeded the quota') > -1) {
                var jsonString = JSON.stringify(_finalTstHtml);
                var sizeInBytes = new Blob([jsonString]).size;
                var _thisKeys = callParentNew("getKeysWithPrefix(tstHtml♠)", "function");
                if (_thisKeys.length > 0) {
                    var ascOrderKeys = _thisKeys
                        .filter(x => x.split('♥')[1])
                        .sort((a, b) => {
                            const numA = parseInt(a.split('♥')[1], 10);
                            const numB = parseInt(b.split('♥')[1], 10);
                            return numA - numB;
                        });

                    var totalSpace = 10 * 1024 * 1024;
                    for (const val of ascOrderKeys) {
                        localStorage.removeItem(val);
                        var _usedMemory = getUsedLocalStorageSpace();
                        if ((totalSpace - _usedMemory) > sizeInBytes) {
                            break;
                        }
                    }
                    try {
                        var _ttime = new Date();
                        let _tlocalTime = _ttime.getTime();
                        localStorage.setItem("tstHtml♠" + transid + "-" + appSUrl + "♥" + _tlocalTime, _finalTstHtml);
                        StoreLsTstHtml(transid, _finalTstHtml);
                    } catch (ex) { }
                }
            }
        }
        callParentNew("originalContent=", "");
        //if (callParentNew("originaltrIds").filter(x => x == transid).length == 0)
        //    callParentNew("originaltrIds").push(transid);
        try {
            let appSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            let storedKey = 'originaltrIds-' + appSessUrl;
            let transidArray = JSON.parse(localStorage.getItem(storedKey) || '[]');
            if (!transidArray.includes(transid)) {
                transidArray.push(transid);
                localStorage.setItem(storedKey, JSON.stringify(transidArray));
            }
        } catch (ex) { }
        isServerSide = 'false';
    } catch (ex) { }
}

function StoreLsTstHtml(_transid, _tstHtml) {
    try {
        $.ajax({
            url: 'tstruct.aspx/StoreLsTstHtml',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                _transid: _transid,
                _tstHtml: _tstHtml
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
            }, error: function (error) {
            }
        });
    } catch (ex) { }
}
function getUsedLocalStorageSpace() {
    var usedSpace = 0;
    // Iterate through all keys in localStorage
    for (var key in localStorage) {
        if (localStorage.hasOwnProperty(key)) {
            usedSpace += (key.length + localStorage[key].length) * 2; // Approximate size in bytes
        }
    }
    return usedSpace;
}

function swicthCompressMode(dvId) {
    if (appGlobalVarsObject._CONSTANTS.compressedMode) {
        if (typeof dvId != "undefined" && dvId != "") {
            $(".compressedModeUI .inline-edit").each(function (index, el) {
                // $(el).find(".form-check-label").addClass("col-form-label-sm fw-boldest");
                $(el).find(".form-check-input").removeClass("h-40px w-40px");
                $(el).find(".selection .select2-selection").addClass("form-select-sm");
                $(el).find(".select2-hidden-accessible").addClass("form-select-sm");
                $(el).find(".edit-mode-content").addClass("input-group-sm");
                $(el).find(".edit-mode-content .input-group").addClass("input-group-sm");
                $(el).find(".edit-mode-content .fldmultiSelect").addClass("py-0");
                $(el).find(".edit-mode-content .select2-selection__choice").addClass("py-1 my-0");
            });

            // $(".compressedModeUI textarea").addClass("py-0 min-h-25px h-25px");
            $(".compressedModeUI table.customSetupTableMN tr textarea:not([data-txt-area]),.compressedModeUI  table.customSetupTableMN tr label").addClass("py-0 min-h-25px h-25px");
            $(".compressedModeUI table.customSetupTableMN tr textarea[data-txt-area]").addClass("min-h-25px h-30px");
            $(".compressedModeUI table.customSetupTableMN").addClass("table-sm");

            $(".compressedModeUI .dvdcframe .gridIconBtns a").addClass("btn-sm");

            $(".compressedModeUI table.customSetupTableMN .form-check").addClass("py-0");

        } else {
            // $(".compressedModeUI").removeClass("p-0").addClass("p-1");

            $(".compressedModeUI .input-group").each(function (index, el) {
                $(el).addClass("input-group-sm");
                // $(el).find(".form-check-label").addClass("col-form-label-sm fw-boldest");
                $(el).find(".form-check-input").removeClass("h-40px w-40px");
                if (typeof AxEnableCheckbox != "undefined" && AxEnableCheckbox == "true") {
                    $(el).find(".form-check-input").parents('.input-group').addClass('flex-shrink-1');
                    $(el).find(".form-check-input").parents('.input-group').css("flex-basis", "content");
                }

                $(el).find(".selection .select2-selection").addClass("form-select-sm");
                $(el).find(".select2-hidden-accessible").addClass("form-select-sm");
                $(el).find(".form-check-input").parents(".agform").addClass("d-flex");
                if (typeof dcLayoutType == "undefined" || dcLayoutType == "" || dcLayoutType == "default")
                 if (!staticRunMode){
                    $(el).find(".form-check-input").parents(".grid-stack-item").addClass("d-flex");
                    $(el).find(".form-check-input").parents(".agform").addClass("d-flex");
                 }
                $(el).find(".radiohori,.radiovert").parents(".agform").addClass("flex-column");

            });

            $(".compressedModeUI .inline-edit").each(function (index, el) {
                // $(el).find(".form-check-label").addClass("col-form-label-sm fw-boldest");
                $(el).find(".form-check-input").removeClass("h-40px w-40px");
                $(el).find(".selection .select2-selection").addClass("form-select-sm");
                $(el).find(".select2-hidden-accessible").addClass("form-select-sm");
            });

            $(".compressedModeUI table.customSetupTableMN tr textarea:not([data-txt-area]),.compressedModeUI  table.customSetupTableMN tr label").addClass("py-0 min-h-25px h-25px");

            $(".compressedModeUI table.customSetupTableMN tr textarea[data-txt-area]").addClass("min-h-25px h-30px");

            if (typeof dcLayoutType == "undefined" || dcLayoutType == "" || dcLayoutType == "default") {
                // $(".compressedModeUI .labelcol").removeClass("row");
            }
            else {
                let fcwidth = parseInt(designObj[0].fieldCaptionWidth);
                fcwidth = fcwidth / 10;
                fcwidth = 12 - fcwidth;
                if (!$(".compressedModeUI .input-group").parent().hasClass("layoutAdded")) {
                    $(".compressedModeUI .input-group").wrap(`<div class='col-sm-${fcwidth} layoutAdded'></div>`);
                }
                $(".upload-button").addClass('mt-4');
                $(".fldImageCamera").removeClass('mt-n4').addClass('mt-0');
                if (!staticRunMode)
                    $(".colFldGridStackWidth").addClass('mb-3');
            }

            // $(".compressedModeUI .fld-wrap3 label").addClass("col-form-label-sm fw-boldest m-0 p-0");

            $(".compressedModeUI .agform").addClass("px-1");

            $(".compressedModeUI table.customSetupTableMN").addClass("table-sm");

            $(".compressedModeUI .toolbarRightMenu a,.compressedModeUI .toolbarRightMenu button").addClass("btn-sm");
            $(".compressedModeUI .toolbarRightMenu a span,.compressedModeUI .toolbarRightMenu button span").addClass("material-icons-2");

            $(".compressedModeUI .BottomToolbarBar a").addClass("btn-sm");
            $(".compressedModeUI .BottomToolbarBar a span").addClass("material-icons-style material-icons-2");
            $(".compressedModeUI .tstructBottomLeftButton a span").addClass("material-icons-style material-icons-2");
            $(".compressedModeUI .tstructBottomLeftButton .btn").addClass("btn-sm");

            //$("select.multiFldChk,select.multiFldChklist,select.fldmultiSelect").parents('.agform').addClass('d-flex flex-column');
            //$("select.multiFldChk,select.multiFldChklist,select.fldmultiSelect").parent().addClass('flex-root overflow-auto');
            $(".compressedModeUI textarea.CodeMirrorApplied").parent().addClass("overflow-auto");
        }
    } else {
        $(".content table.customSetupTableMN tr:not(.inline-edit) textarea:not([data-txt-area]),.content  table.customSetupTableMN tr:not(.inline-edit) label").addClass("py-2 min-h-30px h-30px");
        $(".content table.customSetupTableMN tr textarea[data-txt-area]").addClass("min-h-25px h-30px");
        $(".content table.customSetupTableMN").removeClass("table-sm");

        // $(".content .dvdcframe .gridIconBtns a").removeClass("btn-sm");
        $(".content .dvdcframe .gridIconBtns a").addClass("btn-sm");

        $(".btn.btn-icon").addClass("btn-sm").find(".material-icons").addClass("material-icons-style material-icons-2");

        $(".content table.customSetupTableMN .form-check").addClass("py-2");

        $(".content .dropzone").parent(".form-control").removeClass("p-0").addClass("p-1 rounded-1");
        $(".content [id^=DivFrame] .form-switch .form-check-input").removeClass("w-40px").addClass("w-50px");
        $(".content .form-check-input").parents(".agform").addClass("d-flex");

        $("textarea.memofam.gridstackCalc").parent().addClass('flex-root overflow-auto').parents('.agform').addClass('d-flex flex-column');

        $("select.multiFldChk,select.multiFldChklist,select.fldmultiSelect").parents('.agform').addClass('d-flex flex-column');
        $("select.multiFldChk,select.multiFldChklist,select.fldmultiSelect").parent().addClass('flex-root overflow-auto');
        $(".content textarea.CodeMirrorApplied").parent().addClass("overflow-auto");
        if (typeof dcLayoutType == "undefined" || dcLayoutType == "" || dcLayoutType == "default")
            $(".content .radiohori,.content .radiovert").parents(".agform").addClass("flex-column");
        if (!staticRunMode)
            $(".content .form-check-input").parents(".grid-stack-item").addClass("d-flex");

        if (typeof dcLayoutType != "undefined" && (dcLayoutType == "single" || dcLayoutType == "double" || dcLayoutType == "triple")) {
            let fcwidth = parseInt(designObj[0].fieldCaptionWidth);
            fcwidth = fcwidth / 10;
            fcwidth = 12 - fcwidth;
            if (!$(".content .input-group").parent().hasClass("layoutAdded")) {
                $(".content .input-group").wrap(`<div class='col-sm-${fcwidth} layoutAdded'></div>`);
            }
            $(".upload-button").addClass('mt-4');
            $(".fldImageCamera").removeClass('mt-n4').addClass('mt-0');
            if (!staticRunMode)
                $(".colFldGridStackWidth").addClass('mb-3');
        }
        $("textarea.select2-search__field").addClass("cursor-pointer");
    }

    if (staticRunMode) {
        $(".fld-wrap3").addClass("text-truncate");
    }

    /* Image Upload */
    if (typeof dcLayoutType == "undefined" || dcLayoutType == "" || dcLayoutType == "default")
        $(".imageFileUpload").parents().find(".flex-root").parent().addClass("d-flex flex-column");
    else
        $(".imageFileUpload").parents().find(".flex-root").parent().addClass("d-flex");

    if (isMobile) {
        $(".tstructMainBottomFooter").removeClass("content p-1 pt-0").addClass("bg-white p-0 shadow-sm scroll-x");

        let navBtns = ["Prev", "Next"];
        $.each(navBtns, (index, value) => {
            $(`.tstructBottomLeftButton .lnk${value}`).removeClass("shadow-sm").addClass("flex-column");
            $(`.tstructBottomLeftButton .lnk${value} > .lnkText`).length == 0 ? $(`.tstructBottomLeftButton .lnk${value}`).append(`<span class="lnkText">${value}</span>`) : "";
            $(`.tstructBottomLeftButton .lnk${value} > span.material-icons`).addClass("material-icons-2qx mx-auto");
        });
        $(".BottomToolbarBar").addClass("d-flex");
        $(".BottomToolbarBar .dwbIvBtnbtm").removeClass("shadow-sm").addClass("flex-column");
        $(".BottomToolbarBar .dwbIvBtnbtm > span.material-icons").addClass("material-icons-2x mx-auto");
        $(".BottomToolbarBar .dwbIvBtnbtm#ftbtn_iSave").addClass("btn-light-primary");
    }
    if (AxpGridForm == "form") {
        $(".divBarQrScan").parent().addClass("input-group");
    }
    rtfCkeditorAlignment();
}

function getlatlongitude() {
    try {
        var latlongvalue = getRedisString('hybridinfo', callParentNew("hybridGUID"));
        if (latlongvalue != "") {
            var json = JSON.parse(latlongvalue);
            latlongvalue = json.location.coords.latitude + "," + json.location.coords.longitude;
            var latLongId = axplatlongFldName + "000F" + GetDcNo(axplatlongFldName);
            SetFieldValue(latLongId, latlongvalue);
            UpdateFieldArray(latLongId, "000", latlongvalue, "parent");

            if (latlongvalue == "") {
                AdditionalRunTimeMsg("Geo LocationInfoAPI: LocationInfoAPI called forcelly.");
                callParentNew("LocationInfoAPI(true)", "function");
            } else {
                AdditionalRunTimeMsg("Geo LocationInfoAPI: " + latLongId + ":" + latlongvalue);
            }
            //MainBlur($j("#" + latLongId));
            //$j("#" + latLongId).blur();
        } else {
            AdditionalRunTimeMsg("Geo LocationInfoAPI: LocationInfoAPI called forcelly..");
            callParentNew("LocationInfoAPI(true)", "function");
        }
    } catch (ex) { }
}

function pinItemToTaskbar(item) {
    var temp = $(item).clone();
    temp.find('a > i').remove(); //remove pin icon
    if (temp.hasClass("dropdown-submenu")) {
        temp.removeClass("dropdown-submenu");
        temp.children('a').append('<span class="icon-arrows-down"></span>'); //add down arrow to dropdown item
        // temp.find("ul.dropdown-menu").hide();
        if (temp.attr('id') != "filterWrapper" && (AxpTstButtonStyle == "modern" || temp.attr('id') != "ivirCButtonsWrapper")) {
            temp.addClass("dropdown");
            temp.find('ul').removeAttr("style");
            if (temp.find('a#tasks'))
                temp.find('li').addClass('liTaskItems');
        } else
            if (temp.find('ul').hasClass('dropdown-menu')) {
                temp.find('ul').removeClass('dropdown-menu');
            }
        if (temp.attr('id') == "ivirCButtonsWrapper") {
            var active = temp.find('ul  a:not(.active):eq(0)');
            $(active).find('.customIcon').removeClass('customIcon');
            $(active).html($(active).html().replace($(active).text(), ''));
            temp.html(active.clone());
        }
    } else
        temp.addClass('actionWrapper');

    temp.removeClass("waves-effect").removeClass("waves-block");

    if (temp.attr('id'))
        temp.attr('id', 'pinned' + temp.attr('id'));
    $.each(temp.find('*'), function (index, element) {
        if ($(element).attr('id')) {
            $(element).attr('id', 'pinned' + $(element).attr('id'));
        }

    });
    $('#pinnediconsUl').append(temp);
    setPinedIconContainerWidth();
}

function setPinedIconContainerWidth() {
    let titleBarExtrasWidth = $("#breadcrumb").outerWidth(true) + $(".toolbarRightMenu").outerWidth(true) + 50;

    if (typeof isAxpertPopup != "undefined" && isAxpertPopup) {
        titleBarExtrasWidth += 35;
    }

    $(".modernButtonOptions #pinnedsearchBar").css({
        'width': `calc(100vw - ${titleBarExtrasWidth}px)`,
        'max-width': `calc(100vw - ${titleBarExtrasWidth}px)`
    });

    var neededHeight = $("#pinnediconsUl").outerWidth(true);
    var totalChildHeight = 0;
    $("ul#pinnediconsUl").children("li").each(function () {
        totalChildHeight += $(this).outerWidth(true);
        if (totalChildHeight > neededHeight) {
            $(this).hide();
            $(this).nextAll().hide();
            return false;
        } else {
            $(this).show();
            $(this).nextAll().show();
        }
    });
}


function hideacoptions() {
    if (isMobile) {
        $(".autoClickddl").parent(".edit").removeAttr('style');
        $(".autoinputtxtclear").hide();
        $(".virtualKeyboard").hide();
    }
}

function getLocation() {
    var mobileLatLong = {};
    if (findGetParameter("recordid") > 0) {
        mobileLatLong.latitude = $("#latitude000F" + GetDcNo("latitude")).val() || 0;
        mobileLatLong.longitude = $("#longitude000F" + GetDcNo("longitude")).val() || 0;
    } else {
        try {
            var mobileAPI = callParentNew("ok");

            if (mobileAPI) {
                mobileLatLong = JSON.parse(mobileAPI.getAndroidLocation());
            }
        } catch (ex) {

        }
    }

    if (($.isEmptyObject(mobileLatLong) || mobileLatLong.latitude == 0 || mobileLatLong.longitude == 0) && navigator.geolocation) {
        return navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        return showPosition({
            coords: {
                latitude: mobileLatLong.latitude,
                longitude: mobileLatLong.longitude
            }
        });
    }

}

function showPosition(position) {
    var fldlatitude = "",
        fldlongitude = "";
    if (position && position.coords && position.coords.latitude != 0 && position.coords.latitude != 0 && !IsGridField("latitude") && !IsGridField("longitude")) {
        var latitudeDc = GetDcNo("latitude");
        var longitudeDc = GetDcNo("longitude");

        fldlatitude = "latitude000F" + latitudeDc;
        fldlongitude = "longitude000F" + longitudeDc;

        if ($("#" + fldlatitude).val() == "") {
            $("#" + fldlatitude).val(position.coords.latitude); //.trigger("blur");
            UpdateFieldArray(fldlatitude, "000", $("#" + fldlatitude).val(), "parent");
            UpdateAllFieldValues(fldlatitude, $("#" + fldlatitude).val());
        }
        if ($("#" + fldlongitude).val() == "") {
            $("#" + fldlongitude).val(position.coords.longitude); //.trigger("blur");
            UpdateFieldArray(fldlongitude, "000", $("#" + fldlongitude).val(), "parent");
            UpdateAllFieldValues(fldlongitude, $("#" + fldlongitude).val());
        }

        var googleMapsApiKeyVal = callParentNew("googleMapsApiKey");
        var files = {
            css: [],
            js: [`https://maps.googleapis.com/maps/api/js?key=${callParentNew("googleMapsApiKey")}`]
        };

        if (($.inArray("latlongmap", FNames) != -1) && googleMapsApiKeyVal) {
            loadAndCall({
                files,
                callBack: function () {
                    if (google) {
                        var fldLatLongMap = "";
                        var latlongmapDc = GetDcNo("latlongmap");
                        fldLatLongMap = "latlongmap000F" + latlongmapDc;
                        var latLongMapField = $("#" + fldLatLongMap);
                        var position = center = {
                            lat: parseFloat($("#" + fldlatitude).val()),
                            lng: parseFloat($("#" + fldlongitude).val())
                        };
                        var geocoder = new google.maps.Geocoder();
                        geocoder.geocode({
                            'location': position
                        }, function (results, status) {
                            if (status === 'OK') {
                                if (results[0]) {
                                    latLongMapField.val(results[0].formatted_address).trigger("blur");
                                    if (latLongMapField.is("textarea")) {
                                        var map = new google.maps.Map(latLongMapField.hide().before(`<div class="${latLongMapField.attr("class")} style="${latLongMapField.attr("style")}"></div>`).prev()[0], {
                                            zoom: 11,
                                            center
                                        });

                                        var infowindow = new google.maps.InfoWindow;

                                        if (googleMapsZoom) {
                                            map.setZoom(+googleMapsZoom);
                                        }

                                        var marker = new google.maps.Marker({
                                            position,
                                            map
                                        });
                                        infowindow.setContent(results[0].formatted_address);
                                        infowindow.open(map, marker);
                                    }
                                }
                            }
                        });
                    }
                }
            });
        }

    }

    if ($(`#${fldlatitude}`).val() != "" && $(`#${fldlongitude}`).val() != "") {
        return true;
    } else {
        return false;
    }
}

$(document).on("click", ".virtualKeyboard", function () {
    $(this).parent().children("input").attr("onfocus", function (index, attr) {
        return attr == "blur()" ? null : "blur()";
    });
    $(this).parent().children("input").focus();
    $(this).children().toggleClass('green');
});

function AxpFileFields() {
    if (typeof AxpFileUploadFields != "undefined" && AxpFileUploadFields.length > 0) {
        callParentNew("tstAxpFileFlds=", true);
    } else
        callParentNew("tstAxpFileFlds=", false);
}

function SetFieldSetCarryValue() {
    if (recordid == "0" && (AxActiveAction == "Save" || AxActiveAction == "New")) {
        if (typeof FSetCarry != "undefined") {
            FSetCarry.forEach(function (fldSetCarry) {
                var setCaryyDcNo = GetDcNo(fldSetCarry);
                if (!IsDcGrid(setCaryyDcNo)) {
                    var fieldId = fldSetCarry + "000F" + setCaryyDcNo;
                    $.each(SetCarryFlds, function (j, value) {
                        if (value.split("♠")[0] == fieldId) {
                            SetFieldValue(fieldId, value.split("♠")[1]);
                            var fRowNo = GetFieldsRowNo(fieldId);
                            var dbRowNo = GetDbRowNo(fRowNo, setCaryyDcNo);
                            UpdateFieldArray(fieldId, dbRowNo, value.split("♠")[1], "parent", "AutoComplete");
                        }
                    });
                }
            });
        }
        SetCarryFlds = new Array();
    }
}

function GetFldSetCarryValue() {
    try {
        if (SetCarryFlds.length > 0)
            SetCarryFlds = new Array();
        if (typeof FSetCarry != "undefined") {
            FSetCarry.forEach(function (fldSetCarry) {
                var setCaryyDcNo = GetDcNo(fldSetCarry);
                if (!IsDcGrid(setCaryyDcNo)) {
                    var fieldId = fldSetCarry + "000F" + setCaryyDcNo;
                    SetCarryFlds.push(fieldId + "♠" + $("#" + fieldId).val());
                }
            });
        }
    } catch (ex) { }
}

function OnMobileNewTst() {
    $("#wizardTstructWrapper wizardWrapper").remove();
    $.each(DCIsGrid, function (i, data) {
        if (data === "True") {
            $("#wrapperForEditFields" + (i + 1) + " .btnMobile").remove();
        }
    });
}

function checkIfFormChanges() {
    if ($(".grid-stack ").hasClass('dirty')) {
        isFormChange = true;
    } else {
        isFormChange = false;
    }
    return isFormChange;
}

function SetAutoCompAccess(act, fld) {
    try {
        if (fld == undefined) {
            $j('input.fldFromSelect:disabled').parent().find("i").css("display", "none");
            //$j('input.fldAutocomplete:readonly').parent().find("i").css("display", "none");
        } else {
            if (act == "enabled")
                fld.parent().find("i").css("display", "inline-block");
            else
                fld.parent().find("i").css("display", "none");
        }
    } catch (ex) {
        console.log("SetAutoCompAccess-action" + act + "-msg" + ex.message);
    }

}
//to get the expression for INIT_CAP
function makeFieldInitCap(dcNo) {
    var depFldIndx = GetFldNamesIndx("axp_initcap");
    var depFldType = "";
    var initExpression = "";
    var initCapElems;
    if (depFldIndx != -1) {
        depFldType = GetExpressionType("axp_initcap", depFldIndx);
        initExpression = Expressions[depFldIndx].toString();
    }
    if (initExpression != "") {
        initExpression = initExpression.split(',');
        for (var i = 0; i < initExpression.length; i++) {
            if (dcNo) {
                if ($("#DivFrame" + dcNo).find('input[id*="' + initExpression[i] + '"]').length > 0)
                    $("#DivFrame" + dcNo).find('input[id*="' + initExpression[i] + '"]').addClass('initCapField');
            } else {
                if ($("input[id^='" + initExpression[i] + "']").length > 0)
                    $("input[id^='" + initExpression[i] + "']").addClass('initCapField');
            }
        }
        //dcNo ? initCapElems = $("#DivFrame" + dcNo + " .initCapField") : initCapElems = $(".initCapField");
        //initCapElems.blur(function (e) {
        //    var capitalizedString = $(this).val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
        //        return letter.toUpperCase();
        //    });
        //    $(this).val(capitalizedString);
        //    var fldId = $(this).attr("id");
        //    var dbRowNo = GetDbRowNo();
        //    UpdateFieldArray(fld, fldDbRowNo, fldValue, "parent", "");
        //});
    }



}

window.onbeforeunload = BeforeWindowClose;

function BeforeWindowClose() {

    if (typeof FNames != "undefined" && FNames.filter((fld) => fld.toLowerCase().startsWith("axp_weight_")).length > 0 && appGlobalVarsObject._CONSTANTS.isHybrid) {
        closeWeightScalePort();
    }

    if (draftTimer)
        window.clearInterval(draftTimer);
    if (window.parent.globalChange)
        SetFormDirty(false);
    if (window.opener && !window.opener.closed && window.opener.parent.tstructPop && AxActiveAction != "") {
        window.opener.parent.tstructPop = false;
        ReloadListView(window.opener.parent.listViewPage);
    }
    //RemoveTstDataObj();
    //this function is a dummy call to fire the window close event.
    var rid = $j("#recordid000F0").val();

    if (rid != "0" && AxIsTstructLocked == false && callParentNew("isLockOnRead")) {
        try {
            ASB.WebService.UnlockTStructRecord(tst, rid, false);
        } catch (ex) {

        }
    }
    if (typeof axpRefreshParent != "undefined" && axpRefreshParent == true) {
        if (window.opener.document.title = "Iview") {
            var param = window.opener.document.getElementById("hdnparamValues").value;
            var iName = window.opener.iName;
            var url = "ivtoivload.aspx?ivname=" + iName;
            var strParam = "";
            strParam = param.replace(/¿/g, "&");
            strParam = strParam.replace(/~/g, "=");
            url = url + "&" + strParam + "&axp_refresh=true";
            window.opener.document.getElementById("hdnIvRefresh").value = "true";
            window.opener.document.location.href = url;
            var browservalue = navigator.userAgent.toUpperCase();
            if (browservalue.indexOf("CHROME") > -1) {
                window.opener.document.getElementById("button1").click();
            }
        }
    }

    callParentNew("removeOverlayFromBody()", "function"); //if any remodal popup is opened & user clicks on browser back button then remove window - header, footer & menu overlay css
}

function CheckCustomTStSave() {
    var custActFld = $j("#axp_savedirective000F1");
    var custActFldVal = "";
    var tstIviewname = "";
    var tstParams = "";
    var ivParam = "";
    var tstRedirect = "";

    custActFldVal = custActFld.val();
    if (custActFldVal != undefined) {
        if (custActFldVal.indexOf("$") > -1) { // as a pop up
            custActFldVal = GetPopUpVal(custActFldVal);
        }
        if (custActFldVal.indexOf("~") > -1) {
            tstRedirect = custActFldVal.split('~');
            if (tstRedirect[1] != undefined) {
                tstParams = tstRedirect[1].split('*');
                if (tstRedirect[0] != undefined && tstRedirect[0] != "") {
                    if (tstRedirect.indexOf("iview") > -1) {
                        axCustomTstAction = "ivtoivload.aspx?ivname=" + tstParams[0];
                        if (tstParams[1] != undefined && tstParams[1] != "") {
                            axCustomTstAction += "&" + tstParams[1];
                        }
                    }
                    //tstruct~transid*paramname1=value1^paramname2=value2
                    else if (tstRedirect.indexOf("tstruct") > -1) {
                        axCustomTstAction = "tstruct.aspx?transid=" + tstParams[0] + `&openerIV=${typeof isListView != "undefined" ? iName : tstParams[0]}&isIV=${typeof isListView != "undefined" ? !isListView : "false"}&isDupTab=${callParentNew('isDuplicateTab')}`;
                        if (tstParams[1] != undefined && tstParams[1] != "") {
                            axCustomTstAction += "&" + tstParams[1];
                        }
                    }
                }
            }

        } else {
            axCustomTstAction = custActFld.val();
        }
    }
}

function GetPopUpVal(custActFldVal) {

    var dolrIndx = custActFldVal.indexOf("$");
    var sIndx = custActFldVal.indexOf("*");
    if (sIndx > -1)
        AxPopup = custActFldVal.substring(dolrIndx + 1, sIndx);
    else
        AxPopup = custActFldVal.substring(dolrIndx + 1);

    custActFldVal = custActFldVal.replace("$" + AxPopup, '');

    return custActFldVal;

}
//NOTE:Any modification in the below function should be checked with the AssignJQueryEvents function in tstruct.js
function LoadEvents(dvId) {
    $('[data-toggle="tooltip"]').tooltip();
    CKEDITOR.on('instanceReady', function (event) {
        event.editor.on('change', function () {
            currentCK = this.name;
        });
        isReadyCK = true;
        createCKAutoComplete(event);
        createCKAutoCompleteColon(event);
    });
    WizardKtInit();
    createEditors();

    /*createFormSelect(".fldFromSelect,.multiFldChk");*/
    createFormSelect(".fldFromSelect");
    createFormSelectMultiChecklist(".multiFldChk");
    createFormMultiSelect(".fldmultiSelect");

    var autoDiv = dvId == undefined ? "#divDc1" : dvId;
    if (theMode != "design") {
        DropzoneInit(dvId);
        DropzoneGridInit(dvId);
        HeaderAttachFiles();
    }
    KTApp.initBootstrapPopovers();
    try {
        KTApp?.initBootstrapTooltips();
    } catch (error) { }
    createCheckListTokens(dvId);

    try {
        $(".mainIframe input[type=text]:not(.number,.flatpicker-input)").each(function () {
            let elem = $(this);
            let _editorId = elem.attr("id") != undefined ? elem.attr("id") : "";
            if (_editorId != "") {
                createAxAutocomplete(_editorId);
                createAxAutocompleteColon(_editorId);
            }
        });
    } catch (ex) { }

    //function call for focus event of textarea, textbox, checkbox, checklist & radiogroup.
    $j("input:not([id=searstr],[class=AxAddRows],[class=AxSearchField],.flatpickr-input,.gridRowChk,.gridHdrChk),select:not([id=selectbox]),textarea:not(#txtCommentWF):not(.labelInp)").focus(function () {
        MainFocus($j(this));
    });

    var glType = eval(callParent('gllangType'));
    var dtpkrRTL = false;
    if (glType == "ar")
        dtpkrRTL = true;
    else
        dtpkrRTL = false;
    var glCulture = eval(callParent('glCulture'));
    var dtFormat = "d/m/Y";
    var tsFormat = "d/m/Y H:i:S";
    if (glCulture == "en-us") {
        dtFormat = "m/d/Y";
        tsFormat = "m/d/Y H:i:S";
    }

    $(".flatpickr-input:not(.tstOnlyTime,.tstOnlyTime24hours,.fldtimestamp)").parent(".input-group").flatpickr({
        dateFormat: dtFormat,
        disableMobile: "true",
        allowInput: true,
        wrap: true,
        onPreCalendarPosition: function (selectedDates, dateStr, instance) {
            let _thisfpVal = $(instance.element).find("input").val();
            if (typeof _thisfpVal == "undefined")
                _thisfpVal = new Date();
            if (_thisfpVal != "")
                instance.setDate(_thisfpVal);
        },
        onOpen: function (selectedDates, dateStr, instance) {
            MainFocus($(instance.element).find("input"));
            //setTimeout(() => { instance.calendarContainer.style.right = "0px"; }, 0);
        },
        onChange: function (selectedDates, dateStr, instance) {
            //if ($(".flatpickr-calendar:visible").length == 0 && $(instance.element).find("input").val() != AxOldValue) {
            //    MainBlur($(instance.element).find("input"));
            //}
        },
        onClose: function (selectedDates, dateStr, instance) {
            MainBlur($(instance.element).find("input"));
        }
    });

    $(".flatpickr-input.fldtimestamp").parent(".input-group").flatpickr({
        dateFormat: tsFormat,
        enableTime: true,
        time_24hr: true,
        enableSeconds: true, // optional
        disableMobile: "true",
        allowInput: false,
        wrap: true,
        defaultHour: new Date().getHours(),
        defaultMinute: new Date().getMinutes(),
        defaultSeconds: new Date().getSeconds(),
        onPreCalendarPosition: function (selectedDates, dateStr, instance) {
            let $input = $(instance.element).find("input");
            let inputVal = $input.val();

            if (inputVal && inputVal.trim() !== "") {
                instance.setDate(inputVal, false, tsFormat);
            } else {
                instance.clear();
            }
        },
        onOpen: function (selectedDates, dateStr, instance) {
            MainFocus($(instance.element).find("input"));
        },
        onClose: function (selectedDates, dateStr, instance) {
            MainBlur($(instance.element).find("input"));
        }
    });

    $(".tstOnlyTime").not('.editWrapTr .tstOnlyTime,table.table .tstOnlyTime').parent(".input-group").flatpickr({
        enableTime: true,
        noCalendar: true,
        dateFormat: "h:i K",
        disableMobile: "true",
        //allowInput: true,
        wrap: true,
        onPreCalendarPosition: function (selectedDates, dateStr, instance) {
            let _thisfpVal = $(instance.element).find("input").val();
            if (_thisfpVal != "")
                instance.setDate(_thisfpVal);
        },
        onOpen: function (selectedDates, dateStr, instance) {
            MainFocus($(instance.element).find("input"));
            //setTimeout(() => { instance.calendarContainer.style.right = "0px"; }, 0);           
        },
        onChange: function (selectedDates, dateStr, instance) {
            //if ($(instance.element).find("input").val() != AxOldValue) {
            //    MainBlur($(instance.element).find("input"));
            //}
        },
        onClose: function (selectedDates, dateStr, instance) {
            setTimeout(function () {
                MainBlur($(instance.element).find("input"));
            }, 0);
        }
    });

    $(".tstOnlyTime24hours").not('.editWrapTr .tstOnlyTime24hours,table.table .tstOnlyTime24hours').parent(".input-group").flatpickr({
        enableTime: true,
        noCalendar: true,
        dateFormat: "H:i",
        time_24hr: true,
        disableMobile: "true",
        //allowInput: true,
        wrap: true,
        onPreCalendarPosition: function (selectedDates, dateStr, instance) {
            let _thisfpVal = $(instance.element).find("input").val();
            if (_thisfpVal != "")
                instance.setDate(_thisfpVal);
        },
        onOpen: function (selectedDates, dateStr, instance) {
            MainFocus($(instance.element).find("input"));
            /*setTimeout(() => { instance.calendarContainer.style.right = "0px"; }, 0);            */
        },
        onChange: function (selectedDates, dateStr, instance) {
            //if ($(instance.element).find("input").val() != AxOldValue) {
            //    MainBlur($(instance.element).find("input"));
            //}
        },
        onClose: function (selectedDates, dateStr, instance) {
            setTimeout(function () {
                MainBlur($(instance.element).find("input"));
            }, 0);
        }
    });

    $(document).off("mousedown.designer").on("mousedown.designer", ".tstructDesignMode .grid-stack-item, .tstructDesignMode .ui-resizable", function () {
        setSelectedDesignElement(this);
    });


    $(document).off("dragstop.designer").on("dragstop.designer", ".tstructDesignMode.grid-stack", function (event, ui) {
        selectedDesignObject.elem = event.target;
    });

    $(document).off("change.designer").on("change.designer", ".tstructDesignMode.grid-stack", function (event, ui) {
        setSelectedDesignElement(selectedDesignObject.elem);
    });

    $(document).off("dragstart.designer resizestart.designer").on("dragstart.designer resizestart.designer", ".tstructDesignMode.grid-stack", function (event, ui) {
        $(this).addClass('dirty');
        changeStatus("notSaved");
    });

    $(document).off("gsresizestop.designer").on("gsresizestop.designer", ".tstructDesignMode.grid-stack", function (event, ui) {
        var newHeight = $(ui).attr('data-gs-height');
        var gsiPX = gsiPixels(newHeight);
        // gsiHeight = { "height": gsiPX.toString() + "px" };
        gsiHeight = {
            "height": (isMobile ? gsiPX + 10 : gsiPX).toString() + "px"
        };
        if (typeof CKEDITOR.instances[$(ui).find("textarea").attr("id")] != "undefined") {
            //var ckHeight = CKEDITOR.instances[$(currentElm).attr("id")].resize( '100%', gsiHeight.height, true );
            try {
                var _this = CKEDITOR.instances[$(ui).find("textarea").attr("id")];
                if (!staticRunMode) {
                    _this.resize('100%', gsiPX);
                } else {
                    _this.resize('100%', ($(_this.ui.contentsElement.$).parents(".grid-stack-item-content").height() - $(_this.ui.contentsElement.$).parents(".grid-stack-item-content").children(".fld-wrap3").outerHeight(true) - 2) - $(_this.ui.contentsElement.$).siblings().toArray().reduce((total, elm) => {
                        return total = total + $(elm).outerHeight(true);
                    }, 0), true);
                }
            } catch (error) { }
        }
    });

    //TimePickerEvent(dvId, dtpkrRTL);
    //function call on blur event of textarea, textbox.
    $j("textarea:not(#comment):not(.labelInp,.select2-search__field),[id]:text:not([id=searstr],[class=AxAddRows],[class=AxSearchField],.gridHdrChk,.tstOnlyTime,.tstOnlyTime24hours,.gridHeaderSwitch,.dvgrdchkboxnonedit,.flatpickr-input,.ckbGridStretchSwitch),[id][type=number]:not([id=searstr],[class=AxAddRows],[class=AxSearchField]),:password").blur(function (event) {
        if (theMode != "design")
            MainBlur($j(this));
    });

    $j(document).mousedown(function (e) {
        if (e.target.id != "") {
            blurNextPreventElement = new Object();
            blurNextPreventId = e.target.id;
        } else {
            blurNextPreventId = "";
            blurNextPreventElement = e.target;
        }
    });

    //function call on change event of dropdown.
    $j("select:not(#ddlSearch,#selectbox,.fldFromSelect,.fldmultiSelect,.multiFldChk,.multiFldChklist):not(#designLayoutSelector)").change(function () {
        if ($j(this).find(':selected').text().indexOf("+ Add") == -1) {
            MainBlur($j(this));
            //$j(this).blur();
            $j(this).focus();
        } else {
            if ($j(this).val() != null || $j(this).val() != undefined) {
                var ddlrefval = $j(this).val();
                $j(this).val("");
                eval(ddlrefval);
            }
        }
    });

    //function call on blur event of checkbox, checklist & radiogroup.
    $j(":checkbox:not([class=chkAllList],.gridHdrChk,.gridRowChk,.gridHeaderSwitch,.dvgrdchkboxnonedit,.ckbGridStretchSwitch):not(.tokenSelectAll):not(#ckbCompressedMode):not(#ckbStaticRunMode):not(#ckbWizardDC):not('[id^=ckbGridStretch]'),:radio").not(".chkShwSel").change(function () {
        MainBlur($j(this));
    });

    //function call on keydown event in a textarea.
    $j("textarea:not(.labelInp)").keydown(function (event) {
        LimitText($j(this));
    });

    //function call on keyup event in a textarea.
    $j("textarea:not(.labelInp)").keyup(function () {
        LimitText($j(this));
    });

    //function call on keypress event in a numeric field.
    $j(".number").keypress(function (event) {
        return CheckNumeric(event, $j(this).val());
    });


    $j("#searchoverrelay").unbind("keypress").keypress(function (e) {
        if (e.keyCode == 13) { // detect the enter key
            if (!valid_submit())
                return false;
        }
    });

    //function on click of image field
    $j(".axpImg").click(function () {
        if (tstructCancelled != "Cancelled") {
            var imgFld = $j(this);
            var fldName = imgFld[0].id;
            var onclickevent = document.getElementById(fldName).onclick;
            if (onclickevent == null)
                UploadImg(fldName);
        }
    });

    //function on click of signature field
    $j(".signaturePad").click(function (e) {
        if ($(e.target).attr("onclick") == "ClearImageSrc(this);") {
            return;
        }
        if (tstructCancelled != "Cancelled") {
            var imgFld = $j(this).find(".signatureInput");
            var fldName = imgFld[0].id;
            var onclickevent = document.getElementById(fldName).onclick;
            if (onclickevent == null)
                openSignaturePad(fldName);
        }
    });

    // function on click of Bar/QR code field
    $j(".divBarQrScan").click(function () {
        if (tstructCancelled != "Cancelled") {
            var scanFld = $j(this);
            var fldName = scanFld.parent().find("input")[0].id;
            var onclickevent = document.getElementById(fldName).onclick;
            if (onclickevent == null && !(IsGridField(GetFieldsName(fldName)) && isMobile && AxpGridForm == "form")) {
                openBarQrScanner(fldName);
            }
        }
    });

    //function on click of action buttons
    $(".axpBtn").off("click.axpBtn").on("click.axpBtn", function () {
        var obj = $j(this);
        if (obj.length > 0) {
            var rowNo = GetFieldsRowNo(obj[0].id);
            var dcNo = GetFieldsDcNo(obj[0].id);
            RegisterActiveRow(rowNo, dcNo);
            CallAction(obj[0].id);
        }
    });

    $j(".rowdelete").click(function () {
        DeleteCurrentRow($j(this));
    });

    $j(".subGrid").click(function () {

        ShowPopUp($j(this));
    });

    $j("#taskListPopUp").hide();

    $j('.AxAddRows').blur(function () {
        if (!this.value || isNaN(this.value))
            return this.value = "1";
    });

    $j(".achklist").click(function () {
        $j(this).parent().next('.chkListBdr').toggle();
    });


    $j(".spandate").click(function () {
        $j(this).prev().focus();
    });

    $j(".spanTime").click(function () {
        $j(this).prev().focus();
    });

    $(document).on("keypress", dvId + " .form-group span.fa-paperclip", function (e) {
        if (e.which == "13") {
            $(this).click();
        }
    });
    $j(".rowdelete img").hover(function () {
        $j(this).attr("src", "../axpimages/icons/16x16/delete.png");
    }, function () {
        $j(this).attr("src", "../axpimages/icons/16x16/delete-fade.png");
    });

    if (parent.MainNewEdit == true) {
        $j('html').addClass("makeFullHeight");
    }

    try {
        if ($j("#popupIframeRemodal", parent.document).attr("src") != undefined) {
            $j(document).keyup(function (e) {
                if (e.keyCode === 27) {
                    if (IsFormDirty && $('#bootstrapModal').length === 0) {
                        $.confirm({
                            theme: 'modern',
                            closeIcon: false,
                            title: eval(callParent('lcm[155]')), //lcm[121]
                            content: eval(callParent('lcm[121]')),
                            escapeKey: 'buttonB',
                            onContentReady: function () {
                                disableBackDrop('bind');
                            },
                            buttons: {
                                buttonA: {
                                    text: eval(callParent('lcm[164]')),
                                    btnClass: 'btn btn-primary',
                                    action: function () {
                                        $j('.remodal-close', parent.document).click();
                                    }
                                },
                                buttonB: {
                                    text: eval(callParent('lcm[192]')),
                                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                                    action: function () {
                                        disableBackDrop('destroy');
                                        return true;
                                    },

                                }
                            }
                        });
                    } else if ($('#bootstrapModal').length === 1) {
                        // do nothing;
                    } else {
                        $j('.remodal-close', parent.document).click();
                    }

                }
            });
        }
    } catch (ex) {
        console.log(ex.message);
    }


    $ == undefined ? $ = $j : "";
    jQuery.browser = {};
    (function () {
        jQuery.browser.msie = false;
        jQuery.browser.version = 0;
        if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
            jQuery.browser.msie = true;
            jQuery.browser.version = RegExp.$1;
        }
    })();


    if ($j(".clsPrps").length > 0) {
        $j("#tgPurpose").css("display", "inline-block");
    } else {
        $j("#tgPurpose").css("display", "none");
    }

    //function call on focusin event for masked field value to actual value.
    $j("input.form-control,textarea.memofam").on('focusin', function (e) {
        try {
            var fldId = $j(this).attr("id");
            if (typeof fldId != "undefined" && fldId != "") {
                var fName = GetFieldsName(fldId);
                var fldIndex = $j.inArray(fName, FNames);
                if (fldIndex != undefined && fldIndex > -1) {
                    if (typeof FldMaskType != "undefined" || typeof ScriptMaskFields != "undefined") {
                        let maskType = "";
                        if (ScriptMaskFields.length > 0) {
                            var idx = $j.inArray(fName, ScriptMaskFields);
                            if (idx != -1)
                                maskType = ScriptMaskFields[idx];
                            else
                                maskType = FldMaskType[fldIndex];
                        } else
                            maskType = FldMaskType[fldIndex];
                        if (maskType != "") {
                            let newFldMaskValue = GetFieldValue(fldId);
                            if (newFldMaskValue != "" && newFldMaskValue != '0.00' && newFldMaskValue != '0') {
                                $j(this).val(newFldMaskValue);
                                $j(this).attr("value", newFldMaskValue);
                            }
                        }
                    }
                }
            }
        } catch (ex) { }
    });

    $j("input.number").on('input', function () {
        var fldVal = $j(this).val();
        var fldId = $j(this).attr("id");
        if (fldId != undefined) {
            var fName = GetFieldsName(fldId);
            var fldIndex = $j.inArray(fName, FNames);
            if (fldIndex != undefined && fldIndex > -1) {
                var maxFldLength = FMaxLength[fldIndex];
                var decimalLength = 0;
                if (FCustDecimal[fldIndex] == "True" && typeof gloAxDecimal != "undefined" && gloAxDecimal > -1)
                    decimalLength = gloAxDecimal;
                else
                    decimalLength = FDecimal[fldIndex];
                var fldType = FDataType[fldIndex];
                if (fldType == "Numeric" && decimalLength != undefined && decimalLength > 0) {
                    var intPartMaxLimit = maxFldLength - decimalLength - 1; //Integer Part Max Limit.
                    if (fldVal.indexOf('.') == -1) {
                        if (fldVal.length > intPartMaxLimit) {
                            $j(this).val(fldVal.slice(0, intPartMaxLimit));
                            UpdateAllFieldValues($j(this).attr("id"), fldVal.slice(0, intPartMaxLimit));
                        }
                    } else if (fldVal.indexOf('.') != -1) {
                        var intPart = fldVal.substring(0, fldVal.indexOf('.'));
                        var decPart = fldVal.substring(fldVal.indexOf('.') + 1, fldVal.length);
                        if (intPart.length > intPartMaxLimit) intPart = intPart.slice(0, intPartMaxLimit);
                        if (decPart.length > decimalLength) decPart = decPart.slice(0, decimalLength);
                        $j(this).val(intPart + "." + decPart);
                        UpdateAllFieldValues($j(this).attr("id"), intPart + "." + decPart);
                    }
                }
            }
        }
    });

    // $j(".workflowOptions").click(function () {
    //     if ($("#workflowdropdown").length != 0)
    //     $("#workflowdropdown").append($("#selectbox").detach());
    //          $("#workflowdropdown").append($(".dropbox").detach());
    //     if ($(".wfselectbox").hasClass("d-none")) {
    //         $(".wfselectbox").removeClass("d-none");
    //     } else
    //         $(".wfselectbox").addClass("d-none");
    // });

    $(document).off("click", ".weightScaleIcon").on("click", ".weightScaleIcon", function () {
        if (appGlobalVarsObject._CONSTANTS.isHybrid) {
            /* Clear older weight scale value in redis if exists with current loggedin guid.*/
            let oldWeightScaleVal = getRedisString("HybridWeightScaleInfo", callParentNew("hybridGUID"));

            $(this).parent(".weightScale").parent().children("input").focus();

            try {
                ShowDimmer(true);
                ASB.WebService.NotifyHybridForWeightScale(callParentNew("hybridGUID"),
                    (success) => {
                        if (success == "success") {
                            getHybridWeightScaleInfo($(this).parent(".weightScale").parent().children("input").attr("id"));
                        } else {
                            ShowDimmer(false);
                            showAlertDialog("error", "Error occurred while fetching weight. Please try again.");
                        }
                    },
                    (error) => {
                        ShowDimmer(false);
                        showAlertDialog("error", "Error occurred while fetching weight. Please try again.");
                    }
                );
            } catch (error) {
                ShowDimmer(false);
                showAlertDialog("error", "Exception occurred while fetching weight.");
            }
        }
        else {
            showAlertDialog("error", "Please close the application and login again.");
        }
    });

    $(document).off("change", '.tstfldImage').on("change", '.tstfldImage', function (e) {
        TstFldImageUpload(e.target);
    });

    $(document).off("click", ".imageFileUpload,img.profile-pic").on("click", ".imageFileUpload,img.profile-pic", function (e) {
        $(e.currentTarget).siblings("label.upload-button").find(".tstfldImage").click()
    });

    $(".fldImageCamera").off("click").on("click", function (e) {
        if (isMobile) {
            try {
                $(e.currentTarget).siblings("label.upload-button").find(".tstfldImage").attr("capture", "")
                setTimeout(() => {
                    $(e.currentTarget).siblings("label.upload-button").find(".tstfldImage").click();
                    $(e.currentTarget).siblings("label.upload-button").find(".tstfldImage").removeAttr("capture");

                }, 0);
            } catch (error) { }
        } else {
            let attrId = $(".fldImageCamera").parent(".image-input").find("input").attr("id");
            UploadCaptureImage(attrId);
        }
    });

    if (typeof AxpCameraOption == "undefined" || (typeof AxpCameraOption != "undefined" && (AxpCameraOption == "" || AxpCameraOption != "true"))) {
        //$(".fldImageCamera").removeClass("d-none");
        $(".fldImageCamera:not(.disabledFldFrmControl)").removeClass("d-none");
    }

    $("#wBdr").on("mouseenter mouseleave", ".customSetupTableMN textarea", function (e) {
        $(this).attr('title', $(this).val());
    });
    tstSearchOnChange();
}



function WizardKtInit() {
    if (typeof isWizardTstruct != "undefined" && isWizardTstruct) {
        // Stepper lement
        var element = document.querySelector("#wbdrHtml");

        // Initialize Stepper
        var stepper = new KTStepper(element);

        // Handle navigation click
        stepper.on("kt.stepper.click", function (stepper) {
            let wCurrDc = $(stepper.element).find(".flex-column.current").attr("id");
            let wcDcNo = wCurrDc.slice(8);
            if (wcDcNo < stepper.getClickedStepIndex()) {
                if (!ValidateBeforeSubmit(wcDcNo))
                    return false;
                else {
                    stepper.goTo(stepper.getClickedStepIndex()); // go to clicked step
                    wdcAutoShowFillgrid();
                }
            } else {
                stepper.goTo(stepper.getClickedStepIndex()); // go to clicked step
                wdcAutoShowFillgrid();
            }
        });

        // Handle next step
        stepper.on("kt.stepper.next", function (stepper) {
            let wCurrDc = $(stepper.element).find(".flex-column.current").attr("id");
            let wcDcNo = wCurrDc.slice(8);
            let allDcs = $(stepper.element).find(".flex-column.current").nextAll(".flex-column");
            if (wizardHidenDcNos.length > 0) {
                let isNexyClk = false;
                allDcs.each(function () {
                    let wdcId = $(this).attr("id");
                    wdcId = parseInt(wdcId.slice(8));
                    if (wizardHidenDcNos.includes(wdcId)) {
                        stepper.goNext()
                    } else {
                        isNexyClk = true;
                        return false;
                    }
                });
                if (isNexyClk) {
                    if (!ValidateBeforeSubmit(wcDcNo))
                        return false;
                    else {
                        //return stepper.goNext(); // go next step
                        let _thisStatus = stepper.goNext();
                        wdcAutoShowFillgrid();
                        return _thisStatus;
                    }
                }
            } else {
                if (!ValidateBeforeSubmit(wcDcNo))
                    return false;
                else {
                    /*return stepper.goNext(); // go next step*/
                    let _thisStatus = stepper.goNext();
                    wdcAutoShowFillgrid();
                    return _thisStatus;
                }
            }
        });

        // Handle previous step
        stepper.on("kt.stepper.previous", function (stepper) {
            let wCurrDc = $(stepper.element).find(".flex-column.current").attr("id");
            let wcDcNo = wCurrDc.slice(8);
            let allDcs = $(stepper.element).find(".flex-column.current").prevAll(".flex-column");
            if (wizardHidenDcNos.length > 0) {
                let isPrevClk = false;
                allDcs.each(function () {
                    let wdcId = $(this).attr("id");
                    wdcId = parseInt(wdcId.slice(8));
                    if (wizardHidenDcNos.includes(wdcId)) {
                        stepper.goPrevious(); // go previous step
                    } else {
                        isPrevClk = true;
                        return false;
                    }
                });
                if (isPrevClk) {
                    stepper.goPrevious(); // go previous step
                }
            } else
                stepper.goPrevious(); // go previous step
        });
    }
}

function wdcAutoShowFillgrid() {
    setTimeout(function () {
        setTimeout(function () {
            let _wdcId = $("[id^='wizardDc'].current").attr("id");
            _wdcId = _wdcId.replace('wizardDc', '');
            if ($("#fillgrid" + _wdcId).length > 0 && !$("#fillgrid" + _wdcId).hasClass('wdcFgClicked')) {
                let _thisFgName = $("#fillgrid" + _wdcId).attr("name");
                let _fgind = $j.inArray(_thisFgName, FillGridName);
                if (typeof _fgind != "undefined" && _fgind > -1 && FillAutoShow[_fgind] == 'true') {
                    $("#fillgrid" + _wdcId).addClass('wdcFgClicked');
                    $("#fillgrid" + _wdcId).click();
                }
            }
        }, 0);
    }, 100);
}

function createCheckListTokens(dvId) {
    $((dvId ? "#" + dvId : "") + " .form-select.multiFldChklist:not(span)").each(function () {
        var thisValueList = $(this).data("valuelist");
        var source = typeof thisValueList == "object" ? thisValueList : [...new Set(
            $(this).data("valuelist").split($(this).data("separator"))
        )];

        if (source != "") {
            var result = ($.map(source, function (item) {
                return {
                    id: item,
                    text: item
                }
            }))
            $(this).select2({
                data: result
            }).on("select2:unselect select2:select", function (e) {
                let fldNamesf = $(this).attr("id");
                let fldAcValue = $(this).val();
                if (typeof $(this).data("separator") != "undefined") {
                    let separator = $(this).data("separator");
                    fldAcValue = fldAcValue.join(separator);
                }
                isGrdEditDirty = true;
                var acFrNo = GetFieldsDcNo(fldNamesf);
                var rowNum = GetDbRowNo(GetFieldsRowNo(fldNamesf), acFrNo);
                var fldRowNo = GetFieldsRowNo(fldNamesf);
                if (IsDcGrid(acFrNo) && isGrdEditDirty)
                    UpdateFieldArray(axpIsRowValid + acFrNo + fldRowNo + "F" + acFrNo, GetDbRowNo(fldRowNo, acFrNo), "", "parent", "AddRow");
                UpdateFieldArray(fldNamesf, rowNum, fldAcValue, "parent", "AutoComplete");
                UpdateAllFieldValues(fldNamesf, fldAcValue);
                setTimeout(function () {
                    if (fldAcValue == "")
                        AxOldValue = " ";
                    else
                        AxOldValue = "";
                    MainBlur($j("#" + fldNamesf));
                }, 0);
            }).on('select2:open', (selectEv) => {
                var curDropdown = $(selectEv.currentTarget).data("select2")?.$dropdown;
                var selectedOptionCount = 0;
                if (result.length != 0) {
                    let _fName = GetFieldsName($(selectEv.currentTarget).attr("id"));
                    if (typeof AxHideSelectAll != "undefined") {
                        let _axhideselall = AxHideSelectAll.split('♦');
                        if (_axhideselall[0].toLowerCase() == 'false') {
                            if (curDropdown.find(".select2-results").find(".msSelectAllOption").length == 0) {
                                var selectAllHTML = `<div class="msSelectAllOption form-check form-check-custom align-self-end px-5">
                    <input type="checkbox" class="form-check-input msSelectAll" onchange="checkAllCheckBoxTokens(this, '${$(selectEv.currentTarget).attr("id")}')"/>
                    <label for="SelectAll" class="ps-2 form-check-label form-label col-form-label pb-1 fw-boldest">
                        Select All
                    </label>
                </div>`;
                                curDropdown.find(".select2-results").append(selectAllHTML);
                            }
                        } else if (_axhideselall.length > 1 && _axhideselall[0].toLowerCase() == 'false' && _axhideselall[1] == "") {
                            if (curDropdown.find(".select2-results").find(".msSelectAllOption").length == 0) {
                                var selectAllHTML = `<div class="msSelectAllOption form-check form-check-custom align-self-end px-5">
                    <input type="checkbox" class="form-check-input msSelectAll" onchange="checkAllCheckBoxTokens(this, '${$(selectEv.currentTarget).attr("id")}')"/>
                    <label for="SelectAll" class="ps-2 form-check-label form-label col-form-label pb-1 fw-boldest">
                        Select All
                    </label>
                </div>`;
                                curDropdown.find(".select2-results").append(selectAllHTML);
                            }
                        } else if (_axhideselall.length > 1 && (_axhideselall[0].toLowerCase() != 'true' || (_axhideselall[0].toLowerCase() == 'true' && _axhideselall[1] != "" && _axhideselall[1] != _fName))) {
                            if (curDropdown.find(".select2-results").find(".msSelectAllOption").length == 0) {
                                var selectAllHTML = `<div class="msSelectAllOption form-check form-check-custom align-self-end px-5">
                    <input type="checkbox" class="form-check-input msSelectAll" onchange="checkAllCheckBoxTokens(this, '${$(selectEv.currentTarget).attr("id")}')"/>
                    <label for="SelectAll" class="ps-2 form-check-label form-label col-form-label pb-1 fw-boldest">
                        Select All
                    </label>
                </div>`;
                                curDropdown.find(".select2-results").append(selectAllHTML);
                            }
                        }
                    } else {
                        if (curDropdown.find(".select2-results").find(".msSelectAllOption").length == 0) {
                            var selectAllHTML = `<div class="msSelectAllOption form-check form-check-custom align-self-end px-5">
                    <input type="checkbox" class="form-check-input msSelectAll" onchange="checkAllCheckBoxTokens(this, '${$(selectEv.currentTarget).attr("id")}')"/>
                    <label for="SelectAll" class="ps-2 form-check-label form-label col-form-label pb-1 fw-boldest">
                        Select All
                    </label>
                </div>`;
                            curDropdown.find(".select2-results").append(selectAllHTML);
                        }
                    }

                    curDropdown.find(".select2-results").find(".select2-results__options li:not(.select2-results__option--disabled.loading-results)").each((ind, elm) => {
                        if ($(elm).hasClass("select2-results__option--selected")) {
                            selectedOptionCount++;
                        }
                    });

                    if (selectedOptionCount == 0 && $(selectEv.currentTarget).val().length > 0)
                        selectedOptionCount = $(selectEv.currentTarget).val().length;

                    if (curDropdown.find(".select2-results").find(".select2-results__options li:not(.select2-results__option--disabled.loading-results)").length == selectedOptionCount && selectedOptionCount > 0) {
                        curDropdown.find(".select2-results").find(".msSelectAllOption > .msSelectAll").prop("checked", true);
                    }
                    else if (curDropdown.find(".select2-results").find(".select2-results__options li:not(.select2-results__option--disabled.loading-results)").length != selectedOptionCount && curDropdown.find(".select2-results").find(".msSelectAllOption > .msSelectAll").is(":checked")) {
                        curDropdown.find(".select2-results").find(".msSelectAllOption > .msSelectAll").prop("checked", false);
                    }
                }
            });

            $(this).parent().find(".select2-selection__rendered").addClass("text-wrap text-break p-0");
        }
    });
}

function checkAllCheckBoxTokens(elem, elemId) {
    if ($(`#${elemId}`).hasClass('multiFldChk')) {
        if ($(elem).is(":checked")) {
            $(`#${elemId}`).find('option').remove();
            $(".select2-results ul li").each(function () {
                if ($(this).text() != 'Searching…')
                    $(`#${elemId}`).append('<option value="' + $(this).text() + '" selected="selected">' + $(this).text() + '</option>');
            });

            let fldAcValue = $(`#${elemId}`).val();
            if (typeof $(`#${elemId}`).data("separator") != "undefined" && fldAcValue.length > 1) {
                let separator = $(`#${elemId}`).data("separator");
                fldAcValue = fldAcValue.join(separator);
            }
            isGrdEditDirty = true;
            var acFrNo = GetFieldsDcNo(elemId);
            var rowNum = GetDbRowNo(GetFieldsRowNo(elemId), acFrNo);
            var fldRowNo = GetFieldsRowNo(elemId);
            if (IsDcGrid(acFrNo) && isGrdEditDirty)
                UpdateFieldArray(axpIsRowValid + acFrNo + fldRowNo + "F" + acFrNo, GetDbRowNo(fldRowNo, acFrNo), "", "parent", "AddRow");
            UpdateFieldArray(elemId, rowNum, fldAcValue, "parent", "AutoComplete");
            UpdateAllFieldValues(elemId, fldAcValue);
            setTimeout(function () {
                MainBlur($j("#" + elemId));
            }, 0);
        } else {
            $(`#${elemId}`).find('option').remove();
            $(`#${elemId}`).val('');
            var acFrNo = GetFieldsDcNo(elemId);
            var rowNum = GetDbRowNo(GetFieldsRowNo(elemId), acFrNo);
            UpdateFieldArray(elemId, rowNum, "", "parent", "AutoComplete");
            UpdateAllFieldValues(elemId, "");
            AxOldValue = " ";
            setTimeout(function () {
                MainBlur($j("#" + elemId));
            }, 0);
        }
        $(`#${elemId}`).trigger("change").select2("close");
    } else {
        let _chdata = $(`#${elemId}`).data('valuelist');
        let _separator = $(`#${elemId}`).data("separator");
        let _thisSearchVal = $(`#${elemId}`).parent().find('.select2-search__field').val();
        if ($(elem).is(":checked")) {
            $(`#${elemId}`).find('option').remove();
            _chdata.split(_separator).forEach(function (item) {
                if (_thisSearchVal == "")
                    $(`#${elemId}`).append('<option value="' + item + '" selected="selected">' + item + '</option>');
                else if (_thisSearchVal != "" && item.toLowerCase().indexOf(_thisSearchVal.toLowerCase()) > -1)
                    $(`#${elemId}`).append('<option value="' + item + '" selected="selected">' + item + '</option>');
                else
                    $(`#${elemId}`).append('<option value="' + item + '">' + item + '</option>');
            });
        } else {
            $(`#${elemId}`).find('option').remove();
            _chdata.split(_separator).forEach(function (item) {
                $(`#${elemId}`).append('<option value="' + item + '">' + item + '</option>');
            });
            $(elem).prop("checked", false);
        }

        let fldAcValue = $(`#${elemId}`).val();
        if (typeof $(`#${elemId}`).data("separator") != "undefined") {
            let separator = $(`#${elemId}`).data("separator");
            fldAcValue = fldAcValue.join(separator);
        }
        isGrdEditDirty = true;
        var acFrNo = GetFieldsDcNo(elemId);
        var rowNum = GetDbRowNo(GetFieldsRowNo(elemId), acFrNo);
        var fldRowNo = GetFieldsRowNo(elemId);
        if (IsDcGrid(acFrNo) && isGrdEditDirty)
            UpdateFieldArray(axpIsRowValid + acFrNo + fldRowNo + "F" + acFrNo, GetDbRowNo(fldRowNo, acFrNo), "", "parent", "AddRow");
        UpdateFieldArray(elemId, rowNum, fldAcValue, "parent", "AutoComplete");
        UpdateAllFieldValues(elemId, fldAcValue);
        setTimeout(function () {
            if (fldAcValue == "")
                AxOldValue = " ";
            else
                AxOldValue = "";
            MainBlur($j("#" + elemId));
        }, 0);

        $(`#${elemId}`).trigger("change").select2("close");
    }
}

function ShowPopUp(imgObj) {
    var imgId = imgObj.attr("id");
    var parFld = imgObj.parent().find("input");
    if (parFld.length == 0)
        parFld = imgObj.parent().find("select");
    var parFldId = parFld.attr("id");
    OpenPopUp(parFldId, imgId);
}

//Function to focus the first element in the form, focus Tab dc and first field in the Tab dc.
//Parameter - Dc Div id.
function FocusOnFirstField(dcno) {

    if (dcno == "form") {
        var visibleDCs = $j('[id*="divDc"]');
        for (var i = 0; i < visibleDCs.length; i++) {
            if ($j(visibleDCs[i]).is(':visible')) {
                dcno = visibleDCs[i].id.substring(visibleDCs[i].id.indexOf('divDc') + 5);
                break;
            }
        }
    }
    var objId = "";
    if (IsDcGrid(dcno)) {
        objId = "#wrapperForEditFields" + dcno;
    } else {
        objId = "#divDc" + dcno;
    }
    if ($j(objId).length > 0) {
        try {
            if ($j.inArray("1", TabDCs) == -1 && !isMobile) {
                $j(objId).focus();
            } else if (isMobile && $("#wizardBodyContent").length) {
                $("#wizardBodyContent").scrollTop(0);
            }
        } catch (ex) { }
        var focusObj = getFirstFocusElement(objId);
        //if (!focusObj.hasClass("date")) This condition is removed due to the issue : AGI003000
        if (focusObj != undefined && focusObj.length > 0 && !isMobile) {
            if (recordid != "0" && (focusObj.hasClass("fldFromSelect") || focusObj.hasClass("flatpickr-input"))) { } else {
                setTimeout(function () {
                    setTimeout(function () {
                        if (focusObj.hasClass("flatpickr-input")) {
                            focusObj.focus();
                            if ($(focusObj).val() != '')
                                $(focusObj).siblings("span").find('.material-icons').click();
                        }
                        else
                            focusObj.focus();
                        let _thisId = focusObj.attr('id');
                        if (typeof _thisId != "undefined" && _thisId != "") {
                            let _fldName = GetFieldsName(_thisId);
                            let _fldInd = GetFieldIndex(_fldName);
                            let _fldAEmpty = GetFieldProp(_fldInd, 'allowEmpty');
                            let _thidVal = GetFieldValueNew(_thisId);
                            if (_fldAEmpty == "F" && _thidVal == "") {
                                $(`#${_thisId}`).siblings('select:enabled').select2('open');
                            }
                        }
                        return focusObj;
                    }, 200);
                }, 0);
            }
        } else if (isMobile && $("#wizardBodyContent").length) {
            $("#wizardBodyContent").scrollTop(0);
        } else if (isMobile && axInlineGridEdit) {
            focusObj.focus();
            return focusObj;
        }
    }
}

function SetFocusAfterSaveOnLoad() {
    let sfasId = focusAfterSaveOnLoad;
    focusAfterSaveOnLoad = "";
    let sfasName = GetFieldsName(sfasId);
    let sfasDcNo = GetFieldsDcNo(sfasId);
    if (!IsDcGrid(sfasDcNo)) {
        if ($("#ank" + sfasDcNo).length > 0 && !$("#ank" + sfasDcNo).parent().hasClass("active")) {
            $("#ank" + sfasDcNo).click();
            setTimeout(function () {
                $j("#" + sfasId).focus();
            }, 210);
        } else
            $j("#" + sfasId).focus();
    } else {
        let sfasRowNo = sfasId.substring(sfasId.lastIndexOf("F") - 3, sfasId.lastIndexOf("F"));
        if ($("#ank" + sfasDcNo).length > 0 && !$("#ank" + sfasDcNo).parent().hasClass("active")) {
            $("#ank" + sfasDcNo).click();
            setTimeout(function () {
                $("#gridHd" + sfasDcNo + " tbody tr[id=sp" + sfasDcNo + "R" + sfasRowNo + "F" + sfasDcNo + "]").find(".glyphicon-pencil").parent().click();
                setTimeout(function () {
                    $j("#" + sfasId).focus();
                }, 50);
            }, 300);
        } else if ($("#ank" + sfasDcNo).length > 0 && $("#ank" + sfasDcNo).parent().hasClass("active")) {
            $("#gridHd" + sfasDcNo + " tbody tr[id=sp" + sfasDcNo + "R" + sfasRowNo + "F" + sfasDcNo + "]").find(".glyphicon-pencil").parent().click();
            setTimeout(function () {
                $j("#" + sfasId).focus();
            }, 50);
        }
    }
}


function DisplaySearchDlg() {

    $j(window).keydown(function (e) {
        OpenSearchPop(e);
    });

    if (window.parent) {
        $j(window.parent).keydown(function (e) {
            OpenSearchPop(e);
        });
    }
};

function LockTstruct() {
    Readonlyform();
    $j("#icons").find('*').attr('disabled', true);
    $j("#icons").find('*').prop('disabled', true);
    if ($j(".search").length > 0)
        $j(".search").removeProp("disabled");
    if ($j(".add").length > 0)
        $j(".add").find('*').removeProp("disabled");
    if ($j(".pdf").length > 0)
        $j(".pdf").find('*').removeProp("disabled");
    if ($j(".listview").length > 0)
        $j(".listview").find('*').removeProp("disabled");
}



function loadingNew() {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
}

function EndRequestHandler(sender, args) {
    if (args.get_error() == undefined) {
        if ($j("#goval").length > 0) {
            var gov = $j("#goval");
            if (gov.val() == "go") {
                FillDiv("Show");
                if (!$("#grdSearchRes").parent().hasClass('d-inline-flex'))
                    $("#grdSearchRes").parent().addClass('d-inline-flex-- w-100 overflow-auto');
                $j("#searstr").val(DecodeUrlSplChars($j("#hdnSearchStr").val()));
                gov.val("d");
            }
        }
        ShowDimmer(false);

        var ExportToHtml = $j("#hdnHtml");
        var fileName = $j("#hdnFilename");
        if (ExportToHtml.val() == "Save") {
            showAlertDialog("success", 2028, "client", fileName.val());
            ExportToHtml.val("");
        }
    } else {
        showAlertDialog("error", 2029, "client", args.get_error().message);
    }
}

function displaysearchDiv(divId, title) {

    // $j("#" + divId).show();
    // $j("#" + divId).addClass('Pagebody Bordercolor');
    $j("#searchoverrelay").parent().removeClass("d-none");
    // $j("#searchoverrelay").css("display", "block");

    Resizewindow();
}

function FillDiv(state) {

    if (state == "Show") {
        $("#srchcontent").removeClass("d-none");

        if (isMobile) {
            $(".tst-search").addClass("flex-column");
        }
    }
    else
        $("#srchcontent").addClass("d-none");
    Resizewindow();
}

function Pagination() {

    //var gov = $j("#goval");
    //gov.val("go");
    let _pageNo = $("#lvPage").val();
    GetTstSearchData(_pageNo);
    ShowDimmer(true);
}

function Closediv() {
    $j("#Panel1").hide();
    $j("#srchcontent").addClass("d-none");
    $j("#searchoverrelay").parent().addClass("d-none");
    try {
        $("[id^='dvFilterCondition']").not("#dvFilterCondition").remove();
    } catch (ex) { }
}

function hiddenFloatingDiv(divId) {

    document.getElementById(divId).style.display = "none";
    document.getElementById('dimmer').style.display = 'none';
    DivID = "";
    Resizewindow();
}

function Resizewindow() {

    //adjustwin('100', this.window);
    // This is to adjust the absolute position of the controls once
    // the grid is shown or hidden.
    var closeimg = document.getElementById("closeimg");
    //closeimg.onmouseover();
    //closeimg.onmouseout();
}

function togglesrchselect() {

    var check = false;
    var obj = document.getElementById("searchall");
    if (obj.checked)
        check = true;
    else
        check = false;

    for (var i = 0; i < document.getElementById("s1").options.length; i++) {
        var cntrl = "search" + i;
        document.getElementById(cntrl).checked = check;
    }
}

function PopupfadeTo(obj, opacity) {

    obj = $j("#" + obj.id);
    if (opacity <= 100) {
        if (opacity < 0) {
            opacity = 100;
            document.body.style.cursor = 'default';
        } else {
            document.body.style.cursor = 'Hand';
            opacity = 65;
        }
        SetOpacity(obj, opacity);
    }
}

function tstSearchFilterRepeat() {
    let _dvfcid = $("[id^='dvFilterCondition']").length;
    let _dvfcHtml = `<div id="dvFilterCondition${_dvfcid}" class="col-md-10 d-flex flex-row-auto gap-1 mt-2">${$("#dvFilterCondition").html()}</div>`;
    _dvfcHtml = _dvfcHtml.replace(/ddlSearch/g, "ddlSearch" + _dvfcid);
    _dvfcHtml = _dvfcHtml.replace(/ddlModeSearch/g, "ddlModeSearch" + _dvfcid);
    _dvfcHtml = _dvfcHtml.replace(/searstr/g, "searstr" + _dvfcid);
    $("#dvFilterCondition").parents('.flex-column').find('#srchcontent').before(_dvfcHtml);
    $(`#dvFilterCondition${_dvfcid} #lblsrch`).addClass("d-none");
    $(`#dvFilterCondition${_dvfcid} #lblwth`).addClass("d-none");
    $(`#dvFilterCondition${_dvfcid} #lblsearchvalue`).addClass("d-none");
    tstSearchOnChange();
}
function valid_submit() {
    GetCurrentTime("Tstruct load on Search Go button click(ajax call)");
    var gov = $j("#goval");
    gov.val("go");
    if (ValidateSrchFlds()) {
        ShowDimmer(true);
        GetTstSearchData();
    } else {
        if (iststddlcheck)
            showAlertDialog("warning", 5006, "client");
        else
            showAlertDialog("warning", 2007, "client");
    }
    return;
}

function GetTstSearchData(_pageNo = "1") {
    let _selectSearchddl = "";
    let _selectSearchfcVal = "";
    let _valuesArray = $("[id^='dvFilterCondition'] [id^='ddlModeSearch']").map(function () {
        return $(this).val(); // Get the value of each element
    }).get();
    $("[id^='dvFilterCondition'] [id^='ddlSearch']").each(function () {
        if (_selectSearchddl != "")
            _selectSearchddl = _selectSearchddl + "~~" + $(this).val();
        else
            _selectSearchddl = $(this).val();
    });
    $("[id^='dvFilterCondition'] [id^='searstr']").each(function (ind) {
        if (_selectSearchfcVal != "") {
            if (_valuesArray[ind] == "Starts with")
                _selectSearchfcVal = _selectSearchfcVal + "~~" + "'" + $(this).val() + "%'";
            else if (_valuesArray[ind] == "Ends with")
                _selectSearchfcVal = _selectSearchfcVal + "~~" + "'%" + $(this).val() + "'";
            else
                _selectSearchfcVal = _selectSearchfcVal + "~~" + "'%" + $(this).val() + "%'";
        }
        else {
            if (_valuesArray[ind] == "Starts with")
                _selectSearchfcVal = "'" + $(this).val() + "%'";
            else if (_valuesArray[ind] == "Ends with")
                _selectSearchfcVal = "'%" + $(this).val() + "'";
            else
                _selectSearchfcVal = "'%" + $(this).val() + "%'";
        }
    });
    $j("#hdnSearchStr").val(_selectSearchfcVal);
    $.ajax({
        url: 'tstruct.aspx/GetTstSearchData',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            key: tstDataId,
            transId: transid,
            hdnSearchStr: $j("#hdnSearchStr").val(),
            ddlSearch: _selectSearchddl,// $j("#ddlSearch").val(),
            pageNo: _pageNo,
            pageSize: "10",
            isTstHtmlLs: resTstHtmlLS
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            var result = data.d;
            if (result.indexOf('Error♠') == -1) {
                $("#srchcontent").removeClass('d-none');
                $("#Panel1").show();
                let res = result.split("♠♠")[1];
                $("#Panel1 div:eq(0)").html('');
                $("#Panel1 div:eq(0)").prepend(res);
                let lblres = result.split("♠♠")[0];
                $("#records").text(lblres.split('♠')[1]);
                $("#pages").text(lblres.split('♠')[2]);
                $("#pgCap").text("Page No.");
                let _ddlList = lblres.split('♠')[0];
                if (_ddlList != "")
                    $("#lvPage").empty();
                let ddlList = _ddlList.split(',');
                ddlList.forEach(function (val) {
                    if (val != "")
                        $("#lvPage").append('<option value="' + val + '">' + val + '</option>');
                })
                bindUpdownEvents('grdSearchRes', 'single');

            } else {
                result = result.replace('Error♠', '');
                if (result == 'Duplicate_session') {
                    window.location.href = "err.aspx?errmsg=Needs logout and login again since in-memory is cleared.";
                }
                else if (result.indexOf('records:') == -1) {
                    showAlertDialog("error", result);
                } else {
                    $("#srchcontent").removeClass('d-none');
                    result = result.replace('records:', '');
                    $("#records").text(result);
                }
            }
            AxWaitCursor(false);
            ShowDimmer(false);
        }, error: function (error) {
            AxWaitCursor(false);
            ShowDimmer(false);
        }
    });
}

function DecodeUrlSplChars(value) {
    value = value.replace(/&/g, "&amp;");
    value = value.replace(/%25/g, "%");
    value = value.replace(/%26/g, "&");
    value = value.replace(/%27/g, "'");
    value = value.replace(/%22/g, '"');
    value = value.replace(/%23/g, "#");
    return value;
}
function tstSearchOnChange() {
    $("[id^='dvFilterCondition'] [id^='ddlSearch']").change(function () {
        var srchDdl = $j(this);
        var selFld = srchDdl.val();
        var indx = $j.inArray(selFld, FNames);
        if (indx != -1) {
            var fldType = FDataType[indx];
            if (fldType == "Date/Time") {
                let ddd = srchDdl.attr("id");
                ddd = ddd.replace('ddlSearch', '');
                $("#ddlModeSearch" + ddd).empty();
                $("#ddlModeSearch" + ddd).append('<option value="Equals" Selected="True">Equals</option>');
            } else {
                let ddd = srchDdl.attr("id");
                ddd = ddd.replace('ddlSearch', '');
                $("#ddlModeSearch" + ddd).empty();
                $("#ddlModeSearch" + ddd).append('<option value="Contains" Selected="True">Contains</option>');
                $("#ddlModeSearch" + ddd).append('<option value="Starts with">Starts with</option>');
                $("#ddlModeSearch" + ddd).append('<option value="Ends with">Ends with</option>');
            }
        }
    });
}
function ValidateSrchFlds() {
    iststddlcheck = false;
    let _icot = 0;
    let _isValidate = false;
    $("[id^='dvFilterCondition'] [id^='ddlSearch']").each(function () {
        var srchDdl = $j(this);
        var selFld = srchDdl.val();
        if (selFld != "") {
            var indx = $j.inArray(selFld, FNames);
            var txtVal = _icot == 0 ? $j.trim($j("#searstr").val()) : $j.trim($j("#searstr" + _icot).val());
            if (indx != -1 && txtVal != "") {
                var fldType = FDataType[indx];
                if (fldType == "Date/Time") {
                    let _thisContFld = _icot == 0 ? $j("#searstr") : $j("#searstr" + _icot);
                    var srchFldValue = _icot == 0 ? $j.trim($j("#searstr").val()) : $j.trim($j("#searstr" + _icot).val());
                    var isproperdate = isDate(srchFldValue);
                    if (!isproperdate) {
                        _thisContFld.focus();
                        _isValidate = false;
                        return false;
                    } else {
                        _thisContFld.focus();
                        _isValidate = true;
                    }
                } else {
                    _isValidate = true;
                }
            } else {
                _isValidate = false;
                return false;
            }
            _icot++;
        } else {
            iststddlcheck = true;
            _isValidate = false;
            return false;
        }
    });
    return _isValidate;
}

function loadTstruct(recid) {
    WireElapsTime(serverprocesstime, requestProcess_logtime);
    ShowDimmer(true);
    ResetNavGlobalVariables();
    AxWaitCursor(true);
    //window.document.location.href = "tstruct.aspx?transid=" + transid + "&recordid=" + recid;
    try {
        if (!window.parent.isSessionCleared && !window.opener) {
            ASB.WebService.ClearNavigationSession();
            window.parent.isSessionCleared = true;
        }
        if (!window.opener)
            window.parent.disableNavigation = true;
        else
            window.opener.parent.disableNavigation = true;
        setTimeout(function () {
            loadRecordFromSearch = true;
            GetLoadData(recid, "");
        }, 0);
    } catch (ex) {
        Closediv();
    }
}

// Checks if the browsers is IE or another.
// document.all will return true or false depending if its IE
// If its not IE then it adds the mouse event
if (!document.all)
    document.captureEvents(Event.MOUSEMOVE)

// On the move of the mouse, it will call the function getPosition
// These varibles will be used to store the position of the mouse
var iX = 0;
var iY = 0;
var iWidth = 0;
var timer;
var dDetailsShow = false;
// This is the function that will set the position in the above varibles
function getPosition(args) {
    // Gets IE browser position
    if (document.all) {
        iX = event.clientX + document.body.scrollLeft
        iY = event.clientY + document.body.scrollTop
    }
    // Gets position for other browsers
    else {
        iX = args.pageX
        iY = args.pageY
    }
}

function findPosX(obj) {
    var curleft = 0;
    if (obj.offsetParent) {
        while (obj.offsetParent) {
            curleft += obj.offsetLeft
            obj = obj.offsetParent;
        }
    } else if (obj.x)
        curleft += obj.x;
    return curleft;
}

function findPosY(obj) {
    var curtop = 0;
    if (obj.offsetParent) {
        while (obj.offsetParent) {
            curtop += obj.offsetTop
            obj = obj.offsetParent;
        }
    } else if (obj.y)
        curtop += obj.y;
    return curtop;
}

function FindCtrlPos(fldName) {

    if (typeof (fldName) == "string") {
        fldName = document.getElementById(fldName);
    }
    var fld = fldName;
    var textbox = jQuery(fldName);
    var offset = textbox.offset();
    if ($(textbox).parents('td').length > 0) {
        if (!textbox.hasClass('form-select') && !textbox.hasClass('flatpickr-input')) {
            iX = findPosX(fld) - 10;
            iX = iX + $($(textbox).parents('tr')).offset().left;
            iY = findPosY(fld) + fld.clientHeight + 1;
            iWidth = fld.clientWidth;
        }
        else if (textbox.hasClass('flatpickr-input')) {
            iX = findPosX(fld);
            iX = iX + $($(textbox).parents('tr')).offset().left;
            iY = findPosY(fld) + fld.clientHeight + 1;
            iWidth = fld.clientWidth + textbox.next('span.input-group-text').outerWidth();
        } else {
            iX = offset.left - 10;
            iX = iX + $($(textbox).parents('tr')).offset().left;
            iY = (offset.top + 31) - window.scrollY;
            iWidth = textbox.parent().width();
        }
    } else {
        if (!textbox.hasClass('form-select') && !textbox.hasClass('flatpickr-input')) {
            iX = findPosX(fld) + 1;
            iY = findPosY(fld) + fld.clientHeight + 1;
            iWidth = fld.clientWidth;
        }
        else if (textbox.hasClass('flatpickr-input')) {
            iX = findPosX(fld);
            iY = findPosY(fld) + fld.clientHeight + 1;
            iWidth = fld.clientWidth + textbox.next('span.input-group-text').outerWidth();
        } else {
            iX = offset.left + 1;
            iY = (offset.top + 31) - window.scrollY;
            iWidth = textbox.parent().width() + fld.clientWidth;
        }
    }
}

function ShowTooltipDiv(isfromClk) {
    if (document.getElementById)
        // Standard way to get element
        div = document.getElementById('dvTip');
    else if (document.all)
        // Get the element in old IE's
        div = document.all['dvTip'];

    // if the style.display value is blank we try to check it out here
    if (div.style.display == '' && div.offsetWidth != undefined && div.offsetHeight != undefined) {
        div.style.display = (div.offsetWidth != 0 && elem.offsetHeight != 0) ? 'block' : 'none';
    }

    div.style.display = "block";
    div.style.left = iX + 'px';
    div.style.top = iY + 'px';
    div.style.zIndex = '9999999';
    div.style.width = iWidth + 'px';
    $(div).removeClass("d-none");
    dDetailsShow = true;
    if (!isfromClk) {
        timer = setTimeout(function () {
            HideTooltip();
        }, 3000);
    } else {
        $("#dvInnerTip").parent().find(".closebtn").removeClass("d-none");
    }
}
var regexFld = new RegExp("^[a-zA-z0-9_]+$");
var _thisToolTip = "";
$j(document).off("click", "#dvTip").on("click", "#dvTip", function (e) {
    clearTimeout(timer);
    let _fldId = $(this).find("#dvInnerTip").attr("data-fldId");
    _thisToolTip = _fldId;
    ShowTooltip($(this).find("#dvInnerTip").attr("data-fldName"), $("#" + _fldId), true);
});

$j(document).off("click", "body").on("click", "body", function (e) {
    if (dDetailsShow && e.target.className.indexOf('form-control') == -1 && e.target.className.indexOf('tooltipBg') == -1 && e.target.className.indexOf('closebtn') == -1 && e.target.className.indexOf('select2-selection__rendered') == -1 && e.target.className.indexOf('select2-selection__placeholder') == -1 && e.target.id != "dvInnerTip" && e.target.id != "dvTip" && !$(e.target).parent().hasClass("input-group-text")) {
        _thisToolTip = "";
        HideTooltip();
        dDetailsShow = false;
    }
});

function readOnlyFldddDiv() {

    FFieldReadOnly.forEach(function (val, inx) {
        let _fName = FNames[inx];
        if (val == 'False')
            return;
        if (FToolTip[inx] == "")
            return;
        if (!IsGridField(_fName)) {
            let _fdcno = GetDcNo(_fName);
            let _fldName = _fName + "000F" + _fdcno;
            $("#" + _fldName).parents('#dv' + _fName).find("div:eq(0)").append('<span class="material-icons material-icons-style ms-1 align-middle material-icons-3 cursor-pointer spanddetails" title="Display details">info</span>')
            $("#" + _fldName).parents('#dv' + _fName).find('.spanddetails').attr("onclick", "readOnlyShowTooltip('" + _fName + "','" + _fdcno + "','false')");
        }
    });
}

function readOnlyFldddDivGrid(_thisDcNo, _thisRowNo) {
    let thisFlds = GetGridFields(_thisDcNo);
    thisFlds.forEach(function (val, inx) {
        let _thisInx = GetFieldIndex(val);
        let _fName = FNames[_thisInx];
        if (FFieldReadOnly[_thisInx] == 'False')
            return;
        if (FToolTip[_thisInx] == "")
            return;
        if (IsGridField(_fName)) {
            let _fldName = _fName + _thisRowNo + "F" + _thisDcNo;
            if ($("input#" + _fldName).length > 0) {
                $("input#" + _fldName).parent().addClass('input-group');
                $("input#" + _fldName).parent().append(`<span class="material-icons material-icons-style cursor-pointer input-group-text p-2" title="Display details" style="" onclick="readOnlyShowTooltip('` + _fName + `','` + _thisDcNo + `','true','` + _thisRowNo + `');">info</span>`);
            } else if ($("select#" + _fldName).length > 0) {
                $("select#" + _fldName).parent().addClass('input-group');
                $("select#" + _fldName).next('span.select2-container--bootstrap5').addClass("flex-fill w-auto");
                $("select#" + _fldName).next('span.select2-container--bootstrap5').parent().append(`<span class="material-icons material-icons-style cursor-pointer input-group-text p-2" title="Display details" style="" onclick="readOnlyShowTooltip('` + _fName + `','` + _thisDcNo + `','true','` + _thisRowNo + `');">info</span>`);
            }
        }
    });
}

function readOnlyShowTooltip(_fldName, _fdcno, _isgridfld, _frowNo = '') {
    setTimeout(function () {
        if (_isgridfld == 'true') {
            ShowTooltip(_fldName, $("#" + _fldName + _frowNo + "F" + _fdcno), false);
        } else {
            ShowTooltip(_fldName, $("#" + _fldName + "000F" + _fdcno), false);
        }
    }, 0);
}

function ShowTooltip(fldName, obj, isfromClk = false) {
    HideTooltip();
    var i = 0;
    var toolTip = "";
    var toolTipList = new Array();
    var ttURL;
    var toolTipHyperlnk = "";
    for (i = 0; i < FNames.length; i++) {
        if (FNames[i] == fldName) {

            toolTip = FToolTip[i];
            var displyText = "";
            var stPos = toolTip.indexOf("<h>");
            if (stPos != -1) {
                var enPos = toolTip.indexOf("</h>");
                var ttURL = toolTip.substring(stPos + 3, enPos);

                displyText = toolTip.substring(0, stPos);
                toolTipHyperlnk = "<a class='curHand' id='lblTip'  onclick='javascript:HyperlinkToolTip(\"" + ttURL + "\");'>" + displyText + " </a>";
                toolTip = toolTip.substring(enPos + 4, toolTip.length);
            }

            if (toolTip != "")
                toolTipList = toolTip.split("/");

            break;
        }
    }
    var j = 0;
    if (obj.hasClass('flatpickr-input'))
        return;
    var flname = obj.attr("id");
    var flval = obj.value;
    var temp = "";
    for (j = 0; j < toolTipList.length; j++) {
        //Condition to check if the tooltip Is not empty and is plainText/String .

        if (toolTipList[j] != "") {
            if (toolTipList[j].toString().indexOf(":") == -1) {
                toolTipList[j] = toolTipList[j].trim();
                temp += toolTipList[j].trim();
            }
            //Conodition to check for fields or expressions
            else {
                var tempArr = new Array();
                var tempStr = toolTipList[j];
                var finalStr = "";
                var charCnt = 0;
                var strLen = 0;
                var ch = '';
                var nextCh = '';
                var isColon = false;
                //tempStr = ":name_ Hi";

                //Loop through the string to seperate the string, fields and expressions
                for (charCnt = 0; charCnt <= tempStr.length; charCnt++) {
                    ch = tempStr.charAt(charCnt);

                    if (ch == ":") {
                        nextCh = tempStr.charAt(charCnt + 1);
                        if (nextCh == "+") {
                            //Get the expression and push it to the array
                            var str = tempStr.substring(tempStr.indexOf(":+"), tempStr.indexOf("+:") + 2);
                            str = str.replace(":+", "");
                            str = str.replace("+:", "");
                            str = Evaluate(flname, flval, str, "expr");
                            str = ReverseCheckSpecialChars(str);
                            tempArr.push(str);
                            charCnt = tempStr.indexOf("+:") + 2;
                        } else {
                            isColon = true;
                            if (finalStr != "") {
                                tempArr.push(finalStr);
                            }
                            finalStr = "";
                        }
                    } else {
                        if (isColon == true) {
                            var res = regexFld.test(ch);
                            if (res == false) {
                                var rowFramNo = GetFieldsRowFrameNo(flname);
                                var fldInd = $j.inArray(finalStr, FNames);
                                if (fldInd != -1) {
                                    if (GetDcNo(finalStr) == GetFieldsDcNo(flname)) {
                                        res = GetFieldValue(finalStr + rowFramNo);
                                        res = ReverseCheckSpecialChars(res);
                                    } else if (GetDcNo(finalStr) != GetFieldsDcNo(flname) && !IsDcGrid(GetDcNo(finalStr))) {
                                        let _thisPDcno = GetDcNo(finalStr);
                                        res = GetFieldValue(finalStr + "000F" + _thisPDcno);
                                        res = ReverseCheckSpecialChars(res);
                                    } else if (GetDcNo(finalStr) != GetFieldsDcNo(flname) && IsDcGrid(GetDcNo(finalStr))) {
                                        let _thisPDcno = GetDcNo(finalStr);
                                        res = GetFieldValue(finalStr + "001F" + _thisPDcno);
                                        res = ReverseCheckSpecialChars(res);
                                    }
                                    tempArr.push(res);
                                } else {
                                    res = checkParameterandMemVars(finalStr);
                                    res = ReverseCheckSpecialChars(res);
                                    tempArr.push(res);
                                }

                                finalStr = "";
                                isColon = false;
                            }
                        }
                        finalStr += ch;
                    }
                }
                if (finalStr != "")
                    tempArr.push(finalStr);
                //Loop through the tempArr and evaluate the string and generate the tooltip
                var items = 0;
                toolTipList[j] = "";
                for (items = 0; items < tempArr.length; items++) {
                    toolTipList[j] += tempArr[items];
                }
            }

        }
    }

    //Write the result to the div inner Text
    if (toolTipList.length > 0) {

        FindCtrlPos(flname);
        var dvToolTip = $j("#dvInnerTip");
        dvToolTip.attr("data-fldName", fldName);
        dvToolTip.attr("data-fldId", flname);
        if (_thisToolTip != flname)
            _thisToolTip = "";
        var toolTip = "";
        for (i = 0; i < toolTipList.length; i++) {
            if (toolTipList[i] != "") {
                toolTip += toolTipList[i] + "<br/>";
            }
        }
        //if (toolTipHyperlnk != "") {
        //    dvToolTip.html(toolTipHyperlnk + " " + toolTip);
        //    ShowTooltipDiv(isfromClk);
        //} else if (toolTip != "") {
        //    dvToolTip.html(toolTip);
        //    ShowTooltipDiv(isfromClk);
        //}


        if (toolTipHyperlnk != "") {
            dDetailsShow = true;
            //$(".dvdisplaydetails").parent('.tstructMainBottomFooter').removeClass('pb-4');
            //$(".dvdisplaydetails").parent('.tstructMainBottomFooter').find('div:eq(0)').addClass('pb-1');
            /* $(".dvdisplaydetails").removeClass('d-none');*/
            $(".dvdisplaydetails").css({ "background-color": "#ededca" });            
            $("#dvddHyplink a").remove();
            $("#dvddHyplink").append(toolTipHyperlnk);
            $("#dvddHyplink").removeClass('d-none');
            $("#lblddfooter").text('');
            $("#lblddfooter").html(toolTip);
            $("#lblddfooter").attr("title", $("#lblddfooter").text());
            //$("#lblddfootertitle").text('Details: ');
            $(".dvdisplaydetails").removeClass('pb-6');
        } else if (toolTip != "") {
            dDetailsShow = true;
            //$(".dvdisplaydetails").parent('.tstructMainBottomFooter').removeClass('pb-4');
            //$(".dvdisplaydetails").parent('.tstructMainBottomFooter').find('div:eq(0)').addClass('pb-1');
            //$(".dvdisplaydetails").removeClass('d-none');
            $(".dvdisplaydetails").css({ "background-color": "#ededca" });
            $("#dvddHyplink a").remove();
            $("#dvddHyplink").addClass('d-none');
            $("#lblddfooter").text('');
            $("#lblddfooter").html(toolTip);
            $("#lblddfooter").attr("title", $("#lblddfooter").text());
            //$("#lblddfootertitle").text('Details: ');
            $(".dvdisplaydetails").removeClass('pb-6');
        }
    } else {
        let isFldPurposeKey = true;
        let _finalJson = [];
        try {
            tstConfigurations.config.forEach(function (val) {
                var ret = {};
                $.map(val, function (value, key) {
                    ret[key.toLowerCase()] = value;
                });
                _finalJson.push(ret);
            });
            let _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "avoid purpose field from display details" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.propsval.toLowerCase() == "true");
            if (_formLoadFlag.length > 0)
                isFldPurposeKey = false;
        } catch (ex) { }
        if (isFldPurposeKey) {
            for (i = 0; i < FNames.length; i++) {
                if (FNames[i] == fldName) {
                    let _fldPurpose = FldPurpose[i];
                    if (_fldPurpose != '') {
                        dDetailsShow = true;
                        $(".dvdisplaydetails").css({ "background-color": "#ededca" });
                        $("#dvddHyplink a").remove();
                        $("#dvddHyplink").addClass('d-none');
                        $("#lblddfooter").text('');
                        $("#lblddfooter").html(_fldPurpose);
                        $("#lblddfooter").attr("title", $("#lblddfooter").text());
                        $(".dvdisplaydetails").removeClass('pb-6');
                    }
                    break;
                }
            }
        }
    }
}

var dpTextVarlist = new Array();
var dpTextValuelist = new Array();
var dpTextaxMemVarValue = new Array();
var dpTextaxMemVarList = new Array();
function checkParameterandMemVars(finalStr) {
    if (dpTextVarlist.length > 0) {
        var fldInd = $j.inArray(finalStr, dpTextVarlist);
        if (fldInd != -1) {
            return dpTextValuelist[fldInd];
        } else if (fldInd == -1) {
            var memfldInd = $j.inArray(finalStr, dpTextaxMemVarList);
            if (memfldInd != -1) {
                return dpTextaxMemVarValue[memfldInd];
            }
        }
    } else {
        if (Parameters.length > 1) {
            for (var pki = 0; pki < Parameters.length; pki++) {
                var list = Parameters[pki].toString();
                list = list.split("~");
                dpTextVarlist[pki] = list[0].toString();
                dpTextValuelist[pki] = list[1].toString();
            }
        }

        if (AxMemParameters.length > 1) {
            for (var ami = 0; ami < AxMemParameters.length; ami++) {
                var list = AxMemParameters[ami].toString();
                list = list.split("~");
                dpTextaxMemVarList[ami] = list[0].toString();
                dpTextaxMemVarValue[ami] = list[1].toString().replace(/&quot;/g, '"');
            }
        }

        var fldInd = $j.inArray(finalStr, dpTextVarlist);
        if (fldInd != -1) {
            return dpTextValuelist[fldInd];
        } else if (fldInd == -1) {
            var memfldInd = $j.inArray(finalStr, dpTextaxMemVarList);
            if (memfldInd != -1) {
                return dpTextaxMemVarValue[memfldInd];
            }
        }
    }
}

document.onkeypress = function hideTooltip(event) {
    if (event.keyCode == 27) {
        HideTooltip()
    }
}

function HideTooltip() {
    //$(".dvdisplaydetails").parent('.tstructMainBottomFooter').addClass('pb-4');
    //$(".dvdisplaydetails").parent('.tstructMainBottomFooter').find('div:eq(0)').addClass('pb-1');
    //$(".dvdisplaydetails").addClass('d-none');
    $(".dvdisplaydetails").removeAttr('style');
    $(".dvdisplaydetails").addClass('pb-6');
    $("#lblddfooter").text('');
    //$("#lblddfootertitle").text('');
    $("#dvddHyplink a").remove();
    if (_thisToolTip != "")
        return;
    var dvtooltip = $j("#dvTip");
    if (dvtooltip.length > 0) {
        dvtooltip.hide();
    }
    //$(".dvdisplaydetails").addClass('d-none');
}

var hlTipProps = "";
var hlTipParmUrl = "";
var hlTipUrl = "";
var hlTipParamXml = "";

function HyperlinkToolTip(ttUrl) {
    hlTipParmUrl = "";
    var structName = "";
    var propArr = new Array();
    var arrParam = new Array();
    propArr = ttUrl.split(",");
    var iName = "";
    var refresh = "";
    var newLink = "";
    var openWin = "";
    var pType = "";
    var load = "";
    var propDetails = new Array();
    if (propArr.length > 0) {
        for (var i = 0; i < propArr.length; i++) {
            propDetails = propArr[i].split("=");
            if (propDetails[0] == 'type') {
                temp = propDetails[1];
                if (temp.toLowerCase().startsWith('t')) {
                    hlTipUrl = "tstruct.aspx?act=open&transid=";
                    pType = "t";
                } else if (temp.toLowerCase().startsWith('i')) {
                    hlTipUrl = "ivtoivload.aspx?ivname=";
                    pType = "i";
                } else if (temp.toLowerCase().startsWith('hp')) {
                    hlTipUrl = "htmlPages.aspx?load=";
                    pType = "hp";
                }
            } else if (propDetails[0] == 'name') {
                hlTipUrl = hlTipUrl + propDetails[1] + `&openerIV=${typeof isListView != "undefined" ? iName : propDetails[1]}&isIV=${typeof isListView != "undefined" ? !isListView : "false"}&isDupTab=${callParentNew('isDuplicateTab')}`;
                structName = propDetails[1];
            } else if (propDetails[0] == 'popup') {
                openWin = propDetails[1];
            } else if (propDetails[0] == 'displaymode' && propDetails[1] == "p") {
                openWin = "true";
            } else if (propDetails[0] == 'refresh') {
                refresh = propDetails[1];
            } else if (propDetails[0] == 'load') {
                load = propDetails[1];
            } else if (propDetails[0] == 'param') {
                CreatParamUrl(propArr[i]);
            }
        }
    }

    hlTipProps = pType + "," + load + "," + openWin;

    if (pType == "t" && structName != "" && load.startsWith("t")) {
        try {
            ASB.WebService.GetRecordId(structName, hlTipParamXml, SuccessGetRecId, OnException);
        } catch (e) { }
    } else {
        GetUrl();
    }
}

function SuccessGetRecId(result, eventArgs) {
    var rId = "0";
    if (result != "") {
        var xmlDoc = $j.parseXML(result),
            xml = $j(xmlDoc);
        if (xml.find("recordid").length > 0)
            rId = xml.find("recordid").text();
    }
    GetUrl(rId);
}

function GetUrl(rId) {

    var pType = "";
    var load = "";
    var openWin = "";
    if (hlTipProps != "") {
        var strHlProps = hlTipProps.split(",");
        pType = strHlProps[0];
        load = strHlProps[1];
        openWin = strHlProps[2];
    }

    if (hlTipUrl != "") {

        if (pType == "t" && rId != undefined && rId != "0" && load.startsWith("t"))
            hlTipUrl += '&recordid=' + rId;
        else
            hlTipUrl += hlTipParmUrl;

        if (openWin.startsWith('t')) {
            var newWindow;
            try {
                //newWindow = window.open(hlTipUrl, 'MyPopUp', 'resizable=yes');
                hlTipUrl = hlTipUrl + "&AxPop=true";
                setTimeout(function () {
                    setTimeout(function () {
                        createPopup(hlTipUrl);
                    }, 150);
                }, 0);
            } catch (ex) {
                showAlertDialog("warning", eval(callParent('lcm[356]')));
            }
        } else
            window.document.location.href = hlTipUrl;
    }
}

function CreatParamUrl(temp) {

    var pType = "";
    if (hlTipProps != "") {
        var strHlProps = hlTipProps.split(",");
        pType = strHlProps[0];
    }
    hlTipParmUrl = "";
    var stPos = temp.indexOf("param=");
    if (stPos != -1) {
        var enPos = temp.indexOf("=");
        temp = temp.substring(6, temp.length);
    }
    arrParam = temp.split("~");
    if (arrParam.length > 0) {
        for (var k = 0; k < arrParam.length; k++) {
            var stPos = arrParam[k].indexOf("=");
            var fldName = arrParam[k].substring(stPos + 1, arrParam[k].count);
            var param = arrParam[k].substring(0, stPos)
            if (fldName.startsWith(':')) {
                fldName = fldName.substring(1, fldName.length)
                var isGridDc = IsGridField(fldName);
                var val = "";
                if (IsGridField(fldName)) {
                    var actRowNo = GetRowNoHelper(AxActiveRowNo);
                    var fld = fldName + actRowNo + "F" + AxActiveDc;
                    val = GetFieldValue(fld);
                } else {
                    var frameNo = GetDcNo(fldName);
                    var fld = fldName + "000F" + frameNo;
                    val = GetFieldValue(fld);
                }
            } else if (fldName.startsWith('{')) {
                if (fldName.indexOf("{") != -1)
                    fldName = fldName.replace('{', '');
                if (fldName.indexOf("}") != -1)
                    fldName = fldName.replace('}', '');

                if (fldName.endsWith("*d"))
                    val = fldName.substring(0, fldName.length - 2)
                else
                    val = fldName;
            } else {
                val = fldName;
            }
            hlTipParamXml = hlTipParamXml + "<" + param + ">" + val + "</" + param + ">";
            if (pType == "i" && val != "")
                hlTipUrl = hlTipUrl + '&' + param + '=' + val;
            else if (val != "")
                hlTipParmUrl = hlTipParmUrl + '&' + param + '=' + val;
        }
    }
}

function GetSetTtipHgt(status) {
    var dvTooltip = $j("#dvInnerTip");
    var dvtip = $j("#dvTip");
    if (status == "new") {
        if (dvTooltip.height() < 50) {
            dvTooltip.height("50");
        }
        if (dvTooltip.width() < 100) {
            dvTooltip.width("100");
        }
        if (dvtip.height() < 50) {
            dvtip.height("50");
        }
        if (dvtip.width() < 100) {
            dvtip.width("100");
        }
    } else {
        dvTooltip.css("height", "auto");
        dvTooltip.css("width", "auto");
        dvtip.css("height", "auto");
        dvtip.css("width", "auto");
    }
}
var tmpFldName = "";
//Function to get the picklist data and display it in the picklist div.
function DisplayPickList(obj) {
    ShowDimmer(true);
    AxWaitCursor(true);
    var fldProps = obj.attr("id").split("~");
    var i;
    if (fldProps.length == 2)
        i = 1;
    else
        i = 0;

    var fldName = fldProps[i].substring(0, fldProps[i].lastIndexOf("F") - 3);
    var fldValue = "";
    var pickFld = $j("#" + fldProps[i]);
    //The below condition will not open picklist if the picklist field is made readonly
    if (pickFld.attr("readonly") != undefined || pickFld.attr("disabled") != undefined) {
        ShowDimmer(false);
        AxWaitCursor(false);
        return;
    }

    if (pickFld.length > 0) {
        AxOldValue = pickFld.val();
        fldValue = pickFld.val(); //pickFld.value;
    }

    totalPLRows = 0;
    curPageNo = 1;

    var hdn = $j("#hdnPickFldId");
    if (obj.attr("id").indexOf("~") == -1)
        hdn.val("img~" + obj.attr("id"));
    else
        hdn.val(obj.attr("id"));
    AxFromAssociated = true;
    GetPickListData(fldName, fldValue, curPageNo.toString(), pageSize.toString(), fldProps[i]);
}

//Function to position and show the div containing the picklist data.
function ShowPickList() {
    //hide picklist next/prev loader
    $j("#pickDimmer").css("display", "none");
    var hdn = $j("#hdnPickFldId");
    var objId = hdn.val();
    var fldProps = objId.split("~");
    var i;
    if (fldProps.length == 2)
        i = 1;
    else
        i = 0;
    var pickFld = document.getElementById(fldProps[i]);

    FindCtrlPos(pickFld);
    var pickFld = $j("#" + fldProps[i]);
    var wdth = pickFld.width() + 19;
    var plHgt = pickFld.height();

    var dv = $j("#dvPickList");
    var divFrameNo = GetFieldsDcNo(fldProps[i]);
    var isDcGrid = IsDcGrid(divFrameNo);

    dv.width(wdth);
    dv.show();
    //If it's a scroll div,set the position of the left scroll value
    var scrollleft = $(".dvheightframe ").scrollLeft();
    var scrollTop = $(".dvheightframe ").scrollTop();
    //if (!isDcGrid) {
    //    if (scrollTop > 0) {
    //        var tmpHgt = (iY - plHgt - scrollTop - 18);
    //        var tmpTop = parseInt((tmpHgt - 0), 10);
    //        if (tmpTop > 0)
    //            dv.css("top", tmpTop + 'px');
    //        else
    //            dv.css("top", (iY + 5) + 'px');
    //    }
    //    else
    //        dv.css("top", (iY - 69) + 'px');

    //    if (scrollleft > 0) {
    //        var slValue = (iX - wdth - scrollleft);
    //        var clientWidth = $j("#divDc" + divFrameNo).width() + 20;
    //        var scrollWidth = 0;
    //        var popupScrollWidth = (slValue + 200);
    //        if (clientWidth < popupScrollWidth)
    //            scrollWidth = popupScrollWidth - clientWidth;
    //        var leftV = parseInt((slValue - scrollWidth), 10);
    //        if (leftV > 0)
    //            dv.css("left", leftV + 'px');
    //        else
    //            dv.css("left", '0px');
    //    }
    //    else
    //        dv.css("left", (iX - wdth) + 'px');
    //} else {
    if (axInlineGridEdit)
        scrollTop = scrollTop - 36; //calculating picklist position for inline grid
    var top = (pickFld.offset().top - pickFld.parents('.' + (axInlineGridEdit ? 'input' : 'form') + '-group').outerHeight()) + scrollTop;
    dv.css("top", top + 'px');
    dv.css("left", pickFld.offset().left + 'px');
    //}


    // CheckHeight(iY + 5, 205);
    if ($("#dvPickList .pickLstResultCntnr").length > 0) {
        if ($("#dvPickList .pickLstResultCntnr").offset().top + $("#dvPickList .pickLstResultCntnr").outerHeight() - $(".dvheightframe").offset().top > $(".dvheightframe").outerHeight()) {
            $(".dvheightframe").animate({
                scrollTop: (($("#dvPickList .pickLstResultCntnr").offset().top + $("#dvPickList .pickLstResultCntnr").outerHeight() - $(".dvheightframe").offset().top) - ($(".dvheightframe").outerHeight())) + ($(".dvheightframe").scrollTop())
            }, 100);
        }
    }
}

function CheckHeight(top, dvHgt) {
    var frm = $j("#middle1", parent.document);
    var docHgt = document.body.offsetHeight;
    var totHgt = top + dvHgt;
    if (totHgt > docHgt) {
        docHgt = docHgt + (totHgt - docHgt);
        frm.height(docHgt + 10);
    }
}

//Function to call the service to get the filtered picklist data.
function GetPickListData(fldName, value, pageNo, pageSize, objId) {
    currentPickList = objId;
    //initialise the search value
    initialSrchVal = value;
    var includeDcs = "";
    if (arrRefreshDcs.length > 0) {
        for (var i = 0; i < arrRefreshDcs.length; i++) {
            var arrDcNos = arrRefreshDcs[i].split(':');
            includeDcs = arrDcNos[1].replace("dc", "") + ',' + arrDcNos[0].replace("dc", "");

        }
    }
    value = CheckSpecialCharsInStr(value);
    var fldDcNo = GetFieldsDcNo(objId);

    AxActiveRowNo = parseInt(GetFieldsRowNo(objId), 10);
    AxActiveRowNo = GetDbRowNo(AxActiveRowNo, fldDcNo);
    var activeRow = AxActiveRowNo;

    var parStr = "";
    if (AxActivePRow != "" && AxActivePDc != "")
        parStr = AxActivePDc + "~" + AxActivePRow;

    var subStr = "";
    if (IsParentField(fldName, fldDcNo)) {
        //for each subgrid, get the sub grid rows for the given parent row and send this info
        subStr = GetSubGridInfoForParent(fldDcNo, AxActiveRowNo);
    }

    try {
        ASB.WebService.GetSearchResult(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, fldName, value, pageNo.toString(), pageSize.toString(), tstDataId, fldDcNo, activeRow, parStr, subStr, includeDcs, resTstHtmlLS, SuccGetSearchResult, OnException);
    } catch (exp) {
        AxWaitCursor(false);
        ShowDimmer(false);
        var execMess = exp.name + "^♠^" + exp.message;
        showAlertDialog("error", 2030, "client", execMess);
    }
}

// function to construct td id
function GetTdFrameNo() {
    var hdn = $j("#hdnPickFldId");
    var objId = hdn.val();
    var divFrameNo = GetFieldsDcNo(objId);
    return divFrameNo;
}
var pickStatus = true;
var selectedRow = 0;
//Success function which parses the result and dynamically creates inner html for the picklist div.
function SuccGetSearchResult(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    var hdnFilter = $j("#hdnFiltered");
    var resultArr;
    hdnFilter.val("true");

    if (result != "" && result.substring(0, 7) != "<error>") {
        TogglePrevNextLink("inline");
        var tableStr = "";
        if (result.indexOf("♣") != -1) {
            var totRowStr = result.split("♣");
            totalPLRows = parseInt(totRowStr[0], 10);
            if ((totalPLRows / pageSize) % 1 > 0)
                noOfPLPages = Math.floor(totalPLRows / pageSize) + 1;
            else
                noOfPLPages = totalPLRows / pageSize;
            resultArr = totRowStr[1].split("¿");
        } else {
            resultArr = result.split("¿");
        }

        var dv = $j("#dvPickHead");

        if (resultArr != undefined && resultArr != "") {
            tableStr = "<table id='tblPickData'  data-pick='" + currentPickList + "' class='pickGridData'>";
            for (var i = 0; i < resultArr.length; i++) {
                var tdId = "axPickTd00" + (i + 1) + 'F' + GetTdFrameNo();
                var pickValue = CheckSpecialCharsInHTML(resultArr[i]);
                if (resultArr[i].toString().indexOf("^") != -1) {
                    var displayText = resultArr[i].toString().split('^')[1];

                    tableStr += "<tr><td id =" + tdId + " onclick='javascript:SetPickVal(\"" + pickValue + "\")' class='handCur'><a>" + displayText + "</td></tr>";
                } else {
                    displayText = resultArr[i].toString();
                    if (pickValue.indexOf("\\") != -1) {
                        pickValue = pickValue.replace("\\", "\\\\");
                        tableStr += "<tr><td id =" + tdId + " onclick='javascript:SetPickVal(\"" + pickValue + "\")' class='handCur' ><a>" + displayText + "</td></tr>";
                    } else {
                        tableStr += "<tr><td id =" + tdId + " onclick='javascript:SetPickVal(\"" + pickValue + "\")' class='handCur'><a>" + displayText + "</td></tr>";
                    }
                }
            }

            if (dv.length > 0)
                dv.html(tableStr);

            if ($j("#tblPickData tr").length > 0) {
                $j("#tblPickData tr:nth-child(1)").addClass('active');
            }

        } else {
            if (dv.length > 0) {
                var cutMsg = eval(callParent('lcm[0]'));
                dv.html("<span>" + cutMsg + "</span>");
            }
        }
        SetPrevNextLinks();
        document.getElementById('advancebtn').style.visibility = 'visible';
        document.getElementById('advancesrch').style.visibility = 'visible';
    } else {
        tableStr = "<table style='width:100%;height:auto;' id='tblPickData' cellpadding='1' cellspacing='1'>";
        //tableStr += "<tr><td></td><td align='right' class='hdrRow'><img class='curHand' src='../AxpImages/icons/close-button.png' alt='Close' onclick=\"javascript:HidePLDiv(true);\"/></td></tr>";
        tableStr += "</table>";
        document.getElementById('advancebtn').style.visibility = 'hidden';
        document.getElementById('advancesrch').style.visibility = 'hidden';
        var cutMsg = eval(callParent('lcm[0]'));
        tableStr += "<span style=\"font-size: 12px;\">" + cutMsg + "</span>";
        TogglePrevNextLink("none");
        var dv = $j("#dvPickHead");
        if (dv.length > 0)
            dv.html(tableStr);
    }
    pickListRowCount = $j("#tblPickData tbody tr").length;
    if (pickListRowCount > 1) {
        $j(".inputClass2").keydown(function (e) {
            if (e.which == 40) { // down arrow
                if (selectedRow < pickListRowCount) {
                    if ($j("#tblPickData tr:nth-child(" + selectedRow++ + ")").length == 0)
                        selectedRow = 2;

                    $j("#tblPickData tr").removeClass("pickbg");
                    $j("#tblPickData tr:nth-child(" + selectedRow + ")").addClass("pickbg");

                    var totalHgt = 0;
                    var rowTotHgt = 0;
                    for (var i = 0; i < selectedRow; i++) {
                        rowTotHgt += $j("#tblPickData tr:nth-child(" + i + ")").height();
                    }
                    var dvHgt = 0;
                    dvHgt = $j('#dvPickHead').height();
                    if ((rowTotHgt + 20) > dvHgt) {
                        //$j('#dvPickHead').scrollTop((rowTotHgt - dvHgt) + 20);
                        $j('#dvPickHead').scrollTop($j('#dvPickHead')[0].scrollHeight);
                    }
                }
            } else if (e.which == 38) { // up arrow
                if (selectedRow > 2) {
                    if ($j("#tblPickData tr:nth-child(" + selectedRow-- + ")").length == 0)
                        selectedRow++;

                    $j("#tblPickData tr").removeClass("pickbg");
                    $j("#tblPickData tr:nth-child(" + selectedRow + ")").addClass("pickbg");

                    if (selectedRow == 1) {
                        e.preventDefault();
                    }
                    var totalHgt = 0;
                    var rowTotHgt = 0;
                    for (var i = pickListRowCount; i >= selectedRow; i--) {
                        rowTotHgt += $j("#tblPickData tr:nth-child(" + i + ")").height();
                    }
                    var dvHgt = 0;
                    dvHgt = $j('#dvPickHead').height();
                    if ((rowTotHgt + 10) > dvHgt) {
                        var tmpRowNo = selectedRow - 1;
                        $j('#dvPickHead').scrollTop($j("#tblPickData tr:nth-child(" + tmpRowNo + ")").height());

                    }
                }
            } else if (e.which == 39) { // right arrow
                if ($j('#nextPick').attr("onclick") != undefined && $j('#nextPick').attr("onclick") != "") {
                    GetData('next');
                    $j('#dvPickHead').scrollTop(0);
                    selectedRow = 1;
                }
            } else if (e.which == 37) { // left arrow
                if ($j('#prevPick').attr("onclick") != undefined && $j('#prevPick').attr("onclick") != "") {
                    GetData('prev');
                    selectedRow = 1;
                    $j('#dvPickHead').scrollTop(0);
                }
            } else if (e.which == 27 || e.which == 9) { // for Close the picklist
                HidePLDiv(false);
                selectedRow = 0;
            }
            e.which = -1;
        });
    }
    $j(document).keypress(function (e) {
        if (e.keyCode == -1)
            return;
        if (e.keyCode == 13) {
            var fldId = GetActivePickListId();
            var fieldDcNo = GetFieldsDcNo(fldId);
            var fieldRowNo = GetFieldsRowNo(fldId);
            AxActiveRowNo = GetDbRowNo(fieldRowNo, fieldDcNo);

            if ($j("#dvPickList").is(':visible')) {
                var tdId = "#axPickTd00" + --selectedRow + 'F' + GetTdFrameNo();
                if ($j(tdId).length > 0) {
                    var selcteditm = $j(tdId).find('a')[0].innerText;
                    if (selcteditm == undefined)
                        selcteditm = $j(tdId).find('a')[0].textContent;
                    SetPickVal(selcteditm);
                    pickStatus = false;
                }
                selectedRow = 0;
            }
        }
        e.which = -1;
    });
    $j(document).click(function (e) {
        if (e.target.id != 'nextPick' && e.target.id != 'prevPick' && e.target.className != 'curHand' && e.target.className != 'hdrRow') {
            //show picklist next/prev loader
            if (e.target.parentElement.id == 'nextPick' || e.target.parentElement.id == 'prevPick') {
                $j("#pickDimmer").css("display", "block");
            } else {
                HidePLDiv(false);
            }

            selectedRow = 0;
        }
    });
    currentPickList = "";
    ShowPickList();
    ShowDimmer(false);
    AxWaitCursor(false);
}

function GetActivePickListId() {
    var hdn = $j("#hdnPickFldId");
    var objId = hdn.val();
    var fldProps = objId.split("~");
    var i;
    if (fldProps.length > 1)
        i = 1;
    else
        i = 0;

    var fldName = fldProps[i];
    return fldName;
}

function HidePLDiv(setFocus) {

    if (setFocus == true) {
        var hdn = $j("#hdnPickFldId");
        var objId = hdn.val();
        if (objId != "" && objId.indexOf("~") != -1) {
            var fldProps = objId.split("~");
            var fldName = "#" + fldProps[1];
            var fld = $j(fldName);
            fld.focusNextInputField();
        }
    }

    var dv = $j("#dvPickList");
    if (dv.length > 0)
        dv.hide();

}


$j.fn.focusNextInputField = function () {
    return this.each(function () {
        setTimeout(function () {
            var focusElem = $(this);
            if (axInlineGridEdit && !$($(focusElem).closest("div")).hasClass("form-group"))
                var fields = $j(focusElem).parents('input:eq(0),body').find('button,input[type!="hidden"],textarea,select');
            else
                var fields = $j(focusElem).parents('form:eq(0),body').find('button,input[type!="hidden"],textarea,select');
            var index = fields.index(focusElem);
            if (index > -1 && (index + 1) < fields.length) {

                var fld = fields.eq(index + 1); //togCol
                var isDisabled = fld.prop("disabled");
                if (isDisabled == true || isDisabled == "disabled" || fld.prop("class") == "togCol") {
                    fields.eq(index + 1).focusNextInputField();
                } else {
                    if (axInlineGridEdit && !$(focusElem.closest("div")).hasClass("form-group")) {
                        var dataindex = fields.eq(index).closest("td").attr("data-focus-index");
                        var lastFocusIndex = fields.eq(index).closest("tr").attr("last-focus-index");
                        if (lastFocusIndex != dataindex) {
                            dataindex = parseInt(dataindex) + 1;
                        }
                        $j(focusElem).parents('input:eq(0),body').find("td[data-focus-index='" + dataindex + "']").find("input").focus();

                    } else if (!isMobile)
                        fields.eq(index + 1).focus();
                }
            }
            return false;
        }, 0);
    });
};

//Function to get the next or prev page records in picklist dropdown.
function GetData(str) {

    if (str == "prev" && curPageNo > 1) {
        curPageNo = curPageNo - 1;
    } else if (str == "next" && curPageNo < noOfPLPages) {
        curPageNo = curPageNo + 1;
    }

    var hdn = $j("#hdnPickFldId");
    if (hdn.length > 0) {
        var objId = hdn.val();
        if (objId != "" && objId.indexOf("~") != -1) {
            var fldProps = objId.split("~");
            var fldName = fldProps[1].substring(0, fldProps[1].lastIndexOf("F") - 3);
            var fldValue = $j("#" + fldProps[1]).val();

            GetPickListData(fldName, fldValue, curPageNo.toString(), pageSize.toString(), fldProps[1]);
        }
    }
}

//TO hide or show the next and prev buttons in the picklist div.
function TogglePrevNextLink(status) {
    var prev = $j("#prevPick");
    var next = $j("#nextPick");

    if (status == "block" || status == "inline") {
        prev.show();
        next.show();
    } else {
        prev.hide();
        next.hide();
    }
}

//Function which decides to display the next and prev button in the picklist div.
function SetPrevNextLinks() {
    var prev = $j("#prevPick");
    var next = $j("#nextPick");
    var tblPick = $j("#tblPickData");

    if (curPageNo == 1) {
        prev.attr("disabled", "disabled");
        prev.css("display", "none");
        prev.removeAttr("onclick");
    } else {
        prev.attr("disabled", false);
        prev.css("color", "black");
        next.css("color", "black");
        prev.attr("onclick", "javascript:GetData('prev')");
    }

    if (curPageNo < noOfPLPages) {
        next.attr("disabled", false);
        next.css("color", "black");
        next.attr("onclick", "javascript:GetData('next')");
    } else if (curPageNo >= noOfPLPages) {
        next.attr("disabled", "disabled");
        next.css("display", "none");
        next.removeAttr("onclick");
    } else {
        tblPick.attr("disabled", "disabled");
        tblPick.removeAttr("onclick");

    }

}

//Function to set the selected value in the picklist field.
function SetPickVal(pickVal) {
    var hdn = $j("#hdnPickFldId");
    if (hdn) {
        var objId = hdn.val();
        var fldProps = objId.split("~");
        var i;
        if (fldProps.length > 1)
            i = 1;
        else
            i = 0;

        var fldName = fldProps[i];
        var fld = $j("#" + fldName);
        var pickFld = $j("#pickfld000F0");
        pickFld.val(fldName);
        if (pickVal.indexOf("^") != -1) {
            var rowFrmNo = GetFieldsRowFrameNo(fldName);
            var pickId = $j("#pickIdVal_" + fldName);
            var picStr = pickVal.split("^");
            pickId.val(picStr[0]);
            pickVal = picStr[1];
        }

        pickVal = RepSpecialCharsInHTML(pickVal);
        fld.val(pickVal);
        try {
            fld.focus();
        } catch (ex) { }

        AxFromAssociated = true;
    }

    HidePLDiv(false);
}


//Function to open the old picklist pop up on click of Advanced link in the picklist div.
function CallSearchOpen() {
    //Not to enter in mainfocus twice in chrome
    if (window.chrome)
        $j("#HdnAxAdvPickSearch").val("true");
    ShowDimmer(true);
    AxWaitCursor(true);
    HidePLDiv(false);
    if ($j("#hdnPickFldId")) {
        var fldId = $j("#hdnPickFldId").val();
        var imgObj = $j("#" + fldId);
        $j(".pickimg").each(function () {
            if ($j(this).attr("id") == fldId) {
                imgObj = $j(this);
            }
        });

        if (imgObj.length > 0) {
            SearchOpen(imgObj);
        }
    }
    AxWaitCursor(false);
}


//<Module>  FormControl </Module>
//<Author>  Naveen  </Author>
//<Description> Function to change the property of the dependency controls based on the paricular action of a field </Description>
//<Return> Changes the property of the dependency control </Return>
// 1 - Enable Field, 2 - Disable Field, 3 - SetValue, 4 - Show DC, 5 - Hide DC(its not really hide, rather disable,
// 6 - Enable Field and Editable, 7 - Enable Field but not editable? TODO: check this case
// 8 - Hide (DC or Field), 9 - Show (DC or Field)
// 10 - hidepopdc, 11 - showpopdc
var fcontrols = new Array();
var found = false;
var process = false;
var fc = 0;
var ch, strch, cmdfchar;
var cresult = true;
var cond = '';
var skip;
var fld;
var count = 0;
var cont = false;
var fldExtn = "";
var isobject = false;
var btnObject = false;
var firstFldDc = "NONGRID";
var secFldDc = "NONGRID";
var isFieldBtn = false;
var arrVisibleTabDcs = new Array();

function DoFormControl(componentName) {

    var v;
    var fName = GetFieldsName(componentName);
    var dcNo = GetFieldsDcNo(componentName);
    var rowNo = GetFieldsRowNo(componentName);
    var isGrid = IsDcGrid(dcNo);
    fldExtn = rowNo + "F" + dcNo;
    if (isGrid)
        firstFldDc = "GRID";
    else
        firstFldDc = "NONGRID";


    if (fcontrols.length == 0) {

        for (var m = 0; m < Formcontrols.length; m++) {
            var s = Formcontrols[m];
            if (s.startsWith('f')) {
                v = s.substr(1, s.length);
                fcontrols[fc] = v;
                fc++;
            }
        }
    }

    found = false;
    for (var k = 0; k < fcontrols.length; k++) {
        if (fcontrols[k] == fName) {
            found = true;
            break;
        }
    }
    process = false;

    if (found) {
        EvaluateFormControl(fName);
    }
}

function AddVisTabDcsInArray() {
    var cnt = PagePositions.length;
    for (var i = 0; i < cnt; i++) {
        arrVisibleTabDcs.push("");
    }
}

function DoFormControlOnload() {
    var isgrid = false;

    if (fcontrols.length == 0) {
        for (var m = 0; m < Formcontrols.length; m++) {
            var s = Formcontrols[m];
            if (s.startsWith('f')) {
                v = s.substr(1, s.length);
                fcontrols[fc] = v;
                fc++;
            }
        }
    }

    try {
        var rId = $j("#recordid000F0").val();
        if (rId == "") rId = "0";
        for (var k = 0; k < fcontrols.length; k++) {
            if ((rId == "0" && fcontrols[k] == "_On Form Load") || (rId != "0" && fcontrols[k] == "_On Data Load"))
                EvaluateFormControl(fcontrols[k]);

        }
    } catch (ex) {
        console.log(ex.message);
    }
}

function EvaluateFormControl(fName) {



    for (var j = 0; j < Formcontrols.length; j++) {

        ch = Formcontrols[j];
        strch = ch;
        ch = ch.split('');
        if (strch.startsWith("10") || strch.startsWith("11") || strch.startsWith("12"))
            cmdfchar = ch[0] + ch[1];
        else
            cmdfchar = ch[0];
        if (cmdfchar == "1" && ch.length > 1 && (ch[1] == "0" || ch[1] == "1") && ch[2] == "d" && ch[3] == "c") {
            cmdfchar = ch[0] + ch[1];
            fldname = strch.substr(2, strch.length);
        } else if (cmdfchar == "10" || cmdfchar == "11" || cmdfchar == "12") {
            fldname = strch.substr(2, strch.length);
        } else {
            fldname = strch.substr(1, strch.length);
        }

        if ((cmdfchar == 'f') && (fldname == fName)) {
            process = true;
            continue;
        }

        if ((cmdfchar == 'e') && (process == true)) {
            process = false
            continue;
        }

        if (process == false)
            continue;

        var IndexValue = 0;
        if ((cmdfchar == 3) && (cresult == true)) {
            j = j + 1;
            var result = Formcontrols[j];
            result = result.substring(1, result.length);
            if (result == "''") result = "";
            var ch = result.split('');
            var got = "No";
            if (ch[0] == ":") {
                got = "yes";
                result = result.substring(1, result.length);
                result = GetComponentName(GetExactFieldName(result), fldExtn);
                var srcFld = $j("#" + result);

                if (srcFld.length > 0) {
                    result = GetFieldValue(result);
                } else
                    continue;
            }
            CallProcessFormControl(fldname, fldExtn, "3", result);
        } else if ((cmdfchar == 12) && (cresult == true)) {
            j = j + 1;
            var result = Formcontrols[j];
            result = result.substring(2, result.length);
            if (result == "''") result = "";
            var ch = result.split('');
            var got = "No";
            if (ch[0] == ":") {
                got = "yes";
                result = result.substring(1, result.length);
                result = GetComponentName(GetExactFieldName(result), fldExtn);
                var srcFld = $j("#" + result);

                if (srcFld.length > 0) {
                    result = GetFieldValue(result);
                } else
                    continue;
            }
            CallProcessFormControl(fldname, fldExtn, "12", result);
        } else if ((cmdfchar == 1 || cmdfchar == 2 || cmdfchar == 4 || cmdfchar == 5 || cmdfchar == 6 || cmdfchar == 7 || cmdfchar == 8 || cmdfchar == 9 || cmdfchar == 10 || cmdfchar == 11) && (cresult == true)) {

            CallProcessFormControl(fldname, fldExtn, cmdfchar.toString());
        } else if (cmdfchar == 'c') {

            if (fldname == 'if') {
                count = 0;
                try {
                    cresult = Getcondition(j);
                    skip = cresult;

                    if (skip)
                        count = 1;
                    j += 3;
                } catch (e) { }

            } else if (fldname == "elseif") {

                if (skip) {
                    cresult = false;
                    count = 3;
                } else {
                    cresult = (!cresult);
                }
                if (cresult) {
                    cresult = Getcondition(j);
                    skip = cresult;

                    if (skip)
                        count = 2;
                }
                j += 3;
            } else if (fldname == 'else') {

                if (skip == true)
                    cresult = false;
                else
                    cresult = (!cresult);

                skip = cresult;

            } else if (fldname == 'and') {

                cresult = cresult && Getcondition(j);

                if (count == 1) {
                    if (count == 1 && cresult == false)
                        skip = false;
                }

                if (count == 2) {
                    if (count == 2 && cresult == true)
                        skip = true;
                    else
                        skip = false;
                }

                if (count == 3) {
                    skip = true;
                }
                j += 3;
            } else if (fldname == 'or') {
                //continue;
                cresult = cresult || Getcondition(j);

                if (count == 1) {
                    if (count == 1 && cresult == false)
                        skip = false;
                }

                if (count == 2) {
                    if (count == 2 && cresult == true)
                        skip = true;
                    else
                        skip = false;
                }

                if (count == 3) {
                    skip = true;
                }
                j += 3;

            } else if (fldname == 'end') {
                cresult = true;
                skip = false;
            }
        }
    }
}

//Function to get the component name of the field on which the formcontrol should be applied,
//and to call formcontrol based on the dc type.
function CallProcessFormControl(fldName, fldExtn, actionStr, fldValue) {

    if (actionStr == "4" || actionStr == "5" || (/^dc\d+$/.test(fldName.toLowerCase()) && (actionStr == "8" || actionStr == "9" || actionStr == "10" || actionStr == "11"))) {
        ProcessFormControl(fldName, actionStr, fldValue);
    } else {

        fldName = GetExactFieldName(fldName);
        var conFldDcNo = GetDcNo(fldName);
        var isGrid = IsDcGrid(conFldDcNo);
        if (isGrid)
            secFldDc = "GRID";
        else
            secFldDc = "NONGRID";


        if ((firstFldDc == "NONGRID" && secFldDc == "NONGRID") || (firstFldDc == "GRID" && secFldDc == "NONGRID")) {
            fldName = fldName + "000F" + conFldDcNo;
            ProcessFormControl(fldName, actionStr, fldValue);
        } else if (firstFldDc == "NONGRID" && secFldDc == "GRID") {
            var rCnt = GetDcRowCount(conFldDcNo);
            for (var i = 1; i <= rCnt; i++) {
                var newrow = GetRowNoHelper(i);
                var tmpFldName = fldName + newrow + "F" + conFldDcNo;
                ProcessFormControl(tmpFldName, actionStr, fldValue);
            }
        } else if (firstFldDc == "GRID" && secFldDc == "GRID") {
            var clientRowNo = GetClientRowNo(AxActiveRowNo, conFldDcNo);
            fldName = fldName + clientRowNo + "F" + conFldDcNo;
            ProcessFormControl(fldName, actionStr, fldValue);
        } else {
            fldName = fldName;
            ProcessFormControl(fldName, actionStr, fldValue);
        }
    }
}

//Function to perform SetValue or fieldAccess on the given field based on the command.
function ProcessFormControl(fld, actionStr, fldValue) {

    isFieldBtn = false;
    if (fld.indexOf(".") != -1)
        fld.replace(".", "\\.");

    var destfld = $j("#" + fld);
    if (actionStr == "4" || actionStr == "5" || (/^dc\d+$/.test(fld.toLowerCase()) && (actionStr == "8" || actionStr == "9" || actionStr == "10" || actionStr == "11"))) {
        var dcName = fld.substr(2);
        destfld = $j("#DivFrame" + fld.toString().substring(2));
        if (destfld.length == 0) {
            if ($j.inArray(dcName, TabDCs) != -1) {
                destfld = $j("#ank" + dcName);
            }
        }
    }
    var fldRowNo = GetFieldsRowNo(fld);
    var fldDcNo = GetFieldsDcNo(fld);
    var fldDbRowNo = GetDbRowNo(fldRowNo, fldDcNo);

    var isFldSaveBtn = false;

    if (destfld.length <= 0) {
        //check if the field is button, by removing the rowno and dc number
        var newFldName = fld;
        if (fld.toString().indexOf("F") != -1)
            newFldName = fld.substring(0, fld.lastIndexOf("F") - 3);

        $j(".axpBtn,.axpBtnCustom").each(function () {
            if ($j(this).attr("id") == newFldName) {
                destfld = $j(this);
                isFieldBtn = true;
                return false;
            }
        });

        var actTmpBtn;
        $j(".action img").each(function () {
            if ($j(this).attr("id") == newFldName) {
                actTmpBtn = $j(this);
                isFieldBtn = true;
                return false;
            }
        });

        //AxpTstBtn
        var actTmpBtn;
        var isBtnInDc = false;
        $j(".AxpTstBtn input,img").each(function () {
            if ($j(this).attr("value") == newFldName || $j(this).attr("id") == "AxpTstBtn_" + newFldName) {
                actTmpBtn = $j(this);
                isFieldBtn = true;
                isBtnInDc = true;
                return false;
            }
        });

        if (newFldName.toLowerCase() == "remove")
            newFldName = "delete";
        else if (newFldName.toLowerCase() == "list view")
            newFldName = "listview";
        else if (newFldName.toLowerCase() == "new")
            newFldName = "add";

        if (newFldName.toLowerCase() == "save")
            isFldSaveBtn = true;

        var sfnewFldName = newFldName;
        if (sfnewFldName.toLowerCase() == "listview")
            sfnewFldName = "list view";
        $j("#icons").find("a").each(function () {
            if (typeof $j(this).attr("title") != "undefined" && $j(this).attr("class") == newFldName.toLowerCase() || (typeof btnName !== 'undefined' && $j(this).text() == btnName)) {
                destfld = $j(this);
                isFieldBtn = true;
                return false;
            } else if (typeof $j(this).attr("title") != "undefined" && $j(this).attr("title").toLowerCase() == sfnewFldName.toLowerCase() || (typeof btnName !== 'undefined' && $j(this).text() == btnName)) {
                destfld = $j(this);
                isFieldBtn = true;
                return false;
            }
        });

        $j(".toolbarRightMenu").find("a").each(function () {
            if (typeof $j(this).attr("title") != "undefined" && $j(this).attr("title").toLowerCase() == sfnewFldName.toLowerCase() || (typeof btnName !== 'undefined' && $j(this).text() == btnName)) {
                destfld = $j(this);
                isFieldBtn = true;
                return false;
            }
        });

        //condition to check prev and next button in list view header
        if (!isFieldBtn) {
            $j("#nextprevicons").find("a").each(function () {
                if ($j(this).attr("class") == newFldName.toLowerCase()) {
                    destfld = $j(this);
                    isFieldBtn = true;
                }
            });
        }
        if (!isFieldBtn) {
            $j(".tstructMainBottomFooter").find("a").each(function () {
                if ($j(this).attr("id").toLowerCase() == newFldName.toLowerCase()) {
                    destfld = $j(this);
                    isFieldBtn = true;
                }
            });
        }

        if (!isFieldBtn) {
            $(".toolbarRightMenu .menu-item a").each(function () {
                if (typeof $j(this).attr("id") != "undefined" && ($j(this).attr("id").toLowerCase() == newFldName.toLowerCase()) || (typeof $j(this).attr("title") != "undefined" && $j(this).attr("title").toLowerCase() == sfnewFldName.toLowerCase() || (typeof btnName !== 'undefined' && $j(this).text() == btnName))) {
                    destfld = $j(this);
                    isFieldBtn = true;
                }
            });
        }

        $j(".tstformbutton").each(function () {
            if ((typeof $j(this).attr("id") != "undefined" && $j(this).attr("id").toLowerCase() == newFldName.toLowerCase()) || (typeof $j(this).attr("value") != "undefined" && $j(this).attr("value").toLowerCase() == newFldName.toLowerCase())) {
                destfld = $j(this);
                isFieldBtn = true;
            }
        });


        if (actTmpBtn != undefined && actTmpBtn.length > 0)
            destfld = actTmpBtn.parent(0);
        if (isBtnInDc)
            destfld = actTmpBtn;
    }

    // 1 - Enable Field, 2 - Disable Field, 3 - SetValue, 4 - Show DC, 5 - Hide DC(its not really hide, rather disable,
    // 6 - Enable Field and Editable, 7 - Enable Field but not editable? TODO: check this case
    // 8 - Hide (DC or Field), 9 - Show (DC or Field)
    // 10 - hidepopdc, 11 - showpopdc
    if (destfld.length > 0) {
        switch (actionStr) {
            case ("1"):
                if (isFieldBtn) {
                    if (isFldSaveBtn)
                        EnableSaveBtn(true);
                    else
                        EnableDisableBtns(destfld, true);
                } else {

                    if (IsPickListField(destfld.attr("id")) == true) {
                        var pickFld = document.getElementById("img~" + destfld.attr("id"));
                        pickFld.disabled = false;
                        pickFld.className = "input-group-addon handCur pickimg";
                    }


                    if (destfld.val() == 0 && destfld.prop("type") != "select-one")
                        destfld.val("");

                    if (destfld.attr("title") == dateString && destfld.val() == "")
                        destfld.val(dateString);
                    if (destfld.attr("type") == "checkbox") {
                        var checlistid = destfld.attr("id");

                        EnableDisableCheckbox(checlistid, false)
                    } else {

                        // for enabling the Rich Text Box If it is disabled on dataload using form control
                        if (destfld.attr("id").startsWith("rtf_") || destfld.attr("id").startsWith("rtfm_") || destfld.attr("id").startsWith("fr_rtf_") || GetDWBFieldType(GetFieldsName(destfld.attr("id"))) == "Rich Text") {
                            $j("#cke_" + destfld.attr("id")).prop("disabled", false);
                            destfld.css("display", "none");
                            $j("#cke_" + destfld.attr("id")).removeAttr("disabled");
                        }
                        if (destfld.attr("class") == "axpImg") {
                            destfld.attr("onclick", null);
                        }
                        destfld.prop("disabled", false);
                        destfld.prop("readOnly", false);
                        destfld.removeAttr("readOnly");
                        SetAutoCompAccess("enabled", destfld);

                    }
                }
                break;
            case ("2"):
                if (isFieldBtn) {
                    if (isFldSaveBtn)
                        EnableSaveBtn(false);
                    else
                        EnableDisableBtns(destfld, false);
                } else {
                    if (IsPickListField(destfld.attr("id")) == true) {
                        var pickFld = document.getElementById("img~" + destfld.attr("id"));
                        pickFld.disabled = true;
                        pickFld.className = "pickimg input-group-addon handCur";
                    }
                    if (destfld.attr("title") == dateString && (destfld.val() == dateString || destfld.val() == "''"))
                        destfld.val("");



                    if (destfld.attr("type") == "checkbox") {
                        var checlistid = destfld.attr("id");
                        EnableDisableCheckbox(checlistid, true)
                    } else {

                        // for disabling the Rich Text Box If it is disabled on dataload using form control
                        if (destfld.attr("id").startsWith("rtf_") || destfld.attr("id").startsWith("rtfm_") || destfld.attr("id").startsWith("fr_rtf_") || GetDWBFieldType(GetFieldsName(destfld.attr("id"))) == "Rich Text") {
                            $j("#cke_" + destfld.attr("id")).attr('disabled', 'disabled');
                            destfld.css("display", "none");
                            $j("#cke_" + destfld.attr("id")).prop("readonly", true);
                        }
                        if (destfld.attr("class") == "axpImg") {
                            destfld.attr("onclick", "callnull");
                        }
                        try {
                            if (destfld.hasClass('selectTextArea')) {
                                var _thisdcNo = GetFieldsDcNo(destfld.attr("id"));
                                if (!IsDcGrid(_thisdcNo))
                                    destfld.prop("disabled", true);
                            } else
                                destfld.prop("disabled", true);
                        } catch (ex) {
                            destfld.prop("disabled", true);
                        }
                        //destfld.attr("readOnly", "readOnly");
                        SetAutoCompAccess("disabled", destfld);

                    }
                }
                break;
            case ("3"):
                var dcNo = GetDcNo(fldname);
                if ($j("#" + fld).hasClass("multiFldChk") && fldValue.trim() == "") {
                    if (IsDcGrid(dcNo)) {
                        $j("input[id=" + fld + "]").each(function () {
                            $j(this).removeAttr("checked");
                            $j(this).prop("checked", false);
                        });
                    } else {
                        try {
                            $("#" + fld).val("");
                            $("#" + fld).data("valuelist", "");
                            $("#" + fld).tokenfield('setTokens', []);
                        } catch (ex) { }
                    }
                } else {
                    CallSetFieldValue(fld, fldValue);
                }

                UpdateFieldArray(fld, fldDbRowNo, fldValue, "parent", "");
                break;
            case ("4"):
                /*if (fldname.substr(0, 2) == "dc") {*/
                if (/^dc\d+$/.test(fldname.toLowerCase())) {
                    var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                    ShowingDc(dcNo, "enable");
                }
                break;
            case ("5"):
                /* if (fldname.substr(0, 2) == "dc") {*/
                if (/^dc\d+$/.test(fldname.toLowerCase())) {
                    var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                    ShowingDc(dcNo, "disable");
                }
                break;
            //Enable
            case ("6"):
                if (isFieldBtn) {
                    if (isFldSaveBtn)
                        EnableSaveBtn(true);
                    else
                        EnableDisableBtns(destfld, true);
                } else {
                    if (IsPickListField(destfld.attr("id")) == true) {
                        var pickFld = document.getElementById("img~" + destfld.attr("id"));
                        pickFld.disabled = false;
                        pickFld.className = "input-group-addon handCur pickimg";
                    }
                    destfld.prop("disabled", false);
                    destfld.prop("readOnly", false);
                    if (!isFieldBtn)
                        destfld.removeClass("dis");
                    destfld[0].children[0].className = "handCur";
                }
                break;
            //Disable
            case ("7"):
                if (isFieldBtn) {
                    if (isFldSaveBtn)
                        EnableSaveBtn(false);
                    else
                        EnableDisableBtns(destfld, false);
                } else {
                    if (IsPickListField(destfld.attr("id")) == true) {
                        var pickFld = document.getElementById("img~" + destfld.attr("id"));
                        pickFld.disabled = true;
                        pickFld.className = "input-group-addon  pickimg";
                    }
                    destfld.prop("disabled", true);
                    destfld.prop("readOnly", false);
                    destfld.addClass("dis");
                    destfld[0].children[0].className = "";
                }
                break;
            case ("8"):
                /*if (fldname.substr(0, 2) == "dc") {*/
                if (/^dc\d+$/.test(fldname.toLowerCase())) {
                    var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                    if (typeof isWizardTstruct != "undefined" && isWizardTstruct)
                        ToggleWizardDc(dcNo, "hide");
                    else
                        ShowingDc(dcNo, "hide");
                } else {
                    try {
                        var _thisdcNo = GetDcNo(fldname);
                        if (_thisdcNo != "" && IsDcGrid(_thisdcNo)) {
                            var rowCnt = 0;
                            rowCnt = GetDcRowCount(_thisdcNo);
                            var eleType = getGridFldType(fldname, _thisdcNo);
                            for (var i = 1; i <= rowCnt; i++) {
                                destfld = $j("#" + fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo);
                                let _thisEleId = fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo;
                                if (destfld.length > 0) {
                                    var idx = $j.inArray(_thisdcNo + "~" + fldname + "~show", AxFormContSetFldActGrid);
                                    if (idx == -1) {
                                        idx = $j.inArray(_thisdcNo + "~" + fldname + "~hide", AxFormContSetFldActGrid);
                                        if (idx == -1)
                                            AxFormContSetFldActGrid.push(_thisdcNo + "~" + fldname + "~hide");
                                        else
                                            AxFormContSetFldActGrid[idx] = _thisdcNo + "~" + fldname + "~hide";
                                    } else {
                                        AxFormContSetFldActGrid[idx] = _thisdcNo + "~" + fldname + "~hide";
                                    }
                                }
                            }
                        }
                    } catch (ex) { }

                    HideShowField(fldname, "hide");

                    //  var fieldName = GetFieldsName(fieldID);
                    var fldInd = GetFieldIndex(fldname);
                    var fldDType = GetDWBFieldType("", fldInd);
                    if (fldInd > -1 && (fldname.startsWith("axptm_") || fldname.startsWith("axpdbtm_") || (fldDType != "" && fldDType.toLowerCase() == "time"))) {
                        //$(".ui-datepicker.ui-widget").css("display", "none");
                        $j("#" + destfld.attr("id") + " .tstOnlyTime").removeClass('hasDatepicker');
                        $j("#" + destfld.attr("id") + " .tstOnlyTime24hours").removeClass('hasDatepicker');
                    }
                }
                break;
            case ("9"):
                /*if (fldname.substr(0, 2) == "dc") {*/
                if (/^dc\d+$/.test(fldname.toLowerCase())) {
                    var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                    if (typeof isWizardTstruct != "undefined" && isWizardTstruct)
                        ToggleWizardDc(dcNo, "show");
                    else
                        ShowingDc(dcNo, "show");
                } else {
                    try {
                        var _thisdcNo = GetDcNo(fldname);
                        if (_thisdcNo != "" && IsDcGrid(_thisdcNo)) {
                            var rowCnt = 0;
                            rowCnt = GetDcRowCount(_thisdcNo);
                            var eleType = getGridFldType(fldname, _thisdcNo);
                            for (var i = 1; i <= rowCnt; i++) {
                                destfld = $j("#" + fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo);
                                let _thisEleId = fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo;
                                if (destfld.length > 0) {
                                    var idx = $j.inArray(_thisdcNo + "~" + fldname + "~hide", AxFormContSetFldActGrid);
                                    if (idx == -1) {
                                        idx = $j.inArray(_thisdcNo + "~" + fldname + "~show", AxFormContSetFldActGrid);
                                        if (idx == -1)
                                            AxFormContSetFldActGrid.push(_thisdcNo + "~" + fldname + "~show");
                                        else
                                            AxFormContSetFldActGrid[idx] = _thisdcNo + "~" + fldname + "~show";
                                    } else {
                                        AxFormContSetFldActGrid[idx] = _thisdcNo + "~" + fldname + "~show";
                                    }
                                }
                            }
                        }
                    } catch (ex) { }

                    HideShowField(fldname, "show");
                    //  var fieldName = GetFieldsName(fieldID);
                    var fldInd = GetFieldIndex(fldname);
                    var fldDType = GetDWBFieldType("", fldInd);
                    if (fldInd > -1 && (fldname.startsWith("axptm_") || fldname.startsWith("axpdbtm_") || (fldDType != "" && fldDType.toLowerCase() == "time"))) {
                        //$(".ui-datepicker.ui-widget").css("display", "block");
                        $j("#" + destfld.attr("id") + " .tstOnlyTime").addClass('hasDatepicker');
                        $j("#" + destfld.attr("id") + " .tstOnlyTime24hours").addClass('hasDatepicker');
                    }
                }
                break;
            case ("10"):
                /*if (fldname.substr(0, 2) == "dc") {*/
                if (/^dc\d+$/.test(fldname.toLowerCase())) {
                    var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                    ShowingDc(dcNo, "hidepopdc");
                }
                break;
            case ("11"):
                /*if (fldname.substr(0, 2) == "dc") {*/
                if (/^dc\d+$/.test(fldname.toLowerCase())) {
                    var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                    ShowingDc(dcNo, "showpopdc");
                }
                break;
            case ("12"):
                var dcNo = GetDcNo(fldname);
                if (!IsDcGrid(dcNo)) {
                    if ($j("#" + fld).attr("type") == "checkbox")
                        $("label[for='" + fld + "'] span").text(fldValue);
                    else
                        $("label[for='" + fld + "']").text(fldValue);
                } else {
                    $("#gridHd" + dcNo + " th[id='th-" + fldname + "']").text(fldValue);
                    $("#wrapperForEditFields" + dcNo + " label[for='" + fld + "']").text(fldValue);
                }
                break;
            default:
                return;
        }
    } else {
        EnableDisableField(fld, actionStr, '0');

        //If the Dc to be enabled or disbaled is not available,
        //it is assumed to be a tab dc and the dc no along with the action is captured in the array.
        if ((actionStr == "4" || actionStr == "5") && /^dc\d+$/.test(fldname.toLowerCase())) {
            var actn = "";
            if (actionStr == "4")
                actn = "enable";
            else
                actn = "disable";

            var dcNo = parseInt(fldname.substr(2, fldname.length), 10);

            var idx = $j.inArray(dcNo + "~" + actn, DisabledDcs);
            if (idx == -1)
                DisabledDcs.push(dcNo + "~" + actn);
        }
    }
    // if (typeof isWizardTstruct != "undefined" && isWizardTstruct)
    //     CheckWizardSaveButton();
}


function EnableDisableCheckbox(listID, Value) {
    $j("#chkAll_" + listID).prop("disabled", Value);
    $j("#chkAll_" + listID).attr("readOnly", Value);
    $j("input:checkbox[id='" + listID + "']").each(function () {
        $j(this).prop("disabled", Value);
        $j(this).attr("readOnly", Value);
    });

}

//If the field to be enabled or disable and if not avilabe adding these fields to an array
function EnableDisableField(fldName, action, status) {

    if (status == "1") {
        ProcessFormControl(fldName, action, '')
    } else {
        var idx = $j.inArray(fldName + "~" + action, AxFormContDisableFlds);
        if (idx == -1)
            AxFormContDisableFlds.push(fldName + "~" + action);
    }
}



function HideShowField(fldName, action) {
    var dcNo = GetDcNo(fldName);
    var fld = "";


    if (IsDcGrid(dcNo)) {
        if (action == "hide") {
            $j("#th-" + fldName).hide();
            $j("#tf-" + fldName).addClass('d-none');
        }
        else {
            $j("#th-" + fldName).show();
            $j("#tf-" + fldName).removeClass('d-none');
        }
        var rowCnt = 0;
        rowCnt = GetDcRowCount(dcNo);
        try {
            let _fldIndex = GetFieldIndex(fldName);
            if (action == "hide") {
                FFieldHidden[_fldIndex] = "True";
            } else
                FFieldHidden[_fldIndex] = "False";
        } catch (ex) { }
        var eleType = getGridFldType(fldName, dcNo)
        for (var i = 1; i <= rowCnt; i++) {
            fld = $j("#" + fldName + GetClientRowNo(i, dcNo) + "F" + dcNo);
            if (fld.length > 0) {

                if (action == "hide") {
                    //fld.hide().attr("data-type", eleType);
                    fld.hide();
                    inputValue.css("display", "none");
                    fld.parent().hide();
                    var dataStyle = fld.attr("data-style");
                    if (dataStyle != undefined)
                        fld.attr("data-style", dataStyle.indexOf("display: inline") > 0 ? dataStyle.replace("display: inline;", "display: none;") : (dataStyle.indexOf("display: none") > 0 ? dataStyle : (dataStyle + "display: none;")));
                    if ($j("#dvGrid" + fldName).length)
                        $j("#dvGrid" + fldName).hide();
                    if (fld.closest(".gridElement").length)
                        fld.closest(".gridElement").hide();
                    if (!fld.parent().is('td') && fld.parent().parent().length > 0) {
                        if (fld.parent().parent().is('td'))
                            fld.parent().parent().hide();
                        else if (fld.parent().parent().parent().is('td'))
                            fld.parent().parent().parent().hide();
                    }
                } else {
                    //fld.show().attr("data-type", eleType);
                    fld.show();
                    fld.parent().show();
                    inputValue.css("display", "inline");
                    var dataStyle = fld.attr("data-style");
                    if (dataStyle != undefined)
                        fld.attr("data-style", dataStyle.indexOf("display: inline") > 0 ? dataStyle : (dataStyle + "display: inline;"))
                    if ($j("#dvGrid" + fldName))
                        $j("#dvGrid" + fldName).show();
                    if (fld.closest(".gridElement").length)
                        fld.closest(".gridElement").show();
                    if (!fld.parent().is('td') && fld.parent().parent().length > 0) {
                        if (fld.parent().parent().is('td'))
                            fld.parent().parent().show();
                        else if (fld.parent().parent().parent().is('td'))
                            fld.parent().parent().parent().show();
                    }
                }
            } else {
                var idx = $j.inArray(fldName + "~" + action, AxFormContHiddenFlds);
                if (idx == -1)
                    AxFormContHiddenFlds.push(fldName + "~" + action);
            }
        }
        // SetGridHeaderWidth(dcNo);
    } else {
        fld = $j("#dv" + fldName);
        try {
            let _fldIndex = GetFieldIndex(fldName);
            if (action == "hide") {
                FFieldHidden[_fldIndex] = "True";
            } else
                FFieldHidden[_fldIndex] = "False";
        } catch (ex) { }
        if (fld.length > 0) {
            if (action == "hide")
                fld.addClass("d-none").parents(".grid-stack-item").addClass("d-none");
            else {
                fld.removeClass("d-none").parents(".grid-stack-item").removeClass("d-none");
                var fldInput = fld.find(':input');
                if (fldInput.css('visibility') == 'hidden') { }
                fldInput.css('visibility', '');
            }
        } else {
            var idx = $j.inArray(fldName + "~" + action, AxFormContHiddenFlds);
            if (idx == -1)
                AxFormContHiddenFlds.push(fldName + "~" + action);
        }
    }
}
var inputValue = "";

function getGridFldType(fldName, dcNo) {

    var rc = $("#gridHd" + dcNo + " tbody tr").length == 0 ? 1 : $("#gridHd" + dcNo + " tbody tr").length;
    inputValue = $j("#" + fldName + GetClientRowNo(rc, dcNo) + "F" + dcNo);
    var currentElement = inputValue.closest(".gridElement");
    eleType = "";
    if (currentElement.find('span.picklist').length > 0) {
        eleType = 'pickList';
        hiddenData = currentElement.find('span.picklist input[type = hidden]').attr('id') + '~' + currentElement.find('span.picklist input[type = hidden]').val();
    } else if (currentElement.find('input.grdAttach').length > 0) {
        eleType = 'gridattach';
        isAttachMentExist = true;
    } else if (currentElement.find('input.date').length > 0) {
        eleType = 'datepicker';
    } else if (inputValue.hasClass('fldFromSelect')) {
        inputValue.hasClass('fastdll') ? eleType = 'fromselect-select' : eleType = 'fromselect-pick';
        eleType = inputValue.hasClass('isrefreshsave') ? eleType + "~isrefreshsave" : eleType;

    } else {
        eleType = inputValue[0].nodeName.toLowerCase();
        (eleType == "input" && inputValue[0].type == "checkbox") ? eleType = "checkbox" : "";
        (eleType == "input" && inputValue.hasClass('number')) ? eleType = "numeric" : "";
    }
    return eleType;
}


//<Module>  FormControl </Module>
//<Author>  Naveen  </Author>
//<Description> Function to evaluate the if else condition statement </Description>
//<Return> Returns the evaluated result </Return>
var s;

function Getcondition(i) {

    var isfld = false;
    var expFldname1 = "";
    var fieldVal = '';
    s = Formcontrols[i + 1];
    var firstChar = s.substring(0, 1);
    if (firstChar == ":")
        s = s.substring(1, s.length);
    s = GetExactFieldName(s);
    fld = GetComponentName(s, fldExtn);

    cond = GetCondFldValue(fld);
    if (cond == "") cond = "''";

    var op = Formcontrols[i + 2];
    if (op == "et")
        op = "==";
    if (op == "net")
        op = "!=";
    if (op == "gt")
        op = ">";
    if (op == "lt")
        op = "<";
    if (op == "gtoet")
        op = ">=";
    if (op == "ltoet")
        op = "<=";

    cond = cond + op;
    //TODO:Need to handle the condition (:fieldname = :fieldname)
    s = Formcontrols[i + 3];
    var firstChar = s.substring(0, 1);
    if (firstChar == ":") {
        s = s.substring(1, s.length);
        s = GetExactFieldName(s);
        s = GetComponentName(s, fldExtn);
        s = GetCondFldValue(s);
    }
    s = TrimAll(s);
    s = s.replace(/\s/g, "");
    if (isNaN(s)) {
        s = s.replace(/\"/g, "");
        s = '"' + s + '"';
    }

    cond = cond + s;
    result = eval(cond);
    return result;
}

//
function GetComponentName(fieldName, fldExtn) {

    var fldDcNo = GetDcNo(fieldName);
    var isGrid = IsDcGrid(fldDcNo);

    if (fldExtn == "") {
        if (isGrid)
            fld = fieldName + "001F" + fldDcNo;
        else
            fld = fieldName + "000F" + fldDcNo;
    } else {
        var fld = fieldName + fldExtn;
    }

    if (!document.getElementById(fld)) {
        if (isGrid)
            fld = fieldName + "001F" + fldDcNo;
        else
            fld = fieldName + "000F" + fldDcNo;
    }
    return fld;
}

//Function to fetch the value of the field in condition, from the field or parameters list.
function GetCondFldValue(fld) {
    var isfld = $j("#" + fld);
    var fieldVal = "";

    if (isfld.length > 0) {

        fieldVal = GetFieldValue(fld);
    } else {

        var w = 0;
        if (Parameters.length > 1) {
            for (var pki = 1; pki < Parameters.length; pki++) {
                var list = Parameters[pki].toString();
                list = list.split("~");
                varlist[w] = list[0].toString();
                valuelist[w] = list[1].toString();
                w++;
            }
        }

        for (var ij = 0; ij < varlist.length; ij++) {

            if (s == varlist[ij]) {
                fieldVal = valuelist[ij];
                break;
            }
        }
    }

    if (fieldVal != undefined) {
        fieldVal = fieldVal.replace(/\s/g, "");
    } else {
        fieldVal = "''";
    }
    // fixed for the "dd/mm/yyyy" to be considered as empty value.
    if (fieldVal == dateString) {
        fieldVal = "''";
    }

    if (isNaN(fieldVal))
        fieldVal = '"' + fieldVal + '"';

    return fieldVal;
}


//<Module>  FormControl </Module>
//<Author>  Naveen  </Author>
//<Description> Function to hide/show the DC control </Description>
//<Return> hide/show the DC control based on the action </Return>
function ShowingDc(a, x, calledFrom) {
    try {
        AxBeforeShowingDc(a, x);
    } catch (ex) { }
    var isGrid = IsDcGrid(a);
    var adddcno = parseInt(a, 10);

    var isHidden = 'visible';
    var isDisplay = 'block';
    var classDcBut = "glyphicon glyphicon-chevron-down icon-arrows-down";
    var imgAlt = "show";
    var cursorStyle = 'hand';
    var disable = "true";
    x = x.toString().toLowerCase();

    if (x == "hide") {

        isHidden = 'hidden';
        isDisplay = 'none';
        classDcBut = "glyphicon glyphicon-chevron-up icon-arrows-up";
        imgAlt = "show";
        cursorStyle = 'default';
        disable = "true"
    } else if (x == "show") {

        var dcIdx = GetTabDcIndexPagePos(adddcno);
        if (dcIdx != -1) {
            if (arrVisibleTabDcs[dcIdx] == "") {
                arrVisibleTabDcs[dcIdx] = (adddcno);
                callGetTab = true;
            } else if (arrVisibleTabDcs[dcIdx] == (adddcno)) {
                arrVisibleTabDcs[dcIdx] = (adddcno);
                callGetTab = true;
            }
            else
                callGetTab = false;
        }
        isHidden = 'visible';
        isDisplay = 'block';
        classDcBut = "glyphicon glyphicon-chevron-down icon-arrows-down";
        imgAlt = "hide";
        cursorStyle = 'hand';
        disable = "false";
    }

    var dcBut = $j("#dcButspan" + a);
    if (dcBut.length > 0) {
        dcBut.attr("alt", imgAlt);
        dcBut.attr("class", classDcBut);
        dcBut.css("visibility", isHidden);
        dcBut.css("cursor", cursorStyle);
    }

    $j(".fbutton").each(function () {
        if (x == "hide") {
            $j(this).prop("disabled", true);
        } else {
            $j(this).prop("disabled", false);
        }
    });

    var dvFrame = $j("#DivFrame" + a);
    var thisFldId = "";
    if (dvFrame.length > 0) {
        if (x == "showpopdc" || x == "hidepopdc") {
            //calling popupdcdialog func
            createPopDcDialog(a, dvFrame);
            if (typeof fld != "undefined" || fld != null || fld != "") {
                thisFldId = fld;
            } else {
                if (parseInt(dcno, 10) > 0) {
                    let _dcInx = $j.inArray(dcno, DCFrameNo);
                    if (DCIsGrid[_dcInx].toLowerCase() == "true") {
                        rowNo = GetRowNoHelper(AxActiveRowNo);
                        thisFldId = AxActiveField + rowNo + "F" + GetDcNo(AxActiveField);;
                    } else {
                        rowNo = "000";
                        thisFldId = AxActiveField + rowNo + "F" + GetDcNo(AxActiveField);;
                    }
                }
            }
        }
        if (x == "show") {
            // dvFrame.show();
            dvFrame.removeClass("d-none");
            if ($("#gridHd" + a).length > 0 && $("#gridHd" + a + " tbody tr").hasClass('inline-edit')) {
                updateInlineGridRowValues();
                $("#gridHd" + a + " tbody tr").removeClass('inline-edit');
            } else if ($("#gridHd" + a).length > 0 && !$("#gridHd" + a + " tbody tr").hasClass('inline-edit') && $("#gridHd" + a + " tbody tr").find(".dropzone").length > 0) {
                try {
                    DropzoneInit("#divDc" + a);
                    DropzoneGridInit("#divDc" + a);
                } catch (ex) { }
            }
        } else if (x == "hide") {
            dvFrame.addClass("d-none");
        } else if (x == "showpopdc") {
            if (document.readyState == "complete") {
                dvFrame.dialog('open');
            }
            dvFrame.find("#head" + a).find("a[onclick^='javascript:ShowDc(']").hide();
            if (!($("#" + thisFldId).parent().next()).hasClass("ShowPopDc")) {
                $("<span class=\"fa fa-external-link ShowPopDc\" onclick=\"ShowingDc('" + a + "','showpopdc'); return false;\"></span>").insertAfter($("#" + thisFldId).parent());
            }
        } else if (x == "hidepopdc") {
            if (document.readyState == "complete") {
                dvFrame.dialog('close');
            }
            if (($("#" + thisFldId).parent().next()).hasClass("ShowPopDc")) {
                ($("#" + thisFldId).parent().next()).remove();
            }
        } else if (x == "enable") {
            $j("#DivFrame" + a).find('input,textarea, img, select, a').not('.flddis').removeAttr('disabled');
            $j("#DivFrame" + a).find('.rowdelete').removeClass("disabledelete");
            $j("#wrapperForEditFields" + a).show();
            $j("#gridToggleBtn" + a).removeClass('disables').prop('disables', false);
            $j("#DivFrame" + a).find('.rowadd').removeClass("disableadd");
            $j("#DivFrame" + a).find('.fillcls').removeClass("disablefill");
            $j("#gridAddBtn" + a).prop('disabled', false).removeClass("disableadd");
            $j("#DivFrame" + a + " .gridIconBtns a").not("[id^=clearThisDC]").prop('disabled', false).removeClass('disabled');
            $j("#wrapperForEditFields" + a).find(".editLayoutFooter button").removeClass('disabled').prop('disabled', false);
            //$j("[id ^= 'fillgrid']").prop('disabled', false).removeClass('disabled');
            $j("#fillgrid" + a).prop('disabled', false).removeClass('disabled');
            $j("#fillgridList" + a).prop('disabled', false).removeClass('disabled');
            $j("#gridHd" + a + " tbody tr").removeClass('disableTheRow');
            $j("#gridHd" + a + " tbody tr").find('.glyphicon.glyphicon-pencil,.glyphicon.glyphicon-trash').removeClass('disabled').prop("disabled", false).parent().removeClass('disabled').prop("disabled", false);
            $("#gridAddBtn" + a).next().find(":button").attr("disabled", false).on('click');
            $j("#DivFrame" + a + " .newgridbtn ul li").css("pointer-events", "");
            $(".formGridRow button").prop('disabled', false).removeClass("disabled")
            $j(".formGridRow button").css("pointer-events", "");
        } else if (x == "disable") {
            $j("#DivFrame" + a).find('input,textarea, img, select, a').not('.subGrid,.chkShwSel').attr('disabled', true);

            for (var instanceName in CKEDITOR.instances) {
                try {
                    CKEDITOR.instances[instanceName].setReadOnly(true);
                } catch (ex) { }
            }

            $j("#DivFrame" + a).find('a img').removeClass('handCur');
            $j("#DivFrame" + a).find('img').attr('onclick', 'javascript:void(0);');
            $j("#DivFrame" + a).find('.dvImgClear .icon-arrows-remove').attr('onclick', 'javascript:void(0);');
            $j("#DivFrame" + a).find(":button").removeClass('handCur');
            $j("#DivFrame" + a).find('.rowdelete').addClass("disabledelete");
            $j("#DivFrame" + a).find('.rowadd').addClass("disableadd");
            $j("#gridAddBtn" + a).prop('disabled', true).addClass('disabled');
            $j("#DivFrame" + a + " .gridIconBtns a").not("[id^=clearThisDC]").prop('disabled', true).addClass('disabled');
            $j("#clearThisDC" + a).prop("disabled", true).find(".gridActs").addClass("disabled");
            if (!$("#wrapperForEditFields" + a).parent().hasClass("formGridRow"))
                $j("#wrapperForEditFields" + a).hide();
            $j("#gridToggleBtn" + a).addClass('disabled').prop('disabled', true);
            $j("#DivFrame" + a).find('.fillcls').addClass("disablefill");
            //$j("[id ^= 'fillgrid']").prop('disabled', true).addClass('disabled');
            $j("#fillgrid" + a).prop('disabled', true).addClass('disabled');
            $j("#fillgridList" + a).prop('disabled', true).addClass('disabled');
            $j("#wrapperForEditFields" + a).find(".editLayoutFooter button").addClass('disabled').prop('disabled', true);
            $j("#colScroll" + a + " table tbody tr").addClass('disableTheRow');
            $j("#colScroll" + a + " table tbody tr").find('.glyphicon.glyphicon-pencil,.glyphicon.glyphicon-trash').addClass('disabled').prop("disabled", true).parent().addClass('disabled').prop("disabled", true);
            $("#gridAddBtn" + a).next().find(":button").attr("disabled", true).off('click');
            $j("#DivFrame" + a + " .newgridbtn ul li").css("pointer-events", "none");
            $(".formGridRow button").addClass("disabled").prop('disabled', true);
            $j(".formGridRow button").css("pointer-events", "none");
            //to disable grid attachment icon click event
            $j("#DivFrame" + a).find(".upload-icon").hover(function () {
                $(this).css("cursor", "not-allowed");
            }).attr('onclick', 'javascript:void(0)');

            //to disable grid attachment files click event(remove & preview)
            $j("#DivFrame" + a).find(".attach-files .grdAttach, .grdAttach").hover(function () {
                $(this).css("cursor", "not-allowed");
            }).attr('onclick', 'javascript:void(0)');

            $j("#DivFrame" + a).find(".attachment-count").removeAttr("disabled");

        }
    } else {
        if (x == "disable" || x == "enable") {
            var idx = $j.inArray(a + "~" + x, DisabledDcs);
            if (idx == -1)
                DisabledDcs.push(a + "~" + x);
        }
    }

    var li = $j("#li" + a);
    if (li.length > 0) {
        if (x == "show")
            // li.show();
            li.removeClass("d-none");
        else if (x == "hide")
            // li.hide();
            li.addClass("d-none");
    }


    //Rule 1- If the active tab is action tab and x is hide, then show first tab
    //Rule 2- If action tab is the first tab then show the first tab
    if (li.hasClass("active") && x == "hide") {
        var firstTab = GetFirstTabDcNo(a)
        if (firstTab != -1)
            $j("#ank" + firstTab).click();
    }
    tabedDcSliderHtml();
    try {
        AxAfterShowingDc(a, x);
    } catch (ex) { }
}

//function to createpopdcdialog

function createPopDcDialog(a, dvFrame) {
    var wWidth = $(window).width();
    var dWidth = wWidth * 0.97;
    var wHeight = $(window).height();
    var dHeight = wHeight * 0.97;

    if (document.readyState == "complete") {

        dvFrame.dialog({
            modal: true,
            autoOpen: false,
            width: dWidth,
            open: function () {
                $('.ui-widget-overlay').bind('click', function () {
                    dvFrame.dialog('close');
                })

            },
            close: function () {
                $(this).dialog("close");
                $(this).dialog('destroy');
                dvFrame.hide();
            },
            buttons: [{
                text: "Save",
                click: function () {
                    if (ValidateBeforeSubmit(a)) {
                        dvFrame.dialog('close');
                    }
                },
                'class': "ui-button ui-corner-all ui-widget hotbtn"
            },

            {
                text: "Cancel",
                click: function () {
                    ClearFieldsInDC(a);
                    dvFrame.dialog('close');
                },
                'class': "coldbtn btn",
                style: "background: antiquewhite"
            }
            ]
        });

        $('.ui-dialog-titlebar').hide();

        if (dvFrame.find("div:eq(0) button").length == 0) {
            dvFrame.find("div:eq(0)").prepend('<button class="remodal-close icon-basic-remove" style="color:rgba(76, 53, 53, 0.61);"></button>');
        } else {
            //do nothing
        }

        $('.remodal-close').bind('click', function () {
            dvFrame.dialog('close');
        });
    }

}

function createDataGridDialog(editGridDC) {
    createBootstrapModal("divDc" + editGridDC + " #gridheaddiv", "DC" + editGridDC + " ", "wrapperForEditFields" + editGridDC, "", "", "97%");
}

function clearDataGrid(editGridDC, calledfrom = "") {
    var isExitDummy = false;
    if (gridDummyRowVal.length > 0) {
        gridDummyRowVal.map(function (v) {
            if (v.split("~")[0] == editGridDC) isExitDummy = true;
        });
    }
    if (isExitDummy)
        return false;

    //if ($j("#clearThisDC" + editGridDC).prop("disabled"))
    if ($j("#clearThisDC" + editGridDC).find("a[disabled='disabled']").length > 0)
        return false;
    if ($j("#chkallgridrow" + editGridDC).prop("checked")) {
        if ($(".wrapperForGridData" + editGridDC + " table tbody tr").length > 0) {
            var cutMsg = eval(callParent('lcm[25]'));
            try {
                let _cutMsg = AxBeforeAllGridRowsDelete(editGridDC);
                if (_cutMsg != "")
                    cutMsg = _cutMsg;
            } catch (ex) { }
            cutMsg = cutMsg.replace('{0}', GetDcCaption(editGridDC));
            var glType = eval(callParent('gllangType'));
            var isRTL = false;
            if (glType == "ar")
                isRTL = true;
            else
                isRTL = false;
            var rid = $j("#recordid000F0").val();
            if (rid != "0" && calledfrom != "excelimport") {
                var clearThisDCGrid = $.confirm({
                    theme: 'modern',
                    closeIcon: false,
                    rtl: isRTL,
                    title: eval(callParent('lcm[155]')),
                    onContentReady: function () {
                        $(".jconfirm-buttons .hotbtn").focus(); //bug #AGI000616 -- manually focusing the cursor to Confirm button to avoid tab focus to other elements(once dialog displayed click on Shift+Tab it will navigate to background elements)
                        disableBackDrop('bind');
                    },
                    backgroundDismiss: 'false',
                    escapeKey: 'buttonB',
                    content: cutMsg,
                    buttons: {
                        buttonA: {
                            text: eval(callParent('lcm[164]')),
                            btnClass: 'btn btn-primary',
                            action: function () {
                                try {
                                    FldListParents = new Array();
                                    FldListData = new Array();
                                } catch (ex) { }
                                clearThisDCGrid.close();
                                ShowDimmer(true);
                                AxWaitCursor(true);
                                var fDcRowCount = GetDcRowCount(editGridDC);
                                DeleteAllRows(editGridDC, fDcRowCount);
                                $("#chkallgridrow" + editGridDC).prop("checked", false);
                                $("#clearThisDC" + editGridDC).addClass("disabled");
                                if (AxFormContSetGridCell.length > 0) {
                                    AxFormContSetGridCell = jQuery.grep(AxFormContSetGridCell, function (value) {
                                        return value.split("~")[0] != editGridDC || (value.split("~")[0] == editGridDC && value.split("~")[2] == "*");
                                    });
                                }
                            }
                        },
                        buttonB: {
                            text: eval(callParent('lcm[192]')),
                            btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                            action: function () {
                                AxWaitCursor(false);
                                ShowDimmer(false);
                                disableBackDrop('destroy');
                                return;
                            }
                        },
                    }
                });
            } else {
                try {
                    FldListParents = new Array();
                    FldListData = new Array();
                } catch (ex) { }
                ShowDimmer(true);
                AxWaitCursor(true);
                var fDcRowCount = GetDcRowCount(editGridDC);
                DeleteAllRows(editGridDC, fDcRowCount);
                $("#chkallgridrow" + editGridDC).prop("checked", false);
                $("#clearThisDC" + editGridDC).addClass("disabled");
                if (AxFormContSetGridCell.length > 0) {
                    AxFormContSetGridCell = jQuery.grep(AxFormContSetGridCell, function (value) {
                        return value.split("~")[0] != editGridDC || (value.split("~")[0] == editGridDC && value.split("~")[2] == "*");
                    });
                }
            }
        } else {
            showAlertDialog("info", 2004, "client");
        }
    } else {
        if ($j("input[name=grdchkItemTd" + editGridDC + "]:checked").length > 0) {
            try {
                FldListParents = new Array();
                FldListData = new Array();
            } catch (ex) { }
            DeleteSelectedRows(editGridDC);
        }
    }
}

function DeleteSelectedRows(editGridDC) {

    var glType = eval(callParent('gllangType'));
    var isRTL = false;
    if (glType == "ar")
        isRTL = true;
    else
        isRTL = false;
    var cutMsg = eval(callParent('lcm[5]'));
    try {
        let _cutMsg = AxBeforeSingleGridRowDelete(editGridDC);
        if (_cutMsg != "")
            cutMsg = _cutMsg;
    } catch (ex) { }
    var rid = $j("#recordid000F0").val();
    if (rid != "0") {
        $.confirm({
            theme: 'modern',
            title: eval(callParent('lcm[155]')),
            content: cutMsg,
            escapeKey: 'buttonB',
            rtl: isRTL,
            onContentReady: function () {
                disableBackDrop('bind');
            },
            buttons: {
                buttonA: {
                    text: eval(callParent('lcm[279]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        try {
                            let chkdCount = $j("input[name=grdchkItemTd" + editGridDC + "]:checked").length;
                            let loopCount = 0;
                            $j("input[name=grdchkItemTd" + editGridDC + "]:checked").each(function () {
                                loopCount++;
                                let _thisId = $j(this).attr("id");
                                let rowFrmNo = _thisId.substring(_thisId.lastIndexOf("F"), _thisId.lastIndexOf("F") - 3);// _thisId.substr(7, _thisId.lastIndexOf("F") - 3);
                                if (axInlineGridEdit)
                                    updateInlineGridRowValues();
                                if (loopCount == chkdCount)
                                    DeleteGridRow(editGridDC, rowFrmNo, undefined);
                                else
                                    DeleteGridRow(editGridDC, rowFrmNo, "all");
                                SetPositionfldDisplayTot();
                            });
                        } catch (ex) {
                            AxWaitCursor(false);
                        }
                    }
                },
                buttonB: {
                    text: eval(callParent('lcm[280]')),
                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                    action: function () {

                    }
                }
            }
        });
    } else {
        try {
            let chkdCount = $j("input[name=grdchkItemTd" + editGridDC + "]:checked").length;
            let loopCount = 0;
            $j("input[name=grdchkItemTd" + editGridDC + "]:checked").each(function () {
                loopCount++;
                let _thisId = $j(this).attr("id");
                let rowFrmNo = _thisId.substring(_thisId.lastIndexOf("F"), _thisId.lastIndexOf("F") - 3);// _thisId.substr(7, _thisId.lastIndexOf("F") - 3);
                if (axInlineGridEdit)
                    updateInlineGridRowValues();
                if (loopCount == chkdCount)
                    DeleteGridRow(editGridDC, rowFrmNo, undefined);
                else
                    DeleteGridRow(editGridDC, rowFrmNo, "all");
                SetPositionfldDisplayTot();
            });
        } catch (ex) {
            AxWaitCursor(false);
        }
    }

    if (arrRefreshDcs.length > 0) {
        for (var i = 0; i < arrRefreshDcs.length; i++) {
            var arrDcNos = arrRefreshDcs[i].split(':');
            if (arrDcNos[1] == "dc" + editGridDC) {
                arrRefreshFldDirty[i] = true;
                break;
            }
        }
    }
    OnRowChangeSetHeight(editGridDC);
    checkTableBodyWidths(editGridDC);
    if (isTstPop == "True") {
        TstructTabEventsInPopUP("");
    }

    //DeleteGridRow(dcNo, GetClientRowNo(i, dcNo), "all");
}

function GetFirstTabDcNo(curDcNo) {
    var j = 0;
    for (j = 0; j < PagePositions.length; j++) {

        if (PagePositions[j].toString().indexOf(",") != -1) {

            var strTabs = PagePositions[j].toString().split(',');
            var cnt = 0;
            for (cnt = 0; cnt < strTabs.length; cnt++) {
                if (curDcNo == strTabs[cnt]) {
                    return strTabs[0];
                }
            }
        }
    }

    return -1;
}

//<Module>  FormControl </Module>
//<Author>  Naveen  </Author>
//<Description> Function to Enable/Disable the DC control </Description>
//<Return> Enables/Disables the DC control based on the action </Return>
function EnableDisableDc(a, x) {
    var isGrid = IsDcGrid(a);
    var adddcno = parseInt(a, 10);
    var isDisable = false;
    var cursorStyle = 'hand';
    if (x == "Enable") {

        isDisable = false;
        cursorStyle = 'hand';
    } else if (x == "Disable") {

        isDisable = "disabled";
        cursorStyle = 'default';
    }

    var dcBtn = $j("#dcButspan" + a);
    if (dcBtn.length > 0) {
        dcBtn.attr("alt", "hide");
        dcBtn.attr("class", "glyphicon glyphicon-chevron-down icon-arrows-down");
        dcBtn.attr("disabled", isDisable);
        dcBtn.css("cursor", cursorStyle);
    }

    if (isGrid) {
        var dvFrame = $j("#DivFrame" + a);
        if (dvFrame.length > 0) {
            if (x == "Enable")
                dvFrame.attr("disbaled", false);
            else
                dvFrame.attr("disbaled", "disbaled");
        }
    } else {
        var nonGridDc = $j("#DivFrame" + a);
        if (nonGridDc.length > 0) {
            if (x == "Enable")
                nonGridDc.attr("disbaled", false);
            else
                nonGridDc.attr("disbaled", "disbaled");
        } else {
            var tabDc = $j("#tab-" + a);
            if (tabDc.length > 0) {
                if (x == "Enable")
                    tabDc.attr("disbaled", false);
                else
                    tabDc.attr("disbaled", "disbaled");
            }
        }
    }
}

//-----------------------------List of functions in this file--------------------------------------
//CheckAOWE(dcNo) -Function which returns the actual Grid row count for checking Add Only When Empty in Fill grid.
//FillGrid(tid, dcname, ashow, multiselect, plist) -Function to call fill grid service or open a fillgrid page.
//SuccessFillGridNonMS(result, eventArgs) -Callback function for DoGetFillGridNonMS service.
//ProcessFillGrid(dcNo) -Function which calls the DoGetFillGrid service.
//SuccGetResultValue(result, eventArgs) -Callback function form the DoGetFillGrid webservice.
//DoFillControlPrivilege(strSingleLineText) -Function which executes the partial disabling in the fill grid dc.
//CloseWrapper() -Function to close the wrapper on fill grid window.
//CheckAll() -Function to check all the row once the header row is checked.
//ChkHdrCheckbox() -Function to check if the header checkbox is checked then check all.
//-------------------------------------------------------------------------------------------------
$j.curCSS = function (element, attrib, val) {
    $j(element).css(attrib, val);
};

//Function which returns the actual Grid row count for checking Add Only When Empty in Fill grid.
//By default the grid has one row,
//Function to add a new group to the given format grid dc.
function AddGroup(dcNo, keyColName) {
    fillDcname = dcNo;
    //This below code is to send the existing groups in the format grid on filling new items.
    var dcRowCnt = GetDcRowCount(dcNo);
    if (parseInt(dcRowCnt, 10) == 1) {
        var rowNo = GetClientRowNo("001", dcNo);
        var keyColfldId = keyColName + rowNo + "F" + dcNo;
        var val = GetFieldValue(keyColfldId);
        if (val != "")
            UpdateFieldArray(keyColfldId, "001", val, "");
    }

    var rid = $j("#recordid000F0").val();
    ArrActionLog = "AddGroup-DcNo-" + dcNo + "-KeyColumn-" + keyColName + "-Recordid-" + rid;

    var dIndx = GetFormatGridIndex(dcNo);
    if (dIndx != -1) {
        var multiSelect = DcMultiSelect[dIndx].toString().toLowerCase();
        try {
            if (multiSelect == "true")
                ASB.WebService.GetAddGroupsHtml(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, dcNo, transid, tstDataId, resTstHtmlLS, SuccessGetFillGridMS, OnException);
            else {
                var delRows = GetDeletedRows();
                var chngRows = GetChangedRows();
                var recid = $j("#recordid000F0").val();
                ShowDimmer(true);
                ASB.WebService.ExecuteAction(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ArrActionLog, recid, transid, dcNo, "", "", delRows, chngRows, tstDataId, resTstHtmlLS, SuccExecActionValue, OnException);
            }
        } catch (ex) {
            AxWaitCursor(false);
            var execMess = ex.name + "^♠^" + ex.message;
            showAlertDialog("error", 2030, "client", execMess);
        }
    }
}

var fillGridDc = "";
var fillOrAdd = "";
var AxActivefillGridName = "";
//Function to call fill grid service or open a fillgrid page.
function FillGrid(frNo, addGroup, fillGridName, fastFill) {
    GetCurrentTime("Tstruct FillGrid button click(ws call)");
    fastFill == undefined ? fastFill = false : true;
    var IsConfirm = false;
    //if (!checkEditMode())
    //    return;
    AxStartTime = new Date();
    AxStartTime = GetAxDate(AxStartTime);
    var stTime = new Date();

    try {
        AxBeforeFillGrid();
    } catch (ex) { }
    AxActivefillGridName = fillGridName;
    if (typeof $j("[name=" + fillGridName + "]").attr("class") != "undefined" && $j("[name=" + fillGridName + "]").attr("class").indexOf("disablefill") != -1)
        return;
    //ShowDimmer(true);
    //AxWaitCursor(true);
    var vallist = new Array();

    fillGridDc = frNo;

    var ind = $j.inArray(fillGridName, FillGridName);
    var dcIdx = $j.inArray(frNo, DCFrameNo);
    var isExitDummy = false;
    if (gridDummyRowVal.length > 0) {
        gridDummyRowVal.map(function (v) {
            if (v.split("~")[0] == fillGridDc) isExitDummy = true;
        });
    }
    //Fill grid rule:1
    if (FillCondition[ind] == "APPEND" && $("#gridHd" + frNo + " tbody tr").length > 0 && isExitDummy) {
        UpdateFieldArray(axpIsRowValid + frNo + "001F" + frNo, GetDbRowNo("001", frNo), "false", "parent", "fillgrid");
    } else if (FillCondition[ind] == "INIT") {
        if ($("#gridHd" + frNo + " tbody tr").length > 0 && !isExitDummy) {
            IsConfirm = true;
            var cutMsg = eval(callParent('lcm[26]'));
            var glType = eval(callParent('gllangType'));
            var isRTL = false;
            if (glType == "ar")
                isRTL = true;
            else
                isRTL = false;
            var FillGridCB = $.confirm({
                theme: 'modern',
                closeIcon: false,
                title: eval(callParent('lcm[155]')),
                onContentReady: function () {
                    disableBackDrop('bind');
                },
                backgroundDismiss: 'false',
                rtl: isRTL,
                escapeKey: 'buttonB',
                content: cutMsg,
                buttons: {
                    buttonA: {
                        text: eval(callParent('lcm[164]')),
                        btnClass: 'btn btn-primary',
                        action: function () {
                            GetCurrentTime("Tstruct FillGrid button click(ws call)");
                            FillGridCB.close();

                            if (isMobile && !axInlineGridEdit && AxpGridForm == "form") {
                                var fDcRowCount = GetDcRowCount(frNo);
                                DeleteAllRows(frNo, fDcRowCount, "FillGrid");
                            }

                            if (gridDummyRowVal.length > 0) {
                                gridDummyRowVal.map(function (v) {
                                    if (v.split("~")[0] == fillGridDc)
                                        gridDummyRowVal.splice($.inArray(v, gridDummyRowVal), 1);
                                    gridRowEditOnLoad = false;
                                });
                            }
                            FillGridAfterConfirm(fastFill);
                        }
                    },
                    buttonB: {
                        text: eval(callParent('lcm[192]')),
                        btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                        action: function () {
                            disableBackDrop('destroy');
                            AxWaitCursor(false);
                            ShowDimmer(false);
                            return;
                        }
                    }
                }
            });

        }
    } else if (FillCondition[ind] == "AOWE") {
        if ($("#gridHd" + frNo + " tbody tr").length > 0 && !isExitDummy && DCHasDataRows[dcIdx] == "True") {
            IsConfirm = true;
            //ShowDialog('warning', 5003, "client");
            var cutMsg = eval(callParent('lcm[519]'));
            var glType = eval(callParent('gllangType'));
            var isRTL = false;
            if (glType == "ar")
                isRTL = true;
            else
                isRTL = false;
            var FillGridCB = $.confirm({
                theme: 'modern',
                closeIcon: false,
                title: eval(callParent('lcm[155]')),
                onContentReady: function () {
                    disableBackDrop('bind');
                },
                backgroundDismiss: 'false',
                rtl: isRTL,
                escapeKey: 'buttonB',
                content: cutMsg,
                buttons: {
                    buttonA: {
                        text: eval(callParent('lcm[164]')),
                        btnClass: 'btn btn-primary',
                        action: function () {
                            GetCurrentTime("Tstruct FillGrid button click(ws call)");
                            FillGridCB.close();
                            var fDcRowCount = GetDcRowCount(frNo);
                            DeleteAllRows(frNo, fDcRowCount, "FillGrid");
                            if (gridDummyRowVal.length > 0) {
                                gridDummyRowVal.map(function (v) {
                                    if (v.split("~")[0] == fillGridDc)
                                        gridDummyRowVal.splice($.inArray(v, gridDummyRowVal), 1);
                                    gridRowEditOnLoad = false;
                                });
                            }
                            FillGridAfterConfirm(fastFill);
                        }
                    },
                    buttonB: {
                        text: eval(callParent('lcm[192]')),
                        btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                        action: function () {
                            disableBackDrop('destroy');
                            AxWaitCursor(false);
                            ShowDimmer(false);
                            return;
                        }
                    }
                }
            });
        }
    }

    function FillGridAfterConfirm(fastFill) {
        AxWaitCursor(true);
        ShowDimmer(true);
        var paramList = "";
        if (ind != -1) {
            ashow = FillAutoShow[ind];
            multiselect = FillMultiSelect[ind].toString().toLowerCase();
            paramList = FillParamFld[ind];
        }

        try {
            FldListParents = new Array();
            FldListData = new Array();
        } catch (ex) { }

        //If the dc has fillgrid from source dc then multiselect should not be shown.
        if (FillSourceDc[ind] != "" && multiselect == "true")
            multiselect = "false";

        var paramXml = "";
        if (paramList != "" && multiselect == "true") {

            var paramlst = paramList.toString();
            var inputdata = paramlst.split(',');

            for (var pr = 0; pr < inputdata.length; pr++) {

                var isGrid = IsGridField(inputdata[pr]);
                var dcNo = GetDcNo(inputdata[pr]);
                if (isGrid == true) {

                    var rCount = GetDcRowCount(dcNo);
                    rCount = parseInt(rCount, 10);

                    var paramValStr = "";
                    for (var k = 1; k <= rCount; k++) {

                        k = GetRowNoHelper(k);
                        inputdata[pr] = GetExactFieldName(inputdata[pr]);
                        var fld = $j("#" + inputdata[pr] + k + "F" + dcNo);
                        if (fld) {

                            var fParamName = inputdata[pr].toString() + k + "F" + dcNo;
                            var paramVal = GetFieldValue(fParamName, "fillGrid");
                            paramVal = CheckSpecialCharsInStr(paramVal);
                            paramXml += "<" + inputdata[pr] + " rowno='" + k + "' >" + paramVal + "</" + inputdata[pr] + ">";
                        }
                    }
                } else {
                    var fParamName = inputdata[pr].toString() + "000F" + dcNo;
                    if ($("#" + fParamName).length > 0) {
                        var paramVal = GetFieldValue(fParamName, "fillGrid");
                        paramVal = CheckSpecialCharsInStr(paramVal);
                        paramXml += "<" + inputdata[pr] + ">" + paramVal + "</" + inputdata[pr] + ">";
                    } else {
                        var paramVal = CheckGlobalVars(inputdata[pr].toString());
                        if (paramVal == inputdata[pr].toString())
                            paramXml += "<" + inputdata[pr] + "></" + inputdata[pr] + ">";
                        else
                            paramXml += "<" + inputdata[pr] + ">" + paramVal + "</" + inputdata[pr] + ">";
                    }
                }
            }
        }
        var src = "fill";
        if (addGroup && addGroup != "") src = addGroup;
        fillOrAdd = src;
        if (multiselect == "true") {
            if (fastFill) {
                var res = "t";
                //if (FillGridVExpr[ind] != "") {
                //    res = EvalPrepared("", "000", FillGridVExpr[ind], "expr");
                //}

                //paramXml = "<tid>" + transid + "</tid><dc>" + frNo + "</dc>" + paramXml;
                try {
                    callBackFunDtls = "FillGridAfterConfirm♠" + fastFill;
                    GetProcessTime();
                    ASB.WebService.GetFastFillGridData(frNo, fillGridName, transid, src, tstDataId, res, resTstHtmlLS, SuccessGetFillGridMS, OnException);

                } catch (ex) {
                    AxWaitCursor(false);
                    var execMess = ex.name + "^♠^" + ex.message;
                    showAlertDialog("error", 2030, "client", execMess);
                }
            } else {
                //Evaluate validate expression for fillgrid if any
                var res = "t";
                if (FillGridVExpr[ind] != "") {
                    res = EvalPrepared("", "000", FillGridVExpr[ind], "expr");
                }

                paramXml = "<tid>" + transid + "</tid><dc>" + frNo + "</dc>" + paramXml;
                try {
                    callBackFunDtls = "FillGridAfterConfirm♠" + fastFill;
                    GetProcessTime();
                    ASB.WebService.GetFillGridData(paramXml, ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues,
                        DeletedDCRows, frNo, fillGridName, transid, src, tstDataId, res, resTstHtmlLS, SuccessGetFillGridMS, OnException);

                } catch (ex) {
                    AxWaitCursor(false);
                    var execMess = ex.name + "^♠^" + ex.message;
                    showAlertDialog("error", 2030, "client", execMess);
                    UpdateExceptionMessageInET("GetFillGridData exception : " + ex.message);
                }
            }
        } else {
            if (!IsFormDirty)
                SetFormDirty(true);
            //If multi select is false,
            //Then call the DoFillGrid service directly instead of opening the fillgrid window.
            try {
                changeFillGridDc = frNo;
                callBackFunDtls = "FillGridAfterConfirm♠" + fastFill;
                GetProcessTime();
                ASB.WebService.DoGetFillGridNonMS(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues,
                    DeletedDCRows, frNo, fillGridName, tstDataId, resTstHtmlLS, SuccessFillGridNonMS, OnException);
            } catch (ex) {
                AxWaitCursor(false);
                var execMess = ex.name + "^♠^" + ex.message;
                showAlertDialog("error", 2030, "client", execMess);
                UpdateExceptionMessageInET("DoGetFillGridNonMS exception : " + ex.message);
                GetProcessTime();
                GetTotalElapsTime();
            }
        }
        var edTime = new Date();
        AxTimeBefSerCall = edTime - stTime;
    }
    if (IsConfirm == false) {
        FillGridAfterConfirm(fastFill);
    }
}





var dialogObj;
var fgHt = 400;
var fgWid = 400;

function SuccessGetFillGridMS(result, eventArgs) {
    if (result != "") {
        if (result.split("♠*♠").length > 1) {
            tstDataId = result.split("♠*♠")[0];
            result = result.split("♠*♠")[1];
        }
        if (result.split("*♠♠*").length > 1) {
            var serverprocesstime = result.split("*♠♠*")[0];
            var requestProcess_logtime = result.split("*♠♠*")[1];
            result = result.split("*♠♠*")[2];
            WireElapsTime(serverprocesstime, requestProcess_logtime, true);
        } else {
            UpdateExceptionMessageInET("Error : " + result);
        }
    }
    if (result.toLowerCase().indexOf("access violation") === -1) {
        if (CheckSessionTimeout(result))
            return;
        try {
            AxBeforeFillPopUp();
        } catch (ex) { }
        MultiFillgridSldIndex = new Array();
        resTstHtmlLS = "";
        var fillTitle = "";
        if (fillOrAdd == "FILL")
            fillTitle = "FILL " + GetDcCaption(fillGridDc);
        else
            fillTitle = "ADD " + GetDcCaption(fillGridDc);

        if (result == "") {
            var cutMsg = eval(callParent('lcm[0]'));
            result = "<label>" + cutMsg + "</label>"
        }

        var resSplit = result.split("*♠*");

        ChangedFields = new Array();
        ChangedFieldDbRowNo = new Array();
        ChangedFieldValues = new Array();
        DeletedDCRows = new Array();

        var tablehtml = resSplit[0].toString();
        if (tablehtml != "" && tablehtml == "<label>No data found.</label>") {
            showAlertDialog("warning", 'No data found.');
            AxWaitCursor(false);
            ShowDimmer(false);
        } else if (tablehtml != "" && tablehtml == "<label>All rows are already selected.</label>") {
            showAlertDialog("warning", 'All rows are already selected.');
            AxWaitCursor(false);
            ShowDimmer(false);
        } else {
            if ($j("#dvFillGrid").length > 0) {
                $("#bootstrapModalWrapP").remove();
                $j("#dvFillGrid").html(tablehtml);
            }
            else {
                $("#bootstrapModalWrapP").remove();
                $("#pagebdy").append("<div id=\"dvFillGrid\" style=\"\"></div>");
                $j("#dvFillGrid").html(tablehtml);
            }

            if (resSplit.length > 1 && resSplit[1].toString() != "") {
                var fgSize = resSplit[1].toString();
                var fgsizeSplit = fgSize.split(',');
                try {
                    fgHt = fgsizeSplit[2].toString();
                    fgWid = fgsizeSplit[3].toString();
                } catch (e) { }
                if ((fgHt == "0") || (fgWid == "0")) {
                    fgHt = 400;
                    fgWid = 400;
                }
                var modalData = "";

                modalData += $j("#dvFillGrid").wrap('<p id="bootstrapModalWrapP"/>').parent().html();
                $j("#bootstrapModalWrapP").html("");
                buttons = {
                    "count": 2,
                    "button1": {
                        'name': eval(callParent('lcm[281]')),
                        'function': "ProcessFillGrid(" + fillGridDc + ", '" + AxActivefillGridName + "')",
                        'class': 'btn hotbtn',
                        'id': 'modalCnfirmbtn'
                    },
                    "button2": {
                        'name': eval(callParent('lcm[192]')),
                        'class': 'btn coldbtn'
                    }
                }
                let myModal = new BSModal("dvFillGrid", fillTitle, modalData, () => {
                    $("#dvFillGrid .dataTables_scrollHeadInner,#dvFillGrid .dataTables_scrollHeadInner .customSetupTableMN").addClass("w-100");
                    
                    $("#dvFillGrid .dataTables_paginate").addClass("p-0");
                    $("#dvFillGrid .dataTables_filter input[type=search]").addClass("serachdatatbl form-control mh-30px");
                    $("#dvFillGrid table").addClass('table table-bordered table-row-bordered border-gray-300');
                    $("#dvFillGrid .dataTables_scrollHeadInner table").addClass('bg-light-dark');
                    $('#dvFillGrid .dataTables_length select').removeClass('form-control input-sm selectPaddingFix').addClass('showentries');


                    $("#dvFillGrid table thead th:eq(0)").css({ "position": "sticky", "background-color": "white", "left": "0px" });
                    $("#dvFillGrid table tbody tr td:first-child").css({
                        "position": "sticky",
                        "background-color": "white",
                        "left": "0px"
                    });

                        try {
                            var glCulture = eval(callParent('glCulture'));
                            let dtFormat = "DD/MM/YYYY";
                            if (glCulture == "en-us")
                                dtFormat = "MM/DD/YYYY";
                            $.fn.dataTable.moment(dtFormat);
                        } catch (ex) { }
                    fillGridDatatbl = $('table[id^=tblFillGrid]').DataTable({
                        fixedHeader: {
                            header: true,
                            footer: true
                        },
                        'autoWidth': false,
                        destroy: true,
                        "dom": 'R<"float-start"f><"float-end"l><"w-100 overflow-auto"t>ip',
                        'columnDefs': [{
                            'orderable': false,
                            'targets': 0
                        }, {
                            'orderable': true,
                            'targets': '_all'
                        }],
                        'aaSorting': (typeof fillGridDataQueryOrder != "undefined" && fillGridDataQueryOrder.toString() == "true") ? [] : [
                            [1, 'asc']
                        ],
                        language: {
                            search: "_INPUT_",
                            searchPlaceholder: "Search...",
                        },
                        scrollCollapse: true,
                        scrollX: true,
                        scrollY: 420,
                        "lengthMenu": (typeof fillGridDataShowAll != "undefined" && fillGridDataShowAll.toString() == "true") ? [
                            [-1, 500, 100],
                            ["All", 500, 100]
                        ] : [
                            [100, 500, -1],
                            [100, 500, "All"]
                        ],
                        colReorder: {
                            fixedColumnsLeft: 1
                        }
                    });
                    setTimeout(function () {
                        $('#dvFillGrid th').each(function () {
                            var $th = $(this);
                            var $resizer = $('<div class="dvFgresizer"></div>').css({
                                position: 'absolute',
                                right: '0',
                                top: '0',
                                width: '10px',
                                height: '100%',
                                cursor: 'col-resize',
                                zIndex: 10,
                            });
                            $th.css('position', 'relative').append($resizer);
                            $resizer.on('mousedown', function (e) {
                                e.preventDefault();
                                var startX = e.pageX;
                                var startWidth = $th.width();
                                $(document).on('mousemove', function (e) {
                                    var newWidth = startWidth + (e.pageX - startX);
                                    if (newWidth > 50) { 
                                        var widthChange = newWidth - startWidth;
                                        $th.css('width', newWidth + 'px');
                                        var index = $th.index();
                                        $('#dvFillGrid tbody tr').each(function () {
                                            var $td = $(this).find('td').eq(index);
                                            $td.css('width', (parseFloat($td.css('width')) + widthChange) + 'px');
                                        });
                                        fillGridDatatbl.columns.adjust();
                                    }
                                });
                                $(document).on('mouseup', function () {
                                    $(document).off('mousemove');
                                    $(document).off('mouseup');
                                });
                            });
                        });
                        fillGridDatatbl.columns.adjust();
                    }, 0);

                    $(".modal-body").css({
                        'overflow-x': 'hidden'
                    });
                    $("#dvFillGrid .dataTables_scrollHeadInner").css('width', '100%');
                    $("#dvFillGrid .dataTables_scrollBody").css('width', '100%');

                    AxWaitCursor(false);
                    ShowDimmer(false);

                }, () => {
                    //hide callback
                });
                myModal.changeSize("fullscreen");
                myModal.modalHeader.classList.add("py-2");
                myModal.modalBody.classList.add("py-0");
                myModal.modalFooter.classList.add("py-0");
                myModal.okBtn.innerText = eval(callParent('lcm[281]'));
                myModal.okBtn.setAttribute("onClick", "ProcessFillGrid('" + fillGridDc + "','" + AxActivefillGridName + "')");
                myModal.cancelBtn.innerText = eval(callParent('lcm[192]'));
                let _ind = $j.inArray(AxActivefillGridName, FillGridName);
                let _fgCaption = "";
                if (_ind != -1) {
                    _fgCaption = FillGridCaption[_ind];
                }
                var buttonResizeHTML = `<button type="button" class="btn btn-light shadow-sm btn-resizefillgrid" onclick="saveFillgridColResize('` + fillGridDc + `','` + _fgCaption + `','` + AxActivefillGridName + `');">Save Resize</button>`;
                var footerExtras = document.createDocumentFragment();
                myModal.footerExtras = footerExtras;
                var buttonResize = document.createElement("div");
                buttonResize.classList.add(..."d-flex gap-2".split(" "));
                myModal.modalFooter.prepend(buttonResize);
                myModal.footerExtras.buttonResize = buttonResize;
                myModal.footerExtras.buttonResize.innerHTML = buttonResizeHTML;

                if (jQuery('table[id^=tblFillGrid]').length) bindUpdownEvents(jQuery('table[id^=tblFillGrid]').attr('id'), 'multiple');
                $("#wrapperForMainNewData", window.parent.document).hide();
            } else {
                var modalData = "";
                var buttons = {
                    "count": 1,
                    "button1": {
                        'name': eval(callParent('lcm[281]')),
                        'class': 'btn hotbtn'
                    }
                }
                modalData += $j("#dvFillGrid").wrap('<p id="bootstrapModalWrapP"/>').parent().html();
                createBootstrapModal("dvFillGrid", fillTitle, modalData, buttons);
                if (jQuery('table[id^=tblFillGrid]').length) bindUpdownEvents(jQuery('table[id^=tblFillGrid]').attr('id'), 'multiple');
                $("#wrapperForMainNewData", window.parent.document).hide();
                AxWaitCursor(false);
                ShowDimmer(false);
            }

            //$('#bootstrapModal').ready(function () {
            //    try {
            //        var glCulture = eval(callParent('glCulture'));
            //        let dtFormat = "DD/MM/YYYY";
            //        if (glCulture == "en-us")
            //            dtFormat = "MM/DD/YYYY";
            //        $.fn.dataTable.moment(dtFormat);
            //    } catch (ex) { }
            //    fillGridDatatbl = $('table[id^=tblFillGrid]').DataTable({
            //        fixedHeader: {
            //            header: true,
            //            footer: true
            //        },
            //        "dom": 'R<"float-start"f><"float-end"l><"w-100 overflow-auto"t>ip',
            //        'columnDefs': [{
            //            'orderable': false,
            //            'targets': 0,
            //            'width': '5%'
            //        }], // hide sort icon on header of first column
            //        'aaSorting': (typeof fillGridDataQueryOrder != "undefined" && fillGridDataQueryOrder.toString() == "true") ? [] : [
            //            [1, 'asc']
            //        ], //default sort for 1 column in ascending order
            //        language: {
            //            search: "_INPUT_",
            //            searchPlaceholder: "Search...",
            //        },
            //        scrollCollapse: true,
            //        scrollX: true,
            //        scrollY: 400,
            //        "lengthMenu": (typeof fillGridDataShowAll != "undefined" && fillGridDataShowAll.toString() == "true") ? [
            //            [-1, 500, 100],
            //            ["All", 500, 100]
            //        ] : [
            //            [100, 500, -1],
            //            [100, 500, "All"]
            //        ],
            //        "autoWidth": false
            //    });

            //    //$('table[id^=tblFillGrid]').DataTable().destroy();
            //    $("#dvFillGrid .dataTables_paginate").addClass("p-0");
            //    $("#dvFillGrid .dataTables_filter input[type=search]").addClass("serachdatatbl form-control mh-30px");
            //    $("#dvFillGrid table").addClass('table table-bordered table-row-bordered border-gray-300');               
            //    $("#dvFillGrid .dataTables_scrollHeadInner table").addClass('bg-light-dark'); 
            //    $('#dvFillGrid .dataTables_length select').removeClass('form-control input-sm selectPaddingFix').addClass('showentries');

            //    $("#dvFillGrid table thead th:eq(0)").css({ "position": "sticky", "background-color": "rgb(245 231 200)", "left": "0px" });
            //    $("#dvFillGrid table tbody tr td:first-child").css({
            //        "position": "sticky",
            //        "background-color": "rgb(246 231 195)",
            //        "left": "0px"
            //    });
            //    setTimeout(function () {
            //        fillGridDatatbl.columns.adjust();   
            //    }, 0);
            //});
        }
        //AxWaitCursor(false);
        //ShowDimmer(false);
    } else {
        AxWaitCursor(false);
        ShowDimmer(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
    GetProcessTime();
    GetTotalElapsTime();
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

//Function to get the dc title and return.
function GetDcCaption(dcNo) {
    dcNo = "dc" + dcNo;
    var dcTitle = "";
    var indx = $j.inArray(dcNo, DCName);
    if (indx != -1) {
        dcTitle = DCCaption[indx];
    }
    return dcTitle;
}

//Callback function for DoGetFillGridNonMS service.
function SuccessFillGridNonMS(result, eventArgs) {
    if (result != "") {
        if (result.split("♠*♠").length > 1) {
            tstDataId = result.split("♠*♠")[0];
            result = result.split("♠*♠")[1];
        }
        if (result.split("*♠♠*").length > 1) {
            var serverprocesstime = result.split("*♠♠*")[0];
            var requestProcess_logtime = result.split("*♠♠*")[1];
            result = result.split("*♠♠*")[2];
            WireElapsTime(serverprocesstime, requestProcess_logtime, true);
        } else {
            UpdateExceptionMessageInET("Error : " + result);
        }
    }
    if (result.toLowerCase().indexOf("access violation") === -1) {
        if (gridDummyRowVal.length > 0) {
            gridDummyRowVal.map(function (v) {
                if (v.split("~")[0] == fillGridDc)
                    gridDummyRowVal.splice($.inArray(v, gridDummyRowVal), 1);
                gridRowEditOnLoad = false;
            });
        }

        var stTime = new Date();
        if (CheckSessionTimeout(result))
            return;
        resTstHtmlLS = "";
        if (result != "") {
            ParseServiceResult(result, "FillGrid");
        }

        var dcColumnValue = $j('#axp_colmerge_' + fillGridDc + '000F1');
        dcColumnValue = dcColumnValue.val();
        if (dcColumnValue != null && dcColumnValue != '' && dcColumnValue != undefined) {
            GetGridDcTable(dcColumnValue, fillGridDc);
        }

        UpdateFldArrayInFillgrid(fillGridDc);

        try {
            if (typeof isWizardTstruct != "undefined" && isWizardTstruct) {
                $("#DivFrame" + fillGridDc + " ul.nav-tabs").addClass('d-none');
                $("#fillgrid" + fillGridDc).addClass('wdcFgClicked');
            }
            $('.griddivColumn').addClass('gridFixedHeader').css({ "overflow": "auto" });
            if (typeof gridFixedHeader == "undefined" || gridFixedHeader == "true") {
                (isMobile && AxpGridForm == "form") ? "" : $('.griddivColumn').addClass('gridFixedHeader').css({ "max-height": "calc(100vh - 130px)" });
                $(".gridFixedHeader table thead tr th").css({ "background": "#fff", "position": "sticky", "top": "0" });
            }
        } catch (ex) { }

        try {
            AxAfterFillGrid();
        } catch (ex) {

        }
        changeEditLayoutIds(FillGridFillRows, FillGridCurrentDC, 'fillGrid');
        EvaluateSetFont(fillGridDc);
        showAttachmentPopover();
        if (AxLogTimeTaken == "true") {
            var edTime = new Date();
            var diff = edTime.getTime() - stTime.getTime();
            CreateTimeLog(AxStartTime, AxTimeBefSerCall, diff, ASBTotal, ASBDbTime, "FillGridNonMS");
        }
        //applyDesignJsonAgain(fillGridDc);
        if (isMobile && AxpGridForm == "form") {
            //do nothing
        } else
            setDesignedLayout("#divDc" + fillGridDc);
        checkTableBodyWidths(fillGridDc);
        //AlignDcElements("divDc" + fillGridDc);
        FocusOnFirstGridField(fillGridDc);
        if (isMobile && mobileCardLayout == "none") {
            $("#wrapperForEditFields" + (fillGridDc)).removeClass("d-none");
            $("#wrapperForEditFields" + (fillGridDc)).addClass("mobilewrapperForEditFields");
            if ($(".wrapperForGridData" + (fillGridDc) + " table tbody tr").length == 0 && !axInlineGridEdit)
                $("#colScroll" + (fillGridDc)).hide();
            if (axInlineGridEdit)
                $(".editWrapTr").hide();
            $(".editLayoutFooter").hide();
            if (!axInlineGridEdit)
                $(`#DivFrame${fillGridDc} .newgridbtn`).addClass("d-none");
            let gridButton = $(`#DivFrame${fillGridDc} .newgridbtn>ul`).html();
            $(`#wrapperForEditFields${fillGridDc}`).append(`<div class="clearfix"></div><div class="grdButtons btnMobile"><ul class="left">${gridButton}</ul></div>`);
            $(`#wrapperForEditFields${fillGridDc} .btnMobile`).find(`#viewGrid${fillGridDc}`).addClass("d-none");
            $(".dcTitle").hide().nextAll("hr.hrline").hide();

            try {
                if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].selectedLayout != null && designObj[0].selectedLayout != "" && designObj[0].selectedLayout == "tile") {
                    $(".dcTitle").show().nextAll("hr.hrline").show();
                }
            } catch (ex) { }
        }
        if (isMobile && AxpGridForm == "form" && $("#gridHd" + fillGridDc + " tbody tr").length == 0) {
            GridDcAddEmptyRows();
        } 
        SetPositionfldDisplayTot();
        gridFreezeCols(fillGridDc);
    } else {
        AxWaitCursor(false);
        ShowDimmer(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
        //if (callBackFunDtls != "") {
        //    AxWaitCursor(false);
        //    ShowDimmer(false);
        //    $("#reloaddiv").show();
        //    $("#dvlayout").hide();
        //}
    }
    GetProcessTime();
    GetTotalElapsTime();
}


var addGroupDc = "";
///Function to Add selected groups to the format grid.
function ProcessAddGroup(dcNo) {
    addGroupDc = dcNo;
    var tblFg = $j("#tblFillGrid" + dcNo);
    var rowData = "";
    var data = "";
    var selection = "";

    //Loop through the checklist and get the selected rows data
    tblFg.find('.fgChk').each(function () {
        if ($j(this).prop("checked") != undefined && $j(this).prop("disabled") == false) {
            rowData = $j(this).val();

            var rows = rowData.split("¿");
            if (selection == "")
                selection += rows[0].toString();
            else
                selection += "¿" + rows[0].toString();
        }
    });
    var dcClientRows = GetDcClientRows(dcNo);
    var lastRow = dcClientRows.getMaxVal();
    var newCnt = parseInt(lastRow, 10) + 1;
    var newRowNo = GetRowNoHelper(newCnt);
    //GetNewGroupsHtml
    try {
        ASB.WebService.GetNewGroupsHtml(dcNo, transid, selection, newRowNo, tstDataId, resTstHtmlLS, SuccAddGroups);
    } catch (ex) {
        AxWaitCursor(false);
        var execMess = ex.name + "^♠^" + ex.message;
        showAlertDialog("error", 2030, "client", execMess);
    }
}

///Success function which appends the ne group to the format grid.
function SuccAddGroups(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    if (addGroupDc != "") {
        var strResult = result.split("♣");
        var oldHtml = $j("#gridDc" + addGroupDc).html();
        $j("#gridDc" + addGroupDc + " > tbody:last").append(strResult[1]);
        if (strResult[0] != "") {
            var rowArray = new Array();
            var rows = strResult[0].toString().split(",");
            for (var i = 0; i < rows.length; i++) {
                UpdateDcRowArrays(addGroupDc, rows[i], "Add");
                UpdateKeyColValues(addGroupDc, rows[i]);
            }

        }
        ResetRowCount(addGroupDc, rows.length);
        ResetGridRowsStyle(addGroupDc);
        rowArray.push("#DivFrame" + addGroupDc);
        AssignJQueryEvents(rowArray);
        AxWaitCursor(false);
        if (dialogObj) {
            dialogObj.dialog("close");
            $("#wrapperForMainNewData", window.parent.document).show();
        }
    }
}


function ResetRowCount(dcNo, newCnt) {
    var oldRowCnt = parseInt($j("#hdnRCntDc" + addGroupDc).val(), 10);
    var rowCnt = oldRowCnt + newCnt;
    SetRowCount(addGroupDc, rowCnt);
}

function UpdateKeyColValues(dcNo, rowNo) {
    var dcIndx = GetFormatGridIndex(dcNo);
    if (dcIndx != -1) {
        var keyCol = DcKeyColumns[dcIndx];
        var fld = $j("#" + keyCol + rowNo + "F" + dcNo);
        var fldValue = GetFieldValue(keyCol + rowNo + "F" + dcNo);
        var dbRowNo = GetDbRowNo(rowNo, dcNo);
        UpdateFieldArray(keyCol + rowNo + "F" + dcNo, dbRowNo, fldValue, "parent");
        UpdateAllFieldValues(keyCol + rowNo + "F" + dcNo, fldValue);
    }
}

//Function to fill the values from list or sql without action
function ProcessFormatStaticFill(dcNo) {

    fillDcname = dcNo;
    AxWaitCursor(true);

    var tblFg = $j("#tblFillGrid" + dcNo);
    var rowData = "";
    var data = "";
    var selection = "";
    var selectionCol = "";
    var selectionType = "";

    var dcIdx = $j.inArray("dc" + dcNo, DCName);
    var keyCol = "";
    if (dcIdx != -1)
        keyCol = DcKeyColumns[dcIdx];

    //Loop through the checklist and get the selected rows data
    tblFg.find('.fgChk').each(function () {
        if ($j(this).prop("checked") != undefined) {
            rowData = $j(this).val();

            var rows = rowData.split("¿");
            for (var i = 0; i < rows.length; i++) {
                var colData = rows[i].split("♣");
                if (i == 0) {
                    if (colData[0] == keyCol) {
                        selectionCol = colData[0];
                    }
                }
                if (colData[0] == selectionCol) {

                    if (selection == "")
                        selection = colData[1];
                    else
                        selection += "," + colData[1];
                }
            }
        }
    });

    data = data.replace(/&/g, "&amp;");
    var recid = $j("#recordid000F0").val();
    try {
        ASB.WebService.ExecuteFormatFill(transid, dcNo, selection, tstDataId, resTstHtmlLS, SuccExecActionValue);
    } catch (ex) {
        AxWaitCursor(false);
        var execMess = ex.name + "^♠^" + ex.message;
        showAlertDialog("error", 2030, "client", execMess);
    }
}

//Function to call action for a format grid on the selected rows from the fill.
function ProcessFormatFill(dcNo) {
    fillDcname = dcNo;
    AxWaitCursor(true);

    var tblFg = $j("#tblFillGrid" + dcNo);
    var rowData = "";
    var data = "";
    var selection = "";
    var selectionCol = "";
    var selectionType = "";
    //Loop through the checklist and get the selected rows data
    tblFg.find('.fgChk').each(function () {
        if ($j(this).prop("checked") != undefined) {
            rowData = $j(this).val();

            var rows = rowData.split("¿");
            for (var i = 0; i < rows.length; i++) {
                var colData = rows[i].split("♣");
                if (i == 0) {
                    if (colData[0].substring(1) == "selection") {
                        selectionCol = colData[0];
                        selectionType = colData[0].substring(0, 1);
                    }
                }
                if (colData[0] == selectionCol) {
                    if (selectionType != "n") {
                        if (selection == "")
                            selection = "'" + colData[1] + "'";
                        else
                            selection += "," + "'" + colData[1] + "'";
                    } else {
                        if (selection == "")
                            selection = colData[1];
                        else
                            selection += "," + colData[1];
                    }
                }
            }
        }
    });

    if (selection == "") {
        showAlertDialog("warning", 2005, "client");
        AxWaitCursor(false);
        return;
    }
    data = "<selections>" + selection + "</selections>";
    data = data.replace(/&/g, "&amp;");
    var recid = $j("#recordid000F0").val();

    var rid = $j("#recordid000F0").val();
    ArrActionLog = "FillFormatGrid-DcNo-" + dcNo + "-Selected groups-" + selection + "-Recordid-" + rid;

    var delRows = GetDeletedRows();
    var chngRows = GetChangedRows();
    try {
        ASB.WebService.ExecuteAction(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ArrActionLog, recid, transid, dcNo, data, selection, delRows, chngRows, tstDataId, resTstHtmlLS, SuccExecActionValue, OnException);
    } catch (ex) {
        AxWaitCursor(false);
        var execMess = ex.name + "^♠^" + ex.message;
        showAlertDialog("error", 2030, "client", execMess);
    }
}

//Success Function for action after fill in the format grid dc.
function SuccExecActionValue(result, eventArgs) {
    ArrActionLog = "";
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    //the result format -> jsonsResult*♠*dcno♣rowCnt*?*dchtml
    //First split the json and html, call assignloadvalues for json
    //second parse the html
    if (result != "") {
        ParseServiceResult(result, "Action");
    }
    if (dialogObj) {
        dialogObj.dialog("close");
        $("#wrapperForMainNewData", window.parent.document).show();
    }
}


//Function which calls the DoGetFillGrid service.
function ProcessFillGrid(dcNo, fillGridName) {
    GetCurrentTime("Tstruct Process FillGrid Ok button click(ws call)");
    if (gridDummyRowVal.length > 0) {
        gridDummyRowVal.map(function (v) {
            if (v.split("~")[0] == dcNo)
                gridDummyRowVal.splice($.inArray(v, gridDummyRowVal), 1);
            gridRowEditOnLoad = false;
        });
    }
    AxStartTime = new Date();
    AxStartTime = GetAxDate(AxStartTime);
    var stTime = new Date();
    fillDcname = dcNo;

    ShowDimmer(true);
    AxWaitCursor(true);

    if (!IsFormDirty)
        SetFormDirty(true);

    var tblFg = $j("#tblFillGrid" + dcNo);
    var rowData = "";
    var data = "";

    var presentCB = "";
    var isCBchecked;
    if ($('input[name="grdchkItemTd"]:checked').length == 0)
        isCBchecked = false;
    else
        isCBchecked = true;

    if (MultiFillgridSldIndex.length > 0) {
        MultiFillgridSldIndex.forEach(function (_val) {
            var node = fillGridDatatbl.row(_val).node();
            var allTds = $(node).find('td');
            var chkBx = $(node).find('td input[type="checkbox"]');
            var isCurRowChkd = chkBx.is(":checked");
            if (isCurRowChkd) {
                presentCB = chkBx.val();
                data += "<row>";
                var rows = presentCB.split("¿");
                for (var i = 0; i < rows.length; i++) {
                    var colData = rows[i].split("♣");
                    var selColData = CheckSpecialCharsInStr(colData[1]);
                    data += "<" + colData[0] + ">" + selColData + "</" + colData[0] + ">";
                }
                data += "</row>";
            }
        });
    }
        //fillGridDatatbl.rows().every(function () {
        //    var node = this.node();
        //    var allTds = $(node).find('td');
        //    var chkBx = $(node).find('td input[type="checkbox"]');
        //    var isCurRowChkd = chkBx.is(":checked");

        //    if (isCurRowChkd) {
        //        presentCB = chkBx.val();
        //        data += "<row>";
        //        var rows = presentCB.split("¿");
        //        for (var i = 0; i < rows.length; i++) {
        //            var colData = rows[i].split("♣");
        //            var selColData = CheckSpecialCharsInStr(colData[1]);
        //            data += "<" + colData[0] + ">" + selColData + "</" + colData[0] + ">";
        //        }
        //        data += "</row>";
        //    }
        //});
    data = "<GridList>" + data + "</GridList>";
    if (data != "<GridList></GridList>") {
        try {
            MultiFillgridSldIndex = new Array();
            changeFillGridDc = dcNo;
            callBackFunDtls = "ProcessFillGrid♠" + dcNo + "♠" + fillGridName;
            GetProcessTime();
            ASB.WebService.DoGetFillGrid(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, dcNo, fillGridName, data, tstDataId, resTstHtmlLS, SuccGetResultValue, OnException);
        } catch (ex) {
            AxWaitCursor(false);
            ShowDimmer(false);
            if (dialogObj) {
                dialogObj.dialog("close");
                $("#wrapperForMainNewData", window.parent.document).show();
            }
            var execMess = ex.name + "^♠^" + ex.message;
            showAlertDialog("error", 2030, "client", execMess);
            UpdateExceptionMessageInET("DoGetFillGridWS exception : " + ex.message);
            GetProcessTime();
            GetTotalElapsTime();
        }
    } else {
        ShowDimmer(false);
        AxWaitCursor(false);
        if (dialogObj) {
            dialogObj.dialog("close");
            $("#wrapperForMainNewData", window.parent.document).show();
        }
        GetProcessTime();
        GetTotalElapsTime();
    }
    var edTime = new Date();
    AxTimeBefSerCall = edTime - stTime;
    AxActivefillGridName = "";
}

//Callback function form the DoGetFillGrid webservice.
function SuccGetResultValue(result, eventArgs) {
    if (result != "") {
        if (result.split("♠*♠").length > 1) {
            tstDataId = result.split("♠*♠")[0];
            result = result.split("♠*♠")[1];
        }
        if (result.split("*♠♠*").length > 1) {
            var serverprocesstime = result.split("*♠♠*")[0];
            var requestProcess_logtime = result.split("*♠♠*")[1];
            result = result.split("*♠♠*")[2];
            WireElapsTime(serverprocesstime, requestProcess_logtime, true);
        } else {
            UpdateExceptionMessageInET("Error : " + result);
        }
    }
    if (result.toLowerCase().indexOf("access violation") === -1) {
        var stTime = new Date();
        if (CheckSessionTimeout(result))
            return;
        resTstHtmlLS = "";
        //the result format -> jsonsResult*♠*dcno♣rowCnt*?*dchtml
        //First split the json and html, call assignloadvalues for json
        //second parse the html
        ParseServiceResult(result, "FillGrid");

        UpdateFldArrayInFillgrid(fillDcname);

        try {
            $('.griddivColumn ').addClass('gridFixedHeader').css({ "overflow": "auto" });
            if (typeof gridFixedHeader == "undefined" || gridFixedHeader == "true") {
                (isMobile && AxpGridForm == "form") ? "" : $('.griddivColumn').addClass('gridFixedHeader').css({ "max-height": "calc(100vh - 130px)" });
                $(".gridFixedHeader table thead tr th").css({ "background": "#fff", "position": "sticky", "top": "0" });
                if ($("#" + transid).css("overflow") !== "auto" && !$("#" + transid).hasClass("overflow-auto")) {
                    $("#" + transid).css({ "overflow": "auto" });
                }
            }
        } catch (ex) { }

        let isOverridFomrControl = true;
        if (AxRuesDefFormcontrol == "true") {
            isOverridFomrControl = AxRulesDefParser("formcontrol", "", "formcontrol");
        }
        if (appstatus != "Approved" && appstatus != "Rejected" && (!AxExecFormControl) && theMode != "design" && isOverridFomrControl) {
            DoFormControlOnload();
        }
        if (appstatus != "Approved" && appstatus != "Rejected" && (!AxExecFormControl) && theMode != "design" && isOverridFomrControl) {
            var rid = $j("#recordid000F0").val();
            if (rid != "" && rid != "0")
                DoScriptFormControl("", "On Data Load");
            else
                DoScriptFormControl("", "On Form Load");
        }
        var fldGrImage = $(".wrapperForGridData" + fillDcname + " table tbody tr td").find("textarea[id^=dc" + fillDcname + "_image]"); //$(".wrapperForGridData" + fillDcname + " table tbody tr td").find("[id^=dc" + fillDcname + "_image]");
        var fldaxpGrImage = $(".wrapperForGridData" + fillDcname + " table tbody tr td").find("textarea[id^=axp_gridattach_]");
        var fldAxpFileGrdImage = $(".wrapperForGridData" + fillDcname + " table tbody tr td").find("textarea[class^=axpAttach]");
        if (fldGrImage.length > 0) {
            fldGrImage.each(function (ind, val) {
                //if ($(val).attr("id").toLowerCase() != "dc" + fillDcname + "_imagepath") {
                if ($(val).attr("data-type") == "gridattach") {
                    let fldValue = $("textarea#" + $(val).attr("id")).text();
                    if (fldValue != "" && fldValue != undefined) {
                        ConstructAttachHTML(fldValue, $(val).attr("id"), "FillGrid");
                        showAttachmentPopover();
                    }
                }
            });
        }
        if (fldaxpGrImage.length > 0) {
            fldaxpGrImage.each(function (ind, val) {
                let fldValue = $("textarea#" + $(val).attr("id")).text();
                ConstructAttachHTML(fldValue, $(val).attr("id"), "FillGrid");
                showAttachmentPopover();
            });
        }
        if (fldAxpFileGrdImage.length > 0) {
            fldAxpFileGrdImage.each(function (ind, val) {
                if ($(val).attr("data-type") == "gridattach") {
                    let fldValue = $("textarea#" + $(val).attr("id")).text();
                    if (fldValue != "" && fldValue != undefined) {
                        ConstructAxpAttachHTML(fldValue, $(val).attr("id"), "FillGrid");
                        showAttachmentPopover();
                    }
                }
            });
        }
        var dcColumnValue = $j('#axp_colmerge_' + fillDcname + '000F1');
        dcColumnValue = dcColumnValue.val();
        if (dcColumnValue != null && dcColumnValue != '' && dcColumnValue != undefined) {
            GetGridDcTable(dcColumnValue, fillDcname);
        }

        try {
            AxAfterFillGrid();
        } catch (ex) {

        }

        fillgridColOptVisibility(fillDcname); //hide grid columns(design mode hidden columns) after filling records from fillgrid

        EvaluateSetFont(fillDcname);
        changeEditLayoutIds(FillGridFillRows, FillGridCurrentDC, 'fillGrid');
        //Refer bug - AGI003532 and IWA-C-0000020
        if (axInlineGridEdit) {
            UpdateDcRowArrays(FillGridCurrentDC, GetRowNoHelper(parseInt(FillGridFillRows, 10) + 1), "Add");
        }
        //CallAddRowWS(FillGridCurrentDC, GetRowNoHelper(parseInt(FillGridFillRows, 10)));
        var fields = GetGridFields(FillGridCurrentDC);
        var rowNo = getNewEditRowNo(FillGridCurrentDC);
        AxActiveRowNo = parseInt(rowNo);
        AxActiveDc = FillGridCurrentDC;
        if (typeof callBackFunDtls != "" && callBackFunDtls.startsWith("ExcelImportValues♠" + AxActiveDc)) {
            //Do nothing
        } else {
            if (typeof wsPerfEnabled != "undefined" && wsPerfEnabled)
                CallEvaluateOnAddPerf(FillGridCurrentDC, GetRowNoHelper(parseInt(FillGridFillRows, 10) + 1), fields, "FillGrid");
            else
                CallEvaluateOnAdd(FillGridCurrentDC, GetRowNoHelper(parseInt(FillGridFillRows, 10) + 1), fields, "FillGrid");
        }
        AxWaitCursor(false);

        if (dialogObj) {
            dialogObj.dialog("close");
            $("#wrapperForMainNewData", window.parent.document).show();
        }
        if (AxLogTimeTaken == "true") {
            var edTime = new Date();
            var diff = edTime.getTime() - stTime.getTime();
            CreateTimeLog(AxStartTime, AxTimeBefSerCall, diff, ASBTotal, ASBDbTime, "FillGridMS");
        }
        //applyDesignJsonAgain(fillDcname);
        if (isMobile && AxpGridForm == "form") {
            //do nothing
        } else
            setDesignedLayout("#divDc" + fillDcname);
        //AlignDcElements("divDc" + fillDcname);
        FocusOnFirstGridField(fillDcname);
        customAlignTstructFlds([], FillGridCurrentDC);

        $j("textarea[id^='axpvalid']").each(function () {
            DoFormControlPrivilege($j(this).attr("id"), $j(this).val());
        });

        if (isMobile && mobileCardLayout == "none") {
            $("#wrapperForEditFields" + (fillDcname)).removeClass("d-none");
            $("#wrapperForEditFields" + (fillDcname)).addClass("mobilewrapperForEditFields");
            if ($(".wrapperForGridData" + (fillDcname) + " table tbody tr").length == 0 && !axInlineGridEdit)
                $("#colScroll" + (fillDcname)).hide();
            if (axInlineGridEdit)
                $(".editWrapTr").hide();
            $(".editLayoutFooter").hide();
            if (!axInlineGridEdit)
                $(`#DivFrame${fillDcname} .newgridbtn`).addClass("d-none");
            let gridButton = $(`#DivFrame${fillDcname} .newgridbtn>ul`).html();
            $(`#wrapperForEditFields${fillDcname}`).append(`<div class="clearfix"></div><div class="grdButtons btnMobile"><ul class="left">${gridButton}</ul></div>`);
            $(`#wrapperForEditFields${i + 1} .btnMobile`).find(`#viewGrid${i + 1}`).addClass("d-none");
            $(".dcTitle").hide().nextAll("hr.hrline").hide();
            try {
                if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].selectedLayout != null && designObj[0].selectedLayout != "" && designObj[0].selectedLayout == "tile") {
                    $(".dcTitle").show().nextAll("hr.hrline").show();
                }
            } catch (ex) { }
        }
        gridFreezeCols(fillDcname);
        SetPositionfldDisplayTot();
    } else {
        AxWaitCursor(false);
        ShowDimmer(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
    GetProcessTime();
    GetTotalElapsTime();
}

function UpdateFldArrayInFillgrid(_thisDc) {
    try {
        $(".wrapperForGridData" + _thisDc + " table tbody tr").each(function () {
            $(this).find("td").each(function () {
                let _thisElId = $(this).find("textarea").attr("id");// $(this).attr("id");
                if (typeof _thisElId != "undefined" && _thisElId != "" && !_thisElId.startsWith("axp_recid" + _thisDc)) {
                    _thisElId = _thisElId.replace('EDIT~', '');
                    let _thiselVal = $(this).find("textarea").val(); //GetFieldValueNew(_thisElId);
                    UpdateAllFieldValues(_thisElId, _thiselVal);
                }
            });
        });
    } catch (ex) {

    }
}

function UpdateFldArrayInDeleteRow(_thisDc, _thisRowNo) {
    try {
        let _thisTrId = "sp" + _thisDc + "R" + _thisRowNo + "F" + _thisDc;
        $(".wrapperForGridData" + _thisDc + " table tbody tr#" + _thisTrId).each(function () {
            $(this).find("td").each(function () {
                let _thisElId = $(this).find("textarea").attr("id");
                if (AllFieldNames.length > 0 && typeof _thisElId != "undefined" && _thisElId != "") {
                    _thisElId = _thisElId.replace('EDIT~', '');
                    var idx = $j.inArray(_thisElId, AllFieldNames);
                    if (idx != -1) {
                        AllFieldValues.splice(idx, 1);
                        AllFieldNames.splice(idx, 1);
                    }
                }
            });
        });
    } catch (ex) {

    }
}

function ClearFldArrays() {
    ChangedFields.length = 0;
    ChangedFieldDbRowNo.length = 0;
    ChangedFieldValues.length = 0;
    DeletedDCRows.length = 0;
}

function ParseServiceResult(result, CalledFrom) {
    if (result != "") {
        ClearFldArrays();
        var tabResult = result.split("*♠*");
        var tabDcServiceCalled = result.split("*♠*~");
        if (tabResult[0] != undefined) {
            try {
                AssignLoadValues(tabResult[0].toString(), CalledFrom);
            } catch (ex) {
                AxWaitCursor(false);
                ShowDimmer(false);
            }
        }
        if (tabResult[2] != undefined) {
            try {
                var tempDesignObj = designObj;
                var dcDesignObj = JSON.parse(tabResult[2]);


                if ($.isEmptyObject(dcDesignObj)) {
                    designObj = tempDesignObj;
                } else {
                    //jsonText = tabResult[2];
                    var newIndex = designObj[0].dcs.map(function (obj, index) {
                        return obj.dc_id;
                    });

                    var dcIndex = newIndex.indexOf(dcDesignObj.dc_id);

                    if (dcIndex > -1) {
                        designObj[0].dcs[dcIndex] = dcDesignObj;
                    } else {
                        designObj[0].dcs = _.sortBy([...designObj[0].dcs, dcDesignObj], 'dc_id');
                    }
                }
            } catch (ex) {
                designObj = tempDesignObj;
            }
            jsonText = JSON.stringify(designObj, null, '');
        }
        if (tabResult[1] != undefined) {
            var restxt = tabResult[1].substring(0, ErrLength);

            if (restxt == ErrStr) {
                var nres = tabResult[1].substring(ErrLength, tabResult[1].length - 8);
                AxWaitCursor(false);
                showAlertDialog("error", nres);
            } else {
                AssignHTML(tabResult[1], "FillGrid", CalledFrom);
                if (changeFillGridDc != 0) {
                    CheckShowHideFldsGrid(changeFillGridDc.toString());
                    changeFillGridDc = 0;
                }
                AxWaitCursor(false);
            }
        }
        if (CalledFrom == "GetTabData" && tabDcServiceCalled != undefined) {
            if (tabDcServiceCalled.length > 1 && tabDcServiceCalled[1] == "false") {
                //refer bug - AGI003632 After selecting data from fillgrid popup ,one extra blank row is getting added on first position .(this scenario happening when grid contains expression and which is accept field)
                var dcIdx = $j.inArray(CurrTabNo.toString(), DCFrameNo);
                if (IsDcGrid(CurrTabNo) && DCHasDataRows[dcIdx] == "False")
                    UpdateAxpRowVldInArray(CurrTabNo, "001", "1");

                EvalDcFldExpressions(CurrTabNo);
            }
        }
    }
    try {
        AfterParseServiceResult(CalledFrom);
    } catch (ex) { }
    ShowDimmer(false);
    AxWaitCursor(false);
}

function EvalDcFldExpressions(dcNo) {
    var fields = GetGridFields(dcNo);
    for (var i = 0; i < fields.length; i++) {
        var fldIdnName = fields[i];
        if (fldIdnName == "axp_recid" + dcNo)
            continue;
        if (!IsGridField(fldIdnName)) {
            EvaluateAxFunction(fields[i], fldIdnName, "000F" + dcNo);
        } else {
            EvaluateAxFunction(fields[i], fldIdnName, "001F" + dcNo);
        }
    }
}

//Function which executes the partial disabling in the fill grid dc.
function DoFillControlPrivilege(strSingleLineText) {

    var rno;
    var noofrows = 1;

    if (strSingleLineText == "") {
        return;
    }

    var myfdcJSONObject = eval('(' + strSingleLineText + ')');
    for (var i = 0; i < myfdcJSONObject.root.length; i++) {

        var dcvalues = myfdcJSONObject.root[i].dc;
        var fields = dcvalues.split("###");
        var gdslno = 1;

        for (var fpar = 0; fpar < fields.length; fpar++) {

            var valSplit = fields[fpar].toString();
            valSplit = valSplit.split('~');
            rno = noofrows + "";
            var rnolength = rno.length;
            if (rnolength == 2) nrno = "0" + rno;
            else if (rnolength == 1) nrno = "00" + rno;
            else if (rnolength == 3) nrno = rno;
            nrno = nrno + 'F' + fillDcname;
            var fldVal = "";

            for (m = 0; m < valSplit.length; m++) {

                var FldSplit = valSplit[m].split("=");
                if (FldSplit[0].toString().indexOf("axpvalid") != -1) {
                    fldVal = FldSplit[1];
                }
                if (fldVal != "") {

                    var strValue = fldVal.toString().toUpperCase();
                    //B-Delete button disable
                    if (strValue == "B") {
                        //Get current img button and disabled here

                        var delImg = window.opener.document.getElementById("del" + nrno);
                        if (delImg) {
                            delImg.removeAttribute("onclick");
                            delImg.removeAttribute("onmouseover");
                            delImg.removeAttribute("onmouseout");
                        }
                    }
                    //C-Current Row disable
                    if (strValue == "C") {

                        //B-Delete button disable
                        var delImg = window.opener.document.getElementById("del" + nrno);
                        if (delImg) {
                            delImg.removeAttribute("onclick");
                            delImg.removeAttribute("onmouseover");
                            delImg.removeAttribute("onmouseout");
                        }
                        //Row disable
                        for (var l = 0; l < valSplit.length; l++) {

                            var FldSplit = valSplit[l].split("=");
                            var FieldId = window.opener.document.getElementById(FldSplit[0] + nrno);
                            if (FieldId) {
                                var id = FieldId.id;
                                window.opener.document.getElementById(id).disabled = "true";
                            }
                        }
                        break;
                    }
                }
            }
            dnrno = "'" + nrno + "'";
            noofrows = noofrows + 1;
        }
    }
}

//Function to check all the row once the header row is checked.
function CheckAll(obj, exprResult) {
    //The exprResult will contain the result of the expression evaluation.
    let _thisFgInd = $j.inArray(AxActivefillGridName, FillGridName);
    if (exprResult == "t" || exprResult == "true") {
        $j("input[name=chkItem]:checkbox").each(function () {
            if ($j(this).prop("disabled") == false && $j(this).parent().parent().parent().css("display") != "none")
                $j(this).prop("checked", obj.checked);
            totalFillGridColVal($j(this), _thisFgInd);
            if ($(this).prop('checked')) {
                let rowIndex = fillGridDatatbl.row($j(this).parents('tr')).index();
                let arrayIndex = MultiFillgridSldIndex.indexOf(rowIndex);
                if (arrayIndex == -1) {
                    MultiFillgridSldIndex.push(fillGridDatatbl.row($j(this).parents('tr')).index());
                }
            } else {
                let rowIndex = fillGridDatatbl.row($(this).parents('tr')).index();
                let arrayIndex = MultiFillgridSldIndex.indexOf(rowIndex);
                if (arrayIndex > -1) {
                    MultiFillgridSldIndex.splice(arrayIndex, 1);
                }
            }
        });
    } else {
        obj.checked = false;

    }
    $j("#dvFillGrid").height($j("#dvFillGrid").height());
}

//Function to check the header checkbox is all the checkboxes are checked.
function ChkHdrCheckbox(obj, exprResult) {
    //The exprResult will contain the result of the expression evaluation.
    let _thisFgInd = $j.inArray(AxActivefillGridName, FillGridName);
    if (exprResult == "t" || exprResult == "true") {
        totalFillGridColVal(obj, _thisFgInd);
        if ($j(".fgChk").parents("table.gridData tbody").find(".fgChk:visible").length == $j(".fgChk").parents("table.gridData tbody").find(".fgChk:checked").length)
            $j(".fgHdrChk").parents("table.gridData thead").find(".fgHdrChk").prop("checked", true);
        else
            $j(".fgHdrChk").parents("table.gridData thead").find(".fgHdrChk").prop("checked", false);
    } else {
        obj.checked = false;
    }
    if ($(obj).prop('checked')) {
        let rowIndex = fillGridDatatbl.row($j(obj).parents('tr')).index();
        let arrayIndex = MultiFillgridSldIndex.indexOf(rowIndex);
        if (arrayIndex == -1) {
            MultiFillgridSldIndex.push(fillGridDatatbl.row($j(obj).parents('tr')).index());
        }
    } else {
        let rowIndex = fillGridDatatbl.row($(obj).parents('tr')).index();
        let arrayIndex = MultiFillgridSldIndex.indexOf(rowIndex);
        if (arrayIndex > -1) {
            MultiFillgridSldIndex.splice(arrayIndex, 1);
        }
    }
    $j("#dvFillGrid").height($j("#dvFillGrid").height());
}

function totalFillGridColVal(obj, _thisFgInd) {
    if (typeof FillGridTotalFldVal != "undefined" && FillGridTotalFldVal.length > 0 && FillGridTotalFldVal[_thisFgInd] != "") {
        var _inputString = FillGridTotalFldVal[_thisFgInd];
        //const regex = /=\s*:(\w+)/g;
        //let match;
        //let _matchinflds = [];
        //while ((match = regex.exec(_inputString)) !== null) {
        //    let parts = match[1].split('_');
        //    parts = parts.slice(1).join('_');
        //    _matchinflds.push(parts);
        //}
        const regex = /:tot_(\w+)/g;
        let match;
        let _matchinflds = [];
        while ((match = regex.exec(_inputString)) !== null) {
            _matchinflds.push(match[1]);
        }

        let matchingThIndexes = [];
        _matchinflds.forEach(field => {
            let matched = false;
            $(obj).parents("table").find("thead tr th").each(function (index) {
                if (typeof $(this).attr('orgthname') != "undefined" && $(this).attr('orgthname').trim().toLowerCase() === field.toLowerCase()) {
                    matchingThIndexes.push(index);
                    matched = true;
                    return false;
                }
            });
            if (!matched) {
                matchingThIndexes.push(-1);
            }
        });
        let columnSums = new Array(matchingThIndexes.length).fill(0);
        $(obj).parents("table").find("tbody tr").each(function () {
            if ($(this).find("td:eq(0) input[type=checkbox]").is(":checked")) {
                matchingThIndexes.forEach((index, i) => {
                    if (index != -1) {
                        let tdValue = $(this).find(`td:eq(${index})`).text().trim();
                        let numericValue = parseFloat(tdValue.replace(/[^0-9.-]/g, ''));
                        if (!isNaN(numericValue)) {
                            let decimalPart = numericValue.toString().split('.')[1];
                            if (typeof decimalPart != "undefined" && decimalPart.length > 0) {
                                let decimalLength = decimalPart.length;
                                columnSums[i] += numericValue;
                                columnSums[i] = RoundOff(columnSums[i], decimalLength);
                            } else
                                columnSums[i] += numericValue;
                        }
                    }
                });
            } else
                $(".modal-footer #dvfgTotal").remove();
        });
        _matchinflds.forEach(function (val, ind) {
            let _forTotVal = columnSums[ind];
            try {
                let decimalPart = _forTotVal.toString().split('.')[1];
                if (typeof decimalPart != "undefined" && decimalPart.length > 0) {
                    let decimalLength = decimalPart.length;
                    _forTotVal = CommaFormatted(fixit(columnSums[ind], decimalLength));
                } else
                    _forTotVal = CommaFormatted(columnSums[ind]);
            } catch (ex) { }
            _inputString = _inputString.replace(":tot_" + val, _forTotVal);
        });
        if ($(".modal-footer #dvfgTotal").length > 0)
            $(".modal-footer #dvfgTotal").text(_inputString);
        else
            $(".modal-footer").prepend(`<span id="dvfgTotal" class="ms-0 me-auto" style="font-weight: 600;">${_inputString}</span>`);
    }
}

function DeleteAllRows(dcNo, RowCount, calledfrom="") {
    var rCount = parseInt(RowCount, 10);
    if (rCount > 1 && !axInlineGridEdit && AxpGridForm != "form") {
        grdRCOunt = $("#gridHd" + dcNo + " tbody tr").length;
        if (grdRCOunt == rCount)
            rCount++;
    }

    for (var i = axInlineGridEdit ? rCount : (AxpGridForm == "form") ? rCount : rCount - 1; i >= 1; i--) {
        DeleteGridRow(dcNo, GetClientRowNo(i, dcNo), "all");
    }
    if (!axInlineGridEdit && AxpGridForm == "form") {
        $("#divDc" + dcNo + " .grid-icons").append(gridDivHtml[dcNo]);
        $("#divDc" + dcNo + " .formGridRow").remove();
        if ($(".wrapperForGridData" + dcNo + " table tbody tr").length == 0) {
            adjustEditLayoutId(dcNo);
            if (typeof ClientRowNo != "undefined" && typeof ClientRowNo[0] == "undefined")
                ClientRowNo[0] = "001";
        }
        setDesignedLayout("divDc" + dcNo);
        if (calledfrom == "FillGrid") {
            $(`[id^="dcCaptionBtn${dcNo}-"]`).each(function () {
                const $next = $(this).next();
                if ($next.is('hr.text-gray-500.row-sep')) {
                    $next.remove();
                }
                $(this).remove();                
            });
        }
        if (calledfrom != "FillGrid")
            editTheRow("", dcNo, "", event);

    } else {
        adjustEditLayoutId(dcNo);
        AxActiveRowNo = "1";
        var fields = GetGridFields(dcNo);
        if (typeof wsPerfEnabled != "undefined" && wsPerfEnabled)
            CallEvaluateOnAddPerf(dcNo, "001", fields, "AllRowClear");
        else
            CallEvaluateOnAdd(dcNo, "001", fields);

        if (!IsAddRowCalled) {
            IsDcPopGridCleared = true;
            DeleteGridRow(dcNo, "001F" + dcNo, undefined);
        } else {
            if (!axInlineGridEdit) {
                UpdateDcRowArrays(dcNo, "001", "Add");
            }
            setTimeout(function () {
                ShowDimmer(false);
                AxWaitCursor(false);
            }, 0);
        }
    }
    SetPositionfldDisplayTot();
}

function ChangeDir(dir) {
    $j("#form1").attr("dir", dir);
}

function LoadPdfDDL(ddlStr) {
    document.getElementById("dvPdfDDl").innerHTML = ddlStr;
}

function tstSearchTable() {
    setTimeout(function () {
        var jTbl = $j('#grdSearchRes');
        if (jTbl.find("tbody>tr>th").length > 0) {
            jTbl.find("tbody").before("<thead><tr></tr></thead>");
            jTbl.find("thead:first tr").append(jTbl.find("th"));
            jTbl.find("tbody tr:first").remove();
        }
        osTable = jTbl.DataTable({
            "searching": false,
            "dom": '<"float-end"f><"d-block w-100 overflow-auto"t>',
            "paging": false,
            "ordering": true,
            "info": false,
            "scrollCollapse": true,
            "order": [],
            "columnDefs": [
                {
                    'orderable': false,
                    'targets': 0
                },
                {
                    'orderable': true,
                    'targets': '_all'
                }
            ]
        });
    }, 0);
}

function bindUpdownEvents(idOfTheElement, typeOfSelect) {
    unbindKeyDownEvent();
    if (typeOfSelect != 'single') {
        $("#" + idOfTheElement + " input[type='checkbox']").each(function () {
            $(this).parents('td,th').addClass('text-center');
        });

    }
    if (jQuery("#" + idOfTheElement + " tr:nth-child(2)").length > 0) {
        //jQuery("#" + idOfTheElement + " tr:nth-child(2)").addClass('activeSearchTr').find("input").focus();
    }
    if (jQuery('#' + idOfTheElement).length > 0) {
        jQuery("#" + idOfTheElement).on("keydown.tstrctSrch", "tr:not(:first)", function (event) {
            var elemm = $(this);
            $(elemm).addClass('activeSearchTr');
            if (event.keyCode == 9 && !event.shiftKey) {

                if (jQuery("#" + idOfTheElement).find('tr.activeSearchTr').prevAll("tr:visible").length > 0) {
                    if (typeOfSelect == "multiple") {
                        checkTheHeight(idOfTheElement, elemm, 'bottom');
                    }

                    if ($(elemm).next().is("tr")) {
                        jQuery("#" + idOfTheElement).find('tr.activeSearchTr').removeClass('activeSearchTr');
                        $(elemm).next().addClass('activeSearchTr');
                        jQuery('.activeSearchTr').find("input").focus();
                        event.preventDefault();
                    }
                }

            } else if (event.keyCode == 9 && event.shiftKey) {
                if (jQuery("#" + idOfTheElement).find('tr.activeSearchTr').prevAll("tr:visible").length > 0) {
                    if (typeOfSelect == "multiple") {
                        checkTheHeight(idOfTheElement, elemm, 'top');
                    }

                    if ($(elemm).prev().is("tr") && (!$(elemm).prev().is("#" + idOfTheElement + " tr:first"))) {
                        jQuery("#" + idOfTheElement).find('tr.activeSearchTr').removeClass('activeSearchTr');
                        $(elemm).prev().addClass('activeSearchTr');
                        jQuery('.activeSearchTr').find("input").focus();
                        event.preventDefault();
                    }
                }

            } else if (event.keyCode == 38) {
                //up arrow
                event.preventDefault();
                var index = jQuery("#" + idOfTheElement).find('tr.activeSearchTr').index();
                if (index != 0 && jQuery("#" + idOfTheElement).find('tr.activeSearchTr').prevAll("tr:visible").length > 0) {
                    if (typeOfSelect == "multiple") {
                        checkTheHeight(idOfTheElement, elemm, 'top');
                    }
                    jQuery("#" + idOfTheElement).find('tr.activeSearchTr').removeClass('activeSearchTr').prevAll("tr:visible:first").addClass('activeSearchTr').find("input").focus();
                }
            } else if (event.keyCode == 40) {
                //down arrow
                if (typeOfSelect == "single") {
                    event.preventDefault();
                }
                var presntIndex = jQuery("#" + idOfTheElement).find('tr.activeSearchTr').index();

                var lstIndex = jQuery("#" + idOfTheElement).find('tr').last().index();
                if (presntIndex < lstIndex && jQuery("#" + idOfTheElement).find('tr.activeSearchTr').nextAll("tr:visible").length > 0) {
                    if (typeOfSelect == "multiple") {
                        checkTheHeight(idOfTheElement, elemm, 'bottom');
                    }
                    jQuery("#" + idOfTheElement).find('tr.activeSearchTr').removeClass('activeSearchTr').nextAll("tr:visible:first").addClass('activeSearchTr').find("input").focus();
                }
            } else if (event.keyCode == 32) {
                if ($(event.target).attr('id') != "searchInput") {
                    //event.preventDefault();
                    //jQuery("#" + idOfTheElement + " tr.activeSearchTr input").click();
                    if (typeOfSelect == "single") {
                        unbindKeyDownEvent();
                    }
                }
            } else if (event.keyCode == 13 && $(event.target).attr('id') != "searchInput") {
                if (typeOfSelect == "multiple") {
                    event.preventDefault();
                    unbindKeyDownEvent();
                    //here we have to hit the ok button
                    $('#dvFillGrid').parents('.modal-body').next().find("#modalCnfirmbtn").click();

                } else {
                    jQuery("#" + idOfTheElement + " tr.activeSearchTr input").click();
                    if (typeOfSelect == "single") {
                        unbindKeyDownEvent();
                    }
                }
                unbindKeyDownEvent();
            }

        });

        jQuery("#" + idOfTheElement).on("keydown.tstrctSrch", "tr:first", function (event) {
            if (event.keyCode == 13) {
                $('#dvFillGrid').parents('.modal-body').next().find("#modalCnfirmbtn").click();
            }
        });

        jQuery("#" + idOfTheElement + " tr").hover(function () {
            /* Stuff to do when the mouse enters the element */
            jQuery("#" + idOfTheElement + " tr.activeSearchTr").removeClass('activeSearchTr');
            // jQuery(this).addClass('activeSearchTr').find("input").focus();
            //jQuery('.activeSearchTr').find("input").focus();
        }, function () {
            /* Stuff to do when the mouse leaves the element */
        });

        jQuery("#" + idOfTheElement + " tr").click(function (event) {
            if (typeOfSelect == "single") {
                unbindKeyDownEvent();
            }
            if ($(event.target).attr('type') != "checkbox" && $(event.target).attr('type') != "radio" && $(this).parent("thead").length == 0) {
                typeOfSelect == "single" ? (idOfTheElement == 'grdSearchRes' ? loadTstruct(jQuery(this).find('input[type="radio"]').val()) : jQuery(this).find('input[type="checkbox"]').click()) : jQuery(this).find('input[type="checkbox"]').click();
            }
        });

    }

    if (idOfTheElement == "grdSearchRes")
        tstSearchTable();
}

function checkTheHeight(idOfTheElement, elemm, direction) {
    var heightOfFrame = parseInt(jQuery("#dvFillGrid").parents('.modal-body-content').css('height').replace("px", ""));
    var frameScrollTop = jQuery("#dvFillGrid").parents('.modal-body-content').scrollTop();
    var currentTr = $(elemm);
    var nextTrheight = $(elemm).next().height();
    var prevTrheight = $(elemm).prev().height();
    var topOfCurrentTr = currentTr.position().top;
    // event.preventDefault();
    if (direction == "bottom") {
        if ((topOfCurrentTr + nextTrheight + 50) > heightOfFrame) {
            jQuery("#dvFillGrid").parents('.modal-body-content').animate({
                scrollTop: frameScrollTop + nextTrheight + 30
            }, 'fast');
        }
    } else if (direction == "top") {
        if ((topOfCurrentTr - prevTrheight) < 0) {
            jQuery("#dvFillGrid").parents('.modal-body-content').animate({
                scrollTop: frameScrollTop - prevTrheight - 30
            }, 'fast');
        }
    }
}

function unbindKeyDownEvent() {
    jQuery(document).unbind("keydown.tstrctSrch");
}


$('body').on('click', function (e) {
    $('[data-toggle=popover]').each(function () {
        // hide any open popovers when the anywhere else in the body is clicked
        if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
            $(this).popover('hide');
        }
    });
});

// Searching for input filed with partial id 'axp_colmerge_dcno'
$j(function () {
    if (callParentNew("originalUri") != "") {
        return;
    }
    //Works only on active fields available on pageload.
    let _thsiifId = window.frameElement.id;
    if (_thsiifId != 'middle1' && typeof VisibleDCs == "undefined")
        return;

    for (var i = 0; i < VisibleDCs.length; i++) {
        if (DCIsGrid[i].toLowerCase() == "true") {
            if ($j('#axp_colmerge_' + VisibleDCs[i] + '000F1').length > 0) {
                var dcColumnValue = $j('#axp_colmerge_' + VisibleDCs[i] + '000F1');
                dcColumnValue = dcColumnValue[0].value;
                if (dcColumnValue != null && dcColumnValue != '' && dcColumnValue != undefined) {
                    GetGridDcTable(dcColumnValue, VisibleDCs[i]);
                }
            }
        }
    }
});
/// get the column values and the dc id...
function GetGridDcTable(strCond, dcNo) {
    var iDtoFind = 'gridHd' + dcNo;
    var table = document.getElementById(iDtoFind);
    const existingThead = table.querySelector('thead');
    const newThead = existingThead.cloneNode(true);
    table.insertBefore(newThead, table.childNodes[0]);
    var arrMainHeader = strCond.split('#');
    for (var j = 0; j < newThead.children[0].children.length; j++) {
        var mergeRowContent = arrMainHeader[0].split(',').toString();
        var columns = mergeRowContent.substr(mergeRowContent.indexOf("~") + 1);
        var arrColumns = columns.split(',');
        var mainHeader = mergeRowContent.split("~", 1).toString().replace('"', '').trim(); // Trim whitespace

        for (var k = 0; k < arrColumns.length; k++) {
            var thName = "th-" + arrColumns[k].trim(); // Adjust to match your actual th id pattern
            var selectedItem = newThead.children[0].children[j];
            if (selectedItem.id == thName) {
                var initialWidth = selectedItem.offsetWidth; // Store initial width
                selectedItem.setAttribute('colspan', arrColumns.length);
                selectedItem.innerHTML = '<div id="div-' + arrColumns[0] + '" style="text-align: center;"><b style="font-weight: bold;">' + mainHeader + '</b></div>';

                for (var m = j + 1; m < j + arrColumns.length; m++) {
                    initialWidth = getTotalWidth(newThead.children[0].children[m]);// newThead.children[0].children[m].offsetWidth;
                    newThead.children[0].children[m].classList.add('d-none');
                    newThead.children[0].children[m].innerHTML = "";
                    newThead.children[0].children[m].removeAttribute('style');
                }
                if (arrMainHeader.length > 1) {
                    arrMainHeader.splice(0, 1);
                }
                j = j + arrColumns.length - 1; // Skip over the merged columns
                break; // Exit the loop after merging
            } else {
                selectedItem.innerHTML = "";
            }
        }
    }

    // Modify IDs using jQuery
    $("#" + iDtoFind + " thead:first tr th").each(function () {
        var currentId = $(this).attr('id');
        var newId = 'a-' + currentId;
        $(this).attr('id', newId);
    });
}
function getTotalWidth(element) {
    const styles = window.getComputedStyle(element);
    const borderLeftWidth = parseFloat(styles['borderLeftWidth']);
    const borderRightWidth = parseFloat(styles['borderRightWidth']);
    const paddingLeft = parseFloat(styles['paddingLeft']);
    const paddingRight = parseFloat(styles['paddingRight']);
    const width = element.offsetWidth;

    const totalWidth = width + borderLeftWidth + borderRightWidth + paddingLeft + paddingRight;
    return totalWidth;
}

/// get the column values and the dc id...
//function GetGridDcTable(strCond, dcNo) {
//    //Generate Id according to the dcNo.
//    var iDtoFind = 'gridHd' + dcNo;
//    var table = document.getElementById(iDtoFind);
//    var arrMainHeader = strCond.split('#'); // Split each value from string to create arrays.
//    var htmlConcat = table.outerHTML;
//    htmlConcat = htmlConcat.replace(iDtoFind, 'mergeHd' + dcNo); // assign html to the table with id as MergeHD + dc name for further changes.
//    $j('#' + iDtoFind).before(htmlConcat);

//    var mergeTable = $j('#mergeHd' + dcNo);
//    var tableCells = mergeTable[0].childNodes[0].childNodes[0].childNodes;
//    var count = 0;

//    // loop through each table cell then checking it the array of columns to be merged...
//    for (var j = 0; j < tableCells.length; j++) {
//        var mergeRowContent = arrMainHeader[0].split(',').toString();
//        var columns = mergeRowContent.substr(mergeRowContent.indexOf("~") + 1);
//        var arrColumns = columns.split(',');
//        var mainHeader = mergeRowContent.split("~", 1).toString();
//        // loop through each arrColumn to check if the value is equal to the table cell.
//        for (var k = 0; k < arrColumns.length; k++) {
//            var selectedItem = tableCells[j];
//            selectedItem.innerHTML = '';
//            thName = "th-" + arrColumns[k];
//            if (selectedItem.id == thName) {
//                SetHeader(arrColumns, k, mainHeader);
//                if (arrMainHeader.length > 1) {
//                    arrMainHeader.splice(0, 1);
//                }
//                j = (j + arrColumns.length) - 1;
//            }
//            break;
//        }
//    }
//}

//Clear inner html for each table cell and headers
function SetHeader(arrColumns, index, mainHeader) {

    for (var o = 0; o < arrColumns.length; o++) {
        theadName = "th-" + arrColumns[index + o];
        var selectedItem = document.getElementById(theadName);

        //change the color of the border to grey only if the element is not the last in array.
        if ((o - (arrColumns.length - 1)) != 0) {
            selectedItem.style.borderColor = 'grey';
        }
        if (o == 0) {
            selectedItem.innerHTML = '<div id="div-' + arrColumns[0] + '"><b style="font-weight:101">' + mainHeader + '</b></div>';
            selectedItem.style.borderBottom = 'solid 1px white';
            selectedItem.style.maxWidth = selectedItem.style.width;
        } else {
            selectedItem.innerHTML = "";
            selectedItem.style.borderBottom = 'solid 1px white';
        }
    }


    SetHeaderWidth(arrColumns, arrColumns[0], index);
}


//Set Width to the header..
function SetHeaderWidth(arrColumns, divID, index) {

    var totalWidth = parseInt(0);
    var currenElementWidth = parseInt(0);
    var selectedDiv = $j('#div-' + divID);
    for (var i = 0; i < arrColumns.length; i++) {
        theadName = "th-" + arrColumns[index + i];
        currenElementWidth = parseInt(document.getElementById(theadName).offsetWidth);
        totalWidth = (totalWidth + currenElementWidth);
    }

    if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
        var selectedDiv = document.getElementById('div-' + divID);
        //replace space with a line break for the header to be aligned.
        if (selectedDiv.childNodes[0].innerHTML.indexOf("_") > -1) {
            selectedDiv.style.width = totalWidth + "px";
            selectedDiv.style.maxWidth = totalWidth + "px";
            selectedDiv.childNodes[0].innerHTML = selectedDiv.childNodes[0].innerHTML.replace(/_/g, " ");
        } else {
            selectedDiv.style.width = totalWidth + "px";
            selectedDiv.style.maxWidth = totalWidth + "px";
        }
    } else {
        var selectedDiv = $j('#div-' + divID);
        //replace space with a line break for the header to be aligned.
        if (selectedDiv[0].innerText.indexOf("_") > -1) {
            selectedDiv.css({
                width: totalWidth,
                maxWidth: totalWidth
            });

            selectedDiv[0].innerText = selectedDiv[0].innerText.replace(/_/g, " ");

        } else {
            totalWidth = (totalWidth * 40) / 100;
            selectedDiv.css({
                marginLeft: totalWidth
            });
        }
    }

}

// This file contains the popup grid related functions.
//
//------------------List of functions in this file--------------------------------------------
//OpenPopUp(parentfldName, popDcNo) -Function to display the popUp for the current parent row.
//ClosePopUp(popDcNo, validate) -Function to hide the opened pop up.
//ClearPopUp(popDcNo) -Function which deletes all the rows except the first row, and clears the values in  the first row.
//UpdatePopUpParents(fieldName) -Function to update the changed value of the parent into the arrays.
//CheckDuplicateParents(dcNo, ParentFlds, parentStr) -Function to check for duplicate entries in the parent grid for the parent fields.
//SetPopParents(parentDcNo, popDcNo, parentRowNo, parentStr) -Function to set the concatenated parent value to the array.
//ShowPopUpDiv(dcNo) -Function to set the dimmer on the tstruct, set pop up position and display the popup.
//DisableDeleteForFirmDc(isFirm, popDcNo, rowNo) -Function to diable the delete button in the pop dc if the dc is firm bind.
//DisplaySummaryInPopUp(popDcNo) -Function to display the summary defined for the pop grid.
//GetPopRows(parentDcNo, ParentRowNo, popDcNo) -Function to get the sub grid rows for the current active parent row.
//ClearPopRows(parentDc, parentRow, popDc) -Function to clear the poprows for the given parent row and dc no.
//AddDeletePopRow(parentDcNo, parentRowNo, popDcNo, popRowNo) -Function to add a new row for the given pop up dc no.
//Ax_BeforeHidePopUp(dcNo) -Function to customize validations before closing the popup.
//IsEmptyRow(rowNo, dcNo) -Function which returns true if the given row is empty.
//SetPopDcStyle(PopDcNo, isFirm) -Function to set style for the pop grid dc if firm bind.
//RegisterActivePRow(clientRowNo, pDcNo) -Function to set the Parents active row to the global variable.
//IsPopGridFirmBind(popDcNo) -Function returns true if the PopDc has firm bind attribute as true.
//DisplayHidePopRow(rowNo, style) -Function to Display or hide the row html in the subgrid.
//GetParentFields(popDcNo) -Function to get the parent fields for the given pop dc.
//GetParentString(parentDcNo, parentRow, popDc) -Function to get the parent values concatenated string for the given parent and pop dc.
//GetPopGrids(parentDcNo) -Function which returs the pop grids for the current parent dc.
//AddParentRow(parentDcNo, parentRowNo, popDcNo, parentStr) -Function which adds the parent row info to the pop arrays.
//UpdatePopUpArrays(dcNo, rowNo, isPopDc) -Function which updates the popRows and rowno arrays for the newly added row.

//---------------------------------------------------------------------------------------------
var popUpTitle = "";
//Function to display the popUp for the current parent row.
function OpenPopUp(parentfldName, popRowDcNo) {

    var pRowNo = GetFieldsRowNo(parentfldName);
    var parentDcNo = GetFieldsDcNo(parentfldName);
    AxActivePDc = parentDcNo;
    RegisterActivePRow(pRowNo, parentDcNo);
    AxIsPopActive = true;
    var popDcNo = popRowDcNo.substring(popRowDcNo.lastIndexOf("F") + 1);
    AxActiveDc = popDcNo;

    var pIdx = $j.inArray(popDcNo, PopGridDCs); //PopCondition
    if (pIdx != -1) {
        var result = "f";
        if (PopCondition[pIdx] != "") {
            result = EvalPrepared("", "000", PopCondition[pIdx], "expr");
            if (result.toLowerCase() == "f") {
                showAlertDialog("error", 2006, "client");
                return;
            }
        }
    }

    var isPopFirm = IsPopGridFirmBind(popDcNo);
    SetPopDcStyle(popDcNo, isPopFirm);
    var popRowStr = GetPopRows(parentDcNo, pRowNo, popDcNo);
    var popRows = popRowStr.split(",");
    var rowCntFld = "#hdnRCntDc" + popDcNo;
    var popRowCnt = 0;
    popRowCnt = parseInt($j(rowCntFld).val(), 10);

    popUpTitle = $j("#popDcTitle" + popDcNo).val();
    popUpTitle = "<span style='font-size: 20PX;color: white;font-weight: bold;'>" + popUpTitle + "</span>";


    //hide all rows in the table
    $j("#gridDc" + popDcNo + " tbody tr").each(function () {
        var tRow = $j(this);
        $j(this).hide();
    });

    for (var j = 0; j < popRows.length; j++) {
        var rowId = "#sp" + popDcNo + "R" + popRows[j] + "F" + popDcNo;
        $j(rowId).show();
    }

    if ($j(".AxAddRows").length > 0)
        $j(".AxAddRows").val('1');
    //This condition checks
    //If the parent row does not have any sub grid row
    //All the parent fields are bound in the parent row
    if (popRowStr == "" && IsPopParBound(popDcNo, parentDcNo, pRowNo) == true) {

        var newRowNo = "";
        var rCnt = GetDcRowCount(popDcNo);
        if (rCnt == 1 && IsEmptyRow("001", popDcNo)) {
            var isOldRow = IsRowInPopRows(parentDcNo, popDcNo, pRowNo, "001");
            if (isOldRow == false) {
                newRowNo = 1;
                UpdatePopUpArrays(popDcNo, "001", true, "Add");
                UpdatePopParentFlds(popDcNo, parentDcNo, "001", pRowNo);
                SetRowCount(popDcNo, "1");
                $j("#sp" + popDcNo + "R" + "001" + "F" + popDcNo).show();
                RefreshFillFldsInSubGrid(popDcNo, "001");
                AxActiveRowNo = "1";
                CallGetSubGridDDL(popDcNo, pRowNo, parentDcNo, "001");
            }
        } else {
            AddSubGridRow(popDcNo);
        }
    } else {
        //for all the fill fields in the subgrid, if there is a parent in the nongrid, call combofilldep

        RefreshFillFldsInSubGrid(popDcNo, popRowStr);
        $j("#DivFrame" + popDcNo).dialog({
            title: popUpTitle,
            height: 400,
            width: 550,
            close: function () {
                ClosePopUp(popDcNo);
            },
            position: 'top',
            modal: true,
            buttons: {
                "Ok": function () {
                    ClosePopUp(popDcNo, $j(this));
                }
            }
        });
    }
}

//Function to refresh the fields in the subgrid which have the combo parents in the primary dc.
function RefreshFillFldsInSubGrid(dcNo, rowStr) {
    var fields = GetGridFields(dcNo);
    for (var i = 0; i < fields.length; i++) {
        var idx = GetFieldIndex(fields[i]);
        var parentStr = FldParents[idx].toString().split(",");
        if (FMoe[idx] == "Fill" && parentStr != "") {

            for (var j = 0; j < parentStr.length; j++) {
                var parDcNo = GetDcNo(parentStr[j]);
                if (!IsDcGrid(parDcNo) && $j.inArray(parentStr[j], ComboParentField) != -1) {
                    var pFldValue = GetFieldValue(parentStr[j] + "000F" + parDcNo);
                    var fieldID = parentStr[j] + "000F" + parDcNo;
                    var fName = parentStr[j];


                    for (var ind = 0; ind < ComboParentField.length; ind++) {
                        //var parFldName = GetFieldsName(ComboParentField[i]);
                        if (ComboParentField[ind] == fName && ComboParentValue[ind] == pFldValue) {
                            var strRows = rowStr.split(",");
                            for (var k = 0; k < strRows; k++) {
                                if (strRows[k] == "")
                                    continue;

                                var rowFrameNo = strRows[k] + "F" + dcNo;
                                var dbRowNo = GetDbRowNo(strRows[k], dcNo);

                                var depFldId = ComboDepField[ind] + rowFrameNo;
                                var depFldValue = ComboDepValue[ind].toString();
                                var depFld = $j("#" + depFldId);
                                if (depFld.length > 0) {
                                    CallSetFieldValue(depFldId, depFldValue);
                                    UpdateFieldArray(depFldId, dbRowNo, depFldValue, "parent");
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}

function AddSubGridRow(dcNo, calledFrom) {
    var tableName = "gridDc" + dcNo;
    var newRow = "";
    var sTRowIndx = -1;
    newRow = $j('#' + tableName + ' tbody>tr:last').clone(true);
    var oldRowNo = newRow.attr("id");

    var hdnRowCount = $j("#hdnRCntDc" + dcNo);
    if (hdnRowCount.length > 0) {

        var dcClientRows = GetDcClientRows(dcNo);
        var lastRow = dcClientRows.getMaxVal();
        var rowCount = $j("#hdnRCntDc" + dcNo).val();
        var newCnt = parseInt(lastRow, 10) + 1;
        var newRowNo = GetRowNoHelper(newCnt);
        newRow.attr("id", "sp" + dcNo + "R" + newRowNo + "F" + dcNo);
        newRow.find(':input').val('');
        newRow.find('select,input,label,img,.Grdlnk').each(function () {
            ConvertFieldID($j(this), newRowNo);
        });

        RegisterActiveRow(newRowNo, dcNo);
        AxActiveDc = dcNo;
        var glType = eval(callParent('gllangType'));
        var dtpkrRTL = false;
        if (glType == "ar")
            dtpkrRTL = true;
        else
            dtpkrRTL = false;
        var glCulture = eval(callParent('glCulture'));
        var dtFormat = "dd/mm/yy";
        if (glCulture == "en-us")
            dtFormat = "mm/dd/yy";
        $j(".date").datepicker({
            isRTL: dtpkrRTL,
            dateFormat: dtFormat,
            showOn: "both",
            buttonImage: "../AxpImages/icons/16x16/calendar.png",
            buttonImageOnly: true,
            buttonText: "Select date",
            changeMonth: true,
            changeYear: true,
            yearRange: "-100:+100"
        });
        delImg = newRow.find('.rowdelete');
        if (delImg.attr("id") != undefined)
            delImg.attr("id", ("del" + newRowNo + "F" + dcNo));
        newRow.insertAfter('#' + tableName + ' tbody>tr:last');
        $j("#sp" + dcNo + "R" + newRowNo + "F" + dcNo).show();

        //TODO: the SetRowCount need not be called
        //if the web service is returning the row since it is already updated in the server array.
        if (calledFrom == undefined)
            SetRowCount(dcNo, newCnt, "i" + newRowNo);
        else
            SetRowCount(dcNo, parseInt(rowCount, 10) + 1);

        if (calledFrom == "GetDep")
            UpdateNewPopInfo(dcNo, newRowNo);

        var rowFrameNo = newRowNo + "F" + dcNo;
        //On clearing the rows, the row will be added to the DeletedDCRows,
        //if you add the same row agian then the row should be removed from the DeletedDCRows

        var ind = $j.inArray(rowFrameNo, DeletedDCRows);
        if (ind != -1)
            DeletedDCRows.splice(ind, 1);

        CheckScroll(dcNo);

        UpdateDcRowArrays(dcNo, newRowNo, "Add");
        if (calledFrom == undefined) {
            UpdateFieldsInNewRow(dcNo, newRowNo);
            UpdatePopParentFlds(dcNo, AxActivePDc, newRowNo, GetClientRowNo(AxActivePRow, AxActivePDc));
        }
        IsFunction = "";

        if (calledFrom == "GetDep") {
            if (AxParStrFromDep != "") {
                var parStr = AxParStrFromDep.split("~");
                var parDc = parStr[0];
                var pRow = GetClientRowNo(parStr[1], parDc);
                UpdatePopUpArrays(dcNo, newRowNo, true, "Add", pRow, parDc);
            } else
                UpdatePopUpArrays(dcNo, newRowNo, true, "Add");
        } else
            UpdatePopUpArrays(dcNo, newRowNo, true, "Add");

        if (calledFrom == undefined) {
            RefreshFillFldsInSubGrid(dcNo, newRowNo);
            if (subGridJson == "")
                CallGetSubGridDDL(dcNo, AxActivePRow, AxActivePDc, newRowNo);
            else
                ApplyPopJson(dcNo);
        }

        adjustwin("10", window);
    }
}

function CallGetSubGridDDL(popDcNo, pRowNo, parentDcNo, newRowNo) {
    try {
        var recId = $j("#recordid000F0").val();
        ASB.WebService.GetSubGridDropdown(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, popDcNo, tstDataId, recId, pRowNo, parentDcNo, newRowNo, resTstHtmlLS, SuccessSubGridCombos, OnException);
    } catch (exp) {
        AxWaitCursor(false);
        showAlertDialog("error", ServerErrMsg);
    }
}

var subGridJson = "";

function SuccessSubGridCombos(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    ChangedFields = new Array();
    ChangedFieldDbRowNo = new Array();
    ChangedFieldValues = new Array();
    DeletedDCRows = new Array();
    if (result != "") {
        subGridJson = result;
        var dcClientRows = GetDcClientRows(AxActiveDc);
        AxPopRowNo = dcClientRows.getMaxVal();
        AssignLoadValues(result, "PopGridCombos");
        try {
            AxAfterSubGridAddRow();
        } catch (ex) { }
        $j("#DivFrame" + AxActiveDc).dialog({
            title: popUpTitle,
            height: 400,
            width: 550,
            close: function () {
                ClosePopUp(AxActiveDc);
            },
            position: 'top',
            modal: true,
            buttons: {
                "Ok": function () {
                    ClosePopUp(AxActiveDc, $j(this));
                }
            }
        });
    }
}

function ApplyPopJson(dcNo) {
    var dcClientRows = GetDcClientRows(dcNo);
    AxPopRowNo = dcClientRows.getMaxVal();
    AssignLoadValues(subGridJson, "PopGridCombos");
    UpdateRowInDataObj();
    try {
        AxAfterSubGridAddRow();
    } catch (ex) { }
}

//Function returns true if this row is assigned to a dc in the pop rows arrays.
function IsRowInPopRows(parDcNo, popDcNo, parRow, popRowNo) {
    var popRows = GetPopRows(parDcNo, parRow, popDcNo);
    var popRowStr = popRows.split(",");
    for (var i = 0; i < popRowStr.length; i++) {
        if (popRowStr[i] == popRowNo) {
            return true;
        }
    }

    return false;
}

//Function to check if the parent fields have empty value and return true.
//This function will not take care of integer fields.
function IsPopParBound(popDcNo, parDcNo, parRowNo) {
    var parFldsStr = GetParentFields(popDcNo);
    var isParBound = true;
    for (var i = 0; i < parFldsStr.length; i++) {
        var parFld = parFldsStr[i] + parRowNo + "F" + parDcNo;
        var parValue = GetFieldValue(parFld);
        //TODO: the try catch block needs to be removed.
        try {
            parValue = parseFloat(parValue);
        } catch (ex) { }
        if (parValue.toString() == "") {
            isParBound = false;
            break;
        }
    }
    return isParBound;
}

//function which sets the parent field values from the given parent row to the given rows pop fields.
function UpdatePopParentFlds(popDcNo, parDcNo, popRowNo, parRowNo) {
    var parFldsStr = GetParentFields(popDcNo);
    for (var i = 0; i < parFldsStr.length; i++) {
        var parFld = parFldsStr[i].toString().trim() + parRowNo + "F" + parDcNo;
        var popGridField = GetSubFieldId("sub" + popDcNo + "_" + parFldsStr[i].toString(), popRowNo, popDcNo);
        var parValue = GetFieldValue(parFld);
        SetFieldValue(popGridField, parValue);
        var clientRowNo = GetDbRowNo(popRowNo, popDcNo);
        UpdateFieldArray(popGridField, clientRowNo, parValue, "parent");
    }
}

//This function should be called on close popup to refresh all expression dependent fields outside the pop up.
function EvaluateSubGridDep(popDcNo) {
    var fields = GetGridFields(popDcNo);
    for (var i = 0; i < fields.length; i++) {

        var fldInd = GetFieldIndex(fields[i]);
        if (fldInd != -1) {
            depStr = FldDependents[fldInd].toString();
        }
        var depArray;
        if (depStr != "") {
            depArray = depStr.split(",");

            if (depArray != undefined) {
                for (var di = 0; di < depArray.length; di++) {

                    var dField = depArray[di].toString();
                    var depFirstChar = dField.substring(0, 1);
                    var depfName = dField.substring(1);
                    var depFldDc = GetDcNo(depfName);

                    if (popDcNo != depFldDc) {
                        if (depFirstChar == 'e') {
                            EvaluateAxFunction(depfName, fields[i] + AxActiveRowNo + "F" + popDcNo);
                        }
                    }
                }
            }
        }
    }
}

//Function to hide the opened pop up.
function ClosePopUp(popDcNo, dialogObj) {

    var popRows = GetPopRows(AxActivePDc, GetClientRowNo(AxActivePRow, AxActivePDc), popDcNo).split(",");
    if (popRows.length != 1) {
        for (var i = popRows.length - 1; i >= 0; i--) {
            var rCnt = GetDcRowCount(popDcNo);
            if (popRows[i].toString() != "") {
                if (IsEmptyRow(popRows[i], popDcNo) && rCnt != 1) {
                    DeletePopRow(popDcNo, popRows[i]);
                }
            }
        }
    }
    if ($j("#dvPickList").is(':visible')) $j("#dvPickList").hide();
    AxIsPopActive = false;
    if (dialogObj != undefined)
        dialogObj.dialog("close");
    var parentFld = $j("#" + AxActiveField);
    if (parentFld.length > 0)
        parentFld.focus();
    subGridJson = "";
    EvaluateSubGridDep(popDcNo);
    DisplaySummaryInPopUp(popDcNo);
}

//Function to delete the default pop row in the popgrid.
function DeletePopRow(popDcNo, popRow) {

    var slNo = GetSerialNoCnt(popDcNo);
    slNo = parseInt(slNo, 10) - 1;
    SetSerialNoCnt(popDcNo, slNo);
    var rowCnt = GetDcRowCount(popDcNo);

    var rowFrmNo = popRow + "F" + popDcNo;
    UpdateDcRowArrays(popDcNo, popRow, "Delete");
    var flds = GetGridFields(popDcNo);
    for (var j = 0; j < flds.length; j++) {
        var fld = flds[j] + rowFrmNo;
        RemoveDeletedFields(fld);
    }

    var rowId = "sp" + popDcNo + "R" + popRow + "F" + popDcNo;
    $j("#" + rowId).remove();
    UpdatePopUpArrays(popDcNo, popRow, true, "Delete");
    var rCnt = GetDcRowCount(popDcNo);
    SetRowCount(popDcNo, rCnt - 1);
}

//Function to update the changed value of the parent into the arrays.
function UpdatePopUpParents(fieldName) {
    var fName = GetFieldsName(fieldName);
    var fldDcNo = GetFieldsDcNo(fieldName);
    var fldRowNo = GetFieldsRowNo(fieldName);
    var parentRowNo = fldRowNo;
    for (var i = 0; i < PopParentDCs.length; i++) {
        if (PopParentDCs[i] == fldDcNo) {
            popParentFlds = PopParentFlds[i].toString().split(",");
            var parentStr = "";
            parentStr = GetParentString(fldDcNo, parentRowNo, PopGridDCs[i]);

            var isParentForPop = false;
            var subParFlds = GetParentFields(PopGridDCs[i]);
            for (var parIdx = 0; parIdx < subParFlds.length; parIdx++) {
                if (subParFlds[parIdx] == fName) {
                    isParentForPop = true;
                    break;
                }
            }
            if (!isParentForPop)
                continue;

            //Check for duplicate parents in the parent dc.
            if (CheckDuplicateParents(fldDcNo, popParentFlds, parentStr, PopGridDCs[i], parentRowNo) && parentStr != "") {
                errorFlag = true;
                errorField = fieldName;
                return;
            }

            // replace the old value in the poprows array with parentstr for each of its pop grid.
            SetPopParents(fldDcNo, PopGridDCs[i], parentRowNo, parentStr);

            var popDcNo = PopGridDCs[i];
            var strGridRows = GetPopRows(fldDcNo, parentRowNo, popDcNo);
            var subGrdRows = strGridRows.split(",");

            var newParValue = GetFieldValue(fieldName);

            for (var j = 0; j < subGrdRows.length; j++) {
                if (subGrdRows[j].toString() != "") {
                    var parFldId = GetSubFieldId("sub" + popDcNo + "_" + fName, subGrdRows[j].toString(), popDcNo);
                    parFldId = "#" + parFldId;
                    try {
                        var popFldId = parFldId.substring(1);
                        SetFieldValue(popFldId, newParValue);

                        var dbRowNo = GetDbRowNo(subGrdRows[j].toString(), popDcNo)
                        UpdateFieldArray(popFldId, dbRowNo, newParValue, "parent");
                    } catch (ex) { }
                }
            }
        }
    }
}

//Function to check for duplicate entries in the parent grid for the parent fields.
function CheckDuplicateParents(dcNo, ParentFlds, parentStr, popGridDc, parentRowNo) {

    var isDuplicate = false;
    var rowCount = 0;
    rowCount = parseInt(GetDcRowCount(dcNo), 10);

    for (var i = 1; i < rowCount; i++) {
        var rowNo = GetRowNoHelper(i);
        var rowParentStr = "";
        rowParentStr = GetParentString(dcNo, rowNo, popGridDc);
        if (rowParentStr == parentStr && i != parseInt(parentRowNo, 10))
            isDuplicate = true;
    }
    return isDuplicate;
}

//Function to set the concatenated parent value to the array.
function SetPopParents(parentDcNo, popDcNo, parentRowNo, parentStr) {
    for (var i = 0; i < ParentDcNo.length; i++) {
        if (ParentDcNo[i] == parentDcNo && PopGridDcNo[i] == popDcNo && ParentClientRow[i] == parentRowNo) {
            PopParentsStr[i] = parentStr;
        }
    }
}

//Function to display the summary defined for the pop grid.
function DisplaySummaryInPopUp(popDcNo) {

    var pIndx = -1;
    var i = 0;
    for (i = 0; i < PopGridDCs.length; i++) {
        if (PopGridDCs[i] == popDcNo) {
            pIndx = i;
            break;
        }
    }
    var parentFld = "";
    var summaryColumn = "";
    var delimiter = "";
    var summaryValue = "";
    if (pIndx != -1) {

        parentFld = PopSummaryParent[pIndx];
        summaryColumn = PopSummaryFld[pIndx];
        delimiter = PopSummDelimiter[pIndx] == "" ? "," : PopSummDelimiter[pIndx];
    }

    if (summaryColumn.indexOf(":") != -1) {
        summaryColumn = summaryColumn.replace(":", "");
    }
    var popRowStr = GetPopRows(AxActivePDc, GetClientRowNo(AxActivePRow, AxActivePDc), popDcNo);
    var popRows = popRowStr.split(",");

    for (var j = 0; j < popRows.length; j++) {
        var SummaryFld = summaryColumn + popRows[j] + "F" + popDcNo;
        SummaryFld = $j("#" + SummaryFld);
        if (SummaryFld.length > 0)
            summaryValue += SummaryFld.val() + delimiter;
    }
    summaryValue = summaryValue.substring(0, summaryValue.length - 1);
    parentFld = parentFld + GetClientRowNo(AxActivePRow, AxActivePDc) + "F" + AxActivePDc;
    if ($j("#" + parentFld).length > 0)
        $j("#" + parentFld).val(summaryValue);
}


//General Pop grid functions
//-----------------------------------------------------------------------
//Function to get the sub grid rows for the current active parent row.
function GetPopRows(parentDcNo, parentRowNo, popDcNo) {
    var popRows = "";
    for (var i = 0; i < ParentDcNo.length; i++) {
        if (ParentDcNo[i] == parentDcNo && ParentClientRow[i] == GetRowNoHelper(parentRowNo) && PopGridDcNo[i] == popDcNo) {
            popRows = PopRows[i];
            break;
        }
    }
    return popRows;
}

//Function to clear the poprows for the given parent row and dc no.
function ClearPopRows(parentDc, parentRow, popDc) {
    for (var i = 0; i < ParentDcNo.length; i++) {
        if (ParentDcNo[i] == parentDc && ParentClientRow[i] == parentRow && PopGridDcNo[i] == popDc) {
            PopRows[i] = "";
        }
    }
}

//Function to add a new row for the given pop up dc no.
function AddDeletePopRow(parentDcNo, parentRowNo, popDcNo, popRowNo, action) {

    var IsParentFound = false;
    var IsPDcFound = false;
    var IsFound = false;

    for (var i = 0; i < ParentDcNo.length; i++) {

        if (ParentDcNo[i] == parentDcNo) {

            if (ParentClientRow[i] == parentRowNo) {

                if (PopGridDcNo[i] == popDcNo) {
                    IsFound = true;
                    var tmpPopRows = PopRows[i].toString();
                    if (action == "Add") {
                        if (tmpPopRows == "")
                            tmpPopRows = popRowNo;
                        else {
                            var tmpRows = tmpPopRows.split(",");
                            var indx = $j.inArray(popRowNo, tmpRows)
                            if (indx == -1) {
                                tmpPopRows += "," + popRowNo;
                            }
                        }
                        PopRows[i] = tmpPopRows;
                    } else {
                        tmpPopRows = tmpPopRows.split(",");
                        for (var j = tmpPopRows.length - 1; j >= 0; j--) {
                            if (tmpPopRows[j].toString() == popRowNo) {
                                tmpPopRows.splice(j, 1);
                                break;
                            }
                        }
                        var popRowStr = "";
                        for (var k = 0; k < tmpPopRows.length; k++) {
                            if (popRowStr == "")
                                popRowStr += tmpPopRows[k].toString();
                            else
                                popRowStr += "," + tmpPopRows[k].toString();
                        }
                        PopRows[i] = popRowStr;
                    }
                    break;
                }
            }
        }
    }

    if (action == "Add") {
        if (!IsFound) {
            ParentDcNo.push(parentDcNo);
            ParentClientRow.push(parentRowNo);
            var parStr = GetParentString(parentDcNo, parentRowNo, popDcNo);
            PopGridDcNo.push(popDcNo);
            PopParentsStr.push(parStr);
            PopRows.push(popRowNo);
        }

        //set the parent field values into the components from the parent row.
        //PopParentFlds

        for (var ind = 0; ind < PopParentDCs.length; ind++) {
            if (PopParentDCs[ind] == parentDcNo) {
                parentFlds = PopParentFlds[ind].toString().split(",");
                for (var i = 0; i < parentFlds.length; i++) {
                    var pFldId = parentFlds[i] + parentRowNo + "F" + parentDcNo;
                    var popFldId = GetSubFieldId("sub" + popDcNo + "_" + parentFlds[i], popRowNo, popDcNo);
                    var parFld = $j("#" + pFldId);
                    var popFld = $j("#" + popFldId);

                    if (parFld.length > 0 && popFld.length > 0) {
                        popFld.val(parFld.val());
                    }
                }
            }
        }
    }
}

//Function which returns true if the given row is empty.
function IsEmptyRow(rowNo, dcNo) {
    var range;
    var IsEmpty = false;
    for (var i = 0; i < FldDcRange.length; i++) {
        var dcRange = FldDcRange[i].split("~");
        if (dcRange[0] == dcNo) {
            range = dcRange[1].split(",");
            break;
        }
    }
    if (range != undefined) {
        var startIndex = parseInt(range[0].toString(), 10);
        var endIndex = parseInt(range[1].toString(), 10);

        for (var j = startIndex; j <= endIndex; j++) {
            var fieldName = FNames[j];
            if (FMoe[j] == "Accept" || FMoe[j] == "Select") {
                fieldName = fieldName + rowNo + "F" + dcNo;
                var fldValue = GetFieldValue(fieldName);
                var field = $j("#" + fieldName);
                if (field.length > 0) {
                    if (field.attr("type") != "hidden") {
                        if (FDataType[j].toLowerCase() == "numeric" && fldValue != "")
                            fldValue = parseInt(fldValue, 10);
                        if (fldValue == "" || fldValue == "0") {
                            IsEmpty = true;
                        } else {
                            IsEmpty = false;
                            break;
                        }
                    }
                }
            }
        }
    }
    return IsEmpty;
}

//Function to set style for the pop grid dc if firm bind.
function SetPopDcStyle(PopDcNo, isFirm) {
    var addBtn = document.getElementById("add" + PopDcNo);
    var clearBtn = document.getElementById("btnClear" + PopDcNo);
    var btnOk = document.getElementById("btnOk" + PopDcNo);

    var enableAdd = false;

    if (isFirm) {
        if (addBtn && !enableAdd) {
            addBtn.disabled = true;
            addBtn.style.cursor = 'default';
        }
        if (clearBtn) {
            clearBtn.style.visibility = 'hidden';
        }

        if (btnOk) {
            btnOk.value = "Close";
        }
        ShowingDc(PopDcNo, "Disable");

        if (enableAdd) {
            $j("#DivFrame" + PopDcNo).find('.rowadd').removeClass("disableadd");
            $j("#DivFrame" + PopDcNo).find('.rowadd').removeAttr('disabled');
        }
    } else {
        if (addBtn) {
            addBtn.disabled = false;
            addBtn.style.cursor = 'hand';
        }
        if (clearBtn) {
            clearBtn.style.visibility = 'visible';
        }
        if (btnOk) {
            btnOk.value = "Ok";
        }
    }
}

//Function to set the Parents active row to the global variable.
function RegisterActivePRow(clientRowNo, pDcNo) {
    AxActivePRow = GetDbRowNo(clientRowNo, pDcNo);
    AxActivePDc = pDcNo;
    //update the parameters array.
    var found = false;
    for (var i = Parameters.length - 1; i >= 0; i--) {
        var parameter = Parameters[i].split("~");
        if (parameter[0] == "activeprow") {
            Parameters[i] = "activeprow" + "~" + AxActivePRow;
            found = true;
            break;
        }
    }
    if (!found)
        Parameters[Parameters.length] = "activeprow" + "~" + AxActivePRow;
}

//Function returns true if the PopDc has firm bind attribute as true.
function IsPopGridFirmBind(popDcNo) {
    for (var pIndx = 0; pIndx < PopGridDCs.length; pIndx++) {
        if (PopGridDCs[pIndx] == popDcNo) {
            if (PopGridDCFirm[pIndx].toString().toLowerCase() == "t") {
                return true;
            }
        }
    }
    return false;
}

//Function to get the parent fields for the given pop dc.
function GetParentFields(popDcNo) {
    var parentFlds;
    for (var i = 0; i < PopGridDCs.length; i++) {
        if (PopGridDCs[i] == popDcNo) {
            parentFlds = PopParentFlds[i].toString().split(",");
            break;
        }
    }
    return parentFlds;
}

//Function to get the parent values concatenated string for the given parent row and pop dc.
function GetParentString(parentDcNo, parentRow, popDc) {

    var parentStr = "";
    var parentFlds = GetParentFields(popDc);
    if (parentFlds != undefined) {
        for (var j = 0; j < parentFlds.length; j++) {
            var fldName = parentFlds[j] + parentRow + "F" + parentDcNo;
            if (parentStr == "")
                parentStr = GetFieldValue(fldName);
            else
                parentStr += "¿" + GetFieldValue(fldName);
        }
    }
    return parentStr;
}

//Function which returs the pop grids for the current parent dc.
function GetPopGrids(parentDcNo) {
    var popGrids = "";
    for (var i = 0; i < PopParentDCs.length; i++) {
        if (PopParentDCs[i] == parentDcNo) {
            if (popGrids == "")
                popGrids = PopGridDCs[i];
            else
                popGrids += "," + PopGridDCs[i];
        }
    }
    return popGrids;
}

function GetPopGridDcNo(parentDcNo) {
    var popGrids = "";
    for (var i = 0; i < PopParentDCs.length; i++) {
        if (PopParentDCs[i] == parentDcNo) {
            if (popGrids == "")
                popGrids = "dc" + PopGridDCs[i];
            else
                popGrids += "," + "dc" + PopGridDCs[i];
        }
    }
    return popGrids;
}

//Function which adds the parent row info to the pop arrays.
function AddParentRow(parentDcNo, parentRowNo, popDcNo, parentStr) {
    var isFound = false;
    for (var i = 0; i <= parentDcNo.length; i++) {
        if (ParentDcNo[i] == parentDcNo && PopGridDcNo[i] == popDcNo && ParentClientRow[i] == parentRowNo) {
            PopParentsStr[i] = parentStr;
            isFound = true;
            break;
        }
    }
    if (!isFound) {
        ParentDcNo.push(parentDcNo);
        ParentClientRow.push(parentRowNo);
        PopGridDcNo.push(popDcNo);
        PopParentsStr.push(parentStr);
        PopRows.push("");
    }
}

//Function which updates the popRows and rowno arrays for the newly added row.
function UpdatePopUpArrays(dcNo, rowNo, isPopDc, action, pRow, pDc) {
    if (TstructHasPop) {

        if (isPopDc) {
            var parentClientRow = GetClientRowNo(AxActivePRow, AxActivePDc);
            var activePDc = AxActivePDc;
            var activePRow = parentClientRow;

            if (pRow != undefined && pDc != undefined) {
                activePDc = pDc;
                activePRow = pRow;
            }

            if (action == "Add") {
                AddDeletePopRow(activePDc, activePRow, AxActiveDc, rowNo, "Add");
            } else {
                AddDeletePopRow(activePDc, activePRow, AxActiveDc, rowNo, "Delete");
            }
        } else {
            var popGridsStr = GetPopGrids(dcNo);
            var popGrids = popGridsStr.split(",");

            for (var i = 0; i < popGrids.length; i++) {
                if (popGrids[i] != "") {
                    AddParentRow(dcNo, rowNo, popGrids[i], "");
                }
            }
        }
    }
}

function DeletePopRowsAfterGetDep() {
    //format of AxSubGridRows -popdc1~row1,row2,row3¿popdc2~row1,row2
    var strData = AxSubGridRows.split("¿");
    //AxParStrFromDep -dcno~rowno
    var parData = AxParStrFromDep.split("~");
    for (var i = 0; i < strData.length; i++) {
        if (strData[i] != "") {
            var strDcRows = strData[i].split("~");
            var dcNo = strDcRows[0];

            //for every popgrid for the current parent grid, clear the poprows for the given parent row from the pop arrays
            ClearPopRows(parData[0], GetClientRowNo(parData[1], parData[0]), dcNo);

            var dcRows = strDcRows[1].split(",");
            for (var j = dcRows.length - 1; j >= 0; j--) {
                var rowNo = GetClientRowNo(parseInt(dcRows[j], 10), dcNo);
                if ($j("#gridDc" + dcNo)[0].rows.length == 1) {
                    var rowId = $j("#gridDc" + dcNo)[0].rows[0].id;
                    var rowNo = rowId.substring(rowId.lastIndexOf("R") + 1, rowId.lastIndexOf("F"));
                    ClearDeletedFields(dcNo, rowNo);
                    var dcIdx = $j.inArray(dcNo, DCFrameNo);
                    DCHasDataRows[dcIdx] = "False";
                    //convert the rowid of the first row to 001
                    var a = "sp" + dcNo + "R" + dcRows[j] + "F" + dcNo;
                    var newRow = $j("#" + a);

                    newRow.attr("id", "sp" + dcNo + "R001F" + dcNo);
                    newRow.find(':input').val('');
                    newRow.find('select,a,input,label,img,.Grdlnk').each(function () {
                        ConvertFieldID($j(this), "001");
                    });
                    SetRowCount(dcNo, "1");
                    UpdatePopUpArrays(dcNo, rowNo, true, "Delete");
                    UpdateDcRowArrays(dcNo, rowNo, "Delete", -1);
                } else {

                    //get the row, and remove the html
                    var a = "sp" + dcNo + "R" + rowNo + "F" + dcNo;
                    $j("#" + a).remove();

                    UpdatePopUpArrays(dcNo, rowNo, true, "Delete");
                    UpdateDcRowArrays(dcNo, rowNo, "Delete", -1);
                    SetRowCount(dcNo, GetDcRowCount(dcNo) - 1);
                }
            }
        }
    }
}

function AddRowAfterGetDep(rowNo, dcNo, oldHasData) {
    if (TstructHasPop) {

        //GetDep will always return the sub grid rows starting from "1" for the active parent row.
        //For e.g the cr attribute will always have "i1,i2...." Hence just adding at the end.
        //if (strDelRows[i].toString() != "i1" || (dcIsPop && calledFrom == "GetDep" && GetDcRowCount(dcNo) > 1)) {
        if (rowNo != "1" || oldHasData == "True") {
            AddRow(dcNo, "GetDep");
            AxDepRows.push(rowNo + "~" + GetDcRowCount(dcNo));
        } else {
            //if dc is pop grid or parent grid update the pop rows array
            AxDepRows.push(rowNo + "~" + "1");

            $j("#sp" + dcNo + "R" + "001" + "F" + dcNo).show();
            UpdateNewPopInfo(dcNo, "001");
            if (AxParStrFromDep != "") {
                var parStr = AxParStrFromDep.split("~");
                var parDc = parStr[0];
                var pRow = GetClientRowNo(parStr[1], parDc);
                UpdatePopUpArrays(dcNo, "001", true, "Add", pRow, parDc);
            }
            UpdateDcRowArrays(dcNo, "001", "Add");
        }
    }
}

//Function which returns the dbrowno for the poprows of a given parent row
function GetDbPopRows(strRows, dcNo) {
    if (strRows != "") {
        var dbPopRows = strRows.split(",");
        dbPopRows.sort();
        var tmpRows = "";
        for (var i = 0; i < dbPopRows.length; i++) {
            var rNo = GetRowNoHelper(GetDbRowNo(dbPopRows[i], dcNo));

            if (tmpRows == "")
                tmpRows = rNo;
            else
                tmpRows += "," + rNo;
        }
        return tmpRows;
    } else
        return strRows;
}

//Function to return the calcualted/last rowno to be used for adding to the table.
function GetSubDsRowNo(wsRowNo, dcNo) {
    var dsRowNo = 0;
    var idx = $j.inArray(wsRowNo, AxSubWsRows);
    if (idx == -1) {
        AxSubWsRows.push(wsRowNo);
        dsRowNo = GetDcRowCount(dcNo) + 1;
        AxSubDsRows.push(dsRowNo);
    } else {
        dsRowNo = AxSubDsRows[idx];
    }
    return dsRowNo;
}
var result;

function toggleSwitch(type) {
    var usrFlag = '';
    if ($j("#ckbPurposeUser").prop("checked") == true) {
        //checkbox is unchecked
        $j("#ckbPurposeUser").prop('checked', false);
        usrFlag = '0';
        $j(".clsPrps").css("display", "none");

    } else {
        $j("#ckbPurposeUser").prop('checked', true);
        //checkbox is checked
        usrFlag = '1';
        $j(".clsPrps").css("display", "block");
    }

    try {
        result = ASB.WebService.GetChoices('normal', usrFlag);
    } catch (ex) {
        result = ex.toString();
    }
}


function toggleSwitchDesign(type) {
    var valFlag = '';
    if ($("#ckbPurposeDev").prop("checked") == true) {
        //checkbox is unchecked
        $("#ckbPurposeDev").prop('checked', false);
        valFlag = '0';
        $j(".clsPrps").css("display", "none");

    } else {
        //checkbox is checked
        $("#ckbPurposeDev").prop('checked', true);
        valFlag = '1';
        $j(".clsPrps").css("display", "block");
    }
    try {
        result = ASB.WebService.GetChoices('design', valFlag);
    } catch (ex) {
        result = ex.toString();
    }

}

// AJAX Calls on tstruct design load
function GetChoiceStatusForDSign() {
    //alert(1);
    $.ajax({
        type: "POST",
        url: "TstructDesign.aspx/GetPrpLblStatusForDSign",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //alert(response.d);
            var res = response.d;
            if (res == "true") {
                $j(".clsPrps").css("display", "block");
                $j("#ckbPurposeDev").prop("checked", true);


            } else {
                $j(".clsPrps").css("display", "none");
                $j("#ckbPurposeDev").prop("checked", false);
            }
        }
    });


}

function pageloadlogtime(logTime) {
    console.log("Page Load:  " + logTime + " milliseconds.");
}

function disableDC(dcno) {
    const DCNumber = $j("#DivFrame" + dcno)
    DCNumber.find('input,textarea, img, select, a').not('.subGrid,.chkShwSel,.swtchDummyAnchr,.tgl').attr('disabled', true);
    DCNumber.find('a img').removeClass('handCur');
    DCNumber.find('img').attr('onclick', 'Callnull');
    DCNumber.find(":button").removeClass('handCur');
    DCNumber.find('.rowdelete').addClass("disabledelete");
    DCNumber.find('.rowadd').addClass("disableadd");
    DCNumber.find('.axpBtn').css({
        "pointer-events": "none",
        "cursor": "default"
    });
    DCNumber.find('.axpBtnCustom').css({
        "pointer-events": "none",
        "cursor": "default"
    });
    DCNumber.find('.upload-icon').attr('disabled', true).css("cursor", "default");
    DCNumber.find('.fillcls').addClass("disablefill");
    $j("#gridAddBtn" + dcno).prop('disabled', true);
    if (!axInlineGridEdit && AxpGridForm != "form")
        $j("#wrapperForEditFields" + dcno).hide();
    $j("#gridToggleBtn" + dcno).addClass('disabled').prop('disabled', true);
    //$j("[id ^= 'fillgrid']").prop('disabled', true).addClass('disabled');
    $j("#fillgrid" + dcno).prop('disabled', true).addClass('disabled');
    $j("#fillgridList" + dcno).prop('disabled', true).addClass('disabled');
    $j("#wrapperForEditFields" + dcno).find(".editLayoutFooter button").addClass('disabled').prop('disabled', true);
    $j("#colScroll" + dcno + " table tbody tr").addClass('disableTheRow');
    $j("#colScroll" + dcno + " table tbody tr").find('.glyphicon.glyphicon-pencil,.glyphicon.glyphicon-remove').addClass('disabled').prop("disabled", true).parent().addClass('disabled').prop("disabled", true);
    $("#gridAddBtn" + dcno).next().find(":button").attr("disabled", true).off('click');

}

function createEditors() {
    $("textarea").each(function () {
        let elem = $(this);
        let editorId = "";
        editorId = elem.attr("id") != undefined ? elem.attr("id") : "";
        if (editorId.toLowerCase().indexOf("sql_editor") === 0 || editorId.toLowerCase().indexOf("exp_editor") === 0 || GetDWBFieldType(GetFieldsName(editorId)) == "SQL Editor" || GetDWBFieldType(GetFieldsName(editorId)) == "Expression Editor" || editorId.toLowerCase().indexOf("js_editor") === 0 || editorId.toLowerCase().indexOf("html_editor") === 0) {

            if (IsGridField(GetFieldsName(editorId)) && AxpGridForm != "form" && !elem.parent('div').hasClass("edit-mode-content") && !elem.parents('.modal').length)
                return true;
            if (elem.hasClass('CodeMirrorApplied'))
                return true;
            elem.addClass('CodeMirrorApplied');

            if (elem.data("myeditor")) {
                //destroying the text editor
                elem.data("myeditor").toTextArea();
            }
            if (editorId.toLowerCase().indexOf("sql_editor") === 0 || GetDWBFieldType(GetFieldsName(editorId)) == "SQL Editor") {
                if (typeof callParentNew("mainSqlHintObj") != "undefined" && Object.keys(callParentNew("mainSqlHintObj")).length === 0)
                    getMainSqlHintObj();

                createTheEditor({
                    type: "sql",
                    textarea: editorId,
                    loadSqlHintObj: false,
                    sqlHintObj: callParentNew("mainSqlHintObj"),
                    customValidationFn: customValidationFn
                })
                var btnHTML = "<div id=\"dvpop" + editorId + "\" class=\"tstsqleditorbtn ms-auto\" style=\"float:right;margin-top:7px;\"><a value=\"SQL Editor\" data-id=\"" + editorId + "\" class=\"tstbtnSqlConsole\" href=\"javascript:void(0);\" tooltip=\"SQL Editor\"><i class=\"material-icons material-icons-style material-icons-2\" id=\"tstlinkSqlConsole\">open_in_new</span></a></div>";
                $("#" + editorId).parent().parent().find('.fld-wrap3').append(btnHTML);
            } else if (editorId.toLowerCase().indexOf("exp_editor") === 0 || GetDWBFieldType(GetFieldsName(editorId)) == "Expression Editor") {
                createTheEditor({
                    type: "expression",
                    textarea: editorId,
                    customValidationFn: customValidationFn
                })
                //createTheEditor({ type: "expression", textarea: editorId })
            } else if (editorId.toLowerCase().indexOf("html_editor") === 0) {
                try {
                    createHtmlEditor(editorId);
                } catch (ex) { }
            } else if (editorId.toLowerCase().indexOf("js_editor") === 0) {
                //  createTheEditor({ type: "text/javascript", textarea: editorId, customValidationFn: customValidationFn })
                createJSEditor(editorId, "text/javascript");
            } else {
                console.log("Not Applied " + editorId)
            }

        }
        createAxAutocomplete(editorId);
        createAxAutocompleteColon(editorId);
    })
}

//Open SQL Editor Modified in popup(devendra)
function createPopupdesignnew(txtid) {

    htmlContent = '<div id="axpertPopupWrapperDWB"  class="remodal" data-pushtxt-id="' + txtid + '" data-remodal-id="axpertPopupModalDWB"><button data-remodal-action="close" class="remodal-close remodalCloseBtn icon-basic-remove" title="Close"></button><div style="height:100%;" id="iframeMarkUp1"><div id="QryEditor"><div ><button id="exeQuery" title="Execute"><i class="fa fa-bolt"></i></button><textarea id="dwbtxtEditorsql" rows="4" cols="50"></textarea></div><div class="dvOutput"><div class="rsltHeader">Result<span id="spnRowCnt"></span></div><div id="txtOutput"></div><table id="tblOutput" class="table-responsive"></table></div></div></div></div>';
    $("head").append(htmlContent);
    var options = {
        "closeOnOutsideClick": true,
        "hashTracking": false,
        "closeOnEscape": true
    };
    var inst = $('[data-remodal-id=axpertPopupModalDWB]:not(.remodal-is-initialized):not(.remodal-is-closed):eq(0)').remodal(options);
    if (inst && inst.state != "opened")
        inst.open();

    let mainSqlCM = CodeMirror.fromTextArea(document.getElementById('dwbtxtEditorsql'), {
        mode: 'text/x-sql',
        smartIndent: true,
        lineNumbers: true,
        matchBrackets: true,
        autoCloseBrackets: true,
        theme: "default",
        hintOptions: {
            tables: callParentNew("mainSqlHintObj")
        },

    });

    mainSqlCM.on('keyup', function (editor, event) {
        if (
            !(event.ctrlKey) &&
            (event.keyCode >= 65 && event.keyCode <= 90) ||
            (event.keyCode >= 97 && event.keyCode <= 122) ||
            (event.keyCode >= 46 && event.keyCode <= 57)
        ) {
            // type code and show autocomplete hint in the meanwhile
            CodeMirror.commands.autocomplete(editor, null, {
                completeSingle: false
            });
        }
    });
    var parentdiv = '#dv' + $("#axpertPopupWrapperDWB").attr('data-pushtxt-id').split('0')[0];

    // var parentEditor = $(parentdiv + ' .CodeMirror')[0].CodeMirror;
    $('#QryEditor .CodeMirror')[0].CodeMirror.setValue($(parentdiv + ' .CodeMirror')[0].CodeMirror.getDoc().getValue());
    //return inst;

}
//---------End devendra

// Calling webservice
function customValidationFn(textarea) {
    if (tstDataId.startsWith("b_sql")) {
        axdevSturioCustomValidationFn(textarea);
        return;
    }
    var jsonData = "";
    var webServiceName = "";
    var textSqlandExpression = textarea.value;
    if (textSqlandExpression.length <= 0) {
        return;
    }
    if (textarea.id.toLocaleLowerCase().startsWith("sql_editor") || textarea.id.toLocaleLowerCase().startsWith("exp_editor") || GetDWBFieldType(GetFieldsName(textarea.id)) == "SQL Editor" || GetDWBFieldType(GetFieldsName(textarea.id)) == "Expression Editor") {
        var sourcefield = "",
            sourcetable = "",
            Axp_Web_SqlExp_Val_Def_RestRad = "";
        var calfrom = "",
            fgname = "";
        var fieldname = "",
            fldordno = "",
            datatype = "",
            fieldType = "",
            sfrom = "",
            expression = "",
            validateExpression = "",
            transid = "",
            savenorm = "",
            refresh = "",
            autoselect = "",
            combobox = "",
            sql = "",
            iviewparams = "",
            pmetadata = "",
            sourcefieldid = "",
            sourcetableid = "";
        var dcNo = "";

        fieldType = GetFieldValue(GetComponentName("type", GetDcNo("type"))) || "";
        fieldname = GetFieldValue(GetComponentName("name", GetDcNo("name"))) || "";
        fldordno = GetFieldValue(GetComponentName("fldordno", GetDcNo("fldordno"))) || "";
        datatype = GetFieldValue(GetComponentName("datatype", GetDcNo("datatype"))) || "";

        if (fieldType.toLowerCase() === "field") {
            sfrom = GetFieldValue(GetComponentName("modeofentry", GetDcNo("modeofentry"))) || "";
        } else if (fieldType.toLowerCase() === "Fill Grid") {
            sfrom = "fillgrid";
        }
        transid = GetFieldValue(GetComponentName("stransid", GetDcNo("stransid"))) || callParentNew("transid");
        expression = GetFieldValue(GetComponentName("exp_editor_expression", GetDcNo("exp_editor_expression"))) || "";
        validateExpression = GetFieldValue(GetComponentName("exp_editor_validateexpression", GetDcNo("exp_editor_validateexpression"))) || "";


        sfrom = sfrom.toLowerCase();

        if (sfrom === "select from sql") {
            savenorm = GetFieldValue(GetComponentName("savenormalized", GetDcNo("savenormalized"))) || "";
            refresh = GetFieldValue(GetComponentName("refreshonsav", GetDcNo("refreshonsav"))) || "";
            autoselect = GetFieldValue(GetComponentName("autoselect_sql", GetDcNo("autoselect_sql"))) || "";
            combobox = GetFieldValue(GetComponentName("combobox_sql", GetDcNo("combobox_sql"))) | "";
            sql = GetFieldValue(GetComponentName("sql_editor_test", GetDcNo("sql_editor_test"))) || "";
            sourcefield = GetFieldValue(GetComponentName("sourcefield", GetDcNo("sourcefield"))) || "";
            sourcetable = GetFieldValue(GetComponentName("sourcetable", GetDcNo("sourcetable"))) || "";
            pmetadata = GetFieldValue(GetComponentName("pmetadata", GetDcNo("pmetadata"))) || "";
            sourcefieldid = "sourcefield";
            sourcetableid = "sourcetable";
        } else if (sfrom === "select from form") {
            savenorm = GetFieldValue(GetComponentName("savenormalized_form", GetDcNo("savenormalized_form"))) || "";
            refresh = GetFieldValue(GetComponentName("refreshonsave_form", GetDcNo("refreshonsave_form"))) || "";
            autoselect = GetFieldValue(GetComponentName("autoselect_form", GetDcNo("autoselect_form"))) || "";
            combobox = GetFieldValue(GetComponentName("combobox_form", GetDcNo("combobox_form"))) || "";
            sql = GetFieldValue(GetComponentName("sql_editor_sqltextform", GetDcNo("sql_editor_sqltextform"))) || "";
            sourcefield = GetFieldValue(GetComponentName("selectfield", GetDcNo("selectfield"))) || "";
            sourcetable = GetFieldValue(GetComponentName("tname", GetDcNo("tname"))) || "";
            pmetadata = GetFieldValue(GetComponentName("pmetadata", GetDcNo("pmetadata"))) || "";

        } else if (sfrom === "accept") {
            sql = GetFieldValue(GetComponentName("sql_editor_detail", GetDcNo("sql_editor_detail"))) || "";

        } else if (sql == "") {
            sql = textSqlandExpression;
        }

        Axp_Web_SqlExp_Val_Def_RestRad = GetFieldValue(GetComponentName("Axp_Web_SqlExp_Val_Def_RestRad", GetDcNo("Axp_Web_SqlExp_Val_Def_RestRad"))) || "";

        if (textarea.id.toLocaleLowerCase().startsWith("sql_editor")) {
            jsonData = {
                "_parameters": [{
                    "tstructs": {
                        //"axpapp": eval(callParent('mainProject')),//
                        "axpapp": callParentNew('mainProject'), //Devendra
                        "sessionid": callParentNew('mainSessionId'),
                        "transid": transid, //
                        "fieldname": fieldname,
                        "iviewparams": iviewparams,
                        "fldordno": fldordno,
                        "datatype": datatype,
                        "sfrom": sfrom,
                        "expression": expression,
                        "validateexpression": validateExpression,
                        "savenorm": savenorm,
                        "refresh": refresh,
                        "autoselect": autoselect,
                        "combobox": combobox,
                        "sql": sql, //
                        "sourcefield": sourcefield,
                        "sourcetable": sourcetable,
                        "Axp_Web_SqlExp_Val_Def_RestRad": Axp_Web_SqlExp_Val_Def_RestRad,
                        "metadata": pmetadata,
                        "sourcefieldid": sourcefieldid,
                        "sourcetableid": sourcetableid,
                        "trace": "",
                        "scriptpath": ""
                    }
                }]
            }
            webServiceName = "ValidateSQL";
        } else if (textarea.id.toLocaleLowerCase().startsWith("exp_editor")) {
            //For expression called from
            textAreaID = textarea.id.toLocaleLowerCase();
            if (textAreaID.indexOf("exp_editor_expression") != -1)
                calfrom = "field"
            else if (textAreaID.indexOf("exp_editor_validateexpression") != -1)
                calfrom = "fgval"
            else if (textAreaID.indexOf("exp_editor_footerstring") != -1)
                calfrom = "fgfootstr"
            else
                calfrom = "field";
            let _adafFieldsList = '';
            if (typeof callParentNew("transid") != "undefined" && callParentNew("transid") == "ad_af" && $("#fldsdatatypes000F1").length > 0) {
                _adafFieldsList = $("#fldsdatatypes000F1").val();
            }

            jsonData = {
                "_parameters": [{
                    "tstructs": {
                        "axpapp": eval(callParent('mainProject')),
                        "sessionid": callParentNew('mainSessionId'),
                        "transid": transid,
                        "calfrom": calfrom,
                        "expression": textSqlandExpression,
                        "fgname": fieldname,
                        "Axp_Web_SqlExp_Val_Def_RestRad": Axp_Web_SqlExp_Val_Def_RestRad,
                        "trace": "",
                        "scriptpath": ""
                    }
                }]
            }
            if (_adafFieldsList != '')
                webServiceName = "ValidateExpression#" + _adafFieldsList;
            else
                webServiceName = "ValidateExpression";
        }
        ShowDimmer(true);

        try {
            ASB.WebService.CallRestWS(JSON.stringify(jsonData), webServiceName, SuccessCallbackAction, OnException);
        } catch (exp) { }


        function SuccessCallbackAction(result, eventArgs) {
            ShowDimmer(false);
            try {
                var json = $.parseJSON(result);
                var msg = json["result"][0].msg;
                var status = json["result"][0].status;
                if (typeof json["result"][0].metadata != "undefined") {
                    autocompleteMetadataJson(json["result"][0].metadata);
                } else {
                    sourceAcMetaJsonFlds = new Array();
                    fldSourceAcMetaJson = "";
                }
                if (status.toLowerCase() == "failed") {
                    msg = msg.replace('<error>', '').replace('</error>', '');
                    showAlertDialog("warning", msg);
                }
            } catch (ex) { }
        }

        function OnException(result) {
            ShowDimmer(false)
        }
    }
}

function IsTabDc(dcNo) {
    if ($j.inArray(dcNo, TabDCs) != -1)
        return true;
    else
        return false;
}


function createJSEditor(editorId, Jmode) {

    var opts = {
        smartIndent: true,
        lineNumbers: true,
        matchBrackets: true,
        autoCloseBrackets: true,
        autoRefresh: true,
        foldGutter: true,
        gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"]
    };
    opts.mode = Jmode;
    jsCodeMirror = CodeMirror.fromTextArea(document.getElementById(editorId), opts);
    jsCodeMirror.on("blur", function () {
        SetFieldValue(editorId, jsCodeMirror.getValue());
        UpdateFieldArray(editorId, "000", $("#" + editorId).val(), "parent");
    });
    jsCodeMirror.on('keyup', function (editor, event) {
        if (
            !(event.ctrlKey) &&
            (event.keyCode >= 65 && event.keyCode <= 90) ||
            (event.keyCode >= 97 && event.keyCode <= 122) ||
            (event.keyCode >= 46 && event.keyCode <= 57)
        ) {
            CodeMirror.commands.autocomplete(editor, null, {
                completeSingle: false
            });
        }
    });
}

function discardLodaData() {
    try {
        var dcldId = $j("#recordid000F0").val();
        //GetLoadData(dcldId, "");
        GetLoadDataForDiscard(dcldId, "");
    } catch (exp) { }
}

function hideDiscardButton() {
    try {
        var dcldId = $j("#recordid000F0").val();
        if (dcldId != "" && dcldId != "0" && AxpTstButtonStyle != "old")
            $("#ftbtn_iDiscard").removeClass("d-none");
        else {
            if (!$("#ftbtn_iDiscard").hasClass("d-none"))
                $("#ftbtn_iDiscard").addClass("d-none");
        }
    } catch (exp) { }
}
function showSaveNewbtns() {
    AdditionalRunTimeMsg("showSaveNewbtns called");
    if (typeof IsObjCustomHtml != "undefined" && IsObjCustomHtml != "False") {
        AdditionalRunTimeMsg("showSaveNewbtns called: IsObjCustomHtml:" + btnfooteropenlist);
        return;
    }
    if (typeof btnfooteropenlist != 'undefined' && btnfooteropenlist != "") {
        AdditionalRunTimeMsg("showSaveNewbtns called: btnfooteropenlist:" + btnfooteropenlist);
        $.each(btnfooteropenlist.split(','), function (i, val) {
            if (val != "") {
                let _btnId = val.split('~')[0];
                let _btnHint = val.split('~')[2];
                if (_btnId != "New" && _btnId != "Save") {
                    $("#" + _btnId).removeClass("d-none");
                    if (val.split('~').length > 1) {
                        let _btnDefId = val.split('~')[1];
                        $("#" + _btnId).attr('data-id', _btnDefId);
                        $("#" + _btnId).attr('data-extra', _btnHint);                        
                    }
                    if (AxPegAmdreadOnly == "true") {
                        //donothing
                    } else {
                        if (typeof $("#" + _btnId) != "undefined" && $("#" + _btnId).parents('.tstructMainBottomFooter').hasClass("pegreadonlytrans")) {
                            $("#" + _btnId).removeClass('disabled');
                            $("#" + _btnId).removeClass('d-none');
                            $("#" + _btnId).removeAttr("style").css({ "pointer-events": "auto" });
                            $("#" + _btnId).removeAttr('disabled');
                            $("#" + _btnId).parents('.tstructMainBottomFooter').removeClass('d-none');
                        }
                    }
                } else {
                    if (val.split('~').length > 1) {
                        let _btnDefId = val.split('~')[1];
                        $("#ftbtn_i" + _btnId).attr('data-id', _btnDefId);
                        $("#ftbtn_i" + _btnId).attr('data-extra', _btnHint);                 
                        if (AxPegAmdreadOnly == "true") {
                            //donothing
                        } else {
                            if (typeof $("#ftbtn_i" + _btnId) != "undefined" && $("#ftbtn_i" + _btnId).parents('.tstructMainBottomFooter').hasClass("pegreadonlytrans")) {
                                $("#ftbtn_i" + _btnId).removeClass('disabled');
                                $("#ftbtn_i" + _btnId).removeClass('d-none');
                                $("#ftbtn_i" + _btnId).removeAttr("style").css({ "pointer-events": "auto" });
                                $("#ftbtn_i" + _btnId).removeAttr('disabled');
                                $("#ftbtn_i" + _btnId).parents('.tstructMainBottomFooter').removeClass('d-none');
                            }
                        }
                    }
                }
            }
        });
    }
}

function autocompleteMetadataJson(mtJson) {
    if (mtJson != "") {
        try {
            sourceAcMetaJsonFlds = new Array();
            fldSourceAcMetaJson = mtJson;
            mtJson.forEach(function (varJson) {
                sourceAcMetaJsonFlds.push(varJson.name);
            });
        } catch (ex) { }
    }
}

function tstwfcomments() {
    if ($(".toolbarRightMenu").find(".content").length == 0) {
        var strBtn = "<section class=\"content\">";
        $("#breadcrumb-panel .bcrumb span:not(.ivirCButton):not(.icon-arrows-down).tstivtitle").css("padding", "3px 0px 0px 46px");
        strBtn += "<button type=\"button\" class=\"btn btn-default btn-circle waves-effect waves-circle waves-float wftstBackBtn\" onclick=\"wftstBackBtn();\" style=\"float: left;margin-top: -8px;margin-left: -15px;\">";
        strBtn += "<i class=\"material-icons\">arrow_back</i>";
        strBtn += "</button></section>";
        if ($(".toolbarRightMenu .newRequestJson").length > 0)
            $($(".toolbarRightMenu .newRequestJson")[0]).before(strBtn);
        else if ($(".toolbarRightMenu #iconsNewOption").length > 0)
            $(".toolbarRightMenu #iconsNewOption").before(strBtn);
        else
            $(".toolbarRightMenu").append(strBtn);
    }
}

function wftstBackBtn() {
    window.parent.globalChange = false;
    var prevTransId = $("#stransid000F1").val();
    var prevWfName = $("#wfname000F1").val();
    parent.LoadIframeac('WorkflowNew.aspx?prevTransId=' + prevTransId + '&prevWfName=' + prevWfName + '')
}

function DoScriptFormControl(componentName, eventType) {
    if (SFormControls.length > 0) {
        if (eventType == "On Form Load" || eventType == "On Data Load") {
            var rid = $j("#recordid000F0").val();
            $.each(SFCApply, function (idx, elem) {
                if (elem == "On Form Load" && rid == "0") {
                    isScriptFormLoad = "true";
                    var sfcExp = SFormControls[idx];
                    if (typeof SFCIsActive != "undefined" && SFCIsActive[idx] == "true")
                        EvaluateScriptFormControl(sfcExp);
                } else if (elem == "On Data Load" && rid != "0") {
                    isScriptFormLoad = "true";
                    var sfcExp = SFormControls[idx];
                    if (typeof SFCIsActive != "undefined" && SFCIsActive[idx] == "true")
                        EvaluateScriptFormControl(sfcExp);
                }
            });
        } else if (componentName != "") {
            var sfcfName = GetFieldsName(componentName);
            $.each(SFCFldNames, function (idx, elem) {
                if (SFCApply[idx] == "On Click" && eventType == "On Field Enter")
                    eventType = "On Click";
                if (elem == sfcfName && SFCApply[idx] == eventType) {
                    isScriptFormLoad = "false";
                    var sfcExp = SFormControls[idx];
                    if (typeof SFCIsActive != "undefined" && SFCIsActive[idx] == "true")
                        EvaluateScriptFormControl(sfcExp, componentName);
                }
            });
        }
    }
}

function EvaluateScriptFormControl(sfcExp, sfcfName = "") {
    if (sfcExp != "") {
        var flval = "";
        if (sfcfName != "")
            flval = GetFieldValue(sfcfName);
        //var strefVal= Evaluate(sfcfName, flval, sfcExp, "vexpr");
        var sfName = sfcfName == "" ? "" : sfcfName;
        var arrsfcExp = sfcExp.split("♥");
        AxFormControlList = new Array();
        var strefVal = EvalExprSet(arrsfcExp);
        if (AxFormControlList.length > 0)
            ProcessScriptFormControlOnList(sfName);
        else if (strefVal != "" && strefVal.split("~").length >= 2)
            ProcessScriptFormControl(strefVal.split("~")[1], strefVal.split("~")[0], sfName);
        else if (strefVal != "" && strefVal.split("♠").length > 0 && FormControlSameFormLoad == false)
            ProcessScriptFormControlEvents(strefVal, sfName);
    }
}

function ProcessScriptFormControlOnList(sfName) {
    $.each(AxFormControlList, function (i, val) {
        if (val != "")
            ProcessScriptFormControl(val.split("~")[1], val.split("~")[0], sfName);
    });
}

function ProcessScriptFormControl(listControls, actionStr, sfName) {
    var sfFldVal = "";
    var sfRno = 0;
    if (listControls == "")
        return;
    if (listControls.indexOf("♦") > 0) {
        sfRno = listControls.split("♦")[2];
        sfFldVal = listControls.split("♦")[1];
        listControls = listControls.split("♦")[0];
    }
    listControls.split(',').forEach(function (contName) {

        let setfldCap = "";
        if (contName.indexOf("^") > 0) {
            setfldCap = contName.split("^")[1];
            contName = contName.split("^")[0];
        }

        var fldname = GetExactFieldName(contName);
        contName = fldname;
        var conFldDcNo = GetDcNo(contName);
        if (conFldDcNo != "0") {
            var isGrid = IsDcGrid(conFldDcNo);
            if (isGrid)
                secFldDc = "GRID";
            else
                secFldDc = "NONGRID";


            if (secFldDc == "NONGRID")
                contName = contName + "000F" + conFldDcNo;
            else if (secFldDc == "GRID") {
                let contFldName = contName;
                if (AxActiveRowNo == "0" || AxActiveRowNo == "") {
                    var clientRowNo = "001";
                    contName = contName + clientRowNo + "F" + conFldDcNo;
                } else {
                    var clientRowNo = GetClientRowNo(AxActiveRowNo, conFldDcNo);
                    contName = contName + clientRowNo + "F" + conFldDcNo;
                }
                var destfld = $j("#" + contName + ":not(.tstformbutton,.axpBtnCustom)");
                if (destfld.length == 0) {
                    let grdFldRowId = $("#gridHd" + conFldDcNo + " tbody tr:first").length > 0 ? $("#gridHd" + conFldDcNo + " tbody tr:first").attr("id") : "";
                    if (grdFldRowId != "") {
                        grdFldRowId = grdFldRowId.substring(grdFldRowId.lastIndexOf("F") - 3);
                        contName = contFldName + grdFldRowId;
                    }
                }
            }
        }

        isFieldBtn = false;
        var destfld = $j("#" + contName + ":not(.tstformbutton,.axpBtnCustom,.dwbIvBtnbtm,.dwbBtn,.menu-link)");
        if (destfld.length == 0) {
            destfld = $j("#DivFrame" + contName.toString().substring(2));
            if (destfld.length == 0) {
                var dcName = contName.substr(2);
                if ($j.inArray(dcName, TabDCs) != -1) {
                    destfld = $j("#ank" + dcName);
                }
            }
        } else if (destfld.length > 0) {
            if (destfld.parents(".menu.menu-sub-dropdown").length > 0)
                isFieldBtn = true;
        }

        var fldRowNo = GetFieldsRowNo(contName);
        var fldDcNo = GetFieldsDcNo(contName);
        var fldDbRowNo = GetDbRowNo(fldRowNo, fldDcNo);

        var isFldSaveBtn = false;

        if (destfld.length <= 0) {
            //check if the field is button, by removing the rowno and dc number
            var newFldName = contName;
            if (contName != "" && contName.toString().indexOf("F") != -1) {
                newFldName = contName.substring(0, contName.lastIndexOf("F") - 3);
                if (newFldName == "" || FNames.indexOf(newFldName) == -1)
                    newFldName = contName;
            }

            $j(".axpBtn,.axpBtnCustom").each(function () {
                if ($j(this).attr("id") == newFldName) {
                    destfld = $j(this);
                    isFieldBtn = true;
                    return false;
                }
            });

            var actTmpBtn;
            $j(".action img").each(function () {
                if ($j(this).attr("id") == newFldName) {
                    actTmpBtn = $j(this);
                    isFieldBtn = true;
                    return false;
                }
            });

            //AxpTstBtn
            var actTmpBtn;
            var isBtnInDc = false;
            $j(".AxpTstBtn input,img").each(function () {
                if ($j(this).attr("value") == newFldName || $j(this).attr("id") == "AxpTstBtn_" + newFldName) {
                    actTmpBtn = $j(this);
                    isFieldBtn = true;
                    isBtnInDc = true;
                    return false;
                }
            });

            $j(".tstformbutton").each(function () {
                if ($j(this).attr("id") == newFldName) {
                    actTmpBtn = $j(this);
                    isFieldBtn = true;
                    isBtnInDc = true;
                    return false;
                }
            });


            if (newFldName.toLowerCase() == "remove")
                newFldName = "delete";
            else if (newFldName.toLowerCase() == "list view")
                newFldName = "listview";
            else if (newFldName.toLowerCase() == "new") {
                if ($j(".tstructMainBottomFooter").find('a[title^=new]').length > 0)
                    newFldName = "new";
                else
                    newFldName = "add";
                //btnfooteropenlist = btnfooteropenlist.replace("ftbtn_iNew", "");
            }

            if (newFldName.toLowerCase() == "save" || newFldName.toLowerCase() == "submit") {
                isFldSaveBtn = true;
                newFldName = "save";
                //btnfooteropenlist = btnfooteropenlist.replace("ftbtn_iSave", "");
            }

            var sfnewFldName = newFldName;
            if (sfnewFldName.toLowerCase() == "listview")
                sfnewFldName = "list view";
            $j("#icons").find("a").each(function () {
                if (typeof $j(this).attr("title") != "undefined" && $j(this).attr("class") == newFldName.toLowerCase() || (typeof btnName !== 'undefined' && $j(this).text() == btnName)) {
                    destfld = $j(this);
                    isFieldBtn = true;
                    return false;
                } else if (typeof $j(this).attr("title") != "undefined" && $j(this).attr("title").toLowerCase() == sfnewFldName.toLowerCase() || (typeof btnName !== 'undefined' && $j(this).text() == btnName) || typeof $j(this).attr("id") != "undefined" && $j(this).attr("id").toLowerCase() == sfnewFldName.toLowerCase()) {
                    destfld = $j(this);
                    isFieldBtn = true;
                    return false;
                } else if ((sfnewFldName == "new" && typeof $j(this).attr("title") !== "undefined" && $j(this).find('.tbCaption').text().trim().toLowerCase() === "new") || (sfnewFldName == "delete" && typeof $j(this).attr("title") !== "undefined" && $j(this).find('.tbCaption').text().trim().toLowerCase() === "remove")) {
                    destfld = $j(this);
                    isFieldBtn = true;
                    return false;
                }
            });

            $j(".toolbarRightMenu").find("a").each(function () {
                if (typeof $j(this).attr("title") != "undefined" && $j(this).attr("title").toLowerCase() == sfnewFldName.toLowerCase() || (typeof btnName !== 'undefined' && $j(this).text() == btnName)) {
                    destfld = $j(this);
                    isFieldBtn = true;
                    return false;
                }
            });

            //condition to check prev and next button in list view header
            if (!isFieldBtn) {
                $j("#nextprevicons").find("a").each(function () {
                    if ($j(this).attr("class") == newFldName.toLowerCase()) {
                        destfld = $j(this);
                        isFieldBtn = true;
                    }
                });
            }
            if (!isFieldBtn) {
                $j(".tstructMainBottomFooter").find("a").each(function () {
                    if ($j(this).attr("id").toLowerCase() == newFldName.toLowerCase() || (typeof $j(this).attr("title") != "undefined" && $j(this).attr("title").toLowerCase() == sfnewFldName.toLowerCase() || (typeof btnName !== 'undefined' && $j(this).text() == btnName))) {
                        destfld = $j(this);
                        isFieldBtn = true;
                    }
                });
            }

            if (!isFieldBtn) {
                $(".toolbarRightMenu .menu-item a").each(function () {
                    if (typeof $j(this).attr("id") != "undefined" && ($j(this).attr("id").toLowerCase() == newFldName.toLowerCase()) || (typeof $j(this).attr("title") != "undefined" && $j(this).attr("title").toLowerCase() == sfnewFldName.toLowerCase() || (typeof btnName !== 'undefined' && $j(this).text() == btnName))) {
                        destfld = $j(this);
                        isFieldBtn = true;
                    }
                });
            }

            if (actTmpBtn != undefined && actTmpBtn.length > 0)
                destfld = actTmpBtn.parent(0);
            if (isBtnInDc)
                destfld = actTmpBtn;

            if (destfld.length == 0 && (newFldName.toLowerCase() == "lnknext" || newFldName.toLowerCase() == "lnkprev" || newFldName.toLowerCase() == "ftbtn_idiscard" || newFldName.toLowerCase() == "discard")) {
                AxDiscardNxtPrevFc.push(newFldName + "~" + actionStr);
            }

            if (destfld.length > 0 && (newFldName.toLowerCase() == "ftbtn_idiscard" || newFldName.toLowerCase() == "discard")) {
                AxDiscardNxtPrevFc.push("discard~" + actionStr);
            }
            if (destfld.length == 0 && newFldName!="" && newFldName.toLowerCase() == "draft") {
                destfld = $("#btnAppsDraft");
                isFieldBtn = true;
            }
        } else {
            if (destfld.length > 0 && (destfld.attr("id").toLowerCase() == "lnknext" && contName.toLowerCase() == "lnknext") || (destfld.attr("id").toLowerCase() == "lnkprev" && contName.toLowerCase() == "lnkprev")) {
                AxDiscardNxtPrevFc.push(contName + "~" + actionStr);
            }

            if (destfld.length > 0 && (contName.toLowerCase() == "ftbtn_idiscard" || contName.toLowerCase() == "discard")) {
                AxDiscardNxtPrevFc.push("discard~" + actionStr);
            }
            if (destfld.length == 0 && contName != "" && contName.toLowerCase() == "draft") {
                destfld = $("#btnAppsDraft");
                isFieldBtn = true;
            }
        }

        if (destfld.length > 0) {
            switch (actionStr) {
                case ("enable"):
                    if (isFieldBtn) {
                        if (isFldSaveBtn) {
                            EnableSaveBtn(true);

                            EnableDisableBtns(destfld, true);
                        }
                        else
                            EnableDisableBtns(destfld, true);
                    } else {
                        /*if (fldname.substr(0, 2) == "dc") {*/
                        if (/^dc\d+$/.test(fldname.toLowerCase())) {
                            var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                            if (typeof isWizardTstruct != "undefined" && isWizardTstruct)
                                ToggleWizardDc(dcNo, "enable");
                            else
                                ShowingDc(dcNo, "enable");
                        } else {
                            var _thisdcNo = GetDcNo(fldname);
                            try {
                                let _fName = GetFieldsName(destfld.attr("id"));
                                let _fldIndex = $j.inArray(_fName, FNames);
                                FFieldReadOnly[_fldIndex] = "False";
                            } catch (ex) { }
                            if (IsDcGrid(_thisdcNo)) {
                                var rowCnt = 0;
                                rowCnt = GetDcRowCount(_thisdcNo);
                                var eleType = getGridFldType(fldname, _thisdcNo);
                                for (var i = 1; i <= rowCnt; i++) {
                                    destfld = $j("#" + fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo);
                                    let _thisEleId = fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo;
                                    if (destfld.length > 0) {
                                        var idx = $j.inArray(_thisdcNo + "~" + fldname + "~disable", AxFormContSetFldActGrid);
                                        if (idx == -1) {
                                            idx = $j.inArray(_thisdcNo + "~" + fldname + "~enable", AxFormContSetFldActGrid);
                                            if (idx == -1)
                                                AxFormContSetFldActGrid.push(_thisdcNo + "~" + fldname + "~enable");
                                            else
                                                AxFormContSetFldActGrid[idx] = _thisdcNo + "~" + fldname + "~enable";
                                        } else {
                                            AxFormContSetFldActGrid[idx] = _thisdcNo + "~" + fldname + "~enable";
                                        }

                                        if (IsPickListField(destfld.attr("id")) == true) {
                                            var pickFld = document.getElementById("img~" + destfld.attr("id"));
                                            pickFld.disabled = false;
                                            pickFld.className = "input-group-addon handCur pickimg";
                                        }
                                        if (destfld.val() == 0 && destfld.prop("type") != "select-one")
                                            destfld.val("");

                                        if (destfld.attr("title") == dateString && destfld.val() == "")
                                            destfld.val(dateString);
                                        if (destfld.attr("type") == "checkbox") {
                                            var checlistid = destfld.attr("id");

                                            EnableDisableCheckbox(checlistid, false)
                                        } else {

                                            // for enabling the Rich Text Box If it is disabled on dataload using form control
                                            if (destfld.attr("id").startsWith("rtf_") || destfld.attr("id").startsWith("rtfm_") || destfld.attr("id").startsWith("fr_rtf_") || GetDWBFieldType(GetFieldsName(destfld.attr("id"))) == "Rich Text") {
                                                $j("#cke_" + destfld.attr("id")).prop("disabled", false);
                                                destfld.css("display", "none");
                                                $j("#cke_" + destfld.attr("id")).removeAttr("disabled");
                                            }
                                            if (destfld.attr("class") == "axpImg") {
                                                destfld.attr("onclick", null);
                                            }
                                            destfld.prop("disabled", false);
                                            destfld.prop("readOnly", false);
                                            destfld.removeAttr("readOnly");
                                            SetAutoCompAccess("enabled", destfld);
                                        }
                                    }
                                }
                            } else {
                                if (IsPickListField(destfld.attr("id")) == true) {
                                    var pickFld = document.getElementById("img~" + destfld.attr("id"));
                                    pickFld.disabled = false;
                                    pickFld.className = "input-group-addon handCur pickimg";
                                }


                                if (destfld.val() == 0 && destfld.prop("type") != "select-one")
                                    destfld.val("");

                                if (destfld.attr("title") == dateString && destfld.val() == "")
                                    destfld.val(dateString);
                                if (destfld.attr("type") == "checkbox") {
                                    var checlistid = destfld.attr("id");

                                    EnableDisableCheckbox(checlistid, false)
                                } else {

                                    // for enabling the Rich Text Box If it is disabled on dataload using form control
                                    if (destfld.attr("id").startsWith("rtf_") || destfld.attr("id").startsWith("rtfm_") || destfld.attr("id").startsWith("fr_rtf_") || GetDWBFieldType(GetFieldsName(destfld.attr("id"))) == "Rich Text") {
                                        $j("#cke_" + destfld.attr("id")).prop("disabled", false);
                                        destfld.css("display", "none");
                                        $j("#cke_" + destfld.attr("id")).removeAttr("disabled");
                                    }
                                    if (destfld.attr("class") == "axpImg") {
                                        destfld.attr("onclick", null);
                                    }
                                    destfld.prop("disabled", false);
                                    destfld.prop("readOnly", false);
                                    destfld.removeAttr("readOnly");
                                    SetAutoCompAccess("enabled", destfld);
                                    try {
                                        if (destfld.length > 1 && $(destfld[0]).hasClass("profile-pic")) {
                                            $(destfld[0]).siblings("span.fldImageCamera").removeClass("d-none").removeClass("disabledFldFrmControl");
                                        }
                                    } catch (ex) { }
                                }
                            }
                        }
                    }
                    break;
                case ("disable"):
                    if (isFieldBtn) {
                        if (isFldSaveBtn) {
                            EnableSaveBtn(false);
                            EnableDisableBtns(destfld, false);
                        }
                        else
                            EnableDisableBtns(destfld, false);
                    } else {
                        /*if (fldname.substr(0, 2) == "dc") {*/
                        if (/^dc\d+$/.test(fldname.toLowerCase())) {
                            var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                            if (typeof isWizardTstruct != "undefined" && isWizardTstruct)
                                ToggleWizardDc(dcNo, "disable");
                            else
                                ShowingDc(dcNo, "disable");
                        } else {
                            var _thisdcNo = GetDcNo(fldname);
                            try {
                                let _fName = GetFieldsName(destfld.attr("id"));
                                let _fldIndex = $j.inArray(_fName, FNames);
                                FFieldReadOnly[_fldIndex] = "True";
                            } catch (ex) { }
                            if (IsDcGrid(_thisdcNo)) {
                                var rowCnt = 0;
                                rowCnt = GetDcRowCount(_thisdcNo);
                                var eleType = getGridFldType(fldname, _thisdcNo);
                                for (var i = 1; i <= rowCnt; i++) {
                                    destfld = $j("#" + fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo);
                                    let _thisEleId = fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo;
                                    if (destfld.length > 0) {
                                        var idx = $j.inArray(_thisdcNo + "~" + fldname + "~enable", AxFormContSetFldActGrid);
                                        if (idx == -1) {
                                            idx = $j.inArray(_thisdcNo + "~" + fldname + "~disable", AxFormContSetFldActGrid);
                                            if (idx == -1)
                                                AxFormContSetFldActGrid.push(_thisdcNo + "~" + fldname + "~disable");
                                            else
                                                AxFormContSetFldActGrid[idx] = _thisdcNo + "~" + fldname + "~disable";
                                        } else {
                                            AxFormContSetFldActGrid[idx] = _thisdcNo + "~" + fldname + "~disable";
                                        }
                                        if (IsPickListField(destfld.attr("id")) == true) {
                                            var pickFld = document.getElementById("img~" + destfld.attr("id"));
                                            pickFld.disabled = true;
                                            pickFld.className = "pickimg input-group-addon handCur";
                                        }
                                        if (destfld.attr("title") == dateString && (destfld.val() == dateString || destfld.val() == "''"))
                                            destfld.val("");

                                        if (destfld.attr("type") == "checkbox") {
                                            var checlistid = destfld.attr("id");
                                            EnableDisableCheckbox(checlistid, true)
                                        } else {

                                            // for disabling the Rich Text Box If it is disabled on dataload using form control
                                            if (destfld.attr("id").startsWith("rtf_") || destfld.attr("id").startsWith("rtfm_") || destfld.attr("id").startsWith("fr_rtf_") || GetDWBFieldType(GetFieldsName(destfld.attr("id"))) == "Rich Text") {
                                                $j("#cke_" + destfld.attr("id")).attr('disabled', 'disabled');
                                                destfld.css("display", "none");
                                                $j("#cke_" + destfld.attr("id")).prop("readonly", true);
                                            }
                                            if (destfld.attr("class") == "axpImg") {
                                                destfld.attr("onclick", "callnull");
                                            }
                                            //destfld.prop("disabled", true);
                                            SetAutoCompAccess("disabled", destfld);
                                        }
                                    }
                                }
                            } else {
                                if (IsPickListField(destfld.attr("id")) == true) {
                                    var pickFld = document.getElementById("img~" + destfld.attr("id"));
                                    pickFld.disabled = true;
                                    pickFld.className = "pickimg input-group-addon handCur";
                                }
                                if (destfld.attr("title") == dateString && (destfld.val() == dateString || destfld.val() == "''"))
                                    destfld.val("");

                                if (destfld.attr("type") == "checkbox") {
                                    var checlistid = destfld.attr("id");
                                    EnableDisableCheckbox(checlistid, true)
                                } else {

                                    // for disabling the Rich Text Box If it is disabled on dataload using form control
                                    if (destfld.attr("id").startsWith("rtf_") || destfld.attr("id").startsWith("rtfm_") || destfld.attr("id").startsWith("fr_rtf_") || GetDWBFieldType(GetFieldsName(destfld.attr("id"))) == "Rich Text") {
                                        $j("#cke_" + destfld.attr("id")).attr('disabled', 'disabled');
                                        destfld.css("display", "none");
                                        $j("#cke_" + destfld.attr("id")).prop("readonly", true);
                                    }
                                    if (destfld.attr("class") == "axpImg") {
                                        destfld.attr("onclick", "callnull");
                                    }
                                    destfld.prop("disabled", true);
                                    SetAutoCompAccess("disabled", destfld);
                                    try {
                                        if (destfld.length > 1 && $(destfld[0]).hasClass("profile-pic")) {
                                            $(destfld[0]).siblings("span.fldImageCamera").addClass("d-none").addClass("disabledFldFrmControl");
                                        }
                                    } catch (ex) { }
                                }
                            }
                        }
                    }
                    break;
                case ("hide"):
                    /*if (fldname.substr(0, 2) == "dc") {*/
                    if (/^dc\d+$/.test(fldname.toLowerCase())) {
                        var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                        if (typeof isWizardTstruct != "undefined" && isWizardTstruct)
                            ToggleWizardDc(dcNo, "hide");
                        else
                            ShowingDc(dcNo, "hide");
                    } else {
                        if (isFieldBtn) {
                            EnableDisableSFCBtns(destfld, "hide");
                            if (typeof destfld.attr("id") != "undefined" && destfld.attr("id") == "ftbtn_iSave")
                                btnfooteropenlist = btnfooteropenlist.replace("ftbtn_iSave", "Save");
                            if (typeof destfld.attr("id") != "undefined" && destfld.attr("id") == "ftbtn_iNew")
                                btnfooteropenlist = btnfooteropenlist.replace("ftbtn_iNew", "New");
                        } else {
                            fldsHideOnPage = "true";
                            HideShowField(fldname, "hide");
                            var fldInd = GetFieldIndex(fldname);
                            var fldDType = GetDWBFieldType("", fldInd);
                            if (fldInd > -1 && (fldname.startsWith("axptm_") || fldname.startsWith("axpdbtm_") || (fldDType != "" && fldDType.toLowerCase() == "time"))) {
                                $j("#" + destfld.attr("id") + " .tstOnlyTime").removeClass('hasDatepicker');
                                $j("#" + destfld.attr("id") + " .tstOnlyTime24hours").removeClass('hasDatepicker');
                            }
                        }
                    }
                    break;
                case ("show"):
                    /*if (fldname.substr(0, 2) == "dc") {*/
                    if (/^dc\d+$/.test(fldname.toLowerCase())) {
                        var dcNo = parseInt(fldname.substr(2, fldname.length), 10);
                        if (typeof isWizardTstruct != "undefined" && isWizardTstruct)
                            ToggleWizardDc(dcNo, "show");
                        else
                            ShowingDc(dcNo, "show");
                    } else {
                        if (isFieldBtn) {
                            EnableDisableSFCBtns(destfld, "show");
                        } else {
                            HideShowField(fldname, "show");
                            var fldInd = GetFieldIndex(fldname);
                            var fldDType = GetDWBFieldType("", fldInd);
                            if (fldInd > -1 && (fldname.startsWith("axptm_") || fldname.startsWith("axpdbtm_") || (fldDType != "" && fldDType.toLowerCase() == "time"))) {
                                $j("#" + destfld.attr("id") + " .tstOnlyTime").addClass('hasDatepicker');
                                $j("#" + destfld.attr("id") + " .tstOnlyTime24hours").addClass('hasDatepicker');
                            }
                        }
                    }
                    break;
                case ("setvalue"):
                    axpScriptIsAddrow = false;
                    var dcNo = GetDcNo(fldname);
                    var sffld = "";
                    if (IsDcGrid(dcNo)) {
                        let nrno = "001";
                        var rnolength = sfRno.length;
                        if (rnolength == 2) nrno = "0" + sfRno;
                        else if (rnolength == 1) nrno = "00" + sfRno;
                        else if (rnolength == 3) nrno = sfRno;
                        let clientRow = GetClientRowNo(sfRno, dcNo);
                        sffld = fldname + clientRow + "F" + dcNo;
                        AxActiveRowNo = GetDbRowNo(nrno, dcNo);
                    } else {
                        sffld = fldname + "000F" + dcNo;
                    }
                    if ($j("#" + sffld).hasClass("multiFldChk") && sfFldVal.trim() == "") {
                        if (IsDcGrid(dcNo)) {
                            $j("input[id=" + sffld + "]").each(function () {
                                $j(this).removeAttr("checked");
                                $j(this).prop("checked", false);
                            });
                        } else {
                            try {
                                $("#" + sffld).val("");
                                $("#" + sffld).data("valuelist", "");
                                $("#" + sffld).tokenfield('setTokens', []);
                            } catch (ex) { }
                        }
                    } else {
                        CallSetFieldValue(sffld, sfFldVal);
                    }
                    var fRowNo = GetFieldsRowNo(sffld);
                    var dbRowNo = GetDbRowNo(fRowNo, dcNo);
                    UpdateFieldArray(sffld, dbRowNo, sfFldVal, "parent", "");
                    if (sfName != "") {
                        if (sfName.startsWith("barqr_")) {
                            $j($j("#" + sfName)).val('');
                            try {
                                let _fRowNo = GetFieldsRowNo(sfName);
                                let _dcNo = GetFieldsDcNo(sfName);
                                let _fldDbRow = GetDbRowNo(_fRowNo, _dcNo);
                                UpdateFieldArray(sfName, _fldDbRow, "", "parent", "");
                                UpdateAllFieldValues(sfName, "");
                            } catch (ex) { }
                        }
                        try {
                            let isNotFillDep = false;
                            if (typeof AxpNotFillDepFields != "undefined" && AxpNotFillDepFields != "" && sfName!="") {
                                let sfFldName = GetFieldsName(sfName);
                                if (typeof sfFldName != "undefined" && sfFldName != "") {
                                    var AxpNotFillDepField = AxpNotFillDepFields.split(",");
                                    if (AxpNotFillDepField.indexOf(sfFldName) > -1) {
                                        isNotFillDep = true;
                                    }
                                }
                            }
                            if (isNotFillDep)
                                MainBlur($("#" + sffld));
                        } catch (ex) { }
                    }
                    break;
                case ("axaddrow"):
                    axpScriptIsAddrow = true;
                    var addDcNo = contName.substring(2);
                    var isExitDummy = false;
                    if (gridDummyRowVal.length > 0) {
                        gridDummyRowVal.map(function (v) {
                            if (v.split("~")[0] == addDcNo)
                                isExitDummy = true;
                        });
                    }
                    AxWaitCursor(true);
                    ShowDimmer(true);
                    CallAddRowFromScript(addDcNo, sfName, isExitDummy);
                    break;
                case ("setfieldcaption"):
                    var conFldDcNo = GetFieldsDcNo(contName);
                    if (conFldDcNo != "0") {
                        var isGrid = IsDcGrid(conFldDcNo);
                        if (isGrid) {
                            let thisFldName = GetFieldsName(contName);
                            destfld.parents(".customSetupTableMN").find(`thead th#th-${thisFldName} .thhead`).text(setfldCap);
                        } else {
                            destfld.parents(".grid-stack-item").find(".fld-wrap3 label").text(setfldCap);
                        }
                    }
                    break;
                case ("setdccaption"):
                    let _sdcName = $j.inArray(contName, DCName);
                    if (_sdcName > -1) {
                        let _dcNO = contName.substring(2);
                        if (!isNaN(_dcNO)) {
                            if ($(`div#head${_dcNO} a`).length > 0 && !isWizardTstruct) {
                                $(`div#head${_dcNO} a`).siblings('span[id^="dcCaption"]').text(setfldCap);
                            } else if ($(`li#head${_dcNO}`).length > 0 && !isWizardTstruct) {
                                $(`li#head${_dcNO} a`).text(setfldCap);
                            } else if ($(`#myTab li a#ank${_dcNO}`).length > 0 && !isWizardTstruct) {
                                $(`#myTab li a#ank${_dcNO}`).text(setfldCap);
                            } else if ($(`#wizardHeader .stepper-icon [id^='wdc${_dcNO}']`).length > 0 && isWizardTstruct) {
                                $(`#wizardHeader .stepper-icon [id^='wdc${_dcNO}']`).parents('.stepper-item').find('.stepper-title').text(setfldCap);
                            }
                        }
                    }
                    break;
                case ("collapse"):
                    /*if (fldname.substr(0, 2) == "dc") {*/
                    if (/^dc\d+$/.test(fldname.toLowerCase())) {
                        let _thisDicId = parseInt(fldname.substr(2, fldname.length), 10);
                        if ($("#dcBlean" + _thisDicId).length > 0) {
                            if ($("#dcBlean" + _thisDicId).is(":checked"))
                                $("#dcBlean" + _thisDicId).removeAttr("checked");
                            if ($("#divDc" + _thisDicId + " .gridIconBtns").length > 0) {
                                $("#divDc" + _thisDicId + " .griddivColumn").addClass("d-none");
                                $("#divDc" + _thisDicId + " .gridIconBtns").addClass("d-none");
                            } else
                                $("#divDc" + _thisDicId).addClass("d-none");
                            GetDcStateValue(_thisDicId, 'F');
                        }
                    }
                    break;
                case ("expand"):
                    /* if (fldname.substr(0, 2) == "dc") {*/
                    if (/^dc\d+$/.test(fldname.toLowerCase())) {
                        let _thisDicId = parseInt(fldname.substr(2, fldname.length), 10);
                        if ($("#dcBlean" + _thisDicId).length > 0) {
                            if (!$("#dcBlean" + _thisDicId).is(":checked"))
                                $("#dcBlean" + _thisDicId).attr("checked", "checked");
                            if ($("#divDc" + _thisDicId + " .gridIconBtns").length > 0) {
                                $("#divDc" + _thisDicId + " .griddivColumn").removeClass("d-none");
                                $("#divDc" + _thisDicId + " .gridIconBtns").removeClass("d-none");
                            } else
                                $("#divDc" + _thisDicId).removeClass("d-none");
                            GetDcStateValue(_thisDicId, 'T');
                        }
                    }
                    break;
                case ("mask"):
                    let _fldInd = GetFieldIndex(fldname);
                    if (_fldInd > -1) {
                        let _maskChar = sfFldVal;
                        let _maskApply = sfRno.split('♠');

                        if (isScriptFormLoad == "false") {
                            if (ScriptMaskFields.length > 0) {
                                var idx = $j.inArray(fldname, ScriptMaskFields);
                                if (idx == -1)
                                    ScriptMaskFields.push(fldname);
                            } else
                                ScriptMaskFields.push(fldname);
                        }
                        if (_maskApply.length > 1) {
                            FldMaskType[_fldInd] = "Show few characters";
                            FldMaskDetails[_fldInd] = _maskApply[0] + "♦" + _maskApply[1] + "♦" + _maskChar + "♦";
                        } else {
                            FldMaskType[_fldInd] = "Mask all characters";
                            FldMaskDetails[_fldInd] = "0♦0♦" + _maskChar + "♦";
                        }
                        let _dcNo = GetDcNo(fldname);
                        if (IsDcGrid(_dcNo)) {
                            let rowCnt = GetDcRowCount(_dcNo);
                            for (var i = 1; i <= rowCnt; i++) {
                                let _thisFld = fldname + GetClientRowNo(i, _dcNo) + "F" + _dcNo;
                                let _fld = $j("#" + _thisFld);
                                if (_fld.length > 0) {
                                    let _thisFldVal = GetFieldValue(_thisFld);
                                    if (_thisFldVal != "") {
                                        UpdateAllFieldValues(_thisFld, _thisFldVal);
                                        if (_maskApply[0] == "all") {
                                            _thisFldVal = MaskCharacter(_thisFldVal.toString(), _maskChar, 0);
                                        } else {
                                            _thisFldVal = MaskCharacter(_thisFldVal.toString(), _maskChar, _thisFldVal.length - _maskApply[0]);
                                            _thisFldVal = RevMaskCharacter(_thisFldVal.toString(), _maskChar, _maskApply[1]);
                                        }
                                        $("#" + _thisFld).val(_thisFldVal);
                                        $("#" + _thisFld).attr("value", _thisFldVal);
                                    }
                                }
                            }

                        } else {
                            let _thisFld = fldname + "000F" + _dcNo;
                            let _thisFldVal = GetFieldValue(_thisFld);
                            if (_thisFldVal != "") {
                                UpdateAllFieldValues(_thisFld, _thisFldVal);
                                if (_maskApply[0] == "all") {
                                    _thisFldVal = MaskCharacter(_thisFldVal.toString(), _maskChar, 0);
                                } else {
                                    _thisFldVal = MaskCharacter(_thisFldVal.toString(), _maskChar, _thisFldVal.length - _maskApply[0]);
                                    _thisFldVal = RevMaskCharacter(_thisFldVal.toString(), _maskChar, _maskApply[1]);
                                }
                                $("#" + _thisFld).val(_thisFldVal);
                                $("#" + _thisFld).attr("value", _thisFldVal);
                            }
                        }
                    }
                    break;
                case ("nomask"):
                    let _fldIndnm = GetFieldIndex(fldname);
                    if (_fldIndnm > -1) {
                        if (isScriptFormLoad == "false") {
                            if (ScriptMaskFields.length > 0) {
                                var idx = $j.inArray(fldname, ScriptMaskFields);
                                if (idx > -1)
                                    ScriptMaskFields.splice(idx, 1);
                            }
                        }
                        FldMaskType[_fldIndnm] = "";
                        FldMaskDetails[_fldIndnm] = "";
                        let _dcNo = GetDcNo(fldname);
                        if (IsDcGrid(_dcNo)) {
                            let rowCnt = GetDcRowCount(_dcNo);
                            for (var i = 1; i <= rowCnt; i++) {
                                let _thisFld = fldname + GetClientRowNo(i, _dcNo) + "F" + _dcNo;
                                let _fld = $j("#" + _thisFld);
                                if (_fld.length > 0) {
                                    let _thisFldVal = GetFieldValue(_thisFld);
                                    if (_thisFldVal != "") {
                                        UpdateAllFieldValues(_thisFld, _thisFldVal);
                                        $("#" + _thisFld).val(_thisFldVal);
                                        $("#" + _thisFld).attr("value", _thisFldVal);
                                    }
                                }
                            }

                        } else {
                            let _thisFld = fldname + "000F" + _dcNo;
                            let _thisFldVal = GetFieldValue(_thisFld);
                            if (_thisFldVal != "") {
                                UpdateAllFieldValues(_thisFld, _thisFldVal);
                                $("#" + _thisFld).val(_thisFldVal);
                                $("#" + _thisFld).attr("value", _thisFldVal);
                            }
                        }
                    }
                    break;
                case ("gridcelldisable"):
                    var _thisdcNo = GetDcNo(fldname);
                    if (IsDcGrid(_thisdcNo)) {
                        if (sfFldVal != "" && sfFldVal != 0) {
                            destfld = $j("#" + fldname + GetClientRowNo(sfFldVal, _thisdcNo) + "F" + _thisdcNo);
                            if (destfld.length > 0) {

                                var idx = $j.inArray(_thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~enable", AxFormContSetGridCell);
                                if (idx == -1) {
                                    idx = $j.inArray(_thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~disable", AxFormContSetGridCell);
                                    if (idx == -1)
                                        AxFormContSetGridCell.push(_thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~disable");
                                    else
                                        AxFormContSetGridCell[idx] = _thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~disable";
                                } else {
                                    AxFormContSetGridCell[idx] = _thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~disable";
                                }

                                if (IsPickListField(destfld.attr("id")) == true) {
                                    var pickFld = document.getElementById("img~" + destfld.attr("id"));
                                    pickFld.disabled = true;
                                    pickFld.className = "pickimg input-group-addon handCur";
                                }
                                if (destfld.attr("title") == dateString && (destfld.val() == dateString || destfld.val() == "''"))
                                    destfld.val("");

                                if (destfld.attr("type") == "checkbox") {
                                    var checlistid = destfld.attr("id");
                                    EnableDisableCheckbox(checlistid, true)
                                } else {

                                    // for disabling the Rich Text Box If it is disabled on dataload using form control
                                    if (destfld.attr("id").startsWith("rtf_") || destfld.attr("id").startsWith("rtfm_") || destfld.attr("id").startsWith("fr_rtf_") || GetDWBFieldType(GetFieldsName(destfld.attr("id"))) == "Rich Text") {
                                        $j("#cke_" + destfld.attr("id")).attr('disabled', 'disabled');
                                        destfld.css("display", "none");
                                        $j("#cke_" + destfld.attr("id")).prop("readonly", true);
                                    }
                                    if (destfld.attr("class") == "axpImg") {
                                        destfld.attr("onclick", "callnull");
                                    }
                                    destfld.prop("disabled", true);
                                    SetAutoCompAccess("disabled", destfld);
                                }
                            }
                        }
                    }
                    break;
                case ("gridcellenable"):
                    var _thisdcNo = GetDcNo(fldname);
                    if (IsDcGrid(_thisdcNo)) {
                        if (sfFldVal != "" && sfFldVal != 0) {
                            destfld = $j("#" + fldname + GetClientRowNo(sfFldVal, _thisdcNo) + "F" + _thisdcNo);
                            if (destfld.length > 0) {
                                //let _dbRowNo = GetDbRowNo(GetClientRowNo(sfFldVal, _thisdcNo), _thisdcNo);
                                var idx = $j.inArray(_thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~disable", AxFormContSetGridCell);
                                if (idx == -1) {
                                    idx = $j.inArray(_thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~enable", AxFormContSetGridCell);
                                    if (idx == -1)
                                        AxFormContSetGridCell.push(_thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~enable");
                                    else
                                        AxFormContSetGridCell[idx] = _thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~enable";
                                } else {
                                    AxFormContSetGridCell[idx] = _thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~enable";
                                }

                                if (IsPickListField(destfld.attr("id")) == true) {
                                    var pickFld = document.getElementById("img~" + destfld.attr("id"));
                                    pickFld.disabled = false;
                                    pickFld.className = "input-group-addon handCur pickimg";
                                }
                                if (destfld.val() == 0 && destfld.prop("type") != "select-one")
                                    destfld.val("");

                                if (destfld.attr("title") == dateString && destfld.val() == "")
                                    destfld.val(dateString);
                                if (destfld.attr("type") == "checkbox") {
                                    var checlistid = destfld.attr("id");

                                    EnableDisableCheckbox(checlistid, false)
                                } else {

                                    // for enabling the Rich Text Box If it is disabled on dataload using form control
                                    if (destfld.attr("id").startsWith("rtf_") || destfld.attr("id").startsWith("rtfm_") || destfld.attr("id").startsWith("fr_rtf_") || GetDWBFieldType(GetFieldsName(destfld.attr("id"))) == "Rich Text") {
                                        $j("#cke_" + destfld.attr("id")).prop("disabled", false);
                                        destfld.css("display", "none");
                                        $j("#cke_" + destfld.attr("id")).removeAttr("disabled");
                                    }
                                    if (destfld.attr("class") == "axpImg") {
                                        destfld.attr("onclick", null);
                                    }
                                    destfld.prop("disabled", false);
                                    destfld.prop("readOnly", false);
                                    destfld.removeAttr("readOnly");
                                    SetAutoCompAccess("enabled", destfld);
                                }
                            }
                        }
                    }
                    break;
                case ("setfontproperty"):
                    var _thisdcNo = GetDcNo(fldname);
                    if (IsDcGrid(_thisdcNo)) {
                        let _thisRowNo = sfFldVal;
                        let _thisProperties = sfRno;
                        if (_thisRowNo != "" && _thisRowNo != 0 && _thisProperties != "") {
                            if (_thisRowNo == "allrows") {
                                var rowCnt = 0;
                                rowCnt = GetDcRowCount(_thisdcNo);
                                for (var i = 1; i <= rowCnt; i++) {
                                    let _thisFldId = $j("#" + fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo);
                                    if (_thisFldId.length > 0) {
                                        const _setPreporties = _thisProperties.split('♠');
                                        let _fldStyles = "";
                                        _setPreporties.forEach(function (ele, ind) {
                                            if (ele != "" && ind == 0) {
                                                _fldStyles += "font-family:" + ele + ";";
                                            } else if (ele != "" && ind == 1) {
                                                _fldStyles += "font-size:" + ele + ";";
                                            } else if (ele != "" && ind == 2) {
                                                _fldStyles += "font-weight:" + ele + ";";
                                            } else if (ele != "" && ind == 3) {
                                                _fldStyles += "color:" + ele + ";";
                                            } else if (ele != "" && ind == 4) {
                                                _fldStyles += "background-color:" + ele + " !important";
                                            }
                                        });
                                        document.getElementById(fldname + GetClientRowNo(i, _thisdcNo) + "F" + _thisdcNo).setAttribute("style", _fldStyles);

                                        var idx = $j.inArray(_thisdcNo + "~" + fldname + "~*~setfont~" + _fldStyles, AxFormContSetGridCell);
                                        if (idx == -1) {
                                            AxFormContSetGridCell.push(_thisdcNo + "~" + fldname + "~*~setfont~" + _fldStyles);
                                        } else {
                                            AxFormContSetGridCell[idx] = _thisdcNo + "~" + fldname + "~*~setfont~" + _fldStyles;
                                        }
                                    }
                                }
                            } else {
                                let _thisFldId = $j("#" + fldname + GetClientRowNo(sfFldVal, _thisdcNo) + "F" + _thisdcNo);
                                if (_thisFldId.length > 0) {
                                    const _setPreporties = _thisProperties.split('♠');
                                    let _fldStyles = "";
                                    _setPreporties.forEach(function (ele, ind) {
                                        if (ele != "" && ind == 0) {
                                            _fldStyles += "font-family:" + ele + ";";
                                        } else if (ele != "" && ind == 1) {
                                            _fldStyles += "font-size:" + ele + ";";
                                        } else if (ele != "" && ind == 2) {
                                            _fldStyles += "font-weight:" + ele + ";";
                                        } else if (ele != "" && ind == 3) {
                                            _fldStyles += "color:" + ele + ";";
                                        } else if (ele != "" && ind == 4) {
                                            _fldStyles += "background-color:" + ele + " !important";
                                        }
                                    });
                                    document.getElementById(fldname + GetClientRowNo(sfFldVal, _thisdcNo) + "F" + _thisdcNo).setAttribute("style", _fldStyles);

                                    var idx = $j.inArray(_thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~setfont~" + _fldStyles, AxFormContSetGridCell);
                                    if (idx == -1) {
                                        AxFormContSetGridCell.push(_thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~setfont~" + _fldStyles);
                                    } else {
                                        AxFormContSetGridCell[idx] = _thisdcNo + "~" + fldname + "~" + GetClientRowNo(sfFldVal, _thisdcNo) + "~setfont~" + _fldStyles;
                                    }
                                }
                            }
                        }
                    } else {
                        let _thisProperties = sfRno;
                        if (_thisProperties != "") {
                            let _thisFldId = $j("#" + fldname + "000F" + _thisdcNo);
                            if (_thisFldId.length > 0) {
                                const _setPreporties = _thisProperties.split('♠');
                                let _fldStyles = "";
                                _setPreporties.forEach(function (ele, ind) {
                                    if (ele != "" && ind == 0) {
                                        _fldStyles += "font-family:" + ele + ";";
                                    } else if (ele != "" && ind == 1) {
                                        _fldStyles += "font-size:" + ele + ";";
                                    } else if (ele != "" && ind == 2) {
                                        _fldStyles += "font-weight:" + ele + ";";
                                    } else if (ele != "" && ind == 3) {
                                        _fldStyles += "color:" + ele + ";";
                                    } else if (ele != "" && ind == 4) {
                                        _fldStyles += "background-color:" + ele + " !important";
                                    }
                                });
                                document.getElementById(fldname + "000F" + _thisdcNo).setAttribute("style", _fldStyles);
                            }
                        }
                    }
                    break;
                default:
                    return;
            }
        }
    });
}


function ProcessScriptFormControlEvents(scriptStr, sfName) {
    if (scriptStr != "") {
        let actionStr = scriptStr.split("♠");
        switch (actionStr[0]) {
            case ("LoadPage"): //The given page will be displayed.
                var thisHtmlId = actionStr[1];
                thisHtmlId = thisHtmlId.substring(2);
                var thisParams = actionStr[2];
                thisParams = thisParams.replace(/♦/g, "&");
                var thisPageMode = actionStr[3];
                var thisOnClose = actionStr[4];
                if (thisPageMode == "" || thisPageMode.toLowerCase() == "d" || thisPageMode.toLowerCase() == "default") {
                    let curFrameObj = $(window.frameElement);
                    if (typeof curFrameObj != "undefined" && curFrameObj.attr("id") == "axpiframeac")
                        callParentNew("LoadIframeac(htmlPages.aspx?load=" + thisHtmlId + "&" + thisParams + ")", "function");
                    else
                        callParentNew("LoadIframe(htmlPages.aspx?load=" + thisHtmlId + "&" + thisParams + ")", "function");
                }
                else {
                    tparams = thisParams + "&AxPop=true";
                    tparams += "&axp_refresh=true";
                    var openpopup = 'htmlPages.aspx?load=' + thisHtmlId + "&" + tparams;
                    setTimeout(function () {
                        setTimeout(function () {
                            createPopup(openpopup);
                        }, 150);
                    }, 0);
                }
                break;
            case ("OpenPage"): //No definition available for this function but the code is there for backward compatibility
            case ("LoadForm")://The form will be opened in the new mode with passed parameter.
                var thisTrId = actionStr[1];
                var thisParams = actionStr[2];
                thisParams = thisParams.replace(/♦/g, "&");
                var thisPageMode = actionStr[3];
                var thisOnClose = actionStr[4];
                if (thisPageMode == "" || thisPageMode.toLowerCase() == "d" || thisPageMode.toLowerCase() == "default") {
                    if (transid == thisTrId) {
                        if (thisParams != "") {
                            thisParams.split('&').forEach(function (paramType) {
                                let _fldName = paramType.split('=')[0];
                                let _fldVal = paramType.split('=')[1];
                                _fldVal = ReplaceUrlSpecialChars(_fldVal);
                                let _dcNo = GetDcNo(_fldName);
                                let sffld = "";
                                if (IsDcGrid(_dcNo)) {
                                    sffld = _fldName + "001F" + _dcNo;
                                } else {
                                    sffld = _fldName + "000F" + _dcNo;
                                }
                                if ($j("#" + sffld).hasClass("multiFldChk") && _fldVal.trim() == "") {
                                    if (IsDcGrid(_dcNo)) {
                                        $j("input[id=" + sffld + "]").each(function () {
                                            $j(this).removeAttr("checked");
                                            $j(this).prop("checked", false);
                                        });
                                    } else {
                                        try {
                                            $("#" + sffld).val("");
                                            $("#" + sffld).data("valuelist", "");
                                            $("#" + sffld).tokenfield('setTokens', []);
                                        } catch (ex) { }
                                    }
                                } else {
                                    CallSetFieldValue(sffld, _fldVal);
                                }
                                let fRowNo = GetFieldsRowNo(sffld);
                                let dbRowNo = GetDbRowNo(fRowNo, _dcNo);
                                UpdateFieldArray(sffld, dbRowNo, _fldVal, "parent", "");
                            });
                        }
                    }
                    else {
                        let curFrameObj = $(window.frameElement);
                        let _actflag = "&act=open";
                        if (typeof curFrameObj != "undefined" && curFrameObj.attr("id") == "axpiframeac")
                            callParentNew("LoadIframeac(tstruct.aspx?transid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + _actflag + ")", "function");
                        else {
                            //callParentNew("LoadIframe(tstruct.aspx?transid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + _actflag + ")", "function");
                            let _thisDummyLoad = GetTstHtmlLsKey(thisTrId);
                            let _tstURI = "tstruct.aspx?transid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + _actflag + _thisDummyLoad + "";
                            _tstURI = _tstURI.replace(/♠/g, '%e2%99%a0');
                            callParentNew("lastLoadtstId=", _tstURI);
                            let redirectionLink = "tstruct.aspx?transid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + _actflag + _thisDummyLoad + "";
                            window.document.location.href = redirectionLink;
                        }
                    }
                }
                else {
                    let _actflag = "act=open";
                    tparams = thisParams + "&AxPop=true";
                    tparams += "&axp_refresh=true";
                    var openpopup = 'tstruct.aspx?' + _actflag + '&transid=' + thisTrId + "&" + tparams + `&isDupTab=${callParentNew('isDuplicateTab')}`;
                    setTimeout(function () {
                        setTimeout(function () {
                            createPopup(openpopup);
                        }, 150);
                    }, 0);
                }
                break;
            case ("LoadFormAndData")://A form will be displayed along with data in edit mode.
                var thisTrId = actionStr[1];
                var thisParams = actionStr[2];
                thisParams = thisParams.replace(/♦/g, "&");
                var thisPageMode = actionStr[3];
                var thisOnClose = actionStr[4];
                let trReadOnlyMode = typeof actionStr[5] != "undefined" ? actionStr[5] : "";
                if (trReadOnlyMode != "")
                    trReadOnlyMode = trReadOnlyMode.toLowerCase();
                let axoldmodelReadMode = "false";
                /*if (typeof axOldModelFlag != "undefined" && axOldModelFlag == "true")*/
                if (typeof callParentNew('axOldModelFlag') != "undefined" && callParentNew('axOldModelFlag') == "true")
                    axoldmodelReadMode = "true";
                if (thisPageMode == "" || thisPageMode.toLowerCase() == "d" || thisPageMode.toLowerCase() == "default") {
                    if (transid == thisTrId) {
                        var tstQureystr = '';
                        let _thisRecId = '';
                        if (thisParams != "") {
                            thisParams.split('&').forEach(function (paramType) {
                                tstQureystr += paramType + "♠";
                                if (paramType.toLowerCase().indexOf('recordid=') > -1)
                                    _thisRecId = paramType.split('=')[1];
                            });
                            tstQureystr += "act=load";
                        } else {
                            tstQureystr = "act=load♠";
                        }
                        if (axoldmodelReadMode == "false" && (trReadOnlyMode == "t" || trReadOnlyMode == "true")) {
                            SetFormDirty(false);
                            ShowDimmer(true);
                            let curFrameObj = $(window.frameElement);
                            if (typeof curFrameObj != "undefined" && curFrameObj.attr("id") == "axpiframeac")
                                callParentNew("LoadIframeac(EntityForm.aspx?tstid=" + thisTrId + "&" + tstQureystr + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load&loadformdata=true)", "function");
                            else
                                callParentNew("LoadIframe(EntityForm.aspx?tstid=" + thisTrId + "&" + tstQureystr + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load&loadformdata=true)", "function");
                        } else {
                            tstQureystr += "♠trromode=" + trReadOnlyMode + "";
                            tstQureystr = ReplaceUrlSpecialChars(tstQureystr);
                            setTimeout(function () {
                                FormControlSameFormLoad = true;
                                if (_thisRecId != "")
                                    GetLoadData(_thisRecId, tstQureystr);
                                else {
                                    tstQureystr += "♠loadformdata=true";
                                    GetFormLoadData(tstQureystr, "false", "false", "true");
                                }
                            }, 0);
                        }
                    }
                    else {
                        if (axoldmodelReadMode == "false" && (trReadOnlyMode == "t" || trReadOnlyMode == "true")) {
                            SetFormDirty(false);
                            ShowDimmer(true);
                            let curFrameObj = $(window.frameElement);
                            if (typeof curFrameObj != "undefined" && curFrameObj.attr("id") == "axpiframeac")
                                callParentNew("LoadIframeac(EntityForm.aspx?tstid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load&loadformdata=true)", "function");
                            else
                                callParentNew("LoadIframe(EntityForm.aspx?tstid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load&loadformdata=true)", "function");
                        } else {
                            let curFrameObj = $(window.frameElement);
                            if (typeof curFrameObj != "undefined" && curFrameObj.attr("id") == "axpiframeac")
                                callParentNew("LoadIframeac(tstruct.aspx?transid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load&loadformdata=true&trromode=" + trReadOnlyMode + ")", "function");
                            else {
                                let _thisDummyLoad = GetTstHtmlLsKey(thisTrId);
                                let _tstURI = "tstruct.aspx?transid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load&loadformdata=true&trromode=" + trReadOnlyMode + _thisDummyLoad + "";
                                _tstURI = _tstURI.replace(/♠/g, '%e2%99%a0');
                                callParentNew("lastLoadtstId=", _tstURI);
                                //callParentNew("LoadIframe(tstruct.aspx?transid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load&loadformdata=true&trromode=" + trReadOnlyMode + _thisDummyLoad + ")", "function");
                                let redirectionLink = "tstruct.aspx?transid=" + thisTrId + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load&loadformdata=true&trromode=" + trReadOnlyMode + _thisDummyLoad + "";
                                window.document.location.href = redirectionLink;
                            }
                        }
                    }
                }
                else {
                    if (axoldmodelReadMode == "false" && (trReadOnlyMode == "t" || trReadOnlyMode == "true")) {
                        SetFormDirty(false);
                        ShowDimmer(true);
                        tparams = thisParams + "&AxPop=true";
                        tparams += "&axp_refresh=true";
                        var openpopup = 'EntityForm.aspx?tstid=' + thisTrId + "&" + tparams + `&act=load&isDupTab=${callParentNew('isDuplicateTab')}&loadformdata=true`;
                        setTimeout(function () {
                            setTimeout(function () {
                                createPopup(openpopup);
                            }, 150);
                        }, 0);
                    } else {
                        tparams = thisParams + "&AxPop=true";
                        tparams += "&axp_refresh=true";
                        var openpopup = 'tstruct.aspx?act=load&transid=' + thisTrId + "&" + tparams + `&isDupTab=${callParentNew('isDuplicateTab')}&loadformdata=true&trromode=${trReadOnlyMode}`;
                        setTimeout(function () {
                            setTimeout(function () {
                                createPopup(openpopup);
                            }, 150);
                        }, 0);
                    }
                }
                break;
            case ("LoadIView"):
                var thisIvName = actionStr[1];
                var thisParams = actionStr[2];
                thisParams = thisParams.replace(/♦/g, "&");
                var thisPageMode = actionStr[3];
                var thisOnClose = actionStr[4];
                if (thisPageMode == "" || thisPageMode.toLowerCase() == "d" || thisPageMode.toLowerCase() == "default") {
                    let curFrameObj = $(window.frameElement);
                    if (typeof curFrameObj != "undefined" && curFrameObj.attr("id") == "axpiframeac")
                        callParentNew("LoadIframeac(ivtoivload.aspx?ivname=" + thisIvName + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load)", "function");
                    else
                        window.top.LoadIframe("ivtoivload.aspx?ivname=" + thisIvName + "&" + thisParams + `&isDupTab=${callParentNew('isDuplicateTab')}` + "&act=load");
                }
                else {
                    tparams = thisParams + "&AxIsPop=true";
                    var loadpopup = 'ivtoivload.aspx?ivname=' + thisIvName + "&" + tparams + `&isDupTab=${callParentNew('isDuplicateTab')}`;
                    setTimeout(function () {
                        setTimeout(function () {
                            createPopup(loadpopup);
                        }, 150);
                    }, 0);
                }
                break;
            default:
                return;
        }
    }
}


function CallAddRowFromScript(addDcNo, sfName, isExitDummy) {
    GetCurrentTime("Tstruct grid dc addrow through script(ws call)");
    var dcClientRows = GetDcClientRows(addDcNo);
    var lastRow = dcClientRows.getMaxVal();
    if (lastRow == 0) lastRow = 1;
    if (isExitDummy)
        lastRow = 1;
    var newCnt = parseInt(lastRow, 10);
    var newRowNo = GetRowNoHelper(newCnt);
    var visDcname = GetOpenTabDcs();
    var dbRowNo = GetDbRowNo(newRowNo, addDcNo);
    try {
        if (lastRow != 1 && !validateInlineGridRow(addDcNo, 'fromscript')) {
            let _grid = $("#gridHd" + addDcNo + " tbody tr:last").attr("id");
            if (typeof _grid != "undefined" && lastRow != 1) {
                let _grlastId = _grid.substr(_grid.lastIndexOf("F") - 3, 3);
                dbRowNo = GetDbRowNo(_grlastId, addDcNo);

                var slNo = GetSerialNoCnt(addDcNo);
                slNo = parseInt(slNo, 10) - 1;

                SetSerialNoCnt(addDcNo, slNo);
                AddDeletedRowsToArray(addDcNo, _grlastId)

                var a = "sp" + addDcNo + "R" + _grlastId + "F" + addDcNo;
                if (!$j("#" + a).hasClass("editWrapTr"))
                    $j("#" + a).remove();

                var rowCnt = $j("#hdnRCntDc" + addDcNo).val();
                rowCnt = parseInt(rowCnt, 10);
                if (axInlineGridEdit)
                    SetRowCount(addDcNo, rowCnt - 1, "d");
                var dcIndx = GetFormatGridIndex(addDcNo);
                UpdateDcRowArrays(addDcNo, _grlastId, "Delete", dcIndx);
                //UpdateFieldArray(axpIsRowValid + addDcNo + _grlastId + "F" + addDcNo, GetDbRowNo(_grlastId, addDcNo), "false", "parent", "AddRow");
            }
        }
    } catch (ex) { }
    var rid = $j("#recordid000F0").val();
    var filename = traceSplitStr + "AddRowFromScript-" + transid + traceSplitChar;
    var ixml = '<sqlresultset axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" dcname="' + addDcNo + '" rowno="' + dbRowNo + '" dcnames="' + visDcname + '" sessionid="' + sid + '" trace="' + filename + '" >';
    try {
        GetProcessTime();
        ASB.WebService.AddRowPerfWS(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ixml, addDcNo, tstDataId, true, resTstHtmlLS, SuccessCalAddDataForce, ErrorAddRowForceWS);
    } catch (exp) {
        $j($j("#" + sfName)).val('');
        try {
            let _fRowNo = GetFieldsRowNo(sfName);
            let _dcNo = GetFieldsDcNo(sfName);
            let _fldDbRow = GetDbRowNo(_fRowNo, _dcNo);
            UpdateFieldArray(sfName, _fldDbRow, "", "parent", "");
            UpdateAllFieldValues(sfName, "");
        } catch (ex) { }
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
        UpdateExceptionMessageInET("AddRowPerfWS exception : " + exp.message);
        GetProcessTime();
        GetTotalElapsTime();
    }
    function SuccessCalAddDataForce(result, eventArgs) {
        if (result != "" && result.split("♠*♠").length > 1) {
            tstDataId = result.split("♠*♠")[0];
            result = result.split("♠*♠")[1];
        }
        var passResult = result;
        try {
            if (result != "")
                result = result.split("*♠*")[2];
        } catch (ex) { }
        if (CheckSessionTimeout(result))
            return;
        resTstHtmlLS = "";
        var isValidRow = true;
        if (result != "" && (result == "Not a valid row!" || result.startsWith("Notavalidrow♦"))) {
            isValidRow = false;
            if (result == "Not a valid row!") {
                showAlertDialog("warning", eval(callParent('lcm[517]')));
                UpdateExceptionMessageInET("warning : " + eval(callParent('lcm[517]')));
            }
            else {
                showAlertDialog("warning", result.split('♦')[1]);
                UpdateExceptionMessageInET("warning : " + result.split('♦')[1]);
            }
        }
        if (isValidRow) {
            axpScriptaddrowres = passResult;
            if (isExitDummy && !isMobile) {
                isScriptFCAddClick = true;
                if (isExitDummy) {
                    gridDummyRowVal.splice($.inArray(addDcNo.toString() + "~001", gridDummyRowVal), 1);
                    gridDummyRows = false;
                    gridRowEditOnLoad = false;
                }
                AssignLoadValues(result, "CallAdd");
                isScriptFCAddClick = false;
            } else {
                if (axInlineGridEdit)
                    addNewInlineGridRow(addDcNo);
                else
                    AddNewRowInDc(addDcNo);
            }
            axpScriptaddrowres = "";
            updateInlineGridRowValues();
        } else {
            GetProcessTime();
            GetTotalElapsTime();
        }
        $j($j("#" + sfName)).val('');
        try {
            let _fRowNo = GetFieldsRowNo(sfName);
            let _dcNo = GetFieldsDcNo(sfName);
            let _fldDbRow = GetDbRowNo(_fRowNo, _dcNo);
            UpdateFieldArray(sfName, _fldDbRow, "", "parent", "");
            UpdateAllFieldValues(sfName, "");
        } catch (ex) { }
        AxWaitCursor(false);
        ShowDimmer(false);
    }
    function ErrorAddRowForceWS(result) {
        try {
            UpdateExceptionMessageInET("ErrorAddRowForceWS : " + result._message);
        } catch (ex) { }
        ArrActionLog = "";
        ChangedFields = new Array();
        ChangedFieldDbRowNo = new Array();
        ChangedFieldValues = new Array();
        DeletedDCRows = new Array();
        $j($j("#" + sfName)).val('');
        try {
            let _fRowNo = GetFieldsRowNo(sfName);
            let _dcNo = GetFieldsDcNo(sfName);
            let _fldDbRow = GetDbRowNo(_fRowNo, _dcNo);
            UpdateFieldArray(sfName, _fldDbRow, "", "parent", "");
            UpdateAllFieldValues(sfName, "");
        } catch (ex) { }
        AxWaitCursor(false);
        ShowDimmer(false);
    }
}

function EnableDisableSFCBtns(obj, enable) {
    var isObjFound = false;
    if (obj.length > 0) {
        if (enable == "hide") {
            if ($(obj).hasClass("dwbIvBtnbtm"))
                $(obj).addClass("d-none");
            else if ($(obj).hasClass("dwbBtn"))
                $(obj).addClass("d-none");
            else if ($(obj).parent(".toolbarRightMenu").length > 0 || (typeof $(obj) != "undefined" && $(obj).attr("id") == "btnAppsDraft" && $(obj).parent().parent(".toolbarRightMenu").length>0))
                $(obj).addClass("d-none");
            else if ($(obj).parent("li").length > 0)
                $(obj).parent("li").hide();
            else if ($(obj).parent("div").length > 0)
                $(obj).parent("div").addClass("d-none");
            else {
                $(obj).parent("td").hide();
                $(obj).parents("table").find("th#th-" + $(obj).attr("id")).hide();
                $(obj).parents("table").find("tfoot td#tf-" + $(obj).attr("id")).addClass('d-none');
            }
        } else {
            if ($(obj).hasClass("dwbIvBtnbtm"))
                $(obj).removeClass("d-none");
            else if ($(obj).hasClass("dwbBtn"))
                $(obj).removeClass("d-none");
            else if ($(obj).parent(".toolbarRightMenu").length > 0 || (typeof $(obj) != "undefined" && $(obj).attr("id") == "btnAppsDraft" && $(obj).parent().parent(".toolbarRightMenu").length > 0))
                $(obj).removeClass("d-none");
            else if ($(obj).parent("li").length > 0)
                $(obj).parent("li").show();
            else if ($(obj).parent("div").length > 0)
                $(obj).parent("div").removeClass("d-none");
            else {
                $(obj).parent("td").show();
                $(obj).parents("table").find("th#th-" + $(obj).attr("id")).show();
                $(obj).parents("table").find("tfoot td#tf-" + $(obj).attr("id")).removeClass('d-none');
            }
        }
    }
}

function AlertNoAction() {
    //email smptp test settings
    var smptpHost = $('#smtphost000F1').val();
    var smptpPort = $('#smtpport000F1').val();
    var smptpUser = $('#smtpuser000F1').val();
    var smptpPassword = $('#smtppwd000F1').val();
    try {
        ASB.WebService.TestMailSettings(smptpHost, smptpPort, smptpUser, smptpPassword, sucessCallbackMail, OnException);
    } catch (ex) { }
}

function sucessCallbackMail(result, eventArgs) {
    //sucess callback code for TestMailSettings
    var resultcheck = result;
    try {
        var xmlDoc = jQuery.parseXML(result);
        if (xmlDoc) {
            if (result.indexOf("result") != -1)
                resultcheck = xmlDoc.getElementsByTagName('result')[0].firstChild.nodeValue; //<result>Test mail has been sent to souvik.b@agile-labs.com'</result>
            else
                resultcheck = xmlDoc.getElementsByTagName('error')[0].firstChild.nodeValue;
        }
        if (resultcheck.indexOf("Test mail has been sent to") != -1)
            showAlertDialog("success", resultcheck);
        else
            showAlertDialog("warning", resultcheck);
    } catch (ex) { }
}

function barCodeScannerInit() {
    var scanqrfld = $j(document).find('input[id^=barqr_]').attr("id");
    if (typeof scanqrfld != "undefined") {
        try {
            onScan.detachFrom(document);
        } catch (ex) { }
        loadAndCall({
            files: {
                css: [],
                js: ["/ThirdParty/onscan.js-master/onscan.min.js"]
            },
            callBack() {
                onScan.attachTo(document, {
                    reactToPaste: true,
                    ignoreIfFocusOn: ":not(input[id^='barqr_'])",
                    onScan: function (sCode) { // Alternative to document.addEventListener('scan')
                        if (($j('#waitDiv').css('display') == 'none') == true) {
                            $j("#" + scanqrfld).val(sCode);
                            isScriptFCAddClick = true;
                            axpScanBarFldFocus = scanqrfld;
                            $j($j("#" + scanqrfld)).blur();
                            if (axpScriptIsAddrow == false)
                                $j($j("#" + scanqrfld)).val('').focus();
                            //ShowDimmer(false);
                            isScriptFCAddClick = false;
                        } else {
                            showAlertDialog("warning", "Please wait till the process complete!");
                        }
                    },
                    onKeyDetect: function (iKeyCode, oEvent) {
                        try {
                            let _thisEle = $(oEvent.target).attr("id");
                            if (typeof _thisEle != "undefined" && _thisEle != "" && !_thisEle.toLowerCase().startsWith("barqr_")) {
                                return false;
                            }
                        } catch (ex) { }
                    }
                });
            }
        });
    }
}

function getHybridWeightScaleInfo(axpWeightFldId) {

    try {
        var counter = 0;
        var intervalID = setInterval(() => {
            counter++;
            if (counter == 10) {
                clearInterval(intervalID);
                ShowDimmer(false);
            }

            var weighedValue = getRedisString("HybridWeightScaleInfo", callParentNew("hybridGUID"));

            if (weighedValue != "") {
                var weighedValueJSON = JSON.parse(weighedValue);
                weighedValue = weighedValueJSON.data.weight + weighedValueJSON.data.unit;
                SetFieldValue(axpWeightFldId, weighedValue);
                UpdateFieldArray(axpWeightFldId, GetFieldsRowNo(axpWeightFldId), weighedValue, "parent", "");
                MainBlur($j("#" + axpWeightFldId));
                $("#" + axpWeightFldId).blur();

                clearInterval(intervalID);
                ShowDimmer(false);
            }
        }, 1000);
    } catch (ex) { }

}

function closeWeightScalePort() {
    try {
        ASB.WebService.NotifyCloseWeightScalePort(callParentNew("hybridGUID"), (success) => { }, (error) => { });
    } catch (error) { }
}

function DropzoneInit(dvId) {
    let _thisDiv;
    if (typeof dvId != "undefined") {
        _thisDiv = $(dvId + " .dropzone:not(.dropzoneGrid)");
    } else {
        _thisDiv = $("[id^='divDc']:not(:has(.editWrapTr)) .dropzone:not(.dropzoneGrid)");// $("#divDc1 .dropzone");
    }
    _thisDiv.each(function () {
        if (($(this).parents('.editWrapTr').length > 0 && axInlineGridEdit) || ((!axInlineGridEdit && AxpGridForm == "form" && !$(this).parents('.editWrapTr').length > 0) && $(this).parents('.editWrapTr').length != 0))
            return;
        const id = "#" + $(this).attr("id");
        var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx'));
        const fuName = $(id).attr("id").substr(9);
        funame = fuName.substring(0, fuName.lastIndexOf("F") - 3);
        var _ffuIndex = $j.inArray(funame, FNames);
        if (typeof recordid != "undefined" && recordid != "" && recordid != "0" && typeof _ffuIndex != "undefined" && _ffuIndex > -1) {
            let _thisdc = GetDcNo(funame);
            if (!IsDcGrid(_thisdc)) {
                if (IsTabDc(_thisdc)) {
                    let _dvTabInd = $j.inArray(_thisdc, TabDCs);
                    if (TabDCStatus[_dvTabInd] == "0") {
                        return;
                    }
                }
            }
        }
        const dropzone = document.querySelector(id);

        // set the preview element template
        var previewNode = dropzone.querySelector(".dropzone-item");
        previewNode.id = "";
        var previewTemplate = previewNode.parentNode.innerHTML;
        previewNode.parentNode.removeChild(previewNode);       
        let attachmentSizeMB = callParentNew("axAttachmentSize", "axAttachmentSize") == undefined ? 1 : callParentNew("axAttachmentSize", "axAttachmentSize");
        let maxFilesUpload = AxpFileUploadlmt != "0" ? AxpFileUploadlmt : 100;
        let _uploadFileTypes = "";
        if (typeof UploadFileTypes != "undefined" && UploadFileTypes == "true" && typeof UploadFileTypesVal != "undefined")
            _uploadFileTypes = UploadFileTypesVal;
        let isthisReadOnly = false;
        if (FFieldReadOnly[_ffuIndex] == "True") {
            let _thisFldId = $(id).attr("id").substr(9);
            $("#" + _thisFldId).prop('disabled', true);
            $(id).parent().addClass('readonly');
            isthisReadOnly = true;
        }

        let protectFile = "false";
        if (typeof tstConfigurations != "undefined" && tstConfigurations.config !== undefined && tstConfigurations.config.length > 0) {
            let _finalJson = [];
            try {
                tstConfigurations.config.forEach(function (val) {
                    var ret = {};
                    $.map(val, function (value, key) {
                        ret[key.toLowerCase()] = value;
                    });
                    _finalJson.push(ret);
                });
            } catch (ex) { }
            protectFile = _finalJson.find(item => item.props?.trim().toLowerCase() === "file protected in tstruct attachments")?.propsval;
            if (typeof protectFile == "undefined" || protectFile == "")
                protectFile = "false";
        }

        var myDropzone = new Dropzone(id, { // Make the whole body a dropzone
            url: url + `TstFileUpload.ashx?thisFld=${$(id).attr("id").substr(9)}&filePath=${$(".axpFilePath_" + $(id).attr("id").substring(("dropzone_axpfile_").length)).val() == undefined ? "" : $(".axpFilePath_" + $(id).attr("id").substring(("dropzone_axpfile_").length)).val()}&dcNo=${GetFieldsDcNo($(id).attr("id"))}&attFldName=${GetFieldsName($(id).attr("id").substr(9))}&fileExt=${_uploadFileTypes}&futransid=${transid}&protectFile=${protectFile}`,
            maxFilesize: attachmentSizeMB, // Max filesize in MB
            previewTemplate: previewTemplate,
            previewsContainer: id + " .dropzone-items", // Define the container to display the previews
            clickable: isthisReadOnly ? "" : typeof $(id + " .dropzone-select").parents(".grdattch").attr("id") != "undefined" ? "#" + $(id + " .dropzone-select").parents(".grdattch").attr("id") : "#" + $(id + " .dropzone-select").parents(".nongrdattch").attr("id"), // Define the element that should be used as click trigger to select files.
            maxFiles: maxFilesUpload
        });

        const dzdefault = dropzone.querySelectorAll('.dz-default');
        $(dzdefault).addClass("d-none");       
        if (isthisReadOnly) {
            $(id).on('dragenter', function (e) {
                myDropzone.fileAddedVia = 'drag';
            });
        }

        myDropzone.on("addedfile", function (file) {
            if (isthisReadOnly && myDropzone.fileAddedVia == 'drag') {
                return;
            }
            ShowDimmer(true);
            var extension = file.name.substring(file.name.lastIndexOf('.') + 1);
            if (_uploadFileTypes != "" && _uploadFileTypes.indexOf(extension.toLowerCase()) == -1) {
                showAlertDialog("error", eval(callParent('lcm[521]')));
                ShowDimmer(false);
                return;
            }
            if (gridRowEditOnLoad) {
                let thisFldId = $(id).attr("id").substr(9);
                var fieldRowNo = GetFieldsRowNo(thisFldId);
                var fldDcNo = GetFieldsDcNo(thisFldId);
                UpdateGridRowFlags(thisFldId, fldDcNo, fieldRowNo);
            }
            let _thisFilePath = $(".axpFilePath_" + $(id).attr("id").substring(("dropzone_axpfile_").length)).val()
            if (typeof _thisFilePath == "undefined") {
                var _textareaId = 'axpfilepath_' + $(id).attr("id").substring(("dropzone_axpfile_").length);
                var _textarea = $('textarea').filter(function () {
                    return this.id && this.id.toLowerCase() === _textareaId.toLowerCase();
                });
                if (typeof _textarea != "undefined" && _textarea.length != 0) {
                    _thisFilePath = $(_textarea).val();
                    if (typeof _thisFilePath == "undefined")
                        _thisFilePath = "";
                } else
                    _thisFilePath = "";
            }
            this.options.url = url + `TstFileUpload.ashx?thisFld=${$(id).attr("id").substr(9)}&filePath=${_thisFilePath}&dcNo=${GetFieldsDcNo($(id).attr("id"))}&attFldName=${GetFieldsName($(id).attr("id").substr(9))}&fileExt=${_uploadFileTypes}&futransid=${transid}&protectFile=${protectFile}`;
            // Hookup the start button
            const dropzoneItems = dropzone.querySelectorAll('.dropzone-item');
            dropzoneItems.forEach(dropzoneItem => {
                dropzoneItem.style.display = '';
            });
            $(file.previewElement).parents(".dropzone").css({ "width": $(file.previewElement).parents(".dropzone").outerWidth() + "px" })
        });

        // Hide the total progress bar when nothing"s uploading anymore
        myDropzone.on("complete", function (progress) {
            if (isthisReadOnly && myDropzone.fileAddedVia == 'drag') {
                myDropzone.fileAddedVia = null;
                return;
            }
            ShowDimmer(true);
            const progressBars = dropzone.querySelectorAll('.dz-complete');
            if (progress.status == 'success') {
                if (progress.xhr.response == "success" || progress.xhr.response.startsWith("success:")) {
                    let mesg = progress.xhr.response;
                    let _thisupfile = "";
                    if (mesg != "")
                        _thisupfile = mesg.split('♠')[1];
                    
                    $(progress.previewElement).parent(".dropzone-items").removeClass('d-none');
                    $(progress.previewElement).parents(".dropzone").find("a.dropzone-select").contents().filter(function () {
                        return this.nodeType === 3; // Node type 3 is a text node
                    }).remove();

                    let zipFileName = "";
                    if (typeof protectFile != "undefined" && protectFile.toLowerCase() == "true" && mesg.indexOf('~fileName:') > -1) {
                        let responseText = progress.xhr.response;
                        zipFileName = _thisupfile;// responseText.split("~fileName:")[1] || "";
                        progress.upload = progress.upload || {};
                        progress.upload.filename = zipFileName;
                        progress.name = zipFileName;
                        progress.fileName = zipFileName;
                        const $filenameSpan = $(progress.previewElement).find('.dropzone-filename span');
                        $filenameSpan.text(zipFileName);
                        $filenameSpan.attr('title', zipFileName);
                    }

                    // Adjust width of file name elements
                    $(progress.previewElement).parents(".dropzone").addClass('d-flex-file');
                    $(progress.previewElement).parent(".dropzone-items").addClass('d-flex-file');
                    if (typeof $(progress.previewElement).find('.dropzone-filename span').attr('title') == "undefined") {
                        const filenameElement = $(progress.previewElement).find('.dropzone-filename span');
                        $(progress.previewElement).find('.dropzone-filename span').attr('title', filenameElement.text());
                    }
                    adjustFileNameWidths($(progress.previewElement), myDropzone);
                    $(progress.previewElement).find(".dropzone-toolbar").css({ "margin-left": "auto" });
                    let fuInpId = $(progress.previewElement).parents(".dropzone").attr("id").substr(9);
                    let fuInpVal = $("#" + fuInpId).val();
                    if (fuInpVal == "") {
                        fuInpVal = _thisupfile;
                    }
                    else {
                        const dupFileObj = fuInpVal.split(',').filter(file => file == _thisupfile)?.[0];
                        if (dupFileObj) {
                            myDropzone.removeFile(progress);
                            return false;
                        } else
                            fuInpVal = fuInpVal + "," + _thisupfile;
                    }
                    $("#" + fuInpId).val(fuInpVal);

                    var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
                    let filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
                    filePath += _thisupfile;

                    $(progress.previewElement).find(".dropzone-filename").attr("onclick", "ShowAxpFileuploadLink('" + fuInpId + "','" + _thisupfile + "','" + filePath + "',event)");

                    var fRowNo = GetFieldsRowNo(fuInpId);
                    var dcNo = GetFieldsDcNo(fuInpId);
                    var fldDbRow = GetDbRowNo(fRowNo, dcNo);
                    UpdateFieldArray(fuInpId, fldDbRow, fuInpVal, "parent", "");
                    UpdateAllFieldValues(fuInpId, fuInpVal);
                    try {
                        if (IsDcGrid(dcNo) && ((!axInlineGridEdit && AxpGridForm == "form") || (isMobile && mobileCardLayout != "none" && !axInlineGridEdit)))
                            formGridRowBlur($("#" + fuInpId));
                    } catch (ex) { }
                    let fileSuccessMgs = mesg.split(":")[1];
                    if (fileSuccessMgs.indexOf('~') > -1)
                        fileSuccessMgs = fileSuccessMgs.split("~")[0];
                    showAlertDialog("success", fileSuccessMgs);
                } else if (progress.xhr.response == 'error:File already exists, please rename and upload again!') {
                    let mesg = progress.xhr.response;
                    myDropzone.removeFile(progress);
                    showAlertDialog("error", mesg.split(":")[1]);
                } else {
                    let mesg = progress.xhr.response;
                    myDropzone.removeFile(progress);
                    showAlertDialog("error", mesg.split(":")[1]);
                }
                myDropzone.emit("queuecomplete");
            } else if (progress.status == 'added') {
                let fuInpId = $(myDropzone.element).attr("id").substr(9);
                let fuInpVal = $("input#" + fuInpId).val();
                fuInpVal = fuInpVal.replace(file.name + ",", "").replace(file.name, "");
                if (fuInpVal[fuInpVal.length - 1] == ",")
                    fuInpVal = fuInpVal.substring(0, fuInpVal.length - 1);
                $("input#" + fuInpId).val(fuInpVal);
                var fRowNo = GetFieldsRowNo(fuInpId);
                var dcNo = GetFieldsDcNo(fuInpId);
                var fldDbRow = GetDbRowNo(fRowNo, dcNo);
                UpdateFieldArray(fuInpId, fldDbRow, fuInpVal, "parent", "");
                UpdateAllFieldValues(fuInpId, fuInpVal);
                try {
                    if (IsDcGrid(dcNo) && ((!axInlineGridEdit && AxpGridForm == "form") || (isMobile && mobileCardLayout != "none" && !axInlineGridEdit)))
                        formGridRowBlur($("#" + fuInpId));
                } catch (ex) { }
                if (fuInpVal == "") {
                    $(myDropzone.element).find(".fileuploadmore").addClass("d-none");
                    $(myDropzone.element).find("#gridAttachCounter").addClass("d-none");
                    $(myDropzone.element).find("span.spanAttCount").text(this.files.length);
                } else if (this.files.length > 0)
                    $(myDropzone.element).find("span.spanAttCount").text(this.files.length);
            }
            else {
                if (progress.status == 'error')
                    showAlertDialog("warning", $(progress.previewTemplate).find(".dropzone-error").text());
                myDropzone.emit("queuecomplete");
            }
        });

        myDropzone.on("queuecomplete", function (file, res) {
            ShowDimmer(false);
        });

        myDropzone.on("error", function (file) {
            myDropzone.removeFile(file);
        });

        myDropzone.on("removedfile", function (file) {
            try {
                if (typeof file.xhr != 'undefined' && file.xhr.response == 'error:File already exists, please rename and upload again!') {
                    return;
                }
            } catch (ex) { }
            let fuInpId = $(myDropzone.element).attr("id").substr(9);
            if ($(myDropzone.element).hasClass('isgridEdit'))
                return;
            let fuInpVal = $("input#" + fuInpId).val();
            let axpFName = fuInpId.substr(7);
            var axpPath = "";
            if (typeof $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0] != "undefined" && $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0].value != "")
                axpPath = $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0].value;
            else
                axpPath = $("input[id*=" + axpFName + "]").not(".axpAttach,.grdAttach").length > 0 ? $("input[id*=" + axpFName + "]").not(".axpAttach,.grdAttach")[0].value : "";
            if (axpPath != "" && axpPath.endsWith("\\*")) {
                let _fileVals = "";
                $.each(fuInpVal.split(','), function (i, val) {
                    if ((val.substr(20) != file.name && val != file.name) || (typeof file.fileName != "undefined" && val.substr(20) != file.fileName && val != file.fileName))
                        _fileVals += val + ',';
                    else {
                        DeletedFieldValue.push(fuInpId + "~" + val);
                    }
                });
                fuInpVal = _fileVals;
                if (fuInpVal != "" && fuInpVal.endsWith(',')) {
                    fuInpVal = fuInpVal.slice(0, -1);
                }
            } else {
                if (typeof file.fileName != "undefined" && file.fileName != "")
                    fuInpVal = fuInpVal.replace(file.fileName + ",", "").replace(file.fileName, "");
                else
                    fuInpVal = fuInpVal.replace(file.name + ",", "").replace(file.name, "");
                if (fuInpVal != "" && fuInpVal.endsWith(',')) {
                    fuInpVal = fuInpVal.slice(0, -1);
                }
                DeletedFieldValue.push(fuInpId + "~" + file.name);
            }
            try {
                $("textarea#" + fuInpId).val(fuInpVal);
                $("textarea#" + fuInpId).attr('value', fuInpVal);
            } catch (ex) { }
            $("input#" + fuInpId).val(fuInpVal);
            $("input#" + fuInpId).attr('value', fuInpVal);
            var fRowNo = GetFieldsRowNo(fuInpId);
            var dcNo = GetFieldsDcNo(fuInpId);
            var fldDbRow = GetDbRowNo(fRowNo, dcNo);
            UpdateFieldArray(fuInpId, fldDbRow, fuInpVal, "parent", "");
            UpdateAllFieldValues(fuInpId, fuInpVal);
            try {
                if (IsDcGrid(dcNo) && ((!axInlineGridEdit && AxpGridForm == "form") || (isMobile && mobileCardLayout != "none" && !axInlineGridEdit)))
                    formGridRowBlur($("#" + fuInpId));
            } catch (ex) { }
            if (fuInpVal == "") {
                $(myDropzone.element).find(".fileuploadmore").addClass("d-none");
                $(myDropzone.element).find("span.spanAttCount").text(this.files.length);
                $(myDropzone.element).find("#gridAttachCounter").addClass("d-none");
            } else if (this.files.length > 0)
                $(myDropzone.element).find("span.spanAttCount").text(this.files.length);

            $(myDropzone.element).addClass('d-flex-file');
            $(myDropzone.element).find(".dropzone-items").addClass('d-flex-file');
            adjustFileNameWidths($(myDropzone.element).find(".dropzone-items"), myDropzone);
        });


        let fuInpId = $(id).attr("id").substr(9)
        let fuInpVal = typeof $("textarea#" + fuInpId).val() == "undefined" ? $("#" + fuInpId).val() : $("textarea#" + fuInpId).val();
        if (fuInpVal != "") {
            let axpFName = fuInpId.substr(7);
            var axpPath = "";
            if (typeof $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0] != "undefined" && $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0].value != "")
                axpPath = $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0].value;
            else
                axpPath = $("input[id*=" + axpFName + "]").not(".axpAttach,.grdAttach").length > 0 ? $("input[id*=" + axpFName + "]").not(".axpAttach,.grdAttach")[0].value : "";
            $.each(fuInpVal.split(','), function (i, val) {
                let _fullval = val;
                if (_fullval == "")
                    return;
                if (axpPath != "" && axpPath.endsWith("\\*"))
                    val = val.substr(20);
                var file = {
                    name: _fullval,
                    size: "0",
                    status: Dropzone.ADDED,
                    accepted: true
                };
                myDropzone.emit("addedfile", file);
                myDropzone.emit("complete", file);
                myDropzone.files.push(file);


                $(file.previewElement).parent(".dropzone-items").removeClass('d-none');
                $(file.previewElement).parents(".dropzone").find("a.dropzone-select").contents().filter(function () {
                    return this.nodeType === 3; // Node type 3 is a text node
                }).remove();
                $(file.previewElement).parents(".dropzone").addClass('d-flex-file');
                $(file.previewElement).parent(".dropzone-items").addClass('d-flex-file');
                if (typeof $(file.previewElement).find('.dropzone-filename span').attr('title') == "undefined") {
                    const filenameElement = $(file.previewElement).find('.dropzone-filename span');
                    $(file.previewElement).find('.dropzone-filename span').attr('title', filenameElement.text());
                }
                adjustFileNameWidths($(file.previewElement), myDropzone);
                $(file.previewElement).find(".dropzone-toolbar").css({ "margin-left": "auto" });

                var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
                let filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
                filePath += _fullval;

                $(file.previewElement).find(".dropzone-filename span").text(val);
                $(file.previewElement).find(".dropzone-filename span").attr("data-fullfile", _fullval);
                $(file.previewElement).find(".dropzone-filename").attr("onclick", "ShowAxpFileuploadLink('" + $(myDropzone.files[0].previewElement).parents(".dropzone").attr("id").substr(9) + "','" + _fullval + "','" + filePath + "',event)");
                if (isthisReadOnly)
                    $(file.previewElement).find(".dropzoneItemDelete").addClass('d-none');
            });
            //$(id).find(".fileuploadmore").removeClass("d-none");
            //$(id).find("#gridAttachCounter").removeClass("d-none");
            //$(id).find("span.spanAttCount").text(myDropzone.files.length);
        }
    });
}

function adjustFileNameWidths(_thisEle, _thismyDropzone) {
    const parentElement = _thisEle.parents('.dropzone');
    const items = parentElement.find('.dropzone-item');
    const numItems = items.length;
    const minChars = 7;
    const avgCharWidth = 9;
    if (numItems > 0) {
        let containerWidth = parentElement.outerWidth();
        containerWidth -= parseInt(parentElement.css('padding-left')) + parseInt(parentElement.css('padding-right'));
        parentElement.find('.dropzone-panel').each(function () {
            containerWidth -= $(this).outerWidth(true) + 10;
        });
        const minItemWidthPixels = minChars * avgCharWidth;
        const minItemWidthPercentage = (minItemWidthPixels / containerWidth) * 100;
        let remainingWidth = containerWidth - (minItemWidthPixels * numItems);
        let remainingItemsWidthPercentage = 100 - (minItemWidthPercentage * numItems);

        if (_thisEle.find('.dropzone-filename').attr("title") == 'some_image_file_name.jpg') {
            const _filenameElement = _thisEle.find('.dropzone-filename span');
            const _fullText = _filenameElement.text();
            _thisEle.find('.dropzone-filename').attr('title', _fullText);
        }
        if (remainingWidth > 0 && remainingItemsWidthPercentage > 0) {
            items.each(function () {
                const filenameElement = $(this).find('.dropzone-filename span');
                if (typeof $(this).find('.dropzone-filename span').attr('title') == "undefined") {
                    $(this).find('.dropzone-filename span').attr('title', $(this).find('.dropzone-filename span').text());
                    adjustFileNameWidths($(this), _thismyDropzone);
                }
                const fullText = $(this).find('.dropzone-filename span').attr('title');
                const itemWidthPercentage = minItemWidthPercentage + (remainingItemsWidthPercentage / numItems);
                const itemWidthPixels = (containerWidth * itemWidthPercentage / 100);
                const maxCharsPerItem = Math.floor(itemWidthPixels / avgCharWidth);
                let charsToShow = Math.min(fullText.length, maxCharsPerItem);
                if (charsToShow < minChars) {
                    charsToShow = minChars;
                }
                let truncatedText = fullText.substring(0, charsToShow);
                $(this).css("width", itemWidthPercentage + "%");
                filenameElement.text(truncatedText);
                $(this).addClass('showItem');
                $(this).find('.dropzone-delete').css("width", "22px");
                $(this).find(".dropzone-toolbar").css({ "margin-left": "auto" });
            });
            _thisEle.parents(".dropzone").find(".fileuploadmore").addClass("d-none");
            _thisEle.parents(".dropzone").find("#gridAttachCounter").addClass("d-none");
        } else if (_thisEle.parents(".dropzone").find(".fileuploadmore").hasClass('d-none')) {
            let _icon = _thisEle.parents(".dropzone").find(".fileuploadmore").detach();
            _thisEle.parent('.dropzone-items').after(_icon);

            let _iconatt = _thisEle.parents(".dropzone").find("#gridAttachCounter").detach();
            _thisEle.parents('.dropzone').find(".fileuploadmore").after(_iconatt);

            _thisEle.parents(".dropzone").find(".fileuploadmore").removeClass("d-none");
            _thisEle.parents(".dropzone").find("#gridAttachCounter").removeClass("d-none");
            _thisEle.parents(".dropzone").find("span.spanAttCount").text(_thismyDropzone.files.length);
        } else {
            _thisEle.parents(".dropzone").find("#gridAttachCounter").removeClass("d-none");
            _thisEle.parents(".dropzone").find("span.spanAttCount").text(_thismyDropzone.files.length);
        }
    }
}
$(document).on("mouseover", ".fileuploadmore", function () {
    if (typeof $(this).parents("#dropzone_tstattach.dropzone") != "undefined" && $(this).parents("#dropzone_tstattach.dropzone").length > 0) {
        if ($(this).parents(".dropzone").find(".dropzone-items").hasClass("d-none")) {
            let elId = $(this).closest(".dropzone").attr("id");
            var popover_instance = bootstrap.Popover.getInstance(this);
            if (popover_instance) {
                popover_instance.dispose();
            }
            var newPopover = new bootstrap.Popover(this, {
                content: function () {
                    return "<div class=\"dropzone dropzone-queue mt-n3\" id=\"" + elId + "\">"
                        + $(this).closest(".dropzone").find(".dropzone-items").html() + "</div>";
                },
                placement: "bottom",
                boundary: "viewport",
                html: true,
                sanitize: false
            });
            newPopover.show();
            setTimeout(() => {
                var newPopoverInstance = bootstrap.Popover.getInstance(this);
                if (newPopoverInstance?.tip) {
                    let tip = $(newPopoverInstance.tip);
                    let trigger = $(this);
                    let triggerOffset = trigger.offset();
                    let triggerWidth = trigger.outerWidth();
                    let tipWidth = tip.outerWidth();

                    tip.css({
                        "position": "absolute",
                        "left": (triggerOffset.left + triggerWidth - tipWidth) + "px",
                        "top": (triggerOffset.top + trigger.outerHeight()) + "px", 
                        "transform": "none",
                        "max-width": "350px",
                        "max-height": "400px",
                        "overflow-y": "auto",
                        "overflow-x": "hidden",
                        "white-space": "normal"
                    });
                }
            }, 0);
        } else
            $(this).parents(".dropzone").find(".dropzone-items").addClass("d-none");
    } else {
        let elId = $(this).parents(".dropzone").attr("id");
        var popover_instance = bootstrap.Popover.getInstance(this); //get instance
        if (popover_instance.tip != null)
            popover_instance.tip.classList.add('mw-350px');
        var dropzoneItems = $(this).parents(".dropzone").find(".dropzone-items").clone();
        dropzoneItems.find('.dropzone-item.showItem').remove();
        popover_instance._config.content = "<div class=\"dropzone dropzone-queue mt-n3\" id=\"" + elId + "\">" + dropzoneItems.html() + "</div>";

        popover_instance.show();
        setTimeout(function () {
            if (popover_instance.tip != null)
                popover_instance.tip.classList.add(...'mh-400px overflow-auto'.split(" "));
        }, 0);
    }
});

$j(document).on("click", "body", function (e) {
    if (!$(e.target).hasClass("fileuploadmore") && !$(e.target).hasClass("grdattch") && !$(e.target).hasClass("gridattach") && !$(e.target).hasClass("fw-boldest") && !$(e.target).hasClass("popover-arrow") && !$(e.target).hasClass("popover") && !$(e.target).hasClass("popover-body") && !$(e.target).hasClass("dropzone-delete") && !$(e.target).hasClass("dropzoneItemDelete") && !$(e.target).hasClass("dropzoneItemDeleteHeader") && !$(e.target).hasClass("dropzoneGridItemDelete") && !$(e.target).hasClass("dropzone-panel") && !$(e.target).hasClass("dropzone-file") && !$(e.target).hasClass("dropzone") && !$(e.target).hasClass("dropzone-items") && !$(e.target).hasClass("dropzone-item") && !$(e.target).hasClass("dropzone-filename") && !$(e.target).parent().hasClass("dropzone-filename")) {
        $("[data-bs-toggle]").popover("hide");
    }
});

$j(document).off("click", ".popover-body .dropzoneItemDelete").on("click", ".popover-body .dropzoneItemDelete", function (e) {
    const id = $(e.target).parents(".dropzone").attr("id");
    const dropzone = document.querySelector("#" + id);
    var myDropzone = Dropzone.forElement(dropzone);
    //myDropzone.removeFile(myDropzone.files[0]);
    const delFileName = typeof $(e.target).parents(".dropzone-item").find(".dropzone-filename span").data('fullfile') == 'undefined' ? $(e.target).parents(".dropzone-item").find(".dropzone-filename").text() : $(e.target).parents(".dropzone-item").find(".dropzone-filename span").data('fullfile');
    const delFileObj = myDropzone.files.filter(file => file.name == delFileName)?.[0];
    if (delFileObj) {
        $("[data-bs-toggle]").popover("hide");
        //myDropzone.removeFile(delFileObj);
        tstFileDeleteConfir(myDropzone, delFileObj);
    }
});

function TstFldImageUpload(elem) {
    var fileUpload = elem;
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }


    var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx'));
    let isAxpImagePath = $("#isAxpImagePathHidden").val();
    $.ajax({
        url: url + `TstImageUpload.ashx?fldname=${$(elem).attr("id")}&isAxpImagePath=${isAxpImagePath}&transid=${transid}`,
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success(result) {
            const _thisName = $(elem).attr("id");
            if (result.indexOf("success:") > -1) {
                $(elem).parents(".image-input").find(".imageFileUpload").addClass("d-none");
                $(elem).parents(".image-input").find(".profile-pic").removeClass("d-none");
                let hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
                let imgFldName = GetFieldsName(_thisName);
                let fldValuePath = "";
                if (imgFldName.startsWith('nodb_') || isAxpImagePath == "true")
                    fldValuePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/" + imgFldName + "/" + files[0].name;
                else
                    fldValuePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/" + files[0].name;
                $("img#" + _thisName).css({ "background-image": "unset" });
                $("img#" + _thisName).prop("src", fldValuePath);
                $("#" + _thisName).parent().find(".delete-button").removeClass("d-none");

                try {
                    if (DeletedFieldValue.length > 0 && DeletedFieldValue.filter(x => x.startsWith(_thisName + "~")).length > 0) {
                        let _thisDelVal = DeletedFieldValue.filter(x => x.startsWith(_thisName + "~"));
                        let _thisDelInx = DeletedFieldValue.indexOf(_thisDelVal[0]);
                        DeletedFieldValue.splice(_thisDelInx, 1);
                    }
                } catch (ex) { }

                UpdateFieldArray(_thisName, "0", files[0].name, "parent", "");
                UpdateAllFieldValues(_thisName, files[0].name);
                showAlertDialog("success", result.replace("success:", ""));
            } else {
                $("img#" + _thisName).css({ "background-image": "unset" });
                if (result == "error:sessionexpired")
                    CheckSessionTimeout(result);
                else if (result.startsWith("error:"))
                    showAlertDialog("error", result.replace("error:", ""));
            }
        },
        error(err) {
            handlerType = "upload";
            showAlertDialog("error", err.statusText);
        }
    });
}

function HeaderAttachFiles() {
    const id = "#attachment-overlay";
    const dropzone = document.querySelector(id);
    if (dropzone == null)
        return;

    // set the preview element template
    var previewNode = dropzone.querySelector(".dropzone-item");
    if (previewNode == null)
        return;
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx'));

    let attachmentSizeMB = callParentNew("axAttachmentSize", "axAttachmentSize") == undefined ? 1 : callParentNew("axAttachmentSize", "axAttachmentSize");
    let isSameFile = false;
    let AxtstAFSDB = "false";
    if (typeof AxtstAttachFSDB != "undefined" && AxtstAttachFSDB == "true")
        AxtstAFSDB = "true";
    let _uploadFileTypes = "";
    if (typeof UploadFileTypes != "undefined" && UploadFileTypes == "true" && typeof UploadFileTypesVal != "undefined")
        _uploadFileTypes = UploadFileTypesVal;
    let protectFile = "false";
    if (typeof tstConfigurations != "undefined" && tstConfigurations.config !== undefined && tstConfigurations.config.length > 0) {
        let _finalJson = [];
        try {
            tstConfigurations.config.forEach(function (val) {
                var ret = {};
                $.map(val, function (value, key) {
                    ret[key.toLowerCase()] = value;
                });
                _finalJson.push(ret);
            });
        } catch (ex) { }
        protectFile = _finalJson.find(item => item.props?.trim().toLowerCase() === "file protected in tstruct attachments")?.propsval;
        if (typeof protectFile == "undefined" || protectFile == "")
            protectFile = "false";
    }
    let _delFileList = "";
    var myhdrDropzone = new Dropzone(id, { // Make the whole body a dropzone
        url: url + `HeaderFileUpload.ashx?act=attach&transid=${transid}&delFile=${_delFileList}&AxtstAFSDB=${AxtstAFSDB}&fileExt=${_uploadFileTypes}&protectFile=${protectFile}`,
        maxFilesize: attachmentSizeMB, // Max filesize in MB
        previewTemplate: previewTemplate,
        previewsContainer: id + " .dropzone-items", // Define the container to display the previews
        clickable: id + " .dropzone-select", // Define the element that should be used as click trigger to select files.
        dictDuplicateFile: "Duplicate Files Cannot Be Uploaded",
        preventDuplicates: true
    });

    const dzdefault = dropzone.querySelectorAll('.dz-default');
    $(dzdefault).addClass("d-none");

    myhdrDropzone.on("addedfile", function (file) {
        var extension = file.name.substring(file.name.lastIndexOf('.') + 1);
        if (_uploadFileTypes != "" && _uploadFileTypes.indexOf(extension.toLowerCase()) == -1) {
            showAlertDialog("error", eval(callParent('lcm[521]')));
            return;
        }
        // Hookup the start button
        this.options.url = url + `HeaderFileUpload.ashx?act=attach&transid=${transid}&delFile=${_delFileList}&AxtstAFSDB=${AxtstAFSDB}&fileExt=${_uploadFileTypes}&protectFile=${protectFile}`;
        const dropzoneItems = dropzone.querySelectorAll('.dropzone-item');
        dropzoneItems.forEach(dropzoneItem => {
            dropzoneItem.style.display = '';
        });
    });

    // Hide the total progress bar when nothing"s uploading anymore
    myhdrDropzone.on("complete", function (progress) {
        const progressBars = dropzone.querySelectorAll('.dz-complete');
        if (progress.status == 'success') {
            if (progress.xhr.response == "success" || progress.xhr.response.startsWith("success:")) {
                let mesg = progress.xhr.response;
                let zipFileName = "";
                if (typeof protectFile != "undefined" && protectFile.toLowerCase() == "true" && mesg.indexOf('~fileName:') > -1) {
                    let responseText = progress.xhr.response;
                    zipFileName = responseText.split("~fileName:")[1] || "";
                    progress.upload = progress.upload || {};
                    progress.upload.filename = zipFileName;
                    progress.name = zipFileName;
                    progress.fileName = zipFileName;
                    const $filenameSpan = $(progress.previewElement).find('.dropzone-filename span');
                    $filenameSpan.text(zipFileName);
                    $filenameSpan.attr('title', zipFileName);
                }

                let dupFileObj = filenamearray.filter(file => file == progress.name)?.[0];
                if (typeof dupFileObj == "undefined" && fileonloadarray.length > 0) {
                    dupFileObj = fileonloadarray.filter(file => file == progress.name)?.[0];
                }
                if (dupFileObj) {
                    isSameFile = true;
                    myhdrDropzone.removeFile(progress);
                    showAlertDialog("error", "File already exists, please rename and upload again!");
                    return false;
                } else {
                    isSameFile = false;
                    $(progress.previewElement).parents(".dropzone").find(".fileuploadmore").removeClass("d-none");
                    $(progress.previewElement).parents(".dropzone").find("#hattachCounter").removeClass("d-none");
                    $(progress.previewElement).parents(".dropzone").find("span.spanAttCount").text(this.files.length);
                    if (zipFileName == "")
                        zipFileName = progress.name;
                    filenamearray.push(zipFileName);

                    var rid = 0;
                    if ($j("#recordid000F0").length > 0)
                        rid = $j("#recordid000F0").val();

                    var attachstr = CheckUrlSpecialChars(zipFileName);
                    $(progress.previewElement).find(".dropzone-filename").attr("onclick", "OpenAttachment('" + attachstr + "','" + rid + "',true);");
                }
                //showAlertDialog("success", mesg.split(":")[1]);
                let fileSuccessMgs = mesg.split(":")[1];
                if (fileSuccessMgs.indexOf('~') > -1)
                    fileSuccessMgs = fileSuccessMgs.split("~")[0];
                showAlertDialog("success", fileSuccessMgs);
            } else if (progress.xhr.response == 'error:File already exists, please rename and upload again!') {
                let mesg = progress.xhr.response;
                isSameFile = true;
                myhdrDropzone.removeFile(progress);
                showAlertDialog("error", mesg.split(":")[1]);
            } else {
                let mesg = progress.xhr.response;
                myhdrDropzone.removeFile(progress);
                showAlertDialog("error", mesg.split(":")[1]);
            }
        } else {
            if (progress.status == 'error')
                showAlertDialog("warning", $(progress.previewTemplate).find(".dropzone-error").text());
        }
    });

    myhdrDropzone.on("error", function (file) {
        myhdrDropzone.removeFile(file);
    });

    myhdrDropzone.on("removedfile", function (file) {
        if (!isSameFile) {
            if (_delFileList != "")
                _delFileList += "♦" + file.name;
            else
                _delFileList += file.name;
            RemoveArrVal(file.name, filenamearray);
        }
        else
            isSameFile = false;
        if (this.files.length > 0)
            //$(file.previewElement).parents(".dropzone").find("span.spanAttCount").text(this.files.length);
            $(this.element).find("span.spanAttCount").text(this.files.length);
        else {
            $(this.element).find(".fileuploadmore").addClass("d-none");
            $(this.element).find("span.spanAttCount").text(this.files.length);
            $(this.element).find("#hattachCounter").addClass("d-none");
        }
    });

    if (fileonloadarray.length > 0) {
        $.each(fileonloadarray, function (i, val) {
            var file = {
                name: val,
                size: "0",
                status: Dropzone.ADDED,
                accepted: true
            };
            myhdrDropzone.emit("addedfile", file);
            myhdrDropzone.emit("complete", file);
            myhdrDropzone.files.push(file);
            let rid = 0;
            if ($j("#recordid000F0").length > 0)
                rid = $j("#recordid000F0").val();

            let attachstr = CheckUrlSpecialChars(file.name);
            $(file.previewElement).find(".dropzone-filename").attr("onclick", "OpenAttachment('" + attachstr + "','" + rid + "');");
        });
        $(id).find("span.spanAttCount").text(myhdrDropzone.files.length);
        $(id).find(".fileuploadmore").removeClass("d-none");
        $(id).find("#hattachCounter").removeClass("d-none");
    }
}

$j(document).off("click", ".popover-body .dropzoneItemDeleteHeader").on("click", ".popover-body .dropzoneItemDeleteHeader", function (e) {
    const dropzone = document.querySelector("#attachment-overlay");
    var myDropzone = Dropzone.forElement(dropzone);
    const delFileName = $(e.target).parents(".dropzone-item").find(".dropzone-filename").text();
    const delFileObj = myDropzone.files.filter(file => file.name == delFileName)?.[0];
    if (delFileObj) {
        var rid = 0;
        if ($j("#recordid000F0").length > 0)
            rid = $j("#recordid000F0").val();
        RemoveFile(delFileName, rid, delFileObj);
        //myDropzone.removeFile(delFileObj);
    }
});

function rtfCkeditorAlignment() {
    var A = document.getElementsByTagName("textarea") && document.getElementsByClassName("memofam");
    for (var B = 0; B < A.length; B++) {
        if (A[B].id.startsWith("rtf_") || A[B].id.startsWith("rtfm_") || A[B].id.startsWith("fr_rtf_") || GetDWBFieldType(GetFieldsName(A[B].id)) == "Rich Text" || (GetDWBFieldType(GetFieldsName(A[B].id)) == "Large Text" && !A[B].id.startsWith("exp_editor_"))) {
            A[B].parentElement.classList.add("flex-root");
            A[B].parentElement.parentElement.classList.add(..."d-flex flex-column".split(" "));
        } else if (A[B].id.startsWith("exp_editor_")) {
            A[B].parentElement.classList.add("flex-root");
            if (staticRunMode)
                A[B].parentElement.parentElement.classList.add(..."d-flex flex-column".split(" "));
        } else if (staticRunMode) {
            A[B].parentElement.classList.add("flex-root");
            A[B].parentElement.parentElement.classList.add(..."d-flex flex-column".split(" "));
        }
    }
}

function DropzoneGridInit(dvId) {
    let _thisDiv;
    if (typeof dvId != "undefined") {
        _thisDiv = $(dvId + " .dropzoneGrid");
    } else {
        _thisDiv = $("[id^='divDc']:not(:has(.editWrapTr)) .dropzoneGrid");// $("#divDc1 .dropzone");
    }
    _thisDiv.each(function () {
        //if ($(this).parents('.editWrapTr').length > 0)
        if ($(this).parents('.editWrapTr').length > 0 && !isMobile && AxpGridForm != 'form')
            return;
        const id = "#" + $(this).attr("id");
        const dropzone = document.querySelector(id);

        // set the preview element template
        var previewNode = dropzone.querySelector(".dropzone-item");
        previewNode.id = "";
        var previewTemplate = previewNode.parentNode.innerHTML;
        previewNode.parentNode.removeChild(previewNode);

        var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx'));
        const fuName = $(id).attr("id").substr(9);
        funame = fuName.substring(0, fuName.lastIndexOf("F") - 3);
        let attachmentSizeMB = callParentNew("axAttachmentSize", "axAttachmentSize") == undefined ? 1 : callParentNew("axAttachmentSize", "axAttachmentSize");
        let maxFilesUpload = AxpFileUploadlmt != "0" ? AxpFileUploadlmt : 100;
        let AxtstAFSDB = "false";
        if (typeof AxtstAttachFSDB != "undefined" && AxtstAttachFSDB == "true")
            AxtstAFSDB = "true";

        let _uploadFileTypes = "";
        if (typeof UploadFileTypes != "undefined" && UploadFileTypes == "true" && typeof UploadFileTypesVal != "undefined")
            _uploadFileTypes = UploadFileTypesVal;
        let _isDummyFirstUp = false;
        let _isDummyFirstcount = 0;
        let _valFiles = "";
        let protectFile = "false";
        if (typeof tstConfigurations != "undefined" && tstConfigurations.config !== undefined && tstConfigurations.config.length > 0) {
            let _finalJson = [];
            try {
                tstConfigurations.config.forEach(function (val) {
                    var ret = {};
                    $.map(val, function (value, key) {
                        ret[key.toLowerCase()] = value;
                    });
                    _finalJson.push(ret);
                });
            } catch (ex) { }
            protectFile = _finalJson.find(item => item.props?.trim().toLowerCase() === "file protected in tstruct attachments")?.propsval;
            if (typeof protectFile == "undefined" || protectFile == "")
                protectFile = "false";
        }

        var myDropzone = new Dropzone(id, { // Make the whole body a dropzone
            url: url + `TstGridFileUpload.ashx?thisFld=${$(id).attr("id").substr(9)}&filePath=${$("#" + funame + "path" + $(id).attr("id").substring($(id).attr("id").lastIndexOf("F") - 3)).val() == undefined ? "nofield" : $("#" + funame + "path" + $(id).attr("id").substring($(id).attr("id").lastIndexOf("F") - 3)).val()}&dcNo=${GetFieldsDcNo($(id).attr("id"))}&attFldName=${funame}&attTransId=${transid}&rcId=${recordid}&upldFiles=${_valFiles}&AxtstAFSDB=${AxtstAFSDB}&fileExt=${_uploadFileTypes}&dummyRowEdit=${gridRowEditOnLoad}&protectFile=${protectFile}`,
            maxFilesize: attachmentSizeMB, // Max filesize in MB
            previewTemplate: previewTemplate,
            previewsContainer: id + " .dropzone-items", // Define the container to display the previews
            clickable: "#" + $(id + " .dropzone-select").parents(".grdattch").attr("id"), // Define the element that should be used as click trigger to select files.
            maxFiles: maxFilesUpload
        });

        const dzdefault = dropzone.querySelectorAll('.dz-default');
        $(dzdefault).addClass("d-none");

        myDropzone.on("addedfile", function (file) {
            ShowDimmer(true);
            if (gridRowEditOnLoad) {
                let thisFldId = $(id).attr("id").substr(9);
                var fieldRowNo = GetFieldsRowNo(thisFldId);
                var fldDcNo = GetFieldsDcNo(thisFldId);
                UpdateGridRowFlags(thisFldId, fldDcNo, fieldRowNo);
            }
            if (typeof isAddRowWsCalled != "undefined" && isAddRowWsCalled == "true") {
                ShowDimmer(false);
                var pendingFile = file;
                var checkInterval = setInterval(function () {
                    if (typeof isAddRowWsCalled != "undefined" && isAddRowWsCalled == "false") {
                        clearInterval(checkInterval);
                        if (pendingFile) {
                            myDropzone.options.url = url + `TstGridFileUpload.ashx?thisFld=${$(id).attr("id").substr(9)}&filePath=${$("#" + funame + "path" + $(id).attr("id").substring($(id).attr("id").lastIndexOf("F") - 3)).val() == undefined ? "nofield" : $("#" + funame + "path" + $(id).attr("id").substring($(id).attr("id").lastIndexOf("F") - 3)).val()}&dcNo=${GetFieldsDcNo($(id).attr("id"))}&attFldName=${funame}&attTransId=${transid}&rcId=${recordid}&upldFiles=${_valFiles}&AxtstAFSDB=${AxtstAFSDB}&fileExt=${_uploadFileTypes}&protectFile=${protectFile}`;
                            const dropzoneItems = dropzone.querySelectorAll('.dropzone-item');
                            dropzoneItems.forEach(dropzoneItem => {
                                dropzoneItem.style.display = '';
                            });
                            $(pendingFile.previewElement).parents(".dropzone").css({ "width": $(pendingFile.previewElement).parents(".dropzone").outerWidth() + "px" })
                            _isDummyFirstUp = true;
                            myDropzone.processFile(pendingFile);
                            pendingFile = null;
                        }
                    }
                }, 100);
                return;
            }

            var extension = file.name.substring(file.name.lastIndexOf('.') + 1);
            if (_uploadFileTypes != "" && _uploadFileTypes.indexOf(extension.toLowerCase()) == -1) {
                showAlertDialog("error", eval(callParent('lcm[521]')));
                ShowDimmer(false);
                return;
            }

            funame = GetFieldsName($(id).attr("id").substr(9));
            let _thisDcNo = GetDcNo(funame);
            _valFiles = "";
            $("#gridHd" + _thisDcNo + " tbody tr").each(function () {
                let _trId = $(this).attr('id');
                let _rowNo = _trId.substring(_trId.lastIndexOf("F"), _trId.lastIndexOf("F") - 3);
                let _curFldId = funame + _rowNo + "F" + _thisDcNo;
                if ($("#" + _curFldId).length > 0 && $("#" + _curFldId).val() != "") {
                    let _thidDbRow = GetDbRowNo(_rowNo, _thisDcNo);
                    let _thisRowFiles = $("#" + _curFldId).val();
                    _thisRowFiles.split(',').forEach(function (ele) {
                        _valFiles += ele + "♠" + _thidDbRow + ",";
                    });
                }
            });
            /*}*/
            this.options.url = url + `TstGridFileUpload.ashx?thisFld=${$(id).attr("id").substr(9)}&filePath=${$("#" + funame + "path" + $(id).attr("id").substring($(id).attr("id").lastIndexOf("F") - 3)).val() == undefined ? "nofield" : $("#" + funame + "path" + $(id).attr("id").substring($(id).attr("id").lastIndexOf("F") - 3)).val()}&dcNo=${GetFieldsDcNo($(id).attr("id"))}&attFldName=${funame}&attTransId=${transid}&rcId=${recordid}&upldFiles=${_valFiles}&AxtstAFSDB=${AxtstAFSDB}&fileExt=${_uploadFileTypes}&protectFile=${protectFile}`;
            // Hookup the start button
            const dropzoneItems = dropzone.querySelectorAll('.dropzone-item');
            dropzoneItems.forEach(dropzoneItem => {
                dropzoneItem.style.display = '';
            });
            $(file.previewElement).parents(".dropzone").css({ "width": $(file.previewElement).parents(".dropzone").outerWidth() + "px" })
        });

        // Hide the total progress bar when nothing"s uploading anymore
        myDropzone.on("complete", function (progress) {
            const progressBars = dropzone.querySelectorAll('.dz-complete');
            if (progress.status == 'success') {
                if (progress.xhr.response == "success" || progress.xhr.response.startsWith("success:")) {
                    let mesg = progress.xhr.response;
                    _isDummyFirstcount = _isDummyFirstcount + 1;
                    if (_isDummyFirstUp && _isDummyFirstcount > 1) {
                        _isDummyFirstUp = false;
                        return;
                    }
                    let zipFileName = "";
                    if (typeof protectFile != "undefined" && protectFile.toLowerCase() == "true" && mesg.indexOf('~fileName:') > -1) {
                        let responseText = progress.xhr.response;
                        zipFileName = responseText.split("~fileName:")[1] || "";
                        progress.upload = progress.upload || {};
                        progress.upload.filename = zipFileName;
                        progress.name = zipFileName;
                        progress.fileName = zipFileName;
                        const $filenameSpan = $(progress.previewElement).find('.dropzone-filename span');
                        $filenameSpan.text(zipFileName);
                        $filenameSpan.attr('title', zipFileName);
                    }

                    $(progress.previewElement).parent(".dropzone-items").removeClass('d-none');
                    $(progress.previewElement).parents(".dropzone").find("a.dropzone-select").contents().filter(function () {
                        return this.nodeType === 3; // Node type 3 is a text node
                    }).remove();

                    // Adjust width of file name elements
                    $(progress.previewElement).parents(".dropzone").addClass('d-flex-file');
                    $(progress.previewElement).parent(".dropzone-items").addClass('d-flex-file');
                    if (typeof $(progress.previewElement).find('.dropzone-filename span').attr('title') == "undefined") {
                        const filenameElement = $(progress.previewElement).find('.dropzone-filename span');
                        $(progress.previewElement).find('.dropzone-filename span').attr('title', filenameElement.text());
                    }
                    adjustFileNameWidths($(progress.previewElement), myDropzone);
                    $(progress.previewElement).find(".dropzone-toolbar").css({ "margin-left": "auto" });

                    let fuInpId = $(progress.previewElement).parents(".dropzone").attr("id").substr(9);
                    let fuInpVal = $("#" + fuInpId).val();
                    if (fuInpVal == "") {
                        let _thisFldWidth = $("#" + fuInpId).parent().width();
                        $("#" + fuInpId).parent().width(_thisFldWidth);
                    }
                    if (zipFileName!="")
                        fuInpVal = fuInpVal == "" ? zipFileName : fuInpVal + "," + zipFileName;
                    else
                        fuInpVal = fuInpVal == "" ? progress.name : fuInpVal + "," + progress.name;
                    $("#" + fuInpId).val(fuInpVal);

                    var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
                    let filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
                    filePath += zipFileName != "" ? zipFileName : progress.name;

                    $(progress.previewElement).find(".dropzone-filename").attr("onclick", "ShowGridAttLink('" + filePath + "')")

                    var fRowNo = GetFieldsRowNo(fuInpId);
                    var dcNo = GetFieldsDcNo(fuInpId);
                    var fldDbRow = GetDbRowNo(fRowNo, dcNo);
                    UpdateFieldArray(fuInpId, fldDbRow, fuInpVal, "parent", "");
                    UpdateAllFieldValues(fuInpId, fuInpVal);
                    try {
                        if (IsDcGrid(dcNo) && ((!axInlineGridEdit && AxpGridForm == "form") || (isMobile && mobileCardLayout != "none" && !axInlineGridEdit)))
                            formGridRowBlur($("#" + fuInpId));
                    } catch (ex) { }
                    let fileSuccessMgs = mesg.split(":")[1];
                    if (fileSuccessMgs.indexOf('~') > -1)
                        fileSuccessMgs = fileSuccessMgs.split("~")[0];
                    showAlertDialog("success", fileSuccessMgs);
                } else if (progress.xhr.response == 'error:File already exists, please rename and upload again!') {
                    let mesg = progress.xhr.response;
                    myDropzone.removeFile(progress);
                    showAlertDialog("error", mesg.split(":")[1]);
                } else if (progress.xhr.response != "" && progress.xhr.response.startsWith('error:File already exists at row no ')) {
                    let mesg = progress.xhr.response;
                    myDropzone.removeFile(progress);
                    showAlertDialog("error", mesg.split(":")[1]);
                } else if (progress.xhr.response != "" && progress.xhr.response.startsWith('error:dummyrowloaddep')) {
                    let mesg = progress.xhr.response;
                    myDropzone.removeFile(progress);
                }
                else {
                    let mesg = progress.xhr.response;
                    myDropzone.removeFile(progress);
                    showAlertDialog("error", mesg.split(":")[1]);
                }
            } else if (progress.status == 'added') {
                let fuInpId = $(myDropzone.element).attr("id").substr(9);
                let fuInpVal = $("#" + fuInpId).val();
                fuInpVal = fuInpVal.replace(file.name + ",", "").replace(file.name, "");
                if (fuInpVal[fuInpVal.length - 1] == ",")
                    fuInpVal = fuInpVal.substring(0, fuInpVal.length - 1);
                $("#" + fuInpId).val(fuInpVal);
                var fRowNo = GetFieldsRowNo(fuInpId);
                var dcNo = GetFieldsDcNo(fuInpId);
                var fldDbRow = GetDbRowNo(fRowNo, dcNo);
                UpdateFieldArray(fuInpId, fldDbRow, fuInpVal, "parent", "");
                UpdateAllFieldValues(fuInpId, fuInpVal);
                try {
                    if (IsDcGrid(dcNo) && ((!axInlineGridEdit && AxpGridForm == "form") || (isMobile && mobileCardLayout != "none" && !axInlineGridEdit)))
                        formGridRowBlur($("#" + fuInpId));
                } catch (ex) { }
                if (fuInpVal == "") {
                    $(myDropzone.element).find(".fileuploadmore").addClass("d-none");
                    $(myDropzone.element).find("#gridAttachCounter").addClass("d-none");
                    $(myDropzone.element).find("span.spanAttCount").text(this.files.length);
                } else if (this.files.length > 0)
                    $(myDropzone.element).find("span.spanAttCount").text(this.files.length);
            }
            else {
                if (progress.status == 'error')
                    showAlertDialog("warning", $(progress.previewTemplate).find(".dropzone-error").text());
            }
            myDropzone.emit("queuecomplete");
        });

        myDropzone.on("queuecomplete", function (file, res) {
            ShowDimmer(false);
        });

        myDropzone.on("error", function (file) {
            myDropzone.removeFile(file);
        });

        myDropzone.on("removedfile", function (file) {
            try {
                if (typeof file.xhr != 'undefined' && file.xhr.response == 'error:File already exists, please rename and upload again!') {
                    return;
                } else if (typeof file.xhr != 'undefined' && file.xhr.response.startsWith('error:File already exists at row no ')) {
                    return;
                }
            } catch (ex) { }
            let fuInpId = $(myDropzone.element).attr("id").substr(9);
            let fuInpVal = $("#" + fuInpId).val();
            fuInpVal = fuInpVal.replace(file.name + ",", "").replace(file.name, "");
            if (fuInpVal[fuInpVal.length - 1] == ",")
                fuInpVal = fuInpVal.substring(0, fuInpVal.length - 1);
            $("#" + fuInpId).val(fuInpVal);
            var fRowNo = GetFieldsRowNo(fuInpId);
            var dcNo = GetFieldsDcNo(fuInpId);
            var fldDbRow = GetDbRowNo(fRowNo, dcNo);
            UpdateFieldArray(fuInpId, fldDbRow, fuInpVal, "parent", "");
            UpdateAllFieldValues(fuInpId, fuInpVal);
            try {
                if (IsDcGrid(dcNo) && ((!axInlineGridEdit && AxpGridForm == "form") || (isMobile && mobileCardLayout != "none" && !axInlineGridEdit)))
                    formGridRowBlur($("#" + fuInpId));
            } catch (ex) { }
            if (fuInpVal == "") {
                $(myDropzone.element).find(".fileuploadmore").addClass("d-none");
                $(myDropzone.element).find("#gridAttachCounter").addClass("d-none");
                $(myDropzone.element).find("span.spanAttCount").text(this.files.length);
            } else if (this.files.length > 0)
                $(myDropzone.element).find("span.spanAttCount").text(this.files.length);

            $(myDropzone.element).addClass('d-flex-file');
            $(myDropzone.element).find(".dropzone-items").addClass('d-flex-file');
            adjustFileNameWidths($(myDropzone.element).find(".dropzone-items"), myDropzone);
        });


        let fuInpId = $(id).attr("id").substr(9)
        let fuInpVal = $("#" + fuInpId).val();
        if (fuInpVal != "") {
            $.each(fuInpVal.split(','), function (i, val) {
                var file = {
                    name: val,
                    size: "0",
                    status: Dropzone.ADDED,
                    accepted: true
                };
                myDropzone.emit("addedfile", file);
                myDropzone.emit("complete", file);
                myDropzone.files.push(file);

                $(file.previewElement).parent(".dropzone-items").removeClass('d-none');
                $(file.previewElement).parents(".dropzone").find("a.dropzone-select").contents().filter(function () {
                    return this.nodeType === 3; // Node type 3 is a text node
                }).remove();
                $(file.previewElement).parents(".dropzone").addClass('d-flex-file');
                $(file.previewElement).parent(".dropzone-items").addClass('d-flex-file');
                if (typeof $(file.previewElement).find('.dropzone-filename span').attr('title') == "undefined") {
                    const filenameElement = $(file.previewElement).find('.dropzone-filename span');
                    $(file.previewElement).find('.dropzone-filename span').attr('title', filenameElement.text());
                }
                adjustFileNameWidths($(file.previewElement), myDropzone);
                $(file.previewElement).find(".dropzone-toolbar").css({ "margin-left": "auto" });

                var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
                let filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
                filePath += file.name;

                $(file.previewElement).find(".dropzone-filename").attr("onclick", "ShowGridAttLink('" + filePath + "')")
            });
            //$(id).find(".fileuploadmore").removeClass("d-none");
            //$(id).find("#gridAttachCounter").removeClass("d-none");
            //$(id).find("span.spanAttCount").text(myDropzone.files.length);
        }
    });
}

$j(document).off("click", ".popover-body .dropzoneGridItemDelete").on("click", ".popover-body .dropzoneGridItemDelete", function (e) {
    const id = $(e.target).parents(".dropzone").attr("id");
    const dropzone = document.querySelector("#" + id);
    var myDropzone = Dropzone.forElement(dropzone);
    //myDropzone.removeFile(myDropzone.files[0]);
    const delFileName = $(e.target).parents(".dropzone-item").find(".dropzone-filename").text();
    const delFileObj = myDropzone.files.filter(file => file.name == delFileName)?.[0];
    if (delFileObj) {
        $("[data-bs-toggle]").popover("hide");
        //myDropzone.removeFile(delFileObj);
        tstFileDeleteConfir(myDropzone, delFileObj);
    }
});

$j(document).off("click", ".gridHeaderSwitch").on("click", ".gridHeaderSwitch", function (e) {
    let _thisDicId = $(this).attr("id");
    _thisDicId = _thisDicId.substr(7);
    if ($(this).is(":checked")) {
        if ($("#divDc" + _thisDicId + " .gridIconBtns").length > 0) {
            $("#divDc" + _thisDicId + " .griddivColumn").removeClass("d-none");
            $("#divDc" + _thisDicId + " .gridIconBtns").removeClass("d-none");
            $("#tabCollapseButton" + _thisDicId).attr("title", "Hide Dc").text('expand_less');   
        } else {
            $("#divDc" + _thisDicId).removeClass("d-none");
            $("#tabCollapseButton" + _thisDicId).attr("title", "Hide Dc").text('expand_less');   
        }
        GetDcStateValue(_thisDicId, 'T');
        setTimeout(function () {
            ExpFldsonDcExpand(_thisDicId);
        }, 0);
    } else {
        var isRecLoad = false;
        var rid = $j("#recordid000F0").val();
        if (rid != "0")
            isRecLoad = true;
        //if (rid != "0") {
        //    if ($("#divDc" + _thisDicId + " .gridIconBtns").length > 0) {
        //        $("#divDc" + _thisDicId + " .griddivColumn").addClass("d-none");
        //        $("#divDc" + _thisDicId + " .gridIconBtns").addClass("d-none");
        //    } else
        //        $("#divDc" + _thisDicId).addClass("d-none");
        //    GetDcStateValue(_thisDicId, 'F');
        //} else {
        var isExitDummy = false;
        if (gridDummyRowVal.length > 0) {
            gridDummyRowVal.map(function (v) {
                if (v.split("~")[0] == _thisDicId) isExitDummy = true;
            });
        }
        if (!isExitDummy && $(".wrapperForGridData" + _thisDicId + " table tbody tr").length > 0) {
            var cutMsg = eval(callParent('lcm[520]'));
            cutMsg = cutMsg.replace('{0}', GetDcCaption(_thisDicId));
            var glType = eval(callParent('gllangType'));
            var isRTL = false;
            if (glType == "ar")
                isRTL = true;
            else
                isRTL = false;
            var isKeyOutClick = true;
            var clearThisDCGrid = $.confirm({
                theme: 'modern',
                closeIcon: false,
                rtl: isRTL,
                title: eval(callParent('lcm[155]')),
                onContentReady: function () {
                    disableBackDrop('bind');
                },
                onDestroy: function () {
                    if (isKeyOutClick) {
                        $("#divDc" + _thisDicId + " .griddivColumn").removeClass("d-none");
                        $("#divDc" + _thisDicId + " .gridIconBtns").removeClass("d-none");
                        $("#dcBlean" + _thisDicId).prop("checked", true);
                    }
                },
                backgroundDismiss: 'false',
                escapeKey: 'buttonB',
                content: cutMsg,
                buttons: {
                    buttonA: {
                        text: eval(callParent('lcm[164]')),
                        btnClass: 'btn btn-primary',
                        action: function () {
                            isKeyOutClick = false;
                            clearThisDCGrid.close();
                            var fDcRowCount = GetDcRowCount(_thisDicId);
                            DeleteAllRows(_thisDicId, fDcRowCount);
                            ShowDimmer(false);
                            $("#chkallgridrow" + _thisDicId).prop("checked", false);
                            $("#divDc" + _thisDicId + " .griddivColumn").addClass("d-none");
                            $("#divDc" + _thisDicId + " .gridIconBtns").addClass("d-none");
                            $("#tabCollapseButton" + _thisDicId).attr("title", "Show Dc").text('expand_more');
                            GetDcStateValue(_thisDicId, 'F');
                        }
                    },
                    buttonB: {
                        text: eval(callParent('lcm[192]')),
                        btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                        action: function () {
                            //$("#divDc" + _thisDicId + " .griddivColumn").addClass("d-none");
                            //$("#divDc" + _thisDicId + " .gridIconBtns").addClass("d-none");
                            isKeyOutClick = false;
                            $("#divDc" + _thisDicId + " .griddivColumn").removeClass("d-none");
                            $("#divDc" + _thisDicId + " .gridIconBtns").removeClass("d-none");
                            $("#tabCollapseButton" + _thisDicId).attr("title", "Hide Dc").text('expand_less');   
                            $("#dcBlean" + _thisDicId).prop("checked", true);
                            disableBackDrop('destroy');
                            return;
                        }
                    },
                }
            });
        } else {
            if ($("#divDc" + _thisDicId + " .gridIconBtns").length > 0) {
                $("#divDc" + _thisDicId + " .griddivColumn").addClass("d-none");
                $("#divDc" + _thisDicId + " .gridIconBtns").addClass("d-none");
                $("#tabCollapseButton" + _thisDicId).attr("title", "Show Dc").text('expand_more');
            } else {
                if (IsFormDirty || isRecLoad) {
                    var cutMsg = eval(callParent('lcm[520]'));
                    cutMsg = cutMsg.replace('{0}', GetDcCaption(_thisDicId));
                    var glType = eval(callParent('gllangType'));
                    var isRTL = false;
                    if (glType == "ar")
                        isRTL = true;
                    else
                        isRTL = false;
                    var isKeyOutClick = true;
                    var clearThisDCGrid = $.confirm({
                        theme: 'modern',
                        closeIcon: false,
                        rtl: isRTL,
                        title: eval(callParent('lcm[155]')),
                        onContentReady: function () {
                            disableBackDrop('bind');
                        },
                        onDestroy: function () {
                            if (isKeyOutClick) {
                                $("#divDc" + _thisDicId).removeClass("d-none");
                                $("#dcBlean" + _thisDicId).prop("checked", true);
                            }
                        },
                        backgroundDismiss: 'false',
                        escapeKey: 'buttonB',
                        content: cutMsg,
                        buttons: {
                            buttonA: {
                                text: eval(callParent('lcm[164]')),
                                btnClass: 'btn btn-primary',
                                action: function () {
                                    isKeyOutClick = false;
                                    clearThisDCGrid.close();
                                    var _thisFlds = GetGridFields(_thisDicId);
                                    _thisFlds.forEach(function (_thifld) {
                                        if (_thifld != "axp_recid" + _thisDicId) {
                                            let _thisfldId = _thifld + "000F" + _thisDicId;
                                            SetFieldValue(_thisfldId, "");
                                            UpdateFieldArray(_thisfldId, 0, "", "parent", "");
                                        }
                                    });
                                    ShowDimmer(false);
                                    $("#divDc" + _thisDicId).addClass("d-none");
                                    $("#tabCollapseButton" + _thisDicId).attr("title", "Show Dc").text('expand_more');
                                    GetDcStateValue(_thisDicId, 'F');
                                }
                            },
                            buttonB: {
                                text: eval(callParent('lcm[192]')),
                                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                                action: function () {
                                    isKeyOutClick = false;
                                    //$("#divDc" + _thisDicId).addClass("d-none");
                                    $("#divDc" + _thisDicId).removeClass("d-none");
                                    $("#dcBlean" + _thisDicId).prop("checked", true);
                                    $("#tabCollapseButton" + _thisDicId).attr("title", "Hide Dc").text('expand_less');
                                    disableBackDrop('destroy');
                                    return;
                                }
                            },
                        }
                    });
                } else {
                    $("#divDc" + _thisDicId).addClass("d-none");
                    $("#tabCollapseButton" + _thisDicId).attr("title", "Show Dc").text('expand_more');
                }
            }
        }
        /*GetDcStateValue(_thisDicId, 'F');*/
        // }
    }
});

function GetDcStateValue(dcsId, expandDc) {
    try {
        if ($("#axp_dcstate000F1").length > 0) {
            let _thisIsFormDirty = IsFormDirty;
            try {
                let _fldIndex = $j.inArray("axp_dcstate", FNames);
                FFieldReadOnly[_fldIndex] = "False";
                FFieldHidden[_fldIndex] = "False";
            } catch (ex) { }
            if (dcsId == "") {
                AxpDcstateVal = $("#axp_dcstate000F1").val();
                if ($("[id^=dcBlean]").length > 0) {
                    let _axpdcval = "";// $("#axp_dcstate000F1").val();
                    if (_axpdcval == "") {
                        DCName.forEach(function () {
                            _axpdcval += "T";
                        })
                    }
                    var _arrAxpDc = Array.from(_axpdcval);
                    $("[id^=dcBlean]").each(function () {
                        if ($(this).hasClass('gridHeaderSwitch')) {
                            let _thisId = $(this).attr("id");
                            _thisId = parseInt(_thisId.substring(7));
                            if ($("#dcBlean" + _thisId).is(":checked"))
                                _arrAxpDc[_thisId - 1] = "T";
                            else
                                _arrAxpDc[_thisId - 1] = "F";
                        }
                    });
                    let _thidSwithVal = _arrAxpDc.join('');
                    let _thidFldId = $("#axp_dcstate000F1").attr("id");
                    SetFieldValue(_thidFldId, _thidSwithVal);
                    UpdateFieldArray(_thidFldId, "000", _thidSwithVal, "parent");
                    MainBlur($("#axp_dcstate000F1"));
                }
                AxExecFormControl = false;
            } else {
                let _dcSVal = $("#axp_dcstate000F1").val();
                var _arrAxpDc = Array.from(_dcSVal);
                _arrAxpDc[parseInt(dcsId) - 1] = expandDc;
                let _thidSwithVal = _arrAxpDc.join('');
                let _thidFldId = $("#axp_dcstate000F1").attr("id");
                SetFieldValue(_thidFldId, _thidSwithVal);
                UpdateFieldArray(_thidFldId, "000", _thidSwithVal, "parent");
                MainBlur($("#axp_dcstate000F1"));
            }
            if (!_thisIsFormDirty)
                IsFormDirty = false;
        }
    } catch (ex) { }
}

function GetDcStateOnLodaData(isCoptyTrans = false) {
    let _thisRid = $j("#recordid000F0").val();
    if (_thisRid != "0") {
        if ($("#axp_dcstate000F1").length > 0 && $("#axp_dcstate000F1").val() != "" && AxpDcstateVal != "") {
            var _arrAxpDc = Array.from(AxpDcstateVal);
            _arrAxpDc.forEach(function (val, ind) {
                if (ind == 0)
                    return true;
                if (val == "T")
                    AxFormControlList.push("expand~" + DCName[ind]);
                else
                    AxFormControlList.push("collapse~" + DCName[ind]);
            });
            if (AxFormControlList.length > 0)
                ProcessScriptFormControlOnList('axp_dcstate');
        }
    } else if (_thisRid == "0" && isCoptyTrans) {
        var _arrAxpDc = Array.from(AxpDcstateVal);
        _arrAxpDc.forEach(function (val, ind) {
            if (ind == 0)
                return true;
            if (val == "T")
                AxFormControlList.push("expand~" + DCName[ind]);
            else
                AxFormControlList.push("collapse~" + DCName[ind]);
        });
        if (AxFormControlList.length > 0)
            ProcessScriptFormControlOnList('axp_dcstate');
    }
}

function ExpFldsonDcExpand(_DcNo) {
    if (_DcNo != "" && !IsDcGrid(_DcNo)) {
        let _thidDcFlds = GetGridFields(_DcNo);
        _thidDcFlds.forEach(function (currField) {
            if (currField != "") {
                GetActualFieldName(currField).split(',').forEach(function (currFieldID) {
                    if (currFieldID != "") {
                        EvaluateAxFunction(currField, currFieldID);
                    }
                });
            }
        });
    }
}

$j(document).off("click", ".ckbGridStretchSwitch").on("click", ".ckbGridStretchSwitch", function (e) {
    let _thisDSId = $(this).attr("id");
    _thisDSId = _thisDSId.substr(14);//ckbGridStretch
    if ($(this).prop("checked") == true) {
        $(this).prop("checked", false);
        toggleGridStretch(_thisDSId);
        $(this).prop("checked", true);
    } else {
        $(this).prop("checked", true);
        toggleGridStretch(_thisDSId);
        $(this).prop("checked", false);
    }
});

function createAxAutocomplete(editorId) {
    let _fname = GetFieldsName(editorId);
    let _fldInd = GetFieldIndex(_fname);
    var _autoIntelli = FFieldIntelli[_fldInd];
    if (_fldInd != -1 && _autoIntelli == 'T' && FFieldType[_fldInd] != 'Rich Text' && !_fname.toLowerCase().startsWith('rtf_')) {
        let _keyName = '';
        let _keyCaption = '';
        let _intelliFlag = '';
        let _finalJson = [];
        try {
            tstConfigurations.config.forEach(function (val) {
                var ret = {};
                $.map(val, function (value, key) {
                    ret[key.toLowerCase()] = value;
                });
                _finalJson.push(ret);
            });
        } catch (ex) { }
        let _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield == _fname);
        if (_formLoadFlag.length > 0) {
            _intelliFlag = _formLoadFlag[0].propsval;
        } else {
            _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield == "");
            if (_formLoadFlag.length > 0) {
                _intelliFlag = _formLoadFlag[0].propsval;
            }
        }
        var tribute = new Tribute({
            requireLeadingSpace: false,
            trigger: '{',
            lookup: function (item) {
                return item[_keyName];
            },
            itemClass: 'd-table-row',
            noMatchTemplate: () => {
                showAlertDialog("error", "Intellisense data source formart not matching.");
            },
            loadingItemTemplate: '<div style="padding: 3px"><img src="../AxpImages/icons/5-1.gif" width="30",height="30"/></div>',
            selectTemplate: function (item) {
                if (typeof item === "undefined") return null;
                if (_intelliFlag == "metadata") {
                    return `{` + item.original[_keyCaption] + `[` + item.original[_keyName] + `]}`;
                } else
                    return `{` + item.original[_keyName] + `}`;
            },
            menuItemTemplate: function (item) {
                return Object.values(item.original).map(leg => `<span class="d-table-cell px-2 py-1">${leg}</span>`).join("");
            },
            values: function (text, cb) {
                let _fname = GetFieldsName(editorId);
                var apifldInd = GetFieldIndex(_fname);
                var acceptapi = FFieldAcceptApi[apifldInd];
                if (typeof acceptapi != "undefined" && acceptapi != "") {
                    let _resTstHtmlLS = resTstHtmlLS;
                    resTstHtmlLS = "";                    
                    ASB.WebService.GetAutoIntelliFromAPI(tstDataId, ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, _fname, text.trim(), "", _resTstHtmlLS, (res) => {
                        res == "SESSION_TIMEOUT" && (parent.window.location.href = "../aspx/sess.aspx");
                        if (res != "" && res.split("♠*♠").length > 1) {
                            tstDataId = res.split("♠*♠")[0];
                            res = res.split("♠*♠")[1];
                        }
                        try {
                            dataJSON = JSON.parse(res);
                            _keyName = dataJSON.fields[0].name;
                            if (_intelliFlag == "metadata")
                                _keyCaption = dataJSON.fields[1].name;
                            dataJSON.row ? cb(dataJSON.row) : cb([]);
                        }
                        catch (ex) { cb([]); }
                    }, () => { cb([]) });
                } else {
                    cb([]);
                }
            }
        });
        tribute.attach(document.getElementById(editorId));
    }
}

function createAxAutocompleteColon(editorId) {
    let _fname = GetFieldsName(editorId);
    let _fldInd = GetFieldIndex(_fname);
    var _autoIntelli = FFieldIntelli[_fldInd];
    if (_fldInd != -1 && _autoIntelli == 'T' && FFieldType[_fldInd] != 'Rich Text' && !_fname.toLowerCase().startsWith('rtf_')) {
        let _keyName = '';
        let _intelliFlag = '';
        let _finalJson = [];
        try {
            tstConfigurations.config.forEach(function (val) {
                var ret = {};
                $.map(val, function (value, key) {
                    ret[key.toLowerCase()] = value;
                });
                _finalJson.push(ret);
            });
        } catch (ex) { }
        let _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield == _fname);
        if (_formLoadFlag.length > 0) {
            _intelliFlag = _formLoadFlag[0].propsval;
        } else {
            _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield == "");
            if (_formLoadFlag.length > 0) {
                _intelliFlag = _formLoadFlag[0].propsval;
            }
        }
        var tribute = new Tribute({
            requireLeadingSpace: false,
            trigger: ':',
            lookup: function (item) {
                return item[_keyName];
            },
            itemClass: 'd-table-row',
            noMatchTemplate: () => {
                showAlertDialog("error", "Intellisense data source formart not matching.");
            },
            loadingItemTemplate: '<div style="padding: 3px"><img src="../AxpImages/icons/5-1.gif" width="30",height="30"/></div>',
            selectTemplate: function (item) {
                if (typeof item === "undefined") return null;
                if (_intelliFlag == "metadata")
                    return `:` + item.original[_keyName];
                else
                    return item.original[_keyName];
            },
            menuItemTemplate: function (item) {
                return Object.values(item.original).map(leg => `<span class="d-table-cell px-2 py-1">${leg}</span>`).join("");
            },
            values: function (text, cb) {
                let _fname = GetFieldsName(editorId);
                var apifldInd = GetFieldIndex(_fname);
                var acceptapi = FFieldAcceptApi[apifldInd];
                if (typeof acceptapi != "undefined" && acceptapi != "") {
                    let _resTstHtmlLS = resTstHtmlLS;
                    resTstHtmlLS = "";                    
                    ASB.WebService.GetAutoIntelliFromAPI(tstDataId, ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, _fname, text.trim(), "", _resTstHtmlLS, (res) => {
                        res == "SESSION_TIMEOUT" && (parent.window.location.href = "../aspx/sess.aspx");
                        if (res != "" && res.split("♠*♠").length > 1) {
                            tstDataId = res.split("♠*♠")[0];
                            res = res.split("♠*♠")[1];
                        }
                        try {
                            dataJSON = JSON.parse(res);
                            _keyName = dataJSON.fields[0].name;
                            dataJSON.row ? cb(dataJSON.row) : cb([])
                        }
                        catch (ex) { cb([]); }
                    }, () => { cb([]) });
                } else {
                    cb([]);
                }
            }
        });
        tribute.attach(document.getElementById(editorId));
    }
}

function createCKAutoComplete(event) {
    CKEDITOR.plugins.load(['autocomplete', 'textmatch', 'textwatcher'], (plugins) => {
        let _fname = GetFieldsName(event.editor.name);
        var apifldInd = GetFieldIndex(_fname);
        var acceptapi = FFieldAcceptApi[apifldInd];
        var _autoIntelli = FFieldIntelli[apifldInd];
        if (typeof acceptapi != "undefined" && acceptapi != "" && _autoIntelli == 'T') {
            let _resTstHtmlLS = resTstHtmlLS;
            resTstHtmlLS = "";
            let _intelliFlag = '';
            let _finalJson = [];
            try {
                tstConfigurations.config.forEach(function (val) {
                    var ret = {};
                    $.map(val, function (value, key) {
                        ret[key.toLowerCase()] = value;
                    });
                    _finalJson.push(ret);
                });
            } catch (ex) { }
            let _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield == _fname);
            if (_formLoadFlag.length > 0) {
                _intelliFlag = _formLoadFlag[0].propsval;
            } else {
                _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield == "");
                if (_formLoadFlag.length > 0) {
                    _intelliFlag = _formLoadFlag[0].propsval;
                }
            }
            ASB.WebService.GetAutoIntelliFromAPI(tstDataId, ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, _fname, "", "t", _resTstHtmlLS, (res) => {
                res == "SESSION_TIMEOUT" && (parent.window.location.href = "../aspx/sess.aspx");
                if (res != "" && res.split("♠*♠").length > 1) {
                    tstDataId = res.split("♠*♠")[0];
                    res = res.split("♠*♠")[1];
                }
                try {
                    dataJSON = JSON.parse(res);
                    if (dataJSON.row) {
                        let itemTemplate = `<li data-id='{${dataJSON.fields[0].name}}' class="d-table-row">${dataJSON.fields.map(key => `<span class="d-table-cell px-2 py-1">{${key.name}}</span>`).join("")}</li>`
                        let outputTemplate = `{${dataJSON.fields[0].name}}`;
                        let autocomplete = new CKEDITOR.plugins.autocomplete(event.editor, {
                            followingSpace: true,
                            textTestCallback: textTestCallback,
                            dataCallback: dataCallback,
                            itemTemplate: itemTemplate,
                            outputTemplate: outputTemplate
                        });

                        // Override default getHtmlToInsert to enable rich content output.
                        if (_intelliFlag == "metadata") {
                            let _keyCaption = dataJSON.fields[1].name;
                            autocomplete.getHtmlToInsert = function (item) {
                                return `{` + item[_keyCaption] + `[` + this.outputTemplate.output(item) + `]}`;
                            }
                        } else {
                            if (_intelliFlag == '' && (transid == "ad_fp" || transid == "a__fn" || transid == "ad_pn")) {
                                let _keyCaption = dataJSON.fields[1].name;
                                autocomplete.getHtmlToInsert = function (item) {
                                    return `{` + item[_keyCaption] + `[` + this.outputTemplate.output(item) + `]}`;
                                }
                            }
                            else {
                                autocomplete.getHtmlToInsert = function (item) {
                                    return `{` + this.outputTemplate.output(item) + `}`;
                                }
                            }
                        }
                    }
                }
                catch (ex) { }
            }, () => { cb([]) });
        } else {
            //cb([]);
        }
    });
    function textTestCallback(range) {
        if (!range.collapsed) {
            return null;
        }
        return CKEDITOR.plugins.textMatch.match(range, matchCallback);
    }
    function matchCallback(text, offset) {
        var pattern = /\{([A-z]|)*$/i,
            match = text.slice(0, offset)
                .match(pattern);

        if (!match) {
            return null;
        }

        return {
            start: match.index,
            end: offset
        };
    }
    function dataCallback(matchInfo, callback) {
        var data = [];
        var viewele = matchInfo.autocomplete.view.element.$;
        viewele.style.bottom = 'unset';
        $(viewele).addClass("cke_autocomplete_opened").html(`<li style="padding: 3px" class="loader"><img src="../AxpImages/icons/5-1.gif" width="30",height="30"/></li>`);//to add loader
        matchInfo.autocomplete.view.updatePosition(matchInfo.range);//to update position of autocompete dropdown to current cursor
        let _thisId = $(matchInfo.autocomplete.editor.element.$).attr("id");
        let _fname = GetFieldsName(_thisId);
        let _resTstHtmlLS = resTstHtmlLS;
        resTstHtmlLS = "";
        let _intelliFlag = '';
        var _finalJson = [];
        try {
            tstConfigurations.config.forEach(function (val) {
                var ret = {};
                $.map(val, function (value, key) {
                    ret[key.toLowerCase()] = value;
                });
                _finalJson.push(ret);
            });
        } catch (ex) { }
        let _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield == _fname);
        if (_formLoadFlag.length > 0) {
            _intelliFlag = _formLoadFlag[0].propsval;
        } else {
            _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield =="");
            if (_formLoadFlag.length > 0) {
                _intelliFlag = _formLoadFlag[0].propsval;
            }
        }

        ASB.WebService.GetAutoIntelliFromAPI(tstDataId, ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, _fname, "", "", _resTstHtmlLS, (res) => {
            res == "SESSION_TIMEOUT" && (parent.window.location.href = "../aspx/sess.aspx");
            if (res != "" && res.split("♠*♠").length > 1) {
                tstDataId = res.split("♠*♠")[0];
                res = res.split("♠*♠")[1];
            }
            try {
                dataJSON = JSON.parse(res);
                if (_intelliFlag == "metadata") {
                    let _keyName = dataJSON.fields[0].name;
                    let _keyCaption = dataJSON.fields[1].name;
                    if (dataJSON?.row && dataJSON.row.length) {
                        data = dataJSON.row.filter(function (item) {
                            var itemName = '{' + item[_keyCaption] + '[' + item[_keyName] + ']}';
                            return itemName.indexOf(matchInfo.query) == 0;
                        });
                    }
                } else {
                    if (_intelliFlag == '' && (transid == "ad_fp" || transid == "a__fn" || transid == "ad_pn")) {
                        let _keyName = dataJSON.fields[0].name;
                        let _keyCaption = dataJSON.fields[1].name;
                        if (dataJSON?.row && dataJSON.row.length) {
                            data = dataJSON.row.filter(function (item) {
                                var itemName = '{' + item[_keyCaption] + '[' + item[_keyName] + ']}';
                                return itemName.indexOf(matchInfo.query) == 0;
                            });
                        }
                    } else {
                        let _keyName = dataJSON.fields[0].name;
                        if (dataJSON?.row && dataJSON.row.length) {
                            data = dataJSON.row.filter(function (item) {
                                var itemName = '{' + item[_keyName] + '}';
                                return itemName.indexOf(matchInfo.query) == 0;
                            });
                        }
                    }
                }
                callback(data);

                setTimeout((viewele = matchInfo.autocomplete.view.element.$) => {
                    if (viewele.getBoundingClientRect().y + viewele.getBoundingClientRect().height > document.getElementsByClassName("tstructMainBottomFooter")[0].getBoundingClientRect().y + (typeof document.getElementsByClassName("tab-content")[0]?.getBoundingClientRect().y == "undefined" ? 0 : document.getElementsByClassName("tab-content")[0]?.getBoundingClientRect().y) && transid != "a__fn") {
                        viewele.style.bottom = (window.innerHeight - viewele.getBoundingClientRect().y + 40) + 'px';
                        viewele.style.top = 'unset';
                    }
                    else {
                        viewele.style.bottom = 'unset';
                    }
                }, 0);
            }
            catch (ex) { }
        }, () => { callback([]); });
    }
}

function createCKAutoCompleteColon(event) {
    CKEDITOR.plugins.load(['autocomplete', 'textmatch', 'textwatcher'], (plugins) => {
        let _fname = GetFieldsName(event.editor.name);
        var apifldInd = GetFieldIndex(_fname);
        var acceptapi = FFieldAcceptApi[apifldInd];
        var _autoIntelli = FFieldIntelli[apifldInd];
        if (typeof acceptapi != "undefined" && acceptapi != "" && _autoIntelli == 'T') {
            let _resTstHtmlLS = resTstHtmlLS;
            resTstHtmlLS = "";
            let _intelliFlag = '';
            let _finalJson = [];
            try {
                tstConfigurations.config.forEach(function (val) {
                    var ret = {};
                    $.map(val, function (value, key) {
                        ret[key.toLowerCase()] = value;
                    });
                    _finalJson.push(ret);
                });
            } catch (ex) { }
            let _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield == _fname);
            if (_formLoadFlag.length > 0) {
                _intelliFlag = _formLoadFlag[0].propsval;
            } else {
                _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "intellisense config" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid && item.sfield == "");
                if (_formLoadFlag.length > 0) {
                    _intelliFlag = _formLoadFlag[0].propsval;
                }
            }
            ASB.WebService.GetAutoIntelliFromAPI(tstDataId, ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, _fname, "", "t", _resTstHtmlLS, (res) => {
                res == "SESSION_TIMEOUT" && (parent.window.location.href = "../aspx/sess.aspx");
                if (res != "" && res.split("♠*♠").length > 1) {
                    tstDataId = res.split("♠*♠")[0];
                    res = res.split("♠*♠")[1];
                }
                try {
                    dataJSON = JSON.parse(res);
                    if (dataJSON.row) {
                        let itemTemplate = `<li data-id='{${dataJSON.fields[0].name}}' class="d-table-row">${dataJSON.fields.map(key => `<span class="d-table-cell px-2 py-1">{${key.name}}</span>`).join("")}</li>`
                        let outputTemplate = `{${dataJSON.fields[0].name}}`;
                        let autocomplete = new CKEDITOR.plugins.autocomplete(event.editor, {
                            followingSpace: true,
                            textTestCallback: textTestCallback,
                            dataCallback: dataCallback,
                            itemTemplate: itemTemplate,
                            outputTemplate: outputTemplate
                        });

                        // Override default getHtmlToInsert to enable rich content output.
                        autocomplete.getHtmlToInsert = function (item) {
                            if (_intelliFlag == "metadata")
                                return `:` + this.outputTemplate.output(item);
                            else {
                                if (_intelliFlag == '' && (transid == "ad_fp" || transid == "a__fn" || transid == "ad_pn"))
                                    return `:` + this.outputTemplate.output(item);
                                else
                                    return this.outputTemplate.output(item);
                            }
                        }
                    }
                }
                catch (ex) { }
            }, () => { cb([]) });
        } else {
            //cb([]);
        }
    });
    function textTestCallback(range) {
        if (!range.collapsed) {
            return null;
        }
        return CKEDITOR.plugins.textMatch.match(range, matchCallback);
    }
    function matchCallback(text, offset) {
        var pattern = /\:([A-z]|)*$/i,
            match = text.slice(0, offset)
                .match(pattern);

        if (!match) {
            return null;
        }

        return {
            start: match.index,
            end: offset
        };
    }
    function dataCallback(matchInfo, callback) {
        var data = [];
        var viewele = matchInfo.autocomplete.view.element.$;
        viewele.style.bottom = 'unset';
        $(viewele).addClass("cke_autocomplete_opened").html(`<li style="padding: 3px" class="loader"><img src="../AxpImages/icons/5-1.gif" width="30",height="30"/></li>`);//to add loader
        matchInfo.autocomplete.view.updatePosition(matchInfo.range);//to update position of autocompete dropdown to current cursor
        let _thisId = $(matchInfo.autocomplete.editor.element.$).attr("id");
        let _fname = GetFieldsName(_thisId);
        let _resTstHtmlLS = resTstHtmlLS;
        resTstHtmlLS = "";
        ASB.WebService.GetAutoIntelliFromAPI(tstDataId, ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, _fname, "", "", _resTstHtmlLS, (res) => {
            res == "SESSION_TIMEOUT" && (parent.window.location.href = "../aspx/sess.aspx");
            if (res != "" && res.split("♠*♠").length > 1) {
                tstDataId = res.split("♠*♠")[0];
                res = res.split("♠*♠")[1];
            }
            try {
                dataJSON = JSON.parse(res);
                let _keyName = dataJSON.fields[0].name;
                if (dataJSON?.row && dataJSON.row.length) {
                    data = dataJSON.row.filter(function (item) {
                        var itemName = ':' + item[_keyName] + ':';
                        return itemName.indexOf(matchInfo.query) == 0;
                    });
                }
                callback(data);

                setTimeout((viewele = matchInfo.autocomplete.view.element.$) => {
                    if (viewele.getBoundingClientRect().y + viewele.getBoundingClientRect().height > document.getElementsByClassName("tstructMainBottomFooter")[0].getBoundingClientRect().y + (typeof document.getElementsByClassName("tab-content")[0]?.getBoundingClientRect().y == "undefined" ? 0 : document.getElementsByClassName("tab-content")[0]?.getBoundingClientRect().y) && transid != "a__fn") {
                        viewele.style.bottom = (window.innerHeight - viewele.getBoundingClientRect().y + 40) + 'px';
                        viewele.style.top = 'unset';
                    }
                    else {
                        viewele.style.bottom = 'unset';
                    }
                }, 0);
            }
            catch (ex) { }
        }, () => { callback([]); });
    }
}


function tstFileDeleteConfir(myDropzone, delFileName) {
    var cutMsg = eval(callParent('lcm[47]'));
    var glType = eval(callParent('gllangType'));
    var isRTL = false;
    if (glType == "ar")
        isRTL = true;
    else
        isRTL = false;
    var RemoveFileCB = $.confirm({
        theme: 'modern',
        title: eval(callParent('lcm[155]')),
        onContentReady: function () {
            disableBackDrop('bind');
        },
        backgroundDismiss: 'false',
        rtl: isRTL,
        escapeKey: 'buttonB',
        content: cutMsg,
        buttons: {
            buttonA: {
                text: eval(callParent('lcm[164]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    RemoveFileCB.close();
                    myDropzone.removeFile(delFileName);
                }
            },
            buttonB: {
                text: eval(callParent('lcm[192]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    disableBackDrop('destroy');
                    return;
                }
            }
        }
    });
}

function tstDummyLoad(_openingTrId) {
    try {
        _savedraftKeytime = $("#hdnTstAutoSD").val();
        $("#hdnTstAutoSD").val("");
    } catch (ex) { }
    tstSessInfo = $("#hdnTstSInfo").val();
    try {
        tstDummyLoadParams = $("#hdnDummyParams").val();
        $("#hdnDummyParams").val('');
    } catch (ex) { }
    callParentNew("isdummyLoad=", "true");
    if (callParentNew("originalUri") != "") {
        resTstLoadDummy = $("#hdnTstLoadDummy").val();
        $("#hdnTstLoadDummy").val('');
        let _thsiifId = window.frameElement.id;
        if (_thsiifId == "middle1")
            callParentNew("LoadIframe(" + callParentNew("originalUri") + ")", "function");
        return;
    } else {
        let _thsiifId = window.frameElement.id;
        if ((_thsiifId == 'middle1' || _thsiifId.startsWith("axMultiiframe_")) && callParentNew("lastLoadtstId") != 'fromiview' && isServerSide == 'true') {
            let _thisActUri = window.frameElement.contentWindow.jQuery('#form1').attr('action');
            if (_thisActUri.startsWith('./'))
                _thisActUri = _thisActUri.slice(2);
            let _lastOpenUri = callParentNew("lastLoadtstId");
            if (_lastOpenUri.startsWith('./'))
                _lastOpenUri = _lastOpenUri.slice(2);
            if (_lastOpenUri != _thisActUri) {
                isServerDummyRec = '';
                let _thisURl = callParentNew("lastLoadtstId");
                let _thsiifId = window.frameElement.id;
                if (_thsiifId == "middle1")
                    callParentNew("LoadIframe(" + _thisURl + ")", "function");
                return;
            }
        } else {
            let _thisURl = window.location.href;
            //_thisURl = _thisURl.replace(isServerDummyRec, '');
            isServerDummyRec = '';
            let _LstLoadTst = callParentNew("lastLoadtstId");
            if (callParentNew("lastLoadtstId") != 'fromiview')
                _LstLoadTst = "?" + callParentNew("lastLoadtstId").split('?')[1];
            if (window.location.search.toLowerCase() != _LstLoadTst.toLowerCase() && callParentNew("lastLoadtstId") != 'fromiview' && _LstLoadTst.toLowerCase().indexOf('transid=' + _openingTrId.toLowerCase()) > -1) {
                let _thsiifId = window.frameElement.id;
                if (_thsiifId == "middle1")
                    callParentNew("LoadIframe(" + callParentNew("lastLoadtstId") + ")", "function");
            }
            else {
                resTstLoadDummy = $("#hdnTstLoadDummy").val();
                $("#hdnTstLoadDummy").val('');
                let _thsiifId = window.frameElement.id;
                if (_thsiifId == "middle1")
                    callParentNew("LoadIframe(" + _thisURl + ")", "function");
            }
            return;
        }
    }
}
function tstHtmlDummyLoad(_transid) {
    try {
        try {
            _savedraftKeytime = $("#hdnTstAutoSD").val();
            $("#hdnTstAutoSD").val("");
        } catch (ex) { }
        tstSessInfo = $("#hdnTstSInfo").val();
        try {
            tstDummyLoadParams = $("#hdnDummyParams").val();
            $("#hdnDummyParams").val('');
        } catch (ex) { }
        callParentNew("isdummyLoad=", "true");
        if ($("#hdnisHtmlLsLoad").length > 0 && $("#hdnisHtmlLsLoad").val() != "")
            isHtmlLsLoadData = $("#hdnisHtmlLsLoad").val();
        else
            isHtmlLsLoadData = "";
        let _thsiifId = window.frameElement.id;
        if ((_thsiifId == 'middle1' || _thsiifId.startsWith("axMultiiframe_")) && callParentNew("lastLoadtstId") != 'fromiview') {
            let _thisActUri = window.frameElement.contentWindow.jQuery('#form1').attr('action');
            if (_thisActUri.startsWith('./'))
                _thisActUri = _thisActUri.slice(2);
            let _lastOpenUri = callParentNew("lastLoadtstId");
            if (_lastOpenUri.startsWith('./'))
                _lastOpenUri = _lastOpenUri.slice(2);
            _thisActUri = decodeURIComponent(_thisActUri.replace(/\+/g, "%20"));
            _lastOpenUri = decodeURIComponent(_lastOpenUri.replace(/\+/g, "%20"));

            if (_lastOpenUri != _thisActUri) {
                _lastOpenUri = updateHdnbElapsTime(_lastOpenUri);
                _thisActUri = updateHdnbElapsTime(_thisActUri);
            }

            if (_transid != "" && _lastOpenUri != _thisActUri && _lastOpenUri != "../aspx/" + _thisActUri) {
                resTstLoadDummy = $("#hdnTstLoadDummy").val();
                $("#hdnTstLoadDummy").val('');
                isServerDummyRec = '';
                let _thisURl = callParentNew("lastLoadtstId");
                if (_thisURl.toLowerCase().indexOf('transid=' + _transid.toLowerCase()) > -1) {
                    let _thsiifId = window.frameElement.id;
                    if (_thsiifId == "middle1")
                        callParentNew("LoadIframe(" + _thisURl + ")", "function");
                    if (_thsiifId.startsWith("axMultiiframe_")) {
                        //do not return
                    } else
                        return;
                }
            }
        }
        resTstLoadDummy = $("#hdnTstLoadDummy").val();
        $("#hdnTstLoadDummy").val('');
        let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        var _finalTstHtml = $("#hdntstHtmlDummy").val();
        $("#hdntstHtmlDummy").val('');
        try {
            let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + _transid + "-" + appSUrl + "♥)", "function");
            if (_thisKey.length > 0) {
                for (const val of _thisKey) {
                    localStorage.removeItem(val);
                }
            }
            var _time = new Date();
            var _localTime = _time.getTime();
            localStorage.setItem("tstHtml♠" + _transid + "-" + appSUrl + "♥" + _localTime, _finalTstHtml);
        } catch (ex) {
            if (ex.message.indexOf('exceeded the quota') > -1) {
                var jsonString = JSON.stringify(_finalTstHtml);
                var sizeInBytes = new Blob([jsonString]).size;
                var _thisKeys = callParentNew("getKeysWithPrefix(tstHtml♠)", "function");
                if (_thisKeys.length > 0) {
                    var ascOrderKeys = _thisKeys
                        .filter(x => x.split('♥')[1])
                        .sort((a, b) => {
                            const numA = parseInt(a.split('♥')[1], 10);
                            const numB = parseInt(b.split('♥')[1], 10);
                            return numA - numB;
                        });

                    var totalSpace = 10 * 1024 * 1024;
                    for (const val of ascOrderKeys) {
                        localStorage.removeItem(val);
                        var _usedMemory = getUsedLocalStorageSpace();
                        if ((totalSpace - _usedMemory) > sizeInBytes) {
                            break;
                        }
                    }
                    try {
                        var _ttime = new Date();
                        let _tlocalTime = _ttime.getTime();
                        localStorage.setItem("tstHtml♠" + _transid + "-" + appSUrl + "♥" + _tlocalTime, _finalTstHtml);
                    } catch (ex) { }
                }
            }
        }
        callParentNew("originalContent=", "");
        //if (callParentNew("originaltrIds").filter(x => x == _transid).length == 0)
        //    callParentNew("originaltrIds").push(_transid);
        try {
            let appSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            let storedKey = 'originaltrIds-' + appSessUrl;
            let transidArray = JSON.parse(localStorage.getItem(storedKey) || '[]');
            if (!transidArray.includes(_transid)) {
                transidArray.push(_transid);
                localStorage.setItem(storedKey, JSON.stringify(transidArray));
            }
        } catch (ex) { }

    } catch (ex) { }
    let _thisURl = window.location.href;
    _thisURl = _thisURl.replace(isServerDummyRec, '');
    isServerDummyRec = '';
    let _thsiifId = window.frameElement.id;
    if (_thsiifId == "middle1")
        callParentNew("LoadIframe(" + _thisURl + ")", "function");
    return;
}

function updateHdnbElapsTime(url) {
    const urlParts = url.split("?");
    if (urlParts.length < 2) return url;
    let base = urlParts[0];
    let query = urlParts[1];
    const params = new URLSearchParams(query);
    params.set("hdnbElapsTime", "0");
    return base + "?" + params.toString();
}

function GetUriFormCurrentUri(_ThisSrc, _OpenedTrId) {

    let _oTransID = decodeURI(_ThisSrc)
        .split('?')[1]
        .split('&')
        .map(param => param.split('='))
        .reduce((values, [key, value]) => {
            if (key.toLowerCase() == 'transid') {
                values[key] = value
            }
            return values
        }, {});

    if (_oTransID.transid != _OpenedTrId) {
        _ThisSrc = _ThisSrc.replace('transid=' + _oTransID.transid, 'transid=' + _OpenedTrId);
    }
    return _ThisSrc;
}

function ExcelImportGridCall(xmlOutput, xlDcNo, isDataExist) {
    GetCurrentTime("Tstruct Process ImportGrid Ok button click(ws call)");
    if (gridDummyRowVal.length > 0) {
        gridDummyRowVal.map(function (v) {
            if (v.split("~")[0] == xlDcNo)
                gridDummyRowVal.splice($.inArray(v, gridDummyRowVal), 1);
            gridRowEditOnLoad = false;
        });
    }
    ShowDimmer(true);
    AxWaitCursor(true);
    data = "<GridList>" + xmlOutput + "</GridList>";
    if (data != "<GridList></GridList>") {
        try {
            fillDcname = xlDcNo;
            changeFillGridDc = xlDcNo;
            callBackFunDtls = "ExcelImportValues♠" + xlDcNo + "♠GetExcelImportValues";
            GetProcessTime();
            ASB.WebService.GetExcelImportValues(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, xlDcNo, data, tstDataId, resTstHtmlLS, isDataExist, SuccGetResultValue, OnException);
        } catch (ex) {
            AxWaitCursor(false);
            ShowDimmer(false);
            if (dialogObj) {
                dialogObj.dialog("close");
                $("#wrapperForMainNewData", window.parent.document).show();
            }
            var execMess = ex.name + "^♠^" + ex.message;
            showAlertDialog("error", 2030, "client", execMess);
            UpdateExceptionMessageInET("DoGetFillGridWS exception : " + ex.message);
            GetProcessTime();
            GetTotalElapsTime();
        }
    } else {
        ShowDimmer(false);
        AxWaitCursor(false);
        if (dialogObj) {
            dialogObj.dialog("close");
            $("#wrapperForMainNewData", window.parent.document).show();
        }
        GetProcessTime();
        GetTotalElapsTime();
    }
}

function tstScrollTabs(direction) {
    const tabContainer = document.querySelector('.scrollable-tabs');
    const tabItems = document.querySelectorAll('.scrollable-tabs .nav-item');
    if (tabItems.length === 0) return;
    const tabWidth = tabItems[0].getBoundingClientRect().width;
    if (direction === 'left') {
        tabContainer.scrollBy({ left: -tabWidth, behavior: 'smooth' });
    } else {
        tabContainer.scrollBy({ left: tabWidth, behavior: 'smooth' });
    }

    tabItems.forEach(tab => {
        if (!tab.classList.contains('bg-white')) {
            tab.classList.add('bg-light');
        }
    });
}

function tabedDcSliderHtml() {
    try {
        if (typeof isWizardTstruct != "undefined" && !isWizardTstruct && !isMobile) {
            let tabItems = $('#myTab li');
            let totalWidth = 0;
            tabItems.each(function () {
                totalWidth += $(this).outerWidth(true);
            });
            let screenWidth = $(window).outerWidth();
            if (totalWidth > screenWidth) {
                if ($(".tab-slider-container").length > 0) {
                    $(".tab-slider-container #tab-left-arrow,.tab-slider-container #tab-right-arrow").removeClass('d-none');
                }
                let leftArrowWidth = $('#tab-left-arrow').outerWidth(true) || 35;
                let rightArrowWidth = $('#tab-right-arrow').outerWidth(true) || 35;
                let availableWidth = screenWidth - (leftArrowWidth + rightArrowWidth);
                if (totalWidth > availableWidth) {
                    if ($(".tab-slider-container").length > 0) {
                        $(".scrollable-tabs").addClass('overflow-hidden');
                        $(".scrollable-tabs").css({ "max-width": "" + availableWidth + "px", "white-space": "nowrap" });
                    } else {
                        var currentMyTabHtml = $("#myTab").prop('outerHTML');
                        $("#myTab").replaceWith(`
            <div class="tab-slider-container position-relative">
                <div class="scrollable-tabs overflow-hidden" style="max-width: ${availableWidth}px; white-space: nowrap;">
                    ${currentMyTabHtml}
                </div>
                <button id="tab-left-arrow" class="slider-arrow left-arrow" onclick="tstScrollTabs('left')"><span class="material-icons material-icons-style material-icons-2 pt-1">chevron_left</span></button>
                <button id="tab-right-arrow" class="slider-arrow right-arrow" onclick="tstScrollTabs('right')"><span class="material-icons material-icons-style material-icons-2 pt-1">chevron_right</span></button>
            </div>
            `);
                    }
                }
            } else {
                if ($(".scrollable-tabs.overflow-hidden").length > 0) {
                    $(".scrollable-tabs.overflow-hidden").removeAttr('style');
                    $(".scrollable-tabs.overflow-hidden").removeClass('overflow-hidden');
                    $(".tab-slider-container #tab-left-arrow,.tab-slider-container #tab-right-arrow").addClass('d-none');
                }
            }
        }
    } catch (ex) { }
}

function tabedDcSliderHtmlNew() {
    setTimeout(function () {
        setTimeout(function () {
            try {
                if (typeof isWizardTstruct != "undefined" && !isWizardTstruct && !isMobile) {
                    let tabItems = $('#myTab li');
                    let totalWidth = 0;
                    tabItems.each(function () {
                        totalWidth += $(this).outerWidth(true);
                    });
                    let screenWidth = $(window).outerWidth();
                    if (totalWidth > screenWidth) {
                        if ($(".tab-slider-container").length > 0) {
                            $(".tab-slider-container #tab-left-arrow,.tab-slider-container #tab-right-arrow").removeClass('d-none');
                        }
                        let leftArrowWidth = $('#tab-left-arrow').outerWidth(true) + 2 || 40;
                        let rightArrowWidth = $('#tab-right-arrow').outerWidth(true) + 2 || 40;
                        let availableWidth = screenWidth - (leftArrowWidth + rightArrowWidth);
                        if (totalWidth > availableWidth) {
                            if ($(".tab-slider-container").length > 0) {
                                $(".scrollable-tabs").addClass('overflow-hidden');
                                $(".scrollable-tabs").css({ "max-width": "" + availableWidth + "px", "white-space": "nowrap" });
                                $(".tab-slider-container #tab-left-arrow,.tab-slider-container #tab-right-arrow").removeClass('d-none');
                            } else {
                                var currentMyTabHtml = $("#myTab").prop('outerHTML');
                                $("#myTab").replaceWith(`
            <div class="tab-slider-container position-relative">
                <div class="scrollable-tabs overflow-hidden" style="max-width: ${availableWidth}px; white-space: nowrap;">
                    ${currentMyTabHtml}
                </div>
                <button id="tab-left-arrow" class="slider-arrow left-arrow" onclick="tstScrollTabs('left')"><span class="material-icons material-icons-style material-icons-2 pt-1">chevron_left</span></button>
                <button id="tab-right-arrow" class="slider-arrow right-arrow" onclick="tstScrollTabs('right')"><span class="material-icons material-icons-style material-icons-2 pt-1">chevron_right</span></button>
            </div>
            `);
                            }
                        }
                    } else {
                        if ($(".scrollable-tabs.overflow-hidden").length > 0) {
                            $(".scrollable-tabs.overflow-hidden").removeAttr('style');
                            $(".scrollable-tabs.overflow-hidden").removeClass('overflow-hidden');
                            $(".tab-slider-container #tab-left-arrow,.tab-slider-container #tab-right-arrow").addClass('d-none');
                        }
                    }
                }
            } catch (ex) { }
        }, 0);
    }, 300);
}

$(document).on("click", ".tstbtnSqlConsole", function (event) {
    var dwbbtnid = $(this).attr("data-id");
    createPopup("AxDBScript.aspx?editorid=" + dwbbtnid, true, undefined, undefined, () => {
        $(callParentNew("Bottomnavigationbar", "class")).addClass("hide");
    }, () => {
        $(callParentNew("Bottomnavigationbar", "class")).removeClass("hide");
    });
});

$(document).on("mousedown", "#wbdrHtml", function (event) {
    if (event.which == 3 && theMode == "design" && (designObj[0].dcLayout == null || designObj[0].dcLayout == "default")) {
        if (typeof $(event.target).attr("id") == "undefined" && $(event.target).parent().find(".formLabel").length > 0) {
            let lblid = $(event.target).parent().find(".formLabel").attr("data-id");
            $("#designContextMenuLabelEdit").remove();
            $("#wbdrHtml").append(`<div id="designContextMenuLabelEdit" class="designContextMenuLabelEdit"><ul><li onclick="ChangeFontAndColor('` + lblid + `','label');"><a href="javascript:void(0);">Font</a></li>
                        <li onclick="LabelHyperlink('`+ lblid + `','label');"><a href="javascript:void(0);">Hyperlink</a></li>
                        <li onclick="RemoveLabelHyperlink('`+ lblid + `');"><a href="javascript:void(0);">Remove</a></li>
                        <li onclick="AddFormLabel('`+ lblid + `');"><a href="javascript:void(0);">Edit</a></li></ul></div>`);
            let offSetY = $(event.target).parent().find(".formLabel").offset().top + $('#heightframe').scrollTop();
            let offSetX = $(event.target).parent().find(".formLabel").offset().left + event.offsetX;
            $("#designContextMenu").css({ 'display': 'none' });
            $("#designContextMenuLabelEdit").css({ 'display': 'block', 'left': offSetX + 'px', 'top': offSetY + "px" });
        }
        else if (typeof $(event.target).attr("id") == "undefined" && $(event.target).parent().find(".tstformbutton").length > 0) {
            let lblid = $(event.target).parent().find(".tstformbutton").attr("id");
            $("#designContextMenuLabelEdit").remove();
            $("#wbdrHtml").append(`<div id="designContextMenuLabelEdit" class="designContextMenuLabelEdit"><ul><li onclick="ChangeFontAndColor('` + lblid + `','button');"><a href="javascript:void(0);">Font</a></li>
                       </ul></div>`);
            let offSetY = $(event.target).parent().find(".tstformbutton").offset().top + ($('#heightframe').scrollTop() == 0 ? -25 : $('#heightframe').scrollTop());
            let offSetX = $(event.target).parent().find(".tstformbutton").offset().left + event.offsetX;
            $("#designContextMenu").css({ 'display': 'none' });
            $("#designContextMenuLabelEdit").css({ 'display': 'block', 'left': offSetX + 'px', 'top': offSetY + "px" });
        }
        else if (typeof $(event.target).attr("id") == "undefined" && $(event.target).parent().find("textarea.form-control,input.form-control,input.multiFldChk,input[type=checkbox]").length > 0) {
            let lblid = "";
            lblid = $(event.target).parent().find("textarea.form-control,input.form-control,input[type=checkbox]").attr("id");
            if (typeof lblid == "undefined" || lblid == "")
                lblid = $(event.target).parent().find("input.multiFldChk").attr("id");
            $("#designContextMenuLabelEdit").remove();
            $("#wbdrHtml").append(`<div id="designContextMenuLabelEdit" class="designContextMenuLabelEdit"><ul><li onclick="ChangeFontAndColor('` + lblid + `','field');"><a href="javascript:void(0);">Font</a></li>
                       <li onclick="LabelHyperlink('`+ lblid + `','field');"><a href="javascript:void(0);">Hyperlink</a></li></ul></div>`);
            let offSetY = 0;
            if ($(event.target).parent().find("textarea.form-control,input.form-control,input[type=checkbox]").offset().top > 0)
                offSetY = $(event.target).parent().find("textarea.form-control,input.form-control,input[type=checkbox]").offset().top + ($('#heightframe').scrollTop() == 0 ? -25 : $('#heightframe').scrollTop());
            else
                offSetY = $(event.target).parent().find("input.multiFldChk").offset().top + ($('#heightframe').scrollTop() == 0 ? -25 : $('#heightframe').scrollTop());
            let offSetX = $(event.target).parent().find("textarea.form-control,input.form-control,input.multiFldChk,input[type=checkbox]").offset().left + event.offsetX;
            $("#designContextMenu").css({ 'display': 'none' });
            $("#designContextMenuLabelEdit").css({ 'display': 'block', 'left': offSetX + 'px', 'top': offSetY + "px" });
        }
        else {
            let offSetY = event.offsetY;
            offSetY = (event.pageY - $(this).offset().top);
            $("#designContextMenu").remove();
            $("#wbdrHtml").append(`<div id="designContextMenu" class="designContextMenu"><ul><li onclick="AddFormLabel();"><a href="javascript:void(0);">Add Label</a></li></ul></div>`);

            if ($(event.target).hasClass("grid-stack-item-content ui-draggable-handle")) {
                let trdvId = $(event.target).parent().find('.form-group').attr("id");
                if (typeof trdvId != "undefined" && trdvId != "") {
                    trdvId = trdvId.slice(2);
                    $('#designContextMenu').attr('data-afFld', trdvId);
                }
                else
                    $('#designContextMenu').attr('data-afFld', '');
                offSetY = $(event.target).offset().top + $('#heightframe').scrollTop();
            }
            else if (typeof $(event.target).attr("id") != "undefined" && !$(event.target).attr("id").startsWith("divDc")) {
                let trdvId = $(event.target).parent().find('.form-group').attr("id");
                if (typeof trdvId != "undefined" && trdvId != "") {
                    trdvId = trdvId.slice(2);
                    $('#designContextMenu').attr('data-afFld', trdvId);
                }
                else
                    $('#designContextMenu').attr('data-afFld', '');
            }
            else
                $('#designContextMenu').attr('data-afFld', $(event.target).attr("id"));
            $("#designContextMenuLabelEdit").css({ 'display': 'none' });
            $("#designContextMenu").css({ 'display': 'block', 'left': event.offsetX + 'px', 'top': offSetY + "px" });
        }
        document.oncontextmenu = function (e) {
            e.preventDefault();
        }
    }
});

$(document).on({
    click: function (event) {
        $("#designContextMenu").css({ 'display': 'none' });
        $("#designContextMenuLabelEdit").css({ 'display': 'none' });

        if (typeof event.target.title != "undefined" && event.target.title != "" && event.target.title == "Options")
        {
            if (AxpTstButtonStyle == "classic") {
                var _menu = $('.menu.initialized.show').not(".dvbtnAppsDraft");
                if (_menu.length > 0 && _menu.children(".menu-item.px-3").length > 0) {
                    _menu.children().wrapAll('<div class="mh-350px" style="overflow-x:hidden;"></div>');
                }
            }
            if (AxpTstButtonStyle == "modern") {
                if ($("#btnAppsHeader").next(".menu").find(".card .row .col-4:not(.d-none)").length == 0) {
                    $(".menu.menu-sub.menu-sub-dropdown.menu-column.w-100.w-sm-350px.initialized.show").children().wrapAll('<div class="card"><div class="card-body py-5"><div class="mh-450px scroll-y me-n5 pe-5"><div class="row g-2"></div></div></div></div>');
                }
            }
        }
    }
});

function DropzoneInitFormGridOnLoad(dvId) {
    let _thisDiv;
    if (typeof dvId != "undefined") {
        _thisDiv = $(dvId + " .dropzone:not(.dropzoneGrid)");
    } else {
        _thisDiv = $("[id^='divDc']:not(:has(.editWrapTr)) .dropzone:not(.dropzoneGrid)");// $("#divDc1 .dropzone");
    }
    _thisDiv.each(function () {
        if (($(this).parents('.editWrapTr').length > 0 && axInlineGridEdit) || ((!axInlineGridEdit && AxpGridForm == "form" && !$(this).parents('.editWrapTr').length > 0) && $(this).parents('.editWrapTr').length != 0))
            return;
        const id = "#" + $(this).attr("id");
        const fuName = $(id).attr("id").substr(9);
        funame = fuName.substring(0, fuName.lastIndexOf("F") - 3);
        var _ffuIndex = $j.inArray(funame, FNames);
        let isthisReadOnly = false;
        if (FFieldReadOnly[_ffuIndex] == "True") {
            let _thisFldId = $(id).attr("id").substr(9);
            $("#" + _thisFldId).prop('disabled', true);
            $(id).parent().addClass('readonly');
            isthisReadOnly = true;
        }

        let fuInpId = $(id).attr("id").substr(9);
        const dropzone = document.querySelector(id);
        var myDropzone = Dropzone.forElement(dropzone);

        let fuInpVal = typeof $("textarea#" + fuInpId).val() == "undefined" ? $("#" + fuInpId).val() : $("textarea#" + fuInpId).val();
        if (fuInpVal != "") {
            let axpFName = fuInpId.substr(7);
            var axpPath = "";
            if (typeof $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0] != "undefined" && $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0].value != "")
                axpPath = $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0].value;
            else
                axpPath = $("input[id*=" + axpFName + "]").not(".axpAttach,.grdAttach").length > 0 ? $("input[id*=" + axpFName + "]").not(".axpAttach,.grdAttach")[0].value : "";
            $.each(fuInpVal.split(','), function (i, val) {
                let _fullval = val;
                if (_fullval == "")
                    return;
                if (axpPath != "" && axpPath.endsWith("\\*"))
                    val = val.substr(20);
                var file = {
                    name: _fullval,
                    size: "0",
                    status: Dropzone.ADDED,
                    accepted: true
                };
                myDropzone.emit("addedfile", file);
                myDropzone.emit("complete", file);
                myDropzone.files.push(file);


                $(file.previewElement).parent(".dropzone-items").removeClass('d-none');
                $(file.previewElement).parents(".dropzone").find("a.dropzone-select").contents().filter(function () {
                    return this.nodeType === 3; // Node type 3 is a text node
                }).remove();
                $(file.previewElement).parents(".dropzone").addClass('d-flex-file');
                $(file.previewElement).parent(".dropzone-items").addClass('d-flex-file');
                if (typeof $(file.previewElement).find('.dropzone-filename span').attr('title') == "undefined") {
                    const filenameElement = $(file.previewElement).find('.dropzone-filename span');
                    $(file.previewElement).find('.dropzone-filename span').attr('title', filenameElement.text());
                }
                adjustFileNameWidths($(file.previewElement), myDropzone);
                $(file.previewElement).find(".dropzone-toolbar").css({ "margin-left": "auto" });

                var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
                let filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
                filePath += _fullval;

                $(file.previewElement).find(".dropzone-filename span").text(val);
                $(file.previewElement).find(".dropzone-filename span").attr("data-fullfile", _fullval);
                $(file.previewElement).find(".dropzone-filename").attr("onclick", "ShowAxpFileuploadLink('" + $(myDropzone.files[0].previewElement).parents(".dropzone").attr("id").substr(9) + "','" + _fullval + "','" + filePath + "',event)");
                if (isthisReadOnly)
                    $(file.previewElement).find(".dropzoneItemDelete").addClass('d-none');
            });
        }
    });
}

function DropzoneGridInitFormGridOnLoad(dvId) {
    let _thisDiv;
    if (typeof dvId != "undefined") {
        _thisDiv = $(dvId + " .dropzoneGrid");
    } else {
        _thisDiv = $("[id^='divDc']:not(:has(.editWrapTr)) .dropzoneGrid");
    }
    _thisDiv.each(function () {
        if ($(this).parents('.editWrapTr').length > 0 && !isMobile && AxpGridForm != 'form')
            return;
        const id = "#" + $(this).attr("id");
        let fuInpId = $(id).attr("id").substr(9);
        const dropzone = document.querySelector(id);
        var myDropzone = Dropzone.forElement(dropzone);
        let fuInpVal = $("#" + fuInpId).val();
        if (fuInpVal != "") {
            $.each(fuInpVal.split(','), function (i, val) {
                var file = {
                    name: val,
                    size: "0",
                    status: Dropzone.ADDED,
                    accepted: true
                };
                myDropzone.emit("addedfile", file);
                myDropzone.emit("complete", file);
                myDropzone.files.push(file);

                $(file.previewElement).parent(".dropzone-items").removeClass('d-none');
                $(file.previewElement).parents(".dropzone").find("a.dropzone-select").contents().filter(function () {
                    return this.nodeType === 3; // Node type 3 is a text node
                }).remove();
                $(file.previewElement).parents(".dropzone").addClass('d-flex-file');
                $(file.previewElement).parent(".dropzone-items").addClass('d-flex-file');
                if (typeof $(file.previewElement).find('.dropzone-filename span').attr('title') == "undefined") {
                    const filenameElement = $(file.previewElement).find('.dropzone-filename span');
                    $(file.previewElement).find('.dropzone-filename span').attr('title', filenameElement.text());
                }
                adjustFileNameWidths($(file.previewElement), myDropzone);
                $(file.previewElement).find(".dropzone-toolbar").css({ "margin-left": "auto" });

                var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
                let filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
                filePath += file.name;

                $(file.previewElement).find(".dropzone-filename").attr("onclick", "ShowGridAttLink('" + filePath + "')")
            });
        }
    });
}
function setExtraBtnTooltip() {
    document.querySelectorAll('[data-extra]').forEach(el => {
        el.addEventListener('mouseenter', () => {
            el.dataset.originalTitle = el.getAttribute('title') || '';
            el.setAttribute('title', el.getAttribute('data-extra') || '');
        });
        el.addEventListener('mouseleave', () => {
            el.setAttribute('title', el.dataset.originalTitle || '');
        });
    });
}
